using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Data.OleDb;
using System.Threading;

namespace HDL_Buspro_Setup_Tool
{
    public partial class frm8in1 : Form
    {
        [DllImport("user32.dll")]
        public static extern Boolean FlashWindow(IntPtr hwnd, Boolean bInvert);
        private MultiSensor mySensor = null;
        private string myDevName = null;
        private int mintIDIndex = -1;
        private int MyDeviceType = -1;
        private int MyActivePage = 0;
        private bool isRead = false;
        private BackgroundWorker MyBackGroup;
        private bool isStopDownloadCodes = false;
        private SendIR tempSend = new SendIR();
        private byte SubNetID;
        private byte DevID;
        private int SelectedRow = 0;
        FrmProcess frmProcessTmp;
        public frm8in1(MultiSensor mysensor, string strName, int intDIndex, int intDeviceType)
        {
            InitializeComponent();
            this.mySensor = mysensor;
            this.myDevName = strName;
            this.mintIDIndex = intDIndex;
            this.MyDeviceType = intDeviceType;
            string strDevName = strName.Split('\\')[0].ToString();

            HDLSysPF.DisplayDeviceNameModeDescription(strName, MyDeviceType, cboDevice, tbModel, tbDescription);

            SubNetID = Convert.ToByte(strDevName.Split('-')[0]);
            DevID = Convert.ToByte(strDevName.Split('-')[1]);
            System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = false;
            
           
        }


        public frm8in1()
        {
            InitializeComponent();
        }

        void InitialFormCtrlsTextOrItems()
        {
            HDLSysPF.addIR(tvIR, tempSend, true);  // 添加已有的列表到窗体
            clK3.Items.Clear();
            clK3.Items.Add(CsConst.mstrINIDefault.IniReadValue("keyMode", "00000", ""));
            clK3.Items.Add(CsConst.mstrINIDefault.IniReadValue("keyMode", "00002", ""));
            clK3.Items.Add(CsConst.mstrINIDefault.IniReadValue("keyMode", "00003", ""));
            clK3.Items.Add(CsConst.mstrINIDefault.IniReadValue("keyMode", "00001", ""));
            clK3.Items.Add(CsConst.mstrINIDefault.IniReadValue("keyMode", "00004", ""));
            clK3.Items.Add(CsConst.mstrINIDefault.IniReadValue("keyMode", "00005", ""));
            clK3.Items.Add(CsConst.mstrINIDefault.IniReadValue("keyMode", "00007", ""));

            HDLSysPF.setDataGridViewColumnsWidth(dgvIR);
            HDLSysPF.setDataGridViewColumnsWidth(dgvSec);

            checklistSensor.Items.Clear();
            checklistSensor.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00610", ""));
            checklistSensor.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00611", ""));
            checklistSensor.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00612", ""));
           // checklistSensor.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00613", ""));
            checklistSensor.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00614", ""));
            checklistSensor.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00615", ""));
            checklistSensor.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00616", ""));
            checklistSensor.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00617", ""));
            checklistSensor.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00618", ""));

            cbParam1.Items.Clear();
            cbParam2.Items.Clear();
            for (int i = 1; i < 100; i++)
            {
                cbParam1.Items.Add((i / 100).ToString() + "." + string.Format("{0:D2}", (i % 100)));
                cbParam2.Items.Add((i / 100).ToString() + "." + string.Format("{0:D2}", (i % 100)));
            }
            cbParam1.Items.Add("1");
            cbParam2.Items.Add("1");
            cbCycle.Items.Clear();
            for (int i = 1; i <= 50; i++)
            {
                cbCycle.Items.Add((i / 10).ToString() + "." + (i % 10));
            }
            cbTemp1.Items.Clear();
            cbTemp2.Items.Clear();
            for (int i = 0; i < 80; i++)
            {
                cbTemp1.Items.Add((i - 20).ToString());
                cbTemp2.Items.Add((i - 20).ToString());
            }

            checklistLED.Items.Clear();
            checklistLED.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00600", ""));
            checklistLED.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00602", ""));

            cbIRSensor.Items.Clear();
            cbIRSensor.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99852", ""));
            cbIRSensor.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99853", ""));

            cbDry1.Items.Clear();
            cbDry2.Items.Clear();
            cbDry1.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99616", ""));
            cbDry1.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99617", ""));
            cbDry2.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99616", ""));
            cbDry2.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99617", ""));
            cbUV1.Items.Clear();
            cbUV2.Items.Clear();
            cbUV1.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99614", ""));
            cbUV1.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99615", ""));
            cbUV2.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99614", ""));
            cbUV2.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99615", ""));
            cbLogicStatu.Items.Clear();
            cbLogicStatu.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99856", ""));
            cbLogicStatu.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99857", ""));
            rb1.Checked = true;
            clL3.Items.Clear();
            clL3.Items.Add(CsConst.WholeTextsList[1775].sDisplayName);
            clL3.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00042", ""));
        }

        void SetCtrlsVisbleWithDifferentDeviceType()
        {
            toolStrip1.Visible = (CsConst.MyEditMode == 0);

            Boolean blnIsHasTemperature =  (MyDeviceType == 329);

            if (MyDeviceType == 329)
            {
                tab8in1.TabPages.Remove(tabPage2);
            }
            groupBox1.Visible = blnIsHasTemperature;
            p1.Visible = blnIsHasTemperature;
        }

        private void frm8in1_Load(object sender, EventArgs e)
        {
            InitialFormCtrlsTextOrItems();                      
        }

        private void tbDown_Click(object sender, EventArgs e)
        {
            byte bytTag = Convert.ToByte(((ToolStripButton)sender).Tag.ToString());
            bool blnShowMsg = (CsConst.MyEditMode != 1);
            if (HDLPF.UploadOrReadGoOn(this.Text, bytTag, blnShowMsg))
            {
                Cursor.Current = Cursors.WaitCursor;
                // SetVisableForDownOrUpload(false);
                // ReadDownLoadThread();  //增加线程，使得当前窗体的任何操作不被限制

                CsConst.MyUPload2DownLists = new List<byte[]>();

                string strName = myDevName.Split('\\')[0].ToString();
                byte bytSubID = byte.Parse(strName.Split('-')[0]);
                byte bytDevID = byte.Parse(strName.Split('-')[1]);
                int num1 = 0;
                int num2 = 0;
                if (tab8in1.SelectedTab.Name == "tabPage1")
                {
                    num1 = Convert.ToInt32(txtFrm.Text);
                    num2 = Convert.ToInt32(txtTo.Text);
                }
                else if (tab8in1.SelectedTab.Name == "tabPage2")
                {
                    num1 = cbPage.SelectedIndex * 8 + 1;
                    num2 = cbPage.SelectedIndex * 8 + 8;
                }
                else if (tab8in1.SelectedTab.Name == "tabPage4" && bytTag == 1)
                {
                    if (dgvLogic.RowCount > 0 && dgvLogic.CurrentRow.Index >= 0)
                        num1 = Convert.ToInt32(SelectedRow + 1);
                }
                byte[] ArayRelay = new byte[] { bytSubID, bytDevID, (byte)(MyDeviceType / 256), (byte)(MyDeviceType % 256), 
                    (byte)MyActivePage,(byte)(mintIDIndex / 256), (byte)(mintIDIndex % 256),(byte)num1,(byte)num2 };

                CsConst.MyUPload2DownLists.Add(ArayRelay);
                CsConst.MyUpload2Down = Convert.ToByte(((ToolStripButton)sender).Tag.ToString());
                FrmDownloadShow Frm = new FrmDownloadShow();
                Frm.FormClosing += new FormClosingEventHandler(UpdateDisplayInformationAccordingly);
                Frm.ShowDialog();
            }
        }

        void UpdateDisplayInformationAccordingly(object sender, FormClosingEventArgs e)
        {
            if (tab8in1.SelectedTab.Name == "tabPage1") showIRSenderInfo();
            else if (tab8in1.SelectedTab.Name == "tabPage2") showIRReceiveInfo();
            else if (tab8in1.SelectedTab.Name == "tabPage3") showSensorInfo();
            else if (tab8in1.SelectedTab.Name == "tabPage4") showLogicInfo();
            else if (tab8in1.SelectedTab.Name == "tabPage5") showSecurityInfo();
            this.TopMost = true;
            this.WindowState = FormWindowState.Normal;
            this.Activate();
            this.TopMost = false;
        }

        private void btnRefStatus_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            byte[] ArayTmp = new byte[0];
            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1645, SubNetID, DevID, false, false, true, false) == true)
            {
                lbTempValue.Text = (CsConst.myRevBuf[26] - 20).ToString() + "C";
                lbStatus6.Text = CsConst.myRevBuf[27] * 256 + CsConst.myRevBuf[28] + "Lux";
                if (CsConst.myRevBuf[32] == 1) lbStatus4.Text = CsConst.mstrINIDefault.IniReadValue("Public", "99849", "");
                else lbStatus4.Text = CsConst.mstrINIDefault.IniReadValue("Public", "99848", "");

                if (CsConst.myRevBuf[33] == 1) lbStatus7.Text = CsConst.mstrINIDefault.IniReadValue("Public", "99605", "");
                else lbStatus7.Text = CsConst.mstrINIDefault.IniReadValue("Public", "99655", "");
                if (CsConst.myRevBuf[34] == 1) lbStatus8.Text = CsConst.mstrINIDefault.IniReadValue("Public", "99605", "");
                else lbStatus8.Text = CsConst.mstrINIDefault.IniReadValue("Public", "99655", "");
            }
            Cursor.Current = Cursors.Default;
        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void frm8in1_Shown(object sender, EventArgs e)
        {
            SetCtrlsVisbleWithDifferentDeviceType();
            if (CsConst.MyEditMode == 1) //在线模式
            {
                MyActivePage = 1;
                tbDown_Click(tbDown, null);
                btnRefStatus_Click(null, null);
                btnBroadcast1_Click(null, null);
            }
        }

        private void showIRSenderInfo()
        {
            try
            {
                isRead = true;
                if (mySensor != null)
                {
                    if (mySensor.IRCodes != null)
                    {
                        dgvIR.Rows.Clear();
                        for (int i = 0; i < mySensor.IRCodes.Count; i++)
                        {
                            UVCMD.IRCode ir = mySensor.IRCodes[i];
                            string str = CsConst.WholeTextsList[1775].sDisplayName;
                            if (ir.Enable == 1) str = CsConst.mstrINIDefault.IniReadValue("public", "00042", "");
                            object[] obj = new object[] { ir.KeyID.ToString(), ir.Remark1.ToString(), str ,
                                   CsConst.mstrINIDefault.IniReadValue("public", "99810", "")};
                            dgvIR.Rows.Add(obj);
                        }
                        if (dgvIR.RowCount > 0)
                        {
                            dgvIR.Rows[0].Selected = true;
                        }
                    }
                }
                btnFree_Click(null, null);
            }
            catch
            {
            }
            isRead = false;
        }

        private void showIRReceiveInfo()
        {
            try
            {
                isRead = true;
                if (mySensor == null) return;
                if (mySensor.IrReceiver == null) return;
                dgvKey.Rows.Clear();
                for (int i = 0; i < mySensor.IrReceiver.Count; i++)
                {
                    MultiSensor.IRReceive temp = mySensor.IrReceiver[i];
                    string strMode = CsConst.mstrINIDefault.IniReadValue("keyMode", "000" + temp.IRBtnModel.ToString("D2"), "");
                    if (!clK3.Items.Contains(strMode)) strMode = clK3.Items[0].ToString();
                    object[] obj = new object[] { dgvKey.RowCount + 1, temp.IRBtnRemark, strMode };
                    dgvKey.Rows.Add(obj);
                }
            }
            catch
            {
            }
            isRead = false;
        }

        private void showSensorInfo()
        {
            try
            {
                isRead = true;
                if (MyDeviceType == 329)
                {
                    sbSensitivity1.Maximum = 10;
                }
                else
                {
                    sbSensitivity1.Maximum = 100;
                }
                for (int i = 0; i < 2; i++)
                {
                    if (mySensor.ONLEDs[i] == 1) checklistLED.SetItemChecked(i, true);
                    else checklistLED.SetItemChecked(i, false);
                }
                for (int i = 0; i < 7; i++)
                {
                    if (mySensor.EnableSensors[i] == 1) checklistSensor.SetItemChecked(i, true);
                    else checklistSensor.SetItemChecked(i, false);
                }

                if (mySensor.ParamSensors[0] <= 128)
                {
                    if (sbSensitivity4.Minimum <= mySensor.ParamSensors[0] && mySensor.ParamSensors[0] <= sbSensitivity4.Maximum)
                        sbSensitivity4.Value = mySensor.ParamSensors[0];
                }
                else
                {
                    if (sbSensitivity4.Minimum <= (mySensor.ParamSensors[0] - 128) && (mySensor.ParamSensors[0] - 128) <= sbSensitivity4.Maximum)
                        sbSensitivity4.Value = (mySensor.ParamSensors[0] - 128);
                }
                if (mySensor.ParamSensors[1] <= sbSensitivity1.Maximum)
                    sbSensitivity1.Value = mySensor.ParamSensors[1];
                chbEnable.Checked = (mySensor.EnableBroads[0] == 1);
                txtLux.Text = (mySensor.EnableBroads[1] * 256 + mySensor.EnableBroads[2]).ToString();
                string str1 = ((mySensor.EnableBroads[3] * 256 + mySensor.EnableBroads[4]) / 100).ToString() + "." +
                            string.Format("{0:D2}", ((mySensor.EnableBroads[3] * 256 + mySensor.EnableBroads[4]) % 100));
                if (str1 == "1.00") str1 = "1";
                string str2 = ((mySensor.EnableBroads[5] * 256 + mySensor.EnableBroads[6]) / 100).ToString() + "." +
                            string.Format("{0:D2}", ((mySensor.EnableBroads[5] * 256 + mySensor.EnableBroads[6]) % 100));
                if (str2 == "1.00") str2 = "1";
                string str3 = (mySensor.EnableBroads[7] / 10).ToString() + "." + (mySensor.EnableBroads[7] % 10).ToString();
                cbParam1.SelectedIndex = cbParam1.Items.IndexOf(str1);
                cbParam2.SelectedIndex = cbParam2.Items.IndexOf(str2);
                cbCycle.SelectedIndex = cbCycle.Items.IndexOf(str3);
                if (mySensor.EnableBroads[8] <= sbLimit.Maximum)
                    sbLimit.Value = mySensor.EnableBroads[8];
                if (cbParam1.SelectedIndex < 0) cbParam1.SelectedIndex = 0;
                if (cbParam2.SelectedIndex < 0) cbParam2.SelectedIndex = 0;
                if (cbCycle.SelectedIndex < 0) cbCycle.SelectedIndex = 0;
            }
            catch
            {
            }
            isRead = false;
            sbSensitivity1_ValueChanged(null, null);
            sbSensitivity4_ValueChanged(null, null);
            sbLimit_ValueChanged(null, null);
        }

        private void showLogicInfo()
        {
            try
            {
                isRead = true;
                dgvLogic.Rows.Clear();
                for (int i = 0; i < mySensor.logic.Count; i++)
                {
                    string strEnable = clL3.Items[0].ToString();
                    if (mySensor.logic[i].Enabled == 1) strEnable = clL3.Items[1].ToString();
                    object[] obj = new object[] { mySensor.logic[i].ID.ToString(), mySensor.logic[i].Remarklogic, strEnable };
                    dgvLogic.Rows.Add(obj);
                }
            }
            catch
            {
            }
            isRead = false;
            for (int i = 0; i < dgvLogic.Rows.Count; i++)
            {
                if (dgvLogic.Rows[i].Index == SelectedRow)
                    dgvLogic.Rows[i].Selected = true;
                else
                    dgvLogic.Rows[i].Selected = false;
            }
            this.dgvLogic.CurrentCell = this.dgvLogic.Rows[SelectedRow].Cells[0];
            dgvLogic_CellClick(dgvLogic, new DataGridViewCellEventArgs(0, SelectedRow));
        }

        private void showSecurityInfo()
        {
            try
            {
                isRead = true;
                dgvSec.Rows.Clear();
                if (mySensor.fireset != null && mySensor.fireset.Count != 0)
                {
                    for (int i = 0; i < mySensor.fireset.Count; i++)
                    {
                        UVCMD.SecurityInfo reader = mySensor.fireset[i];
                        bool enable = false;
                        if (i < 2)
                        {
                            if (reader.bytTerms == 1) enable = true;
                        }
                        else
                        {
                            if (reader.bytTerms == 2) enable = true;
                        }
                        string strHint = CsConst.WholeTextsList[51].sDisplayName;
                        switch (i)
                        {
                            case 1: strHint = CsConst.WholeTextsList[50].sDisplayName; break;
                            case 2: strHint = CsConst.WholeTextsList[89].sDisplayName; break;
                            case 3: strHint = CsConst.WholeTextsList[91].sDisplayName; break;
                        }
                        object[] obj = new object[]{dgvSec.RowCount+1,strHint, enable,
                        reader.strRemark,reader.bytSubID.ToString(),reader.bytDevID.ToString(), reader.bytArea.ToString()};
                        dgvSec.Rows.Add(obj);
                    }
                }

            }
            catch
            {
            }
            isRead = false;
        }

        private void dgvLogic_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                SelectedRow = e.RowIndex;
                byte[] arayTmp = new byte[1];
                arayTmp[0] = Convert.ToByte(dgvLogic[0, e.RowIndex].Value.ToString());
                if (CsConst.mySends.AddBufToSndList(arayTmp, 0x1650, SubNetID, DevID, false, true, true, false) == true)
                {
                    SensorLogic oTmp = mySensor.logic[e.RowIndex];
                    oTmp.bytRelation = CsConst.myRevBuf[27];
                    oTmp.EnableSensors = new byte[15];
                    oTmp.Paramters = new byte[20];
                    int intTmp = CsConst.myRevBuf[28];
                    if (intTmp == 255) intTmp = 0;
                    //oTmp.EnableSensors
                    for (byte bytI = 0; bytI <= 6; bytI++)
                    {
                        oTmp.EnableSensors[bytI] = Convert.ToByte((intTmp & (1 << bytI)) == (1 << bytI));
                    }

                    oTmp.DelayTimeT = CsConst.myRevBuf[29] * 256 + CsConst.myRevBuf[30];
                    Array.Copy(CsConst.myRevBuf, 31, oTmp.Paramters, 0, 7);
                    oTmp.UV1 = new UniversalSwitchSet();
                    oTmp.UV1.id = Convert.ToByte(HDLPF.IsNumStringMode(CsConst.myRevBuf[38].ToString(), 201, 248));
                    oTmp.UV1.condition = CsConst.myRevBuf[39];

                    oTmp.UV2 = new UniversalSwitchSet();
                    oTmp.UV2.id = Convert.ToByte(HDLPF.IsNumStringMode(CsConst.myRevBuf[40].ToString(), 201, 248));
                    if (oTmp.UV2.id == oTmp.UV1.id) oTmp.UV2.id = Convert.ToByte(oTmp.UV1.id + 1);
                    oTmp.UV2.condition = CsConst.myRevBuf[41];
                    oTmp.DelayTimeF = CsConst.myRevBuf[42] * 256 + CsConst.myRevBuf[43];
                    
                }
                else
                {
                    Cursor.Current = Cursors.Default;
                    return;
                }
                DisplaySomeLogicInformation(e.RowIndex);
            }
            catch
            {
            }
        }

        private void DisplaySomeLogicInformation(int Index)
        {
            if (dgvLogic.CurrentRow.Index < 0) return;
            if (mySensor == null) return;
            if (mySensor.logic == null) return;
            isRead = true;
            try
            {
                chbTemp.Enabled = (mySensor.EnableSensors[0] == 1);
                checkboxBright.Enabled = (mySensor.EnableSensors[1] == 1);
                chbIR.Enabled = (mySensor.EnableSensors[2] == 1);
                chbDry1.Enabled = (mySensor.EnableSensors[3] == 1);
                chbDry2.Enabled = (mySensor.EnableSensors[4] == 1);
                checkboxUV1.Enabled = (mySensor.EnableSensors[5] == 1);
                checkboxUV2.Enabled = (mySensor.EnableSensors[6] == 1);
                CheckBoxLogic.Enabled = false;
                SensorLogic oLogic = mySensor.logic[Index];

                chbTemp.Checked = false;
                checkboxBright.Checked = Convert.ToBoolean(oLogic.EnableSensors[1]);
                chbIR.Checked = Convert.ToBoolean(oLogic.EnableSensors[2]);
                chbDry1.Checked = Convert.ToBoolean(oLogic.EnableSensors[3]);
                chbDry2.Checked = Convert.ToBoolean(oLogic.EnableSensors[4]);
                checkboxUV1.Checked = Convert.ToBoolean(oLogic.EnableSensors[5]);
                checkboxUV2.Checked = Convert.ToBoolean(oLogic.EnableSensors[6]);
                CheckBoxLogic.Checked = false;

                if (oLogic.bytRelation == 0) rbOr.Checked = true;
                else if (oLogic.bytRelation == 1) rbAnd.Checked = true;
                cbTemp1.SelectedIndex = 0;
                cbTemp2.SelectedIndex = 0;

                if ((0 <= Convert.ToDecimal(oLogic.Paramters[0] * 256 + oLogic.Paramters[1])) && (Convert.ToDecimal(oLogic.Paramters[0] * 256 + oLogic.Paramters[1]) <= 5000))
                    NumBr1.Value = Convert.ToDecimal(oLogic.Paramters[0] * 256 + oLogic.Paramters[1]);
                if ((0 <= Convert.ToDecimal(oLogic.Paramters[2] * 256 + oLogic.Paramters[3])) && (Convert.ToDecimal(oLogic.Paramters[2] * 256 + oLogic.Paramters[3]) <= 5000))
                    NumBr2.Value = Convert.ToDecimal(oLogic.Paramters[2] * 256 + oLogic.Paramters[3]);
                if (oLogic.Paramters[4] < 2)
                    cbIRSensor.Text = cbIRSensor.Items[oLogic.Paramters[4]].ToString();
                if (cbIRSensor.SelectedIndex < 0) cbIRSensor.SelectedIndex = 0;
                if (oLogic.Paramters[5] < 2)
                    cbDry1.Text = cbDry1.Items[oLogic.Paramters[5]].ToString();
                if (cbDry1.SelectedIndex < 0) cbDry1.SelectedIndex = 0;
                if (oLogic.Paramters[6] < 2)
                    cbDry2.Text = cbDry2.Items[oLogic.Paramters[6]].ToString();
                if (cbDry2.SelectedIndex < 0) cbDry2.SelectedIndex = 0;
                cbLogicNum.SelectedIndex = 0;
                cbLogicStatu.SelectedIndex = 0;
                if (oLogic.UV1 != null)
                {
                    txtUVID1.Text = oLogic.UV1.id.ToString();
                    if (oLogic.UV1.remark == null) oLogic.UV1.remark = "";
                    txtUVRemark1.Text = oLogic.UV1.remark.ToString();
                    if (oLogic.UV1.condition < 2)
                        cbUV1.Text = cbUV1.Items[oLogic.UV1.condition].ToString();
                    checkAuto1.Checked = oLogic.UV1.isAutoOff;
                    txtUVAuto1.Text = oLogic.UV1.autoOffDelay.ToString();
                    lbUV1.Visible = true;
                    if (oLogic.UV1.state == 0) lbUV1.Text = CsConst.mstrINIDefault.IniReadValue("Public", "99859", "") + txtUVID1.Text + " " +
                                                                CsConst.Status[0];
                    else lbUV1.Text = CsConst.mstrINIDefault.IniReadValue("Public", "99859", "") + txtUVID1.Text + " " +
                                                                CsConst.Status[0];
                }

                if (oLogic.UV2 != null)
                {
                    txtUVID2.Text = oLogic.UV2.id.ToString();
                    if (oLogic.UV2.remark == null) oLogic.UV2.remark = "";
                    txtUVRemark2.Text = oLogic.UV2.remark.ToString();
                    if (oLogic.UV2.condition < 2)
                        cbUV2.Text = cbUV2.Items[oLogic.UV2.condition].ToString();
                    checkAuto2.Checked = oLogic.UV2.isAutoOff;
                    txtUVAuto2.Text = oLogic.UV2.autoOffDelay.ToString();
                    lbUV2.Visible = true;
                    if (oLogic.UV1.state == 0) lbUV2.Text = CsConst.mstrINIDefault.IniReadValue("Public", "99859", "") + txtUVID2.Text + " " +
                                                                CsConst.Status[0];
                    else lbUV2.Text = CsConst.mstrINIDefault.IniReadValue("Public", "99859", "") + txtUVID2.Text + " " +
                                                                CsConst.Status[0];
                }
                truetime.Text = oLogic.DelayTimeT.ToString();
                falsetime.Text = oLogic.DelayTimeF.ToString();
            }
            catch
            {
            }
            isRead = false;
            txtUVID1_TextChanged(null, null);
            txtUVID2_TextChanged(null, null);

            chbTemp_CheckedChanged(chbTemp, null);
            chbTemp_CheckedChanged(checkboxBright, null);
            chbTemp_CheckedChanged(chbIR, null);
            chbTemp_CheckedChanged(chbDry1, null);
            chbTemp_CheckedChanged(chbDry2, null);
            chbTemp_CheckedChanged(checkboxUV1, null);
            chbTemp_CheckedChanged(checkboxUV2, null);
            chbTemp_CheckedChanged(CheckBoxLogic, null);

        }

        private void btnFree_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            byte[] arayTmp = new byte[0];
            if (CsConst.mySends.AddBufToSndList(arayTmp, 0x1696, SubNetID, DevID, false, true, true, false) == true)
            {
                double temp = (double)(CsConst.myRevBuf[27] * 256 + CsConst.myRevBuf[28]) / (double)(CsConst.myRevBuf[25] * 256 + CsConst.myRevBuf[26]);
                lbFree.Text = string.Format("{0:P}", temp);
                
            }
            Cursor.Current = Cursors.Default;
        }

        private void btnModify_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            byte[] arayRemark = new byte[20];
            string strRemark = txtKey.Text.Trim();
            byte[] arayTmp2 = HDLUDP.StringToByte(strRemark);
            if (arayTmp2.Length > 20)
            {
                Array.Copy(arayTmp2, 0, arayRemark, 0, 20);
            }
            else
            {
                Array.Copy(arayTmp2, 0, arayRemark, 0, arayTmp2.Length);
            }
            byte[] araySendIR = new byte[12];
            araySendIR[0] = Convert.ToByte(Convert.ToInt32(dgvIR[0, dgvIR.CurrentRow.Index].Value.ToString()) - 1);
            araySendIR[1] = 0;
            for (int K = 0; K <= 9; K++) araySendIR[2 + K] = arayRemark[K];
            if (CsConst.mySends.AddBufToSndList(araySendIR, 0x1690, SubNetID, DevID, false, true, true, false) == true)
            {
                HDLUDP.TimeBetwnNext(20);
                
            }

            araySendIR[1] = 1;    //save the remark then
            for (int K = 0; K <= 9; K++) araySendIR[2 + K] = arayRemark[10 + K];

            if (CsConst.mySends.AddBufToSndList(araySendIR, 0x1690, SubNetID, DevID, false, true, true, false) == true)
            {
                HDLUDP.TimeBetwnNext(20);
                
            }
            dgvIR[1, dgvIR.CurrentRow.Index].Value = txtKey.Text;
            Cursor.Current = Cursors.Default;
        }

        private void btnSure_Click(object sender, EventArgs e)
        {
            tbDown_Click(tbDown, null);
        }

        private void btnInitital_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult result = MessageBox.Show(CsConst.mstrINIDefault.IniReadValue("public", "99805", ""), "", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                if (result == DialogResult.OK)
                {
                    byte[] arayTmp = new byte[0];
                    if (CsConst.mySends.AddBufToSndList(arayTmp, 0x1698, SubNetID, DevID, false, false, false, CsConst.minAllWirelessDeviceType.Contains(MyDeviceType)) == true)
                    {
                        
                    }

                    CsConst.isIniAllKeySucess = false;
                    MyBackGroup = new BackgroundWorker();
                    MyBackGroup.DoWork += new DoWorkEventHandler(calculationWorker_DoWork2);
                    MyBackGroup.ProgressChanged += new ProgressChangedEventHandler(calculationWorker_ProgressChanged2);
                    MyBackGroup.WorkerReportsProgress = true;
                    MyBackGroup.RunWorkerCompleted += new RunWorkerCompletedEventHandler(calculationWorker_RunWorkerCompleted2);
                    MyBackGroup.RunWorkerAsync();
                    MyBackGroup.WorkerSupportsCancellation = true;
                    frmProcessTmp = new FrmProcess();
                    frmProcessTmp.ShowDialog();
                }
            }
            catch
            {
            }
            Cursor.Current = Cursors.Default;
        }

        void calculationWorker_RunWorkerCompleted2(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                frmProcessTmp.Close();
                if (CsConst.isIniAllKeySucess)
                {
                    MessageBox.Show(CsConst.mstrINIDefault.IniReadValue("public", "99806", ""));
                    dgvIR.Rows.Clear();
                }
                else
                {
                    MessageBox.Show(CsConst.mstrINIDefault.IniReadValue("public", "99807", ""));
                }
                
            }
            catch
            {
            }
        }

        void calculationWorker_ProgressChanged2(object sender, ProgressChangedEventArgs e)
        {
            try
            {

            }
            catch
            {
            }
        }

        void calculationWorker_DoWork2(object sender, DoWorkEventArgs e)
        {
            try
            {
                DateTime t1 = DateTime.Now;
                DateTime t2 = DateTime.Now;
                while (!CsConst.isIniAllKeySucess)
                {
                    t2 = DateTime.Now;
                    if (HDLSysPF.Compare(t2, t1) > 180000)
                    {
                        break;
                    }
                }

            }
            catch
            {
            }
        }

        private void btnRef2_Click(object sender, EventArgs e)
        {
            tbDown_Click(tbDown, null);
        }

        private void btnSave2_Click(object sender, EventArgs e)
        {
            if (tab8in1.SelectedTab.Name == "tabPage4")
            {
                chbTemp_CheckedChanged(chbTemp, null);
                cbTemp1_SelectedIndexChanged(null, null);
                chbTemp_CheckedChanged(checkboxBright, null);
                NumBr1_ValueChanged(null, null);
                chbTemp_CheckedChanged(chbIR, null);
                cbIRSensor_SelectedIndexChanged(null, null);
                chbTemp_CheckedChanged(chbDry1, null);
                cbDry1_SelectedIndexChanged(null, null);
                chbTemp_CheckedChanged(chbDry2, null);
                cbDry2_SelectedIndexChanged(null, null);
                chbTemp_CheckedChanged(checkboxUV1, null);
                chbTemp_CheckedChanged(checkboxUV2, null);
                cbUV1_SelectedIndexChanged(null, null);
                chbTemp_CheckedChanged(CheckBoxLogic, null);
                cbLogicNum_SelectedIndexChanged(null, null);
                cbLogicStatu_SelectedIndexChanged(null, null);
                truetime_TextChanged(null, null);
                rbOr_CheckedChanged(null, null);
            }
            tbDown_Click(tbUpload, null);
        }

        private void btnSaveAndClose2_Click(object sender, EventArgs e)
        {
            if (tab8in1.SelectedTab.Name == "tabPage4")
            {

                chbTemp_CheckedChanged(chbTemp, null);
                cbTemp1_SelectedIndexChanged(null, null);
                chbTemp_CheckedChanged(checkboxBright, null);
                NumBr1_ValueChanged(null, null);
                chbTemp_CheckedChanged(chbIR, null);
                cbIRSensor_SelectedIndexChanged(null, null);
                chbTemp_CheckedChanged(chbDry1, null);
                cbDry1_SelectedIndexChanged(null, null);
                chbTemp_CheckedChanged(chbDry2, null);
                cbDry2_SelectedIndexChanged(null, null);
                chbTemp_CheckedChanged(checkboxUV1, null);
                chbTemp_CheckedChanged(checkboxUV2, null);
                cbUV1_SelectedIndexChanged(null, null);
                chbTemp_CheckedChanged(CheckBoxLogic, null);
                cbLogicNum_SelectedIndexChanged(null, null);
                cbLogicStatu_SelectedIndexChanged(null, null);
                truetime_TextChanged(null, null);
                rbOr_CheckedChanged(null, null);
            }
            tbDown_Click(tbUpload, null);
            this.Close();
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void lbSensitivity1_Click(object sender, EventArgs e)
        {

        }

        private void cbTemp1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cbTemp1.SelectedIndex >= 0 && cbTemp2.SelectedIndex >= 0)
                {
                    int num1 = Convert.ToInt32(cbTemp1.Text);
                    int num2 = Convert.ToInt32(cbTemp2.Text);
                    if (num2 < num1) cbTemp2.SelectedIndex = cbTemp1.SelectedIndex;
                }
                if (isRead) return;
                if (dgvLogic.RowCount <= 0) return;
                if (dgvLogic.CurrentRow.Index < 0) return;
            }
            catch
            {
            }
        }

        private void chbTemp_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                int Tag = Convert.ToInt32((sender as CheckBox).Tag);
                if (Tag == 0)
                {
                    cbTemp1.Enabled = ((sender as CheckBox).Checked && (sender as CheckBox).Enabled);
                    cbTemp2.Enabled = ((sender as CheckBox).Checked && (sender as CheckBox).Enabled);
                    if (isRead) return;
                    if (dgvLogic.RowCount <= 0) return;
                    if (dgvLogic.CurrentRow.Index < 0) return;
                    mySensor.logic[SelectedRow].EnableSensors[0] = 0;
                }
                else if (Tag == 1)
                {
                    NumBr1.Enabled = ((sender as CheckBox).Checked && (sender as CheckBox).Enabled);
                    NumBr2.Enabled = ((sender as CheckBox).Checked && (sender as CheckBox).Enabled);
                    if (isRead) return;
                    if (dgvLogic.RowCount <= 0) return;
                    if (dgvLogic.CurrentRow.Index < 0) return;
                    mySensor.logic[SelectedRow].EnableSensors[1] = Convert.ToByte((sender as CheckBox).Checked);
                }
                else if (Tag == 2)
                {
                    cbIRSensor.Enabled = ((sender as CheckBox).Checked && (sender as CheckBox).Enabled);
                    if (isRead) return;
                    if (dgvLogic.RowCount <= 0) return;
                    if (dgvLogic.CurrentRow.Index < 0) return;
                    mySensor.logic[SelectedRow].EnableSensors[2] = Convert.ToByte((sender as CheckBox).Checked);
                }
                else if (Tag == 4)
                {
                    cbDry1.Enabled = ((sender as CheckBox).Checked && (sender as CheckBox).Enabled);
                    if (isRead) return;
                    if (dgvLogic.RowCount <= 0) return;
                    if (dgvLogic.CurrentRow.Index < 0) return;
                    mySensor.logic[SelectedRow].EnableSensors[3] = Convert.ToByte((sender as CheckBox).Checked);
                }
                else if (Tag == 5)
                {
                    cbDry2.Enabled = ((sender as CheckBox).Checked && (sender as CheckBox).Enabled);
                    if (isRead) return;
                    if (dgvLogic.RowCount <= 0) return;
                    if (dgvLogic.CurrentRow.Index < 0) return;
                    mySensor.logic[SelectedRow].EnableSensors[4] = Convert.ToByte((sender as CheckBox).Checked);
                }
                else if (Tag == 6)
                {
                    cbUV1.Enabled = ((sender as CheckBox).Checked && (sender as CheckBox).Enabled);
                    txtUVID1.Enabled = ((sender as CheckBox).Checked && (sender as CheckBox).Enabled);
                    txtUVRemark1.Enabled = ((sender as CheckBox).Checked && (sender as CheckBox).Enabled);
                    checkAuto1.Enabled = ((sender as CheckBox).Checked && (sender as CheckBox).Enabled);
                    txtUVAuto1.Enabled = ((sender as CheckBox).Checked && (sender as CheckBox).Enabled);
                    if (isRead) return;
                    if (dgvLogic.RowCount <= 0) return;
                    if (dgvLogic.CurrentRow.Index < 0) return;
                    mySensor.logic[SelectedRow].EnableSensors[5] = Convert.ToByte((sender as CheckBox).Checked);
                }
                else if (Tag == 7)
                {
                    cbUV2.Enabled = ((sender as CheckBox).Checked && (sender as CheckBox).Enabled);
                    txtUVID2.Enabled = ((sender as CheckBox).Checked && (sender as CheckBox).Enabled);
                    txtUVRemark2.Enabled = ((sender as CheckBox).Checked && (sender as CheckBox).Enabled);
                    checkAuto2.Enabled = ((sender as CheckBox).Checked && (sender as CheckBox).Enabled);
                    txtUVAuto2.Enabled = ((sender as CheckBox).Checked && (sender as CheckBox).Enabled);
                    if (isRead) return;
                    if (dgvLogic.RowCount <= 0) return;
                    if (dgvLogic.CurrentRow.Index < 0) return;
                    mySensor.logic[SelectedRow].EnableSensors[6] = Convert.ToByte((sender as CheckBox).Checked);
                }
                else if (Tag == 8)
                {
                    cbLogicNum.Enabled = ((sender as CheckBox).Checked && (sender as CheckBox).Enabled);
                    cbLogicStatu.Enabled = ((sender as CheckBox).Checked && (sender as CheckBox).Enabled);
                    if (isRead) return;
                    if (dgvLogic.RowCount <= 0) return;
                    if (dgvLogic.CurrentRow.Index < 0) return;
                    mySensor.logic[SelectedRow].EnableSensors[7] = Convert.ToByte((sender as CheckBox).Checked);
                }
            }
            catch
            {
            }
        }

        private void NumBr1_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                int num1 = Convert.ToInt32(NumBr1.Value);
                int num2 = Convert.ToInt32(NumBr2.Value);
                if (num2 < num1) NumBr2.Value = NumBr1.Value;
                if (isRead) return;
                if (dgvLogic.RowCount <= 0) return;
                if (dgvLogic.CurrentRow.Index < 0) return;
                num1 = Convert.ToInt32(NumBr1.Value);
                mySensor.logic[SelectedRow].Paramters[0] = Convert.ToByte(num1 / 256);
                mySensor.logic[SelectedRow].Paramters[1] = Convert.ToByte(num1 % 256);
                num2 = Convert.ToInt32(NumBr2.Value);
                mySensor.logic[SelectedRow].Paramters[2] = Convert.ToByte(num2 / 256);
                mySensor.logic[SelectedRow].Paramters[3] = Convert.ToByte(num2 % 256);
            }
            catch
            {
            }
        }

        private void cbIRSensor_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            if (dgvLogic.RowCount <= 0) return;
            if (dgvLogic.CurrentRow.Index < 0) return;
            mySensor.logic[SelectedRow].Paramters[4] = Convert.ToByte(cbIRSensor.SelectedIndex);
        }

        private void cbDry1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            if (dgvLogic.RowCount <= 0) return;
            if (dgvLogic.CurrentRow.Index < 0) return;
            mySensor.logic[SelectedRow].Paramters[5] = Convert.ToByte(cbDry1.SelectedIndex);
        }

        private void cbDry2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            if (dgvLogic.RowCount <= 0) return;
            if (dgvLogic.CurrentRow.Index < 0) return;
            mySensor.logic[SelectedRow].Paramters[6] = Convert.ToByte(cbDry2.SelectedIndex);
        }

        private void timerSensor_Tick(object sender, EventArgs e)
        {
            if (isRead) return;
            byte[] ArayTmp = new byte[0];
            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1645, SubNetID, DevID, false, false, true, false) == true)
            {
                lbTempValue.Text = (CsConst.myRevBuf[26] - 20).ToString() + "C";
                lbStatus6.Text = CsConst.myRevBuf[27] * 256 + CsConst.myRevBuf[28] + "Lux";
                if (CsConst.myRevBuf[29] == 1) lbStatus4.Text = CsConst.mstrINIDefault.IniReadValue("Public", "99849", "");
                else lbStatus4.Text = CsConst.mstrINIDefault.IniReadValue("Public", "99848", "");

                if (CsConst.myRevBuf[31] == 1) lbStatus7.Text = CsConst.mstrINIDefault.IniReadValue("Public", "99655", "");
                else lbStatus7.Text = CsConst.mstrINIDefault.IniReadValue("Public", "99605", "");
                if (CsConst.myRevBuf[32] == 1) lbStatus8.Text = CsConst.mstrINIDefault.IniReadValue("Public", "99655", "");
                else lbStatus8.Text = CsConst.mstrINIDefault.IniReadValue("Public", "99605", "");
            }
        }

        private void btnBroadcast1_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                byte[] ArayTmp = new byte[0];
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x16B3, SubNetID, DevID, false, false, true, false) == true)
                {
                    chbBroadcast.Checked = (CsConst.myRevBuf[26] == 1);

                }
                
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1645, SubNetID, DevID, false, false, true, false) == true)
                {
                    lbTempValue.Text = (CsConst.myRevBuf[26] - 20).ToString() + "C";
                    lbStatus6.Text = CsConst.myRevBuf[27] * 256 + CsConst.myRevBuf[28] + "Lux";
                    if (CsConst.myRevBuf[32] == 1) lbStatus4.Text = CsConst.mstrINIDefault.IniReadValue("Public", "99849", "");
                    else lbStatus4.Text = CsConst.mstrINIDefault.IniReadValue("Public", "99848", "");

                    if (CsConst.myRevBuf[33] == 1) lbStatus7.Text = CsConst.mstrINIDefault.IniReadValue("Public", "99605", "");
                    else lbStatus7.Text = CsConst.mstrINIDefault.IniReadValue("Public", "99655", "");
                    if (CsConst.myRevBuf[34] == 1) lbStatus8.Text = CsConst.mstrINIDefault.IniReadValue("Public", "99605", "");
                    else lbStatus8.Text = CsConst.mstrINIDefault.IniReadValue("Public", "99655", "");
                }
                else
                {
                    Cursor.Current = Cursors.Default;
                    return;
                }
                Cursor.Current = Cursors.Default;
            }
            catch { }
            Cursor.Current = Cursors.Default;
        }

        private void btnBroadcast2_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            byte[] ArayTmp = new byte[1];
            if (chbBroadcast.Checked) ArayTmp[0] = 1;
            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x16B5, SubNetID, DevID, false, false, true, false) == true)
            {
                
            }
            Cursor.Current = Cursors.Default;
        }

        private void cbUV1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            if (dgvLogic.RowCount <= 0) return;
            if (dgvLogic.CurrentRow.Index < 0) return;
            if (cbUV1.SelectedIndex >= 0)
                mySensor.logic[SelectedRow].UV1.condition = Convert.ToByte(cbUV1.SelectedIndex);
            if (cbUV2.SelectedIndex >= 0)
                mySensor.logic[SelectedRow].UV2.condition = Convert.ToByte(cbUV2.SelectedIndex);
        }

        private void txtUVID1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (isRead) return;
                if (dgvLogic.RowCount <= 0) return;
                if (dgvLogic.CurrentRow.Index < 0) return;
                string str = txtUVID1.Text.Trim();
                if (str == "") return;
                if (str.Length >= 3)
                {
                    txtUVID1.Text = HDLPF.IsNumStringMode(str, 201, 248);
                    if (checkboxUV2.Checked && txtUVID1.Text == txtUVID2.Text)
                    {
                        if (txtUVID2.Text == "201") txtUVID1.Text = "202";
                        else txtUVID1.Text = (Convert.ToInt32(txtUVID2.Text) - 1).ToString();
                    }
                    txtUVID1.SelectionStart = txtUVID1.Text.Length;
                    mySensor.logic[SelectedRow].UV1.id = byte.Parse(txtUVID1.Text);
                    if (CsConst.MyEditMode == 1)
                    {
                        Cursor.Current = Cursors.WaitCursor;
                        isRead = true;
                        byte[] arayTmp = new byte[1];
                        arayTmp[0] = Convert.ToByte(txtUVID1.Text);
                        if ((CsConst.mySends.AddBufToSndList(arayTmp, 0x164C, SubNetID, DevID, false, false, true, false) == true))
                        {
                            byte[] arayRemark = new byte[20];
                            Array.Copy(CsConst.myRevBuf, 27, arayRemark, 0, arayRemark.Length);
                            mySensor.logic[SelectedRow].UV1.remark = HDLPF.Byte2String(arayRemark);
                            txtUVRemark1.Text = mySensor.logic[SelectedRow].UV1.remark;
                            mySensor.logic[SelectedRow].UV1.isAutoOff = (CsConst.myRevBuf[47] == 1);
                            checkAuto1.Checked = mySensor.logic[SelectedRow].UV1.isAutoOff;
                            if (CsConst.myRevBuf[48] * 256 + CsConst.myRevBuf[49] > 3600)
                                mySensor.logic[SelectedRow].UV1.autoOffDelay = 1;
                            else
                                mySensor.logic[SelectedRow].UV1.autoOffDelay = CsConst.myRevBuf[48] * 256 + CsConst.myRevBuf[49];
                            txtUVAuto1.Text = mySensor.logic[SelectedRow].UV1.autoOffDelay.ToString();
                            
                        }
                        isRead = false;
                        if ((CsConst.mySends.AddBufToSndList(arayTmp, 0xE018, SubNetID, DevID, false, false, true, false) == true))
                        {
                            lbUV1.Visible = true;
                            if (CsConst.myRevBuf[26] == 0) lbUV1.Text = CsConst.mstrINIDefault.IniReadValue("Public", "99859", "") + txtUVID1.Text + " " +
                                                                        CsConst.Status[0];
                            else lbUV1.Text = CsConst.mstrINIDefault.IniReadValue("Public", "99859", "") + txtUVID1.Text + " " +
                                                                        CsConst.Status[0];
                            
                        }
                        else
                        {
                            lbUV1.Visible = false;
                        }
                        Cursor.Current = Cursors.Default;
                    }
                }
            }
            catch
            {
            }
        }

        private void txtUVRemark1_TextChanged(object sender, EventArgs e)
        {
            if (isRead == true) return;
            if (dgvLogic.RowCount <= 0) return;
            if (dgvLogic.CurrentRow.Index < 0) return;
            mySensor.logic[SelectedRow].UV1.remark = txtUVRemark1.Text;
            mySensor.logic[SelectedRow].UV1.isAutoOff = checkAuto1.Checked;
            if (txtUVAuto1.Text.Length > 0)
                mySensor.logic[SelectedRow].UV1.autoOffDelay = Convert.ToInt32(txtUVAuto1.Text);
        }

        private void txtUVRemark1_Leave(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            txtUVAuto1.Text = HDLPF.IsNumStringMode(txtUVAuto1.Text, 1, 3600);
            byte[] arayTmp = new byte[24];
            arayTmp[0] = Convert.ToByte(txtUVID1.Text);
            byte[] arayRemark = HDLUDP.StringToByte(txtUVRemark1.Text);
            if (arayRemark.Length <= 20)
                Array.Copy(arayRemark, 0, arayTmp, 1, arayRemark.Length);
            else
                Array.Copy(arayRemark, 0, arayTmp, 1, 20);
            if (checkAuto1.Checked)
                arayTmp[21] = 1;
            int num = Convert.ToInt32(txtUVAuto1.Text);
            arayTmp[22] = Convert.ToByte(num / 256);
            arayTmp[23] = Convert.ToByte(num % 256);
            if ((CsConst.mySends.AddBufToSndList(arayTmp, 0x160C, SubNetID, DevID, false, false, true, false) == true))
            {
                
            }
            Cursor.Current = Cursors.Default;
        }

        private void txtUVID2_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (isRead) return;
                if (dgvLogic.RowCount <= 0) return;
                if (dgvLogic.CurrentRow.Index < 0) return;
                if (mySensor.logic[SelectedRow].UV2 == null) mySensor.logic[SelectedRow].UV2 = new UniversalSwitchSet();
                string str = txtUVID2.Text.Trim();
                if (str == "") return;
                if (str.Length >= 3)
                {
                    txtUVID2.Text = HDLPF.IsNumStringMode(str, 201, 248);
                    if (checkboxUV1.Checked && txtUVID1.Text == txtUVID2.Text)
                    {
                        if (txtUVID1.Text == "248") txtUVID2.Text = "247";
                        else txtUVID2.Text = (Convert.ToInt32(txtUVID1.Text) + 1).ToString();
                    }
                    txtUVID2.SelectionStart = txtUVID2.Text.Length;
                    mySensor.logic[SelectedRow].UV2.id = byte.Parse(txtUVID2.Text);
                    if (CsConst.MyEditMode == 1)
                    {
                        Cursor.Current = Cursors.WaitCursor;
                        isRead = true;
                        byte[] arayTmp = new byte[1];
                        arayTmp[0] = byte.Parse(txtUVID2.Text);
                        if ((CsConst.mySends.AddBufToSndList(arayTmp, 0x164C, SubNetID, DevID, false, false, true, false) == true))
                        {
                            byte[] arayRemark = new byte[20];
                            Array.Copy(CsConst.myRevBuf, 27, arayRemark, 0, arayRemark.Length);
                            mySensor.logic[SelectedRow].UV2.remark = HDLPF.Byte2String(arayRemark);
                            txtUVRemark2.Text = mySensor.logic[SelectedRow].UV2.remark;
                            mySensor.logic[SelectedRow].UV2.isAutoOff = (CsConst.myRevBuf[47] == 1);
                            checkAuto2.Checked = mySensor.logic[SelectedRow].UV2.isAutoOff;
                            if (CsConst.myRevBuf[48] * 256 + CsConst.myRevBuf[49] > 3600)
                                mySensor.logic[SelectedRow].UV2.autoOffDelay = 1;
                            else
                                mySensor.logic[SelectedRow].UV2.autoOffDelay = CsConst.myRevBuf[48] * 256 + CsConst.myRevBuf[49];
                            txtUVAuto2.Text = mySensor.logic[SelectedRow].UV2.autoOffDelay.ToString();
                            
                        }
                        isRead = false;
                        if ((CsConst.mySends.AddBufToSndList(arayTmp, 0xE018, SubNetID, DevID, false, false, true, CsConst.minAllWirelessDeviceType.Contains(MyDeviceType)) == true))
                        {
                            lbUV2.Visible = true;
                            if (CsConst.myRevBuf[26] == 0) lbUV2.Text = CsConst.mstrINIDefault.IniReadValue("Public", "99859", "") + txtUVID2.Text + " " +
                                                                        CsConst.Status[0];
                            else lbUV2.Text = CsConst.mstrINIDefault.IniReadValue("Public", "99859", "") + txtUVID2.Text + " " +
                                                                        CsConst.Status[0];
                            
                        }
                        else
                        {
                            lbUV2.Visible = false;
                        }
                    }
                }
            }
            catch
            {
            }
            Cursor.Current = Cursors.Default;
        }

        private void txtUVRemark2_TextChanged(object sender, EventArgs e)
        {
            if (isRead == true) return;
            if (dgvLogic.RowCount <= 0) return;
            if (dgvLogic.CurrentRow.Index < 0) return;
            mySensor.logic[SelectedRow].UV2.remark = txtUVRemark2.Text;
            mySensor.logic[SelectedRow].UV2.isAutoOff = checkAuto2.Checked;
            if (txtUVAuto2.Text.Length > 0)
                mySensor.logic[SelectedRow].UV2.autoOffDelay = Convert.ToInt32(txtUVAuto2.Text);
        }

        private void txtUVRemark2_Leave(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            txtUVAuto2.Text = HDLPF.IsNumStringMode(txtUVAuto2.Text, 1, 3600);
            byte[] arayTmp = new byte[24];
            arayTmp[0] = Convert.ToByte(txtUVID2.Text);
            byte[] arayRemark = HDLUDP.StringToByte(txtUVRemark2.Text);
            if (arayRemark.Length <= 20)
                Array.Copy(arayRemark, 0, arayTmp, 1, arayRemark.Length);
            else
                Array.Copy(arayRemark, 0, arayTmp, 1, 20);
            if (checkAuto2.Checked)
                arayTmp[21] = 1;
            int num = Convert.ToInt32(txtUVAuto2.Text);
            arayTmp[22] = Convert.ToByte(num / 256);
            arayTmp[23] = Convert.ToByte(num % 256);
            if ((CsConst.mySends.AddBufToSndList(arayTmp, 0x160C, SubNetID, DevID, false, false, true, false) == true))
            {
                
            }
            Cursor.Current = Cursors.Default;
        }

        private void txtFrm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar) || e.KeyChar == 8)
                e.Handled = false;
            else
                e.Handled = true;
        }

        private void cbLogicNum_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            if (dgvLogic.RowCount <= 0) return;
            if (dgvLogic.CurrentRow.Index < 0) return;
            mySensor.logic[SelectedRow].Paramters[15] = Convert.ToByte(cbLogicNum.SelectedIndex + 1);
        }

        private void cbLogicStatu_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            if (dgvLogic.RowCount <= 0) return;
            if (dgvLogic.CurrentRow.Index < 0) return;
            mySensor.logic[SelectedRow].Paramters[16] = Convert.ToByte(cbLogicStatu.SelectedIndex);
        }

        private void truetime_TextChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            if (dgvLogic.RowCount <= 0) return;
            if (dgvLogic.CurrentRow.Index < 0) return;
            mySensor.logic[SelectedRow].DelayTimeT = Convert.ToInt32(truetime.Text);
            mySensor.logic[SelectedRow].DelayTimeF = Convert.ToInt32(falsetime.Text);
        }

        private void cbPage_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            tbDown_Click(tbDown, null);
        }

        private void btnLearner_Click(object sender, EventArgs e)
        {
            frmIRlearner frmTmp = new frmIRlearner();
            frmTmp.FormClosed += frmTmp_FormClosed;
            frmTmp.ShowDialog();
        }

        void frmTmp_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                HDLSysPF.addIR(tvIR, tempSend, true);  // 添加已有的列表到窗体
            }
            catch
            {
            }
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {

            try
            {
                isStopDownloadCodes = false;
                tsbar.Visible = true;
                tsbl4.Visible = true;
                MyBackGroup = new BackgroundWorker();
                MyBackGroup.DoWork += new DoWorkEventHandler(calculationWorker_DoWork);
                MyBackGroup.ProgressChanged += new ProgressChangedEventHandler(calculationWorker_ProgressChanged);
                MyBackGroup.WorkerReportsProgress = true;
                MyBackGroup.RunWorkerCompleted += new RunWorkerCompletedEventHandler(calculationWorker_RunWorkerCompleted);
                MyBackGroup.RunWorkerAsync();
                MyBackGroup.WorkerSupportsCancellation = true;
            }
            catch
            {
            }
            Cursor.Current = Cursors.Default;
        }

        void calculationWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                tsbar.Value = 100;
                tsbl4.Text = "100%";
                System.Threading.Thread.Sleep(1000);
                tsbar.Visible = false;
                tsbl4.Visible = false;
            }
            catch
            {
            }
        }

        void calculationWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //throw new NotImplementedException();
            try
            {
                this.tsbar.Value = e.ProgressPercentage;
                this.tsbl4.Text = e.ProgressPercentage.ToString() + "%";
            }
            catch
            {
            }
        }

        void calculationWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                #region
                if (tempSend.IRCodes == null) return;
                Cursor.Current = Cursors.WaitCursor;
                List<string> listCode = new List<string>();
                List<string> listRemark = new List<string>();
                int RowIndex = dgvIR.CurrentRow.Index;
                int RowCount = dgvIR.RowCount;
                for (int i = 0; i < tvIR.Nodes.Count; i++)
                {
                    if (tvIR.Nodes[i].Checked)
                    {
                        for (int j = 0; j < tvIR.Nodes[i].Nodes.Count; j++)
                        {
                            int DeviceID = Convert.ToInt32(tvIR.Nodes[i].Name);
                            int KeyID = Convert.ToInt32(tvIR.Nodes[i].Nodes[j].Name);
                            string strCodes = "";
                            for (int k = 0; k < tempSend.IRCodes.Count; k++)
                            {
                                if (tempSend.IRCodes[k].KeyID == KeyID && tempSend.IRCodes[k].IRLoation == DeviceID)
                                {
                                    strCodes = tempSend.IRCodes[k].Codes;
                                    break;
                                }
                            }
                            if (strCodes != "")
                            {
                                listCode.Add(strCodes);
                                listRemark.Add(tvIR.Nodes[i].Text.ToString() + "-" + tvIR.Nodes[i].Nodes[j].Text.ToString());
                            }
                        }
                    }
                    else
                    {
                        for (int j = 0; j < tvIR.Nodes[i].Nodes.Count; j++)
                        {
                            if (tvIR.Nodes[i].Nodes[j].Checked)
                            {
                                int DeviceID = Convert.ToInt32(tvIR.Nodes[i].Name);
                                int KeyID = Convert.ToInt32(tvIR.Nodes[i].Nodes[j].Name);
                                string strCodes = "";
                                for (int k = 0; k < tempSend.IRCodes.Count; k++)
                                {
                                    if (tempSend.IRCodes[k].KeyID == KeyID && tempSend.IRCodes[k].IRLoation == DeviceID)
                                    {
                                        strCodes = tempSend.IRCodes[k].Codes;
                                        break;
                                    }
                                }
                                if (strCodes != "")
                                {
                                    listCode.Add(strCodes);
                                    listRemark.Add(tvIR.Nodes[i].Text.ToString() + "-" + tvIR.Nodes[i].Nodes[j].Text.ToString());
                                }
                            }
                        }
                    }
                }
                int CodeIndex = 0;
                int FirstIndex = dgvIR.CurrentRow.Index;
                if (MyBackGroup != null && MyBackGroup.IsBusy) MyBackGroup.ReportProgress(0);
                for (int i = RowIndex; i < RowCount; i++)
                {
                    string strCodes = listCode[CodeIndex];
                    string[] ListData = new string[0];
                    if (strCodes.Contains(";"))
                        ListData = strCodes.Split(';');
                    else
                    {
                        ListData = new string[1];
                        ListData[0] = strCodes;
                    }
                    byte[] arayCode = new byte[ListData.Length * 16];
                    for (int j = 0; j < ListData.Length; j++)
                    {
                        byte[] arayCodeTmp = GlobalClass.HexToByte(ListData[j]);
                        Array.Copy(arayCodeTmp, 0, arayCode, 16 * j, 16);
                    }
                    byte[] arayTmp = new byte[3];
                    arayTmp[0] = Convert.ToByte(Convert.ToInt32(dgvIR[0, dgvIR.CurrentRow.Index].Value.ToString()) - 1);
                    arayTmp[1] = arayCode[2];
                    arayTmp[2] = arayCode[3];
                    if (CsConst.mySends.AddBufToSndList(arayTmp, 0x1692, SubNetID, DevID, false, true, true, false) == true)
                    {
                        if (CsConst.myRevBuf[25] == 0xF8)
                        {
                            
                            for (int j = 0; j < ListData.Length; j++)
                            {
                                arayTmp = new byte[16];
                                Array.Copy(arayCode, j * 16, arayTmp, 0, 16);
                                if (CsConst.mySends.AddBufToSndList(arayTmp, 0x1694, SubNetID, DevID, false, true, true, false) == true)
                                {
                                    
                                }
                                else
                                {
                                    Cursor.Current = Cursors.Default;
                                    return;
                                }
                                HDLUDP.TimeBetwnNext(20);
                            }
                            arayTmp = new byte[12];
                            arayTmp[0] = Convert.ToByte(Convert.ToInt32(dgvIR[0, dgvIR.CurrentRow.Index].Value.ToString()) - 1);
                            arayTmp[1] = 0;
                            string strRemark = listRemark[CodeIndex];
                            byte[] arayTmpRemark = HDLUDP.StringToByte(strRemark);
                            if (arayTmpRemark.Length >= 10)
                                Array.Copy(arayTmpRemark, 0, arayTmp, 2, 10);
                            else
                                Array.Copy(arayTmpRemark, 0, arayTmp, 2, arayTmpRemark.Length);
                            if (CsConst.mySends.AddBufToSndList(arayTmp, 0x1690, SubNetID, DevID, false, true, true, false) == true)
                            {
                                
                            }
                            else
                            {
                                Cursor.Current = Cursors.Default;
                                return;
                            }
                            HDLUDP.TimeBetwnNext(20);
                            arayTmp = new byte[12];
                            arayTmp[0] = Convert.ToByte(Convert.ToInt32(dgvIR[0, dgvIR.CurrentRow.Index].Value.ToString()) - 1);
                            arayTmp[1] = 1;
                            if (arayTmpRemark.Length >= 20)
                                Array.Copy(arayTmpRemark, 10, arayTmp, 2, 10);
                            else if (arayTmpRemark.Length > 10 && arayTmpRemark.Length < 20)
                                Array.Copy(arayTmpRemark, 10, arayTmp, 2, arayTmpRemark.Length - 10);
                            if (CsConst.mySends.AddBufToSndList(arayTmp, 0x1690, SubNetID, DevID, false, true, true, false) == true)
                            {
                                
                            }
                            else
                            {
                                Cursor.Current = Cursors.Default;
                                return;
                            }
                            dgvIR.Rows[FirstIndex + CodeIndex].Cells[1].Value = strRemark;
                            dgvIR.Rows[FirstIndex + CodeIndex].Cells[2].Value = CsConst.mstrINIDefault.IniReadValue("public", "00042", "");
                            CodeIndex = CodeIndex + 1;
                            dgvIR.Rows[FirstIndex + CodeIndex].Selected = true;
                            this.dgvIR.CurrentCell = this.dgvIR.Rows[FirstIndex + CodeIndex].Cells[0];
                            if (isStopDownloadCodes) break;
                            if (CodeIndex >= listCode.Count) break;
                        }
                    }
                    else break;
                    if (MyBackGroup != null && MyBackGroup.IsBusy) MyBackGroup.ReportProgress(i * 90 / RowCount);
                }
                if (btnFree.Visible)
                    btnFree_Click(null, null);
                #endregion
            }
            catch
            {
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            try
            {
                isStopDownloadCodes = true;
                if (MyBackGroup != null && MyBackGroup.IsBusy) MyBackGroup.CancelAsync();
            }
            catch
            {
            }
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            byte[] arayTmp = new byte[2];
            arayTmp[0] = Convert.ToByte(Convert.ToInt32(dgvIR[0, dgvIR.CurrentRow.Index].Value.ToString()) - 1);
            if (rb1.Checked) arayTmp[1] = 0;
            else if (rb2.Checked) arayTmp[1] = 1;
            else if (rb3.Checked) arayTmp[1] = 2;
            if (CsConst.mySends.AddBufToSndList(arayTmp, 0x169C, SubNetID, DevID, false, false, false, false) == true)
            {
                
            }

            Cursor.Current = Cursors.Default;
        }

        private void checklistLED_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (isRead) return;
            mySensor.ONLEDs[e.Index] = Convert.ToByte(e.NewValue);
        }

        private void checklistSensor_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (isRead) return;
            mySensor.EnableSensors[e.Index] = Convert.ToByte(e.NewValue);
        }

        private void sbSensitivity4_ValueChanged(object sender, EventArgs e)
        {
            lbSensitivity4.Text = sbSensitivity4.Value.ToString() + "C";
            if (isRead) return;
            if (sbSensitivity4.Value >= 0)
                mySensor.ParamSensors[0] = Convert.ToByte(sbSensitivity4.Value);
            else
                mySensor.ParamSensors[0] = Convert.ToByte(128 + Math.Abs(sbSensitivity4.Value));
        }

        private void sbSensitivity1_ValueChanged(object sender, EventArgs e)
        {
            if (sbSensitivity1.Maximum == 100)
                lbSensitivity1.Text = sbSensitivity1.Value.ToString() + "%";
            else
                lbSensitivity1.Text = (sbSensitivity1.Value * 10).ToString() + "%";
            if (isRead) return;
            mySensor.ParamSensors[1] = Convert.ToByte(sbSensitivity1.Value);
        }

        private void chbEnable_CheckedChanged(object sender, EventArgs e)
        {
            txtLux.Enabled = chbEnable.Checked;
            lbCycle.Enabled = chbEnable.Checked;
            cbCycle.Enabled = chbEnable.Checked;
            panel19.Enabled = chbEnable.Checked;
            if (isRead) return;
            if (chbEnable.Checked)
                mySensor.EnableBroads[0] = 1;
            else
                mySensor.EnableBroads[0] = 0;
        }

        private void txtLux_TextChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            if (txtLux.Text.Length > 0)
            {
                int num = Convert.ToInt32(txtLux.Text);
                mySensor.EnableBroads[1] = Convert.ToByte(num / 256);
                mySensor.EnableBroads[2] = Convert.ToByte(num % 256);
            }
        }

        private void cbCycle_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            string str = cbCycle.Text;
            int num = Convert.ToInt32(Convert.ToSingle(str) * 10);
            mySensor.EnableBroads[7] = Convert.ToByte(num);
        }

        private void cbParam1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            string str = cbParam1.Text;
            int num = Convert.ToInt32(Convert.ToSingle(str) * 100);
            mySensor.EnableBroads[3] = Convert.ToByte(num / 256);
            mySensor.EnableBroads[4] = Convert.ToByte(num % 256);
        }

        private void cbParam2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            string str = cbParam2.Text;
            int num = Convert.ToInt32(Convert.ToSingle(str) * 100);
            mySensor.EnableBroads[5] = Convert.ToByte(num / 256);
            mySensor.EnableBroads[6] = Convert.ToByte(num % 256);
        }

        private void sbLimit_ValueChanged(object sender, EventArgs e)
        {
            lbLimitValue.Text = sbLimit.Value.ToString() + "%";
            if (isRead) return;
            mySensor.EnableBroads[8] = Convert.ToByte(sbLimit.Value);
        }

        private void dgvSec_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (mySensor == null) return;
                if (mySensor.fireset == null) return;
                if (e.RowIndex == -1) return;
                if (e.ColumnIndex == -1) return;
                if (dgvSec[e.ColumnIndex, e.RowIndex].Value == null) dgvSec[e.ColumnIndex, e.RowIndex].Value = "";
                for (int i = 0; i < dgvSec.SelectedRows.Count; i++)
                {
                    UVCMD.SecurityInfo tempfire = mySensor.fireset[dgvSec.SelectedRows[i].Index];
                    dgvSec.SelectedRows[i].Cells[e.ColumnIndex].Value = dgvSec[e.ColumnIndex, e.RowIndex].Value.ToString();
                    switch (e.ColumnIndex)
                    {
                        case 2:
                            if (dgvSec[2, dgvSec.SelectedRows[i].Index].Value.ToString().ToLower() == "true")
                            {
                                if (dgvSec.SelectedRows[i].Index < 2)
                                    tempfire.bytTerms = 1;
                                else
                                    tempfire.bytTerms = 2;
                            }
                            else
                            {
                                tempfire.bytTerms = 0;
                            }
                            break;

                        case 3:
                            if (dgvSec[3, dgvSec.SelectedRows[i].Index].Value == null)
                            {
                                dgvSec[3, dgvSec.SelectedRows[i].Index].Value = "";
                            }
                            string strTmp = dgvSec[3, dgvSec.SelectedRows[i].Index].Value.ToString();
                            tempfire.strRemark = strTmp;

                            break;

                        case 4:
                            strTmp = dgvSec[4, dgvSec.SelectedRows[i].Index].Value.ToString();
                            dgvSec[4, dgvSec.SelectedRows[i].Index].Value = HDLPF.IsNumStringMode(strTmp, 0, 254);
                            tempfire.bytSubID = byte.Parse(dgvSec[4, dgvSec.SelectedRows[i].Index].Value.ToString());
                            break;

                        case 5:
                            strTmp = dgvSec[5, dgvSec.SelectedRows[i].Index].Value.ToString();
                            dgvSec[5, dgvSec.SelectedRows[i].Index].Value = HDLPF.IsNumStringMode(strTmp, 0, 254);
                            tempfire.bytDevID = byte.Parse(dgvSec[5, dgvSec.SelectedRows[i].Index].Value.ToString());
                            break;

                        case 6:
                            strTmp = dgvSec[6, dgvSec.SelectedRows[i].Index].Value.ToString();
                            dgvSec[6, dgvSec.SelectedRows[i].Index].Value = HDLPF.IsNumStringMode(strTmp, 0, 254);
                            tempfire.bytArea = byte.Parse(dgvSec[6, dgvSec.SelectedRows[i].Index].Value.ToString());
                            break;
                    }
                }
                if (e.ColumnIndex == 2)
                {
                    if (dgvSec[2, e.RowIndex].Value.ToString().ToLower() == "true")
                    {
                        if (e.RowIndex < 2)
                            mySensor.fireset[e.RowIndex].bytTerms = 1;
                        else
                            mySensor.fireset[e.RowIndex].bytTerms = 2;
                    }
                    else
                    {
                        mySensor.fireset[e.RowIndex].bytTerms = 0;
                    }
                }
                if (e.ColumnIndex == 3)
                {
                    string strTmp = dgvSec[3, e.RowIndex].Value.ToString();
                    mySensor.fireset[e.RowIndex].strRemark = strTmp;
                }
                if (e.ColumnIndex == 4)
                {
                    string strTmp = dgvSec[4, e.RowIndex].Value.ToString();
                    dgvSec[4, e.RowIndex].Value = HDLPF.IsNumStringMode(strTmp, 0, 254);
                    mySensor.fireset[e.RowIndex].bytSubID = byte.Parse(dgvSec[4, e.RowIndex].Value.ToString());
                }
                if (e.ColumnIndex == 5)
                {
                    string strTmp = dgvSec[5, e.RowIndex].Value.ToString();
                    dgvSec[5, e.RowIndex].Value = HDLPF.IsNumStringMode(strTmp, 0, 254);
                    mySensor.fireset[e.RowIndex].bytDevID = byte.Parse(dgvSec[5, e.RowIndex].Value.ToString());
                }
                if (e.ColumnIndex == 6)
                {
                    string strTmp = dgvSec[6, e.RowIndex].Value.ToString();
                    dgvSec[6, e.RowIndex].Value = HDLPF.IsNumStringMode(strTmp, 0, 254);
                    mySensor.fireset[e.RowIndex].bytArea = byte.Parse(dgvSec[6, e.RowIndex].Value.ToString());
                }
            }
            catch
            {
            }
        }

        private void dgvSec_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            dgvSec.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void tab8in1_SelectedIndexChanged(object sender, EventArgs e)
        {
            lbUV1.Visible = (tab8in1.SelectedTab.Name == "tabPage4");
            lbUV2.Visible = (tab8in1.SelectedTab.Name == "tabPage4");
            if (tab8in1.SelectedTab.Name == "tabPage1") MyActivePage = 1;
            else if (tab8in1.SelectedTab.Name == "tabPage2")
            {
                isRead = true;
                if (cbPage.SelectedIndex < 0) cbPage.SelectedIndex = 0;
                MyActivePage = 2;
            }
            else if (tab8in1.SelectedTab.Name == "tabPage3") MyActivePage = 3;
            else if (tab8in1.SelectedTab.Name == "tabPage4") MyActivePage = 4;
            else if (tab8in1.SelectedTab.Name == "tabPage5") MyActivePage = 5;
            if (CsConst.MyEditMode == 1)
            {
                if (tab8in1.SelectedTab.Name != "tabPage1")
                {
                    if (mySensor.MyRead2UpFlags[MyActivePage - 1] == false)
                    {
                        tbDown_Click(tbDown, null);
                    }
                    else
                    {
                        //基本信息
                        if (tab8in1.SelectedTab.Name == "tabPage2") showIRReceiveInfo();
                        else if (tab8in1.SelectedTab.Name == "tabPage3") showSensorInfo();
                        else if (tab8in1.SelectedTab.Name == "tabPage4") showLogicInfo();
                        else if (tab8in1.SelectedTab.Name == "tabPage5") showSecurityInfo();
                    }
                }
            }
        }

        private void rbOr_CheckedChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            if (dgvLogic.RowCount < 0) return;
            if (dgvLogic.CurrentRow.Index < 0) return;
            if (rbOr.Checked)
                mySensor.logic[SelectedRow].bytRelation = 0;
            else if (rbAnd.Checked)
                mySensor.logic[SelectedRow].bytRelation = 1;
        }

        private void tvIR_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (e.Action == TreeViewAction.ByMouse)
            {
                if (e.Node.Checked)
                {
                    //选中节点选中状态之后，选中父节点的选中状态
                    HDLPF.setChildNodeCheckedState(e.Node, true);
                    if (e.Node.Parent != null)
                    {
                        bool isAllTrue = true;
                        for (int i = 0; i < e.Node.Parent.Nodes.Count; i++)
                        {
                            if (e.Node.Parent.Nodes[i].Checked == false)
                            {
                                isAllTrue = false;
                                break;
                            }
                        }
                        if (isAllTrue) HDLPF.setParentNodeCheckedState(e.Node, true);
                    }
                }
                else
                {
                    //取消节点之后，取消节点的所有子节点
                    HDLPF.setChildNodeCheckedState(e.Node, false);
                    //如果节点存在父节点，取消父节点的选中状态
                    if (e.Node.Parent != null)
                    {
                        HDLPF.setParentNodeCheckedState(e.Node, false);
                    }
                }
            }
        }

        private void btnTargets_Click(object sender, EventArgs e)
        {
            Form form = null;
            bool isOpen = true;
            foreach (Form frm in Application.OpenForms)
            {
                if (frm.Name == "FrmReceiverTargets")
                {
                    if ((frm as FrmReceiverTargets).DIndex == mintIDIndex)
                    {
                        form = frm;
                        form.TopMost = true;
                        form.WindowState = FormWindowState.Normal;
                        form.Activate();
                        form.TopMost = false;
                        isOpen = false;
                        break;
                    }
                }
            }
            if (isOpen)
            {
                FrmReceiverTargets frmTmp = new FrmReceiverTargets(myDevName, mySensor, cbPage.SelectedIndex, dgvKey.CurrentRow.Index, MyDeviceType);
                frmTmp.Show();
            }
        }

        private void dgvKey_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            btnTargets_Click(null, null);
        }

        private void dgvKey_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (mySensor == null) return;
                if (mySensor.IrReceiver == null) return;
                if ((e.RowIndex == -1) || (e.ColumnIndex == -1)) return;
                if (dgvKey.SelectedRows.Count == 0) return;
                if (isRead) return;
                if (dgvKey[e.ColumnIndex, e.RowIndex].Value == null) dgvKey[e.ColumnIndex, e.RowIndex].Value = "";
                for (int i = 0; i < dgvKey.SelectedRows.Count; i++)
                {
                    dgvKey.SelectedRows[i].Cells[e.ColumnIndex].Value = dgvKey[e.ColumnIndex, e.RowIndex].Value.ToString();
                    string strTmp = "";
                    switch (e.ColumnIndex)
                    {
                        case 1:
                            strTmp = dgvKey[1, dgvKey.SelectedRows[i].Index].Value.ToString();
                            mySensor.IrReceiver[dgvKey.SelectedRows[i].Index].IRBtnRemark = strTmp;
                            break;
                        case 2:
                            strTmp = dgvKey[2, dgvKey.SelectedRows[i].Index].Value.ToString();
                            int Mode = Convert.ToByte(clK3.Items.IndexOf(strTmp));
                            if (Mode == 0) mySensor.IrReceiver[dgvKey.SelectedRows[i].Index].IRBtnModel = 0;
                            else if (Mode == 1) mySensor.IrReceiver[dgvKey.SelectedRows[i].Index].IRBtnModel = 2;
                            else if (Mode == 2) mySensor.IrReceiver[dgvKey.SelectedRows[i].Index].IRBtnModel = 3;
                            else if (Mode == 3) mySensor.IrReceiver[dgvKey.SelectedRows[i].Index].IRBtnModel = 1;
                            else if (Mode == 4) mySensor.IrReceiver[dgvKey.SelectedRows[i].Index].IRBtnModel = 4;
                            else if (Mode == 5) mySensor.IrReceiver[dgvKey.SelectedRows[i].Index].IRBtnModel = 5;
                            else if (Mode == 6) mySensor.IrReceiver[dgvKey.SelectedRows[i].Index].IRBtnModel = 7;
                            break;
                    }
                }
                if (e.ColumnIndex == 1)
                {
                    string strTmp = dgvKey[1, e.RowIndex].Value.ToString();
                    mySensor.IrReceiver[e.RowIndex].IRBtnRemark = dgvKey[1, e.RowIndex].Value.ToString();
                }
                else if (e.ColumnIndex == 2)
                {
                    string strTmp = dgvKey[2, e.RowIndex].Value.ToString();
                    int Mode = Convert.ToByte(clK3.Items.IndexOf(strTmp));
                    if (Mode == 0) mySensor.IrReceiver[e.RowIndex].IRBtnModel = 0;
                    else if (Mode == 1) mySensor.IrReceiver[e.RowIndex].IRBtnModel = 2;
                    else if (Mode == 2) mySensor.IrReceiver[e.RowIndex].IRBtnModel = 3;
                    else if (Mode == 3) mySensor.IrReceiver[e.RowIndex].IRBtnModel = 1;
                    else if (Mode == 4) mySensor.IrReceiver[e.RowIndex].IRBtnModel = 4;
                    else if (Mode == 5) mySensor.IrReceiver[e.RowIndex].IRBtnModel = 5;
                    else if (Mode == 6) mySensor.IrReceiver[e.RowIndex].IRBtnModel = 7;
                }
            }
            catch
            {
            }
        }

        private void dgvKey_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            dgvKey.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void dgvLogic_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            dgvLogic.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void dgvLogic_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (mySensor == null) return;
                if (mySensor.logic == null) return;
                if ((e.RowIndex == -1) || (e.ColumnIndex == -1)) return;
                if (dgvLogic.SelectedRows.Count == 0) return;
                if (isRead) return;
                if (dgvLogic[e.ColumnIndex, e.RowIndex].Value == null) dgvLogic[e.ColumnIndex, e.RowIndex].Value = "";
                for (int i = 0; i < dgvLogic.SelectedRows.Count; i++)
                {
                    dgvLogic.SelectedRows[i].Cells[e.ColumnIndex].Value = dgvLogic[e.ColumnIndex, e.RowIndex].Value.ToString();
                    string strTmp = "";
                    switch (e.ColumnIndex)
                    {
                        case 1:
                            strTmp = dgvLogic[1, dgvLogic.SelectedRows[i].Index].Value.ToString();
                            mySensor.logic[dgvLogic.SelectedRows[i].Index].Remarklogic = strTmp;
                            break;
                        case 2:
                            strTmp = dgvLogic[2, dgvLogic.SelectedRows[i].Index].Value.ToString();
                            mySensor.logic[dgvLogic.SelectedRows[i].Index].Enabled = Convert.ToByte(clL3.Items.IndexOf(strTmp));
                            break;
                    }
                }
                if (e.ColumnIndex == 1)
                {
                    string strTmp = dgvLogic[1, e.RowIndex].Value.ToString();
                    mySensor.logic[e.RowIndex].Remarklogic = dgvLogic[1, e.RowIndex].Value.ToString();
                }
                else if (e.ColumnIndex == 2)
                {
                    string strTmp = dgvLogic[2, e.RowIndex].Value.ToString();
                    mySensor.logic[e.RowIndex].Enabled = Convert.ToByte(clL3.Items.IndexOf(strTmp));
                }
            }
            catch
            {
            }
        }

        private void btnTure_Click(object sender, EventArgs e)
        {
            Byte[] PageID = new Byte[4];
            if (dgvLogic.SelectedRows != null && dgvLogic.SelectedRows.Count > 0)
            {
                PageID[0] = (Byte)dgvLogic.SelectedRows[0].Index;
            }
            frmCmdSetup CmdSetup = new frmCmdSetup(mySensor, myDevName, MyDeviceType, PageID);
            CmdSetup.ShowDialog();
        }

        private void chbUpdata_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void btnAdjustLux_Click(object sender, EventArgs e)
        {
            FrmCalibrationLux frmTmp = new FrmCalibrationLux(myDevName, MyDeviceType);
            frmTmp.ShowDialog();
        }

    }
}
