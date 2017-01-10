Public Class ViewUI

  Private Users As Dictionary(Of String, User)

  Public Sub New()
    InitializeComponent()
    lst.ListViewItemSorter = New MySorter
  End Sub

  Private Sub ViewUI_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    DirectCast(lst.ListViewItemSorter, MySorter).SortCols.Add(0)
    SendMsgToService(New Byte() {1})
  End Sub

  Public Overrides Sub LoadLayout(ByVal layout As String)
    If String.IsNullOrEmpty(layout) Then Exit Sub
    Dim la = layout.Split("|"c)
    lst.Columns(0).Width = Integer.Parse(la(0))
    lst.Columns(1).Width = Integer.Parse(la(1))
    lst.Columns(2).Width = Integer.Parse(la(2))
    lst.Columns(3).Width = Integer.Parse(la(3))
    With DirectCast(lst.ListViewItemSorter, MySorter)
      .OrderDesc = (la(4) = "1")
      .SortCols.Clear()
      For i = 5 To la.Length - 1
        .SortCols.Add(Integer.Parse(la(i)))
      Next
    End With
    lst.Sort()
  End Sub

  Public Overrides Function SaveLayout() As String
    Dim rv As String = lst.Columns(0).Width & "|" & _
            lst.Columns(1).Width & "|" & _
            lst.Columns(2).Width & "|" & _
            lst.Columns(3).Width & "|"
    With DirectCast(lst.ListViewItemSorter, MySorter)
      rv &= If(.OrderDesc, "1", "0")
      For i = 0 To .SortCols.Count - 1
        rv &= "|" & .SortCols(i)
      Next
    End With
    Return rv
  End Function

  Public Overrides Sub MsgFromService(ByVal msg() As Byte)
    Invoke(MFSDGDG, msg)
  End Sub
  Private MFSDGDG As MFSDG = New MFSDG(AddressOf MsgFromService2)
  Private Delegate Sub MFSDG(ByVal msg() As Byte)
  Private Sub MsgFromService2(ByVal msg() As Byte)
    Select Case msg(0)
      Case 1 'listing users
        Dim u = ParseUser(msg, 1)
        lst.Items.Add(u.ToListViewItem)
      Case 2 'update user
        Dim u = ParseUser(msg, 1)
        For i = 0 To lst.Items.Count - 1
          If lst.Items(i).Text = u.ID Then lst.Items.RemoveAt(i) : Exit For
        Next
        lst.Items.Add(u.ToListViewItem)
    End Select
  End Sub

  Private Function ParseUser(ByVal buf As Byte(), ByVal index As Integer) As User
    Dim rv As New User
    Dim i = buf(index)
    rv.ID = System.Text.Encoding.ASCII.GetString(buf, index + 1, i)
    Dim p = index + 1 + i
    rv.Status = CType(buf(p), UserStatus)
    p += 1
    rv.LastUpdate = #1/1/1970#.AddSeconds((CUInt(buf(p)) << 24) Or _
                          (CUInt(buf(p + 1)) << 16) Or _
                          (CUInt(buf(p + 2)) << 8) Or _
                          CUInt(buf(p + 3)))
    p += 4
    Dim ipBA(3) As Byte
    Array.Copy(buf, p, ipBA, 0, 4)
    rv.IPAddr = New JHSoftware.SimpleDNS.Plugin.IPAddressV4(ipBA)
    Return rv
  End Function

  Class User
    Friend ID As String
    Friend IPAddr As JHSoftware.SimpleDNS.Plugin.IPAddressV4
    Friend Status As UserStatus
    Friend LastUpdate As DateTime

    Function ToListViewItem() As ListViewItem
      Dim rv As New ListViewItem(ID)
      Dim x = IPAddr.ToString
      If x = "0.0.0.0" Then rv.SubItems.Add("None") Else rv.SubItems.Add(x)
      Select Case Status
        Case UserStatus.Disabled
          rv.SubItems.Add("Disabled")
          rv.ForeColor = Color.Gray
        Case UserStatus.Offline
          rv.SubItems.Add("Offline")
          rv.ForeColor = Color.Gray
        Case UserStatus.Online
          rv.SubItems.Add("Online")
      End Select
      If LastUpdate = #1/1/1970# Then
        rv.SubItems.Add("Never")
      Else
        rv.SubItems.Add(LastUpdate.ToLocalTime.ToString)
      End If
      rv.Tag = Me
      Return rv
    End Function
  End Class

  Enum UserStatus
    Disabled = 0
    Online = 1
    Offline = 2
  End Enum

  Class MySorter
    Implements IComparer
    Friend OrderDesc As Boolean
    Friend SortCols As New List(Of Integer)

    Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements System.Collections.IComparer.Compare
      Dim u1 = DirectCast(DirectCast(x, ListViewItem).Tag, User)
      Dim u2 = DirectCast(DirectCast(y, ListViewItem).Tag, User)
      Dim rv As Integer
      For i = 0 To SortCols.Count - 1
        Select Case SortCols(i)
          Case 0 'id
            rv = u1.ID.CompareTo(u2.ID)
          Case 1 'ip
            rv = u1.IPAddr.CompareTo(u2.IPAddr)
          Case 2 'status
            rv = u1.Status.CompareTo(u2.Status)
          Case 3 'last update
            rv = u1.LastUpdate.CompareTo(u2.LastUpdate)
        End Select
        If rv <> 0 Then Return If(OrderDesc, -rv, rv)
      Next
      Return 0
    End Function
  End Class

  Private Sub lst_ColumnClick(ByVal sender As Object, ByVal e As System.Windows.Forms.ColumnClickEventArgs) Handles lst.ColumnClick
    With DirectCast(lst.ListViewItemSorter, MySorter)
      If .SortCols(0) = e.Column Then
        .OrderDesc = Not .OrderDesc
      Else
        .OrderDesc = False
        For i = 0 To .SortCols.Count - 1
          If .SortCols(i) = e.Column Then .SortCols.RemoveAt(i) : Exit For
        Next
        .SortCols.Insert(0, e.Column)
      End If
    End With
    lst.Sort()
  End Sub

  Private Sub mnuItem_Opening(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles mnuItem.Opening
    If lst.SelectedItems.Count = 0 Then e.Cancel = True : Exit Sub
    If DirectCast(lst.SelectedItems(0).Tag, User).Status <> UserStatus.Online Then e.Cancel = True : Exit Sub

  End Sub

  Private Sub mnuOffline_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuOffline.Click
    If lst.SelectedItems.Count = 0 Then Exit Sub
    With DirectCast(lst.SelectedItems(0).Tag, User)
      If .Status <> UserStatus.Online Then Exit Sub
      Dim uBA = System.Text.Encoding.ASCII.GetBytes(.ID)
      Dim ba(uBA.Length) As Byte
      ba(0) = 2
      uBA.CopyTo(ba, 1)
      SendMsgToService(ba)
    End With
  End Sub

End Class
