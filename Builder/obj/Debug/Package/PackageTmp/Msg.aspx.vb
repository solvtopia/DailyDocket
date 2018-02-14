Imports System.IO
Imports DailyDocket.CommonCore.Shared.Common

Public Class Msg
    Inherits System.Web.UI.Page

    Dim oPayments As Payments

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim usr As New SystemUser(131)
        'Me.TransferFileRecords()
        'Dim fr As New FileRecord(3)
        'oPayments = New Payments(ConfigurationManager.AppSettings("StripeMode"))

        'Dim usr As New SystemUser(40)
        'Dim success As Boolean = oPayments.CreateCharge(49.95, "John S. Doe - April, 2017", usr)

        'Dim htmlText As String = ""
        'Using sr As New StreamReader(Server.MapPath("~/1234567890-42017.html"))
        '    htmlText = sr.ReadToEnd()
        'End Using

        'Printing.HtmlToPdf(htmlText, Server.MapPath("~/1234567890-42017.pdf"))

        'Dim info As New Payments.ManagedAccountInfo
        'info.AccountNumber = "36017497241"
        'info.AccountRoutingNumber = "031176110"
        'info.BirthDay = 9
        'info.BirthMonth = 3
        'info.BirthYear = 1979
        'info.EntityType = Payments.EntityType.Individual
        'info.Email = "james@solvtopia.com"
        'info.FirstName = "Leland"
        'info.LastName = "Davis"
        'info.IPAddress = "24.140.52.138"
        'info.Address = "11886 Mill Race Street NW"
        'info.City = "Canal Fulton"
        'info.State = "OH"
        'info.Zip = "44614"
        'info.Last4SSN = "8054"
        'oPayments.CreateManagedAccount(info)

        'Dim ex As New Exception
        'ex.WriteToErrorLog(New ErrorLogEntry(If(Me.Page.OnMobile, Enums.ProjectName.MobileApp, Enums.ProjectName.Builder)))

        'Dim lst As List(Of IO.FileInfo) = CommonCore.Shared.Common.SearchDir(Server.MapPath("~/images"), "*.*", Enums.FileSort.Size)
    End Sub

    Private Sub TransferFileRecords()
        Dim cnOld As New SqlClient.SqlConnection("Data Source=mssql.anaxanet.com;Initial Catalog=wdivzylm_dailydocket;Persist Security Info=True;User ID=wdivzylm_dailydocket;Password=9dR00g326d;Pooling=false;MultipleActiveResultSets=True")

        Try
            Dim cmdOld As New SqlClient.SqlCommand("SELECT [ID], [xmlData] FROM [FileRecords] ORDER BY [ID];", cnOld)
            If cmdOld.Connection.State = ConnectionState.Closed Then cmdOld.Connection.Open()
            Dim rsOld As SqlClient.SqlDataReader = cmdOld.ExecuteReader
            Do While rsOld.Read
                ' create a new filerecord from the old xml
                Dim fr As New FileRecord
                fr = CType(fr.DeserializeFromXml(rsOld("xmlData").ToString), FileRecord)
                'fr.ID = rsOld("ID").ToString.ToInteger

                ' save to the new database
                fr.Save()
            Loop
            cmdOld.Cancel()
            rsOld.Close()

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(Enums.ProjectName.Builder))
        Finally
            cnOld.Close()
        End Try
    End Sub

End Class