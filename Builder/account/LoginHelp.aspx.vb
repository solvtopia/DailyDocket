Imports System.Configuration.ConfigurationManager
Imports DailyDocket.CommonCore.Shared.Common

Public Class LoginHelp
    Inherits builderPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.lblMessage.Text = ""
        Me.Master.Master.UserInfoPanel.Visible = False
        Me.Master.Master.TopRightPanel.Visible = False
        Me.Master.Master.Breadcrumbs.Visible = (Not Me.OnMobile)

        If Not IsPostBack Then
            Me.txtEmail.Focus()
        End If

        Me.Master.BackButton.Visible = False
    End Sub

    Protected Sub btnLogin_Click(sender As Object, e As EventArgs) Handles btnLogin.Click
        Dim cn As New SqlClient.SqlConnection(ConnectionString)

        Try
            Dim cmd As New SqlClient.SqlCommand("SELECT [xPassword] FROM [Users] WHERE [xEmail] LIKE @email or [xMobileNumber] LIKE @mobileNumber;", cn)
            If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
            cmd.Parameters.AddWithValue("@email", Me.txtEmail.Text.Trim)
            cmd.Parameters.AddWithValue("@mobileNumber", Me.txtMobileNumber.Text.Trim.Replace(" ", "").Replace("-", "").Replace("(", ""))
            Dim rs As SqlClient.SqlDataReader = cmd.ExecuteReader
            If rs.Read Then
                ' email the customer
                Dim msg As New Mailer
                msg.HostServer = AppSettings("MailHost")
                msg.UserName = AppSettings("MailUser")
                msg.Password = AppSettings("MailPassword")
                msg.Port = AppSettings("MailPort").ToInteger
                msg.To.Add(Me.txtEmail.Text)
                msg.Body = Mailer.GetHtmlBody(Enums.EmailType.Client_Forgot_Password, rs("xPassword").ToString)
                msg.Subject = "Daily Docket - Forgot Password"
                msg.From = "sales@solvtopia.com"
                msg.HtmlBody = True
                msg.Send()
            Else
                Me.lblMessage.Text = "We could not find any accounts with the email address or mobile number you provided."
            End If
            cmd.Cancel()
            rs.Close()

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(If(Me.Page.OnMobile, Enums.ProjectName.MobileApp, Enums.ProjectName.Builder)))
        Finally
            cn.Close()
        End Try
    End Sub
End Class