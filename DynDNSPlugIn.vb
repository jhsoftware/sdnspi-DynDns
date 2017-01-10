Imports JHSoftware.SimpleDNS.Plugin

Public Class DynDNSPlugIn
  Implements IGetHostPlugIn
  Implements ITSIGUpdateHost
  Implements IViewUI
  Implements IUpdateHost
  Implements IListsIPAddress
  Implements IListsDomainName
  Implements IQuestions

  Friend Const DefaultTTL As Integer = 30

  Friend Cfg As MyConfig

  Friend HostUser As Dictionary(Of JHSoftware.SimpleDNS.Plugin.DomainName, MyConfig.User)
  Friend HostUserWC As Dictionary(Of JHSoftware.SimpleDNS.Plugin.DomainName, MyConfig.User)

  Friend UserByIP As JHSortedList(Of IPAddressV4, MyConfig.User)

  Friend hli As Net.HttpListener
  Friend gdli As Net.Sockets.Socket

  Friend IsStopping As Boolean

  Friend GnuDIPKey(15) As Byte

#Region "events"
  Public Event LogLine(ByVal text As String) Implements JHSoftware.SimpleDNS.Plugin.IPlugInBase.LogLine
  Public Event AsyncError(ByVal ex As System.Exception) Implements JHSoftware.SimpleDNS.Plugin.IPlugInBase.AsyncError
  Public Event SaveConfig(ByVal config As String) Implements JHSoftware.SimpleDNS.Plugin.IPlugInBase.SaveConfig
  Public Event MsgToViewUI(ByVal connID As Integer, ByVal msg() As Byte) Implements JHSoftware.SimpleDNS.Plugin.IViewUI.MsgToViewUI
  Public Event UpdateHost(ByVal hostName As JHSoftware.SimpleDNS.Plugin.DomainName, ByVal ipAddress As IPAddress, ByVal ttl As Integer, ByVal comment As String, ByRef result As Boolean, ByRef failReason As String) Implements JHSoftware.SimpleDNS.Plugin.IUpdateHost.UpdateHost
  Public Event UpdateHostReverse(ByVal ipAddress As IPAddress, ByVal hostName As JHSoftware.SimpleDNS.Plugin.DomainName, ByVal ttl As Integer, ByVal comment As String, ByRef result As Boolean, ByRef failReason As String) Implements JHSoftware.SimpleDNS.Plugin.IUpdateHost.UpdateHostReverse
#End Region

#Region "not implemented"

  Public Sub LookupReverse(ByVal req As IDNSRequest, ByRef resultName As JHSoftware.SimpleDNS.Plugin.DomainName, ByRef resultTTL As Integer) Implements JHSoftware.SimpleDNS.Plugin.IGetHostPlugIn.LookupReverse
  End Sub

  Public Sub LookupTXT(ByVal req As IDNSRequest, ByRef resultText As String, ByRef resultTTL As Integer) Implements JHSoftware.SimpleDNS.Plugin.IGetHostPlugIn.LookupTXT
  End Sub

#End Region

#Region "Other methods"

  Public Function GetPlugInTypeInfo() As JHSoftware.SimpleDNS.Plugin.IPlugInBase.PlugInTypeInfo Implements JHSoftware.SimpleDNS.Plugin.IPlugInBase.GetPlugInTypeInfo
    With GetPlugInTypeInfo
      .Name = "DynDNS Service"
      .Description = "Accepts remote updates from DynDNS clients"
      .InfoURL = "http://www.simpledns.com/kb.asp?kbid=1267"
      .ConfigFile = True
      .MultiThreaded = False
    End With
  End Function

  Public Function GetDNSAskAbout() As JHSoftware.SimpleDNS.Plugin.DNSAskAboutGH Implements JHSoftware.SimpleDNS.Plugin.IGetHostPlugIn.GetDNSAskAbout
    With GetDNSAskAbout
      .ForwardIPv4 = True
    End With
  End Function

  Public Function GetOptionsUI(ByVal instanceID As Guid, ByVal dataPath As String) As JHSoftware.SimpleDNS.Plugin.OptionsUI Implements JHSoftware.SimpleDNS.Plugin.IPlugInBase.GetOptionsUI
    Return New OptionsUI
  End Function

  Public Function InstanceConflict(ByVal config1 As String, ByVal config2 As String, ByRef errorMsg As String) As Boolean Implements JHSoftware.SimpleDNS.Plugin.IPlugInBase.InstanceConflict
    Dim c1 = MyConfig.LoadFromXML(config1)
    Dim c2 = MyConfig.LoadFromXML(config2)
    If c1.Suffix = c2.Suffix Then
      errorMsg = "Another plug-in instance is using the same host name suffix"
      Return True
    End If
    If c1.AnyHTTPServices AndAlso c2.AnyHTTPServices AndAlso _
       c1.BaseURL.ToLower = c2.BaseURL.ToLower Then
      errorMsg = "Another plug-in instance is using the same base URL for HTTP services"
      Return True
    End If
    Return False
  End Function

  Public Sub LoadConfig(ByVal config As String, ByVal instanceID As Guid, ByVal dataPath As String, ByRef maxThreads As Integer) Implements JHSoftware.SimpleDNS.Plugin.IPlugInBase.LoadConfig
    Cfg = MyConfig.LoadFromXML(config)

    HostUser = New Dictionary(Of JHSoftware.SimpleDNS.Plugin.DomainName, MyConfig.User)
    HostUserWC = New Dictionary(Of JHSoftware.SimpleDNS.Plugin.DomainName, MyConfig.User)
    Dim wc = JHSoftware.SimpleDNS.Plugin.DomainName.Parse("*")
    For Each user In Cfg.Users.Values
      If user.Disabled Then Continue For
      HostUser.Add(user.ID & Cfg.Suffix, user)
      For Each hn In user.HostNames
        If hn.GetSegments(0, 1) = wc Then
          HostUserWC.Add(hn.GetSegments(1, hn.SegmentCount - 1), user)
        Else
          HostUser.Add(hn, user)
        End If
      Next
    Next

    UserByIP = New JHSortedList(Of IPAddressV4, MyConfig.User)
  End Sub

  Public Function SaveState() As String Implements JHSoftware.SimpleDNS.Plugin.IPlugInBase.SaveState
    Dim doc As New Xml.XmlDocument
    Dim root = doc.PrepConfig
    For Each user In Cfg.Users.Values
      With root.CreateChildElement("User")
        .SetAttribute("ID", user.ID.ToString)
        If user.CurIP IsNot Nothing Then .SetAttribute("IP", user.CurIP.ToString)
        If user.CurTTL <> DefaultTTL Then .SetAttrInt("TTL", user.CurTTL)
        If user.LastUpdate <> #1/1/1970# Then .SetAttrDateTime("LastUpdate", user.LastUpdate)
        If user.Offline Then .SetAttrBool("Offline", True)
      End With
    Next
    Return doc.OuterXml
  End Function

  Public Sub LoadState(ByVal state As String) Implements JHSoftware.SimpleDNS.Plugin.IPlugInBase.LoadState
    UserByIP = New JHSortedList(Of IPAddressV4, MyConfig.User)
    If state.Length = 0 Then Exit Sub
    Dim doc = New Xml.XmlDocument
    doc.LoadXml(state)
    Dim root = DirectCast(doc.GetElementsByTagName("config").Item(0), Xml.XmlElement)
    Dim user As MyConfig.User = Nothing
    For Each elem As Xml.XmlElement In root.GetElementsByTagName("User")
      If Cfg.Users.TryGetValue(JHSoftware.SimpleDNS.Plugin.DomainName.Parse(elem.GetAttribute("ID")), user) Then
        If elem.HasAttribute("IP") Then user.CurIP = IPAddressV4.Parse(elem.GetAttribute("IP"))
        user.CurTTL = elem.GetAttrInt("TTL", DefaultTTL)
        user.LastUpdate = elem.GetAttrDateTime("LastUpdate", #1/1/1970#)
        user.Offline = elem.GetAttrBool("Offline")
        If Not user.Disabled AndAlso _
           Not user.Offline AndAlso _
           user.CurIP IsNot Nothing Then UserByIP.Add(user.CurIP, user)
      End If
    Next
  End Sub

  Public Sub Lookup(ByVal req As IDNSRequest, ByRef resultIP As IPAddress, ByRef resultTTL As Integer) Implements JHSoftware.SimpleDNS.Plugin.IGetHostPlugIn.Lookup
    Dim lookupName = req.QName
    SyncLock Me
      Dim user = FindUserWithDomain(lookupName)
      If user Is Nothing OrElse user.Disabled Then resultIP = Nothing : Exit Sub
      If user.Offline AndAlso user.OffLineIP IsNot Nothing Then
        resultIP = user.OffLineIP
        resultTTL = DynDNSPlugIn.DefaultTTL
      Else
        resultIP = user.CurIP
        resultTTL = user.CurTTL
      End If
    End SyncLock
  End Sub

  Public Sub StartService() Implements JHSoftware.SimpleDNS.Plugin.IPlugInBase.StartService
    IsStopping = False
    myRandom.NextBytes(GnuDIPKey)
    If Cfg.UpMeGnuDIP Then
      gdli = New Net.Sockets.Socket(Net.Sockets.AddressFamily.InterNetwork, Net.Sockets.SocketType.Stream, Net.Sockets.ProtocolType.Tcp)
      Try
        gdli.Bind(New Net.IPEndPoint(Net.IPAddress.Any, Cfg.GnuDIPPort))
        gdli.Listen(5)
        gdli.BeginAccept(AddressOf GDLI_CallBack, gdli)
        RaiseEvent LogLine("Listening for GnuDIP requests on TCP port " & Cfg.GnuDIPPort)
      Catch ex As Exception
        RaiseEvent LogLine("GnuDIP socket not started - Error: " & ex.Message)
      End Try
    End If
    If Cfg.AnyHTTPServices Then
      hli = New Net.HttpListener
      hli.IgnoreWriteExceptions = True
      hli.AuthenticationSchemes = Net.AuthenticationSchemes.Anonymous Or Net.AuthenticationSchemes.Basic
      Try
        hli.Prefixes.Add(Cfg.BaseURL)
        hli.Start()
        hli.BeginGetContext(AddressOf HLI_CallBack, hli)
      Catch ex As Exception
        RaiseEvent LogLine("HTTP listener not started - Error: " & ex.Message)
        Exit Sub
      End Try
      RaiseEvent LogLine("Listening for HTTP requests at " & Cfg.BaseURL)
    End If
  End Sub

  Public Sub StopService() Implements JHSoftware.SimpleDNS.Plugin.IPlugInBase.StopService
    IsStopping = True
    If gdli IsNot Nothing Then
      Try
        gdli.Close()
      Catch
      End Try
      gdli = Nothing
    End If
    If hli IsNot Nothing Then
      Try
        hli.Stop()
        hli.Abort()
      Catch
      End Try
      hli = Nothing
    End If
  End Sub

  Public Function GetTSIGKeySecret(ByVal keyName As JHSoftware.SimpleDNS.Plugin.DomainName, ByVal algorithm As String) As Byte() Implements JHSoftware.SimpleDNS.Plugin.ITSIGUpdateHost.GetTSIGKeySecret
    If Not Cfg.UpMeTsig Then Return Nothing
    If keyName.SegmentCount <> 1 Then Return Nothing
    If algorithm <> "HMAC-MD5" Then Return Nothing
    SyncLock Me
      Dim user As MyConfig.User = Nothing
      If Not Cfg.Users.TryGetValue(keyName, user) Then Return Nothing
      If user.Disabled Then Return Nothing
      Return user.TSIGKeyValue
    End SyncLock
  End Function

  Public Function TSIGUpdateHost(ByVal fromIP As IPAddress, _
                                 ByVal keyName As JHSoftware.SimpleDNS.Plugin.DomainName, _
                                 ByVal algorithm As String, _
                                 ByVal hostName As JHSoftware.SimpleDNS.Plugin.DomainName, _
                                 ByVal ipAddress As IPAddress, _
                                 ByVal ttl As Integer) As Boolean Implements JHSoftware.SimpleDNS.Plugin.ITSIGUpdateHost.TSIGUpdateHost
    If Not Cfg.UpMeTsig Then Return False
    If ipAddress.IPVersion <> 4 Then Return False
    If keyName.SegmentCount <> 1 Then Return False
    If keyName & Cfg.Suffix <> hostName Then Return False
    If algorithm <> "HMAC-MD5" Then Return False
    SyncLock Me
      Dim user As MyConfig.User = Nothing
      If Not Cfg.Users.TryGetValue(keyName, user) Then Return False
      If user.Disabled Then Return False
      PerformUpdate(user, DirectCast(ipAddress, IPAddressV4), ttl, "DNS TSIG")
      Return True
    End SyncLock
  End Function

#End Region

  Friend Sub SetUserOffline(ByVal user As MyConfig.User, ByVal UpMethod As String)
    If user.Disabled Then Exit Sub
    If user.OffLineIP IsNot Nothing Then
      PerformUpdate(user, user.OffLineIP, DynDNSPlugIn.DefaultTTL, UpMethod, True)
    Else
      RaiseEvent LogLine("User '" & user.ID.ToString & "' offline via " & UpMethod)
      If user.CurIP IsNot Nothing Then UserByIP.Remove(user.CurIP, user)
      user.Offline = True
      user.LastUpdate = DateTime.UtcNow
      SendViewUserMsg(-1, 2, user)
    End If
  End Sub

  Friend Sub PerformUpdate(ByVal user As MyConfig.User, _
                           ByVal newIP As IPAddressV4, _
                           ByVal newTTL As Integer, _
                           ByVal UpMethod As String, _
                           Optional ByVal OffLine As Boolean = False)
    If user.Disabled Then Exit Sub

    If user.CurIP IsNot Nothing Then UserByIP.Remove(user.CurIP, user)
    user.CurIP = newIP
    user.CurTTL = newTTL
    user.Offline = OffLine
    user.LastUpdate = DateTime.UtcNow
    If Not OffLine Then UserByIP.Add(newIP, user)

    If OffLine Then
      RaiseEvent LogLine("User '" & user.ID.ToString & "' offline via " & UpMethod)
    Else
      RaiseEvent LogLine("User '" & user.ID.ToString & "' now online at " & newIP.ToString & " via " & UpMethod)
    End If

    SendViewUserMsg(-1, 2, user)

    If Cfg.UpdateZones Then
      Dim recComment = "user '" & user.ID.ToString & "' via " & UpMethod
      Dim result As Boolean
      Dim failReason As String = Nothing
      Dim hn = user.ID & Cfg.Suffix
      RaiseEvent UpdateHost(hn, newIP, newTTL, recComment, result, failReason)
      If Not result Then RaiseEvent LogLine("Failed to update A-record " & hn.ToString & " = " & newIP.ToString & " in local zone: " & failReason)
      For Each hn In user.HostNames
        RaiseEvent UpdateHost(hn, newIP, newTTL, recComment, result, failReason)
        If Not result Then RaiseEvent LogLine("Failed to update A-record " & hn.ToString & " = " & newIP.ToString & " in local zone: " & failReason)
      Next
    End If

  End Sub

  Sub GDLI_CallBack(ByVal ia As IAsyncResult)
    Try
      Dim sck As Net.Sockets.Socket = Nothing
      Try
        sck = gdli.EndAccept(ia)
      Catch ex As Net.Sockets.SocketException
        If IsStopping Then Exit Sub
        REM ignore 10054 here - confusing users
        If ex.SocketErrorCode <> Net.Sockets.SocketError.ConnectionReset Then
          RaiseEvent LogLine("Socket error accepting GnuDIP TCP connection: " & ex.ErrorCode & " " & ex.Message)
        End If
        GoTo markWaitForNext
      End Try

      Dim conn = New GnuDIPConn
      conn.plugin = Me
      conn.sock = sck
      conn.Process()

markWaitForNext:
      gdli.BeginAccept(AddressOf GDLI_CallBack, gdli)

    Catch ex As Exception
      If IsStopping Then Exit Sub
      RaiseEvent AsyncError(ex)
    End Try
  End Sub

  Private Sub HLI_CallBack(ByVal ia As IAsyncResult)
    Try
      REM in case plug-in has been restarted - error reported by jAssing on 28.2.2009
      If ia.AsyncState IsNot hli Then Exit Sub

      Dim ctx = hli.EndGetContext(ia)

      SyncLock Me
        Dim h As New HTTPHandler
        h.ctx = ctx
        h.plugin = Me
        h.Process()
      End SyncLock

      hli.BeginGetContext(AddressOf HLI_CallBack, hli)

    Catch ex As Exception
      If IsStopping Then Exit Sub
      RaiseEvent AsyncError(ex)
    End Try
  End Sub

  Friend Sub ReportAsyncError(ByVal ex As Exception)
    RaiseEvent AsyncError(ex)
  End Sub

  Public Function GetViewUI() As JHSoftware.SimpleDNS.Plugin.ViewUI Implements JHSoftware.SimpleDNS.Plugin.IViewUI.GetViewUI
    Return New ViewUI
  End Function

  Public Sub MsgFromViewUI(ByVal connID As Integer, ByVal msg() As Byte) Implements JHSoftware.SimpleDNS.Plugin.IViewUI.MsgFromViewUI
    Select Case msg(0)
      Case 1 ' get list 
        For Each user In Cfg.Users.Values
          If Not user.Disabled Then SendViewUserMsg(connID, 1, user)
        Next
      Case 2 'set user offline
        Dim userID = JHSoftware.SimpleDNS.Plugin.DomainName.Parse(System.Text.Encoding.ASCII.GetString(msg, 1, msg.Length - 1))
        Dim user As MyConfig.User = Nothing
        If Not Cfg.Users.TryGetValue(userID, user) Then Exit Sub
        SetUserOffline(user, "GUI")
    End Select
  End Sub

  Private Sub SendViewUserMsg(ByVal connID As Integer, ByVal cmd As Byte, ByVal user As MyConfig.User)
    Dim baID = user.ID.GetBytesNT()
    Dim ba(baID.Length + 10 - 1) As Byte
    ba(0) = cmd
    baID.CopyTo(ba, 1)
    Dim p = 1 + baID.Length
    If user.Disabled Then
      ba(p) = 0
    ElseIf user.Offline Then
      ba(p) = 2
    Else
      ba(p) = 1
    End If
    p += 1
    Dim lu = CUInt(user.LastUpdate.Subtract(#1/1/1970#).TotalSeconds)
    ba(p) = CByte(lu >> 24)
    ba(p + 1) = CByte((lu >> 16) And 255)
    ba(p + 2) = CByte((lu >> 8) And 255)
    ba(p + 3) = CByte(lu And 255)
    p += 4
    If user.CurIP IsNot Nothing Then user.CurIP.GetBytes.CopyTo(ba, p)
    RaiseEvent MsgToViewUI(connID, ba)
  End Sub

  Public Function ListsIPAddress(ByVal ip As JHSoftware.SimpleDNS.Plugin.IPAddress) As Boolean Implements JHSoftware.SimpleDNS.Plugin.IListsIPAddress.ListsIPAddress
    SyncLock Me
      Return FindActiveUserWithIP(ip) IsNot Nothing
    End SyncLock
  End Function

  Public Function ListsDomainName(ByVal domain As JHSoftware.SimpleDNS.Plugin.DomainName) As Boolean Implements JHSoftware.SimpleDNS.Plugin.IListsDomainName.ListsDomainName
    SyncLock Me
      Dim user = FindUserWithDomain(domain)
      If user Is Nothing OrElse user.Disabled OrElse user.Offline Then Return False
      Return True
    End SyncLock
  End Function


  Public Function QuestionList() As JHSoftware.SimpleDNS.Plugin.IQuestions.QuestionInfo() Implements JHSoftware.SimpleDNS.Plugin.IQuestions.QuestionList
    Dim rv(0) As IQuestions.QuestionInfo
    rv(0).ID = 1
    rv(0).Description = "Notes of DynDNS user, who sent DNS request, contain text"
    rv(0).HasUI = True
    Return rv
  End Function

  Public Function QuestionGetUI(ByVal id As Integer) As JHSoftware.SimpleDNS.Plugin.OptionsUI Implements JHSoftware.SimpleDNS.Plugin.IQuestions.QuestionGetUI
    Return New Q1OptionsUI
  End Function

  Public Function QuestionLoadConfig(ByVal id As Integer, ByVal configStr As String) As Object Implements JHSoftware.SimpleDNS.Plugin.IQuestions.QuestionLoadConfig
    Return configStr
  End Function

  Public Function QuestionAsk(ByVal id As Integer, ByVal configObj As Object, ByVal req As JHSoftware.SimpleDNS.Plugin.IDNSRequest) As Boolean Implements JHSoftware.SimpleDNS.Plugin.IQuestions.QuestionAsk
    SyncLock Me
      Dim user = FindActiveUserWithIP(req.FromIP)
      If user Is Nothing Then Return False
      Return (user.Notes.IndexOf(DirectCast(configObj, String), StringComparison.CurrentCultureIgnoreCase) >= 0)
    End SyncLock
  End Function

  Private Function FindActiveUserWithIP(ByVal ip As IPAddress) As MyConfig.User
    If ip.IPVersion <> 4 Then Return Nothing
    Dim i = UserByIP.IndexOf(DirectCast(ip, IPAddressV4))
    If i < 0 Then Return Nothing
    Do
      With UserByIP(i)
        If Not .Disabled AndAlso Not .Offline Then Return UserByIP(i)
      End With
      i += 1
    Loop While i < UserByIP.Count AndAlso UserByIP(i).CurIP = DirectCast(ip, IPAddressV4)
    Return Nothing
  End Function

  Private Function FindUserWithDomain(ByVal dom As DomainName) As MyConfig.User
    Dim rv As MyConfig.User = Nothing
    If HostUser.TryGetValue(dom, rv) Then Return rv
    If HostUserWC.Count = 0 Then Return Nothing
    Dim segCt = dom.SegmentCount
    While segCt > 1
      dom = dom.Parent
      segCt -= 1
      If HostUserWC.TryGetValue(dom, rv) Then Return rv
    End While
    Return Nothing
  End Function
End Class

