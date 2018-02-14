Imports DailyDocket.CommonCore.Shared.Common
Imports System.Configuration.ConfigurationManager

Public Class Registration
    Inherits builderPage

    Dim oPayments As Payments

#Region "Properties"

    Private ReadOnly Property Duration As Integer
        Get
            If Request.QueryString("duration") = "" Then
                Return 7
            Else Return Request.QueryString("duration").ToInteger
            End If
        End Get
    End Property
    Private Property aRecord As AttorneyRecord
        Get
            If Session("aRecord") Is Nothing Then Session("aRecord") = New AttorneyRecord
            Return CType(Session("aRecord"), AttorneyRecord)
        End Get
        Set(value As AttorneyRecord)
            Session("aRecord") = value
        End Set
    End Property
    Private Property cRecord As SystemClient
        Get
            If Session("cRecord") Is Nothing Then Session("cRecord") = New SystemClient
            Return CType(Session("cRecord"), SystemClient)
        End Get
        Set(value As SystemClient)
            Session("cRecord") = value
        End Set
    End Property
    Private Property uRecord As SystemUser
        Get
            If Session("uRecord") Is Nothing Then Session("uRecord") = New SystemUser
            Return CType(Session("uRecord"), SystemUser)
        End Get
        Set(value As SystemUser)
            Session("uRecord") = value
        End Set
    End Property

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ' setup the payment processor
        oPayments = New Payments(ConfigurationManager.AppSettings("StripeMode"))

        If Not IsPostBack Then
            Me.pnlStep1.Visible = True
            Me.pnlStep2.Visible = False
            Me.pnlStep3.Visible = False
            Me.pnlDone.Visible = False
            Me.pnlErrors.Visible = False

            Me.ddlState.SelectedValue = "NC"
            Me.txtBarNumber.Focus()

            Me.LoadLists()
        End If
    End Sub

    Private Sub LoadLists()
        Me.ddlExpYear.Items.Clear()
        For x As Integer = Now.Year To Now.Year + 20
            Me.ddlExpYear.Items.Add(New Telerik.Web.UI.DropDownListItem(x.ToString, x.ToString))
        Next

        Me.lblMonthlyCharge.Text = FormatCurrency(oPayments.GetChargeAmount(Payments.ChargeAmount.MonthlySubscription, 0), 2)
        Me.lblMonthlyCharge1.Text = Me.lblMonthlyCharge.Text
    End Sub

    Protected Sub btnFind_Click(sender As Object, e As EventArgs) Handles btnFind.Click
        Dim name As String = Me.txtLastName.Text.Trim & "," & Me.txtFirstName.Text.Trim

        Me.aRecord = New AttorneyRecord(Me.txtBarNumber.Text) 'LookupAttorney(Me.txtBarNumber.Text.Trim, name.ToUpper, Me.ddlState.SelectedValue)

        If Me.aRecord.Name <> "" Then
            Me.lblBarNumber.Text = Me.aRecord.BarNumber
            Me.lblName.Text = Me.aRecord.Name
            Me.lblAddress.Text = Me.aRecord.Address1 & "<br/>" & Me.aRecord.Address2 & "<br/>" & Me.aRecord.Address3 & "<br/>" & Me.aRecord.City & ", " & Me.aRecord.State & "  " & Me.aRecord.ZipCode
            Me.lblWorkPhone.Text = Me.aRecord.WorkPhone
            Me.lblEmail.Text = Me.aRecord.Email
            Me.lblLicenseDate.Text = Me.aRecord.LicenseDate.ToLongDateString

            Me.pnlFound.Visible = True
            Me.pnlNotFound.Visible = False
        Else
            Me.pnlFound.Visible = False
            Me.pnlNotFound.Visible = True
        End If

        Me.pnlStep1.Visible = False
        Me.pnlStep2.Visible = True
        Me.pnlStep3.Visible = False
        Me.pnlDone.Visible = False
    End Sub

    Protected Sub btnNotMe_Click(sender As Object, e As EventArgs) Handles btnNotMe.Click, btnTryAgain.Click
        Me.txtBarNumber.Text = ""
        Me.txtFirstName.Text = ""
        Me.txtLastName.Text = ""
        Me.ddlState.SelectedValue = "NC"
        Me.txtBarNumber.Focus()

        Me.pnlStep1.Visible = True
        Me.pnlStep2.Visible = False
        Me.pnlStep3.Visible = False
        Me.pnlDone.Visible = False
    End Sub

    Protected Sub btnThatsMe_Click(sender As Object, e As EventArgs) Handles btnThatsMe.Click
        Dim cn As New SqlClient.SqlConnection(ConnectionString)

        Try
            Me.txtEmail.Text = Me.aRecord.Email.Trim
            Me.txtAddress1.Text = Me.aRecord.Address1.Trim
            Me.txtAddress2.Text = Me.aRecord.Address2.Trim
            Me.txtZipCode.Text = Me.aRecord.ZipCode.Trim
            Me.LoadZipCode()
            'Me.txtCity.Text = Me.aRecord.City
            'Me.ddlBillingState.SelectedValue = Me.aRecord.State

            Dim name As String = Me.lblName.Text.Replace("Mr. ", "").Replace("Ms. ", "")
            Dim nameParts As NameParts = GetNameParts(name, Enums.NameType.FirstNameFirst)

            Me.aRecord.NameFromFile = (nameParts.LastName & "," & nameParts.FirstName & "," & nameParts.MiddleName).ToUpper

            'Dim caseCount As Integer = 0
            'Dim cmd As New SqlClient.SqlCommand("procMyCases", cn)
            'cmd.CommandType = CommandType.StoredProcedure
            'cmd.Parameters.AddWithValue("@firstName", nameParts.FirstName)
            'cmd.Parameters.AddWithValue("@middleName", nameParts.MiddleName)
            'cmd.Parameters.AddWithValue("@lastName", nameParts.LastName)
            'If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
            'Dim rs As SqlClient.SqlDataReader = cmd.ExecuteReader
            'Do While rs.Read
            '    caseCount += 1
            'Loop
            'cmd.Cancel()
            'rs.Close()

            'Me.lblCaseCount.Text = FormatNumber(caseCount, 0)

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(If(Me.Page.OnMobile, Enums.ProjectName.MobileApp, Enums.ProjectName.Builder)))
        Finally
            cn.Close()
        End Try

        Me.pnlStep1.Visible = False
        Me.pnlStep2.Visible = False
        Me.pnlStep3.Visible = True
        Me.pnlDone.Visible = False
    End Sub

    Private Function CheckForErrors() As List(Of String)
        Dim retVal As New List(Of String)

        If Not Me.txtEmail.Text.IsValidEmail Then retVal.Add("INVALID EMAIL ADDRESS: User accounts are created based on your email address. This is also used for notifications.")
        If (Not Me.txtPassword.Text = Me.txtConfirm.Text) Or Me.txtPassword.Text = "" Or Me.txtConfirm.Text = "" Then retVal.Add("PASSWORDS DO NOT MATCH: The passwords you entered do not match. Please re-enter the passwords and make sure they are the same.")
        If Not Me.txtMobileNumber.Text.FixPhoneNumber.Length = 10 Then retVal.Add("INVALID MOBILE NUMBER: A valid 10 digit mobile number is required for SMS notifications.")
        'If Me.txtAddress1.Text = "" Or Me.txtZipCode.Text = "" Or Me.txtCity.Text = "" Then retVal.Add("INVALID BILLING ADDRESS: Please enter a valid billing address.")
        'If Me.txtCCNumber.Text = "" Or Me.txtCVV.Text = "" Then retVal.Add("INVALID CREDIT/DEBIT CARD: Please verify your Credit/Debit Card information.")

        Return retVal
    End Function

    Protected Sub btnSignUp_Click(sender As Object, e As EventArgs) Handles btnSignUp.Click
        Dim errs As List(Of String) = Me.CheckForErrors

        Me.pnlErrors.Visible = errs.Count > 0

        If errs.Count > 0 Then
            ' show any errors
            Me.litErrors.Text = ""
            For Each err As String In errs
                If Me.litErrors.Text = "" Then Me.litErrors.Text = err Else Me.litErrors.Text &= "<br/>" & err
            Next
        Else
            ' create the attorney record
            Me.aRecord = Me.SaveAttorney(Me.aRecord)

            ' create the client record
            Me.cRecord.Active = True
            Me.cRecord.Address1 = Me.aRecord.Address1
            Me.cRecord.Address2 = Me.aRecord.Address2
            Me.cRecord.Approved = Enums.SystemMode.Live
            Me.cRecord.City = Me.aRecord.City
            Me.cRecord.ContactEmail = Me.txtEmail.Text.Trim
            Me.cRecord.ContactName = Me.aRecord.Name.Replace("Mr. ", "").Replace("Ms. ", "").Trim
            Me.cRecord.State = Me.aRecord.State
            Me.cRecord.ZipCode = Me.aRecord.ZipCode
            If Me.aRecord.Address1.ContainsNumbers Then
                Me.cRecord.Name = Me.aRecord.Name.Replace("Mr. ", "").Replace("Ms. ", "").Trim
            Else Me.cRecord.Name = Me.aRecord.Address1.XmlDecode
            End If
            If Me.Duration > 0 Then
                Me.cRecord.DemoDuration = Me.Duration
                Me.cRecord.DemoStartDate = Now
            End If
            Me.cRecord = Me.cRecord.Save

            ' create the user record
            Me.uRecord.Active = True
            Me.uRecord.AttorneyID = Me.aRecord.ID
            Me.uRecord.ClientID = Me.cRecord.ID
            Me.uRecord.Email = Me.txtEmail.Text.Trim
            Me.uRecord.IsAdminUser = True
            Me.uRecord.IsSysAdmin = True
            Me.uRecord.Name = Me.aRecord.Name.Replace("Mr. ", "").Replace("Ms. ", "").Trim
            Me.uRecord.NameFromFile = Me.aRecord.NameFromFile
            Me.uRecord.Password = Me.txtPassword.Text.Trim
            Me.uRecord.Permissions = Enums.SystemUserPermissions.SystemAdministrator
            Me.uRecord.WebEnabled = True
            If Me.txtMobileNumber.Text.Trim <> "" Then Me.uRecord.MobileNumbers.Add(Me.txtMobileNumber.Text.FixPhoneNumber)
            Me.uRecord.SalesMan = Enums.SalesMan.Web

            If Me.uRecord.MobileNumbers.Count > 0 Then
                Me.uRecord.NotificationType = Enums.NotificationType.SMS
            Else Me.uRecord.NotificationType = Enums.NotificationType.Email
            End If

            Me.uRecord.BillingAddress1 = Me.txtAddress1.Text
            Me.uRecord.BillingAddress2 = Me.txtAddress2.Text
            Me.uRecord.BillingCity = Me.txtCity.Text
            Me.uRecord.BillingState = Me.ddlBillingState.SelectedValue
            Me.uRecord.BillingZipCode = Me.txtZipCode.Text
            Me.uRecord.CCType = CType(Me.ddlCCType.SelectedValue, Enums.CreditCardType)
            Me.uRecord.CCNumber = Me.txtCCNumber.Text
            Me.uRecord.CCExpirationMonth = Me.ddlExpMonth.SelectedValue.ToInteger
            Me.uRecord.CCExpirationYear = Me.ddlExpYear.SelectedValue.ToInteger
            Me.uRecord.CCCVC = Me.txtCVV.Text

            ' if there is a demo period set the last paid date to 30 - the duration period days ago so they get auto billed in 1 week
            If Me.Duration > 0 Then Me.uRecord.LastPaidDate = Now.AddDays((30 - Me.Duration) * -1)

            ' get the device type
            If Me.UserPlatform <> Enums.UserPlatform.Desktop Then Me.uRecord.MobileDeviceType = Me.UserPlatform

            Me.uRecord = Me.uRecord.Save

            ' create a stripe customer
            If Me.txtCCNumber.Text <> "" Then Me.uRecord.StripeCustomer = oPayments.CreateCustomer(Me.uRecord)

            If Me.aRecord.ID > 0 And Me.cRecord.ID > 0 And Me.uRecord.ID > 0 Then
                If Me.Duration = 0 Then
                    ' no demo period so bill the customer
                    Dim success As String = oPayments.CreateCharge(Payments.ChargeAmount.MonthlySubscription, Me.uRecord)

                    If success = "" Then
                        Dim msgReceipt As New Mailer

                        ' email the receipt
                        Dim invoiceNumber As String = Me.aRecord.BarNumber & "-" & Now.Month & Now.Year
                        Dim receiptBody As String = Mailer.GetHtmlBody(Enums.EmailType.Client_Receipt)
                        receiptBody = receiptBody.Replace("[ClientName]", Me.cRecord.ContactName)
                        receiptBody = receiptBody.Replace("[InvoiceDate]", FormatDateTime(Now.Date, DateFormat.ShortDate))
                        receiptBody = receiptBody.Replace("[InvoiceNumber]", invoiceNumber)
                        receiptBody = receiptBody.Replace("[AmountDue]", Me.lblMonthlyCharge.Text)
                        receiptBody = receiptBody.Replace("[DateSent]", FormatDateTime(Now, DateFormat.LongDate))
                        receiptBody = receiptBody.Replace("[TimeSent]", FormatDateTime(Now, DateFormat.LongTime))

                        msgReceipt.HostServer = AppSettings("MailHost")
                        msgReceipt.UserName = AppSettings("MailUser")
                        msgReceipt.Password = AppSettings("MailPassword")
                        msgReceipt.Port = AppSettings("MailPort").ToInteger
                        msgReceipt.To.Add(Me.txtEmail.Text.Trim)
                        msgReceipt.Body = receiptBody
                        msgReceipt.Subject = "Welcome to Daily Docket!"
                        msgReceipt.From = "sales@solvtopia.com"
                        msgReceipt.HtmlBody = True
                        msgReceipt.Send()

                        Me.uRecord.LastPaidDate = Now
                        Me.uRecord = Me.uRecord.Save()
                    Else
                        ' show an error that we couldn't charge the card
                        Me.litErrors.Text = "BILLING ERROR: We were unable to charge your card for the monthly service fee. Please update your billing information and try again."
                        Me.pnlErrors.Visible = True

                        Me.pnlStep1.Visible = False
                        Me.pnlStep2.Visible = False
                        Me.pnlStep3.Visible = True
                        Me.pnlDone.Visible = False
                    End If
                Else
                    ' demo period so don't bill the client
                End If

                If Not Me.pnlErrors.Visible Then
                    ' email the customer the welcome message
                    Dim msg As New Mailer
                    msg.HostServer = AppSettings("MailHost")
                    msg.UserName = AppSettings("MailUser")
                    msg.Password = AppSettings("MailPassword")
                    msg.Port = AppSettings("MailPort").ToInteger
                    msg.To.Add(Me.txtEmail.Text.Trim)
                    msg.Body = Mailer.GetHtmlBody(Enums.EmailType.Client_Registration, Me.uRecord)
                    If Me.Duration > 0 Then
                        Dim demoMessage As String = "<p>Thank you for your interest in Daily Docket! You have been registered for a FREE 7 Day Demo which expires on " & FormatDateTime(Now.AddDays(7), DateFormat.LongDate) & ". "
                        demoMessage &= "During this demo period you can use all the features of Daily Docket including SMS Notifications for you and your clients totally free of charge.</p>"
                        demoMessage &= "<p>Once your demo expires you will be prompted to enter a Credit/Debit Card to continue service. At that time you will be charged the Monthly Service Fee of " & Me.lblMonthlyCharge.Text & "."
                        demoMessage &= "If you no longer wish to use the Daily Docket service at any time during your demo simply click on the 'Cancel My Service' button on the Billing Information tab of your profile.</p>"
                        msg.Body = msg.Body.Replace("[DemoMessage]", demoMessage)
                    Else msg.Body = msg.Body.Replace("[DemoMessage]", "")
                    End If
                    msg.Subject = "Welcome to Daily Docket!"
                    msg.From = "sales@solvtopia.com"
                    msg.HtmlBody = True
                    msg.Send()

                    ' text the client the welcome message
                    Dim cn As New SqlClient.SqlConnection(ConnectionString)

                    Try
                        Dim cmd As New SqlClient.SqlCommand("SELECT [CivilCount], [CriminalCount], [TotalCount] FROM [vwSalesReport] WHERE [AttorneyID] = " & Me.aRecord.ID, cn)
                        If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
                        Dim rs As SqlClient.SqlDataReader = cmd.ExecuteReader
                        If rs.Read Then
                            Messaging.SendTwilioNotification(Me.txtMobileNumber.Text.FixPhoneNumber, "Your calendar has been created with " & FormatNumber(rs("CivilCount"), 0) & " Civil Case(s) and " & FormatNumber(rs("CriminalCount"), 0) & " Criminal Case(s). Welcome to Daily Docket!")
                        End If
                        cmd.Cancel()
                        rs.Close()

                    Catch ex As Exception
                        ex.WriteToErrorLog(New ErrorLogEntry(If(Me.Page.OnMobile, Enums.ProjectName.MobileApp, Enums.ProjectName.Builder)))
                    Finally
                        cn.Close()
                    End Try

                    ' email Jose and let him know
                    msg = New Mailer
                    msg.HostServer = AppSettings("MailHost")
                    msg.UserName = AppSettings("MailUser")
                    msg.Password = AppSettings("MailPassword")
                    msg.Port = AppSettings("MailPort").ToInteger
                    msg.To.Add("jose@solvtopia.com")
                    msg.To.Add("developer@solvtopia.com")
                    msg.Body = Mailer.GetHtmlBody(Enums.EmailType.Sys_Registration, Me.uRecord)
                    msg.Subject = "New User Signup for Daily Docket"
                    msg.From = "sales@solvtopia.com"
                    msg.HtmlBody = True
                    msg.Send()

                    Me.pnlStep1.Visible = False
                    Me.pnlStep2.Visible = False
                    Me.pnlStep3.Visible = False
                    Me.pnlDone.Visible = True
                End If
            Else
                ' show an error that we couldn't setup the accounts
                If Me.aRecord.ID = 0 Then
                    Me.litErrors.Text = "SETUP ERROR: We were unable to save your attorney record to our database. Please try again."
                ElseIf Me.cRecord.ID = 0 Then
                    Me.litErrors.Text = "SETUP ERROR: We were unable to save your client record to our database. Please try again."
                ElseIf Me.uRecord.ID = 0 Then
                    Me.litErrors.Text = "SETUP ERROR: We were unable to save your user record to our database. Please try again."
                ElseIf Me.uRecord.StripeCustomer = "" Then
                    Me.litErrors.Text = "BILLING ERROR: We were unable to save your customer billing account. Please verify your billing information and try again."
                End If
                Me.pnlErrors.Visible = True

                Me.pnlStep1.Visible = False
                Me.pnlStep2.Visible = False
                Me.pnlStep3.Visible = True
                Me.pnlDone.Visible = False
            End If
        End If
    End Sub

    Private Sub LoadZipCode()
        Dim svc As New ZipLookup.USZip

        If Me.txtZipCode.Text = "" Then
        Else
            Dim xNode As System.Xml.XmlNode = svc.GetInfoByZIP(Me.txtZipCode.Text)

            Me.txtCity.Text = xNode.Item("Table").Item("CITY").InnerText

            Me.ddlBillingState.SelectedValue = xNode.Item("Table").Item("STATE").InnerText
        End If
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

                Dim cmd As New SqlClient.SqlCommand("SELECT [ID] FROM [Attorneys] WHERE ([xBarNumber] LIKE '" & ar.BarNumber & "' OR [xNameFromFile] LIKE '" & ar.NameFromFile.Replace("'", "") & "') AND [xState] LIKE '" & Me.ddlState.SelectedValue & "';", cn)
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
                ex.WriteToErrorLog(New ErrorLogEntry(If(Me.Page.OnMobile, Enums.ProjectName.MobileApp, Enums.ProjectName.Builder)))
            Finally
                cn.Close()
            End Try
        End If

        Return retVal
    End Function

    Protected Sub btnStartOver_Click(sender As Object, e As EventArgs) Handles btnStartOver.Click
        Me.txtBarNumber.Text = ""
        Me.txtFirstName.Text = ""
        Me.txtLastName.Text = ""
        Me.ddlState.SelectedValue = "NC"

        Me.pnlStep1.Visible = True
        Me.pnlStep2.Visible = False
        Me.pnlStep3.Visible = False
        Me.pnlDone.Visible = False
    End Sub

    Protected Sub txtZipCode_TextChanged(sender As Object, e As EventArgs) Handles txtZipCode.TextChanged
        Me.LoadZipCode()
    End Sub

    Private Sub btnLogin_Click(sender As Object, e As EventArgs) Handles btnLogin.Click
        Me.RunClientScript("window.parent.location = 'Login.aspx?usr=" & Me.uRecord.ID & "'")
    End Sub
End Class