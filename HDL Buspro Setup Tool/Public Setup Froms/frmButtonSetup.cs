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
    public partial class frmButtonSetup : Form
    {
        private string MyDeviceName = null;
        private Object MyActiveObject = null;
        private int mywdDevicerType = -1;
        private byte SubNetID;
        private byte DeviceID;

        private Byte[] PageAndButtonID; // 按键号 页号 遥控器号

        public frmButtonSetup()
        {
            InitializeComponent();
        }

        public frmButtonSetup(object TmpObject, String DeviceName, Byte[] PageOrRemoteController)
        {
            InitializeComponent();

            MyActiveObject = TmpObject;
            MyDeviceName = DeviceName;
            if (PageOrRemoteController != null)
            {
                PageAndButtonID = PageOrRemoteController;
            }
            else
            {
                PageAndButtonID = new Byte[4];
            }
        }

        private void frmUpgrade_Load(object sender, EventArgs e)
        {
            if (CsConst.myOnlines == null) return;
            HDLSysPF.LoadDeviceListsWithDifferentSensorType(cboDevA, 3);
            cboDevA.SelectedIndex = cboDevA.Items.IndexOf(MyDeviceName);
        }

        void InitialCtrlsDisplay()
        {
        }

        private void btnReadA_Click(object sender, EventArgs e)
        {
            HDLSysPF.LoadDeviceListsWithDifferentSensorType(cboDevA, 3);
        }

        private void cboDevA_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboDevA.SelectedIndex == -1) return;
            if (cboDevA.Text == null || cboDevA.Text == "") return;

            MyDeviceName = cboDevA.Text;

            foreach (DevOnLine oDev in CsConst.myOnlines)
            {
                if (oDev.DevName == MyDeviceName)
                {
                    SubNetID = oDev.bytSub;
                    DeviceID = oDev.bytDev;
                    mywdDevicerType = oDev.DeviceType;
                    MyActiveObject = HDLSysPF.FindRightObjectAccordingItsDeviceType(MyDeviceName, mywdDevicerType, oDev.intDIndex);
                    break;
                }
            }
            //设置页面显示
            SetCtrlsVisibleAccordinglyDeviceType();
            //正常显示处理
            FurtherDisplayWithVisibleControls();
        }
        /// <summary>
        /// 页面显示
        /// </summary>
        void SetCtrlsVisibleAccordinglyDeviceType()
        {
            if (IPmoduleDeviceTypeList.RFIpModuleV2.Contains(mywdDevicerType)) // 无线网关设备
            {
                #region
                clLed.Visible = false;
                clLink.Visible = false;
                clMutux.Visible = false;
                DryDim.Visible = false;
                #endregion
            }
            else if (DLPPanelDeviceTypeList.HDLDLPPanelDeviceTypeList.Contains(mywdDevicerType)) // DLP
            {
                lbController.Visible = false;
                cboController.Visible = false;
                DryDim.Visible = false;
                panel2.Visible = true;
                clMutux.Items.Clear();
                String[] arrString = new String[]{CsConst.mstrInvalid,"1","2","3","4"};
                clMutux.Items.AddRange(arrString); 
            }
            else if (NormalPanelDeviceTypeList.HDLNormalPanelDeviceTypeList.Contains(mywdDevicerType)) // 普通面板
            {
                lbController.Visible = false;
                cboController.Visible = false;
                panel2.Visible = false;
                DryDim.Visible = false;
                clLink.Visible = !NormalPanelDeviceTypeList.WirelessSimpleFloorHeatingDeviceTypeList.Contains(mywdDevicerType); // 地热面板
            }
            else if (MS04DeviceTypeList.HDLMS04DeviceTypeList.Contains(mywdDevicerType) 
                  || HotelMixModuleDeviceType.HDLRCUDeviceTypeLists.Contains(mywdDevicerType)) // 是不是干结点
            {
                lbController.Visible = false;
                cboController.Visible = false;
                panel2.Visible = false;
                clLed.Visible = false;
                clLink.Visible = false;
                clMutux.Visible = false;
                ClDim.Visible = false;
            }
        }

        /// <summary>
        /// 进一步界面处理
        /// </summary>
        void FurtherDisplayWithVisibleControls()
        {
            try
            {
                if (cboController.Visible == true)
                {
                    if (cboController.SelectedIndex == -1) cboController.SelectedIndex = PageAndButtonID[1];
                }
                else if (cboPages.Visible == true)
                {
                    if (cboPages.SelectedIndex == -1) cboPages.SelectedIndex = PageAndButtonID[0];
                }
                else //直接显示按键信息
                {
                    DisplayButtonParametersToGridTable();
                }
            }
            catch
            {
 
            }
        }

        
        /// <summary>
        /// 不同类型显示到界面
        /// </summary>
        void DisplayButtonParametersToGridTable()
        {
            if (MyActiveObject is IPModule)
            {
                #region
                IPModule TmpIpModule = (IPModule)MyActiveObject;
                if (TmpIpModule == null) return;
                if (cboController.SelectedIndex == -1) return;
                if (cboPages.SelectedIndex == -1) return;
                dgvListA.Rows.Clear();
                Byte ControllerID = Convert.ToByte(cboController.SelectedIndex);
                Byte PageID = Convert.ToByte(cboPages.SelectedIndex);

                for (int i = 0; i < 8; i++)
                {
                    HDLButton obutton = TmpIpModule.MyRemoteControllers[ControllerID * 64 + PageID * 8 + i];
                    if (obutton == null) obutton = new HDLButton();
                    String ButtonModeTmp = ButtonMode.ConvertorKeyModeToPublicModeGroup(obutton.Mode);
                    Object[] obj = new object[] { (i + 1).ToString(), obutton.Remark,ButtonModeTmp, (obutton.IsLEDON ==1),(obutton.IsDimmer ==1),
                                                  (obutton.SaveDimmer ==1),clMutux.Items[obutton.bytMutex],clLink.Items[obutton.byteLink]};
                    dgvListA.Rows.Add(obj);
                }
                #endregion
            }
            else if (MyActiveObject is DLP) // 
            {
                Panel TmpPanel = (Panel)MyActiveObject;
                if (TmpPanel == null) return;

                dgvListA.Rows.Clear();
                Byte PageID = Convert.ToByte(cboPages.SelectedIndex);
                int wdMaxValue = int.Parse(CsConst.mstrINIDefault.IniReadValue("DeviceType" + mywdDevicerType.ToString(), "MaxValue", "0"));
                for (int i = 0; i < 8; i++)
                {
                    HDLButton obutton = TmpPanel.PanelKey[PageID * 8 + i];
                    if (obutton == null) obutton = new HDLButton();
                    String ButtonModeTmp = ButtonMode.ConvertorKeyModeToPublicModeGroup(obutton.Mode);
                    Object[] obj = new object[] { (i + 1).ToString(), obutton.Remark,ButtonModeTmp, (obutton.IsLEDON ==0),(obutton.IsDimmer ==1),
                                                  (obutton.SaveDimmer ==1),clMutux.Items[obutton.bytMutex],clLink.Items[obutton.byteLink]};
                    dgvListA.Rows.Add(obj);
                }

            }
            else if (MyActiveObject is Panel)
            {
                #region
                Panel TmpPanel = (Panel)MyActiveObject;
                if (TmpPanel == null) return;

                dgvListA.Rows.Clear();

                for (int i = 0; i < TmpPanel.PanelKey.Count; i++)
                {
                    HDLButton obutton = TmpPanel.PanelKey[i];
                    if (obutton == null) obutton = new HDLButton();
                    String ButtonModeTmp = ButtonMode.ConvertorKeyModeToPublicModeGroup(obutton.Mode);
                    if (obutton.bytMutex >= clMutux.Items.Count) obutton.bytMutex = (Byte)(clMutux.Items.Count - 1);
                    Object[] obj = new object[] { (i + 1).ToString(), obutton.Remark,ButtonModeTmp, (obutton.IsLEDON ==0),(obutton.IsDimmer ==1),
                                                  (obutton.SaveDimmer ==1),clMutux.Items[obutton.bytMutex],clLink.Items[obutton.byteLink]};
                    dgvListA.Rows.Add(obj);
                }
                #endregion
            }
            else if (MyActiveObject is MS04)
            {
                #region
                MS04 TmpPanel = (MS04)MyActiveObject;
                if (TmpPanel == null) return;

                dgvListA.Rows.Clear();

                for (int i = 0; i < TmpPanel.MSKeys.Count; i++)
                {
                    HDLButton obutton = TmpPanel.MSKeys[i];
                    if (obutton == null) obutton = new HDLButton();
                    String ButtonModeTmp = DryMode.ConvertorKeyModeToPublicModeGroup(obutton.Mode);
                    Object[] obj = new object[] { (i + 1).ToString(), obutton.Remark,ButtonModeTmp, (obutton.IsLEDON ==0),(obutton.IsDimmer ==1),
                                                  (obutton.SaveDimmer ==1),clMutux.Items[obutton.bytMutex],clLink.Items[obutton.byteLink],DryDim.Items[obutton.IsDimmer]};
                    dgvListA.Rows.Add(obj);
                }
                #endregion
            }
            else if (MyActiveObject is MHRCU)
            {
                #region
                MHRCU TmpPanel = (MHRCU)MyActiveObject;
                if (TmpPanel == null) return;

                dgvListA.Rows.Clear();

                for (int i = 0; i < TmpPanel.MSKeys.Count; i++)
                {
                    HDLButton obutton = TmpPanel.MSKeys[i];
                    if (obutton == null) obutton = new HDLButton();
                    String ButtonModeTmp = DryMode.ConvertorKeyModeToPublicModeGroup(obutton.Mode);
                    Object[] obj = new object[] { (i + 1).ToString(), obutton.Remark,ButtonModeTmp, (obutton.IsLEDON ==0),(obutton.IsDimmer ==1),
                                                  (obutton.SaveDimmer ==1),clMutux.Items[obutton.bytMutex],clLink.Items[obutton.byteLink],DryDim.Items[obutton.IsDimmer]};
                    dgvListA.Rows.Add(obj);
                }
                #endregion
            }
        }

        /// <summary>
        /// 刷新当前
        /// </summary>
        void RefreshButtonParametersToDataGrid()
        {
            if (MyActiveObject is IPModule)
            {
                #region
                IPModule TmpIpModule = (IPModule)MyActiveObject;
                if (TmpIpModule == null) return;
                if (TmpIpModule.MyRemoteControllers == null) return;
                if (cboController.SelectedIndex == -1) return;
                Byte RemoteControllerID = (Byte)cboController.SelectedIndex;
                Byte PageID = (Byte)cboPages.SelectedIndex;

                Byte[] ArayButtonDimAndSaveDim = TmpIpModule.ReadButtonDimFlagFrmDeviceToBuf(RemoteControllerID, SubNetID, DeviceID);

                for (Byte bytI = 0; bytI < IPmoduleDeviceTypeList.HowManyButtonsEachPage; bytI++)
                {
                    TmpIpModule.MyRemoteControllers[RemoteControllerID * IPmoduleDeviceTypeList.HowManyButtonsEachPage + bytI].IsDimmer = (Byte)(ArayButtonDimAndSaveDim[bytI] >> 4);
                    TmpIpModule.MyRemoteControllers[RemoteControllerID * IPmoduleDeviceTypeList.HowManyButtonsEachPage + bytI].SaveDimmer = (Byte)(ArayButtonDimAndSaveDim[bytI] & 0x01);
                }
                cboPages_SelectedIndexChanged(cboPages, null);
                #endregion
            }
            else if (MyActiveObject is Panel)
            {
                #region
                Panel TmpPanel = (Panel)MyActiveObject;
                if (TmpPanel == null) return;
                if (TmpPanel.PanelKey == null) return;

                Byte[] ArayButtonLedBuf = new Byte[TmpPanel.PanelKey.Count];
                Byte[] ArayButtonDimAndSaveDim = new Byte[TmpPanel.PanelKey.Count];
                Byte[] ArayButtonMutux = new Byte[TmpPanel.PanelKey.Count];
                Byte[] ArayButtonLink = new Byte[TmpPanel.PanelKey.Count]; 
                if (clLed.Visible == true)
                {
                    ArayButtonLedBuf = TmpPanel.ReadButtonLEDFrmDeviceToBuf(SubNetID, DeviceID);
                }

                if (clMutux.Visible == true)
                {
                    ArayButtonMutux = TmpPanel.ReadButtonMutuxFrmDeviceToBuf(SubNetID, DeviceID);
                }

                if (clLink.Visible == true)
                {
                    ArayButtonLink = TmpPanel.ReadButtonLinkableFrmDeviceToBuf(SubNetID, DeviceID);
                }

                if (ClDim.Visible == true)
                {
                    ArayButtonDimAndSaveDim = TmpPanel.ReadButtonDimFlagFrmDeviceToBuf(SubNetID, DeviceID);
                    for (Byte bytI = 0; bytI < TmpPanel.PanelKey.Count; bytI++)
                    {
                        TmpPanel.PanelKey[bytI].IsDimmer = (Byte)(ArayButtonDimAndSaveDim[bytI] >> 4);
                        TmpPanel.PanelKey[bytI].SaveDimmer = (Byte)(ArayButtonDimAndSaveDim[bytI] & 0x01);
                        TmpPanel.PanelKey[bytI].IsLEDON = ArayButtonLedBuf[bytI];
                        TmpPanel.PanelKey[bytI].bytMutex = ArayButtonMutux[bytI];
                        TmpPanel.PanelKey[bytI].byteLink = ArayButtonLink[bytI];
                    }
                }

                DisplayButtonParametersToGridTable();
                #endregion
            }
            else if (MyActiveObject is MS04)
            {
                #region
                MS04 TmpPanel = (MS04)MyActiveObject;
                if (TmpPanel == null) return;
                if (TmpPanel.MSKeys == null) return;

                Byte[] ArayButtonDimAndSaveDim = new Byte[TmpPanel.MSKeys.Count];
                Byte[] ArayButtonDimDirection = new Byte[TmpPanel.MSKeys.Count];

                if (DryDim.Visible == true)
                {
                    ArayButtonDimDirection = TmpPanel.ReadButtonDimDirectionFrmDeviceToBuf(SubNetID, DeviceID);
                }

                if (ClDimValue.Visible == true)
                {
                    ArayButtonDimAndSaveDim = TmpPanel.ReadButtonDimFlagFrmDeviceToBuf(SubNetID, DeviceID);

                    for (Byte bytI = 0; bytI < TmpPanel.MSKeys.Count; bytI++)
                    {
                        TmpPanel.MSKeys[bytI].IsDimmer = ArayButtonDimDirection[bytI];
                        TmpPanel.MSKeys[bytI].SaveDimmer = ArayButtonDimAndSaveDim[bytI];
                    }
                }

                DisplayButtonParametersToGridTable();
                #endregion
            }
            else if (MyActiveObject is MHRCU)
            {
                #region
                MHRCU TmpPanel = (MHRCU)MyActiveObject;
                if (TmpPanel == null) return;
                if (TmpPanel.MSKeys == null) return;

                Byte[] ArayButtonDimAndSaveDim = new Byte[TmpPanel.MSKeys.Count];
                Byte[] ArayButtonDimDirection = new Byte[TmpPanel.MSKeys.Count];

                if (DryDim.Visible == true)
                {
                    ArayButtonDimDirection = TmpPanel.ReadButtonDimDirectionFrmDeviceToBuf(SubNetID, DeviceID);
                }

                if (ClDimValue.Visible == true)
                {
                    ArayButtonDimAndSaveDim = TmpPanel.ReadButtonDimFlagFrmDeviceToBuf(SubNetID, DeviceID);

                    for (Byte bytI = 0; bytI < TmpPanel.MSKeys.Count; bytI++)
                    {
                        TmpPanel.MSKeys[bytI].IsDimmer = ArayButtonDimDirection[bytI];
                        TmpPanel.MSKeys[bytI].SaveDimmer = ArayButtonDimAndSaveDim[bytI];
                    }
                }

                DisplayButtonParametersToGridTable();
                #endregion
            }
        }

        /// <summary>
        /// 更新到结构体 需要则上传到设备
        /// </summary>
        void SaveButtonParametersToBuf()
        {
            if (MyActiveObject is IPModule)
            {
                #region
                IPModule TmpIpModule = (IPModule)MyActiveObject;
                if (TmpIpModule == null) return;
                if (TmpIpModule.MyRemoteControllers == null) return;
                if (cboController.SelectedIndex == -1) return;
                Byte RemoteControllerID = (Byte)cboController.SelectedIndex;
                Byte PageID = (Byte)cboPages.SelectedIndex;

                Byte[] DimEnable = GetDataFromDataGridToPublicBuf(4);
                Byte[] SaveDimEnable = GetDataFromDataGridToPublicBuf(5);

                for (Byte bytI = 0; bytI < 8; bytI++)
                {
                    TmpIpModule.MyRemoteControllers[RemoteControllerID * IPmoduleDeviceTypeList.HowManyButtonsEachPage + bytI + PageID * 8].IsDimmer = DimEnable[bytI];
                    TmpIpModule.MyRemoteControllers[RemoteControllerID * IPmoduleDeviceTypeList.HowManyButtonsEachPage + bytI + PageID * 8].SaveDimmer = SaveDimEnable[bytI];
                }

                if (CsConst.MyEditMode == 1)
                    TmpIpModule.SaveButtonDimFlagToDeviceFrmBuf(RemoteControllerID, SubNetID, DeviceID, mywdDevicerType);
                #endregion
            }
            else if (MyActiveObject is Panel)
            {
                #region
                Panel TmpPanel = (Panel)MyActiveObject;
                if (TmpPanel == null) return;
                if (TmpPanel.PanelKey == null) return;

                Byte PageID = 0;
                if (cboPages.Visible) PageID = Convert.ToByte(cboPages.SelectedIndex);
                int wdMaxValue = int.Parse(CsConst.mstrINIDefault.IniReadValue("DeviceType" + mywdDevicerType.ToString(), "MaxValue", "0"));

                Byte[] ArayButtonLedBuf = new Byte[TmpPanel.PanelKey.Count];
                Byte[] ArayButtonDim = new Byte[TmpPanel.PanelKey.Count];
                Byte[] ArayButtonSaveDim = new Byte[TmpPanel.PanelKey.Count];
                Byte[] ArayButtonMutux = new Byte[TmpPanel.PanelKey.Count];
                Byte[] ArayButtonLink = new Byte[TmpPanel.PanelKey.Count];

                if (clLed.Visible == true)
                {
                     ArayButtonLedBuf = GetDataFromDataGridToPublicBuf(3);
                     for (int i = 0; i < ArayButtonLedBuf.Length; i++)
                     {
                         if (ArayButtonLedBuf[i] == 0) ArayButtonLedBuf[i] = 1;
                         else ArayButtonLedBuf[i] = 0;
                     }
                }

                if (ClDim.Visible == true)
                {
                    ArayButtonDim = GetDataFromDataGridToPublicBuf(4);
                }

                if (ClDimValue.Visible ==true)
                {
                    ArayButtonSaveDim = GetDataFromDataGridToPublicBuf(5);
                }

                if (clMutux.Visible == true)
                {
                    ArayButtonMutux = GetDataFromDataGridToPublicBuf(6);
                }

                if (clLink.Visible == true)
                {
                    ArayButtonLink = GetDataFromDataGridToPublicBuf(7);
                }

                for (Byte bytI = 0; bytI < dgvListA.RowCount; bytI++)
                {
                    TmpPanel.PanelKey[bytI + wdMaxValue * PageID].IsDimmer = ArayButtonDim[bytI];
                    TmpPanel.PanelKey[bytI + wdMaxValue * PageID].SaveDimmer = ArayButtonSaveDim[bytI];
                    TmpPanel.PanelKey[bytI + wdMaxValue * PageID].IsLEDON = ArayButtonLedBuf[bytI];
                    TmpPanel.PanelKey[bytI + wdMaxValue * PageID].bytMutex = ArayButtonMutux[bytI];
                    TmpPanel.PanelKey[bytI + wdMaxValue * PageID].byteLink = ArayButtonLink[bytI];
                }

                if (CsConst.MyEditMode == 1)
                {
                    if (clLed.Visible == true) TmpPanel.SaveButtonLEDEnableToDeviceFrmBuf(SubNetID,DeviceID,mywdDevicerType);
                    if (ClDim.Visible  == true) TmpPanel.SaveButtonDimFlagToDeviceFrmBuf(SubNetID, DeviceID, mywdDevicerType);
                    if (clMutux.Visible == true) TmpPanel.SaveButtonMutuxToDeviceFrmBuf(SubNetID, DeviceID, mywdDevicerType);
                    if (clLink.Visible == true) TmpPanel.SaveButtonLinkageToDeviceFrmBuf(SubNetID, DeviceID, mywdDevicerType);
                }
                #endregion
            }
            else if (MyActiveObject is MS04)
            {
                #region
                MS04 TmpPanel = (MS04)MyActiveObject;
                if (TmpPanel == null) return;
                if (TmpPanel.MSKeys == null) return;

                Byte[] ArayButtonDim = new Byte[TmpPanel.MSKeys.Count];
                Byte[] ArayButtonSaveDim = new Byte[TmpPanel.MSKeys.Count];

                if (DryDim.Visible == true)
                {
                    ArayButtonDim = GetDataFromDataGridToPublicBuf(8);
                }

                if (ClDimValue.Visible == true)
                {
                    ArayButtonSaveDim = GetDataFromDataGridToPublicBuf(5);
                }

                for (Byte bytI = 0; bytI < TmpPanel.MSKeys.Count; bytI++)
                {
                    TmpPanel.MSKeys[bytI].IsDimmer = ArayButtonDim[bytI];
                    TmpPanel.MSKeys[bytI].SaveDimmer = ArayButtonSaveDim[bytI];
                }

                if (CsConst.MyEditMode == 1)
                {
                    if (ClDimValue.Visible == true) TmpPanel.SaveButtonDimFlagToDeviceFrmBuf(SubNetID, DeviceID, mywdDevicerType);
                    if (DryDim.Visible == true) TmpPanel.SaveButtonDimDirectionsToDeviceFrmBuf(SubNetID, DeviceID, mywdDevicerType);

                }
                #endregion
            }
            else if (MyActiveObject is MHRCU)
            {
                #region
                MHRCU TmpPanel = (MHRCU)MyActiveObject;
                if (TmpPanel == null) return;
                if (TmpPanel.MSKeys == null) return;

                Byte[] ArayButtonDim = new Byte[TmpPanel.MSKeys.Count];
                Byte[] ArayButtonSaveDim = new Byte[TmpPanel.MSKeys.Count];

                if (DryDim.Visible == true)
                {
                    ArayButtonDim = GetDataFromDataGridToPublicBuf(8);
                }

                if (ClDimValue.Visible == true)
                {
                    ArayButtonSaveDim = GetDataFromDataGridToPublicBuf(5);
                }

                for (Byte bytI = 0; bytI < TmpPanel.MSKeys.Count; bytI++)
                {
                    TmpPanel.MSKeys[bytI].IsDimmer = ArayButtonDim[bytI];
                    TmpPanel.MSKeys[bytI].SaveDimmer = ArayButtonSaveDim[bytI];
                }

                if (CsConst.MyEditMode == 1)
                {
                    if (ClDimValue.Visible == true) TmpPanel.SaveButtonDimFlagToDeviceFrmBuf(SubNetID, DeviceID, mywdDevicerType);
                    if (DryDim.Visible == true) TmpPanel.SaveButtonDimDirectionsToDeviceFrmBuf(SubNetID, DeviceID, mywdDevicerType);

                }
                #endregion
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Byte[] GetDataFromDataGridToPublicBuf(int ColumeID)
        { 
            if (cboDevA.SelectedIndex ==-1) return null;
            if (dgvListA.Rows.Count == 0) return null;

            Byte[] TmpBuf = new Byte[dgvListA.Rows.Count];

            if (ColumeID >= 3 && ColumeID <= 5)
            {
                for (int i = 0; i < dgvListA.Rows.Count; i++)
                {
                    if (dgvListA[ColumeID, i].Value.ToString().ToLower() == "true")
                    {
                        TmpBuf[i] = 1;
                    }
                }
            }
            else if (ColumeID == 6)// && ColumeID <= 6)
            {
                for (int i = 0; i < dgvListA.Rows.Count; i++)
                {
                    TmpBuf[i] = (Byte)clMutux.Items.IndexOf(dgvListA[ColumeID, i].Value.ToString());
                }
            }
            else if (ColumeID == 7)
            {
                for (int i = 0; i < dgvListA.Rows.Count; i++)
                {
                    TmpBuf[i] = (Byte)clLink.Items.IndexOf(dgvListA[ColumeID, i].Value.ToString());
                }
            }
            else if (ColumeID == 8)
            {
                for (int i = 0; i < dgvListA.Rows.Count; i++)
                {
                    TmpBuf[i] = (Byte)DryDim.Items.IndexOf(dgvListA[8,i].Value.ToString());
                }
            }
            return TmpBuf;
        }

        private void frmUpgrade_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                this.Dispose();
            }
            catch
            {
            }
        }

        private void cboController_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboController.SelectedIndex == -1) return;
            if (cboPages.SelectedIndex == -1) cboPages.SelectedIndex = PageAndButtonID[0];
        }

        private void cboPages_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboPages.SelectedIndex == -1) return;
            DisplayButtonParametersToGridTable();
        }


        private void RefreshDisplay_Click(object sender, EventArgs e)
        {
             RefreshButtonParametersToDataGrid();
        }

        private void btnUpdateA_Click(object sender, EventArgs e)
        {
            SaveButtonParametersToBuf();
        }

        private void Next_Click(object sender, EventArgs e)
        {
            if (cboPages.SelectedIndex < cboPages.Items.Count - 1) cboPages.SelectedIndex++;
        }

        private void Pre_Click(object sender, EventArgs e)
        {
            if (cboPages.SelectedIndex !=0) cboPages.SelectedIndex--;
        }

        private void frmButtonSetup_Shown(object sender, EventArgs e)
        {
        }

        private void dgvListA_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (dgvListA.SelectedRows == null) return;
            if (dgvListA.Rows == null) return;
            try
            {
                if (e.ColumnIndex >=1)
                {
                    Byte Mode = ButtonMode.ConvertorKeyModesToPublicModeGroup(dgvListA[2, e.RowIndex].Value.ToString());
                    Byte[] ModeThatCouldMutexGroup = new Byte[]{4,5,7}; //互斥的按键模式
                    Byte[] ModeThatCouldLinkageGroup = new Byte[]{1,2,3,4,5,6}; // 可以关联的按键

                    dgvListA[6, e.RowIndex].ReadOnly = !ModeThatCouldMutexGroup.Contains(Mode);
                    dgvListA[7, e.RowIndex].ReadOnly = !ModeThatCouldLinkageGroup.Contains(Mode);

                    if (dgvListA[6, e.RowIndex].ReadOnly == true)
                    {
                        dgvListA[6, e.RowIndex].Value = clMutux.Items[0].ToString();
                    }

                    if (dgvListA[7, e.RowIndex].ReadOnly == true)
                    {
                        dgvListA[7, e.RowIndex].Value = CsConst.mstrInvalid;
                    }
                }
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        private void dgvListA_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (dgvListA == null) return;
                if (dgvListA.Rows.Count == 0) return;
                if ((e.RowIndex == -1) || (e.ColumnIndex == -1)) return;
                if (dgvListA[e.ColumnIndex, e.RowIndex].Value == null) return;
                if (dgvListA.SelectedRows.Count == 0) return;

                String tempText = dgvListA[e.ColumnIndex, e.RowIndex].Value.ToString();

                for (int i = 0; i < dgvListA.SelectedRows.Count; i++)
                {
                    dgvListA.SelectedRows[i].Cells[e.ColumnIndex].Value = tempText;
                }
            }
            catch
            {
            }
        }

        private void dgvListA_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            dgvListA.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

    }
}
