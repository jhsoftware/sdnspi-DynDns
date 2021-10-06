<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ViewUI
    Inherits JHSoftware.SimpleDNS.Plugin.ViewUI

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
    Me.components = New System.ComponentModel.Container
    Me.lst = New System.Windows.Forms.ListView
    Me.colID = New System.Windows.Forms.ColumnHeader
    Me.colIP = New System.Windows.Forms.ColumnHeader
    Me.colStatus = New System.Windows.Forms.ColumnHeader
    Me.colLastUp = New System.Windows.Forms.ColumnHeader
    Me.mnuItem = New System.Windows.Forms.ContextMenuStrip(Me.components)
    Me.mnuOffline = New System.Windows.Forms.ToolStripMenuItem
    Me.mnuItem.SuspendLayout()
    Me.SuspendLayout()
    '
    'lst
    '
    Me.lst.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.colID, Me.colIP, Me.colStatus, Me.colLastUp})
    Me.lst.ContextMenuStrip = Me.mnuItem
    Me.lst.Dock = System.Windows.Forms.DockStyle.Fill
    Me.lst.FullRowSelect = True
    Me.lst.Location = New System.Drawing.Point(0, 0)
    Me.lst.MultiSelect = False
    Me.lst.Name = "lst"
    Me.lst.Size = New System.Drawing.Size(479, 263)
    Me.lst.TabIndex = 0
    Me.lst.UseCompatibleStateImageBehavior = False
    Me.lst.View = System.Windows.Forms.View.Details
    '
    'colID
    '
    Me.colID.Text = "User ID"
    Me.colID.Width = 94
    '
    'colIP
    '
    Me.colIP.Text = "IP address"
    Me.colIP.Width = 103
    '
    'colStatus
    '
    Me.colStatus.Text = "Status"
    Me.colStatus.Width = 76
    '
    'colLastUp
    '
    Me.colLastUp.Text = "Last update"
    Me.colLastUp.Width = 176
    '
    'mnuItem
    '
    Me.mnuItem.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuOffline})
    Me.mnuItem.Name = "ContextMenuStrip1"
    Me.mnuItem.RenderMode = System.Windows.Forms.ToolStripRenderMode.System
    Me.mnuItem.ShowImageMargin = False
    Me.mnuItem.Size = New System.Drawing.Size(123, 26)
    '
    'mnuOffline
    '
    Me.mnuOffline.Name = "mnuOffline"
    Me.mnuOffline.Size = New System.Drawing.Size(122, 22)
    Me.mnuOffline.Text = "Set to offline"
    '
    'ViewUI
    '
    Me.Controls.Add(Me.lst)
    Me.Name = "ViewUI"
    Me.Size = New System.Drawing.Size(479, 263)
    Me.mnuItem.ResumeLayout(False)
    Me.ResumeLayout(False)

  End Sub
  Friend WithEvents lst As System.Windows.Forms.ListView
  Friend WithEvents colID As System.Windows.Forms.ColumnHeader
  Friend WithEvents colIP As System.Windows.Forms.ColumnHeader
  Friend WithEvents colStatus As System.Windows.Forms.ColumnHeader
  Friend WithEvents colLastUp As System.Windows.Forms.ColumnHeader
  Friend WithEvents mnuItem As System.Windows.Forms.ContextMenuStrip
  Friend WithEvents mnuOffline As System.Windows.Forms.ToolStripMenuItem

End Class
