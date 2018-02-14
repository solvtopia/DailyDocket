Imports DailyDocket.CommonCore.Shared.Common
Imports DailyDocket.Builder.Common
Imports Telerik.Web.UI

Public Class Preview
    Inherits builderPage

#Region "Properties"

    Private Property AttorneyId As Integer
        Get
            If Session("PreviewAttorneyId") Is Nothing Then Session("PreviewAttorneyId") = 0
            Return CInt(Session("PreviewAttorneyId"))
        End Get
        Set(value As Integer)
            Session("PreviewAttorneyId") = value
        End Set
    End Property

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.lblMessage.Text = ""
        Me.Master.UserInfoPanel.Visible = False
        Me.Master.TopRightPanel.Visible = False
        Me.Master.Breadcrumbs.Visible = (Not Me.OnMobile)

        Me.pnlStep2.Visible = False

        If Not IsPostBack Then
            Me.txtBarNumber.Focus()
            Me.SetProfitData()
        End If

        Me.LoadAdminData()
        Me.LoadAttorneyCases()

        If Me.OnPhone Then
            Me.radScheduler.SelectedView = SchedulerViewType.AgendaView
            Me.radScheduler.ShowViewTabs = False
            Me.radScheduler.Visible = False
            Me.RadSearchGridMobile.Visible = True
            Me.pnlDates.Visible = False
        Else
            Me.radScheduler.SelectedView = SchedulerViewType.AgendaView
            Me.radScheduler.ShowViewTabs = True
            Me.radScheduler.Visible = True
            Me.RadSearchGridMobile.Visible = False
            Me.pnlDates.Visible = True
        End If
    End Sub

    Protected Sub btnFind_Click(sender As Object, e As EventArgs) Handles btnFind.Click
        Me.LoadAttorneyCases()
    End Sub

    Private Sub LoadAdminData()
        Dim cn As New SqlClient.SqlConnection(ConnectionString)

        Try
            Dim monthStart As New Date(Now.Year, Now.Month, 1)
            Dim cmd As New SqlClient.SqlCommand("SELECT Count(ID) AS [TotalCount] FROM [FileRecords] WHERE [dtInserted] >= '" & monthStart.ToString & "';", cn)
            If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
            Dim rs As SqlClient.SqlDataReader = cmd.ExecuteReader
            If rs.Read Then
                Me.lblTotalCases.Text = FormatNumber(rs("TotalCount").ToString, 0)
            End If
            cmd.Cancel()
            rs.Close()

            cmd = New SqlClient.SqlCommand("SELECT Count(ID) AS [TotalCount] FROM [FileRecords] WHERE [dtInserted] >= '" & Now.Date.ToString & "';", cn)
            If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
            rs = cmd.ExecuteReader
            If rs.Read Then
                Me.lblCasesToday.Text = FormatNumber(rs("TotalCount").ToString, 0)
            End If
            cmd.Cancel()
            rs.Close()

            cmd = New SqlClient.SqlCommand("SELECT Count(ID) AS [TotalCount] FROM [Attorneys];", cn)
            If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
            rs = cmd.ExecuteReader
            If rs.Read Then
                Me.lblTotalAttorneys.Text = FormatNumber(rs("TotalCount").ToString, 0)
            End If
            cmd.Cancel()
            rs.Close()

            'cmd = New SqlClient.SqlCommand("SELECT Count(ID) AS [TotalCount] FROM [Users] WHERE [ID] > 40 AND [Active] = 1 AND [xBillingLock] = 0 AND [xClientID] > 0;", cn)
            'If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
            'rs = cmd.ExecuteReader
            'If rs.Read Then
            '    Me.lblActiveUsers.Text = FormatNumber(rs("TotalCount").ToString, 0)
            'End If
            'cmd.Cancel()
            'rs.Close()

            'cmd = New SqlClient.SqlCommand("SELECT Count(ID) AS [TotalCount] FROM [Users] WHERE [ID] > 40 AND [Active] = 1 AND [xClientID] > 0;", cn)
            'If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
            'rs = cmd.ExecuteReader
            'If rs.Read Then
            '    Me.lblTotalUsers.Text = FormatNumber(rs("TotalCount").ToString, 0)
            'End If
            'cmd.Cancel()
            'rs.Close()

            'cmd = New SqlClient.SqlCommand("SELECT Count(ID) AS [TotalCount] FROM [Users] WHERE ([ID] > 40 AND [Active] = 1 AND [xClientID] > 0) AND [dtInserted] >= '" & monthStart.ToString & "';", cn)
            'If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
            'rs = cmd.ExecuteReader
            'If rs.Read Then
            '    Me.lblUsersMonth.Text = FormatNumber(rs("TotalCount").ToString, 0)
            'End If
            'cmd.Cancel()
            'rs.Close()

            'cmd = New SqlClient.SqlCommand("SELECT Sum(CASE WHEN [MobileDeviceType] LIKE '%android%' THEN 1 ELSE 0 END) AS [AndroidCount], Sum(CASE WHEN [MobileDeviceType] LIKE '%iphone%' OR [MobileDeviceType] LIKE '%ipad%' OR [MobileDeviceType] LIKE '%ipod%' THEN 1 ELSE 0 END) AS [AppleCount] FROM [vwDevices] WHERE [ID] > 40 AND [Active] = 1 AND [xClientID] > 0;", cn)
            'If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
            'rs = cmd.ExecuteReader
            'If rs.Read Then
            '    Me.lblAndroid.Text = FormatNumber(rs("AndroidCount").ToString, 0)
            '    Me.lblApple.Text = FormatNumber(rs("AppleCount").ToString, 0)
            'End If
            'cmd.Cancel()
            'rs.Close()

            cmd = New SqlClient.SqlCommand("SELECT IsNull(Sum(CASE WHEN [AttorneyID] > 0 THEN 1 ELSE 0 END),0) AS [AttorneyCount], IsNull(Sum(CASE WHEN [ClientID] > 0 THEN 1 ELSE 0 END),0) AS [ClientCount] FROM [Sys_SMSLog] WHERE [dtInserted] >= '" & monthStart.ToString & "';", cn)
            If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
            rs = cmd.ExecuteReader
            If rs.Read Then
                Me.lblAttorneySMS.Text = FormatNumber(rs("AttorneyCount").ToString, 0)
                'Me.lblClientSMS.Text = FormatNumber(rs("ClientCount").ToString, 0)
            End If
            cmd.Cancel()
            rs.Close()

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(App.CurrentUser.ID, App.CurrentClient.ID, If(Me.Page.OnMobile, Enums.ProjectName.MobileApp, Enums.ProjectName.Builder)))
        Finally
            cn.Close()
        End Try
    End Sub

    Private Sub LoadAttorneyCases()
        Dim cn As New SqlClient.SqlConnection(ConnectionString)

        Try
            If Me.txtBarNumber.Text <> "" Then
                Dim ar As New AttorneyRecord(Me.txtBarNumber.Text)

                If ar.ID > 0 Then
                    Dim civilCount, criminalCount, caseCount As Integer

                    Dim cmd As New SqlClient.SqlCommand("SELECT [CivilCount], [CriminalCount], [TotalCount] FROM [vwSalesReport] WHERE [AttorneyID] = " & ar.ID, cn)
                    If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
                    Dim rs As SqlClient.SqlDataReader = cmd.ExecuteReader
                    If rs.Read Then
                        civilCount = rs("CivilCount").ToString.ToInteger
                        criminalCount = rs("CriminalCount").ToString.ToInteger
                        caseCount = civilCount + criminalCount
                    Else caseCount = 0
                    End If
                    cmd.Cancel()
                    rs.Close()

                    If Session("auditSaved") Is Nothing OrElse Session("auditSaved").ToString.ToBoolean = False Then
                        Dim audit As New AuditLogEntry
                        audit.ActionType = "Bar Number Lookup"
                        audit.ClientID = 0
                        audit.Description = Me.txtBarNumber.Text
                        audit.IpAddress = Request.UserHostAddress
                        audit.Project = Enums.ProjectName.Builder
                        audit.UserID = ar.ID
                        audit.WriteToAuditLog
                        Session("auditSaved") = True
                    End If

                    If caseCount > 0 Then
                        Me.lblCaseCount.Text = "Great news! Based on your cases and clients, you can <strong><span style='color: #cc0000'>INCREASE BILLABLE SERVICE FEES (TEXT AND EMAIL NOTIFICATIONS) by: " & Me.lblClientFees.Text & " per month!<span></strong><br/><br/>"
                        'Me.lblCaseCount.Text &= "Below is a preview of your calendar. If you like what you see and want to get the details of your cases along with SMS or Email Notifications for both you and your clients, click on the Register link under the menu on the left."

                        If App.MyCases.Count = 0 Then App.MyCases = LoadCases(Me.Page, Me.txtBarNumber.Text, True)

                        Me.AttorneyId = ar.ID

                        If Me.OnPhone Then
                            Me.RadSearchGridMobile.DataSource = App.MyCases
                            Me.RadSearchGridMobile.DataBind()
                        Else
                            Me.radScheduler.DataSource = App.MyCases
                            Me.radScheduler.DataBind()
                        End If

                        Me.LoadDates()

                        Me.SetProfitData()

                        Me.pnlStep2.Visible = True
                    Else
                        Me.lblMessage.Text = "Oh No! It looks like we don't have any cases in our database for you.<br/><br/>"
                        Me.lblMessage.Text &= "Not to worry though ... we receive new cases every 2 hours, 24 hours a day, 7 days a week. I'm sure your name will pop up."
                    End If
                Else
                    Me.lblMessage.Text = "OOPS!! There seems to be a problem ... We couldn't find your attorney record.<br/><br/>"
                    Me.lblMessage.Text &= "Not to worry though ... we receive new cases every 2 hours, 24 hours a day, 7 days a week. I'm sure your name will pop up."
                End If
            End If

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(Enums.ProjectName.Builder))
        Finally
            cn.Close()
        End Try
    End Sub

    Private Sub SetProfitData()
        Dim oPayments As New Payments(ConfigurationManager.AppSettings("StripeMode"))
        Dim amt As Double = oPayments.GetChargeAmount(Payments.ChargeAmount.MonthlySubscription, App.CurrentUser.ID)
        If Me.txtClientCharge.Text.ToInteger = 0 Then Me.txtClientCharge.Text = FormatNumber(amt, 2)
        Me.LoadChartData()
        'Me.fraProfitChart.ContentUrl = "~/admin/ProfitChart.aspx?aid=" & Me.AttorneyId & "&charge=" & Me.txtClientCharge.Text
        'Me.RadAjaxPanel1.RaisePostBackEvent("")
    End Sub

    Private Sub LoadChartData()
        Dim cn As New SqlClient.SqlConnection(ConnectionString)

        Try
            Dim attorneyCount As Integer = 0
            Dim clientCount As Integer = 0
            If Me.AttorneyId = 0 Then
                Me.pnlChart.Visible = False
                'Dim cmd As New SqlClient.SqlCommand("SELECT [xBarNumber] FROM [Attorneys] WHERE [ID] IN (SELECT [xAttorneyID] FROM [Users] WHERE [ID] > 40 AND [Active] = 1);", cn)
                'If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
                'Dim rs As SqlClient.SqlDataReader = cmd.ExecuteReader
                'Do While rs.Read
                '    Dim lst As List(Of MyAppointment) = Common.LoadCases(Me.Page, rs("xBarNumber").ToString)

                '    Dim lstNames As New List(Of String)
                '    For Each ap As MyAppointment In lst
                '        Dim fr As New FileRecord
                '        fr = CType(fr.DeserializeFromXml(ap.XmlData), FileRecord)

                '        Dim myRole As Enums.AttorneyType = fr.AttorneyType(App.CurrentAttorney.AllNames)

                '        If myRole = Enums.AttorneyType.Plaintiff Then
                '            For Each s As String In fr.Plaintiff
                '                If s <> "" Then
                '                    If Not lstNames.Contains(s) Then lstNames.Add(s)
                '                End If
                '            Next
                '        ElseIf myRole = Enums.AttorneyType.Defendant Then
                '            For Each s As String In fr.Defendant
                '                If s <> "" Then
                '                    If Not lstNames.Contains(s) Then lstNames.Add(s)
                '                End If
                '            Next
                '        End If
                '    Next

                '    clientCount += lstNames.Count
                '    attorneyCount += 1
                'Loop
                'cmd.Cancel()
                'rs.Close()
            Else
                Me.pnlChart.Visible = True
                Dim ar As New AttorneyRecord(Me.AttorneyId)
                Dim lst As List(Of MyAppointment) = Common.LoadCases(Me.Page, ar.BarNumber, True)

                Dim lstNames As New List(Of String)
                For Each ap As MyAppointment In lst
                    Dim fr As New FileRecord
                    fr = CType(fr.DeserializeFromXml(ap.XmlData), FileRecord)

                    Dim myRole As Enums.AttorneyType = fr.AttorneyType(App.CurrentAttorney.AllNames)

                    If myRole = Enums.AttorneyType.Plaintiff Then
                        For Each s As String In fr.Plaintiff
                            If s <> "" Then
                                If Not lstNames.Contains(s) Then lstNames.Add(s)
                            End If
                        Next
                    ElseIf myRole = Enums.AttorneyType.Defendant Then
                        For Each s As String In fr.Defendant
                            If s <> "" Then
                                If Not lstNames.Contains(s) Then lstNames.Add(s)
                            End If
                        Next
                    End If
                Next

                clientCount += lstNames.Count
                attorneyCount = 1
            End If

            Dim oPayments As New Payments(ConfigurationManager.AppSettings("StripeMode"))
            Dim amt As Double = oPayments.GetChargeAmount(Payments.ChargeAmount.MonthlySubscription, App.CurrentUser.ID)

            Dim userData As New PieSeries
            userData.StartAngle = 90
            userData.LabelsAppearance.Position = HtmlChart.PieAndDonutLabelsPosition.OutsideEnd
            userData.LabelsAppearance.Visible = False '.DataFormatString = "{0:C}"
            userData.TooltipsAppearance.Visible = False '.DataFormatString = "{0:C}"

            Dim itemAttorney As New SeriesItem
            itemAttorney.Name = "Monthly Daily Docket Attorney Subscription"
            itemAttorney.YValue = amt.ToDecimal * attorneyCount
            userData.Items.Add(itemAttorney)
            Me.lblAttorneyCost.Text = FormatCurrency(amt.ToDecimal * attorneyCount, 2)

            Dim itemClients As New SeriesItem
            itemClients.Name = "Monthly Billable Client Service Fees"
            itemClients.YValue = Me.txtClientCharge.Text.ToDecimal * clientCount
            userData.Items.Add(itemClients)
            Me.lblClientFees.Text = FormatCurrency(Me.txtClientCharge.Text.ToDecimal * clientCount, 2)

            Me.RadHtmlChart1.PlotArea.Series.Add(userData)

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(App.CurrentUser.ID, App.CurrentClient.ID, Enums.ProjectName.Builder))
        Finally
            cn.Close()
        End Try
    End Sub

    Private Sub LoadDates()
        ' sort the date list
        App.DateList.Sort(Function(p1, p2) p1.Date.CompareTo(p2.Date))

        Dim x As Integer = 0

        Me.tblDates.Rows.Clear()
        Dim tr As New TableRow
        For Each dt As Date In App.DateList
            ' add the date to the table at the top of the page for quick reference
            Dim tc As New TableCell
            Dim styleString As String = "display: inline-block;"
            styleString &= " padding: 10px;"
            tc.Attributes.Add("style", styleString)
            Dim lnk As New LinkButton
            lnk.ID = "lnkDate_" & x
            lnk.CommandName = dt.ToString
            lnk.Text = dt.ToLongDateString
            AddHandler lnk.Click, AddressOf lnkDate_Click
            tc.Controls.Add(lnk)
            tr.Cells.Add(tc)

            x += 1
        Next
        Me.tblDates.Rows.Add(tr)
    End Sub

    Private Sub txtClientCharge_TextChanged(sender As Object, e As EventArgs) Handles txtClientCharge.TextChanged
        Me.SetProfitData()
    End Sub

    Private Sub lnkDate_Click(sender As Object, e As EventArgs)
        Dim lnk As LinkButton = CType(sender, LinkButton)
        Me.radScheduler.SelectedDate = CDate(lnk.CommandName)
    End Sub

    Private Sub radScheduler_AppointmentInsert(sender As Object, e As AppointmentInsertEventArgs) Handles radScheduler.AppointmentInsert
        If radScheduler.Appointments.GetAppointmentsInRange(e.Appointment.Start, e.Appointment.[End]).Count > 0 Then
            e.Cancel = True
        End If
    End Sub

    Private Sub radScheduler_AppointmentUpdate(sender As Object, e As AppointmentUpdateEventArgs) Handles radScheduler.AppointmentUpdate
        If radScheduler.Appointments.GetAppointmentsInRange(e.ModifiedAppointment.Start, e.ModifiedAppointment.[End]).Count > 0 Then
            For Each a As Appointment In radScheduler.Appointments.GetAppointmentsInRange(e.ModifiedAppointment.Start, e.ModifiedAppointment.[End])
                If a.ID IsNot e.Appointment.ID Then
                    e.Cancel = True
                End If
            Next
        End If
    End Sub

    Private Sub Preview_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
        If Not IsPostBack Then
            Dim x As Integer = 0
            For Each itm In Me.RadSearchGridMobile.MasterTableView.GetItems(GridItemType.GroupHeader)
                If x = 0 Then itm.Expanded = True Else itm.Expanded = False
                x += 1
            Next
        End If
    End Sub
End Class