<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class Form2
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
    'Public WithEvents Script1 As AxMSScriptControl.AxScriptControl
	Public WithEvents cboTone As System.Windows.Forms.ComboBox
	Public WithEvents txtInfo As System.Windows.Forms.TextBox
    Public WithEvents Grid1 As AxMSFlexGridLib.AxMSFlexGrid
	Public WithEvents lstScript As Microsoft.VisualBasic.Compatibility.VB6.FileListBox
    Public WithEvents Grid2 As AxMSFlexGridLib.AxMSFlexGrid
	Public WithEvents mnuOption As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuRefresh As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuSearch As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuCalcWords As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuRepeatSync As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuCharCounter As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuFastExit As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuGrid As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuModeCommon As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuModeEvent As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuMode As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuJC As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuJCA As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuJCU As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuJUCAlter As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuLang As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuViewAll As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuViewComplete As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuViewUnfinish As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuView As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuDummy As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuAbout As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents MainMenu1 As System.Windows.Forms.MenuStrip
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form2))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        'Me.Script1 = New AxMSScriptControl.AxScriptControl
        Me.cboTone = New System.Windows.Forms.ComboBox
        Me.txtInfo = New System.Windows.Forms.TextBox
        Me.Grid1 = New AxMSFlexGridLib.AxMSFlexGrid
        Me.lstScript = New Microsoft.VisualBasic.Compatibility.VB6.FileListBox
        Me.Grid2 = New AxMSFlexGridLib.AxMSFlexGrid
        Me.MainMenu1 = New System.Windows.Forms.MenuStrip
        Me.mnuGrid = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuOption = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuRefresh = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuSearch = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuCalcWords = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuRepeatSync = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuCharCounter = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuFastExit = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuMode = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuModeCommon = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuModeEvent = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuLang = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuJC = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuJCA = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuJCU = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuJUCAlter = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuView = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuViewAll = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuViewComplete = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuViewUnfinish = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuDummy = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuAbout = New System.Windows.Forms.ToolStripMenuItem
        'CType(Me.Script1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Grid1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Grid2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.MainMenu1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Script1
        '
        'Me.Script1.Enabled = True
        'Me.Script1.Location = New System.Drawing.Point(272, 520)
        'Me.Script1.Name = "Script1"
        'Me.Script1.OcxState = CType(resources.GetObject("Script1.OcxState"), System.Windows.Forms.AxHost.State)
        'Me.Script1.Size = New System.Drawing.Size(38, 38)
        'Me.Script1.TabIndex = 0
        '
        'cboTone
        '
        Me.cboTone.BackColor = System.Drawing.SystemColors.Window
        Me.cboTone.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboTone.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboTone.Font = New System.Drawing.Font("SimSun", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboTone.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboTone.Location = New System.Drawing.Point(0, 512)
        Me.cboTone.Name = "cboTone"
        Me.cboTone.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboTone.Size = New System.Drawing.Size(142, 20)
        Me.cboTone.TabIndex = 4
        Me.cboTone.Visible = False
        '
        'txtInfo
        '
        Me.txtInfo.AcceptsReturn = True
        Me.txtInfo.BackColor = System.Drawing.SystemColors.Control
        Me.txtInfo.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtInfo.Font = New System.Drawing.Font("SimSun", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtInfo.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.txtInfo.Location = New System.Drawing.Point(0, 424)
        Me.txtInfo.MaxLength = 0
        Me.txtInfo.Multiline = True
        Me.txtInfo.Name = "txtInfo"
        Me.txtInfo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtInfo.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtInfo.Size = New System.Drawing.Size(615, 85)
        Me.txtInfo.TabIndex = 2
        '
        'Grid1
        '
        Me.Grid1.Location = New System.Drawing.Point(-1, 20)
        Me.Grid1.Name = "Grid1"
        Me.Grid1.OcxState = CType(resources.GetObject("Grid1.OcxState"), System.Windows.Forms.AxHost.State)
        Me.Grid1.Size = New System.Drawing.Size(616, 398)
        Me.Grid1.TabIndex = 1
        '
        'lstScript
        '
        Me.lstScript.BackColor = System.Drawing.SystemColors.Window
        Me.lstScript.Cursor = System.Windows.Forms.Cursors.Default
        Me.lstScript.Font = New System.Drawing.Font("SimSun", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lstScript.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lstScript.FormattingEnabled = True
        Me.lstScript.Location = New System.Drawing.Point(488, 480)
        Me.lstScript.Name = "lstScript"
        Me.lstScript.Pattern = "*.*"
        Me.lstScript.Size = New System.Drawing.Size(118, 52)
        Me.lstScript.TabIndex = 0
        Me.lstScript.Visible = False
        '
        'Grid2
        '
        Me.Grid2.Location = New System.Drawing.Point(0, 20)
        Me.Grid2.Name = "Grid2"
        Me.Grid2.OcxState = CType(resources.GetObject("Grid2.OcxState"), System.Windows.Forms.AxHost.State)
        Me.Grid2.Size = New System.Drawing.Size(616, 398)
        Me.Grid2.TabIndex = 3
        '
        'MainMenu1
        '
        Me.MainMenu1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuGrid, Me.mnuMode, Me.mnuLang, Me.mnuView, Me.mnuDummy, Me.mnuAbout})
        Me.MainMenu1.Location = New System.Drawing.Point(0, 0)
        Me.MainMenu1.Name = "MainMenu1"
        Me.MainMenu1.Size = New System.Drawing.Size(615, 24)
        Me.MainMenu1.TabIndex = 5
        '
        'mnuGrid
        '
        Me.mnuGrid.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuOption, Me.mnuRefresh, Me.mnuSearch, Me.mnuCalcWords, Me.mnuRepeatSync, Me.mnuCharCounter, Me.mnuFastExit})
        Me.mnuGrid.Name = "mnuGrid"
        Me.mnuGrid.Size = New System.Drawing.Size(55, 20)
        Me.mnuGrid.Text = "主选单"
        '
        'mnuOption
        '
        Me.mnuOption.Name = "mnuOption"
        Me.mnuOption.Size = New System.Drawing.Size(238, 22)
        Me.mnuOption.Text = "选项"
        '
        'mnuRefresh
        '
        Me.mnuRefresh.Name = "mnuRefresh"
        Me.mnuRefresh.Size = New System.Drawing.Size(238, 22)
        Me.mnuRefresh.Text = "刷新"
        '
        'mnuSearch
        '
        Me.mnuSearch.Name = "mnuSearch"
        Me.mnuSearch.ShortcutKeys = System.Windows.Forms.Keys.F3
        Me.mnuSearch.Size = New System.Drawing.Size(238, 22)
        Me.mnuSearch.Text = "查找"
        '
        'mnuCalcWords
        '
        Me.mnuCalcWords.Name = "mnuCalcWords"
        Me.mnuCalcWords.Size = New System.Drawing.Size(238, 22)
        Me.mnuCalcWords.Text = "计算选定范围内字数"
        Me.mnuCalcWords.Visible = False
        '
        'mnuRepeatSync
        '
        Me.mnuRepeatSync.Name = "mnuRepeatSync"
        Me.mnuRepeatSync.Size = New System.Drawing.Size(238, 22)
        Me.mnuRepeatSync.Text = "重复文本同步"
        '
        'mnuCharCounter
        '
        Me.mnuCharCounter.Name = "mnuCharCounter"
        Me.mnuCharCounter.Size = New System.Drawing.Size(238, 22)
        Me.mnuCharCounter.Text = "字符使用统计"
        '
        'mnuFastExit
        '
        Me.mnuFastExit.Name = "mnuFastExit"
        Me.mnuFastExit.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.F4), System.Windows.Forms.Keys)
        Me.mnuFastExit.Size = New System.Drawing.Size(238, 22)
        Me.mnuFastExit.Text = "快速退出(不保存cache)"
        Me.mnuFastExit.Visible = False
        '
        'mnuMode
        '
        Me.mnuMode.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuModeCommon, Me.mnuModeEvent})
        Me.mnuMode.Name = "mnuMode"
        Me.mnuMode.Size = New System.Drawing.Size(43, 20)
        Me.mnuMode.Text = "视图"
        '
        'mnuModeCommon
        '
        Me.mnuModeCommon.Checked = True
        Me.mnuModeCommon.CheckState = System.Windows.Forms.CheckState.Checked
        Me.mnuModeCommon.Name = "mnuModeCommon"
        Me.mnuModeCommon.Size = New System.Drawing.Size(102, 22)
        Me.mnuModeCommon.Text = "普通"
        '
        'mnuModeEvent
        '
        Me.mnuModeEvent.Name = "mnuModeEvent"
        Me.mnuModeEvent.Size = New System.Drawing.Size(102, 22)
        Me.mnuModeEvent.Text = "Event"
        '
        'mnuLang
        '
        Me.mnuLang.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuJC, Me.mnuJCA, Me.mnuJCU, Me.mnuJUCAlter})
        Me.mnuLang.Name = "mnuLang"
        Me.mnuLang.Size = New System.Drawing.Size(43, 20)
        Me.mnuLang.Text = "语言"
        '
        'mnuJC
        '
        Me.mnuJC.Checked = True
        Me.mnuJC.CheckState = System.Windows.Forms.CheckState.Checked
        Me.mnuJC.Name = "mnuJC"
        Me.mnuJC.Size = New System.Drawing.Size(170, 22)
        Me.mnuJC.Text = "日－中"
        '
        'mnuJCA
        '
        Me.mnuJCA.Name = "mnuJCA"
        Me.mnuJCA.Size = New System.Drawing.Size(170, 22)
        Me.mnuJCA.Text = "日－中－对比"
        '
        'mnuJCU
        '
        Me.mnuJCU.Name = "mnuJCU"
        Me.mnuJCU.Size = New System.Drawing.Size(170, 22)
        Me.mnuJCU.Text = "日－中－英"
        '
        'mnuJUCAlter
        '
        Me.mnuJUCAlter.Name = "mnuJUCAlter"
        Me.mnuJUCAlter.Size = New System.Drawing.Size(170, 22)
        Me.mnuJUCAlter.Text = "日－英－中－对比"
        '
        'mnuView
        '
        Me.mnuView.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuViewAll, Me.mnuViewComplete, Me.mnuViewUnfinish})
        Me.mnuView.Name = "mnuView"
        Me.mnuView.Size = New System.Drawing.Size(43, 20)
        Me.mnuView.Text = "显示"
        '
        'mnuViewAll
        '
        Me.mnuViewAll.Checked = True
        Me.mnuViewAll.CheckState = System.Windows.Forms.CheckState.Checked
        Me.mnuViewAll.Name = "mnuViewAll"
        Me.mnuViewAll.Size = New System.Drawing.Size(110, 22)
        Me.mnuViewAll.Text = "所有"
        '
        'mnuViewComplete
        '
        Me.mnuViewComplete.Name = "mnuViewComplete"
        Me.mnuViewComplete.Size = New System.Drawing.Size(110, 22)
        Me.mnuViewComplete.Text = "已完成"
        '
        'mnuViewUnfinish
        '
        Me.mnuViewUnfinish.Name = "mnuViewUnfinish"
        Me.mnuViewUnfinish.Size = New System.Drawing.Size(110, 22)
        Me.mnuViewUnfinish.Text = "未完成"
        '
        'mnuDummy
        '
        Me.mnuDummy.Enabled = False
        Me.mnuDummy.Name = "mnuDummy"
        Me.mnuDummy.Size = New System.Drawing.Size(25, 20)
        Me.mnuDummy.Text = "  "
        '
        'mnuAbout
        '
        Me.mnuAbout.Name = "mnuAbout"
        Me.mnuAbout.Size = New System.Drawing.Size(43, 20)
        Me.mnuAbout.Text = "关于"
        '
        'Form2
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 14.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(615, 560)
        'Me.Controls.Add(Me.Script1)
        Me.Controls.Add(Me.cboTone)
        Me.Controls.Add(Me.txtInfo)
        Me.Controls.Add(Me.Grid1)
        Me.Controls.Add(Me.lstScript)
        Me.Controls.Add(Me.Grid2)
        Me.Controls.Add(Me.MainMenu1)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("SimSun", 10.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Location = New System.Drawing.Point(11, 30)
        Me.MaximizeBox = False
        Me.Name = "Form2"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Script Editor 4.0 (Build 06-07-23)"
        'CType(Me.Script1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Grid1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Grid2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.MainMenu1.ResumeLayout(False)
        Me.MainMenu1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
#End Region 
End Class