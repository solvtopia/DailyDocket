<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class fMain
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.btnProcessFiles = New System.Windows.Forms.Button()
        Me.lblTotalTime = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.tmrTimer = New System.Windows.Forms.Timer(Me.components)
        Me.Label3 = New System.Windows.Forms.Label()
        Me.lblSecondary = New System.Windows.Forms.Label()
        Me.lblPrimary = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.tmrAutoRun = New System.Windows.Forms.Timer(Me.components)
        Me.pbrPrimary = New System.Windows.Forms.ProgressBar()
        Me.pbrSecondary = New System.Windows.Forms.ProgressBar()
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.lstLog = New System.Windows.Forms.ListBox()
        Me.txtError = New System.Windows.Forms.TextBox()
        Me.TabPage2 = New System.Windows.Forms.TabPage()
        Me.chkAutoArchive = New System.Windows.Forms.CheckBox()
        Me.txtArchiveDays = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.chkCriminal = New System.Windows.Forms.CheckBox()
        Me.chkCivil = New System.Windows.Forms.CheckBox()
        Me.chkNotifications = New System.Windows.Forms.CheckBox()
        Me.chkAdminCalendars = New System.Windows.Forms.CheckBox()
        Me.TabPage3 = New System.Windows.Forms.TabPage()
        Me.btnCheckProcessed = New System.Windows.Forms.Button()
        Me.btnArchive = New System.Windows.Forms.Button()
        Me.btnAdminCalendars = New System.Windows.Forms.Button()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.btnTest = New System.Windows.Forms.Button()
        Me.btnProcessClients = New System.Windows.Forms.Button()
        Me.btnManualNotifications = New System.Windows.Forms.Button()
        Me.txtManualNotificationsDate = New System.Windows.Forms.TextBox()
        Me.btnAttorneyNames = New System.Windows.Forms.Button()
        Me.btnProcessAttorneys = New System.Windows.Forms.Button()
        Me.btnProcessAttorneyCases = New System.Windows.Forms.Button()
        Me.lblVersion = New System.Windows.Forms.Label()
        Me.TabControl1.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.TabPage2.SuspendLayout()
        Me.TabPage3.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnProcessFiles
        '
        Me.btnProcessFiles.Location = New System.Drawing.Point(689, 73)
        Me.btnProcessFiles.Margin = New System.Windows.Forms.Padding(4)
        Me.btnProcessFiles.Name = "btnProcessFiles"
        Me.btnProcessFiles.Size = New System.Drawing.Size(149, 28)
        Me.btnProcessFiles.TabIndex = 3
        Me.btnProcessFiles.Text = "Process Files"
        Me.btnProcessFiles.UseVisualStyleBackColor = True
        '
        'lblTotalTime
        '
        Me.lblTotalTime.AutoSize = True
        Me.lblTotalTime.Location = New System.Drawing.Point(119, 16)
        Me.lblTotalTime.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblTotalTime.Name = "lblTotalTime"
        Me.lblTotalTime.Size = New System.Drawing.Size(85, 17)
        Me.lblTotalTime.TabIndex = 5
        Me.lblTotalTime.Text = "lblTotalTime"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 16)
        Me.Label1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(98, 17)
        Me.Label1.TabIndex = 10
        Me.Label1.Text = "Process Time:"
        '
        'tmrTimer
        '
        Me.tmrTimer.Interval = 1000
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(688, 218)
        Me.Label3.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(111, 17)
        Me.Label3.TabIndex = 13
        Me.Label3.Text = "Secondary Task"
        '
        'lblSecondary
        '
        Me.lblSecondary.AutoSize = True
        Me.lblSecondary.Location = New System.Drawing.Point(689, 284)
        Me.lblSecondary.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblSecondary.Name = "lblSecondary"
        Me.lblSecondary.Size = New System.Drawing.Size(36, 17)
        Me.lblSecondary.TabIndex = 14
        Me.lblSecondary.Text = "0 / 0"
        '
        'lblPrimary
        '
        Me.lblPrimary.AutoSize = True
        Me.lblPrimary.Location = New System.Drawing.Point(689, 188)
        Me.lblPrimary.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblPrimary.Name = "lblPrimary"
        Me.lblPrimary.Size = New System.Drawing.Size(36, 17)
        Me.lblPrimary.TabIndex = 16
        Me.lblPrimary.Text = "0 / 0"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(689, 122)
        Me.Label5.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(95, 17)
        Me.Label5.TabIndex = 15
        Me.Label5.Text = "Primary Task:"
        '
        'tmrAutoRun
        '
        Me.tmrAutoRun.Enabled = True
        Me.tmrAutoRun.Interval = 60000
        '
        'pbrPrimary
        '
        Me.pbrPrimary.Location = New System.Drawing.Point(692, 153)
        Me.pbrPrimary.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.pbrPrimary.Name = "pbrPrimary"
        Me.pbrPrimary.Size = New System.Drawing.Size(143, 23)
        Me.pbrPrimary.TabIndex = 18
        '
        'pbrSecondary
        '
        Me.pbrSecondary.Location = New System.Drawing.Point(692, 249)
        Me.pbrSecondary.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.pbrSecondary.Name = "pbrSecondary"
        Me.pbrSecondary.Size = New System.Drawing.Size(143, 23)
        Me.pbrSecondary.TabIndex = 19
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.TabPage1)
        Me.TabControl1.Controls.Add(Me.TabPage2)
        Me.TabControl1.Controls.Add(Me.TabPage3)
        Me.TabControl1.Location = New System.Drawing.Point(15, 48)
        Me.TabControl1.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(667, 713)
        Me.TabControl1.TabIndex = 21
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.Label2)
        Me.TabPage1.Controls.Add(Me.lstLog)
        Me.TabPage1.Controls.Add(Me.txtError)
        Me.TabPage1.Location = New System.Drawing.Point(4, 25)
        Me.TabPage1.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.TabPage1.Size = New System.Drawing.Size(659, 684)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "Logs"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(3, 474)
        Me.Label2.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(51, 17)
        Me.Label2.TabIndex = 15
        Me.Label2.Text = "Errors:"
        '
        'lstLog
        '
        Me.lstLog.FormattingEnabled = True
        Me.lstLog.ItemHeight = 16
        Me.lstLog.Location = New System.Drawing.Point(7, 11)
        Me.lstLog.Margin = New System.Windows.Forms.Padding(4)
        Me.lstLog.Name = "lstLog"
        Me.lstLog.Size = New System.Drawing.Size(644, 452)
        Me.lstLog.TabIndex = 14
        '
        'txtError
        '
        Me.txtError.Location = New System.Drawing.Point(7, 498)
        Me.txtError.Margin = New System.Windows.Forms.Padding(4)
        Me.txtError.Multiline = True
        Me.txtError.Name = "txtError"
        Me.txtError.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtError.Size = New System.Drawing.Size(644, 176)
        Me.txtError.TabIndex = 13
        '
        'TabPage2
        '
        Me.TabPage2.Controls.Add(Me.chkAutoArchive)
        Me.TabPage2.Controls.Add(Me.txtArchiveDays)
        Me.TabPage2.Controls.Add(Me.Label6)
        Me.TabPage2.Controls.Add(Me.chkCriminal)
        Me.TabPage2.Controls.Add(Me.chkCivil)
        Me.TabPage2.Controls.Add(Me.chkNotifications)
        Me.TabPage2.Controls.Add(Me.chkAdminCalendars)
        Me.TabPage2.Location = New System.Drawing.Point(4, 25)
        Me.TabPage2.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.TabPage2.Size = New System.Drawing.Size(659, 684)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "Settings"
        Me.TabPage2.UseVisualStyleBackColor = True
        '
        'chkAutoArchive
        '
        Me.chkAutoArchive.AutoSize = True
        Me.chkAutoArchive.Checked = True
        Me.chkAutoArchive.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkAutoArchive.Location = New System.Drawing.Point(16, 186)
        Me.chkAutoArchive.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.chkAutoArchive.Name = "chkAutoArchive"
        Me.chkAutoArchive.Size = New System.Drawing.Size(140, 21)
        Me.chkAutoArchive.TabIndex = 36
        Me.chkAutoArchive.Text = "Run Auto Archive"
        Me.chkAutoArchive.UseVisualStyleBackColor = True
        '
        'txtArchiveDays
        '
        Me.txtArchiveDays.Location = New System.Drawing.Point(262, 184)
        Me.txtArchiveDays.Margin = New System.Windows.Forms.Padding(4)
        Me.txtArchiveDays.Name = "txtArchiveDays"
        Me.txtArchiveDays.Size = New System.Drawing.Size(132, 22)
        Me.txtArchiveDays.TabIndex = 35
        Me.txtArchiveDays.Text = "30"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(163, 187)
        Me.Label6.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(91, 17)
        Me.Label6.TabIndex = 34
        Me.Label6.Text = "Archive Days"
        '
        'chkCriminal
        '
        Me.chkCriminal.AutoSize = True
        Me.chkCriminal.Checked = True
        Me.chkCriminal.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkCriminal.Location = New System.Drawing.Point(16, 145)
        Me.chkCriminal.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.chkCriminal.Name = "chkCriminal"
        Me.chkCriminal.Size = New System.Drawing.Size(203, 21)
        Me.chkCriminal.TabIndex = 29
        Me.chkCriminal.Text = "Process Criminal Calendars"
        Me.chkCriminal.UseVisualStyleBackColor = True
        '
        'chkCivil
        '
        Me.chkCivil.AutoSize = True
        Me.chkCivil.Checked = True
        Me.chkCivil.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkCivil.Location = New System.Drawing.Point(16, 105)
        Me.chkCivil.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.chkCivil.Name = "chkCivil"
        Me.chkCivil.Size = New System.Drawing.Size(178, 21)
        Me.chkCivil.TabIndex = 28
        Me.chkCivil.Text = "Process Civil Calendars"
        Me.chkCivil.UseVisualStyleBackColor = True
        '
        'chkNotifications
        '
        Me.chkNotifications.AutoSize = True
        Me.chkNotifications.Checked = True
        Me.chkNotifications.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkNotifications.Location = New System.Drawing.Point(16, 64)
        Me.chkNotifications.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.chkNotifications.Name = "chkNotifications"
        Me.chkNotifications.Size = New System.Drawing.Size(162, 21)
        Me.chkNotifications.TabIndex = 27
        Me.chkNotifications.Text = "Process Notifications"
        Me.chkNotifications.UseVisualStyleBackColor = True
        '
        'chkAdminCalendars
        '
        Me.chkAdminCalendars.AutoSize = True
        Me.chkAdminCalendars.Checked = True
        Me.chkAdminCalendars.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkAdminCalendars.Location = New System.Drawing.Point(16, 23)
        Me.chkAdminCalendars.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.chkAdminCalendars.Name = "chkAdminCalendars"
        Me.chkAdminCalendars.Size = New System.Drawing.Size(192, 21)
        Me.chkAdminCalendars.TabIndex = 26
        Me.chkAdminCalendars.Text = "Process Admin Calendars"
        Me.chkAdminCalendars.UseVisualStyleBackColor = True
        '
        'TabPage3
        '
        Me.TabPage3.Controls.Add(Me.btnCheckProcessed)
        Me.TabPage3.Controls.Add(Me.btnArchive)
        Me.TabPage3.Controls.Add(Me.btnAdminCalendars)
        Me.TabPage3.Controls.Add(Me.Label4)
        Me.TabPage3.Controls.Add(Me.btnTest)
        Me.TabPage3.Controls.Add(Me.btnProcessClients)
        Me.TabPage3.Controls.Add(Me.btnManualNotifications)
        Me.TabPage3.Controls.Add(Me.txtManualNotificationsDate)
        Me.TabPage3.Controls.Add(Me.btnAttorneyNames)
        Me.TabPage3.Controls.Add(Me.btnProcessAttorneys)
        Me.TabPage3.Controls.Add(Me.btnProcessAttorneyCases)
        Me.TabPage3.Location = New System.Drawing.Point(4, 25)
        Me.TabPage3.Name = "TabPage3"
        Me.TabPage3.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage3.Size = New System.Drawing.Size(659, 684)
        Me.TabPage3.TabIndex = 2
        Me.TabPage3.Text = "Testing"
        Me.TabPage3.UseVisualStyleBackColor = True
        '
        'btnCheckProcessed
        '
        Me.btnCheckProcessed.Location = New System.Drawing.Point(17, 355)
        Me.btnCheckProcessed.Margin = New System.Windows.Forms.Padding(4)
        Me.btnCheckProcessed.Name = "btnCheckProcessed"
        Me.btnCheckProcessed.Size = New System.Drawing.Size(195, 28)
        Me.btnCheckProcessed.TabIndex = 29
        Me.btnCheckProcessed.Text = "Check Processed Files"
        Me.btnCheckProcessed.UseVisualStyleBackColor = True
        '
        'btnArchive
        '
        Me.btnArchive.Location = New System.Drawing.Point(17, 211)
        Me.btnArchive.Margin = New System.Windows.Forms.Padding(4)
        Me.btnArchive.Name = "btnArchive"
        Me.btnArchive.Size = New System.Drawing.Size(195, 28)
        Me.btnArchive.TabIndex = 30
        Me.btnArchive.Text = "Archive Cases"
        Me.btnArchive.UseVisualStyleBackColor = True
        '
        'btnAdminCalendars
        '
        Me.btnAdminCalendars.Location = New System.Drawing.Point(17, 319)
        Me.btnAdminCalendars.Margin = New System.Windows.Forms.Padding(4)
        Me.btnAdminCalendars.Name = "btnAdminCalendars"
        Me.btnAdminCalendars.Size = New System.Drawing.Size(195, 28)
        Me.btnAdminCalendars.TabIndex = 28
        Me.btnAdminCalendars.Text = "Admin Calendars"
        Me.btnAdminCalendars.UseVisualStyleBackColor = True
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(17, 22)
        Me.Label4.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(356, 17)
        Me.Label4.TabIndex = 37
        Me.Label4.Text = "Manually run notofications from the date below forward:"
        '
        'btnTest
        '
        Me.btnTest.Location = New System.Drawing.Point(17, 283)
        Me.btnTest.Margin = New System.Windows.Forms.Padding(4)
        Me.btnTest.Name = "btnTest"
        Me.btnTest.Size = New System.Drawing.Size(195, 28)
        Me.btnTest.TabIndex = 27
        Me.btnTest.Text = "Test File"
        Me.btnTest.UseVisualStyleBackColor = True
        '
        'btnProcessClients
        '
        Me.btnProcessClients.Location = New System.Drawing.Point(17, 247)
        Me.btnProcessClients.Margin = New System.Windows.Forms.Padding(4)
        Me.btnProcessClients.Name = "btnProcessClients"
        Me.btnProcessClients.Size = New System.Drawing.Size(195, 28)
        Me.btnProcessClients.TabIndex = 23
        Me.btnProcessClients.Text = "Process Clients"
        Me.btnProcessClients.UseVisualStyleBackColor = True
        '
        'btnManualNotifications
        '
        Me.btnManualNotifications.Location = New System.Drawing.Point(498, 45)
        Me.btnManualNotifications.Margin = New System.Windows.Forms.Padding(4)
        Me.btnManualNotifications.Name = "btnManualNotifications"
        Me.btnManualNotifications.Size = New System.Drawing.Size(141, 28)
        Me.btnManualNotifications.TabIndex = 35
        Me.btnManualNotifications.Text = "Manual Run"
        Me.btnManualNotifications.UseVisualStyleBackColor = True
        '
        'txtManualNotificationsDate
        '
        Me.txtManualNotificationsDate.Location = New System.Drawing.Point(17, 48)
        Me.txtManualNotificationsDate.Margin = New System.Windows.Forms.Padding(4)
        Me.txtManualNotificationsDate.Name = "txtManualNotificationsDate"
        Me.txtManualNotificationsDate.Size = New System.Drawing.Size(472, 22)
        Me.txtManualNotificationsDate.TabIndex = 34
        '
        'btnAttorneyNames
        '
        Me.btnAttorneyNames.Location = New System.Drawing.Point(17, 175)
        Me.btnAttorneyNames.Margin = New System.Windows.Forms.Padding(4)
        Me.btnAttorneyNames.Name = "btnAttorneyNames"
        Me.btnAttorneyNames.Size = New System.Drawing.Size(195, 28)
        Me.btnAttorneyNames.TabIndex = 26
        Me.btnAttorneyNames.Text = "Get All Attorney Names"
        Me.btnAttorneyNames.UseVisualStyleBackColor = True
        '
        'btnProcessAttorneys
        '
        Me.btnProcessAttorneys.Location = New System.Drawing.Point(17, 103)
        Me.btnProcessAttorneys.Margin = New System.Windows.Forms.Padding(4)
        Me.btnProcessAttorneys.Name = "btnProcessAttorneys"
        Me.btnProcessAttorneys.Size = New System.Drawing.Size(195, 28)
        Me.btnProcessAttorneys.TabIndex = 22
        Me.btnProcessAttorneys.Text = "Process Attorneys"
        Me.btnProcessAttorneys.UseVisualStyleBackColor = True
        '
        'btnProcessAttorneyCases
        '
        Me.btnProcessAttorneyCases.Location = New System.Drawing.Point(17, 139)
        Me.btnProcessAttorneyCases.Margin = New System.Windows.Forms.Padding(4)
        Me.btnProcessAttorneyCases.Name = "btnProcessAttorneyCases"
        Me.btnProcessAttorneyCases.Size = New System.Drawing.Size(195, 28)
        Me.btnProcessAttorneyCases.TabIndex = 25
        Me.btnProcessAttorneyCases.Text = "Process Attorney Cases"
        Me.btnProcessAttorneyCases.UseVisualStyleBackColor = True
        '
        'lblVersion
        '
        Me.lblVersion.Location = New System.Drawing.Point(563, 16)
        Me.lblVersion.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblVersion.Name = "lblVersion"
        Me.lblVersion.Size = New System.Drawing.Size(275, 28)
        Me.lblVersion.TabIndex = 22
        Me.lblVersion.Text = "Version: "
        Me.lblVersion.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'fMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(851, 772)
        Me.Controls.Add(Me.lblVersion)
        Me.Controls.Add(Me.TabControl1)
        Me.Controls.Add(Me.pbrSecondary)
        Me.Controls.Add(Me.pbrPrimary)
        Me.Controls.Add(Me.lblPrimary)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.lblSecondary)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.lblTotalTime)
        Me.Controls.Add(Me.btnProcessFiles)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.MaximizeBox = False
        Me.Name = "fMain"
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Daily Docket - File Processor"
        Me.TabControl1.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.TabPage1.PerformLayout()
        Me.TabPage2.ResumeLayout(False)
        Me.TabPage2.PerformLayout()
        Me.TabPage3.ResumeLayout(False)
        Me.TabPage3.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnProcessFiles As Button
    Friend WithEvents lblTotalTime As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents tmrTimer As Timer
    Friend WithEvents Label3 As Label
    Friend WithEvents lblSecondary As Label
    Friend WithEvents lblPrimary As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents tmrAutoRun As Timer
    Friend WithEvents pbrPrimary As ProgressBar
    Friend WithEvents pbrSecondary As ProgressBar
    Friend WithEvents TabControl1 As TabControl
    Friend WithEvents TabPage1 As TabPage
    Friend WithEvents TabPage2 As TabPage
    Friend WithEvents Label2 As Label
    Friend WithEvents lstLog As ListBox
    Friend WithEvents txtError As TextBox
    Friend WithEvents chkNotifications As CheckBox
    Friend WithEvents chkAdminCalendars As CheckBox
    Friend WithEvents chkCriminal As CheckBox
    Friend WithEvents chkCivil As CheckBox
    Friend WithEvents txtArchiveDays As TextBox
    Friend WithEvents Label6 As Label
    Friend WithEvents chkAutoArchive As CheckBox
    Friend WithEvents lblVersion As Label
    Friend WithEvents TabPage3 As TabPage
    Friend WithEvents btnCheckProcessed As Button
    Friend WithEvents btnArchive As Button
    Friend WithEvents btnAdminCalendars As Button
    Friend WithEvents Label4 As Label
    Friend WithEvents btnTest As Button
    Friend WithEvents btnProcessClients As Button
    Friend WithEvents btnManualNotifications As Button
    Friend WithEvents txtManualNotificationsDate As TextBox
    Friend WithEvents btnAttorneyNames As Button
    Friend WithEvents btnProcessAttorneys As Button
    Friend WithEvents btnProcessAttorneyCases As Button
End Class
