Imports DailyDocket.CommonCore.Shared.Common
Imports System.Runtime.CompilerServices
Imports System.Web
Imports System.Windows.Forms

Public Module Logs

    <Extension()> Public Sub LogNotification(ByVal msg As NotificationMessage)
        Dim cn As New SqlClient.SqlConnection(ConnectionString)

        Try
            Dim cmd As New SqlClient.SqlCommand("INSERT INTO [Sys_SMSLog] ([ToNumber], [Message], [AttorneyID], [ClientID], [Type], [ToEmail], [MessageType], [dtInserted]) VALUES ('" & msg.ToNumber & "', @message, '" & msg.AttorneyId & "', '" & msg.ClientId & "', '" & CStr(msg.Type) & "', '" & msg.ToEmail & "', '" & CStr(msg.MessageType) & "', '" & Now.ToString & "');", cn)
            cmd.Parameters.AddWithValue("@message", msg.Message)
            If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
            cmd.ExecuteNonQuery()

        Catch ex As Exception
        Finally
            cn.Close()
        End Try
    End Sub

    <Extension()> Public Sub LogFileProcessorHistory(ByRef lst As ListBox, ByVal text As String)
        Dim cn As New SqlClient.SqlConnection(ConnectionString)

        Try
            Dim cmd As New SqlClient.SqlCommand("INSERT INTO [Sys_ProcessorLog] ([Text], [dtInserted]) VALUES ('" & text & "', '" & Now.ToString & "');", cn)
            If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
            cmd.ExecuteNonQuery()

        Catch ex As Exception
        Finally
            cn.Close()
        End Try

        lst.Items.Add(text.Replace("<strong>", " ****** ").Replace("</strong>", " ****** "))
        lst.SelectedIndex = lst.Items.Count - 1
    End Sub

    <Extension()> Public Sub LogBillingProcessorHistory(ByRef lst As ListBox, ByVal text As String)
        Dim cn As New SqlClient.SqlConnection(ConnectionString)

        Try
            Dim cmd As New SqlClient.SqlCommand("INSERT INTO [Sys_BillingLog] ([Text], [dtInserted]) VALUES ('" & text & "', '" & Now.ToString & "');", cn)
            If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
            cmd.ExecuteNonQuery()

        Catch ex As Exception
        Finally
            cn.Close()
        End Try

        lst.Items.Add(text.Replace("<strong>", " ****** ").Replace("</strong>", " ****** "))
        lst.SelectedIndex = lst.Items.Count - 1
    End Sub


    ' writes a new entry to the audit log
    <Extension()> Public Sub WriteToAuditLog(ByVal entry As AuditLogEntry)
        If entry.ActionType <> Enums.LogEntry.QueryRecord.ToString Then
            Dim cn As New SqlClient.SqlConnection(ConnectionString)

            Try
                ' add the user's ip address to the entry object before saving
                'Dim REMOTE_ADDR As String = HttpContext.Current.Request.ServerVariables("REMOTE_ADDR")
                'If REMOTE_ADDR = "127.0.0.1" Then REMOTE_ADDR = "96.36.105.18"

                'entry.IpAddress = REMOTE_ADDR

                Dim cmd As New SqlClient.SqlCommand("INSERT INTO [Sys_AuditLog] (ClientID, xmlData, dtInserted, dtUpdated, insertedBy, updatedBy) VALUES (@ClientID, @xmlData, '" & Now.ToString & "', '" & Now.ToString & "', '" & entry.UserID & "', '" & entry.UserID & "');", cn)
                cmd.Parameters.Clear()
                cmd.Parameters.AddWithValue("@ClientID", entry.ClientID)
                cmd.Parameters.AddWithValue("@xmlData", entry.SerializeToXml)
                If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
                cmd.CommandTimeout = 0
                cmd.ExecuteNonQuery()
                cmd.Cancel()

            Catch ex As Exception
                ex.WriteToErrorLog(New ErrorLogEntry(entry.UserID, entry.ClientID, entry.Project))
            Finally
                cn.Close()
            End Try
        End If
    End Sub

    ' writes a new entry to the error log
    <Extension()> Public Sub WriteToErrorLog(ByVal exp As Exception, ByVal entry As ErrorLogEntry)
        Dim cn As New SqlClient.SqlConnection(ConnectionString)

        Try
            If exp IsNot Nothing Then
                Dim ex As String = exp.SerializeToXml
                If ex = "" Then ex = ObjectToXml(exp)
                If ex = "" Then ex = "<Exception>" & exp.ToString & "</Exception>"

                Dim cmd As New SqlClient.SqlCommand("INSERT INTO [Sys_ErrorLog] (UserID, ClientID, xmlData, Project, dtInserted, dtUpdated, insertedBy, updatedBy) VALUES (@UserID, @ClientID, @xmlData, @Project, '" & Now.ToString & "', '" & Now.ToString & "', '0', '0');", cn)
                cmd.Parameters.Clear()
                cmd.Parameters.AddWithValue("@UserID", entry.userId)
                cmd.Parameters.AddWithValue("@ClientID", entry.clientId)
                cmd.Parameters.AddWithValue("@xmlData", ex)
                cmd.Parameters.AddWithValue("@Project", entry.project.ToString)
                If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
                cmd.CommandTimeout = 0
                cmd.ExecuteNonQuery()
                cmd.Cancel()

                'Dim msg As New Mailer
                'msg.MailTo.Add("james@solvtopia.com")
                'msg.MailFrom = "noreply@nk5.co"
                'msg.Password = "nk5coNoReply!"
                'msg.Subject = "Error from Utility Wizards Builder - " & project & " Module"
                'msg.Body = exp.ToString
                'msg.Send()
            End If

        Catch ex As Exception
            ex.WriteToEventLog
            exp.WriteToEventLog
        Finally
            cn.Close()
        End Try
    End Sub

    ' writes a new entry to the servers event log
    <Extension()> Public Function WriteToEventLog(ByVal exp As Exception) As Boolean
        Return exp.WriteToEventLog
    End Function
    <Extension()> Public Function WriteToEventLog(ByVal exp As Exception, ByVal userID As Integer) As Boolean
        Try
            ''stop

            'Dim logName As String = System.Configuration.ConfigurationManager.AppSettings("EventLog")

            '' Create the source, if it does not already exist.
            'If Not EventLog.SourceExists("NK5" & logName) Then
            '    EventLog.CreateEventSource("NK5" & logName, logName)
            'End If

            '' Create an EventLog instance and assign its source.
            'Dim myLog As New EventLog()
            'myLog.Source = "NK5" & logName

            '' Write an informational entry to the event log.
            'Dim usr As SystemUser = GetUser(userID)
            'Dim strMsg As String = "Error occurred at " & Date.Now.ToString & ControlChars.NewLine

            'If (usr.ID > 0) Then
            '    strMsg &= "Current User: " & usr.Email & ControlChars.NewLine
            'End If
            'strMsg &= "Exception: " & exp.ToString

            'myLog.WriteEntry(strMsg, EventLogEntryType.Error)

            Return True

        Catch ex As Exception
            Return False
        End Try
    End Function

End Module
