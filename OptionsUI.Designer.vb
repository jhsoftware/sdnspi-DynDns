<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class OptionsUI
    Inherits JHSoftware.SimpleDNS.Plugin.OptionsUI

  'Form overrides dispose to clean up the component list.
  <System.Diagnostics.DebuggerNonUserCode()>
  Protected Overrides Sub Dispose(disposing As Boolean)
    If disposing AndAlso components IsNot Nothing Then
      components.Dispose()
    End If
    MyBase.Dispose(disposing)
  End Sub

  'Required by the Windows Form Designer
  Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
    Me.Label1 = New System.Windows.Forms.Label
    Me.txtSuffix = New System.Windows.Forms.TextBox
    Me.chkUpdate = New System.Windows.Forms.CheckBox
    Me.Label3 = New System.Windows.Forms.Label
    Me.btnUpdMet = New System.Windows.Forms.Button
    Me.lstUsers = New AERListBox
    Me.SuspendLayout()
    '
    'Label1
    '
    Me.Label1.AutoSize = True
    Me.Label1.Location = New System.Drawing.Point(-3, 0)
    Me.Label1.Margin = New System.Windows.Forms.Padding(3, 10, 3, 0)
    Me.Label1.Name = "Label1"
    Me.Label1.Size = New System.Drawing.Size(308, 13)
    Me.Label1.TabIndex = 0
    Me.Label1.Text = "Host name suffix (full host name = <user-id>.<host name suffix>):"
    '
    'txtSuffix
    '
    Me.txtSuffix.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtSuffix.Location = New System.Drawing.Point(0, 16)
    Me.txtSuffix.Name = "txtSuffix"
    Me.txtSuffix.Size = New System.Drawing.Size(328, 20)
    Me.txtSuffix.TabIndex = 1
    '
    'chkUpdate
    '
    Me.chkUpdate.AutoSize = True
    Me.chkUpdate.Location = New System.Drawing.Point(0, 202)
    Me.chkUpdate.Margin = New System.Windows.Forms.Padding(3, 13, 3, 3)
    Me.chkUpdate.Name = "chkUpdate"
    Me.chkUpdate.Size = New System.Drawing.Size(189, 17)
    Me.chkUpdate.TabIndex = 5
    Me.chkUpdate.Text = "Update host records in local zones"
    Me.chkUpdate.UseVisualStyleBackColor = True
    '
    'Label3
    '
    Me.Label3.AutoSize = True
    Me.Label3.Location = New System.Drawing.Point(-3, 49)
    Me.Label3.Margin = New System.Windows.Forms.Padding(3, 10, 3, 0)
    Me.Label3.Name = "Label3"
    Me.Label3.Size = New System.Drawing.Size(37, 13)
    Me.Label3.TabIndex = 2
    Me.Label3.Text = "Users:"
    '
    'btnUpdMet
    '
    Me.btnUpdMet.Location = New System.Drawing.Point(0, 163)
    Me.btnUpdMet.Margin = New System.Windows.Forms.Padding(3, 13, 3, 3)
    Me.btnUpdMet.Name = "btnUpdMet"
    Me.btnUpdMet.Size = New System.Drawing.Size(117, 23)
    Me.btnUpdMet.TabIndex = 4
    Me.btnUpdMet.Text = "Update methods..."
    Me.btnUpdMet.UseVisualStyleBackColor = True
    '
    'lstUsers
    '
    Me.lstUsers.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.lstUsers.Location = New System.Drawing.Point(0, 65)
    Me.lstUsers.Name = "lstUsers"
    Me.lstUsers.Size = New System.Drawing.Size(328, 82)
    Me.lstUsers.Sorted = True
    Me.lstUsers.TabIndex = 3
    '
    'OptionsUI
    '
    Me.Controls.Add(Me.btnUpdMet)
    Me.Controls.Add(Me.Label3)
    Me.Controls.Add(Me.lstUsers)
    Me.Controls.Add(Me.chkUpdate)
    Me.Controls.Add(Me.txtSuffix)
    Me.Controls.Add(Me.Label1)
    Me.Name = "OptionsUI"
    Me.Size = New System.Drawing.Size(328, 227)
    Me.ResumeLayout(False)
    Me.PerformLayout()

  End Sub
  Friend WithEvents Label1 As System.Windows.Forms.Label
  Friend WithEvents txtSuffix As System.Windows.Forms.TextBox
  Friend WithEvents chkUpdate As System.Windows.Forms.CheckBox
  Friend WithEvents lstUsers As AERListBox
  Friend WithEvents Label3 As System.Windows.Forms.Label
  Friend WithEvents btnUpdMet As System.Windows.Forms.Button

End Class
