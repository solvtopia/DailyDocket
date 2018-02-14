Public Class InformationPopUp
    Inherits System.Web.UI.MasterPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Page.RunClientScript(CommonCore.Common.ReturnToParentScript)
        Exit Sub
    End Sub
End Class