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
    public partial class FrmMiniSensor : Form
    {
        [DllImport("user32.dll")]
        public static extern Boolean FlashWindow(IntPtr hwnd, Boolean bInvert);
        private MiniSensor mySensor = null;
        private string myDevName = null;
        private int mintIDIndex = -1;
        private int MyintDeviceType = -1;
        private int MyActivePage = 0; //按页面上传下载
        private byte SubNetID;
        private byte DevID;
        private bool isDealing = false;
        private int mySelectedLNo = 0; //用于存储当前选中的逻辑块
        private SingleChn sbTest = new SingleChn();
        private Byte bIsUlOrPir = 0;  // 0 ul sensor; 1 pir sensor

        public FrmMiniSensor()
        {
            InitializeComponent();
        }
        public FrmMiniSensor(MiniSensor mysensor, string strName, int intDIndex, int intDeviceTyp)
        {
            InitializeComponent();
            this.mySensor = mysensor;
            this.myDevName = strName;
            this.mintIDIndex = intDIndex;
            string strDevName = strName.Split('\\')[0].ToString();
            this.MyintDeviceType = intDeviceTyp;

            HDLSysPF.DisplayDeviceNameModeDescription(strName, MyintDeviceType, cboDevice, tbModel, tbDescription);

            SubNetID = Convert.ToByte(strDevName.Split('-')[0]);
            DevID = Convert.ToByte(strDevName.Split('-')[1]);
            tsl3.Text = strName;           
        }

        void sbTest_TextChanged(object sender, SingleChn.TextChangedEventHandler e)
        {
            Cursor.Current = Cursors.WaitCursor;
            byte[] arayTmp = new byte[2];
            arayTmp[0] = 1;
            arayTmp[1] = Convert.ToByte(sbTest.Text);
            if (CsConst.mySends.AddBufToSndList(arayTmp, 0x3502, SubNetID, DevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(MyintDeviceType)) == true)
            {
                Cursor.Current = Cursors.Default;
                tbDown_Click(tbDown, null);
                return;
            }
            Cursor.Current = Cursors.Default;
        }


        void InitialFormCtrlsTextOrItems()
        {
            checklistLED.Items.Clear();
            checklistLED.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00602", ""));

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

            checkBox1.Checked = false;

            cbTemp1.Items.Clear();
            cbTemp2.Items.Clear();
            for (int i = 0; i < 80; i++)
            {
                cbTemp1.Items.Add((i - 20).ToString());
                cbTemp2.Items.Add((i - 20).ToString());
            }
            cbUV1.Items.Clear();
            cbUV2.Items.Clear();
            cbUV1.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99614", ""));
            cbUV1.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99615", ""));
            cbUV2.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99614", ""));
            cbUV2.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99615", ""));
            cbLogicStatu.Items.Clear();
            cbLogicStatu.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99856", ""));
            cbLogicStatu.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99857", ""));
            clL3.Items.Clear();
            clL3.Items.Add(CsConst.WholeTextsList[1775].sDisplayName);
            clL3.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00042", ""));

            cbUL.Items.Clear();
            cbUL.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99852", ""));
            cbUL.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99853", ""));

            grpDimming.Controls.Add(sbTest);
            sbTest.Left = lbCurDimming.Left;
            sbTest.Top = lbCurDimming.Top + 30;
        }

        void SetCtrlsVisbleWithDifferentDeviceType()
        {
            toolStrip1.Visible = (CsConst.MyEditMode == 0);
            if (MyintDeviceType == 323 || MyintDeviceType == 327 || MyintDeviceType == 334)
            {
                checklistSensor.Items.Clear();
                checklistSensor.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00611", ""));
                checklistSensor.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00612", ""));
                checklistSensor.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00616", ""));
                checklistSensor.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00617", ""));
                checklistSensor.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00618", ""));

                sbSensitivity2.Tag = "2";
                bIsUlOrPir = 1;
                lbUI.Text = CsConst.mstrINIDefault.IniReadValue("Public", "99542", "");
                lbStatus2.Text = CsConst.mstrINIDefault.IniReadValue("Public", "99541", "");
                chbUL.Text = CsConst.mstrINIDefault.IniReadValue("Public", "99541", "");
                chb3.Text = CsConst.mstrINIDefault.IniReadValue("Public", "99541", "");
            }
            else
            {
                bIsUlOrPir = 0;
                sbSensitivity2.Tag = "3";
                checklistSensor.Items.Clear();
                checklistSensor.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00611", ""));
                checklistSensor.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00613", ""));
                checklistSensor.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00616", ""));
                checklistSensor.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00617", ""));
                checklistSensor.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00618", ""));
            }

            if (MyintDeviceType == 326 || MyintDeviceType == 327)
                sbSensitivity2.Maximum = 10;
            else
                sbSensitivity2.Maximum = 100;

            gbConstant.Visible = (MyintDeviceType == 326 || MyintDeviceType == 327 || MyintDeviceType == 334 || MyintDeviceType == 335);
            if (MyintDeviceType == 1502) grpLed.Visible = false;
        }

        private void FrmMSPUcs_Load(object sender, EventArgs e)
        {
            InitialFormCtrlsTextOrItems();
        }

        private void tbDown_Click(object sender, EventArgs e)
        {
            isDealing = true;
            byte bytTag = Convert.ToByte(((ToolStripButton)sender).Tag.ToString());
            if (tabControl.SelectedTab.Name == "tabPage2")
            {
                if (MyintDeviceType == 1502 && bytTag == 0)
                {
                    byte[] arayTmp = new byte[0];
                    if (CsConst.mySends.AddBufToSndList(arayTmp, 0x3500, SubNetID, DevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(MyintDeviceType)) == true)
                    {
                        if (CsConst.myRevBuf[27] <= 100)
                            sbTest.Text = CsConst.myRevBuf[27].ToString();
                        lbCurDimmingValue.Text = CsConst.myRevBuf[28].ToString();
                    }
                    else
                    {
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                }
            }
            bool blnShowMsg = (CsConst.MyEditMode != 1);
            if (HDLPF.UploadOrReadGoOn(this.Text, bytTag, blnShowMsg))
            {
                Cursor.Current = Cursors.WaitCursor;

                CsConst.MyUPload2DownLists = new List<byte[]>();

                string strName = myDevName.Split('\\')[0].ToString();
                byte bytSubID = byte.Parse(strName.Split('-')[0]);
                byte bytDevID = byte.Parse(strName.Split('-')[1]);
                int num1 = 0;
                int num2 = 0;
                if (tabControl.SelectedTab.Name == "tabPage1" && bytTag == 1)
                {
                    if (dgvLogic.RowCount > 0 && dgvLogic.CurrentRow.Index >= 0)
                        num1 = Convert.ToInt32(mySelectedLNo + 1);
                }
                byte[] ArayRelay = new byte[] { SubNetID, DevID, (byte)(MyintDeviceType / 256), (byte)(MyintDeviceType % 256)
                    , (byte)MyActivePage,(byte)(mintIDIndex/256),(byte)(mintIDIndex%256),(byte)num1,(byte)num2  };
                CsConst.MyUPload2DownLists.Add(ArayRelay);
                CsConst.MyUpload2Down = Convert.ToByte(((ToolStripButton)sender).Tag.ToString());
                FrmDownloadShow Frm = new FrmDownloadShow();
                Frm.FormClosing += new FormClosingEventHandler(UpdateDisplayInformationAccordingly);
                Frm.ShowDialog();
            }
            isDealing = false;
        }

        void UpdateDisplayInformationAccordingly(object sender, FormClosingEventArgs e)
        {
            switch (tabControl.SelectedTab.Name)
            {
                case "tabBasic": showBasicInfo(); break;
                case "tabLogic": showLogicInfo(); break;
                case "tabSecurity": showSecInfo(); break;
                case "tabTest": showSimulateInfo(); break;
            }
            this.TopMost = true;
            this.WindowState = FormWindowState.Normal;
            this.Activate();
            this.TopMost = false;
        }

        private void showSimulateInfo()
        {
            try
            {
                isDealing = true;
                if (mySensor.SimulateEnable[0] == 1) lbState.Text = CsConst.mstrINIDefault.IniReadValue("Public", "99619", "");
                else lbState.Text = CsConst.mstrINIDefault.IniReadValue("Public", "99618", "");
                if (mySensor.SimulateEnable[2] == 1) chb1.Checked = true;
                else chb1.Checked = false;
                if (mySensor.SimulateEnable[4] == 1) chb3.Checked = true;
                else chb3.Checked = false;
                int num = mySensor.ParamSimulate[1] * 256 + mySensor.ParamSimulate[2];
                if (num <= sb1.Maximum)
                    sb1.Value = num;
                if (mySensor.ParamSimulate[4] <= sb3.Maximum)
                    sb3.Value = mySensor.ParamSimulate[4];
                sb1_ValueChanged(sb1, null);
                sb3_ValueChanged(sb3, null);
            }
            catch
            {
            }
            isDealing = false;
        }

        private void showBasicInfo()
        {
            try
            {
                isDealing = true;
                for (int i = 0; i < 1; i++)
                {
                    if (mySensor.ONLEDs[i] == 1) checklistLED.SetItemChecked(i, true);
                    else checklistLED.SetItemChecked(i, false);
                }
                if (mySensor.EnableSensors[1] == 1) checklistSensor.SetItemChecked(0, true);
                else checklistSensor.SetItemChecked(0, false);
                if (mySensor.EnableSensors[3] == 1) checklistSensor.SetItemChecked(1, true);
                else checklistSensor.SetItemChecked(1, false);
                if (mySensor.EnableSensors[5] == 1) checklistSensor.SetItemChecked(2, true);
                else checklistSensor.SetItemChecked(2, false);
                if (mySensor.EnableSensors[6] == 1) checklistSensor.SetItemChecked(3, true);
                else checklistSensor.SetItemChecked(3, false);
                if (mySensor.EnableSensors[7] == 1) checklistSensor.SetItemChecked(4, true);
                else checklistSensor.SetItemChecked(4, false);

                if (sbSensitivity2.Tag.ToString() == "2")
                {
                    if (1 <= mySensor.ParamSensors[2] && mySensor.ParamSensors[2] <= 100) sbSensitivity2.Value = mySensor.ParamSensors[2];
                }
                else if (sbSensitivity2.Tag.ToString() == "3")
                {
                    if (1 <= mySensor.ParamSensors[3] && mySensor.ParamSensors[3] <= 100) sbSensitivity2.Value = mySensor.ParamSensors[3];
                }

                if (gbConstant.Visible == true)
                {
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
            }
            catch
            {
            }
            isDealing = false;

        }

        private void  showLogicInfo()
        {
            try
            {
                isDealing = true;
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
            isDealing = false;
            for (int i = 0; i < dgvLogic.Rows.Count; i++)
            {
                if (dgvLogic.Rows[i].Index == mySelectedLNo)
                    dgvLogic.Rows[i].Selected = true;
                else
                    dgvLogic.Rows[i].Selected = false;
            }
            dgvLogic_CellClick(dgvLogic, new DataGridViewCellEventArgs(0, mySelectedLNo));
        }

        private void DisplaySomeLogicInformation(int Index)
        {
            if (mySelectedLNo == -1) return;
            if (mySensor == null) return;
            if (mySensor.logic == null) return;
            isDealing = true;
            try
            {
                SensorLogic oLogic = mySensor.logic[Index];

                checkboxBright.Checked = Convert.ToBoolean(oLogic.EnableSensors[1]);
                chbUL.Checked = Convert.ToBoolean(oLogic.EnableSensors[3]);
                checkboxUV1.Checked = Convert.ToBoolean(oLogic.EnableSensors[5]);
                checkboxUV2.Checked = Convert.ToBoolean(oLogic.EnableSensors[6]);
                CheckBoxLogic.Checked = Convert.ToBoolean(oLogic.EnableSensors[7]);

                if (oLogic.bytRelation == 0) rbOr.Checked = true;
                else if (oLogic.bytRelation == 1) rbAnd.Checked = true;

                if (oLogic.Paramters[0] < cbTemp1.Items.Count) cbTemp1.SelectedIndex = oLogic.Paramters[0];
                if (oLogic.Paramters[1] < cbTemp2.Items.Count) cbTemp2.SelectedIndex = oLogic.Paramters[1];
                if (cbTemp1.SelectedIndex < 0) cbTemp1.SelectedIndex = 0;
                if (cbTemp2.SelectedIndex < 0) cbTemp2.SelectedIndex = cbTemp1.SelectedIndex;


                if ((0 <= Convert.ToDecimal(oLogic.Paramters[2] * 256 + oLogic.Paramters[3])) && (Convert.ToDecimal(oLogic.Paramters[2] * 256 + oLogic.Paramters[3]) <= 5000))
                    NumBr1.Value = Convert.ToDecimal(oLogic.Paramters[2] * 256 + oLogic.Paramters[3]);
                if ((0 <= Convert.ToDecimal(oLogic.Paramters[4] * 256 + oLogic.Paramters[5])) && (Convert.ToDecimal(oLogic.Paramters[4] * 256 + oLogic.Paramters[5]) <= 5000))
                    NumBr2.Value = Convert.ToDecimal(oLogic.Paramters[4] * 256 + oLogic.Paramters[5]);
                if (oLogic.Paramters[7] < 2)
                    cbUL.Text = cbUL.Items[oLogic.Paramters[7]].ToString();
                if (cbUL.SelectedIndex < 0) cbUL.SelectedIndex = 0;

                if (bIsUlOrPir == 1) // PIR sensor
                {
                    chbUL.Checked = Convert.ToBoolean(oLogic.EnableSensors[2]);
                    if (oLogic.Paramters[6] < 2)
                        cbUL.Text = cbUL.Items[oLogic.Paramters[6]].ToString();
                }
                else // UL sensor
                {
                    chbUL.Checked = Convert.ToBoolean(oLogic.EnableSensors[3]);
                    if (oLogic.Paramters[7] < 2)
                        cbUL.Text = cbUL.Items[oLogic.Paramters[7]].ToString();
                }

                if (1 <= oLogic.Paramters[9] && oLogic.Paramters[9] <= 24)
                    cbLogicNum.SelectedIndex = oLogic.Paramters[9] - 1;
                else
                    cbLogicNum.SelectedIndex = 0;
                if (oLogic.Paramters[10] < cbLogicStatu.Items.Count)
                    cbLogicStatu.SelectedIndex = oLogic.Paramters[10];
                else
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
            isDealing = false;
            txtUVID1_TextChanged(null, null);
            txtUVID2_TextChanged(null, null);
            chbTemp_CheckedChanged(checkboxBright, null);
            chbTemp_CheckedChanged(chbUL, null);
            chbTemp_CheckedChanged(checkboxUV1, null);
            chbTemp_CheckedChanged(checkboxUV2, null);
            chbTemp_CheckedChanged(CheckBoxLogic, null);

        }

        private void showSecInfo()
        {
            dgvSec.Rows.Clear();
            if (mySensor.fireset != null && mySensor.fireset.Count != 0)
            {
                for (int i = 0; i < mySensor.fireset.Count; i++)
                {
                    UVCMD.SecurityInfo reader = mySensor.fireset[i];
                    string strHint = strHint = chbUL.Text;
                    object[] obj = new object[]{dgvSec.RowCount+1,strHint, (reader.bytTerms==1),
                        reader.strRemark,reader.bytSubID.ToString(),reader.bytDevID.ToString(), reader.bytArea.ToString()};
                    dgvSec.Rows.Add(obj);
                }
            }
        }

        private void FrmMSPU_Shown(object sender, EventArgs e)
        {
            SetCtrlsVisbleWithDifferentDeviceType();
            if (CsConst.MyEditMode == 0)
            {
                btn1.Visible = false;
                btn2.Visible = false;
                groupBox2.Visible = false;
            }
            else if (CsConst.MyEditMode == 1) //在线模式
            {
                MyActivePage = 1;
                tbDown_Click(tbDown, null);

                timerSensor_Tick(null, null);
                btnBroadcast1_Click(null, null);
            }
        }

        private void timerSensor_Tick(object sender, EventArgs e)
        {
            if (isDealing) return;
            byte[] ArayTmp = new byte[0];
            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1604, SubNetID, DevID, false, false, true, CsConst.minAllWirelessDeviceType.Contains(MyintDeviceType)) == true)
            {
                lbStatus6.Text = CsConst.myRevBuf[27] * 256 + CsConst.myRevBuf[28] + "Lux";
                if (bIsUlOrPir == 0)
                {
                    if (CsConst.myRevBuf[37] == 1) lbStatus5.Text = CsConst.mstrINIDefault.IniReadValue("Public", "99851", "");
                    else lbStatus5.Text = CsConst.mstrINIDefault.IniReadValue("Public", "99850", "");
                }
                else if (bIsUlOrPir == 1) // PIR 
                {
                    if (CsConst.myRevBuf[32] == 1) lbStatus5.Text = CsConst.mstrINIDefault.IniReadValue("Public", "99851", "");
                    else lbStatus5.Text = CsConst.mstrINIDefault.IniReadValue("Public", "99850", "");
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            timerSensor.Enabled = checkBox1.Checked;
        }

        private void btn1_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            (sender as Button).Enabled = false;
            int tag = Convert.ToInt32((sender as Button).Tag);
            byte[] ArayTmp = new byte[16];

            if (tag == 1)
            {
                ArayTmp[0] = 1;
                if (chb1.Checked) ArayTmp[2] = 1;
                if (chb3.Checked) ArayTmp[14] = 1;

                if (bIsUlOrPir ==1) // Pir
                {
                    if (chb3.Checked) ArayTmp[6] = 1;
                }
                else // ul sensor
                {
                    if (chb3.Checked) ArayTmp[14] = 1;
                }

                ArayTmp[8] = Convert.ToByte(sb1.Value / 256);
                ArayTmp[9] = Convert.ToByte(sb1.Value % 256);
                ArayTmp[15] = Convert.ToByte(sb3.Value);
                ArayTmp[10] = 20;
                ArayTmp[11] = 2;
                ArayTmp[12] = 2;
            }
            else if (tag == 0)
            {
                ArayTmp[10] = 20;
                ArayTmp[11] = 2;
                ArayTmp[12] = 2;
            }
            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1622, SubNetID, DevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(MyintDeviceType)) == true)
            {
                if (tag == 1)
                {
                    lbState.Text = CsConst.mstrINIDefault.IniReadValue("Public", "99619", "");
                }
                else
                {
                    lbState.Text = CsConst.mstrINIDefault.IniReadValue("Public", "99618", "");
                    chb1.Checked = false;
                    chb3.Checked = false;
                    sb1.Value = 0;
                    sb3.Value = 0;
                }
            }
            Cursor.Current = Cursors.Default;
            (sender as Button).Enabled = true;
        }


        private void sb3_ValueChanged(object sender, EventArgs e)
        {
            if (sb3.Value == 1) lb3.Text = CsConst.mstrINIDefault.IniReadValue("Public", "99855", "");
            else lb3.Text = CsConst.mstrINIDefault.IniReadValue("Public", "99854", "");
            mySensor.ParamSimulate[4] = Convert.ToByte(sb3.Value);
        }



        private void sbSensitivity2_ValueChanged(object sender, EventArgs e)
        {
            lbSensitivity2.Text = sbSensitivity2.Value.ToString() + "%";
            if (isDealing) return;
            if (mySensor == null) return;
            if (mySensor.ParamSensors == null) return;
            try
            {
                if ((sender as ScrollBar).Tag == null) (sender as ScrollBar).Tag = "3";
                Byte bTag = Convert.ToByte((sender as ScrollBar).Tag.ToString());
                mySensor.ParamSensors[bTag] = Convert.ToByte(sbSensitivity2.Value);
            }
            catch
            { }
        }

        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            lbUV1.Visible = (tabControl.SelectedTab.Name == "tabPage1");
            lbUV2.Visible = (tabControl.SelectedTab.Name == "tabPage1");
            if (CsConst.MyEditMode == 1)
            {
                MyActivePage = tabControl.SelectedIndex + 1;
                if (mySensor.MyRead2UpFlags[tabControl.SelectedIndex] == false)
                {
                    tbDown_Click(tbDown, null);
                }
                else
                {
                    switch (tabControl.SelectedIndex)
                    {
                        case 0: showBasicInfo(); break;
                        case 1: showLogicInfo(); break;
                        case 2: showSecInfo(); break;
                    }
                }
            }
            Cursor.Current = Cursors.Default;
        }

        private void checklistLED_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (isDealing) return;
            mySensor.ONLEDs[e.Index] = Convert.ToByte(e.NewValue);
        }

        private void checklistSensor_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (isDealing) return;
            if (e.Index == 0)
                mySensor.EnableSensors[1] = Convert.ToByte(e.NewValue);
            else if(e.Index==1)
                mySensor.EnableSensors[3] = Convert.ToByte(e.NewValue);
            else if(e.Index==2)
                mySensor.EnableSensors[5] = Convert.ToByte(e.NewValue);
            else if(e.Index==3)
                mySensor.EnableSensors[6] = Convert.ToByte(e.NewValue);
            else if (e.Index == 4)
                mySensor.EnableSensors[7] = Convert.ToByte(e.NewValue);
        }

        void SetVisibleForLogic(bool blnIsEnalbe)
        {

        }


        private void NumBr1_ValueChanged(object sender, EventArgs e)
        {
            if (isDealing) return;
            if (dgvLogic.RowCount < 0) return;
            if (dgvLogic.CurrentRow.Index < 0) return;
            int num1 = Convert.ToInt32(NumBr1.Value);
            mySensor.logic[mySelectedLNo].Paramters[2] = Convert.ToByte(num1 / 256);
            mySensor.logic[mySelectedLNo].Paramters[3] = Convert.ToByte(num1 % 256);
            int num2 = Convert.ToInt32(NumBr2.Value);
            mySensor.logic[mySelectedLNo].Paramters[4] = Convert.ToByte(num2 / 256);
            mySensor.logic[mySelectedLNo].Paramters[5] = Convert.ToByte(num2 % 256);
        }



        private void cbUL_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isDealing) return;
            if (dgvLogic.RowCount < 0) return;
            if (dgvLogic.CurrentRow.Index < 0) return;
            if (bIsUlOrPir == 0)  // ul sensor
            {
                mySensor.logic[mySelectedLNo].Paramters[7] = Convert.ToByte(cbUL.SelectedIndex);
            }
            else if (bIsUlOrPir == 1)  // PIR sensor
            {
                mySensor.logic[mySelectedLNo].Paramters[6] = Convert.ToByte(cbUL.SelectedIndex);
            }
        }


        private void cbUV1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isDealing) return;
            if (dgvLogic.RowCount < 0) return;
            if (dgvLogic.CurrentRow.Index < 0) return;
            if (cbUV1.SelectedIndex >= 0)
                mySensor.logic[mySelectedLNo].UV1.condition = Convert.ToByte(cbUV1.SelectedIndex);
            if (cbUV2.SelectedIndex >= 0)
            mySensor.logic[mySelectedLNo].UV2.condition = Convert.ToByte(cbUV2.SelectedIndex);
        }

        private void cbLogicNum_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isDealing) return;
            if (dgvLogic.RowCount < 0) return;
            if (dgvLogic.CurrentRow.Index < 0) return;
            if (cbLogicNum.SelectedIndex >= 0)
                mySensor.logic[mySelectedLNo].Paramters[9] = Convert.ToByte(cbLogicNum.SelectedIndex + 1);
            if (cbLogicStatu.SelectedIndex >= 0)
                mySensor.logic[mySelectedLNo].Paramters[10] = Convert.ToByte(cbLogicStatu.SelectedIndex);
        }

        private void truetime_TextChanged(object sender, EventArgs e)
        {
            if (isDealing) return;
            if (dgvLogic.RowCount < 0) return;
            if (dgvLogic.CurrentRow.Index < 0) return;
            mySensor.logic[mySelectedLNo].DelayTimeT = Convert.ToInt32(truetime.Text);
            mySensor.logic[mySelectedLNo].DelayTimeF = Convert.ToInt32(falsetime.Text);
        }

        private void txtUVID1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar) || e.KeyChar == 8)
                e.Handled = false;
            else
                e.Handled = true;
        }

        private void txtUVID1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (isDealing) return;
                if (dgvLogic.RowCount < 0) return;
                if (dgvLogic.CurrentRow.Index < 0) return;
                if (mySensor.logic[mySelectedLNo].UV1 == null) mySensor.logic[mySelectedLNo].UV1 = new UniversalSwitchSet();
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
                    mySensor.logic[mySelectedLNo].UV1.id = byte.Parse(txtUVID1.Text);
                    if (CsConst.MyEditMode == 1)
                    {
                        Cursor.Current = Cursors.WaitCursor;
                        isDealing = true;
                        byte[] arayTmp = new byte[1];
                        arayTmp[0] = Convert.ToByte(txtUVID1.Text);
                        if ((CsConst.mySends.AddBufToSndList(arayTmp, 0x160A, SubNetID, DevID, false, false, true, CsConst.minAllWirelessDeviceType.Contains(MyintDeviceType)) == true))
                        {
                            byte[] arayRemark = new byte[20];
                            Array.Copy(CsConst.myRevBuf, 27, arayRemark, 0, arayRemark.Length);
                            mySensor.logic[mySelectedLNo].UV1.remark = HDLPF.Byte2String(arayRemark);
                            txtUVRemark1.Text = mySensor.logic[mySelectedLNo].UV1.remark;
                            mySensor.logic[mySelectedLNo].UV1.isAutoOff = (CsConst.myRevBuf[47] == 1);
                            checkAuto1.Checked = mySensor.logic[mySelectedLNo].UV1.isAutoOff;
                            if (CsConst.myRevBuf[48] * 256 + CsConst.myRevBuf[49] > 3600)
                                mySensor.logic[mySelectedLNo].UV1.autoOffDelay = 1;
                            else
                                mySensor.logic[mySelectedLNo].UV1.autoOffDelay = CsConst.myRevBuf[48] * 256 + CsConst.myRevBuf[49];
                            txtUVAuto1.Text = mySensor.logic[mySelectedLNo].UV1.autoOffDelay.ToString();
                            
                        }
                        isDealing = false;
                        if ((CsConst.mySends.AddBufToSndList(arayTmp, 0xE018, SubNetID, DevID, false, false, true, CsConst.minAllWirelessDeviceType.Contains(MyintDeviceType)) == true))
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
                    }
                }
            }
            catch
            {
            }
            Cursor.Current = Cursors.Default;
        }

        private void txtUVID2_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (isDealing) return;
                if (dgvLogic.RowCount <= 0) return;
                if (dgvLogic.CurrentRow.Index < 0) return;
                if (mySensor.logic[mySelectedLNo].UV2 == null) mySensor.logic[mySelectedLNo].UV2 = new UniversalSwitchSet();
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
                    mySensor.logic[mySelectedLNo].UV2.id = byte.Parse(txtUVID2.Text);
                    if (CsConst.MyEditMode == 1)
                    {
                        Cursor.Current = Cursors.WaitCursor;
                        isDealing = true;
                        byte[] arayTmp = new byte[1];
                        arayTmp[0] = byte.Parse(txtUVID2.Text);
                        if ((CsConst.mySends.AddBufToSndList(arayTmp, 0x160A, SubNetID, DevID, false, false, true, false) == true))
                        {
                            byte[] arayRemark = new byte[20];
                            Array.Copy(CsConst.myRevBuf, 27, arayRemark, 0, arayRemark.Length);
                            mySensor.logic[mySelectedLNo].UV2.remark = HDLPF.Byte2String(arayRemark);
                            txtUVRemark2.Text = mySensor.logic[mySelectedLNo].UV2.remark;
                            mySensor.logic[mySelectedLNo].UV2.isAutoOff = (CsConst.myRevBuf[47] == 1);
                            checkAuto2.Checked = mySensor.logic[mySelectedLNo].UV2.isAutoOff;
                            if (CsConst.myRevBuf[48] * 256 + CsConst.myRevBuf[49] > 3600)
                                mySensor.logic[mySelectedLNo].UV2.autoOffDelay = 1;
                            else
                                mySensor.logic[mySelectedLNo].UV2.autoOffDelay = CsConst.myRevBuf[48] * 256 + CsConst.myRevBuf[49];
                            txtUVAuto2.Text = mySensor.logic[mySelectedLNo].UV2.autoOffDelay.ToString();
                            
                        }
                        isDealing = false;
                        if ((CsConst.mySends.AddBufToSndList(arayTmp, 0xE018, SubNetID, DevID, false, false, true, CsConst.minAllWirelessDeviceType.Contains(MyintDeviceType)) == true))
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

        private void txtUVRemark1_TextChanged(object sender, EventArgs e)
        {
            if (isDealing == true) return;
            if (dgvLogic.RowCount < 0) return;
            if (dgvLogic.CurrentRow.Index < 0) return;
            
            mySensor.logic[mySelectedLNo].UV1.remark = txtUVRemark1.Text;
            mySensor.logic[mySelectedLNo].UV1.isAutoOff = checkAuto1.Checked;
            if (txtUVAuto1.TextLength > 0)
                mySensor.logic[mySelectedLNo].UV1.autoOffDelay = Convert.ToInt32(txtUVAuto1.Text);
        }

        private void txtUVRemark2_TextChanged(object sender, EventArgs e)
        {
            if (isDealing == true) return;
            if (dgvLogic.RowCount < 0) return;
            if (dgvLogic.CurrentRow.Index < 0) return;
            mySensor.logic[mySelectedLNo].UV2.remark = txtUVRemark2.Text;
            mySensor.logic[mySelectedLNo].UV2.isAutoOff = checkAuto2.Checked;
            if (txtUVAuto2.TextLength > 0)
                mySensor.logic[mySelectedLNo].UV2.autoOffDelay = Convert.ToInt32(txtUVAuto2.Text);
        }

        private void dgvSec_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (mySensor == null) return;
            if (mySensor.fireset == null) return;
            if (e.RowIndex == -1) return;
            if (e.ColumnIndex == -1) return;

            UVCMD.SecurityInfo tempfire = mySensor.fireset[e.RowIndex];

            switch (e.ColumnIndex)
            {
                case 2:
                    tempfire.bytTerms = Convert.ToByte(dgvSec[2, e.RowIndex].Value.ToString().ToLower() == "true");
                    break;

                case 3:
                    if (dgvSec[3, e.RowIndex].Value == null)
                    {
                        dgvSec[3, e.RowIndex].Value = "";
                    }
                    string strTmp = dgvSec[3, e.RowIndex].Value.ToString();
                    tempfire.strRemark = strTmp;

                    break;

                case 4:
                    strTmp = dgvSec[4, e.RowIndex].Value.ToString();
                    dgvSec[4, e.RowIndex].Value = HDLPF.IsNumStringMode(strTmp, 0, 254);
                    tempfire.bytSubID = byte.Parse(dgvSec[4, e.RowIndex].Value.ToString());
                    break;

                case 5:
                    strTmp = dgvSec[5, e.RowIndex].Value.ToString();
                    dgvSec[5, e.RowIndex].Value = HDLPF.IsNumStringMode(strTmp, 0, 254);
                    tempfire.bytDevID = byte.Parse(dgvSec[5, e.RowIndex].Value.ToString());
                    break;

                case 6:
                    strTmp = dgvSec[6, e.RowIndex].Value.ToString();
                    dgvSec[6, e.RowIndex].Value = HDLPF.IsNumStringMode(strTmp, 0, 254);
                    tempfire.bytArea = byte.Parse(dgvSec[6, e.RowIndex].Value.ToString());
                    break;
            }
        }

        private void dgvSec_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            dgvSec.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }



        private void tmRead_Click(object sender, EventArgs e)
        {
            tbDown_Click(tbDown, null);
        }

        private void tmUpload_Click(object sender, EventArgs e)
        {
            tbDown_Click(tbUpload, null);
        }


        private void btnSave1_Click(object sender, EventArgs e)
        {
            if (tabControl.SelectedTab.Name == "tabPage1")
            {
                chbTemp_CheckedChanged(chbTemp, null);
                cbTemp1_SelectedIndexChanged(null, null);
                chbTemp_CheckedChanged(checkboxBright, null);
                NumBr1_ValueChanged(null, null);
                chbTemp_CheckedChanged(chbUL, null);
                cbUL_SelectedIndexChanged(null, null);
                chbTemp_CheckedChanged(checkboxUV1, null);
                chbTemp_CheckedChanged(checkboxUV2, null);
                cbUV1_SelectedIndexChanged(null, null);
                chbTemp_CheckedChanged(CheckBoxLogic, null);
                cbLogicNum_SelectedIndexChanged(null, null);
                truetime_TextChanged(null, null);
                rbOr_CheckedChanged(null, null);
            }
            tbDown_Click(tbUpload, null);
        }

        private void btnSaveAndClose1_Click(object sender, EventArgs e)
        {
            tbDown_Click(tbUpload, null);
            this.Close();
        }

        private void dgvLogic_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            dgvLogic.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void dgvLogic_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                mySelectedLNo = e.RowIndex;
                byte[] arayTmp = new byte[1];
                arayTmp[0] = Convert.ToByte(dgvLogic[0, e.RowIndex].Value.ToString());
                if (CsConst.mySends.AddBufToSndList(arayTmp, 0x160E, SubNetID, DevID, false, true, true, false) == true)
                {
                    SensorLogic oTmp = mySensor.logic[e.RowIndex];
                    oTmp.bytRelation = CsConst.myRevBuf[27];
                    oTmp.EnableSensors = new byte[15];
                    int intTmp = CsConst.myRevBuf[28] * 256 + CsConst.myRevBuf[29];
                    if (intTmp == 65535) intTmp = 0;
                    for (byte i = 0; i < 12; i++)
                    {
                        if ((intTmp & (1 << i)) == (1 << i))
                        {
                            switch (i)
                            {
                                case 0: oTmp.EnableSensors[0] = 1; break;
                                case 1: oTmp.EnableSensors[1] = 1; break;
                                case 5: oTmp.EnableSensors[2] = 1; break;
                                case 6: oTmp.EnableSensors[4] = 1; break;
                                case 8: oTmp.EnableSensors[5] = 1; break;
                                case 9: oTmp.EnableSensors[6] = 1; break;
                                case 10: oTmp.EnableSensors[7] = 1; break;
                                case 11: oTmp.EnableSensors[3] = 1; break;
                            }
                        }
                    }

                    oTmp.DelayTimeT = CsConst.myRevBuf[30] * 256 + CsConst.myRevBuf[31];
                    oTmp.DelayTimeF = CsConst.myRevBuf[32] * 256 + CsConst.myRevBuf[33];
                    Array.Copy(CsConst.myRevBuf, 34, oTmp.Paramters, 0, 6);
                    oTmp.Paramters[6] = CsConst.myRevBuf[46];
                    oTmp.Paramters[7] = CsConst.myRevBuf[55];
                    oTmp.Paramters[8] = CsConst.myRevBuf[47];
                    oTmp.UV1 = new UniversalSwitchSet();
                    if (201 <= CsConst.myRevBuf[49] && CsConst.myRevBuf[49] <= 248)
                        oTmp.UV1.id = CsConst.myRevBuf[49];
                    else
                        oTmp.UV1.id = 201;
                    if (CsConst.myRevBuf[50] <= 1)
                        oTmp.UV1.condition = CsConst.myRevBuf[50];
                    oTmp.UV2 = new UniversalSwitchSet();
                    if (201 <= CsConst.myRevBuf[51] && CsConst.myRevBuf[51] <= 248)
                        oTmp.UV2.id = CsConst.myRevBuf[51];
                    else
                    {
                        oTmp.UV2.id = 201;
                        if (oTmp.UV1.id == 201) oTmp.UV2.id = 202;
                    }
                    if (CsConst.myRevBuf[52] <= 1)
                        oTmp.UV2.condition = CsConst.myRevBuf[52];
                    oTmp.Paramters[9] = CsConst.myRevBuf[53];
                    oTmp.Paramters[10] = CsConst.myRevBuf[54];
                    
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

        private void dgvLogic_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (mySensor == null) return;
                if (mySensor.logic == null) return;
                if ((e.RowIndex == -1) || (e.ColumnIndex == -1)) return;
                if (dgvLogic.SelectedRows.Count == 0) return;
                if (isDealing) return;
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
            frmCmdSetup CmdSetup = new frmCmdSetup(mySensor, myDevName, MyintDeviceType, PageID);
            CmdSetup.ShowDialog();
        }

        private void dgvSec_SizeChanged(object sender, EventArgs e)
        {
            HDLSysPF.setDataGridViewColumnsWidth(dgvSec);
        }

        private void chbTemp_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                int Tag = Convert.ToInt32((sender as CheckBox).Tag);
                if (Tag == 0)
                {
                    if (isDealing) return;
                    if (dgvLogic.RowCount < 0) return;
                    if (dgvLogic.CurrentRow.Index < 0) return;
                    mySensor.logic[mySelectedLNo].EnableSensors[0] = 0;
                }
                else if (Tag == 1)
                {
                    NumBr1.Enabled = (sender as CheckBox).Checked;
                    NumBr2.Enabled = (sender as CheckBox).Checked;
                    if (isDealing) return;
                    if (dgvLogic.RowCount < 0) return;
                    if (dgvLogic.CurrentRow.Index < 0) return;
                    mySensor.logic[mySelectedLNo].EnableSensors[1] = Convert.ToByte((sender as CheckBox).Checked);
                }
                else if (Tag == 2)
                {
                    
                }
                else if (Tag == 3)
                {
                    cbUL.Enabled = (sender as CheckBox).Checked;
                    if (isDealing) return;
                    if (dgvLogic.RowCount < 0) return;
                    if (dgvLogic.CurrentRow.Index < 0) return;
                    if (bIsUlOrPir == 0) // UL sensor
                    {
                        mySensor.logic[mySelectedLNo].EnableSensors[3] = Convert.ToByte((sender as CheckBox).Checked);
                    }
                    else if (bIsUlOrPir == 1)
                    {
                        mySensor.logic[mySelectedLNo].EnableSensors[2] = Convert.ToByte((sender as CheckBox).Checked);
                    }
                }
                else if (Tag == 4)
                {
                    
                }
                else if (Tag == 5)
                {
                    cbUV1.Enabled = (sender as CheckBox).Checked;
                    txtUVID1.Enabled = (sender as CheckBox).Checked;
                    txtUVRemark1.Enabled = (sender as CheckBox).Checked;
                    checkAuto1.Enabled = (sender as CheckBox).Checked;
                    txtUVAuto1.Enabled = (sender as CheckBox).Checked;
                    if (isDealing) return;
                    if (dgvLogic.RowCount < 0) return;
                    if (dgvLogic.CurrentRow.Index < 0) return;
                    mySensor.logic[mySelectedLNo].EnableSensors[5] = Convert.ToByte((sender as CheckBox).Checked);
                }
                else if (Tag == 6)
                {
                    cbUV2.Enabled = (sender as CheckBox).Checked;
                    txtUVID2.Enabled = (sender as CheckBox).Checked;
                    txtUVRemark2.Enabled = (sender as CheckBox).Checked;
                    checkAuto2.Enabled = (sender as CheckBox).Checked;
                    txtUVAuto2.Enabled = (sender as CheckBox).Checked;
                    if (isDealing) return;
                    if (dgvLogic.RowCount < 0) return;
                    if (dgvLogic.CurrentRow.Index < 0) return;
                    mySensor.logic[mySelectedLNo].EnableSensors[6] = Convert.ToByte((sender as CheckBox).Checked);
                }
                else if (Tag == 7)
                {
                    cbLogicNum.Enabled = (sender as CheckBox).Checked;
                    cbLogicStatu.Enabled = (sender as CheckBox).Checked;
                    if (isDealing) return;
                    if (dgvLogic.RowCount < 0) return;
                    if (dgvLogic.CurrentRow.Index < 0) return;
                    mySensor.logic[mySelectedLNo].EnableSensors[7] = Convert.ToByte((sender as CheckBox).Checked);
                }
                
            }
            catch
            {
            }
        }

        private void rbOr_CheckedChanged(object sender, EventArgs e)
        {
            if (isDealing) return;
            if (dgvLogic.RowCount < 0) return;
            if (dgvLogic.CurrentRow.Index < 0) return;
            if (rbOr.Checked)
                mySensor.logic[mySelectedLNo].bytRelation = 0;
            else if (rbAnd.Checked)
                mySensor.logic[mySelectedLNo].bytRelation = 1;
        }


        private void btnOff_Click(object sender, EventArgs e)
        {
            byte[] arayTmp = new byte[4];
            arayTmp[0] = 1;
            CsConst.mySends.AddBufToSndList(arayTmp, 0x0031, SubNetID, DevID, false, false, false, CsConst.minAllWirelessDeviceType.Contains(MyintDeviceType));

        }

        private void chbEnable_CheckedChanged(object sender, EventArgs e)
        {
            txtLux.Enabled = chbEnable.Checked;
            lbCycle.Enabled = chbEnable.Checked;
            cbCycle.Enabled = chbEnable.Checked;
            panel14.Enabled = chbEnable.Checked;
            if (isDealing) return;
            if (chbEnable.Checked)
                mySensor.EnableBroads[0] = 1;
            else
                mySensor.EnableBroads[0] = 0;
        }

        private void txtLux_TextChanged(object sender, EventArgs e)
        {
            if (isDealing) return;
            if (txtLux.Text.Length > 0)
            {
                int num = Convert.ToInt32(txtLux.Text);
                mySensor.EnableBroads[1] = Convert.ToByte(num / 256);
                mySensor.EnableBroads[2] = Convert.ToByte(num % 256);
            }
        }

        private void cbCycle_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isDealing) return;
            string str = cbCycle.Text;
            int num = Convert.ToInt32(Convert.ToSingle(str) * 10);
            mySensor.EnableBroads[7] = Convert.ToByte(num);
        }

        private void cbParam1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isDealing) return;
            string str = cbParam1.Text;
            int num = Convert.ToInt32(Convert.ToSingle(str) * 100);
            mySensor.EnableBroads[3] = Convert.ToByte(num / 256);
            mySensor.EnableBroads[4] = Convert.ToByte(num % 256);
        }

        private void cbParam2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isDealing) return;
            string str = cbParam2.Text;
            int num = Convert.ToInt32(Convert.ToSingle(str) * 100);
            mySensor.EnableBroads[5] = Convert.ToByte(num / 256);
            mySensor.EnableBroads[6] = Convert.ToByte(num % 256);
        }

        private void sbLimit_ValueChanged(object sender, EventArgs e)
        {
            lbLimitValue.Text = sbLimit.Value.ToString() + "%";
            if (isDealing) return;
            mySensor.EnableBroads[8] = Convert.ToByte(sbLimit.Value);
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
                if (isDealing) return;
                if (dgvLogic.RowCount <= 0) return;
                if (dgvLogic.CurrentRow.Index < 0) return;
                int Tmp1 = Convert.ToInt32(cbTemp1.Text);
                int Tmp2 = Convert.ToInt32(cbTemp2.Text);
                mySensor.logic[mySelectedLNo].Paramters[0] = Convert.ToByte(Tmp1 + 20);
                mySensor.logic[mySelectedLNo].Paramters[1] = Convert.ToByte(Tmp2 + 20);
            }
            catch
            {
            }
        }

        private void sb1_ValueChanged(object sender, EventArgs e)
        {
            lb1.Text = sb1.Value.ToString();
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

        private void btnAdjust_Click(object sender, EventArgs e)
        {
            FrmCalibrationLux frmTmp = new FrmCalibrationLux(myDevName, MyintDeviceType);
            frmTmp.ShowDialog();
        }

        private void btnBroadcast1_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            byte[] ArayTmp = new byte[0];
            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x16B3, SubNetID, DevID, false, false, true, false) == true)
            {
                chbBroadcast.Checked = (CsConst.myRevBuf[26] == 1);
                
            }
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

        private void sbSensitivity2_Scroll(object sender, ScrollEventArgs e)
        {

        }
    }
}
