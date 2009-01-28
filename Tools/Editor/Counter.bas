Attribute VB_Name = "Counter"
Option Explicit

'ͳ��������

Public Function Word_CounterNoSort(InText As String, arrWord() As String, arrCount() As Long)

    'arrWord        IN/OUT      'IN  ԭ�е�ͳ��һ��ʼ��Ҫ��ʼ��Ϊ 0 0
                                'OUT ���µ�ͳ��
    'arrCount  ͬ��
     
    '��ǰ�汾����ո񣬲�����س��ͻ���
    
    Dim strText
    
    strText = InText
    
    strText = Replace(strText, vbCr, "")
    strText = Replace(strText, vbLf, "")
    
    'ȫ���滻Ϊ���
    strText = Replace(strText, "��", " ")
    
    
    Dim I&
    Dim w As String
    
    For I = 1 To Len(strText)
    
        w = Mid(strText, I, 1)
        
        Dim x
        Dim isFound As Boolean
        isFound = False
        For x = 1 To UBound(arrWord)
            If arrWord(x) = w Then
                arrCount(x) = arrCount(x) + 1
                isFound = True
            End If
        Next x
        
        If Not isFound Then
            ReDim Preserve arrWord(0 To UBound(arrWord) + 1) As String
            ReDim Preserve arrCount(0 To UBound(arrCount) + 1) As Long
            
            arrWord(UBound(arrWord)) = w
            arrCount(UBound(arrCount)) = 1
            
        End If
    Next I
    

End Function

Public Function Word_CounterNoSortRemoveC(InText As String, arrWord() As String, arrCount() As Long, arrID() As String, strID As String)

    'arrWord        IN/OUT      'IN  ԭ�е�ͳ��һ��ʼ��Ҫ��ʼ��Ϊ 0 0
                                'OUT ���µ�ͳ��
    'arrCount  ͬ��
     
    '��ǰ�汾����ո񣬲�����س��ͻ���
    
    Dim strText As String
    
    strText = InText
    
    '����replace��Ӱ���ٶ�
    'strText = Replace(strText, vbCr, "")
    'strText = Replace(strText, vbLf, "")
    
    'ȫ���滻Ϊ���
    'strText = Replace(strText, "��", " ")
    
    
    Dim I&
    Dim w As String
    Dim isControl As Long
    
    isControl = 0
    
    
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
        
        If w = "��" Then w = " "
        
        
        Dim x
        Dim isFound As Boolean
        isFound = False
        For x = 1 To UBound(arrWord)
            If arrWord(x) = w Then
                arrCount(x) = arrCount(x) + 1
                isFound = True
            End If
        Next x
        
        If Not isFound Then
            ReDim Preserve arrWord(0 To UBound(arrWord) + 1) As String
            ReDim Preserve arrCount(0 To UBound(arrCount) + 1) As Long
            ReDim Preserve arrID(0 To UBound(arrID) + 1) As String
            
            arrWord(UBound(arrWord)) = w
            arrID(UBound(arrID)) = strID
            arrCount(UBound(arrCount)) = 1
            
        End If
        
for_i_next:
    Next I
    

End Function


Public Function Word_Counter(InText As String, arrWord() As String, arrCount() As Long)


    Word_CounterNoSort InText, arrWord, arrCount
    BubbleSort arrWord, arrCount, 1, UBound(arrCount)
    
    
End Function


Public Function BubbleSort(s As Variant, n As Variant, Low As Long, high As Long)
    '����n�е�ֵ��s���дӴ�Сð������.
    'n()  : ����������������߸���������
    's()  : �������͵�����-�����ַ���. s �������n��������ͬ
    'low  : �����½�
    'hight: �����Ͻ�

    Dim I&, j&
    Dim t_n
    Dim t_s
    
    If Not IsArray(s) Then Exit Function
    If Not IsArray(n) Then Exit Function
    
    For I = Low To high - 1
        t_n = n(I)
        t_s = s(I)
        
        j = I
        For j = I + 1 To high
            If n(j) > n(I) Then '����
                t_n = n(j)
                t_s = s(j)
                n(j) = n(I)
                s(j) = s(I)
                n(I) = t_n
                s(I) = t_s
            End If
        Next j
    Next I

End Function

