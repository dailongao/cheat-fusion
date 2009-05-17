Option Strict Off
Option Explicit On
Imports VB = Microsoft.VisualBasic
Friend Class Form4
	Inherits System.Windows.Forms.Form
	
	Private g_SearchStop As Boolean
	
	Private g_ReplParagraphID() As String
	Private g_ReplParagraphNo() As Integer
	Private g_ReplSentence() As Integer
	Private g_ReplEnable() As Boolean
	
	Private g_ReplHTMLJP() As String
	Private g_ReplHTMLCN() As String
	Private g_ReplHTMLUS() As String
	
	Private Sub cmdReplace_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdReplace.Click
		
		'扫描一遍，获取哪些选择了，那些没有。根据颜色判定
		Dim I As Integer
		Dim nSelected As Integer
		
		Dim doc As mshtml.IHTMLDocument2
		Dim ObjID As mshtml.IHTMLInputElement
		
		doc = wbMain.Document.DomDocument
		nSelected = 0
		
		If UBound(g_ReplParagraphID) = 0 Then Exit Sub
		
		'UPGRADE_WARNING: Lower bound of array g_ReplEnable was changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="0F1C9BE1-AF9D-476E-83B1-17D43BECFF20"'
		ReDim g_ReplEnable(UBound(g_ReplParagraphID))
		For I = 1 To UBound(g_ReplParagraphID)
			
			ObjID = doc.All.item("ID_" & I)
			If ObjID.Value = "Y" Then
				nSelected = nSelected + 1
				g_ReplEnable(I) = True
			Else
				g_ReplEnable(I) = False
			End If
		Next I
		
		Dim sPrompt As String
		
		sPrompt = "总计 " & UBound(g_ReplParagraphID) & vbCrLf
		sPrompt = sPrompt & "选择 " & nSelected & vbCrLf & vbCrLf
		sPrompt = sPrompt & "确认替换? "
		
		If MsgBoxResult.OK <> MsgBox(sPrompt, MsgBoxStyle.OKCancel) Then Exit Sub
		info("正在执行替换请等待...")
		
		Dim nLang As Integer
		nLang = cboLang.SelectedIndex
		
		'UPGRADE_WARNING: Arrays in structure inf may need to be initialized before they can be used. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="814DF224-76BD-4BB4-BFFB-EA359CB9FC48"'
		Dim inf As SCRIPTINFO
		
		Dim txtReplaceFrom As String
		Dim txtReplaceTo As String
		
		txtReplaceFrom = Tool_StripCrLf((txtReplFrom.Text))
		txtReplaceTo = Tool_StripCrLf((txtReplTo.Text))
		
		Dim txt() As String
		Dim filename As String
		
		For I = 1 To UBound(g_ReplParagraphID)
			If g_ReplEnable(I) Then
				'UPGRADE_WARNING: Couldn't resolve default property of object inf. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
				inf = Script_GetInfo(g_ReplParagraphID(I))
				
				filename = ""
				Select Case nLang
					Case LANGUAGE_JP
						filename = g_Dir & "\jp-text\" & g_ReplParagraphID(I) & ".txt"
						inf.JpText(g_ReplSentence(I)) = Replace(inf.JpText(g_ReplSentence(I)), txtReplaceFrom, txtReplaceTo,  ,  , CompareMethod.Binary)
					Case LANGUAGE_CN
						filename = g_Dir & "\cn-text\" & g_ReplParagraphID(I) & ".txt"
						inf.CnText(g_ReplSentence(I)) = Replace(inf.CnText(g_ReplSentence(I)), txtReplaceFrom, txtReplaceTo,  ,  , CompareMethod.Binary)
					Case LANGUAGE_US
						filename = g_Dir & "\us-text\" & g_ReplParagraphID(I) & ".txt"
						inf.UsText(g_ReplSentence(I)) = Replace(inf.UsText(g_ReplSentence(I)), txtReplaceFrom, txtReplaceTo,  ,  , CompareMethod.Binary)
				End Select
				
				Script_Save(g_ReplParagraphID(I), inf, nLang)
			End If
		Next I
		
		info("替换完毕")
	End Sub
	
	Private Sub cmdSearch_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdSearch.Click
		
		Dim sSearch As String
		Dim sOldCaption As String
		
		sOldCaption = cmdSearch.Text
		
		sSearch = Trim(txtReplFrom.Text)
		sSearch = Tool_StripCrLf(sSearch)
		
		txtReplFrom.Text = sSearch
		
		If cmdSearch.Text = "stop" Then
			g_SearchStop = True
			Exit Sub
		End If
		
		If sSearch = "" Then Exit Sub
		
		g_SearchStop = False
		cmdSearch.Text = "stop"
		
		txtInfo.Text = ""
		info("查找全文包含[" & sSearch & "]中，pls wait ...")
		'UPGRADE_WARNING: Navigate2 was upgraded to Navigate and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
		wbMain.Navigate(New System.URI("about:blank"))
		
		Dim I, j As Integer
		'UPGRADE_WARNING: Arrays in structure inf may need to be initialized before they can be used. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="814DF224-76BD-4BB4-BFFB-EA359CB9FC48"'
		Dim inf As SCRIPTINFO
		
		
		ReDim g_ReplParagraphID(0)
		ReDim g_ReplParagraphNo(0)
		ReDim g_ReplSentence(0)
		ReDim g_ReplHTMLJP(0)
		ReDim g_ReplHTMLCN(0)
		ReDim g_ReplHTMLUS(0)
		
		Dim nLang As Integer
		nLang = cboLang.SelectedIndex
		Dim nFrom As Integer
		Dim nTo As Integer
		
		nFrom = Val(txtSearchFrom.Text)
		If nFrom < 1 Then nFrom = 1
		txtSearchFrom.Text = CStr(nFrom)
		
		nTo = Val(txtSearchTo.Text)
		If nTo > UBound(g_CacheInfo) Then nTo = UBound(g_CacheInfo)
		txtSearchTo.Text = CStr(nTo)
		
		
		Dim sID As String
		Dim txtAll As String
		Dim txt() As String
		Dim tmpids() As String
		Dim tmplen() As Integer
		Dim sTmp As String
		Dim sText(2) As String '3种语言的预览文本，没有加颜色
		For I = nFrom To nTo
			
			
			sID = g_CacheInfo(I).sID
			If nLang = 0 Then
				txtAll = Tool_LoadTextFile(g_Dir & "\jp-text\" & sID & ".txt")
			ElseIf nLang = 1 Then 
				txtAll = Tool_LoadTextFile(g_Dir & "\cn-text\" & sID & ".txt")
			ElseIf nLang = 2 Then 
				txtAll = Tool_LoadTextFile(g_Dir & "\us-text\" & sID & ".txt")
			Else
				MsgBox("err")
				Exit Sub
			End If
			
			If g_TextFormat = 1 Then
				Script_DecodeText(txtAll, txt, tmpids)
			Else
				Script_DecodeText_Format2(txtAll, txt, tmpids, tmplen)
			End If
			
			For j = 1 To UBound(txt)
				
				
				If VB.Right(txt(j), Len("{enter}")) = "{enter}" Then
					info("found {enter} bug: " & I & " - " & sID & " - " & j)
				End If
				
				sTmp = Callback_PlainText(txt(j), LANGUAGE_CN)
				
				If InStr(1, sTmp, sSearch, CompareMethod.Binary) > 0 Then
					'If sSearch = Left(sText, Len(sSearch)) Then
					
					'UPGRADE_WARNING: Couldn't resolve default property of object inf. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
					inf = Script_GetInfo(sID)
					sText(0) = Callback_PlainText(inf.JpText(j), LANGUAGE_JP)
					sText(0) = Replace(sText(0), vbCrLf, "<br>")
					sText(1) = Callback_PlainText(inf.CnText(j), LANGUAGE_CN)
					sText(1) = Replace(sText(1), vbCrLf, "<br>")
					sText(2) = Callback_PlainText(inf.UsText(j), LANGUAGE_US)
					sText(2) = Replace(sText(2), vbCrLf, "<br>")
					
					'info "#" & I & " - " & sID & " - " & j
					info(sID & " - " & j)
					
					ReDim Preserve g_ReplParagraphID(UBound(g_ReplParagraphID) + 1)
					ReDim Preserve g_ReplParagraphNo(UBound(g_ReplParagraphID) + 1)
					ReDim Preserve g_ReplSentence(UBound(g_ReplParagraphID) + 1)
					ReDim Preserve g_ReplHTMLJP(UBound(g_ReplParagraphID) + 1)
					ReDim Preserve g_ReplHTMLCN(UBound(g_ReplParagraphID) + 1)
					ReDim Preserve g_ReplHTMLUS(UBound(g_ReplParagraphID) + 1)
					
					g_ReplParagraphID(UBound(g_ReplParagraphID)) = sID
					g_ReplParagraphNo(UBound(g_ReplParagraphID)) = I
					g_ReplSentence(UBound(g_ReplParagraphID)) = j
					
					'加颜色
					sText(nLang) = Replace(sText(nLang), sSearch, "<font color=red>" & sSearch & "</font>")
					
					g_ReplHTMLJP(UBound(g_ReplParagraphID)) = sText(0)
					g_ReplHTMLCN(UBound(g_ReplParagraphID)) = sText(1)
					g_ReplHTMLUS(UBound(g_ReplParagraphID)) = sText(2)
					
					If g_ReplHTMLJP(UBound(g_ReplParagraphID)) = "" Then g_ReplHTMLJP(UBound(g_ReplParagraphID)) = "&nbsp;"
					If g_ReplHTMLCN(UBound(g_ReplParagraphID)) = "" Then g_ReplHTMLCN(UBound(g_ReplParagraphID)) = "&nbsp;"
					If g_ReplHTMLUS(UBound(g_ReplParagraphID)) = "" Then g_ReplHTMLUS(UBound(g_ReplParagraphID)) = "&nbsp;"
					
				End If
				
				
				System.Windows.Forms.Application.DoEvents()
				
				If g_SearchStop = True Then
					info("stop. last position is " & sID)
					GoTo Search_STOP
				End If
				
			Next j
			
			
			System.Windows.Forms.Application.DoEvents()
			
		Next I
		
Search_STOP: 
		
		Dim sAll As String
		Dim sHTMLFile As String
		Dim tpl_body, tpl_row As String '读入模板
		Dim tpl_js As String
		Dim tpl_dir As String
		
		tpl_dir = g_Dir & "\template\search"
		
		tpl_body = Tool_LoadTextFile(tpl_dir & "\body.html")
		TemplateSetColSize(tpl_body)
		
		tpl_row = Tool_LoadTextFile(tpl_dir & "\row.html")
		TemplateSetColSize(tpl_row)
		
		Dim rows As String
		Dim sItem As String
		Dim sHidden As String
		
		rows = ""
		sHidden = ""
		For I = 1 To UBound(g_ReplParagraphID)
			sItem = Replace(tpl_row, "TEMPLATE_SEARCH_ID", CStr(I))
			'sItem = Replace(sItem, "TEMPLATE_PARAGRAPH_NO", g_ReplParagraphNo(I))
			sItem = Replace(sItem, "TEMPLATE_PARAGRAPH_ID", g_ReplParagraphID(I))
			sItem = Replace(sItem, "TEMPLATE_SENTENCE", CStr(g_ReplSentence(I)))
			
			sItem = Replace(sItem, "TEMPLATE_JP", g_ReplHTMLJP(I))
			sItem = Replace(sItem, "TEMPLATE_CN", g_ReplHTMLCN(I))
			sItem = Replace(sItem, "TEMPLATE_US", g_ReplHTMLUS(I))
			rows = rows & sItem
			
			sHidden = sHidden & "<input type=hidden name=" & """" & "ID_" & I & """" & " value=" & """" & "Y" & """" & ">" & vbCrLf
		Next I
		
		sAll = Replace(tpl_body, "TEMPLATE_MAX", CStr(UBound(g_ReplParagraphID)))
		sAll = Replace(sAll, "<!-- TEMPLATE_HIDDEN -->", sHidden)
		sAll = Replace(sAll, "<!-- TEMPLATE_ROWS -->", rows)
		
		
		sAll = sAll & vbCrLf
        'sHTMLFile = g_Dir & "\template\temp\preview.html"
        sHTMLFile = My.Application.GetEnvironmentVariable("TEMP") & "\preview.html"
		Tool_WriteTextFile(sHTMLFile, sAll)
		
		'UPGRADE_WARNING: Navigate2 was upgraded to Navigate and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
		wbMain.Navigate(New System.URI(sHTMLFile))
		
		
		cmdSearch.Text = sOldCaption
		
		info("总计结果 " & UBound(g_ReplParagraphID))
	End Sub
	
	
	
	Private Sub cmdSearch1Line_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdSearch1Line.Click
		Dim sSearch As String
		Dim sOldCaption As String
		
		sOldCaption = cmdSearch1Line.Text
		
		sSearch = Trim(txtReplFrom.Text)
		sSearch = Tool_StripCrLf(sSearch)
		
		txtReplFrom.Text = sSearch
		
		If cmdSearch1Line.Text = "stop" Then
			g_SearchStop = True
			Exit Sub
		End If
		
		If sSearch = "" Then Exit Sub
		
		g_SearchStop = False
		cmdSearch1Line.Text = "stop"
		
		txtInfo.Text = ""
		info("查找第一行为[" & sSearch & "] 中，pls wait ...")
		'UPGRADE_WARNING: Navigate2 was upgraded to Navigate and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
		wbMain.Navigate(New System.URI("about:blank"))
		
		Dim I, j As Integer
		'UPGRADE_WARNING: Arrays in structure inf may need to be initialized before they can be used. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="814DF224-76BD-4BB4-BFFB-EA359CB9FC48"'
		Dim inf As SCRIPTINFO
		
		
		ReDim g_ReplParagraphID(0)
		ReDim g_ReplParagraphNo(0)
		ReDim g_ReplSentence(0)
		ReDim g_ReplHTMLJP(0)
		ReDim g_ReplHTMLCN(0)
		ReDim g_ReplHTMLUS(0)
		
		Dim nLang As Integer
		nLang = cboLang.SelectedIndex
		Dim nFrom As Integer
		Dim nTo As Integer
		
		nFrom = Val(txtSearchFrom.Text)
		If nFrom < 1 Then nFrom = 1
		txtSearchFrom.Text = CStr(nFrom)
		
		nTo = Val(txtSearchTo.Text)
		If nTo > UBound(g_CacheInfo) Then nTo = UBound(g_CacheInfo)
		txtSearchTo.Text = CStr(nTo)
		
		'********************
		
		sSearch = sSearch & "<br>"
		
		Dim sID As String
		Dim sText(2) As String
		Dim sTmp As String '3种语言的预览文本，没有加颜色
		For I = nFrom To nTo
			
			
			sID = g_CacheInfo(I).sID
			'UPGRADE_WARNING: Couldn't resolve default property of object inf. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
			inf = Script_GetInfo(sID)
			
			
			For j = 1 To inf.JpCount
				
				
				
				sText(0) = Callback_Preview(inf.JpText(j), LANGUAGE_JP)
				sText(1) = Callback_Preview(inf.CnText(j), LANGUAGE_CN)
				sText(2) = Callback_Preview(inf.UsText(j), LANGUAGE_US)
				
				sText(nLang) = Replace(sText(nLang), "<b>", "")
				sText(nLang) = Replace(sText(nLang), "</b>", "")
				
				If InStr(1, sText(nLang), sSearch, CompareMethod.Binary) = 1 Then
					'If sSearch = Left(sText, Len(sSearch)) Then
					'info "# " & I & " - " & sID & " - " & j
					info(sID & " - " & j)
					
					ReDim Preserve g_ReplParagraphID(UBound(g_ReplParagraphID) + 1)
					ReDim Preserve g_ReplParagraphNo(UBound(g_ReplParagraphID) + 1)
					ReDim Preserve g_ReplSentence(UBound(g_ReplParagraphID) + 1)
					ReDim Preserve g_ReplHTMLJP(UBound(g_ReplParagraphID) + 1)
					ReDim Preserve g_ReplHTMLCN(UBound(g_ReplParagraphID) + 1)
					ReDim Preserve g_ReplHTMLUS(UBound(g_ReplParagraphID) + 1)
					
					g_ReplParagraphID(UBound(g_ReplParagraphID)) = sID
					g_ReplParagraphNo(UBound(g_ReplParagraphID)) = I
					g_ReplSentence(UBound(g_ReplParagraphID)) = j
					
					g_ReplHTMLJP(UBound(g_ReplParagraphID)) = sText(0)
					g_ReplHTMLCN(UBound(g_ReplParagraphID)) = sText(1)
					g_ReplHTMLUS(UBound(g_ReplParagraphID)) = sText(2)
					
					If g_ReplHTMLJP(UBound(g_ReplParagraphID)) = "" Then g_ReplHTMLJP(UBound(g_ReplParagraphID)) = "&nbsp;"
					If g_ReplHTMLCN(UBound(g_ReplParagraphID)) = "" Then g_ReplHTMLCN(UBound(g_ReplParagraphID)) = "&nbsp;"
					If g_ReplHTMLUS(UBound(g_ReplParagraphID)) = "" Then g_ReplHTMLUS(UBound(g_ReplParagraphID)) = "&nbsp;"
					
				End If
				
				
				System.Windows.Forms.Application.DoEvents()
				
				If g_SearchStop = True Then
					info("stop. last position is " & sID)
					GoTo Search_STOP
				End If
				
			Next j
			
			
			System.Windows.Forms.Application.DoEvents()
			
		Next I
		
Search_STOP: 
		
		Dim sAll As String
		Dim sHTMLFile As String
		Dim tpl_body, tpl_row As String '读入模板
		Dim tpl_js As String
		Dim tpl_dir As String
		
		tpl_dir = g_Dir & "\template\search"
		
		tpl_body = Tool_LoadTextFile(tpl_dir & "\body.html")
		tpl_row = Tool_LoadTextFile(tpl_dir & "\row.html")
		tpl_js = Tool_LoadTextFile(tpl_dir & "\js.html")
		
		Dim rows As String
		Dim sItem As String
		Dim sHidden As String
		
		rows = ""
		sHidden = ""
		For I = 1 To UBound(g_ReplParagraphID)
			sItem = Replace(tpl_row, "TEMPLATE_SEARCH_ID", CStr(I))
			'sItem = Replace(sItem, "TEMPLATE_PARAGRAPH_NO", g_ReplParagraphNo(I))
			sItem = Replace(sItem, "TEMPLATE_PARAGRAPH_ID", g_ReplParagraphID(I))
			sItem = Replace(sItem, "TEMPLATE_SENTENCE", CStr(g_ReplSentence(I)))
			
			sItem = Replace(sItem, "TEMPLATE_JP", g_ReplHTMLJP(I))
			sItem = Replace(sItem, "TEMPLATE_CN", g_ReplHTMLCN(I))
			sItem = Replace(sItem, "TEMPLATE_US", g_ReplHTMLUS(I))
			rows = rows & sItem
			
			sHidden = sHidden & "<input type=hidden name=" & """" & "ID_" & I & """" & " value=" & """" & "Y" & """" & ">" & vbCrLf
		Next I
		
		sAll = Replace(tpl_body, "TEMPLATE_MAX", CStr(UBound(g_ReplParagraphID)))
		sAll = Replace(sAll, "<!-- TEMPLATE_HIDDEN -->", sHidden)
		sAll = Replace(sAll, "<!-- TEMPLATE_ROWS -->", rows)
		
		
		sAll = sAll & vbCrLf & tpl_js
        'sHTMLFile = g_Dir & "\template\temp\preview.html"
        sHTMLFile = My.Application.GetEnvironmentVariable("TEMP") & "\preview.html"
		Tool_WriteTextFile(sHTMLFile, sAll)
		
		'UPGRADE_WARNING: Navigate2 was upgraded to Navigate and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
		wbMain.Navigate(New System.URI(sHTMLFile))
		
		
		cmdSearch1Line.Text = sOldCaption
		
		info("总计结果 " & UBound(g_ReplParagraphID))
		
	End Sub
	
	'UPGRADE_WARNING: Form event Form4.Activate has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"'
	Private Sub Form4_Activated(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Activated
		txtReplFrom.Focus()
	End Sub
	
	Private Sub txtReplFrom_KeyDown(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyEventArgs) Handles txtReplFrom.KeyDown
		Dim KeyCode As Short = eventArgs.KeyCode
		Dim Shift As Short = eventArgs.KeyData \ &H10000
		If KeyCode = 13 Then cmdSearch_Click(cmdSearch, New System.EventArgs())
	End Sub
	
	
	Private Sub Form4_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
		
		cboLang.Items.Clear()
		cboLang.Items.Add("日文")
		cboLang.Items.Add("中文")
		cboLang.Items.Add("英文")
		cboLang.SelectedIndex = 1
		
		info("日文输入法乱码的解决")
		info("1. 打开日文输入法，在记事本输入要查找的词")
		info("2. 关闭日文输入法")
		info("3. 把输入的词复制&粘贴到程序的查找框")
		
		info("")
		info("ps: 推荐使用ultra edit等软件的批量查找、替换功能")
		
		Me.BackColor = Form2.Grid1.BackColor
		lblTo.BackColor = Form2.Grid1.BackColor
		txtInfo.BackColor = Form2.Grid1.BackColor
		
		txtSearchFrom.Text = "1"
		txtSearchTo.Text = CStr(UBound(g_CacheInfo))
		
		ReDim g_ReplParagraphID(0)
		'UPGRADE_WARNING: Navigate2 was upgraded to Navigate and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
		wbMain.Navigate(New System.URI("about:blank"))
		System.Windows.Forms.Application.DoEvents()
		
	End Sub
	
	'UPGRADE_WARNING: Event Form4.Resize may fire when form is initialized. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="88B12AE1-6DE0-48A0-86F1-60C0686C026A"'
	Private Sub Form4_Resize(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Resize
		Dim w As Integer
		Dim h As Integer
		
		w = VB6.PixelsToTwipsX(Me.Width) - 200
		If w <= 1000 Then w = 1000
		
		h = VB6.PixelsToTwipsY(Me.Height) * 0.7
		If h <= 1000 Then h = 1000
		
		wbMain.Width = VB6.TwipsToPixelsX(w)
		wbMain.Left = VB6.TwipsToPixelsX(1)
		
		wbMain.Height = VB6.TwipsToPixelsY(h)
		
		cboLang.Top = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(wbMain.Height) + VB6.PixelsToTwipsY(wbMain.Top) + 200)
		txtSearchFrom.Top = cboLang.Top
		txtSearchTo.Top = cboLang.Top
		lblTo.Top = cboLang.Top
		
		txtReplFrom.Top = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(cboLang.Height) + VB6.PixelsToTwipsY(cboLang.Top) + 200)
		txtReplTo.Top = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(txtReplFrom.Height) + VB6.PixelsToTwipsY(txtReplFrom.Top) + 200)
		cmdSearch.Top = txtReplFrom.Top
		cmdSearch1Line.Top = txtReplFrom.Top
		cmdReplace.Top = txtReplTo.Top
		txtInfo.Top = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(wbMain.Height) + VB6.PixelsToTwipsY(wbMain.Top) + 200)
		
	End Sub
	
	Public Sub info(ByRef s As Object)
		'UPGRADE_WARNING: Couldn't resolve default property of object s. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		txtInfo.Text = txtInfo.Text & s & vbCrLf
		txtInfo.SelectionStart = Len(txtInfo.Text)
		txtInfo.Refresh()
		System.Windows.Forms.Application.DoEvents()
	End Sub
	
	Private Sub mnuExport_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuExport.Click
		
		Dim sSearch As String
		sSearch = Trim(txtReplFrom.Text)
		sSearch = Tool_StripCrLf(sSearch)
		
		txtReplFrom.Text = sSearch
		
		If sSearch = "" Then Exit Sub
		
		txtInfo.Text = ""
		info("提取人名 [" & sSearch & "] 中，pls wait ...")
		
		Dim nTotalJPCount As Integer
		
		nTotalJPCount = 0
		
		Dim sDir As String
		Dim strExpFile As String
		sDir = g_Dir & "\export"
		'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
		If Dir(sDir, FileAttribute.Directory) = "" Then MkDir(sDir)
		'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
		If Dir(sDir & "\jp-text", FileAttribute.Directory) = "" Then MkDir(sDir & "\jp-text")
		'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
		If Dir(sDir & "\cn-text", FileAttribute.Directory) = "" Then MkDir(sDir & "\cn-text")
		'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
		If Dir(sDir & "\us-text", FileAttribute.Directory) = "" Then MkDir(sDir & "\us-text")
		
		Dim fso As New Scripting.FileSystemObject
		Dim oFileJP As Scripting.TextStream
		Dim oFileCN As Scripting.TextStream
		Dim oFileUS As Scripting.TextStream
		
		oFileJP = fso.OpenTextFile(sDir & "\jp-text\" & sSearch & ".txt", Scripting.IOMode.ForWriting, True, Scripting.Tristate.TristateFalse)
		oFileCN = fso.OpenTextFile(sDir & "\cn-text\" & sSearch & ".txt", Scripting.IOMode.ForWriting, True, Scripting.Tristate.TristateFalse)
		oFileUS = fso.OpenTextFile(sDir & "\us-text\" & sSearch & ".txt", Scripting.IOMode.ForWriting, True, Scripting.Tristate.TristateFalse)
		
		Dim I, j As Integer
		'UPGRADE_WARNING: Arrays in structure inf may need to be initialized before they can be used. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="814DF224-76BD-4BB4-BFFB-EA359CB9FC48"'
		Dim inf As SCRIPTINFO
		
		
		ReDim g_ReplParagraphID(0)
		ReDim g_ReplParagraphNo(0)
		ReDim g_ReplSentence(0)
		ReDim g_ReplHTMLJP(0)
		ReDim g_ReplHTMLCN(0)
		ReDim g_ReplHTMLUS(0)
		
		Dim nLang As Integer
		nLang = cboLang.SelectedIndex
		Dim nFrom As Integer
		Dim nTo As Integer
		
		nFrom = Val(txtSearchFrom.Text)
		If nFrom < 1 Then nFrom = 1
		txtSearchFrom.Text = CStr(nFrom)
		
		nTo = Val(txtSearchTo.Text)
		If nTo > UBound(g_CacheInfo) Then nTo = UBound(g_CacheInfo)
		txtSearchTo.Text = CStr(nTo)
		
		'********************
		
		sSearch = sSearch & "<br>"
		
		Dim sID As String
		Dim sText(2) As String
		Dim sTmp As String
		Dim sID_File As String '3种语言的预览文本，没有加颜色
		For I = nFrom To nTo
			
			
			sID = g_CacheInfo(I).sID
			'UPGRADE_WARNING: Couldn't resolve default property of object inf. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
			inf = Script_GetInfo(sID)
			
			
			For j = 1 To inf.JpCount
				
				
				
				sText(0) = Callback_Preview(inf.JpText(j), LANGUAGE_JP)
				sText(1) = Callback_Preview(inf.CnText(j), LANGUAGE_CN)
				sText(2) = Callback_Preview(inf.UsText(j), LANGUAGE_US)
				
				sText(nLang) = Replace(sText(nLang), "<b>", "")
				sText(nLang) = Replace(sText(nLang), "</b>", "")
				
				If InStr(1, sText(nLang), sSearch, CompareMethod.Binary) = 1 Then
					'info "# " & I & " - " & sID & " - " & j
					
					nTotalJPCount = nTotalJPCount + Tool_GetWords(inf.JpText(j), LANGUAGE_JP)
					
					
					sID_File = sID & "-" & j
					
					oFileJP.WriteLine("#### " & sID_File & " ####")
					oFileCN.WriteLine("#### " & sID_File & " ####")
					oFileUS.WriteLine("#### " & sID_File & " ####")
					oFileJP.WriteLine(inf.JpText(j))
					oFileCN.WriteLine(inf.CnText(j))
					oFileUS.WriteLine(inf.UsText(j))
					oFileJP.WriteLine("")
					oFileCN.WriteLine("")
					oFileUS.WriteLine("")
					
				End If
				
				
				System.Windows.Forms.Application.DoEvents()
			Next j
			
			System.Windows.Forms.Application.DoEvents()
			
		Next I
		
		oFileCN.Close()
		oFileJP.Close()
		oFileUS.Close()
		
		info("done, total " & nTotalJPCount & " words.")
		
	End Sub
End Class