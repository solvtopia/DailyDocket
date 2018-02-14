Imports System.IO
Imports System.Reflection
Imports System.Runtime.CompilerServices
'Imports System.Web.UI
'Imports System.Web.UI.WebControls
Imports System.Xml
Imports System.Xml.Serialization

Public Module Xml

    Public Function NewXmlDocument() As XmlDocument
        Return NewXmlDocument("Data")
    End Function
    Public Function NewXmlDocument(ByVal rootElement As String) As XmlDocument
        Dim retVal As New XmlDocument

        Dim dec As XmlDeclaration = retVal.CreateXmlDeclaration("1.0", "utf-16", "")
        retVal.PrependChild(dec)

        Dim root As XmlElement = retVal.CreateElement(rootElement)
        retVal.AppendChild(root)

        Return retVal
    End Function

    Public Function HashtableToXml(ByVal ht As Hashtable) As String
        Dim retVal As String = ""

        Dim xDoc As New XmlDocument

        Dim dec As XmlDeclaration = xDoc.CreateXmlDeclaration("1.0", "utf-16", "")
        xDoc.PrependChild(dec)

        'add root element
        Dim quoteElement As XmlElement = xDoc.CreateElement("Data")

        If Not ht Is Nothing Then
            For Each key In ht.Keys
                quoteElement.AppendChild(NewElement(xDoc, key.ToString, ht(key).ToString))
            Next
        End If

        xDoc.AppendChild(quoteElement)

        'return just the string
        Dim sw As StringWriter = New StringWriter
        xDoc.Save(sw)

        xDoc = Nothing

        retVal = sw.ToString()

        Return retVal
    End Function

    Public Function GetXmlField(ByVal xNode As XmlNode, ByVal fieldName As String) As String
        Dim retVal As String = ""

        Try
            For Each xE As XmlElement In xNode
                If xE.Name.ToLower = fieldName.ToLower Then
                    retVal = xE.InnerText
                    Exit For
                End If
            Next

        Catch ex As Exception
            retVal = ""
        End Try

        Return retVal
    End Function

    'Public Function SerializeToXml(ByVal o As Object) As String
    '    Dim sw As New StringWriter()
    '    Dim tw As XmlTextWriter = Nothing

    '    Try
    '        Dim serializer As New XmlSerializer(o.GetType)
    '        tw = New XmlTextWriter(sw)
    '        serializer.Serialize(tw, o)

    '    Catch ex As Exception
    '    Finally
    '        sw.Close()
    '        If tw IsNot Nothing Then
    '            tw.Close()
    '        End If
    '    End Try

    '    Return sw.ToString()
    'End Function

    'Public Function DeserializeFromXML(ByVal xml As String, objectType As Type) As Object
    '    Dim strReader As StringReader = Nothing
    '    Dim serializer As XmlSerializer = Nothing
    '    Dim xmlReader As XmlTextReader = Nothing
    '    Dim obj As Object = Nothing

    '    Try
    '        strReader = New StringReader(xml)
    '        serializer = New XmlSerializer(objectType)
    '        xmlReader = New XmlTextReader(strReader)
    '        obj = serializer.Deserialize(xmlReader)

    '    Catch exp As Exception
    '    Finally
    '        If xmlReader IsNot Nothing Then
    '            xmlReader.Close()
    '        End If
    '        If strReader IsNot Nothing Then
    '            strReader.Close()
    '        End If
    '    End Try

    '    Return obj
    'End Function


    ' loops through all properties and fields of an object and returns a new XmlDocument with values
    Public Function ObjectToXml(ByVal obj As Object) As String
        Dim retVal As String = ""

        Dim myValue As String = ""
        Dim myName As String = ""
        Dim xDoc As New XmlDocument

        Dim dec As XmlDeclaration = xDoc.CreateXmlDeclaration("1.0", "utf-16", "")
        xDoc.PrependChild(dec)

        ' get the type of the object we are working with
        Dim objectType As Type = CType(obj.GetType, Type)

        'add root element
        Dim quoteElement As XmlElement = xDoc.CreateElement(objectType.ToString)

        ' get a list of properties and fields to loop through
        Dim properties As PropertyInfo() = objectType.GetProperties()
        Dim fields As FieldInfo() = objectType.GetFields()

        ' loop through properties first (this works on compiled objects)
        For Each prop As PropertyInfo In properties
            Dim value As String = ""
            Try
                If Not prop.GetValue(obj, Nothing) Is Nothing Then
                    value = CStr(prop.GetValue(obj, Nothing))
                End If

            Catch ex As Exception
                ' skip complex types
            End Try

            quoteElement.AppendChild(NewElement(xDoc, prop.Name.XmlEncode, value.XmlEncode))
        Next

        ' now loop through the fields (this works on local classes)
        For Each fld As FieldInfo In fields
            Dim value As String = ""
            Try
                If Not fld.GetValue(obj) Is Nothing Then
                    value = CStr(fld.GetValue(obj))
                End If

            Catch ex As Exception
                ' skip complex types
            End Try

            quoteElement.AppendChild(NewElement(xDoc, fld.Name.XmlEncode, value.XmlEncode))
        Next

        xDoc.AppendChild(quoteElement)

        'return just the string
        Dim sw As StringWriter = New StringWriter
        xDoc.Save(sw)

        xDoc = Nothing

        retVal = sw.ToString()

        Return retVal
    End Function

    <Extension()> Public Function NewElement(ByVal xDoc As XmlDocument, ByVal name As String) As XmlElement
        Return NewElement(xDoc, name, "")
    End Function
    <Extension()> Public Function NewElement(ByVal xDoc As XmlDocument, ByVal name As String, ByVal value As String) As XmlElement
        Return NewElement(xDoc, name, value, Nothing)
    End Function
    <Extension()> Public Function NewElement(ByVal xDoc As XmlDocument, ByVal name As String, ByVal value As String, ByVal attribute As XmlAttribute) As XmlElement
        Dim xElem As XmlElement = xDoc.CreateElement(name.XmlEncode(True))
        If value IsNot Nothing AndAlso value <> "" Then
            If value.Trim.Length > 0 Then xElem.InnerText = value.XmlEncode
        End If
        If attribute IsNot Nothing Then xElem.Attributes.Append(attribute)
        Return xElem
    End Function
    <Extension()> Public Function NewElement(ByVal xDoc As XmlDocument, ByVal name As String, ByVal value As String, ByVal type As String, ByVal selected As String, ByVal index As String, ByVal attribute As XmlAttribute) As XmlElement
        Dim xElem As XmlElement = NewElement(xDoc, name.XmlEncode(True), value.XmlEncode)
        If type <> "" Then
            Dim xAttr As XmlAttribute = xDoc.CreateAttribute("Type")
            xAttr.Value = type.XmlEncode
            xElem.Attributes.Append(xAttr)
        End If
        If selected <> "" Then
            Dim xAttr As XmlAttribute = xDoc.CreateAttribute("Selected")
            xAttr.Value = selected.XmlEncode
            xElem.Attributes.Append(xAttr)
        End If
        If index <> "" Then
            Dim xAttr As XmlAttribute = xDoc.CreateAttribute("Index")
            xAttr.Value = index.XmlEncode
            xElem.Attributes.Append(xAttr)
        End If
        If attribute IsNot Nothing Then xElem.Attributes.Append(attribute)

        Return xElem
    End Function

    <Extension()> Public Function NewAttribute(ByRef xDoc As XmlDocument, ByVal name As String, ByVal value As String) As XmlAttribute
        Dim retVal As XmlAttribute = xDoc.CreateAttribute(name)
        retVal.Value = value

        Return retVal
    End Function

    <Extension()> Public Function XmlEncode(ByVal str As String, Optional ByVal deep As Boolean = False) As String
        Dim sNew As String = str

        sNew = Replace(sNew, "<", "&lt;")
        sNew = Replace(sNew, ">", "&gt;")
        sNew = Replace(sNew, "&", "&amp;")
        sNew = Replace(sNew, """", "&quot;")
        sNew = Replace(sNew, "'", "&apos;")
        If deep Then
            sNew = Replace(sNew, " ", "_nbsp;")
            sNew = Replace(sNew, "$", "_cur;")
            sNew = Replace(sNew, "/", "_fsl;")
            sNew = Replace(sNew, "\", "_bksl;")
            sNew = Replace(sNew, ":", "_colon;")
            sNew = Replace(sNew, "!", "_bang;")
            sNew = Replace(sNew, "=", "_eq;")
            sNew = Replace(sNew, "?", "_quest;")
            sNew = Replace(sNew, ";", "_scolon")
        End If

        Return sNew
    End Function

    <Extension()> Public Function XmlDecode(ByVal str As String, Optional ByVal deep As Boolean = False) As String
        Dim sNew As String = str

        ' we call this so many times to make sure we have all the ampersands
        sNew = Replace(sNew, "&amp;", "&")
        sNew = Replace(sNew, "&amp;", "&")
        sNew = Replace(sNew, "&amp;", "&")
        sNew = Replace(sNew, "&amp;", "&")

        sNew = Replace(sNew, "&lt;", "<")
        sNew = Replace(sNew, "&gt;", ">")
        sNew = Replace(sNew, "&quot;", """")
        sNew = Replace(sNew, "&apos;", "'")
        If deep Then
            sNew = Replace(sNew, "_nbsp;", " ")
            sNew = Replace(sNew, "_cur;", "$")
            sNew = Replace(sNew, "_fsl;", "/")
            sNew = Replace(sNew, "_bksl;", "\")
            sNew = Replace(sNew, "_colon;", ":")
            sNew = Replace(sNew, "_bang;", "!")
            sNew = Replace(sNew, "_eq;", "=")
            sNew = Replace(sNew, "_quest;", "?")
            sNew = Replace(sNew, "_scolon", ";")
        End If

        Return sNew
    End Function

    '<Extension()> Public Function ToXml(ByVal str As String) As XmlDocument
    '    Try
    '        Dim xDoc As New XmlDocument
    '        If Not IsDBNull(str) Then xDoc.LoadXml(str)

    '        Return xDoc

    '    Catch ex As Exception
    '        ex.WriteToErrorLog(Enums.ProjectName.CommonCoreShared)
    '        Return New XmlDocument
    '    End Try
    'End Function
    '<Extension()> Public Function ToXml(ByVal ht As Hashtable) As XmlDocument
    '    Try
    '        Dim xDoc As New XmlDocument
    '        Dim dec As XmlDeclaration = xDoc.CreateXmlDeclaration("1.0", "utf-16", "")
    '        xDoc.PrependChild(dec)

    '        xDoc.AppendChild(xDoc.FromHashtable(ht, "Data"))

    '        Return xDoc

    '    Catch ex As Exception
    '        ex.WriteToErrorLog(Enums.ProjectName.CommonCoreShared)(0, "SDK")
    '        Return Nothing
    '    End Try
    'End Function
    '<Extension()> Public Function ToXml(ByVal ctrl As Control) As XmlDocument
    '    Return ToXml(ctrl, OriginalXml:=Nothing)
    'End Function
    '<Extension()> Public Function ToXml(ByVal ctrl As Control, ByVal OriginalXml As XmlDocument) As XmlDocument
    '    Dim htNew As Hashtable = ctrl.ToHashtable("root")

    '    Try
    '        ' get a list of all the xml sections from the custom controls
    '        Dim paths As List(Of String) = GetXmlPaths(ctrl)

    '        If Not OriginalXml Is Nothing Then
    '            ' convert the original string to a hashtable
    '            Dim htOriginal As Hashtable = OriginalXml.ToHashtable

    '            ' now loop through all the paths and add those into their paths
    '            For Each p As String In paths
    '                htNew.Add(p, ctrl.ToHashtable(p)) 'ToHashtable(ctrl)
    '            Next

    '            ' call the merge function to put the two together and return the resulting xml
    '            Return UpdateHashtableValues(htOriginal, htNew).ToXml
    '        Else
    '            ' now loop through all the paths and add those into their paths
    '            For Each p As String In paths
    '                htNew.Add(p, ctrl.ToHashtable(p)) 'ToHashtable(ctrl)
    '            Next
    '        End If

    '    Catch ex As Exception
    '        ex.WriteToErrorLog(Enums.ProjectName.CommonCoreShared)(0, "SDK")
    '    End Try

    '    Return htNew.ToXml
    'End Function
    '<Extension()> Public Function ToXml(ByVal ctrl As Control, ByVal XmlPath As String) As XmlDocument
    '    If XmlPath.Trim = "" Then XmlPath = "root"

    '    Dim htNew As Hashtable = ctrl.ToHashtable(XmlPath)

    '    Return htNew.ToXml
    'End Function

    '<Extension()> Public Function ToXmlString(ByVal ht As Hashtable) As String
    '    Try
    '        Return ht.ToXml.ToXmlString

    '    Catch ex As Exception
    '        ex.WriteToErrorLog(Enums.ProjectName.CommonCoreShared)(0, "SDK")
    '        Return Nothing
    '    End Try
    'End Function
    <Extension()> Public Function ToXmlString(ByVal xDoc As XmlDocument) As String
        Try
            Dim sw As StringWriter = New StringWriter
            xDoc.Save(sw)
            xDoc = Nothing

            Return sw.ToString()

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(Enums.ProjectName.CommonCoreShared))
            Return ""
        End Try
    End Function

    '<Extension()> Public Function ToHashtable(ByVal xElement As XmlElement) As Hashtable
    '    Try
    '        Dim ht As New Hashtable

    '        If xElement IsNot Nothing Then
    '            For Each child As XmlNode In xElement.ChildNodes
    '                If child.HasChildNodes Then
    '                    If child.ChildNodes.Count <> 1 And Not child.ChildNodes(0).Name.StartsWith("#") Then
    '                        Dim tmp As Hashtable = CType(child, XmlElement).ToHashtable
    '                        If child.Attributes.Count > 0 Then
    '                            ht.Add(child.Name.XmlDecode(True), tmp)
    '                            If Not child.Attributes("Type") Is Nothing Then ht.Add("_" & child.Name.XmlDecode(True) & "_type", child.Attributes("Type").Value.XmlDecode)
    '                            If Not child.Attributes("Selected") Is Nothing Then ht.Add("_" & child.Name.XmlDecode(True) & "_selected", child.Attributes("Selected").Value.XmlDecode)
    '                            If Not child.Attributes("Index") Is Nothing Then ht.Add("_" & child.Name.XmlDecode(True) & "_index", child.Attributes("Index").Value.XmlDecode)
    '                        Else : ht.Add(child.Name.XmlDecode(True), tmp)
    '                        End If
    '                    Else
    '                        If child.Attributes.Count > 0 Then
    '                            ht.Add(child.Name.XmlDecode(True), child.InnerText.XmlDecode)
    '                            If Not child.Attributes("Type") Is Nothing Then ht.Add("_" & child.Name.XmlDecode(True) & "_type", child.Attributes("Type").Value.XmlDecode)
    '                            If Not child.Attributes("Selected") Is Nothing Then ht.Add("_" & child.Name.XmlDecode(True) & "_selected", child.Attributes("Selected").Value.XmlDecode)
    '                            If Not child.Attributes("Index") Is Nothing Then ht.Add("_" & child.Name.XmlDecode(True) & "_index", child.Attributes("Index").Value.XmlDecode)
    '                        Else : ht.Add(child.Name.XmlDecode(True), CheckString(child.InnerText.XmlDecode, ""))
    '                        End If
    '                    End If
    '                Else
    '                    If child.Attributes.Count > 0 Then
    '                        ht.Add(child.Name.XmlDecode(True), child.InnerText.XmlDecode)
    '                        If Not child.Attributes("Type") Is Nothing Then ht.Add("_" & child.Name.XmlDecode(True) & "_type", child.Attributes("Type").Value.XmlDecode)
    '                        If Not child.Attributes("Selected") Is Nothing Then ht.Add("_" & child.Name.XmlDecode(True) & "_selected", child.Attributes("Selected").Value.XmlDecode)
    '                        If Not child.Attributes("Index") Is Nothing Then ht.Add("_" & child.Name.XmlDecode(True) & "_index", child.Attributes("Index").Value.XmlDecode)
    '                    Else : ht.Add(child.Name.XmlDecode(True), CheckString(child.InnerText.XmlDecode, ""))
    '                    End If
    '                End If
    '            Next
    '        End If

    '        Return ht

    '    Catch ex As Exception
    '        ex.WriteToErrorLog(Enums.ProjectName.CommonCoreShared)(0, "SDK")
    '        Return New Hashtable
    '    End Try
    'End Function
    '<Extension()> Public Function ToHashtable(ByVal xDoc As XmlDocument) As Hashtable
    '    Dim xElem As XmlElement = xDoc.GetFirstElement
    '    Dim htTemp As Hashtable = xElem.ToHashtable
    '    Return htTemp
    'End Function
    '<Extension()> Public Function ToHashtable(ByVal ctrl As Control, ByVal GroupPath As String) As Hashtable
    '    Dim ctrls As New Hashtable

    '    Try
    '        ' loop through all controls in the start control
    '        For Each ctr As Object In ctrl.Controls
    '            ' get the name based on the ID by default
    '            Dim sName As String = IIf(CType(ctr, Control).ID Is Nothing, CType(ctr, Control).ClientID, CType(ctr, Control).ID).ToString
    '            Dim sTypeName As String = "_" & sName & "_type"
    '            Dim noGroupPath As Boolean = (GroupPath Is Nothing OrElse GroupPath.ToLower = "root")

    '            'If sName.ToLower.Contains("construction") Then'stop

    '            If TypeOf ctr Is Controls.TextBoxes.Base Then
    '                Dim ctl As Controls.TextBoxes.TextBox = CType(ctr, Controls.TextBoxes.TextBox)

    '                If ctl.DataFieldName.Trim <> "" Then
    '                    sName = ctl.DataFieldName
    '                    sTypeName = "_" & sName & "_type"
    '                End If

    '                Dim addToList As Boolean = False
    '                If Not GroupPath Is Nothing Then
    '                    If GroupPath.ToLower = "root" Then
    '                        If ctl.XmlPath.Trim = "" Then addToList = True
    '                    Else
    '                        If GroupPath.ToLower = ctl.XmlPath.ToLower Then addToList = True
    '                    End If
    '                Else
    '                    If ctl.XmlPath.Trim = "" Then addToList = True
    '                End If
    '                If addToList Then If Not ctrls.ContainsKey(sName) Then ctrls.Add(sName, ctl.Text) Else ctrls(sName) = ctl.Text

    '            ElseIf TypeOf ctr Is Controls.TextBoxes.NumericTextBox Then
    '                Dim ctl As Controls.TextBoxes.NumericTextBox = CType(ctr, Controls.TextBoxes.NumericTextBox)

    '                If ctl.DataFieldName.Trim <> "" Then
    '                    sName = ctl.DataFieldName
    '                    sTypeName = "_" & sName & "_type"
    '                End If

    '                Dim addToList As Boolean = False
    '                If Not GroupPath Is Nothing Then
    '                    If GroupPath.ToLower = "root" Then
    '                        If ctl.XmlPath.Trim = "" Then addToList = True
    '                    Else
    '                        If GroupPath.ToLower = ctl.XmlPath.ToLower Then addToList = True
    '                    End If
    '                Else
    '                    If ctl.XmlPath.Trim = "" Then addToList = True
    '                End If
    '                If addToList Then If Not ctrls.ContainsKey(sName) Then ctrls.Add(sName, ctl.Text) Else ctrls(sName) = ctl.Text

    '            ElseIf TypeOf ctr Is Controls.TextBoxes.DateTextBox Then
    '                Dim ctl As Controls.TextBoxes.DateTextBox = CType(ctr, Controls.TextBoxes.DateTextBox)

    '                If ctl.DataFieldName.Trim <> "" Then
    '                    sName = ctl.DataFieldName
    '                    sTypeName = "_" & sName & "_type"
    '                End If

    '                Dim addToList As Boolean = False
    '                If Not GroupPath Is Nothing Then
    '                    If GroupPath.ToLower = "root" Then
    '                        If ctl.XmlPath.Trim = "" Then addToList = True
    '                    Else
    '                        If GroupPath.ToLower = ctl.XmlPath.ToLower Then addToList = True
    '                    End If
    '                Else
    '                    If ctl.XmlPath.Trim = "" Then addToList = True
    '                End If
    '                If addToList Then If Not ctrls.ContainsKey(sName) Then ctrls.Add(sName, ctl.Text) Else ctrls(sName) = ctl.Text

    '            ElseIf TypeOf ctr Is Controls.TextBoxes.MaskedTextBox Then
    '                Dim ctl As Controls.TextBoxes.MaskedTextBox = CType(ctr, Controls.TextBoxes.MaskedTextBox)

    '                If ctl.DataFieldName.Trim <> "" Then
    '                    sName = ctl.DataFieldName
    '                    sTypeName = "_" & sName & "_type"
    '                End If

    '                Dim addToList As Boolean = False
    '                If Not GroupPath Is Nothing Then
    '                    If GroupPath.ToLower = "root" Then
    '                        If ctl.XmlPath.Trim = "" Then addToList = True
    '                    Else
    '                        If GroupPath.ToLower = ctl.XmlPath.ToLower Then addToList = True
    '                    End If
    '                Else
    '                    If ctl.XmlPath.Trim = "" Then addToList = True
    '                End If
    '                If addToList Then If Not ctrls.ContainsKey(sName) Then ctrls.Add(sName, ctl.Text) Else ctrls(sName) = ctl.Text

    '            ElseIf TypeOf ctr Is Controls.Labels.Base Then
    '                Dim ctl As Controls.Labels.DataLabel = CType(ctr, Controls.Labels.DataLabel)

    '                If ctl.DataFieldName.Trim <> "" Then
    '                    sName = ctl.DataFieldName
    '                    sTypeName = "_" & sName & "_type"
    '                End If

    '                Dim addToList As Boolean = False
    '                If Not GroupPath Is Nothing Then
    '                    If GroupPath.ToLower = "root" Then
    '                        If ctl.XmlPath.Trim = "" Then addToList = True
    '                    Else
    '                        If GroupPath.ToLower = ctl.XmlPath.ToLower Then addToList = True
    '                    End If
    '                Else
    '                    If ctl.XmlPath.Trim = "" Then addToList = True
    '                End If
    '                If addToList Then If Not ctrls.ContainsKey(sName) Then ctrls.Add(sName, ctl.Text) Else ctrls(sName) = ctl.Text

    '            ElseIf TypeOf ctr Is Controls.RadioButtons.Base Then
    '                Dim ctl As Controls.RadioButtons.RadioButton = CType(ctr, Controls.RadioButtons.RadioButton)

    '                If ctl.DataFieldName.Trim <> "" Then
    '                    sName = ctl.DataFieldName
    '                    sTypeName = "_" & sName & "_type"
    '                End If

    '                Dim addToList As Boolean = False
    '                If Not GroupPath Is Nothing Then
    '                    If GroupPath.ToLower = "root" Then
    '                        If ctl.XmlPath.Trim = "" Then addToList = True
    '                    Else
    '                        If GroupPath.ToLower = ctl.XmlPath.ToLower Then addToList = True
    '                    End If
    '                Else
    '                    If ctl.XmlPath.Trim = "" Then addToList = True
    '                End If
    '                If addToList Then If Not ctrls.ContainsKey(sName) Then ctrls.Add(sName, ctl.Checked) Else ctrls(sName) = ctl.Checked

    '            ElseIf TypeOf ctr Is Controls.CheckBoxes.Base Then
    '                Dim ctl As Controls.CheckBoxes.CheckBox = CType(ctr, Controls.CheckBoxes.CheckBox)

    '                If ctl.DataFieldName.Trim <> "" Then
    '                    sName = ctl.DataFieldName
    '                    sTypeName = "_" & sName & "_type"
    '                End If

    '                Dim addToList As Boolean = False
    '                If Not GroupPath Is Nothing Then
    '                    If GroupPath.ToLower = "root" Then
    '                        If ctl.XmlPath.Trim = "" Then addToList = True
    '                    Else
    '                        If GroupPath.ToLower = ctl.XmlPath.ToLower Then addToList = True
    '                    End If
    '                Else
    '                    If ctl.XmlPath.Trim = "" Then addToList = True
    '                End If
    '                If addToList Then If Not ctrls.ContainsKey(sName) Then ctrls.Add(sName, ctl.Checked) Else ctrls(sName) = ctl.Checked

    '            ElseIf TypeOf ctr Is Controls.DropDownLists.Base Then
    '                Dim ctl As Controls.DropDownLists.DropDownList = CType(ctr, Controls.DropDownLists.DropDownList)

    '                If ctl.DataFieldName.Trim <> "" Then
    '                    sName = ctl.DataFieldName
    '                    sTypeName = "_" & sName & "_type"
    '                End If

    '                Dim addToList As Boolean = False
    '                If Not GroupPath Is Nothing Then
    '                    If GroupPath.ToLower = "root" Then
    '                        If ctl.XmlPath.Trim = "" Then addToList = True
    '                    Else
    '                        If GroupPath.ToLower = ctl.XmlPath.ToLower Then addToList = True
    '                    End If
    '                Else
    '                    If ctl.XmlPath.Trim = "" Then addToList = True
    '                End If
    '                Dim v As String = If(ctl.SelectedValue = "", ctl.Text, ctl.SelectedValue)
    '                If addToList Then If Not ctrls.ContainsKey(sName) Then ctrls.Add(sName, v) Else ctrls(sName) = v

    '            ElseIf noGroupPath And TypeOf ctr Is Telerik.Web.UI.RadTimePicker Then
    '                If Not ctrls.ContainsKey(sName) Then ctrls.Add(sName, CType(ctr, Telerik.Web.UI.RadTimePicker).SelectedDate) Else ctrls(sName) = CType(ctr, Telerik.Web.UI.RadTimePicker).SelectedDate

    '            ElseIf noGroupPath And TypeOf ctr Is Telerik.Web.UI.RadDatePicker Then
    '                If Not ctrls.ContainsKey(sName) Then ctrls.Add(sName, CType(ctr, Telerik.Web.UI.RadDatePicker).SelectedDate) Else ctrls(sName) = CType(ctr, Telerik.Web.UI.RadDatePicker).SelectedDate

    '            ElseIf noGroupPath And TypeOf ctr Is Telerik.Web.UI.RadCalendar Then
    '                If Not ctrls.ContainsKey(sName) Then ctrls.Add(sName, CType(ctr, Telerik.Web.UI.RadCalendar).SelectedDate) Else ctrls(sName) = CType(ctr, Telerik.Web.UI.RadCalendar).SelectedDate

    '            ElseIf noGroupPath And TypeOf ctr Is TextBox Then
    '                If Not ctrls.ContainsKey(sName) Then ctrls.Add(sName, CType(ctr, TextBox).Text) Else ctrls(sName) = CType(ctr, TextBox).Text

    '            ElseIf noGroupPath And TypeOf ctr Is RadioButton Then
    '                If Not ctrls.ContainsKey(sName) Then ctrls.Add(sName, CType(ctr, RadioButton).Checked) Else ctrls(sName) = CType(ctr, RadioButton).Checked

    '            ElseIf noGroupPath And TypeOf ctr Is CheckBox Then
    '                If Not ctrls.ContainsKey(sName) Then ctrls.Add(sName, CType(ctr, CheckBox).Checked) Else ctrls(sName) = CType(ctr, CheckBox).Checked

    '            ElseIf noGroupPath And TypeOf ctr Is DropDownList Then
    '                If Not ctrls.ContainsKey(sName) Then ctrls.Add(sName, CType(ctr, DropDownList).SelectedValue) Else ctrls(sName) = CType(ctr, DropDownList).SelectedValue

    '            Else

    '                ' anything other containers
    '                If CType(ctr, Control).HasControls Then

    '                    ' radgrid comes in as a container
    '                    If TypeOf ctr Is Telerik.Web.UI.RadGrid Then
    '                        '    Dim tmp As New Hashtable
    '                        '    Dim y As Integer = 0
    '                        '    For Each myRow As Telerik.Web.UI.GridItem In CType(ctr, Telerik.Web.UI.RadGrid).Items
    '                        '        Dim tmpRow As New Hashtable
    '                        '        For x As Integer = 0 To CType(ctr, Telerik.Web.UI.RadGrid).Columns.Count - 1
    '                        '            Dim h As String = CType(ctr, Telerik.Web.UI.RadGrid).Columns(x).HeaderText & "_" & x
    '                        '            Dim r As String = myRow.Cells(x).Text
    '                        '            tmpRow.Add(h.XmlEncode, r.XmlEncode)
    '                        '        Next
    '                        '        If tmpRow.Count > 0 Then tmp.Add("Row" & y, tmpRow)
    '                        '        y += 1
    '                        '    Next
    '                        '    If tmp.Count > 0 Then ctrls.Add(sName, tmp) : ctrls.Add(sTypeName, ctr.GetType.Name)

    '                        '    If Not TypeOf ctr Is LiteralControl And Not TypeOf ctr Is HtmlHead Then
    '                        '        Dim tmp As Hashtable = ToHashtable(CType(ctr, Control), GroupPath)
    '                        '        For Each key In tmp.Keys
    '                        '            If Not ctrls.ContainsKey(key) Then
    '                        '                If TypeOf tmp(key) Is Hashtable Then
    '                        '                    ctrls.Add(key.ToString, tmp(key))
    '                        '                Else : ctrls.Add(key.ToString, tmp(key).ToString)
    '                        '                End If
    '                        '            Else : ctrls(key) = tmp(key)
    '                        '            End If
    '                        '        Next
    '                        '    End If

    '                    ElseIf TypeOf ctr Is Telerik.Web.UI.RadAjaxPanel Then
    '                        Dim tmp As Hashtable = ToHashtable(CType(ctr, Telerik.Web.UI.RadAjaxPanel), GroupPath)
    '                        For Each key In tmp.Keys
    '                            If Not ctrls.ContainsKey(key) Then ctrls.Add(key.ToString, tmp(key).ToString) Else ctrls(key) = tmp(key)
    '                        Next

    '                    ElseIf TypeOf ctr Is UpdatePanel Then
    '                        Dim tmp As Hashtable = ToHashtable(CType(ctr, UpdatePanel).Controls(0), GroupPath)
    '                        For Each key In tmp.Keys
    '                            If Not ctrls.ContainsKey(key) Then
    '                                If TypeOf tmp(key) Is Hashtable Then
    '                                    ctrls.Add(key.ToString, tmp(key))
    '                                Else : ctrls.Add(key.ToString, tmp(key).ToString)
    '                                End If
    '                            Else : ctrls(key) = tmp(key)
    '                            End If
    '                        Next

    '                    ElseIf TypeOf ctr Is Panel Then
    '                        Dim tmp As Hashtable = ToHashtable(CType(ctr, Panel), GroupPath)
    '                        For Each key In tmp.Keys
    '                            If Not ctrls.ContainsKey(key) Then
    '                                If TypeOf tmp(key) Is Hashtable Then
    '                                    ctrls.Add(key.ToString, tmp(key))
    '                                Else : ctrls.Add(key.ToString, tmp(key).ToString)
    '                                End If
    '                            Else : ctrls(key) = tmp(key)
    '                            End If
    '                        Next

    '                    Else

    '                        ' list controls get saved as containers with values and selected attributes
    '                        If TypeOf ctr Is RadioButtonList Then
    '                            Dim tmp As New Hashtable
    '                            Dim x As Integer = 0
    '                            For Each itm As ListItem In CType(ctr, RadioButtonList).Items
    '                                tmp.Add(itm.Text, itm.Value)
    '                                tmp.Add("_" & itm.Text & "_selected", itm.Selected)
    '                                tmp.Add("_" & itm.Text & "_index", x)
    '                                x += 1
    '                            Next
    '                            If Not ctrls.ContainsKey(sName) Then ctrls.Add(sName, tmp) Else ctrls(sName) = tmp
    '                            'ctrls.Add(sTypeName, ctr.GetType.Name)

    '                        ElseIf TypeOf ctr Is CheckBoxList Then
    '                            Dim tmp As New Hashtable
    '                            Dim x As Integer = 0
    '                            For Each itm As ListItem In CType(ctr, CheckBoxList).Items
    '                                tmp.Add(itm.Text, itm.Value)
    '                                tmp.Add("_" & itm.Text & "_selected", itm.Selected)
    '                                tmp.Add("_" & itm.Text & "_index", x)
    '                                x += 1
    '                            Next
    '                            If Not ctrls.ContainsKey(sName) Then ctrls.Add(sName, tmp) Else ctrls(sName) = tmp
    '                            'ctrls.Add(sTypeName, ctr.GetType.Name)

    '                        ElseIf TypeOf ctr Is ListBox Then
    '                            Dim tmp As New Hashtable
    '                            Dim x As Integer = 0
    '                            For Each itm As ListItem In CType(ctr, ListBox).Items
    '                                tmp.Add(itm.Text, itm.Value)
    '                                tmp.Add("_" & itm.Text & "_selected", itm.Selected)
    '                                tmp.Add("_" & itm.Text & "_index", x)
    '                                x += 1
    '                            Next
    '                            If Not ctrls.ContainsKey(sName) Then ctrls.Add(sName, tmp) Else ctrls(sName) = tmp

    '                        Else
    '                            Dim tmp As Hashtable = ToHashtable(CType(ctr, Control), GroupPath)
    '                            For Each key In tmp.Keys
    '                                If Not ctrls.ContainsKey(key) Then
    '                                    If TypeOf tmp(key) Is Hashtable Then
    '                                        ctrls.Add(key.ToString, tmp(key))
    '                                    Else : ctrls.Add(key.ToString, tmp(key).ToString)
    '                                    End If
    '                                Else : ctrls(key) = tmp(key)
    '                                End If
    '                            Next

    '                        End If

    '                        ' gridview is special
    '                        'If TypeOf ctr Is GridView Then
    '                        '    Dim tmp As New Hashtable
    '                        '    Dim y As Integer = 0
    '                        '    For Each myRow As GridViewRow In CType(ctr, GridView).Rows
    '                        '        Dim tmpRow As New Hashtable
    '                        '        For x As Integer = 0 To myRow.Cells.Count - 1
    '                        '            Dim h As String = CType(ctr, GridView).Columns(x).HeaderText & "_" & x
    '                        '            Dim r As String = myRow.Cells(x).Text
    '                        '            tmpRow.Add(h.XmlEncode, r.XmlEncode)
    '                        '        Next
    '                        '        If tmpRow.Count > 0 Then tmp.Add("Row" & y, tmpRow)
    '                        '        y += 1
    '                        '    Next
    '                        '    If tmp.Count > 0 Then ctrls.Add(sName, tmp) : ctrls.Add(sTypeName, ctr.GetType.Name)
    '                        'End If
    '                        'End If
    '                    End If
    '                End If
    '            End If
    '        Next

    '        Return ctrls

    '    Catch ex As Exception
    '        ex.WriteToErrorLog(Enums.ProjectName.CommonCoreShared)(0, "SDK")
    '        Return ctrls
    '    End Try
    'End Function

    <Extension()> Public Function GetFirstElement(ByVal xDoc As XmlDocument) As XmlElement
        Try
            Return CType(xDoc.FirstChild, XmlElement)
        Catch ex As Exception
            Return CType(xDoc.ChildNodes(1), XmlElement)
        End Try
    End Function

    '<Extension()> Public Sub FromXml(ByVal ctrl As Control, ByVal data As String)
    '    Try
    '        Dim xDoc As New XmlDocument
    '        xDoc.LoadXml(data)

    '        FromXml(ctrl, xDoc)

    '    Catch ex As Exception
    '        ex.WriteToErrorLog(Enums.ProjectName.CommonCoreShared)(0, "SDK")
    '        Exit Sub
    '    End Try
    'End Sub
    '<Extension()> Public Sub FromXml(ByVal ctrl As Control, ByVal data As XmlDocument)
    '    Try
    '        Dim htTemp As Hashtable = ToHashtable(data)
    '        'Dim htValues As Hashtable = htTemp.Flatten
    '        FromHashtable(ctrl, htTemp)

    '    Catch ex As Exception
    '        ex.WriteToErrorLog(Enums.ProjectName.CommonCoreShared)(0, "SDK")
    '        Exit Sub
    '    End Try
    'End Sub

    '<Extension()> Public Function FromHashtable(ByVal xDoc As XmlDocument, ByVal data As Hashtable, ByVal name As String) As XmlElement
    '    Try
    '        Dim xElem As XmlElement = xDoc.CreateElement(name)

    '        For Each key In data.Keys
    '            If Not key.ToString.StartsWith("_") And (Not key.ToString.ToLower.EndsWith("_type") And Not key.ToString.ToLower.EndsWith("_selected")) Then
    '                If TypeOf data(key) Is Hashtable Then
    '                    Dim tmp As XmlElement = xDoc.FromHashtable(CType(data(key), Hashtable), key.ToString.XmlEncode(True))
    '                    xElem.AppendChild(tmp)
    '                Else
    '                    Dim type = "", selected = "", index As String = ""

    '                    ' handle the type attribute
    '                    If data("_" & key.ToString & "_type") IsNot Nothing Then
    '                        type = CStr(data("_" & key.ToString & "_type")).XmlEncode
    '                    End If

    '                    ' handle the selected attribute
    '                    If data("_" & key.ToString & "_selected") IsNot Nothing Then
    '                        selected = CStr(data("_" & key.ToString & "_selected")).XmlEncode
    '                    End If

    '                    ' handle the index attribute
    '                    If data("_" & key.ToString & "_index") IsNot Nothing Then
    '                        index = CStr(data("_" & key.ToString & "_index")).XmlEncode
    '                    End If

    '                    xElem.AppendChild(NewElement(xDoc, key.ToString, CStr(IIf(Not data(key) Is Nothing, data(key), "")), type, selected, index, Nothing))
    '                End If
    '            End If
    '        Next

    '        Return xElem

    '    Catch ex As Exception
    '        ex.WriteToErrorLog(Enums.ProjectName.CommonCoreShared)(0, "SDK")
    '        Return Nothing
    '    End Try
    'End Function
    '<Extension()> Public Sub FromHashtable(ByVal ctrl As Control, ByVal data As Hashtable)
    '    Try
    '        ' loop through all controls in the start control
    '        For Each ctr In ctrl.Controls
    '            ' get the name based on the ID by default
    '            Dim lookupName As String = IIf(CType(ctr, Control).ID = "", CType(ctr, Control).ClientID, CType(ctr, Control).ID).ToString

    '            'If lookupName.ToLower.Contains("phone") Then'stop

    '            If TypeOf ctr Is Controls.TextBoxes.Base Then
    '                Dim name As String = lookupName
    '                Dim ctl As Controls.TextBoxes.TextBox = CType(ctr, Controls.TextBoxes.TextBox)

    '                If ctl.DataFieldName.Trim <> "" Then lookupName = ctl.DataFieldName

    '                If ctl.XmlPath.Trim <> "" Then
    '                    Dim xPath As String = ctl.XmlPath.Trim
    '                    Try
    '                        If Not CType(data(xPath), Hashtable) Is Nothing Then
    '                            If CType(data(xPath), Hashtable)(lookupName) IsNot Nothing Then
    '                                ctl.Text = CType(data(xPath), Hashtable)(lookupName).ToString.XmlDecode
    '                            ElseIf CType(data(xPath), Hashtable)(name) IsNot Nothing Then
    '                                ctl.Text = CType(data(xPath), Hashtable)(name).ToString.XmlDecode
    '                            Else : ctl.Text = ""
    '                            End If
    '                        Else : ctl.Text = ""
    '                        End If
    '                    Catch ex As Exception
    '                        ctl.Text = data(xPath).ToString.XmlDecode
    '                    End Try
    '                Else : If data(lookupName) IsNot Nothing Then ctl.Text = data(lookupName).ToString.XmlDecode
    '                End If

    '            ElseIf TypeOf ctr Is Controls.TextBoxes.NumericTextBox Then
    '                Dim name As String = lookupName
    '                Dim ctl As Controls.TextBoxes.NumericTextBox = CType(ctr, Controls.TextBoxes.NumericTextBox)

    '                If ctl.DataFieldName.Trim <> "" Then lookupName = ctl.DataFieldName

    '                If ctl.XmlPath.Trim <> "" Then
    '                    Dim xPath As String = ctl.XmlPath.Trim
    '                    Try
    '                        If Not CType(data(xPath), Hashtable) Is Nothing Then
    '                            If CType(data(xPath), Hashtable)(lookupName) IsNot Nothing Then
    '                                ctl.Text = CType(data(xPath), Hashtable)(lookupName).ToString.XmlDecode
    '                            ElseIf CType(data(xPath), Hashtable)(name) IsNot Nothing Then
    '                                ctl.Text = CType(data(xPath), Hashtable)(name).ToString.XmlDecode
    '                            Else : ctl.Text = ""
    '                            End If
    '                        Else : ctl.Text = ""
    '                        End If
    '                    Catch ex As Exception
    '                        ctl.Text = data(xPath).ToString.XmlDecode
    '                    End Try
    '                Else : If data(lookupName) IsNot Nothing Then ctl.Text = data(lookupName).ToString.XmlDecode
    '                End If

    '            ElseIf TypeOf ctr Is Controls.TextBoxes.DateTextBox Then
    '                Dim name As String = lookupName
    '                Dim ctl As Controls.TextBoxes.DateTextBox = CType(ctr, Controls.TextBoxes.DateTextBox)

    '                If ctl.DataFieldName.Trim <> "" Then lookupName = ctl.DataFieldName

    '                If ctl.XmlPath.Trim <> "" Then
    '                    Dim xPath As String = ctl.XmlPath.Trim
    '                    Try
    '                        If Not CType(data(xPath), Hashtable) Is Nothing Then
    '                            If CType(data(xPath), Hashtable)(lookupName) IsNot Nothing Then
    '                                ctl.Text = CType(data(xPath), Hashtable)(lookupName).ToString.XmlDecode
    '                            ElseIf CType(data(xPath), Hashtable)(name) IsNot Nothing Then
    '                                ctl.Text = CType(data(xPath), Hashtable)(name).ToString.XmlDecode
    '                            Else : ctl.Text = ""
    '                            End If
    '                        Else : ctl.Text = ""
    '                        End If
    '                    Catch ex As Exception
    '                        ctl.Text = data(xPath).ToString.XmlDecode
    '                    End Try
    '                Else : If data(lookupName) IsNot Nothing Then ctl.Text = data(lookupName).ToString.XmlDecode
    '                End If

    '            ElseIf TypeOf ctr Is Controls.TextBoxes.MaskedTextBox Then
    '                Dim name As String = lookupName
    '                Dim ctl As Controls.TextBoxes.MaskedTextBox = CType(ctr, Controls.TextBoxes.MaskedTextBox)

    '                If ctl.DataFieldName.Trim <> "" Then lookupName = ctl.DataFieldName

    '                If ctl.XmlPath.Trim <> "" Then
    '                    Dim xPath As String = ctl.XmlPath.Trim
    '                    Try
    '                        If Not CType(data(xPath), Hashtable) Is Nothing Then
    '                            If CType(data(xPath), Hashtable)(lookupName) IsNot Nothing Then
    '                                ctl.Text = CType(data(xPath), Hashtable)(lookupName).ToString.XmlDecode
    '                            ElseIf CType(data(xPath), Hashtable)(name) IsNot Nothing Then
    '                                ctl.Text = CType(data(xPath), Hashtable)(name).ToString.XmlDecode
    '                            Else : ctl.Text = ""
    '                            End If
    '                        Else : ctl.Text = ""
    '                        End If
    '                    Catch ex As Exception
    '                        ctl.Text = data(xPath).ToString.XmlDecode
    '                    End Try
    '                Else : If data(lookupName) IsNot Nothing Then ctl.Text = data(lookupName).ToString.XmlDecode
    '                End If

    '            ElseIf TypeOf ctr Is Controls.Labels.Base Then
    '                Dim name As String = lookupName
    '                Dim ctl As Controls.Labels.DataLabel = CType(ctr, Controls.Labels.DataLabel)

    '                If ctl.DataFieldName.Trim <> "" Then lookupName = ctl.DataFieldName

    '                If ctl.XmlPath.Trim <> "" Then
    '                    Dim xPath As String = ctl.XmlPath.Trim
    '                    Try
    '                        If Not CType(data(xPath), Hashtable) Is Nothing Then
    '                            If CType(data(xPath), Hashtable)(lookupName) IsNot Nothing Then
    '                                ctl.Text = CType(data(xPath), Hashtable)(lookupName).ToString.XmlDecode
    '                            ElseIf CType(data(xPath), Hashtable)(name) IsNot Nothing Then
    '                                ctl.Text = CType(data(xPath), Hashtable)(name).ToString.XmlDecode
    '                            Else : ctl.Text = ""
    '                            End If
    '                        Else : ctl.Text = ""
    '                        End If
    '                    Catch ex As Exception
    '                        ctl.Text = data(xPath).ToString.XmlDecode
    '                    End Try
    '                Else : If data(lookupName) IsNot Nothing Then ctl.Text = data(lookupName).ToString.XmlDecode
    '                End If

    '            ElseIf TypeOf ctr Is Controls.RadioButtons.RadioButton Then
    '                Dim name As String = lookupName
    '                Dim ctl As Controls.RadioButtons.RadioButton = CType(ctr, Controls.RadioButtons.RadioButton)

    '                If ctl.DataFieldName.Trim <> "" Then lookupName = ctl.DataFieldName

    '                If ctl.XmlPath.Trim <> "" Then
    '                    Dim xPath As String = ctl.XmlPath.Trim
    '                    Try
    '                        If Not CType(data(xPath), Hashtable) Is Nothing Then
    '                            If CType(data(xPath), Hashtable)(lookupName) IsNot Nothing Then
    '                                ctl.Checked = CType(data(xPath), Hashtable)(lookupName).ToString.ToBoolean
    '                            ElseIf CType(data(xPath), Hashtable)(name) IsNot Nothing Then
    '                                ctl.Checked = CType(data(xPath), Hashtable)(name).ToString.ToBoolean
    '                            Else : ctl.Checked = False
    '                            End If
    '                        Else : ctl.Checked = False
    '                        End If
    '                    Catch ex As Exception
    '                        ctl.Checked = data(xPath).ToString.ToBoolean
    '                    End Try
    '                Else : If data(lookupName) IsNot Nothing Then ctl.Checked = data(lookupName).ToString.ToBoolean
    '                End If

    '            ElseIf TypeOf ctr Is Controls.CheckBoxes.CheckBox Then
    '                Dim name As String = lookupName
    '                Dim ctl As Controls.CheckBoxes.CheckBox = CType(ctr, Controls.CheckBoxes.CheckBox)

    '                If ctl.DataFieldName.Trim <> "" Then lookupName = ctl.DataFieldName

    '                If ctl.XmlPath.Trim <> "" Then
    '                    Dim xPath As String = ctl.XmlPath.Trim
    '                    Try
    '                        If data(xPath) IsNot Nothing AndAlso data(xPath).ToString = "System.Collections.Hashtable" Then
    '                            If Not CType(data(xPath), Hashtable) Is Nothing Then
    '                                If CType(data(xPath), Hashtable)(lookupName) IsNot Nothing Then
    '                                    ctl.Checked = CType(data(xPath), Hashtable)(lookupName).ToString.ToBoolean
    '                                ElseIf CType(data(xPath), Hashtable)(name) IsNot Nothing Then
    '                                    ctl.Checked = CType(data(xPath), Hashtable)(name).ToString.ToBoolean
    '                                Else : ctl.Checked = False
    '                                End If
    '                            Else : ctl.Checked = False
    '                            End If
    '                        End If
    '                    Catch ex As Exception
    '                        ctl.Checked = data(xPath).ToString.ToBoolean
    '                    End Try
    '                Else : If data(lookupName) IsNot Nothing Then ctl.Checked = data(lookupName).ToString.ToBoolean
    '                End If

    '            ElseIf TypeOf ctr Is Controls.DropDownLists.Base Then
    '                Dim name As String = lookupName
    '                Dim ctl As Controls.DropDownLists.DropDownList = CType(ctr, Controls.DropDownLists.DropDownList)

    '                If ctl.DataFieldName.Trim <> "" Then lookupName = ctl.DataFieldName

    '                If ctl.XmlPath.Trim <> "" Then
    '                    Dim xPath As String = ctl.XmlPath.Trim
    '                    Try
    '                        If Not CType(data(xPath), Hashtable) Is Nothing Then
    '                            If CType(data(xPath), Hashtable)(lookupName) IsNot Nothing Then
    '                                Dim v As String = CType(data(xPath), Hashtable)(lookupName).ToString.XmlDecode
    '                                If ctl.Items.InList(v) Then ctl.SelectedValue = v
    '                            ElseIf CType(data(xPath), Hashtable)(name) IsNot Nothing Then
    '                                Dim v As String = CType(data(xPath), Hashtable)(name).ToString.XmlDecode
    '                                If ctl.Items.InList(v) Then ctl.SelectedValue = v
    '                            Else : ctl.SelectedIndex = -1
    '                            End If
    '                        Else : ctl.SelectedIndex = -1
    '                        End If
    '                    Catch ex As Exception
    '                        If ctl.Items.InList(data(xPath).ToString.XmlDecode) Then ctl.SelectedValue = data(xPath).ToString.XmlDecode
    '                    End Try
    '                Else
    '                    If data(lookupName) IsNot Nothing Then
    '                        Dim v As String = data(lookupName).ToString.XmlDecode
    '                        If ctl.Items.InList(v) Then ctl.SelectedValue = v
    '                    End If
    '                End If

    '            ElseIf TypeOf ctr Is Telerik.Web.UI.RadTimePicker Then
    '                If data(lookupName) IsNot Nothing Then CType(ctr, Telerik.Web.UI.RadTimePicker).SelectedDate = CType(data(lookupName), Date?)

    '            ElseIf TypeOf ctr Is Telerik.Web.UI.RadDatePicker Then
    '                If data(lookupName) IsNot Nothing Then CType(ctr, Telerik.Web.UI.RadDatePicker).SelectedDate = CType(data(lookupName), Date?)

    '            ElseIf TypeOf ctr Is Telerik.Web.UI.RadCalendar Then
    '                If data(lookupName) IsNot Nothing Then CType(ctr, Telerik.Web.UI.RadCalendar).SelectedDate = CDate(data(lookupName))

    '            ElseIf TypeOf ctr Is TextBox Then
    '                If data(lookupName) IsNot Nothing Then CType(ctr, TextBox).Text = data(lookupName).ToString.XmlDecode

    '            ElseIf TypeOf ctr Is RadioButton Then
    '                If data(lookupName) IsNot Nothing Then CType(ctr, RadioButton).Checked = data(lookupName).ToString.ToBoolean

    '            ElseIf TypeOf ctr Is CheckBox Then
    '                If data(lookupName) IsNot Nothing Then CType(ctr, CheckBox).Checked = data(lookupName).ToString.ToBoolean

    '            ElseIf TypeOf ctr Is DropDownList Then
    '                If data(lookupName) IsNot Nothing Then CType(ctr, DropDownList).SelectedValue = data(lookupName).ToString.XmlDecode

    '            Else
    '                ' handle containers
    '                If CType(ctr, Control).HasControls Then
    '                    FromHashtable(CType(ctr, Control), data)

    '                Else
    '                    ' list controls get saved as containers with values and selected attributes
    '                    If TypeOf ctr Is RadioButtonList Then
    '                        If data(lookupName) IsNot Nothing Then
    '                            Dim tmp As Hashtable = CType(data(lookupName), Hashtable)
    '                            For Each key In tmp.Keys
    '                                If Not key.ToString.StartsWith("_") And (Not key.ToString.ToLower.EndsWith("selected") And Not key.ToString.ToLower.EndsWith("index")) Then
    '                                    Dim itm As New ListItem(key.ToString, tmp(key).ToString)
    '                                    If CType(ctr, RadioButtonList).Items.Contains(itm) Then
    '                                        Dim tmpIndex As Integer = 0
    '                                        For x As Integer = 0 To CType(ctr, RadioButtonList).Items.Count - 1
    '                                            If CType(ctr, RadioButtonList).Items(x).Text.ToLower = itm.Text.ToLower And CType(ctr, RadioButtonList).Items(x).Value.ToLower = itm.Value.ToLower Then
    '                                                CType(ctr, RadioButtonList).Items(x).Selected = tmp("_" & key.ToString & "_selected").ToString.ToBoolean
    '                                                Exit For
    '                                            End If
    '                                        Next
    '                                    Else
    '                                        itm.Selected = tmp("_" & key.ToString & "_selected").ToString.ToBoolean
    '                                        CType(ctr, RadioButtonList).Items.Add(itm)
    '                                    End If
    '                                End If
    '                            Next
    '                        End If

    '                    ElseIf TypeOf ctr Is CheckBoxList Then
    '                        If data(lookupName) IsNot Nothing Then
    '                            Dim tmp As Hashtable = CType(data(lookupName), Hashtable)
    '                            For Each key In tmp.Keys
    '                                If Not key.ToString.StartsWith("_") And (Not key.ToString.ToLower.EndsWith("selected") And Not key.ToString.ToLower.EndsWith("index")) Then
    '                                    Dim itm As New ListItem(key.ToString, tmp(key).ToString)
    '                                    If CType(ctr, CheckBoxList).Items.Contains(itm) Then
    '                                        Dim tmpIndex As Integer = 0
    '                                        For x As Integer = 0 To CType(ctr, CheckBoxList).Items.Count - 1
    '                                            If CType(ctr, CheckBoxList).Items(x).Text.ToLower = itm.Text.ToLower And CType(ctr, CheckBoxList).Items(x).Value.ToLower = itm.Value.ToLower Then
    '                                                CType(ctr, CheckBoxList).Items(x).Selected = tmp("_" & key.ToString & "_selected").ToString.ToBoolean
    '                                                Exit For
    '                                            End If
    '                                        Next
    '                                    Else
    '                                        itm.Selected = tmp("_" & key.ToString & "_selected").ToString.ToBoolean
    '                                        CType(ctr, CheckBoxList).Items.Add(itm)
    '                                    End If
    '                                End If
    '                            Next
    '                        End If

    '                    ElseIf TypeOf ctr Is ListBox Then
    '                        If data(lookupName) IsNot Nothing Then
    '                            Dim tmp As Hashtable = CType(data(lookupName), Hashtable)
    '                            For Each key In tmp.Keys
    '                                If Not key.ToString.StartsWith("_") And (Not key.ToString.ToLower.EndsWith("selected") And Not key.ToString.ToLower.EndsWith("index")) Then
    '                                    Dim itm As New ListItem(key.ToString, tmp(key).ToString)
    '                                    If CType(ctr, ListBox).Items.Contains(itm) Then
    '                                        Dim tmpIndex As Integer = 0
    '                                        For x As Integer = 0 To CType(ctr, ListBox).Items.Count - 1
    '                                            If CType(ctr, ListBox).Items(x).Text.ToLower = itm.Text.ToLower And CType(ctr, ListBox).Items(x).Value.ToLower = itm.Value.ToLower Then
    '                                                CType(ctr, ListBox).Items(x).Selected = tmp("_" & key.ToString & "_selected").ToString.ToBoolean
    '                                                Exit For
    '                                            End If
    '                                        Next
    '                                    Else
    '                                        itm.Selected = tmp("_" & key.ToString & "_selected").ToString.ToBoolean
    '                                        CType(ctr, ListBox).Items.Add(itm)
    '                                    End If
    '                                End If
    '                            Next
    '                        End If

    '                    Else

    '                        ' gridview is special
    '                        If TypeOf ctr Is GridView Then
    '                            Dim grid As Hashtable = GridFromHashtable(data, lookupName)

    '                            If grid.Keys.Count > 0 Then
    '                                Dim tbl As New DataTable

    '                                For x As Integer = 0 To CType(grid("Row0"), Hashtable).Keys.Count - 1
    '                                    tbl.Columns.Add(x.ToString)
    '                                Next

    '                                For Each column In CType(grid("Row0"), Hashtable).Keys
    '                                    Dim columnID As Integer = CInt(Right(column.ToString, 1))
    '                                    tbl.Columns(columnID).ColumnName = Left(column.ToString, Len(column) - 2).XmlDecode
    '                                    tbl.Columns(columnID).Caption = tbl.Columns(columnID).ColumnName.XmlDecode
    '                                Next

    '                                For x As Integer = 0 To grid.Keys.Count - 1
    '                                    Dim row As Hashtable = CType(grid("Row" & x), Hashtable)
    '                                    Dim sRowValues(row.Keys.Count - 1) As String

    '                                    For Each column In row.Keys
    '                                        Dim columnID As Integer = CInt(Right(column.ToString, 1))
    '                                        sRowValues(columnID) = row(column).ToString.XmlDecode
    '                                    Next
    '                                    tbl.Rows.Add(sRowValues)
    '                                Next

    '                                CType(ctr, GridView).DataSource = tbl
    '                                CType(ctr, GridView).DataBind()
    '                            End If
    '                        End If
    '                    End If
    '                End If
    '            End If
    '        Next

    '    Catch ex As Exception
    '        ex.WriteToErrorLog(Enums.ProjectName.CommonCoreShared)(0, "SDK")
    '        Exit Sub
    '    End Try
    'End Sub

    <Extension()> Public Function GetNodeValue(ByVal xDoc As XmlDocument, ByVal nodeName As String) As String
        Dim xElem As XmlElement = GetFirstElement(xDoc)

        If nodeName.Contains(".") Then
            ' we're looking for an attribute value
            Dim xNode As XmlNode = FindNodeInElement(xElem, nodeName.Split(CChar("."))(0))

            ' see if the attribute exists.  if so, return it.  if not, return an empty string
            If Not xNode.Attributes(nodeName.Split(CChar("."))(1)) Is Nothing Then
                Return xNode.Attributes(nodeName.Split(CChar("."))(1)).InnerText
            Else : Return ""
            End If
        Else
            ' we're looking for the node value
            Dim xNode As XmlNode = FindNodeInElement(xElem, nodeName)
            Return xNode.InnerText
        End If
    End Function

    <Extension()> Public Function NodeExists(ByVal xDoc As XmlDocument, ByVal nodeName As String) As Boolean
        Dim xElem As XmlElement = GetFirstElement(xDoc)
        Dim xNode As XmlNode = FindNodeInElement(xElem, nodeName)
        Return Not (xNode.Name = "SABorderJumpersNode")
    End Function

    Private Function FindNodeInElement(ByVal data As XmlElement, ByVal nodeName As String) As XmlNode
        Try
            Dim node As XmlNode = NewElement(New XmlDocument, "SABorderJumpersNode", "")
            Dim bFound As Boolean = False

            If data.Name.ToLower = nodeName.ToLower Then
                node = data
            Else
                For Each child As XmlNode In data.ChildNodes
                    If child.HasChildNodes Then
                        Dim tmp As XmlNode = FindNodeInElement(CType(child, XmlElement), nodeName)
                        If LCase(tmp.Name) = LCase(nodeName) Then
                            node = tmp : bFound = True
                            Exit For
                        End If
                        For Each chld As XmlNode In tmp.ChildNodes
                            If LCase(child.Name) = LCase(nodeName) Then
                                node = child : bFound = True
                                Exit For
                            End If
                        Next
                    Else
                        If LCase(child.Name) = LCase(nodeName) Then
                            node = child : bFound = True
                            Exit For
                        End If
                    End If
                    If bFound Then Exit For
                Next
            End If

            Return node

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(Enums.ProjectName.CommonCoreShared))
            Return Nothing
        End Try
    End Function

    'Private Function GetXmlPaths(ByVal ctrl As Control) As List(Of String)
    '    Dim retVal As New List(Of String)

    '    For Each ctr As Control In ctrl.Controls
    '        If ctr.HasControls Then
    '            Dim tmp As List(Of String) = GetXmlPaths(ctr)
    '            For Each s As String In tmp
    '                If Not retVal.Contains(s) Then retVal.Add(s)
    '            Next
    '        Else
    '            If TypeOf ctr Is Controls.TextBoxes.Base Then
    '                If CType(ctr, Controls.TextBoxes.Base).XmlPath.Trim <> "" Then
    '                    If Not retVal.Contains(CType(ctr, Controls.TextBoxes.Base).XmlPath.Trim) Then retVal.Add(CType(ctr, Controls.TextBoxes.Base).XmlPath.Trim)
    '                End If

    '            ElseIf TypeOf ctr Is Controls.TextBoxes.NumericTextBox Then
    '                If CType(ctr, Controls.TextBoxes.NumericTextBox).XmlPath.Trim <> "" Then
    '                    If Not retVal.Contains(CType(ctr, Controls.TextBoxes.NumericTextBox).XmlPath.Trim) Then retVal.Add(CType(ctr, Controls.TextBoxes.NumericTextBox).XmlPath.Trim)
    '                End If

    '            ElseIf TypeOf ctr Is Controls.TextBoxes.DateTextBox Then
    '                If CType(ctr, Controls.TextBoxes.DateTextBox).XmlPath.Trim <> "" Then
    '                    If Not retVal.Contains(CType(ctr, Controls.TextBoxes.DateTextBox).XmlPath.Trim) Then retVal.Add(CType(ctr, Controls.TextBoxes.DateTextBox).XmlPath.Trim)
    '                End If

    '            ElseIf TypeOf ctr Is Controls.TextBoxes.MaskedTextBox Then
    '                If CType(ctr, Controls.TextBoxes.MaskedTextBox).XmlPath.Trim <> "" Then
    '                    If Not retVal.Contains(CType(ctr, Controls.TextBoxes.MaskedTextBox).XmlPath.Trim) Then retVal.Add(CType(ctr, Controls.TextBoxes.MaskedTextBox).XmlPath.Trim)
    '                End If

    '            ElseIf TypeOf ctr Is Controls.Labels.Base Then
    '                If CType(ctr, Controls.Labels.DataLabel).XmlPath.Trim <> "" Then
    '                    If Not retVal.Contains(CType(ctr, Controls.Labels.DataLabel).XmlPath.Trim) Then retVal.Add(CType(ctr, Controls.Labels.DataLabel).XmlPath.Trim)
    '                End If

    '            ElseIf TypeOf ctr Is Controls.CheckBoxes.Base Then
    '                If CType(ctr, Controls.CheckBoxes.CheckBox).XmlPath.Trim <> "" Then
    '                    If Not retVal.Contains(CType(ctr, Controls.CheckBoxes.CheckBox).XmlPath.Trim) Then retVal.Add(CType(ctr, Controls.CheckBoxes.CheckBox).XmlPath.Trim)
    '                End If

    '            ElseIf TypeOf ctr Is Controls.RadioButtons.Base Then
    '                If CType(ctr, Controls.RadioButtons.RadioButton).XmlPath.Trim <> "" Then
    '                    If Not retVal.Contains(CType(ctr, Controls.RadioButtons.RadioButton).XmlPath.Trim) Then retVal.Add(CType(ctr, Controls.RadioButtons.RadioButton).XmlPath.Trim)
    '                End If

    '            ElseIf TypeOf ctr Is Controls.DropDownLists.Base Then
    '                If CType(ctr, Controls.DropDownLists.Base).XmlPath.Trim <> "" Then
    '                    If Not retVal.Contains(CType(ctr, Controls.DropDownLists.Base).XmlPath.Trim) Then retVal.Add(CType(ctr, Controls.DropDownLists.Base).XmlPath.Trim)
    '                End If
    '            End If
    '        End If
    '    Next

    '    Return retVal
    'End Function

    Public Function UpdateHashtableValues(ByVal HashtableWithOldValues As Hashtable, ByVal HashtableWithNewValues As Hashtable) As Hashtable
        Try
            Dim htReturn As Hashtable
            If HashtableWithOldValues IsNot Nothing Then
                htReturn = HashtableWithOldValues
                For Each key In HashtableWithNewValues.Keys
                    If HashtableWithOldValues.ContainsKey(key) Then
                        If TypeOf HashtableWithNewValues(key) Is Hashtable Then
                            If TypeOf htReturn(key) Is Hashtable Then
                                htReturn(key) = UpdateHashtableValues(CType(htReturn(key), Hashtable), CType(HashtableWithNewValues(key), Hashtable))
                            Else : htReturn(key) = CType(HashtableWithNewValues(key), Hashtable)
                            End If
                        Else : htReturn(key) = HashtableWithNewValues(key) 'htReturn = UpdateHashtableItem(htReturn, key.tostring, IIf(Not HashtableWithNewValues(key) Is Nothing, HashtableWithNewValues(key), "").tostring)
                        End If
                    Else : htReturn.Add(key, HashtableWithNewValues(key))
                    End If
                Next
            Else : htReturn = HashtableWithNewValues
            End If

            Return htReturn

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(Enums.ProjectName.CommonCoreShared))
            Return New Hashtable
        End Try
    End Function

    Private Function GridFromHashtable(ByVal data As Hashtable, ByVal id As String) As Hashtable
        Try
            Dim tmp As New Hashtable
            For Each item In data.Keys
                If item.ToString.StartsWith(id) Then
                    tmp.Add(Right(item.ToString, (Len(item) - Len(id)) - 1), data(item))
                End If
            Next

            Dim grid As New Hashtable
            For Each item In tmp.Keys
                Dim currentRow() As String = item.ToString.Split(CChar("_"))
                If grid(currentRow(0)) Is Nothing Then
                    grid.Add(currentRow(0), New Hashtable)
                End If
            Next

            For Each item In tmp.Keys
                Dim currentRow() As String = item.ToString.Split(CChar("_"))
                Dim gridRow As Hashtable = CType(grid(currentRow(0)), Hashtable)

                If gridRow(currentRow(1)) Is Nothing Then
                    gridRow.Add(currentRow(1) & "_" & currentRow(2), tmp(item).ToString)
                End If
            Next

            Return grid

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(Enums.ProjectName.CommonCoreShared))
            Return New Hashtable
        End Try
    End Function

End Module
