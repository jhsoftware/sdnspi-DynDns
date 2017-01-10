<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Q1OptionsUI
  Inherits JHSoftware.SimpleDNS.Plugin.OptionsUI

  'Form overrides dispose to clean up the component list.
  <System.Diagnostics.DebuggerNonUserCode()> _
  Protected Overrides Sub Dispose(ByVal disposing As Boolean)
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
    Me.txtContains = New System.Windows.Forms.TextBox
    Me.SuspendLayout()
    '
    'Label1
    '
    Me.Label1.AutoSize = True
    Me.Label1.Location = New System.Drawing.Point(-3, 0)
    Me.Label1.Name = "Label1"
    Me.Label1.Size = New System.Drawing.Size(272, 13)
    Me.Label1.TabIndex = 0
    Me.Label1.Text = "Notes of DynDNS user, who sent DNS request, contain:"
    '
    'txtContains
    '
    Me.txtContains.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtContains.Location = New System.Drawing.Point(0, 16)
    Me.txtContains.Name = "txtContains"
    Me.txtContains.Size = New System.Drawing.Size(284, 20)
    Me.txtContains.TabIndex = 1
    '
    'Q1OptionsUI
    '
    Me.Controls.Add(Me.txtContains)
    Me.Controls.Add(Me.Label1)
    Me.Name = "Q1OptionsUI"
    Me.Size = New System.Drawing.Size(284, 47)
    Me.ResumeLayout(False)
    Me.PerformLayout()

  End Sub
  Friend WithEvents Label1 As System.Windows.Forms.Label
  Friend WithEvents txtContains As System.Windows.Forms.TextBox

End Class
