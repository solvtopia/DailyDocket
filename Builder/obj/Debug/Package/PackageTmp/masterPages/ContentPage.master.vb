Imports Telerik.Web.UI

Public Class ContentPage
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
    Public ReadOnly Property MenuBar As PlaceHolder
        Get
            Return Me.Master.MenuBar
        End Get
    End Property
    Public ReadOnly Property MenuAjaxPanel As RadAjaxPanel
        Get
            Return Me.Master.MenuAjaxPanel
        End Get
    End Property
    Public ReadOnly Property BackButton As ImageButton
        Get
            Return Me.imgBack
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
        If Me.Page.Title <> "" Then
            Me.TitleText = Me.Page.Title
        End If
    End Sub

End Class