Option Strict Off
Option Explicit On
Module tool_cache
	
	'表格的信息显示
	
	Public Structure CACHE_INFO
		Dim sNo As String '实际未用
		Dim sID As String '文本ID，对应文件名
		Dim nTrans As Integer '翻译了多少句
		Dim nTotal As Integer '总共多少句
		Dim nWords As Integer '总共多少字
		Dim sAuthor As String '作者
		Dim sMemo As String '注释
	End Structure
	
	Public g_CacheFile As String
	
	Public g_CacheInfo() As CACHE_INFO
	
	Public Function Cache_Load() As Boolean
		
		Dim strAll As String
		Dim strLine() As String
		Dim arr() As String
		
		Cache_Load = False
		
		'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
		If Dir(g_CacheFile) = "" Then Exit Function
		
		strAll = Tool_LoadTextFile(g_CacheFile)
		If strAll = "" Then Exit Function
		
		strLine = Split(strAll, vbCrLf)
		
		'UPGRADE_WARNING: Lower bound of array g_CacheInfo was changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="0F1C9BE1-AF9D-476E-83B1-17D43BECFF20"'
		ReDim g_CacheInfo(UBound(strLine))
		
		Dim I As Integer
		Dim j As Integer
		
		For I = 1 To UBound(strLine) + 1
			
			If Trim(strLine(I - 1)) = "" Then Exit For
			arr = Split(strLine(I - 1), "|")
			
			If UBound(arr) = 6 Then
				g_CacheInfo(I).sNo = arr(0)
				g_CacheInfo(I).sID = arr(1)
				g_CacheInfo(I).nTrans = CInt(arr(2))
				g_CacheInfo(I).nTotal = CInt(arr(3))
				g_CacheInfo(I).nWords = CInt(arr(4))
				g_CacheInfo(I).sAuthor = arr(5)
				g_CacheInfo(I).sMemo = arr(6)
			Else
				Exit Function
			End If
			
		Next I
		
		Cache_Load = True
	End Function
	
	Public Function Cache_Save() As Boolean
		
		Form2.info("saving cache...pls wait...")
		System.Windows.Forms.Application.DoEvents()
		
		Dim strAll As String
		
		Cache_Save = False
		
		
		Dim I As Integer
		
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
		
		Tool_WriteTextFile(g_CacheFile, strAll)
		
		Cache_Save = True
	End Function
End Module