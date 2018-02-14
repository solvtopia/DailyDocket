Public Class MissingCase
    Inherits builderPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then

        End If

        Me.Master.BackButton.Visible = (Me.OnMobile)
        AddHandler Me.Master.BackButton.Click, AddressOf imgBack_Click
    End Sub

    Protected Sub imgBack_Click(sender As Object, e As ImageClickEventArgs)
        Dim usrString As String = If(Me.DeviceId <> "", "deviceid=" & Me.DeviceId, "uid=" & Me.UserId)
        Response.Redirect("~/Default.aspx?" & usrString, False)
    End Sub

End Class