Public Class Enums
    Public Enum NameType
        LastNameFirst
        FirstNameFirst
    End Enum

    Public Enum DBName
        ' common databases
        DailyDocket

        ' project specific databases
    End Enum
    Public Enum NotificationRecipientType
        Attorney
        Client
    End Enum
    Public Enum NotificationMessageType
        SMS
        Email
    End Enum
    Public Enum AttorneyType
        Defendant
        Plaintiff
        Unavailable
    End Enum
    Public Enum FileSort
        Name
        Size
    End Enum
    Public Enum ProjectName
        API
        BillingProcessor
        Builder
        FileProcessor
        MobileApp
        CommonCore
        CommonCoreShared
    End Enum
    Public Enum EmailType
        Client_Notification_Detail
        Client_Registration
        Client_Forgot_Password
        Client_Invoice
        Client_Receipt
        Client_Payment_Failed
        Client_Cancel_Service
        Sys_Registration
        Sys_Cancel_Service
        Client_TrialEnding
        Client_TrialExpired
        Custom
    End Enum
    Public Enum ScrapeType
        ReturnAll
        KeepTags
        RemoveAll
    End Enum
    Public Enum SystemModuleStatus
        Open = 0
        Pending = 1
        Completed = 2
    End Enum
    Public Enum NotificationType
        Custom
        Email
        SMS
    End Enum
    Public Enum NotificationLevel
        Detail
        Summary
    End Enum

    Public Enum RecordSource
        Email
        AdminCalendar
    End Enum

    Public Enum SystemModuleType
        Folder
        [Module]
    End Enum

    Public Enum SystemModulePriority
        Normal
        Emergency
    End Enum

    Public Enum TransactionType
        Unavailable
        [New]
        Existing
    End Enum

    Public Enum SystemQuestionType
        CheckBox
        DropDownList
        TextBox
        MemoField
        NumericTextBox
    End Enum

    Public Enum SystemUserPermissions
        User
        Administrator
        SystemAdministrator
        Solvtopia
        Technician
        Supervisor
    End Enum

    Public Enum SalesMan
        David = 1
        Jose = 2
        Web = 0
        AutoRegister = 3
    End Enum

    Public Enum IconSize
        Small
        Large
    End Enum

    Public Enum SystemMode
        Live
        Demo
    End Enum

    Public Enum LogEntry
        QueryRecord
        AddRecord
        EditRecord
        DeleteRecord

        UserLogin
        ChatSession

        UnDeleteRecord
    End Enum

    Public Enum CreditCardType
        Visa
        MasterCard
        Discover
        Amex
        DinersClub
        enRoute
        JCB
        Invalid
    End Enum

    Public Enum ValidationErrorType
        Warning
        [Error]
    End Enum

    Public Enum UserPlatform
        Unavailable = 100

        iPhone = 0
        iPod = 1
        iPad = 2

        AndroidPhone = 3
        AndroidTablet = 4

        WindowsPhone = 5

        Desktop = 6

        Api = 7
    End Enum

    Public Enum InformationPopupButtons
        YesNo
        OkCancel
        OkOnly
    End Enum

    Public Enum InformationPopupType
        DeleteFolder
        DeleteModule
        ErrorMessage
        InformationOnly
        MoveModule
        DeleteRecord
        DeleteUser
        DeleteReport
    End Enum

    Public Enum InformationPopupIcon
        [Error]
        Information
        Question
    End Enum

#Region "Control Enums"

    Public Enum NumericFormat
        Currency
        Custom
        Number
        Percent
    End Enum

    Public Enum CasingOption
        Normal
        UPPER
        lower
    End Enum

    Public Enum SaveFormat
        Bmp
        Emf
        Exif
        Gif
        Icon
        Jpeg
        MemoryBmp
        Png
        Tiff
        Wmf
    End Enum

    Public Enum GradientDirection
        Vertical
        Horizontal
        ForwardDiagonal
        BackwardDiagonal
    End Enum

    Public Enum GradientColors
        Two
        Three
    End Enum

#End Region

#Region "API Enums"

    Public Enum ApiResultCode
        success = 0
        failed = 1
    End Enum

#End Region

End Class
