Imports DailyDocket.CommonCore.Shared.Common
Imports Telerik.Web.UI

Public Class ProcessorLog
    Inherits System.Web.UI.Page

#Region "Properties"

    Public ReadOnly Property ListType As String
        Get
            Dim retVal As String = Request.QueryString("type")
            If retVal = "" Then retVal = "file"

            Return retVal
        End Get
    End Property

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.LoadHistory()
    End Sub

    Private Sub LoadHistory()
        Dim cn As New SqlClient.SqlConnection(ConnectionString)

        Try
            Dim sqlTable As String = "Sys_ProcessorLog"
            If Me.ListType.ToLower = "billing" Then sqlTable = "Sys_BillingLog"

            Dim lastRun As String = ""
            Dim contentMessage As String = ""
            Dim pbr As New RadPanelItem
            pbr.BorderStyle = BorderStyle.Solid
            pbr.BorderWidth = New Unit(1, UnitType.Pixel)
            pbr.BorderColor = GetColor("#C0C0C0")

            Dim cmd As New SqlClient.SqlCommand("SELECT [Text] FROM [" & sqlTable & "] WHERE DATEADD(dd, DATEDIFF(dd,0,[dtInserted]), 0) = DATEADD(dd, DATEDIFF(dd,0,GETDATE()), 0) ORDER BY [ID] DESC", cn)
            If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
            Dim rs As SqlClient.SqlDataReader = cmd.ExecuteReader
            Do While rs.Read
                pbr.Expanded = False

                If rs("Text").ToString.ToLower.Contains("run:") And lastRun <> rs("Text").ToString Then
                    pbr.Text = rs("Text").ToString
                    lastRun = rs("Text").ToString
                    pbr.ContentTemplate = New ContentTemplate("", contentMessage)
                    Me.RadPanelBar1.Items.Add(pbr)
                    pbr = New RadPanelItem
                    contentMessage = ""
                    pbr.BorderStyle = BorderStyle.Solid
                    pbr.BorderWidth = New Unit(1, UnitType.Pixel)
                    pbr.BorderColor = GetColor("#C0C0C0")
                Else
                    If contentMessage = "" Then contentMessage = rs("Text").ToString Else contentMessage &= "<br/>" & rs("Text").ToString
                End If
            Loop
            cmd.Cancel()
            rs.Close()

            If Me.RadPanelBar1.Items.Count = 0 Then
                pbr = New RadPanelItem
                pbr.Expanded = True
                pbr.Text = "No items yet to show in this view."
                pbr.BorderStyle = BorderStyle.Solid
                pbr.BorderWidth = New Unit(1, UnitType.Pixel)
                pbr.BorderColor = GetColor("#C0C0C0")
                Me.RadPanelBar1.Items.Add(pbr)
            End If

            Me.RadPanelBar1.CollapseAllItems()

            Me.RadPanelBar1.Items(0).Expanded = True

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(App.CurrentUser.ID, App.CurrentClient.ID, Enums.ProjectName.Builder))
        Finally
            cn.Close()
        End Try
    End Sub

    'Private Sub LoadHistory()
    '    Dim cn As New SqlClient.SqlConnection(ConnectionString)

    '    Try
    '        Me.tblHistory.Rows.Clear()

    '        Dim rowCount As Integer = 0
    '        Dim cmd As New SqlClient.SqlCommand("SELECT [Text] FROM [Sys_ProcessorLog] WHERE DATEADD(dd, DATEDIFF(dd,0,[dtInserted]), 0) = DATEADD(dd, DATEDIFF(dd,0,GETDATE()), 0) ORDER BY [ID] DESC", cn)
    '        If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
    '        Dim rs As SqlClient.SqlDataReader = cmd.ExecuteReader
    '        Do While rs.Read
    '            Dim tr As New TableRow
    '            Dim tc1 As New TableCell
    '            tc1.VerticalAlign = VerticalAlign.Top
    '            tc1.Wrap = False
    '            tc1.Text = rs("Text").ToString
    '            tr.Cells.Add(tc1)
    '            Me.tblHistory.Rows.Add(tr)

    '            If rowCount = 0 Then
    '                If rs("Text").ToString.ToLower.Contains("finished") Then Me.lblStatus.Text = "Waiting." Else Me.lblStatus.Text = "Running."
    '            End If
    '            rowCount += 1
    '        Loop

    '        If rowCount = 0 Then
    '            Dim tr As New TableRow
    '            Dim tc1 As New TableCell
    '            tc1.ColumnSpan = 3
    '            tc1.Text = "No records yet to show for today."
    '            tr.Cells.Add(tc1)
    '            Me.tblHistory.Rows.Add(tr)
    '        End If

    '    Catch ex As Exception
    '        ex.WriteToErrorLog(New ErrorLogEntry(If(Me.Page.OnMobile, Enums.ProjectName.MobileApp, Enums.ProjectName.Builder)))
    '    Finally
    '        cn.Close()
    '    End Try
    'End Sub
End Class