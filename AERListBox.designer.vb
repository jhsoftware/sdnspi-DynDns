<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class AERListBox
  Inherits System.Windows.Forms.UserControl

  'UserControl overrides dispose to clean up the component list.
  <System.Diagnostics.DebuggerNonUserCode()>
  Protected Overrides Sub Dispose(disposing As Boolean)
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
    Me.btnAdd = New System.Windows.Forms.Button
    Me.btnEdit = New System.Windows.Forms.Button
    Me.btnRemove = New System.Windows.Forms.Button
    Me.ListBox1 = New System.Windows.Forms.ListBox
    Me.SuspendLayout()
    '
    'btnAdd
    '
    Me.btnAdd.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.btnAdd.Location = New System.Drawing.Point(268, 0)
    Me.btnAdd.Name = "btnAdd"
    Me.btnAdd.Size = New System.Drawing.Size(65, 21)
    Me.btnAdd.TabIndex = 1
    Me.btnAdd.Text = "Add..."
    Me.btnAdd.UseVisualStyleBackColor = True
    '
    'btnEdit
    '
    Me.btnEdit.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.btnEdit.Enabled = False
    Me.btnEdit.Location = New System.Drawing.Point(268, 27)
    Me.btnEdit.Name = "btnEdit"
    Me.btnEdit.Size = New System.Drawing.Size(65, 21)
    Me.btnEdit.TabIndex = 2
    Me.btnEdit.Text = "Edit..."
    Me.btnEdit.UseVisualStyleBackColor = True
    '
    'btnRemove
    '
    Me.btnRemove.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.btnRemove.Enabled = False
    Me.btnRemove.Location = New System.Drawing.Point(268, 54)
    Me.btnRemove.Name = "btnRemove"
    Me.btnRemove.Size = New System.Drawing.Size(65, 21)
    Me.btnRemove.TabIndex = 3
    Me.btnRemove.Text = "Remove"
    Me.btnRemove.UseVisualStyleBackColor = True
    '
    'ListBox1
    '
    Me.ListBox1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.ListBox1.FormattingEnabled = True
    Me.ListBox1.IntegralHeight = False
    Me.ListBox1.Location = New System.Drawing.Point(0, 0)
    Me.ListBox1.Name = "ListBox1"
    Me.ListBox1.Size = New System.Drawing.Size(262, 249)
    Me.ListBox1.TabIndex = 0
    '
    'AERListBox
    '
    Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
    Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
    Me.Controls.Add(Me.ListBox1)
    Me.Controls.Add(Me.btnRemove)
    Me.Controls.Add(Me.btnEdit)
    Me.Controls.Add(Me.btnAdd)
    Me.Name = "AERListBox"
    Me.Size = New System.Drawing.Size(333, 249)
    Me.ResumeLayout(False)

  End Sub
  Protected WithEvents btnAdd As System.Windows.Forms.Button
  Protected WithEvents btnEdit As System.Windows.Forms.Button
  Protected WithEvents btnRemove As System.Windows.Forms.Button
  Protected WithEvents ListBox1 As System.Windows.Forms.ListBox

End Class
