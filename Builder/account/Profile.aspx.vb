Imports DailyDocket.CommonCore.Shared.Common
Imports Telerik.Web.UI

Public Class Profile
    Inherits builderPage

    Dim oPayments As Payments

#Region "Properties"

    Public ReadOnly Property EditId As Integer
        Get
            Dim retVal As Integer = 0

            If Request.QueryString("editid") <> "" Then retVal = Request.QueryString("editid").ToInteger Else retVal = Me.UserId

            If retVal = 0 Then retVal = App.CurrentUser.ID

            Return retVal
        End Get
    End Property
    Public ReadOnly Property ReturnTo As String
        Get
            Dim retVal As String = "~/Default.aspx?"

            Dim usr As New SystemUser(Me.EditId)

            If usr.BillingLock Or App.CurrentClient.DemoDays >= App.CurrentClient.DemoDuration Then
                retVal = "~/account/Profile.aspx?tab=" & Me.SelectedTab & "&"
            Else
                If Request.QueryString("editid") <> "" Then retVal = "~/admin/Users.aspx?"
            End If

            Dim usrString As String = If(Me.DeviceId <> "", "deviceid=" & Me.DeviceId, "uid=" & Me.UserId)

            Return retVal & usrString
        End Get
    End Property
    Public ReadOnly Property SelectedTab As Integer
        Get
            Return Request.QueryString("tab").ToInteger
        End Get
    End Property

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ' setup the payment processor
        oPayments = New Payments(ConfigurationManager.AppSettings("StripeMode"))

        If Not IsPostBack Then
            Me.LoadLists()
            Me.LoadData()

            Me.RadTabStrip1.SelectedIndex = Me.SelectedTab
            Me.RadMultiPage1.SelectedIndex = Me.SelectedTab
        End If

        Me.SetupForm()

        Me.Master.BackButton.Visible = False
    End Sub

    Private Sub LoadData()
        Dim usr As New SystemUser(Me.EditId)

        ' user details
        If Me.EditId = 0 Then
            Me.ddlClient.SelectedValue = App.CurrentClient.ID.ToString
        Else Me.ddlClient.SelectedValue = usr.ClientID.ToString
        End If
        Me.txtName.Text = usr.Name
        Me.txtEmail.Text = usr.Email.Trim
        Me.txtPassword.Text = usr.Password.Trim
        For Each n As String In usr.MobileNumbers
            If Me.txtMobileNumber.Text = "" Then Me.txtMobileNumber.Text = n.Trim Else Me.txtMobileNumber.Text &= vbCrLf & n.Trim
        Next
        Me.ddlPermissions.SelectedValue = CStr(usr.Permissions)
        Me.ddlSupervisor.SelectedValue = CStr(usr.SupervisorID)
        Me.txtDeviceID.Text = usr.MobileDeviceId
        Me.txtOneSignal.Text = usr.OneSignalUserID
        Me.chkApiAccess.Checked = usr.ApiEnabled
        Me.chkWebAccess.Checked = usr.WebEnabled
        Me.ddlDeviceType.SelectedValue = CStr(usr.MobileDeviceType)

        Me.ddlNotificationType.SelectedValue = CStr(usr.NotificationType)
        Me.ddlNotificationLevel.SelectedValue = CStr(usr.NotificationLevel)
        Me.txtMaxSMSPerBatch.Text = usr.MaxNotificationsPerBatch.ToString

        Me.litDeviceIDs.Text = ""
        For Each s As String In usr.MobileDeviceIds
            If s <> "" And s = Me.DeviceId Then s = "<strong>" & s & "</strong>"

            If Me.litDeviceIDs.Text = "" Then Me.litDeviceIDs.Text = s Else Me.litDeviceIDs.Text &= "<br/>" & s
        Next

        ' billing information
        Me.txtNameCard.Text = usr.BillingName
        If Me.txtNameCard.Text = "" Then Me.txtNameCard.Text = Me.txtName.Text
        Me.txtAddress1.Text = usr.BillingAddress1
        Me.txtAddress2.Text = usr.BillingAddress2
        Me.txtZipCode.Text = usr.BillingZipCode
        Me.LoadZipCode()
        If usr.BillingCity <> "" Then
            Me.txtCity.Text = usr.BillingCity
            Me.ddlBillingState.SelectedValue = usr.BillingState
        End If
        Me.ddlCCType.SelectedValue = CInt(usr.CCType).ToString
        Me.txtCCNumber.Text = usr.CCNumber
        Me.ddlExpMonth.SelectedValue = usr.CCExpirationMonth.ToString
        Me.ddlExpYear.SelectedValue = usr.CCExpirationYear.ToString
        Me.txtCVV.Text = usr.CCCVC
        Me.pnlBillingLock.Visible = (usr.BillingLock Or App.CurrentClient.DemoDays >= App.CurrentClient.DemoDuration)

        If usr.BillingLock Then
            Me.lblBillingLock.Text = "BILLING ERROR: Your account has been locked because we were unable to charge your card for the monthly service fee. Please update your billing information below and click Save to avoid further service interruption."
        ElseIf App.CurrentClient.DemoDays >= App.CurrentClient.DemoDuration Then
            Me.lblBillingLock.Text = "DEMO EXPIRED: Your account has been locked because your " & App.CurrentClient.DemoDuration & " Day Demo has expired. Please update your billing information below and click Save to continue using the Daily Docket platform."
        End If

        ' attorney record
        Dim ar As New AttorneyRecord(usr.AttorneyID)
        Me.lblBarNumber.Text = ar.BarNumber
        Me.lblName.Text = ar.Name
        Me.lblAddress.Text = ar.Address1 & "<br/>" & ar.Address2 & "<br/>" & ar.Address3 & "<br/>" & ar.City & ", " & ar.State & "  " & ar.ZipCode
        Me.lblWorkPhone.Text = ar.WorkPhone
        Me.lblEmail.Text = ar.Email
        Me.lblLicenseDate.Text = ar.LicenseDate.ToLongDateString
    End Sub

    Private Sub LoadZipCode()
        Dim svc As New ZipLookup.USZip

        If Me.txtZipCode.Text = "" Then
            'Me.ddlBillingState.Items.Clear()
        Else
            Dim xNode As System.Xml.XmlNode = svc.GetInfoByZIP(Me.txtZipCode.Text)

            Me.txtCity.Text = xNode.Item("Table").Item("CITY").InnerText
            Me.ddlBillingState.SelectedValue = xNode.Item("Table").Item("STATE").InnerText.Trim.ToUpper
        End If
    End Sub

    Private Sub LoadLists()
        Dim cn As New SqlClient.SqlConnection(ConnectionString)

        Try
            Me.ddlExpYear.Items.Clear()
            For x As Integer = Now.Year To Now.Year + 20
                Me.ddlExpYear.Items.Add(New DropDownListItem(x.ToString, x.ToString))
            Next

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


            Me.ddlClient.Items.Clear()
            Dim cmd As New SqlClient.SqlCommand("SELECT [ID], [xName] FROM [Clients];", cn)
            If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
            Dim rs As SqlClient.SqlDataReader = cmd.ExecuteReader
            Do While rs.Read
                Me.ddlClient.Items.Add(New RadComboBoxItem(rs("xName").ToString, rs("ID").ToString))
            Loop
            cmd.Cancel()
            rs.Close()

            Me.ddlPermissions.Items.Clear()
            enumValues = System.[Enum].GetValues(GetType(Enums.SystemUserPermissions))
            For Each resource As Enums.SystemUserPermissions In enumValues
                If resource <> Enums.SystemUserPermissions.Solvtopia Then
                    Me.ddlPermissions.Items.Add(New RadComboBoxItem(resource.ToString, CStr(resource)))
                End If
            Next

            Me.ddlDeviceType.Items.Clear()
            enumValues = System.[Enum].GetValues(GetType(Enums.UserPlatform))
            For Each resource As Enums.UserPlatform In enumValues
                Me.ddlDeviceType.Items.Add(New RadComboBoxItem(resource.ToString, CStr(resource)))
            Next

            Me.ddlSupervisor.Items.Clear()
            Dim sWhere As String = ""
            If App.CurrentUser.Permissions <> Enums.SystemUserPermissions.Solvtopia Then
                sWhere = " AND [xClientID] = " & App.CurrentClient.ID
            End If
            cmd = New SqlClient.SqlCommand("SELECT [ID], [xName] FROM [Users] WHERE [xPermissions] = '" & Enums.SystemUserPermissions.Supervisor.ToString & "'" & sWhere, cn)
            If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
            rs = cmd.ExecuteReader
            Do While rs.Read
                Me.ddlSupervisor.Items.Add(New RadComboBoxItem(rs("xName").ToString, rs("ID").ToString))
            Loop
            cmd.Cancel()
            rs.Close()

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(App.CurrentUser.ID, App.CurrentClient.ID, If(Me.Page.OnMobile, Enums.ProjectName.MobileApp, Enums.ProjectName.Builder)))
        Finally
            cn.Close()
        End Try
    End Sub

    Private Sub SetupForm()
        If App.CurrentUser.Permissions = Enums.SystemUserPermissions.Solvtopia Then
            Me.ddlClient.Enabled = True

            Me.txtDeviceID.Enabled = True
            Me.txtOneSignal.Enabled = True
            Me.chkApiAccess.Enabled = True
            Me.chkWebAccess.Enabled = True

            Dim usr As New SystemUser(Me.EditId)
            If usr.Permissions = Enums.SystemUserPermissions.Solvtopia Then
                Me.RadTabStrip1.Tabs(1).Visible = False
                Me.RadTabStrip1.Tabs(2).Visible = False
                Me.RadTabStrip1.Tabs(3).Visible = False
                Me.RadTabStrip1.Tabs(4).Visible = False
            Else
                Me.RadTabStrip1.Tabs(1).Visible = True
                Me.RadTabStrip1.Tabs(2).Visible = True
                Me.RadTabStrip1.Tabs(3).Visible = True
                Me.RadTabStrip1.Tabs(4).Visible = True
                Me.btnCancelService.Visible = True
            End If
        Else
            Me.ddlClient.Enabled = False

            Me.txtDeviceID.Enabled = False
            Me.txtOneSignal.Enabled = False
            Me.chkApiAccess.Enabled = False
            Me.chkWebAccess.Enabled = False

            Me.RadTabStrip1.Tabs(1).Visible = True
            Me.RadTabStrip1.Tabs(2).Visible = True
            Me.RadTabStrip1.Tabs(3).Visible = True
            Me.RadTabStrip1.Tabs(4).Visible = True
            Me.btnCancelService.Visible = True
        End If

        Me.ddlSupervisor.Enabled = (CType(Me.ddlPermissions.SelectedValue, Enums.SystemUserPermissions) = Enums.SystemUserPermissions.Technician)
        If Not Me.ddlSupervisor.Enabled Then
            Me.ddlSupervisor.SelectedIndex = -1
        End If

        If Me.ddlNotificationType.SelectedValue = CStr(Enums.NotificationType.Email) Then
            Me.ddlNotificationLevel.SelectedValue = CStr(Enums.NotificationLevel.Detail)
            Me.lblMaxSMSPerBatch.Visible = False
            Me.txtMaxSMSPerBatch.Visible = False
        Else
            Me.lblMaxSMSPerBatch.Visible = (Me.ddlNotificationLevel.SelectedValue = CStr(Enums.NotificationLevel.Detail))
            Me.txtMaxSMSPerBatch.Visible = Me.lblMaxSMSPerBatch.Visible
        End If

        Select Case CType(Me.ddlPermissions.SelectedValue, Enums.SystemUserPermissions)
            Case Enums.SystemUserPermissions.Administrator, Enums.SystemUserPermissions.SystemAdministrator
                Me.imgAvatar.ImageUrl = "~/images/icon_administrator_avatar.png"
            Case Enums.SystemUserPermissions.Technician
                Me.imgAvatar.ImageUrl = "~/images/icon_technician_avatar.png"
            Case Enums.SystemUserPermissions.Solvtopia
                Me.imgAvatar.ImageUrl = "~/images/icon_72.png"
            Case Enums.SystemUserPermissions.Supervisor
                Me.imgAvatar.ImageUrl = "~/images/icon_supervisor_avatar.png"
            Case Else
                Me.imgAvatar.ImageUrl = "~/images/icon_user_avatar.png"
        End Select
    End Sub

    Private Function SaveChanges() As Boolean
        Dim retVal As Boolean = True

        Dim cn As New SqlClient.SqlConnection(ConnectionString)

        Try
            Dim usr As New SystemUser(Me.EditId)
            Dim ar As New AttorneyRecord(usr.AttorneyID)
            Dim cl As New SystemClient(usr.ClientID)

            usr.ID = Me.EditId
            usr.Name = Me.txtName.Text.Trim
            usr.Email = Me.txtEmail.Text.Trim
            usr.Password = Me.txtPassword.Text.Trim
            Dim lstNumbers As New List(Of String)
            Dim splitter As Char = CChar(vbCrLf)
            If Not Me.txtMobileNumber.Text.Contains(splitter) Then splitter = CChar(vbLf)
            For Each n As String In Me.txtMobileNumber.Text.Split(splitter)
                If Not lstNumbers.Contains(n.FixPhoneNumber) Then lstNumbers.Add(n.FixPhoneNumber)
            Next
            usr.MobileNumbers = lstNumbers

            If usr.Permissions <> Enums.SystemUserPermissions.Solvtopia Then
                usr.BillingName = Me.txtNameCard.Text
                usr.BillingAddress1 = Me.txtAddress1.Text
                usr.BillingAddress2 = Me.txtAddress2.Text
                usr.BillingCity = Me.txtCity.Text
                usr.BillingState = Me.ddlBillingState.SelectedValue
                usr.BillingZipCode = Me.txtZipCode.Text
                usr.CCType = CType(Me.ddlCCType.SelectedValue, Enums.CreditCardType)
                usr.CCNumber = Me.txtCCNumber.Text
                usr.CCExpirationMonth = Me.ddlExpMonth.SelectedValue.ToInteger
                usr.CCExpirationYear = Me.ddlExpYear.SelectedValue.ToInteger
                usr.CCCVC = Me.txtCVV.Text

                usr.NotificationType = CType(Me.ddlNotificationType.SelectedValue, Enums.NotificationType)
                usr.NotificationLevel = CType(Me.ddlNotificationLevel.SelectedValue, Enums.NotificationLevel)
                If Me.txtMaxSMSPerBatch.Text = "" Then Me.txtMaxSMSPerBatch.Text = "5"
                usr.MaxNotificationsPerBatch = Me.txtMaxSMSPerBatch.Text.ToInteger

                ' get the device type
                If Me.UserPlatform <> Enums.UserPlatform.Desktop And App.CurrentUser.Permissions <> Enums.SystemUserPermissions.Solvtopia Then usr.MobileDeviceType = Me.UserPlatform

                ' save any changes
                usr = usr.Save

                ' update the stripe customer
                If usr.StripeCustomer = "" Then
                    usr.StripeCustomer = oPayments.CreateCustomer(usr)
                Else usr.StripeCustomer = oPayments.UpdateCustomer(usr)
                End If

                If usr.BillingLock Or App.CurrentClient.DemoDays >= App.CurrentClient.DemoDuration Then
                    Dim amt As Double = oPayments.GetChargeAmount(Payments.ChargeAmount.MonthlySubscription, usr.ID)
                    Dim desc As String = usr.Name & " - " & MonthName(Now.Month) & ", " & Now.Year

                    ' if the user is locked for billing charge their card
                    Dim success As String = oPayments.CreateCharge(amt, desc, usr)

                    ' update the billing lock if we successfully charged their card
                    If success = "" Then
                        Dim msgReceipt As New Mailer

                        ' email the receipt
                        Dim invoiceNumber As String = ar.BarNumber & "-" & Now.Month & Now.Year
                        Dim receiptBody As String = Mailer.GetHtmlBody(Enums.EmailType.Client_Receipt)
                        receiptBody = receiptBody.Replace("[ClientName]", cl.ContactName)
                        receiptBody = receiptBody.Replace("[InvoiceDate]", FormatDateTime(Now.Date, DateFormat.ShortDate))
                        receiptBody = receiptBody.Replace("[InvoiceNumber]", invoiceNumber)
                        receiptBody = receiptBody.Replace("[AmountDue]", FormatCurrency(amt, 2))
                        receiptBody = receiptBody.Replace("[MonthlyCharge]", FormatCurrency(amt, 2))
                        receiptBody = receiptBody.Replace("[ClientCharge]", FormatCurrency(0, 2))
                        receiptBody = receiptBody.Replace("[SubTotal]", FormatCurrency(amt, 2))
                        receiptBody = receiptBody.Replace("[DateSent]", FormatDateTime(Now, DateFormat.LongDate))
                        receiptBody = receiptBody.Replace("[TimeSent]", FormatDateTime(Now, DateFormat.LongTime))

                        msgReceipt.HostServer = ConfigurationManager.AppSettings("MailHost")
                        msgReceipt.UserName = ConfigurationManager.AppSettings("MailUser")
                        msgReceipt.Password = ConfigurationManager.AppSettings("MailPassword")
                        msgReceipt.Port = ConfigurationManager.AppSettings("MailPort").ToInteger
                        msgReceipt.To.Add(If(usr.ID = 40, "james@solvtopia.com", usr.Email))
                        msgReceipt.Body = receiptBody
                        msgReceipt.Subject = "Daily Docket Billing"
                        msgReceipt.From = "sales@solvtopia.com"
                        msgReceipt.HtmlBody = True
                        msgReceipt.Send()

                        ' save the invoice
                        If Not My.Computer.FileSystem.DirectoryExists(Server.MapPath("~/billing_tmp/" & ar.BarNumber & "/")) Then My.Computer.FileSystem.CreateDirectory(Server.MapPath("~/billing_tmp/" & ar.BarNumber & "/"))

                        Dim ftpFileName As String = Server.MapPath("~/billing_tmp/" & ar.BarNumber & "/" & invoiceNumber & ".pdf")
                        If My.Computer.FileSystem.FileExists(ftpFileName) Then My.Computer.FileSystem.DeleteFile(ftpFileName)
                        success = Printing.HtmlToPdf(receiptBody, ftpFileName)

                        usr.LastPaidDate = Now
                        usr.BillingLock = False
                        cl.DemoStartDate = New Date(2099, 12, 31)
                    Else
                        usr.BillingLock = True
                    End If
                End If
            Else
                ' solvtopia users
                cl.DemoStartDate = New Date(2099, 12, 31)
            End If

            ' save any changes
            cl = cl.Save
            usr = usr.Save

            ' update stripe
            If usr.StripeCustomer = "" Then
                oPayments.CreateCustomer(usr)
            Else oPayments.UpdateCustomer(usr)
            End If

            usr = New SystemUser(Me.EditId)

            If Me.EditId = App.CurrentUser.ID Then
                App.CurrentUser = usr
                App.CurrentClient = cl
            End If

            Try
                Dim cmd As New SqlClient.SqlCommand("EXEC [procRefreshUserIDs]", cn)
                If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
                cmd.ExecuteNonQuery()
                cmd.Cancel()
            Catch ex As Exception
            End Try

            LogHistory("User Information Updated for " & Me.txtName.Text)

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(App.CurrentUser.ID, App.CurrentClient.ID, If(Me.Page.OnMobile, Enums.ProjectName.MobileApp, Enums.ProjectName.Builder)))
            retVal = False
        Finally
            cn.Close()
        End Try

        Return retVal
    End Function

    Private Sub ClearForm()
        If Me.EditId = 0 Then
            Me.ddlClient.SelectedValue = App.CurrentClient.ID.ToString
            'Else Me.ddlClient.SelectedValue = usr.ClientID.ToString
        End If
        Me.txtName.Text = ""
        Me.txtEmail.Text = ""
        Me.txtPassword.Text = ""
        Me.ddlPermissions.SelectedIndex = -1
        Me.ddlSupervisor.SelectedIndex = -1
        Me.ddlDeviceType.SelectedValue = "100"
        Me.txtDeviceID.Text = ""
        Me.txtOneSignal.Text = ""
        Me.chkApiAccess.Checked = False
        Me.chkWebAccess.Checked = False
    End Sub

    Protected Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        If Me.SaveChanges Then
            Response.Redirect(Me.ReturnTo, False)
        Else Me.MsgBox("We were unable to save your changes.")
        End If
    End Sub

    Protected Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Response.Redirect(Me.ReturnTo, False)
    End Sub

    Private Sub txtZipCode_TextChanged(sender As Object, e As EventArgs) Handles txtZipCode.TextChanged
        Me.LoadZipCode()
    End Sub

    Private Sub btnCancelService_Click(sender As Object, e As EventArgs) Handles btnCancelService.Click
        Dim cn As New SqlClient.SqlConnection(ConnectionString)

        Try
            Dim editId As Integer = Me.EditId

            Dim usr As New SystemUser(Me.EditId)
            usr.Delete()

            ' process any refund
            Dim refundAmount As Double = 0

            ' email the customer
            Dim msg As New Mailer
            msg.HostServer = ConfigurationManager.AppSettings("MailHost")
            msg.UserName = ConfigurationManager.AppSettings("MailUser")
            msg.Password = ConfigurationManager.AppSettings("MailPassword")
            msg.Port = ConfigurationManager.AppSettings("MailPort").ToInteger
            msg.To.Add(If(usr.ID = 40, "james@solvtopia.com", usr.Email))
            msg.Body = Mailer.GetHtmlBody(Enums.EmailType.Client_Cancel_Service, usr)
            msg.Body = msg.Body.Replace("[RefundAmount]", FormatCurrency(refundAmount, 2))
            msg.Subject = "Daily Docket - Service Canceled"
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
            msg.Subject = "Daily Docket - Service Canceled"
            msg.From = "sales@solvtopia.com"
            msg.HtmlBody = True
            msg.Send()

            ' sign out and return to the dashboard
            App.CurrentUser = New SystemUser
            App.CurrentAttorney = New AttorneyRecord
            App.CurrentClient = New SystemClient
            Dim usrString As String = If(Me.DeviceId <> "", "deviceid=", "uid=0")
            Response.Redirect("~/Default.aspx?" & usrString & "&signout=1", False)

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(App.CurrentUser.ID, App.CurrentClient.ID, Enums.ProjectName.Builder))
        Finally
            cn.Close()
        End Try
    End Sub
End Class