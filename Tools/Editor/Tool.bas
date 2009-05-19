Attribute VB_Name = "tool_common"
Option Explicit


Public Function Tool_DumpBin(sFile As String, barr() As Byte)
    '字节数组写入二进制文件
    Dim hFile
    hFile = FreeFile
    Open sFile For Binary As hFile
    Put hFile, , barr
    Close hFile
    
End Function


Public Function Tool_LoadBin(sFile As String) As Byte()
    '下标从1开始
    
    Dim barr() As Byte
    
    If Dir(sFile) = "" Then
        Tool_LoadBin = barr
        Exit Function
    End If
    Dim hFile
    hFile = FreeFile
    Open sFile For Binary As hFile
    ReDim barr(1 To LOF(hFile))
    Get hFile, , barr
    Close hFile
    
    Tool_LoadBin = barr
End Function
Public Function Tool_DeleteFile(strFilename As String) As String
    On Error Resume Next
    Kill strFilename
End Function

Public Function Tool_GetFilenameMain(strFilename As String) As String
    
    '得到一个文件名的 前部分
    '比如  hello.txt
    '得到  hello
    
    Dim pos As Integer
    
    pos = InStrRev(strFilename, ".", 1, vbTextCompare)
    
    If pos > 0 Then
        Tool_GetFilenameMain = Left(strFilename, pos - 1)
    Else
        Tool_GetFilenameMain = ""
    End If
    
End Function

Public Function ToHex(ByVal v As Long, nMaxCount As Long) As String
    
    '格式化一个数字 v 为十六进制字符串，如果不够长，前面补0
    
    '例如nMaxCount = 8, v = 128:
    '等于C语言的 printf("%08X", v);
    
    Dim h$
    h = Hex(v)
    
    Do
        If Len(h) >= nMaxCount Then Exit Do
        h = "0" & h
    Loop

    ToHex = h
End Function


Public Function ToHexWithCut(ByVal v As Long, nMaxCount As Long) As String
    
    Dim h$
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

Public Function ToDec(v As Long, nMaxCount As Long) As String
    
    '格式化一个数字 v 为十进制字符串，如果不够长，前面补0
    
    '例如nMaxCount = 8, v = 128:
    '等于C语言的 printf("%08d", v);
    
    Dim h$
    h = "" & v
    
    Do
        If Len(h) >= nMaxCount Then Exit Do
        h = "0" & h
    Loop

    ToDec = h
End Function

Public Function GetFileLen(sFile As String) As Long

    GetFileLen = 0

    On Error GoTo file_exist_error
    GetFileLen = FileLen(sFile)
    
file_exist_error:
    
    
End Function


Public Function Tool_StripCrLf(sText As String) As String
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

Public Function Tool_StripCrLfLast(sText As String) As String
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

Public Function Tool_LoadTextFile(strFilename As String) As String
    
    Dim fso As New FileSystemObject
    Dim objFile As TextStream
        
    On Error GoTo text_error
    Set objFile = fso.OpenTextFile(strFilename, ForReading, False, TristateFalse)
    Tool_LoadTextFile = objFile.ReadAll()
    objFile.Close

    Exit Function

text_error:
    Tool_LoadTextFile = ""
    
End Function


Public Function Tool_WriteTextFile(strFilename As String, sText As String) As Boolean
    
    Dim fso As New FileSystemObject
    Dim objFile As TextStream
        
    On Error GoTo text_error
    Set objFile = fso.OpenTextFile(strFilename, ForWriting, True, TristateFalse)
    objFile.Write sText
    objFile.Close

    Tool_WriteTextFile = True
    Exit Function

text_error:
    Tool_WriteTextFile = False
    
End Function

Public Function Tool_WriteTextFileUnicode(strFilename As String, sText As String) As Boolean
    
    Dim fso As New FileSystemObject
    Dim objFile As TextStream
        
    On Error GoTo text_error
    Set objFile = fso.OpenTextFile(strFilename, ForWriting, True, TristateTrue)
    objFile.Write sText
    objFile.Close

    Tool_WriteTextFileUnicode = True
    Exit Function

text_error:
    Tool_WriteTextFileUnicode = False
    
End Function


Public Function Tool_GetLineCount(ByVal sText) As Long
    
    '计算一段文本共几行
    '返回值最小是1

    Dim s As String
    Dim n As Long
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
            If Tool_GetLineCount = 0 Then  '说明只有一行, 找不到 vbcrlf
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
    Dim pos
    
    txt = ""
    
    Dim start As Long
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
    Dim pos
    Dim start As Long
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
        
    Dim n As Long
    n = 1
    
    result = ""
    Do
        If n > Len(txt) Then Exit Do
        s = Mid(txt, n, 1)
        
        If s = "{" Then
            Dim pos
            pos = InStr(n, txt, "}")
            
            If Not (pos > 0) Then
                Form2.info " {} not match"
                Tool_WideConv = ""
                Exit Function
            End If
            s = Mid(txt, n, pos - n + 1)
            n = pos + 1
        Else
            If s = "<" Then
                Dim pos1
                pos1 = InStr(n, txt, ">")
            
                If Not (pos1 > 0) Then
                    Form2.info " <> not match"
                    Tool_WideConv = ""
                    Exit Function
                End If
                s = Mid(txt, n, pos1 - n + 1)
                n = pos1 + 1
            Else
            'normal text
            s = StrConv(s, vbWide)
            n = n + 1
            End If
        End If
        
        result = result & s
    Loop
    
    Tool_WideConv = result


End Function

Public Function MemReplace(bOld() As Byte, nReplaceOffset As Long, nReplaceLength As Long, bReplace() As Byte) As Byte()
    '123456
    '内存替换。
    'bOld,bReplace 都是下标从1开始的字节数组
    'nReplaceOffset 为从第几个开始替换

    Dim b() As Byte
    
    Dim I As Long
    I = UBound(bOld) + UBound(bReplace) - nReplaceLength
    ReDim b(1 To I) As Byte
    
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
    
    MemReplace = b
    
End Function


Public Function Tool_GetWords(ByVal s As String, nLang As Long) As Long

    '控制字符不算字数
    s = Callback_PlainText(s, nLang)
    
    
    '回车，空格不算字数，标点算。
    s = Replace(s, vbCrLf, "")
    s = Replace(s, "　", "")
    s = Replace(s, " ", "")
    s = Replace(s, "&nbsp;", "")

    Tool_GetWords = Len(s)
End Function
