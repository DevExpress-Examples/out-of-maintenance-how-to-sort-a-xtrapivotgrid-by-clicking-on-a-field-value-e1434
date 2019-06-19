Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Text
Imports System.Windows.Forms
Imports DevExpress.XtraPivotGrid

Namespace Q205054
	Partial Public Class Form1
		Inherits Form

		Public Sub New()
			InitializeComponent()
		End Sub

		Private Sub Form1_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
			' TODO: This line of code loads data into the 'nwindDataSet.ProductReports' table. You can move, or remove it, as needed.
			Me.productReportsTableAdapter.Fill(Me.nwindDataSet.ProductReports)

		End Sub

		Private Sub pivotGridControl1_MouseClick(ByVal sender As Object, ByVal e As MouseEventArgs) Handles pivotGridControl1.MouseClick
			Dim hInfo As PivotGridHitInfo = pivotGridControl1.CalcHitInfo(e.Location)
			If hInfo.HitTest = PivotGridHitTest.Value Then
				HandleValueMouseClick(hInfo.ValueInfo)
			End If
		End Sub

		Private Sub HandleValueMouseClick(ByVal e As PivotFieldValueEventArgs)
			Dim higherFields() As PivotGridField = e.GetHigherLevelFields()
			Dim higherValues(higherFields.Length - 1) As Object
			For i As Integer = 0 To higherFields.Length - 1
				higherValues(i) = e.GetHigherLevelFieldValue(higherFields(i))
			Next i

			pivotGridControl1.BeginUpdate()
			Dim otherArea As PivotArea = GetOtherArea(e)
			Dim otherFields As List(Of PivotGridField) = pivotGridControl1.GetFieldsByArea(otherArea)
			For i As Integer = 0 To otherFields.Count - 1
				otherFields(i).SortBySummaryInfo.Field = e.DataField
				otherFields(i).SortBySummaryInfo.Conditions.Clear()
				For j As Integer = 0 To higherFields.Length - 1
					otherFields(i).SortBySummaryInfo.Conditions.Add(New PivotGridFieldSortCondition(higherFields(j), higherValues(j)))
				Next j
				If e.Field IsNot Nothing AndAlso e.Field.Area <> PivotArea.DataArea Then
					otherFields(i).SortBySummaryInfo.Conditions.Add(New PivotGridFieldSortCondition(e.Field, e.Value))
				End If
			Next i
			pivotGridControl1.EndUpdate()
		End Sub

		Private Function GetOtherArea(ByVal e As PivotFieldValueEventArgs) As PivotArea
			Return If(e.IsColumn, PivotArea.RowArea, PivotArea.ColumnArea)
		End Function
	End Class
End Namespace