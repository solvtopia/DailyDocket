Imports DailyDocket.CommonCore.Shared.Common
Imports Telerik.Web.UI

Public Class ProfitChart
    Inherits System.Web.UI.Page

#Region "Properties"

    Private ReadOnly Property AttorneyId As Integer
        Get
            Return Request.QueryString("aid").ToInteger
        End Get
    End Property
    Private ReadOnly Property ClientCharge As Double
        Get
            Return Request.QueryString("charge").ToDouble
        End Get
    End Property

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.LoadChartData()
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
            itemClients.YValue = Me.ClientCharge.ToDecimal * clientCount
            userData.Items.Add(itemClients)
            Me.lblClientFees.Text = FormatCurrency(Me.ClientCharge.ToDecimal * clientCount, 2)

            Me.RadHtmlChart1.PlotArea.Series.Add(userData)

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(App.CurrentUser.ID, App.CurrentClient.ID, Enums.ProjectName.Builder))
        Finally
            cn.Close()
        End Try
    End Sub

End Class