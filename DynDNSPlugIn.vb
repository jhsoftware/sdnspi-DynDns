Imports System.Threading.Tasks
Imports JHSoftware.SimpleDNS.Plugin

Public Class DynDNSPlugIn
  Implements ILookupHost
  Implements ITSIGUpdateHost
  Implements IViewUI
  Implements IListsIPAddress
  Implements IListsDomainName
  Implements IQuestions
  Implements IOptionsUI
  Implements IState

  Friend Const DefaultTTL As Integer = 30

  Friend Cfg As MyConfig

  Friend HostUser As Dictionary(Of DomName, MyConfig.User)
  Friend HostUserWC As Dictionary(Of DomName, MyConfig.User)

  Friend UserByIP As JHSortedList(Of SdnsIPv4, MyConfig.User)

  Friend hli As Net.HttpListener
  Friend gdli As Net.Sockets.Socket

  Friend IsStopping As Boolean

  Friend GnuDIPKey(15) As Byte

  Public Property Host As IHost Implements IPlugInBase.Host

#Region "events"
  Public Event MsgToViewUI(connID As Integer, msg() As Byte) Implements JHSoftware.SimpleDNS.Plugin.IViewUI.MsgToViewUI
#End Region

#Region "Other methods"

  Public Function GetPlugInTypeInfo() As TypeInfo Implements JHSoftware.SimpleDNS.Plugin.IPlugInBase.GetTypeInfo
    With GetPlugInTypeInfo
      .Name = "DynDNS Service"
      .Description = "Accepts remote updates from DynDNS clients"
      .InfoURL = "https://simpledns.plus/plugin-dyndns"
    End With
  End Function

  Public Function GetOptionsUI(instanceID As Guid, dataPath As String) As JHSoftware.SimpleDNS.Plugin.OptionsUI Implements JHSoftware.SimpleDNS.Plugin.IOptionsUI.GetOptionsUI
    Return New OptionsUI
  End Function

  Public Function InstanceConflict(config1 As String, config2 As String, ByRef errorMsg As String) As Boolean Implements JHSoftware.SimpleDNS.Plugin.IPlugInBase.InstanceConflict
    Dim c1 = MyConfig.LoadFromXML(config1)
    Dim c2 = MyConfig.LoadFromXML(config2)
    If c1.Suffix = c2.Suffix Then
      errorMsg = "Another plug-in instance is using the same host name suffix"
      Return True
    End If
    If c1.BaseUrlInUse AndAlso c2.BaseUrlInUse AndAlso
       c1.BaseURL.ToLower = c2.BaseURL.ToLower Then
      errorMsg = "Another plug-in instance is using the same base URL for HTTP services"
      Return True
    End If
    If c1.UpMeHttpDynCom AndAlso c2.UpMeHttpDynCom Then
      errorMsg = "Only one plug-in instance can have the ""HTTP - Dyn.com URL format"" update method enabled"
      Return True
    End If
    Return False
  End Function

  Public Sub LoadConfig(config As String, instanceID As Guid, dataPath As String) Implements JHSoftware.SimpleDNS.Plugin.IPlugInBase.LoadConfig
    Cfg = MyConfig.LoadFromXML(config)

    HostUser = New Dictionary(Of DomName, MyConfig.User)
    HostUserWC = New Dictionary(Of DomName, MyConfig.User)
    Dim wc = DomName.Parse("*")
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

    UserByIP = New JHSortedList(Of SdnsIPv4, MyConfig.User)
  End Sub

  Public Function SaveState() As String Implements IState.SaveState
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

  Public Sub LoadState(state As String) Implements IState.LoadState
    UserByIP = New JHSortedList(Of SdnsIPv4, MyConfig.User)
    If state.Length = 0 Then Exit Sub
    Dim doc = New Xml.XmlDocument
    doc.LoadXml(state)
    Dim root = DirectCast(doc.GetElementsByTagName("config").Item(0), Xml.XmlElement)
    Dim user As MyConfig.User = Nothing
    For Each elem As Xml.XmlElement In root.GetElementsByTagName("User")
      If Cfg.Users.TryGetValue(DomName.Parse(elem.GetAttribute("ID")), user) Then
        If elem.HasAttribute("IP") Then user.CurIP = SdnsIPv4.Parse(elem.GetAttribute("IP"))
        user.CurTTL = elem.GetAttrInt("TTL", DefaultTTL)
        user.LastUpdate = elem.GetAttrDateTime("LastUpdate", #1/1/1970#)
        user.Offline = elem.GetAttrBool("Offline")
        If Not user.Disabled AndAlso
           Not user.Offline AndAlso
           user.CurIP IsNot Nothing Then UserByIP.Add(user.CurIP, user)
      End If
    Next
  End Sub

  Public Function LookupHost(name As DomName, ipv6 As Boolean, req As IRequestContext) As Task(Of LookupResult(Of SdnsIP)) Implements ILookupHost.LookupHost
    Return Task.FromResult(LookupHost2(name, ipv6, req))
  End Function
  Public Function LookupHost2(name As DomName, ipv6 As Boolean, req As IRequestContext) As LookupResult(Of SdnsIP)
    If ipv6 Then Return Nothing
    SyncLock Me
      Dim user = FindUserWithDomain(name)
      If user Is Nothing OrElse user.Disabled Then Return Nothing
      If user.Offline AndAlso user.OffLineIP IsNot Nothing Then
        Return New LookupResult(Of SdnsIP) With {.Value = user.OffLineIP, .TTL = DefaultTTL}
      Else
        Return New LookupResult(Of SdnsIP) With {.Value = user.CurIP, .TTL = user.CurTTL}
      End If
    End SyncLock
  End Function

  Private Async Function StartService() As Task Implements IPlugInBase.StartService
    IsStopping = False
    myRandom.NextBytes(GnuDIPKey)
    If Cfg.UpMeGnuDIP Then
      gdli = New Net.Sockets.Socket(Net.Sockets.AddressFamily.InterNetwork, Net.Sockets.SocketType.Stream, Net.Sockets.ProtocolType.Tcp)
      Try
        gdli.Bind(New Net.IPEndPoint(Net.IPAddress.Any, Cfg.GnuDIPPort))
        gdli.Listen(5)
        gdli.BeginAccept(AddressOf GDLI_CallBack, gdli)
        Host.LogLine("Listening for GnuDIP requests on TCP port " & Cfg.GnuDIPPort)
      Catch ex As Exception
        Host.LogLine("GnuDIP socket not started - Error: " & ex.Message)
      End Try
    End If
    If Cfg.BaseUrlInUse Or Cfg.UpMeHttpDynCom Then
      hli = New Net.HttpListener
      hli.IgnoreWriteExceptions = True
      hli.AuthenticationSchemes = Net.AuthenticationSchemes.Anonymous Or Net.AuthenticationSchemes.Basic
      Try
        If Cfg.BaseUrlInUse Then hli.Prefixes.Add(Cfg.BaseURL)
        If Cfg.UpMeHttpDynCom Then hli.Prefixes.Add("http://*/nic/")
        hli.Start()
        hli.BeginGetContext(AddressOf HLI_CallBack, hli)
      Catch ex As Exception
        Host.LogLine("HTTP listener not started - Error: " & ex.Message)
        Exit Function
      End Try
      If Cfg.BaseUrlInUse Then Host.LogLine("Listening for HTTP requests at " & Cfg.BaseURL)
      If Cfg.UpMeHttpDynCom Then Host.LogLine("Listening for HTTP requests at http://*/nic/")
    End If
  End Function

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

  Public Async Function GetTSIGKeySecret(keyName As DomName, algorithm As String) As Threading.Tasks.Task(Of Byte()) Implements JHSoftware.SimpleDNS.Plugin.ITSIGUpdateHost.GetTSIGKeySecret
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

  Public Async Function TSIGUpdateHost(fromIP As SdnsIP,
                                 keyName As DomName,
                                 algorithm As String,
                                 hostName As DomName,
                                 ipAddress As SdnsIP,
                                 ttl As Integer) As Threading.Tasks.Task(Of Boolean) Implements JHSoftware.SimpleDNS.Plugin.ITSIGUpdateHost.TSIGUpdateHost
    If Not Cfg.UpMeTsig Then Return False
    If ipAddress.IPVersion <> 4 Then Return False
    If keyName.SegmentCount <> 1 Then Return False
    If keyName & Cfg.Suffix <> hostName Then Return False
    If algorithm <> "HMAC-MD5" Then Return False
    SyncLock Me
      Dim user As MyConfig.User = Nothing
      If Not Cfg.Users.TryGetValue(keyName, user) Then Return False
      If user.Disabled Then Return False
      Dim unused = PerformUpdate(user, DirectCast(ipAddress, SdnsIPv4), ttl, "DNS TSIG")
      Return True
    End SyncLock
  End Function

#End Region

  Friend Sub SetUserOffline(user As MyConfig.User, UpMethod As String)
    If user.Disabled Then Exit Sub
    If user.OffLineIP IsNot Nothing Then
      Dim unused = PerformUpdate(user, user.OffLineIP, DynDNSPlugIn.DefaultTTL, UpMethod, True)
    Else
      Host.LogLine("User '" & user.ID.ToString & "' offline via " & UpMethod)
      If user.CurIP IsNot Nothing Then UserByIP.Remove(user.CurIP, user)
      user.Offline = True
      user.LastUpdate = DateTime.UtcNow
      SendViewUserMsg(-1, 2, user)
    End If
  End Sub

  Friend Async Function PerformUpdate(user As MyConfig.User,
                           newIP As SdnsIPv4,
                           newTTL As Integer,
                           UpMethod As String,
                           Optional OffLine As Boolean = False) As Task
    If user.Disabled Then Exit Function

    If user.CurIP IsNot Nothing Then UserByIP.Remove(user.CurIP, user)
    user.CurIP = newIP
    user.CurTTL = newTTL
    user.Offline = OffLine
    user.LastUpdate = DateTime.UtcNow
    If Not OffLine Then UserByIP.Add(newIP, user)

    If OffLine Then
      Host.LogLine("User '" & user.ID.ToString & "' offline via " & UpMethod)
    Else
      Host.LogLine("User '" & user.ID.ToString & "' now online at " & newIP.ToString & " via " & UpMethod)
    End If

    SendViewUserMsg(-1, 2, user)

    If Cfg.UpdateZones Then
      Dim recComment = "user '" & user.ID.ToString & "' via " & UpMethod
      Dim hn = user.ID & Cfg.Suffix
      Try
        Await Host.UpdateHost(hn, newIP, newTTL, recComment)
      Catch ex As Exception
        Host.LogLine("Failed to update A-record " & hn.ToString & " = " & newIP.ToString & " in local zone: " & ex.Message)
      End Try
      For Each hn In user.HostNames
        Try
          Await Host.UpdateHost(hn, newIP, newTTL, recComment)
        Catch ex As Exception
          Host.LogLine("Failed to update A-record " & hn.ToString & " = " & newIP.ToString & " in local zone: " & ex.Message)
        End Try
      Next
    End If

  End Function

  Sub GDLI_CallBack(ia As IAsyncResult)
    Try
      Dim sck As Net.Sockets.Socket = Nothing
      Try
        sck = gdli.EndAccept(ia)
      Catch ex As Net.Sockets.SocketException
        If IsStopping Then Exit Sub
        REM ignore 10054 here - confusing users
        If ex.SocketErrorCode <> Net.Sockets.SocketError.ConnectionReset Then
          Host.LogLine("Socket error accepting GnuDIP TCP connection: " & ex.ErrorCode & " " & ex.Message)
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
      Host.AsyncError(ex)
    End Try
  End Sub

  Private Sub HLI_CallBack(ia As IAsyncResult)
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
      Host.AsyncError(ex)
    End Try
  End Sub

  Public Function GetViewUI() As JHSoftware.SimpleDNS.Plugin.ViewUI Implements JHSoftware.SimpleDNS.Plugin.IViewUI.GetViewUI
    Return New ViewUI
  End Function

  Public Sub MsgFromViewUI(connID As Integer, msg() As Byte) Implements JHSoftware.SimpleDNS.Plugin.IViewUI.MsgFromViewUI
    Select Case msg(0)
      Case 1 ' get list 
        For Each user In Cfg.Users.Values
          If Not user.Disabled Then SendViewUserMsg(connID, 1, user)
        Next
      Case 2 'set user offline
        Dim userID = DomName.Parse(System.Text.Encoding.ASCII.GetString(msg, 1, msg.Length - 1))
        Dim user As MyConfig.User = Nothing
        If Not Cfg.Users.TryGetValue(userID, user) Then Exit Sub
        SetUserOffline(user, "GUI")
    End Select
  End Sub

  Private Sub SendViewUserMsg(connID As Integer, cmd As Byte, user As MyConfig.User)
    Dim baID = user.ID.GetBytes()
    Dim ba(baID.Length + 8) As Byte
    ba(0) = cmd
    baID.CopyTo(ba, 1)
    Dim p = baID.Length ' 1 + baID.Length -1 
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

  Public Async Function ListsIPAddress(ip As SdnsIP) As Threading.Tasks.Task(Of Boolean) Implements JHSoftware.SimpleDNS.Plugin.IListsIPAddress.ListsIPAddress
    SyncLock Me
      Return FindActiveUserWithIP(ip) IsNot Nothing
    End SyncLock
  End Function

  Public Async Function ListsDomainName(domain As DomName) As Threading.Tasks.Task(Of Boolean) Implements JHSoftware.SimpleDNS.Plugin.IListsDomainName.ListsDomainName
    SyncLock Me
      Dim user = FindUserWithDomain(domain)
      If user Is Nothing OrElse user.Disabled OrElse user.Offline Then Return False
      Return True
    End SyncLock
  End Function


  Public Function QuestionList() As JHSoftware.SimpleDNS.Plugin.IQuestions.QuestionInfo() Implements JHSoftware.SimpleDNS.Plugin.IQuestions.QuestionList
    Dim rv(0) As IQuestions.QuestionInfo
    rv(0).Question = "Notes of DynDNS user, who sent DNS request, contain text"
    rv(0).ValuePrompt = "Notes of DynDNS user, who sent DNS request, contain"
    Return rv
  End Function

  Public Async Function QuestionAsk(id As Integer, value As String, req As IRequestContext) As Threading.Tasks.Task(Of Boolean) Implements JHSoftware.SimpleDNS.Plugin.IQuestions.QuestionAsk
    SyncLock Me
      Dim user = FindActiveUserWithIP(req.FromIP)
      If user Is Nothing Then Return False
      Return (user.Notes.IndexOf(value, StringComparison.CurrentCultureIgnoreCase) >= 0)
    End SyncLock
  End Function

  Private Function FindActiveUserWithIP(ip As SdnsIP) As MyConfig.User
    If ip.IPVersion <> 4 Then Return Nothing
    Dim i = UserByIP.IndexOf(DirectCast(ip, SdnsIPv4))
    If i < 0 Then Return Nothing
    Do
      With UserByIP(i)
        If Not .Disabled AndAlso Not .Offline Then Return UserByIP(i)
      End With
      i += 1
    Loop While i < UserByIP.Count AndAlso UserByIP(i).CurIP = DirectCast(ip, SdnsIPv4)
    Return Nothing
  End Function

  Private Function FindUserWithDomain(dom As DomName) As MyConfig.User
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

