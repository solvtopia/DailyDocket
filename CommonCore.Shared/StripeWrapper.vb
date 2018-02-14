Imports DailyDocket.CommonCore.Shared.Common

Public Class Payments

    Public Sub New(ByVal stripeMode As String)
        Me.Mode = stripeMode
    End Sub

    Private Mode As String

#Region "Keys"

    Public StripeTestSecretKey As String = "sk_test_tL0CmThgmj15YkTlYoolKFVq"
    Public StripeTestPublishableKey As String = "pk_test_0DEE4ROo1KZGoHzMBttVjcy0"
    Public StripeLiveSecretKey As String = "sk_live_2l7VUI0WXQVxFEaGdhCbhPF8"
    Public StripeLivePublishableKey As String = "pk_live_mQ0nrMIIFesJkTPO7egdrsgt"

#End Region

#Region "Enums"

    Public Enum BalanceType
        Available
        Pending
    End Enum

    Public Enum ChargeAmount
        MonthlySubscription = 1
        ClientSMS = 2
    End Enum

    Public Enum EntityType
        Company
        Individual
    End Enum

#End Region

    Public Structure ManagedAccountInfo
        Public Email As String
        Public BirthDay As Integer
        Public BirthMonth As Integer
        Public BirthYear As Integer
        Public FirstName As String
        Public LastName As String
        Public IPAddress As String
        Public BusinessName As String
        Public BusinessTaxID As String
        Public AccountNumber As String
        Public AccountRoutingNumber As String
        Public Address As String
        Public City As String
        Public State As String
        Public Zip As String
        Public Last4SSN As String
        Public EntityType As EntityType
    End Structure



    Public Function GetBalance(ByVal type As BalanceType) As Double
        ' use only secret keys for the api, not publishable
        If Me.Mode.ToLower = "live" Then
            Stripe.StripeConfiguration.SetApiKey(Me.StripeLiveSecretKey)
        Else Stripe.StripeConfiguration.SetApiKey(Me.StripeTestSecretKey)
        End If

        Dim retVal As Double = 0

        Try
            Dim balanceService As New Stripe.StripeBalanceService
            Dim rs As Stripe.StripeBalance = balanceService.Get

            Select Case type
                Case BalanceType.Pending
                    For Each a As Stripe.StripeBalanceAmount In rs.Pending
                        retVal += a.Amount
                    Next
                Case BalanceType.Available
                    For Each a As Stripe.StripeBalanceAmount In rs.Available
                        retVal += a.Amount
                    Next
            End Select

            ' amounts return in hundreds (no decimals) e.g. 10.00 is 1000
            retVal /= 100

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(Enums.ProjectName.CommonCoreShared))
        End Try

        Return retVal
    End Function

    Public Function CreateCustomer(ByVal usr As SystemUser) As String
        ' use only secret keys for the api, not publishable
        If Me.Mode.ToLower = "live" Then
            Stripe.StripeConfiguration.SetApiKey(Me.StripeLiveSecretKey)
        Else Stripe.StripeConfiguration.SetApiKey(Me.StripeTestSecretKey)
        End If

        Dim retval As String = ""

        Try
            Dim myCustomer As New Stripe.StripeCustomerCreateOptions

            myCustomer.Email = usr.Email
            myCustomer.Description = usr.Name

            ' setting up the card
            Dim card As New Stripe.SourceCard
            card.Number = usr.CCNumber
            Dim expYear As Integer = usr.CCExpirationYear
            card.ExpirationYear = expYear
            card.ExpirationMonth = usr.CCExpirationMonth
            card.AddressCountry = "US" ' Optional
            card.AddressLine1 = usr.BillingAddress1 ' Optional
            card.AddressLine2 = usr.BillingAddress2 ' Optional
            card.AddressCity = usr.BillingCity ' Optional
            card.AddressState = usr.BillingState ' Optional
            card.AddressZip = usr.BillingZipCode ' Optional
            card.Name = usr.BillingName ' Optional
            card.Cvc = usr.CCCVC ' Optional

            myCustomer.SourceCard = card

            Dim customerService As New Stripe.StripeCustomerService
            Dim stripeCustomer As Stripe.StripeCustomer = customerService.Create(myCustomer)

            ' save the customer id in the user object
            usr.StripeCustomer = stripeCustomer.Id
            usr.Save()

            retval = usr.StripeCustomer

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(usr.ID, usr.ClientID, Enums.ProjectName.CommonCoreShared))
            retval = ""
        End Try

        Return retval
    End Function

    Public Function UpdateCustomer(ByVal usr As SystemUser) As String
        ' use only secret keys for the api, not publishable
        If Me.Mode.ToLower = "live" Then
            Stripe.StripeConfiguration.SetApiKey(Me.StripeLiveSecretKey)
        Else Stripe.StripeConfiguration.SetApiKey(Me.StripeTestSecretKey)
        End If

        Dim retVal As String = ""

        Try
            Dim myCustomer As New Stripe.StripeCustomerUpdateOptions
            myCustomer.Email = usr.Email
            myCustomer.Description = usr.Name

            ' setting up the card
            Dim card As New Stripe.SourceCard
            card.Number = usr.CCNumber
            Dim expYear As Integer = usr.CCExpirationYear
            card.ExpirationYear = expYear
            card.ExpirationMonth = usr.CCExpirationMonth
            card.AddressCountry = "US" ' Optional
            card.AddressLine1 = usr.BillingAddress1 ' Optional
            card.AddressLine2 = usr.BillingAddress2 ' Optional
            card.AddressCity = usr.BillingCity ' Optional
            card.AddressState = usr.BillingState ' Optional
            card.AddressZip = usr.BillingZipCode ' Optional
            card.Name = usr.BillingName ' Optional
            card.Cvc = usr.CCCVC ' Optional

            myCustomer.SourceCard = card

            Dim customerService As New Stripe.StripeCustomerService
            Dim stripeCustomer As Stripe.StripeCustomer = customerService.Update(usr.StripeCustomer, myCustomer)

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(usr.ID, usr.ClientID, Enums.ProjectName.CommonCoreShared))
            retVal = ex.ToString
        End Try

        Return retVal
    End Function

    Private Function LookupCustomer(ByVal customerId As String) As Stripe.StripeCustomer
        ' use only secret keys for the api, not publishable
        If Me.Mode.ToLower = "live" Then
            Stripe.StripeConfiguration.SetApiKey(Me.StripeLiveSecretKey)
        Else Stripe.StripeConfiguration.SetApiKey(Me.StripeTestSecretKey)
        End If

        Dim retval As New Stripe.StripeCustomer

        Dim customerService As New Stripe.StripeCustomerService
        retval = customerService.Get(customerId)

        Return retval
    End Function

    Public Function GetChargeAmount(ByVal amount As ChargeAmount, ByVal userId As Integer) As Double
        Dim retVal As Double = 0

        Dim cn As New SqlClient.SqlConnection(ConnectionString)

        Try
            ' get the charge amount and description from the database
            Dim cmd As New SqlClient.SqlCommand("procPricing", cn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@userId", userId)
            cmd.Parameters.AddWithValue("@chargeId", CInt(amount))
            If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
            Dim rs As SqlClient.SqlDataReader = cmd.ExecuteReader
            If rs.Read Then
                retVal = rs("UserAmount").ToString.ToDouble
            End If
            cmd.Cancel()
            rs.Close()

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(Enums.ProjectName.CommonCoreShared))
        Finally
            cn.Close()
        End Try

        Return retVal
    End Function
    Public Function CreateCharge(ByVal amount As ChargeAmount, ByVal usr As SystemUser) As String
        Dim cn As New SqlClient.SqlConnection(ConnectionString)

        Dim amt As Double = 0
        Dim desc As String = ""

        Try
            ' get the charge amount and description from the database
            Dim cmd As New SqlClient.SqlCommand("procPricing", cn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@userId", usr.ID)
            cmd.Parameters.AddWithValue("@chargeId", CInt(amount))
            If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
            Dim rs As SqlClient.SqlDataReader = cmd.ExecuteReader
            If rs.Read Then
                amt = rs("UserAmount").ToString.ToDouble
                desc = rs("Description").ToString
            End If
            cmd.Cancel()
            rs.Close()

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(usr.ID, usr.ClientID, Enums.ProjectName.CommonCoreShared))
        Finally
            cn.Close()
        End Try

        Return CreateCharge(amt, desc, usr)
    End Function
    Public Function CreateCharge(ByVal amount As Double, ByVal description As String, ByVal usr As SystemUser) As String
        ' use only secret keys for the api, not publishable
        If Me.Mode.ToLower = "live" Then
            Stripe.StripeConfiguration.SetApiKey(Me.StripeLiveSecretKey)
        Else Stripe.StripeConfiguration.SetApiKey(Me.StripeTestSecretKey)
        End If

        Dim retVal As String = ""

        Dim cn As New SqlClient.SqlConnection(ConnectionString)

        Try
            ' make sure the customer information is updated with stripe
            If usr.StripeCustomer = "" Then
                CreateCustomer(usr)
            Else UpdateCustomer(usr)
            End If

            ' pull the latest user info to make sure we get the stripe customer id
            usr = New SystemUser(usr.ID)

            ' setting up the card
            Dim myCharge As New Stripe.StripeChargeCreateOptions

            ' always set these properties
            ' amounts need to be in hundreds (no decimals) e.g. 10.00 is 1000
            myCharge.Amount = (amount * 100).ToInteger
            myCharge.Currency = "usd"

            ' set this if you want to
            myCharge.Description = description

            ' set this property if using a customer
            myCharge.CustomerId = usr.StripeCustomer

            ' set this if you have your own application fees (you must have your application configured first within Stripe)
            'myCharge.ApplicationFee = 0

            ' (Not required) set this to false if you don't want to capture the charge yet - requires you call capture later
            myCharge.Capture = True

            Dim chargeService As New Stripe.StripeChargeService
            Dim stripecharge As Stripe.StripeCharge = chargeService.Create(myCharge)

            ' log the payment
            Dim s As String = stripecharge.StripeResponse.SerializeToXml

            Dim cmd As New SqlClient.SqlCommand("INSERT INTO [Sys_Payments] ([UserID], [xmlData], [Status], [Mode], [Amount], [Description], [dtInserted]) VALUES ('" & usr.ID & "', @xmlData, @Status, @Mode, @Amount, @Description, '" & Now.ToString & "');", cn)
            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@xmlData", s)
            cmd.Parameters.AddWithValue("@Status", stripecharge.Status)
            cmd.Parameters.AddWithValue("@Mode", If(stripecharge.LiveMode, "live", "test"))
            cmd.Parameters.AddWithValue("@Amount", amount)
            cmd.Parameters.AddWithValue("@Description", description)
            If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
            cmd.ExecuteNonQuery()
            cmd.Cancel()

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(usr.ID, usr.ClientID, Enums.ProjectName.CommonCoreShared))
            retVal = ex.ToString
        Finally
            cn.Close()
        End Try

        Return retVal
    End Function

    Public Function CreateManagedAccount(ByVal info As ManagedAccountInfo) As String
        ' use only secret keys for the api, not publishable
        If Me.Mode.ToLower = "live" Then
            Stripe.StripeConfiguration.SetApiKey(Me.StripeLiveSecretKey)
        Else Stripe.StripeConfiguration.SetApiKey(Me.StripeTestSecretKey)
        End If

        Dim retVal As String = ""

        Try
            Dim account As New Stripe.StripeAccountCreateOptions
            ' this Is required If it Is Not a managed account. the user Is emailed On standalone accounts,
            ' it's only used for reference on managed accounts
            account.Email = info.Email
            account.Type = Stripe.StripeAccountType.Custom
            'account.Managed = True ' Set this To True If you want a managed account (email Is Not required If this Is Set To True)
            account.Country = "US" ' defaults To your country
            account.BusinessName = If(info.BusinessName = "", info.FirstName & " " & info.LastName, info.BusinessName)

            ' setup the legal entity for identity verification
            Dim legalEntity As New Stripe.StripeAccountLegalEntityOptions
            legalEntity.BirthDay = info.BirthDay
            legalEntity.BirthMonth = info.BirthMonth
            legalEntity.BirthYear = info.BirthYear
            legalEntity.FirstName = info.FirstName
            legalEntity.LastName = info.LastName
            legalEntity.Type = info.EntityType.ToString.ToLower
            legalEntity.AddressCity = info.City
            legalEntity.AddressLine1 = info.Address
            legalEntity.AddressState = info.State
            legalEntity.AddressPostalCode = info.Zip
            legalEntity.SSNLast4 = info.Last4SSN
            If info.EntityType = EntityType.Company Then
                legalEntity.BusinessTaxId = info.BusinessTaxID
                legalEntity.BusinessName = info.BusinessName
            End If
            account.LegalEntity = legalEntity

            ' setup the external account
            Dim bankAccount As New Stripe.StripeAccountBankAccountOptions
            bankAccount.AccountNumber = info.AccountNumber
            bankAccount.RoutingNumber = info.AccountRoutingNumber
            bankAccount.Country = "US"
            account.ExternalBankAccount = bankAccount

            ' agree to the terms of service
            account.TosAcceptanceDate = Now
            account.TosAcceptanceIp = info.IPAddress

            Dim accountService As New Stripe.StripeAccountService
            Dim rs As Stripe.StripeAccount = accountService.Create(account)

            retVal = rs.Id

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(Enums.ProjectName.CommonCoreShared))
            retVal = ""
        End Try

        Return retVal
    End Function

    Public Function UpdateManagedAccount(ByVal acctID As String, ByVal info As ManagedAccountInfo) As String
        ' use only secret keys for the api, not publishable
        If Me.Mode.ToLower = "live" Then
            Stripe.StripeConfiguration.SetApiKey(Me.StripeLiveSecretKey)
        Else Stripe.StripeConfiguration.SetApiKey(Me.StripeTestSecretKey)
        End If

        Dim retVal As String = ""

        Try
            Dim account As New Stripe.StripeAccountUpdateOptions
            account.Email = info.Email
            account.BusinessName = If(info.BusinessName = "", info.FirstName & " " & info.LastName, info.BusinessName)

            ' setup the legal entity for identity verification
            Dim legalEntity As New Stripe.StripeAccountLegalEntityOptions
            legalEntity.BirthDay = info.BirthDay
            legalEntity.BirthMonth = info.BirthMonth
            legalEntity.BirthYear = info.BirthYear
            legalEntity.FirstName = info.FirstName
            legalEntity.LastName = info.LastName
            legalEntity.Type = info.EntityType.ToString.ToLower
            legalEntity.AddressCity = info.City
            legalEntity.AddressLine1 = info.Address
            legalEntity.AddressState = info.State
            legalEntity.AddressPostalCode = info.Zip
            legalEntity.SSNLast4 = info.Last4SSN
            If info.EntityType = EntityType.Company Then
                legalEntity.BusinessTaxId = info.BusinessTaxID
                legalEntity.BusinessName = info.BusinessName
            End If
            account.LegalEntity = legalEntity

            ' setup the external account
            Dim bankAccount As New Stripe.StripeAccountBankAccountOptions
            bankAccount.AccountNumber = info.AccountNumber
            bankAccount.RoutingNumber = info.AccountRoutingNumber
            bankAccount.Country = "US"
            account.ExternalBankAccount = bankAccount

            Dim accountService As New Stripe.StripeAccountService
            Dim rs As Stripe.StripeAccount = accountService.Update(acctID, account)

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(Enums.ProjectName.CommonCoreShared))
            retVal = ex.ToString
        End Try

        Return retVal
    End Function

    Public Function ListManagedAccounts() As List(Of Stripe.StripeAccount)
        ' use only secret keys for the api, not publishable
        If Me.Mode.ToLower = "live" Then
            Stripe.StripeConfiguration.SetApiKey(Me.StripeLiveSecretKey)
        Else Stripe.StripeConfiguration.SetApiKey(Me.StripeTestSecretKey)
        End If

        Dim retVal As New List(Of Stripe.StripeAccount)

        Try
            Dim accountService = New Stripe.StripeAccountService()
            Dim rs As IEnumerable(Of Stripe.StripeAccount) = accountService.List()
            ' optional StripeListOptions

            retVal = CType(rs, List(Of Stripe.StripeAccount))

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(Enums.ProjectName.CommonCoreShared))
            retVal = New List(Of Stripe.StripeAccount)
        End Try

        Return retVal
    End Function

    Public Function DeleteManagedAccount(ByVal acctID As String) As String
        ' use only secret keys for the api, not publishable
        If Me.Mode.ToLower = "live" Then
            Stripe.StripeConfiguration.SetApiKey(Me.StripeLiveSecretKey)
        Else Stripe.StripeConfiguration.SetApiKey(Me.StripeTestSecretKey)
        End If

        Dim retVal As String = ""

        Try
            Dim accountService = New Stripe.StripeAccountService()
            accountService.Delete(acctID)

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(Enums.ProjectName.CommonCoreShared))
            retVal = ex.ToString
        End Try

        Return retVal
    End Function

    Public Function CreateTransfer(ByVal destAcctID As String, ByVal amount As Double) As String
        ' use only secret keys for the api, not publishable
        If Me.Mode.ToLower = "live" Then
            Stripe.StripeConfiguration.SetApiKey(Me.StripeLiveSecretKey)
        Else Stripe.StripeConfiguration.SetApiKey(Me.StripeTestSecretKey)
        End If

        Dim retVal As String = ""

        Try
            Dim myTransfer As New Stripe.StripeTransferCreateOptions
            myTransfer.Amount = (amount * 100).ToInteger
            myTransfer.Currency = "USD"
            myTransfer.Destination = destAcctID

            Dim transferService As New Stripe.StripeTransferService
            Dim stripeTransfer As Stripe.StripeTransfer = transferService.Create(myTransfer)

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(Enums.ProjectName.CommonCoreShared))
            retVal = ex.ToString
        End Try

        Return retVal
    End Function
End Class
