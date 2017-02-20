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
    public partial class frmGPRS : Form
    {
        [DllImport("user32.dll")]
        public static extern Boolean FlashWindow(IntPtr hwnd, Boolean bInvert);
        private GPRS myGPRS = null;
        private string myDevName = null;
        private int mintIDIndex = -1;
        private int MyDeviceType = -1;
        private int MyActivePage = 0; //按页面上传下载
        NetworkInForm networkinfo;
        DateAndTime dateandtime;
        private byte SubNetID = 0;
        private byte DevID = 0;
        const byte myMaxGroups = 99;
        private bool isClick = false;
        private ComboBox cbTarget = new ComboBox();
        private ComboBox cbPanleControl = new ComboBox();
        private bool isRead = false;
        public frmGPRS()
        {
            InitializeComponent();
        }

        public frmGPRS(GPRS mygprs, string strName, int intDIndex, int intDeviceType)
        {
            InitializeComponent();
            this.myGPRS = mygprs;
            this.myDevName = strName;
            this.mintIDIndex = intDIndex;
            this.MyDeviceType = intDeviceType;
            string strDevName = strName.Split('\\')[0].ToString();

            HDLSysPF.DisplayDeviceNameModeDescription(strName, MyDeviceType, cboDevice, tbModel, tbDescription);

            SubNetID = Convert.ToByte(strDevName.Split('-')[0]);
            DevID = Convert.ToByte(strDevName.Split('-')[1]);
            this.Text = strName;
            tsl3.Text = strName;
        }


        void InitialFormCtrlsTextOrItems()
        {
            cbTarget.Items.Clear();
            HDLSysPF.AddControlTypeToControl(cbTarget, MyDeviceType);
            clSend5.Items.Clear();
            clSend5.Items.Add(CsConst.WholeTextsList[1775].sDisplayName);
            clSend5.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00042", ""));
            
            if (cbGroup.SelectedIndex == -1) cbGroup.SelectedIndex = 0;
        }

        private void frmGPRS_Load(object sender, EventArgs e)
        {
            InitialFormCtrlsTextOrItems();
        }

        void SetCtrlsVisbleWithDifferentDeviceType()
        {
            toolStrip1.Visible = (CsConst.MyEditMode == 0);

            cbGroup.Items.Clear();
            int num = 20;
            if (MyDeviceType == 4001)
            {
                num = 90;
                lbReceive.Text = lbReceive.Text.Split('-')[0].ToString() + "-90)";
            }
            for (int i = 1; i <= num; i++)
            {
                cbGroup.Items.Add(i.ToString());
            }
        }

        private void loadNetWorkAndConn()
        {
            try
            {
                panel2.Controls.Clear();
                networkinfo = new NetworkInForm(SubNetID, DevID, MyDeviceType);
                panel2.Controls.Add(networkinfo);
                networkinfo.Dock = DockStyle.Fill;

                panel3.Controls.Clear();
                dateandtime = new DateAndTime(SubNetID, DevID, MyDeviceType);
                panel3.Controls.Add(dateandtime);
                dateandtime.Dock = DockStyle.Top;
            }
            catch
            {
            }
        }

        private void DisplayBasicInformation()
        {
            loadNetWorkAndConn();
        }

        private void DisplaySMSControls()
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                dgvReceive.Rows.Clear();
                setAllControlVisible(false);
                byte bytFrm = Convert.ToByte(Convert.ToInt32(txtFrm.Text));
                byte bytTo = Convert.ToByte(txtTo.Text);
                for (byte byt = bytFrm; byt <= bytTo; byt++)
                {
                    for (int i = 0; i < myGPRS.MyControls.Count; i++)
                    {
                        if (byt == (myGPRS.MyControls[i].ID + 1))
                        {
                            object[] obj = new object[] { byt.ToString(), myGPRS.MyControls[i].strRemark,
                    myGPRS.MyControls[i].strSMSContent,myGPRS.MyControls[i].blnIsVerify,
                    myGPRS.MyControls[i].blnReply};
                            dgvReceive.Rows.Add(obj);
                            break;
                        }
                    }
                }
                Cursor.Current = Cursors.Default;
            }
            catch
            {
            }
        }

        private void DisplaySendSMSOut()
        {
            if (myGPRS == null) return;
            if (myGPRS.MySendSMS == null) return;
            isRead = true;
            for (int i = 0; i < myGPRS.MySendSMS.Count; i++)
            {
                if (cbGroup.SelectedIndex == myGPRS.MySendSMS[i].ID)
                {
                    txtGroupRemark.Text = myGPRS.MySendSMS[i].strRemark;
                    dgvSend.Rows.Clear();
                    for (int j = 0; j < myGPRS.MySendSMS[i].MyGuests.Count; j++)
                    {
                        GPRS.PhoneInF temp = myGPRS.MySendSMS[i].MyGuests[j];
                        int intTmp = 0;
                        if (temp.Valid) intTmp = 1;
                        object[] obj = new object[] { dgvSend.RowCount + 1, temp.Remark, temp.PhoneNum, temp.strSMS, clSend5.Items[intTmp].ToString() };
                        dgvSend.Rows.Add(obj);
                    }
                    break;
                }
            }
            isRead = false;
        }

        private void saveGPRSSet()
        {
            myGPRS.blnTo485 = cbto485.Checked;
        }

        private void btnSaveSend_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            myGPRS.SaveSensorInfoToDB();
            Cursor.Current = Cursors.Default;
        }

        private void tvSend_AfterSelect(object sender, TreeViewEventArgs e)
        {
            DisplaySendSMSOut();
        }

        void frmTmp_FormClosing(object sender, FormClosingEventArgs e)
        {
            DisplaySMSControls();
        }


        private void dafaultInfo()
        {
            dgvSend.Rows.Clear();
            for (int i = 0; i < 10; i++)
            {
                object[] obj = new object[] { dgvSend.RowCount + 1, "", "", "" };
                dgvSend.Rows.Add(obj);
            }
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

                    string strName = myDevName.Split('\\')[0].ToString();
                    byte bytSubID = byte.Parse(strName.Split('-')[0]);
                    byte bytDevID = byte.Parse(strName.Split('-')[1]);
                    int num1 = 0;
                    int num2 = 0;
                    if (lbSList.SelectedIndex == 0)
                    {
                        num1 = Convert.ToInt32(txtFrm.Text)-1;
                        num2 = Convert.ToInt32(txtTo.Text);
                    }
                    else if (lbSList.SelectedIndex == 1)
                    {
                        num1 = cbGroup.SelectedIndex;
                        num2 = cbGroup.SelectedIndex + 1;
                    }
                    byte[] ArayRelay = new byte[] { bytSubID, bytDevID, (byte)(MyDeviceType / 256), (byte)(MyDeviceType % 256), (byte)MyActivePage,
                    (byte)(mintIDIndex/ 256), (byte)(mintIDIndex % 256),(byte)num1,(byte)num2};
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
            switch (lbSList.SelectedIndex)
            {
                case 0: DisplayBasicInformation();
                    DisplaySMSControls(); break;
                case 1:
                    DisplaySendSMSOut(); break;
            }
            Cursor.Current = Cursors.Default;
            this.TopMost = true;
            this.WindowState = FormWindowState.Normal;
            this.Activate();
            this.TopMost = false;
        }

        void SetVisableForDownOrUpload(bool blnIsEnable)
        {
            tsbDown.Enabled = blnIsEnable;
            tbUpload.Enabled = blnIsEnable;

            if (blnIsEnable == true)
            {
                tsbHint.Visible = true;
            }
            else
            {
                tsbar.Value = 0;
                tsbHint.Visible = false;
            }

            if (tsbar.Value == 100)
            {
                tsbHint.Text = "Fully Success!";
            }
            tsbar.Visible = !blnIsEnable;
            tsbl4.Visible = !blnIsEnable;
        }


        private void txtSwiIP_TextChanged(object sender, EventArgs e)
        {
            if (myGPRS == null) return;

            string strTmp = ((MaskedTextBox)sender).Text.ToString();
            byte bytTag = byte.Parse(((MaskedTextBox)sender).Tag.ToString());

            if (strTmp == null)
            {
                strTmp = "192.168.010.250";
                ((MaskedTextBox)sender).Text = strTmp;
            }

            switch (bytTag)
            {
                case 0:
                    myGPRS.strIP = strTmp;
                    break;
                case 1:
                    myGPRS.strRouterIP = strTmp;
                    break;
                case 2:
                    myGPRS.strMAC = strTmp;
                    break;
            }
        }


        private void txtCenterSer_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtCountyCode_TextChanged(object sender, EventArgs e)
        {

        }

        private void tsbDefault_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            myGPRS.ReadDefaultInfo();

            DisplayBasicInformation();
            DisplaySMSControls();
            DisplaySendSMSOut();
            Cursor.Current = Cursors.Default;
        }

        private void tsbSave_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            myGPRS.SaveSensorInfoToDB();
            Cursor.Current = Cursors.Default;
        }

        private void tvSend_SelectionChanged(object sender, EventArgs e)
        {
            
        }

        private void dgvControl_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (myGPRS == null) return;
            if (myGPRS.MyControls == null) return;
            if (e.RowIndex == -1) return;
            if (e.ColumnIndex == -1) return;
            if (dgvControl[0, e.RowIndex].Value == null || dgvControl[0, e.RowIndex].Value.ToString() == "") return;

            GPRS.SMSControls temp = myGPRS.MyControls[e.RowIndex];
            if (temp.MyVerify == null) temp.MyVerify = new List<GPRS.PhoneInF>();
            if (temp.MyTargets == null) temp.MyTargets = new List<UVCMD.ControlTargets>();
            string strTmp = null;

            switch (e.ColumnIndex)
            {
                case 1:
                    if (dgvControl[1, e.RowIndex].Value == null)
                    {
                        dgvControl[1, e.RowIndex].Value = "";
                    }
                    strTmp = dgvControl[1, e.RowIndex].Value.ToString();
                    temp.strRemark = strTmp;
                    break;

                case 2:
                    if (dgvControl[2, e.RowIndex].Value == null)
                    {
                        dgvControl[2, e.RowIndex].Value = "";
                    }
                    strTmp = dgvControl[2, e.RowIndex].Value.ToString();
                    temp.strSMSContent = strTmp;
                    break;

                case 3:
                    temp.blnIsVerify = Convert.ToBoolean(dgvControl[3, e.RowIndex].Value.ToString().ToLower() == "true");
                    break;

                case 4:
                    temp.blnReply = Convert.ToBoolean(dgvControl[4, e.RowIndex].Value.ToString().ToLower() == "true");
                    break;
            }
        }

        private void tbSave_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            myGPRS.SaveSensorInfoToDB();
            Cursor.Current = Cursors.Default;
        }

        private void tmRead_Click(object sender, EventArgs e)
        {
            tsbDown_Click(tsbDown, null);
        }

        private void frmGPRS_Shown(object sender, EventArgs e)
        {
            SetCtrlsVisbleWithDifferentDeviceType();
            if (CsConst.MyEditMode == 0)
            {
                DisplayBasicInformation();
                DisplaySMSControls();
                DisplaySendSMSOut();
            }
            else if (CsConst.MyEditMode == 1)
            {
                MyActivePage = 1;
                if (myGPRS.MyRead2UpFlags[0] == false)
                {
                    tsbDown_Click(tsbDown, null);
                    myGPRS.MyRead2UpFlags[0] = true;
                }
                else
                {
                    DisplayBasicInformation();
                    DisplaySMSControls();
                }
            }
        }

        private void lbSList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CsConst.MyEditMode == 1)
            {
                MyActivePage = lbSList.SelectedIndex + 1;
                if (myGPRS.MyRead2UpFlags[MyActivePage - 1] == false)
                {
                    if (lbSList.SelectedIndex != 2)
                        tsbDown_Click(tsbDown, null);
                    else
                    {
                        btnrefence_Click(null, null);
                        button1_Click(null, null);
                    }
                }
                else
                {
                    switch (lbSList.SelectedIndex)
                    {
                        case 0: DisplayBasicInformation();
                            DisplaySMSControls(); break;
                        case 1:
                            DisplaySendSMSOut(); break;
                    }
                }
            }
        }


        private void btnSure_Click(object sender, EventArgs e)
        {
            tsbDown_Click(tsbDown, null);
            if (myGPRS == null) return;
            if (myGPRS.MyControls == null) return;
            isClick = true;
            Cursor.Current = Cursors.WaitCursor;
            dgvReceive.Rows.Clear();
            setAllControlVisible(false);
            for (byte byt = 0; byt < myGPRS.MyControls.Count; byt++)
            {
                object[] obj = new object[] {  (myGPRS.MyControls[byt].ID+1).ToString(), myGPRS.MyControls[byt].strRemark, 
                    myGPRS.MyControls[byt].strSMSContent,myGPRS.MyControls[byt].blnIsVerify,
                    myGPRS.MyControls[byt].blnReply};
                dgvReceive.Rows.Add(obj);
            }
            Cursor.Current = Cursors.Default;
            isClick = false;
        }

        private void setAllControlVisible(bool TF)
        {

        }

        private void txtFrm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar) || e.KeyChar == 8)
                e.Handled = false;
            else
                e.Handled = true;
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
            txtTo.Text = HDLPF.IsNumStringMode(str, num, 99);
            txtTo.SelectionStart = txtTo.Text.Length;
        }

        private void dgvReceive_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                Cursor.Current = Cursors.WaitCursor;
                GetPhoneInfo(e.RowIndex);
                getTargetInfo(e.RowIndex);
                Cursor.Current = Cursors.Default;
            }
        }

        private void GetPhoneInfo(int rowindex)
        {
            Cursor.Current = Cursors.WaitCursor;
            dgvPhone.Rows.Clear();
            int index = Convert.ToInt32(dgvReceive[0, rowindex].Value.ToString());
            for (int i = 0; i < myGPRS.MyControls.Count; i++)
            {
                if (myGPRS.MyControls[i].ID == (index - 1))
                {
                    for (int j = 0; j < myGPRS.MyControls[i].MyVerify.Count; j++)
                    {
                        object[] obj = new object[]{dgvPhone.RowCount+1,myGPRS.MyControls[i].MyVerify[j].Remark,
                    myGPRS.MyControls[i].MyVerify[j].PhoneNum};
                        dgvPhone.Rows.Add(obj);
                    }
                    break;
                }
            }
            Cursor.Current = Cursors.Default;
        }

        private void getTargetInfo(int rowindex)
        {
            Cursor.Current = Cursors.WaitCursor;
            dgvTarget.Rows.Clear();
            int index = Convert.ToInt32(dgvReceive[0, rowindex].Value.ToString());
            for (int i = 0; i < myGPRS.MyControls.Count; i++)
            {
                if (myGPRS.MyControls[i].ID == (index - 1))
                {
                    for (int j = 0; j < myGPRS.MyControls[i].MyTargets.Count; j++)
                    {
                        string strType = CsConst.WholeTextsList[1775].sDisplayName;
                        strType = GetIDAccordinglyRightControlTypeSMS(myGPRS.MyControls[i].MyTargets[j].Type);
                        string strParam1 = "0", strParam2 = "0", strParam3 = "0", strParam4 = "0";
                        strParam1 = myGPRS.MyControls[i].MyTargets[j].Param1.ToString();
                        strParam2 = myGPRS.MyControls[i].MyTargets[j].Param2.ToString();
                        strParam3 = myGPRS.MyControls[i].MyTargets[j].Param3.ToString();
                        strParam4 = myGPRS.MyControls[i].MyTargets[j].Param4.ToString();
                        SetParams(ref strType, ref strParam1, ref strParam2, ref strParam3, strParam4);
                        string strValid = CsConst.WholeTextsList[1775].sDisplayName;
                        strType = cbTarget.Items[getCbSelIndex(myGPRS.MyControls[i].MyTargets[j].Type)].ToString();
                        if (myGPRS.MyControls[i].MyTargets[j].IsValid) strValid = CsConst.mstrINIDefault.IniReadValue("Public", "00042", "");
                        object[] obj = new object[]{dgvTarget.RowCount+1,
                                            myGPRS.MyControls[i].MyTargets[j].SubnetID.ToString(),
                                            myGPRS.MyControls[i].MyTargets[j].DeviceID.ToString(),
                                            strType,strParam1,strParam2,strParam3,strValid};
                        dgvTarget.Rows.Add(obj);
                    }
                    break;
                }
            }
            Cursor.Current = Cursors.Default;
        }

        private byte getCbSelIndex(byte bytType)
        {
            byte bytResult = 0;
            switch (bytType)
            {
                case 0: bytResult = 1; break;//场景
                case 1: bytResult = 2; break;//序列
                case 2: bytResult = 3; break;//通用开关
                case 9: bytResult = 0; break;//无效
                case 3: bytResult = 4; break;//单路调节
                case 10: bytResult = 5; break;//窗帘开关
                case 7: bytResult = 6; break;//面板控制
                case 4: bytResult = 7; break;//广播场景
                case 5: bytResult = 8; break;//广播回路
                case 8: bytResult = 9; break;//消防模块
                case 11: bytResult = 10; break;//音乐播放
            }
            return bytResult;
        }

        private string GetIDAccordinglyRightControlTypeSMS(byte bytType)
        {
            string bytResult = "0";
            switch (bytType)
            {
                case 0: bytResult = "85"; break;//场景
                case 1: bytResult = "86"; break;//序列
                case 2: bytResult = "88"; break;//通用开关
                case 9: bytResult = "0"; break;//无效
                case 3: bytResult = "89"; break;//单路调节
                case 4: bytResult = "100"; break;//广播场景
                case 5: bytResult = "101"; break;//广播回路
                case 10: bytResult = "92"; break;//窗帘开关
                case 7: bytResult = "95"; break;//面板控制
                case 8: bytResult = "102"; break;//消防模块
                case 11: bytResult = "103"; break;//音乐播放
            }
            return bytResult;
        }

        private void SetParams(ref string strType, ref string str1, ref string str2, ref string str3, string str4)
        {
            int ID = Convert.ToInt32(strType);
            if (ID == 0)//无效
            {
                #region
                str1 = "N/A";
                str2 = "N/A";
                str3 = "N/A";
                #endregion
            }
            else if (ID == 85)//场景
            {
                #region
                str1 = str1 + "(" + CsConst.WholeTextsList[2510].sDisplayName + ")";
                str2 = str2 + "(" + CsConst.WholeTextsList[2511].sDisplayName + ")";
                str3 = "N/A";
                #endregion
            }
            else if (ID == 86)//序列
            {
                #region
                str1 = str1 + "(" + CsConst.WholeTextsList[2510].sDisplayName + ")";
                str2 = str2 + "(" + CsConst.WholeTextsList[2512].sDisplayName+ ")";
                str3 = "N/A";
                #endregion
            }
            else if (ID == 88)//通用开关
            {
                #region
                str1 = str1 + "(" + CsConst.WholeTextsList[2513].sDisplayName + ")";
                if (str2 == "0") str2 = CsConst.Status[0] + "(" + CsConst.WholeTextsList[2529].sDisplayName + ")";
                else if (str2 == "255") str2 = CsConst.Status[1] + "(" + CsConst.WholeTextsList[2529].sDisplayName + ")";
                str3 = "N/A";
                #endregion
            }
            else if (ID == 89)//单路调节
            {
                #region
                str1 = str1 + "(" + CsConst.WholeTextsList[934].sDisplayName + ")";
                str2 = str2 + "(" + CsConst.WholeTextsList[2524].sDisplayName + ")";
                int intTmp = Convert.ToInt32(str3) * 256 + Convert.ToInt32(str4);
                str3 = HDLPF.GetStringFromTime(intTmp, ":") + "(" + CsConst.WholeTextsList[2525].sDisplayName + ")";
                #endregion
            }
            else if (ID == 100)//广播场景
            {
                #region
                str1 = CsConst.WholeTextsList[2566].sDisplayName;
                str2 = str2 + "(" + CsConst.WholeTextsList[2511].sDisplayName + ")";
                str3 = "N/A";
                #endregion
            }
            else if (ID == 101)//广播回路
            {
                #region
                str1 = CsConst.WholeTextsList[2567].sDisplayName;
                str2 = str2 + "(" + CsConst.WholeTextsList[2524].sDisplayName + ")";
                int intTmp = Convert.ToInt32(str3) * 256 + Convert.ToInt32(str4);
                str3 = HDLPF.GetStringFromTime(intTmp, ":") + "(" + CsConst.WholeTextsList[2525].sDisplayName + ")";
                #endregion
            }
            else if (ID == 92)//窗帘开关
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
                str1 = str1 + "(" + CsConst.mstrINIDefault.IniReadValue("public", "99844", "") + ")";
                str3 = "N/A";
                #endregion
            }
            else if (ID == 94)//GPRS
            {
                #region
                if (str1 == "1") str1 = CsConst.mstrINIDefault.IniReadValue("public", "99862", "");
                else if (str1 == "2") str1 = CsConst.mstrINIDefault.IniReadValue("public", "99863", "");
                else str1 = CsConst.WholeTextsList[1775].sDisplayName;
                str2 = str2 + "(" + CsConst.mstrINIDefault.IniReadValue("public", "99864", "") + ")";
                str3 = "N/A";
                #endregion
            }
            else if (ID == 95)//面板控制
            {
                #region
                if (str1 == "0")
                {
                    str2 = "N/A";
                    str3 = "N/A";
                }
                else if (str1 == "1" || str1 == "11" ||
                         str1 == "2" || str1 == "12" ||
                         str1 == "24" || str1 == "3" ||
                         str1 == "20")
                {
                    if (str2 == "0") str2 = CsConst.Status[0] + "(" + CsConst.WholeTextsList[2529].sDisplayName + ")";
                    else if (str2 == "1") str2 = CsConst.Status[1] + "(" + CsConst.WholeTextsList[2529].sDisplayName + ")";
                    str3 = "N/A";
                }
                else if (str1 == "13" || str1 == "14")
                {
                    str2 = str2 + "(" + CsConst.WholeTextsList[2524].sDisplayName + ")";
                    str3 = "N/A";
                }
                else if (str1 == "16" || str1 == "15" ||
                         str1 == "17" || str1 == "18")
                {
                    int intTmp = Convert.ToInt32(str2);
                    if (str1 == "16")
                    {
                        if (1 <= intTmp && intTmp <= 7) str2 = CsConst.mstrINIDefault.IniReadValue("public", "00048", "") + intTmp.ToString();
                        else if (intTmp == 255) str2 = CsConst.mstrINIDefault.IniReadValue("public", "00049", "");
                        else str2 = CsConst.WholeTextsList[1775].sDisplayName;
                    }
                    else if (str1 == "15")
                    {
                        if (intTmp == 255) str2 = CsConst.mstrINIDefault.IniReadValue("public", "00049", "");
                        else if (1 <= intTmp && intTmp <= 56) str2 = CsConst.mstrINIDefault.IniReadValue("public", "99867", "") + intTmp.ToString();
                        else str2 = CsConst.WholeTextsList[1775].sDisplayName;
                    }
                    else if (str1 == "17")
                    {
                        if (intTmp == 255) str2 = CsConst.mstrINIDefault.IniReadValue("public", "00049", "");
                        else if (1 <= intTmp && intTmp <= 32) str2 = CsConst.mstrINIDefault.IniReadValue("public", "99867", "") + intTmp.ToString();
                        else if (intTmp == 101) str2 = CsConst.mstrINIDefault.IniReadValue("public", "99867", "");
                        else if (intTmp == 102) str2 = CsConst.mstrINIDefault.IniReadValue("public", "99868", "");
                        else if (intTmp == 103) str2 = CsConst.mstrINIDefault.IniReadValue("public", "99869", "");
                        else if (intTmp == 104) str2 = CsConst.mstrINIDefault.IniReadValue("public", "99870", "");
                        else str2 = CsConst.WholeTextsList[1775].sDisplayName;
                    }
                    else if (str1 == "18")
                    {
                        if (1 <= intTmp && intTmp <= 32) str2 = CsConst.mstrINIDefault.IniReadValue("public", "99867", "") + intTmp.ToString();
                        else str2 = CsConst.WholeTextsList[1775].sDisplayName;
                    }
                    if (str3 == "1") str3 = CsConst.mstrINIDefault.IniReadValue("public", "00042", "");
                    else str3 = CsConst.WholeTextsList[1775].sDisplayName;
                }
                else if (str1 == "6")
                {
                    int intTmp = Convert.ToInt32(str2);
                    if (intTmp <= 4) str2 = CsConst.mstrINIDefault.IniReadValue("public", "0005" + intTmp.ToString(), "");
                    str3 = "N/A";
                }
                else if (str1 == "5")
                {
                    int intTmp = Convert.ToInt32(str2);
                    if (intTmp <= 3) str2 = CsConst.mstrINIDefault.IniReadValue("public", "0006" + intTmp.ToString(), "");
                    str3 = "N/A";
                }
                else if (str1 =="9" || str1 == "10" ||
                    str1 == "23" || str1 == "24")
                {
                    str3 = "N/A";
                }
                else if (str1 == "4" || str1 == "7" ||
                         str1 == "8" || str1 == "19")
                {
                    int intTmp = Convert.ToInt32(str2);
                    if (0 <= intTmp && intTmp <= 30) str2 = str2 + "C";
                    else if (32 <= intTmp && intTmp <= 86) str2 = str2 + "F";
                    str3 = "N/A";
                }
                else if (str1 == "21")
                {
                    int intTmp = Convert.ToInt32(str2);
                    if (intTmp <= 5) str2 = CsConst.mstrINIDefault.IniReadValue("public", "0007" + (intTmp - 1).ToString(), "");
                    if (str3 == "255") str3 = CsConst.mstrINIDefault.IniReadValue("public", "00049", "");
                    else str3 = str3 + "(" + CsConst.WholeTextsList[934].sDisplayName + ")";
                }
                str1 = HDLSysPF.InquirePanelControTypeStringFromDB(Convert.ToInt32(str1));
                #endregion
            }
            else if (ID == 102)//消防模块
            {
                #region
                str1 = str1 + "(" + CsConst.WholeTextsList[2510].sDisplayName + ")";
                int intTmp = Convert.ToInt32(str2);
                if (1 <= intTmp && intTmp <= 10) str2 = CsConst.mstrINIDefault.IniReadValue("public", "0008" + (intTmp - 1).ToString(), "");
                str3 = "N/A";
                #endregion
            }
            else if (ID == 103)//音乐控制
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
                else if (str1 == CsConst.mstrINIDefault.IniReadValue("public", "00097", "") || 
                         str1 == CsConst.mstrINIDefault.IniReadValue("public", "00098", ""))
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
                #endregion
            }
        }

        private void addcontrols(int col, int row, Control con)
        {
            con.Show();
            con.Visible = true;
            Rectangle rect = dgvReceive.GetCellDisplayRectangle(col, row, true);
            con.Size = rect.Size;
            con.Top = rect.Top;
            con.Left = rect.Left;
        }

        private void dgvReceive_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if ((e.RowIndex == -1) || (e.ColumnIndex == -1)) return;
                if (dgvReceive.SelectedRows.Count == 0) return;
                if (isClick) return;
                if (dgvReceive[e.ColumnIndex, e.RowIndex].Value == null) dgvReceive[e.ColumnIndex, e.RowIndex].Value = "";
                for (int i = 0; i < dgvReceive.SelectedRows.Count; i++)
                {

                    dgvReceive.SelectedRows[i].Cells[e.ColumnIndex].Value = dgvReceive[e.ColumnIndex, e.RowIndex].Value.ToString();
                    string strTmp = "";
                    switch (e.ColumnIndex)
                    {
                        case 1:
                            strTmp = dgvReceive[1, dgvReceive.SelectedRows[i].Index].Value.ToString();
                            for (int j = 0; j < myGPRS.MyControls.Count; j++)
                            {
                                if (myGPRS.MyControls[j].ID == (Convert.ToInt32(dgvReceive.SelectedRows[i].Cells[0].Value.ToString()) - 1))
                                {
                                    myGPRS.MyControls[j].strRemark = strTmp;
                                    break;
                                }
                            }
                            break;
                        case 2:
                            strTmp = dgvReceive[2, dgvReceive.SelectedRows[i].Index].Value.ToString();
                            for (int j = 0; j < myGPRS.MyControls.Count; j++)
                            {
                                if (myGPRS.MyControls[j].ID == (Convert.ToInt32(dgvReceive.SelectedRows[i].Cells[0].Value.ToString()) - 1))
                                {
                                    myGPRS.MyControls[j].strSMSContent = strTmp;
                                    break;
                                }
                            }
                            break;
                        case 3:
                            if (dgvReceive[3, dgvReceive.SelectedRows[i].Index].Value.ToString().ToLower() == "true")
                            {
                                for (int j = 0; j < myGPRS.MyControls.Count; j++)
                                {
                                    if (myGPRS.MyControls[j].ID == (Convert.ToInt32(dgvReceive.SelectedRows[i].Cells[0].Value.ToString()) - 1))
                                    {
                                        myGPRS.MyControls[j].blnIsVerify = true;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                for (int j = 0; j < myGPRS.MyControls.Count; j++)
                                {
                                    if (myGPRS.MyControls[j].ID == (Convert.ToInt32(dgvReceive.SelectedRows[i].Cells[0].Value.ToString()) - 1))
                                    {
                                        myGPRS.MyControls[j].blnIsVerify = false;
                                        break;
                                    }
                                }
                            }
                            break;
                        case 4:
                            if (dgvReceive[4, dgvReceive.SelectedRows[i].Index].Value.ToString().ToLower() == "true")
                                for (int j = 0; j < myGPRS.MyControls.Count; j++)
                                {
                                    if (myGPRS.MyControls[j].ID == (Convert.ToInt32(dgvReceive.SelectedRows[i].Cells[0].Value.ToString()) - 1))
                                    {
                                        myGPRS.MyControls[j].blnReply = true;
                                        break;
                                    }
                                }
                            else
                                for (int j = 0; j < myGPRS.MyControls.Count; j++)
                                {
                                    if (myGPRS.MyControls[j].ID == (Convert.ToInt32(dgvReceive.SelectedRows[i].Cells[0].Value.ToString()) - 1))
                                    {
                                        myGPRS.MyControls[j].blnReply = false;
                                        break;
                                    }
                                }
                            break;
                    }
                }

                if (e.ColumnIndex == 1)
                {
                    string strTmp = dgvReceive[1, e.RowIndex].Value.ToString();
                    for (int j = 0; j < myGPRS.MyControls.Count; j++)
                    {
                        if (myGPRS.MyControls[j].ID == (Convert.ToInt32(dgvReceive[0, e.RowIndex].Value.ToString()) - 1))
                        {
                            myGPRS.MyControls[j].strRemark = strTmp;
                            break;
                        }
                    }
                }
                else if (e.ColumnIndex == 2)
                {
                    string strTmp = dgvReceive[2, e.RowIndex].Value.ToString();
                    for (int j = 0; j < myGPRS.MyControls.Count; j++)
                    {
                        if (myGPRS.MyControls[j].ID == (Convert.ToInt32(dgvReceive[0, e.RowIndex].Value.ToString()) - 1))
                        {
                            myGPRS.MyControls[j].strSMSContent = strTmp;
                            break;
                        }
                    }
                }
                else if (e.ColumnIndex == 3)
                {
                    if (dgvReceive[3, e.RowIndex].Value.ToString().ToLower() == "true")
                    {
                        for (int j = 0; j < myGPRS.MyControls.Count; j++)
                        {
                            if (myGPRS.MyControls[j].ID == (Convert.ToInt32(dgvReceive[0, e.RowIndex].Value.ToString()) - 1))
                            {
                                myGPRS.MyControls[j].blnIsVerify = true;
                                break;
                            }
                        }
                    }
                    else
                    {
                        for (int j = 0; j < myGPRS.MyControls.Count; j++)
                        {
                            if (myGPRS.MyControls[j].ID == (Convert.ToInt32(dgvReceive[0, e.RowIndex].Value.ToString()) - 1))
                            {
                                myGPRS.MyControls[j].blnIsVerify = false;
                                break;
                            }
                        }
                    }
                }
                else if (e.ColumnIndex == 4)
                {
                    if (dgvReceive[4, e.RowIndex].Value.ToString().ToLower() == "true")
                    {
                        for (int j = 0; j < myGPRS.MyControls.Count; j++)
                        {
                            if (myGPRS.MyControls[j].ID == (Convert.ToInt32(dgvReceive[0, e.RowIndex].Value.ToString()) - 1))
                            {
                                myGPRS.MyControls[j].blnReply = true;
                                break;
                            }
                        }
                    }
                    else
                    {
                        for (int j = 0; j < myGPRS.MyControls.Count; j++)
                        {
                            if (myGPRS.MyControls[j].ID == (Convert.ToInt32(dgvReceive[0, e.RowIndex].Value.ToString()) - 1))
                            {
                                myGPRS.MyControls[j].blnReply = false;
                            }
                        }
                    }
                }
            }
            catch
            {

            }
        }
        
        private void dgvReceive_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            dgvReceive.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void dgvPhone_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if ((e.RowIndex == -1) || (e.ColumnIndex == -1)) return;
            if (dgvPhone.SelectedRows.Count == 0) return;
            if (isClick) return;
            if (dgvPhone[e.ColumnIndex, e.RowIndex].Value == null) dgvPhone[e.ColumnIndex, e.RowIndex].Value = "";
            int index = Convert.ToInt32(dgvReceive[0, dgvReceive.CurrentRow.Index].Value.ToString());
            for (int i = 0; i < dgvPhone.SelectedRows.Count; i++)
            {
                dgvPhone.SelectedRows[i].Cells[e.ColumnIndex].Value = dgvPhone[e.ColumnIndex, e.RowIndex].Value.ToString();
                string strTmp = "";
                switch (e.ColumnIndex)
                {
                    case 1:
                        strTmp = dgvPhone[1, dgvPhone.SelectedRows[i].Index].Value.ToString();
                        for (int j = 0; j < myGPRS.MyControls.Count; j++)
                        {
                            if (myGPRS.MyControls[j].ID == (index - 1))
                            {
                                myGPRS.MyControls[j].MyVerify[Convert.ToInt32(dgvPhone.SelectedRows[i].Cells[0].Value.ToString()) - 1].Remark = strTmp;
                                break;
                            }
                        }
                        break;
                    case 2:
                        strTmp = dgvPhone[2, dgvPhone.SelectedRows[i].Index].Value.ToString();
                        for (int j = 0; j < myGPRS.MyControls.Count; j++)
                        {
                            if (myGPRS.MyControls[j].ID == (index - 1))
                            {
                                myGPRS.MyControls[j].MyVerify[Convert.ToInt32(dgvPhone.SelectedRows[i].Cells[0].Value.ToString()) - 1].PhoneNum = strTmp;
                                break;
                            }
                        }
                        break;
                }
            }
            if (e.ColumnIndex == 1)
            {
                string strTmp = dgvPhone[1, e.RowIndex].Value.ToString();
                for (int j = 0; j < myGPRS.MyControls.Count; j++)
                {
                    if (myGPRS.MyControls[j].ID == (index - 1))
                    {
                        myGPRS.MyControls[j].MyVerify[Convert.ToInt32(dgvPhone[0, e.RowIndex].Value.ToString()) - 1].Remark = strTmp;
                        break;
                    }
                }
            }
            else if (e.ColumnIndex == 2)
            {
                string strTmp = dgvPhone[2, e.RowIndex].Value.ToString();
                for (int j = 0; j < myGPRS.MyControls.Count; j++)
                {
                    if (myGPRS.MyControls[j].ID == (index - 1))
                    {
                        myGPRS.MyControls[index - 1].MyVerify[Convert.ToInt32(dgvPhone[0, e.RowIndex].Value.ToString()) - 1].Remark = strTmp;
                        break;
                    }
                }
            }
        }

        private void dgvPhone_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            dgvPhone.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void dgvTarget_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            Byte[] PageID = new Byte[4];
            if (dgvTarget.SelectedRows != null && dgvTarget.SelectedRows.Count > 0)
            {
                PageID[0] = (Byte)dgvTarget.SelectedRows[0].Index;
            }
            PageID[1] = Convert.ToByte(dgvReceive.CurrentRow.Index);
            frmCmdSetup CmdSetup = new frmCmdSetup(myGPRS,myDevName , MyDeviceType, PageID);
            CmdSetup.ShowDialog();
        }

        void frmtemp_FormClosing(object sender, FormClosingEventArgs e)
        {
            dgvReceive.Rows.Clear();
            dgvPhone.Rows.Clear();
            dgvTarget.Rows.Clear();
            DisplaySMSControls();
        }

        private void cbGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            /*if (myGPRS.MySendSMS[cbGroup.SelectedIndex].MyGuests == null) return;
            if (myGPRS.MySendSMS[cbGroup.SelectedIndex].MyGuests.Count == 0) return;
            txtGroupRemark.Text = myGPRS.MySendSMS[cbGroup.SelectedIndex].strRemark;
            dgvSend.Rows.Clear();
            for (int i = 0; i < myGPRS.MySendSMS[cbGroup.SelectedIndex].MyGuests.Count; i++)
            {
                GPRS.PhoneInF temp = myGPRS.MySendSMS[cbGroup.SelectedIndex].MyGuests[i];
                int intTmp = 0;
                if (temp.Valid) intTmp = 1;
                object[] obj = new object[] { dgvSend.RowCount + 1, temp.Remark, temp.PhoneNum, temp.strSMS, clSend5.Items[intTmp] };
                dgvSend.Rows.Add(obj);
            }*/
            tmRead_Click(null, null);
        }

        private void dgvSend_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            dgvSend.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void dgvSend_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if ((e.RowIndex == -1) || (e.ColumnIndex == -1)) return;
                if (dgvSend.SelectedRows.Count == 0) return;
                if (isClick) return;
                if (dgvSend[e.ColumnIndex, e.RowIndex].Value == null) dgvSend[e.ColumnIndex, e.RowIndex].Value = "";
                for (int i = 0; i < dgvSend.SelectedRows.Count; i++)
                {
                    dgvSend.SelectedRows[i].Cells[e.ColumnIndex].Value = dgvSend[e.ColumnIndex, e.RowIndex].Value.ToString();
                    string strTmp = "";
                    switch (e.ColumnIndex)
                    {
                        case 1:
                            strTmp = dgvSend[1, dgvSend.SelectedRows[i].Index].Value.ToString();
                            for (int j = 0; j < myGPRS.MySendSMS.Count; j++)
                            {
                                if (myGPRS.MySendSMS[j].ID == cbGroup.SelectedIndex)
                                {
                                    myGPRS.MySendSMS[j].MyGuests[Convert.ToInt32(dgvSend.SelectedRows[i].Cells[0].Value.ToString()) - 1].Remark = strTmp;
                                    break;
                                }
                            }
                            break;
                        case 2:
                            strTmp = dgvSend[2, dgvSend.SelectedRows[i].Index].Value.ToString();
                            for (int j = 0; j < myGPRS.MySendSMS.Count; j++)
                            {
                                if (myGPRS.MySendSMS[j].ID == cbGroup.SelectedIndex)
                                {
                                    myGPRS.MySendSMS[j].MyGuests[Convert.ToInt32(dgvSend.SelectedRows[i].Cells[0].Value.ToString()) - 1].PhoneNum = strTmp;
                                    break;
                                }
                            }
                            break;
                        case 3:
                            strTmp = dgvSend[3, dgvSend.SelectedRows[i].Index].Value.ToString();
                            for (int j = 0; j < myGPRS.MySendSMS.Count; j++)
                            {
                                if (myGPRS.MySendSMS[j].ID == cbGroup.SelectedIndex)
                                {
                                    myGPRS.MySendSMS[j].MyGuests[Convert.ToInt32(dgvSend.SelectedRows[i].Cells[0].Value.ToString()) - 1].strSMS = strTmp;
                                    break;
                                }
                            }
                            break;
                        case 4:
                            if (clSend5.Items.IndexOf(dgvSend[4, dgvSend.SelectedRows[i].Index].Value.ToString()) == 1)
                                for (int j = 0; j < myGPRS.MySendSMS.Count; j++)
                                {
                                    if (myGPRS.MySendSMS[j].ID == cbGroup.SelectedIndex)
                                    {
                                        myGPRS.MySendSMS[j].MyGuests[Convert.ToInt32(dgvSend.SelectedRows[i].Cells[0].Value.ToString()) - 1].Valid = true;
                                        break;
                                    }
                                }
                            else
                                for (int j = 0; j < myGPRS.MySendSMS.Count; j++)
                                {
                                    if (myGPRS.MySendSMS[j].ID == cbGroup.SelectedIndex)
                                    {
                                        myGPRS.MySendSMS[j].MyGuests[Convert.ToInt32(dgvSend.SelectedRows[i].Cells[0].Value.ToString()) - 1].Valid = false;
                                        break;
                                    }
                                }
                            break;
                    }
                }

                if (e.ColumnIndex == 1)
                {
                    string strTmp = dgvSend[1, e.RowIndex].Value.ToString();
                    for (int j = 0; j < myGPRS.MySendSMS.Count; j++)
                    {
                        if (myGPRS.MySendSMS[j].ID == cbGroup.SelectedIndex)
                        {
                            myGPRS.MySendSMS[j].MyGuests[e.RowIndex].Remark = strTmp;
                            break;
                        }
                    }
                }
                else if (e.ColumnIndex == 2)
                {
                    string strTmp = dgvSend[2, e.RowIndex].Value.ToString();
                    for (int j = 0; j < myGPRS.MySendSMS.Count; j++)
                    {
                        if (myGPRS.MySendSMS[j].ID == cbGroup.SelectedIndex)
                        {
                            myGPRS.MySendSMS[j].MyGuests[e.RowIndex].PhoneNum = strTmp;
                            break;
                        }
                    }
                }
                else if (e.ColumnIndex == 3)
                {
                    string strTmp = dgvSend[3, e.RowIndex].Value.ToString();
                    for (int j = 0; j < myGPRS.MySendSMS.Count; j++)
                    {
                        if (myGPRS.MySendSMS[j].ID == cbGroup.SelectedIndex)
                        {
                            myGPRS.MySendSMS[j].MyGuests[e.RowIndex].strSMS = strTmp;
                            break;
                        }
                    }
                }
                else if (e.ColumnIndex == 4)
                {
                    if (clSend5.Items.IndexOf(dgvSend[4, e.RowIndex].Value.ToString()) == 1)
                        for (int j = 0; j < myGPRS.MySendSMS.Count; j++)
                        {
                            if (myGPRS.MySendSMS[j].ID == cbGroup.SelectedIndex)
                            {
                                myGPRS.MySendSMS[j].MyGuests[e.RowIndex].Valid = true;
                                break;
                            }
                        }
                    else
                        for (int j = 0; j < myGPRS.MySendSMS.Count; j++)
                        {
                            if (myGPRS.MySendSMS[j].ID == cbGroup.SelectedIndex)
                            {
                                myGPRS.MySendSMS[j].MyGuests[e.RowIndex].Valid = false;
                                break;
                            }
                        }
                }
            }
            catch
            {
            }
        }

        private void txtGroupRemark_TextChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            if (cbGroup.SelectedIndex < 0) return;
            for (int i = 0; i < myGPRS.MySendSMS.Count; i++)
            {
                if (cbGroup.SelectedIndex == myGPRS.MySendSMS[i].ID)
                {
                    myGPRS.MySendSMS[i].strRemark = txtGroupRemark.Text.Trim();
                    break;
                }
            }
            //cbGroup.Items[cbGroup.SelectedIndex] = (cbGroup.SelectedIndex + 1).ToString() + "-" + txtGroupRemark.Text;
        }

        private void chb3_CheckedChanged(object sender, EventArgs e)
        {
            txtAppend.Enabled = chb3.Checked;
        }

        private void btnrefence_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                byte[] arayTmp = new byte[0];
                if (CsConst.mySends.AddBufToSndList(arayTmp, 0xC102, SubNetID, DevID, false, true, true,false) == true)
                {
                    if (CsConst.myRevBuf[25] == 0)
                        picSignal.Image = imgs1.Images[0];
                    else if (1 <= CsConst.myRevBuf[25] && CsConst.myRevBuf[25] <= 3) picSignal.Image = imgs1.Images[1];
                    else if (4 <= CsConst.myRevBuf[25] && CsConst.myRevBuf[25] <= 6) picSignal.Image = imgs1.Images[2];
                    else if (7 <= CsConst.myRevBuf[25] && CsConst.myRevBuf[25] <= 9) picSignal.Image = imgs1.Images[3];
                    else if (10 <= CsConst.myRevBuf[25] && CsConst.myRevBuf[25] <= 14) picSignal.Image = imgs1.Images[4];
                    else if (15 <= CsConst.myRevBuf[25] && CsConst.myRevBuf[25] <= 255) picSignal.Image = imgs1.Images[5];
                }
                else
                {
                    Cursor.Current = Cursors.Default;
                    return;
                }
                
                byte bytAll = 0;
                arayTmp = new byte[5];
                arayTmp[0] = 0;
                arayTmp[1] = 3;
                if (CsConst.mySends.AddBufToSndList(arayTmp, 0xC100, SubNetID, DevID, true, true, true, false) == true)
                {
                    bytAll = CsConst.myRevBuf[28];
                }
                else
                {
                    Cursor.Current = Cursors.Default;
                    return;
                }

                if (bytAll > 0)
                {
                    dgvMessage.Rows.Clear();
                    for (int i = 1; i <= bytAll; i++)
                    {
                        arayTmp = new byte[5];
                        arayTmp[0] = 0;
                        arayTmp[1] = 3;
                        arayTmp[2] = Convert.ToByte(i);
                        if (CsConst.mySends.AddBufToSndList(arayTmp, 0xC100, SubNetID, DevID, true, true, true, false) == true)
                        {
                            byte[] araySMS = new byte[152];
                            for (int j = 0; j < 152; j++) araySMS[j] = CsConst.myRevBuf[64 + j];
                            string str1 = "";
                            string str2 = "";
                            string str3 = "";
                            byte[] array = new byte[16];
                            Array.Copy(CsConst.myRevBuf, 32, array, 0, 16);
                            str1 = HDLPF.Byte2String(array);
                            array = new byte[12];
                            Array.Copy(CsConst.myRevBuf, 48, array, 0, 12);
                            string strTmp = HDLPF.Byte2String(array);
                            if (strTmp != "")
                            {
                                string str = "20" + strTmp.Substring(0, 2) + "-" + strTmp.Substring(2, 2) + "-" + strTmp.Substring(4, 2);
                                str2 = strTmp.Substring(6, 2) + ":" + strTmp.Substring(8, 2) + ":" + strTmp.Substring(10, 2) + "  " + str;
                            }
                            str3 = HDLPF.Byte22String(araySMS, true);
                            object[] obj = new object[] { dgvMessage.RowCount + 1, str1, str2, str3,
                                       GlobalClass.AddLeftZero(CsConst.myRevBuf[60].ToString("X"),2)+"-"+
                                       GlobalClass.AddLeftZero(CsConst.myRevBuf[61].ToString("X"),2)};
                            dgvMessage.Rows.Add(obj);
                        }
                        else
                        {
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                        
                        HDLUDP.TimeBetwnNext(20);
                    }
                }
            }
            catch
            {
            }
            Cursor.Current = Cursors.Default;
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            byte[] arayTmp = new byte[60];
            string strRemark = txtSned.Text.Trim();
            for (int i = 0; i < 20; i++) arayTmp[i] = 0x0D;
            arayTmp[2] = Convert.ToByte(strRemark.Length);
            if (strRemark != "")
            {
                for (int i = 0; i < strRemark.Length; i++)
                {
                    if ((i + 3) < arayTmp.Length)
                        arayTmp[i + 3] = Convert.ToByte(strRemark[i]);
                }
            }
            strRemark = txtSMS.Text.Trim();
            byte bytSend = 0;
            byte[] arayUnicode = HDLUDP.StringTo2Byte(strRemark, true);
            if (arayUnicode.Length > 40)
            {
                if ((arayUnicode.Length - 40) % 56 == 0)
                    bytSend = Convert.ToByte((arayUnicode.Length - 40) / 56);
                else
                    bytSend = Convert.ToByte(((arayUnicode.Length - 40) / 56) + 1);
            }
            arayTmp[0] = Convert.ToByte(bytSend + 1);
            arayTmp[1] = 1;
            if (bytSend < 1)
            {
                arayTmp[19] = Convert.ToByte(arayUnicode.Length + 1);
                for (int i = 0; i < arayUnicode.Length; i++) arayTmp[i + 20] = arayUnicode[i];
            }
            else
            {
                arayTmp[19] = 20;
                for (int i = 0; i < 40; i++) arayTmp[i + 20] = arayUnicode[i];
            }
            if (CsConst.mySends.AddBufToSndList(arayTmp, 0xC106, SubNetID, DevID, false, true, true, false) == true)
            {
                
                HDLUDP.TimeBetwnNext(20);
            }
            else
            {
                Cursor.Current = Cursors.Default;
                return;
            }
            
            System.Threading.Thread.Sleep(50);
            if (bytSend >= 1)
            {
                for (byte i = 1; i <= bytSend; i++)
                {
                    arayTmp[1] = Convert.ToByte(i + 1);
                    if (bytSend != i) arayTmp[2] = 56;
                    else arayTmp[2] = Convert.ToByte((arayUnicode.Length - 40) % 56);
                    for (int j = 0; j < arayTmp[2]; j++)
                        arayTmp[j + 3] = arayUnicode[(i - 1) * 56 + 40 + j];
                    if (CsConst.mySends.AddBufToSndList(arayTmp, 0xC106, SubNetID, DevID, false, true, true, false) == true)
                    {
                        
                        HDLUDP.TimeBetwnNext(20);
                    }
                    else
                    {
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                }
            }
            Cursor.Current = Cursors.Default;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            byte[] arayTmp = new byte[1];
            arayTmp[0] = 0;
            if (CsConst.mySends.AddBufToSndList(arayTmp, 0xC104, SubNetID, DevID, false, true, true, false) == true)
            {
                
                HDLUDP.TimeBetwnNext(20);
            }
            else
            {
                Cursor.Current = Cursors.Default;
                return;
            }
            dgvMessage.Rows.Clear();
            Cursor.Current = Cursors.Default;
        }

        private void btnSaveOther_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            byte[] arayTmp = new byte[1];
            if (cbto485.Checked) arayTmp[0] = 1;
            if (CsConst.mySends.AddBufToSndList(arayTmp, 0xC10A, SubNetID, DevID, false, true, true, false) == true)
            {
                
                HDLUDP.TimeBetwnNext(20);
            }
            else
            {
                Cursor.Current = Cursors.Default;
                return;
            }
            
            arayTmp = new byte[43];
            if (chb1.Checked) arayTmp[0] = 1;
            if (chb2.Checked) arayTmp[1] = 1;
            if (chb3.Checked) arayTmp[2] = 1;
            if (txtAppend.Text != "")
            {
                byte[] arayRemark = HDLUDP.StringTo2Byte(txtAppend.Text.Trim(), true);
                for (int i = 0; i < arayRemark.Length; i++)
                {
                    if (i < 40)
                    {
                        arayTmp[i + 3] = arayRemark[i];
                    }
                }
            }
            if (CsConst.mySends.AddBufToSndList(arayTmp, 0xC10E, SubNetID, DevID, false, true, true, false) == true)
            {
                
                HDLUDP.TimeBetwnNext(20);
            }
            else
            {
                Cursor.Current = Cursors.Default;
                return;
            }
            
            Cursor.Current = Cursors.Default;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            byte[] arayTmp = new byte[1];
            arayTmp[0] = 0;
            if (CsConst.mySends.AddBufToSndList(arayTmp, 0xC108, SubNetID, DevID, false, true, true, false) == true)
            {
                
                HDLUDP.TimeBetwnNext(20);
            }
            else
            {
                Cursor.Current = Cursors.Default;
                return;
            }
            cbto485.Checked = (CsConst.myRevBuf[25] != 0);
            

            if (CsConst.mySends.AddBufToSndList(arayTmp, 0xC10C, SubNetID, DevID, false, true, true, false) == true)
            {
                
                HDLUDP.TimeBetwnNext(20);
            }
            else
            {
                Cursor.Current = Cursors.Default;
                return;
            }
            chb1.Checked = (CsConst.myRevBuf[25] != 0);
            chb2.Checked = (CsConst.myRevBuf[26] != 0);
            chb3.Checked = (CsConst.myRevBuf[27] != 0);
            byte[] arayRemark = new byte[40];
            for (int i = 0; i < 40; i++) arayRemark[i] = CsConst.myRevBuf[28 + i];
            txtAppend.Text = HDLPF.Byte22String(arayRemark, true);
            
            Cursor.Current = Cursors.Default;
        }

        private void btnSave1_Click(object sender, EventArgs e)
        {
            tsbDown_Click(tbUpload, null);
        }

        private void btnSaveAndClose1_Click(object sender, EventArgs e)
        {
            tsbDown_Click(tbUpload, null);
            this.Close();
        }

        private void btnSave2_Click(object sender, EventArgs e)
        {
            tsbDown_Click(tbUpload, null);
        }

        private void btnSaveAndClose2_Click(object sender, EventArgs e)
        {
            tsbDown_Click(tbUpload, null);
            this.Close();
        }

        private void btnRef3_Click(object sender, EventArgs e)
        {
            btnrefence_Click(null, null);
            button1_Click(null, null);
        }

        private void btnSaveAndClose3_Click(object sender, EventArgs e)
        {
            btnSaveOther_Click(null, null);
            this.Close();
        }
    }
}
