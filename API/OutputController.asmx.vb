Imports System.Web.Services
Imports System.ComponentModel
Imports DailyDocket.CommonCore.Common
Imports System.Xml.Serialization
Imports System.Xml

' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
<System.Web.Script.Services.ScriptService()>
<System.Web.Services.WebService(Namespace:="https://api.dailycourtdocket.com/")>
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)>
<ToolboxItem(False)>
Public Class OutputController
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
    Public Function GetAttorneyByBarNumber(ByVal req As ApiRequest, ByVal barNumber As String) As ApiResponse
        Dim retVal As New ApiResponse

        Try
            If req.apiKey <> "" Then
                Dim apiUsr As New SystemUser("", "", req.apiKey)

                retVal.responseObject = New AttorneyRecord(barNumber).ID
            Else
                retVal.responseCode = Enums.ApiResultCode.failed
                retVal.responseMessage = "No API Key Provided"
                retVal.responseObject = Nothing
            End If

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(Enums.ProjectName.API))
            retVal.responseCode = Enums.ApiResultCode.failed
            retVal.responseMessage = ex.Message
            retVal.responseObject = Nothing
        End Try

        Return retVal
    End Function

End Class