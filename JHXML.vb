'Imports System.Windows.Forms

Namespace System.Runtime.CompilerServices
  'TODO: V6: remove this when upgrading to .NET 3.5
  Friend NotInheritable Class ExtensionAttribute
    Inherits System.Attribute
  End Class
End Namespace

Module JHXML
  <System.Runtime.CompilerServices.Extension()>
  Function GetAttrBool(elem As Xml.XmlElement, name As String, Optional defVal As Boolean = False) As Boolean
    If Not elem.HasAttribute(name) Then Return defVal
    Return (elem.GetAttribute(name).ToLowerInvariant.Trim = "true")
  End Function
  <System.Runtime.CompilerServices.Extension()>
  Function TryGetAttrBool(elem As Xml.XmlElement, name As String, ByRef toVal As Boolean) As Boolean
    If Not elem.HasAttribute(name) Then Return False
    toVal = (elem.GetAttribute(name).ToLowerInvariant.Trim = "true")
    Return True
  End Function
  <System.Runtime.CompilerServices.Extension()>
  Sub SetAttrBool(elem As Xml.XmlElement, name As String, value As Boolean)
    elem.SetAttribute(name, If(value, "true", "false"))
  End Sub

  <System.Runtime.CompilerServices.Extension()>
  Function GetAttrInt(elem As Xml.XmlElement, name As String, Optional defVal As Integer = 0) As Integer
    If Not elem.HasAttribute(name) Then Return defVal
    Dim rv As Integer
    If Not Integer.TryParse(elem.GetAttribute(name), rv) Then Return defVal
    Return rv
  End Function
  <System.Runtime.CompilerServices.Extension()>
  Function TryGetAttrInt(elem As Xml.XmlElement, name As String, ByRef toVal As Integer) As Boolean
    If Not elem.HasAttribute(name) Then Return False
    Return Integer.TryParse(elem.GetAttribute(name), toVal)
  End Function
  <System.Runtime.CompilerServices.Extension()>
  Sub SetAttrInt(elem As Xml.XmlElement, name As String, value As Integer)
    elem.SetAttribute(name, value.ToString)
  End Sub

  <System.Runtime.CompilerServices.Extension()>
  Function GetAttrStr(elem As Xml.XmlElement, name As String, Optional defVal As String = "") As String
    If Not elem.HasAttribute(name) Then Return defVal
    Return elem.GetAttribute(name)
  End Function
  <System.Runtime.CompilerServices.Extension()>
  Function TryGetAttrStr(elem As Xml.XmlElement, name As String, ByRef toVal As String) As Boolean
    If Not elem.HasAttribute(name) Then Return False
    toVal = elem.GetAttribute(name)
    Return True
  End Function

  <System.Runtime.CompilerServices.Extension()>
  Sub SetAttrDateTime(elem As Xml.XmlElement, name As String, value As DateTime)
    elem.SetAttribute(name, Xml.XmlConvert.ToString(value, Xml.XmlDateTimeSerializationMode.Unspecified))
  End Sub

  <System.Runtime.CompilerServices.Extension()>
  Function GetAttrDateTime(elem As Xml.XmlElement, name As String, defVal As DateTime) As DateTime
    Try
      Return Xml.XmlConvert.ToDateTime(elem.GetAttribute(name), Xml.XmlDateTimeSerializationMode.Unspecified)
    Catch
      Return defVal
    End Try
  End Function
  <System.Runtime.CompilerServices.Extension()>
  Function TryGetAttrDateTime(elem As Xml.XmlElement, name As String, ByRef toVal As DateTime) As Boolean
    Try
      toVal = Xml.XmlConvert.ToDateTime(elem.GetAttribute(name), Xml.XmlDateTimeSerializationMode.Unspecified)
      Return True
    Catch
      Return False
    End Try
  End Function

  <System.Runtime.CompilerServices.Extension()>
  Function CreateChildElement(elem As Xml.XmlElement, name As String) As Xml.XmlElement
    Dim rv = elem.OwnerDocument.CreateElement(name)
    elem.AppendChild(rv)
    Return rv
  End Function

  <System.Runtime.CompilerServices.Extension()>
  Friend Function GetChildElementsByName(node As Xml.XmlNode, name As String) As List(Of Xml.XmlElement)
    Dim rv As New List(Of Xml.XmlElement)
    For Each n In node.ChildNodes
      If TypeOf n Is Xml.XmlElement AndAlso
         DirectCast(n, Xml.XmlElement).Name = name Then rv.Add(DirectCast(n, Xml.XmlElement))
    Next
    Return rv
  End Function

  <System.Runtime.CompilerServices.Extension()>
  Friend Function HasChildElement(elem As Xml.XmlElement, name As String) As Boolean
    For Each n In elem.ChildNodes
      If TypeOf n Is Xml.XmlElement AndAlso
         DirectCast(n, Xml.XmlElement).Name = name Then Return True
    Next
    Return False
  End Function

  <System.Runtime.CompilerServices.Extension()>
  Friend Function PrepConfig(doc As Xml.XmlDocument) As Xml.XmlElement
    doc.AppendChild(doc.CreateXmlDeclaration("1.0", Nothing, Nothing))
    Dim rv As Xml.XmlElement = doc.CreateElement("config")
    doc.AppendChild(rv)
    Return rv
  End Function

  <System.Runtime.CompilerServices.Extension()>
  Friend Function PrepRoot(doc As Xml.XmlDocument, rootName As String) As Xml.XmlElement
    doc.AppendChild(doc.CreateXmlDeclaration("1.0", Nothing, Nothing))
    Dim rv As Xml.XmlElement = doc.CreateElement(rootName)
    doc.AppendChild(rv)
    Return rv
  End Function

End Module

