Option Strict Off
Option Explicit On
Imports VB = Microsoft.VisualBasic
Friend Class Form2
	Inherits System.Windows.Forms.Form
	
	Private g_SearchStop As Boolean
	Private g_NeedSaveCache As Boolean
	Private g_ViewComplete As Boolean
	Private g_ViewUnfinish As Boolean
	
	
	
	Private Sub Ini_Load()
		Dim hide_repeat, default_lan, default_format, hide_complete As String
		default_lan = INI_GetValue(g_Dir & "\Editor.ini", "GUI", "language", "0")
		default_format = INI_GetValue(g_Dir & "\Editor.ini", "GUI", "format", "1")
		hide_repeat = INI_GetValue(g_Dir & "\Editor.ini", "GUI", "hide_repeat", "1")
		
		g_len_id = INI_GetValue(g_Dir & "\Editor.ini", "PreviewWidth", "ID", "30")
		g_len_j = INI_GetValue(g_Dir & "\Editor.ini", "PreviewWidth", "JP", "300")
		g_len_c = INI_GetValue(g_Dir & "\Editor.ini", "PreviewWidth", "CN", "300")
		g_len_u = INI_GetValue(g_Dir & "\Editor.ini", "PreviewWidth", "US", "300")
		
		Dim I As Integer
		
		I = CInt(default_lan)
		If I = 0 Then
			mnuJC_Click(mnuJC, New System.EventArgs())
		ElseIf I = 1 Then 
			mnuJCA_Click(mnuJCA, New System.EventArgs())
		ElseIf I = 2 Then 
			mnuJCU_Click(mnuJCU, New System.EventArgs())
		Else
			mnuJUCAlter_Click(mnuJUCAlter, New System.EventArgs())
		End If
		
		
		g_TextFormat = CInt(default_format)
		
		g_hide_repeat = (hide_repeat = "1")
		
	End Sub
	
	Private Sub Ini_Save()
		Dim hide_repeat, default_lan, default_format, hide_complete As String
		
		If mnuJC.Checked Then
			default_lan = "0"
		ElseIf mnuJCA.Checked Then 
			default_lan = "1"
		ElseIf mnuJCU.Checked Then 
			default_lan = "2"
		Else
			default_lan = "3"
		End If
		
		default_format = CStr(g_TextFormat)
		
		hide_repeat = "0"
		If g_hide_repeat Then hide_repeat = "1"
		
		INI_SetValue(g_Dir & "\Editor.ini", "GUI", "language", default_lan)
		INI_SetValue(g_Dir & "\Editor.ini", "GUI", "format", default_format)
		INI_SetValue(g_Dir & "\Editor.ini", "GUI", "hide_repeat", hide_repeat)
		
		INI_SetValue(g_Dir & "\Editor.ini", "PreviewWidth", "ID", g_len_id)
		INI_SetValue(g_Dir & "\Editor.ini", "PreviewWidth", "JP", g_len_j)
		INI_SetValue(g_Dir & "\Editor.ini", "PreviewWidth", "CN", g_len_c)
		INI_SetValue(g_Dir & "\Editor.ini", "PreviewWidth", "US", g_len_u)
		
		
		
	End Sub
	
	
	Private Sub Form2_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
		
		g_Dir = My.Application.Info.DirectoryPath
		
		'调试代码时，工作目录不用 app.path，而是改为实际目录
		'g_Dir = "E:\vp2"
		'g_Dir = "D:\J2C\projects\vp\vp_compiler"
		
		On Error GoTo BypassReg
        'Shell("regsvr32.exe -s """ & g_Dir & "\SynMemoU.ocx""")
BypassReg: 
		
		'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
		If Dir(g_Dir & "\jp-text", FileAttribute.Directory) = "" Then
			MsgBox("当前目录下没有 jp-text目录")
			End
		End If
		
		'颜色，界面初始化
		Grid1.BackColor = System.Drawing.ColorTranslator.FromOle(RGB(&HF6s, &HF7s, &HEBs))
		Grid1.BackColorFixed = System.Drawing.ColorTranslator.FromOle(RGB(&HD1s, &HD9s, &HC1s))
		Grid2.BackColor = System.Drawing.ColorTranslator.FromOle(RGB(&HF6s, &HF7s, &HEBs))
		Grid2.BackColorFixed = System.Drawing.ColorTranslator.FromOle(RGB(&HD1s, &HD9s, &HC1s))
		txtInfo.BackColor = Grid1.BackColor
		Me.BackColor = Grid1.BackColor
		
		g_NeedSaveCache = True
		
		'此游戏特殊初始化(在游戏自定义模块中)
		Call Ini_Load()
		Call VBS_Init()
		Call Callback_Init()
		
		
		'信息表初始化
		
		
		g_CacheFile = g_Dir & "\data\cache.dat"
		If Cache_Load() = False Then Call Cache_Rebuild(True)
		
		'显示表格
		Call CommonGrid_Init()
		Call EventGrid_Init()
		
		'当前文本格式初始化（根据菜单所check的）
		Call On_Mode_Click()
		
	End Sub
	
	Private Sub Form2_FormClosed(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
		
		If g_NeedSaveCache Then
			Cache_Save()
		End If
		
		Form1.OnClickClose()
		
		Call Ini_Save()
		
		End
		
	End Sub
	
	Private Sub Grid1_ClickEvent(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles Grid1.ClickEvent
		ToolTip1.SetToolTip(Grid1, CStr(Grid1.Row))
	End Sub
	
	Private Sub Grid1_DblClick(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles Grid1.DblClick
		
		'如果没有文本就退出
		If Grid1.Row = 0 Then Exit Sub
		
        Form1.Load_Renamed((Grid1.Row), "")

		System.Windows.Forms.Application.DoEvents()
		Form1.Show()
		
	End Sub
	
	Private Sub Grid2_DblClick(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles Grid2.DblClick
		
		If Grid2.Row = 0 Then Exit Sub
		
		Form1.LoadEvent(Grid2.get_TextMatrix(Grid2.Row, 1), "")
		System.Windows.Forms.Application.DoEvents()
		Form1.Show()
		
	End Sub
	
	
	Private Sub Grid1_MouseUpEvent(ByVal eventSender As System.Object, ByVal eventArgs As AxMSFlexGridLib.DMSFlexGridEvents_MouseUpEvent) Handles Grid1.MouseUpEvent
		
		Dim sID As String
		If eventArgs.Button = 2 Then
			'输入注释
			sID = g_CacheInfo(Grid1.Row).sID
			Form3.ApplyData((Grid1.Row), sID, g_CacheInfo(Grid1.Row).sAuthor, g_CacheInfo(Grid1.Row).sMemo)
			
			If Not Form3.IsCancel Then
				Script_MemoSave(sID, (Form3.CurrentUser), (Form3.CurrentMemo))
				Grid_Update((Grid1.Row)) '界面更新user/memo以及翻译的个数
				System.Windows.Forms.Application.DoEvents()
			End If
		End If
		
	End Sub
	
	
	Private Sub Cache_Rebuild(ByRef bWordsCalc As Boolean)
		
		Dim nAllJP, nAllCN As Integer
		Dim total_words, total_trans As Integer
		Dim I, j As Integer
		Dim s As String
		'UPGRADE_WARNING: Arrays in structure tmp_info may need to be initialized before they can be used. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="814DF224-76BD-4BB4-BFFB-EA359CB9FC48"'
		Dim tmp_info As SCRIPTINFO
		
		Dim tmp_nWords As Short
		
		lstScript.Path = g_Dir & "\jp-text"
		lstScript.Pattern = "*.txt"
		lstScript.Refresh()
		
		If lstScript.Items.Count = 0 Then
			Exit Sub
		End If
		
		'UPGRADE_WARNING: Lower bound of array g_CacheInfo was changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="0F1C9BE1-AF9D-476E-83B1-17D43BECFF20"'
		ReDim g_CacheInfo(lstScript.Items.Count)
		
		
		For I = 1 To lstScript.Items.Count
			
			g_CacheInfo(I).sNo = ToDec(I, 3)
			g_CacheInfo(I).sID = Tool_GetFilenameMain(lstScript.Items(I - 1))
			'UPGRADE_WARNING: Couldn't resolve default property of object tmp_info. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
			tmp_info = Script_GetInfo(g_CacheInfo(I).sID)
			
			'计算字数比较花费时间
			If bWordsCalc Then
				g_CacheInfo(I).nWords = 0
				For j = 1 To tmp_info.JpCount
					tmp_nWords = Tool_GetWords(tmp_info.JpText(j), LANGUAGE_JP)
					g_CacheInfo(I).nWords = g_CacheInfo(I).nWords + tmp_nWords
					If tmp_info.CnText(j) <> "" And VB.Left(tmp_info.JpText(j), 1) <> "〓" Then total_trans = total_trans + tmp_nWords
				Next j
			End If
			
			total_words = total_words + g_CacheInfo(I).nWords
			
			'If tmp_info.CnCount = tmp_info.dispCount Then total_trans = total_trans + g_CacheInfo(I).nWords
			
			g_CacheInfo(I).nTrans = tmp_info.CnCount
			g_CacheInfo(I).nTotal = tmp_info.dispCount
			g_CacheInfo(I).sAuthor = tmp_info.User
			g_CacheInfo(I).sMemo = tmp_info.Memo
			
			
			
			
			'        Dim bsave As Boolean
			'        bsave = False
			'
			'        For j = 1 To tmp_info.JpCount
			'            If Left(tmp_info.JpText(j), 1) = "〓" Then
			'                Dim Repeat_Script As SCRIPTINFO
			'                Dim Repeat_Sentence As Long
			'                Dim tempstr As String
			'                Dim str() As String
			'
			'                tempstr = Left(tmp_info.JpText(j), 30)
			'                tempstr = Mid(tempstr, 2)
			'                str = Split(tempstr, "-")
			'
			'                Repeat_Script = Script_GetInfo(str(0))
			'                Repeat_Sentence = Val(str(1))
			'                If UBound(Repeat_Script.CnText) >= Repeat_Sentence And Repeat_Script.CnText(Repeat_Sentence) <> "" Then
			'                    tmp_info.CnText(j) = Repeat_Script.CnText(Repeat_Sentence)
			'                    bsave = True
			'                End If
			'            End If
			'        Next j
			'
			'        If bsave = True Then
			'            Script_Save g_CacheInfo(I).sID, tmp_info, LANGUAGE_CN
			'        End If
			
			
			
			
			System.Windows.Forms.Application.DoEvents()
			
		Next I
		
		Call Cache_Save()
		
		Me.info("日文总计字数" & Str(total_words) & ",已翻译" & Str(total_trans))
		
	End Sub
	
	Private Sub CommonGrid_Init()
		
		Grid1.cols = 6
		Grid1.set_ColAlignment(0, MSFlexGridLib.AlignmentSettings.flexAlignLeftTop)
		Grid1.set_ColAlignment(1, MSFlexGridLib.AlignmentSettings.flexAlignCenterCenter)
		Grid1.set_ColAlignment(2, MSFlexGridLib.AlignmentSettings.flexAlignCenterCenter)
		Grid1.set_ColAlignment(3, MSFlexGridLib.AlignmentSettings.flexAlignRightTop)
		Grid1.set_ColAlignment(4, MSFlexGridLib.AlignmentSettings.flexAlignCenterCenter)
		Grid1.set_ColAlignment(5, MSFlexGridLib.AlignmentSettings.flexAlignLeftTop)
		
		Grid1.set_ColWidth(0, 0) '此列隐藏
		Grid1.set_ColWidth(1, VB6.PixelsToTwipsX(Grid1.Width) * 0.2)
        Grid1.set_ColWidth(2, VB6.PixelsToTwipsX(Grid1.Width) * 0.12)
		Grid1.set_ColWidth(3, VB6.PixelsToTwipsX(Grid1.Width) * 0.1)
		Grid1.set_ColWidth(4, VB6.PixelsToTwipsX(Grid1.Width) * 0.12)
		Grid1.set_ColWidth(5, VB6.PixelsToTwipsX(Grid1.Width) * 0.53)
		
		
		Grid1.set_TextMatrix(0, 0, "No.")
		Grid1.set_TextMatrix(0, 1, "ID")
		Grid1.set_TextMatrix(0, 2, "OK / ALL")
		Grid1.set_TextMatrix(0, 3, "字数")
		Grid1.set_TextMatrix(0, 4, "译者")
		Grid1.set_TextMatrix(0, 5, "备注")
		
		Grid1.rows = 1
		
		Dim I As Integer
		
		Grid1.rows = UBound(g_CacheInfo) + 1
		
		For I = 1 To UBound(g_CacheInfo)
			Grid1.set_TextMatrix(I, 0, g_CacheInfo(I).sNo)
			Grid1.set_TextMatrix(I, 1, g_CacheInfo(I).sID)
			Grid1.set_TextMatrix(I, 2, g_CacheInfo(I).nTrans & " / " & g_CacheInfo(I).nTotal)
			Grid1.set_TextMatrix(I, 3, g_CacheInfo(I).nWords)
			Grid1.set_TextMatrix(I, 4, g_CacheInfo(I).sAuthor)
			Grid1.set_TextMatrix(I, 5, g_CacheInfo(I).sMemo)
			
			If g_ViewUnfinish And g_CacheInfo(I).nTrans = g_CacheInfo(I).nTotal Then
				Grid1.set_RowHeight(I, 0)
			End If
			
			If g_ViewComplete And g_CacheInfo(I).nTrans < g_CacheInfo(I).nTotal Then
				Grid1.set_RowHeight(I, 0)
			End If
			
			If g_hide_repeat And g_CacheInfo(I).nTotal = 0 Then
				Grid1.set_RowHeight(I, 0)
			End If
		Next I
		
	End Sub
	
	Private Sub EventGrid_Init()
		
		Grid2.cols = 2
		Grid2.set_ColAlignment(0, MSFlexGridLib.AlignmentSettings.flexAlignLeftTop)
		Grid2.set_ColAlignment(1, MSFlexGridLib.AlignmentSettings.flexAlignLeftTop)
		
		Grid2.set_ColWidth(0, 0) '不显示
		Grid2.set_ColWidth(1, VB6.PixelsToTwipsX(Grid1.Width) * 0.9)
		
		Grid2.set_TextMatrix(0, 0, "No.")
		Grid2.set_TextMatrix(0, 1, "Event")
		
		Grid2.rows = 1
		
		'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
		If Dir(g_Dir & "\event", FileAttribute.Directory) = "" Then
			Exit Sub
		End If
		
		lstScript.Path = g_Dir & "\event"
		lstScript.Pattern = "*.evt"
		lstScript.Refresh()
		
		Grid2.rows = lstScript.Items.Count + 1
		
		Dim I As Integer
		For I = 1 To lstScript.Items.Count
			
			Grid2.set_TextMatrix(I, 0, I)
			Grid2.set_TextMatrix(I, 1, Tool_GetFilenameMain(lstScript.Items(I - 1)))
			
			System.Windows.Forms.Application.DoEvents()
			
		Next I
		
	End Sub
	
	Public Sub Grid_Update(ByRef nRow As Integer)
		
		Dim sID As String
		sID = g_CacheInfo(nRow).sID
		
		'UPGRADE_WARNING: Arrays in structure info may need to be initialized before they can be used. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="814DF224-76BD-4BB4-BFFB-EA359CB9FC48"'
		Dim info As SCRIPTINFO
		
		'UPGRADE_WARNING: Couldn't resolve default property of object info. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		info = Script_GetInfo(sID)
		g_CacheInfo(nRow).nTrans = info.CnCount
		g_CacheInfo(nRow).sAuthor = info.User
		g_CacheInfo(nRow).sMemo = info.Memo
		
		Grid1.set_TextMatrix(nRow, 2, g_CacheInfo(nRow).nTrans & " / " & g_CacheInfo(nRow).nTotal)
		Grid1.set_TextMatrix(nRow, 4, g_CacheInfo(nRow).sAuthor)
		Grid1.set_TextMatrix(nRow, 5, g_CacheInfo(nRow).sMemo)
		
	End Sub
	
	
	Public Sub info(ByRef s As Object)
		'UPGRADE_WARNING: Couldn't resolve default property of object s. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		txtInfo.Text = txtInfo.Text & s & vbCrLf
		txtInfo.SelectionStart = Len(txtInfo.Text)
		txtInfo.Refresh()
		System.Windows.Forms.Application.DoEvents()
	End Sub
	
	Public Sub infoNoCr(ByRef s As Object)
		'UPGRADE_WARNING: Couldn't resolve default property of object s. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		txtInfo.Text = txtInfo.Text & s
		txtInfo.SelectionStart = Len(txtInfo.Text)
		txtInfo.Refresh()
		System.Windows.Forms.Application.DoEvents()
	End Sub
	
	Public Sub mnuAbout_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuAbout.Click
		MsgBox(vbCrLf & "-Script Editor- is open source project." & vbCrLf & "for more information, pls check " & vbCrLf & "http://agemo.126.com/" & vbCrLf & vbCrLf & "(C) Agemo 2003-2006" & vbCrLf)
	End Sub
	
	Public Sub mnuCalcWords_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuCalcWords.Click
		txtInfo.Text = ""
		info("正在统计.... pls wait...")
		
		
		Dim I, j As Integer
		'UPGRADE_WARNING: Arrays in structure inf may need to be initialized before they can be used. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="814DF224-76BD-4BB4-BFFB-EA359CB9FC48"'
		Dim inf As SCRIPTINFO
		
		Dim strTxtCN As String
		
		Dim nTotalJPCount As Integer
		nTotalJPCount = 0
		
		Dim nTotalCNCount As Integer
		nTotalCNCount = 0
		
		Dim nStart, nEnd As Integer
		
		If Grid1.RowSel >= Grid1.Row Then
			nStart = Grid1.Row
			nEnd = Grid1.RowSel
		Else
			nStart = Grid1.RowSel
			nEnd = Grid1.Row
		End If
		
		Dim sID As String
		For I = nStart To nEnd
			
			
			sID = g_CacheInfo(I).sID
			
			'UPGRADE_WARNING: Couldn't resolve default property of object inf. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
			inf = Script_GetInfo(sID)
			
			For j = 1 To inf.JpCount
				nTotalJPCount = nTotalJPCount + Tool_GetWords(inf.JpText(j), LANGUAGE_JP)
				nTotalCNCount = nTotalCNCount + Tool_GetWords(inf.CnText(j), LANGUAGE_CN)
			Next j
			
			System.Windows.Forms.Application.DoEvents()
			
		Next I
		
		Me.info("日文字数 " & nTotalJPCount)
		Me.info("译文字数 " & nTotalCNCount)
		
	End Sub
	
	
	Public Sub mnuCharCounter_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuCharCounter.Click
		txtInfo.Text = ""
		info("正在统计.... pls wait...")
		
		
		Dim I, j As Integer
		'UPGRADE_WARNING: Arrays in structure inf may need to be initialized before they can be used. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="814DF224-76BD-4BB4-BFFB-EA359CB9FC48"'
		Dim inf As SCRIPTINFO
		
		Dim w() As String
		Dim c() As Integer
		ReDim w(0)
		ReDim c(0)
		
		Dim sGlobalText As String
		
		sGlobalText = ""
		
		Dim sID As String
		For I = 1 To UBound(g_CacheInfo)
			
			sID = g_CacheInfo(I).sID
			'UPGRADE_WARNING: Couldn't resolve default property of object inf. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
			inf = Script_GetInfo(sID)
			System.Windows.Forms.Application.DoEvents()
			For j = 1 To inf.JpCount
				sGlobalText = sGlobalText & Tool_ControlCodeRemoveEx(inf.CnText(j))
			Next j
			
		Next I
		
		Word_Counter(sGlobalText, w, c)
		
		sGlobalText = ""
		For I = 1 To UBound(w)
			sGlobalText = sGlobalText & w(I) & Chr(9) & c(I) & vbCrLf
		Next I
		
		Tool_WriteTextFile(g_Dir & "\char.txt", sGlobalText)
		info("统计完毕，已经保存为工具目录下的 char.txt")
	End Sub
	
	
	Public Sub mnuFastExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuFastExit.Click
		g_NeedSaveCache = False
		Form1.OnClickClose()
		End
	End Sub
	
	Public Sub mnuJCA_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuJCA.Click
		If Not mnuJCA.Checked Then
			mnuJCU.Checked = False
			mnuJC.Checked = False
			mnuJCA.Checked = True
			mnuJUCAlter.Checked = False
		End If
	End Sub
	
	Public Sub mnuJUCAlter_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuJUCAlter.Click
		If Not mnuJUCAlter.Checked Then
			mnuJUCAlter.Checked = True
			mnuJCU.Checked = False
			mnuJC.Checked = False
			mnuJCA.Checked = False
		End If
	End Sub
	
	Public Sub mnuJC_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuJC.Click
		If Not mnuJC.Checked Then
			mnuJC.Checked = True
			mnuJCU.Checked = False
			mnuJUCAlter.Checked = False
			mnuJCA.Checked = False
		End If
	End Sub
	
	Public Sub mnuJCU_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuJCU.Click
		If Not mnuJCU.Checked Then
			mnuJCU.Checked = True
			mnuJC.Checked = False
			mnuJUCAlter.Checked = False
			mnuJCA.Checked = False
		End If
	End Sub
	
	Public Sub mnuModeCommon_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuModeCommon.Click
		If Not mnuModeCommon.Checked Then
			mnuModeCommon.Checked = True
			mnuModeEvent.Checked = False
			On_Mode_Click()
		End If
	End Sub
	
	Public Sub mnuModeEvent_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuModeEvent.Click
		If Not mnuModeEvent.Checked Then
			mnuModeEvent.Checked = True
			EventGrid_Init()
			mnuModeCommon.Checked = False
			On_Mode_Click()
		End If
	End Sub
	Private Sub On_Mode_Click()
		If mnuModeEvent.Checked Then
			Grid1.Visible = False
			Grid2.Visible = True
		Else
			Grid1.Visible = True
			Grid2.Visible = False
		End If
	End Sub
	
	
	Public Sub mnuOption_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuOption.Click
		Form5.ShowDialog()
		CommonGrid_Init()
	End Sub
	
	
	Public Sub mnuRefresh_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuRefresh.Click
		info("refreshing, pls wait")
		Cache_Rebuild((True))
		CommonGrid_Init()
		EventGrid_Init()
		info("done")
	End Sub
	
	Public Sub mnuRepeatSync_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuRepeatSync.Click
		
		Me.info("正在同步...")
		
		Dim I, j As Integer
		Dim s As String
		'UPGRADE_WARNING: Arrays in structure tmp_info may need to be initialized before they can be used. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="814DF224-76BD-4BB4-BFFB-EA359CB9FC48"'
		Dim tmp_info As SCRIPTINFO
		
		lstScript.Path = g_Dir & "\jp-text"
		lstScript.Pattern = "*.txt"
		lstScript.Refresh()
		
		If lstScript.Items.Count = 0 Then
			Exit Sub
		End If
		
		Dim tempID As String
		
		
		
		Dim bsave As Boolean
		Dim Repeat_Sentence As Integer
		Dim tempstr As String
		'UPGRADE_NOTE: str was upgraded to str_Renamed. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"'
		Dim str_Renamed() As String
		'UPGRADE_WARNING: Arrays in structure Repeat_Script may need to be initialized before they can be used. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="814DF224-76BD-4BB4-BFFB-EA359CB9FC48"'
		Dim Repeat_Script As SCRIPTINFO
		For I = 1 To lstScript.Items.Count
			
			tempID = Tool_GetFilenameMain(lstScript.Items(I - 1))
			'UPGRADE_WARNING: Couldn't resolve default property of object tmp_info. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
			tmp_info = Script_GetInfo(tempID)
			
			
			bsave = False
			
			For j = 1 To tmp_info.JpCount
				If VB.Left(tmp_info.JpText(j), 1) = "〓" Then
					
					tempstr = VB.Left(tmp_info.JpText(j), 30)
					tempstr = Mid(tempstr, 2)
					str_Renamed = Split(tempstr, "-")
					
					'UPGRADE_WARNING: Couldn't resolve default property of object Repeat_Script. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
					Repeat_Script = Script_GetInfo(str_Renamed(0))
					Repeat_Sentence = Val(str_Renamed(1))
					If UBound(Repeat_Script.CnText) >= Repeat_Sentence And Repeat_Script.CnText(Repeat_Sentence) <> "" Then
						tmp_info.CnText(j) = Repeat_Script.CnText(Repeat_Sentence)
						bsave = True
					End If
				End If
			Next j
			
			If bsave = True Then
				Script_Save(tempID, tmp_info, LANGUAGE_CN)
			End If
			
			
			
			
			System.Windows.Forms.Application.DoEvents()
			
		Next I
		
		
		Me.info("重复文本已同步")
		
		
	End Sub
	
	Public Sub mnuSearch_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuSearch.Click
		Form4.Show()
	End Sub
	
	Public Sub mnuViewAll_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuViewAll.Click
		g_ViewComplete = False
		g_ViewUnfinish = False
		mnuViewAll.Checked = True
		mnuViewComplete.Checked = False
		mnuViewUnfinish.Checked = False
		CommonGrid_Init()
	End Sub
	
	Public Sub mnuViewComplete_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuViewComplete.Click
		g_ViewComplete = True
		g_ViewUnfinish = False
		mnuViewAll.Checked = False
		mnuViewComplete.Checked = True
		mnuViewUnfinish.Checked = False
		CommonGrid_Init()
	End Sub
	
	Public Sub mnuViewUnfinish_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuViewUnfinish.Click
		g_ViewComplete = False
		g_ViewUnfinish = True
		mnuViewAll.Checked = False
		mnuViewComplete.Checked = False
		mnuViewUnfinish.Checked = True
		CommonGrid_Init()
	End Sub
End Class