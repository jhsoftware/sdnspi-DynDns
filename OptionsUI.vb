Public Class OptionsUI

  WithEvents ipPublic As Control

  Dim cfg As New MyConfig

  Private Sub btnUpdMet_Click(sender As System.Object, e As System.EventArgs) Handles btnUpdMet.Click
    Dim frm = New frmUpdateMethods
    frm.LoadConfig(cfg)
    If frm.ShowDialog() <> DialogResult.OK Then Exit Sub
    frm.SaveToConfig(cfg)
  End Sub

  Private Sub lstUsers_EditItem(curItem As Object) Handles lstUsers.EditItem
    Dim frm = New frmUser
    frm.FromList = lstUsers
    If curItem IsNot Nothing Then frm.LoadData(DirectCast(curItem, MyConfig.User))
    frm.ShowDialog()
  End Sub

  Public Overrides Sub LoadData(config As String)
    If config Is Nothing Then Exit Sub 'new instance
    cfg = MyConfig.LoadFromXML(config)
    txtSuffix.Text = cfg.Suffix.ToString
    For Each user In cfg.Users.Values
      lstUsers.Items.Add(user)
    Next
    chkUpdate.Checked = cfg.UpdateZones
  End Sub

  Public Overrides Function SaveData() As String
    cfg.Suffix = DomName.Parse(txtSuffix.Text.Trim)
    cfg.SuffixSegCt = cfg.Suffix.SegmentCount
    cfg.Users = New Dictionary(Of DomName, MyConfig.User)
    For Each user As MyConfig.User In lstUsers.Items
      cfg.Users.Add(user.ID, user)
    Next
    cfg.UpdateZones = chkUpdate.Checked
    Return cfg.SaveToXML.OuterXml
  End Function

  Public Overrides Function ValidateData() As Boolean
    Dim suffix As DomName = Nothing
    If txtSuffix.Text.Trim.Length = 0 Then
      MessageBox.Show("Host name suffix is required", "DynDNS", MessageBoxButtons.OK, MessageBoxIcon.Error)
      txtSuffix.Focus()
      Return False
    End If
    If Not DomName.TryParse(txtSuffix.Text.Trim, suffix) Then
      MessageBox.Show("Invalid host name suffix", "DynDNS", MessageBoxButtons.OK, MessageBoxIcon.Error)
      txtSuffix.Focus()
      Return False
    End If
    If suffix.SegmentCount < 2 Then
      MessageBox.Show("Host name suffix must be at least 2 segments", "DynDNS", MessageBoxButtons.OK, MessageBoxIcon.Error)
      txtSuffix.Focus()
      Return False
    End If
    If lstUsers.Items.Count = 0 Then
      MessageBox.Show("At least one user is required", "DynDNS", MessageBoxButtons.OK, MessageBoxIcon.Error)
      txtSuffix.Focus()
      Return False
    End If
    Dim UserHN As DomName
    For Each user As MyConfig.User In lstUsers.Items
      UserHN = user.ID & suffix
      For Each user2 As MyConfig.User In lstUsers.Items
        For Each hn In user2.HostNames
          If UserHN = hn Then
            MessageBox.Show("The additonal host name '" & hn.ToString & "' defined for user '" & user2.ID.ToString & "' conflicts with the default host name for user ID '" & user.ID.ToString & "'", "DynDNS", MessageBoxButtons.OK, MessageBoxIcon.Error)
            txtSuffix.Focus()
            Return False
          End If
        Next
      Next
    Next

    Return True
  End Function

End Class
