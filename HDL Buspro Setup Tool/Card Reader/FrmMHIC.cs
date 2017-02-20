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
    public partial class FrmMHIC : Form
    {
        private MHIC MyMHIC;
        private string myDevName = null;
        private int mintIDIndex = -1;
        private int MyDeviceType = -1;
        private int MyActivePage = 0; //按页面上传下载
        private byte SubNetID = 0;
        private byte DevID = 0;
        private bool isRead = false;
        private TimeText txtSeries = new TimeText(":");

        private System.Windows.Forms.Panel pnlOn = new System.Windows.Forms.Panel();
        private System.Windows.Forms.Panel pnlOff = new System.Windows.Forms.Panel();
        private NumericUpDown NumSensitivity = new NumericUpDown();

        private TextBox txtR = new TextBox();
        private TextBox txtG = new TextBox();
        private TextBox txtB = new TextBox();
        private bool blState = false;
        public FrmMHIC()
        {
            InitializeComponent();
        }
        public FrmMHIC(MHIC mymhic, string strName, int intDIndex, int intDeviceType)
        {
            InitializeComponent();
            this.MyMHIC = mymhic;
            this.myDevName = strName;
            this.mintIDIndex = intDIndex;
            this.MyDeviceType = intDeviceType;
            string strDevName = strName.Split('\\')[0].ToString();

            HDLSysPF.DisplayDeviceNameModeDescription(strName, MyDeviceType, cboDevice, tbModel, tbDescription);

            SubNetID = Convert.ToByte(strDevName.Split('-')[0]);
            DevID = Convert.ToByte(strDevName.Split('-')[1]);
            this.Text = strName;
            tsl3.Text = strName;
        }

        private void txtBroadSub_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar) || e.KeyChar == 8)
                e.Handled = false;
            else
                e.Handled = true;
        }

        void txtSeries_TextChanged(object sender, EventArgs e)
        {
            if (dgvKey.CurrentRow.Index < 0) return;
            if (dgvKey.RowCount <= 0) return;
            int index = dgvKey.CurrentRow.Index;
            string str = HDLPF.GetStringFromTime(Convert.ToInt32(txtSeries.Text), ":");
            if (txtSeries.Visible)
            {
                dgvKey[3, index].Value = str;
                MyMHIC.myKeySetting[index].Delay = Convert.ToInt32(txtSeries.Text);
            }
        }

        private void sb1_ValueChanged(object sender, EventArgs e)
        {
            lbv1.Text = sb1.Value.ToString();
            if (isRead) return;
            MyMHIC.Backlight = Convert.ToByte(sb1.Value);
        }

        private void FrmMHIC_Load(object sender, EventArgs e)
        {
            InitialFormCtrlsTextOrItems();
        }

        void InitialFormCtrlsTextOrItems()
        {
            #region
            dgvKey.Controls.Add(txtSeries);
            txtSeries.TextChanged += txtSeries_TextChanged;
            setAllVisible(false);
            #endregion
            #region
            dgvOther.Controls.Add(pnlOn);
            dgvOther.Controls.Add(pnlOff);
            dgvOther.Controls.Add(NumSensitivity);
            pnlOff.Click += pnlOff_Click;
            pnlOn.Click += pnlOn_Click;
            NumSensitivity.ValueChanged += NumSensitivity_ValueChanged;
            #endregion
            #region
            dgvBalance.Controls.Add(txtR);
            dgvBalance.Controls.Add(txtG);
            dgvBalance.Controls.Add(txtB);
            txtR.TextChanged += txtR_TextChanged;
            txtG.TextChanged += txtG_TextChanged;
            txtB.TextChanged += txtB_TextChanged;
            txtR.KeyPress += txtBroadSub_KeyPress;
            txtG.KeyPress += txtBroadSub_KeyPress;
            txtB.KeyPress += txtBroadSub_KeyPress;
            #endregion

            HDLSysPF.LoadButtonModeWithDifferentDeviceType(clK3, MyDeviceType);
        }

        void SetCtrlsVisbleWithDifferentDeviceType()
        {
            toolStrip1.Visible = (CsConst.MyEditMode == 0);
            //if (MyDeviceType == 3058) clK4.Visible = false;
            if (MyDeviceType != 3076 && MyDeviceType != 3078 && MyDeviceType != 3080) tabDLP.TabPages.Remove(tabOther);
        }

        private void sb2_ValueChanged(object sender, EventArgs e)
        {
            lbv2.Text = sb2.Value.ToString();
            if (isRead) return;
            MyMHIC.Ledlight = Convert.ToByte(sb2.Value);
        }

        private void tsbDown_Click(object sender, EventArgs e)
        {
            try
            {
                setAllVisible(false);
                byte bytTag = Convert.ToByte(((ToolStripButton)sender).Tag.ToString());
                bool blnShowMsg = (CsConst.MyEditMode != 1);
                if (HDLPF.UploadOrReadGoOn(this.Text, bytTag, blnShowMsg))
                {
                    Cursor.Current = Cursors.WaitCursor;

                    CsConst.MyUPload2DownLists = new List<byte[]>();

                    string strName = myDevName.Split('\\')[0].ToString();
                    byte bytSubID = byte.Parse(strName.Split('-')[0]);
                    byte bytDevID = byte.Parse(strName.Split('-')[1]);

                    byte[] ArayRelay = new byte[] { bytSubID, bytDevID, (byte)(MyDeviceType / 256), (byte)(MyDeviceType % 256), (byte)MyActivePage,
                                                (byte)(mintIDIndex / 256), (byte)(mintIDIndex % 256)};
                    CsConst.MyUPload2DownLists.Add(ArayRelay);
                    CsConst.MyUpload2Down = Convert.ToByte(((ToolStripButton)sender).Tag.ToString());
                    FrmDownloadShow Frm = new FrmDownloadShow();
                    Frm.FormClosing += new FormClosingEventHandler(UpdateDisplayInformationAccordingly);
                    Frm.ShowDialog();
                }
            }
            catch
            {
            }
        }

        void UpdateDisplayInformationAccordingly(object sender, FormClosingEventArgs e)
        {
            switch (tabDLP.SelectedIndex)
            {
                case 0: showBasicInfo(); break;
                case 1: ShowKeysInformationPanel(); break;
                case 2: showOthreinfo(); break;
            }
            this.TopMost = true;
            this.WindowState = FormWindowState.Normal;
            this.Activate();
            this.TopMost = false;
        }

        private void showOthreinfo()
        {
            try
            {
                isRead = true;
                int wdMaxValue = int.Parse(CsConst.mstrINIDefault.IniReadValue("DeviceType" + MyDeviceType.ToString(), "MaxValue", "0"));
                if (MyDeviceType == 3076 || MyDeviceType == 3078||MyDeviceType==3080) wdMaxValue = 3;
                dgvOther.Rows.Clear();
                for (int i = 1; i <= wdMaxValue; i++)
                {
                    object[] obj = new object[] { i.ToString(), "", "", MyMHIC.arayButtonSensitiVity[i - 1] };
                    dgvOther.Rows.Add(obj);
                }
                for (int i = 0; i < dgvOther.RowCount; i++)
                {
                    dgvOther[1, i].Style.BackColor = Color.FromArgb(MyMHIC.arayButtonColor[i * 6], MyMHIC.arayButtonColor[i * 6 + 1], MyMHIC.arayButtonColor[i * 6 + 2]);
                    dgvOther[2, i].Style.BackColor = Color.FromArgb(MyMHIC.arayButtonColor[i * 6 + 3], MyMHIC.arayButtonColor[i * 6 + 4], MyMHIC.arayButtonColor[i * 6 + 5]);
                }
                panel1.Visible = (MyMHIC.arayButtonBalance[0] > 0);
                if (MyMHIC.arayButtonBalance[0] > 0)
                {
                    dgvBalance.Rows.Clear();
                    for (int i = 0; i < MyMHIC.arayButtonBalance[0]; i++)
                    {
                        object[] obj = new object[] { dgvBalance.RowCount+1,MyMHIC.arayButtonBalance[i*3+1],
                                         MyMHIC.arayButtonBalance[i*3+2],MyMHIC.arayButtonBalance[i*3+3]};
                        dgvBalance.Rows.Add(obj);
                    }
                }
            }
            catch
            {
            }
            isRead = false;
        }

        private void showBasicInfo()
        {
            try
            {
                isRead = true;
                if (MyMHIC == null) return;
                sb1.Value = MyMHIC.Backlight;
                sb2.Value = MyMHIC.Ledlight;
                byte[] arayRemark = new byte[8];
                Array.Copy(MyMHIC.arayHotel, 0, arayRemark, 0, 8);
                txtName.Text = HDLPF.Byte2String(arayRemark);
                txtBuilding.Text = GlobalClass.AddLeftZero(MyMHIC.arayHotel[8].ToString("X"), 2);
                txtStorey.Text = GlobalClass.AddLeftZero(MyMHIC.arayHotel[9].ToString("X"), 2);
                txtRoom.Text = GlobalClass.AddLeftZero(MyMHIC.arayHotel[10].ToString("X"), 2) +
                             GlobalClass.AddLeftZero(MyMHIC.arayHotel[11].ToString("X"), 2);
                chbEnable.Checked = (MyMHIC.arayHotel[12] == 1);
            }
            catch
            {
            }
            isRead = false;
        }

        private void ShowKeysInformationPanel()
        {
            try
            {
                isRead = true;
                if (MyMHIC == null) return;
                if (MyMHIC.myKeySetting == null) return;
                setAllVisible(false);
                dgvKey.Rows.Clear();
                for (int i = 0; i < MyMHIC.myKeySetting.Count; i++)
                {
                    MHIC.Key temp = MyMHIC.myKeySetting[i];
                    string strDelay = CsConst.mstrInvalid;
                    string strMode = ButtonMode.ConvertorKeyModeToPublicModeGroup(temp.Mode);
                    if (!clK3.Items.Contains(strMode)) strMode = clK3.Items[0].ToString();
                    if (temp.Mode ==14) // 机械开关
                    {
                        strDelay = HDLPF.GetStringFromTime(temp.Delay, ":");
                    }
                    string strKey = (dgvKey.RowCount + 1).ToString();
                    if (MyDeviceType == 3076 || MyDeviceType == 3065 || MyDeviceType == 3068 ||
                        MyDeviceType == 3064 || MyDeviceType == 3069 || MyDeviceType == 3078 || MyDeviceType == 3080)
                        strKey = (i + 1).ToString() + "-" + CsConst.mstrINIDefault.IniReadValue("Public", "0028" + i.ToString(), "");

                    object[] obj = new object[] { strKey, temp.Remark, strMode, strDelay, "Test" };
                    int RowID = dgvKey.Rows.Add(obj);

                    if (temp.Mode != 14) 
                    {
                        dgvKey[3, RowID].ReadOnly = true;
                    }
                }
            }
            catch
            {
            }
            isRead = false;
        }


        private void setAllVisible(bool TF)
        {
            txtSeries.Visible = TF;

            pnlOn.Visible = TF;
            pnlOff.Visible = TF;
            NumSensitivity.Visible = TF;

            txtR.Visible = TF;
            txtG.Visible = TF;
            txtB.Visible = TF;
        }

        private void addcontrols(int col, int row, Control con, DataGridView dgv)
        {
            con.Show();
            con.Visible = true;
            Rectangle rect = dgv.GetCellDisplayRectangle(col, row, true);
            con.Size = rect.Size;
            con.Top = rect.Top;
            con.Left = rect.Left;
        }

        private void dgvKey_CellClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dgvKey_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dgvOther_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                setAllVisible(false);
                if (e.RowIndex >= 0)
                {
                    pnlOn.BackColor = dgvOther[1, e.RowIndex].Style.BackColor;
                    addcontrols(1, e.RowIndex, pnlOn, dgvOther);
                    pnlOn.Width = pnlOn.Width - 1;
                    pnlOn.Height = pnlOn.Height - 1;
                    pnlOff.BackColor = dgvOther[2, e.RowIndex].Style.BackColor;
                    addcontrols(2, e.RowIndex, pnlOff, dgvOther);
                    pnlOff.Width = pnlOff.Width - 1;
                    pnlOff.Height = pnlOff.Height - 1;
                    if (clO4.Visible)
                    {
                        NumSensitivity.Value = Convert.ToDecimal(dgvOther[3, e.RowIndex].Value.ToString());
                        addcontrols(3, e.RowIndex, NumSensitivity, dgvOther);
                    }
                }
                if (NumSensitivity.Visible) NumSensitivity_ValueChanged(null, null);
                if (e.ColumnIndex == 1) pnlOn_Click(null, null);
                if (e.ColumnIndex == 2) pnlOff_Click(null, null);
            }
            catch
            {
            }
        }

        void NumSensitivity_ValueChanged(object sender, EventArgs e)
        {
            if (dgvOther.CurrentRow.Index < 0) return;
            if (dgvOther.RowCount <= 0) return;
            int index = dgvOther.CurrentRow.Index;
            dgvOther[3, index].Value = NumSensitivity.Value.ToString();
            string strTmp = dgvOther[3, index].Value.ToString();
            if (dgvOther.SelectedRows == null || dgvOther.SelectedRows.Count == 0) return;
            if (strTmp == null) strTmp = "";
            if (dgvOther.SelectedRows.Count > 1)
            {
                for (int i = 0; i < dgvOther.SelectedRows.Count; i++)
                {
                    if (dgvOther.SelectedRows[i].Cells[3].Value.ToString() != "N/A")
                    {
                        dgvOther.SelectedRows[i].Cells[3].Value = strTmp;
                    }
                }
            }
        }

        void pnlOn_Click(object sender, EventArgs e)
        {
            if (dgvOther.CurrentRow.Index < 0) return;
            if (dgvOther.RowCount <= 0) return;
            int index = dgvOther.CurrentRow.Index;
            if (PanelColorDialog.ShowDialog() == DialogResult.OK)
            {
                dgvOther[1, index].Style.BackColor = PanelColorDialog.Color;
                dgvOther[1, index].Style.SelectionBackColor = PanelColorDialog.Color;
                pnlOn.BackColor = PanelColorDialog.Color;
                if (dgvOther.SelectedRows == null || dgvOther.SelectedRows.Count == 0) return;
                if (dgvOther.SelectedRows.Count > 1)
                {
                    for (int i = 0; i < dgvOther.SelectedRows.Count; i++)
                    {
                        dgvOther[1, index].Style.BackColor = PanelColorDialog.Color;
                    }
                }
            }
        }

        void pnlOff_Click(object sender, EventArgs e)
        {
            if (dgvOther.CurrentRow.Index < 0) return;
            if (dgvOther.RowCount <= 0) return;
            int index = dgvOther.CurrentRow.Index;
            if (PanelColorDialog.ShowDialog() == DialogResult.OK)
            {
                dgvOther[2, index].Style.BackColor = PanelColorDialog.Color;
                dgvOther[2, index].Style.SelectionBackColor = PanelColorDialog.Color;
                pnlOff.BackColor = PanelColorDialog.Color;
                if (dgvOther.SelectedRows == null || dgvOther.SelectedRows.Count == 0) return;

                if (dgvOther.SelectedRows.Count > 1)
                {
                    for (int i = 0; i < dgvOther.SelectedRows.Count; i++)
                    {
                        dgvOther[2, index].Style.BackColor = PanelColorDialog.Color;
                    }
                }
            }
        }

        private void dgvBalance_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                setAllVisible(false);
                if (e.RowIndex >= 0)
                {
                    txtR.Text = dgvBalance[1, e.RowIndex].Value.ToString();
                    addcontrols(1, e.RowIndex, txtR, dgvBalance);

                    txtG.Text = dgvBalance[2, e.RowIndex].Value.ToString();
                    addcontrols(2, e.RowIndex, txtG, dgvBalance);

                    txtB.Text = dgvBalance[3, e.RowIndex].Value.ToString();
                    addcontrols(3, e.RowIndex, txtB, dgvBalance);
                }
                if (txtR.Visible) txtR_TextChanged(null, null);
                if (txtG.Visible) txtG_TextChanged(null, null);
                if (txtB.Visible) txtB_TextChanged(null, null);
            }
            catch
            {
            }
        }

        void txtB_TextChanged(object sender, EventArgs e)
        {
            if (dgvBalance.CurrentRow.Index < 0) return;
            if (dgvBalance.RowCount <= 0) return;
            int index = dgvBalance.CurrentRow.Index;
            if (txtB.Text.Length > 0)
            {
                txtB.Text = HDLPF.IsNumStringMode(txtB.Text, 0, 255);
                txtB.SelectionStart = txtB.Text.Length;
                dgvBalance[3, index].Value = txtB.Text;
                ModifyMultilinesIfNeeds(dgvBalance[3, index].Value.ToString(), 3, dgvBalance);
            }
        }

        void txtG_TextChanged(object sender, EventArgs e)
        {
            if (dgvBalance.CurrentRow.Index < 0) return;
            if (dgvBalance.RowCount <= 0) return;
            int index = dgvBalance.CurrentRow.Index;
            if (txtG.Text.Length > 0)
            {
                txtG.Text = HDLPF.IsNumStringMode(txtG.Text, 0, 255);
                txtG.SelectionStart = txtG.Text.Length;
                dgvBalance[2, index].Value = txtG.Text;
                ModifyMultilinesIfNeeds(dgvBalance[2, index].Value.ToString(), 2, dgvBalance);
            }
        }

        void txtR_TextChanged(object sender, EventArgs e)
        {
            if (dgvBalance.CurrentRow.Index < 0) return;
            if (dgvBalance.RowCount <= 0) return;
            int index = dgvBalance.CurrentRow.Index;
            if (txtR.Text.Length > 0)
            {
                txtR.Text = HDLPF.IsNumStringMode(txtR.Text, 0, 255);
                txtR.SelectionStart = txtR.Text.Length;
                dgvBalance[1, index].Value = txtR.Text;
                ModifyMultilinesIfNeeds(dgvBalance[1, index].Value.ToString(), 1, dgvBalance);
            }
        }

        private void btnSaveOther_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            MyMHIC.arayButtonSensitiVity = new byte[dgvOther.RowCount];
            MyMHIC.arayButtonColor = new byte[dgvOther.RowCount * 6];
            MyMHIC.arayButtonBalance = new byte[dgvOther.RowCount * 3 + 1];
            for (int i = 0; i < dgvOther.RowCount; i++)
            {
                MyMHIC.arayButtonColor[i * 6] = dgvOther[1, i].Style.BackColor.R;
                MyMHIC.arayButtonColor[i * 6 + 1] = dgvOther[1, i].Style.BackColor.G;
                MyMHIC.arayButtonColor[i * 6 + 2] = dgvOther[1, i].Style.BackColor.B;
                MyMHIC.arayButtonColor[i * 6 + 3] = dgvOther[2, i].Style.BackColor.R;
                MyMHIC.arayButtonColor[i * 6 + 4] = dgvOther[2, i].Style.BackColor.G;
                MyMHIC.arayButtonColor[i * 6 + 5] = dgvOther[2, i].Style.BackColor.B;
                if (clO4.Visible)
                {
                    MyMHIC.arayButtonSensitiVity[i] = Convert.ToByte(dgvOther[3, i].Value.ToString());
                }
            }
            if (panel1.Visible)
            {
                MyMHIC.arayButtonBalance[0] = Convert.ToByte(dgvBalance.RowCount);
                for (int i = 0; i < dgvBalance.RowCount; i++)
                {
                    MyMHIC.arayButtonBalance[i * 3 + 1] = Convert.ToByte(dgvBalance[1, i].Value.ToString());
                    MyMHIC.arayButtonBalance[i * 3 + 2] = Convert.ToByte(dgvBalance[2, i].Value.ToString());
                    MyMHIC.arayButtonBalance[i * 3 + 3] = Convert.ToByte(dgvBalance[3, i].Value.ToString());
                }
            }
            tsbDown_Click(toolStripLabel2, null);
        }

        void ModifyMultilinesIfNeeds(string strTmp, int ColumnIndex, DataGridView dgv)
        {
            if (dgv.SelectedRows == null || dgv.SelectedRows.Count == 0) return;
            if (strTmp == null) strTmp = "";
            // change the value in selected more than one line
            if (dgv.SelectedRows.Count > 1)
            {
                for (int i = 0; i < dgv.SelectedRows.Count; i++)
                {
                    dgv.SelectedRows[i].Cells[ColumnIndex].Value = strTmp;
                }
            }
        }

        private void btnSaveKey_Click(object sender, EventArgs e)
        {
            setAllVisible(false);
           
            tsbDown_Click(toolStripLabel2, null);
        }

        private void btnTargets_Click(object sender, EventArgs e)
        {
            Byte[] PageID = new Byte[4];
            if (dgvKey.SelectedRows != null && dgvKey.SelectedRows.Count > 0)
            {
                PageID[0] = (Byte)dgvKey.SelectedRows[0].Index;
            }
            frmCmdSetup CmdSetup = new frmCmdSetup(MyMHIC, myDevName, MyDeviceType, PageID);
            CmdSetup.Show();
        }

        private void btnSaveBasic_Click(object sender, EventArgs e)
        {
            try
            {
                if (tabDLP.SelectedTab.Name == "tabOther")
                {
                    Cursor.Current = Cursors.WaitCursor;
                    MyMHIC.arayButtonSensitiVity = new byte[dgvOther.RowCount];
                    MyMHIC.arayButtonColor = new byte[dgvOther.RowCount * 6];
                    MyMHIC.arayButtonBalance = new byte[dgvOther.RowCount * 3 + 1];
                    for (int i = 0; i < dgvOther.RowCount; i++)
                    {
                        MyMHIC.arayButtonColor[i * 6] = dgvOther[1, i].Style.BackColor.R;
                        MyMHIC.arayButtonColor[i * 6 + 1] = dgvOther[1, i].Style.BackColor.G;
                        MyMHIC.arayButtonColor[i * 6 + 2] = dgvOther[1, i].Style.BackColor.B;
                        MyMHIC.arayButtonColor[i * 6 + 3] = dgvOther[2, i].Style.BackColor.R;
                        MyMHIC.arayButtonColor[i * 6 + 4] = dgvOther[2, i].Style.BackColor.G;
                        MyMHIC.arayButtonColor[i * 6 + 5] = dgvOther[2, i].Style.BackColor.B;
                        if (clO4.Visible)
                        {
                            MyMHIC.arayButtonSensitiVity[i] = Convert.ToByte(dgvOther[3, i].Value.ToString());
                        }
                    }
                    if (panel1.Visible)
                    {
                        MyMHIC.arayButtonBalance[0] = Convert.ToByte(dgvBalance.RowCount);
                        for (int i = 0; i < dgvBalance.RowCount; i++)
                        {
                            MyMHIC.arayButtonBalance[i * 3 + 1] = Convert.ToByte(dgvBalance[1, i].Value.ToString());
                            MyMHIC.arayButtonBalance[i * 3 + 2] = Convert.ToByte(dgvBalance[2, i].Value.ToString());
                            MyMHIC.arayButtonBalance[i * 3 + 3] = Convert.ToByte(dgvBalance[3, i].Value.ToString());
                        }
                    }
                }
                tsbDown_Click(toolStripLabel2, null);
            }
            catch
            { }
        }

        private void txtBuilding_KeyPress(object sender, KeyPressEventArgs e)
        {
            string str = "abcdefABCDEF1234567890";
            if (e.KeyChar != 8)
            {
                if (str.IndexOf(e.KeyChar.ToString()) < 0)
                {
                    e.Handled = true;
                }
            }
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            if (MyMHIC.arayHotel == null) MyMHIC.arayHotel = new byte[13];
            string str = txtName.Text.Trim();
            byte[] arayTmpRemark = HDLUDP.StringToByte(str);
            Array.Copy(arayTmpRemark, 0, MyMHIC.arayHotel, 0, 8);
        }

        private void txtBuilding_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtStorey_TextChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            if (MyMHIC.arayHotel == null) MyMHIC.arayHotel = new byte[13];
            if (txtStorey.Text.Length > 0)
                MyMHIC.arayHotel[9] = Convert.ToByte(txtStorey.Text, 16);
        }

        private void txtRoom_TextChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            if (MyMHIC.arayHotel == null) MyMHIC.arayHotel = new byte[13];

            string str = txtRoom.Text;
            str = GlobalClass.AddLeftZero(str, 4);
            MyMHIC.arayHotel[10] = Convert.ToByte(str.Substring(0, 2), 16);
            MyMHIC.arayHotel[11] = Convert.ToByte(str.Substring(2, 2), 16);
        }

        private void chbEnable_CheckedChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            if (MyMHIC.arayHotel == null) MyMHIC.arayHotel = new byte[13];
            if (chbEnable.Checked) MyMHIC.arayHotel[12] = 1;
            else MyMHIC.arayHotel[12] = 0;
        }

        private void tmRead_Click(object sender, EventArgs e)
        {
            tsbDown_Click(tsbDown, null);
        }

        private void tmUpload_Click(object sender, EventArgs e)
        {
            tsbDown_Click(toolStripLabel2, null);
        }

        private void FrmMHIC_Shown(object sender, EventArgs e)
        {
            isRead = true;
            SetCtrlsVisbleWithDifferentDeviceType();
            if (CsConst.MyEditMode == 0)
            {

            }
            else if (CsConst.MyEditMode == 1) //在线模式
            {
                MyActivePage = 1;
                tsbDown_Click(tsbDown, null);
            }
            isRead = false;
        }

        private void tabDLP_SelectedIndexChanged(object sender, EventArgs e)
        {
            MyActivePage = tabDLP.SelectedIndex + 1;
            if (CsConst.MyEditMode == 1)
            {
                if (MyMHIC.MyRead2UpFlags[MyActivePage - 1] == false)
                {
                    tsbDown_Click(tsbDown, null);
                }
                else
                {
                    //基本信息
                    UpdateDisplayInformationAccordingly(null, null);
                }
            }
        }

        private void btnSaveAndClose_Click(object sender, EventArgs e)
        {
            btnSaveBasic_Click(null, null);
            this.Close();
        }

        private void btnSaveAndClose2_Click(object sender, EventArgs e)
        {
            btnSaveKey_Click(null, null);
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            btnSaveOther_Click(null, null);
            this.Close();
        }

        private void dgvKey_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            btnTargets_Click(null, null);
        }

        private void dgvKey_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (isRead) return;
            if (e.ColumnIndex == -1 || e.RowIndex == -1) return;
            if (dgvKey.CurrentRow.Index < 0) return;
            if (dgvKey.RowCount <= 0) return;
            Cursor.Current = Cursors.WaitCursor;

            if (dgvKey[e.ColumnIndex, e.RowIndex].Value == null) dgvKey[e.ColumnIndex, e.RowIndex].Value = "";
            String Remark = dgvKey[e.ColumnIndex, e.RowIndex].Value.ToString();

            if (dgvKey.SelectedRows.Count > 0)
            {
                for (int i = 0; i < dgvKey.SelectedRows.Count; i++)
                {
                    dgvKey.SelectedRows[i].Cells[e.ColumnIndex].Value = Remark;
                    int index = dgvKey.SelectedRows[i].Index;
                    switch (e.ColumnIndex)
                    {
                        case 1:
                            MyMHIC.myKeySetting[index].Remark = Remark;
                            break;
                        case 2:
                            MyMHIC.myKeySetting[index].Mode = ButtonMode.ConvertorKeyModesToPublicModeGroup(Remark);
                            if (MyMHIC.myKeySetting[index].Mode == 14)
                            {
                                addcontrols(3, index, txtSeries, dgvKey);
                                string str = HDLPF.GetStringFromTime(MyMHIC.myKeySetting[index].Delay, ":");
                                txtSeries.Text = HDLPF.GetTimeFromString(str, ':');
                            }
                            break;
                        case 3:
                            byte[] arayTmp = new byte[3];
                            arayTmp[0] = 18;
                            arayTmp[1] = Convert.ToByte(e.RowIndex + 1);
                            arayTmp[2] = 255;
                            string sTestFlag = dgvKey[4, e.RowIndex].Value.ToString();
                            if (sTestFlag.ToLower() == "false")
                            {
                                arayTmp[2] = 0;
                            }
                            if (CsConst.mySends.AddBufToSndList(arayTmp, 0xE3D8, SubNetID, DevID, false, false, false, CsConst.minAllWirelessDeviceType.Contains(MyDeviceType)) == true)
                            {
                                CsConst.myRevBuf = new byte[1200];
                            }
                            break;
                    }
                }
            }
            Cursor.Current = Cursors.Default;

        }

        private void dgvKey_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            dgvKey.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void dgvKey_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (isRead) return;
            if (e.ColumnIndex == -1 || e.RowIndex == -1) return;
            if (dgvKey.CurrentRow.Index < 0) return;
            if (dgvKey.RowCount <= 0) return;
            Cursor.Current = Cursors.WaitCursor;
            txtSeries.Visible = false;

            if (dgvKey[2, e.RowIndex].Value == null) dgvKey[e.ColumnIndex, e.RowIndex].Value = "";
            String Remark = dgvKey[2, e.RowIndex].Value.ToString();
            if (Remark == CsConst.mstrInvalid) return;

            Byte bTmpMode = ButtonMode.ConvertorKeyModesToPublicModeGroup(Remark);
            if (bTmpMode == 14)
            {
                addcontrols(3, e.RowIndex, txtSeries, dgvKey);
                string str = HDLPF.GetStringFromTime(MyMHIC.myKeySetting[e.RowIndex].Delay, ":");
                txtSeries.Text = HDLPF.GetTimeFromString(str, ':');
            }
                  
            Cursor.Current = Cursors.Default;
        }
    }
}
