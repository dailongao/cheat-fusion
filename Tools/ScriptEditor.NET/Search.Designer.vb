<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class Form4
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
	Public WithEvents mnuExport As System.Windows.Forms.Button
	Public WithEvents cmdSearch1Line As System.Windows.Forms.Button
	Public WithEvents txtSearchTo As System.Windows.Forms.TextBox
	Public WithEvents txtSearchFrom As System.Windows.Forms.TextBox
	Public WithEvents cboLang As System.Windows.Forms.ComboBox
	Public WithEvents txtInfo As System.Windows.Forms.TextBox
	Public WithEvents cmdReplace As System.Windows.Forms.Button
	Public WithEvents cmdSearch As System.Windows.Forms.Button
	Public WithEvents txtReplTo As System.Windows.Forms.TextBox
	Public WithEvents txtReplFrom As System.Windows.Forms.TextBox
	Public WithEvents wbMain As System.Windows.Forms.WebBrowser
	Public WithEvents lblTo As System.Windows.Forms.Label
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
		Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(Form4))
		Me.components = New System.ComponentModel.Container()
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(components)
		Me.mnuExport = New System.Windows.Forms.Button
		Me.cmdSearch1Line = New System.Windows.Forms.Button
		Me.txtSearchTo = New System.Windows.Forms.TextBox
		Me.txtSearchFrom = New System.Windows.Forms.TextBox
		Me.cboLang = New System.Windows.Forms.ComboBox
		Me.txtInfo = New System.Windows.Forms.TextBox
		Me.cmdReplace = New System.Windows.Forms.Button
		Me.cmdSearch = New System.Windows.Forms.Button
		Me.txtReplTo = New System.Windows.Forms.TextBox
		Me.txtReplFrom = New System.Windows.Forms.TextBox
		Me.wbMain = New System.Windows.Forms.WebBrowser
		Me.lblTo = New System.Windows.Forms.Label
		Me.SuspendLayout()
		Me.ToolTip1.Active = True
		Me.Text = "Search & Replace"
		Me.ClientSize = New System.Drawing.Size(725, 628)
		Me.Location = New System.Drawing.Point(4, 23)
		Me.Icon = CType(resources.GetObject("Form4.Icon"), System.Drawing.Icon)
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
		Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable
		Me.ControlBox = True
		Me.Enabled = True
		Me.KeyPreview = False
		Me.MaximizeBox = True
		Me.MinimizeBox = True
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = True
		Me.HelpButton = False
		Me.Name = "Form4"
		Me.mnuExport.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.mnuExport.Text = "人名拆离"
		Me.mnuExport.Font = New System.Drawing.Font("宋体", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.mnuExport.Size = New System.Drawing.Size(113, 25)
		Me.mnuExport.Location = New System.Drawing.Point(296, 600)
		Me.mnuExport.TabIndex = 11
		Me.mnuExport.Visible = False
		Me.mnuExport.BackColor = System.Drawing.SystemColors.Control
		Me.mnuExport.CausesValidation = True
		Me.mnuExport.Enabled = True
		Me.mnuExport.ForeColor = System.Drawing.SystemColors.ControlText
		Me.mnuExport.Cursor = System.Windows.Forms.Cursors.Default
		Me.mnuExport.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.mnuExport.TabStop = True
		Me.mnuExport.Name = "mnuExport"
		Me.cmdSearch1Line.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdSearch1Line.Text = "查找第一行(人名)"
		Me.cmdSearch1Line.Font = New System.Drawing.Font("宋体", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.cmdSearch1Line.Size = New System.Drawing.Size(113, 25)
		Me.cmdSearch1Line.Location = New System.Drawing.Point(295, 560)
		Me.cmdSearch1Line.TabIndex = 10
		Me.cmdSearch1Line.Visible = False
		Me.cmdSearch1Line.BackColor = System.Drawing.SystemColors.Control
		Me.cmdSearch1Line.CausesValidation = True
		Me.cmdSearch1Line.Enabled = True
		Me.cmdSearch1Line.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdSearch1Line.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdSearch1Line.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdSearch1Line.TabStop = True
		Me.cmdSearch1Line.Name = "cmdSearch1Line"
		Me.txtSearchTo.AutoSize = False
		Me.txtSearchTo.Size = New System.Drawing.Size(57, 25)
		Me.txtSearchTo.Location = New System.Drawing.Point(256, 520)
		Me.txtSearchTo.TabIndex = 8
		Me.txtSearchTo.Text = "Text1"
		Me.txtSearchTo.AcceptsReturn = True
		Me.txtSearchTo.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtSearchTo.BackColor = System.Drawing.SystemColors.Window
		Me.txtSearchTo.CausesValidation = True
		Me.txtSearchTo.Enabled = True
		Me.txtSearchTo.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtSearchTo.HideSelection = True
		Me.txtSearchTo.ReadOnly = False
		Me.txtSearchTo.Maxlength = 0
		Me.txtSearchTo.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtSearchTo.MultiLine = False
		Me.txtSearchTo.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtSearchTo.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtSearchTo.TabStop = True
		Me.txtSearchTo.Visible = True
		Me.txtSearchTo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtSearchTo.Name = "txtSearchTo"
		Me.txtSearchFrom.AutoSize = False
		Me.txtSearchFrom.Size = New System.Drawing.Size(57, 25)
		Me.txtSearchFrom.Location = New System.Drawing.Point(168, 520)
		Me.txtSearchFrom.TabIndex = 7
		Me.txtSearchFrom.Text = "Text1"
		Me.txtSearchFrom.AcceptsReturn = True
		Me.txtSearchFrom.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtSearchFrom.BackColor = System.Drawing.SystemColors.Window
		Me.txtSearchFrom.CausesValidation = True
		Me.txtSearchFrom.Enabled = True
		Me.txtSearchFrom.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtSearchFrom.HideSelection = True
		Me.txtSearchFrom.ReadOnly = False
		Me.txtSearchFrom.Maxlength = 0
		Me.txtSearchFrom.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtSearchFrom.MultiLine = False
		Me.txtSearchFrom.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtSearchFrom.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtSearchFrom.TabStop = True
		Me.txtSearchFrom.Visible = True
		Me.txtSearchFrom.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtSearchFrom.Name = "txtSearchFrom"
		Me.cboLang.Size = New System.Drawing.Size(145, 21)
		Me.cboLang.Location = New System.Drawing.Point(16, 520)
		Me.cboLang.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.cboLang.TabIndex = 6
		Me.cboLang.BackColor = System.Drawing.SystemColors.Window
		Me.cboLang.CausesValidation = True
		Me.cboLang.Enabled = True
		Me.cboLang.ForeColor = System.Drawing.SystemColors.WindowText
		Me.cboLang.IntegralHeight = True
		Me.cboLang.Cursor = System.Windows.Forms.Cursors.Default
		Me.cboLang.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cboLang.Sorted = False
		Me.cboLang.TabStop = True
		Me.cboLang.Visible = True
		Me.cboLang.Name = "cboLang"
		Me.txtInfo.AutoSize = False
		Me.txtInfo.BackColor = System.Drawing.SystemColors.Control
		Me.txtInfo.Font = New System.Drawing.Font("Arial", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.txtInfo.ForeColor = System.Drawing.Color.FromARGB(64, 64, 64)
		Me.txtInfo.Size = New System.Drawing.Size(399, 129)
		Me.txtInfo.Location = New System.Drawing.Point(432, 488)
		Me.txtInfo.MultiLine = True
		Me.txtInfo.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
		Me.txtInfo.TabIndex = 5
		Me.txtInfo.AcceptsReturn = True
		Me.txtInfo.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtInfo.CausesValidation = True
		Me.txtInfo.Enabled = True
		Me.txtInfo.HideSelection = True
		Me.txtInfo.ReadOnly = False
		Me.txtInfo.Maxlength = 0
		Me.txtInfo.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtInfo.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtInfo.TabStop = True
		Me.txtInfo.Visible = True
		Me.txtInfo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtInfo.Name = "txtInfo"
		Me.cmdReplace.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdReplace.Text = "执行替换"
		Me.cmdReplace.Font = New System.Drawing.Font("宋体", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.cmdReplace.Size = New System.Drawing.Size(89, 25)
		Me.cmdReplace.Location = New System.Drawing.Point(192, 600)
		Me.cmdReplace.TabIndex = 4
		Me.cmdReplace.BackColor = System.Drawing.SystemColors.Control
		Me.cmdReplace.CausesValidation = True
		Me.cmdReplace.Enabled = True
		Me.cmdReplace.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdReplace.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdReplace.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdReplace.TabStop = True
		Me.cmdReplace.Name = "cmdReplace"
		Me.cmdSearch.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdSearch.Text = "查找"
		Me.cmdSearch.Font = New System.Drawing.Font("宋体", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.cmdSearch.Size = New System.Drawing.Size(89, 25)
		Me.cmdSearch.Location = New System.Drawing.Point(192, 560)
		Me.cmdSearch.TabIndex = 3
		Me.cmdSearch.BackColor = System.Drawing.SystemColors.Control
		Me.cmdSearch.CausesValidation = True
		Me.cmdSearch.Enabled = True
		Me.cmdSearch.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdSearch.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdSearch.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdSearch.TabStop = True
		Me.cmdSearch.Name = "cmdSearch"
		Me.txtReplTo.AutoSize = False
		Me.txtReplTo.Font = New System.Drawing.Font("宋体", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.txtReplTo.Size = New System.Drawing.Size(161, 25)
		Me.txtReplTo.Location = New System.Drawing.Point(16, 600)
		Me.txtReplTo.TabIndex = 2
		Me.txtReplTo.AcceptsReturn = True
		Me.txtReplTo.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtReplTo.BackColor = System.Drawing.SystemColors.Window
		Me.txtReplTo.CausesValidation = True
		Me.txtReplTo.Enabled = True
		Me.txtReplTo.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtReplTo.HideSelection = True
		Me.txtReplTo.ReadOnly = False
		Me.txtReplTo.Maxlength = 0
		Me.txtReplTo.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtReplTo.MultiLine = False
		Me.txtReplTo.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtReplTo.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtReplTo.TabStop = True
		Me.txtReplTo.Visible = True
		Me.txtReplTo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtReplTo.Name = "txtReplTo"
		Me.txtReplFrom.AutoSize = False
		Me.txtReplFrom.Font = New System.Drawing.Font("宋体", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.txtReplFrom.Size = New System.Drawing.Size(161, 25)
		Me.txtReplFrom.Location = New System.Drawing.Point(16, 560)
		Me.txtReplFrom.TabIndex = 1
		Me.txtReplFrom.AcceptsReturn = True
		Me.txtReplFrom.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtReplFrom.BackColor = System.Drawing.SystemColors.Window
		Me.txtReplFrom.CausesValidation = True
		Me.txtReplFrom.Enabled = True
		Me.txtReplFrom.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtReplFrom.HideSelection = True
		Me.txtReplFrom.ReadOnly = False
		Me.txtReplFrom.Maxlength = 0
		Me.txtReplFrom.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtReplFrom.MultiLine = False
		Me.txtReplFrom.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtReplFrom.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtReplFrom.TabStop = True
		Me.txtReplFrom.Visible = True
		Me.txtReplFrom.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtReplFrom.Name = "txtReplFrom"
		Me.wbMain.Size = New System.Drawing.Size(857, 488)
		Me.wbMain.Location = New System.Drawing.Point(1, -2)
		Me.wbMain.TabIndex = 0
		Me.wbMain.AllowWebBrowserDrop = True
		Me.wbMain.Name = "wbMain"
		Me.lblTo.Text = "To"
		Me.lblTo.Size = New System.Drawing.Size(13, 13)
		Me.lblTo.Location = New System.Drawing.Point(232, 520)
		Me.lblTo.TabIndex = 9
		Me.lblTo.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblTo.BackColor = System.Drawing.SystemColors.Control
		Me.lblTo.Enabled = True
		Me.lblTo.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblTo.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblTo.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblTo.UseMnemonic = True
		Me.lblTo.Visible = True
		Me.lblTo.AutoSize = True
		Me.lblTo.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblTo.Name = "lblTo"
		Me.Controls.Add(mnuExport)
		Me.Controls.Add(cmdSearch1Line)
		Me.Controls.Add(txtSearchTo)
		Me.Controls.Add(txtSearchFrom)
		Me.Controls.Add(cboLang)
		Me.Controls.Add(txtInfo)
		Me.Controls.Add(cmdReplace)
		Me.Controls.Add(cmdSearch)
		Me.Controls.Add(txtReplTo)
		Me.Controls.Add(txtReplFrom)
		Me.Controls.Add(wbMain)
		Me.Controls.Add(lblTo)
		Me.ResumeLayout(False)
		Me.PerformLayout()
	End Sub
#End Region 
End Class