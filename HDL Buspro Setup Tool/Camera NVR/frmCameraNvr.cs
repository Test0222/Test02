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
    public partial class frmCameraNvr : Form
    {
        NetworkInForm deviceNetworkInformation = null;

        private Camera oCamera = null;

        private string myDevName = null;
        private int mintIDIndex = -1;
        private int myintDeviceTpye = -1;
        private int MyActivePage = 0; //按页面上传下载

        private byte SubNetID;
        private byte DevID;

         public const Byte userNameLength = 16;

        public frmCameraNvr()
        {
            InitializeComponent();
        }

        public frmCameraNvr(object oCameraOrNvr,string strName,int dIndex,int wdDeviceType)
        {
            InitializeComponent();

            oCamera = (Camera)oCameraOrNvr;

            myDevName = strName;
            mintIDIndex = dIndex;
            myintDeviceTpye = wdDeviceType;

            string strDevName = strName.Split('\\')[0].ToString();

            HDLSysPF.DisplayDeviceNameModeDescription(strName, myintDeviceTpye, cboDevice, tbModel, tbDescription);
            SubNetID = Convert.ToByte(strDevName.Split('-')[0]);
            DevID = Convert.ToByte(strDevName.Split('-')[1]);            
        }

        private void frmCameraNvr_Load(object sender, EventArgs e)
        {
            InitialFormCtrlsTextOrItems();
        }

        void InitialFormCtrlsTextOrItems()
        {
        }

        void SetCtrlsVisbleWithDifferentDeviceType()
        {
            toolStrip1.Visible = (CsConst.MyEditMode == 0);
            panel2.Controls.Clear();
            deviceNetworkInformation = new NetworkInForm(SubNetID, DevID, myintDeviceTpye);
            panel2.Controls.Add(deviceNetworkInformation);
            deviceNetworkInformation.Dock = DockStyle.Fill;
            deviceNetworkInformation.BringToFront();
            deviceNetworkInformation.Visible = true;

            if (CameraNvrDeviceType.nvrThridPartDeviceType.Contains(myintDeviceTpye)) // network vedio recorder 
                tabControl1.TabPages.Remove(tabCamera);
            else
                tabControl1.TabPages.Remove(tabNvr);
        }

        private void frmCameraNvr_Shown(object sender, EventArgs e)
        {
            SetCtrlsVisbleWithDifferentDeviceType();
            if (CsConst.MyEditMode == 0)
            {
               // DisplaySettingToForm();
            }
            else if (CsConst.MyEditMode == 1) //在线模式
            {
                btnRead_Click(btnRef1, null);
            }
        }

        private void btnRead_Click(object sender, EventArgs e)
        {
            if (oCamera == null) return;
            Cursor.Current = Cursors.WaitCursor;
            ((sender) as Button).Enabled = false;
            if (tabControl1.SelectedTab.Name == "tabCamera")
            {
                #region               
                if (oCamera == null) return;
                byte[] Tmp = new byte[] { CameraNvrDeviceType.cameraNvrBigType, CameraNvrDeviceType.cameraSmallType, 1 };
                if (CsConst.mySends.AddBufToSndList(Tmp, 0xE464, SubNetID, DevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(myintDeviceTpye)) == true)
                {
                    byte[] arayRemark = new byte[32];
                    Array.Copy(CsConst.myRevBuf, 30, arayRemark, 0, 16);
                    oCamera.UserName = HDLPF.Byte2String(arayRemark);

                    Array.Copy(CsConst.myRevBuf, 46, arayRemark, 0, 32);
                    oCamera.Password = HDLPF.Byte2String(arayRemark);

                    CsConst.myRevBuf = new byte[1200];
                }

                userNameCtrl.Text = oCamera.UserName;
                passwordCtrl.Text = oCamera.Password;
                #endregion                
            }
            else if (tabControl1.SelectedTab.Name == "tabNvr")
            {
                #region
                //first read its channel number
                Byte totalChannel = 0;
                Byte[] sendBuffer = new byte[2];
                if (CsConst.mySends.AddBufToSndList(sendBuffer, 0xE548, SubNetID, DevID, false, false, true, CsConst.minAllWirelessDeviceType.Contains(myintDeviceTpye)) == true)
                {
                    totalChannel = CsConst.myRevBuf[49];
                }

                if (totalChannel == 0) return;
                sendBuffer = new byte[3];
                ((Nvr)oCamera).cameraLists = new List<string[]>();
                for (Byte i = 1; i <= totalChannel; i++)
                {
                    String[] CameraUnderIt = new String[5]{"0.0.0.0","0","1","",""};
                    sendBuffer =new Byte[]{CameraNvrDeviceType.cameraNvrBigType, CameraNvrDeviceType.nvrSmallType, i};
                    if (CsConst.mySends.AddBufToSndList(sendBuffer, 0xE464, SubNetID, DevID, false, false, true, CsConst.minAllWirelessDeviceType.Contains(myintDeviceTpye)) == true)
                    {
                        String ipAddress = String.Format("{0}.{1}.{2}.{3}", CsConst.myRevBuf[31], CsConst.myRevBuf[32],
                                              CsConst.myRevBuf[33], CsConst.myRevBuf[34]);
                        int port = CsConst.myRevBuf[35] * 256 + CsConst.myRevBuf[36];
                        Byte loopNumber = CsConst.myRevBuf[37];

                        byte[] arayRemark = new byte[16];
                        Array.Copy(CsConst.myRevBuf, 38, arayRemark, 0, 16);
                        String UserName = HDLPF.Byte2String(arayRemark);

                        Array.Copy(CsConst.myRevBuf, 55, arayRemark, 0, userNameLength);
                        String Password = HDLPF.Byte2String(arayRemark);

                        CameraUnderIt[0] = ipAddress;
                        CameraUnderIt[1] = port.ToString();
                        CameraUnderIt[2] = loopNumber.ToString();
                        CameraUnderIt[3] = UserName;
                        CameraUnderIt[4] = Password;
                        ((Nvr)oCamera).cameraLists.Add(CameraUnderIt);
                        CsConst.myRevBuf = new byte[1200];
                    }
                    else
                    {
                        for (Byte j = i; j <= totalChannel; j++)
                        {
                            ((Nvr)oCamera).cameraLists.Add(CameraUnderIt);
                        }
                        break;
                    }
                }
                #endregion
            }

            UpdateDisplayInformationAccordingly();
            Cursor.Current = Cursors.Default;
            ((sender) as Button).Enabled = true;
        }

        private void UpdateDisplayInformationAccordingly()
        {
            if (tabControl1.SelectedTab.Name == "tabCamera")
            { }
            else
            {
                DisplayCameraListUnderNVR();
            }
        }

        private void DisplayCameraListUnderNVR()
        {
            if (((Nvr)oCamera).cameraLists == null) return;

            cameraListCtrl.Rows.Clear();
            int ID = 1;
            foreach(String[] Tmp in ((Nvr)oCamera).cameraLists)
            {
                Object[] obj = new Object[] { ID.ToString(), Tmp[0], Tmp[1], Tmp[2], Tmp[3],Tmp[4] };
                cameraListCtrl.Rows.Add(obj);
                ID++;
            }
        }

        private void btnModify_Click(object sender, EventArgs e)
        {
            if (oCamera == null) return;

            Cursor.Current = Cursors.WaitCursor;
            ((sender) as Button).Enabled = false;

            if (tabControl1.SelectedTab.Name == "tabCamera")
            {
                #region
                if (userNameCtrl.Text == null) userNameCtrl.Text = "";
                if (passwordCtrl.Text == null) passwordCtrl.Text = "";

                oCamera.UserName = userNameCtrl.Text;
                oCamera.Password = passwordCtrl.Text;
                Byte[] userNamePasswordBuffer = new byte[53];

                byte[] Tmp = new byte[] { CameraNvrDeviceType.cameraNvrBigType, CameraNvrDeviceType.cameraSmallType, 1, 1, 7 };
                Tmp.CopyTo(userNamePasswordBuffer, 0);

                byte[] arayTmpRemark = HDLUDP.StringToByte(oCamera.UserName);
                // user name
                #region
                if (arayTmpRemark.Length > userNameLength)
                {
                    Array.Copy(arayTmpRemark, 0, userNamePasswordBuffer, 5, userNameLength);
                }
                else
                {
                    Array.Copy(arayTmpRemark, 0, userNamePasswordBuffer, 5, arayTmpRemark.Length);
                }
                #endregion

                arayTmpRemark = HDLUDP.StringToByte(oCamera.Password);
                //password
                #region
                if (arayTmpRemark.Length > userNameLength)
                {
                    Array.Copy(arayTmpRemark, 0, userNamePasswordBuffer, 21, userNameLength);
                }
                else
                {
                    Array.Copy(arayTmpRemark, 0, userNamePasswordBuffer, 21, arayTmpRemark.Length);
                }
                #endregion
                if (CsConst.mySends.AddBufToSndList(userNamePasswordBuffer, 0xE466, SubNetID, DevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(myintDeviceTpye)) == true)
                {
                }
                #endregion
            }
            else if (tabControl1.SelectedTab.Name == "tabNvr")
            {
                #region
                //first read its channel number
                Byte totalChannel = (Byte)cameraListCtrl.Rows.Count;
                Byte[] sendBuffer = new byte[2];

                if (totalChannel == 0) return;
                sendBuffer = new byte[60];
                sendBuffer[0] = CameraNvrDeviceType.cameraNvrBigType;
                sendBuffer[1] = CameraNvrDeviceType.nvrSmallType;
                sendBuffer[3] = 1;
                sendBuffer[4] = 6; // NVR配置
                for (Byte i = 0; i < totalChannel; i++)
                {
                    sendBuffer[2] = (Byte)(i + 1);
                    String[] ipAddress = cameraListCtrl[1, i].Value.ToString().Split('.').ToArray();
                    sendBuffer[5] = Convert.ToByte(ipAddress[0]);
                    sendBuffer[6] = Convert.ToByte(ipAddress[1]);
                    sendBuffer[7] = Convert.ToByte(ipAddress[2]);
                    sendBuffer[8] = Convert.ToByte(ipAddress[3]);

                    sendBuffer[9] = Convert.ToByte(Convert.ToInt16(cameraListCtrl[2, i].Value.ToString()) / 256);
                    sendBuffer[10] = Convert.ToByte(Convert.ToInt16(cameraListCtrl[2, i].Value.ToString()) % 256);

                    sendBuffer[11] = Convert.ToByte(cameraListCtrl[3, i].Value.ToString());

                    byte[] arayTmpRemark = HDLUDP.StringToByte(cameraListCtrl[4, i].Value.ToString());
                    // user name
                    #region
                    if (arayTmpRemark.Length != 0)
                    {
                        if (arayTmpRemark.Length > 16)
                        {
                            Array.Copy(arayTmpRemark, 0, sendBuffer, 12, 16);
                        }
                        else
                        {
                            Array.Copy(arayTmpRemark, 0, sendBuffer, 12, arayTmpRemark.Length);
                        }
                    }
                    #endregion

                    arayTmpRemark = HDLUDP.StringToByte(cameraListCtrl[5, i].Value.ToString());
                    //password
                    #region
                    if (arayTmpRemark.Length != 0)
                    {
                        if (arayTmpRemark.Length > userNameLength)
                        {
                            Array.Copy(arayTmpRemark, 0, sendBuffer,28, userNameLength);
                        }
                        else
                        {
                            Array.Copy(arayTmpRemark, 0, sendBuffer, 28, arayTmpRemark.Length);
                        }
                    }
                    #endregion

                    if (CsConst.mySends.AddBufToSndList(sendBuffer, 0xE466, SubNetID, DevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(myintDeviceTpye)) == true)
                    {

                    }
                    else break;
                }
                #endregion
            }

           Cursor.Current = Cursors.Default;
           ((sender) as Button).Enabled = true;
        }

        private void saveNvrCtrl_Click(object sender, EventArgs e)
        {
            if (oCamera == null) return;

            Cursor.Current = Cursors.WaitCursor;
            ((sender) as Button).Enabled = false;

           

            Cursor.Current = Cursors.Default;
            ((sender) as Button).Enabled = true;
        }

        private void btnSaveAndClose2_Click(object sender, EventArgs e)
        {
            //saveNvrCtrl_Click(btnSave2, null);
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            btnModify_Click(btnSave1, null);
            this.Close();
        }

        private void cameraListCtrl_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            Form frmTmp = null;

            foreach (Form frm in Application.OpenForms)
            {
                if (frm.Name == "frmTSensor")
                {
                    frmTmp = frm;
                    break;
                }
            }

            if (frmTmp == null)
            {
               // frmTmp = new frmTSensor(myHVAC, 1, 0);
                //frmTmp.Show();
            }
            else
            {
                frmTmp.BringToFront();
            }
        }

        private void frmCameraNvr_FormClosing(object sender, FormClosingEventArgs e)
        {
            deviceNetworkInformation = null;
            this.Dispose();
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            bool isOpen = HDLSysPF.IsAlreadyOpenedForm("frmCamera");

            if (isOpen == false)
            {
                frmCamera frmTest = new frmCamera(oCamera, cameraListCtrl.Rows.Count,0);
                frmTest.Show();
            }
        }
    }
}
