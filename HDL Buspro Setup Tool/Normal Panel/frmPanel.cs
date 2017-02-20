using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace HDL_Buspro_Setup_Tool
{
    public partial class frmPanel : Form
    {
        [DllImport("user32.dll")]
        public static extern Boolean FlashWindow(IntPtr hwnd, Boolean bInvert);
        private byte SubNetID;
        private byte DeviceID;
        private Panel oPanel = null;
        private string MyDevName = null;
        private int mywdDevicerType = 0;  
        private int mintIDIndex = -1;
        private int MyActivePage = 0; //按页面上传下载
        private bool isRead = true;
        private byte[] MyBufferProperties = new byte[20];
        private TextBox txtKeyRemark = new TextBox();

        private ComboBox cbKeyDim = new ComboBox();
        private ComboBox cbKeySaveDim = new ComboBox();
        private ComboBox cbKeyMutex = new ComboBox();
        private ComboBox cbKeyLED = new ComboBox();
        private ComboBox cbKeylink = new ComboBox();
        private Boolean BlnShowBasicInformation = false;

        private System.Windows.Forms.Panel pnlOn = new System.Windows.Forms.Panel();
        private System.Windows.Forms.Panel pnlOff = new System.Windows.Forms.Panel();
        private NumericUpDown NumSensitivity = new NumericUpDown();

        private TextBox txtR = new TextBox();
        private TextBox txtG = new TextBox();
        private TextBox txtB = new TextBox();

        private ComboBox cbTempEnable = new ComboBox();
        private TextBox txtSub = new TextBox();
        private TextBox txtDev = new TextBox();
        private ComboBox cbCompen = new ComboBox();

        private SingleChn sbTest = new SingleChn();

        public frmPanel()
        {
            InitializeComponent();
        }

        public frmPanel(Panel oPan, string strName, int intDeviceType, int intDIndex)
        {
            InitializeComponent();
            this.oPanel = oPan;
            this.mywdDevicerType = intDeviceType;
            this.MyDevName = strName;
            this.mintIDIndex = intDIndex;

            string strDevName = strName.Split('\\')[0].ToString();

            HDLSysPF.DisplayDeviceNameModeDescription(strName, mywdDevicerType, cboDevice, tbModel, tbDescription);

            SubNetID = Convert.ToByte(strDevName.Split('-')[0]);
            DeviceID = Convert.ToByte(strDevName.Split('-')[1]);
        }

        void InitialFormCtrlsTextOrItems()
        {
            #region
            dgvKey.Controls.Add(txtKeyRemark);

            txtKeyRemark.TextChanged += txtKeyRemark_TextChanged;

            HDLSysPF.LoadButtonModeWithDifferentDeviceType(clK3, mywdDevicerType);

            int MaxValue = int.Parse(CsConst.mstrINIDefault.IniReadValue("DeviceType" + mywdDevicerType.ToString(), "MaxValue", "1"));
            for (int i = 1; i <= MaxValue; i++)
                cbKeylink.Items.Add(i.ToString());

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
            #region
            cbTempEnable.Items.Clear();
            cbTempEnable.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99826", ""));
            cbTempEnable.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99827", ""));
            cbCompen.Items.Clear();
            for (int i = 2; i <= 18; i++)
                cbCompen.Items.Add((i - 10).ToString());
            cbCompen.DropDownStyle = ComboBoxStyle.DropDownList;
            cbTempEnable.DropDownStyle = ComboBoxStyle.DropDownList;
            cbTempEnable.SelectedIndexChanged += cbTempEnable_SelectedIndexChanged;
            cbCompen.SelectedIndexChanged += cbCompen_SelectedIndexChanged;
            txtSub.TextChanged += txtSub_TextChanged;
            txtDev.TextChanged += txtDev_TextChanged;
            dgvTemp.Controls.Add(cbTempEnable);
            dgvTemp.Controls.Add(cbCompen);
            dgvTemp.Controls.Add(txtSub);
            dgvTemp.Controls.Add(txtDev);
            #endregion
            #region
            sbTest = new SingleChn();
            //sbTest.UserControlValueChanged += sbTest_ValueChanged;
            DgChns.Controls.Add(sbTest);
            #endregion

            #region
            for (int i = 1; i <= 4; i++)
            {
                ComboBox temp = this.Controls.Find("cbHeatSensor" + i.ToString(), true)[0] as ComboBox;
                temp.Items.Clear();
                temp.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99647", ""));
                temp.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99891", ""));
                if (CsConst.mintNewDLPFHSetupDeviceType.Contains(mywdDevicerType))
                    temp.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99892", ""));
            }
            cbHeatSpeed.Items.Clear();
            for (int i = 0; i <= 5; i++)
            {
                cbHeatSpeed.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "0015" + i.ToString(), ""));
            }
            cbMinPWM.Items.Clear();
            cbMaxPWM.Items.Clear();
            for (int i = 0; i <= 100; i++)
            {
                cbMinPWM.Items.Add(i.ToString() + "%");
                cbMaxPWM.Items.Add(i.ToString() + "%");
            }
            #endregion

            cbCurrentMode.Items.Clear();
            chbListHeatMode.Items.Clear();
            for (int i = 0; i <= 4; i++)
            {
                cbCurrentMode.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "0007" + i.ToString(), ""));
                chbListHeatMode.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "0007" + i.ToString(), ""));
            }

            cl32.Items.Clear();
            CsConst.LoadType = CsConst.mstrINIDefault.IniReadValuesInALlSectionStr("PublicLoads");
            cl32.Items.AddRange(CsConst.LoadType);            

            cbBaseDimming.Items.Clear();
            cbBaseDimming.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99879", ""));
            cbBaseDimming.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99880", ""));

            cbHeatType.Items.Clear();
            cbHeatType.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00055", ""));
            cbHeatType.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99886", ""));
            cbHeatType.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99887", ""));
            cbHeatType.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99888", ""));

            cbControl.Items.Clear();
            cbControl.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99889", ""));
            cbControl.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99890", ""));

            cbPages.Items.Clear();
            for (int i = 1; i < 6; i++)
            {
                cbPages.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "0080" + i.ToString(), ""));
            }
            setAllVisible(false);
        }

        void SetCtrlsVisbleWithDifferentDeviceType()
        {
            if (oPanel == null) return;
            toolStrip1.Visible = (CsConst.MyEditMode == 0);
            
            Boolean BlnHasWireless = NormalPanelDeviceTypeList.NormalWIFIPanelDeviceType.Contains(mywdDevicerType);
            Boolean BlnRoundButton = NormalPanelDeviceTypeList.NormalWIFIRoundButtonPanelDeviceType.Contains(mywdDevicerType)||
                                     NormalPanelDeviceTypeList.NormalRoundButtonPanelDeviceType.Contains(mywdDevicerType);

            Boolean BlnTouchGlass = NormalPanelDeviceTypeList.NormalWIFIThouchPanelDeviceType.Contains(mywdDevicerType) ||
                                    NormalPanelDeviceTypeList.NormalTouchButtonPanelDeviceType.Contains(mywdDevicerType);
            Boolean BlnIsNewWirelessPanel = NormalPanelDeviceTypeList.NormalNewWIFIPanelDeviceType.Contains(mywdDevicerType);
            Boolean BlnHasNoBalance = NormalPanelDeviceTypeList.PanelHasNoBalanceLightDeviceType.Contains(mywdDevicerType);
            Boolean BlnHasFloorheating = NormalPanelDeviceTypeList.WirelessSimpleFloorHeatingDeviceTypeList.Contains(mywdDevicerType);

            gbLcd.Visible = BlnHasFloorheating;

            if (NormalPanelDeviceTypeList.PanelKorayPanelWithOutTemperature.Contains(mywdDevicerType) ||
                NormalPanelDeviceTypeList.TouchPanelGenerationOneWithOutTemperature.Contains(mywdDevicerType))
                gbTemperature.Visible = false;
            else
            {
                String TmpVersion = HDLSysPF.ReadDeviceVersion(SubNetID, DeviceID,false);
                if (TmpVersion != null && TmpVersion != "" && TmpVersion.Contains("_"))
                {
                    String[] Tmp = TmpVersion.Split('_').ToArray();

                    gbTemperature.Visible = (string.Compare(Tmp[2], "2013/10") >= 0 && Tmp[2] != "2013/12/10");
                }
            }

            if (NormalPanelDeviceTypeList.IranSpeicalTouchButtonPanelDeviceType.Contains(mywdDevicerType))
            {
                clO2.Visible = false;
                clO3.Visible = false;
            }

            clO4.Visible = BlnTouchGlass;

            if (BlnHasFloorheating == false)
            {
                tabDLP.TabPages.Remove(tabHeat);
            }
            if (BlnHasWireless == false)
            {
                tabDLP.TabPages.Remove(tabChns);
                tabDLP.TabPages.Remove(tabDimmer);
            }

            if (BlnIsNewWirelessPanel == false)
            {
                tabDLP.TabPages.Remove(tabDimmer);
            }

            if (BlnRoundButton == false && BlnTouchGlass == false)
            {
                tabDLP.TabPages.Remove(tabOther);
            }

            if (BlnTouchGlass == true)
            {
                lbDoubleTime.Visible = false;
                lbDValue.Visible = lbDoubleTime.Visible;
                lbDHint.Visible = lbDoubleTime.Visible;
                numD1.Visible = lbDoubleTime.Visible;
                numD2.Visible = lbDoubleTime.Visible;
            }

            gbBalance.Visible = !BlnHasNoBalance;
        }

        void sbTest_ValueChanged(object sender, EventArgs e)
        {
             if (DgChns.CurrentRow.Index < 0) return;
             if (DgChns.RowCount <= 0) return;

             Byte RowID = (Byte)DgChns.CurrentRow.Index;
             DgChns[4, RowID].Value = sbTest.Text;
             if (DgChns[5, RowID].Value.ToString().ToLower() == "true")
             {
                 OnsiteOutput(RowID); 
             }
        }

        void txtDev_TextChanged(object sender, EventArgs e)
        {
            if (dgvTemp.CurrentRow.Index < 0) return;
            if (dgvTemp.RowCount <= 0) return;
            int index = dgvTemp.CurrentRow.Index;
            if (txtDev.Text.Length > 0)
            {
                txtDev.Text = HDLPF.IsNumStringMode(txtDev.Text, 0, 255);
                txtDev.SelectionStart = txtDev.Text.Length;
                dgvTemp[3, index].Value = txtDev.Text;
                ModifyMultilinesIfNeeds(dgvTemp[3, index].Value.ToString(), 3, dgvTemp);
            }
        }

        void txtSub_TextChanged(object sender, EventArgs e)
        {
            if (dgvTemp.CurrentRow.Index < 0) return;
            if (dgvTemp.RowCount <= 0) return;
            int index = dgvTemp.CurrentRow.Index;
            if (txtSub.Text.Length > 0)
            {
                txtSub.Text = HDLPF.IsNumStringMode(txtSub.Text, 0, 255);
                txtSub.SelectionStart = txtSub.Text.Length;
                dgvTemp[2, index].Value = txtSub.Text;
                ModifyMultilinesIfNeeds(dgvTemp[2, index].Value.ToString(), 2, dgvTemp);
            }
        }

        void cbCompen_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dgvTemp.CurrentRow.Index < 0) return;
            if (dgvTemp.RowCount <= 0) return;
            int index = dgvTemp.CurrentRow.Index;
            dgvTemp[4, index].Value = cbCompen.Text;
            ModifyMultilinesIfNeeds(dgvTemp[4, index].Value.ToString(), 4, dgvTemp);
        }

        void cbTempEnable_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dgvTemp.CurrentRow.Index < 0) return;
            if (dgvTemp.RowCount <= 0) return;
            int index = dgvTemp.CurrentRow.Index;
            dgvTemp[1, index].Value = cbTempEnable.Text;
            ModifyMultilinesIfNeeds(dgvTemp[1, index].Value.ToString(), 1, dgvTemp);
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

        void cbKeylink_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dgvKey.CurrentRow.Index < 0) return;
            if (dgvKey.RowCount <= 0) return;
            int index = dgvKey.CurrentRow.Index;
            dgvKey[7, index].Value = cbKeylink.Text;
            string strTmp = dgvKey[7, index].Value.ToString();
            if (dgvKey.SelectedRows == null || dgvKey.SelectedRows.Count == 0) return;
            if (strTmp == null) strTmp = "";
            if (dgvKey.SelectedRows.Count > 1)
            {
                for (int i = 0; i < dgvKey.SelectedRows.Count; i++)
                {
                    if (dgvKey.SelectedRows[i].Cells[7].Value.ToString() != "N/A")
                    {
                        dgvKey.SelectedRows[i].Cells[7].Value = strTmp;
                    }
                }
            }
        }

        void txtKeyRemark_TextChanged(object sender, EventArgs e)
        {
            if (dgvKey.CurrentRow.Index < 0) return;
            if (dgvKey.RowCount <= 0) return;
            int index = dgvKey.CurrentRow.Index;
            if (txtKeyRemark.Text == null) txtKeyRemark.Text = "";
            dgvKey[1, index].Value = txtKeyRemark.Text;
            ModifyMultilinesIfNeeds(dgvKey[1, index].Value.ToString(), 1, dgvKey);
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

        void cbKeyLED_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dgvKey.CurrentRow.Index < 0) return;
            if (dgvKey.RowCount <= 0) return;
            int index = dgvKey.CurrentRow.Index;
            dgvKey[5, index].Value = cbKeyLED.Text;
            ModifyMultilinesIfNeeds(dgvKey[5, index].Value.ToString(), 5, dgvKey);
        }

        void cbKeyMutex_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dgvKey.CurrentRow.Index < 0) return;
            if (dgvKey.RowCount <= 0) return;
            int index = dgvKey.CurrentRow.Index;
            dgvKey[6, index].Value = cbKeyMutex.Text;
            string strTmp = dgvKey[6, index].Value.ToString();
            if (dgvKey.SelectedRows == null || dgvKey.SelectedRows.Count == 0) return;
            if (strTmp == null) strTmp = "";
            if (dgvKey.SelectedRows.Count > 1)
            {
                for (int i = 0; i < dgvKey.SelectedRows.Count; i++)
                {
                    if (dgvKey.SelectedRows[i].Cells[6].Value.ToString() != "N/A")
                    {
                        dgvKey.SelectedRows[i].Cells[6].Value = strTmp;
                    }
                }
            }
        }

        void cbKeySaveDim_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dgvKey.CurrentRow.Index < 0) return;
            if (dgvKey.RowCount <= 0) return;
            int index = dgvKey.CurrentRow.Index;
            dgvKey[4, index].Value = cbKeySaveDim.Text;
            ModifyMultilinesIfNeeds(dgvKey[4, index].Value.ToString(), 4, dgvKey);
        }

        void cbKeyDim_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dgvKey.CurrentRow.Index < 0) return;
            if (dgvKey.RowCount <= 0) return;
            int index = dgvKey.CurrentRow.Index;
            dgvKey[3, index].Value = cbKeyDim.Text;
            ModifyMultilinesIfNeeds(dgvKey[3, index].Value.ToString(), 3, dgvKey);
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

        private void setAllVisible(bool TF)
        {
            cbKeyDim.Visible = TF;
            cbKeySaveDim.Visible = TF;
            cbKeyMutex.Visible = TF;
            cbKeyLED.Visible = TF;
            txtKeyRemark.Visible = TF;
            cbKeylink.Visible = TF;
            pnlOn.Visible = TF;
            pnlOff.Visible = TF;
            NumSensitivity.Visible = TF;

            txtR.Visible = TF;
            txtG.Visible = TF;
            txtB.Visible = TF;

            cbTempEnable.Visible = TF;
            cbCompen.Visible = TF;
            txtSub.Visible = TF;
            txtDev.Visible = TF;

            sbTest.Visible = TF;
        }

        private void frmPanel_Load(object sender, EventArgs e)
        {
            InitialFormCtrlsTextOrItems();
        }

        void ShowBasicInformationPanel()
        {
            if (oPanel == null) return;

            //基本信息的现在
            if (oPanel.otherInfo == null)
            {
                oPanel.otherInfo = new Panel.OtherInfo();
                if (oPanel.otherInfo.bytAryShowPage == null) oPanel.otherInfo.bytAryShowPage = new byte[7];
            }

            if (Convert.ToInt32(oPanel.Backlight) <= sb1.Maximum)
                sb1.Value = oPanel.Backlight;
            if (Convert.ToInt32(oPanel.Ledlight) <= sb2.Maximum)
                sb2.Value = oPanel.Ledlight;

            if (Convert.ToInt32(oPanel.LlimitDimmer) <= sb4.Maximum)
                sb4.Value = oPanel.LlimitDimmer;


            if (3 <= oPanel.LTime && oPanel.LTime <= 250)
            {
                numL1.Value = Convert.ToDecimal(oPanel.LTime / 10);
                numL2.Value = Convert.ToDecimal(oPanel.LTime % 10);
            }

            if (1 <= oPanel.DTime && oPanel.DTime <= 20)
            {
                numD1.Value = Convert.ToDecimal(oPanel.DTime / 10);
                numD2.Value = Convert.ToDecimal(oPanel.DTime % 10);
            }

            chbLock.Checked = (oPanel.LockKeys == 1);
            chbIR.Checked = (oPanel.IRON == 0);
            isRead = false;
            sb1_ValueChanged(null, null);
            sb2_ValueChanged(null, null);
            sb4_ValueChanged(null, null);
        }

        void ShowKeysInformationPanel()
        {
            try
            {
                setAllVisible(false);
                if (oPanel == null) return;
                if (oPanel.PanelKey == null) return;
                dgvKey.Rows.Clear();
                for (int i = 0; i < oPanel.PanelKey.Count; i++)
                {
                    HDLButton temp = oPanel.PanelKey[i];
                    string strMode = ButtonMode.ConvertorKeyModeToPublicModeGroup(temp.Mode);

                    object[] obj = new object[] { dgvKey.RowCount + 1, temp.Remark, strMode,false}; //strDimming, strSaveDimmingValue, strLED,strMutex,strDouble
                    dgvKey.Rows.Add(obj);
                }
            }
            catch
            {
            }
            isRead = false;
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            if (oPanel == null) return;
            Cursor.Current = Cursors.WaitCursor;
            oPanel.SavePanelToDB();
            Cursor.Current = Cursors.Default;
        }

        private void frmPanel_SizeChanged(object sender, EventArgs e)
        {

        }

        private void frmPanel_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Dispose();
        }

        private void toolStripButton11_Click(object sender, EventArgs e)
        {
            if (oPanel == null) return;
        }

        private void tsbDown_Click(object sender, EventArgs e)
        {
            try
            {
                byte bytTag = Convert.ToByte(((ToolStripButton)sender).Tag.ToString());
                bool blnShowMsg = (CsConst.MyEditMode != 1);
                if (HDLPF.UploadOrReadGoOn(this.Text, bytTag, blnShowMsg))
                {
                    Cursor.Current = Cursors.WaitCursor;
                    // SetVisableForDownOrUpload(false);
                    // ReadDownLoadThread();  //增加线程，使得当前窗体的任何操作不被限制

                    CsConst.MyUPload2DownLists = new List<byte[]>();

                    string strName = MyDevName.Split('\\')[0].ToString();
                    byte bytSubID = byte.Parse(strName.Split('-')[0]);
                    byte bytDevID = byte.Parse(strName.Split('-')[1]);
                    int num1 = 0;
                    int num2 = 0;
                    if (tabDLP.SelectedTab.Name == "tabBasic") MyActivePage = 2;
                    else if (tabDLP.SelectedTab.Name == "tabKeys")
                    {
                        MyActivePage = 1;
                        num1 = 0;
                        num2 = DeviceTypeList.GetMaxValueFromPublicModeGroup(mywdDevicerType);
                    }
                    else if (tabDLP.SelectedTab.Name == "tabChns") MyActivePage = 6;
                    else if (tabDLP.SelectedTab.Name == "tabDimmer") MyActivePage = 7;
                    else if (tabDLP.SelectedTab.Name == "tabOther")
                    {
                        MyActivePage = 8;
                        UpdateColorAndSensitivitySetup();
                    }
                    else if (tabDLP.SelectedTab.Name == "tabHeat")
                    {
                        MyActivePage = 5;
                        UpdateFloorheatingSettingToStruct();
                    }
                    byte[] ArayRelay = new byte[] { bytSubID, bytDevID, (byte)(mywdDevicerType / 256), (byte)(mywdDevicerType % 256)
                        , (byte)MyActivePage,(byte)(mintIDIndex/256),(byte)(mintIDIndex%256),(byte)num1,(byte)num2  };
                    CsConst.MyUPload2DownLists.Add(ArayRelay);
                    CsConst.MyUpload2Down = Convert.ToByte(((ToolStripButton)sender).Tag.ToString());
                    FrmDownloadShow Frm = new FrmDownloadShow();
                    if (CsConst.MyUpload2Down == 0) Frm.FormClosing += new FormClosingEventHandler(UpdateDisplayInformationAccordingly);
                    Frm.ShowDialog();
                }
            }
            catch
            {
            }
        }

        void UpdateColorAndSensitivitySetup()
        {
            Cursor.Current = Cursors.WaitCursor;
            oPanel.arayButtonSensitiVity = new byte[dgvOther.RowCount];
            oPanel.arayButtonColor = new byte[dgvOther.RowCount * 6];
            oPanel.arayButtonBalance = new byte[dgvOther.RowCount * 3 + 1];
            for (int i = 0; i < dgvOther.RowCount; i++)
            {
                oPanel.arayButtonColor[i * 6] = dgvOther[1, i].Style.BackColor.R;
                oPanel.arayButtonColor[i * 6 + 1] = dgvOther[1, i].Style.BackColor.G;
                oPanel.arayButtonColor[i * 6 + 2] = dgvOther[1, i].Style.BackColor.B;
                oPanel.arayButtonColor[i * 6 + 3] = dgvOther[2, i].Style.BackColor.R;
                oPanel.arayButtonColor[i * 6 + 4] = dgvOther[2, i].Style.BackColor.G;
                oPanel.arayButtonColor[i * 6 + 5] = dgvOther[2, i].Style.BackColor.B;
                if (clO4.Visible)
                {
                    oPanel.arayButtonSensitiVity[i] = Convert.ToByte(dgvOther[3, i].Value.ToString());
                }
            }
            if (gbBalance.Visible)
            {
                oPanel.arayButtonBalance[0] = Convert.ToByte(dgvBalance.RowCount);
                for (int i = 0; i < dgvBalance.RowCount; i++)
                {
                    oPanel.arayButtonBalance[i * 3 + 1] = Convert.ToByte(dgvBalance[1, i].Value.ToString());
                    oPanel.arayButtonBalance[i * 3 + 2] = Convert.ToByte(dgvBalance[2, i].Value.ToString());
                    oPanel.arayButtonBalance[i * 3 + 3] = Convert.ToByte(dgvBalance[3, i].Value.ToString());
                }
            }
        }

        void UpdateDisplayInformationAccordingly(object sender, FormClosingEventArgs e)
        {
            isRead = true;
            if (BlnShowBasicInformation == false)
            {
                DisplayBasicDLPInformationsToForm();
                BlnShowBasicInformation = true;
            }

            switch (tabDLP.SelectedTab.Name)
            {
                case "tabBasic":
                    //基本信息
                    DisplayBasicDLPInformationsToForm(); break;
                //显示目标信息
                case "tabKeys": ShowKeysInformationPanel(); break;
                case "tabChns": showChannelInfo(); break;
                case "tabHeat": ShowFHInformation(); break;
                //显示音乐播放目标
                case "tabDimmer": showDimmingInfo(); break;
                case "tabOther": ShowSensitivityAndColorSetup(); break;
            }
            Cursor.Current = Cursors.Default;
            this.BringToFront();
            isRead = false;
        }

        private void ShowFHInformation()
        {
            try
            {
                isRead = true;
                FloorHeating temp = oPanel.DLPFH;
                if (temp.HeatType <= 2) cbHeatType.SelectedIndex = temp.HeatType + 1;
                else cbHeatType.SelectedIndex = 0;
                if (temp.ControlMode <= 1) cbControl.SelectedIndex = temp.ControlMode;
                else cbControl.SelectedIndex = 0;
                cbControl_SelectedIndexChanged(null, null);
                TemperatureModeForFH();
                if (temp.SourceTemp == 0) rbHeat1.Checked = true;
                else if (temp.SourceTemp == 1) rbHeat2.Checked = true;
                else if (temp.SourceTemp == 2) rbHeat3.Checked = true;
                if (rbHeat1.Checked == false)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        ComboBox tmpCbo = grpHeatTemp.Controls.Find("cbHeatSensor" + (i + 1).ToString(), true)[0] as ComboBox;
                        if (temp != null) tmpCbo.SelectedIndex = temp.OutDoorParam[i * 4 + 0];

                        NumericUpDown tmpNum = grpHeatTemp.Controls.Find("NumSub" + (i + 1).ToString(), true)[0] as NumericUpDown;
                        tmpNum.Value = Convert.ToDecimal(temp.OutDoorParam[i * 4 + 1]);

                        tmpNum = grpHeatTemp.Controls.Find("NumDev" + (i + 1).ToString(), true)[0] as NumericUpDown;
                        tmpNum.Value = Convert.ToDecimal(temp.OutDoorParam[i * 4 + 2]);

                        tmpNum = grpHeatTemp.Controls.Find("NumChn" + (i + 1).ToString(), true)[0] as NumericUpDown;
                        tmpNum.Value = Convert.ToDecimal(temp.OutDoorParam[i * 4 + 3]);
                    }
                }
                if (temp.SourceParam[0] <= (cbHeatSensor3.Items.Count - 1)) cbHeatSensor3.SelectedIndex = temp.SourceParam[0];
                else cbHeatSensor3.SelectedIndex = 0;
                NumSub3.Value = Convert.ToDecimal(temp.SourceParam[1]);
                NumDev3.Value = Convert.ToDecimal(temp.SourceParam[2]);
                NumChn3.Value = Convert.ToDecimal(temp.SourceParam[3]);
                if (temp.SourceParam[4] <= (cbHeatSensor4.Items.Count - 1)) cbHeatSensor4.SelectedIndex = temp.SourceParam[4];
                else cbHeatSensor4.SelectedIndex = 0;
                NumSub4.Value = Convert.ToDecimal(temp.SourceParam[5]);
                NumDev4.Value = Convert.ToDecimal(temp.SourceParam[6]);
                NumChn4.Value = Convert.ToDecimal(temp.SourceParam[7]);
                if (oPanel.bytTempType == 0)
                    cbMaxTemp.SelectedIndex = cbMaxTemp.Items.IndexOf(temp.ProtectTemperature.ToString() + "C");
                else if (oPanel.bytTempType == 1)
                    cbMaxTemp.SelectedIndex = cbMaxTemp.Items.IndexOf(temp.ProtectTemperature.ToString() + "F");
                if (cbMaxTemp.SelectedIndex < 0) cbMaxTemp.SelectedIndex = 0;
                if (temp.PIDEnable == 1) chbHeat2.Checked = true;
                else chbHeat2.Checked = false;

                if (temp.Switch == 1) chbHeat1.Checked = true;
                else chbHeat1.Checked = false;
                if (temp.SysEnable[0] == 1)
                    chbHeat3.Checked = true;
                else
                    chbHeat3.Checked = false;

                if (temp.SysEnable[1] == 1)
                    chbHeat4.Checked = true;
                else
                    chbHeat4.Checked = false;

                if (temp.SysEnable[2] == 1)
                    chbHeat5.Checked = true;
                else
                    chbHeat5.Checked = false;

                if (temp.SysEnable[3] == 1)
                    chbHeat6.Checked = true;
                else
                    chbHeat6.Checked = false;
                if (temp.OutputType <= 1) cbOutput.SelectedIndex = temp.OutputType;
                else cbOutput.SelectedIndex = 0;
                if (temp.Cycle < cbCycle.Items.Count) cbCycle.SelectedIndex = temp.Cycle;
                else cbCycle.SelectedIndex = 0;
                if (temp.minPWM <= 100) cbMinPWM.SelectedIndex = temp.minPWM;
                else cbMinPWM.SelectedIndex = 0;
                if (temp.maxPWM <= 100) cbMaxPWM.SelectedIndex = temp.maxPWM;
                else cbMaxPWM.SelectedIndex = 0;
                if (temp.Speed <= 4) cbHeatSpeed.SelectedIndex = temp.Speed;
                else cbHeatSpeed.SelectedIndex = 0;
                if (temp.TimeAry[0] <= 23) numStart1.Value = Convert.ToDecimal(temp.TimeAry[0]);
                if (temp.TimeAry[1] <= 59) numStart2.Value = Convert.ToDecimal(temp.TimeAry[1]);
                if (temp.TimeAry[2] <= 23) numEnd1.Value = Convert.ToDecimal(temp.TimeAry[2]);
                if (temp.TimeAry[3] <= 59) numEnd2.Value = Convert.ToDecimal(temp.TimeAry[3]);
                for (int j = 0; j < 5; j++)
                    chbListHeatMode.SetItemChecked(j, temp.ModeAry[j] == 1);
                if (1 <= temp.CompenValue && temp.CompenValue <= 5) sbModeChaneover.Value = temp.CompenValue;
                if (oPanel.bytTempType == 0)
                    cbHeatMinTemp.SelectedIndex = cbHeatMinTemp.Items.IndexOf(temp.minTemp.ToString() + "C");
                else if (oPanel.bytTempType == 1)
                    cbHeatMinTemp.SelectedIndex = cbHeatMinTemp.Items.IndexOf(temp.minTemp.ToString() + "F");
                if (oPanel.bytTempType == 0)
                    cbHeatMaxTemp.SelectedIndex = cbHeatMaxTemp.Items.IndexOf(temp.maxTemp.ToString() + "C");
                else if (oPanel.bytTempType == 1)
                    cbHeatMaxTemp.SelectedIndex = cbHeatMaxTemp.Items.IndexOf(temp.maxTemp.ToString() + "F");
                if (cbHeatMinTemp.SelectedIndex < 0) cbHeatMinTemp.SelectedIndex = 0;
                if (cbHeatMaxTemp.SelectedIndex < 0) cbHeatMaxTemp.SelectedIndex = 0;

                DisplayCurrentStatusFloorheating();
                isRead = false;
                sbHeatTemp1_ValueChanged(null, null);
                sbHeatTemp2_ValueChanged(null, null);
                sbHeatTemp3_ValueChanged(null, null);
                sbHeatTemp4_ValueChanged(null, null);
            }
            catch
            {
            }
        }

        private void TemperatureModeForFH()
        {
            if (oPanel.bytTempType == 0)
            {
                cbHeatMinTemp.Items.Clear();
                cbHeatMaxTemp.Items.Clear();
                cbMaxTemp.Items.Clear();
                for (int i = 5; i <= 35; i++)
                {
                    cbHeatMinTemp.Items.Add(i.ToString() + "C");
                    cbHeatMaxTemp.Items.Add(i.ToString() + "C");

                }
                for (int i = 5; i <= 80; i++)
                {
                    cbMaxTemp.Items.Add(i.ToString() + "C");
                }
                if (mywdDevicerType == 180 || mywdDevicerType == 5004)
                {
                    cbMaxTemp.Items.Clear();
                    for (int i = 5; i <= 99; i++)
                    {
                        cbMaxTemp.Items.Add(i.ToString() + "C");
                    }
                }
                cbHeatMinTemp.SelectedIndex = 0;
                cbHeatMaxTemp.SelectedIndex = 0;
                cbMaxTemp.SelectedIndex = 0;
                for (int i = 1; i <= 4; i++)
                {
                    HScrollBar temp = this.Controls.Find("sbHeatTemp" + i.ToString(), true)[0] as HScrollBar;
                    temp.Minimum = 5;
                    temp.Maximum = 35;
                    if (mywdDevicerType == 180 || mywdDevicerType == 5004)
                        temp.Maximum = 99;
                    temp.Value = 5;
                }
            }
            else if (oPanel.bytTempType == 1)
            {
                cbHeatMinTemp.Items.Clear();
                cbHeatMaxTemp.Items.Clear();
                cbMaxTemp.Items.Clear();
                for (int i = 41; i <= 95; i++)
                {
                    cbHeatMinTemp.Items.Add(i.ToString() + "F");
                    cbHeatMaxTemp.Items.Add(i.ToString() + "F");
                }
                for (int i = 41; i <= 176; i++)
                {
                    cbMaxTemp.Items.Add(i.ToString() + "F");
                }
                cbHeatMinTemp.SelectedIndex = 0;
                cbHeatMaxTemp.SelectedIndex = 0;
                cbMaxTemp.SelectedIndex = 0;
                for (int i = 1; i <= 4; i++)
                {
                    HScrollBar temp = this.Controls.Find("sbHeatTemp" + i.ToString(), true)[0] as HScrollBar;
                    temp.Minimum = 41;
                    temp.Maximum = 95;
                    temp.Value = 41;
                }
            }
        }

        void DisplayCurrentStatusFloorheating()
        {
            try
            {
                FloorHeating temp = oPanel.DLPFH;
                if (temp.WorkingSwitch == 1) chbHeatSwitch.Checked = true;
                else chbHeatSwitch.Checked = false;
                if (oPanel.bytTempType == 0)
                {
                    if (HDLSysPF.GetBit(temp.CurrentTemp, 7) == 1)
                        lbHeatCurrentTempValue.Text = "-" + ((temp.CurrentTemp & (byte.MaxValue - (1 << 7)))).ToString() + "C";
                    else
                        lbHeatCurrentTempValue.Text = temp.CurrentTemp.ToString() + "C";
                }
                else if (oPanel.bytTempType == 1)
                {
                    if (HDLSysPF.GetBit(temp.CurrentTemp, 7) == 1)
                        lbHeatCurrentTempValue.Text = "-" + ((temp.CurrentTemp & (byte.MaxValue - (1 << 7)))).ToString() + "F";
                    else
                        lbHeatCurrentTempValue.Text = temp.CurrentTemp.ToString() + "F";
                }
                if (1 <= temp.WorkingTempMode && temp.WorkingTempMode <= 5) cbCurrentMode.SelectedIndex = temp.WorkingTempMode - 1;
                else cbCurrentMode.SelectedIndex = 0;
                if (sbHeatTemp1.Minimum <= temp.ModeTemp[0] && temp.ModeTemp[0] <= sbHeatTemp1.Maximum) sbHeatTemp1.Value = Convert.ToInt32(temp.ModeTemp[0]);
                if (sbHeatTemp2.Minimum <= temp.ModeTemp[1] && temp.ModeTemp[1] <= sbHeatTemp2.Maximum) sbHeatTemp2.Value = Convert.ToInt32(temp.ModeTemp[1]);
                if (sbHeatTemp3.Minimum <= temp.ModeTemp[2] && temp.ModeTemp[2] <= sbHeatTemp3.Maximum) sbHeatTemp3.Value = Convert.ToInt32(temp.ModeTemp[2]);
                if (sbHeatTemp4.Minimum <= temp.ModeTemp[3] && temp.ModeTemp[3] <= sbHeatTemp4.Maximum) sbHeatTemp4.Value = Convert.ToInt32(temp.ModeTemp[3]);
            }
            catch { }
        }

        private void ShowSensitivityAndColorSetup()
        {
            try
            {
                setAllVisible(false);
                int wdMaxValue = DeviceTypeList.GetMaxValueFromPublicModeGroup(mywdDevicerType);
                dgvOther.Rows.Clear();
                for (int i = 1; i <= wdMaxValue; i++)
                {
                    object[] obj = new object[] { i.ToString(), "", "", oPanel.arayButtonSensitiVity[i - 1] };
                    dgvOther.Rows.Add(obj);
                }
                
                for (int i = 0; i < dgvOther.RowCount; i++)
                {
                    dgvOther[1, i].Style.BackColor = Color.FromArgb(oPanel.arayButtonColor[i * 6], oPanel.arayButtonColor[i * 6 + 1], oPanel.arayButtonColor[i * 6 + 2]);
                    dgvOther[2, i].Style.BackColor = Color.FromArgb(oPanel.arayButtonColor[i * 6 + 3], oPanel.arayButtonColor[i * 6 + 4], oPanel.arayButtonColor[i * 6 + 5]);
                }
                gbBalance.Visible = (oPanel.arayButtonBalance[0] > 0);
                if (oPanel.arayButtonBalance[0] > 0)
                {
                    dgvBalance.Rows.Clear();
                    for (int i = 0; i < oPanel.arayButtonBalance[0]; i++)
                    {
                        object[] obj = new object[] { dgvBalance.RowCount+1,oPanel.arayButtonBalance[i*3+1],
                                         oPanel.arayButtonBalance[i*3+2],oPanel.arayButtonBalance[i*3+3]};
                        dgvBalance.Rows.Add(obj);
                    }
                }
                DisplayBackgroudColor(0);
            }
            catch
            {
            }
            isRead = false;
        }

        private void showDimmingInfo()
        {
            try
            {
                dgvAddress.Rows.Clear();
                if (oPanel.bytWirelessConnectInfo.Length >= 28)
                {
                    int ConnectedCount = 0;
                    for (int i = 0; i < 4; i++)
                    {
                        string strState = "";
                        string strConnection = "";
                        string strAddressMac = "";
                        if (oPanel.bytWirelessConnectInfo[i * 6] == 0)
                        {
                            strState = CsConst.strWirelessAddressState[0];
                        }
                        else if (oPanel.bytWirelessConnectInfo[i * 6] == 1)
                        {
                            strState = CsConst.strWirelessAddressState[1];
                        }
                        strAddressMac = GlobalClass.AddLeftZero(oPanel.bytWirelessConnectInfo[i * 6 + 1].ToString("X"), 2) + "."
                                        + GlobalClass.AddLeftZero(oPanel.bytWirelessConnectInfo[i * 6 + 2].ToString("X"), 2) + "."
                                        + GlobalClass.AddLeftZero(oPanel.bytWirelessConnectInfo[i * 6 + 3].ToString("X"), 2) + "."
                                        + GlobalClass.AddLeftZero(oPanel.bytWirelessConnectInfo[i * 6 + 4].ToString("X"), 2) + "."
                                        + GlobalClass.AddLeftZero(oPanel.bytWirelessConnectInfo[i * 6 + 5].ToString("X"), 2) + ".";
                        if (oPanel.bytWirelessConnectInfo[24 + i] == 1)
                        {
                            strConnection = CsConst.strWirelessAddressConnection[1];
                            ConnectedCount = ConnectedCount + 1;
                        }
                        else
                        {
                            strConnection = CsConst.strWirelessAddressConnection[0];
                        }
                        //底座类型和回路
                        string str2 = "N/A";
                        string str3 = "N/A";
                        #region
                        if (oPanel.BaseInfomation != null && oPanel.BaseInfomation[oPanel.BaseInfomation.Length - 1] == 1)
                        {
                            if (oPanel.BaseInfomation.Length >= 17)
                            {
                                str2 = "N/A";
                                str3 = "N/A";

                                if (1 <= oPanel.BaseInfomation[i * 4 + 1] && oPanel.BaseInfomation[i * 4 + 1] <= 4)
                                {
                                    str2 = CsConst.mstrINIDefault.IniReadValue("Public", "0021" + (oPanel.BaseInfomation[i * 4 + 1] - 1).ToString(), "");
                                    str3 = oPanel.BaseInfomation[i * 4 + 3].ToString();
                                }
                            }
                        }
                        #endregion
                        object[] obj = new object[] { dgvAddress.RowCount + 1, strState, strConnection, strAddressMac,str2,str3 };
                        dgvAddress.Rows.Add(obj);
                    }
                    lbCountValue.Text = ConnectedCount.ToString();
                }
                hbTemp.Value = Convert.ToInt32(oPanel.bytChn[1]);
                lbSaveDimming.Visible = (oPanel.bytChn[0] == 2);
                cbBaseDimming.Visible = (oPanel.bytChn[0] == 2);
                cbBaseDimming.SelectedIndex = oPanel.bytChn[2];
               
                groupBox7.Visible = (oPanel.BaseTempInfomation != null && oPanel.BaseTempInfomation[oPanel.BaseTempInfomation.Length - 1] == 1);
                if (groupBox7.Visible)
                {
                    dgvTemp.Rows.Clear();
                    if (oPanel.BaseTempInfomation.Length >= 24)
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            string str1 = CsConst.mstrINIDefault.IniReadValue("Public", "99826", "");
                            string strTemp = oPanel.BaseTempInfomation[i * 6 + 5].ToString();
                            if (oPanel.BaseTempInfomation[i * 6 + 5] > 128) strTemp = "-" + (oPanel.BaseTempInfomation[i * 6 + 5] - 128).ToString();
                            if (oPanel.BaseTempInfomation[i * 6 + 1] == 1) str1 = CsConst.mstrINIDefault.IniReadValue("Public", "99827", "");
                            object[] obj = new object[] { dgvTemp.RowCount+1,str1,oPanel.BaseTempInfomation[i*6+2].ToString(),
                                            oPanel.BaseTempInfomation[i*6+3].ToString(),(oPanel.BaseTempInfomation[i*6+4]-10).ToString(),
                                            strTemp};
                            dgvTemp.Rows.Add(obj);
                        }
                    }
                }
            }
            catch
            {
            }
            isRead = false;
        }

        private void showChannelInfo()
        {
            try
            {
                DgChns.Rows.Clear();
                foreach (Channel ch in oPanel.myPanelDim)
                {
                    object[] boj = new object[] { ch.ID, ch.Remark, cl32.Items[ch.LoadType], ch.MinValue, ch.MaxLevel, 0, ch.MaxLevel,false };
                    DgChns.Rows.Add(boj);
                }
            }
            catch
            {
            }
            isRead = false;
        }

        private void DisplayBasicDLPInformationsToForm()
        {
            try
            {
                if (oPanel == null) return;

                //基本信息的现在
                if (oPanel.otherInfo == null)
                {
                    oPanel.otherInfo = new Panel.OtherInfo();
                    if (oPanel.otherInfo.bytAryShowPage == null) oPanel.otherInfo.bytAryShowPage = new byte[7];
                }

                if (gpLed.Visible == true)
                {
                    if (Convert.ToInt32(oPanel.Backlight) <= sb1.Maximum)
                        sb1.Value = oPanel.Backlight;
                    if (Convert.ToInt32(oPanel.Ledlight) <= sb2.Maximum)
                        sb2.Value = oPanel.Ledlight;
                }               

                if (Convert.ToInt32(oPanel.LlimitDimmer) <= sb4.Maximum)
                    sb4.Value = oPanel.LlimitDimmer;

                if (3 <= oPanel.LTime && oPanel.LTime <= 250)
                {
                    numL1.Value = Convert.ToDecimal(oPanel.LTime / 10);
                    numL2.Value = Convert.ToDecimal(oPanel.LTime % 10);
                }

                if (1 <= oPanel.DTime && oPanel.DTime <= 20)
                {
                    numD1.Value = Convert.ToDecimal(oPanel.DTime / 10);
                    numD2.Value = Convert.ToDecimal(oPanel.DTime % 10);
                }

                if (gbTemperature.Visible == true)
                {
                    chbBroadCast.Checked = (oPanel.arayTempBroadCast[1] == 1);
                    txtBroadSub.Text = oPanel.arayTempBroadCast[2].ToString();
                    txtBroadDev.Text = oPanel.arayTempBroadCast[3].ToString();
                    if (oPanel.arayTempBroadCast[4] >= sbAdjust.Minimum && oPanel.arayTempBroadCast[4] <= sbAdjust.Maximum)
                        sbAdjust.Value = Convert.ToInt32(oPanel.arayTempBroadCast[4]);
                    if (HDLSysPF.GetBit(oPanel.arayTempBroadCast[5], 7) == 1)
                        lbCurTempValue.Text = "-" + ((oPanel.arayTempBroadCast[5] & (byte.MaxValue - (1 << 7)))).ToString() + "C";
                    else
                        lbCurTempValue.Text = oPanel.arayTempBroadCast[5].ToString() + "C";
                    sbAdjust_ValueChanged(null, null);
                }

                #region
                if (gbLcd.Visible)
                {
                    if (Convert.ToInt32(oPanel.otherInfo.bytBacklight) <= sb3.Maximum)
                        sb3.Value = oPanel.otherInfo.bytBacklight;
                    sb3_ValueChanged(sb3, null);
                    
                    if (oPanel.otherInfo.bytBacklightTime == 0 || oPanel.otherInfo.bytBacklightTime == 100)
                        rb3.Checked = true;
                    else
                    {
                        if (Convert.ToInt32(oPanel.otherInfo.bytBacklightTime) >= Convert.ToInt32(tbLCDelay.Minimum) &&
                            Convert.ToInt32(oPanel.otherInfo.bytBacklightTime) <= Convert.ToInt32(tbLCDelay.Maximum))
                            tbLCDelay.Value = oPanel.otherInfo.bytBacklightTime;
                        rb4.Checked = true;
                    }
                    chbAutoLock.Checked = (oPanel.otherInfo.AutoLock == 1);
                    chbPWM.Checked = (oPanel.LcdDisplayFlag == 1);
                    txtSwitch.Text = oPanel.otherInfo.bytAryShowPage[0].ToString();
                    for (byte bytI = 1; bytI < 6; bytI++)
                    {
                        cbPages.SetItemChecked(bytI - 1, (oPanel.otherInfo.bytAryShowPage[bytI] == 1));
                    }
               }
                #endregion
            }
            catch
            {
            }
            chbIR.Checked = (oPanel.IRON == 0);
            chbLock.Checked = (oPanel.LockKeys == 1);
            isRead = false;
        }

        private void frmPanel_Shown(object sender, EventArgs e)
        {
            SetCtrlsVisbleWithDifferentDeviceType();
            if (CsConst.MyEditMode == 0)
            {

            }
            else if (CsConst.MyEditMode == 1)
            {
                if (oPanel.MyRead2UpFlags[1] == false)
                {
                    tsbDown_Click(tsbDown, null);
                }
                else
                {
                    UpdateDisplayInformationAccordingly(null,null);
                }
            }
            
        }

        private void tabDLP_SelectedIndexChanged(object sender, EventArgs e)
        {
            isRead = true;
            if (tabDLP.SelectedTab.Name == "tabBasic") MyActivePage = 2;
            else if (tabDLP.SelectedTab.Name == "tabKeys") MyActivePage = 1;
            else if (tabDLP.SelectedTab.Name == "tabChns") MyActivePage = 6;
            else if (tabDLP.SelectedTab.Name == "tabDimmer") MyActivePage = 7;
            else if (tabDLP.SelectedTab.Name == "tabOther") MyActivePage = 8;
            else if (tabDLP.SelectedTab.Name == "tabHeat") MyActivePage = 5;
            if (CsConst.MyEditMode == 1)
            {
                if (oPanel.MyRead2UpFlags[MyActivePage - 1] == false)
                {
                    tsbDown_Click(tsbDown, null);
                }
                else
                {
                    UpdateDisplayInformationAccordingly(null, null);
                }
            }
        }

        private void devName1_Load(object sender, EventArgs e)
        {

        }

        private void hbTemp_ValueChanged(object sender, EventArgs e)
        {
            lbProTmp.Text = hbTemp.Value.ToString();
            if (isRead) return;
            oPanel.bytChn[1] = Convert.ToByte(hbTemp.Value);
        }

        private void btnSaveBasic_Click(object sender, EventArgs e)
        {
            tsbDown_Click(toolStripLabel2, null);
        }

        private void sb1_ValueChanged(object sender, EventArgs e)
        {
            lbv1.Text = sb1.Value.ToString();
            if (isRead) return;
            if (oPanel == null) return;
            oPanel.Backlight = byte.Parse(sb1.Value.ToString());
        }

        private void sb2_ValueChanged(object sender, EventArgs e)
        {
            lbv2.Text = sb2.Value.ToString();
            if (isRead) return;
            if (oPanel == null) return;
            oPanel.Ledlight = byte.Parse(sb2.Value.ToString());
        }

        private void sb4_ValueChanged(object sender, EventArgs e)
        {
            lbv4.Text = sb4.Value.ToString();
            if (isRead) return;
            if (oPanel == null) return;
            oPanel.LlimitDimmer = byte.Parse(sb4.Value.ToString());
        }

        private void numL1_ValueChanged(object sender, EventArgs e)
        {
            if ((Convert.ToInt32(numL1.Value) == 0) && (Convert.ToInt32(numL2.Value) < 3)) numL2.Value = 3;
            if (Convert.ToInt32(numL1.Value) == 25) numL2.Value = 0;
            if (isRead) return;
            if (oPanel == null) return;
            oPanel.LTime = Convert.ToByte(Convert.ToInt32(numL1.Value) * 10 + Convert.ToInt32(numL2.Value));
        }

        private void numD1_ValueChanged(object sender, EventArgs e)
        {
            if ((Convert.ToInt32(numD1.Value) == 0) && (Convert.ToInt32(numD2.Value) < 1)) numD2.Value = 1;
            if (Convert.ToInt32(numD1.Value) == 2) numD2.Value = 0;
            if (isRead) return;
            oPanel.DTime = Convert.ToByte(Convert.ToInt32(numD1.Value) * 10 + Convert.ToInt32(numD2.Value));
        }

        private void chbIR_CheckedChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            if (oPanel == null) return;
            if (chbIR.Checked == true) oPanel.IRON = 0;
            else oPanel.IRON = 1;
        }

        private void chbLock_CheckedChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            if (oPanel == null) return;
            if (chbLock.Checked == true) oPanel.LockKeys = 1;
            else oPanel.LockKeys = 0;
        }

        private void btnSaveKey_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvKey.RowCount <= 0) return;
                if (oPanel == null) return;
                for (int i = 0; i < dgvKey.RowCount; i++)
                {
                    oPanel.PanelKey[i].Remark = dgvKey[1, i].Value.ToString();
                    string strMod = dgvKey[2, i].Value.ToString();

                    oPanel.PanelKey[i].Mode = ButtonMode.ConvertorKeyModesToPublicModeGroup(strMod);

                    string strDimming = dgvKey[3, i].Value.ToString();
                    oPanel.PanelKey[i].IsDimmer = Convert.ToByte(cbKeyDim.Items.IndexOf(strDimming));
                    string strSaveDimming = dgvKey[4, i].Value.ToString();
                    oPanel.PanelKey[i].SaveDimmer = Convert.ToByte(cbKeySaveDim.Items.IndexOf(strSaveDimming));
                    string strKeyLED = dgvKey[5, i].Value.ToString();
                    if (strKeyLED == cbKeyLED.Items[0].ToString())
                        oPanel.PanelKey[i].IsLEDON = 1;
                    else
                        oPanel.PanelKey[i].IsLEDON = 0;
                    if (oPanel.PanelKey[i].Mode == 4 ||
                        oPanel.PanelKey[i].Mode == 5 ||
                        oPanel.PanelKey[i].Mode == 7)
                    {
                        string strMutex = dgvKey[6, i].Value.ToString();
                        oPanel.PanelKey[i].bytMutex = Convert.ToByte(cbKeyMutex.Items.IndexOf(strMutex));
                    }
                    else if (2 <= oPanel.PanelKey[i].Mode && oPanel.PanelKey[i].Mode <= 5)
                    {
                        string strLink= dgvKey[7, i].Value.ToString();
                        oPanel.PanelKey[i].byteLink = Convert.ToByte(cbKeylink.Items.IndexOf(strLink));
                    }
                }
            }
            catch
            {
            }
            tsbDown_Click(toolStripLabel2, null);
        }

        private void btnTargets_Click(object sender, EventArgs e)
        {

        }

        private void DgChns_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (oPanel == null) return;
                if ((e.RowIndex == -1) || (e.ColumnIndex == -1)) return;
                if (DgChns[e.ColumnIndex, e.RowIndex].Value == null) DgChns[e.ColumnIndex, e.RowIndex].Value = "";
                for (int i = 0; i < DgChns.SelectedRows.Count; i++)
                {
                    int RowID = DgChns.SelectedRows[i].Index;
                    string strTmp = DgChns[e.ColumnIndex, e.RowIndex].Value.ToString();
                    //DgChns[e.ColumnIndex, RowID].Value = strTmp;

                    switch (e.ColumnIndex)
                    {
                        case 1:
                            DgChns[e.ColumnIndex, RowID].Value = strTmp;
                            DgChns[1, RowID].Value = HDLPF.IsRightStringMode(strTmp);
                            oPanel.myPanelDim[RowID].Remark = DgChns[1, RowID].Value.ToString();
                            break;
                        case 2:
                            DgChns[e.ColumnIndex, RowID].Value = strTmp;
                            oPanel.myPanelDim[RowID].LoadType = cl32.Items.IndexOf(strTmp);
                            break;
                        case 3:
                            DgChns[e.ColumnIndex, RowID].Value = strTmp;
                            DgChns[3, RowID].Value = HDLPF.IsNumStringMode(strTmp, 0, 100);
                            oPanel.myPanelDim[RowID].MinValue = int.Parse(DgChns[3, e.RowIndex].Value.ToString());
                            break;
                        case 4:
                            DgChns[e.ColumnIndex, RowID].Value = strTmp;
                            DgChns[4, e.RowIndex].Value = HDLPF.IsNumStringMode(strTmp, 0, 100);
                            oPanel.myPanelDim[RowID].MaxLevel = int.Parse(DgChns[4, e.RowIndex].Value.ToString());
                            break;
                        case 5:
                            OnsiteOutput((Byte)RowID); 
                            break;
                    }
                }
            }
            catch
            {
            }
        }
        delegate void SetvalueHandle(int rowIndex);

        void OnsiteOutput(Byte RowID)
        {
            byte[] bytTmp = new byte[4];
            bytTmp[0] = (byte)(RowID + 1);
            bytTmp[2] = 0;
            bytTmp[3] = 0;


            if (DgChns[5, RowID].Value.ToString().ToLower() == "true")
                bytTmp[1] = (Byte)oPanel.myPanelDim[RowID].MaxLevel;
            else
                bytTmp[1] = 0;

            Cursor.Current = Cursors.WaitCursor;
            if (CsConst.mySends.AddBufToSndList(bytTmp, 0x0031, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(mywdDevicerType)) == false)
            {
            }

            Cursor.Current = Cursors.Default;
        }

        private void Setvalue(int rowIndex)
        {
            DgChns.CellValueChanged -= new DataGridViewCellEventHandler(DgChns_CellValueChanged);
            System.Threading.Thread.Sleep(50);
            List<int> values = new List<int>();

            for (int i = 0; i < DgChns.Rows.Count; i++)
            {
                if (DgChns[6, i].Value.ToString().ToLower() == "true")
                {
                    if (i != rowIndex)
                        values.Add(i);
                }
            }
            DgChns.Rows.Clear();

            foreach (Channel ch in oPanel.myPanelDim)
            {
                object[] boj = new object[] { ch.ID, ch.Remark, cl32.Items[ch.LoadType], ch.MinValue, ch.MaxValue, ch.MaxLevel, false };
                DgChns.Rows.Add(boj);
            }

            for (int i = 0; i < DgChns.Rows.Count; i++)
            {
                if (values.Contains(i))
                {
                    DgChns[6, i].Value = true;
                }
            }
            DgChns[0, 0].Selected = false;
            DgChns[6, rowIndex].Selected = true;

            DgChns.CellValueChanged += new DataGridViewCellEventHandler(DgChns_CellValueChanged);
        }

        private void DgChns_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            DgChns.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void btnSaveChnInfo_Click(object sender, EventArgs e)
        {
           
        }

        private void btnSaveDim_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            setAllVisible(false);
            try
            {
                if (oPanel.BaseTempInfomation == null) oPanel.BaseTempInfomation = new byte[25];
                if (groupBox7.Visible)
                {
                    for (int i = 0; i < dgvTemp.Rows.Count; i++)
                    {
                        oPanel.BaseTempInfomation[i * 6] = Convert.ToByte(i + 2);
                        string str1 = dgvTemp[1, i].Value.ToString();
                        string str2 = dgvTemp[2, i].Value.ToString();
                        string str3 = dgvTemp[3, i].Value.ToString();
                        string str4 = dgvTemp[4, i].Value.ToString();
                        if (str1 == CsConst.mstrINIDefault.IniReadValue("Public", "99827", ""))
                            oPanel.BaseTempInfomation[i * 6 + 1] = 1;
                        else
                            oPanel.BaseTempInfomation[i * 6 + 1] = 0;


                        oPanel.BaseTempInfomation[i * 6 + 2] = Convert.ToByte(str2);
                        oPanel.BaseTempInfomation[i * 6 + 3] = Convert.ToByte(str3);
                        oPanel.BaseTempInfomation[i * 6 + 4] = Convert.ToByte(Convert.ToInt32(str4) + 10);
                    }
                }
            }
            catch
            {
            }
            tsbDown_Click(toolStripLabel2, null);
        }

        private void sbAdjust_ValueChanged(object sender, EventArgs e)
        {
            lbAdjust.Text = (Convert.ToInt32(sbAdjust.Value) - 10).ToString();
            if (isRead) return;
            if (oPanel == null) return;
            if (oPanel.arayTempBroadCast == null || oPanel.arayTempBroadCast.Length < 6) return;
            oPanel.arayTempBroadCast[4] = Convert.ToByte(sbAdjust.Value);
        }

        private void chbBroadCast_CheckedChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            if (oPanel == null) return;
            if (oPanel.arayTempBroadCast == null || oPanel.arayTempBroadCast.Length < 6) return;
            if (chbBroadCast.Checked) oPanel.arayTempBroadCast[1] = 1;
            else oPanel.arayTempBroadCast[1] = 0;
        }

        private void txtBroadSub_TextChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            if (oPanel == null) return;
            if (oPanel.arayTempBroadCast == null || oPanel.arayTempBroadCast.Length < 6) return;
            oPanel.arayTempBroadCast[2] = Convert.ToByte(txtBroadSub.Text);
            oPanel.arayTempBroadCast[3] = Convert.ToByte(txtBroadDev.Text);
        }

        private void txtBroadSub_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar) || e.KeyChar == 8)
                e.Handled = false;
            else
                e.Handled = true;
        }

        private void txtBroadDev_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar) || e.KeyChar == 8)
                e.Handled = false;
            else
                e.Handled = true;
        }

        private void tmRead_Click(object sender, EventArgs e)
        {
            

        }

        private void btnSaveOther_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            oPanel.arayButtonSensitiVity = new byte[dgvOther.RowCount];
            oPanel.arayButtonColor = new byte[dgvOther.RowCount * 6];
            oPanel.arayButtonBalance = new byte[dgvOther.RowCount * 3 + 1];
            for (int i = 0; i < dgvOther.RowCount; i++)
            {
                oPanel.arayButtonColor[i * 6] = dgvOther[1, i].Style.BackColor.R;
                oPanel.arayButtonColor[i * 6 + 1] = dgvOther[1, i].Style.BackColor.G;
                oPanel.arayButtonColor[i * 6 + 2] = dgvOther[1, i].Style.BackColor.B;
                oPanel.arayButtonColor[i * 6 + 3] = dgvOther[2, i].Style.BackColor.R;
                oPanel.arayButtonColor[i * 6 + 4] = dgvOther[2, i].Style.BackColor.G;
                oPanel.arayButtonColor[i * 6 + 5] = dgvOther[2, i].Style.BackColor.B;
                if (clO4.Visible)
                {
                    oPanel.arayButtonSensitiVity[i] = Convert.ToByte(dgvOther[3, i].Value.ToString());
                }
            }
            if (gbBalance.Visible)
            {
                oPanel.arayButtonBalance[0] = Convert.ToByte(dgvBalance.RowCount);
                for (int i = 0; i < dgvBalance.RowCount; i++)
                {
                    oPanel.arayButtonBalance[i * 3 + 1] = Convert.ToByte(dgvBalance[1, i].Value.ToString());
                    oPanel.arayButtonBalance[i * 3 + 2] = Convert.ToByte(dgvBalance[2, i].Value.ToString());
                    oPanel.arayButtonBalance[i * 3 + 3] = Convert.ToByte(dgvBalance[3, i].Value.ToString());
                }
            }
            tsbDown_Click(toolStripLabel2, null);
        }

        private void dgvOther_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                setAllVisible(false);
                if (e.RowIndex >= 0)
                {
                    DisplayBackgroudColor(e.RowIndex);
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

        void DisplayBackgroudColor(int RowIndex)
        {
            pnlOn.BackColor = dgvOther[1, RowIndex].Style.BackColor;
            addcontrols(1, RowIndex, pnlOn, dgvOther);
            pnlOn.Width = pnlOn.Width - 1;
            pnlOn.Height = pnlOn.Height - 1;
            pnlOff.BackColor = dgvOther[2, RowIndex].Style.BackColor;
            addcontrols(2, RowIndex, pnlOff, dgvOther);
            pnlOff.Width = pnlOff.Width - 1;
            pnlOff.Height = pnlOff.Height - 1;
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

        private void chbHeating_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void cbHeatType_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnMoreFh.Enabled = (cbHeatType.SelectedIndex != 0);
            grpHeatTemp.Enabled = btnMoreFh.Enabled;
            grpStatus.Enabled = btnMoreFh.Enabled;
            grpWorking.Enabled = btnMoreFh.Enabled;
        }

        private void cbControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnMoreFh.Visible = (cbControl.SelectedIndex == 1);
            btnRelay.Visible = (cbControl.SelectedIndex == 1);
            btnSlaves.Visible = (cbControl.SelectedIndex == 0);
            if (cbControl.SelectedIndex == 0)
            {
                grpHeatTemp.Enabled = false;
                grpWorking.Enabled = false;

            }
            else if (cbControl.SelectedIndex == 1)
            {
                grpHeatTemp.Enabled = true;
                grpWorking.Enabled = true;
            }
        }

        private void btnAdvance_Click(object sender, EventArgs e)
        {

        }

        private void btnFHTargets_Click(object sender, EventArgs e)
        {
        }

        private void rbHeat1_CheckedChanged(object sender, EventArgs e)
        {
            Boolean blnEnable = !(rbHeat1.Checked);

            try
            {
                for (int i = 0; i < 2; i++)
                {
                    ComboBox tmpCbo = grpHeatTemp.Controls.Find("cbHeatSensor" + (i + 1).ToString(), true)[0] as ComboBox;
                    if (tmpCbo != null) tmpCbo.Visible = blnEnable;

                    NumericUpDown tmpNum = grpHeatTemp.Controls.Find("NumSub" + (i + 1).ToString(), true)[0] as NumericUpDown;
                    tmpNum.Visible = blnEnable;

                    tmpNum = grpHeatTemp.Controls.Find("NumDev" + (i + 1).ToString(), true)[0] as NumericUpDown;
                    tmpNum.Visible = blnEnable;

                    tmpNum = grpHeatTemp.Controls.Find("NumChn" + (i + 1).ToString(), true)[0] as NumericUpDown;
                    tmpNum.Visible = blnEnable;
                }
            }
            catch
            { }
        }

        private void UpdateFloorheatingSettingToStruct()
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                //basic information of setting
                #region
                if (cbHeatType.SelectedIndex != 0) oPanel.DLPFH.HeatEnable = 1;
                else oPanel.DLPFH.HeatEnable = 0;

                if (rbHeat1.Checked) oPanel.DLPFH.SourceTemp = 0;
                else if (rbHeat2.Checked) oPanel.DLPFH.SourceTemp = 1;
                else if (rbHeat3.Checked) oPanel.DLPFH.SourceTemp = 2;

                oPanel.DLPFH.OutDoorParam = new Byte[8];
                if (cbHeatSensor1.SelectedIndex == -1) cbHeatSensor1.SelectedIndex = 0;
                oPanel.DLPFH.OutDoorParam[0] = Convert.ToByte(cbHeatSensor1.SelectedIndex);
                oPanel.DLPFH.OutDoorParam[1] = Convert.ToByte(NumSub1.Value);
                oPanel.DLPFH.OutDoorParam[2] = Convert.ToByte(NumDev1.Value);
                oPanel.DLPFH.OutDoorParam[3] = Convert.ToByte(NumChn1.Value);

                if (cbHeatSensor2.SelectedIndex == -1) cbHeatSensor2.SelectedIndex = 0;
                oPanel.DLPFH.OutDoorParam[4] = Convert.ToByte(cbHeatSensor2.SelectedIndex);
                oPanel.DLPFH.OutDoorParam[5] = Convert.ToByte(NumSub2.Value);
                oPanel.DLPFH.OutDoorParam[6] = Convert.ToByte(NumDev2.Value);
                oPanel.DLPFH.OutDoorParam[7] = Convert.ToByte(NumChn2.Value);

                if (oPanel.DLPFH.SourceParam == null) oPanel.DLPFH.SourceParam = new byte[8];
                if (cbHeatSensor3.SelectedIndex == -1) cbHeatSensor3.SelectedIndex = 0;
                oPanel.DLPFH.SourceParam[0] = Convert.ToByte(cbHeatSensor3.SelectedIndex);
                oPanel.DLPFH.SourceParam[1] = Convert.ToByte(NumSub3.Value);
                oPanel.DLPFH.SourceParam[2] = Convert.ToByte(NumDev3.Value);
                oPanel.DLPFH.SourceParam[3] = Convert.ToByte(NumChn3.Value);

                if (cbHeatSensor4.SelectedIndex == -1) cbHeatSensor4.SelectedIndex = 0;
                oPanel.DLPFH.SourceParam[4] = Convert.ToByte(cbHeatSensor4.SelectedIndex);
                oPanel.DLPFH.SourceParam[5] = Convert.ToByte(NumSub4.Value);
                oPanel.DLPFH.SourceParam[6] = Convert.ToByte(NumDev4.Value);
                oPanel.DLPFH.SourceParam[7] = Convert.ToByte(NumChn4.Value);
                if (chbHeat2.Checked) oPanel.DLPFH.PIDEnable = 1;
                oPanel.DLPFH.OutputType = Convert.ToByte(cbOutput.SelectedIndex);
                oPanel.DLPFH.minPWM = Convert.ToByte(cbMinPWM.SelectedIndex);
                oPanel.DLPFH.maxPWM = Convert.ToByte(cbMaxPWM.SelectedIndex);
                oPanel.DLPFH.Speed = Convert.ToByte(cbHeatSpeed.SelectedIndex);
                oPanel.DLPFH.Cycle = Convert.ToByte(cbCycle.SelectedIndex);

                for (int i = 0; i <= 4; i++)
                {
                    if (chbListHeatMode.GetItemChecked(i) == true) oPanel.DLPFH.ModeAry[i] = 1;
                    else oPanel.DLPFH.ModeAry[i] = 0;
                }

                if (chbHeat1.Checked) oPanel.DLPFH.Switch = 1;
                else oPanel.DLPFH.Switch = 0;

                oPanel.DLPFH.TimeAry = new Byte[4];
                oPanel.DLPFH.TimeAry[0] = Convert.ToByte(numStart1.Value);
                oPanel.DLPFH.TimeAry[1] = Convert.ToByte(numStart2.Value);
                oPanel.DLPFH.TimeAry[2] = Convert.ToByte(numEnd1.Value);
                oPanel.DLPFH.TimeAry[3] = Convert.ToByte(numEnd2.Value);
                String str = cbMaxTemp.Text;
                str = str.Replace("C", "");
                str = str.Replace("F", "");
                oPanel.DLPFH.ProtectTemperature = Convert.ToByte(str);
                oPanel.DLPFH.ControlMode = Convert.ToByte(cbControl.SelectedIndex);
                oPanel.DLPFH.HeatType = Convert.ToByte(cbHeatType.SelectedIndex - 1);
                if (chbHeat3.Checked) oPanel.DLPFH.SysEnable[0] = 1;
                else oPanel.DLPFH.SysEnable[0] = 0;

                if (chbHeat4.Checked) oPanel.DLPFH.SysEnable[1] = 1;
                else oPanel.DLPFH.SysEnable[1] = 0;

                if (chbHeat5.Checked) oPanel.DLPFH.SysEnable[2] = 1;
                else oPanel.DLPFH.SysEnable[2] = 0;

                if (chbHeat6.Checked) oPanel.DLPFH.SysEnable[3] = 1;
                else oPanel.DLPFH.SysEnable[3] = 0;

                oPanel.DLPFH.CompenValue = Convert.ToByte(sbModeChaneover.Value);
                #endregion
                // 温度范围
                #region
                str = cbHeatMinTemp.Text;
                str = str.Replace("C", "");
                str = str.Replace("F", "");
                oPanel.DLPFH.minTemp = Convert.ToByte(str);
                str = cbHeatMaxTemp.Text;
                str = str.Replace("<=", "");
                str = str.Replace("C", "");
                str = str.Replace("F", "");
                oPanel.DLPFH.maxTemp = Convert.ToByte(str);
                #endregion
            }
            catch
            {
            }
            Cursor.Current = Cursors.Default;
        }

        private void cbHeatSensor1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cbHeatSensor2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cbHeatSensor3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cbHeatSensor4_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void sbModeChaneover_ValueChanged(object sender, EventArgs e)
        {

        }      

        private void cbBaseDimming_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            oPanel.bytChn[2] = Convert.ToByte(cbBaseDimming.SelectedIndex);
        }

        private void btnSaveAndClose1_Click(object sender, EventArgs e)
        {
            btnSaveBasic_Click(null, null);
            this.Close();
        }

        private void btnSaveAndClose2_Click(object sender, EventArgs e)
        {
            btnSaveKey_Click(null, null);
            this.Close();
        }

        private void btnSaveAndCose4_Click(object sender, EventArgs e)
        {
            btnSaveChnInfo_Click(null, null);
            this.Close();
        }

        private void btnSaveAndClose5_Click(object sender, EventArgs e)
        {
            btnSaveDim_Click(null, null);
            this.Close();
        }

        private void btnSaveAndClose6_Click(object sender, EventArgs e)
        {
            btnSaveOther_Click(null, null);
            this.Close();
        }

        private void dgvTemp_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                cbTempEnable.Text = dgvTemp[1, e.RowIndex].Value.ToString();
                addcontrols(1, e.RowIndex, cbTempEnable, dgvTemp);

                txtSub.Text = dgvTemp[2, e.RowIndex].Value.ToString();
                addcontrols(2, e.RowIndex, txtSub, dgvTemp);

                txtDev.Text = dgvTemp[3, e.RowIndex].Value.ToString();
                addcontrols(3, e.RowIndex, txtDev, dgvTemp);

                cbCompen.Text = dgvTemp[4, e.RowIndex].Value.ToString();
                addcontrols(4, e.RowIndex, cbCompen, dgvTemp);
            }
        }

        private void DgChns_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                setAllVisible(false);
                if (e.RowIndex >= 0 && e.ColumnIndex == 4)
                {
                    sbTest.Text = HDLPF.IsNumStringMode(DgChns[4, e.RowIndex].Value.ToString(), 0, 100);
                    addcontrols(4, e.RowIndex, sbTest, DgChns);
                    sbTest.TextChanged += sbTest_ValueChanged;
                }
            }
            catch
            {
            }
        }

        private void btnRef2_Click(object sender, EventArgs e)
        {
            BlnShowBasicInformation = false;
            setAllVisible(false);

            tsbDown_Click(tsbDown, null);
        }

        private void btnSave2_Click(object sender, EventArgs e)
        {
            tsbDown_Click(toolStripLabel2, null);
        }

        private void btnSaveAndClose2_Click_1(object sender, EventArgs e)
        {
            tsbDown_Click(toolStripLabel2, null);
            this.Close();
        }

        private void sb1_Scroll(object sender, ScrollEventArgs e)
        {

        }

        private void dgvKey_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            dgvKey.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void dgvKey_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {

        }

        private void btnMode_Click(object sender, EventArgs e)
        {
            frmButtonSetup ButtonConfigration = new frmButtonSetup(oPanel, MyDevName,null);
            ButtonConfigration.ShowDialog();
        }

        private void btnCMD_Click(object sender, EventArgs e)
        {
            Byte[] PageID = new Byte[4];
            if (dgvKey.SelectedRows != null && dgvKey.SelectedRows.Count >0) 
            {
                PageID[0] = (Byte)dgvKey.SelectedRows[0].Index;
            }
            frmCmdSetup CmdSetup = new frmCmdSetup(oPanel, MyDevName, mywdDevicerType, PageID);
            CmdSetup.Show();
        }

        private void sb4_Scroll(object sender, ScrollEventArgs e)
        {

        }

        private void sb2_Scroll(object sender, ScrollEventArgs e)
        {

        }

        private void dgvKey_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (isRead) return;
            if (dgvKey.CurrentRow.Index < 0) return;
            if (dgvKey.RowCount <= 0) return;
            Cursor.Current = Cursors.WaitCursor;

            if (dgvKey[e.ColumnIndex, e.RowIndex].Value == null) dgvKey[e.ColumnIndex, e.RowIndex].Value = "";
            String Remark = dgvKey[e.ColumnIndex, e.RowIndex].Value.ToString();

            try
            {
                if (dgvKey.SelectedRows.Count > 0)
                {
                    for (int i = 0; i < dgvKey.SelectedRows.Count; i++)
                    {
                        dgvKey.SelectedRows[i].Cells[e.ColumnIndex].Value = Remark;
                        int index = dgvKey.SelectedRows[i].Index;
                        switch (e.ColumnIndex)
                        {
                            case 1:
                                if (dgvKey[1, index].Value == null) dgvKey[1, index].Value = "";
                                String ButtonRemark = dgvKey[1, index].Value.ToString();
                                oPanel.PanelKey[index].Remark = ButtonRemark;
                                break;
                            case 2:
                                oPanel.PanelKey[index].Mode = ButtonMode.ConvertorKeyModesToPublicModeGroup(dgvKey[2, index].Value.ToString());
                                break;
                            case 3:
                                byte[] arayTmp = new byte[3];
                                arayTmp[0] = 18;
                                arayTmp[1] = Convert.ToByte(e.RowIndex + 1);
                                arayTmp[2] = 255;
                                string str = dgvKey[3, e.RowIndex].Value.ToString();
                                if (str.ToLower() == "false")
                                {
                                    arayTmp[2] = 0;
                                }
                                if (CsConst.mySends.AddBufToSndList(arayTmp, 0xE3D8, SubNetID, DeviceID, false, false, false, CsConst.minAllWirelessDeviceType.Contains(mywdDevicerType)) == true)
                                {
                                    CsConst.myRevBuf = new byte[1200];
                                }
                                break;
                        }
                    }
                }
            }
            catch
            { }
            Cursor.Current = Cursors.Default;

        }

        private void sbAdjust_Scroll(object sender, ScrollEventArgs e)
        {

        }

        private void btnRelay_Click(object sender, EventArgs e)
        {
            FrmCalculateTargetForColorDLP frmTmp = new FrmCalculateTargetForColorDLP(MyDevName, mywdDevicerType, oPanel, 0);
            frmTmp.ShowDialog();
        }

        private void btnSlaves_Click(object sender, EventArgs e)
        {
            FrmHeatTargets frmTmp = new FrmHeatTargets(MyDevName, mywdDevicerType, oPanel);
            frmTmp.ShowDialog();
        }

        private void btnMoreFh_Click(object sender, EventArgs e)
        {
            Byte[] PageID = new Byte[4] { 101, 0, 0, 0 };

            frmCmdSetup CmdSetup = new frmCmdSetup(oPanel, MyDevName, mywdDevicerType, PageID);
            CmdSetup.ShowDialog();
        }


        void UpdateFloorheatingCurrentStatusToStruct()
        {
            // 当前状态
            #region
            if (chbHeatSwitch.Checked) oPanel.DLPFH.WorkingSwitch = 1;
            else oPanel.DLPFH.WorkingSwitch = 0;

            oPanel.DLPFH.WorkingTempMode = Convert.ToByte(cbCurrentMode.SelectedIndex + 1);
            oPanel.DLPFH.ModeTemp = new byte[4];
            oPanel.DLPFH.ModeTemp[0] = Convert.ToByte(sbHeatTemp1.Value);
            oPanel.DLPFH.ModeTemp[1] = Convert.ToByte(sbHeatTemp2.Value);
            oPanel.DLPFH.ModeTemp[2] = Convert.ToByte(sbHeatTemp3.Value);
            oPanel.DLPFH.ModeTemp[3] = Convert.ToByte(sbHeatTemp4.Value);
            #endregion
         }

        private void btnModifyFH_Click(object sender, EventArgs e)
        {
            UpdateFloorheatingCurrentStatusToStruct();
            oPanel.DLPFH.ModifyCurrentStatusFromFloorheatingModule(SubNetID, DeviceID, mywdDevicerType, oPanel.bytTempType);
        }

        private void sbHeatTemp4_ValueChanged(object sender, EventArgs e)
        {
            if (oPanel.bytTempType == 0)
            {
                lbHeatTempValue4.Text = sbHeatTemp4.Value.ToString() + "C";
            }
            else
            {
                lbHeatTempValue4.Text = sbHeatTemp4.Value.ToString() + "F";
            }
        }

        private void sbHeatTemp3_ValueChanged(object sender, EventArgs e)
        {
            if (oPanel.bytTempType == 0)
            {
                lbHeatTempValue3.Text = sbHeatTemp3.Value.ToString() + "C";
            }
            else
            {
                lbHeatTempValue3.Text = sbHeatTemp3.Value.ToString() + "F";
            }
        }

        private void sbHeatTemp2_ValueChanged(object sender, EventArgs e)
        {
            if (oPanel.bytTempType == 0)
            {
                lbHeatTempValue2.Text = sbHeatTemp2.Value.ToString() + "C";
            }
            else
            {
                lbHeatTempValue2.Text = sbHeatTemp2.Value.ToString() + "F";
            }
        }

        private void sbHeatTemp1_ValueChanged(object sender, EventArgs e)
        {
            if (oPanel.bytTempType == 0)
            {
                lbHeatTempValue1.Text = sbHeatTemp1.Value.ToString() + "C";
            }
            else
            {
                lbHeatTempValue1.Text = sbHeatTemp1.Value.ToString() + "F";
            }
        }


        private void btnStatusFH_Click(object sender, EventArgs e)
        {
            //读取
            oPanel.DLPFH.ReadCurrentStatusFromFloorheatingModule(SubNetID, DeviceID, mywdDevicerType);
            //界面处理
            DisplayCurrentStatusFloorheating();
        }

        private void cbMinPWM_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cbOutput_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void sb3_ValueChanged(object sender, EventArgs e)
        {
            lbv3.Text = (sb3.Value * 10).ToString() + "%";
            if (isRead) return;
            if (oPanel.otherInfo == null) return;
            oPanel.otherInfo.bytBacklight = Convert.ToByte(sb3.Value);
        }

        private void cbPages_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (isRead) return;
            if (oPanel == null) return;
            if (oPanel.otherInfo == null) return;
            if (oPanel.otherInfo.bytAryShowPage == null)
            {
                oPanel.otherInfo.bytAryShowPage = new byte[7];
            }
            if (NormalPanelDeviceTypeList.WirelessSimpleFloorHeatingDeviceTypeList.Contains(mywdDevicerType))
            {
                oPanel.otherInfo.bytAryShowPage[e.Index + 1] = Convert.ToByte(e.NewValue);
            }
            else
            {
                oPanel.otherInfo.bytAryShowPage[e.Index] = Convert.ToByte(e.NewValue);
            }
        }

        private void txtSwitch_TextChanged(object sender, EventArgs e)
        {
            string str = txtSwitch.Text;
            if (str == null) return;
            try
            {
                txtSwitch.Text = HDLPF.IsNumStringMode(str, 5, 60);
                txtSwitch.SelectionStart = txtSwitch.Text.Length;

                if (isRead) return;
                if (oPanel == null) return;
                if (oPanel.otherInfo.bytAryShowPage == null) return;
                oPanel.otherInfo.bytAryShowPage[0] = Convert.ToByte(txtSwitch.Text);
            }
            catch { }
        }

        private void txtSwitch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar) || e.KeyChar == 8)
                e.Handled = false;
            else
                e.Handled = true;
        }

        private void chbPWM_CheckedChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            if (oPanel == null) return;
            if (chbPWM.Checked) oPanel.LcdDisplayFlag = 1;
            else oPanel.LcdDisplayFlag = 0;
        }

        private void sbModeChaneover_Scroll(object sender, ScrollEventArgs e)
        {
            lbModeChangeOverValue.Text = sbModeChaneover.Value.ToString() + "C";
        }

        private void btnRange_Click(object sender, EventArgs e)
        {
            UpdateFloorheatingSettingToStruct();
            Byte[] arayAcTemperatureRange = new Byte[] { 5, 30, 5, 30, 5, 30,0, 5, 30 };
            oPanel.DLPFH.ModifyTemperatureRange(SubNetID, DeviceID, mywdDevicerType, arayAcTemperatureRange);
        }
    }
}
