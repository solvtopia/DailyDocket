Imports Telerik.Web.UI
Imports DailyDocket.Builder.Common
Imports DailyDocket.CommonCore.Shared.Common
Imports System.Configuration.ConfigurationManager

Public Class MessageCenter
    Inherits builderPage

#Region "Properties"

#End Region

#Region "Structures"

    Private Structure ClientNames
        Dim Name As String
        Dim NameFromFile As String
        Dim SMSEnabled As Boolean
        Dim MobileNumber As String
    End Structure

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Me.LoadLists()
            Me.LoadData()
        End If

        Me.SetupForm()

        Me.Master.BackButton.Visible = (Me.OnMobile)
        AddHandler Me.Master.BackButton.Click, AddressOf imgBack_Click
    End Sub

    Private Sub SetupForm()
        If Me.ddlNotificationType.SelectedValue = CStr(Enums.NotificationType.Email) Then
            Me.ddlNotificationLevel.SelectedValue = CStr(Enums.NotificationLevel.Detail)
            Me.lblMaxSMSPerBatch.Visible = False
            Me.txtMaxSMSPerBatch.Visible = False
        Else
            Me.lblMaxSMSPerBatch.Visible = (Me.ddlNotificationLevel.SelectedValue = CStr(Enums.NotificationLevel.Detail))
            Me.txtMaxSMSPerBatch.Visible = Me.lblMaxSMSPerBatch.Visible
        End If

        Me.pnlClientRecord.Visible = (Me.lstClients.SelectedIndex > -1)

        Me.fraAttorneySMSLog.ContentUrl = "~/admin/SMSLog.aspx?type=attorney&aid=" & App.CurrentAttorney.ID & "&id=0"
    End Sub

    Private Sub LoadLists()
        App.CurrentAttorney = New AttorneyRecord(App.CurrentUser.AttorneyID)

        Dim cn As New SqlClient.SqlConnection(ConnectionString)

        Try
            Me.ddlNotificationType.Items.Clear()
            Dim enumValues As Array = System.[Enum].GetValues(GetType(Enums.NotificationType))
            For Each resource As Enums.NotificationType In enumValues
                If resource <> Enums.NotificationType.Custom Then
                    Me.ddlNotificationType.Items.Add(New DropDownListItem(resource.ToString, CStr(resource)))
                End If
            Next

            Me.ddlNotificationLevel.Items.Clear()
            enumValues = System.[Enum].GetValues(GetType(Enums.NotificationLevel))
            For Each resource As Enums.NotificationLevel In enumValues
                Me.ddlNotificationLevel.Items.Add(New DropDownListItem(resource.ToString, CStr(resource)))
            Next

            Me.ddlTo.Items.Clear()
            Me.ddlTo.Items.Add(New Telerik.Web.UI.DropDownListItem("All Clients", "0"))

            If App.MyCases Is Nothing OrElse App.MyCases.Count = 0 Then App.MyCases = LoadCases(Me.Page, App.CurrentUser.AttorneyID, True)

            Me.lstClients.Items.Clear()

            Dim lstNames As New List(Of ClientNames)
            For Each ap As MyAppointment In App.MyCases
                Dim fr As New FileRecord
                fr = CType(fr.DeserializeFromXml(ap.XmlData), FileRecord)

                Dim myRole As Enums.AttorneyType = fr.AttorneyType(App.CurrentAttorney.AllNames)

                If myRole = Enums.AttorneyType.Plaintiff Then
                    For Each s As String In fr.Plaintiff
                        If s <> "" Then
                            Dim itm As New ClientNames
                            itm.Name = FormatName(s)
                            itm.NameFromFile = s
                            itm.SMSEnabled = False
                            itm.MobileNumber = ""
                            If Not lstNames.Contains(itm) Then lstNames.Add(itm)
                        End If
                    Next
                ElseIf myRole = Enums.AttorneyType.Defendant Then
                    For Each s As String In fr.Defendant
                        If s <> "" Then
                            Dim itm As New ClientNames
                            itm.Name = FormatName(s)
                            itm.NameFromFile = s
                            itm.SMSEnabled = False
                            itm.MobileNumber = ""
                            If Not lstNames.Contains(itm) Then lstNames.Add(itm)
                        End If
                    Next
                End If
            Next

            lstNames.Sort(Function(p1, p2) p1.Name.CompareTo(p2.Name))

            For Each s As ClientNames In lstNames
                If s.Name <> "" Then
                    Dim itm As New RadListBoxItem
                    itm.Text = s.Name
                    itm.Value = s.NameFromFile
                    Me.lstClients.Items.Add(itm)

                    Me.ddlTo.Items.Add(New Telerik.Web.UI.DropDownListItem(itm.Text, itm.Value))
                End If
            Next

            Me.ddlTo.SelectedIndex = 0

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(App.CurrentUser.ID, App.CurrentClient.ID, If(Me.Page.OnMobile, Enums.ProjectName.MobileApp, Enums.ProjectName.Builder)))
        Finally
            cn.Close()
        End Try
    End Sub

    Private Sub LoadData()
        Me.ddlNotificationType.SelectedValue = CStr(App.CurrentUser.NotificationType)
        Me.ddlNotificationLevel.SelectedValue = CStr(App.CurrentUser.NotificationLevel)
        Me.txtMaxSMSPerBatch.Text = App.CurrentUser.MaxNotificationsPerBatch.ToString
    End Sub

    Private Sub SaveChanges()
        Dim lstNumbers As New List(Of String)
        Dim splitter As Char = CChar(vbCrLf)
        If Not Me.txtMobileNumbers.Text.Contains(splitter) Then splitter = CChar(vbLf)
        For Each n As String In Me.txtMobileNumbers.Text.Split(splitter)
            If Not lstNumbers.Contains(n.FixPhoneNumber) Then lstNumbers.Add(n.FixPhoneNumber)
        Next
        App.CurrentUser.MobileNumbers = lstNumbers

        App.CurrentUser.NotificationType = CType(Me.ddlNotificationType.SelectedValue, Enums.NotificationType)
        App.CurrentUser.NotificationLevel = CType(Me.ddlNotificationLevel.SelectedValue, Enums.NotificationLevel)
        If Me.txtMaxSMSPerBatch.Text = "" Then Me.txtMaxSMSPerBatch.Text = "5"
        App.CurrentUser.MaxNotificationsPerBatch = Me.txtMaxSMSPerBatch.Text.ToInteger

        App.CurrentUser = App.CurrentUser.Save
    End Sub

    Protected Sub imgBack_Click(sender As Object, e As ImageClickEventArgs)
        Dim usrString As String = If(Me.DeviceId <> "", "deviceid=" & Me.DeviceId, "uid=" & Me.UserId)
        Response.Redirect("~/Default.aspx?" & usrString, False)
    End Sub

    Protected Sub lstClients_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lstClients.SelectedIndexChanged
        Dim cr As New ClientRecord(lstClients.SelectedValue, App.CurrentAttorney.ID)

        Me.txtName.Text = cr.Name
        Me.txtMobileNumber.Text = cr.MobileNumber
        Me.txtEmail.Text = cr.Email
        Me.txtAddress1.Text = cr.Address1
        Me.txtAddress2.Text = cr.Address2
        Me.txtZipCode.Text = cr.ZipCode
        Me.LoadZipCode()
        If cr.City <> "" Then Me.txtCity.Text = cr.City

        Me.fraClientSMSLog.ContentUrl = "~/admin/SMSLog.aspx?type=client&aid=0&id=" & cr.ID
    End Sub

    Protected Sub btnSaveClient_Click(sender As Object, e As EventArgs) Handles btnSaveClient.Click
        Dim cr As New ClientRecord(lstClients.SelectedValue, App.CurrentAttorney.ID)

        cr.Name = Me.txtName.Text
        cr.NameFromFile = lstClients.SelectedValue
        cr.MobileNumber = Me.txtMobileNumber.Text.FixPhoneNumber
        cr.Email = Me.txtEmail.Text
        cr.Address1 = Me.txtAddress1.Text
        cr.Address2 = Me.txtAddress2.Text
        cr.ZipCode = Me.txtZipCode.Text
        cr.City = Me.txtCity.Text
        cr.State = Me.ddlState.SelectedValue
        cr.AttorneyID = App.CurrentAttorney.ID

        cr.Save()
    End Sub

    Protected Sub txtZipCode_TextChanged(sender As Object, e As EventArgs) Handles txtZipCode.TextChanged
        Me.LoadZipCode()
    End Sub

    Private Sub LoadZipCode()
        Dim svc As New ZipLookup.USZip

        If Me.txtZipCode.Text = "" Then
            'Me.ddlBillingState.Items.Clear()
        Else
            Dim xNode As System.Xml.XmlNode = svc.GetInfoByZIP(Me.txtZipCode.Text)

            Me.txtCity.Text = xNode.Item("Table").Item("CITY").InnerText
            Me.ddlState.SelectedValue = xNode.Item("Table").Item("STATE").InnerText.Trim.ToUpper
        End If
    End Sub

    Private Sub btnSend_Click(sender As Object, e As EventArgs) Handles btnSend.Click
        Dim cn As New SqlClient.SqlConnection(ConnectionString)

        Try
            Dim messagesSent As Integer = 0
            Dim txt As String = ""

            If Me.RadTabStrip3.SelectedIndex = 0 Then
                ' email
                txt = "[MessaageHeader]<br/><br/>Dear [ToName],<br/><br/>"
                txt &= Me.RadEditor1.Content

                ' add the daily docket footer
                txt &= "<br/><br/><br/>This message was sent with Daily Docket by Solvtopia, LLC."

                If txt <> "" Then
                    Dim sBody As String = txt
                    sBody = sBody.Replace("[HeaderMessage]", "<strong>" & Me.txtSubject.Text & "</strong>")

                    Dim msg As New Mailer
                    msg.HostServer = AppSettings("MailHost")
                    msg.UserName = AppSettings("MailUser")
                    msg.Password = AppSettings("MailPassword")
                    msg.Port = AppSettings("MailPort").ToInteger
                    msg.Body = sBody
                    msg.Subject = Me.txtSubject.Text
                    msg.From = "sales@solvtopia.com"
                    msg.HtmlBody = True

                    If Me.ddlTo.SelectedValue.ToInteger = 0 Then
                        ' all clients
                        Dim cmd As New SqlClient.SqlCommand("SELECT [ID], [xName], [xEmail], [xAttorneyID] FROM [ClientRecords] WHERE [xAttorneyID] = " & App.CurrentAttorney.ID & ";", cn)
                        If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
                        Dim rs As SqlClient.SqlDataReader = cmd.ExecuteReader
                        Do While rs.Read
                            Dim nMsg As New NotificationMessage
                            nMsg.CaseNumber = ""
                            nMsg.AttorneyId = rs("xAttorneyID").ToString.ToInteger
                            nMsg.ClientId = rs("ID").ToString.ToInteger
                            nMsg.Message = txt
                            nMsg.ToNumber = ""
                            nMsg.ToEmail = rs("xEmail").ToString
                            nMsg.MessageType = Enums.NotificationMessageType.Email
                            nMsg.Type = Enums.NotificationRecipientType.Client
                            nMsg.LogNotification

                            msg.To.Clear()
                            msg.Body = msg.Body.Replace("[ToName]", rs("xName").ToString)
                            msg.To.Add(rs("xEmail").ToString)
                            Dim success As Boolean = msg.Send()

                            msg.Body = msg.Body.Replace(rs("xName").ToString, "[ToName]")
                            If success Then messagesSent += 1
                        Loop
                        cmd.Cancel()
                        rs.Close()
                    Else
                        ' selected client
                        Dim cl As New ClientRecord(Me.ddlTo.SelectedValue, App.CurrentAttorney.ID)
                        Dim nMsg As New NotificationMessage
                        nMsg.CaseNumber = ""
                        nMsg.AttorneyId = cl.AttorneyID
                        nMsg.ClientId = cl.ID
                        nMsg.Message = txt
                        nMsg.ToNumber = ""
                        nMsg.ToEmail = cl.Email
                        nMsg.MessageType = Enums.NotificationMessageType.Email
                        nMsg.Type = Enums.NotificationRecipientType.Client
                        nMsg.LogNotification

                        msg.Body = msg.Body.Replace("[ToName]", cl.Name)
                        msg.To.Add(cl.Email)
                        Dim success As Boolean = msg.Send()
                        If success Then messagesSent += 1
                    End If
                End If
            Else
                ' sms
                txt = Me.txtSMS.Text
                If Me.ddlTo.SelectedValue.ToInteger = 0 Then
                    ' all clients
                    Dim cmd As New SqlClient.SqlCommand("SELECT [ID], [xName], [xEmail], [xAttorneyID] FROM [ClientRecords] WHERE [xAttorneyID] = " & App.CurrentAttorney.ID & ";", cn)
                    If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
                    Dim rs As SqlClient.SqlDataReader = cmd.ExecuteReader
                    Do While rs.Read
                        Dim cl As New ClientRecord(rs("ID").ToString.ToInteger)

                        Dim msg As New NotificationMessage
                        msg.CaseNumber = ""
                        msg.AttorneyId = rs("xAttorneyID").ToString.ToInteger
                        msg.ClientId = cl.ID
                        msg.Message = Me.txtSMS.Text
                        msg.ToNumber = cl.MobileNumber.FixPhoneNumber
                        msg.Type = Enums.NotificationRecipientType.Client
                        msg.ToEmail = ""
                        msg.MessageType = Enums.NotificationMessageType.SMS
                        msg.LogNotification

                        Dim success As Boolean = Messaging.SendTwilioNotification(msg.ToNumber, msg.Message)
                        If success Then messagesSent += 1
                    Loop
                    cmd.Cancel()
                    rs.Close()
                Else
                    ' selected client
                    Dim cl As New ClientRecord(Me.ddlTo.SelectedValue, App.CurrentAttorney.ID)

                    Dim msg As New NotificationMessage
                    msg.CaseNumber = ""
                    msg.AttorneyId = cl.AttorneyID
                    msg.ClientId = cl.ID
                    msg.Message = txt
                    msg.ToNumber = cl.MobileNumber.FixPhoneNumber
                    msg.Type = Enums.NotificationRecipientType.Client
                    msg.ToEmail = ""
                    msg.MessageType = Enums.NotificationMessageType.SMS
                    msg.LogNotification

                    Dim success As Boolean = Messaging.SendTwilioNotification(msg.ToNumber, msg.Message)
                    If success Then messagesSent += 1
                End If
            End If

            Me.lblError.Text = messagesSent & " Message(s) Sent."

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(App.CurrentUser.ID, App.CurrentClient.ID, If(Me.Page.OnMobile, Enums.ProjectName.MobileApp, Enums.ProjectName.Builder)))
        Finally
            cn.Close()
        End Try
    End Sub
End Class