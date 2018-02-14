Imports System.IO
Imports System.Net
Imports System.Reflection
Imports System.Text.RegularExpressions
Imports System.Web

Public Class Common
    Private Const AspNetNamespace As String = "ASP"

    Public Shared Function GetApplicationAssembly(ctx As HttpContext) As Assembly
        ' Try the EntryAssembly, this doesn't work for ASP.NET classic pipeline (untested on integrated)
        Dim ass As Assembly = Assembly.GetEntryAssembly()

        ' Look for web application assembly
        If ctx IsNot Nothing Then
            ass = GetWebApplicationAssembly(ctx)
        End If

        ' Fallback to executing assembly
        Return If(ass, (Assembly.GetExecutingAssembly()))
    End Function

    Private Shared Function GetWebApplicationAssembly(context As HttpContext) As Assembly
        'Guard.AgainstNullArgument(context)

        Dim app As Object = context.ApplicationInstance
        If app Is Nothing Then
            Return Nothing
        End If

        Dim type As Type = app.[GetType]()
        While type IsNot Nothing AndAlso type <> GetType(Object) AndAlso type.[Namespace] = AspNetNamespace
            type = type.BaseType
        End While

        Return type.Assembly
    End Function

    Public Shared Function ConnectionString() As String
        Return ConnectionString(Enums.DBName.DailyDocket)
    End Function
    Public Shared Function ConnectionString(ByVal DbName As Enums.DBName) As String
        Dim retVal As String = ""

        Dim db As String = ""

        Select Case DbName
            Case Enums.DBName.DailyDocket : db = "solvtopia_DailyDocket"

            Case Else
                'If System.Configuration.ConfigurationManager.ConnectionStrings(DbName) IsNot Nothing Then
                '    retVal = System.Configuration.ConfigurationManager.ConnectionStrings(DbName).ConnectionString
                '    Return retVal
                'End If

        End Select

        retVal = My.Settings.DbConnection.Replace("[DataBaseName]", db)

        ' if the project is running local on the sql server then change the server to local for speed
        'If My.Computer.Name.ToLower.StartsWith("WIN-UANEHHPM5J8") Then
        '    retVal = retVal.Replace("mssql.solvtopia.com", "localhost")
        'End If

        Return retVal
    End Function

    Public Shared Function SearchDir(ByVal sDir As String, ByVal searchPattern As String, ByVal sort As Enums.FileSort) As List(Of IO.FileInfo)
        Dim retVal As New List(Of IO.FileInfo)

        Dim d As String
        Dim f As String

        Try
            ' recursively search any sub directories
            ' add files for the current directory
            For Each f In Directory.GetFiles(sDir, searchPattern)
                Dim fi As New FileInfo(f)
                retVal.Add(fi)
            Next

            ' add any sub directories
            Dim directories() As String = Directory.GetDirectories(sDir)
            For Each d In directories
                retVal.AddRange(SearchDir(d, searchPattern, sort))
            Next

            Select Case sort
                Case Enums.FileSort.Name
                    retVal.Sort(Function(p1, p2) p1.Name.CompareTo(p2.Name))
                Case Enums.FileSort.Size
                    retVal.Sort(Function(p1, p2) p1.Length.CompareTo(p2.Length))
            End Select

        Catch ex As System.Exception
            ex.WriteToErrorLog(New ErrorLogEntry(Enums.ProjectName.CommonCoreShared))
        End Try

        Return retVal
    End Function

    Public Shared Function GetNameParts(ByVal name As String, ByVal type As Enums.NameType) As NameParts
        Dim retVal As New NameParts

        If name <> "" Then
            Dim nameParts As New List(Of String)
        If name.Contains(","c) Then
            nameParts = name.Split(","c).ToList
        ElseIf name.Contains(" "c) Then
            nameParts = name.Split(" "c).ToList
        End If

            If type = Enums.NameType.FirstNameFirst Then
                If nameParts.Count >= 3 Then
                    ' we have a first, middle, and last name
                    retVal.FirstName = nameParts(0).Trim
                    retVal.MiddleName = nameParts(1).Trim.Replace(".", "")
                    retVal.LastName = nameParts(2).Trim
                    If nameParts.Count > 3 Then retVal.Extra = nameParts(3).Trim
                ElseIf nameParts.Count = 2 Then
                    ' we have a first, and last name
                    ' make sure we don't have an initial for the middle name ... if so, remove it
                    If nameParts(0).Contains(" ") Then nameParts(0) = nameParts(0).Substring(0, nameParts(0).IndexOf(" ")).Trim

                    retVal.FirstName = nameParts(0).Trim
                    retVal.MiddleName = ""
                    retVal.LastName = nameParts(1).Trim
                    retVal.Extra = ""
                End If
            ElseIf type = Enums.NameType.LastNameFirst Then
                If nameParts.Count >= 3 Then
                    ' we have a first, middle, and last name
                    retVal.FirstName = nameParts(1).Trim
                    retVal.MiddleName = nameParts(2).Trim.Replace(".", "")
                    retVal.LastName = nameParts(0).Trim
                    If nameParts.Count > 3 Then retVal.Extra = nameParts(3).Trim
                ElseIf nameParts.Count = 2 Then
                    ' we have a first, and last name
                    ' make sure we don't have an initial for the middle name ... if so, remove it
                    If nameParts(1).Contains(" ") Then nameParts(1) = nameParts(1).Substring(0, nameParts(1).IndexOf(" ")).Trim

                    retVal.FirstName = nameParts(1).Trim
                    retVal.MiddleName = ""
                    retVal.LastName = nameParts(0).Trim
                    retVal.Extra = ""
                End If
            End If
        End If

        Return retVal
    End Function

    Public Shared Function FormatName(ByVal name As String) As String
        Dim retVal As String = ""

        If name <> "" Then
            Dim nameParts As NameParts = GetNameParts(name, Enums.NameType.LastNameFirst)

            If nameParts.MiddleName = "" Then
                retVal = nameParts.FirstName & " " & nameParts.LastName
            Else retVal = nameParts.FirstName & " " & nameParts.MiddleName & " " & nameParts.LastName
            End If
            If nameParts.Extra <> "" Then retVal &= " " & nameParts.Extra

            ' pro se needs to stay the same ...
            If name.ToLower = "pro,se" Or name.ToLower = "pro se" Then retVal = "PRO SE"
        End If

        Return retVal
    End Function

    Public Shared Function LookupAttorney(ByVal barNumber As String) As AttorneyRecord
        Return LookupAttorney(barNumber, "", "")
    End Function
    Public Shared Function LookupAttorney(ByVal name As String, ByVal state As String) As AttorneyRecord
        Return LookupAttorney("", name, state)
    End Function
    Public Shared Function LookupAttorney(ByVal barNumber As String, ByVal name As String, ByVal state As String) As AttorneyRecord
        Dim retVal As New AttorneyRecord
        retVal.Active = True
        retVal.NameFromFile = name

        Try
            If name.ToLower <> "pro,se" And name.ToLower <> "pro se" Then
                Dim nameParts As NameParts = GetNameParts(name, Enums.NameType.LastNameFirst)

                Dim bFoundRecord As Boolean = False

                ' first we need to query the nc bar website to see if we can find the attorney record
                Dim url As String
                Dim foundBarId As String = ""
                Dim urlText As String

                If barNumber = "" And (nameParts.FirstName <> "" Or nameParts.MiddleName <> "" Or nameParts.LastName <> "") Then
                    url = "https://www.ncbar.gov/gxweb_if/results.aspx?"

                    url &= nameParts.FirstName & "," & nameParts.MiddleName & "," & nameParts.LastName & ",," & state

                    urlText = ScrapeUrl(url, Enums.ScrapeType.RemoveAll)

                    ' process the results from the url to see if we have a match
                    Dim recordResult As String = ""
                    For Each s As String In urlText.Split(CChar(vbLf)).ToList
                        If (s.ToLower.Contains(nameParts.FirstName.ToLower) And
                            s.ToLower.Contains(nameParts.MiddleName.ToLower) And
                            s.ToLower.Contains(nameParts.LastName.ToLower) And
                            s.ToLower.Contains("active")) Then recordResult = s.Trim : bFoundRecord = True : Exit For
                    Next
                    recordResult = recordResult.Substring(recordResult.ToLower.IndexOf("bar id"))
                    recordResult = recordResult.Replace("<TD class=""MTTextGrid"">", "").Replace("</TD>", "")
                    Dim x As Integer = 0
                    For Each s As String In recordResult.Split("|"c)
                        If IsNumeric(s) Then
                            If recordResult.Split("|"c)(x + 7).Trim.ToLower = "active" Then
                                foundBarId = s.Trim
                                Exit For
                            End If
                        End If
                        x += 1
                    Next

                    ' see if we got a bar id and if the name we found has all the parts of the name we're looking for
                    bFoundRecord = (IsNumeric(foundBarId) And
                           recordResult.ToLower.Contains(nameParts.FirstName.ToLower) And
                           recordResult.ToLower.Contains(nameParts.MiddleName.ToLower) And
                           recordResult.ToLower.Contains(nameParts.LastName.ToLower))

                End If

                ' we found the attorney so pull the details and put it in our object
                If bFoundRecord Or barNumber <> "" Then
                    If barNumber <> "" Then foundBarId = barNumber

                    url = "https://www.ncbar.gov/gxweb_if/wpViewMember.aspx?" & foundBarId
                    urlText = ScrapeUrl(url, Enums.ScrapeType.RemoveAll)

                    Dim line1 As String = ""
                    Dim line2 As String = ""

                    ' find the lines with data and save them
                    Dim i As Integer = 0
                    For Each s As String In urlText.Split(CChar(vbLf)).ToList
                        If (s.ToLower.Contains(nameParts.FirstName.ToLower) And
                            s.ToLower.Contains(nameParts.MiddleName.ToLower) And
                            s.ToLower.Contains(nameParts.LastName.ToLower) And
                            s.ToLower.Contains(foundBarId)) Then

                            line1 = s
                            line2 = urlText.Split(CChar(vbLf)).ToList(i + 1)
                            Exit For
                        End If
                        i += 1
                    Next

                    ' loop through line1 and find the properties we need
                    i = 0
                    For Each s As String In line1.Split("|"c)
                        If s.Trim.ToLower = "id" Then
                            retVal.BarNumber = line1.Split("|"c)(i + 4).Trim
                        ElseIf s.Trim.ToLower = "name" Then
                            retVal.Name = line1.Split("|"c)(i + 4).Trim
                        ElseIf s.Trim.ToLower = "address" Then
                            retVal.Address1 = line1.Split("|"c)(i + 4).Trim
                            retVal.Address2 = line1.Split("|"c)(i + 12).Trim
                            retVal.Address3 = line1.Split("|"c)(i + 20).Trim
                        ElseIf s.Trim.ToLower = "city" Then
                            retVal.City = line1.Split("|"c)(i + 4).Trim
                        ElseIf s.Trim.ToLower = "state" Then
                            retVal.State = line1.Split("|"c)(i + 4).Trim
                        ElseIf s.Trim.ToLower = "zip code" Then
                            retVal.ZipCode = line1.Split("|"c)(i + 4).Trim
                        ElseIf s.Trim.ToLower = "work phone" Then
                            retVal.WorkPhone = line1.Split("|"c)(i + 4).Trim
                        ElseIf s.Trim.ToLower = "fax" Then
                            retVal.Fax = line1.Split("|"c)(i + 4).Trim
                        ElseIf s.Trim.ToLower = "email" Then
                            retVal.Email = line1.Split("|"c)(i + 4).Trim
                        ElseIf s.Trim.ToLower = "license date" Then
                            retVal.LicenseDate = CDate(line1.Split("|"c)(i + 5).Trim)
                        End If

                        i += 1
                    Next

                    ' loop through line2 and find the properties we need
                    i = 0
                    For Each s As String In line2.Split("|"c)
                        If s.Trim.ToLower = "status" Then
                            retVal.Status = line2.Split("|"c)(i + 4).Trim
                        End If

                        i += 1
                    Next
                End If
            End If

        Catch ex As Exception
        End Try

        Return retVal
    End Function

    Public Shared Function ScrapeUrl(ByVal url As String, ByVal type As Enums.ScrapeType) As String
        Dim retVal As String = ""

        Try
            Dim strOutput As String = ""

            Dim wrResponse As WebResponse
            Dim wrRequest As WebRequest = HttpWebRequest.Create(url)

            wrResponse = wrRequest.GetResponse()

            Using sr As New StreamReader(wrResponse.GetResponseStream())
                strOutput = sr.ReadToEnd()
                sr.Close()
            End Using

            'Formatting Techniques
            If Not type = Enums.ScrapeType.ReturnAll Then
                ' Remove Doctype ( HTML 5 )
                strOutput = Regex.Replace(strOutput, "<!(.|\s)*?>", "")

                If Not type = Enums.ScrapeType.KeepTags Then
                    ' Replace HTML Tags with a pipe (|) so we can keep values separate
                    strOutput = Regex.Replace(strOutput, "</?[a-z][a-z0-9]*[^<>]*>", "|")
                End If

                ' Remove HTML Comments
                strOutput = Regex.Replace(strOutput, "<!--(.|\s)*?-->", "")

                ' Remove Script Tags
                strOutput = Regex.Replace(strOutput, "<script.*?</script>", "", RegexOptions.Singleline Or RegexOptions.IgnoreCase)

                ' Remove Stylesheets
                strOutput = Regex.Replace(strOutput, "<style.*?</style>", "", RegexOptions.Singleline Or RegexOptions.IgnoreCase)
            End If

            retVal = strOutput

        Catch ex As Exception
            Console.WriteLine(ex.Message, "Error")
        End Try

        Return retVal
    End Function

End Class
