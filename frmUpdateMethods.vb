Imports System.Windows.Forms

Public Class frmUpdateMethods

  Friend Sub LoadConfig(ByVal cfg As MyConfig)
    chkTSIG.Checked = cfg.UpMeTsig
    chkGnudipN.Checked = cfg.UpMeGnuDIP
    numPort.Value = cfg.GnuDIPPort
    chkURL.Checked = cfg.UpMeHttpUrl
    chkBasic.Checked = cfg.UpMeHttpBasic
    chkGnudipH.Checked = cfg.UpMeHttpGD
    chkDyncom.Checked = cfg.UpMeHttpDynCom

    chkRDetect.Checked = cfg.RemoteDetect

    txtBaseUrl.Text = cfg.BaseURL
  End Sub

  Friend Sub SaveToConfig(ByVal cfg As MyConfig)
    cfg.UpMeTsig = chkTSIG.Checked
    cfg.UpMeGnuDIP = chkGnudipN.Checked
    cfg.GnuDIPPort = CInt(numPort.Value)
    cfg.UpMeHttpUrl = chkURL.Checked
    cfg.UpMeHttpBasic = chkBasic.Checked
    cfg.UpMeHttpGD = chkGnudipH.Checked
    cfg.UpMeHttpDynCom = chkDyncom.Checked

    cfg.RemoteDetect = chkRDetect.Checked

    cfg.BaseURL = txtBaseUrl.Text.Trim
  End Sub

  Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
    If txtBaseUrl.Enabled AndAlso _
       Not ValidateBaseUrl() Then Exit Sub

    If Not chkTSIG.Checked And
       Not chkGnudipN.Checked And
       Not chkURL.Checked And
       Not chkBasic.Checked And
       Not chkGnudipH.Checked And
       Not chkDyncom.Checked Then
      MessageBox.Show("At least one update method must be selected", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
      Exit Sub
    End If

    If txtBaseUrl.Enabled Then
      If Not Net.HttpListener.IsSupported Then
        MessageBox.Show("The HTTP services are not supported on this Windows version" & vbCrLf & _
                        "(requires Windows XP SP2 / Windows Server 2003 or later).", _
                        Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Exit Sub
      End If
    End If

    Me.DialogResult = System.Windows.Forms.DialogResult.OK
    Me.Close()
  End Sub

  Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
    Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
    Me.Close()
  End Sub

  Private Sub chkURL_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkURL.CheckedChanged, chkBasic.CheckedChanged, chkGnudipH.CheckedChanged, chkGnudipN.CheckedChanged, chkRDetect.CheckedChanged, chkDyncom.CheckedChanged
    numPort.Enabled = chkGnudipN.Checked
    txtBaseUrl.Enabled = (chkBasic.Checked Or chkURL.Checked Or chkGnudipH.Checked Or chkRDetect.Checked)
    btnShowFull.Enabled = (chkBasic.Checked Or chkURL.Checked Or chkGnudipH.Checked Or chkRDetect.Checked Or chkDyncom.Checked)
  End Sub

  Private Sub btnShowFull_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnShowFull.Click
    If txtBaseUrl.Enabled AndAlso Not ValidateBaseUrl() Then Exit Sub

    Dim frm = New frmShowURLs
    Dim x = txtBaseUrl.Text.Trim
    If chkURL.Checked Then
      frm.txtURL1.Text = x & "ddns?user=<user-id>&pw=<password>&ip=<ip-address>&ttl=<ttl>&offline=<offline>"
    Else
      frm.Label1.Enabled = False
      frm.txtURL1.Enabled = False
      frm.txtURL1.BackColor = Drawing.SystemColors.Control
      frm.btnCopy1.Enabled = False
    End If

    If chkBasic.Checked Then
      frm.txtURL2.Text = x & "ddns?ip=<ip-address>&ttl=<ttl>&offline=<offline>"
    Else
      frm.Label2.Enabled = False
      frm.txtURL2.Enabled = False
      frm.txtURL2.BackColor = Drawing.SystemColors.Control
      frm.btnCopy2.Enabled = False
    End If

    If chkGnudipH.Checked Then
      frm.txtURL3.Text = x & "gnudip"
    Else
      frm.Label3.Enabled = False
      frm.txtURL3.Enabled = False
      frm.txtURL3.BackColor = Drawing.SystemColors.Control
      frm.btnCopy3.Enabled = False
    End If

    If chkDyncom.Checked Then
      frm.txtURL4.Text = "http://*/nic/update?myip=<ip-address>&offline=<offline>"
    Else
      frm.Label4.Enabled = False
      frm.txtURL4.Enabled = False
      frm.txtURL4.BackColor = Drawing.SystemColors.Control
      frm.btnCopy4.Enabled = False
    End If

    If chkRDetect.Checked Then
      frm.txtURL5.Text = x & "myip"
    Else
      frm.Label5.Enabled = False
      frm.txtURL5.Enabled = False
      frm.txtURL5.BackColor = Drawing.SystemColors.Control
      frm.btnCopy5.Enabled = False
    End If

    frm.ShowDialog()
  End Sub

  Private Function ValidateBaseUrl() As Boolean
    Dim x = txtBaseUrl.Text.Trim
    If Not (x.StartsWith("http://") OrElse x.StartsWith("https://")) Then
      MessageBox.Show("Base URL must start with http:// or https://", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
      txtBaseUrl.Focus()
      Return False
    End If
    If x.Length < 8 Then
      MessageBox.Show("Base URL is too short", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
      txtBaseUrl.Focus()
      Return False
    End If
    If Not x.EndsWith("/") Then
      x &= "/"
      txtBaseUrl.Text = x
    End If
    Try
      Dim u = New Uri(x)
    Catch ex As Exception
      MessageBox.Show("Invalid Base URL", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
      txtBaseUrl.Focus()
      Return False
    End Try
    Return True
  End Function

End Class
