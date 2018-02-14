Imports DailyDocket.CommonCore.Shared.Common

Public Class Login
    Inherits builderPage

#Region "Properties"

    Private ReadOnly Property Usr As Integer
        Get
            Return Request.QueryString("usr").ToInteger
        End Get
    End Property

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.lblMessage.Text = ""
        Me.Master.Master.UserInfoPanel.Visible = False
        Me.Master.Master.TopRightPanel.Visible = False
        Me.Master.Master.Breadcrumbs.Visible = (Not Me.OnMobile)
        Me.imgLogo.Visible = Not Me.OnMobile

        If Not IsPostBack Then
            Me.txtEmail.Focus()

            If Me.Usr > 0 Then
                ' if we have a userId passed from the registration page automatically log them in
                Dim usr As New SystemUser(Me.Usr)
                Me.txtEmail.Text = usr.Email
                Me.txtPassword.Text = usr.Password
                btnLogin_Click(Nothing, New EventArgs)
            End If

            Me.LoadTip()
        End If

        Me.pnlTip.Visible = (Now.Date >= New Date(2017, 5, 26) Or Me.OnLocal)

        Me.Master.BackButton.Visible = False
    End Sub

    Private Sub LoadTip()
        If Me.litTip.Text = "" Then
            Dim cn As New SqlClient.SqlConnection(ConnectionString)

            Try
                Dim cmd As New SqlClient.SqlCommand("SELECT TOP 1 [Content] FROM [Sys_Help] WHERE [Type] LIKE 'faq' ORDER BY NEWID();", cn)
                If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
                Dim rs As SqlClient.SqlDataReader = cmd.ExecuteReader
                If rs.Read Then
                    Me.litTip.Text = rs("Content").ToString
                End If
                cmd.Cancel()
                rs.Close()

            Catch ex As Exception
                ex.WriteToErrorLog(New ErrorLogEntry(App.CurrentUser.ID, App.CurrentClient.ID, If(Me.Page.OnMobile, Enums.ProjectName.MobileApp, Enums.ProjectName.Builder)))
            Finally
                cn.Close()
            End Try
        End If
    End Sub

    Protected Sub btnLogin_Click(sender As Object, e As EventArgs) Handles btnLogin.Click
        Dim cn As New SqlClient.SqlConnection(ConnectionString)

        Try
            Dim usr As New SystemUser(Me.txtEmail.Text.Trim, Me.txtPassword.Text.Trim, "")

            ' get the device type
            If Me.UserPlatform <> Enums.UserPlatform.Desktop Then usr.MobileDeviceType = Me.UserPlatform

            ' save the device id to the list
            If Me.DeviceId <> "" And Not usr.MobileDeviceIds.Contains(Me.DeviceId) Then usr.MobileDeviceIds.Add(Me.DeviceId)

            ' save any changes
            If usr.ID > 0 Then usr = usr.Save

            Dim cl As New SystemClient(usr.ClientID)

            App.CurrentUser = usr
            App.CurrentAttorney = New AttorneyRecord(usr.AttorneyID)
            App.CurrentClient = cl

            If usr.ID = 0 Then
                Me.lblMessage.Text = "We could not find a user matching the credentials you specified.<br/><br/>If you have not registered your account yet please visit http://www.dailycourtdocket.com/registration to setup your profile."
            Else
                If cl.Approved = Enums.SystemMode.Demo And cl.DemoDays > cl.DemoDuration Then
                    Me.lblMessage.Text = "Your system demo period has expired.<br/><br/>Please contact sales@solvtopia.com for more information."
                ElseIf Not App.CurrentClient.Active Then
                    Me.lblMessage.Text = "Your client profile is currently disabled.<br/><br/>Please contact sales@solvtopia.com for more information."
                Else
                    Dim usrString As String = If(Me.DeviceId <> "", "deviceid=" & Me.DeviceId, "uid=" & usr.ID)
                    If usr.BillingLock Or cl.DemoDays >= cl.DemoDuration Then
                        Response.Redirect("~/account/Profile.aspx?" & usrString & "&tab=2", False)
                    Else
                        LogHistory("User Login " & usr.Name)
                        Response.Redirect("~/Default.aspx?" & usrString & "&signout=0", False)
                    End If
                    Exit Sub
                End If
            End If

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(If(Me.Page.OnMobile, Enums.ProjectName.MobileApp, Enums.ProjectName.Builder)))
        Finally
            cn.Close()
        End Try
    End Sub
End Class