Friend Class JHSortedList(Of T) 'As IComparable(Of T)
  Implements IList(Of T)

  Private myList As New List(Of T)
  Private comparer As IComparer(Of T)

  Friend Sub New()
    REM default constructor
  End Sub

  Friend Sub New(ByVal comparer As IComparer(Of T))
    Me.comparer = comparer
  End Sub

  Private Function Compare(ByVal item1 As T, ByVal item2 As T) As Integer
    If comparer IsNot Nothing Then Return comparer.Compare(item1, item2)
    If TypeOf item1 Is IComparable(Of T) Then Return DirectCast(item1, IComparable(Of T)).CompareTo(item2)
    If TypeOf item1 Is IComparable Then Return DirectCast(item1, IComparable).CompareTo(item2)
    Throw New Exception("No comparer found")
  End Function

  Friend Sub ForEach(ByVal act As System.Action(Of T))
    myList.ForEach(act)
  End Sub

  Friend Function ItemsEqual(ByVal other As JHSortedList(Of T)) As Boolean
    If myList.Count <> other.myList.Count Then Return False
    For i = 0 To myList.Count - 1
      If Compare(myList(i), other.myList(i)) <> 0 Then Return False
    Next
    Return True
  End Function

  Public Sub Add(ByVal item As T) Implements System.Collections.Generic.ICollection(Of T).Add
    Dim l As Integer = 0
    If myList.Count > 0 Then
      Dim h As Integer = myList.Count - 1
      Dim m As Integer
      While l < h
        m = l + ((h - l) >> 1)
        If Compare(item, myList(m)) < 0 Then h = m Else l = m + 1
      End While
      If Compare(item, myList(l)) >= 0 Then l += 1
    End If
    myList.Insert(l, item)
  End Sub

  Public Sub Clear() Implements System.Collections.Generic.ICollection(Of T).Clear
    myList.Clear()
  End Sub

  Public Sub CopyTo(ByVal toArray() As T, ByVal arrayIndex As Integer) Implements System.Collections.Generic.ICollection(Of T).CopyTo
    myList.CopyTo(toArray, arrayIndex)
  End Sub

  Public ReadOnly Property Count() As Integer Implements System.Collections.Generic.ICollection(Of T).Count
    Get
      Return myList.Count
    End Get
  End Property

  Public ReadOnly Property IsReadOnly() As Boolean Implements System.Collections.Generic.ICollection(Of T).IsReadOnly
    Get
      Return False
    End Get
  End Property

  Public Function Remove(ByVal item As T) As Boolean Implements System.Collections.Generic.ICollection(Of T).Remove
    Dim i As Integer = IndexOf(item)
    If i < 0 Then Return False
    RemoveAt(i)
    Return True
  End Function

  Public Sub RemoveAt(ByVal index As Integer) Implements System.Collections.Generic.IList(Of T).RemoveAt
    myList.RemoveAt(index)
  End Sub

  Public Function Contains(ByVal item As T) As Boolean Implements System.Collections.Generic.ICollection(Of T).Contains
    Return (IndexOf(item) >= 0)
  End Function

  Public Function IndexOf(ByVal item As T) As Integer Implements System.Collections.Generic.IList(Of T).IndexOf
    If myList.Count = 0 Then Return -1
    Dim l As Integer = 0
    Dim h As Integer = myList.Count - 1
    Dim m As Integer
    While l < h
      m = l + ((h - l) >> 1)
      If Compare(item, myList(m)) <= 0 Then h = m Else l = m + 1
    End While
    If Compare(item, myList(l)) = 0 Then Return l Else Return -1
  End Function

  Delegate Function CompareKeyToItem(Of TKey, TItem)(ByVal key As TKey, ByVal item As TItem) As Integer

  Public Sub Insert(ByVal index As Integer, ByVal item As T) Implements System.Collections.Generic.IList(Of T).Insert
    Throw New NotImplementedException
  End Sub

  Default Public Property Item(ByVal index As Integer) As T Implements System.Collections.Generic.IList(Of T).Item
    Get
      Return myList(index)
    End Get
    Set(ByVal value As T)
      Throw New NotImplementedException
    End Set
  End Property

  Public Function GetEnumerator() As System.Collections.Generic.IEnumerator(Of T) Implements System.Collections.Generic.IEnumerable(Of T).GetEnumerator
    Return myList.GetEnumerator
  End Function
  Public Function GetEnumerator1() As System.Collections.IEnumerator Implements System.Collections.IEnumerable.GetEnumerator
    Return myList.GetEnumerator
  End Function

End Class

'*********************************************************************************************

Friend Class JHSortedList(Of TKey, TValue)
  Implements IEnumerable(Of KeyValuePair(Of TKey, TValue))

  Private myList As JHSortedList(Of KeyValuePair(Of TKey, TValue))
  Private comparer As IComparer(Of TKey)

  Private Class MyComparer
    Implements IComparer(Of Collections.Generic.KeyValuePair(Of TKey, TValue))
    Public comparer As IComparer(Of TKey)
    Public Function Compare(ByVal x As System.Collections.Generic.KeyValuePair(Of TKey, TValue), ByVal y As System.Collections.Generic.KeyValuePair(Of TKey, TValue)) As Integer Implements System.Collections.Generic.IComparer(Of System.Collections.Generic.KeyValuePair(Of TKey, TValue)).Compare
      If comparer IsNot Nothing Then Return comparer.Compare(x.Key, y.Key)
      If TypeOf x.Key Is IComparable(Of TKey) Then Return DirectCast(x.Key, IComparable(Of TKey)).CompareTo(y.Key)
      If TypeOf x.Key Is IComparable Then Return DirectCast(x.Key, IComparable).CompareTo(y.Key)
      Throw New Exception("No comparer found")
    End Function
  End Class

  Sub New()
    Dim cp As IComparer(Of Collections.Generic.KeyValuePair(Of TKey, TValue)) = New MyComparer
    myList = New JHSortedList(Of KeyValuePair(Of TKey, TValue))(cp)
  End Sub

  Sub New(ByVal comparer As IComparer(Of TKey))
    Dim cp As IComparer(Of Collections.Generic.KeyValuePair(Of TKey, TValue)) = New MyComparer With {.comparer = comparer}
    myList = New JHSortedList(Of KeyValuePair(Of TKey, TValue))(cp)
    Me.comparer = comparer
  End Sub

  Default ReadOnly Property Item(ByVal index As Integer) As TValue
    Get
      Return myList(index).Value
    End Get
  End Property

  Public Sub Add(ByVal key As TKey, ByVal value As TValue)
    myList.Add(New KeyValuePair(Of TKey, TValue)(key, value))
  End Sub

  Public Sub Clear()
    myList.Clear()
  End Sub

  Public Function IndexOf(ByVal key As TKey) As Integer
    If myList.Count = 0 Then Return -1
    Dim l As Integer = 0
    Dim h As Integer = myList.Count - 1
    Dim m As Integer
    While l < h
      m = l + ((h - l) >> 1)
      If KeyCompare(key, myList(m).Key) <= 0 Then h = m Else l = m + 1
    End While
    If KeyCompare(key, myList(l).Key) = 0 Then Return l Else Return -1
  End Function

  Public Function IndexOf(ByVal key As TKey, ByVal value As TValue, Optional ByVal ve As ValueEquals = Nothing) As Integer
    If ve Is Nothing Then ve = Function(v1 As TValue, v2 As TValue) Object.ReferenceEquals(v1, v2)
    Dim i = IndexOf(key)
    If i < 0 Then Return i
    While i < myList.Count
      If KeyCompare(myList(i).Key, key) <> 0 Then Return -1
      If ve(value, myList(i).Value) Then Return i
      i += 1
    End While
    Return -1
  End Function

  Delegate Function ValueEquals(ByVal val1 As TValue, ByVal val2 As TValue) As Boolean

  Private Function KeyCompare(ByVal x As TKey, ByVal y As TKey) As Integer
    If comparer IsNot Nothing Then Return comparer.Compare(x, y)
    If TypeOf x Is IComparable(Of TKey) Then Return DirectCast(x, IComparable(Of TKey)).CompareTo(y)
    If TypeOf x Is IComparable Then Return DirectCast(x, IComparable).CompareTo(y)
    Throw New Exception("No comparer found")
  End Function

  Public ReadOnly Property Count() As Integer
    Get
      Return myList.Count
    End Get
  End Property

  Public Sub Remove(ByVal key As TKey, ByVal value As TValue, Optional ByVal ve As ValueEquals = Nothing)
    Dim i = IndexOf(key, value, ve)
    If i >= 0 Then myList.RemoveAt(i)
  End Sub

  Public Sub RemoveAt(ByVal index As Integer)
    myList.RemoveAt(index)
  End Sub

  Public Function GetEnumerator() As System.Collections.Generic.IEnumerator(Of System.Collections.Generic.KeyValuePair(Of TKey, TValue)) Implements System.Collections.Generic.IEnumerable(Of System.Collections.Generic.KeyValuePair(Of TKey, TValue)).GetEnumerator
    Return myList.GetEnumerator
  End Function

  Public Function GetEnumerator1() As System.Collections.IEnumerator Implements System.Collections.IEnumerable.GetEnumerator
    Return myList.GetEnumerator
  End Function
End Class
