Public Class Menu
    Inherits System.Web.UI.UserControl

#Region "Properties"

    Public Property UrlString As String
        Get
            Return Me.lblUrlString.Text
        End Get
        Set(value As String)
            Me.lblUrlString.Text = value
        End Set
    End Property

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.pnlAdmin.Visible = (App.CurrentUser.Permissions = Enums.SystemUserPermissions.Solvtopia)

        ' message center is not available on phones
        Me.lnkMessages.Visible = (Not Me.pnlAdmin.Visible And Not Me.Page.OnPhone)

        ' help topics are not available on phones
        Me.lnkHelp.Visible = (Not Me.Page.OnPhone)
        If Me.lnkHelp.Visible Then
            Me.lnkHelp.Visible = (Not App.CurrentUser.Permissions = Enums.SystemUserPermissions.Solvtopia)
        End If

        Me.pnlLoginOptions.Visible = False
        Me.pnlMainOptions.Visible = False

        ' show specific options depending on the page we are displaying
        Dim currentUrl As String = Request.Url.ToString.ToLower
        Select Case True
            Case currentUrl.Contains("login.aspx")
                Me.pnlLoginOptions.Visible = True
                Me.lnkDashboard.Visible = False
                Me.lnkLogin.Visible = False
            Case currentUrl.Contains("loginhelp.aspx")
                Me.pnlLoginOptions.Visible = True
                Me.lnkDashboard.Visible = False
                Me.lnkForgotPassword.Visible = False
            Case currentUrl.Contains("register.aspx")
                Me.pnlLoginOptions.Visible = True
                Me.lnkDashboard.Visible = False
                Me.lnkForgotPassword.Visible = False
            Case currentUrl.Contains("preview.aspx")
                Me.pnlLoginOptions.Visible = True
                Me.lnkDashboard.Visible = False
                Me.lnkForgotPassword.Visible = False
            Case Else
                Me.pnlMainOptions.Visible = True
        End Select

        Me.SetActive()
    End Sub

    Private Sub SetActive()
        Dim currentUrl As String = Request.Url.ToString.ToLower
        Select Case True
            Case currentUrl.Contains("documentation/default.aspx")
                Me.liHelp.Attributes.Add("class", "active")
            Case currentUrl.Contains("faq.aspx")
                Me.liFAQ.Attributes.Add("class", "active")
            Case currentUrl.Contains("messagecenter.aspx")
                Me.liMessages.Attributes.Add("class", "active")
            Case currentUrl.Contains("salesreport.aspx")
                Me.liSalesReport.Attributes.Add("class", "active")
            Case currentUrl.Contains("users.aspx")
                Me.liUsers.Attributes.Add("class", "active")
            Case currentUrl.Contains("sendmail.aspx")
                Me.liMailer.Attributes.Add("class", "active")
            Case currentUrl.Contains("logs.aspx")
                Me.liLogs.Attributes.Add("class", "active")
            Case currentUrl.Contains("helptopics.aspx")
                Me.liHelpTopics.Attributes.Add("class", "active")
            Case currentUrl.Contains("login.aspx")
                Me.liLogin.Attributes.Add("class", "active")
            Case currentUrl.Contains("loginhelp.aspx")
                Me.liForgotPassword.Attributes.Add("class", "active")
            Case currentUrl.Contains("register.aspx")
                Me.liRegister.Attributes.Add("class", "active")
            Case Else
                Me.liDashboard.Attributes.Add("class", "active")
        End Select
    End Sub

    Private Sub lnkDashboard_Click(sender As Object, e As EventArgs) Handles lnkDashboard.Click
        Response.Redirect("~/Default.aspx?" & Me.UrlString, False)
    End Sub

    Private Sub lnkHelp_Click(sender As Object, e As EventArgs) Handles lnkHelp.Click
        Response.Redirect("~/documentation/Default.aspx?" & Me.UrlString, False)
    End Sub

    Private Sub lnkMessages_Click(sender As Object, e As EventArgs) Handles lnkMessages.Click
        Response.Redirect("~/account/MessageCenter.aspx?" & Me.UrlString, False)
    End Sub

    Private Sub lnkSalesReport_Click(sender As Object, e As EventArgs) Handles lnkSalesReport.Click
        Response.Redirect("~/admin/SalesReport.aspx?" & Me.UrlString, False)
    End Sub

    Private Sub lnkUsers_Click(sender As Object, e As EventArgs) Handles lnkUsers.Click
        Response.Redirect("~/admin/Users.aspx?" & Me.UrlString, False)
    End Sub

    Private Sub lnkMailer_Click(sender As Object, e As EventArgs) Handles lnkMailer.Click
        Response.Redirect("~/admin/SendMail.aspx?" & Me.UrlString, False)
    End Sub

    Private Sub lnkLogs_Click(sender As Object, e As EventArgs) Handles lnkLogs.Click
        Response.Redirect("~/admin/Logs.aspx?" & Me.UrlString, False)
    End Sub

    Private Sub lnkForgotPassword_Click(sender As Object, e As EventArgs) Handles lnkForgotPassword.Click
        Response.Redirect("~/account/LoginHelp.aspx", False)
    End Sub

    Private Sub lnkLogin_Click(sender As Object, e As EventArgs) Handles lnkLogin.Click
        Response.Redirect("~/account/Login.aspx", False)
    End Sub

    Private Sub lnkRegister_Click(sender As Object, e As EventArgs) Handles lnkRegister.Click
        Response.Redirect("~/Register.aspx?" & Me.UrlString & "&duration=7", False)
    End Sub

    Private Sub lnkFaq_Click(sender As Object, e As EventArgs) Handles lnkFaq.Click
        Response.Redirect("~/documentation/faq.aspx?" & Me.UrlString, False)
    End Sub

    Private Sub lnkHelpTopics_Click(sender As Object, e As EventArgs) Handles lnkHelpTopics.Click
        Response.Redirect("~/admin/HelpTopics.aspx?" & Me.UrlString, False)
    End Sub
End Class