Imports System.IO
Imports System.Net
Imports DailyDocket.CommonCore.Shared.Common
Imports Telerik.Web.UI

Public Class Common
    Public Shared Function LoadCases(ByVal pg As Page, ByVal barNumber As String, ByVal showAll As Boolean) As List(Of MyAppointment)
        Dim ar As New AttorneyRecord(barNumber)

        Return LoadCases(pg, ar.ID, showAll)
    End Function
    Public Shared Function LoadCases(ByVal pg As Page, ByVal attorneyID As Integer, ByVal showAll As Boolean) As List(Of MyAppointment)
        Dim retVal As New List(Of MyAppointment)

        Dim cn As New SqlClient.SqlConnection(ConnectionString)

        Try
            App.CurrentAttorney = New AttorneyRecord(attorneyID)

            Dim startDiff As New TimeSpan
            Dim endDiff As New TimeSpan

            Dim lastDate As New Date

            ' build a list of cases
            Dim lstCases As New List(Of FileRecord)
            Dim cmd As New SqlClient.SqlCommand("SELECT * FROM [vwMyCases] WHERE [AttorneyID] = " & attorneyID & " ORDER BY [State], [SessionDate], [County], [RecordNumber];", cn)
            If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
            Dim rs As SqlClient.SqlDataReader = cmd.ExecuteReader
            Do While rs.Read
                Dim lst As New List(Of NameValuePair)
                For x As Integer = 0 To rs.FieldCount - 1
                    lst.Add(New NameValuePair(rs.GetName(x), If(IsDBNull(rs.GetValue(x)), "", rs.GetValue(x).ToString)))
                Next

                Dim fr As New FileRecord(lst)
                fr.ID = rs("caseid").ToString.ToInteger
                If Not lstCases.Contains(fr) Then
                    Dim addCase As Boolean = False
                    For Each n As String In App.CurrentAttorney.AllNames
                        If fr.DefendantAttorney.Contains(n) Or fr.PlaintiffAttorney.Contains(n) Then addCase = True : Exit For
                    Next

                    If Not addCase Then
                        For Each an As String In App.CurrentAttorney.AllNames
                            For Each n As String In fr.DefendantAttorney
                                If an.ToLower.Contains(n.ToLower) Then addCase = True : Exit For
                            Next

                            If addCase Then Exit For
                        Next
                    End If

                    If addCase Then lstCases.Add(fr)
                End If
            Loop
            cmd.Cancel()
            rs.Close()

            For Each fr As FileRecord In lstCases
                If lastDate <> fr.SessionDate.Date Then
                    If Not App.DateList.Contains(fr.SessionDate.Date) Then App.DateList.Add(fr.SessionDate.Date)

                    startDiff = New TimeSpan
                    endDiff = New TimeSpan
                    lastDate = fr.SessionDate.Date
                End If

                Dim subject As String = ""
                Dim lst As New List(Of String)
                Dim attorneyFlag As String = ""
                For Each n As String In App.CurrentAttorney.AllNames
                    If fr.DefendantAttorney.Contains(n) Then
                        attorneyFlag = " (D)"
                        lst = fr.Defendant
                    ElseIf fr.PlaintiffAttorney.Contains(n) Then
                        attorneyFlag = " (P)"
                        lst = fr.Plaintiff
                    End If
                Next

                For Each d As String In lst
                    If d IsNot Nothing AndAlso d <> "" Then
                        If subject = "" Then subject = FormatName(d) Else subject &= ", " & FormatName(d)
                    End If
                Next
                Dim courtRoomNumber As String = fr.CourtRoomNumber
                If IsNumeric(fr.CourtRoomNumber) Then courtRoomNumber = Format(fr.CourtRoomNumber.ToInteger, "0000")
                subject = fr.AbbreviateCounty & "-" & courtRoomNumber & " (Seq. #" & Format(fr.RecordNumber, "0000") & ") " & fr.CaseNumber.FixCaseNumber & " - " & subject & attorneyFlag

                Dim appt As New MyAppointment
                appt.ID = fr.ID
                appt.XmlData = fr.SerializeToXml
                appt.Subject = subject
                appt.Start = fr.SessionDate 'fr.SessionDate + startDiff
                appt.End = fr.SessionDate 'fr.EndDate + endDiff
                appt.GroupDate = fr.SessionDate.Date
                appt.County = fr.County
                appt.Type = fr.RecordType
                If pg.OnMobile() And Not showAll Then
                    If fr.SessionDate.Date >= Now.Date Then
                        retVal.Add(appt)

                        'startDiff = startDiff.Add(New TimeSpan(0, 30, 0))
                        'endDiff = endDiff.Add(New TimeSpan(0, 30, 0))
                    End If
                Else
                    retVal.Add(appt)

                    'startDiff = startDiff.Add(New TimeSpan(0, 30, 0))
                    'endDiff = endDiff.Add(New TimeSpan(0, 30, 0))
                End If
            Next

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(If(pg.OnMobile, Enums.ProjectName.MobileApp, Enums.ProjectName.Builder)))
        Finally
            cn.Close()
        End Try

        Return retVal
    End Function
End Class
