Imports DailyDocket.API.Common

Public Class builderPage
    Inherits System.Web.UI.Page

#Region "Master Event Handlers"

    Public Sub Layout_NewModuleClicked(ByVal folderId As Integer)
    End Sub

    Public Sub Layout_ShowModule(ByVal modId As Integer)
    End Sub

    Public Sub Layout_FolderClicked(ByVal editId As Integer)
    End Sub

#End Region


    Private Sub builderPage_Load(sender As Object, e As EventArgs) Handles Me.Load

    End Sub
End Class

Public Class builderMasterPage
    Inherits System.Web.UI.MasterPage

    Private Sub builderMasterPage_Load(sender As Object, e As EventArgs) Handles Me.Load

    End Sub
End Class