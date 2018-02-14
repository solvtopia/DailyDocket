Imports Telerik.Web.UI

Public Class Dashboard
    Inherits builderMasterPage

#Region "Events"

    Public Event NewModuleClicked(ByVal folderId As Integer)
    Public Event FolderClicked(ByVal editId As Integer)

#End Region

#Region "Workers"

    Public Sub ShowProfile()
        Try
            Dim usrString As String = If(Me.Page.OnMobile, "deviceid=" & Me.DeviceId, "uid=" & Me.UserId)
            Dim url As String = "~/account/Profile.aspx?" & usrString
            Response.Redirect(url, False)

        Catch ex As Exception
            Exit Sub
        End Try
    End Sub

    Public Sub ShowNewsEditor()
        Try
            Dim usrString As String = If(Me.Page.OnMobile, "deviceid=" & Me.DeviceId, "uid=" & Me.UserId)
            Dim url As String = "~/account/NewsEditor.aspx?" & usrString
            Response.Redirect(url, False)

        Catch ex As Exception
            Exit Sub
        End Try
    End Sub

    Private Sub LoadNews()
        'Me.SqlDataSource1.SelectCommand = "SELECT Top(5) * FROM [Sys_News] ORDER BY [dtInserted] DESC;"
    End Sub

#End Region

#Region "Properties"

    Public ReadOnly Property TopRightPanel As Panel
        Get
            Return Me.pnlTopRight
        End Get
    End Property
    Public ReadOnly Property UserInfoPanel As Panel
        Get
            Return Me.pnlUserInfo
        End Get
    End Property
    Public ReadOnly Property MenuAjaxPanel As RadAjaxPanel
        Get
            Return Me.MenuAjaxPanel1
        End Get
    End Property
    Public ReadOnly Property MenuBar As PlaceHolder
        Get
            Return Me.pnlMenu
        End Get
    End Property
    Public ReadOnly Property Breadcrumbs As PlaceHolder
        Get
            Return Me.pnlBreadcrumbs
        End Get
    End Property
    Public ReadOnly Property DeviceId As String
        Get
            Return Request.QueryString("deviceid")
        End Get
    End Property
    Public ReadOnly Property UserId As Integer
        Get
            Return Request.QueryString("uid").ToInteger
        End Get
    End Property

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then

        End If

        Me.Menu.UrlString = If(Me.Page.OnMobile, "deviceid=" & Me.DeviceId, "uid=" & Me.UserId)

        ' load the user info
        Me.lblUserInfo_Name.Text = App.CurrentUser.Name
        Me.lblUserInfo_Name2.Text = Me.lblUserInfo_Name.Text
        Me.lblUserInfo_Name3.Text = Me.lblUserInfo_Name.Text
        Me.lblUserInfo_Email.Text = App.CurrentUser.Email
        'Me.pbrMain.Items(2).Visible = App.CurrentUser.IsAdminUser
        If App.CurrentClient.Approved = Enums.SystemMode.Demo Then
            Dim daysToShow As Long = DateDiff(DateInterval.Day, App.CurrentClient.DemoStartDate, Now.Date)
            If daysToShow < 0 Then daysToShow = App.CurrentClient.DemoDuration
            'Me.lblDemo.Text = "You have " & daysToShow & " Day(s) remaining on your Demo ... We hope you're enjoying your experience with our software!"
        End If
        Select Case App.CurrentUser.Permissions
            Case Enums.SystemUserPermissions.User
                Me.lblUserInfo_Role.Text = "System User"
            Case Enums.SystemUserPermissions.Administrator
                Me.lblUserInfo_Role.Text = "Administrator"
            Case Enums.SystemUserPermissions.SystemAdministrator
                Me.lblUserInfo_Role.Text = "System Administrator"
            Case Enums.SystemUserPermissions.Solvtopia
                Me.lblUserInfo_Role.Text = "Solvtopia User"
        End Select
        If My.Computer.FileSystem.FileExists(HttpContext.Current.Server.MapPath("~/images/gallery/clients/" & App.CurrentClient.ID & "/logo.png")) Then
            Me.ibtnAvitar.ImageUrl = "~/images/gallery/clients/" & App.CurrentClient.ID & "/logo.png"
        Else Me.ibtnAvitar.ImageUrl = App.CurrentUser.ImageUrl
        End If
        Me.ibtnAvitar2.ImageUrl = Me.ibtnAvitar.ImageUrl
        Me.ibtnAvitar3.ImageUrl = Me.ibtnAvitar.ImageUrl
        'Me.pbrMain.Items(0).Expanded = (App.CurrentUser.ID > 0)
        'Me.pbrMain.Items(1).Expanded = (App.CurrentUser.ID > 0)
        'Me.ibtnEditNews.Visible = (App.CurrentUser.Permissions = Enums.SystemUserPermissions.Solvtopia)
        Me.LoadNews()
    End Sub

    Private Sub btnProfile_Click(sender As Object, e As EventArgs) Handles btnProfile.Click
        Me.ShowProfile()
    End Sub

    Private Sub btnSignOut_Click(sender As Object, e As EventArgs) Handles btnSignOut.Click
        ' remove the device id from the user's list
        Dim lstDeviceIds As New List(Of String)
        For Each s As String In App.CurrentUser.MobileDeviceIds
            If s <> Me.DeviceId And s.Trim <> "" Then lstDeviceIds.Add(s)
        Next
        App.CurrentUser.MobileDeviceIds = lstDeviceIds
        App.CurrentUser.Save()

        App.CurrentUser = New SystemUser
        App.CurrentAttorney = New AttorneyRecord
        App.CurrentClient = New SystemClient
        Dim usrString As String = If(Me.Page.OnMobile, "deviceid=", "uid=0")
        Response.Redirect("~/Default.aspx?" & usrString & "&signout=1", False)
    End Sub
End Class