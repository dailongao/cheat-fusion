Attribute VB_Name = "VBS"
Option Explicit

Dim g_HavePreviewText As Boolean
Dim g_HavePreviewHTML As Boolean

Public Function VBS_Init()
    '���뱾��Ϸʹ�õḺ̌�
    
    Dim vbs_content$
    vbs_content = Tool_LoadTextFile(g_Dir & "\game.vbs")
    
    Err.Clear
    On Error Resume Next
    Form2.Script1.AddCode vbs_content
    If Err.Number <> 0 Then
        MsgBox Err.Description, vbCritical, "Game.vbs ����"
        End
    End If
    
    Dim I As Long
    g_HavePreviewText = False
    g_HavePreviewHTML = False
    
    For I = 1 To Form2.Script1.Procedures.Count
        If Form2.Script1.Procedures(I).Name = "PreviewText" Then g_HavePreviewText = True
        If Form2.Script1.Procedures(I).Name = "PreviewHTML" Then g_HavePreviewHTML = True
    Next I
    
    If g_HavePreviewText = False And g_HavePreviewHTML = False Then
        Form2.info "no file - Game.vbs"
    End If
            
End Function

Public Function Callback_PlainText(ByVal sText As String, ByVal LanguageId As Long) As String
   
    If Left(sText, 1) = "��" Then
        Callback_PlainText = ""
        Exit Function
    End If
     
    If g_HavePreviewText Then
        Err.Clear
        On Error Resume Next
        sText = Form2.Script1.Run("PreviewText", sText, LanguageId)
        If Err.Number <> 0 Then
            MsgBox Err.Description, vbCritical, "Game.vbs ����"
            End
        End If
    End If

    sText = Tool_ControlCodeRemove(sText)
    If sText = "" Then sText = "&nbsp;"
    
    Callback_PlainText = sText

End Function


Public Function Callback_Preview(ByVal sText As String, ByVal LanguageId As Long) As String

    '��ڲ���
    '           sText           ���֣��������ַ���
    '           LanguageId      �����ġ����Ļ�Ӣ�ġ�
    '����ֵ
    '           Ҫʵ����ʾ�����ݣ����Ժ�HTML���
    
    
    If Left(sText, 1) = "��" Then
        sText = "<font color=#8d8ad3>" & sText & "</font>"
    Else
        
        If g_HavePreviewHTML Then
            Err.Clear
            On Error Resume Next
            sText = Form2.Script1.Run("PreviewHTML", sText, LanguageId)
            If Err.Number <> 0 Then
                MsgBox Err.Description, vbCritical, "Game.vbs ����"
                End
            End If
        End If

    End If
    
    '============= END ===================
    
    
    '�������
    sText = Replace(sText, vbCrLf, "<br>")
    
    
    sText = Tool_ControlCodeRemove(sText)
    If sText = "" Then sText = "&nbsp;"
    
    Callback_Preview = sText
    
End Function


Public Function Callback_Init()

End Function

Public Function Callback_Save(sentence As String) As String
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


