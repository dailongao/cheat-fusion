Option Strict Off
Option Explicit On
Friend Class Form5
	Inherits System.Windows.Forms.Form
	
	Private Sub cmdOK_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdOK.Click
		
		If cboTextFormat.SelectedIndex = 0 Then
			g_TextFormat = 1
		Else
			g_TextFormat = 2
		End If
		
		If chkHideRepeat.CheckState = 1 Then
			g_hide_repeat = True
		Else
			g_hide_repeat = False
		End If
		
		
		g_len_id = CStr(Val(Text1.Text))
		g_len_j = CStr(Val(Text2.Text))
		g_len_u = CStr(Val(Text3.Text))
		g_len_c = CStr(Val(Text4.Text))
		
		Me.Close()
		
	End Sub
	
	Private Sub Form5_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
		
		Dim c As System.Windows.Forms.Control
		Me.BackColor = Form2.Grid1.BackColor
		For	Each c In Me.Controls
			c.BackColor = Me.BackColor
		Next c
		
		
		cboTextFormat.Items.Add("格式1 - Agemo用")
		cboTextFormat.Items.Add("格式2 - 地址,长度,文字")
		If g_TextFormat = 1 Then
			cboTextFormat.SelectedIndex = 0
		Else
			cboTextFormat.SelectedIndex = 1
		End If
		
		If g_hide_repeat Then
			chkHideRepeat.CheckState = System.Windows.Forms.CheckState.Checked
		Else
			chkHideRepeat.CheckState = System.Windows.Forms.CheckState.Unchecked
		End If
		
		Text1.Text = g_len_id
		Text2.Text = g_len_j
		Text3.Text = g_len_u
		Text4.Text = g_len_c
		
	End Sub
End Class