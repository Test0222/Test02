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
    public partial class frmDS : Form
    {
        private string myDevName = null;
        private int mintIDIndex = -1;
        private byte SubNetID;
        private byte DevID;
        private int MyintDeviceType;
        private DS myDS = new DS();
        private int MyActivePage;
        private bool isRead = false;
        private int SelectCardType = 0;
        NetworkInForm networkinfo;
        private PictureBox myPicTmp;
        public frmDS()
        {
            InitializeComponent();
        }

        public frmDS(DS myds, string strName, int intDIndex, int intDeviceType)
        {
            InitializeComponent();
            this.myDevName = strName;
            this.mintIDIndex = intDIndex;
            string strDevName = strName.Split('\\')[0].ToString();
            this.MyintDeviceType = intDeviceType;
            this.myDS = myds;

            HDLSysPF.DisplayDeviceNameModeDescription(strName, MyintDeviceType, cboDevice, tbModel, tbDescription);

            SubNetID = Convert.ToByte(strDevName.Split('-')[0]);
            DevID = Convert.ToByte(strDevName.Split('-')[1]);
            tsl3.Text = strName;
            this.Text = strName;
        }


        void InitialFormCtrlsTextOrItems()
        {
            cbRelay.Items.Clear();
            cbRelay.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99769", ""));
            cbRelay.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99770", ""));
            cbRelay.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99771", ""));
            toolStrip1.Visible = (CsConst.MyEditMode == 0);
            cbHint.Items.Clear();
            for (int i = 0; i < 7; i++)
                cbHint.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "0036" + i.ToString(), ""));
            cbHint.SelectedIndex = 0;
            toolStrip1.Visible = false;
            pnlNet.Controls.Clear();
            networkinfo = new NetworkInForm(SubNetID, DevID, MyintDeviceType);
            pnlNet.Controls.Add(networkinfo);
            networkinfo.Dock = DockStyle.Fill;
            btnRefreshStatus_Click(null, null);
        }

        private void frmDS_Load(object sender, EventArgs e)
        {
            InitialFormCtrlsTextOrItems();
        }


        private void frmDS_Shown(object sender, EventArgs e)
        {
            if (CsConst.MyEditMode == 1)
            {
                MyActivePage = 1;
                tsbDown_Click(tsbDown, null);
            }
        }

        private void btnRef1_Click(object sender, EventArgs e)
        {
            if (tabDS.SelectedTab.Name == "tabRoom")
            {
                MyActivePage = 6;
            }
            tsbDown_Click(tsbDown, null);
        }

        private void btnSave1_Click(object sender, EventArgs e)
        {
            if (tabDS.SelectedTab.Name == "tabRoom")
            {
                MyActivePage = 6;
            }
            tsbDown_Click(tsbUpload, null);
        }

        private void btnSaveAndClose1_Click(object sender, EventArgs e)
        {
            if (tabDS.SelectedTab.Name == "tabRoom")
            {
                MyActivePage = 6;
            }
            tsbDown_Click(tsbUpload, null);
            this.Close();
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

                    CsConst.MyUPload2DownLists = new List<byte[]>();
                    int num1 = 1;
                    int num2 = 1;
                    if (tabDS.SelectedTab.Name == "tabRoom")
                    {
                        num1 = Convert.ToByte(txtFrm.Text);
                        num2 = Convert.ToByte(txtTo.Text);
                    }
                    else if (tabDS.SelectedTab.Name == "tabHistory")
                    {
                        num1 = Convert.ToByte(txtH1.Text);
                        num2 = Convert.ToByte(txtH2.Text);
                    }
                    string strName = myDevName.Split('\\')[0].ToString();
                    byte bytSubID = byte.Parse(strName.Split('-')[0]);
                    byte bytDevID = byte.Parse(strName.Split('-')[1]);
                    byte[] ArayRelay = new byte[] { SubNetID, DevID, (byte)(MyintDeviceType / 256), (byte)(MyintDeviceType % 256)
                        , (byte)MyActivePage,(byte)(mintIDIndex/256),(byte)(mintIDIndex%256),(byte)num1,(byte)num2 };
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
            switch (tabDS.SelectedTab.Name)
            {
                case "tabBasic": showBasicInfo(); break;
                case "tabRoom": showRoomSetting(); break;
                case "tabHistory": showHistory(); break;
                case "tabCard": showCardInfo(); break;
            }
            Cursor.Current = Cursors.Default;
            this.TopMost = true;
            this.WindowState = FormWindowState.Normal;
            this.Activate();
            this.TopMost = false;
        }

        private void showBasicInfo()
        {
            try
            {
                isRead = true;
                txtCallNum.Text = myDS.strNO;
                txtPassword.Text = myDS.strPassword;
                chbSound.Checked = (myDS.arayBasic[4] == 1);
                for (int i = 0; i < 4; i++)
                {
                    System.Windows.Forms.Panel temp = this.Controls.Find("pnlC" + (i + 1).ToString(), true)[0] as System.Windows.Forms.Panel;

                    switch (myDS.arayBasic[i])
                    {
                        case 1: temp.BackColor = Color.Red; break;
                        case 2: temp.BackColor = Color.Green; break;
                        case 3: temp.BackColor = Color.Blue; break;
                        case 4: temp.BackColor = Color.Yellow; break;
                        case 5: temp.BackColor = Color.Cyan; break;
                        case 6: temp.BackColor = Color.Purple; break;
                        case 7: temp.BackColor = Color.White; break;
                    }
                }
                txtRelay.Text = myDS.arayInfo[0].ToString();
                cbRelay.SelectedIndex = myDS.arayInfo[1];
                byte[] arayTmp = new byte[4];
                Array.Copy(myDS.arayCall, 0, arayTmp, 0, 4);
                txtZone.Text = GlobalClass.AddLeftZero(HDLPF.Byte2String(arayTmp), 4);
                chb1.Checked = (myDS.arayCall[4] == 1);
                txtConn.Text = myDS.arayCall[5].ToString();
                chb2.Checked = (myDS.arayCall[6] == 1);
                ip1.Text = myDS.arayCall[7].ToString() + "." + myDS.arayCall[8].ToString() + "."
                         + myDS.arayCall[9].ToString() + "." + myDS.arayCall[10].ToString();
                chbEco.Checked = myDS.arayBrightness[2] != 0;
                if (chbEco.Checked && myDS.arayBrightness[2] >= tbLCDelay.Minimum && myDS.arayBrightness[2] <= tbLCDelay.Maximum) tbLCDelay.Value = myDS.arayBrightness[2];
                if (myDS.arayBrightness[0] <= sb3.Maximum)
                    sb3.Value = myDS.arayBrightness[0];
                if (myDS.arayBrightness[1] <= sb4.Maximum)
                    sb4.Value = myDS.arayBrightness[1];
                if (myDS.arayBrightness[3] <= sb1.Maximum)
                    sb1.Value = myDS.arayBrightness[3];
                if (myDS.arayBrightness[4] <= sb2.Maximum)
                    sb2.Value = myDS.arayBrightness[4];
                chbEco_CheckedChanged(null, null);
                sb1_ValueChanged(null, null);
                sb2_ValueChanged(null, null);

                for (int i = 0; i < 4; i++)
                {
                    TextBox txtTmp = this.Controls.Find("txtP" + (i + 1).ToString(), true)[0] as TextBox;
                    byte bytLen = myDS.arayPassword[i * 12 + 1];
                    if (bytLen > 9)
                        txtTmp.Text = "";
                    else
                    {
                        string strPassword = "";
                        for (int j = 0; j < bytLen; j++)
                        {
                            strPassword = strPassword + myDS.arayPassword[i * 12 + 2 + j].ToString();
                        }
                        txtTmp.Text = strPassword;
                    }
                }
            }
            catch
            {
            }
            isRead = false;
        }

        private void showRoomSetting()
        {
            try
            {
           
                dgvRoom.Rows.Clear();
                if (myDS == null) return;
                if (myDS.confiinfo == null) return;
                txtCount.Text = myDS.RoomCount.ToString();
                if (myDS.RoomCount < 1) myDS.RoomCount = 1;
                lbRoom.Text = lbRoom.Text.Split('-')[0].ToString() + "-" + myDS.RoomCount.ToString() + "):";
                isRead = true;
                for (int i = 0; i < myDS.confiinfo.Count; i++)
                {
                    Image img = null;
                    for (int j = 0; j < myDS.ImageList.Count; j++)
                    {
                        byte[] arayImageTmp = myDS.ImageList[j];
                        if (arayImageTmp[0] == Convert.ToByte(myDS.confiinfo[i].ID / 256) &&
                            arayImageTmp[1] == Convert.ToByte(myDS.confiinfo[i].ID % 256))
                        {
                            byte[] arayTmp = new byte[320];
                            Array.Copy(arayImageTmp, 2, arayTmp, 0, 320);
                            img = HDLPF.ByteToImage(arayTmp, 160, 16, MyintDeviceType);
                            break;
                        }
                    }
                    object[] obj = new object[] { myDS.confiinfo[i].ID.ToString(),myDS.confiinfo[i].RoomNum.ToString(),
                        myDS.confiinfo[i].Remark,CsConst.mstrINIDefault.IniReadValue("Public", "99537", ""),
                        CsConst.mstrINIDefault.IniReadValue("Public", "99536", ""),
                        img};
                    dgvRoom.Rows.Add(obj);
                }
                
            }
            catch
            {
            }
            isRead = false;
        }

        private void showHistory()
        {
            try
            {
                dgvHistory.Rows.Clear();
                if (myDS == null) return;
                if (myDS.HistoryCount == 0)
                {
                    lbHistoryCount.Text = myDS.HistoryCount.ToString();
                    lbTarget.Text = CsConst.mstrINIDefault.IniReadValue("Public", "99798", "");
                }
                else
                {
                    lbHistoryCount.Text = myDS.HistoryCount.ToString();
                    lbTarget.Text = CsConst.mstrINIDefault.IniReadValue("Public", "99798", "") + "(1-" + lbHistoryCount.Text + "):";
                    txtFrm.Enabled = true;
                    txtTo.Enabled = true;
                    btnSure.Enabled = true;
                    if (Convert.ToInt32(txtTo.Text) > myDS.HistoryCount) txtTo.Text = myDS.HistoryCount.ToString();
                    if (Convert.ToInt32(txtFrm.Text) > myDS.HistoryCount) txtFrm.Text = txtTo.Text;
                    if (Convert.ToInt32(txtFrm.Text) <= 0) txtFrm.Text = "1";
                    if (myDS.MyHistory == null) return;
                    for (int i = 0; i < myDS.MyHistory.Count; i++)
                    {
                        DS.History temp = myDS.MyHistory[i];
                        string strTime = (temp.arayDate[0] + 2000).ToString() + "/" + temp.arayDate[1].ToString() + "/" +
                                          temp.arayDate[2].ToString() + " " + temp.arayDate[3].ToString("D2") + ":" +
                                          temp.arayDate[4].ToString("D2") + ":" + temp.arayDate[5].ToString("D2");
                        string strType = CsConst.mstrINIDefault.IniReadValue("Public", "00354", "");
                        strType = CsConst.mstrINIDefault.IniReadValue("Public", "0035" + (temp.Type - 1).ToString(), "");
                        string strInfo = "";
                        string strTmp = "";
                        if (temp.Type == 1)
                        {
                            strInfo = CsConst.mstrINIDefault.IniReadValue("Public", "0031" + temp.arayInfo[0].ToString(), "") + "   " +
                                      CsConst.mstrINIDefault.IniReadValue("Public", "99797", "") + ":" +
                                      (temp.arayInfo[1] * 256 + temp.arayInfo[2]).ToString();
                        }
                        else if (temp.Type == 2)
                        {
                            strInfo = CsConst.mstrINIDefault.IniReadValue("Public", "0032" + temp.arayInfo[1].ToString(), "");
                        }
                        else if (temp.Type == 3)
                        {
                            for (int j = 0; j < temp.arayInfo[8]; j++)
                            {
                                if ((9 + j) < temp.arayInfo.Length)
                                    strTmp = strTmp + GlobalClass.AddLeftZero(temp.arayInfo[9 + j].ToString("X"), 2);
                            }
                            strInfo = CsConst.mstrINIDefault.IniReadValue("Public", "99796", "") + ":" + (temp.arayInfo[0] * 256 + temp.arayInfo[1]).ToString() + "   " +
                                      CsConst.mstrINIDefault.IniReadValue("Public", "99795", "") + ":" + (temp.arayInfo[2] * 256 + temp.arayInfo[3]).ToString() + "   " +
                                      CsConst.mstrINIDefault.IniReadValue("Public", "99794", "") + ":" + (temp.arayInfo[4] * 256 + temp.arayInfo[5]).ToString() + "   " +
                                      CsConst.mstrINIDefault.IniReadValue("Public", "99797", "") + ":" + (temp.arayInfo[6] * 256 + temp.arayInfo[7]).ToString() + "   " +
                                      "UID" + CsConst.mstrINIDefault.IniReadValue("Public", "99793", "") + ":" + temp.arayInfo[8].ToString() + "  " +
                                      "UID" + CsConst.mstrINIDefault.IniReadValue("Public", "99792", "") + ":" + strTmp;
                        }
                        else if (temp.Type == 4)
                        {
                            if (temp.arayInfo[0] == 1) strTmp = CsConst.mstrINIDefault.IniReadValue("Public", "00038", "");
                            else if (temp.arayInfo[0] == 0) strTmp = CsConst.mstrINIDefault.IniReadValue("Public", "00037", "");

                            strInfo = CsConst.mstrINIDefault.IniReadValue("Public", "99791", "") + ":" + strTmp + "   " +
                                      CsConst.mstrINIDefault.IniReadValue("Public", "99790", "") + ":" +
                                      CsConst.mstrINIDefault.IniReadValue("Public", "0033" + temp.arayInfo[1].ToString(), "");
                        }
                        else if (temp.Type == 5)
                        {
                            strType = CsConst.mstrINIDefault.IniReadValue("Public", "00352", "");
                            strInfo = CsConst.mstrINIDefault.IniReadValue("Public", "99768", "") + ":" +
                                      CsConst.mstrINIDefault.IniReadValue("Public", "00354", "");
                        }
                        else if (temp.Type == 10)
                        {
                            strInfo = CsConst.mstrINIDefault.IniReadValue("Public", "99789", "") + ":" +
                                      CsConst.mstrINIDefault.IniReadValue("Public", "0034" + temp.arayInfo[0].ToString(), "");
                        }
                        object[] obj = new object[] { temp.ID.ToString(), strTime, strType, strInfo };
                        dgvHistory.Rows.Add(obj);
                    }
                }
            }
            catch
            {
            }
            isRead = false;
        }

        private void showCardInfo()
        {
            try
            {
                if (myDS == null) return;
                isRead = true;
                SetCount();
                btnValid_Click(btnValid, null);
            }
            catch
            {
            }
            isRead = false;
        }

        public void SetCount()
        {
            try
            {
                int ValidCount = 0;
                int LostCount = 0;
                int LimitCount = 0;
                for (int i = 0; i < myDS.MyCardInfo.Count; i++)
                {
                    NewDS.CardInfo temp = myDS.MyCardInfo[i];
                    if (temp.CardType == 1) ValidCount = ValidCount + 1;
                    else if (temp.CardType == 2) LostCount = LostCount + 1;
                    else if (temp.CardType == 3) LimitCount = LimitCount + 1;
                }
                lbValidCount.Text = ValidCount.ToString();
                lbLossCount.Text = LostCount.ToString();
                lbLimitCount.Text = LimitCount.ToString();
            }
            catch
            {
            }
        }

        private void txtFrm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar) || e.KeyChar == 8)
                e.Handled = false;
            else
                e.Handled = true;
        }

        private void txtFrm_Leave(object sender, EventArgs e)
        {
            string str = txtFrm.Text;
            string strCount = lbRoom.Text.Split('-')[1].ToString();
            strCount = strCount.Replace(")", "");
            strCount = strCount.Replace(":", "");
            int num = Convert.ToInt32(strCount);
            if (num < 1) num = 1;
            if (HDLPF.IsRightNumStringMode(str, 1, num))
            {
                txtFrm.SelectionStart = txtFrm.Text.Length;
            }
            else
            {
                if (txtFrm.Text == null || txtFrm.Text == "")
                {
                    txtFrm.Text = "1";
                }
                else
                {
                    txtTo.Text = txtFrm.Text;
                    txtTo.Focus();
                    txtTo.SelectionStart = txtTo.Text.Length;
                }
            }
        }

        private void txtTo_Leave(object sender, EventArgs e)
        {
            string str = txtTo.Text;
            int num = Convert.ToInt32(txtFrm.Text);
            string strCount = lbRoom.Text.Split('-')[1].ToString();
            strCount = strCount.Replace(")", "");
            strCount = strCount.Replace(":", "");
            int Count = Convert.ToInt32(strCount);
            if (num > Count) num = Count;
            txtTo.Text = HDLPF.IsNumStringMode(str, num, Count);
            txtTo.SelectionStart = txtTo.Text.Length;
        }

        private void btnPC_Click(object sender, EventArgs e)
        {
            DateTime d1;
            d1 = DateTime.Now;
            numTime1.Value = Convert.ToDecimal(d1.Hour);
            numTime2.Value = Convert.ToDecimal(d1.Minute);
            numTime3.Value = Convert.ToDecimal(d1.Second);
            DatePicker.Text = d1.ToString();
        }

        private void btnRefreshStatus_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                byte[] arayTmp = new byte[0];
                if (CsConst.mySends.AddBufToSndList(arayTmp, 0x3521, SubNetID, DevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(MyintDeviceType)) == true)
                {
                    byte bytMonth = CsConst.myRevBuf[26];
                    byte bytDay = CsConst.myRevBuf[27];
                    byte bytHour = CsConst.myRevBuf[28];
                    byte bytMinute = CsConst.myRevBuf[29];
                    byte bytSecond = CsConst.myRevBuf[30];
                    DateTime d1 = new DateTime(2000 + CsConst.myRevBuf[25], bytMonth, bytDay);
                    if (bytHour > 23) bytHour = 0;
                    if (bytMinute > 59) bytMinute = 0;
                    if (bytSecond > 59) bytSecond = 0;
                    if (bytMonth > 12) bytMonth = 1;
                    if (bytDay > 31) bytDay = 1;
                    numTime1.Value = Convert.ToDecimal(bytHour);
                    numTime2.Value = Convert.ToDecimal(bytMinute);
                    numTime3.Value = Convert.ToDecimal(bytSecond);
                    DatePicker.Text = d1.ToString();
                    
                }
            }
            catch
            {
            }
            Cursor.Current = Cursors.Default;
        }

        private void btnSaveStatus_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                byte[] arayTmp = new byte[7];
                arayTmp[0] = Convert.ToByte(DatePicker.Value.Year - 2000);
                arayTmp[1] = Convert.ToByte(DatePicker.Value.Month);
                arayTmp[2] = Convert.ToByte(DatePicker.Value.Day);
                arayTmp[3] = Convert.ToByte(numTime1.Value);
                arayTmp[4] = Convert.ToByte(numTime2.Value);
                arayTmp[5] = Convert.ToByte(numTime3.Value);
                arayTmp[6] = Convert.ToByte(DatePicker.Value.DayOfWeek.GetHashCode());
                if (CsConst.mySends.AddBufToSndList(arayTmp, 0x3523, SubNetID, DevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(MyintDeviceType)) == true)
                {
                    
                }
            }
            catch
            {
            }
            Cursor.Current = Cursors.Default;
        }

        private void DatePicker_ValueChanged(object sender, EventArgs e)
        {
            DayOfWeek Week = DatePicker.Value.DayOfWeek;
            label5.Text = CsConst.mstrINIDefault.IniReadValue("Public", "0075" + Week.GetHashCode().ToString(), "");
        }

        private void btnReadRoom_Click(object sender, EventArgs e)
        {
            MyActivePage = 2;
            tsbDown_Click(tsbDown, null);
        }

        private void tabDS_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabDS.SelectedTab.Name == "tabBasic") MyActivePage = 1;
            else if (tabDS.SelectedTab.Name == "tabRoom") MyActivePage = 2;
            else if (tabDS.SelectedTab.Name == "tabCard") MyActivePage = 3;
            else if (tabDS.SelectedTab.Name == "tabHistory") MyActivePage = 4;
            
            if (myDS.MyRead2UpFlags[MyActivePage - 1] == false)
            {
                tsbDown_Click(tsbDown, null);
            }
            else
            {
                if (tabDS.SelectedTab.Name == "tabRoom") showRoomSetting();
                else if (tabDS.SelectedTab.Name == "tabHistory") showHistory();
                else if (tabDS.SelectedTab.Name == "tabCard") showCardInfo();
                else if (tabDS.SelectedTab.Name == "tabBasic") showBasicInfo();
            }
        }

        private void sb1_ValueChanged(object sender, EventArgs e)
        {
            lb1.Text = sb1.Value.ToString();
            if (isRead) return;
            if (myDS == null) return;
            if (myDS.arayBrightness == null) return;
            myDS.arayBrightness[3] = Convert.ToByte(sb1.Value);
        }

        private void sb2_ValueChanged(object sender, EventArgs e)
        {
            lb2.Text = sb2.Value.ToString();
            if (isRead) return;
            if (myDS == null) return;
            if (myDS.arayBrightness == null) return;
            myDS.arayBrightness[4] = Convert.ToByte(sb2.Value);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 4; i++)
            {
                System.Windows.Forms.Panel temp = this.Controls.Find("pnlC" + (i + 1).ToString(), true)[0] as System.Windows.Forms.Panel;
                if (temp.BackColor == Color.Red)
                {
                    myDS.arayBasic[i] = 1;
                }
                else if (temp.BackColor == Color.Green)
                {
                    myDS.arayBasic[i] = 2;
                }
                else if (temp.BackColor == Color.Blue)
                {
                    myDS.arayBasic[i] = 3;
                }
                else if (temp.BackColor == Color.Yellow)
                {
                    myDS.arayBasic[i] = 4;
                }
                else if (temp.BackColor == Color.Cyan)
                {
                    myDS.arayBasic[i] = 5;
                }
                else if (temp.BackColor == Color.Purple)
                {
                    myDS.arayBasic[i] = 6;
                }
                else if (temp.BackColor == Color.White)
                {
                    myDS.arayBasic[i] = 7;
                }
            }
            string[] strTmp = ip1.Text.Split('.');
            for (int i = 0; i < 4; i++)
            {
                if (strTmp[i] != null && strTmp[i] != "")
                    myDS.arayCall[i + 7] = byte.Parse(strTmp[i].ToString());
            }
            tsbDown_Click(tsbUpload, null);
        }

        private void btnSaveAndClose_Click(object sender, EventArgs e)
        {
            button2_Click(null, null);
            this.Close();
        }

        private void pnlC3_Click(object sender, EventArgs e)
        {
            int tag = Convert.ToInt32((sender as System.Windows.Forms.Panel).Tag);
            FrmColorForDS frmTmp = new FrmColorForDS(myDS.arayBasic[tag], myDS, tag);
            DialogResult = DialogResult.Cancel;
            if (frmTmp.ShowDialog() == DialogResult.OK)
            {
                showBasicInfo();
            }
        }

        private void txtRelay_Leave(object sender, EventArgs e)
        {
            string str = txtRelay.Text;
            txtRelay.Text = HDLPF.IsNumStringMode(str, 0, 255);
            txtRelay.SelectionStart = txtRelay.Text.Length;
            if (isRead) return;
            if (myDS == null) return;
            if (myDS.arayInfo == null) return;
            myDS.arayInfo[0] = Convert.ToByte(txtRelay.Text);
        }

        private void cbRelay_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            if (myDS == null) return;
            if (myDS.arayInfo == null) return;
            myDS.arayInfo[1] = Convert.ToByte(cbRelay.SelectedIndex);
        }

        private void tbLCDelay_ValueChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            if (myDS == null) return;
            if (myDS.arayBrightness == null) return;
            myDS.arayBrightness[2] = Convert.ToByte(tbLCDelay.Value);
        }

        private void btnValid_Click(object sender, EventArgs e)
        {
            try
            {
                if (myDS == null) return;
                if (myDS.MyCardInfo == null) return;
                btnValid.BackColor = Color.Transparent;
                btnLost.BackColor = Color.Transparent;
                btnFobib.BackColor = Color.Transparent;
                int Width = panel19.Width;
                int Heigh = panel19.Height;
                int Tag = Convert.ToInt32((sender as Button).Tag);
                (sender as Button).BackColor = Color.Yellow;
                SelectCardType = Tag;
                panel19.Controls.Clear();
                int WCount = Width / 150;
                int num = 0;
                for (int i = 0; i < myDS.MyCardInfo.Count; i++)
                {
                    NewDS.CardInfo temp = myDS.MyCardInfo[i];
                    if (temp.CardType == Convert.ToByte(Tag))
                    {
                        UserForNewDoor tmp = new UserForNewDoor(myDS, i, MyintDeviceType, cbHint.SelectedIndex, myDevName, panel19, SelectCardType, this);
                        tmp.Name = "Card" + num.ToString();
                        tmp.Left = (num % WCount) * 140 + 10;
                        tmp.Top = (num / WCount) * 120 + 10;
                        panel19.Controls.Add(tmp);
                        num = num + 1;
                    }
                }
            }
            catch
            {
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                bool isHaveEmptyCard = false;
                for (int i = 0; i < myDS.MyCardInfo.Count; i++)
                {
                    if (myDS.MyCardInfo[i].CardType == 0)
                    {
                        isHaveEmptyCard = true;
                        break;
                    }
                }
                if (isHaveEmptyCard)
                {
                    byte[] arayTmp = new byte[0];
                    byte[] arayUID = new byte[0];
                    CsConst.MyBlnNeedF8 = true;
                    if (CsConst.mySends.AddBufToSndList(arayTmp, 0x3529, SubNetID, DevID, false, false, true, CsConst.minAllWirelessDeviceType.Contains(MyintDeviceType)) == true)
                    {
                        arayUID = new byte[CsConst.myRevBuf[26] + 1];
                        Array.Copy(CsConst.myRevBuf, 26, arayUID, 0, arayUID.Length);
                    }
                    else
                    {
                        MessageBox.Show(CsConst.mstrINIDefault.IniReadValue("Public", "99779", ""), ""
                                                        , MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                        return;
                    }
                    bool isExit = false;
                    int ID = 0;
                    for (int i = 0; i < myDS.MyCardInfo.Count; i++)
                    {
                        if (myDS.MyCardInfo[i].UIDL == arayUID[0])
                        {
                            arayTmp = new byte[arayUID[0]];
                            Array.Copy(arayUID, 1, arayTmp, 0, arayTmp.Length);
                            bool isEqual = true;
                            for (int j = 0; j < arayTmp.Length; j++)
                            {
                                if (arayTmp[j] != myDS.MyCardInfo[i].UID[j])
                                {
                                    isEqual = false;
                                }

                            }
                            if (isEqual)
                            {
                                isExit = true;
                                ID = i;
                            }
                        }
                    }
                    if (isExit)
                    {
                        FrmAddNewCard frmTmp = new FrmAddNewCard(arayUID, myDS, MyintDeviceType, myDevName, 1, ID);
                        frmTmp.ShowDialog();
                        switch (SelectCardType)
                        {
                            case 1: btnValid_Click(btnValid, null); break;
                            case 2: btnValid_Click(btnLost, null); break;
                            case 3: btnValid_Click(btnFobib, null); break;
                        }
                        SetCount();
                    }
                    else
                    {
                        FrmAddNewCard frmTmp = new FrmAddNewCard(arayUID, myDS, MyintDeviceType, myDevName, 0, 0);
                        frmTmp.ShowDialog();
                        switch (SelectCardType)
                        {
                            case 1: btnValid_Click(btnValid, null); break;
                            case 2: btnValid_Click(btnLost, null); break;
                            case 3: btnValid_Click(btnFobib, null); break;
                        }
                        SetCount();
                    }
                }
                else
                {
                }
            }
            catch
            {
                MessageBox.Show(CsConst.mstrINIDefault.IniReadValue("Public", "99774", ""), ""
                                                    , MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            }
            Cursor.Current = Cursors.Default;
        }

        private void btnModify_Click(object sender, EventArgs e)
        {
            try
            {
                if (checkIsSelected())
                {
                    for (int i = 0; i < panel19.Controls.Count; i++)
                    {
                        UserForNewDoor temp = (UserForNewDoor)panel19.Controls[i];
                        if (temp.isSelect)
                        {
                            temp.ModifyInterFace();
                            break;
                        }
                    }
                    switch (SelectCardType)
                    {
                        case 1: btnValid_Click(btnValid, null); break;
                        case 2: btnValid_Click(btnLost, null); break;
                        case 3: btnValid_Click(btnFobib, null); break;
                    }
                    SetCount();
                }
                else
                {
                    MessageBox.Show(CsConst.mstrINIDefault.IniReadValue("Public", "99775", ""), ""
                                                        , MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                }
            }
            catch
            {
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                if (checkIsSelected())
                {
                    for (int i = 0; i < panel19.Controls.Count; i++)
                    {
                        UserForNewDoor temp = (UserForNewDoor)panel19.Controls[i];
                        if (temp.isSelect)
                        {
                            if (temp.DeleteCard())
                            {
                                switch (SelectCardType)
                                {
                                    case 1: btnValid_Click(btnValid, null); break;
                                    case 2: btnValid_Click(btnLost, null); break;
                                    case 3: btnValid_Click(btnFobib, null); break;
                                }
                                SetCount();
                                break;
                            }
                            else
                            {
                                Cursor.Current = Cursors.Default;
                                MessageBox.Show(CsConst.mstrINIDefault.IniReadValue("Public", "99776", ""), ""
                                                            , MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                                return;
                            }
                        }
                    }
                }
                else
                {
                    Cursor.Current = Cursors.Default;
                    MessageBox.Show(CsConst.mstrINIDefault.IniReadValue("Public", "99775", ""), ""
                                                        , MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                }
            }
            catch
            {
            }
            Cursor.Current = Cursors.Default;
        }

        private void cbHint_SelectedIndexChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < panel19.Controls.Count; i++)
            {
                UserForNewDoor temp = (UserForNewDoor)panel19.Controls[i];
                temp.LoadUser(cbHint.SelectedIndex);

            }
        }

        private bool checkIsSelected()
        {
            bool isSelected = false;
            for (int i = 0; i < panel19.Controls.Count; i++)
            {
                UserForNewDoor temp = (UserForNewDoor)panel19.Controls[i];
                if (temp.isSelect)
                {
                    isSelected = true;
                    break;
                }
            }
            return isSelected;
        }

        private void btnSure_Click(object sender, EventArgs e)
        {
            tsbDown_Click(tsbDown, null);
        }

        private void txtH1_Leave(object sender, EventArgs e)
        {
            string str = txtH1.Text;
            int num = Convert.ToInt32(txtH2.Text);
            if (HDLPF.IsRightNumStringMode(str, 1, num))
            {
                txtH1.SelectionStart = txtH1.Text.Length;
            }
            else
            {
                if (txtH1.Text == null || txtH1.Text == "")
                {
                    txtH1.Text = "1";
                }
                else
                {
                    txtH2.Text = txtH1.Text;
                    txtH2.Focus();
                    txtH2.SelectionStart = txtH2.Text.Length;
                }
            }
        }

        private void txtH2_Leave(object sender, EventArgs e)
        {
            string str = txtH2.Text;
            int num = Convert.ToInt32(txtH1.Text);
            txtH2.Text = HDLPF.IsNumStringMode(str, num, Convert.ToInt32(lbHistoryCount.Text));
            txtH2.SelectionStart = txtH2.Text.Length;
        }

        private void txtCallNum_TextChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            string str = txtCallNum.Text;
            str = GlobalClass.AddLeftZero(str, 4);
            myDS.strNO = str;
        }

        private void chb1_CheckedChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            if (chb1.Checked)
                myDS.arayCall[4] = 1;
            else
                myDS.arayCall[4] = 0;
        }

        private void chb2_CheckedChanged(object sender, EventArgs e)
        {

            if (isRead) return;
            if (chb2.Checked)
                myDS.arayCall[6] = 1;
            else
                myDS.arayCall[6] = 0;
        }

        private void txtZone_TextChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            string str = txtZone.Text;
            str = GlobalClass.AddLeftZero(str, 4);
            byte[] arayTmp = HDLUDP.StringToByte(str);
            if (arayTmp.Length <= 4)
                Array.Copy(arayTmp, 0, myDS.arayCall, 0, arayTmp.Length);
            else
                Array.Copy(arayTmp, 0, myDS.arayCall, 0, 4);
        }

        private void txtConn_TextChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            if (txtConn.Text.Length > 0)
            {
                string str = txtConn.Text;
                myDS.arayCall[5] = Convert.ToByte(txtConn.Text);
            }
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            string str = txtPassword.Text;
            str = GlobalClass.AddLeftZero(str, 6);
            myDS.strPassword = str;
        }

        private void sb3_ValueChanged(object sender, EventArgs e)
        {
            lb3.Text = sb3.Value.ToString();
            if (isRead) return;
            if (myDS == null) return;
            if (myDS.arayBrightness == null) return;
            myDS.arayBrightness[0] = Convert.ToByte(sb3.Value);
        }

        private void sb4_ValueChanged(object sender, EventArgs e)
        {
            lb4.Text = sb4.Value.ToString();
            if (isRead) return;
            if (myDS == null) return;
            if (myDS.arayBrightness == null) return;
            myDS.arayBrightness[1] = Convert.ToByte(sb4.Value);
        }

        private void chbEco_CheckedChanged(object sender, EventArgs e)
        {
            pnleco.Visible = chbEco.Checked;
            if (isRead) return;
            if (myDS == null) return;
            if (myDS.arayBrightness == null) return;
            if (chbEco.Checked)
                myDS.arayBrightness[2] = Convert.ToByte(tbLCDelay.Value);
            else
                myDS.arayBrightness[2] = 0;

        }

        private void dgvRoom_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            dgvRoom.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void tmRead_Click(object sender, EventArgs e)
        {
            tsbDown_Click(tsbDown, null);
        }

        private void dgvRoom_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (isRead) return;
                if (myDS == null) return;
                if (myDS.confiinfo == null) return;
                if ((e.RowIndex == -1) || (e.ColumnIndex == -1)) return;
                if (e.ColumnIndex > 2) return;
                if (dgvRoom[e.ColumnIndex, e.RowIndex].Value == null ) dgvRoom[e.ColumnIndex, e.RowIndex].Value = "";
                for (int i = 0; i < dgvRoom.SelectedRows.Count; i++)
                {
                    string strTmp = "";
                    switch (e.ColumnIndex)
                    {
                        case 1:
                            strTmp = dgvRoom[1, dgvRoom.SelectedRows[i].Index].Value.ToString();
                            if (!HDLSysPF.IsNumeric(strTmp)||strTmp=="") strTmp = "1";
                            dgvRoom[1, dgvRoom.SelectedRows[i].Index].Value = strTmp;
                            myDS.confiinfo[dgvRoom.SelectedRows[i].Index].RoomNum = Convert.ToUInt64(strTmp);
                            break;
                        case 2:
                            strTmp = dgvRoom[2, dgvRoom.SelectedRows[i].Index].Value.ToString();
                            dgvRoom[2, dgvRoom.SelectedRows[i].Index].Value = strTmp;
                            myDS.confiinfo[dgvRoom.SelectedRows[i].Index].Remark = strTmp;
                            break;
                    }
                    dgvRoom.SelectedRows[i].Cells[e.ColumnIndex].Value = dgvRoom[e.ColumnIndex, e.RowIndex].Value.ToString();
                }
                if (e.ColumnIndex == 1)
                {
                    string strTmp = dgvRoom[1, e.RowIndex].Value.ToString();
                    if (!HDLSysPF.IsNumeric(strTmp) || strTmp == "") strTmp = "1";
                    dgvRoom[1, e.RowIndex].Value = strTmp;
                    myDS.confiinfo[e.RowIndex].RoomNum = Convert.ToUInt64(strTmp);
                }
                else if (e.ColumnIndex == 2)
                {
                    string strTmp = dgvRoom[2, e.RowIndex].Value.ToString();
                    dgvRoom[2, e.RowIndex].Value = strTmp;
                    myDS.confiinfo[e.RowIndex].Remark = strTmp;
                }
            }
            catch
            {
            }
        }

        private void txtP1_Leave(object sender, EventArgs e)
        {
            if (isRead) return;
            if (myDS == null) return;
            int Tag = Convert.ToInt32((sender as TextBox).Tag);
            string str = (sender as TextBox).Text;
            if (str.Length < 4) str = GlobalClass.AddLeftZero(str, 4);
            (sender as TextBox).Text = str;
            myDS.arayPassword[Tag * 12 + 1] = Convert.ToByte(str.Length);
            for (int i = 0; i < str.Length; i++)
                myDS.arayPassword[Tag * 12 + 2 + i] = Convert.ToByte(str[i].ToString());
            if (str.Length < 9)
            {
                for (int i = str.Length; i < 9; i++)
                    myDS.arayPassword[Tag * 12 + 2 + i] = 0;
            }
        }

        private void dgvRoom_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (e.ColumnIndex == 3)
                {
                    if (myDS.ImageList == null) myDS.ImageList = new List<byte[]>();
                    byte[] arayImge = new byte[323];
                    byte[] arayTmp = new byte[3];
                    int ID = Convert.ToInt32(dgvRoom[0, e.RowIndex].Value.ToString());
                    arayTmp[0] = Convert.ToByte(ID / 256);
                    arayTmp[1] = Convert.ToByte(ID % 256);
                    arayImge[0] = arayTmp[0];
                    arayImge[1] = arayTmp[1];
                    for (byte i = 0; i < 5; i++)
                    {
                        arayTmp[2] = i;
                        if (CsConst.mySends.AddBufToSndList(arayTmp, 0x353F, SubNetID, DevID, false, true, true, false) == true)
                        {
                            Array.Copy(CsConst.myRevBuf, 29, arayImge, 64 * i + 2, 64);
                            HDLUDP.TimeBetwnNext(1);
                        }
                        else
                        {
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                    }
                    for (int i = 0; i < myDS.ImageList.Count; i++)
                    {
                        byte[] arayImageTmp = myDS.ImageList[i];
                        if (arayImageTmp[0] == arayImge[0] && arayImageTmp[1] == arayImge[1])
                        {
                            myDS.ImageList.RemoveAt(i);
                            break;
                        }
                    }
                    myDS.ImageList.Add(arayImge);
                    arayTmp = new byte[320];
                    Array.Copy(arayImge, 2, arayTmp, 0, 320);
                    Image img = HDLPF.ByteToImage(arayTmp, 160, 16,MyintDeviceType);
                    dgvRoom[5, e.RowIndex].Value = img;
                }
                else if (e.ColumnIndex == 4)
                {
                    Image img = (Image)dgvRoom[5, e.RowIndex].Value;
                    if (img != null)
                    {
                        byte[] arayImage = HDLPF.ImageToByte((Bitmap)img, 160, 16, 4);
                        byte[] arayTmp = new byte[67];
                        int IDNum = Convert.ToInt32(dgvRoom[0, e.RowIndex].Value.ToString());
                        arayTmp[0] = Convert.ToByte(IDNum / 256);
                        arayTmp[1] = Convert.ToByte(IDNum % 256);
                        for (byte i = 0; i < 5; i++)
                        {
                            arayTmp[2] = i;
                            Array.Copy(arayImage, i * 64, arayTmp, 3, 64);
                            if (CsConst.mySends.AddBufToSndList(arayTmp, 0x3541, SubNetID, DevID, false, true, true, false) == true)
                            {
                                HDLUDP.TimeBetwnNext(20);
                            }
                            else
                            {
                                Cursor.Current = Cursors.Default;
                                return;
                            }
                        }
                        for (int i = 0; i < myDS.ImageList.Count; i++)
                        {
                            byte[] arayImageTmp = myDS.ImageList[i];
                            if (arayImageTmp[0] == Convert.ToByte(IDNum / 256) &&
                                arayImageTmp[1] == Convert.ToByte(IDNum % 256))
                            {
                                arayImageTmp[322] = 0;
                                break;
                            }
                        }
                    }
                }
            }
            catch
            {
            }
            Cursor.Current = Cursors.Default;
        }

        private void dgvRoom_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == 5)
                {
                    string MyPath = HDLPF.OpenFileDialog("BMP Image|*.bmp|JPEG Image|*.jpg|PNG Image|*.png", "Add Icons");
                    if (MyPath == null || MyPath == "") return;
                    Image img = Image.FromFile(MyPath);
                    if (img.Width != 160 || img.Height != 16)
                    {
                        MessageBox.Show(CsConst.mstrINIDefault.IniReadValue("Public", "99811", "") + "160*16");
                        return;
                    }
                    bool isAdd = true;
                    dgvRoom[5, e.RowIndex].Value = img;
                    int IDNum = Convert.ToInt32(dgvRoom[0, e.RowIndex].Value.ToString());
                    byte[] arayImagetmp = HDLPF.ImageToByte((Bitmap)img, 160, 16, 4);
                    for (int i = 0; i < myDS.ImageList.Count; i++)
                    {
                        byte[] arayImageTmp = myDS.ImageList[i];
                        if (arayImageTmp[0] == Convert.ToByte(IDNum / 256) &&
                            arayImageTmp[1] == Convert.ToByte(IDNum % 256))
                        {
                            arayImageTmp[322] = 1;
                            Array.Copy(arayImagetmp, 0, myDS.ImageList[i], 2, 320);
                            isAdd = false;
                            break;
                        }
                    }
                    if (isAdd)
                    {
                        byte[] arayImge = new byte[323];
                        arayImge[0] = Convert.ToByte(IDNum / 256);
                        arayImge[1] = Convert.ToByte(IDNum % 256);
                        Array.Copy(arayImagetmp, 0, arayImge, 2, 320);
                        arayImge[322] = 1;
                        myDS.ImageList.Add(arayImge);
                    }
                }
            }
            catch
            {
            }
        }

        private void txtCount_Leave(object sender, EventArgs e)
        {
            string str = txtCount.Text;
            txtCount.Text = HDLPF.IsNumStringMode(str, 1, 100);
            txtCount.SelectionStart = txtCount.Text.Length;
        }

        private void txtImage_TextChanged(object sender, EventArgs e)
        {
            string str = txtImage.Text;
            Font font = txtImage.Font;
            Bitmap image = new Bitmap(160, picImage.Height);
            picImageTmp.DrawToBitmap(image, new Rectangle(0, 0, 160, picImage.Height));
            Graphics g = Graphics.FromImage(image);
            System.Drawing.Brush brush = System.Drawing.Brushes.Black;
            PointF point = new PointF(0, 0);
            System.Drawing.StringFormat sf = new System.Drawing.StringFormat();
            sf.FormatFlags = StringFormatFlags.DisplayFormatControl;
            g.DrawString(str.ToString(), font, brush, point, sf);
            picImage.Image = image;
        }

        private void btnCount_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            byte[] arayTmp = new byte[2];
            int Count = Convert.ToInt32(txtCount.Text);
            arayTmp[0] = Convert.ToByte(Count / 256);
            arayTmp[1] = Convert.ToByte(Count % 256);
            if (CsConst.mySends.AddBufToSndList(arayTmp, 0x351E, SubNetID, DevID, false, true, true, false) == true)
            {
                myDS.RoomCount = Count;
                HDLUDP.TimeBetwnNext(20);
                lbRoom.Text = lbRoom.Text.Split('-')[0].ToString() + "-" + Count.ToString() + "):";
            }
            Cursor.Current = Cursors.Default;
        }

        private void btnReadAllImage_Click(object sender, EventArgs e)
        {
            MyActivePage = 5;
            tsbDown_Click(tsbDown, null);
        }

        private void btnSaveAllImage_Click(object sender, EventArgs e)
        {
            MyActivePage = 5;
            tsbDown_Click(tsbUpload, null);
        }

        private void btnFontColor_Click(object sender, EventArgs e)
        {
            if (Fonts.ShowDialog() == DialogResult.OK)
            {
                txtImage.Font = Fonts.Font;
                txtImage_TextChanged(null, null);
            }
        }

        private void btnReadCount_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            if (CsConst.mySends.AddBufToSndList(null, 0x351C, SubNetID, DevID, false, true, true, false) == true)
            {
                int Count = CsConst.myRevBuf[25] * 256 + CsConst.myRevBuf[26];
                if (Count > 100) Count = 100;
                myDS.RoomCount = Count;
                txtCount.Text = Count.ToString();
                lbRoom.Text = lbRoom.Text.Split('-')[0].ToString() + "-" + Count.ToString() + "):";
                HDLUDP.TimeBetwnNext(20);
            }
            Cursor.Current = Cursors.Default;
        }

        private void btnSaveRoom_Click(object sender, EventArgs e)
        {
            MyActivePage = 2;
            tsbDown_Click(tsbUpload, null);
        }

        private void txtCount_TextChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            myDS.RoomCount = Convert.ToInt32(txtCount.Text);
        }

        private void picImage_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                System.Windows.Forms.PictureBox Tmp = ((System.Windows.Forms.PictureBox)sender);
                if (Tmp == null) return;
                myPicTmp = Tmp;
                DoDragDrop(Tmp, DragDropEffects.Copy);
            }
        }

        private void dgvRoom_DragDrop(object sender, DragEventArgs e)
        {
            try
            {
                if (myPicTmp == null) return;
                System.Drawing.Bitmap partImg = new System.Drawing.Bitmap(160, 16);
                System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(partImg);
                System.Drawing.Rectangle destRect = new System.Drawing.Rectangle(new System.Drawing.Point(0, 0), new System.Drawing.Size(160, 16));//目标位置
                System.Drawing.Rectangle origRect = new System.Drawing.Rectangle(new System.Drawing.Point(0, 0), new System.Drawing.Size(160, 16));//原图位置（默认从原图中截取的图片大小等于目标图片的大小）
                graphics.DrawImage(myPicTmp.Image, destRect, origRect, System.Drawing.GraphicsUnit.Pixel);
                DataGridView.HitTestInfo hitinfo;
                Point point = dgvRoom.PointToClient(new Point(e.X, e.Y)); //坐标转换
                hitinfo = dgvRoom.HitTest(point.X, point.Y);
                int rowIndex = hitinfo.RowIndex;
                if (rowIndex >= 0 && rowIndex < dgvRoom.RowCount)
                {
                    dgvRoom[5, rowIndex].Value = partImg;
                    bool isAdd = true;
                    int IDNum = Convert.ToInt32(dgvRoom[0, rowIndex].Value.ToString());
                    byte[] arayImagetmp = HDLPF.ImageToByte(partImg, 160, 16, 4);
                    for (int i = 0; i < myDS.ImageList.Count; i++)
                    {
                        byte[] arayImageTmp = myDS.ImageList[i];
                        if (arayImageTmp[0] == Convert.ToByte(IDNum / 256) &&
                            arayImageTmp[1] == Convert.ToByte(IDNum % 256))
                        {
                            isAdd = false;
                            Array.Copy(arayImagetmp, 0, myDS.ImageList[i], 2, 320);
                            arayImageTmp[322] = 1;
                            break;
                        }
                    }
                    if (isAdd)
                    {
                        byte[] arayImge = new byte[323];
                        arayImge[0] = Convert.ToByte(IDNum / 256);
                        arayImge[1] = Convert.ToByte(IDNum % 256);
                        Array.Copy(arayImagetmp, 0, arayImge, 2, 320);
                        arayImge[322] = 1;
                        myDS.ImageList.Add(arayImge);
                    }
                }
            }
            catch
            {
            }
        }

        private void dgvRoom_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(System.Windows.Forms.PictureBox)))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void tabBasic_Click(object sender, EventArgs e)
        {

        }

    }
}
