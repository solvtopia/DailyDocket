Imports DailyDocket.CommonCore.Shared.Common
Imports Telerik.Web.UI

Public Class Users
    Inherits builderPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Me.LoadChartData()
        End If

        Me.Master.BackButton.Visible = (Me.OnMobile)
        AddHandler Me.Master.BackButton.Click, AddressOf imgBack_Click

        Me.RadGrid1.Visible = (Not Me.OnPhone)
    End Sub

    Protected Sub imgBack_Click(sender As Object, e As ImageClickEventArgs)
        Dim usrString As String = If(Me.DeviceId <> "", "deviceid=" & Me.DeviceId, "uid=" & Me.UserId)
        Response.Redirect("~/Default.aspx?" & usrString, False)
    End Sub

    Private Sub LoadChartData()
        Dim cn As New SqlClient.SqlConnection(ConnectionString)

        Try
            Dim userData As New PieSeries
            userData.StartAngle = 90
            userData.LabelsAppearance.Position = HtmlChart.PieAndDonutLabelsPosition.OutsideEnd
            userData.LabelsAppearance.DataFormatString = "{0}"
            userData.TooltipsAppearance.DataFormatString = "{0}"

            Dim cmd As New SqlClient.SqlCommand("SELECT [Type], [Count] FROM [vwUserBreakdown];", cn)
            If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
            Dim rs As SqlClient.SqlDataReader = cmd.ExecuteReader()
            Do While rs.Read
                Dim item As New SeriesItem
                item.Name = rs("Type").ToString
                item.YValue = rs("Count").ToString.ToDecimal
                userData.Items.Add(item)
            Loop
            cmd.Cancel()
            rs.Close()

            Me.RadHtmlChart1.PlotArea.Series.Add(userData)

            Dim deviceData As New PieSeries
            deviceData.StartAngle = 90
            deviceData.LabelsAppearance.Position = HtmlChart.PieAndDonutLabelsPosition.OutsideEnd
            deviceData.LabelsAppearance.DataFormatString = "{0}"
            deviceData.TooltipsAppearance.DataFormatString = "{0}"

            cmd = New SqlClient.SqlCommand("SELECT Sum(CASE WHEN [MobileDeviceType] LIKE '%android%' THEN 1 ELSE 0 END) AS [AndroidCount], Sum(CASE WHEN [MobileDeviceType] LIKE '%iphone%' OR [MobileDeviceType] LIKE '%ipad%' OR [MobileDeviceType] LIKE '%ipod%' THEN 1 ELSE 0 END) AS [AppleCount] FROM [vwDevices] WHERE [ID] > 40 AND [Active] = 1 AND [xClientID] > 0;", cn)
            If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
            rs = cmd.ExecuteReader
            If rs.Read Then
                Dim item1 As New SeriesItem
                item1.Name = "Android"
                item1.YValue = rs("AndroidCount").ToString.ToDecimal
                deviceData.Items.Add(item1)

                Dim item2 As New SeriesItem
                item2.Name = "Apple"
                item2.YValue = rs("AppleCount").ToString.ToDecimal
                deviceData.Items.Add(item2)
            End If
            cmd.Cancel()
            rs.Close()

            Me.RadHtmlChart2.PlotArea.Series.Add(deviceData)

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(App.CurrentUser.ID, App.CurrentClient.ID, Enums.ProjectName.Builder))
        Finally
            cn.Close()
        End Try
    End Sub

    Private Sub RadGrid1_ItemCommand(sender As Object, e As GridCommandEventArgs) Handles RadGrid1.ItemCommand
        e.Canceled = True

        Try
            Dim editId As Integer = e.Item.Cells(5).Text.ToInteger

            If e.CommandName.ToLower = "edit" Then
                Dim usrString As String = If(Me.Page.OnMobile, "deviceid=" & Me.DeviceId, "uid=" & Me.UserId)
                Response.Redirect("~/account/Profile.aspx?" & usrString & "&editid=" & editId, False)
            ElseIf e.CommandName.ToLower = "delete" Then
                Dim usr As New SystemUser(editId)
                usr.Delete()

                ' process any refund
                Dim refundAmount As Double = 0

                ' email the customer
                Dim msg As New Mailer
                msg.HostServer = ConfigurationManager.AppSettings("MailHost")
                msg.UserName = ConfigurationManager.AppSettings("MailUser")
                msg.Password = ConfigurationManager.AppSettings("MailPassword")
                msg.Port = ConfigurationManager.AppSettings("MailPort").ToInteger
                msg.To.Add(usr.Email)
                msg.Body = Mailer.GetHtmlBody(Enums.EmailType.Client_Cancel_Service, usr)
                msg.Body = msg.Body.Replace("[RefundAmount]", FormatCurrency(refundAmount, 2))
                msg.Subject = "Daily Docket - Service Cancelled"
                msg.From = "sales@solvtopia.com"
                msg.HtmlBody = True
                msg.Send()

                ' email jose
                msg = New Mailer
                msg.HostServer = ConfigurationManager.AppSettings("MailHost")
                msg.UserName = ConfigurationManager.AppSettings("MailUser")
                msg.Password = ConfigurationManager.AppSettings("MailPassword")
                msg.Port = ConfigurationManager.AppSettings("MailPort").ToInteger
                msg.To.Add("jose@solvtopia.com")
                msg.To.Add("james@solvtopia.com")
                msg.Body = Mailer.GetHtmlBody(Enums.EmailType.Sys_Cancel_Service, usr)
                msg.Body = msg.Body.Replace("[RefundAmount]", FormatCurrency(refundAmount, 2))
                msg.Subject = "Daily Docket - Service Cancelled"
                msg.From = "sales@solvtopia.com"
                msg.HtmlBody = True
                msg.Send()

                Me.RadGrid1.Rebind()
            End If

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(App.CurrentUser.ID, App.CurrentClient.ID, Enums.ProjectName.Builder))
        End Try
    End Sub
End Class