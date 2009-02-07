Attribute VB_Name = "tool_script"
Option Explicit

Public Const LANGUAGE_JP = 0
Public Const LANGUAGE_CN = 1
Public Const LANGUAGE_US = 2

Public g_TextFormat As Long
'1 = agemo文本格式
'2 = 地址，长度，文本 这种格式


Public g_len_id As String
Public g_len_j As String
Public g_len_c As String
Public g_len_u As String

Public g_hide_repeat As Boolean

Public Type SCRIPTINFO
    'data
    ID       As String      'ID (比如用 ISO offset 代替)
    
    JpText() As String
    UsText() As String
    CnText() As String
    CnTextAlter() As String
    
    dispCount As Long
    JpCount As Long           '一段中，日文多少句
    UsCount As Long           '一段中，英文多少句
    CnCount As Long           '一段中，译文多少句
    CnAlterCount As Long      '一段中，修改译文多少句
    
    JpTextAll As String
    CnTextAll As String
    CnTextAlterAll As String
    UsTextAll As String
    
    User As String          '译者
    Memo As String          '注释
    
    '对于这种格式用：地址,句子长度,文本
    Address() As String         'address 也是描述
    Maxlen() As Long
    
    'Event专用, event的 address = arrid _ arrnum
    arrID() As String
    arrNum() As Long
    isEventMode As Boolean
    
    
    
End Type

Public g_Dir As String      'script 路径，也就是工作路径


Public Sub TemplateSetColSize(sText As String)
    sText = Replace(sText, "WIDTH_ID", g_len_id)
    sText = Replace(sText, "WIDTH_JP", g_len_j)
    sText = Replace(sText, "WIDTH_CN", g_len_c)
    sText = Replace(sText, "WIDTH_US", g_len_u)
End Sub

Private Function MarkTextDiffHTML_Internal(ByRef src As String, ByRef dst As String) As Boolean
    '返回值表示，是否已经标记了。如果dst比原文短，则需要对换dst src重新做一次
    
    Dim ret As String
    Dim I As Long
    Dim s As String
    Dim d As String
    
    MarkTextDiffHTML_Internal = True
    
    Dim nDiffStart&, nDiffEnd&
    
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

Public Function MarkTextDiffHTML(ByRef src As String, ByRef dst As String)
    'src ,dst 输入输出，
    
    '返回HTML标记的好的text，注意，双测都有可能标记
    
    If src = dst Then
        dst = ""
        Exit Function
    End If
    
    src = Replace(src, "<br>", "∝")
    dst = Replace(dst, "<br>", "∝")
    
    If False = MarkTextDiffHTML_Internal(src, dst) Then
        MarkTextDiffHTML_Internal dst, src
    End If
    
    src = Replace(src, "∝", "<br>")
    dst = Replace(dst, "∝", "<br>")
    
    
End Function
Public Function Script_GetInfo(strID As String) As SCRIPTINFO

    If g_TextFormat = 2 Then
        Script_GetInfo = Script_GetInfo_Format2(strID)
    Else
        Script_GetInfo = Script_GetInfo_Format1(strID)
    End If
End Function


Public Function Script_GetInfo_Format1(strID As String) As SCRIPTINFO

    Script_GetInfo_Format1 = Script_GetInfo_Format1_Ex(g_Dir, strID)
    
End Function

Private Sub Event_InitData(ByVal sEventFile As String, ByRef nCount As Long, ByRef arrID() As String, arrNum() As Long)
    Dim barr() As Byte
    Dim I As Long
    
    

    barr = Tool_LoadBin(sEventFile)
    
    nCount = barr(1) + barr(2) * 256
    
    If nCount Mod 4 <> 0 Or nCount <= 0 Then
        nCount = 0
        MsgBox "Error in event file " & vbCrLf & sEventFile
        Exit Sub
    End If
    
    nCount = nCount / 4
    ReDim arrID(1 To nCount)
    ReDim arrNum(1 To nCount)
    For I = 1 To nCount
        'arrID(I) = ToDec(barr(16 + (I - 1) * 4 + 1) + barr(16 + (I - 1) * 4 + 2) * 256, 3)  ' 3 for xg
        arrID(I) = ToDec(barr(16 + (I - 1) * 4 + 1) + barr(16 + (I - 1) * 4 + 2) * 256, 4)  ' 4 for vp
        'arrNum(I) = barr(16 + (I - 1) * 4 + 3) + barr(16 + (I - 1) * 4 + 4) * 256 + 1        '+ 1 for xg
        arrNum(I) = barr(16 + (I - 1) * 4 + 3) + barr(16 + (I - 1) * 4 + 4) * 256 - 13       '- 13 for vp
    Next I
End Sub

Public Function Script_GetEventInfo(sEventID As String) As SCRIPTINFO
    
    Dim info  As SCRIPTINFO
    Dim info_tmp  As SCRIPTINFO
    
    Dim strID As String
    
    info.isEventMode = True
    
    Event_InitData g_Dir & "\event\" & sEventID & ".evt", info.JpCount, info.arrID, info.arrNum
    info.CnCount = info.JpCount
    info.UsCount = info.JpCount
    info.ID = sEventID
    
    Dim fso As New FileSystemObject
    Dim objFile As TextStream
    Dim strFile As String
    Dim I As Long
    
    Dim sTemp() As String
    
    ReDim info.CnText(0 To info.JpCount)
    ReDim info.JpText(0 To info.JpCount)
    ReDim info.UsText(0 To info.JpCount)
    ReDim info.CnTextAlter(0 To info.JpCount)
    ReDim info.Address(0 To info.JpCount)
    
    
    For I = 1 To info.JpCount
        
        strID = info.arrID(I)
        'read & decode jp, us, cn txt
        '############# JP
        strFile = g_Dir & "\jp-text\" & strID & ".txt"
        info_tmp.JpTextAll = Tool_LoadTextFile(strFile)
        Script_DecodeText info_tmp.JpTextAll, info_tmp.JpText, info_tmp.Address
        info.JpText(I) = info_tmp.JpText(info.arrNum(I))
        
        '############# CN
        strFile = g_Dir & "\cn-text\" & strID & ".txt"
        info_tmp.CnTextAll = Tool_LoadTextFile(strFile)
        Script_DecodeText info_tmp.CnTextAll, info_tmp.CnText, sTemp
        info.CnText(I) = info_tmp.CnText(info.arrNum(I))
        
        '############# alter
        strFile = g_Dir & "\cn-compare\" & strID & ".txt"
        info_tmp.CnTextAlterAll = Tool_LoadTextFile(strFile)
        Script_DecodeText info_tmp.CnTextAlterAll, info_tmp.CnTextAlter, sTemp
        ReDim Preserve info_tmp.CnTextAlter(0 To UBound(info_tmp.JpText))
        info.CnTextAlter(I) = info_tmp.CnTextAlter(info.arrNum(I))
        
        
        '############# US
        '注意的是，可能极特殊情况下美版要比日版句数多, 也可能根本没有美版的
        strFile = g_Dir & "\us-text\" & strID & ".txt"
        info_tmp.UsTextAll = Tool_LoadTextFile(strFile)
        Script_DecodeText info_tmp.UsTextAll, info_tmp.UsText, sTemp
        ReDim Preserve info_tmp.UsText(0 To UBound(info_tmp.JpText))
        info.UsText(I) = info_tmp.UsText(info.arrNum(I))
        
        info.Address(I) = strID & " " & info.arrNum(I)
    Next I
    
    Script_GetEventInfo = info
    
    
End Function

Public Function Script_GetInfo_Format1_Ex(strBaseDir As String, strID As String) As SCRIPTINFO

    '获取信息 *
    
    Dim info  As SCRIPTINFO
    
    info.ID = strID
    Dim sTemp() As String
    
    Dim fso As New FileSystemObject
    Dim objFile As TextStream
    Dim strFile As String
    
    Dim nReserverd As Long      '{repeat:   {dummy:
    Dim I As Long '计算已经翻译了多少句
    
    nReserverd = 0
    'read & decode jp, us, cn txt
    '############# JP
    strFile = strBaseDir & "\jp-text\" & strID & ".txt"
    info.JpTextAll = Tool_LoadTextFile(strFile)
    Script_DecodeText info.JpTextAll, info.JpText, info.Address
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
    Script_DecodeText info.CnTextAll, info.CnText, sTemp
    ReDim Preserve info.CnText(0 To info.JpCount)
    info.CnCount = 0
    For I = 0 To UBound(info.CnText)
        If info.CnText(I) <> "" And Left(info.JpText(I), Len("{repeat:")) <> "{repeat:" And Left(info.JpText(I), Len("{dummy:")) <> "{dummy:" Then info.CnCount = info.CnCount + 1
    Next I
    
    
    '############# alter
    strFile = strBaseDir & "\cn-compare\" & strID & ".txt"
    info.CnTextAlterAll = Tool_LoadTextFile(strFile)
    Script_DecodeText info.CnTextAlterAll, info.CnTextAlter, sTemp
    ReDim Preserve info.CnTextAlter(0 To info.JpCount)
    
    If info.CnTextAlterAll = "" Then
        info.CnAlterCount = 0
    Else
        info.CnAlterCount = info.JpCount
    End If
    
    '############# US
    '注意的是，可能极特殊情况下美版要比日版句数多
    strFile = strBaseDir & "\us-text\" & strID & ".txt"
    info.UsTextAll = Tool_LoadTextFile(strFile)
    Script_DecodeText info.UsTextAll, info.UsText, sTemp
    If UBound(info.UsText) < UBound(info.JpText) Then
        ReDim Preserve info.UsText(0 To info.JpCount)
    End If
    info.UsCount = UBound(info.UsText)
    
    info.isEventMode = False
    
    
    'done
    Script_MemoLoad strID, info.User, info.Memo
    Script_GetInfo_Format1_Ex = info
    
End Function



Public Function Script_GetInfo_Format2(strID As String) As SCRIPTINFO

    '获取信息 *
    
    Dim info  As SCRIPTINFO
    
    info.ID = strID
    
    Dim fso As New FileSystemObject
    Dim objFile As TextStream
    Dim strFile As String
    
    Dim Address() As String
    Dim Maxlen() As Long
    
    'read & decode jp, us, cn txt
    '############# JP
    strFile = g_Dir & "\jp-text\" & strID & ".txt"
    info.JpTextAll = Tool_LoadTextFile(strFile)
    Script_DecodeText_Format2 info.JpTextAll, info.JpText, info.Address, info.Maxlen
    info.JpCount = UBound(info.JpText)
    info.dispCount = info.JpCount
    
    '############# CN
    strFile = g_Dir & "\cn-text\" & strID & ".txt"
    info.CnTextAll = Tool_LoadTextFile(strFile)
    Script_DecodeText_Format2 info.CnTextAll, info.CnText, Address, Maxlen
    ReDim Preserve info.CnText(0 To info.JpCount)
    Dim I As Long '计算已经翻译了多少句
    info.CnCount = 0
    For I = 0 To UBound(info.CnText)
        If info.CnText(I) <> "" Then info.CnCount = info.CnCount + 1
    Next I
    
    '############# alter
    strFile = g_Dir & "\cn-compare\" & strID & ".txt"
    info.CnTextAlterAll = Tool_LoadTextFile(strFile)
    Script_DecodeText_Format2 info.CnTextAlterAll, info.CnTextAlter, Address, Maxlen
    ReDim Preserve info.CnTextAlter(0 To info.JpCount)
    
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
    Script_DecodeText_Format2 info.UsTextAll, info.UsText, Address, Maxlen
    If UBound(info.UsText) < UBound(info.JpText) Then
        ReDim Preserve info.UsText(0 To info.JpCount)
    End If
    info.UsCount = UBound(info.UsText)
    
    
    info.isEventMode = False
    
    'done
    Script_MemoLoad strID, info.User, info.Memo
    
    Script_GetInfo_Format2 = info
    
End Function

Public Function Script_Save(sID As String, info As SCRIPTINFO, lang_id As Long)
    If g_TextFormat = 2 Then
        Script_Save = Script_Save_Format2(g_Dir, sID, info, lang_id)
    Else
        Script_Save = Script_Save_Format1(g_Dir, sID, info, lang_id)
    End If

End Function

Public Function Script_SaveEvent(sID As String, nNum As Long, sCnText As String)

    Dim strFile As String
    Dim I As Long
    
    Dim WriteText() As String
    
    Dim tmp_info As SCRIPTINFO
    
    strFile = g_Dir & "\cn-text\" & sID & ".txt"
    tmp_info.CnTextAll = Tool_LoadTextFile(strFile)
    Script_DecodeText tmp_info.CnTextAll, tmp_info.CnText, tmp_info.Address
    tmp_info.CnText(nNum) = sCnText
    tmp_info.JpCount = UBound(tmp_info.CnText)
    Script_Save_Format1 g_Dir, sID, tmp_info, LANGUAGE_CN

End Function

Public Function Script_Save_Format1(strPath As String, sID As String, info As SCRIPTINFO, lang_id As Long)
    Dim strFile As String
    Dim fso As New FileSystemObject
    Dim objFile As TextStream
    
    Dim WriteText() As String
    
    Select Case lang_id
        Case LANGUAGE_JP
            strFile = strPath & "\jp-text\" & sID & ".txt"
            WriteText = info.JpText
        Case LANGUAGE_CN
            strFile = strPath & "\cn-text\" & sID & ".txt"
            WriteText = info.CnText
        Case LANGUAGE_US
            strFile = strPath & "\us-text\" & sID & ".txt"
            WriteText = info.UsText
    End Select
    
    Set objFile = fso.OpenTextFile(strFile, ForWriting, True, TristateFalse)
    Dim I As Long
    For I = 1 To info.JpCount
        
        objFile.Write "#### " & info.Address(I) & " ####" & vbCrLf
        objFile.Write WriteText(I)
        objFile.Write vbCrLf & vbCrLf
        
    Next I
    objFile.Close

End Function



Public Function Script_Save_Format2(strPath As String, sID As String, info As SCRIPTINFO, lang_id As Long)
    Dim strFile As String
    Dim fso As New FileSystemObject
    Dim objFile As TextStream
    
    Dim WriteText() As String
    
    Select Case lang_id
        Case LANGUAGE_JP
            strFile = strPath & "\jp-text\" & sID & ".txt"
            WriteText = info.JpText
        Case LANGUAGE_CN
            strFile = strPath & "\cn-text\" & sID & ".txt"
            WriteText = info.CnText
        Case LANGUAGE_US
            strFile = strPath & "\us-text\" & sID & ".txt"
            WriteText = info.UsText
    End Select
    
    Set objFile = fso.OpenTextFile(strFile, ForWriting, True, TristateFalse)
    Dim I As Long
    Dim s As String
    
    For I = 1 To info.JpCount
        objFile.Write info.Address(I) & "," & info.Maxlen(I) & ","
        
        s = WriteText(I)
        s = Replace(s, vbCrLf, "{换行}")
        objFile.Write s
        objFile.Write vbCrLf & vbCrLf
    Next I
    objFile.Close

End Function


Public Function Script_DecodeText_Format2(strAll As String, strText() As String, Address() As String, Maxlen() As Long) As Long
    
    '解析文本的格式,对于
    '地址,句子长度,文本
    
    Dim nID As Long
    Dim nSharpStart&, nStarpEnd&
    Dim nTextStart&, nTextEnd&
    Dim nScanPos As Long
    
    Dim s As String
    Dim strID As String
    
    
    nScanPos = 1
    
    
    nID = 0
    ReDim strText(0 To nID)
    ReDim Address(0 To nID)
    ReDim Maxlen(0 To nID)
    
    
    Dim strLines() As String
    strLines = Split(strAll, vbCrLf, , vbBinaryCompare)
    
    Dim sThisLine$
    Dim sThisAddr$, sThisLen$, sThisText$
    Dim nPos
    Dim I As Long
    
    For I = 0 To UBound(strLines)
        
        sThisLine = strLines(I)
        
        '空行则跳过
        If Trim(sThisLine) = "" Then GoTo for_i_next
        
        
        nPos = InStr(1, sThisLine, ",", vbBinaryCompare)
        If Not (nPos > 0) Then
            MsgBox "在文本文件行:" & I, vbCritical + vbOKOnly, "文本格式错误"
            End
        End If
        sThisAddr = Left(sThisLine, nPos - 1)
        sThisLine = Mid(sThisLine, nPos + 1)
        
        nPos = InStr(1, sThisLine, ",", vbBinaryCompare)
        If Not (nPos > 0) Then
            MsgBox "在文本文件行:" & I, vbCritical + vbOKOnly, "文本格式错误"
            End
        End If
        sThisLen = Left(sThisLine, nPos - 1)
        sThisText = Mid(sThisLine, nPos + 1)
        
        nID = nID + 1
        ReDim Preserve strText(0 To nID)
        ReDim Preserve Address(0 To nID)
        ReDim Preserve Maxlen(0 To nID)
        
        sThisText = Replace(sThisText, "{换行}", vbCrLf, , , vbBinaryCompare)
        
        strText(nID) = sThisText
        Address(nID) = sThisAddr
        Maxlen(nID) = CLng(sThisLen)
        
for_i_next:
        
        DoEvents
    Next I
    
    Script_DecodeText_Format2 = nID
End Function



Public Function Script_DecodeText(strAll As String, strText() As String, strID() As String) As Long
    
    '解析文本的格式
    '得到字符串数组
    
    Dim nID As Long
    Dim nSharpStart&, nStarpEnd&
    Dim nTextStart&, nTextEnd&
    Dim nScanPos As Long
    
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
    
    ReDim strText(0 To 0) As String
    Do
    
    
        '查找开头 #### 1 #### 标志
        nSharpStart = InStr(nScanPos, strAll, "####", vbBinaryCompare)
        If Not nSharpStart > 0 Then Exit Do
        
        nStarpEnd = InStr(nSharpStart + 4, strAll, "####" & vbCrLf, vbBinaryCompare)
        If Not nStarpEnd > 0 Then
            'Form2.info "#### is not match - " &
            Exit Do
        End If
        
        strIDTemp = Mid(strAll, nSharpStart + 4, nStarpEnd - nSharpStart - 4)
        strIDTemp = Trim(strIDTemp)
        nID = nID + 1
        
        ReDim Preserve strText(0 To nID)
        ReDim Preserve strID(0 To nID)
        
        nTextStart = nStarpEnd + 4 + 2
        
        
        '查找下一个 #### 标志，或者到了文本结束
        
        nSharpStart = InStr(nTextStart, strAll, vbCrLf & "#### ", vbBinaryCompare)
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
        
        DoEvents
    Loop
    
    Script_DecodeText = nID
End Function


Public Function Script_MemoLoad(strID As String, strUser$, strMemo$)
    'strID   IN
    'strUser OUT
    'strMemo OUT
    
    
    strUser = ""
    strMemo = ""
    
    Dim fso As New FileSystemObject
    
    Dim objFile As TextStream
    Dim strFile As String
    
    strFile = g_Dir & "\Info\" & strID & ".txt"
    
    On Error GoTo error_read
    Set objFile = fso.OpenTextFile(strFile, ForReading, False, TristateTrue)
    strUser = objFile.ReadLine
    strMemo = objFile.ReadLine
    objFile.Close
    
error_read:

End Function


Public Function Script_MemoSave(strID As String, strUser$, strMemo$)
    'strID   IN
    'strUser IN
    'strMemo IN
    
    Dim fso As New FileSystemObject
    
    Dim objFile As TextStream
    Dim strFile As String
    
    strFile = g_Dir & "\Info\" & strID & ".txt"
    
    Set objFile = fso.OpenTextFile(strFile, ForWriting, True, TristateTrue)
    objFile.WriteLine strUser
    objFile.WriteLine strMemo
    objFile.Close

End Function


Public Function Script_GetCnText(info As SCRIPTINFO, n As Long) As String
    Script_GetCnText = info.CnText(n)
End Function


Public Function Script_GetJpText(info As SCRIPTINFO, n As Long) As String
    Script_GetJpText = info.JpText(n)
End Function

Public Function Script_GetUsText(info As SCRIPTINFO, n As Long) As String
    Script_GetUsText = info.UsText(n)
End Function


Private Sub Script_Sync(src_id$, dst_id$, src_start&, src_end&, dst_start&)
    Dim src As SCRIPTINFO
    Dim dst As SCRIPTINFO
    Dim I As Long
    
        'src = Script_GetInfo("00000009")
        'For I = 123 To 734
            'If I Mod 2 = 1 Then
         '       src.CnText(I) = ""
            'End If
        'Next I
        'Script_Save "00000009", src
    
    
    If dst_id = "delsingle" Then
        src = Script_GetInfo(src_id)
        For I = src_start To src_end
            If I Mod 2 = 1 Then
                src.CnText(I) = ""
            End If
        Next I
        Script_Save src_id, src, LANGUAGE_CN
        
        Exit Sub
    End If
    
    src = Script_GetInfo(src_id)
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
    
    Script_Save dst_id, dst, LANGUAGE_CN
    
End Sub

Public Function SplitParaSen(ByVal sAddress$, ByRef sPara$, ByRef nSent&) As Boolean
    Dim nTmp
    Dim t1 As String, t2 As String
    sPara = ""
    nSent = 0
    SplitParaSen = False
    
    nTmp = InStr(1, sAddress, "-", vbTextCompare)
    If Not (nTmp > 0) Then Exit Function
    
    t1 = Left(sAddress, nTmp - 1)
    t2 = Mid(sAddress, nTmp + 1)
    
    '段落是固定3位，前面补0
    '句子是普通的数字
    If ("" & Val(t2)) <> t2 Then Exit Function
    
    sPara = t1
    nSent = Val(t2)
    
    SplitParaSen = True
End Function

