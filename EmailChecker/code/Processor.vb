Imports System.IO
Imports System.Net
Imports System.Text
Imports System.Text.RegularExpressions
Public Class Processor

    Public Shared Function NCCivilFile(ByVal fPath As String) As List(Of FileRecord)
        Dim retVal As New List(Of FileRecord)

        Try
            Dim layoutVersion As Integer = 1

            Dim sCounty As String = fPath.Replace(files_tmp, "").Split("\"c)(1)
            Dim sType As String = fPath.Replace(files_tmp, "").Split("\"c)(2)

            Dim SessionDate As DateTime = New Date(2000, 1, 1)
            Dim SessionTime As New TimeSpan
            Dim CourtroomNumber As String = ""
            Dim CourtroomLocation As String = ""
            Dim PresidingJudge As String = ""

            ' open the file and read it into our list(of string) then close the file so we don't leave it open
            ' we remove the blank lines so they don't clutter the routine
            Dim ht As New Hashtable
            Dim lastRecordNumber As Integer = 0
            Dim recordMultiplier As Integer = 1
            Dim tmpRecord As New List(Of String)

            Dim rdr As New System.IO.StreamReader(fPath)
            Do While rdr.Peek() <> -1
                Dim s As String = rdr.ReadLine

                ' remove invalid characters we know about
                s = s.Replace("ChrW(22)", "")
                s = s.Replace(ChrW(22), "")

                If s.Trim.ToLower.StartsWith("session beginning") Then
                    If s.Trim.ToLower.Contains(" at ") Then
                        If IsDate(s.Trim.Substring(18).Substring(0, s.Trim.IndexOf("AT") - 18).Trim) Then
                            SessionDate = CDate(s.Trim.Substring(18).Substring(0, s.Trim.IndexOf("AT") - 18).Trim)
                        End If
                    Else
                        If IsDate(s.Trim.Substring(18).Trim) Then
                            SessionDate = CDate(s.Trim.Substring(18).Trim)
                        End If
                    End If
                ElseIf s.Trim.ToLower.Contains("beginning") And s.Trim.ToLower.Contains(" at ") Then
                    If s.Trim.ToLower.Contains("a.m.") Or s.Trim.ToLower.Contains("p.m.") Then
                        If SessionTime = New TimeSpan Then SessionTime = New TimeSpan(CInt(Right(s.Trim, 10).Split(":"c)(0)), CInt(Right(s.Trim, 10).Split(":"c)(1).Substring(0, 2)), 0)
                    Else
                        If SessionTime = New TimeSpan Then SessionTime = New TimeSpan(CInt(Right(s.Trim, 8).Split(":"c)(0)), CInt(Right(s.Trim, 8).Split(":"c)(1).Substring(0, 2)), 0)
                    End If
                ElseIf s.Trim.ToLower.StartsWith("courtroom number") Then
                    CourtroomNumber = s.Trim.Split(","c)(0).Substring(17).Trim
                    If s.Trim.Contains(",") Then CourtroomLocation = s.Trim.Split(","c)(1).Trim
                ElseIf s.Trim.ToLower.Contains("presiding") And s.Trim.ToLower.Contains("judge") Then
                    PresidingJudge = s.Trim.Split(","c)(0).Trim
                ElseIf s.Trim.ToLower.StartsWith("pldg type clk dt by party") And layoutVersion = 0 Then
                    layoutVersion = 2
                Else
                    If s.Trim <> "" And s.Length > 3 Then
                        If s.ToLower.StartsWith("#") Or (IsNumeric(s.ToLower.Substring(0, 3)) And s.ToLower.Substring(0, 4).EndsWith(" ")) Then
                            If lastRecordNumber > 0 Then
                                Dim tmpRecordNumber As Integer = lastRecordNumber + (100 * recordMultiplier)

                                If ht.ContainsKey("record_" & tmpRecordNumber) Then
                                    recordMultiplier += 1
                                    tmpRecordNumber = lastRecordNumber + (100 * recordMultiplier)
                                End If

                                ht.Add("record_" & tmpRecordNumber, tmpRecord)
                                tmpRecord = New List(Of String)
                            End If

                            ' files with a record number in the first 3 characters of the line also include the case number, time, and type
                            If IsNumeric(s.Trim.ToLower.Substring(0, 3)) Then
                                lastRecordNumber = CInt(s.Trim.ToLower.Substring(0, 3))
                                tmpRecord.Add(s)
                            Else
                                If s.Trim.Split(" "c).Count > 1 AndAlso IsNumeric(s.Trim.Split(" "c)(1)) Then
                                    lastRecordNumber = CInt(s.Trim.Split(" "c)(1))
                                Else
                                    s = s.Replace("#", "")
                                    s = s.ReplaceAll("_", "")
                                    lastRecordNumber = CInt(s.Trim.Split(" "c)(0))
                                End If
                            End If
                        ElseIf (Not s.ToLower.Contains(" page ") And Not s.StartsWith("____________________")) And lastRecordNumber > 0 And s.Trim.Length < 100 Then
                            tmpRecord.Add(s)
                        End If
                    End If
                End If

                My.Application.DoEvents()
            Loop
            rdr.Close()

            ' handle if the file only has 1 record
            'If ht.Count = 0 And tmpRecord.Count > 0 Then
            '    ht.Add("record_" & lastRecordNumber + (100 * recordMultiplier), tmpRecord)
            'End If

            ' handle the last record if it wasn't already added
            If Not ht.ContainsKey("record_" & lastRecordNumber + (100 * recordMultiplier)) Then
                ht.Add("record_" & lastRecordNumber + (100 * recordMultiplier), tmpRecord)
            End If

            ' now we can loop through all the "records" and gather the data into our object
            For Each itm In ht.Keys
                If CType(ht(itm), List(Of String)).Count > 0 Then
                    Dim lstR As List(Of String) = CType(ht(itm), List(Of String))

                    Dim plaintiffsFinished As Boolean = False
                    Dim defendantsFinished As Boolean = False
                    Dim issuesOrEventsStarted As Boolean = False
                    Dim issuesOrEventsFinished As Boolean = False
                    Dim notesStarted As Boolean = False

                    Dim r As New FileRecord()

                    ' create variables to hold the lists
                    Dim lstDefendant As New List(Of String)
                    Dim lstDefendantAttorney As New List(Of String)
                    Dim lstIssuesOrEvents As New List(Of String)
                    Dim lstNotes As New List(Of String)
                    Dim lstPlaintiff As New List(Of String)
                    Dim lstPlaintiffAttorney As New List(Of String)

                    r.RecordNumber = CInt(itm.ToString.Substring(7))
                    r.SessionDate = SessionDate
                    r.CourtRoomNumber = CourtroomNumber
                    r.CourtRoomLocation = CourtroomLocation
                    r.PresidingJudge = PresidingJudge
                    r.RecordData = lstR
                    r.County = sCounty
                    r.State = "NC"
                    r.FileName = fPath
                    r.RecordType = sType
                    r.LayoutVersion = layoutVersion
                    r.Source = DailyDocket.CommonCore.Shared.Enums.RecordSource.Email

                    For Each s As String In lstR
                        Dim colCount As Integer = 0

                        Dim col1 As String = ""
                        Dim col2 As String = ""
                        Dim col3 As String = ""
                        Dim col4 As String = ""

                        Select Case s.Length
                            Case >= 100
                                ' we have 4 columns
                                colCount = 4
                                col1 = s.Substring(0, 15).Trim
                                col2 = s.Substring(16, 41).Trim
                                col3 = s.Substring(42, 18).Trim
                                col4 = s.Substring(68).Trim

                                r.CaseNumber = col2
                                If SessionTime = New TimeSpan Then SessionTime = New TimeSpan(CInt(col3.Split(":"c)(0)), CInt(col3.Split(":"c)(1).Substring(0, 2)), 0)
                                r.CaseType = col4

                            Case > 56
                                ' we have 3 columns
                                colCount = 3
                                col1 = s.Substring(0, 14).Trim
                                col2 = s.Substring(15, 40).Trim
                                col3 = s.Substring(56).Trim

                            Case > 14
                                ' we have 2 columns
                                colCount = 2
                                If s.Trim.ToLower.StartsWith("issues") Or (issuesOrEventsStarted And Not issuesOrEventsFinished) Then
                                    ' the issues section has different column widths
                                    col1 = s.Substring(0, 10).Trim
                                    col2 = s.Substring(11).Trim
                                ElseIf s.Trim.ToLower.StartsWith("event") Or (issuesOrEventsStarted And Not issuesOrEventsFinished) Then
                                    ' the events section has different column widths
                                    col1 = s.Substring(0, 6).Trim
                                    col2 = s.Substring(7).Trim
                                ElseIf s.Trim.ToLower.StartsWith("type") Then
                                    r.CaseType = s.Trim.Split(":"c)(1).Trim
                                Else
                                    ' assume all others are the same
                                    col1 = s.Substring(0, 14).Trim
                                    col2 = s.Substring(15).Trim
                                End If

                            Case < 14
                                ' we have 1 column
                                colCount = 1
                                col1 = s.Trim
                        End Select

                        If r.CaseNumber = "" And col1.Contains("-") Then r.CaseNumber = col1
                        If col2.ToLower = "-vs-" Or col2.ToLower = "vs" Then plaintiffsFinished = True
                        If col1.ToLower.Contains("event") Or col1.ToLower.Contains("issues") Then defendantsFinished = True : issuesOrEventsStarted = True
                        If s.Trim.ToLower.Contains("days since filing") And defendantsFinished Then issuesOrEventsStarted = False : issuesOrEventsFinished = True
                        If s.Trim.ToLower.Contains("case notes mediation status") Then notesStarted = True

                        If s.Trim.ToLower.Contains("days since filing") Then
                            If IsNumeric(s.Trim.Substring(18).Trim) Then
                                r.DaysSinceFiling = CInt(s.Trim.Substring(18).Trim)
                            Else r.DaysSinceFiling = 0
                            End If
                            defendantsFinished = True
                        ElseIf s.Trim.ToLower.StartsWith("type") Then
                            r.CaseType = s.Trim.Split(":"c)(1).Trim
                        ElseIf s.Trim.ToLower.Contains("length of trial") Then
                            If s.Trim.Length > 17 Then r.LengthOfTrial = s.Trim.Substring(17).Trim
                        ElseIf s.Trim.ToLower.Contains("case notes mediation status") Or notesStarted Then
                            If s.Trim.ToLower.Contains("case notes mediation status") Then
                                lstNotes.Add(s.Trim.Substring(29).Trim)
                            Else lstNotes.Add(s.Trim)
                            End If
                        Else
                            If colCount = 3 And Not plaintiffsFinished Then
                                lstPlaintiff.Add(col2)
                                lstPlaintiffAttorney.Add(col3)
                            End If

                            If colCount = 3 And (plaintiffsFinished And Not defendantsFinished) Then
                                lstDefendant.Add(col2)
                                lstDefendantAttorney.Add(col3)
                            End If

                            If colCount = 2 And (plaintiffsFinished And defendantsFinished) And Not issuesOrEventsFinished Then
                                lstIssuesOrEvents.Add(col2)
                            End If
                        End If

                        My.Application.DoEvents()
                    Next

                    r.SessionDate += SessionTime
                    r.Defendant = lstDefendant
                    r.DefendantAttorney = lstDefendantAttorney
                    r.IssuesOrEvents = lstIssuesOrEvents
                    r.Notes = lstNotes
                    r.Plaintiff = lstPlaintiff
                    r.PlaintiffAttorney = lstPlaintiffAttorney

                    If r.IsValid Then retVal.Add(r)
                End If

                My.Application.DoEvents()
            Next

            ' sort the list by record number
            retVal.Sort(Function(p1, p2) p1.RecordNumber.CompareTo(p2.RecordNumber))

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(DailyDocket.CommonCore.Shared.Enums.ProjectName.FileProcessor))
            Dim s As String = ex.ToString
            retVal = New List(Of FileRecord)
        End Try

        Return retVal
    End Function

    Public Shared Function NCCivilFileNew(ByVal fPath As String) As List(Of FileRecord)
        Dim retVal As New List(Of FileRecord)

        Try
            Dim layoutVersion As Integer = 1

            Dim sCounty As String = fPath.Replace(files_tmp, "").Split("\"c)(1)
            Dim sType As String = fPath.Replace(files_tmp, "").Split("\"c)(2)

            Dim SessionDate As DateTime = New Date(2000, 1, 1)
            Dim SessionTime As New TimeSpan
            Dim CourtroomNumber As String = ""
            Dim CourtroomLocation As String = ""
            Dim PresidingJudge As String = ""

            ' open the file and read it into our list(of string) then close the file so we don't leave it open
            ' we remove the blank lines so they don't clutter the routine
            Dim ht As New Hashtable
            Dim lastRecordNumber As Integer = 0
            Dim recordMultiplier As Integer = 1
            Dim tmpRecord As New List(Of String)

            Dim rdr As New System.IO.StreamReader(fPath)
            Do While rdr.Peek() <> -1
                Dim s As String = rdr.ReadLine

                ' remove invalid characters we know about
                s = s.Replace("ChrW(22)", "")
                s = s.Replace(ChrW(22), "")

                If s.Trim.ToLower.StartsWith("session beginning") Then
                    If s.Trim.ToLower.Contains(" at ") Then
                        If IsDate(s.Trim.Substring(18).Substring(0, s.Trim.IndexOf("AT") - 18).Trim) Then
                            SessionDate = CDate(s.Trim.Substring(18).Substring(0, s.Trim.IndexOf("AT") - 18).Trim)
                        End If
                    Else
                        If IsDate(s.Trim.Substring(18).Trim) Then
                            SessionDate = CDate(s.Trim.Substring(18).Trim)
                        End If
                    End If
                ElseIf s.Trim.ToLower.Contains("beginning") And s.Trim.ToLower.Contains(" at ") Then
                    If s.Trim.ToLower.Contains("a.m.") Or s.Trim.ToLower.Contains("p.m.") Then
                        If SessionTime = New TimeSpan Then SessionTime = New TimeSpan(CInt(Right(s.Trim, 10).Split(":"c)(0)), CInt(Right(s.Trim, 10).Split(":"c)(1).Substring(0, 2)), 0)
                    Else
                        If SessionTime = New TimeSpan Then SessionTime = New TimeSpan(CInt(Right(s.Trim, 8).Split(":"c)(0)), CInt(Right(s.Trim, 8).Split(":"c)(1).Substring(0, 2)), 0)
                    End If
                ElseIf s.Trim.ToLower.StartsWith("courtroom number") Then
                    CourtroomNumber = s.Trim.Split(","c)(0).Substring(17).Trim
                    If s.Trim.Contains(",") Then CourtroomLocation = s.Trim.Split(","c)(1).Trim
                ElseIf s.Trim.ToLower.Contains("presiding") And s.Trim.ToLower.Contains("judge") Then
                    PresidingJudge = s.Trim.Split(","c)(0).Trim
                ElseIf s.Trim.ToLower.StartsWith("pldg type clk dt by party") And layoutVersion = 0 Then
                    layoutVersion = 2
                Else
                    If s.Trim <> "" And s.Length > 3 Then
                        If s.ToLower.StartsWith("#") Or (IsNumeric(s.ToLower.Substring(0, 3)) And s.ToLower.Substring(0, 4).EndsWith(" ")) Then
                            If lastRecordNumber > 0 Then
                                Dim tmpRecordNumber As Integer = lastRecordNumber + (100 * recordMultiplier)

                                If ht.ContainsKey("record_" & tmpRecordNumber) Then
                                    recordMultiplier += 1
                                    tmpRecordNumber = lastRecordNumber + (100 * recordMultiplier)
                                End If

                                ht.Add("record_" & tmpRecordNumber, tmpRecord)
                                tmpRecord = New List(Of String)
                            End If

                            ' files with a record number in the first 3 characters of the line also include the case number, time, and type
                            If IsNumeric(s.Trim.ToLower.Substring(0, 3)) Then
                                lastRecordNumber = CInt(s.Trim.ToLower.Substring(0, 3))
                                tmpRecord.Add(s)
                            Else
                                If IsNumeric(s.Trim.Split(" "c)(1)) Then
                                    lastRecordNumber = CInt(s.Trim.Split(" "c)(1))
                                Else
                                    s = s.Replace("#", "")
                                    s = s.ReplaceAll("_", "")
                                    lastRecordNumber = CInt(s.Trim.Split(" "c)(0))
                                End If
                            End If
                        ElseIf (Not s.ToLower.Contains(" page ") And Not s.StartsWith("____________________")) And lastRecordNumber > 0 And s.Trim.Length < 100 Then
                            tmpRecord.Add(s)
                        End If
                    End If
                End If

                My.Application.DoEvents()
            Loop
            rdr.Close()

            ' handle if the file only has 1 record
            'If ht.Count = 0 And tmpRecord.Count > 0 Then
            '    ht.Add("record_" & lastRecordNumber + (100 * recordMultiplier), tmpRecord)
            'End If

            ' handle the last record if it wasn't already added
            If Not ht.ContainsKey("record_" & lastRecordNumber + (100 * recordMultiplier)) Then
                ht.Add("record_" & lastRecordNumber + (100 * recordMultiplier), tmpRecord)
            End If

            ' now we can loop through all the "records" and gather the data into our object
            For Each itm In ht.Keys
                If CType(ht(itm), List(Of String)).Count > 0 Then
                    Dim lstR As List(Of String) = CType(ht(itm), List(Of String))

                    Dim plaintiffsFinished As Boolean = False
                    Dim defendantsFinished As Boolean = False
                    Dim issuesOrEventsStarted As Boolean = False
                    Dim issuesOrEventsFinished As Boolean = False
                    Dim notesStarted As Boolean = False

                    Dim r As New FileRecord()

                    ' create variables to hold the lists
                    Dim lstDefendant As New List(Of String)
                    Dim lstDefendantAttorney As New List(Of String)
                    Dim lstIssuesOrEvents As New List(Of String)
                    Dim lstNotes As New List(Of String)
                    Dim lstPlaintiff As New List(Of String)
                    Dim lstPlaintiffAttorney As New List(Of String)

                    r.RecordNumber = CInt(itm.ToString.Substring(7))
                    r.SessionDate = SessionDate
                    r.CourtRoomNumber = CourtroomNumber
                    r.CourtRoomLocation = CourtroomLocation
                    r.PresidingJudge = PresidingJudge
                    r.RecordData = lstR
                    r.County = sCounty
                    r.State = "NC"
                    r.FileName = fPath
                    r.RecordType = sType
                    r.LayoutVersion = layoutVersion
                    r.Source = DailyDocket.CommonCore.Shared.Enums.RecordSource.Email

                    For Each s As String In lstR
                        Dim colCount As Integer = 0

                        Dim col1 As String = ""
                        Dim col2 As String = ""
                        Dim col3 As String = ""
                        Dim col4 As String = ""

                        Select Case s.Length
                            Case >= 100
                                ' we have 4 columns
                                colCount = 4
                                col1 = s.Substring(0, 15).Trim
                                col2 = s.Substring(16, 41).Trim
                                col3 = s.Substring(42, 18).Trim
                                col4 = s.Substring(68).Trim

                                r.CaseNumber = col2
                                If SessionTime = New TimeSpan Then SessionTime = New TimeSpan(CInt(col3.Split(":"c)(0)), CInt(col3.Split(":"c)(1).Substring(0, 2)), 0)
                                r.CaseType = col4

                            Case > 56
                                ' we have 3 columns
                                colCount = 3
                                col1 = s.Substring(0, 14).Trim
                                col2 = s.Substring(15, 40).Trim
                                col3 = s.Substring(56).Trim

                            Case > 14
                                ' we have 2 columns
                                colCount = 2
                                If s.Trim.ToLower.StartsWith("issues") Or (issuesOrEventsStarted And Not issuesOrEventsFinished) Then
                                    ' the issues section has different column widths
                                    col1 = s.Substring(0, 10).Trim
                                    col2 = s.Substring(11).Trim
                                ElseIf s.Trim.ToLower.StartsWith("event") Or (issuesOrEventsStarted And Not issuesOrEventsFinished) Then
                                    ' the events section has different column widths
                                    col1 = s.Substring(0, 6).Trim
                                    col2 = s.Substring(7).Trim
                                ElseIf s.Trim.ToLower.StartsWith("type") Then
                                    r.CaseType = s.Trim.Split(":"c)(1).Trim
                                Else
                                    ' assume all others are the same
                                    col1 = s.Substring(0, 14).Trim
                                    col2 = s.Substring(15).Trim
                                End If

                            Case < 14
                                ' we have 1 column
                                colCount = 1
                                col1 = s.Trim
                        End Select

                        If r.CaseNumber = "" And col1.Contains("-") Then r.CaseNumber = col1
                        If col2.ToLower = "-vs-" Or col2.ToLower = "vs" Then plaintiffsFinished = True
                        If col1.ToLower.Contains("event") Or col1.ToLower.Contains("issues") Then defendantsFinished = True : issuesOrEventsStarted = True
                        If s.Trim.ToLower.Contains("days since filing") And defendantsFinished Then issuesOrEventsStarted = False : issuesOrEventsFinished = True
                        If s.Trim.ToLower.Contains("case notes mediation status") Then notesStarted = True

                        If s.Trim.ToLower.Contains("days since filing") Then
                            If IsNumeric(s.Trim.Substring(18).Trim) Then
                                r.DaysSinceFiling = CInt(s.Trim.Substring(18).Trim)
                            Else r.DaysSinceFiling = 0
                            End If
                            defendantsFinished = True
                        ElseIf s.Trim.ToLower.StartsWith("type") Then
                            r.CaseType = s.Trim.Split(":"c)(1).Trim
                        ElseIf s.Trim.ToLower.Contains("length of trial") Then
                            If s.Trim.Length > 17 Then r.LengthOfTrial = s.Trim.Substring(17).Trim
                        ElseIf s.Trim.ToLower.Contains("case notes mediation status") Or notesStarted Then
                            If s.Trim.ToLower.Contains("case notes mediation status") Then
                                lstNotes.Add(s.Trim.Substring(29).Trim)
                            Else lstNotes.Add(s.Trim)
                            End If
                        Else
                            If colCount = 3 And Not plaintiffsFinished Then
                                lstPlaintiff.Add(col2)
                                lstPlaintiffAttorney.Add(col3)
                            End If

                            If colCount = 3 And (plaintiffsFinished And Not defendantsFinished) Then
                                lstDefendant.Add(col2)
                                lstDefendantAttorney.Add(col3)
                            End If

                            If colCount = 2 And (plaintiffsFinished And defendantsFinished) And Not issuesOrEventsFinished Then
                                lstIssuesOrEvents.Add(col2)
                            End If
                        End If

                        My.Application.DoEvents()
                    Next

                    r.SessionDate += SessionTime
                    r.Defendant = lstDefendant
                    r.DefendantAttorney = lstDefendantAttorney
                    r.IssuesOrEvents = lstIssuesOrEvents
                    r.Notes = lstNotes
                    r.Plaintiff = lstPlaintiff
                    r.PlaintiffAttorney = lstPlaintiffAttorney

                    If r.IsValid Then retVal.Add(r)
                End If

                My.Application.DoEvents()
            Next

            ' sort the list by record number
            retVal.Sort(Function(p1, p2) p1.RecordNumber.CompareTo(p2.RecordNumber))

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(DailyDocket.CommonCore.Shared.Enums.ProjectName.FileProcessor))
            Dim s As String = ex.ToString
        End Try

        Return retVal
    End Function

    Public Shared Function NCCriminalFile(ByVal fPath As String) As List(Of FileRecord)
        Dim retVal As New List(Of FileRecord)

        Try
            Dim layoutVersion As Integer = 0

            Dim sCounty As String = fPath.Replace(files_tmp, "").Split("\"c)(1)
            Dim sType As String = fPath.Replace(files_tmp, "").Split("\"c)(2)

            Dim Page As Integer = 0
            Dim EndFile As Boolean = False

            Dim SessionDate As DateTime = New Date(2000, 1, 1)
            Dim SessionTime As New TimeSpan
            Dim CourtroomNumber As String = ""
            Dim CourtroomLocation As String = ""
            Dim PresidingJudge As String = ""
            Dim CourtRoomClerk As String = ""
            Dim lstProsecutor As New List(Of String)
            Dim lstAssistantDA As New List(Of String)

            ' open the file and read it into our list(of string) then close the file so we don't leave it open
            ' we remove the blank lines so they don't clutter the routine
            Dim ht As New Hashtable
            Dim lastRecordNumber As Integer = 0
            Dim recordMultiplier As Integer = 1
            Dim tmpRecord As New List(Of String)

            Dim rdr As New System.IO.StreamReader(fPath)
            Do While rdr.Peek() <> -1
                Dim s As String = rdr.ReadLine

                ' remove invalid characters we know about
                s = s.Replace("ChrW(22)", "")
                s = s.Replace(ChrW(22), "")

                If s.Trim.ToLower.StartsWith("court date") And Not EndFile Then
                    Dim colData As String = s.Trim.Substring(11, 17).Trim
                    If IsDate(colData) Then SessionDate = CDate(colData)

                    colData = s.Trim.Substring(33, 16).Trim
                    If SessionTime = New TimeSpan Then SessionTime = New TimeSpan(CInt(colData.Substring(0, 5).Split(":"c)(0)), CInt(colData.Substring(0, 5).Split(":"c)(1).Substring(0, 2)), 0)

                    colData = s.Trim.Substring(67).Trim
                    CourtroomNumber = colData
                ElseIf s.Trim.ToLower.Contains("presiding") And s.Trim.ToLower.Contains("judge") And Not EndFile Then
                    PresidingJudge = s.Trim.Split(":"c)(1).Trim
                ElseIf s.Trim.ToLower.Contains("courtroom") And s.Trim.ToLower.Contains("clerk") And Not EndFile Then
                    CourtRoomClerk = s.Trim.Split(":"c)(1).Trim
                ElseIf s.Trim.ToLower.Contains("prosecutor") And Not EndFile Then
                    Dim v As String = s.Trim.Split(":"c)(1).Trim
                    If v.EndsWith(",DA") Then
                        lstProsecutor.Add(Left(v, v.Length - 3).Trim)
                    Else lstProsecutor.Add(v.Trim)
                    End If
                ElseIf s.Trim.ToLower.StartsWith(": ") And Not EndFile Then
                    Dim v As String = s.Trim.Split(":"c)(1).Trim
                    If v.EndsWith(",ADA") Then
                        lstAssistantDA.Add(Left(v, v.Length - 4).Trim)
                    Else lstProsecutor.Add(v.Trim)
                    End If
                ElseIf s.Trim.ToLower.StartsWith("assistant da") And Not EndFile Then
                    lstAssistantDA.Add(s.Trim.Split(":"c)(1).Trim)
                ElseIf s.Trim.ToLower.StartsWith("location") And Not EndFile Then
                    CourtroomLocation = s.Trim.Substring(9, 21).Trim
                ElseIf s.Trim.ToLower.StartsWith("no.") And layoutVersion = 0 And Not EndFile Then
                    If s.Trim.ToLower.StartsWith("no.") And s.ToLower.Contains("complainant") Then
                        layoutVersion = 1
                    Else layoutVersion = 2
                    End If
                ElseIf s.Trim.ToLower.Contains("page ") Or s.Trim.ToLower.Contains("page:") And Not EndFile Then
                    Dim pg As String = ""
                    'If layoutVersion = 1 Then
                    If s.Trim.ToLower.Contains("page:") Then
                        If s.Trim.Substring(s.Trim.ToLower.IndexOf("page:") + 5).Trim.Length >= 5 Then
                            pg = s.Trim.Substring(s.Trim.ToLower.IndexOf("page:") + 5, 5).Trim
                            If (Page > 0 And CInt(pg) = 1) And CInt(pg) <> Page Then EndFile = True
                            Page = CInt(pg)
                        Else
                            pg = s.Trim.Substring(s.Trim.ToLower.IndexOf("page:") + 5).Trim
                            If (Page > 0 And CInt(pg) = 1) And CInt(pg) <> Page Then EndFile = True
                            Page = CInt(pg)
                        End If
                    ElseIf s.Trim.ToLower.Contains("page") Then
                        If s.Trim.Substring(s.Trim.ToLower.IndexOf("page") + 4).Trim.Length >= 5 Then
                            pg = s.Trim.Substring(s.Trim.ToLower.IndexOf("page") + 4, 5).Trim
                            If IsNumeric(pg) Then
                                If Page > 0 And CInt(pg) = 1 Then EndFile = True
                                Page = CInt(pg)
                            End If
                        Else
                            pg = s.Trim.Substring(s.Trim.ToLower.IndexOf("page") + 4).Trim
                            If IsNumeric(pg) Then
                                If Page > 0 And CInt(pg) = 1 Then EndFile = True
                                Page = CInt(pg)
                            End If
                        End If
                    End If
                    'ElseIf layoutVersion = 2 Then
                    '    If s.Trim.Substring(s.Trim.ToLower.IndexOf("page:") + 5).Trim.Length >= 5 Then
                    '        If Page > 0 And CInt(s.Trim.Substring(s.Trim.ToLower.IndexOf("page:") + 5, 5).Trim) = 1 Then EndFile = True
                    '        Page = CInt(s.Trim.Substring(s.Trim.ToLower.IndexOf("page:") + 5, 5).Trim)
                    '    Else
                    '        If Page > 0 And CInt(s.Trim.Substring(s.Trim.ToLower.IndexOf("page:") + 5).Trim) = 1 Then EndFile = True
                    '        Page = CInt(s.Trim.Substring(s.Trim.ToLower.IndexOf("page:") + 5).Trim)
                    '    End If
                    'End If
                Else
                    If s.Trim <> "" And s.Length > 3 And Not EndFile Then
                        If IsNumeric(s.ToLower.Substring(0, 6).Trim) And IsNumeric(s.ToLower.Substring(8, 2).Trim) Then
                            If lastRecordNumber > 0 Then
                                Dim tmpRecordNumber As Integer = lastRecordNumber + (100 * recordMultiplier)

                                If ht.ContainsKey("record_" & tmpRecordNumber) Then
                                    recordMultiplier += 1
                                    tmpRecordNumber = lastRecordNumber + (100 * recordMultiplier)
                                End If

                                ht.Add("record_" & tmpRecordNumber, tmpRecord)
                                tmpRecord = New List(Of String)
                            End If

                            ' files with a record number in the first 6 characters of the line also include the defendant, complainant, attorney, and continuances
                            If IsNumeric(s.ToLower.Substring(0, 6).Trim) Then
                                lastRecordNumber = CInt(s.ToLower.Substring(0, 6).Trim)
                                tmpRecord.Add(s)
                            End If
                        ElseIf (Not s.ToLower.Contains("page:") _
                                And Not s.Contains("********************")) _
                                And Not s.ToLower.Contains("no.  file number") _
                                And Not s.ToLower.Contains("court date:") _
                                And lastRecordNumber > 0 And Not EndFile Then

                            tmpRecord.Add(s)
                        End If
                    End If
                End If

                My.Application.DoEvents()
            Loop
            rdr.Close()

            ' handle if the file only has 1 record
            'If ht.Count = 0 And tmpRecord.Count > 0 Then
            '    ht.Add("record_" & lastRecordNumber + (100 * recordMultiplier), tmpRecord)
            'End If

            ' handle the last record if it wasn't already added
            If Not ht.ContainsKey("record_" & lastRecordNumber + (100 * recordMultiplier)) Then
                ht.Add("record_" & lastRecordNumber + (100 * recordMultiplier), tmpRecord)
            End If

            ' now we can loop through all the "records" and gather the data into our object
            For Each itm In ht.Keys
                If CType(ht(itm), List(Of String)).Count > 0 Then
                    Dim lstR As List(Of String) = CType(ht(itm), List(Of String))

                    Dim lstCharges As New List(Of CriminalCharge)
                    Dim chg As New CriminalCharge

                    Dim chargeStarted As Boolean = False
                    Dim complainantFinished As Boolean = False

                    Dim r As New FileRecord()

                    ' create variables to hold the lists
                    Dim lstDefendant As New List(Of String)
                    Dim lstDefendantAttorney As New List(Of String)
                    Dim lstNotes As New List(Of String)
                    Dim lstComplainant As New List(Of String)
                    Dim lstComplainantType As New List(Of String)

                    r.RecordNumber = CInt(itm.ToString.Substring(7))
                    r.SessionDate = SessionDate
                    r.CourtRoomNumber = CourtroomNumber
                    r.CourtRoomLocation = CourtroomLocation
                    r.PresidingJudge = PresidingJudge
                    r.CourtRoomClerk = CourtRoomClerk
                    r.Prosecutor = lstProsecutor
                    r.AssistantDA = lstAssistantDA
                    r.RecordData = lstR
                    r.County = sCounty
                    r.State = "NC"
                    r.FileName = fPath
                    r.RecordType = sType
                    r.LayoutVersion = layoutVersion
                    r.Source = DailyDocket.CommonCore.Shared.Enums.RecordSource.Email

                    For Each s As String In lstR
                        If IsNumeric(s.Substring(2, 4).Trim) Then ' first line starts with a record number
                            r.RecordNumber = CInt(s.Substring(2, 4).Trim)

                            If layoutVersion = 1 Then
                                ' line has case number, defendant, complainant, attorney, and continuances
                                r.CaseNumber = s.Substring(6, 13).Trim
                                lstDefendant.Add(s.Substring(20, 22).Trim)
                                If s.Trim.ToLower.Contains("apt.:") Or s.Trim.ToLower.Contains("atty:") Or s.Trim.ToLower.Contains("p.d.:") Then
                                    If IsNumeric(Right(s.Trim, 3).Trim) Then
                                        ' we have continuances
                                        If s.Trim.ToLower.IndexOf("apt") > 0 Then ' appointed
                                            lstDefendantAttorney.Add(s.Trim.Substring(s.Trim.IndexOf("APT.:") + 5, 18).Trim)
                                            'r.AttorneyType = "APT"
                                        ElseIf s.Trim.ToLower.IndexOf("atty") > 0 Then ' private
                                            lstDefendantAttorney.Add(s.Trim.Substring(s.Trim.IndexOf("ATTY:") + 5, 18).Trim)
                                            'r.AttorneyType = "ATTY"
                                        ElseIf s.Trim.ToLower.IndexOf("p.d.") > 0 Then ' public defender
                                            lstDefendantAttorney.Add(s.Trim.Substring(s.Trim.IndexOf("P.D.:") + 5, 18).Trim)
                                            'r.AttorneyType = "PD"
                                        End If
                                        r.Continuances = Right(s.Trim, 3).Trim
                                        lstComplainant.Add(s.Substring(42, 15).Trim)
                                        lstComplainantType.Add(s.Substring(57, 3).Trim)
                                    Else
                                        ' line does not include continuances
                                        If s.Trim.ToLower.IndexOf("apt") > 0 Then ' appointed
                                            lstDefendantAttorney.Add(s.Trim.Substring(s.Trim.IndexOf("APT.:") + 5).Trim)
                                            'r.AttorneyType = "APT"
                                        ElseIf s.Trim.ToLower.IndexOf("atty") > 0 Then ' private
                                            lstDefendantAttorney.Add(s.Trim.Substring(s.Trim.IndexOf("ATTY:") + 5).Trim)
                                            'r.AttorneyType = "ATTY"
                                        ElseIf s.Trim.ToLower.IndexOf("p.d.") > 0 Then ' public defender
                                            lstDefendantAttorney.Add(s.Trim.Substring(s.Trim.IndexOf("P.D.:") + 5).Trim)
                                            'r.AttorneyType = "PD"
                                        End If
                                        If s.Trim.ToLower.Contains("apt") Or s.Trim.ToLower.Contains("atty") Or s.Trim.ToLower.Contains("p.d.") Then
                                            lstComplainant.Add(s.Substring(42, 15).Trim)
                                            lstComplainantType.Add(s.Substring(57, 3).Trim)
                                        Else
                                            lstComplainant.Add(s.Substring(36).Trim)
                                            lstComplainantType.Add("")
                                        End If
                                    End If
                                End If
                            ElseIf layoutVersion = 2 Then
                                r.CaseNumber = s.Substring(7, 14).Trim
                                If s.Trim.ToLower.Contains("ada:") Then
                                    lstDefendant.Add(s.Substring(22, 25).Trim)
                                    If s.Trim.ToLower.Contains("apt.:") Or s.Trim.ToLower.Contains("atty:") Then
                                        If s.Trim.ToLower.IndexOf("apt") > 0 Then ' appointed
                                            lstDefendantAttorney.Add(s.Trim.Substring(s.Trim.IndexOf("APT.:") + 5, 18).Trim)
                                            'r.AttorneyType = "APT"
                                        ElseIf s.Trim.ToLower.IndexOf("atty") > 0 Then ' private
                                            lstDefendantAttorney.Add(s.Trim.Substring(s.Trim.IndexOf("ATTY:") + 5, 18).Trim)
                                            'r.AttorneyType = "ATTY"
                                        End If
                                    End If
                                    If IsNumeric(Right(s.Trim, 3).Trim) Then
                                        ' ada and continuances
                                        chg.ADA = s.Trim.Substring(s.Trim.IndexOf("ADA:") + 4, 4).Trim
                                        r.Continuances = Right(s.Trim, 3).Trim
                                    Else
                                        ' just ada
                                        chg.ADA = s.Trim.Substring(s.Trim.IndexOf("ADA:") + 4).Trim
                                        r.Continuances = "0"
                                    End If
                                Else
                                    If s.Trim.Length >= 47 Then
                                        lstDefendant.Add(s.Substring(22, 25).Trim)
                                        If s.Trim.ToLower.Contains("apt.:") Or s.Trim.ToLower.Contains("atty:") Then
                                            If s.Trim.ToLower.IndexOf("apt") > 0 Then ' appointed
                                                If IsNumeric(Right(s.Trim, 3).Trim) Then
                                                    lstDefendantAttorney.Add(s.Trim.Substring(s.Trim.IndexOf("APT.:") + 5, 18).Trim)
                                                Else lstDefendantAttorney.Add(s.Trim.Substring(s.Trim.IndexOf("APT.:") + 5).Trim)
                                                End If
                                                'r.AttorneyType = "APT"
                                            ElseIf s.Trim.ToLower.IndexOf("atty") > 0 Then ' private
                                                If IsNumeric(Right(s.Trim, 3).Trim) Then
                                                    lstDefendantAttorney.Add(s.Trim.Substring(s.Trim.IndexOf("ATTY:") + 5, 18).Trim)
                                                Else lstDefendantAttorney.Add(s.Trim.Substring(s.Trim.IndexOf("ATTY:") + 5).Trim)
                                                End If
                                                'r.AttorneyType = "ATTY"
                                            End If
                                        End If
                                        If IsNumeric(Right(s.Trim, 3).Trim) Then
                                            r.Continuances = Right(s.Trim, 3).Trim
                                        Else r.Continuances = "0"
                                        End If
                                    Else
                                        lstDefendant.Add(s.Substring(22).Trim)
                                        If s.Trim.ToLower.Contains("apt.:") Or s.Trim.ToLower.Contains("atty:") Then
                                            If s.Trim.ToLower.IndexOf("apt") > 0 Then ' appointed
                                                If IsNumeric(Right(s.Trim, 3).Trim) Then
                                                    lstDefendantAttorney.Add(s.Trim.Substring(s.Trim.IndexOf("APT.:") + 5, 18).Trim)
                                                Else lstDefendantAttorney.Add(s.Trim.Substring(s.Trim.IndexOf("APT.:") + 5).Trim)
                                                End If
                                                'r.AttorneyType = "APT"
                                            ElseIf s.Trim.ToLower.IndexOf("atty") > 0 Then ' private
                                                If IsNumeric(Right(s.Trim, 3).Trim) Then
                                                    lstDefendantAttorney.Add(s.Trim.Substring(s.Trim.IndexOf("ATTY:") + 5, 18).Trim)
                                                Else lstDefendantAttorney.Add(s.Trim.Substring(s.Trim.IndexOf("ATTY:") + 5).Trim)
                                                End If
                                                'r.AttorneyType = "ATTY"
                                            End If
                                        End If
                                        If IsNumeric(Right(s.Trim, 3).Trim) Then
                                            r.Continuances = Right(s.Trim, 3).Trim
                                        Else r.Continuances = "0"
                                        End If
                                    End If
                                End If
                            End If

                        ElseIf s.Trim.ToLower.StartsWith("******** ") Then ' any notes
                            lstNotes.Add(s.Trim.Substring(9).Trim)

                        ElseIf s.Trim.ToLower.StartsWith("aka:") Then
                            r.AKA = s.Trim.Substring(4).Trim

                        ElseIf s.Trim.ToLower.StartsWith("jail status:") Or s.Trim.ToLower.Contains("lid number:") Then
                            r.JailStatus = s.Trim.Substring(s.Trim.IndexOf("JAIL STATUS:") + 12, 2).Trim
                            If s.Trim.ToLower.Contains("lid number") Then
                                r.LidNumber = s.Trim.Substring(s.Trim.IndexOf("LID NUMBER:") + 11, 7).Trim
                            End If

                        ElseIf s.Trim.ToLower.Contains("bond:") Or s.Trim.ToLower.Contains("bo:") Or s.Trim.ToLower.Contains("ap:") Or s.Trim.ToLower.Contains("tb:") Or s.Trim.ToLower.Contains("cmpl:") Then ' line (optional) has the bond amount
                            If s.Trim.ToLower.Contains("bo:") Then ' bond over
                                r.BondOver = s.Trim.Substring(s.Trim.IndexOf("BO:") + 3, 7).Trim
                            End If
                            If s.Trim.ToLower.Contains("ap:") Then ' date appealed
                                r.DateAppealed = s.Trim.Substring(s.Trim.IndexOf("AP:") + 3, 7).Trim
                            End If
                            If s.Trim.ToLower.Contains("tb:") Then ' true bill of indictment
                                r.TrueBillOfIndictment = s.Trim.Substring(s.Trim.IndexOf("TB:") + 3, 7).Trim
                            End If
                            If s.Trim.ToLower.Contains("bond:") Then ' bond amount
                                If s.Trim.Length >= s.Trim.IndexOf("BOND:") + 22 Then
                                    r.Bond = s.Trim.Substring(s.Trim.IndexOf("BOND:") + 5, 17).Trim
                                ElseIf s.Trim.Length >= s.Trim.IndexOf("BOND:") + 21 Then
                                    r.Bond = s.Trim.Substring(s.Trim.IndexOf("BOND:") + 5, 16).Trim
                                ElseIf s.Trim.Length >= s.Trim.IndexOf("BOND:") + 20 Then
                                    r.Bond = s.Trim.Substring(s.Trim.IndexOf("BOND:") + 5, 15).Trim
                                End If
                            End If
                            If s.Trim.ToLower.Contains("cmpl:") Then
                                If s.Trim.Length >= s.Trim.IndexOf("CMPL:") + 19 Then
                                    r.CMPL = s.Trim.Substring(s.Trim.IndexOf("CMPL:") + 5, 14).Trim
                                ElseIf s.Trim.Length >= s.Trim.IndexOf("CMPL:") + 16 Then
                                    r.CMPL = s.Trim.Substring(s.Trim.IndexOf("CMPL:") + 5, 11).Trim
                                End If
                            End If

                        ElseIf s.Trim.ToLower.Contains("judgment:") Then
                            complainantFinished = True

                            If layoutVersion = 1 Then
                                If s.Trim.ToLower.Contains("cls:") Then ' offense class
                                    chg.OffenseClass = s.Trim.Substring(s.Trim.IndexOf("CLS:") + 4, 3).Trim
                                End If
                                If s.Trim.ToLower.Contains("p:") Then ' points
                                    chg.Points = s.Trim.Substring(s.Trim.IndexOf("P:") + 2, 3).Trim
                                End If
                                If s.Trim.ToLower.Contains("l:") Then ' level of punishment
                                    chg.LevelOfPunishment = s.Trim.Substring(s.Trim.IndexOf("L:") + 2, 3).Trim
                                End If
                                If s.Trim.ToLower.Contains("judgment:") Then ' judgment
                                    If s.Length >= 52 Then
                                        chg.Judgment = s.Trim.Substring(s.Trim.IndexOf("JUDGMENT:") + 9, 27).Trim
                                    Else
                                        chg.Judgment = s.Trim.Substring(s.Trim.IndexOf("JUDGMENT:") + 9).Trim
                                    End If
                                End If
                                If s.ToLower.Contains("ada:") Then ' ada
                                    If s.Trim.Length >= s.Trim.IndexOf("ADA:") + 8 Then
                                        chg.ADA = s.Trim.Substring(s.Trim.IndexOf("ADA:") + 4, 4).Trim
                                    Else chg.ADA = s.Trim.Substring(s.Trim.IndexOf("ADA:") + 4).Trim
                                    End If
                                End If
                            ElseIf layoutVersion = 2 Then
                                If s.Trim.ToLower.Contains("cls:") Then ' offense class
                                    chg.OffenseClass = s.Trim.Substring(s.Trim.IndexOf("CLS:") + 4, 3).Trim
                                End If
                                If s.Trim.ToLower.Contains("p:") Then ' points
                                    chg.Points = s.Trim.Substring(s.Trim.IndexOf("P:") + 2, 3).Trim
                                End If
                                If s.Trim.ToLower.Contains(" l:") Then ' level of punishment
                                    chg.LevelOfPunishment = s.Trim.Substring(s.Trim.IndexOf("L:") + 2, 3).Trim
                                End If
                                If s.Trim.ToLower.Contains("plea:") Then ' plea
                                    chg.Plea = s.Trim.Substring(s.Trim.IndexOf("PLEA:") + 5, 6).Trim
                                End If
                                If s.Trim.ToLower.Contains("ver:") Then ' verdict
                                    chg.Verdict = s.Trim.Substring(s.Trim.IndexOf("VER:") + 4, 6).Trim
                                End If
                                If s.Trim.ToLower.Contains("dom vl:") Then ' domestic violence case
                                    Dim v As String = s.Trim.Substring(s.Trim.IndexOf("DOM VL:") + 7, 5).Trim
                                    If v.ToLower = "y" Then chg.DomesticViolence = True Else chg.DomesticViolence = False
                                End If
                                If s.Trim.ToLower.Contains("judgment:") Then ' judgment
                                    chg.Judgment = s.Trim.Substring(s.Trim.IndexOf("JUDGMENT:") + 9).Trim
                                End If
                            End If

                        ElseIf s.Trim.ToLower.StartsWith("(") Then
                            chargeStarted = True
                            complainantFinished = False

                            If chg.OffenseType <> "" Then
                                ' add the charge to the list and reset the variables
                                chargeStarted = False
                                complainantFinished = True
                                chg.Complainant = lstComplainant
                                chg.ComplainantType = lstComplainantType
                                lstCharges.Add(chg)
                                chg = New CriminalCharge
                                lstComplainant = New List(Of String)
                                lstComplainantType = New List(Of String)
                            End If

                            If layoutVersion = 1 Then
                                ' line has the charge, plea, and verdict
                                chg.OffenseType = s.Trim.Substring(0, 3).Trim
                                chg.OffenseText = s.Trim.Substring(3, 33).Trim
                                If s.Trim.ToLower.Contains("plea:") Then ' plea
                                    If s.Trim.Length >= s.Trim.IndexOf("PLEA:") + 21 Then
                                        chg.Plea = s.Trim.Substring(s.Trim.IndexOf("PLEA:") + 5, 16).Trim
                                    Else
                                        chg.Plea = s.Trim.Substring(s.Trim.IndexOf("PLEA:") + 5).Trim
                                    End If
                                End If
                                If s.Trim.ToLower.Contains("ver:") Then ' verdict
                                    If s.Trim.Length >= s.Trim.IndexOf("VER:") + 18 Then
                                        chg.Verdict = s.Trim.Substring(s.Trim.IndexOf("VER:") + 4, 14).Trim
                                    Else
                                        chg.Verdict = s.Trim.Substring(s.Trim.IndexOf("VER:") + 4).Trim
                                    End If
                                End If
                            ElseIf layoutVersion = 2 Then
                                ' line has charge and complainants
                                chg.OffenseType = s.Trim.Substring(0, 3).Trim
                                If s.Trim.Length >= 36 Then
                                    chg.OffenseText = s.Trim.Substring(3, 33).Trim
                                    If s.Trim.Length >= 58 Then
                                        lstComplainant.Add(s.Trim.Substring(36, 19).Trim)
                                        lstComplainantType.Add(s.Trim.Substring(55).Trim)
                                    Else
                                        lstComplainant.Add(s.Trim.Substring(36).Trim)
                                        lstComplainantType.Add("")
                                    End If
                                Else
                                    chg.OffenseText = s.Trim.Substring(3).Trim
                                End If
                            End If

                        ElseIf Not complainantFinished Then
                            If layoutVersion = 1 Then
                            ElseIf layoutVersion = 2 Then
                                ' line has charge and complainants
                                If s.Trim.Length > 19 Then
                                    ' we have a complainant and type
                                    lstComplainant.Add(s.Trim.Substring(0, 19).Trim)
                                    lstComplainantType.Add(s.Trim.Substring(19).Trim)
                                Else
                                    ' we just have the complainant
                                    lstComplainant.Add(s.Trim)
                                    lstComplainantType.Add("")
                                End If
                            End If
                        End If

                        My.Application.DoEvents()
                    Next

                    If chg.OffenseType <> "" Then
                        ' add the charge to the list and reset the variables
                        chargeStarted = False
                        complainantFinished = True
                        chg.Complainant = lstComplainant
                        chg.ComplainantType = lstComplainantType
                        lstCharges.Add(chg)
                        chg = New CriminalCharge
                        lstComplainant = New List(Of String)
                        lstComplainantType = New List(Of String)
                    End If

                    r.SessionDate += SessionTime
                    r.Charges = lstCharges
                    r.Defendant = lstDefendant
                    r.DefendantAttorney = lstDefendantAttorney
                    r.Notes = lstNotes

                    If r.IsValid Then retVal.Add(r) Else Dim asdf As String = ""
                End If

                My.Application.DoEvents()
            Next

            ' sort the list by record number
            retVal.Sort(Function(p1, p2) p1.RecordNumber.CompareTo(p2.RecordNumber))

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(DailyDocket.CommonCore.Shared.Enums.ProjectName.FileProcessor))
            Dim s As String = ex.ToString
        End Try

        Return retVal
    End Function

    Public Shared Function NCCriminalAdminCalendar(ByVal fPath As String) As List(Of FileRecord)
        Dim retVal As New List(Of FileRecord)

        Try
            Dim layoutVersion As Integer = 0

            Dim sCounty As String = fPath.Replace(files_tmp, "").Split("\"c)(1)
            Dim sType As String = "criminal"

            Dim Page As Integer = 0
            Dim EndFile As Boolean = False

            Dim SessionDate As DateTime = New Date(2000, 1, 1)
            Dim SessionTime As New TimeSpan
            Dim AttorneyName As String = ""
            Dim DefendantName As String = ""

            ' these calendars are in pdf format ... so turn the pdf into text and read it into our list(of string)
            ' we remove the blank lines so they don't clutter the routine
            Dim ht As New Hashtable
            Dim lastRecordNumber As Integer = 0
            Dim recordMultiplier As Integer = 1
            Dim tmpRecord As New List(Of String)

            Dim f As List(Of String) = Printing.PdfToText(fPath).Split(CChar(vbLf)).ToList
            For Each s As String In f
                ' remove invalid characters we know about
                s = s.Replace("ChrW(22)", "")
                s = s.Replace(ChrW(22), "")

                If IsDate(s.Trim.ToLower) And Not EndFile Then
                    Dim colData As String = s.Trim
                    If IsDate(colData) Then SessionDate = CDate(colData)

                    ' the admin calendars do not include a time so we default to 9:00AM
                    If SessionTime = New TimeSpan Then SessionTime = New TimeSpan(9, 0, 0)

                    ' there is also no court room number for admin calendar entries
                ElseIf s.Trim.ToLower.Contains("presiding") And s.Trim.ToLower.Contains("judge") And Not EndFile Then
                    ' no judge
                ElseIf s.Trim.ToLower.Contains("courtroom") And s.Trim.ToLower.Contains("clerk") And Not EndFile Then
                    ' no clerk
                ElseIf s.Trim.ToLower.Contains("prosecutor") And Not EndFile Then
                    ' no prosecutor
                ElseIf s.Trim.ToLower.StartsWith(": ") And Not EndFile Then
                    ' no da or ada ... just initials handled later
                ElseIf s.Trim.ToLower.StartsWith("assistant da") And Not EndFile Then
                    ' no ada
                ElseIf s.Trim.ToLower.StartsWith("location") And Not EndFile Then
                    ' no location
                ElseIf s.Trim.ToLower.StartsWith("no.") And layoutVersion = 0 And Not EndFile Then
                    ' layout version
                ElseIf s.Trim.ToLower.Contains("page ") Or s.Trim.ToLower.Contains("page:") And Not EndFile Then
                    Dim pg As String = ""
                    If s.Trim.ToLower.Contains("page:") Then
                        If s.Trim.Substring(s.Trim.ToLower.IndexOf("page:") + 5).Trim.Length >= 5 Then
                            pg = s.Trim.Substring(s.Trim.ToLower.IndexOf("page:") + 5, 5).Trim
                            If (Page > 0 And CInt(pg) = 1) And CInt(pg) <> Page Then EndFile = True
                            Page = CInt(pg)
                        Else
                            pg = s.Trim.Substring(s.Trim.ToLower.IndexOf("page:") + 5).Trim
                            If (Page > 0 And CInt(pg) = 1) And CInt(pg) <> Page Then EndFile = True
                            Page = CInt(pg)
                        End If
                        If s.Trim.Length > 6 Then tmpRecord.Add(s.Substring(0, s.IndexOf("page")))
                    ElseIf s.Trim.ToLower.Contains("page ") Then
                        If s.Trim.Substring(s.Trim.ToLower.IndexOf("page ") + 5).Trim.Length >= 5 Then
                            pg = s.Trim.Substring(s.Trim.ToLower.IndexOf("page ") + 5, 5).Trim
                            If IsNumeric(pg) Then
                                If Page > 0 And CInt(pg) = 1 Then EndFile = True
                                Page = CInt(pg)
                            End If
                        Else
                            pg = s.Trim.Substring(s.Trim.ToLower.IndexOf("page ") + 5).Trim
                            If IsNumeric(pg) Then
                                If Page > 0 And CInt(pg) = 1 Then EndFile = True
                                Page = CInt(pg)
                            End If
                        End If
                        If s.Trim.Length > 6 Then tmpRecord.Add(s.Substring(0, s.ToLower.IndexOf("page")))
                    End If
                Else
                    If s.Trim <> "" And s.Length > 3 And Not EndFile Then
                        ' there is no record number either so we use the case number to choose records instead
                        If s.Length <= 27 OrElse (IsNumeric(s.ToLower.Substring(27, 2).Trim) AndAlso IsNumeric(s.ToLower.Substring(32, 6).Trim)) Then
                            If lastRecordNumber > 0 Then
                                Dim tmpRecordNumber As Integer = lastRecordNumber + (100 * recordMultiplier)

                                If ht.ContainsKey("record_" & tmpRecordNumber) Then
                                    recordMultiplier += 1
                                    tmpRecordNumber = lastRecordNumber + (100 * recordMultiplier)
                                End If

                                ht.Add("record_" & tmpRecordNumber, tmpRecord)
                                tmpRecord = New List(Of String)
                            End If

                            If s.Length > 27 AndAlso s.Substring(0, 27).Contains(",") Then
                                ' grab the defendant name if the line has one (this is used for lines that don't have it)
                                DefendantName = s.Substring(0, 27).Trim
                            End If

                            If s.Length <= 27 Then
                                AttorneyName = s.Trim
                            ElseIf IsNumeric(s.ToLower.Substring(27, 2).Trim) AndAlso IsNumeric(s.ToLower.Substring(32, 6).Trim) Then
                                If s.Substring(0, 27).Trim = "" Then
                                    ' no defendant name on this line, so add it ...
                                    s = DefendantName & s.Substring(DefendantName.Length)
                                End If

                                lastRecordNumber += 1
                                If Not tmpRecord.Contains(AttorneyName) Then tmpRecord.Add(AttorneyName)
                                tmpRecord.Add(s)
                            End If
                        ElseIf (Not s.Contains("********************")) _
                            And Not s.ToLower.Contains("administrative calendar") _
                            And Not s.ToLower.Contains("defendant name") _
                            And Not s.ToLower.Contains("no.  file number") _
                            And Not s.ToLower.Contains("court date:") _
                            And lastRecordNumber > 0 And Not EndFile Then

                            tmpRecord.Add(s)
                        End If
                    End If
                End If

                My.Application.DoEvents()
            Next

            ' handle the last record if it wasn't already added
            If Not ht.ContainsKey("record_" & lastRecordNumber + (100 * recordMultiplier)) Then
                ht.Add("record_" & lastRecordNumber + (100 * recordMultiplier), tmpRecord)
            End If

            ' now we can loop through all the "records" and gather the data into our object
            For Each itm In ht.Keys
                If CType(ht(itm), List(Of String)).Count > 0 Then
                    Dim lstR As List(Of String) = CType(ht(itm), List(Of String))

                    Dim lstCharges As New List(Of CriminalCharge)
                    Dim chg As New CriminalCharge

                    Dim chargeStarted As Boolean = False
                    Dim complainantFinished As Boolean = False

                    Dim r As New FileRecord()

                    ' create variables to hold the lists
                    Dim lstDefendant As New List(Of String)
                    Dim lstDefendantAttorney As New List(Of String)

                    r.RecordNumber = CInt(itm.ToString.Substring(7))
                    r.SessionDate = SessionDate
                    r.RecordData = lstR
                    r.County = sCounty
                    r.State = "NC"
                    r.FileName = fPath
                    r.RecordType = sType
                    r.LayoutVersion = layoutVersion
                    r.Source = DailyDocket.CommonCore.Shared.Enums.RecordSource.AdminCalendar

                    For Each s As String In lstR
                        If s.Length <= 27 Then ' first line is the attorney name
                            If Not lstDefendantAttorney.Contains(s.Trim) Then lstDefendantAttorney.Add(s.Trim)
                        ElseIf IsNumeric(s.ToLower.Substring(27, 2).Trim) AndAlso IsNumeric(s.ToLower.Substring(32, 6).Trim) Then
                            ' this line has a defendant, case number, charge, indict date, da, set
                            If s.Substring(0, 27).Contains(",") Then
                                ' we have a defendant name
                                If Not lstDefendant.Contains(s.Substring(0, 27).Trim) Then lstDefendant.Add(s.Substring(0, 27).Trim)
                            End If
                            r.CaseNumber = s.Substring(27, 13).Trim
                            chg.OffenseText = s.Substring(40, 27).Trim
                            If IsDate(s.Substring(67, 12).Trim) Then
                                r.IndictDate = CDate(s.Substring(67, 12).Trim)
                            End If
                            chg.ADA = s.Substring(79, 5).Trim
                            r.Set = s.Substring(84, 4).Trim
                            lstCharges.Add(chg)
                            chg = New CriminalCharge
                        Else
                            ' this line only has charges to add to the list
                            'r.CaseNumber = s.Substring(27, 13).Trim
                            chg.OffenseText = s.Substring(40, 27).Trim
                            If IsDate(s.Substring(67, 12).Trim) Then
                                r.IndictDate = CDate(s.Substring(67, 12).Trim)
                            End If
                            chg.ADA = s.Substring(79, 5).Trim
                            r.Set = s.Substring(84, 4).Trim
                            lstCharges.Add(chg)
                            chg = New CriminalCharge
                        End If

                        My.Application.DoEvents()
                    Next

                    r.SessionDate += SessionTime
                    r.Charges = lstCharges
                    r.Defendant = lstDefendant
                    r.DefendantAttorney = lstDefendantAttorney

                    If r.IsValid Then retVal.Add(r) Else Dim asdf As String = ""
                End If

                My.Application.DoEvents()
            Next

            ' sort the list by record number
            retVal.Sort(Function(p1, p2) p1.RecordNumber.CompareTo(p2.RecordNumber))

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(DailyDocket.CommonCore.Shared.Enums.ProjectName.FileProcessor))
            Dim s As String = ex.ToString
        End Try

        Return retVal
    End Function
End Class
