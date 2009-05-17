Option Strict Off
Option Explicit On
Friend Class VBOnEvent
	'By Agemo
	
	'See MSDN article: "Handling HTML Events in Visual Basic Applications"
	
	
	
	Dim oObject As Object
	Dim sMethod As String
	Dim bInstantiated As Boolean
	
	
	'UPGRADE_NOTE: Class_Initialize was upgraded to Class_Initialize_Renamed. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"'
	Private Sub Class_Initialize_Renamed()
		bInstantiated = False
	End Sub
	Public Sub New()
		MyBase.New()
		Class_Initialize_Renamed()
	End Sub
	
	Public Sub Set_Destination(ByRef oInObject As Object, ByRef sInMethod As String)
		
		oObject = oInObject
		sMethod = sInMethod
		bInstantiated = True
		
	End Sub
	
	
    Public Function My_Default_Method(ByVal pEvtObj As mshtml.IHTMLEventObj) As Boolean
        If bInstantiated Then
            CallByName(oObject, sMethod, CallType.Method)
        End If
        Return True
    End Function
End Class