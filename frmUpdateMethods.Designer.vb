<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmUpdateMethods
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
    Me.btnOK = New System.Windows.Forms.Button
    Me.btnCancel = New System.Windows.Forms.Button
    Me.numPort = New System.Windows.Forms.NumericUpDown
    Me.chkGnudipN = New System.Windows.Forms.CheckBox
    Me.chkGnudipH = New System.Windows.Forms.CheckBox
    Me.chkBasic = New System.Windows.Forms.CheckBox
    Me.chkURL = New System.Windows.Forms.CheckBox
    Me.txtBaseUrl = New System.Windows.Forms.TextBox
    Me.GroupBox1 = New System.Windows.Forms.GroupBox
    Me.Label1 = New System.Windows.Forms.Label
    Me.btnShowFull = New System.Windows.Forms.Button
    Me.chkRDetect = New System.Windows.Forms.CheckBox
    Me.chkTSIG = New System.Windows.Forms.CheckBox
    CType(Me.numPort, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.GroupBox1.SuspendLayout()
    Me.SuspendLayout()
    '
    'btnOK
    '
    Me.btnOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.btnOK.Location = New System.Drawing.Point(165, 310)
    Me.btnOK.Name = "btnOK"
    Me.btnOK.Size = New System.Drawing.Size(75, 23)
    Me.btnOK.TabIndex = 1
    Me.btnOK.Text = "OK"
    '
    'btnCancel
    '
    Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
    Me.btnCancel.Location = New System.Drawing.Point(246, 310)
    Me.btnCancel.Name = "btnCancel"
    Me.btnCancel.Size = New System.Drawing.Size(75, 23)
    Me.btnCancel.TabIndex = 2
    Me.btnCancel.Text = "Cancel"
    '
    'numPort
    '
    Me.numPort.Enabled = False
    Me.numPort.Location = New System.Drawing.Point(213, 48)
    Me.numPort.Margin = New System.Windows.Forms.Padding(3, 0, 3, 3)
    Me.numPort.Maximum = New Decimal(New Integer() {65535, 0, 0, 0})
    Me.numPort.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
    Me.numPort.Name = "numPort"
    Me.numPort.Size = New System.Drawing.Size(63, 20)
    Me.numPort.TabIndex = 2
    Me.numPort.Value = New Decimal(New Integer() {3495, 0, 0, 0})
    '
    'chkGnudipN
    '
    Me.chkGnudipN.AutoSize = True
    Me.chkGnudipN.Location = New System.Drawing.Point(18, 49)
    Me.chkGnudipN.Name = "chkGnudipN"
    Me.chkGnudipN.Size = New System.Drawing.Size(189, 17)
    Me.chkGnudipN.TabIndex = 1
    Me.chkGnudipN.Text = "GnuDIP direct TCP protocol - Port:"
    Me.chkGnudipN.UseVisualStyleBackColor = True
    '
    'chkGnudipH
    '
    Me.chkGnudipH.AutoSize = True
    Me.chkGnudipH.Location = New System.Drawing.Point(18, 118)
    Me.chkGnudipH.Name = "chkGnudipH"
    Me.chkGnudipH.Size = New System.Drawing.Size(172, 17)
    Me.chkGnudipH.TabIndex = 5
    Me.chkGnudipH.Text = "HTTP - GnuDIP authentication"
    Me.chkGnudipH.UseVisualStyleBackColor = True
    '
    'chkBasic
    '
    Me.chkBasic.AutoSize = True
    Me.chkBasic.Location = New System.Drawing.Point(18, 95)
    Me.chkBasic.Name = "chkBasic"
    Me.chkBasic.Size = New System.Drawing.Size(192, 17)
    Me.chkBasic.TabIndex = 4
    Me.chkBasic.Text = "HTTP - Basic HTTP authentication"
    Me.chkBasic.UseVisualStyleBackColor = True
    '
    'chkURL
    '
    Me.chkURL.AutoSize = True
    Me.chkURL.Location = New System.Drawing.Point(18, 72)
    Me.chkURL.Name = "chkURL"
    Me.chkURL.Size = New System.Drawing.Size(156, 17)
    Me.chkURL.TabIndex = 3
    Me.chkURL.Text = "HTTP - URL authentication"
    Me.chkURL.UseVisualStyleBackColor = True
    '
    'txtBaseUrl
    '
    Me.txtBaseUrl.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtBaseUrl.Enabled = False
    Me.txtBaseUrl.Location = New System.Drawing.Point(18, 220)
    Me.txtBaseUrl.Name = "txtBaseUrl"
    Me.txtBaseUrl.Size = New System.Drawing.Size(273, 20)
    Me.txtBaseUrl.TabIndex = 8
    Me.txtBaseUrl.Text = "http://"
    '
    'GroupBox1
    '
    Me.GroupBox1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.GroupBox1.Controls.Add(Me.Label1)
    Me.GroupBox1.Controls.Add(Me.txtBaseUrl)
    Me.GroupBox1.Controls.Add(Me.btnShowFull)
    Me.GroupBox1.Controls.Add(Me.chkRDetect)
    Me.GroupBox1.Controls.Add(Me.chkTSIG)
    Me.GroupBox1.Controls.Add(Me.numPort)
    Me.GroupBox1.Controls.Add(Me.chkURL)
    Me.GroupBox1.Controls.Add(Me.chkBasic)
    Me.GroupBox1.Controls.Add(Me.chkGnudipN)
    Me.GroupBox1.Controls.Add(Me.chkGnudipH)
    Me.GroupBox1.Location = New System.Drawing.Point(12, 12)
    Me.GroupBox1.Name = "GroupBox1"
    Me.GroupBox1.Padding = New System.Windows.Forms.Padding(15, 10, 15, 15)
    Me.GroupBox1.Size = New System.Drawing.Size(309, 292)
    Me.GroupBox1.TabIndex = 0
    Me.GroupBox1.TabStop = False
    Me.GroupBox1.Text = "Update methods:"
    '
    'Label1
    '
    Me.Label1.AutoSize = True
    Me.Label1.Location = New System.Drawing.Point(18, 204)
    Me.Label1.Margin = New System.Windows.Forms.Padding(3, 15, 3, 0)
    Me.Label1.Name = "Label1"
    Me.Label1.Size = New System.Drawing.Size(148, 13)
    Me.Label1.TabIndex = 7
    Me.Label1.Text = "Base URL for HTTP services:"
    '
    'btnShowFull
    '
    Me.btnShowFull.Enabled = False
    Me.btnShowFull.Location = New System.Drawing.Point(18, 246)
    Me.btnShowFull.Name = "btnShowFull"
    Me.btnShowFull.Size = New System.Drawing.Size(117, 23)
    Me.btnShowFull.TabIndex = 9
    Me.btnShowFull.Text = "Show full URLs..."
    Me.btnShowFull.UseVisualStyleBackColor = True
    '
    'chkRDetect
    '
    Me.chkRDetect.AutoSize = True
    Me.chkRDetect.CheckAlign = System.Drawing.ContentAlignment.TopLeft
    Me.chkRDetect.Location = New System.Drawing.Point(18, 156)
    Me.chkRDetect.Margin = New System.Windows.Forms.Padding(3, 18, 3, 3)
    Me.chkRDetect.Name = "chkRDetect"
    Me.chkRDetect.Size = New System.Drawing.Size(246, 30)
    Me.chkRDetect.TabIndex = 6
    Me.chkRDetect.Text = "Enable remote IP detection service" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "(simple web service returning client IP addre" & _
        "ss)"
    Me.chkRDetect.UseVisualStyleBackColor = True
    '
    'chkTSIG
    '
    Me.chkTSIG.AutoSize = True
    Me.chkTSIG.Checked = True
    Me.chkTSIG.CheckState = System.Windows.Forms.CheckState.Checked
    Me.chkTSIG.Location = New System.Drawing.Point(18, 26)
    Me.chkTSIG.Name = "chkTSIG"
    Me.chkTSIG.Size = New System.Drawing.Size(200, 17)
    Me.chkTSIG.TabIndex = 0
    Me.chkTSIG.Text = "DNS - TSIG signed dynamic updates"
    Me.chkTSIG.UseVisualStyleBackColor = True
    '
    'frmUpdateMethods
    '
    Me.AcceptButton = Me.btnOK
    Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
    Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
    Me.CancelButton = Me.btnCancel
    Me.ClientSize = New System.Drawing.Size(333, 345)
    Me.Controls.Add(Me.GroupBox1)
    Me.Controls.Add(Me.btnCancel)
    Me.Controls.Add(Me.btnOK)
    Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
    Me.MaximizeBox = False
    Me.MinimizeBox = False
    Me.Name = "frmUpdateMethods"
    Me.ShowInTaskbar = False
    Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
    Me.Text = "DynDNS client update methods"
    CType(Me.numPort, System.ComponentModel.ISupportInitialize).EndInit()
    Me.GroupBox1.ResumeLayout(False)
    Me.GroupBox1.PerformLayout()
    Me.ResumeLayout(False)

  End Sub
  Friend WithEvents btnOK As System.Windows.Forms.Button
  Friend WithEvents btnCancel As System.Windows.Forms.Button
  Friend WithEvents numPort As System.Windows.Forms.NumericUpDown
  Friend WithEvents chkGnudipN As System.Windows.Forms.CheckBox
  Friend WithEvents chkGnudipH As System.Windows.Forms.CheckBox
  Friend WithEvents chkBasic As System.Windows.Forms.CheckBox
  Friend WithEvents chkURL As System.Windows.Forms.CheckBox
  Friend WithEvents txtBaseUrl As System.Windows.Forms.TextBox
  Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
  Friend WithEvents btnShowFull As System.Windows.Forms.Button
  Friend WithEvents chkTSIG As System.Windows.Forms.CheckBox
  Friend WithEvents chkRDetect As System.Windows.Forms.CheckBox
  Friend WithEvents Label1 As System.Windows.Forms.Label

End Class
