Imports DailyDocket.CommonCore.Shared.Common
Imports System.Configuration.ConfigurationManager

Public Class Availability
    Inherits builderPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Me.LoadCounties()
        End If
    End Sub

    Private Sub LoadCounties()
        Dim cn As New SqlClient.SqlConnection(ConnectionString)

        Try
            Me.tblCounties.Rows.Clear()
            Dim hr As New TableRow
            Dim hc1 As New TableCell
            hc1.Font.Bold = True
            hc1.HorizontalAlign = HorizontalAlign.Left
            hc1.Text = "&nbsp;"
            hr.Cells.Add(hc1)
            Dim hc2 As New TableCell
            hc2.Font.Bold = True
            hc2.HorizontalAlign = HorizontalAlign.Center
            hc2.Text = "Civil"
            hr.Cells.Add(hc2)
            Dim hc3 As New TableCell
            hc3.Font.Bold = True
            hc3.HorizontalAlign = HorizontalAlign.Center
            hc3.Text = "Criminal"
            hr.Cells.Add(hc3)
            Me.tblCounties.Rows.Add(hr)

            Dim x As Integer = 0
            Dim cmd As New SqlClient.SqlCommand("SELECT [CivilCount], [CriminalCount], [County], [State] FROM [vwCountySales] ORDER BY [County];", cn)
            If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
            Dim rs As SqlClient.SqlDataReader = cmd.ExecuteReader
            Do While rs.Read
                If rs("County").ToString.ToUpper <> "GUILFORD-GREENSBORO" And rs("County").ToString.ToUpper <> "GUILFORD-HIGHPOINT" Then
                    Dim tr As New TableRow
                    Dim tc1 As New TableCell
                    tc1.HorizontalAlign = HorizontalAlign.Left
                    tc1.Text = rs("County").ToString.Replace("_", " ") & ", " & rs("State").ToString.ToUpper
                    tr.Cells.Add(tc1)
                    Dim tc2 As New TableCell
                    tc2.HorizontalAlign = HorizontalAlign.Center
                    Dim imgCivil As New Image
                    imgCivil.ID = "imgCivil" & x
                    imgCivil.ImageUrl = If(rs("CivilCount").ToString.ToInteger > 0 Or rs("County").ToString.ToUpper = "GUILFORD", "~/images/toolbar/icon_accept.png", "~/images/toolbar/icon_reject.png")
                    tc2.Controls.Add(imgCivil)
                    tr.Cells.Add(tc2)
                    Dim tc3 As New TableCell
                    tc3.HorizontalAlign = HorizontalAlign.Center
                    Dim imgCriminal As New Image
                    imgCriminal.ID = "imgCriminal" & x
                    imgCriminal.ImageUrl = If(rs("CriminalCount").ToString.ToInteger > 0, "~/images/toolbar/icon_accept.png", "~/images/toolbar/icon_reject.png")
                    tc3.Controls.Add(imgCriminal)
                    tr.Cells.Add(tc3)
                    Me.tblCounties.Rows.Add(tr)

                    x += 1
                End If
            Loop
            cmd.Cancel()
            rs.Close()

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(If(Me.Page.OnMobile, Enums.ProjectName.MobileApp, Enums.ProjectName.Builder)))
        Finally
            cn.Close()
        End Try
    End Sub
End Class