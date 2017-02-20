using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace HDL_Buspro_Setup_Tool
{
    public partial class frmBacNet : Form
    {
        private string myDevName = null;
        private int mintIDIndex = -1;
        private byte SubNetID;
        private byte DevID;
        private int MyintDeviceType;
        private bool isReading = false;
        private BacNet myBcakNet = new BacNet();
        NetworkInForm networkinfo;
        BackNetID backnetid = new BackNetID("AI:0");
        ComboBox cbtype = new ComboBox();
        ComboBox cbpam1PanelControl = new ComboBox();
        ComboBox cbpam1Analog = new ComboBox();
        ComboBox cbpam2 = new ComboBox();
        TextBox txtSub = new TextBox();
        TextBox txtDev = new TextBox();
        TextBox txtpam1 = new TextBox();
        TextBox txtpam2 = new TextBox();
        TimeText timetxt = new TimeText(":");

        public frmBacNet()
        {
            InitializeComponent();
        }

        public frmBacNet(BacNet mybacknet, string strName, int intDIndex, int intDeviceType)
        {
            InitializeComponent();
            this.myDevName = strName;
            this.mintIDIndex = intDIndex;
            string strDevName = strName.Split('\\')[0].ToString();
            this.MyintDeviceType = intDeviceType;
            this.myBcakNet = mybacknet;

            HDLSysPF.DisplayDeviceNameModeDescription(strName, MyintDeviceType, cboDevice, tbModel, tbDescription);

            SubNetID = Convert.ToByte(strDevName.Split('-')[0]);
            DevID = Convert.ToByte(strDevName.Split('-')[1]);
            
            backnetid.UserControlValueChanged += new BackNetID.TextBoxChangedHandle(backnetid_UserControlValueChanged);
            cbtype.SelectedIndexChanged += new EventHandler(cbtype_SelectedIndexChanged);
            txtSub.TextChanged += new EventHandler(txtSub_TextChanged);
            txtDev.TextChanged += new EventHandler(txtDev_TextChanged);
            txtpam1.TextChanged += new EventHandler(txtpam1_TextChanged);
            txtpam2.TextChanged += new EventHandler(txtpam2_TextChanged);
            timetxt.TextChanged += new EventHandler(timetxt_TextChanged);
            cbpam1PanelControl.SelectedIndexChanged += new EventHandler(cbpam1PanelControl_SelectedIndexChanged);
            cbpam1Analog.SelectedIndexChanged += cbpam1Analog_SelectedIndexChanged;
        }

        void cbpam1Analog_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DgvList.CurrentRow.Index < 0) return;
            if (DgvList.RowCount <= 0) return;
            int index = DgvList.CurrentRow.Index;
            if (cbpam1Analog.Visible)
                DgvList[6, index].Value = cbpam1Analog.Text;
        }

        void timetxt_TextChanged(object sender, EventArgs e)
        {
            if (DgvList.CurrentRow.Index < 0) return;
            if (DgvList.RowCount <= 0) return;
            int index = DgvList.CurrentRow.Index;
            string str = HDLPF.GetStringFromTime(int.Parse(timetxt.Text.ToString()), ":");
            if (timetxt.Visible)
                DgvList[7, index].Value = str;
        }

        private void backnetid_UserControlValueChanged(object sender, EventArgs e)
        {
            if (DgvList.CurrentRow.Index < 0) return;
            if (DgvList.RowCount <= 0) return;
            int index = DgvList.CurrentRow.Index;

            string str = backnetid.Text;
            DgvList[1, index].Value = str;
            int intTmp = 0;
            string strTmp1 = str.Split(':')[0];
            string strTmp2 = str.Split(':')[1];
            switch (strTmp1)
            {
                case "AI":
                    intTmp = 0;
                    break;
                case "AO":
                    intTmp = 0x400000;
                    break;
                case "AV":
                    intTmp = 0x800000;
                    break;
                case "BI":
                    intTmp = 0xC00000;
                    break;
                case "BO":
                    intTmp = 0x1000000;
                    break;
                case "BV":
                    intTmp = 0x1400000;
                    break;
                case "MI":
                    intTmp = 0x3400000;
                    break;
                case "MO":
                    intTmp = 0x3800000;
                    break;
                case "MV":
                    intTmp = 0x4C00000;
                    break;
                case "Others":
                    intTmp = 0;
                    break;
            }
            intTmp = intTmp + Convert.ToInt32(strTmp2);
            DgvList[2, index].Value = "0x" + GlobalClass.AddLeftZero(Convert.ToString(intTmp, 16), 8);
            ModifyMultilinesIfNeeds(DgvList[1, index].Value.ToString(), 1);
            ModifyMultilinesIfNeeds(DgvList[2, index].Value.ToString(), 2);
        }

        private void txtpam2_TextChanged(object sender, EventArgs e)
        {
            if (DgvList.CurrentRow.Index < 0) return;
            if (DgvList.RowCount <= 0) return;
            int index = DgvList.CurrentRow.Index;

            string str = txtpam2.Text;
            if (!GlobalClass.IsNumeric(str)) str = "1";
            int num = Convert.ToInt32(str);
            switch (cbtype.SelectedIndex)
            {
                case 8:///面板控制
                    if (num > 255) str = "0";
                    if (cbpam1PanelControl.SelectedIndex == 15 || cbpam1PanelControl.SelectedIndex == 17 || cbpam1PanelControl.SelectedIndex == 18)
                        DgvList[7, index].Value = str + "(" + CsConst.mstrINIDefault.IniReadValue("public", "99846", "") + ")";
                    else if(cbpam1PanelControl.SelectedIndex==16)
                        DgvList[7, index].Value = str + "(" + CsConst.mstrINIDefault.IniReadValue("public", "99847", "") + ")";
                    break;
                case 12://通用控制
                    if (num > 65535) str = "0";
                    DgvList[7, index].Value = str;
                    break;
            }
            txtpam2.Text = str;
        }

        private void txtpam1_TextChanged(object sender, EventArgs e)
        {
            if (DgvList.CurrentRow.Index < 0) return;
            if (DgvList.RowCount <= 0) return;
            int index = DgvList.CurrentRow.Index;

            string str = txtpam1.Text;
            if (!GlobalClass.IsNumeric(str)) str = "1";

            int num = Convert.ToInt32(str);
            switch (cbtype.SelectedIndex)
            {
                case 1:///场景
                case 2:///序列
                    if (num > 254) str = "0";
                    DgvList[6, index].Value = str + "(" + CsConst.WholeTextsList[2510].sDisplayName + ")";
                    break;
                case 3:///通用开关
                    if (num > 255) str = "0";
                    DgvList[6, index].Value = str + "(" + CsConst.WholeTextsList[2513].sDisplayName + ")";
                    break;
                case 5://时间开关
                    if (num > 255) str = "0";
                    DgvList[6, index].Value = str + "(" + CsConst.mstrINIDefault.IniReadValue("public", "99921", "") + ")";
                    break;
                case 6:///窗帘
                    if (num > 255) str = "0";
                    DgvList[6, index].Value = str + "(" + CsConst.mstrINIDefault.IniReadValue("public", "99844", "") + ")";
                    break;
                case 7://GPRS控制
                    if (num > 255) str = "0";
                    DgvList[6, index].Value = str + "(" + CsConst.mstrINIDefault.IniReadValue("public", "99845", "") + ")";
                    break;
                case 11:///消防
                    if (num < 1 || num > 8) str = "1";
                    DgvList[6, index].Value = str + "(" + CsConst.WholeTextsList[2513].sDisplayName + ")";
                    break;
                case 15:///温度读取（1 byte）
                case 16:///温度读取（4 byte）
                    if (num < 1 ||num > 255) str = "1";
                    DgvList[6, index].Value = str + "(" + CsConst.mstrINIDefault.IniReadValue("public", "99921", "") + ")";
                    break;
                case 4:///单路调节
                    if (num < 1 || num > 254) str = "1";
                    DgvList[6, index].Value = str + "(" + CsConst.WholeTextsList[934].sDisplayName + ")";
                    break;
                case 12:///通用控制
                    if (num > 65535) str = "0";
                    DgvList[6, index].Value = str; 
                    break;
                case 13://干接点
                    if (num < 1 || num > 255) str = "1";
                    DgvList[6, index].Value = str;
                    break;
                case 17://DALI灯状态
                    if (num > 255) str = "1";
                    DgvList[6, index].Value = str;
                    break;
            }
            txtpam1.Text = str;
        }

        private void txtDev_TextChanged(object sender, EventArgs e)
        {
            if (DgvList.CurrentRow.Index < 0) return;
            if (DgvList.RowCount <= 0) return;
            int index = DgvList.CurrentRow.Index;
            string str =txtDev.Text;
            if (GlobalClass.IsNumeric(str))
            {
                int num = Convert.ToInt32(str);
                if (num > 254) str = "0";
            }
            else
            {
                str = "0";
            }
            txtDev.Text = str;
            DgvList[5, index].Value = str;
            ModifyMultilinesIfNeeds(DgvList[5, index].Value.ToString(),5);
        }

        private void txtSub_TextChanged(object sender, EventArgs e)
        {
            if (DgvList.CurrentRow.Index < 0) return;
            if (DgvList.RowCount <= 0) return;
            int index = DgvList.CurrentRow.Index;
            string str = txtSub.Text;
            if (GlobalClass.IsNumeric(str))
            {
                int num = Convert.ToInt32(str);
                if (num > 254) str = "0";
            }
            else
            {
                str = "0";
            }
            txtSub.Text = str;
            DgvList[4, index].Value = str;
            ModifyMultilinesIfNeeds(str,4);
        }

        void ModifyMultilinesIfNeeds(string strTmp, int ColumnIndex)
        {
            if (DgvList.SelectedRows == null || DgvList.SelectedRows.Count == 0) return;
            if (strTmp == null) strTmp = "";
            // change the value in selected more than one line
            for (int i = 0; i < DgvList.SelectedRows.Count; i++)
            {
                DgvList.SelectedRows[i].Cells[ColumnIndex].Value = strTmp;
            }
        }

        private void setAllVisible(Boolean TF)
        {
            if (isReading)
            {
                cbtype.Visible = TF;
                txtSub.Visible = TF;
                txtDev.Visible = TF;
                backnetid.Visible = TF;
            }
            cbpam1PanelControl.Visible = TF;
            cbpam1Analog.Visible = TF;
            cbpam2.Visible = TF;
            txtpam1.Visible = TF;
            txtpam2.Visible = TF;
            timetxt.Visible = TF;
        }

        private void cbpam1PanelControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DgvList.CurrentRow.Index < 0) return;
            if (DgvList.RowCount <= 0) return;
            int index = DgvList.CurrentRow.Index;

            if (cbpam1PanelControl.SelectedIndex == 15 || cbpam1PanelControl.SelectedIndex == 16
                || cbpam1PanelControl.SelectedIndex == 17 || cbpam1PanelControl.SelectedIndex == 18)
            {
                HDLSysPF.AddTextBoxToDGV(txtpam2, DgvList, 7, index);
            }
            else
            {
                txtpam2.Visible = false;
                DgvList[7, index].Value = "N/A";
            }
            DgvList[6, index].Value = cbpam1PanelControl.Text;
            ModifyMultilinesIfNeeds(DgvList[6, index].Value.ToString(),6);
        }

        private void cbtype_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DgvList.CurrentRow.Index < 0) return;
            if (DgvList.RowCount <= 0) return;
            int index=DgvList.CurrentRow.Index;
            setAllVisible(false);
            switch (cbtype.SelectedIndex)
            {
                case 0:
                    DgvList[6, index].Value = CsConst.mstrInvalid;
                    DgvList[7, index].Value = CsConst.mstrInvalid;
                    break;
                case 1://场景
                case 2://序列
                case 3://通用开关
                case 5://时间开关
                case 6://窗帘开关
                case 7://GPRS控制
                case 11://消防模块
                case 13:///干接点
                case 15://温度读取（1 byte）
                case 16://温度读取（4 byte）
                case 17://读取DALI灯状态
                    HDLSysPF.AddTextBoxToDGV(txtpam1, DgvList, 6, index);
                    DgvList[7, index].Value = CsConst.mstrInvalid;
                    break;
                case 4://单路调节
                    HDLSysPF.AddTextBoxToDGV(txtpam1, DgvList, 6, index);
                    addcontrols(7, index, timetxt);
                    timetxt.Text = HDLPF.GetTimeFromString(DgvList[7, index].Value.ToString(), ':');
                    break;
                case 8:///面板控制
                    HDLSysPF.AddComboboxToDGV(cbpam1PanelControl, DgvList, 6, index);
                    break;
                case 9://广播场景
                    DgvList[6, index].Value = CsConst.WholeTextsList[2566].sDisplayName;
                    DgvList[7, index].Value = CsConst.mstrInvalid;
                    break;
                case 10://广播回路
                    DgvList[6, index].Value = CsConst.WholeTextsList[2567].sDisplayName;
                    addcontrols(7, index,timetxt);
                    timetxt.Text = HDLPF.GetTimeFromString(DgvList[7, index].Value.ToString(), ':');
                    break;
                case 12://通用控制
                    HDLSysPF.AddTextBoxToDGV(txtpam1, DgvList, 6, index);
                    HDLSysPF.AddTextBoxToDGV(txtpam2, DgvList, 7, index);
                    break;
                case 14:///模拟量输出
                case 18:///模拟量设置
                    HDLSysPF.AddComboboxToDGV(cbpam1Analog, DgvList, 6, index);
                    DgvList[7, index].Value = CsConst.mstrInvalid;
                    break;
            }
            if (cbpam1PanelControl.Visible && cbpam1PanelControl.SelectedIndex < 0)
            {
                cbpam1PanelControl.SelectedIndex = 0;
            }
            if (cbpam1Analog.Visible && cbpam1Analog.SelectedIndex < 0)
            {
                cbpam1Analog.SelectedIndex = 0;
            }
            DgvList[3, index].Value = cbtype.Text;
            ModifyMultilinesIfNeeds(DgvList[3, index].Value.ToString(),3);
            if (txtpam1.Visible) txtpam1_TextChanged(null, null);
            if (txtpam2.Visible) txtpam2_TextChanged(null, null);
            if (cbpam1PanelControl.Visible) cbpam1PanelControl_SelectedIndexChanged(null, null);
            if (timetxt.Visible) timetxt_TextChanged(null, null);
            if (cbpam1Analog.Visible) cbpam1Analog_SelectedIndexChanged(null, null);

        }

        void SetVisibleAccordingly()
        {
            networkinfo = new NetworkInForm(SubNetID, DevID, MyintDeviceType);
            groupBox1.Controls.Add(networkinfo);
            networkinfo.Dock = DockStyle.Fill;

            cbtype.Items.Clear();
            cbtype.DropDownStyle = ComboBoxStyle.DropDownList;
            for (int i = 1; i < 20; i++)
            {
                cbtype.Items.Add(CsConst.mstrINIDefault.IniReadValue("KeyFunType", "000" + GlobalClass.AddLeftZero(i.ToString(), 2), ""));
            }
            cbpam1Analog.Items.Clear();
            cbpam1Analog.DropDownStyle = ComboBoxStyle.DropDownList;
            cbpam1Analog.Items.Add(CsConst.mstrINIDefault.IniReadValue("public", "99848", ""));
            cbpam1PanelControl.Items.Clear();
            cbpam1PanelControl.DropDownStyle = ComboBoxStyle.DropDownList;
            string sql = "";
            if (CsConst.iLanguageId == 0)
            {
                sql = "select NoteInEng from tmpAirControlTypeForPanelControl";
            }
            else if (CsConst.iLanguageId == 1)
            {
                sql = "select NoteInChn from tmpAirControlTypeForPanelControl";
            }
            HDLSysPF.AddItemtoCbFromDB(sql, cbpam1PanelControl);

            DgvList.Controls.Add(txtpam1);
            DgvList.Controls.Add(cbpam1Analog);
            DgvList.Controls.Add(cbpam1PanelControl);
            DgvList.Controls.Add(txtpam2);
            DgvList.Controls.Add(timetxt);
            isReading = true;
            setAllVisible(false);
            isReading = false;
        }

        private void frmBacNet_Load(object sender, EventArgs e)
        {
            toolStrip1.Visible = (CsConst.MyEditMode == 0);
            HDLSysPF.setDataGridViewColumnsWidth(DgvList);
        }


        private void pictureBox1_DoubleClick(object sender, EventArgs e)
        {
            PictureBox temp = new PictureBox();
        }

        private void tsbSave_Click(object sender, EventArgs e)
        {
            
        }

        private void DisplayBasicInformation()
        {
            chbEnable.Checked = myBcakNet.blnSwitch;
            numAddress.Value = myBcakNet.Address;
            numPort.Value = myBcakNet.intPort;
            numValid.Value = Convert.ToDecimal(myBcakNet.ValidCount);
        }

        private void showBackNetSetupInfo()
        {
            try
            {
                if (myBcakNet.otherInfo == null) return;
                if (myBcakNet.otherInfo.Count <= 0) return;
                DgvList.Rows.Clear();
                #region
                for (int i = 0; i < myBcakNet.otherInfo.Count; i++)
                {
                    BacNet.OtherInfo temp = myBcakNet.otherInfo[i];
                    int intValue = ((((temp.BackNetIDAry[3] * 256 + temp.BackNetIDAry[2]) & 0x7FF) * 65536 + temp.BackNetIDAry[1] * 256 + temp.BackNetIDAry[0]) & 0x7FFFFFF);
                    string strID = "";
                    string strhexID = "";
                    if (intValue >= 0 && intValue <= 0x3FFFFF)
                    {
                        strID = CsConst.BackNetIDString[0] + ":" + Convert.ToString(intValue);
                    }
                    else if (intValue >= 0x400000 && intValue <= 0x7FFFFF)
                    {
                        strID = CsConst.BackNetIDString[1] + ":" + Convert.ToString(intValue - 0x400000);
                    }
                    else if (intValue >= 0x800000 && intValue <= 0xBFFFFF)
                    {
                        strID = CsConst.BackNetIDString[2] + ":" + Convert.ToString(intValue - 0x800000);
                    }
                    else if (intValue >= 0xC00000 && intValue <= 0xFFFFFF)
                    {
                        strID = CsConst.BackNetIDString[3] + ":" + Convert.ToString(intValue - 0xC00000);
                    }
                    else if (intValue >= 0x1000000 && intValue <= 0x13FFFFF)
                    {
                        strID = CsConst.BackNetIDString[4] + ":" + Convert.ToString(intValue - 0x1000000);
                    }
                    else if (intValue >= 0x1400000 && intValue <= 0x17FFFFF)
                    {
                        strID = CsConst.BackNetIDString[5] + ":" + Convert.ToString(intValue - 0x1400000);
                    }
                    else if (intValue >= 0x3400000 && intValue <= 0x37FFFFF)
                    {
                        strID = CsConst.BackNetIDString[6] + ":" + Convert.ToString(intValue - 0x3400000);
                    }
                    else if (intValue >= 0x3800000 && intValue <= 0x3BFFFFF)
                    {
                        strID = CsConst.BackNetIDString[7] + ":" + Convert.ToString(intValue - 0x3800000);
                    }
                    else if (intValue >= 0x4C00000 && intValue <= 0x4FFFFFF)
                    {
                        strID = CsConst.BackNetIDString[8] + ":" + Convert.ToString(intValue - 0x4C00000);
                    }
                    else
                    {
                        strID = CsConst.BackNetIDString[9] + ":" + Convert.ToString(intValue);
                    }
                    strhexID = "0x" + GlobalClass.AddLeftZero(Convert.ToString(intValue, 16), 8);
                    string strType = "";
                    string stFirst = "";
                    string strSecond = "";
                    switch (temp.Type)
                    {
                        case 0x000C:
                            if (temp.Param1 == 255)///广播场景
                            {
                                strType = CsConst.mstrINIDefault.IniReadValue("KeyFunType", "00010", "");
                                stFirst = CsConst.WholeTextsList[2566].sDisplayName;
                            }
                            else////场景控制
                            {
                                strType = CsConst.mstrINIDefault.IniReadValue("KeyFunType", "00002", "");
                                stFirst = temp.Param1.ToString() + "(" + CsConst.WholeTextsList[2510].sDisplayName + ")";
                            }
                            break;
                        case 0xE014:///序列控制
                            strType = CsConst.mstrINIDefault.IniReadValue("KeyFunType", "00003", "");
                            stFirst = temp.Param1.ToString() + "(" + CsConst.WholeTextsList[2510].sDisplayName + ")";
                            break;
                        case 0xE018:////通用开关
                            strType = CsConst.mstrINIDefault.IniReadValue("KeyFunType", "00004", "");
                            stFirst = temp.Param1.ToString() + "(" + CsConst.WholeTextsList[2513].sDisplayName + ")";
                            break;
                        case 0x0033:
                            if (temp.Param1 == 255)////广播回路
                            {
                                strType = CsConst.mstrINIDefault.IniReadValue("KeyFunType", "00011", "");
                                stFirst = CsConst.WholeTextsList[2567].sDisplayName;
                            }
                            else///单路调节
                            {
                                strType = CsConst.mstrINIDefault.IniReadValue("KeyFunType", "00005", "");
                                stFirst = temp.Param1.ToString() + "(" + CsConst.WholeTextsList[934].sDisplayName + ")";
                            }
                            int intTime = temp.Param2 * 256 + temp.Param3;
                            intTime = intTime % 3659;
                            strSecond = Convert.ToString(intTime / 60) + ":" + Convert.ToString(intTime % 60);
                            break;
                        case 0xF112:///时间开关
                            strType = CsConst.mstrINIDefault.IniReadValue("KeyFunType", "00006", "");
                            stFirst = temp.Param1.ToString() + "(" + CsConst.mstrINIDefault.IniReadValue("public", "99921", "") + ")";
                            break;
                        case 0xE3E2:////窗帘开关
                            strType = CsConst.mstrINIDefault.IniReadValue("KeyFunType", "00007", "");
                            stFirst = temp.Param1.ToString() + "(" + CsConst.mstrINIDefault.IniReadValue("public", "99844", "") + ")";
                            break;
                        case 0xE3D6:///GPRS控制
                            strType = CsConst.mstrINIDefault.IniReadValue("KeyFunType", "00008", "");
                            stFirst = temp.Param1.ToString() + "(" + CsConst.mstrINIDefault.IniReadValue("public", "99845", "") + ")";
                            break;
                        case 0xE3DA:///面板控制
                            strType = CsConst.mstrINIDefault.IniReadValue("KeyFunType", "00009", "");
                            if (temp.Param1 > 28 || temp.Param1 < 0)
                            {
                                temp.Param1 = 0;
                            }
                            stFirst = HDLSysPF.selectNoteResultFromDB("tmpAirControlTypeForPanelControl", Convert.ToInt32(temp.Param1));
                            switch (temp.Param1)
                            {
                                case 15:
                                case 17:
                                case 18:
                                    strSecond = temp.Param2.ToString() + "(" + CsConst.mstrINIDefault.IniReadValue("public", "99846", "") + ")";
                                    break;
                                case 16:
                                    strSecond = temp.Param2.ToString() + "(" + CsConst.mstrINIDefault.IniReadValue("public", "99847", "") + ")";
                                    break;
                            }
                            break;
                        case 0x011E:///消防模块
                            strType = CsConst.mstrINIDefault.IniReadValue("KeyFunType", "00012", "");
                            //if (Param1 > 8 || Param1 < 1) Param1 = 1;
                            stFirst = Convert.ToString(temp.Param1) + "(" + CsConst.WholeTextsList[2510].sDisplayName + ")";
                            break;
                        case 0x16A4:///通用控制
                            strType = CsConst.mstrINIDefault.IniReadValue("KeyFunType", "00013", "");
                            stFirst = Convert.ToString(temp.Param2);
                            strSecond = Convert.ToString(temp.Param1);
                            break;
                        case 0x15CE:///干接点
                            strType = CsConst.mstrINIDefault.IniReadValue("KeyFunType", "00014", "");
                            stFirst = Convert.ToString(temp.Param1);
                            break;
                        case 0xE440:///模拟量输出
                            strType = CsConst.mstrINIDefault.IniReadValue("KeyFunType", "00015", "");
                            stFirst = cbpam1Analog.Items[0].ToString();
                            break;
                        case 0xE3E7:///温度读取 1byte
                            strType = CsConst.mstrINIDefault.IniReadValue("KeyFunType", "00016", "");
                            stFirst = Convert.ToString(temp.Param1) + "(" + CsConst.mstrINIDefault.IniReadValue("public", "99921", "") + ")";
                            break;
                        case 0x1948:///温度读取 4byte
                            strType = CsConst.mstrINIDefault.IniReadValue("KeyFunType", "00017", "");
                            stFirst = Convert.ToString(temp.Param1) + "(" + CsConst.mstrINIDefault.IniReadValue("public", "99921", "") + ")";
                            break;
                        case 0xA008:///读取DALI灯状态
                            strType = CsConst.mstrINIDefault.IniReadValue("KeyFunType", "00018", "");
                            stFirst = Convert.ToString(temp.Param1);
                            break;
                        case 0xE444:///模拟量设置
                            strType = CsConst.mstrINIDefault.IniReadValue("KeyFunType", "00019", "");
                            stFirst = cbpam1Analog.Items[0].ToString();
                            break;
                    }
                    if (strType == "")
                    {
                        strType = CsConst.mstrINIDefault.IniReadValue("KeyFunType", "00001", "");
                        stFirst = "N/A";
                    }
                    if (strSecond == "") strSecond = "N/A";
                    object[] obj = new object[] { temp.ID.ToString(),strID,strhexID,strType,temp.strDevName.Split('/')[0].ToString(),
                                              temp.strDevName.Split('/')[1].ToString(),stFirst,strSecond,temp.Remark};
                    DgvList.Rows.Add(obj);
                }
                #endregion
            }
            catch
            {
            }
            isReading = false;
        }

        private void addcontrols(int col, int row, Control con)
        {
            con.Visible = true;
            Rectangle rect = DgvList.GetCellDisplayRectangle(col, row, true);
            con.Size = rect.Size;
            con.Top = rect.Top;
            con.Left = rect.Left;
        }

        private void DgvList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            backnetid.Visible = false;
            cbtype.Visible = false;
            txtSub.Visible = false;
            txtDev.Visible = false;
            if (e.RowIndex >= 0)
            {
                //backnetid = new BackNetID(DgvList[1, e.RowIndex].Value.ToString());
                //backnetid.UserControlValueChanged += new BackNetID.TextBoxChangedHandle(backnetid_UserControlValueChanged);
                backnetid.Text = DgvList[1, e.RowIndex].Value.ToString();
                DgvList.Controls.Add(backnetid);
                backnetid.Show();
                backnetid.Visible = true;
                addcontrols(1, e.RowIndex, backnetid);

                cbtype.SelectedIndex = cbtype.Items.IndexOf(DgvList[3, e.RowIndex].Value.ToString());
                DgvList.Controls.Add(cbtype);
                cbtype.Show();
                cbtype.Visible = true;
                addcontrols(3, e.RowIndex, cbtype);

                
                DgvList.Controls.Add(txtSub);
                txtSub.Show();
                txtSub.Visible = true;
                addcontrols(4, e.RowIndex, txtSub);
                txtSub.Text = DgvList[4, e.RowIndex].Value.ToString().Trim();
                

                txtDev.Text = DgvList[5, e.RowIndex].Value.ToString().Trim();
                DgvList.Controls.Add(txtDev);
                txtDev.Show();
                txtDev.Visible = true;
                addcontrols(5, e.RowIndex, txtDev);

                txtSub_TextChanged(null, null);
                txtDev_TextChanged(null, null);
                cbtype_SelectedIndexChanged(null, null);
            }
        }

        private void tslWriteCurrentPage_Click(object sender, EventArgs e)
        {
            isReading = true;
            setAllVisible(false);
            SetBackNetValue();
            if (tabBackNet.SelectedIndex == 0)
            {
                myBcakNet.UploadCurtainInfosToDevice(myDevName, 1);
            }
            isReading = false;
        }

        private void SetBackNetValue()
        {
            myBcakNet.otherInfo = new List<BacNet.OtherInfo>();
            for (int i = 0; i < DgvList.RowCount; i++)
            {
                BacNet.OtherInfo temp = new BacNet.OtherInfo();
                temp.ID = Convert.ToByte(i + 1);
                temp.BackNetIDAry = new byte[4];
                int intTmp = Convert.ToInt32(DgvList[2, i].Value.ToString(), 16);
                temp.BackNetIDAry[0] = Convert.ToByte((intTmp & 0xFF));
                temp.BackNetIDAry[1] = Convert.ToByte((intTmp & 0xFFFF) / 256);
                temp.BackNetIDAry[2] = Convert.ToByte((intTmp / 0xFFFF) & 256);
                temp.BackNetIDAry[2] = Convert.ToByte((intTmp & 0xFFFF) / 256);

                int typeTmp = cbtype.Items.IndexOf(DgvList[3, i].Value.ToString());
                string strFist = DgvList[6, i].Value.ToString();
                if (strFist.Contains("(")) strFist = strFist.Split('(')[0];
                string strSencond = DgvList[7, i].Value.ToString();
                if (strSencond.Contains("(")) strSencond = strSencond.Split('(')[0];
                switch (typeTmp)
                {
                    case 0:
                        temp.Type = 0;
                        break;
                    case 1://场景
                        temp.Type = 0x000C;
                        temp.Param1 = Convert.ToByte(strFist);
                        break;
                    case 9: // 广播场景
                        temp.Type = 0x000C;
                        temp.Param1 = 255;
                        break;
                    case 2://序列
                        temp.Type = 0xE014;
                        temp.Param1 = Convert.ToByte(strFist);
                        break;
                    case 3://通用开关
                        temp.Type = 0xE018;
                        temp.Param1 = Convert.ToByte(strFist);
                        break;
                    case 4://单路调节
                        temp.Type = 0x0033;
                        int intTmp2 = Convert.ToInt32(HDLPF.GetTimeFromString(strSencond, ':'));
                        temp.Param1 = Convert.ToByte(Convert.ToInt32(strFist));
                        temp.Param2 = Convert.ToByte(intTmp2 / 256);
                        temp.Param3 = Convert.ToByte(intTmp2 % 256);
                        break;
                    case 10:// 广播回路
                        temp.Type = 0x0033;
                        int intTmpTime = Convert.ToInt32(HDLPF.GetTimeFromString(strSencond, ':'));
                        temp.Param1 = 255;
                        temp.Param2 = Convert.ToByte(intTmpTime / 256);
                        temp.Param3 = Convert.ToByte(intTmpTime % 256);
                        break;
                    case 5://时间开关
                        temp.Type = 0xF112;
                        temp.Param1 = Convert.ToByte(strFist);
                        break;
                    case 6://窗帘开关
                        temp.Type = 0xE3E2;
                        if (strFist == CsConst.mstrInvalid) strFist = CsConst.CurtainModes[1];
                        temp.Param1 = Convert.ToByte(strFist);
                        break;
                    case 7://GPRS控制
                        temp.Type = 0xE3D6;
                        temp.Param1 = Convert.ToByte(strFist);
                        break;
                    case 8://面板控制
                        temp.Type = 0xE3DA;
                        temp.Param1 = Convert.ToByte(cbpam1PanelControl.Items.IndexOf(strFist));
                        if (temp.Param1 == 15 || temp.Param1 == 16 || temp.Param1 == 17 || temp.Param1 == 18)
                        {
                            temp.Param2 = Convert.ToByte(strSencond);
                        }
                        break;
                    case 11://消防模块
                        temp.Type = 0x011E;
                        temp.Param1 = Convert.ToByte(strFist);
                        break;
                    case 12://通用控制
                        temp.Type = 0x16A4;
                        temp.Param1 = Convert.ToByte(strFist);
                        temp.Param2 = Convert.ToByte(strSencond);
                        break;
                    case 13://干接点
                        temp.Type = 0x15CE;
                        temp.Param1 = Convert.ToByte(strFist);
                        break;
                    case 14://模拟值输出
                        temp.Type = 0xE440;
                        temp.Param1 = Convert.ToByte(cbpam1Analog.Items.IndexOf(strFist));
                        break;
                    case 15://温度读取（1 byte）
                        temp.Type = 0xE3E7;
                        temp.Param1 = Convert.ToByte(strFist);
                        break;
                    case 16://温度读取（4 byte）
                        temp.Type = 0x1948;
                        temp.Param1 = Convert.ToByte(strFist);
                        break;
                    case 17://读取DALI灯状态
                        temp.Type = 0xA008;
                        temp.Param1 = Convert.ToByte(strFist);
                        break;
                    case 18://模拟量设置
                        temp.Type = 0xE444;
                        temp.Param1 = Convert.ToByte(cbpam1Analog.Items.IndexOf(strFist));
                        break;
                }
                if (DgvList[8, i].Value == null) temp.Remark = "";
                else
                    temp.Remark = DgvList[8, i].Value.ToString();
                temp.strDevName = Convert.ToByte(DgvList[4, i].Value.ToString()) + "/" + Convert.ToByte(DgvList[5, i].Value.ToString());
                myBcakNet.otherInfo.Add(temp);
            }
        }

        private void DgvList_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (DgvList.RowCount <= 0) return;
            int index = e.RowIndex;
            if (timetxt.Visible)
            {
                DgvList[7, e.RowIndex].Value = HDLPF.GetStringFromTime(int.Parse(timetxt.Text.ToString()), ":");
            }
            DgvList[1, e.RowIndex].Value = backnetid.Text;
        }

        private void DgvList_Scroll(object sender, ScrollEventArgs e)
        {
            isReading = true;
            setAllVisible(false);
            isReading = false;
        }

        private void tsbDown_Click(object sender, EventArgs e)
        {
            try
            {
                isReading = true;
                setAllVisible(false);

                //更新数据
                SetBackNetValue();

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

                    byte[] ArayRelay = new byte[] { bytSubID, bytDevID, (byte)(MyintDeviceType / 256), (byte)(MyintDeviceType % 256),0, 
                                                    (byte)(mintIDIndex / 256), (byte)(mintIDIndex % 256),
                                                    Convert.ToByte(txtFrm.Text),Convert.ToByte(txtTo.Text)};
                    CsConst.MyUPload2DownLists.Add(ArayRelay);
                    CsConst.MyUpload2Down = Convert.ToByte(((ToolStripButton)sender).Tag.ToString());
                    FrmDownloadShow Frm = new FrmDownloadShow();
                    if (CsConst.MyUpload2Down == 0) Frm.FormClosing += new FormClosingEventHandler(UpdateDisplayInformationAccordingly);
                    Frm.ShowDialog();
                }
            }
            catch
            {
            }
        }

        void UpdateDisplayInformationAccordingly(object sender, FormClosingEventArgs e)
        {
            #region
            DisplayBasicInformation();
            showBackNetSetupInfo();
            #endregion
            Cursor.Current = Cursors.Default;
            this.BringToFront();
        }

        private void frmBacNet_Shown(object sender, EventArgs e)
        {
            SetVisibleAccordingly();

            if (CsConst.MyEditMode == 0)
            {
                DisplayBasicInformation();
                showBackNetSetupInfo();
            }
            else if (CsConst.MyEditMode == 1) //在线模式
            {
                if (myBcakNet.MyRead2UpFlags[0] == false)
                {
                    tsbDown_Click(tsbDown, null);
                }
                else
                {
                    DisplayBasicInformation();
                    showBackNetSetupInfo();
                }
            }
            myBcakNet.MyRead2UpFlags = new bool[] { true,true};
        }
        private void chbEnable_CheckedChanged(object sender, EventArgs e)
        {
            myBcakNet.blnSwitch = chbEnable.Checked;
        }

        private void numAddress_ValueChanged(object sender, EventArgs e)
        {
            myBcakNet.Address =Convert.ToInt16(numAddress.Value.ToString());
        }

        private void numPort_ValueChanged(object sender, EventArgs e)
        {
            myBcakNet.intPort = Convert.ToInt32(numPort.Value.ToString());
        }

        private void txtNum_ValueChanged(object sender, EventArgs e)
        {

        }

        private void tsbDefault_Click(object sender, EventArgs e)
        {
            myBcakNet.ReadDefaultInfo();

            DisplayBasicInformation();
            showBackNetSetupInfo();
            myBcakNet.MyRead2UpFlags[1] = false;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            tsbDown_Click(tsbUpload, null);
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
            int num = Convert.ToInt32(txtTo.Text);
            txtFrm.Text = HDLPF.IsNumStringMode(str, 1, num);
            txtFrm.SelectionStart = txtFrm.Text.Length;
        }

        private void txtTo_Leave(object sender, EventArgs e)
        {
            string str = txtTo.Text;
            int num = Convert.ToInt32(txtFrm.Text);
            txtTo.Text = HDLPF.IsNumStringMode(str, num, 65535);
            txtTo.SelectionStart = txtTo.Text.Length;
        }

        private void btnSure_Click(object sender, EventArgs e)
        {
            tsbDown_Click(tsbDown, null);
        }

        private void btnrefence_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                byte[] ArayTmp = new byte[0];
                // 读取基本信息bacnet地址设置
                #region
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xBACE, SubNetID, DevID, false, true, true,false) == true)
                {
                    myBcakNet.Address = CsConst.myRevBuf[25] * 256 * 256 * 256
                            + CsConst.myRevBuf[26] * 256 * 256
                            + CsConst.myRevBuf[27] * 256
                            + CsConst.myRevBuf[28];
                    CsConst.myRevBuf = new byte[1200];
                }
                else return;

                #endregion

                //读取bacnet端口
                #region
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x180D, SubNetID, DevID, false, true, true,false) == true)
                {
                    myBcakNet.blnSwitch = (CsConst.myRevBuf[26] == 1);
                    myBcakNet.intPort = CsConst.myRevBuf[31] * 256 + CsConst.myRevBuf[32];
                }
                else return;
                #endregion

                // 读取转换信息列表
                //读取bacnet个数
                #region
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xBAC6, SubNetID, DevID, false, true, true,false) == true)
                {
                    myBcakNet.ValidCount = CsConst.myRevBuf[25] * 256 + CsConst.myRevBuf[26];
                    CsConst.myRevBuf = new byte[1200];
                }
                else return;
                #endregion
                DisplayBasicInformation();
            }
            catch
            {
            }
            Cursor.Current = Cursors.Default;
        }

        private void btnSaveTemp_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                // 修改基本信息设备ID bacnet标准设置
                #region
                byte[] ArayTmp = new byte[4];
                ArayTmp[0] = Convert.ToByte(myBcakNet.Address >> 24);
                ArayTmp[1] = Convert.ToByte(myBcakNet.Address >> 16 & 0x00FF);
                ArayTmp[2] = Convert.ToByte(myBcakNet.Address >> 8 & 0x0000FF);
                ArayTmp[3] = Convert.ToByte(myBcakNet.Address % 256);
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xBAD0, SubNetID, DevID, false, true, true, false) == true)
                {
                    CsConst.myRevBuf = new byte[1200];
                }
                else return;
                #endregion

                //bacnet 端口 IP设置
                #region
                ArayTmp = new byte[7];
                if (myBcakNet.blnSwitch) ArayTmp[0] = 1;
                ArayTmp[5] = Convert.ToByte(myBcakNet.intPort / 256);
                ArayTmp[6] = Convert.ToByte(myBcakNet.intPort % 256);
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x180F, SubNetID, DevID, false, true, true, false) == true)
                {
                    CsConst.myRevBuf = new byte[1200];
                }
                else return;

                ArayTmp = new byte[2];
                ArayTmp[0] = Convert.ToByte(myBcakNet.ValidCount / 256);
                ArayTmp[1] = Convert.ToByte(myBcakNet.ValidCount % 256);

                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xBAC8, SubNetID, DevID, false, true, true, false) == false)
                {
                    CsConst.myRevBuf = new byte[1200];
                }
                else return;
            }
            catch
            {
            }
            Cursor.Current = Cursors.Default;
            #endregion
        }

        private void btnSaveAndClose_Click(object sender, EventArgs e)
        {
            tsbDown_Click(tsbUpload, null);
            this.Close();
        }

        private void tmRead_Click(object sender, EventArgs e)
        {
            tsbDown_Click(tsbDown, null);
        }

        private void numValid_ValueChanged(object sender, EventArgs e)
        {
            myBcakNet.ValidCount = Convert.ToInt32(numValid.Value);
        }

    }
}
