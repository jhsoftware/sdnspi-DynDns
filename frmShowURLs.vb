Imports System.Windows.Forms

Public Class frmShowURLs

  Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
    Me.DialogResult = System.Windows.Forms.DialogResult.OK
    Me.Close()
  End Sub

  Private Sub btnCopy1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCopy1.Click
    MyCopy(txtURL1)
  End Sub
  Private Sub btnCopy2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCopy2.Click
    MyCopy(txtURL2)
  End Sub
  Private Sub btnCopy3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCopy3.Click
    MyCopy(txtURL3)
  End Sub
  Private Sub btnCopy4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCopy4.Click
    MyCopy(txtURL4)
  End Sub
  Private Sub btnCopy5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCopy5.Click
    MyCopy(txtURL5)
  End Sub

  Private Sub MyCopy(ByVal txt As TextBox)
    txt.SelectAll()
    txt.Focus()
    My.Computer.Clipboard.SetText(txt.Text)
  End Sub


End Class
