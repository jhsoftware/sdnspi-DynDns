Imports System.ComponentModel

Public Class AERListBox
  Private Adding As Boolean

  Public Event EditItem(ByVal curItem As Object)

  Public Delegate Function ItemsEqualDG(ByVal item1 As Object, ByVal item2 As Object) As Boolean

  Public Function CompleteEditItem(ByVal newItem As Object, ByVal itemsEqual As ItemsEqualDG) As Boolean
    For i = 0 To ListBox1.Items.Count - 1
      If Not Adding AndAlso ListBox1.SelectedIndex = i Then Continue For
      If itemsEqual(newItem, ListBox1.Items(i)) Then Return False
    Next
    If Adding Then
      ListBox1.SelectedIndex = ListBox1.Items.Add(newItem)
    Else
      If Sorted Then
        ListBox1.Items(ListBox1.SelectedIndex) = newItem
      Else
        ListBox1.Items.RemoveAt(ListBox1.SelectedIndex)
        ListBox1.SelectedIndex = ListBox1.Items.Add(newItem)
      End If
    End If
    Return True
  End Function

  <DefaultValue(False)> _
  Public Property Sorted() As Boolean
    Get
      Return ListBox1.Sorted
    End Get
    Set(ByVal value As Boolean)
      ListBox1.Sorted = value
    End Set
  End Property

  Function Items() As System.Windows.Forms.ListBox.ObjectCollection
    Return ListBox1.Items
  End Function

  Private Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
    Adding = True
    RaiseEvent EditItem(Nothing)
  End Sub

  Private Sub btnEdit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEdit.Click, ListBox1.DoubleClick
    If ListBox1.SelectedIndex < 0 Then Exit Sub
    Adding = False
    RaiseEvent EditItem(ListBox1.SelectedItem)
  End Sub

  Private Sub btnRemove_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRemove.Click
    If ListBox1.SelectedIndex < 0 Then Exit Sub
    ListBox1.Items.RemoveAt(ListBox1.SelectedIndex)
  End Sub

  Private Sub ListBox1_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles ListBox1.KeyUp
    If e.KeyData = Keys.Delete Then btnRemove_Click(Nothing, Nothing)
  End Sub

  Private Sub ListBox1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ListBox1.SelectedIndexChanged
    btnEdit.Enabled = (ListBox1.SelectedIndex >= 0)
    btnRemove.Enabled = (ListBox1.SelectedIndex >= 0)
  End Sub

End Class
