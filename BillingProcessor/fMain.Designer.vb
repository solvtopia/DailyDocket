<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class fMain
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
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
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.chkAutoRegister = New System.Windows.Forms.CheckBox()
        Me.TabPage2 = New System.Windows.Forms.TabPage()
        Me.chkTransfers = New System.Windows.Forms.CheckBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.lstLog = New System.Windows.Forms.ListBox()
        Me.txtError = New System.Windows.Forms.TextBox()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.TabPage3 = New System.Windows.Forms.TabPage()
        Me.btnFixClients = New System.Windows.Forms.Button()
        Me.btnAutoRegister = New System.Windows.Forms.Button()
        Me.pbrSecondary = New System.Windows.Forms.ProgressBar()
        Me.pbrPrimary = New System.Windows.Forms.ProgressBar()
        Me.tmrAutoRun = New System.Windows.Forms.Timer(Me.components)
        Me.lblPrimary = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.lblSecondary = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.tmrTimer = New System.Windows.Forms.Timer(Me.components)
        Me.Label1 = New System.Windows.Forms.Label()
        Me.lblTotalTime = New System.Windows.Forms.Label()
        Me.btnProcessPayments = New System.Windows.Forms.Button()
        Me.lblVersion = New System.Windows.Forms.Label()
        Me.btnProcessTransfers = New System.Windows.Forms.Button()
        Me.TabPage2.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.TabControl1.SuspendLayout()
        Me.TabPage3.SuspendLayout()
        Me.SuspendLayout()
        '
        'chkAutoRegister
        '
        Me.chkAutoRegister.AutoSize = True
        Me.chkAutoRegister.Checked = True
        Me.chkAutoRegister.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkAutoRegister.Location = New System.Drawing.Point(16, 23)
        Me.chkAutoRegister.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.chkAutoRegister.Name = "chkAutoRegister"
        Me.chkAutoRegister.Size = New System.Drawing.Size(205, 21)
        Me.chkAutoRegister.TabIndex = 26
        Me.chkAutoRegister.Text = "Auto Register From Preview"
        Me.chkAutoRegister.UseVisualStyleBackColor = True
        '
        'TabPage2
        '
        Me.TabPage2.Controls.Add(Me.chkTransfers)
        Me.TabPage2.Controls.Add(Me.chkAutoRegister)
        Me.TabPage2.Location = New System.Drawing.Point(4, 25)
        Me.TabPage2.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.TabPage2.Size = New System.Drawing.Size(659, 684)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "Settings"
        Me.TabPage2.UseVisualStyleBackColor = True
        '
        'chkTransfers
        '
        Me.chkTransfers.AutoSize = True
        Me.chkTransfers.Checked = True
        Me.chkTransfers.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkTransfers.Location = New System.Drawing.Point(16, 61)
        Me.chkTransfers.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.chkTransfers.Name = "chkTransfers"
        Me.chkTransfers.Size = New System.Drawing.Size(146, 21)
        Me.chkTransfers.TabIndex = 27
        Me.chkTransfers.Text = "Process Transfers"
        Me.chkTransfers.UseVisualStyleBackColor = True
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
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.TabPage1)
        Me.TabControl1.Controls.Add(Me.TabPage2)
        Me.TabControl1.Controls.Add(Me.TabPage3)
        Me.TabControl1.Location = New System.Drawing.Point(15, 46)
        Me.TabControl1.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(667, 713)
        Me.TabControl1.TabIndex = 31
        '
        'TabPage3
        '
        Me.TabPage3.Controls.Add(Me.btnProcessTransfers)
        Me.TabPage3.Controls.Add(Me.btnFixClients)
        Me.TabPage3.Controls.Add(Me.btnAutoRegister)
        Me.TabPage3.Location = New System.Drawing.Point(4, 25)
        Me.TabPage3.Name = "TabPage3"
        Me.TabPage3.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage3.Size = New System.Drawing.Size(659, 684)
        Me.TabPage3.TabIndex = 2
        Me.TabPage3.Text = "Testing"
        Me.TabPage3.UseVisualStyleBackColor = True
        '
        'btnFixClients
        '
        Me.btnFixClients.Location = New System.Drawing.Point(17, 56)
        Me.btnFixClients.Margin = New System.Windows.Forms.Padding(4)
        Me.btnFixClients.Name = "btnFixClients"
        Me.btnFixClients.Size = New System.Drawing.Size(195, 28)
        Me.btnFixClients.TabIndex = 24
        Me.btnFixClients.Text = "Fix Auto Register Clients"
        Me.btnFixClients.UseVisualStyleBackColor = True
        '
        'btnAutoRegister
        '
        Me.btnAutoRegister.Location = New System.Drawing.Point(17, 20)
        Me.btnAutoRegister.Margin = New System.Windows.Forms.Padding(4)
        Me.btnAutoRegister.Name = "btnAutoRegister"
        Me.btnAutoRegister.Size = New System.Drawing.Size(195, 28)
        Me.btnAutoRegister.TabIndex = 23
        Me.btnAutoRegister.Text = "Process Auto Register"
        Me.btnAutoRegister.UseVisualStyleBackColor = True
        '
        'pbrSecondary
        '
        Me.pbrSecondary.Location = New System.Drawing.Point(692, 246)
        Me.pbrSecondary.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.pbrSecondary.Name = "pbrSecondary"
        Me.pbrSecondary.Size = New System.Drawing.Size(143, 23)
        Me.pbrSecondary.TabIndex = 30
        '
        'pbrPrimary
        '
        Me.pbrPrimary.Location = New System.Drawing.Point(692, 150)
        Me.pbrPrimary.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.pbrPrimary.Name = "pbrPrimary"
        Me.pbrPrimary.Size = New System.Drawing.Size(143, 23)
        Me.pbrPrimary.TabIndex = 29
        '
        'tmrAutoRun
        '
        Me.tmrAutoRun.Enabled = True
        Me.tmrAutoRun.Interval = 60000
        '
        'lblPrimary
        '
        Me.lblPrimary.AutoSize = True
        Me.lblPrimary.Location = New System.Drawing.Point(689, 186)
        Me.lblPrimary.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblPrimary.Name = "lblPrimary"
        Me.lblPrimary.Size = New System.Drawing.Size(36, 17)
        Me.lblPrimary.TabIndex = 28
        Me.lblPrimary.Text = "0 / 0"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(689, 119)
        Me.Label5.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(95, 17)
        Me.Label5.TabIndex = 27
        Me.Label5.Text = "Primary Task:"
        '
        'lblSecondary
        '
        Me.lblSecondary.AutoSize = True
        Me.lblSecondary.Location = New System.Drawing.Point(689, 282)
        Me.lblSecondary.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblSecondary.Name = "lblSecondary"
        Me.lblSecondary.Size = New System.Drawing.Size(36, 17)
        Me.lblSecondary.TabIndex = 26
        Me.lblSecondary.Text = "0 / 0"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(688, 215)
        Me.Label3.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(111, 17)
        Me.Label3.TabIndex = 25
        Me.Label3.Text = "Secondary Task"
        '
        'tmrTimer
        '
        Me.tmrTimer.Interval = 1000
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 14)
        Me.Label1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(98, 17)
        Me.Label1.TabIndex = 24
        Me.Label1.Text = "Process Time:"
        '
        'lblTotalTime
        '
        Me.lblTotalTime.AutoSize = True
        Me.lblTotalTime.Location = New System.Drawing.Point(119, 14)
        Me.lblTotalTime.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblTotalTime.Name = "lblTotalTime"
        Me.lblTotalTime.Size = New System.Drawing.Size(85, 17)
        Me.lblTotalTime.TabIndex = 23
        Me.lblTotalTime.Text = "lblTotalTime"
        '
        'btnProcessPayments
        '
        Me.btnProcessPayments.Location = New System.Drawing.Point(689, 70)
        Me.btnProcessPayments.Margin = New System.Windows.Forms.Padding(4)
        Me.btnProcessPayments.Name = "btnProcessPayments"
        Me.btnProcessPayments.Size = New System.Drawing.Size(149, 28)
        Me.btnProcessPayments.TabIndex = 22
        Me.btnProcessPayments.Text = "Process Payments"
        Me.btnProcessPayments.UseVisualStyleBackColor = True
        '
        'lblVersion
        '
        Me.lblVersion.Location = New System.Drawing.Point(563, 14)
        Me.lblVersion.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblVersion.Name = "lblVersion"
        Me.lblVersion.Size = New System.Drawing.Size(275, 28)
        Me.lblVersion.TabIndex = 32
        Me.lblVersion.Text = "Version: "
        Me.lblVersion.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'btnProcessTransfers
        '
        Me.btnProcessTransfers.Location = New System.Drawing.Point(17, 92)
        Me.btnProcessTransfers.Margin = New System.Windows.Forms.Padding(4)
        Me.btnProcessTransfers.Name = "btnProcessTransfers"
        Me.btnProcessTransfers.Size = New System.Drawing.Size(195, 28)
        Me.btnProcessTransfers.TabIndex = 25
        Me.btnProcessTransfers.Text = "Process Transfers"
        Me.btnProcessTransfers.UseVisualStyleBackColor = True
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
        Me.Controls.Add(Me.btnProcessPayments)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.MaximizeBox = False
        Me.Name = "fMain"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Daily Docket - Billing Processor"
        Me.TabPage2.ResumeLayout(False)
        Me.TabPage2.PerformLayout()
        Me.TabPage1.ResumeLayout(False)
        Me.TabPage1.PerformLayout()
        Me.TabControl1.ResumeLayout(False)
        Me.TabPage3.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents chkAutoRegister As CheckBox
    Friend WithEvents TabPage2 As TabPage
    Friend WithEvents Label2 As Label
    Friend WithEvents lstLog As ListBox
    Friend WithEvents txtError As TextBox
    Friend WithEvents TabPage1 As TabPage
    Friend WithEvents TabControl1 As TabControl
    Friend WithEvents pbrSecondary As ProgressBar
    Friend WithEvents pbrPrimary As ProgressBar
    Friend WithEvents tmrAutoRun As Timer
    Friend WithEvents lblPrimary As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents lblSecondary As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents tmrTimer As Timer
    Friend WithEvents Label1 As Label
    Friend WithEvents lblTotalTime As Label
    Friend WithEvents btnProcessPayments As Button
    Friend WithEvents TabPage3 As TabPage
    Friend WithEvents chkTransfers As CheckBox
    Friend WithEvents lblVersion As Label
    Friend WithEvents btnAutoRegister As Button
    Friend WithEvents btnFixClients As Button
    Friend WithEvents btnProcessTransfers As Button
End Class
