Public Class Wizard_V2
    Inherits builderMasterPage

#Region "Properties"

    Public Property TitleText As String
        Get
            Return Me.lblHeader.Text
        End Get
        Set(value As String)
            Me.lblHeader.Text = value
        End Set
    End Property

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

End Class