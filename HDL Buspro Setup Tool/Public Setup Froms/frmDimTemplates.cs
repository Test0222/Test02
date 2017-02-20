using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;


namespace HDL_Buspro_Setup_Tool
{
    public partial class frmDimTemplates : Form
    {
        private Object MyActiveObject = null;
        private String DeviceName = "";
        private Byte SubNetID;
        private Byte DeviceID;
        private int mywdDevicerType;

        private Int16[] arrCurrentCurve;

        private UVCMD.ControlTargets TempCMD = null; //保存时更新到buffer
        public List<UVCMD.ControlTargets> TempCMDGroup = null;

        public frmDimTemplates()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;

            string strDevName = DeviceName.Split('\\')[0].ToString();
        }

        private void frmUpgrade_Load(object sender, EventArgs e)
        {
            InitialFormCtrlsTextOrItems();
        }

        void InitialFormCtrlsTextOrItems()
        {
            DimTemplates.ReadAllGroupCommandsFrmDatabase();
            DimTemplates.ShowGroupCommandsToTreetview(tvTemplates);
        }

        private void frmUpgrade_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                DimTemplates.SaveAllGroupCommandsFrmDatabase();
                this.Dispose();
            }
            catch
            {
            }
        }

        private void frmCmdSetup_Shown(object sender, EventArgs e)
        {
        }

        private void btnSure_Click(object sender, EventArgs e)
        {
             if (tbTemplates.Text == null || tbTemplates.Text == "") return;
             if (arrCurrentCurve == null || arrCurrentCurve.Length ==0) return;
             if (tvTemplates.Nodes == null || tvTemplates.Nodes.Count == 0) return;
             TreeNode oNode = tvTemplates.SelectedNode;

             if (oNode == null) return;
             if (oNode.Level == 1) oNode = oNode.Parent;
             if (CsConst.myDimTemplates == null || CsConst.myDimTemplates.Count == 0) return;

             Cursor.Current = Cursors.WaitCursor;
             try
             {
                 DimTemplate oDimCurve = new DimTemplate();
                 oDimCurve.iID = DimTemplates.GetNewIDForGroup(oNode.Text);
                 oDimCurve.sName = tbTemplates.Text.ToString();

                 oDimCurve.arrChannelLevel = arrCurrentCurve;

                 if (CsConst.myDimTemplates[oNode.Index].arrDimProfiles == null)
                     CsConst.myDimTemplates[oNode.Index].arrDimProfiles = new List<DimTemplate>();

                 CsConst.myDimTemplates[oNode.Index].arrDimProfiles.Add(oDimCurve);
                 oNode.Nodes.Add(tbTemplates.Text);
             }
             catch
             { }
             Cursor.Current = Cursors.Default;
        }

        private void txtFrm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar) || e.KeyChar == 8)
                e.Handled = false;
            else
                e.Handled = true;
        }

        private void btnUpdateA_Click(object sender, EventArgs e)
        {
            if (tbTemplates.Text == null || tbTemplates.Text == "") return;
            if (arrCurrentCurve == null || arrCurrentCurve.Length == 0) return;
            if (tvTemplates.Nodes == null || tvTemplates.Nodes.Count == 0) return;
            TreeNode oNode = tvTemplates.SelectedNode;

            if (oNode == null) return;
            if (oNode.Level == 0) return;
            if (CsConst.myDimTemplates == null || CsConst.myDimTemplates.Count == 0) return;
            if (arrCurrentCurve == null) return;
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                int iGroupId = oNode.Parent.Index;
                int TemplateID = oNode.Index;

                CsConst.myDimTemplates[iGroupId].arrDimProfiles[TemplateID].sName = tbTemplates.Text.ToString();

                CsConst.myDimTemplates[iGroupId].arrDimProfiles[TemplateID].arrChannelLevel = arrCurrentCurve;

                oNode.Text  = tbTemplates.Text;
            }
            catch
            { }
            Cursor.Current = Cursors.Default;
        }

        private void btnSavetemplate_Click(object sender, EventArgs e)
        {
            try
            {
                ControlTemplates oTmp = ControlTemplates.AddNewTemplateToPublicGroup(tbTemplates.Text, TempCMDGroup);

                if (oTmp != null)
                {
                    ListViewItem oLv = new ListViewItem();
                   // oLv.Text = oTmp.ID.ToString();
                   // oLv.SubItems.Add(oTmp.Name);
                    //tvTemplates.Nodes.Add(oLv);
                }
            }
            catch
            { 
            }
        }

        private void tbDelTemplate_Click(object sender, EventArgs e)
        {
            if (tvTemplates.Nodes == null || tvTemplates.Nodes.Count == 0) return;
            if (tvTemplates.SelectedNode == null) return;
            TreeNode oNode = tvTemplates.SelectedNode;

            if (CsConst.myTemplates == null || CsConst.myTemplates.Count == 0) return;
            try
            {
                if (oNode.Level == 0)
                {
                    CsConst.myDimTemplates.RemoveAt(oNode.Index);
                }
                else if (oNode.Level == 1)
                {
                    CsConst.myDimTemplates[oNode.Parent.Index].arrDimProfiles.RemoveAt(oNode.Index);
                }
                tvTemplates.Nodes.Remove(oNode);
            }
            catch { }

        }

        private void tvTemplates_MouseDown(object sender, MouseEventArgs e)
        {
            if (tvTemplates.Nodes == null) return;
            TreeNode oNode = tvTemplates.GetNodeAt(e.Location);
            if (oNode == null) return;
            if (oNode.Level == 0) return;
            try
            {
                int iGroupId = oNode.Parent.Index;
                int TemplateID = oNode.Index;
                tbTemplates.Text = oNode.Text;

                if (CsConst.myTemplates == null || CsConst.myTemplates.Count == 0) return;

                arrCurrentCurve = CsConst.myDimTemplates[iGroupId].arrDimProfiles[TemplateID].arrChannelLevel; 
                DisplayCurveAccordinglyBuffer();
                gridValueTable.Rows.Clear();
                for (Byte bIndex = 0; bIndex <= 100; bIndex++)
                {
                    Object[] obj = new Object[] { bIndex,arrCurrentCurve[bIndex]};
                    gridValueTable.Rows.Add(obj);
                }
            }
            catch
            { }
        }

        private void Newtemplates_Click(object sender, EventArgs e)
        {
            try
            {
                Boolean isOpen = HDLSysPF.IsAlreadyOpenedForm("frmAdd");

                if (isOpen == false)
                {
                    frmAdd frmNew = new frmAdd(4, CsConst.MyUnnamed, CsConst.MyUnnamed);
                    frmNew.ShowDialog();

                    if (frmNew.DialogResult == DialogResult.OK)
                    {
                        if (CsConst.MyTmpName == null || CsConst.MyTmpName.Count == 0) return;

                        if (CsConst.MyTmpName != null && CsConst.MyTmpName.Count != 0)
                        {
                            tvTemplates.Nodes.Add(CsConst.MyTmpName[0]);

                            if (CsConst.myDimTemplates == null) CsConst.myDimTemplates = new List<DimTemplates>();
                            DimTemplates oTmp = new DimTemplates();
                            oTmp.sProjectName = CsConst.MyTmpName[0];
                            oTmp.arrDimProfiles = new List<DimTemplate>();
                            CsConst.myDimTemplates.Add(oTmp);
                        }
                    }
                }
            }
            catch
            { }
        }

        private void cboFunction_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboFunction.SelectedIndex == -1) return;
            arrCurrentCurve = new Int16[101];
            
            gridValueTable.Rows.Clear();
            try
            {
                for (Byte bIndex = 0; bIndex <= 100; bIndex++)
                {
                    switch (cboFunction.SelectedIndex)
                    {
                        case 0: arrCurrentCurve[bIndex] = bIndex; break;
                        case 1: arrCurrentCurve[bIndex] = Convert.ToInt16(Math.Round(bIndex * 1.2).ToString()); break;
                        case 2: arrCurrentCurve[bIndex] = Convert.ToInt16(Math.Round(bIndex * 1.5).ToString()); break;
                        case 3: arrCurrentCurve[bIndex] = Convert.ToInt16(Math.Round(bIndex * 1.7).ToString()); break;
                        case 4: arrCurrentCurve[bIndex] = Convert.ToInt16(Math.Round(bIndex * 2.0).ToString()); break;
                        case 5: arrCurrentCurve[bIndex] = Convert.ToInt16(Math.Round(bIndex / 1.2).ToString()); break;
                        case 6: arrCurrentCurve[bIndex] = Convert.ToInt16(Math.Round(bIndex / 1.5).ToString()); break;
                        case 7: arrCurrentCurve[bIndex] = Convert.ToInt16(Math.Round(bIndex / 1.7).ToString()); break;
                        case 8: arrCurrentCurve[bIndex] = Convert.ToInt16(Math.Round(bIndex / 2.0).ToString()); break;
                        case 9: arrCurrentCurve[bIndex] = Convert.ToInt16(Math.Round(Math.Pow(bIndex, 1.2)).ToString()); break;
                        case 10: arrCurrentCurve[bIndex] = Convert.ToInt16(Math.Round(Math.Pow(bIndex, 1.5)).ToString()); break;
                        case 11: arrCurrentCurve[bIndex] = Convert.ToInt16(Math.Round(Math.Pow(bIndex, 1.7)).ToString()); break;
                        case 12: arrCurrentCurve[bIndex] = Convert.ToInt16(Math.Round(Math.Pow(bIndex, 2.0)).ToString()); break;
                        case 13: arrCurrentCurve[bIndex] = Convert.ToInt16(Math.Round(Math.Pow(bIndex, 1 / 1.2)).ToString()); break;
                        case 14: arrCurrentCurve[bIndex] = Convert.ToInt16(Math.Round(Math.Pow(bIndex, 1 / 1.5)).ToString()); break;
                        case 15: arrCurrentCurve[bIndex] = Convert.ToInt16(Math.Round(Math.Pow(bIndex, 1 / 1.7)).ToString()); break;
                        case 16: arrCurrentCurve[bIndex] = Convert.ToInt16(Math.Round(Math.Pow(bIndex, 1 / 2.0)).ToString()); break;
                    }
                    if (arrCurrentCurve[bIndex] > 255) arrCurrentCurve[bIndex] = 255;
                    Object[] obj = new Object[] { bIndex,arrCurrentCurve[bIndex]};
                    gridValueTable.Rows.Add(obj);
                }
                DisplayCurveAccordinglyBuffer();
            }
            catch
            { }
        }

        private void gridValueTable_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex ==-1 || e.RowIndex ==-1) return;
            if (gridValueTable.Rows == null || gridValueTable.RowCount == 0) return;
            if (arrCurrentCurve == null || arrCurrentCurve.Length ==0) arrCurrentCurve = new short[101];

            try
            {
                String sTmpValue = gridValueTable[e.ColumnIndex,e.RowIndex].Value.ToString();
                sTmpValue = HDLPF.IsNumStringMode(sTmpValue, 0, 255);
                arrCurrentCurve[e.RowIndex] = Convert.ToInt16(sTmpValue);
                DisplayCurveAccordinglyBuffer();
            }
            catch
            { }
        }


        void DisplayCurveAccordinglyBuffer()
        {
            if (arrCurrentCurve == null || arrCurrentCurve.Length == 0) return;
            try
            {
                chart1.Series.Clear();
                System.Windows.Forms.DataVisualization.Charting.Series oSeries = new System.Windows.Forms.DataVisualization.Charting.Series();
                oSeries.Name = "Dimming Curve";
                oSeries.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;

                for (Byte bIndex = 0; bIndex < arrCurrentCurve.Length; bIndex++)
                {
                    oSeries.Points.Add(arrCurrentCurve[bIndex]);
                }
                chart1.Series.Add(oSeries);
                chart1.ChartAreas[0].AxisY.Minimum = 0;
                chart1.ChartAreas[0].AxisY.Maximum = 255;
                chart1.ChartAreas[0].AxisY.MinorTickMark.Interval = 1;
            }
            catch
            { }
        }
    }
}
