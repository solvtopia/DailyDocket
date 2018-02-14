Imports System.Net.Mail

Public Class Mailer
    Public Sub New()
        Me.To = New List(Of String)
        Me.CC = New List(Of String)
        Me.BCC = New List(Of String)
        Me.Attachments = New List(Of String)
    End Sub
    Public Sub New(ByVal subject As String, ByVal body As String, ByVal [to] As String)
        Me.To = New List(Of String)
        Me.CC = New List(Of String)
        Me.BCC = New List(Of String)
        Me.Attachments = New List(Of String)

        Me.Subject = subject
        Me.Body = body
        Me.To.Add([to])
    End Sub
    Public Sub New(ByVal subject As String, ByVal body As String, ByVal [to] As String, ByVal attachment As String)
        Me.To = New List(Of String)
        Me.CC = New List(Of String)
        Me.BCC = New List(Of String)
        Me.Attachments = New List(Of String)

        Me.Subject = subject
        Me.Body = body
        Me.To.Add([to])
        Me.Attachments.Add(attachment)
    End Sub

    Public Subject As String
    Public Body As String

    Public [To] As List(Of String)
    Public [From] As String
    Public CC As List(Of String)
    Public BCC As List(Of String)

    Public Attachments As List(Of String)
    Public HtmlBody As Boolean

    Public HostServer As String
    Public UserName As String
    Public Password As String
    Public Port As Integer

    Public Shared Function GetHtmlBody(ByVal type As Enums.EmailType) As String
        Return GetHtmlBody(type, Nothing)
    End Function
    Public Shared Function GetHtmlBody(ByVal type As Enums.EmailType, ByVal filler As Object) As String
        Dim retVal As String = ""

        ' get the body of the email from the html files based on the type
        retVal = Common.ScrapeUrl("https://access.dailycourtdocket.com/email_bodies/" & type.ToString & ".html", Enums.ScrapeType.KeepTags)

        Dim dateSent As String = FormatDateTime(Now, DateFormat.LongDate)
        Dim timeSent As String = FormatDateTime(Now, DateFormat.LongTime)

        ' fill in the blanks based on the type using the filler object
        Select Case type
            Case Enums.EmailType.Client_Notification_Detail
                Dim lst As List(Of NotificationMessage) = CType(filler, List(Of NotificationMessage))
                Dim caseList As String = ""
                For Each txt As NotificationMessage In lst
                    retVal = retVal.Replace("[BatchRunDate]", FormatDateTime(txt.BatchRunDate, DateFormat.ShortDate))
                    retVal = retVal.Replace("[BatchRunTime]", FormatDateTime(txt.BatchRunDate, DateFormat.LongTime))

                    Dim t As String = If(txt.Message.ToLower.Contains("added"), "Added", "Updated")
                    Dim m As String = txt.Message.Replace(" has been updated on your calendar.", "").Replace(" has been added to your calendar.", "")
                    If Not caseList.ToLower.Contains(txt.CaseNumber.ToLower) Then
                        If caseList = "" Then caseList = m & " " & t Else caseList &= "<br/>" & m & " " & t
                    End If
                Next
                retVal = retVal.Replace("[CaseList]", caseList)

            Case Enums.EmailType.Custom
                ' custom email body used by the admin console to send messages to all clients

            Case Enums.EmailType.Client_Invoice
                ' these fields will be processed inside the billing processor

            Case Enums.EmailType.Client_Receipt
                ' these fields will be processed inside the billing processor

            Case Enums.EmailType.Client_Payment_Failed
                ' these fields will be processed inside the billing processor

            Case Enums.EmailType.Client_TrialEnding
                Dim usr As SystemUser = CType(filler, SystemUser)
                Dim cl As SystemClient = New SystemClient(usr.ClientID)
                retVal = retVal.Replace("[ClientName]", cl.Name)
                retVal = retVal.Replace("[DemoDuration]", cl.DemoDuration.ToString)

            Case Enums.EmailType.Client_TrialExpired
                Dim usr As SystemUser = CType(filler, SystemUser)
                Dim cl As SystemClient = New SystemClient(usr.ClientID)
                retVal = retVal.Replace("[ClientName]", cl.Name)
                retVal = retVal.Replace("[DemoDuration]", cl.DemoDuration.ToString)

            Case Enums.EmailType.Client_Cancel_Service
                Dim usr As SystemUser = CType(filler, SystemUser)
                Dim cl As SystemClient = New SystemClient(usr.ClientID)
                retVal = retVal.Replace("[ClientName]", cl.Name)
                retVal = retVal.Replace("[CancelDate]", FormatDateTime(Now, DateFormat.ShortDate))

            Case Enums.EmailType.Client_Registration
                Dim usr As SystemUser = CType(filler, SystemUser)
                retVal = retVal.Replace("[UserName]", usr.Email)
                retVal = retVal.Replace("[Password]", usr.Password)

            Case Enums.EmailType.Client_Forgot_Password
                retVal = retVal.Replace("[Password]", filler.ToString)

            Case Enums.EmailType.Sys_Registration
                Dim usr As SystemUser = CType(filler, SystemUser)
                Dim cl As SystemClient = New SystemClient(usr.ClientID)
                retVal = retVal.Replace("[ClientName]", cl.Name)
                retVal = retVal.Replace("[ContactName]", cl.ContactName)
                retVal = retVal.Replace("[ContactEmail]", cl.ContactEmail)
                retVal = retVal.Replace("[ContactPhone]", cl.ContactPhone)

            Case Enums.EmailType.Sys_Cancel_Service
                Dim usr As SystemUser = CType(filler, SystemUser)
                Dim cl As SystemClient = New SystemClient(usr.ClientID)
                retVal = retVal.Replace("[ClientName]", cl.Name)
                retVal = retVal.Replace("[ContactName]", cl.ContactName)
                retVal = retVal.Replace("[ContactEmail]", cl.ContactEmail)
                retVal = retVal.Replace("[ContactPhone]", cl.ContactPhone)

        End Select

        retVal = retVal.Replace("[DateSent]", dateSent)
        retVal = retVal.Replace("[TimeSent]", timeSent)

        Return retVal
    End Function

#Region "Workers"

    Public Function Send() As Boolean
        Dim retVal As Boolean = True

        Try
            Dim Smtp_Server As New SmtpClient
            Dim e_mail As New MailMessage()
            Smtp_Server.UseDefaultCredentials = False
            Smtp_Server.Credentials = New Net.NetworkCredential(Me.UserName, Me.Password)
            Smtp_Server.Host = Me.HostServer
            Smtp_Server.Port = Me.Port

            e_mail = New MailMessage()
            e_mail.From = New MailAddress(Me.From)

            For Each toAddress As String In Me.To
                e_mail.To.Add(toAddress)
            Next
            For Each toAddress As String In Me.CC
                If toAddress.Trim <> "" Then e_mail.CC.Add(toAddress)
            Next
            For Each toAddress As String In Me.BCC
                If toAddress.Trim <> "" Then e_mail.Bcc.Add(toAddress)
            Next
            e_mail.Subject = Me.Subject
            e_mail.IsBodyHtml = Me.HtmlBody
            e_mail.Body = Me.Body

            For Each att As String In Me.Attachments
                e_mail.Attachments.Add(New Attachment(att))
            Next
            Smtp_Server.Send(e_mail)

        Catch ex As Exception
            retVal = False
        End Try

        Return retVal
    End Function

#End Region

End Class
