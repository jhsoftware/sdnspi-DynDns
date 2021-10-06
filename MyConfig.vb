Imports JHSoftware.SimpleDNS.Plugin

Friend Class MyConfig
  Public Suffix As DomName
  Public SuffixSegCt As Integer
  Public Users As New Dictionary(Of DomName, User)
  Public UpdateZones As Boolean

  REM update methods / URLs
  Public UpMeTsig As Boolean = True
  Public UpMeGnuDIP As Boolean = False
  Public UpMeHttpBasic As Boolean = False
  Public UpMeHttpUrl As Boolean = False
  Public UpMeHttpGD As Boolean = False
  Public UpMeHttpDynCom As Boolean = False
  Public GnuDIPPort As Integer = 3495

  Public RemoteDetect As Boolean = False

  Public BaseURL As String = "http://"

  Friend Function BaseUrlInUse() As Boolean
    Return (UpMeHttpBasic Or UpMeHttpUrl Or UpMeHttpGD Or RemoteDetect)
  End Function

  Friend Function SaveToXML() As Xml.XmlDocument
    Dim doc As New Xml.XmlDocument
    Dim root = doc.PrepConfig()
    root.SetAttribute("Suffix", Suffix.ToString)
    root.SetAttrBool("UpdateZones", UpdateZones)
    root.SetAttrBool("UpMeTSIG", UpMeTsig)
    root.SetAttrBool("UpMeGnuDIP", UpMeGnuDIP)
    root.SetAttrBool("UpMeHTTPURL", UpMeHttpUrl)
    root.SetAttrBool("UpMeHTTPBasic", UpMeHttpBasic)
    root.SetAttrBool("UpMeHTTPGnuDIP", UpMeHttpGD)
    root.SetAttrBool("UpMeHTTPDynCom", UpMeHttpDynCom)
    root.SetAttrInt("GNUDIPPort", GnuDIPPort)
    root.SetAttrBool("RemoteDetect", RemoteDetect)
    root.SetAttribute("BaseURL", BaseURL)
    For Each User In Users.Values
      User.SaveToXML(root.CreateChildElement("User"))
    Next
    Return doc
  End Function

  Friend Shared Function LoadFromXML(config As String) As MyConfig
    Dim rv As New MyConfig
    Dim doc = New Xml.XmlDocument
    doc.LoadXml(config)
    Dim root = DirectCast(doc.GetElementsByTagName("config").Item(0), Xml.XmlElement)
    With rv
      .Suffix = DomName.Parse(root.GetAttribute("Suffix"))
      .SuffixSegCt = .Suffix.SegmentCount
      .UpdateZones = root.GetAttrBool("UpdateZones")
      .UpMeTsig = root.GetAttrBool("UpMeTSIG", True)
      .UpMeGnuDIP = root.GetAttrBool("UpMeGnuDIP")
      .UpMeHttpUrl = root.GetAttrBool("UpMeHTTPURL")
      .UpMeHttpBasic = root.GetAttrBool("UpMeHTTPBasic")
      .UpMeHttpGD = root.GetAttrBool("UpMeHTTPGnuDIP")
      .UpMeHttpDynCom = root.GetAttrBool("UpMeHTTPDynCom")
      .GnuDIPPort = root.GetAttrInt("GNUDIPPort", 3495)
      .RemoteDetect = root.GetAttrBool("RemoteDetect")
      .BaseURL = root.GetAttribute("BaseURL")
      Dim u As User
      For Each elem As Xml.XmlElement In root.GetElementsByTagName("User")
        u = User.FromXML(elem)
        .Users.Add(u.ID, u)
      Next

      REM v. 5.1 options - no longer supported
      '.Recursion = root.GetAttrBool("Recursion")
      '.DNSBLWhiteList = root.GetAttrBool("DNSBLWhitelist")
      '.DNSBLDom = If(root.HasAttribute("DNSBLDomain"), DomName.Parse(root.GetAttribute("DNSBLDomain")), Nothing)
    End With
    Return rv
  End Function

  Friend Class User
    Public ID As DomName
    Public Password As String
    Public TSIGKeyValue As Byte()
    Public TSIGAutoHash As Boolean
    Public OffLineIP As SdnsIPv4
    Public HostNames As New List(Of DomName)
    Public Notes As String
    Public Disabled As Boolean

    Public Overrides Function ToString() As String
      Return ID.ToString & If(Disabled, " [disabled]", "")
    End Function

    Friend Sub SaveToXML(elem As Xml.XmlElement)
      elem.SetAttribute("ID", ID.ToString)
      elem.SetAttribute("Password", Password)
      If Not TSIGAutoHash Then elem.SetAttribute("TSIGKeyValue", Convert.ToBase64String(TSIGKeyValue))
      If OffLineIP IsNot Nothing Then elem.SetAttribute("OfflineIP", OffLineIP.ToString)
      If Notes.Length > 0 Then elem.SetAttribute("Notes", Notes)
      If Disabled Then elem.SetAttrBool("Disabled", True)
      For Each hn In HostNames
        elem.CreateChildElement("HostName").InnerText = hn.ToString
      Next
    End Sub

    Friend Shared Function FromXML(elem As Xml.XmlElement) As User
      Dim rv As New User
      With rv
        .ID = DomName.Parse(elem.GetAttribute("ID"))
        .Password = elem.GetAttribute("Password")
        If elem.HasAttribute("TSIGKeyValue") Then
          .TSIGAutoHash = False
          .TSIGKeyValue = Convert.FromBase64String(elem.GetAttribute("TSIGKeyValue"))
        Else
          .TSIGAutoHash = True
          Dim md5 As New System.Security.Cryptography.MD5CryptoServiceProvider
          .TSIGKeyValue = md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(.Password))
        End If
        .OffLineIP = If(elem.HasAttribute("OfflineIP"), SdnsIPv4.Parse(elem.GetAttribute("OfflineIP")), Nothing)
        .Notes = elem.GetAttrStr("Notes")
        .Disabled = elem.GetAttrBool("Disabled")
        For Each elem2 As Xml.XmlElement In elem.GetElementsByTagName("HostName")
          .HostNames.Add(DomName.Parse(elem2.InnerText))
        Next
      End With
      Return rv
    End Function


    REM ***************** state *******************
    Friend LastUpdate As DateTime = #1/1/1970#
    Friend CurIP As SdnsIPv4 = Nothing
    Friend CurTTL As Integer = DynDNSPlugIn.DefaultTTL
    Friend Offline As Boolean = True

  End Class

End Class
