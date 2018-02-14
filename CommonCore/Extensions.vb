Imports System.CodeDom.Compiler
Imports System.IO
Imports System.Reflection
Imports System.Runtime.CompilerServices
Imports System.Text
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Xml
Imports System.Xml.Serialization
Imports Telerik.Web.UI
Imports System.Web

Public Module Extensions

    <Extension()> Public Function UserPlatform(ByVal pg As System.Web.UI.Page) As Enums.UserPlatform
        Dim retVal As Enums.UserPlatform = Enums.UserPlatform.Unavailable

        If HttpContext.Current.Request.ServerVariables("HTTP_USER_AGENT").ToLower.Contains("iphone") Then
            retVal = Enums.UserPlatform.iPhone
        ElseIf HttpContext.Current.Request.ServerVariables("HTTP_USER_AGENT").ToLower.Contains("ipod") Then
            retVal = Enums.UserPlatform.iPod
        ElseIf HttpContext.Current.Request.ServerVariables("HTTP_USER_AGENT").ToLower.Contains("ipad") Then
            retVal = Enums.UserPlatform.iPad
        ElseIf HttpContext.Current.Request.ServerVariables("HTTP_USER_AGENT").ToLower.Contains("android") And HttpContext.Current.Request.ServerVariables("HTTP_USER_AGENT").ToLower.Contains("mobile") Then
            retVal = Enums.UserPlatform.AndroidPhone
        ElseIf HttpContext.Current.Request.ServerVariables("HTTP_USER_AGENT").ToLower.Contains("android") Then
            retVal = Enums.UserPlatform.AndroidTablet
        ElseIf HttpContext.Current.Request.ServerVariables("HTTP_USER_AGENT").ToLower.Contains("winphone") Then
            retVal = Enums.UserPlatform.WindowsPhone
        Else retval = Enums.UserPlatform.Desktop
        End If

        Return retVal
    End Function

    <Extension()> Public Function OnLocal(ByVal pg As System.Web.UI.Page) As Boolean
        Dim retVal As Boolean

        retVal = HttpContext.Current.Request.Url.ToString.ToLower.Contains("localhost")

        Return retVal
    End Function

    <Extension()> Public Function OnMobile(ByVal pg As System.Web.UI.Page) As Boolean
        Dim retVal As Boolean

        Select Case pg.UserPlatform
            Case Enums.UserPlatform.AndroidPhone, Enums.UserPlatform.AndroidTablet, Enums.UserPlatform.iPad, Enums.UserPlatform.iPhone, Enums.UserPlatform.iPod, Enums.UserPlatform.WindowsPhone
                retVal = True
            Case Else
                retVal = False
        End Select

        Return retVal
    End Function

    <Extension()> Public Function OnPhone(ByVal pg As System.Web.UI.Page) As Boolean
        Dim retVal As Boolean

        Select Case pg.UserPlatform
            Case Enums.UserPlatform.AndroidPhone, Enums.UserPlatform.iPhone, Enums.UserPlatform.iPod, Enums.UserPlatform.WindowsPhone
                retVal = True
            Case Else
                retVal = False
        End Select

        Return retVal
    End Function

    <Extension()> Public Function OnTablet(ByVal pg As System.Web.UI.Page) As Boolean
        Dim retVal As Boolean

        Select Case pg.UserPlatform
            Case Enums.UserPlatform.AndroidTablet, Enums.UserPlatform.iPad
                retVal = True
            Case Else
                retVal = False
        End Select

        Return retVal
    End Function

    <Extension()> Public Sub MsgBox(ByVal pg As System.Web.UI.Page, ByVal message As String, Optional ByVal scriptAfterAlert As String = "")
        Try
            message = Replace(message, "'", """")
            message = Replace(message, vbCrLf, "\r\n")
            message = Replace(message, vbLf, "\r\n")

            Dim myScript As String = "alert('" & message & "');" & scriptAfterAlert

            ' use the script manager to attach a new local script to popup a message box
            ScriptManager.RegisterStartupScript(pg, pg.GetType(), "MsgBoxScript", myScript, True)

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(Enums.ProjectName.CommonCore))
            Exit Sub
        End Try
    End Sub

    <Extension()> Public Sub OpenWindow(ByVal pg As System.Web.UI.Page, ByVal url As String, Optional ByVal height As Integer = 240, Optional ByVal width As Integer = 320, Optional ByVal scrollbars As Boolean = False, Optional ByVal addressbar As Boolean = False, Optional ByVal resizable As Boolean = False, Optional ByVal toolbar As Boolean = False, Optional ByVal status As Boolean = False, Optional ByVal directories As Boolean = False, Optional ByVal menubar As Boolean = False)
        Try
            Dim myScript, sScrollBars, sAddressBar, sResizable, sToolbar, sStatus, sDirectories, sMenuBar As String

            If scrollbars = True Then sScrollBars = "1" Else sScrollBars = "0"
            If addressbar = True Then sAddressBar = "1" Else sAddressBar = "0"
            If resizable = True Then sResizable = "1" Else sResizable = "0"
            If toolbar = True Then sToolbar = "1" Else sToolbar = "0"
            If status = True Then sStatus = "1" Else sStatus = "0"
            If directories = True Then sDirectories = "1" Else sDirectories = "0"
            If menubar = True Then sMenuBar = "1" Else sMenuBar = "0"

            'myScript = "window.open('" & url & "','_blank', 'height=" & height & ",width=" & width & ",scrollbars=" & sScrollBars & ",location=" & sAddressBar & ",resizable=1');"
            myScript = "var newwin = window.open('" & url & "', '_blank', 'scrollbars=" & sScrollBars & ", resizable=" & sResizable & ", width=" & width & ",height=" & height & ", location=" & sAddressBar & ", toolbar=" & sToolbar & ", status=" & sStatus & ",directories=" & sDirectories & ",menubar=" & sMenuBar & "'); if (newwin == null) { alert('\n Your web browser just prevented a pop-up window from opening.  Try holding down the control key and hit the button again.\n'); } else { newwin.focus(); }"

            ' use the script manager to attach a new local script to popup a message box
            ScriptManager.RegisterStartupScript(pg, pg.GetType(), "MyScript", myScript, True)

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(Enums.ProjectName.CommonCore))
            Exit Sub
        End Try
    End Sub

    <Extension()> Public Function FindInControl(ByVal ctrl As Control, ByVal id As String) As Control
        Dim retVal As Control = Nothing

        If Not ctrl Is Nothing Then
            For Each ctr As Control In ctrl.Controls
                Dim tmpID As String = ""
                If ctr.ID Is Nothing Then
                    Try
                        tmpID = ctr.ClientID
                    Catch ex As Exception
                        tmpID = ""
                    End Try
                Else : tmpID = ctr.ID
                End If

                If tmpID.ToLower = id.ToLower Then retVal = ctr : Exit For

                If ctr.HasControls Then
                    Dim tmp As Control = ctr.FindInControl(id)
                    If Not tmp Is Nothing Then retVal = tmp : Exit For
                Else
                End If
            Next
        End If

        Return retVal
    End Function

    <Extension()> Public Sub RunClientScript(ByVal pg As System.Web.UI.Page, ByVal script As String)
        Try
            script = Replace(script, "'", """")

            ' use the script manager to attach a new local script
            If pg IsNot Nothing Then
                ScriptManager.RegisterStartupScript(pg, pg.GetType(), "MyClientScript", script, True)
            End If

        Catch ex As Exception
            Exit Sub
        End Try
    End Sub

    <Extension()> Public Function GetPostBackControl(ByVal pg As Page) As Control
        If Not pg.IsPostBack Then
            Return Nothing
        End If

        Dim control As Control = Nothing
        ' first we will check the "__EVENTTARGET" because if post back made by the controls
        ' which used "_doPostBack" function also available in Request.Form collection.
        Dim controlName As String = pg.Request.Params("__EVENTTARGET")
        If Not [String].IsNullOrEmpty(controlName) Then
            control = pg.FindControl(controlName)
        Else
            ' if __EVENTTARGET is null, the control is a button type and we need to
            ' iterate over the form collection to find it

            ' ReSharper disable TooWideLocalVariableScope
            Dim controlId As String
            Dim foundControl As Control
            ' ReSharper restore TooWideLocalVariableScope

            For Each ctl As String In pg.Request.Form
                ' handle ImageButton they having an additional "quasi-property" 
                ' in their Id which identifies mouse x and y coordinates
                If ctl.EndsWith(".x") OrElse ctl.EndsWith(".y") Then
                    controlId = ctl.Substring(0, ctl.Length - 2)
                    foundControl = pg.FindControl(controlId)
                Else
                    foundControl = pg.FindControl(ctl)
                End If

                If Not (TypeOf foundControl Is Button OrElse TypeOf foundControl Is ImageButton OrElse TypeOf foundControl Is LinkButton) Then
                    Continue For
                End If

                control = foundControl
                Exit For
            Next
        End If

        Return If(control Is Nothing, Nothing, control)
    End Function

    <Extension()> Function InList(ByVal items As RadComboBoxItemCollection, ByVal value As String) As Boolean
        Dim retVal As Boolean = False

        For Each item As RadComboBoxItem In items
            If item.Value.ToLower = value.ToLower Then
                retVal = True
                Exit For
            End If
        Next

        Return retVal
    End Function

End Module
