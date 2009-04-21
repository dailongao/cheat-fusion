VERSION 5.00
Object = "{EAB22AC0-30C1-11CF-A7EB-0000C05BAE0B}#1.1#0"; "shdocvw.dll"
Object = "{508D1EAC-D8DB-4512-A4E5-2437B500C602}#1.0#0"; "SynMemoU.ocx"
Begin VB.Form Form1 
   Caption         =   "Edit"
   ClientHeight    =   11115
   ClientLeft      =   65
   ClientTop       =   455
   ClientWidth     =   14781
   Icon            =   "Preview.frx":0000
   LinkTopic       =   "Form1"
   MinButton       =   0   'False
   ScaleHeight     =   11115
   ScaleWidth      =   14781
   StartUpPosition =   2  '屏幕中心
   WindowState     =   2  'Maximized
   Begin SynMemoU.SynMemoX txtCN 
      Height          =   2535
      Left            =   0
      TabIndex        =   13
      Top             =   7440
      Width           =   9135
      Color           =   14737632
      ActiveLineColor =   16777152
      Ctl3D           =   -1  'True
      ParentCtl3D     =   -1  'True
      Enabled         =   0   'False
      BeginProperty Font {0BE35203-8F91-11CE-9DE3-00AA004BB851} 
         Name            =   "宋体"
         Size            =   11.3333
         Charset         =   134
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      ParentColor     =   0   'False
      Object.Visible         =   -1  'True
      BorderStyle     =   1
      ExtraLineSpacing=   0
      HideSelection   =   0   'False
      ImeMode         =   3
      ImeName         =   ""
      InsertCaret     =   0
      InsertMode      =   -1  'True
      MaxScrollWidth  =   1024
      MaxUndo         =   1024
      OverwriteCaret  =   3
      ReadOnly        =   0   'False
      RightEdge       =   80
      RightEdgeColor  =   12632256
      ScrollHintColor =   -16777192
      ScrollHintFormat=   0
      ScrollBars      =   3
      SelectionMode   =   0
      TabWidth        =   8
      WantReturns     =   -1  'True
      WantTabs        =   0   'False
      WordWrap        =   0   'False
      SelStart        =   0
      SelEnd          =   0
      AlwaysShowCaret =   0   'False
      CaretX          =   1
      CaretY          =   1
      LeftChar        =   1
      LineText        =   ""
      Modified        =   0   'False
      SelLength       =   0
      SelText         =   ""
      Text            =   ""
      TopLine         =   1
      ActiveSelectionMode=   0
      DoubleBuffered  =   0   'False
   End
   Begin VB.CommandButton cmdSaveCN 
      Caption         =   "保存+预览(自动)"
      BeginProperty Font 
         Name            =   "宋体"
         Size            =   9.34
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   330
      Left            =   4920
      TabIndex        =   2
      Top             =   10200
      Visible         =   0   'False
      Width           =   2055
   End
   Begin SHDocVwCtl.WebBrowser wbRuler 
      CausesValidation=   0   'False
      Height          =   375
      Left            =   0
      TabIndex        =   0
      TabStop         =   0   'False
      Top             =   15
      Width           =   14040
      ExtentX         =   24765
      ExtentY         =   661
      ViewMode        =   0
      Offline         =   0
      Silent          =   0
      RegisterAsBrowser=   0
      RegisterAsDropTarget=   0
      AutoArrange     =   0   'False
      NoClientEdge    =   0   'False
      AlignLeft       =   0   'False
      NoWebView       =   0   'False
      HideFileNames   =   0   'False
      SingleClick     =   0   'False
      SingleSelection =   0   'False
      NoFolders       =   0   'False
      Transparent     =   0   'False
      ViewID          =   "{0057D0E0-3573-11CF-AE69-08002B2E1262}"
      Location        =   ""
   End
   Begin SHDocVwCtl.WebBrowser wbMain 
      Height          =   7005
      Left            =   -15
      TabIndex        =   1
      Top             =   345
      Width           =   13935
      ExtentX         =   24580
      ExtentY         =   12356
      ViewMode        =   0
      Offline         =   0
      Silent          =   0
      RegisterAsBrowser=   0
      RegisterAsDropTarget=   1
      AutoArrange     =   0   'False
      NoClientEdge    =   0   'False
      AlignLeft       =   0   'False
      NoWebView       =   0   'False
      HideFileNames   =   0   'False
      SingleClick     =   0   'False
      SingleSelection =   0   'False
      NoFolders       =   0   'False
      Transparent     =   0   'False
      ViewID          =   "{0057D0E0-3573-11CF-AE69-08002B2E1262}"
      Location        =   ""
   End
   Begin VB.Frame FrameTool 
      Height          =   2415
      Left            =   9240
      TabIndex        =   3
      Top             =   7440
      Width           =   4695
      Begin VB.CommandButton btnReloadSyn 
         Caption         =   "刷新关键字"
         Height          =   375
         Left            =   120
         TabIndex        =   14
         Top             =   1920
         Width           =   1335
      End
      Begin VB.CommandButton cmdLoadCN 
         Caption         =   "Reload"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   7.33
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   375
         Left            =   1320
         TabIndex        =   12
         Top             =   240
         Width           =   1335
      End
      Begin VB.CheckBox chkCode 
         Caption         =   "显示控制字符"
         Height          =   375
         Left            =   120
         TabIndex        =   11
         Top             =   720
         Width           =   1575
      End
      Begin VB.CommandButton cmdCopyJP 
         Caption         =   "↓"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   7.33
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   375
         Left            =   720
         TabIndex        =   10
         ToolTipText     =   "用日文原文覆盖"
         Top             =   240
         Width           =   495
      End
      Begin VB.TextBox txtNo 
         Height          =   375
         Left            =   120
         TabIndex        =   9
         Text            =   "Text1"
         ToolTipText     =   "当前选中的句子"
         Top             =   240
         Width           =   495
      End
      Begin VB.CheckBox chkModFilter 
         Caption         =   "仅显示修改的"
         Height          =   375
         Left            =   2880
         TabIndex        =   8
         Top             =   720
         Width           =   1455
      End
      Begin VB.Frame Frame2 
         Caption         =   "编辑字体大小"
         Height          =   615
         Left            =   2880
         TabIndex        =   6
         Top             =   1080
         Width           =   1455
         Begin VB.TextBox txtFontSize 
            Height          =   285
            Left            =   120
            TabIndex        =   7
            Text            =   "11"
            Top             =   240
            Width           =   1215
         End
      End
      Begin VB.TextBox txtInfo 
         Height          =   615
         Left            =   120
         MultiLine       =   -1  'True
         TabIndex        =   5
         Text            =   "Preview.frx":030A
         ToolTipText     =   "提示信息"
         Top             =   1080
         Width           =   2535
      End
      Begin VB.CommandButton cmdAlter 
         Caption         =   "复制此句译文从cn-compare\"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   7.33
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   495
         Left            =   2880
         TabIndex        =   4
         Top             =   120
         Width           =   1575
      End
   End
End
Attribute VB_Name = "Form1"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit

Private g_CurSentence As Long    '当前选择第几句
Private g_Script As SCRIPTINFO
Private g_IsDirty As Boolean

Private g_CurParagraph  As Long   '主画面的第几段

Private g_is_compare_mode As Boolean

Private Function GetControlCodeHtml(s As String)
    GetControlCodeHtml = Replace(s, vbCrLf, "<br>")
    If GetControlCodeHtml = "" Then GetControlCodeHtml = "&nbsp;"
End Function

Public Sub Load(sNo As Long, sPreviewFile As String)
    txtCN.Enabled = False
    txtInfo.Text = ""
    txtNo.Text = ""
    cmdCopyJP.Enabled = False
    
    Dim sID As String
    
    If g_CurParagraph <> 0 And g_Script.isEventMode = False Then Form2.Grid_Update g_CurParagraph
    
    g_CurParagraph = CLng(sNo)
    
    sID = g_CacheInfo(g_CurParagraph).sID
    g_Script = Script_GetInfo(sID)
    
    
    Form1.Caption = sID
    txtCN.Text = ""
    g_IsDirty = False
    
    
    Dim tpl_body As String, tpl_row As String   '读入模板
    Dim tpl_js As String, tpl_ruler$
    Dim tpl_dir As String
    
    g_is_compare_mode = False
    
    If Form2.mnuJCU.Checked = True Then tpl_dir = "J-C-U"
    If Form2.mnuJC.Checked = True Then tpl_dir = "J-C"
    If Form2.mnuJUCAlter.Checked = True Then
        cmdAlter.Visible = True
        chkModFilter.Visible = True
        tpl_dir = "J-U-C-Compare"
        g_is_compare_mode = True
    ElseIf Form2.mnuJCA.Checked = True Then
        cmdAlter.Visible = True
        chkModFilter.Visible = True
        tpl_dir = "J-C-Compare"
        g_is_compare_mode = True
    Else
        cmdAlter.Visible = False
        chkModFilter.Visible = False
    End If

    
    tpl_dir = g_Dir & "\template\" & tpl_dir
    
    tpl_js = Tool_LoadTextFile(g_Dir & "\template\" & "\js.html")
    tpl_row = Tool_LoadTextFile(tpl_dir & "\row.html")
    TemplateSetColSize tpl_row
    
    tpl_body = Tool_LoadTextFile(tpl_dir & "\body.html")
    TemplateSetColSize tpl_body
    
    tpl_ruler = Tool_LoadTextFile(tpl_dir & "\ruler.html")
    TemplateSetColSize tpl_ruler
    

    
    Dim rows As String
    Dim s As String
    Dim sItemJP As String
    Dim sItemUS As String
    Dim sItemCN As String
    Dim sItemCNCompare As String
    
    rows = ""
        
    Dim I As Long
    Dim nAlters As Long
    
    nAlters = 0
    
    '标记第一行说话者
    'Dim sTalker As String
    'Dim PageTalkerCount As Long
    'Dim PageTalkerFind As Long
    
    'PageTalkerCount = 0
    'sTalker = Trim(txtTalker.Text)
    
    For I = 1 To g_Script.JpCount
    
        s = Replace(tpl_row, "TEMPLATE_ADDR", g_Script.Address(I))
        s = Replace(s, "TEMPLATE_ID", I)

        
        If chkCode.Value = 1 Then
            sItemJP = GetControlCodeHtml(g_Script.JpText(I))
            sItemUS = GetControlCodeHtml(g_Script.UsText(I))
            sItemCN = GetControlCodeHtml(g_Script.CnText(I))
        Else
            sItemJP = Callback_Preview(g_Script.JpText(I), LANGUAGE_JP)
            sItemUS = Callback_Preview(g_Script.UsText(I), LANGUAGE_US)
            sItemCN = Callback_Preview(g_Script.CnText(I), LANGUAGE_CN)
        End If
     '   PageTalkerFind = 0
     '   If sTalker <> "" Then
     '       If InStr(1, sItemCN, sTalker & "<br>") = 1 Then PageTalkerFind = 1
     '       If InStr(1, sItemCN, "<b>" & sTalker & "</b><br>") = 1 Then PageTalkerFind = 1
     '   End If
        
        sItemCNCompare = ""
        
        If g_is_compare_mode Then
            
            If g_Script.CnTextAlter(I) <> g_Script.CnText(I) And g_Script.CnText(I) <> "" Then
                If chkCode.Value = 1 Then
                    sItemCNCompare = GetControlCodeHtml(g_Script.CnTextAlter(I))
                Else
                    sItemCNCompare = Callback_Preview(g_Script.CnTextAlter(I), LANGUAGE_CN)
                End If
                nAlters = nAlters + 1
                MarkTextDiffHTML sItemCN, sItemCNCompare
            ElseIf chkModFilter.Value = 1 Then
                GoTo for_i_next
            End If
            
        End If
        
        If (sItemJP = "&nbsp;" Or sItemJP = "") And (sItemCN = "&nbsp;" Or sItemCN = "") Then
            '日文预览为空的，形成表格融合的效果，这样一眼能看到
            s = Replace(s, "TEMPLATE_JP", "")
            s = Replace(s, "TEMPLATE_US", "")
            s = Replace(s, "TEMPLATE_CN", "")
            s = Replace(s, "TEMPLATE_COMPARE", "")
            s = Replace(s, "TEMPLATE_MARK_COLOR", "#CCBB99")
            
            's = "" '此处改为空的，可以使得不显示。
        Else
            s = Replace(s, "TEMPLATE_JP", sItemJP)
            s = Replace(s, "TEMPLATE_US", sItemUS)
            s = Replace(s, "TEMPLATE_CN", sItemCN)
            s = Replace(s, "TEMPLATE_COMPARE", sItemCNCompare)
            
            'If PageTalkerFind = 0 Then
                s = Replace(s, "TEMPLATE_MARK_COLOR", "#CCCC99")
            'Else
            '    s = Replace(s, "TEMPLATE_MARK_COLOR", "#8888CC")
            'End If
        End If
        
        
        
        If g_hide_repeat Then
            If Left(g_Script.JpText(I), 1) = "〓" Then
                s = Replace(s, "DISPLAY_FLAG", "style='display:none'")
            End If
            
        End If
        
        If g_Script.JpText(I) = "" Then
            s = Replace(s, "DISPLAY_FLAG", "style='display:none'")
        Else
            s = Replace(s, "DISPLAY_FLAG", "")
        End If
        
        rows = rows & s

for_i_next:
    Next I
    
    '如果美版比日版多的话
    For I = g_Script.JpCount + 1 To g_Script.UsCount
        s = tpl_row
        s = Replace(s, "TEMPLATE_ID", "U" & I)
        s = Replace(s, "TEMPLATE_ADDR", "N/A")
        
        sItemUS = Callback_Preview(g_Script.UsText(I), LANGUAGE_US)
        
        
        s = Replace(s, "TEMPLATE_JP", "US edition only. " & vbCrLf & "do NOT translation")
        s = Replace(s, "TEMPLATE_CN", "US edition only. " & vbCrLf & "do NOT translation")
        s = Replace(s, "TEMPLATE_US", sItemUS)
        s = Replace(s, "TEMPLATE_MARK_COLOR", "#ccbb99")
        s = Replace(s, "TEMPLATE_COMPARE", "&nbsp;")
        
        If sItemUS = "" Or sItemUS = "&nbsp;" Then s = ""
        
        rows = rows & s
        
    Next I
    
    Dim sAll As String
    Dim sHTMLFile As String

    sAll = Replace(tpl_body, "<!-- TEMPLATE_ROWS -->", rows)
    
    
    
    sAll = sAll & tpl_js
    
    If sPreviewFile = "" Then sPreviewFile = "preview.html"
    
   
    sHTMLFile = g_Dir & "\template\temp\" & sPreviewFile
    Tool_WriteTextFile sHTMLFile, sAll
    wbMain.Navigate2 sHTMLFile
    
    sHTMLFile = g_Dir & "\template\temp\ruler.htm"
    Tool_WriteTextFile sHTMLFile, tpl_ruler
    wbRuler.Navigate2 sHTMLFile
    
    If g_is_compare_mode Then
        If g_Script.CnAlterCount = 0 Then
            info "本段不存在对照译文"
        Else
            info "本段译文修改句数： " & nAlters
        End If
    End If

End Sub



Public Sub LoadEvent(sEventID As String, sPreviewFile As String)
    Dim sID As String
    
    If g_CurParagraph <> 0 And g_Script.isEventMode = False Then Form2.Grid_Update g_CurParagraph
    
    g_CurParagraph = -1         'CLng(sNo)
    
    g_Script = Script_GetEventInfo(sEventID)
    
    Form1.Caption = sEventID
    txtCN.Text = ""
    g_IsDirty = False
    
    Dim tpl_body As String, tpl_row As String   '读入模板
    Dim tpl_js As String, tpl_ruler As String
    Dim tpl_dir As String, tpl_scene As String
    
    
    g_is_compare_mode = False
    
    If Form2.mnuJCU.Checked = True Then tpl_dir = "J-C-U"
    If Form2.mnuJC.Checked = True Then tpl_dir = "J-C"
    If Form2.mnuJUCAlter.Checked = True Then
        cmdAlter.Visible = True
        chkModFilter.Visible = True
        tpl_dir = "J-U-C-Compare"
        g_is_compare_mode = True
    ElseIf Form2.mnuJCA.Checked = True Then
        cmdAlter.Visible = True
        chkModFilter.Visible = True
        tpl_dir = "J-C-Compare"
        g_is_compare_mode = True
    Else
        cmdAlter.Visible = False
        chkModFilter.Visible = False
    End If

    tpl_dir = g_Dir & "\template\" & tpl_dir
    
    tpl_js = Tool_LoadTextFile(g_Dir & "\template\" & "\js.html")
    tpl_scene = Tool_LoadTextFile(tpl_dir & "\scene.html")
    
    tpl_row = Tool_LoadTextFile(tpl_dir & "\row.html")
    TemplateSetColSize tpl_row
    
    tpl_body = Tool_LoadTextFile(tpl_dir & "\body.html")
    TemplateSetColSize tpl_body
    
    tpl_ruler = Tool_LoadTextFile(tpl_dir & "\ruler.html")
    TemplateSetColSize tpl_ruler
    
    Dim rows As String
    Dim s As String
    Dim sItemJP As String
    Dim sItemUS As String
    Dim sItemCN As String
    Dim sItemCNCompare As String
    
    rows = ""
        
    Dim I As Long
    Dim nAlters As Long
    
    nAlters = 0
    
    '标记第一行说话者
    'Dim sTalker As String
    'Dim PageTalkerCount As Long
    'Dim PageTalkerFind As Long
    
    'PageTalkerCount = 0
    'sTalker = Trim(txtTalker.Text)
    
    
    Dim sCurrentID As String

    sCurrentID = ""
    
    For I = 1 To g_Script.JpCount
    
        If g_Script.arrID(I) <> sCurrentID Then
            sCurrentID = g_Script.arrID(I)
            Dim sMemo$, sUser$
            Script_MemoLoad sCurrentID, sUser, sMemo
            sUser = sCurrentID & " - " & sMemo
            s = Replace(tpl_scene, "SCENE_INFO", sUser)
            
            rows = rows & s
        End If
        
    
        s = tpl_row
        s = Replace(s, "TEMPLATE_ADDR", g_Script.Address(I))
        s = Replace(s, "TEMPLATE_ID", I)
        
        sItemJP = Callback_Preview(g_Script.JpText(I), LANGUAGE_JP)
        sItemUS = Callback_Preview(g_Script.UsText(I), LANGUAGE_US)
        sItemCN = Callback_Preview(g_Script.CnText(I), LANGUAGE_CN)
        
     '   PageTalkerFind = 0
     '   If sTalker <> "" Then
     '       If InStr(1, sItemCN, sTalker & "<br>") = 1 Then PageTalkerFind = 1
     '       If InStr(1, sItemCN, "<b>" & sTalker & "</b><br>") = 1 Then PageTalkerFind = 1
     '   End If
        
        sItemCNCompare = ""
        
        If g_is_compare_mode Then
            
            If g_Script.CnTextAlter(I) <> g_Script.CnText(I) And g_Script.CnText(I) <> "" Then
                sItemCNCompare = Callback_Preview(g_Script.CnTextAlter(I), LANGUAGE_CN)
                nAlters = nAlters + 1
                MarkTextDiffHTML sItemCN, sItemCNCompare
            End If
        End If
        
        If (sItemJP = "&nbsp;" Or sItemJP = "") And (sItemUS = "&nbsp;" Or sItemUS = "") And (sItemCN = "&nbsp;" Or sItemCN = "") Then
            '日文预览为空的，形成表格融合的效果，这样一眼能看到
            s = Replace(s, "TEMPLATE_JP", "")
            s = Replace(s, "TEMPLATE_US", "")
            s = Replace(s, "TEMPLATE_CN", "")
            s = Replace(s, "TEMPLATE_COMPARE", "")
            s = Replace(s, "TEMPLATE_MARK_COLOR", "#CCBB99")
        Else
            s = Replace(s, "TEMPLATE_JP", sItemJP)
            s = Replace(s, "TEMPLATE_US", sItemUS)
            s = Replace(s, "TEMPLATE_CN", sItemCN)
            s = Replace(s, "TEMPLATE_COMPARE", sItemCNCompare)
            
            'If PageTalkerFind = 0 Then
                s = Replace(s, "TEMPLATE_MARK_COLOR", "#CCCC99")
            'Else
            '    s = Replace(s, "TEMPLATE_MARK_COLOR", "#8888CC")
            'End If
        End If
        
        rows = rows & s
        
    Next I
    
    '如果美版比日版多的话
    For I = g_Script.JpCount + 1 To g_Script.UsCount
        s = tpl_row
        s = Replace(s, "TEMPLATE_ID", "U" & I)
        
        sItemUS = Callback_Preview(g_Script.UsText(I), LANGUAGE_US)
        s = Replace(s, "TEMPLATE_ADDR", "N/A")
        s = Replace(s, "TEMPLATE_JP", "Us Only. " & vbCrLf & "Do NOT Translation")
        s = Replace(s, "TEMPLATE_CN", "Us Only. " & vbCrLf & "Do NOT Translation")
        s = Replace(s, "TEMPLATE_US", sItemUS)
        s = Replace(s, "TEMPLATE_MARK_COLOR", "#CCbb99")
        s = Replace(s, "TEMPLATE_COMPARE", "&nbsp;")
        
        rows = rows & s
        
    Next I
    
    Dim sAll As String
    Dim sHTMLFile As String

    sAll = Replace(tpl_body, "<!-- TEMPLATE_ROWS -->", rows)
    
    
    
    sAll = sAll & tpl_js
    
    If sPreviewFile = "" Then sPreviewFile = "preview.html"
    
    sHTMLFile = g_Dir & "\template\temp\" & sPreviewFile
    Tool_WriteTextFile sHTMLFile, sAll
    wbMain.Navigate2 sHTMLFile
    
    sHTMLFile = g_Dir & "\template\temp\ruler.htm"
    Tool_WriteTextFile sHTMLFile, tpl_ruler
    wbRuler.Navigate2 sHTMLFile
    
    If g_is_compare_mode Then
        If g_Script.CnAlterCount = 0 Then
            info "本段不存在对照译文"
        Else
            info "本段译文修改句数： " & nAlters
        End If
    End If

End Sub






Private Sub btnReloadSyn_Click()
  txtCN.LoadSynHighlight (g_Dir)
End Sub

Private Sub chkCode_Click()
    cmdLoadCN_Click
End Sub

Private Sub cmdAlter_Click()

    If vbOK <> MsgBox("此句、用的修改译文覆盖现有译文吗？", vbDefaultButton2 + vbOKCancel, "") Then
        Exit Sub
    End If
    
    If g_Script.CnTextAlter(g_CurSentence) = "" Then
        MsgBox "此句，修改后译文为空，不能覆盖", vbExclamation
        Exit Sub
    End If

    If g_Script.CnTextAlter(g_CurSentence) = g_Script.CnText(g_CurSentence) Then
        MsgBox "此句，没有修改", vbExclamation
        Exit Sub
    End If

    txtCN.Text = g_Script.CnTextAlter(g_CurSentence)
    g_IsDirty = True
    DoEvents
    
End Sub



Private Sub cmdCopyJP_Click()

    If txtCN.Text <> "" Then
        If vbOK <> MsgBox("已存在翻译文本, 是否覆盖", vbDefaultButton2 + vbOKCancel, "") Then
            Exit Sub
        End If
    End If
    
    If Left(g_Script.JpText(g_CurSentence), 1) = "〓" Then
    '如果是重复文本,就用已翻译文本
        Dim Repeat_Script As SCRIPTINFO
        Dim Repeat_Sentence As Long
        Dim tempstr As String
        Dim str() As String

        tempstr = Left(g_Script.JpText(g_CurSentence), 40)
        tempstr = Mid(tempstr, 2)
        str = Split(tempstr, "-")
            
        Repeat_Script = Script_GetInfo(str(0))
        Repeat_Sentence = Val(str(1))
        txtCN.Text = Repeat_Script.CnText(Repeat_Sentence)
        '如果已翻译文本实际上没翻译也还是空,那就用日版文本
        If txtCN.Text = "" Then
            MsgBox ("重复文本的对照源文本未翻译")
            txtCN.Text = Tool_WideConv(g_Script.JpText(g_CurSentence))
        End If
    Else
        txtCN.Text = Tool_WideConv(g_Script.JpText(g_CurSentence))
    End If
    g_IsDirty = True
    DoEvents
    
End Sub

Private Sub cmdLoadCN_Click()
    If g_Script.isEventMode Then
        LoadEvent g_Script.ID, ""
    Else
        Load g_CurParagraph, ""
    End If
End Sub

Private Sub cmdSaveCN_Click()
    g_IsDirty = False

    g_Script.CnText(g_CurSentence) = Callback_Save(txtCN.Text)
    
    '///////////////////////////////////////////
    '修改预览
    '///////////////////////////////////////////
    Dim doc As IHTMLDocument2
    Dim ObjID As IHTMLElement
    Set doc = wbMain.Document
    
    Dim sItemCN$, sItemCNCompare$
    
    sItemCN = Callback_Preview(g_Script.CnText(g_CurSentence), LANGUAGE_CN)
    
    If g_is_compare_mode Then
        sItemCNCompare = Callback_Preview(g_Script.CnTextAlter(g_CurSentence), LANGUAGE_CN)
        MarkTextDiffHTML sItemCN, sItemCNCompare
            
        Set ObjID = doc.All("TD_COMPARE_" & g_CurSentence)
        ObjID.innerHTML = sItemCNCompare
    End If
    
    Set ObjID = doc.All("PREVIEW_" & g_CurSentence)
    ObjID.innerHTML = sItemCN
    
    '///////////////////////////////////////////
    'save file
    '///////////////////////////////////////////
    If g_Script.isEventMode Then
        Script_SaveEvent g_Script.arrID(g_CurSentence), g_Script.arrNum(g_CurSentence), g_Script.CnText(g_CurSentence)
        Form1.info "event saved"
    Else
        Script_Save g_CacheInfo(g_CurParagraph).sID, g_Script, LANGUAGE_CN
        Form1.info "" & g_CacheInfo(g_CurParagraph).sID & " saved"
    End If
    
    
    
    

    
End Sub






Public Sub OnClickClose()
    If g_IsDirty Then
        cmdSaveCN_Click
    End If
    
    
End Sub



Private Sub Form_Unload(Cancel As Integer)
    Cancel = True
    Me.Hide
    
    OnClickClose
    
    If g_Script.isEventMode = False Then Form2.Grid_Update g_CurParagraph
    
End Sub

Private Sub Form_Resize()
    Dim w As Long
    Dim h As Long
    
    w = Form1.Width - 200
    If w <= 1000 Then w = 1000
    
    h = Form1.Height * 0.7
    If h <= 1000 Then h = 1000
    
    wbRuler.Left = 1
    wbRuler.Width = w
    
    wbMain.Width = w
    wbMain.Left = 1
    
    wbMain.Height = h
    
    txtCN.Top = wbMain.Height + wbMain.Top + 100
    txtCN.Height = Form1.Height * 0.2
    
    FrameTool.Top = txtCN.Top
    FrameTool.Left = Form1.Width - FrameTool.Width - 200
    
    txtCN.Width = FrameTool.Left - txtCN.Left - 500

End Sub
Private Sub Form_Load()
    Dim c As Control
    Me.BackColor = Form2.Grid1.BackColor
    For Each c In Me.Controls
        If TypeName(c) <> "WebBrowser" And TypeName(c) <> "SynMemoX" Then
            c.BackColor = Me.BackColor
        End If
    Next
    
    wbMain.Navigate2 "about:blank"
    DoEvents
    
End Sub

Public Sub info(s)
    txtInfo.Text = txtInfo.Text & s & vbCrLf
    txtInfo.SelStart = Len(txtInfo.Text)
End Sub

Private Sub txtCN_Change()
    g_IsDirty = True
End Sub
Private Sub txtCN_KeyUp(KeyCode As Integer, Shift As Integer)

    If Shift = vbCtrlMask And (KeyCode = 38 Or KeyCode = 40) Then 'up/down
    
        Dim doc As IHTMLDocument2
        Dim ObjID As IHTMLInputElement
        Set doc = wbMain.Document
        Set ObjID = doc.All("NEXT_ID")
        
        If ObjID Is Nothing Then
            MsgBox "当前template文件是旧版的，不支持NEXT_ID，请更换 js.html"
            Exit Sub
        End If
    
        If KeyCode = 40 Then
            ObjID.Value = "+"
        Else
            ObjID.Value = "-"
        End If

        Do
            If ObjID.Value = "" Then Exit Do
            DoEvents
        Loop
        
        OnHTMLClick
    End If
End Sub

Public Sub OnHTMLClick()

    txtCN.Enabled = True
    
    If g_IsDirty = True Then
        cmdSaveCN_Click
    End If
   
    Dim doc As IHTMLDocument2
    Dim ObjID As IHTMLInputElement
    Set doc = wbMain.Document
    Set ObjID = doc.All("SELECT_ID")
    
    
    Dim sID As String
    sID = ObjID.Value
    
    g_CurSentence = Val(sID)
    txtCN.Text = g_Script.CnText(g_CurSentence)
    txtNo.Text = g_CurSentence
    cmdCopyJP.Enabled = True
    
    If txtCN.Text = "" Then
    '如果是未翻译文本
        If Left(g_Script.JpText(g_CurSentence), 1) = "〓" Then
        '如果是重复文本
            Dim Repeat_Script As SCRIPTINFO
            Dim Repeat_Sentence As Long
            Dim tempstr As String
            Dim str() As String
            
            tempstr = Left(g_Script.JpText(g_CurSentence), 40)
            tempstr = Mid(tempstr, 2)
            str = Split(tempstr, "-")
            
            Repeat_Script = Script_GetInfo(str(0))
            Repeat_Sentence = Val(str(1))
            txtCN.Text = Repeat_Script.CnText(Repeat_Sentence)
            If txtCN.Text = "" Then
            '如果已翻译源文本实际上没翻译,不更新
                g_IsDirty = False
            Else
                g_IsDirty = True
            End If
        Else
        '非重复文本则显示日文文本
            txtCN.Text = g_Script.JpText(g_CurSentence)
            g_IsDirty = False
        End If
    Else
        g_IsDirty = False
    End If

    
End Sub

Private Sub txtCN_OnChange()
    g_IsDirty = True
End Sub


Private Sub txtCN_OnKeyUp(KeyCode As Integer, ByVal Shift As Long)
    If Shift >= 4 And (KeyCode = 38 Or KeyCode = 40) Then 'up/down
        Dim doc As IHTMLDocument2
        Dim ObjID As IHTMLInputElement
        Set doc = wbMain.Document
        Set ObjID = doc.All("NEXT_ID")
        
        If ObjID Is Nothing Then
            MsgBox "当前template文件是旧版的，不支持NEXT_ID，请更换 js.html"
            Exit Sub
        End If
    
        If KeyCode = 40 Then
            ObjID.Value = "+"
        Else
            ObjID.Value = "-"
        End If

        Do
            If ObjID.Value = "" Then Exit Do
            DoEvents
        Loop
        
        OnHTMLClick
    End If
End Sub

Private Sub txtFontSize_Change()

    If Val(txtFontSize) > 8 Then
        txtCN.FontSize = Val(txtFontSize)
    End If
End Sub

Private Sub wbMain_DocumentComplete(ByVal pDisp As Object, URL As Variant)
    
    
    Dim evtHTMLSel As VBOnEvent
    Set evtHTMLSel = New VBOnEvent
    evtHTMLSel.Set_Destination Me, "OnHTMLClick"
    
    Dim doc As IHTMLDocument2
    
    On Error GoTo err_doc
    Set doc = wbMain.Document
    On Error GoTo 0
    If TypeName(doc) = "HTMLDocument" Then doc.onclick = evtHTMLSel
    
err_doc:

End Sub

