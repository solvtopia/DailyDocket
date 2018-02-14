Imports DailyDocket.CommonCore.Shared.Common

Public Class SSO
    Inherits System.Web.UI.Page

#Region "Properties"

    Private ReadOnly Property Email As String
        Get
            Return Request.QueryString("e")
        End Get
    End Property
    Private ReadOnly Property Password As String
        Get
            Return Request.QueryString("p")
        End Get
    End Property
    Private ReadOnly Property SendTo As String
        Get
            Return Request.QueryString("s")
        End Get
    End Property

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Me.Email <> "" And Me.Password <> "" Then
            Dim cn As New SqlClient.SqlConnection(ConnectionString)

            Try
                Dim usr As New SystemUser(Me.Email.Trim, Me.Password.Trim, "")

                ' get the device type
                If Me.UserPlatform <> Enums.UserPlatform.Desktop Then usr.MobileDeviceType = Me.UserPlatform

                ' save any changes
                If usr.ID > 0 Then usr = usr.Save

                Dim cl As New SystemClient(usr.ClientID)

                App.CurrentUser = usr
                App.CurrentAttorney = New AttorneyRecord(usr.AttorneyID)
                App.CurrentClient = cl

                If usr.ID = 0 Then
                    Response.Redirect("~/account/Login.aspx", False)
                Else
                    If cl.Approved = Enums.SystemMode.Demo And cl.DemoDays > cl.DemoDuration Then
                        'Me.lblMessage.Text = "Your system demo period has expired.<br/><br/>Please contact sales@solvtopia.com for more information."
                    ElseIf Not App.CurrentClient.Active Then
                        'Me.lblMessage.Text = "Your client profile is currently disabled.<br/><br/>Please contact sales@solvtopia.com for more information."
                    Else
                        Dim usrString As String = "uid=" & usr.ID
                        If Me.SendTo.ToLower = "billing" Then
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
        End If
    End Sub

End Class