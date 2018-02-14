Public Class Layout
    Inherits builderMasterPage

#Region "Events"

#End Region

#Region "Workers"

    Public Sub ShowInformationWindow(ByVal url As String)
        Try
            Dim script As String = "openInformationWindow('" & url & "', 'InformationWindow');"
            script = Replace(script, "'", """")

            ' use the script manager to attach a new local script
            ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType(), "ShowInformationWindowScript", script, True)

        Catch ex As Exception
            Exit Sub
        End Try
    End Sub

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then

        End If
    End Sub

    Protected Sub rtvNav_NodeClick(sender As Object, e As Telerik.Web.UI.RadTreeNodeEventArgs) Handles rtvNav.NodeClick
        If e.Node.Value <> "" Then
            Me.ShowInformationWindow("Workers/" & e.Node.Value & ".aspx")
        End If
    End Sub
End Class