Imports Telerik.Web.UI

Public Class Logs
    Inherits builderPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Me.LoadLists()
            Me.RadGrid1.DataBind()
        End If

        Me.Master.BackButton.Visible = (Me.OnMobile)
        AddHandler Me.Master.BackButton.Click, AddressOf imgBack_Click
    End Sub

    Private Sub ddlProject_SelectedIndexChanged(sender As Object, e As DropDownListEventArgs) Handles ddlProject.SelectedIndexChanged
        Me.RadGrid1.DataBind()
    End Sub

    Private Sub LoadLists()
        Dim v As String = ""
        Me.ddlProject.Items.Clear()
        Dim enumValues As Array = System.[Enum].GetValues(GetType(Enums.ProjectName))
        For Each resource As Enums.ProjectName In enumValues
            Me.ddlProject.Items.Add(New DropDownListItem(resource.ToString, resource.ToString))
            If resource.ToString.ToLower = "builder" Then v = resource.ToString
        Next
        Me.ddlProject.SelectedValue = v
    End Sub

    Protected Sub imgBack_Click(sender As Object, e As ImageClickEventArgs)
        Dim usrString As String = If(Me.DeviceId <> "", "deviceid=" & Me.DeviceId, "uid=" & Me.UserId)
        Response.Redirect("~/Default.aspx?" & usrString, False)
    End Sub
End Class