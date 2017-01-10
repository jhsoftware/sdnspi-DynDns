<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmUser
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
    Me.GroupBox1 = New System.Windows.Forms.GroupBox
    Me.txtDummy = New System.Windows.Forms.TextBox
    Me.Label5 = New System.Windows.Forms.Label
    Me.chkAutoHash = New System.Windows.Forms.CheckBox
    Me.txtTSIG = New System.Windows.Forms.TextBox
    Me.Label3 = New System.Windows.Forms.Label
    Me.lstHostNames = New AERListBox
    Me.Label4 = New System.Windows.Forms.Label
    Me.chkDisable = New System.Windows.Forms.CheckBox
    Me.txtPassword = New System.Windows.Forms.TextBox
    Me.Label2 = New System.Windows.Forms.Label
    Me.txtID = New System.Windows.Forms.TextBox
    Me.Label1 = New System.Windows.Forms.Label
    Me.Label6 = New System.Windows.Forms.Label
    Me.txtNotes = New System.Windows.Forms.TextBox
    Me.GroupBox1.SuspendLayout()
    Me.SuspendLayout()
    '
    'btnOK
    '
    Me.btnOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.btnOK.Location = New System.Drawing.Point(146, 401)
    Me.btnOK.Name = "btnOK"
    Me.btnOK.Size = New System.Drawing.Size(75, 23)
    Me.btnOK.TabIndex = 1
    Me.btnOK.Text = "OK"
    '
    'btnCancel
    '
    Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
    Me.btnCancel.Location = New System.Drawing.Point(227, 401)
    Me.btnCancel.Name = "btnCancel"
    Me.btnCancel.Size = New System.Drawing.Size(75, 23)
    Me.btnCancel.TabIndex = 2
    Me.btnCancel.Text = "Cancel"
    '
    'GroupBox1
    '
    Me.GroupBox1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.GroupBox1.Controls.Add(Me.txtNotes)
    Me.GroupBox1.Controls.Add(Me.Label6)
    Me.GroupBox1.Controls.Add(Me.txtDummy)
    Me.GroupBox1.Controls.Add(Me.Label5)
    Me.GroupBox1.Controls.Add(Me.chkAutoHash)
    Me.GroupBox1.Controls.Add(Me.txtTSIG)
    Me.GroupBox1.Controls.Add(Me.Label3)
    Me.GroupBox1.Controls.Add(Me.lstHostNames)
    Me.GroupBox1.Controls.Add(Me.Label4)
    Me.GroupBox1.Controls.Add(Me.chkDisable)
    Me.GroupBox1.Controls.Add(Me.txtPassword)
    Me.GroupBox1.Controls.Add(Me.Label2)
    Me.GroupBox1.Controls.Add(Me.txtID)
    Me.GroupBox1.Controls.Add(Me.Label1)
    Me.GroupBox1.Location = New System.Drawing.Point(12, 12)
    Me.GroupBox1.Name = "GroupBox1"
    Me.GroupBox1.Padding = New System.Windows.Forms.Padding(15, 10, 15, 15)
    Me.GroupBox1.Size = New System.Drawing.Size(290, 383)
    Me.GroupBox1.TabIndex = 0
    Me.GroupBox1.TabStop = False
    Me.GroupBox1.Text = "DynDNS Service User"
    '
    'txtDummy
    '
    Me.txtDummy.Location = New System.Drawing.Point(91, 154)
    Me.txtDummy.Margin = New System.Windows.Forms.Padding(3, 13, 3, 3)
    Me.txtDummy.Name = "txtDummy"
    Me.txtDummy.Size = New System.Drawing.Size(100, 20)
    Me.txtDummy.TabIndex = 8
    Me.txtDummy.Visible = False
    '
    'Label5
    '
    Me.Label5.AutoSize = True
    Me.Label5.Location = New System.Drawing.Point(18, 157)
    Me.Label5.Margin = New System.Windows.Forms.Padding(3, 10, 3, 0)
    Me.Label5.Name = "Label5"
    Me.Label5.Size = New System.Drawing.Size(53, 26)
    Me.Label5.TabIndex = 7
    Me.Label5.Text = "Offline IP:" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "(optional)"
    '
    'chkAutoHash
    '
    Me.chkAutoHash.AutoSize = True
    Me.chkAutoHash.Checked = True
    Me.chkAutoHash.CheckState = System.Windows.Forms.CheckState.Checked
    Me.chkAutoHash.Location = New System.Drawing.Point(91, 121)
    Me.chkAutoHash.Margin = New System.Windows.Forms.Padding(3, 0, 3, 3)
    Me.chkAutoHash.Name = "chkAutoHash"
    Me.chkAutoHash.Size = New System.Drawing.Size(135, 17)
    Me.chkAutoHash.TabIndex = 6
    Me.chkAutoHash.Text = "MD5 hash of password"
    Me.chkAutoHash.UseVisualStyleBackColor = True
    '
    'txtTSIG
    '
    Me.txtTSIG.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtTSIG.Location = New System.Drawing.Point(91, 98)
    Me.txtTSIG.Margin = New System.Windows.Forms.Padding(3, 13, 3, 3)
    Me.txtTSIG.Name = "txtTSIG"
    Me.txtTSIG.ReadOnly = True
    Me.txtTSIG.Size = New System.Drawing.Size(181, 20)
    Me.txtTSIG.TabIndex = 5
    '
    'Label3
    '
    Me.Label3.AutoSize = True
    Me.Label3.Location = New System.Drawing.Point(18, 101)
    Me.Label3.Margin = New System.Windows.Forms.Padding(3, 10, 3, 0)
    Me.Label3.Name = "Label3"
    Me.Label3.Size = New System.Drawing.Size(67, 26)
    Me.Label3.TabIndex = 4
    Me.Label3.Text = "TSIG secret:" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "(base64)"
    '
    'lstHostNames
    '
    Me.lstHostNames.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.lstHostNames.Location = New System.Drawing.Point(18, 209)
    Me.lstHostNames.Name = "lstHostNames"
    Me.lstHostNames.Size = New System.Drawing.Size(254, 77)
    Me.lstHostNames.Sorted = True
    Me.lstHostNames.TabIndex = 10
    '
    'Label4
    '
    Me.Label4.AutoSize = True
    Me.Label4.Location = New System.Drawing.Point(18, 193)
    Me.Label4.Margin = New System.Windows.Forms.Padding(3, 10, 3, 0)
    Me.Label4.Name = "Label4"
    Me.Label4.Size = New System.Drawing.Size(159, 13)
    Me.Label4.TabIndex = 9
    Me.Label4.Text = "Additional host names (optional):"
    '
    'chkDisable
    '
    Me.chkDisable.AutoSize = True
    Me.chkDisable.Location = New System.Drawing.Point(18, 351)
    Me.chkDisable.Margin = New System.Windows.Forms.Padding(3, 13, 3, 3)
    Me.chkDisable.Name = "chkDisable"
    Me.chkDisable.Size = New System.Drawing.Size(103, 17)
    Me.chkDisable.TabIndex = 13
    Me.chkDisable.Text = "Disable this user"
    Me.chkDisable.UseVisualStyleBackColor = True
    '
    'txtPassword
    '
    Me.txtPassword.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtPassword.Location = New System.Drawing.Point(91, 62)
    Me.txtPassword.Margin = New System.Windows.Forms.Padding(3, 13, 3, 3)
    Me.txtPassword.Name = "txtPassword"
    Me.txtPassword.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
    Me.txtPassword.Size = New System.Drawing.Size(181, 20)
    Me.txtPassword.TabIndex = 3
    Me.txtPassword.UseSystemPasswordChar = True
    '
    'Label2
    '
    Me.Label2.AutoSize = True
    Me.Label2.Location = New System.Drawing.Point(18, 65)
    Me.Label2.Margin = New System.Windows.Forms.Padding(3, 10, 3, 0)
    Me.Label2.Name = "Label2"
    Me.Label2.Size = New System.Drawing.Size(56, 13)
    Me.Label2.TabIndex = 2
    Me.Label2.Text = "Password:"
    '
    'txtID
    '
    Me.txtID.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtID.CharacterCasing = System.Windows.Forms.CharacterCasing.Lower
    Me.txtID.Location = New System.Drawing.Point(91, 26)
    Me.txtID.MaxLength = 63
    Me.txtID.Name = "txtID"
    Me.txtID.Size = New System.Drawing.Size(181, 20)
    Me.txtID.TabIndex = 1
    '
    'Label1
    '
    Me.Label1.AutoSize = True
    Me.Label1.Location = New System.Drawing.Point(18, 29)
    Me.Label1.Name = "Label1"
    Me.Label1.Size = New System.Drawing.Size(46, 13)
    Me.Label1.TabIndex = 0
    Me.Label1.Text = "User-ID:"
    '
    'Label6
    '
    Me.Label6.AutoSize = True
    Me.Label6.Location = New System.Drawing.Point(18, 299)
    Me.Label6.Margin = New System.Windows.Forms.Padding(3, 10, 3, 0)
    Me.Label6.Name = "Label6"
    Me.Label6.Size = New System.Drawing.Size(84, 13)
    Me.Label6.TabIndex = 11
    Me.Label6.Text = "Notes (optional):"
    '
    'txtNotes
    '
    Me.txtNotes.Location = New System.Drawing.Point(18, 315)
    Me.txtNotes.Name = "txtNotes"
    Me.txtNotes.Size = New System.Drawing.Size(254, 20)
    Me.txtNotes.TabIndex = 12
    '
    'frmUser
    '
    Me.AcceptButton = Me.btnOK
    Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
    Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
    Me.CancelButton = Me.btnCancel
    Me.ClientSize = New System.Drawing.Size(314, 436)
    Me.Controls.Add(Me.GroupBox1)
    Me.Controls.Add(Me.btnCancel)
    Me.Controls.Add(Me.btnOK)
    Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
    Me.MaximizeBox = False
    Me.MinimizeBox = False
    Me.Name = "frmUser"
    Me.ShowInTaskbar = False
    Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
    Me.Text = "DynDNS Service User"
    Me.GroupBox1.ResumeLayout(False)
    Me.GroupBox1.PerformLayout()
    Me.ResumeLayout(False)

  End Sub
  Friend WithEvents btnOK As System.Windows.Forms.Button
  Friend WithEvents btnCancel As System.Windows.Forms.Button
  Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
  Friend WithEvents txtPassword As System.Windows.Forms.TextBox
  Friend WithEvents Label2 As System.Windows.Forms.Label
  Friend WithEvents txtID As System.Windows.Forms.TextBox
  Friend WithEvents Label1 As System.Windows.Forms.Label
  Friend WithEvents chkDisable As System.Windows.Forms.CheckBox
  Friend WithEvents lstHostNames As AERListBox
  Friend WithEvents Label4 As System.Windows.Forms.Label
  Friend WithEvents Label3 As System.Windows.Forms.Label
  Friend WithEvents chkAutoHash As System.Windows.Forms.CheckBox
  Friend WithEvents txtTSIG As System.Windows.Forms.TextBox
  Friend WithEvents txtDummy As System.Windows.Forms.TextBox
  Friend WithEvents Label5 As System.Windows.Forms.Label
  Friend WithEvents txtNotes As System.Windows.Forms.TextBox
  Friend WithEvents Label6 As System.Windows.Forms.Label

End Class
