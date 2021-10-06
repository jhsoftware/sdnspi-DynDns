Imports System.Windows.Forms

Public Class frmUser

  Friend FromList As AERListBox

  Private Sub OK_Button_Click(sender As System.Object, e As System.EventArgs) Handles btnOK.Click
    Dim x = txtID.Text.Trim
    If x.Length = 0 Then
      MessageBox.Show("User ID is required", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
      txtID.Focus()
      Exit Sub
    End If
    For i = 0 To x.Length - 1
      If x(i) <> "-"c AndAlso
         (x(i) < "a"c Or x(i) > "z"c) AndAlso
         (x(i) < "0"c Or x(i) > "9"c) Then
        MessageBox.Show("Invalid characters in user ID (only: a-z, 0-9, -)", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
        txtID.Focus()
        Exit Sub
      End If
    Next
    x = txtPassword.Text.Trim
    If x.Length = 0 Then
      MessageBox.Show("Password is required", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
      txtPassword.Focus()
      Exit Sub
    End If
    If Not chkAutoHash.Checked Then
      Dim ba As Byte()
      Try
        ba = Convert.FromBase64String(txtTSIG.Text.Trim)
      Catch ex As Exception
        MessageBox.Show("Invalid TSIG key value", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
        txtTSIG.Focus()
        Exit Sub
      End Try
      If ba.Length < 16 Then
        MessageBox.Show("TSIG key value too short (min. 16 bytes)", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
        txtTSIG.Focus()
        Exit Sub
      End If
    End If

    If txtIP.Text.Trim.Length > 0 Then
      Dim ip As SdnsIPv4 = Nothing
      If Not SdnsIPv4.TryParse(txtIP.Text.Trim, ip) Then
        MessageBox.Show("Invalid offline IP address", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
        txtIP.Focus()
        Exit Sub
      End If
    End If

    If Not FromList.CompleteEditItem(SaveData, AddressOf ItemsEqual) Then Exit Sub

    Me.DialogResult = System.Windows.Forms.DialogResult.OK
    Me.Close()
  End Sub

  Private Function ItemsEqual(item1 As Object, item2 As Object) As Boolean
    Dim u1 = DirectCast(item1, MyConfig.User)
    Dim u2 = DirectCast(item2, MyConfig.User)
    If u1.ID = u2.ID Then
      MessageBox.Show("User ID is already in the list", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
      txtID.Focus()
      Return True
    End If
    For Each hn1 In u1.HostNames
      For Each hn2 In u2.HostNames
        If hn1 = hn2 Then
          MessageBox.Show("The host name '" & hn1.ToString & "' already exists for user '" & u2.ID.ToString & "'", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
          Return True
        End If
      Next
    Next
    Return False
  End Function

  Private Sub Cancel_Button_Click(sender As System.Object, e As System.EventArgs) Handles btnCancel.Click
    Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
    Me.Close()
  End Sub

  Private Sub txtPassword_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtPassword.TextChanged
    If Not chkAutoHash.Checked Then Exit Sub
    Dim x = txtPassword.Text.Trim
    If x.Length = 0 Then txtTSIG.Text = "" : Exit Sub
    Dim md5 As New System.Security.Cryptography.MD5CryptoServiceProvider
    txtTSIG.Text = Convert.ToBase64String(md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(x)))
  End Sub

  Private Sub chkAutoHash_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkAutoHash.CheckedChanged
    txtTSIG.ReadOnly = chkAutoHash.Checked
    txtPassword_TextChanged(Nothing, Nothing)
  End Sub

  Private Function SaveData() As MyConfig.User
    Dim rv = New MyConfig.User
    rv.ID = DomName.Parse(txtID.Text.Trim)
    rv.Password = txtPassword.Text.Trim
    rv.TSIGAutoHash = chkAutoHash.Checked
    If Not chkAutoHash.Checked Then
      rv.TSIGKeyValue = Convert.FromBase64String(txtTSIG.Text.Trim)
    End If
    rv.OffLineIP = If(txtIP.Text.Trim.Length > 0, SdnsIPv4.Parse(txtIP.Text.Trim), Nothing)
    For Each hn As DomName In lstHostNames.Items
      rv.HostNames.Add(hn)
    Next
    rv.Notes = txtNotes.Text.Trim
    rv.Disabled = chkDisable.Checked
    Return rv
  End Function

  Friend Sub LoadData(user As MyConfig.User)
    txtID.Text = user.ID.ToString
    txtID.Enabled = False
    chkAutoHash.Checked = user.TSIGAutoHash
    txtPassword.Text = user.Password
    If Not user.TSIGAutoHash Then txtTSIG.Text = Convert.ToBase64String(user.TSIGKeyValue)
    txtIP.Text = If(user.OffLineIP IsNot Nothing, user.OffLineIP.ToString, "")
    For Each hn In user.HostNames
      lstHostNames.Items.Add(hn)
    Next
    txtNotes.Text = user.Notes
    chkDisable.Checked = user.Disabled
  End Sub

  Private Sub lstHostNames_EditItem(curItem As Object) Handles lstHostNames.EditItem
    Dim frm = New frmHostName
    frm.FromList = lstHostNames
    If curItem IsNot Nothing Then frm.txtHostName.Text = DirectCast(curItem, DomName).ToString
    frm.ShowDialog()
  End Sub


End Class
