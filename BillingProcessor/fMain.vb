Imports System.IO
Imports System.Xml
Imports DailyDocket.CommonCore.Shared.Common

Public Class fMain
    Dim dtStartTime As DateTime

    Dim oPayments As Payments
    Dim oFtp As DailyDocket.CommonCore.Shared.Ftp

    Private Sub fMain_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.lstLog.Items.Clear()
        Me.lblTotalTime.Text = "0"
        Me.txtError.Text = ""

        Me.lblVersion.Text = "Version: " & GetApplicationAssembly(Nothing).GetName.Version.ToString
        My.Application.DoEvents()

        ' setup the payment processor
        oPayments = New Payments("live")

        ' setup the ftp server
        oFtp = New Ftp("ftp://ftp.dailycourtdocket.com/access.dailycourtdocket.com/wwwroot/", "wdivzylm_root", "9dR00g326d!")

        ' autorun on load if we are on the server
        If My.Computer.Name.ToUpper.Contains("WIN-CQJRSQ0R80J") Then
            'Me.lstLog.LogBillingProcessorHistory("Auto Run: " & Now.ToString)
            'My.Application.DoEvents()

            'Me.RunProcess()

            'Me.lstLog.LogBillingProcessorHistory("<strong>Finished in " & Me.lblTotalTime.Text & "</strong>")
            'My.Application.DoEvents()

            'End
        End If
    End Sub

    Private Sub RunProcess()
        dtStartTime = Now

        Me.lblTotalTime.Text = "0"
        My.Application.DoEvents()

        Me.tmrTimer.Enabled = True

        ' process the client payments
        Me.Process()

        ' process the transfers
        If Me.chkTransfers.Checked Then
            Me.ProcessTransfers()
        End If

        ' process any auto registrations
        If Me.chkAutoRegister.Checked Then
            Me.ProcessAutoRegister()
        End If

        Me.tmrTimer.Enabled = False
    End Sub

    Private Sub btnProcessPayments_Click(sender As Object, e As EventArgs) Handles btnProcessPayments.Click
        Me.lstLog.LogBillingProcessorHistory("Manual Run: " & Now.ToString)
        My.Application.DoEvents()

        Me.RunProcess()

        Me.lstLog.LogBillingProcessorHistory("<strong>Finished in " & Me.lblTotalTime.Text & "</strong>")
        My.Application.DoEvents()
    End Sub

    Private Sub Process()
        Me.lblPrimary.Text = "0/0 (0.0%)"
        Me.lblSecondary.Text = "0/0 (0.0%)"

        files_tmp = "C:\billing_tmp\"
        If Not My.Computer.FileSystem.DirectoryExists(files_tmp) Then My.Computer.FileSystem.CreateDirectory(files_tmp)

        Dim cn As New SqlClient.SqlConnection(ConnectionString)
        Dim cn1 As New SqlClient.SqlConnection(ConnectionString)

        Dim retryCount As Integer = 0

        Try
try_again:
            Dim sql As String = "SELECT Count([ID]) AS [TotalCount] FROM [Users] WHERE [ID] > 40 AND [xName] IS NOT NULL AND [xBillingLock] = 0 AND [Active] = 1;"
            Dim cmd As New SqlClient.SqlCommand(sql, cn)
            If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
            Dim rs As SqlClient.SqlDataReader = cmd.ExecuteReader
            If rs.Read Then
                lstLog.LogBillingProcessorHistory("Processing " & rs("TotalCount").ToString.ToInteger & " User Payments ...")
                Me.pbrPrimary.Maximum = rs("TotalCount").ToString.ToInteger
                Me.pbrPrimary.Value = 0
                Me.lblPrimary.Text = FormatNumber(Me.pbrPrimary.Value, 0) & "/" & FormatNumber(Me.pbrPrimary.Maximum, 0) & " (" & FormatNumber((Me.pbrPrimary.Value / Me.pbrPrimary.Maximum) * 100, 1) & "%)"
                My.Application.DoEvents()
            End If
            cmd.Cancel()
            rs.Close()

            Dim demoExpiringCount As Integer = 0
            Dim demoExpiredCount As Integer = 0
            Dim billedCount As Integer = 0
            Dim skippedCount As Integer = 0

            sql = "SELECT [ID], [xName] FROM [Users] WHERE [ID] > 40 AND [xName] IS NOT NULL AND [xBillingLock] = 0 AND [Active] = 1;"
            cmd = New SqlClient.SqlCommand(sql, cn)
            If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
            rs = cmd.ExecuteReader
            Do While rs.Read
                Dim usr As New SystemUser(rs("id").ToString.ToInteger)
                Dim ar As New AttorneyRecord(usr.AttorneyID)
                Dim cl As New SystemClient(usr.ClientID)

                If usr.LastPaidDate = New Date Then usr.LastPaidDate = cl.DemoStartDate

                If cl.DemoDays = cl.DemoDuration - 3 Then
                    ' send trial expiration emails 3 days before the demo ends
                    Dim msg As New Mailer
                    msg.HostServer = "mail.solvtopia.com"
                    msg.UserName = "sales@solvtopia.com"
                    msg.Password = "Josie2005!"
                    msg.Port = 26
                    msg.To.Add(If(usr.ID = 40, "james@solvtopia.com", usr.Email))
                    msg.Body = Mailer.GetHtmlBody(Enums.EmailType.Client_TrialEnding, usr)
                    msg.Body = msg.Body.Replace("[DemoExpiringIn]", "in 3 days")
                    msg.Subject = "Daily Docket Trial Ending"
                    msg.From = "sales@solvtopia.com"
                    msg.HtmlBody = True
                    msg.Send()

                    demoExpiringCount += 1

                ElseIf cl.DemoDays = cl.DemoDuration - 2 Then
                    ' send trial expiration emails 2 days before the demo ends
                    Dim msg As New Mailer
                    msg.HostServer = "mail.solvtopia.com"
                    msg.UserName = "sales@solvtopia.com"
                    msg.Password = "Josie2005!"
                    msg.Port = 26
                    msg.To.Add(If(usr.ID = 40, "james@solvtopia.com", usr.Email))
                    msg.Body = Mailer.GetHtmlBody(Enums.EmailType.Client_TrialEnding, usr)
                    msg.Body = msg.Body.Replace("[DemoExpiringIn]", "in 2 days")
                    msg.Subject = "Daily Docket Trial Ending"
                    msg.From = "sales@solvtopia.com"
                    msg.HtmlBody = True
                    msg.Send()

                    demoExpiringCount += 1

                ElseIf cl.DemoDays = cl.DemoDuration - 1 Then
                    ' send trial expiration emails 1 day before the demo ends
                    Dim msg As New Mailer
                    msg.HostServer = "mail.solvtopia.com"
                    msg.UserName = "sales@solvtopia.com"
                    msg.Password = "Josie2005!"
                    msg.Port = 26
                    msg.To.Add(If(usr.ID = 40, "james@solvtopia.com", usr.Email))
                    msg.Body = Mailer.GetHtmlBody(Enums.EmailType.Client_TrialEnding, usr)
                    msg.Body = msg.Body.Replace("[DemoExpiringIn]", "tomorrow")
                    msg.Subject = "Daily Docket Trial Ending"
                    msg.From = "sales@solvtopia.com"
                    msg.HtmlBody = True
                    msg.Send()

                    demoExpiringCount += 1

                ElseIf cl.DemoDays = cl.DemoDuration Then
                    ' send trial ended emails
                    Dim msg As New Mailer
                    msg.HostServer = "mail.solvtopia.com"
                    msg.UserName = "sales@solvtopia.com"
                    msg.Password = "Josie2005!"
                    msg.Port = 26
                    msg.To.Add(If(usr.ID = 40, "james@solvtopia.com", usr.Email))
                    msg.Body = Mailer.GetHtmlBody(Enums.EmailType.Client_TrialExpired, usr)
                    msg.Subject = "Daily Docket Trial Expired"
                    msg.From = "sales@solvtopia.com"
                    msg.HtmlBody = True
                    msg.Send()

                    demoExpiredCount += 1

                ElseIf usr.ID > 0 And usr.UnpaidDays >= 30 Then
                    ' handle users over 30 days that need to pay

                    Dim invoiceNumber As String = ar.BarNumber & "-" & Now.Month & Now.Year

                    ' build the charges based on the monthly fee + client fees for anyone that received messages this month
                    Dim monthlyCharge As Double = oPayments.GetChargeAmount(Payments.ChargeAmount.MonthlySubscription, usr.ID)
                    Dim clientCharge As Double = oPayments.GetChargeAmount(Payments.ChargeAmount.ClientSMS, usr.ID)

                    Dim cmd2 As New SqlClient.SqlCommand("SELECT Count([ID]) AS [TotalCount] FROM [Sys_SMSLog] WHERE [AttorneyID] = " & ar.ID & " AND [Type] = " & CStr(Enums.NotificationRecipientType.Client) & " AND [dtInserted] BETWEEN '" & New Date(Now.Year, Now.Month, 1).ToString & "' AND '" & Now.Date.ToString & "';", cn1)
                    If cmd2.Connection.State = ConnectionState.Closed Then cmd2.Connection.Open()
                    Dim rs1 As SqlClient.SqlDataReader = cmd2.ExecuteReader
                    If rs1.Read Then
                        clientCharge = clientCharge * rs1("TotalCount").ToString.ToInteger
                    End If
                    cmd2.Cancel()
                    rs1.Close()

                    Dim amountDue As Double = monthlyCharge + clientCharge

                    Dim emailBody As String = ""
                    Dim success As String = ""

                    If usr.CCNumber <> "" Then
                        'If DateDiff(DateInterval.Day, Now.Date, usr.LastPaidDate.AddDays(-7)) >= 30 Then
                        '    ' send an invoice 1 week before the due date
                        '    Dim emailBody As String = Mailer.GetHtmlBody(Enums.EmailType.Client_Invoice)
                        '    emailBody = emailBody.Replace("[ClientName]", cl.ContactName)
                        '    emailBody = emailBody.Replace("[InvoiceDate]", FormatDateTime(Now.Date, DateFormat.ShortDate))
                        '    emailBody = emailBody.Replace("[InvoiceNumber]", invoiceNumber)
                        '    emailBody = emailBody.Replace("[AmountDue]", FormatCurrency(amountDue, 2))
                        '    emailBody = emailBody.Replace("[DueDate]", FormatDateTime(Now.Date.AddDays(7), DateFormat.ShortDate))
                        '    emailBody = emailBody.Replace("[MonthlyCharge]", FormatCurrency(monthlyCharge, 2))
                        '    emailBody = emailBody.Replace("[ClientCharge]", FormatCurrency(clientCharge, 2))
                        '    emailBody = emailBody.Replace("[SubTotal]", FormatCurrency(amountDue, 2))
                        '    emailBody = emailBody.Replace("[DateSent]", FormatDateTime(Now, DateFormat.LongDate))
                        '    emailBody = emailBody.Replace("[TimeSent]", FormatDateTime(Now, DateFormat.LongTime))

                        ' charge the user's card 30 days from the last paid date
                        Dim desc As String = rs("xName").ToString & " - " & MonthName(Now.Month) & ", " & Now.Year
                        success = oPayments.CreateCharge(monthlyCharge + clientCharge, desc, usr)
                        If success = "" Then
                            emailBody = Mailer.GetHtmlBody(Enums.EmailType.Client_Receipt)
                            emailBody = emailBody.Replace("[ClientName]", cl.ContactName)
                            emailBody = emailBody.Replace("[InvoiceDate]", FormatDateTime(Now.Date, DateFormat.ShortDate))
                            emailBody = emailBody.Replace("[InvoiceNumber]", invoiceNumber)
                            emailBody = emailBody.Replace("[AmountDue]", FormatCurrency(amountDue, 2))
                            emailBody = emailBody.Replace("[MonthlyCharge]", FormatCurrency(monthlyCharge, 2))
                            emailBody = emailBody.Replace("[ClientCharge]", FormatCurrency(clientCharge, 2))
                            emailBody = emailBody.Replace("[SubTotal]", FormatCurrency(amountDue, 2))
                            emailBody = emailBody.Replace("[DateSent]", FormatDateTime(Now, DateFormat.LongDate))
                            emailBody = emailBody.Replace("[TimeSent]", FormatDateTime(Now, DateFormat.LongTime))

                            usr.LastPaidDate = Now.Date

                            lstLog.LogBillingProcessorHistory(rs("xName").ToString & ": " & FormatCurrency(amountDue, 2) & " Succeeded")
                            My.Application.DoEvents()
                        Else
                            emailBody = Mailer.GetHtmlBody(Enums.EmailType.Client_Payment_Failed)
                            emailBody = emailBody.Replace("[ClientName]", cl.ContactName)
                            emailBody = emailBody.Replace("[InvoiceDate]", FormatDateTime(Now.Date, DateFormat.ShortDate))
                            emailBody = emailBody.Replace("[InvoiceNumber]", invoiceNumber)
                            emailBody = emailBody.Replace("[AmountDue]", FormatCurrency(amountDue, 2))
                            emailBody = emailBody.Replace("[DateSent]", FormatDateTime(Now, DateFormat.LongDate))
                            emailBody = emailBody.Replace("[TimeSent]", FormatDateTime(Now, DateFormat.LongTime))

                            usr.BillingLock = True

                            lstLog.LogBillingProcessorHistory(rs("xName").ToString & ": " & FormatCurrency(amountDue, 2) & " Failed")
                            My.Application.DoEvents()
                        End If
                    Else
                        emailBody = Mailer.GetHtmlBody(Enums.EmailType.Client_Payment_Failed)
                        emailBody = emailBody.Replace("[ClientName]", cl.ContactName)
                        emailBody = emailBody.Replace("[InvoiceDate]", FormatDateTime(Now.Date, DateFormat.ShortDate))
                        emailBody = emailBody.Replace("[InvoiceNumber]", invoiceNumber)
                        emailBody = emailBody.Replace("[AmountDue]", FormatCurrency(amountDue, 2))
                        emailBody = emailBody.Replace("[DateSent]", FormatDateTime(Now, DateFormat.LongDate))
                        emailBody = emailBody.Replace("[TimeSent]", FormatDateTime(Now, DateFormat.LongTime))

                        usr.BillingLock = True

                        lstLog.LogBillingProcessorHistory(rs("xName").ToString & ": " & FormatCurrency(amountDue, 2) & " Failed")
                        My.Application.DoEvents()
                    End If

                    ' save any changes we made to the user
                    usr.Save()

                    ' email the client
                    Dim msg As New Mailer
                    msg.HostServer = "mail.solvtopia.com"
                    msg.UserName = "sales@solvtopia.com"
                    msg.Password = "Josie2005!"
                    msg.Port = 26
                    msg.To.Add(If(usr.ID = 40, "james@solvtopia.com", usr.Email))
                    msg.Body = emailBody
                    msg.Subject = "Daily Docket Billing"
                    msg.From = "sales@solvtopia.com"
                    msg.HtmlBody = True
                    msg.Send()

                    ' save the invoice
                    If Not My.Computer.FileSystem.DirectoryExists(My.Computer.FileSystem.CombinePath(files_tmp, ar.BarNumber)) Then My.Computer.FileSystem.CreateDirectory(My.Computer.FileSystem.CombinePath(files_tmp, ar.BarNumber))

                    Dim ftpFileName As String = "billing_tmp/" & ar.BarNumber & "/" & invoiceNumber & ".pdf"
                    Dim fName As String = My.Computer.FileSystem.CombinePath(files_tmp, ar.BarNumber & "\" & invoiceNumber & ".pdf")
                    If My.Computer.FileSystem.FileExists(fName) Then My.Computer.FileSystem.DeleteFile(fName)

                    ' convert the html to pdf and save the pdf to the server for history
                    success = Printing.HtmlToPdf(emailBody, fName)
                    If success = "" Then
                        If Not oFtp.DirectoryExists("billing_tmp") Then oFtp.CreateDirectory("billing_tmp")
                        If Not oFtp.DirectoryExists("billing_tmp/" &
                                                       ar.BarNumber) Then oFtp.CreateDirectory("billing_tmp/" & ar.BarNumber)
                        If oFtp.FileExists(ftpFileName) Then
                            oFtp.DeleteFile(ftpFileName)
                        End If
                        If oFtp.CopyFile(fName, ftpFileName) Then My.Computer.FileSystem.DeleteFile(fName)
                    End If

                    billedCount += 1

                Else
                    ' all good, skip them
                    Dim s As String = cl.DemoDays & ", " & usr.UnpaidDays

                    skippedCount += 1
                End If

                If Me.pbrPrimary.Value + 1 <= Me.pbrPrimary.Maximum Then Me.pbrPrimary.Value += 1
                Me.lblPrimary.Text = FormatNumber(Me.pbrPrimary.Value, 0) & "/" & FormatNumber(Me.pbrPrimary.Maximum, 0) & " (" & FormatNumber((Me.pbrPrimary.Value / Me.pbrPrimary.Maximum) * 100, 1) & "%)"
                My.Application.DoEvents()
            Loop
            cmd.Cancel()
            rs.Close()

            lstLog.LogBillingProcessorHistory(demoExpiringCount & " Expiring Demo(s) Processed")
            lstLog.LogBillingProcessorHistory(demoExpiredCount & " Expired Demo(s) Processed")
            lstLog.LogBillingProcessorHistory(billedCount & " User Payments Processed")
            lstLog.LogBillingProcessorHistory(skippedCount & " User Payments Skipped")

        Catch ex As Exception
            If retryCount <= 10 Then
                retryCount += 1
                ' wait 1 second before retrying ...
                System.Threading.Thread.Sleep(1000)
                GoTo try_again
            Else
                ex.WriteToErrorLog(New ErrorLogEntry(DailyDocket.CommonCore.Shared.Enums.ProjectName.BillingProcessor))
                Me.txtError.Text &= ex.Message & vbCrLf
                Me.txtError.Text &= ex.ToString & vbCrLf & vbCrLf & vbCrLf
            End If
        Finally
            cn.Close()
            cn1.Close()
        End Try
    End Sub

    Private Sub ProcessTransfers()
        Dim cn As New SqlClient.SqlConnection(ConnectionString)
        Dim cn1 As New SqlClient.SqlConnection(ConnectionString)

        Me.lstLog.LogBillingProcessorHistory("Processing Transfers ...")

        Try
            ' get the stripe balance so we know what we're working with
            Dim bal As Double = oPayments.GetBalance(Payments.BalanceType.Available)

            If bal > 0 Then
                ' get a list of employees, percentages, and stripe account id's
                Dim cmd As New SqlClient.SqlCommand("SELECT [ID], [Employee], [Percentage] FROM [Sys_EmpPay] WHERE [Employee] NOT LIKE 'solvtopia' ORDER BY [ID];", cn)
                If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
                Dim rs As SqlClient.SqlDataReader = cmd.ExecuteReader
                Do While rs.Read
                    ' get the total percentage for the employee
                    Dim empShare As Double = bal * (rs("Percentage").ToString.ToInteger / 100)

                    Dim cmd1 As New SqlClient.SqlCommand("SELECT DISTINCT [StripeAccount], [Percentage] FROM [Sys_EmpAccounts] WHERE [EmpID] = " & rs("ID").ToString & " ORDER BY [Percentage];", cn1)
                    If cmd1.Connection.State = ConnectionState.Closed Then cmd1.Connection.Open()
                    Dim rs1 As SqlClient.SqlDataReader = cmd1.ExecuteReader
                    Do While rs1.Read
                        Dim acctShare As Double = empShare * (rs1("Percentage").ToString.ToInteger / 100)

                        ' create the transfer
                        Dim success As String = oPayments.CreateTransfer(rs1("StripeAccount").ToString, acctShare)

                        ' if the transfer was successful then take the amount from the available balance
                        If success = "" Then
                            bal -= acctShare

                            lstLog.LogBillingProcessorHistory(rs("Employee").ToString & ": " & FormatCurrency(acctShare, 2) & " Transfer Succeeded")
                            My.Application.DoEvents()
                        Else
                            lstLog.LogBillingProcessorHistory(rs("Employee").ToString & ": " & FormatCurrency(acctShare, 2) & " Transfer Failed")
                            My.Application.DoEvents()
                        End If

                        My.Application.DoEvents()
                    Loop
                    cmd1.Cancel()
                    rs1.Close()

                    My.Application.DoEvents()
                Loop
                cmd.Cancel()
                rs.Close()

                ' solvtopia gets the remaining balance
                cmd = New SqlClient.SqlCommand("SELECT [ID], [Employee], [Percentage] FROM [Sys_EmpPay] WHERE [Employee] LIKE 'solvtopia';", cn)
                If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
                rs = cmd.ExecuteReader
                Do While rs.Read
                    ' get the total percentage for the employee
                    Dim empShare As Double = bal

                    Dim cmd1 As New SqlClient.SqlCommand("SELECT DISTINCT [StripeAccount], [Percentage] FROM [Sys_EmpAccounts] WHERE [EmpID] = " & rs("ID").ToString & ";", cn1)
                    If cmd1.Connection.State = ConnectionState.Closed Then cmd1.Connection.Open()
                    Dim rs1 As SqlClient.SqlDataReader = cmd1.ExecuteReader
                    Do While rs1.Read
                        Dim acctShare As Double = 0

                        ' create the transfer
                        Dim success As String = oPayments.CreateTransfer(rs1("StripeAccount").ToString, acctShare)

                        ' if the transfer was successful then take the amount from the available balance
                        If success = "" Then
                            bal -= acctShare

                            lstLog.LogBillingProcessorHistory(rs("Employee").ToString & ": " & FormatCurrency(acctShare, 2) & " Transfer Succeeded")
                            My.Application.DoEvents()
                        Else
                            lstLog.LogBillingProcessorHistory(rs("Employee").ToString & ": " & FormatCurrency(acctShare, 2) & " Transfer Failed")
                            My.Application.DoEvents()
                        End If

                        My.Application.DoEvents()
                    Loop
                    cmd1.Cancel()
                    rs1.Close()

                    My.Application.DoEvents()
                Loop
                cmd.Cancel()
                rs.Close()
            End If

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(DailyDocket.CommonCore.Shared.Enums.ProjectName.BillingProcessor))
            Me.txtError.Text &= ex.Message & vbCrLf
            Me.txtError.Text &= ex.ToString & vbCrLf & vbCrLf & vbCrLf
        Finally
            cn.Close()
            cn1.Close()
        End Try
    End Sub

    Private Sub ProcessAutoRegister()
        Dim cn As New SqlClient.SqlConnection(ConnectionString)

        Try
            Me.lstLog.LogBillingProcessorHistory("Processing Auto Registrations ...")
            My.Application.DoEvents()

            Dim monthlyCharge As Double = oPayments.GetChargeAmount(Payments.ChargeAmount.MonthlySubscription, 0)
            Dim lst As New List(Of String)

            Dim cmd As New SqlClient.SqlCommand("SELECT DISTINCT [xBarNumber] FROM [vwAutoRegister] WHERE [xPassword] IS NULL;", cn)
            If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
            Dim rs As SqlClient.SqlDataReader = cmd.ExecuteReader
            Do While rs.Read
                ' get the attorney record
                Dim aRecord As New AttorneyRecord(rs("xBarNumber").ToString)

                If Not lst.Contains(aRecord.Name) Then lst.Add(aRecord.Name)

                Dim cRecord As New SystemClient
                ' create the client record
                cRecord.Active = True
                cRecord.Address1 = aRecord.Address1
                cRecord.Address2 = aRecord.Address2
                cRecord.Approved = Enums.SystemMode.Live
                cRecord.City = aRecord.City
                cRecord.ContactEmail = aRecord.Email
                cRecord.ContactName = aRecord.Name.Replace("Mr. ", "").Replace("Ms. ", "").Trim
                cRecord.State = aRecord.State
                cRecord.ZipCode = aRecord.ZipCode
                If aRecord.Address1.ContainsNumbers Then
                    cRecord.Name = aRecord.Name.Replace("Mr. ", "").Replace("Ms. ", "").Trim
                Else cRecord.Name = aRecord.Address1.XmlDecode
                End If
                cRecord.DemoDuration = 365
                cRecord.DemoStartDate = Now
                cRecord = cRecord.Save

                ' create the user record
                Dim uRecord As New SystemUser
                uRecord.Active = True
                uRecord.AttorneyID = aRecord.ID
                uRecord.ClientID = cRecord.ID
                uRecord.Email = aRecord.Email
                uRecord.IsAdminUser = True
                uRecord.IsSysAdmin = True
                uRecord.Name = aRecord.Name.Replace("Mr. ", "").Replace("Ms. ", "").Trim
                uRecord.NameFromFile = aRecord.NameFromFile
                uRecord.Password = "9dR00g326d"
                uRecord.Permissions = Enums.SystemUserPermissions.SystemAdministrator
                uRecord.WebEnabled = True
                'uRecord.MobileNumbers.Add(aRecord.WorkPhone.FixPhoneNumber)
                uRecord.MaxNotificationsPerBatch = 10
                uRecord.SalesMan = Enums.SalesMan.AutoRegister

                If uRecord.MobileNumbers.Count > 0 Then
                    uRecord.NotificationType = Enums.NotificationType.SMS
                    uRecord.NotificationLevel = Enums.NotificationLevel.Summary
                Else
                    uRecord.NotificationType = Enums.NotificationType.Email
                    uRecord.NotificationLevel = Enums.NotificationLevel.Detail
                End If

                uRecord.BillingAddress1 = aRecord.Address1
                uRecord.BillingAddress2 = aRecord.Address2
                uRecord.BillingCity = aRecord.City
                uRecord.BillingState = aRecord.State
                uRecord.BillingZipCode = aRecord.ZipCode

                ' if there is a demo period set the last paid date to 30 - the duration period days ago so they get auto billed in 1 week
                uRecord.LastPaidDate = Now.AddDays((30 - 7) * -1)

                uRecord = uRecord.Save

                If aRecord.ID > 0 And cRecord.ID > 0 And uRecord.ID > 0 Then
                    ' email the customer the welcome message
                    Dim msg As New Mailer
                    msg.HostServer = "mail.solvtopia.com"
                    msg.UserName = "sales@solvtopia.com"
                    msg.Password = "Josie2005!"
                    msg.Port = 26
                    msg.To.Add(aRecord.Email)
                    msg.Body = Mailer.GetHtmlBody(Enums.EmailType.Client_Registration, uRecord)
                    Dim demoMessage As String = "<p>Thank you for your interest in Daily Docket! You have been registered for a FREE 7 Day Demo which expires on " & FormatDateTime(Now.AddDays(7), DateFormat.LongDate) & ". "
                    demoMessage &= "During this demo period you can use all the features of Daily Docket including SMS Notifications for you and your clients totally free of charge.</p>"
                    demoMessage &= "<p>Once your demo expires you will be prompted to enter a Credit/Debit Card to continue service. At that time you will be charged the Monthly Service Fee of " & FormatCurrency(monthlyCharge, 2) & "."
                    demoMessage &= "If you no longer wish to use the Daily Docket service at any time during your demo simply click on the 'Cancel My Service' button on the Billing Information tab of your profile.</p>"
                    msg.Body = msg.Body.Replace("[DemoMessage]", demoMessage)
                    msg.Subject = "Welcome to Daily Docket!"
                    msg.From = "sales@solvtopia.com"
                    msg.HtmlBody = True
                    msg.Send()

                    lstLog.LogBillingProcessorHistory(aRecord.Name & " Auto Registered")

                    ' text the client the welcome message
                    Dim cn1 As New SqlClient.SqlConnection(ConnectionString)

                    Try
                        Dim cmd1 As New SqlClient.SqlCommand("SELECT [CivilCount], [CriminalCount], [TotalCount] FROM [vwSalesReport] WHERE [AttorneyID] = " & aRecord.ID, cn1)
                        If cmd1.Connection.State = ConnectionState.Closed Then cmd1.Connection.Open()
                        Dim rs1 As SqlClient.SqlDataReader = cmd1.ExecuteReader
                        If rs1.Read Then
                            Messaging.SendTwilioNotification(aRecord.WorkPhone.FixPhoneNumber, "Your calendar has been created with " & FormatNumber(rs1("CivilCount"), 0) & " Civil Case(s) And " & FormatNumber(rs1("CriminalCount"), 0) & " Criminal Case(s). Welcome to Daily Docket!")
                        End If
                        cmd1.Cancel()
                        rs1.Close()

                    Catch ex As Exception
                        ex.WriteToErrorLog(New ErrorLogEntry(Enums.ProjectName.BillingProcessor))
                    Finally
                        cn1.Close()
                    End Try
                Else
                    Dim s As String = ""
                End If

                My.Application.DoEvents()
            Loop
            cmd.Cancel()
            rs.Close()

            If lst.Count > 0 Then
                ' email Jose and let him know
                Dim msg1 As New Mailer
                msg1.HostServer = "mail.solvtopia.com"
                msg1.UserName = "sales@solvtopia.com"
                msg1.Password = "Josie2005!"
                msg1.Port = 26
                msg1.To.Add("jose@solvtopia.com")
                msg1.To.Add("developer@solvtopia.com")
                msg1.Body = Mailer.GetHtmlBody(Enums.EmailType.Custom)
                msg1.Body = msg1.Body.Replace("[HeaderMessage]", "Auto Registration Log")
                msg1.Body = msg1.Body.Replace("[ToName]", "Solvtopia")
                Dim txt As String = ""
                For Each n As String In lst
                    If txt = "" Then txt = n Else txt &= "<br/>" & n
                Next
                msg1.Body = msg1.Body.Replace("[MessageContent]", "The following users have been automatically registered for Daily Docket based on the Calendar Preview:<br/><br/>" & txt)
                msg1.Subject = "New User Auto-Signup for Daily Docket"
                msg1.From = "sales@solvtopia.com"
                msg1.HtmlBody = True
                msg1.Send()
            End If

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(Enums.ProjectName.BillingProcessor))
            Me.txtError.Text &= ex.Message & vbCrLf
            Me.txtError.Text &= ex.ToString & vbCrLf & vbCrLf & vbCrLf
        Finally
            cn.Close()
        End Try
    End Sub

    Private Function SaveAttorney(ByVal ar As AttorneyRecord) As AttorneyRecord
        Dim retVal As New AttorneyRecord

        If ar.NameFromFile.Trim <> "" And ar.NameFromFile.Trim.Contains(",") And ar.NameFromFile.Trim.ToLower <> "pro,se" And ar.NameFromFile.Trim.ToLower <> "pro se" And ar.NameFromFile.Trim.ToLower <> "waived" And ar.BarNumber <> "" Then
            Dim lar As New AttorneyRecord(ar.BarNumber)

            ' check to see if we already have the attorney in the database
            ' if so, update the record, else add the record
            Dim cn As New SqlClient.SqlConnection(ConnectionString)

            Try
                Dim lstNames As New List(Of String)

                Dim cmd As New SqlClient.SqlCommand("SELECT [ID] FROM [Attorneys] WHERE ([xBarNumber] Like '" & ar.BarNumber & "' OR [xNameFromFile] LIKE '" & ar.NameFromFile.Replace("'", "") & "') AND [xState] LIKE 'NC';", cn)
                If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
                Dim rs As SqlClient.SqlDataReader = cmd.ExecuteReader
                If rs.Read Then
                    ar.ID = CInt(rs("id").ToString)
                    lar = New AttorneyRecord(CInt(rs("id").ToString))
                End If
                cmd.Cancel()
                rs.Close()

                If ar.ID > 0 And lar.ID > 0 Then
                    ' fix any names that have numbers at the end
                    If lar.AKA IsNot Nothing Then
                        For Each n As String In lar.AKA
                            If n.ContainsNumbers Then
                                Do Until Not n.ContainsNumbers
                                    n = n.Substring(0, n.Length - 1)
                                Loop
                            End If
                            If Not lstNames.Contains(n.Trim) Then lstNames.Add(n.Trim)
                        Next
                    Else lstNames.Add(ar.NameFromFile)
                    End If
                Else lstNames.Add(ar.NameFromFile)
                End If

                ar.AKA = lstNames
                retVal = ar.Save

            Catch ex As Exception
                ex.WriteToErrorLog(New ErrorLogEntry(Enums.ProjectName.BillingProcessor))
                Me.txtError.Text &= ex.Message & vbCrLf
                Me.txtError.Text &= ex.ToString & vbCrLf & vbCrLf & vbCrLf
            Finally
                cn.Close()
            End Try
        End If

        Return retVal
    End Function

    Private Sub tmrTimer_Tick(sender As Object, e As EventArgs) Handles tmrTimer.Tick
        Me.tmrTimer.Enabled = False

        Dim time As TimeSpan = TimeSpan.FromSeconds(DateDiff(DateInterval.Second, dtStartTime, Now))
        Dim str As String = time.ToString("hh\:mm\:ss\:fff")

        Me.lblTotalTime.Text = str & " to Process"

        Me.tmrTimer.Enabled = True
        My.Application.DoEvents()
    End Sub

    Private Sub btnAutoRegister_Click(sender As Object, e As EventArgs) Handles btnAutoRegister.Click
        Me.ProcessAutoRegister()
    End Sub

    Private Sub btnFixClients_Click(sender As Object, e As EventArgs) Handles btnFixClients.Click
        Dim cn As New SqlClient.SqlConnection(ConnectionString)
        Dim cn1 As New SqlClient.SqlConnection(ConnectionString)

        Try
            Dim cmd As New SqlClient.SqlCommand("SELECT [ID], [xAttorneyID] FROM [Users] WHERE [xName] IS NOT NULL AND [xClientID] = 0;", cn)
            If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
            Dim rs As SqlClient.SqlDataReader = cmd.ExecuteReader
            Do While rs.Read
                Dim aRecord As New AttorneyRecord(rs("xAttorneyID").ToString.ToInteger)

                Dim cRecord As New SystemClient
                ' create the client record
                cRecord.Active = True
                cRecord.Address1 = aRecord.Address1
                cRecord.Address2 = aRecord.Address2
                cRecord.Approved = Enums.SystemMode.Live
                cRecord.City = aRecord.City
                cRecord.ContactEmail = aRecord.Email
                cRecord.ContactName = aRecord.Name.Replace("Mr. ", "").Replace("Ms. ", "").Trim
                cRecord.State = aRecord.State
                cRecord.ZipCode = aRecord.ZipCode
                If aRecord.Address1.ContainsNumbers Then
                    cRecord.Name = aRecord.Name.Replace("Mr. ", "").Replace("Ms. ", "").Trim
                Else cRecord.Name = aRecord.Address1.XmlDecode
                End If
                cRecord.DemoDuration = 7
                cRecord.DemoStartDate = Now
                cRecord = cRecord.Save

                If cRecord.ID > 0 Then
                    Dim xDoc As New XmlDocument
                    Dim cmd1 As New SqlClient.SqlCommand("SELECT [xmlData] FROM [Users] WHERE [ID] = " & rs("ID").ToString & ";", cn1)
                    If cmd1.Connection.State = ConnectionState.Closed Then cmd1.Connection.Open()
                    Dim rs2 As SqlClient.SqlDataReader = cmd1.ExecuteReader
                    If rs2.Read Then
                        xDoc.LoadXml(rs2("xmlData").ToString)
                    End If
                    cmd1.Cancel()
                    rs2.Close()

                    xDoc.Item("SystemUser").Item("ClientID").InnerText = cRecord.ID.ToString

                    cmd1 = New SqlClient.SqlCommand("UPDATE [Users] SET [xmlData] = @xmlData WHERE [ID] = " & rs("ID").ToString & ";", cn1)
                    If cmd1.Connection.State = ConnectionState.Closed Then cmd1.Connection.Open()
                    cmd1.Parameters.AddWithValue("@xmlData", xDoc.ToXmlString)
                    cmd1.ExecuteNonQuery()
                    cmd1.Cancel()
                Else
                    Dim s As String = ""
                End If

                My.Application.DoEvents()
            Loop
            cmd.Cancel()
            rs.Close()

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(Enums.ProjectName.BillingProcessor))
            Me.txtError.Text &= ex.Message & vbCrLf
            Me.txtError.Text &= ex.ToString & vbCrLf & vbCrLf & vbCrLf
        Finally
            cn.Close()
            cn1.Close()
        End Try
    End Sub

    Private Sub btnProcessTransfers_Click(sender As Object, e As EventArgs) Handles btnProcessTransfers.Click
        Me.ProcessTransfers()
    End Sub
End Class
