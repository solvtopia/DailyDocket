Imports Telerik.Web.UI

Public Class Calendar
    Inherits builderPage

#Region "Properties"

    Private ReadOnly Property AttorneyID As Integer
        Get
            Dim ar As New AttorneyRecord(Request.QueryString("aid"))
            Return ar.ID
        End Get
    End Property

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If App.CurrentClient.ID > 0 Then
                If Me.OnPhone Then
                    Me.RadSearchGridMobile.DataSource = App.MyCases
                    Me.RadSearchGridMobile.DataBind()
                Else
                    Me.radScheduler.DataSource = App.MyCases
                    Me.radScheduler.DataBind()
                End If
            Else
                App.MyCases = Nothing
                App.SelectedCase = Nothing
                App.DateList = Nothing
            End If
        End If

        If Me.AttorneyID > 0 Then
            Me.LoadDates()
        End If

        If Me.OnPhone Then
            Me.radScheduler.SelectedView = SchedulerViewType.AgendaView
            Me.radScheduler.ShowViewTabs = False
            Me.radScheduler.Visible = False
            Me.RadSearchGridMobile.Visible = True
            Me.pnlDates.Visible = False
        Else
            Me.radScheduler.SelectedView = SchedulerViewType.AgendaView
            Me.radScheduler.ShowViewTabs = True
            Me.radScheduler.Visible = True
            Me.RadSearchGridMobile.Visible = False
            Me.pnlDates.Visible = True
        End If

        Me.Master.BackButton.Visible = (Me.OnMobile)
        AddHandler Me.Master.BackButton.Click, AddressOf imgBack_Click
    End Sub

    Private Sub LoadDates()
        ' sort the date list
        App.DateList.Sort(Function(p1, p2) p1.Date.CompareTo(p2.Date))

        Dim x As Integer = 0

        Me.tblDates.Rows.Clear()
        Dim tr As New TableRow
        For Each dt As Date In App.DateList
            If dt >= Now.Date Then
                ' add the date to the table at the top of the page for quick reference
                Dim tc As New TableCell
                Dim styleString As String = "display: inline-block;"
                styleString &= " padding: 10px;"
                tc.Attributes.Add("style", styleString)
                Dim lnk As New LinkButton
                lnk.ID = "lnkDate_" & x
                lnk.CommandName = dt.ToString
                lnk.Text = dt.ToLongDateString
                AddHandler lnk.Click, AddressOf lnkDate_Click
                tc.Controls.Add(lnk)
                tr.Cells.Add(tc)

                x += 1
            End If
        Next
        Me.tblDates.Rows.Add(tr)
    End Sub

    Private Sub lnkDate_Click(sender As Object, e As EventArgs)
        Dim lnk As LinkButton = CType(sender, LinkButton)
        Me.radScheduler.SelectedDate = CDate(lnk.CommandName)
    End Sub

    Private Sub radScheduler_AppointmentInsert(sender As Object, e As AppointmentInsertEventArgs) Handles radScheduler.AppointmentInsert
        If radScheduler.Appointments.GetAppointmentsInRange(e.Appointment.Start, e.Appointment.[End]).Count > 0 Then
            e.Cancel = True
        End If
    End Sub

    Private Sub radScheduler_AppointmentUpdate(sender As Object, e As AppointmentUpdateEventArgs) Handles radScheduler.AppointmentUpdate
        If radScheduler.Appointments.GetAppointmentsInRange(e.ModifiedAppointment.Start, e.ModifiedAppointment.[End]).Count > 0 Then
            For Each a As Appointment In radScheduler.Appointments.GetAppointmentsInRange(e.ModifiedAppointment.Start, e.ModifiedAppointment.[End])
                If a.ID IsNot e.Appointment.ID Then
                    e.Cancel = True
                End If
            Next
        End If
    End Sub

    Protected Sub imgBack_Click(sender As Object, e As ImageClickEventArgs)
        Dim usrString As String = If(Me.DeviceId <> "", "deviceid=" & Me.DeviceId, "uid=" & Me.UserId)
        Response.Redirect("~/Default.aspx?" & usrString, False)
    End Sub
End Class