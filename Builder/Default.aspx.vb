Imports Telerik.Web.UI
Imports DailyDocket.CommonCore.Shared.Common
Imports DailyDocket.Builder.Common

Public Class _Default4
    Inherits builderPage

#Region "Properties"

    Private ReadOnly Property ConflictDetected As String
        Get
            Dim retVal As String = ""

            Dim cn As New SqlClient.SqlConnection(ConnectionString)

            Try
                Dim cmd As New SqlClient.SqlCommand("procDetectConflicts", cn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.AddWithValue("@attorneyID", App.CurrentAttorney.ID)
                If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
                Dim rs As SqlClient.SqlDataReader = cmd.ExecuteReader
                Do While rs.Read
                    If CDate(rs("ConflictDate").ToString) > Now.Date Then
                        If retVal = "" Then retVal = rs("ConflictText").ToString Else retVal &= "<br/>" & rs("ConflictText").ToString
                    End If
                Loop
                cmd.Cancel()
                rs.Close()

            Catch ex As Exception
                ex.WriteToErrorLog(New ErrorLogEntry(App.CurrentUser.ID, App.CurrentClient.ID, If(Me.Page.OnMobile, Enums.ProjectName.MobileApp, Enums.ProjectName.Builder)))
            Finally
                cn.Close()
            End Try

            Return retVal
        End Get
    End Property

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If App.CurrentClient.ID > 0 And (App.MyCases Is Nothing OrElse App.MyCases.Count = 0) Then App.MyCases = LoadCases(Me.Page, App.CurrentUser.AttorneyID, False)

        Me.RecordDetails.UrlString = If(Me.DeviceId <> "", "deviceid=" & Me.DeviceId, "uid=" & Me.UserId)

        If App.CurrentUser.Permissions = Enums.SystemUserPermissions.Solvtopia Then
            ' solvtopia users get the admin dashboard
            Me.pnlAdminDB.Visible = True
            Me.pnlCalendar.Visible = False
            Me.pnlDetails.Visible = False
            Me.pnlConflict.Visible = False
            Me.pnlSalesReport.Visible = Not Me.OnPhone
            Me.btnShowCalendar.Visible = Not Me.OnMobile
            Me.pnlBadges.Visible = Not Me.OnPhone

            If Not IsPostBack Then
                Me.LoadAdminData()
                Me.pnlAttorneySearch.Visible = True
                Me.pnlFound.Visible = False
                Me.ddlState.SelectedValue = "NC"
                If Me.OnMobile Then Me.ddlSMSLogType.SelectedValue = "queue"
            End If
        Else
            ' non solvtopia users
            If Not IsPostBack Then
                If App.CurrentClient.ID > 0 Then

                    If Me.OnPhone Then
                        Me.RadSearchGridMobile.DataSource = App.MyCases
                        Me.RadSearchGridMobile.DataBind()
                    Else
                        Me.radScheduler.DataSource = App.MyCases
                        Me.radScheduler.DataBind()
                    End If
                Else
                    App.MyCases = Nothing
                    App.SelectedCase = Nothing
                    App.DateList = Nothing
                End If
            End If

            If App.CurrentClient.ID > 0 Then
                Me.LoadDates()
            End If

            If App.SelectedCase IsNot Nothing AndAlso App.SelectedCase.ID > 0 Then
                Me.radScheduler.SelectedDate = App.SelectedCase.SessionDate
                Me.RecordDetails.LoadCase()
                Me.ShowOptions()
            End If

            Me.ShowOptions()

            Me.pnlAdminDB.Visible = False
        End If

        ' everyone
        If Not IsPostBack Then
            Me.SetLogTypes()
        End If
    End Sub

    Private Sub SetLogTypes()
        Me.fraSMSLog.ContentUrl = "~/admin/SMSLog.aspx?type=" & Me.ddlSMSLogType.SelectedValue
        Me.fraProcessorLog.ContentUrl = "~/admin/ProcessorLog.aspx?type=" & Me.ddlProcessorLogType.SelectedValue
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

            cmd = New SqlClient.SqlCommand("SELECT Count(ID) AS [TotalCount] FROM [Users] WHERE ([ID] > 40 AND [Active] = 1 AND [xClientID] > 0) AND [dtInserted] >= '" & monthStart.ToString & "';", cn)
            If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
            rs = cmd.ExecuteReader
            If rs.Read Then
                Me.lblUsersMonth.Text = FormatNumber(rs("TotalCount").ToString, 0)
            End If
            cmd.Cancel()
            rs.Close()

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
                Me.lblClientSMS.Text = FormatNumber(rs("ClientCount").ToString, 0)
            End If
            cmd.Cancel()
            rs.Close()

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(App.CurrentUser.ID, App.CurrentClient.ID, If(Me.Page.OnMobile, Enums.ProjectName.MobileApp, Enums.ProjectName.Builder)))
        Finally
            cn.Close()
        End Try
    End Sub

    Private Sub CheckCounties()
        Dim cn As New SqlClient.SqlConnection(ConnectionString)

        Try
            ' get a list of counties from the cases
            Dim countyList As String = ""
            For Each c As MyAppointment In App.MyCases
                If c.Type.ToLower = "criminal" Then
                    If Not countyList.Contains(c.County.ToUpper) Then If countyList = "" Then countyList = c.County.ToUpper Else countyList &= ", " & c.County.ToUpper
                End If
            Next

            ' check the criminal counties for matching civil counties
            Dim missingCounties As String = ""
            Dim cmd As New SqlClient.SqlCommand("SELECT [CivilCount], [CriminalCount], [County] FROM [vwCountySales] WHERE [County] IN ('" & countyList & "');", cn)
            If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
            Dim rs As SqlClient.SqlDataReader = cmd.ExecuteReader
            Do While rs.Read
                If rs("CivilCount").ToString.ToInteger = 0 Then
                    If missingCounties = "" Then missingCounties = rs("County").ToString.ToUpper Else missingCounties &= ", " & rs("County").ToString.ToUpper
                End If
            Loop
            cmd.Cancel()
            rs.Close()

            Me.lblMissingCounties.Text = StrConv(missingCounties.Replace("_", " "), vbProperCase)
            Me.pnlMissingCounties.Visible = (Me.lblMissingCounties.Text <> "")

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(App.CurrentUser.ID, App.CurrentClient.ID, If(Me.Page.OnMobile, Enums.ProjectName.MobileApp, Enums.ProjectName.Builder)))
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
            If dt >= Now.Date Then
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
            End If
        Next
        Me.tblDates.Rows.Add(tr)
    End Sub

    Private Sub lnkClient_Click(sender As Object, e As EventArgs)
        Dim lnk As LinkButton = CType(sender, LinkButton)

        Dim usrString As String = If(Me.DeviceId <> "", "deviceid=" & Me.DeviceId, "uid=" & Me.UserId)
        Response.Redirect("~/account/ClientProfile.aspx?name=" & lnk.CommandName & "&" & usrString, False)
    End Sub

    Private Sub lnkDate_Click(sender As Object, e As EventArgs)
        Dim lnk As LinkButton = CType(sender, LinkButton)
        Me.radScheduler.SelectedDate = CDate(lnk.CommandName)
    End Sub

    Private Sub ShowOptions()
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

        Me.pnlDetails.Visible = (Not Me.OnMobile And (App.SelectedCase IsNot Nothing AndAlso App.SelectedCase.ID > 0))

        If Not IsPostBack Then
            Me.lblConflict.Text = Me.ConflictDetected
            Me.pnlConflict.Visible = (Me.lblConflict.Text <> "")

            Me.pnlDemo.Visible = (App.CurrentClient.DemoDays <= App.CurrentClient.DemoDuration And Not App.CurrentClient.DemoStartDate = New Date(2099, 12, 31))
            Select Case App.CurrentClient.DemoDays
                Case 1
                    Me.lblDemoMessage.Text = "You only have 1 day of your FREE " & App.CurrentClient.DemoDuration & " Day Demo remaining."
                Case CInt(Math.Abs(App.CurrentClient.DemoDuration / 2))
                    Me.lblDemoMessage.Text = "You're half way through your FREE " & App.CurrentClient.DemoDuration & " Day Demo."
                Case Else
                    Me.lblDemoMessage.Text = "You have " & App.CurrentClient.DemoDays & " days of your FREE " & App.CurrentClient.DemoDuration & " Day Demo remaining."
            End Select

            Me.CheckCounties()
        End If
    End Sub

    Private Sub radScheduler_AppointmentClick(sender As Object, e As SchedulerEventArgs) Handles radScheduler.AppointmentClick
        App.SelectedCase = New FileRecord(e.Appointment.ID.ToString.ToInteger)

        If Me.OnMobile Then
            Dim usrString As String = If(Me.DeviceId <> "", "deviceid=" & Me.DeviceId, "uid=" & Me.UserId)
            Response.Redirect("~/account/CaseDetails.aspx?" & usrString, False)
        Else
            Me.RecordDetails.LoadCase()
            Me.ShowOptions()
        End If
    End Sub

    Private Sub RadSearchGridMobile_SelectedIndexChanged(sender As Object, e As EventArgs) Handles RadSearchGridMobile.SelectedIndexChanged
        If Me.RadSearchGridMobile.SelectedItems.Count > 0 Then
            Dim dataItem As GridItem = Me.RadSearchGridMobile.SelectedItems(0)

            App.SelectedCase = New FileRecord(dataItem.Cells(4).Text.ToInteger)

            If Me.OnMobile Then
                Dim usrString As String = If(Me.DeviceId <> "", "deviceid=" & Me.DeviceId, "uid=" & Me.UserId)
                Response.Redirect("~/account/CaseDetails.aspx?" & usrString, False)
            Else
                Me.RecordDetails.LoadCase()
                Me.ShowOptions()
            End If
        End If
    End Sub

    Private Sub RadSearchGridMobile_ItemDataBound(sender As Object, e As GridItemEventArgs) Handles RadSearchGridMobile.ItemDataBound
        Try
            If TypeOf e.Item Is GridItem Then
                Dim dataItem As GridItem = e.Item
                If dataItem.Cells.Count > 4 Then
                    Dim xData As String = dataItem.Cells(5).Text
                    If xData.Contains("-") Then

                    End If
                End If
                '    If IsNumeric(dataItem.Cells(4).Text) Then
                '        dataItem.BackColor = GetColor("#27AAD0")
                '    End If
            End If
        Catch ex As Exception
        End Try
    End Sub

    Protected Sub ibtnRefresh_Click(sender As Object, e As ImageClickEventArgs) Handles ibtnRefresh.Click
        App.MyCases = Nothing
        App.SelectedCase = Nothing
        App.DateList = Nothing

        LoadCases(Me.Page, App.CurrentUser.AttorneyID, False)
        Me.LoadDates()

        Me.ShowOptions()
    End Sub

    Private Sub btnFind_Click(sender As Object, e As EventArgs) Handles btnFind.Click
        Dim cn As New SqlClient.SqlConnection(ConnectionString)

        Try
            Dim ar As New AttorneyRecord
            Dim lar As New AttorneyRecord

            If Me.txtBarNumber.Text <> "" Then
                ar = New AttorneyRecord(Me.txtBarNumber.Text)
            Else
                'LookupAttorney(Me.txtBarNumber.Text, Me.txtLastName.Text.ToUpper & "," & Me.txtFirstName.Text.ToUpper, Me.ddlState.SelectedValue)
                If Me.txtBarNumber.Text <> "" Then
                    lar = New AttorneyRecord(Me.txtBarNumber.Text)
                Else lar = LookupAttorney("", Me.txtLastName.Text.ToUpper & "," & Me.txtFirstName.Text.ToUpper, Me.ddlState.SelectedValue)
                End If
                If lar.BarNumber <> "" Then
                    ar = New AttorneyRecord(lar.BarNumber)
                    If ar.ID = 0 Then ar = lar
                End If
            End If

            If ar.BarNumber <> "" Then
                Me.lblID.Text = ar.ID.ToString
                Me.lblBarNumber.Text = ar.BarNumber
                Me.lblName.Text = ar.Name
                Me.lblAddress.Text = ar.Address1 & "<br/>" & ar.Address2 & "<br/>" & ar.Address3 & "<br/>" & ar.City & ", " & ar.State & " " & ar.ZipCode
                Me.lblWorkPhone.Text = ar.WorkPhone
                Me.lblEmail.Text = ar.Email
                Me.lblLicenseDate.Text = FormatDateTime(ar.LicenseDate, DateFormat.LongDate)

                Dim usr As New SystemUser

                Dim cmd As New SqlClient.SqlCommand("SELECT [ID] FROM [Users] WHERE [xAttorneyID] = " & ar.ID & " AND [ID] > 40;", cn)
                If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
                Dim rs As SqlClient.SqlDataReader = cmd.ExecuteReader
                If rs.Read Then
                    usr = New SystemUser(rs("ID").ToString.ToInteger)
                End If
                cmd.Cancel()
                rs.Close()

                If usr.ID > 0 Then
                    Me.lblUsername.Text = usr.Email
                    Me.lblPassword.Text = usr.Password
                Else
                    Me.lblUsername.Text = "Not Registered"
                    Me.lblPassword.Text = ""
                End If

                cmd = New SqlClient.SqlCommand("SELECT [CivilCount], [CriminalCount], [TotalCount] FROM [vwSalesReport] WHERE [AttorneyID] = " & ar.ID, cn)
                If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
                rs = cmd.ExecuteReader
                If rs.Read Then
                    Me.lblCaseCount.Text = FormatNumber(rs("CivilCount").ToString, 0) & " Civil Cases and " & FormatNumber(rs("CriminalCount").ToString, 0) & " Criminal"
                Else Me.lblCaseCount.Text = "0"
                End If
                cmd.Cancel()
                rs.Close()

                Me.txtBarNumber.Text = ""
                Me.txtFirstName.Text = ""
                Me.txtLastName.Text = ""
                Me.pnlAttorneySearch.Visible = False
                Me.pnlFound.Visible = True
            End If

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(App.CurrentUser.ID, App.CurrentClient.ID, If(Me.Page.OnMobile, Enums.ProjectName.MobileApp, Enums.ProjectName.Builder)))
        Finally
            cn.Close()
        End Try
    End Sub

    Private Sub btnStartOver_Click(sender As Object, e As EventArgs) Handles btnStartOver.Click
        Me.pnlAttorneySearch.Visible = True
        Me.pnlFound.Visible = False
    End Sub

    Private Sub ddlSMSLogType_SelectedIndexChanged(sender As Object, e As DropDownListEventArgs) Handles ddlSMSLogType.SelectedIndexChanged
        Me.SetLogTypes()
    End Sub

    Private Sub btnShowCalendar_Click(sender As Object, e As EventArgs) Handles btnShowCalendar.Click
        App.MyCases = LoadCases(Me.Page, Me.lblBarNumber.Text, False)
        Response.Redirect("~/admin/Calendar.aspx?aid=" & Me.lblBarNumber.Text)
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

    Private Sub _Default4_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
        If Not IsPostBack Then
            Dim x As Integer = 0
            For Each itm In Me.RadSearchGridMobile.MasterTableView.GetItems(GridItemType.GroupHeader)
                If x = 0 Then itm.Expanded = True Else itm.Expanded = False
                x += 1
            Next
        End If
    End Sub

    Private Sub ddlProcessorLogType_SelectedIndexChanged(sender As Object, e As DropDownListEventArgs) Handles ddlProcessorLogType.SelectedIndexChanged
        Me.SetLogTypes()
    End Sub
End Class