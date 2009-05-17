Option Strict Off
Option Explicit On

Module VBS
	
	Dim g_HavePreviewText As Boolean
    Dim g_HavePreviewHTML As Boolean

    Dim sc As New MSScriptControl.ScriptControl
	Public Function VBS_Init() As Object
		'读入本游戏使用的教本
		
		Dim vbs_content As String
        vbs_content = Tool_LoadTextFile(g_Dir & "\game.vbs")
        sc.Language = "VBScript"
		
		Err.Clear()
		On Error Resume Next
        sc.AddCode(vbs_content)
		If Err.Number <> 0 Then
			MsgBox(Err.Description, MsgBoxStyle.Critical, "Game.vbs 错误")
			End
		End If
		
		Dim I As Integer
		g_HavePreviewText = False
		g_HavePreviewHTML = False
		
        For I = 1 To sc.Procedures.Count
            If sc.Procedures(I).Name = "PreviewText" Then g_HavePreviewText = True
            If sc.Procedures(I).Name = "PreviewHTML" Then g_HavePreviewHTML = True
        Next I
		
		If g_HavePreviewText = False And g_HavePreviewHTML = False Then
			Form2.info("no file - Game.vbs")
		End If
		
	End Function
	
    Public Function Callback_PlainText(ByVal sText As String, ByVal LanguageId As Integer) As String

        Dim vbsarg(2) As Object
        vbsarg(0) = sText
        vbsarg(1) = LanguageId

        If Left(sText, 1) = "〓" Then
            Callback_PlainText = ""
            Exit Function
        End If

        If g_HavePreviewText Then
            Err.Clear()
            On Error Resume Next
            'UPGRADE_WARNING: 未能解析对象 Form2.Script1.Run() 的默认属性。 单击以获得更多信息:“ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"”
            sText = sc.Run("PreviewText", vbsarg(0), vbsarg(1))
            If Err.Number <> 0 Then
                MsgBox(Err.Description, MsgBoxStyle.Critical, "Game.vbs 错误")
                End
            End If
        End If

        sText = Tool_ControlCodeRemove(sText)
        If sText = "" Then sText = "&nbsp;"

        Callback_PlainText = sText

    End Function
	
	
	Public Function Callback_Preview(ByVal sText As String, ByVal LanguageId As Integer) As String
		
		'入口参数
		'           sText           文字，含控制字符。
		'           LanguageId      是日文、中文或英文。
		'返回值
        '           要实际显示的数据，可以含HTML标记
        Dim vbsarg(2) As Object
        vbsarg(0) = sText
        vbsarg(1) = LanguageId
		
		If Left(sText, 1) = "〓" Then
			sText = "<font color=#8d8ad3>" & sText & "</font>"
        Else
            If g_HavePreviewHTML Then
                Err.Clear()
                On Error GoTo err_run
                'UPGRADE_WARNING: 未能解析对象 Form2.Script1.Run() 的默认属性。 单击以获得更多信息:“ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"”
                sText = sc.Run("PreviewHTML", vbsarg(0), vbsarg(1))
                If Err.Number <> 0 Then
                    MsgBox(Err.Description, MsgBoxStyle.Critical, "Game.vbs 错误")
                    End
                End If
            End If

            End If
err_run:
            '============= END ===================


            '处理结束
            sText = Replace(sText, vbCrLf, "<br>")


            sText = Tool_ControlCodeRemove(sText)
            If sText = "" Then sText = "&nbsp;"

            Callback_Preview = sText
    End Function
	
	
	Public Function Callback_Init() As Object
		
	End Function
	
	Public Function Callback_Save(ByRef sentence As String) As String
		'保存一句翻译的时候被调用
		'函数返回处理过的句子
		
		'Dim s As String
		
		'所有空格替换为全角的。不然无法对齐。
		'如果极特殊情况想用半角来作对齐，直接编辑文本文件吧。
		Callback_Save = sentence
		Callback_Save = Replace(Callback_Save, " ", "　")
		Callback_Save = Replace(Callback_Save, ",", "，")
		Callback_Save = Replace(Callback_Save, "?", "？")
		Callback_Save = Replace(Callback_Save, "!", "！")
		
	End Function
End Module