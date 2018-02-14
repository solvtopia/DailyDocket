Public Class CaseDetails
    Inherits builderPage

#Region "Properties"

    Private Property SelectedCase As FileRecord
        Get
            If Session("SelectedCase") Is Nothing Then Session("SelectedCase") = New FileRecord
            Return CType(Session("SelectedCase"), FileRecord)
        End Get
        Set(value As FileRecord)
            Session("SelectedCase") = value
        End Set
    End Property

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then

        End If

        Me.RecordDetails.UrlString = If(Me.DeviceId <> "", "deviceid=" & Me.DeviceId, "uid=" & Me.UserId)

        If App.SelectedCase.ID > 0 Then Me.RecordDetails.LoadCase()

        Me.Master.BackButton.Visible = (Me.OnMobile)
        AddHandler Me.Master.BackButton.Click, AddressOf imgBack_Click
    End Sub

    Protected Sub imgBack_Click(sender As Object, e As ImageClickEventArgs)
        Dim usrString As String = If(Me.DeviceId <> "", "deviceid=" & Me.DeviceId, "uid=" & Me.UserId)
        Response.Redirect("~/Default.aspx?" & usrString, False)
    End Sub
End Class