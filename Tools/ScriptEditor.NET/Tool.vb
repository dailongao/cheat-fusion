Option Strict Off
Option Explicit On
Module tool_common
	
	
	Public Function Tool_DumpBin(ByRef sFile As String, ByRef barr() As Byte) As Object
		'字节数组写入二进制文件
		Dim hFile As Object
		'UPGRADE_WARNING: Couldn't resolve default property of object hFile. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		hFile = FreeFile
		'UPGRADE_WARNING: Couldn't resolve default property of object hFile. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		FileOpen(hFile, sFile, OpenMode.Binary)
		'UPGRADE_WARNING: Couldn't resolve default property of object hFile. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		'UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
		FilePut(hFile, barr)
		'UPGRADE_WARNING: Couldn't resolve default property of object hFile. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		FileClose(hFile)
		
	End Function
	
	
	Public Function Tool_LoadBin(ByRef sFile As String) As Byte()
		'下标从1开始
		
		Dim barr() As Byte
		
		'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
		If Dir(sFile) = "" Then
			Tool_LoadBin = VB6.CopyArray(barr)
			Exit Function
		End If
		Dim hFile As Object
		'UPGRADE_WARNING: Couldn't resolve default property of object hFile. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		hFile = FreeFile
		'UPGRADE_WARNING: Couldn't resolve default property of object hFile. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		FileOpen(hFile, sFile, OpenMode.Binary)
		'UPGRADE_WARNING: Lower bound of array barr was changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="0F1C9BE1-AF9D-476E-83B1-17D43BECFF20"'
		'UPGRADE_WARNING: Couldn't resolve default property of object hFile. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		ReDim barr(LOF(hFile))
		'UPGRADE_WARNING: Couldn't resolve default property of object hFile. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		'UPGRADE_WARNING: Get was upgraded to FileGet and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
		FileGet(hFile, barr)
		'UPGRADE_WARNING: Couldn't resolve default property of object hFile. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		FileClose(hFile)
		
		Tool_LoadBin = VB6.CopyArray(barr)
	End Function
	Public Function Tool_DeleteFile(ByRef strFilename As String) As String
		On Error Resume Next
		Kill(strFilename)
	End Function
	
	Public Function Tool_GetFilenameMain(ByRef strFilename As String) As String
		
		'得到一个文件名的 前部分
		'比如  hello.txt
		'得到  hello
		
		Dim pos As Object
		
		'UPGRADE_WARNING: Couldn't resolve default property of object pos. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        pos = InStrRev(strFilename, ".", -1, CompareMethod.Text)
		
		'UPGRADE_WARNING: Couldn't resolve default property of object pos. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		If pos > 0 Then
			'UPGRADE_WARNING: Couldn't resolve default property of object pos. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
			Tool_GetFilenameMain = Left(strFilename, pos - 1)
		Else
			Tool_GetFilenameMain = ""
		End If
		
	End Function
	
	Public Function ToHex(ByVal v As Integer, ByRef nMaxCount As Integer) As String
		
		'格式化一个数字 v 为十六进制字符串，如果不够长，前面补0
		
		'例如nMaxCount = 8, v = 128:
		'等于C语言的 printf("%08X", v);
		
		Dim h As String
		h = Hex(v)
		
		Do 
			If Len(h) >= nMaxCount Then Exit Do
			h = "0" & h
		Loop 
		
		ToHex = h
	End Function
	
	
	Public Function ToHexWithCut(ByVal v As Integer, ByRef nMaxCount As Integer) As String
		
		Dim h As String
		h = Hex(v)
		
		Do 
			If Len(h) > nMaxCount Then
				'截断
				h = Right(h, nMaxCount)
			End If
			If Len(h) = nMaxCount Then Exit Do
			h = "0" & h
		Loop 
		
		ToHexWithCut = h
	End Function
	
	Public Function ToDec(ByRef v As Integer, ByRef nMaxCount As Integer) As String
		
		'格式化一个数字 v 为十进制字符串，如果不够长，前面补0
		
		'例如nMaxCount = 8, v = 128:
		'等于C语言的 printf("%08d", v);
		
		Dim h As String
		h = "" & v
		
		Do 
			If Len(h) >= nMaxCount Then Exit Do
			h = "0" & h
		Loop 
		
		ToDec = h
	End Function
	
	Public Function GetFileLen(ByRef sFile As String) As Integer
		
		GetFileLen = 0
		
		On Error GoTo file_exist_error
		GetFileLen = FileLen(sFile)
		
file_exist_error: 
		
		
	End Function
	
	
	Public Function Tool_StripCrLf(ByRef sText As String) As String
		'去掉一字符串首尾的回车换行
		
		'忽略掉文本前面的换行
		
		Dim s As String
		
		s = sText
		Do 
			If Left(s, 2) = vbCrLf Then
				s = Mid(s, 3)
			Else
				Exit Do
			End If
		Loop 
		
		
		'忽略掉文本后面的换行
		'
		Do 
			If Right(s, 2) = vbCrLf Then
				s = Left(s, Len(s) - 2)
			Else
				Exit Do
			End If
		Loop 
		
		
		Tool_StripCrLf = s
	End Function
	
	Public Function Tool_StripCrLfLast(ByRef sText As String) As String
		'去掉一字符串尾部的回车换行
		
		Dim s As String
		s = sText
		'忽略掉文本后面的换行
		'
		Do 
			If Right(s, 2) = vbCrLf Then
				s = Left(s, Len(s) - 2)
			Else
				Exit Do
			End If
		Loop 
		
		
		Tool_StripCrLfLast = s
	End Function
	
	Public Function Tool_LoadTextFile(ByRef strFilename As String) As String
		

		On Error GoTo text_error
        Tool_LoadTextFile = My.Computer.FileSystem.ReadAllText(strFilename, System.Text.Encoding.Default)
		Exit Function
		
text_error: 
		Tool_LoadTextFile = ""
		
	End Function
	
	
	Public Function Tool_WriteTextFile(ByRef strFilename As String, ByRef sText As String) As Boolean
		
        On Error GoTo text_error
        My.Computer.FileSystem.WriteAllText(strFilename, sText, False, System.Text.Encoding.Default)
        Tool_WriteTextFile = True
		Exit Function
		
text_error: 
		Tool_WriteTextFile = False
		
	End Function
	
	Public Function Tool_WriteTextFileUnicode(ByRef strFilename As String, ByRef sText As String) As Boolean
		
        On Error GoTo text_error
        My.Computer.FileSystem.WriteAllText(strFilename, sText, False, System.Text.Encoding.Unicode)
        Tool_WriteTextFileUnicode = True
        Exit Function

text_error:
        Tool_WriteTextFileUnicode = False
		
	End Function
	
	
	Public Function Tool_GetLineCount(ByVal sText As Object) As Integer
		
		'计算一段文本共几行
		'返回值最小是1
		
		Dim s As String
		Dim n As Integer
		'UPGRADE_WARNING: Couldn't resolve default property of object sText. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		s = sText
		
		n = 1
		
		Tool_GetLineCount = 0
		
		Do 
			n = InStr(n, s, vbCrLf)
			
			If n > 1 Then
				Tool_GetLineCount = Tool_GetLineCount + 1
				n = n + 2
			Else
				'查找完毕。
				If Tool_GetLineCount = 0 Then '说明只有一行, 找不到 vbcrlf
					Tool_GetLineCount = 1
				Else
					Tool_GetLineCount = Tool_GetLineCount + 1
				End If
				
				Exit Do
			End If
		Loop 
		
	End Function
	
	Public Function Tool_ControlCodeRemove(ByVal sText As String) As String
		'去掉一段文本的控制字符 {}
		
		Dim txt As String
		Dim pos As Object
		
		txt = ""
		
		Dim start As Integer
		start = 1
		
		Dim s As String
		Dim isFlag As Boolean
		
		isFlag = False
		Do 
			s = Mid(sText, start, 1)
			If s = "" Then Exit Do
			
			If s = "{" Then
				isFlag = True
			ElseIf s = "}" Then 
				isFlag = False
			ElseIf isFlag = False Then 
				txt = txt & s
			End If
			
			start = start + 1
			
		Loop 
		
		Tool_ControlCodeRemove = txt
		
	End Function
	
	Public Function Tool_ControlCodeRemoveEx(ByVal sText As String) As String
		'去掉一段文本的控制字符 {} <>
		
		Dim txt As String
		Dim pos As Object
		Dim start As Integer
		Dim s As String
		Dim isFlag As Boolean
		
		isFlag = False
		start = 1
		txt = ""
		Do 
			s = Mid(sText, start, 1)
			If s = "" Then Exit Do
			
			If s = "{" Then
				isFlag = True
			ElseIf s = "}" Then 
				isFlag = False
			ElseIf isFlag = False Then 
				txt = txt & s
			End If
			
			start = start + 1
			
		Loop 
		
		sText = txt
		
		isFlag = False
		start = 1
		txt = ""
		Do 
			s = Mid(sText, start, 1)
			If s = "" Then Exit Do
			
			If s = "<" Then
				isFlag = True
			ElseIf s = ">" Then 
				isFlag = False
			ElseIf isFlag = False Then 
				txt = txt & s
			End If
			
			start = start + 1
			
		Loop 
		
		Tool_ControlCodeRemoveEx = txt
		
	End Function
	
	
	Public Function Tool_WideConv(ByVal sOld As String) As String
		
		'能检测控制字符, {} 以内的东西不转为全角。
		Dim txt As String
		Dim result As String
		Dim s As String
		
		s = sOld
		
		
		txt = RTrim(s)
		
		Dim n As Integer
		n = 1
		
		result = ""
		Dim pos As Object
		Dim pos1 As Object
		Do 
			If n > Len(txt) Then Exit Do
			s = Mid(txt, n, 1)
			
			If s = "{" Then
				'UPGRADE_WARNING: Couldn't resolve default property of object pos. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
				pos = InStr(n, txt, "}")
				
				'UPGRADE_WARNING: Couldn't resolve default property of object pos. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
				If Not (pos > 0) Then
					Form2.info(" {} not match")
					Tool_WideConv = ""
					Exit Function
				End If
				'UPGRADE_WARNING: Couldn't resolve default property of object pos. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
				s = Mid(txt, n, pos - n + 1)
				'UPGRADE_WARNING: Couldn't resolve default property of object pos. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
				n = pos + 1
			Else
				If s = "<" Then
					'UPGRADE_WARNING: Couldn't resolve default property of object pos1. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
					pos1 = InStr(n, txt, ">")
					
					'UPGRADE_WARNING: Couldn't resolve default property of object pos1. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
					If Not (pos1 > 0) Then
						Form2.info(" <> not match")
						Tool_WideConv = ""
						Exit Function
					End If
					'UPGRADE_WARNING: Couldn't resolve default property of object pos1. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
					s = Mid(txt, n, pos1 - n + 1)
					'UPGRADE_WARNING: Couldn't resolve default property of object pos1. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
					n = pos1 + 1
				Else
					'normal text
					s = StrConv(s, VbStrConv.Wide)
					n = n + 1
				End If
			End If
			
			result = result & s
		Loop 
		
		Tool_WideConv = result
		
		
	End Function
	
	Public Function MemReplace(ByRef bOld() As Byte, ByRef nReplaceOffset As Integer, ByRef nReplaceLength As Integer, ByRef bReplace() As Byte) As Byte()
		'123456
		'内存替换。
		'bOld,bReplace 都是下标从1开始的字节数组
		'nReplaceOffset 为从第几个开始替换
		
		Dim b() As Byte
		
		Dim I As Integer
		I = UBound(bOld) + UBound(bReplace) - nReplaceLength
		'UPGRADE_WARNING: Lower bound of array b was changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="0F1C9BE1-AF9D-476E-83B1-17D43BECFF20"'
		ReDim b(I)
		
		'复制前部分
		For I = 1 To nReplaceOffset
			b(I) = bOld(I)
		Next I
		
		'插入新部分
		For I = 1 To UBound(bReplace)
			b(nReplaceOffset + I) = bReplace(I)
		Next I
		
		'复制后部分
		For I = 1 To UBound(bOld) - nReplaceOffset - nReplaceLength
			b(nReplaceOffset + UBound(bReplace) + I) = bOld(I + nReplaceOffset + nReplaceLength)
		Next I
		
		MemReplace = VB6.CopyArray(b)
		
	End Function
	
	
	Public Function Tool_GetWords(ByVal s As String, ByRef nLang As Integer) As Integer
		
		'控制字符不算字数
		s = Callback_PlainText(s, nLang)
		
		
		'回车，空格不算字数，标点算。
		s = Replace(s, vbCrLf, "")
		s = Replace(s, "　", "")
		s = Replace(s, " ", "")
		s = Replace(s, "&nbsp;", "")
		
		Tool_GetWords = Len(s)
	End Function
End Module