Imports System.Web

Public Class App
    Public Shared Property CurrentUser As SystemUser
        Get
            Dim retVal As New SystemUser
            If HttpContext.Current.Session IsNot Nothing AndAlso HttpContext.Current.Session("CurrentUser") IsNot Nothing Then retVal = CType(HttpContext.Current.Session("CurrentUser"), SystemUser)
            HttpContext.Current.Session("CurrentUser") = retVal
            Return retVal
        End Get
        Set(value As SystemUser)
            HttpContext.Current.Session("CurrentUser") = value
        End Set
    End Property

    Public Shared Property CurrentAttorney As AttorneyRecord
        Get
            Dim retVal As New AttorneyRecord
            If HttpContext.Current.Session IsNot Nothing AndAlso HttpContext.Current.Session("CurrentAttorney") IsNot Nothing Then retVal = CType(HttpContext.Current.Session("CurrentAttorney"), AttorneyRecord)
            HttpContext.Current.Session("CurrentAttorney") = retVal
            Return retVal
        End Get
        Set(value As AttorneyRecord)
            HttpContext.Current.Session("CurrentAttorney") = value
        End Set
    End Property

    Public Shared Property CurrentClient As SystemClient
        Get
            Dim retVal As New SystemClient
            If HttpContext.Current.Session IsNot Nothing AndAlso HttpContext.Current.Session("CurrentClient") IsNot Nothing Then retVal = CType(HttpContext.Current.Session("CurrentClient"), SystemClient)
            HttpContext.Current.Session("CurrentClient") = retVal
            Return retVal
        End Get
        Set(value As SystemClient)
            HttpContext.Current.Session("CurrentClient") = value
        End Set
    End Property

    Public Shared Property MyCases As List(Of MyAppointment)
        Get
            Dim retVal As New List(Of MyAppointment)
            If HttpContext.Current.Session IsNot Nothing AndAlso HttpContext.Current.Session("MyCases") IsNot Nothing Then retVal = CType(HttpContext.Current.Session("MyCases"), List(Of MyAppointment))
            HttpContext.Current.Session("MyCases") = retVal
            Return CType(HttpContext.Current.Session("MyCases"), List(Of MyAppointment))
        End Get
        Set(value As List(Of MyAppointment))
            HttpContext.Current.Session("MyCases") = value
        End Set
    End Property
    Public Shared Property SelectedCase As FileRecord
        Get
            Dim retVal As New FileRecord
            If HttpContext.Current.Session IsNot Nothing AndAlso HttpContext.Current.Session("SelectedCase") IsNot Nothing Then retVal = CType(HttpContext.Current.Session("SelectedCase"), FileRecord)
            HttpContext.Current.Session("SelectedCase") = retVal
            Return CType(HttpContext.Current.Session("SelectedCase"), FileRecord)
        End Get
        Set(value As FileRecord)
            HttpContext.Current.Session("SelectedCase") = value
        End Set
    End Property
    Public Shared Property DateList As List(Of Date)
        Get
            Dim retVal As New List(Of Date)
            If HttpContext.Current.Session IsNot Nothing AndAlso HttpContext.Current.Session("DateList") IsNot Nothing Then retVal = CType(HttpContext.Current.Session("DateList"), List(Of Date))
            HttpContext.Current.Session("DateList") = retVal
            Return CType(HttpContext.Current.Session("DateList"), List(Of Date))
        End Get
        Set(value As List(Of Date))
            HttpContext.Current.Session("DateList") = value
        End Set
    End Property

End Class
