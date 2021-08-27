<!-- default badges list -->
[![](https://img.shields.io/badge/Open_in_DevExpress_Support_Center-FF7200?style=flat-square&logo=DevExpress&logoColor=white)](https://supportcenter.devexpress.com/ticket/details/E1434)
[![](https://img.shields.io/badge/📖_How_to_use_DevExpress_Examples-e9f6fc?style=flat-square)](https://docs.devexpress.com/GeneralInformation/403183)
<!-- default badges end -->
<!-- default file list -->
*Files to look at*:

* [Form1.cs](./CS/Form1.cs) (VB: [Form1.vb](./VB/Form1.vb))
* [Program.cs](./CS/Program.cs) (VB: [Program.vb](./VB/Program.vb))
<!-- default file list end -->
# How to sort a XtraPivotGrid by clicking on a field value


<p>The PivotGrid allows sorting by any column. To do this, right click any field value and select an appropriate field to sort. Sometimes it is necessary to sort by any column by simply clicking on it. This example shows how to implement this behavior. Handle the MouseClick event and call the CalcHitInfo method to determine whether a field value was clicked:</p>


```cs
PivotGridHitInfo hInfo = pivotGridControl1.CalcHitInfo(e.Location);<br>
if(hInfo.HitTest == PivotGridHitTest.Value)<br>
   HandleValueMouseClick(hInfo.ValueInfo);<br>

```


<p>The HandleValueMouseClick method should be implemented as follows:</p>


```cs
private void HandleValueMouseClick(PivotFieldValueEventArgs e) {
   pivotGridControl1.BeginUpdate();
   PivotArea otherArea = GetOtherArea(e);
   List<PivotGridField> otherFields = pivotGridControl1.GetFieldsByArea(otherArea);
   for(int i = 0; i < otherFields.Count; i++) {
       otherFields[i].SortBySummaryInfo.Field = e.FieldCellViewInfo.Item.DataField;
       otherFields[i].SortBySummaryInfo.Conditions.Clear();
       if(e.Field != null && e.Field.Area != PivotArea.DataArea)
           otherFields[i].SortBySummaryInfo.Conditions.Add(new PivotGridFieldSortCondition(e.Field, e.Value));
   }
   pivotGridControl1.EndUpdate();
}

```


<p>Here we determine the cross-area and fill the <a href="https://documentation.devexpress.com/#CoreLibraries/DevExpressXtraPivotGridPivotGridFieldBase_SortBySummaryInfotopic">SortBySummaryInfo</a> structure for cross-area fields. Condition e.Field != null is necessary to correctly handle Grand Totals.</p>
<p>If there are more than one field in the clicked area, we should create a separate SortBySummaryInfo condition for each field. To do this, we should modify the HandleValueMouseClick click method as follows:</p>


```cs
private void HandleValueMouseClick(PivotFieldValueEventArgs e) {
   PivotGridField[] higherFields = e.GetHigherLevelFields();
   object[] higherValues = new object[higherFields.Length];
   for(int i = 0; i < higherFields.Length; i++) 
       higherValues[i] = e.GetHigherLevelFieldValue(higherFields[i]);
   pivotGridControl1.BeginUpdate();
   PivotArea otherArea = GetOtherArea(e);
   List<PivotGridField> otherFields = pivotGridControl1.GetFieldsByArea(otherArea);
   for(int i = 0; i < otherFields.Count; i++) {
       otherFields[i].SortBySummaryInfo.Field = e.FieldCellViewInfo.Item.DataField;
       otherFields[i].SortBySummaryInfo.Conditions.Clear();
       for(int j = 0; j < higherFields.Length; j++) {
           otherFields[i].SortBySummaryInfo.Conditions.Add(new PivotGridFieldSortCondition(higherFields[j], higherValues[j]));
       }
       if(e.Field != null && e.Field.Area != PivotArea.DataArea)
           otherFields[i].SortBySummaryInfo.Conditions.Add(new PivotGridFieldSortCondition(e.Field, e.Value));
   }
   pivotGridControl1.EndUpdate();
}
```



<br/>


