VERSION 5.00
Begin VB.Form Form3 
   Caption         =   "Memo"
   ClientHeight    =   2985
   ClientLeft      =   60
   ClientTop       =   450
   ClientWidth     =   5325
   BeginProperty Font 
      Name            =   "ËÎÌå"
      Size            =   9
      Charset         =   134
      Weight          =   400
      Underline       =   0   'False
      Italic          =   0   'False
      Strikethrough   =   0   'False
   EndProperty
   Icon            =   "Memo.frx":0000
   LinkTopic       =   "Form3"
   MaxButton       =   0   'False
   ScaleHeight     =   2985
   ScaleWidth      =   5325
   StartUpPosition =   2  'CenterScreen
   Begin VB.CommandButton cmdOK 
      Caption         =   "OK"
      Default         =   -1  'True
      Height          =   375
      Left            =   3840
      TabIndex        =   4
      Top             =   2520
      Width           =   1215
   End
   Begin VB.Frame Frame2 
      Caption         =   "±¸×¢"
      Height          =   870
      Left            =   255
      TabIndex        =   2
      Top             =   1395
      Width           =   4860
      Begin VB.TextBox txtMemo 
         Height          =   435
         Left            =   360
         TabIndex        =   3
         Top             =   270
         Width           =   4110
      End
   End
   Begin VB.Frame Frame1 
      Caption         =   "ÒëÕß"
      Height          =   825
      Left            =   240
      TabIndex        =   0
      Top             =   240
      Width           =   4905
      Begin VB.TextBox txtUser 
         Height          =   435
         Left            =   360
         TabIndex        =   1
         Top             =   240
         Width           =   4125
      End
   End
End
Attribute VB_Name = "Form3"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit

Public CurrentUser As String
Public CurrentMemo As String
Public CurrentID As String
Public CurrentGridNo As Long
Public IsCancel As Boolean

Public Sub ApplyData(nNo&, sID$, strUser$, strMemo$)

    IsCancel = True
    CurrentGridNo = nNo
    txtUser.Text = strUser
    txtMemo.Text = strMemo
    CurrentID = sID
    Me.Show 1, Form2
End Sub

Private Sub cmdOK_Click()
    CurrentUser = txtUser.Text
    CurrentMemo = txtMemo.Text
    
    IsCancel = False
    Me.Hide
End Sub

Private Sub Form_Load()
    Dim c As Control
    Me.BackColor = Form2.Grid1.BackColor
    For Each c In Me.Controls
        c.BackColor = Me.BackColor
    Next
   
End Sub
