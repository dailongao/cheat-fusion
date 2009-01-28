VERSION 5.00
Object = "{EAB22AC0-30C1-11CF-A7EB-0000C05BAE0B}#1.1#0"; "shdocvw.dll"
Begin VB.Form Form4 
   Caption         =   "Search & Replace"
   ClientHeight    =   9420
   ClientLeft      =   60
   ClientTop       =   345
   ClientWidth     =   10875
   Icon            =   "Search.frx":0000
   LinkTopic       =   "Form4"
   ScaleHeight     =   9420
   ScaleWidth      =   10875
   StartUpPosition =   2  'CenterScreen
   WindowState     =   2  'Maximized
   Begin VB.CommandButton mnuExport 
      Caption         =   "人名拆离"
      BeginProperty Font 
         Name            =   "宋体"
         Size            =   9
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   375
      Left            =   4440
      TabIndex        =   11
      Top             =   9000
      Visible         =   0   'False
      Width           =   1695
   End
   Begin VB.CommandButton cmdSearch1Line 
      Caption         =   "查找第一行(人名)"
      BeginProperty Font 
         Name            =   "宋体"
         Size            =   9
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   375
      Left            =   4425
      TabIndex        =   10
      Top             =   8400
      Visible         =   0   'False
      Width           =   1695
   End
   Begin VB.TextBox txtSearchTo 
      BeginProperty DataFormat 
         Type            =   1
         Format          =   "0"
         HaveTrueFalseNull=   0
         FirstDayOfWeek  =   0
         FirstWeekOfYear =   0
         LCID            =   2052
         SubFormatType   =   1
      EndProperty
      Height          =   375
      Left            =   3840
      TabIndex        =   8
      Text            =   "Text1"
      Top             =   7800
      Width           =   855
   End
   Begin VB.TextBox txtSearchFrom 
      BeginProperty DataFormat 
         Type            =   1
         Format          =   "0"
         HaveTrueFalseNull=   0
         FirstDayOfWeek  =   0
         FirstWeekOfYear =   0
         LCID            =   2052
         SubFormatType   =   1
      EndProperty
      Height          =   375
      Left            =   2520
      TabIndex        =   7
      Text            =   "Text1"
      Top             =   7800
      Width           =   855
   End
   Begin VB.ComboBox cboLang 
      Height          =   315
      Left            =   240
      Style           =   2  'Dropdown List
      TabIndex        =   6
      Top             =   7800
      Width           =   2175
   End
   Begin VB.TextBox txtInfo 
      BackColor       =   &H8000000F&
      BeginProperty Font 
         Name            =   "Arial"
         Size            =   9
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      ForeColor       =   &H00404040&
      Height          =   1935
      Left            =   6480
      MultiLine       =   -1  'True
      ScrollBars      =   2  'Vertical
      TabIndex        =   5
      Top             =   7320
      Width           =   5985
   End
   Begin VB.CommandButton cmdReplace 
      Caption         =   "执行替换"
      BeginProperty Font 
         Name            =   "宋体"
         Size            =   9
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   375
      Left            =   2880
      TabIndex        =   4
      Top             =   9000
      Width           =   1335
   End
   Begin VB.CommandButton cmdSearch 
      Caption         =   "查找"
      BeginProperty Font 
         Name            =   "宋体"
         Size            =   9
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   375
      Left            =   2880
      TabIndex        =   3
      Top             =   8400
      Width           =   1335
   End
   Begin VB.TextBox txtReplTo 
      BeginProperty Font 
         Name            =   "宋体"
         Size            =   9
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   375
      Left            =   240
      TabIndex        =   2
      Top             =   9000
      Width           =   2415
   End
   Begin VB.TextBox txtReplFrom 
      BeginProperty Font 
         Name            =   "宋体"
         Size            =   9
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   375
      Left            =   240
      TabIndex        =   1
      Top             =   8400
      Width           =   2415
   End
   Begin SHDocVwCtl.WebBrowser wbMain 
      Height          =   7320
      Left            =   15
      TabIndex        =   0
      Top             =   -30
      Width           =   12855
      ExtentX         =   22675
      ExtentY         =   12912
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
      Location        =   "http:///"
   End
   Begin VB.Label lblTo 
      AutoSize        =   -1  'True
      Caption         =   "To"
      Height          =   195
      Left            =   3480
      TabIndex        =   9
      Top             =   7800
      Width           =   195
   End
End
Attribute VB_Name = "Form4"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit

Private g_SearchStop As Boolean

Private g_ReplParagraphID() As String
Private g_ReplParagraphNo() As Long
Private g_ReplSentence()  As Long
Private g_ReplEnable()  As Boolean

Private g_ReplHTMLJP() As String
Private g_ReplHTMLCN() As String
Private g_ReplHTMLUS() As String

Private Sub cmdReplace_Click()

    '扫描一遍，获取哪些选择了，那些没有。根据颜色判定
    Dim I As Long
    Dim nSelected As Long
    
    Dim doc As IHTMLDocument2
    Dim ObjID As IHTMLInputElement
    
    Set doc = wbMain.Document
    nSelected = 0
    
    If UBound(g_ReplParagraphID) = 0 Then Exit Sub
    
    ReDim g_ReplEnable(1 To UBound(g_ReplParagraphID))
    For I = 1 To UBound(g_ReplParagraphID)
    
        Set ObjID = doc.All("ID_" & I)
        If ObjID.Value = "Y" Then
            nSelected = nSelected + 1
            g_ReplEnable(I) = True
        Else
            g_ReplEnable(I) = False
        End If
    Next I
    
    Dim sPrompt As String
    
    sPrompt = "总计 " & UBound(g_ReplParagraphID) & vbCrLf
    sPrompt = sPrompt & "选择 " & nSelected & vbCrLf & vbCrLf
    sPrompt = sPrompt & "确认替换? "
    
    If vbOK <> MsgBox(sPrompt, vbOKCancel) Then Exit Sub
    info "正在执行替换请等待..."
    
    Dim nLang As Long
    nLang = cboLang.ListIndex
    
    Dim inf As SCRIPTINFO
    
    Dim txtReplaceFrom As String
    Dim txtReplaceTo As String
    
    txtReplaceFrom = Tool_StripCrLf(txtReplFrom.Text)
    txtReplaceTo = Tool_StripCrLf(txtReplTo.Text)
    
    Dim txt() As String
    Dim filename As String
    
    For I = 1 To UBound(g_ReplParagraphID)
        If g_ReplEnable(I) Then
            inf = Script_GetInfo(g_ReplParagraphID(I))
            
            filename = ""
            Select Case nLang
                Case LANGUAGE_JP
                    filename = g_Dir & "\jp-text\" & g_ReplParagraphID(I) & ".txt"
                    inf.JpText(g_ReplSentence(I)) = Replace(inf.JpText(g_ReplSentence(I)), txtReplaceFrom, txtReplaceTo, , , vbBinaryCompare)
                Case LANGUAGE_CN
                    filename = g_Dir & "\cn-text\" & g_ReplParagraphID(I) & ".txt"
                    inf.CnText(g_ReplSentence(I)) = Replace(inf.CnText(g_ReplSentence(I)), txtReplaceFrom, txtReplaceTo, , , vbBinaryCompare)
                Case LANGUAGE_US
                    filename = g_Dir & "\us-text\" & g_ReplParagraphID(I) & ".txt"
                    inf.UsText(g_ReplSentence(I)) = Replace(inf.UsText(g_ReplSentence(I)), txtReplaceFrom, txtReplaceTo, , , vbBinaryCompare)
            End Select
            
            Script_Save g_ReplParagraphID(I), inf, nLang
        End If
    Next I
    
    info "替换完毕"
End Sub

Private Sub cmdSearch_Click()

    Dim sSearch As String
    Dim sOldCaption As String
    
    sOldCaption = cmdSearch.Caption
    
    sSearch = Trim(txtReplFrom.Text)
    sSearch = Tool_StripCrLf(sSearch)
    
    txtReplFrom.Text = sSearch
    
    If cmdSearch.Caption = "stop" Then
        g_SearchStop = True
        Exit Sub
    End If

    If sSearch = "" Then Exit Sub

    g_SearchStop = False
    cmdSearch.Caption = "stop"

    txtInfo.Text = ""
    info "查找全文包含[" & sSearch & "]中，pls wait ..."
    wbMain.Navigate2 "about:blank"
    
    Dim I&, j&
    Dim inf As SCRIPTINFO
    
    
    ReDim g_ReplParagraphID(0 To 0)
    ReDim g_ReplParagraphNo(0 To 0)
    ReDim g_ReplSentence(0 To 0)
    ReDim g_ReplHTMLJP(0 To 0)
    ReDim g_ReplHTMLCN(0 To 0)
    ReDim g_ReplHTMLUS(0 To 0)
    
    Dim nLang As Long
    nLang = cboLang.ListIndex
    Dim nFrom As Long
    Dim nTo As Long
    
    nFrom = Val(txtSearchFrom.Text)
    If nFrom < 1 Then nFrom = 1
    txtSearchFrom.Text = nFrom
    
    nTo = Val(txtSearchTo.Text)
    If nTo > UBound(g_CacheInfo) Then nTo = UBound(g_CacheInfo)
    txtSearchTo.Text = nTo
    
    
    For I = nFrom To nTo

        Dim sID$
        Dim txtAll$
        Dim txt()  As String
        Dim tmpids() As String
        Dim tmplen() As Long
    
        sID = g_CacheInfo(I).sID
        If nLang = 0 Then
            txtAll = Tool_LoadTextFile(g_Dir & "\jp-text\" & sID & ".txt")
        ElseIf nLang = 1 Then
            txtAll = Tool_LoadTextFile(g_Dir & "\cn-text\" & sID & ".txt")
        ElseIf nLang = 2 Then
            txtAll = Tool_LoadTextFile(g_Dir & "\us-text\" & sID & ".txt")
        Else
            MsgBox "err"
            Exit Sub
        End If
        
        If g_TextFormat = 1 Then
            Script_DecodeText txtAll, txt, tmpids
        Else
            Script_DecodeText_Format2 txtAll, txt, tmpids, tmplen
        End If
        
        For j = 1 To UBound(txt)
            
            Dim sTmp As String
            
            If Right(txt(j), Len("{enter}")) = "{enter}" Then
                info "found {enter} bug: " & I & " - " & sID & " - " & j
            End If
            
            sTmp = Callback_PlainText(txt(j), LANGUAGE_CN)
            
            If InStr(1, sTmp, sSearch, vbBinaryCompare) > 0 Then
            'If sSearch = Left(sText, Len(sSearch)) Then
            
                Dim sText(0 To 2) As String         '3种语言的预览文本，没有加颜色
                inf = Script_GetInfo(sID)
                sText(0) = Callback_PlainText(inf.JpText(j), LANGUAGE_JP)
                sText(0) = Replace(sText(0), vbCrLf, "<br>")
                sText(1) = Callback_PlainText(inf.CnText(j), LANGUAGE_CN)
                sText(1) = Replace(sText(1), vbCrLf, "<br>")
                sText(2) = Callback_PlainText(inf.UsText(j), LANGUAGE_US)
                sText(2) = Replace(sText(2), vbCrLf, "<br>")
                
                'info "#" & I & " - " & sID & " - " & j
                info sID & " - " & j
            
                ReDim Preserve g_ReplParagraphID(0 To UBound(g_ReplParagraphID) + 1)
                ReDim Preserve g_ReplParagraphNo(0 To UBound(g_ReplParagraphID) + 1)
                ReDim Preserve g_ReplSentence(0 To UBound(g_ReplParagraphID) + 1)
                ReDim Preserve g_ReplHTMLJP(0 To UBound(g_ReplParagraphID) + 1)
                ReDim Preserve g_ReplHTMLCN(0 To UBound(g_ReplParagraphID) + 1)
                ReDim Preserve g_ReplHTMLUS(0 To UBound(g_ReplParagraphID) + 1)
                
                g_ReplParagraphID(UBound(g_ReplParagraphID)) = sID
                g_ReplParagraphNo(UBound(g_ReplParagraphID)) = I
                g_ReplSentence(UBound(g_ReplParagraphID)) = j
                
                '加颜色
                sText(nLang) = Replace(sText(nLang), sSearch, "<font color=red>" & sSearch & "</font>")
                
                g_ReplHTMLJP(UBound(g_ReplParagraphID)) = sText(0)
                g_ReplHTMLCN(UBound(g_ReplParagraphID)) = sText(1)
                g_ReplHTMLUS(UBound(g_ReplParagraphID)) = sText(2)
                
                If g_ReplHTMLJP(UBound(g_ReplParagraphID)) = "" Then g_ReplHTMLJP(UBound(g_ReplParagraphID)) = "&nbsp;"
                If g_ReplHTMLCN(UBound(g_ReplParagraphID)) = "" Then g_ReplHTMLCN(UBound(g_ReplParagraphID)) = "&nbsp;"
                If g_ReplHTMLUS(UBound(g_ReplParagraphID)) = "" Then g_ReplHTMLUS(UBound(g_ReplParagraphID)) = "&nbsp;"
                
            End If
            
            
            DoEvents
            
            If g_SearchStop = True Then
                info "stop. last position is " & sID
                GoTo Search_STOP
            End If

        Next j
        
        
        DoEvents
        
    Next I
    
Search_STOP:

    Dim sAll As String
    Dim sHTMLFile As String
    Dim tpl_body As String, tpl_row As String   '读入模板
    Dim tpl_js As String
    Dim tpl_dir As String
    
    tpl_dir = g_Dir & "\template\search"
    
    tpl_body = Tool_LoadTextFile(tpl_dir & "\body.html")
    TemplateSetColSize tpl_body
    
    tpl_row = Tool_LoadTextFile(tpl_dir & "\row.html")
    TemplateSetColSize tpl_row
    
    Dim rows As String
    Dim sItem As String
    Dim sHidden As String
    
    rows = ""
    sHidden = ""
    For I = 1 To UBound(g_ReplParagraphID)
        sItem = Replace(tpl_row, "TEMPLATE_SEARCH_ID", I)
        'sItem = Replace(sItem, "TEMPLATE_PARAGRAPH_NO", g_ReplParagraphNo(I))
        sItem = Replace(sItem, "TEMPLATE_PARAGRAPH_ID", g_ReplParagraphID(I))
        sItem = Replace(sItem, "TEMPLATE_SENTENCE", g_ReplSentence(I))
        
        sItem = Replace(sItem, "TEMPLATE_JP", g_ReplHTMLJP(I))
        sItem = Replace(sItem, "TEMPLATE_CN", g_ReplHTMLCN(I))
        sItem = Replace(sItem, "TEMPLATE_US", g_ReplHTMLUS(I))
        rows = rows & sItem
        
        sHidden = sHidden & "<input type=hidden name=" & """" & "ID_" & I & """" & " value=" & """" & "Y" & """" & ">" & vbCrLf
    Next I

    sAll = Replace(tpl_body, "TEMPLATE_MAX", UBound(g_ReplParagraphID))
    sAll = Replace(sAll, "<!-- TEMPLATE_HIDDEN -->", sHidden)
    sAll = Replace(sAll, "<!-- TEMPLATE_ROWS -->", rows)
    
    
    sAll = sAll & vbCrLf
    sHTMLFile = g_Dir & "\template\temp\preview.html"
    Tool_WriteTextFile sHTMLFile, sAll
    
    wbMain.Navigate2 sHTMLFile
    
    
    cmdSearch.Caption = sOldCaption
    
    info "总计结果 " & UBound(g_ReplParagraphID)
End Sub



Private Sub cmdSearch1Line_Click()
    Dim sSearch As String
    Dim sOldCaption As String
    
    sOldCaption = cmdSearch1Line.Caption
    
    sSearch = Trim(txtReplFrom.Text)
    sSearch = Tool_StripCrLf(sSearch)
    
    txtReplFrom.Text = sSearch
    
    If cmdSearch1Line.Caption = "stop" Then
        g_SearchStop = True
        Exit Sub
    End If

    If sSearch = "" Then Exit Sub

    g_SearchStop = False
    cmdSearch1Line.Caption = "stop"

    txtInfo.Text = ""
    info "查找第一行为[" & sSearch & "] 中，pls wait ..."
    wbMain.Navigate2 "about:blank"
    
    Dim I&, j&
    Dim inf As SCRIPTINFO
    
    
    ReDim g_ReplParagraphID(0 To 0)
    ReDim g_ReplParagraphNo(0 To 0)
    ReDim g_ReplSentence(0 To 0)
    ReDim g_ReplHTMLJP(0 To 0)
    ReDim g_ReplHTMLCN(0 To 0)
    ReDim g_ReplHTMLUS(0 To 0)
    
    Dim nLang As Long
    nLang = cboLang.ListIndex
    Dim nFrom As Long
    Dim nTo As Long
    
    nFrom = Val(txtSearchFrom.Text)
    If nFrom < 1 Then nFrom = 1
    txtSearchFrom.Text = nFrom
    
    nTo = Val(txtSearchTo.Text)
    If nTo > UBound(g_CacheInfo) Then nTo = UBound(g_CacheInfo)
    txtSearchTo.Text = nTo
    
    '********************
    
    sSearch = sSearch & "<br>"
    
    For I = nFrom To nTo

        Dim sID$
        
        sID = g_CacheInfo(I).sID
        inf = Script_GetInfo(sID)
        
        
        For j = 1 To inf.JpCount
            
            Dim sText(0 To 2) As String         '3种语言的预览文本，没有加颜色
            Dim sTmp As String
            
            
            sText(0) = Callback_Preview(inf.JpText(j), LANGUAGE_JP)
            sText(1) = Callback_Preview(inf.CnText(j), LANGUAGE_CN)
            sText(2) = Callback_Preview(inf.UsText(j), LANGUAGE_US)
            
            sText(nLang) = Replace(sText(nLang), "<b>", "")
            sText(nLang) = Replace(sText(nLang), "</b>", "")
            
            If InStr(1, sText(nLang), sSearch, vbBinaryCompare) = 1 Then
            'If sSearch = Left(sText, Len(sSearch)) Then
                'info "# " & I & " - " & sID & " - " & j
                info sID & " - " & j
            
                ReDim Preserve g_ReplParagraphID(0 To UBound(g_ReplParagraphID) + 1)
                ReDim Preserve g_ReplParagraphNo(0 To UBound(g_ReplParagraphID) + 1)
                ReDim Preserve g_ReplSentence(0 To UBound(g_ReplParagraphID) + 1)
                ReDim Preserve g_ReplHTMLJP(0 To UBound(g_ReplParagraphID) + 1)
                ReDim Preserve g_ReplHTMLCN(0 To UBound(g_ReplParagraphID) + 1)
                ReDim Preserve g_ReplHTMLUS(0 To UBound(g_ReplParagraphID) + 1)
                
                g_ReplParagraphID(UBound(g_ReplParagraphID)) = sID
                g_ReplParagraphNo(UBound(g_ReplParagraphID)) = I
                g_ReplSentence(UBound(g_ReplParagraphID)) = j
                
                g_ReplHTMLJP(UBound(g_ReplParagraphID)) = sText(0)
                g_ReplHTMLCN(UBound(g_ReplParagraphID)) = sText(1)
                g_ReplHTMLUS(UBound(g_ReplParagraphID)) = sText(2)
                
                If g_ReplHTMLJP(UBound(g_ReplParagraphID)) = "" Then g_ReplHTMLJP(UBound(g_ReplParagraphID)) = "&nbsp;"
                If g_ReplHTMLCN(UBound(g_ReplParagraphID)) = "" Then g_ReplHTMLCN(UBound(g_ReplParagraphID)) = "&nbsp;"
                If g_ReplHTMLUS(UBound(g_ReplParagraphID)) = "" Then g_ReplHTMLUS(UBound(g_ReplParagraphID)) = "&nbsp;"
                
            End If
            
            
            DoEvents
            
            If g_SearchStop = True Then
                info "stop. last position is " & sID
                GoTo Search_STOP
            End If

        Next j
        
        
        DoEvents
        
    Next I
    
Search_STOP:

    Dim sAll As String
    Dim sHTMLFile As String
    Dim tpl_body As String, tpl_row As String   '读入模板
    Dim tpl_js As String
    Dim tpl_dir As String
    
    tpl_dir = g_Dir & "\template\search"
    
    tpl_body = Tool_LoadTextFile(tpl_dir & "\body.html")
    tpl_row = Tool_LoadTextFile(tpl_dir & "\row.html")
    tpl_js = Tool_LoadTextFile(tpl_dir & "\js.html")
    
    Dim rows As String
    Dim sItem As String
    Dim sHidden As String
    
    rows = ""
    sHidden = ""
    For I = 1 To UBound(g_ReplParagraphID)
        sItem = Replace(tpl_row, "TEMPLATE_SEARCH_ID", I)
        'sItem = Replace(sItem, "TEMPLATE_PARAGRAPH_NO", g_ReplParagraphNo(I))
        sItem = Replace(sItem, "TEMPLATE_PARAGRAPH_ID", g_ReplParagraphID(I))
        sItem = Replace(sItem, "TEMPLATE_SENTENCE", g_ReplSentence(I))
        
        sItem = Replace(sItem, "TEMPLATE_JP", g_ReplHTMLJP(I))
        sItem = Replace(sItem, "TEMPLATE_CN", g_ReplHTMLCN(I))
        sItem = Replace(sItem, "TEMPLATE_US", g_ReplHTMLUS(I))
        rows = rows & sItem
        
        sHidden = sHidden & "<input type=hidden name=" & """" & "ID_" & I & """" & " value=" & """" & "Y" & """" & ">" & vbCrLf
    Next I

    sAll = Replace(tpl_body, "TEMPLATE_MAX", UBound(g_ReplParagraphID))
    sAll = Replace(sAll, "<!-- TEMPLATE_HIDDEN -->", sHidden)
    sAll = Replace(sAll, "<!-- TEMPLATE_ROWS -->", rows)
    
    
    sAll = sAll & vbCrLf & tpl_js
    sHTMLFile = g_Dir & "\template\temp\preview.html"
    Tool_WriteTextFile sHTMLFile, sAll
    
    wbMain.Navigate2 sHTMLFile
    
    
    cmdSearch1Line.Caption = sOldCaption
    
    info "总计结果 " & UBound(g_ReplParagraphID)

End Sub

Private Sub Form_Activate()
    txtReplFrom.SetFocus
End Sub

Private Sub txtReplFrom_KeyDown(KeyCode As Integer, Shift As Integer)
    If KeyCode = 13 Then cmdSearch_Click
End Sub


Private Sub Form_Load()
    
    cboLang.Clear
    cboLang.AddItem "日文"
    cboLang.AddItem "中文"
    cboLang.AddItem "英文"
    cboLang.ListIndex = 1
    
    info "日文输入法乱码的解决"
    info "1. 打开日文输入法，在记事本输入要查找的词"
    info "2. 关闭日文输入法"
    info "3. 把输入的词复制&粘贴到程序的查找框"
    
    info ""
    info "ps: 推荐使用ultra edit等软件的批量查找、替换功能"
    
    Me.BackColor = Form2.Grid1.BackColor
    lblTo.BackColor = Form2.Grid1.BackColor
    txtInfo.BackColor = Form2.Grid1.BackColor
    
    txtSearchFrom.Text = "1"
    txtSearchTo.Text = UBound(g_CacheInfo)
    
    ReDim g_ReplParagraphID(0 To 0)
    wbMain.Navigate2 "about:blank"
    DoEvents
    
End Sub

Private Sub Form_Resize()
    Dim w As Long
    Dim h As Long
    
    w = Form4.Width - 200
    If w <= 1000 Then w = 1000
    
    h = Form4.Height * 0.7
    If h <= 1000 Then h = 1000
    
    wbMain.Width = w
    wbMain.Left = 1
    
    wbMain.Height = h

    cboLang.Top = wbMain.Height + wbMain.Top + 200
    txtSearchFrom.Top = cboLang.Top
    txtSearchTo.Top = cboLang.Top
    lblTo.Top = cboLang.Top
    
    txtReplFrom.Top = cboLang.Height + cboLang.Top + 200
    txtReplTo.Top = txtReplFrom.Height + txtReplFrom.Top + 200
    cmdSearch.Top = txtReplFrom.Top
    cmdSearch1Line.Top = txtReplFrom.Top
    cmdReplace.Top = txtReplTo.Top
    txtInfo.Top = wbMain.Height + wbMain.Top + 200

End Sub

Public Sub info(s)
    txtInfo.Text = txtInfo.Text & s & vbCrLf
    txtInfo.SelStart = Len(txtInfo.Text)
    txtInfo.Refresh
    DoEvents
End Sub

Private Sub mnuExport_Click()

    Dim sSearch As String
    sSearch = Trim(txtReplFrom.Text)
    sSearch = Tool_StripCrLf(sSearch)
    
    txtReplFrom.Text = sSearch
    
    If sSearch = "" Then Exit Sub

    txtInfo.Text = ""
    info "提取人名 [" & sSearch & "] 中，pls wait ..."

    Dim nTotalJPCount As Long
    
    nTotalJPCount = 0

    Dim sDir As String
    Dim strExpFile As String
    sDir = g_Dir & "\export"
    If Dir(sDir, vbDirectory) = "" Then MkDir sDir
    If Dir(sDir & "\jp-text", vbDirectory) = "" Then MkDir sDir & "\jp-text"
    If Dir(sDir & "\cn-text", vbDirectory) = "" Then MkDir sDir & "\cn-text"
    If Dir(sDir & "\us-text", vbDirectory) = "" Then MkDir sDir & "\us-text"
    
    Dim fso As New FileSystemObject
    Dim oFileJP As TextStream
    Dim oFileCN As TextStream
    Dim oFileUS As TextStream
    
    Set oFileJP = fso.OpenTextFile(sDir & "\jp-text\" & sSearch & ".txt", ForWriting, True, TristateFalse)
    Set oFileCN = fso.OpenTextFile(sDir & "\cn-text\" & sSearch & ".txt", ForWriting, True, TristateFalse)
    Set oFileUS = fso.OpenTextFile(sDir & "\us-text\" & sSearch & ".txt", ForWriting, True, TristateFalse)
    
    Dim I&, j&
    Dim inf As SCRIPTINFO
    
    
    ReDim g_ReplParagraphID(0 To 0)
    ReDim g_ReplParagraphNo(0 To 0)
    ReDim g_ReplSentence(0 To 0)
    ReDim g_ReplHTMLJP(0 To 0)
    ReDim g_ReplHTMLCN(0 To 0)
    ReDim g_ReplHTMLUS(0 To 0)
    
    Dim nLang As Long
    nLang = cboLang.ListIndex
    Dim nFrom As Long
    Dim nTo As Long
    
    nFrom = Val(txtSearchFrom.Text)
    If nFrom < 1 Then nFrom = 1
    txtSearchFrom.Text = nFrom
    
    nTo = Val(txtSearchTo.Text)
    If nTo > UBound(g_CacheInfo) Then nTo = UBound(g_CacheInfo)
    txtSearchTo.Text = nTo
    
    '********************
    
    sSearch = sSearch & "<br>"
    
    For I = nFrom To nTo

        Dim sID$
        
        sID = g_CacheInfo(I).sID
        inf = Script_GetInfo(sID)
        
        
        For j = 1 To inf.JpCount
            
            Dim sText(0 To 2) As String         '3种语言的预览文本，没有加颜色
            Dim sTmp As String
            
            
            sText(0) = Callback_Preview(inf.JpText(j), LANGUAGE_JP)
            sText(1) = Callback_Preview(inf.CnText(j), LANGUAGE_CN)
            sText(2) = Callback_Preview(inf.UsText(j), LANGUAGE_US)
            
            sText(nLang) = Replace(sText(nLang), "<b>", "")
            sText(nLang) = Replace(sText(nLang), "</b>", "")
            
            If InStr(1, sText(nLang), sSearch, vbBinaryCompare) = 1 Then
                'info "# " & I & " - " & sID & " - " & j
                
                nTotalJPCount = nTotalJPCount + Tool_GetWords(inf.JpText(j), LANGUAGE_JP)
                
                Dim sID_File As String
                
                sID_File = sID & "-" & j
                
                oFileJP.WriteLine "#### " & sID_File & " ####"
                oFileCN.WriteLine "#### " & sID_File & " ####"
                oFileUS.WriteLine "#### " & sID_File & " ####"
                oFileJP.WriteLine inf.JpText(j)
                oFileCN.WriteLine inf.CnText(j)
                oFileUS.WriteLine inf.UsText(j)
                oFileJP.WriteLine ""
                oFileCN.WriteLine ""
                oFileUS.WriteLine ""
                
            End If
            
            
            DoEvents
        Next j
        
        DoEvents
        
    Next I
    
    oFileCN.Close
    oFileJP.Close
    oFileUS.Close
    
    info "done, total " & nTotalJPCount & " words."
    
End Sub
    


