Imports DailyDocket.CommonCore.Shared.Common
Imports Telerik.Web.UI

Public Class FAQ
    Inherits builderPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Me.LoadFaq()
        End If

        Me.Master.BackButton.Visible = (Me.OnMobile)
        AddHandler Me.Master.BackButton.Click, AddressOf imgBack_Click
    End Sub

    Protected Sub imgBack_Click(sender As Object, e As ImageClickEventArgs)
        Dim usrString As String = If(Me.DeviceId <> "", "deviceid=" & Me.DeviceId, "uid=" & Me.UserId)
        Response.Redirect("~/Default.aspx?" & usrString, False)
    End Sub

    Private Sub LoadFaq()
        Dim cn As New SqlClient.SqlConnection(ConnectionString)

        Try
            Dim cmd As New SqlClient.SqlCommand("SELECT [ID], [Source], [Content] FROM [Sys_Help] WHERE [Type] LIKE 'faq' ORDER BY [Source];", cn)
            If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
            Dim rs As SqlClient.SqlDataReader = cmd.ExecuteReader
            Do While rs.Read
                Dim pbr As New RadPanelItem
                pbr.Expanded = False
                pbr.Text = "Q: " & rs("Source").ToString
                pbr.BorderStyle = BorderStyle.Solid
                pbr.BorderWidth = New Unit(1, UnitType.Pixel)
                pbr.BorderColor = GetColor("#C0C0C0")
                pbr.ContentTemplate = New ContentTemplate("A: ", rs("Content").ToString)
                Me.RadPanelBar1.Items.Add(pbr)
            Loop
            cmd.Cancel()
            rs.Close()

            Me.RadPanelBar1.CollapseAllItems()

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(App.CurrentUser.ID, App.CurrentClient.ID, Enums.ProjectName.Builder))
        Finally
            cn.Close()
        End Try
    End Sub
End Class