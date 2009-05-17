Option Strict Off
Option Explicit On
Module INITools
	'公共函数库
	
	
	'UPGRADE_ISSUE: Declaring a parameter 'As Any' is not supported. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="FAE78A8D-8978-4FD4-8208-5B7324A8F795"'
    Private Declare Function GetPrivateProfileString Lib "kernel32" Alias "GetPrivateProfileStringA" (ByVal lpApplicationName As String, ByVal lpKeyName As String, ByVal lpDefault As String, ByVal lpReturnedString As String, ByVal nSize As Integer, ByVal lpFileName As String) As Integer
	'UPGRADE_ISSUE: Declaring a parameter 'As Any' is not supported. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="FAE78A8D-8978-4FD4-8208-5B7324A8F795"'
	'UPGRADE_ISSUE: Declaring a parameter 'As Any' is not supported. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="FAE78A8D-8978-4FD4-8208-5B7324A8F795"'
    Private Declare Function WritePrivateProfileString Lib "kernel32" Alias "WritePrivateProfileStringA" (ByVal lpApplicationName As String, ByVal lpKeyName As String, ByVal lpString As String, ByVal lpFileName As String) As Integer
	
	
	Public Function INI_GetValue(ByRef strFile As String, ByRef strApp As String, ByRef strKey As String, ByRef strDefault As String) As String
		
		'获取INI中键的值，注意strFile是文件全路径名
		
		Dim strBuffer As New VB6.FixedLengthString(1024)
		Dim lRet As Integer
		
		lRet = GetPrivateProfileString(strApp, strKey, strDefault, strBuffer.Value, 1023, strFile)
		
		Dim nPos As Object
		If lRet = 0 Then
			INI_GetValue = Trim(strDefault)
		Else
			'don't use lRet (bytes)
			'UPGRADE_WARNING: Couldn't resolve default property of object nPos. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
			nPos = InStr(1, strBuffer.Value, Chr(0), CompareMethod.Binary)
			'UPGRADE_WARNING: Couldn't resolve default property of object nPos. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
			If nPos > 0 Then
				'UPGRADE_WARNING: Couldn't resolve default property of object nPos. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
				INI_GetValue = Left(strBuffer.Value, nPos - 1)
			Else
				INI_GetValue = Trim(strBuffer.Value)
			End If
			
		End If
		
	End Function
	
	Public Function INI_SetValue(ByRef strFile As String, ByRef strApp As String, ByRef strKey As String, ByRef strValue As String) As Boolean
		
		'设置INI中键的值
		Dim lRet As Integer
		
		lRet = WritePrivateProfileString(strApp, strKey, strValue, strFile)
		
		If lRet = 0 Then
			INI_SetValue = False
		Else
			INI_SetValue = True
		End If
		
	End Function
End Module