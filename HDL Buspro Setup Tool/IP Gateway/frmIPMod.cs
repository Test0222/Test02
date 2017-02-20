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
    public partial class frmIPMod : Form
    {
        private IPModule myIPModule = null;
        private string myDevName = null;
        private int mintIDIndex = -1;
        private int mywdDevicerType = -1;
        private byte SubNetID;
        private byte DeviceID;
        private bool isRead = false;
        private TextBox txtCommand = new TextBox();

        private static Connetion RemoteConnection;
        private static NetworkInForm networkinfo;
        private int MyActivePage = 0; //按页面上传下载
        public frmIPMod()
        {
            InitializeComponent();
        }

        public frmIPMod(IPModule oIPModule, string strName, int intDIndex,int intDeviceType)
        {
            InitializeComponent();
            
            this.myIPModule = oIPModule;
            this.myDevName = strName;
            this.mintIDIndex = intDIndex;
            this.mywdDevicerType = intDeviceType;

            string strDevName = strName.Split('\\')[0].ToString();

            HDLSysPF.DisplayDeviceNameModeDescription(strName, mywdDevicerType, cboDevice, tbModel, tbDescription);

            SubNetID = Convert.ToByte(strDevName.Split('-')[0]);
            DeviceID = Convert.ToByte(strDevName.Split('-')[1]);
            tsl3.Text = strName;
            txtCommand = new TextBox();
            txtCommand.KeyPress += txtCommand_KeyPress;
            txtCommand.TextChanged += txtCommand_TextChanged;
            dgvFilter.Controls.Add(txtCommand);
            txtCommand.Visible = false;
            txtCommand.MaxLength = 4;
        }

        void txtCommand_TextChanged(object sender, EventArgs e)
        {
            if (dgvFilter.CurrentRow.Index < 0) return;
            if (dgvFilter.RowCount <= 0) return;
            int index = dgvFilter.CurrentRow.Index;
            if (txtCommand.Text.Length > 0)
            {
                dgvFilter[6, index].Value = txtCommand.Text;
                ModifyMultilinesIfNeeds(dgvFilter[6, index].Value.ToString(), 6);
            }
        }

        void txtCommand_KeyPress(object sender, KeyPressEventArgs e)
        {
            string str = "abcdefABCDEF1234567890";
            if (e.KeyChar != 8)
            {
                if (str.IndexOf(e.KeyChar.ToString()) < 0)
                {
                    e.Handled = true;
                }
            }
        }

        void ModifyMultilinesIfNeeds(string strTmp, int ColumnIndex)
        {
            if (dgvFilter.SelectedRows == null || dgvFilter.SelectedRows.Count == 0) return;
            if (strTmp == null) strTmp = "";
            // change the value in selected more than one line
            if (dgvFilter.SelectedRows.Count > 1)
            {
                for (int i = 0; i < dgvFilter.SelectedRows.Count; i++)
                {
                    dgvFilter.SelectedRows[i].Cells[ColumnIndex].Value = strTmp;
                }
            }
        }


        void DisplaySettingToForm()
        {
            try
            {
                if (GbRf.Visible == true) btnRead_Click(btnRead, null);
                myIPModule.MyRead2UpFlags[0] = true;
            }
            catch
            {
            }
            isRead = false;
        }

        void InitialFormCtrlsTextOrItems()
        {
            HDLSysPF.LoadButtonModeWithDifferentDeviceType(cboButtonMode, mywdDevicerType);
        }


        private void tbGroup_TextChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            if (myIPModule == null) myIPModule.ReadDefaultInfo();

            string strTmp = ((TextBox)sender).Text.ToString();
            byte bytTag = byte.Parse(((TextBox)sender).Tag.ToString());

            switch (bytTag)
            {
                case 0: 
                    myIPModule.strGroup = strTmp;
                    break;
                case 1: 
                    myIPModule.strPrjName = strTmp;
                    break;
                case 2: 
                    myIPModule.strUser = strTmp;
                    break;
                case 3: myIPModule.strPWD = strTmp;
                    break;
                case 4: 
                    if(strTmp != "")
                        myIPModule.intPort1 = Convert.ToInt32(strTmp);
                    break;
                case 5:
                    if(strTmp != "")
                        myIPModule.intPort2 = Convert.ToInt32(strTmp);
                    break;
                case 6:
                    if(strTmp != "")
                        myIPModule.bytTimer = Convert.ToByte(strTmp);
                    break;
            }

        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            if (myIPModule == null) return;
            myIPModule.ReadDefaultInfo();

            DisplaySettingToForm();
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            if (myIPModule == null) return;
            myIPModule.SaveCurtainToDB();
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

                    byte[] ArayRelay = new byte[] { bytSubID, bytDevID, (byte)(mywdDevicerType / 256), (byte)(mywdDevicerType % 256)
                        , (byte)MyActivePage,(byte)(mintIDIndex/256),(byte)(mintIDIndex%256) };
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
            switch ( tabControl.SelectedTab.Name)
            {
                case "tabKeys":
                    DisplaySettingToForm(); break;
                case "tabRF":
                        ShowFilterInfomation();
                    break;
                case "tabRfButton":
                        ShowRFButtonsInformation();
                    break;
            }
            #endregion
            this.BringToFront();
        }

        private void toolStripButton11_Click(object sender, EventArgs e)
        {
            if (myIPModule == null) return;

            //frmProperty frm = new frmProperty(myDevName, mywdDevicerType);
            //frm.ShowDialog();
        }

        private void frmIPMod_Shown(object sender, EventArgs e)
        {
            SetCtrlsVisbleWithDifferentDeviceType();
            if (CsConst.MyEditMode == 0) //工程师模式
            {
                DisplaySettingToForm();
            }
            else if (CsConst.MyEditMode == 1) //在线模式
            {
                DisplaySettingToForm();
            }
        }

        void SetCtrlsVisbleWithDifferentDeviceType()
        {
            if (myIPModule == null) return;

            RemoteConnection = new Connetion(SubNetID, DeviceID, mywdDevicerType);
            tabKeys.Controls.Add(RemoteConnection);
            RemoteConnection.BringToFront();
            RemoteConnection.Dock = DockStyle.Fill;

            networkinfo = new NetworkInForm(SubNetID, DeviceID, mywdDevicerType);
            panel1.Controls.Add(networkinfo);
            networkinfo.Dock = DockStyle.Top;
            networkinfo.BringToFront();

            if (IPmoduleDeviceTypeList.RFIpModuleV1.Contains(mywdDevicerType) || IPmoduleDeviceTypeList.RFIpModuleV2.Contains(mywdDevicerType))
            {
                GbRf.Visible = true;
                if (IPmoduleDeviceTypeList.RFIpModuleV1.Contains(mywdDevicerType)) tabControl.TabPages.Remove(tabRfButton);
            }
            else if (IPmoduleDeviceTypeList.IpModuleV3TimeZoneUrl.Contains(mywdDevicerType)) // 新版一端口
            {
                GbRf.Visible = false;
                gbNetwork.Visible = false;
                tabControl.TabPages.Remove(tabRfButton);
            }
            else
            {
                GbRf.Visible = false;
                tabControl.TabPages.Remove(tabRF);
                tabControl.TabPages.Remove(tabRfButton);
            }

            for (int i = 1; i <= 255; i++)
            {
                cbThrough.Items.Add(i.ToString());
            }
        }

        private void frmIPMod_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void tmRead_Click(object sender, EventArgs e)
        {
            btnRef2_Click(btnRef2, null);
        }

        private void tmUpload_Click(object sender, EventArgs e)
        {
            tslRead_Click(toolStripLabel2,null);
        }

        private void dgvFilter_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtCommand.Visible = false;
            if (e.RowIndex >= 0)
            {
                txtCommand.Text = dgvFilter[6, e.RowIndex].Value.ToString();
                HDLSysPF.addcontrols(6, e.RowIndex, txtCommand, dgvFilter);
            }
        }

        private void dgvFilter_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (myIPModule == null) return;
                if (myIPModule.MyBlocks == null) return;
                if ((e.RowIndex == -1) || (e.ColumnIndex == -1)) return;
                if (dgvFilter[e.ColumnIndex, e.RowIndex].Value == null) return;
                if (dgvFilter.SelectedRows.Count == 0) return;
                if (isRead) return;
                if (dgvFilter[e.ColumnIndex, e.RowIndex].Value == null) dgvFilter[e.ColumnIndex, e.RowIndex].Value = "";
                for (int i = 0; i < dgvFilter.SelectedRows.Count; i++)
                {
                    dgvFilter.SelectedRows[i].Cells[e.ColumnIndex].Value = dgvFilter[e.ColumnIndex, e.RowIndex].Value.ToString();
                    string strTmp = "";
                    string strEnable = "00000";
                    switch (e.ColumnIndex)
                    {
                        case 1:
                            if (dgvFilter[1, dgvFilter.SelectedRows[i].Index].Value.ToString().ToLower() == "true")
                                myIPModule.MyBlocks[dgvFilter.SelectedRows[i].Index].isEnable = 1;
                            else
                                myIPModule.MyBlocks[dgvFilter.SelectedRows[i].Index].isEnable = 0;
                            break;
                        case 2:
                            string str1 = "0";
                            string str2 = "0";
                            if (dgvFilter[5, dgvFilter.SelectedRows[i].Index].Value.ToString().ToLower() == "true") str1 = "1";
                            if (dgvFilter[7, dgvFilter.SelectedRows[i].Index].Value.ToString().ToLower() == "true") str2 = "1";
                            if (dgvFilter[2, dgvFilter.SelectedRows[i].Index].Value.ToString().ToLower() == "true")
                            {
                                strEnable = strEnable + str2 + str1 + "1";
                            }
                            else
                            {
                                strEnable = strEnable + str2 + str1 + "0";
                            }
                            myIPModule.MyBlocks[dgvFilter.SelectedRows[i].Index].blockType = Convert.ToByte(GlobalClass.BitToInt(strEnable));
                            break;
                        case 3:
                            strTmp = dgvFilter[3, dgvFilter.SelectedRows[i].Index].Value.ToString();
                            myIPModule.MyBlocks[dgvFilter.SelectedRows[i].Index].sourceSubnetID = byte.Parse(HDLPF.IsNumStringMode(strTmp, 0, 255));
                            break;
                        case 4:
                            strTmp = dgvFilter[4, dgvFilter.SelectedRows[i].Index].Value.ToString();
                            myIPModule.MyBlocks[dgvFilter.SelectedRows[i].Index].sourceDeviceID = byte.Parse(HDLPF.IsNumStringMode(strTmp, 0, 255));
                            break;
                        case 5:
                            str1 = "0";
                            str2 = "0";
                            if (dgvFilter[2, dgvFilter.SelectedRows[i].Index].Value.ToString().ToLower() == "true") str1 = "1";
                            if (dgvFilter[7, dgvFilter.SelectedRows[i].Index].Value.ToString().ToLower() == "true") str2 = "1";
                            if (dgvFilter[5, dgvFilter.SelectedRows[i].Index].Value.ToString().ToLower() == "true")
                            {
                                strEnable = strEnable + str2 + "1" + str1;
                            }
                            else
                            {
                                strEnable = strEnable + str2 + "0" + str1;
                            }
                            myIPModule.MyBlocks[dgvFilter.SelectedRows[i].Index].blockType = Convert.ToByte(GlobalClass.BitToInt(strEnable));
                            break;
                        case 6:
                            strTmp = dgvFilter[6, dgvFilter.SelectedRows[i].Index].Value.ToString();
                            myIPModule.MyBlocks[dgvFilter.SelectedRows[i].Index].command = Convert.ToInt32(strTmp, 16);
                            break;
                        case 7:
                            str1 = "0";
                            str2 = "0";
                            if (dgvFilter[2, dgvFilter.SelectedRows[i].Index].Value.ToString().ToLower() == "true") str1 = "1";
                            if (dgvFilter[5, dgvFilter.SelectedRows[i].Index].Value.ToString().ToLower() == "true") str2 = "1";
                            if (dgvFilter[7, dgvFilter.SelectedRows[i].Index].Value.ToString().ToLower() == "true")
                            {
                                strEnable = strEnable + "1" + str2 + str1;
                            }
                            else
                            {
                                strEnable = strEnable + "0" + str2 + str1;
                            }
                            myIPModule.MyBlocks[dgvFilter.SelectedRows[i].Index].blockType = Convert.ToByte(GlobalClass.BitToInt(strEnable));
                            break;
                        case 8:
                            strTmp = dgvFilter[8, dgvFilter.SelectedRows[i].Index].Value.ToString();
                            myIPModule.MyBlocks[dgvFilter.SelectedRows[i].Index].destSubnetID = byte.Parse(HDLPF.IsNumStringMode(strTmp, 0, 255));
                            break;
                        case 9:
                            strTmp = dgvFilter[8, dgvFilter.SelectedRows[i].Index].Value.ToString();
                            myIPModule.MyBlocks[dgvFilter.SelectedRows[i].Index].destDeviceID = byte.Parse(HDLPF.IsNumStringMode(strTmp, 0, 255));
                            break;
                    }
                }
                if (e.RowIndex == 1)
                {
                    if (dgvFilter[1, e.RowIndex].Value.ToString().ToLower() == "true")
                        myIPModule.MyBlocks[e.RowIndex].isEnable = 1;
                    else
                        myIPModule.MyBlocks[e.RowIndex].isEnable = 0;
                }
                else if (e.ColumnIndex == 2)
                {
                    string strEnable = "00000";
                    string str1 = "0";
                    string str2 = "0";
                    if (dgvFilter[5, e.RowIndex].Value.ToString().ToLower() == "true") str1 = "1";
                    if (dgvFilter[7, e.RowIndex].Value.ToString().ToLower() == "true") str2 = "1";
                    if (dgvFilter[2, e.RowIndex].Value.ToString().ToLower() == "true")
                    {
                        strEnable = strEnable + str2 + str1 + "1";
                    }
                    else
                    {
                        strEnable = strEnable + str2 + str1 + "0";
                    }
                    myIPModule.MyBlocks[e.RowIndex].blockType = Convert.ToByte(GlobalClass.BitToInt(strEnable));
                }
                else if (e.ColumnIndex == 3)
                {
                    string strTmp = dgvFilter[3, e.RowIndex].Value.ToString();
                    myIPModule.MyBlocks[e.RowIndex].sourceSubnetID = byte.Parse(HDLPF.IsNumStringMode(strTmp, 0, 255));
                }
                else if (e.ColumnIndex == 4)
                {
                    string strTmp = dgvFilter[4, e.RowIndex].Value.ToString();
                    myIPModule.MyBlocks[e.RowIndex].sourceDeviceID = byte.Parse(HDLPF.IsNumStringMode(strTmp, 0, 255));
                }
                else if (e.ColumnIndex == 5)
                {
                    string strEnable = "00000";
                    string str1 = "0";
                    string str2 = "0";
                    if (dgvFilter[2, e.RowIndex].Value.ToString().ToLower() == "true") str1 = "1";
                    if (dgvFilter[7, e.RowIndex].Value.ToString().ToLower() == "true") str2 = "1";
                    if (dgvFilter[5, e.RowIndex].Value.ToString().ToLower() == "true")
                    {
                        strEnable = strEnable + str2 + "1" + str1;
                    }
                    else
                    {
                        strEnable = strEnable + str2 + "0" + str1;
                    }
                    myIPModule.MyBlocks[e.RowIndex].blockType = Convert.ToByte(GlobalClass.BitToInt(strEnable));
                }
                else if (e.ColumnIndex == 6)
                {
                    string strTmp = dgvFilter[6, e.RowIndex].Value.ToString();
                    myIPModule.MyBlocks[e.RowIndex].command = Convert.ToInt32(strTmp, 16);
                }
                else if (e.ColumnIndex == 7)
                {
                    string strEnable = "00000";
                    string str1 = "0";
                    string str2 = "0";
                    if (dgvFilter[2, e.RowIndex].Value.ToString().ToLower() == "true") str1 = "1";
                    if (dgvFilter[5, e.RowIndex].Value.ToString().ToLower() == "true") str2 = "1";
                    if (dgvFilter[7, e.RowIndex].Value.ToString().ToLower() == "true")
                    {
                        strEnable = strEnable + "1" + str2 + str1;
                    }
                    else
                    {
                        strEnable = strEnable + "0" + str2 + str1;
                    }
                    myIPModule.MyBlocks[e.RowIndex].blockType = Convert.ToByte(GlobalClass.BitToInt(strEnable));
                }
                else if (e.ColumnIndex == 8)
                {
                    string strTmp = dgvFilter[8, e.RowIndex].Value.ToString();
                    myIPModule.MyBlocks[e.RowIndex].destSubnetID = byte.Parse(HDLPF.IsNumStringMode(strTmp, 0, 255));
                }
                else if (e.ColumnIndex == 9)
                {
                    string strTmp = dgvFilter[9, e.RowIndex].Value.ToString();
                    myIPModule.MyBlocks[e.RowIndex].destDeviceID = byte.Parse(HDLPF.IsNumStringMode(strTmp, 0, 255));
                }
            }
            catch
            {
            }
        }

        private void btnRef2_Click(object sender, EventArgs e)
        {
            if (tabControl.SelectedTab.Name == "tabKeys")
            {
                if (networkinfo != null) networkinfo.btnRead_Click(null,null);
                if (RemoteConnection != null) RemoteConnection.readConnetionInfomation();
            }
            else if (tabControl.SelectedTab.Name == "tabRF")
            {
                tslRead_Click(tslRead, null);
            }
            else if (tabControl.SelectedTab.Name == "tabRfButton")
            {
                if (myIPModule == null) return;
                if (myIPModule.MyRemoteControllers == null) return;
                if (cboController.SelectedIndex == -1) return;
                Byte RemoteControllerID = (Byte)cboController.SelectedIndex;
                Byte PageID = (Byte)cboPages.SelectedIndex;

                Byte[] arayKeyMode = myIPModule.ReadButtonModeFrmDeviceToBuf(RemoteControllerID, SubNetID, DeviceID);

                for (Byte bytI = 0; bytI < IPmoduleDeviceTypeList.HowManyButtonsEachPage; bytI++)
                {
                    myIPModule.MyRemoteControllers[RemoteControllerID * IPmoduleDeviceTypeList.HowManyButtonsEachPage + bytI].Mode = arayKeyMode[bytI];
                }

                for (Byte bytI = 0; bytI < 8; bytI++)
                {
                    myIPModule.MyRemoteControllers[RemoteControllerID * IPmoduleDeviceTypeList.HowManyButtonsEachPage + PageID * 8 + bytI].ReadButtonRemark(SubNetID, DeviceID, mywdDevicerType, RemoteControllerID);
                }

                cboPages_SelectedIndexChanged(cboPages, null);
            }
        }

        private void btnSave2_Click(object sender, EventArgs e)
        {
            if (tabControl.SelectedTab.Name == "tabRF")
            {
                tslRead_Click(toolStripLabel2, null);
            }
            else if (tabControl.SelectedTab.Name == "tabRfButton")
            {
                if (myIPModule == null) return;
                if (myIPModule.MyRemoteControllers == null) return;
                if (cboController.SelectedIndex == -1) return;
                Byte RemoteControllerID = (Byte)cboController.SelectedIndex;
                Byte PageID = (Byte)cboPages.SelectedIndex;

                for (Byte bytI = 0; bytI < 8; bytI++)
                {
                    myIPModule.MyRemoteControllers[RemoteControllerID * IPmoduleDeviceTypeList.HowManyButtonsEachPage + PageID * 8 + bytI].ModifyButtonRemark(SubNetID, DeviceID, mywdDevicerType, RemoteControllerID);
                }

                myIPModule.SaveButtonModeToDeviceFrmBuf(RemoteControllerID, SubNetID, DeviceID, mywdDevicerType);
            }
        }

        private void btnSaveAndClose2_Click(object sender, EventArgs e)
        {
            btnSave2_Click(btnSave2, null);
        }

        private void dgvFilter_Scroll(object sender, ScrollEventArgs e)
        {
            txtCommand.Visible = false;
        }

        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CsConst.MyEditMode == 1)
            {
                MyActivePage = tabControl.SelectedIndex + 1;
                #region
                switch ( tabControl.SelectedTab.Name)
                {
                    case "tabKeys":
                        if (myIPModule.MyRead2UpFlags[0] == false)
                        {
                            DisplaySettingToForm();
                        }
                        break;
                    case "tabRF":
                        if (myIPModule.MyRead2UpFlags[1] == false)
                        {
                            if (gbNetwork.Visible == true)
                                btnReadNetwork_Click(btnReadNetwork, null);
                            tslRead_Click(tslRead, null);
                        }
                        else
                        {
                            if (gbNetwork.Visible == true)
                                btnReadNetwork_Click(btnReadNetwork, null);
                            ShowFilterInfomation();
                        }
                        break;
                    case "tabRfButton":
                        if (myIPModule.MyRead2UpFlags[2] == false)
                        {
                            tslRead_Click(tslRead, null);
                        }
                        else
                        {
                            ShowRFButtonsInformation();
                        }
                        break;
                }
                #endregion
            }
        }

        void ShowRFButtonsInformation()
        {
            NumAddr1.Value = myIPModule.RemoteControllers[0] * 256 + myIPModule.RemoteControllers[1];
            NumAddr2.Value = myIPModule.RemoteControllers[2] * 256 + myIPModule.RemoteControllers[3];
            NumAddr3.Value = myIPModule.RemoteControllers[4] * 256 + myIPModule.RemoteControllers[5];
            NumAddr4.Value = myIPModule.RemoteControllers[6] * 256 + myIPModule.RemoteControllers[7];

            if (cboController.SelectedIndex == -1) cboController.SelectedIndex = 0;
            myIPModule.MyRead2UpFlags[2] = true;
        }

        void ShowFilterInfomation()
        {
            try
            {
                if (myIPModule == null) return;
                if (myIPModule.MyBlocks == null) return;
                isRead = true;
                dgvFilter.Rows.Clear();
                for (int i = 0; i < myIPModule.MyBlocks.Count; i++)
                {
                    RFBlock temp = myIPModule.MyBlocks[i];
                    string strEnable = GlobalClass.AddLeftZero(Convert.ToString(temp.blockType, 2), 8);

                    object[] obj = new object[] { dgvFilter.RowCount + 1, temp.isEnable==1,strEnable.Substring(7,1)=="1",
                                                  temp.sourceSubnetID.ToString(),temp.sourceDeviceID.ToString(), 
                                                  strEnable.Substring(6,1)=="1",GlobalClass.AddLeftZero(temp.command.ToString("X"),4),
                                                  strEnable.Substring(5,1)=="1",temp.destSubnetID.ToString(),temp.destDeviceID.ToString(), };
                    dgvFilter.Rows.Add(obj);
                }
                myIPModule.MyRead2UpFlags[1] = true;
            }
            catch
            {
                isRead = false;
            }
            isRead = false;
        }

        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
           
        }

        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            
        }

        private void btnRead_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                byte[] ArayTmp = new byte[0];
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1D34, SubNetID, DeviceID, false, true, true, false) == true)
                {
                    myIPModule.bytStatus = CsConst.myRevBuf[25];
                    myIPModule.intSSID = CsConst.myRevBuf[28] * 256 + CsConst.myRevBuf[27];
                    myIPModule.bytGateSubID = CsConst.myRevBuf[29];
                    myIPModule.bytGateDevID = CsConst.myRevBuf[30];
                    myIPModule.bytCFre = CsConst.myRevBuf[31];
                    myIPModule.bytChns = CsConst.myRevBuf[32];
                    myIPModule.bytPWD = new byte[16];
                    Array.Copy(CsConst.myRevBuf, 33, myIPModule.bytPWD, 0, 16);
                    CsConst.myRevBuf = new byte[1200];
                }
                DisplayRFSettingToForm();
            }
            catch
            {
            }
            Cursor.Current = Cursors.Default;
        }

        void DisplayRFSettingToForm()
        {
            try
            {
                tbSSID.Text = myIPModule.intSSID.ToString();
                if (myIPModule.bytCFre >= 0 && myIPModule.bytCFre <= 2)
                    cboCF.SelectedIndex = myIPModule.bytCFre;
                if (myIPModule.bytChns >= 0 && myIPModule.bytChns <= 12)
                    cboChn.SelectedIndex = myIPModule.bytChns;
                tbPSK.Text = HDLPF.Byte2String(myIPModule.bytPWD);
                if (myIPModule.bytStatus == 1)
                {
                    if (CsConst.iLanguageId == 1) lbRfState.Text = "配置状态";
                    else if (CsConst.iLanguageId == 0) lbRfState.Text = "Configuration Status";
                }
                else
                {
                    if (CsConst.iLanguageId == 1) lbRfState.Text = "正常状态";
                    else if (CsConst.iLanguageId == 0) lbRfState.Text = "Normal Status";
                }
            }
            catch
            {
            }
        }

        private void btnModify_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            btnModify.Enabled = false;
            byte[] arayTmp = new byte[0];
            if (CsConst.mySends.AddBufToSndList(arayTmp, 0x1D38, SubNetID, DeviceID, false, true, true, false) == true)
            {
                myIPModule.bytGateSubID = SubNetID;
                myIPModule.bytGateDevID = DeviceID;

                arayTmp = new byte[23];
                arayTmp[0] = 1;
                arayTmp[1] = (byte)(myIPModule.intSSID % 256);
                arayTmp[2] = (byte)(myIPModule.intSSID / 256);
                arayTmp[3] = myIPModule.bytGateSubID;
                arayTmp[4] = myIPModule.bytGateDevID;
                arayTmp[5] = myIPModule.bytCFre;
                arayTmp[6] = myIPModule.bytChns;
                if (myIPModule.bytPWD != null) Array.Copy(myIPModule.bytPWD, 0, arayTmp, 7, myIPModule.bytPWD.Length);

                if (CsConst.mySends.AddBufToSndList(arayTmp, 0x1D36, SubNetID, DeviceID, false, true, true, false) == false)
                {

                }
            }
            btnModify.Enabled = true;
            Cursor.Current = Cursors.Default;
        }

        private void cboCF_SelectedIndexChanged(object sender, EventArgs e)
        {
            myIPModule.bytCFre = Convert.ToByte(((ComboBox)sender).SelectedIndex);
            cboChn.Items.Clear();
            object[] obj = null;
            switch (myIPModule.bytCFre)
            {
                case 0: obj = new object[] {"780MHz","782MHz","784MHz","786MHz","770MHz(Non-standard)","772MHz(Non-standard)","774MHz(Non-standard)","776MHz(Non-standard)",
                                         "778MHz(Non-standard)","788MHz(Non-standard)","790MHz(Non-standard)","792MHz(Non-standard)","794MHz(Non-standard)"}; break;
                case 1: obj = new object[] {"864MHz","866MHz","868MHz","870MHz","858MHz(Non-standard)","860MHz(Non-standard)","862MHz(Non-standard)","872MHz(Non-standard)",
                                         "874MHz(Non-standard)","876MHz(Non-standard)","878MHz(Non-standard)","880MHz(Non-standard)","882MHz(Non-standard)"}; break;
                case 2: obj = new object[] { "904MHz","906MHz","908MHz","910MHz","912MHz","914MHz","916MHz","918MHz",
                                         "920MHz","922MHz","924MHz","926MHz","928MHz" }; break;
            }
            cboChn.Items.AddRange(obj);
            if (myIPModule.bytChns <= 12)
                cboChn.SelectedIndex = myIPModule.bytChns;
            myIPModule.MyRead2UpFlags[2] = false;
        }

        private void cboNetwork_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbThrough.Visible = (cboNetwork.SelectedIndex == 0);
            lbThrough.Visible = (cboNetwork.SelectedIndex == 0);
            if (isRead) return;
            if (myIPModule == null) return;
            myIPModule.isEnable = Convert.ToByte(cboNetwork.SelectedIndex);
        }

        private void cbThrough_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            if (myIPModule == null) return;
            myIPModule.bytPassSub = Convert.ToByte(cbThrough.SelectedIndex + 1);
        }

        private void btnReadNetwork_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                byte[] ArayTmp = new byte[0];
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1D46, SubNetID, DeviceID, false, true, true, false) == true)
                {
                    myIPModule.isEnable = CsConst.myRevBuf[25];
                    myIPModule.bytPassSub = CsConst.myRevBuf[26];
                    if (myIPModule.bytPassSub == 0) myIPModule.bytPassSub = 255;
                    CsConst.myRevBuf = new byte[1200];
                }

                cboNetwork.SelectedIndex = myIPModule.isEnable;
                cbThrough.SelectedIndex = cbThrough.Items.IndexOf(myIPModule.bytPassSub.ToString());
            }
            catch
            {
            }
            Cursor.Current = Cursors.Default;
        }

        private void btnSaveNetwork_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                Byte[] arayTmp = new byte[2];
                arayTmp[0] = myIPModule.isEnable;
                arayTmp[1] = myIPModule.bytPassSub;
                if (CsConst.mySends.AddBufToSndList(arayTmp, 0x1D48, SubNetID, DeviceID, false, true, true, false) == false)
                {
                }
            }
            catch 
            { }
            Cursor.Current = Cursors.Default;
        }

        private void frmIPMod_Load(object sender, EventArgs e)
        {
            InitialFormCtrlsTextOrItems();
        }

        private void cboController_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboController.SelectedIndex == -1) return;
            if (myIPModule == null) return;
            if (myIPModule.MyRemoteControllers == null) return;

            if (cboPages.SelectedIndex == -1) cboPages.SelectedIndex = 0;
        }

        private void cboPages_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (myIPModule == null) return;
            if (cboController.SelectedIndex == -1) return;
            if (cboPages.SelectedIndex == -1) return;
            dgvRfbuttons.Rows.Clear();
            Byte ControllerID = Convert.ToByte(cboController.SelectedIndex);
            Byte PageID = Convert.ToByte(cboPages.SelectedIndex);

            for (int i = 0; i < 8; i++)
            {
                HDLButton obutton = myIPModule.MyRemoteControllers[ControllerID * 64 + PageID * 8 + i];
                if (obutton == null) obutton = new HDLButton();
                String ButtonModeString = ButtonMode.ConvertorKeyModeToPublicModeGroup(obutton.Mode);
                int TmpMode = cboButtonMode.Items.IndexOf(ButtonModeString);
                if (TmpMode != -1)
                {
                    Object[] obj = new object[] { (i + 1).ToString(), obutton.Remark, ButtonModeString };
                    dgvRfbuttons.Rows.Add(obj);
                }
                else
                {
                    Object[] obj = new object[] { (i + 1).ToString(), obutton.Remark, cboButtonMode.Items[0] };
                    dgvRfbuttons.Rows.Add(obj);
                }
            }
        }

        private void ReadCurrent_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                byte[] ArayTmp = new byte[0];
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xE2F0, SubNetID, DeviceID, false, true, true, false) == true)
                {
                    lbCurrent.Text = (CsConst.myRevBuf[25] * 256 + CsConst.myRevBuf[26]).ToString();
                    CsConst.myRevBuf = new byte[1200];
                }
            }
            catch
            {
            }
            Cursor.Current = Cursors.Default;
        }

        private void Next_Click(object sender, EventArgs e)
        {
            if (cboPages.SelectedIndex == cboPages.Items.Count - 1) return;
            cboPages.SelectedIndex += 1;
        }

        private void Pre_Click(object sender, EventArgs e)
        {
            if (cboPages.SelectedIndex == 0) return;
            cboPages.SelectedIndex -= 1;
        }

        private void dgvRfbuttons_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == 2)
            {
               // btnMode_Click(btnMode, null);
            }
            else
            {
               // btnCMD_Click(btnCMD, null);
            }
        }

        private void SaveRemarkMode_Click(object sender, EventArgs e)
        {
            if (myIPModule == null) return;
            if (myIPModule.MyRemoteControllers == null) return;
            if (cboController.SelectedIndex == -1) return;
            Byte RemoteControllerID = (Byte)cboController.SelectedIndex;
            Byte PageID = (Byte)cboPages.SelectedIndex;

            for (Byte bytI = 0; bytI < 8; bytI++)
            {
                myIPModule.MyRemoteControllers[RemoteControllerID * IPmoduleDeviceTypeList.HowManyButtonsEachPage +PageID * 8 + bytI].ModifyButtonRemark(SubNetID, DeviceID, mywdDevicerType, RemoteControllerID);
            }

            myIPModule.SaveButtonModeToDeviceFrmBuf(RemoteControllerID, SubNetID, DeviceID, mywdDevicerType);
        }

        private void dgvRfbuttons_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
           
        }

        private void dgvRfbuttons_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnMode_Click(object sender, EventArgs e)
        {
            if (cboPages.SelectedIndex == -1) return;
            if (cboController.SelectedIndex == -1) return;

            Byte[] TmpCurrentSelections = new Byte[] {(Byte)dgvRfbuttons.CurrentCell.RowIndex,(Byte)cboPages.SelectedIndex,(Byte)cboController.SelectedIndex };
            frmButtonSetup ButtonConfigration = new frmButtonSetup(myIPModule, myIPModule.DeviceName, TmpCurrentSelections);
            ButtonConfigration.ShowDialog();
        }

        private void btnCMD_Click(object sender, EventArgs e)
        {
            Byte[] PageID = new Byte[4];
            if (dgvRfbuttons.SelectedRows != null && dgvRfbuttons.SelectedRows.Count != 0)
            {
                PageID[0] = (Byte)dgvRfbuttons.SelectedRows[0].Index;
            }
            PageID[1] = (Byte)cboPages.SelectedIndex;
            PageID[2] = (Byte)cboController.SelectedIndex;
            PageID[3] = 6;

            frmCmdSetup CmdSetup = new frmCmdSetup(myIPModule, myDevName, mywdDevicerType, PageID);
            CmdSetup.ShowDialog();
        }

        private void tbPSK_TextChanged(object sender, EventArgs e)
        {
            byte[] arayTmpRemark = new byte[16];
            if (tbPSK.Text != null)
            {
                arayTmpRemark = HDLUDP.StringToByte(tbPSK.Text);
            }
            myIPModule.bytPWD = new byte[16];
            Array.Copy(arayTmpRemark, 0, myIPModule.bytPWD, 0, arayTmpRemark.Length);
            myIPModule.MyRead2UpFlags[2] = false;
        }

        private void frmIPMod_FormClosing_1(object sender, FormClosingEventArgs e)
        {
            RemoteConnection = null;
            networkinfo = null;
            this.Dispose();
        }

        private void cboChn_SelectedIndexChanged(object sender, EventArgs e)
        {
            myIPModule.bytChns = Convert.ToByte(((ComboBox)sender).SelectedIndex);
        }

        private void btnSaveAddress_Click(object sender, EventArgs e)
        {
            try
            {
                myIPModule.RemoteControllers[0] = (Byte)(NumAddr1.Value / 256);
                myIPModule.RemoteControllers[1] = (Byte)(NumAddr1.Value % 256);

                myIPModule.RemoteControllers[2] = (Byte)(NumAddr2.Value / 256);
                myIPModule.RemoteControllers[3] = (Byte)(NumAddr2.Value % 256);

                myIPModule.RemoteControllers[4] = (Byte)(NumAddr3.Value / 256);
                myIPModule.RemoteControllers[5] = (Byte)(NumAddr3.Value % 256);

                myIPModule.RemoteControllers[6] = (Byte)(NumAddr4.Value / 256);
                myIPModule.RemoteControllers[7] = (Byte)(NumAddr4.Value % 256);

                Cursor.Current = Cursors.WaitCursor;
                byte[] ArayTmp = new byte[11];
                Array.Copy(myIPModule.RemoteControllers, 0, ArayTmp, 3, 8);
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xE012, SubNetID, DeviceID, false, true, true, false) == true)
                {
                    CsConst.myRevBuf = new byte[1200];
                }
            }
            catch
            {
            }
            Cursor.Current = Cursors.Default;
        }

        private void NumAddr1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void dgvRfbuttons_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            dgvRfbuttons.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void dgvRfbuttons_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == -1) return;
            if (e.RowIndex == -1) return;
            if (myIPModule == null) return;
            if (myIPModule.MyRemoteControllers == null) return;
            if (cboController.SelectedIndex == -1) return;
            Byte RemoteControllerID = (Byte)cboController.SelectedIndex;
            Byte PageID = (Byte)cboPages.SelectedIndex;

            String Remark = dgvRfbuttons[e.ColumnIndex, e.RowIndex].Value.ToString();
            if (dgvRfbuttons.SelectedRows.Count > 1)
            {
                for (int i = 0; i < dgvRfbuttons.SelectedRows.Count; i++)
                {
                    dgvRfbuttons.SelectedRows[i].Cells[e.ColumnIndex].Value = Remark;
                    if (e.ColumnIndex == 1) // 备注
                    {
                        myIPModule.MyRemoteControllers[RemoteControllerID * IPmoduleDeviceTypeList.HowManyButtonsEachPage + PageID * 8 + e.RowIndex].Remark = Remark;
                    }
                    else if (e.ColumnIndex == 2) // 按键模式
                    {
                        Byte currentMode = ButtonMode.ConvertorKeyModesToPublicModeGroup(dgvRfbuttons[2, e.RowIndex].Value.ToString());
                        myIPModule.MyRemoteControllers[RemoteControllerID * IPmoduleDeviceTypeList.HowManyButtonsEachPage + PageID * 8 + e.RowIndex].Mode = currentMode;
                    }
                }
            }
        }

    }
}
