Public Class Q1OptionsUI

  Public Overrides Sub LoadData(ByVal config As String)
    If config Is Nothing Then Exit Sub
    txtContains.Text = config
  End Sub

  Public Overrides Function ValidateData() As Boolean
    If txtContains.Text.Trim.Length = 0 Then
      MessageBox.Show("No data entered", "Notes contain", MessageBoxButtons.OK, MessageBoxIcon.Error)
      Return False
    End If
    Return True
  End Function

  Public Overrides Function SaveData() As String
    Return txtContains.Text.Trim
  End Function

End Class
