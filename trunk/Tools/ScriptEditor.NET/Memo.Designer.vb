<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class Form3
#Region "Windows Form Designer generated code "
	<System.Diagnostics.DebuggerNonUserCode()> Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
	End Sub
	'Form overrides dispose to clean up the component list.
	<System.Diagnostics.DebuggerNonUserCode()> Protected Overloads Overrides Sub Dispose(ByVal Disposing As Boolean)
		If Disposing Then
			If Not components Is Nothing Then
				components.Dispose()
			End If
		End If
		MyBase.Dispose(Disposing)
	End Sub
	'Required by the Windows Form Designer
	Private components As System.ComponentModel.IContainer
	Public ToolTip1 As System.Windows.Forms.ToolTip
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents txtMemo As System.Windows.Forms.TextBox
	Public WithEvents Frame2 As System.Windows.Forms.GroupBox
	Public WithEvents txtUser As System.Windows.Forms.TextBox
	Public WithEvents Frame1 As System.Windows.Forms.GroupBox
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
		Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(Form3))
		Me.components = New System.ComponentModel.Container()
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(components)
		Me.cmdOK = New System.Windows.Forms.Button
		Me.Frame2 = New System.Windows.Forms.GroupBox
		Me.txtMemo = New System.Windows.Forms.TextBox
		Me.Frame1 = New System.Windows.Forms.GroupBox
		Me.txtUser = New System.Windows.Forms.TextBox
		Me.Frame2.SuspendLayout()
		Me.Frame1.SuspendLayout()
		Me.SuspendLayout()
		Me.ToolTip1.Active = True
		Me.Text = "Memo"
		Me.ClientSize = New System.Drawing.Size(355, 199)
		Me.Location = New System.Drawing.Point(4, 30)
		Me.Font = New System.Drawing.Font("ËÎÌå", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(134, Byte))
		Me.Icon = CType(resources.GetObject("Form3.Icon"), System.Drawing.Icon)
		Me.MaximizeBox = False
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable
		Me.ControlBox = True
		Me.Enabled = True
		Me.KeyPreview = False
		Me.MinimizeBox = True
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = True
		Me.HelpButton = False
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Me.Name = "Form3"
		Me.cmdOK.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdOK.Text = "OK"
		Me.AcceptButton = Me.cmdOK
		Me.cmdOK.Size = New System.Drawing.Size(81, 25)
		Me.cmdOK.Location = New System.Drawing.Point(256, 168)
		Me.cmdOK.TabIndex = 4
		Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
		Me.cmdOK.CausesValidation = True
		Me.cmdOK.Enabled = True
		Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdOK.TabStop = True
		Me.cmdOK.Name = "cmdOK"
		Me.Frame2.Text = "±¸×¢"
		Me.Frame2.Size = New System.Drawing.Size(324, 58)
		Me.Frame2.Location = New System.Drawing.Point(17, 93)
		Me.Frame2.TabIndex = 2
		Me.Frame2.BackColor = System.Drawing.SystemColors.Control
		Me.Frame2.Enabled = True
		Me.Frame2.ForeColor = System.Drawing.SystemColors.ControlText
		Me.Frame2.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Frame2.Visible = True
		Me.Frame2.Name = "Frame2"
		Me.txtMemo.AutoSize = False
		Me.txtMemo.Size = New System.Drawing.Size(274, 29)
		Me.txtMemo.Location = New System.Drawing.Point(24, 18)
		Me.txtMemo.TabIndex = 3
		Me.txtMemo.AcceptsReturn = True
		Me.txtMemo.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtMemo.BackColor = System.Drawing.SystemColors.Window
		Me.txtMemo.CausesValidation = True
		Me.txtMemo.Enabled = True
		Me.txtMemo.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtMemo.HideSelection = True
		Me.txtMemo.ReadOnly = False
		Me.txtMemo.Maxlength = 0
		Me.txtMemo.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtMemo.MultiLine = False
		Me.txtMemo.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtMemo.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtMemo.TabStop = True
		Me.txtMemo.Visible = True
		Me.txtMemo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtMemo.Name = "txtMemo"
		Me.Frame1.Text = "ÒëÕß"
		Me.Frame1.Size = New System.Drawing.Size(327, 55)
		Me.Frame1.Location = New System.Drawing.Point(16, 16)
		Me.Frame1.TabIndex = 0
		Me.Frame1.BackColor = System.Drawing.SystemColors.Control
		Me.Frame1.Enabled = True
		Me.Frame1.ForeColor = System.Drawing.SystemColors.ControlText
		Me.Frame1.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Frame1.Visible = True
		Me.Frame1.Name = "Frame1"
		Me.txtUser.AutoSize = False
		Me.txtUser.Size = New System.Drawing.Size(275, 29)
		Me.txtUser.Location = New System.Drawing.Point(24, 16)
		Me.txtUser.TabIndex = 1
		Me.txtUser.AcceptsReturn = True
		Me.txtUser.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtUser.BackColor = System.Drawing.SystemColors.Window
		Me.txtUser.CausesValidation = True
		Me.txtUser.Enabled = True
		Me.txtUser.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtUser.HideSelection = True
		Me.txtUser.ReadOnly = False
		Me.txtUser.Maxlength = 0
		Me.txtUser.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtUser.MultiLine = False
		Me.txtUser.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtUser.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtUser.TabStop = True
		Me.txtUser.Visible = True
		Me.txtUser.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtUser.Name = "txtUser"
		Me.Controls.Add(cmdOK)
		Me.Controls.Add(Frame2)
		Me.Controls.Add(Frame1)
		Me.Frame2.Controls.Add(txtMemo)
		Me.Frame1.Controls.Add(txtUser)
		Me.Frame2.ResumeLayout(False)
		Me.Frame1.ResumeLayout(False)
		Me.ResumeLayout(False)
		Me.PerformLayout()
	End Sub
#End Region 
End Class