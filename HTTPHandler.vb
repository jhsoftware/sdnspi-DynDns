Imports System.Linq

Friend Class HTTPHandler
  Friend ctx As Net.HttpListenerContext
  Friend plugin As DynDNSPlugIn

  Friend Sub Process()
    If Not {"GET", "POST", "PUT"}.Contains(ctx.Request.HttpMethod) Then
      ctx.Response.StatusCode = 405
      ctx.Response.Close()
      Exit Sub
    End If

    ctx.Response.AddHeader("Cache-Control", "private")
    Dim x = ctx.Request.Url.AbsolutePath.ToLower
    Select Case x.Substring(x.LastIndexOf("/"c) + 1)
      Case "myip"
        ProcMyIP()
      Case "ddns"
        ProcDDNS()
      Case "gnudip"
        ProcGnuDIP()
      Case Else
        If x.EndsWith("/gnudip/cgi-bin/gdipupdt.cgi") Then
          ProcGnuDIP()
        ElseIf x = "/nic/update" Then
          ProcDynCom()
        Else
          Send404()
        End If
    End Select
  End Sub

  Private Sub ProcMyIP()
    If plugin.Cfg.RemoteDetect Then
      ctx.Response.ContentType = "text/plain"
      ctx.Response.Close(System.Text.Encoding.ASCII.GetBytes(DetectIP.ToString), False)
    Else
      ctx.Response.StatusCode = 404
      ctx.Response.Close(System.Text.Encoding.ASCII.GetBytes("Not found"), False)
    End If
  End Sub

  Private Sub ProcDDNS()
    Dim userID As DomName = Nothing
    Dim user As MyConfig.User = Nothing
    Dim ipAddr As SdnsIPv4 = Nothing
    Dim ttl As Integer
    Dim x As String
    Dim upMethod As String

    If String.IsNullOrEmpty(ctx.Request.QueryString("user")) Then
      If Not plugin.Cfg.UpMeHttpBasic Then
        If plugin.Cfg.UpMeHttpUrl Then
          SendError("Missing 'user' value")
        Else
          SendError("URL/Basic HTTP authentication update methods are not enabled")
        End If
        Exit Sub
      End If

      REM no user param - must be Basic Auth
      upMethod = "HTTP Basic Auth"
      If ctx.User Is Nothing OrElse _
         Not TypeOf ctx.User.Identity Is Net.HttpListenerBasicIdentity Then Send401() : Exit Sub

      With DirectCast(ctx.User.Identity, Net.HttpListenerBasicIdentity)
        If Not DomName.TryParse(.Name, userID) OrElse
           Not plugin.Cfg.Users.TryGetValue(userID, user) OrElse
           user.Disabled OrElse
           user.Password <> .Password Then Send401() : Exit Sub
      End With

    Else
      upMethod = "HTTP URL Auth"
      If Not plugin.Cfg.UpMeHttpUrl Then SendError("URL authentication update method is not enabled") : Exit Sub
      x = ctx.Request.QueryString("user")
      If String.IsNullOrEmpty(x) OrElse x.Trim.Length = 0 Then SendError("No user ID specified") : Exit Sub
      If Not DomName.TryParse(x.Trim, userID) OrElse
         userID.SegmentCount <> 1 Then SendError("Invalid user ID specified") : Exit Sub
      If Not plugin.Cfg.Users.TryGetValue(userID, user) Then SendError("Unknown user ID") : Exit Sub
      If user.Disabled Then SendError("User ID is disabled") : Exit Sub

      x = ctx.Request.QueryString("pw")
      If String.IsNullOrEmpty(x) Then SendError("No password specified") : Exit Sub
      If user.Password <> x Then SendError("Incorrect password") : Exit Sub
    End If

    x = ctx.Request.QueryString("ip")
    If Not String.IsNullOrEmpty(x) Then
      If Not SdnsIPv4.TryParse(x.Trim, ipAddr) Then SendError("Invalid IP address") : Exit Sub
    Else
      ipAddr = DetectIP()
    End If

    x = ctx.Request.QueryString("ttl")
    If Not String.IsNullOrEmpty(x) Then
      If Not Integer.TryParse(x, ttl) OrElse _
         ttl < 0 Then SendError("Invalid TTL value") : Exit Sub
    Else
      ttl = DynDNSPlugIn.DefaultTTL
    End If

    x = ctx.Request.QueryString("offline")
    If Not String.IsNullOrEmpty(x) AndAlso x.ToUpper()(0) = "Y"c Then
      plugin.SetUserOffline(user, upMethod)
      SendOK("Offline")
    Else
      Dim unused = plugin.PerformUpdate(user, ipAddr, ttl, upMethod)
      SendOK(ipAddr.ToString)
    End If
  End Sub

  Private Sub ProcDynCom()
    If Not plugin.Cfg.UpMeHttpDynCom Then SendError("Dyn.com update method is not enabled") : Exit Sub

    Dim userID As DomName = Nothing
    Dim user As MyConfig.User = Nothing
    If ctx.User Is Nothing OrElse Not TypeOf ctx.User.Identity Is Net.HttpListenerBasicIdentity Then Send401() : Exit Sub

    With DirectCast(ctx.User.Identity, Net.HttpListenerBasicIdentity)
      If Not DomName.TryParse(.Name, userID) OrElse
             Not plugin.Cfg.Users.TryGetValue(userID, user) OrElse
             user.Disabled OrElse
             user.Password <> .Password Then Send401() : Exit Sub
    End With

    Dim ipAddr As SdnsIPv4 = Nothing
    Dim x As String

    x = ctx.Request.QueryString("myip")
    If Not String.IsNullOrEmpty(x) Then
      'When invalid IP is specified dyn.com uses client IP (tested 10 jan 2017) 
      If Not SdnsIPv4.TryParse(x.Trim, ipAddr) Then ipAddr = DetectIP()
    Else
      ipAddr = DetectIP()
    End If

    x = ctx.Request.QueryString("offline")
    If Not String.IsNullOrEmpty(x) AndAlso x.ToUpper()(0) = "Y"c Then
      plugin.SetUserOffline(user, "HTTP - Dyn.com URL format")
    Else
      Dim unused = plugin.PerformUpdate(user, ipAddr, DynDNSPlugIn.DefaultTTL, "HTTP - Dyn.com URL format")
    End If

    ctx.Response.ContentType = "text/plain"
    ctx.Response.Close(System.Text.Encoding.ASCII.GetBytes("good " & ipAddr.ToString), False)
  End Sub


  Private Sub ProcGnuDIP()
    If Not plugin.Cfg.UpMeHttpGD Then SendError("GNUDIP authentication update method is not enabled") : Exit Sub

    If ctx.Request.HttpMethod <> "GET" Then SendError("GnuDIP authentication requests must use GET") : Exit Sub

    If String.IsNullOrEmpty(ctx.Request.Url.Query) OrElse ctx.Request.Url.Query = "?" Then
      REM request for salt
      SendGnuDIPSalt()
      Exit Sub
    End If

    Dim salt = ctx.Request.QueryString("salt")
    If String.IsNullOrEmpty(salt) OrElse salt.Length <> 10 Then _
          SendGnuDIPResult(GnuDIPResult.InvalidLogin, "", "Invalid 'salt' value") : Exit Sub

    Dim timeStr = ctx.Request.QueryString("time")
    Dim timeIntNow = CInt(DateTime.UtcNow.Subtract(#1/1/2000#).TotalSeconds)
    Dim timeInt As Integer
    If String.IsNullOrEmpty(timeStr) OrElse _
       Not Integer.TryParse(timeStr, timeInt) OrElse _
       timeInt > timeIntNow Then _
          SendGnuDIPResult(GnuDIPResult.InvalidLogin, "", "Invalid 'time' value") : Exit Sub
    If timeInt < timeIntNow - 60 Then _
      SendGnuDIPResult(GnuDIPResult.InvalidLogin, "", "Salt has expired") : Exit Sub

    Dim sign = ctx.Request.QueryString("sign")
    If String.IsNullOrEmpty(sign) Then _
          SendGnuDIPResult(GnuDIPResult.InvalidLogin, "", "Missing 'sign' value") : Exit Sub
    If sign <> GnuDIPSignSaltTime(salt, timeStr) Then _
          SendGnuDIPResult(GnuDIPResult.InvalidLogin, "", "Incorrect 'sign' value") : Exit Sub

    Dim userID As DomName = Nothing
    Dim user As MyConfig.User = Nothing
    Dim x = ctx.Request.QueryString("user")
    If String.IsNullOrEmpty(x) OrElse x.Trim.Length = 0 OrElse
       Not DomName.TryParse(x.Trim, userID) OrElse
       userID.SegmentCount <> 1 Then _
           SendGnuDIPResult(GnuDIPResult.InvalidLogin, "", "Invalid 'user' value") : Exit Sub
    If Not plugin.Cfg.Users.TryGetValue(userID, user) Then _
           SendGnuDIPResult(GnuDIPResult.InvalidLogin, "", "Unknown user ID") : Exit Sub
    If user.Disabled Then _
           SendGnuDIPResult(GnuDIPResult.InvalidLogin, "", "User ID is disabled") : Exit Sub

    x = ctx.Request.QueryString("pass")
    If String.IsNullOrEmpty(x) Or x.Trim.Length = 0 Then _
         SendGnuDIPResult(GnuDIPResult.InvalidLogin, "", "Missing 'pass' value") : Exit Sub
    If x <> MakeGnuDIPPass(user.Password, salt) Then _
         SendGnuDIPResult(GnuDIPResult.InvalidLogin, "", "Incorrect 'pass' value") : Exit Sub

    x = ctx.Request.QueryString("domn")
    If Not String.IsNullOrEmpty(x) Then
      Dim d As DomName = Nothing
      If Not DomName.TryParse(x, d) Then _
            SendGnuDIPResult(GnuDIPResult.InvalidLogin, "", "Invalid 'domn' value") : Exit Sub
      If userID & plugin.Cfg.Suffix <> d Then _
            SendGnuDIPResult(GnuDIPResult.InvalidLogin, "", "User not allowed to update host name in 'domn' value") : Exit Sub
    End If

    Dim reqc = ctx.Request.QueryString("reqc")
    '"0" - register the address passed with this request 
    '"1" - go offline 
    '"2" - register the address you see me at, and pass it back to me 
    If String.IsNullOrEmpty(reqc) OrElse (reqc <> "0" And reqc <> "1" And reqc <> "2") Then _
           SendGnuDIPResult(GnuDIPResult.InvalidLogin, "", "Invalid 'reqc' value") : Exit Sub

    Dim ipAddr As SdnsIPv4 = Nothing
    If reqc = "0" Then
      'the IP address to be registered, if the request code is "0" ("addr=") 
      x = ctx.Request.QueryString("addr")
      If String.IsNullOrEmpty(x) OrElse
         Not SdnsIPv4.TryParse(x, ipAddr) Then _
            SendGnuDIPResult(GnuDIPResult.InvalidLogin, "", "Invalid 'addr' value") : Exit Sub
    ElseIf reqc = "2" Then
      ipAddr = DetectIP()
    End If

    If reqc = "1" Then
      plugin.SetUserOffline(user, "HTTP GnuDIP Auth")
      SendGnuDIPResult(GnuDIPResult.SuccessOffline, "", "OK")
    Else
      Dim unused = plugin.PerformUpdate(user, ipAddr, DynDNSPlugIn.DefaultTTL, "HTTP GnuDIP Auth")
      SendGnuDIPResult(GnuDIPResult.Success, If(reqc = "2", ipAddr.ToString, ""), "OK")
    End If
  End Sub

  Private Function GnuDIPSignSaltTime(salt As String, timeStr As String) As String
    Dim ba(salt.Length + timeStr.Length + plugin.GnuDIPKey.Length - 1) As Byte
    plugin.GnuDIPKey.CopyTo(ba, 0)
    System.Text.Encoding.ASCII.GetBytes(salt & timeStr).CopyTo(ba, plugin.GnuDIPKey.Length)
    Return BytesToHex(md5.ComputeHash(ba))
  End Function

  Sub SendGnuDIPSalt()
    Dim salt = MakeGnuDIPSalt()
    Dim timeStr = CInt(DateTime.UtcNow.Subtract(#1/1/2000#).TotalSeconds).ToString
    Dim x = "<html>" & vbCrLf & _
            "<head>" & vbCrLf & _
            "<meta name=""salt"" content=""" & salt & """>" & vbCrLf & _
            "<meta name=""time"" content=""" & timeStr & """>" & vbCrLf & _
            "<meta name=""sign"" content=""" & GnuDIPSignSaltTime(salt, timeStr) & """>" & vbCrLf & _
            "<title>GnuDIP Salt</title>" & vbCrLf & _
            "</head>" & vbCrLf & _
            "<body>" & vbCrLf & _
            "<h1>GnuDIP salt generated</h1>" & vbCrLf & _
            "</body>" & vbCrLf & _
            "</html>" & vbCrLf
    ctx.Response.Close(System.Text.Encoding.ASCII.GetBytes(x), False)
  End Sub

  Private Sub SendGnuDIPResult(result As GnuDIPResult, addr As String, msg As String)
    Dim x = "<html>" & vbCrLf &
            "<head>" & vbCrLf &
            "<meta name=""retc"" content=""" & CInt(result) & """>" & vbCrLf
    If Not String.IsNullOrEmpty(addr) Then x &= "<meta name=""addr"" content=""" & addr & """>" & vbCrLf
    x &= "<title>GnuDIP Result</title>" & vbCrLf &
            "</head>" & vbCrLf &
            "<body>" & vbCrLf &
            "<h1>GnuDIP result: " & CInt(result) & "</h1>" & vbCrLf
    If Not String.IsNullOrEmpty(msg) Then x &= "<p>" & msg & "</p>" & vbCrLf
    x &= "</body>" & vbCrLf &
            "</html>" & vbCrLf
    ctx.Response.Close(System.Text.Encoding.ASCII.GetBytes(x), False)
  End Sub

  Sub SendError(errMsg As String)
    ctx.Request.Headers.Add("X-DynDNS-Result", "FAIL")
    Dim x = "<html>" & vbCrLf &
            "<head>" & vbCrLf &
            "<meta name=""DynDNS-Result"" content=""FAIL"">" & vbCrLf &
            "<title>DynDNS update ERROR</title>" & vbCrLf &
            "</head>" & vbCrLf &
            "<body>" & vbCrLf &
            "<h1>ERROR: " & errMsg & "</h1>" & vbCrLf &
            "</body>" & vbCrLf &
            "</html>" & vbCrLf
    ctx.Response.Close(System.Text.Encoding.ASCII.GetBytes(x), False)
  End Sub

  Sub SendOK(msg As String)
    ctx.Request.Headers.Add("X-DynDNS-Result", "OK")
    Dim x = "<html>" & vbCrLf &
            "<head>" & vbCrLf &
            "<meta name=""DynDNS-Result"" content=""OK"">" & vbCrLf &
            "<title>DynDNS update OK</title>" & vbCrLf &
            "</head>" & vbCrLf &
            "<body>" & vbCrLf &
            "<h1>OK: " & msg & "</h1>" & vbCrLf &
            "</body>" & vbCrLf &
            "</html>" & vbCrLf
    ctx.Response.Close(System.Text.Encoding.ASCII.GetBytes(x), False)
  End Sub

  Sub Send404()
    ctx.Response.StatusCode = 404
    Dim x = "<html><head><title>Document not found</title></head><body><h1>Document not found</h1></body></html>" & vbCrLf
    Dim ba = System.Text.Encoding.ASCII.GetBytes(x)
    ctx.Response.Close(ba, False)
  End Sub

  Sub Send401()
    ctx.Response.AddHeader("WWW-Authenticate", "Basic realm=""DynDNS Service""")
    ctx.Response.StatusCode = 401
    Dim ba = System.Text.Encoding.ASCII.GetBytes("<html><head><title>Unauthorized</title></head><body>Unauthorized</body></html>" & vbCrLf)
    ctx.Response.Close(ba, False)
  End Sub

  Private Function DetectIP() As SdnsIPv4
    Dim rv As SdnsIPv4 = Nothing

    Dim x = ctx.Request.Headers("X-Forwarded-For")
    If Not String.IsNullOrEmpty(x) AndAlso
       SdnsIPv4.TryParse(x, rv) AndAlso
       rv.IPVersion = 4 AndAlso
       Not IPisPrivate(rv) Then Return rv

    x = ctx.Request.Headers("Client-IP")
    If Not String.IsNullOrEmpty(x) AndAlso
       SdnsIPv4.TryParse(x, rv) AndAlso
       Not IPisPrivate(rv) Then Return rv

    If ctx.Request.RemoteEndPoint.Address.AddressFamily = Net.Sockets.AddressFamily.InterNetwork Then
      Return New SdnsIPv4(ctx.Request.RemoteEndPoint.Address.GetAddressBytes)
    End If

    Return SdnsIPv4.Loopback
  End Function
End Class
