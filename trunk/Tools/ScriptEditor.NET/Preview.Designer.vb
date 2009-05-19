<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class Form1
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
	Public WithEvents txtCN As AxSynMemoU.AxSynMemoX
	Public WithEvents cmdSaveCN As System.Windows.Forms.Button
	Public WithEvents wbRuler As System.Windows.Forms.WebBrowser
	Public WithEvents wbMain As System.Windows.Forms.WebBrowser
	Public WithEvents btnReloadSyn As System.Windows.Forms.Button
	Public WithEvents cmdLoadCN As System.Windows.Forms.Button
	Public WithEvents chkCode As System.Windows.Forms.CheckBox
	Public WithEvents cmdCopyJP As System.Windows.Forms.Button
	Public WithEvents txtNo As System.Windows.Forms.TextBox
	Public WithEvents chkModFilter As System.Windows.Forms.CheckBox
	Public WithEvents txtFontSize As System.Windows.Forms.TextBox
	Public WithEvents Frame2 As System.Windows.Forms.GroupBox
	Public WithEvents txtInfo As System.Windows.Forms.TextBox
	Public WithEvents cmdAlter As System.Windows.Forms.Button
	Public WithEvents FrameTool As System.Windows.Forms.GroupBox
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdCopyJP = New System.Windows.Forms.Button
        Me.txtNo = New System.Windows.Forms.TextBox
        Me.txtInfo = New System.Windows.Forms.TextBox
        Me.txtCN = New AxSynMemoU.AxSynMemoX
        Me.cmdSaveCN = New System.Windows.Forms.Button
        Me.wbRuler = New System.Windows.Forms.WebBrowser
        Me.wbMain = New System.Windows.Forms.WebBrowser
        Me.FrameTool = New System.Windows.Forms.GroupBox
        Me.btnReloadSyn = New System.Windows.Forms.Button
        Me.cmdLoadCN = New System.Windows.Forms.Button
        Me.chkCode = New System.Windows.Forms.CheckBox
        Me.chkModFilter = New System.Windows.Forms.CheckBox
        Me.Frame2 = New System.Windows.Forms.GroupBox
        Me.txtFontSize = New System.Windows.Forms.TextBox
        Me.cmdAlter = New System.Windows.Forms.Button
        CType(Me.txtCN, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.FrameTool.SuspendLayout()
        Me.Frame2.SuspendLayout()
        Me.SuspendLayout()
        '
        'cmdCopyJP
        '
        Me.cmdCopyJP.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCopyJP.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCopyJP.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCopyJP.Location = New System.Drawing.Point(48, 16)
        Me.cmdCopyJP.Name = "cmdCopyJP"
        Me.cmdCopyJP.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCopyJP.Size = New System.Drawing.Size(33, 25)
        Me.cmdCopyJP.TabIndex = 10
        Me.cmdCopyJP.Text = "↓"
        Me.ToolTip1.SetToolTip(Me.cmdCopyJP, "用日文原文覆盖")
        Me.cmdCopyJP.UseVisualStyleBackColor = False
        '
        'txtNo
        '
        Me.txtNo.AcceptsReturn = True
        Me.txtNo.BackColor = System.Drawing.SystemColors.Window
        Me.txtNo.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtNo.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtNo.Location = New System.Drawing.Point(8, 16)
        Me.txtNo.MaxLength = 0
        Me.txtNo.Name = "txtNo"
        Me.txtNo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtNo.Size = New System.Drawing.Size(33, 21)
        Me.txtNo.TabIndex = 9
        Me.txtNo.Text = "Text1"
        Me.ToolTip1.SetToolTip(Me.txtNo, "当前选中的句子")
        '
        'txtInfo
        '
        Me.txtInfo.AcceptsReturn = True
        Me.txtInfo.BackColor = System.Drawing.SystemColors.Window
        Me.txtInfo.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtInfo.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtInfo.Location = New System.Drawing.Point(8, 72)
        Me.txtInfo.MaxLength = 0
        Me.txtInfo.Multiline = True
        Me.txtInfo.Name = "txtInfo"
        Me.txtInfo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtInfo.Size = New System.Drawing.Size(169, 41)
        Me.txtInfo.TabIndex = 5
        Me.txtInfo.Text = "Text1"
        Me.ToolTip1.SetToolTip(Me.txtInfo, "提示信息")
        '
        'txtCN
        '
        Me.txtCN.Location = New System.Drawing.Point(0, 496)
        Me.txtCN.Name = "txtCN"
        Me.txtCN.OcxState = CType(resources.GetObject("txtCN.OcxState"), System.Windows.Forms.AxHost.State)
        Me.txtCN.Size = New System.Drawing.Size(609, 169)
        Me.txtCN.TabIndex = 13
        '
        'cmdSaveCN
        '
        Me.cmdSaveCN.BackColor = System.Drawing.SystemColors.Control
        Me.cmdSaveCN.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdSaveCN.Font = New System.Drawing.Font("SimSun", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdSaveCN.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdSaveCN.Location = New System.Drawing.Point(328, 680)
        Me.cmdSaveCN.Name = "cmdSaveCN"
        Me.cmdSaveCN.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdSaveCN.Size = New System.Drawing.Size(137, 22)
        Me.cmdSaveCN.TabIndex = 2
        Me.cmdSaveCN.Text = "保存+预览(自动)"
        Me.cmdSaveCN.UseVisualStyleBackColor = False
        Me.cmdSaveCN.Visible = False
        '
        'wbRuler
        '
        Me.wbRuler.AllowWebBrowserDrop = False
        Me.wbRuler.CausesValidation = False
        Me.wbRuler.Location = New System.Drawing.Point(0, 1)
        Me.wbRuler.Name = "wbRuler"
        Me.wbRuler.Size = New System.Drawing.Size(936, 25)
        Me.wbRuler.TabIndex = 0
        Me.wbRuler.TabStop = False
        '
        'wbMain
        '
        Me.wbMain.Location = New System.Drawing.Point(-1, 23)
        Me.wbMain.Name = "wbMain"
        Me.wbMain.Size = New System.Drawing.Size(929, 467)
        Me.wbMain.TabIndex = 1
        '
        'FrameTool
        '
        Me.FrameTool.BackColor = System.Drawing.SystemColors.Control
        Me.FrameTool.Controls.Add(Me.btnReloadSyn)
        Me.FrameTool.Controls.Add(Me.cmdLoadCN)
        Me.FrameTool.Controls.Add(Me.chkCode)
        Me.FrameTool.Controls.Add(Me.cmdCopyJP)
        Me.FrameTool.Controls.Add(Me.txtNo)
        Me.FrameTool.Controls.Add(Me.chkModFilter)
        Me.FrameTool.Controls.Add(Me.Frame2)
        Me.FrameTool.Controls.Add(Me.txtInfo)
        Me.FrameTool.Controls.Add(Me.cmdAlter)
        Me.FrameTool.ForeColor = System.Drawing.SystemColors.ControlText
        Me.FrameTool.Location = New System.Drawing.Point(616, 496)
        Me.FrameTool.Name = "FrameTool"
        Me.FrameTool.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.FrameTool.Size = New System.Drawing.Size(313, 161)
        Me.FrameTool.TabIndex = 3
        Me.FrameTool.TabStop = False
        '
        'btnReloadSyn
        '
        Me.btnReloadSyn.BackColor = System.Drawing.SystemColors.Control
        Me.btnReloadSyn.Cursor = System.Windows.Forms.Cursors.Default
        Me.btnReloadSyn.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnReloadSyn.Location = New System.Drawing.Point(8, 128)
        Me.btnReloadSyn.Name = "btnReloadSyn"
        Me.btnReloadSyn.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btnReloadSyn.Size = New System.Drawing.Size(89, 25)
        Me.btnReloadSyn.TabIndex = 14
        Me.btnReloadSyn.Text = "刷新关键字"
        Me.btnReloadSyn.UseVisualStyleBackColor = False
        '
        'cmdLoadCN
        '
        Me.cmdLoadCN.BackColor = System.Drawing.SystemColors.Control
        Me.cmdLoadCN.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdLoadCN.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdLoadCN.Location = New System.Drawing.Point(88, 16)
        Me.cmdLoadCN.Name = "cmdLoadCN"
        Me.cmdLoadCN.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdLoadCN.Size = New System.Drawing.Size(89, 25)
        Me.cmdLoadCN.TabIndex = 12
        Me.cmdLoadCN.Text = "Reload"
        Me.cmdLoadCN.UseVisualStyleBackColor = False
        '
        'chkCode
        '
        Me.chkCode.BackColor = System.Drawing.SystemColors.Control
        Me.chkCode.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkCode.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkCode.Location = New System.Drawing.Point(8, 48)
        Me.chkCode.Name = "chkCode"
        Me.chkCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkCode.Size = New System.Drawing.Size(105, 25)
        Me.chkCode.TabIndex = 11
        Me.chkCode.Text = "显示控制字符"
        Me.chkCode.UseVisualStyleBackColor = False
        '
        'chkModFilter
        '
        Me.chkModFilter.BackColor = System.Drawing.SystemColors.Control
        Me.chkModFilter.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkModFilter.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkModFilter.Location = New System.Drawing.Point(192, 48)
        Me.chkModFilter.Name = "chkModFilter"
        Me.chkModFilter.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkModFilter.Size = New System.Drawing.Size(97, 25)
        Me.chkModFilter.TabIndex = 8
        Me.chkModFilter.Text = "仅显示修改的"
        Me.chkModFilter.UseVisualStyleBackColor = False
        '
        'Frame2
        '
        Me.Frame2.BackColor = System.Drawing.SystemColors.Control
        Me.Frame2.Controls.Add(Me.txtFontSize)
        Me.Frame2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame2.Location = New System.Drawing.Point(192, 72)
        Me.Frame2.Name = "Frame2"
        Me.Frame2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame2.Size = New System.Drawing.Size(97, 41)
        Me.Frame2.TabIndex = 6
        Me.Frame2.TabStop = False
        Me.Frame2.Text = "编辑字体大小"
        '
        'txtFontSize
        '
        Me.txtFontSize.AcceptsReturn = True
        Me.txtFontSize.BackColor = System.Drawing.SystemColors.Window
        Me.txtFontSize.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtFontSize.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtFontSize.Location = New System.Drawing.Point(8, 16)
        Me.txtFontSize.MaxLength = 0
        Me.txtFontSize.Name = "txtFontSize"
        Me.txtFontSize.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtFontSize.Size = New System.Drawing.Size(81, 21)
        Me.txtFontSize.TabIndex = 7
        Me.txtFontSize.Text = "11"
        '
        'cmdAlter
        '
        Me.cmdAlter.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAlter.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAlter.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAlter.Location = New System.Drawing.Point(192, 8)
        Me.cmdAlter.Name = "cmdAlter"
        Me.cmdAlter.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAlter.Size = New System.Drawing.Size(105, 33)
        Me.cmdAlter.TabIndex = 4
        Me.cmdAlter.Text = "复制此句译文从cn-compare\"
        Me.cmdAlter.UseVisualStyleBackColor = False
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(986, 741)
        Me.Controls.Add(Me.txtCN)
        Me.Controls.Add(Me.cmdSaveCN)
        Me.Controls.Add(Me.wbRuler)
        Me.Controls.Add(Me.wbMain)
        Me.Controls.Add(Me.FrameTool)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Location = New System.Drawing.Point(4, 30)
        Me.MinimizeBox = False
        Me.Name = "Form1"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Edit"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.txtCN, System.ComponentModel.ISupportInitialize).EndInit()
        Me.FrameTool.ResumeLayout(False)
        Me.FrameTool.PerformLayout()
        Me.Frame2.ResumeLayout(False)
        Me.Frame2.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
#End Region
End Class