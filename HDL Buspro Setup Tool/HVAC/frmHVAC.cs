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
    public partial class frmHVAC : Form
    {
        [DllImport("user32.dll")]
        public static extern Boolean FlashWindow(IntPtr hwnd, Boolean bInvert);

        private HVAC myHVAC = null;
        private int myintDIndex = -1;
        private string myDevName = null;
        private bool isRead = false;
        private int mywdDeviceType = -1;
        private int MyActivePage = 0; //按页面上传下载
        private byte SubNetID;
        private byte DevID;
        public frmHVAC()
        {
            InitializeComponent();
        }


        public frmHVAC(HVAC oHVAC, string strName, int intDIndex, int intDeviceType)
        {
            InitializeComponent();

            this.myHVAC = oHVAC;
            this.myDevName = strName;
            this.myintDIndex = intDIndex;
            this.mywdDeviceType = intDeviceType;
            string strDevName = strName.Split('\\')[0].ToString();

            HDLSysPF.DisplayDeviceNameModeDescription(strName, mywdDeviceType, cboDevice, tbModel, tbDescription);

            SubNetID = Convert.ToByte(strDevName.Split('-')[0].ToString());
            DevID = Convert.ToByte(strDevName.Split('-')[1].ToString());
            this.Text = strName;
            tsl31.Text = strName;
        }


        private void frmHVAC_Load(object sender, EventArgs e)
        {
            AddComboboxItem();            
        }

        private void AddComboboxItem()
        {
            cbPower.Items.Clear();
            cbPower.Items.Add(CsConst.Status[0]);
            cbPower.Items.Add(CsConst.Status[1]);
            cbSpeed.Items.Clear();
            for (int i = 0; i < 4; i++)
                cbSpeed.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "0006" + i.ToString(), ""));
            cbMode.Items.Clear();
            for (int i = 0; i < 5; i++)
                cbMode.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "0007" + i.ToString(), ""));
            cbOper.Items.Clear();
            cbOper.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00771", ""));
            cbOper.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00770", ""));
            cbSlaveChn.Items.Clear();
            cbSlaveChn.Items.Add("1");
            cbSlaveChn.Items.Add("2");
            cbSlaveChn.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99533", ""));
            cbDelay2.Items.Clear();
            for (int i = 1; i <= 10; i++)
                cbDelay2.Items.Add(i.ToString());
            cbDelay3.Items.Clear();
            for (int i = 3; i <= 300; i++)
                cbDelay3.Items.Add(i.ToString());
            cbWind1.Items.Clear();
            cbWind2.Items.Clear();
            cbWind3.Items.Clear();
            for (int i = 0; i <= 10; i++)
            {
                cbWind1.Items.Add(i.ToString());
                cbWind2.Items.Add(i.ToString());
                cbWind3.Items.Add(i.ToString());
            }
            cbWind4.Items.Clear();
            cbWind4.Items.Add(CsConst.WholeTextsList[1681].sDisplayName);
            cbWind4.Items.Add(CsConst.WholeTextsList[2339].sDisplayName);
            cbTime1.Items.Clear();
            for (int i = 1; i <= 120; i++)
                cbTime1.Items.Add(i.ToString());
            cbTime2.Items.Clear();
            for (int i = 0; i <= 10; i++)
                cbTime2.Items.Add(i.ToString());
            cbOperType.Items.Clear();
            cbOperType.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00771", ""));
            cbOperType.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00770", ""));
        }

        private void frmHVAC_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Dispose();
        }

        private void tslRead_Click(byte bytTag)
        {
            bool blnShowMsg = (CsConst.MyEditMode != 1);
            if (HDLPF.UploadOrReadGoOn(this.Text, bytTag, blnShowMsg))
            {
                Cursor.Current = Cursors.WaitCursor;
                UpdateActivePageIndexFromUint();
                CsConst.MyUPload2DownLists = new List<byte[]>();

                string strName = myDevName.Split('\\')[0].ToString();
                byte bytSubID = byte.Parse(strName.Split('-')[0]);
                byte bytDevID = byte.Parse(strName.Split('-')[1]);

                byte[] ArayRelay = new byte[] { bytSubID, bytDevID, (byte)(mywdDeviceType / 256), (byte)(mywdDeviceType % 256), 
                    (byte)MyActivePage,(byte)(myintDIndex/256),(byte)(myintDIndex%256) };
                CsConst.MyUPload2DownLists.Add(ArayRelay);
                CsConst.MyUpload2Down = bytTag;
                FrmDownloadShow Frm = new FrmDownloadShow();
                if (bytTag ==0) Frm.FormClosing += new FormClosingEventHandler(UpdateDisplayInformationAccordingly);
                Frm.ShowDialog();
            }
        }

        void UpdateDisplayInformationAccordingly(object sender, FormClosingEventArgs e)
        {
            if (tab12in1.SelectedTab.Name == "tabBasic")
            {
                showBasicInfo();
            }
            else if (tab12in1.SelectedTab.Name =="tabRelay")
            {
                showACSetupInfo();
            }
            else if (tab12in1.SelectedTab.Name == "tabOther")
            {
                showOtherInfo();
            }
            else if (tab12in1.SelectedTab.Name == "tabAcSetup")
            {
 
            }
            Cursor.Current = Cursors.Default;
            this.TopMost = true;
            this.WindowState = FormWindowState.Normal;
            this.Activate();
            this.TopMost = false;
        }

        private void showBasicInfo()
        {
            isRead = true;
            try
            {
                if (1 <= myHVAC.aryAirTime[0] && myHVAC.aryAirTime[0] <= 10) cbD1.SelectedIndex = myHVAC.aryAirTime[0] - 1;
                if (1 <= myHVAC.aryAirTime[1] && myHVAC.aryAirTime[1] <= 10) cbD2.SelectedIndex = myHVAC.aryAirTime[1] - 1;
                if (cbD1.SelectedIndex < 0) cbD1.SelectedIndex = 0;
                if (cbD2.SelectedIndex < 0) cbD2.SelectedIndex = 0;
                if (panel18.Visible)
                {
                    if (myHVAC.aryAirTime[2] > 128)
                    {
                        rbS.Checked = true;
                        rbM_CheckedChanged(rbS, null);
                        cbDelay1.Text = (myHVAC.aryAirTime[2] - 128).ToString();
                    }
                    else
                    {
                        rbM.Checked = true;
                        rbM_CheckedChanged(rbM, null);
                        cbDelay1.Text = myHVAC.aryAirTime[2].ToString();
                    }
                    cbDelay2.Text = myHVAC.aryAirTime[3].ToString();
                    cbDelay3.Text = myHVAC.aryAirTime[4].ToString();
                }

                if (grpVoltage.Visible)
                {
                    cbWind1.SelectedIndex = myHVAC.arayWindEle[0];
                    cbWind2.SelectedIndex = myHVAC.arayWindEle[1];
                    cbWind3.SelectedIndex = myHVAC.arayWindEle[2];
                    cbWind4.SelectedIndex = myHVAC.arayWindEle[3];
                }
            }
            catch
            {
            }
            isRead = false;
        }

        private void showACSetupInfo()
        {
            isRead = true;
            try
            {
                if (myHVAC.AryACSetup[0] <= 1) cbOper.SelectedIndex = myHVAC.AryACSetup[0];
                if (HDLSysPF.GetBit(myHVAC.AryACSetup[1], 0) == 1) chbDS1.Checked = true;
                else chbDS1.Checked = false;
                if (HDLSysPF.GetBit(myHVAC.AryACSetup[1], 1) == 1) chbBus1.Checked = true;
                else chbBus1.Checked = false;
                txtDS1.Text = myHVAC.AryACSetup[2].ToString();
                if (sbTemp1.Minimum <= myHVAC.AryACSetup[3] && myHVAC.AryACSetup[3] <= sbTemp1.Maximum)
                    sbTemp1.Value = myHVAC.AryACSetup[3];
                txtSub1.Text = myHVAC.AryACSetup[4].ToString();
                txtDev1.Text = myHVAC.AryACSetup[5].ToString();
                txtChn1.Text = myHVAC.AryACSetup[6].ToString();
                if (HDLSysPF.GetBit(myHVAC.AryACSetup[7], 0) == 1) chbDS2.Checked = true;
                else chbDS2.Checked = false;
                if (HDLSysPF.GetBit(myHVAC.AryACSetup[7], 1) == 1) chbBus2.Checked = true;
                else chbBus2.Checked = false;
                txtDS2.Text = myHVAC.AryACSetup[8].ToString();
                if (sbTemp2.Minimum <= myHVAC.AryACSetup[9] && myHVAC.AryACSetup[9] <= sbTemp2.Maximum)
                    sbTemp2.Value = myHVAC.AryACSetup[9];
                txtSub2.Text = myHVAC.AryACSetup[10].ToString();
                txtDev2.Text = myHVAC.AryACSetup[11].ToString();
                txtChn2.Text = myHVAC.AryACSetup[12].ToString();
            }
            catch
            {
            }
            isRead = false;
            chbDS1_CheckedChanged(null, null);
            chbBus1_CheckedChanged(null, null);
            chbDS2_CheckedChanged(null, null);
            chbBus2_CheckedChanged(null, null);
            sbTemp1_ValueChanged(null, null);
            sbTemp2_ValueChanged(null, null);
        }

        private void showOtherInfo()
        {
            isRead = true;
            try
            {
                if (HVACModuleDeviceTypeList.HVACG2ComplexRelayDeviceTypeLists.Contains(mywdDeviceType))
                {
                    chbHost.Checked = (myHVAC.AryHost[0] == 1);
                    if (1 <= myHVAC.AryHost[1] && myHVAC.AryHost[1] <= 2)
                        cbHost.SelectedIndex = (myHVAC.AryHost[1] - 1);
                    else
                        cbHost.SelectedIndex = 0;
                }
                else
                {
                    chbHost.Checked = (myHVAC.AryHost[1] == 1);
                }
                if (cbSlave.SelectedIndex < 0) cbSlave.SelectedIndex = 0;
            }
            catch
            {
            }
            isRead = false;
            cbSlave_SelectedIndexChanged(null, null);
        }

        private void btnRef1_Click(object sender, EventArgs e)
        {
            tslRead_Click(0);
        }

        private void btnSave1_Click(object sender, EventArgs e)
        {
            tslRead_Click(1);
        }

        private void btnSaveAndClose1_Click(object sender, EventArgs e)
        {
            tslRead_Click(1);
            this.Close();
        }

        private void tmRead_Click(object sender, EventArgs e)
        {
            tslRead_Click(0);
        }

        /// <summary>
        /// 刷新Test页面按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRead_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                isRead = true;
                byte[] arayTmp = new byte[1];
                if (cbACNO.SelectedIndex < 0) cbACNO.SelectedIndex = 0;
                arayTmp[0] = Convert.ToByte(cbACNO.SelectedIndex + 1);
                if (CsConst.mySends.AddBufToSndList(arayTmp, 0x1938, SubNetID, DevID, false, true, true, false))
                {
                    if (CsConst.myRevBuf[25] < cbACNO.Items.Count) cbACNO.SelectedIndex = (CsConst.myRevBuf[25] - 1);
                    if (CsConst.myRevBuf[26] == 0)
                    {
                        lbTempTypeValue.Text = "C";
                        for (int i = 1; i <= 4; i++)
                        {
                            HScrollBar sb = this.Controls.Find("sbHeatTemp" + i.ToString(), true)[0] as HScrollBar;
                            sb.Minimum = 0;
                            sb.Maximum = 30;
                        }
                    }
                    else if (CsConst.myRevBuf[26] == 1)
                    {
                        lbTempTypeValue.Text = "F";
                        for (int i = 1; i <= 4; i++)
                        {
                            HScrollBar sb = this.Controls.Find("sbHeatTemp" + i.ToString(), true)[0] as HScrollBar;
                            sb.Minimum = 32;
                            sb.Maximum = 86;
                        }
                    }
                    lbCurTempValue.Text = CsConst.myRevBuf[27].ToString() + " " + lbTempTypeValue.Text;
                    if (sbHeatTemp1.Minimum <= CsConst.myRevBuf[28] && CsConst.myRevBuf[28] <= sbHeatTemp1.Maximum)
                        sbHeatTemp1.Value = CsConst.myRevBuf[28];
                    if (sbHeatTemp2.Minimum <= CsConst.myRevBuf[29] && CsConst.myRevBuf[29] <= sbHeatTemp2.Maximum)
                        sbHeatTemp2.Value = CsConst.myRevBuf[29];
                    if (sbHeatTemp3.Minimum <= CsConst.myRevBuf[30] && CsConst.myRevBuf[30] <= sbHeatTemp3.Maximum)
                        sbHeatTemp3.Value = CsConst.myRevBuf[30];
                    if (sbHeatTemp4.Minimum <= CsConst.myRevBuf[31] && CsConst.myRevBuf[31] <= sbHeatTemp4.Maximum)
                        sbHeatTemp4.Value = CsConst.myRevBuf[31];
                    if (CsConst.myRevBuf[33] == 1) cbPower.SelectedIndex = 1;
                    else cbPower.SelectedIndex = 0;
                    if (CsConst.myRevBuf[34] < cbMode.Items.Count) cbMode.SelectedIndex = CsConst.myRevBuf[34];
                    if (CsConst.myRevBuf[35] < cbSpeed.Items.Count) cbSpeed.SelectedIndex = CsConst.myRevBuf[35];
                    if (cbACNO.SelectedIndex < 0) cbACNO.SelectedIndex = 0;
                    if (cbMode.SelectedIndex < 0) cbMode.SelectedIndex = 0;
                    if (cbSpeed.SelectedIndex < 0) cbSpeed.SelectedIndex = 0;
                }
            }
            catch
            {
            }
            isRead = false;
            Cursor.Current = Cursors.Default;
            sbHeatTemp1_ValueChanged(sbHeatTemp1, null);
            sbHeatTemp1_ValueChanged(sbHeatTemp2, null);
            sbHeatTemp1_ValueChanged(sbHeatTemp3, null);
            sbHeatTemp1_ValueChanged(sbHeatTemp4, null);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                byte[] arayTmp = new byte[13];
                arayTmp[0] = Convert.ToByte(cbACNO.SelectedIndex + 1);
                if (lbTempTypeValue.Text == "C") arayTmp[1] = 0;
                else if (lbTempTypeValue.Text == "F") arayTmp[1] = 1;
                arayTmp[3] = Convert.ToByte(sbHeatTemp1.Value);
                arayTmp[4] = Convert.ToByte(sbHeatTemp2.Value);
                arayTmp[5] = Convert.ToByte(sbHeatTemp3.Value);
                arayTmp[6] = Convert.ToByte(sbHeatTemp4.Value);
                arayTmp[8] = Convert.ToByte(cbPower.SelectedIndex);
                arayTmp[9] = Convert.ToByte(cbMode.SelectedIndex);
                arayTmp[10] = Convert.ToByte(cbSpeed.SelectedIndex);
                CsConst.mySends.AddBufToSndList(arayTmp, 0x193A, SubNetID, DevID, false, true, true, false);
            }
            catch
            {
            }
            Cursor.Current = Cursors.Default;
        }

        private void sbHeatTemp1_ValueChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            //int Tag = Convert.ToInt32((sender as HScrollBar).Tag);
            //Label lb = this.Controls.Find("lbHeatTempValue" + Tag.ToString(), true)[0] as Label;
            //lb.Text = (sender as HScrollBar).Value.ToString() + lbTempTypeValue.Text;     
        }

        private void cbACNO_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            btnRead_Click(null, null);
        }

        void SetCtrlsVisbleWithDifferentDeviceType()
        {
            Boolean bIsHasHostFunction = (HVACModuleDeviceTypeList.HVACG2ComplexRelayHostDeviceTypeLists.Contains(mywdDeviceType));
            lbHost.Visible = bIsHasHostFunction;
            cbHost.Visible = bIsHasHostFunction;
            lbSlaveChn.Visible = bIsHasHostFunction;
            cbSlaveChn.Visible = bIsHasHostFunction;
            panel18.Visible = (mywdDeviceType != 107 && mywdDeviceType != 733 && mywdDeviceType != 737);

            if (mywdDeviceType == 735)
            {
                tab12in1.TabPages.Remove(tabAcSetup);
            }

            if (HVACModuleDeviceTypeList.HVACG1NormalRelayDeviceTypeLists.Contains(mywdDeviceType)) // generation 1
            {
                tab12in1.TabPages.Remove(tabOther);
            }
            else if (HVACModuleDeviceTypeList.HVACG2ComplexRelayDeviceTypeLists.Contains(mywdDeviceType)) // 复杂模式继电器
            {
                panFan.Visible = false;
                tab12in1.TabPages.Remove(tabRelay);
            }
        }

        private void frmHVAC_Shown(object sender, EventArgs e)
        {
            SetCtrlsVisbleWithDifferentDeviceType();
            MyActivePage = 1;
            tslRead_Click(0);
            if (HVACModuleDeviceTypeList.HVACG2ComplexRelayDeviceTypeLists.Contains(mywdDeviceType))
            {
                grpTest.Visible = true;
                btnRead_Click(null, null);
            }
        }

        private void btnACSetup_Click(object sender, EventArgs e)
        {
            FrmACSetup frmTmp = new FrmACSetup(myDevName, myHVAC, mywdDeviceType);
            frmTmp.ShowDialog();
        }

        private void chbDS1_CheckedChanged(object sender, EventArgs e)
        {
            txtDS1.Enabled = chbDS1.Checked;
            if (isRead) return;
            if (chbDS1.Checked) myHVAC.AryACSetup[1] = HDLSysPF.SetBit(myHVAC.AryACSetup[1], 0);
            else myHVAC.AryACSetup[1] = HDLSysPF.ClearBit(myHVAC.AryACSetup[1], 0);
        }

        private void chbBus1_CheckedChanged(object sender, EventArgs e)
        {
            txtChn1.Enabled = chbBus1.Checked;
            txtSub1.Enabled = chbBus1.Checked;
            txtDev1.Enabled = chbBus1.Checked;
            if (isRead) return;
            if (chbBus1.Checked) myHVAC.AryACSetup[1] = HDLSysPF.SetBit(myHVAC.AryACSetup[1], 1);
            else myHVAC.AryACSetup[1] = HDLSysPF.ClearBit(myHVAC.AryACSetup[1], 1);
        }
        
        private void chbDS2_CheckedChanged(object sender, EventArgs e)
        {
            txtDS2.Enabled = chbDS2.Checked;
            if (isRead) return;
            if (chbDS2.Checked) myHVAC.AryACSetup[7] = HDLSysPF.SetBit(myHVAC.AryACSetup[7], 0);
            else myHVAC.AryACSetup[7] = HDLSysPF.ClearBit(myHVAC.AryACSetup[7], 0);
        }

        private void txtDS1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar) || e.KeyChar == 8)
                e.Handled = false;
            else
                e.Handled = true;
        }

        private void sbTemp1_ValueChanged(object sender, EventArgs e)
        {
            lbAdjustValue1.Text = (sbTemp1.Value - 10).ToString() + lbAdjustValue1.Text;
            if (isRead) return;
            myHVAC.AryACSetup[3] = Convert.ToByte(sbTemp1.Value);
        }

        private void sbTemp2_ValueChanged(object sender, EventArgs e)
        {
            lbAdjustValue2.Text = (sbTemp2.Value - 10).ToString() + lbAdjustValue2.Text;
            if (isRead) return;
            myHVAC.AryACSetup[9] = Convert.ToByte(sbTemp2.Value);
        }

        private void cbSlave_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            chbSlaveEnable.Checked = ((myHVAC.AryHost[2] & (1 << (cbSlave.SelectedIndex))) > 0);
            txtHostSub.Text = myHVAC.AryHost[(cbSlave.SelectedIndex + 2) * 2 - 1].ToString();
            txtHostDev.Text = myHVAC.AryHost[(cbSlave.SelectedIndex + 2) * 2].ToString();
            if (mywdDeviceType == 737)
            {
                if (1 <= myHVAC.AryHost[cbSlave.SelectedIndex + 19] && myHVAC.AryHost[cbSlave.SelectedIndex + 19] <= 2)
                    cbSlaveChn.SelectedIndex = myHVAC.AryHost[cbSlave.SelectedIndex + 19] - 1;
                else if (myHVAC.AryHost[cbSlave.SelectedIndex + 19] == 100)
                    cbSlaveChn.SelectedIndex = 2;
            }
        }

        void UpdateActivePageIndexFromUint()
        {
            if (tab12in1.SelectedTab.Name == "tabBasic") MyActivePage = 1;
            else if (tab12in1.SelectedTab.Name == "tabRelay") MyActivePage = 2;
            else if (tab12in1.SelectedTab.Name == "tabOther") MyActivePage = 3;
            else if (tab12in1.SelectedTab.Name == "tabAcSetup") MyActivePage = 4;
        }

        private void tab12in1_SelectedIndexChanged(object sender, EventArgs e)
        {
            isRead = true;
            UpdateActivePageIndexFromUint();
            if (CsConst.MyEditMode == 1)
            {
                if (myHVAC.MyRead2UpFlags[MyActivePage - 1] == false)
                {
                    tslRead_Click(0);
                }
                else
                {
                    UpdateDisplayInformationAccordingly(null, null);
                }
            }
        }

        private void cbD1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            myHVAC.aryAirTime[0] = Convert.ToByte(cbD1.SelectedIndex + 1);
        }

        private void cbD2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            myHVAC.aryAirTime[1] = Convert.ToByte(cbD2.SelectedIndex + 1);
        }

        private void cbOper_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            myHVAC.AryACSetup[0] = Convert.ToByte(cbOper.SelectedIndex);
        }

        private void txtDS1_Leave(object sender, EventArgs e)
        {
            string str = txtDS1.Text;
            txtDS1.Text = HDLPF.IsNumStringMode(str, 0, 255);
            txtDS1.SelectionStart = txtDS1.Text.Length;
            if (isRead) return;
            if (txtDS1.Text.Length > 0)
            {
                myHVAC.AryACSetup[2] = Convert.ToByte(txtDS1.Text);
            }
        }

        private void txtSub1_Leave(object sender, EventArgs e)
        {
            string str = txtSub1.Text;
            txtSub1.Text = HDLPF.IsNumStringMode(str, 0, 255);
            txtSub1.SelectionStart = txtSub1.Text.Length;
            if (isRead) return;
            if (txtSub1.Text.Length > 0)
            {
                myHVAC.AryACSetup[4] = Convert.ToByte(txtSub1.Text);
            }
        }

        private void txtDev1_Leave(object sender, EventArgs e)
        {
            string str = txtDev1.Text;
            txtDev1.Text = HDLPF.IsNumStringMode(str, 0, 255);
            txtDev1.SelectionStart = txtDev1.Text.Length;
            if (isRead) return;
            if (txtDev1.Text.Length > 0)
            {
                myHVAC.AryACSetup[5] = Convert.ToByte(txtDev1.Text);
            }
        }

        private void txtChn1_Leave(object sender, EventArgs e)
        {
            string str = txtChn1.Text;
            txtChn1.Text = HDLPF.IsNumStringMode(str, 0, 255);
            txtChn1.SelectionStart = txtChn1.Text.Length;
            if (isRead) return;
            if (txtChn1.Text.Length > 0)
            {
                myHVAC.AryACSetup[6] = Convert.ToByte(txtChn1.Text);
            }
        }

        private void chbBus2_CheckedChanged(object sender, EventArgs e)
        {
            txtChn2.Enabled = chbBus2.Checked;
            txtSub2.Enabled = chbBus2.Checked;
            txtDev2.Enabled = chbBus2.Checked;
            if (isRead) return;
            if (chbBus2.Checked) myHVAC.AryACSetup[7] = HDLSysPF.SetBit(myHVAC.AryACSetup[7], 1);
            else myHVAC.AryACSetup[7] = HDLSysPF.ClearBit(myHVAC.AryACSetup[7], 1);
        }

        private void txtDS2_Leave(object sender, EventArgs e)
        {
            string str = txtDS2.Text;
            txtDS2.Text = HDLPF.IsNumStringMode(str, 0, 255);
            txtDS2.SelectionStart = txtDS2.Text.Length;
            if (isRead) return;
            if (txtDS2.Text.Length > 0)
            {
                myHVAC.AryACSetup[8] = Convert.ToByte(txtDS2.Text);
            }
        }

        private void txtSub2_Leave(object sender, EventArgs e)
        {
            string str = txtSub2.Text;
            txtSub2.Text = HDLPF.IsNumStringMode(str, 0, 255);
            txtSub2.SelectionStart = txtSub2.Text.Length;
            if (isRead) return;
            if (txtSub2.Text.Length > 0)
            {
                myHVAC.AryACSetup[10] = Convert.ToByte(txtSub2.Text);
            }
        }

        private void txtDev2_Leave(object sender, EventArgs e)
        {
            string str = txtDev2.Text;
            txtDev2.Text = HDLPF.IsNumStringMode(str, 0, 255);
            txtDev2.SelectionStart = txtDev2.Text.Length;
            if (isRead) return;
            if (txtDev2.Text.Length > 0)
            {
                myHVAC.AryACSetup[11] = Convert.ToByte(txtDev2.Text);
            }
        }

        private void txtChn2_Leave(object sender, EventArgs e)
        {
            string str = txtChn2.Text;
            txtChn2.Text = HDLPF.IsNumStringMode(str, 0, 255);
            txtChn2.SelectionStart = txtChn2.Text.Length;
            if (isRead) return;
            if (txtChn2.Text.Length > 0)
            {
                myHVAC.AryACSetup[12] = Convert.ToByte(txtChn2.Text);
            }
        }

        private void chbHost_CheckedChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            if (chbHost.Checked)
            {
                if (HVACModuleDeviceTypeList.HVACG2ComplexRelayDeviceTypeLists.Contains(mywdDeviceType))
                    myHVAC.AryHost[0] = 1;
                else
                    myHVAC.AryHost[1] = 1;
            }
            else
            {
                if (HVACModuleDeviceTypeList.HVACG2ComplexRelayDeviceTypeLists.Contains(mywdDeviceType))
                    myHVAC.AryHost[0] = 0;
                else
                    myHVAC.AryHost[1] = 0;
            }
        }

        private void cbHost_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            myHVAC.AryHost[1] = Convert.ToByte(cbHost.SelectedIndex + 1);
        }

        private void chbSlaveEnable_CheckedChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            if (chbSlaveEnable.Checked)
                myHVAC.AryHost[2] = Convert.ToByte(myHVAC.AryHost[2] | (1 << cbSlave.SelectedIndex));
            else
                myHVAC.AryHost[2] = Convert.ToByte(myHVAC.AryHost[2] & (~((1 << cbSlave.SelectedIndex) & 0xFF)));
        }

        private void txtHostSub_Leave(object sender, EventArgs e)
        {
            string str = txtHostSub.Text;
            txtHostSub.Text = HDLPF.IsNumStringMode(str, 0, 255);
            txtHostSub.SelectionStart = txtHostSub.Text.Length;
            if (isRead) return;
            if (txtHostSub.Text.Length > 0)
            {
                myHVAC.AryHost[(cbSlave.SelectedIndex + 2) * 2 - 1] = Convert.ToByte(txtHostSub.Text);
            }
        }

        private void txtHostDev_Leave(object sender, EventArgs e)
        {
            string str = txtHostDev.Text;
            txtHostDev.Text = HDLPF.IsNumStringMode(str, 0, 255);
            txtHostDev.SelectionStart = txtHostDev.Text.Length;
            if (isRead) return;
            if (txtHostDev.Text.Length > 0)
            {
                myHVAC.AryHost[(cbSlave.SelectedIndex + 2) * 2] = Convert.ToByte(txtHostDev.Text);
            }
        }

        private void cbSlaveChn_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            if (cbSlaveChn.SelectedIndex < 2)
                myHVAC.AryHost[cbSlave.SelectedIndex + 19] = Convert.ToByte(cbSlaveChn.SelectedIndex + 1);
            else if (cbSlaveChn.SelectedIndex == 2)
                myHVAC.AryHost[cbSlave.SelectedIndex + 19] = 100;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FrmACSetup frmTmp = new FrmACSetup(myDevName, myHVAC, mywdDeviceType);
            frmTmp.ShowDialog();
        }

        private void rbM_CheckedChanged(object sender, EventArgs e)
        {
            if (rbM.Checked)
            {
                cbDelay1.Items.Clear();
                for (int i = 1; i <= 10; i++)
                    cbDelay1.Items.Add(i.ToString());
                lbD5.Text = "(M)";
            }
            else if (rbS.Checked)
            {
                cbDelay1.Items.Clear();
                for (int i = 3; i <= 127; i++)
                    cbDelay1.Items.Add(i.ToString());
                lbD5.Text = "(S)";
            }
        }

        private void btnDS18B20_Click(object sender, EventArgs e)
        {

        }

        private void cbDelay1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (rbS.Checked == true) // myHVAC.aryAirTime[2] > 128)
                {
                    myHVAC.aryAirTime[2] = Convert.ToByte(128 + Convert.ToByte(cbDelay1.Text));
                }
                else
                {
                    myHVAC.aryAirTime[2] = Convert.ToByte(Convert.ToByte(cbDelay1.Text));
                }
            }
            catch
            { }
        }

        private void cbDelay2_SelectedIndexChanged(object sender, EventArgs e)
        {
            Byte bTag = Convert.ToByte(((ComboBox)sender).Tag.ToString());

            try
            {
                myHVAC.aryAirTime[bTag] = Convert.ToByte(((ComboBox)sender).Text.ToString());
            }
            catch
            { }
        }

        private void cbWind1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Byte bTag = Convert.ToByte(((ComboBox)sender).Tag.ToString());

            try
            {
                myHVAC.arayWindEle[bTag] = Convert.ToByte(((ComboBox)sender).Text.ToString());
            }
            catch
            { }
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            //  2017/2/10添加空调的Test点击事件【完成】
            Cursor.Current = Cursors.WaitCursor;
            byte[] arrayTemp = new byte[3];   //开关（00，01），模式（01，02,04），风速（01，02,04）
            if (rbOFF.Checked)
            {
                arrayTemp[0] = 0;    
                arrayTemp[2] = 0;
                foreach (RadioButton item in panMode.Controls)
                {
                    if (item.Checked)
                    {
                        arrayTemp[1] = (byte)Math.Pow(2, Convert.ToInt32(item.Tag.ToString()) - 1);
                    }
                }
            }
            else
            {
                arrayTemp[0] = 1;
                foreach(RadioButton item in panMode.Controls)
                {              
                    if(item.Checked)
                    {
                       arrayTemp[1] = (byte)Math.Pow(2, Convert.ToInt32(item.Tag.ToString())-1);
                    }
                }
                foreach (RadioButton item in panel1.Controls)
                {
                    if (item.Checked)
                    {
                        arrayTemp[2] = (byte)Math.Pow(2, Convert.ToInt32(item.Tag.ToString()) - 1);
                    }
                }
            }
            if (CsConst.mySends.AddBufToSndList(arrayTemp, 0x1C94, SubNetID, DevID, false, false, false, CsConst.mintHVACDeviceType.Contains(mywdDeviceType)) == false)
            {
                Cursor.Current = Cursors.Default;
                return;
            }
            CsConst.myRevBuf = new byte[1200];
            Cursor.Current = Cursors.Default;
        }



        private void btnExit_Click(object sender, EventArgs e)
        {

        }

        ///2017/02/09添加移动滚动条label显示温度值
        private void sbHeatTemp1_Scroll(object sender, ScrollEventArgs e)
        {
           // if (isRead) return;
            int Tag = Convert.ToInt32((sender as HScrollBar).Tag);
            Label lb = this.Controls.Find("lbHeatTempValue" + Tag.ToString(), true)[0] as Label;
            lb.Text = (sender as HScrollBar).Value.ToString() + lbTempTypeValue.Text;
        }
    }
}
