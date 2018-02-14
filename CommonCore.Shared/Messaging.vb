Imports System.IO
Imports System.Net
Imports System.Text
Imports Twilio
Imports Twilio.Rest.Api.V2010.Account
Imports Twilio.Types
Public Class Messaging

#Region "OneSignal"

    'Public Shared Function SendOneSignalNotification(ByVal usr As SystemUser, ByVal type As Enums.NotificationType) As Boolean
    '    Return SendOneSignalNotification(usr, type, "")
    'End Function
    'Public Shared Function SendOneSignalNotification(ByVal usr As SystemUser, ByVal msgText As String) As Boolean
    '    Return SendOneSignalNotification(usr, Enums.NotificationType.Custom, msgText)
    'End Function
    'Public Shared Function SendOneSignalNotification(ByVal usr As SystemUser, ByVal type As Enums.NotificationType, ByVal msgText As String) As Boolean
    '    Dim retVal As Boolean = True

    '    Try
    '        If usr.OneSignalUserID <> "" Then
    '            Dim osAppId As String = ""
    '            Dim osRestApiKey As String = ""

    '            Select Case usr.MobileDeviceType
    '                Case Enums.UserPlatform.AndroidPhone, Enums.UserPlatform.AndroidTablet
    '                    osAppId = "d76ee4ac-7375-4537-8575-9bd21db9f81b"
    '                    osRestApiKey = "ZjY5YjE4OWQtYzc3My00NmM2LTllZmMtNjY2NTQ5Y2Y5NTgw"
    '                Case Enums.UserPlatform.iPad, Enums.UserPlatform.iPhone, Enums.UserPlatform.iPod
    '                    osAppId = "1d6c5527-98ce-4eb9-a7a4-8dff903877e1"
    '                    osRestApiKey = "MGIyY2NkNjItM2JlMy00ODY2LThiOGMtOGE5MTRjZmMzZjhk"
    '            End Select

    '            Dim msg As String = ""
    '            Select Case type
    '                Case Enums.NotificationType.NormalWorkOrderCreated
    '                    msg = "New Work Order Created"
    '                Case Enums.NotificationType.EmergencyWorkOrderCreated
    '                    msg = "New Emergency Work Order Created"
    '                Case Enums.NotificationType.WorkOrderAssigned
    '                    msg = "New Work Order Assigned to You"
    '                Case Enums.NotificationType.Custom
    '                    msg = msgText
    '            End Select

    '            Dim request = TryCast(WebRequest.Create("https://onesignal.com/api/v1/notifications"), HttpWebRequest)

    '            request.KeepAlive = True
    '            request.Method = "POST"
    '            request.ContentType = "application/json"

    '            request.Headers.Add("authorization", "Basic " & osRestApiKey)

    '            Dim byteArray As Byte() = Encoding.UTF8.GetBytes("{" + """app_id"": """ & osAppId & """," + """contents"": {""en"": """ & msg & """}," + """include_player_ids"": [""" & usr.OneSignalUserID & """]}")

    '            Dim responseContent As String = Nothing

    '            Try
    '                Using writer = request.GetRequestStream()
    '                    writer.Write(byteArray, 0, byteArray.Length)
    '                End Using

    '                Using response = TryCast(request.GetResponse(), HttpWebResponse)
    '                    Using reader = New StreamReader(response.GetResponseStream())
    '                        responseContent = reader.ReadToEnd()
    '                    End Using
    '                End Using
    '            Catch ex As WebException
    '                ex.WriteToErrorLog(Enums.ProjectName.CommonCoreShared)
    '                retVal = False
    '            End Try
    '        End If

    '    Catch ex As Exception
    '        ex.WriteToErrorLog(Enums.ProjectName.CommonCoreShared)
    '        retVal = False
    '    End Try

    '    Return retVal
    'End Function

#End Region


#Region "Twilio"

    Public Shared Function SendTwilioNotification(ByVal usr As SystemUser, ByVal type As Enums.NotificationType) As Boolean
        Dim retVal As Boolean

        For Each n As String In usr.MobileNumbers
            retVal = SendTwilioNotification(n.FixPhoneNumber, type, "")
        Next

        Return retVal
    End Function
    Public Shared Function SendTwilioNotification(ByVal usr As SystemUser, ByVal msgText As String) As Boolean
        Dim retVal As Boolean

        For Each n As String In usr.MobileNumbers
            retVal = SendTwilioNotification(n.FixPhoneNumber, Enums.NotificationType.Custom, msgText)
        Next

        Return retVal
    End Function
    Public Shared Function SendTwilioNotification(ByVal usr As SystemUser, ByVal type As Enums.NotificationType, ByVal msgText As String) As Boolean
        Dim retVal As Boolean

        For Each n As String In usr.MobileNumbers
            retVal = SendTwilioNotification(n.FixPhoneNumber, type, msgText)
        Next

        Return retVal
    End Function
    Public Shared Function SendTwilioNotification(ByVal toNumber As String, ByVal msgText As String) As Boolean
        Return SendTwilioNotification(toNumber.FixPhoneNumber, Enums.NotificationType.Custom, msgText)
    End Function
    Public Shared Function SendTwilioNotification(ByVal toNumber As String, ByVal type As Enums.NotificationType, ByVal msgText As String) As Boolean
        Dim retVal As Boolean = True

        Try
            Dim liveSid As String = "AC884029d0f4300e93f5a7b9440213a7ba"
            Dim liveAuthToken As String = "5a7b14902ff02baff64634dac16bdbaf"
            Dim testSid As String = "AC1fe87320d28a211d772975e581941b60"
            Dim testAuthToken As String = "f9c28af31d50ae5c070008ecd6a480b0"
            Dim fromNumber As String = "+19102188036"

            If toNumber <> "" Then
                Try
                    TwilioClient.Init(liveSid, liveAuthToken)

                    Dim txt As String = ""
                    Select Case type
                        Case Enums.NotificationType.Custom
                            txt = msgText
                    End Select

                    Dim msg = MessageResource.Create(to:=New PhoneNumber(toNumber), from:=New PhoneNumber(fromNumber), body:=txt)
                    If msg.Sid <> "" Then

                    End If

                Catch ex As WebException
                    ex.WriteToErrorLog(New ErrorLogEntry(Enums.ProjectName.CommonCoreShared))
                    retVal = False
                End Try
            End If

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(Enums.ProjectName.CommonCoreShared))
            retVal = False
        End Try

        Return retVal
    End Function

#End Region
End Class
