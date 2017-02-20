using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HDL_Buspro_Setup_Tool
{
    public partial class Connetion : UserControl
    {
        private byte bytSubID;
        private byte bytDevID;
        private int DeviceType;
        public byte ConnetionType;
        public string strGrpName;
        public string strProName;
        public string strUser;
        public string strPWD;
        public string strIP1;
        public int intPort1;
        public string strIP2;
        public int intPort2;
        public byte intTimer;
        public byte bytEnDHCP;

        public Byte ExtraMinute; // 2016 12 28 new add
        public Byte ExtraSecond;
        public Connetion()
        {
            InitializeComponent();
        }

        public Connetion(byte SubNetID, byte DevID, int DevType)
        {
            InitializeComponent();
            this.bytSubID = SubNetID;
            this.bytDevID = DevID;
            this.DeviceType = DevType;
            cbZone.Items.Clear();

            Boolean DisplayTimeZone = IPmoduleDeviceTypeList.IpModuleV3TimeZoneUrl.Contains(DeviceType);

            lbTimer.Visible = !DisplayTimeZone;
            tbTimer.Visible = !DisplayTimeZone;
            lbZone.Visible = DisplayTimeZone;
            cbZone.Visible = DisplayTimeZone;
            lbTime.Visible = DisplayTimeZone;

            for (int i = 0; i < 34; i++)
                cbZone.Items.Add(CsConst.mstrINIDefault.IniReadValue("TimeZone", "000" + GlobalClass.AddLeftZero(i.ToString(),2), ""));
            if (CsConst.iLanguageId == 1)
            {
                grpSetup.Text = CsConst.mstrINIDefault.IniReadValue("Connetion", "00001", "");
                lbhint.Text = CsConst.mstrINIDefault.IniReadValue("Connetion", "00000", "");
                lbType.Text = CsConst.mstrINIDefault.IniReadValue("Connetion", "00002", "");
                lbGroup.Text = CsConst.mstrINIDefault.IniReadValue("Connetion", "00003", "");
                lbProject.Text = CsConst.mstrINIDefault.IniReadValue("Connetion", "00004", "");
                lbUser.Text = CsConst.mstrINIDefault.IniReadValue("Connetion", "00005", "");
                lbPWD.Text = CsConst.mstrINIDefault.IniReadValue("Connetion", "00006", "");
                lbSev1.Text = CsConst.mstrINIDefault.IniReadValue("Connetion", "00007", "");
                lbport1.Text = CsConst.mstrINIDefault.IniReadValue("Connetion", "00008", "");
                lbSev2.Text = CsConst.mstrINIDefault.IniReadValue("Connetion", "00009", "");
                lbport2.Text = CsConst.mstrINIDefault.IniReadValue("Connetion", "00010", "");
                lbTimer.Text = CsConst.mstrINIDefault.IniReadValue("Connetion", "00011", "");
                btnModify.Text = CsConst.mstrINIDefault.IniReadValue("Connetion", "00012", "");
                groupBox1.Text = CsConst.mstrINIDefault.IniReadValue("Connetion", "00013", "");
                btnRead.Text = CsConst.mstrINIDefault.IniReadValue("Connetion", "00019", "");
                lbZone.Text = CsConst.mstrINIDefault.IniReadValue("Connetion", "00020", "");
            }
            if (CsConst.MyEditMode == 1)
            {
                readConnetionInfomation();
            }
            else
            {
                dafaulInfo();
            }
        }

        public void btnModify_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            byte[] arayTmp = new byte[57];
            arayTmp[0] = Convert.ToByte(cbType.SelectedIndex);
            byte[] arayTmpRemark = new byte[20];
            arayTmpRemark = HDLUDP.StringToByte(tbGroup.Text.Trim());
            if (arayTmpRemark != null) Array.Copy(arayTmpRemark, 0, arayTmp, 1, arayTmpRemark.Length);
            arayTmpRemark = HDLUDP.StringToByte(tbProject.Text.Trim());
            if (arayTmpRemark != null) Array.Copy(arayTmpRemark, 0, arayTmp, 21, arayTmpRemark.Length);
            arayTmpRemark = HDLUDP.StringToByte(tbUser.Text.Trim());
            if (arayTmpRemark != null) Array.Copy(arayTmpRemark, 0, arayTmp, 41, arayTmpRemark.Length);
            arayTmpRemark = HDLUDP.StringToByte(tbPWD.Text.Trim());
            if (arayTmpRemark != null) Array.Copy(arayTmpRemark, 0, arayTmp, 49, arayTmpRemark.Length);

            if (CsConst.mySends.AddBufToSndList(arayTmp, 0x3003, bytSubID, bytDevID, false, true, true,false) == false)
            {
                return;
            }

            if (arayTmp[0] <= 2) // 不是域名解析的
            {
                SaveServerIpAddressInformation();
            }
            else
            {
                SaveServerURLInformation();
            }

            if (cbZone.Visible == true) lbTime.Text = HDLSysPF.ReadDeviceDateTime(bytSubID, bytDevID);
            Cursor.Current = Cursors.Default;
        }

        public void readConnetionInfomation()
        {
            Cursor.Current = Cursors.WaitCursor;
            byte[] ArayTmp = new byte[0];

            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x3007, bytSubID, bytDevID, false, true, true, false) == true)
            {
                this.ConnetionType = CsConst.myRevBuf[25];

                DisplayIPProjectInformation(CsConst.myRevBuf);

                if (ConnetionType <= 2) //没有域名解析
                {
                    ReadServerIpInformation();
                }
                else
                {
                    ReadServerURLInformation();
                }
            }
            else
            {
                return;
            }
            Cursor.Current = Cursors.Default;
        }

        private void DisplayIPProjectInformation(Byte[] RevData)
        {
            byte[] arayRemark = new byte[20];
            HDLSysPF.CopyRemarkBufferFrmMyRevBuffer(RevData, arayRemark, 26);
            this.strGrpName = HDLPF.Byte2String(arayRemark);

            arayRemark = new byte[20];
            HDLSysPF.CopyRemarkBufferFrmMyRevBuffer(RevData, arayRemark, 46);
            this.strProName = HDLPF.Byte2String(arayRemark);

            arayRemark = new byte[8];
            for (int intI = 0; intI < 8; intI++) { arayRemark[intI] = RevData[66 + intI]; }
            this.strUser = HDLPF.Byte2String(arayRemark);

            arayRemark = new byte[8];
            for (int intI = 0; intI < 8; intI++) { arayRemark[intI] = RevData[74 + intI]; }
            this.strPWD = HDLPF.Byte2String(arayRemark);
            cbType.SelectedIndex = this.ConnetionType;
            tbGroup.Text = this.strGrpName;
            tbProject.Text = this.strProName;
            tbUser.Text = this.strUser;
            tbPWD.Text = this.strPWD;
            CsConst.myRevBuf = new byte[1200];
        }

        private Boolean ReadServerURLInformation()
        {
            Boolean blnIsSuccess = false;
            try
            {
                Byte[] ArayTmp = null;
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x3020, bytSubID, bytDevID, false, true, true, false) == true)
                {
                    ArayTmp = new Byte[68];
                    Array.Copy(CsConst.myRevBuf, 25, ArayTmp, 0, 68);

                    URL.Text = HDLPF.Byte2String(ArayTmp);
                    CsConst.myRevBuf = new byte[1200];
                    blnIsSuccess = true;
                }
                else
                {
                    blnIsSuccess = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            return blnIsSuccess;
        }

        private Boolean SaveServerURLInformation()
        {
            Boolean blnIsSuccess = false;
            try
            {
                String UrlAddress = URL.Text;

                Byte[] ArayTmp = HDLUDP.StringToByte(UrlAddress);
                Byte[] SendBuffer = new Byte[68];
                ArayTmp.CopyTo(SendBuffer, 0);
                if (CsConst.mySends.AddBufToSndList(SendBuffer, 0x3022, bytSubID, bytDevID, false, true, true, false) == true)
                {
                    CsConst.myRevBuf = new byte[1200];
                    blnIsSuccess = true;
                }
                else
                {
                    blnIsSuccess = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            return blnIsSuccess;
        }

        private Boolean SaveServerIpAddressInformation()
        {
            Boolean BlnIsSuccess = false;
            try
            {
                Byte[] arayTmp = new byte[16];
                string[] strTmp = mtbSev1.Text.Split('.');
                for (int i = 0; i < 4; i++)
                {
                    if (strTmp[i] != null && strTmp[i] != "")
                        arayTmp[i] = byte.Parse(strTmp[i].ToString());
                }
                int intPort = Convert.ToInt32(tbPort1.Text);
                arayTmp[4] = (byte)(intPort / 256);
                arayTmp[5] = (byte)(intPort % 256);

                strTmp = mtbSev2.Text.Split('.');
                for (int i = 0; i < 4; i++)
                {
                    if (strTmp[i] != null && strTmp[i] != "")
                        arayTmp[i + 6] = byte.Parse(strTmp[i].ToString());
                }
                intPort = Convert.ToInt32(tbPort2.Text);
                arayTmp[10] = (byte)(intPort / 256);
                arayTmp[11] = (byte)(intPort % 256);
                arayTmp[12] = this.bytEnDHCP;
                arayTmp[13] = Convert.ToByte(tbTimer.Text.Trim());
                if (cbZone.Visible)
                {
                    if (cbZone.SelectedIndex == 0) arayTmp[13] = 255;
                    else
                    {
                        arayTmp[13] = Convert.ToByte(cbZone.SelectedIndex - 1);
                        if (arayTmp[13] > 24)
                        {
                            String sTmpTimeZone = cbZone.Text;
                            Byte bTimeZone = 0;
                            if (sTmpTimeZone.Contains("+")) bTimeZone = 12;
                            sTmpTimeZone = sTmpTimeZone.Substring(5, 5);
                            String[] TimeZoneAndMinute = sTmpTimeZone.Split(':');
                            arayTmp[13] = (Byte)(Convert.ToByte(TimeZoneAndMinute[0]) + bTimeZone);
                            arayTmp[14] = Convert.ToByte(TimeZoneAndMinute[1]);
                        }
                    }
                }
                BlnIsSuccess = CsConst.mySends.AddBufToSndList(arayTmp, 0x3005, bytSubID, bytDevID, false, true, true, false);
            }
            catch
            { }
            return BlnIsSuccess;
        }

        private Boolean ReadServerIpInformation()
        {
            Boolean blnIsSuccess = false;
            try
            {
                Byte[] ArayTmp = null;
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x3009, bytSubID, bytDevID, false, true, true, false) == true)
                {
                    this.strIP1 = CsConst.myRevBuf[25].ToString() + "." + CsConst.myRevBuf[26].ToString()
                          + "." + CsConst.myRevBuf[27].ToString() + "." + CsConst.myRevBuf[28].ToString();
                    intPort1 = CsConst.myRevBuf[29] * 256 + CsConst.myRevBuf[30];

                    this.strIP2 = CsConst.myRevBuf[31].ToString() + "." + CsConst.myRevBuf[32].ToString()
                          + "." + CsConst.myRevBuf[33].ToString() + "." + CsConst.myRevBuf[34].ToString();
                    intPort2 = CsConst.myRevBuf[35] * 256 + CsConst.myRevBuf[36];

                    this.bytEnDHCP = CsConst.myRevBuf[37];
                    this.intTimer = CsConst.myRevBuf[38];
                    this.ExtraMinute = CsConst.myRevBuf[39];
                    this.ExtraSecond = CsConst.myRevBuf[40];
                    if (ExtraMinute > 59) ExtraMinute = 0;
                    if (ExtraSecond > 59) ExtraSecond = 0;
                    mtbSev1.Text = strIP1;
                    mtbSev2.Text = strIP2;
                    tbPort1.Text = intPort1.ToString();
                    tbPort2.Text = intPort2.ToString();
                    tbTimer.Text = intTimer.ToString();
                    if (intTimer < cbZone.Items.Count) //time zone add half an hour or later
                    {
                        cbZone.SelectedIndex = intTimer + 1;
                        String sTmpTimeZone = "";
                        if (ExtraMinute != 0 || ExtraSecond !=0)
                        {
                            if (intTimer<12) 
                                sTmpTimeZone = "-" + intTimer.ToString("D2") + ":" + ExtraMinute.ToString("D2");
                            else
                                sTmpTimeZone = "+" + (intTimer - 12).ToString("D2") + ":" + ExtraMinute.ToString("D2");
                            cbZone.SelectedIndex = HDLSysPF.GetSelectedIndexWhichContainedPartString(cbZone, sTmpTimeZone);
                        }
                    }
                    else
                        cbZone.SelectedIndex = 0;
                    CsConst.myRevBuf = new byte[1200];
                    blnIsSuccess = true;
                }
                else
                {
                    blnIsSuccess = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            return blnIsSuccess;
        }

        private void dafaulInfo()
        {
            ConnetionType = 0; //0 局域网，1 p2p, 2 server
            strGrpName = CsConst.MyUnnamed;
            strProName = CsConst.MyUnnamed;
            strUser = "user";
            strPWD = "user";
            strIP1 = "115.29.251.24";
            intPort1 = 9999;
            strIP2 = "0.0.0.0";
            intPort2 = 9999;
            intTimer = 1;
        }

        private void tbPort1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar) || e.KeyChar==(char)8)
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void cbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            tbGroup.Enabled = (cbType.SelectedIndex >= 2);
            tbProject.Enabled = (cbType.SelectedIndex >= 2);
            tbUser.Enabled = (cbType.SelectedIndex >= 2);
            tbPWD.Enabled = (cbType.SelectedIndex >= 2);
            tbTimer.Enabled = (cbType.SelectedIndex == 2);
            mtbSev1.Enabled = (cbType.SelectedIndex == 1 || cbType.SelectedIndex == 2);
            tbPort1.Enabled = (cbType.SelectedIndex == 1 || cbType.SelectedIndex == 2);
            mtbSev2.Enabled = (cbType.SelectedIndex == 1 || cbType.SelectedIndex == 2);
            tbPort2.Enabled = (cbType.SelectedIndex == 1 || cbType.SelectedIndex == 2);
            cbZone.Enabled = (cbType.SelectedIndex == 2); 
            URL.Visible = (cbType.SelectedIndex ==3);
            if (cbType.SelectedIndex == 2) lbZone.Text = "Time Zone:";
            else lbZone.Text = "URL:";
        }

        private void mtbSev1_KeyPress(object sender, KeyPressEventArgs e)
        {
            cbType_SelectedIndexChanged(null, null);
        }

        private void tbTimer_Leave(object sender, EventArgs e)
        {
            string str = tbTimer.Text;
            int num = Convert.ToInt32(tbTimer.Text);
            tbTimer.Text = HDLPF.IsNumStringMode(str, 1, 4);
            tbTimer.SelectionStart = tbTimer.Text.Length;
        }

        public void btnRead_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                lvHistory.Items.Clear();
                int Count = 0;
                byte[] arayTmp = new byte[1];
                arayTmp[0] = 1;
                if (CsConst.mySends.AddBufToSndList(arayTmp, 0x3017, bytSubID, bytDevID, false, true, true, false) == true)
                {
                    Count = CsConst.myRevBuf[26];
                    CsConst.myRevBuf = new byte[1200];
                }
                else
                {
                    Cursor.Current = Cursors.Default;
                    return;
                }

                if (Count == 0)
                {
                    MessageBox.Show(CsConst.mstrINIDefault.IniReadValue("Public", "99780", ""), ""
                                                 , MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                }
                else
                {
                    for (int i = 1; i <= Count; i++)
                    {
                        arayTmp[0] = Convert.ToByte(i);
                        if (CsConst.mySends.AddBufToSndList(arayTmp, 0x3017, bytSubID, bytDevID, false, true, true, false) == true)
                        {
                            string strIP = CsConst.myRevBuf[27].ToString() + "." + CsConst.myRevBuf[28].ToString() + "." +
                                         CsConst.myRevBuf[29].ToString() + "." + CsConst.myRevBuf[30].ToString();
                            string strPort = (CsConst.myRevBuf[31] * 256 + CsConst.myRevBuf[32]).ToString();
                            string strDate = (CsConst.myRevBuf[33] + 2000).ToString() + "/" + CsConst.myRevBuf[34].ToString() + "/"
                                           + CsConst.myRevBuf[35].ToString() + "-" + CsConst.myRevBuf[36].ToString() + ":"
                                           + CsConst.myRevBuf[37].ToString() + ":" + CsConst.myRevBuf[38].ToString();
                            byte[] arayRemark = new byte[16];
                            Array.Copy(CsConst.myRevBuf, 39, arayRemark, 0, 16);
                            string strRemark = HDLPF.Byte2String(arayRemark);
                            ListViewItem oTmp = new ListViewItem();
                            oTmp.Text = (i.ToString());
                            oTmp.SubItems.Add(strDate); 
                            oTmp.SubItems.Add(strRemark); 
                            oTmp.SubItems.Add(strIP);
                            oTmp.SubItems.Add(strPort); 
                            //object[] obj = new object[] { i.ToString(), strDate, strRemark, strIP, strPort };
                            lvHistory.Items.Insert(0,oTmp);
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

        private void cbZone_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Connetion_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            byte[] ArayTmp = new byte[0];
            lvActive.Items.Clear();
            
            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x3019, bytSubID, bytDevID, false, true, true, false) == true)
            {
                int currentActive = 0;
                for (int i = 0; i < 3; i++)
                {
                    if (CsConst.myRevBuf[25 + i * 10] == 1)
                    {
                        currentActive++;
                        ListViewItem oLv = new ListViewItem();
                        oLv.Text = currentActive.ToString();
                        string ConnectIp = CsConst.myRevBuf[26 + i * 10].ToString() + "."
                                         + CsConst.myRevBuf[27 + i * 10].ToString() + "."
                                         + CsConst.myRevBuf[28 + i * 10].ToString() + "."
                                         + CsConst.myRevBuf[29 + i * 10].ToString();
                        string Port = (CsConst.myRevBuf[30 + i * 10] * 256 + CsConst.myRevBuf[31 + i * 10]).ToString();
                        string RemainsSeconds = (CsConst.myRevBuf[32 + i * 10] * 256 + CsConst.myRevBuf[33 + i * 10]).ToString();
                        oLv.SubItems.Add(ConnectIp);
                        oLv.SubItems.Add(Port);
                        oLv.SubItems.Add(RemainsSeconds);
                        lvActive.Items.Add(oLv);
                    }
                }

                if (lvActive.Items.Count > 0)
                {
                    lbActive.Text = CsConst.mstrINIDefault.IniReadValue("Menu", "00233", "") + lvActive.Items.Count.ToString() + ";" + "\r\n"
                                  + CsConst.mstrINIDefault.IniReadValue("Menu", "00234", "") + (4 - lvActive.Items.Count).ToString();
                }
                CsConst.myRevBuf = new byte[1200];
            }
            else
            {
                return;
            }
            Cursor.Current = Cursors.Default;
        }
    }
}
