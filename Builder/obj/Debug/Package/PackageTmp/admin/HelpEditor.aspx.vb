Imports DailyDocket.CommonCore.Shared.Common

Public Class HelpEditor
    Inherits builderPage

#Region "Properties"

    Private ReadOnly Property EditId As Integer
        Get
            Return Request.QueryString("id").ToInteger
        End Get
    End Property
    Private ReadOnly Property EditUrl As String
        Get
            Return Request.QueryString("page")
        End Get
    End Property

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Me.LoadTopic()
        End If

        Me.Master.BackButton.Visible = (Me.OnMobile)
        AddHandler Me.Master.BackButton.Click, AddressOf imgBack_Click
    End Sub

    Private Sub LoadTopic()
        Dim cn As New SqlClient.SqlConnection(ConnectionString)

        Try
            Dim sql As String = ""

            If Me.EditId > 0 Then ' faq
                sql = "SELECT [Content], [Source] FROM [Sys_Help] WHERE [ID] = " & Me.EditId.ToString & ";"
                Me.txtSource.Enabled = True
            ElseIf Me.EditUrl <> "" Then ' help topic
                sql = "SELECT [Content], [Source] FROM [Sys_Help] WHERE [Source] LIKE '" & Me.EditUrl & "';"
                Me.txtSource.Enabled = False
            End If

            If sql <> "" Then
                Dim cmd As New SqlClient.SqlCommand(sql, cn)
                If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
                Dim rs As SqlClient.SqlDataReader = cmd.ExecuteReader
                If rs.Read Then
                    Me.txtContent.Content = rs("Content").ToString
                    Me.txtSource.Text = rs("Source").ToString
                End If
                cmd.Cancel()
                rs.Close()
            End If

            If Me.txtSource.Text = "" Then Me.txtSource.Text = Me.EditUrl

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(App.CurrentUser.ID, App.CurrentClient.ID, Enums.ProjectName.Builder))
        Finally
            cn.Close()
        End Try
    End Sub

    Protected Sub imgBack_Click(sender As Object, e As ImageClickEventArgs)
        Dim usrString As String = If(Me.DeviceId <> "", "deviceid=" & Me.DeviceId, "uid=" & Me.UserId)
        Response.Redirect("~/admin/HelpTopics.aspx?" & usrString, False)
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Dim cn As New SqlClient.SqlConnection(ConnectionString)

        Try
            Dim eId As Integer = 0
            Dim type As String = If(Me.txtSource.Enabled, "faq", "help")
            Dim cmd As New SqlClient.SqlCommand("SELECT [ID] FROM [Sys_Help] WHERE [ID] = " & Me.EditId & " OR [Source] LIKE '" & Me.EditUrl & "';", cn)
            If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
            Dim rs As SqlClient.SqlDataReader = cmd.ExecuteReader
            If rs.Read Then
                eId = rs("ID").ToString.ToInteger
            End If
            cmd.Cancel()
            rs.Close()

            If eId = 0 Then ' new record
                cmd = New SqlClient.SqlCommand("INSERT INTO [Sys_Help] ([Source], [Content], [Type]) VALUES (@Source, @Content, '" & type & "');", cn)
            Else ' update
                cmd = New SqlClient.SqlCommand("UPDATE [Sys_Help] SET [Source] = @Source, [Content] = @Content WHERE [ID] = " & eId & ";", cn)
            End If
            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@Source", Me.txtSource.Text)
            cmd.Parameters.AddWithValue("@Content", Me.txtContent.Content)
            If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
            cmd.ExecuteNonQuery()

            Dim usrString As String = If(Me.DeviceId <> "", "deviceid=" & Me.DeviceId, "uid=" & Me.UserId)
            Response.Redirect("~/admin/HelpTopics.aspx?type=" & type & "&" & usrString, False)

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(App.CurrentUser.ID, App.CurrentClient.ID, Enums.ProjectName.Builder))
        Finally
            cn.Close()
        End Try
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Dim usrString As String = If(Me.DeviceId <> "", "deviceid=" & Me.DeviceId, "uid=" & Me.UserId)
        Dim type As String = If(Me.txtSource.Enabled, "faq", "help")
        Response.Redirect("~/admin/HelpTopics.aspx?type=" & type & "&" & usrString, False)
    End Sub
End Class