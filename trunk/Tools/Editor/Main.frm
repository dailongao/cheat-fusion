VERSION 5.00
Object = "{5E9E78A0-531B-11CF-91F6-C2863C385E30}#1.0#0"; "MSFLXGRD.OCX"
Object = "{0E59F1D2-1FBE-11D0-8FF2-00A0D10038BC}#1.0#0"; "msscript.ocx"
Begin VB.Form Form2 
   Caption         =   "Script Editor 4.0 (Build 06-07-23)"
   ClientHeight    =   8400
   ClientLeft      =   165
   ClientTop       =   450
   ClientWidth     =   9225
   BeginProperty Font 
      Name            =   "����"
      Size            =   10.5
      Charset         =   0
      Weight          =   400
      Underline       =   0   'False
      Italic          =   0   'False
      Strikethrough   =   0   'False
   EndProperty
   Icon            =   "Main.frx":0000
   LinkTopic       =   "Form2"
   MaxButton       =   0   'False
   ScaleHeight     =   8400
   ScaleWidth      =   9225
   StartUpPosition =   2  '��Ļ����
   Begin MSScriptControlCtl.ScriptControl Script1 
      Left            =   4080
      Top             =   7800
      _ExtentX        =   1005
      _ExtentY        =   1005
      UseSafeSubset   =   -1  'True
   End
   Begin VB.ComboBox cboTone 
      BeginProperty Font 
         Name            =   "����"
         Size            =   9
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   300
      Left            =   0
      Style           =   2  'Dropdown List
      TabIndex        =   4
      Top             =   7680
      Visible         =   0   'False
      Width           =   2130
   End
   Begin VB.TextBox txtInfo 
      BackColor       =   &H8000000F&
      BeginProperty Font 
         Name            =   "����"
         Size            =   9
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      ForeColor       =   &H00404040&
      Height          =   1275
      Left            =   0
      MultiLine       =   -1  'True
      ScrollBars      =   2  'Vertical
      TabIndex        =   2
      Top             =   6360
      Width           =   9225
   End
   Begin MSFlexGridLib.MSFlexGrid Grid1 
      Height          =   6210
      Left            =   0
      TabIndex        =   1
      Top             =   0
      Width           =   9225
      _ExtentX        =   16272
      _ExtentY        =   10954
      _Version        =   393216
      Cols            =   7
      FocusRect       =   0
      SelectionMode   =   1
      AllowUserResizing=   1
      BeginProperty Font {0BE35203-8F91-11CE-9DE3-00AA004BB851} 
         Name            =   "����"
         Size            =   9
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
   End
   Begin VB.FileListBox lstScript 
      BeginProperty Font 
         Name            =   "����"
         Size            =   9
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   810
      Left            =   7320
      TabIndex        =   0
      Top             =   7200
      Visible         =   0   'False
      Width           =   1770
   End
   Begin MSFlexGridLib.MSFlexGrid Grid2 
      Height          =   6210
      Left            =   0
      TabIndex        =   3
      Top             =   0
      Width           =   9240
      _ExtentX        =   16298
      _ExtentY        =   10954
      _Version        =   393216
      Cols            =   7
      FocusRect       =   0
      SelectionMode   =   1
      AllowUserResizing=   1
      BeginProperty Font {0BE35203-8F91-11CE-9DE3-00AA004BB851} 
         Name            =   "����"
         Size            =   9
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
   End
   Begin VB.Menu mnuGrid 
      Caption         =   "��ѡ��"
      Begin VB.Menu mnuOption 
         Caption         =   "ѡ��"
      End
      Begin VB.Menu mnuRefresh 
         Caption         =   "ˢ��"
      End
      Begin VB.Menu mnuSearch 
         Caption         =   "����"
         Shortcut        =   {F3}
      End
      Begin VB.Menu mnuCalcWords 
         Caption         =   "����ѡ����Χ������"
         Visible         =   0   'False
      End
      Begin VB.Menu mnuRepeatSync 
         Caption         =   "�ظ��ı�ͬ��"
      End
      Begin VB.Menu mnuCharCounter 
         Caption         =   "�ַ�ʹ��ͳ��"
      End
      Begin VB.Menu mnuFastExit 
         Caption         =   "�����˳�(������cache)"
         Shortcut        =   ^{F4}
         Visible         =   0   'False
      End
   End
   Begin VB.Menu mnuMode 
      Caption         =   "��ͼ"
      Begin VB.Menu mnuModeCommon 
         Caption         =   "��ͨ"
         Checked         =   -1  'True
      End
      Begin VB.Menu mnuModeEvent 
         Caption         =   "Event"
      End
   End
   Begin VB.Menu mnuLang 
      Caption         =   "����"
      Begin VB.Menu mnuJC 
         Caption         =   "�գ���"
         Checked         =   -1  'True
      End
      Begin VB.Menu mnuJCA 
         Caption         =   "�գ��У��Ա�"
      End
      Begin VB.Menu mnuJCU 
         Caption         =   "�գ��У�Ӣ"
      End
      Begin VB.Menu mnuJUCAlter 
         Caption         =   "�գ�Ӣ���У��Ա�"
      End
   End
   Begin VB.Menu mnuView 
      Caption         =   "��ʾ"
      Begin VB.Menu mnuViewAll 
         Caption         =   "����"
         Checked         =   -1  'True
      End
      Begin VB.Menu mnuViewComplete 
         Caption         =   "�����"
      End
      Begin VB.Menu mnuViewUnfinish 
         Caption         =   "δ���"
      End
   End
   Begin VB.Menu mnuDummy 
      Caption         =   "  "
      Enabled         =   0   'False
   End
   Begin VB.Menu mnuAbout 
      Caption         =   "����"
   End
End
Attribute VB_Name = "Form2"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit

Private g_SearchStop As Boolean
Private g_NeedSaveCache As Boolean
Private g_ViewComplete As Boolean
Private g_ViewUnfinish As Boolean



Private Sub Ini_Load()
    Dim default_lan$, default_format$, hide_repeat$, hide_complete$
    default_lan = INI_GetValue(g_Dir & "\Editor.ini", "GUI", "language", "0")
    default_format = INI_GetValue(g_Dir & "\Editor.ini", "GUI", "format", "1")
    hide_repeat = INI_GetValue(g_Dir & "\Editor.ini", "GUI", "hide_repeat", "1")
    
    g_len_id = INI_GetValue(g_Dir & "\Editor.ini", "PreviewWidth", "ID", "30")
    g_len_j = INI_GetValue(g_Dir & "\Editor.ini", "PreviewWidth", "JP", "300")
    g_len_c = INI_GetValue(g_Dir & "\Editor.ini", "PreviewWidth", "CN", "300")
    g_len_u = INI_GetValue(g_Dir & "\Editor.ini", "PreviewWidth", "US", "300")
    
    Dim I&
    
    I = CLng(default_lan)
    If I = 0 Then
        mnuJC_Click
    ElseIf I = 1 Then
        mnuJCA_Click
    ElseIf I = 2 Then
        mnuJCU_Click
    Else
        mnuJUCAlter_Click
    End If
    
    
    g_TextFormat = CLng(default_format)
    
    g_hide_repeat = (hide_repeat = "1")
    
End Sub

Private Sub Ini_Save()
    Dim default_lan$, default_format$, hide_repeat$, hide_complete$
    
    If mnuJC.Checked Then
        default_lan = "0"
    ElseIf mnuJCA.Checked Then
        default_lan = "1"
    ElseIf mnuJCU.Checked Then
        default_lan = "2"
    Else
        default_lan = "3"
    End If
    
    default_format = CStr(g_TextFormat)
    
    hide_repeat = "0"
    If g_hide_repeat Then hide_repeat = "1"
        
    INI_SetValue g_Dir & "\Editor.ini", "GUI", "language", default_lan
    INI_SetValue g_Dir & "\Editor.ini", "GUI", "format", default_format
    INI_SetValue g_Dir & "\Editor.ini", "GUI", "hide_repeat", hide_repeat
    
    INI_SetValue g_Dir & "\Editor.ini", "PreviewWidth", "ID", g_len_id
    INI_SetValue g_Dir & "\Editor.ini", "PreviewWidth", "JP", g_len_j
    INI_SetValue g_Dir & "\Editor.ini", "PreviewWidth", "CN", g_len_c
    INI_SetValue g_Dir & "\Editor.ini", "PreviewWidth", "US", g_len_u
    
    
    
End Sub


Private Sub Form_Load()

    g_Dir = App.Path
    
    '���Դ���ʱ������Ŀ¼���� app.path�����Ǹ�Ϊʵ��Ŀ¼
    'g_Dir = "E:\vp2"
    'g_Dir = "D:\J2C\projects\vp\vp_compiler"
    
    On Error GoTo BypassReg
    Shell ("regsvr32.exe -s """ + g_Dir + "\SynMemoU.ocx""")
BypassReg:

    If Dir(g_Dir & "\jp-text", vbDirectory) = "" Then
        MsgBox "��ǰĿ¼��û�� jp-textĿ¼"
        End
    End If
    
    '��ɫ�������ʼ��
    Grid1.BackColor = RGB(&HF6, &HF7, &HEB)
    Grid1.BackColorFixed = RGB(&HD1, &HD9, &HC1)
    Grid2.BackColor = RGB(&HF6, &HF7, &HEB)
    Grid2.BackColorFixed = RGB(&HD1, &HD9, &HC1)
    txtInfo.BackColor = Grid1.BackColor
    Me.BackColor = Grid1.BackColor
    
    g_NeedSaveCache = True

    '����Ϸ�����ʼ��(����Ϸ�Զ���ģ����)
    Call Ini_Load
    Call VBS_Init
    Call Callback_Init
    
    
    '��Ϣ���ʼ��
    
    
    g_CacheFile = g_Dir & "\data\cache.dat"
    If Cache_Load() = False Then Call Cache_Rebuild(True)
    
    '��ʾ���
    Call CommonGrid_Init
    Call EventGrid_Init
    
    '��ǰ�ı���ʽ��ʼ�������ݲ˵���check�ģ�
    Call On_Mode_Click
   
End Sub

Private Sub Form_Unload(Cancel As Integer)

    If g_NeedSaveCache Then
        Cache_Save
    End If
    
    Form1.OnClickClose
       
    Call Ini_Save
    
    End

End Sub

Private Sub Grid1_Click()
    Grid1.ToolTipText = Grid1.Row
End Sub

Private Sub Grid1_DblClick()
    
    '���û���ı����˳�
    If Grid1.Row = 0 Then Exit Sub
    
    Form1.Load Grid1.Row, ""
    DoEvents
    Form1.Show

End Sub

Private Sub Grid2_DblClick()

    If Grid2.Row = 0 Then Exit Sub
    
    Form1.LoadEvent Grid2.TextMatrix(Grid2.Row, 1), ""
    DoEvents
    Form1.Show

End Sub


Private Sub Grid1_MouseUp(Button As Integer, Shift As Integer, x As Single, Y As Single)

    If Button = 2 Then
        '����ע��
        Dim sID As String
        sID = g_CacheInfo(Grid1.Row).sID
        Form3.ApplyData Grid1.Row, sID, g_CacheInfo(Grid1.Row).sAuthor, g_CacheInfo(Grid1.Row).sMemo
        
        If Not Form3.IsCancel Then
            Script_MemoSave sID, Form3.CurrentUser, Form3.CurrentMemo
            Grid_Update Grid1.Row   '�������user/memo�Լ�����ĸ���
            DoEvents
        End If
    End If
    
End Sub


Private Sub Cache_Rebuild(bWordsCalc As Boolean)

    Dim nAllJP&, nAllCN&
    Dim total_words&, total_trans&
    Dim I&, j&, s$
    Dim tmp_info As SCRIPTINFO
    
    lstScript.Path = g_Dir & "\jp-text"
    lstScript.Pattern = "*.txt"
    lstScript.Refresh
    
    If lstScript.ListCount = 0 Then
        Exit Sub
    End If

    ReDim g_CacheInfo(1 To lstScript.ListCount)
    
    
    For I = 1 To lstScript.ListCount
        
        g_CacheInfo(I).sNo = ToDec(I, 3)
        g_CacheInfo(I).sID = Tool_GetFilenameMain(lstScript.List(I - 1))
        tmp_info = Script_GetInfo(g_CacheInfo(I).sID)
        
        '���������Ƚϻ���ʱ��
        If bWordsCalc Then
            g_CacheInfo(I).nWords = 0
            For j = 1 To tmp_info.JpCount
                g_CacheInfo(I).nWords = g_CacheInfo(I).nWords + Tool_GetWords(tmp_info.JpText(j), LANGUAGE_JP)
            Next j
        End If
        
        total_words = total_words + g_CacheInfo(I).nWords
        
        If tmp_info.CnCount = tmp_info.dispCount Then total_trans = total_trans + g_CacheInfo(I).nWords
        
        g_CacheInfo(I).nTrans = tmp_info.CnCount
        g_CacheInfo(I).nTotal = tmp_info.dispCount
        g_CacheInfo(I).sAuthor = tmp_info.User
        g_CacheInfo(I).sMemo = tmp_info.Memo
        
        
        
        
'        Dim bsave As Boolean
'        bsave = False
'
'        For j = 1 To tmp_info.JpCount
'            If Left(tmp_info.JpText(j), 1) = "��" Then
'                Dim Repeat_Script As SCRIPTINFO
'                Dim Repeat_Sentence As Long
'                Dim tempstr As String
'                Dim str() As String
'
'                tempstr = Left(tmp_info.JpText(j), 30)
'                tempstr = Mid(tempstr, 2)
'                str = Split(tempstr, "-")
'
'                Repeat_Script = Script_GetInfo(str(0))
'                Repeat_Sentence = Val(str(1))
'                If UBound(Repeat_Script.CnText) >= Repeat_Sentence And Repeat_Script.CnText(Repeat_Sentence) <> "" Then
'                    tmp_info.CnText(j) = Repeat_Script.CnText(Repeat_Sentence)
'                    bsave = True
'                End If
'            End If
'        Next j
'
'        If bsave = True Then
'            Script_Save g_CacheInfo(I).sID, tmp_info, LANGUAGE_CN
'        End If
        
        
        
        
        DoEvents
        
    Next I

    Call Cache_Save
    
    Form2.info "�����ܼ�����" + str(total_words) + ",�ѷ���" + str(total_trans)
    
End Sub

Private Sub CommonGrid_Init()

    Grid1.cols = 6
    Grid1.ColAlignment(0) = flexAlignLeftTop
    Grid1.ColAlignment(1) = flexAlignCenterCenter
    Grid1.ColAlignment(2) = flexAlignCenterCenter
    Grid1.ColAlignment(3) = flexAlignRightTop
    Grid1.ColAlignment(4) = flexAlignCenterCenter
    Grid1.ColAlignment(5) = flexAlignLeftTop
    
    Grid1.ColWidth(0) = 0   '��������
    Grid1.ColWidth(1) = Grid1.Width * 0.2
    Grid1.ColWidth(2) = Grid1.Width * 0.1
    Grid1.ColWidth(3) = Grid1.Width * 0.1
    Grid1.ColWidth(4) = Grid1.Width * 0.12
    Grid1.ColWidth(5) = Grid1.Width * 0.53
    
    
    Grid1.TextMatrix(0, 0) = "No."
    Grid1.TextMatrix(0, 1) = "ID"
    Grid1.TextMatrix(0, 2) = "OK / ALL"
    Grid1.TextMatrix(0, 3) = "����"
    Grid1.TextMatrix(0, 4) = "����"
    Grid1.TextMatrix(0, 5) = "��ע"

    Grid1.rows = 1

    Dim I As Long
    
    Grid1.rows = UBound(g_CacheInfo) + 1
    
    For I = 1 To UBound(g_CacheInfo)
        Grid1.TextMatrix(I, 0) = g_CacheInfo(I).sNo
        Grid1.TextMatrix(I, 1) = g_CacheInfo(I).sID
        Grid1.TextMatrix(I, 2) = g_CacheInfo(I).nTrans & " / " & g_CacheInfo(I).nTotal
        Grid1.TextMatrix(I, 3) = g_CacheInfo(I).nWords
        Grid1.TextMatrix(I, 4) = g_CacheInfo(I).sAuthor
        Grid1.TextMatrix(I, 5) = g_CacheInfo(I).sMemo
        
        If g_ViewUnfinish And g_CacheInfo(I).nTrans = g_CacheInfo(I).nTotal Then
            Grid1.RowHeight(I) = 0
        End If
        
        If g_ViewComplete And g_CacheInfo(I).nTrans < g_CacheInfo(I).nTotal Then
            Grid1.RowHeight(I) = 0
        End If
    
        If g_hide_repeat And g_CacheInfo(I).nTotal = 0 Then
            Grid1.RowHeight(I) = 0
        End If
    Next I
    
End Sub

Private Sub EventGrid_Init()

    Grid2.cols = 2
    Grid2.ColAlignment(0) = flexAlignLeftTop
    Grid2.ColAlignment(1) = flexAlignLeftTop
    
    Grid2.ColWidth(0) = 0   '����ʾ
    Grid2.ColWidth(1) = Grid1.Width * 0.9
  
    Grid2.TextMatrix(0, 0) = "No."
    Grid2.TextMatrix(0, 1) = "Event"

    Grid2.rows = 1
   
    If Dir(g_Dir & "\event", vbDirectory) = "" Then
        Exit Sub
    End If

    lstScript.Path = g_Dir & "\event"
    lstScript.Pattern = "*.evt"
    lstScript.Refresh
    
    Grid2.rows = lstScript.ListCount + 1
    
    Dim I As Long
    For I = 1 To lstScript.ListCount
        
        Grid2.TextMatrix(I, 0) = I
        Grid2.TextMatrix(I, 1) = Tool_GetFilenameMain(lstScript.List(I - 1))
        
        DoEvents
        
    Next I
    
End Sub

Public Sub Grid_Update(nRow As Long)
    
    Dim sID As String
    sID = g_CacheInfo(nRow).sID
        
    Dim info As SCRIPTINFO
     
    info = Script_GetInfo(sID)
    g_CacheInfo(nRow).nTrans = info.CnCount
    g_CacheInfo(nRow).sAuthor = info.User
    g_CacheInfo(nRow).sMemo = info.Memo
        
    Grid1.TextMatrix(nRow, 2) = g_CacheInfo(nRow).nTrans & " / " & g_CacheInfo(nRow).nTotal
    Grid1.TextMatrix(nRow, 4) = g_CacheInfo(nRow).sAuthor
    Grid1.TextMatrix(nRow, 5) = g_CacheInfo(nRow).sMemo
        
End Sub


Public Sub info(s)
    txtInfo.Text = txtInfo.Text & s & vbCrLf
    txtInfo.SelStart = Len(txtInfo.Text)
    txtInfo.Refresh
    DoEvents
End Sub

Public Sub infoNoCr(s)
    txtInfo.Text = txtInfo.Text & s
    txtInfo.SelStart = Len(txtInfo.Text)
    txtInfo.Refresh
    DoEvents
End Sub

Private Sub mnuAbout_Click()
    MsgBox vbCrLf & "-Script Editor- is open source project." & vbCrLf & "for more information, pls check " & vbCrLf & "http://agemo.126.com/" & vbCrLf & vbCrLf & "(C) Agemo 2003-2006" & vbCrLf
End Sub

Private Sub mnuCalcWords_Click()
    txtInfo.Text = ""
    info "����ͳ��.... pls wait..."
    
    
    Dim I&, j&
    Dim inf As SCRIPTINFO
    
    Dim strTxtCN As String
    
    Dim nTotalJPCount&
    nTotalJPCount = 0
    
    Dim nTotalCNCount&
    nTotalCNCount = 0
    
    Dim nStart&, nEnd&
    
    If Grid1.RowSel >= Grid1.Row Then
        nStart = Grid1.Row
        nEnd = Grid1.RowSel
    Else
        nStart = Grid1.RowSel
        nEnd = Grid1.Row
    End If
    
    For I = nStart To nEnd
        
        Dim sID$
    
        sID = g_CacheInfo(I).sID
        
        inf = Script_GetInfo(sID)
        
        For j = 1 To inf.JpCount
            nTotalJPCount = nTotalJPCount + Tool_GetWords(inf.JpText(j), LANGUAGE_JP)
            nTotalCNCount = nTotalCNCount + Tool_GetWords(inf.CnText(j), LANGUAGE_CN)
        Next j

        DoEvents
        
    Next I
    
    Form2.info "�������� " & nTotalJPCount
    Form2.info "�������� " & nTotalCNCount

End Sub


Private Sub mnuCharCounter_Click()
    txtInfo.Text = ""
    info "����ͳ��.... pls wait..."
    
    
    Dim I&, j&
    Dim inf As SCRIPTINFO
    
    Dim w() As String, c() As Long
    ReDim w(0 To 0) As String
    ReDim c(0 To 0) As Long
    
    Dim sGlobalText As String
    
    sGlobalText = ""
    
    For I = 1 To UBound(g_CacheInfo)
        
        Dim sID$
        sID = g_CacheInfo(I).sID
        inf = Script_GetInfo(sID)
        DoEvents
        For j = 1 To inf.JpCount
            sGlobalText = sGlobalText & Tool_ControlCodeRemoveEx(inf.CnText(j))
        Next j
        
    Next I
    
    Word_Counter sGlobalText, w, c
    
    sGlobalText = ""
    For I = 1 To UBound(w)
        sGlobalText = sGlobalText & w(I) & Chr(9) & c(I) & vbCrLf
    Next I
    
    Tool_WriteTextFile g_Dir & "\char.txt", sGlobalText
    info "ͳ����ϣ��Ѿ�����Ϊ����Ŀ¼�µ� char.txt"
End Sub


Private Sub mnuFastExit_Click()
    g_NeedSaveCache = False
    Form1.OnClickClose
    End
End Sub

Private Sub mnuJCA_Click()
    If Not mnuJCA.Checked Then
        mnuJCU.Checked = False
        mnuJC.Checked = False
        mnuJCA.Checked = True
        mnuJUCAlter.Checked = False
    End If
End Sub

Private Sub mnuJUCAlter_Click()
    If Not mnuJUCAlter.Checked Then
        mnuJUCAlter.Checked = True
        mnuJCU.Checked = False
        mnuJC.Checked = False
        mnuJCA.Checked = False
    End If
End Sub

Private Sub mnuJC_Click()
    If Not mnuJC.Checked Then
        mnuJC.Checked = True
        mnuJCU.Checked = False
        mnuJUCAlter.Checked = False
        mnuJCA.Checked = False
    End If
End Sub

Private Sub mnuJCU_Click()
    If Not mnuJCU.Checked Then
        mnuJCU.Checked = True
        mnuJC.Checked = False
        mnuJUCAlter.Checked = False
        mnuJCA.Checked = False
    End If
End Sub

Private Sub mnuModeCommon_Click()
    If Not mnuModeCommon.Checked Then
        mnuModeCommon.Checked = True
        mnuModeEvent.Checked = False
        On_Mode_Click
    End If
End Sub

Private Sub mnuModeEvent_Click()
    If Not mnuModeEvent.Checked Then
        mnuModeEvent.Checked = True
        EventGrid_Init
        mnuModeCommon.Checked = False
        On_Mode_Click
    End If
End Sub
Private Sub On_Mode_Click()
    If mnuModeEvent.Checked Then
        Grid1.Visible = False
        Grid2.Visible = True
    Else
        Grid1.Visible = True
        Grid2.Visible = False
    End If
End Sub


Private Sub mnuOption_Click()
    Form5.Show 1
    CommonGrid_Init
End Sub


Private Sub mnuRefresh_Click()
    info "refreshing, pls wait"
    Cache_Rebuild (True)
    CommonGrid_Init
    EventGrid_Init
    info "done"
End Sub

Private Sub mnuRepeatSync_Click()

    Form2.info "����ͬ��..."

    Dim I&, j&, s$
    Dim tmp_info As SCRIPTINFO
    
    lstScript.Path = g_Dir & "\jp-text"
    lstScript.Pattern = "*.txt"
    lstScript.Refresh
    
    If lstScript.ListCount = 0 Then
        Exit Sub
    End If
    
    Dim tempID As String
    
    
    
    For I = 1 To lstScript.ListCount
        
        tempID = Tool_GetFilenameMain(lstScript.List(I - 1))
        tmp_info = Script_GetInfo(tempID)
        
        
        Dim bsave As Boolean
        bsave = False

        For j = 1 To tmp_info.JpCount
            If Left(tmp_info.JpText(j), 1) = "��" Then
                Dim Repeat_Script As SCRIPTINFO
                Dim Repeat_Sentence As Long
                Dim tempstr As String
                Dim str() As String

                tempstr = Left(tmp_info.JpText(j), 30)
                tempstr = Mid(tempstr, 2)
                str = Split(tempstr, "-")

                Repeat_Script = Script_GetInfo(str(0))
                Repeat_Sentence = Val(str(1))
                If UBound(Repeat_Script.CnText) >= Repeat_Sentence And Repeat_Script.CnText(Repeat_Sentence) <> "" Then
                    tmp_info.CnText(j) = Repeat_Script.CnText(Repeat_Sentence)
                    bsave = True
                End If
            End If
        Next j

        If bsave = True Then
            Script_Save tempID, tmp_info, LANGUAGE_CN
        End If
        
        
        
        
        DoEvents
        
    Next I
    

    Form2.info "�ظ��ı���ͬ��"


End Sub

Private Sub mnuSearch_Click()
    Form4.Show
End Sub

Private Sub mnuViewAll_Click()
g_ViewComplete = False
g_ViewUnfinish = False
mnuViewAll.Checked = True
mnuViewComplete.Checked = False
mnuViewUnfinish.Checked = False
CommonGrid_Init
End Sub

Private Sub mnuViewComplete_Click()
g_ViewComplete = True
g_ViewUnfinish = False
mnuViewAll.Checked = False
mnuViewComplete.Checked = True
mnuViewUnfinish.Checked = False
CommonGrid_Init
End Sub

Private Sub mnuViewUnfinish_Click()
g_ViewComplete = False
g_ViewUnfinish = True
mnuViewAll.Checked = False
mnuViewComplete.Checked = False
mnuViewUnfinish.Checked = True
CommonGrid_Init
End Sub
