using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraPivotGrid;

namespace Q205054 {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e) {
            // TODO: This line of code loads data into the 'nwindDataSet.ProductReports' table. You can move, or remove it, as needed.
            this.productReportsTableAdapter.Fill(this.nwindDataSet.ProductReports);

        }

        private void pivotGridControl1_MouseClick(object sender, MouseEventArgs e) {
            PivotGridHitInfo hInfo = pivotGridControl1.CalcHitInfo(e.Location);
            if(hInfo.HitTest == PivotGridHitTest.Value)
                HandleValueMouseClick(hInfo.ValueInfo);
        }

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

        private PivotArea GetOtherArea(PivotFieldValueEventArgs e) {
            return e.FieldCellViewInfo.Item.IsColumn ? PivotArea.RowArea : PivotArea.ColumnArea;
        }
    }
}