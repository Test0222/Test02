using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace HDL_Buspro_Setup_Tool
{
    public partial class frmDLP : Form
    {
        [DllImport("user32.dll")]
        public static extern Boolean FlashWindow(IntPtr hwnd, Boolean bInvert);

        string myIcons = string.Empty;
        private DLP oPanel = null;
        private string MyDevName = null;
        private byte SubNetID;
        private byte DeviceID;
        private int mywdDevicerType = 0;
        private int mintIDIndex = -1;
        private Point Position = new Point(0, 0);
        private int MyActivePage = 0; //按页面上传下载

        private byte[] MyBufferProperties = new byte[20];
        private bool isRead = false;        

        private ComboBox cbTempEnable = new ComboBox();
        private System.Windows.Forms.Panel[] pnlOnColor = new System.Windows.Forms.Panel[8];
        private System.Windows.Forms.Panel pnlOff = new System.Windows.Forms.Panel();
        private TextBox txtSub = new TextBox();
        private TextBox txtDev = new TextBox();
        private ComboBox cbCompen = new ComboBox();

        private SingleChn sbTest = new SingleChn();
        private bool blState = false;
        public frmDLP()
        {
            InitializeComponent();
        }

        public frmDLP(Panel oPan, string strName, int intDeviceType, int intDIndex)
        {
            InitializeComponent();
            this.oPanel = (DLP)oPan;
            this.mywdDevicerType = intDeviceType;
            this.MyDevName = strName;
            this.mintIDIndex = intDIndex;
            tsl3.Text = strName;
            string strDevName = strName.Split('\\')[0].ToString();

            HDLSysPF.DisplayDeviceNameModeDescription(strName, mywdDevicerType, cboDevice, tbModel, tbDescription);

            cl32.Items.Clear(); cl32.Items.AddRange(CsConst.LoadType);

            SubNetID = Convert.ToByte(strDevName.Split('-')[0]);
            DeviceID = Convert.ToByte(strDevName.Split('-')[1]);
            tabDLP.SelectedIndex = 0;
        }

        void sbTest_ValueChanged(object sender, EventArgs e)
        {
            if (DgChns.CurrentRow.Index < 0) return;
            if (DgChns.RowCount <= 0) return;
            if (sbTest.Visible)
            {
                int index = DgChns.CurrentRow.Index;
                DgChns[4, index].Value = sbTest.Text.ToString();
                DgChns[6, index].Value = sbTest.Text.ToString();
                byte[] bytTmp = new byte[4];
                bytTmp[0] = (byte)(DgChns.CurrentRow.Index + 1);
                bytTmp[2] = 0;
                bytTmp[3] = 0;


                byte[] arayTmp = new byte[DgChns.RowCount];
                for (int j = 0; j < arayTmp.Length; j++)
                {
                    arayTmp[j] = Convert.ToByte(DgChns[4, j].Value.ToString());
                }
                if (CsConst.mySends.AddBufToSndList(arayTmp, 0xF022, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(mywdDevicerType)) == false)
                {
                    Cursor.Current = Cursors.Default;
                    return;
                }
                bytTmp[1] = Convert.ToByte(oPanel.myPanelDim[DgChns.CurrentRow.Index].MaxLevel);

                CsConst.mySends.AddBufToSndList(bytTmp, 0x0031, SubNetID, DeviceID, false, false, false, CsConst.minAllWirelessDeviceType.Contains(mywdDevicerType));
                
                ModifyMultilinesIfNeeds(DgChns[6, index].Value.ToString(), 6, DgChns);
                ModifyMultilinesIfNeeds(DgChns[4, index].Value.ToString(), 4, DgChns);
            }
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

        private void setAllVisible(bool TF)
        {
            cbTempEnable.Visible = TF;
            cbCompen.Visible = TF;
            txtSub.Visible = TF;
            txtDev.Visible = TF;
            sbTest.Visible = TF;
            for (int i = 0; i < pnlOnColor.Length;i++ )
            {
                System.Windows.Forms.Panel TmpPanel = pnlOnColor[i];
                if (TmpPanel == null)
                {
                    pnlOnColor[i] = new System.Windows.Forms.Panel();
                    TmpPanel = new System.Windows.Forms.Panel();
                }
                TmpPanel.Visible = TF;
            }
            pnlOff.Visible = TF;
        }

        void frmDLP_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.All;
            else e.Effect = DragDropEffects.None;
        }


        private void rb3_CheckedChanged(object sender, EventArgs e)
        {
            lbLCDelay.Visible = rb4.Checked;
            tbLCDelay.Visible = rb4.Checked;
            ls1.Visible = rb4.Checked;
            lbStandby.Visible = rb4.Checked;
            sb3.Visible = rb4.Checked;
            lbv3.Visible = rb4.Checked;
            chbAutoLock.Visible = rb4.Checked;
            if (isRead) return;
            if (oPanel == null) return;
            if (oPanel.otherInfo == null) return;
            if (rb3.Checked)
                oPanel.otherInfo.bytBacklightTime = 100;
            else if (rb4.Checked)
                oPanel.otherInfo.bytBacklightTime = (byte)tbLCDelay.Value;
        }

        private void rb1_CheckedChanged(object sender, EventArgs e)
        {
            lbPage.Visible = rb2.Checked;
            tbPage.Visible = rb2.Checked;
            lbDelay.Visible = rb2.Checked;
            tbDelay.Visible = rb2.Checked;
            ls2.Visible = rb2.Checked;
            if (isRead) return;
            if (rb1.Checked) oPanel.otherInfo.bytGotoPage = 0;
            else if (rb2.Checked) oPanel.otherInfo.bytGotoPage = (byte)tbPage.Value;
        }

        private void tbPage_ValueChanged(object sender, EventArgs e)
        {
            rb2.Text = CsConst.mstrINIDefault.IniReadValue("Public", "99635", "") + " " + tbPage.Value.ToString() + " " +
                       CsConst.mstrINIDefault.IniReadValue("Public", "99634", "") + " " + tbDelay.Value.ToString() + "S";
            if (isRead) return;
            if (oPanel == null) return;
            if (oPanel.otherInfo == null) return;
            oPanel.otherInfo.bytGotoPage = (byte)tbPage.Value;

        }

        void InitialFormCtrlsTextOrItems()
        {
            cbBaseDimming.Items.Clear();
            cbBaseDimming.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99879", ""));
            cbBaseDimming.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99880", ""));

            chbDisplay.Items.Clear();
            chbDisplay.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99874", ""));
            chbDisplay.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99800", ""));
            if (mywdDevicerType == 86 || mywdDevicerType == 87)
            {
                chbDisplay.Items.Clear();
                chbDisplay.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99874", ""));
            }

            cbPages.Items.Clear();
            for (int i = 0; i <= 6; i++)
                cbPages.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "0030" + i.ToString(), ""));
            if (mywdDevicerType == 86 || mywdDevicerType == 87)
            {
                cbPages.Items.Clear();
                for (int i = 0; i <= 5; i++)
                    cbPages.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "0030" + i.ToString(), ""));
                grpMusicIRControl.Visible = false;
            }

            chbSleepList.Items.Clear();
            for (int i = 0; i < 32; i++)
            {
                chbSleepList.Items.Add("Join " + CsConst.WholeTextsList[2437].sDisplayName + " "+ (i + 1).ToString());
            }

            HDLSysPF.LoadButtonModeWithDifferentDeviceType(clK3, mywdDevicerType);

            cbFont.Items.Clear();
            cbFont.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00250", ""));
            cbFont.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00251", ""));
            cbTempSource.Items.Clear();
            for (int i = 0; i < 3; i++)
            {
                cbTempSource.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "0024" + i.ToString(), ""));
            }
            cbTimeType.Items.Clear();
            for (int i = 3; i <= 6; i++)
                cbTimeType.Items.Add(CsConst.mstrINIDefault.IniReadValue("public", "9991" + i.ToString(), ""));
            cbFanSpeed.Items.Clear();
            for (int i = 0; i < 4; i++)
            {
                cbFanSpeed.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "0006" + i.ToString(), ""));
            }
            cbPowerOnState.Items.Clear();
            cbPowerOnState.Items.Add(CsConst.Status[0]);
            cbPowerOnState.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99883", ""));
            cbACType.Items.Clear();
            cbACType.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99884", ""));
            cbACType.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99885", ""));
            cbMode.Items.Clear();
            for (int i = 0; i < 5; i++)
                cbMode.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "0005" + i.ToString(), ""));
            cbHeatType.Items.Clear();
            cbHeatType.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00055", ""));
            cbHeatType.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99886", ""));
            cbHeatType.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99887", ""));
            cbHeatType.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99888", ""));
            cbControl.Items.Clear();
            cbControl.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99889", ""));
            cbControl.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99890", ""));
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
                cbSensitivity.Items.Add(i.ToString() + "%");
            }

            cbCurrentMode.Items.Clear();
            for (int i = 0; i <= 4; i++)
            {
                cbCurrentMode.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "0007" + i.ToString(), ""));
            }

            cbIRTarget.Items.Clear();
            cbIRTarget.Items.Add(CsConst.WholeTextsList[1775].sDisplayName);
            for (int i = 1; i <= 32; i++)
            {
                cbIRTarget.Items.Add(i.ToString());
            }

            for (int i = 0; i < pnlOnColor.Length;i++ )
            {
                System.Windows.Forms.Panel TmpPanel = pnlOnColor[i];
                if (TmpPanel == null)
                {
                    TmpPanel = new System.Windows.Forms.Panel();
                    pnlOnColor[i] = TmpPanel;
                }
                dgvBalance.Controls.Add(TmpPanel);
                TmpPanel.DoubleClick += pnlOn_Click;
            }
            dgvBalance.Controls.Add(pnlOff);
            pnlOff.DoubleClick += pnlOn_Click;

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
            sbTest.Text = "0";
            sbTest.TextChanged += sbTest_ValueChanged;
            DgChns.Controls.Add(sbTest);
            #endregion
            setAllVisible(false);
        }

        void pnlOn_Click(object sender, EventArgs e)
        {
            if (dgvBalance.CurrentRow.Index < 0) return;
            if (dgvBalance.RowCount <= 0) return;
            int index = dgvBalance.CurrentRow.Index;
            int ColumnID = dgvBalance.CurrentCell.ColumnIndex;

            if (PanelColorDialog.ShowDialog() == DialogResult.OK)
            {
                dgvBalance[ColumnID, index].Style.BackColor = PanelColorDialog.Color;
                dgvBalance[ColumnID, index].Style.SelectionBackColor = PanelColorDialog.Color;
                ((System.Windows.Forms.Panel)sender).BackColor = PanelColorDialog.Color;
                if (dgvBalance.SelectedRows == null || dgvBalance.SelectedRows.Count == 0) return;
                if (dgvBalance.SelectedRows.Count > 1)
                {
                    for (int i = 0; i < dgvBalance.SelectedRows.Count; i++)
                    {
                        dgvBalance[ColumnID, index].Style.BackColor = PanelColorDialog.Color;
                    }
                }
            }
        }

        void SetCtrlsVisbleWithDifferentDeviceType()
        {
            toolStrip1.Visible = (CsConst.MyEditMode == 0);
            if (mywdDevicerType == 162 || mywdDevicerType == 5000 || !CsConst.mintNewDLPFHSetupDeviceType.Contains(mywdDevicerType))
            {
                sbModeChaneover.Visible = false;
                lbModeChangeover.Visible = false;
                lbModeChangeOverValue.Visible = false;
                lbTimeFont.Visible = false;
                cbFont.Visible = false;
                lbTempSource.Visible = false;
                cbTempSource.Visible = false;
            }

            if (!CsConst.mintNewDLPFHSetupDeviceType.Contains(mywdDevicerType))
            {
                //cbHeatType.Enabled = false;
                cbControl.Visible = false;
                btnMoreFh.Visible = false;
                btnSlaves.Visible = false;
                btnRelay.Visible = false;
               // gbDevice.Visible = false;
            }

            if (mywdDevicerType == 176)
            {
                tabDLP.TabPages.Remove(tabMusic);
                tabDLP.TabPages.Remove(tabHeat);
            }
            if (mywdDevicerType == 155)
            {
                tabDLP.TabPages.Remove(tabKeys);
                tabDLP.TabPages.Remove(tabAC);
                tabDLP.TabPages.Remove(tabMusic);
            }
            if (mywdDevicerType == 159)
            {
                tabDLP.TabPages.Remove(tabAC);
                tabDLP.TabPages.Remove(tabHeat);
            }

            Boolean BlnHasWireless = DLPPanelDeviceTypeList.WirelessDLPDeviceTypeList.Contains(mywdDevicerType);
            if (!BlnHasWireless)
            {
                tabDLP.TabPages.Remove(tabChns);
                tabDLP.TabPages.Remove(tabDimmer);
            }
            if (mywdDevicerType == 86 || mywdDevicerType == 87)
            {
                tabDLP.TabPages.Remove(tabHeat);
            }
            Boolean IsColorfulDLP = DLPPanelDeviceTypeList.ColorfulDLPDeviceType.Contains(mywdDevicerType);
            if (IsColorfulDLP == false)
            {
                tabDLP.TabPages.Remove(tabSleep);
            }
        }

        private void frmDLP_Load(object sender, EventArgs e)
        {
            isRead = true;
            InitialFormCtrlsTextOrItems();
        }

        void txLTime_TextChanged(object sender, EventArgs e)
        {
            int tag = Convert.ToInt32((sender as TimeText).Tag.ToString());
            //throw new NotImplementedException();
        }

        private void DisplayBasicDLPInformationsToForm()
        {
            isRead = true;
            try
            {
                if (oPanel == null) return;
                if (oPanel.BrdDev == null) oPanel.BrdDev = new DeviceInfo("255-255");
                //基本信息的现在
                if (oPanel.otherInfo == null)
                {
                    oPanel.otherInfo = new Panel.OtherInfo();
                    if (oPanel.otherInfo.bytAryShowPage == null) oPanel.otherInfo.bytAryShowPage = new byte[7];
                }
                chbBroadCast.Checked = oPanel.IsBrdTemp;
                txtBroadSub.Text = oPanel.BrdDev.SubnetID.ToString();
                txtBroadDev.Text = oPanel.BrdDev.DeviceID.ToString();
                cbTemp.SelectedIndex = oPanel.bytTempType;
                chbAutoLock.Checked = (oPanel.otherInfo.AutoLock == 1);
                if (2 <= oPanel.AdjustValue && oPanel.AdjustValue <= 18)
                    sbAdjust.Value = Convert.ToInt32(oPanel.AdjustValue);
                for (byte bytI = 0; bytI < cbPages.Items.Count; bytI++)
                {
                    cbPages.SetItemChecked(bytI, (oPanel.otherInfo.bytAryShowPage[bytI] == 1));
                }
                if (Convert.ToInt32(oPanel.Backlight) <= sb1.Maximum)
                    sb1.Value = oPanel.Backlight;
                if (Convert.ToInt32(oPanel.Ledlight) <= sb2.Maximum)
                    sb2.Value = oPanel.Ledlight;
                if (Convert.ToInt32(oPanel.otherInfo.bytBacklight) <= sb3.Maximum)
                    sb3.Value = oPanel.otherInfo.bytBacklight;

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

                if (Convert.ToInt32(oPanel.LlimitDimmer) <= sb4.Maximum)
                    sb4.Value = oPanel.LlimitDimmer;

                if (oPanel.otherInfo.bytGotoPage == 0)
                    rb1.Checked = true;
                else
                {
                    rb2.Checked = true;
                    tbPage.Value = oPanel.otherInfo.bytGotoPage;
                    tbDelay.Value = oPanel.otherInfo.bytGotoTime;
                }

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

                if (oPanel.bytTimeFormat == 0) rbTime1.Checked = true;
                else if (oPanel.bytTimeFormat == 1) rbTime2.Checked = true;
                if (oPanel.bytDateFormat < 4)
                    cbTimeType.SelectedIndex = oPanel.bytDateFormat;

                chbDisplay.SetItemChecked(0, (oPanel.otherInfo.bytDisTemp == 1));
                if (mywdDevicerType != 86 && mywdDevicerType != 87)
                    chbDisplay.SetItemChecked(1, (oPanel.otherInfo.bytDisTime == 1));
                cbFont.Visible = (oPanel.TemperatureSource != 255);
                if (oPanel.TimeFontSize <= 1) cbFont.SelectedIndex = oPanel.TimeFontSize;
                cbTempSource.Visible = (oPanel.TemperatureSource != 255);
                if (oPanel.TemperatureSource <= 2) cbTempSource.SelectedIndex = oPanel.TemperatureSource;
                chbIR.Checked = (oPanel.IRON == 0);
                sb1_ValueChanged(null, null);
                sb2_ValueChanged(null, null);
                sb3_ValueChanged(null, null);
                rb1_CheckedChanged(rb1, null);
                rb3_CheckedChanged(rb3, null);
                sbAdjust_ValueChanged(null, null);
                tbDelay_ValueChanged(null, null);
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
                setAllVisible(false);
                if (oPanel == null) return;
                if (oPanel.PanelKey == null) return;
                Byte PageID = 0;
                if (cbPage.Visible)
                {
                    if (cbPage.Items.Count > 0 && cbPage.SelectedIndex < 0) cbPage.SelectedIndex = 0;
                    PageID = Convert.ToByte(cbPage.SelectedIndex);
                }

                int wdMaxValue = DeviceTypeList.GetMaxValueFromPublicModeGroup(mywdDevicerType);
                dgvKey.Rows.Clear();

                for (int i = 0; i < wdMaxValue; i++)
                {
                    HDLButton temp = oPanel.PanelKey[PageID * wdMaxValue + i];

                    string strMode = ButtonMode.ConvertorKeyModeToPublicModeGroup(temp.Mode);

                    object[] obj = new object[] { dgvKey.RowCount + 1, temp.Remark, strMode, false }; //strDimming, strSaveDimmingValue, strLED,strMutex,strDouble
                    dgvKey.Rows.Add(obj);
                }
            }
            catch
            {
            }
            isRead = false;
        }

        private void ShowTargetsInformationMusic()
        {
            try
            {
                isRead = true;
                if (oPanel == null) return;
                if (oPanel.DLPMusic == null) return;
                if (oPanel.DLPMusic.ArayDevs == null || oPanel.DLPMusic.ArayDevs.Length == 0)
                    oPanel.DLPMusic.ArayDevs = new byte[48];
                DLP.BGMusic temp = oPanel.DLPMusic;

                if (temp.bytEnable == 1) rbMusic1.Checked = true;
                else rbMusic2.Checked = true;
                if (temp.bytMusicType < cbMusicMode.Items.Count) cbMusicMode.SelectedIndex = temp.bytMusicType;
                if (1 <= temp.bytCurZone && temp.bytCurZone <= 24) cbMusicZone.SelectedIndex = temp.bytCurZone - 1;
                txtMusicSub.Text = temp.ArayDevs[cbMusicZone.SelectedIndex * 2].ToString();
                txtMusicDev.Text = temp.ArayDevs[cbMusicZone.SelectedIndex * 2 + 1].ToString();

                if (grpMusicIRControl.Visible == true)
                    ReadMusicUVTargets();
            }
            catch
            {
            }
            isRead = false;
        }

        private void ReadMusicUVTargets()
        {
            if (oPanel == null) return;
            if (oPanel.DLPMusic == null) return;
            try
            {
                isRead = true;
                if (oPanel.DLPMusic.KeyTargets == null || oPanel.DLPMusic.KeyTargets.Length == 0) return;

                foreach (UVCMD.ControlTargets tmp in oPanel.DLPMusic.KeyTargets)
                {
                    string str = "";
                    string strType = "";
                    string str1 = "N/A";
                    string str2 = "N/A";
                    string str3 = "N/A";
                    if (tmp != null)
                    {
                        switch (tmp.ID)
                        {
                            #region
                            case 0: str = CsConst.mstrINIDefault.IniReadValue("Public", "00160", ""); break;
                            case 1: str = CsConst.mstrINIDefault.IniReadValue("Public", "00161", ""); break;
                            case 2: str = CsConst.mstrINIDefault.IniReadValue("Public", "00162", ""); break;
                            case 3: str = CsConst.mstrINIDefault.IniReadValue("Public", "00163", ""); break;
                            case 4: str = CsConst.mstrINIDefault.IniReadValue("Public", "00164", ""); break;
                            case 5: str = CsConst.mstrINIDefault.IniReadValue("Public", "00165", ""); break;
                            case 6: str = CsConst.mstrINIDefault.IniReadValue("Public", "00166", ""); break;
                            case 7: str = CsConst.mstrINIDefault.IniReadValue("Public", "00167", ""); break;
                            case 8: str = CsConst.mstrINIDefault.IniReadValue("Public", "00168", ""); break;
                            case 9: str = CsConst.mstrINIDefault.IniReadValue("Public", "00169", ""); break;
                            case 10: str = CsConst.mstrINIDefault.IniReadValue("Public", "00170", ""); break;
                            case 11: str = CsConst.mstrINIDefault.IniReadValue("Public", "00171", ""); break;
                            case 12: str = CsConst.mstrINIDefault.IniReadValue("Public", "00172", ""); break;
                            case 13: str = CsConst.mstrINIDefault.IniReadValue("Public", "00173", ""); break;
                            #endregion
                        }
                        #region
                        strType = ButtonControlType.ConvertorKeyModeToPublicModeGroup(tmp.Type);
                        if (tmp.Type == 85)//场景
                        {
                            str1 = tmp.Param1.ToString() + "(" + CsConst.WholeTextsList[2510].sDisplayName + ")";
                            str2 = tmp.Param2.ToString() + "(" + CsConst.WholeTextsList[2511].sDisplayName + ")";
                        }
                        else if (tmp.Type == 88)//通用开关
                        {
                            str1 = tmp.Param1.ToString() + "(" + CsConst.WholeTextsList[2513].sDisplayName + ")";
                            if (tmp.Param2.ToString() == "0") str2 = CsConst.Status[0] + "(" + CsConst.WholeTextsList[2529].sDisplayName + ")";
                            else if (tmp.Param2.ToString() == "255") str2 = CsConst.Status[1] + "(" + CsConst.WholeTextsList[2529].sDisplayName + ")";
                        }
                        else if (tmp.Type == 89)//单路调节
                        {
                            str1 = tmp.Param1.ToString() + "(" + CsConst.WholeTextsList[934].sDisplayName + ")";
                            str2 = tmp.Param2.ToString() + "(" + CsConst.WholeTextsList[2524].sDisplayName + ")";
                            int intTmp = tmp.Param3 * 256 + tmp.Param4;
                            str3 = HDLPF.GetStringFromTime(intTmp, ":") + "(" + CsConst.WholeTextsList[2525].sDisplayName + ")";
                        }
                        #endregion
                        object[] obj = new object[] {str,tmp.SubnetID.ToString(),tmp.DeviceID.ToString(),
                                            strType,str1,str2,str3};
                        dgvMusic.Rows.Add(obj);
                    }
                }
            }
            catch
            {
            }
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

        private void ShowACInformationToForm()
        {
            try
            {
                isRead = true;
                if (oPanel == null) return;
                if (oPanel.DLPAC == null)
                {
                    oPanel.DLPAC = new ACSetting();
                    oPanel.DLPAC.bytAryWind = new byte[5]{4,0,1,2,3};
                    oPanel.DLPAC.bytAryMode = new byte[6] {5, 0, 1,2, 3, 4 };
                    oPanel.DLPAC.arayControl = new byte[10];
                }
                chbFunction.Checked = oPanel.DLPAC.IsEnable;
                if (oPanel.DLPAC.AcPowerOn >= 1)//13年早期的四代面板没加这个功能
                    cbPowerOnState.SelectedIndex = 1;
                else
                    cbPowerOnState.SelectedIndex = 0;
                if (oPanel.DLPAC.MyHVAC == null) oPanel.DLPAC.MyHVAC = new DeviceInfo("0-0");
                NumHVACSub.Text = oPanel.DLPAC.MyHVAC.SubnetID.ToString();
                NumHVACDev.Text = oPanel.DLPAC.MyHVAC.DeviceID.ToString();
                cbACType.SelectedIndex = oPanel.DLPAC.ACType;

                NumAC.Value = Convert.ToDecimal(oPanel.DLPAC.ACNo);
                chbIR1.Checked = (oPanel.DLPAC.bytAutoControl == 1);
                chbIR2.Checked = (oPanel.DLPAC.bytIRON == 1);
                chbIR3.Checked = (oPanel.DLPAC.bytRunMode == 1);
                chbIR4.Checked = (oPanel.DLPAC.bytSendIR == 1);

                UpdateTemperatureToForm();
                UpdateWindAndModesToForm();
                btnRefControl_Click(btnRefControl, null);

                isRead = false;
                chbFunction_CheckedChanged(null, null);
                sbCoolTemp_ValueChanged(null, null);

                DisplayTemperatureSensorInformation();
            }
            catch
            {
            }
        }

        void RefreshCurrentAcStatus()
        {
            isRead = true;
            cbPowerOn.Checked = (oPanel.DLPAC.arayControl[0] == 1);
            if (sbCoolTemp.Minimum <= oPanel.DLPAC.arayControl[1] && oPanel.DLPAC.arayControl[1] <= sbCoolTemp.Maximum)
                sbCoolTemp.Value = oPanel.DLPAC.arayControl[1];
            cbMode.SelectedIndex = Convert.ToInt32(oPanel.DLPAC.arayControl[2] >> 4);
            cbFanSpeed.SelectedIndex = Convert.ToInt32((oPanel.DLPAC.arayControl[2] & 0x0F) >> 4);
            if (oPanel.DLPAC.arayControl[3] == 1) chbLock.Checked = true;
            else chbLock.Checked = false;
            if (oPanel.bytTempType == 0)
            {
                if (HDLSysPF.GetBit(oPanel.DLPAC.arayControl[4], 7) == 1)
                    lbACTempValue.Text = "-" + ((oPanel.DLPAC.arayControl[4] & (byte.MaxValue - (1 << 7)))).ToString() + "C";
                else
                    lbACTempValue.Text = oPanel.DLPAC.arayControl[4].ToString() + "C";
            }
            else if (oPanel.bytTempType == 1)
            {
                if (HDLSysPF.GetBit(oPanel.DLPAC.arayControl[4], 7) == 1)
                    lbACTempValue.Text = "-" + ((oPanel.DLPAC.arayControl[4] & (byte.MaxValue - (1 << 7)))).ToString() + "F";
                else
                    lbACTempValue.Text = oPanel.DLPAC.arayControl[4].ToString() + "F";
            }
            if (sbHeatTemp.Minimum <= oPanel.DLPAC.arayControl[5] && oPanel.DLPAC.arayControl[5] <= sbHeatTemp.Maximum)
                sbHeatTemp.Value = oPanel.DLPAC.arayControl[5];
            string strMode = CsConst.mstrINIDefault.IniReadValue("Public", "0005" + (oPanel.DLPAC.arayControl[6] >> 4).ToString(), "");
            string strSpeed = CsConst.mstrINIDefault.IniReadValue("Public", "0006" + (oPanel.DLPAC.arayControl[6] & 0x0F).ToString(), "");
            if (sbAutoTemp.Minimum <= oPanel.DLPAC.arayControl[7] && oPanel.DLPAC.arayControl[7] <= sbAutoTemp.Maximum)
                sbAutoTemp.Value = oPanel.DLPAC.arayControl[7];
            if (sbDryTemp.Minimum <= oPanel.DLPAC.arayControl[8] && oPanel.DLPAC.arayControl[8] <= sbDryTemp.Maximum)
                sbDryTemp.Value = oPanel.DLPAC.arayControl[8];
            chbWind.Checked = ((oPanel.DLPAC.arayControl[9] & 0x0F) == 1);
            string strWind = "";
            if (chbWind.Checked)
                strWind = CsConst.mstrINIDefault.IniReadValue("Public", "99893", "");
            lbACStateValue.Text = strMode + "," + strSpeed + "," + strWind;
            isRead = false;
        }

        private void frmDLP_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                FlashWindow(this.Handle, true);
            }
        }

        private void pu1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(TreeNode)))
            {
                e.Effect = DragDropEffects.Move;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void toolStripButton12_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            //GetKeysIconsPanelToBuffer();
            oPanel.UploadPanelIconsToDevice(mywdDevicerType, MyDevName);
            Cursor.Current = Cursors.Default;
        }

        private void tabDLP_SelectedIndexChanged(object sender, EventArgs e)
        {
            isRead = true;
            if (tabDLP.SelectedTab.Name == "tabBasic") MyActivePage = 2;
            else if (tabDLP.SelectedTab.Name == "tabKeys") MyActivePage = 1;
            else if (tabDLP.SelectedTab.Name == "tabAC") MyActivePage = 3;
            else if (tabDLP.SelectedTab.Name == "tabMusic") MyActivePage = 4;
            else if (tabDLP.SelectedTab.Name == "tabHeat") MyActivePage = 5;
            else if (tabDLP.SelectedTab.Name == "tabChns") MyActivePage = 6;
            else if (tabDLP.SelectedTab.Name == "tabDimmer") MyActivePage = 7;
            else if (tabDLP.SelectedTab.Name == "tabSleep") MyActivePage = 9;
            if (CsConst.MyEditMode == 1)
            {

                if (oPanel.MyRead2UpFlags[MyActivePage - 1] == false)
                {
                    tsbDown_Click(tsbDown, null);
                }
                else
                {
                    UpdateDisplayInformationAccordingly(null,null);
                }
            }
        }

        private void chbIR_CheckedChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            if (oPanel == null) return;
            if (chbIR.Checked == true) oPanel.IRON = 0;
            else oPanel.IRON = 1;
        }

        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            if (oPanel == null) return;
            Cursor.Current = Cursors.WaitCursor;
            oPanel.SavePanelToDB();
            Cursor.Current = Cursors.Default;
        }

        private void cboTtypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            byte bytTmp = (byte)((ComboBox)sender).SelectedIndex;
            oPanel.bytTempType = bytTmp;
        }

        private void helpToolStripButton_Click(object sender, EventArgs e)
        {
            if (oPanel == null) return;

            
        }

        delegate void SetvalueHandle(int rowIndex);

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

        private void DgChns_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (oPanel == null) return;
                if ((e.RowIndex == -1) || (e.ColumnIndex == -1)) return;
                isRead = true;
                if (DgChns[e.ColumnIndex, e.RowIndex].Value == null) DgChns[e.ColumnIndex, e.RowIndex].Value = "";
                for (int i = 0; i < DgChns.SelectedRows.Count; i++)
                {
                    string strTmp = "";
                    switch (e.ColumnIndex)
                    {
                        case 1:
                            strTmp = DgChns[1, e.RowIndex].Value.ToString();
                            DgChns[1, e.RowIndex].Value = HDLPF.IsRightStringMode(strTmp);
                            oPanel.myPanelDim[e.RowIndex].Remark = DgChns[1, e.RowIndex].Value.ToString();
                            break;
                        case 2:
                            oPanel.myPanelDim[e.RowIndex].LoadType = cl32.Items.IndexOf(DgChns[2, e.RowIndex].Value.ToString());
                            break;
                        case 3:
                            strTmp = DgChns[3, e.RowIndex].Value.ToString();
                            DgChns[3, e.RowIndex].Value = HDLPF.IsNumStringMode(strTmp, 0, 100);
                            oPanel.myPanelDim[e.RowIndex].MinValue = int.Parse(DgChns[3, e.RowIndex].Value.ToString());
                            break;
                        case 4:
                            strTmp = DgChns[4, e.RowIndex].Value.ToString();
                            DgChns[4, e.RowIndex].Value = HDLPF.IsNumStringMode(strTmp, 0, 100);
                            oPanel.myPanelDim[e.RowIndex].MaxLevel = int.Parse(DgChns[4, e.RowIndex].Value.ToString());
                            DgChns[6, e.RowIndex].Value = DgChns[4, e.RowIndex].Value.ToString();

                            break;
                        case 5:
                            strTmp = DgChns[5, e.RowIndex].Value.ToString();
                            DgChns[5, e.RowIndex].Value = HDLPF.IsNumStringMode(strTmp, 0, 100);
                            oPanel.myPanelDim[e.RowIndex].MaxValue = int.Parse(DgChns[5, e.RowIndex].Value.ToString());
                            break;
                    }
                    if (e.ColumnIndex != 6)
                        DgChns.SelectedRows[i].Cells[e.ColumnIndex].Value = DgChns[e.ColumnIndex, e.RowIndex].Value.ToString();
                }               
            }
            catch
            {
            }
            isRead = false;
        }

        private void DgChns_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            DgChns.CommitEdit(DataGridViewDataErrorContexts.Commit);
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
                    int num2 = 32;
                    UpdateMyActivePageID();
                    
                    byte[] ArayRelay = new byte[] { bytSubID, bytDevID, (byte)(mywdDevicerType / 256), (byte)(mywdDevicerType % 256), 
                        (byte)MyActivePage,(byte)(mintIDIndex / 256), (byte)(mintIDIndex % 256),(byte)num1,(byte)num2 };
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

        void UpdateMyActivePageID()
        {
            if (tabDLP.SelectedTab.Name == "tabBasic") MyActivePage = 2;
            else if (tabDLP.SelectedTab.Name == "tabKeys")
            {
                if (cbPage.Items.Count > 0 && cbPage.SelectedIndex < 0) cbPage.SelectedIndex = 0;
                MyActivePage = 1;                
            }
            else if (tabDLP.SelectedTab.Name == "tabAC") MyActivePage = 3;
            else if (tabDLP.SelectedTab.Name == "tabPage1") MyActivePage = 4;
            else if (tabDLP.SelectedTab.Name == "tabHeat")
            {
                MyActivePage = 5;
                UpdateFloorheatingSettingToStruct();
            }
            else if (tabDLP.SelectedTab.Name == "tabChns") MyActivePage = 6;
            else if (tabDLP.SelectedTab.Name == "tabDimmer") MyActivePage = 7;
            else if (tabDLP.SelectedTab.Name == "tabSleep")
            {
                MyActivePage = 9;
                UpdateColorAndSensitivitySetup();
            }
        }

        void UpdateDisplayInformationAccordingly(object sender, FormClosingEventArgs e)
        {
            switch (tabDLP.SelectedTab.Name)
            {
                case "tabBasic":
                    //基本信息
                    DisplayBasicDLPInformationsToForm(); break;
                //显示目标信息
                case "tabKeys":
                    DisplayBasicDLPInformationsToForm();
                    ShowKeysInformationPanel(); break;
                case "tabAC": ShowACInformationToForm(); break;
                //显示音乐播放目标
                case "tabMusic": ShowTargetsInformationMusic(); break;
                case "tabHeat": ShowFHInformation(); break;
                case "tabChns": showChannelInfo(); break;
                case "tabDimmer": showDimmingInfo(); break;
                case "tabSleep":
                    ShowSensitivityAndColorSetup();
                    showSleepMode(); break;
            }
            Cursor.Current = Cursors.Default;
            this.TopMost = true;
            this.WindowState = FormWindowState.Normal;
            this.BringToFront();
            this.Activate();
            this.TopMost = false;
        }

        private void showSleepMode()
        {
            try
            {
                isRead = true;

                if (oPanel.otherInfo.IRCloseTargets <= 32) cbIRTarget.SelectedIndex = oPanel.otherInfo.IRCloseTargets;
                if (chbSensor.Visible)
                {
                    chbSensor.Checked = (oPanel.otherInfo.IRCloseSensor == 1);
                    if (oPanel.otherInfo.CloseToSensorSensitivity <= 100) cbSensitivity.SelectedIndex = oPanel.otherInfo.CloseToSensorSensitivity;
                }
                string strEnable = "";
                chbSleep.Checked = (oPanel.araySleep[1] == 1);
                for (int i = 0; i < 4; i++)
                {
                    string strTmp = GlobalClass.AddLeftZero(Convert.ToString(oPanel.araySleep[i + 2], 2), 8);
                    for (int j = 0; j <=7; j++)
                    {
                        string str = strTmp.Substring(7 - j, 1);
                        chbSleepList.SetItemChecked(i * 8 + j, str == "1");
                    }
                }
            }
            catch
            {
            }
            isRead = false;
        }

        private void ShowSensitivityAndColorSetup()
        {
            try
            {
                setAllVisible(false);
                int wdMaxValue = int.Parse(CsConst.mstrINIDefault.IniReadValue("DeviceType" + mywdDevicerType.ToString(), "MaxValue", "0"));
                dgvBalance.Rows.Clear();
                for (int i = 0; i <= wdMaxValue; i++)
                {
                    String ButtonHint = i.ToString();
                    if (i == 0) ButtonHint = "Shortcut";
                    object[] obj = new object[] { ButtonHint, oPanel.arayButtonBalance[i * 3 + 1], oPanel.arayButtonBalance[i * 3 + 2], oPanel.arayButtonBalance[i * 3 + 3] };
                    dgvBalance.Rows.Add(obj);
                }

                for (int j = 0; j < 7; j++)
                {
                    for (int i = 0; i < dgvBalance.RowCount; i++)
                    {
                        dgvBalance[4 + j, i].Style.BackColor = Color.FromArgb(oPanel.arayButtonColor[9 + i * 6 + j * 54], oPanel.arayButtonColor[9 + i * 6 + 1 + j * 54],
                                                                          oPanel.arayButtonColor[9 + i * 6 + 2 + j * 54]);
                        if (j ==6) // 显示off的颜色
                           dgvBalance[11, i].Style.BackColor = Color.FromArgb(oPanel.arayButtonColor[9 + i * 6 + 3], oPanel.arayButtonColor[9 + i * 6 + 4], 
                                                                              oPanel.arayButtonColor[9 + i * 6 + 5]);
                    }
                }
                DisplayBackgroudColor(0);
            }
            catch
            {
            }
            isRead = false;
        }

        void DisplayBackgroudColor(int RowIndex)
        {
            int ColumnID = 4;
            foreach (System.Windows.Forms.Panel TmpPanel in pnlOnColor)
            {
                TmpPanel.BackColor = dgvBalance[ColumnID, RowIndex].Style.BackColor;
                addcontrols(ColumnID, RowIndex, TmpPanel, dgvBalance);
                TmpPanel.Width = TmpPanel.Width - 1;
                TmpPanel.Height = TmpPanel.Height - 1;
                ColumnID++;
            }
            pnlOff.BackColor = dgvBalance[11, RowIndex].Style.BackColor;
            addcontrols(11, RowIndex, pnlOff, dgvBalance);
            pnlOff.Width = pnlOff.Width - 1;
            pnlOff.Height = pnlOff.Height - 1;
        }

        private void showDimmingInfo()
        {
            Boolean IsGiveTemperature = false;
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
                                    str3 = oPanel.BaseInfomation[i * 4 + 2].ToString();
                                    IsGiveTemperature = (oPanel.BaseTempInfomation[i * 4 + 1] ==4);
                                }
                            }
                        }
                        #endregion
                        object[] obj = new object[] { dgvAddress.RowCount + 1, strState, strConnection, strAddressMac, str2, str3 };
                        dgvAddress.Rows.Add(obj);
                    }
                    lbCountValue.Text = ConnectedCount.ToString();
                }
                hbTemp.Value = Convert.ToInt32(oPanel.bytChn[1]);
                lbSaveDimming.Visible = (oPanel.bytChn[0] == 2);
                cbBaseDimming.Visible = (oPanel.bytChn[0] == 2);
                cbBaseDimming.SelectedIndex = oPanel.bytChn[2];

                groupBox1.Visible = (oPanel.BaseTempInfomation != null && IsGiveTemperature);
                if (groupBox1.Visible)
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
                setAllVisible(false);
                DgChns.Rows.Clear();
                foreach (Channel ch in oPanel.myPanelDim)
                {
                    object[] boj = new object[] { ch.ID, ch.Remark, cl32.Items[ch.LoadType], ch.MinValue, ch.MaxLevel, 0, ch.MaxLevel };
                    DgChns.Rows.Add(boj);
                }
            }
            catch
            {
            }
            isRead = false;
        }


        private void tmRead_Click(object sender, EventArgs e)
        {
            setAllVisible(false);
            tsbDown_Click(tsbDown, null);
        }


        private void frmDLP_Shown(object sender, EventArgs e)
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
                    //基本信息
                    DisplayBasicDLPInformationsToForm();
                }
            }
            isRead = false;
        }

        private void tbPage_TextChanged(object sender, EventArgs e)
        {
            int tag = Convert.ToInt32(((NumericUpDown)sender).Tag);
            UpDownBase baseTmp = (UpDownBase)sender;

            if (string.IsNullOrEmpty(baseTmp.Text))
            {
                if (tag == 0)
                    baseTmp.Text = Convert.ToString(1);
                else if (tag == 1)
                    baseTmp.Text = Convert.ToString(20);
            }
        }

        private void sb3_ValueChanged(object sender, EventArgs e)
        {
            lbv3.Text = (sb3.Value * 10).ToString() + "%";
            if (isRead) return;
            if (oPanel.otherInfo == null) return;
            oPanel.otherInfo.bytBacklight = Convert.ToByte(sb3.Value);
        }

        private void sb1_ValueChanged(object sender, EventArgs e)
        {
            lbv1.Text = sb1.Value.ToString() + "%";
            if (isRead) return;
            if (oPanel == null) return;
            oPanel.Backlight = byte.Parse(sb1.Value.ToString());
        }

        private void sb2_ValueChanged(object sender, EventArgs e)
        {
            lbv2.Text = sb2.Value.ToString() + "%";
            if (isRead) return;
            if (oPanel == null) return;
            oPanel.Ledlight = byte.Parse(sb2.Value.ToString());
        }

        private void tbLCDelay_ValueChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            if (oPanel == null) return;
            if (oPanel.otherInfo == null) return;
            oPanel.otherInfo.bytBacklightTime = (byte)tbLCDelay.Value;
        }

        private void tbDelay_ValueChanged(object sender, EventArgs e)
        {
            rb2.Text = CsConst.mstrINIDefault.IniReadValue("Public", "99635", "") + " " + tbPage.Value.ToString() + " " +
                       CsConst.mstrINIDefault.IniReadValue("Public", "99634", "") + " " + tbDelay.Value.ToString() + "S";
            if (isRead) return;
            if (oPanel == null) return;
            if (oPanel.otherInfo == null) return;
            oPanel.otherInfo.bytGotoTime = (byte)tbDelay.Value;
        }

        private void sb4_ValueChanged(object sender, EventArgs e)
        {
            lbv4.Text = sb4.Value.ToString()+"%";
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

        private void rbTime1_CheckedChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            if (oPanel == null) return;
            if (rbTime1.Checked) oPanel.bytTimeFormat = 0;
            else if (rbTime2.Checked) oPanel.bytTimeFormat = 1;
        }

        private void cbTimeType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            if (oPanel == null) return;
            oPanel.bytDateFormat = Convert.ToByte(cbTimeType.SelectedIndex);
        }

        private void chbDisplay_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (isRead) return;
            if (oPanel == null) return;
            if (oPanel.otherInfo == null) return;
            if (e.Index == 0)
                oPanel.otherInfo.bytDisTemp = Convert.ToByte(e.NewValue);
            if (e.Index == 1)
                oPanel.otherInfo.bytDisTime = Convert.ToByte(e.NewValue);
        }

        private void cbPages_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (isRead) return;
            if (oPanel == null) return;
            if (oPanel.otherInfo == null) return;
            if (oPanel.otherInfo.bytAryShowPage == null)
            {
                oPanel.otherInfo.bytAryShowPage = new byte[cbPages.Items.Count];
            }
            oPanel.otherInfo.bytAryShowPage[e.Index] = Convert.ToByte(e.NewValue);
        }

        private void cbPage_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            if (oPanel.MyRead2UpFlags[0] == false)
            {
                tsbDown_Click(tsbDown, null);
            }
            ShowKeysInformationPanel();
        }

        private void chbFunction_CheckedChanged(object sender, EventArgs e)
        {
            grpsection.Enabled = chbFunction.Checked;
            grpBroadcast.Enabled = chbFunction.Checked;
            if (isRead) return;
            if (oPanel == null) return;
            if (oPanel.DLPAC == null) return;
            oPanel.DLPAC.IsEnable = chbFunction.Checked;
        }

        private void btnACSetup_Click(object sender, EventArgs e)
        {
            if (mywdDevicerType == 155 || mywdDevicerType == 156 || mywdDevicerType == 157 || mywdDevicerType == 160 ||
                mywdDevicerType == 86 || mywdDevicerType == 87 || mywdDevicerType == 48 || mywdDevicerType == 149 ||
                mywdDevicerType == 154)
            { }
        }

        private void btnrefence_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                byte[] ArayTmp = new byte[0];
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xE120, SubNetID, DeviceID, false, true, true,CsConst.minAllWirelessDeviceType.Contains(mywdDevicerType)) == true)
                {
                    if (CsConst.myRevBuf[25] <= 1)
                        cbTemp.SelectedIndex = CsConst.myRevBuf[25];
                    CsConst.myRevBuf = new byte[1200];
                }
                else return;
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xE0E4, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(mywdDevicerType)) == true)
                {
                    oPanel.AdjustValue = CsConst.myRevBuf[29];//补偿值
                    if (2 <= oPanel.AdjustValue && oPanel.AdjustValue <= 18)
                        sbAdjust.Value = Convert.ToInt32(oPanel.AdjustValue);
                    if (oPanel.DLPAC == null)
                    {
                        oPanel.DLPAC = new ACSetting();
                        oPanel.DLPAC.MyHVAC = new DeviceInfo("0-0");
                    }
                    oPanel.DLPAC.IsEnable = (CsConst.myRevBuf[25] == 1);
                    oPanel.DLPAC.MyHVAC.SubnetID = CsConst.myRevBuf[26];
                    oPanel.DLPAC.MyHVAC.DeviceID = CsConst.myRevBuf[27];
                    oPanel.AdjustValue = CsConst.myRevBuf[28];
                    oPanel.DLPAC.ACNo = CsConst.myRevBuf[29];
                    oPanel.DLPAC.ACType = CsConst.myRevBuf[30];
                    oPanel.DLPAC.bytIRON = CsConst.myRevBuf[31];
                    oPanel.DLPAC.AcPowerOn = CsConst.myRevBuf[32];

                    sbAdjust_ValueChanged(null, null);
                }
                CsConst.myRevBuf = new byte[1200];

                if (CsConst.mySends.AddBufToSndList(null, 0xE0F8, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(mywdDevicerType)) == true)
                {

                    oPanel.BrdDev = new DeviceInfo("0-0");

                    if (CsConst.myRevBuf[25] == 1)
                        oPanel.IsBrdTemp = true;
                    else
                        oPanel.IsBrdTemp = false;
                    oPanel.BrdDev = new DeviceInfo(CsConst.myRevBuf[26].ToString() + "-" + CsConst.myRevBuf[27].ToString());
                    chbBroadCast.Checked = oPanel.IsBrdTemp;
                    txtBroadSub.Text = oPanel.BrdDev.SubnetID.ToString();
                    txtBroadDev.Text = oPanel.BrdDev.DeviceID.ToString();
                    CsConst.myRevBuf = new byte[1200];
                    HDLUDP.TimeBetwnNext(10);
                }
                else return;
            }
            catch
            {
            }
            Cursor.Current = Cursors.Default;
        }

        private void btnSaveTemp_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            byte[] ArayTmp = new byte[1];
            ArayTmp[0] = Convert.ToByte(cbTemp.SelectedIndex);
            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xE122, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(mywdDevicerType)) == true)
            {
                oPanel.bytTempType = Convert.ToByte(cbTemp.SelectedIndex);
                UpdateTemperatureToForm();
                if (tabDLP.SelectedTab.Name == "tabAC" || tabDLP.SelectedTab.Name == "tabHeat")
                {
                    if (tabDLP.SelectedTab.Name == "tabAC")
                    {
                        tsbDown_Click(tsbDown, null);
                    }
                    else if (tabDLP.SelectedTab.Name == "tabHeat")
                    {
                        tsbDown_Click(tsbDown, null);
                    }
                }
                CsConst.myRevBuf = new byte[1200];
            }
            else return;
            if (oPanel.DLPAC.MyHVAC != null)
            {
                ArayTmp = new byte[8];   //HVAC基本设置
                ArayTmp[0] = Convert.ToByte(oPanel.DLPAC.IsEnable);
                ArayTmp[1] = oPanel.DLPAC.MyHVAC.SubnetID;
                ArayTmp[2] = oPanel.DLPAC.MyHVAC.DeviceID;
                ArayTmp[3] = Convert.ToByte(sbAdjust.Value);
                ArayTmp[4] = oPanel.DLPAC.ACNo;
                ArayTmp[5] = oPanel.DLPAC.ACType;
                ArayTmp[6] = oPanel.DLPAC.bytIRON;
                ArayTmp[7] = oPanel.DLPAC.AcPowerOn;
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xE0E6, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(mywdDevicerType)) == true)
                {
                    oPanel.AdjustValue = Convert.ToByte(sbAdjust.Value);
                }
            }
            CsConst.myRevBuf = new byte[1200];
            btnBroadcast_Click(null, null);
            Cursor.Current = Cursors.Default;
        }

        private void TemperatureModeForFH()
        {
            if (oPanel.bytTempType == 0)
            {
                cbHeatMinTemp.Items.Clear();
                cbHeatMaxTemp.Items.Clear();
                cbMaxTemp.Items.Clear();
                for (int i = 5; i <= 99; i++)
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
                for (int i = 41; i <= 210; i++)
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

        private void cbPowerOnState_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            if (oPanel == null) return;
            if (oPanel.DLPAC == null) return;
            oPanel.DLPAC.AcPowerOn = Convert.ToByte(cbPowerOnState.SelectedIndex);
        }

        private void cbACType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            if (oPanel == null) return;
            if (oPanel.DLPAC == null) return;
            oPanel.DLPAC.ACType = Convert.ToByte(cbACType.SelectedIndex);
        }

        private void NumHVACSub_ValueChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            if (oPanel == null) return;
            if (oPanel.DLPAC == null) return;
            if (((TextBox)sender).Text == null || (((TextBox)sender).Text == "")) return;
            oPanel.DLPAC.MyHVAC.SubnetID = Convert.ToByte(NumHVACSub.Text);
            oPanel.DLPAC.MyHVAC.DeviceID = Convert.ToByte(NumHVACDev.Text);
        }

        private void chbIR1_CheckedChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            if (oPanel == null) return;
            if (oPanel.DLPAC == null) return;
            if (chbIR1.Checked) chbIR3.Checked = true;
            if (chbIR1.Checked) oPanel.DLPAC.bytAutoControl = 1;
            else oPanel.DLPAC.bytAutoControl = 0;
            if (chbIR2.Checked) oPanel.DLPAC.bytIRON = 1;
            else oPanel.DLPAC.bytIRON = 0;
            if (chbIR3.Checked) oPanel.DLPAC.bytRunMode = 1;
            else oPanel.DLPAC.bytRunMode = 0;
            if (chbIR4.Checked) oPanel.DLPAC.bytSendIR = 1;
            else oPanel.DLPAC.bytSendIR = 0;
            btnIrList.Visible = chbIR4.Checked;
            btnSYN.Visible = chbIR3.Checked;
            btnTSensor.Visible = chbIR3.Checked;
        }

        private void sbAdjust_ValueChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            if (oPanel == null) return;
            if (oPanel.DLPAC == null) return;
            lbAdjust.Text = (sbAdjust.Value - 10).ToString();
        }

        private void cbPowerOn_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void SetControlInfo()
        {
            if (isRead) return;
            if (oPanel == null) return;
            if (oPanel.DLPAC == null) return;
            if (cbMode.SelectedIndex == -1) return;
            if (cbFanSpeed.SelectedIndex == -1) return;
            try
            {
                if (oPanel.DLPAC.arayControl == null) oPanel.DLPAC.arayControl = new byte[10];
                if (cbPowerOn.Checked) oPanel.DLPAC.arayControl[0] = 1;
                oPanel.DLPAC.arayControl[1] = Convert.ToByte(sbCoolTemp.Value);
                oPanel.DLPAC.arayControl[2] = Convert.ToByte((cbMode.SelectedIndex << 4) + cbFanSpeed.SelectedIndex);
                if (chbLock.Checked)
                    oPanel.DLPAC.arayControl[3] = 1;
                else
                    oPanel.DLPAC.arayControl[3] = 0;
                oPanel.DLPAC.arayControl[5] = Convert.ToByte(sbHeatTemp.Value);
                oPanel.DLPAC.arayControl[6] = Convert.ToByte(sbAutoTemp.Value);
                oPanel.DLPAC.arayControl[7] = Convert.ToByte(sbDryTemp.Value);
                if (chbWind.Checked) oPanel.DLPAC.arayControl[9] = 1;
                else oPanel.DLPAC.arayControl[9] = 0;
            }
            catch { }
        }

        private void sbCoolTemp_ValueChanged(object sender, EventArgs e)
        {
            if (oPanel.bytTempType == 0)
            {
                lbCoolTempValue.Text = sbCoolTemp.Value.ToString() + "C";
                lbHeatTempValue.Text = sbHeatTemp.Value.ToString() + "C";
                lbAutoTempValue.Text = sbAutoTemp.Value.ToString() + "C";
                lbDryTempValue.Text = sbDryTemp.Value.ToString() + "C";
            }
            else if (oPanel.bytTempType == 1)
            {
                lbCoolTempValue.Text = sbCoolTemp.Value.ToString() + "F";
                lbHeatTempValue.Text = sbHeatTemp.Value.ToString() + "F";
                lbAutoTempValue.Text = sbAutoTemp.Value.ToString() + "F";
                lbDryTempValue.Text = sbDryTemp.Value.ToString() + "F";
            }
            SetControlInfo();
        }

        private void btnRefControl_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.Default;

                if (oPanel.DLPAC.ReadDLPACPageCurrentStatus(SubNetID, DeviceID, mywdDevicerType) == true)
                {
                    RefreshCurrentAcStatus();
                }
            }
            catch
            {
            }
            Cursor.Current = Cursors.Default;
        }

        private void chbBroadCast_CheckedChanged(object sender, EventArgs e)
        {
            txtBroadSub.Enabled = chbBroadCast.Checked;
            txtBroadDev.Enabled = chbBroadCast.Checked;
            if (isRead) return;
            oPanel.IsBrdTemp = chbBroadCast.Checked;
        }

        private void chbLock_CheckedChanged(object sender, EventArgs e)
        {
            SetControlInfo();
        }

        private void cbMusicZone_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (oPanel == null) return;
            if (oPanel.DLPMusic == null) return;
            if (isRead) return;
            oPanel.DLPMusic.bytCurZone = Convert.ToByte(cbMusicZone.SelectedIndex + 1);
            txtMusicSub.Text = oPanel.DLPMusic.ArayDevs[cbMusicZone.SelectedIndex * 2].ToString();
            txtMusicDev.Text = oPanel.DLPMusic.ArayDevs[cbMusicZone.SelectedIndex * 2 + 1].ToString();
        }

        private void btnSaveHeating_Click(object sender, EventArgs e)
        {

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

        private void cbHeatTempType_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void sbModeChaneover_ValueChanged(object sender, EventArgs e)
        {
            lbModeChangeOverValue.Text = sbModeChaneover.Value.ToString() + "C";
        }

        private void cbHeatSensor4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbHeatSensor4.SelectedIndex == 0)
            {
                NumSub4.Enabled = false;
                NumDev4.Enabled = false;
                NumChn4.Enabled = false;
            }
            else
            {
                NumSub4.Enabled = true;
                NumDev4.Enabled = true;
                NumChn4.Enabled = true;
            }
        }

        private void cbHeatSensor3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbHeatSensor3.SelectedIndex == 0)
            {
                NumSub3.Enabled = false;
                NumDev3.Enabled = false;
                NumChn3.Enabled = false;
                cbMaxTemp.Enabled = false;
            }
            else
            {
                NumSub3.Enabled = true;
                NumDev3.Enabled = true;
                NumChn3.Enabled = true;
                cbMaxTemp.Enabled = true;
            }
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

        private void btnSaveChnInfo_Click(object sender, EventArgs e)
        {
            tsbDown_Click(Upload, null);
        }

        private void btnSaveDim_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            setAllVisible(false);
            try
            {
                oPanel.BaseTempInfomation = new byte[24];
                for (int i = 0; i < dgvTemp.Rows.Count; i++)
                {
                    oPanel.BaseTempInfomation[i * 6] = Convert.ToByte(i + 1);
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
            catch
            {
            }
            tsbDown_Click(Upload, null);
            Cursor.Current = Cursors.Default;
        }

        private void hbTemp_ValueChanged(object sender, EventArgs e)
        {
            lbProTmp.Text = hbTemp.Value.ToString();
            if (isRead) return;
            oPanel.bytChn[1] = Convert.ToByte(hbTemp.Value);
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

        private void addcontrols(int col, int row, Control con, DataGridView dgv)
        {
            con.Show();
            con.Visible = true;
            Rectangle rect = dgv.GetCellDisplayRectangle(col, row, true);
            con.Size = rect.Size;
            con.Top = rect.Top;
            con.Left = rect.Left;
        }

        private void btnSaveBasic_Click(object sender, EventArgs e)
        {
            tsbDown_Click(Upload, null);
        }


        //==========================                  //绘制单元格         
        private void dataGridView1_CellPainting(object sender, System.Windows.Forms.DataGridViewCellPaintingEventArgs e)
        {
            if (e.ColumnIndex == 7 && e.RowIndex >= 0)
            {
                using (Brush gridBrush = new SolidBrush(this.dgvKey.GridColor), backColorBrush = new SolidBrush(e.CellStyle.BackColor))
                {
                    using (Pen gridLinePen = new Pen(gridBrush))
                    {
                        // 擦除原单元格背景                         
                        e.Graphics.FillRectangle(backColorBrush, e.CellBounds);
                    }
                }

                Rectangle re = dgvKey.GetCellDisplayRectangle(7, e.RowIndex, false);
                re.Width = dgvKey.Columns[7].Width;
                re.Height = dgvKey.Rows[e.RowIndex].Height;
                e.Graphics.FillRectangle(Brushes.Yellow, re);
                Pen pen = new Pen(dgvKey.BackgroundColor, 1);
                pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
                if (e.RowIndex % 2 != 0)
                {
                    e.Graphics.DrawLine(pen, re.X, re.Y + re.Height - 1, re.X + re.Width, re.Y + re.Height - 1);//画底下的线
                }
                e.Graphics.DrawLine(pen, re.X + re.Width - 1, re.Y, re.X + re.Width - 1, re.Y + re.Height);//画右边的线

                SizeF strSize = e.Graphics.MeasureString(dgvKey.Rows[e.RowIndex].Cells[7].Value.ToString(), dgvKey.Font);
                e.Graphics.DrawString(dgvKey.Rows[e.RowIndex].Cells[7].Value.ToString(), dgvKey.Font, Brushes.Crimson,
                    re.X + (re.Width - strSize.Width) / 2, re.Y + (re.Height - strSize.Height) / 2 + 5);
                e.Handled = true;
            }
        }

        private void btnTargets_Click(object sender, EventArgs e)
        {
            if (cbPage.SelectedIndex < 0) return;
            
        }

        private void dgvKey_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            if (e.ColumnIndex == 8)
            {
                byte[] arayTmp = new byte[3];
                arayTmp[0] = 18;
                arayTmp[1] = Convert.ToByte(cbPage.SelectedIndex * 8 + e.RowIndex + 1);
                arayTmp[2] = 1;
                string str = dgvKey[2, e.RowIndex].Value.ToString();

                if (CsConst.mySends.AddBufToSndList(arayTmp, 0xE3D8, SubNetID, DeviceID, false, false, false, CsConst.minAllWirelessDeviceType.Contains(mywdDevicerType)) == true)
                {
                    CsConst.myRevBuf = new byte[1200];
                }
            }
            Cursor.Current = Cursors.Default;
        }

        private void btnSaveAC_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                if (oPanel.DLPAC.SaveHVACAddressAndAdjustValue(SubNetID, DeviceID, mywdDevicerType, oPanel.AdjustValue))
                {
                    CsConst.myRevBuf = new byte[1200];
                }
            }
            catch
            {
            }
            Cursor.Current = Cursors.Default;
        }

        private void btnSaveOther_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                //红外控制
                if (oPanel.DLPAC.SaveIrControlHVACFlag(SubNetID, DeviceID, mywdDevicerType) == false) return;

                //红外自动控制
                if (oPanel.DLPAC.SaveIrAutoControlHVACFlag(SubNetID, DeviceID, mywdDevicerType) == false) return;

                if (oPanel.DLPAC.SaveTemperatureSensor(SubNetID, DeviceID, mywdDevicerType) == false) return;
            }
            catch
            {
            }
            Cursor.Current = Cursors.Default;
        }

        private void btnSaveControl_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                byte[] ArayTmp = new byte[8];
                if (oPanel.DLPAC.arayControl != null)
                    Array.Copy(oPanel.DLPAC.arayControl, 0, ArayTmp, 0, 4);
                ArayTmp[4] = oPanel.DLPAC.arayControl[5];
                ArayTmp[5] = oPanel.DLPAC.arayControl[7];
                ArayTmp[6] = oPanel.DLPAC.arayControl[8];
                ArayTmp[7] = oPanel.DLPAC.arayControl[9];
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xE0EE, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(mywdDevicerType)) == true)
                {
                    CsConst.myRevBuf = new byte[1200];
                }
                Cursor.Current = Cursors.Default;
            }
            catch
            {
            }
            Cursor.Current = Cursors.Default;
        }

        void UpdateTemperatureToForm()
        {
            //if (oPanel.bytTempType == 0)
            //{
                sbCoolTemp.Minimum = oPanel.DLPAC.bytTempArea[0];
                sbHeatTemp.Minimum = oPanel.DLPAC.bytTempArea[2];
                sbAutoTemp.Minimum = oPanel.DLPAC.bytTempArea[4];
                sbDryTemp.Minimum = oPanel.DLPAC.bytTempArea[6];
                sbCoolTemp.Maximum = oPanel.DLPAC.bytTempArea[1];
                sbHeatTemp.Maximum = oPanel.DLPAC.bytTempArea[3];
                sbAutoTemp.Maximum = oPanel.DLPAC.bytTempArea[5];
                sbDryTemp.Maximum = oPanel.DLPAC.bytTempArea[7];
            //}
            //else if (oPanel.bytTempType == 1)
            //{
            //    sbCoolTemp.Minimum = 32;
            //    sbHeatTemp.Minimum = 32;
            //    sbAutoTemp.Minimum = 32;
            //    sbDryTemp.Minimum = 32;
            //    sbCoolTemp.Maximum = 86;
            //    sbHeatTemp.Maximum = 86;
            //    sbAutoTemp.Maximum = 86;
            //    sbDryTemp.Maximum = 86;
            //}
        }

        void UpdateWindAndModesToForm()
        {
            if (oPanel.DLPAC.bytAryWind !=  null && oPanel.DLPAC.bytAryWind.Length > 0)
            {
                cbFanSpeed.Items.Clear();
                for (int i = 0; i < oPanel.DLPAC.bytAryWind[0];i++ )
                {
                    //2017
                    cbFanSpeed.Items.Add(CsConst.StatusFAN[oPanel.DLPAC.bytAryWind[i +1]]);
                }
            }

            if (oPanel.DLPAC.bytAryMode != null && oPanel.DLPAC.bytAryMode.Length > 0)
            {
                cbMode.Items.Clear();
                for (int i = 0; i < oPanel.DLPAC.bytAryMode[0]; i++)
                {
                    cbMode.Items.Add(CsConst.StatusAC[oPanel.DLPAC.bytAryMode[i + 1]]);
                }
            }
        }

        private void btnBroadcast_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                byte[] bytTmp = new byte[3];   //自动广播温度
                #region
                bytTmp[0] = Convert.ToByte(oPanel.IsBrdTemp);
                if (oPanel.BrdDev != null)
                {
                    bytTmp[1] = oPanel.BrdDev.SubnetID;
                    bytTmp[2] = oPanel.BrdDev.DeviceID;
                }
                else
                {
                    bytTmp[1] = 255;
                    bytTmp[2] = 255;
                }
                if (CsConst.mySends.AddBufToSndList(bytTmp, 0xE0FA, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(mywdDevicerType)) == true)
                    CsConst.myRevBuf = new byte[1200];
                else return;
                HDLUDP.TimeBetwnNext(bytTmp.Length);
                #endregion

                //温度类型
                #region
                bytTmp = new Byte[]{Convert.ToByte(cbTemp.SelectedIndex)};
                if (CsConst.mySends.AddBufToSndList(bytTmp, 0xE122, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(mywdDevicerType)) == true)
                {
                    oPanel.bytTempType = Convert.ToByte(cbTemp.SelectedIndex);
                }
                #endregion

                oPanel.ModifyPageVisibleOrNotInformation(SubNetID, DeviceID, mywdDevicerType);

                oPanel.ModifyJumpPagesInformationAfterDelay(SubNetID, DeviceID, mywdDevicerType);
            }
            catch
            {
            }
            Cursor.Current = Cursors.Default;
        }

        private void NumAC_ValueChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            if (oPanel == null) return;
            if (oPanel.DLPAC == null) return;
            oPanel.DLPAC.ACNo = Convert.ToByte(NumAC.Value);
        }

        private void txtBroadSub_TextChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            if (oPanel == null) return;
            if (oPanel.BrdDev == null) oPanel.BrdDev = new DeviceInfo("0-0");
            oPanel.BrdDev = new DeviceInfo(txtBroadSub.Text + "-" + txtBroadDev.Text);
        }

        private void txtBroadSub_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar) || e.KeyChar == 8)
                e.Handled = false;
            else
                e.Handled = true;
        }

        private void btnSaveMusic_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                byte[] arayTmp = new byte[3];
                if (rbMusic1.Checked) arayTmp[0] = 1;
                arayTmp[1] = Convert.ToByte(cbMusicZone.SelectedIndex + 1);
                arayTmp[2] = Convert.ToByte(cbMusicMode.SelectedIndex);
                if (oPanel == null) return;
                if (oPanel.DLPMusic == null) return;
                oPanel.DLPMusic.bytEnable = arayTmp[0];
                oPanel.DLPMusic.bytCurZone = arayTmp[1];
                oPanel.DLPMusic.bytMusicType = arayTmp[2];
                tsbDown_Click(Upload, null);

            }
            catch
            {
            }
            Cursor.Current = Cursors.Default;
        }

        private void txtMusicSub_TextChanged(object sender, EventArgs e)
        {
            string str = txtMusicSub.Text;
            txtMusicSub.Text = HDLPF.IsNumStringMode(str, 0, 255);
            txtMusicSub.SelectionStart = txtMusicSub.Text.Length;
            if (isRead) return;
            if (oPanel == null) return;
            if (oPanel.DLPMusic == null) return;
            oPanel.DLPMusic.ArayDevs[cbMusicZone.SelectedIndex * 2] = Convert.ToByte(txtMusicSub.Text);
        }

        private void txtMusicDev_TextChanged(object sender, EventArgs e)
        {
            string str = txtMusicDev.Text;
            txtMusicDev.Text = HDLPF.IsNumStringMode(str, 0, 255);
            txtMusicDev.SelectionStart = txtMusicDev.Text.Length;
            if (isRead) return;
            if (oPanel == null) return;
            if (oPanel.DLPMusic == null) return;
            oPanel.DLPMusic.ArayDevs[cbMusicZone.SelectedIndex * 2 + 1] = Convert.ToByte(txtMusicDev.Text);
        }

        private void dgvMusic_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            Byte[] PageID = new Byte[4]{200,0,0,0};

            HDLSysPF.CopyCMDToPublicBufferWaitPasteOrCopyToGrid(dgvMusic);

            frmCmdSetup CmdSetup = new frmCmdSetup(oPanel, MyDevName, mywdDevicerType, PageID);
            CmdSetup.ShowDialog();

            if (CmdSetup.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                oPanel.DLPMusic.KeyTargets = CmdSetup.TempCMDGroup.ToArray();
                HDLSysPF.PasteCMDToPublicBufferWaitPasteOrCopyToGrid(dgvMusic);
            }
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
                oPanel.DLPFH.OutDoorParam[0] = Convert.ToByte(cbHeatSensor1.SelectedIndex);
                oPanel.DLPFH.OutDoorParam[1] = Convert.ToByte(NumSub1.Value);
                oPanel.DLPFH.OutDoorParam[2] = Convert.ToByte(NumDev1.Value);
                oPanel.DLPFH.OutDoorParam[3] = Convert.ToByte(NumChn1.Value);

                oPanel.DLPFH.OutDoorParam[4] = Convert.ToByte(cbHeatSensor2.SelectedIndex);
                oPanel.DLPFH.OutDoorParam[5] = Convert.ToByte(NumSub2.Value);
                oPanel.DLPFH.OutDoorParam[6] = Convert.ToByte(NumDev2.Value);
                oPanel.DLPFH.OutDoorParam[7] = Convert.ToByte(NumChn2.Value);

                if (oPanel.DLPFH.SourceParam == null) oPanel.DLPFH.SourceParam = new byte[8];
                oPanel.DLPFH.SourceParam[0] = Convert.ToByte(cbHeatSensor3.SelectedIndex);
                oPanel.DLPFH.SourceParam[1] = Convert.ToByte(NumSub3.Value);
                oPanel.DLPFH.SourceParam[2] = Convert.ToByte(NumDev3.Value);
                oPanel.DLPFH.SourceParam[3] = Convert.ToByte(NumChn3.Value);
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

        void DisplayCurrentStatusFloorheating()
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

        private void cbFont_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            if (oPanel == null) return;
            oPanel.TimeFontSize = Convert.ToByte(cbFont.SelectedIndex);
        }

        private void cbTempSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            if (oPanel == null) return;
            oPanel.TemperatureSource = Convert.ToByte(cbTempSource.SelectedIndex);
        }


        private void cbBaseDimming_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            oPanel.bytChn[2] = Convert.ToByte(cbBaseDimming.SelectedIndex);
        }

        private void btnSaveAndClose_Click(object sender, EventArgs e)
        {
            tsbDown_Click(Upload, null);
            this.Close();
        }

        private void btnRefreshPage_Click(object sender, EventArgs e)
        {
            tsbDown_Click(tsbDown, null);
        }

        private void btnRefresh3_Click(object sender, EventArgs e)
        {
            tsbDown_Click(tsbDown, null);
        }

        private void btnSaveAndClose3_Click(object sender, EventArgs e)
        {
            tsbDown_Click(Upload, null);
            this.Close();
        }

        private void btnSave3_Click(object sender, EventArgs e)
        {
            tsbDown_Click(Upload, null);
        }

        private void btnRefresh2_Click(object sender, EventArgs e)
        {
            tsbDown_Click(tsbDown, null);
        }

        private void btnSaveAndClose2_Click(object sender, EventArgs e)
        {
            tsbDown_Click(Upload, null);
            this.Close();
        }

        private void btnRefresh4_Click(object sender, EventArgs e)
        {
            tsbDown_Click(tsbDown, null);
        }

        private void btnSaveAndClose4_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            byte[] arayTmp = new byte[3];
            if (rbMusic1.Checked) arayTmp[0] = 1;
            arayTmp[1] = Convert.ToByte(cbMusicZone.SelectedIndex + 1);
            arayTmp[2] = Convert.ToByte(cbMusicMode.SelectedIndex);
            if (oPanel == null) return;
            if (oPanel.DLPMusic == null) return;
            oPanel.DLPMusic.bytEnable = arayTmp[0];
            oPanel.DLPMusic.bytCurZone = arayTmp[1];
            oPanel.DLPMusic.bytMusicType = arayTmp[2];
            tsbDown_Click(Upload, null);
            this.Close();
        }

        private void btnSave4_Click(object sender, EventArgs e)
        {
            tsbDown_Click(Upload, null);
        }

        private void btnRefresh5_Click(object sender, EventArgs e)
        {
            tsbDown_Click(tsbDown, null);
        }

        private void btnSaveAndClose5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnRefresh6_Click(object sender, EventArgs e)
        {
            tsbDown_Click(tsbDown, null);
        }

        private void btnSaveAndClose6_Click(object sender, EventArgs e)
        {
            tsbDown_Click(Upload, null);
            this.Close();
        }

        private void btnSave6_Click(object sender, EventArgs e)
        {
            tsbDown_Click(Upload, null);
        }

        private void btnSave7_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            setAllVisible(false);
            try
            {
                if (oPanel.BaseTempInfomation == null) oPanel.BaseTempInfomation = new byte[25];
                if (groupBox1.Visible)
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
            tsbDown_Click(Upload, null);
            Cursor.Current = Cursors.Default;
        }

        private void btnSaveAndClose7_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            setAllVisible(false);
            try
            {
                oPanel.BaseTempInfomation = new byte[24];
                for (int i = 0; i < dgvTemp.Rows.Count; i++)
                {
                    oPanel.BaseTempInfomation[i * 6] = Convert.ToByte(i + 1);
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
            catch
            {
            }
            tsbDown_Click(Upload, null);
            Cursor.Current = Cursors.Default;
            this.Close();
        }

        private void btnRefresh7_Click(object sender, EventArgs e)
        {
            tsbDown_Click(tsbDown, null);
        }

        private void lbLTime_Click(object sender, EventArgs e)
        {

        }

        private void rbMusic1_CheckedChanged(object sender, EventArgs e)
        {
            if (oPanel == null) return;
            if (oPanel.DLPMusic == null) return;
            if (isRead) return;
            if (rbMusic1.Checked)
                oPanel.DLPMusic.bytEnable = 1;
            else
                oPanel.DLPMusic.bytEnable = 0;
        }

        private void cbMusicMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbMusicMode.Items.Count == 3 && cbMusicMode.SelectedIndex == 2)
            {
                cbMusicZone_SelectedIndexChanged(null, null);
            }
            else
            {

            }
            if (oPanel == null) return;
            if (oPanel.DLPMusic == null) return;
            if (isRead) return;
            oPanel.DLPMusic.bytMusicType = Convert.ToByte(cbMusicMode.SelectedIndex);
        }

        private void DgChns_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                setAllVisible(false);
                if (e.RowIndex >= 0)
                {
                    sbTest.Text = HDLPF.IsNumStringMode(DgChns[4, e.RowIndex].Value.ToString(), 0, 100);
                    addcontrols(4, e.RowIndex, sbTest, DgChns);
                }
            }
            catch
            {
            }
        }

        private void chbAutoLock_CheckedChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            if (oPanel.otherInfo == null) return;
            if (chbAutoLock.Checked) oPanel.otherInfo.AutoLock = 1;
            else oPanel.otherInfo.AutoLock = 0;
        }

        private void cbServer1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cbServer2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void dgvKey_SizeChanged(object sender, EventArgs e)
        {
            
        }

        private void dgvMusic_SizeChanged(object sender, EventArgs e)
        {
            
        }

        private void DgChns_SizeChanged(object sender, EventArgs e)
        {
            
        }

        private void dgvAddress_SizeChanged(object sender, EventArgs e)
        {
            HDLSysPF.setDataGridViewColumnsWidth(dgvAddress);
        }

        private void dgvTemp_SizeChanged(object sender, EventArgs e)
        {
            HDLSysPF.setDataGridViewColumnsWidth(dgvTemp);
        }

        private void Next_Click(object sender, EventArgs e)
        {
            if (cbPage.SelectedIndex < cbPage.Items.Count - 1) cbPage.SelectedIndex++;
        }

        private void Pre_Click(object sender, EventArgs e)
        {
            if (cbPage.SelectedIndex != 0) cbPage.SelectedIndex--;
        }

        private void btnMode_Click(object sender, EventArgs e)
        {
            Byte[] TmpCurrentSelections = new Byte[] { (Byte)dgvKey.CurrentCell.RowIndex, (Byte)cbPage.SelectedIndex };
            frmButtonSetup ButtonConfigration = new frmButtonSetup(oPanel, MyDevName, TmpCurrentSelections);
            ButtonConfigration.ShowDialog();
        }

        private void btnCMD_Click(object sender, EventArgs e)
        {
            Byte[] PageID = new Byte[4];
            if (dgvKey.SelectedRows != null && dgvKey.SelectedRows.Count > 0)
            {
                PageID[0] = (Byte)dgvKey.SelectedRows[0].Index;
            }
            PageID[1] = Convert.ToByte(cbPage.SelectedIndex);
            frmCmdSetup CmdSetup = new frmCmdSetup(oPanel, MyDevName, mywdDevicerType, PageID);
            CmdSetup.ShowDialog();
        }

        private void dgvKey_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (isRead) return;
            if (e.RowIndex ==-1 || e.ColumnIndex == -1) return;
            if (dgvKey.CurrentRow.Index < 0) return;
            if (dgvKey.RowCount <= 0) return;
            int index = dgvKey.CurrentRow.Index;

            Byte PageID = 0;
            if (cbPage.Visible) PageID = Convert.ToByte(cbPage.SelectedIndex);
            int wdMaxValue = int.Parse(CsConst.mstrINIDefault.IniReadValue("DeviceType" + mywdDevicerType.ToString(), "MaxValue", "0"));

            Cursor.Current = Cursors.WaitCursor;

            if (dgvKey[e.ColumnIndex, e.RowIndex].Value == null)  dgvKey[e.ColumnIndex, e.RowIndex].Value = "";
            string strTmp = dgvKey[e.ColumnIndex, e.RowIndex].Value.ToString();
            for (int i = 0; i < dgvKey.SelectedRows.Count; i++)
            {
                int RowID =dgvKey.SelectedRows[i].Index;
                dgvKey.SelectedRows[i].Cells[e.ColumnIndex].Value = strTmp;
                switch (e.ColumnIndex)
                {
                    case 1:
                        if (dgvKey[1, index].Value == null) dgvKey[1, index].Value = "";
                        String ButtonRemark = dgvKey[1, index].Value.ToString();
                        oPanel.PanelKey[RowID + PageID * wdMaxValue].Remark = ButtonRemark;
                        break;
                    case 2:
                        oPanel.PanelKey[RowID + PageID * wdMaxValue].Mode = ButtonMode.ConvertorKeyModesToPublicModeGroup(dgvKey[2, index].Value.ToString());
                        break;
                    case 3:
                        byte[] arayTmp = new byte[3];
                        arayTmp[0] = 18;
                        arayTmp[1] = Convert.ToByte(e.RowIndex + 1 + PageID * wdMaxValue);
                        arayTmp[2] = 1;
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
            Cursor.Current = Cursors.Default;
        }

        private void dgvKey_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            dgvKey.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void btnRefresh2_Click_1(object sender, EventArgs e)
        {
            tsbDown_Click(tsbDown, null);
        }

        private void btnSaveKey_Click_1(object sender, EventArgs e)
        {
            tsbDown_Click(Upload, null);
        }

        private void btnSaveAndClose2_Click_1(object sender, EventArgs e)
        {
            tsbDown_Click(Upload, null);
            this.Close();
        }

        private void btnUI_Click(object sender, EventArgs e)
        {
            FrmACSetup ACSetup = new FrmACSetup(MyDevName, oPanel, mywdDevicerType);
            ACSetup.ShowDialog();
            if (ACSetup.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                UpdateTemperatureToForm();
                UpdateWindAndModesToForm();
                btnRefControl_Click(btnRefControl, null);
            }
        }

        private void sbHeatTemp_Scroll(object sender, ScrollEventArgs e)
        {
           
        }

        private void btnTSensor_Click(object sender, EventArgs e)
        {
            frmTSensor ACSetup = new frmTSensor(oPanel, 2, CsConst.ButtonSetupGroup,CsConst.TemperatureGroup, MyDevName);
            ACSetup.ShowDialog();
        }

        public void DisplayTemperatureSensorInformation()
        {
            lvSensor.Items.Clear();
            rbIN.Checked = (oPanel.DLPAC.bytTempSensor == 0);
            rbBus.Checked = (oPanel.DLPAC.bytTempSensor == 1);
            rbBOTH.Checked = (oPanel.DLPAC.bytTempSensor == 2);

            if (rbBus.Checked || rbBOTH.Checked)
            {
                for (int i = 0; i < 8; i++)// (DeviceInfo oTmp in oPanel.DLPAC.TempSensors)
                {
                    if (oPanel.DLPAC.TempSensors[i * 4] != 0)
                    {
                        ListViewItem tmp = new ListViewItem();
                        tmp.Checked = true;
                        string strRemark = HDLSysPF.GetRemarkAccordingly(oPanel.DLPAC.TempSensors[i * 4 + 1], oPanel.DLPAC.TempSensors[i * 4 + 2],-1);
                        tmp.SubItems.Add(strRemark);
                        tmp.SubItems.Add(oPanel.DLPAC.TempSensors[i * 4 + 3].ToString());
                        lvSensor.Items.Add(tmp);
                    }
                }
            }
        }

        private void rbIN_CheckedChanged(object sender, EventArgs e)
        {
            lvSensor.Enabled = !rbIN.Checked;

            byte bytTag = Convert.ToByte(((RadioButton)sender).Tag.ToString());

            if (((RadioButton)sender).Checked)
            {
                oPanel.DLPAC.bytTempSensor = bytTag;
            }
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
           // lvSensorFh.Enabled = !(rbHeat1.Checked);
           // btnTempFh.Enabled = lvSensorFh.Enabled;
        }

        private void btnTempFh_Click(object sender, EventArgs e)
        {
            frmTSensor ACSetup = new frmTSensor(oPanel, 2,CsConst.ButtonSetupGroup,  CsConst.TemperatureGroup, MyDevName);
            ACSetup.ShowDialog();
        }

        private void btnRelay_Click(object sender, EventArgs e)
        {
            FrmCalculateTargetForColorDLP frmTmp = new FrmCalculateTargetForColorDLP(MyDevName, mywdDevicerType, oPanel, 0);
            frmTmp.ShowDialog();
        }

        private void cbOutput_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnRelay.Enabled = (cbOutput.SelectedIndex == 0);
        }

        private void btnMoreFh_Click(object sender, EventArgs e)
        {
            Byte[] PageID = new Byte[4] { 101, 0, 0, 0 };

            frmCmdSetup CmdSetup = new frmCmdSetup(oPanel, MyDevName, mywdDevicerType, PageID);
            CmdSetup.ShowDialog();
        }

        private void cbHeatType_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnMoreFh.Enabled = (cbHeatType.SelectedIndex !=0);
            grpHeatTemp.Enabled = btnMoreFh.Enabled;
            grpStatus.Enabled = btnMoreFh.Enabled;
            grpWorking.Enabled = btnMoreFh.Enabled;
        }

        private void btnStatusFH_Click(object sender, EventArgs e)
        {
            //读取
            oPanel.DLPFH.ReadCurrentStatusFromFloorheatingModule(SubNetID, DeviceID, mywdDevicerType);
            //界面处理
            DisplayCurrentStatusFloorheating();
        }

        private void btnModifyFH_Click(object sender, EventArgs e)
        {
            UpdateFloorheatingCurrentStatusToStruct();
            oPanel.DLPFH.ModifyCurrentStatusFromFloorheatingModule(SubNetID, DeviceID, mywdDevicerType, oPanel.bytTempType);
        }

        private void chbSleepList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            if (oPanel == null) return;
            if (oPanel.araySleep == null) oPanel.araySleep = new Byte[6];

            if (chbSleep.Checked) oPanel.araySleep[1] = 1;
            else oPanel.araySleep[1] = 0;
            string str = "";
            for (int i = 31; i >= 0; i--)
            {
                if (chbSleepList.GetItemChecked(i) == true)
                    str = str + "1";
                else
                    str = str + "0";
            }
            oPanel.araySleep[5] = Convert.ToByte(str.Substring(0, 8), 2);
            oPanel.araySleep[4] = Convert.ToByte(str.Substring(1 * 8, 8), 2);
            oPanel.araySleep[3] = Convert.ToByte(str.Substring(2 * 8, 8), 2);
            oPanel.araySleep[2] = Convert.ToByte(str.Substring(3 * 8, 8), 2);
        }

        private void chbSensor_CheckedChanged(object sender, EventArgs e)
        {
            if (oPanel.otherInfo == null) oPanel.otherInfo = new Panel.OtherInfo();

            if (chbSensor.Checked == true) oPanel.otherInfo.IRCloseSensor = 1;
            else oPanel.otherInfo.IRCloseSensor = 0;

            cbIRTarget.Enabled = chbSensor.Checked;
            cbSensitivity.Enabled = chbSensor.Checked;
        }

        private void chbSleep_CheckedChanged(object sender, EventArgs e)
        {
            if (chbSleep.Checked) oPanel.araySleep[1] = 1;
            else oPanel.araySleep[1] = 0;

            chbSleepList.Enabled = chbSleep.Checked;
        }

        private void cbIRTarget_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            if (oPanel == null) return;
            if (oPanel.otherInfo == null) oPanel.otherInfo = new Panel.OtherInfo();
            oPanel.otherInfo.IRCloseTargets = Convert.ToByte(cbIRTarget.SelectedIndex);
        }

        private void cbSensitivity_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            if (oPanel == null) return;
            if (oPanel.otherInfo == null) oPanel.otherInfo = new Panel.OtherInfo();
            oPanel.otherInfo.CloseToSensorSensitivity = Convert.ToByte(cbSensitivity.SelectedIndex);
        }

        private void chbSound_CheckedChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            if (oPanel == null) return;
            if (oPanel.otherInfo == null) oPanel.otherInfo = new Panel.OtherInfo();
            if (chbSound.Checked == true) oPanel.otherInfo.SoundClick = 1;
            else oPanel.otherInfo.SoundClick = 0;
        }

        private void dgvBalance_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                setAllVisible(false);
                if (e.RowIndex >= 0)
                {
                    DisplayBackgroudColor(e.RowIndex);
                }
            }
            catch
            {
            }
        }

        private void dgvBalance_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            dgvBalance.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void dgvBalance_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {

        }

        void UpdateColorAndSensitivitySetup()
        {
            Cursor.Current = Cursors.WaitCursor;
            Byte EachPacketSize = 54;
            Byte TotalColumns = (Byte)(dgvBalance.ColumnCount -1);

            oPanel.arayButtonBalance[0] = Convert.ToByte(dgvBalance.RowCount * 3);
            for (int i = 0; i < dgvBalance.RowCount; i++)
            {
                oPanel.arayButtonBalance[i * 3 + 1] = Convert.ToByte(dgvBalance[1, i].Value.ToString());
                oPanel.arayButtonBalance[i * 3 + 2] = Convert.ToByte(dgvBalance[2, i].Value.ToString());
                oPanel.arayButtonBalance[i * 3 + 3] = Convert.ToByte(dgvBalance[3, i].Value.ToString());
            }

            oPanel.arayButtonColor = new byte[7 * EachPacketSize + 9];
            oPanel.arayButtonColor[0] = 7;
            for (int i = 0; i < dgvBalance.RowCount; i++)
            {
                for (int ColumnID = 0; ColumnID < TotalColumns - 4; ColumnID++)
                {
                    oPanel.arayButtonColor[9 + i * 6 + ColumnID * EachPacketSize] = dgvBalance[ColumnID + 4, i].Style.BackColor.R;
                    oPanel.arayButtonColor[9 + i * 6 + ColumnID * EachPacketSize + 1] = dgvBalance[ColumnID + 4, i].Style.BackColor.G;
                    oPanel.arayButtonColor[9 + i * 6 + ColumnID * EachPacketSize + 2] = dgvBalance[ColumnID + 4, i].Style.BackColor.B;
                    oPanel.arayButtonColor[9 + i * 6 + ColumnID * EachPacketSize + 3] = dgvBalance[TotalColumns, i].Style.BackColor.R;
                    oPanel.arayButtonColor[9 + i * 6 + ColumnID * EachPacketSize + 4] = dgvBalance[TotalColumns, i].Style.BackColor.G;
                    oPanel.arayButtonColor[9 + i * 6 + ColumnID * EachPacketSize + 5] = dgvBalance[TotalColumns, i].Style.BackColor.B;
                }
            }
        }

        private void cbPowerOn_CheckedChanged(object sender, EventArgs e)
        {
            if (cbMode.SelectedIndex == 4)
            {
                cbFanSpeed.SelectedIndex = 3;
                cbFanSpeed.Enabled = false;
            }
            else
            {
                cbFanSpeed.Enabled = true;
            }
            SetControlInfo();
        }

        private void btnSYN_Click(object sender, EventArgs e)
        {
            FrmSlave frmTmp = new FrmSlave(MyDevName, oPanel, mywdDevicerType);
            frmTmp.ShowDialog();
        }

        private void lvSensor_MouseDoubleClick(object sender, MouseEventArgs e)
        {

        }

        private void btnSlaves_Click(object sender, EventArgs e)
        {
            FrmHeatTargets frmTmp = new FrmHeatTargets(MyDevName, mywdDevicerType, oPanel);
            frmTmp.ShowDialog();
        }

        private void cbMinPWM_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnIrList_Click(object sender, EventArgs e)
        {
            FrmInfrared frmTmp = new FrmInfrared(MyDevName, mywdDevicerType, oPanel,1);
            frmTmp.ShowDialog();
           
        }

        private void btnCombination_Click(object sender, EventArgs e)
        {
            FrmCombinationWays frmTmp = new FrmCombinationWays(MyDevName, mywdDevicerType, cbPage.SelectedIndex, oPanel);
            frmTmp.ShowDialog();
        }

        private void cbHeatSensor1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void cbTemp_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbTemp.SelectedIndex == -1) return;
            oPanel.bytTempType = (Byte)(cbTemp.SelectedIndex);
        }
    }
}
