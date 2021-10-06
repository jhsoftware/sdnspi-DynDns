<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmShowURLs
    Inherits System.Windows.Forms.Form

  'Form overrides dispose to clean up the component list.
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
    Me.components = New System.ComponentModel.Container()
    Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmShowURLs))
    Me.btnOK = New System.Windows.Forms.Button()
    Me.Label1 = New System.Windows.Forms.Label()
    Me.Label2 = New System.Windows.Forms.Label()
    Me.Label3 = New System.Windows.Forms.Label()
    Me.txtURL1 = New System.Windows.Forms.TextBox()
    Me.txtURL2 = New System.Windows.Forms.TextBox()
    Me.txtURL3 = New System.Windows.Forms.TextBox()
    Me.btnCopy1 = New System.Windows.Forms.Button()
    Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
    Me.btnCopy3 = New System.Windows.Forms.Button()
    Me.btnCopy2 = New System.Windows.Forms.Button()
    Me.btnCopy5 = New System.Windows.Forms.Button()
    Me.lblParams = New System.Windows.Forms.Label()
    Me.txtURL5 = New System.Windows.Forms.TextBox()
    Me.Label5 = New System.Windows.Forms.Label()
    Me.btnCopy4 = New System.Windows.Forms.Button()
    Me.txtURL4 = New System.Windows.Forms.TextBox()
    Me.Label4 = New System.Windows.Forms.Label()
    Me.SuspendLayout()
    '
    'btnOK
    '
    Me.btnOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.btnOK.Location = New System.Drawing.Point(374, 307)
    Me.btnOK.Name = "btnOK"
    Me.btnOK.Size = New System.Drawing.Size(75, 23)
    Me.btnOK.TabIndex = 16
    Me.btnOK.Text = "OK"
    '
    'Label1
    '
    Me.Label1.AutoSize = True
    Me.Label1.Location = New System.Drawing.Point(12, 9)
    Me.Label1.Name = "Label1"
    Me.Label1.Size = New System.Drawing.Size(189, 13)
    Me.Label1.TabIndex = 0
    Me.Label1.Text = "DynDNS update - URL authentication:"
    '
    'Label2
    '
    Me.Label2.AutoSize = True
    Me.Label2.Location = New System.Drawing.Point(12, 58)
    Me.Label2.Margin = New System.Windows.Forms.Padding(3, 10, 3, 0)
    Me.Label2.Name = "Label2"
    Me.Label2.Size = New System.Drawing.Size(225, 13)
    Me.Label2.TabIndex = 3
    Me.Label2.Text = "DynDNS update - Basic HTTP authentication:"
    '
    'Label3
    '
    Me.Label3.AutoSize = True
    Me.Label3.Location = New System.Drawing.Point(12, 107)
    Me.Label3.Margin = New System.Windows.Forms.Padding(3, 10, 3, 0)
    Me.Label3.Name = "Label3"
    Me.Label3.Size = New System.Drawing.Size(209, 13)
    Me.Label3.TabIndex = 6
    Me.Label3.Text = "DynDNS update - GNUDIP authentication:"
    '
    'txtURL1
    '
    Me.txtURL1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtURL1.BackColor = System.Drawing.SystemColors.Window
    Me.txtURL1.Location = New System.Drawing.Point(12, 25)
    Me.txtURL1.Name = "txtURL1"
    Me.txtURL1.ReadOnly = True
    Me.txtURL1.Size = New System.Drawing.Size(406, 20)
    Me.txtURL1.TabIndex = 1
    '
    'txtURL2
    '
    Me.txtURL2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtURL2.BackColor = System.Drawing.SystemColors.Window
    Me.txtURL2.Location = New System.Drawing.Point(12, 72)
    Me.txtURL2.Name = "txtURL2"
    Me.txtURL2.ReadOnly = True
    Me.txtURL2.Size = New System.Drawing.Size(406, 20)
    Me.txtURL2.TabIndex = 4
    '
    'txtURL3
    '
    Me.txtURL3.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtURL3.BackColor = System.Drawing.SystemColors.Window
    Me.txtURL3.Location = New System.Drawing.Point(12, 123)
    Me.txtURL3.Name = "txtURL3"
    Me.txtURL3.ReadOnly = True
    Me.txtURL3.Size = New System.Drawing.Size(406, 20)
    Me.txtURL3.TabIndex = 7
    '
    'btnCopy1
    '
    Me.btnCopy1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.btnCopy1.Image = Global.My.Resources.Resources.CopyHS
    Me.btnCopy1.Location = New System.Drawing.Point(424, 23)
    Me.btnCopy1.Name = "btnCopy1"
    Me.btnCopy1.Size = New System.Drawing.Size(25, 23)
    Me.btnCopy1.TabIndex = 2
    Me.ToolTip1.SetToolTip(Me.btnCopy1, "Copy URL to clipboard")
    Me.btnCopy1.UseVisualStyleBackColor = True
    '
    'btnCopy3
    '
    Me.btnCopy3.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.btnCopy3.Image = Global.My.Resources.Resources.CopyHS
    Me.btnCopy3.Location = New System.Drawing.Point(424, 121)
    Me.btnCopy3.Name = "btnCopy3"
    Me.btnCopy3.Size = New System.Drawing.Size(25, 23)
    Me.btnCopy3.TabIndex = 8
    Me.ToolTip1.SetToolTip(Me.btnCopy3, "Copy URL to clipboard")
    Me.btnCopy3.UseVisualStyleBackColor = True
    '
    'btnCopy2
    '
    Me.btnCopy2.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.btnCopy2.Image = Global.My.Resources.Resources.CopyHS
    Me.btnCopy2.Location = New System.Drawing.Point(424, 72)
    Me.btnCopy2.Name = "btnCopy2"
    Me.btnCopy2.Size = New System.Drawing.Size(25, 23)
    Me.btnCopy2.TabIndex = 5
    Me.ToolTip1.SetToolTip(Me.btnCopy2, "Copy URL to clipboard")
    Me.btnCopy2.UseVisualStyleBackColor = True
    '
    'btnCopy5
    '
    Me.btnCopy5.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.btnCopy5.Image = Global.My.Resources.Resources.CopyHS
    Me.btnCopy5.Location = New System.Drawing.Point(424, 219)
    Me.btnCopy5.Name = "btnCopy5"
    Me.btnCopy5.Size = New System.Drawing.Size(25, 23)
    Me.btnCopy5.TabIndex = 14
    Me.ToolTip1.SetToolTip(Me.btnCopy5, "Copy URL to clipboard")
    Me.btnCopy5.UseVisualStyleBackColor = True
    '
    'lblParams
    '
    Me.lblParams.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
    Me.lblParams.AutoSize = True
    Me.lblParams.Location = New System.Drawing.Point(12, 265)
    Me.lblParams.Name = "lblParams"
    Me.lblParams.Size = New System.Drawing.Size(316, 65)
    Me.lblParams.TabIndex = 15
    Me.lblParams.Text = resources.GetString("lblParams.Text")
    '
    'txtURL5
    '
    Me.txtURL5.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtURL5.BackColor = System.Drawing.SystemColors.Window
    Me.txtURL5.Location = New System.Drawing.Point(12, 221)
    Me.txtURL5.Name = "txtURL5"
    Me.txtURL5.ReadOnly = True
    Me.txtURL5.Size = New System.Drawing.Size(406, 20)
    Me.txtURL5.TabIndex = 13
    '
    'Label5
    '
    Me.Label5.AutoSize = True
    Me.Label5.Location = New System.Drawing.Point(12, 205)
    Me.Label5.Margin = New System.Windows.Forms.Padding(3, 10, 3, 0)
    Me.Label5.Name = "Label5"
    Me.Label5.Size = New System.Drawing.Size(184, 13)
    Me.Label5.TabIndex = 12
    Me.Label5.Text = "Remote IP address detection service:"
    '
    'btnCopy4
    '
    Me.btnCopy4.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.btnCopy4.Image = Global.My.Resources.Resources.CopyHS
    Me.btnCopy4.Location = New System.Drawing.Point(424, 170)
    Me.btnCopy4.Name = "btnCopy4"
    Me.btnCopy4.Size = New System.Drawing.Size(25, 23)
    Me.btnCopy4.TabIndex = 11
    Me.ToolTip1.SetToolTip(Me.btnCopy4, "Copy URL to clipboard")
    Me.btnCopy4.UseVisualStyleBackColor = True
    '
    'txtURL4
    '
    Me.txtURL4.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtURL4.BackColor = System.Drawing.SystemColors.Window
    Me.txtURL4.Location = New System.Drawing.Point(12, 172)
    Me.txtURL4.Name = "txtURL4"
    Me.txtURL4.ReadOnly = True
    Me.txtURL4.Size = New System.Drawing.Size(406, 20)
    Me.txtURL4.TabIndex = 10
    '
    'Label4
    '
    Me.Label4.AutoSize = True
    Me.Label4.Location = New System.Drawing.Point(12, 156)
    Me.Label4.Margin = New System.Windows.Forms.Padding(3, 10, 3, 0)
    Me.Label4.Name = "Label4"
    Me.Label4.Size = New System.Drawing.Size(228, 13)
    Me.Label4.TabIndex = 9
    Me.Label4.Text = "DynDNS update - HTTP Dyn.com URL format:"
    '
    'frmShowURLs
    '
    Me.AcceptButton = Me.btnOK
    Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
    Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
    Me.ClientSize = New System.Drawing.Size(461, 342)
    Me.Controls.Add(Me.btnCopy4)
    Me.Controls.Add(Me.txtURL4)
    Me.Controls.Add(Me.Label4)
    Me.Controls.Add(Me.btnCopy5)
    Me.Controls.Add(Me.txtURL5)
    Me.Controls.Add(Me.Label5)
    Me.Controls.Add(Me.lblParams)
    Me.Controls.Add(Me.btnCopy2)
    Me.Controls.Add(Me.btnCopy3)
    Me.Controls.Add(Me.btnCopy1)
    Me.Controls.Add(Me.txtURL3)
    Me.Controls.Add(Me.txtURL2)
    Me.Controls.Add(Me.txtURL1)
    Me.Controls.Add(Me.Label3)
    Me.Controls.Add(Me.Label2)
    Me.Controls.Add(Me.Label1)
    Me.Controls.Add(Me.btnOK)
    Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
    Me.MaximizeBox = False
    Me.MinimizeBox = False
    Me.Name = "frmShowURLs"
    Me.ShowInTaskbar = False
    Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
    Me.Text = "HTTP service URLs"
    Me.ResumeLayout(False)
    Me.PerformLayout()

  End Sub
  Friend WithEvents btnOK As System.Windows.Forms.Button
  Friend WithEvents Label1 As System.Windows.Forms.Label
  Friend WithEvents Label2 As System.Windows.Forms.Label
  Friend WithEvents Label3 As System.Windows.Forms.Label
  Friend WithEvents txtURL1 As System.Windows.Forms.TextBox
  Friend WithEvents txtURL2 As System.Windows.Forms.TextBox
  Friend WithEvents txtURL3 As System.Windows.Forms.TextBox
  Friend WithEvents btnCopy1 As System.Windows.Forms.Button
  Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
  Friend WithEvents btnCopy3 As System.Windows.Forms.Button
  Friend WithEvents btnCopy2 As System.Windows.Forms.Button
  Friend WithEvents lblParams As System.Windows.Forms.Label
  Friend WithEvents btnCopy5 As System.Windows.Forms.Button
  Friend WithEvents txtURL5 As System.Windows.Forms.TextBox
  Friend WithEvents Label5 As System.Windows.Forms.Label
  Friend WithEvents btnCopy4 As Button
  Friend WithEvents txtURL4 As TextBox
  Friend WithEvents Label4 As Label
End Class
