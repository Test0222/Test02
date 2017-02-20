using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HDL_Buspro_Setup_Tool
{
    public partial class frmHvacRelay : Form
    {
        private HVAC myHVAC = null;
        private String MyDeviceName;
        private Byte SensorType;

        TimeText txtScene = new TimeText(".");


        public frmHvacRelay()
        {
            InitializeComponent();
        }

        public frmHvacRelay(HVAC oTmp, string TmpDeviceName, Byte sensorType)
        {
            InitializeComponent();
            myHVAC = oTmp;
            this.MyDeviceName = TmpDeviceName;
            this.SensorType = sensorType;
        }

        private void frmHvI_Load(object sender, EventArgs e)
        {
            if (CsConst.myOnlines == null) return;
            HDLSysPF.LoadDeviceListsWithDifferentSensorType(cboDevice, SensorType);
            InitialFormCtrlsTextOrItems();
        }

        void InitialFormCtrlsTextOrItems()
        {
        }

        void DisplayRelaySetups()
        {
            if (myHVAC == null) return;
            if (myHVAC.bytSteps == null || myHVAC.bytSteps.Length == 0) return;
            for (int i = 0; i <= 4; i++)
            {
                byte[] ArayTmp = new byte[20];
                Array.Copy(myHVAC.bytSteps, 40 + i * 20, ArayTmp, 0, 20);
                object[] obj = { cm1.Items[ArayTmp[0]], cm1.Items[ArayTmp[2]], cm1.Items[ArayTmp[4]], HDLPF.GetStringFromTime(ArayTmp[1], ".") };
                DgMode.Rows.Add(obj);
            }
            rbD.Checked = (myHVAC.bytSteps[0] == 0);
            rbC.Checked = (myHVAC.bytSteps[0] == 1);

            DgMode.Rows[0].HeaderCell.Value = "When " + CsConst.StatusAC[0];
            DgMode.Rows[1].HeaderCell.Value = "When " + CsConst.StatusAC[1];
            DgMode.Rows[2].HeaderCell.Value = "When " + CsConst.StatusAC[4];
            DgMode.Rows[3].HeaderCell.Value = "When " + CsConst.StatusAC[2];
            DgMode.Rows[4].HeaderCell.Value = "When " + CsConst.StatusAC[3];
        }

        private void DgMode_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (e.ColumnIndex == -1) return;
            if (e.RowIndex == -1) return;
            if (DgMode.RowCount == 0) return;
            if (e.ColumnIndex == 3)
            {
                DgMode.Controls.Add(txtScene);
                txtScene.Text = HDLPF.GetTimeFromString(DgMode[3, e.RowIndex].Value.ToString(), '.');

                Rectangle rect = DgMode.GetCellDisplayRectangle(3, e.RowIndex, true);
                txtScene.Size = rect.Size;
                txtScene.Top = rect.Top;
                txtScene.Left = rect.Left;
                txtScene.Show();
                txtScene.Visible = true;
                txtScene.TextChanged += new EventHandler(txtScene_TextChanged);
            }
            else
            {
                txtScene.Visible = false;
            }
        }

        void txtScene_TextChanged(object sender, EventArgs e)
        {
            if (myHVAC == null) return;
            if (myHVAC.bytSteps == null) return;

            if ((DgMode.CurrentCell.RowIndex == -1) || (DgMode.CurrentCell.ColumnIndex == -1)) return;
            if (txtScene.Visible)
            {
                DgMode[3, DgMode.CurrentCell.RowIndex].Value = HDLPF.GetStringFromTime(int.Parse(txtScene.Text.ToString()), ".");
                // DgMode_CellValueChanged(DgMode, e);
            }
            //throw new NotImplementedException();
        }

        private void rbD_CheckedChanged(object sender, EventArgs e)
        {
            DgMode.Enabled = !(rbD.Checked);
        }

        private void DgMode_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
           
        }

        private void DgMode_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (myHVAC == null) return;
            if (myHVAC.bytSteps == null) return;
            if ((e.RowIndex == -1) || (e.ColumnIndex == -1)) return;
            if (DgMode.SelectedRows.Count == 0) return;

            for (int i = 0; i < DgMode.SelectedRows.Count; i++)
            {
                DgMode.SelectedRows[i].Cells[e.ColumnIndex].Value = DgMode[e.ColumnIndex, e.RowIndex].Value.ToString();
            }
        }

        private void DgMode_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            DgMode.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (myHVAC == null) return;
            if (myHVAC.bytSteps == null) return;
            if (DgMode.RowCount == 0) return;

            string strTmp = null;

            if (rbD.Checked) myHVAC.bytSteps[0] = 0;
            else if (rbC.Checked) myHVAC.bytSteps[0] = 1;

            for (int i = 0; i < DgMode.RowCount; i++)
            {
                for (int j = 0; j < DgMode.ColumnCount; j++)
                {
                    switch (j)
                    {
                        case 0:
                        case 1:
                        case 2:
                            if (DgMode[j, i].Value != null) strTmp = DgMode[j, i].Value.ToString();
                            else strTmp = cm1.Items[2].ToString();
                            DgMode[j, i].Value = HDLPF.IsRightStringMode(strTmp);
                            myHVAC.bytSteps[40 + i * 20 + j * 2] = Convert.ToByte(cm1.Items.IndexOf(strTmp));
                            break;
                        case 3:
                            myHVAC.bytSteps[40 + i * 20 + 1] = Convert.ToByte(HDLPF.GetTimeFromString(DgMode[3, i].Value.ToString(), '.'));
                            myHVAC.bytSteps[40 + i * 20 + 3] = Convert.ToByte(HDLPF.GetTimeFromString(DgMode[3, i].Value.ToString(), '.'));
                            myHVAC.bytSteps[40 + i * 20 + 5] = Convert.ToByte(HDLPF.GetTimeFromString(DgMode[3, i].Value.ToString(), '.'));
                            break;
                    }
                }
            }
            this.Close();
        }

        private void cboDevice_SelectedIndexChanged(object sender, EventArgs e)
        {
            DisplayRelaySetups();
        }

        private void frmHvacRelay_Shown(object sender, EventArgs e)
        {
            cboDevice.SelectedIndex = cboDevice.Items.IndexOf(MyDeviceName);
        }
    }
}
