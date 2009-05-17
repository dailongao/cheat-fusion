Option Strict Off
Option Explicit On
Module tool_script
	
	Public Const LANGUAGE_JP As Short = 0
	Public Const LANGUAGE_CN As Short = 1
	Public Const LANGUAGE_US As Short = 2
	
	Public g_TextFormat As Integer
	'1 = agemo文本格式
	'2 = 地址，长度，文本 这种格式
	
	
	Public g_len_id As String
	Public g_len_j As String
	Public g_len_c As String
	Public g_len_u As String
	
	Public g_hide_repeat As Boolean
	
	Public Structure SCRIPTINFO
		'data
		Dim ID As String 'ID (比如用 ISO offset 代替)
		Dim JpText() As String
		Dim UsText() As String
		Dim CnText() As String
		Dim CnTextAlter() As String
		Dim dispCount As Integer
		Dim JpCount As Integer '一段中，日文多少句
		Dim UsCount As Integer '一段中，英文多少句
		Dim CnCount As Integer '一段中，译文多少句
		Dim CnAlterCount As Integer '一段中，修改译文多少句
		Dim JpTextAll As String
		Dim CnTextAll As String
		Dim CnTextAlterAll As String
		Dim UsTextAll As String
		Dim User As String '译者
		Dim Memo As String '注释
		'对于这种格式用：地址,句子长度,文本
		Dim Address() As String 'address 也是描述
		Dim Maxlen() As Integer
		'Event专用, event的 address = arrid _ arrnum
		Dim arrID() As String
		Dim arrNum() As Integer
		Dim isEventMode As Boolean
	End Structure
	
	Public g_Dir As String 'script 路径，也就是工作路径
	
	
	Public Sub TemplateSetColSize(ByRef sText As String)
		sText = Replace(sText, "WIDTH_ID", g_len_id)
		sText = Replace(sText, "WIDTH_JP", g_len_j)
		sText = Replace(sText, "WIDTH_CN", g_len_c)
		sText = Replace(sText, "WIDTH_US", g_len_u)
	End Sub
	
	Private Function MarkTextDiffHTML_Internal(ByRef src As String, ByRef dst As String) As Boolean
		'返回值表示，是否已经标记了。如果dst比原文短，则需要对换dst src重新做一次
		
		Dim ret As String
		Dim I As Integer
		Dim s As String
		Dim d As String
		
		MarkTextDiffHTML_Internal = True
		
		Dim nDiffStart, nDiffEnd As Integer
		
		nDiffStart = Len(src) + 1
		
		'difference start flag
		For I = 1 To Len(src)
			s = Mid(src, I, 1)
			d = Mid(dst, I, 1)
			
			If s <> d Then
				nDiffStart = I
				Exit For
			Else
			End If
		Next I
		
		
		'difference end flag
		nDiffEnd = Len(dst)
		For I = 1 To Len(dst)
			If Len(src) - I + 1 < 1 Then Exit For
			s = Mid(src, Len(src) - I + 1, 1)
			d = Mid(dst, Len(dst) - I + 1, 1)
			
			If s <> d Then
				nDiffEnd = Len(dst) - I + 1
				Exit For
			Else
			End If
		Next I
		
		If nDiffEnd < nDiffStart Then
			'dst = "<font color=red>(删了一些词，见对侧的红色部分)</font><br>" & dst
			MarkTextDiffHTML_Internal = False
		Else
			dst = Left(dst, nDiffStart - 1) & "<font color=#d00000>" & Mid(dst, nDiffStart, nDiffEnd - nDiffStart + 1) & "</font>" & Mid(dst, nDiffEnd + 1)
		End If
		
	End Function
	
	Public Function MarkTextDiffHTML(ByRef src As String, ByRef dst As String) As Object
		'src ,dst 输入输出，
		
		'返回HTML标记的好的text，注意，双测都有可能标记
		
		If src = dst Then
			dst = ""
			Exit Function
		End If
		
		src = Replace(src, "<br>", "∝")
		dst = Replace(dst, "<br>", "∝")
		
		If False = MarkTextDiffHTML_Internal(src, dst) Then
			MarkTextDiffHTML_Internal(dst, src)
		End If
		
		src = Replace(src, "∝", "<br>")
		dst = Replace(dst, "∝", "<br>")
		
		
	End Function
	Public Function Script_GetInfo(ByRef strID As String) As SCRIPTINFO
		
		If g_TextFormat = 2 Then
			Script_GetInfo = Script_GetInfo_Format2(strID)
		Else
			Script_GetInfo = Script_GetInfo_Format1(strID)
		End If
	End Function
	
	
	Public Function Script_GetInfo_Format1(ByRef strID As String) As SCRIPTINFO
		
		Script_GetInfo_Format1 = Script_GetInfo_Format1_Ex(g_Dir, strID)
		
	End Function
	
	Private Sub Event_InitData(ByVal sEventFile As String, ByRef nCount As Integer, ByRef arrID() As String, ByRef arrNum() As Integer)
		Dim barr() As Byte
		Dim I As Integer
		
		
		
		barr = Tool_LoadBin(sEventFile)
		
		nCount = barr(1) + barr(2) * 256
		
		If nCount Mod 4 <> 0 Or nCount <= 0 Then
			nCount = 0
			MsgBox("Error in event file " & vbCrLf & sEventFile)
			Exit Sub
		End If
		
		nCount = nCount / 4
		'UPGRADE_WARNING: Lower bound of array arrID was changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="0F1C9BE1-AF9D-476E-83B1-17D43BECFF20"'
		ReDim arrID(nCount)
		'UPGRADE_WARNING: Lower bound of array arrNum was changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="0F1C9BE1-AF9D-476E-83B1-17D43BECFF20"'
		ReDim arrNum(nCount)
		For I = 1 To nCount
			'arrID(I) = ToDec(barr(16 + (I - 1) * 4 + 1) + barr(16 + (I - 1) * 4 + 2) * 256, 3)  ' 3 for xg
			arrID(I) = ToDec(barr(16 + (I - 1) * 4 + 1) + barr(16 + (I - 1) * 4 + 2) * 256, 4) ' 4 for vp
			'arrNum(I) = barr(16 + (I - 1) * 4 + 3) + barr(16 + (I - 1) * 4 + 4) * 256 + 1        '+ 1 for xg
			arrNum(I) = barr(16 + (I - 1) * 4 + 3) + barr(16 + (I - 1) * 4 + 4) * 256 - 13 '- 13 for vp
		Next I
	End Sub
	
	Public Function Script_GetEventInfo(ByRef sEventID As String) As SCRIPTINFO
		
		'UPGRADE_WARNING: Arrays in structure info may need to be initialized before they can be used. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="814DF224-76BD-4BB4-BFFB-EA359CB9FC48"'
		Dim info As SCRIPTINFO
		'UPGRADE_WARNING: Arrays in structure info_tmp may need to be initialized before they can be used. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="814DF224-76BD-4BB4-BFFB-EA359CB9FC48"'
		Dim info_tmp As SCRIPTINFO
		
		Dim strID As String
		
		info.isEventMode = True
		
		Event_InitData(g_Dir & "\event\" & sEventID & ".evt", info.JpCount, info.arrID, info.arrNum)
		info.CnCount = info.JpCount
		info.UsCount = info.JpCount
		info.ID = sEventID
		
        Dim fso As New Scripting.FileSystemObject
		Dim objFile As Scripting.TextStream
		Dim strFile As String
		Dim I As Integer
		
		Dim sTemp() As String
		
		ReDim info.CnText(info.JpCount)
		ReDim info.JpText(info.JpCount)
		ReDim info.UsText(info.JpCount)
		ReDim info.CnTextAlter(info.JpCount)
		ReDim info.Address(info.JpCount)
		
		
		For I = 1 To info.JpCount
			
			strID = info.arrID(I)
			'read & decode jp, us, cn txt
			'############# JP
			strFile = g_Dir & "\jp-text\" & strID & ".txt"
			info_tmp.JpTextAll = Tool_LoadTextFile(strFile)
			Script_DecodeText(info_tmp.JpTextAll, info_tmp.JpText, info_tmp.Address)
			info.JpText(I) = info_tmp.JpText(info.arrNum(I))
			
			'############# CN
			strFile = g_Dir & "\cn-text\" & strID & ".txt"
			info_tmp.CnTextAll = Tool_LoadTextFile(strFile)
			Script_DecodeText(info_tmp.CnTextAll, info_tmp.CnText, sTemp)
			info.CnText(I) = info_tmp.CnText(info.arrNum(I))
			
			'############# alter
			strFile = g_Dir & "\cn-compare\" & strID & ".txt"
			info_tmp.CnTextAlterAll = Tool_LoadTextFile(strFile)
			Script_DecodeText(info_tmp.CnTextAlterAll, info_tmp.CnTextAlter, sTemp)
			ReDim Preserve info_tmp.CnTextAlter(UBound(info_tmp.JpText))
			info.CnTextAlter(I) = info_tmp.CnTextAlter(info.arrNum(I))
			
			
			'############# US
			'注意的是，可能极特殊情况下美版要比日版句数多, 也可能根本没有美版的
			strFile = g_Dir & "\us-text\" & strID & ".txt"
			info_tmp.UsTextAll = Tool_LoadTextFile(strFile)
			Script_DecodeText(info_tmp.UsTextAll, info_tmp.UsText, sTemp)
			ReDim Preserve info_tmp.UsText(UBound(info_tmp.JpText))
			info.UsText(I) = info_tmp.UsText(info.arrNum(I))
			
			info.Address(I) = strID & " " & info.arrNum(I)
		Next I
		
		'UPGRADE_WARNING: Couldn't resolve default property of object Script_GetEventInfo. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		Script_GetEventInfo = info
		
		
	End Function
	
	Public Function Script_GetInfo_Format1_Ex(ByRef strBaseDir As String, ByRef strID As String) As SCRIPTINFO
		
		'获取信息 *
		
		'UPGRADE_WARNING: Arrays in structure info may need to be initialized before they can be used. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="814DF224-76BD-4BB4-BFFB-EA359CB9FC48"'
		Dim info As SCRIPTINFO
		
		info.ID = strID
		Dim sTemp() As String
		
		Dim fso As New Scripting.FileSystemObject
		Dim objFile As Scripting.TextStream
		Dim strFile As String
		
		Dim nReserverd As Integer '{repeat:   {dummy:
		Dim I As Integer '计算已经翻译了多少句
		
		nReserverd = 0
		'read & decode jp, us, cn txt
		'############# JP
		strFile = strBaseDir & "\jp-text\" & strID & ".txt"
		info.JpTextAll = Tool_LoadTextFile(strFile)
		Script_DecodeText(info.JpTextAll, info.JpText, info.Address)
		info.JpCount = UBound(info.JpText)
		'for chrono cross
		For I = 0 To UBound(info.JpText)
			If Left(info.JpText(I), Len("{repeat:")) = "{repeat:" Or Left(info.JpText(I), Len("{dummy:")) = "{dummy:" Then nReserverd = nReserverd + 1
		Next I
		
		'新版重复文本采用符号,计算总句子有关,默认不把重复文本计算在内,要让他计算在内,所以注释掉
		'For I = 0 To UBound(info.JpText)
		'    If Left(info.JpText(I), 1) = "〓" Then nReserverd = nReserverd + 1
		'Next I
		
		
		For I = 1 To UBound(info.JpText)
			If info.JpText(I) = "" Then nReserverd = nReserverd + 1
		Next I
		
		info.dispCount = info.JpCount - nReserverd
		
		
		'############# CN
		strFile = strBaseDir & "\cn-text\" & strID & ".txt"
		info.CnTextAll = Tool_LoadTextFile(strFile)
		Script_DecodeText(info.CnTextAll, info.CnText, sTemp)
		ReDim Preserve info.CnText(info.JpCount)
		info.CnCount = 0
		For I = 0 To UBound(info.CnText)
			If info.CnText(I) <> "" And Left(info.JpText(I), Len("{repeat:")) <> "{repeat:" And Left(info.JpText(I), Len("{dummy:")) <> "{dummy:" Then info.CnCount = info.CnCount + 1
		Next I
		
		
		'############# alter
		strFile = strBaseDir & "\cn-compare\" & strID & ".txt"
		info.CnTextAlterAll = Tool_LoadTextFile(strFile)
		Script_DecodeText(info.CnTextAlterAll, info.CnTextAlter, sTemp)
		ReDim Preserve info.CnTextAlter(info.JpCount)
		
		If info.CnTextAlterAll = "" Then
			info.CnAlterCount = 0
		Else
			info.CnAlterCount = info.JpCount
		End If
		
		'############# US
		'注意的是，可能极特殊情况下美版要比日版句数多
		strFile = strBaseDir & "\us-text\" & strID & ".txt"
		info.UsTextAll = Tool_LoadTextFile(strFile)
		Script_DecodeText(info.UsTextAll, info.UsText, sTemp)
		If UBound(info.UsText) < UBound(info.JpText) Then
			ReDim Preserve info.UsText(info.JpCount)
		End If
		info.UsCount = UBound(info.UsText)
		
		info.isEventMode = False
		
		
		'done
		Script_MemoLoad(strID, info.User, info.Memo)
		'UPGRADE_WARNING: Couldn't resolve default property of object Script_GetInfo_Format1_Ex. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		Script_GetInfo_Format1_Ex = info
		
	End Function
	
	
	
	Public Function Script_GetInfo_Format2(ByRef strID As String) As SCRIPTINFO
		
		'获取信息 *
		
		'UPGRADE_WARNING: Arrays in structure info may need to be initialized before they can be used. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="814DF224-76BD-4BB4-BFFB-EA359CB9FC48"'
		Dim info As SCRIPTINFO
		
		info.ID = strID
		
		Dim fso As New Scripting.FileSystemObject
		Dim objFile As Scripting.TextStream
		Dim strFile As String
		
		Dim Address() As String
		Dim Maxlen() As Integer
		
		'read & decode jp, us, cn txt
		'############# JP
		strFile = g_Dir & "\jp-text\" & strID & ".txt"
		info.JpTextAll = Tool_LoadTextFile(strFile)
		Script_DecodeText_Format2(info.JpTextAll, info.JpText, info.Address, info.Maxlen)
		info.JpCount = UBound(info.JpText)
		info.dispCount = info.JpCount
		
		'############# CN
		strFile = g_Dir & "\cn-text\" & strID & ".txt"
		info.CnTextAll = Tool_LoadTextFile(strFile)
		Script_DecodeText_Format2(info.CnTextAll, info.CnText, Address, Maxlen)
		ReDim Preserve info.CnText(info.JpCount)
		Dim I As Integer '计算已经翻译了多少句
		info.CnCount = 0
		For I = 0 To UBound(info.CnText)
			If info.CnText(I) <> "" Then info.CnCount = info.CnCount + 1
		Next I
		
		'############# alter
		strFile = g_Dir & "\cn-compare\" & strID & ".txt"
		info.CnTextAlterAll = Tool_LoadTextFile(strFile)
		Script_DecodeText_Format2(info.CnTextAlterAll, info.CnTextAlter, Address, Maxlen)
		ReDim Preserve info.CnTextAlter(info.JpCount)
		
		If info.CnTextAlterAll = "" Then
			info.CnAlterCount = 0
		Else
			info.CnAlterCount = info.JpCount
		End If
		
		
		'############# US
		'注意的是，可能极特殊情况下美版要比日版句数多
		strFile = g_Dir & "\us-text\" & strID & ".txt"
		info.UsTextAll = Tool_LoadTextFile(strFile)
		'Script_DecodeText_Format2 info.UsTextAll, info.UsText
		Script_DecodeText_Format2(info.UsTextAll, info.UsText, Address, Maxlen)
		If UBound(info.UsText) < UBound(info.JpText) Then
			ReDim Preserve info.UsText(info.JpCount)
		End If
		info.UsCount = UBound(info.UsText)
		
		
		info.isEventMode = False
		
		'done
		Script_MemoLoad(strID, info.User, info.Memo)
		
		'UPGRADE_WARNING: Couldn't resolve default property of object Script_GetInfo_Format2. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		Script_GetInfo_Format2 = info
		
	End Function
	
	Public Function Script_Save(ByRef sID As String, ByRef info As SCRIPTINFO, ByRef lang_id As Integer) As Object
		If g_TextFormat = 2 Then
			'UPGRADE_WARNING: Couldn't resolve default property of object Script_Save_Format2(). Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
			Script_Save = Script_Save_Format2(g_Dir, sID, info, lang_id)
		Else
			'UPGRADE_WARNING: Couldn't resolve default property of object Script_Save_Format1(). Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
			Script_Save = Script_Save_Format1(g_Dir, sID, info, lang_id)
		End If
		
	End Function
	
	Public Function Script_SaveEvent(ByRef sID As String, ByRef nNum As Integer, ByRef sCnText As String) As Object
		
		Dim strFile As String
		Dim I As Integer
		
		Dim WriteText() As String
		
		'UPGRADE_WARNING: Arrays in structure tmp_info may need to be initialized before they can be used. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="814DF224-76BD-4BB4-BFFB-EA359CB9FC48"'
		Dim tmp_info As SCRIPTINFO
		
		strFile = g_Dir & "\cn-text\" & sID & ".txt"
		tmp_info.CnTextAll = Tool_LoadTextFile(strFile)
		Script_DecodeText(tmp_info.CnTextAll, tmp_info.CnText, tmp_info.Address)
		tmp_info.CnText(nNum) = sCnText
		tmp_info.JpCount = UBound(tmp_info.CnText)
		Script_Save_Format1(g_Dir, sID, tmp_info, LANGUAGE_CN)
		
	End Function
	
	Public Function Script_Save_Format1(ByRef strPath As String, ByRef sID As String, ByRef info As SCRIPTINFO, ByRef lang_id As Integer) As Object
		Dim strFile As String
		Dim fso As New Scripting.FileSystemObject
		Dim objFile As Scripting.TextStream
		
		Dim WriteText() As String
		
		Select Case lang_id
			Case LANGUAGE_JP
				strFile = strPath & "\jp-text\" & sID & ".txt"
				WriteText = VB6.CopyArray(info.JpText)
			Case LANGUAGE_CN
				strFile = strPath & "\cn-text\" & sID & ".txt"
				WriteText = VB6.CopyArray(info.CnText)
			Case LANGUAGE_US
				strFile = strPath & "\us-text\" & sID & ".txt"
				WriteText = VB6.CopyArray(info.UsText)
		End Select
		
		objFile = fso.OpenTextFile(strFile, Scripting.IOMode.ForWriting, True, Scripting.Tristate.TristateFalse)
		Dim I As Integer
		For I = 1 To info.JpCount
			
			objFile.Write("#### " & info.Address(I) & " ####" & vbCrLf)
			objFile.Write(WriteText(I))
			objFile.Write(vbCrLf & vbCrLf)
			
		Next I
		objFile.Close()
		
	End Function
	
	
	
	Public Function Script_Save_Format2(ByRef strPath As String, ByRef sID As String, ByRef info As SCRIPTINFO, ByRef lang_id As Integer) As Object
		Dim strFile As String
		Dim fso As New Scripting.FileSystemObject
		Dim objFile As Scripting.TextStream
		
		Dim WriteText() As String
		
		Select Case lang_id
			Case LANGUAGE_JP
				strFile = strPath & "\jp-text\" & sID & ".txt"
				WriteText = VB6.CopyArray(info.JpText)
			Case LANGUAGE_CN
				strFile = strPath & "\cn-text\" & sID & ".txt"
				WriteText = VB6.CopyArray(info.CnText)
			Case LANGUAGE_US
				strFile = strPath & "\us-text\" & sID & ".txt"
				WriteText = VB6.CopyArray(info.UsText)
		End Select
		
		objFile = fso.OpenTextFile(strFile, Scripting.IOMode.ForWriting, True, Scripting.Tristate.TristateFalse)
		Dim I As Integer
		Dim s As String
		
		For I = 1 To info.JpCount
			objFile.Write(info.Address(I) & "," & info.Maxlen(I) & ",")
			
			s = WriteText(I)
			s = Replace(s, vbCrLf, "{换行}")
			objFile.Write(s)
			objFile.Write(vbCrLf & vbCrLf)
		Next I
		objFile.Close()
		
	End Function
	
	
	Public Function Script_DecodeText_Format2(ByRef strAll As String, ByRef strText() As String, ByRef Address() As String, ByRef Maxlen() As Integer) As Integer
		
		'解析文本的格式,对于
		'地址,句子长度,文本
		
		Dim nID As Integer
		Dim nSharpStart, nStarpEnd As Integer
		Dim nTextStart, nTextEnd As Integer
		Dim nScanPos As Integer
		
		Dim s As String
		Dim strID As String
		
		
		nScanPos = 1
		
		
		nID = 0
		ReDim strText(nID)
		ReDim Address(nID)
		ReDim Maxlen(nID)
		
		
		Dim strLines() As String
		strLines = Split(strAll, vbCrLf,  , CompareMethod.Binary)
		
		Dim sThisLine As String
		Dim sThisLen, sThisAddr, sThisText As String
		Dim nPos As Object
		Dim I As Integer
		
		For I = 0 To UBound(strLines)
			
			sThisLine = strLines(I)
			
			'空行则跳过
			If Trim(sThisLine) = "" Then GoTo for_i_next
			
			
			'UPGRADE_WARNING: Couldn't resolve default property of object nPos. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
			nPos = InStr(1, sThisLine, ",", CompareMethod.Binary)
			'UPGRADE_WARNING: Couldn't resolve default property of object nPos. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
			If Not (nPos > 0) Then
				MsgBox("在文本文件行:" & I, MsgBoxStyle.Critical + MsgBoxStyle.OKOnly, "文本格式错误")
				End
			End If
			'UPGRADE_WARNING: Couldn't resolve default property of object nPos. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
			sThisAddr = Left(sThisLine, nPos - 1)
			'UPGRADE_WARNING: Couldn't resolve default property of object nPos. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
			sThisLine = Mid(sThisLine, nPos + 1)
			
			'UPGRADE_WARNING: Couldn't resolve default property of object nPos. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
			nPos = InStr(1, sThisLine, ",", CompareMethod.Binary)
			'UPGRADE_WARNING: Couldn't resolve default property of object nPos. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
			If Not (nPos > 0) Then
				MsgBox("在文本文件行:" & I, MsgBoxStyle.Critical + MsgBoxStyle.OKOnly, "文本格式错误")
				End
			End If
			'UPGRADE_WARNING: Couldn't resolve default property of object nPos. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
			sThisLen = Left(sThisLine, nPos - 1)
			'UPGRADE_WARNING: Couldn't resolve default property of object nPos. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
			sThisText = Mid(sThisLine, nPos + 1)
			
			nID = nID + 1
			ReDim Preserve strText(nID)
			ReDim Preserve Address(nID)
			ReDim Preserve Maxlen(nID)
			
			sThisText = Replace(sThisText, "{换行}", vbCrLf,  ,  , CompareMethod.Binary)
			
			strText(nID) = sThisText
			Address(nID) = sThisAddr
			Maxlen(nID) = CInt(sThisLen)
			
for_i_next: 
			
			System.Windows.Forms.Application.DoEvents()
		Next I
		
		Script_DecodeText_Format2 = nID
	End Function
	
	
	
	Public Function Script_DecodeText(ByRef strAll As String, ByRef strText() As String, ByRef strID() As String) As Integer
		
		'解析文本的格式
		'得到字符串数组
		
		Dim nID As Integer
		Dim nSharpStart, nStarpEnd As Integer
		Dim nTextStart, nTextEnd As Integer
		Dim nScanPos As Integer
		
		Dim s As String
		Dim strIDTemp As String
		
		'----------
		'strText = Split(strAll, "ˇ")
		
		'If UBound(strText) = 0 Then
		'    ReDim Preserve strText(0 To 1)
		'    ReDim Preserve strID(0 To 1)
		'    strText(1) = strAll
		'End If
		'Script_DecodeText = UBound(strText)
		'Exit Function
		'----------
		
		nID = 0
		nScanPos = 1
		
		ReDim strText(0)
		Do 
			
			
			'查找开头 #### 1 #### 标志
			nSharpStart = InStr(nScanPos, strAll, "####", CompareMethod.Binary)
			If Not nSharpStart > 0 Then Exit Do
			
			nStarpEnd = InStr(nSharpStart + 4, strAll, "####" & vbCrLf, CompareMethod.Binary)
			If Not nStarpEnd > 0 Then
				'Form2.info "#### is not match - " &
				Exit Do
			End If
			
			strIDTemp = Mid(strAll, nSharpStart + 4, nStarpEnd - nSharpStart - 4)
			strIDTemp = Trim(strIDTemp)
			nID = nID + 1
			
			ReDim Preserve strText(nID)
			ReDim Preserve strID(nID)
			
			nTextStart = nStarpEnd + 4 + 2
			
			
			'查找下一个 #### 标志，或者到了文本结束
			
			nSharpStart = InStr(nTextStart, strAll, vbCrLf & "#### ", CompareMethod.Binary)
			If Not nSharpStart > 0 Then
				nTextEnd = Len(strAll)
			Else
				nTextEnd = nSharpStart - 1
			End If
			
			s = Mid(strAll, nTextStart, nTextEnd - nTextStart + 1)
			
			s = Tool_StripCrLfLast(s)
			
			strText(nID) = s
			strID(nID) = strIDTemp
			
			nScanPos = nTextEnd
			
			System.Windows.Forms.Application.DoEvents()
		Loop 
		
		Script_DecodeText = nID
	End Function
	
	
	Public Function Script_MemoLoad(ByRef strID As String, ByRef strUser As String, ByRef strMemo As String) As Object
		'strID   IN
		'strUser OUT
		'strMemo OUT
		
		
		strUser = ""
		strMemo = ""
		
		Dim fso As New Scripting.FileSystemObject
		
		Dim objFile As Scripting.TextStream
		Dim strFile As String
		
		strFile = g_Dir & "\Info\" & strID & ".txt"
		
		On Error GoTo error_read
		objFile = fso.OpenTextFile(strFile, Scripting.IOMode.ForReading, False, Scripting.Tristate.TristateTrue)
		strUser = objFile.ReadLine
		strMemo = objFile.ReadLine
		objFile.Close()
		
error_read: 
		
	End Function
	
	
	Public Function Script_MemoSave(ByRef strID As String, ByRef strUser As String, ByRef strMemo As String) As Object
		'strID   IN
		'strUser IN
		'strMemo IN
		
		Dim fso As New Scripting.FileSystemObject
		
		Dim objFile As Scripting.TextStream
		Dim strFile As String
		
		strFile = g_Dir & "\Info\" & strID & ".txt"
		
		objFile = fso.OpenTextFile(strFile, Scripting.IOMode.ForWriting, True, Scripting.Tristate.TristateTrue)
		objFile.WriteLine(strUser)
		objFile.WriteLine(strMemo)
		objFile.Close()
		
	End Function
	
	
	Public Function Script_GetCnText(ByRef info As SCRIPTINFO, ByRef n As Integer) As String
		Script_GetCnText = info.CnText(n)
	End Function
	
	
	Public Function Script_GetJpText(ByRef info As SCRIPTINFO, ByRef n As Integer) As String
		Script_GetJpText = info.JpText(n)
	End Function
	
	Public Function Script_GetUsText(ByRef info As SCRIPTINFO, ByRef n As Integer) As String
		Script_GetUsText = info.UsText(n)
	End Function
	
	
	Private Sub Script_Sync(ByRef src_id As String, ByRef dst_id As String, ByRef src_start As Integer, ByRef src_end As Integer, ByRef dst_start As Integer)
		'UPGRADE_WARNING: Arrays in structure src may need to be initialized before they can be used. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="814DF224-76BD-4BB4-BFFB-EA359CB9FC48"'
		Dim src As SCRIPTINFO
		'UPGRADE_WARNING: Arrays in structure dst may need to be initialized before they can be used. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="814DF224-76BD-4BB4-BFFB-EA359CB9FC48"'
		Dim dst As SCRIPTINFO
		Dim I As Integer
		
		'src = Script_GetInfo("00000009")
		'For I = 123 To 734
		'If I Mod 2 = 1 Then
		'       src.CnText(I) = ""
		'End If
		'Next I
		'Script_Save "00000009", src
		
		
		If dst_id = "delsingle" Then
			'UPGRADE_WARNING: Couldn't resolve default property of object src. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
			src = Script_GetInfo(src_id)
			For I = src_start To src_end
				If I Mod 2 = 1 Then
					src.CnText(I) = ""
				End If
			Next I
			Script_Save(src_id, src, LANGUAGE_CN)
			
			Exit Sub
		End If
		
		'UPGRADE_WARNING: Couldn't resolve default property of object src. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		src = Script_GetInfo(src_id)
		'UPGRADE_WARNING: Couldn't resolve default property of object dst. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		dst = Script_GetInfo(dst_id)
		
		
		For I = src_start To src_end
			
			If Left(dst.CnText(dst_start + I - src_start), 1) = "￠" Then
				'do nothing, 不覆盖
			ElseIf Left(dst.CnText(dst_start + I - src_start), 1) = "〓" Then 
				dst.CnText(dst_start + I - src_start) = "〓" & src.CnText(I)
			Else
				dst.CnText(dst_start + I - src_start) = "┫" & src.CnText(I)
			End If
		Next I
		
		Script_Save(dst_id, dst, LANGUAGE_CN)
		
	End Sub
	
	Public Function SplitParaSen(ByVal sAddress As String, ByRef sPara As String, ByRef nSent As Integer) As Boolean
		Dim nTmp As Object
		Dim t1, t2 As String
		sPara = ""
		nSent = 0
		SplitParaSen = False
		
		'UPGRADE_WARNING: Couldn't resolve default property of object nTmp. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		nTmp = InStr(1, sAddress, "-", CompareMethod.Text)
		'UPGRADE_WARNING: Couldn't resolve default property of object nTmp. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		If Not (nTmp > 0) Then Exit Function
		
		'UPGRADE_WARNING: Couldn't resolve default property of object nTmp. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		t1 = Left(sAddress, nTmp - 1)
		'UPGRADE_WARNING: Couldn't resolve default property of object nTmp. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		t2 = Mid(sAddress, nTmp + 1)
		
		'段落是固定3位，前面补0
		'句子是普通的数字
		If ("" & Val(t2)) <> t2 Then Exit Function
		
		sPara = t1
		nSent = Val(t2)
		
		SplitParaSen = True
	End Function
End Module