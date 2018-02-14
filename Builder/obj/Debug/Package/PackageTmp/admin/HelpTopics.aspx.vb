Imports DailyDocket.CommonCore.Shared.Common

Public Class HelpTopics
    Inherits builderPage

#Region "Properties"

    Private ReadOnly Property Type As String
        Get
            If Request.QueryString("type") = "" Then
                Return "help"
            Else Return Request.QueryString("type").ToLower
            End If
        End Get
    End Property

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Me.ddlType.SelectedValue = Me.Type
        End If

        Me.LoadTopics()

        Me.Master.BackButton.Visible = (Me.OnMobile)
        AddHandler Me.Master.BackButton.Click, AddressOf imgBack_Click
    End Sub

    Private Sub LoadTopics()
        Dim cn As New SqlClient.SqlConnection(ConnectionString)

        Try
            Me.tblTopics.Rows.Clear()

            Select Case Me.ddlType.SelectedValue.ToLower
                Case "help"
                    Me.btnAddTopic.Visible = False

                    Dim lst As List(Of IO.FileInfo) = SearchDir(Server.MapPath("~/"), "*.aspx", Enums.FileSort.Name)
                    For Each itm As IO.FileInfo In lst
                        If Not itm.FullName.ToLower.Contains("debug") Then
                            Dim txt As String = itm.FullName
                            txt = txt.ToLower.Replace("c:\users\james\google drive\projects\solvtopia\dailydocket\builder", "")
                            txt = txt.ToLower.Replace("c:\inetpub\access.dailycourtdocket.com\wwwroot", "")
                            txt = txt.Replace("\", "/")

                            Dim tr As New TableRow
                            Dim tc2 As New TableCell
                            Dim lnkEdit As New LinkButton
                            lnkEdit.Text = "Edit"
                            lnkEdit.CommandArgument = txt
                            AddHandler lnkEdit.Click, AddressOf lnkEdit_Click
                            tc2.Controls.Add(lnkEdit)
                            tr.Cells.Add(tc2)
                            Dim tc3 As New TableCell
                            Dim lnkDelete As New LinkButton
                            lnkDelete.Text = "Delete"
                            lnkDelete.CommandArgument = txt
                            AddHandler lnkDelete.Click, AddressOf lnkDelete_Click
                            tc3.Controls.Add(lnkDelete)
                            tr.Cells.Add(tc3)
                            Dim tc1 As New TableCell
                            tc1.Text = txt
                            tr.Cells.Add(tc1)
                            Me.tblTopics.Rows.Add(tr)
                        End If
                    Next

                Case "faq"
                    Me.btnAddTopic.Visible = True

                    Dim cmd As New SqlClient.SqlCommand("SELECT [ID], [Source] FROM [Sys_Help] WHERE [Type] LIKE 'faq';", cn)
                    If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
                    Dim rs As SqlClient.SqlDataReader = cmd.ExecuteReader
                    Do While rs.Read
                        Dim tr As New TableRow
                        Dim tc2 As New TableCell
                        Dim lnkEdit As New LinkButton
                        lnkEdit.Text = "Edit"
                        lnkEdit.CommandArgument = rs("ID").ToString
                        AddHandler lnkEdit.Click, AddressOf lnkEdit_Click
                        tc2.Controls.Add(lnkEdit)
                        tr.Cells.Add(tc2)
                        Dim tc3 As New TableCell
                        Dim lnkDelete As New LinkButton
                        lnkDelete.Text = "Delete"
                        lnkDelete.CommandArgument = rs("ID").ToString
                        AddHandler lnkDelete.Click, AddressOf lnkDelete_Click
                        tc3.Controls.Add(lnkDelete)
                        tr.Cells.Add(tc3)
                        Dim tc1 As New TableCell
                        tc1.Text = rs("Source").ToString
                        tr.Cells.Add(tc1)
                        Me.tblTopics.Rows.Add(tr)
                    Loop
                    cmd.Cancel()
                    rs.Close()
            End Select

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(App.CurrentUser.ID, App.CurrentClient.ID, Enums.ProjectName.Builder))
        Finally
            cn.Close()
        End Try
    End Sub

    Protected Sub btnAddTopic_Click(sender As Object, e As EventArgs) Handles btnAddTopic.Click
        ' add for faq only ...
        Dim usrString As String = If(Me.DeviceId <> "", "deviceid=" & Me.DeviceId, "uid=" & Me.UserId)
        Response.Redirect("~/admin/HelpEditor.aspx?id=0&" & usrString)
    End Sub

    Protected Sub lnkEdit_Click(sender As Object, e As EventArgs)
        Dim usrString As String = If(Me.DeviceId <> "", "deviceid=" & Me.DeviceId, "uid=" & Me.UserId)

        Dim lnk As LinkButton = CType(sender, LinkButton)
        If IsNumeric(lnk.CommandArgument) Then
            ' faq
            Response.Redirect("~/admin/HelpEditor.aspx?id=" & lnk.CommandArgument & "&" & usrString)
        Else
            ' help topic
            Response.Redirect("~/admin/HelpEditor.aspx?page=" & lnk.CommandArgument & "&" & usrString)
        End If
    End Sub

    Protected Sub lnkDelete_Click(sender As Object, e As EventArgs)
        Dim usrString As String = If(Me.DeviceId <> "", "deviceid=" & Me.DeviceId, "uid=" & Me.UserId)

        Dim lnk As LinkButton = CType(sender, LinkButton)
        If IsNumeric(lnk.CommandArgument) Then
            ' faq
        Else
            ' help topic
        End If
    End Sub

    Protected Sub imgBack_Click(sender As Object, e As ImageClickEventArgs)
        Dim usrString As String = If(Me.DeviceId <> "", "deviceid=" & Me.DeviceId, "uid=" & Me.UserId)
        Response.Redirect("~/Default.aspx?" & usrString, False)
    End Sub
End Class