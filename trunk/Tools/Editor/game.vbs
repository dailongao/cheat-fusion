'----------------------------------------------------
'	预览界面用. 由原始文本转为显示的html字符串
' 主要是解释游戏的控制字符
'
'	入口参数
'           sText           文字，含控制字符。
'           LanguageId      是日文、中文或英文, 整数
'	返回值
'           HTML格式的显示文本
'	注：编辑器显示时，会自动去掉控制字符。所以在这里解释控制字符
'----------------------------------------------------
Function PreviewHTML(sText, LanguageId)

'误操作空格显示
	sText = Replace(sText, " ", "~")
	sText = Replace(sText, "　", "~")

sText = Replace(sText, "{pause}", "●" & vbcrlf)
sText = Replace(sText, "{page}", "■" & vbcrlf)
	
	PreviewHTML = sText
End Function                      
                                  
                                  
'----------------------------------------------------
'	计算字数、查找等使用          
' 本函数可以不写。会使用默认的计算方式
'	入口参数同上。返回值为供查找、算字数的文本
'----------------------------------------------------
Function PreviewText(sText, LanguageId)
	                              
	sText = Replace(sText, "{pause}", "●" & vbcrlf)
	sText = Replace(sText, " ", "~")
	sText = Replace(sText, "　", "~")
	                              
	PreviewText = sText           
End Function                      
