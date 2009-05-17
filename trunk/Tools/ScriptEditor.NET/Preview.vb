Option Strict Off
Option Explicit On
Imports VB = Microsoft.VisualBasic
Friend Class Form1
	Inherits System.Windows.Forms.Form
	
	Private g_CurSentence As Integer '当前选择第几句
	'UPGRADE_WARNING: Arrays in structure g_Script may need to be initialized before they can be used. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="814DF224-76BD-4BB4-BFFB-EA359CB9FC48"'
	Private g_Script As SCRIPTINFO
	Private g_IsDirty As Boolean
	
	Private g_CurParagraph As Integer '主画面的第几段
	
    Private g_is_compare_mode As Boolean

    Private evtHTMLSel As New VBOnEvent

    Private g_lastkeycode As Integer  '最后一次按下的非控制按键


	Private Function GetControlCodeHtml(ByRef s As String) As Object
		GetControlCodeHtml = Replace(s, vbCrLf, "<br>")
		'UPGRADE_WARNING: Couldn't resolve default property of object GetControlCodeHtml. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		If GetControlCodeHtml = "" Then GetControlCodeHtml = "&nbsp;"
	End Function
	
	'UPGRADE_NOTE: Load was upgraded to Load_Renamed. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"'
	Public Sub Load_Renamed(ByRef sNo As Integer, ByRef sPreviewFile As String)
        txtCN.Enabled = False
		txtInfo.Text = ""
		txtNo.Text = ""
		cmdCopyJP.Enabled = False
		
		Dim sID As String
		
		If g_CurParagraph <> 0 And g_Script.isEventMode = False Then Form2.Grid_Update(g_CurParagraph)
		
		g_CurParagraph = CInt(sNo)
		
		sID = g_CacheInfo(g_CurParagraph).sID
		'UPGRADE_WARNING: Couldn't resolve default property of object g_Script. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		g_Script = Script_GetInfo(sID)
		
		
		Me.Text = sID
		txtCN.Text = ""
		g_IsDirty = False
		
		
		Dim tpl_body, tpl_row As String '读入模板
		Dim tpl_js, tpl_ruler As String
        Dim tpl_dir As String
		
		g_is_compare_mode = False
		
		If Form2.mnuJCU.Checked = True Then tpl_dir = "J-C-U"
		If Form2.mnuJC.Checked = True Then tpl_dir = "J-C"
		If Form2.mnuJUCAlter.Checked = True Then
			cmdAlter.Visible = True
			chkModFilter.Visible = True
			tpl_dir = "J-U-C-Compare"
			g_is_compare_mode = True
		ElseIf Form2.mnuJCA.Checked = True Then 
			cmdAlter.Visible = True
			chkModFilter.Visible = True
			tpl_dir = "J-C-Compare"
			g_is_compare_mode = True
		Else
			cmdAlter.Visible = False
			chkModFilter.Visible = False
		End If
		
		tpl_dir = g_Dir & "\template\" & tpl_dir
		
		tpl_js = Tool_LoadTextFile(g_Dir & "\template\" & "\js.html")
		tpl_row = Tool_LoadTextFile(tpl_dir & "\row.html")
		TemplateSetColSize(tpl_row)
		
		tpl_body = Tool_LoadTextFile(tpl_dir & "\body.html")
		TemplateSetColSize(tpl_body)
		
		tpl_ruler = Tool_LoadTextFile(tpl_dir & "\ruler.html")
		TemplateSetColSize(tpl_ruler)
		
		
		
		Dim rows As String
		Dim s As String
		Dim sItemJP As String
		Dim sItemUS As String
		Dim sItemCN As String
		Dim sItemCNCompare As String
		
		rows = ""
		
		Dim I As Integer
		Dim nAlters As Integer
		
		nAlters = 0
		
		'标记第一行说话者
		'Dim sTalker As String
		'Dim PageTalkerCount As Long
		'Dim PageTalkerFind As Long
		
		'PageTalkerCount = 0
		'sTalker = Trim(txtTalker.Text)
		
		For I = 1 To g_Script.JpCount
			
			s = Replace(tpl_row, "TEMPLATE_ADDR", g_Script.Address(I))
			s = Replace(s, "TEMPLATE_ID", CStr(I))
			
			
			If chkCode.CheckState = 1 Then
				'UPGRADE_WARNING: Couldn't resolve default property of object GetControlCodeHtml(). Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
				sItemJP = GetControlCodeHtml(g_Script.JpText(I))
				'UPGRADE_WARNING: Couldn't resolve default property of object GetControlCodeHtml(). Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
				sItemUS = GetControlCodeHtml(g_Script.UsText(I))
				'UPGRADE_WARNING: Couldn't resolve default property of object GetControlCodeHtml(). Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
				sItemCN = GetControlCodeHtml(g_Script.CnText(I))
			Else
				sItemJP = Callback_Preview(g_Script.JpText(I), LANGUAGE_JP)
				sItemUS = Callback_Preview(g_Script.UsText(I), LANGUAGE_US)
				sItemCN = Callback_Preview(g_Script.CnText(I), LANGUAGE_CN)
			End If
			'   PageTalkerFind = 0
			'   If sTalker <> "" Then
			'       If InStr(1, sItemCN, sTalker & "<br>") = 1 Then PageTalkerFind = 1
			'       If InStr(1, sItemCN, "<b>" & sTalker & "</b><br>") = 1 Then PageTalkerFind = 1
			'   End If
			
			sItemCNCompare = ""
			
			If g_is_compare_mode Then
				
				If g_Script.CnTextAlter(I) <> g_Script.CnText(I) And g_Script.CnText(I) <> "" Then
					If chkCode.CheckState = 1 Then
						'UPGRADE_WARNING: Couldn't resolve default property of object GetControlCodeHtml(). Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
						sItemCNCompare = GetControlCodeHtml(g_Script.CnTextAlter(I))
					Else
						sItemCNCompare = Callback_Preview(g_Script.CnTextAlter(I), LANGUAGE_CN)
					End If
					nAlters = nAlters + 1
					MarkTextDiffHTML(sItemCN, sItemCNCompare)
				ElseIf chkModFilter.CheckState = 1 Then 
					GoTo for_i_next
				End If
				
			End If
			
			If (sItemJP = "&nbsp;" Or sItemJP = "") And (sItemCN = "&nbsp;" Or sItemCN = "") Then
				'日文预览为空的，形成表格融合的效果，这样一眼能看到
				s = Replace(s, "TEMPLATE_JP", "")
				s = Replace(s, "TEMPLATE_US", "")
				s = Replace(s, "TEMPLATE_CN", "")
				s = Replace(s, "TEMPLATE_COMPARE", "")
				s = Replace(s, "TEMPLATE_MARK_COLOR", "#CCBB99")
				
				's = "" '此处改为空的，可以使得不显示。
			Else
				s = Replace(s, "TEMPLATE_JP", sItemJP)
				s = Replace(s, "TEMPLATE_US", sItemUS)
				s = Replace(s, "TEMPLATE_CN", sItemCN)
				s = Replace(s, "TEMPLATE_COMPARE", sItemCNCompare)
				
				'If PageTalkerFind = 0 Then
				s = Replace(s, "TEMPLATE_MARK_COLOR", "#CCCC99")
				'Else
				'    s = Replace(s, "TEMPLATE_MARK_COLOR", "#8888CC")
				'End If
			End If
			
			
			
			If g_hide_repeat Then
				If VB.Left(g_Script.JpText(I), 1) = "〓" Then
					s = Replace(s, "DISPLAY_FLAG", "style='display:none'")
				End If
				
			End If
			
			If g_Script.JpText(I) = "" Then
				s = Replace(s, "DISPLAY_FLAG", "style='display:none'")
			Else
				s = Replace(s, "DISPLAY_FLAG", "")
			End If
			
			rows = rows & s
			
for_i_next: 
		Next I
		
		'如果美版比日版多的话
		For I = g_Script.JpCount + 1 To g_Script.UsCount
			s = tpl_row
			s = Replace(s, "TEMPLATE_ID", "U" & I)
			s = Replace(s, "TEMPLATE_ADDR", "N/A")
			
			sItemUS = Callback_Preview(g_Script.UsText(I), LANGUAGE_US)
			
			
			s = Replace(s, "TEMPLATE_JP", "US edition only. " & vbCrLf & "do NOT translation")
			s = Replace(s, "TEMPLATE_CN", "US edition only. " & vbCrLf & "do NOT translation")
			s = Replace(s, "TEMPLATE_US", sItemUS)
			s = Replace(s, "TEMPLATE_MARK_COLOR", "#ccbb99")
			s = Replace(s, "TEMPLATE_COMPARE", "&nbsp;")
			
			If sItemUS = "" Or sItemUS = "&nbsp;" Then s = ""
			
			rows = rows & s
			
		Next I
		
		Dim sAll As String
		Dim sHTMLFile As String
		
		sAll = Replace(tpl_body, "<!-- TEMPLATE_ROWS -->", rows)
		
		
		
		sAll = sAll & tpl_js
		
		If sPreviewFile = "" Then sPreviewFile = "preview.html"
		
        sHTMLFile = My.Application.GetEnvironmentVariable("TEMP") & "\preview.html"

        'sHTMLFile = g_Dir & "\template\temp\" & sPreviewFile
		Tool_WriteTextFile(sHTMLFile, sAll)
		'UPGRADE_WARNING: Navigate2 was upgraded to Navigate and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
		wbMain.Navigate(New System.URI(sHTMLFile))

        sHTMLFile = My.Application.GetEnvironmentVariable("TEMP") & "\ruler.htm"
        'sHTMLFile = g_Dir & "\template\temp\ruler.htm"
		Tool_WriteTextFile(sHTMLFile, tpl_ruler)
		'UPGRADE_WARNING: Navigate2 was upgraded to Navigate and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
		wbRuler.Navigate(New System.URI(sHTMLFile))
		
		If g_is_compare_mode Then
			If g_Script.CnAlterCount = 0 Then
				info("本段不存在对照译文")
			Else
				info("本段译文修改句数： " & nAlters)
			End If
		End If
		
	End Sub
	
	
	
	Public Sub LoadEvent(ByRef sEventID As String, ByRef sPreviewFile As String)
		Dim sID As String
		
		If g_CurParagraph <> 0 And g_Script.isEventMode = False Then Form2.Grid_Update(g_CurParagraph)
		
		g_CurParagraph = -1 'CLng(sNo)
		
		'UPGRADE_WARNING: Couldn't resolve default property of object g_Script. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		g_Script = Script_GetEventInfo(sEventID)
		
		Me.Text = sEventID
		txtCN.Text = ""
		g_IsDirty = False
		
		Dim tpl_body, tpl_row As String '读入模板
		Dim tpl_js, tpl_ruler As String
		Dim tpl_dir, tpl_scene As String
		
		
		g_is_compare_mode = False
		
		If Form2.mnuJCU.Checked = True Then tpl_dir = "J-C-U"
		If Form2.mnuJC.Checked = True Then tpl_dir = "J-C"
		If Form2.mnuJUCAlter.Checked = True Then
			cmdAlter.Visible = True
			chkModFilter.Visible = True
			tpl_dir = "J-U-C-Compare"
			g_is_compare_mode = True
		ElseIf Form2.mnuJCA.Checked = True Then 
			cmdAlter.Visible = True
			chkModFilter.Visible = True
			tpl_dir = "J-C-Compare"
			g_is_compare_mode = True
		Else
			cmdAlter.Visible = False
			chkModFilter.Visible = False
		End If
		
		tpl_dir = g_Dir & "\template\" & tpl_dir
		
		tpl_js = Tool_LoadTextFile(g_Dir & "\template\" & "\js.html")
		tpl_scene = Tool_LoadTextFile(tpl_dir & "\scene.html")
		
		tpl_row = Tool_LoadTextFile(tpl_dir & "\row.html")
		TemplateSetColSize(tpl_row)
		
		tpl_body = Tool_LoadTextFile(tpl_dir & "\body.html")
		TemplateSetColSize(tpl_body)
		
		tpl_ruler = Tool_LoadTextFile(tpl_dir & "\ruler.html")
		TemplateSetColSize(tpl_ruler)
		
		Dim rows As String
		Dim s As String
		Dim sItemJP As String
		Dim sItemUS As String
		Dim sItemCN As String
		Dim sItemCNCompare As String
		
		rows = ""
		
		Dim I As Integer
		Dim nAlters As Integer
		
		nAlters = 0
		
		'标记第一行说话者
		'Dim sTalker As String
		'Dim PageTalkerCount As Long
		'Dim PageTalkerFind As Long
		
		'PageTalkerCount = 0
		'sTalker = Trim(txtTalker.Text)
		
		
		Dim sCurrentID As String
		
		sCurrentID = ""
		
		Dim sMemo, sUser As String
		For I = 1 To g_Script.JpCount
			
			If g_Script.arrID(I) <> sCurrentID Then
				sCurrentID = g_Script.arrID(I)
				Script_MemoLoad(sCurrentID, sUser, sMemo)
				sUser = sCurrentID & " - " & sMemo
				s = Replace(tpl_scene, "SCENE_INFO", sUser)
				
				rows = rows & s
			End If
			
			
			s = tpl_row
			s = Replace(s, "TEMPLATE_ADDR", g_Script.Address(I))
			s = Replace(s, "TEMPLATE_ID", CStr(I))
			
			sItemJP = Callback_Preview(g_Script.JpText(I), LANGUAGE_JP)
			sItemUS = Callback_Preview(g_Script.UsText(I), LANGUAGE_US)
			sItemCN = Callback_Preview(g_Script.CnText(I), LANGUAGE_CN)
			
			'   PageTalkerFind = 0
			'   If sTalker <> "" Then
			'       If InStr(1, sItemCN, sTalker & "<br>") = 1 Then PageTalkerFind = 1
			'       If InStr(1, sItemCN, "<b>" & sTalker & "</b><br>") = 1 Then PageTalkerFind = 1
			'   End If
			
			sItemCNCompare = ""
			
			If g_is_compare_mode Then
				
				If g_Script.CnTextAlter(I) <> g_Script.CnText(I) And g_Script.CnText(I) <> "" Then
					sItemCNCompare = Callback_Preview(g_Script.CnTextAlter(I), LANGUAGE_CN)
					nAlters = nAlters + 1
					MarkTextDiffHTML(sItemCN, sItemCNCompare)
				End If
			End If
			
			If (sItemJP = "&nbsp;" Or sItemJP = "") And (sItemUS = "&nbsp;" Or sItemUS = "") And (sItemCN = "&nbsp;" Or sItemCN = "") Then
				'日文预览为空的，形成表格融合的效果，这样一眼能看到
				s = Replace(s, "TEMPLATE_JP", "")
				s = Replace(s, "TEMPLATE_US", "")
				s = Replace(s, "TEMPLATE_CN", "")
				s = Replace(s, "TEMPLATE_COMPARE", "")
				s = Replace(s, "TEMPLATE_MARK_COLOR", "#CCBB99")
			Else
				s = Replace(s, "TEMPLATE_JP", sItemJP)
				s = Replace(s, "TEMPLATE_US", sItemUS)
				s = Replace(s, "TEMPLATE_CN", sItemCN)
				s = Replace(s, "TEMPLATE_COMPARE", sItemCNCompare)
				
				'If PageTalkerFind = 0 Then
				s = Replace(s, "TEMPLATE_MARK_COLOR", "#CCCC99")
				'Else
				'    s = Replace(s, "TEMPLATE_MARK_COLOR", "#8888CC")
				'End If
			End If
			
			rows = rows & s
			
		Next I
		
		'如果美版比日版多的话
		For I = g_Script.JpCount + 1 To g_Script.UsCount
			s = tpl_row
			s = Replace(s, "TEMPLATE_ID", "U" & I)
			
			sItemUS = Callback_Preview(g_Script.UsText(I), LANGUAGE_US)
			s = Replace(s, "TEMPLATE_ADDR", "N/A")
			s = Replace(s, "TEMPLATE_JP", "Us Only. " & vbCrLf & "Do NOT Translation")
			s = Replace(s, "TEMPLATE_CN", "Us Only. " & vbCrLf & "Do NOT Translation")
			s = Replace(s, "TEMPLATE_US", sItemUS)
			s = Replace(s, "TEMPLATE_MARK_COLOR", "#CCbb99")
			s = Replace(s, "TEMPLATE_COMPARE", "&nbsp;")
			
			rows = rows & s
			
		Next I
		
		Dim sAll As String
		Dim sHTMLFile As String
		
		sAll = Replace(tpl_body, "<!-- TEMPLATE_ROWS -->", rows)
		
		
		
		sAll = sAll & tpl_js
		
		If sPreviewFile = "" Then sPreviewFile = "preview.html"
        sHTMLFile = My.Application.GetEnvironmentVariable("TEMP") & "\preview.html"
        'sHTMLFile = g_Dir & "\template\temp\" & sPreviewFile
		Tool_WriteTextFile(sHTMLFile, sAll)
		'UPGRADE_WARNING: Navigate2 was upgraded to Navigate and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
		wbMain.Navigate(New System.URI(sHTMLFile))
        sHTMLFile = My.Application.GetEnvironmentVariable("TEMP") & "\ruler.html"
        'sHTMLFile = g_Dir & "\template\temp\ruler.htm"
		Tool_WriteTextFile(sHTMLFile, tpl_ruler)
		'UPGRADE_WARNING: Navigate2 was upgraded to Navigate and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
		wbRuler.Navigate(New System.URI(sHTMLFile))
		
		If g_is_compare_mode Then
			If g_Script.CnAlterCount = 0 Then
				info("本段不存在对照译文")
			Else
				info("本段译文修改句数： " & nAlters)
			End If
		End If
		
	End Sub
	
	
	
	
	
	
	Private Sub btnReloadSyn_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles btnReloadSyn.Click
		txtCN.LoadSynHighlight((g_Dir))
	End Sub
	
	'UPGRADE_WARNING: Event chkCode.CheckStateChanged may fire when form is initialized. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="88B12AE1-6DE0-48A0-86F1-60C0686C026A"'
	Private Sub chkCode_CheckStateChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles chkCode.CheckStateChanged
		cmdLoadCN_Click(cmdLoadCN, New System.EventArgs())
	End Sub
	
	Private Sub cmdAlter_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdAlter.Click
		
		If MsgBoxResult.OK <> MsgBox("此句、用的修改译文覆盖现有译文吗？", MsgBoxStyle.DefaultButton2 + MsgBoxStyle.OKCancel, "") Then
			Exit Sub
		End If
		
		If g_Script.CnTextAlter(g_CurSentence) = "" Then
			MsgBox("此句，修改后译文为空，不能覆盖", MsgBoxStyle.Exclamation)
			Exit Sub
		End If
		
		If g_Script.CnTextAlter(g_CurSentence) = g_Script.CnText(g_CurSentence) Then
			MsgBox("此句，没有修改", MsgBoxStyle.Exclamation)
			Exit Sub
		End If
		
		txtCN.Text = g_Script.CnTextAlter(g_CurSentence)
		g_IsDirty = True
		System.Windows.Forms.Application.DoEvents()
		
	End Sub
	
	
	
	Private Sub cmdCopyJP_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdCopyJP.Click
		
		If txtCN.Text <> "" Then
			If MsgBoxResult.OK <> MsgBox("已存在翻译文本, 是否覆盖", MsgBoxStyle.DefaultButton2 + MsgBoxStyle.OKCancel, "") Then
				Exit Sub
			End If
		End If
		
		Dim Repeat_Sentence As Integer
		Dim tempstr As String
		'UPGRADE_NOTE: str was upgraded to str_Renamed. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"'
		Dim str_Renamed() As String
		'UPGRADE_WARNING: Arrays in structure Repeat_Script may need to be initialized before they can be used. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="814DF224-76BD-4BB4-BFFB-EA359CB9FC48"'
		Dim Repeat_Script As SCRIPTINFO
		If VB.Left(g_Script.JpText(g_CurSentence), 1) = "〓" Then
			'如果是重复文本,就用已翻译文本
			
			tempstr = VB.Left(g_Script.JpText(g_CurSentence), 30)
			tempstr = Mid(tempstr, 2)
			str_Renamed = Split(tempstr, "-")
			
			'UPGRADE_WARNING: Couldn't resolve default property of object Repeat_Script. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
			Repeat_Script = Script_GetInfo(str_Renamed(0))
			Repeat_Sentence = Val(str_Renamed(1))
			txtCN.Text = Repeat_Script.CnText(Repeat_Sentence)
			'如果已翻译文本实际上没翻译也还是空,那就用日版文本
			If txtCN.Text = "" Then
				MsgBox("重复文本的对照源文本未翻译")
				txtCN.Text = Tool_WideConv(g_Script.JpText(g_CurSentence))
			End If
		Else
			txtCN.Text = Tool_WideConv(g_Script.JpText(g_CurSentence))
		End If
		g_IsDirty = True
		System.Windows.Forms.Application.DoEvents()
		
	End Sub
	
	Private Sub cmdLoadCN_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdLoadCN.Click
		If g_Script.isEventMode Then
			LoadEvent(g_Script.ID, "")
		Else
			Load_Renamed(g_CurParagraph, "")
		End If
	End Sub
	
	Private Sub cmdSaveCN_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdSaveCN.Click
		g_IsDirty = False
		
		g_Script.CnText(g_CurSentence) = Callback_Save((txtCN.Text))
		
		'///////////////////////////////////////////
		'修改预览
		'///////////////////////////////////////////
		Dim doc As mshtml.IHTMLDocument2
		Dim ObjID As mshtml.IHTMLElement
		doc = wbMain.Document.DomDocument
		
		Dim sItemCN, sItemCNCompare As String
		
		sItemCN = Callback_Preview(g_Script.CnText(g_CurSentence), LANGUAGE_CN)
		
		If g_is_compare_mode Then
			sItemCNCompare = Callback_Preview(g_Script.CnTextAlter(g_CurSentence), LANGUAGE_CN)
			MarkTextDiffHTML(sItemCN, sItemCNCompare)
			
			ObjID = doc.All.item("TD_COMPARE_" & g_CurSentence)
			ObjID.innerHTML = sItemCNCompare
		End If
		
		ObjID = doc.All.item("PREVIEW_" & g_CurSentence)
		ObjID.innerHTML = sItemCN
		
		'///////////////////////////////////////////
		'save file
		'///////////////////////////////////////////
		If g_Script.isEventMode Then
			Script_SaveEvent(g_Script.arrID(g_CurSentence), g_Script.arrNum(g_CurSentence), g_Script.CnText(g_CurSentence))
			Me.info("event saved")
		Else
			Script_Save(g_CacheInfo(g_CurParagraph).sID, g_Script, LANGUAGE_CN)
			Me.info("" & g_CacheInfo(g_CurParagraph).sID & " saved")
		End If
		
		
		
		
		
		
	End Sub
	
	
	
	
	
	
	Public Sub OnClickClose()
		If g_IsDirty Then
			cmdSaveCN_Click(cmdSaveCN, New System.EventArgs())
		End If
		
		
	End Sub
	
	
	
	Private Sub Form1_FormClosed(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
		'UPGRADE_ISSUE: Event parameter Cancel was not upgraded. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="FB723E3C-1C06-4D2B-B083-E6CD0D334DA8"'
        'Cancel = True
		Me.Hide()
		
		OnClickClose()
		
		If g_Script.isEventMode = False Then Form2.Grid_Update(g_CurParagraph)
		
	End Sub
	
	'UPGRADE_WARNING: Event Form1.Resize may fire when form is initialized. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="88B12AE1-6DE0-48A0-86F1-60C0686C026A"'
	Private Sub Form1_Resize(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Resize
		Dim w As Integer
		Dim h As Integer
		
		w = VB6.PixelsToTwipsX(Me.Width) - 200
		If w <= 1000 Then w = 1000
		
		h = VB6.PixelsToTwipsY(Me.Height) * 0.7
		If h <= 1000 Then h = 1000
		
		wbRuler.Left = VB6.TwipsToPixelsX(1)
		wbRuler.Width = VB6.TwipsToPixelsX(w)
		
		wbMain.Width = VB6.TwipsToPixelsX(w)
		wbMain.Left = VB6.TwipsToPixelsX(1)
		
		wbMain.Height = VB6.TwipsToPixelsY(h)
		
		txtCN.Top = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(wbMain.Height) + VB6.PixelsToTwipsY(wbMain.Top) + 100)
		txtCN.Height = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(Me.Height) * 0.2)
		
		FrameTool.Top = txtCN.Top
		FrameTool.Left = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(Me.Width) - VB6.PixelsToTwipsX(FrameTool.Width) - 200)
		
		txtCN.Width = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(FrameTool.Left) - VB6.PixelsToTwipsX(txtCN.Left) - 500)
		
	End Sub
	Private Sub Form1_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
		Dim c As System.Windows.Forms.Control
		Me.BackColor = Form2.Grid1.BackColor
		For	Each c In Me.Controls
			'UPGRADE_WARNING: TypeName has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            If TypeName(c).ToString <> "WebBrowser" And TypeName(c).ToString <> "AxSynMemoX" Then
                c.BackColor = Me.BackColor
            End If
		Next c
		
		'UPGRADE_WARNING: Navigate2 was upgraded to Navigate and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        'wbMain.Navigate(New System.Uri("about:blank"))
		System.Windows.Forms.Application.DoEvents()
		
	End Sub
	
	Public Sub info(ByRef s As Object)
		'UPGRADE_WARNING: Couldn't resolve default property of object s. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		txtInfo.Text = txtInfo.Text & s & vbCrLf
		txtInfo.SelectionStart = Len(txtInfo.Text)
	End Sub
	
	Private Sub txtCN_Change()
		g_IsDirty = True
	End Sub
	Private Sub txtCN_KeyUp(ByRef KeyCode As Short, ByRef Shift As Short)
		
		Dim doc As mshtml.IHTMLDocument2
		Dim ObjID As mshtml.IHTMLInputElement
		If Shift = VB6.ShiftConstants.CtrlMask And (KeyCode = 38 Or KeyCode = 40) Then 'up/down
			
			doc = wbMain.Document.DomDocument
			ObjID = doc.All.item("NEXT_ID")
			
			If ObjID Is Nothing Then
				MsgBox("当前template文件是旧版的，不支持NEXT_ID，请更换 js.html")
				Exit Sub
			End If
			
			If KeyCode = 40 Then
				ObjID.Value = "+"
			Else
				ObjID.Value = "-"
			End If
			
			Do 
				If ObjID.Value = "" Then Exit Do
				System.Windows.Forms.Application.DoEvents()
			Loop 
			
			OnHTMLClick()
		End If
	End Sub
	
    Public Sub OnHTMLClick()
        txtCN.Enabled = True

        If g_IsDirty = True Then
            cmdSaveCN_Click(cmdSaveCN, New System.EventArgs())
        End If

        Dim doc As mshtml.IHTMLDocument2
        Dim ObjID As mshtml.IHTMLInputElement
        doc = wbMain.Document.DomDocument
        ObjID = doc.All.item("SELECT_ID")


        Dim sID As String
        sID = ObjID.Value

        g_CurSentence = Val(sID)
        txtCN.Text = g_Script.CnText(g_CurSentence)
        txtNo.Text = CStr(g_CurSentence)
        cmdCopyJP.Enabled = True

        Dim Repeat_Sentence As Integer
        Dim tempstr As String
        'UPGRADE_NOTE: str was upgraded to str_Renamed. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"'
        Dim str_Renamed() As String
        'UPGRADE_WARNING: Arrays in structure Repeat_Script may need to be initialized before they can be used. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="814DF224-76BD-4BB4-BFFB-EA359CB9FC48"'
        Dim Repeat_Script As SCRIPTINFO
        If txtCN.Text = "" Then
            '如果是未翻译文本
            If VB.Left(g_Script.JpText(g_CurSentence), 1) = "〓" Then
                '如果是重复文本

                tempstr = VB.Left(g_Script.JpText(g_CurSentence), 30)
                tempstr = Mid(tempstr, 2)
                str_Renamed = Split(tempstr, "-")

                'UPGRADE_WARNING: Couldn't resolve default property of object Repeat_Script. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                Repeat_Script = Script_GetInfo(str_Renamed(0))
                Repeat_Sentence = Val(str_Renamed(1))
                txtCN.Text = Repeat_Script.CnText(Repeat_Sentence)
                If txtCN.Text = "" Then
                    '如果已翻译源文本实际上没翻译,不更新
                    g_IsDirty = False
                Else
                    g_IsDirty = True
                End If
            Else
                '非重复文本则显示日文文本
                txtCN.Text = g_Script.JpText(g_CurSentence)
                g_IsDirty = False
            End If
        Else
            g_IsDirty = False
        End If


    End Sub

    'Private Sub txtCN_OnKeyPressEvent(ByVal sender As Object, ByVal e As AxSynMemoU.ISynMemoXEvents_OnKeyPressEvent) Handles txtCN.OnKeyPress
    '    g_lastkeycode = e.key
    'End Sub
    Private Sub txtCN_OnChange(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCN.OnChange
        g_IsDirty = True
    End Sub


    Private Sub txtCN_OnKeyUpEvent(ByVal sender As Object, ByVal e As AxSynMemoU.ISynMemoXEvents_OnKeyUpEvent) Handles txtCN.OnKeyUp
        Dim doc As mshtml.IHTMLDocument2
        Dim ObjID As mshtml.IHTMLInputElement
        Dim keycode As Integer
        keycode = e.keyCode

        If e.shift = 0 And (keycode = Keys.PrintScreen Or keycode = Keys.Pause) Then 'up/down
            doc = wbMain.Document.DomDocument
            ObjID = doc.all.item("NEXT_ID")

            If ObjID Is Nothing Then
                MsgBox("当前template文件是旧版的，不支持NEXT_ID，请更换 js.html")
                Exit Sub
            End If


            If keycode = Keys.Pause Then
                ObjID.value = "+"
            Else
                ObjID.value = "-"
            End If

            Do
                If ObjID.value = "" Then Exit Do
                System.Windows.Forms.Application.DoEvents()
            Loop

            OnHTMLClick()
        End If
        'g_lastkeycode = 0
    End Sub

    'UPGRADE_WARNING: Event txtFontSize.TextChanged may fire when form is initialized. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="88B12AE1-6DE0-48A0-86F1-60C0686C026A"'
    Private Sub txtFontSize_TextChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles txtFontSize.TextChanged

        If Val(txtFontSize.Text) > 8 Then
            txtCN.Font = VB6.FontChangeSize(txtCN.Font, Val(txtFontSize.Text))
        End If
    End Sub

    'UPGRADE_ISSUE: ShDocW.WebBrowser.DocumentComplete pDisp was not upgraded. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6B85A2A7-FE9F-4FBE-AA0C-CF11AC86A305"'
    Private Sub wbMain_DocumentCompleted(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.WebBrowserDocumentCompletedEventArgs) Handles wbMain.DocumentCompleted

        If wbMain.ReadyState <> WebBrowserReadyState.Complete Then Exit Sub
        'System.Threading.Thread.Sleep(1000)
        Dim URL As String = eventArgs.Url.ToString()

        evtHTMLSel.Set_Destination(Me, "OnHTMLClick")
        Dim doc As mshtml.IHTMLDocument2
        On Error GoTo err_doc
        doc = wbMain.Document.DomDocument
        'UPGRADE_WARNING: TypeName has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        'If TypeName(doc).ToString = "HTMLDocumentClass" Then
        'doc.title = "good"
        On Error GoTo err_set
        AddHandler CType(doc, mshtml.HTMLDocumentEvents2_Event).onclick, AddressOf evtHTMLSel.My_Default_Method
        'End If
        Exit Sub
err_doc:
        MsgBox("doc error")
        Exit Sub
err_set:
        MsgBox("ser onclick error")
    End Sub
End Class