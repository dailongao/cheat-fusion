'----------------------------------------------------
'	Ԥ��������. ��ԭʼ�ı�תΪ��ʾ��html�ַ���
' ��Ҫ�ǽ�����Ϸ�Ŀ����ַ�
'
'	��ڲ���
'           sText           ���֣��������ַ���
'           LanguageId      �����ġ����Ļ�Ӣ��, ����
'	����ֵ
'           HTML��ʽ����ʾ�ı�
'	ע���༭����ʾʱ�����Զ�ȥ�������ַ���������������Ϳ����ַ�
'----------------------------------------------------
Function PreviewHTML(sText, LanguageId)

'������ո���ʾ
	sText = Replace(sText, " ", "~")
	sText = Replace(sText, "��", "~")

sText = Replace(sText, "{pause}", "��" & vbcrlf)
sText = Replace(sText, "{page}", "��" & vbcrlf)
	
	PreviewHTML = sText
End Function                      
                                  
                                  
'----------------------------------------------------
'	�������������ҵ�ʹ��          
' ���������Բ�д����ʹ��Ĭ�ϵļ��㷽ʽ
'	��ڲ���ͬ�ϡ�����ֵΪ�����ҡ����������ı�
'----------------------------------------------------
Function PreviewText(sText, LanguageId)
	                              
	sText = Replace(sText, "{pause}", "��" & vbcrlf)
	sText = Replace(sText, " ", "~")
	sText = Replace(sText, "��", "~")
	                              
	PreviewText = sText           
End Function                      
