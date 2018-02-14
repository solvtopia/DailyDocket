Imports DailyDocket.CommonCore.Shared.Common
Imports Telerik.Web.UI

Public Class SMSLog
    Inherits builderPage

#Region "Properties"

    Public ReadOnly Property ListType As String
        Get
            Dim retVal As String = Request.QueryString("type")
            If retVal = "" Then retVal = "history"

            Return retVal
        End Get
    End Property
    Public ReadOnly Property AttorneyID As Integer
        Get
            Return Request.QueryString("aid").ToInteger
        End Get
    End Property
    Public ReadOnly Property ClientRecordID As Integer
        Get
            Return Request.QueryString("id").ToInteger
        End Get
    End Property

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Me.LoadHistory()
        End If
    End Sub

    Private Sub LoadHistory()
        Dim cn As New SqlClient.SqlConnection(ConnectionString)

        Try
            Dim cmd As New SqlClient.SqlCommand("procSMSLog", cn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@type", Me.ListType)
            cmd.Parameters.AddWithValue("@attorneyID", Me.AttorneyID)
            cmd.Parameters.AddWithValue("@clientID", Me.ClientRecordID)
            If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
            Dim rs As SqlClient.SqlDataReader = cmd.ExecuteReader
            Do While rs.Read
                Dim processorStatus As String = rs("ProcessorStatus").ToString

                Dim pbr As New RadPanelItem
                pbr.Expanded = False

                Dim dateToShow As String = ""
                If Me.ListType.ToLower <> "history" Then
                    dateToShow = " on " & FormatDateTime(CDate(rs("dtInserted").ToString), DateFormat.GeneralDate)
                End If

                Dim middleText As String = ""
                If Me.ListType.ToLower = "queue" Then
                    pbr.Text = ("Email Notification in queue for " & rs("AttorneyName").ToString & " " & dateToShow).Trim
                Else
                    Select Case CType(rs("MessageType").ToString.ToInteger, Enums.NotificationMessageType)
                        Case Enums.NotificationMessageType.Email
                            pbr.Text = ("Email Notification sent to " & rs("ToEmail").ToString & " " & dateToShow).Trim
                        Case Enums.NotificationMessageType.SMS
                            pbr.Text = ("SMS Notification sent to " & rs("ToNumber").ToString & " " & dateToShow).Trim
                    End Select
                End If

                pbr.BorderStyle = BorderStyle.Solid
                pbr.BorderWidth = New Unit(1, UnitType.Pixel)
                pbr.BorderColor = GetColor("#C0C0C0")
                pbr.ContentTemplate = New ContentTemplate("Message: ", rs("Message").ToString)
                If Me.ListType.ToLower = "history" Or processorStatus.ToLower = "running" Then Me.RadPanelBar1.Items.Add(pbr)
            Loop
            cmd.Cancel()
            rs.Close()

            If Me.RadPanelBar1.Items.Count = 0 Then
                Dim pbr As New RadPanelItem
                pbr.Expanded = True
                pbr.Text = "No items yet to show in this view."
                pbr.BorderStyle = BorderStyle.Solid
                pbr.BorderWidth = New Unit(1, UnitType.Pixel)
                pbr.BorderColor = GetColor("#C0C0C0")
                Me.RadPanelBar1.Items.Add(pbr)
            End If

            Me.RadPanelBar1.CollapseAllItems()

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(App.CurrentUser.ID, App.CurrentClient.ID, Enums.ProjectName.Builder))
        Finally
            cn.Close()
        End Try
    End Sub

    Protected Sub btnResend_Click(sender As Object, e As EventArgs)
        'Dim btn As Telerik.Web.UI.RadButton = CType(sender, Telerik.Web.UI.RadButton)
        'Dim msgId As Integer = btn.ID.Split("_"c)(1).ToInteger

        'Dim cn As New SqlClient.SqlConnection(ConnectionString)

        'Try
        '    Dim cmd As New SqlClient.SqlCommand("SELECT [ToNumber], [Message] FROM [Sys_SMSLog] WHERE [ID] = " & msgId & ";", cn)
        '    If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
        '    Dim rs As SqlClient.SqlDataReader = cmd.ExecuteReader
        '    Do While rs.Read
        '        Messaging.SendTwilioNotification(rs("ToNumber").ToString, rs("Message").ToString)
        '    Loop
        '    cmd.Cancel()
        '    rs.Close()

        'Catch ex As Exception
        '    ex.WriteToErrorLog(New ErrorLogEntry(If(Me.Page.OnMobile, Enums.ProjectName.MobileApp, Enums.ProjectName.Builder)))
        'Finally
        '    cn.Close()
        'End Try
    End Sub
End Class