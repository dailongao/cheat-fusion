Option Strict Off
Option Explicit On
Module Counter
	
	'统计字数用
	
	Public Function Word_CounterNoSort(ByRef InText As String, ByRef arrWord() As String, ByRef arrCount() As Integer) As Object
		
		'arrWord        IN/OUT      'IN  原有的统计一开始需要初始化为 0 0
		'OUT 最新的统计
		'arrCount  同上
		
		'当前版本计算空格，不计算回车和换行
		
		Dim strText As Object
		
		'UPGRADE_WARNING: Couldn't resolve default property of object strText. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		strText = InText
		
		'UPGRADE_WARNING: Couldn't resolve default property of object strText. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		strText = Replace(strText, vbCr, "")
		'UPGRADE_WARNING: Couldn't resolve default property of object strText. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		strText = Replace(strText, vbLf, "")
		
		'全角替换为半角
		'UPGRADE_WARNING: Couldn't resolve default property of object strText. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		strText = Replace(strText, "", " ")
		
		
		Dim I As Integer
		Dim w As String
		
		Dim x As Object
		Dim isFound As Boolean
		For I = 1 To Len(strText)
			
			'UPGRADE_WARNING: Couldn't resolve default property of object strText. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
			w = Mid(strText, I, 1)
			
			isFound = False
			For x = 1 To UBound(arrWord)
				'UPGRADE_WARNING: Couldn't resolve default property of object x. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
				If arrWord(x) = w Then
					'UPGRADE_WARNING: Couldn't resolve default property of object x. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
					arrCount(x) = arrCount(x) + 1
					isFound = True
				End If
			Next x
			
			If Not isFound Then
				ReDim Preserve arrWord(UBound(arrWord) + 1)
				ReDim Preserve arrCount(UBound(arrCount) + 1)
				
				arrWord(UBound(arrWord)) = w
				arrCount(UBound(arrCount)) = 1
				
			End If
		Next I
		
		
	End Function
	
	Public Function Word_CounterNoSortRemoveC(ByRef InText As String, ByRef arrWord() As String, ByRef arrCount() As Integer, ByRef arrID() As String, ByRef strID As String) As Object
		
		'arrWord        IN/OUT      'IN  原有的统计一开始需要初始化为 0 0
		'OUT 最新的统计
		'arrCount  同上
		
		'当前版本计算空格，不计算回车和换行
		
		Dim strText As String
		
		strText = InText
		
		'不用replace，影像速度
		'strText = Replace(strText, vbCr, "")
		'strText = Replace(strText, vbLf, "")
		
		'全角替换为半角
		'strText = Replace(strText, "", " ")
		
		
		Dim I As Integer
		Dim w As String
		Dim isControl As Integer
		
		isControl = 0
		
		
		Dim x As Object
		Dim isFound As Boolean
		For I = 1 To Len(strText)
			
			w = Mid(strText, I, 1)
			
			If isControl Then
				If w = "}" Then isControl = 0
				GoTo for_i_next
			End If
			
			If w = "{" Then
				isControl = 1
				GoTo for_i_next
			End If
			
			If w = vbCr Then GoTo for_i_next
			If w = vbLf Then GoTo for_i_next
			
			If w = "　" Then w = " "
			
			
			isFound = False
			For x = 1 To UBound(arrWord)
				'UPGRADE_WARNING: Couldn't resolve default property of object x. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
				If arrWord(x) = w Then
					'UPGRADE_WARNING: Couldn't resolve default property of object x. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
					arrCount(x) = arrCount(x) + 1
					isFound = True
				End If
			Next x
			
			If Not isFound Then
				ReDim Preserve arrWord(UBound(arrWord) + 1)
				ReDim Preserve arrCount(UBound(arrCount) + 1)
				ReDim Preserve arrID(UBound(arrID) + 1)
				
				arrWord(UBound(arrWord)) = w
				arrID(UBound(arrID)) = strID
				arrCount(UBound(arrCount)) = 1
				
			End If
			
for_i_next: 
		Next I
		
		
	End Function
	
	
	Public Function Word_Counter(ByRef InText As String, ByRef arrWord() As String, ByRef arrCount() As Integer) As Object
		
		
		Word_CounterNoSort(InText, arrWord, arrCount)
		BubbleSort(arrWord, arrCount, 1, UBound(arrCount))
		
		
	End Function
	
	
	Public Function BubbleSort(ByRef s As Object, ByRef n As Object, ByRef Low As Integer, ByRef high As Integer) As Object
		'根据n中的值对s进行从大到小冒泡排序.
		'n()  : 可以是整数数组或者浮点数数组
		's()  : 其他类型的数组-比如字符串. s 的排序和n的排序相同
		'low  : 数组下界
		'hight: 数组上界
		
		Dim I, j As Integer
		Dim t_n As Object
		Dim t_s As Object
		
		If Not IsArray(s) Then Exit Function
		If Not IsArray(n) Then Exit Function
		
		For I = Low To high - 1
			'UPGRADE_WARNING: Couldn't resolve default property of object n(). Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
			'UPGRADE_WARNING: Couldn't resolve default property of object t_n. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
			t_n = n(I)
			'UPGRADE_WARNING: Couldn't resolve default property of object s(). Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
			'UPGRADE_WARNING: Couldn't resolve default property of object t_s. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
			t_s = s(I)
			
			j = I
			For j = I + 1 To high
				'UPGRADE_WARNING: Couldn't resolve default property of object n(I). Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
				'UPGRADE_WARNING: Couldn't resolve default property of object n(j). Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
				If n(j) > n(I) Then '交换
					'UPGRADE_WARNING: Couldn't resolve default property of object n(). Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
					'UPGRADE_WARNING: Couldn't resolve default property of object t_n. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
					t_n = n(j)
					'UPGRADE_WARNING: Couldn't resolve default property of object s(). Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
					'UPGRADE_WARNING: Couldn't resolve default property of object t_s. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
					t_s = s(j)
					'UPGRADE_WARNING: Couldn't resolve default property of object n(). Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
					n(j) = n(I)
					'UPGRADE_WARNING: Couldn't resolve default property of object s(). Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
					s(j) = s(I)
					'UPGRADE_WARNING: Couldn't resolve default property of object t_n. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
					'UPGRADE_WARNING: Couldn't resolve default property of object n(). Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
					n(I) = t_n
					'UPGRADE_WARNING: Couldn't resolve default property of object t_s. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
					'UPGRADE_WARNING: Couldn't resolve default property of object s(). Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
					s(I) = t_s
				End If
			Next j
		Next I
		
	End Function
End Module