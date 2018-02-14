Imports System.Security.Cryptography
Imports System.Text
Imports DailyDocket.CommonCore.Shared.Enums
Imports DailyDocket.CommonCore.Shared.Common
Imports System.Reflection
Imports Telerik.Web.UI
Imports System.Xml

Public Class FileRecord
    Sub New()
        Me.ID = 0

        Me.Plaintiff = New List(Of String)
        Me.PlaintiffAttorney = New List(Of String)
        Me.Defendant = New List(Of String)
        Me.DefendantAttorney = New List(Of String)
        Me.Prosecutor = New List(Of String)
        Me.AssistantDA = New List(Of String)
        Me.IssuesOrEvents = New List(Of String)
        Me.Notes = New List(Of String)
        Me.RecordData = New List(Of String)
        Me.Charges = New List(Of CriminalCharge)

        Me.Source = RecordSource.Email
    End Sub
    Sub New(ByVal id As Integer)
        Dim cn As New SqlClient.SqlConnection(ConnectionString)

        Try
            Dim cmd As New SqlClient.SqlCommand("SELECT * FROM [FileRecords] WHERE [ID] = " & id & ";", cn)
            If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
            Dim rs As SqlClient.SqlDataReader = cmd.ExecuteReader
            If rs.Read Then
                ' make a list of all the fields and values
                Dim lst As New List(Of NameValuePair)
                For x As Integer = 0 To rs.FieldCount - 1
                    lst.Add(New NameValuePair(rs.GetName(x), If(IsDBNull(rs.GetValue(x)), "", rs.GetValue(x).ToString)))
                Next

                Me.Initialize(lst)
            End If
            cmd.Cancel()
            rs.Close()

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(Enums.ProjectName.CommonCoreShared))
        Finally
            cn.Close()
        End Try
    End Sub
    Sub New(ByVal lst As List(Of NameValuePair))
        Me.Initialize(lst)
    End Sub

    Public Property ID As Integer
    Public Property Active As Boolean
    Public Property CaseNumber As String
    Public Property RecordData As List(Of String)
    Public Property RecordNumber As Integer
    Public Property FileName As String
    Public Property RecordType As String
    Public Property LayoutVersion As Integer
    Public Property Source As RecordSource
    Public Property Defendant As List(Of String)
    Public Property DefendantAttorney As List(Of String)
    Public Property Notes As List(Of String)

#Region "Civil Fields"

    Public Property Plaintiff As List(Of String)
    Public Property PlaintiffAttorney As List(Of String)
    Public Property CaseType As String
    Public Property IssuesOrEvents As List(Of String)
    Public Property DaysSinceFiling As Integer
    Public Property LengthOfTrial As String

#End Region


#Region "Criminal Fields"

    Public Property CourtRoomClerk As String
    Public Property Prosecutor As List(Of String)
    Public Property Continuances As String ' CONT
    Public Property JailStatus As String
    Public Property LidNumber As String
    Public Property CMPL As String
    Public Property Bond As String
    Public Property TrueBillOfIndictment As String ' TB
    Public Property BondOver As String ' BO
    Public Property DateAppealed As String ' AP
    Public Property Charges As List(Of CriminalCharge)
    Public Property AssistantDA As List(Of String)
    Public Property AKA As String
    Public Property IndictDate As Date
    Public Property [Set] As String

#End Region


#Region "Header Fields"

    Public Property County As String
    Public Property State As String
    Public Property SessionDate As DateTime

    Public ReadOnly Property EndDate() As DateTime
        Get
            Return Me.SessionDate.AddMinutes(60)
        End Get
    End Property

    Public Property CourtRoomNumber As String
    Public Property CourtRoomLocation As String
    Public Property PresidingJudge As String

#End Region


#Region "Workers"

    ' returns a populated Object by looping through all properties And fields Of an Object And getting the value
    ' out of a list(of NameValuePair)
    Private Function ObjectFromReader(ByVal obj As Object, ByVal lst As List(Of NameValuePair)) As Object
        ' get the type of the object we are working with
        Dim objectType As Type = CType(obj.GetType, Type)

        ' get a list of properties and fields to loop through
        Dim properties As PropertyInfo() = objectType.GetProperties()
        Dim fields As FieldInfo() = objectType.GetFields()

        ' loop through properties first (this works on compiled objects)
        For Each prop As PropertyInfo In properties
            ' get the value of the property from the list
            Dim setValue As String = lst.FindItem(prop.Name).value.XmlDecode.XmlDecode
            Dim propType As String = prop.PropertyType.ToString

            Try
                Select Case True
                    Case prop.PropertyType Is System.Type.GetType("System.String")
                        prop.SetValue(obj, setValue, Nothing)
                    Case prop.PropertyType Is System.Type.GetType("System.Int32")
                        prop.SetValue(obj, setValue.ToInteger, Nothing)
                    Case prop.PropertyType Is System.Type.GetType("System.Double")
                        prop.SetValue(obj, setValue.ToDouble, Nothing)
                    Case prop.PropertyType Is System.Type.GetType("System.Date"), prop.PropertyType Is System.Type.GetType("System.DateTime")
                        prop.SetValue(obj, setValue.ToDate(New Date), Nothing)
                    Case prop.PropertyType Is System.Type.GetType("System.Boolean")
                        prop.SetValue(obj, setValue.ToBoolean, Nothing)
                    Case Else
                        Dim s As String = prop.PropertyType.ToString
                        ' enums and collections are handled separate
                        If propType.ToLower.Contains("enum") Then
                            Select Case True
                                Case propType.ToLower.Contains("recordsource")
                                    prop.SetValue(obj, CType(setValue, Enums.RecordSource))
                                Case Else
                                    Dim q As String = ""
                            End Select
                            ' enums we have to find the right type
                        ElseIf propType.ToLower.Contains("collection") Then
                            ' collections are stored as xml in a nvarchar(max) field
                            Dim xDoc As New XmlDocument
                            xDoc.LoadXml("<data>" & setValue & "</data>")
                            Select Case True
                                Case propType.ToLower.Contains("string")
                                    Dim lstValues As New List(Of String)
                                    For Each node As XmlNode In xDoc.GetElementsByTagName("string")
                                        lstValues.Add(node.InnerText.XmlDecode)
                                    Next
                                    prop.SetValue(obj, lstValues)
                                Case propType.ToLower.Contains("criminalcharge")
                                    Dim lstValues As New List(Of CriminalCharge)
                                    For Each node As XmlNode In xDoc.GetElementsByTagName("CriminalCharge")
                                        Dim chg As New CriminalCharge
                                        If node.Item("ADA") IsNot Nothing Then chg.ADA = node.Item("ADA").InnerText
                                        chg.Complainant = New List(Of String)
                                        If node.Item("Complainant") IsNot Nothing Then
                                            For Each n As XmlNode In node.Item("Complainant").GetElementsByTagName("string")
                                                chg.Complainant.Add(n.InnerText)
                                            Next
                                        End If
                                        chg.ComplainantType = New List(Of String)
                                        If node.Item("ComplainantType") IsNot Nothing Then
                                            For Each n As XmlNode In node.Item("ComplainantType").GetElementsByTagName("string")
                                                chg.ComplainantType.Add(n.InnerText)
                                            Next
                                        End If
                                        If node.Item("DomesticViolence") IsNot Nothing Then chg.DomesticViolence = node.Item("DomesticViolence").InnerText.ToBoolean
                                        If node.Item("Judgment") IsNot Nothing Then chg.Judgment = node.Item("Judgment").InnerText
                                        If node.Item("LevelOfPunishment") IsNot Nothing Then chg.LevelOfPunishment = node.Item("LevelOfPunishment").InnerText
                                        If node.Item("OffenseClass") IsNot Nothing Then chg.OffenseClass = node.Item("OffenseClass").InnerText
                                        If node.Item("OffenseText") IsNot Nothing Then chg.OffenseText = node.Item("OffenseText").InnerText
                                        If node.Item("OffenseType") IsNot Nothing Then chg.OffenseType = node.Item("OffenseType").InnerText
                                        If node.Item("Plea") IsNot Nothing Then chg.Plea = node.Item("Plea").InnerText
                                        If node.Item("Points") IsNot Nothing Then chg.Points = node.Item("Points").InnerText
                                        If node.Item("Verdict") IsNot Nothing Then chg.Verdict = node.Item("Verdict").InnerText
                                        lstValues.Add(chg)
                                    Next
                                    prop.SetValue(obj, lstValues)
                                Case Else
                                    Dim q As String = ""
                            End Select
                        End If
                End Select

            Catch ex As Exception
                ' skip any errors
            End Try
        Next

        ' now loop through the fields (this works on local classes)
        For Each fld As FieldInfo In fields
            ' get the value of the field from the xml
            Dim setValue As String = lst.FindItem(fld.Name).value.XmlDecode.XmlDecode

            Try
                Select Case True
                    Case fld.FieldType Is System.Type.GetType("System.String")
                        fld.SetValue(obj, setValue)
                    Case fld.FieldType Is System.Type.GetType("System.Int32")
                        fld.SetValue(obj, CInt(IIf(IsNumeric(setValue), setValue, 0)))
                    Case fld.FieldType Is System.Type.GetType("System.Double")
                        fld.SetValue(obj, CDbl(IIf(IsNumeric(setValue), setValue, 0)))
                    Case fld.FieldType Is System.Type.GetType("System.Date"), fld.FieldType Is System.Type.GetType("System.DateTime")
                        fld.SetValue(obj, CDate(IIf(IsDate(setValue), setValue, Now)))
                    Case fld.FieldType Is System.Type.GetType("System.Boolean")
                        fld.SetValue(obj, setValue.ToBoolean)
                    Case Else
                        Dim s As String = fld.FieldType.ToString
                        ' skip the complex types
                End Select

            Catch ex As Exception
                ' skip any errors
            End Try
        Next

        Return obj
    End Function

    Public Sub Initialize(ByVal lst As List(Of NameValuePair))
        Try
            Dim typeB As Type = Me.[GetType]()

            Dim source As New FileRecord
            source = CType(ObjectFromReader(source, lst), FileRecord)

            ' handle the properties
            For Each [property] As PropertyInfo In source.[GetType]().GetProperties()
                If Not [property].CanRead OrElse ([property].GetIndexParameters().Length > 0) Then
                    Continue For
                End If

                Dim other As PropertyInfo = typeB.GetProperty([property].Name)
                If (other IsNot Nothing) AndAlso (other.CanWrite) Then
                    other.SetValue(Me, [property].GetValue(source, Nothing), Nothing)
                End If
            Next

            ' handle the fields
            For Each [field] As FieldInfo In source.[GetType]().GetFields()
                Dim other As FieldInfo = typeB.GetField([field].Name)
                If (other IsNot Nothing) Then
                    other.SetValue(Me, [field].GetValue(source))
                End If
            Next

        Catch ex As Exception
            Dim s As String = ""
        End Try
    End Sub

    Public Function Save() As FileRecord
        Return Me.Save("")
    End Function
    Public Function Save(ByVal xmlData As String) As FileRecord
        Dim cn As New SqlClient.SqlConnection(ConnectionString)

        Dim retVal As New FileRecord

        Try
            Dim xDoc As New XmlDocument
            If xmlData = "" Then
                xDoc.LoadXml(Me.SerializeToXml)
            Else xDoc.LoadXml(xmlData)
            End If

            Dim sql As String = ""
            If Me.ID = 0 Then
                sql = "INSERT INTO [FileRecords] ([Active], [CaseNumber], [RecordData], [RecordNumber], [FileName], [RecordType], [LayoutVersion], [Source], [Defendant], [DefendantAttorney], [Notes], [Plaintiff], [PlaintiffAttorney], [CaseType], [IssuesOrEvents], [DaysSinceFiling], [LengthOfTrial], [CourtRoomClerk], [Prosecutor], [Continuances], [JailStatus], [LidNumber], [CMPL], [Bond], [TrueBillOfIndictment], [BondOver], [DateAppealed], [Charges], [AssistantDA], [AKA], [IndictDate], [Set], [County], [State], [SessionDate], [CourtRoomNumber], [CourtRoomLocation], [PresidingJudge], [dtInserted], [dtUpdated], [insertedBy], [updatedBy]) VALUES (@Active, @CaseNumber, @RecordData, @RecordNumber, @FileName, @RecordType, @LayoutVersion, @Source, @Defendant, @DefendantAttorney, @Notes, @Plaintiff, @PlaintiffAttorney, @CaseType, @IssuesOrEvents, @DaysSinceFiling, @LengthOfTrial, @CourtRoomClerk, @Prosecutor, @Continuances, @JailStatus, @LidNumber, @CMPL, @Bond, @TrueBillOfIndictment, @BondOver, @DateAppealed, @Charges, @AssistantDA, @AKA, @IndictDate, @Set, @County, @State, @SessionDate, @CourtRoomNumber, @CourtRoomLocation, @PresidingJudge, @dtInserted, @dtUpdated, @insertedBy, @updatedBy);SELECT @@Identity AS [SCOPEIDENTITY];"
            Else
                sql = "UPDATE [FileRecords] SET [Active] = @Active, [CaseNumber] = @CaseNumber, [RecordData] = @RecordData, [RecordNumber] = @RecordNumber, [FileName] = @FileName, [RecordType] = @RecordType, [LayoutVersion] = @LayoutVersion, [Source] = @Source, [Defendant] = @Defendant, [DefendantAttorney] = @DefendantAttorney, [Notes] = @Notes, [Plaintiff] = @Plaintiff, [PlaintiffAttorney] = @PlaintiffAttorney, [CaseType] = @CaseType, [IssuesOrEvents] = @IssuesOrEvents, [DaysSinceFiling] = @DaysSinceFiling, [LengthOfTrial] = @LengthOfTrial, [CourtRoomClerk] = @CourtRoomClerk, [Prosecutor] = @Prosecutor, [Continuances] = @Continuances, [JailStatus] = @JailStatus, [LidNumber] = @LidNumber, [CMPL] = @CMPL, [Bond] = @Bond, [TrueBillOfIndictment] = @TrueBillOfIndictment, [BondOver] = @BondOver, [DateAppealed] = @DateAppealed, [Charges] = @Charges, [AssistantDA] = @AssistantDA, [AKA] = @AKA, [IndictDate] = @IndictDate, [Set] = @Set, [County] = @County, [State] = @State, [SessionDate] = @SessionDate, [CourtRoomNumber] = @CourtRoomNumber, [CourtRoomLocation] = @CourtRoomLocation, [PresidingJudge] = @PresidingJudge, [dtUpdated] = @dtUpdated, [updatedBy] = @updatedBy WHERE ID = @ID;"
            End If
            Dim cmd As New SqlClient.SqlCommand(sql, cn)
            cmd.Parameters.AddWithValue("@Active", True)
            cmd.Parameters.AddWithValue("@CaseNumber", If(Me.CaseNumber Is Nothing, "", Me.CaseNumber.FixCaseNumber))
            cmd.Parameters.AddWithValue("@RecordData", xDoc.Item("FileRecord").Item("RecordData").InnerXml)
            cmd.Parameters.AddWithValue("@RecordNumber", Me.RecordNumber)
            cmd.Parameters.AddWithValue("@FileName", If(Me.FileName Is Nothing, "", Me.FileName))
            cmd.Parameters.AddWithValue("@RecordType", If(Me.RecordType Is Nothing, "", Me.RecordType))
            cmd.Parameters.AddWithValue("@LayoutVersion", Me.LayoutVersion)
            cmd.Parameters.AddWithValue("@Source", CInt(Me.Source))
            cmd.Parameters.AddWithValue("@Defendant", xDoc.Item("FileRecord").Item("Defendant").InnerXml)
            cmd.Parameters.AddWithValue("@DefendantAttorney", xDoc.Item("FileRecord").Item("DefendantAttorney").InnerXml)
            cmd.Parameters.AddWithValue("@Notes", xDoc.Item("FileRecord").Item("Notes").InnerXml)
            cmd.Parameters.AddWithValue("@Plaintiff", xDoc.Item("FileRecord").Item("Plaintiff").InnerXml)
            cmd.Parameters.AddWithValue("@PlaintiffAttorney", xDoc.Item("FileRecord").Item("PlaintiffAttorney").InnerXml)
            cmd.Parameters.AddWithValue("@CaseType", If(Me.CaseType Is Nothing, "", Me.CaseType))
            cmd.Parameters.AddWithValue("@IssuesOrEvents", xDoc.Item("FileRecord").Item("IssuesOrEvents").InnerXml)
            cmd.Parameters.AddWithValue("@DaysSinceFiling", Me.DaysSinceFiling)
            cmd.Parameters.AddWithValue("@LengthOfTrial", If(Me.LengthOfTrial Is Nothing, "", Me.LengthOfTrial))
            cmd.Parameters.AddWithValue("@CourtRoomClerk", If(Me.CourtRoomClerk Is Nothing, "", Me.CourtRoomClerk))
            cmd.Parameters.AddWithValue("@Prosecutor", xDoc.Item("FileRecord").Item("Prosecutor").InnerXml)
            cmd.Parameters.AddWithValue("@Continuances", If(Me.Continuances Is Nothing, "", Me.Continuances))
            cmd.Parameters.AddWithValue("@JailStatus", If(Me.JailStatus Is Nothing, "", Me.JailStatus))
            cmd.Parameters.AddWithValue("@LidNumber", If(Me.LidNumber Is Nothing, "", Me.LidNumber))
            cmd.Parameters.AddWithValue("@CMPL", If(Me.CMPL Is Nothing, "", Me.CMPL))
            cmd.Parameters.AddWithValue("@Bond", If(Me.Bond Is Nothing, "", Me.Bond))
            cmd.Parameters.AddWithValue("@TrueBillOfIndictment", If(Me.TrueBillOfIndictment Is Nothing, "", Me.TrueBillOfIndictment))
            cmd.Parameters.AddWithValue("@BondOver", If(Me.BondOver Is Nothing, "", Me.BondOver))
            cmd.Parameters.AddWithValue("@DateAppealed", If(Me.DateAppealed Is Nothing, "", Me.DateAppealed))
            cmd.Parameters.AddWithValue("@Charges", xDoc.Item("FileRecord").Item("Charges").InnerXml)
            cmd.Parameters.AddWithValue("@AssistantDA", xDoc.Item("FileRecord").Item("AssistantDA").InnerText)
            cmd.Parameters.AddWithValue("@AKA", If(Me.AKA Is Nothing, "", Me.AKA))
            cmd.Parameters.AddWithValue("@IndictDate", If(Me.IndictDate = New Date, New Date(1900, 1, 1), Me.IndictDate))
            cmd.Parameters.AddWithValue("@Set", If(Me.Set Is Nothing, "", Me.Set))
            cmd.Parameters.AddWithValue("@County", If(Me.County Is Nothing, "", Me.County))
            cmd.Parameters.AddWithValue("@State", If(Me.State Is Nothing, "", Me.State))
            cmd.Parameters.AddWithValue("@SessionDate", Me.SessionDate)
            cmd.Parameters.AddWithValue("@CourtRoomNumber", If(Me.CourtRoomNumber Is Nothing, "", Me.CourtRoomNumber))
            cmd.Parameters.AddWithValue("@CourtRoomLocation", If(Me.CourtRoomLocation Is Nothing, "", Me.CourtRoomLocation))
            cmd.Parameters.AddWithValue("@PresidingJudge", If(Me.PresidingJudge Is Nothing, "", Me.PresidingJudge))

            If Me.ID = 0 Then
                Me.Active = True

                cmd.Parameters.AddWithValue("@dtInserted", Now.ToString)
                cmd.Parameters.AddWithValue("@dtUpdated", Now.ToString)
                cmd.Parameters.AddWithValue("@insertedBy", 0)
                cmd.Parameters.AddWithValue("@updatedBy", 0)

                If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
                Dim rs As SqlClient.SqlDataReader = cmd.ExecuteReader
                If rs.Read Then
                    Me.ID = rs("SCOPEIDENTITY").ToString.ToInteger
                End If
                cmd.Cancel()
                rs.Close()
            Else
                cmd.Parameters.AddWithValue("@dtUpdated", Now.ToString)
                cmd.Parameters.AddWithValue("@updatedBy", 0)
                cmd.Parameters.AddWithValue("@ID", Me.ID)

                If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
                cmd.ExecuteNonQuery()
                cmd.Cancel()
            End If

            retVal = Me

        Catch ex As Exception
            retVal = New FileRecord
            ex.WriteToErrorLog(New ErrorLogEntry(Enums.ProjectName.CommonCoreShared))
        Finally
            cn.Close()
        End Try

        Return retVal
    End Function

    Public Function Delete() As Boolean
        Dim cn As New SqlClient.SqlConnection(ConnectionString)

        Dim retVal As Boolean = True

        Try
            Me.Active = False

            Dim cmd As New SqlClient.SqlCommand("UPDATE [FileRecords] SET [xmlData] = @xmlData, [Active] = 0, [dtUpdated] = '" & Now.ToString & "', [updatedBy] = " & Me.ID & " WHERE [ID] = " & Me.ID, cn)
            cmd.Parameters.AddWithValue("@xmlData", Me.SerializeToXml)
            If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
            cmd.ExecuteNonQuery()

        Catch ex As Exception
            retVal = False
            ex.WriteToErrorLog(New ErrorLogEntry(Enums.ProjectName.CommonCoreShared))
        Finally
            cn.Close()
        End Try

        Return retVal
    End Function

#End Region

End Class

Public Structure CriminalCharge
    Public Property Complainant As List(Of String)
    Public Property ComplainantType As List(Of String)
    Public Property Plea As String
    Public Property Verdict As String ' VER
    Public Property OffenseClass As String ' CLS
    Public Property Points As String ' P
    Public Property LevelOfPunishment As String ' L
    Public Property Judgment As String
    Public Property OffenseType As String ' (M)isdemeanor, (F)elony, (T)raffic or (I)nfraction
    Public Property OffenseText As String
    Public Property ADA As String
    Public Property DomesticViolence As Boolean ' DOM VL
End Structure

Public Class AttorneyRecord
    Sub New()
        Me.ID = 0
    End Sub
    Sub New(ByVal id As Integer)
        Dim cn As New SqlClient.SqlConnection(ConnectionString)

        Try
            Dim cmd As New SqlClient.SqlCommand("SELECT [ID], [xmlData] FROM [Attorneys] WHERE [ID] = " & id & ";", cn)
            If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
            Dim rs As SqlClient.SqlDataReader = cmd.ExecuteReader
            If rs.Read Then
                Me.Initialize(rs("xmlData").ToString)
                Me.ID = rs("id").ToString.ToInteger
            End If
            cmd.Cancel()
            rs.Close()

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(Enums.ProjectName.CommonCoreShared))
        Finally
            cn.Close()
        End Try
    End Sub
    Sub New(ByVal barNumber As String)
        Dim cn As New SqlClient.SqlConnection(ConnectionString)

        Try
            Dim cmd As New SqlClient.SqlCommand("SELECT [ID], [xmlData] FROM [Attorneys] WHERE [xBarNumber] LIKE '" & barNumber & "';", cn)
            If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
            Dim rs As SqlClient.SqlDataReader = cmd.ExecuteReader
            If rs.Read Then
                Me.Initialize(rs("xmlData").ToString)
                Me.ID = rs("id").ToString.ToInteger
            End If
            cmd.Cancel()
            rs.Close()

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(Enums.ProjectName.CommonCoreShared))
        Finally
            cn.Close()
        End Try
    End Sub

    Public Property ID As Integer
    Public Property Active As Boolean

    Public Property BarNumber As String
    Public Property Name As String
    Public Property NameFromFile As String
    Public Property AKA As List(Of String)
    Public Property Address1 As String
    Public Property Address2 As String
    Public Property Address3 As String
    Public Property City As String
    Public Property State As String
    Public Property ZipCode As String
    Public Property WorkPhone As String
    Public Property Fax As String
    Public Property Email As String
    Public Property LicenseDate As Date
    Public Property Status As String
    Public Property MobileNumber As String

#Region "Workers"

    Public Sub Initialize(ByVal xmlData As String)
        Try
            Dim typeB As Type = Me.[GetType]()

            Dim source As New AttorneyRecord
            source = CType(source.DeserializeFromXml(xmlData), AttorneyRecord)

            ' handle the properties
            For Each [property] As PropertyInfo In source.[GetType]().GetProperties()
                If Not [property].CanRead OrElse ([property].GetIndexParameters().Length > 0) Then
                    Continue For
                End If

                Dim other As PropertyInfo = typeB.GetProperty([property].Name)
                If (other IsNot Nothing) AndAlso (other.CanWrite) Then
                    other.SetValue(Me, [property].GetValue(source, Nothing), Nothing)
                End If
            Next

            ' handle the fields
            For Each [field] As FieldInfo In source.[GetType]().GetFields()
                Dim other As FieldInfo = typeB.GetField([field].Name)
                If (other IsNot Nothing) Then
                    other.SetValue(Me, [field].GetValue(source))
                End If
            Next

        Catch ex As Exception
            Dim s As String = ""
        End Try
    End Sub

    Public Function Save() As AttorneyRecord
        Return Me.Save("")
    End Function
    Public Function Save(ByVal xmlData As String) As AttorneyRecord
        Dim cn As New SqlClient.SqlConnection(ConnectionString)

        Dim retVal As New AttorneyRecord

        Try
            If Me.ID = 0 Then
                Me.Active = True

                Dim cmd As New SqlClient.SqlCommand("INSERT INTO [Attorneys] ([xmlData], [dtInserted], [dtUpdated], [insertedBy], [updatedBy], [Active]) VALUES (@xmlData, '" & Now.ToString & "', '" & Now.ToString & "', '" & Me.ID & "', '" & Me.ID & "', '1');SELECT @@Identity AS SCOPEIDENTITY;", cn)
                If xmlData = "" Then
                    cmd.Parameters.AddWithValue("@xmlData", Me.SerializeToXml)
                Else cmd.Parameters.AddWithValue("@xmlData", xmlData)
                End If
                If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
                Dim rs As SqlClient.SqlDataReader = cmd.ExecuteReader
                If rs.Read Then
                    Me.ID = rs("SCOPEIDENTITY").ToString.ToInteger
                End If
                rs.Close()
                cmd.Cancel()
            Else
                Dim cmd As New SqlClient.SqlCommand("UPDATE [Attorneys] SET [xmlData] = @xmlData, [dtUpdated] = '" & Now.ToString & "', [updatedBy] = " & Me.ID & " WHERE [ID] = " & Me.ID, cn)
                If xmlData = "" Then
                    cmd.Parameters.AddWithValue("@xmlData", Me.SerializeToXml)
                Else : cmd.Parameters.AddWithValue("@xmlData", xmlData)
                End If
                If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
                cmd.ExecuteNonQuery()
            End If

            retVal = Me

        Catch ex As Exception
            retVal = New AttorneyRecord
            ex.WriteToErrorLog(New ErrorLogEntry(Enums.ProjectName.CommonCoreShared))
        Finally
            cn.Close()
        End Try

        Return retVal
    End Function

    Public Function Delete() As Boolean
        Dim cn As New SqlClient.SqlConnection(ConnectionString)

        Dim retVal As Boolean = True

        Try
            Me.Active = False

            Dim cmd As New SqlClient.SqlCommand("UPDATE [Attorneys] SET [xmlData] = @xmlData, [Active] = 0, [dtUpdated] = '" & Now.ToString & "', [updatedBy] = " & Me.ID & " WHERE [ID] = " & Me.ID, cn)
            cmd.Parameters.AddWithValue("@xmlData", Me.SerializeToXml)
            If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
            cmd.ExecuteNonQuery()

        Catch ex As Exception
            retVal = False
            ex.WriteToErrorLog(New ErrorLogEntry(Enums.ProjectName.CommonCoreShared))
        Finally
            cn.Close()
        End Try

        Return retVal
    End Function

#End Region

End Class

Public Class ClientRecord
    Sub New()
        Me.ID = 0
    End Sub
    Sub New(ByVal id As Integer)
        Dim cn As New SqlClient.SqlConnection(ConnectionString)

        Try
            Dim cmd As New SqlClient.SqlCommand("SELECT [ID], [xmlData] FROM [ClientRecords] WHERE [ID] = " & id & ";", cn)
            If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
            Dim rs As SqlClient.SqlDataReader = cmd.ExecuteReader
            If rs.Read Then
                Me.Initialize(rs("xmlData").ToString)
                Me.ID = rs("ID").ToString.ToInteger
            End If
            cmd.Cancel()
            rs.Close()

            If Me.ID = 0 Then
                Me.NameFromFile = Name
                Me.Name = FormatName(Name)
            End If

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(0, Me.ID, Enums.ProjectName.CommonCoreShared))
        Finally
            cn.Close()
        End Try
    End Sub
    Sub New(ByVal name As String, ByVal attorneyID As Integer)
        Dim cn As New SqlClient.SqlConnection(ConnectionString)

        Try
            Dim cmd As New SqlClient.SqlCommand("SELECT [ID], [xmlData] FROM [ClientRecords] WHERE [xNameFromFile] LIKE '" & name & "' AND [xAttorneyID] = " & attorneyID & ";", cn)
            If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
            Dim rs As SqlClient.SqlDataReader = cmd.ExecuteReader
            If rs.Read Then
                Me.Initialize(rs("xmlData").ToString)
                Me.ID = rs("ID").ToString.ToInteger
            End If
            cmd.Cancel()
            rs.Close()

            If Me.ID = 0 Then
                Me.NameFromFile = name
                Me.Name = FormatName(name)
            End If

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(0, Me.ID, Enums.ProjectName.CommonCoreShared))
        Finally
            cn.Close()
        End Try
    End Sub

    Public Property ID As Integer
    Public Property Active As Boolean

    Public Property Name As String
    Public Property NameFromFile As String
    Public Property MobileNumber As String
    Public Property Address1 As String
    Public Property Address2 As String
    Public Property City As String
    Public Property State As String
    Public Property ZipCode As String
    Public Property Email As String
    Public Property TextAlerts As Boolean
    Public Property AttorneyID As Integer

#Region "Workers"

    Public Sub Initialize(ByVal xmlData As String)
        Try
            Dim typeB As Type = Me.[GetType]()

            Dim source As New ClientRecord
            source = CType(source.DeserializeFromXml(xmlData), ClientRecord)

            ' handle the properties
            For Each [property] As PropertyInfo In source.[GetType]().GetProperties()
                If Not [property].CanRead OrElse ([property].GetIndexParameters().Length > 0) Then
                    Continue For
                End If

                Dim other As PropertyInfo = typeB.GetProperty([property].Name)
                If (other IsNot Nothing) AndAlso (other.CanWrite) Then
                    other.SetValue(Me, [property].GetValue(source, Nothing), Nothing)
                End If
            Next

            ' handle the fields
            For Each [field] As FieldInfo In source.[GetType]().GetFields()
                Dim other As FieldInfo = typeB.GetField([field].Name)
                If (other IsNot Nothing) Then
                    other.SetValue(Me, [field].GetValue(source))
                End If
            Next

        Catch ex As Exception
            Dim s As String = ""
        End Try
    End Sub

    Public Function Save() As ClientRecord
        Return Me.Save("")
    End Function
    Public Function Save(ByVal xmlData As String) As ClientRecord
        Dim cn As New SqlClient.SqlConnection(ConnectionString)

        Dim retVal As New ClientRecord

        Try
            If Me.ID = 0 Then
                Me.Active = True

                Dim cmd As New SqlClient.SqlCommand("INSERT INTO [ClientRecords] ([xmlData], [dtInserted], [dtUpdated], [insertedBy], [updatedBy], [Active]) VALUES (@xmlData, '" & Now.ToString & "', '" & Now.ToString & "', '" & Me.ID & "', '" & Me.ID & "', '1');SELECT @@Identity AS SCOPEIDENTITY;", cn)
                If xmlData = "" Then
                    cmd.Parameters.AddWithValue("@xmlData", Me.SerializeToXml)
                Else cmd.Parameters.AddWithValue("@xmlData", xmlData)
                End If
                If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
                Dim rs As SqlClient.SqlDataReader = cmd.ExecuteReader
                If rs.Read Then
                    Me.ID = rs("SCOPEIDENTITY").ToString.ToInteger
                End If
                rs.Close()
                cmd.Cancel()
            Else
                Dim cmd As New SqlClient.SqlCommand("UPDATE [ClientRecords] SET [xmlData] = @xmlData, [dtUpdated] = '" & Now.ToString & "', [updatedBy] = " & Me.ID & " WHERE [ID] = " & Me.ID, cn)
                If xmlData = "" Then
                    cmd.Parameters.AddWithValue("@xmlData", Me.SerializeToXml)
                Else : cmd.Parameters.AddWithValue("@xmlData", xmlData)
                End If
                If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
                cmd.ExecuteNonQuery()
            End If

            retVal = Me

        Catch ex As Exception
            retVal = New ClientRecord
            ex.WriteToErrorLog(New ErrorLogEntry(0, Me.ID, Enums.ProjectName.CommonCoreShared))
        Finally
            cn.Close()
        End Try

        Return retVal
    End Function

    Public Function Delete() As Boolean
        Dim cn As New SqlClient.SqlConnection(ConnectionString)

        Dim retVal As Boolean = True

        Try
            Me.Active = False

            Dim cmd As New SqlClient.SqlCommand("UPDATE [ClientRecords] SET [xmlData] = @xmlData, [Active] = 0, [dtUpdated] = '" & Now.ToString & "', [updatedBy] = " & Me.ID & " WHERE [ID] = " & Me.ID, cn)
            cmd.Parameters.AddWithValue("@xmlData", Me.SerializeToXml)
            If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
            cmd.ExecuteNonQuery()

        Catch ex As Exception
            retVal = False
            ex.WriteToErrorLog(New ErrorLogEntry(0, Me.ID, Enums.ProjectName.CommonCoreShared))
        Finally
            cn.Close()
        End Try

        Return retVal
    End Function

#End Region

End Class

Public Structure NameParts
    Public FirstName As String
    Public MiddleName As String
    Public LastName As String
    Public Extra As String
End Structure

Public Class MyAppointment
    Inherits Appointment

    Public Property XmlData As String
    Public Property GroupDate As Date
    Public Property County As String
    Public Property Type As String
End Class

Public Structure NotificationMessage
    Sub New(ByVal attorneyID As Integer, ByVal message As String)
        Me.AttorneyId = attorneyID
        Me.ClientId = 0
        Me.Message = message
        Me.Type = Enums.NotificationRecipientType.Attorney
        Me.ToNumber = ""
        Me.ToEmail = ""
        Me.MessageType = NotificationMessageType.SMS
    End Sub
    Sub New(ByVal clientId As Integer, ByVal toNumber As String, ByVal message As String)
        Me.AttorneyId = 0
        Me.ClientId = clientId
        Me.Message = message
        Me.Type = Enums.NotificationRecipientType.Client
        Me.ToNumber = toNumber
        Me.ToEmail = ""
        Me.MessageType = NotificationMessageType.SMS
    End Sub

    Public AttorneyId As Integer
    Public ClientId As Integer
    Public ToNumber As String
    Public Message As String
    Public [Type] As Enums.NotificationRecipientType
    Public CaseNumber As String
    Public BatchRunDate As Date
    Public ToEmail As String
    Public MessageType As Enums.NotificationMessageType
End Structure




#Region "System Objects"


Public Structure ErrorLogEntry
    Sub New(ByVal userId As Integer, ByVal clientId As Integer, ByVal project As Enums.ProjectName)
        Me.userId = userId
        Me.clientId = clientId
        Me.project = project
    End Sub
    Sub New(ByVal project As Enums.ProjectName)
        Me.userId = 0
        Me.clientId = 0
        Me.project = project
    End Sub

    Public userId As Integer
    Public clientId As Integer
    Public project As Enums.ProjectName
End Structure

Public Class SystemUser
    Public Sub New()
        Me.ID = 0
        Me.MobileDeviceIds = New List(Of String)
        Me.MobileNumbers = New List(Of String)
        Me.SalesMan = SalesMan.Web
        Me.NotificationType = NotificationType.SMS
        Me.NotificationLevel = NotificationLevel.Summary
        Me.MaxNotificationsPerBatch = 1
    End Sub
    Public Sub New(ByVal userEmail As String, ByVal userPassword As String, ByVal apiKey As String)
        Dim cn As New SqlClient.SqlConnection(ConnectionString)

        Try
            Dim cl As New SystemClient
            Dim cmd As New SqlClient.SqlCommand()
            Dim rs As SqlClient.SqlDataReader

            Dim mobileUserFound As Boolean = False

            If apiKey = "" Then
                ' check login based on xMobileUsername and xPassword first
                cmd = New SqlClient.SqlCommand("SELECT [ID], [UserXmlData], [ClientXmlData] FROM [vwUserInfo] WHERE [xMobileUsername] LIKE @Email AND [xPassword] LIKE @Password AND [xActive] = 1;", cn)
                cmd.Parameters.AddWithValue("@Email", userEmail)
                cmd.Parameters.AddWithValue("@Password", userPassword)
                If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
                rs = cmd.ExecuteReader
                If rs.Read Then
                    Me.Initialize(rs("UserXmlData").ToString)
                    Me.ID = rs("id").ToString.ToInteger
                    cl.Initialize(rs("ClientXmlData").ToString)

                    mobileUserFound = True
                End If
                rs.Close()
                cmd.Cancel()

                If Not mobileUserFound Then
                    ' no mobile user found so check login based on xEmail and xPassword
                    cmd = New SqlClient.SqlCommand("SELECT [ID], [UserXmlData], [ClientXmlData] FROM [vwUserInfo] WHERE [xEmail] LIKE @Email AND [xPassword] LIKE @Password AND [xActive] = 1;", cn)
                    cmd.Parameters.AddWithValue("@Email", userEmail)
                    cmd.Parameters.AddWithValue("@Password", userPassword)
                    If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
                    rs = cmd.ExecuteReader
                    If rs.Read Then
                        Me.Initialize(rs("UserXmlData").ToString)
                        Me.ID = rs("id").ToString.ToInteger
                        cl.Initialize(rs("ClientXmlData").ToString)
                    End If
                    rs.Close()
                    cmd.Cancel()
                End If
            Else
                ' we have an apiKey so lookup the user based on that
                cmd = New SqlClient.SqlCommand("SELECT [ID], [UserXmlData], [ClientXmlData], [xEmail], [xPassword] FROM [vwUserInfo] WHERE [xApiEnabled] = 1 AND [xActive] = 1;", cn)
                If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
                rs = cmd.ExecuteReader
                Do While rs.Read
                    Dim convertedToBytes As Byte() = Encoding.UTF8.GetBytes(rs("xPassword").ToString & rs("xEmail").ToString)
                    Dim hashType As HashAlgorithm = New SHA512Managed()
                    Dim hashBytes As Byte() = hashType.ComputeHash(convertedToBytes)
                    Dim hashedResult As String = Convert.ToBase64String(hashBytes)

                    If apiKey = hashedResult Then
                        Me.Initialize(rs("UserXmlData").ToString)
                        Me.ID = rs("id").ToString.ToInteger
                        cl.Initialize(rs("ClientXmlData").ToString)
                        Exit Do
                    End If
                Loop
                rs.Close()
                cmd.Cancel()
            End If

            If Me.ID > 0 Then
                If Me.MobileNumbers.Count = 0 Then Me.NotificationType = Enums.NotificationType.Email

                If cl.Approved = SystemMode.Demo And DateDiff(DateInterval.Day, cl.DemoStartDate, Now.Date) > cl.DemoDuration Then
                    Me.APIResponseCode = Enums.ApiResultCode.failed
                    Me.APIResponseMessage = "Your system demo period has expired."
                ElseIf Not cl.Active Then
                    Me.APIResponseCode = Enums.ApiResultCode.failed
                    Me.APIResponseMessage = "Your client profile is currently disabled."
                Else
                    Me.APIResponseCode = Enums.ApiResultCode.success
                    Me.APIResponseMessage = ""
                End If
            End If

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(Me.ID, Me.ClientID, Enums.ProjectName.CommonCoreShared))
        Finally
            cn.Close()
        End Try
    End Sub
    Public Sub New(ByVal deviceId As String)
        Dim cn As New SqlClient.SqlConnection(ConnectionString)

        Try
            Dim cl As New SystemClient

            ' pull the last user that logged in with this device id
            Dim cmd As New SqlClient.SqlCommand("SELECT [ID], [UserXmlData], [ClientXmlData] FROM [vwUserInfo] WHERE [UserXmlData].exist('/SystemUser/MobileDeviceIds [contains(.,""" & deviceId & """)]') = 1 AND [ID] > 1 ORDER BY [dtUpdated] DESC;", cn)
            If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
            Dim rs As SqlClient.SqlDataReader = cmd.ExecuteReader
            If rs.Read Then
                Me.Initialize(rs("UserXmlData").ToString)

                ' save the device id to the list
                If Not Me.MobileDeviceIds.Contains(deviceId) Then Me.MobileDeviceIds.Add(deviceId)

                'save any changes
                Me.Save()

                Me.ID = rs("id").ToString.ToInteger
                cl.Initialize(rs("ClientXmlData").ToString)
            End If
            rs.Close()
            cmd.Cancel()

            If Me.ID > 0 Then
                If Me.MobileNumbers.Count = 0 Then Me.NotificationType = Enums.NotificationType.Email

                If cl.Approved = SystemMode.Demo And DateDiff(DateInterval.Day, cl.DemoStartDate, Now.Date) > cl.DemoDuration Then
                    Me.APIResponseCode = Enums.ApiResultCode.failed
                    Me.APIResponseMessage = "Your system demo period has expired."
                ElseIf Not cl.Active Then
                    Me.APIResponseCode = Enums.ApiResultCode.failed
                    Me.APIResponseMessage = "Your client profile is currently disabled."
                Else
                    Me.APIResponseCode = Enums.ApiResultCode.success
                    Me.APIResponseMessage = ""
                End If
            End If

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(Me.ID, Me.ClientID, Enums.ProjectName.CommonCoreShared))
        Finally
            cn.Close()
        End Try
    End Sub
    Public Sub New(ByVal userId As Integer)
        Dim cn As New SqlClient.SqlConnection(ConnectionString)

        Try
            Dim cl As New SystemClient

            Dim cmd As New SqlClient.SqlCommand("SELECT [ID], [UserXmlData], [ClientXmlData] FROM [vwUserInfo] WHERE [ID] = " & userId & " AND [xActive] = 1;", cn)
            If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
            Dim rs As SqlClient.SqlDataReader = cmd.ExecuteReader
            If rs.Read Then
                Me.Initialize(rs("UserXmlData").ToString)
                Me.ID = rs("id").ToString.ToInteger
                cl.Initialize(rs("ClientXmlData").ToString)
            End If
            rs.Close()
            cmd.Cancel()

            If Me.ID > 0 Then
                If Me.MobileNumbers.Count = 0 Then Me.NotificationType = Enums.NotificationType.Email

                If cl.Approved = SystemMode.Demo And DateDiff(DateInterval.Day, cl.DemoStartDate, Now.Date) > cl.DemoDuration Then
                    Me.APIResponseCode = Enums.ApiResultCode.failed
                    Me.APIResponseMessage = "Your system demo period has expired."
                ElseIf Not cl.Active Then
                    Me.APIResponseCode = Enums.ApiResultCode.failed
                    Me.APIResponseMessage = "Your client profile is currently disabled."
                Else
                    Me.APIResponseCode = Enums.ApiResultCode.success
                    Me.APIResponseMessage = ""
                End If
            End If

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(Me.ID, Me.ClientID, Enums.ProjectName.CommonCoreShared))
        Finally
            cn.Close()
        End Try
    End Sub

    Public ID As Integer
    Public Name As String
    Public Email As String
    Public Password As String
    Public Permissions As SystemUserPermissions
    Public SalesMan As SalesMan
    Public LastPaidDate As Date
    Public StripeCustomer As String
    Public NotificationType As NotificationType
    Public NotificationLevel As NotificationLevel
    Public MaxNotificationsPerBatch As Integer
    Public ReadOnly Property Hash As String
        Get
            Dim retVal As String = ""

            Dim convertedToBytes As Byte() = Encoding.UTF8.GetBytes(Me.Password & Me.Email)
            Dim hashType As HashAlgorithm = New SHA512Managed()
            Dim hashBytes As Byte() = hashType.ComputeHash(convertedToBytes)
            retVal = Convert.ToBase64String(hashBytes)

            Return retVal
        End Get
    End Property
    Public ReadOnly Property UnpaidDays As Long
        Get
            Dim retVal As Long = DateDiff(DateInterval.Day, Me.LastPaidDate, Now.Date)
            Return retVal
        End Get
    End Property
    Public Property ImageUrl As String
        Get
            Dim retVal As String = ""

            Select Case Me.Permissions
                Case Enums.SystemUserPermissions.Administrator, Enums.SystemUserPermissions.SystemAdministrator
                    retVal = "~/images/icon_administrator_avatar.png"
                Case Enums.SystemUserPermissions.Technician
                    retVal = "~/images/icon_technician_avatar.png"
                Case Enums.SystemUserPermissions.Solvtopia
                    retVal = "~/images/icon.png"
                Case Enums.SystemUserPermissions.Supervisor
                    retVal = "~/images/icon_supervisor_avatar.png"
                Case Else
                    retVal = "~/images/icon_user_avatar.png"
            End Select

            Return retVal
        End Get
        Set(value As String)
            ' Property is ReadOnly. Set is for API Compatibility Only
        End Set
    End Property
    Public Property MobileImageUrl As String
        Get
            Dim retVal As String = Me.ImageUrl

            retVal = retVal.Replace("~/", "https://access.dailycourtdocket.com/")
            'retVal = retVal.Replace("avatar.png", "mobile_avatar.png")

            Return retVal
        End Get
        Set(value As String)
            ' Property is ReadOnly. Set is for API Compatibility Only
        End Set
    End Property
    Public MobileDeviceId As String
    Public MobileDeviceIds As List(Of String)
    Public MobileUsername As String
    Public MobileNumbers As List(Of String)
    'Public MobilePassword As String
    Public Active As Boolean
    Public ClientID As Integer
    Public ApiEnabled As Boolean
    Public WebEnabled As Boolean
    Public BillingLock As Boolean
    Public Canceled As Boolean
    Public Property IsAdminUser As Boolean
        Get
            Return (Me.Permissions = SystemUserPermissions.Administrator Or
                    Me.Permissions = SystemUserPermissions.SystemAdministrator Or
                    Me.Permissions = SystemUserPermissions.Solvtopia)
        End Get
        Set(value As Boolean)
            ' Property is ReadOnly. Set is for API Compatibility Only
        End Set
    End Property
    Public Property IsSysAdmin As Boolean
        Get
            Return (Me.Permissions = SystemUserPermissions.SystemAdministrator Or
                    Me.Permissions = SystemUserPermissions.Solvtopia)
        End Get
        Set(value As Boolean)
            ' Property is ReadOnly. Set is for API Compatibility Only
        End Set
    End Property
    Public SupervisorID As Integer
    Public MobileDeviceType As Enums.UserPlatform
    Public OneSignalUserID As String
    Public OneSignalPushToken As String

    Public APIResponseCode As ApiResultCode
    Public APIResponseMessage As String

    Public AttorneyID As Integer
    Public NameFromFile As String

    Public Property BillingName As String
    Public Property BillingAddress1 As String
    Public Property BillingAddress2 As String
    Public Property BillingCity As String
    Public Property BillingState As String
    Public Property BillingZipCode As String
    Public Property CCType As CreditCardType
    Public Property CCNumber As String
    Public Property CCCVC As String
    Public Property CCExpirationMonth As Integer
    Public Property CCExpirationYear As Integer

#Region "Workers"

    Public Sub Initialize(ByVal xmlData As String)
        Try
            Dim typeB As Type = Me.[GetType]()

            Dim source As New SystemUser
            source = CType(source.DeserializeFromXml(xmlData), SystemUser)

            ' handle the properties
            For Each [property] As PropertyInfo In source.[GetType]().GetProperties()
                If Not [property].CanRead OrElse ([property].GetIndexParameters().Length > 0) Then
                    Continue For
                End If

                Dim other As PropertyInfo = typeB.GetProperty([property].Name)
                If (other IsNot Nothing) AndAlso (other.CanWrite) Then
                    other.SetValue(Me, [property].GetValue(source, Nothing), Nothing)
                End If
            Next

            ' handle the fields
            For Each [field] As FieldInfo In source.[GetType]().GetFields()
                Dim other As FieldInfo = typeB.GetField([field].Name)
                If (other IsNot Nothing) Then
                    other.SetValue(Me, [field].GetValue(source))
                End If
            Next

        Catch ex As Exception
            Dim s As String = ""
        End Try
    End Sub

    Public Function Save() As SystemUser
        Return Me.Save("")
    End Function
    Public Function Save(ByVal xmlData As String) As SystemUser
        Dim cn As New SqlClient.SqlConnection(ConnectionString)

        Dim retVal As New SystemUser

        Try
            If Me.ID = 0 Then
                Me.Active = True

                Dim cmd As New SqlClient.SqlCommand("INSERT INTO [Users] ([xmlData], [dtInserted], [dtUpdated], [insertedBy], [updatedBy], [Active]) VALUES (@xmlData, '" & Now.ToString & "', '" & Now.ToString & "', '" & Me.ID & "', '" & Me.ID & "', '1');SELECT @@Identity AS SCOPEIDENTITY;", cn)
                If xmlData = "" Then
                    cmd.Parameters.AddWithValue("@xmlData", Me.SerializeToXml)
                Else cmd.Parameters.AddWithValue("@xmlData", xmlData)
                End If
                If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
                Dim rs As SqlClient.SqlDataReader = cmd.ExecuteReader
                If rs.Read Then
                    Me.ID = rs("SCOPEIDENTITY").ToString.ToInteger
                End If
                rs.Close()
                cmd.Cancel()
            Else
                Dim cmd As New SqlClient.SqlCommand("UPDATE [Users] SET [xmlData] = @xmlData, [dtUpdated] = '" & Now.ToString & "', [updatedBy] = " & Me.ID & " WHERE [ID] = " & Me.ID, cn)
                If xmlData = "" Then
                    cmd.Parameters.AddWithValue("@xmlData", Me.SerializeToXml)
                Else : cmd.Parameters.AddWithValue("@xmlData", xmlData)
                End If
                If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
                cmd.ExecuteNonQuery()
            End If

            retVal = Me

        Catch ex As Exception
            retVal = New SystemUser
            ex.WriteToErrorLog(New ErrorLogEntry(Me.ID, Me.ClientID, Enums.ProjectName.CommonCoreShared))
        Finally
            cn.Close()
        End Try

        Return retVal
    End Function

    Public Function Delete() As Boolean
        Dim cn As New SqlClient.SqlConnection(ConnectionString)

        Dim retVal As Boolean = True

        Try
            Me.Active = False

            Dim cmd As New SqlClient.SqlCommand("UPDATE [Users] SET [xmlData] = @xmlData, [Active] = 0, [dtUpdated] = '" & Now.ToString & "', [updatedBy] = " & Me.ID & " WHERE [ID] = " & Me.ID, cn)
            cmd.Parameters.AddWithValue("@xmlData", Me.SerializeToXml)
            If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
            cmd.ExecuteNonQuery()

        Catch ex As Exception
            retVal = False
            ex.WriteToErrorLog(New ErrorLogEntry(Me.ID, Me.ClientID, Enums.ProjectName.CommonCoreShared))
        Finally
            cn.Close()
        End Try

        Return retVal
    End Function

#End Region

End Class

Public Class SystemClient
    Public Sub New()
        Me.ID = 0
        Me.IconSize = IconSize.Small
        Me.DemoDuration = 7
    End Sub
    Public Sub New(ByVal clientId As Integer)
        Dim cn As New SqlClient.SqlConnection(ConnectionString)

        Try
            Dim cmd As New SqlClient.SqlCommand("SELECT [ID], [xmlData] FROM [Clients] WHERE [ID] = " & clientId, cn)
            If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
            Dim rs As SqlClient.SqlDataReader = cmd.ExecuteReader
            If rs.Read Then
                Me.Initialize(rs("xmlData").ToString)
                Me.ID = rs("id").ToString.ToInteger
                If Me.DemoDuration = 0 Then Me.DemoDuration = 7
            End If
            rs.Close()
            cmd.Cancel()

            If Me.Approved = SystemMode.Demo And DateDiff(DateInterval.Day, Me.DemoStartDate, Now.Date) > Me.DemoDuration Then
                Me.APIResponseCode = Enums.ApiResultCode.failed
                Me.APIResponseMessage = "Your system demo period has expired."
            ElseIf Not Me.Active Then
                Me.APIResponseCode = Enums.ApiResultCode.failed
                Me.APIResponseMessage = "Your client profile is currently disabled."
            Else
                Me.APIResponseCode = Enums.ApiResultCode.success
                Me.APIResponseMessage = ""
            End If

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(0, Me.ID, Enums.ProjectName.CommonCoreShared))
        Finally
            cn.Close()
        End Try
    End Sub

    Public ID As Integer
    Public Name As String
    Public Address1 As String
    Public Address2 As String
    Public City As String
    Public State As String
    Public ZipCode As String
    Public ContactName As String
    Public ContactPhone As String
    Public ContactEmail As String
    Public Active As Boolean
    Public Approved As Enums.SystemMode
    Public IconSize As Enums.IconSize
    Public ReadOnly Property DemoDays As Long
        Get
            Dim retval As Long = DateDiff(DateInterval.Day, Me.DemoStartDate, Now.Date)
            'If retval < 0 Then retval = Math.Abs(retval)

            Return retval
        End Get
    End Property

    Public DemoStartDate As Date
    Public DemoDuration As Integer

    Public ProjectUrl As String

    Public APIResponseCode As ApiResultCode
    Public APIResponseMessage As String

#Region "Workers"

    Public Sub Initialize(ByVal xmlData As String)
        Try
            Dim typeB As Type = Me.[GetType]()

            Dim source As New SystemClient
            source = CType(source.DeserializeFromXml(xmlData), SystemClient)

            ' handle the properties
            For Each [property] As PropertyInfo In source.[GetType]().GetProperties()
                If Not [property].CanRead OrElse ([property].GetIndexParameters().Length > 0) Then
                    Continue For
                End If

                Dim other As PropertyInfo = typeB.GetProperty([property].Name)
                If (other IsNot Nothing) AndAlso (other.CanWrite) Then
                    other.SetValue(Me, [property].GetValue(source, Nothing), Nothing)
                End If
            Next

            ' handle the fields
            For Each [field] As FieldInfo In source.[GetType]().GetFields()
                Dim other As FieldInfo = typeB.GetField([field].Name)
                If (other IsNot Nothing) Then
                    other.SetValue(Me, [field].GetValue(source))
                End If
            Next

        Catch ex As Exception
            Dim s As String = ""
        End Try
    End Sub

    Public Function Save() As SystemClient
        Return Me.Save("")
    End Function
    Public Function Save(ByVal xmlData As String) As SystemClient
        Dim cn As New SqlClient.SqlConnection(ConnectionString)

        Dim retVal As New SystemClient

        Try
            If Me.ID = 0 Then
                Me.Active = True

                Dim cmd As New SqlClient.SqlCommand("INSERT INTO [Clients] ([xmlData], [dtInserted], [dtUpdated], [insertedBy], [updatedBy]) VALUES (@xmlData, '" & Now.ToString & "', '" & Now.ToString & "', '0', '0');SELECT @@Identity AS SCOPEIDENTITY;", cn)
                If xmlData = "" Then
                    cmd.Parameters.AddWithValue("@xmlData", Me.SerializeToXml)
                Else cmd.Parameters.AddWithValue("@xmlData", xmlData)
                End If
                If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
                Dim rs As SqlClient.SqlDataReader = cmd.ExecuteReader
                If rs.Read Then
                    Me.ID = rs("SCOPEIDENTITY").ToString.ToInteger
                End If
                rs.Close()
                cmd.Cancel()
            Else
                Dim cmd As New SqlClient.SqlCommand("UPDATE [Clients] SET [xmlData] = @xmlData, [dtUpdated] = '" & Now.ToString & "', [updatedBy] = 0 WHERE [ID] = " & Me.ID, cn)
                If xmlData = "" Then
                    cmd.Parameters.AddWithValue("@xmlData", Me.SerializeToXml)
                Else cmd.Parameters.AddWithValue("@xmlData", xmlData)
                End If
                If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
                cmd.ExecuteNonQuery()
            End If

            retVal = Me

        Catch ex As Exception
            retVal = New SystemClient
            ex.WriteToErrorLog(New ErrorLogEntry(0, Me.ID, Enums.ProjectName.CommonCoreShared))
        Finally
            cn.Close()
        End Try

        Return retVal
    End Function

    Public Function Delete() As Boolean
        Dim cn As New SqlClient.SqlConnection(ConnectionString)

        Dim retVal As Boolean = True

        Try
            Me.Active = False

            Dim cmd As New SqlClient.SqlCommand("UPDATE [Clients] SET [xmlData] = @xmlData, [dtUpdated] = '" & Now.ToString & "', [updatedBy] = 0 WHERE [ID] = " & Me.ID, cn)
            cmd.Parameters.AddWithValue("@xmlData", Me.SerializeToXml)
            If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
            cmd.ExecuteNonQuery()

        Catch ex As Exception
            retVal = False
            ex.WriteToErrorLog(New ErrorLogEntry(0, Me.ID, Enums.ProjectName.CommonCoreShared))
        Finally
            cn.Close()
        End Try

        Return retVal
    End Function

#End Region

End Class

Public Class ApiKeyResult
    Sub New()
    End Sub
    Sub New(ByVal userEmail As String, ByVal userPassword As String)
        Dim cn As New SqlClient.SqlConnection(ConnectionString)

        Try
            Me.UserEmail = userEmail
            Me.UserPassword = userPassword

            Dim convertedToBytes As Byte() = Encoding.UTF8.GetBytes(userPassword & userEmail)
            Dim hashType As HashAlgorithm = New SHA512Managed()
            Dim hashBytes As Byte() = hashType.ComputeHash(convertedToBytes)
            Dim hashedResult As String = Convert.ToBase64String(hashBytes)

            Me.ApiKey = hashedResult

            Dim cmd As New SqlClient.SqlCommand("SELECT [xClientID] FROM [Users] WHERE [xEmail] LIKE @Email AND [xPassword] LIKE @Password;", cn)
            cmd.Parameters.AddWithValue("@Email", userEmail)
            cmd.Parameters.AddWithValue("@Password", userPassword)
            If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
            Dim rs As SqlClient.SqlDataReader = cmd.ExecuteReader
            If rs.Read Then
                Me.ClientID = rs("xClientID").ToString.ToInteger
            End If
            cmd.Cancel()
            rs.Close()

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(0, Me.ClientID, Enums.ProjectName.CommonCoreShared))
        Finally
            cn.Close()
        End Try
    End Sub

    Public UserEmail As String
    Public UserPassword As String
    Public ApiKey As String
    Public ClientID As Integer

    Public responseCode As Enums.ApiResultCode
    Public responseMessage As String
End Class

Public Class ApiResponse
    Sub New()
        Me.responseCode = ApiResultCode.success
        Me.responseMessage = ""
    End Sub

    Public responseCode As ApiResultCode
    Public responseMessage As String
    Public responseObject As Object
End Class

Public Class ApiRequest
    Sub New()
    End Sub
    Sub New(ByVal apiKey As String, ByVal clientID As Integer, ByVal deviceId As String, ByVal deviceType As UserPlatform)
        Me.apiKey = apiKey
        Me.deviceId = deviceId
        Me.deviceType = deviceType
    End Sub

    Public apiKey As String
    Public clientId As Integer
    Public deviceId As String
    Public deviceType As UserPlatform
End Class

Public Structure NameValuePair
    Public Sub New(ByVal n As String, ByVal v As String)
        Me.Name = n
        Me.value = v
    End Sub

    Public Name As String
    Public value As String
End Structure

Public Structure AuditLogEntry
    Public ActionType As String
    Public Description As String
    Public UserID As Integer
    Public ClientID As Integer
    Public IpAddress As String
    Public Project As Enums.ProjectName
End Structure

Public Structure MaintenanceInfo
    Public Web As Boolean
    Public Android As Boolean
    Public iOS As Boolean
    Public Property Active As Boolean
        Get
            Return Web Or Android Or iOS
        End Get
        Set(value As Boolean)
        End Set
    End Property
    Public Notes As String
End Structure

#End Region
