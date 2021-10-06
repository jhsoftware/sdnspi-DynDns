Imports System.Windows.Forms

Public Class frmHostName

  Friend FromList As AERListBox

  Private Sub OK_Button_Click(sender As System.Object, e As System.EventArgs) Handles btnOK.Click
    Dim x = txtHostName.Text.Trim
    If x.Length = 0 Then Exit Sub
    Dim hn As DomName = Nothing
    If Not DomName.TryParse(x, hn) Then
      MessageBox.Show("Invalid host name", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
      txtHostName.Focus()
      Exit Sub
    End If
    If hn.SegmentCount < 2 Then
      MessageBox.Show("Host name must be at least two segments", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
      txtHostName.Focus()
      Exit Sub
    End If
    If x(0) = "*"c AndAlso x(1) <> "."c Then
      MessageBox.Show("Wildcard character (*) must be immediately followed by a dot (.)", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
      txtHostName.Focus()
      Exit Sub
    End If
    If x.IndexOf("*"c, 1) > 0 Then
      MessageBox.Show("Wildcard character (*) is only allowed as first character", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
      txtHostName.Focus()
      Exit Sub
    End If

    If Not FromList.CompleteEditItem(hn, AddressOf ItemsEqual) Then Exit Sub

    Me.DialogResult = System.Windows.Forms.DialogResult.OK
    Me.Close()
  End Sub

  Private Function ItemsEqual(item1 As Object, item2 As Object) As Boolean
    If DirectCast(item1, DomName) =
       DirectCast(item2, DomName) Then
      MessageBox.Show("Host name is already in the list", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
      txtHostName.Focus()
      Return True
    End If
    Return False
  End Function

  Private Sub Cancel_Button_Click(sender As System.Object, e As System.EventArgs) Handles btnCancel.Click
    Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
    Me.Close()
  End Sub

End Class
