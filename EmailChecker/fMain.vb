Imports System.Xml
Imports Ionic.Zip
Imports DailyDocket.CommonCore.Shared.Common

Public Class fMain
    Dim dtStartTime As DateTime

    Dim lastRunTime As DateTime
    Dim processRunning As Boolean

    'Dim oFtp As DailyDocket.CommonCore.Shared.Ftp

    Private Sub Form1_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        Me.lstLog.Items.Clear()
        Me.lblTotalTime.Text = "0"
        Me.txtError.Text = ""

        ' setup the ftp server
        'oFtp = New Ftp("ftp://ftp.dailycourtdocket.com/access.dailycourtdocket.com/wwwroot/", "wdivzylm_root", "9dR00g326d!")

        processRunning = False

        Me.lblVersion.Text = "Version: " & GetApplicationAssembly(Nothing).GetName.Version.ToString
        My.Application.DoEvents()

        ' autorun on load if we are on the server
        If My.Computer.Name.ToUpper.Contains("WIN-CQJRSQ0R80J") Then
            'Me.lstLog.LogFileProcessorHistory("Auto Run: " & Now.ToString)
            'Me.Process(True)
        End If
    End Sub

    Private Sub Process(ByVal fetchEmails As Boolean)
        Dim retry_location As String = ""
        processRunning = True

        lastRunTime = Now

        dtStartTime = Now
        Me.tmrTimer.Enabled = True

        Me.lblTotalTime.Text = "0"
        My.Application.DoEvents()

        Me.lblPrimary.Text = "0/0 (0.0%)"
        Me.lblSecondary.Text = "0/0 (0.0%)"

        files_tmp = "C:\inetpub\access.dailycourtdocket.com\wwwroot\files_tmp\"
        If Not My.Computer.FileSystem.DirectoryExists(files_tmp) Then My.Computer.FileSystem.CreateDirectory(files_tmp)

        Dim cn As New SqlClient.SqlConnection(ConnectionString)
        Dim cn1 As New SqlClient.SqlConnection(ConnectionString)
        Dim cn2 As New SqlClient.SqlConnection(ConnectionString)

        Dim retryCount As Integer = 0

        Try
            Dim q As Integer = cn.PacketSize

            If Me.chkAdminCalendars.Checked Then
                ' process the admin calendars first so we overwrite fewer existing criminal records (if any) with ones that have less detail
                Me.lstLog.LogFileProcessorHistory("Processing Administrative Calendars ...")

                Me.ProcessAdminCalendars()
            End If

            retryCount = 0
retry_connection_top:
            retry_location = "top"
            ' loop through the email accounts and save the attachments
            Dim cmd As New SqlClient.SqlCommand("SELECT [ID], [Email], [Password], [State], [Type] FROM [EmailAccounts] WHERE [Active] = 1 ORDER BY [State];", cn)
            If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
            Dim rs As SqlClient.SqlDataReader = cmd.ExecuteReader
            Do While rs.Read
                Me.lstLog.LogFileProcessorHistory("<strong>" & rs("Email").ToString & "</strong>")

                ' get a list of active counties for this account
                Dim activeCounties As String = ""
                retryCount = 0
retry_connection_counties:
                retry_location = "counties"
                Dim cmd2 As New SqlClient.SqlCommand("SELECT [County] FROM [Counties] WHERE [EmailAccountID] = " & rs("ID").ToString & " AND [Active] = 1;", cn2)
                If cmd2.Connection.State = ConnectionState.Closed Then cmd2.Connection.Open()
                Dim rs2 As SqlClient.SqlDataReader = cmd2.ExecuteReader
                Do While rs2.Read
                    If activeCounties = "" Then activeCounties = rs2("County").ToString Else activeCounties &= "," & rs2("County").ToString
                Loop
                cmd2.Cancel()
                rs2.Close()

                Me.lstLog.LogFileProcessorHistory("Checking Messages ...")

                ' create a state folder to store all the attachments in
                Dim state_tmp As String = My.Computer.FileSystem.CombinePath(files_tmp, rs("State").ToString)
                If Not My.Computer.FileSystem.DirectoryExists(state_tmp) Then My.Computer.FileSystem.CreateDirectory(state_tmp)

                If fetchEmails Then
                    Dim processedMessages As Integer = 0
                    Dim processedMessageIDs As New List(Of Integer)

                    Dim client As OpenPop.Pop3.Pop3Client = New OpenPop.Pop3.Pop3Client()
                    client.Connect("mail.solvtopia.com", 110, False)
                    client.Authenticate(rs("Email").ToString, rs("Password").ToString)

                    Dim messageCount As Integer = client.GetMessageCount()
                    Dim allMessages As List(Of OpenPop.Mime.Message) = New List(Of OpenPop.Mime.Message)(messageCount)
                    For count As Integer = 1 To messageCount
                        Dim msg As OpenPop.Mime.Message = client.GetMessage(count)

                        ' make sure the message is in the active county list
                        For Each s As String In activeCounties.Split(","c)
                            If msg.ToMailMessage.Subject.ToLower.Contains(s.ToLower) Then
                                processedMessageIDs.Add(count)

                                Dim attachments = msg.FindAllAttachments
                                For Each att In attachments
                                    If att.FileName.ToLower.EndsWith(".zip") Then
                                        Dim thefile = att.FileName
                                        Dim filetype = att.ContentType
                                        Dim contentid = att.ContentId

                                        Dim tmpPath As String = My.Computer.FileSystem.CombinePath(state_tmp, thefile)
                                        If My.Computer.FileSystem.FileExists(tmpPath) Then My.Computer.FileSystem.DeleteFile(tmpPath)
                                        System.IO.File.WriteAllBytes(tmpPath, att.Body)

                                        processedMessages += 1
                                    End If

                                    My.Application.DoEvents()
                                Next

                                Exit For
                            End If

                            My.Application.DoEvents()
                        Next

                        Me.pbrPrimary.Maximum = messageCount
                        If count <= Me.pbrPrimary.Maximum Then Me.pbrPrimary.Value = count
                        Me.lblPrimary.Text = FormatNumber(count, 0) & "/" & FormatNumber(messageCount, 0) & " (" & FormatNumber((count / messageCount) * 100, 1) & "%)"

                        My.Application.DoEvents()
                    Next

                    Me.lstLog.LogFileProcessorHistory(processedMessages & " Message(s) Downloaded ...")
                    Me.lstLog.LogFileProcessorHistory("Processing Zip File(s) ...")

                    ' delete all messages we processed from the account after processing
                    For Each i As Integer In processedMessageIDs
                        client.DeleteMessage(i)
                    Next

                    client.Disconnect()
                    client.Dispose()
                End If

                ' extract the zip files
                Dim processedZipFiles As Integer = 0
                Dim fileEntries As List(Of IO.FileInfo) = SearchDir(state_tmp, "*.zip", DailyDocket.CommonCore.Shared.Enums.FileSort.Name)
                For Each fi As IO.FileInfo In fileEntries
                    Try
                        Using zip As ZipFile = ZipFile.Read(fi.FullName)
                            zip.ExtractAll(state_tmp, ExtractExistingFileAction.OverwriteSilently)
                        End Using

                        My.Computer.FileSystem.DeleteFile(fi.FullName)

                    Catch ex As Exception
                        My.Computer.FileSystem.RenameFile(fi.FullName, fi.Name & ".skipped")
                        Me.txtError.Text &= ex.Message & vbCrLf
                        Me.txtError.Text &= ex.ToString & vbCrLf & vbCrLf & vbCrLf
                    End Try

                    processedZipFiles += 1

                    My.Application.DoEvents()
                Next

                Me.lstLog.LogFileProcessorHistory(processedZipFiles & " Zip File(s) Processed ...")
                Me.lstLog.LogFileProcessorHistory("Processing Text File(s) ...")

                ' find all the text files and process them
                Dim processedTextFiles As Integer = 0
                fileEntries = SearchDir(state_tmp, "*.txt", DailyDocket.CommonCore.Shared.Enums.FileSort.Size)
                For Each fi As IO.FileInfo In fileEntries
                    Me.pbrPrimary.Maximum = fileEntries.Count
                    If processedTextFiles <= Me.pbrPrimary.Maximum Then Me.pbrPrimary.Value = processedTextFiles
                    Me.lblPrimary.Text = FormatNumber(processedTextFiles, 0) & "/" & FormatNumber(fileEntries.Count, 0) & " (" & FormatNumber((processedTextFiles / fileEntries.Count) * 100, 1) & "%)"
                    My.Application.DoEvents()

                    Dim fa() As String = fi.FullName.Replace(files_tmp, "").Split("\"c)

                    Dim sCounty As String = fi.FullName.Replace(files_tmp, "").Split("\"c)(1)
                    Dim sType As String = fi.FullName.Replace(files_tmp, "").Split("\"c)(2)
                    Dim sDate As String = ""

                    If sType.ToLower = rs("Type").ToString.ToLower Then
                        If sType.ToLower = "civil" Then
                            sDate = fi.FullName.Replace(files_tmp, "").Split("\"c)(3)
                        ElseIf sType.ToLower = "criminal" Then
                            sDate = fi.Name.Split("."c)(2) & "." & fi.Name.Split("."c)(3) & "." & fi.Name.Split("."c)(4)
                        End If

                        ' process only the active counties
                        If activeCounties.ToLower.Contains(sCounty.ToLower) Then
                            Dim lst As New List(Of FileRecord)
                            Select Case True
                                Case rs("State").ToString.ToLower = "nc" And sType.ToLower = "civil"
                                    lst = Processor.NCCivilFile(fi.FullName)
                                Case rs("State").ToString.ToLower = "nc" And sType.ToLower = "criminal"
                                    lst = Processor.NCCriminalFile(fi.FullName)
                            End Select

                            Me.lstLog.LogFileProcessorHistory("Processing file " & processedTextFiles + 1 & " of " & fileEntries.Count & " with " & FormatNumber(lst.Count, 0) & " " & If(lst.Count > 1, "records", "record") & " ...")

                            ' loop through the records we got and save them to the database using the api
                            Dim recordsProcessed As Integer = 0
                            If lst.Count > 0 Then
                                For Each fr As FileRecord In lst
                                    Me.pbrSecondary.Maximum = lst.Count
                                    If recordsProcessed <= Me.pbrSecondary.Maximum Then Me.pbrSecondary.Value = recordsProcessed
                                    Me.lblSecondary.Text = FormatNumber(recordsProcessed, 0) & "/" & FormatNumber(lst.Count, 0) & " (" & FormatNumber((recordsProcessed / lst.Count) * 100, 1) & "%)"
                                    My.Application.DoEvents()

                                    Dim caseReturn As RecordReturn = Me.SaveRecord(fr, False)

                                    Dim cnClient As New SqlClient.SqlConnection(ConnectionString)
                                    Dim cmdClient As New SqlClient.SqlCommand

                                    Try
                                        If fr.RecordType.ToLower = "civil" Then
                                            ' lookup and save the attorneys for civil
                                            For Each s As String In fr.DefendantAttorney
                                                If s.Trim <> "" And s.Trim.Contains(",") Then
                                                    Dim attorneyReturn As RecordReturn = Me.SaveAttorney(s, rs("State").ToString, caseReturn.ID)
                                                End If

                                                My.Application.DoEvents()
                                            Next

                                            For Each s As String In fr.PlaintiffAttorney
                                                If s.Trim <> "" And s.Trim.Contains(",") Then
                                                    Dim attorneyReturn As RecordReturn = Me.SaveAttorney(s, rs("State").ToString, caseReturn.ID)
                                                End If

                                                My.Application.DoEvents()
                                            Next

                                        ElseIf fr.RecordType.ToLower = "criminal" Then

                                            ' lookup and save the attorneys for criminal
                                            For Each s As String In fr.DefendantAttorney
                                                If s.Trim <> "" And s.Trim.Contains(",") Then
                                                    Dim attorneyReturn As RecordReturn = Me.SaveAttorney(s, rs("State").ToString, caseReturn.ID)
                                                End If

                                                My.Application.DoEvents()
                                            Next
                                        End If

                                    Catch ex As Exception
                                        ex.WriteToErrorLog(New ErrorLogEntry(DailyDocket.CommonCore.Shared.Enums.ProjectName.FileProcessor))
                                        Me.txtError.Text &= ex.Message & vbCrLf
                                        Me.txtError.Text &= ex.ToString & vbCrLf & vbCrLf & vbCrLf
                                    Finally
                                        cnClient.Close()
                                    End Try

                                    recordsProcessed += 1

                                    If recordsProcessed <= Me.pbrSecondary.Maximum Then Me.pbrSecondary.Value = recordsProcessed
                                    Me.lblSecondary.Text = FormatNumber(recordsProcessed, 0) & "/" & FormatNumber(lst.Count, 0) & " (" & FormatNumber((recordsProcessed / lst.Count) * 100, 1) & "%)"
                                    My.Application.DoEvents()
                                Next

                                Try
                                    If My.Computer.FileSystem.FileExists(fi.FullName.Replace(".txt", ".txt.processed")) Then
                                        My.Computer.FileSystem.DeleteFile(fi.FullName.Replace(".txt", ".txt.processed"))
                                    End If
                                    My.Computer.FileSystem.RenameFile(fi.FullName, fi.Name.Replace(".txt", ".txt.processed"))

                                Catch ex As Exception
                                    ex.WriteToErrorLog(New ErrorLogEntry(DailyDocket.CommonCore.Shared.Enums.ProjectName.FileProcessor))
                                    Me.txtError.Text &= ex.Message & vbCrLf
                                    Me.txtError.Text &= ex.ToString & vbCrLf & vbCrLf & vbCrLf
                                End Try
                            Else
                                Try
                                    If My.Computer.FileSystem.FileExists(fi.FullName.Replace(".txt", ".txt.norecords")) Then
                                        My.Computer.FileSystem.DeleteFile(fi.FullName.Replace(".txt", ".txt.norecords"))
                                    End If
                                    My.Computer.FileSystem.RenameFile(fi.FullName, fi.Name.Replace(".txt", ".txt.norecords"))
                                Catch ex As Exception
                                    ex.WriteToErrorLog(New ErrorLogEntry(DailyDocket.CommonCore.Shared.Enums.ProjectName.FileProcessor))
                                    Me.txtError.Text &= ex.Message & vbCrLf
                                    Me.txtError.Text &= ex.ToString & vbCrLf & vbCrLf & vbCrLf
                                End Try
                            End If

                            processedTextFiles += 1

                            If processedTextFiles <= Me.pbrPrimary.Maximum Then Me.pbrPrimary.Value = processedTextFiles
                            Me.lblPrimary.Text = FormatNumber(processedTextFiles, 0) & "/" & FormatNumber(fileEntries.Count, 0) & " (" & FormatNumber((processedTextFiles / fileEntries.Count) * 100, 1) & "%)"
                            My.Application.DoEvents()
                        End If
                    End If

                    My.Application.DoEvents()
                Next

                Me.lstLog.LogFileProcessorHistory(processedTextFiles & " Text File(s) Processed ...")

                'Me.ProcessAllFiles(files_tmp, rs("County").ToString, rs("State").ToString)

                My.Application.DoEvents()
            Loop
            cmd.Cancel()
            rs.Close()

            Me.lstLog.LogFileProcessorHistory("Updating Xml IDs ...")

            Try
                cmd = New SqlClient.SqlCommand("EXEC [procRefreshAttorneyIDs];", cn)
                If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
                cmd.ExecuteNonQuery()

            Catch ex As Exception
                ex.WriteToErrorLog(New ErrorLogEntry(DailyDocket.CommonCore.Shared.Enums.ProjectName.FileProcessor))
                Me.txtError.Text &= ex.Message & vbCrLf
                Me.txtError.Text &= ex.ToString & vbCrLf & vbCrLf & vbCrLf
            End Try

            Me.lstLog.LogFileProcessorHistory("Processing Null Records ...")
            Me.ProcessNulls()

            If Me.chkAutoArchive.Checked Then
                Me.lstLog.LogFileProcessorHistory("Archiving Old Cases ...")
                Me.ArchiveCases()
            End If


            If Me.chkNotifications.Checked Then
                ' send all the text messages we saved to the list
                Me.lstLog.LogFileProcessorHistory("Sending Notifications ...")

                Dim messagesSent As Integer = Me.ProcessNotifications(dtStartTime)

                Me.lstLog.LogFileProcessorHistory(messagesSent & " Notification(s) Sent ...")
            End If

            'me.lstLog.LogFileProcessorHistory(messagesSent & " Text Message(s) Sent ...")
            Me.lstLog.LogFileProcessorHistory("<strong>Finished In " & Me.lblTotalTime.Text & "</strong>")

            Me.tmrTimer.Enabled = False

            processRunning = False

        Catch ex As Exception
            If retryCount <= 10 Then
                Me.txtError.Text &= ex.Message & vbCrLf
                Me.txtError.Text &= ex.ToString & vbCrLf & vbCrLf & vbCrLf
                retryCount += 1
                ' wait 1 second before retrying ...
                'System.Threading.Thread.Sleep(1000)
                Select Case retry_location.ToLower
                    Case "top" : GoTo retry_connection_top
                    Case "counties" : GoTo retry_connection_counties
                End Select
            Else
                ex.WriteToErrorLog(New ErrorLogEntry(DailyDocket.CommonCore.Shared.Enums.ProjectName.FileProcessor))
                Me.txtError.Text &= ex.Message & vbCrLf
                Me.txtError.Text &= ex.ToString & vbCrLf & vbCrLf & vbCrLf
            End If
        Finally
            cn.Close()
            cn1.Close()
            cn2.Close()
        End Try
    End Sub

    Private Sub CheckProcessed()
        Try
            files_tmp = "C:\inetpub\access.dailycourtdocket.com\wwwroot\files_tmp\"

            Dim lst As List(Of IO.FileInfo) = SearchDir(files_tmp, "*.txt", DailyDocket.CommonCore.Shared.Enums.FileSort.Name)

            Me.pbrPrimary.Maximum = lst.Count
            Me.pbrPrimary.Value = 0
            My.Application.DoEvents()

            For Each fi As IO.FileInfo In lst
                If My.Computer.FileSystem.FileExists(fi.FullName.Replace(".txt", ".txt.processed")) Then My.Computer.FileSystem.DeleteFile(fi.FullName)

                Me.pbrPrimary.Value += 1
                Me.lblPrimary.Text = FormatNumber(Me.pbrPrimary.Value, 0) & "/" & FormatNumber(lst.Count, 0) & " (" & FormatNumber((Me.pbrPrimary.Value / lst.Count) * 100, 1) & "%)"
                My.Application.DoEvents()
            Next

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(DailyDocket.CommonCore.Shared.Enums.ProjectName.FileProcessor))
            Me.txtError.Text &= ex.Message & vbCrLf
            Me.txtError.Text &= ex.ToString & vbCrLf & vbCrLf & vbCrLf
        End Try
    End Sub

    Public Function ProcessAdminCalendars() As Integer
        'Me.tmrAutoRun.Enabled = False

        Dim retVal As Integer = 0

        If files_tmp Is Nothing OrElse files_tmp = "" Then files_tmp = "C:\inetpub\access.dailycourtdocket.com\wwwroot\files_tmp\"

        Dim processedPdfFiles As Integer = 0

        Dim cn As New SqlClient.SqlConnection(ConnectionString)

        Dim retryCount As Integer = 0

        Try
            Dim lstDownloads As New List(Of String)

tryCountiesAgain:
            Dim cmd As New SqlClient.SqlCommand("SELECT [County], [State], [Type], [UrlStart] FROM [AdminCalendars] WHERE [Active] = 1;", cn)
            If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
            Dim rs As SqlClient.SqlDataReader = cmd.ExecuteReader
            Do While rs.Read
                ' fix any county names that don't match what is already in the database
                Dim sCounty As String = rs("County").ToString
                Select Case sCounty.ToLower
                    Case "newhanover" : sCounty = "NEW_HANOVER"
                End Select

                ' get the url text to find what calendars we have
                Dim urlText As String = ""
                Select Case rs("State").ToString.ToLower
                    Case "nc"
                        Dim urlStart As String = rs("urlStart").ToString & rs("County").ToString
                        urlText = ScrapeUrl(urlStart & "/Calendars.asp", DailyDocket.CommonCore.Shared.Enums.ScrapeType.KeepTags)

                        ' find the links in the url text that point to the calendar files
                        For Each l As String In urlText.Split(CChar(vbCrLf))
                            If l.Trim.ToLower.Contains("<a href=""/county/" & rs("County").ToString.ToLower & "/calendars/admin") Then
                                ' this is a link line for the calendar ... add it to the list for downloads
                                Dim tmp As String = l.Trim.Substring(l.Trim.IndexOf("<a href=""") + 9, l.Trim.IndexOf(".pdf") - 9)
                                tmp = tmp.ToLower.Replace("/county/" & rs("County").ToString.ToLower, urlStart)
                                If tmp.ToLower.EndsWith(".pdf") And Not lstDownloads.Contains(tmp) Then
                                    lstDownloads.Add(tmp)
                                End If
                            End If

                            My.Application.DoEvents()
                        Next

                        ' download the pdf files to the temp folder
                        For Each d As String In lstDownloads
                            Dim tmpPath As String = My.Computer.FileSystem.CombinePath(files_tmp, rs("State").ToString & "\" & sCounty & "\admin")
                            If Not My.Computer.FileSystem.DirectoryExists(tmpPath) Then My.Computer.FileSystem.CreateDirectory(tmpPath)
                            tmpPath = My.Computer.FileSystem.CombinePath(tmpPath, d.Substring(d.LastIndexOf("/") + 1))
                            'If My.Computer.FileSystem.FileExists(tmpPath) Then My.Computer.FileSystem.DeleteFile(tmpPath)
                            If Not My.Computer.FileSystem.FileExists(tmpPath.Replace(".pdf", ".pdf.processed")) Then My.Computer.Network.DownloadFile(d, tmpPath)

                            My.Application.DoEvents()
                        Next

                        ' process the pdf files
                        Dim state_tmp As String = My.Computer.FileSystem.CombinePath(files_tmp, rs("State").ToString)
                        Dim fileEntries As List(Of IO.FileInfo) = SearchDir(state_tmp, "*.pdf", DailyDocket.CommonCore.Shared.Enums.FileSort.Name)
                        For Each fi As IO.FileInfo In fileEntries
                            Me.pbrPrimary.Maximum = fileEntries.Count
                            If processedPdfFiles <= Me.pbrPrimary.Maximum Then Me.pbrPrimary.Value = processedPdfFiles
                            Me.pbrPrimary.Text = FormatNumber(processedPdfFiles, 0) & "/" & FormatNumber(fileEntries.Count, 0) & " (" & FormatNumber((processedPdfFiles / fileEntries.Count) * 100, 1) & "%)"
                            My.Application.DoEvents()

                            ' fix any county names that don't match what is already in the database
                            sCounty = fi.FullName.Replace(files_tmp, "").Split("\"c)(1)

                            'Dim ftpFileName As String = ""
                            'ftpFileName = "files_tmp/" & rs("State").ToString & "/" & sCounty & "/admin/" & fi.Name.Replace(".pdf", ".pdf.processed")

                            Dim lst As New List(Of FileRecord)
                            Select Case True
                                Case rs("State").ToString.ToLower = "nc" And rs("Type").ToString.ToLower = "criminal"
                                    lst = Processor.NCCriminalAdminCalendar(fi.FullName)
                            End Select

                            Me.lstLog.LogFileProcessorHistory("Processing file " & processedPdfFiles + 1 & " of " & fileEntries.Count & " with " & FormatNumber(lst.Count, 0) & " " & If(lst.Count > 1, "records", "record") & " ...")

                            ' loop through the records we got and save them to the database using the api
                            Dim recordsProcessed As Integer = 0
                            For Each fr As FileRecord In lst
                                Me.pbrSecondary.Maximum = lst.Count
                                If recordsProcessed <= Me.pbrSecondary.Maximum Then Me.pbrSecondary.Value = recordsProcessed
                                Me.lblSecondary.Text = FormatNumber(recordsProcessed, 0) & "/" & FormatNumber(lst.Count, 0) & " (" & FormatNumber((recordsProcessed / lst.Count) * 100, 1) & "%)"
                                My.Application.DoEvents()

                                Dim caseReturn As RecordReturn = Me.SaveRecord(fr, True)

                                If caseReturn.ID > 0 Then
                                    Try
                                        If fr.RecordType.ToLower = "criminal" Then

                                            ' lookup and save the attorneys for criminal
                                            For Each s As String In fr.DefendantAttorney
                                                If s.Trim <> "" And s.Trim.Contains(",") Then
                                                    Dim attorneyReturn As RecordReturn = Me.SaveAttorney(s, rs("State").ToString, caseReturn.ID)
                                                End If

                                                My.Application.DoEvents()
                                            Next
                                        End If

                                    Catch ex As Exception
                                        ex.WriteToErrorLog(New ErrorLogEntry(DailyDocket.CommonCore.Shared.Enums.ProjectName.FileProcessor))
                                        Me.txtError.Text &= ex.Message & vbCrLf
                                        Me.txtError.Text &= ex.ToString & vbCrLf & vbCrLf & vbCrLf
                                    End Try
                                End If

                                recordsProcessed += 1

                                If recordsProcessed <= Me.pbrSecondary.Maximum Then Me.pbrSecondary.Value = recordsProcessed
                                Me.lblSecondary.Text = FormatNumber(recordsProcessed, 0) & "/" & FormatNumber(lst.Count, 0) & " (" & FormatNumber((recordsProcessed / lst.Count) * 100, 1) & "%)"
                                My.Application.DoEvents()
                            Next

                            If My.Computer.FileSystem.FileExists(fi.FullName.Replace(".pdf", ".pdf.processed")) Then
                                My.Computer.FileSystem.DeleteFile(fi.FullName.Replace(".pdf", ".pdf.processed"))
                            End If
                            My.Computer.FileSystem.RenameFile(fi.FullName, fi.Name.Replace(".pdf", ".pdf.processed"))

                            processedPdfFiles += 1

                            If processedPdfFiles <= Me.pbrPrimary.Maximum Then Me.pbrPrimary.Value = processedPdfFiles
                            Me.lblPrimary.Text = FormatNumber(processedPdfFiles, 0) & "/" & FormatNumber(fileEntries.Count, 0) & " (" & FormatNumber((processedPdfFiles / fileEntries.Count) * 100, 1) & "%)"

                            My.Application.DoEvents()
                        Next
                End Select

                Me.lstLog.LogFileProcessorHistory(processedPdfFiles & " PDF File(s) Processed ...")

                My.Application.DoEvents()
            Loop
            cmd.Cancel()
            rs.Close()

        Catch ex As Exception
            If retryCount <= 10 Then
                retryCount += 1
                ' wait 1 second before retrying ...
                'System.Threading.Thread.Sleep(1000)
                GoTo tryCountiesAgain
            Else
                ex.WriteToErrorLog(New ErrorLogEntry(DailyDocket.CommonCore.Shared.Enums.ProjectName.FileProcessor))
                Me.txtError.Text &= ex.Message & vbCrLf
                Me.txtError.Text &= ex.ToString & vbCrLf & vbCrLf & vbCrLf
            End If
        Finally
            cn.Close()
        End Try

        retVal = processedPdfFiles

        Return retVal
    End Function

    Private Sub RenameProcessedFiles()
        Dim lst As List(Of IO.FileInfo) = SearchDir("c:\files_tmp", "*.processed", DailyDocket.CommonCore.Shared.Enums.FileSort.Name)

        Me.pbrPrimary.Maximum = lst.Count
        Me.pbrPrimary.Value = 0
        My.Application.DoEvents()

        For Each fi As IO.FileInfo In lst
            Try
                If My.Computer.FileSystem.FileExists(fi.FullName.Replace(".txt.processed", ".txt")) Then
                    My.Computer.FileSystem.DeleteFile(fi.FullName.Replace(".txt.processed", ".txt"))
                End If

                My.Computer.FileSystem.RenameFile(fi.FullName, fi.Name.Replace(".txt.processed", ".txt"))

                Me.pbrPrimary.Value += 1
                My.Application.DoEvents()

            Catch ex As Exception
                Dim s As String = ""
            End Try
        Next
    End Sub

    Public Function ProcessNotifications(ByVal startDate As DateTime) As Integer
        Dim retVal As Integer = 0

        Dim lstMessages As New List(Of NotificationMessage)
        Dim retryCount As Integer = 0

        Dim cn As New SqlClient.SqlConnection(ConnectionString)
        Dim cn1 As New SqlClient.SqlConnection(ConnectionString)
        Dim cn2 As New SqlClient.SqlConnection(ConnectionString)

        Try
            ' build a list of messages to send
retry_users:
            Dim cmdUsers As New SqlClient.SqlCommand("SELECT [ID], [xAttorneyID], [xEmail] FROM [Users] WHERE [ID] > 1 AND [xClientID] > 0 AND [Active] = 1;", cn)
            If cmdUsers.Connection.State = ConnectionState.Closed Then cmdUsers.Connection.Open()
            Dim rsUsers As SqlClient.SqlDataReader = cmdUsers.ExecuteReader
            Do While rsUsers.Read
                Dim usr As New SystemUser(rsUsers("ID").ToString.ToInteger)

                If Not usr.BillingLock Then
                    Dim ar As New AttorneyRecord(usr.AttorneyID)

                    Dim lstAttorneyMessages As New List(Of NotificationMessage)
                    Dim lstClientMessages As New List(Of NotificationMessage)

retry_cases:
                    Try
                        retryCount = 0
                        Dim cmdCases As New SqlClient.SqlCommand("procSMSQueue", cn1)
                        cmdCases.CommandType = CommandType.StoredProcedure
                        cmdCases.Parameters.AddWithValue("@startDate", startDate)
                        cmdCases.Parameters.AddWithValue("@attorneyID", rsUsers("xAttorneyID").ToString)
                        If cmdCases.Connection.State = ConnectionState.Closed Then cmdCases.Connection.Open()
                        Dim rsCases As SqlClient.SqlDataReader = cmdCases.ExecuteReader
                        Do While rsCases.Read
                            Dim fr As New FileRecord(rsCases("ID").ToString.ToInteger)

                            Dim aType As DailyDocket.CommonCore.Shared.Enums.AttorneyType = fr.AttorneyType(ar.AllNames)

                            Dim names As String = ""
                            If aType = DailyDocket.CommonCore.Shared.Enums.AttorneyType.Defendant Then
                                For Each n As String In fr.Defendant
                                    If Not names.ToLower.Contains(FormatName(n).ToLower) Then If names = "" Then names = FormatName(n) Else names &= ", " & FormatName(n)
                                Next
                            ElseIf aType = DailyDocket.CommonCore.Shared.Enums.AttorneyType.Plaintiff Then
                                For Each n As String In fr.Plaintiff
                                    If Not names.ToLower.Contains(FormatName(n).ToLower) Then If names = "" Then names = FormatName(n) Else names &= ", " & FormatName(n)
                                Next
                            End If

                            Dim location As String = ""
                            If fr.CourtRoomLocation IsNot Nothing Then
                                location = If(fr.CourtRoomLocation.Trim = "", fr.County & " County", fr.CourtRoomLocation)
                            Else location = fr.County.Replace("_", " ") & " County"
                            End If
                            location = StrConv(location, VbStrConv.ProperCase)

                            Dim aMessage As String = ""
                            Dim cMessage As String = ""
                            If CDate(rsCases("dtUpdated")) > CDate(rsCases("dtInserted")) Then
                                ' updated
                                aMessage = "Case Number " & fr.CaseNumber.FixCaseNumber & " for " & names & " has been updated on your calendar."
                                cMessage = "Your case has been updated. You are now scheduled to appear in Court Room " & fr.CourtRoomNumber & ", in " & location & ", on " & FormatDateTime(fr.SessionDate, DateFormat.LongDate) & " at " & FormatDateTime(fr.SessionDate, DateFormat.LongTime)
                            Else
                                ' new
                                aMessage = "Case Number " & fr.CaseNumber.FixCaseNumber & " for " & names & " has been added to your calendar."
                                cMessage = "You are scheduled to appear in Court Room " & fr.CourtRoomNumber & ", in " & location & ", on " & FormatDateTime(fr.SessionDate, DateFormat.LongDate) & " at " & FormatDateTime(fr.SessionDate, DateFormat.LongTime)
                            End If

                            ' add the attorney messages to the list
                            If usr.MobileNumbers.Count > 0 Then
                                For Each n As String In usr.MobileNumbers
                                    If n.Trim <> "" Then
                                        Dim txt As New NotificationMessage
                                        txt.CaseNumber = fr.CaseNumber.FixCaseNumber
                                        txt.AttorneyId = rsUsers("xAttorneyID").ToString.ToInteger
                                        txt.ClientId = 0
                                        txt.Message = aMessage
                                        txt.ToNumber = n.FixPhoneNumber
                                        txt.Type = DailyDocket.CommonCore.Shared.Enums.NotificationRecipientType.Attorney
                                        txt.ToEmail = ""
                                        txt.MessageType = DailyDocket.CommonCore.Shared.Enums.NotificationMessageType.SMS
                                        txt.BatchRunDate = startDate
                                        lstAttorneyMessages.Add(txt)
                                    End If
                                Next
                            Else
                                ' no mobile numbers so use only email
                                Dim txt As New NotificationMessage
                                txt.CaseNumber = fr.CaseNumber.FixCaseNumber
                                txt.AttorneyId = rsUsers("xAttorneyID").ToString.ToInteger
                                txt.ClientId = 0
                                txt.Message = aMessage
                                txt.ToNumber = ""
                                txt.Type = DailyDocket.CommonCore.Shared.Enums.NotificationRecipientType.Attorney
                                txt.ToEmail = rsUsers("xEmail").ToString
                                txt.MessageType = DailyDocket.CommonCore.Shared.Enums.NotificationMessageType.Email
                                txt.BatchRunDate = startDate
                                lstAttorneyMessages.Add(txt)
                            End If

                            ' add the client messages to the list
retry_clients:
                            retryCount = 0
                            Try
                                Dim cmd As New SqlClient.SqlCommand("SELECT [ID], [xMobileNumber], [xNameFromFile] FROM [ClientRecords] WHERE [xAttorneyID] = " & rsUsers("xAttorneyID").ToString & ";", cn2)
                                If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
                                Dim rs As SqlClient.SqlDataReader = cmd.ExecuteReader
                                Do While rs.Read
                                    If fr.Defendant.Contains(rs("xNameFromFile").ToString) OrElse fr.Plaintiff.Contains(rs("xNameFromFile").ToString) Then
                                        If Not IsDBNull(rs("xMobileNumber")) Then
                                            Dim txt As New NotificationMessage
                                            txt.CaseNumber = fr.CaseNumber.FixCaseNumber
                                            txt.AttorneyId = rsUsers("xAttorneyID").ToString.ToInteger
                                            txt.ClientId = rs("ID").ToString.ToInteger
                                            txt.Message = cMessage
                                            txt.ToNumber = rs("xMobileNumber").ToString.FixPhoneNumber
                                            txt.Type = DailyDocket.CommonCore.Shared.Enums.NotificationRecipientType.Client
                                            txt.ToEmail = ""
                                            txt.MessageType = DailyDocket.CommonCore.Shared.Enums.NotificationMessageType.SMS
                                            txt.BatchRunDate = startDate
                                            lstClientMessages.Add(txt)
                                        End If
                                    End If

                                    My.Application.DoEvents()
                                Loop
                                cmd.Cancel()
                                rs.Close()

                            Catch ex As Exception
                                If retryCount <= 10 Then
                                    retryCount += 1
                                    ' wait 1 second before retrying ...
                                    'System.Threading.Thread.Sleep(1000)
                                    GoTo retry_clients
                                Else
                                    ex.WriteToErrorLog(New ErrorLogEntry(DailyDocket.CommonCore.Shared.Enums.ProjectName.FileProcessor))
                                    Me.txtError.Text &= ex.Message & vbCrLf
                                    Me.txtError.Text &= ex.ToString & vbCrLf & vbCrLf & vbCrLf
                                End If
                            End Try

                            My.Application.DoEvents()
                        Loop
                        cmdCases.Cancel()
                        rsCases.Close()

                    Catch ex As Exception
                        If retryCount <= 10 Then
                            retryCount += 1
                            ' wait 1 second before retrying ...
                            'System.Threading.Thread.Sleep(1000)
                            GoTo retry_cases
                        Else
                            ex.WriteToErrorLog(New ErrorLogEntry(DailyDocket.CommonCore.Shared.Enums.ProjectName.FileProcessor))
                            Me.txtError.Text &= ex.Message & vbCrLf
                            Me.txtError.Text &= ex.ToString & vbCrLf & vbCrLf & vbCrLf
                        End If
                    End Try

                    ' email notifications always get details
                    If usr.NotificationType = DailyDocket.CommonCore.Shared.Enums.NotificationType.Email Then
                        If lstAttorneyMessages.Count > 0 Then
                            Dim sBody As String = Mailer.GetHtmlBody(DailyDocket.CommonCore.Shared.Enums.EmailType.Client_Notification_Detail, lstAttorneyMessages)

                            Dim nMsg As New NotificationMessage
                            nMsg.CaseNumber = ""
                            nMsg.AttorneyId = usr.AttorneyID
                            nMsg.ClientId = 0
                            nMsg.Message = sBody
                            nMsg.ToNumber = ""
                            nMsg.ToEmail = If(usr.ID = 40, "james@solvtopia.com", usr.Email)
                            nMsg.MessageType = DailyDocket.CommonCore.Shared.Enums.NotificationMessageType.Email
                            nMsg.Type = DailyDocket.CommonCore.Shared.Enums.NotificationRecipientType.Attorney
                            nMsg.LogNotification

                            ' email get details only
                            Dim Mailmsg As New Mailer
                            Mailmsg.HostServer = "mail.solvtopia.com"
                            Mailmsg.UserName = "sales@solvtopia.com"
                            Mailmsg.Password = "Josie2005!"
                            Mailmsg.Port = 26
                            Mailmsg.To.Add(If(usr.ID = 40, "james@solvtopia.com", usr.Email))
                            Mailmsg.Body = sBody
                            Mailmsg.Subject = "Daily Docket - Calendar Updated"
                            Mailmsg.From = "sales@solvtopia.com"
                            Mailmsg.HtmlBody = True
                            Mailmsg.Send()
                            retVal += 1
                        End If

                    ElseIf usr.NotificationType = DailyDocket.CommonCore.Shared.Enums.NotificationType.SMS Then
                        If usr.NotificationLevel = DailyDocket.CommonCore.Shared.Enums.NotificationLevel.Detail Then
                            ' if the notifications top the max for the user switch to summary automatically
                            If lstAttorneyMessages.Count > (usr.MaxNotificationsPerBatch * usr.MobileNumbers.Count) Then
                                Dim count As Integer = lstAttorneyMessages.Count
                                lstAttorneyMessages = New List(Of NotificationMessage)

                                If count > 0 Then
                                    ' add the attorney messages to the list
                                    For Each n As String In usr.MobileNumbers
                                        If n.Trim <> "" Then
                                            Dim txt As New NotificationMessage
                                            txt.AttorneyId = rsUsers("xAttorneyID").ToString.ToInteger
                                            txt.ClientId = 0
                                            txt.Message = count & " Cases have been updated on or added to your calendar."
                                            txt.ToNumber = n.FixPhoneNumber
                                            txt.Type = DailyDocket.CommonCore.Shared.Enums.NotificationRecipientType.Attorney
                                            txt.ToEmail = ""
                                            txt.MessageType = DailyDocket.CommonCore.Shared.Enums.NotificationMessageType.SMS
                                            txt.BatchRunDate = startDate
                                            lstAttorneyMessages.Add(txt)
                                        End If
                                    Next
                                End If
                            End If

                            lstMessages.AddRange(lstAttorneyMessages)
                        ElseIf usr.NotificationLevel = DailyDocket.CommonCore.Shared.Enums.NotificationLevel.Summary Then
                            ' only send a summary message
                            Dim count As Integer = lstAttorneyMessages.Count
                            lstAttorneyMessages = New List(Of NotificationMessage)

                            If count > 0 Then
                                ' add the attorney messages to the list
                                For Each n As String In usr.MobileNumbers
                                    If n.Trim <> "" Then
                                        Dim txt As New NotificationMessage
                                        txt.AttorneyId = rsUsers("xAttorneyID").ToString.ToInteger
                                        txt.ClientId = 0
                                        txt.Message = count & " Cases have been updated on or added to your calendar."
                                        txt.ToNumber = n.FixPhoneNumber
                                        txt.Type = DailyDocket.CommonCore.Shared.Enums.NotificationRecipientType.Attorney
                                        txt.ToEmail = ""
                                        txt.MessageType = DailyDocket.CommonCore.Shared.Enums.NotificationMessageType.SMS
                                        txt.BatchRunDate = startDate
                                        lstAttorneyMessages.Add(txt)
                                    End If
                                Next
                            End If

                            lstMessages.AddRange(lstAttorneyMessages)
                        End If
                    End If

                    lstMessages.AddRange(lstClientMessages)
                End If

                My.Application.DoEvents()
            Loop
            cmdUsers.Cancel()
            rsUsers.Close()

            ' send the messages
            For Each msg As NotificationMessage In lstMessages
                Dim txt As String = msg.Message & " Thank you for choosing Daily Docket by Solvtopia, LLC!"

                Dim success As Boolean = Messaging.SendTwilioNotification(msg.ToNumber.FixPhoneNumber, txt)
                msg.LogNotification
                retVal += 1
            Next

        Catch ex As Exception
            If retryCount <= 10 Then
                retryCount += 1
                ' wait 1 second before retrying ...
                'System.Threading.Thread.Sleep(1000)
                GoTo retry_users
            Else
                ex.WriteToErrorLog(New ErrorLogEntry(DailyDocket.CommonCore.Shared.Enums.ProjectName.FileProcessor))
                Me.txtError.Text &= ex.Message & vbCrLf
                Me.txtError.Text &= ex.ToString & vbCrLf & vbCrLf & vbCrLf
            End If
        Finally
            cn.Close()
            cn1.Close()
            cn2.Close()
        End Try

        Return retVal
    End Function

    'Public Function MoveProcessedFiles(ByVal startDir As String) As Integer
    '    Dim retVal As Integer = 0

    '    Try
    '        Dim lst As List(Of IO.FileInfo) = SearchDir(startDir, "*.processed", DailyDocket.CommonCore.Shared.Enums.FileSort.Name)

    '        Me.pbrPrimary.Maximum = lst.Count
    '        Me.pbrPrimary.Value = 0
    '        My.Application.DoEvents()

    '        For Each fi As IO.FileInfo In lst
    '            Dim fa() As String = fi.FullName.Split("\"c)

    '            Dim sState As String = ""
    '            Dim sCounty As String = ""

    '            Dim sType As String = ""
    '            Select Case True
    '                Case fi.FullName.ToLower.Contains("\criminal\") : sType = "criminal"
    '                Case fi.FullName.ToLower.Contains("\civil\") : sType = "civil"
    '                Case fi.FullName.ToLower.Contains(".pdf") : sType = "admin"
    '            End Select

    '            ' create the directory structure on the web
    '            If Not oFtp.DirectoryExists("files_tmp") Then oFtp.CreateDirectory("files_tmp")
    '            If Not oFtp.DirectoryExists("files_tmp/" & sState) Then oFtp.CreateDirectory("files_tmp/" & sState)
    '            If Not oFtp.DirectoryExists("files_tmp/" & sState & "/" & sCounty) Then oFtp.CreateDirectory("files_tmp/" & sState & "/" & sCounty)
    '            If Not oFtp.DirectoryExists("files_tmp/" & sState & "/" & sCounty & "/" & sType) Then oFtp.CreateDirectory("files_tmp/" & sState & "/" & sCounty & "/" & sType)

    '            Dim ftpFileName As String = ""
    '            If sType.ToLower = "civil" Then
    '                Dim sDate As String = ""

    '                ' the civil path includes dates, criminal does not
    '                If Not oFtp.DirectoryExists("files_tmp/" & sState & "/" & sCounty & "/" & sType & "/" & sDate) Then oFtp.CreateDirectory("files_tmp/" & sState & "/" & sCounty & "/" & sType & "/" & sDate)

    '                ftpFileName = "files_tmp/" & sState & "/" & sCounty & "/" & sType & "/" & sDate & "/" & fi.Name
    '            ElseIf sType.ToLower = "criminal" Then
    '                ftpFileName = "files_tmp/" & sState & "/" & sCounty & "/" & sType & "/" & fi.Name
    '            ElseIf sType.ToLower = "admin" Then
    '                'If Not oFtp.DirectoryExists("files_tmp/" & sState & "/" & sCounty & "/" & "admin") Then oFtp.CreateDirectory("files_tmp/" & sState & "/" & sCounty & "/" & "admin")
    '                ftpFileName = "files_tmp/" & sState & "/" & sCounty & "/" & sType & "/" & fi.Name
    '            End If

    '            If oFtp.FileExists(ftpFileName) Then
    '                oFtp.DeleteFile(ftpFileName)
    '            End If

    '            If oFtp.CopyFile(fi.FullName, ftpFileName) Then My.Computer.FileSystem.DeleteFile(fi.FullName)

    '            Me.pbrPrimary.Value += 1
    '            My.Application.DoEvents()
    '        Next

    '        retVal = Me.pbrPrimary.Value

    '    Catch ex As Exception
    '        ex.WriteToErrorLog(New ErrorLogEntry(DailyDocket.CommonCore.Shared.Enums.ProjectName.FileProcessor))
    '    End Try

    '    Return retVal
    'End Function

    Public Function SaveAttorney(ByVal name As String, ByVal state As String, ByVal caseID As Integer) As RecordReturn
        Dim retVal As New RecordReturn

        If name.ContainsNumbers Then
            Do Until Not name.ContainsNumbers
                name = name.Substring(0, name.Length - 1)
            Loop
        End If
        name = name.Trim

        Dim ar As AttorneyRecord = LookupAttorney(name, state)
        Dim lstAKA As New List(Of String)

        If ar.NameFromFile.ToLower <> "pro, se" And ar.NameFromFile.ToLower <> "pro se" And ar.NameFromFile.ToLower <> "waived" And ar.NameFromFile.Contains(",") And ar.BarNumber <> "" Then
            ' check to see if we already have the attorney in the database
            ' if so, update the record, else add the record
            Dim cn As New SqlClient.SqlConnection(ConnectionString)

            Dim retryCount As Integer = 0

            Try
                If ar.AKA IsNot Nothing Then lstAKA = ar.AKA.ToList

retry_connection:
                Dim cmd As New SqlClient.SqlCommand("Select [ID], [xNameFromFile] FROM [Attorneys] WHERE ([xBarNumber] Like '" & ar.BarNumber & "' OR [xNameFromFile] LIKE '" & ar.NameFromFile.Replace("'", "") & "' OR [xAKA] LIKE '" & ar.NameFromFile.Replace("'", "") & "') AND [xState] LIKE '" & state & "';", cn)
                If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
                Dim rs As SqlClient.SqlDataReader = cmd.ExecuteReader
                If rs.Read Then
                    ar = New AttorneyRecord(rs("id").ToString.ToInteger)
                    ar.ID = CInt(rs("id").ToString)
                    If ar.AKA IsNot Nothing Then lstAKA = ar.AKA
                End If
                cmd.Cancel()
                rs.Close()

                If Not lstAKA.Contains(name) Then lstAKA.Add(name)

                ar.AKA = lstAKA

                If ar.ID = 0 Then
                    retVal.Type = Enums.RecordTransactionType.NewRecord
                Else retVal.Type = Enums.RecordTransactionType.Update
                End If

                retVal.ID = ar.Save.ID

                If retVal.ID > 0 And caseID > 0 Then
                    cmd = New SqlClient.SqlCommand("procSaveAttorneyCase", cn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.AddWithValue("@attorneyID", retVal.ID)
                    cmd.Parameters.AddWithValue("@caseID", caseID)
                    If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
                    cmd.ExecuteNonQuery()
                    cmd.Cancel()

                    ' copy attorney 63 records to our demo user 4896
                    If retVal.ID = 63 Then
                        cmd = New SqlClient.SqlCommand("procSaveAttorneyCase", cn)
                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Parameters.AddWithValue("@attorneyID", 4896)
                        cmd.Parameters.AddWithValue("@caseID", caseID)
                        If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
                        cmd.ExecuteNonQuery()
                        cmd.Cancel()
                    End If
                End If

            Catch ex As Exception
                If retryCount <= 10 Then
                    retryCount += 1
                    ' wait 1 second before retrying ...
                    'System.Threading.Thread.Sleep(1000)
                    GoTo retry_connection
                Else
                    ex.WriteToErrorLog(New ErrorLogEntry(DailyDocket.CommonCore.Shared.Enums.ProjectName.FileProcessor))
                    Me.txtError.Text &= ex.Message & vbCrLf
                    Me.txtError.Text &= ex.ToString & vbCrLf & vbCrLf & vbCrLf
                End If
            Finally
                cn.Close()
            End Try
        End If

        Return retVal
    End Function

    Public Function SaveRecord(ByVal fr As FileRecord, skipUpdates As Boolean) As RecordReturn
        Dim retVal As New RecordReturn

        Dim skipSave As Boolean = False

        If fr.CaseNumber <> "" Then
            fr.CaseNumber = fr.CaseNumber.FixCaseNumber

            ' check to see if we already have the case in the database
            ' if so, update the record, else add the record
            Dim cn As New SqlClient.SqlConnection(ConnectionString)

            Dim retryCount As Integer = 0

            Try
                If fr.SessionDate >= Now.AddDays(Me.txtArchiveDays.Text.ToInteger * -1) Then
retry_connection:
                    Dim sSource As String = ""

                    Dim cmd As New SqlClient.SqlCommand("SELECT [ID], [Source], [SessionDate] FROM [FileRecords] WHERE REPLACE(REPLACE([CaseNumber],'-',''),' ','') LIKE '" & fr.CaseNumber.FixCaseNumber & "' AND [State] LIKE '" & fr.State & "' AND [County] LIKE '" & fr.County & "' AND [RecordType] LIKE '" & fr.RecordType & "';", cn)
                    If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
                    Dim rs As SqlClient.SqlDataReader = cmd.ExecuteReader
                    If rs.Read Then
                        sSource = rs("Source").ToString

                        If CDate(rs("SessionDate").ToString) > fr.SessionDate Then
                            ' don't replace newer records
                            skipSave = True
                        Else
                            ' replace everything else including if the session date is the same
                            ' that way we get any updates to the record if there are any
                            fr.ID = CInt(rs("id").ToString)
                        End If
                    End If
                    cmd.Cancel()
                    rs.Close()

                    If Not skipSave Then
                        'Dim svc As New DocketInput.InputController
                        'Dim rq As New DocketInput.ApiRequest
                        'rq.apiKey = apiKey
                        Dim msg As String = ""
                        If fr.ID = 0 Then
                            retVal.Type = Enums.RecordTransactionType.NewRecord
                        Else retVal.Type = Enums.RecordTransactionType.Update
                        End If

                        Dim tmp As New FileRecord
                        If retVal.Type = Enums.RecordTransactionType.Update Then
                            If Not skipUpdates Or sSource.ToLower = "admincalendar" Then tmp = fr.Save
                        Else tmp = fr.Save
                        End If
                        retVal.ID = tmp.ID

                        If retVal.ID = 0 Then Dim s As String = ""

                        'svc.SendMessage(rq, msg, "", typ)
                    End If
                End If

            Catch ex As Exception
                If retryCount <= 10 Then
                    retryCount += 1
                    ' wait 1 second before retrying ...
                    'System.Threading.Thread.Sleep(1000)
                    GoTo retry_connection
                Else
                    ex.WriteToErrorLog(New ErrorLogEntry(DailyDocket.CommonCore.Shared.Enums.ProjectName.FileProcessor))
                    Me.txtError.Text &= ex.Message & vbCrLf
                    Me.txtError.Text &= ex.ToString & vbCrLf & vbCrLf & vbCrLf
                End If
            Finally
                cn.Close()
            End Try
        End If

        Return retVal
    End Function

    Public Sub SaveClient(ByVal name As String)
        Dim cn1 As New SqlClient.SqlConnection(ConnectionString)

        Try
            Dim firstName = "", middleName = "", lastName = "", extraName As String = ""
            Dim nameParts As List(Of String) = name.Split(","c).ToList
            If nameParts.Count >= 3 Then
                ' we have a first, middle, and last name
                firstName = nameParts(1).Replace("'", "")
                middleName = nameParts(2).Replace("'", "")
                lastName = nameParts(0).Replace("'", "")
                If nameParts.Count = 4 Then extraName = nameParts(3).Replace("'", "")

                'If middleName.Length = 1 Then middleName = ""
            ElseIf nameParts.Count = 2 Then
                ' we have a first, and last name
                ' make sure we don't have an initial for the middle name ... if so, remove it
                If nameParts(1).Contains(" ") Then nameParts(1) = nameParts(1).Substring(0, nameParts(1).IndexOf(" ")).Trim

                firstName = nameParts(1).Replace("'", "")
                lastName = nameParts(0).Replace("'", "")
            End If

            Dim cr As New ClientRecord
            cr.Active = True
            cr.NameFromFile = name
            If middleName = "" Then
                cr.Name = firstName & " " & lastName
            Else cr.Name = firstName & " " & middleName & " " & lastName
            End If
            If extraName <> "" Then cr.Name &= " " & extraName

            Dim cmd1 As New SqlClient.SqlCommand("SELECT [ID] FROM [ClientRecords] WHERE [xNameFromFile] LIKE '" & cr.NameFromFile.Replace("'", "") & "'", cn1)
            If cmd1.Connection.State = ConnectionState.Closed Then cmd1.Connection.Open()
            Dim rs1 As SqlClient.SqlDataReader = cmd1.ExecuteReader
            If rs1.Read Then
                cr.ID = CInt(rs1("id").ToString)
            End If
            cr.Save()

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(DailyDocket.CommonCore.Shared.Enums.ProjectName.FileProcessor))
            Me.txtError.Text &= ex.Message & vbCrLf
            Me.txtError.Text &= ex.ToString & vbCrLf & vbCrLf & vbCrLf
        Finally
            cn1.Close()
        End Try
    End Sub

    Private Sub ProcessAttorneys()
        Dim dStartTime As DateTime = Now

        Dim cn As New SqlClient.SqlConnection(ConnectionString)

        Try
            Dim cmd As New SqlClient.SqlCommand("SELECT [xNameFromFile], [xState] FROM [Attorneys] WHERE [xStatus] NOT LIKE 'active';", cn)
            If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
            Dim rs As SqlClient.SqlDataReader = cmd.ExecuteReader
            Do While rs.Read
                SaveAttorney(rs("xNameFromFile").ToString, rs("xState").ToString, 0)
            Loop
            cmd.Cancel()
            rs.Close()

            Dim time As TimeSpan = TimeSpan.FromSeconds(DateDiff(DateInterval.Second, dStartTime, Now))
            Dim str As String = time.ToString("hh\:mm\:ss\:fff")

            Me.lblTotalTime.Text = str & " to Process"
            My.Application.DoEvents()

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(DailyDocket.CommonCore.Shared.Enums.ProjectName.FileProcessor))
            Me.txtError.Text &= ex.Message & vbCrLf
            Me.txtError.Text &= ex.ToString & vbCrLf & vbCrLf & vbCrLf
        Finally
            cn.Close()
        End Try
    End Sub

    Private Sub ProcessAttorneyCases()
        Me.tmrAutoRun.Enabled = False

        Dim cn As New SqlClient.SqlConnection(ConnectionString)

        Try
            Dim lst As New List(Of String)

            Me.lblSecondary.Text = "0/0 (0.0%)"
            My.Application.DoEvents()

            Dim cmd As New SqlClient.SqlCommand("SELECT [ID], [xNameFromFile], [xmlData] FROM [Attorneys] ORDER BY [ID];", cn)
            If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
            Dim rs As SqlClient.SqlDataReader = cmd.ExecuteReader
            Do While rs.Read
                Dim xDoc As New XmlDocument
                xDoc.LoadXml(rs("xmlData").ToString)

                Dim s As String = ""
                If rs("xNameFromFile").ToString <> "" Then
                    s = rs("ID").ToString & "|" & rs("xNameFromFile").ToString
                End If

                If Not lst.Contains(s) Then lst.Add(s)

                If xDoc.Item("AttorneyRecord").Item("AKA") IsNot Nothing Then
                    For Each itm As XmlElement In xDoc.Item("AttorneyRecord").Item("AKA")
                        s = rs("ID").ToString & "|" & itm.InnerText
                        If Not lst.Contains(s) Then lst.Add(s)
                    Next
                End If

                My.Application.DoEvents()
            Loop
            cmd.Cancel()
            rs.Close()

            Me.pbrSecondary.Maximum = lst.Count
            Me.pbrSecondary.Value = 0
            Me.lblSecondary.Text = "0/" & FormatNumber(lst.Count, 0)
            My.Application.DoEvents()

            Dim x As Integer = 0

            For Each n As String In lst
                Dim aID As Long = CLng(n.Split("|"c)(0))
                Dim aName As String = n.Split("|"c)(1)

                If aName.Contains(",") Then
                    aName = aName.Replace("'", "")
                    cmd = New SqlClient.SqlCommand("SELECT [ID] FROM [FileRecords] WHERE [RecordData] LIKE '%" & aName & "%'", cn)
                    If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
                    rs = cmd.ExecuteReader
                    Do While rs.Read
                        Dim cn1 As New SqlClient.SqlConnection(ConnectionString)

                        Dim retryCount As Integer = 0

                        Try
retry_connection:
                            Dim cmd1 As New SqlClient.SqlCommand("procSaveAttorneyCase", cn1)
                            cmd1.CommandType = CommandType.StoredProcedure
                            cmd1.Parameters.AddWithValue("@attorneyID", aID)
                            cmd1.Parameters.AddWithValue("@caseID", rs("ID").ToString)
                            If cmd1.Connection.State = ConnectionState.Closed Then cmd1.Connection.Open()
                            cmd1.ExecuteNonQuery()
                            cmd1.Cancel()

                        Catch ex As Exception
                            If retryCount <= 10 Then
                                retryCount += 1
                                ' wait 1 second before retrying ...
                                'System.Threading.Thread.Sleep(1000)
                                GoTo retry_connection
                            Else
                                ex.WriteToErrorLog(New ErrorLogEntry(DailyDocket.CommonCore.Shared.Enums.ProjectName.FileProcessor))
                                Me.txtError.Text &= ex.Message & vbCrLf
                                Me.txtError.Text &= ex.ToString & vbCrLf & vbCrLf & vbCrLf
                            End If
                        Finally
                            cn1.Close()
                        End Try

                        My.Application.DoEvents()
                    Loop
                    cmd.Cancel()
                    rs.Close()
                End If

                x += 1
                If x <= Me.pbrSecondary.Maximum Then Me.pbrSecondary.Value = x
                Me.lblSecondary.Text = FormatNumber(x, 0) & "/" & FormatNumber(lst.Count, 0) & " (" & FormatNumber((x / lst.Count) * 100, 1) & "%)"
                My.Application.DoEvents()
            Next

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(DailyDocket.CommonCore.Shared.Enums.ProjectName.FileProcessor))
            Me.txtError.Text &= ex.Message & vbCrLf
            Me.txtError.Text &= ex.ToString & vbCrLf & vbCrLf & vbCrLf
        Finally
            cn.Close()
        End Try
    End Sub

    Private Sub GetAllAttorneyNames()
        Me.tmrAutoRun.Enabled = False

        dtStartTime = Now
        Me.tmrTimer.Enabled = True

        Me.lblTotalTime.Text = "0"
        My.Application.DoEvents()

        Me.lblPrimary.Text = "0/0 (0.0%)"
        Me.lblSecondary.Text = "0/0 (0.0%)"

        Me.lstLog.Items.Add("Connecting ...")
        Me.lstLog.SelectedIndex = Me.lstLog.Items.Count - 1
        My.Application.DoEvents()

        files_tmp = "C:\inetpub\access.dailycourtdocket.com\wwwroot\files_tmp\"
        If Not My.Computer.FileSystem.DirectoryExists(files_tmp) Then My.Computer.FileSystem.CreateDirectory(files_tmp)

        Dim cn As New SqlClient.SqlConnection(ConnectionString)
        Dim cn1 As New SqlClient.SqlConnection(ConnectionString)

        Try
            Dim fileEntries As New List(Of IO.FileInfo)

            ' find all the text files and process them
            Dim processedTextFiles As Integer = 0
            fileEntries = SearchDir(files_tmp, "*.processed", DailyDocket.CommonCore.Shared.Enums.FileSort.Size)
            fileEntries.Sort()
            For Each fi As IO.FileInfo In fileEntries
                Me.pbrPrimary.Maximum = fileEntries.Count
                If processedTextFiles <= Me.pbrPrimary.Maximum Then Me.pbrPrimary.Value = processedTextFiles
                Me.lblPrimary.Text = FormatNumber(processedTextFiles, 0) & "/" & FormatNumber(fileEntries.Count, 0) & " (" & FormatNumber((processedTextFiles / fileEntries.Count) * 100, 1) & "%)"
                My.Application.DoEvents()

                Dim fa() As String = fi.FullName.Replace(files_tmp, "").Split("\"c)
                Dim sType As String = ""
                If fi.FullName.ToLower.Contains("\civil\") Then
                    sType = "civil"
                ElseIf fi.FullName.ToLower.Contains("\criminal\") Then
                    sType = "criminal"
                End If

                Dim lst As New List(Of FileRecord)
                Select Case True
                    Case sType.ToLower = "civil"
                        lst = Processor.NCCivilFile(fi.FullName)
                    Case sType.ToLower = "criminal"
                        lst = Processor.NCCriminalFile(fi.FullName)
                End Select

                ' loop through the records we got and save them to the database using the api
                Dim recordsProcessed As Integer = 0
                For Each fr As FileRecord In lst
                    Me.pbrSecondary.Maximum = lst.Count
                    If recordsProcessed <= Me.pbrSecondary.Maximum Then Me.pbrSecondary.Value = recordsProcessed
                    Me.lblSecondary.Text = FormatNumber(recordsProcessed, 0) & "/" & FormatNumber(lst.Count, 0) & " (" & FormatNumber((recordsProcessed / lst.Count) * 100, 1) & "%)"
                    My.Application.DoEvents()

                    Dim lstNames As New List(Of String)

                    If fr.RecordType.ToLower = "civil" Then
                        ' lookup and save the attorneys for civil
                        For Each s As String In fr.DefendantAttorney
                            If s.Trim <> "" And s.Trim.Contains(",") And s.Trim.ToLower <> "pro,se" And s.Trim.ToLower <> "pro se" And s.Trim.ToLower <> "waived" Then
                                ' lookup the name from the file
                                Dim lar As AttorneyRecord = LookupAttorney(s, "NC")
                                If lar.BarNumber <> "" Then
                                    ' see if we have an attorney in the database with that bar number
                                    Dim ar As New AttorneyRecord(lar.BarNumber)
                                    If ar.ID > 0 Then
                                        ' we have a match
                                        If Not ar.AKA.Contains(lar.NameFromFile) Then ar.AKA.Add(lar.NameFromFile)
                                    Else ar = lar
                                    End If

                                    ' fix any names that have numbers at the end
                                    lstNames = New List(Of String)
                                    If ar.AKA IsNot Nothing Then
                                        For Each n As String In ar.AKA
                                            If n.ContainsNumbers Then
                                                Do Until Not n.ContainsNumbers
                                                    n = n.Substring(0, n.Length - 1)
                                                Loop
                                            End If
                                            If Not lstNames.Contains(n.Trim) Then lstNames.Add(n.Trim)
                                        Next
                                    Else lstNames.Add(ar.NameFromFile)
                                    End If
                                    ar.AKA = lstNames
                                    ar.Save()
                                End If
                            End If

                            My.Application.DoEvents()
                        Next

                        For Each s As String In fr.PlaintiffAttorney
                            If s.Trim <> "" And s.Trim.Contains(",") And s.Trim.ToLower <> "pro,se" And s.Trim.ToLower <> "pro se" And s.Trim.ToLower <> "waived" Then
                                ' lookup the name from the file
                                Dim lar As AttorneyRecord = LookupAttorney(s, "NC")
                                If lar.BarNumber <> "" Then
                                    ' see if we have an attorney in the database with that bar number
                                    Dim ar As New AttorneyRecord(lar.BarNumber)
                                    If ar.ID > 0 Then
                                        ' we have a match
                                        If Not ar.AKA.Contains(lar.NameFromFile) Then ar.AKA.Add(lar.NameFromFile)
                                    Else ar = lar
                                    End If

                                    ' fix any names that have numbers at the end
                                    lstNames = New List(Of String)
                                    If ar.AKA IsNot Nothing Then
                                        For Each n As String In ar.AKA
                                            If n.ContainsNumbers Then
                                                Do Until Not n.ContainsNumbers
                                                    n = n.Substring(0, n.Length - 1)
                                                Loop
                                            End If
                                            If Not lstNames.Contains(n.Trim) Then lstNames.Add(n.Trim)
                                        Next
                                    Else lstNames.Add(ar.NameFromFile)
                                    End If
                                    ar.AKA = lstNames
                                    ar.Save()
                                End If
                            End If

                            My.Application.DoEvents()
                        Next
                    ElseIf fr.RecordType.ToLower = "criminal" Then
                        ' lookup and save the attorneys for criminal
                        For Each s As String In fr.DefendantAttorney
                            If s.Trim <> "" And s.Trim.Contains(",") And s.Trim.ToLower <> "pro,se" And s.Trim.ToLower <> "pro se" And s.Trim.ToLower <> "waived" Then
                                ' lookup the name from the file
                                Dim lar As AttorneyRecord = LookupAttorney(s, "NC")
                                If lar.BarNumber <> "" Then
                                    ' see if we have an attorney in the database with that bar number
                                    Dim ar As New AttorneyRecord(lar.BarNumber)
                                    If ar.ID > 0 Then
                                        ' we have a match
                                        If Not ar.AKA.Contains(lar.NameFromFile) Then ar.AKA.Add(lar.NameFromFile)
                                    Else ar = lar
                                    End If

                                    ' fix any names that have numbers at the end
                                    lstNames = New List(Of String)
                                    If ar.AKA IsNot Nothing Then
                                        For Each n As String In ar.AKA
                                            If n.ContainsNumbers Then
                                                Do Until Not n.ContainsNumbers
                                                    n = n.Substring(0, n.Length - 1)
                                                Loop
                                            End If
                                            If Not lstNames.Contains(n.Trim) Then lstNames.Add(n.Trim)
                                        Next
                                    Else lstNames.Add(ar.NameFromFile)
                                    End If
                                    ar.AKA = lstNames
                                    ar.Save()
                                End If
                            End If

                            My.Application.DoEvents()
                        Next
                    End If

                    recordsProcessed += 1

                    If recordsProcessed <= Me.pbrSecondary.Maximum Then Me.pbrSecondary.Value = recordsProcessed
                    Me.lblSecondary.Text = FormatNumber(recordsProcessed, 0) & "/" & FormatNumber(lst.Count, 0) & " (" & FormatNumber((recordsProcessed / lst.Count) * 100, 1) & "%)"
                    My.Application.DoEvents()
                Next

                processedTextFiles += 1

                My.Computer.FileSystem.DeleteFile(fi.FullName)

                If processedTextFiles <= Me.pbrPrimary.Maximum Then Me.pbrPrimary.Value = processedTextFiles
                Me.lblPrimary.Text = FormatNumber(processedTextFiles, 0) & "/" & FormatNumber(fileEntries.Count, 0) & " (" & FormatNumber((processedTextFiles / fileEntries.Count) * 100, 1) & "%)"
                My.Application.DoEvents()

                My.Application.DoEvents()
            Next

            Me.lstLog.Items.Add(processedTextFiles & " Text Files Processed ...")
            Me.lstLog.SelectedIndex = Me.lstLog.Items.Count - 1
            My.Application.DoEvents()

            'Me.ProcessAllFiles(files_tmp, rs("County").ToString, rs("State").ToString)

            My.Application.DoEvents()

            Me.lstLog.Items.Add("Updating Xml IDs ...")
            Me.lstLog.SelectedIndex = Me.lstLog.Items.Count - 1
            My.Application.DoEvents()

            Try
                Dim cmd As New SqlClient.SqlCommand("EXEC [procRefreshAttorneyIDs];", cn)
                If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
                cmd.ExecuteNonQuery()

            Catch ex As Exception
                ex.WriteToErrorLog(New ErrorLogEntry(DailyDocket.CommonCore.Shared.Enums.ProjectName.FileProcessor))
                Me.txtError.Text &= ex.Message & vbCrLf
                Me.txtError.Text &= ex.ToString & vbCrLf & vbCrLf & vbCrLf
            End Try

            ' find any duplicates and create a new record with all the names
            Dim lstRemove As New List(Of Integer)
            Dim cmd1 As New SqlClient.SqlCommand("SELECT xBarNumber, COUNT(ID) AS TotalCount FROM Attorneys GROUP BY xBarNumber HAVING (COUNT(ID) > 1) ORDER BY xBarNumber", cn)
            If cmd1.Connection.State = ConnectionState.Closed Then cmd1.Connection.Open()
            Dim rs As SqlClient.SqlDataReader = cmd1.ExecuteReader
            Do While rs.Read
                Dim ar_new As AttorneyRecord = LookupAttorney(rs("xBarNumber").ToString)
                Dim lstNames As New List(Of String)

                Dim cmd2 As New SqlClient.SqlCommand("SELECT [ID] FROM [Attorneys] WHERE [xBarNumber] LIKE '" & rs("xBarNumber").ToString & "';", cn1)
                If cmd2.Connection.State = ConnectionState.Closed Then cmd2.Connection.Open()
                Dim rs2 As SqlClient.SqlDataReader = cmd2.ExecuteReader
                Do While rs2.Read
                    Dim ar As New AttorneyRecord(rs2("id").ToString.ToInteger)
                    If ar.AKA IsNot Nothing AndAlso ar.AKA.Count > 0 Then
                        For Each n As String In ar.AKA
                            If n.ContainsNumbers Then
                                Do Until Not n.ContainsNumbers
                                    n = n.Substring(0, n.Length - 1)
                                Loop
                            End If
                            If Not lstNames.Contains(n.Trim) Then lstNames.Add(n.Trim)
                        Next
                    Else lstNames.Add(ar.NameFromFile)
                    End If

                    lstRemove.Add(rs2("id").ToString.ToInteger)
                Loop
                cmd2.Cancel()
                rs2.Close()

                ar_new.AKA = lstNames
                If lstNames.Count > 0 And ar_new.NameFromFile = "" Then ar_new.NameFromFile = lstNames(0)
                ar_new.Save()
            Loop
            cmd1.Cancel()
            rs.Close()

            ' delete the duplicates we found from the database
            Dim removeList As String = ""
            For Each i As Integer In lstRemove
                If removeList = "" Then removeList = i.ToString Else removeList &= ", " & i.ToString
            Next
            cmd1 = New SqlClient.SqlCommand("DELETE FROM [Attorneys] WHERE [ID] IN (" & removeList & ");", cn)
            If cmd1.Connection.State = ConnectionState.Closed Then cmd1.Connection.Open()
            cmd1.ExecuteNonQuery()

            Me.lstLog.Items.Add("Finished!")
            Me.lstLog.SelectedIndex = Me.lstLog.Items.Count - 1
            My.Application.DoEvents()

            Me.tmrTimer.Enabled = False

            processRunning = False

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(DailyDocket.CommonCore.Shared.Enums.ProjectName.FileProcessor))
            Me.txtError.Text &= ex.Message & vbCrLf
            Me.txtError.Text &= ex.ToString & vbCrLf & vbCrLf & vbCrLf
        Finally
            cn.Close()
            cn1.Close()
        End Try
    End Sub

    Private Sub ProcessNulls()
        Dim cn As New SqlClient.SqlConnection(ConnectionString)

        Dim retryCount As Integer = 0

        Dim lstRemoveIDs As New List(Of Integer)

        Try
retry_connection:
            ' get a list of attorneys with a null name
            Dim cmd As New SqlClient.SqlCommand("SELECT [ID], [xmlData] FROM [Attorneys] WHERE [xName] IS NULL;", cn)
            If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
            Dim rs As SqlClient.SqlDataReader = cmd.ExecuteReader
            Do While rs.Read
                ' see if we have a matching attorney that is not null by the aka names
                Dim xDoc As New XmlDocument
                xDoc.LoadXml(rs("xmlData").ToString)

                Dim lstNames As New List(Of String)

                Dim attorneyID As Integer = 0
                If xDoc.Item("AttorneyRecord").Item("AKA") IsNot Nothing Then
                    For Each itm As XmlElement In xDoc.Item("AttorneyRecord").Item("AKA")
                        Dim cn1 As New SqlClient.SqlConnection(ConnectionString)

                        retryCount = 0

                        Try
retry_find_match:
                            Dim cmd1 As New SqlClient.SqlCommand("SELECT [ID], [xmlData] FROM [Attorneys] WHERE [xmlData].exist('/AttorneyRecord/AKA [contains(.,""" & itm.InnerText & """)]') = 1 AND [xName] IS NOT NULL;", cn1)
                            If cmd1.Connection.State = ConnectionState.Closed Then cmd1.Connection.Open()
                            Dim rs1 As SqlClient.SqlDataReader = cmd1.ExecuteReader
                            If rs1.Read Then
                                attorneyID = rs1("ID").ToString.ToInteger
                            End If
                            cmd1.Cancel()
                            rs1.Close()

                            lstNames.Add(itm.InnerText)

                        Catch ex As Exception
                            If retryCount <= 10 Then
                                retryCount += 1
                                ' wait 1 second before retrying ...
                                'System.Threading.Thread.Sleep(1000)
                                GoTo retry_find_match
                            Else
                                ex.WriteToErrorLog(New ErrorLogEntry(DailyDocket.CommonCore.Shared.Enums.ProjectName.FileProcessor))
                                Me.txtError.Text &= ex.Message & vbCrLf
                                Me.txtError.Text &= ex.ToString & vbCrLf & vbCrLf & vbCrLf
                            End If
                        Finally
                            cn1.Close()
                        End Try
                    Next
                End If

try_match_again:
                If attorneyID > 0 Then
                    ' combine the aka names from the null record and the actual attorney record
                    Dim ar As New AttorneyRecord(attorneyID)
                    For Each n As String In ar.AKA
                        If Not lstNames.Contains(n) Then lstNames.Add(n)
                    Next

                    ar.AKA = lstNames

                    ' save the changes to the attorney record
                    If ar.Save.ID > 0 Then
                        If Not lstRemoveIDs.Contains(rs("id").ToString.ToInteger) Then lstRemoveIDs.Add(rs("id").ToString.ToInteger)

                        ' update the attorneycases table to move the cases to the new id
                        Dim cn1 As New SqlClient.SqlConnection(ConnectionString)

                        retryCount = 0

                        Try
                            Dim cmd1 As New SqlClient.SqlCommand("UPDATE [AttorneyCases] SET [AttorneyID] = " & attorneyID & " WHERE [AttorneyID] = " & rs("id").ToString & ";", cn1)
                            If cmd1.Connection.State = ConnectionState.Closed Then cmd1.Connection.Open()
                            cmd1.ExecuteNonQuery()

                        Catch ex As Exception
                            ex.WriteToErrorLog(New ErrorLogEntry(DailyDocket.CommonCore.Shared.Enums.ProjectName.FileProcessor))
                            Me.txtError.Text &= ex.Message & vbCrLf
                            Me.txtError.Text &= ex.ToString & vbCrLf & vbCrLf & vbCrLf
                        Finally
                            cn1.Close()
                        End Try
                    End If
                Else
                    Dim ar As AttorneyRecord = LookupAttorney(lstNames(0), "NC")
                    ar.AKA = lstNames
                    If ar.BarNumber <> "" Then
                        ar = ar.Save
                        attorneyID = ar.ID
                        GoTo try_match_again
                    End If
                End If

                My.Application.DoEvents()
            Loop
            cmd.Cancel()
            rs.Close()

            Dim inString As String = ""
            For Each i As Integer In lstRemoveIDs
                If inString = "" Then inString = i.ToString Else inString &= ", " & i.ToString
            Next

            If inString <> "" Then
                cmd = New SqlClient.SqlCommand("DELETE FROM [Attorneys] WHERE [ID] IN (" & inString & ");", cn)
                If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
                cmd.ExecuteNonQuery()
            End If

            Me.lstLog.Items.Add(lstRemoveIDs.Count & " Null Records Processed ...")
            Me.lstLog.SelectedIndex = Me.lstLog.Items.Count - 1
            My.Application.DoEvents()

        Catch ex As Exception
            If retryCount <= 10 Then
                retryCount += 1
                ' wait 1 second before retrying ...
                'System.Threading.Thread.Sleep(1000)
                GoTo retry_connection
            Else
                ex.WriteToErrorLog(New ErrorLogEntry(DailyDocket.CommonCore.Shared.Enums.ProjectName.FileProcessor))
                Me.txtError.Text &= ex.Message & vbCrLf
                Me.txtError.Text &= ex.ToString & vbCrLf & vbCrLf & vbCrLf
            End If
        Finally
            cn.Close()
        End Try
    End Sub

    Private Function ArchiveCases() As Integer
        Dim retVal As Integer = 0

        Dim cn As New SqlClient.SqlConnection(ConnectionString)

        Try
            Dim cmd As New SqlClient.SqlCommand("procArchiveCases", cn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@days", Me.txtArchiveDays.Text.ToInteger)
            cmd.Parameters.AddWithValue("@countOnly", 0)
            If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
            Dim rs As SqlClient.SqlDataReader = cmd.ExecuteReader
            If rs.Read Then
                retVal = rs("ArchiveCount").ToString.ToInteger
            End If
            cmd.Cancel()
            rs.Close()

            Me.lstLog.LogFileProcessorHistory(retVal & " Case(s) Archived ...")
            My.Application.DoEvents()

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(DailyDocket.CommonCore.Shared.Enums.ProjectName.FileProcessor))
            Me.txtError.Text &= ex.Message & vbCrLf
            Me.txtError.Text &= ex.ToString & vbCrLf & vbCrLf & vbCrLf
        End Try

        Return retVal
    End Function

    Private Function ProcessNightBeforeReminders() As Integer
        ' these notifications go to attorneys the night before court at 8pm
        Dim retVal As Integer = 0

        Dim cn As New SqlClient.SqlConnection(ConnectionString)

        Try
            Dim cmd As New SqlClient.SqlCommand("SELECT [xAttorneyID] FROM [Users] WHERE [ID] > 40 AND [xName] IS NOT NULL AND [xClientID] > 0 AND [Active] = 1;", cn)
            If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
            Dim rs As SqlClient.SqlDataReader = cmd.ExecuteReader
            Do While rs.Read

                My.Application.DoEvents()
            Loop
            cmd.Cancel()
            rs.Close()

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(DailyDocket.CommonCore.Shared.Enums.ProjectName.FileProcessor))
            Me.txtError.Text &= ex.Message & vbCrLf
            Me.txtError.Text &= ex.ToString & vbCrLf & vbCrLf & vbCrLf
        Finally
            cn.Close()
        End Try

        Return retVal
    End Function

    Private Function ProcessMorningOfReminders() As Integer
        ' these notifications go to clients the morning of court at 7am
        Dim retVal As Integer = 0

        Dim cn As New SqlClient.SqlConnection(ConnectionString)

        Try
            Dim cmd As New SqlClient.SqlCommand("SELECT [xAttorneyID] FROM [Users] WHERE [ID] > 40 AND [xName] IS NOT NULL AND [xClientID] > 0 AND [Active] = 1;", cn)
            If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
            Dim rs As SqlClient.SqlDataReader = cmd.ExecuteReader
            Do While rs.Read

                My.Application.DoEvents()
            Loop
            cmd.Cancel()
            rs.Close()

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(DailyDocket.CommonCore.Shared.Enums.ProjectName.FileProcessor))
            Me.txtError.Text &= ex.Message & vbCrLf
            Me.txtError.Text &= ex.ToString & vbCrLf & vbCrLf & vbCrLf
        Finally
            cn.Close()
        End Try

        Return retVal
    End Function

#Region "Buttons & Timers"

    Private Sub btnProcess_Click(sender As Object, e As EventArgs) Handles btnProcessFiles.Click
        Me.lstLog.LogFileProcessorHistory("Manual Run: " & Now.ToString)
        Me.Process(True)
    End Sub

    Private Sub btnProcessAttorneys_Click(sender As Object, e As EventArgs)
        Me.tmrAutoRun.Enabled = False
        Me.ProcessAttorneys()
        Me.tmrAutoRun.Enabled = True
        'LookupAttorney("HUNT,AMY,P", "NC")
    End Sub

    Private Sub btnProcessClients_Click(sender As Object, e As EventArgs)
        Me.tmrAutoRun.Enabled = False
        Me.tmrAutoRun.Enabled = True
    End Sub

    Private Sub btnTest_Click(sender As Object, e As EventArgs)
        Me.tmrAutoRun.Enabled = False

        Dim lst As New List(Of FileRecord)

        lst = Processor.NCCivilFileNew("c:\temp\S.__ALAN_Z_THORNBURG.1.10_00_.TRIALS.txt")
        'files_tmp = "C:\temp\"

        'Dim fileEntries As New List(Of String)
        'fileEntries = SearchDir(files_tmp, "*.txt")
        'fileEntries.Sort()
        'For Each fName As String In fileEntries
        '    Dim fi As New System.IO.FileInfo(fName)
        '    Dim fa() As String = fName.Replace(files_tmp, "").Split("\"c)

        '    Dim sCounty As String = fName.Replace(files_tmp, "").Split("\"c)(1)
        '    Dim sType As String = fName.Replace(files_tmp, "").Split("\"c)(2)
        '    Dim sDate As String = ""

        '    If sType.ToLower = "civil" Then
        '        sDate = fName.Replace(files_tmp, "").Split("\"c)(3)
        '    ElseIf sType.ToLower = "criminal" Then
        '        sDate = fi.Name.Split("."c)(2) & "." & fi.Name.Split("."c)(3) & "." & fi.Name.Split("."c)(4)
        '    End If

        '    Dim lst As New List(Of FileRecord)
        '    Select Case True
        '        Case sType.ToLower = "civil"
        '            lst = Processor.NCCivilFile(fName)
        '        Case sType.ToLower = "criminal"
        '            lst = Processor.NCCriminalFile(fName)
        '    End Select

        Dim x As Integer = lst.Count
        'Next

        Me.tmrAutoRun.Enabled = True
    End Sub

    Private Sub tmrTimer_Tick(sender As Object, e As EventArgs) Handles tmrTimer.Tick
        Me.tmrTimer.Enabled = False

        Dim time As TimeSpan = TimeSpan.FromSeconds(DateDiff(DateInterval.Second, dtStartTime, Now))
        Dim str As String = time.ToString("hh\:mm\:ss\:fff")

        Me.lblTotalTime.Text = str & " to Process"

        Me.tmrTimer.Enabled = True
        My.Application.DoEvents()
    End Sub

    Private Sub tmrAutoRun_Tick(sender As Object, e As EventArgs) Handles tmrAutoRun.Tick
        If DateDiff(DateInterval.Minute, lastRunTime, Now) >= 120 And Not processRunning Then
            Me.lstLog.LogFileProcessorHistory("Auto Run: " & Now.ToString)

            Me.Process(True)
        End If
    End Sub

    Private Sub btnProcessAttorneyCases_Click(sender As Object, e As EventArgs) Handles btnProcessAttorneyCases.Click
        Me.tmrAutoRun.Enabled = False
        Me.ProcessAttorneyCases()
        Me.tmrAutoRun.Enabled = True
    End Sub

    Private Sub btnAttorneyNames_Click(sender As Object, e As EventArgs) Handles btnAttorneyNames.Click
        Me.tmrAutoRun.Enabled = False
        Me.GetAllAttorneyNames()
        Me.tmrAutoRun.Enabled = True
    End Sub

    Private Sub btnManualNotifications_Click(sender As Object, e As EventArgs) Handles btnManualNotifications.Click
        Me.tmrAutoRun.Enabled = False
        Me.ProcessNotifications(CDate(Me.txtManualNotificationsDate.Text))
        Me.tmrAutoRun.Enabled = True
    End Sub

    Private Sub btnAdminCalendars_Click(sender As Object, e As EventArgs) Handles btnAdminCalendars.Click
        Me.tmrAutoRun.Enabled = False
        Me.ProcessAdminCalendars()
        Me.tmrAutoRun.Enabled = True
    End Sub

    Private Sub btnCheckProcessed_Click(sender As Object, e As EventArgs) Handles btnCheckProcessed.Click
        Me.tmrAutoRun.Enabled = False
        Me.CheckProcessed()
        Me.tmrAutoRun.Enabled = True
    End Sub

    Private Sub btnArchive_Click(sender As Object, e As EventArgs) Handles btnArchive.Click
        Me.tmrAutoRun.Enabled = False
        Me.ArchiveCases()
        Me.tmrAutoRun.Enabled = True
    End Sub

#End Region
End Class
