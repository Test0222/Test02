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
    public partial class frmFH : Form
    {
        private string myDevName = null;
        private byte SubNetID;
        private byte DevID;
        private int mintIDIndex = -1;
        private int MyintDeviceType;
        private FH myFH = null;
        private bool isRead = false;
        private int MyActivePage = 0; //按页面上传下载
        private int PumpsSelectedIndex = 0;
        private TextBox txtSub = new TextBox();
        private TextBox txtDev = new TextBox();
        private ComboBox cbControlType = new ComboBox();
        private ComboBox cbbox1 = new ComboBox();
        private ComboBox cbbox2 = new ComboBox();
        private ComboBox cbbox3 = new ComboBox();
        private TextBox txtbox1 = new TextBox();
        private TextBox txtbox2 = new TextBox();
        private TextBox txtbox3 = new TextBox();
        private TimeText txtSeries = new TimeText(":");
        private ComboBox cbPanleControl = new ComboBox();
        private ComboBox cbAudioControl = new ComboBox();

        private TextBox txtSub2 = new TextBox();
        private TextBox txtDev2 = new TextBox();
        private ComboBox cbControlType2 = new ComboBox();
        private ComboBox cbbox12 = new ComboBox();
        private ComboBox cbbox22 = new ComboBox();
        private ComboBox cbbox32 = new ComboBox();
        private TextBox txtbox12 = new TextBox();
        private TextBox txtbox22 = new TextBox();
        private TextBox txtbox32 = new TextBox();
        private TimeText txtSeries2 = new TimeText(":");
        private ComboBox cbPanleControl2 = new ComboBox();
        private ComboBox cbAudioControl2 = new ComboBox();

        private TextBox txtSub3 = new TextBox();
        private TextBox txtDev3 = new TextBox();
        private ComboBox cbControlType3 = new ComboBox();
        private ComboBox cbbox13 = new ComboBox();
        private ComboBox cbbox23 = new ComboBox();
        private ComboBox cbbox33 = new ComboBox();
        private TextBox txtbox13 = new TextBox();
        private TextBox txtbox23 = new TextBox();
        private TextBox txtbox33 = new TextBox();
        private TimeText txtSeries3 = new TimeText(":");
        private ComboBox cbPanleControl3 = new ComboBox();
        private ComboBox cbAudioControl3 = new ComboBox();

        private TextBox txtSub4 = new TextBox();
        private TextBox txtDev4 = new TextBox();
        private ComboBox cbControlType4 = new ComboBox();
        private ComboBox cbbox14 = new ComboBox();
        private ComboBox cbbox24 = new ComboBox();
        private ComboBox cbbox34 = new ComboBox();
        private TextBox txtbox14 = new TextBox();
        private TextBox txtbox24 = new TextBox();
        private TextBox txtbox34 = new TextBox();
        private TimeText txtSeries4 = new TimeText(":");
        private ComboBox cbPanleControl4 = new ComboBox();
        private ComboBox cbAudioControl4 = new ComboBox();
        private System.Windows.Forms.Panel pnl1 = new System.Windows.Forms.Panel();
        private System.Windows.Forms.Panel pnl2 = new System.Windows.Forms.Panel();
        private System.Windows.Forms.Panel pnl3 = new System.Windows.Forms.Panel();
        private System.Windows.Forms.Panel pnl4 = new System.Windows.Forms.Panel();
        private System.Windows.Forms.Panel pnl5 = new System.Windows.Forms.Panel();
        private System.Windows.Forms.Panel pnl6 = new System.Windows.Forms.Panel();
        public frmFH()
        {
            InitializeComponent();
        }

        public frmFH(FH mybacknet, string strName, int intDIndex, int intDeviceType)
        {
            InitializeComponent();
            this.myDevName = strName;
            this.mintIDIndex = intDIndex;
            string strDevName = strName.Split('\\')[0].ToString();
            this.MyintDeviceType = intDeviceType;
            this.myFH = mybacknet;

            HDLSysPF.DisplayDeviceNameModeDescription(strName, MyintDeviceType, cboDevice, tbModel, tbDescription);

            SubNetID = Convert.ToByte(strDevName.Split('-')[0]);
            DevID = Convert.ToByte(strDevName.Split('-')[1]);            
        }


        private void frmFH_Shown(object sender, EventArgs e)
        {
            SetCtrlsVisbleWithDifferentDeviceType();
            MyActivePage = 1;
            tslRead_Click(tslRead, null);
        }

        private void tslRead_Click(object sender, EventArgs e)
        {
            setAllControlVisible(false);
            byte bytTag = Convert.ToByte(((ToolStripButton)sender).Tag.ToString());
            bool blnShowMsg = (CsConst.MyEditMode != 1);
            UpdateActivePageIndexWithUnit();
            if (HDLPF.UploadOrReadGoOn(this.Text, bytTag, blnShowMsg))
            {
                Cursor.Current = Cursors.WaitCursor;

                CsConst.MyUPload2DownLists = new List<byte[]>();
                string strName = myDevName.Split('\\')[0].ToString();
                byte bytSubID = byte.Parse(strName.Split('-')[0]);
                byte bytDevID = byte.Parse(strName.Split('-')[1]);
                byte num1 = 0;
                byte num2 = 0;
                if (tabControl1.SelectedTab.Name == "tabPage2")
                {
                    if (cbFH.SelectedIndex < 0)
                        num1 = 1;
                    else
                        num1 = Convert.ToByte(cbFH.SelectedIndex + 1);
                }
                else if (tabControl1.SelectedTab.Name == "tabPage4")
                {
                    num1 = 0;
                    num2 = 0;
                    byte chn1 = Convert.ToByte(cbSwitch.SelectedIndex + 1);
                    if (chn1 == 0) chn1 = 1;
                    byte chn2 = Convert.ToByte(cbRelay.SelectedIndex + 1);
                    if (chn2 == 0) chn2 = 1;
                    byte s1 = 0;
                    byte s2 = 0;
                    if (rbS1.Checked) s1 = 0;
                    else if (rbS2.Checked) s1 = 1;
                    else if (rbS3.Checked) s1 = 2;
                    else if (rbS4.Checked) s1 = 3;
                    if (rbR1.Checked) s2 = 0;
                    else if (rbR2.Checked) s2 = 1;
                    num1 = Convert.ToByte((chn1 << 4) | chn2);
                    num2 = Convert.ToByte((s1 << 4) | s2);
                }
                byte[] ArayRelay = new byte[] { bytSubID, bytDevID, (byte)(MyintDeviceType / 256), (byte)(MyintDeviceType % 256), 
                    (byte)MyActivePage,(byte)(mintIDIndex / 256), (byte)(mintIDIndex % 256),num1,num2 };
                CsConst.MyUPload2DownLists.Add(ArayRelay);
                CsConst.MyUpload2Down = Convert.ToByte(((ToolStripButton)sender).Tag.ToString());
                FrmDownloadShow Frm = new FrmDownloadShow();
                if (CsConst.MyUpload2Down==0) Frm.FormClosing += new FormClosingEventHandler(UpdateDisplayInformationAccordingly);
                Frm.ShowDialog();
            }
        }

        void UpdateDisplayInformationAccordingly(object sender, FormClosingEventArgs e)
        {
            switch (tabControl1.SelectedTab.Name)
            {
                case "tabBasic": ShowBasicInfo(); break;
                case "tabChn": ShowFloorHeatingInfo(); break;
                case "tabMaster": ShowOtherInfo(); break;
                case "tabAdvCmd": ShowTargetsInfo(); break;
                case "tabPump": showPumpsInfo(); break;
            }
            Cursor.Current = Cursors.Default;
            this.TopMost = true;
            this.WindowState = FormWindowState.Normal;
            this.Activate();
            this.TopMost = false;
            isRead = false;
        }

        private void ShowBasicInfo()
        {
            try
            {
                if (myFH == null) return;
                grp1.Visible = (MyintDeviceType == 211 || MyintDeviceType == 208 || MyintDeviceType == 209);
                isRead = true;
                int wdYear = Convert.ToInt32(myFH.arayDateTime[0]) + 2000;
                byte bytMonth = myFH.arayDateTime[1];
                byte bytDay = myFH.arayDateTime[2];
                byte bytHour = myFH.arayDateTime[3];
                byte bytMinute = myFH.arayDateTime[4];
                byte bytSecond = myFH.arayDateTime[5];

                if (bytHour > 23) bytHour = 0;
                if (bytMinute > 59) bytMinute = 0;
                if (bytSecond > 59) bytSecond = 0;
                if (bytMonth > 12 || bytMonth < 1) bytMonth = 1;
                if (bytDay > 31 || bytDay < 1) bytDay = 1;

                DateTime d = new DateTime(wdYear, Convert.ToInt32(bytMonth), Convert.ToInt32(bytDay));
                DatePicker.Value = d;
                DatePicker_ValueChanged(null, null);
                numTime1.Value = Convert.ToDecimal(bytHour);
                numTime2.Value = Convert.ToDecimal(bytMinute);
                numTime3.Value = Convert.ToDecimal(bytSecond);

                if (myFH.arayDateTime[7] == 1) chbTime.Checked = true;
                else chbTime.Checked = false;
                if (myFH.arayDateTime[8] < cbClock.Items.Count) cbClock.SelectedIndex = myFH.arayDateTime[8];
                if (cbClock.SelectedIndex < 0) cbClock.SelectedIndex = 0;

                chb1.Checked = (myFH.araySummer[0] == 1);
                if (1 <= myFH.araySummer[3] && myFH.araySummer[3] <= 12) cbM1.SelectedIndex = myFH.araySummer[3] - 1;
                else cbM1.SelectedIndex = 0;
                if (0 <= myFH.araySummer[7] && myFH.araySummer[7] <= 6) cbD1.SelectedIndex = myFH.araySummer[7];
                else cbD1.SelectedIndex = 0;
                if (0 <= myFH.araySummer[6] && myFH.araySummer[6] <= 6) cbW1.SelectedIndex = myFH.araySummer[6];
                else cbW1.SelectedIndex = 0;
                if (0 <= myFH.araySummer[5] && myFH.araySummer[5] <= 23) cbT1.SelectedIndex = myFH.araySummer[5];
                else cbT1.SelectedIndex = 0;

                chb2.Checked = (myFH.araySummer[8] == 1);
                if (1 <= myFH.araySummer[11] && myFH.araySummer[11] <= 12) cbM2.SelectedIndex = myFH.araySummer[11] - 1;
                else cbM2.SelectedIndex = 0;
                if (0 <= myFH.araySummer[15] && myFH.araySummer[15] <= 6) cbD2.SelectedIndex = myFH.araySummer[15];
                else cbD2.SelectedIndex = 0;
                if (0 <= myFH.araySummer[14] && myFH.araySummer[14] <= 6) cbW2.SelectedIndex = myFH.araySummer[14];
                else cbW2.SelectedIndex = 0;
                if (0 <= myFH.araySummer[13] && myFH.araySummer[13] <= 23) cbT2.SelectedIndex = myFH.araySummer[13];
                else cbT2.SelectedIndex = 0;

                if (grp1.Visible)
                {
                    chbOutdoor.Checked = (myFH.arayOutdoor[0] == 1);
                    txtOutdoor.Text = myFH.arayOutdoor[1].ToString();
                    if (myFH.arayOutdoor[3] == 1) cbOutdoor.Text = "F";
                    else cbOutdoor.Text = "C";
                    if (5 <= myFH.arayOutdoor[2] && myFH.arayOutdoor[2] <= 15)
                        sbAdjust.Value = myFH.arayOutdoor[2];
                    else
                        sbAdjust.Value = 10;

                    if (HDLSysPF.GetBit(myFH.arayOutdoor[4], 7) == 1)
                        lb2.Text = "-" + ((myFH.arayOutdoor[4] & (byte.MaxValue - (1 << 7)))).ToString() + cbOutdoor.Text;
                    else
                        lb2.Text = myFH.arayOutdoor[4].ToString() + cbOutdoor.Text;
                    sbAdjust_ValueChanged(null, null);
                }
                chb1_CheckedChanged(null, null);
                chb2_CheckedChanged(null, null);
            }
            catch
            {
            }
            isRead = false;
        }

        private void ShowFloorHeatingInfo()
        {
            try
            {
                isRead = true;
                lbMax.Visible=(MyintDeviceType == 210);
                sbMax.Visible=(MyintDeviceType == 210);
                lbMaxVauel.Visible=(MyintDeviceType == 210);
                lbTempType.Visible = (MyintDeviceType == 210);
                pnlID.Visible = (MyintDeviceType == 210);
                lb29.Visible = (MyintDeviceType == 211 || MyintDeviceType == 208 || MyintDeviceType == 209);
                cbOutput.Visible = (MyintDeviceType == 211 || MyintDeviceType == 208 || MyintDeviceType == 209);
                chbW4.Visible = (MyintDeviceType == 211 || MyintDeviceType == 208 || MyintDeviceType == 209);
                chbW3.Visible = (MyintDeviceType == 211 || MyintDeviceType == 208 || MyintDeviceType == 209);
                chbW5.Visible = (MyintDeviceType == 211 || MyintDeviceType == 208 || MyintDeviceType == 209);
                chbID1.Visible = (MyintDeviceType == 211 || MyintDeviceType == 208 || MyintDeviceType == 209);
                chbID2.Visible = (MyintDeviceType == 211 || MyintDeviceType == 208 || MyintDeviceType == 209);
                chbID3.Visible = (MyintDeviceType == 211 || MyintDeviceType == 208 || MyintDeviceType == 209);
                chbID4.Visible = (MyintDeviceType == 211 || MyintDeviceType == 208 || MyintDeviceType == 209);
                txtID1.Visible = (MyintDeviceType == 211 || MyintDeviceType == 208 || MyintDeviceType == 209);
                txtID2.Visible = (MyintDeviceType == 211 || MyintDeviceType == 208 || MyintDeviceType == 209);
                txtID3.Visible = (MyintDeviceType == 211 || MyintDeviceType == 208 || MyintDeviceType == 209);
                txtID4.Visible = (MyintDeviceType == 211 || MyintDeviceType == 208 || MyintDeviceType == 209);
                if (cbFH.SelectedIndex < 0) cbFH.SelectedIndex = 0;
                for (int i = 0; i < myFH.myHeating.Count; i++)
                {
                    FH.FHeating temp = myFH.myHeating[i];
                    if (temp.ID == Convert.ToByte(cbFH.SelectedIndex + 1))
                    {
                        txtChannel.Text = temp.strRemark;
                        if ((temp.arayWorkControl[0] & 0x0F) == 1) chbChn.Checked = true;
                        else chbChn.Enabled = false;
                        if (MyintDeviceType == 210)
                        {
                            #region
                            if ((temp.arayWorkControl[0] >> 4) == 1) rbCool.Checked = true;
                            else rbHeat.Checked = true;
                            string strTempType = "C";
                            if (temp.arayWorkControl[1] == 1)
                            {
                                strTempType = "F";
                                for (int j = 1; j <= 4; j++)
                                {
                                    HScrollBar tmp = this.Controls.Find("sb" + j.ToString(), true)[0] as HScrollBar;
                                    tmp.Maximum = 95;
                                    tmp.Minimum = 41;
                                }
                                cbMax.Items.Clear();
                                for (int j = 41; j <= 104; j++)
                                {
                                    if (rbHeat.Checked)
                                        cbMax.Items.Add(">=" + j.ToString() + strTempType);
                                    else
                                        cbMax.Items.Add("<=" + j.ToString() + strTempType);
                                }
                            }
                            else
                            {
                                strTempType = "C";
                                for (int j = 1; j <= 4; j++)
                                {
                                    HScrollBar tmp = this.Controls.Find("sb" + j.ToString(), true)[0] as HScrollBar;
                                    tmp.Maximum = 35;
                                    tmp.Minimum = 5;
                                }
                                cbMax.Items.Clear();
                                for (int j = 5; j <= 40; j++)
                                {
                                    if (rbHeat.Checked)
                                        cbMax.Items.Add(">=" + j.ToString() + strTempType);
                                    else
                                        cbMax.Items.Add("<=" + j.ToString() + strTempType);
                                }
                            }
                            lbTempType.Text = strTempType;
                            if (1 <= temp.arayWorkControl[2] && temp.arayWorkControl[2] <= 5) cbCurMode.SelectedIndex = temp.arayWorkControl[2] - 1;
                            else cbCurMode.SelectedIndex = 0;
                            for (int j = 3; j <= 6; j++)
                            {
                                HScrollBar tmp = this.Controls.Find("sb" + (j - 2).ToString(), true)[0] as HScrollBar;
                                if (strTempType == "C")
                                {
                                    if (5 <= temp.arayWorkControl[j] && temp.arayWorkControl[j] <= 35)
                                    {
                                        tmp.Value = temp.arayWorkControl[j];
                                    }
                                    else
                                    {
                                        tmp.Value = 20;
                                    }
                                }
                                else
                                {
                                    if (41 <= temp.arayWorkControl[j] && temp.arayWorkControl[j] <= 95)
                                    {
                                        tmp.Value = temp.arayWorkControl[j];
                                    }
                                    else
                                    {
                                        tmp.Value = 68;
                                    }
                                }
                            }
                            if (temp.arayWorkControl[7] == 0) lbS1.Text = CsConst.mstrINIDefault.IniReadValue("public", "00790", "");
                            else lbS1.Text = CsConst.mstrINIDefault.IniReadValue("public", "00791", "");
                            if (temp.arayWorkControl[8] == 0x80) temp.arayWorkControl[8] = 0;
                            if ((temp.arayWorkControl[8] & 0x80) > 0) lbS4.Text = "-" + (temp.arayWorkControl[8] & 0x7F).ToString();
                            else lbS4.Text = (temp.arayWorkControl[8] & 0x7F).ToString();
                            if(temp.arayWorkControl[9]==1) lbS2.Text = CsConst.mstrINIDefault.IniReadValue("public", "00037", "");
                            else lbS2.Text = CsConst.mstrINIDefault.IniReadValue("public", "00038", "");
                            if (temp.arayWorkControl[10] > 100) temp.arayWorkControl[10] = 0;
                            lbS3.Text = temp.arayWorkControl[10].ToString();


                            if (temp.araySensorSetting[0] == 1) cbMode.SelectedIndex = 1;
                            else cbMode.SelectedIndex = 0;
                            if (temp.araySensorSetting[1] == 1) rb2.Checked = true;
                            else if (temp.araySensorSetting[1] == 2) rb3.Checked = true;
                            else rb1.Checked = true;
                            txtSensorID.Text = temp.araySensorSetting[2].ToString();
                            txt1.Text = temp.araySensorSetting[3].ToString();
                            txt2.Text = temp.araySensorSetting[4].ToString();
                            if (1 <= temp.araySensorSetting[5] && temp.araySensorSetting[5] <= 10) cb1.SelectedIndex = temp.araySensorSetting[5] - 1;
                            else cb1.SelectedIndex = 0;
                            if ((temp.araySensorSetting[6] & 0x80) == 0x80) chbProtect.Checked = true;
                            else chbProtect.Checked = false;
                            if ((temp.araySensorSetting[6] & 1) == 1) rb5.Checked = true;
                            else rb4.Checked = true;
                            txtID.Text = temp.araySensorSetting[7].ToString();
                            txt3.Text = temp.araySensorSetting[8].ToString();
                            txt4.Text = temp.araySensorSetting[9].ToString();
                            if (1 <= temp.araySensorSetting[10] && temp.araySensorSetting[10] <= 10) cb2.SelectedIndex = temp.araySensorSetting[10] - 1;
                            else cb2.SelectedIndex = 0;
                            chbTemp.Checked = ((temp.araySensorSetting[11] & 0x80) == 0x80);
                            temp.araySensorSetting[11] = Convert.ToByte(temp.araySensorSetting[11] & 0x7F);
                            string strTmp = "";
                            string strTmp1 = "";
                            if (temp.araySensorSetting[14] > 0x7F) strTmp = "-";
                            else strTmp = "";
                            temp.araySensorSetting[14] = Convert.ToByte(temp.araySensorSetting[14] & 0x7F);
                            if (temp.araySensorSetting[15] > 0x7F) strTmp1 = "-";
                            else strTmp1 = "";
                            temp.araySensorSetting[15] = Convert.ToByte(temp.araySensorSetting[15] & 0x7F);
                            if (lbTempType.Text == "C")
                            {
                                if (5 <= temp.araySensorSetting[11] && temp.araySensorSetting[11] <= 40) cbMax.SelectedIndex = temp.araySensorSetting[11] - 5;
                                else cbMax.SelectedIndex = 20;
                                lbCurValue1.Text = temp.araySensorSetting[14].ToString() + lbTempType.Text;
                            }
                            else
                            {
                                if (41 <= temp.araySensorSetting[11] && temp.araySensorSetting[11] <= 104) cbMax.SelectedIndex = temp.araySensorSetting[11] - 41;
                                else cbMax.SelectedIndex = 36;
                                int intvalue = Convert.ToInt32(strTmp + temp.araySensorSetting[13].ToString()) * 9 + 160;
                                if ((intvalue % 5) > 2)
                                    intvalue = (intvalue / 5) + 2;
                                else
                                    intvalue = intvalue / 5;
                                
                                int intvalue1 = Convert.ToInt32(strTmp1 + temp.araySensorSetting[14].ToString()) * 9 + 160;
                                if ((intvalue1 % 5) > 2)
                                    intvalue1 = (intvalue1 / 5) + 2;
                                else
                                    intvalue1 = intvalue1 / 5;
                                lbCurValue1.Text = intvalue1.ToString() + lbTempType.Text;
                            }
                            if (5 <= temp.araySensorSetting[12] && temp.araySensorSetting[12] <= 15) sbMax.Value = temp.araySensorSetting[12];
                            else sbMax.Value = 10;
                            #endregion
                        }
                        else if (MyintDeviceType == 211 || MyintDeviceType == 208 || MyintDeviceType == 209)
                        {
                            #region
                            if ((temp.arayWorkControl[0] & (1 << 4)) > 0) rbCool.Checked = true;
                            else rbHeat.Checked = true;
                            if ((temp.arayWorkControl[0] & (1 << 5)) > 0) cbOutput.SelectedIndex = 1;
                            else cbOutput.SelectedIndex = 0;
                            string strTempType = "C";
                            if (temp.arayWorkControl[1] == 1) strTempType = "F";
                            else strTempType = "C";
                            if (strTempType == "C") cbType.SelectedIndex = 0;
                            else cbType.SelectedIndex = 1;
                            if (strTempType == "F")
                            {
                                cbMax.Items.Clear();
                                for (int j = 41; j <= 104; j++)
                                {
                                    if (rbHeat.Checked)
                                        cbMax.Items.Add(">=" + j.ToString() + strTempType);
                                    else
                                        cbMax.Items.Add("<=" + j.ToString() + strTempType);
                                }
                            }
                            else
                            {
                                cbMax.Items.Clear();
                                for (int j = 5; j <= 40; j++)
                                {
                                    if (rbHeat.Checked)
                                        cbMax.Items.Add(">=" + j.ToString() + strTempType);
                                    else
                                        cbMax.Items.Add("<=" + j.ToString() + strTempType);
                                }
                            }
                            if (cbOutput.SelectedIndex == 0)
                            {
                                if (strTempType == "F")
                                {
                                    for (int j = 1; j <= 4; j++)
                                    {
                                        HScrollBar tmp = this.Controls.Find("sb" + j.ToString(), true)[0] as HScrollBar;
                                        tmp.Maximum = 95;
                                        tmp.Minimum = 41;
                                    }
                                }
                                else
                                {
                                    for (int j = 1; j <= 4; j++)
                                    {
                                        HScrollBar tmp = this.Controls.Find("sb" + j.ToString(), true)[0] as HScrollBar;
                                        tmp.Maximum = 35;
                                        tmp.Minimum = 5;
                                    }
                                }
                                for (int j = 3; j <= 6; j++)
                                {
                                    if (strTempType == "C")
                                    {
                                        if (5 <= temp.arayWorkControl[j] && temp.arayWorkControl[j] <= 35)
                                        {
                                        }
                                        else
                                        {
                                            temp.arayWorkControl[j] = 20;
                                        }
                                    }
                                    else
                                    {
                                        if (41 <= temp.arayWorkControl[j] && temp.arayWorkControl[j] <= 95)
                                        {
                                        }
                                        else
                                        {
                                            temp.arayWorkControl[j] = 68;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                for (int j = 1; j <= 4; j++)
                                {
                                    HScrollBar tmp = this.Controls.Find("sb" + j.ToString(), true)[0] as HScrollBar;
                                    tmp.Maximum = 10;
                                    tmp.Minimum = 0;
                                }
                                for (int j = 3; j <= 6; j++)
                                {
                                    if (0 <= temp.arayWorkControl[j] && temp.arayWorkControl[j] <= 10)
                                    {
                                    }
                                    else
                                    {
                                        temp.arayWorkControl[j] = 0;
                                    }
                                }
                            }
                            if (1 <= temp.arayWorkControl[2] && temp.arayWorkControl[2] <= 5) cbCurMode.SelectedIndex = temp.arayWorkControl[2] - 1;
                            else cbCurMode.SelectedIndex = 0;
                            for (int j = 3; j <= 6; j++)
                            {
                                HScrollBar tmp = this.Controls.Find("sb" + (j - 2).ToString(), true)[0] as HScrollBar;
                                tmp.Value = temp.arayWorkControl[j];
                            }
                            if (temp.arayWorkControl[7] == 0) lbS1.Text = CsConst.mstrINIDefault.IniReadValue("public", "00790", "");
                            else lbS1.Text = CsConst.mstrINIDefault.IniReadValue("public", "00791", "");
                            if (temp.arayWorkControl[8] == 0x80) temp.arayWorkControl[8] = 0;
                            if ((temp.arayWorkControl[8] & 0x80) > 0) lbS4.Text = "-" + (temp.arayWorkControl[8] & 0x7F).ToString();
                            else lbS4.Text = (temp.arayWorkControl[8] & 0x7F).ToString();
                            if (temp.arayWorkControl[9] == 1) lbS2.Text = CsConst.mstrINIDefault.IniReadValue("public", "00037", "");
                            else lbS2.Text = CsConst.mstrINIDefault.IniReadValue("public", "00038", "");
                            if (temp.arayWorkControl[10] > 100) temp.arayWorkControl[10] = 0;
                            lbS3.Text = temp.arayWorkControl[10].ToString();

                            if (temp.araySensorSetting[0] == 1) cbMode.SelectedIndex = 1;
                            else cbMode.SelectedIndex = 0;
                            if (temp.araySensorSetting[1] == 1) rb2.Checked = true;
                            else if (temp.araySensorSetting[1] == 2) rb3.Checked = true;
                            else rb1.Checked = true;
                            if ((temp.arayWorkControl[12] & 1) > 0) chbID1.Checked = true;
                            else chbID1.Checked = false;
                            if ((temp.arayWorkControl[12] & 2) > 0) chbID2.Checked = true;
                            else chbID2.Checked = false;
                            if ((temp.arayWorkControl[12] & 4) > 0) chbID3.Checked = true;
                            else chbID3.Checked = false;
                            if ((temp.arayWorkControl[12] & 8) > 0) chbID4.Checked = true;
                            else chbID4.Checked = false;
                            txtID1.Text = temp.araySensorSetting[13].ToString();
                            txtID2.Text = temp.araySensorSetting[14].ToString();
                            txtID3.Text = temp.araySensorSetting[15].ToString();
                            txtID4.Text = temp.araySensorSetting[16].ToString();
                            txt1.Text = temp.araySensorSetting[3].ToString();
                            txt2.Text = temp.araySensorSetting[4].ToString();
                            if (1 <= temp.araySensorSetting[5] && temp.araySensorSetting[5] <= 10) cb1.SelectedIndex = temp.araySensorSetting[5] - 1;
                            else cb1.SelectedIndex = 0;
                            if ((temp.araySensorSetting[6] & 0x80) == 0x80) chbProtect.Checked = true;
                            else chbProtect.Checked = false;
                            if ((temp.araySensorSetting[6] & 1) == 1) rb5.Checked = true;
                            else rb4.Checked = true;
                            txtID.Text = temp.araySensorSetting[7].ToString();
                            txt3.Text = temp.araySensorSetting[8].ToString();
                            txt4.Text = temp.araySensorSetting[9].ToString();
                            if (1 <= temp.araySensorSetting[10] && temp.araySensorSetting[10] <= 10) cb2.SelectedIndex = temp.araySensorSetting[10] - 1;
                            else cb2.SelectedIndex = 0;
                            chbTemp.Checked = ((temp.araySensorSetting[11] & 0x80) == 0x80);
                            temp.araySensorSetting[11] = Convert.ToByte(temp.araySensorSetting[11] & 0x7F);
                            string strTmp = "";
                            string strTmp1 = "";
                            if (temp.araySensorSetting[17] > 0x7F) strTmp = "-";
                            else strTmp = "";
                            temp.araySensorSetting[17] = Convert.ToByte(temp.araySensorSetting[17] & 0x7F);
                            if (temp.araySensorSetting[18] > 0x7F) strTmp1 = "-";
                            else strTmp1 = "";
                            temp.araySensorSetting[18] = Convert.ToByte(temp.araySensorSetting[18] & 0x7F);
                            if (lbTempType.Text == "C")
                            {
                                if (5 <= temp.araySensorSetting[11] && temp.araySensorSetting[11] <= 40) cbMax.SelectedIndex = temp.araySensorSetting[11] - 5;
                                else cbMax.SelectedIndex = 20;
                                lbCurValue1.Text = temp.araySensorSetting[14].ToString() + lbTempType.Text;
                            }
                            else
                            {
                                if (41 <= temp.araySensorSetting[11] && temp.araySensorSetting[11] <= 104) cbMax.SelectedIndex = temp.araySensorSetting[11] - 41;
                                else cbMax.SelectedIndex = 36;
                                int intvalue = Convert.ToInt32(strTmp + temp.araySensorSetting[17].ToString()) * 9 + 160;
                                if ((intvalue % 5) > 2)
                                    intvalue = (intvalue / 5) + 2;
                                else
                                    intvalue = intvalue / 5;

                                int intvalue1 = Convert.ToInt32(strTmp1 + temp.araySensorSetting[18].ToString()) * 9 + 160;
                                if ((intvalue1 % 5) > 2)
                                    intvalue1 = (intvalue1 / 5) + 2;
                                else
                                    intvalue1 = intvalue1 / 5;
                                lbCurValue1.Text = intvalue1.ToString() + lbTempType.Text;
                            }
                            #endregion
                        }
                        #region
                        if (temp.arayWorkSetting[0] == 0) chbW1.Checked = false;
                        else chbW1.Checked = true;
                        if (temp.arayWorkSetting[1] <= 4) cbMin.SelectedIndex = temp.arayWorkSetting[1];
                        else cbMin.SelectedIndex = 0;
                        if (temp.arayWorkSetting[2] <= 4) cbHeatSpeed.SelectedIndex = temp.arayWorkSetting[2];
                        else cbHeatSpeed.SelectedIndex = 2;
                        if (temp.arayWorkSetting[3] <= 7) cbCycle.SelectedIndex = temp.arayWorkSetting[3];
                        else cbCycle.SelectedIndex = 3;
                        if (temp.arayWorkSetting[5] == 1) chbW2.Checked = true;
                        else chbW2.Checked = false;
                        if (temp.arayWorkSetting[6] > 23) temp.arayWorkSetting[6] = 0;
                        if (temp.arayWorkSetting[7] > 59) temp.arayWorkSetting[7] = 0;
                        if (temp.arayWorkSetting[8] > 23) temp.arayWorkSetting[8] = 0;
                        if (temp.arayWorkSetting[9] > 59) temp.arayWorkSetting[9] = 0;
                        numStart1.Value = temp.arayWorkSetting[6];
                        numStart2.Value = temp.arayWorkSetting[7];
                        numEnd1.Value = temp.arayWorkSetting[8];
                        numEnd2.Value = temp.arayWorkSetting[9];
                        if (MyintDeviceType == 211 || MyintDeviceType == 208 || MyintDeviceType == 209)
                        {
                            if ((temp.arayWorkSetting[10] & (1 << 0)) > 0) chbW3.Checked = true;
                            else chbW3.Checked = false;
                            if ((temp.arayWorkSetting[10] & (1 << 1)) > 0) chbW4.Checked = true;
                            else chbW4.Checked = false;
                            if ((temp.arayWorkSetting[10] & (1 << 2)) > 0) chbW5.Checked = true;
                            else chbW5.Checked = false;
                        }
                        #endregion
                        #region
                        chbFlush.Checked = (temp.arayFlush[0] == 1);
                        chbF1.Checked = ((temp.arayFlush[1] & 1) == 1);
                        chbF2.Checked = ((temp.arayFlush[1] & 2) == 2);
                        chbF3.Checked = ((temp.arayFlush[1] & 4) == 4);
                        if (temp.arayFlush[2] > 23) temp.arayFlush[2] = 0;
                        if (temp.arayFlush[3] > 59) temp.arayFlush[3] = 0;
                        numF1H.Value = temp.arayFlush[2];
                        numF1M.Value = temp.arayFlush[3];
                        if (1 <= temp.arayFlush[4] && temp.arayFlush[4] <= 30)
                            txtF1.Text = temp.arayFlush[4].ToString();
                        else
                            txtF1.Text = "1";
                        if (temp.arayFlush[5] > 23) temp.arayFlush[5] = 0;
                        if (temp.arayFlush[6] > 59) temp.arayFlush[6] = 0;
                        numF2H.Value = temp.arayFlush[5];
                        numF2M.Value = temp.arayFlush[6];
                        if (1 <= temp.arayFlush[7] && temp.arayFlush[7] <= 30)
                            txtF2.Text = temp.arayFlush[7].ToString();
                        else
                            txtF2.Text = "1";
                        if (temp.arayFlush[8] > 23) temp.arayFlush[8] = 0;
                        if (temp.arayFlush[9] > 59) temp.arayFlush[9] = 0;
                        numF3H.Value = temp.arayFlush[8];
                        numF3M.Value = temp.arayFlush[9];
                        if (1 <= temp.arayFlush[10] && temp.arayFlush[10] <= 30)
                            txtF3.Text = temp.arayFlush[10].ToString();
                        else
                            txtF3.Text = "1";
                        #endregion
                        break;
                    }
                }
                rb1_CheckedChanged(null, null);
                rb4_CheckedChanged(null, null);
                chbFlush_CheckedChanged(null, null);
                chbF1_CheckedChanged(null, null);
                chbF2_CheckedChanged(null, null);
                chbF3_CheckedChanged(null, null);
                sb1_ValueChanged(null, null);
                sb2_ValueChanged(null, null);
                sb3_ValueChanged(null, null);
                sb4_ValueChanged(null, null);
            }
            catch
            {
            }
            isRead = false;
        }

        private void ShowOtherInfo()
        {
            try
            {
                isRead = true;
                for (int i = 1; i <= 6; i++)
                {
                    ComboBox cb = this.Controls.Find("cbS" + i.ToString(), true)[0] as ComboBox;
                    if (myFH.araySynChannel[i - 1] <= 6) cb.SelectedIndex = myFH.araySynChannel[i - 1];
                    else cb.SelectedIndex = 0;
                }

                chbHost.Checked = (myFH.arayHost[0] == 1);
                if (1 <= myFH.arayHost[1] && myFH.arayHost[1] <= 6) cbHost.SelectedIndex = myFH.arayHost[1] - 1;
                else cbHost.SelectedIndex = 0;
                if ((myFH.arayHost[2] & 1) == 1) chbSlave1.Checked = true;
                else chbSlave1.Checked = false;
                if ((myFH.arayHost[2] & 2) == 2) chbSlave2.Checked = true;
                else chbSlave2.Checked = false;
                if ((myFH.arayHost[2] & 4) == 4) chbSlave3.Checked = true;
                else chbSlave3.Checked = false;
                if ((myFH.arayHost[2] & 8) == 8) chbSlave4.Checked = true;
                else chbSlave4.Checked = false;
                txtSubSlave1.Text = myFH.arayHost[3].ToString();
                txtDevSlave1.Text = myFH.arayHost[4].ToString();
                if (1 <= myFH.arayHost[5] && myFH.arayHost[5] <= 6) cbSlave1.SelectedIndex = myFH.arayHost[5] - 1;
                else cbSlave1.SelectedIndex = 0;
                txtSubSlave2.Text = myFH.arayHost[6].ToString();
                txtDevSlave2.Text = myFH.arayHost[7].ToString();
                if (1 <= myFH.arayHost[8] && myFH.arayHost[8] <= 6) cbSlave2.SelectedIndex = myFH.arayHost[8] - 1;
                else cbSlave2.SelectedIndex = 0;
                txtSubSlave3.Text = myFH.arayHost[9].ToString();
                txtDevSlave3.Text = myFH.arayHost[10].ToString();
                if (1 <= myFH.arayHost[11] && myFH.arayHost[11] <= 6) cbSlave3.SelectedIndex = myFH.arayHost[11] - 1;
                else cbSlave3.SelectedIndex = 0;
                txtSubSlave4.Text = myFH.arayHost[12].ToString();
                txtDevSlave4.Text = myFH.arayHost[13].ToString();
                if (1 <= myFH.arayHost[14] && myFH.arayHost[14] <= 6) cbSlave4.SelectedIndex = myFH.arayHost[14] - 1;
                else cbSlave4.SelectedIndex = 0;
                chbHost_CheckedChanged(null, null);
                chbSlave1_CheckedChanged(null, null);
                chbSlave2_CheckedChanged(null, null);
                chbSlave3_CheckedChanged(null, null);
                chbSlave4_CheckedChanged(null, null);
            }
            catch
            {
            }
            isRead = false;
        }

        private void ShowTargetsInfo()
        {
            try
            {
                groupBox7.Visible = (MyintDeviceType == 211 || MyintDeviceType == 208 || MyintDeviceType == 209);
                chbSwitch.Visible = (MyintDeviceType == 211 || MyintDeviceType == 208 || MyintDeviceType == 209);
                chbRelay.Visible = (MyintDeviceType == 211 || MyintDeviceType == 208 || MyintDeviceType == 209);
                if (myFH == null) return;
                dgvSwitch.Rows.Clear();
                dgvRelay.Rows.Clear();
                isRead = true;
                if (cbSwitch.SelectedIndex < 0) cbSwitch.SelectedIndex = 0;
                if (!rbS1.Checked && !rbS2.Checked && !rbS3.Checked && !rbS4.Checked) rbS1.Checked = true;
                byte bytStatues = 0;
                if (rbS1.Checked) bytStatues = 0;
                else if (rbS2.Checked) bytStatues = 1;
                else if (rbS3.Checked) bytStatues = 2;
                else if (rbS4.Checked) bytStatues = 3;
                for (int i = 0; i < myFH.myTargets1.Count; i++)
                {
                    FH.FHTargets temp = myFH.myTargets1[i];
                    if (temp.ID == (byte)(cbSwitch.SelectedIndex + 1))
                    {
                        if (temp.StatusIndex == bytStatues)
                        {
                            for (int j = 0; j < temp.Targets.Count; j++)
                            {
                                UVCMD.ControlTargets cmd = temp.Targets[j];
                                string strType = "";
                                strType = ButtonMode.ConvertorKeyModeToPublicModeGroup(cmd.Type);
                                string strParam1 = "0", strParam2 = "0", strParam3 = "0", strParam4 = "0";
                                strParam1 = cmd.Param1.ToString();
                                strParam2 = cmd.Param2.ToString();
                                strParam3 = cmd.Param3.ToString();
                                strParam4 = cmd.Param4.ToString();
                                SetParams(ref strType, ref strParam1, ref strParam2, ref strParam3, ref strParam4);
                                object[] obj = new object[] { (cmd.ID+1).ToString(),cmd.SubnetID.ToString(),cmd.DeviceID.ToString(),strType
                                ,strParam1,strParam2,strParam3,strParam4};
                                dgvSwitch.Rows.Add(obj);
                            }
                            break;
                        }
                    }
                }
                if (groupBox7.Visible)
                {
                    if (cbRelay.SelectedIndex < 0) cbRelay.SelectedIndex = 0;
                    if (!rbR1.Checked && !rbR2.Checked) rbR1.Checked = true;
                    bytStatues = 0;
                    if (rbR1.Checked) bytStatues = 0;
                    else if (rbR2.Checked) bytStatues = 1;
                    for (int i = 0; i < myFH.myTargets2.Count; i++)
                    {
                        FH.FHTargets temp = myFH.myTargets2[i];
                        if (temp.ID == (byte)(cbRelay.SelectedIndex + 1))
                        {
                            if (temp.StatusIndex == bytStatues)
                            {
                                for (int j = 0; j < temp.Targets.Count; j++)
                                {
                                    UVCMD.ControlTargets cmd = temp.Targets[j];
                                    string strType = "";
                                    strType = ButtonMode.ConvertorKeyModeToPublicModeGroup(cmd.Type);
                                    string strParam1 = "0", strParam2 = "0", strParam3 = "0", strParam4 = "0";
                                    strParam1 = cmd.Param1.ToString();
                                    strParam2 = cmd.Param2.ToString();
                                    strParam3 = cmd.Param3.ToString();
                                    strParam4 = cmd.Param4.ToString();
                                    SetParams(ref strType, ref strParam1, ref strParam2, ref strParam3, ref strParam4);
                                    object[] obj = new object[] { (cmd.ID+1).ToString(),cmd.SubnetID.ToString(),cmd.DeviceID.ToString(),strType
                                ,strParam1,strParam2,strParam3,strParam4};
                                    dgvRelay.Rows.Add(obj);
                                }
                                break;
                            }
                        }
                    }
                }
                if (chbSwitch.Visible)
                {
                    if ((myFH.FHTargetEnable & (1 << cbSwitch.SelectedIndex)) > 0) chbSwitch.Checked = true;
                    else chbSwitch.Checked = false;

                    if ((myFH.RelayTargetEnable & (1 << cbRelay.SelectedIndex)) > 0) chbRelay.Checked = true;
                    else chbRelay.Checked = false;
                }
            }
            catch
            {
            }
            isRead = false;
        }

        private void SetParams(ref string strType, ref string str1, ref string str2, ref string str3, ref string str4)
        {
            if (strType == CsConst.mstrINIDefault.IniReadValue("TargetType", "00000", ""))//无效
            {
                #region
                str1 = "N/A";
                str2 = "N/A";
                str3 = "N/A";
                str4 = "N/A";
                #endregion
            }
            else if (strType == CsConst.myPublicControlType[1].ControlTypeName)//场景
            {
                #region
                if (str1 == "255")
                {
                    strType = CsConst.WholeTextsList[1777].sDisplayName;
                    str1 = CsConst.WholeTextsList[2566].sDisplayName;
                    str2 = str2 + "(" + CsConst.WholeTextsList[2511].sDisplayName + ")";
                    str3 = "N/A";
                }
                else
                {
                    str1 = str1 + "(" + CsConst.WholeTextsList[2510].sDisplayName + ")";
                    str2 = str2 + "(" + CsConst.WholeTextsList[2511].sDisplayName + ")";
                    str3 = "N/A";
                }
                str4 = "N/A";
                #endregion
            }
            else if (strType == CsConst.myPublicControlType[2].ControlTypeName)//序列
            {
                #region
                str1 = str1 + "(" + CsConst.WholeTextsList[2510].sDisplayName + ")";
                str2 = str2 + "(" + CsConst.WholeTextsList[2512].sDisplayName+ ")";
                str3 = "N/A";
                str4 = "N/A";
                #endregion
            }
            else if (strType == CsConst.myPublicControlType[4].ControlTypeName)//通用开关
            {
                #region
                str1 = str1 + "(" + CsConst.WholeTextsList[2513].sDisplayName + ")";
                if (str2 == "0") str2 = CsConst.Status[0] + "(" + CsConst.WholeTextsList[2529].sDisplayName + ")";
                else if (str2 == "255") str2 = CsConst.Status[1] + "(" + CsConst.WholeTextsList[2529].sDisplayName + ")";
                str3 = "N/A";
                str4 = "N/A";
                #endregion
            }
            else if (strType == CsConst.myPublicControlType[5].ControlTypeName)//单路调节
            {
                #region
                if (str1 == "255")
                {
                    strType =  CsConst.myPublicControlType[11].ControlTypeName;
                    str1 = CsConst.WholeTextsList[2567].sDisplayName;
                    str2 = str2 + "(" + CsConst.WholeTextsList[2524].sDisplayName + ")";
                    int intTmp = Convert.ToInt32(str3) * 256 + Convert.ToInt32(str4);
                    str3 = HDLPF.GetStringFromTime(intTmp, ":") + "(" + CsConst.WholeTextsList[2525].sDisplayName + ")";
                }
                else
                {
                    str1 = str1 + "(" + CsConst.WholeTextsList[934].sDisplayName + ")";
                    str2 = str2 + "(" + CsConst.WholeTextsList[2524].sDisplayName + ")";
                    int intTmp = Convert.ToInt32(str3) * 256 + Convert.ToInt32(str4);
                    str3 = HDLPF.GetStringFromTime(intTmp, ":") + "(" + CsConst.WholeTextsList[2525].sDisplayName + ")";
                }
                str4 = "N/A";
                #endregion
            }
            else if (strType == CsConst.myPublicControlType[10].ControlTypeName)//广播场景
            {
                #region
                str1 = CsConst.WholeTextsList[2566].sDisplayName;
                str2 = str2 + "(" + CsConst.WholeTextsList[2511].sDisplayName + ")";
                str3 = "N/A";
                str4 = "N/A";
                #endregion
            }
            else if (strType == CsConst.myPublicControlType[11].ControlTypeName)//广播回路
            {
                #region
                str1 = CsConst.WholeTextsList[2567].sDisplayName;
                str2 = str2 + "(" + CsConst.WholeTextsList[2524].sDisplayName + ")";
                int intTmp = Convert.ToInt32(str3) * 256 + Convert.ToInt32(str4);
                str3 = HDLPF.GetStringFromTime(intTmp, ":") + "(" + CsConst.WholeTextsList[2525].sDisplayName + ")";
                str4 = "N/A";
                #endregion
            }
            else if (strType == CsConst.myPublicControlType[6].ControlTypeName)//窗帘开关
            {
                #region
                if (Convert.ToInt32(str1) >= 17 && Convert.ToInt32(str1) <= 34)
                {
                    str2 = (Convert.ToInt32(str2)).ToString() + "%" + "(" + CsConst.WholeTextsList[2529].sDisplayName + ")";
                    str1 = (Convert.ToInt32(str1) - 16).ToString() + "(" + CsConst.mstrINIDefault.IniReadValue("public", "99844", "") + ")";
                }
                else
                {
                    if (str2 == "0") str2 = CsConst.mstrINIDefault.IniReadValue("public", "00036", "") + "(" + CsConst.WholeTextsList[2529].sDisplayName + ")";
                    else if (str2 == "1") str2 = CsConst.mstrINIDefault.IniReadValue("public", "00037", "") + "(" + CsConst.WholeTextsList[2529].sDisplayName + ")";
                    else if (str2 == "2") str2 = CsConst.mstrINIDefault.IniReadValue("public", "00038", "") + "(" + CsConst.WholeTextsList[2529].sDisplayName + ")";
                    else str2 = (Convert.ToInt32(str2)).ToString() + "%" + "(" + CsConst.WholeTextsList[2529].sDisplayName + ")";
                    str1 = str1 + "(" + CsConst.mstrINIDefault.IniReadValue("public", "99844", "") + ")";
                }
                str3 = "N/A";
                str4 = "N/A";
                #endregion
            }
            else if (strType == CsConst.myPublicControlType[7].ControlTypeName)//GPRS
            {
                #region
                if (str1 == "1") str1 = CsConst.mstrINIDefault.IniReadValue("public", "99862", "");
                else if (str1 == "2") str1 = CsConst.mstrINIDefault.IniReadValue("public", "99863", "");
                else str1 = CsConst.WholeTextsList[1775].sDisplayName;
                str2 = str2 + "(" + CsConst.mstrINIDefault.IniReadValue("public", "99864", "") + ")";
                str3 = "N/A";
                str4 = "N/A";
                #endregion
            }
            else if (strType == CsConst.myPublicControlType[8].ControlTypeName)//面板控制
            {
                #region
                str1 = HDLSysPF.InquirePanelControTypeStringFromDB(Convert.ToInt32(str1));
                if (str1 == CsConst.myPublicControlType[0].ControlTypeName)
                {
                    str2 = "N/A";
                    str3 = "N/A";
                }
                else if (str1 == CsConst.myPublicControlType[1].ControlTypeName ||
                         str1 == CsConst.myPublicControlType[2].ControlTypeName ||
                         str1 == CsConst.myPublicControlType[5].ControlTypeName ||
                         str1 == CsConst.myPublicControlType[7].ControlTypeName ||
                         str1 == CsConst.myPublicControlType[8].ControlTypeName ||
                         str1 ==  CsConst.PanelControl[12] ||
                         str1 == CsConst.PanelControl[21])
                {
                    if (str2 == "0") str2 = CsConst.Status[0] + "(" + CsConst.WholeTextsList[2529].sDisplayName + ")";
                    else if (str2 == "1") str2 = CsConst.Status[1] + "(" + CsConst.WholeTextsList[2529].sDisplayName + ")";
                    str3 = "N/A";
                }
                else if (str1 == CsConst.myPublicControlType[3].ControlTypeName ||
                         str1 == CsConst.myPublicControlType[4].ControlTypeName)
                {
                    str2 = str2 + "(" + CsConst.WholeTextsList[2524].sDisplayName + ")";
                    str3 = "N/A";
                }
                else if (str1 == CsConst.myPublicControlType[6].ControlTypeName ||
                         str1 == CsConst.myPublicControlType[9].ControlTypeName ||
                         str1 ==  CsConst.PanelControl[10] ||
                         str1 ==  CsConst.PanelControl[11])
                {
                    int intTmp = Convert.ToInt32(str2);
                    if (str1 == CsConst.myPublicControlType[6].ControlTypeName)
                    {
                        if (1 <= intTmp && intTmp <= 7) str2 = CsConst.mstrINIDefault.IniReadValue("public", "00048", "") + intTmp.ToString();
                        else if (intTmp == 255) str2 = CsConst.mstrINIDefault.IniReadValue("public", "00049", "");
                        else str2 = CsConst.WholeTextsList[1775].sDisplayName;
                    }
                    else if (str1 == CsConst.myPublicControlType[9].ControlTypeName)
                    {
                        if (intTmp == 255) str2 = CsConst.mstrINIDefault.IniReadValue("public", "00049", "");
                        else if (1 <= intTmp && intTmp <= 56) str2 = CsConst.mstrINIDefault.IniReadValue("public", "99867", "") + intTmp.ToString();
                        else str2 = CsConst.WholeTextsList[1775].sDisplayName;
                    }
                    else if (str1 ==  CsConst.PanelControl[10])
                    {
                        if (intTmp == 255) str2 = CsConst.mstrINIDefault.IniReadValue("public", "00049", "");
                        else if (1 <= intTmp && intTmp <= 32) str2 = CsConst.mstrINIDefault.IniReadValue("public", "99867", "") + intTmp.ToString();
                        else if (intTmp == 101) str2 = CsConst.mstrINIDefault.IniReadValue("public", "99868", "");
                        else if (intTmp == 102) str2 = CsConst.mstrINIDefault.IniReadValue("public", "99869", "");
                        else if (intTmp == 103) str2 = CsConst.mstrINIDefault.IniReadValue("public", "99870", "");
                        else if (intTmp == 104) str2 = CsConst.mstrINIDefault.IniReadValue("public", "99871", "");
                        else str2 = CsConst.mstrINIDefault.IniReadValue("public", "00049", "");
                    }
                    else if (str1 ==  CsConst.PanelControl[11])
                    {
                        if (1 <= intTmp && intTmp <= 32) str2 = CsConst.mstrINIDefault.IniReadValue("public", "99867", "") + intTmp.ToString();
                        else str2 = CsConst.WholeTextsList[1775].sDisplayName;
                    }
                    if (str3 == "1") str3 = CsConst.mstrINIDefault.IniReadValue("public", "00042", "");
                    else str3 = CsConst.WholeTextsList[1775].sDisplayName;
                }
                else if (str1 ==  CsConst.PanelControl[13])
                {
                    int intTmp = Convert.ToInt32(str2);
                    if (intTmp <= 4) str2 = CsConst.mstrINIDefault.IniReadValue("public", "0005" + intTmp.ToString(), "");
                    str3 = "N/A";
                }
                else if (str1 ==  CsConst.PanelControl[14])
                {
                    int intTmp = Convert.ToInt32(str2);
                    if (intTmp <= 3) str2 = CsConst.mstrINIDefault.IniReadValue("public", "0006" + intTmp.ToString(), "");
                    str3 = "N/A";
                }
                else if (str1 ==  CsConst.PanelControl[15] ||
                         str1 ==  CsConst.PanelControl[16])
                {
                    str3 = "N/A";
                }
                else if (str1 == CsConst.PanelControl[23] ||
                         str1 == CsConst.PanelControl[24])
                {
                    if (str3 == "255") str3 = CsConst.mstrINIDefault.IniReadValue("public", "00049", "");
                    else str3 = str3 + "(" + CsConst.WholeTextsList[934].sDisplayName + ")";
                }
                else if (str1 == CsConst.PanelControl[17] ||
                         str1 == CsConst.PanelControl[18] ||
                         str1 == CsConst.PanelControl[19] ||
                         str1 == CsConst.PanelControl[20])
                {
                    int intTmp = Convert.ToInt32(str2);
                    if (0 <= intTmp && intTmp <= 30) str2 = str2 + "C";
                    else if (32 <= intTmp && intTmp <= 86) str2 = str2 + "F";
                    str3 = "N/A";
                }
                else if (str1 == CsConst.myPublicControlType[22].ControlTypeName)
                {
                    int intTmp = Convert.ToInt32(str2);
                    if (intTmp <= 5) str2 = CsConst.mstrINIDefault.IniReadValue("public", "0007" + (intTmp - 1).ToString(), "");
                    if (str3 == "255") str3 = CsConst.mstrINIDefault.IniReadValue("public", "00049", "");
                    else str3 = str3 + "(" + CsConst.WholeTextsList[934].sDisplayName + ")";
                }
                str4 = "N/A";
                #endregion
            }
            else if (strType == CsConst.myPublicControlType[12].ControlTypeName)//消防模块
            {
                #region
                str1 = str1 + "(" + CsConst.WholeTextsList[2510].sDisplayName + ")";
                int intTmp = Convert.ToInt32(str2);
                if (1 <= intTmp && intTmp <= 10) str2 = CsConst.mstrINIDefault.IniReadValue("public", "0008" + (intTmp - 1).ToString(), "");
                str3 = "N/A";
                str4 = "N/A";
                #endregion
            }
            else if (strType == CsConst.myPublicControlType[13].ControlTypeName)//音乐控制
            {
                #region
                int intTmp = Convert.ToInt32(str1);
                if (1 <= intTmp && intTmp <= 8) str1 = CsConst.mstrINIDefault.IniReadValue("public", "0009" + intTmp.ToString(), "");
                else str1 = CsConst.MusicControl[0];
                if (str1 == CsConst.MusicControl[0])
                {
                    intTmp = Convert.ToInt32(str2);
                    if (1 <= intTmp && intTmp <= 4) str2 = CsConst.mstrINIDefault.IniReadValue("public", "0010" + intTmp.ToString(), "");
                    str3 = "N/A";
                }
                else if (str1 == CsConst.mstrINIDefault.IniReadValue("public", "00092", ""))
                {
                    intTmp = Convert.ToInt32(str2);
                    if (1 <= intTmp && intTmp <= 4) str2 = CsConst.mstrINIDefault.IniReadValue("public", "0011" + intTmp.ToString(), "");
                    str3 = "N/A";
                }
                else if (str1 == CsConst.mstrINIDefault.IniReadValue("public", "00093", ""))
                {
                    intTmp = Convert.ToInt32(str2);
                    if (1 <= intTmp && intTmp <= 6) str2 = CsConst.mstrINIDefault.IniReadValue("public", "0012" + intTmp.ToString(), "");
                    if (intTmp == 3 || intTmp == 6)
                        str3 = str3 + "(" + CsConst.mstrINIDefault.IniReadValue("public", "00043", "") + ")";
                    else
                        str3 = "N/A";
                }
                else if (str1 == CsConst.mstrINIDefault.IniReadValue("public", "00094", ""))
                {
                    intTmp = Convert.ToInt32(str2);
                    if (1 <= intTmp && intTmp <= 4) str2 = CsConst.mstrINIDefault.IniReadValue("public", "0013" + intTmp.ToString(), "");
                    str3 = "N/A";
                }
                else if (str1 == CsConst.mstrINIDefault.IniReadValue("public", "00095", ""))
                {
                    intTmp = Convert.ToInt32(str2);
                    if (1 <= intTmp && intTmp <= 3) str2 = CsConst.mstrINIDefault.IniReadValue("public", "0014" + intTmp.ToString(), "");
                    if (intTmp == 1)
                    {
                        intTmp = Convert.ToInt32(str3);
                        if (intTmp >= 3)
                            str3 = CsConst.mstrINIDefault.IniReadValue("public", "99872", "") + ":" + (79 - (Convert.ToInt32(str4))).ToString();
                        else
                        {
                            if (intTmp == 1) str3 = CsConst.mstrINIDefault.IniReadValue("public", "00044", "");
                            else if (intTmp == 2) str3 = CsConst.mstrINIDefault.IniReadValue("public", "00045", "");
                        }
                    }
                    else
                    {
                        if (intTmp == 1) str3 = CsConst.mstrINIDefault.IniReadValue("public", "00044", "");
                        else if (intTmp == 2) str3 = CsConst.mstrINIDefault.IniReadValue("public", "00045", "");
                    }
                }
                else if (str1 == CsConst.mstrINIDefault.IniReadValue("public", "00096", ""))
                {
                    str2 = str2 + "(" + CsConst.mstrINIDefault.IniReadValue("public", "00046", "") + ")";
                    str3 = (Convert.ToInt32(str3) * 256 + Convert.ToInt32(str4)).ToString() + "(" + CsConst.mstrINIDefault.IniReadValue("public", "00043", "") + ")";
                }
                else if (str1 == CsConst.mstrINIDefault.IniReadValue("public", "00097", "") || str1 == CsConst.mstrINIDefault.IniReadValue("public", "00098", ""))
                {
                    intTmp = Convert.ToInt32(str2);
                    if (intTmp == 64)
                        str2 = CsConst.mstrINIDefault.IniReadValue("public", "00047", "");
                    else if (65 <= intTmp && intTmp <= 112)
                        str2 = "SD:" + (intTmp - 64).ToString();
                    else if (129 <= intTmp && intTmp <= 176)
                        str2 = "FTP:" + (intTmp - 128).ToString();
                    intTmp = Convert.ToInt32(str3) * 256 + Convert.ToInt32(str4);
                    str3 = intTmp.ToString() + "(" + CsConst.mstrINIDefault.IniReadValue("public", "00043", "") + ")";
                }
                str4 = "N/A";
                #endregion
            }
        }

        private void frmFH_Load(object sender, EventArgs e)
        {
            InitialFormCtrlsTextOrItems();           
        }

        void SetCtrlsVisbleWithDifferentDeviceType()
        {
            toolStrip1.Visible = (CsConst.MyEditMode == 0);
            if (MyintDeviceType != 208)
            {
                tabControl1.TabPages.Remove(tabPump);
            }
            addComboboxItem();
        }

        void InitialFormCtrlsTextOrItems()
        {
            #region
            cbControlType.Items.Clear();
            HDLSysPF.AddControlTypeToControl(cbControlType, MyintDeviceType);
            cbPanleControl.Items.Clear();
            HDLSysPF.getPanlControlType(cbPanleControl, MyintDeviceType);
            cbAudioControl.Items.Clear();
            for (int i = 1; i <= 8; i++)
                cbAudioControl.Items.Add(CsConst.mstrINIDefault.IniReadValue("public", "0009" + i.ToString(), ""));

            cbControlType.DropDownStyle = ComboBoxStyle.DropDownList;
            cbPanleControl.DropDownStyle = ComboBoxStyle.DropDownList;
            cbAudioControl.DropDownStyle = ComboBoxStyle.DropDownList;
            cbbox1.DropDownStyle = ComboBoxStyle.DropDownList;
            cbbox2.DropDownStyle = ComboBoxStyle.DropDownList;
            cbbox3.DropDownStyle = ComboBoxStyle.DropDownList;

            txtSub.KeyPress += txtOutdoor_KeyPress;
            txtDev.KeyPress += txtOutdoor_KeyPress;
            txtbox1.KeyPress += txtOutdoor_KeyPress;
            txtbox2.KeyPress += txtOutdoor_KeyPress;
            txtbox3.KeyPress += txtOutdoor_KeyPress;

            dgvSwitch.Controls.Add(cbControlType);
            dgvSwitch.Controls.Add(cbPanleControl);
            dgvSwitch.Controls.Add(cbAudioControl);
            dgvSwitch.Controls.Add(txtSub);
            dgvSwitch.Controls.Add(txtDev);
            dgvSwitch.Controls.Add(cbbox1);
            dgvSwitch.Controls.Add(cbbox2);
            dgvSwitch.Controls.Add(cbbox3);
            dgvSwitch.Controls.Add(txtSeries);
            dgvSwitch.Controls.Add(txtbox1);
            dgvSwitch.Controls.Add(txtbox2);
            dgvSwitch.Controls.Add(txtbox3);

            cbControlType.SelectedIndexChanged += cbControlType_SelectedIndexChanged;
            cbPanleControl.SelectedIndexChanged += cbPanleControl_SelectedIndexChanged;
            cbAudioControl.SelectedIndexChanged += cbAudioControl_SelectedIndexChanged;
            cbbox1.SelectedIndexChanged += cbbox1_SelectedIndexChanged;
            cbbox2.SelectedIndexChanged += cbbox2_SelectedIndexChanged;
            cbbox3.SelectedIndexChanged += cbbox3_SelectedIndexChanged;
            txtSub.TextChanged += txtSub_TextChanged;
            txtDev.TextChanged += txtDev_TextChanged;
            txtbox1.TextChanged += txtbox1_TextChanged;
            txtbox2.TextChanged += txtbox2_TextChanged;
            txtbox3.TextChanged += txtbox3_TextChanged;
            txtSeries.TextChanged += txtSeries_TextChanged;
            #endregion
            #region
            cbControlType2.Items.Clear();
            HDLSysPF.AddControlTypeToControl(cbControlType2, MyintDeviceType);
            cbPanleControl2.Items.Clear();
            HDLSysPF.getPanlControlType(cbPanleControl2, MyintDeviceType);
            cbAudioControl2.Items.Clear();
            for (int i = 1; i <= 8; i++)
                cbAudioControl2.Items.Add(CsConst.mstrINIDefault.IniReadValue("public", "0009" + i.ToString(), ""));

            cbControlType2.DropDownStyle = ComboBoxStyle.DropDownList;
            cbPanleControl2.DropDownStyle = ComboBoxStyle.DropDownList;
            cbAudioControl2.DropDownStyle = ComboBoxStyle.DropDownList;
            cbbox12.DropDownStyle = ComboBoxStyle.DropDownList;
            cbbox22.DropDownStyle = ComboBoxStyle.DropDownList;
            cbbox32.DropDownStyle = ComboBoxStyle.DropDownList;

            txtSub2.KeyPress += txtOutdoor_KeyPress;
            txtDev2.KeyPress += txtOutdoor_KeyPress;
            txtbox12.KeyPress += txtOutdoor_KeyPress;
            txtbox22.KeyPress += txtOutdoor_KeyPress;
            txtbox32.KeyPress += txtOutdoor_KeyPress;

            dgvRelay.Controls.Add(cbControlType2);
            dgvRelay.Controls.Add(cbPanleControl2);
            dgvRelay.Controls.Add(cbAudioControl2);
            dgvRelay.Controls.Add(txtSub2);
            dgvRelay.Controls.Add(txtDev2);
            dgvRelay.Controls.Add(cbbox12);
            dgvRelay.Controls.Add(cbbox22);
            dgvRelay.Controls.Add(cbbox32);
            dgvRelay.Controls.Add(txtSeries2);
            dgvRelay.Controls.Add(txtbox12);
            dgvRelay.Controls.Add(txtbox22);
            dgvRelay.Controls.Add(txtbox32);

            cbControlType2.SelectedIndexChanged += cbControlType2_SelectedIndexChanged;
            cbPanleControl2.SelectedIndexChanged += cbPanleControl2_SelectedIndexChanged;
            cbAudioControl2.SelectedIndexChanged += cbAudioControl2_SelectedIndexChanged;
            cbbox12.SelectedIndexChanged += cbbox12_SelectedIndexChanged;
            cbbox22.SelectedIndexChanged += cbbox22_SelectedIndexChanged;
            cbbox32.SelectedIndexChanged += cbbox32_SelectedIndexChanged;
            txtSub2.TextChanged += txtSub2_TextChanged;
            txtDev2.TextChanged += txtDev2_TextChanged;
            txtbox12.TextChanged += txtbox12_TextChanged;
            txtbox22.TextChanged += txtbox22_TextChanged;
            txtbox32.TextChanged += txtbox32_TextChanged;
            txtSeries2.TextChanged += txtSeries2_TextChanged;
            #endregion
            #region
            cbControlType3.Items.Clear();
            HDLSysPF.AddControlTypeToControl(cbControlType3, MyintDeviceType);
            cbPanleControl3.Items.Clear();
            HDLSysPF.getPanlControlType(cbPanleControl3, MyintDeviceType);
            cbAudioControl3.Items.Clear();
            for (int i = 1; i <= 8; i++)
                cbAudioControl3.Items.Add(CsConst.mstrINIDefault.IniReadValue("public", "0009" + i.ToString(), ""));

            cbControlType3.DropDownStyle = ComboBoxStyle.DropDownList;
            cbPanleControl3.DropDownStyle = ComboBoxStyle.DropDownList;
            cbAudioControl3.DropDownStyle = ComboBoxStyle.DropDownList;
            cbbox13.DropDownStyle = ComboBoxStyle.DropDownList;
            cbbox23.DropDownStyle = ComboBoxStyle.DropDownList;
            cbbox33.DropDownStyle = ComboBoxStyle.DropDownList;

            txtSub3.KeyPress += txtOutdoor_KeyPress;
            txtDev3.KeyPress += txtOutdoor_KeyPress;
            txtbox13.KeyPress += txtOutdoor_KeyPress;
            txtbox23.KeyPress += txtOutdoor_KeyPress;
            txtbox33.KeyPress += txtOutdoor_KeyPress;

            setAllControlVisible(false);
            dgvP1.Controls.Add(cbControlType3);
            dgvP1.Controls.Add(cbPanleControl3);
            dgvP1.Controls.Add(cbAudioControl3);
            dgvP1.Controls.Add(txtSub3);
            dgvP1.Controls.Add(txtDev3);
            dgvP1.Controls.Add(cbbox13);
            dgvP1.Controls.Add(cbbox23);
            dgvP1.Controls.Add(cbbox33);
            dgvP1.Controls.Add(txtSeries3);
            dgvP1.Controls.Add(txtbox13);
            dgvP1.Controls.Add(txtbox23);
            dgvP1.Controls.Add(txtbox33);

            cbControlType3.SelectedIndexChanged += cbControlType3_SelectedIndexChanged;
            cbPanleControl3.SelectedIndexChanged += cbPanleControl3_SelectedIndexChanged;
            cbAudioControl3.SelectedIndexChanged += cbAudioControl3_SelectedIndexChanged;
            cbbox13.SelectedIndexChanged += cbbox13_SelectedIndexChanged;
            cbbox23.SelectedIndexChanged += cbbox23_SelectedIndexChanged;
            cbbox33.SelectedIndexChanged += cbbox33_SelectedIndexChanged;
            txtSub3.TextChanged += txtSub3_TextChanged;
            txtDev3.TextChanged += txtDev3_TextChanged;
            txtbox13.TextChanged += txtbox13_TextChanged;
            txtbox23.TextChanged += txtbox23_TextChanged;
            txtbox33.TextChanged += txtbox33_TextChanged;
            txtSeries3.TextChanged += txtSeries3_TextChanged;
            #endregion
            #region
            cbControlType4.Items.Clear();
            HDLSysPF.AddControlTypeToControl(cbControlType4, MyintDeviceType);
            cbPanleControl4.Items.Clear();
            HDLSysPF.getPanlControlType(cbPanleControl4, MyintDeviceType);
            cbAudioControl4.Items.Clear();
            for (int i = 1; i <= 8; i++)
                cbAudioControl4.Items.Add(CsConst.mstrINIDefault.IniReadValue("public", "0009" + i.ToString(), ""));

            cbControlType4.DropDownStyle = ComboBoxStyle.DropDownList;
            cbPanleControl4.DropDownStyle = ComboBoxStyle.DropDownList;
            cbAudioControl4.DropDownStyle = ComboBoxStyle.DropDownList;
            cbbox14.DropDownStyle = ComboBoxStyle.DropDownList;
            cbbox24.DropDownStyle = ComboBoxStyle.DropDownList;
            cbbox34.DropDownStyle = ComboBoxStyle.DropDownList;

            txtSub4.KeyPress += txtOutdoor_KeyPress;
            txtDev4.KeyPress += txtOutdoor_KeyPress;
            txtbox14.KeyPress += txtOutdoor_KeyPress;
            txtbox24.KeyPress += txtOutdoor_KeyPress;
            txtbox34.KeyPress += txtOutdoor_KeyPress;

            dgvP2.Controls.Add(cbControlType4);
            dgvP2.Controls.Add(cbPanleControl4);
            dgvP2.Controls.Add(cbAudioControl4);
            dgvP2.Controls.Add(txtSub4);
            dgvP2.Controls.Add(txtDev4);
            dgvP2.Controls.Add(cbbox14);
            dgvP2.Controls.Add(cbbox24);
            dgvP2.Controls.Add(cbbox34);
            dgvP2.Controls.Add(txtSeries4);
            dgvP2.Controls.Add(txtbox14);
            dgvP2.Controls.Add(txtbox24);
            dgvP2.Controls.Add(txtbox34);

            cbControlType4.SelectedIndexChanged += cbControlType4_SelectedIndexChanged;
            cbPanleControl4.SelectedIndexChanged += cbPanleControl4_SelectedIndexChanged;
            cbAudioControl4.SelectedIndexChanged += cbAudioControl4_SelectedIndexChanged;
            cbbox14.SelectedIndexChanged += cbbox14_SelectedIndexChanged;
            cbbox24.SelectedIndexChanged += cbbox24_SelectedIndexChanged;
            cbbox34.SelectedIndexChanged += cbbox34_SelectedIndexChanged;
            txtSub4.TextChanged += txtSub4_TextChanged;
            txtDev4.TextChanged += txtDev4_TextChanged;
            txtbox14.TextChanged += txtbox14_TextChanged;
            txtbox24.TextChanged += txtbox24_TextChanged;
            txtbox34.TextChanged += txtbox34_TextChanged;
            txtSeries4.TextChanged += txtSeries4_TextChanged;
            #endregion
            dgvPumps.Controls.Add(pnl1);
            dgvPumps.Controls.Add(pnl2);
            dgvPumps.Controls.Add(pnl3);
            dgvPumps.Controls.Add(pnl4);
            dgvPumps.Controls.Add(pnl5);
            dgvPumps.Controls.Add(pnl6);
            pnl1.BackColor = dgvPumps.DefaultCellStyle.BackColor;
            pnl2.BackColor = dgvPumps.DefaultCellStyle.BackColor;
            pnl3.BackColor = dgvPumps.DefaultCellStyle.BackColor;
            pnl4.BackColor = dgvPumps.DefaultCellStyle.BackColor;
            pnl5.BackColor = dgvPumps.DefaultCellStyle.BackColor;
            pnl6.BackColor = dgvPumps.DefaultCellStyle.BackColor;
            pnl1.Visible = false;
            pnl2.Visible = false;
            setAllControlVisible(false);
        }

        private void addComboboxItem()
        {
            cbClock.Items.Clear();
            cbClock.Items.Add(CsConst.mstrINIDefault.IniReadValue("public", "00720", ""));
            cbClock.Items.Add(CsConst.mstrINIDefault.IniReadValue("public", "00721", ""));

            for (int i = 1; i <= 2; i++)
            {
                ComboBox cb1 = this.Controls.Find("cbM" + i.ToString(), true)[0] as ComboBox;
                cb1.Items.Clear();
                for (int j = 30; j <= 41; j++)
                    cb1.Items.Add(CsConst.mstrINIDefault.IniReadValue("public", "007" + j.ToString(), ""));
            }

            for (int i = 1; i <= 2; i++)
            {
                ComboBox cb1 = this.Controls.Find("cbD" + i.ToString(), true)[0] as ComboBox;
                cb1.Items.Clear();
                for (int j = 50; j <= 56; j++)
                    cb1.Items.Add(CsConst.mstrINIDefault.IniReadValue("public", "007" + j.ToString(), ""));
            }

            for (int i = 1; i <= 2; i++)
            {
                ComboBox cb1 = this.Controls.Find("cbW" + i.ToString(), true)[0] as ComboBox;
                cb1.Items.Clear();
                for (int j = 60; j <= 64; j++)
                    cb1.Items.Add(CsConst.mstrINIDefault.IniReadValue("public", "007" + j.ToString(), ""));
            }

            for (int i = 1; i <= 2; i++)
            {
                ComboBox cb1 = this.Controls.Find("cbT" + i.ToString(), true)[0] as ComboBox;
                cb1.Items.Clear();
                for (int j = 0; j <= 23; j++)
                    cb1.Items.Add(j.ToString() + ":" + "00");
            }

            cbMode.Items.Clear();
            cbMode.Items.Add(CsConst.mstrINIDefault.IniReadValue("public", "00770", ""));
            cbMode.Items.Add(CsConst.mstrINIDefault.IniReadValue("public", "00771", ""));

            cbHeatSpeed.Items.Clear();
            for (int i = 0; i <= 5; i++)
            {
                cbHeatSpeed.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "0015" + i.ToString(), ""));
            }

            cbMin.Items.Clear();
            cbMin.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00590", ""));
            cbMin.Items.Add("5%");
            cbMin.Items.Add("10%");
            cbMin.Items.Add("15%");
            cbMin.Items.Add("20%");

            cbCurMode.Items.Clear();
            for (int i = 0; i <= 4; i++)
            {
                cbCurMode.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "0007" + i.ToString(), ""));
            }

            cbOutput.Items.Clear();
            cbOutput.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00780", ""));
            cbOutput.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00781", ""));

            for (int i = 1; i <= 6; i++)
            {
                ComboBox cb = this.Controls.Find("cbS" + i.ToString(), true)[0] as ComboBox;
                cb.Items.Clear();
                cb.Items.Add(CsConst.WholeTextsList[1775].sDisplayName);
                for (int j = 1; j <= 6; j++)
                {
                    cb.Items.Add(i.ToString());
                }
            }
        }


        private void dgvSwitch_SizeChanged(object sender, EventArgs e)
        {

        }

        private void chbFlush_CheckedChanged(object sender, EventArgs e)
        {
            chbF1.Enabled = chbFlush.Checked;
            chbF2.Enabled = chbFlush.Checked;
            chbF3.Enabled = chbFlush.Checked;
            numF1H.Enabled = chbFlush.Checked;
            numF1M.Enabled = chbFlush.Checked;
            numF2H.Enabled = chbFlush.Checked;
            numF2M.Enabled = chbFlush.Checked;
            numF3H.Enabled = chbFlush.Checked;
            numF3M.Enabled = chbFlush.Checked;
            txtF1.Enabled = chbFlush.Checked;
            txtF2.Enabled = chbFlush.Checked;
            txtF3.Enabled = chbFlush.Checked;
            chbW1_CheckedChanged(null, null);
        }

        private void chbF1_CheckedChanged(object sender, EventArgs e)
        {
            numF1H.Enabled = (chbFlush.Checked && chbF1.Checked);
            numF1M.Enabled = (chbFlush.Checked && chbF1.Checked);
            txtF1.Enabled = (chbFlush.Checked && chbF1.Checked);
            chbW1_CheckedChanged(null, null);
        }

        private void chbF3_CheckedChanged(object sender, EventArgs e)
        {
            numF3H.Enabled = (chbFlush.Checked && chbF3.Checked);
            numF3M.Enabled = (chbFlush.Checked && chbF3.Checked);
            txtF3.Enabled = (chbFlush.Checked && chbF3.Checked);
            chbW1_CheckedChanged(null, null);
        }

        private void chbProtect_CheckedChanged(object sender, EventArgs e)
        {
            pnlMax.Enabled = chbProtect.Checked;
            cbMode_SelectedIndexChanged(null, null);
        }

        private void btnRef1_Click(object sender, EventArgs e)
        {
            tslRead_Click(tslRead, null);
        }

        private void btnSaveAndClose1_Click(object sender, EventArgs e)
        {
            tslRead_Click(tbUpload, null);
            this.Close();
        }

        private void btnSave1_Click(object sender, EventArgs e)
        {
            tslRead_Click(tbUpload, null);
        }

        private void btnPC_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime d1;
                d1 = DateTime.Now;
                numTime1.Value = Convert.ToDecimal(d1.Hour);
                numTime2.Value = Convert.ToDecimal(d1.Minute);
                numTime3.Value = Convert.ToDecimal(d1.Second);
                DatePicker.Text = d1.ToString();
                DatePicker_ValueChanged(null, null);
            }
            catch
            {
            }
        }

        private void DatePicker_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                DayOfWeek Week = DatePicker.Value.DayOfWeek;
                label5.Text = CsConst.mstrINIDefault.IniReadValue("Public", "0075" + Week.GetHashCode().ToString(), "");
                if (isRead) return;
                if (myFH == null) return;
                if (myFH.arayDateTime == null) myFH.arayDateTime = new byte[10];
                myFH.arayDateTime[0] = Convert.ToByte(DatePicker.Value.Year - 2000);
                myFH.arayDateTime[1] = Convert.ToByte(DatePicker.Value.Month);
                myFH.arayDateTime[2] = Convert.ToByte(DatePicker.Value.Day);
                myFH.arayDateTime[6]=Convert.ToByte(Week.GetHashCode());
            }
            catch
            {
            }
        }

        private void btnReadSysTime_Click(object sender, EventArgs e)
        {
            try
            {
                if (myFH == null) return;
                if (myFH.arayDateTime == null) myFH.arayDateTime = new byte[10];
                Cursor.Current = Cursors.WaitCursor;
                byte[] ArayTmp = new byte[0];
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xD99E, SubNetID, DevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(MyintDeviceType)) == true)
                {
                    Array.Copy(CsConst.myRevBuf, 25, myFH.arayDateTime, 0, 7);

                    int wdYear = Convert.ToInt32(myFH.arayDateTime[0]) + 2000;
                    byte bytMonth = myFH.arayDateTime[1];
                    byte bytDay = myFH.arayDateTime[2];
                    byte bytHour = myFH.arayDateTime[3];
                    byte bytMinute = myFH.arayDateTime[4];
                    byte bytSecond = myFH.arayDateTime[5];

                    if (bytHour > 23) bytHour = 0;
                    if (bytMinute > 59) bytMinute = 0;
                    if (bytSecond > 59) bytSecond = 0;
                    if (bytMonth > 12 || bytMonth < 1) bytMonth = 1;
                    if (bytDay > 31 || bytDay < 1) bytDay = 1;

                    DateTime d = new DateTime(wdYear, Convert.ToInt32(bytMonth), Convert.ToInt32(bytDay));
                    DatePicker.Value = d;
                    DatePicker_ValueChanged(null, null);
                    numTime1.Value = Convert.ToDecimal(bytHour);
                    numTime2.Value = Convert.ToDecimal(bytMinute);
                    numTime3.Value = Convert.ToDecimal(bytSecond);
                    HDLUDP.TimeBetwnNext(1);
                }
                else
                {
                    Cursor.Current = Cursors.Default;
                    return;
                }

                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1C12, SubNetID, DevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(MyintDeviceType)) == true)
                {
                    Array.Copy(CsConst.myRevBuf, 25, myFH.arayDateTime, 7, 2);
                    if (myFH.arayDateTime[7] == 1) chbTime.Checked = true;
                    else chbTime.Checked = false;
                    if (myFH.arayDateTime[8] < cbClock.Items.Count) cbClock.SelectedIndex = myFH.arayDateTime[8];
                    if (cbClock.SelectedIndex < 0) cbClock.SelectedIndex = 0;
                    HDLUDP.TimeBetwnNext(1);
                }

            }
            catch
            {
            }
            Cursor.Current = Cursors.Default;
        }

        private void btnSaveSysTime_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (myFH == null) return;
                if (myFH.arayDateTime == null) return;
                Cursor.Current = Cursors.WaitCursor;
                byte[] ArayTmp = new byte[7];
                Array.Copy(myFH.arayDateTime, 0, ArayTmp, 0, 7);
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xD99C, SubNetID, DevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(MyintDeviceType)) == true)
                {
                    HDLUDP.TimeBetwnNext(20);
                }
                else
                {
                    Cursor.Current = Cursors.Default;
                    return;
                }

                ArayTmp = new byte[2];
                Array.Copy(myFH.arayDateTime, 0, ArayTmp, 2, 2);
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1C10, SubNetID, DevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(MyintDeviceType)) == true)
                {
                    HDLUDP.TimeBetwnNext(20);
                }
            }
            catch
            {
            }
            Cursor.Current = Cursors.Default;
        }

        private void btnOutdoor1_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (CsConst.mySends.AddBufToSndList(null, 0x1D06, SubNetID, DevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(MyintDeviceType)) == true)
                {
                    Array.Copy(CsConst.myRevBuf, 25, myFH.arayOutdoor, 0, 5);
                    HDLUDP.TimeBetwnNext(1);
                    chbOutdoor.Checked = (myFH.arayOutdoor[0] == 1);
                    txtOutdoor.Text = myFH.arayOutdoor[1].ToString();
                    if (myFH.arayOutdoor[3] <= 1) cbOutdoor.Text="F";
                    else cbOutdoor.Text = "F";
                    if (5 <= myFH.arayOutdoor[2] && myFH.arayOutdoor[2] <= 15)
                        sbAdjust.Value = myFH.arayOutdoor[2];
                    else
                        sbAdjust.Value = 10;
                    if (HDLSysPF.GetBit(myFH.arayOutdoor[4], 7) == 1)
                        lb2.Text = "-" + ((myFH.arayOutdoor[4] & (byte.MaxValue - (1 << 7)))).ToString() + cbOutdoor.Text;
                    else
                        lb2.Text = myFH.arayOutdoor[4].ToString() + cbOutdoor.Text;
                    sbAdjust_ValueChanged(null, null);
                }
            }
            catch
            {
            }
            Cursor.Current = Cursors.Default;
        }

        private void btnOutdoor2_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (myFH == null) return;
                if (myFH.arayOutdoor == null) return;
                byte[] arayTmp = new byte[4];
                Array.Copy(myFH.arayOutdoor, 0, arayTmp, 0, 4);
                if (CsConst.mySends.AddBufToSndList(arayTmp, 0x1D04, SubNetID, DevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(MyintDeviceType)) == true)
                {
                    HDLUDP.TimeBetwnNext(20);
                }
            }
            catch
            {
            }
            Cursor.Current = Cursors.Default;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form form = null;
            bool isOpen = true;
            //foreach (Form frm in Application.OpenForms)
            //{
            //    if (frm.Name == "FrmCalibrationLuxOld")
            //    {
            //        if (((frm as FrmCalibrationLuxOld).DIndex == mintIDIndex))
            //        {
            //            form = frm;
            //            form.TopMost = true;
            //            form.WindowState = FormWindowState.Normal;
            //            form.Activate();
            //            form.TopMost = false;
            //            isOpen = false;
            //            break;
            //        }
            //    }
            //}
            //if (isOpen)
            //{
            //    FrmCalibrationLuxOld frmTmp = new FrmCalibrationLuxOld(myDevName, MyintDeviceType, mintIDIndex);
            //    frmTmp.Show();
            //}
        }

        private void txtOutdoor_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar) || e.KeyChar == 8)
                e.Handled = false;
            else
                e.Handled = true;
        }

        private void btnSensor_Click(object sender, EventArgs e)
        {
            Form form = null;
            bool isOpen = true;
            //foreach (Form frm in Application.OpenForms)
            //{
            //    if (frm.Name == "FrmEditFHSensor")
            //    {
            //        if (((frm as FrmEditFHSensor).DIndex == mintIDIndex))
            //        {
            //            form = frm;
            //            form.TopMost = true;
            //            form.WindowState = FormWindowState.Normal;
            //            form.Activate();
            //            form.TopMost = false;
            //            isOpen = false;
            //            break;
            //        }
            //    }
            //}
            //if (isOpen)
            //{
            //    FrmEditFHSensor frmTmp = new FrmEditFHSensor(myDevName, MyintDeviceType, mintIDIndex);
            //    frmTmp.Show();
            //}
        }

        private void sbAdjust_ValueChanged(object sender, EventArgs e)
        {
            lbAdjust.Text = (sbAdjust.Value - 10).ToString() + cbOutdoor.Text;
        }

        private void cbFH_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            try
            {
                if (myFH.MyRead2UpFlags[1] == false)
                {
                    tslRead_Click(tslRead, null);
                }
                else
                {
                    ShowFloorHeatingInfo();
                }
            }
            catch
            { }
        }

        private void rb1_CheckedChanged(object sender, EventArgs e)
        {
            if (rb1.Checked)
            {
                pnlID.Enabled = true;
                chbID1.Enabled = true;
                txtID1.Enabled = true;
                chbID2.Enabled = true;
                txtID2.Enabled = true;
                chbID3.Enabled = true;
                txtID3.Enabled = true;
                chbID4.Enabled = true;
                txtID4.Enabled = true;
                txt1.Enabled = false;
                txt2.Enabled = false;
                cb1.Enabled = false;
            }
            else if (rb2.Checked)
            {
                pnlID.Enabled = false;
                chbID1.Enabled = false;
                txtID1.Enabled = false;
                chbID2.Enabled = false;
                txtID2.Enabled = false;
                chbID3.Enabled = false;
                txtID3.Enabled = false;
                chbID4.Enabled = false;
                txtID4.Enabled = false;
                txt1.Enabled = true;
                txt2.Enabled = true;
                cb1.Enabled = true;
            }
            else if (rb3.Checked)
            {
                pnlID.Enabled = true;
                chbID1.Enabled = true;
                txtID1.Enabled = true;
                chbID2.Enabled = true;
                txtID2.Enabled = true;
                chbID3.Enabled = true;
                txtID3.Enabled = true;
                chbID4.Enabled = true;
                txtID4.Enabled = true;
                txt1.Enabled = true;
                txt2.Enabled = true;
                cb1.Enabled = true;
            }
            cbMode_SelectedIndexChanged(null, null);
        }

        private void rb4_CheckedChanged(object sender, EventArgs e)
        {
            if (rb4.Checked)
            {
                txtID.Enabled = true;
                txt3.Enabled = false;
                txt4.Enabled = false;
                cb2.Enabled = false;
                cbMax.Enabled = false;
            }
            else if (rb5.Checked)
            {
                txtID.Enabled = false;
                txt3.Enabled = true;
                txt4.Enabled = true;
                cb2.Enabled = true;
                cbMax.Enabled = true;
            }
            cbMode_SelectedIndexChanged(null, null);
        }

        private void sb1_ValueChanged(object sender, EventArgs e)
        {
            if (MyintDeviceType == 210)
            {
                lbV1.Text = sb1.Value.ToString() + lbTempType.Text;
            }
            else
            {
                lbV1.Text = sb1.Value.ToString() + cbType.Text;
            }
            chbChn_CheckedChanged(null, null);
        }

        private void sb2_ValueChanged(object sender, EventArgs e)
        {
            if (MyintDeviceType == 210)
            {
                lbV2.Text = sb2.Value.ToString() + lbTempType.Text;
            }
            else
            {
                lbV2.Text = sb2.Value.ToString() + cbType.Text;
            }
            chbChn_CheckedChanged(null, null);
        }

        private void sb3_ValueChanged(object sender, EventArgs e)
        {
            if (MyintDeviceType == 210)
            {
                lbV3.Text = sb3.Value.ToString() + lbTempType.Text;
            }
            else
            {
                lbV3.Text = sb3.Value.ToString() + cbType.Text;
            }
            chbChn_CheckedChanged(null, null);
        }

        private void sb4_ValueChanged(object sender, EventArgs e)
        {
            if (MyintDeviceType == 210)
            {
                lbV4.Text = sb4.Value.ToString() + lbTempType.Text;
            }
            else
            {
                lbV4.Text = sb4.Value.ToString() + cbType.Text;
            }
            chbChn_CheckedChanged(null, null);
        }

        private void sbMax_ValueChanged(object sender, EventArgs e)
        {
            lbMaxVauel.Text = (sbMax.Value - 10).ToString() + lbTempType.Text;
            cbMode_SelectedIndexChanged(null, null);
        }

        void UpdateActivePageIndexWithUnit()
        {
            if (tabControl1.SelectedTab.Name == "tabBasic") MyActivePage = 1;
            else if (tabControl1.SelectedTab.Name == "tabChn") MyActivePage = 2;
            else if (tabControl1.SelectedTab.Name == "tabMaster") MyActivePage = 3;
            else if (tabControl1.SelectedTab.Name == "tabAdvCmd") MyActivePage = 6;
            else if (tabControl1.SelectedTab.Name == "tabPump") MyActivePage = 7;
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateActivePageIndexWithUnit();
            if (myFH.MyRead2UpFlags[MyActivePage - 1] == false)
            {
                tslRead_Click(tslRead, null);
            }
            else
            {
                UpdateDisplayInformationAccordingly(null,null);
            }

        }

        private void showPumpsInfo()
        {
            isRead = true;
            try
            {
                chbLogic.Checked = (myFH.PumpsEnable == 1);
                dgvPumps.Rows.Clear();
                if (myFH.myPumps != null)
                {
                    for (int i = 0; i < myFH.myPumps.Count; i++)
                    {
                        FH.Pumps temp = myFH.myPumps[i];
                        string strHint = "Pump-" + (i + 1).ToString();
                        if (temp.ID == 6) strHint = "Heater";
                        bool bl1 = (HDLSysPF.GetBit(temp.ChooseChns, 0) == 1);
                        bool bl2 = (HDLSysPF.GetBit(temp.ChooseChns, 1) == 1);
                        bool bl3 = (HDLSysPF.GetBit(temp.ChooseChns, 2) == 1);
                        bool bl4 = (HDLSysPF.GetBit(temp.ChooseChns, 3) == 1);
                        bool bl5 = (HDLSysPF.GetBit(temp.ChooseChns, 4) == 1);
                        bool bl6 = (HDLSysPF.GetBit(temp.ChooseChns, 5) == 1);
                        if (temp.ID == 6)
                        {
                            bl1 = true;
                            bl2 = true;
                            bl3 = true;
                            bl4 = true;
                            bl5 = true;
                            bl6 = true;
                        }
                        object[] obj = new object[] { strHint,(temp.Enable==1),bl1,bl2,
                                                    bl3,bl4,
                                                    bl5,bl6,
                                                    temp.TrueDelay.ToString(),
                                                    temp.FalseDelay.ToString()};
                        dgvPumps.Rows.Add(obj);
                    }
                }
                for (int i = 0; i < dgvPumps.RowCount; i++)
                {
                    dgvPumps.Rows[i].Selected = false;
                }
                dgvPumps.Rows[PumpsSelectedIndex].Selected = true;
            }
            catch
            {
            }
            isRead = false;
            dgvPumps_CellClick(dgvPumps, new DataGridViewCellEventArgs(0, PumpsSelectedIndex));
            dgvPumps_CellMouseDoubleClick(dgvPumps, new DataGridViewCellMouseEventArgs(0, PumpsSelectedIndex, 0, 0, new MouseEventArgs(MouseButtons.Left, 2, 0, 0, 0)));
        }

        private void chbHost_CheckedChanged(object sender, EventArgs e)
        {
            pnlHost.Visible = chbHost.Checked;
            cbHost_SelectedIndexChanged(null, null);
        }

        private void chbSlave1_CheckedChanged(object sender, EventArgs e)
        {
            txtSubSlave1.Enabled = chbSlave1.Checked;
            txtDevSlave1.Enabled = chbSlave1.Checked;
            cbSlave1.Enabled = chbSlave1.Checked;
            cbHost_SelectedIndexChanged(null, null);
        }

        private void chbSlave2_CheckedChanged(object sender, EventArgs e)
        {
            txtSubSlave2.Enabled = chbSlave2.Checked;
            txtDevSlave2.Enabled = chbSlave2.Checked;
            cbSlave2.Enabled = chbSlave2.Checked;
            cbHost_SelectedIndexChanged(null, null);
        }

        private void chbSlave3_CheckedChanged(object sender, EventArgs e)
        {
            txtSubSlave3.Enabled = chbSlave3.Checked;
            txtDevSlave3.Enabled = chbSlave3.Checked;
            cbSlave3.Enabled = chbSlave3.Checked;
            cbHost_SelectedIndexChanged(null, null);
        }

        private void chbSlave4_CheckedChanged(object sender, EventArgs e)
        {
            txtSubSlave4.Enabled = chbSlave4.Checked;
            txtDevSlave4.Enabled = chbSlave4.Checked;
            cbSlave4.Enabled = chbSlave4.Checked;
            cbHost_SelectedIndexChanged(null, null);
        }

        private void chbF2_CheckedChanged(object sender, EventArgs e)
        {
            numF2H.Enabled = (chbFlush.Checked && chbF2.Checked);
            numF2M.Enabled = (chbFlush.Checked && chbF2.Checked);
            txtF2.Enabled = (chbFlush.Checked && chbF2.Checked);
            chbW1_CheckedChanged(null, null);
        }

        private void btnReadSwitch_Click(object sender, EventArgs e)
        {
            setAllControlVisible(false);
            MyActivePage = 4;
            tslRead_Click(tslRead, null);
        }

        private void btnReadRelay_Click(object sender, EventArgs e)
        {
            setAllControlVisible(false);
            MyActivePage = 5;
            tslRead_Click(tslRead, null);
        }

        private void cbSwitch_SelectedIndexChanged(object sender, EventArgs e)
        {
            setAllControlVisible(false);
            if (isRead) return;
            if (cbSwitch.SelectedIndex ==-1) return;

            try
            {
            MyActivePage = 4;
            if (myFH.myTargets1 == null || myFH.myTargets1[cbSwitch.SelectedIndex].Targets == null)
                {
                    btnReadSwitch_Click(null, null);
                }
                else
                {
                    ShowTargetsInfo();
                }
            }
            catch
            {}
        }

        private void cbRelay_SelectedIndexChanged(object sender, EventArgs e)
        {
            setAllControlVisible(false);
            if (isRead) return;
            MyActivePage = 5;
            btnReadRelay_Click(null, null);
        }

        private void rbS1_CheckedChanged(object sender, EventArgs e)
        {
            setAllControlVisible(false);
            if (isRead) return;
            ShowTargetsInfo();
        }

        private void rbR1_CheckedChanged(object sender, EventArgs e)
        {
            setAllControlVisible(false);
            if (isRead) return;
            if ((sender as RadioButton).Checked)
            {
                MyActivePage = 5;
                tslRead_Click(tslRead, null);
            }
        }

        private void dgvRelay_SizeChanged(object sender, EventArgs e)
        {

        }

        private void cbClock_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            if (myFH == null) return;
            if (myFH.arayDateTime == null) myFH.arayDateTime = new byte[10];
            myFH.arayDateTime[8] = Convert.ToByte(cbClock.SelectedIndex);
        }

        private void numTime1_ValueChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            if (myFH == null) return;
            if (myFH.arayDateTime == null) myFH.arayDateTime = new byte[10];
            myFH.arayDateTime[3] = Convert.ToByte(numTime1.Value);
            myFH.arayDateTime[4] = Convert.ToByte(numTime2.Value);
            myFH.arayDateTime[5] = Convert.ToByte(numTime3.Value);
        }

        private void chbTime_CheckedChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            if (myFH == null) return;
            if (myFH.arayDateTime == null) myFH.arayDateTime = new byte[10];
            if (chbTime.Checked)
                myFH.arayDateTime[7] = 1;
            else
                myFH.arayDateTime[7] = 0;
        }

        private void chb1_CheckedChanged(object sender, EventArgs e)
        {
            cbM1.Enabled = chb1.Checked;
            cbD1.Enabled = chb1.Checked;
            cbW1.Enabled = chb1.Checked;
            cbT1.Enabled = chb1.Checked;
            if (isRead) return;
            if (myFH == null) return;
            if (myFH.araySummer == null) myFH.araySummer = new byte[30];
            if (chb1.Checked)
                myFH.araySummer[0] = 1;
            else
                myFH.araySummer[0] = 0;
        }

        private void chb2_CheckedChanged(object sender, EventArgs e)
        {
            cbM2.Enabled = chb2.Checked;
            cbD2.Enabled = chb2.Checked;
            cbW2.Enabled = chb2.Checked;
            cbT2.Enabled = chb2.Checked;
            if (isRead) return;
            if (myFH == null) return;
            if (myFH.araySummer == null) myFH.araySummer = new byte[30];
            if (chb2.Checked)
                myFH.araySummer[8] = 1;
            else
                myFH.araySummer[8] = 0;
        }

        private void cbM1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            if (myFH == null) return;
            if (myFH.araySummer == null) myFH.araySummer = new byte[30];
            myFH.araySummer[3] = Convert.ToByte(cbM1.SelectedIndex + 1);
        }

        private void cbD1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            if (myFH == null) return;
            if (myFH.araySummer == null) myFH.araySummer = new byte[30];
            myFH.araySummer[7] = Convert.ToByte(cbD1.SelectedIndex);
        }

        private void cbW1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            if (myFH == null) return;
            if (myFH.araySummer == null) myFH.araySummer = new byte[30];
            myFH.araySummer[6] = Convert.ToByte(cbW1.SelectedIndex);
        }

        private void cbT1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            if (myFH == null) return;
            if (myFH.araySummer == null) myFH.araySummer = new byte[30];
            myFH.araySummer[5] = Convert.ToByte(cbT1.SelectedIndex);
        }

        private void cbM2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            if (myFH == null) return;
            if (myFH.araySummer == null) myFH.araySummer = new byte[30];
            myFH.araySummer[11] = Convert.ToByte(cbM1.SelectedIndex + 1);
        }

        private void cbD2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            if (myFH == null) return;
            if (myFH.araySummer == null) myFH.araySummer = new byte[30];
            myFH.araySummer[15] = Convert.ToByte(cbD2.SelectedIndex);
        }

        private void cbW2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            if (myFH == null) return;
            if (myFH.araySummer == null) myFH.araySummer = new byte[30];
            myFH.araySummer[14] = Convert.ToByte(cbW2.SelectedIndex);
        }

        private void cbT2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            if (myFH == null) return;
            if (myFH.araySummer == null) myFH.araySummer = new byte[30];
            myFH.araySummer[13] = Convert.ToByte(cbT2.SelectedIndex);
        }

        private void cbMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            if (myFH == null) return;
            if (myFH.myHeating == null) return;
            for (int i = 0; i < myFH.myHeating.Count; i++)
            {
                FH.FHeating temp = myFH.myHeating[i];
                if (temp.ID == Convert.ToByte(cbFH.SelectedIndex + 1))
                {
                    if (MyintDeviceType == 210)
                    {
                        if (cbMode.SelectedIndex == 1) temp.araySensorSetting[0] = 1;
                        else temp.araySensorSetting[0] = 0;
                        if (rb1.Checked) temp.araySensorSetting[1] = 0;
                        else if (rb2.Checked) temp.araySensorSetting[1] = 1;
                        else if (rb3.Checked) temp.araySensorSetting[1] = 2;
                        else temp.araySensorSetting[1] = 0;
                        temp.araySensorSetting[2] = Convert.ToByte(txtSensorID.Text);
                        temp.araySensorSetting[3] = Convert.ToByte(txt1.Text);
                        temp.araySensorSetting[4] = Convert.ToByte(txt2.Text);
                        if (cb1.SelectedIndex >= 0) temp.araySensorSetting[5] = Convert.ToByte(cb1.SelectedIndex + 1);
                        else temp.araySensorSetting[5] = 1;
                        if (chbProtect.Checked)
                        {
                            if (rb5.Checked) temp.araySensorSetting[6] = 0x81;
                            else temp.araySensorSetting[6] = 0x80;
                        }
                        else
                        {
                            if (rb5.Checked) temp.araySensorSetting[6] = 1;
                            else temp.araySensorSetting[6] = 0;
                        }
                        temp.araySensorSetting[7] = Convert.ToByte(txtID.Text);
                        temp.araySensorSetting[8] = Convert.ToByte(txt3.Text);
                        temp.araySensorSetting[9] = Convert.ToByte(txt4.Text);
                        temp.araySensorSetting[10] = Convert.ToByte(cb2.SelectedIndex + 1);
                        string strMax = cbMax.Text;
                        strMax = strMax.Replace("C", "");
                        strMax = strMax.Replace("F", "");
                        strMax = strMax.Replace("<=", "");
                        strMax = strMax.Replace(">=", "");
                        temp.araySensorSetting[11] = Convert.ToByte(strMax);
                        if (chbTemp.Checked) temp.araySensorSetting[11] = Convert.ToByte(0x128 | temp.araySensorSetting[11]);
                        else temp.araySensorSetting[11] = Convert.ToByte(strMax);
                        temp.araySensorSetting[12] = Convert.ToByte(sbMax.Value);
                    }
                    else if (MyintDeviceType == 211 || MyintDeviceType == 208 || MyintDeviceType == 209)
                    {
                        if (cbMode.SelectedIndex == 1) temp.araySensorSetting[0] = 1;
                        else temp.araySensorSetting[0] = 0;
                        if (rb1.Checked) temp.araySensorSetting[1] = 0;
                        else if (rb2.Checked) temp.araySensorSetting[1] = 1;
                        else if (rb3.Checked) temp.araySensorSetting[1] = 2;
                        else temp.araySensorSetting[1] = 0;
                        temp.araySensorSetting[3] = Convert.ToByte(txt1.Text);
                        temp.araySensorSetting[4] = Convert.ToByte(txt2.Text);
                        if (cb1.SelectedIndex >= 0) temp.araySensorSetting[5] = Convert.ToByte(cb1.SelectedIndex + 1);
                        if (chbProtect.Checked)
                        {
                            if (rb5.Checked) temp.araySensorSetting[6] = 0x81;
                            else temp.araySensorSetting[6] = 0x80;
                        }
                        else
                        {
                            if (rb5.Checked) temp.araySensorSetting[6] = 1;
                            else temp.araySensorSetting[6] = 0;
                        }
                        temp.araySensorSetting[7] = Convert.ToByte(txtID.Text);
                        temp.araySensorSetting[8] = Convert.ToByte(txt3.Text);
                        temp.araySensorSetting[9] = Convert.ToByte(txt4.Text);
                        temp.araySensorSetting[10] = Convert.ToByte(cb2.SelectedIndex + 1);
                        string strMax = cbMax.Text;
                        strMax = strMax.Replace("C", "");
                        strMax = strMax.Replace("F", "");
                        strMax = strMax.Replace("<=", "");
                        strMax = strMax.Replace(">=", "");
                        temp.araySensorSetting[11] = Convert.ToByte(strMax);
                        if (chbTemp.Checked) temp.araySensorSetting[11] = Convert.ToByte(0x128 | temp.araySensorSetting[11]);
                        else temp.araySensorSetting[11] = Convert.ToByte(strMax);
                        temp.araySensorSetting[12] = 0;
                        if (chbID1.Checked) temp.araySensorSetting[12] = Convert.ToByte(temp.araySensorSetting[12] | 1);
                        if (chbID2.Checked) temp.araySensorSetting[12] = Convert.ToByte(temp.araySensorSetting[12] | 2);
                        if (chbID3.Checked) temp.araySensorSetting[12] = Convert.ToByte(temp.araySensorSetting[12] | 4);
                        if (chbID4.Checked) temp.araySensorSetting[12] = Convert.ToByte(temp.araySensorSetting[12] | 8);
                        temp.araySensorSetting[13] = Convert.ToByte(txtID1.Text);
                        temp.araySensorSetting[14] = Convert.ToByte(txtID2.Text);
                        temp.araySensorSetting[15] = Convert.ToByte(txtID3.Text);
                        temp.araySensorSetting[16] = Convert.ToByte(txtID4.Text);
                    }
                    break;
                }
            }
        }

        private void btnChannel_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                byte[] ArayTmp = new byte[21];
                ArayTmp[0] = Convert.ToByte(cbFH.SelectedIndex + 1);
                byte[] arayTmpRemark = HDLUDP.StringTo2Byte(txtChannel.Text, true);
                if (arayTmpRemark.Length > 20)
                {
                    Array.Copy(arayTmpRemark, 0, ArayTmp, 1, 20);
                }
                else
                {
                    Array.Copy(arayTmpRemark, 0, ArayTmp, 1, arayTmpRemark.Length);
                }
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1D00, SubNetID, DevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(MyintDeviceType)) == true)
                {
                    myFH.myHeating[cbFH.SelectedIndex].strRemark = txtChannel.Text;
                    HDLUDP.TimeBetwnNext(20);
                }
            }
            catch
            {
            }
            Cursor.Current = Cursors.Default;
        }

        private void txtChannel_TextChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            if (myFH == null) return;
            if (myFH.myHeating == null) return;
            for (int i = 0; i < myFH.myHeating.Count; i++)
            {
                FH.FHeating temp = myFH.myHeating[i];
                if (temp.ID == Convert.ToByte(cbFH.SelectedIndex + 1))
                {
                    temp.strRemark = txtChannel.Text;
                    break;
                }
            }
        }

        private void chbChn_CheckedChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            if (myFH == null) return;
            if (myFH.myHeating == null) return;
            for (int i = 0; i < myFH.myHeating.Count; i++)
            {
                FH.FHeating temp = myFH.myHeating[i];
                if (temp.ID == Convert.ToByte(cbFH.SelectedIndex + 1))
                {
                    if (MyintDeviceType == 210)
                    {
                        if (chbChn.Checked)
                        {
                            if (rbCool.Checked) temp.arayWorkControl[0] = 0x11;
                            else temp.arayWorkControl[0] = 1;
                        }
                        else
                        {
                            if (rbCool.Checked) temp.arayWorkControl[0] = 0x10;
                            else temp.arayWorkControl[0] = 0;
                        }
                        if (lbTempType.Text == "F") temp.arayWorkControl[1] = 1;
                        else temp.arayWorkControl[1] = 0;
                        if (cbCurMode.SelectedIndex >= 0)
                            temp.arayWorkControl[2] = Convert.ToByte(cbCurMode.SelectedIndex + 1);
                        else temp.arayWorkControl[2] = 1;
                        temp.arayWorkControl[3] = Convert.ToByte(sb1.Value);
                        temp.arayWorkControl[4] = Convert.ToByte(sb2.Value);
                        temp.arayWorkControl[5] = Convert.ToByte(sb3.Value);
                        temp.arayWorkControl[6] = Convert.ToByte(sb4.Value);
                    }
                    else if (MyintDeviceType == 211 || MyintDeviceType == 208 || MyintDeviceType == 209)
                    {
                        temp.arayWorkControl[0] = 0;
                        if (chbChn.Checked) temp.arayWorkControl[0] = 1;
                        if (rbCool.Checked) temp.arayWorkControl[0] = Convert.ToByte(temp.arayWorkControl[0] | (1 << 4));
                        if (cbOutput.SelectedIndex >= 0) temp.arayWorkControl[0] = Convert.ToByte(temp.arayWorkControl[0] | (1 << 5));
                        if (cbType.Text == "F") temp.arayWorkControl[1] = 1;
                        else temp.arayWorkControl[1] = 0;
                        if (cbCurMode.SelectedIndex >= 0) temp.arayWorkControl[2] = Convert.ToByte(cbCurMode.SelectedIndex + 1);
                        else temp.arayWorkControl[2] = 1;
                        temp.arayWorkControl[3] = Convert.ToByte(sb1.Value);
                        temp.arayWorkControl[4] = Convert.ToByte(sb2.Value);
                        temp.arayWorkControl[5] = Convert.ToByte(sb3.Value);
                        temp.arayWorkControl[6] = Convert.ToByte(sb4.Value);
                    }
                    break;
                }
            }
        }

        private void txt1_Leave(object sender, EventArgs e)
        {
            string str = (sender as TextBox).Text;
            (sender as TextBox).Text = HDLPF.IsNumStringMode(str, 0, 255);
            (sender as TextBox).SelectionStart = (sender as TextBox).Text.Length;
            cbMode_SelectedIndexChanged(null, null);
        }

        private void chbW1_CheckedChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            if (myFH == null) return;
            if (myFH.myHeating == null) return;
            for (int i = 0; i < myFH.myHeating.Count; i++)
            {
                FH.FHeating temp = myFH.myHeating[i];
                if (temp.ID == Convert.ToByte(cbFH.SelectedIndex + 1))
                {
                    if (chbW1.Checked) temp.arayWorkSetting[0] = 1;
                    else temp.arayWorkSetting[0] = 0;
                    if (cbMin.SelectedIndex >= 0) temp.arayWorkSetting[1] = Convert.ToByte(cbMin.SelectedIndex);
                    else temp.arayWorkSetting[1] = 0;
                    if (cbHeatSpeed.SelectedIndex >= 0) temp.arayWorkSetting[2] = Convert.ToByte(cbHeatSpeed.SelectedIndex);
                    else temp.arayWorkSetting[2] = 2;
                    if (cbCycle.SelectedIndex >= 0) temp.arayWorkSetting[3] = Convert.ToByte(cbCycle.SelectedIndex);
                    else temp.arayWorkSetting[3] = 3;
                    if (chbW2.Checked) temp.arayWorkSetting[5] = 1;
                    else temp.arayWorkSetting[5] = 0;
                    temp.arayWorkSetting[6] = Convert.ToByte(numStart1.Value);
                    temp.arayWorkSetting[7] = Convert.ToByte(numStart2.Value);
                    temp.arayWorkSetting[8] = Convert.ToByte(numEnd1.Value);
                    temp.arayWorkSetting[9] = Convert.ToByte(numEnd2.Value);
                    temp.arayWorkSetting[10] = 0;
                    if (chbW3.Checked) temp.arayWorkSetting[10] = Convert.ToByte(temp.arayWorkSetting[10] | (1 << 0));
                    if (chbW4.Checked) temp.arayWorkSetting[10] = Convert.ToByte(temp.arayWorkSetting[10] | (1 << 1));
                    if (chbW5.Checked) temp.arayWorkSetting[10] = Convert.ToByte(temp.arayWorkSetting[10] | (1 << 2));

                    if (chbFlush.Checked) temp.arayFlush[0] = 1;
                    else temp.arayFlush[0] = 0;
                    temp.arayFlush[1] = 0;
                    if (chbF1.Checked) temp.arayFlush[1] = Convert.ToByte(temp.arayFlush[1] | 1);
                    if (chbF2.Checked) temp.arayFlush[1] = Convert.ToByte(temp.arayFlush[1] | 2);
                    if (chbF3.Checked) temp.arayFlush[1] = Convert.ToByte(temp.arayFlush[1] | 4);
                    temp.arayFlush[2] = Convert.ToByte(numF1H.Value);
                    temp.arayFlush[3] = Convert.ToByte(numF1M.Value);
                    temp.arayFlush[4] = Convert.ToByte(txtF1.Text);
                    temp.arayFlush[5] = Convert.ToByte(numF2H.Value);
                    temp.arayFlush[6] = Convert.ToByte(numF2M.Value);
                    temp.arayFlush[7] = Convert.ToByte(txtF2.Text);
                    temp.arayFlush[8] = Convert.ToByte(numF3H.Value);
                    temp.arayFlush[9] = Convert.ToByte(numF3M.Value);
                    temp.arayFlush[10] = Convert.ToByte(txtF3.Text);
                    break;
                }
            }
        }

        private void txtF1_Leave(object sender, EventArgs e)
        {
            string str = (sender as TextBox).Text;
            (sender as TextBox).Text = HDLPF.IsNumStringMode(str, 1, 30);
            (sender as TextBox).SelectionStart = (sender as TextBox).Text.Length;
            chbW1_CheckedChanged(null, null);
        }

        private void cbS1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            if (myFH == null) return;
            if (myFH.araySynChannel == null) myFH.araySynChannel = new byte[10];
            int Tag = Convert.ToInt32((sender as ComboBox).Tag);
            myFH.araySynChannel[Tag] = Convert.ToByte((sender as ComboBox).SelectedIndex);
        }

        private void cbHost_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            if (myFH == null) return;
            if (myFH.arayHost == null) myFH.arayHost = new byte[20];
            if (chbHost.Checked) myFH.arayHost[0] = 1;
            else myFH.arayHost[0] = 0;
            if (cbHost.SelectedIndex >= 0)
                myFH.arayHost[1] = Convert.ToByte(cbHost.SelectedIndex + 1);
            else
                myFH.arayHost[1] = 1;
            myFH.arayHost[2] = 0;
            if (chbSlave1.Checked) myFH.arayHost[2] = Convert.ToByte(myFH.arayHost[2] | 1);
            if (chbSlave2.Checked) myFH.arayHost[2] = Convert.ToByte(myFH.arayHost[2] | 2);
            if (chbSlave3.Checked) myFH.arayHost[2] = Convert.ToByte(myFH.arayHost[2] | 4);
            if (chbSlave4.Checked) myFH.arayHost[2] = Convert.ToByte(myFH.arayHost[2] | 8);
            myFH.arayHost[3] = Convert.ToByte(txtSubSlave1.Text);
            myFH.arayHost[4] = Convert.ToByte(txtDevSlave1.Text);
            if (cbSlave1.SelectedIndex >= 0) myFH.arayHost[5] = Convert.ToByte(cbSlave1.SelectedIndex + 1);
            else myFH.arayHost[5] = 1;
            myFH.arayHost[6] = Convert.ToByte(txtSubSlave2.Text);
            myFH.arayHost[7] = Convert.ToByte(txtDevSlave2.Text);
            if (cbSlave2.SelectedIndex >= 0) myFH.arayHost[8] = Convert.ToByte(cbSlave2.SelectedIndex + 1);
            else myFH.arayHost[8] = 1;
            myFH.arayHost[9] = Convert.ToByte(txtSubSlave3.Text);
            myFH.arayHost[10] = Convert.ToByte(txtDevSlave3.Text);
            if (cbSlave3.SelectedIndex >= 0) myFH.arayHost[11] = Convert.ToByte(cbSlave3.SelectedIndex + 1);
            else myFH.arayHost[11] = 1;
            myFH.arayHost[12] = Convert.ToByte(txtSubSlave4.Text);
            myFH.arayHost[13] = Convert.ToByte(txtDevSlave4.Text);
            if (cbSlave4.SelectedIndex >= 0) myFH.arayHost[14] = Convert.ToByte(cbSlave4.SelectedIndex + 1);
            else myFH.arayHost[14] = 1;
        }

        private void txtSubSlave1_Leave(object sender, EventArgs e)
        {
            string str = (sender as TextBox).Text;
            (sender as TextBox).Text = HDLPF.IsNumStringMode(str, 0, 255);
            (sender as TextBox).SelectionStart = (sender as TextBox).Text.Length;
            cbHost_SelectedIndexChanged(null, null);
        }

     

        private void setAllControlVisible(bool TF)
        {
            txtSub.Visible = TF;
            txtDev.Visible = TF;
            cbControlType.Visible = TF;
            cbbox1.Visible = TF;
            cbbox2.Visible = TF;
            cbbox3.Visible = TF;
            txtbox1.Visible = TF;
            txtbox2.Visible = TF;
            txtbox3.Visible = TF;
            txtSeries.Visible = TF;
            cbPanleControl.Visible = TF;
            cbAudioControl.Visible = TF;

            txtSub2.Visible = TF;
            txtDev2.Visible = TF;
            cbControlType2.Visible = TF;
            cbbox12.Visible = TF;
            cbbox22.Visible = TF;
            cbbox32.Visible = TF;
            txtbox12.Visible = TF;
            txtbox22.Visible = TF;
            txtbox32.Visible = TF;
            txtSeries2.Visible = TF;
            cbPanleControl2.Visible = TF;
            cbAudioControl2.Visible = TF;

            txtSub3.Visible = TF;
            txtDev3.Visible = TF;
            cbControlType3.Visible = TF;
            cbbox13.Visible = TF;
            cbbox23.Visible = TF;
            cbbox33.Visible = TF;
            txtbox13.Visible = TF;
            txtbox23.Visible = TF;
            txtbox33.Visible = TF;
            txtSeries3.Visible = TF;
            cbPanleControl3.Visible = TF;
            cbAudioControl3.Visible = TF;

            txtSub4.Visible = TF;
            txtDev4.Visible = TF;
            cbControlType4.Visible = TF;
            cbbox14.Visible = TF;
            cbbox24.Visible = TF;
            cbbox34.Visible = TF;
            txtbox14.Visible = TF;
            txtbox24.Visible = TF;
            txtbox34.Visible = TF;
            txtSeries4.Visible = TF;
            cbPanleControl4.Visible = TF;
            cbAudioControl4.Visible = TF;

            pnl1.Visible = TF;
            pnl2.Visible = TF;
            pnl3.Visible = TF;
            pnl4.Visible = TF;
            pnl5.Visible = TF;
            pnl6.Visible = TF;
        }

        void txtbox3_TextChanged(object sender, EventArgs e)
        {
            if (dgvSwitch.CurrentRow.Index < 0) return;
            if (dgvSwitch.RowCount <= 0) return;
            int index = dgvSwitch.CurrentRow.Index;
            string str = txtbox3.Text;
            if (str.Contains("(")) str = str.Split('(')[0].ToString();
            #region
            if (cbAudioControl.Visible)
            {
                if (cbAudioControl.SelectedIndex == 2)//列表/频道
                {
                    if (cbbox2.Visible && cbbox2.Items.Count >= 6)
                    {
                        if (txtbox3.Text.Length > 0)
                        {
                            if (cbbox2.SelectedIndex == 2)//列表号
                            {
                                txtbox3.Text = HDLPF.IsNumStringMode(str, 1, 255);
                                dgvSwitch[6, index].Value = txtbox3.Text + "(" + CsConst.mstrINIDefault.IniReadValue("public", "00046", "") + ")";
                            }
                            else if (cbbox2.SelectedIndex == 5)//频道号
                            {
                                txtbox3.Text = HDLPF.IsNumStringMode(str, 1, 50);
                                dgvSwitch[6, index].Value = txtbox3.Text + "(" + CsConst.mstrINIDefault.IniReadValue("public", "99873", "") + ")";
                            }
                            txtbox3.SelectionStart = txtbox3.Text.Length;
                        }
                    }
                }
                else if (cbAudioControl.SelectedIndex >= 5)
                {
                    if (txtbox3.Text.Length > 0)
                    {
                        txtbox3.Text = HDLPF.IsNumStringMode(str, 1, 999);
                        dgvSwitch[6, index].Value = txtbox3.Text + "(" + CsConst.mstrINIDefault.IniReadValue("public", "00043", "") + ")";
                        txtbox3.SelectionStart = txtbox3.Text.Length;
                    }
                }
            }
            #endregion
            if (dgvSwitch.SelectedRows == null || dgvSwitch.SelectedRows.Count == 0) return;
            string strTmp = dgvSwitch[6, index].Value.ToString();
            if (strTmp == null) strTmp = "N/A";
            if (txtbox3.Focused)
                HDLSysPF.ModifyMultilinesIfNeeds(strTmp, 6, dgvSwitch);
        }

        void txtbox32_TextChanged(object sender, EventArgs e)
        {
            if (dgvRelay.CurrentRow.Index < 0) return;
            if (dgvRelay.RowCount <= 0) return;
            int index = dgvRelay.CurrentRow.Index;
            string str = txtbox32.Text;
            if (str.Contains("(")) str = str.Split('(')[0].ToString();
            #region
            if (cbAudioControl2.Visible)
            {
                if (cbAudioControl2.SelectedIndex == 2)//列表/频道
                {
                    if (cbbox22.Visible && cbbox22.Items.Count >= 6)
                    {
                        if (txtbox32.Text.Length > 0)
                        {
                            if (cbbox22.SelectedIndex == 2)//列表号
                            {
                                txtbox32.Text = HDLPF.IsNumStringMode(str, 1, 255);
                                dgvRelay[6, index].Value = txtbox32.Text + "(" + CsConst.mstrINIDefault.IniReadValue("public", "00046", "") + ")";
                            }
                            else if (cbbox22.SelectedIndex == 5)//频道号
                            {
                                txtbox32.Text = HDLPF.IsNumStringMode(str, 1, 50);
                                dgvRelay[6, index].Value = txtbox32.Text + "(" + CsConst.mstrINIDefault.IniReadValue("public", "99873", "") + ")";
                            }
                            txtbox32.SelectionStart = txtbox32.Text.Length;
                        }
                    }
                }
                else if (cbAudioControl2.SelectedIndex >= 5)
                {
                    if (txtbox32.Text.Length > 0)
                    {
                        txtbox32.Text = HDLPF.IsNumStringMode(str, 1, 999);
                        dgvRelay[6, index].Value = txtbox32.Text + "(" + CsConst.mstrINIDefault.IniReadValue("public", "00043", "") + ")";
                        txtbox32.SelectionStart = txtbox32.Text.Length;
                    }
                }
            }
            #endregion
            if (dgvRelay.SelectedRows == null || dgvRelay.SelectedRows.Count == 0) return;
            string strTmp = dgvRelay[6, index].Value.ToString();
            if (strTmp == null) strTmp = "N/A";
            if (txtbox32.Focused)
                HDLSysPF.ModifyMultilinesIfNeeds(strTmp, 6, dgvRelay);
        }

        void txtbox33_TextChanged(object sender, EventArgs e)
        {
            if (dgvP1.CurrentRow.Index < 0) return;
            if (dgvP1.RowCount <= 0) return;
            int index = dgvP1.CurrentRow.Index;
            string str = txtbox33.Text;
            if (str.Contains("(")) str = str.Split('(')[0].ToString();
            #region
            if (cbAudioControl3.Visible)
            {
                if (cbAudioControl3.SelectedIndex == 2)//列表/频道
                {
                    if (cbbox23.Visible && cbbox23.Items.Count >= 6)
                    {
                        if (txtbox33.Text.Length > 0)
                        {
                            if (cbbox23.SelectedIndex == 2)//列表号
                            {
                                txtbox33.Text = HDLPF.IsNumStringMode(str, 1, 255);
                                dgvP1[6, index].Value = txtbox33.Text + "(" + CsConst.mstrINIDefault.IniReadValue("public", "00046", "") + ")";
                            }
                            else if (cbbox23.SelectedIndex == 5)//频道号
                            {
                                txtbox33.Text = HDLPF.IsNumStringMode(str, 1, 50);
                                dgvP1[6, index].Value = txtbox33.Text + "(" + CsConst.mstrINIDefault.IniReadValue("public", "99873", "") + ")";
                            }
                            txtbox33.SelectionStart = txtbox33.Text.Length;
                        }
                    }
                }
                else if (cbAudioControl3.SelectedIndex >= 5)
                {
                    if (txtbox33.Text.Length > 0)
                    {
                        txtbox33.Text = HDLPF.IsNumStringMode(str, 1, 999);
                        dgvP1[6, index].Value = txtbox33.Text + "(" + CsConst.mstrINIDefault.IniReadValue("public", "00043", "") + ")";
                        txtbox33.SelectionStart = txtbox33.Text.Length;
                    }
                }
            }
            #endregion
            if (dgvP1.SelectedRows == null || dgvP1.SelectedRows.Count == 0) return;
            string strTmp = dgvP1[6, index].Value.ToString();
            if (strTmp == null) strTmp = "N/A";
            if (txtbox33.Focused)
                HDLSysPF.ModifyMultilinesIfNeeds(strTmp, 6, dgvP1);
        }

        void txtbox34_TextChanged(object sender, EventArgs e)
        {
            if (dgvP2.CurrentRow.Index < 0) return;
            if (dgvP2.RowCount <= 0) return;
            int index = dgvP2.CurrentRow.Index;
            string str = txtbox34.Text;
            if (str.Contains("(")) str = str.Split('(')[0].ToString();
            #region
            if (cbAudioControl4.Visible)
            {
                if (cbAudioControl4.SelectedIndex == 2)//列表/频道
                {
                    if (cbbox24.Visible && cbbox24.Items.Count >= 6)
                    {
                        if (txtbox34.Text.Length > 0)
                        {
                            if (cbbox24.SelectedIndex == 2)//列表号
                            {
                                txtbox34.Text = HDLPF.IsNumStringMode(str, 1, 255);
                                dgvP2[6, index].Value = txtbox34.Text + "(" + CsConst.mstrINIDefault.IniReadValue("public", "00046", "") + ")";
                            }
                            else if (cbbox24.SelectedIndex == 5)//频道号
                            {
                                txtbox34.Text = HDLPF.IsNumStringMode(str, 1, 50);
                                dgvP2[6, index].Value = txtbox34.Text + "(" + CsConst.mstrINIDefault.IniReadValue("public", "99873", "") + ")";
                            }
                            txtbox34.SelectionStart = txtbox34.Text.Length;
                        }
                    }
                }
                else if (cbAudioControl4.SelectedIndex >= 5)
                {
                    if (txtbox34.Text.Length > 0)
                    {
                        txtbox34.Text = HDLPF.IsNumStringMode(str, 1, 999);
                        dgvP2[6, index].Value = txtbox34.Text + "(" + CsConst.mstrINIDefault.IniReadValue("public", "00043", "") + ")";
                        txtbox34.SelectionStart = txtbox34.Text.Length;
                    }
                }
            }
            #endregion
            if (dgvP2.SelectedRows == null || dgvP2.SelectedRows.Count == 0) return;
            string strTmp = dgvP2[6, index].Value.ToString();
            if (strTmp == null) strTmp = "N/A";
            if (txtbox34.Focused)
                HDLSysPF.ModifyMultilinesIfNeeds(strTmp, 6, dgvP2);
        }

        void txtSeries_TextChanged(object sender, EventArgs e)
        {
            if (dgvSwitch.CurrentRow.Index < 0) return;
            if (dgvSwitch.RowCount <= 0) return;
            int index = dgvSwitch.CurrentRow.Index;
            string str = HDLPF.GetStringFromTime(Convert.ToInt32(txtSeries.Text), ":");
            if (txtSeries.Visible)
            {
                dgvSwitch[6, index].Value = str + "(" + CsConst.WholeTextsList[2525].sDisplayName + ")";
                if (dgvSwitch.SelectedRows == null || dgvSwitch.SelectedRows.Count == 0) return;
                string strTmp = dgvSwitch[6, index].Value.ToString();
                if (strTmp == null) strTmp = "N/A";
                //if (txtSeries.minute.Focused || txtSeries.sencond.Focused)
                //    HDLSysPF.ModifyMultilinesIfNeeds(strTmp, 6, dgvSwitch);
            }
        }

        void txtSeries2_TextChanged(object sender, EventArgs e)
        {
            if (dgvRelay.CurrentRow.Index < 0) return;
            if (dgvRelay.RowCount <= 0) return;
            int index = dgvRelay.CurrentRow.Index;
            string str = HDLPF.GetStringFromTime(Convert.ToInt32(txtSeries2.Text), ":");
            if (txtSeries2.Visible)
            {
                dgvRelay[6, index].Value = str + "(" + CsConst.WholeTextsList[2525].sDisplayName + ")";
                if (dgvRelay.SelectedRows == null || dgvRelay.SelectedRows.Count == 0) return;
                string strTmp = dgvRelay[6, index].Value.ToString();
                if (strTmp == null) strTmp = "N/A";
                //if (txtSeries2.minute.Focused || txtSeries2.sencond.Focused)
                //    HDLSysPF.ModifyMultilinesIfNeeds(strTmp, 6, dgvRelay);
            }
        }

        void txtSeries3_TextChanged(object sender, EventArgs e)
        {
            if (dgvP1.CurrentRow.Index < 0) return;
            if (dgvP1.RowCount <= 0) return;
            int index = dgvP1.CurrentRow.Index;
            string str = HDLPF.GetStringFromTime(Convert.ToInt32(txtSeries3.Text), ":");
            if (txtSeries3.Visible)
            {
                dgvP1[6, index].Value = str + "(" + CsConst.WholeTextsList[2525].sDisplayName + ")";
                if (dgvP1.SelectedRows == null || dgvP1.SelectedRows.Count == 0) return;
                string strTmp = dgvP1[6, index].Value.ToString();
                if (strTmp == null) strTmp = "N/A";
                if (txtSeries3.minute.Focused || txtSeries3.sencond.Focused)
                    HDLSysPF.ModifyMultilinesIfNeeds(strTmp, 6, dgvP1);
            }
        }

        void txtSeries4_TextChanged(object sender, EventArgs e)
        {
            if (dgvP2.CurrentRow.Index < 0) return;
            if (dgvP2.RowCount <= 0) return;
            int index = dgvP2.CurrentRow.Index;
            string str = HDLPF.GetStringFromTime(Convert.ToInt32(txtSeries4.Text), ":");
            if (txtSeries4.Visible)
            {
                dgvP2[6, index].Value = str + "(" + CsConst.WholeTextsList[2525].sDisplayName + ")";
                if (dgvP2.SelectedRows == null || dgvP2.SelectedRows.Count == 0) return;
                string strTmp = dgvP2[6, index].Value.ToString();
                if (strTmp == null) strTmp = "N/A";
                if (txtSeries4.minute.Focused || txtSeries4.sencond.Focused)
                    HDLSysPF.ModifyMultilinesIfNeeds(strTmp, 6, dgvP2);
            }
        }

        void txtbox2_TextChanged(object sender, EventArgs e)
        {
            if (dgvSwitch.CurrentRow.Index < 0) return;
            if (dgvSwitch.RowCount <= 0) return;
            int index = dgvSwitch.CurrentRow.Index;
            string str = txtbox2.Text;
            if (str.Contains("(")) str = str.Split('(')[0].ToString();
            #region
            if (txtbox2.Text.Length > 0)
            {
                if (cbControlType.Text == CsConst.myPublicControlType[1].ControlTypeName || //场景
                    cbControlType.Text == CsConst.myPublicControlType[10].ControlTypeName)   //广播场景
                {
                    txtbox2.Text = HDLPF.IsNumStringMode(str, 0, 255);
                    dgvSwitch[5, index].Value = txtbox2.Text + "(" + CsConst.WholeTextsList[2511].sDisplayName + ")";
                }
                else if (cbControlType.Text == CsConst.myPublicControlType[2].ControlTypeName)//序列
                {
                    txtbox2.Text = HDLPF.IsNumStringMode(str, 0, 99);
                    dgvSwitch[5, index].Value = txtbox2.Text + "(" + CsConst.WholeTextsList[2512].sDisplayName+ ")";
                }
                else if (cbControlType.Text == CsConst.myPublicControlType[5].ControlTypeName || //单路调节 
                         cbControlType.Text == CsConst.myPublicControlType[11].ControlTypeName)   //广播回路
                {
                    txtbox2.Text = HDLPF.IsNumStringMode(str, 0, 100);
                    dgvSwitch[5, index].Value = txtbox2.Text + "(" + CsConst.mstrINIDefault.IniReadValue("public", "00011", "") + ")";
                }
                else if (cbControlType.Text == CsConst.myPublicControlType[7].ControlTypeName)//GPRS控制
                {
                    txtbox2.Text = HDLPF.IsNumStringMode(str, 1, 99);
                    dgvSwitch[5, index].Value = txtbox2.Text + "(" + CsConst.mstrINIDefault.IniReadValue("public", "99864", "") + ")";
                }
                else if (cbControlType.Text == CsConst.myPublicControlType[8].ControlTypeName)//面板控制
                {
                    if (cbPanleControl.Text == CsConst.myPublicControlType[3].ControlTypeName ||//背光亮度
                        cbPanleControl.Text == CsConst.myPublicControlType[4].ControlTypeName) //状态灯亮度
                    {
                        txtbox2.Text = HDLPF.IsNumStringMode(str, 0, 100);
                        dgvSwitch[5, index].Value = txtbox2.Text + "(" + CsConst.WholeTextsList[2524].sDisplayName + ")";
                    }
                    else if (cbPanleControl.Text ==  CsConst.PanelControl[15] ||//空调升高温度 
                             cbPanleControl.Text ==  CsConst.PanelControl[16] ||//空调降低温度
                             cbPanleControl.Text == CsConst.PanelControl[23] ||//地热身高温度
                             cbPanleControl.Text == CsConst.PanelControl[24]) //地热降低温度
                    {
                        txtbox2.Text = HDLPF.IsNumStringMode(str, 1, 255);
                        dgvSwitch[5, index].Value = txtbox2.Text;

                    }
                }
                else if (cbControlType.Text == CsConst.myPublicControlType[13].ControlTypeName)//音乐播放
                {
                    if (cbAudioControl.SelectedIndex == 5)//歌曲播放
                    {
                        txtbox2.Text = HDLPF.IsNumStringMode(str, 0, 255);
                        dgvSwitch[5, index].Value = txtbox2.Text + "(" + CsConst.mstrINIDefault.IniReadValue("public", "00046", "") + ")";
                    }
                }
                txtbox2.SelectionStart = txtbox2.Text.Length;
            }
            #endregion
            if (dgvSwitch.SelectedRows == null || dgvSwitch.SelectedRows.Count == 0) return;
            string strTmp = dgvSwitch[5, index].Value.ToString();
            if (strTmp == null) strTmp = "N/A";
            if (txtbox2.Focused)
                HDLSysPF.ModifyMultilinesIfNeeds(strTmp, 5, dgvSwitch);
        }

        void txtbox22_TextChanged(object sender, EventArgs e)
        {
            if (dgvRelay.CurrentRow.Index < 0) return;
            if (dgvRelay.RowCount <= 0) return;
            int index = dgvRelay.CurrentRow.Index;
            string str = txtbox22.Text;
            if (str.Contains("(")) str = str.Split('(')[0].ToString();
            #region
            if (txtbox22.Text.Length > 0)
            {
                if (cbControlType2.Text == CsConst.myPublicControlType[1].ControlTypeName || //场景
                    cbControlType2.Text == CsConst.myPublicControlType[10].ControlTypeName)   //广播场景
                {
                    txtbox22.Text = HDLPF.IsNumStringMode(str, 0, 255);
                    dgvRelay[5, index].Value = txtbox22.Text + "(" + CsConst.WholeTextsList[2511].sDisplayName + ")";
                }
                else if (cbControlType2.Text == CsConst.myPublicControlType[2].ControlTypeName)//序列
                {
                    txtbox22.Text = HDLPF.IsNumStringMode(str, 0, 99);
                    dgvRelay[5, index].Value = txtbox22.Text + "(" + CsConst.WholeTextsList[2512].sDisplayName+ ")";
                }
                else if (cbControlType2.Text == CsConst.myPublicControlType[5].ControlTypeName || //单路调节 
                         cbControlType2.Text == CsConst.myPublicControlType[11].ControlTypeName)   //广播回路
                {
                    txtbox22.Text = HDLPF.IsNumStringMode(str, 0, 100);
                    dgvRelay[5, index].Value = txtbox22.Text + "(" + CsConst.mstrINIDefault.IniReadValue("public", "00011", "") + ")";
                }
                else if (cbControlType2.Text == CsConst.myPublicControlType[7].ControlTypeName)//GPRS控制
                {
                    txtbox22.Text = HDLPF.IsNumStringMode(str, 1, 99);
                    dgvRelay[5, index].Value = txtbox22.Text + "(" + CsConst.mstrINIDefault.IniReadValue("public", "99864", "") + ")";
                }
                else if (cbControlType2.Text == CsConst.myPublicControlType[8].ControlTypeName)//面板控制
                {
                    if (cbPanleControl2.Text == CsConst.myPublicControlType[3].ControlTypeName ||//背光亮度
                        cbPanleControl2.Text == CsConst.myPublicControlType[4].ControlTypeName) //状态灯亮度
                    {
                        txtbox22.Text = HDLPF.IsNumStringMode(str, 0, 100);
                        dgvRelay[5, index].Value = txtbox22.Text + "(" + CsConst.WholeTextsList[2524].sDisplayName + ")";
                    }
                    else if (cbPanleControl2.Text ==  CsConst.PanelControl[15] ||//空调升高温度 
                             cbPanleControl2.Text ==  CsConst.PanelControl[16] ||//空调降低温度
                             cbPanleControl2.Text == CsConst.PanelControl[23] ||//地热身高温度
                             cbPanleControl2.Text == CsConst.PanelControl[24]) //地热降低温度
                    {
                        txtbox22.Text = HDLPF.IsNumStringMode(str, 1, 255);
                        dgvRelay[5, index].Value = txtbox22.Text;

                    }
                }
                else if (cbControlType2.Text == CsConst.myPublicControlType[13].ControlTypeName)//音乐播放
                {
                    if (cbAudioControl2.SelectedIndex == 5)//歌曲播放
                    {
                        txtbox22.Text = HDLPF.IsNumStringMode(str, 0, 255);
                        dgvRelay[5, index].Value = txtbox22.Text + "(" + CsConst.mstrINIDefault.IniReadValue("public", "00046", "") + ")";
                    }
                }
                txtbox22.SelectionStart = txtbox22.Text.Length;
            }
            #endregion
            if (dgvRelay.SelectedRows == null || dgvRelay.SelectedRows.Count == 0) return;
            string strTmp = dgvRelay[5, index].Value.ToString();
            if (strTmp == null) strTmp = "N/A";
            if (txtbox22.Focused)
                HDLSysPF.ModifyMultilinesIfNeeds(strTmp, 5, dgvRelay);
        }


        void txtbox23_TextChanged(object sender, EventArgs e)
        {
            if (dgvP1.CurrentRow.Index < 0) return;
            if (dgvP1.RowCount <= 0) return;
            int index = dgvP1.CurrentRow.Index;
            string str = txtbox23.Text;
            if (str.Contains("(")) str = str.Split('(')[0].ToString();
            #region
            if (txtbox23.Text.Length > 0)
            {
                if (cbControlType3.Text == CsConst.myPublicControlType[1].ControlTypeName || //场景
                    cbControlType3.Text == CsConst.myPublicControlType[10].ControlTypeName)   //广播场景
                {
                    txtbox23.Text = HDLPF.IsNumStringMode(str, 0, 255);
                    dgvP1[5, index].Value = txtbox23.Text + "(" + CsConst.WholeTextsList[2511].sDisplayName + ")";
                }
                else if (cbControlType3.Text == CsConst.myPublicControlType[2].ControlTypeName)//序列
                {
                    txtbox23.Text = HDLPF.IsNumStringMode(str, 0, 99);
                    dgvP1[5, index].Value = txtbox23.Text + "(" + CsConst.WholeTextsList[2512].sDisplayName+ ")";
                }
                else if (cbControlType3.Text == CsConst.myPublicControlType[5].ControlTypeName || //单路调节 
                         cbControlType3.Text == CsConst.myPublicControlType[11].ControlTypeName)   //广播回路
                {
                    txtbox23.Text = HDLPF.IsNumStringMode(str, 0, 100);
                    dgvP1[5, index].Value = txtbox23.Text + "(" + CsConst.mstrINIDefault.IniReadValue("public", "00011", "") + ")";
                }
                else if (cbControlType3.Text == CsConst.myPublicControlType[7].ControlTypeName)//GPRS控制
                {
                    txtbox23.Text = HDLPF.IsNumStringMode(str, 1, 99);
                    dgvP1[5, index].Value = txtbox23.Text + "(" + CsConst.mstrINIDefault.IniReadValue("public", "99864", "") + ")";
                }
                else if (cbControlType3.Text == CsConst.myPublicControlType[8].ControlTypeName)//面板控制
                {
                    if (cbPanleControl3.Text == CsConst.myPublicControlType[3].ControlTypeName ||//背光亮度
                        cbPanleControl3.Text == CsConst.myPublicControlType[4].ControlTypeName) //状态灯亮度
                    {
                        txtbox23.Text = HDLPF.IsNumStringMode(str, 0, 100);
                        dgvP1[5, index].Value = txtbox23.Text + "(" + CsConst.WholeTextsList[2524].sDisplayName + ")";
                    }
                    else if (cbPanleControl3.Text ==  CsConst.PanelControl[15] ||//空调升高温度 
                             cbPanleControl3.Text ==  CsConst.PanelControl[16] ||//空调降低温度
                             cbPanleControl3.Text == CsConst.PanelControl[23] ||//地热身高温度
                             cbPanleControl3.Text == CsConst.PanelControl[24]) //地热降低温度
                    {
                        txtbox23.Text = HDLPF.IsNumStringMode(str, 1, 255);
                        dgvP1[5, index].Value = txtbox23.Text;

                    }
                }
                else if (cbControlType3.Text == CsConst.myPublicControlType[13].ControlTypeName)//音乐播放
                {
                    if (cbAudioControl3.SelectedIndex == 5)//歌曲播放
                    {
                        txtbox23.Text = HDLPF.IsNumStringMode(str, 0, 255);
                        dgvP1[5, index].Value = txtbox23.Text + "(" + CsConst.mstrINIDefault.IniReadValue("public", "00046", "") + ")";
                    }
                }
                txtbox23.SelectionStart = txtbox23.Text.Length;
            }
            #endregion
            if (dgvP1.SelectedRows == null || dgvP1.SelectedRows.Count == 0) return;
            string strTmp = dgvP1[5, index].Value.ToString();
            if (strTmp == null) strTmp = "N/A";
            if (txtbox23.Focused)
                HDLSysPF.ModifyMultilinesIfNeeds(strTmp, 5, dgvP1);
        }

        void txtbox24_TextChanged(object sender, EventArgs e)
        {
            if (dgvP2.CurrentRow.Index < 0) return;
            if (dgvP2.RowCount <= 0) return;
            int index = dgvP2.CurrentRow.Index;
            string str = txtbox24.Text;
            if (str.Contains("(")) str = str.Split('(')[0].ToString();
            #region
            if (txtbox24.Text.Length > 0)
            {
                if (cbControlType4.Text == CsConst.myPublicControlType[1].ControlTypeName || //场景
                    cbControlType4.Text == CsConst.myPublicControlType[10].ControlTypeName)   //广播场景
                {
                    txtbox24.Text = HDLPF.IsNumStringMode(str, 0, 255);
                    dgvP2[5, index].Value = txtbox24.Text + "(" + CsConst.WholeTextsList[2511].sDisplayName + ")";
                }
                else if (cbControlType3.Text == CsConst.myPublicControlType[2].ControlTypeName)//序列
                {
                    txtbox24.Text = HDLPF.IsNumStringMode(str, 0, 99);
                    dgvP2[5, index].Value = txtbox24.Text + "(" + CsConst.WholeTextsList[2512].sDisplayName+ ")";
                }
                else if (cbControlType4.Text == CsConst.myPublicControlType[5].ControlTypeName || //单路调节 
                         cbControlType4.Text == CsConst.myPublicControlType[11].ControlTypeName)   //广播回路
                {
                    txtbox24.Text = HDLPF.IsNumStringMode(str, 0, 100);
                    dgvP2[5, index].Value = txtbox24.Text + "(" + CsConst.mstrINIDefault.IniReadValue("public", "00011", "") + ")";
                }
                else if (cbControlType4.Text == CsConst.myPublicControlType[7].ControlTypeName)//GPRS控制
                {
                    txtbox24.Text = HDLPF.IsNumStringMode(str, 1, 99);
                    dgvP2[5, index].Value = txtbox24.Text + "(" + CsConst.mstrINIDefault.IniReadValue("public", "99864", "") + ")";
                }
                else if (cbControlType4.Text == CsConst.myPublicControlType[8].ControlTypeName)//面板控制
                {
                    if (cbPanleControl4.Text == CsConst.myPublicControlType[3].ControlTypeName ||//背光亮度
                        cbPanleControl4.Text == CsConst.myPublicControlType[4].ControlTypeName) //状态灯亮度
                    {
                        txtbox24.Text = HDLPF.IsNumStringMode(str, 0, 100);
                        dgvP2[5, index].Value = txtbox24.Text + "(" + CsConst.WholeTextsList[2524].sDisplayName + ")";
                    }
                    else if (cbPanleControl4.Text ==  CsConst.PanelControl[15] ||//空调升高温度 
                             cbPanleControl4.Text ==  CsConst.PanelControl[16] ||//空调降低温度
                             cbPanleControl4.Text == CsConst.PanelControl[23] ||//地热身高温度
                             cbPanleControl4.Text == CsConst.PanelControl[24]) //地热降低温度
                    {
                        txtbox24.Text = HDLPF.IsNumStringMode(str, 1, 255);
                        dgvP2[5, index].Value = txtbox24.Text;

                    }
                }
                else if (cbControlType4.Text == CsConst.myPublicControlType[13].ControlTypeName)//音乐播放
                {
                    if (cbAudioControl4.SelectedIndex == 5)//歌曲播放
                    {
                        txtbox24.Text = HDLPF.IsNumStringMode(str, 0, 255);
                        dgvP2[5, index].Value = txtbox24.Text + "(" + CsConst.mstrINIDefault.IniReadValue("public", "00046", "") + ")";
                    }
                }
                txtbox24.SelectionStart = txtbox24.Text.Length;
            }
            #endregion
            if (dgvP2.SelectedRows == null || dgvP2.SelectedRows.Count == 0) return;
            string strTmp = dgvP2[5, index].Value.ToString();
            if (strTmp == null) strTmp = "N/A";
            if (txtbox24.Focused)
                HDLSysPF.ModifyMultilinesIfNeeds(strTmp, 5, dgvP2);
        }

        void txtbox1_TextChanged(object sender, EventArgs e)
        {
            if (dgvSwitch.CurrentRow.Index < 0) return;
            if (dgvSwitch.RowCount <= 0) return;
            int index = dgvSwitch.CurrentRow.Index;
            string str = txtbox1.Text;
            if (str.Contains("(")) str = str.Split('(')[0].ToString();
            #region
            if (txtbox1.Text.Length > 0)
            {
                if (cbControlType.Text == CsConst.myPublicControlType[1].ControlTypeName || //场景
                    cbControlType.Text == CsConst.myPublicControlType[2].ControlTypeName)   //序列
                {
                    txtbox1.Text = HDLPF.IsNumStringMode(str, 1, 48);
                    dgvSwitch[4, index].Value = txtbox1.Text + "(" + CsConst.WholeTextsList[2510].sDisplayName + ")";
                }
                else if (cbControlType.Text == CsConst.myPublicControlType[4].ControlTypeName)//通用开关
                {
                    txtbox1.Text = HDLPF.IsNumStringMode(str, 1, 255);
                    dgvSwitch[4, index].Value = txtbox1.Text + "(" + CsConst.WholeTextsList[2513].sDisplayName + ")";
                }
                else if (cbControlType.Text == CsConst.myPublicControlType[5].ControlTypeName)//单路调节
                {
                    txtbox1.Text = HDLPF.IsNumStringMode(str, 1, 240);
                    dgvSwitch[4, index].Value = txtbox1.Text + "(" + CsConst.WholeTextsList[934].sDisplayName + ")";
                }
                else if (cbControlType.Text == CsConst.myPublicControlType[6].ControlTypeName)//窗帘开关
                {
                    txtbox1.Text = HDLPF.IsNumStringMode(str, 1, 16);
                    dgvSwitch[4, index].Value = txtbox1.Text + "(" + CsConst.mstrINIDefault.IniReadValue("public", "99844", "") + ")";
                }
                else if (cbControlType.Text == CsConst.myPublicControlType[12].ControlTypeName)//消防模块
                {
                    txtbox1.Text = HDLPF.IsNumStringMode(str, 1, 8);
                    dgvSwitch[4, index].Value = txtbox1.Text + "(" + CsConst.WholeTextsList[2510].sDisplayName + ")";
                }
                txtbox1.SelectionStart = txtbox1.Text.Length;
            }
            #endregion

            if (dgvSwitch.SelectedRows == null || dgvSwitch.SelectedRows.Count == 0) return;
            string strTmp = dgvSwitch[4, index].Value.ToString();
            if (strTmp == null) strTmp = "N/A";
            if (txtbox1.Focused)
                HDLSysPF.ModifyMultilinesIfNeeds(strTmp, 4, dgvSwitch);
        }

        void txtbox12_TextChanged(object sender, EventArgs e)
        {
            if (dgvRelay.CurrentRow.Index < 0) return;
            if (dgvRelay.RowCount <= 0) return;
            int index = dgvRelay.CurrentRow.Index;
            string str = txtbox12.Text;
            if (str.Contains("(")) str = str.Split('(')[0].ToString();
            #region
            if (txtbox12.Text.Length > 0)
            {
                if (cbControlType2.Text == CsConst.myPublicControlType[1].ControlTypeName || //场景
                    cbControlType2.Text == CsConst.myPublicControlType[2].ControlTypeName)   //序列
                {
                    txtbox12.Text = HDLPF.IsNumStringMode(str, 1, 48);
                    dgvRelay[4, index].Value = txtbox12.Text + "(" + CsConst.WholeTextsList[2510].sDisplayName + ")";
                }
                else if (cbControlType2.Text == CsConst.myPublicControlType[4].ControlTypeName)//通用开关
                {
                    txtbox12.Text = HDLPF.IsNumStringMode(str, 1, 255);
                    dgvRelay[4, index].Value = txtbox12.Text + "(" + CsConst.WholeTextsList[2513].sDisplayName + ")";
                }
                else if (cbControlType2.Text == CsConst.myPublicControlType[5].ControlTypeName)//单路调节
                {
                    txtbox12.Text = HDLPF.IsNumStringMode(str, 1, 240);
                    dgvRelay[4, index].Value = txtbox12.Text + "(" + CsConst.WholeTextsList[934].sDisplayName + ")";
                }
                else if (cbControlType2.Text == CsConst.myPublicControlType[6].ControlTypeName)//窗帘开关
                {
                    txtbox12.Text = HDLPF.IsNumStringMode(str, 1, 16);
                    dgvRelay[4, index].Value = txtbox12.Text + "(" + CsConst.mstrINIDefault.IniReadValue("public", "99844", "") + ")";
                }
                else if (cbControlType2.Text == CsConst.myPublicControlType[12].ControlTypeName)//消防模块
                {
                    txtbox12.Text = HDLPF.IsNumStringMode(str, 1, 8);
                    dgvRelay[4, index].Value = txtbox12.Text + "(" + CsConst.WholeTextsList[2510].sDisplayName + ")";
                }
                txtbox12.SelectionStart = txtbox12.Text.Length;
            }
            #endregion

            if (dgvRelay.SelectedRows == null || dgvRelay.SelectedRows.Count == 0) return;
            string strTmp = dgvRelay[4, index].Value.ToString();
            if (strTmp == null) strTmp = "N/A";
            if (txtbox12.Focused)
                HDLSysPF.ModifyMultilinesIfNeeds(strTmp, 4, dgvRelay);
        }

        void txtbox13_TextChanged(object sender, EventArgs e)
        {
            if (dgvP1.CurrentRow.Index < 0) return;
            if (dgvP1.RowCount <= 0) return;
            int index = dgvP1.CurrentRow.Index;
            string str = txtbox13.Text;
            if (str.Contains("(")) str = str.Split('(')[0].ToString();
            #region
            if (txtbox13.Text.Length > 0)
            {
                if (cbControlType3.Text == CsConst.myPublicControlType[1].ControlTypeName || //场景
                    cbControlType3.Text == CsConst.myPublicControlType[2].ControlTypeName)   //序列
                {
                    txtbox13.Text = HDLPF.IsNumStringMode(str, 1, 48);
                    dgvP1[4, index].Value = txtbox13.Text + "(" + CsConst.WholeTextsList[2510].sDisplayName + ")";
                }
                else if (cbControlType3.Text == CsConst.myPublicControlType[4].ControlTypeName)//通用开关
                {
                    txtbox13.Text = HDLPF.IsNumStringMode(str, 1, 255);
                    dgvP1[4, index].Value = txtbox13.Text + "(" + CsConst.WholeTextsList[2513].sDisplayName + ")";
                }
                else if (cbControlType3.Text == CsConst.myPublicControlType[5].ControlTypeName)//单路调节
                {
                    txtbox13.Text = HDLPF.IsNumStringMode(str, 1, 240);
                    dgvP1[4, index].Value = txtbox13.Text + "(" + CsConst.WholeTextsList[934].sDisplayName + ")";
                }
                else if (cbControlType3.Text == CsConst.myPublicControlType[6].ControlTypeName)//窗帘开关
                {
                    txtbox13.Text = HDLPF.IsNumStringMode(str, 1, 16);
                    dgvP1[4, index].Value = txtbox13.Text + "(" + CsConst.mstrINIDefault.IniReadValue("public", "99844", "") + ")";
                }
                else if (cbControlType3.Text == CsConst.myPublicControlType[12].ControlTypeName)//消防模块
                {
                    txtbox13.Text = HDLPF.IsNumStringMode(str, 1, 8);
                    dgvP1[4, index].Value = txtbox13.Text + "(" + CsConst.WholeTextsList[2510].sDisplayName + ")";
                }
                txtbox13.SelectionStart = txtbox13.Text.Length;
            }
            #endregion

            if (dgvP1.SelectedRows == null || dgvP1.SelectedRows.Count == 0) return;
            string strTmp = dgvP1[4, index].Value.ToString();
            if (strTmp == null) strTmp = "N/A";
            if (txtbox13.Focused)
                HDLSysPF.ModifyMultilinesIfNeeds(strTmp, 4, dgvP1);
        }

        void txtbox14_TextChanged(object sender, EventArgs e)
        {
            if (dgvP2.CurrentRow.Index < 0) return;
            if (dgvP2.RowCount <= 0) return;
            int index = dgvP2.CurrentRow.Index;
            string str = txtbox14.Text;
            if (str.Contains("(")) str = str.Split('(')[0].ToString();
            #region
            if (txtbox14.Text.Length > 0)
            {
                if (cbControlType4.Text == CsConst.myPublicControlType[1].ControlTypeName || //场景
                    cbControlType4.Text == CsConst.myPublicControlType[2].ControlTypeName)   //序列
                {
                    txtbox14.Text = HDLPF.IsNumStringMode(str, 1, 48);
                    dgvP2[4, index].Value = txtbox14.Text + "(" + CsConst.WholeTextsList[2510].sDisplayName + ")";
                }
                else if (cbControlType4.Text == CsConst.myPublicControlType[4].ControlTypeName)//通用开关
                {
                    txtbox14.Text = HDLPF.IsNumStringMode(str, 1, 255);
                    dgvP2[4, index].Value = txtbox14.Text + "(" + CsConst.WholeTextsList[2513].sDisplayName + ")";
                }
                else if (cbControlType4.Text == CsConst.myPublicControlType[5].ControlTypeName)//单路调节
                {
                    txtbox14.Text = HDLPF.IsNumStringMode(str, 1, 240);
                    dgvP2[4, index].Value = txtbox14.Text + "(" + CsConst.WholeTextsList[934].sDisplayName + ")";
                }
                else if (cbControlType4.Text == CsConst.myPublicControlType[6].ControlTypeName)//窗帘开关
                {
                    txtbox14.Text = HDLPF.IsNumStringMode(str, 1, 16);
                    dgvP2[4, index].Value = txtbox14.Text + "(" + CsConst.mstrINIDefault.IniReadValue("public", "99844", "") + ")";
                }
                else if (cbControlType4.Text == CsConst.myPublicControlType[12].ControlTypeName)//消防模块
                {
                    txtbox14.Text = HDLPF.IsNumStringMode(str, 1, 8);
                    dgvP2[4, index].Value = txtbox14.Text + "(" + CsConst.WholeTextsList[2510].sDisplayName + ")";
                }
                txtbox14.SelectionStart = txtbox14.Text.Length;
            }
            #endregion

            if (dgvP2.SelectedRows == null || dgvP2.SelectedRows.Count == 0) return;
            string strTmp = dgvP2[4, index].Value.ToString();
            if (strTmp == null) strTmp = "N/A";
            if (txtbox14.Focused)
                HDLSysPF.ModifyMultilinesIfNeeds(strTmp, 4, dgvP2);
        }

        void txtDev_TextChanged(object sender, EventArgs e)
        {
            if (dgvSwitch.CurrentRow.Index < 0) return;
            if (dgvSwitch.RowCount <= 0) return;
            int index = dgvSwitch.CurrentRow.Index;
            if (txtDev.Text.Length > 0)
            {
                txtDev.Text = HDLPF.IsNumStringMode(txtDev.Text, 0, 254);
                txtDev.SelectionStart = txtDev.Text.Length;
                dgvSwitch[2, index].Value = txtDev.Text;
                if (txtDev.Focused)
                    HDLSysPF.ModifyMultilinesIfNeeds(dgvSwitch[2, index].Value.ToString(), 2, dgvSwitch);
            }
        }

        void txtDev2_TextChanged(object sender, EventArgs e)
        {
            if (dgvRelay.CurrentRow.Index < 0) return;
            if (dgvRelay.RowCount <= 0) return;
            int index = dgvRelay.CurrentRow.Index;
            if (txtDev2.Text.Length > 0)
            {
                txtDev2.Text = HDLPF.IsNumStringMode(txtDev2.Text, 0, 254);
                txtDev2.SelectionStart = txtDev2.Text.Length;
                dgvRelay[2, index].Value = txtDev2.Text;
                if (txtDev2.Focused)
                    HDLSysPF.ModifyMultilinesIfNeeds(dgvRelay[2, index].Value.ToString(), 2, dgvRelay);
            }
        }

        void txtDev3_TextChanged(object sender, EventArgs e)
        {
            if (dgvP1.CurrentRow.Index < 0) return;
            if (dgvP1.RowCount <= 0) return;
            int index = dgvP1.CurrentRow.Index;
            if (txtDev3.Text.Length > 0)
            {
                txtDev3.Text = HDLPF.IsNumStringMode(txtDev3.Text, 0, 254);
                txtDev3.SelectionStart = txtDev3.Text.Length;
                dgvP1[2, index].Value = txtDev3.Text;
                if (txtDev3.Focused)
                    HDLSysPF.ModifyMultilinesIfNeeds(dgvP1[2, index].Value.ToString(), 2, dgvP1);
            }
        }

        void txtDev4_TextChanged(object sender, EventArgs e)
        {
            if (dgvP2.CurrentRow.Index < 0) return;
            if (dgvP2.RowCount <= 0) return;
            int index = dgvP2.CurrentRow.Index;
            if (txtDev4.Text.Length > 0)
            {
                txtDev4.Text = HDLPF.IsNumStringMode(txtDev4.Text, 0, 254);
                txtDev4.SelectionStart = txtDev4.Text.Length;
                dgvP2[2, index].Value = txtDev4.Text;
                if (txtDev4.Focused)
                    HDLSysPF.ModifyMultilinesIfNeeds(dgvP2[2, index].Value.ToString(), 2, dgvP2);
            }
        }

        void txtSub_TextChanged(object sender, EventArgs e)
        {
            if (dgvSwitch.CurrentRow.Index < 0) return;
            if (dgvSwitch.RowCount <= 0) return;
            int index = dgvSwitch.CurrentRow.Index;
            if (txtSub.Text.Length > 0)
            {
                txtSub.Text = HDLPF.IsNumStringMode(txtSub.Text, 0, 254);
                txtSub.SelectionStart = txtSub.Text.Length;
                dgvSwitch[1, index].Value = txtSub.Text;
                if (txtSub.Focused)
                    HDLSysPF.ModifyMultilinesIfNeeds(dgvSwitch[1, index].Value.ToString(), 1, dgvSwitch);
            }
        }

        void txtSub2_TextChanged(object sender, EventArgs e)
        {
            if (dgvRelay.CurrentRow.Index < 0) return;
            if (dgvRelay.RowCount <= 0) return;
            int index = dgvRelay.CurrentRow.Index;
            if (txtSub2.Text.Length > 0)
            {
                txtSub2.Text = HDLPF.IsNumStringMode(txtSub2.Text, 0, 254);
                txtSub2.SelectionStart = txtSub2.Text.Length;
                dgvRelay[1, index].Value = txtSub2.Text;
                if (txtSub2.Focused)
                    HDLSysPF.ModifyMultilinesIfNeeds(dgvRelay[1, index].Value.ToString(), 1, dgvRelay);
            }
        }

        void txtSub3_TextChanged(object sender, EventArgs e)
        {
            if (dgvP1.CurrentRow.Index < 0) return;
            if (dgvP1.RowCount <= 0) return;
            int index = dgvP1.CurrentRow.Index;
            if (txtSub3.Text.Length > 0)
            {
                txtSub3.Text = HDLPF.IsNumStringMode(txtSub3.Text, 0, 254);
                txtSub3.SelectionStart = txtSub3.Text.Length;
                dgvP1[1, index].Value = txtSub3.Text;
                if (txtSub3.Focused)
                    HDLSysPF.ModifyMultilinesIfNeeds(dgvP1[1, index].Value.ToString(), 1, dgvP1);
            }
        }

        void txtSub4_TextChanged(object sender, EventArgs e)
        {
            if (dgvP2.CurrentRow.Index < 0) return;
            if (dgvP2.RowCount <= 0) return;
            int index = dgvP2.CurrentRow.Index;
            if (txtSub4.Text.Length > 0)
            {
                txtSub4.Text = HDLPF.IsNumStringMode(txtSub4.Text, 0, 254);
                txtSub4.SelectionStart = txtSub4.Text.Length;
                dgvP2[1, index].Value = txtSub4.Text;
                if (txtSub4.Focused)
                    HDLSysPF.ModifyMultilinesIfNeeds(dgvP2[1, index].Value.ToString(), 1, dgvP2);
            }
        }

        void cbbox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dgvSwitch.CurrentRow.Index < 0) return;
            if (dgvSwitch.RowCount <= 0) return;
            int index = dgvSwitch.CurrentRow.Index;
            string str = dgvSwitch[6, index].Value.ToString();
            if (str.Contains("(")) str = str.Split('(')[0].ToString();
            if (cbPanleControl.Visible && cbPanleControl.Text == CsConst.myPublicControlType[22].ControlTypeName)
            {
                if (cbbox3.SelectedIndex == 8)
                    dgvSwitch[6, index].Value = cbbox3.Text;
                else
                    dgvSwitch[6, index].Value = cbbox3.Text.ToString() + "(" + CsConst.WholeTextsList[934].sDisplayName + ")";
            }
            else
                dgvSwitch[6, index].Value = cbbox3.Text;
            #region
            if (dgvSwitch.SelectedRows == null || dgvSwitch.SelectedRows.Count == 0) return;
            string strTmp = dgvSwitch[6, index].Value.ToString();
            if (strTmp == null) strTmp = "N/A";
            if (cbbox3.Focused)
                HDLSysPF.ModifyMultilinesIfNeeds(strTmp, 6, dgvSwitch);
            #endregion
        }

        void cbbox32_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dgvRelay.CurrentRow.Index < 0) return;
            if (dgvRelay.RowCount <= 0) return;
            int index = dgvRelay.CurrentRow.Index;
            string str = dgvRelay[6, index].Value.ToString();
            if (str.Contains("(")) str = str.Split('(')[0].ToString();
            if (cbPanleControl2.Visible && cbPanleControl2.Text == CsConst.myPublicControlType[22].ControlTypeName)
            {
                if (cbbox32.SelectedIndex == 8)
                    dgvRelay[6, index].Value = cbbox32.Text;
                else
                    dgvRelay[6, index].Value = cbbox32.Text.ToString() + "(" + CsConst.WholeTextsList[934].sDisplayName + ")";
            }
            else
                dgvRelay[6, index].Value = cbbox32.Text;
            #region
            if (dgvRelay.SelectedRows == null || dgvRelay.SelectedRows.Count == 0) return;
            string strTmp = dgvRelay[6, index].Value.ToString();
            if (strTmp == null) strTmp = "N/A";
            if (cbbox32.Focused)
                HDLSysPF.ModifyMultilinesIfNeeds(strTmp, 6, dgvRelay);
            #endregion
        }

        void cbbox33_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dgvP1.CurrentRow.Index < 0) return;
            if (dgvP1.RowCount <= 0) return;
            int index = dgvP1.CurrentRow.Index;
            string str = dgvP1[6, index].Value.ToString();
            if (str.Contains("(")) str = str.Split('(')[0].ToString();
            if (cbPanleControl3.Visible && cbPanleControl3.Text == CsConst.myPublicControlType[22].ControlTypeName)
            {
                if (cbbox33.SelectedIndex == 8)
                    dgvP1[6, index].Value = cbbox33.Text;
                else
                    dgvP1[6, index].Value = cbbox33.Text.ToString() + "(" + CsConst.WholeTextsList[934].sDisplayName + ")";
            }
            else
                dgvP1[6, index].Value = cbbox33.Text;
            #region
            if (dgvP1.SelectedRows == null || dgvP1.SelectedRows.Count == 0) return;
            string strTmp = dgvP1[6, index].Value.ToString();
            if (strTmp == null) strTmp = "N/A";
            if (cbbox33.Focused)
                HDLSysPF.ModifyMultilinesIfNeeds(strTmp, 6, dgvP1);
            #endregion
        }

        void cbbox34_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dgvP2.CurrentRow.Index < 0) return;
            if (dgvP2.RowCount <= 0) return;
            int index = dgvP2.CurrentRow.Index;
            string str = dgvP2[6, index].Value.ToString();
            if (str.Contains("(")) str = str.Split('(')[0].ToString();
            if (cbPanleControl4.Visible && cbPanleControl4.Text == CsConst.myPublicControlType[22].ControlTypeName)
            {
                if (cbbox34.SelectedIndex == 8)
                    dgvP2[6, index].Value = cbbox34.Text;
                else
                    dgvP2[6, index].Value = cbbox34.Text.ToString() + "(" + CsConst.WholeTextsList[934].sDisplayName + ")";
            }
            else
                dgvP2[6, index].Value = cbbox34.Text;
            #region
            if (dgvP2.SelectedRows == null || dgvP2.SelectedRows.Count == 0) return;
            string strTmp = dgvP2[6, index].Value.ToString();
            if (strTmp == null) strTmp = "N/A";
            if (cbbox34.Focused)
                HDLSysPF.ModifyMultilinesIfNeeds(strTmp, 6, dgvP2);
            #endregion
        }

        void cbbox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dgvSwitch.CurrentRow.Index < 0) return;
            if (dgvSwitch.RowCount <= 0) return;
            txtbox3.Visible = false;
            cbbox3.Visible = false;
            int index = dgvSwitch.CurrentRow.Index;
            string str = dgvSwitch[5, index].Value.ToString();
            if (str.Contains("(")) str = str.Split('(')[0].ToString();
            string str3 = dgvSwitch[6, index].Value.ToString();
            if (str3.Contains("(")) str3 = str3.Split('(')[0].ToString();
            #region
            if (cbControlType.Text == CsConst.myPublicControlType[4].ControlTypeName || //通用开关
                cbControlType.Text == CsConst.myPublicControlType[6].ControlTypeName)   //窗帘开关
            {
                dgvSwitch[5, index].Value = cbbox2.Text + "(" + CsConst.WholeTextsList[2529].sDisplayName + ")";
                if (cbControlType.Text == CsConst.myPublicControlType[6].ControlTypeName)
                {
                    if (cbbox2.SelectedIndex > 2)
                    {
                        txtbox1_TextChanged(null, null);
                    }
                    else
                    {
                        HDLSysPF.addcontrols(4, index, txtbox1, dgvSwitch);
                    }
                }
            }
            else if (cbControlType.Text == CsConst.myPublicControlType[8].ControlTypeName)//面板控制
            {
                if (cbPanleControl.Text == CsConst.myPublicControlType[1].ControlTypeName ||
                    cbPanleControl.Text == CsConst.myPublicControlType[2].ControlTypeName ||
                    cbPanleControl.Text == CsConst.myPublicControlType[5].ControlTypeName ||
                    cbPanleControl.Text == CsConst.myPublicControlType[7].ControlTypeName ||
                    cbPanleControl.Text == CsConst.myPublicControlType[8].ControlTypeName ||
                    cbPanleControl.Text ==  CsConst.PanelControl[12] ||
                    cbPanleControl.Text == CsConst.PanelControl[21])
                {
                    dgvSwitch[5, index].Value = cbbox2.Text + "(" + CsConst.WholeTextsList[2529].sDisplayName + ")";
                }
                else if (cbPanleControl.Text == CsConst.PanelControl[17] ||
                         cbPanleControl.Text == CsConst.PanelControl[18] ||
                         cbPanleControl.Text == CsConst.PanelControl[19] ||
                         cbPanleControl.Text == CsConst.PanelControl[20])
                {
                    dgvSwitch[5, index].Value = cbbox2.Text + "(" + CsConst.mstrINIDefault.IniReadValue("public", "99874", "") + ")";
                }
                else if (cbPanleControl.Text ==  CsConst.PanelControl[13] ||
                         cbPanleControl.Text == CsConst.myPublicControlType[22].ControlTypeName)
                {
                    dgvSwitch[5, index].Value = cbbox2.Text + "(" + CsConst.mstrINIDefault.IniReadValue("public", "99876", "") + ")";
                }
                else if (cbPanleControl.Text ==  CsConst.PanelControl[14])
                {
                    dgvSwitch[5, index].Value = cbbox2.Text + "(" + CsConst.mstrINIDefault.IniReadValue("public", "99875", "") + ")";
                }
                else if (cbPanleControl.Text == CsConst.myPublicControlType[6].ControlTypeName ||
                         cbPanleControl.Text == CsConst.myPublicControlType[9].ControlTypeName ||
                         cbPanleControl.Text ==  CsConst.PanelControl[10] ||
                         cbPanleControl.Text ==  CsConst.PanelControl[11])
                {
                    dgvSwitch[5, index].Value = cbbox2.Text;
                }
            }
            else if (cbControlType.Text == CsConst.myPublicControlType[12].ControlTypeName)//消防模块
            {
                dgvSwitch[5, index].Value = cbbox2.Text;
            }
            else if (cbControlType.Text == CsConst.myPublicControlType[13].ControlTypeName)//音乐模块
            {
                if (cbAudioControl.SelectedIndex == 0 || cbAudioControl.SelectedIndex == 1 ||
                    cbAudioControl.SelectedIndex == 2 || cbAudioControl.SelectedIndex == 3 ||
                    cbAudioControl.SelectedIndex == 4 || cbAudioControl.SelectedIndex == 6 ||
                    cbAudioControl.SelectedIndex == 7)
                {
                    dgvSwitch[5, index].Value = cbbox2.Text;
                }
            }
            #endregion
            #region
            if (cbAudioControl.Visible)
            {
                if (cbAudioControl.SelectedIndex == 2)
                {
                    if (cbbox2.Visible && cbbox2.Items.Count >= 6)
                    {
                        if (cbbox2.SelectedIndex == 2 || cbbox2.SelectedIndex == 5)
                        {
                            txtbox3.Text = str3;
                            HDLSysPF.addcontrols(6, index, txtbox3, dgvSwitch);
                        }
                    }
                }
                else if (cbAudioControl.SelectedIndex == 4)
                {
                    if (cbbox2.Visible && cbbox2.Items.Count > 0)
                    {
                        if (cbbox2.SelectedIndex == 0)
                        {
                            cbbox3.Items.Clear();
                            cbbox3.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00044", ""));
                            cbbox3.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00045", ""));
                            for (int i = 0; i < 80; i++)
                                cbbox3.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99872", "") + ":" + i.ToString());
                        }
                        else
                        {
                            cbbox3.Items.Clear();
                            cbbox3.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00044", ""));
                            cbbox3.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00045", ""));
                        }
                        HDLSysPF.addcontrols(6, index, cbbox3, dgvSwitch);
                    }
                }
                else if (cbAudioControl.SelectedIndex == 6 || cbAudioControl.SelectedIndex == 7)//直接源播放 广播播放
                {
                    txtbox3.Visible = true;
                }
            }
            else if (cbPanleControl.Visible)
            {
                if ((cbPanleControl.Text == CsConst.myPublicControlType[6].ControlTypeName ||//面板页面锁 
                     cbPanleControl.Text == CsConst.myPublicControlType[9].ControlTypeName ||//按键锁
                     cbPanleControl.Text ==  CsConst.PanelControl[10] ||//控制按键状态
                     cbPanleControl.Text ==  CsConst.PanelControl[11] || //控制面板按键
                     cbPanleControl.Text == CsConst.PanelControl[21] ||//地热开关
                     cbPanleControl.Text == CsConst.myPublicControlType[22].ControlTypeName)) //地热模式
                {
                    cbbox3.Visible = true;
                }
            }
            #endregion
            #region
            if (cbbox3.Visible && cbbox3.Items.Count > 0)
            {
                if (!cbbox3.Items.Contains(str3))
                    cbbox3.SelectedIndex = 0;
                else
                    cbbox3.Text = str3;
            }
            #endregion
            #region
            if (txtbox3.Visible) txtbox3_TextChanged(null, null);
            if (cbbox3.Visible) cbbox3_SelectedIndexChanged(null, null);
            #endregion
            #region
            if (dgvSwitch.SelectedRows == null || dgvSwitch.SelectedRows.Count == 0) return;
            string strTmp = dgvSwitch[5, index].Value.ToString();
            if (strTmp == null) strTmp = "N/A";
            if (cbbox2.Focused)
                HDLSysPF.ModifyMultilinesIfNeeds(strTmp, 5, dgvSwitch);
            #endregion
        }

        void cbbox22_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dgvRelay.CurrentRow.Index < 0) return;
            if (dgvRelay.RowCount <= 0) return;
            txtbox32.Visible = false;
            cbbox32.Visible = false;
            int index = dgvRelay.CurrentRow.Index;
            string str = dgvRelay[5, index].Value.ToString();
            if (str.Contains("(")) str = str.Split('(')[0].ToString();
            string str3 = dgvRelay[6, index].Value.ToString();
            if (str3.Contains("(")) str3 = str3.Split('(')[0].ToString();
            #region
            if (cbControlType2.Text == CsConst.myPublicControlType[4].ControlTypeName || //通用开关
                cbControlType2.Text == CsConst.myPublicControlType[6].ControlTypeName)   //窗帘开关
            {
                dgvRelay[5, index].Value = cbbox22.Text + "(" + CsConst.WholeTextsList[2529].sDisplayName + ")";
                if (cbControlType2.Text == CsConst.myPublicControlType[6].ControlTypeName)
                {
                    if (cbbox22.SelectedIndex > 2)
                    {
                        txtbox12_TextChanged(null, null);
                    }
                    else
                    {
                        HDLSysPF.addcontrols(4, index, txtbox12, dgvRelay);
                    }
                }
            }
            else if (cbControlType2.Text == CsConst.myPublicControlType[8].ControlTypeName)//面板控制
            {
                if (cbPanleControl2.Text == CsConst.myPublicControlType[1].ControlTypeName ||
                    cbPanleControl2.Text == CsConst.myPublicControlType[2].ControlTypeName ||
                    cbPanleControl2.Text == CsConst.myPublicControlType[5].ControlTypeName ||
                    cbPanleControl2.Text == CsConst.myPublicControlType[7].ControlTypeName ||
                    cbPanleControl2.Text == CsConst.myPublicControlType[8].ControlTypeName ||
                    cbPanleControl2.Text ==  CsConst.PanelControl[12] ||
                    cbPanleControl2.Text == CsConst.PanelControl[21])
                {
                    dgvRelay[5, index].Value = cbbox22.Text + "(" + CsConst.WholeTextsList[2529].sDisplayName + ")";
                }
                else if (cbPanleControl2.Text == CsConst.PanelControl[17] ||
                         cbPanleControl2.Text == CsConst.PanelControl[18] ||
                         cbPanleControl2.Text == CsConst.PanelControl[19] ||
                         cbPanleControl2.Text == CsConst.PanelControl[20])
                {
                    dgvRelay[5, index].Value = cbbox22.Text + "(" + CsConst.mstrINIDefault.IniReadValue("public", "99874", "") + ")";
                }
                else if (cbPanleControl2.Text ==  CsConst.PanelControl[13] ||
                         cbPanleControl2.Text == CsConst.myPublicControlType[22].ControlTypeName)
                {
                    dgvRelay[5, index].Value = cbbox22.Text + "(" + CsConst.mstrINIDefault.IniReadValue("public", "99876", "") + ")";
                }
                else if (cbPanleControl2.Text ==  CsConst.PanelControl[14])
                {
                    dgvRelay[5, index].Value = cbbox22.Text + "(" + CsConst.mstrINIDefault.IniReadValue("public", "99875", "") + ")";
                }
                else if (cbPanleControl2.Text == CsConst.myPublicControlType[6].ControlTypeName ||
                         cbPanleControl2.Text == CsConst.myPublicControlType[9].ControlTypeName ||
                         cbPanleControl2.Text ==  CsConst.PanelControl[10] ||
                         cbPanleControl2.Text ==  CsConst.PanelControl[11])
                {
                    dgvRelay[5, index].Value = cbbox22.Text;
                }
            }
            else if (cbControlType2.Text == CsConst.myPublicControlType[12].ControlTypeName)//消防模块
            {
                dgvRelay[5, index].Value = cbbox22.Text;
            }
            else if (cbControlType2.Text == CsConst.myPublicControlType[13].ControlTypeName)//音乐模块
            {
                if (cbAudioControl2.SelectedIndex == 0 || cbAudioControl2.SelectedIndex == 1 ||
                    cbAudioControl2.SelectedIndex == 2 || cbAudioControl2.SelectedIndex == 3 ||
                    cbAudioControl2.SelectedIndex == 4 || cbAudioControl2.SelectedIndex == 6 ||
                    cbAudioControl2.SelectedIndex == 7)
                {
                    dgvRelay[5, index].Value = cbbox22.Text;
                }
            }
            #endregion
            #region
            if (cbAudioControl2.Visible)
            {
                if (cbAudioControl2.SelectedIndex == 2)
                {
                    if (cbbox22.Visible && cbbox22.Items.Count >= 6)
                    {
                        if (cbbox22.SelectedIndex == 2 || cbbox22.SelectedIndex == 5)
                        {
                            txtbox32.Text = str3;
                            HDLSysPF.addcontrols(6, index, txtbox32, dgvRelay);
                        }
                    }
                }
                else if (cbAudioControl2.SelectedIndex == 4)
                {
                    if (cbbox22.Visible && cbbox22.Items.Count > 0)
                    {
                        if (cbbox22.SelectedIndex == 0)
                        {
                            cbbox32.Items.Clear();
                            cbbox32.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00044", ""));
                            cbbox32.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00045", ""));
                            for (int i = 0; i < 80; i++)
                                cbbox32.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99872", "") + ":" + i.ToString());
                        }
                        else
                        {
                            cbbox32.Items.Clear();
                            cbbox32.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00044", ""));
                            cbbox32.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00045", ""));
                        }
                        HDLSysPF.addcontrols(6, index, cbbox32, dgvRelay);
                    }
                }
                else if (cbAudioControl2.SelectedIndex == 6 || cbAudioControl2.SelectedIndex == 7)//直接源播放 广播播放
                {
                    txtbox32.Visible = true;
                }
            }
            else if (cbPanleControl2.Visible)
            {
                if ((cbPanleControl2.Text == CsConst.myPublicControlType[6].ControlTypeName ||//面板页面锁 
                     cbPanleControl2.Text == CsConst.myPublicControlType[9].ControlTypeName ||//按键锁
                     cbPanleControl2.Text ==  CsConst.PanelControl[10] ||//控制按键状态
                     cbPanleControl2.Text ==  CsConst.PanelControl[11] || //控制面板按键
                     cbPanleControl2.Text == CsConst.PanelControl[21] ||//地热开关
                     cbPanleControl2.Text == CsConst.myPublicControlType[22].ControlTypeName)) //地热模式
                {
                    cbbox32.Visible = true;
                }
            }
            #endregion
            #region
            if (cbbox32.Visible && cbbox32.Items.Count > 0)
            {
                if (!cbbox32.Items.Contains(str3))
                    cbbox32.SelectedIndex = 0;
                else
                    cbbox32.Text = str3;
            }
            #endregion
            #region
            if (txtbox32.Visible) txtbox32_TextChanged(null, null);
            if (cbbox32.Visible) cbbox32_SelectedIndexChanged(null, null);
            #endregion
            #region
            if (dgvRelay.SelectedRows == null || dgvRelay.SelectedRows.Count == 0) return;
            string strTmp = dgvRelay[5, index].Value.ToString();
            if (strTmp == null) strTmp = "N/A";
            if (cbbox22.Focused)
                HDLSysPF.ModifyMultilinesIfNeeds(strTmp, 5, dgvRelay);
            #endregion
        }

        void cbbox23_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dgvP1.CurrentRow.Index < 0) return;
            if (dgvP1.RowCount <= 0) return;
            txtbox33.Visible = false;
            cbbox33.Visible = false;
            int index = dgvP1.CurrentRow.Index;
            string str = dgvP1[5, index].Value.ToString();
            if (str.Contains("(")) str = str.Split('(')[0].ToString();
            string str3 = dgvP1[6, index].Value.ToString();
            if (str3.Contains("(")) str3 = str3.Split('(')[0].ToString();
            #region
            if (cbControlType3.Text == CsConst.myPublicControlType[4].ControlTypeName || //通用开关
                cbControlType3.Text == CsConst.myPublicControlType[6].ControlTypeName)   //窗帘开关
            {
                dgvP1[5, index].Value = cbbox23.Text + "(" + CsConst.WholeTextsList[2529].sDisplayName + ")";
                if (cbControlType3.Text == CsConst.myPublicControlType[6].ControlTypeName)
                {
                    if (cbbox23.SelectedIndex > 2)
                    {
                        txtbox13_TextChanged(null, null);
                    }
                    else
                    {
                        HDLSysPF.addcontrols(4, index, txtbox13, dgvP1);
                    }
                }
            }
            else if (cbControlType3.Text == CsConst.myPublicControlType[8].ControlTypeName)//面板控制
            {
                if (cbPanleControl3.Text == CsConst.myPublicControlType[1].ControlTypeName ||
                    cbPanleControl3.Text == CsConst.myPublicControlType[2].ControlTypeName ||
                    cbPanleControl3.Text == CsConst.myPublicControlType[5].ControlTypeName ||
                    cbPanleControl3.Text == CsConst.myPublicControlType[7].ControlTypeName ||
                    cbPanleControl3.Text == CsConst.myPublicControlType[8].ControlTypeName ||
                    cbPanleControl3.Text ==  CsConst.PanelControl[12] ||
                    cbPanleControl3.Text == CsConst.PanelControl[21])
                {
                    dgvP1[5, index].Value = cbbox23.Text + "(" + CsConst.WholeTextsList[2529].sDisplayName + ")";
                }
                else if (cbPanleControl3.Text == CsConst.PanelControl[17] ||
                         cbPanleControl3.Text == CsConst.PanelControl[18] ||
                         cbPanleControl3.Text == CsConst.PanelControl[19] ||
                         cbPanleControl3.Text == CsConst.PanelControl[20])
                {
                    dgvP1[5, index].Value = cbbox23.Text + "(" + CsConst.mstrINIDefault.IniReadValue("public", "99874", "") + ")";
                }
                else if (cbPanleControl3.Text ==  CsConst.PanelControl[13] ||
                         cbPanleControl3.Text == CsConst.myPublicControlType[22].ControlTypeName)
                {
                    dgvP1[5, index].Value = cbbox23.Text + "(" + CsConst.mstrINIDefault.IniReadValue("public", "99876", "") + ")";
                }
                else if (cbPanleControl3.Text ==  CsConst.PanelControl[14])
                {
                    dgvP1[5, index].Value = cbbox23.Text + "(" + CsConst.mstrINIDefault.IniReadValue("public", "99875", "") + ")";
                }
                else if (cbPanleControl3.Text == CsConst.myPublicControlType[6].ControlTypeName ||
                         cbPanleControl3.Text == CsConst.myPublicControlType[9].ControlTypeName ||
                         cbPanleControl3.Text ==  CsConst.PanelControl[10] ||
                         cbPanleControl3.Text ==  CsConst.PanelControl[11])
                {
                    dgvP1[5, index].Value = cbbox23.Text;
                }
            }
            else if (cbControlType3.Text == CsConst.myPublicControlType[12].ControlTypeName)//消防模块
            {
                dgvP1[5, index].Value = cbbox23.Text;
            }
            else if (cbControlType3.Text == CsConst.myPublicControlType[13].ControlTypeName)//音乐模块
            {
                if (cbAudioControl3.SelectedIndex == 0 || cbAudioControl3.SelectedIndex == 1 ||
                    cbAudioControl3.SelectedIndex == 2 || cbAudioControl3.SelectedIndex == 3 ||
                    cbAudioControl3.SelectedIndex == 4 || cbAudioControl3.SelectedIndex == 6 ||
                    cbAudioControl3.SelectedIndex == 7)
                {
                    dgvP1[5, index].Value = cbbox23.Text;
                }
            }
            #endregion
            #region
            if (cbAudioControl3.Visible)
            {
                if (cbAudioControl3.SelectedIndex == 2)
                {
                    if (cbbox23.Visible && cbbox23.Items.Count >= 6)
                    {
                        if (cbbox23.SelectedIndex == 2 || cbbox23.SelectedIndex == 5)
                        {
                            txtbox33.Text = str3;
                            HDLSysPF.addcontrols(6, index, txtbox33, dgvP1);
                        }
                    }
                }
                else if (cbAudioControl3.SelectedIndex == 4)
                {
                    if (cbbox23.Visible && cbbox23.Items.Count > 0)
                    {
                        if (cbbox23.SelectedIndex == 0)
                        {
                            cbbox33.Items.Clear();
                            cbbox33.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00044", ""));
                            cbbox33.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00045", ""));
                            for (int i = 0; i < 80; i++)
                                cbbox33.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99872", "") + ":" + i.ToString());
                        }
                        else
                        {
                            cbbox33.Items.Clear();
                            cbbox33.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00044", ""));
                            cbbox33.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00045", ""));
                        }
                        HDLSysPF.addcontrols(6, index, cbbox33, dgvP1);
                    }
                }
                else if (cbAudioControl3.SelectedIndex == 6 || cbAudioControl3.SelectedIndex == 7)//直接源播放 广播播放
                {
                    txtbox33.Visible = true;
                }
            }
            else if (cbPanleControl3.Visible)
            {
                if ((cbPanleControl3.Text == CsConst.myPublicControlType[6].ControlTypeName ||//面板页面锁 
                     cbPanleControl3.Text == CsConst.myPublicControlType[9].ControlTypeName ||//按键锁
                     cbPanleControl3.Text ==  CsConst.PanelControl[10] ||//控制按键状态
                     cbPanleControl3.Text ==  CsConst.PanelControl[11] || //控制面板按键
                     cbPanleControl3.Text == CsConst.PanelControl[21] ||//地热开关
                     cbPanleControl3.Text == CsConst.myPublicControlType[22].ControlTypeName)) //地热模式
                {
                    cbbox33.Visible = true;
                }
            }
            #endregion
            #region
            if (cbbox33.Visible && cbbox33.Items.Count > 0)
            {
                if (!cbbox33.Items.Contains(str3))
                    cbbox33.SelectedIndex = 0;
                else
                    cbbox33.Text = str3;
            }
            #endregion
            #region
            if (txtbox33.Visible) txtbox33_TextChanged(null, null);
            if (cbbox33.Visible) cbbox33_SelectedIndexChanged(null, null);
            #endregion
            #region
            if (dgvP1.SelectedRows == null || dgvP1.SelectedRows.Count == 0) return;
            string strTmp = dgvP1[5, index].Value.ToString();
            if (strTmp == null) strTmp = "N/A";
            if (cbbox23.Focused)
                HDLSysPF.ModifyMultilinesIfNeeds(strTmp, 5, dgvP1);
            #endregion
        }

        void cbbox24_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dgvP2.CurrentRow.Index < 0) return;
            if (dgvP2.RowCount <= 0) return;
            txtbox34.Visible = false;
            cbbox34.Visible = false;
            int index = dgvP2.CurrentRow.Index;
            string str = dgvP2[5, index].Value.ToString();
            if (str.Contains("(")) str = str.Split('(')[0].ToString();
            string str3 = dgvP2[6, index].Value.ToString();
            if (str3.Contains("(")) str3 = str3.Split('(')[0].ToString();
            #region
            if (cbControlType4.Text == CsConst.myPublicControlType[4].ControlTypeName || //通用开关
                cbControlType4.Text == CsConst.myPublicControlType[6].ControlTypeName)   //窗帘开关
            {
                dgvP2[5, index].Value = cbbox24.Text + "(" + CsConst.WholeTextsList[2529].sDisplayName + ")";
                if (cbControlType4.Text == CsConst.myPublicControlType[6].ControlTypeName)
                {
                    if (cbbox24.SelectedIndex > 2)
                    {
                        txtbox14_TextChanged(null, null);
                    }
                    else
                    {
                        HDLSysPF.addcontrols(4, index, txtbox14, dgvP2);
                    }
                }
            }
            else if (cbControlType4.Text == CsConst.myPublicControlType[8].ControlTypeName)//面板控制
            {
                if (cbPanleControl4.Text == CsConst.myPublicControlType[1].ControlTypeName ||
                    cbPanleControl4.Text == CsConst.myPublicControlType[2].ControlTypeName ||
                    cbPanleControl4.Text == CsConst.myPublicControlType[5].ControlTypeName ||
                    cbPanleControl4.Text == CsConst.myPublicControlType[7].ControlTypeName ||
                    cbPanleControl4.Text == CsConst.myPublicControlType[8].ControlTypeName ||
                    cbPanleControl4.Text ==  CsConst.PanelControl[12] ||
                    cbPanleControl4.Text == CsConst.PanelControl[21])
                {
                    dgvP2[5, index].Value = cbbox24.Text + "(" + CsConst.WholeTextsList[2529].sDisplayName + ")";
                }
                else if (cbPanleControl4.Text == CsConst.PanelControl[17] ||
                         cbPanleControl4.Text == CsConst.PanelControl[18] ||
                         cbPanleControl4.Text == CsConst.PanelControl[19] ||
                         cbPanleControl4.Text == CsConst.PanelControl[20])
                {
                    dgvP2[5, index].Value = cbbox24.Text + "(" + CsConst.mstrINIDefault.IniReadValue("public", "99874", "") + ")";
                }
                else if (cbPanleControl4.Text ==  CsConst.PanelControl[13] ||
                         cbPanleControl4.Text == CsConst.myPublicControlType[22].ControlTypeName)
                {
                    dgvP2[5, index].Value = cbbox24.Text + "(" + CsConst.mstrINIDefault.IniReadValue("public", "99876", "") + ")";
                }
                else if (cbPanleControl4.Text ==  CsConst.PanelControl[14])
                {
                    dgvP2[5, index].Value = cbbox24.Text + "(" + CsConst.mstrINIDefault.IniReadValue("public", "99875", "") + ")";
                }
                else if (cbPanleControl4.Text == CsConst.myPublicControlType[6].ControlTypeName ||
                         cbPanleControl4.Text == CsConst.myPublicControlType[9].ControlTypeName ||
                         cbPanleControl4.Text ==  CsConst.PanelControl[10] ||
                         cbPanleControl4.Text ==  CsConst.PanelControl[11])
                {
                    dgvP2[5, index].Value = cbbox24.Text;
                }
            }
            else if (cbControlType4.Text == CsConst.myPublicControlType[12].ControlTypeName)//消防模块
            {
                dgvP2[5, index].Value = cbbox24.Text;
            }
            else if (cbControlType4.Text == CsConst.myPublicControlType[13].ControlTypeName)//音乐模块
            {
                if (cbAudioControl4.SelectedIndex == 0 || cbAudioControl4.SelectedIndex == 1 ||
                    cbAudioControl4.SelectedIndex == 2 || cbAudioControl4.SelectedIndex == 3 ||
                    cbAudioControl4.SelectedIndex == 4 || cbAudioControl4.SelectedIndex == 6 ||
                    cbAudioControl4.SelectedIndex == 7)
                {
                    dgvP2[5, index].Value = cbbox24.Text;
                }
            }
            #endregion
            #region
            if (cbAudioControl4.Visible)
            {
                if (cbAudioControl4.SelectedIndex == 2)
                {
                    if (cbbox24.Visible && cbbox24.Items.Count >= 6)
                    {
                        if (cbbox24.SelectedIndex == 2 || cbbox24.SelectedIndex == 5)
                        {
                            txtbox34.Text = str3;
                            HDLSysPF.addcontrols(6, index, txtbox34, dgvP2);
                        }
                    }
                }
                else if (cbAudioControl4.SelectedIndex == 4)
                {
                    if (cbbox24.Visible && cbbox24.Items.Count > 0)
                    {
                        if (cbbox24.SelectedIndex == 0)
                        {
                            cbbox34.Items.Clear();
                            cbbox34.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00044", ""));
                            cbbox34.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00045", ""));
                            for (int i = 0; i < 80; i++)
                                cbbox34.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99872", "") + ":" + i.ToString());
                        }
                        else
                        {
                            cbbox34.Items.Clear();
                            cbbox34.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00044", ""));
                            cbbox34.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00045", ""));
                        }
                        HDLSysPF.addcontrols(6, index, cbbox34, dgvP2);
                    }
                }
                else if (cbAudioControl4.SelectedIndex == 6 || cbAudioControl4.SelectedIndex == 7)//直接源播放 广播播放
                {
                    txtbox34.Visible = true;
                }
            }
            else if (cbPanleControl4.Visible)
            {
                if ((cbPanleControl4.Text == CsConst.myPublicControlType[6].ControlTypeName ||//面板页面锁 
                     cbPanleControl4.Text == CsConst.myPublicControlType[9].ControlTypeName ||//按键锁
                     cbPanleControl4.Text ==  CsConst.PanelControl[10] ||//控制按键状态
                     cbPanleControl4.Text ==  CsConst.PanelControl[11] || //控制面板按键
                     cbPanleControl4.Text == CsConst.PanelControl[21] ||//地热开关
                     cbPanleControl4.Text == CsConst.myPublicControlType[22].ControlTypeName)) //地热模式
                {
                    cbbox34.Visible = true;
                }
            }
            #endregion
            #region
            if (cbbox34.Visible && cbbox34.Items.Count > 0)
            {
                if (!cbbox34.Items.Contains(str3))
                    cbbox34.SelectedIndex = 0;
                else
                    cbbox34.Text = str3;
            }
            #endregion
            #region
            if (txtbox34.Visible) txtbox34_TextChanged(null, null);
            if (cbbox34.Visible) cbbox34_SelectedIndexChanged(null, null);
            #endregion
            #region
            if (dgvP2.SelectedRows == null || dgvP2.SelectedRows.Count == 0) return;
            string strTmp = dgvP2[5, index].Value.ToString();
            if (strTmp == null) strTmp = "N/A";
            if (cbbox24.Focused)
                HDLSysPF.ModifyMultilinesIfNeeds(strTmp, 5, dgvP2);
            #endregion
        }

        void cbbox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dgvSwitch.CurrentRow.Index < 0) return;
            if (dgvSwitch.RowCount <= 0) return;
            int index = dgvSwitch.CurrentRow.Index;
            string str = dgvSwitch[4, index].Value.ToString();
            if (str.Contains("(")) str = str.Split('(')[0].ToString();
            if (cbControlType.Text == CsConst.myPublicControlType[7].ControlTypeName)//GPRS控制
            {
                dgvSwitch[4, index].Value = cbbox1.Text;
            }
            #region
            if (dgvSwitch.SelectedRows == null || dgvSwitch.SelectedRows.Count == 0) return;
            string strTmp = dgvSwitch[4, index].Value.ToString();
            if (strTmp == null) strTmp = "N/A";
            if(cbbox1.Focused)
                HDLSysPF.ModifyMultilinesIfNeeds(strTmp, 4, dgvSwitch);
            #endregion
        }

        void cbbox12_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dgvRelay.CurrentRow.Index < 0) return;
            if (dgvRelay.RowCount <= 0) return;
            int index = dgvRelay.CurrentRow.Index;
            string str = dgvRelay[4, index].Value.ToString();
            if (str.Contains("(")) str = str.Split('(')[0].ToString();
            if (cbControlType2.Text == CsConst.myPublicControlType[7].ControlTypeName)//GPRS控制
            {
                dgvRelay[4, index].Value = cbbox12.Text;
            }
            #region
            if (dgvRelay.SelectedRows == null || dgvRelay.SelectedRows.Count == 0) return;
            string strTmp = dgvRelay[4, index].Value.ToString();
            if (strTmp == null) strTmp = "N/A";
            if (cbbox12.Focused)
                HDLSysPF.ModifyMultilinesIfNeeds(strTmp, 4, dgvRelay);
            #endregion
        }

        void cbbox13_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dgvP1.CurrentRow.Index < 0) return;
            if (dgvP1.RowCount <= 0) return;
            int index = dgvP1.CurrentRow.Index;
            string str = dgvP1[4, index].Value.ToString();
            if (str.Contains("(")) str = str.Split('(')[0].ToString();
            if (cbControlType3.Text == CsConst.myPublicControlType[7].ControlTypeName)//GPRS控制
            {
                dgvP1[4, index].Value = cbbox13.Text;
            }
            #region
            if (dgvP1.SelectedRows == null || dgvP1.SelectedRows.Count == 0) return;
            string strTmp = dgvP1[4, index].Value.ToString();
            if (strTmp == null) strTmp = "N/A";
            if (cbbox13.Focused)
                HDLSysPF.ModifyMultilinesIfNeeds(strTmp, 4, dgvP1);
            #endregion
        }

        void cbbox14_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dgvP2.CurrentRow.Index < 0) return;
            if (dgvP2.RowCount <= 0) return;
            int index = dgvP2.CurrentRow.Index;
            string str = dgvP2[4, index].Value.ToString();
            if (str.Contains("(")) str = str.Split('(')[0].ToString();
            if (cbControlType4.Text == CsConst.myPublicControlType[7].ControlTypeName)//GPRS控制
            {
                dgvP2[4, index].Value = cbbox14.Text;
            }
            #region
            if (dgvP2.SelectedRows == null || dgvP2.SelectedRows.Count == 0) return;
            string strTmp = dgvP2[4, index].Value.ToString();
            if (strTmp == null) strTmp = "N/A";
            if (cbbox14.Focused)
                HDLSysPF.ModifyMultilinesIfNeeds(strTmp, 4, dgvP2);
            #endregion
        }

        void cbAudioControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbbox2.Visible = false;
            cbbox3.Visible = false;
            txtbox2.Visible = false;
            txtbox3.Visible = false;
            int index = dgvSwitch.CurrentRow.Index;
            string str2 = dgvSwitch[5, index].Value.ToString();
            string str3 = dgvSwitch[6, index].Value.ToString();
            if (str2.Contains("(")) str2 = str2.Split('(')[0].ToString();
            if (str3.Contains("(")) str3 = str3.Split('(')[0].ToString();
            if (cbAudioControl.SelectedIndex == 0 || cbAudioControl.SelectedIndex == 1 ||//音源选择 播放模式
                cbAudioControl.SelectedIndex == 2 || cbAudioControl.SelectedIndex == 3 ||//列表/频道  播放控制
                cbAudioControl.SelectedIndex == 4)
            {
                #region
                cbbox2.Items.Clear();
                if (cbAudioControl.SelectedIndex == 0)//音源选择
                {
                    #region
                    for (int i = 1; i <= 4; i++)
                        cbbox2.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "0010" + i.ToString(), ""));
                    #endregion
                }
                else if (cbAudioControl.SelectedIndex == 1)
                {
                    #region
                    for (int i = 1; i <= 4; i++)
                        cbbox2.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "0011" + i.ToString(), ""));
                    #endregion
                }
                else if (cbAudioControl.SelectedIndex == 2)
                {
                    #region
                    for (int i = 1; i <= 6; i++)
                        cbbox2.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "0012" + i.ToString(), ""));
                    #endregion
                }
                else if (cbAudioControl.SelectedIndex == 3)
                {
                    #region
                    for (int i = 1; i <= 4; i++)
                        cbbox2.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "0013" + i.ToString(), ""));
                    #endregion
                }
                else if (cbAudioControl.SelectedIndex == 4)
                {
                    #region
                    for (int i = 1; i <= 3; i++)
                        cbbox2.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "0014" + i.ToString(), ""));
                    #endregion
                }

                HDLSysPF.addcontrols(5, index, cbbox2, dgvSwitch);
                dgvSwitch[6, index].Value = "N/A";
                #endregion
            }
            else if (cbAudioControl.SelectedIndex == 5)//歌曲播放
            {
                #region
                txtbox2.Text = str2;
                HDLSysPF.addcontrols(5, index, txtbox2, dgvSwitch);

                txtbox3.Text = str3;
                HDLSysPF.addcontrols(6, index, txtbox3, dgvSwitch);
                #endregion
            }
            else if (cbAudioControl.SelectedIndex == 6 || cbAudioControl.SelectedIndex == 7)//直接源播放 广播播放
            {
                #region
                cbbox2.Items.Clear();
                cbbox2.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00047", ""));
                for (int i = 1; i <= 48; i++)
                    cbbox2.Items.Add("SD:" + i.ToString());
                for (int i = 1; i <= 48; i++)
                    cbbox2.Items.Add("FTP:" + i.ToString());
                HDLSysPF.addcontrols(5, index, cbbox2, dgvSwitch);
                txtbox3.Text = str3;
                HDLSysPF.addcontrols(6, index, txtbox3, dgvSwitch);
                #endregion
            }
            dgvSwitch[4, index].Value = cbAudioControl.Text;
            #region
            if (cbbox2.Visible && cbbox2.Items.Count > 0)
            {
                if (!cbbox2.Items.Contains(str2))
                    cbbox2.SelectedIndex = 0;
                else
                    cbbox2.Text = str2;
            }
            if (cbbox3.Visible && cbbox3.Items.Count > 0)
            {
                if (!cbbox3.Items.Contains(str3))
                    cbbox3.SelectedIndex = 0;
                else
                    cbbox3.Text = str3;
            }
            #endregion
            #region
            if (txtbox2.Visible) txtbox2_TextChanged(null, null);
            if (txtbox3.Visible) txtbox3_TextChanged(null, null);
            if (cbbox2.Visible) cbbox2_SelectedIndexChanged(null, null);
            if (cbbox3.Visible) cbbox3_SelectedIndexChanged(null, null);
            #endregion
            if(cbAudioControl.Focused)
                HDLSysPF.ModifyMultilinesIfNeeds(dgvSwitch[4, index].Value.ToString(), 4, dgvSwitch);
        }

        void cbAudioControl2_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbbox22.Visible = false;
            cbbox32.Visible = false;
            txtbox22.Visible = false;
            txtbox32.Visible = false;
            int index = dgvRelay.CurrentRow.Index;
            string str2 = dgvRelay[5, index].Value.ToString();
            string str3 = dgvRelay[6, index].Value.ToString();
            if (str2.Contains("(")) str2 = str2.Split('(')[0].ToString();
            if (str3.Contains("(")) str3 = str3.Split('(')[0].ToString();
            if (cbAudioControl2.SelectedIndex == 0 || cbAudioControl2.SelectedIndex == 1 ||//音源选择 播放模式
                cbAudioControl2.SelectedIndex == 2 || cbAudioControl2.SelectedIndex == 3 ||//列表/频道  播放控制
                cbAudioControl2.SelectedIndex == 4)
            {
                #region
                cbbox22.Items.Clear();
                if (cbAudioControl2.SelectedIndex == 0)//音源选择
                {
                    #region
                    for (int i = 1; i <= 4; i++)
                        cbbox22.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "0010" + i.ToString(), ""));
                    #endregion
                }
                else if (cbAudioControl2.SelectedIndex == 1)
                {
                    #region
                    for (int i = 1; i <= 4; i++)
                        cbbox22.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "0011" + i.ToString(), ""));
                    #endregion
                }
                else if (cbAudioControl2.SelectedIndex == 2)
                {
                    #region
                    for (int i = 1; i <= 6; i++)
                        cbbox22.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "0012" + i.ToString(), ""));
                    #endregion
                }
                else if (cbAudioControl2.SelectedIndex == 3)
                {
                    #region
                    for (int i = 1; i <= 4; i++)
                        cbbox22.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "0013" + i.ToString(), ""));
                    #endregion
                }
                else if (cbAudioControl2.SelectedIndex == 4)
                {
                    #region
                    for (int i = 1; i <= 3; i++)
                        cbbox22.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "0014" + i.ToString(), ""));
                    #endregion
                }

                HDLSysPF.addcontrols(5, index, cbbox22, dgvRelay);
                dgvRelay[6, index].Value = "N/A";
                #endregion
            }
            else if (cbAudioControl2.SelectedIndex == 5)//歌曲播放
            {
                #region
                txtbox22.Text = str2;
                HDLSysPF.addcontrols(5, index, txtbox22, dgvRelay);

                txtbox32.Text = str3;
                HDLSysPF.addcontrols(6, index, txtbox32, dgvRelay);
                #endregion
            }
            else if (cbAudioControl2.SelectedIndex == 6 || cbAudioControl2.SelectedIndex == 7)//直接源播放 广播播放
            {
                #region
                cbbox22.Items.Clear();
                cbbox22.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00047", ""));
                for (int i = 1; i <= 48; i++)
                    cbbox22.Items.Add("SD:" + i.ToString());
                for (int i = 1; i <= 48; i++)
                    cbbox22.Items.Add("FTP:" + i.ToString());
                HDLSysPF.addcontrols(5, index, cbbox22, dgvRelay);
                txtbox32.Text = str3;
                HDLSysPF.addcontrols(6, index, txtbox32, dgvRelay);
                #endregion
            }
            dgvRelay[4, index].Value = cbAudioControl2.Text;
            #region
            if (cbbox22.Visible && cbbox22.Items.Count > 0)
            {
                if (!cbbox22.Items.Contains(str2))
                    cbbox22.SelectedIndex = 0;
                else
                    cbbox22.Text = str2;
            }
            if (cbbox32.Visible && cbbox32.Items.Count > 0)
            {
                if (!cbbox32.Items.Contains(str3))
                    cbbox32.SelectedIndex = 0;
                else
                    cbbox32.Text = str3;
            }
            #endregion
            #region
            if (txtbox22.Visible) txtbox22_TextChanged(null, null);
            if (txtbox32.Visible) txtbox32_TextChanged(null, null);
            if (cbbox22.Visible) cbbox22_SelectedIndexChanged(null, null);
            if (cbbox32.Visible) cbbox32_SelectedIndexChanged(null, null);
            #endregion
            if (cbAudioControl2.Focused)
                HDLSysPF.ModifyMultilinesIfNeeds(dgvRelay[4, index].Value.ToString(), 4, dgvRelay);
        }

        void cbAudioControl3_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbbox23.Visible = false;
            cbbox33.Visible = false;
            txtbox23.Visible = false;
            txtbox33.Visible = false;
            int index = dgvP1.CurrentRow.Index;
            string str2 = dgvP1[5, index].Value.ToString();
            string str3 = dgvP1[6, index].Value.ToString();
            if (str2.Contains("(")) str2 = str2.Split('(')[0].ToString();
            if (str3.Contains("(")) str3 = str3.Split('(')[0].ToString();
            if (cbAudioControl3.SelectedIndex == 0 || cbAudioControl3.SelectedIndex == 1 ||//音源选择 播放模式
                cbAudioControl3.SelectedIndex == 2 || cbAudioControl3.SelectedIndex == 3 ||//列表/频道  播放控制
                cbAudioControl3.SelectedIndex == 4)
            {
                #region
                cbbox23.Items.Clear();
                if (cbAudioControl3.SelectedIndex == 0)//音源选择
                {
                    #region
                    for (int i = 1; i <= 4; i++)
                        cbbox23.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "0010" + i.ToString(), ""));
                    #endregion
                }
                else if (cbAudioControl3.SelectedIndex == 1)
                {
                    #region
                    for (int i = 1; i <= 4; i++)
                        cbbox23.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "0011" + i.ToString(), ""));
                    #endregion
                }
                else if (cbAudioControl3.SelectedIndex == 2)
                {
                    #region
                    for (int i = 1; i <= 6; i++)
                        cbbox23.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "0012" + i.ToString(), ""));
                    #endregion
                }
                else if (cbAudioControl3.SelectedIndex == 3)
                {
                    #region
                    for (int i = 1; i <= 4; i++)
                        cbbox23.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "0013" + i.ToString(), ""));
                    #endregion
                }
                else if (cbAudioControl3.SelectedIndex == 4)
                {
                    #region
                    for (int i = 1; i <= 3; i++)
                        cbbox23.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "0014" + i.ToString(), ""));
                    #endregion
                }

                HDLSysPF.addcontrols(5, index, cbbox23, dgvP1);
                dgvP1[6, index].Value = "N/A";
                #endregion
            }
            else if (cbAudioControl3.SelectedIndex == 5)//歌曲播放
            {
                #region
                txtbox23.Text = str2;
                HDLSysPF.addcontrols(5, index, txtbox23, dgvP1);

                txtbox33.Text = str3;
                HDLSysPF.addcontrols(6, index, txtbox33, dgvP1);
                #endregion
            }
            else if (cbAudioControl3.SelectedIndex == 6 || cbAudioControl3.SelectedIndex == 7)//直接源播放 广播播放
            {
                #region
                cbbox23.Items.Clear();
                cbbox23.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00047", ""));
                for (int i = 1; i <= 48; i++)
                    cbbox23.Items.Add("SD:" + i.ToString());
                for (int i = 1; i <= 48; i++)
                    cbbox23.Items.Add("FTP:" + i.ToString());
                HDLSysPF.addcontrols(5, index, cbbox23, dgvP1);
                txtbox33.Text = str3;
                HDLSysPF.addcontrols(6, index, txtbox33, dgvP1);
                #endregion
            }
            dgvP1[4, index].Value = cbAudioControl3.Text;
            #region
            if (cbbox23.Visible && cbbox23.Items.Count > 0)
            {
                if (!cbbox23.Items.Contains(str2))
                    cbbox23.SelectedIndex = 0;
                else
                    cbbox23.Text = str2;
            }
            if (cbbox33.Visible && cbbox33.Items.Count > 0)
            {
                if (!cbbox33.Items.Contains(str3))
                    cbbox33.SelectedIndex = 0;
                else
                    cbbox33.Text = str3;
            }
            #endregion
            #region
            if (txtbox23.Visible) txtbox23_TextChanged(null, null);
            if (txtbox33.Visible) txtbox33_TextChanged(null, null);
            if (cbbox23.Visible) cbbox23_SelectedIndexChanged(null, null);
            if (cbbox33.Visible) cbbox33_SelectedIndexChanged(null, null);
            #endregion
            if (cbAudioControl3.Focused)
                HDLSysPF.ModifyMultilinesIfNeeds(dgvP1[4, index].Value.ToString(), 4, dgvP1);
        }

        void cbAudioControl4_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbbox24.Visible = false;
            cbbox34.Visible = false;
            txtbox24.Visible = false;
            txtbox34.Visible = false;
            int index = dgvP2.CurrentRow.Index;
            string str2 = dgvP2[5, index].Value.ToString();
            string str3 = dgvP2[6, index].Value.ToString();
            if (str2.Contains("(")) str2 = str2.Split('(')[0].ToString();
            if (str3.Contains("(")) str3 = str3.Split('(')[0].ToString();
            if (cbAudioControl4.SelectedIndex == 0 || cbAudioControl4.SelectedIndex == 1 ||//音源选择 播放模式
                cbAudioControl4.SelectedIndex == 2 || cbAudioControl4.SelectedIndex == 3 ||//列表/频道  播放控制
                cbAudioControl4.SelectedIndex == 4)
            {
                #region
                cbbox24.Items.Clear();
                if (cbAudioControl4.SelectedIndex == 0)//音源选择
                {
                    #region
                    for (int i = 1; i <= 4; i++)
                        cbbox24.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "0010" + i.ToString(), ""));
                    #endregion
                }
                else if (cbAudioControl4.SelectedIndex == 1)
                {
                    #region
                    for (int i = 1; i <= 4; i++)
                        cbbox24.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "0011" + i.ToString(), ""));
                    #endregion
                }
                else if (cbAudioControl4.SelectedIndex == 2)
                {
                    #region
                    for (int i = 1; i <= 6; i++)
                        cbbox24.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "0012" + i.ToString(), ""));
                    #endregion
                }
                else if (cbAudioControl4.SelectedIndex == 3)
                {
                    #region
                    for (int i = 1; i <= 4; i++)
                        cbbox24.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "0013" + i.ToString(), ""));
                    #endregion
                }
                else if (cbAudioControl4.SelectedIndex == 4)
                {
                    #region
                    for (int i = 1; i <= 3; i++)
                        cbbox24.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "0014" + i.ToString(), ""));
                    #endregion
                }

                HDLSysPF.addcontrols(5, index, cbbox24, dgvP2);
                dgvP2[6, index].Value = "N/A";
                #endregion
            }
            else if (cbAudioControl4.SelectedIndex == 5)//歌曲播放
            {
                #region
                txtbox24.Text = str2;
                HDLSysPF.addcontrols(5, index, txtbox24, dgvP2);

                txtbox34.Text = str3;
                HDLSysPF.addcontrols(6, index, txtbox34, dgvP2);
                #endregion
            }
            else if (cbAudioControl4.SelectedIndex == 6 || cbAudioControl4.SelectedIndex == 7)//直接源播放 广播播放
            {
                #region
                cbbox24.Items.Clear();
                cbbox24.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00047", ""));
                for (int i = 1; i <= 48; i++)
                    cbbox24.Items.Add("SD:" + i.ToString());
                for (int i = 1; i <= 48; i++)
                    cbbox24.Items.Add("FTP:" + i.ToString());
                HDLSysPF.addcontrols(5, index, cbbox24, dgvP2);
                txtbox34.Text = str3;
                HDLSysPF.addcontrols(6, index, txtbox34, dgvP2);
                #endregion
            }
            dgvP2[4, index].Value = cbAudioControl4.Text;
            #region
            if (cbbox24.Visible && cbbox24.Items.Count > 0)
            {
                if (!cbbox24.Items.Contains(str2))
                    cbbox24.SelectedIndex = 0;
                else
                    cbbox24.Text = str2;
            }
            if (cbbox34.Visible && cbbox34.Items.Count > 0)
            {
                if (!cbbox34.Items.Contains(str3))
                    cbbox34.SelectedIndex = 0;
                else
                    cbbox34.Text = str3;
            }
            #endregion
            #region
            if (txtbox24.Visible) txtbox24_TextChanged(null, null);
            if (txtbox34.Visible) txtbox34_TextChanged(null, null);
            if (cbbox24.Visible) cbbox24_SelectedIndexChanged(null, null);
            if (cbbox34.Visible) cbbox34_SelectedIndexChanged(null, null);
            #endregion
            if (cbAudioControl4.Focused)
                HDLSysPF.ModifyMultilinesIfNeeds(dgvP2[4, index].Value.ToString(), 4, dgvP2);
        }

        void cbPanleControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbbox2.Visible = false;
            cbbox3.Visible = false;
            txtbox2.Visible = false;
            txtbox3.Visible = false;
            int index = dgvSwitch.CurrentRow.Index;
            string str2 = dgvSwitch[5, index].Value.ToString();
            string str3 = dgvSwitch[6, index].Value.ToString();
            if (str2.Contains("(")) str2 = str2.Split('(')[0].ToString();
            if (str3.Contains("(")) str3 = str3.Split('(')[0].ToString();
            if (cbPanleControl.Text == CsConst.myPublicControlType[0].ControlTypeName)//无效
            {
                #region
                dgvSwitch[5, index].Value = "N/A";
                dgvSwitch[6, index].Value = "N/A";
                #endregion
            }
            else if (cbPanleControl.Text == CsConst.myPublicControlType[1].ControlTypeName ||//红外接收使能 
                     cbPanleControl.Text == CsConst.myPublicControlType[2].ControlTypeName ||//背光开关
                     cbPanleControl.Text == CsConst.myPublicControlType[5].ControlTypeName ||//面板全锁
                     cbPanleControl.Text == CsConst.myPublicControlType[7].ControlTypeName ||//空调锁
                     cbPanleControl.Text == CsConst.myPublicControlType[8].ControlTypeName ||//配置页面锁 
                     cbPanleControl.Text ==  CsConst.PanelControl[12] ||//空调开关
                     cbPanleControl.Text ==  CsConst.PanelControl[13] ||//空调模式 
                     cbPanleControl.Text ==  CsConst.PanelControl[14])  //空调风速
            {
                #region
                cbbox2.Items.Clear();
                if (cbPanleControl.Text ==  CsConst.PanelControl[13])
                {
                    for (int i = 0; i < 5; i++)
                        cbbox2.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "0005" + i.ToString(), ""));
                }
                else if (cbPanleControl.Text ==  CsConst.PanelControl[14])
                {
                    for (int i = 0; i < 4; i++)
                        cbbox2.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "0006" + i.ToString(), ""));
                }
                else
                {
                    cbbox2.Items.Add(CsConst.Status[0]);
                    cbbox2.Items.Add(CsConst.Status[1]);
                }
                HDLSysPF.addcontrols(5, index, cbbox2, dgvSwitch);
                dgvSwitch[6, index].Value = "N/A";
                #endregion
            }
            else if (cbPanleControl.Text == CsConst.myPublicControlType[3].ControlTypeName ||//背光灯亮度
                     cbPanleControl.Text == CsConst.myPublicControlType[4].ControlTypeName ||//状态灯亮度
                     cbPanleControl.Text ==  CsConst.PanelControl[15] ||//空调升高温度 
                     cbPanleControl.Text ==  CsConst.PanelControl[16])//空调降低温度
            {
                #region
                txtbox2.Text = str2;
                HDLSysPF.addcontrols(5, index, txtbox2, dgvSwitch);
                dgvSwitch[6, index].Value = "N/A";
                #endregion
            }
            else if (cbPanleControl.Text == CsConst.myPublicControlType[6].ControlTypeName ||//面板页面锁
                     cbPanleControl.Text == CsConst.myPublicControlType[9].ControlTypeName ||//按键锁
                     cbPanleControl.Text ==  CsConst.PanelControl[10] ||//控制按键状态
                     cbPanleControl.Text ==  CsConst.PanelControl[11] ||//控制面板按键
                     cbPanleControl.Text == CsConst.PanelControl[21] ||//地热开关
                     cbPanleControl.Text == CsConst.myPublicControlType[22].ControlTypeName) //地热模式
            {
                #region
                cbbox2.Items.Clear();
                cbbox3.Items.Clear();
                if (cbPanleControl.Text == CsConst.myPublicControlType[6].ControlTypeName)//面板页面锁
                {
                    #region
                    cbbox2.Items.Add(CsConst.WholeTextsList[1775].sDisplayName);
                    for (int i = 1; i <= 7; i++)
                        cbbox2.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00048", "") + i.ToString());
                    cbbox2.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00049", ""));

                    cbbox3.Items.Add(CsConst.WholeTextsList[1775].sDisplayName);
                    cbbox3.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99866", ""));
                    #endregion
                }
                else if (cbPanleControl.Text == CsConst.myPublicControlType[9].ControlTypeName)//按键锁
                {
                    #region
                    cbbox2.Items.Add(CsConst.WholeTextsList[1775].sDisplayName);
                    for (int i = 1; i <= 56; i++)
                        cbbox2.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99867", "") + i.ToString());
                    cbbox2.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00049", ""));

                    cbbox3.Items.Add(CsConst.WholeTextsList[1775].sDisplayName);
                    cbbox3.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99866", ""));
                    #endregion
                }
                else if (cbPanleControl.Text ==  CsConst.PanelControl[10])//控制按键状态
                {
                    #region
                    cbbox2.Items.Add(CsConst.WholeTextsList[1775].sDisplayName);
                    for (int i = 1; i <= 32; i++)
                        cbbox2.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99867", "") + i.ToString());
                    cbbox2.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99868", ""));
                    cbbox2.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99869", ""));
                    cbbox2.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99870", ""));
                    cbbox2.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99871", ""));
                    cbbox2.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00049", ""));

                    cbbox3.Items.Add(CsConst.WholeTextsList[1775].sDisplayName);
                    cbbox3.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99866", ""));
                    #endregion
                }
                else if (cbPanleControl.Text ==  CsConst.PanelControl[11])//控制面板按键
                {
                    #region
                    cbbox2.Items.Add(CsConst.WholeTextsList[1775].sDisplayName);
                    for (int i = 1; i <= 32; i++)
                        cbbox2.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99867", "") + i.ToString());

                    cbbox3.Items.Add(CsConst.WholeTextsList[1775].sDisplayName);
                    cbbox3.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99866", ""));
                    #endregion
                }
                else if (cbPanleControl.Text == CsConst.PanelControl[21])//地热开关
                {
                    #region
                    cbbox2.Items.Add(CsConst.Status[0]);
                    cbbox2.Items.Add(CsConst.Status[1]);

                    for (int i = 1; i <= 8; i++)
                        cbbox3.Items.Add(i.ToString());
                    cbbox3.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00049", ""));
                    #endregion
                }
                else if (cbPanleControl.Text == CsConst.myPublicControlType[22].ControlTypeName)//地热模式
                {
                    #region
                    for (int i = 0; i <= 4; i++)
                        cbbox2.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "0007" + i.ToString(), ""));

                    for (int i = 1; i <= 8; i++)
                        cbbox3.Items.Add(i.ToString());
                    cbbox3.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00049", ""));
                    #endregion
                }
                HDLSysPF.addcontrols(5, index, cbbox2,dgvSwitch);
                HDLSysPF.addcontrols(6, index, cbbox3,dgvSwitch);
                #endregion
            }
            else if (cbPanleControl.Text == CsConst.PanelControl[17] ||//空调制冷温度
                     cbPanleControl.Text == CsConst.PanelControl[18] ||//空调制热温度
                     cbPanleControl.Text == CsConst.PanelControl[19] ||//空调自动温度 
                     cbPanleControl.Text == CsConst.PanelControl[20])//空调除湿温度 
            {
                #region
                cbbox2.Items.Clear();
                for (int i = 0; i < 31; i++)
                    cbbox2.Items.Add(i.ToString() + "C");
                for (int i = 32; i < 87; i++)
                    cbbox2.Items.Add(i.ToString() + "F");
                HDLSysPF.addcontrols(5, index, cbbox2,dgvSwitch);
                dgvSwitch[6, index].Value = "N/A";
                #endregion
            }
            else if (cbPanleControl.Text == CsConst.PanelControl[23] ||//地热身高温度
                     cbPanleControl.Text == CsConst.PanelControl[24]) //地热降低温度
            {
                #region
                txtbox2.Text = str2;
                HDLSysPF.addcontrols(5, index, txtbox2,dgvSwitch);
                cbbox3.Items.Clear();
                for (int i = 1; i <= 8; i++)
                    cbbox3.Items.Add(i.ToString());
                cbbox3.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00049", ""));
                HDLSysPF.addcontrols(6, index, cbbox3,dgvSwitch);
                #endregion
            }
            dgvSwitch[4, index].Value = cbPanleControl.Text;
            #region
            if (cbbox2.Visible && cbbox2.Items.Count > 0)
            {
                if (!cbbox2.Items.Contains(str2))
                    cbbox2.SelectedIndex = 0;
                else
                    cbbox2.Text = str2;
            }
            if (cbbox3.Visible && cbbox3.Items.Count > 0)
            {
                if (!cbbox3.Items.Contains(str3))
                    cbbox3.SelectedIndex = 0;
                else
                    cbbox3.Text = str3;
            }
            #endregion
            #region
            if (txtbox2.Visible) txtbox2_TextChanged(null, null);
            if (txtbox3.Visible) txtbox3_TextChanged(null, null);
            if (cbbox2.Visible) cbbox2_SelectedIndexChanged(null, null);
            if (cbbox3.Visible) cbbox3_SelectedIndexChanged(null, null);
            #endregion
            if(cbPanleControl.Focused)
                HDLSysPF.ModifyMultilinesIfNeeds(dgvSwitch[4, index].Value.ToString(), 4, dgvSwitch);
        }

        void cbPanleControl2_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbbox22.Visible = false;
            cbbox32.Visible = false;
            txtbox22.Visible = false;
            txtbox32.Visible = false;
            int index = dgvRelay.CurrentRow.Index;
            string str2 = dgvRelay[5, index].Value.ToString();
            string str3 = dgvRelay[6, index].Value.ToString();
            if (str2.Contains("(")) str2 = str2.Split('(')[0].ToString();
            if (str3.Contains("(")) str3 = str3.Split('(')[0].ToString();
            if (cbPanleControl2.Text == CsConst.myPublicControlType[0].ControlTypeName)//无效
            {
                #region
                dgvRelay[5, index].Value = "N/A";
                dgvRelay[6, index].Value = "N/A";
                #endregion
            }
            else if (cbPanleControl2.Text == CsConst.myPublicControlType[1].ControlTypeName ||//红外接收使能 
                     cbPanleControl2.Text == CsConst.myPublicControlType[2].ControlTypeName ||//背光开关
                     cbPanleControl2.Text == CsConst.myPublicControlType[5].ControlTypeName ||//面板全锁
                     cbPanleControl2.Text == CsConst.myPublicControlType[7].ControlTypeName ||//空调锁
                     cbPanleControl2.Text == CsConst.myPublicControlType[8].ControlTypeName ||//配置页面锁 
                     cbPanleControl2.Text ==  CsConst.PanelControl[12] ||//空调开关
                     cbPanleControl2.Text ==  CsConst.PanelControl[13] ||//空调模式 
                     cbPanleControl2.Text ==  CsConst.PanelControl[14])  //空调风速
            {
                #region
                cbbox22.Items.Clear();
                if (cbPanleControl2.Text ==  CsConst.PanelControl[13])
                {
                    for (int i = 0; i < 5; i++)
                        cbbox22.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "0005" + i.ToString(), ""));
                }
                else if (cbPanleControl2.Text ==  CsConst.PanelControl[14])
                {
                    for (int i = 0; i < 4; i++)
                        cbbox22.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "0006" + i.ToString(), ""));
                }
                else
                {
                    cbbox22.Items.Add(CsConst.Status[0]);
                    cbbox22.Items.Add(CsConst.Status[1]);
                }
                HDLSysPF.addcontrols(5, index, cbbox22, dgvRelay);
                dgvRelay[6, index].Value = "N/A";
                #endregion
            }
            else if (cbPanleControl2.Text == CsConst.myPublicControlType[3].ControlTypeName ||//背光灯亮度
                     cbPanleControl2.Text == CsConst.myPublicControlType[4].ControlTypeName ||//状态灯亮度
                     cbPanleControl2.Text ==  CsConst.PanelControl[15] ||//空调升高温度 
                     cbPanleControl2.Text ==  CsConst.PanelControl[16])//空调降低温度
            {
                #region
                txtbox22.Text = str2;
                HDLSysPF.addcontrols(5, index, txtbox22, dgvRelay);
                dgvRelay[6, index].Value = "N/A";
                #endregion
            }
            else if (cbPanleControl2.Text == CsConst.myPublicControlType[6].ControlTypeName ||//面板页面锁
                     cbPanleControl2.Text == CsConst.myPublicControlType[9].ControlTypeName ||//按键锁
                     cbPanleControl2.Text ==  CsConst.PanelControl[10] ||//控制按键状态
                     cbPanleControl2.Text ==  CsConst.PanelControl[11] ||//控制面板按键
                     cbPanleControl2.Text == CsConst.PanelControl[21] ||//地热开关
                     cbPanleControl2.Text == CsConst.myPublicControlType[22].ControlTypeName) //地热模式
            {
                #region
                cbbox22.Items.Clear();
                cbbox32.Items.Clear();
                if (cbPanleControl2.Text == CsConst.myPublicControlType[6].ControlTypeName)//面板页面锁
                {
                    #region
                    cbbox22.Items.Add(CsConst.WholeTextsList[1775].sDisplayName);
                    for (int i = 1; i <= 7; i++)
                        cbbox22.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00048", "") + i.ToString());
                    cbbox22.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00049", ""));

                    cbbox32.Items.Add(CsConst.WholeTextsList[1775].sDisplayName);
                    cbbox32.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99866", ""));
                    #endregion
                }
                else if (cbPanleControl2.Text == CsConst.myPublicControlType[9].ControlTypeName)//按键锁
                {
                    #region
                    cbbox22.Items.Add(CsConst.WholeTextsList[1775].sDisplayName);
                    for (int i = 1; i <= 56; i++)
                        cbbox22.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99867", "") + i.ToString());
                    cbbox22.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00049", ""));

                    cbbox32.Items.Add(CsConst.WholeTextsList[1775].sDisplayName);
                    cbbox32.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99866", ""));
                    #endregion
                }
                else if (cbPanleControl2.Text ==  CsConst.PanelControl[10])//控制按键状态
                {
                    #region
                    cbbox22.Items.Add(CsConst.WholeTextsList[1775].sDisplayName);
                    for (int i = 1; i <= 32; i++)
                        cbbox22.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99867", "") + i.ToString());
                    cbbox22.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99868", ""));
                    cbbox22.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99869", ""));
                    cbbox22.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99870", ""));
                    cbbox22.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99871", ""));
                    cbbox22.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00049", ""));

                    cbbox32.Items.Add(CsConst.WholeTextsList[1775].sDisplayName);
                    cbbox32.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99866", ""));
                    #endregion
                }
                else if (cbPanleControl2.Text ==  CsConst.PanelControl[11])//控制面板按键
                {
                    #region
                    cbbox22.Items.Add(CsConst.WholeTextsList[1775].sDisplayName);
                    for (int i = 1; i <= 32; i++)
                        cbbox22.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99867", "") + i.ToString());

                    cbbox32.Items.Add(CsConst.WholeTextsList[1775].sDisplayName);
                    cbbox32.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99866", ""));
                    #endregion
                }
                else if (cbPanleControl2.Text == CsConst.PanelControl[21])//地热开关
                {
                    #region
                    cbbox22.Items.Add(CsConst.Status[0]);
                    cbbox22.Items.Add(CsConst.Status[1]);

                    for (int i = 1; i <= 8; i++)
                        cbbox32.Items.Add(i.ToString());
                    cbbox32.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00049", ""));
                    #endregion
                }
                else if (cbPanleControl2.Text == CsConst.myPublicControlType[22].ControlTypeName)//地热模式
                {
                    #region
                    for (int i = 0; i <= 4; i++)
                        cbbox22.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "0007" + i.ToString(), ""));

                    for (int i = 1; i <= 8; i++)
                        cbbox32.Items.Add(i.ToString());
                    cbbox32.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00049", ""));
                    #endregion
                }
                HDLSysPF.addcontrols(5, index, cbbox22, dgvRelay);
                HDLSysPF.addcontrols(6, index, cbbox32, dgvRelay);
                #endregion
            }
            else if (cbPanleControl2.Text == CsConst.PanelControl[17] ||//空调制冷温度
                     cbPanleControl2.Text == CsConst.PanelControl[18] ||//空调制热温度
                     cbPanleControl2.Text == CsConst.PanelControl[19] ||//空调自动温度 
                     cbPanleControl2.Text == CsConst.PanelControl[20])//空调除湿温度 
            {
                #region
                cbbox22.Items.Clear();
                for (int i = 0; i < 31; i++)
                    cbbox22.Items.Add(i.ToString() + "C");
                for (int i = 32; i < 87; i++)
                    cbbox22.Items.Add(i.ToString() + "F");
                HDLSysPF.addcontrols(5, index, cbbox22, dgvRelay);
                dgvRelay[6, index].Value = "N/A";
                #endregion
            }
            else if (cbPanleControl2.Text == CsConst.PanelControl[23] ||//地热身高温度
                     cbPanleControl2.Text == CsConst.PanelControl[24]) //地热降低温度
            {
                #region
                txtbox22.Text = str2;
                HDLSysPF.addcontrols(5, index, txtbox22, dgvRelay);
                cbbox32.Items.Clear();
                for (int i = 1; i <= 8; i++)
                    cbbox32.Items.Add(i.ToString());
                cbbox32.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00049", ""));
                HDLSysPF.addcontrols(6, index, cbbox32, dgvRelay);
                #endregion
            }
            dgvRelay[4, index].Value = cbPanleControl2.Text;
            #region
            if (cbbox22.Visible && cbbox22.Items.Count > 0)
            {
                if (!cbbox22.Items.Contains(str2))
                    cbbox22.SelectedIndex = 0;
                else
                    cbbox22.Text = str2;
            }
            if (cbbox32.Visible && cbbox32.Items.Count > 0)
            {
                if (!cbbox32.Items.Contains(str3))
                    cbbox32.SelectedIndex = 0;
                else
                    cbbox32.Text = str3;
            }
            #endregion
            #region
            if (txtbox22.Visible) txtbox22_TextChanged(null, null);
            if (txtbox32.Visible) txtbox32_TextChanged(null, null);
            if (cbbox22.Visible) cbbox22_SelectedIndexChanged(null, null);
            if (cbbox32.Visible) cbbox32_SelectedIndexChanged(null, null);
            #endregion
            if (cbPanleControl2.Focused)
                HDLSysPF.ModifyMultilinesIfNeeds(dgvRelay[4, index].Value.ToString(), 4, dgvRelay);
        }

        void cbPanleControl3_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbbox23.Visible = false;
            cbbox33.Visible = false;
            txtbox23.Visible = false;
            txtbox33.Visible = false;
            int index = dgvP1.CurrentRow.Index;
            string str2 = dgvP1[5, index].Value.ToString();
            string str3 = dgvP1[6, index].Value.ToString();
            if (str2.Contains("(")) str2 = str2.Split('(')[0].ToString();
            if (str3.Contains("(")) str3 = str3.Split('(')[0].ToString();
            if (cbPanleControl3.Text == CsConst.myPublicControlType[0].ControlTypeName)//无效
            {
                #region
                dgvP1[5, index].Value = "N/A";
                dgvP1[6, index].Value = "N/A";
                #endregion
            }
            else if (cbPanleControl3.Text == CsConst.myPublicControlType[1].ControlTypeName ||//红外接收使能 
                     cbPanleControl3.Text == CsConst.myPublicControlType[2].ControlTypeName ||//背光开关
                     cbPanleControl3.Text == CsConst.myPublicControlType[5].ControlTypeName ||//面板全锁
                     cbPanleControl3.Text == CsConst.myPublicControlType[7].ControlTypeName ||//空调锁
                     cbPanleControl3.Text == CsConst.myPublicControlType[8].ControlTypeName ||//配置页面锁 
                     cbPanleControl3.Text ==  CsConst.PanelControl[12] ||//空调开关
                     cbPanleControl3.Text ==  CsConst.PanelControl[13] ||//空调模式 
                     cbPanleControl3.Text ==  CsConst.PanelControl[14])  //空调风速
            {
                #region
                cbbox23.Items.Clear();
                if (cbPanleControl3.Text ==  CsConst.PanelControl[13])
                {
                    for (int i = 0; i < 5; i++)
                        cbbox23.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "0005" + i.ToString(), ""));
                }
                else if (cbPanleControl3.Text ==  CsConst.PanelControl[14])
                {
                    for (int i = 0; i < 4; i++)
                        cbbox23.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "0006" + i.ToString(), ""));
                }
                else
                {
                    cbbox23.Items.Add(CsConst.Status[0]);
                    cbbox23.Items.Add(CsConst.Status[1]);
                }
                HDLSysPF.addcontrols(5, index, cbbox23, dgvP1);
                dgvP1[6, index].Value = "N/A";
                #endregion
            }
            else if (cbPanleControl3.Text == CsConst.myPublicControlType[3].ControlTypeName ||//背光灯亮度
                     cbPanleControl3.Text == CsConst.myPublicControlType[4].ControlTypeName ||//状态灯亮度
                     cbPanleControl3.Text ==  CsConst.PanelControl[15] ||//空调升高温度 
                     cbPanleControl3.Text ==  CsConst.PanelControl[16])//空调降低温度
            {
                #region
                txtbox23.Text = str2;
                HDLSysPF.addcontrols(5, index, txtbox23, dgvP1);
                dgvP1[6, index].Value = "N/A";
                #endregion
            }
            else if (cbPanleControl3.Text == CsConst.myPublicControlType[6].ControlTypeName ||//面板页面锁
                     cbPanleControl3.Text == CsConst.myPublicControlType[9].ControlTypeName ||//按键锁
                     cbPanleControl3.Text ==  CsConst.PanelControl[10] ||//控制按键状态
                     cbPanleControl3.Text ==  CsConst.PanelControl[11] ||//控制面板按键
                     cbPanleControl3.Text == CsConst.PanelControl[21] ||//地热开关
                     cbPanleControl3.Text == CsConst.myPublicControlType[22].ControlTypeName) //地热模式
            {
                #region
                cbbox23.Items.Clear();
                cbbox33.Items.Clear();
                if (cbPanleControl3.Text == CsConst.myPublicControlType[6].ControlTypeName)//面板页面锁
                {
                    #region
                    cbbox23.Items.Add(CsConst.WholeTextsList[1775].sDisplayName);
                    for (int i = 1; i <= 7; i++)
                        cbbox23.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00048", "") + i.ToString());
                    cbbox23.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00049", ""));

                    cbbox33.Items.Add(CsConst.WholeTextsList[1775].sDisplayName);
                    cbbox33.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99866", ""));
                    #endregion
                }
                else if (cbPanleControl3.Text == CsConst.myPublicControlType[9].ControlTypeName)//按键锁
                {
                    #region
                    cbbox23.Items.Add(CsConst.WholeTextsList[1775].sDisplayName);
                    for (int i = 1; i <= 56; i++)
                        cbbox23.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99867", "") + i.ToString());
                    cbbox23.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00049", ""));

                    cbbox33.Items.Add(CsConst.WholeTextsList[1775].sDisplayName);
                    cbbox33.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99866", ""));
                    #endregion
                }
                else if (cbPanleControl3.Text ==  CsConst.PanelControl[10])//控制按键状态
                {
                    #region
                    cbbox23.Items.Add(CsConst.WholeTextsList[1775].sDisplayName);
                    for (int i = 1; i <= 32; i++)
                        cbbox23.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99867", "") + i.ToString());
                    cbbox23.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99868", ""));
                    cbbox23.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99869", ""));
                    cbbox23.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99870", ""));
                    cbbox23.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99871", ""));
                    cbbox23.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00049", ""));

                    cbbox33.Items.Add(CsConst.WholeTextsList[1775].sDisplayName);
                    cbbox33.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99866", ""));
                    #endregion
                }
                else if (cbPanleControl3.Text ==  CsConst.PanelControl[11])//控制面板按键
                {
                    #region
                    cbbox23.Items.Add(CsConst.WholeTextsList[1775].sDisplayName);
                    for (int i = 1; i <= 32; i++)
                        cbbox23.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99867", "") + i.ToString());

                    cbbox33.Items.Add(CsConst.WholeTextsList[1775].sDisplayName);
                    cbbox33.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99866", ""));
                    #endregion
                }
                else if (cbPanleControl3.Text == CsConst.PanelControl[21])//地热开关
                {
                    #region
                    cbbox23.Items.Add(CsConst.Status[0]);
                    cbbox23.Items.Add(CsConst.Status[1]);

                    for (int i = 1; i <= 8; i++)
                        cbbox33.Items.Add(i.ToString());
                    cbbox33.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00049", ""));
                    #endregion
                }
                else if (cbPanleControl3.Text == CsConst.myPublicControlType[22].ControlTypeName)//地热模式
                {
                    #region
                    for (int i = 0; i <= 4; i++)
                        cbbox23.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "0007" + i.ToString(), ""));

                    for (int i = 1; i <= 8; i++)
                        cbbox33.Items.Add(i.ToString());
                    cbbox33.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00049", ""));
                    #endregion
                }
                HDLSysPF.addcontrols(5, index, cbbox23, dgvP1);
                HDLSysPF.addcontrols(6, index, cbbox33, dgvP1);
                #endregion
            }
            else if (cbPanleControl3.Text == CsConst.PanelControl[17] ||//空调制冷温度
                     cbPanleControl3.Text == CsConst.PanelControl[18] ||//空调制热温度
                     cbPanleControl3.Text == CsConst.PanelControl[19] ||//空调自动温度 
                     cbPanleControl3.Text == CsConst.PanelControl[20])//空调除湿温度 
            {
                #region
                cbbox23.Items.Clear();
                for (int i = 0; i < 31; i++)
                    cbbox23.Items.Add(i.ToString() + "C");
                for (int i = 32; i < 87; i++)
                    cbbox23.Items.Add(i.ToString() + "F");
                HDLSysPF.addcontrols(5, index, cbbox23, dgvP1);
                dgvP1[6, index].Value = "N/A";
                #endregion
            }
            else if (cbPanleControl3.Text == CsConst.PanelControl[23] ||//地热身高温度
                     cbPanleControl3.Text == CsConst.PanelControl[24]) //地热降低温度
            {
                #region
                txtbox23.Text = str2;
                HDLSysPF.addcontrols(5, index, txtbox23, dgvP1);
                cbbox33.Items.Clear();
                for (int i = 1; i <= 8; i++)
                    cbbox33.Items.Add(i.ToString());
                cbbox33.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00049", ""));
                HDLSysPF.addcontrols(6, index, cbbox33, dgvP1);
                #endregion
            }
            dgvP1[4, index].Value = cbPanleControl3.Text;
            #region
            if (cbbox23.Visible && cbbox23.Items.Count > 0)
            {
                if (!cbbox23.Items.Contains(str2))
                    cbbox23.SelectedIndex = 0;
                else
                    cbbox23.Text = str2;
            }
            if (cbbox33.Visible && cbbox33.Items.Count > 0)
            {
                if (!cbbox33.Items.Contains(str3))
                    cbbox33.SelectedIndex = 0;
                else
                    cbbox33.Text = str3;
            }
            #endregion
            #region
            if (txtbox23.Visible) txtbox23_TextChanged(null, null);
            if (txtbox33.Visible) txtbox33_TextChanged(null, null);
            if (cbbox23.Visible) cbbox23_SelectedIndexChanged(null, null);
            if (cbbox33.Visible) cbbox33_SelectedIndexChanged(null, null);
            #endregion
            if (cbPanleControl3.Focused)
                HDLSysPF.ModifyMultilinesIfNeeds(dgvP1[4, index].Value.ToString(), 4, dgvP1);
        }

        void cbPanleControl4_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbbox24.Visible = false;
            cbbox34.Visible = false;
            txtbox24.Visible = false;
            txtbox34.Visible = false;
            int index = dgvP2.CurrentRow.Index;
            string str2 = dgvP2[5, index].Value.ToString();
            string str3 = dgvP2[6, index].Value.ToString();
            if (str2.Contains("(")) str2 = str2.Split('(')[0].ToString();
            if (str3.Contains("(")) str3 = str3.Split('(')[0].ToString();
            if (cbPanleControl4.Text == CsConst.myPublicControlType[0].ControlTypeName)//无效
            {
                #region
                dgvP2[5, index].Value = "N/A";
                dgvP2[6, index].Value = "N/A";
                #endregion
            }
            else if (cbPanleControl4.Text == CsConst.myPublicControlType[1].ControlTypeName ||//红外接收使能 
                     cbPanleControl4.Text == CsConst.myPublicControlType[2].ControlTypeName ||//背光开关
                     cbPanleControl4.Text == CsConst.myPublicControlType[5].ControlTypeName ||//面板全锁
                     cbPanleControl4.Text == CsConst.myPublicControlType[7].ControlTypeName ||//空调锁
                     cbPanleControl4.Text == CsConst.myPublicControlType[8].ControlTypeName ||//配置页面锁 
                     cbPanleControl4.Text ==  CsConst.PanelControl[12] ||//空调开关
                     cbPanleControl4.Text ==  CsConst.PanelControl[13] ||//空调模式 
                     cbPanleControl4.Text ==  CsConst.PanelControl[14])  //空调风速
            {
                #region
                cbbox24.Items.Clear();
                if (cbPanleControl4.Text ==  CsConst.PanelControl[13])
                {
                    for (int i = 0; i < 5; i++)
                        cbbox24.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "0005" + i.ToString(), ""));
                }
                else if (cbPanleControl4.Text ==  CsConst.PanelControl[14])
                {
                    for (int i = 0; i < 4; i++)
                        cbbox24.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "0006" + i.ToString(), ""));
                }
                else
                {
                    cbbox24.Items.Add(CsConst.Status[0]);
                    cbbox24.Items.Add(CsConst.Status[1]);
                }
                HDLSysPF.addcontrols(5, index, cbbox24, dgvP2);
                dgvP2[6, index].Value = "N/A";
                #endregion
            }
            else if (cbPanleControl3.Text == CsConst.myPublicControlType[3].ControlTypeName ||//背光灯亮度
                     cbPanleControl4.Text == CsConst.myPublicControlType[4].ControlTypeName ||//状态灯亮度
                     cbPanleControl4.Text ==  CsConst.PanelControl[15] ||//空调升高温度 
                     cbPanleControl4.Text ==  CsConst.PanelControl[16])//空调降低温度
            {
                #region
                txtbox24.Text = str2;
                HDLSysPF.addcontrols(5, index, txtbox24, dgvP2);
                dgvP2[6, index].Value = "N/A";
                #endregion
            }
            else if (cbPanleControl4.Text == CsConst.myPublicControlType[6].ControlTypeName ||//面板页面锁
                     cbPanleControl4.Text == CsConst.myPublicControlType[9].ControlTypeName ||//按键锁
                     cbPanleControl4.Text ==  CsConst.PanelControl[10] ||//控制按键状态
                     cbPanleControl4.Text ==  CsConst.PanelControl[11] ||//控制面板按键
                     cbPanleControl4.Text == CsConst.PanelControl[21] ||//地热开关
                     cbPanleControl4.Text == CsConst.myPublicControlType[22].ControlTypeName) //地热模式
            {
                #region
                cbbox24.Items.Clear();
                cbbox34.Items.Clear();
                if (cbPanleControl4.Text == CsConst.myPublicControlType[6].ControlTypeName)//面板页面锁
                {
                    #region
                    cbbox24.Items.Add(CsConst.WholeTextsList[1775].sDisplayName);
                    for (int i = 1; i <= 7; i++)
                        cbbox24.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00048", "") + i.ToString());
                    cbbox24.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00049", ""));

                    cbbox34.Items.Add(CsConst.WholeTextsList[1775].sDisplayName);
                    cbbox34.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99866", ""));
                    #endregion
                }
                else if (cbPanleControl4.Text == CsConst.myPublicControlType[9].ControlTypeName)//按键锁
                {
                    #region
                    cbbox24.Items.Add(CsConst.WholeTextsList[1775].sDisplayName);
                    for (int i = 1; i <= 56; i++)
                        cbbox24.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99867", "") + i.ToString());
                    cbbox24.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00049", ""));

                    cbbox34.Items.Add(CsConst.WholeTextsList[1775].sDisplayName);
                    cbbox34.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99866", ""));
                    #endregion
                }
                else if (cbPanleControl4.Text ==  CsConst.PanelControl[10])//控制按键状态
                {
                    #region
                    cbbox24.Items.Add(CsConst.WholeTextsList[1775].sDisplayName);
                    for (int i = 1; i <= 32; i++)
                        cbbox24.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99867", "") + i.ToString());
                    cbbox24.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99868", ""));
                    cbbox24.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99869", ""));
                    cbbox24.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99870", ""));
                    cbbox24.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99871", ""));
                    cbbox24.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00049", ""));

                    cbbox34.Items.Add(CsConst.WholeTextsList[1775].sDisplayName);
                    cbbox34.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99866", ""));
                    #endregion
                }
                else if (cbPanleControl4.Text ==  CsConst.PanelControl[11])//控制面板按键
                {
                    #region
                    cbbox24.Items.Add(CsConst.WholeTextsList[1775].sDisplayName);
                    for (int i = 1; i <= 32; i++)
                        cbbox24.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99867", "") + i.ToString());

                    cbbox34.Items.Add(CsConst.WholeTextsList[1775].sDisplayName);
                    cbbox34.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99866", ""));
                    #endregion
                }
                else if (cbPanleControl4.Text == CsConst.PanelControl[21])//地热开关
                {
                    #region
                    cbbox24.Items.Add(CsConst.Status[0]);
                    cbbox24.Items.Add(CsConst.Status[1]);

                    for (int i = 1; i <= 8; i++)
                        cbbox34.Items.Add(i.ToString());
                    cbbox34.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00049", ""));
                    #endregion
                }
                else if (cbPanleControl4.Text == CsConst.myPublicControlType[22].ControlTypeName)//地热模式
                {
                    #region
                    for (int i = 0; i <= 4; i++)
                        cbbox24.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "0007" + i.ToString(), ""));

                    for (int i = 1; i <= 8; i++)
                        cbbox34.Items.Add(i.ToString());
                    cbbox34.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00049", ""));
                    #endregion
                }
                HDLSysPF.addcontrols(5, index, cbbox24, dgvP2);
                HDLSysPF.addcontrols(6, index, cbbox34, dgvP2);
                #endregion
            }
            else if (cbPanleControl4.Text == CsConst.PanelControl[17] ||//空调制冷温度
                     cbPanleControl4.Text == CsConst.PanelControl[18] ||//空调制热温度
                     cbPanleControl4.Text == CsConst.PanelControl[19] ||//空调自动温度 
                     cbPanleControl4.Text == CsConst.PanelControl[20])//空调除湿温度 
            {
                #region
                cbbox24.Items.Clear();
                for (int i = 0; i < 31; i++)
                    cbbox24.Items.Add(i.ToString() + "C");
                for (int i = 32; i < 87; i++)
                    cbbox24.Items.Add(i.ToString() + "F");
                HDLSysPF.addcontrols(5, index, cbbox24, dgvP2);
                dgvP2[6, index].Value = "N/A";
                #endregion
            }
            else if (cbPanleControl4.Text == CsConst.PanelControl[23] ||//地热身高温度
                     cbPanleControl4.Text == CsConst.PanelControl[24]) //地热降低温度
            {
                #region
                txtbox24.Text = str2;
                HDLSysPF.addcontrols(5, index, txtbox24, dgvP2);
                cbbox34.Items.Clear();
                for (int i = 1; i <= 8; i++)
                    cbbox34.Items.Add(i.ToString());
                cbbox34.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00049", ""));
                HDLSysPF.addcontrols(6, index, cbbox34, dgvP2);
                #endregion
            }
            dgvP2[4, index].Value = cbPanleControl4.Text;
            #region
            if (cbbox24.Visible && cbbox24.Items.Count > 0)
            {
                if (!cbbox24.Items.Contains(str2))
                    cbbox24.SelectedIndex = 0;
                else
                    cbbox24.Text = str2;
            }
            if (cbbox34.Visible && cbbox34.Items.Count > 0)
            {
                if (!cbbox34.Items.Contains(str3))
                    cbbox34.SelectedIndex = 0;
                else
                    cbbox34.Text = str3;
            }
            #endregion
            #region
            if (txtbox24.Visible) txtbox24_TextChanged(null, null);
            if (txtbox34.Visible) txtbox34_TextChanged(null, null);
            if (cbbox24.Visible) cbbox24_SelectedIndexChanged(null, null);
            if (cbbox34.Visible) cbbox34_SelectedIndexChanged(null, null);
            #endregion
            if (cbPanleControl4.Focused)
                HDLSysPF.ModifyMultilinesIfNeeds(dgvP2[4, index].Value.ToString(), 4, dgvP2);
        }

        void cbControlType_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbPanleControl.Visible = false;
            cbAudioControl.Visible = false;
            cbbox1.Visible = false;
            cbbox2.Visible = false;
            cbbox3.Visible = false;
            txtbox1.Visible = false;
            txtbox2.Visible = false;
            txtbox3.Visible = false;
            txtSeries.Visible = false;
            int index = dgvSwitch.CurrentRow.Index;
            string str1 = dgvSwitch[4, index].Value.ToString();
            string str2 = dgvSwitch[5, index].Value.ToString();
            string str3 = dgvSwitch[6, index].Value.ToString();
            if (str1.Contains('(')) str1 = str1.Split('(')[0].ToString();
            if (str2.Contains('(')) str2 = str2.Split('(')[0].ToString();
            if (str3.Contains('(')) str3 = str3.Split('(')[0].ToString();
            if (cbControlType.Text == CsConst.mstrINIDefault.IniReadValue("TargetType", "00000", ""))//无效
            {
                #region
                if (dgvSwitch.SelectedRows.Count > 0)
                {
                    for (int i = 0; i < dgvSwitch.SelectedRows.Count; i++)
                    {
                        dgvSwitch.SelectedRows[i].Cells[3].Value = cbControlType.Items[0].ToString();
                        dgvSwitch[4, index].Value = "N/A";
                        dgvSwitch[5, index].Value = "N/A";
                        dgvSwitch[6, index].Value = "N/A";
                    }
                }
                #endregion
            }
            else if (cbControlType.Text == CsConst.myPublicControlType[1].ControlTypeName || //场景 
                     cbControlType.Text == CsConst.myPublicControlType[2].ControlTypeName)   //序列
            {
                #region
                txtbox1.Text = str1;
                HDLSysPF.addcontrols(4, index, txtbox1,dgvSwitch);

                txtbox2.Text = str2;
                HDLSysPF.addcontrols(5, index, txtbox2,dgvSwitch);
                dgvSwitch[6, index].Value = "N/A";
                #endregion
            }
            else if (cbControlType.Text == CsConst.myPublicControlType[4].ControlTypeName || //通用开关
                     cbControlType.Text == CsConst.myPublicControlType[6].ControlTypeName || //窗帘开关 
                     cbControlType.Text == CsConst.myPublicControlType[12].ControlTypeName)   //消防模块
            {
                #region
                txtbox1.Text = str1;
                HDLSysPF.addcontrols(4, index, txtbox1,dgvSwitch);

                cbbox2.Items.Clear();
                if (cbControlType.Text == CsConst.myPublicControlType[4].ControlTypeName)
                {
                    cbbox2.Items.Add(CsConst.Status[0]);
                    cbbox2.Items.Add(CsConst.Status[1]);
                }
                else if (cbControlType.Text == CsConst.myPublicControlType[6].ControlTypeName)
                {
                    cbbox2.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00036", ""));
                    cbbox2.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00037", ""));
                    cbbox2.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00038", ""));
                    for (int i = 1; i <= 100; i++)
                        cbbox2.Items.Add(i.ToString() + "%");
                }
                else if (cbControlType.Text == CsConst.myPublicControlType[12].ControlTypeName)
                {
                    for (int i = 0; i < 10; i++)
                        cbbox2.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "0008" + i.ToString(), ""));
                }
                HDLSysPF.addcontrols(5, index, cbbox2,dgvSwitch);
                dgvSwitch[6, index].Value = "N/A";
                #endregion
            }
            else if (cbControlType.Text == CsConst.myPublicControlType[5].ControlTypeName)//单路调节
            {
                #region
                txtbox1.Text = str1;
                HDLSysPF.addcontrols(4, index, txtbox1,dgvSwitch);

                txtbox2.Text = str2;
                HDLSysPF.addcontrols(5, index, txtbox2,dgvSwitch);


                if (!str3.Contains(":"))
                    txtSeries.Text = "0:0";
                else
                {
                    if (HDLPF.IsRightNumStringMode(str3.Split(':')[0].ToString(), 0, 255) &&
                        HDLPF.IsRightNumStringMode(str3.Split(':')[1].ToString(), 0, 255))
                        txtSeries.Text = HDLPF.GetTimeFromString(str3, ':');
                    else
                        txtSeries.Text = "0:0";
                }
                HDLSysPF.addcontrols(6, index, txtSeries,dgvSwitch);
                #endregion
            }
            else if (cbControlType.Text == CsConst.myPublicControlType[7].ControlTypeName)//GPRS控制
            {
                #region
                cbbox1.Items.Clear();
                cbbox1.Items.Add(CsConst.WholeTextsList[1775].sDisplayName);
                cbbox1.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99862", ""));
                cbbox1.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99863", ""));
                HDLSysPF.addcontrols(4, index, cbbox1,dgvSwitch);

                txtbox2.Text = str2;
                HDLSysPF.addcontrols(5, index, txtbox2,dgvSwitch);
                dgvSwitch[6, index].Value = "N/A";
                #endregion
            }
            else if (cbControlType.Text == CsConst.myPublicControlType[8].ControlTypeName)//面板控制
            {
                #region
                HDLSysPF.addcontrols(4, index, cbPanleControl,dgvSwitch);
                #endregion
            }
            else if (cbControlType.Text == CsConst.myPublicControlType[10].ControlTypeName || //广播场景
                     cbControlType.Text == CsConst.myPublicControlType[11].ControlTypeName)   //广播回路
            {
                #region
                if (cbControlType.Text == CsConst.myPublicControlType[10].ControlTypeName)
                {
                    dgvSwitch[4, index].Value = CsConst.WholeTextsList[2566].sDisplayName;
                    dgvSwitch[6, index].Value = "N/A";
                }
                else if (cbControlType.Text == CsConst.myPublicControlType[11].ControlTypeName)
                {
                    dgvSwitch[4, index].Value = CsConst.WholeTextsList[2567].sDisplayName;
                    if (!str3.Contains(":"))
                        txtSeries.Text = "0:0";
                    else
                    {
                        if (HDLPF.IsRightNumStringMode(str3.Split(':')[0].ToString(), 0, 255) &&
                        HDLPF.IsRightNumStringMode(str3.Split(':')[1].ToString(), 0, 255))
                            txtSeries.Text = HDLPF.GetTimeFromString(str3, ':');
                        else
                            txtSeries.Text = "0:0";
                    }
                    HDLSysPF.addcontrols(6, index, txtSeries,dgvSwitch);
                }
                txtbox2.Text = str2;
                HDLSysPF.addcontrols(5, index, txtbox2,dgvSwitch);
                #endregion
            }
            else if (cbControlType.Text == CsConst.myPublicControlType[13].ControlTypeName)//音乐播放
            {
                #region
                HDLSysPF.addcontrols(4, index, cbAudioControl,dgvSwitch);
                #endregion
            }
            dgvSwitch[3, index].Value = cbControlType.Text;
            #region
            if (cbbox1.Visible && cbbox1.Items.Count > 0)
            {
                if (!cbbox1.Items.Contains(str1))
                    cbbox1.SelectedIndex = 0;
                else
                    cbbox1.Text = str1;
            }
            if (cbbox2.Visible && cbbox2.Items.Count > 0)
            {
                if (!cbbox2.Items.Contains(str2))
                    cbbox2.SelectedIndex = 0;
                else
                    cbbox2.Text = str2;
            }
            if (cbbox3.Visible && cbbox3.Items.Count > 0)
            {
                if (!cbbox3.Items.Contains(str3))
                    cbbox3.SelectedIndex = 0;
                else
                    cbbox3.Text = str3;
            }
            if (cbPanleControl.Visible && cbPanleControl.Items.Count > 0)
            {
                if (!cbPanleControl.Items.Contains(str1))
                    cbPanleControl.SelectedIndex = 0;
                else
                    cbPanleControl.Text = str1;
            }
            if (cbAudioControl.Visible && cbAudioControl.Items.Count > 0)
            {
                if (!cbAudioControl.Items.Contains(str1))
                    cbAudioControl.SelectedIndex = 0;
                else
                    cbAudioControl.Text = str1;
            }
            #endregion
            #region
            if (txtbox1.Visible) txtbox1_TextChanged(null, null);
            if (txtbox2.Visible) txtbox2_TextChanged(null, null);
            if (txtSeries.Visible) txtSeries_TextChanged(null, null);
            if (cbbox1.Visible) cbbox1_SelectedIndexChanged(null, null);
            if (cbbox2.Visible) cbbox2_SelectedIndexChanged(null, null);
            if (cbbox3.Visible) cbbox3_SelectedIndexChanged(null, null);
            if (cbPanleControl.Visible) cbPanleControl_SelectedIndexChanged(null, null);
            if (cbAudioControl.Visible) cbAudioControl_SelectedIndexChanged(null, null);
            #endregion
            if (cbControlType.Focused)
                HDLSysPF.ModifyMultilinesIfNeeds(dgvSwitch[3, index].Value.ToString(), 3, dgvSwitch);
        }

        void cbControlType2_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbPanleControl2.Visible = false;
            cbAudioControl2.Visible = false;
            cbbox12.Visible = false;
            cbbox22.Visible = false;
            cbbox32.Visible = false;
            txtbox12.Visible = false;
            txtbox22.Visible = false;
            txtbox32.Visible = false;
            txtSeries2.Visible = false;
            int index = dgvRelay.CurrentRow.Index;
            string str1 = dgvRelay[4, index].Value.ToString();
            string str2 = dgvRelay[5, index].Value.ToString();
            string str3 = dgvRelay[6, index].Value.ToString();
            if (str1.Contains('(')) str1 = str1.Split('(')[0].ToString();
            if (str2.Contains('(')) str2 = str2.Split('(')[0].ToString();
            if (str3.Contains('(')) str3 = str3.Split('(')[0].ToString();
            if (cbControlType2.Text == CsConst.mstrINIDefault.IniReadValue("TargetType", "00000", ""))//无效
            {
                #region
                if (dgvRelay.SelectedRows.Count > 0)
                {
                    for (int i = 0; i < dgvRelay.SelectedRows.Count; i++)
                    {
                        dgvRelay.SelectedRows[i].Cells[3].Value = cbControlType2.Items[0].ToString();
                        dgvRelay[4, index].Value = "N/A";
                        dgvRelay[5, index].Value = "N/A";
                        dgvRelay[6, index].Value = "N/A";
                    }
                }
                #endregion
            }
            else if (cbControlType2.Text == CsConst.myPublicControlType[1].ControlTypeName || //场景 
                     cbControlType2.Text == CsConst.myPublicControlType[2].ControlTypeName)   //序列
            {
                #region
                txtbox12.Text = str1;
                HDLSysPF.addcontrols(4, index, txtbox12, dgvRelay);

                txtbox22.Text = str2;
                HDLSysPF.addcontrols(5, index, txtbox22, dgvRelay);
                dgvRelay[6, index].Value = "N/A";
                #endregion
            }
            else if (cbControlType2.Text == CsConst.myPublicControlType[4].ControlTypeName || //通用开关
                     cbControlType2.Text == CsConst.myPublicControlType[6].ControlTypeName || //窗帘开关 
                     cbControlType2.Text == CsConst.myPublicControlType[12].ControlTypeName)   //消防模块
            {
                #region
                txtbox12.Text = str1;
                HDLSysPF.addcontrols(4, index, txtbox12, dgvRelay);

                cbbox22.Items.Clear();
                if (cbControlType2.Text == CsConst.myPublicControlType[4].ControlTypeName)
                {
                    cbbox22.Items.Add(CsConst.Status[0]);
                    cbbox22.Items.Add(CsConst.Status[1]);
                }
                else if (cbControlType2.Text == CsConst.myPublicControlType[6].ControlTypeName)
                {
                    cbbox22.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00036", ""));
                    cbbox22.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00037", ""));
                    cbbox22.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00038", ""));
                    for (int i = 1; i <= 100; i++)
                        cbbox22.Items.Add(i.ToString() + "%");
                }
                else if (cbControlType2.Text == CsConst.myPublicControlType[12].ControlTypeName)
                {
                    for (int i = 0; i < 10; i++)
                        cbbox22.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "0008" + i.ToString(), ""));
                }
                HDLSysPF.addcontrols(5, index, cbbox22, dgvRelay);
                dgvRelay[6, index].Value = "N/A";
                #endregion
            }
            else if (cbControlType2.Text == CsConst.myPublicControlType[5].ControlTypeName)//单路调节
            {
                #region
                txtbox12.Text = str1;
                HDLSysPF.addcontrols(4, index, txtbox12, dgvRelay);

                txtbox22.Text = str2;
                HDLSysPF.addcontrols(5, index, txtbox22, dgvRelay);


                if (!str3.Contains(":"))
                    txtSeries2.Text = "0:0";
                else
                {
                    if (HDLPF.IsRightNumStringMode(str3.Split(':')[0].ToString(), 0, 255) &&
                        HDLPF.IsRightNumStringMode(str3.Split(':')[1].ToString(), 0, 255))
                        txtSeries2.Text = HDLPF.GetTimeFromString(str3, ':');
                    else
                        txtSeries2.Text = "0:0";
                }
                HDLSysPF.addcontrols(6, index, txtSeries2, dgvRelay);
                #endregion
            }
            else if (cbControlType2.Text == CsConst.myPublicControlType[7].ControlTypeName)//GPRS控制
            {
                #region
                cbbox12.Items.Clear();
                cbbox12.Items.Add(CsConst.WholeTextsList[1775].sDisplayName);
                cbbox12.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99862", ""));
                cbbox12.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99863", ""));
                HDLSysPF.addcontrols(4, index, cbbox12, dgvRelay);

                txtbox22.Text = str2;
                HDLSysPF.addcontrols(5, index, txtbox22, dgvRelay);
                dgvRelay[6, index].Value = "N/A";
                #endregion
            }
            else if (cbControlType2.Text == CsConst.myPublicControlType[8].ControlTypeName)//面板控制
            {
                #region
                HDLSysPF.addcontrols(4, index, cbPanleControl2, dgvRelay);
                #endregion
            }
            else if (cbControlType2.Text == CsConst.myPublicControlType[10].ControlTypeName || //广播场景
                     cbControlType2.Text == CsConst.myPublicControlType[11].ControlTypeName)   //广播回路
            {
                #region
                if (cbControlType2.Text == CsConst.myPublicControlType[10].ControlTypeName)
                {
                    dgvRelay[4, index].Value = CsConst.WholeTextsList[2566].sDisplayName;
                    dgvRelay[6, index].Value = "N/A";
                }
                else if (cbControlType2.Text == CsConst.myPublicControlType[11].ControlTypeName)
                {
                    dgvRelay[4, index].Value = CsConst.WholeTextsList[2567].sDisplayName;
                    if (!str3.Contains(":"))
                        txtSeries2.Text = "0:0";
                    else
                    {
                        if (HDLPF.IsRightNumStringMode(str3.Split(':')[0].ToString(), 0, 255) &&
                        HDLPF.IsRightNumStringMode(str3.Split(':')[1].ToString(), 0, 255))
                            txtSeries2.Text = HDLPF.GetTimeFromString(str3, ':');
                        else
                            txtSeries2.Text = "0:0";
                    }
                    HDLSysPF.addcontrols(6, index, txtSeries2, dgvRelay);
                }
                txtbox22.Text = str2;
                HDLSysPF.addcontrols(5, index, txtbox22, dgvRelay);
                #endregion
            }
            else if (cbControlType2.Text == CsConst.myPublicControlType[13].ControlTypeName)//音乐播放
            {
                #region
                HDLSysPF.addcontrols(4, index, cbAudioControl2, dgvRelay);
                #endregion
            }
            dgvRelay[3, index].Value = cbControlType2.Text;
            #region
            if (cbbox12.Visible && cbbox12.Items.Count > 0)
            {
                if (!cbbox12.Items.Contains(str1))
                    cbbox12.SelectedIndex = 0;
                else
                    cbbox12.Text = str1;
            }
            if (cbbox22.Visible && cbbox22.Items.Count > 0)
            {
                if (!cbbox22.Items.Contains(str2))
                    cbbox22.SelectedIndex = 0;
                else
                    cbbox22.Text = str2;
            }
            if (cbbox32.Visible && cbbox32.Items.Count > 0)
            {
                if (!cbbox32.Items.Contains(str3))
                    cbbox32.SelectedIndex = 0;
                else
                    cbbox32.Text = str3;
            }
            if (cbPanleControl2.Visible && cbPanleControl2.Items.Count > 0)
            {
                if (!cbPanleControl2.Items.Contains(str1))
                    cbPanleControl2.SelectedIndex = 0;
                else
                    cbPanleControl2.Text = str1;
            }
            if (cbAudioControl2.Visible && cbAudioControl2.Items.Count > 0)
            {
                if (!cbAudioControl2.Items.Contains(str1))
                    cbAudioControl2.SelectedIndex = 0;
                else
                    cbAudioControl2.Text = str1;
            }
            #endregion
            #region
            if (txtbox12.Visible) txtbox12_TextChanged(null, null);
            if (txtbox22.Visible) txtbox22_TextChanged(null, null);
            if (txtSeries2.Visible) txtSeries2_TextChanged(null, null);
            if (cbbox12.Visible) cbbox12_SelectedIndexChanged(null, null);
            if (cbbox22.Visible) cbbox22_SelectedIndexChanged(null, null);
            if (cbbox32.Visible) cbbox32_SelectedIndexChanged(null, null);
            if (cbPanleControl2.Visible) cbPanleControl2_SelectedIndexChanged(null, null);
            if (cbAudioControl2.Visible) cbAudioControl2_SelectedIndexChanged(null, null);
            #endregion
            if (cbControlType2.Focused)
                HDLSysPF.ModifyMultilinesIfNeeds(dgvRelay[3, index].Value.ToString(), 3, dgvRelay);
        }

        void cbControlType3_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbPanleControl3.Visible = false;
            cbAudioControl3.Visible = false;
            cbbox13.Visible = false;
            cbbox23.Visible = false;
            cbbox33.Visible = false;
            txtbox13.Visible = false;
            txtbox23.Visible = false;
            txtbox33.Visible = false;
            txtSeries3.Visible = false;
            int index = dgvP1.CurrentRow.Index;
            string str1 = dgvP1[4, index].Value.ToString();
            string str2 = dgvP1[5, index].Value.ToString();
            string str3 = dgvP1[6, index].Value.ToString();
            if (str1.Contains('(')) str1 = str1.Split('(')[0].ToString();
            if (str2.Contains('(')) str2 = str2.Split('(')[0].ToString();
            if (str3.Contains('(')) str3 = str3.Split('(')[0].ToString();
            if (cbControlType3.Text == CsConst.mstrINIDefault.IniReadValue("TargetType", "00000", ""))//无效
            {
                #region
                if (dgvP1.SelectedRows.Count > 0)
                {
                    for (int i = 0; i < dgvP1.SelectedRows.Count; i++)
                    {
                        dgvP1.SelectedRows[i].Cells[3].Value = cbControlType3.Items[0].ToString();
                        dgvP1[4, index].Value = "N/A";
                        dgvP1[5, index].Value = "N/A";
                        dgvP1[6, index].Value = "N/A";
                    }
                }
                #endregion
            }
            else if (cbControlType3.Text == CsConst.myPublicControlType[1].ControlTypeName || //场景 
                     cbControlType3.Text == CsConst.myPublicControlType[2].ControlTypeName)   //序列
            {
                #region
                txtbox13.Text = str1;
                HDLSysPF.addcontrols(4, index, txtbox13, dgvP1);

                txtbox23.Text = str2;
                HDLSysPF.addcontrols(5, index, txtbox23, dgvP1);
                dgvP1[6, index].Value = "N/A";
                #endregion
            }
            else if (cbControlType3.Text == CsConst.myPublicControlType[4].ControlTypeName || //通用开关
                     cbControlType3.Text == CsConst.myPublicControlType[6].ControlTypeName || //窗帘开关 
                     cbControlType3.Text == CsConst.myPublicControlType[12].ControlTypeName)   //消防模块
            {
                #region
                txtbox13.Text = str1;
                HDLSysPF.addcontrols(4, index, txtbox13, dgvP1);

                cbbox23.Items.Clear();
                if (cbControlType3.Text == CsConst.myPublicControlType[4].ControlTypeName)
                {
                    cbbox23.Items.Add(CsConst.Status[0]);
                    cbbox23.Items.Add(CsConst.Status[1]);
                }
                else if (cbControlType3.Text == CsConst.myPublicControlType[6].ControlTypeName)
                {
                    cbbox23.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00036", ""));
                    cbbox23.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00037", ""));
                    cbbox23.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00038", ""));
                    for (int i = 1; i <= 100; i++)
                        cbbox23.Items.Add(i.ToString() + "%");
                }
                else if (cbControlType3.Text == CsConst.myPublicControlType[12].ControlTypeName)
                {
                    for (int i = 0; i < 10; i++)
                        cbbox23.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "0008" + i.ToString(), ""));
                }
                HDLSysPF.addcontrols(5, index, cbbox23, dgvP1);
                dgvP1[6, index].Value = "N/A";
                #endregion
            }
            else if (cbControlType3.Text == CsConst.myPublicControlType[5].ControlTypeName)//单路调节
            {
                #region
                txtbox13.Text = str1;
                HDLSysPF.addcontrols(4, index, txtbox13, dgvP1);

                txtbox23.Text = str2;
                HDLSysPF.addcontrols(5, index, txtbox23, dgvP1);


                if (!str3.Contains(":"))
                    txtSeries3.Text = "0:0";
                else
                {
                    if (HDLPF.IsRightNumStringMode(str3.Split(':')[0].ToString(), 0, 255) &&
                        HDLPF.IsRightNumStringMode(str3.Split(':')[1].ToString(), 0, 255))
                        txtSeries3.Text = HDLPF.GetTimeFromString(str3, ':');
                    else
                        txtSeries3.Text = "0:0";
                }
                HDLSysPF.addcontrols(6, index, txtSeries3, dgvP1);
                #endregion
            }
            else if (cbControlType3.Text == CsConst.myPublicControlType[7].ControlTypeName)//GPRS控制
            {
                #region
                cbbox13.Items.Clear();
                cbbox13.Items.Add(CsConst.WholeTextsList[1775].sDisplayName);
                cbbox13.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99862", ""));
                cbbox13.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99863", ""));
                HDLSysPF.addcontrols(4, index, cbbox13, dgvP1);

                txtbox23.Text = str2;
                HDLSysPF.addcontrols(5, index, txtbox23, dgvP1);
                dgvP1[6, index].Value = "N/A";
                #endregion
            }
            else if (cbControlType3.Text == CsConst.myPublicControlType[8].ControlTypeName)//面板控制
            {
                #region
                HDLSysPF.addcontrols(4, index, cbPanleControl3, dgvP1);
                #endregion
            }
            else if (cbControlType3.Text == CsConst.myPublicControlType[10].ControlTypeName || //广播场景
                     cbControlType3.Text == CsConst.myPublicControlType[11].ControlTypeName)   //广播回路
            {
                #region
                if (cbControlType3.Text == CsConst.myPublicControlType[10].ControlTypeName)
                {
                    dgvP1[4, index].Value = CsConst.WholeTextsList[2566].sDisplayName;
                    dgvP1[6, index].Value = "N/A";
                }
                else if (cbControlType3.Text == CsConst.myPublicControlType[11].ControlTypeName)
                {
                    dgvP1[4, index].Value = CsConst.WholeTextsList[2567].sDisplayName;
                    if (!str3.Contains(":"))
                        txtSeries3.Text = "0:0";
                    else
                    {
                        if (HDLPF.IsRightNumStringMode(str3.Split(':')[0].ToString(), 0, 255) &&
                        HDLPF.IsRightNumStringMode(str3.Split(':')[1].ToString(), 0, 255))
                            txtSeries3.Text = HDLPF.GetTimeFromString(str3, ':');
                        else
                            txtSeries3.Text = "0:0";
                    }
                    HDLSysPF.addcontrols(6, index, txtSeries3, dgvP1);
                }
                txtbox23.Text = str2;
                HDLSysPF.addcontrols(5, index, txtbox23, dgvP1);
                #endregion
            }
            else if (cbControlType3.Text == CsConst.myPublicControlType[13].ControlTypeName)//音乐播放
            {
                #region
                HDLSysPF.addcontrols(4, index, cbAudioControl3, dgvP1);
                #endregion
            }
            dgvP1[3, index].Value = cbControlType3.Text;
            #region
            if (cbbox13.Visible && cbbox13.Items.Count > 0)
            {
                if (!cbbox13.Items.Contains(str1))
                    cbbox13.SelectedIndex = 0;
                else
                    cbbox13.Text = str1;
            }
            if (cbbox23.Visible && cbbox23.Items.Count > 0)
            {
                if (!cbbox23.Items.Contains(str2))
                    cbbox23.SelectedIndex = 0;
                else
                    cbbox23.Text = str2;
            }
            if (cbbox33.Visible && cbbox33.Items.Count > 0)
            {
                if (!cbbox33.Items.Contains(str3))
                    cbbox33.SelectedIndex = 0;
                else
                    cbbox33.Text = str3;
            }
            if (cbPanleControl3.Visible && cbPanleControl3.Items.Count > 0)
            {
                if (!cbPanleControl3.Items.Contains(str1))
                    cbPanleControl3.SelectedIndex = 0;
                else
                    cbPanleControl3.Text = str1;
            }
            if (cbAudioControl3.Visible && cbAudioControl3.Items.Count > 0)
            {
                if (!cbAudioControl3.Items.Contains(str1))
                    cbAudioControl3.SelectedIndex = 0;
                else
                    cbAudioControl3.Text = str1;
            }
            #endregion
            #region
            if (txtbox13.Visible) txtbox13_TextChanged(null, null);
            if (txtbox23.Visible) txtbox23_TextChanged(null, null);
            if (txtSeries3.Visible) txtSeries3_TextChanged(null, null);
            if (cbbox13.Visible) cbbox13_SelectedIndexChanged(null, null);
            if (cbbox23.Visible) cbbox23_SelectedIndexChanged(null, null);
            if (cbbox33.Visible) cbbox33_SelectedIndexChanged(null, null);
            if (cbPanleControl3.Visible) cbPanleControl3_SelectedIndexChanged(null, null);
            if (cbAudioControl3.Visible) cbAudioControl3_SelectedIndexChanged(null, null);
            #endregion
            if (cbControlType3.Focused)
                HDLSysPF.ModifyMultilinesIfNeeds(dgvP1[3, index].Value.ToString(), 3, dgvP1);
        }

        void cbControlType4_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbPanleControl4.Visible = false;
            cbAudioControl4.Visible = false;
            cbbox14.Visible = false;
            cbbox24.Visible = false;
            cbbox34.Visible = false;
            txtbox14.Visible = false;
            txtbox24.Visible = false;
            txtbox34.Visible = false;
            txtSeries4.Visible = false;
            int index = dgvP2.CurrentRow.Index;
            string str1 = dgvP2[4, index].Value.ToString();
            string str2 = dgvP2[5, index].Value.ToString();
            string str3 = dgvP2[6, index].Value.ToString();
            if (str1.Contains('(')) str1 = str1.Split('(')[0].ToString();
            if (str2.Contains('(')) str2 = str2.Split('(')[0].ToString();
            if (str3.Contains('(')) str3 = str3.Split('(')[0].ToString();
            if (cbControlType4.Text == CsConst.mstrINIDefault.IniReadValue("TargetType", "00000", ""))//无效
            {
                #region
                if (dgvP2.SelectedRows.Count > 0)
                {
                    for (int i = 0; i < dgvP2.SelectedRows.Count; i++)
                    {
                        dgvP2.SelectedRows[i].Cells[3].Value = cbControlType4.Items[0].ToString();
                        dgvP2[4, index].Value = "N/A";
                        dgvP2[5, index].Value = "N/A";
                        dgvP2[6, index].Value = "N/A";
                    }
                }
                #endregion
            }
            else if (cbControlType4.Text == CsConst.myPublicControlType[1].ControlTypeName || //场景 
                     cbControlType4.Text == CsConst.myPublicControlType[2].ControlTypeName)   //序列
            {
                #region
                txtbox14.Text = str1;
                HDLSysPF.addcontrols(4, index, txtbox14, dgvP2);

                txtbox24.Text = str2;
                HDLSysPF.addcontrols(5, index, txtbox24, dgvP2);
                dgvP2[6, index].Value = "N/A";
                #endregion
            }
            else if (cbControlType4.Text == CsConst.myPublicControlType[4].ControlTypeName || //通用开关
                     cbControlType4.Text == CsConst.myPublicControlType[6].ControlTypeName || //窗帘开关 
                     cbControlType4.Text == CsConst.myPublicControlType[12].ControlTypeName)   //消防模块
            {
                #region
                txtbox14.Text = str1;
                HDLSysPF.addcontrols(4, index, txtbox14, dgvP2);

                cbbox24.Items.Clear();
                if (cbControlType4.Text == CsConst.myPublicControlType[4].ControlTypeName)
                {
                    cbbox24.Items.Add(CsConst.Status[0]);
                    cbbox24.Items.Add(CsConst.Status[1]);
                }
                else if (cbControlType4.Text == CsConst.myPublicControlType[6].ControlTypeName)
                {
                    cbbox24.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00036", ""));
                    cbbox24.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00037", ""));
                    cbbox24.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00038", ""));
                    for (int i = 1; i <= 100; i++)
                        cbbox24.Items.Add(i.ToString() + "%");
                }
                else if (cbControlType4.Text == CsConst.myPublicControlType[12].ControlTypeName)
                {
                    for (int i = 0; i < 10; i++)
                        cbbox24.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "0008" + i.ToString(), ""));
                }
                HDLSysPF.addcontrols(5, index, cbbox24, dgvP2);
                dgvP2[6, index].Value = "N/A";
                #endregion
            }
            else if (cbControlType4.Text == CsConst.myPublicControlType[5].ControlTypeName)//单路调节
            {
                #region
                txtbox14.Text = str1;
                HDLSysPF.addcontrols(4, index, txtbox14, dgvP2);

                txtbox24.Text = str2;
                HDLSysPF.addcontrols(5, index, txtbox24, dgvP2);


                if (!str3.Contains(":"))
                    txtSeries4.Text = "0:0";
                else
                {
                    if (HDLPF.IsRightNumStringMode(str3.Split(':')[0].ToString(), 0, 255) &&
                        HDLPF.IsRightNumStringMode(str3.Split(':')[1].ToString(), 0, 255))
                        txtSeries4.Text = HDLPF.GetTimeFromString(str3, ':');
                    else
                        txtSeries4.Text = "0:0";
                }
                HDLSysPF.addcontrols(6, index, txtSeries4, dgvP2);
                #endregion
            }
            else if (cbControlType4.Text == CsConst.myPublicControlType[7].ControlTypeName)//GPRS控制
            {
                #region
                cbbox14.Items.Clear();
                cbbox14.Items.Add(CsConst.WholeTextsList[1775].sDisplayName);
                cbbox14.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99862", ""));
                cbbox14.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99863", ""));
                HDLSysPF.addcontrols(4, index, cbbox14, dgvP2);

                txtbox24.Text = str2;
                HDLSysPF.addcontrols(5, index, txtbox24, dgvP2);
                dgvP2[6, index].Value = "N/A";
                #endregion
            }
            else if (cbControlType4.Text == CsConst.myPublicControlType[8].ControlTypeName)//面板控制
            {
                #region
                HDLSysPF.addcontrols(4, index, cbPanleControl4, dgvP2);
                #endregion
            }
            else if (cbControlType4.Text == CsConst.myPublicControlType[10].ControlTypeName || //广播场景
                     cbControlType4.Text == CsConst.myPublicControlType[11].ControlTypeName)   //广播回路
            {
                #region
                if (cbControlType4.Text == CsConst.myPublicControlType[10].ControlTypeName)
                {
                    dgvP2[4, index].Value = CsConst.WholeTextsList[2566].sDisplayName;
                    dgvP2[6, index].Value = "N/A";
                }
                else if (cbControlType4.Text == CsConst.myPublicControlType[11].ControlTypeName)
                {
                    dgvP2[4, index].Value = CsConst.WholeTextsList[2567].sDisplayName;
                    if (!str3.Contains(":"))
                        txtSeries4.Text = "0:0";
                    else
                    {
                        if (HDLPF.IsRightNumStringMode(str3.Split(':')[0].ToString(), 0, 255) &&
                        HDLPF.IsRightNumStringMode(str3.Split(':')[1].ToString(), 0, 255))
                            txtSeries4.Text = HDLPF.GetTimeFromString(str3, ':');
                        else
                            txtSeries4.Text = "0:0";
                    }
                    HDLSysPF.addcontrols(6, index, txtSeries4, dgvP2);
                }
                txtbox24.Text = str2;
                HDLSysPF.addcontrols(5, index, txtbox24, dgvP2);
                #endregion
            }
            else if (cbControlType4.Text == CsConst.myPublicControlType[13].ControlTypeName)//音乐播放
            {
                #region
                HDLSysPF.addcontrols(4, index, cbAudioControl4, dgvP2);
                #endregion
            }
            dgvP2[3, index].Value = cbControlType4.Text;
            #region
            if (cbbox14.Visible && cbbox14.Items.Count > 0)
            {
                if (!cbbox14.Items.Contains(str1))
                    cbbox14.SelectedIndex = 0;
                else
                    cbbox14.Text = str1;
            }
            if (cbbox24.Visible && cbbox24.Items.Count > 0)
            {
                if (!cbbox24.Items.Contains(str2))
                    cbbox24.SelectedIndex = 0;
                else
                    cbbox24.Text = str2;
            }
            if (cbbox34.Visible && cbbox34.Items.Count > 0)
            {
                if (!cbbox34.Items.Contains(str3))
                    cbbox34.SelectedIndex = 0;
                else
                    cbbox34.Text = str3;
            }
            if (cbPanleControl4.Visible && cbPanleControl4.Items.Count > 0)
            {
                if (!cbPanleControl4.Items.Contains(str1))
                    cbPanleControl4.SelectedIndex = 0;
                else
                    cbPanleControl4.Text = str1;
            }
            if (cbAudioControl4.Visible && cbAudioControl4.Items.Count > 0)
            {
                if (!cbAudioControl4.Items.Contains(str1))
                    cbAudioControl4.SelectedIndex = 0;
                else
                    cbAudioControl4.Text = str1;
            }
            #endregion
            #region
            if (txtbox14.Visible) txtbox14_TextChanged(null, null);
            if (txtbox24.Visible) txtbox24_TextChanged(null, null);
            if (txtSeries4.Visible) txtSeries4_TextChanged(null, null);
            if (cbbox14.Visible) cbbox14_SelectedIndexChanged(null, null);
            if (cbbox24.Visible) cbbox24_SelectedIndexChanged(null, null);
            if (cbbox34.Visible) cbbox34_SelectedIndexChanged(null, null);
            if (cbPanleControl4.Visible) cbPanleControl4_SelectedIndexChanged(null, null);
            if (cbAudioControl4.Visible) cbAudioControl4_SelectedIndexChanged(null, null);
            #endregion
            if (cbControlType4.Focused)
                HDLSysPF.ModifyMultilinesIfNeeds(dgvP2[3, index].Value.ToString(), 3, dgvP2);
        }

        private void dgvSwitch_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                txtSub.Text = dgvSwitch[1, e.RowIndex].Value.ToString();
                HDLSysPF.addcontrols(1, e.RowIndex, txtSub, dgvSwitch);

                txtDev.Text = dgvSwitch[2, e.RowIndex].Value.ToString();
                HDLSysPF.addcontrols(2, e.RowIndex, txtDev, dgvSwitch);

                cbControlType.Text = dgvSwitch[3, e.RowIndex].Value.ToString();
                HDLSysPF.addcontrols(3, e.RowIndex, cbControlType, dgvSwitch);

                txtSub_TextChanged(txtSub, null);
                txtDev_TextChanged(txtDev, null);
                cbControlType_SelectedIndexChanged(cbControlType, null);
            }
        }

        private void btnSaveTemp_Click(object sender, EventArgs e)
        {

        }

        private void dgvRelay_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                txtSub2.Text = dgvRelay[1, e.RowIndex].Value.ToString();
                HDLSysPF.addcontrols(1, e.RowIndex, txtSub2, dgvRelay);

                txtDev2.Text = dgvRelay[2, e.RowIndex].Value.ToString();
                HDLSysPF.addcontrols(2, e.RowIndex, txtDev2, dgvRelay);

                cbControlType2.Text = dgvRelay[3, e.RowIndex].Value.ToString();
                HDLSysPF.addcontrols(3, e.RowIndex, cbControlType2, dgvRelay);

                txtSub2_TextChanged(txtSub2, null);
                txtDev2_TextChanged(txtDev2, null);
                cbControlType2_SelectedIndexChanged(cbControlType2, null);
            }
        }

        private void chbLogic_CheckedChanged(object sender, EventArgs e)
        {
            pnlLogic.Visible = chbLogic.Checked;
            if (isRead) return;
            if (myFH == null) return;
            if (chbLogic.Checked) myFH.PumpsEnable = 1;
            else myFH.PumpsEnable = 0;
            Cursor.Current = Cursors.WaitCursor;
            byte[] ArayTmp = new byte[1];
            ArayTmp[0] = myFH.PumpsEnable;
            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1D20, SubNetID, DevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(MyintDeviceType)) == true)
            {
                HDLUDP.TimeBetwnNext(20);
            }
            Cursor.Current = Cursors.Default;
        }

        private void dgvPumps_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                setAllControlVisible(false);
                if (isRead) return;
                dgvP1.Rows.Clear();
                dgvP2.Rows.Clear();
                PumpsSelectedIndex = e.RowIndex;
                if (dgvPumps.RowCount >= 7)
                {
                    for (int i = 2; i <= 7; i++)
                    {
                        dgvPumps[i, 6].ReadOnly = true;
                        dgvPumps[i, 6].Style.BackColor = Color.Black;
                    }
                }
                if (myFH.myPumps != null)
                {
                    if (myFH.myPumps[e.RowIndex].TrueTargets != null)
                    {
                        for (int i = 0; i < myFH.myPumps[e.RowIndex].TrueTargets.Count; i++)
                        {
                            UVCMD.ControlTargets cmd = myFH.myPumps[e.RowIndex].TrueTargets[i];
                            string strType = "";
                            strType = ButtonMode.ConvertorKeyModeToPublicModeGroup(cmd.Type);
                            string strParam1 = "0", strParam2 = "0", strParam3 = "0", strParam4 = "0";
                            strParam1 = cmd.Param1.ToString();
                            strParam2 = cmd.Param2.ToString();
                            strParam3 = cmd.Param3.ToString();
                            strParam4 = cmd.Param4.ToString();
                            SetParams(ref strType, ref strParam1, ref strParam2, ref strParam3, ref strParam4);
                            object[] obj = new object[] { (cmd.ID+1).ToString(),cmd.SubnetID.ToString(),cmd.DeviceID.ToString(),strType
                                ,strParam1,strParam2,strParam3,strParam4};
                            dgvP1.Rows.Add(obj);
                        }
                    }
                    if (myFH.myPumps[e.RowIndex].FalseTargets != null)
                    {
                        for (int i = 0; i < myFH.myPumps[e.RowIndex].FalseTargets.Count; i++)
                        {
                            UVCMD.ControlTargets cmd = myFH.myPumps[e.RowIndex].FalseTargets[i];
                            string strType = "";
                            strType = ButtonMode.ConvertorKeyModeToPublicModeGroup(cmd.Type);
                            string strParam1 = "0", strParam2 = "0", strParam3 = "0", strParam4 = "0";
                            strParam1 = cmd.Param1.ToString();
                            strParam2 = cmd.Param2.ToString();
                            strParam3 = cmd.Param3.ToString();
                            strParam4 = cmd.Param4.ToString();
                            SetParams(ref strType, ref strParam1, ref strParam2, ref strParam3, ref strParam4);
                            object[] obj = new object[] { (cmd.ID+1).ToString(),cmd.SubnetID.ToString(),cmd.DeviceID.ToString(),strType
                                ,strParam1,strParam2,strParam3,strParam4};
                            dgvP2.Rows.Add(obj);
                        }
                    }
                }
            }
            catch
            {
            }
        }


        private void dgvPumps_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (isRead) return;
                if (myFH == null) return;
                if ((e.RowIndex == -1) || (e.ColumnIndex == -1)) return;
                if (dgvPumps[e.ColumnIndex, e.RowIndex].Value == null) dgvPumps[e.ColumnIndex, e.RowIndex].Value = "";
                for (int i = 0; i < dgvPumps.SelectedRows.Count; i++)
                {
                    string strTmp = "";
                    dgvPumps.SelectedRows[i].Cells[e.ColumnIndex].Value = dgvPumps[e.ColumnIndex, e.RowIndex].Value.ToString();
                    switch (e.ColumnIndex)
                    {
                        case 1:
                            if (dgvPumps[1, dgvPumps.SelectedRows[i].Index].Value.ToString().ToLower() == "true")
                                myFH.myPumps[dgvPumps.SelectedRows[i].Index].Enable = 1;
                            else
                                myFH.myPumps[dgvPumps.SelectedRows[i].Index].Enable = 0;
                            break;
                        case 2:
                            if (dgvPumps[2, dgvPumps.SelectedRows[i].Index].Value.ToString().ToLower() == "true")
                                myFH.myPumps[dgvPumps.SelectedRows[i].Index].ChooseChns = HDLSysPF.SetBit(myFH.myPumps[dgvPumps.SelectedRows[i].Index].ChooseChns, 0);
                            else
                                myFH.myPumps[dgvPumps.SelectedRows[i].Index].ChooseChns = HDLSysPF.ClearBit(myFH.myPumps[dgvPumps.SelectedRows[i].Index].ChooseChns, 0);
                            break;
                        case 3:
                            if (dgvPumps[3, dgvPumps.SelectedRows[i].Index].Value.ToString().ToLower() == "true")
                                myFH.myPumps[dgvPumps.SelectedRows[i].Index].ChooseChns = HDLSysPF.SetBit(myFH.myPumps[dgvPumps.SelectedRows[i].Index].ChooseChns, 1);
                            else
                                myFH.myPumps[dgvPumps.SelectedRows[i].Index].ChooseChns = HDLSysPF.ClearBit(myFH.myPumps[dgvPumps.SelectedRows[i].Index].ChooseChns, 1);
                            break;
                        case 4:
                            if (dgvPumps[4, dgvPumps.SelectedRows[i].Index].Value.ToString().ToLower() == "true")
                                myFH.myPumps[dgvPumps.SelectedRows[i].Index].ChooseChns = HDLSysPF.SetBit(myFH.myPumps[dgvPumps.SelectedRows[i].Index].ChooseChns, 2);
                            else
                                myFH.myPumps[dgvPumps.SelectedRows[i].Index].ChooseChns = HDLSysPF.ClearBit(myFH.myPumps[dgvPumps.SelectedRows[i].Index].ChooseChns, 2);
                            break;
                        case 5:
                            if (dgvPumps[5, dgvPumps.SelectedRows[i].Index].Value.ToString().ToLower() == "true")
                                myFH.myPumps[dgvPumps.SelectedRows[i].Index].ChooseChns = HDLSysPF.SetBit(myFH.myPumps[dgvPumps.SelectedRows[i].Index].ChooseChns, 3);
                            else
                                myFH.myPumps[dgvPumps.SelectedRows[i].Index].ChooseChns = HDLSysPF.ClearBit(myFH.myPumps[dgvPumps.SelectedRows[i].Index].ChooseChns, 3);
                            break;
                        case 6:
                            if (dgvPumps[6, dgvPumps.SelectedRows[i].Index].Value.ToString().ToLower() == "true")
                                myFH.myPumps[dgvPumps.SelectedRows[i].Index].ChooseChns = HDLSysPF.SetBit(myFH.myPumps[dgvPumps.SelectedRows[i].Index].ChooseChns, 4);
                            else
                                myFH.myPumps[dgvPumps.SelectedRows[i].Index].ChooseChns = HDLSysPF.ClearBit(myFH.myPumps[dgvPumps.SelectedRows[i].Index].ChooseChns, 4);
                            break;
                        case 7:
                            if (dgvPumps[7, dgvPumps.SelectedRows[i].Index].Value.ToString().ToLower() == "true")
                                myFH.myPumps[dgvPumps.SelectedRows[i].Index].ChooseChns = HDLSysPF.SetBit(myFH.myPumps[dgvPumps.SelectedRows[i].Index].ChooseChns, 5);
                            else
                                myFH.myPumps[dgvPumps.SelectedRows[i].Index].ChooseChns = HDLSysPF.ClearBit(myFH.myPumps[dgvPumps.SelectedRows[i].Index].ChooseChns, 5);
                            break;
                        case 8:
                            strTmp = dgvPumps[8, dgvPumps.SelectedRows[i].Index].Value.ToString();
                            myFH.myPumps[dgvPumps.SelectedRows[i].Index].TrueDelay = int.Parse(HDLPF.IsNumStringMode(strTmp, 0, 600));
                            dgvPumps[8, dgvPumps.SelectedRows[i].Index].Value = int.Parse(HDLPF.IsNumStringMode(strTmp, 0, 600)).ToString();
                            break;
                        case 9:
                            strTmp = dgvPumps[9, dgvPumps.SelectedRows[i].Index].Value.ToString();
                            myFH.myPumps[dgvPumps.SelectedRows[i].Index].FalseDelay = int.Parse(HDLPF.IsNumStringMode(strTmp, 0, 600));
                            dgvPumps[9, dgvPumps.SelectedRows[i].Index].Value = int.Parse(HDLPF.IsNumStringMode(strTmp, 0, 600)).ToString();
                            break;
                    }
                }

                if (e.ColumnIndex == 1)
                {
                    if (dgvPumps[1, e.RowIndex].Value.ToString().ToLower() == "true")
                        myFH.myPumps[e.RowIndex].Enable = 1;
                    else
                        myFH.myPumps[e.RowIndex].Enable = 0;
                }
                if (e.ColumnIndex == 2)
                {
                    if (dgvPumps[2, e.RowIndex].Value.ToString().ToLower() == "true")
                        myFH.myPumps[e.RowIndex].ChooseChns = HDLSysPF.SetBit(myFH.myPumps[e.RowIndex].ChooseChns, 0);
                    else
                        myFH.myPumps[e.RowIndex].ChooseChns = HDLSysPF.ClearBit(myFH.myPumps[e.RowIndex].ChooseChns, 0);
                }
                if (e.ColumnIndex == 3)
                {
                    if (dgvPumps[3, e.RowIndex].Value.ToString().ToLower() == "true")
                        myFH.myPumps[e.RowIndex].ChooseChns = HDLSysPF.SetBit(myFH.myPumps[e.RowIndex].ChooseChns, 1);
                    else
                        myFH.myPumps[e.RowIndex].ChooseChns = HDLSysPF.ClearBit(myFH.myPumps[e.RowIndex].ChooseChns, 1);
                }
                if (e.ColumnIndex == 4)
                {
                    if (dgvPumps[4, e.RowIndex].Value.ToString().ToLower() == "true")
                        myFH.myPumps[e.RowIndex].ChooseChns = HDLSysPF.SetBit(myFH.myPumps[e.RowIndex].ChooseChns, 2);
                    else
                        myFH.myPumps[e.RowIndex].ChooseChns = HDLSysPF.ClearBit(myFH.myPumps[e.RowIndex].ChooseChns, 2);
                }
                if (e.ColumnIndex == 5)
                {
                    if (dgvPumps[5, e.RowIndex].Value.ToString().ToLower() == "true")
                        myFH.myPumps[e.RowIndex].ChooseChns = HDLSysPF.SetBit(myFH.myPumps[e.RowIndex].ChooseChns, 3);
                    else
                        myFH.myPumps[e.RowIndex].ChooseChns = HDLSysPF.ClearBit(myFH.myPumps[e.RowIndex].ChooseChns, 3);
                }
                if (e.ColumnIndex == 6)
                {
                    if (dgvPumps[6, e.RowIndex].Value.ToString().ToLower() == "true")
                        myFH.myPumps[e.RowIndex].ChooseChns = HDLSysPF.SetBit(myFH.myPumps[e.RowIndex].ChooseChns, 4);
                    else
                        myFH.myPumps[e.RowIndex].ChooseChns = HDLSysPF.ClearBit(myFH.myPumps[e.RowIndex].ChooseChns, 4);
                }
                if (e.ColumnIndex == 7)
                {
                    if (dgvPumps[7, e.RowIndex].Value.ToString().ToLower() == "true")
                        myFH.myPumps[e.RowIndex].ChooseChns = HDLSysPF.SetBit(myFH.myPumps[e.RowIndex].ChooseChns, 5);
                    else
                        myFH.myPumps[e.RowIndex].ChooseChns = HDLSysPF.ClearBit(myFH.myPumps[e.RowIndex].ChooseChns, 5);
                }
                if (e.ColumnIndex == 8)
                {
                    string strTmp = dgvPumps[8, e.RowIndex].Value.ToString();
                    myFH.myPumps[e.RowIndex].TrueDelay = int.Parse(HDLPF.IsNumStringMode(strTmp, 0, 600));
                    dgvPumps[8, e.RowIndex].Value = int.Parse(HDLPF.IsNumStringMode(strTmp, 0, 600)).ToString();
                }
                if (e.ColumnIndex == 9)
                {
                    string strTmp = dgvPumps[9, e.RowIndex].Value.ToString();
                    myFH.myPumps[e.RowIndex].FalseDelay = int.Parse(HDLPF.IsNumStringMode(strTmp, 0, 600));
                    dgvPumps[9, e.RowIndex].Value = int.Parse(HDLPF.IsNumStringMode(strTmp, 0, 600)).ToString();
                }
            }
            catch
            {
            }
        }

        private void dgvPumps_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            dgvPumps.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void dgvPumps_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                setAllControlVisible(false);
                dgvP1.Rows.Clear();
                dgvP2.Rows.Clear();
                Cursor.Current = Cursors.WaitCursor;
                byte[] arayTmp = new byte[3];
                myFH.myPumps[e.RowIndex].TrueTargets = new List<UVCMD.ControlTargets>();
                myFH.myPumps[e.RowIndex].FalseTargets = new List<UVCMD.ControlTargets>();
                for (byte i = 0; i < 2; i++)
                {
                    arayTmp[0] = Convert.ToByte(e.RowIndex);
                    arayTmp[1] = i;
                    for (byte j = 0; j < 2; j++)
                    {
                        arayTmp[2] = j;
                        if (CsConst.mySends.AddBufToSndList(arayTmp, 0x1D2A, SubNetID, DevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(MyintDeviceType)) == true)
                        {
                            string strType = "";
                            strType = ButtonMode.ConvertorKeyModeToPublicModeGroup(CsConst.myRevBuf[28]);
                            string strParam1 = "0", strParam2 = "0", strParam3 = "0", strParam4 = "0";
                            strParam1 = CsConst.myRevBuf[31].ToString();
                            strParam2 = CsConst.myRevBuf[32].ToString();
                            strParam3 = CsConst.myRevBuf[33].ToString();
                            strParam4 = CsConst.myRevBuf[34].ToString();
                            SetParams(ref strType, ref strParam1, ref strParam2, ref strParam3, ref strParam4);
                            object[] obj = new object[] { (j+1).ToString(),CsConst.myRevBuf[29].ToString(),CsConst.myRevBuf[30].ToString(),strType
                                ,strParam1,strParam2,strParam3,strParam4};
                            UVCMD.ControlTargets oCMD = new UVCMD.ControlTargets();
                            oCMD.ID = Convert.ToByte(CsConst.myRevBuf[27]);
                            oCMD.Type = CsConst.myRevBuf[28];  //转换为正确的类型
                            oCMD.SubnetID = CsConst.myRevBuf[29];
                            oCMD.DeviceID = CsConst.myRevBuf[30];
                            oCMD.Param1 = CsConst.myRevBuf[31];
                            oCMD.Param2 = CsConst.myRevBuf[32];
                            oCMD.Param3 = CsConst.myRevBuf[33];
                            oCMD.Param4 = CsConst.myRevBuf[34];
                            if (i == 0)
                            {
                                dgvP1.Rows.Add(obj);
                                myFH.myPumps[e.RowIndex].TrueTargets.Add(oCMD);
                            }
                            else if (i == 1)
                            {
                                dgvP2.Rows.Add(obj);
                                myFH.myPumps[e.RowIndex].FalseTargets.Add(oCMD);
                            }
                        }
                        else
                        {
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                    }
                }
            }
            catch
            {
            }
            Cursor.Current = Cursors.Default;
        }

        private void dgvP1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                txtSub3.Text = dgvP1[1, e.RowIndex].Value.ToString();
                HDLSysPF.addcontrols(1, e.RowIndex, txtSub3, dgvP1);

                txtDev3.Text = dgvP1[2, e.RowIndex].Value.ToString();
                HDLSysPF.addcontrols(2, e.RowIndex, txtDev3, dgvP1);

                cbControlType3.Text = dgvP1[3, e.RowIndex].Value.ToString();
                HDLSysPF.addcontrols(3, e.RowIndex, cbControlType3, dgvP1);

                txtSub3_TextChanged(txtSub, null);
                txtDev3_TextChanged(txtDev, null);
                cbControlType3_SelectedIndexChanged(cbControlType, null);
            }
        }

        private void dgvP2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                txtSub4.Text = dgvP2[1, e.RowIndex].Value.ToString();
                HDLSysPF.addcontrols(1, e.RowIndex, txtSub4, dgvP2);

                txtDev4.Text = dgvP2[2, e.RowIndex].Value.ToString();
                HDLSysPF.addcontrols(2, e.RowIndex, txtDev4, dgvP2);

                cbControlType4.Text = dgvP2[3, e.RowIndex].Value.ToString();
                HDLSysPF.addcontrols(3, e.RowIndex, cbControlType4, dgvP2);

                txtSub4_TextChanged(txtSub, null);
                txtDev4_TextChanged(txtDev, null);
                cbControlType4_SelectedIndexChanged(cbControlType, null);
            }
        }

        private void chbSwitch_CheckedChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            Cursor.Current = Cursors.WaitCursor;
            byte[] arayTmp = new byte[2];
            arayTmp[1] = myFH.RelayTargetEnable;
            if (chbSwitch.Checked)
            {
                myFH.FHTargetEnable = Convert.ToByte(myFH.FHTargetEnable | (1 << (cbSwitch.SelectedIndex)));
            }
            else
            {
                myFH.FHTargetEnable = Convert.ToByte(myFH.FHTargetEnable & (~(1 << (cbSwitch.SelectedIndex))));
            }
            arayTmp[0] = myFH.FHTargetEnable;
            if (CsConst.mySends.AddBufToSndList(arayTmp, 0x1CE8, SubNetID, DevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(MyintDeviceType)) == true)
            {
            } 
            Cursor.Current = Cursors.Default;
        }

        private void chbRelay_CheckedChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            Cursor.Current = Cursors.WaitCursor;
            byte[] arayTmp = new byte[2];
            arayTmp[0] = myFH.FHTargetEnable;
            if (chbRelay.Checked)
            {
                myFH.RelayTargetEnable = Convert.ToByte(myFH.RelayTargetEnable | (1 << (cbRelay.SelectedIndex)));
            }
            else
            {
                myFH.RelayTargetEnable = Convert.ToByte(myFH.RelayTargetEnable & (~(1 << (cbRelay.SelectedIndex))));
            }
            arayTmp[1] = myFH.RelayTargetEnable;
            if (CsConst.mySends.AddBufToSndList(arayTmp, 0x1CEA, SubNetID, DevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(MyintDeviceType)) == true)
            {
            }
            Cursor.Current = Cursors.Default;
        }

        private void tmRead_Click(object sender, EventArgs e)
        {
            tslRead_Click(tslRead, null);
        }

        private void dgvPumps_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex == 6)
            {
                if (e.ColumnIndex == 2)
                {
                    HDLSysPF.addcontrols(e.ColumnIndex, e.RowIndex, pnl1, dgvPumps);
                }
                else if (e.ColumnIndex == 3)
                {
                    HDLSysPF.addcontrols(e.ColumnIndex, e.RowIndex, pnl2, dgvPumps);
                }
                else if (e.ColumnIndex == 4)
                {
                    HDLSysPF.addcontrols(e.ColumnIndex, e.RowIndex, pnl3, dgvPumps);
                }
                else if (e.ColumnIndex == 5)
                {
                    HDLSysPF.addcontrols(e.ColumnIndex, e.RowIndex, pnl4, dgvPumps);
                }
                else if (e.ColumnIndex == 6)
                {
                    HDLSysPF.addcontrols(e.ColumnIndex, e.RowIndex, pnl5, dgvPumps);
                }
                else if (e.ColumnIndex == 7)
                {
                    HDLSysPF.addcontrols(e.ColumnIndex, e.RowIndex, pnl6, dgvPumps);
                }
            }
        }
    }
}
