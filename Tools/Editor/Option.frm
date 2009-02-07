VERSION 5.00
Begin VB.Form Form5 
   Caption         =   "Option"
   ClientHeight    =   3960
   ClientLeft      =   60
   ClientTop       =   345
   ClientWidth     =   4320
   Icon            =   "Option.frx":0000
   LinkTopic       =   "Form5"
   MaxButton       =   0   'False
   ScaleHeight     =   3960
   ScaleWidth      =   4320
   StartUpPosition =   1  '所有者中心
   Begin VB.ComboBox cboTextFormat 
      Height          =   315
      Left            =   480
      TabIndex        =   2
      Top             =   360
      Width           =   3495
   End
   Begin VB.CheckBox chkHideRepeat 
      Caption         =   "启用"
      Height          =   375
      Left            =   480
      TabIndex        =   1
      Top             =   1200
      Value           =   1  'Checked
      Width           =   3135
   End
   Begin VB.CommandButton cmdOK 
      Caption         =   "OK"
      Default         =   -1  'True
      Height          =   375
      Left            =   3000
      TabIndex        =   0
      Top             =   3480
      Width           =   1215
   End
   Begin VB.Frame Frame1 
      Caption         =   "文本格式"
      Height          =   735
      Left            =   240
      TabIndex        =   3
      Top             =   120
      Width           =   3975
   End
   Begin VB.Frame Frame2 
      Caption         =   "翻译界面各列宽度(单位：象素)"
      Height          =   1575
      Left            =   240
      TabIndex        =   4
      Top             =   1800
      Width           =   3975
      Begin VB.TextBox Text4 
         Height          =   375
         Left            =   2640
         TabIndex        =   12
         Text            =   "299"
         Top             =   960
         Width           =   495
      End
      Begin VB.TextBox Text3 
         Height          =   375
         Left            =   2640
         TabIndex        =   11
         Text            =   "299"
         Top             =   480
         Width           =   495
      End
      Begin VB.TextBox Text2 
         Height          =   375
         Left            =   840
         TabIndex        =   10
         Text            =   "299"
         Top             =   960
         Width           =   495
      End
      Begin VB.TextBox Text1 
         Height          =   375
         Left            =   840
         TabIndex        =   9
         Text            =   "29"
         Top             =   480
         Width           =   495
      End
      Begin VB.Label Label4 
         AutoSize        =   -1  'True
         Caption         =   "ID"
         Height          =   195
         Left            =   600
         TabIndex        =   8
         Top             =   600
         Width           =   165
      End
      Begin VB.Label Label3 
         AutoSize        =   -1  'True
         Caption         =   "中"
         Height          =   195
         Left            =   2400
         TabIndex        =   7
         Top             =   1080
         Width           =   180
      End
      Begin VB.Label Label2 
         AutoSize        =   -1  'True
         Caption         =   "英"
         Height          =   195
         Left            =   2400
         TabIndex        =   6
         Top             =   600
         Width           =   180
      End
      Begin VB.Label Label1 
         AutoSize        =   -1  'True
         Caption         =   "日"
         Height          =   195
         Left            =   600
         TabIndex        =   5
         Top             =   1080
         Width           =   180
      End
   End
   Begin VB.Frame Frame3 
      Caption         =   "隐藏以〓开头的重复文本"
      Height          =   735
      Left            =   240
      TabIndex        =   13
      Top             =   960
      Width           =   3975
   End
End
Attribute VB_Name = "Form5"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False

Private Sub cmdOK_Click()
    
    If cboTextFormat.ListIndex = 0 Then
        g_TextFormat = 1
    Else
        g_TextFormat = 2
    End If
    
    If chkHideRepeat.Value = 1 Then
        g_hide_repeat = True
    Else
        g_hide_repeat = False
    End If
    
    
    g_len_id = Val(Text1.Text)
    g_len_j = Val(Text2.Text)
    g_len_u = Val(Text3.Text)
    g_len_c = Val(Text4.Text)
    
    Unload Me

End Sub

Private Sub Form_Load()

    Dim c As Control
    Me.BackColor = Form2.Grid1.BackColor
    For Each c In Me.Controls
        c.BackColor = Me.BackColor
    Next
    
    
    cboTextFormat.AddItem "格式1 - Agemo用"
    cboTextFormat.AddItem "格式2 - 地址,长度,文字"
    If g_TextFormat = 1 Then
        cboTextFormat.ListIndex = 0
    Else
        cboTextFormat.ListIndex = 1
    End If
    
    If g_hide_repeat Then
        chkHideRepeat.Value = 1
    Else
        chkHideRepeat.Value = 0
    End If
    
    Text1.Text = g_len_id
    Text2.Text = g_len_j
    Text3.Text = g_len_u
    Text4.Text = g_len_c

End Sub

