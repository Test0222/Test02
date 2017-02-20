using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace HDL_Buspro_Setup_Tool
{
    public partial class frmRS232 : Form
    {
        [DllImport("user32.dll")]
        public static extern Boolean FlashWindow(IntPtr hwnd, Boolean bInvert);
        private RS232 myRS232 = null;
        private string myDevName = null;
        private int mintIDIndex = -1;
        private int mintDeviceType = -1;
        public bool blnIsFirstLoading = true;
        private int MyActivePage = 0; //按页面上传下载
        public List<Rs232Param> myTmpCMDLists = null;
        private byte SubNetID;
        private byte DeviceID;
        private bool isRead = false;
        NetworkInForm networkinfo;
        public frmRS232()
        {
            InitializeComponent();
        }

        public frmRS232(RS232 myrs232, string strName, int intDIndex,int intDeviceType)
        {
            InitializeComponent();
            this.myRS232 = myrs232;
            this.myDevName = strName;
            this.mintIDIndex = intDIndex;
            this.Text = strName;
            this.mintDeviceType = intDeviceType;

            string strDevName = strName.Split('\\')[0].ToString();

            HDLSysPF.DisplayDeviceNameModeDescription(strName, mintDeviceType, cboDevice, tbModel, tbDescription);

            this.SubNetID = Convert.ToByte(strDevName.Split('-')[0]);
            this.DeviceID = Convert.ToByte(strDevName.Split('-')[1]);
            this.Text = strName;
            tsl3.Text = strName;
        }

        void InitialFormCtrlsTextOrItems()
        {
            cbMode.Items.Clear();
            HDLSysPF.setRS232ModeByDB(mintDeviceType, cbMode);
            clAC6.Items.Clear();
            for (int i = 0; i < 3; i++)
                clAC6.Items.Add(CsConst.mstrINIDefault.IniReadValue("public", "0082" + i.ToString(), ""));

            cbType.Items.Clear();
            for (int i = 0; i < 3; i++)
                cbType.Items.Add(CsConst.mstrINIDefault.IniReadValue("public", "0051" + i.ToString(), ""));

            clM2.Items.Clear();
            clM2.Items.Add(CsConst.WholeTextsList[1775].sDisplayName);
            clM2.Items.Add(CsConst.mstrINIDefault.IniReadValue("public", "00042", ""));

            clM6.Items.Clear();
            clM6.Items.Add(CsConst.mstrINIDefault.IniReadValue("public", "00202", ""));
            clM6.Items.Add(CsConst.mstrINIDefault.IniReadValue("public", "00032", ""));
        }

        void SetCtrlsVisbleWithDifferentDeviceType()
        {
            toolStrip1.Visible = (CsConst.MyEditMode == 0);

            if (mintDeviceType == 1016 || mintDeviceType == 1019 || mintDeviceType == 1014 || mintDeviceType == 1033 ||
                mintDeviceType == 1018 || mintDeviceType == 1027 || mintDeviceType == 1039)
            {
                lb232ToBus.Text = lb232ToBus.Text.Split('-')[0].ToString() + "-200)";
                lbBus1.Text = lbBus1.Text.Split('-')[0].ToString() + "-200)";
            }
            else
            {
                lb232ToBus.Text = lb232ToBus.Text.Split('-')[0].ToString() + "-99)";
                lbBus1.Text = lbBus1.Text.Split('-')[0].ToString() + "-99)";
            }

            grpReply.Visible = (mintDeviceType == 1018 || mintDeviceType == 1027);
            if (mintDeviceType == 1040 || mintDeviceType == 1004)
            {
                clAC6.Visible = true;
                clAC7.Visible = true;
                clAC8.Visible = true;
            }
            if (mintDeviceType == 1041)
            {
                btn485.Visible = false;
                tab232.TabPages.Remove(tabBus232);
                tab232.TabPages.Remove(tab485Bus);
                tab232.TabPages.Remove(tabBus485);
                clA3.Visible = false;
                clA4.Visible = false;
                clA5.Visible = false;
                clA6.Visible = true;
                clA7.Visible = true;
                clA8.Visible = true;
                clBusTarget3.Visible = false;
                clBusTarget4.Visible = false;
                clBusTarget5.Visible = true;
                clBusTarget6.Visible = true;
                clBusTarget7.Visible = true;
                clBusTarget8.Visible = true;
                clAC6.Visible = true;
                clAC7.Visible = true;
                clAC8.Visible = true;
                lbCount.Text = lbCount.Text.Split('-')[0].ToString() + "-10):";
                btnOther2.Text = btnOther2.Text.Split('-')[0].ToString() + "-10)";
                lb232ToBus.Text = lb232ToBus.Text.Split('-')[0].ToString() + "-320):";
            }


            if (!Rs232DeviceTypeList.Rs233FhConvertorGateway.Contains(mintDeviceType))
            {
                tab232.TabPages.Remove(tabFH);
            }
            
            if (!Rs232DeviceTypeList.Rs232FilterConvertorGateway.Contains(mintDeviceType))
            {
                tab232.TabPages.Remove(tabFilter);
            }
            if (!Rs232DeviceTypeList.Rs232ommaxConvertorGateway.Contains(mintDeviceType))
            {
                tab232.TabPages.Remove(tabCommax);
            }
            if (!Rs232DeviceTypeList.Rs233AcConvertorGateway.Contains(mintDeviceType))
            {
                tab232.TabPages.Remove(tabAC);
            }

            if (!Rs232DeviceTypeList.Rs233CurtainConvertorGateway.Contains(mintDeviceType))
            {
                tab232.TabPages.Remove(tabCurtain);
                grpSomfy.Visible = false;
            }
            if (mintDeviceType == 1006 || mintDeviceType == 1016 || mintDeviceType == 1017 || mintDeviceType == 1019
                || mintDeviceType == 1020 || mintDeviceType == 1021 || mintDeviceType == 1022 || mintDeviceType == 1023
                || mintDeviceType == 1024 || mintDeviceType == 1029 || mintDeviceType == 1030 || mintDeviceType == 1031
                || mintDeviceType == 1032 || mintDeviceType == 1033 || mintDeviceType == 1034 || mintDeviceType == 1035
                || mintDeviceType == 1036)
            {
                btn485.Visible = false;
                tab232.TabPages.Remove(tab485Bus);
                tab232.TabPages.Remove(tabBus485);
            }

            if (Rs232DeviceTypeList.Rs232DoesNotHave232485Tab.Contains(mintDeviceType))
            {
                btn485.Visible = false;
                tab232.TabPages.Remove(tab232Bus);
                tab232.TabPages.Remove(tabBus232);
                tab232.TabPages.Remove(tab485Bus);
                tab232.TabPages.Remove(tabBus485);
            }

            if (Rs232DeviceTypeList.Rs233CurtainConvertorGateway .Contains(mintDeviceType))
            {
                tab232.TabPages.Remove(tabCurtain);
            }
            if (mintDeviceType == 1034)
            {
                tab232.TabPages.Remove(tab232Bus);
                tab232.TabPages.Remove(tabBus232);
            }

            if (mintDeviceType == 1040)
            {
                tab232.TabPages.Remove(tabBus232);
                tab232.TabPages.Remove(tab485Bus);
                tab232.TabPages.Remove(tabBus485);
            }
            if(mintDeviceType == 1042)    //20170217增加这个
            {
                //tab232.TabPages.Remove(tabBus232);
                //tab232.TabPages.Remove(tab232Bus);
                //tab232.TabPages.Remove(tab485Bus);
                //tab232.TabPages.Remove(tabBus485);
                tab232.TabPages.Add(tabCurtain);
                grpMode.Visible = false;
                grpSomfy.Visible = false;
            }
        }

        private void frmRS232_Load(object sender, EventArgs e)
        {
            InitialFormCtrlsTextOrItems();
            //Public.Localization.ReadLanguage(((Control)sender), Public.Localization.LanguageType.English);
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            if (myRS232 == null) return;
            Cursor.Current = Cursors.WaitCursor;
            myRS232.SaveRS232ToDB();
            Cursor.Current = Cursors.Default;
        }
        
        private void DisplayBasicInfo()
        {
            try
            {
                isRead = true;
                pnl5.Visible = (mintDeviceType == 1018 || mintDeviceType == 1027);
                pnl5.Visible = (CsConst.mintDevicesHasIPNetworkDevType.Contains(mintDeviceType));
                txtID.Text = myRS232.SomfyID;
                if (mintDeviceType == 1019 || mintDeviceType == 1033 || mintDeviceType == 1039)
                {
                    if (myRS232.bytMode == 1) cbMode.SelectedIndex = 2;
                    else if (myRS232.bytMode == 2) cbMode.SelectedIndex = 3;
                    else if (myRS232.bytMode == 3) cbMode.SelectedIndex = 1;
                    else if (myRS232.bytMode == 4) cbMode.SelectedIndex = 4;
                    else cbMode.SelectedIndex = 0;
                }
                else
                {
                    if (cbMode.Items.Count > 0 && myRS232.bytMode < cbMode.Items.Count)
                        cbMode.SelectedIndex = myRS232.bytMode;
                }
                if (grpReply.Visible)
                {
                    if (myRS232.ArayExtra[0] == 0) rb1.Checked = true;
                    else if (myRS232.ArayExtra[0] == 1) rb2.Checked = true;
                    else if (myRS232.ArayExtra[0] == 2) rb3.Checked = true;
                    if (myRS232.ArayExtra[1] == 1) chb1.Checked = true;
                    else chb1.Checked = false;
                    if (myRS232.ArayExtra[2] == 1) chb2.Checked = true;
                    else chb2.Checked = false;
                }
                if (grpReply.Visible)
                {
                    if (rb1.Checked)
                    {
                        if (!tab232.TabPages.Contains(tab232Bus))
                            tab232.TabPages.Add(tab232Bus);
                        if (!tab232.TabPages.Contains(tabBus232))
                            tab232.TabPages.Add(tabBus232);
                        if (!tab232.TabPages.Contains(tab485Bus))
                            tab232.TabPages.Add(tab485Bus);
                        if (!tab232.TabPages.Contains(tabBus485))
                            tab232.TabPages.Add(tabBus485);
                        lb232ToBus.Text = lb232ToBus.Text.Split('-')[0].ToString() + "-99):";
                        lbBus1.Text = lbBus1.Text.Split('-')[0].ToString() + "-99):";
                        lb485Bus.Text = lb485Bus.Text.Split('-')[0].ToString() + "-99):";
                        lbBus485.Text = lbBus485.Text.Split('-')[0].ToString() + "-99):";
                    }
                    else if (rb2.Checked)
                    {
                        if (!tab232.TabPages.Contains(tab232Bus))
                            tab232.TabPages.Add(tab232Bus);
                        if (!tab232.TabPages.Contains(tabBus232))
                            tab232.TabPages.Add(tabBus232);
                        tab232.TabPages.Remove(tab485Bus);
                        tab232.TabPages.Remove(tabBus485);
                        lb232ToBus.Text = lb232ToBus.Text.Split('-')[0].ToString() + "-200):";
                        lbBus1.Text = lbBus1.Text.Split('-')[0].ToString() + "-200):";
                        lb485Bus.Text = lb485Bus.Text.Split('-')[0].ToString() + "-200):";
                        lbBus485.Text = lbBus485.Text.Split('-')[0].ToString() + "-200):";
                    }
                    else if (rb3.Checked)
                    {
                        tab232.TabPages.Remove(tab232Bus);
                        tab232.TabPages.Remove(tabBus232);
                        if (!tab232.TabPages.Contains(tab485Bus))
                            tab232.TabPages.Add(tab485Bus);
                        if (!tab232.TabPages.Contains(tabBus485))
                            tab232.TabPages.Add(tabBus485);
                        lb232ToBus.Text = lb232ToBus.Text.Split('-')[0].ToString() + "-200):";
                        lbBus1.Text = lbBus1.Text.Split('-')[0].ToString() + "-200):";
                        lb485Bus.Text = lb485Bus.Text.Split('-')[0].ToString() + "-200):";
                        lbBus485.Text = lbBus485.Text.Split('-')[0].ToString() + "-200):";
                    }
                }
            }
            catch
            {
            }
            isRead = false;
        }

        private void DisplayRS232BUSInfo()
        {
            try
            {
                int wdMaxValue = int.Parse(CsConst.mstrINIDefault.IniReadValue("DeviceType" + mintDeviceType.ToString(), "MaxValue", "33"));
                isRead = true;
                dgv232ToBus.Rows.Clear();
                if (myRS232 == null) return;
                if (myRS232.myRS2BUS == null) return;
                for (int i = 0; i < myRS232.myRS2BUS.Count; i++)
                {
                    string str1 = "N/A";
                    string str2 = "N/A";
                    string str3 = "N/A";
                    string str4 = "N/A";
                    Rs232ToBus temp = myRS232.myRS2BUS[i];
                    string strEnable = CsConst.WholeTextsList[1775].sDisplayName;
                    if (temp.rs232Param.enable == 1) strEnable = CsConst.mstrINIDefault.IniReadValue("public", "00042", "");
                    string strType = CsConst.WholeTextsList[1775].sDisplayName;
                    string strCMD = "";
                    if (temp.rs232Param.type == 0)
                    {
                        strType = CsConst.mstrINIDefault.IniReadValue("public", "99838", "");
                        //int Count = temp.rs232Param.rsCmd[temp.rs232Param.RSCMD.Length - 1];
                        //if (Count > wdMaxValue) Count = wdMaxValue;
                        //byte[] arayTmp = new byte[Count];
                        //Array.Copy(temp.rs232Param.RSCMD, 0, arayTmp, 0, Count);
                        //if (Count == 0)
                        //    strCMD = "";
                        //else
                           // strCMD = HDLPF.Byte2String(arayTmp);
                    }
                    else if (temp.rs232Param.type == 1)
                    {
                        strType = CsConst.mstrINIDefault.IniReadValue("public", "99839", "");
                        //int Count = temp.rs232Param.rsCmd[temp.TmpRS232.RSCMD.Length - 1];
                        //if (Count > wdMaxValue) Count = wdMaxValue;
                        //for (int j = 0; j < Count; j++)
                        //{
                        //    strCMD = strCMD + GlobalClass.AddLeftZero(temp.rs232Param.rsCmd[j].ToString("X"), 2) + " ";
                        //}
                    }
                    strCMD = strCMD.Trim();
                    if (mintDeviceType == 1041)
                    {
                        strCMD = "N/A";
                        str1 = CsConst.mstrINIDefault.IniReadValue("keyMode", "00000", "");
                        if (temp.rs232Param.enable == 2)
                            str1 = CsConst.mstrINIDefault.IniReadValue("keyMode", "00002", "");
                        else if (temp.rs232Param.enable == 3)
                            str1 = CsConst.mstrINIDefault.IniReadValue("keyMode", "00003", "");
                        else if (temp.rs232Param.enable == 1)
                            str1 = CsConst.mstrINIDefault.IniReadValue("keyMode", "00001", "");
                        else if (temp.rs232Param.enable == 4)
                            str1 = CsConst.mstrINIDefault.IniReadValue("keyMode", "00004", "");
                        else if (temp.rs232Param.enable == 5)
                            str1 = CsConst.mstrINIDefault.IniReadValue("keyMode", "00005", "");
                        else if (temp.rs232Param.enable == 7)
                            str1 = CsConst.mstrINIDefault.IniReadValue("keyMode", "00007", "");
                        str2 = temp.rs232Param.type.ToString();
                        str3 = temp.rs232Param.rsCmd[0].ToString();
                        if (myRS232.bytMode == 0)
                        {
                            //str4 = CsConst.mstrINIDefault.IniReadValue("public", "99551", "");
                            //if(temp.TmpRS232.RSCMD[1]==1)
                            //    str4 = CsConst.mstrINIDefault.IniReadValue("public", "99551", "");
                            //else if(temp.TmpRS232.RSCMD[1]==2)
                                str4 = CsConst.mstrINIDefault.IniReadValue("public", "99552", "");
                        }
                        else if (myRS232.bytMode == 1 || myRS232.bytMode == 2)
                        {
                            str4 = "N/A";
                        }
                    }
                    object[] obj = new object[] { temp.ID.ToString(), temp.remark.ToString(), strEnable, strType, strCMD,
                                                 str1,str2,str3,str4};
                    dgv232ToBus.Rows.Add(obj);
                }
                if (dgv232ToBus.RowCount > 0)
                    dgv232ToBus_CellClick(dgv232ToBus, new DataGridViewCellEventArgs(0, 0));
            }
            catch
            {
            }
            isRead = false;
        }

        private void DisplayBUSRS232Info()
        {
            try
            {
                isRead = true;
                dgvBUSto232.Rows.Clear();
                if (myRS232 == null) return;
                if (myRS232.myBUS2RS == null) return;
                for (int i = 0; i < myRS232.myBUS2RS.Count; i++)
                {
                    RS232.BUS2RS temp = myRS232.myBUS2RS[i];
                    if (mintDeviceType == 1041)
                    {
                        string strType = CsConst.mstrINIDefault.IniReadValue("TargetType", "00000", "");
                        if (temp.bytType == 0xBB) strType = CsConst.myPublicControlType[14].ControlTypeName;
                        string strParam1 = temp.bytParam1.ToString() + "/" + temp.bytParam2.ToString();
                        string strParam2 = temp.bytParam3.ToString() + "/" + temp.bytParam4.ToString();
                        object[] obj = new object[] { temp.ID.ToString(), temp.Remark.ToString(), strType, strParam1, strParam2 };
                        dgvBUSto232.Rows.Add(obj);
                    }
                    else
                    {
                        string strType = CsConst.mstrINIDefault.IniReadValue("TargetType", "00000", "");
                        if (temp.bytType == 0x58) strType = CsConst.myPublicControlType[4].ControlTypeName;
                        string strParam1 = temp.bytParam1.ToString() + "(" +
                                           CsConst.WholeTextsList[2513].sDisplayName + ")";
                        string strParam2 = CsConst.Status[0] + "(" +
                                           CsConst.WholeTextsList[2529].sDisplayName + ")";
                        if (temp.bytParam2 == 255)
                            strParam2 = CsConst.Status[1] + "(" +
                                        CsConst.WholeTextsList[2529].sDisplayName + ")";
                        object[] obj = new object[] { temp.ID.ToString(), temp.Remark.ToString(), strType, strParam1, strParam2 };
                        dgvBUSto232.Rows.Add(obj);
                    }
                }
                if (dgvBUSto232.RowCount > 0)
                    dgvBUSto232_CellClick(dgvBUSto232, new DataGridViewCellEventArgs(0, 0));
            }
            catch
            {
            }
            isRead = false;
        }

        private void DisplayRS485BUSInfo()
        {
            try
            {
                int wdMaxValue = int.Parse(CsConst.mstrINIDefault.IniReadValue("DeviceType" + mintDeviceType.ToString(), "MaxValue", "33"));
                isRead = true;
                dgv485Bus.Rows.Clear();
                if (myRS232 == null) return;
                if (myRS232.my4852BUS == null) return;
                for (int i = 0; i < myRS232.my4852BUS.Count; i++)
                {
                    Rs232ToBus temp = myRS232.my4852BUS[i];
                    string strEnable = CsConst.WholeTextsList[1775].sDisplayName;
                    if (temp.rs232Param.enable == 1) strEnable = CsConst.mstrINIDefault.IniReadValue("public", "00042", "");
                    string strType = CsConst.WholeTextsList[1775].sDisplayName;
                    string strCMD = "";
                    if (temp.rs232Param.type == 0)
                    {
                        strType = CsConst.mstrINIDefault.IniReadValue("public", "99838", "");
                        //int Count = temp.rs232Param.rsCmd[temp.TmpRS232.RSCMD.Length - 1];
                        //if (Count > wdMaxValue) Count = wdMaxValue;
                        //byte[] arayTmp = new byte[Count];
                        //Array.Copy(temp.rs232Param.RSCMD, 0, arayTmp, 0, Count);
                        //if (Count == 0)
                        //    strCMD = "";
                        //else
                           // strCMD = HDLPF.Byte2String(arayTmp);
                    }
                    else if (temp.rs232Param.type == 1)
                    {   
                        strType = CsConst.mstrINIDefault.IniReadValue("public", "99839", "");
                        //int Count = temp.rs232Param.rsCmd[temp.TmpRS232.RSCMD.Length - 1];
                        //if (Count > wdMaxValue) Count = wdMaxValue;
                        //for (int j = 0; j < Count; j++)
                        //{
                        //    strCMD = strCMD + GlobalClass.AddLeftZero(temp.TmpRS232.RSCMD[j].ToString("X"), 2) + " ";
                        //}
                    }
                    strCMD = strCMD.Trim();
                    object[] obj = new object[] { temp.ID.ToString(), temp.remark.ToString(), strEnable, strType, strCMD };
                    dgv485Bus.Rows.Add(obj);
                }
                if (dgv485Bus.RowCount > 0)
                    dgv485Bus_CellClick(dgv485Bus, new DataGridViewCellEventArgs(0, 0));
            }
            catch
            {
            }
            isRead = false;
        }

        private void DisplayBUSRS485Info()
        {
            try
            {
                isRead = true;
                dgvBus485.Rows.Clear();
                if (myRS232 == null) return;
                if (myRS232.myBUS2485 == null) return;
                for (int i = 0; i < myRS232.myBUS2485.Count; i++)
                {
                    RS232.BUS2RS temp = myRS232.myBUS2485[i];
                    string strType = CsConst.mstrINIDefault.IniReadValue("TargetType", "00000", "");
                    if (temp.bytType == 0x58) strType = CsConst.myPublicControlType[4].ControlTypeName;
                    string strParam1 = temp.bytParam1.ToString() + "(" +
                                       CsConst.WholeTextsList[2513].sDisplayName + ")";
                    string strParam2 = CsConst.Status[0] + "(" +
                                       CsConst.WholeTextsList[2529].sDisplayName + ")";
                    if (temp.bytParam2 == 255)
                        strParam2 = CsConst.Status[1] + "(" +
                                    CsConst.WholeTextsList[2529].sDisplayName + ")";
                    object[] obj = new object[] { temp.ID.ToString(), temp.Remark.ToString(), strType, strParam1, strParam2 };
                    dgvBus485.Rows.Add(obj);
                }
                if (dgvBus485.RowCount > 0)
                    dgvBus485_CellClick(dgvBus485, new DataGridViewCellEventArgs(0, 0));
            }
            catch
            {
            }
            isRead = false;
        }

        private void tsbDown_Click(object sender, EventArgs e)
        {
            byte bytTag = Convert.ToByte(((ToolStripButton)sender).Tag.ToString());
            bool blnShowMsg = (CsConst.MyEditMode != 1);
            if (HDLPF.UploadOrReadGoOn(this.Text, bytTag, blnShowMsg))
            {
                Cursor.Current = Cursors.WaitCursor;
                CsConst.MyUPload2DownLists = new List<byte[]>();

                string strName = myDevName.Split('\\')[0].ToString();  //名字   如 1-1\\1端口交换机
                byte bytSubID = byte.Parse(strName.Split('-')[0]);
                byte bytDevID = byte.Parse(strName.Split('-')[1]);
                int num1 = 1;
                int num2 = 1;
                switch (tab232.SelectedTab.Name)
                {
                    case "tabPage1": num1 = Convert.ToInt32(txt232ToBusFrm.Text);
                        num2 = Convert.ToInt32(txt232ToBusTo.Text); break;
                    case "tabPage2": num1 = Convert.ToInt32(txtBusFrm.Text);
                        num2 = Convert.ToInt32(txtBusTo.Text); break;
                    case "tabPage3": num1 = Convert.ToInt32(txtFrm485Bus.Text);
                        num2 = Convert.ToInt32(txtTo485Bus.Text); break;
                    case "tabPage4": num1 = Convert.ToInt32(txtFrmBus485.Text);
                        num2 = Convert.ToInt32(txtToBus485.Text); break;
                    //  case "tabPage8": num1 = Convert.ToInt32(txtC1.Text);
                    //                   num2 = Convert.ToInt32(txtC2.Text); break;
                    case "tabCurtain": num1 = Convert.ToInt32(txtC1.Text);
                        num2 = Convert.ToInt32(txtC2.Text); break;
                }

                byte[] ArayRelay = new byte[] { bytSubID, bytDevID, (byte)(mintDeviceType / 256), (byte)(mintDeviceType % 256),(byte)MyActivePage, 
                                                (byte)(mintIDIndex / 256), (byte)(mintIDIndex % 256),Convert.ToByte(num1/256),Convert.ToByte(num1%256),
                                               Convert.ToByte(num2/256),Convert.ToByte(num2%256)};
                CsConst.MyUPload2DownLists.Add(ArayRelay);
                CsConst.MyUpload2Down = Convert.ToByte(((ToolStripButton)sender).Tag.ToString());
                FrmDownloadShow Frm = new FrmDownloadShow();
                if (CsConst.MyUpload2Down == 0) Frm.FormClosing += new FormClosingEventHandler(UpdateDisplayInformationAccordingly);
                Frm.ShowDialog();

               
            }
        }


        void UpdateDisplayInformationAccordingly(object sender, FormClosingEventArgs e)
        {
            switch (tab232.SelectedTab.Name)
            {
                case "tabBasic": DisplayBasicInfo(); break;
                case "tab232Bus": DisplayRS232BUSInfo(); break;
                case "tabBus232": DisplayBUSRS232Info(); break;
                case "tabPage3": DisplayRS485BUSInfo(); break;
                case "tabPage4": DisplayBUSRS485Info(); break;
                case "tabPage6": DisplayFilterInfo(); break;
                case "tabAC": DisplayACInfo(); break;
              //  case "tabPage8": DisplayCurtainInfo(); break;
                case "tabCurtain": DisplayCurtainInfo(); break;
                case "tabFH": DisplayFHInfo(); break;
            }
            Cursor.Current = Cursors.Default;
            this.TopMost = true;
            this.WindowState = FormWindowState.Normal;
            this.Activate();
            this.TopMost = false;
        }

        private void DisplayCurtainInfo()
        {
            try
            {
                isRead = true;
                dgvCurtain.Rows.Clear();
                for (int i = 0; i < myRS232.myCurTain.Count; i++)
                {
                    object[] obj = new object[] { myRS232.myCurTain[i][0].ToString(), myRS232.myCurTain[i][1].ToString(), 
                                    myRS232.myCurTain[i][2].ToString()};
                    dgvCurtain.Rows.Add(obj);
                }
            }
            catch
            {
            }
            isRead = false;
        }

        private void DisplayACInfo()
        {
            try
            {
                isRead = true;
                dgvAC.Rows.Clear();
                if (myRS232 == null) return;
                if (myRS232.Time > 50) myRS232.Time = 50;
                if (myRS232.Time <= 0) myRS232.Time = 1;
                numTime1.Value = myRS232.Time / 10;
                numTime2.Value = myRS232.Time % 10;
                txtCount.Text = myRS232.Count.ToString();
                if (myRS232.myAC == null) return;
                if (mintDeviceType == 1034 || mintDeviceType == 1004)
                {
                    for (int i = 0; i < myRS232.myAC.Count; i++)
                    {
                        byte[] arayRemark = new byte[10];
                        Array.Copy(myRS232.myAC[i], 4, arayRemark, 0, 10);
                        string strRemark = HDLPF.Byte2String(arayRemark);
                        object[] obj = new object[] { (myRS232.myAC[i][0]).ToString(),strRemark,(myRS232.myAC[i][1].ToString()),
                        myRS232.myAC[i][2].ToString(),myRS232.myAC[i][3].ToString()};
                        dgvAC.Rows.Add(obj);
                    }
                }
                else if (mintDeviceType == 1040)
                {
                    for (int i = 0; i < myRS232.myAC.Count; i++)
                    {
                        byte bytControlway = myRS232.myAC[i][1];
                        if (bytControlway > 3 || bytControlway < 1)
                        {
                            bytControlway = 1;
                            myRS232.myAC[i][1] = 1;
                        }
                        byte[] arayRemark = new byte[10];
                        Array.Copy(myRS232.myAC[i], 9, arayRemark, 0, 10);
                        string strRemark = HDLPF.Byte2String(arayRemark);
                        object[] obj = new object[] { (myRS232.myAC[i][0]).ToString(),strRemark,(myRS232.myAC[i][2].ToString()),
                                                       (myRS232.myAC[i][5]*256+myRS232.myAC[i][6]).ToString(),
                                                       (myRS232.myAC[i][7]*256+myRS232.myAC[i][8]).ToString(),
                                                       clAC6.Items[bytControlway-1].ToString(),
                                                       myRS232.myAC[i][3].ToString(),myRS232.myAC[i][4].ToString()};
                        dgvAC.Rows.Add(obj);
                    }
                }
                else if (mintDeviceType == 1041)
                {
                    for (int i = 0; i < myRS232.myAC.Count; i++)
                    {
                        byte bytControlway = myRS232.myAC[i][1];
                        if (bytControlway > 3 || bytControlway < 1)
                        {
                            bytControlway = 1;
                            myRS232.myAC[i][1] = 1;
                        }
                        byte[] arayRemark = new byte[10];
                        Array.Copy(myRS232.myAC[i], 7, arayRemark, 0, 10);
                        string strRemark = HDLPF.Byte2String(arayRemark);
                        object[] obj = new object[] { (myRS232.myAC[i][0]).ToString(),strRemark,(myRS232.myAC[i][2].ToString()),
                                                       myRS232.myAC[i][5].ToString(),myRS232.myAC[i][6].ToString(),
                                                       clAC6.Items[bytControlway-1].ToString(),
                                                       myRS232.myAC[i][3].ToString(),myRS232.myAC[i][4].ToString()};
                        dgvAC.Rows.Add(obj);
                    }
                }
                else
                {
                    for (int i = 0; i < myRS232.myAC.Count; i++)
                    {
                        byte[] arayRemark = new byte[10];
                        Array.Copy(myRS232.myAC[i], 6, arayRemark, 0, 10);
                        string strRemark = HDLPF.Byte2String(arayRemark);
                        object[] obj = new object[] { (myRS232.myAC[i][0]).ToString(),strRemark,(myRS232.myAC[i][1].ToString()),
                        (myRS232.myAC[i][2]*256+myRS232.myAC[i][3]).ToString(),(myRS232.myAC[i][4]*256+myRS232.myAC[i][5]).ToString()};
                        dgvAC.Rows.Add(obj);
                    }
                }
            }
            catch
            {
            }
            isRead = false;
        }

        private void DisplayFilterInfo()
        {
            try
            {
                isRead = true;
                dgvFilter.Rows.Clear();
                if (myRS232 == null) return;
                if (myRS232.myFilter == null) return;
                for (int i = 0; i < myRS232.myFilter.Count; i++)
                {
                    object[] obj = new object[] { (myRS232.myFilter[i][0]+1).ToString(),(myRS232.myFilter[i][1]==1),
                    myRS232.myFilter[i][2].ToString(),myRS232.myFilter[i][3].ToString()};
                    dgvFilter.Rows.Add(obj);
                }
            }
            catch
            {
            }
            isRead = false;
        }


        private void DisplayFHInfo()
        {
            try
            {
                isRead = true;
                dgFh.Rows.Clear();
                if (myRS232 == null) return;
                if (myRS232.Time < 50) myRS232.Time = 50;
                if (myRS232.Time <= 0) myRS232.Time = 1;
                numTimeFh1.Value = myRS232.Time / 10;
                numTimeFh2.Value = myRS232.Time % 10;
                tbFhCount.Text = myRS232.Count.ToString();
                if (myRS232.myAC == null) return;

                for (int i = 0; i < myRS232.myAC.Count; i++)
                {
                    byte[] arayRemark = new byte[10];
                    Array.Copy(myRS232.myAC[i], 6, arayRemark, 0, 8);
                    string strRemark = HDLPF.Byte2String(arayRemark);
                    object[] obj = new object[] { (myRS232.myAC[i][0]).ToString(),strRemark,(myRS232.myAC[i][1].ToString()),
                                                   myRS232.myAC[i][2].ToString(),myRS232.myAC[i][3].ToString(),myRS232.myAC[i][5]==1,
                                                   myRS232.myAC[i][4].ToString()};
                    dgFh.Rows.Add(obj);
                }
            }
            catch
            {
            }
            isRead = false;
        }

        private void tmRead_Click(object sender, EventArgs e)
        {
            tsbDown_Click(tsbDown, null);
        }

        private void frmRS232_Shown(object sender, EventArgs e)
        {
            SetCtrlsVisbleWithDifferentDeviceType();
            if (CsConst.MyEditMode == 1)
            {
                MyActivePage = 1;
                if (myRS232.MyRead2UpFlags[0] == false)
                {
                    tsbDown_Click(tsbDown, null);
                }
                else
                {
                    switch (tab232.SelectedTab.Name)
                    {
                        case "tabPage5": DisplayBasicInfo(); break;
                        case "tabPage1": DisplayRS232BUSInfo(); break;
                        case "tabPage2": DisplayBUSRS232Info(); break;
                        case "tabPage3": DisplayRS485BUSInfo(); break;
                        case "tabPage4": DisplayBUSRS485Info(); break;
                        case "tabPage6": DisplayFilterInfo(); break;
                        case "tabPage7": cbType_SelectedIndexChanged(null, null); break;
                    }
                }

                if (CsConst.mintDevicesHasIPNetworkDevType.Contains(mintDeviceType))
                {
                    pnl5.Controls.Clear();
                    networkinfo = new NetworkInForm(SubNetID, DeviceID, mintDeviceType);
                    pnl5.Controls.Add(networkinfo);
                    networkinfo.Dock = DockStyle.Fill;
                }
            }
        }

        private void tab232_SelectedIndexChanged(object sender, EventArgs e)
        {
            isRead = true;
            switch (tab232.SelectedTab.Name)
            {
                case "tabBasic": MyActivePage=1; break;
                case "tab232Bus": MyActivePage=2; break;
                case "tabBus232": MyActivePage = 3; break;
                case "tab485Bus": MyActivePage = 4; break;
                case "tabBus485": MyActivePage = 5; break;
                case "tabPage6": MyActivePage = 6; break;
                case "tabPage7": MyActivePage = 7; break;
                case "tabAC":
                case "tabFH":    MyActivePage = 8; break;
               // case "tabPage8": MyActivePage = 9; break;
                case "tabCurtain": MyActivePage = 9; break;
            }
            if (CsConst.MyEditMode == 1)
            {
                if (tab232.SelectedTab.Name == "tabPage7")
                {
                    isRead = true;
                    btnReadMax_Click(null, null);
                    isRead = false;
                }
                else
                {
                    if (myRS232.MyRead2UpFlags[MyActivePage - 1] == false)
                    {
                        tsbDown_Click(tsbDown, null);
                    }
                    else
                    {
                        UpdateDisplayInformationAccordingly(null, null);
                    }
                }
            }
        }

        private void txt4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar) || e.KeyChar == 8)
                e.Handled = false;
            else
                e.Handled = true;
        }

        private void txtID_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar) || e.KeyChar == 8 ||
                e.KeyChar == 65 || e.KeyChar == 66 || e.KeyChar == 67 || e.KeyChar == 68 || e.KeyChar == 69 || e.KeyChar == 70 ||
                e.KeyChar == 97 || e.KeyChar == 98 || e.KeyChar == 99 || e.KeyChar == 100 || e.KeyChar == 101 || e.KeyChar == 102
                )
                e.Handled = false;
            else
                e.Handled = true;
        }

        private void btnPort_Click(object sender, EventArgs e)
        {
            FrmBoundRate frmtmp = new FrmBoundRate(SubNetID, DeviceID, myRS232, false);
            frmtmp.ShowDialog();
        }

        private void btnMode_Click(object sender, EventArgs e)
        {
            btnMode.Enabled = false;
            Cursor.Current = Cursors.WaitCursor;
            cbMode_SelectedIndexChanged(null, null);
            byte[] arayTmp = new byte[1];
            arayTmp[0] = myRS232.bytMode;
            if (CsConst.mySends.AddBufToSndList(arayTmp, 0xE42E, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(mintDeviceType)) == true)
            {
                
            }
            Cursor.Current = Cursors.Default;
            btnMode.Enabled = true;
        }

        private void btnID_Click(object sender, EventArgs e)
        {
            btnID.Enabled = false;
            Cursor.Current = Cursors.WaitCursor;
            byte[] arayTmp = new byte[3];
            while (myRS232.SomfyID.Length < 6) myRS232.SomfyID = "0" + myRS232.SomfyID;
            arayTmp[0] = Convert.ToByte(Convert.ToInt32(myRS232.SomfyID.Substring(0, 2), 16));
            arayTmp[1] = Convert.ToByte(Convert.ToInt32(myRS232.SomfyID.Substring(2, 2), 16));
            arayTmp[2] = Convert.ToByte(Convert.ToInt32(myRS232.SomfyID.Substring(4, 2), 16));
            if (CsConst.mySends.AddBufToSndList(arayTmp, 0x1F52, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(mintDeviceType)) == true)
            {
                
            }
            Cursor.Current = Cursors.Default;
            btnID.Enabled = true;
        }


        private void cbMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            if (mintDeviceType == 1007)
            {
                myRS232.bytMode = Convert.ToByte(HDLSysPF.GetRS232ModeByString(cbMode.Text));
            }
            else if (mintDeviceType == 10008 || mintDeviceType == 1009 || mintDeviceType == 1013 || mintDeviceType == 1016
                  || mintDeviceType == 1014)
            {
                myRS232.bytMode = Convert.ToByte(HDLSysPF.GetRS232ModeByString(cbMode.Text));
            }
            else if (mintDeviceType == 1017 || mintDeviceType == 1018 || mintDeviceType == 1020 || mintDeviceType == 1027 ||
                     mintDeviceType == 1029 || mintDeviceType == 1030 || mintDeviceType == 1031 || mintDeviceType == 1032 ||
                     mintDeviceType == 1034 || mintDeviceType == 1035 || mintDeviceType == 1036 || mintDeviceType == 1040)
            {
                myRS232.bytMode = Convert.ToByte(cbMode.SelectedIndex);
            }
            else if (mintDeviceType == 1019 || mintDeviceType == 1033 || mintDeviceType == 1039)
            {
                switch (cbMode.SelectedIndex)
                {
                    case 0: myRS232.bytMode = 0; break;
                    case 1: myRS232.bytMode = 3; break;
                    case 2: myRS232.bytMode = 1; break;
                    case 3: myRS232.bytMode = 2; break;
                    case 4: myRS232.bytMode = 4; break;
                }
            }
            else if (mintDeviceType == 1041)
            {
                myRS232.bytMode = Convert.ToByte(cbMode.SelectedIndex);
            }
        }

        private void txtID_TextChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            myRS232.SomfyID = txtID.Text;
        }


        private void btn485_Click(object sender, EventArgs e)
        {
            FrmBoundRate frmtmp = new FrmBoundRate(SubNetID, DeviceID, myRS232, true);
            frmtmp.ShowDialog();
        }

        private void btnSave1_Click(object sender, EventArgs e)
        {
            tsbDown_Click(tbUpload, null);
        }

        private void btnSaveAndClose1_Click(object sender, EventArgs e)
        {
            btnSave1_Click(null, null);
            this.Close();
        }

        private void txtFrm_Leave(object sender, EventArgs e)
        {
            string str = txt232ToBusFrm.Text;
            int num = Convert.ToInt32(txt232ToBusTo.Text);
            if (HDLPF.IsRightNumStringMode(str, 1, num))
            {
                txt232ToBusFrm.SelectionStart = txt232ToBusFrm.Text.Length;
            }
            else
            {
                if (txt232ToBusFrm.Text == null || txt232ToBusFrm.Text == "")
                {
                    txt232ToBusFrm.Text = "1";
                }
                else
                {
                    txt232ToBusTo.Text = txt232ToBusFrm.Text;
                    txt232ToBusTo.Focus();
                    txt232ToBusTo.SelectionStart = txt232ToBusTo.Text.Length;
                }
            }
        }

        private void txt232ToBusTo_Leave(object sender, EventArgs e)
        {
            string str = txt232ToBusTo.Text;
            int num = Convert.ToInt32(txt232ToBusFrm.Text);
            if (mintDeviceType == 1016 || mintDeviceType == 1019 || mintDeviceType == 1014 || mintDeviceType == 1033 ||
                mintDeviceType == 1018 || mintDeviceType == 1027 || mintDeviceType == 1039)
            {
                txt232ToBusTo.Text = HDLPF.IsNumStringMode(str, num, 200);
            }
            else if (mintDeviceType == 1041)
            {
                txt232ToBusTo.Text = HDLPF.IsNumStringMode(str, num, 320);
            }
            else
            {
                txt232ToBusTo.Text = HDLPF.IsNumStringMode(str, num, 99);
            }
            txt232ToBusTo.SelectionStart = txt232ToBusTo.Text.Length;
        }

        private void btn232ToBusSure_Click(object sender, EventArgs e)
        {
            tsbDown_Click(tsbDown, null);
        }

        private void btnSave2_Click(object sender, EventArgs e)
        {
            tsbDown_Click(tbUpload,null);
        }

        private void btnRef2_Click(object sender, EventArgs e)
        {
            tsbDown_Click(tsbDown, null);
        }

        private void btnSaveAndClose2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void readRS232BUSTargets(int SelectIndex,int Type)
        {
            Cursor.Current = Cursors.WaitCursor;
            int CMD = 0xE414;
            if (Type == 1)
            {
                dgv232ToBusTarget.Rows.Clear();
                CMD = 0xE414;
            }
            else if (Type == 2)
            {
                dgv484BusTargets.Rows.Clear();
                CMD = 0xDA55;
            }
            for (int i = 1; i <= 6; i++)
            {
                byte[] ArayTmp = new byte[2];
                if (mintDeviceType == 1041)
                {
                    ArayTmp = new byte[3];
                    ArayTmp[0] = Convert.ToByte(SelectIndex / 256);
                    ArayTmp[1] = Convert.ToByte(SelectIndex % 256);
                    ArayTmp[2] = Convert.ToByte(i);
                }
                else
                {
                    ArayTmp[0] = Convert.ToByte(SelectIndex);
                    ArayTmp[1] = Convert.ToByte(i);
                }
                if (CsConst.mySends.AddBufToSndList(ArayTmp, CMD, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(mintDeviceType)) == true)
                {
                    string strType = "";
                    string strParam1 = "0", strParam2 = "0", strParam3 = "0", strParam4 = "0";
                    string strSubnetID = "0";
                    string strDeviceID = "0";
                    if (mintDeviceType == 1041)
                    {
                        strType = DryControlType.ConvertorKeyModeToPublicModeGroup(CsConst.myRevBuf[29]);
                        strSubnetID = CsConst.myRevBuf[30].ToString();
                        strDeviceID = CsConst.myRevBuf[31].ToString();
                        strParam1 = CsConst.myRevBuf[32].ToString();
                        strParam2 = CsConst.myRevBuf[33].ToString();
                        strParam3 = CsConst.myRevBuf[34].ToString();
                        strParam4 = CsConst.myRevBuf[35].ToString();
                    }
                    else
                    {
                        strType = DryControlType.ConvertorKeyModeToPublicModeGroup(CsConst.myRevBuf[28]);
                        strSubnetID = CsConst.myRevBuf[29].ToString();
                        strDeviceID = CsConst.myRevBuf[30].ToString();
                        strParam1 = CsConst.myRevBuf[31].ToString();
                        strParam2 = CsConst.myRevBuf[32].ToString();
                        strParam3 = CsConst.myRevBuf[33].ToString();
                        strParam4 = CsConst.myRevBuf[34].ToString();
                    }
                    SetParams(ref strType, ref strParam1, ref strParam2, ref strParam3, strParam4);
                    object[] obj = new object[] { i.ToString(),strSubnetID,strDeviceID,strType
                                ,strParam1,strParam2,strParam3};
                    if (Type == 1)
                    {
                        dgv232ToBusTarget.Rows.Add(obj);
                    }
                    else if (Type == 2)
                    {
                        dgv484BusTargets.Rows.Add(obj);
                    }
                    
                    System.Threading.Thread.Sleep(10);
                }
                else break;
            }
            Cursor.Current = Cursors.Default;
        }

        private void SetParams(ref string strType, ref string str1, ref string str2, ref string str3, string str4)
        {

            if (strType == CsConst.mstrINIDefault.IniReadValue("TargetType", "00000", ""))//无效
            {
                #region
                str1 = "N/A";
                str2 = "N/A";
                str3 = "N/A";
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
                #endregion
            }
            else if (strType == CsConst.myPublicControlType[2].ControlTypeName)//序列
            {
                #region
                str1 = str1 + "(" + CsConst.WholeTextsList[2510].sDisplayName + ")";
                str2 = str2 + "(" + CsConst.WholeTextsList[2512].sDisplayName+ ")";
                str3 = "N/A";
                #endregion
            }
            else if (strType == CsConst.myPublicControlType[4].ControlTypeName)//通用开关
            {
                #region
                str1 = str1 + "(" + CsConst.WholeTextsList[2513].sDisplayName + ")";
                if (str2 == "0") str2 = CsConst.Status[0] + "(" + CsConst.WholeTextsList[2529].sDisplayName + ")";
                else if (str2 == "255") str2 = CsConst.Status[1] + "(" + CsConst.WholeTextsList[2529].sDisplayName + ")";
                str3 = "N/A";
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
                #endregion
            }
            else if (strType == CsConst.myPublicControlType[10].ControlTypeName)//广播场景
            {
                #region
                str1 = CsConst.WholeTextsList[2566].sDisplayName;
                str2 = str2 + "(" + CsConst.WholeTextsList[2511].sDisplayName + ")";
                str3 = "N/A";
                #endregion
            }
            else if (strType == CsConst.myPublicControlType[11].ControlTypeName)//广播回路
            {
                #region
                str1 = CsConst.WholeTextsList[2567].sDisplayName;
                str2 = str2 + "(" + CsConst.WholeTextsList[2524].sDisplayName + ")";
                int intTmp = Convert.ToInt32(str3) * 256 + Convert.ToInt32(str4);
                str3 = HDLPF.GetStringFromTime(intTmp, ":") + "(" + CsConst.WholeTextsList[2525].sDisplayName + ")";
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
                #endregion
            }
            else if (strType == CsConst.myPublicControlType[8].ControlTypeName)//面板控制
            {
                #region
                str1 = HDLSysPF.InquirePanelControTypeStringFromDB(Convert.ToInt32(str1));
                if (str1 == CsConst.myPublicControlType[0].ControlTypeName)//无效
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
                else if(str1 == CsConst.myPublicControlType[6].ControlTypeName ||
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
                else if( str1 == CsConst.PanelControl[23] ||
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
                #endregion
            }
            else if (strType == CsConst.myPublicControlType[12].ControlTypeName)//消防模块
            {
                #region
                str1 = str1 + "(" + CsConst.WholeTextsList[2510].sDisplayName + ")";
                int intTmp = Convert.ToInt32(str2);
                if (1 <= intTmp && intTmp <= 10) str2 = CsConst.mstrINIDefault.IniReadValue("public", "0008" + (intTmp - 1).ToString(), "");
                str3 = "N/A";
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

        private void dgv232ToBusTarget_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int MaxCount = 20;
            if (mintDeviceType == 1006 || mintDeviceType == 1007 || mintDeviceType == 1008 || mintDeviceType == 1009 ||
                mintDeviceType == 1013 || mintDeviceType == 1014 || mintDeviceType == 1027 || mintDeviceType == 1039 ||
                mintDeviceType == 1016 || mintDeviceType == 1018 || mintDeviceType == 1019 || mintDeviceType == 1033)
            {
                MaxCount = 20;
            }
            else MaxCount = 48;
            if (rb1.Checked)
            {
                MaxCount = 10;
            }
            Form form = null;
            bool isOpen = true;
            
        }

        void FrmRS232BUSTargets_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (tab232.SelectedTab.Name == "tabPage1")
            {
                if (dgv232ToBus.RowCount > 0)
                    dgv232ToBus_CellClick(dgv232ToBus, new DataGridViewCellEventArgs(0, dgv232ToBus.CurrentRow.Index));
            }
            else if (tab232.SelectedTab.Name == "tabPage3")
            {
                if (dgv485Bus.RowCount > 0)
                    dgv485Bus_CellClick(dgv232ToBus, new DataGridViewCellEventArgs(0, dgv485Bus.CurrentRow.Index));
            }
        }

        private void dgv232ToBus_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            int id = Convert.ToInt32(dgv232ToBus[0, e.RowIndex].Value.ToString());
            readRS232BUSTargets(id,1);
        }

        private void dgv232ToBus_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0) return;
            int MaxCount = 200;
            if (mintDeviceType == 1016 || mintDeviceType == 1019 || mintDeviceType == 1014 || mintDeviceType == 1033 ||
                mintDeviceType == 1018 || mintDeviceType == 1027 || mintDeviceType == 1039)
            {
                MaxCount = 200;
            }
            else
            {
                MaxCount = 99;
            }

            if (rb1.Checked)
            {
                MaxCount = 99;
            }
            if (mintDeviceType == 1041) MaxCount = 320;
            FrmRS232BUS frmTmp = new FrmRS232BUS(myDevName, myRS232, mintDeviceType, MaxCount, 1);
            frmTmp.FormClosed += frmTmp_FormClosed;
            frmTmp.ShowDialog();
        }

        void frmTmp_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (tab232.SelectedTab.Name == "tabPage1")
                DisplayRS232BUSInfo();
            else if (tab232.SelectedTab.Name == "tabPage3")
                DisplayRS485BUSInfo();
        }

        private void txtBusFrm_Leave(object sender, EventArgs e)
        {
            string str = txtBusFrm.Text;
            int num = Convert.ToInt32(txtBusTo.Text);
            if (HDLPF.IsRightNumStringMode(str, 1, num))
            {
                txtBusFrm.SelectionStart = txtBusFrm.Text.Length;
            }
            else
            {
                if (txtBusFrm.Text == null || txtBusFrm.Text == "")
                {
                    txtBusFrm.Text = "1";
                }
                else
                {
                    txtBusTo.Text = txtBusFrm.Text;
                    txtBusTo.Focus();
                    txtBusTo.SelectionStart = txtBusTo.Text.Length;
                }
            }
        }

        private void txtBusTo_Leave(object sender, EventArgs e)
        {
            string str = txtBusTo.Text;
            int num = Convert.ToInt32(txtBusFrm.Text);
            if (mintDeviceType == 1016 || mintDeviceType == 1019 || mintDeviceType == 1014 || mintDeviceType == 1033 ||
                mintDeviceType == 1018 || mintDeviceType == 1027 || mintDeviceType == 1039)
            {
                txtBusTo.Text = HDLPF.IsNumStringMode(str, num, 200);
            }
            else
            {
                txtBusTo.Text = HDLPF.IsNumStringMode(str, num, 99);
            }
            txtBusTo.SelectionStart = txtBusTo.Text.Length;
        }

        private void dgvBUSto232_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int id = Convert.ToInt32(dgvBUSto232[0, e.RowIndex].Value.ToString());
            readBusRS232Targets(id,1);
        }

        private void dgvBUSto232_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0) return;
            int MaxCount = 200;
            if (mintDeviceType == 1016 || mintDeviceType == 1019 || mintDeviceType == 1014 || mintDeviceType == 1033 ||
                mintDeviceType == 1018 || mintDeviceType == 1027 || mintDeviceType == 1039)
            {
                MaxCount = 200;
            }
            else
            {
                MaxCount = 99;
            }
            if (rb1.Checked)
            {
                MaxCount = 99;
            }
            FrmBusToRS232 frmTmp = new FrmBusToRS232(myDevName, myRS232, mintDeviceType, MaxCount,1);
            frmTmp.FormClosed += FrmBusToRS232_FormClosed;
            frmTmp.ShowDialog();
        }

        void FrmBusToRS232_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (tab232.SelectedTab.Name == "tabPage2")
                DisplayBUSRS232Info();
            else if (tab232.SelectedTab.Name == "tabPage4")
                DisplayBUSRS485Info();
        }

        private void dgvBusTarget_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int MaxCount = 20;
            if (mintDeviceType == 1006 || mintDeviceType == 1007 || mintDeviceType == 1008 || mintDeviceType == 1009 ||
                mintDeviceType == 1013 || mintDeviceType == 1014 || mintDeviceType == 1027 || mintDeviceType == 1039 ||
                mintDeviceType == 1016 || mintDeviceType == 1018 || mintDeviceType == 1019 || mintDeviceType == 1033)
            {
                MaxCount = 20;
            }
            else MaxCount = 48;
            if (rb1.Checked)
            {
                MaxCount = 10;
            }

            Byte[] arrTmpIndexList =new Byte[]{ (Byte)MaxCount,Convert.ToByte(dgvBUSto232[0,dgvBUSto232.CurrentRow.Index].Value.ToString()),1};
            frmCmdSetup frmTmp = new frmCmdSetup(myRS232, myDevName, mintDeviceType, arrTmpIndexList);
            frmTmp.FormClosed += FrmBusRS232Target_FormClosed;
            frmTmp.ShowDialog();
        }

        void FrmBusRS232Target_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (tab232.SelectedTab.Name == "tabPage2")
            {
                if (dgvBUSto232.RowCount > 0)
                    dgvBUSto232_CellClick(dgvBUSto232, new DataGridViewCellEventArgs(0, dgvBUSto232.CurrentRow.Index));
            }
            else if (tab232.SelectedTab.Name == "tabPage4")
            {
                if (dgvBus485.RowCount > 0)
                    dgvBus485_CellClick(dgvBus485, new DataGridViewCellEventArgs(0, dgvBus485.CurrentRow.Index));
            }
        }

        private void readBusRS232Targets(int SelectIndex,int Type)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                int CMD = 0xE424;
                if (Type == 1)
                {
                    dgvBusTarget.Rows.Clear();
                    CMD = 0xE424;
                }
                else if (Type == 2)
                {
                    dgvBus485Targets.Rows.Clear();
                    CMD = 0xDA65;
                }
                int wdMaxValue = int.Parse(CsConst.mstrINIDefault.IniReadValue("DeviceType" + mintDeviceType.ToString(), "MaxValue", "33"));
                for (int i = 1; i <= 6; i++)
                {

                    byte[] ArayTmp = new byte[2];
                    ArayTmp[0] = Convert.ToByte(SelectIndex);
                    ArayTmp[1] = Convert.ToByte(i);

                    if (CsConst.mySends.AddBufToSndList(ArayTmp, CMD, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(mintDeviceType)) == true)
                    {
                        string strTime = CsConst.WholeTextsList[1775].sDisplayName;
                        if (1 <= CsConst.myRevBuf[28] && CsConst.myRevBuf[28] <= 8) strTime = CsConst.strAryRS232Time[CsConst.myRevBuf[28] - 1];
                        string strType = CsConst.WholeTextsList[1775].sDisplayName;
                        string strCMD = "";
                        string str1 = "N/A";
                        string str2 = "N/A";
                        string str3 = "N/A";
                        string str4 = "N/A";
                        byte[] arayCMD = new byte[wdMaxValue + 1];
                        Array.Copy(CsConst.myRevBuf, 30, arayCMD, 0, arayCMD.Length);
                        if (mintDeviceType == 1041)
                        {
                            strCMD = "N/A";
                            if (arayCMD[0] == 1)
                            {
                                str1 = CsConst.mstrINIDefault.IniReadValue("public", "99551", "");
                                str2 = arayCMD[1].ToString();
                                str3 = arayCMD[2].ToString();
                                if (arayCMD[3] == 3)
                                    str4 = CsConst.mstrINIDefault.IniReadValue("public", "99548", "");
                                else if (arayCMD[3] == 4)
                                    str4 = CsConst.mstrINIDefault.IniReadValue("public", "99549", "");
                                else if (arayCMD[3] == 5)
                                    str4 = CsConst.mstrINIDefault.IniReadValue("public", "99550", "");
                            }
                            else if (arayCMD[0] == 2)
                            {
                                str1 = CsConst.mstrINIDefault.IniReadValue("public", "99552", "");
                                str2 = arayCMD[1].ToString();
                                str3 = "N/A";
                                if (arayCMD[3] == 1)
                                    str4 = CsConst.mstrINIDefault.IniReadValue("public", "00050", "");
                                else if (arayCMD[3] == 2)
                                    str4 = CsConst.mstrINIDefault.IniReadValue("public", "00051", "");
                            }
                        }
                        else
                        {
                            if (CsConst.myRevBuf[29] == 0)
                            {
                                strType = CsConst.mstrINIDefault.IniReadValue("public", "99838", "");
                                int Count = arayCMD[arayCMD.Length - 1];
                                if (Count > wdMaxValue) Count = wdMaxValue;
                                byte[] arayTmp = new byte[Count];
                                if (Count == 0) strCMD = "";
                                else
                                {
                                    Array.Copy(arayCMD, 0, arayTmp, 0, Count);
                                    strCMD = HDLPF.Byte2String(arayTmp);
                                }
                            }
                            else if (CsConst.myRevBuf[29] == 1)
                            {
                                strType = CsConst.mstrINIDefault.IniReadValue("public", "99839", "");
                                int Count = arayCMD[arayCMD.Length - 1];
                                if (Count > wdMaxValue) Count = wdMaxValue;
                                for (int j = 0; j < Count; j++)
                                {
                                    strCMD = strCMD + GlobalClass.AddLeftZero(arayCMD[j].ToString("X"), 2) + " ";
                                }
                            }
                        }
                        object[] obj = new object[] { i.ToString(), strTime, strType, strCMD, str1, str2, str3, str4 };
                        if (Type == 1)
                        {
                            dgvBusTarget.Rows.Add(obj);
                        }
                        else if (Type == 2)
                        {
                            dgvBus485Targets.Rows.Add(obj);
                        }

                        HDLUDP.TimeBetwnNext(1);
                    }
                    else break;
                }
            }
            catch
            {
            }
            Cursor.Current = Cursors.Default;
        }

        private void dgvFilter_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            dgvFilter.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void dgvFilter_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (myRS232 == null) return;
                if (myRS232.myFilter == null) return;
                if ((e.RowIndex == -1) || (e.ColumnIndex == -1)) return;
                if (dgvFilter.SelectedRows.Count == 0) return;
                if (isRead) return;
                if (dgvFilter[e.ColumnIndex, e.RowIndex].Value == null) dgvFilter[e.ColumnIndex, e.RowIndex].Value = "";
                for (int i = 0; i < dgvFilter.SelectedRows.Count; i++)
                {
                    string strTmp = "";
                    switch (e.ColumnIndex)
                    {
                        case 1:
                            if (dgvFilter[1, dgvFilter.SelectedRows[i].Index].Value.ToString().ToLower() == "true")
                                myRS232.myFilter[dgvFilter.SelectedRows[i].Index][1] = 1;
                            else
                                myRS232.myFilter[dgvFilter.SelectedRows[i].Index][1] = 0;
                            break;
                        case 2:
                            strTmp = dgvFilter[2, dgvFilter.SelectedRows[i].Index].Value.ToString();
                            myRS232.myFilter[dgvFilter.SelectedRows[i].Index][2] = byte.Parse(HDLPF.IsNumStringMode(strTmp, 0, 255));
                            break;
                        case 3:
                            strTmp = dgvFilter[3, dgvFilter.SelectedRows[i].Index].Value.ToString();
                            myRS232.myFilter[dgvFilter.SelectedRows[i].Index][3] = byte.Parse(HDLPF.IsNumStringMode(strTmp, 0, 255));
                            break;
                    }
                    dgvFilter.SelectedRows[i].Cells[e.ColumnIndex].Value = dgvFilter[e.ColumnIndex, e.RowIndex].Value.ToString();
                }
                if (e.ColumnIndex == 1)
                {
                    if (dgvFilter[1, e.RowIndex].Value.ToString().ToLower() == "true")
                        myRS232.myFilter[e.RowIndex][1] = 1;
                    else
                        myRS232.myFilter[e.RowIndex][1] = 0;
                }
                if (e.ColumnIndex == 2)
                {
                    string strTmp = dgvFilter[2, e.RowIndex].Value.ToString();
                    myRS232.myFilter[e.RowIndex][2] = byte.Parse(HDLPF.IsNumStringMode(strTmp, 0, 255));
                }
                if (e.ColumnIndex == 3)
                {
                    string strTmp = dgvFilter[3, e.RowIndex].Value.ToString();
                    myRS232.myFilter[e.RowIndex][3] = byte.Parse(HDLPF.IsNumStringMode(strTmp, 0, 255));
                }
            }
            catch
            {
            }
        }

        private void btnReadMax_Click(object sender, EventArgs e)
        {
            if (cbType.SelectedIndex < 0)
                cbType.SelectedIndex = 0;
            Cursor.Current = Cursors.WaitCursor;
            int num1 = Convert.ToInt32(txtFrmMax.Text);
            int num2 = Convert.ToInt32(txtToMax.Text);
            myRS232.myComMax = new List<byte[]>();
            for (int i = num1; i <= num2; i++)
            {
                byte[] arayTmp = new byte[2];
                arayTmp[0] = Convert.ToByte(cbType.SelectedIndex + 1);
                arayTmp[1] = Convert.ToByte(i);
                if (CsConst.mySends.AddBufToSndList(arayTmp, 0x1CC2, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(mintDeviceType)) == true)
                {
                    byte[] arayData = new byte[7];
                    Array.Copy(CsConst.myRevBuf, 25, arayData, 0, 7);
                    myRS232.myComMax.Add(arayData);
                    
                    System.Threading.Thread.Sleep(1);
                }
                else
                {
                    Cursor.Current = Cursors.Default;
                    return;
                }
            }
            ShowComMaxInfo();
            Cursor.Current = Cursors.Default;
        }

        private void ShowComMaxInfo()
        {
            try
            {
                dgvCom.Rows.Clear();
                if (myRS232 == null) return;
                if (myRS232.myComMax == null) return;
                if (cbType.SelectedIndex == 0) clM6.Visible = false;
                else if (cbType.SelectedIndex == 1) clM6.Visible = false;
                else if (cbType.SelectedIndex == 2) clM6.Visible = true;
                for (int i = 0; i < myRS232.myComMax.Count; i++)
                {
                    byte[] arayTmp = myRS232.myComMax[i];
                    string strEnable = clM2.Items[0].ToString();
                    if (arayTmp[2] == 1) strEnable = clM2.Items[1].ToString();
                    string strType = clM6.Items[0].ToString();
                    if (arayTmp[6] == 1) strType = clM6.Items[1].ToString();
                    object[] obj = new object[] { dgvCom.RowCount + 1, strEnable, arayTmp[3].ToString(), 
                        arayTmp[4].ToString(), arayTmp[5].ToString(),strType };
                    dgvCom.Rows.Add(obj);
                }
            }
            catch
            {
            }
        }

        private void cbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            if (cbType.SelectedIndex < 0) cbType.SelectedIndex = 0;
            if (cbType.SelectedIndex == 0)
            {
                txtToMax.Text = "4";
                txtFrmMax.Text = "1";
                lbNumMax.Text = lbNumMax.Text.Split('-')[0].ToString() + "-4)";
                clM6.Visible = false;
            }
            else if (cbType.SelectedIndex == 1)
            {
                txtToMax.Text = "4";
                txtFrmMax.Text = "1";
                lbNumMax.Text = lbNumMax.Text.Split('-')[0].ToString() + "-4)";
                clM6.Visible = false;
            }
            else if (cbType.SelectedIndex == 2)
            {
                txtToMax.Text = "6";
                txtFrmMax.Text = "1";
                lbNumMax.Text = lbNumMax.Text.Split('-')[0].ToString() + "-40)";
                clM6.Visible = true;
            }
            btnReadMax_Click(null, null);
        }

        private void txtFrmMax_Leave(object sender, EventArgs e)
        {
            string str = txtFrmMax.Text;
            int num = Convert.ToInt32(txtToMax.Text);
            if (HDLPF.IsRightNumStringMode(str, 1, num))
            {
                txtFrmMax.SelectionStart = txtFrmMax.Text.Length;
            }
            else
            {
                if (txtFrmMax.Text == null || txtFrmMax.Text == "")
                {
                    txtFrmMax.Text = "1";
                }
                else
                {
                    txtToMax.Text = txtFrmMax.Text;
                    txtToMax.Focus();
                    txtToMax.SelectionStart = txtToMax.Text.Length;
                }
            }
        }

        private void txtToMax_Leave(object sender, EventArgs e)
        {
            string str = txtToMax.Text;
            int num = Convert.ToInt32(txtFrmMax.Text);
            if (cbType.SelectedIndex==0)
            {
                txtToMax.Text = HDLPF.IsNumStringMode(str, num, 4);
            }
            else if (cbType.SelectedIndex==1)
            {
                txtToMax.Text = HDLPF.IsNumStringMode(str, num, 4);
            }
            else if (cbType.SelectedIndex == 2)
            {
                txtToMax.Text = HDLPF.IsNumStringMode(str, num, 40);
            }
            txtToMax.SelectionStart = txtToMax.Text.Length;
        }

        private void dgvCom_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            dgvCom.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void btnSave7_Click(object sender, EventArgs e)
        {
            myRS232.myComMax = new List<byte[]>();
            for (int i = 0; i < dgvCom.RowCount; i++)
            {
                byte[] arayTmp = new byte[7];
                arayTmp[0] = Convert.ToByte(cbType.SelectedIndex + 1);
                arayTmp[1] = Convert.ToByte(dgvCom[0, i].Value.ToString());
                arayTmp[2] = Convert.ToByte(clM2.Items.IndexOf(dgvCom[1, i].Value.ToString()));
                arayTmp[3] = Convert.ToByte(HDLPF.IsNumStringMode(dgvCom[2, i].Value.ToString(), 0, 255));
                arayTmp[4] = Convert.ToByte(HDLPF.IsNumStringMode(dgvCom[3, i].Value.ToString(), 0, 255));
                arayTmp[5] = Convert.ToByte(HDLPF.IsNumStringMode(dgvCom[4, i].Value.ToString(), 0, 255));
                arayTmp[6] = Convert.ToByte(clM6.Items.IndexOf(dgvCom[5, i].Value.ToString()));
                myRS232.myComMax.Add(arayTmp);
            }
            tsbDown_Click(tbUpload, null);
        }

        private void btnSaveAndClose7_Click(object sender, EventArgs e)
        {
            btnSave7_Click(null, null);
            this.Close();
        }

        private void dgvCom_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            for (int i = 0; i < dgvCom.SelectedRows.Count; i++)
            {
                dgvCom.SelectedRows[i].Cells[e.ColumnIndex].Value = dgvCom[e.ColumnIndex, e.RowIndex].Value.ToString();
            }
        }

        private void btn485Bus_Click(object sender, EventArgs e)
        {
            tsbDown_Click(tsbDown, null);
        }

        private void btnBus485_Click(object sender, EventArgs e)
        {
            tsbDown_Click(tsbDown, null);
        }

        private void dgv485Bus_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            int id = Convert.ToInt32(dgv485Bus[0, e.RowIndex].Value.ToString());
            readRS232BUSTargets(id, 2);
        }

        private void dgvBus485_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            int id = Convert.ToInt32(dgvBus485[0, e.RowIndex].Value.ToString());
            readBusRS232Targets(id, 2);
        }

        private void dgv485Bus_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0) return;
            int MaxCount = 200;
            if (mintDeviceType == 1016 || mintDeviceType == 1019 || mintDeviceType == 1014 || mintDeviceType == 1033 ||
                mintDeviceType == 1018 || mintDeviceType == 1027 || mintDeviceType == 1039)
            {
                MaxCount = 200;
            }
            else
            {
                MaxCount = 99;
            }
            if (rb1.Checked)
            {
                MaxCount = 99;
            }
            FrmRS232BUS frmTmp = new FrmRS232BUS(myDevName, myRS232, mintDeviceType, MaxCount, 2);
            frmTmp.FormClosed += frmTmp_FormClosed;
            frmTmp.ShowDialog();
        }

        private void dgv484BusTargets_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int MaxCount = 20;
            if (mintDeviceType == 1006 || mintDeviceType == 1007 || mintDeviceType == 1008 || mintDeviceType == 1009 ||
                mintDeviceType == 1013 || mintDeviceType == 1014 || mintDeviceType == 1027 || mintDeviceType == 1039 ||
                mintDeviceType == 1016 || mintDeviceType == 1018 || mintDeviceType == 1019 || mintDeviceType == 1033)
            {
                MaxCount = 20;
            }
            else MaxCount = 48;
            if (rb1.Checked)
            {
                MaxCount = 10;
            }
            Form form = null;
            bool isOpen = true;
            
        }

        private void dgvBus485_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0) return;
            int MaxCount = 200;
            if (mintDeviceType == 1016 || mintDeviceType == 1019 || mintDeviceType == 1014 || mintDeviceType == 1033 ||
                mintDeviceType == 1018 || mintDeviceType == 1027 || mintDeviceType == 1039)
            {
                MaxCount = 200;
            }
            else
            {
                MaxCount = 99;
            }
            if (rb1.Checked)
            {
                MaxCount = 99;
            }
            FrmBusToRS232 frmTmp = new FrmBusToRS232(myDevName, myRS232, mintDeviceType, MaxCount, 2);
            frmTmp.FormClosed += FrmBusToRS232_FormClosed;
            frmTmp.ShowDialog();
        }

        private void dgvBus485Targets_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int MaxCount = 20;
            if (mintDeviceType == 1006 || mintDeviceType == 1007 || mintDeviceType == 1008 || mintDeviceType == 1009 ||
                mintDeviceType == 1013 || mintDeviceType == 1014 || mintDeviceType == 1027 || mintDeviceType == 1039 ||
                mintDeviceType == 1016 || mintDeviceType == 1018 || mintDeviceType == 1019 || mintDeviceType == 1033)
            {
                MaxCount = 20;
            }
            else MaxCount = 48;
            if (rb1.Checked)
            {
                MaxCount = 10;
            }
            Byte[] arrIndexList = new Byte[]{(Byte)MaxCount,Convert.ToByte(dgvBus485[0, dgvBus485.CurrentRow.Index].Value.ToString()), 2};
            frmCmdSetup frmTmp = new frmCmdSetup(myRS232, myDevName, mintDeviceType, arrIndexList);
            frmTmp.FormClosed += FrmBusRS232Target_FormClosed;
            frmTmp.ShowDialog();
        }

        private void rb1_CheckedChanged(object sender, EventArgs e)
        {
            if (rb1.Checked)
            {
                chb1.Enabled = true;
                chb2.Enabled = true;
            }
            else if (rb2.Checked)
            {
                chb1.Enabled = true;
                chb2.Enabled = false;
                chb2.Checked = false;
            }
            else if (rb3.Checked)
            {
                chb1.Enabled = false;
                chb1.Checked = false;
                chb2.Enabled = true;
            }
            if (isRead) return;
            if (myRS232 == null) return;
            if (myRS232.ArayExtra == null) return;
            if (rb1.Checked) myRS232.ArayExtra[0] = 0;
            else if (rb2.Checked) myRS232.ArayExtra[0] = 1;
            else if (rb3.Checked) myRS232.ArayExtra[0] = 2;
            if (chb1.Checked) myRS232.ArayExtra[1] = 1;
            else myRS232.ArayExtra[1] = 0;
            if (chb2.Checked) myRS232.ArayExtra[2] = 1;
            else myRS232.ArayExtra[2] = 0;
        }

        private void btnSaveACTime_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            numTime1_ValueChanged(null, null);
            numTime2_ValueChanged(null, null);
            byte[] ArayTmp = new byte[1];
            myRS232.Time = Convert.ToByte(Convert.ToInt32(numTime1.Value) * 10 + Convert.ToInt32(numTime2.Value));
            if (mintDeviceType == 1041)
            {
                if (myRS232.Time < 50) myRS232.Time = 50;
            }
            ArayTmp[0] = myRS232.Time;
            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x310E, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(mintDeviceType)) == true)
            {
                
                HDLUDP.TimeBetwnNext(20);
            }
            Cursor.Current = Cursors.Default;
        }

        private void btnSaveACCount_Click(object sender, EventArgs e)
        {
            byte[] ArayTmp = new byte[1];
            myRS232.Count = Convert.ToByte(txtCount.Text);
            ArayTmp[0] = myRS232.Count;
            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x3102, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(mintDeviceType)) == true)
            {
                
                HDLUDP.TimeBetwnNext(20);
            }
            else
            {
                Cursor.Current = Cursors.Default;
                return;
            }
            tsbDown_Click(tsbDown, null);
        }

        private void numTime1_ValueChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt32(numTime1.Value) == 5) numTime2.Value = 0;
            if (isRead) return;
            if (myRS232 == null) return;
            myRS232.Time = Convert.ToByte(Convert.ToInt32(numTime1.Value) * 10 + Convert.ToInt32(numTime2.Value));
        }

        private void numTime2_ValueChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            if (myRS232 == null) return;
            myRS232.Time = Convert.ToByte(Convert.ToInt32(numTime1.Value) * 10 + Convert.ToInt32(numTime2.Value));
        }

        private void btnRef8_Click(object sender, EventArgs e)
        {
            tsbDown_Click(tsbDown, null);
        }

        private void btnSave8_Click(object sender, EventArgs e)
        {
            tsbDown_Click(tbUpload, null);
        }

        private void btnSaveAndClose8_Click(object sender, EventArgs e)
        {
            tsbDown_Click(tbUpload, null);
            this.Close();
        }

        private void dgvAC_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (myRS232 == null) return;
                if (myRS232.myAC == null) return;
                if ((e.RowIndex == -1) || (e.ColumnIndex == -1)) return;
                if (dgvAC.SelectedRows.Count == 0) return;
                if (isRead) return;
                if (dgvAC[e.ColumnIndex, e.RowIndex].Value == null) dgvAC[e.ColumnIndex, e.RowIndex].Value = "";
                for (int i = 0; i < dgvAC.SelectedRows.Count; i++)
                {
                    string strTmp = "";
                    int intTmp = 0;
                    switch (e.ColumnIndex)
                    {
                        case 1:
                            for (int j = 0; j < 10; j++)
                            {
                                if (mintDeviceType == 1034 || mintDeviceType == 1004)
                                    myRS232.myAC[dgvAC.SelectedRows[i].Index][4 + j] = 0;
                                else
                                    myRS232.myAC[dgvAC.SelectedRows[i].Index][6 + j] = 0;
                            }
                            strTmp = dgvAC[1, dgvAC.SelectedRows[i].Index].Value.ToString();
                            byte[] arayTmpRemark = HDLUDP.StringToByte(strTmp.Trim());
                            if (arayTmpRemark.Length > 10)
                            {
                                if (mintDeviceType == 1034 || mintDeviceType == 1004)
                                    Array.Copy(arayTmpRemark, 0, myRS232.myAC[dgvAC.SelectedRows[i].Index], 4, 10);
                                else if (mintDeviceType == 1040)
                                    Array.Copy(arayTmpRemark, 0, myRS232.myAC[dgvAC.SelectedRows[i].Index], 9, 10);
                                else if (mintDeviceType == 1041)
                                    Array.Copy(arayTmpRemark, 0, myRS232.myAC[dgvAC.SelectedRows[i].Index], 7, 10);
                                else
                                    Array.Copy(arayTmpRemark, 0, myRS232.myAC[dgvAC.SelectedRows[i].Index], 6, 10);
                            }
                            else
                            {
                                if (mintDeviceType == 1034)
                                    Array.Copy(arayTmpRemark, 0, myRS232.myAC[dgvAC.SelectedRows[i].Index], 4, arayTmpRemark.Length);
                                else if (mintDeviceType == 1040)
                                    Array.Copy(arayTmpRemark, 0, myRS232.myAC[dgvAC.SelectedRows[i].Index], 9, arayTmpRemark.Length);
                                else if (mintDeviceType == 1041)
                                    Array.Copy(arayTmpRemark, 0, myRS232.myAC[dgvAC.SelectedRows[i].Index], 7, arayTmpRemark.Length);
                                else
                                    Array.Copy(arayTmpRemark, 0, myRS232.myAC[dgvAC.SelectedRows[i].Index], 6, arayTmpRemark.Length);
                            }
                            break;
                        case 2:
                            strTmp = dgvAC[2, dgvAC.SelectedRows[i].Index].Value.ToString();
                            if (mintDeviceType == 1040)
                                myRS232.myAC[dgvAC.SelectedRows[i].Index][2] = byte.Parse(HDLPF.IsNumStringMode(strTmp, 0, 255));
                            else if (mintDeviceType == 1041)
                                myRS232.myAC[dgvAC.SelectedRows[i].Index][2] = byte.Parse(HDLPF.IsNumStringMode(strTmp, 0, 255));
                            else
                                myRS232.myAC[dgvAC.SelectedRows[i].Index][1] = byte.Parse(HDLPF.IsNumStringMode(strTmp, 0, 255));
                            break;
                        case 3:
                            strTmp = dgvAC[3, dgvAC.SelectedRows[i].Index].Value.ToString();
                            if (mintDeviceType == 1034)
                                strTmp = HDLPF.IsNumStringMode(strTmp, 0, 255);
                            else if (mintDeviceType == 1040)
                                strTmp = HDLPF.IsNumStringMode(strTmp, 0, 65535);
                            else if (mintDeviceType == 1041)
                                strTmp = HDLPF.IsNumStringMode(strTmp, 0, 255);
                            else
                                strTmp = HDLPF.IsNumStringMode(strTmp, 0, 65535);
                            intTmp = Convert.ToInt32(strTmp);
                            if (mintDeviceType == 1034)
                                myRS232.myAC[dgvAC.SelectedRows[i].Index][2] = Convert.ToByte(intTmp);
                            else if (mintDeviceType == 1041)
                                myRS232.myAC[dgvAC.SelectedRows[i].Index][5] = Convert.ToByte(intTmp);
                            else if (mintDeviceType == 1040)
                            {
                                myRS232.myAC[dgvAC.SelectedRows[i].Index][5] = Convert.ToByte(intTmp / 256);
                                myRS232.myAC[dgvAC.SelectedRows[i].Index][6] = Convert.ToByte(intTmp % 256);
                            }
                            else
                            {
                                myRS232.myAC[dgvAC.SelectedRows[i].Index][2] = Convert.ToByte(intTmp / 256);
                                myRS232.myAC[dgvAC.SelectedRows[i].Index][3] = Convert.ToByte(intTmp % 256);
                            }
                            break;
                        case 4:
                            strTmp = dgvAC[4, dgvAC.SelectedRows[i].Index].Value.ToString();
                            if (mintDeviceType == 1034)
                                strTmp = HDLPF.IsNumStringMode(strTmp, 0, 255);
                            else if (mintDeviceType == 1040)
                                strTmp = HDLPF.IsNumStringMode(strTmp, 0, 65535);
                            else if (mintDeviceType == 1041)
                                strTmp = HDLPF.IsNumStringMode(strTmp, 0, 255);
                            else
                                strTmp = HDLPF.IsNumStringMode(strTmp, 0, 65535);
                            intTmp = Convert.ToInt32(strTmp);
                            if (mintDeviceType == 1034)
                                myRS232.myAC[dgvAC.SelectedRows[i].Index][3] = Convert.ToByte(intTmp);
                            else if (mintDeviceType == 1040)
                            {
                                myRS232.myAC[dgvAC.SelectedRows[i].Index][7] = Convert.ToByte(intTmp / 256);
                                myRS232.myAC[dgvAC.SelectedRows[i].Index][8] = Convert.ToByte(intTmp % 256);
                            }
                            else if (mintDeviceType == 1041)
                                myRS232.myAC[dgvAC.SelectedRows[i].Index][6] = Convert.ToByte(intTmp);
                            else
                            {
                                myRS232.myAC[dgvAC.SelectedRows[i].Index][4] = Convert.ToByte(intTmp / 256);
                                myRS232.myAC[dgvAC.SelectedRows[i].Index][5] = Convert.ToByte(intTmp % 256);
                            }
                            break;
                        case 5:
                            strTmp = dgvAC[5, dgvAC.SelectedRows[i].Index].Value.ToString();
                            myRS232.myAC[dgvAC.SelectedRows[i].Index][1] = Convert.ToByte(clAC6.Items.IndexOf(strTmp) + 1);
                            break;
                        case 6:
                            strTmp = dgvAC[6, dgvAC.SelectedRows[i].Index].Value.ToString();
                            strTmp = HDLPF.IsNumStringMode(strTmp, 0, 255);
                            myRS232.myAC[dgvAC.SelectedRows[i].Index][3] = Convert.ToByte(strTmp);
                            break;
                        case 7:
                            strTmp = dgvAC[7, dgvAC.SelectedRows[i].Index].Value.ToString();
                            strTmp = HDLPF.IsNumStringMode(strTmp, 0, 255);
                            myRS232.myAC[dgvAC.SelectedRows[i].Index][4] = Convert.ToByte(strTmp);
                            break;
                    }
                    dgvAC.SelectedRows[i].Cells[e.ColumnIndex].Value = dgvAC[e.ColumnIndex, e.RowIndex].Value.ToString();
                }
                if (e.ColumnIndex == 1)
                {
                    for (int j = 0; j < 10; j++)
                    {
                        if (mintDeviceType == 1034)
                            myRS232.myAC[e.RowIndex][4 + j] = 0;
                        else
                            myRS232.myAC[e.RowIndex][6 + j] = 0;
                    }
                    string strTmp = dgvAC[1, e.RowIndex].Value.ToString();
                    byte[] arayTmpRemark = HDLUDP.StringToByte(strTmp.Trim());
                    if (arayTmpRemark.Length > 10)
                    {
                        if (mintDeviceType == 1034)
                            Array.Copy(arayTmpRemark, 0, myRS232.myAC[e.RowIndex], 4, 10);
                        else if (mintDeviceType == 1040)
                            Array.Copy(arayTmpRemark, 0, myRS232.myAC[e.RowIndex], 9, 10);
                        else if (mintDeviceType == 1041)
                            Array.Copy(arayTmpRemark, 0, myRS232.myAC[e.RowIndex], 7, 10);
                        else
                            Array.Copy(arayTmpRemark, 0, myRS232.myAC[e.RowIndex], 6, 10);
                    }
                    else
                    {
                        if (mintDeviceType == 1034)
                            Array.Copy(arayTmpRemark, 0, myRS232.myAC[e.RowIndex], 4, arayTmpRemark.Length);
                        else if (mintDeviceType == 1040)
                            Array.Copy(arayTmpRemark, 0, myRS232.myAC[e.RowIndex], 9, arayTmpRemark.Length);
                        else if (mintDeviceType == 1041)
                            Array.Copy(arayTmpRemark, 0, myRS232.myAC[e.RowIndex], 7, arayTmpRemark.Length);
                        else
                            Array.Copy(arayTmpRemark, 0, myRS232.myAC[e.RowIndex], 6, arayTmpRemark.Length);
                    }
                }
                if (e.ColumnIndex == 2)
                {
                    string strTmp = dgvAC[2, e.RowIndex].Value.ToString();
                    if (mintDeviceType == 1040)
                        myRS232.myAC[e.RowIndex][2] = byte.Parse(HDLPF.IsNumStringMode(strTmp, 0, 255));
                    else if (mintDeviceType == 1041)
                        myRS232.myAC[e.RowIndex][2] = byte.Parse(HDLPF.IsNumStringMode(strTmp, 0, 255));
                    else
                        myRS232.myAC[e.RowIndex][1] = byte.Parse(HDLPF.IsNumStringMode(strTmp, 0, 255));
                }
                if (e.ColumnIndex == 3)
                {
                    string strTmp = dgvAC[3, e.RowIndex].Value.ToString();
                    if (mintDeviceType == 1034)
                        strTmp = HDLPF.IsNumStringMode(strTmp, 0, 255);
                    else if (mintDeviceType == 1040)
                        strTmp = HDLPF.IsNumStringMode(strTmp, 0, 65535);
                    else if (mintDeviceType == 1041)
                        strTmp = HDLPF.IsNumStringMode(strTmp, 0, 255);
                    else
                        strTmp = HDLPF.IsNumStringMode(strTmp, 0, 65535);
                    int intTmp = Convert.ToInt32(strTmp);
                    if (mintDeviceType == 1034)
                        myRS232.myAC[e.RowIndex][2] = Convert.ToByte(intTmp);
                    else if (mintDeviceType == 1041)
                        myRS232.myAC[e.RowIndex][5] = Convert.ToByte(intTmp);
                    else if (mintDeviceType == 1040)
                    {
                        myRS232.myAC[e.RowIndex][5] = Convert.ToByte(intTmp / 256);
                        myRS232.myAC[e.RowIndex][6] = Convert.ToByte(intTmp % 256);
                    }
                    else
                    {
                        myRS232.myAC[e.RowIndex][2] = Convert.ToByte(intTmp / 256);
                        myRS232.myAC[e.RowIndex][3] = Convert.ToByte(intTmp % 256);
                    }
                }
                if (e.ColumnIndex == 4)
                {
                    string strTmp = dgvAC[4, e.RowIndex].Value.ToString();
                    if (mintDeviceType == 1034)
                        strTmp = HDLPF.IsNumStringMode(strTmp, 0, 255);
                    else if (mintDeviceType == 1040)
                        strTmp = HDLPF.IsNumStringMode(strTmp, 0, 65535);
                    else if (mintDeviceType == 1041)
                        strTmp = HDLPF.IsNumStringMode(strTmp, 0, 255);
                    else
                        strTmp = HDLPF.IsNumStringMode(strTmp, 0, 65535);
                    int intTmp = Convert.ToInt32(strTmp);
                    if (mintDeviceType == 1034)
                        myRS232.myAC[e.RowIndex][3] = Convert.ToByte(intTmp);
                    else if (mintDeviceType == 1040)
                    {
                        myRS232.myAC[e.RowIndex][7] = Convert.ToByte(intTmp / 256);
                        myRS232.myAC[e.RowIndex][8] = Convert.ToByte(intTmp % 256);
                    }
                    else if (mintDeviceType == 1041)
                        myRS232.myAC[e.RowIndex][6] = Convert.ToByte(intTmp);
                    else
                    {
                        myRS232.myAC[e.RowIndex][4] = Convert.ToByte(intTmp / 256);
                        myRS232.myAC[e.RowIndex][5] = Convert.ToByte(intTmp % 256);
                    }
                }
                else if (e.ColumnIndex == 5)
                {
                    string strTmp = dgvAC[5, e.RowIndex].Value.ToString();
                    myRS232.myAC[e.RowIndex][1] = Convert.ToByte(clAC6.Items.IndexOf(strTmp) + 1);
                }
                else if (e.ColumnIndex == 6)
                {
                    string strTmp = dgvAC[6, e.RowIndex].Value.ToString();
                    strTmp = HDLPF.IsNumStringMode(strTmp, 0, 255);
                    myRS232.myAC[e.RowIndex][3] = Convert.ToByte(strTmp);
                }
                else if (e.ColumnIndex == 7)
                {
                    string strTmp = dgvAC[7, e.RowIndex].Value.ToString();
                    strTmp = HDLPF.IsNumStringMode(strTmp, 0, 255);
                    myRS232.myAC[e.RowIndex][4] = Convert.ToByte(strTmp);
                }
            }
            catch
            {
            }
        }

        private void dgvAC_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            dgvAC.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void txtCount_TextChanged(object sender, EventArgs e)
        {
            if (txtCount.TextLength > 0)
            {
                int Count = 128;
                if (mintDeviceType == 1041) Count = 10;
                txtCount.Text = HDLPF.IsNumStringMode(txtCount.Text, 1, Count);
                txtCount.SelectionStart = txtCount.Text.Length;
            }
            if (isRead) return;
            if (myRS232 == null) return;
            myRS232.Count = Convert.ToByte(txtCount.Text);
        }

        private void btnOther2_Click(object sender, EventArgs e)
        {
            Form form = null;
            bool isOpen = true;
            foreach (Form frm in Application.OpenForms)
            {
                if (frm.Name == "FrmInfrared")
                {
                    if (((frm as FrmInfrared).DIndex == mintIDIndex))
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
                byte Index = (byte)(dgvAC.CurrentRow.Index + 1);
                FrmInfrared frmTmp = new FrmInfrared(myDevName, mintDeviceType, myRS232, Index);
                frmTmp.ShowDialog();
            }
        }

        private void dgvAC_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvAC.RowCount <= 0) return;
            if (dgvAC.CurrentRow.Index < 0) return;
            int Index = dgvAC.CurrentRow.Index;
            if (mintDeviceType == 1041)
            {
                
            }
            else
            {
                if (Index >= 5) btnOther2.Enabled = false;
                else btnOther2.Enabled = true;
            }
        }

        private void txtFrm485Bus_Leave(object sender, EventArgs e)
        {
            string str = txtFrm485Bus.Text;
            int num = Convert.ToInt32(txtTo485Bus.Text);
            if (HDLPF.IsRightNumStringMode(str, 1, num))
            {
                txtFrm485Bus.SelectionStart = txtFrm485Bus.Text.Length;
            }
            else
            {
                if (txtFrm485Bus.Text == null || txtFrm485Bus.Text == "")
                {
                    txtFrm485Bus.Text = "1";
                }
                else
                {
                    txtTo485Bus.Text = txtFrm485Bus.Text;
                    txtTo485Bus.Focus();
                    txtTo485Bus.SelectionStart = txtTo485Bus.Text.Length;
                }
            }
        }

        private void txtTo485Bus_Leave(object sender, EventArgs e)
        {
            string str = txtTo485Bus.Text;
            int num = Convert.ToInt32(txtFrm485Bus.Text);
            txtTo485Bus.Text = HDLPF.IsNumStringMode(str, num, 99);
            txtTo485Bus.SelectionStart = txtTo485Bus.Text.Length;
        }

        private void txtFrmBus485_Leave(object sender, EventArgs e)
        {
            string str = txtFrmBus485.Text;
            int num = Convert.ToInt32(txtToBus485.Text);
            if (HDLPF.IsRightNumStringMode(str, 1, num))
            {
                txtFrmBus485.SelectionStart = txtFrmBus485.Text.Length;
            }
            else
            {
                if (txtFrmBus485.Text == null || txtFrmBus485.Text == "")
                {
                    txtFrmBus485.Text = "1";
                }
                else
                {
                    txtToBus485.Text = txtFrmBus485.Text;
                    txtToBus485.Focus();
                    txtToBus485.SelectionStart = txtToBus485.Text.Length;
                }
            }
        }

        private void txtToBus485_Leave(object sender, EventArgs e)
        {
            string str = txtToBus485.Text;
            int num = Convert.ToInt32(txtFrmBus485.Text);
            txtToBus485.Text = HDLPF.IsNumStringMode(str, num, 99);
            txtToBus485.SelectionStart = txtToBus485.Text.Length;
        }

        private void txtC1_Leave(object sender, EventArgs e)
        {
            string str = txtC1.Text;
            int num = Convert.ToInt32(txtC2.Text);
            if (HDLPF.IsRightNumStringMode(str, 1, num))
            {
                txtC1.SelectionStart = txtC1.Text.Length;
            }
            else
            {
                if (txtC1.Text == null || txtC1.Text == "")
                {
                    txtC1.Text = "1";
                }
                else
                {
                    txtC2.Text = txtC1.Text;
                    txtC2.Focus();
                    txtC2.SelectionStart = txtC2.Text.Length;
                }
            }
        }

        private void txtC2_Leave(object sender, EventArgs e)
        {
            string str = txtC2.Text;
            int num = Convert.ToInt32(txtC1.Text);
            txtC2.Text = HDLPF.IsNumStringMode(str, num, 50);
            txtC2.SelectionStart = txtC2.Text.Length;
        }

        private void dgvCurtain_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (isRead) return;
                if (myRS232 == null) return;
                if (myRS232.myCurTain == null) return;
                if ((e.RowIndex == -1) || (e.ColumnIndex == -1)) return;
                if (dgvCurtain.SelectedRows.Count == 0) return;
                if (dgvCurtain[e.ColumnIndex, e.RowIndex].Value == null) dgvCurtain[e.ColumnIndex, e.RowIndex].Value = "";
                for (int i = 0; i < dgvCurtain.SelectedRows.Count; i++)
                {
                    string strTmp = "";
                    int intTmp = 0;
                    switch (e.ColumnIndex)
                    {
                        case 1:
                            strTmp = dgvCurtain[1, dgvCurtain.SelectedRows[i].Index].Value.ToString();
                            dgvCurtain[1, dgvCurtain.SelectedRows[i].Index].Value = HDLPF.IsNumStringMode(strTmp, 0, 255);
                            myRS232.myCurTain[dgvCurtain.SelectedRows[i].Index][1] = Convert.ToByte(HDLPF.IsNumStringMode(strTmp, 0, 255));
                            break;
                        case 2:
                            strTmp = dgvCurtain[2, dgvCurtain.SelectedRows[i].Index].Value.ToString();
                            dgvCurtain[2, dgvCurtain.SelectedRows[i].Index].Value = HDLPF.IsNumStringMode(strTmp, 0, 255);
                            myRS232.myCurTain[dgvCurtain.SelectedRows[i].Index][2] = Convert.ToByte(HDLPF.IsNumStringMode(strTmp, 0, 255));
                            break;
                    }
                    dgvCurtain.SelectedRows[i].Cells[e.ColumnIndex].Value = dgvCurtain[e.ColumnIndex, e.RowIndex].Value.ToString();
                }
                if (e.ColumnIndex == 1)
                {
                    string strTmp = dgvCurtain[1, e.RowIndex].Value.ToString();
                    myRS232.myCurTain[e.RowIndex][1] = Convert.ToByte(HDLPF.IsNumStringMode(strTmp, 0, 255));
                }
                else if (e.ColumnIndex == 2)
                {
                    string strTmp = dgvCurtain[2, e.RowIndex].Value.ToString();
                    myRS232.myCurTain[e.RowIndex][2] = Convert.ToByte(HDLPF.IsNumStringMode(strTmp, 0, 255));
                }
            }
            catch
            {
            }
        }

        private void dgvCurtain_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            dgvCurtain.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void NumSubID_Click(object sender, EventArgs e)
        {

        }

        private void btnFhCount_Click(object sender, EventArgs e)
        {
            byte[] ArayTmp = new byte[1];
            myRS232.Count = Convert.ToByte(tbFhCount.Text);
            ArayTmp[0] = myRS232.Count;
            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x3102, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(mintDeviceType)) == true)
            {
                HDLUDP.TimeBetwnNext(20);
            }
            else
            {
                Cursor.Current = Cursors.Default;
                return;
            }
            tsbDown_Click(tsbDown, null);
        }

        private void btnFhSave_Click(object sender, EventArgs e)
        {
            if (isRead) return;
            if (myRS232 == null) return;
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                myRS232.Time = Convert.ToByte(Convert.ToInt32(numTimeFh1.Value) * 10 + Convert.ToInt32(numTimeFh2.Value));
                if (myRS232.Time > 50) myRS232.Time = 50;
                if (myRS232.Time < 10) myRS232.Time = 10;
                Byte[] ArayTmp = new byte[1]{ myRS232.Time};
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x310E, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(mintDeviceType)) == true)
                {
                    HDLUDP.TimeBetwnNext(20);
                }
            }
            catch 
            { 
                Cursor.Current = Cursors.Default; 
            }
            Cursor.Current = Cursors.Default;
        }

        private void dgFh_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            dgFh.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void dgFh_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (myRS232 == null) return;
                if (myRS232.myAC == null) return;
                if ((e.RowIndex == -1) || (e.ColumnIndex == -1)) return;
                if (dgFh.SelectedRows.Count == 0) return;
                if (isRead) return;
                if (dgFh[e.ColumnIndex, e.RowIndex].Value == null) dgFh[e.ColumnIndex, e.RowIndex].Value = "";
                for (int i = 0; i < dgFh.SelectedRows.Count; i++)
                {
                    string strTmp = "";
                    int intTmp = 0;
                    switch (e.ColumnIndex)
                    {
                        case 1:
                            strTmp = dgFh[1, dgFh.SelectedRows[i].Index].Value.ToString();
                            byte[] arayTmpRemark = HDLUDP.StringToByte(strTmp.Trim());
                            if (arayTmpRemark.Length > 8)
                            {
                                Array.Copy(arayTmpRemark, 0, myRS232.myAC[dgFh.SelectedRows[i].Index], 6, 8);
                            }
                            else
                            {
                                Array.Copy(arayTmpRemark, 0, myRS232.myAC[dgFh.SelectedRows[i].Index], 6, arayTmpRemark.Length);
                            }
                            break;
                        case 2:
                            strTmp = dgFh[2, dgvAC.SelectedRows[i].Index].Value.ToString();
                            myRS232.myAC[dgFh.SelectedRows[i].Index][1] = byte.Parse(HDLPF.IsNumStringMode(strTmp, 0, 255));
                            break;
                        case 3:
                            strTmp = dgFh[3, dgFh.SelectedRows[i].Index].Value.ToString();
                            strTmp = HDLPF.IsNumStringMode(strTmp, 0, 255);
                            intTmp = Convert.ToInt32(strTmp);
                            myRS232.myAC[dgFh.SelectedRows[i].Index][2] = Convert.ToByte(intTmp);
                            break;
                        case 4:   //发热体温度
                            strTmp = dgFh[4, dgFh.SelectedRows[i].Index].Value.ToString();
                            strTmp = HDLPF.IsNumStringMode(strTmp, 0, 255);
                            intTmp = Convert.ToInt32(strTmp);
                            myRS232.myAC[dgFh.SelectedRows[i].Index][3] = Convert.ToByte(intTmp);
                            break;
                        case 5:  // 防冻开关
                            strTmp = dgFh[5, dgFh.SelectedRows[i].Index].Value.ToString().ToLower();
                            if (strTmp == "true")
                                myRS232.myAC[dgFh.SelectedRows[i].Index][5] = 1;
                            else
                                myRS232.myAC[dgFh.SelectedRows[i].Index][5] = 0;
                            break;
                        case 6:  // 防冻温度
                            strTmp = dgFh[6, dgFh.SelectedRows[i].Index].Value.ToString();
                            strTmp = HDLPF.IsNumStringMode(strTmp, 0, 255);
                            myRS232.myAC[dgFh.SelectedRows[i].Index][4] = Convert.ToByte(strTmp);
                            break;
                    }
                    dgFh.SelectedRows[i].Cells[e.ColumnIndex].Value = dgFh[e.ColumnIndex, e.RowIndex].Value.ToString();
                }
            }
            catch
            {
            }
        }

      
    }
}
