Imports DailyDocket.CommonCore.Shared.Common
Imports Telerik.Web.UI

Public Class RecordDetails
    Inherits System.Web.UI.UserControl

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
    Public Property UrlString As String
        Get
            Return Me.lblUrlString.Text
        End Get
        Set(value As String)
            Me.lblUrlString.Text = value
        End Set
    End Property

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then

        End If

        Me.ShowOptions()
    End Sub

    Public Sub LoadCase()
        App.CurrentAttorney = New AttorneyRecord(App.CurrentUser.AttorneyID)

        If App.SelectedCase.ID > 0 Then
            Me.lblCaseNumber.Text = App.SelectedCase.CaseNumber.FixCaseNumber
            Me.lblRecordNumber.Text = Format(App.SelectedCase.RecordNumber, "0000")
            Me.lblSessionDate.Text = App.SelectedCase.SessionDate.Date.ToLongDateString
            Me.lblSessionTime.Text = App.SelectedCase.SessionDate.ToLongTimeString
            Dim courtRoomNumber As String = App.SelectedCase.CourtRoomNumber
            If IsNumeric(App.SelectedCase.CourtRoomNumber) Then courtRoomNumber = Format(App.SelectedCase.CourtRoomNumber.ToInteger, "0000")
            Me.lblCourtRoom.Text = courtRoomNumber
            Me.lblLocation.Text = App.SelectedCase.CourtRoomLocation
            Me.lblJudge.Text = App.SelectedCase.PresidingJudge
            Me.lblCounty.Text = App.SelectedCase.County
            Me.lblState.Text = App.SelectedCase.State
            Me.lblRecordType.Text = App.SelectedCase.RecordType.ToUpper

            If App.SelectedCase.RecordType.ToLower = "civil" Then
                Me.lblType.Text = App.SelectedCase.CaseType
                Me.lblDaysSinceFiling.Text = FormatNumber(App.SelectedCase.DaysSinceFiling, 0)
                Me.lblLengthOfTrial.Text = App.SelectedCase.LengthOfTrial

                Me.lstCivilPlaintiff.Items.Clear()
                For Each s As String In App.SelectedCase.Plaintiff
                    Dim itm As New RadListBoxItem
                    itm.Text = FormatName(s)
                    Me.lstCivilPlaintiff.Items.Add(itm)
                Next

                Me.lstCivilPlaintiffAttorney.Items.Clear()
                For Each s As String In App.SelectedCase.PlaintiffAttorney
                    Dim itm As New RadListBoxItem
                    If App.CurrentAttorney.AllNames.Contains(s) And App.CurrentUser.ID = 40 Then
                        itm.Text = App.CurrentUser.Name.ToUpper
                    Else itm.Text = FormatName(s)
                    End If
                    Me.lstCivilPlaintiffAttorney.Items.Add(itm)
                Next

                Me.lstCivilDefendant.Items.Clear()
                For Each s As String In App.SelectedCase.Defendant
                    Dim itm As New RadListBoxItem
                    itm.Text = FormatName(s)
                    Me.lstCivilDefendant.Items.Add(itm)
                Next

                Me.lstCivilDefendantAttorney.Items.Clear()
                For Each s As String In App.SelectedCase.DefendantAttorney
                    Dim itm As New RadListBoxItem
                    If App.CurrentAttorney.AllNames.Contains(s) And App.CurrentUser.ID = 40 Then
                        itm.Text = App.CurrentUser.Name.ToUpper
                    Else itm.Text = FormatName(s)
                    End If
                    Me.lstCivilDefendantAttorney.Items.Add(itm)
                Next

                Me.lstIssuesOrEvents.Items.Clear()
                For Each s As String In App.SelectedCase.IssuesOrEvents
                    Dim itm As New RadListBoxItem
                    itm.Text = FormatName(s)
                    Me.lstIssuesOrEvents.Items.Add(itm)
                Next

                Me.lstCivilNotes.Items.Clear()
                For Each s As String In App.SelectedCase.Notes
                    Dim itm As New RadListBoxItem
                    itm.Text = FormatName(s)
                    Me.lstCivilNotes.Items.Add(itm)
                Next

            ElseIf App.SelectedCase.RecordType.ToLower = "criminal" Then

                Me.lblClerk.Text = App.SelectedCase.CourtRoomClerk
                Me.lblTB.Text = App.SelectedCase.TrueBillOfIndictment
                Me.lblBond.Text = App.SelectedCase.Bond
                Me.lblContinuances.Text = App.SelectedCase.Continuances

                Dim x As Integer = 0
                Me.lstProsecutor.Items.Clear()
                For Each s As String In App.SelectedCase.Prosecutor
                    Dim itm As New RadListBoxItem
                    itm.Text = FormatName(s)
                    Me.lstProsecutor.Items.Add(itm)
                Next

                Me.lstAssistantDA.Items.Clear()
                For Each s As String In App.SelectedCase.AssistantDA
                    Dim itm As New RadListBoxItem
                    itm.Text = FormatName(s)
                    Me.lstAssistantDA.Items.Add(itm)
                Next

                Me.lstCriminalDefendant.Items.Clear()
                For Each s As String In App.SelectedCase.Defendant
                    Dim itm As New RadListBoxItem
                    itm.Text = FormatName(s)
                    Me.lstCriminalDefendant.Items.Add(itm)
                Next

                Me.lstCriminalDefendantAttorney.Items.Clear()
                For Each s As String In App.SelectedCase.DefendantAttorney
                    Dim itm As New RadListBoxItem
                    itm.Text = FormatName(s)
                    Me.lstCriminalDefendantAttorney.Items.Add(itm)
                Next

                Me.lstCriminalNotes.Items.Clear()
                For Each s As String In App.SelectedCase.Notes
                    Dim itm As New RadListBoxItem
                    itm.Text = FormatName(s)
                    Me.lstCriminalNotes.Items.Add(itm)
                Next

                Dim chgCount As Integer = 1
                Me.tblCharges.Rows.Clear()
                For Each chg As CriminalCharge In App.SelectedCase.Charges
                    Dim tr1 As New TableRow
                    Dim tc1a As New TableCell
                    tc1a.Text = chgCount.ToString
                    tr1.Cells.Add(tc1a)
                    Dim tc1b As New TableCell
                    tc1b.Text = chg.OffenseType & " - " & chg.OffenseText
                    tr1.Cells.Add(tc1b)
                    Me.tblCharges.Rows.Add(tr1)
                    If chg.Complainant.Count > 0 Then
                        Dim tr2 As New TableRow
                        Dim tc2a As New TableCell
                        tc2a.Text = "&nbsp;"
                        tr2.Cells.Add(tc2a)
                        Dim tc2b As New TableCell
                        tc2b.Text = "Complainant(s)"
                        tr2.Cells.Add(tc2b)
                        Me.tblCharges.Rows.Add(tr2)
                        Dim y As Integer = 0
                        For Each s As String In chg.Complainant
                            Dim tr3 As New TableRow
                            Dim tc3a As New TableCell
                            tc3a.Text = "&nbsp;"
                            tr3.Cells.Add(tc3a)
                            Dim tc3b As New TableCell
                            tc3b.Text = FormatName(s) & " (" & chg.ComplainantType(y) & ")"
                            tr3.Cells.Add(tc3b)
                            Me.tblCharges.Rows.Add(tr3)
                            y += 1
                        Next
                    End If
                    If chg.ADA <> "" Then
                        Dim tr4 As New TableRow
                        Dim tc4a As New TableCell
                        tc4a.Text = "&nbsp;"
                        tr4.Cells.Add(tc4a)
                        Dim tc4b As New TableCell
                        tc4b.Text = "ADA: " & chg.ADA
                        tr4.Cells.Add(tc4b)
                        Me.tblCharges.Rows.Add(tr4)
                    End If

                    chgCount += 1
                Next
            End If
        End If

        Me.tblRawView.Rows.Clear()
        Me.tblRawView.Style.Add("margin-left", "10px")
        Me.tblRawView.Style.Add("margin-right", "10px")
        Me.tblRawView.Style.Add("margin-top", "10px")
        Me.tblRawView.Style.Add("margin-bottom", "100px")

        ' pull the header from the file and display it
        Dim fName As String = App.SelectedCase.FileName
        Dim lastLineOfHeader As Boolean = False
        Try
            Dim headerPath As String = ""
            If Request.Url.ToString.ToLower.Contains("localhost") Then
                headerPath = App.SelectedCase.FileName.ToLower & ".processed"
            Else
                If fName.ToLower.Contains("c:\files_tmp\") Then
                    fName = fName.ToLower.Replace("c:\files_tmp\", "~/files_tmp/")
                Else fName = fName.ToLower.Replace("c:\inetpub\access.dailycourtdocket.com\wwwroot\files_tmp\", "~/files_tmp/")
                End If
                fName = fName.Replace("\", "/")

                headerPath = Server.MapPath(fName & ".processed")
            End If

            Using sr As New IO.StreamReader(headerPath)
                Do While sr.Peek() >= 0
                    Dim s As String = sr.ReadLine

                    If App.SelectedCase.RecordType.ToLower = "civil" Then
                        If s.Trim <> "" And s.Length > 3 Then
                            If s.Trim.StartsWith("================") Then
                                lastLineOfHeader = True
                            End If
                        End If
                    ElseIf App.SelectedCase.RecordType.ToLower = "criminal" Then
                        If s.Trim <> "" And s.Length > 3 Then
                            If s.Trim.StartsWith("****************") Then
                                lastLineOfHeader = True
                            End If
                        End If
                    End If

                    s = s.Replace(" ", "&nbsp;")
                    Dim tr As New TableRow
                    Dim tc As New TableCell
                    tc.Wrap = False
                    tc.Text = s
                    tr.Cells.Add(tc)
                    Me.tblRawView.Rows.Add(tr)

                    If lastLineOfHeader Then Exit Do
                Loop
            End Using
        Catch ex As Exception
        End Try

        ' add the file record to the table
        If App.SelectedCase.RecordData IsNot Nothing Then
            For Each s As String In App.SelectedCase.RecordData
                s = s.Replace(" ", "&nbsp;")

                Dim tr As New TableRow
                Dim tc As New TableCell
                tc.Wrap = False
                tc.Text = s
                tr.Cells.Add(tc)
                Me.tblRawView.Rows.Add(tr)
            Next

            ' add the file path
            Dim tr1 As New TableRow
            Dim tc1 As New TableCell
            tc1.Wrap = False
            tc1.Text = "<br/><br/>" & fName.ToLower.Replace("~/files_tmp/", "").ToUpper
            tr1.Cells.Add(tc1)
            Me.tblRawView.Rows.Add(tr1)
        End If

        Me.ShowOptions()
    End Sub

    Private Sub btnNotMine_Click(sender As Object, e As EventArgs) Handles btnNotMine.Click
        Dim cn As New SqlClient.SqlConnection(ConnectionString)

        Try
            Dim cmd As New SqlClient.SqlCommand("DELETE FROM [AttorneyCases] WHERE [CaseID] = " & App.SelectedCase.ID & " AND [AttorneyID] = " & App.CurrentUser.AttorneyID, cn)
            If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
            cmd.ExecuteNonQuery()

            App.MyCases = Nothing
            App.SelectedCase = Nothing
            App.DateList = Nothing

            ' redirect the parent back to the dashboard
            Me.Page.RunClientScript("window.parent.location = '../Default.aspx?" & Me.UrlString & "'")

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(App.CurrentUser.ID, App.CurrentClient.ID, If(Me.Page.OnMobile, Enums.ProjectName.MobileApp, Enums.ProjectName.Builder)))
        Finally
            cn.Close()
        End Try
    End Sub

    Private Sub lnkClient_Click(sender As Object, e As EventArgs)
        Dim lnk As LinkButton = CType(sender, LinkButton)

        Me.Page.RunClientScript("window.parent.location = 'ClientProfile.aspx?" & Me.UrlString & "&name=" & lnk.CommandName & "'")
    End Sub

    Private Sub ShowOptions()
        If App.SelectedCase.RecordType <> "" Then
            Me.pnlCivil.Visible = (App.SelectedCase.RecordType.ToLower = "civil")
            Me.pnlCivilParties.Visible = (App.SelectedCase.RecordType.ToLower = "civil")
            Me.pnlCriminal.Visible = (App.SelectedCase.RecordType.ToLower = "criminal")
            Me.pnlCriminalParties.Visible = (App.SelectedCase.RecordType.ToLower = "criminal")

            Me.pnlCivilIssues.Visible = (App.SelectedCase.IssuesOrEvents.Count > 0)
            Me.pnlCivilNotes.Visible = (App.SelectedCase.Notes.Count > 0)
            Me.pnlCriminalNotes.Visible = (App.SelectedCase.Notes.Count > 0)
        End If
    End Sub

End Class