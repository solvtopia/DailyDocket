Public Class Register
    Inherits builderPage

#Region "Properties"

    Private ReadOnly Property Duration As Integer
        Get
            If Request.QueryString("duration") = "" Then
                Return 7
            Else Return Request.QueryString("duration").ToInteger
            End If
        End Get
    End Property

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.Master.Master.UserInfoPanel.Visible = False
        Me.Master.Master.TopRightPanel.Visible = False
        Me.Master.Master.Breadcrumbs.Visible = (Not Me.OnMobile)

        If Not IsPostBack Then
            Me.fraRegister.ContentUrl = "~/account/Registration.aspx?duration=" & Me.Duration.ToString
        End If

        Me.Master.BackButton.Visible = (Me.OnMobile)
        AddHandler Me.Master.BackButton.Click, AddressOf imgBack_Click
    End Sub

    Protected Sub imgBack_Click(sender As Object, e As ImageClickEventArgs)
        Dim usrString As String = If(Me.DeviceId <> "", "deviceid=" & Me.DeviceId, "uid=" & Me.UserId)
        Response.Redirect("~/Default.aspx?" & usrString, False)
    End Sub
End Class