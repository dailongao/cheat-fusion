Attribute VB_Name = "tool_cache"
Option Explicit

'������Ϣ��ʾ

Public Type CACHE_INFO
    sNo As String   'ʵ��δ��
    sID As String   '�ı�ID����Ӧ�ļ���
    nTrans As Long  '�����˶��پ�
    nTotal As Long  '�ܹ����پ�
    nWords As Long  '�ܹ�������
    sAuthor As String '����
    sMemo As String 'ע��
End Type

Public g_CacheFile As String

Public g_CacheInfo() As CACHE_INFO

Public Function Cache_Load() As Boolean
   
    Dim strAll As String
    Dim strLine() As String
    Dim arr() As String
    
    Cache_Load = False
    
    If Dir(g_CacheFile) = "" Then Exit Function
    
    strAll = Tool_LoadTextFile(g_CacheFile)
    If strAll = "" Then Exit Function
    
    strLine = Split(strAll, vbCrLf)
    
    ReDim g_CacheInfo(1 To UBound(strLine))
    
    Dim I As Long
    Dim j As Long
    
    For I = 1 To UBound(strLine) + 1
    
        If Trim(strLine(I - 1)) = "" Then Exit For
        arr = Split(strLine(I - 1), "|")
        
        If UBound(arr) = 6 Then
            g_CacheInfo(I).sNo = arr(0)
            g_CacheInfo(I).sID = arr(1)
            g_CacheInfo(I).nTrans = CLng(arr(2))
            g_CacheInfo(I).nTotal = CLng(arr(3))
            g_CacheInfo(I).nWords = CLng(arr(4))
            g_CacheInfo(I).sAuthor = arr(5)
            g_CacheInfo(I).sMemo = arr(6)
        Else
            Exit Function
        End If
    
    Next I
    
    Cache_Load = True
End Function

Public Function Cache_Save() As Boolean

    Form2.info "saving cache...pls wait..."
    DoEvents
   
    Dim strAll As String
    
    Cache_Save = False
    
    
    Dim I As Long
    
    strAll = ""
    
    For I = 1 To UBound(g_CacheInfo)
    
        strAll = strAll & g_CacheInfo(I).sNo
        strAll = strAll & "|" & g_CacheInfo(I).sID
        strAll = strAll & "|" & g_CacheInfo(I).nTrans
        strAll = strAll & "|" & g_CacheInfo(I).nTotal
        strAll = strAll & "|" & g_CacheInfo(I).nWords
        strAll = strAll & "|" & g_CacheInfo(I).sAuthor
        strAll = strAll & "|" & g_CacheInfo(I).sMemo
        strAll = strAll & vbCrLf
    
    Next I
    
    Tool_WriteTextFile g_CacheFile, strAll
    
    Cache_Save = True
End Function


