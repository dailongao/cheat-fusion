Option Strict Off
Option Explicit On

Module VBS
	
	Dim g_HavePreviewText As Boolean
    Dim g_HavePreviewHTML As Boolean

    Dim sc As New MSScriptControl.ScriptControl
	Public Function VBS_Init() As Object
		'���뱾��Ϸʹ�õḺ̌�
		
		Dim vbs_content As String
        vbs_content = Tool_LoadTextFile(g_Dir & "\game.vbs")
        sc.Language = "VBScript"
		
		Err.Clear()
		On Error Resume Next
        sc.AddCode(vbs_content)
		If Err.Number <> 0 Then
			MsgBox(Err.Description, MsgBoxStyle.Critical, "Game.vbs ����")
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

        If Left(sText, 1) = "��" Then
            Callback_PlainText = ""
            Exit Function
        End If

        If g_HavePreviewText Then
            Err.Clear()
            On Error Resume Next
            'UPGRADE_WARNING: δ�ܽ������� Form2.Script1.Run() ��Ĭ�����ԡ� �����Ի�ø�����Ϣ:��ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"��
            sText = sc.Run("PreviewText", vbsarg(0), vbsarg(1))
            If Err.Number <> 0 Then
                MsgBox(Err.Description, MsgBoxStyle.Critical, "Game.vbs ����")
                End
            End If
        End If

        sText = Tool_ControlCodeRemove(sText)
        If sText = "" Then sText = "&nbsp;"

        Callback_PlainText = sText

    End Function
	
	
	Public Function Callback_Preview(ByVal sText As String, ByVal LanguageId As Integer) As String
		
		'��ڲ���
		'           sText           ���֣��������ַ���
		'           LanguageId      �����ġ����Ļ�Ӣ�ġ�
		'����ֵ
        '           Ҫʵ����ʾ�����ݣ����Ժ�HTML���
        Dim vbsarg(2) As Object
        vbsarg(0) = sText
        vbsarg(1) = LanguageId
		
		If Left(sText, 1) = "��" Then
			sText = "<font color=#8d8ad3>" & sText & "</font>"
        Else
            If g_HavePreviewHTML Then
                Err.Clear()
                On Error GoTo err_run
                'UPGRADE_WARNING: δ�ܽ������� Form2.Script1.Run() ��Ĭ�����ԡ� �����Ի�ø�����Ϣ:��ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"��
                sText = sc.Run("PreviewHTML", vbsarg(0), vbsarg(1))
                If Err.Number <> 0 Then
                    MsgBox(Err.Description, MsgBoxStyle.Critical, "Game.vbs ����")
                    End
                End If
            End If

            End If
err_run:
            '============= END ===================


            '�������
            sText = Replace(sText, vbCrLf, "<br>")


            sText = Tool_ControlCodeRemove(sText)
            If sText = "" Then sText = "&nbsp;"

            Callback_Preview = sText
    End Function
	
	
	Public Function Callback_Init() As Object
		
	End Function
	
	Public Function Callback_Save(ByRef sentence As String) As String
		'����һ�䷭���ʱ�򱻵���
		'�������ش�����ľ���
		
		'Dim s As String
		
		'���пո��滻Ϊȫ�ǵġ���Ȼ�޷����롣
		'���������������ð���������룬ֱ�ӱ༭�ı��ļ��ɡ�
		Callback_Save = sentence
		Callback_Save = Replace(Callback_Save, " ", "��")
		Callback_Save = Replace(Callback_Save, ",", "��")
		Callback_Save = Replace(Callback_Save, "?", "��")
		Callback_Save = Replace(Callback_Save, "!", "��")
		
	End Function
End Module