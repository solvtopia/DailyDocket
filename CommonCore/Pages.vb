Imports System.Web.UI
Imports DailyDocket.CommonCore.Shared.Enums

Public Class builderPage
    Inherits System.Web.UI.Page

#Region "Master Event Handlers"

    Public Sub Layout_NewModuleClicked(ByVal folderId As Integer)
        ShowModuleWizard(0, folderId, Enums.SystemModuleType.Module)
    End Sub

    Public Sub Layout_ShowModule(ByVal modId As Integer)
        ShowModule(modId)
    End Sub

    Public Sub Layout_FolderClicked(ByVal editId As Integer)
        ShowModuleWizard(editId, 0, Enums.SystemModuleType.Folder)
    End Sub

#End Region

#Region "Workers"

    Public Sub ShowModuleWizard(ByVal id As Integer, ByVal folderId As Integer, ByVal type As Enums.SystemModuleType)
        Try
            Dim url As String = "~/account/ModuleWizard.aspx?id=" & id & "&fid=" & folderId & "&t=" & CStr(type)
            Response.Redirect(url, False)

        Catch ex As Exception
            Exit Sub
        End Try
    End Sub

    Public Sub ShowModule(ByVal id As Integer)
        Try
            Dim url As String = "~/account/ModuleLandingPage.aspx?modid=" & id
            Response.Redirect(url, False)

        Catch ex As Exception
            Exit Sub
        End Try
    End Sub

    Public Sub ShowProfile()
        Try
            Dim url As String = "~/account/Profile.aspx?id=" & App.CurrentUser.ID
            Response.Redirect(url, False)

        Catch ex As Exception
            Exit Sub
        End Try
    End Sub

    Public Sub ShowInformationPopup(ByVal ex As Exception)
        ShowInformationPopup(Enums.InformationPopupType.ErrorMessage, Enums.InformationPopupButtons.OkOnly, Enums.InformationPopupIcon.Error, ex.Message, -1)
    End Sub
    Public Sub ShowInformationPopup(ByVal buttons As Enums.InformationPopupButtons)
        Dim icon As Enums.InformationPopupIcon = Enums.InformationPopupIcon.Information

        Select Case buttons
            Case Enums.InformationPopupButtons.YesNo
                icon = Enums.InformationPopupIcon.Question
            Case Enums.InformationPopupButtons.OkCancel, Enums.InformationPopupButtons.OkOnly
                icon = Enums.InformationPopupIcon.Information
        End Select

        ShowInformationPopup(Enums.InformationPopupType.InformationOnly, buttons, icon, "", -1)
    End Sub
    Public Sub ShowInformationPopup(ByVal type As Enums.InformationPopupType, ByVal buttons As Enums.InformationPopupButtons)
        Dim icon As Enums.InformationPopupIcon = Enums.InformationPopupIcon.Information

        Select Case buttons
            Case Enums.InformationPopupButtons.YesNo
                icon = Enums.InformationPopupIcon.Question
            Case Enums.InformationPopupButtons.OkCancel, Enums.InformationPopupButtons.OkOnly
                icon = Enums.InformationPopupIcon.Information
        End Select

        ShowInformationPopup(type, buttons, icon, "", -1)
    End Sub
    Public Sub ShowInformationPopup(ByVal type As Enums.InformationPopupType, ByVal buttons As Enums.InformationPopupButtons, ByVal editId As Integer)
        Dim icon As Enums.InformationPopupIcon = Enums.InformationPopupIcon.Information

        Select Case buttons
            Case Enums.InformationPopupButtons.YesNo
                icon = Enums.InformationPopupIcon.Question
            Case Enums.InformationPopupButtons.OkCancel, Enums.InformationPopupButtons.OkOnly
                icon = Enums.InformationPopupIcon.Information
        End Select

        ShowInformationPopup(type, buttons, icon, "", editId)
    End Sub
    Public Sub ShowInformationPopup(ByVal type As Enums.InformationPopupType, ByVal buttons As Enums.InformationPopupButtons, ByVal icon As Enums.InformationPopupIcon, ByVal msg As String, ByVal editId As Integer)
        Try
            Dim url As String = "~/includes/Confirmation.aspx"

            url &= "?t=" & CStr(type) & "&b=" & CStr(buttons) & "&i=" & CStr(icon) & "&m=" & Server.UrlEncode(msg) & "&id=" & editId
            Response.Redirect(url, False)

        Catch ex As Exception
            Exit Sub
        End Try
    End Sub

    Public Sub SetSkin(ByVal ctrl As Control, ByVal skinName As String)
        Try
            If Not ctrl Is Nothing Then
                For Each ctr As Control In ctrl.Controls
                    If TypeOf ctr Is Controls.TextBoxes.Base Then
                        Dim ctl As Controls.TextBoxes.TextBox = CType(ctr, Controls.TextBoxes.TextBox)
                        ctl.Skin = skinName

                    ElseIf TypeOf ctr Is Controls.TextBoxes.NumericTextBox Then
                        Dim ctl As Controls.TextBoxes.NumericTextBox = CType(ctr, Controls.TextBoxes.NumericTextBox)
                        ctl.Skin = skinName

                    ElseIf TypeOf ctr Is Controls.TextBoxes.DateTextBox Then
                        Dim ctl As Controls.TextBoxes.DateTextBox = CType(ctr, Controls.TextBoxes.DateTextBox)
                        ctl.Skin = skinName

                    ElseIf TypeOf ctr Is Controls.TextBoxes.MaskedTextBox Then
                        Dim ctl As Controls.TextBoxes.MaskedTextBox = CType(ctr, Controls.TextBoxes.MaskedTextBox)
                        ctl.Skin = skinName

                    ElseIf TypeOf ctr Is Controls.RadioButtons.Base Then
                        Dim ctl As Controls.RadioButtons.RadioButton = CType(ctr, Controls.RadioButtons.RadioButton)
                        ctl.Skin = skinName

                    ElseIf TypeOf ctr Is Controls.CheckBoxes.Base Then
                        Dim ctl As Controls.CheckBoxes.CheckBox = CType(ctr, Controls.CheckBoxes.CheckBox)
                        ctl.Skin = skinName

                    ElseIf TypeOf ctr Is Controls.DropDownLists.Base Then
                        Dim ctl As Controls.DropDownLists.DropDownList = CType(ctr, Controls.DropDownLists.DropDownList)
                        ctl.Skin = skinName

                    ElseIf TypeOf ctr Is Telerik.Web.UI.RadTextBox Then
                        Dim ctl As Telerik.Web.UI.RadTextBox = CType(ctr, Telerik.Web.UI.RadTextBox)
                        ctl.Skin = skinName

                    ElseIf TypeOf ctr Is Telerik.Web.UI.RadComboBox Then
                        Dim ctl As Telerik.Web.UI.RadComboBox = CType(ctr, Telerik.Web.UI.RadComboBox)
                        ctl.Skin = skinName

                    ElseIf TypeOf ctr Is Telerik.Web.UI.RadTimePicker Then
                        Dim ctl As Telerik.Web.UI.RadTimePicker = CType(ctr, Telerik.Web.UI.RadTimePicker)
                        ctl.Skin = skinName

                    ElseIf TypeOf ctr Is Telerik.Web.UI.RadDatePicker Then
                        Dim ctl As Telerik.Web.UI.RadDatePicker = CType(ctr, Telerik.Web.UI.RadDatePicker)
                        ctl.Skin = skinName

                    ElseIf TypeOf ctr Is Telerik.Web.UI.RadCalendar Then
                        Dim ctl As Telerik.Web.UI.RadCalendar = CType(ctr, Telerik.Web.UI.RadCalendar)
                        ctl.Skin = skinName

                    ElseIf TypeOf ctr Is Telerik.Web.UI.RadButton Then
                        Dim ctl As Telerik.Web.UI.RadButton = CType(ctr, Telerik.Web.UI.RadButton)
                        ctl.Skin = skinName

                    ElseIf TypeOf ctr Is Telerik.Web.UI.RadGrid Then
                        Dim ctl As Telerik.Web.UI.RadGrid = CType(ctr, Telerik.Web.UI.RadGrid)
                        ctl.Skin = skinName

                    ElseIf TypeOf ctr Is Telerik.Web.UI.RadFormDecorator Then
                        Dim ctl As Telerik.Web.UI.RadFormDecorator = CType(ctr, Telerik.Web.UI.RadFormDecorator)
                        ctl.Skin = skinName

                    ElseIf TypeOf ctr Is Telerik.Web.UI.RadScheduler Then
                        Dim ctl As Telerik.Web.UI.RadScheduler = CType(ctr, Telerik.Web.UI.RadScheduler)
                        ctl.Skin = skinName

                    ElseIf TypeOf ctr Is Telerik.Web.UI.RadTabStrip Then
                        Dim ctl As Telerik.Web.UI.RadTabStrip = CType(ctr, Telerik.Web.UI.RadTabStrip)
                        ctl.Skin = skinName

                    ElseIf TypeOf ctr Is Telerik.Web.UI.RadMultiPage Then
                        Dim ctl As Telerik.Web.UI.RadMultiPage = CType(ctr, Telerik.Web.UI.RadMultiPage)
                        ctl.Skin = skinName

                    ElseIf TypeOf ctr Is Telerik.Web.UI.RadDropDownList Then
                        Dim ctl As Telerik.Web.UI.RadDropDownList = CType(ctr, Telerik.Web.UI.RadDropDownList)
                        ctl.Skin = skinName

                    ElseIf TypeOf ctr Is Telerik.Web.UI.RadPanelBar Then
                        'Dim ctl As Telerik.Web.UI.RadPanelBar = CType(ctr, Telerik.Web.UI.RadPanelBar)
                        'ctl.Skin = skinName

                    ElseIf TypeOf ctr Is Telerik.Web.UI.RadListBox Then
                        Dim ctl As Telerik.Web.UI.RadListBox = CType(ctr, Telerik.Web.UI.RadListBox)
                        ctl.Skin = skinName

                    ElseIf TypeOf ctr Is Telerik.Web.UI.RadHtmlChart Then
                        Dim ctl As Telerik.Web.UI.RadHtmlChart = CType(ctr, Telerik.Web.UI.RadHtmlChart)
                        ctl.Skin = skinName

                    ElseIf TypeOf ctr Is Telerik.Web.UI.RadFormDecorator Then
                        Dim ctl As Telerik.Web.UI.RadFormDecorator = CType(ctr, Telerik.Web.UI.RadFormDecorator)
                        ctl.Skin = skinName

                    Else
                        If ctr.HasControls OrElse ctr.Controls.Count > 0 Then
                            SetSkin(ctr, skinName)
                        Else
                        End If

                    End If
                Next
            End If

        Catch ex As Exception
            Dim s As String = ""
        End Try
    End Sub

#End Region

#Region "Properties"

    Public ReadOnly Property DeviceId As String
        Get
            Return Request.QueryString("deviceid")
        End Get
    End Property
    Public ReadOnly Property UserId As Integer
        Get
            Return Request.QueryString("uid").ToInteger
        End Get
    End Property
    Public ReadOnly Property SignOut As Boolean
        Get
            Return Request.QueryString("signout").ToBoolean
        End Get
    End Property

#End Region


    Private Sub builderPage_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
tryAgain:
            Dim signoutInt As Integer = If(Me.SignOut, 1, 0)
            If App.CurrentUser.ID = 0 And CommonCore.Common.ShowLogin(Request.Url.AbsolutePath) Then
                If App.CurrentUser.ID = 0 And (Me.DeviceId <> "" Or Me.UserId > 0) And Not Me.SignOut Then
                    If Me.DeviceId <> "" Then
                        ' device id passed from the app
                        Dim usr As New SystemUser(Me.DeviceId)
                        If usr.ID > 0 Then
                            ' only save if we have a device id
                            usr.MobileDeviceType = Me.UserPlatform
                            App.CurrentUser = usr.Save
                        End If
                    Else
                        ' no device id so try the user id from the web
                        App.CurrentUser = New SystemUser(Me.UserId)
                    End If
                    App.CurrentAttorney = New AttorneyRecord(App.CurrentUser.AttorneyID)
                    App.CurrentClient = New SystemClient(App.CurrentUser.ClientID)

                    If App.CurrentUser.ID > 0 Then
                        LogHistory("User Login " & App.CurrentUser.Name)
                        GoTo tryAgain
                    Else
                        Dim usrString As String = If(Me.DeviceId <> "", "deviceid=" & Me.DeviceId, "uid=" & Me.UserId)
                        Response.Redirect("~/account/login.aspx?" & usrString, False)
                    End If
                Else
                    Dim usrString As String = If(Me.DeviceId <> "", "deviceid=" & Me.DeviceId, "uid=" & Me.UserId)
                    Response.Redirect("~/account/login.aspx?" & usrString & "&signout=" & signoutInt, False)
                End If
            End If
        End If

        ' hide the register link on mobile ... because Apple is stupid!!
        'If Me.FindInControl("lnkRegister") IsNot Nothing Then
        '    Dim lnk As WebControls.LinkButton = CType(Me.FindInControl("lnkRegister"), WebControls.LinkButton)
        '    lnk.Visible = Not Me.OnMobile
        'End If

        If Me.FindInControl("lblSSL") IsNot Nothing Then
            Dim lbl As WebControls.Label = CType(Me.FindInControl("lblSSL"), WebControls.Label)
            lbl.Text = Common.GetApplicationAssembly(Me.Context).GetName.Version.ToString & If(Me.OnPhone, "M", If(Me.OnTablet, "T", "D")) & If(Request.Url.Scheme.ToLower = "https", "s", "u")
        End If

        If Me.OnMobile Then
            Me.SetSkin(Me, System.Configuration.ConfigurationManager.AppSettings("Telerik_Skin_Mobile"))
        Else Me.SetSkin(Me, System.Configuration.ConfigurationManager.AppSettings("Telerik_Skin_Desktop"))
        End If
    End Sub
End Class

Public Class builderMasterPage
    Inherits System.Web.UI.MasterPage

    Private Sub builderMasterPage_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Me.FindInControl("lblMasterSSL") IsNot Nothing Then
            Dim lbl As WebControls.Label = CType(Me.FindInControl("lblMasterSSL"), WebControls.Label)
            lbl.Text = Common.GetApplicationAssembly(Me.Context).GetName.Version.ToString & If(Me.Page.OnPhone, "M", If(Me.Page.OnTablet, "T", "D")) & If(Request.Url.Scheme.ToLower = "https", "s", "u")
        End If
    End Sub
End Class