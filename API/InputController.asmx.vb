Imports System.Web.Services
Imports System.ComponentModel
Imports DailyDocket.CommonCore.Common
Imports System.Xml
Imports System.Xml.Serialization
Imports DailyDocket.CommonCore.Shared.Enums

' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
<System.Web.Script.Services.ScriptService()>
<System.Web.Services.WebService(Namespace:="https://api.dailycourtdocket.com/")>
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)>
<ToolboxItem(False)>
Public Class InputController
    Inherits System.Web.Services.WebService

#Region "Common Routines"

    <WebMethod()>
    Public Function GetApiKey(ByVal userEmail As String, ByVal userPassword As String) As ApiKeyResult
        Dim retVal As New ApiKeyResult(userEmail, userPassword)

        If retVal.ApiKey = "" Then
            retVal.responseCode = Enums.ApiResultCode.failed
            retVal.responseMessage = "User not authorized to access service"
        End If

        Return retVal
    End Function

#End Region

    <WebMethod()>
    Public Function UpdateFileRecord(ByVal req As ApiRequest, ByVal fRecord As FileRecord) As ApiResponse
        Dim retVal As New ApiResponse

        retVal.responseObject = True

        Try
            If req.apiKey <> "" Then
                Dim apiUsr As New SystemUser("", "", req.apiKey)

                retVal.responseObject = fRecord.Save
            Else
                retVal.responseCode = Enums.ApiResultCode.failed
                retVal.responseMessage = "No API Key Provided"
                retVal.responseObject = Nothing
            End If

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(Enums.ProjectName.API))
            retVal.responseCode = Enums.ApiResultCode.failed
            retVal.responseMessage = ex.Message
            retVal.responseObject = False
        End Try

        Return retVal
    End Function

    <WebMethod()>
    Public Function CreateFileRecord(ByVal req As ApiRequest, ByVal fRecord As FileRecord) As ApiResponse
        Dim retVal As New ApiResponse

        retVal.responseObject = True

        Try
            If req.apiKey <> "" Then
                Dim apiUsr As New SystemUser("", "", req.apiKey)

                Dim cr As FileRecord = fRecord.Save
                If cr.ID > 0 Then
                    retVal.responseObject = cr
                Else
                    retVal.responseCode = Enums.ApiResultCode.failed
                    retVal.responseMessage = "Failed to Save New File Record"
                    retVal.responseObject = Nothing
                End If
            Else
                retVal.responseCode = Enums.ApiResultCode.failed
                retVal.responseMessage = "No API Key Provided"
                retVal.responseObject = Nothing
            End If

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(Enums.ProjectName.API))
            retVal.responseCode = Enums.ApiResultCode.failed
            retVal.responseMessage = ex.Message
            retVal.responseObject = False
        End Try

        Return retVal
    End Function

    <WebMethod()>
    Public Function DeleteFileRecord(ByVal req As ApiRequest, ByVal fRecord As FileRecord) As ApiResponse
        Dim retVal As New ApiResponse

        retVal.responseObject = True

        Try
            If req.apiKey <> "" Then
                Dim apiUsr As New SystemUser("", "", req.apiKey)

                retVal.responseObject = fRecord.Delete
                If Not retVal.responseObject.ToString.ToBoolean Then
                    retVal.responseCode = Enums.ApiResultCode.failed
                    retVal.responseMessage = "Failed to Delete File Record"
                    retVal.responseObject = False
                End If
            Else
                retVal.responseCode = Enums.ApiResultCode.failed
                retVal.responseMessage = "No API Key Provided"
                retVal.responseObject = Nothing
            End If

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(Enums.ProjectName.API))
            retVal.responseCode = Enums.ApiResultCode.failed
            retVal.responseMessage = ex.Message
            retVal.responseObject = False
        End Try

        Return retVal
    End Function

    <WebMethod()>
    Public Function UpdateAttorneyRecord(ByVal req As ApiRequest, ByVal aRecord As AttorneyRecord) As ApiResponse
        Dim retVal As New ApiResponse

        retVal.responseObject = True

        Try
            If req.apiKey <> "" Then
                Dim apiUsr As New SystemUser("", "", req.apiKey)

                retVal.responseObject = aRecord.Save
            Else
                retVal.responseCode = Enums.ApiResultCode.failed
                retVal.responseMessage = "No API Key Provided"
                retVal.responseObject = Nothing
            End If

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(Enums.ProjectName.API))
            retVal.responseCode = Enums.ApiResultCode.failed
            retVal.responseMessage = ex.Message
            retVal.responseObject = False
        End Try

        Return retVal
    End Function

    <WebMethod()>
    Public Function CreateAttorneyRecord(ByVal req As ApiRequest, ByVal aRecord As AttorneyRecord) As ApiResponse
        Dim retVal As New ApiResponse

        retVal.responseObject = True

        Try
            If req.apiKey <> "" Then
                Dim apiUsr As New SystemUser("", "", req.apiKey)

                Dim cr As AttorneyRecord = aRecord.Save
                If cr.ID > 0 Then
                    retVal.responseObject = cr
                Else
                    retVal.responseCode = Enums.ApiResultCode.failed
                    retVal.responseMessage = "Failed to Save New Attorney Record"
                    retVal.responseObject = Nothing
                End If
            Else
                retVal.responseCode = Enums.ApiResultCode.failed
                retVal.responseMessage = "No API Key Provided"
                retVal.responseObject = Nothing
            End If

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(Enums.ProjectName.API))
            retVal.responseCode = Enums.ApiResultCode.failed
            retVal.responseMessage = ex.Message
            retVal.responseObject = False
        End Try

        Return retVal
    End Function

    <WebMethod()>
    Public Function DeleteAttorneyRecord(ByVal req As ApiRequest, ByVal aRecord As AttorneyRecord) As ApiResponse
        Dim retVal As New ApiResponse

        retVal.responseObject = True

        Try
            If req.apiKey <> "" Then
                Dim apiUsr As New SystemUser("", "", req.apiKey)

                retVal.responseObject = aRecord.Delete
                If Not retVal.responseObject.ToString.ToBoolean Then
                    retVal.responseCode = Enums.ApiResultCode.failed
                    retVal.responseMessage = "Failed to Delete Attorney Record"
                    retVal.responseObject = False
                End If
            Else
                retVal.responseCode = Enums.ApiResultCode.failed
                retVal.responseMessage = "No API Key Provided"
                retVal.responseObject = Nothing
            End If

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(Enums.ProjectName.API))
            retVal.responseCode = Enums.ApiResultCode.failed
            retVal.responseMessage = ex.Message
            retVal.responseObject = False
        End Try

        Return retVal
    End Function

    <WebMethod()>
    Public Function UpdateClientRecord(ByVal req As ApiRequest, ByVal cRecord As ClientRecord) As ApiResponse
        Dim retVal As New ApiResponse

        retVal.responseObject = True

        Try
            If req.apiKey <> "" Then
                Dim apiUsr As New SystemUser("", "", req.apiKey)

                retVal.responseObject = cRecord.Save
            Else
                retVal.responseCode = Enums.ApiResultCode.failed
                retVal.responseMessage = "No API Key Provided"
                retVal.responseObject = Nothing
            End If

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(Enums.ProjectName.API))
            retVal.responseCode = Enums.ApiResultCode.failed
            retVal.responseMessage = ex.Message
            retVal.responseObject = False
        End Try

        Return retVal
    End Function

    <WebMethod()>
    Public Function CreateClientRecord(ByVal req As ApiRequest, ByVal cRecord As ClientRecord) As ApiResponse
        Dim retVal As New ApiResponse

        retVal.responseObject = True

        Try
            If req.apiKey <> "" Then
                Dim apiUsr As New SystemUser("", "", req.apiKey)

                Dim cr As ClientRecord = cRecord.Save
                If cr.ID > 0 Then
                    retVal.responseObject = cr
                Else
                    retVal.responseCode = Enums.ApiResultCode.failed
                    retVal.responseMessage = "Failed to Save New Attorney Record"
                    retVal.responseObject = Nothing
                End If
            Else
                retVal.responseCode = Enums.ApiResultCode.failed
                retVal.responseMessage = "No API Key Provided"
                retVal.responseObject = Nothing
            End If

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(Enums.ProjectName.API))
            retVal.responseCode = Enums.ApiResultCode.failed
            retVal.responseMessage = ex.Message
            retVal.responseObject = False
        End Try

        Return retVal
    End Function

    <WebMethod()>
    Public Function DeleteClientRecord(ByVal req As ApiRequest, ByVal cRecord As ClientRecord) As ApiResponse
        Dim retVal As New ApiResponse

        retVal.responseObject = True

        Try
            If req.apiKey <> "" Then
                Dim apiUsr As New SystemUser("", "", req.apiKey)

                retVal.responseObject = cRecord.Delete
                If Not retVal.responseObject.ToString.ToBoolean Then
                    retVal.responseCode = Enums.ApiResultCode.failed
                    retVal.responseMessage = "Failed to Delete Attorney Record"
                    retVal.responseObject = False
                End If
            Else
                retVal.responseCode = Enums.ApiResultCode.failed
                retVal.responseMessage = "No API Key Provided"
                retVal.responseObject = Nothing
            End If

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(Enums.ProjectName.API))
            retVal.responseCode = Enums.ApiResultCode.failed
            retVal.responseMessage = ex.Message
            retVal.responseObject = False
        End Try

        Return retVal
    End Function


    <WebMethod()> Public Function SendMessage(ByVal req As ApiRequest, ByVal msg As String, ByVal userNumber As String, ByVal type As TransactionType) As ApiResponse

    End Function




#Region "System Routines"

    <WebMethod()>
    Public Function UpdateUserRecord(ByVal req As ApiRequest, ByVal usr As SystemUser) As ApiResponse
        Dim retVal As New ApiResponse

        retVal.responseObject = True

        Try
            If req.apiKey <> "" Then
                Dim apiUsr As New SystemUser("", "", req.apiKey)

                retVal.responseObject = usr.Save
            Else
                retVal.responseCode = Enums.ApiResultCode.failed
                retVal.responseMessage = "No API Key Provided"
                retVal.responseObject = Nothing
            End If

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(Enums.ProjectName.API))
            retVal.responseCode = Enums.ApiResultCode.failed
            retVal.responseMessage = ex.Message
            retVal.responseObject = False
        End Try

        Return retVal
    End Function

    <WebMethod()>
    Public Function CreateUserRecord(ByVal req As ApiRequest, ByVal usr As SystemUser) As ApiResponse
        Dim retVal As New ApiResponse

        retVal.responseObject = True

        Try
            If req.apiKey <> "" Then
                Dim apiUsr As New SystemUser("", "", req.apiKey)

                Dim obj As SystemUser = usr.Save
                If obj.ID > 0 Then
                    retVal.responseObject = obj
                Else
                    retVal.responseCode = Enums.ApiResultCode.failed
                    retVal.responseMessage = "Failed to Save New User Record"
                    retVal.responseObject = Nothing
                End If
            Else
                retVal.responseCode = Enums.ApiResultCode.failed
                retVal.responseMessage = "No API Key Provided"
                retVal.responseObject = Nothing
            End If

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(Enums.ProjectName.API))
            retVal.responseCode = Enums.ApiResultCode.failed
            retVal.responseMessage = ex.Message
            retVal.responseObject = False
        End Try

        Return retVal
    End Function

    <WebMethod()>
    Public Function DeleteUserRecord(ByVal req As ApiRequest, ByVal usr As SystemUser) As ApiResponse
        Dim retVal As New ApiResponse

        retVal.responseObject = True

        Try
            If req.apiKey <> "" Then
                Dim apiUsr As New SystemUser("", "", req.apiKey)

                retVal.responseObject = usr.Delete
                If Not retVal.responseObject.ToString.ToBoolean Then
                    retVal.responseCode = Enums.ApiResultCode.failed
                    retVal.responseMessage = "Failed to Delete User Record"
                    retVal.responseObject = False
                End If
            Else
                retVal.responseCode = Enums.ApiResultCode.failed
                retVal.responseMessage = "No API Key Provided"
                retVal.responseObject = Nothing
            End If

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(Enums.ProjectName.API))
            retVal.responseCode = Enums.ApiResultCode.failed
            retVal.responseMessage = ex.Message
            retVal.responseObject = False
        End Try

        Return retVal
    End Function

#End Region

End Class