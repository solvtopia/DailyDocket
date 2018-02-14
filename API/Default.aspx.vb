Public Class _Default
    Inherits System.Web.UI.Page

#Region "Workers"

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.Redirect("https://access.dailycourtdocket.com/documentation/api/default.aspx", False)
    End Sub

    Protected Sub lnkInputController_Click(sender As Object, e As EventArgs) Handles lnkInputController.Click
        Me.Master.ShowInformationWindow("Workers/InputController.aspx")
    End Sub

    Private Sub lnkOutputController_Click(sender As Object, e As EventArgs) Handles lnkOutputController.Click
        Me.Master.ShowInformationWindow("Workers/OutputController.aspx")
    End Sub
End Class