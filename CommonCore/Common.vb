Imports System.Drawing
Imports System.IO
Imports System.Net
Imports System.Reflection
Imports System.Runtime.CompilerServices
Imports System.Web
Imports System.Xml
Imports Newtonsoft.Json

Public Module Common
    Public ReturnToParentScript As String = "returnToParent('True','../Default.aspx');"
    Private Const AspNetNamespace As String = "ASP"

    Public Function GetApplicationAssembly(ctx As HttpContext) As Assembly
        ' Try the EntryAssembly, this doesn't work for ASP.NET classic pipeline (untested on integrated)
        Dim ass As Assembly = Assembly.GetEntryAssembly()

        ' Look for web application assembly
        If ctx IsNot Nothing Then
            ass = GetWebApplicationAssembly(ctx)
        End If

        ' Fallback to executing assembly
        Return If(ass, (Assembly.GetExecutingAssembly()))
    End Function

    Private Function GetWebApplicationAssembly(context As HttpContext) As Assembly
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

    ' sets all variables to blanks (used to initialize new objects)
    Public Function InitializeObject(ByVal o As Object) As Object
        ' get the type of the object we are working with
        Dim objectType As Type = CType(o.GetType, Type)

        ' get a list of properties and fields to loop through
        Dim properties As PropertyInfo() = objectType.GetProperties()
        Dim fields As FieldInfo() = objectType.GetFields()

        ' loop through properties first (this works on compiled objects)
        For Each prop As PropertyInfo In properties
            ' get the value of the property from the xml
            Try
                Select Case True
                    Case prop.PropertyType Is System.Type.GetType("System.String")
                        prop.SetValue(o, "", Nothing)
                    Case prop.PropertyType Is System.Type.GetType("System.Int32"), prop.PropertyType Is System.Type.GetType("System.Int64")
                        prop.SetValue(o, 0, Nothing)
                    Case prop.PropertyType Is System.Type.GetType("System.Double")
                        prop.SetValue(o, 0, Nothing)
                    Case prop.PropertyType Is System.Type.GetType("System.Date"), prop.PropertyType Is System.Type.GetType("System.DateTime")
                        prop.SetValue(o, Now, Nothing)
                    Case prop.PropertyType Is System.Type.GetType("System.Boolean")
                        prop.SetValue(o, False, Nothing)
                    Case prop.PropertyType.FullName.StartsWith("System.Collections.Generic.List")
                        Dim ptn As String = prop.PropertyType.FullName
                        Dim ts As String = ptn.Substring(ptn.IndexOf("[") + 2).Replace("]]", "")
                        Dim t As Type = System.Type.GetType(ts)
                        Dim lst = DirectCast(GetType(List(Of )).MakeGenericType(t).GetConstructor(Type.EmptyTypes).Invoke(Nothing), IList)

                        prop.SetValue(o, lst, Nothing)
                    Case Else
                        Dim t As Type = prop.PropertyType
                        ' skip the complex types
                End Select

            Catch ex As Exception
                ' skip any errors
            End Try
        Next

        ' now loop through the fields (this works on local classes)
        For Each fld As FieldInfo In fields
            ' get the value of the field from the xml
            Try
                Select Case True
                    Case fld.FieldType Is System.Type.GetType("System.String")
                        fld.SetValue(o, "")
                    Case fld.FieldType Is System.Type.GetType("System.Int32"), fld.FieldType Is System.Type.GetType("System.Int64")
                        fld.SetValue(o, 0)
                    Case fld.FieldType Is System.Type.GetType("System.Double")
                        fld.SetValue(o, 0)
                    Case fld.FieldType Is System.Type.GetType("System.Date"), fld.FieldType Is System.Type.GetType("System.DateTime")
                        fld.SetValue(o, Now)
                    Case fld.FieldType Is System.Type.GetType("System.Boolean")
                        fld.SetValue(o, False)
                    Case fld.FieldType.FullName.StartsWith("System.Collections.Generic.List")
                        Dim ftn As String = fld.FieldType.FullName
                        Dim ts As String = ftn.Substring(ftn.IndexOf("[") + 2).Replace("]]", "")
                        Dim t As Type = System.Type.GetType(ts)
                        Dim lst = DirectCast(GetType(List(Of )).MakeGenericType(t).GetConstructor(Type.EmptyTypes).Invoke(Nothing), IList)

                        fld.SetValue(o, lst)
                    Case Else
                        Dim t As Type = fld.FieldType
                        ' skip the complex types
                End Select

            Catch ex As Exception
                ' skip any errors
            End Try
        Next

        Return o
    End Function

    <Extension()> Public Sub CopyObject(ByVal source As Object, ByRef destination As Object)
        Dim typeB As Type = destination.[GetType]()
        For Each [property] As PropertyInfo In source.[GetType]().GetProperties()
            If Not [property].CanRead OrElse ([property].GetIndexParameters().Length > 0) Then
                Continue For
            End If

            Dim other As PropertyInfo = typeB.GetProperty([property].Name)
            If (other IsNot Nothing) AndAlso (other.CanWrite) Then
                other.SetValue(destination, [property].GetValue(source, Nothing), Nothing)
            End If
        Next
    End Sub
    Public Sub LogHistory(ByVal itemText As String)
        Dim cn As New SqlClient.SqlConnection(CommonCore.Shared.Common.ConnectionString)

        Try
            Dim cmd As New SqlClient.SqlCommand("INSERT INTO [Sys_History] ([itemText], [dtInserted], [insertedBy]) VALUES ('" & itemText & "', '" & Now.ToString & "', '" & App.CurrentUser.ID & "');SELECT @@Identity AS SCOPEIDENTITY;", cn)
            If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
            cmd.ExecuteNonQuery()
            cmd.Cancel()

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(Enums.ProjectName.Builder))
        Finally
            cn.Close()
        End Try
    End Sub


    Public Function ShowLogin(ByVal page As String) As Boolean
        Dim retVal As Boolean = True

        ' don't force login for these pages ...
        Select Case True
            Case page.ToLower.EndsWith("login.aspx") : retVal = False
            Case page.ToLower.EndsWith("register.aspx") : retVal = False
            Case page.ToLower.EndsWith("registration.aspx") : retVal = False
            Case page.ToLower.EndsWith("loginhelp.aspx") : retVal = False
            Case page.ToLower.EndsWith("availability.aspx") : retVal = False
            Case page.ToLower.EndsWith("preview.aspx") : retVal = False
        End Select

        Return retVal
    End Function

    ' translate an address to latitude/longitude
    Public Function AddressToLatLon(ByVal address As String) As List(Of String)
        Dim retVal As New List(Of String)

        address = Replace(address, " ", "+")

        Dim lookupResponse As String = GetUrlResponse("http://maps.google.com/maps/api/geocode/xml?address=" & address & "&sensor=false").ReadToEnd
        Dim xDoc As New XmlDocument
        xDoc.LoadXml(lookupResponse)

        retVal.Add(xDoc("GeocodeResponse")("result")("geometry")("location")("lat").InnerText)
        retVal.Add(xDoc("GeocodeResponse")("result")("geometry")("location")("lng").InnerText)

        Return retVal
    End Function

    Public Function GetUrlResponse(ByVal url As String) As StreamReader
        Try
            Dim req As WebRequest = WebRequest.Create(url)
            Dim resp As WebResponse = req.GetResponse()
            Return New StreamReader(resp.GetResponseStream())

        Catch ex As Exception
            Return Nothing
        End Try
    End Function

#Region "JSON Stuff"

    Public Function PostJsonToUrl(ByVal url As String, ByVal json As String) As String
        Dim retVal As String = ""

        Dim httpWebRequest As HttpWebRequest = CType(WebRequest.Create(url), HttpWebRequest)
        httpWebRequest.ContentType = "application/json; charset=utf-8"
        httpWebRequest.Method = "POST"

        httpWebRequest.Headers.Add("x-rsui-user", "guest4@rsuiex.net")
        httpWebRequest.Headers.Add("x-rsui-identifier", "7E10A430-FCFB-4A90-91DB-0D781448AB15")
        Using streamWriter = New System.IO.StreamWriter(httpWebRequest.GetRequestStream())
            streamWriter.Write(json)
            streamWriter.Flush()
        End Using

        Dim httpResponse = DirectCast(httpWebRequest.GetResponse(), HttpWebResponse)
        Using streamReader = New System.IO.StreamReader(httpResponse.GetResponseStream())
            retVal = streamReader.ReadToEnd
        End Using

        Return retVal
    End Function

    Function JsonToXml(ByVal json As String) As XmlDocument
        Dim node As XNode = JsonConvert.DeserializeXNode(json, "Root")

        Dim xDoc As New XmlDocument
        xDoc.LoadXml(node.ToString)

        Return xDoc
    End Function

    Function XmlToJson(ByVal xDoc As XmlDocument) As String
        Dim json As String = JsonConvert.SerializeXmlNode(xDoc)

        Return json
    End Function

#End Region

#Region "Colors"

    Public Function GetHtmlColor(c As Color) As String
        Return "#" + c.R.ToString("X2") + c.G.ToString("X2") + c.B.ToString("X2")
    End Function

    Public Function GetRGBColor(c As System.Drawing.Color) As String
        Return "RGB(" + c.R.ToString() + "," + c.G.ToString() + "," + c.B.ToString() + ")"
    End Function

    Public Function GetColor(hexValue As String) As Color
        Try
            hexValue = hexValue.Replace("#", "")
            Dim position As Integer = 0
            Dim alpha As Byte = System.Convert.ToByte("ff", 16)

            If hexValue.Length = 8 Then
                alpha = System.Convert.ToByte(hexValue.Substring(position, 2), 16)
                position = 2
            End If

            Dim red As Byte = System.Convert.ToByte(hexValue.Substring(position, 2), 16)
            position += 2

            Dim green As Byte = System.Convert.ToByte(hexValue.Substring(position, 2), 16)
            position += 2

            Dim blue As Byte = System.Convert.ToByte(hexValue.Substring(position, 2), 16)
            position += 2

            Return Color.FromArgb(red, green, blue)

        Catch ex As Exception
            Dim s As String = ex.Message
            Return Color.White
        End Try
    End Function

#End Region

End Module
