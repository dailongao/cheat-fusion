Option Strict Off
Option Explicit On
Friend Class Form3
	Inherits System.Windows.Forms.Form
	
	Public CurrentUser As String
	Public CurrentMemo As String
	Public CurrentID As String
	Public CurrentGridNo As Integer
	Public IsCancel As Boolean
	
	Public Sub ApplyData(ByRef nNo As Integer, ByRef sID As String, ByRef strUser As String, ByRef strMemo As String)
		
		IsCancel = True
		CurrentGridNo = nNo
		txtUser.Text = strUser
		txtMemo.Text = strMemo
		CurrentID = sID
		VB6.ShowForm(Me, 1, Form2)
	End Sub
	
	Private Sub cmdOK_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdOK.Click
		CurrentUser = txtUser.Text
		CurrentMemo = txtMemo.Text
		
		IsCancel = False
		Me.Hide()
	End Sub
	
	Private Sub Form3_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
		Dim c As System.Windows.Forms.Control
		Me.BackColor = Form2.Grid1.BackColor
		For	Each c In Me.Controls
			c.BackColor = Me.BackColor
		Next c
		
	End Sub
End Class