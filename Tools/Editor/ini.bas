Attribute VB_Name = "INITools"
'公共函数库

Option Explicit

Private Declare Function GetPrivateProfileString Lib "kernel32" Alias "GetPrivateProfileStringA" (ByVal lpApplicationName As String, ByVal lpKeyName As Any, ByVal lpDefault As String, ByVal lpReturnedString As String, ByVal nSize As Long, ByVal lpFileName As String) As Long
Private Declare Function WritePrivateProfileString Lib "kernel32" Alias "WritePrivateProfileStringA" (ByVal lpApplicationName As String, ByVal lpKeyName As Any, ByVal lpString As Any, ByVal lpFileName As String) As Long


Public Function INI_GetValue(strFile As String, strApp As String, strKey As String, strDefault As String) As String
    
    '获取INI中键的值，注意strFile是文件全路径名
    
    Dim strBuffer As String * 1024
    Dim lRet&
    
    lRet = GetPrivateProfileString(strApp, strKey, strDefault, strBuffer, 1023, strFile)
    
    If lRet = 0 Then
        INI_GetValue = Trim(strDefault)
    Else
        'don't use lRet (bytes)
        Dim nPos
        nPos = InStr(1, strBuffer, Chr(0), vbBinaryCompare)
        If nPos > 0 Then
            INI_GetValue = Left(strBuffer, nPos - 1)
        Else
            INI_GetValue = Trim(strBuffer)
        End If
        
    End If
    
End Function

Public Function INI_SetValue(strFile As String, strApp As String, strKey As String, strValue As String) As Boolean
    
    '设置INI中键的值
    Dim lRet&
    
    lRet = WritePrivateProfileString(strApp, strKey, strValue, strFile)
    
    If lRet = 0 Then
        INI_SetValue = False
    Else
        INI_SetValue = True
    End If
    
End Function

