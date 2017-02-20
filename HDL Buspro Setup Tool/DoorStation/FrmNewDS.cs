using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace HDL_Buspro_Setup_Tool
{
    public partial class FrmNewDS : Form
    {
        private string myDevName = null;
        private int mintIDIndex = -1;
        private byte SubNetID;
        private byte DevID;
        private int MyintDeviceType;
        private NewDS oNewDS = new NewDS();
        private int MyActivePage = 0; //按页面上传下载
        private int SelectCardType = 0;
        NetworkInForm networkinfo;
        private bool isRead = false;
        private BackgroundWorker MyBackGroup;
        FrmProcess frmProcessTmp;
        private byte[] SourceFile = null;
        private int FileIndex = 0;
        public FrmNewDS()
        {
            InitializeComponent();
        }

        public FrmNewDS(NewDS newds, string strName, int intDIndex, int intDeviceType)
        {
            InitializeComponent();
            this.myDevName = strName;
            this.mintIDIndex = intDIndex;
            string strDevName = strName.Split('\\')[0].ToString();
            this.MyintDeviceType = intDeviceType;
            this.oNewDS = newds;
            HDLSysPF.DisplayDeviceNameModeDescription(strName, MyintDeviceType, cboDevice, tbModel, tbDescription);
            SubNetID = Convert.ToByte(strDevName.Split('-')[0]);
            DevID = Convert.ToByte(strDevName.Split('-')[1]);
            tsl3.Text = strName;
            this.Text = strName;
            Control.CheckForIllegalCrossThreadCalls = false;
        }


        void InitialFormCtrlsTextOrItems()
        {
            cbBell.Items.Clear();
            for (int i = 50; i <= 60; i++)
                cbBell.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "008" + i.ToString(), ""));
            cbRelay.Items.Clear();
            cbRelay.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99769", ""));
            cbRelay.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99770", ""));
            cbRelay.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99771", ""));
            cbHint.Items.Clear();
            for (int i = 0; i < 7; i++)
                cbHint.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "0036" + i.ToString(), ""));
            cbHint.SelectedIndex = 0;
        }

        private void FrmNewDS_Load(object sender, EventArgs e)
        {
            InitialFormCtrlsTextOrItems();
        }

        private void tsbDown_Click(byte bytTag)
        {
            try
            {
                bool blnShowMsg = (CsConst.MyEditMode != 1);
                if (HDLPF.UploadOrReadGoOn(this.Text, bytTag, blnShowMsg))
                {
                    Cursor.Current = Cursors.WaitCursor;

                    CsConst.MyUPload2DownLists = new List<byte[]>();

                    string strName = myDevName.Split('\\')[0].ToString();
                    byte bytSubID = byte.Parse(strName.Split('-')[0]);
                    byte bytDevID = byte.Parse(strName.Split('-')[1]);
                    byte[] ArayRelay = new byte[] { SubNetID, DevID, (byte)(MyintDeviceType / 256), (byte)(MyintDeviceType % 256), (byte)MyActivePage,
                                                (byte)(mintIDIndex / 256), (byte)(mintIDIndex % 256),Convert.ToByte(txtFrm.Text),Convert.ToByte(txtTo.Text)};
                    CsConst.MyUPload2DownLists.Add(ArayRelay);
                    CsConst.MyUpload2Down = bytTag;
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
            switch (tabControl.SelectedIndex)
            {
                case 0: showBasicInfo(); break;   
                case 1: showCardInfo(); break;
                case 2: showHistory(); break;
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
                txtCallNum.Text = oNewDS.strNO;
                txtPassword.Text = oNewDS.strPassword;
                chbSound.Checked = (oNewDS.arayBasic[4] == 1);
                for (int i = 0; i < 4; i++)
                {
                    System.Windows.Forms.Panel temp = this.Controls.Find("pnlC" + (i + 1).ToString(), true)[0] as System.Windows.Forms.Panel;

                    switch (oNewDS.arayBasic[i])
                    {
                        case 1: temp.BackColor = Color.Red; break;
                        case 2: temp.BackColor = Color.FromArgb(0, 255, 0); break;
                        case 3: temp.BackColor = Color.FromArgb(0, 0, 255); break;
                        case 4: temp.BackColor = Color.Yellow; break;
                        case 5: temp.BackColor = Color.Cyan; break;
                        case 6: temp.BackColor = Color.Purple; break;
                        case 7: temp.BackColor = Color.White; break;
                    }
                }
                txtRelay.Text = oNewDS.arayInfo[0].ToString();
                cbRelay.SelectedIndex = oNewDS.arayInfo[1];
                byte[] arayTmp=new byte[4];
                Array.Copy(oNewDS.arayCall, 0, arayTmp, 0, 4);
                txtZone.Text = GlobalClass.AddLeftZero(HDLPF.Byte2String(arayTmp), 4);
                chb1.Checked = (oNewDS.arayCall[4] == 1);
                txtConn.Text = oNewDS.arayCall[5].ToString();
                chb2.Checked = (oNewDS.arayCall[6] == 1);
                ip1.Text = oNewDS.arayCall[7].ToString() + "." + oNewDS.arayCall[8].ToString() + "."
                         + oNewDS.arayCall[9].ToString() + "." + oNewDS.arayCall[10].ToString();
                if (oNewDS.arayInfo[2] <= 90) sbSensitivity.Value = oNewDS.arayInfo[2];
                sbSensitivity_ValueChanged(null, null);
                if (1 <= oNewDS.arayInfo[3] && oNewDS.arayInfo[3] <= 11)
                {
                    cbBell.SelectedIndex = (oNewDS.arayInfo[3] - 1);
                }
                if (cbBell.SelectedIndex < 0) cbBell.SelectedIndex = 0;
                txtLux.Text = (oNewDS.arayInfo[5] * 256 + oNewDS.arayInfo[6]).ToString();
                luLuxValue.Text = (oNewDS.arayInfo[7] * 256 + oNewDS.arayInfo[8]).ToString();
                sbLcdLimit.Value = oNewDS.arayInfo[9];
                chbCardEnable.Checked = (oNewDS.bEnableCard == 1);
            }
            catch
            {
            }
            isRead = false;
        }

        private void showCardInfo()
        {
            if (oNewDS == null) return;
            SetCount();
            btnValid_Click(btnValid, null);

        }

        public void SetCount()
        {
            try
            {
                int ValidCount = 0;
                int LostCount = 0;
                int LimitCount = 0;
                for (int i = 0; i < oNewDS.MyCardInfo.Count; i++)
                {
                    NewDS.CardInfo temp = oNewDS.MyCardInfo[i];
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

        private void showHistory()
        {
            try
            {
                dgvHistory.Rows.Clear();
                if (oNewDS == null) return;
                if (oNewDS.HistoryCount == 0)
                {
                    lbHistoryCount.Text = oNewDS.HistoryCount.ToString();
                    lbTarget.Text = CsConst.mstrINIDefault.IniReadValue("Public", "99798", "");
                    txtFrm.Enabled = false;
                    txtTo.Enabled = false;
                    txtFrm.Text = "0";
                    txtTo.Text = "0";
                }
                else
                {
                    lbHistoryCount.Text = oNewDS.HistoryCount.ToString();
                    lbTarget.Text = CsConst.mstrINIDefault.IniReadValue("Public", "99798", "") + "(1-" + lbHistoryCount.Text + "):";
                    txtFrm.Enabled = true;
                    txtTo.Enabled = true;
                    btnSure.Enabled = true;
                    if (Convert.ToInt32(txtTo.Text) > oNewDS.HistoryCount) txtTo.Text = oNewDS.HistoryCount.ToString();
                    if (Convert.ToInt32(txtFrm.Text) > oNewDS.HistoryCount) txtFrm.Text = txtTo.Text;
                    if (Convert.ToInt32(txtFrm.Text) <= 0) txtFrm.Text = "1";
                    if (oNewDS.MyHistory == null) return;
                    for (int i = 0; i < oNewDS.MyHistory.Count; i++)
                    {
                        NewDS.History temp = oNewDS.MyHistory[i];
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

                            strInfo = CsConst.mstrINIDefault.IniReadValue("Public", "99791", "") + ":" + strTmp+"   " +
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

        private void btnPC_Click(object sender, EventArgs e)
        {
            DateTime d1;
            d1 = DateTime.Now;
            numTime1.Value = Convert.ToDecimal(d1.Hour);
            numTime2.Value = Convert.ToDecimal(d1.Minute);
            numTime3.Value = Convert.ToDecimal(d1.Second);
            DatePicker.Text = d1.ToString();
        }

        private void txtFrm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar) || e.KeyChar == 8)
                e.Handled = false;
            else
                e.Handled = true;
        }

        private void btnSure_Click(object sender, EventArgs e)
        {
            tsbDown_Click(0);
        }

        private void txtFrm_TextChanged(object sender, EventArgs e)
        {
            string str = txtFrm.Text;
            int num = Convert.ToInt32(txtTo.Text);
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

        private void txtTo_TextChanged(object sender, EventArgs e)
        {
            string str = txtTo.Text;
            int num = Convert.ToInt32(txtFrm.Text);
            txtTo.Text = HDLPF.IsNumStringMode(str, num, Convert.ToInt32(lbHistoryCount.Text));
            txtTo.SelectionStart = txtTo.Text.Length;
        }

        private void DatePicker_ValueChanged(object sender, EventArgs e)
        {
            DayOfWeek Week = DatePicker.Value.DayOfWeek;
            label5.Text = CsConst.mstrINIDefault.IniReadValue("Public", "0075" + Week.GetHashCode().ToString(), "");
        }

        private void btnValid_Click(object sender, EventArgs e)
        {
            try
            {
                if (oNewDS == null) return;
                if (oNewDS.MyCardInfo == null) return;
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
                for (int i = 0; i < oNewDS.MyCardInfo.Count; i++)
                {
                    NewDS.CardInfo temp = oNewDS.MyCardInfo[i];
                    if (temp.CardType == Convert.ToByte(Tag))
                    {
                        UserForNewDoor tmp = new UserForNewDoor(oNewDS, i, MyintDeviceType, cbHint.SelectedIndex, myDevName, panel19, SelectCardType, this);
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

        private void FrmNewDS_Shown(object sender, EventArgs e)
        {
            if (CsConst.MyEditMode == 0)
            {
                
            }
            else if (CsConst.MyEditMode == 1) //在线模式
            {
                MyActivePage = 1;
                tsbDown_Click(0);
                gpNetwork.Controls.Clear();
                networkinfo = new NetworkInForm(SubNetID, DevID, MyintDeviceType);
                gpNetwork.Controls.Add(networkinfo);
                networkinfo.Dock = DockStyle.Top;
            }
        }

        private void tmRead_Click(object sender, EventArgs e)
        {
            tsbDown_Click(0);
        }

        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            MyActivePage = tabControl.SelectedIndex + 1;
            if (CsConst.MyEditMode == 1)
            {
                if (oNewDS.MyRead2UpFlags[MyActivePage - 1] == false)
                {
                    tsbDown_Click(0);
                }
                else
                {
                    switch (tabControl.SelectedIndex)
                    {
                        case 0: showBasicInfo(); break;
                        case 1: showCardInfo(); break;
                        case 2: showHistory(); break;
                    }
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                bool isHaveEmptyCard = false;
                for (int i = 0; i < oNewDS.MyCardInfo.Count; i++)
                {
                    if (oNewDS.MyCardInfo[i].CardType == 0)
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
                    for (int i = 0; i < oNewDS.MyCardInfo.Count; i++)
                    {
                        if (oNewDS.MyCardInfo[i].UIDL == arayUID[0])
                        {
                            arayTmp = new byte[arayUID[0]];
                            Array.Copy(arayUID, 1, arayTmp, 0, arayTmp.Length);
                            bool isEqual = true;
                            for (int j = 0; j < arayTmp.Length; j++)
                            {
                                if (arayTmp[j] != oNewDS.MyCardInfo[i].UID[j])
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
                        FrmAddNewCard frmTmp = new FrmAddNewCard(arayUID, oNewDS, MyintDeviceType, myDevName, 1, ID);
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
                        FrmAddNewCard frmTmp = new FrmAddNewCard(arayUID, oNewDS, MyintDeviceType, myDevName, 0, 0);
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

        private void btnSaveBasic_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 4; i++)
            {
                System.Windows.Forms.Panel temp = this.Controls.Find("pnlC" + (i + 1).ToString(), true)[0] as System.Windows.Forms.Panel;
                if (temp.BackColor == Color.Red)
                {
                    oNewDS.arayBasic[i] = 1;
                }
                else if (temp.BackColor == Color.Green)
                {
                    oNewDS.arayBasic[i] = 2;
                }
                else if (temp.BackColor == Color.Blue)
                {
                    oNewDS.arayBasic[i] = 3;
                }
                else if (temp.BackColor == Color.Yellow)
                {
                    oNewDS.arayBasic[i] = 4;
                }
                else if (temp.BackColor == Color.Cyan)
                {
                    oNewDS.arayBasic[i] = 5;
                }
                else if (temp.BackColor == Color.Purple)
                {
                    oNewDS.arayBasic[i] = 6;
                }
                else if (temp.BackColor == Color.White)
                {
                    oNewDS.arayBasic[i] = 7;
                }
            }
            string[] strTmp = ip1.Text.Split('.');
            for (int i = 0; i < 4; i++)
            {
                if (strTmp[i] != null && strTmp[i] != "")
                    oNewDS.arayCall[i + 7] = byte.Parse(strTmp[i].ToString());
            }
            tsbDown_Click(1);
        }

        private void txtCallNum_Leave(object sender, EventArgs e)
        {
            if (txtCallNum.Text == null || txtCallNum.Text == "") txtCallNum.Text = "0000";
        }

        private void txtCallNum_TextChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            string str = txtCallNum.Text;
            str = GlobalClass.AddLeftZero(str, 4);
            oNewDS.strNO = str;
        }

        private void chbSound_CheckedChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            if (chbSound.Checked)
                oNewDS.arayBasic[4] = 1;
            else
                oNewDS.arayBasic[4] = 0;
        }

        private void pnlC1_Click(object sender, EventArgs e)
        {
            int tag = Convert.ToInt32((sender as System.Windows.Forms.Panel).Tag);
            FrmColorForDS frmTmp = new FrmColorForDS(oNewDS.arayBasic[tag], oNewDS, tag);
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
            oNewDS.arayInfo[0] = Convert.ToByte(txtRelay.Text);
        }

        private void cbRelay_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            oNewDS.arayInfo[1] = Convert.ToByte(cbRelay.SelectedIndex);
        }

        private void txtZone_Leave(object sender, EventArgs e)
        {
            if (txtZone.Text == null || txtZone.Text == "") txtZone.Text = "0000";
        }

        private void txtConn_Leave(object sender, EventArgs e)
        {
            string str = txtFrm.Text;
            txtFrm.Text = HDLPF.IsNumStringMode(str, 1, 30);
            txtFrm.SelectionStart = txtFrm.Text.Length;
        }

        private void txtZone_TextChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            string str = txtZone.Text;
            str = GlobalClass.AddLeftZero(str, 4);
            byte[] arayTmp = HDLUDP.StringToByte(str);
            Array.Copy(arayTmp, 0, oNewDS.arayCall, 0, 4);
        }

        private void chb1_CheckedChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            if (chb1.Checked)
                oNewDS.arayCall[4] = 1;
            else
                oNewDS.arayCall[4] = 0;
        }

        private void txtConn_TextChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            if (txtConn.Text.Length > 0)
            {
                string str = txtConn.Text;
                oNewDS.arayCall[5] = Convert.ToByte(txtConn.Text);
            }
        }

        private void chb2_CheckedChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            if (chb2.Checked)
                oNewDS.arayCall[6] = 1;
            else
                oNewDS.arayCall[6] = 0;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            string str = txtPassword.Text;
            str = GlobalClass.AddLeftZero(str, 6);
            oNewDS.strPassword = str;
        }

        private void btnSaveAndClose1_Click(object sender, EventArgs e)
        {
            btnSaveBasic_Click(null, null);
            this.Close();
        }

        private void sbSensitivity_ValueChanged(object sender, EventArgs e)
        {
            lbSensitivity.Text = sbSensitivity.Value.ToString();
            if (isRead) return;
            oNewDS.arayInfo[2] = Convert.ToByte(sbSensitivity.Value);
        }

        private void btnUser_Click(object sender, EventArgs e)
        {
            try
            {
                FileIndex = 0;
                UDPReceive.receiveQueueForAudio = new Queue<byte[]>();
                CsConst.MyBlnFinish = false;
                CsConst.isStartUploadFile = false;
                string MyPath = HDLPF.OpenFileDialog("WAV File|*.wav", "Choose File");
                if (MyPath == null || MyPath == "") return;
                FileStream fs = new FileStream(MyPath, FileMode.Open, FileAccess.Read);//创建文件流
                SourceFile = new byte[fs.Length];
                fs.Read(SourceFile, 0, SourceFile.Length);
                fs.Flush();
                fs.Close();
                int FileLength = SourceFile.Length;
                string strFileNmae = "share/sounds/linphone/rings/011.wav";
                byte[] arayTmp = new byte[strFileNmae.Length + 6 + 2];
                int DataLength = strFileNmae.Length + 6;
                arayTmp[0] = Convert.ToByte(DataLength / 256);
                arayTmp[1] = Convert.ToByte(DataLength % 256);
                arayTmp[2] = 1;
                byte[] arayName = HDLUDP.StringToByte(strFileNmae);
                arayName.CopyTo(arayTmp, 3);
                arayTmp[strFileNmae.Length + 3] = 13;
                arayTmp[strFileNmae.Length + 4] = Convert.ToByte(FileLength / 256 / 256 / 256);
                arayTmp[strFileNmae.Length + 5] = Convert.ToByte(FileLength / 256 / 256 % 256);
                arayTmp[strFileNmae.Length + 6] = Convert.ToByte(FileLength / 256 % 256);
                arayTmp[strFileNmae.Length + 7] = Convert.ToByte(FileLength % 256);
                if (CsConst.mySends.AddBufToSndList(arayTmp, 0x1364, SubNetID, DevID, true, true, true, false) == true)
                {
                    if (CsConst.isStartUploadFile)
                    {
                        MyBackGroup = new BackgroundWorker();
                        MyBackGroup.DoWork += new DoWorkEventHandler(calculationWorker_DoWork);
                        MyBackGroup.ProgressChanged += new ProgressChangedEventHandler(calculationWorker_ProgressChanged);
                        MyBackGroup.WorkerReportsProgress = true;
                        MyBackGroup.RunWorkerCompleted += new RunWorkerCompletedEventHandler(calculationWorker_RunWorkerCompleted);
                        MyBackGroup.RunWorkerAsync();
                        MyBackGroup.WorkerSupportsCancellation = true;
                        frmProcessTmp = new FrmProcess();
                        frmProcessTmp.ShowDialog();
                    }
                    else
                    {
                        MessageBox.Show(CsConst.mstrINIDefault.IniReadValue("public", "99535", ""));
                    }
                }
            }
            catch
            {
            }
        }

        void calculationWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                frmProcessTmp.Close();
            }
            catch
            {
            }
        }


        void calculationWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            try
            {

            }
            catch
            {
            }
        }

        void calculationWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                MyBackGroup.ReportProgress(0, null);
                DateTime t1 = DateTime.Now;
                DateTime t2 = DateTime.Now;
                while (!CsConst.MyBlnFinish)
                {
                    if (UDPReceive.receiveQueueForAudio.Count > 0)
                    {
                        t1 = DateTime.Now;
                        byte[] arayTmp = UDPReceive.receiveQueueForAudio.Dequeue();
                        int FileDataIndex = arayTmp[0] * 256 * 256 * 256 + arayTmp[1] * 256 * 256 + arayTmp[2] * 256 + arayTmp[3];
                        if (arayTmp[0] == 0xFF && arayTmp[1] == 0xFF && arayTmp[2] == 0xFF && arayTmp[3] == 0xFF)
                        {
                            CsConst.MyBlnFinish = true;
                            break;
                        }
                        if ((FileDataIndex + 1024) <= SourceFile.Length)
                        {
                            byte[] arayData = new byte[1024 + 7];
                            arayData[0] = 0x04;
                            arayData[1] = 0x05;
                            arayData[2] = 2;
                            arayData[3] = arayTmp[0];
                            arayData[4] = arayTmp[1];
                            arayData[5] = arayTmp[2];
                            arayData[6] = arayTmp[3];
                            Array.Copy(SourceFile, FileDataIndex, arayData, 7, 1024);
                            CsConst.mySends.AddBufToSndList(arayData, 0x1364, SubNetID, DevID, true, false, false, false);
                            MyBackGroup.ReportProgress(FileDataIndex * 99 / SourceFile.Length);
                        }
                        else
                        {
                            byte[] arayData = new byte[SourceFile.Length - FileDataIndex + 7];
                            arayData[0] = Convert.ToByte((arayData.Length - 2) / 256);
                            arayData[1] = Convert.ToByte((arayData.Length - 2) % 256);
                            arayData[2] = 2;
                            arayData[3] = arayTmp[0];
                            arayData[4] = arayTmp[1];
                            arayData[5] = arayTmp[2];
                            arayData[6] = arayTmp[3];
                            Array.Copy(SourceFile, FileDataIndex, arayData, 7, SourceFile.Length - FileDataIndex);
                            CsConst.mySends.AddBufToSndList(arayData, 0x1364, SubNetID, DevID, true, false, false, false);
                        }

                    }
                    else
                    {
                        t2 = DateTime.Now;
                        if (HDLSysPF.Compare(t2, t1) > 60000)
                        {
                            break;
                        }
                    }
                }
                if (CsConst.MyBlnFinish)
                {
                    MyBackGroup.ReportProgress(100, null);
                    MessageBox.Show(CsConst.mstrINIDefault.IniReadValue("public", "99534", ""));
                }
            }
            catch
            {
            }
            UDPReceive.ClearQueueDataForAudio();
        }


        private void cbBell_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            oNewDS.arayInfo[3] = Convert.ToByte(cbBell.SelectedIndex + 1);
        }

        private void txtLux_Leave(object sender, EventArgs e)
        {
            string str = txtLux.Text;
            txtLux.Text = HDLPF.IsNumStringMode(str, 0, 65535);
            txtLux.SelectionStart = txtLux.Text.Length;
            if (isRead) return;
            int intTmp = Convert.ToInt32(txtLux.Text);
            oNewDS.arayInfo[5] = Convert.ToByte(intTmp / 256);
            oNewDS.arayInfo[6] = Convert.ToByte(intTmp % 256);
        }

        private void btnrefence_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            if (CsConst.mySends.AddBufToSndList(null, 0x3342, SubNetID, DevID, false, true, true, false) == true)
            {
                oNewDS.arayInfo[5] = CsConst.myRevBuf[25];
                oNewDS.arayInfo[6] = CsConst.myRevBuf[26];
                oNewDS.arayInfo[7] = CsConst.myRevBuf[27];
                oNewDS.arayInfo[8] = CsConst.myRevBuf[28];
                oNewDS.arayInfo[9] = CsConst.myRevBuf[29]; //  2016 12 28 new lcd level
                txtLux.Text = (oNewDS.arayInfo[5] * 256 + oNewDS.arayInfo[6]).ToString();
                luLuxValue.Text = (oNewDS.arayInfo[7] * 256 + oNewDS.arayInfo[8]).ToString() + " lux";
                sbLcdLimit.Value = oNewDS.arayInfo[9];
                sbLcdLimit_ValueChanged(sbLcdLimit, null);
                HDLUDP.TimeBetwnNext(1);
            }
            Cursor.Current = Cursors.Default;
        }

        private void btnSaveTemp_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            byte[] arayTmp = new byte[3 ];
            int intTmp = Convert.ToInt32(txtLux.Text);
            arayTmp[0] = Convert.ToByte(intTmp / 256);
            arayTmp[1] = Convert.ToByte(intTmp % 256);
            arayTmp[2] = Convert.ToByte(sbLcdLimit.Value.ToString());
            if (CsConst.mySends.AddBufToSndList(arayTmp, 0x3340, SubNetID, DevID, false, true, true, false) == true)
            {
                oNewDS.arayInfo[5] = arayTmp[0];
                oNewDS.arayInfo[6] = arayTmp[1];
                oNewDS.arayInfo[9] = arayTmp[2];
                HDLUDP.TimeBetwnNext(20);
            }
            Cursor.Current = Cursors.Default;
        }

        private void sbSensitivity_Scroll(object sender, ScrollEventArgs e)
        {

        }

        private void sbLcdLimit_ValueChanged(object sender, EventArgs e)
        {
            lbLcdLimit.Text = sbLcdLimit.Value.ToString() + "%";
            if (isRead) return;
            oNewDS.arayInfo[9] = Convert.ToByte(sbSensitivity.Value);
        }

        private void txtLux_TextChanged(object sender, EventArgs e)
        {

        }

        private void sbLcdLimit_Scroll(object sender, ScrollEventArgs e)
        {

        }

        private void grpColor_Enter(object sender, EventArgs e)
        {

        }

        private void chbCardEnable_CheckedChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            if (chbCardEnable.Checked)
            {
                oNewDS.bEnableCard = 1;
                if (tabControl.TabPages.IndexOf(tabCard) == -1) tabControl.TabPages.Add(tabCard);
            }
            else
            {
                oNewDS.bEnableCard = 0;
                tabControl.TabPages.Remove(tabCard);
            }
        }
    }
}
