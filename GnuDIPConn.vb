Friend Class GnuDIPConn
  Friend plugin As DynDNSPlugIn
  Friend sock As Net.Sockets.Socket

  Private salt As String

  Private rBuf(255) As Byte

  Friend Sub Process()
    REM calling code has try/catch

    sock.ReceiveTimeout = 60000
    salt = MakeGnuDIPSalt()
    Dim ba = System.Text.Encoding.ASCII.GetBytes(salt & vbCrLf)
    Try
      sock.BeginSend(ba, 0, ba.Length, Net.Sockets.SocketFlags.None, AddressOf Send_CallBack, sock)
    Catch
      Close()
    End Try
  End Sub

  Private Sub Send_CallBack(ia As IAsyncResult)
    Try

      Try
        sock.EndSend(ia)
      Catch
        Close()
        Exit Sub
      End Try

      Dim len As Integer
      Dim rBufPos As Integer = 0
      Do
        Try
          len = sock.Receive(rBuf, rBufPos, rBuf.Length - rBufPos, Net.Sockets.SocketFlags.None)
        Catch
          Close()
          Exit Sub
        End Try
        If len = 0 Then Close() : Exit Sub
        rBufPos += len
        If rBuf(rBufPos - 1) = 10 Then Exit Do
        If rBufPos = rBuf.Length Then Close() : Exit Sub
      Loop

      Dim x = System.Text.Encoding.ASCII.GetString(rBuf, 0, rBufPos).TrimEnd
      Dim i As Integer

      SyncLock plugin

        REM user ID
        i = x.IndexOf(":")
        If i <= 0 Then Respond(GnuDIPResult.InvalidLogin) : Exit Sub
        Dim y = x.Substring(0, i)
        x = x.Substring(i + 1)
        Dim UserID As DomName = Nothing
        If Not DomName.TryParse(y, UserID) Then Respond(GnuDIPResult.InvalidLogin) : Exit Sub
        Dim User As MyConfig.User = Nothing
        If Not plugin.Cfg.Users.TryGetValue(UserID, User) Then Respond(GnuDIPResult.InvalidLogin) : Exit Sub
        If User.Disabled Then Respond(GnuDIPResult.InvalidLogin) : Exit Sub

        REM password hash
        i = x.IndexOf(":")
        If i <= 0 Then Respond(GnuDIPResult.InvalidLogin) : Exit Sub
        y = x.Substring(0, i)
        x = x.Substring(i + 1)
        If MakeGnuDIPPass(User.Password, salt) <> y Then Respond(GnuDIPResult.InvalidLogin) : Exit Sub

        REM host name
        i = x.IndexOf(":")
        If i <= 0 Then Respond(GnuDIPResult.InvalidLogin) : Exit Sub
        y = x.Substring(0, i)
        x = x.Substring(i + 1)
        Dim hn As DomName = Nothing
        If Not DomName.TryParse(y, hn) Then Respond(GnuDIPResult.InvalidLogin) : Exit Sub
        If hn <> UserID & plugin.Cfg.Suffix Then Respond(GnuDIPResult.InvalidLogin) : Exit Sub

        REM command
        i = x.IndexOf(":")
        Dim cmd, addrStr As String
        If i < 0 Then
          cmd = x
          addrStr = ""
        Else
          cmd = x.Substring(0, i)
          addrStr = x.Substring(i + 1)
        End If
        If cmd <> "0" And cmd <> "1" And cmd <> "2" Then Respond(GnuDIPResult.InvalidLogin) : Exit Sub

        Dim ip As SdnsIPv4 = Nothing
        If cmd = "0" AndAlso addrStr.Length > 0 Then
          If Not SdnsIPv4.TryParse(addrStr, ip) Then Respond(GnuDIPResult.InvalidLogin) : Exit Sub
        Else
          With DirectCast(sock.RemoteEndPoint, Net.IPEndPoint).Address
            If .AddressFamily <> Net.Sockets.AddressFamily.InterNetwork Then Respond(GnuDIPResult.InvalidLogin) : Exit Sub
            ip = New SdnsIPv4(.GetAddressBytes)
          End With
        End If

        If cmd = "1" Then
          plugin.SetUserOffline(User, "GnuDIP")
          Respond(GnuDIPResult.SuccessOffline)
        Else
          Dim unused = plugin.PerformUpdate(User, ip, DynDNSPlugIn.DefaultTTL, "GnuDIP")
          Respond(GnuDIPResult.Success, If(cmd = "2", ip.ToString, ""))
        End If

      End SyncLock

    Catch ex As Exception
      plugin.Host.AsyncError(ex)
    End Try
  End Sub

  Sub Respond(result As GnuDIPResult, Optional addr As String = "")
    Try
      Dim ba = System.Text.Encoding.ASCII.GetBytes(CInt(result).ToString & If(addr.Length > 0, ":" & addr, "") & vbCrLf)
      Try
        sock.Send(ba, ba.Length, Net.Sockets.SocketFlags.None)
      Catch ex As Exception
        Close()
        Exit Sub
      End Try

      Dim TimeOut As DateTime = DateTime.UtcNow.AddMinutes(1)
      Do
        Try
          If sock.Receive(rBuf, 0, rBuf.Length, Net.Sockets.SocketFlags.None) = 0 Then Exit Do
        Catch
          Close()
          Exit Sub
        End Try
      Loop While TimeOut > DateTime.UtcNow
      Close()


    Catch ex As Exception
      plugin.Host.AsyncError(ex)
    End Try
  End Sub

  Friend Sub Close()
    If sock Is Nothing Then Exit Sub
    Try
      sock.Close()
    Catch
    End Try
    sock = Nothing
  End Sub
End Class
