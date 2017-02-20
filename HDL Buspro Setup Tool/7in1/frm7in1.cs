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

namespace HDL_Buspro_Setup_Tool
{
    public partial class frm7in1 : Form
    {
        [DllImport("user32.dll")]
        public static extern Boolean FlashWindow(IntPtr hwnd, Boolean bInvert);
        private Sensor_7in1 mySensor = null;
        private string myDevName = null;
        private int mintIDIndex = -1;
        private int MyintDeviceType = -1;
        private int MyActivePage = 0; //按页面上传下载
        private bool isRead = false;
        private byte SubNetID;
        private byte DevID;
        private int SelectedRow = 0;
        public frm7in1()
        {
            InitializeComponent();
        }

        public frm7in1(Sensor_7in1 mysensor, string strName, int intDIndex, int intDeviceType)
        {
            InitializeComponent();
            this.mySensor = mysensor;
            this.myDevName = strName;
            this.mintIDIndex = intDIndex;
            string strDevName = strName.Split('\\')[0].ToString();
            this.MyintDeviceType = intDeviceType;

            HDLSysPF.DisplayDeviceNameModeDescription(strName, MyintDeviceType, cboDevice, tbModel, tbDescription);

            SubNetID = Convert.ToByte(strDevName.Split('-')[0]);
            DevID = Convert.ToByte(strDevName.Split('-')[1]);
        }

        private void frm12in1_Load(object sender, EventArgs e)
        {
            toolStrip1.Visible = (CsConst.MyEditMode == 0);
            HDLSysPF.setDataGridViewColumnsWidth(dgvSec);
            groupBox1.Visible = (MyintDeviceType == 328);
            if (MyintDeviceType == 328) sbSensitivity1.Maximum = 10;
            else sbSensitivity1.Maximum = 100;
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

            checklistSensor.Items.Clear();
            checklistSensor.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00610", ""));
            checklistSensor.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00611", ""));
            checklistSensor.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00612", ""));
            checklistSensor.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00614", ""));
            checklistSensor.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00615", ""));
            checklistSensor.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00616", ""));
            checklistSensor.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00617", ""));
            checklistSensor.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00618", ""));

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
            clL3.Items.Clear();
            clL3.Items.Add(CsConst.WholeTextsList[1775].sDisplayName);
            clL3.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00042", ""));
        }

        private void tslRead_Click(object sender, EventArgs e)
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

                    string strName = myDevName.Split('\\')[0].ToString();
                    byte bytSubID = byte.Parse(strName.Split('-')[0]);
                    byte bytDevID = byte.Parse(strName.Split('-')[1]);
                    int num1 = 0;
                    int num2 = 0;
                    if (tab12in1.SelectedTab.Name == "tabPage4" && bytTag==1)
                    {
                        if (dgvLogic.RowCount > 0 && dgvLogic.CurrentRow.Index >= 0)
                            num1 = Convert.ToInt32(SelectedRow + 1);
                    }
                    byte[] ArayRelay = new byte[] { bytSubID, bytDevID, (byte)(MyintDeviceType / 256), (byte)(MyintDeviceType % 256), 
                    (byte)MyActivePage,(byte)(mintIDIndex / 256), (byte)(mintIDIndex % 256),(byte)num1,(byte)num2 };

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
            if (tab12in1.SelectedTab.Name == "tabPage1") showSensorInfo();
            else if (tab12in1.SelectedTab.Name == "tabPage4") showLogicInfo();
            else if (tab12in1.SelectedTab.Name == "tabPage3") showSecurityInfo();
            else if (tab12in1.SelectedTab.Name == "tabPage6") showSimulateInfo();
            this.TopMost = true;
            this.WindowState = FormWindowState.Normal;
            this.Activate();
            this.TopMost = false;
        }

        private void frm12in1_Shown(object sender, EventArgs e)
        {
            if (CsConst.MyEditMode == 1) //在线模式
            {
                MyActivePage = 1;
                tslRead_Click(tslRead, null);
                btnRefStatus_Click(null, null);
                btnBroadcast1_Click(null, null);
            }
        }

        private void tab12in1_SelectedIndexChanged(object sender, EventArgs e)
        {
            lbUV1.Visible = (tab12in1.SelectedTab.Name == "tabPage4");
            lbUV2.Visible = (tab12in1.SelectedTab.Name == "tabPage4");
            if (tab12in1.SelectedTab.Name == "tabPage1") MyActivePage = 1;
            else if (tab12in1.SelectedTab.Name == "tabPage4") MyActivePage = 2;
            else if (tab12in1.SelectedTab.Name == "tabPage3") MyActivePage = 3;
            else if (tab12in1.SelectedTab.Name == "tabPage6") MyActivePage = 4;
            if (CsConst.MyEditMode == 1)
            {
                if (tab12in1.SelectedTab.Name != "tabKeys")
                {
                    if (mySensor.MyRead2UpFlags[MyActivePage - 1] == false)
                    {
                        tslRead_Click(tslRead, null);
                    }
                    else
                    {
                        //基本信息

                         if (tab12in1.SelectedTab.Name == "tabPage1") showSensorInfo();
                        else if (tab12in1.SelectedTab.Name == "tabPage4") showLogicInfo();
                        else if (tab12in1.SelectedTab.Name == "tabPage3") showSecurityInfo();
                        else if (tab12in1.SelectedTab.Name == "tabPage6") showSimulateInfo();
                    }
                }
            }
        }

        private void showSensorInfo()
        {
            try
            {
                isRead = true;
                for (int i = 0; i < 2; i++)
                {
                    if (mySensor.ONLEDs[i] == 1) checklistLED.SetItemChecked(i, true);
                    else checklistLED.SetItemChecked(i, false);
                }
                if (mySensor.EnableSensors[0] == 1) checklistSensor.SetItemChecked(0, true);
                else checklistSensor.SetItemChecked(0, false);
                if (mySensor.EnableSensors[1] == 1) checklistSensor.SetItemChecked(1, true);
                else checklistSensor.SetItemChecked(1, false);
                for (int i = 2; i < 8; i++)
                {
                    if (mySensor.EnableSensors[i + 3] == 1) checklistSensor.SetItemChecked(i, true);
                    else checklistSensor.SetItemChecked(i, false);
                }
                if (mySensor.ParamSensors[0] <= 128)
                    sbSensitivity4.Value = mySensor.ParamSensors[0];
                else
                    sbSensitivity4.Value = (mySensor.ParamSensors[0] - 128);
                if (mySensor.ParamSensors[12] >= sbSensitivity1.Minimum && mySensor.ParamSensors[12] <= sbSensitivity1.Maximum)
                    sbSensitivity1.Value = mySensor.ParamSensors[12];

                chbEnable.Checked = (mySensor.EnableBroads[0] == 1);
                txtLux.Text = (mySensor.EnableBroads[1] * 256 + mySensor.EnableBroads[2]).ToString();
                string str1=((mySensor.EnableBroads[3] * 256 + mySensor.EnableBroads[4]) / 100).ToString() + "."+
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
                    object[] obj = new object[] { mySensor.logic[i].ID.ToString(),mySensor.logic[i].Remarklogic,strEnable};
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
            if (dgvLogic.RowCount == 0) return;
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


        private void showSimulateInfo()
        {
            try
            {
                isRead = true;
                if (mySensor.SimulateEnable[0] == 1) lbState.Text = CsConst.mstrINIDefault.IniReadValue("Public", "99619", "");
                else lbState.Text = CsConst.mstrINIDefault.IniReadValue("Public", "99618", "");

                if (mySensor.SimulateEnable[1] == 1) chb0.Checked = true;
                else chb0.Checked = false;

                if (mySensor.SimulateEnable[2] == 1) chb1.Checked = true;
                else chb2.Checked = false;
                if (mySensor.SimulateEnable[3] == 1) chb2.Checked = true;
                else chb2.Checked = false;
                if (mySensor.ParamSimulate[0] <= sb0.Maximum)
                    sb0.Value = mySensor.ParamSimulate[0];
                int num = mySensor.ParamSimulate[1] * 256 + mySensor.ParamSimulate[2];
                if (num <= sb1.Maximum)
                    sb1.Value = num;
                if (mySensor.ParamSimulate[3] <= sb2.Maximum)
                    sb2.Value = mySensor.ParamSimulate[3];
            }
            catch
            {
            }
            isRead = false;
            sb0_ValueChanged(null, null);
            sb1_ValueChanged(null, null);
            sb2_ValueChanged(null, null);

        }


        private void txtFrm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar) || e.KeyChar == 8)
                e.Handled = false;
            else
                e.Handled = true;
        }

        private void tmRead_Click(object sender, EventArgs e)
        {
            if (isRead) return;
            tslRead_Click(tslRead, null);
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
            lbSensitivity1.Text = sbSensitivity1.Value.ToString() + "%";
            if (sbSensitivity1.Maximum == 10)
            {
                lbSensitivity1.Text = (sbSensitivity1.Value * 10).ToString() + "%";
            }
            if (isRead) return;
            mySensor.ParamSensors[12] = Convert.ToByte(sbSensitivity1.Value);
        }


        private void sbLimit_ValueChanged(object sender, EventArgs e)
        {
            lbLimitValue.Text = sbLimit.Value.ToString() + "%";
            if (isRead) return;
            mySensor.EnableBroads[8] = Convert.ToByte(sbLimit.Value);
        }

        private void sb0_ValueChanged(object sender, EventArgs e)
        {
            lb0.Text = (sb0.Value - 20).ToString() + "C";
        }


        private void sb2_ValueChanged(object sender, EventArgs e)
        {
            if (sb2.Value == 1) lb2.Text = CsConst.mstrINIDefault.IniReadValue("Public", "99849", "");
            else lb2.Text = CsConst.mstrINIDefault.IniReadValue("Public", "99848", "");
            if (isRead) return;
            mySensor.ParamSimulate[3] = Convert.ToByte(sb2.Value);
        }


        private void timerSensor_Tick(object sender, EventArgs e)
        {
            if (isRead) return;
            byte[] ArayTmp = new byte[0];
            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1604, SubNetID, DevID, false, false, true,false) == true)
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
                    mySensor.logic[SelectedRow].UV1.UvNum = byte.Parse(txtUVID1.Text);
                    if (CsConst.MyEditMode == 1)
                    {
                        Cursor.Current = Cursors.WaitCursor;
                        isRead = true;
                        byte[] arayTmp = new byte[1];
                        arayTmp[0] = Convert.ToByte(txtUVID1.Text);
                        if ((CsConst.mySends.AddBufToSndList(arayTmp, 0x160A, SubNetID, DevID, false, false, true, false) == true))
                        {
                            byte[] arayRemark = new byte[20];
                            Array.Copy(CsConst.myRevBuf, 27, arayRemark, 0, arayRemark.Length);
                            mySensor.logic[SelectedRow].UV1.UvRemark = HDLPF.Byte2String(arayRemark);
                            txtUVRemark1.Text = mySensor.logic[SelectedRow].UV1.UvRemark;
                            mySensor.logic[SelectedRow].UV1.AutoOff = (CsConst.myRevBuf[47] == 1);
                            checkAuto1.Checked = mySensor.logic[SelectedRow].UV1.AutoOff;
                            if (CsConst.myRevBuf[48] * 256 + CsConst.myRevBuf[49] > 3600)
                                mySensor.logic[SelectedRow].UV1.OffTime = 1;
                            else
                                mySensor.logic[SelectedRow].UV1.OffTime = CsConst.myRevBuf[48] * 256 + CsConst.myRevBuf[49];
                            txtUVAuto1.Text = mySensor.logic[SelectedRow].UV1.OffTime.ToString();
                            
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
                chbIR.Enabled = (mySensor.EnableSensors[5] == 1);
                chbDry1.Enabled = (mySensor.EnableSensors[6] == 1);
                chbDry2.Enabled = (mySensor.EnableSensors[7] == 1);
                checkboxUV1.Enabled = (mySensor.EnableSensors[8] == 1);
                checkboxUV2.Enabled = (mySensor.EnableSensors[9] == 1);
                CheckBoxLogic.Enabled = (mySensor.EnableSensors[10] == 1);
                Sensor_7in1.Logic oLogic = mySensor.logic[Index];

                chbTemp.Checked = Convert.ToBoolean(oLogic.EnableSensors[0]);
                checkboxBright.Checked = Convert.ToBoolean(oLogic.EnableSensors[1]);
                chbIR.Checked = Convert.ToBoolean(oLogic.EnableSensors[5]);
                chbDry1.Checked = Convert.ToBoolean(oLogic.EnableSensors[6]);
                chbDry2.Checked = Convert.ToBoolean(oLogic.EnableSensors[7]);
                checkboxUV1.Checked = Convert.ToBoolean(oLogic.EnableSensors[8]);
                checkboxUV2.Checked = Convert.ToBoolean(oLogic.EnableSensors[9]);
                CheckBoxLogic.Checked = Convert.ToBoolean(oLogic.EnableSensors[10]);

                if (oLogic.bytRelation == 0) rbOr.Checked = true;
                else if (oLogic.bytRelation == 1) rbAnd.Checked = true;
                cbTemp1.SelectedIndex = cbTemp1.Items.IndexOf((oLogic.Paramters[0] - 20).ToString());
                cbTemp2.SelectedIndex = cbTemp1.Items.IndexOf((oLogic.Paramters[1] - 20).ToString());
                if (cbTemp1.SelectedIndex < 0) cbTemp1.SelectedIndex = 0;
                if (cbTemp2.SelectedIndex < 0) cbTemp2.SelectedIndex = cbTemp1.SelectedIndex;

                if ((0 <= Convert.ToDecimal(oLogic.Paramters[2] * 256 + oLogic.Paramters[3])) && (Convert.ToDecimal(oLogic.Paramters[2] * 256 + oLogic.Paramters[3]) <= 5000))
                    NumBr1.Value = Convert.ToDecimal(oLogic.Paramters[2] * 256 + oLogic.Paramters[3]);
                if ((0 <= Convert.ToDecimal(oLogic.Paramters[4] * 256 + oLogic.Paramters[5])) && (Convert.ToDecimal(oLogic.Paramters[4] * 256 + oLogic.Paramters[5]) <= 5000))
                    NumBr2.Value = Convert.ToDecimal(oLogic.Paramters[4] * 256 + oLogic.Paramters[5]);
                if (oLogic.Paramters[12] < 2)
                    cbIRSensor.Text = cbIRSensor.Items[oLogic.Paramters[12]].ToString();
                if (cbIRSensor.SelectedIndex < 0) cbIRSensor.SelectedIndex = 0;
                if (oLogic.Paramters[13] < 2)
                    cbDry1.Text = cbDry1.Items[oLogic.Paramters[13]].ToString();
                if (cbDry1.SelectedIndex < 0) cbDry1.SelectedIndex = 0;
                if (oLogic.Paramters[14] < 2)
                    cbDry2.Text = cbDry2.Items[oLogic.Paramters[14]].ToString();
                if (cbDry2.SelectedIndex < 0) cbDry2.SelectedIndex = 0;
                if (1 <= oLogic.Paramters[15] && oLogic.Paramters[15] <= 24)
                    cbLogicNum.SelectedIndex = oLogic.Paramters[15] - 1;
                if (cbLogicNum.SelectedIndex < 0) cbLogicNum.SelectedIndex = 0;
                if (oLogic.Paramters[16] < 2)
                    cbLogicStatu.SelectedIndex = oLogic.Paramters[16];
                if (cbLogicStatu.SelectedIndex < 0) cbLogicStatu.SelectedIndex = 0;
                if (oLogic.UV1 != null)
                {
                    txtUVID1.Text = oLogic.UV1.UvNum.ToString();
                    if (oLogic.UV1.UvRemark == null) oLogic.UV1.UvRemark = "";
                    txtUVRemark1.Text = oLogic.UV1.UvRemark.ToString();
                    if (oLogic.UV1.UVCondition < 2)
                        cbUV1.Text = cbUV1.Items[oLogic.UV1.UVCondition].ToString();
                    checkAuto1.Checked = oLogic.UV1.AutoOff;
                    txtUVAuto1.Text = oLogic.UV1.OffTime.ToString();
                    lbUV1.Visible = true;
                    if (oLogic.UV1.state == 0) lbUV1.Text = CsConst.mstrINIDefault.IniReadValue("Public", "99859", "") + txtUVID1.Text + " " +
                                                                CsConst.Status[0];
                    else lbUV1.Text = CsConst.mstrINIDefault.IniReadValue("Public", "99859", "") + txtUVID1.Text + " " +
                                                                CsConst.Status[0];
                }

                if (oLogic.UV2 != null)
                {
                    txtUVID2.Text = oLogic.UV2.UvNum.ToString();
                    if (oLogic.UV2.UvRemark == null) oLogic.UV2.UvRemark = "";
                    txtUVRemark2.Text = oLogic.UV2.UvRemark.ToString();
                    if (oLogic.UV2.UVCondition < 2)
                        cbUV2.Text = cbUV2.Items[oLogic.UV2.UVCondition].ToString();
                    checkAuto2.Checked = oLogic.UV2.AutoOff;
                    txtUVAuto2.Text = oLogic.UV2.OffTime.ToString();
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

        void SetVisibleForLogic(bool blnIsEnalbe)
        {
           
            truetime.Enabled = blnIsEnalbe;
            falsetime.Enabled = blnIsEnalbe;
            p1.Enabled = blnIsEnalbe;
            p2.Enabled = blnIsEnalbe;
            p3.Enabled = blnIsEnalbe;
            p5.Enabled = blnIsEnalbe;
            p6.Enabled = blnIsEnalbe;
            p8.Enabled = blnIsEnalbe;
            p7.Enabled = blnIsEnalbe;
            p9.Enabled = blnIsEnalbe;
            rbAnd.Enabled = blnIsEnalbe;
            rbOr.Enabled = blnIsEnalbe;
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
                    mySensor.logic[SelectedRow].EnableSensors[0] = Convert.ToByte((sender as CheckBox).Checked);
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
                    mySensor.logic[SelectedRow].EnableSensors[5] = Convert.ToByte((sender as CheckBox).Checked);
                }
                else if (Tag == 4)
                {
                    cbDry1.Enabled = ((sender as CheckBox).Checked && (sender as CheckBox).Enabled);
                    if (isRead) return;
                    if (dgvLogic.RowCount <= 0) return;
                    if (dgvLogic.CurrentRow.Index < 0) return;
                    mySensor.logic[SelectedRow].EnableSensors[6] = Convert.ToByte((sender as CheckBox).Checked);
                }
                else if (Tag == 5)
                {
                    cbDry2.Enabled = ((sender as CheckBox).Checked && (sender as CheckBox).Enabled);
                    if (isRead) return;
                    if (dgvLogic.RowCount <= 0) return;
                    if (dgvLogic.CurrentRow.Index < 0) return;
                    mySensor.logic[SelectedRow].EnableSensors[7] = Convert.ToByte((sender as CheckBox).Checked);
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
                    mySensor.logic[SelectedRow].EnableSensors[8] = Convert.ToByte((sender as CheckBox).Checked);
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
                    mySensor.logic[SelectedRow].EnableSensors[9] = Convert.ToByte((sender as CheckBox).Checked);
                }
                else if (Tag == 8)
                {
                    cbLogicNum.Enabled = ((sender as CheckBox).Checked && (sender as CheckBox).Enabled);
                    cbLogicStatu.Enabled = ((sender as CheckBox).Checked && (sender as CheckBox).Enabled);
                    if (isRead) return;
                    if (dgvLogic.RowCount <= 0) return;
                    if (dgvLogic.CurrentRow.Index < 0) return;
                    mySensor.logic[SelectedRow].EnableSensors[10] = Convert.ToByte((sender as CheckBox).Checked);
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
                mySensor.logic[SelectedRow].Paramters[2] = Convert.ToByte(num1 / 256);
                mySensor.logic[SelectedRow].Paramters[3] = Convert.ToByte(num1 % 256);
                num2 = Convert.ToInt32(NumBr2.Value);
                mySensor.logic[SelectedRow].Paramters[4] = Convert.ToByte(num2 / 256);
                mySensor.logic[SelectedRow].Paramters[5] = Convert.ToByte(num2 % 256);
            }
            catch
            {
            }
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
                int Tmp1 = Convert.ToInt32(cbTemp1.Text);
                int Tmp2 = Convert.ToInt32(cbTemp2.Text);
                mySensor.logic[SelectedRow].Paramters[0] = Convert.ToByte(Tmp1 + 20);
                mySensor.logic[SelectedRow].Paramters[1] = Convert.ToByte(Tmp2 + 20);
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
            mySensor.logic[SelectedRow].Paramters[12] = Convert.ToByte(cbIRSensor.SelectedIndex);
        }



        private void cbDry1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            if (dgvLogic.RowCount <= 0) return;
            if (dgvLogic.CurrentRow.Index < 0) return;
            mySensor.logic[SelectedRow].Paramters[13] = Convert.ToByte(cbDry1.SelectedIndex);
        }

        private void cbDry2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            if (dgvLogic.RowCount <= 0) return;
            if (dgvLogic.CurrentRow.Index < 0) return;
            mySensor.logic[SelectedRow].Paramters[14] = Convert.ToByte(cbDry2.SelectedIndex);
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

        private void txtUVID2_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (isRead) return;
                if (dgvLogic.RowCount <= 0) return;
                if (dgvLogic.CurrentRow.Index < 0) return;
                if (mySensor.logic[SelectedRow].UV2 == null) mySensor.logic[SelectedRow].UV2 = new Sensor_7in1.UVSet();
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
                    mySensor.logic[SelectedRow].UV2.UvNum = byte.Parse(txtUVID2.Text);
                    if (CsConst.MyEditMode == 1)
                    {
                        Cursor.Current = Cursors.WaitCursor;
                        isRead = true;
                        byte[] arayTmp = new byte[1];
                        arayTmp[0] = byte.Parse(txtUVID2.Text);
                        if ((CsConst.mySends.AddBufToSndList(arayTmp, 0x160A, SubNetID, DevID, false, false, true, false) == true))
                        {
                            byte[] arayRemark = new byte[20];
                            Array.Copy(CsConst.myRevBuf, 27, arayRemark, 0, arayRemark.Length);
                            mySensor.logic[SelectedRow].UV2.UvRemark = HDLPF.Byte2String(arayRemark);
                            txtUVRemark2.Text = mySensor.logic[SelectedRow].UV2.UvRemark;
                            mySensor.logic[SelectedRow].UV2.AutoOff = (CsConst.myRevBuf[47] == 1);
                            checkAuto2.Checked = mySensor.logic[SelectedRow].UV2.AutoOff;
                            if (CsConst.myRevBuf[48] * 256 + CsConst.myRevBuf[49] > 3600)
                                mySensor.logic[SelectedRow].UV2.OffTime = 1;
                            else
                                mySensor.logic[SelectedRow].UV2.OffTime = CsConst.myRevBuf[48] * 256 + CsConst.myRevBuf[49];
                            txtUVAuto2.Text = mySensor.logic[SelectedRow].UV2.OffTime.ToString();
                            
                        }
                        isRead = false;
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
            if (isRead == true) return;
            if (dgvLogic.RowCount <= 0) return;
            if (dgvLogic.CurrentRow.Index < 0) return;
            mySensor.logic[SelectedRow].UV1.UvRemark = txtUVRemark1.Text;
            mySensor.logic[SelectedRow].UV1.AutoOff = checkAuto1.Checked;
            if (txtUVAuto1.Text.Length > 0)
                mySensor.logic[SelectedRow].UV1.OffTime = Convert.ToInt32(txtUVAuto1.Text);
        }

        private void txtUVRemark2_TextChanged(object sender, EventArgs e)
        {
            if (isRead == true) return;
            if (dgvLogic.RowCount <= 0) return;
            if (dgvLogic.CurrentRow.Index < 0) return;
            mySensor.logic[SelectedRow].UV2.UvRemark = txtUVRemark2.Text;
            mySensor.logic[SelectedRow].UV2.AutoOff = checkAuto2.Checked;
            if (txtUVAuto2.Text.Length > 0)
                mySensor.logic[SelectedRow].UV2.OffTime = Convert.ToInt32(txtUVAuto2.Text);
        }

        private void cbUV1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            if (dgvLogic.RowCount <= 0) return;
            if (dgvLogic.CurrentRow.Index < 0) return;
            if (cbUV1.SelectedIndex >= 0)
                mySensor.logic[SelectedRow].UV1.UVCondition = Convert.ToByte(cbUV1.SelectedIndex);
            if (cbUV2.SelectedIndex >= 0)
                mySensor.logic[SelectedRow].UV2.UVCondition = Convert.ToByte(cbUV2.SelectedIndex);
        }

        private void truetime_TextChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            if (dgvLogic.RowCount <= 0) return;
            if (dgvLogic.CurrentRow.Index < 0) return;
            mySensor.logic[SelectedRow].DelayTimeT = Convert.ToInt32(truetime.Text);
            mySensor.logic[SelectedRow].DelayTimeF = Convert.ToInt32(falsetime.Text);
        }

        private void dgvSec_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            dgvSec.CommitEdit(DataGridViewDataErrorContexts.Commit);
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

        private void chbUpdata_CheckedChanged(object sender, EventArgs e)
        {
            timerSensor.Enabled = chbUpdata.Checked;
        }

        private void btnSure_Click(object sender, EventArgs e)
        {
            tslRead_Click(tslRead, null);
        }

        private void btnRefStatus_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            byte[] ArayTmp = new byte[0];
            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1604, SubNetID, DevID, false, false, true, false) == true)
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


        private void btnSave1_Click(object sender, EventArgs e)
        {
            if (tab12in1.SelectedTab.Name == "tabPage4")
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
            tslRead_Click(tbUpload, null);
        }

        private void btnSaveAndClose1_Click(object sender, EventArgs e)
        {
            tslRead_Click(tbUpload, null);
            this.Close();
        }

        private void checklistLED_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (isRead) return;
            mySensor.ONLEDs[e.Index] = Convert.ToByte(e.NewValue);
        }

        private void checklistSensor_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (isRead) return;
            if (e.Index == 0 || e.Index == 1)
                mySensor.EnableSensors[e.Index] = Convert.ToByte(e.NewValue);
            else
                mySensor.EnableSensors[e.Index + 3] = Convert.ToByte(e.NewValue);
        }

        private void chbEnable_CheckedChanged(object sender, EventArgs e)
        {
            lbLux.Enabled = chbEnable.Checked;
            txtLux.Enabled = chbEnable.Checked;
            lbCycle.Enabled = chbEnable.Checked;
            cbCycle.Enabled = chbEnable.Checked;
            panel19.Enabled = chbEnable.Checked;
            if (isRead) return;
            if(chbEnable.Checked)
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

        private void rbOr_CheckedChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            if (dgvLogic.RowCount < 0) return;
            if (dgvLogic.CurrentRow.Index < 0) return;
            if (rbOr.Checked)
                mySensor.logic[SelectedRow].bytRelation = 0;
            else if(rbAnd.Checked)
                mySensor.logic[SelectedRow].bytRelation = 1;
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
                if (chb0.Checked) ArayTmp[1] = 1;
                if (chb1.Checked) ArayTmp[2] = 1;

                ArayTmp[7] = Convert.ToByte(sb0.Value + 20);
                int num = Convert.ToInt32(sb1.Value);
                ArayTmp[8] = Convert.ToByte(num / 256);
                ArayTmp[9] = Convert.ToByte(num % 256);
                ArayTmp[10] = 20;
                ArayTmp[13] = Convert.ToByte(sb2.Value);
            }
            else if (tag == 0)
            {
                ArayTmp[10] = 20;
                ArayTmp[11] = 2;
                ArayTmp[12] = 2;
            }

            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1622, SubNetID, DevID, false, true, true, false) == true)
            {
                if (tag == 1)
                {
                    lbState.Text = CsConst.mstrINIDefault.IniReadValue("public", "99619", "");
                }
                else
                {
                    lbState.Text = CsConst.mstrINIDefault.IniReadValue("public", "99618", "");
                    chb0.Checked = false;
                    chb1.Checked = false;
                    chb2.Checked = false;
                    sb0.Value = 0;
                    sb1.Value = 0;
                    sb2.Value = 0;

                }
            }
            Cursor.Current = Cursors.Default;
            (sender as Button).Enabled = true;
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


        private void sb1_ValueChanged(object sender, EventArgs e)
        {
            lb1.Text = sb1.Value.ToString() + "Lux";
            if (isRead) return;
            mySensor.ParamSimulate[1] = Convert.ToByte(sb1.Value / 256);
            mySensor.ParamSimulate[2] = Convert.ToByte(sb1.Value % 256);
        }

        private void dgvLogic_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                SelectedRow = e.RowIndex;
                byte[] arayTmp = new byte[1];
                arayTmp[0] = Convert.ToByte(dgvLogic[0, e.RowIndex].Value.ToString());
                if (CsConst.mySends.AddBufToSndList(arayTmp, 0x160E, SubNetID, DevID, false, true, true, false) == true)
                {
                    Sensor_7in1.Logic oTmp = mySensor.logic[e.RowIndex];
                    oTmp.bytRelation = CsConst.myRevBuf[27];
                    oTmp.EnableSensors = new byte[15];
                    oTmp.Paramters = new byte[20];
                    int intTmp = CsConst.myRevBuf[28] * 256 + CsConst.myRevBuf[29];
                    if (intTmp == 65535) intTmp = 0;
                    //oTmp.EnableSensors
                    for (byte bytI = 0; bytI <= 10; bytI++)
                    {
                        oTmp.EnableSensors[bytI] = Convert.ToByte((intTmp & (1 << bytI)) == (1 << bytI));
                    }
                    oTmp.DelayTimeT = CsConst.myRevBuf[30] * 256 + CsConst.myRevBuf[31];
                    oTmp.DelayTimeF = CsConst.myRevBuf[32] * 256 + CsConst.myRevBuf[33];
                    Array.Copy(CsConst.myRevBuf, 34, oTmp.Paramters, 0, 15);
                    oTmp.UV1 = new Sensor_7in1.UVSet();
                    oTmp.UV1.UvNum = Convert.ToByte(HDLPF.IsNumStringMode(CsConst.myRevBuf[49].ToString(), 201, 248));
                    oTmp.UV1.UVCondition = CsConst.myRevBuf[50];

                    oTmp.UV2 = new Sensor_7in1.UVSet();
                    oTmp.UV2.UvNum = Convert.ToByte(HDLPF.IsNumStringMode(CsConst.myRevBuf[51].ToString(), 201, 248));
                    if (oTmp.UV2.UvNum == oTmp.UV1.UvNum) oTmp.UV2.UvNum = Convert.ToByte(oTmp.UV1.UvNum + 1);
                    oTmp.UV2.UVCondition = CsConst.myRevBuf[52];
                    oTmp.Paramters[15] = CsConst.myRevBuf[53];
                    oTmp.Paramters[16] = CsConst.myRevBuf[54];
                    
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

        private void dgvLogic_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            dgvLogic.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void chb0_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

    }
}
