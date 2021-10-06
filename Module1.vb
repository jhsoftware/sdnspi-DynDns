Module Module1

  Friend md5 As New System.Security.Cryptography.MD5CryptoServiceProvider
  Friend Const GnuDIPSaltChars As String = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789"
  Friend myRandom As New Random

  Friend Enum GnuDIPResult
    Success = 0
    InvalidLogin = 1
    SuccessOffline = 2
  End Enum

  Friend Function BytesToHex(ba As Byte()) As String
    Dim rv(ba.Length * 2 - 1) As Char
    For i = 0 To ba.Length - 1
      rv(i << 1) = "0123456789abcdef"(ba(i) >> 4)
      rv((i << 1) + 1) = "0123456789abcdef"(ba(i) And 15)
    Next
    Return rv
  End Function

  Friend Function MakeGnuDIPSalt() As String
    Dim rv(9) As Char
    For i = 0 To 9
      rv(i) = GnuDIPSaltChars(myRandom.Next(0, GnuDIPSaltChars.Length))
    Next
    Return rv
  End Function

  Friend Function MakeGnuDIPPass(userPW As String, salt As String) As String
    '    Digest the user's password using the MD5 digest message digest algorithm. 
    'Convert the digest value (which is a binary value) to its hexadecimal character 
    'string representation (characters 0-9 and lower case a-f). 
    Dim x = BytesToHex(md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(userPW)))

    'Append a period (".") and the salt value to create a longer character string. 
    x &= "." & salt

    'Digest this longer character string and convert it to its hexadecimal character representation. 
    Return BytesToHex(md5.ComputeHash(System.Text.Encoding.ASCII.GetBytes(x)))
  End Function

  Friend Function IPisPrivate(ip As SdnsIPv4) As Boolean
    Dim ba = ip.GetBytes
    If ba(0) = 10 Then Return True
    If ba(0) = 172 AndAlso (ba(1) >= 16 And ba(1) < 32) Then Return True
    If ba(0) = 192 AndAlso ba(1) = 168 Then Return True
    If ba(0) = 169 AndAlso ba(1) = 254 Then Return True
    If ip = SdnsIPv4.Loopback Then Return True
    Return False
  End Function

  Friend Function TryIPFromDom(dom As DomName, ByRef IP As UInt32) As Boolean
    If dom.SegmentCount < 5 Then Return False
    Dim ba = dom.GetBytes
    Dim p = 0
    Dim segLen, segVal As Integer
    Dim baIP(3) As Byte
    For seg = 0 To 3
      segLen = ba(p)
      If segLen < 1 Or segLen > 3 Then Return False
      p += 1
      segVal = 0
      For i = 1 To segLen
        If ba(p) < 48 Or ba(p) > 57 Then Return False
        segVal = segVal * 10 + ba(p) - 48
        p += 1
      Next
      If segVal > 255 Then Return False
      baIP(3 - seg) = CByte(segVal)
    Next
    IP = (CUInt(baIP(0)) << 24) Or (CUInt(baIP(1)) << 16) Or (CUInt(baIP(2)) << 8) Or CUInt(baIP(3))
    Return True
  End Function


End Module
