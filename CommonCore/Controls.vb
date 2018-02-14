Option Strict On

Imports System.ComponentModel
Imports System.Security.Permissions
Imports System.Drawing
Imports System.Web
Imports System.Web.UI

Imports DailyDocket.CommonCore.Enums
Imports System.Web.UI.HtmlControls
Imports DailyDocket.CommonCore.Shared.Enums

Namespace Controls

    Namespace TextBoxes

        ' Inherits Telerik.Web.UI.RadTextBox
        <AspNetHostingPermission(SecurityAction.Demand,
        Level:=AspNetHostingPermissionLevel.Minimal),
        AspNetHostingPermission(SecurityAction.InheritanceDemand,
        Level:=AspNetHostingPermissionLevel.Minimal),
        DefaultProperty("Text"),
        PersistChildren(False),
        ParseChildren(True)>
        Public Class Base
            Inherits Telerik.Web.UI.RadTextBox

            Protected Overrides Sub AddAttributesToRender(ByVal writer As System.Web.UI.HtmlTextWriter)
                MyBase.AddAttributesToRender(writer)

            End Sub

            <Description("Miscellaneous data."),
            PersistenceMode(PersistenceMode.Attribute),
            DefaultValue(""),
            Category("NK5"),
            Bindable(True),
            Localizable(True)>
            Public Property Tag() As String
                Get
                    Dim value As String = CStr(ViewState("Tag"))
                    If value Is Nothing Then
                        Return String.Empty
                    Else
                        Return value
                    End If
                End Get
                Set(ByVal value As String)
                    ViewState("Tag") = value
                End Set
            End Property

            <Description("Sets casing of control data."),
            PersistenceMode(PersistenceMode.Attribute),
            DefaultValue(CasingOption.Normal),
            Category("NK5"),
            Bindable(True),
            Localizable(True)>
            Public Overridable Property Casing() As CasingOption
                Get
                    Dim value As String = CStr(ViewState("Casing"))
                    If (value Is Nothing) OrElse (Not IsNumeric(value)) Then
                        Return CasingOption.Normal
                    Else
                        Return CType(value, CasingOption)
                    End If
                End Get
                Set(ByVal value As CasingOption)
                    ViewState("Casing") = value
                End Set
            End Property

            ''' <summary>
            ''' 
            ''' </summary>
            ''' <returns>Object</returns>
            ''' <remarks></remarks>
            Public Overridable Function DataValue() As Object
                Return Me.Text
            End Function

            <Description("Column name of Xml field when storing data in XML columns."),
            PersistenceMode(PersistenceMode.Attribute),
            DefaultValue(""),
            Category("NK5"),
            Bindable(True),
            Localizable(True)>
            Public Property XMLColumnName() As String
                Get
                    Dim value As String = CStr(ViewState("XMLColumnName"))
                    If value Is Nothing Then
                        Return String.Empty
                    Else
                        Return value
                    End If
                End Get
                Set(ByVal value As String)
                    ViewState("XMLColumnName") = value.Replace(" ", "")
                End Set
            End Property

            <Description("XPath to used when storing and retrieving data for this control."),
            PersistenceMode(PersistenceMode.Attribute),
            DefaultValue(""),
            Category("NK5"),
            Bindable(True),
            Localizable(True)>
            Public Property XmlPath() As String
                Get
                    Dim value As String = CStr(ViewState("XmlPath"))
                    If value Is Nothing Then
                        Return String.Empty
                    Else
                        Return value
                    End If
                End Get
                Set(ByVal value As String)
                    ViewState("XmlPath") = value.Replace(" ", "")
                End Set
            End Property

            <Description("Indicates if the control requires input."),
            PersistenceMode(PersistenceMode.Attribute),
            DefaultValue(False),
            Category("NK5"),
            Bindable(True),
            Localizable(True)>
            Public Overridable Property Required() As Boolean
                Get
                    Dim value As String = CStr(ViewState("Required"))
                    If value Is Nothing Then
                        Return False
                    Else
                        Return CBool(value)
                    End If
                End Get
                Set(ByVal value As Boolean)
                    ViewState("Required") = value
                End Set
            End Property

            <Description("When control is required, this value will be treated as an empty value."),
            PersistenceMode(PersistenceMode.Attribute),
            DefaultValue(""),
            Category("NK5"),
            Bindable(True),
            Localizable(True)>
            Public Overridable Property RequiredEmptyValue() As String
                Get
                    Dim value As String = CStr(ViewState("RequiredEmptyValue"))
                    If value Is Nothing Then
                        Return String.Empty
                    Else
                        Return value
                    End If
                End Get
                Set(ByVal value As String)
                    ViewState("RequiredEmptyValue") = value
                End Set
            End Property

            <Description("Name of the Xml element used to store the control data."),
            PersistenceMode(PersistenceMode.Attribute),
            DefaultValue(""),
            Category("NK5"),
            Bindable(True),
            Localizable(True)>
            Public Overridable Property DataFieldName() As String
                Get
                    Dim value As String = CStr(ViewState("DataFieldName"))
                    If value Is Nothing Then
                        Return String.Empty
                    Else
                        Return value
                    End If
                End Get
                Set(ByVal value As String)
                    ViewState("DataFieldName") = value
                End Set
            End Property

            Overridable Function ValidateValue() As String
                'stub function for inherited text box controls
                Return String.Empty
            End Function

            <Description("Text to display if control value is in error."),
            PersistenceMode(PersistenceMode.Attribute),
            DefaultValue(""),
            Category("NK5"),
            Bindable(True),
            Localizable(True)>
            Public Overridable Property ErrorText() As String
                Get
                    Dim value As String = CStr(ViewState("ErrorText"))
                    If value Is Nothing Then
                        Return String.Empty
                    Else
                        Return value
                    End If
                End Get
                Set(ByVal value As String)
                    ViewState("ErrorText") = value
                End Set
            End Property

            Public Overrides Property Text() As String
                Get
                    Select Case Me.Casing
                        Case CasingOption.lower
                            Return MyBase.Text.ToLower
                        Case CasingOption.UPPER
                            Return MyBase.Text.ToUpper
                        Case Else
                            Return MyBase.Text
                    End Select
                End Get
                Set(ByVal value As String)
                    If value IsNot Nothing Then
                        value = value.Replace("<script", "").Replace("--", "")
                    End If
                    MyBase.Text = value
                End Set
            End Property

            <Description("Friendly name of control."),
            PersistenceMode(PersistenceMode.Attribute),
            DefaultValue(""),
            Category("NK5"),
            Bindable(True),
            Localizable(True)>
            Public Property FriendlyName() As String
                Get
                    Dim value As String = CStr(ViewState("FriendlyName"))
                    If value Is Nothing Then
                        If Me.DataFieldName = String.Empty Then
                            Return Me.ID
                        Else
                            Return Me.DataFieldName
                        End If
                    Else
                        Return value
                    End If
                End Get
                Set(ByVal value As String)
                    ViewState("FriendlyName") = value
                End Set
            End Property
            ''' <summary>
            ''' 
            ''' </summary>
            ''' <returns>String containing data input error description</returns>
            ''' <remarks></remarks>
            Public Function CheckErrors() As String
                'should return any error message with FriendlyName prefix, if any,
                ' and set value of ErrorText to error message without FriendlyName prefix
                Dim RetVal As String = String.Empty
                Dim strErrorText As String = String.Empty

                If (Me.Text.Trim.Length = 0) Or (Me.Text.Trim = Me.RequiredEmptyValue.Trim) Then
                    strErrorText = "Entry required."
                    If Me.FriendlyName.Trim.Length = 0 Then
                        RetVal = "Entry required."
                    Else
                        RetVal = Me.FriendlyName & " - entry is required."
                    End If
                Else
                    strErrorText = Me.ValidateValue
                    If (strErrorText IsNot Nothing) AndAlso (strErrorText.Trim.Length > 0) Then
                        If Me.FriendlyName.Trim.Length = 0 Then
                            RetVal = strErrorText
                        Else
                            RetVal = Me.FriendlyName & " - " & strErrorText
                        End If
                    Else
                        strErrorText = String.Empty
                    End If
                End If

                Me.ErrorText = strErrorText

                Return RetVal
            End Function
            ''' <summary>
            ''' Clears control's error text and optionally clears associated error label control
            ''' </summary>
            ''' <param name="ClearErrorLabel">Set to True to clear associated error label control</param>
            ''' <remarks></remarks>
            Public Sub ClearErrors(ByVal ClearErrorLabel As Boolean)
                Me.ErrorText = String.Empty
            End Sub
        End Class

        ' Inherits Base
        <AspNetHostingPermission(SecurityAction.Demand,
        Level:=AspNetHostingPermissionLevel.Minimal),
        AspNetHostingPermission(SecurityAction.InheritanceDemand,
        Level:=AspNetHostingPermissionLevel.Minimal),
        DefaultProperty("Text"),
        PersistChildren(False),
        ParseChildren(True, "Text"),
        ToolboxData("<{0}:TextBox runat=""server"" />")>
        Public Class TextBox
            Inherits Base

        End Class

        ' Inherits Telerik.Web.UI.RadNumericTextBox
        <AspNetHostingPermission(SecurityAction.Demand,
        Level:=AspNetHostingPermissionLevel.Minimal),
        AspNetHostingPermission(SecurityAction.InheritanceDemand,
        Level:=AspNetHostingPermissionLevel.Minimal),
        DefaultProperty("Text"),
        PersistChildren(False),
        ParseChildren(True, "Text"),
        ToolboxData("<{0}:NumericTextBox runat=""server"" />")>
        Public Class NumericTextBox
            Inherits Telerik.Web.UI.RadNumericTextBox

            Protected Overrides Sub AddAttributesToRender(ByVal writer As System.Web.UI.HtmlTextWriter)
                MyBase.AddAttributesToRender(writer)

            End Sub

            <Description("Miscellaneous data."),
            PersistenceMode(PersistenceMode.Attribute),
            DefaultValue(""),
            Category("NK5"),
            Bindable(True),
            Localizable(True)>
            Public Property Tag() As String
                Get
                    Dim value As String = CStr(ViewState("Tag"))
                    If value Is Nothing Then
                        Return String.Empty
                    Else
                        Return value
                    End If
                End Get
                Set(ByVal value As String)
                    ViewState("Tag") = value
                End Set
            End Property

            '<Description("Sets casing of control data."), _
            'PersistenceMode(PersistenceMode.Attribute), _
            'DefaultValue(CasingOption.Normal), _
            'Category("NK5"), _
            'Bindable(True), _
            'Localizable(True)> _
            'Public Overridable Property Casing() As CasingOption
            '    Get
            '        Dim value As String = CStr(ViewState("Casing"))
            '        If (value Is Nothing) OrElse (Not IsNumeric(value)) Then
            '            Return CasingOption.Normal
            '        Else
            '            Return CType(value, CasingOption)
            '        End If
            '    End Get
            '    Set(ByVal value As CasingOption)
            '        ViewState("Casing") = value
            '    End Set
            'End Property

            ''' <summary>
            ''' 
            ''' </summary>
            ''' <returns>Object</returns>
            ''' <remarks></remarks>
            Public Overridable Function DataValue() As Object
                Dim value As String = Me.Text
                If value.Contains("%") Then
                    value = value.Replace("%", "")
                    If IsNumeric(value) Then
                        value = (CDec(value) / 100).ToString
                    End If
                End If

                If IsNumeric(value) Then
                    Return CDec(value)
                Else
                    Return 0
                End If
            End Function

            <Description("Column name of Xml field when storing data in XML columns."),
            PersistenceMode(PersistenceMode.Attribute),
            DefaultValue(""),
            Category("NK5"),
            Bindable(True),
            Localizable(True)>
            Public Property XMLColumnName() As String
                Get
                    Dim value As String = CStr(ViewState("XMLColumnName"))
                    If value Is Nothing Then
                        Return String.Empty
                    Else
                        Return value
                    End If
                End Get
                Set(ByVal value As String)
                    ViewState("XMLColumnName") = value.Replace(" ", "")
                End Set
            End Property

            <Description("XPath to used when storing and retrieving data for this control."),
            PersistenceMode(PersistenceMode.Attribute),
            DefaultValue(""),
            Category("NK5"),
            Bindable(True),
            Localizable(True)>
            Public Property XmlPath() As String
                Get
                    Dim value As String = CStr(ViewState("XmlPath"))
                    If value Is Nothing Then
                        Return String.Empty
                    Else
                        Return value
                    End If
                End Get
                Set(ByVal value As String)
                    ViewState("XmlPath") = value.Replace(" ", "")
                End Set
            End Property

            <Description("Indicates if the control requires input."),
            PersistenceMode(PersistenceMode.Attribute),
            DefaultValue(False),
            Category("NK5"),
            Bindable(True),
            Localizable(True)>
            Public Overridable Property Required() As Boolean
                Get
                    Dim value As String = CStr(ViewState("Required"))
                    If value Is Nothing Then
                        Return False
                    Else
                        Return CBool(value)
                    End If
                End Get
                Set(ByVal value As Boolean)
                    ViewState("Required") = value
                End Set
            End Property

            <Description("When control is required, this value will be treated as an empty value."),
            PersistenceMode(PersistenceMode.Attribute),
            DefaultValue(""),
            Category("NK5"),
            Bindable(True),
            Localizable(True)>
            Public Overridable Property RequiredEmptyValue() As String
                Get
                    Dim value As String = CStr(ViewState("RequiredEmptyValue"))
                    If value Is Nothing Then
                        Return String.Empty
                    Else
                        Return value
                    End If
                End Get
                Set(ByVal value As String)
                    ViewState("RequiredEmptyValue") = value
                End Set
            End Property

            <Description("Name of the Xml element used to store the control data."),
            PersistenceMode(PersistenceMode.Attribute),
            DefaultValue(""),
            Category("NK5"),
            Bindable(True),
            Localizable(True)>
            Public Overridable Property DataFieldName() As String
                Get
                    Dim value As String = CStr(ViewState("DataFieldName"))
                    If value Is Nothing Then
                        Return String.Empty
                    Else
                        Return value
                    End If
                End Get
                Set(ByVal value As String)
                    ViewState("DataFieldName") = value
                End Set
            End Property

            Overridable Function ValidateValue() As String
                'stub function for inherited text box controls
                Return String.Empty
            End Function

            <Description("Text to display if control value is in error."),
            PersistenceMode(PersistenceMode.Attribute),
            DefaultValue(""),
            Category("NK5"),
            Bindable(True),
            Localizable(True)>
            Public Overridable Property ErrorText() As String
                Get
                    Dim value As String = CStr(ViewState("ErrorText"))
                    If value Is Nothing Then
                        Return String.Empty
                    Else
                        Return value
                    End If
                End Get
                Set(ByVal value As String)
                    ViewState("ErrorText") = value
                End Set
            End Property

            'Public Overrides Property Text() As String
            '    Get
            '        Return Me.GetFormattedValue(CStr(ViewState("Text")))
            '    End Get
            '    Set(ByVal value As String)
            '        ViewState("Text") = value
            '    End Set
            'End Property

            <Description("Friendly name of control."),
            PersistenceMode(PersistenceMode.Attribute),
            DefaultValue(""),
            Category("NK5"),
            Bindable(True),
            Localizable(True)>
            Public Property FriendlyName() As String
                Get
                    Dim value As String = CStr(ViewState("FriendlyName"))
                    If value Is Nothing Then
                        If Me.DataFieldName = String.Empty Then
                            Return Me.ID
                        Else
                            Return Me.DataFieldName
                        End If
                    Else
                        Return value
                    End If
                End Get
                Set(ByVal value As String)
                    ViewState("FriendlyName") = value
                End Set
            End Property
            ''' <summary>
            ''' 
            ''' </summary>
            ''' <returns>String containing data input error description</returns>
            ''' <remarks></remarks>
            Public Function CheckErrors() As String
                'should return any error message with FriendlyName prefix, if any,
                ' and set value of ErrorText to error message without FriendlyName prefix
                Dim RetVal As String = String.Empty
                Dim strErrorText As String = String.Empty

                If (Me.Text.Trim.Length = 0) Or (Me.Text.Trim = Me.RequiredEmptyValue.Trim) Then
                    strErrorText = "Entry required."
                    If Me.FriendlyName.Trim.Length = 0 Then
                        RetVal = "Entry required."
                    Else
                        RetVal = Me.FriendlyName & " - entry is required."
                    End If
                Else
                    strErrorText = Me.ValidateValue
                    If (strErrorText IsNot Nothing) AndAlso (strErrorText.Trim.Length > 0) Then
                        If Me.FriendlyName.Trim.Length = 0 Then
                            RetVal = strErrorText
                        Else
                            RetVal = Me.FriendlyName & " - " & strErrorText
                        End If
                    Else
                        strErrorText = String.Empty
                    End If
                End If

                Me.ErrorText = strErrorText

                Return RetVal
            End Function
            ''' <summary>
            ''' Clears control's error text and optionally clears associated error label control
            ''' </summary>
            ''' <param name="ClearErrorLabel">Set to True to clear associated error label control</param>
            ''' <remarks></remarks>
            Public Sub ClearErrors(ByVal ClearErrorLabel As Boolean)
                Me.ErrorText = String.Empty
            End Sub

            '<Description("Named format to apply to underlying value."), _
            'PersistenceMode(PersistenceMode.Attribute), _
            'DefaultValue(NumericFormat.Custom), _
            'Category("NK5"), _
            'Bindable(True), _
            'Localizable(True)> _
            'Public Property FormatNamed() As NumericFormat
            '    Get
            '        Dim value As String = CStr(ViewState("FormatNamed"))
            '        If (value Is Nothing) OrElse (Not IsNumeric(value)) Then
            '            Return NumericFormat.Custom
            '        Else
            '            Return CType(value, NumericFormat)
            '        End If
            '    End Get
            '    Set(ByVal value As NumericFormat)
            '        ViewState("FormatNamed") = value
            '    End Set
            'End Property

            '<Description("Number of decimals to display if using a Named format."), _
            'PersistenceMode(PersistenceMode.Attribute), _
            'DefaultValue(0), _
            'Category("NK5"), _
            'Bindable(True), _
            'Localizable(True)> _
            'Public Property FormatNamedDecimals() As Integer
            '    Get
            '        Dim value As String = CStr(ViewState("FormatNamedDecimals"))
            '        If (value Is Nothing) OrElse (Not IsNumeric(value)) Then
            '            Return 0
            '        Else
            '            Return CInt(value)
            '        End If
            '    End Get
            '    Set(ByVal value As Integer)
            '        ViewState("FormatNamedDecimals") = value
            '    End Set
            'End Property

            '<Description("Custom format to apply to underlying value."), _
            'PersistenceMode(PersistenceMode.Attribute), _
            'DefaultValue(""), _
            'Category("NK5"), _
            'Bindable(True), _
            'Localizable(True)> _
            'Public Property FormatCustom() As String
            '    Get
            '        Dim value As String = CStr(ViewState("DisplayFormat"))
            '        If value Is Nothing Then
            '            Return String.Empty
            '        Else
            '            Return value
            '        End If
            '    End Get
            '    Set(ByVal value As String)
            '        ViewState("DisplayFormat") = value
            '    End Set
            'End Property

            'Private Function GetFormattedValue(ByVal value As Object) As String
            '    If (value Is Nothing) OrElse (value.ToString.Trim.Length = 0) Then
            '        Return String.Empty
            '    ElseIf value.ToString.Contains("%") Then
            '        value = value.ToString.Replace("%", "")
            '        If IsNumeric(value) Then
            '            value = (CDec(value) / 100).ToString
            '        End If
            '    End If

            '    If Not IsNumeric(value) Then
            '        Return String.Empty
            '    End If

            '    Select Case Me.FormatNamed
            '        Case NumericFormat.Currency
            '            Return FormatCurrency(value, Me.FormatNamedDecimals)
            '        Case NumericFormat.Percent
            '            Return FormatPercent(value, Me.FormatNamedDecimals)
            '        Case NumericFormat.Number
            '            Return FormatNumber(value, Me.FormatNamedDecimals)
            '        Case Else
            '            Return Format(CDec(value), Me.FormatCustom)
            '    End Select
            'End Function
        End Class

        ' Inherits Telerik.Web.UI.RadDateInput
        <AspNetHostingPermission(SecurityAction.Demand,
        Level:=AspNetHostingPermissionLevel.Minimal),
        AspNetHostingPermission(SecurityAction.InheritanceDemand,
        Level:=AspNetHostingPermissionLevel.Minimal),
        DefaultProperty("Text"),
        PersistChildren(False),
        ParseChildren(True, "Text"),
        ToolboxData("<{0}:DateTextBox runat=""server"" />")>
        Public Class DateTextBox
            Inherits Telerik.Web.UI.RadDateInput

            Protected Overrides Sub AddAttributesToRender(ByVal writer As System.Web.UI.HtmlTextWriter)
                MyBase.AddAttributesToRender(writer)

            End Sub

            <Description("Miscellaneous data."),
            PersistenceMode(PersistenceMode.Attribute),
            DefaultValue(""),
            Category("NK5"),
            Bindable(True),
            Localizable(True)>
            Public Property Tag() As String
                Get
                    Dim value As String = CStr(ViewState("Tag"))
                    If value Is Nothing Then
                        Return String.Empty
                    Else
                        Return value
                    End If
                End Get
                Set(ByVal value As String)
                    ViewState("Tag") = value
                End Set
            End Property

            '<Description("Sets casing of control data."), _
            'PersistenceMode(PersistenceMode.Attribute), _
            'DefaultValue(CasingOption.Normal), _
            'Category("NK5"), _
            'Bindable(True), _
            'Localizable(True)> _
            'Public Overridable Property Casing() As CasingOption
            '    Get
            '        Dim value As String = CStr(ViewState("Casing"))
            '        If (value Is Nothing) OrElse (Not IsNumeric(value)) Then
            '            Return CasingOption.Normal
            '        Else
            '            Return CType(value, CasingOption)
            '        End If
            '    End Get
            '    Set(ByVal value As CasingOption)
            '        ViewState("Casing") = value
            '    End Set
            'End Property

            ''' <summary>
            ''' 
            ''' </summary>
            ''' <returns>Object</returns>
            ''' <remarks></remarks>
            Public Overridable Function DataValue() As Object
                Dim value As String = Me.Text
                If value.Contains("%") Then
                    value = value.Replace("%", "")
                    If IsNumeric(value) Then
                        value = (CDec(value) / 100).ToString
                    End If
                End If

                If IsNumeric(value) Then
                    Return CDec(value)
                Else
                    Return 0
                End If
            End Function

            <Description("Column name of Xml field when storing data in XML columns."),
            PersistenceMode(PersistenceMode.Attribute),
            DefaultValue(""),
            Category("NK5"),
            Bindable(True),
            Localizable(True)>
            Public Property XMLColumnName() As String
                Get
                    Dim value As String = CStr(ViewState("XMLColumnName"))
                    If value Is Nothing Then
                        Return String.Empty
                    Else
                        Return value
                    End If
                End Get
                Set(ByVal value As String)
                    ViewState("XMLColumnName") = value.Replace(" ", "")
                End Set
            End Property

            <Description("XPath to used when storing and retrieving data for this control."),
            PersistenceMode(PersistenceMode.Attribute),
            DefaultValue(""),
            Category("NK5"),
            Bindable(True),
            Localizable(True)>
            Public Property XmlPath() As String
                Get
                    Dim value As String = CStr(ViewState("XmlPath"))
                    If value Is Nothing Then
                        Return String.Empty
                    Else
                        Return value
                    End If
                End Get
                Set(ByVal value As String)
                    ViewState("XmlPath") = value.Replace(" ", "")
                End Set
            End Property

            <Description("Indicates if the control requires input."),
            PersistenceMode(PersistenceMode.Attribute),
            DefaultValue(False),
            Category("NK5"),
            Bindable(True),
            Localizable(True)>
            Public Overridable Property Required() As Boolean
                Get
                    Dim value As String = CStr(ViewState("Required"))
                    If value Is Nothing Then
                        Return False
                    Else
                        Return CBool(value)
                    End If
                End Get
                Set(ByVal value As Boolean)
                    ViewState("Required") = value
                End Set
            End Property

            <Description("When control is required, this value will be treated as an empty value."),
            PersistenceMode(PersistenceMode.Attribute),
            DefaultValue(""),
            Category("NK5"),
            Bindable(True),
            Localizable(True)>
            Public Overridable Property RequiredEmptyValue() As String
                Get
                    Dim value As String = CStr(ViewState("RequiredEmptyValue"))
                    If value Is Nothing Then
                        Return String.Empty
                    Else
                        Return value
                    End If
                End Get
                Set(ByVal value As String)
                    ViewState("RequiredEmptyValue") = value
                End Set
            End Property

            <Description("Name of the Xml element used to store the control data."),
            PersistenceMode(PersistenceMode.Attribute),
            DefaultValue(""),
            Category("NK5"),
            Bindable(True),
            Localizable(True)>
            Public Overridable Property DataFieldName() As String
                Get
                    Dim value As String = CStr(ViewState("DataFieldName"))
                    If value Is Nothing Then
                        Return String.Empty
                    Else
                        Return value
                    End If
                End Get
                Set(ByVal value As String)
                    ViewState("DataFieldName") = value
                End Set
            End Property

            <Description("Text to display if control value is in error."),
            PersistenceMode(PersistenceMode.Attribute),
            DefaultValue(""),
            Category("NK5"),
            Bindable(True),
            Localizable(True)>
            Public Overridable Property ErrorText() As String
                Get
                    Dim value As String = CStr(ViewState("ErrorText"))
                    If value Is Nothing Then
                        Return String.Empty
                    Else
                        Return value
                    End If
                End Get
                Set(ByVal value As String)
                    ViewState("ErrorText") = value
                End Set
            End Property

            'Public Overrides Property Text() As String
            '    Get
            '        Return Me.GetFormattedValue(CStr(ViewState("Text")))
            '    End Get
            '    Set(ByVal value As String)
            '        ViewState("Text") = value
            '    End Set
            'End Property

            <Description("Friendly name of control."),
            PersistenceMode(PersistenceMode.Attribute),
            DefaultValue(""),
            Category("NK5"),
            Bindable(True),
            Localizable(True)>
            Public Property FriendlyName() As String
                Get
                    Dim value As String = CStr(ViewState("FriendlyName"))
                    If value Is Nothing Then
                        If Me.DataFieldName = String.Empty Then
                            Return Me.ID
                        Else
                            Return Me.DataFieldName
                        End If
                    Else
                        Return value
                    End If
                End Get
                Set(ByVal value As String)
                    ViewState("FriendlyName") = value
                End Set
            End Property
            ''' <summary>
            ''' 
            ''' </summary>
            ''' <returns>String containing data input error description</returns>
            ''' <remarks></remarks>
            Public Function CheckErrors() As String
                'should return any error message with FriendlyName prefix, if any,
                ' and set value of ErrorText to error message without FriendlyName prefix
                Dim RetVal As String = String.Empty
                Dim strErrorText As String = String.Empty

                If (Me.Text.Trim.Length = 0) Or (Me.Text.Trim = Me.RequiredEmptyValue.Trim) Then
                    strErrorText = "Entry required."
                    If Me.FriendlyName.Trim.Length = 0 Then
                        RetVal = "Entry required."
                    Else
                        RetVal = Me.FriendlyName & " - entry is required."
                    End If
                Else
                    strErrorText = Me.ValidateValue
                    If (strErrorText IsNot Nothing) AndAlso (strErrorText.Trim.Length > 0) Then
                        If Me.FriendlyName.Trim.Length = 0 Then
                            RetVal = strErrorText
                        Else
                            RetVal = Me.FriendlyName & " - " & strErrorText
                        End If
                    Else
                        strErrorText = String.Empty
                    End If
                End If

                Me.ErrorText = strErrorText

                Return RetVal
            End Function
            ''' <summary>
            ''' Clears control's error text and optionally clears associated error label control
            ''' </summary>
            ''' <param name="ClearErrorLabel">Set to True to clear associated error label control</param>
            ''' <remarks></remarks>
            Public Sub ClearErrors(ByVal ClearErrorLabel As Boolean)
                Me.ErrorText = String.Empty
            End Sub

            '<Description("Named format to apply to underlying value."), _
            'PersistenceMode(PersistenceMode.Attribute), _
            'DefaultValue(NumericFormat.Custom), _
            'Category("NK5"), _
            'Bindable(True), _
            'Localizable(True)> _
            'Public Property FormatNamed() As NumericFormat
            '    Get
            '        Dim value As String = CStr(ViewState("FormatNamed"))
            '        If (value Is Nothing) OrElse (Not IsNumeric(value)) Then
            '            Return NumericFormat.Custom
            '        Else
            '            Return CType(value, NumericFormat)
            '        End If
            '    End Get
            '    Set(ByVal value As NumericFormat)
            '        ViewState("FormatNamed") = value
            '    End Set
            'End Property

            '<Description("Number of decimals to display if using a Named format."), _
            'PersistenceMode(PersistenceMode.Attribute), _
            'DefaultValue(0), _
            'Category("NK5"), _
            'Bindable(True), _
            'Localizable(True)> _
            'Public Property FormatNamedDecimals() As Integer
            '    Get
            '        Dim value As String = CStr(ViewState("FormatNamedDecimals"))
            '        If (value Is Nothing) OrElse (Not IsNumeric(value)) Then
            '            Return 0
            '        Else
            '            Return CInt(value)
            '        End If
            '    End Get
            '    Set(ByVal value As Integer)
            '        ViewState("FormatNamedDecimals") = value
            '    End Set
            'End Property

            '<Description("Custom format to apply to underlying value."), _
            'PersistenceMode(PersistenceMode.Attribute), _
            'DefaultValue(""), _
            'Category("NK5"), _
            'Bindable(True), _
            'Localizable(True)> _
            'Public Property FormatCustom() As String
            '    Get
            '        Dim value As String = CStr(ViewState("DisplayFormat"))
            '        If value Is Nothing Then
            '            Return String.Empty
            '        Else
            '            Return value
            '        End If
            '    End Get
            '    Set(ByVal value As String)
            '        ViewState("DisplayFormat") = value
            '    End Set
            'End Property

            'Private Function GetFormattedValue(ByVal value As Object) As String
            '    If (value Is Nothing) OrElse (value.ToString.Trim.Length = 0) Then
            '        Return String.Empty
            '    ElseIf value.ToString.Contains("%") Then
            '        value = value.ToString.Replace("%", "")
            '        If IsNumeric(value) Then
            '            value = (CDec(value) / 100).ToString
            '        End If
            '    End If

            '    If Not IsNumeric(value) Then
            '        Return String.Empty
            '    End If

            '    Select Case Me.FormatNamed
            '        Case NumericFormat.Currency
            '            Return FormatCurrency(value, Me.FormatNamedDecimals)
            '        Case NumericFormat.Percent
            '            Return FormatPercent(value, Me.FormatNamedDecimals)
            '        Case NumericFormat.Number
            '            Return FormatNumber(value, Me.FormatNamedDecimals)
            '        Case Else
            '            Return Format(CDec(value), Me.FormatCustom)
            '    End Select
            'End Function

            Overridable Function ValidateValue() As String
                '???
                If IsDate(Me.Text) Then
                    Return String.Empty
                Else
                    Return "Enter a valid date."
                End If
            End Function
        End Class

        ' Inherits Telerik.Web.UI.RadMaskedTextBox
        <AspNetHostingPermission(SecurityAction.Demand,
        Level:=AspNetHostingPermissionLevel.Minimal),
        AspNetHostingPermission(SecurityAction.InheritanceDemand,
        Level:=AspNetHostingPermissionLevel.Minimal),
        DefaultProperty("Text"),
        PersistChildren(False),
        ParseChildren(True, "Text"),
        ToolboxData("<{0}:MaskedTextBox runat=""server"" />")>
        Public Class MaskedTextBox
            Inherits Telerik.Web.UI.RadMaskedTextBox

            Protected Overrides Sub AddAttributesToRender(ByVal writer As System.Web.UI.HtmlTextWriter)
                MyBase.AddAttributesToRender(writer)

            End Sub

            <Description("Miscellaneous data."),
            PersistenceMode(PersistenceMode.Attribute),
            DefaultValue(""),
            Category("NK5"),
            Bindable(True),
            Localizable(True)>
            Public Property Tag() As String
                Get
                    Dim value As String = CStr(ViewState("Tag"))
                    If value Is Nothing Then
                        Return String.Empty
                    Else
                        Return value
                    End If
                End Get
                Set(ByVal value As String)
                    ViewState("Tag") = value
                End Set
            End Property

            <Description("Column name of Xml field when storing data in XML columns."),
            PersistenceMode(PersistenceMode.Attribute),
            DefaultValue(""),
            Category("NK5"),
            Bindable(True),
            Localizable(True)>
            Public Property XMLColumnName() As String
                Get
                    Dim value As String = CStr(ViewState("XMLColumnName"))
                    If value Is Nothing Then
                        Return String.Empty
                    Else
                        Return value
                    End If
                End Get
                Set(ByVal value As String)
                    ViewState("XMLColumnName") = value.Replace(" ", "")
                End Set
            End Property

            <Description("XPath to used when storing and retrieving data for this control."),
            PersistenceMode(PersistenceMode.Attribute),
            DefaultValue(""),
            Category("NK5"),
            Bindable(True),
            Localizable(True)>
            Public Property XmlPath() As String
                Get
                    Dim value As String = CStr(ViewState("XmlPath"))
                    If value Is Nothing Then
                        Return String.Empty
                    Else
                        Return value
                    End If
                End Get
                Set(ByVal value As String)
                    ViewState("XmlPath") = value.Replace(" ", "")
                End Set
            End Property

            <Description("Indicates if the control requires input."),
            PersistenceMode(PersistenceMode.Attribute),
            DefaultValue(False),
            Category("NK5"),
            Bindable(True),
            Localizable(True)>
            Public Overridable Property Required() As Boolean
                Get
                    Dim value As String = CStr(ViewState("Required"))
                    If value Is Nothing Then
                        Return False
                    Else
                        Return CBool(value)
                    End If
                End Get
                Set(ByVal value As Boolean)
                    ViewState("Required") = value
                End Set
            End Property

            <Description("When control is required, this value will be treated as an empty value."),
            PersistenceMode(PersistenceMode.Attribute),
            DefaultValue(""),
            Category("NK5"),
            Bindable(True),
            Localizable(True)>
            Public Overridable Property RequiredEmptyValue() As String
                Get
                    Dim value As String = CStr(ViewState("RequiredEmptyValue"))
                    If value Is Nothing Then
                        Return String.Empty
                    Else
                        Return value
                    End If
                End Get
                Set(ByVal value As String)
                    ViewState("RequiredEmptyValue") = value
                End Set
            End Property

            <Description("Name of the Xml element used to store the control data."),
            PersistenceMode(PersistenceMode.Attribute),
            DefaultValue(""),
            Category("NK5"),
            Bindable(True),
            Localizable(True)>
            Public Overridable Property DataFieldName() As String
                Get
                    Dim value As String = CStr(ViewState("DataFieldName"))
                    If value Is Nothing Then
                        Return String.Empty
                    Else
                        Return value
                    End If
                End Get
                Set(ByVal value As String)
                    ViewState("DataFieldName") = value
                End Set
            End Property

            <Description("Text to display if control value is in error."),
            PersistenceMode(PersistenceMode.Attribute),
            DefaultValue(""),
            Category("NK5"),
            Bindable(True),
            Localizable(True)>
            Public Overridable Property ErrorText() As String
                Get
                    Dim value As String = CStr(ViewState("ErrorText"))
                    If value Is Nothing Then
                        Return String.Empty
                    Else
                        Return value
                    End If
                End Get
                Set(ByVal value As String)
                    ViewState("ErrorText") = value
                End Set
            End Property

            'Public Overrides Property Text() As String
            '    Get
            '        Return Me.GetFormattedValue(CStr(ViewState("Text")))
            '    End Get
            '    Set(ByVal value As String)
            '        ViewState("Text") = value
            '    End Set
            'End Property

            <Description("Friendly name of control."),
            PersistenceMode(PersistenceMode.Attribute),
            DefaultValue(""),
            Category("NK5"),
            Bindable(True),
            Localizable(True)>
            Public Property FriendlyName() As String
                Get
                    Dim value As String = CStr(ViewState("FriendlyName"))
                    If value Is Nothing Then
                        If Me.DataFieldName = String.Empty Then
                            Return Me.ID
                        Else
                            Return Me.DataFieldName
                        End If
                    Else
                        Return value
                    End If
                End Get
                Set(ByVal value As String)
                    ViewState("FriendlyName") = value
                End Set
            End Property
            ''' <summary>
            ''' 
            ''' </summary>
            ''' <returns>String containing data input error description</returns>
            ''' <remarks></remarks>
            Public Function CheckErrors() As String
                'should return any error message with FriendlyName prefix, if any,
                ' and set value of ErrorText to error message without FriendlyName prefix
                Dim RetVal As String = String.Empty
                Dim strErrorText As String = String.Empty

                If (Me.Text.Trim.Length = 0) Or (Me.Text.Trim = Me.RequiredEmptyValue.Trim) Then
                    strErrorText = "Entry required."
                    If Me.FriendlyName.Trim.Length = 0 Then
                        RetVal = "Entry required."
                    Else
                        RetVal = Me.FriendlyName & " - entry is required."
                    End If
                Else
                    strErrorText = Me.ValidateValue
                    If (strErrorText IsNot Nothing) AndAlso (strErrorText.Trim.Length > 0) Then
                        If Me.FriendlyName.Trim.Length = 0 Then
                            RetVal = strErrorText
                        Else
                            RetVal = Me.FriendlyName & " - " & strErrorText
                        End If
                    Else
                        strErrorText = String.Empty
                    End If
                End If

                Me.ErrorText = strErrorText

                Return RetVal
            End Function
            ''' <summary>
            ''' Clears control's error text and optionally clears associated error label control
            ''' </summary>
            ''' <param name="ClearErrorLabel">Set to True to clear associated error label control</param>
            ''' <remarks></remarks>
            Public Sub ClearErrors(ByVal ClearErrorLabel As Boolean)
                Me.ErrorText = String.Empty
            End Sub

            '<Description("Named format to apply to underlying value."), _
            'PersistenceMode(PersistenceMode.Attribute), _
            'DefaultValue(NumericFormat.Custom), _
            'Category("NK5"), _
            'Bindable(True), _
            'Localizable(True)> _
            'Public Property FormatNamed() As NumericFormat
            '    Get
            '        Dim value As String = CStr(ViewState("FormatNamed"))
            '        If (value Is Nothing) OrElse (Not IsNumeric(value)) Then
            '            Return NumericFormat.Custom
            '        Else
            '            Return CType(value, NumericFormat)
            '        End If
            '    End Get
            '    Set(ByVal value As NumericFormat)
            '        ViewState("FormatNamed") = value
            '    End Set
            'End Property

            '<Description("Number of decimals to display if using a Named format."), _
            'PersistenceMode(PersistenceMode.Attribute), _
            'DefaultValue(0), _
            'Category("NK5"), _
            'Bindable(True), _
            'Localizable(True)> _
            'Public Property FormatNamedDecimals() As Integer
            '    Get
            '        Dim value As String = CStr(ViewState("FormatNamedDecimals"))
            '        If (value Is Nothing) OrElse (Not IsNumeric(value)) Then
            '            Return 0
            '        Else
            '            Return CInt(value)
            '        End If
            '    End Get
            '    Set(ByVal value As Integer)
            '        ViewState("FormatNamedDecimals") = value
            '    End Set
            'End Property

            '<Description("Custom format to apply to underlying value."), _
            'PersistenceMode(PersistenceMode.Attribute), _
            'DefaultValue(""), _
            'Category("NK5"), _
            'Bindable(True), _
            'Localizable(True)> _
            'Public Property FormatCustom() As String
            '    Get
            '        Dim value As String = CStr(ViewState("DisplayFormat"))
            '        If value Is Nothing Then
            '            Return String.Empty
            '        Else
            '            Return value
            '        End If
            '    End Get
            '    Set(ByVal value As String)
            '        ViewState("DisplayFormat") = value
            '    End Set
            'End Property

            'Private Function GetFormattedValue(ByVal value As Object) As String
            '    If (value Is Nothing) OrElse (value.ToString.Trim.Length = 0) Then
            '        Return String.Empty
            '    ElseIf value.ToString.Contains("%") Then
            '        value = value.ToString.Replace("%", "")
            '        If IsNumeric(value) Then
            '            value = (CDec(value) / 100).ToString
            '        End If
            '    End If

            '    If Not IsNumeric(value) Then
            '        Return String.Empty
            '    End If

            '    Select Case Me.FormatNamed
            '        Case NumericFormat.Currency
            '            Return FormatCurrency(value, Me.FormatNamedDecimals)
            '        Case NumericFormat.Percent
            '            Return FormatPercent(value, Me.FormatNamedDecimals)
            '        Case NumericFormat.Number
            '            Return FormatNumber(value, Me.FormatNamedDecimals)
            '        Case Else
            '            Return Format(CDec(value), Me.FormatCustom)
            '    End Select
            'End Function

            Overridable Function ValidateValue() As String
                Return String.Empty
            End Function
        End Class

    End Namespace

    ' Inherits Telerik.Web.UI.RadComboBox
    Namespace DropDownLists
        'DefaultProperty("Text"), _
        'PersistChildren(False), _
        'ParseChildren(True)> _
        <AspNetHostingPermission(SecurityAction.Demand,
        Level:=AspNetHostingPermissionLevel.Minimal),
        AspNetHostingPermission(SecurityAction.InheritanceDemand,
        Level:=AspNetHostingPermissionLevel.Minimal)>
        Public Class Base
            Inherits Telerik.Web.UI.RadComboBox

            Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
                MyBase.Render(writer)
            End Sub

            <Description("Miscellaneous data."),
            PersistenceMode(PersistenceMode.Attribute),
            DefaultValue(""),
            Category("NK5"),
            Bindable(True),
            Localizable(True)>
            Public Property Tag() As String
                Get
                    Dim value As String = CStr(ViewState("Tag"))
                    If value Is Nothing Then
                        Return String.Empty
                    Else
                        Return value
                    End If
                End Get
                Set(ByVal value As String)
                    ViewState("Tag") = value
                End Set
            End Property

            <Description("Sets casing of control data."),
            PersistenceMode(PersistenceMode.Attribute),
            DefaultValue(CasingOption.Normal),
            Category("NK5"),
            Bindable(True),
            Localizable(True)>
            Public Overridable Property Casing() As CasingOption
                Get
                    Dim value As String = CStr(ViewState("Casing"))
                    If (value Is Nothing) OrElse (Not IsNumeric(value)) Then
                        Return CasingOption.Normal
                    Else
                        Return CType(value, CasingOption)
                    End If
                End Get
                Set(ByVal value As CasingOption)
                    ViewState("Casing") = value
                End Set
            End Property

            'Public Overrides ReadOnly Property Items() As System.Web.UI.WebControls.ListItemCollection
            '    Get
            '        For Each myItem As ListItem In MyBase.Items
            '            Select Case Me.Casing
            '                Case CasingOption.lower
            '                    myItem.Text = myItem.Text.ToLower
            '                    myItem.Value = myItem.Value.ToLower
            '                Case CasingOption.UPPER
            '                    myItem.Text = myItem.Text.ToUpper
            '                    myItem.Value = myItem.Value.ToUpper
            '            End Select
            '        Next
            '        Return MyBase.Items
            '    End Get
            'End Property

            Public Overrides Property SelectedValue() As String
                Get
                    Return MyBase.SelectedValue
                End Get
                Set(ByVal value As String)
                    If (Me.Items.FindItemByValue(value) Is Nothing) Then
                        If Me.AddItemsOnDemand Then
                            Dim itm As New Telerik.Web.UI.RadComboBoxItem(value)
                            Me.Items.Add(itm)
                        Else
                            Exit Property
                        End If
                    End If
                    MyBase.SelectedValue = value
                End Set
            End Property

            <Description("Sets the value of the dropdownlist to the item whose numeric value matches the argument. If no match is found then the SelectedValue property remains unchanged"),
            PersistenceMode(PersistenceMode.Attribute),
            DefaultValue(0),
            Category("NK5"),
            Bindable(True),
            Localizable(True)>
            Public Property SelectedLongValue() As Long
                Get
                    Try
                        Return CLng(MyBase.SelectedValue)
                    Catch ex As Exception
                        Return 0
                    End Try
                End Get
                Set(ByVal value As Long)
                    'cycle through list items looking for one whose numeric equivalent is same as value we're looking for
                    For Each myItem As WebControls.ListItem In MyBase.Items
                        If IsNumeric(myItem.Value) And myItem.Enabled Then
                            Try
                                If CLng(myItem.Value) = value Then
                                    MyBase.SelectedValue = myItem.Value
                                End If
                            Catch ex As Exception
                            End Try
                        End If
                    Next
                End Set

            End Property

            <Description("Flag indicating of list items are added on demand as necessary when setting control's SelectedValue property."),
            PersistenceMode(PersistenceMode.Attribute),
            DefaultValue(True),
            Category("NK5"),
            Bindable(True),
            Localizable(True)>
            Public Property AddItemsOnDemand() As Boolean
                Get
                    Dim value As String = CStr(ViewState("AddItemsOnDemand"))
                    If value Is Nothing Then
                        Return True
                    Else
                        Return CBool(value)
                    End If
                End Get
                Set(ByVal value As Boolean)
                    ViewState("AddItemsOnDemand") = value
                End Set
            End Property

            ''' <summary>
            ''' 
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>
            ''' <remarks>This is used during routine to write control data to data source. 
            ''' If the dropdownlist's SelectedValue property is the same as its PromptItemValue property then
            ''' a NULL value is written to the data column.
            ''' This property will typically be set during a listitem population routine.</remarks>
            <Description("Value of the list item that is used as informational prompt."),
            DefaultValue(""),
            Category("NK5"),
            Bindable(True),
            Localizable(True)>
            Public Property PromptItemValue() As String
                Get
                    Dim value As String = CStr(ViewState("PromptItemValue"))
                    If value Is Nothing Then
                        Return String.Empty
                    Else
                        Return value
                    End If
                End Get
                Set(ByVal value As String)
                    ViewState("PromptItemValue") = value
                End Set
            End Property

            <Description("Column name of Xml field when storing data in XML columns."),
            PersistenceMode(PersistenceMode.Attribute),
            DefaultValue(""),
            Category("NK5"),
            Bindable(True),
            Localizable(True)>
            Public Property XMLColumnName() As String
                Get
                    Dim value As String = CStr(ViewState("XMLColumnName"))
                    If value Is Nothing Then
                        Return String.Empty
                    Else
                        Return value
                    End If
                End Get
                Set(ByVal value As String)
                    ViewState("XMLColumnName") = value.Replace(" ", "")
                End Set
            End Property

            <Description("XPath to used when storing and retrieving data for this control."),
            PersistenceMode(PersistenceMode.Attribute),
            DefaultValue(""),
            Category("NK5"),
            Bindable(True),
            Localizable(True)>
            Public Property XmlPath() As String
                Get
                    Dim value As String = CStr(ViewState("XmlPath"))
                    If value Is Nothing Then
                        Return String.Empty
                    Else
                        Return value
                    End If
                End Get
                Set(ByVal value As String)
                    ViewState("XmlPath") = value.Replace(" ", "")
                End Set
            End Property

            <Description("Indicates if the control requires input at run-time."),
            PersistenceMode(PersistenceMode.Attribute),
            DefaultValue(False),
            Category("NK5"),
            Bindable(True),
            Localizable(True)>
            Public Overridable Property Required() As Boolean
                Get
                    Dim value As String = CStr(ViewState("Required"))
                    If value Is Nothing Then
                        Return False
                    Else
                        Return CBool(value)
                    End If
                End Get
                Set(ByVal value As Boolean)
                    ViewState("Required") = value
                End Set
            End Property

            <Description("Name of the Xml element used to store the control data."),
            PersistenceMode(PersistenceMode.Attribute),
            DefaultValue(""),
            Category("NK5"),
            Bindable(True),
            Localizable(True)>
            Public Property DataFieldName() As String
                Get
                    Dim value As String = CStr(ViewState("DataFieldName"))
                    If value Is Nothing Then
                        Return String.Empty
                    Else
                        Return value
                    End If
                End Get
                Set(ByVal value As String)
                    ViewState("DataFieldName") = value
                End Set
            End Property

            <Description("Friendly name of control."),
            PersistenceMode(PersistenceMode.Attribute),
            DefaultValue(""),
            Category("NK5"),
            Bindable(True),
            Localizable(True)>
            Public Property FriendlyName() As String
                Get
                    Dim value As String = CStr(ViewState("FriendlyName"))
                    If value Is Nothing Then
                        If Me.DataFieldName = String.Empty Then
                            Return Me.ID
                        Else
                            Return Me.DataFieldName
                        End If
                    Else
                        Return value
                    End If
                End Get
                Set(ByVal value As String)
                    ViewState("FriendlyName") = value
                End Set
            End Property

            Public Function CheckErrors() As String
                'idea is that if a dropdownlist control is in error, 
                '  then it will have no value or its PromptItemValue will be selected
                'the hosting app should have logic to check the Required property to determine
                '  if this routine needs to be run
                'should return any error message with FriendlyName prefix, if any,
                ' and set value of ErrorText to error message without FriendlyName prefix
                Dim RetVal As String = String.Empty
                Dim strErrorText As String = String.Empty

                If (Me.SelectedItem Is Nothing) OrElse (Me.SelectedItem.Text = Me.PromptItemValue) Then
                    strErrorText = "Selection required."
                    If Me.FriendlyName.Trim.Length = 0 Then
                        RetVal = "Selection required."
                    Else
                        RetVal = Me.FriendlyName & " - selection is required."
                    End If
                Else
                    'nothing else to validate since dropdownlists by nature show only valid data selections
                End If

                Me.ErrorText = strErrorText

                Return RetVal
            End Function

            ''' <summary>
            ''' Clears control's error text and optionally clears associated error label control
            ''' </summary>
            ''' <param name="ClearErrorLabel">Set to True to clear associated error label control</param>
            ''' <remarks></remarks>
            Public Sub ClearErrors(ByVal ClearErrorLabel As Boolean)
                Me.ErrorText = String.Empty
            End Sub

            Public Sub Highlight(ByVal HighlightColor As System.Drawing.Color)
                Me.BackColor = HighlightColor
            End Sub

            Public Sub Unhighlight()
                Me.BackColor = Nothing
            End Sub

            <Description("Text to display if control value is in error."),
            PersistenceMode(PersistenceMode.Attribute),
            DefaultValue(""),
            Category("NK5"),
            Bindable(True),
            Localizable(True)>
            Public Property ErrorText() As String
                Get
                    Dim value As String = CStr(ViewState("ErrorText"))
                    If value Is Nothing Then
                        Return String.Empty
                    Else
                        Return value
                    End If
                End Get
                Set(ByVal value As String)
                    ViewState("ErrorText") = value
                End Set
            End Property
        End Class

        '<AspNetHostingPermission(SecurityAction.Demand, _
        'Level:=AspNetHostingPermissionLevel.Minimal), _
        'AspNetHostingPermission(SecurityAction.InheritanceDemand, _
        'Level:=AspNetHostingPermissionLevel.Minimal), _
        'PersistChildren(False), _
        'ParseChildren(True, "Text"), _
        <DefaultProperty("Text"),
        ToolboxData("<{0}:DropDownList runat=""server"" />")>
        Public Class DropDownList
            Inherits Base

        End Class

    End Namespace

    ' Inherits System.Web.UI.WebControls.Label
    Namespace Labels
        <AspNetHostingPermission(SecurityAction.Demand,
        Level:=AspNetHostingPermissionLevel.Minimal),
        AspNetHostingPermission(SecurityAction.InheritanceDemand,
        Level:=AspNetHostingPermissionLevel.Minimal),
        DefaultProperty("Text"),
        PersistChildren(False),
        ParseChildren(True)>
        Public Class Base
            Inherits System.Web.UI.WebControls.Label

            <Description("Miscellaneous data."),
            PersistenceMode(PersistenceMode.Attribute),
            DefaultValue(""),
            Category("NK5"),
            Bindable(True),
            Localizable(True)>
            Public Property Tag() As String
                Get
                    Dim value As String = CStr(ViewState("Tag"))
                    If value Is Nothing Then
                        Return String.Empty
                    Else
                        Return value
                    End If
                End Get
                Set(ByVal value As String)
                    ViewState("Tag") = value
                End Set
            End Property

            Public Sub Clear()
                Me.Text = String.Empty
            End Sub
        End Class

        <AspNetHostingPermission(SecurityAction.Demand,
        Level:=AspNetHostingPermissionLevel.Minimal),
        AspNetHostingPermission(SecurityAction.InheritanceDemand,
        Level:=AspNetHostingPermissionLevel.Minimal),
        DefaultProperty("Text"),
        PersistChildren(False),
        ParseChildren(True, "Text"),
        ToolboxData("<{0}:Label runat=""server"" />")>
        Public Class Label
            Inherits Base

        End Class

        <AspNetHostingPermission(SecurityAction.Demand,
        Level:=AspNetHostingPermissionLevel.Minimal),
        AspNetHostingPermission(SecurityAction.InheritanceDemand,
        Level:=AspNetHostingPermissionLevel.Minimal),
        DefaultProperty("Text"),
        PersistChildren(False),
        ParseChildren(True, "Text"),
        ToolboxData("<{0}:DataLabel runat=""server"" />")>
        Public Class DataLabel
            Inherits Base

            <Description("Sets casing of control data."),
            PersistenceMode(PersistenceMode.Attribute),
            DefaultValue(CasingOption.Normal),
            Category("NK5"),
            Bindable(True),
            Localizable(True)>
            Public Overridable Property Casing() As CasingOption
                Get
                    Dim value As String = CStr(ViewState("Casing"))
                    If (value Is Nothing) OrElse (Not IsNumeric(value)) Then
                        Return CasingOption.Normal
                    Else
                        Return CType(value, CasingOption)
                    End If
                End Get
                Set(ByVal value As CasingOption)
                    ViewState("Casing") = value
                End Set
            End Property

            Public Overrides Property Text() As String
                Get
                    Select Case Me.Casing
                        Case CasingOption.lower
                            Return MyBase.Text.ToLower
                        Case CasingOption.UPPER
                            Return MyBase.Text.ToUpper
                        Case Else
                            Return MyBase.Text
                    End Select
                End Get
                Set(ByVal value As String)
                    If value IsNot Nothing Then
                        value = value.Replace("<script", "").Replace("--", "")
                    End If
                    MyBase.Text = value
                End Set
            End Property

            <Description("Name of the Xml element used to store the control data."),
            PersistenceMode(PersistenceMode.Attribute),
            DefaultValue(""),
            Category("NK5"),
            Bindable(True),
            Localizable(True)>
            Public Overridable Property DataFieldName() As String
                Get
                    Dim value As String = CStr(ViewState("DataFieldName"))
                    If value Is Nothing Then
                        Return String.Empty
                    Else
                        Return value
                    End If
                End Get
                Set(ByVal value As String)
                    ViewState("DataFieldName") = value
                End Set
            End Property

            <Description("Indicates if the label's content is to be processed during a Save event."),
            PersistenceMode(PersistenceMode.Attribute),
            DefaultValue(True),
            Category("NK5"),
            Bindable(True),
            Localizable(True)>
            Public Overridable Property SaveData() As Boolean
                Get
                    Dim value As String = CStr(ViewState("SaveData"))
                    If value Is Nothing Then
                        Return True
                    Else
                        Return CBool(value)
                    End If
                End Get
                Set(ByVal value As Boolean)
                    ViewState("SaveData") = value
                End Set
            End Property

            <Description("Column name of Xml field when storing data in XML columns."),
            PersistenceMode(PersistenceMode.Attribute),
            DefaultValue(""),
            Category("NK5"),
            Bindable(True),
            Localizable(True)>
            Public Property XMLColumnName() As String
                Get
                    Dim value As String = CStr(ViewState("XMLColumnName"))
                    If value Is Nothing Then
                        Return String.Empty
                    Else
                        Return value
                    End If
                End Get
                Set(ByVal value As String)
                    ViewState("XMLColumnName") = value.Replace(" ", "")
                End Set
            End Property

            <Description("XPath to used when storing and retrieving data for this control."),
            PersistenceMode(PersistenceMode.Attribute),
            DefaultValue(""),
            Category("NK5"),
            Bindable(True),
            Localizable(True)>
            Public Property XmlPath() As String
                Get
                    Dim value As String = CStr(ViewState("XmlPath"))
                    If value Is Nothing Then
                        Return String.Empty
                    Else
                        Return value
                    End If
                End Get
                Set(ByVal value As String)
                    ViewState("XmlPath") = value.Replace(" ", "")
                End Set
            End Property

        End Class

        Public Class DateDataLabel
            Inherits DataLabel

            <Description("Named format to apply to underlying value."),
            PersistenceMode(PersistenceMode.Attribute),
            DefaultValue(Microsoft.VisualBasic.DateFormat.GeneralDate),
            Category("NK5"),
            Bindable(True),
            Localizable(True)>
            Public Property FormatNamed() As Microsoft.VisualBasic.DateFormat
                Get
                    Dim value As String = CStr(ViewState("FormatNamed"))
                    If (value Is Nothing) OrElse (Not IsNumeric(value)) Then
                        Return Microsoft.VisualBasic.DateFormat.GeneralDate
                    Else
                        Return CType(value, Microsoft.VisualBasic.DateFormat)
                    End If
                End Get
                Set(ByVal value As Microsoft.VisualBasic.DateFormat)
                    ViewState("FormatNamed") = value
                End Set
            End Property

            <Description("Custom format to apply to underlying value."),
            PersistenceMode(PersistenceMode.Attribute),
            DefaultValue(""),
            Category("NK5"),
            Bindable(True),
            Localizable(True)>
            Public Property FormatCustom() As String
                Get
                    Dim value As String = CStr(ViewState("FormatCustom"))
                    If value Is Nothing Then
                        Return String.Empty
                    Else
                        Return value
                    End If
                End Get
                Set(ByVal value As String)
                    ViewState("FormatCustom") = value
                End Set
            End Property

            Public Overrides Property Text() As String
                Get
                    Return Me.GetFormattedValue(CStr(ViewState("Text")))
                End Get
                Set(ByVal value As String)
                    ViewState("Text") = Me.GetFormattedValue(value)
                End Set
            End Property

            Private Function GetFormattedValue(ByVal value As Object) As String
                If (value Is Nothing) OrElse (Not IsDate(value)) Then
                    Return String.Empty
                End If

                If Me.FormatCustom.Trim.Length = 0 Then
                    Return FormatDateTime(CDate(value), Me.FormatNamed)
                Else
                    Return CDate(value).ToString(Me.FormatCustom)
                End If
            End Function
        End Class

        Public Class NumericDataLabel
            Inherits DataLabel

            <Description("Named format to apply to underlying value."),
            PersistenceMode(PersistenceMode.Attribute),
            DefaultValue(NumericFormat.Custom),
            Category("NK5"),
            Bindable(True),
            Localizable(True)>
            Public Property FormatNamed() As NumericFormat
                Get
                    Dim value As String = CStr(ViewState("FormatNamed"))
                    If (value Is Nothing) OrElse (Not IsNumeric(value)) Then
                        Return NumericFormat.Custom
                    Else
                        Return CType(value, NumericFormat)
                    End If
                End Get
                Set(ByVal value As NumericFormat)
                    ViewState("FormatNamed") = value
                End Set
            End Property

            <Description("Number of decimals to display if using a Named format."),
            PersistenceMode(PersistenceMode.Attribute),
            DefaultValue(0),
            Category("NK5"),
            Bindable(True),
            Localizable(True)>
            Public Property FormatNamedDecimals() As Integer
                Get
                    Dim value As String = CStr(ViewState("FormatNamedDecimals"))
                    If (value Is Nothing) OrElse (Not IsNumeric(value)) Then
                        Return 0
                    Else
                        Return CInt(value)
                    End If
                End Get
                Set(ByVal value As Integer)
                    ViewState("FormatNamedDecimals") = value
                End Set
            End Property

            <Description("Custom format to apply to underlying value."),
            PersistenceMode(PersistenceMode.Attribute),
            DefaultValue(""),
            Category("NK5"),
            Bindable(True),
            Localizable(True)>
            Public Property FormatCustom() As String
                Get
                    Dim value As String = CStr(ViewState("DisplayFormat"))
                    If value Is Nothing Then
                        Return String.Empty
                    Else
                        Return value
                    End If
                End Get
                Set(ByVal value As String)
                    ViewState("DisplayFormat") = value
                End Set
            End Property

            Public Overrides Property Text() As String
                Get
                    Return Me.GetFormattedValue(CStr(ViewState("Text")))
                End Get
                Set(ByVal value As String)
                    ViewState("Text") = Me.GetFormattedValue(value)
                End Set
            End Property

            <Description("Value to display instead of 0."),
            PersistenceMode(PersistenceMode.Attribute),
            DefaultValue(""),
            Category("NK5"),
            Bindable(True),
            Localizable(True)>
            Public Property TextIfZero() As String
                Get
                    Dim value As String = CStr(ViewState("TextIfZero"))
                    If value Is Nothing Then
                        Return String.Empty
                    Else
                        Return value
                    End If
                End Get
                Set(ByVal value As String)
                    ViewState("TextIfZero") = value
                End Set
            End Property

            Private Function GetFormattedValue(ByVal value As Object) As String
                If value Is Nothing Then
                    value = 0
                ElseIf value.ToString.Contains("%") Then
                    value = value.ToString.Replace("%", "")
                    If IsNumeric(value) Then
                        value = (CDec(value) / 100).ToString
                    End If
                End If

                If Not IsNumeric(value) Then
                    Return String.Empty
                End If

                If (value.ToString = 0.ToString) And (Me.TextIfZero.Length <> 0) Then
                    Return Me.TextIfZero
                End If

                Select Case Me.FormatNamed
                    Case NumericFormat.Currency
                        Return FormatCurrency(value, Me.FormatNamedDecimals)
                    Case NumericFormat.Percent
                        Return FormatPercent(value, Me.FormatNamedDecimals)
                    Case NumericFormat.Number
                        Return FormatNumber(value, Me.FormatNamedDecimals)
                    Case Else
                        Return Format(CDec(value), Me.FormatCustom)
                End Select
            End Function
        End Class
    End Namespace

    ' Inherits Telerik.Web.UI.RadButton
    Namespace CheckBoxes
        <AspNetHostingPermission(SecurityAction.Demand,
        Level:=AspNetHostingPermissionLevel.Minimal),
        AspNetHostingPermission(SecurityAction.InheritanceDemand,
        Level:=AspNetHostingPermissionLevel.Minimal),
        DefaultProperty("Checked"),
        PersistChildren(False),
        ParseChildren(True)>
        Public Class Base
            Inherits Telerik.Web.UI.RadButton

            <Description("Miscellaneous data."),
            PersistenceMode(PersistenceMode.Attribute),
            DefaultValue(""),
            Category("NK5"),
            Bindable(True),
            Localizable(True)>
            Public Property Tag() As String
                Get
                    Dim value As String = CStr(ViewState("Tag"))
                    If value Is Nothing Then
                        Return String.Empty
                    Else
                        Return value
                    End If
                End Get
                Set(ByVal value As String)
                    ViewState("Tag") = value
                End Set
            End Property

            <Description("Name of the Xml element used to store the control data."),
            PersistenceMode(PersistenceMode.Attribute),
            DefaultValue(""),
            Category("NK5"),
            Bindable(True),
            Localizable(True)>
            Public Overridable Property DataFieldName() As String
                Get
                    Dim value As String = CStr(ViewState("DataFieldName"))
                    If value Is Nothing Then
                        Return String.Empty
                    Else
                        Return value
                    End If
                End Get
                Set(ByVal value As String)
                    ViewState("DataFieldName") = value
                End Set
            End Property

            <Description("Indicates if the control requires input."),
            PersistenceMode(PersistenceMode.Attribute),
            DefaultValue(False),
            Category("NK5"),
            Bindable(True),
            Localizable(True)>
            Public Overridable Property Required() As Boolean
                Get
                    Dim value As String = CStr(ViewState("Required"))
                    If value Is Nothing Then
                        Return False
                    Else
                        Return CBool(value)
                    End If
                End Get
                Set(ByVal value As Boolean)
                    ViewState("Required") = value
                End Set
            End Property

            <Description("Column name of Xml field when storing data in XML columns."),
            PersistenceMode(PersistenceMode.Attribute),
            DefaultValue(""),
            Category("NK5"),
            Bindable(True),
            Localizable(True)>
            Public Property XMLColumnName() As String
                Get
                    Dim value As String = CStr(ViewState("XMLColumnName"))
                    If value Is Nothing Then
                        Return String.Empty
                    Else
                        Return value
                    End If
                End Get
                Set(ByVal value As String)
                    ViewState("XMLColumnName") = value.Replace(" ", "")
                End Set
            End Property

            <Description("XPath to used when storing and retrieving data for this control."),
            PersistenceMode(PersistenceMode.Attribute),
            DefaultValue(""),
            Category("NK5"),
            Bindable(True),
            Localizable(True)>
            Public Property XmlPath() As String
                Get
                    Dim value As String = CStr(ViewState("XmlPath"))
                    If value Is Nothing Then
                        Return String.Empty
                    Else
                        Return value
                    End If
                End Get
                Set(ByVal value As String)
                    ViewState("XmlPath") = value.Replace(" ", "")
                End Set
            End Property

            <Description("Friendly name of control."),
            PersistenceMode(PersistenceMode.Attribute),
            DefaultValue(""),
            Category("NK5"),
            Bindable(True),
            Localizable(True)>
            Public Property FriendlyName() As String
                Get
                    Dim value As String = CStr(ViewState("FriendlyName"))
                    If value Is Nothing Then
                        If Me.DataFieldName = String.Empty Then
                            Return Me.ID
                        Else
                            Return Me.DataFieldName
                        End If
                    Else
                        Return value
                    End If
                End Get
                Set(ByVal value As String)
                    ViewState("FriendlyName") = value
                End Set
            End Property

            <Description("Text to display if control value is in error."),
            PersistenceMode(PersistenceMode.Attribute),
            DefaultValue(""),
            Category("NK5"),
            Bindable(True),
            Localizable(True)>
            Public Overridable Property ErrorText() As String
                Get
                    Dim value As String = CStr(ViewState("ErrorText"))
                    If value Is Nothing Then
                        Return String.Empty
                    Else
                        Return value
                    End If
                End Get
                Set(ByVal value As String)
                    ViewState("ErrorText") = value
                End Set
            End Property

            <Description(""),
            PersistenceMode(PersistenceMode.Attribute),
            DefaultValue(""),
            Category("NK5"),
            Bindable(True),
            Browsable(False),
            Localizable(True)>
            Public Overrides Property ToggleType() As Telerik.Web.UI.ButtonToggleType
                Get
                    Return Telerik.Web.UI.ButtonToggleType.CheckBox
                End Get
                Set(ByVal value As Telerik.Web.UI.ButtonToggleType)
                    ViewState("ToggleType") = value
                End Set
            End Property

            <Description(""),
            PersistenceMode(PersistenceMode.Attribute),
            DefaultValue(""),
            Category("NK5"),
            Bindable(True),
            Browsable(False),
            Localizable(True)>
            Public Overrides Property ButtonType() As Telerik.Web.UI.RadButtonType
                Get
                    Return Telerik.Web.UI.RadButtonType.ToggleButton
                End Get
                Set(ByVal value As Telerik.Web.UI.RadButtonType)
                    ViewState("ToggleType") = value
                End Set
            End Property

            ''' <summary>
            ''' 
            ''' </summary>
            ''' <returns>String containing data input error description</returns>
            ''' <remarks></remarks>
            Public Function CheckErrors() As String
                'should return any error message with FriendlyName prefix, if any,
                ' and set value of ErrorText to error message without FriendlyName prefix
                Dim RetVal As String = String.Empty
                Dim strErrorText As String = String.Empty

                If (Me.Checked = False) Then
                    strErrorText = "Entry required."
                    If Me.FriendlyName.Trim.Length = 0 Then
                        RetVal = "Entry required."
                    Else
                        RetVal = Me.FriendlyName & " - entry is required."
                    End If
                End If

                Me.ErrorText = strErrorText

                Return RetVal
            End Function
            ''' <summary>
            ''' Clears control's error text and optionally clears associated error label control
            ''' </summary>
            ''' <param name="ClearErrorLabel">Set to True to clear associated error label control</param>
            ''' <remarks></remarks>
            Public Sub ClearErrors(ByVal ClearErrorLabel As Boolean)
                Me.ErrorText = String.Empty
            End Sub

        End Class

        <AspNetHostingPermission(SecurityAction.Demand,
        Level:=AspNetHostingPermissionLevel.Minimal),
        AspNetHostingPermission(SecurityAction.InheritanceDemand,
        Level:=AspNetHostingPermissionLevel.Minimal),
        DefaultProperty("Checked"),
        PersistChildren(False),
        ParseChildren(True, "Text"),
        ToolboxData("<{0}:CheckBox runat=""server"" />")>
        Public Class CheckBox
            Inherits Base

        End Class

    End Namespace

    ' Inherits Telerik.Web.UI.RadButton
    Namespace RadioButtons
        <AspNetHostingPermission(SecurityAction.Demand,
        Level:=AspNetHostingPermissionLevel.Minimal),
        AspNetHostingPermission(SecurityAction.InheritanceDemand,
        Level:=AspNetHostingPermissionLevel.Minimal),
        DefaultProperty("Checked"),
        PersistChildren(False),
        ParseChildren(True)>
        Public Class Base
            Inherits Telerik.Web.UI.RadButton

            <Description("Miscellaneous data."),
            PersistenceMode(PersistenceMode.Attribute),
            DefaultValue(""),
            Category("NK5"),
            Bindable(True),
            Localizable(True)>
            Public Property Tag() As String
                Get
                    Dim value As String = CStr(ViewState("Tag"))
                    If value Is Nothing Then
                        Return String.Empty
                    Else
                        Return value
                    End If
                End Get
                Set(ByVal value As String)
                    ViewState("Tag") = value
                End Set
            End Property

            <Description("Name of the Xml element used to store the control data."),
            PersistenceMode(PersistenceMode.Attribute),
            DefaultValue(""),
            Category("NK5"),
            Bindable(True),
            Localizable(True)>
            Public Overridable Property DataFieldName() As String
                Get
                    Dim value As String = CStr(ViewState("DataFieldName"))
                    If value Is Nothing Then
                        Return String.Empty
                    Else
                        Return value
                    End If
                End Get
                Set(ByVal value As String)
                    ViewState("DataFieldName") = value
                End Set
            End Property

            <Description("Indicates if the control requires input."),
            PersistenceMode(PersistenceMode.Attribute),
            DefaultValue(False),
            Category("NK5"),
            Bindable(True),
            Localizable(True)>
            Public Overridable Property Required() As Boolean
                Get
                    Dim value As String = CStr(ViewState("Required"))
                    If value Is Nothing Then
                        Return False
                    Else
                        Return CBool(value)
                    End If
                End Get
                Set(ByVal value As Boolean)
                    ViewState("Required") = value
                End Set
            End Property

            <Description("Column name of Xml field when storing data in XML columns."),
            PersistenceMode(PersistenceMode.Attribute),
            DefaultValue(""),
            Category("NK5"),
            Bindable(True),
            Localizable(True)>
            Public Property XMLColumnName() As String
                Get
                    Dim value As String = CStr(ViewState("XMLColumnName"))
                    If value Is Nothing Then
                        Return String.Empty
                    Else
                        Return value
                    End If
                End Get
                Set(ByVal value As String)
                    ViewState("XMLColumnName") = value.Replace(" ", "")
                End Set
            End Property

            <Description("XPath to used when storing and retrieving data for this control."),
            PersistenceMode(PersistenceMode.Attribute),
            DefaultValue(""),
            Category("NK5"),
            Bindable(True),
            Localizable(True)>
            Public Property XmlPath() As String
                Get
                    Dim value As String = CStr(ViewState("XmlPath"))
                    If value Is Nothing Then
                        Return String.Empty
                    Else
                        Return value
                    End If
                End Get
                Set(ByVal value As String)
                    ViewState("XmlPath") = value.Replace(" ", "")
                End Set
            End Property

            <Description("Friendly name of control."),
            PersistenceMode(PersistenceMode.Attribute),
            DefaultValue(""),
            Category("NK5"),
            Bindable(True),
            Localizable(True)>
            Public Property FriendlyName() As String
                Get
                    Dim value As String = CStr(ViewState("FriendlyName"))
                    If value Is Nothing Then
                        If Me.DataFieldName = String.Empty Then
                            Return Me.ID
                        Else
                            Return Me.DataFieldName
                        End If
                    Else
                        Return value
                    End If
                End Get
                Set(ByVal value As String)
                    ViewState("FriendlyName") = value
                End Set
            End Property

            <Description("Text to display if control value is in error."),
            PersistenceMode(PersistenceMode.Attribute),
            DefaultValue(""),
            Category("NK5"),
            Bindable(True),
            Localizable(True)>
            Public Overridable Property ErrorText() As String
                Get
                    Dim value As String = CStr(ViewState("ErrorText"))
                    If value Is Nothing Then
                        Return String.Empty
                    Else
                        Return value
                    End If
                End Get
                Set(ByVal value As String)
                    ViewState("ErrorText") = value
                End Set
            End Property

            <Description(""),
            PersistenceMode(PersistenceMode.Attribute),
            DefaultValue(""),
            Category("NK5"),
            Bindable(True),
            Browsable(False),
            Localizable(True)>
            Public Overrides Property ToggleType() As Telerik.Web.UI.ButtonToggleType
                Get
                    Return Telerik.Web.UI.ButtonToggleType.Radio
                End Get
                Set(ByVal value As Telerik.Web.UI.ButtonToggleType)
                    ViewState("ToggleType") = value
                End Set
            End Property

            <Description(""),
            PersistenceMode(PersistenceMode.Attribute),
            DefaultValue(""),
            Category("NK5"),
            Bindable(True),
            Browsable(False),
            Localizable(True)>
            Public Overrides Property ButtonType() As Telerik.Web.UI.RadButtonType
                Get
                    Return Telerik.Web.UI.RadButtonType.ToggleButton
                End Get
                Set(ByVal value As Telerik.Web.UI.RadButtonType)
                    ViewState("ToggleType") = value
                End Set
            End Property

            ''' <summary>
            ''' 
            ''' </summary>
            ''' <returns>String containing data input error description</returns>
            ''' <remarks></remarks>
            Public Function CheckErrors() As String
                'should return any error message with FriendlyName prefix, if any,
                ' and set value of ErrorText to error message without FriendlyName prefix
                Dim RetVal As String = String.Empty
                Dim strErrorText As String = String.Empty

                If (Me.Checked = False) Then
                    strErrorText = "Entry required."
                    If Me.FriendlyName.Trim.Length = 0 Then
                        RetVal = "Entry required."
                    Else
                        RetVal = Me.FriendlyName & " - entry is required."
                    End If
                End If

                Me.ErrorText = strErrorText

                Return RetVal
            End Function
            ''' <summary>
            ''' Clears control's error text and optionally clears associated error label control
            ''' </summary>
            ''' <param name="ClearErrorLabel">Set to True to clear associated error label control</param>
            ''' <remarks></remarks>
            Public Sub ClearErrors(ByVal ClearErrorLabel As Boolean)
                Me.ErrorText = String.Empty
            End Sub

        End Class

        <AspNetHostingPermission(SecurityAction.Demand,
        Level:=AspNetHostingPermissionLevel.Minimal),
        AspNetHostingPermission(SecurityAction.InheritanceDemand,
        Level:=AspNetHostingPermissionLevel.Minimal),
        DefaultProperty("Checked"),
        PersistChildren(False),
        ParseChildren(True, "Text"),
        ToolboxData("<{0}:RadioButton runat=""server"" />")>
        Public Class RadioButton
            Inherits Base

        End Class

    End Namespace


    Namespace Navigation

        <AspNetHostingPermission(SecurityAction.Demand,
        Level:=AspNetHostingPermissionLevel.Minimal),
        AspNetHostingPermission(SecurityAction.InheritanceDemand,
        Level:=AspNetHostingPermissionLevel.Minimal),
        ToolboxData("<{0}:ScrollPersister runat=""server"" />")>
        Public Class ScrollPersister
            Inherits System.Web.UI.Control

            Private m_theForm As New HtmlControls.HtmlForm

            Public Sub New()

            End Sub

            Private Function GetServerForm(ByVal parent As ControlCollection) As HtmlControls.HtmlForm
                Dim retVal As HtmlControls.HtmlForm = Nothing

                For Each child As Control In parent
                    If TypeOf child Is System.Web.UI.HtmlControls.HtmlForm Then
                        retVal = DirectCast(child, HtmlControls.HtmlForm)
                        Exit For
                    End If

                    If child.HasControls() Then
                        Dim tmp As HtmlControls.HtmlForm = GetServerForm(child.Controls)
                        If Not tmp Is Nothing Then retVal = tmp
                    End If
                Next

                Return retVal
            End Function

            Protected Overrides Sub OnInit(ByVal e As EventArgs)
                m_theForm = GetServerForm(Page.Controls)

                Dim hidScrollLeft As New HtmlInputHidden
                hidScrollLeft.ID = "scrollLeft"

                Dim hidScrollTop As New HtmlInputHidden
                hidScrollTop.ID = "scrollTop"

                Me.Controls.Add(hidScrollLeft)
                Me.Controls.Add(hidScrollTop)

                Dim script As String = ""

                ' ScrollPersister script
                script = "var scrollX, scrollY;" &
                vbCr & vbLf & "function ScrollPersister_GetCoords() {" &
                vbCr & vbLf & "    if (document.all) {" &
                vbCr & vbLf & "        if (!document.documentElement.scrollLeft) {" &
                vbCr & vbLf & "            scrollX = document.body.scrollLeft;" &
                vbCr & vbLf & "        } else {" &
                vbCr & vbLf & "            scrollX = document.documentElement.scrollLeft;" &
                vbCr & vbLf & "        }" &
                vbCr & vbLf & "        if (!document.documentElement.scrollTop) {" &
                vbCr & vbLf & "            scrollY = document.body.scrollTop;" &
                vbCr & vbLf & "        } else {" &
                vbCr & vbLf & "            scrollY = document.documentElement.scrollTop;" &
                vbCr & vbLf & "        }" &
                vbCr & vbLf & "    } else {" &
                vbCr & vbLf & "        scrollX = window.pageXOffset;" &
                vbCr & vbLf & "        scrollY = window.pageYOffset;" &
                vbCr & vbLf & "    }" &
                vbCr & vbLf & "    document.forms['" & m_theForm.ClientID & "']." & hidScrollLeft.ClientID & ".value = scrollX;" &
                vbCr & vbLf & "    document.forms['" & m_theForm.ClientID & "']." & hidScrollTop.ClientID & ".value = scrollY;" &
                vbCr & vbLf & "}" &
                vbCr & vbLf & "function ScrollPersister_Scroll() {" &
                vbCr & vbLf & "    var x = document.forms['" & m_theForm.ClientID & "']." & hidScrollLeft.ClientID & ".value = scrollX;" &
                vbCr & vbLf & "    var y = document.forms['" & m_theForm.ClientID & "']." & hidScrollTop.ClientID & ".value = scrollY;" &
                vbCr & vbLf & "    window.scrollTo(x, y);" &
                vbCr & vbLf & "}" &
                vbCr & vbLf & "window.onload = ScrollPersister_Scroll;" &
                vbCr & vbLf & "window.onscroll = ScrollPersister_GetCoords;" &
                vbCr & vbLf & "window.onclick = ScrollPersister_GetCoords;" &
                vbCr & vbLf & "window.onkeypress = ScrollPersister_GetCoords;"

                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.Page.GetType, "_Startup_NavigationScrollPersister", script, True)
            End Sub

            Protected Overrides Sub Render(ByVal writer As HtmlTextWriter)
                Page.VerifyRenderingInServerForm(Me)
                MyBase.Render(writer)
            End Sub
        End Class

    End Namespace


    Namespace Drawing

        <AspNetHostingPermission(SecurityAction.Demand,
        Level:=AspNetHostingPermissionLevel.Minimal),
        AspNetHostingPermission(SecurityAction.InheritanceDemand,
        Level:=AspNetHostingPermissionLevel.Minimal),
        ToolboxData("<{0}:GradientPanel runat=""server"" />")>
        Public Class GradientPanel
            Inherits System.Web.UI.WebControls.Panel

            Protected Overrides Sub AddAttributesToRender(ByVal writer As System.Web.UI.HtmlTextWriter)
                PaintGradient()
                MyBase.AddAttributesToRender(writer)
            End Sub

#Region "Properties"

            <Description("Starting Color for the Gradient."),
            PersistenceMode(PersistenceMode.Attribute),
            DefaultValue(GetType(Color), "White"),
            Category("NK5"),
            Bindable(True),
            Localizable(True)>
            Public Property StartColor() As System.Drawing.Color
                Get
                    Dim value As Color = Color.White
                    If ViewState("StartColor") IsNot Nothing Then value = CType(ViewState("StartColor"), Color)
                    Return value
                End Get
                Set(ByVal value As System.Drawing.Color)
                    ViewState("StartColor") = value
                End Set
            End Property

            <Description("Middle Color for the Gradient."),
            PersistenceMode(PersistenceMode.Attribute),
            DefaultValue(GetType(Color), "White"),
            Category("NK5"),
            Bindable(True),
            Localizable(True)>
            Public Property MiddleColor() As System.Drawing.Color
                Get
                    Dim value As Color = Color.White
                    If ViewState("MiddleColor") IsNot Nothing Then value = CType(ViewState("MiddleColor"), Color)
                    Return value
                End Get
                Set(ByVal value As System.Drawing.Color)
                    ViewState("MiddleColor") = value
                End Set
            End Property

            <Description("Ending Color for the Gradient."),
            PersistenceMode(PersistenceMode.Attribute),
            DefaultValue(GetType(Color), "White"),
            Category("NK5"),
            Bindable(True),
            Localizable(True)>
            Public Property EndColor() As System.Drawing.Color
                Get
                    Dim value As Color = Color.White
                    If ViewState("EndColor") IsNot Nothing Then value = CType(ViewState("EndColor"), Color)
                    Return value
                End Get
                Set(ByVal value As System.Drawing.Color)
                    ViewState("EndColor") = value
                End Set
            End Property

            <Description("Relative path to save the resulting image to."),
            PersistenceMode(PersistenceMode.Attribute),
            DefaultValue(""),
            Category("NK5"),
            Bindable(True),
            Localizable(True)>
            Public Property SavePath() As String
                Get
                    Dim value As String = ""
                    If ViewState("SavePath") IsNot Nothing Then value = ViewState("SavePath").ToString
                    Return value
                End Get
                Set(ByVal value As String)
                    ViewState("SavePath") = value
                End Set
            End Property

            <Description("Path image file has been saved to."),
            PersistenceMode(PersistenceMode.Attribute),
            DefaultValue(""),
            Browsable(False),
            Category("NK5"),
            Bindable(True),
            Localizable(True)>
            Public Property SavedPath() As String
                Get
                    Dim value As String = ""
                    If ViewState("SavedPath") IsNot Nothing Then value = ViewState("SavedPath").ToString
                    Return value
                End Get
                Set(ByVal value As String)
                    ViewState("SavedPath") = value
                End Set
            End Property

            <Description("Direction to draw the gradient."),
            PersistenceMode(PersistenceMode.Attribute),
            DefaultValue(GetType(Enums.GradientDirection), "Vertical"),
            Category("NK5"),
            Bindable(True),
            Localizable(True)>
            Public Property GradientDirection() As Enums.GradientDirection
                Get
                    Dim value As Enums.GradientDirection = Enums.GradientDirection.Horizontal
                    If ViewState("GradientDirection") IsNot Nothing Then value = CType(ViewState("GradientDirection"), Enums.GradientDirection)
                    Return value
                End Get
                Set(ByVal value As Enums.GradientDirection)
                    ViewState("GradientDirection") = value
                End Set
            End Property

            <Description("Image format to save the resulting image as."),
            PersistenceMode(PersistenceMode.Attribute),
            DefaultValue(GetType(Enums.SaveFormat), "Png"),
            Category("NK5"),
            Bindable(True),
            Localizable(True)>
            Public Property SaveFormat() As Enums.SaveFormat
                Get
                    Dim value As Enums.SaveFormat = Enums.SaveFormat.Png
                    If ViewState("SaveFormat") IsNot Nothing Then value = CType(ViewState("SaveFormat"), Enums.SaveFormat)
                    Return value
                End Get
                Set(ByVal value As Enums.SaveFormat)
                    ViewState("SaveFormat") = value
                End Set
            End Property

            <Description("Number of colors to use to build the graident."),
            PersistenceMode(PersistenceMode.Attribute),
            DefaultValue(GetType(Enums.GradientColors), "Three"),
            Category("NK5"),
            Bindable(True),
            Localizable(True)>
            Public Property NumColors() As Enums.GradientColors
                Get
                    Dim value As Enums.GradientColors = GradientColors.Three
                    If ViewState("NumColors") IsNot Nothing Then value = CType(ViewState("NumColors"), GradientColors)
                    Return value
                End Get
                Set(ByVal value As Enums.GradientColors)
                    ViewState("NumColors") = value
                End Set
            End Property

            <Browsable(False)>
            Public Overrides Property BackImageUrl As String
                Get
                    Dim value As String = ""
                    If ViewState("BackImageUrl") IsNot Nothing Then value = ViewState("BackImageUrl").ToString
                    Return value
                End Get
                Set(ByVal value As String)
                    MyBase.BackImageUrl = value
                End Set
            End Property

            <Browsable(False)>
            Public Overrides Property BackColor As System.Drawing.Color
                Get
                    Return Color.White
                End Get
                Set(ByVal value As System.Drawing.Color)
                    MyBase.BackColor = value
                End Set
            End Property

#End Region

            Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
                PaintGradient()
                MyBase.Render(writer)
            End Sub

            Private Sub PaintGradient()
                Try
                    ''If FindInControl(Me, Me.ClientID & "_PanelSizeHidden") Is Nothing AndAlso Me.ClientID IsNot Nothing AndAlso Me.Page IsNot Nothing Then
                    ''    Dim tb As New HtmlInputText
                    ''    tb.ID = Me.ClientID & "_PanelSizeHidden"
                    ''    Me.Controls.Add(tb)
                    ''Dim script As String = "document.getElementById('" & Me.ClientID & "_PanelSizeHidden" & "').value = document.getElementById('" & Me.ClientID & "').clientHeight + 'x' + document.getElementById('" & Me.ClientID & "').clientWidth;"
                    'Dim script As String = "document.getElementById('" & Me.ClientID & "').style.height = document.getElementById('" & Me.ClientID & "').clientHeight + 'px';"
                    ''script = "alert(document.getElementById('" & Me.ClientID & "').clientWidth);"
                    'ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType, Me.ClientID & "_PanelSizeScript", script, True)
                    ''End If

                    ''Dim txt As HtmlInputText = CType(FindInControl(Me, Me.ClientID & "_PanelSizeHidden"), HtmlInputText)

                    If Me.BackImageUrl = "" AndAlso Me.ClientID IsNot Nothing Then
                        Dim width As Integer = CInt(Me.Width.Value)
                        Dim height As Integer = CInt(Me.Height.Value)

                        Dim bmp As New Bitmap(width, height)
                        Dim g As Graphics = Graphics.FromImage(bmp)

                        Select Case Me.GradientDirection
                            Case Enums.GradientDirection.Vertical
                                Dim r1 As New Rectangle(0, 0, width, CInt(height / 2))
                                Dim r2 As New Rectangle(0, CInt(height / 2), width, CInt(height / 2))
                                If height > 22 Then r2 = New Rectangle(0, CInt(height / 2) - 1, width, CInt(height / 2) + 1)

                                Dim gradBrush1 As System.Drawing.Drawing2D.LinearGradientBrush
                                Dim gradBrush2 As System.Drawing.Drawing2D.LinearGradientBrush

                                If Me.NumColors = GradientColors.Three Then
                                    gradBrush1 = New System.Drawing.Drawing2D.LinearGradientBrush(r1, Me.StartColor, Me.MiddleColor, linearGradientMode:=Drawing2D.LinearGradientMode.Vertical)
                                    gradBrush2 = New System.Drawing.Drawing2D.LinearGradientBrush(r2, Me.MiddleColor, Me.EndColor, linearGradientMode:=Drawing2D.LinearGradientMode.Vertical)
                                    g.FillRectangle(gradBrush1, r1)
                                    g.FillRectangle(gradBrush2, r2)
                                ElseIf Me.NumColors = GradientColors.Two Then
                                    r1 = New Rectangle(0, 0, width, height)
                                    gradBrush1 = New System.Drawing.Drawing2D.LinearGradientBrush(r1, Me.StartColor, Me.EndColor, linearGradientMode:=Drawing2D.LinearGradientMode.Vertical)
                                    g.FillRectangle(gradBrush1, r1)
                                End If

                                g.Save()

                            Case Enums.GradientDirection.Horizontal
                                Dim r1 As New Rectangle(0, 0, CInt(width / 2), height)
                                Dim r2 As New Rectangle(CInt(width / 2), 0, CInt(width / 2), height)
                                If width > 22 Then r2 = New Rectangle(CInt(width / 2) - 1, 0, CInt(width / 2) + 1, height)

                                Dim gradBrush1 As System.Drawing.Drawing2D.LinearGradientBrush
                                Dim gradBrush2 As System.Drawing.Drawing2D.LinearGradientBrush

                                If Me.NumColors = GradientColors.Three Then
                                    gradBrush1 = New System.Drawing.Drawing2D.LinearGradientBrush(r1, Me.StartColor, Me.MiddleColor, linearGradientMode:=Drawing2D.LinearGradientMode.Horizontal)
                                    gradBrush2 = New System.Drawing.Drawing2D.LinearGradientBrush(r2, Me.MiddleColor, Me.EndColor, linearGradientMode:=Drawing2D.LinearGradientMode.Horizontal)
                                    g.FillRectangle(gradBrush1, r1)
                                    g.FillRectangle(gradBrush2, r2)
                                ElseIf Me.NumColors = GradientColors.Two Then
                                    r1 = New Rectangle(0, 0, width, height)
                                    gradBrush1 = New System.Drawing.Drawing2D.LinearGradientBrush(r1, Me.StartColor, Me.EndColor, linearGradientMode:=Drawing2D.LinearGradientMode.Horizontal)
                                    g.FillRectangle(gradBrush1, r1)
                                End If

                                g.Save()

                            Case Enums.GradientDirection.ForwardDiagonal
                                Dim r1 As New Rectangle(0, 0, width, height)

                                Dim gradBrush1 As System.Drawing.Drawing2D.LinearGradientBrush
                                gradBrush1 = New System.Drawing.Drawing2D.LinearGradientBrush(r1, Me.StartColor, Me.EndColor, linearGradientMode:=Drawing2D.LinearGradientMode.ForwardDiagonal)

                                g.FillRectangle(gradBrush1, r1)

                                g.Save()

                            Case Enums.GradientDirection.BackwardDiagonal
                                Dim r1 As New Rectangle(0, 0, width, height)

                                Dim gradBrush1 As System.Drawing.Drawing2D.LinearGradientBrush
                                gradBrush1 = New System.Drawing.Drawing2D.LinearGradientBrush(r1, Me.StartColor, Me.EndColor, linearGradientMode:=Drawing2D.LinearGradientMode.BackwardDiagonal)

                                g.FillRectangle(gradBrush1, r1)

                                g.Save()

                        End Select

                        Dim uPath As String = Me.SavePath.Trim
                        If uPath = "" Then uPath = "~/" & Me.ClientID & ".png"
                        Dim uFormat As Enums.SaveFormat = Me.SaveFormat
                        If uPath.Contains(Me.ClientID) Then uFormat = Enums.SaveFormat.Png

                        If uPath IsNot Nothing AndAlso uPath.Trim <> "" Then
                            Dim typ As Imaging.ImageFormat
                            Select Case uFormat
                                Case Enums.SaveFormat.Bmp : typ = Imaging.ImageFormat.Bmp
                                Case Enums.SaveFormat.Emf : typ = Imaging.ImageFormat.Emf
                                Case Enums.SaveFormat.Exif : typ = Imaging.ImageFormat.Exif
                                Case Enums.SaveFormat.Gif : typ = Imaging.ImageFormat.Gif
                                Case Enums.SaveFormat.Icon : typ = Imaging.ImageFormat.Icon
                                Case Enums.SaveFormat.Jpeg : typ = Imaging.ImageFormat.Jpeg
                                Case Enums.SaveFormat.MemoryBmp : typ = Imaging.ImageFormat.MemoryBmp
                                Case Enums.SaveFormat.Png : typ = Imaging.ImageFormat.Png
                                Case Enums.SaveFormat.Tiff : typ = Imaging.ImageFormat.Tiff
                                Case Enums.SaveFormat.Wmf : typ = Imaging.ImageFormat.Wmf
                                Case Else : typ = Imaging.ImageFormat.Png
                            End Select
                            bmp.Save(Me.Context.Server.MapPath(uPath), typ)
                            Me.SavedPath = uPath
                        End If

                        Me.BackImageUrl = Me.SavedPath
                    End If

                Catch ex As Exception

                End Try
            End Sub

            Public Sub New()
                PaintGradient()
            End Sub

        End Class

        <AspNetHostingPermission(SecurityAction.Demand,
        Level:=AspNetHostingPermissionLevel.Minimal),
        AspNetHostingPermission(SecurityAction.InheritanceDemand,
        Level:=AspNetHostingPermissionLevel.Minimal),
        ToolboxData("<{0}:GradientImage runat=""server"" />")>
        Public Class GradientImage
            Inherits System.Web.UI.WebControls.Image

            Protected Overrides Sub AddAttributesToRender(ByVal writer As System.Web.UI.HtmlTextWriter)
                PaintGradient()
                MyBase.AddAttributesToRender(writer)
            End Sub

#Region "Properties"

            <Description("Starting Color for the Gradient."),
            PersistenceMode(PersistenceMode.Attribute),
            DefaultValue(GetType(Color), "White"),
            Category("NK5"),
            Bindable(True),
            Localizable(True)>
            Public Property StartColor() As System.Drawing.Color
                Get
                    Dim value As Color = Color.White
                    If ViewState("StartColor") IsNot Nothing Then value = CType(ViewState("StartColor"), Color)
                    Return value
                End Get
                Set(ByVal value As System.Drawing.Color)
                    ViewState("StartColor") = value
                End Set
            End Property

            <Description("Middle Color for the Gradient."),
            PersistenceMode(PersistenceMode.Attribute),
            DefaultValue(GetType(Color), "White"),
            Category("NK5"),
            Bindable(True),
            Localizable(True)>
            Public Property MiddleColor() As System.Drawing.Color
                Get
                    Dim value As Color = Color.White
                    If ViewState("MiddleColor") IsNot Nothing Then value = CType(ViewState("MiddleColor"), Color)
                    Return value
                End Get
                Set(ByVal value As System.Drawing.Color)
                    ViewState("MiddleColor") = value
                End Set
            End Property

            <Description("Ending Color for the Gradient."),
            PersistenceMode(PersistenceMode.Attribute),
            DefaultValue(GetType(Color), "White"),
            Category("NK5"),
            Bindable(True),
            Localizable(True)>
            Public Property EndColor() As System.Drawing.Color
                Get
                    Dim value As Color = Color.White
                    If ViewState("EndColor") IsNot Nothing Then value = CType(ViewState("EndColor"), Color)
                    Return value
                End Get
                Set(ByVal value As System.Drawing.Color)
                    ViewState("EndColor") = value
                End Set
            End Property

            <Description("Relative path to save the resulting image to."),
            PersistenceMode(PersistenceMode.Attribute),
            DefaultValue(""),
            Category("NK5"),
            Bindable(True),
            Localizable(True)>
            Public Property SavePath() As String
                Get
                    Dim value As String = ""
                    If ViewState("SavePath") IsNot Nothing Then value = ViewState("SavePath").ToString
                    Return value
                End Get
                Set(ByVal value As String)
                    ViewState("SavePath") = value
                End Set
            End Property

            <Description("Path image file has been saved to."),
            PersistenceMode(PersistenceMode.Attribute),
            DefaultValue(""),
            Browsable(False),
            Category("NK5"),
            Bindable(True),
            Localizable(True)>
            Public Property SavedPath() As String
                Get
                    Dim value As String = ""
                    If ViewState("SavedPath") IsNot Nothing Then value = ViewState("SavedPath").ToString
                    Return value
                End Get
                Set(ByVal value As String)
                    ViewState("SavedPath") = value
                End Set
            End Property

            <Description("Direction to draw the gradient."),
            PersistenceMode(PersistenceMode.Attribute),
            DefaultValue(GetType(Enums.GradientDirection), "Vertical"),
            Category("NK5"),
            Bindable(True),
            Localizable(True)>
            Public Property GradientDirection() As Enums.GradientDirection
                Get
                    Dim value As Enums.GradientDirection = Enums.GradientDirection.Horizontal
                    If ViewState("GradientDirection") IsNot Nothing Then value = CType(ViewState("GradientDirection"), Enums.GradientDirection)
                    Return value
                End Get
                Set(ByVal value As Enums.GradientDirection)
                    ViewState("GradientDirection") = value
                    If value = Enums.GradientDirection.BackwardDiagonal Or value = Enums.GradientDirection.ForwardDiagonal Then
                        Me.NumColors = GradientColors.Two
                    End If
                End Set
            End Property

            <Description("Image format to save the resulting image as."),
            PersistenceMode(PersistenceMode.Attribute),
            DefaultValue(GetType(Enums.SaveFormat), "Png"),
            Category("NK5"),
            Bindable(True),
            Localizable(True)>
            Public Property SaveFormat() As Enums.SaveFormat
                Get
                    Dim value As Enums.SaveFormat = Enums.SaveFormat.Png
                    If ViewState("SaveFormat") IsNot Nothing Then value = CType(ViewState("SaveFormat"), Enums.SaveFormat)
                    Return value
                End Get
                Set(ByVal value As Enums.SaveFormat)
                    ViewState("SaveFormat") = value
                End Set
            End Property

            <Description("Number of colors to use to build the graident."),
            PersistenceMode(PersistenceMode.Attribute),
            DefaultValue(GetType(Enums.GradientColors), "Three"),
            Category("NK5"),
            Bindable(True),
            Localizable(True)>
            Public Property NumColors() As Enums.GradientColors
                Get
                    Dim value As Enums.GradientColors = GradientColors.Three
                    If ViewState("NumColors") IsNot Nothing Then value = CType(ViewState("NumColors"), GradientColors)
                    Return value
                End Get
                Set(ByVal value As Enums.GradientColors)
                    ViewState("NumColors") = value
                End Set
            End Property

            Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
                PaintGradient()
                MyBase.Render(writer)
            End Sub

            <Browsable(False)>
            Public Overrides Property ImageUrl As String
                Get
                    Dim value As String = ""
                    If ViewState("ImageUrl") IsNot Nothing Then value = ViewState("ImageUrl").ToString
                    Return value
                End Get
                Set(ByVal value As String)
                    MyBase.ImageUrl = value
                End Set
            End Property

            <Browsable(False)>
            Public Overrides Property BackColor As System.Drawing.Color
                Get
                    Return Color.White
                End Get
                Set(ByVal value As System.Drawing.Color)
                    MyBase.BackColor = value
                End Set
            End Property

#End Region

            Public Sub PaintGradient()
                Try
                    If Me.ImageUrl = "" And (Me.Width.Value > 0 Or Me.Height.Value > 0) Then
                        If Me.Width.Value = 0 Then Me.Width = Me.Height
                        If Me.Height.Value = 0 Then Me.Height = Me.Width

                        Dim width As Integer = CInt(Me.Width.Value)
                        Dim height As Integer = CInt(Me.Height.Value)

                        Dim bmp As New Bitmap(width, height)
                        Dim g As Graphics = Graphics.FromImage(bmp)

                        Select Case Me.GradientDirection
                            Case Enums.GradientDirection.Vertical
                                Dim r1 As New Rectangle(0, 0, width, CInt(height / 2))
                                Dim r2 As New Rectangle(0, CInt(height / 2), width, CInt(height / 2))
                                If height > 22 Then r2 = New Rectangle(0, CInt(height / 2) - 1, width, CInt(height / 2) + 1)

                                Dim gradBrush1 As System.Drawing.Drawing2D.LinearGradientBrush
                                Dim gradBrush2 As System.Drawing.Drawing2D.LinearGradientBrush

                                If Me.NumColors = GradientColors.Three Then
                                    gradBrush1 = New System.Drawing.Drawing2D.LinearGradientBrush(r1, Me.StartColor, Me.MiddleColor, linearGradientMode:=Drawing2D.LinearGradientMode.Vertical)
                                    gradBrush2 = New System.Drawing.Drawing2D.LinearGradientBrush(r2, Me.MiddleColor, Me.EndColor, linearGradientMode:=Drawing2D.LinearGradientMode.Vertical)
                                    g.FillRectangle(gradBrush1, r1)
                                    g.FillRectangle(gradBrush2, r2)
                                ElseIf Me.NumColors = GradientColors.Two Then
                                    r1 = New Rectangle(0, 0, width, height)
                                    gradBrush1 = New System.Drawing.Drawing2D.LinearGradientBrush(r1, Me.StartColor, Me.EndColor, linearGradientMode:=Drawing2D.LinearGradientMode.Vertical)
                                    g.FillRectangle(gradBrush1, r1)
                                End If

                                g.Save()

                            Case Enums.GradientDirection.Horizontal
                                Dim r1 As New Rectangle(0, 0, CInt(width / 2), height)
                                Dim r2 As New Rectangle(CInt(width / 2), 0, CInt(width / 2), height)
                                If width > 22 Then r2 = New Rectangle(CInt(width / 2) - 1, 0, CInt(width / 2) + 1, height)

                                Dim gradBrush1 As System.Drawing.Drawing2D.LinearGradientBrush
                                Dim gradBrush2 As System.Drawing.Drawing2D.LinearGradientBrush

                                If Me.NumColors = GradientColors.Three Then
                                    gradBrush1 = New System.Drawing.Drawing2D.LinearGradientBrush(r1, Me.StartColor, Me.MiddleColor, linearGradientMode:=Drawing2D.LinearGradientMode.Horizontal)
                                    gradBrush2 = New System.Drawing.Drawing2D.LinearGradientBrush(r2, Me.MiddleColor, Me.EndColor, linearGradientMode:=Drawing2D.LinearGradientMode.Horizontal)
                                    g.FillRectangle(gradBrush1, r1)
                                    g.FillRectangle(gradBrush2, r2)
                                ElseIf Me.NumColors = GradientColors.Two Then
                                    r1 = New Rectangle(0, 0, width, height)
                                    gradBrush1 = New System.Drawing.Drawing2D.LinearGradientBrush(r1, Me.StartColor, Me.EndColor, linearGradientMode:=Drawing2D.LinearGradientMode.Horizontal)
                                    g.FillRectangle(gradBrush1, r1)
                                End If

                                g.Save()

                            Case Enums.GradientDirection.ForwardDiagonal
                                Dim r1 As New Rectangle(0, 0, width, height)

                                Dim gradBrush1 As System.Drawing.Drawing2D.LinearGradientBrush
                                gradBrush1 = New System.Drawing.Drawing2D.LinearGradientBrush(r1, Me.StartColor, Me.EndColor, linearGradientMode:=Drawing2D.LinearGradientMode.ForwardDiagonal)

                                g.FillRectangle(gradBrush1, r1)

                                g.Save()

                            Case Enums.GradientDirection.BackwardDiagonal
                                Dim r1 As New Rectangle(0, 0, width, height)

                                Dim gradBrush1 As System.Drawing.Drawing2D.LinearGradientBrush
                                gradBrush1 = New System.Drawing.Drawing2D.LinearGradientBrush(r1, Me.StartColor, Me.EndColor, linearGradientMode:=Drawing2D.LinearGradientMode.BackwardDiagonal)

                                g.FillRectangle(gradBrush1, r1)

                                g.Save()

                        End Select

                        Dim uPath As String = Me.SavePath.Trim
                        If uPath = "" Then uPath = "~/" & Me.ClientID & ".png"
                        Dim uFormat As Enums.SaveFormat = Me.SaveFormat
                        If uPath.Contains(Me.ClientID) Then uFormat = Enums.SaveFormat.Png

                        If uPath IsNot Nothing AndAlso uPath.Trim <> "" Then
                            Dim typ As Imaging.ImageFormat
                            Select Case uFormat
                                Case Enums.SaveFormat.Bmp : typ = Imaging.ImageFormat.Bmp
                                Case Enums.SaveFormat.Emf : typ = Imaging.ImageFormat.Emf
                                Case Enums.SaveFormat.Exif : typ = Imaging.ImageFormat.Exif
                                Case Enums.SaveFormat.Gif : typ = Imaging.ImageFormat.Gif
                                Case Enums.SaveFormat.Icon : typ = Imaging.ImageFormat.Icon
                                Case Enums.SaveFormat.Jpeg : typ = Imaging.ImageFormat.Jpeg
                                Case Enums.SaveFormat.MemoryBmp : typ = Imaging.ImageFormat.MemoryBmp
                                Case Enums.SaveFormat.Png : typ = Imaging.ImageFormat.Png
                                Case Enums.SaveFormat.Tiff : typ = Imaging.ImageFormat.Tiff
                                Case Enums.SaveFormat.Wmf : typ = Imaging.ImageFormat.Wmf
                                Case Else : typ = Imaging.ImageFormat.Png
                            End Select
                            bmp.Save(Me.Context.Server.MapPath(uPath), typ)
                            Me.SavedPath = uPath
                        End If

                        Me.ImageUrl = Me.SavedPath
                    End If

                Catch ex As Exception

                End Try
            End Sub

            Public Sub New()
                PaintGradient()
            End Sub

        End Class

    End Namespace

End Namespace
