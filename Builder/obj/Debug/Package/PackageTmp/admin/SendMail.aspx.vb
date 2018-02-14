Imports DailyDocket.CommonCore.Shared.Common
Imports System.Configuration.ConfigurationManager

Public Class SendMail
    Inherits builderPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Me.LoadLists()
            Me.txtEmail.Content = Mailer.GetHtmlBody(Enums.EmailType.Custom)
            Me.txtBCC.Text = "sales@solvtopia.com"
        End If

        Me.Master.BackButton.Visible = (Me.OnMobile)
        AddHandler Me.Master.BackButton.Click, AddressOf imgBack_Click
    End Sub

    Private Sub LoadLists()
        Dim cn As New SqlClient.SqlConnection(ConnectionString)

        Try
            Me.ddlTo.Items.Clear()
            Me.ddlTo.Items.Add(New Telerik.Web.UI.DropDownListItem("All Users", "0"))

            Dim cmd As New SqlClient.SqlCommand("SELECT [ID], [xName] FROM [Users] WHERE [ID] > 1 AND [xClientID] > 0 AND [xName] IS NOT NULL;", cn)
            If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
            Dim rs As SqlClient.SqlDataReader = cmd.ExecuteReader
            Do While rs.Read
                Me.ddlTo.Items.Add(New Telerik.Web.UI.DropDownListItem(rs("xName").ToString, rs("ID").ToString))
            Loop
            cmd.Cancel()
            rs.Close()

            Me.ddlTo.SelectedIndex = 0

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(App.CurrentUser.ID, App.CurrentClient.ID, If(Me.Page.OnMobile, Enums.ProjectName.MobileApp, Enums.ProjectName.Builder)))
        Finally
            cn.Close()
        End Try
    End Sub

    Private Sub btnSend_Click(sender As Object, e As EventArgs) Handles btnSend.Click
        Dim cn As New SqlClient.SqlConnection(ConnectionString)

        Try
            Dim messagesSent As Integer = 0
            Dim txt As String = ""

            If Me.RadTabStrip1.SelectedIndex = 0 Then
                ' email
                txt = Me.txtEmail.Content
                If txt <> "" Then
                    Dim sBody As String = txt
                    sBody = sBody.Replace("[HeaderMessage]", "<strong>" & Me.txtSubject.Text & "</strong>")
                    sBody = sBody.Replace("[MessageContent]", txt)

                    Dim msg As New Mailer
                    msg.HostServer = AppSettings("MailHost")
                    msg.UserName = AppSettings("MailUser")
                    msg.Password = AppSettings("MailPassword")
                    msg.Port = AppSettings("MailPort").ToInteger
                    msg.Body = sBody
                    msg.Subject = Me.txtSubject.Text
                    msg.From = "sales@solvtopia.com"
                    msg.HtmlBody = True
                    msg.CC = Me.txtCC.Text.Split(";"c).ToList
                    msg.BCC = Me.txtBCC.Text.Split(";"c).ToList

                    If Me.ddlTo.SelectedValue.ToInteger = 0 Then
                        ' all users
                        Dim cmd As New SqlClient.SqlCommand("SELECT [ID], [xName], [xEmail], [xPassword], [xAttorneyID] FROM [Users] WHERE [ID] > 1 AND [xClientID] > 0 AND [xName] IS NOT NULL AND [Active] = 1;", cn)
                        If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
                        Dim rs As SqlClient.SqlDataReader = cmd.ExecuteReader
                        Do While rs.Read
                            Dim nMsg As New NotificationMessage
                            nMsg.CaseNumber = ""
                            nMsg.AttorneyId = rs("xAttorneyID").ToString.ToInteger
                            nMsg.ClientId = 0
                            nMsg.Message = txt
                            nMsg.ToNumber = ""
                            nMsg.ToEmail = If(rs("ID").ToString.ToInteger = 40, "james@solvtopia.com", rs("xEmail").ToString)
                            nMsg.MessageType = Enums.NotificationMessageType.Email
                            nMsg.Type = Enums.NotificationRecipientType.Attorney
                            nMsg.LogNotification

                            msg.To.Clear()
                            msg.Body = msg.Body.Replace("[UserEmail]", rs("xEmail").ToString)
                            msg.Body = msg.Body.Replace("[UserPassword]", rs("xPassword").ToString)
                            msg.Body = msg.Body.Replace("[ToName]", rs("xName").ToString)
                            msg.To.Add(If(rs("ID").ToString.ToInteger = 40, "james@solvtopia.com", rs("xEmail").ToString))
                            Dim success As Boolean = msg.Send()

                            msg.Body = msg.Body.Replace(rs("xName").ToString, "[ToName]")
                            If success Then messagesSent += 1
                        Loop
                        cmd.Cancel()
                        rs.Close()
                    Else
                        ' selected user
                        Dim usr As New SystemUser(Me.ddlTo.SelectedValue.ToInteger)
                        Dim nMsg As New NotificationMessage
                        nMsg.CaseNumber = ""
                        nMsg.AttorneyId = usr.AttorneyID
                        nMsg.ClientId = 0
                        nMsg.Message = txt
                        nMsg.ToNumber = ""
                        nMsg.ToEmail = If(usr.ID = 40, "james@solvtopia.com", usr.Email)
                        nMsg.MessageType = Enums.NotificationMessageType.Email
                        nMsg.Type = Enums.NotificationRecipientType.Attorney
                        nMsg.LogNotification

                        msg.Body = msg.Body.Replace("[UserEmail]", usr.Email)
                        msg.Body = msg.Body.Replace("[UserPassword]", usr.Password)
                        msg.Body = msg.Body.Replace("[ToName]", usr.Name)
                        msg.To.Add(If(usr.ID = 40, "james@solvtopia.com", usr.Email))
                        Dim success As Boolean = msg.Send()
                        If success Then messagesSent += 1
                    End If
                End If
            Else
                ' sms
                txt = Me.txtSMS.Text
                If Me.ddlTo.SelectedValue.ToInteger = 0 Then
                    ' all users
                    Dim cmd As New SqlClient.SqlCommand("SELECT [ID], [xAttorneyID] FROM [Users] WHERE [ID] > 1 AND [xClientID] > 0 AND [xName] IS NOT NULL AND [Active] = 1;", cn)
                    If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
                    Dim rs As SqlClient.SqlDataReader = cmd.ExecuteReader
                    Do While rs.Read
                        Dim usr As New SystemUser(rs("ID").ToString.ToInteger)

                        For Each n As String In usr.MobileNumbers
                            Dim msg As New NotificationMessage
                            msg.CaseNumber = ""
                            msg.AttorneyId = rs("xAttorneyID").ToString.ToInteger
                            msg.ClientId = 0
                            msg.Message = Me.txtSMS.Text
                            msg.ToNumber = n.FixPhoneNumber
                            msg.Type = Enums.NotificationRecipientType.Attorney
                            msg.ToEmail = ""
                            msg.MessageType = Enums.NotificationMessageType.SMS
                            msg.LogNotification

                            Dim success As Boolean = Messaging.SendTwilioNotification(msg.ToNumber, msg.Message)
                            If success Then messagesSent += 1
                        Next
                    Loop
                    cmd.Cancel()
                    rs.Close()
                Else
                    ' selected user
                    Dim usr As New SystemUser(Me.ddlTo.SelectedValue.ToInteger)

                    For Each n As String In usr.MobileNumbers
                        Dim msg As New NotificationMessage
                        msg.CaseNumber = ""
                        msg.AttorneyId = usr.AttorneyID
                        msg.ClientId = 0
                        msg.Message = txt
                        msg.ToNumber = n.FixPhoneNumber
                        msg.Type = Enums.NotificationRecipientType.Attorney
                        msg.ToEmail = ""
                        msg.MessageType = Enums.NotificationMessageType.SMS
                        msg.LogNotification

                        Dim success As Boolean = Messaging.SendTwilioNotification(msg.ToNumber, msg.Message)
                        If success Then messagesSent += 1
                    Next
                End If
            End If

            Me.lblError.Text = messagesSent & " Message(s) Sent."

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(App.CurrentUser.ID, App.CurrentClient.ID, If(Me.Page.OnMobile, Enums.ProjectName.MobileApp, Enums.ProjectName.Builder)))
        Finally
            cn.Close()
        End Try
    End Sub

    Protected Sub imgBack_Click(sender As Object, e As ImageClickEventArgs)
        Dim usrString As String = If(Me.DeviceId <> "", "deviceid=" & Me.DeviceId, "uid=" & Me.UserId)
        Response.Redirect("~/Default.aspx?" & usrString, False)
    End Sub
End Class