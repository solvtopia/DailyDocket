Imports DailyDocket.CommonCore.Shared.Common
Imports Telerik.Web.UI

Public Class _Default
    Inherits builderPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Me.lblComingFrom.Text = "" Then Me.lblComingFrom.Text = Request.UrlReferrer.LocalPath.ToLower

        If Not IsPostBack Then
            Me.LoadArticle()
        End If

        Me.Master.BackButton.Visible = (Me.OnMobile)
        Me.btnBack.Visible = (Not Me.OnMobile)
        AddHandler Me.Master.BackButton.Click, AddressOf imgBack_Click
    End Sub

    Protected Sub imgBack_Click(sender As Object, e As ImageClickEventArgs)
        Dim usrString As String = If(Me.DeviceId <> "", "deviceid=" & Me.DeviceId, "uid=" & Me.UserId)
        Dim returnTo As String = "~" & Me.lblComingFrom.Text & "?" & usrString
        Response.Redirect(returnTo, False)
    End Sub

    Private Sub LoadArticle()
        Dim cn As New SqlClient.SqlConnection(connectionString)

        Try
            Dim cmd As New SqlClient.SqlCommand("SELECT [Content] FROM [Sys_Help] WHERE [Source] LIKE '" & Me.lblComingFrom.Text & "' AND [Type] LIKE 'help';", cn)
            If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
            Dim rs As SqlClient.SqlDataReader = cmd.ExecuteReader
            If rs.Read Then
                Me.litArticle.Text = rs("Content").ToString
            Else Me.litArticle.Text = "No Article Content For This Topic"
            End If
            cmd.Cancel()
            rs.Close()

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(App.CurrentUser.ID, App.CurrentClient.ID, Enums.ProjectName.Builder))
        Finally
            cn.Close()
        End Try
    End Sub

    Private Sub btnBack_Click(sender As Object, e As EventArgs) Handles btnBack.Click
        Dim usrString As String = If(Me.DeviceId <> "", "deviceid=" & Me.DeviceId, "uid=" & Me.UserId)
        Dim returnTo As String = "~" & Me.lblComingFrom.Text & "?" & usrString
        Response.Redirect(returnTo, False)
    End Sub
End Class