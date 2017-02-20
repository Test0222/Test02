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
    public partial class frmCmdSetup : Form
    {
        private Object MyActiveObject = null;
        private String DeviceName = "";
        private Byte SubNetID;
        private Byte DeviceID;
        private int mywdDevicerType;

        private Boolean blnIsLoading;

        private Int32 remoteId;
        private Int32 pageId;
        private Int32 buttonId;
        private Int32 remoteSum;

        private IPModule TmpIpModule = null;
        private Panel TmpPanel = null;
        private ColorDLP TmpEnviroOld = null;
        private EnviroPanel TmpEnviro = null;
        private MS04 TmpMS04 = null;
        private MHRCU TmpMHRcu = null;
        private MiniSensor TmpMiniSensor = null;
        private MultiSensor Tmp8in1 = null;
        private Sensor_12in1 Tmp12in1 = null;
        private MSPU TmpWUSensor = null;

        private HDLButton TmpButton = null;
        private SensorLogic TmpSensorBlock = null; // 传感器逻辑功能
        

        private Logic TmpLogic = null;

        private TextBox txtSub = new TextBox();
        private TextBox txtDev = new TextBox();
        private ComboBox cbControlType = new ComboBox();
        private ComboBox cbbox1 = new ComboBox();
        private ComboBox cbbox2 = new ComboBox();
        private ComboBox cbbox3 = new ComboBox();
        private TextBox txtbox1 = new TextBox();
        private TextBox txtbox2 = new TextBox();
        private TextBox txtbox3 = new TextBox();
        private TextBox txtbox4 = new TextBox();
        private TimeText txtSeries = new TimeText(":");
        private ComboBox cbPanleControl = new ComboBox();
        private ComboBox cbAudioControl = new ComboBox();
        private UnivasalControl uc1 = new UnivasalControl("0/0");
        private UnivasalControl uc2 = new UnivasalControl("0/0");
        private NewIRKeyID IrCtrl = new NewIRKeyID("1");


        private UVCMD.ControlTargets TempCMD = null; //保存时更新到buffer
        public List<UVCMD.ControlTargets> TempCMDGroup = null;

        public frmCmdSetup()
        {
            InitializeComponent();
        }

        public frmCmdSetup(Object CurrentObj, String TmpDeviceName, int wdDeviceType,Byte[] FurtherParamters)
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
            try
            {
                MyActiveObject = CurrentObj;
                this.DeviceName = TmpDeviceName;
                if (FurtherParamters == null) FurtherParamters = new Byte[4];
                remoteId = FurtherParamters[2];   // 按键号 页号 遥控器号.遥控器总数
                pageId = FurtherParamters[1];
                buttonId = FurtherParamters[0];
                remoteSum = FurtherParamters[3];

                mywdDevicerType = wdDeviceType;

                string strDevName = DeviceName.Split('\\')[0].ToString();

                SubNetID = Convert.ToByte(strDevName.Split('-')[0]);
                DeviceID = Convert.ToByte(strDevName.Split('-')[1]);
                LoadControlsText.DisplayTextToFormWhenFirstShow(this);
                LoadButtonOrDryContactOrLogicTablesRemarkToCboKeyIfNeeds();
            }
            catch { }
        }

        protected override bool ProcessCmdKey(ref  System.Windows.Forms.Message msg, System.Windows.Forms.Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.C))
            {
                copy_Click(copy, null);
                return true;
            }
            else if (keyData == (Keys.Control | Keys.V))
            {
                paste_Click(paste, null);
                return true;
            }

            return base.ProcessCmdKey(ref   msg, keyData);
        }

        private void frmUpgrade_Load(object sender, EventArgs e)
        {
            blnIsLoading = true;
            InitialFormCtrlsTextOrItems();

            if (CsConst.myOnlines == null) return;
            HDLSysPF.LoadDeviceListsWithDifferentSensorType(cboDevA, 255);
            cboDevA.SelectedIndex = cboDevA.Items.IndexOf(DeviceName);
        }

        void InitialFormCtrlsTextOrItems()
        {
            cbAudioControl.Items.Clear();
            cbAudioControl.Items.AddRange(CsConst.MusicControl);

            #region
            cbControlType.Items.Clear();
            HDLSysPF.AddControlTypeToControl(cbControlType, mywdDevicerType);
            cbPanleControl.Items.Clear();
            HDLSysPF.getPanlControlType(cbPanleControl, mywdDevicerType);

            cbControlType.DropDownStyle = ComboBoxStyle.DropDownList;
            cbPanleControl.DropDownStyle = ComboBoxStyle.DropDownList;
            cbAudioControl.DropDownStyle = ComboBoxStyle.DropDownList;
            cbbox1.DropDownStyle = ComboBoxStyle.DropDownList;
            cbbox2.DropDownStyle = ComboBoxStyle.DropDownList;
            cbbox3.DropDownStyle = ComboBoxStyle.DropDownList;

            txtSub.KeyPress += txtFrm_KeyPress;
            txtDev.KeyPress += txtFrm_KeyPress;
            txtbox1.KeyPress += txtFrm_KeyPress;
            txtbox2.KeyPress += txtFrm_KeyPress;
            txtbox3.KeyPress += txtFrm_KeyPress;
            txtbox4.KeyPress += txtFrm_KeyPress;
            txtSub.TabIndex = 1;
            txtDev.TabIndex = 2;
            cbControlType.TabIndex = 3;
            txtbox1.TabIndex = 4;
            txtbox2.TabIndex = 5;
            txtbox3.TabIndex = 6;
            txtbox4.TabIndex = 7;
            cbbox1.TabIndex = 4; 
            cbbox2.TabIndex = 5;
            cbbox3.TabIndex = 6;
            txtSeries.TabIndex = 4;
            cbPanleControl.TabIndex = 5;
            cbAudioControl.TabIndex = 6;

            setAllControlVisible(false);
            dgvListA.Controls.Add(cbControlType);
            dgvListA.Controls.Add(cbPanleControl);
            dgvListA.Controls.Add(cbAudioControl);
            dgvListA.Controls.Add(txtSub);
            dgvListA.Controls.Add(txtDev);
            dgvListA.Controls.Add(cbbox1);
            dgvListA.Controls.Add(cbbox2);
            dgvListA.Controls.Add(cbbox3);
            dgvListA.Controls.Add(txtSeries);
            dgvListA.Controls.Add(txtbox1);
            dgvListA.Controls.Add(txtbox2);
            dgvListA.Controls.Add(txtbox3);
            dgvListA.Controls.Add(txtbox4);
            dgvListA.Controls.Add(IrCtrl);
            dgvListA.Controls.Add(uc1);
            dgvListA.Controls.Add(uc2);

            IrCtrl.UserControlValueChanged += ir_UserControlValueChanged;

            cbControlType.SelectedIndexChanged += cbControlType_SelectedIndexChanged;
            uc1.UserControlValueChanged += uc1_UserControlValueChanged;
            uc2.UserControlValueChanged += uc2_UserControlValueChanged;
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
            txtbox4.TextChanged += txtbox4_TextChanged;
            txtSeries.TextChanged += txtSeries_TextChanged;
            #endregion      
        }

        void SetCtrlsVisbleWithDifferentDeviceType()
        {
            if (MyActiveObject == null) return;
            Boolean IsHasSecondLevelLikeRemoteIDLogicBlocks = IPmoduleDeviceTypeList.RFIpModuleV1.Contains(mywdDevicerType) ||
                                                              IPmoduleDeviceTypeList.RFIpModuleV2.Contains(mywdDevicerType) ||
                                                              EnviroNewDeviceTypeList.EnviroNewPanelDeviceTypeList.Contains(mywdDevicerType) ||
                                                              LogicDeviceTypeList.LogicDeviceType.Contains(mywdDevicerType);

            Boolean IsHasMorePagesInformation = IPmoduleDeviceTypeList.RFIpModuleV1.Contains(mywdDevicerType)
                                             || IPmoduleDeviceTypeList.RFIpModuleV2.Contains(mywdDevicerType)
                                             || EnviroNewDeviceTypeList.EnviroNewPanelDeviceTypeList.Contains(mywdDevicerType)
                                             || DLPPanelDeviceTypeList.HDLDLPPanelDeviceTypeList.Contains(mywdDevicerType)
                                             || EnviroDLPPanelDeviceTypeList.HDLEnviroDLPPanelDeviceTypeList.Contains(mywdDevicerType)
                                             ||(Twelvein1DeviceTypeList.HDL12in1DeviceType.Contains(mywdDevicerType) && remoteId ==1);

            Boolean IsHasLogicTrueAndFalseSetup = MSPUSenserDeviceTypeList.SensorsDeviceTypeList.Contains(mywdDevicerType)
                                               || Eightin1DeviceTypeList.HDL8in1DeviceType.Contains(mywdDevicerType)
                                               || (HotelMixModuleDeviceType.HDLRCUDeviceTypeLists.Contains(mywdDevicerType) && pageId == 3)
                                               || MiniSenserDeviceTypeList.SensorsDeviceTypeList.Contains(mywdDevicerType)
                                               || (Twelvein1DeviceTypeList.HDL12in1DeviceType.Contains(mywdDevicerType) && remoteId ==0);

            rbShortOff.Visible = IsHasLogicTrueAndFalseSetup;
            rbLongPressON.Visible = IsHasLogicTrueAndFalseSetup;
            lbRemote.Visible = IsHasSecondLevelLikeRemoteIDLogicBlocks;
            cboRemote.Visible = IsHasSecondLevelLikeRemoteIDLogicBlocks;
            cboPages.Visible = IsHasMorePagesInformation;
            lbPage.Visible = cboPages.Visible;

            if (EnviroNewDeviceTypeList.EnviroNewPanelDeviceTypeList.Contains(mywdDevicerType)) // 彩屏面板
            {
                #region
                if (buttonId > 100)
                {
                    cboPages.Visible = false;
                    lbKey.Visible = false;
                    cbKey.Visible = false;
                    cboRemote.Items.Clear();
                    for (int i = 0; i < 8; i++) cboRemote.Items.Add((i + 1).ToString());
                }
                #endregion
            }
            else if (DLPPanelDeviceTypeList.HDLDLPPanelDeviceTypeList.Contains(mywdDevicerType) || 
                     NormalPanelDeviceTypeList.WirelessSimpleFloorHeatingDeviceTypeList.Contains(mywdDevicerType))     //DLP
            {
                #region
                if (buttonId > 100)
                {
                    cboPages.Visible = false;
                    lbKey.Visible = false;
                    cbKey.Visible = false;
                    lbPage.Visible = false;
                }
                #endregion
            }
            else if (MSPUSenserDeviceTypeList.SensorsDeviceTypeList.Contains(mywdDevicerType)   //超声波
                  || Eightin1DeviceTypeList.HDL8in1DeviceType.Contains(mywdDevicerType)
                  || HotelMixModuleDeviceType.HDLRCUDeviceTypeLists.Contains(mywdDevicerType)
                  || MiniSenserDeviceTypeList.SensorsDeviceTypeList.Contains(mywdDevicerType)
                  || (Twelvein1DeviceTypeList.HDL12in1DeviceType.Contains(mywdDevicerType) && remoteId !=1))
            {
                rbShortOff.Text = "True";
                rbLongPressON.Text = "False";
            }
            else if (MS04DeviceTypeList.HDLMS04DeviceTypeList.Contains(mywdDevicerType)) // 干节点
            {
                rbShortOff.Text = "OFF";
                rbLongPressON.Text = "ON";
            }
        }

        void uc2_UserControlValueChanged(object sender, UnivasalControl.TextChangeEventArgs e)
        {
            if (dgvListA.CurrentRow.Index < 0) return;
            if (dgvListA.RowCount <= 0) return;
            int index = dgvListA.CurrentRow.Index;
            string str = uc2.Text;
            if (uc2.Visible)
            {
                dgvListA[5, index].Value = str;
                if (dgvListA.SelectedRows == null || dgvListA.SelectedRows.Count == 0) return;
                string strTmp = dgvListA[5, index].Value.ToString() + "(" + CsConst.WholeTextsList[2496].sDisplayName + ")";
                if (strTmp == null) strTmp = "N/A";
                if (uc2.Focused)
                    ModifyMultilinesIfNeeds(strTmp, 5);
            }
        }

        void uc1_UserControlValueChanged(object sender, UnivasalControl.TextChangeEventArgs e)
        {
            if (dgvListA.CurrentRow.Index < 0) return;
            if (dgvListA.RowCount <= 0) return;
            int index = dgvListA.CurrentRow.Index;
            string str = uc1.Text;
            if (uc1.Visible)
            {
                dgvListA[4, index].Value = str;
                if (dgvListA.SelectedRows == null || dgvListA.SelectedRows.Count == 0) return;
                string strTmp = dgvListA[4, index].Value.ToString();
                if (strTmp == null) strTmp = "N/A";
                if (uc1.Focused)
                    ModifyMultilinesIfNeeds(strTmp, 4);
            }
        }

        void ir_UserControlValueChanged(object sender, NewIRKeyID.TextChangeEventArgs e)
        {
            if (dgvListA.CurrentRow.Index < 0) return;
            if (dgvListA.RowCount <= 0) return;
            int index = dgvListA.CurrentRow.Index;
            string str = IrCtrl.Text;
            if (IrCtrl.Visible)
            {
                dgvListA[6, index].Value = str + "(" + CsConst.WholeTextsList[2505].sDisplayName + ")";
                if (dgvListA.SelectedRows == null || dgvListA.SelectedRows.Count == 0) return;
                string strTmp = dgvListA[6, index].Value.ToString();
                if (strTmp == null) strTmp = "N/A";
                if (IrCtrl.Focused)
                    ModifyMultilinesIfNeeds(strTmp, 6);
            }
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

        private void frmCmdSetup_Shown(object sender, EventArgs e)
        {
            SetCtrlsVisbleWithDifferentDeviceType();
            //直接复制窗体目标到此编辑
            
            blnIsLoading = false;
            DisplayWithVisibleControlsIfHasRemoteOrLogicBlock();
            blnIsLoading = false;
        }

        /// <summary>
        /// 进一步界面处理
        /// </summary>
        void DisplayWithVisibleControlsIfHasRemoteOrLogicBlock()
        {
            try
            {
                if (blnIsLoading == true) return;

                if (CsConst.MyCopyDataGridView != null) //直接复制
                {
                    HDLSysPF.PasteCMDToPublicBufferWaitPasteOrCopyToGrid(dgvListA);
                }
                else
                {
                    if (cboRemote.Visible == true) //第二个逻辑
                    {
                        if (cboRemote.SelectedIndex == -1) cboRemote.SelectedIndex = remoteId;
                    }
                    else
                    {
                        DisplayCommandGroupIfHasPageOrButton();
                    }
                }
            }
            catch
            { }
            //Ruby
        }

        void DisplayCommandGroupIfHasPageOrButton()
        {
            try
            {
                if (cboPages.Visible == true)  // 是否还有页信息
                {
                    if (cboPages.SelectedIndex == -1) cboPages.SelectedIndex = pageId;
                }
                else if (cbKey.Visible == true) //直接显示按键信息
                {
                    if (cbKey.SelectedIndex == -1) cbKey.SelectedIndex = buttonId;
                }
                else // 直接复制表格并等待更新
                {
                    //直接显示目标在此，同时记得更新
                    #region
                    DisplayCmdDirectlyWithDifferentDeviceType();
                    #endregion
                }
            }
            catch
            { }
        }

        void DisplayCmdDirectlyWithDifferentDeviceType()
        {
            dgvListA.Rows.Clear();
            // 是不是标准按键或者干接点
            //显示基本信息
            if (TmpButton != null)
            {
                DisplayButtonCommandsGroup();
            }
            else
            {
                if (DLPPanelDeviceTypeList.HDLDLPPanelDeviceTypeList.Contains(mywdDevicerType)) // DLP地热页面
                {
                    #region
                    DLP TmpDLp = (DLP)MyActiveObject;
                    if (TmpDLp.DLPFH.AdvancedCommands != null && TmpDLp.DLPFH.AdvancedCommands.Length != 0)
                    {
                        txtFrm.Text = (buttonId - 1).ToString();
                        txtTo.Text = (buttonId + TmpDLp.DLPFH.AdvancedCommands.Length -1).ToString();
                        foreach (UVCMD.ControlTargets oTmp in TmpDLp.DLPFH.AdvancedCommands)
                        {
                            DisplayCMDToDataGrid(oTmp);
                        }
                    }
                    #endregion
                }
                else if (NormalPanelDeviceTypeList.WirelessSimpleFloorHeatingDeviceTypeList.Contains(mywdDevicerType)) // ismart地热页面
                {
                    #region
                    Panel TmpDLp = (Panel)MyActiveObject;
                    if (TmpDLp.DLPFH.AdvancedCommands != null && TmpDLp.DLPFH.AdvancedCommands.Length != 0)
                    {
                        txtFrm.Text = (buttonId - 1).ToString();
                        txtTo.Text = (buttonId + TmpDLp.DLPFH.AdvancedCommands.Length - 1).ToString();
                        foreach (UVCMD.ControlTargets oTmp in TmpDLp.DLPFH.AdvancedCommands)
                        {
                            DisplayCMDToDataGrid(oTmp);
                        }
                    }
                    #endregion
                }
                else if (EnviroNewDeviceTypeList.EnviroNewPanelDeviceTypeList.Contains(mywdDevicerType)) // 彩屏 音乐或者地热页面
                {
                    #region
                    EnviroPanel TmpDLp = (EnviroPanel)MyActiveObject;
                    if (TmpDLp.MyHeat[remoteId].AdvancedCommands != null && TmpDLp.MyHeat[remoteId].AdvancedCommands.Length != 0)
                    {
                        txtTo.Text = TmpDLp.MyHeat[remoteId].AdvancedCommands.Length.ToString();
                        foreach (UVCMD.ControlTargets oTmp in TmpDLp.MyHeat[remoteId].AdvancedCommands)
                        {
                            if (oTmp != null) DisplayCMDToDataGrid(oTmp);
                        }
                    }
                    #endregion
                }
                else if (MSPUSenserDeviceTypeList.SensorsDeviceTypeList.Contains(mywdDevicerType) // 传感器
                      || Eightin1DeviceTypeList.HDL8in1DeviceType.Contains(mywdDevicerType) // 8in1
                      || (HotelMixModuleDeviceType.HDLRCUDeviceTypeLists.Contains(mywdDevicerType) && pageId ==3)  // 酒店混合模块 + 逻辑功能
                      || MiniSenserDeviceTypeList.SensorsDeviceTypeList.Contains(mywdDevicerType) // 迷你传感器
                      || MSPUSenserDeviceTypeList.SensorsDeviceTypeList.Contains(mywdDevicerType))  //无线红外超声波
                {
                    #region
                    if (rbShortOff.Checked == false && rbLongPressON.Checked == false)
                    {
                        rbLongPressON.Checked = true;
                        rbShort_CheckedChanged(rbLongPressON, null);
                    }
                    else if (rbLongPressON.Checked)
                    {
                        rbShort_CheckedChanged(rbLongPressON, null);
                    }
                    else rbShort_CheckedChanged(rbShortOff, null);
                    #endregion
                }
                else if (LogicDeviceTypeList.LogicDeviceType.Contains(mywdDevicerType)) // 逻辑模块
                {
                    #region
                    if (buttonId == -1) return;
                    Byte ControllerID = Convert.ToByte(remoteId);
                    Byte ButtonID = Convert.ToByte(buttonId);

                    TmpLogic = (Logic)MyActiveObject;
                    foreach (UVCMD.ControlTargets Tmp in TmpLogic.MyDesign[ControllerID].ArmCmds[ButtonID])
                    {
                        DisplayCMDToDataGrid(Tmp);
                    }
                    #endregion
                }
                else if (TmpMHRcu != null) // 混合酒店主机
                {
                    #region
                    if (buttonId == -1) return;

                    if (pageId == 1) // 232 to bus
                    {
                        foreach (UVCMD.ControlTargets Tmp in TmpMHRcu.myRS2BUS[buttonId].busTargets)
                        {
                            DisplayCMDToDataGrid(Tmp);
                        }
                    }
                    else if (pageId ==2) // 485 to bus
                    {
                        foreach (UVCMD.ControlTargets Tmp in TmpMHRcu.my4852BUS[buttonId].busTargets)
                        {
                            DisplayCMDToDataGrid(Tmp);
                        }
                    }
                    #endregion
                }
                else if (Tmp12in1 != null)
                {
                    #region
                    if (buttonId == -1) return;

                    if (remoteId == 1) // 红外接收
                    {
                        foreach (UVCMD.ControlTargets Tmp in Tmp12in1.IrReceiver[buttonId + pageId * 8].TargetInfo)
                        {
                            DisplayCMDToDataGrid(Tmp);
                        }
                    }
                    else
                    {
                        if (rbShortOff.Checked == false && rbLongPressON.Checked == false)  rbShortOff.Checked = true;
                        if (rbShortOff.Checked == true) rbShort_CheckedChanged(rbShortOff, null);
                        else rbShort_CheckedChanged(rbLongPressON, null);
                    }
                    #endregion
                }
            }
        }

        /// <summary>
        /// 不同类型显示到界面
        /// </summary>
        void DisplayButtonParametersToGridTable()
        {
            if (TmpIpModule != null)
            {
                #region
                if (remoteId == -1) return;
                if (pageId == -1) return;
                dgvListA.Rows.Clear();
                Byte ControllerID = Convert.ToByte(remoteId);
                Byte PageID = Convert.ToByte(pageId);
                Byte ButtonID = Convert.ToByte(buttonId);

                foreach (UVCMD.ControlTargets Tmp in TmpIpModule.MyRemoteControllers[ControllerID * 48 + PageID * 8 + ButtonID].KeyTargets)
                {
                    DisplayCMDToDataGrid(Tmp);
                }
                #endregion
            }
            else if (TmpPanel != null)
            {
                #region
                dgvListA.Rows.Clear();
                Byte ButtonID = Convert.ToByte(buttonId);
                foreach (UVCMD.ControlTargets Tmp in TmpPanel.PanelKey[ButtonID].KeyTargets)
                {
                    DisplayCMDToDataGrid(Tmp);
                }
                #endregion
            }
        }

        Boolean DisplayCMDToDataGrid(UVCMD.ControlTargets oCmd) // 0 面板 1 干结点
        {
            Byte bStartCmd = Convert.ToByte(txtFrm.Text.ToString());
            Byte bToCmd = Convert.ToByte(txtTo.Text.ToString());
            if (oCmd.ID < bStartCmd || oCmd.ID > bToCmd) return false;
            Boolean bIsAddToGridTable = true;
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                string strType = "";
                strType = ButtonControlType.ConvertorKeyModeToPublicModeGroup(oCmd.Type);

                if (MSPUSenserDeviceTypeList.SensorsDeviceTypeList.Contains(mywdDevicerType) || // 超声波
                    LogicDeviceTypeList.LogicDeviceType.Contains(mywdDevicerType) ||             // 逻辑模块
                    Eightin1DeviceTypeList.HDL8in1DeviceType.Contains(mywdDevicerType) ||   // 8in1传感器
                   (HotelMixModuleDeviceType.HDLRCUDeviceTypeLists.Contains(mywdDevicerType) && (pageId == 0 || pageId == 3)) ||  //酒店混合模块干节点页面
                    (MS04DeviceTypeList.MS04NormalDeviceTypeList.Contains(mywdDevicerType)) || // 四路干节点
                    MiniSenserDeviceTypeList.SensorsDeviceTypeList.Contains(mywdDevicerType) || // 迷你传感器
                    Twelvein1DeviceTypeList.HDL12in1DeviceType.Contains(mywdDevicerType)) // 十二合一
                {
                    strType = DryControlType.ConvertorKeyModeToPublicModeGroup(oCmd.Type);
                }
                String sHead = oCmd.ID.ToString();
                if ((DLPPanelDeviceTypeList.HDLDLPPanelDeviceTypeList.Contains(mywdDevicerType) ||NormalPanelDeviceTypeList.WirelessSimpleFloorHeatingDeviceTypeList.Contains(mywdDevicerType))
                    && buttonId == 101)
                {
                    switch (oCmd.ID - 99)
                    {
                        #region
                        case 1:
                        case 2:
                        case 3:
                        case 4:
                            sHead = CsConst.mstrINIDefault.IniReadValue("Public", "00050", "") + " "
                                  + CsConst.Status[0] + " "
                                  + (oCmd.ID - 100 + 1).ToString(); break;

                        case 5:
                        case 6:
                        case 7:
                        case 8:
                            sHead = CsConst.mstrINIDefault.IniReadValue("Public", "00050", "") + " "
                                  + CsConst.Status[1] + " "
                                  + (oCmd.ID - 104 + 1).ToString(); break;

                        case 9:
                        case 10:
                        case 11:
                        case 12:
                            sHead = CsConst.mstrINIDefault.IniReadValue("Public", "00051", "") + " "
                                  + CsConst.Status[0] + " "
                                  + (oCmd.ID - 108+ 1).ToString(); break;
                        case 13:
                        case 14:
                        case 15:
                        case 16:
                            sHead = CsConst.mstrINIDefault.IniReadValue("Public", "00051", "") + " "
                                  + CsConst.Status[1] + " "
                                  + (oCmd.ID - 112+ 1).ToString(); break;
                        #endregion
                    }
                }
                else
                {
                    Byte CmdID = HDLSysPF.GetIndexFromBuffers(dgvListA, 0);
                    sHead = CmdID.ToString();
                }
                string strParam1 = "0", strParam2 = "0", strParam3 = "0", strParam4 = "0";
                strParam1 = oCmd.Param1.ToString();
                strParam2 = oCmd.Param2.ToString();
                strParam3 = oCmd.Param3.ToString();
                strParam4 = oCmd.Param4.ToString();
                SetParams(ref strType, ref strParam1, ref strParam2, ref strParam3, ref strParam4);

               
                object[] obj = new object[] {sHead,oCmd.SubnetID.ToString(),oCmd.DeviceID,strType
                                ,strParam1,strParam2,strParam3,strParam4};
                dgvListA.Rows.Add(obj);
            }
            catch 
            {
                return bIsAddToGridTable;
            }
            Cursor.Current = Cursors.Default;
            return bIsAddToGridTable;
        }

        private void SetParams(ref string strType, ref string str1, ref string str2, ref string str3, ref string str4)
        {
            if (strType == CsConst.myPublicControlType[0].ControlTypeName) //无效
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
                String sAcId = CsConst.mstrInvalid;
                if ((Convert.ToInt32(str1) >= 3 && Convert.ToInt32(str1) <= 10) ||
                    (Convert.ToInt32(str1) >= 20 && Convert.ToInt32(str1) <= 28)) sAcId = str3;
                #region
                str1 = HDLSysPF.InquirePanelControTypeStringFromDB(Convert.ToInt32(str1));
                if (str1 == CsConst.PanelControl[0])
                {
                    str2 = "N/A";
                    str3 = "N/A";
                }
                else if (str1 == CsConst.PanelControl[1] ||
                         str1 == CsConst.PanelControl[2] ||
                         str1 == CsConst.PanelControl[5] ||
                         str1 == CsConst.PanelControl[7] ||
                         str1 == CsConst.PanelControl[8] ||
                         str1 ==  CsConst.PanelControl[12] ||
                         str1 == CsConst.PanelControl[21])
                {
                    if (str2 == "0") str2 = CsConst.Status[0] + "(" + CsConst.WholeTextsList[2529].sDisplayName + ")";
                    else if (str2 == "1") str2 = CsConst.Status[1] + "(" + CsConst.WholeTextsList[2529].sDisplayName + ")";
                    str3 = "N/A";
                }
                else if (str1 == CsConst.PanelControl[3] ||
                         str1 == CsConst.PanelControl[4])
                {
                    str2 = str2 + "(" + CsConst.WholeTextsList[2524].sDisplayName + ")";
                    str3 = "N/A";
                }
                else if (str1 == CsConst.PanelControl[6] ||
                         str1 == CsConst.PanelControl[9] ||
                         str1 ==  CsConst.PanelControl[10] ||
                         str1 ==  CsConst.PanelControl[11])
                {
                    int intTmp = Convert.ToInt32(str2);
                    if (str1 == CsConst.PanelControl[6])
                    {
                        if (1 <= intTmp && intTmp <= 7) str2 = CsConst.mstrINIDefault.IniReadValue("public", "00048", "") + intTmp.ToString();
                        else if (intTmp == 255) str2 = CsConst.mstrINIDefault.IniReadValue("public", "00049", "");
                        else str2 = CsConst.WholeTextsList[1775].sDisplayName;
                    }
                    else if (str1 == CsConst.PanelControl[9])
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
                else if (str1 == CsConst.PanelControl[22])
                {
                    int intTmp = Convert.ToInt32(str2);
                    if (intTmp <= 5) str2 = CsConst.mstrINIDefault.IniReadValue("public", "0007" + (intTmp - 1).ToString(), "");
                    if (str3 == "255") str3 = CsConst.mstrINIDefault.IniReadValue("public", "00049", "");
                    else str3 = str3 + "(" + CsConst.WholeTextsList[934].sDisplayName + ")";
                }
                else if (str1 == CsConst.PanelControl[25] ||
                         str1 == CsConst.PanelControl[26] ||
                         str1 == CsConst.PanelControl[27] ||
                         str1 == CsConst.PanelControl[28])
                {
                    int intTmp = Convert.ToInt32(str2);
                    if (5 <= intTmp && intTmp <= 35) str2 = str2 + "C";
                    else if (41 <= intTmp && intTmp <= 95) str2 = str2 + "F";
                    intTmp = Convert.ToInt32(str3);
                    if (1 <= intTmp && intTmp <= 8) str2 = intTmp.ToString() + "(" + CsConst.WholeTextsList[934].sDisplayName + ")";
                    else if (intTmp == 255) str2 = CsConst.mstrINIDefault.IniReadValue("public", "00049", "");
                }
                else if (str1 == CsConst.PanelControl[29])
                {
                    int intTmp = Convert.ToInt32(str2);
                    if (1 <= intTmp && intTmp <= 7) str2 = CsConst.mstrINIDefault.IniReadValue("public", "00048", "") + str2;
                    str3 = "N/A";
                }
                else if (str1 == CsConst.PanelControl[30])
                {
                    int intTmp = Convert.ToInt32(str2);
                    if (1 <= intTmp && intTmp <= 32) str2 = str2 + "(" + CsConst.mstrINIDefault.IniReadValue("public", "99846", "") + ")";
                    str3 = "N/A";
                }

                str4 = "N/A";
                #endregion
                if (sAcId != CsConst.mstrInvalid) str3 = sAcId;
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
                if (1 <= intTmp && intTmp <= 8) str1 = CsConst.MusicControl[intTmp];
                else str1 = CsConst.MusicControl[0];
                if (str1 == cbAudioControl.Items[0].ToString())
                {
                    intTmp = Convert.ToInt32(str2);
                    if (1 <= intTmp && intTmp <= 4) str2 = CsConst.mstrINIDefault.IniReadValue("public", "0010" + intTmp.ToString(), "");
                    str3 = "N/A";
                }
                else if (str1 == cbAudioControl.Items[1].ToString())
                {
                    intTmp = Convert.ToInt32(str2);
                    if (1 <= intTmp && intTmp <= 4) str2 = CsConst.mstrINIDefault.IniReadValue("public", "0011" + intTmp.ToString(), "");
                    str3 = "N/A";
                }
                else if (str1 == cbAudioControl.Items[2].ToString())
                {
                    intTmp = Convert.ToInt32(str2);
                    if (1 <= intTmp && intTmp <= 6) str2 = CsConst.mstrINIDefault.IniReadValue("public", "0012" + intTmp.ToString(), "");
                    if (intTmp == 3 || intTmp == 6)
                        str3 = str3 + "(" + CsConst.mstrINIDefault.IniReadValue("public", "00043", "") + ")";
                    else
                        str3 = "N/A";
                }
                else if (str1 == cbAudioControl.Items[3].ToString())
                {
                    intTmp = Convert.ToInt32(str2);
                    if (1 <= intTmp && intTmp <= 4) str2 = CsConst.mstrINIDefault.IniReadValue("public", "0013" + intTmp.ToString(), "");
                    str3 = "N/A";
                }
                else if (str1 == cbAudioControl.Items[4].ToString())
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
                else if (str1 == cbAudioControl.Items[5].ToString())
                {
                    str2 = str2 + "(" + CsConst.mstrINIDefault.IniReadValue("public", "00046", "") + ")";
                    str3 = (Convert.ToInt32(str3) * 256 + Convert.ToInt32(str4)).ToString() + "(" + CsConst.mstrINIDefault.IniReadValue("public", "00043", "") + ")";
                }
                else if (str1 == cbAudioControl.Items[6].ToString() || str1 == cbAudioControl.Items[7].ToString())
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
            else if (strType == CsConst.myPublicControlType[3].ControlTypeName)//时间开关
            {
                #region
                str1 = str1 + "(" + CsConst.WholeTextsList[2513].sDisplayName + ")";
                if (str2 == "0") str2 = CsConst.Status[0] + "(" + CsConst.WholeTextsList[2529].sDisplayName + ")";
                else if (str2 == "255") str2 = CsConst.Status[1] + "(" + CsConst.WholeTextsList[2529].sDisplayName + ")";
                str3 = "N/A";
                str4 = "N/A";
                #endregion
            }
            else if (strType == CsConst.myPublicControlType[14].ControlTypeName)//通用控制
            {
                #region
                str1 = str1 + "/" + str2;
                str2 = str3 + "/" + str4;
                str3 = "N/A";
                str4 = "N/A";
                #endregion
            }
            else if (strType == CsConst.myPublicControlType[15].ControlTypeName)//连接页
            {
                #region
                str1 = str1 + "(" + CsConst.WholeTextsList[2499].sDisplayName + ")";
                if (str2 == "0")
                    str2 = CsConst.WholeTextsList[2497].sDisplayName;
                else if (str2 == "1")
                    str2 = CsConst.WholeTextsList[2498].sDisplayName;
                else
                    str2 = CsConst.WholeTextsList[1775].sDisplayName;
                str3 = "N/A";
                str4 = "N/A";
                #endregion
            }
            else if (strType == CsConst.myPublicControlType[18].ControlTypeName)//红外控制
            {
                #region
                str1 = str1 + "(" + CsConst.WholeTextsList[2506].sDisplayName + ")";
                str2 = str2 + "(" + CsConst.WholeTextsList[216].sDisplayName + ")";
                str3 = str3 + "(" + CsConst.WholeTextsList[2505].sDisplayName + ")";
                str4 = "N/A";
                #endregion
            }
            else if (strType == CsConst.myPublicControlType[19].ControlTypeName)//逻辑灯调节
            {
                #region
                string strTmp = str2;
                str1 = str1 + "(" + CsConst.WholeTextsList[2440].sDisplayName + ")";
                str2 = str3 + "(" + CsConst.WholeTextsList[2439].sDisplayName + ")";
                str3 = strTmp + "(" + CsConst.WholeTextsList[2524].sDisplayName + ")";
                str4 = str4 + "(" + CsConst.WholeTextsList[2438].sDisplayName + ")";
                #endregion
            }
            else if (strType == CsConst.myPublicControlType[20].ControlTypeName)//逻辑场景
            {
                #region
                str1 = str1 + "(" + CsConst.WholeTextsList[2510].sDisplayName + ")";
                str2 = str2 + "(" + CsConst.WholeTextsList[2511].sDisplayName + ")";
                str3 = "N/A";
                str4 = "N/A";
                #endregion
            }
            else if (strType == CsConst.myPublicControlType[21].ControlTypeName)//休眠模式
            {
                #region
                if (str1 == "1") str1 = CsConst.WholeTextsList[757].sDisplayName;
                else str1 = CsConst.WholeTextsList[1051].sDisplayName;
                if (str1 == CsConst.WholeTextsList[757].sDisplayName)
                {
                    if (str2 == "255")
                        str2 = CsConst.WholeTextsList[2434].sDisplayName;
                    else
                        str2 = str2 + "(" + CsConst.WholeTextsList[2437].sDisplayName + ")";
                    str3 = str3 + "(" + CsConst.WholeTextsList[2435].sDisplayName + ")";
                    str4 = str4 + "(" + CsConst.WholeTextsList[2436].sDisplayName + ")";
                }
                else
                {
                    str2 = "N/A";
                    str3 = "N/A";
                    str4 = "N/A";
                }
                #endregion
            }
        }

        private void cboDevA_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CsConst.MyCopyDataGridView != null)
            {
                HDLSysPF.PasteCMDToPublicBufferWaitPasteOrCopyToGrid(dgvListA);
            }
            else
            {
                DisplayWithVisibleControlsIfHasRemoteOrLogicBlock();
            }
        }

        private void cboRemote_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (blnIsLoading) return;
            try
            {
                remoteId = (Byte)(remoteId);

                if (cboPages.Visible == true)
                {
                    if (pageId == -1) pageId = 0;
                }
                else
                {
                    DisplayCommandGroupIfHasPageOrButton();
                }
            }
            catch
            { }
        }

        void LoadButtonOrDryContactOrLogicTablesRemarkToCboKeyIfNeeds()
        {
            if (MyActiveObject == null) return;
            cboRemote.Items.Clear();
            for (int i = 0; i < remoteSum; i++) cboRemote.Items.Add((i + 1).ToString()); 
            cbKey.Items.Clear();
            try
            {
                if (MyActiveObject is IPModule)
                {
                    TmpIpModule = (IPModule)MyActiveObject;
                   if (TmpIpModule != null)
                    {
                        #region
                        if (remoteId == -1 || pageId ==-1) return;
                        for (int i = 0; i < 8; i++)
                        {
                            cbKey.Items.Add(TmpIpModule.MyRemoteControllers[remoteId * 48 + pageId * 8 + i].ID + "( " +
                                            TmpIpModule.MyRemoteControllers[remoteId * 48 + pageId * 8 + i].Remark + " ) -- " +
                                            ButtonMode.ConvertorKeyModeToPublicModeGroup(TmpIpModule.MyRemoteControllers[remoteId * 48 + pageId * 8 + i].Mode));
                        }
                        #endregion
                    }
                }
                else if (MyActiveObject is Panel)
                {
                    TmpPanel = (Panel)MyActiveObject;
                    if (TmpPanel != null)
                    {
                        #region
                        if (remoteId == -1 || pageId == -1) return;
                        Byte currentButtons = 8;
                        if (TmpPanel.PanelKey.Count <= 8) currentButtons = (Byte)TmpPanel.PanelKey.Count;
                        for (int i = 0; i < currentButtons; i++)
                        {
                            cbKey.Items.Add(TmpPanel.PanelKey[pageId * 8 + i].ID + "( " + TmpPanel.PanelKey[pageId * 8 + i].Remark + " ) -- " +
                                            ButtonMode.ConvertorKeyModeToPublicModeGroup(TmpPanel.PanelKey[pageId * 8 + i].Mode));
                        }
                        #endregion
                    }
                }
                else if (MyActiveObject is ColorDLP)
                {
                    TmpEnviroOld = (ColorDLP)MyActiveObject;
                    if (TmpEnviroOld != null)
                    {
                        #region
                        if (remoteId == -1 || pageId == -1) return;
                        Byte currentButtons = 9;
                        if (TmpEnviroOld.MyKeys.Count <= currentButtons) currentButtons = (Byte)TmpEnviroOld.MyKeys.Count;
                        for (int i = 0; i < currentButtons; i++)
                        {
                            cbKey.Items.Add(TmpEnviroOld.MyKeys[pageId * currentButtons + i].ID + "( " + TmpEnviroOld.MyKeys[pageId * currentButtons + i].Remark + " ) -- " +
                                            ButtonMode.ConvertorKeyModeToPublicModeGroup(TmpEnviroOld.MyKeys[pageId * currentButtons + i].Mode));
                        }
                        #endregion
                    }
                }
                else if (MyActiveObject is EnviroPanel)
                {
                    TmpEnviro = (EnviroPanel)MyActiveObject;
                    if (TmpEnviro != null)
                    {
                        #region
                        if (remoteId == -1 || pageId == -1) return;
                        Byte currentButtons = 12;
                        if (TmpEnviro.MyKeys.Count <= currentButtons) currentButtons = (Byte)TmpEnviro.MyKeys.Count;
                        for (int i = 0; i < currentButtons; i++)
                        {
                            cbKey.Items.Add(TmpEnviro.MyKeys[pageId * currentButtons + i].ID + "( " + TmpEnviro.MyKeys[pageId * currentButtons + i].Remark + " ) -- " +
                                            ButtonMode.ConvertorKeyModeToPublicModeGroup(TmpEnviro.MyKeys[pageId * currentButtons + i].Mode));
                        }
                        #endregion
                    }
                }
                else if (MyActiveObject is MS04)
                {
                    TmpMS04 = (MS04)MyActiveObject;
                    if (TmpMS04 != null)
                    {
                        #region
                        foreach (HDLButton tmp in TmpMS04.MSKeys)
                        {
                            cbKey.Items.Add(tmp.ID + "( " + tmp.Remark + " ) -- " + DryMode.ConvertorKeyModeToPublicModeGroup(tmp.Mode));
                        }
                        #endregion
                    }
                }
                else if (MyActiveObject is MHRCU)
                {
                    TmpMHRcu = (MHRCU)MyActiveObject; 
                    if (TmpMHRcu != null)
                    {
                        #region
                        if (pageId == 0) // 干节点
                        {
                            foreach (HDLButton tmp in TmpMHRcu.MSKeys)
                            {
                                cbKey.Items.Add(tmp.ID + "( " + tmp.Remark + " )");
                            }
                        }
                        else if (pageId == 1) // 232 --> 485
                        {
                            foreach (Rs232ToBus tmp in TmpMHRcu.myRS2BUS)
                            {
                                cbKey.Items.Add(tmp.ID + "( " + tmp.remark + " )");
                            }
                        }
                        else if (pageId == 2) // 485 --> HDL 485
                        {
                            foreach (Rs232ToBus tmp in TmpMHRcu.my4852BUS)
                            {
                                cbKey.Items.Add(tmp.ID + "( " + tmp.remark + " )");
                            }
                        }
                        else if (pageId == 3) // 逻辑功能配置
                        {
                            foreach (MHRCU.RCULogic tmp in TmpMHRcu.myLogic)
                            {
                                cbKey.Items.Add(tmp.ID + "( " + tmp.Remark + " )");
                            }
                        }
                        #endregion
                    }
                }
                else if (MyActiveObject is MiniSensor)
                {
                    TmpMiniSensor = (MiniSensor)MyActiveObject;
                    if (TmpMiniSensor != null && TmpMiniSensor.logic != null && TmpMiniSensor.logic.Count != 0)
                    {
                        #region
                        foreach (SensorLogic tmp in TmpMiniSensor.logic)
                        {
                            cbKey.Items.Add(tmp.ID.ToString() + "-" + tmp.Remarklogic);
                        }
                        #endregion
                    }
                }
                else if (MyActiveObject is MHIC) // 插卡取电模块
                {
                    #region
                    Byte currentButtons = 8;
                    MHIC oTmpMhic = (MHIC)MyActiveObject;
                    if (oTmpMhic.myKeySetting.Count <= 8) currentButtons = Convert.ToByte(oTmpMhic.myKeySetting.Count.ToString());
                    for (int i = 0; i < currentButtons; i++)
                    {
                        cbKey.Items.Add(oTmpMhic.myKeySetting[i].ID + "( " + oTmpMhic.myKeySetting[i].Remark + " ) -- " +
                                        ButtonMode.ConvertorKeyModeToPublicModeGroup(oTmpMhic.myKeySetting[i].Mode));
                    }
                    #endregion
                }
                else if (MyActiveObject is MSPU)
                {
                    TmpWUSensor = (MSPU)MyActiveObject;
                    if (TmpWUSensor != null && TmpWUSensor.logic != null && TmpWUSensor.logic.Count != 0)
                    {
                        #region
                        foreach (SensorLogic tmp in TmpWUSensor.logic)
                        {
                            cbKey.Items.Add(tmp.ID.ToString() + "-" + tmp.Remarklogic);
                        }
                        #endregion
                    }
                }
                else if (MyActiveObject is MultiSensor)
                {
                    Tmp8in1 = (MultiSensor)MyActiveObject;
                    if (Tmp8in1 != null && Tmp8in1.logic != null && Tmp8in1.logic.Count != 0)
                    {
                        #region
                        foreach (SensorLogic tmp in Tmp8in1.logic)
                        {
                            cbKey.Items.Add(tmp.ID.ToString() + "-" + tmp.Remarklogic);
                        }
                        #endregion
                    }
                }
                else if (MyActiveObject is Sensor_12in1)
                {
                    Tmp12in1 = (Sensor_12in1)MyActiveObject;

                    if (remoteId == 1)  // 红外接收页面
                    {
                        #region
                        if (remoteId == -1 || pageId == -1) return;
                        for (int i = 0; i < 8; i++)
                        {
                            cbKey.Items.Add((i + 1).ToString() + "( " +
                                            Tmp12in1.IrReceiver[pageId * 8 + i].IRBtnRemark + " ) -- " +
                                            ButtonMode.ConvertorKeyModeToPublicModeGroup(Tmp12in1.IrReceiver[pageId * 8 + i].IRBtnModel));
                        }
                        #endregion
                    }
                    else  // 逻辑界面
                    {
                        if (Tmp12in1 != null && Tmp12in1.logic != null && Tmp12in1.logic.Count != 0)
                        {
                            #region
                            foreach (Sensor_12in1.Logic tmp in Tmp12in1.logic)
                            {
                                cbKey.Items.Add(tmp.ID.ToString() + "-" + tmp.Remarklogic);
                            }
                            #endregion
                        }
                    }
                }
            }
            catch
            { }
        }

        private void cboPage_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboPages.SelectedIndex == -1)
            {
                cboPages.SelectedIndex = pageId;
            }
            else
            {
                pageId = cboPages.SelectedIndex;
                LoadButtonOrDryContactOrLogicTablesRemarkToCboKeyIfNeeds();
                cbKey_SelectedIndexChanged(cbKey, null);
            }
        }

        private void cbKey_SelectedIndexChanged(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            if (blnIsLoading) return;
            setAllControlVisible(false);
            buttonId = (Byte)cbKey.SelectedIndex;

            TmpButton = null;
            GetPublcButtonBasicInformation();
            if (TmpButton != null)
            {
                DisplayCmdDirectlyWithDifferentDeviceType();
            }
            Cursor.Current = Cursors.Default;
        }

        private void GetPublcButtonBasicInformation()
        {
            try
            {
                if (TmpIpModule != null) //一端口交换机
                {
                    if (buttonId > cbKey.Items.Count) buttonId = 0;
                    TmpButton = TmpIpModule.MyRemoteControllers[remoteId * 48 + pageId * 8 + buttonId];
                }
                else if (TmpPanel != null) //面板
                {
                    Byte PageID = 0;
                    if (cboPages.Visible) PageID = Convert.ToByte(pageId);
                    int wdMaxValue = DeviceTypeList.GetMaxValueFromPublicModeGroup(mywdDevicerType);
                    if (buttonId == 255 || buttonId > cbKey.Items.Count) buttonId = 0;
                    TmpButton = TmpPanel.PanelKey[buttonId + PageID * wdMaxValue];
                }
                else if (MS04DeviceTypeList.HDLMS04DeviceTypeList.Contains(mywdDevicerType))
                {
                    TmpButton = ((MS04)MyActiveObject).MSKeys[buttonId];
                }
                else if (MHICDeviceTypeList.HDLCardReaderDeviceType.Contains(mywdDevicerType)) // 插卡去电面板
                {
                    TmpButton = ((MHIC)MyActiveObject).myKeySetting[buttonId];
                }
                else if (EnviroNewDeviceTypeList.EnviroNewPanelDeviceTypeList.Contains(mywdDevicerType)) // 彩屏面板
                {
                    if (buttonId < 100)
                    {
                        TmpButton = ((EnviroPanel)MyActiveObject).MyKeys[buttonId];
                    }
                }
                else if (EnviroDLPPanelDeviceTypeList.HDLEnviroDLPPanelDeviceTypeList.Contains(mywdDevicerType)) // 旧版彩屏面板
                {
                    if (buttonId < 100)
                    {
                        TmpButton = TmpEnviroOld.MyKeys[buttonId];
                    }
                }
                else if (HotelMixModuleDeviceType.HDLRCUDeviceTypeLists.Contains(mywdDevicerType) && pageId == 0) // 混合主机
                {
                    if (pageId == 0)
                    {
                        TmpButton = ((MHRCU)MyActiveObject).MSKeys[buttonId];
                    }
                }
            }
            catch { }
        }

        void DisplayButtonCommandsGroup()
        {
            rbShortOff.Visible = false;
            rbLongPressON.Visible = false;
            try
            {
                //显示基本信息
                if (TmpButton != null)
                {
                    Byte TotalCMD = HDLSysPF.GetButtonTotalCMDFromPublicModeList(TmpButton.Mode);
                    #region
                    if (MS04DeviceTypeList.HDLMS04DeviceTypeList.Contains(mywdDevicerType) ||
                        HotelMixModuleDeviceType.HDLRCUDeviceTypeLists.Contains(mywdDevicerType))
                    {
                        if (TmpButton.Mode == 0 || pageId == 3) // 机械开关
                        {
                            TotalCMD = 99;
                            rbShortOff.Visible = true;
                            rbLongPressON.Visible = true;
                            if (rbShortOff.Checked == false && rbLongPressON.Checked == false) rbShortOff.Checked = true;
                        }
                        else if (TmpButton.Mode == 9 || pageId == 3)
                        {
                            TotalCMD = 99;
                            rbShortOff.Visible = true;
                            rbLongPressON.Visible = true;
                            if (rbShortOff.Checked == false && rbLongPressON.Checked == false) rbShortOff.Checked = true;
                        }
                    }
                    else if ((TmpButton.Mode >= 8 && TmpButton.Mode <= 12) || (TmpButton.Mode >=14 && TmpButton.Mode <= 19) || TmpButton.Mode == 21) // 机械开关时补充状态显示
                    {
                        rbShortOff.Visible = true;
                        rbLongPressON.Visible = true;
                        rbShortOff.Text = "OFF";
                        rbLongPressON.Text = "ON";
                    }
                    else
                    {
                        txtFrm.Text = "1";
                        txtTo.Text = TotalCMD.ToString();
                    }
                    #endregion
                    //设置有效的目标数据
                    txtFrm.Enabled = (TotalCMD > 1);
                    txtTo.Enabled = (TotalCMD > 1);

                    dgvListA.Rows.Clear();
                    Byte bCurrentCmdID = 1;

                    #region
                    if (TmpButton.KeyTargets != null && TmpButton.KeyTargets.Count != 0)
                    {
                        if (rbShortOff.Visible == false && rbLongPressON.Visible == false)
                        {
                            txtTo.Text = TmpButton.KeyTargets.Count.ToString();
                        }
                        else if (rbShortOff.Visible == true && rbShortOff.Checked == true)
                        {
                            txtFrm.Text = "50";
                            txtTo.Text = "99";
                        }
                        else if (rbLongPressON.Visible == true && rbLongPressON.Checked == true)
                        {
                            txtFrm.Text = "1";
                            txtTo.Text = "49";
                        }

                        foreach (UVCMD.ControlTargets oTmp in TmpButton.KeyTargets)
                        {
                            if (DisplayCMDToDataGrid(oTmp) == true)
                            {
                                bCurrentCmdID++;
                            }
                            if (bCurrentCmdID > TotalCMD) break;
                        }

                    }
                    #endregion
                }
            }
            catch { }
        }

        private void btnSure_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            dgvListA.Rows.Clear();
            try
            {
                Byte bStartCmd = Convert.ToByte(txtFrm.Text.ToString());
                Byte bToCmd = Convert.ToByte(txtTo.Text.ToString());

                #region
                if (DLPPanelDeviceTypeList.HDLDLPPanelDeviceTypeList.Contains(mywdDevicerType) && buttonId == 101)  // DLP 音乐界面
                {
                    #region
                    DLP TmpDLP = (DLP)MyActiveObject;
                    TmpDLP.DLPFH.ReadFloorheatingAdvancedCommandsGroup(SubNetID, DeviceID, mywdDevicerType);
                    DisplayWithVisibleControlsIfHasRemoteOrLogicBlock();
                    #endregion
                }
                if (NormalPanelDeviceTypeList.WirelessSimpleFloorHeatingDeviceTypeList.Contains(mywdDevicerType) && buttonId == 101)  // ismart panel 地热页面
                {
                    #region
                    Panel TmpDLP = (Panel)MyActiveObject;
                    TmpDLP.DLPFH.ReadFloorheatingAdvancedCommandsGroup(SubNetID, DeviceID, mywdDevicerType);
                    DisplayWithVisibleControlsIfHasRemoteOrLogicBlock();
                    #endregion
                }
                else if (EnviroNewDeviceTypeList.EnviroNewPanelDeviceTypeList.Contains(mywdDevicerType))  //彩屏面板
                {
                    #region
                    EnviroPanel TmpDLP = (EnviroPanel)MyActiveObject;
                    TmpDLP.MyHeat[remoteId].ReadFloorheatingAdvancedCommandsGroup(SubNetID, DeviceID, mywdDevicerType);
                    DisplayWithVisibleControlsIfHasRemoteOrLogicBlock();
                    #endregion
                }
                else if (TmpSensorBlock != null) // 传感器 // 8in1传感器
                {
                    #region
                    if (rbShortOff.Checked == true)
                    {
                        TmpSensorBlock.DownloadLogicTrueCommandsFromDevice(SubNetID, DeviceID, mywdDevicerType, bStartCmd,  bToCmd);
                    }
                    else if (rbLongPressON.Checked == true)
                    {
                        TmpSensorBlock.DownloadLogicFalseCommandsFromDevice(SubNetID, DeviceID, mywdDevicerType, bStartCmd, bToCmd);
                    }
                    #endregion
                    DisplayCmdDirectlyWithDifferentDeviceType();
                }
                else if (LogicDeviceTypeList.LogicDeviceType.Contains(mywdDevicerType)) // 逻辑模块
                {
                    #region
                    if (buttonId == -1) return;
                    Byte ControllerID = Convert.ToByte(remoteId);
                    Byte ButtonID = Convert.ToByte(cbKey.Text);

                    TmpLogic = (Logic)MyActiveObject;
                    TmpLogic.DownLoadLogicBlockOneTableCommandsGroup(SubNetID, DeviceID, ControllerID, ButtonID, bStartCmd, bToCmd);
                    foreach (UVCMD.ControlTargets Tmp in TmpLogic.MyDesign[ControllerID].ArmCmds[buttonId])
                    {
                        DisplayCMDToDataGrid(Tmp);
                    }
                    #endregion
                }
                else if (TmpMHRcu != null) // 混合酒店主机
                {
                    #region
                    if (pageId == 0)  // 干节点界面
                    {
                        if (rbLongPressON.Checked == true && rbLongPressON.Visible == true)
                        {
                            ((HDL_Buspro_Setup_Tool.MHRCU)TmpMHRcu).ReadDryCommandsGroupFrmDeviceToBuf(SubNetID, DeviceID, mywdDevicerType, (Byte)buttonId, 0);
                        }
                        else
                        {
                            ((HDL_Buspro_Setup_Tool.MHRCU)TmpMHRcu).ReadDryCommandsGroupFrmDeviceToBuf(SubNetID, DeviceID, mywdDevicerType, (Byte)buttonId, 1);
                        }
                    }
                    else if (pageId == 1) // 232 到485界面
                    {
                        TmpMHRcu.myRS2BUS[buttonId].busTargets = MHRCU.Read232ToBusCommandsGroupFrmDeviceToBuf(SubNetID, DeviceID, mywdDevicerType, (Byte)buttonId);
                    }
                    else if (pageId == 2) // 485 到HDl bus界面
                    {
                        TmpMHRcu.my4852BUS[buttonId].busTargets = MHRCU.Read232ToBusCommandsGroupFrmDeviceToBuf(SubNetID, DeviceID, mywdDevicerType, (Byte)buttonId);
                    }
                    else if (pageId == 3)
                    {
                        if (rbShortOff.Checked == true)
                        {
                            TmpMHRcu.myLogic[buttonId].Setup = HdlUdpPublicFunctions.DownloadLogicTrueCommandsFromDevice(SubNetID, DeviceID, mywdDevicerType, (Byte)buttonId);
                        }
                        else if (rbLongPressON.Checked == true)
                        {
                            TmpMHRcu.myLogic[buttonId].NoSetup = HdlUdpPublicFunctions.DownloadLogicFalseCommandsFromDevice(SubNetID, DeviceID, mywdDevicerType, (Byte)buttonId);
                        }
                    }
                    #endregion
                    DisplayCmdDirectlyWithDifferentDeviceType();
                }
                else if (Tmp12in1 != null) // 12in1
                {
                    #region
                    if (remoteId == 1)  // 红外接收页面
                    {
                        Tmp12in1.ReadIrReceiverCommandGroup(SubNetID, DeviceID, Tmp12in1.IrReceiver[buttonId + pageId * 8].IRBtnModel, (Byte)(buttonId + 1 + pageId * 8));
                    }
                    else
                    {
                        if (rbShortOff.Checked == true)
                        {
                            Tmp12in1.logic[buttonId].SetUp = Tmp12in1.DownloadLogicTrueCommandsFromDevice(SubNetID, DeviceID, mywdDevicerType, (Byte)(buttonId + 1), bStartCmd, bToCmd);
                        }
                        else if (rbLongPressON.Checked == true)
                        {
                            Tmp12in1.logic[buttonId].NoSetUp = Tmp12in1.DownloadLogicFalseCommandsFromDevice(SubNetID, DeviceID, mywdDevicerType, (Byte)(buttonId + 1), bStartCmd, bToCmd);
                        }
                    }
                    #endregion
                    DisplayCmdDirectlyWithDifferentDeviceType();
                }
                else
                {
                    if (TmpButton == null) return;
                    #region
                    // 增加目标设置
                    if (TmpButton is MS04Key) // MS04支持旧命令
                    {
                        #region
                        if (rbShortOff.Checked == true && rbShortOff.Visible == true)
                        {
                            if (MS04DeviceTypeList.MS04IOModuleDeviceTypeList.Contains(mywdDevicerType)) // 新版干节点
                            {
                                ((MS04)MyActiveObject).ReadIOModuleCommandsGroupFrmDeviceToBuf(SubNetID, DeviceID, mywdDevicerType, (Byte)buttonId, 0, bStartCmd, bToCmd);
                            }
                            else if (MS04DeviceTypeList.MS04DryContactWithE000PublicCMD.Contains(mywdDevicerType))
                            {
                                ((MS04)MyActiveObject).ReadButtonRemarkAndCMDFromDevice(SubNetID, DeviceID, mywdDevicerType, (Byte)buttonId, 0, bStartCmd, bToCmd);
                            }
                            else
                            {
                                ((MS04)MyActiveObject).ReadDryCommandsGroupFrmDeviceToBuf(SubNetID, DeviceID, mywdDevicerType, (Byte)buttonId, 0, bStartCmd, bToCmd);
                            }
                            
                            foreach (UVCMD.ControlTargets oTmp in ((MS04Key)TmpButton).KeyOffTargets)
                            {
                                DisplayCMDToDataGrid(oTmp);
                            }
                        }
                        else
                        {
                            if (MS04DeviceTypeList.MS04IOModuleDeviceTypeList.Contains(mywdDevicerType)) // 新版干节点
                            {
                                ((MS04)MyActiveObject).ReadIOModuleCommandsGroupFrmDeviceToBuf(SubNetID, DeviceID, mywdDevicerType, (Byte)buttonId, 1, bStartCmd, bToCmd);
                            }
                            else if (MS04DeviceTypeList.MS04DryContactWithE000PublicCMD.Contains(mywdDevicerType))
                            {
                                ((MS04)MyActiveObject).ReadButtonRemarkAndCMDFromDevice(SubNetID, DeviceID, mywdDevicerType, (Byte)buttonId, 1, bStartCmd, bToCmd);
                            }
                            else
                            {
                                ((MS04)MyActiveObject).ReadDryCommandsGroupFrmDeviceToBuf(SubNetID, DeviceID, mywdDevicerType, (Byte)buttonId, 1, bStartCmd, bToCmd);
                            }
                            foreach (UVCMD.ControlTargets oTmp in TmpButton.KeyTargets)
                            {
                                DisplayCMDToDataGrid(oTmp);
                            }
                        }
                        #endregion
                    }
                    else
                    {
                        #region
                        if (TmpButton.Mode != 0 && CsConst.MyEditMode == 1)
                        {
                            TmpButton.ReadButtonRemarkAndCMDFromDevice(SubNetID, DeviceID, mywdDevicerType, remoteId, 1, false, bStartCmd, bToCmd);
                        }

                        if (TmpButton.KeyTargets != null && TmpButton.KeyTargets.Count != 0)
                        {
                            txtTo.Text = TmpButton.KeyTargets.Count.ToString();
                            DisplayCmdDirectlyWithDifferentDeviceType();
                        }
                        #endregion
                    }
                    #endregion
                }
                #endregion
            }
            catch
            { }
            Cursor.Current = Cursors.Default;
        }

        void txtbox4_TextChanged(object sender, EventArgs e)
        {
            if (dgvListA.CurrentRow.Index < 0) return;
            if (dgvListA.RowCount <= 0) return;
            int index = dgvListA.CurrentRow.Index;
            string str = txtbox4.Text;
            if (str.Contains("(")) str = str.Split('(')[0].ToString();
            if (txtbox4.TextLength > 0)
            {
                if (cbControlType.Text == CsConst.myPublicControlType[19].ControlTypeName)//逻辑灯调节
                {
                    txtbox4.Text = HDLPF.IsNumStringMode(str, 0, 255);
                    dgvListA[7, index].Value = txtbox4.Text + "(" + CsConst.WholeTextsList[2438].sDisplayName + ")";
                }
                if (cbControlType.Text == CsConst.myPublicControlType[21].ControlTypeName)//休眠模式
                {
                    txtbox4.Text = HDLPF.IsNumStringMode(str, 0, 100);
                    dgvListA[7, index].Value = txtbox4.Text + "(" + CsConst.WholeTextsList[2436].sDisplayName + ")";
                }
                txtbox4.SelectionStart = txtbox4.Text.Length;
            }
            if (dgvListA.SelectedRows == null || dgvListA.SelectedRows.Count == 0) return;
            string strTmp = dgvListA[7, index].Value.ToString();
            if (strTmp == null) strTmp = "N/A";
            if (dgvListA.SelectedRows.Count > 1)
            {
                for (int i = 0; i < dgvListA.SelectedRows.Count; i++)
                {
                    if (dgvListA.SelectedRows[i].Cells[3].Value.ToString() == dgvListA[3, index].Value.ToString())
                        dgvListA.SelectedRows[i].Cells[7].Value = strTmp;
                }
            }
        }

        void txtbox3_TextChanged(object sender, EventArgs e)
        {
            if (dgvListA.CurrentRow.Index < 0) return;
            if (dgvListA.RowCount <= 0) return;
            int index = dgvListA.CurrentRow.Index;
            string str = txtbox3.Text;
            if (str.Contains("(")) str = str.Split('(')[0].ToString();
            #region
            if (cbControlType.Text == CsConst.myPublicControlType[18].ControlTypeName)//红外控制
            {
                if (cbbox1.SelectedIndex == 2)
                {
                    txtbox3.Text = HDLPF.IsNumStringMode(str, 0, 255);
                    dgvListA[6, index].Value = txtbox3.Text + "(" + CsConst.mstrINIDefault.IniReadValue("public", "99819", "") + ")";
                    txtbox3.SelectionStart = txtbox3.Text.Length;
                }
            }
            else if (cbControlType.Text == CsConst.myPublicControlType[19].ControlTypeName)//逻辑灯调节
            {
                txtbox3.Text = HDLPF.IsNumStringMode(str, 0, 100);
                dgvListA[6, index].Value = txtbox3.Text + "(" + CsConst.mstrINIDefault.IniReadValue("public", "00011", "") + ")";
                txtbox3.SelectionStart = txtbox3.Text.Length;
            }
            else if (cbControlType.Text == CsConst.myPublicControlType[21].ControlTypeName)//休眠模式
            {
                txtbox3.Text = HDLPF.IsNumStringMode(str, 0, 100);
                dgvListA[6, index].Value = txtbox3.Text + "(" + CsConst.WholeTextsList[2435].sDisplayName + ")";
                txtbox3.SelectionStart = txtbox3.Text.Length;
            }
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
                                 dgvListA[6, index].Value = txtbox3.Text + "(" + CsConst.mstrINIDefault.IniReadValue("public", "00046", "") + ")";
                            }
                            else if (cbbox2.SelectedIndex == 5)//频道号
                            {
                                txtbox3.Text = HDLPF.IsNumStringMode(str, 1, 50);
                                 dgvListA[6, index].Value = txtbox3.Text + "(" + CsConst.mstrINIDefault.IniReadValue("public", "99873", "") + ")";
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
                         dgvListA[6, index].Value = txtbox3.Text + "(" + CsConst.mstrINIDefault.IniReadValue("public", "00043", "") + ")";
                        txtbox3.SelectionStart = txtbox3.Text.Length;
                    }
                }
            }
            #endregion
            if ( dgvListA.SelectedRows == null ||  dgvListA.SelectedRows.Count == 0) return;
            string strTmp =  dgvListA[6, index].Value.ToString();
            if (strTmp == null) strTmp = "N/A";
            if ( dgvListA.SelectedRows.Count > 1)
            {
                for (int i = 0; i <  dgvListA.SelectedRows.Count; i++)
                {
                    if ( dgvListA.SelectedRows[i].Cells[3].Value.ToString() ==  dgvListA[3, index].Value.ToString() &&
                         dgvListA.SelectedRows[i].Cells[4].Value.ToString() ==  dgvListA[4, index].Value.ToString() &&
                         dgvListA.SelectedRows[i].Cells[5].Value.ToString() ==  dgvListA[5, index].Value.ToString())
                         dgvListA.SelectedRows[i].Cells[6].Value = strTmp;
                }
            }
        }

        void ModifyMultilinesIfNeeds(string strTmp, int ColumnIndex)
        {
            if ( dgvListA.SelectedRows == null ||  dgvListA.SelectedRows.Count == 0) return;
            if (strTmp == null) strTmp = "";
            // change the value in selected more than one line
            if ( dgvListA.SelectedRows.Count > 1)
            {
                for (int i = 0; i <  dgvListA.SelectedRows.Count; i++)
                {
                     dgvListA.SelectedRows[i].Cells[ColumnIndex].Value = strTmp;
                }
            }
        }

        void txtSeries_TextChanged(object sender, EventArgs e)
        {
            if ( dgvListA.CurrentRow.Index < 0) return;
            if ( dgvListA.RowCount <= 0) return;
            int index =  dgvListA.CurrentRow.Index;
            string str = HDLPF.GetStringFromTime(Convert.ToInt32(txtSeries.Text), ":");
            if (txtSeries.Visible)
            {
                 dgvListA[6, index].Value = str + "(" + CsConst.WholeTextsList[2525].sDisplayName + ")";
                if ( dgvListA.SelectedRows == null ||  dgvListA.SelectedRows.Count == 0) return;
                string strTmp =  dgvListA[6, index].Value.ToString();
                if (strTmp == null) strTmp = "N/A";
                if ( dgvListA.SelectedRows.Count > 1)
                {
                    for (int i = 0; i <  dgvListA.SelectedRows.Count; i++)
                    {
                        if ( dgvListA.SelectedRows[i].Cells[3].Value.ToString() == CsConst.myPublicControlType[5].ControlTypeName ||
                             dgvListA.SelectedRows[i].Cells[3].Value.ToString() == CsConst.myPublicControlType[11].ControlTypeName)
                             dgvListA.SelectedRows[i].Cells[6].Value = strTmp;
                    }
                }
            }
        }

        void txtbox2_TextChanged(object sender, EventArgs e)
        {
            if ( dgvListA.CurrentRow.Index < 0) return;
            if ( dgvListA.RowCount <= 0) return;
            int index =  dgvListA.CurrentRow.Index;
            string str = txtbox2.Text;
            if (str.Contains("(")) str = str.Split('(')[0].ToString();
            #region
            if (txtbox2.Text.Length > 0)
            {
                if (cbControlType.Text == CsConst.myPublicControlType[1].ControlTypeName || //场景
                    cbControlType.Text == CsConst.myPublicControlType[10].ControlTypeName ||//广播场景
                    cbControlType.Text == CsConst.myPublicControlType[20].ControlTypeName)  //逻辑场景
                {
                    txtbox2.Text = HDLPF.IsNumStringMode(str, 0, 255);
                    dgvListA[5, index].Value = txtbox2.Text + "(" + CsConst.WholeTextsList[2511].sDisplayName + ")";
                }
                else if (cbControlType.Text == CsConst.myPublicControlType[2].ControlTypeName)//序列
                {
                    txtbox2.Text = HDLPF.IsNumStringMode(str, 0, 99);

                    dgvListA[5, index].Value = txtbox2.Text + "(" + CsConst.WholeTextsList[2512].sDisplayName+ ")";
                }
                else if (cbControlType.Text == CsConst.myPublicControlType[5].ControlTypeName || //单路调节
                         cbControlType.Text == CsConst.myPublicControlType[11].ControlTypeName) //广播回路
                {
                    txtbox2.Text = HDLPF.IsNumStringMode(str, 0, 100);
                   
                    dgvListA[5, index].Value = txtbox2.Text + "(" + CsConst.mstrINIDefault.IniReadValue("public", "00011", "") + ")";
                }
                else if (cbControlType.Text == CsConst.myPublicControlType[7].ControlTypeName)//GPRS控制
                {
                    txtbox2.Text = HDLPF.IsNumStringMode(str, 1, 99);
                    dgvListA[5, index].Value = txtbox2.Text + "(" + CsConst.mstrINIDefault.IniReadValue("public", "99864", "") + ")";
                }
                else if (cbControlType.Text == CsConst.myPublicControlType[8].ControlTypeName)//面板控制
                {
                    if (cbPanleControl.Text == CsConst.myPublicControlType[3].ControlTypeName ||//背光亮度
                        cbPanleControl.Text == CsConst.myPublicControlType[4].ControlTypeName) //状态灯亮度
                    {
                        txtbox2.Text = HDLPF.IsNumStringMode(str, 0, 100);
                        dgvListA[5, index].Value = txtbox2.Text + "(" + CsConst.WholeTextsList[2524].sDisplayName + ")";
                    }
                    else if (cbPanleControl.Text ==  CsConst.PanelControl[15] ||//空调升高温度 
                             cbPanleControl.Text ==  CsConst.PanelControl[16] ||//空调降低温度
                             cbPanleControl.Text == CsConst.PanelControl[23] ||//地热身高温度
                             cbPanleControl.Text == CsConst.PanelControl[24]) //地热降低温度
                    {
                        txtbox2.Text = HDLPF.IsNumStringMode(str, 1, 255);
                        dgvListA[5, index].Value = txtbox2.Text;
                    }
                }
                else if (cbControlType.Text == CsConst.myPublicControlType[13].ControlTypeName)//音乐播放
                {
                    if (cbAudioControl.SelectedIndex == 5)//歌曲播放
                    {
                        txtbox2.Text = HDLPF.IsNumStringMode(str, 0, 255);
                        dgvListA[5, index].Value = txtbox2.Text + "(" + CsConst.mstrINIDefault.IniReadValue("public", "00046", "") + ")";
                    }
                }
                else if (cbControlType.Text == CsConst.myPublicControlType[18].ControlTypeName)//红外控制
                {
                    txtbox2.Text = HDLPF.IsNumStringMode(str, 0, 255);
                    dgvListA[5, index].Value = txtbox2.Text + "(" + CsConst.WholeTextsList[216].sDisplayName + ")";
                }
                else if (cbControlType.Text == CsConst.myPublicControlType[18].ControlTypeName)//逻辑灯调节
                {
                    txtbox2.Text = HDLPF.IsNumStringMode(str, 1, 4);
                    dgvListA[5, index].Value = txtbox2.Text + "(" + CsConst.WholeTextsList[2439].sDisplayName + ")";
                }
                txtbox2.SelectionStart = txtbox2.TextLength;
            }
            #endregion
        }

        void txtbox1_TextChanged(object sender, EventArgs e)
        {
            if ( dgvListA.CurrentRow.Index < 0) return;
            if ( dgvListA.RowCount <= 0) return;
            int index =  dgvListA.CurrentRow.Index;
            string str = txtbox1.Text;
            if (str.Contains("(")) str = str.Split('(')[0].ToString();
            #region
            if (txtbox1.Text.Length > 0)
            {
                if (cbControlType.Text == CsConst.myPublicControlType[1].ControlTypeName || //场景
                    cbControlType.Text == CsConst.myPublicControlType[2].ControlTypeName ||//序列
                    cbControlType.Text == CsConst.myPublicControlType[20].ControlTypeName)   //逻辑场景
                {
                    txtbox1.Text = HDLPF.IsNumStringMode(str, 1, 48);
                    dgvListA[4, index].Value = txtbox1.Text + "(" + CsConst.WholeTextsList[2510].sDisplayName + ")";
                }
                else if (cbControlType.Text == CsConst.myPublicControlType[4].ControlTypeName)//通用开关
                {
                    txtbox1.Text = HDLPF.IsNumStringMode(str, 1, 255);
                    dgvListA[4, index].Value = txtbox1.Text + "(" + CsConst.WholeTextsList[2513].sDisplayName + ")";
                }
                else if (cbControlType.Text == CsConst.myPublicControlType[5].ControlTypeName)//单路调节
                {
                    txtbox1.Text = HDLPF.IsNumStringMode(str, 1, 240);
                    dgvListA[4, index].Value = txtbox1.Text + "(" + CsConst.WholeTextsList[934].sDisplayName + ")";
                }
                else if (cbControlType.Text == CsConst.myPublicControlType[6].ControlTypeName)//窗帘开关
                {
                    txtbox1.Text = HDLPF.IsNumStringMode(str, 1, 16);
                    dgvListA[4, index].Value = txtbox1.Text + "(" + CsConst.mstrINIDefault.IniReadValue("public", "99844", "") + ")";
                }
                else if (cbControlType.Text == CsConst.myPublicControlType[12].ControlTypeName)//消防模块
                {
                    txtbox1.Text = HDLPF.IsNumStringMode(str, 1, 8);
                    dgvListA[4, index].Value = txtbox1.Text + "(" + CsConst.WholeTextsList[2510].sDisplayName + ")";
                }
                else if (cbControlType.Text == CsConst.myPublicControlType[15].ControlTypeName)//连接页
                {
                    txtbox1.Text = HDLPF.IsNumStringMode(str, 1, 7);
                    dgvListA[4, index].Value = txtbox1.Text + "(" + CsConst.WholeTextsList[2510].sDisplayName + ")";
                }
                else if (cbControlType.Text == CsConst.myPublicControlType[20].ControlTypeName)//逻辑灯调节
                {
                    txtbox1.Text = HDLPF.IsNumStringMode(str, 0, 255);
                    dgvListA[4, index].Value = txtbox1.Text + "(" + CsConst.WholeTextsList[2440].sDisplayName + ")";
                }
                else if (cbControlType.Text == CsConst.myPublicControlType[18].ControlTypeName)//红外控制
                {
                    txtbox1.Text = HDLPF.IsNumStringMode(str, 0, 255);
                    dgvListA[4, index].Value = txtbox1.Text + "(" + CsConst.mstrINIDefault.IniReadValue("public", "99921", "") + ")";
                }
                txtbox1.SelectionStart = txtbox1.Text.Length;
            }
            #endregion

            if (dgvListA.SelectedRows == null || dgvListA.SelectedRows.Count == 0) return;
            string strTmp = dgvListA[4, index].Value.ToString();
            if (strTmp == null) strTmp = "N/A";
            if (dgvListA.SelectedRows.Count > 1)
            {
                for (int i = 0; i < dgvListA.SelectedRows.Count; i++)
                {
                    if (dgvListA.SelectedRows[i].Cells[3].Value.ToString() == dgvListA[3, index].Value.ToString())
                        dgvListA.SelectedRows[i].Cells[4].Value = strTmp;
                }
            }
        }

        void txtDev_TextChanged(object sender, EventArgs e)
        {
            if (dgvListA.CurrentRow.Index < 0) return;
            if (dgvListA.RowCount <= 0) return;
            int index = dgvListA.CurrentRow.Index;
            if (txtDev.Text.Length > 0)
            {
                txtDev.Text = HDLPF.IsNumStringMode(txtDev.Text, 0, 254);
                txtDev.SelectionStart = txtDev.Text.Length;
                dgvListA[2, index].Value = txtDev.Text;
                ModifyMultilinesIfNeeds(dgvListA[2, index].Value.ToString(), 2);
            }
        }

        void txtSub_TextChanged(object sender, EventArgs e)
        {
            if (dgvListA.CurrentRow.Index < 0) return;
            if (dgvListA.RowCount <= 0) return;
            int index = dgvListA.CurrentRow.Index;
            if (txtSub.Text.Length > 0)
            {
                txtSub.Text = HDLPF.IsNumStringMode(txtSub.Text, 0, 254);
                txtSub.SelectionStart = txtSub.Text.Length;
                dgvListA[1, index].Value = txtSub.Text;
                ModifyMultilinesIfNeeds(dgvListA[1, index].Value.ToString(), 1);
            }
        }

        void cbbox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dgvListA.CurrentRow.Index < 0) return;
            if (dgvListA.RowCount <= 0) return;
            int index = dgvListA.CurrentRow.Index;
            string str = dgvListA[6, index].Value.ToString();
            if (str.Contains("(")) str = str.Split('(')[0].ToString();
            if (cbPanleControl.Visible && cbPanleControl.Text == CsConst.PanelControl[22])
            {
                if (cbbox3.SelectedIndex == 8)
                    dgvListA[6, index].Value = cbbox3.Text;
                else
                    dgvListA[6, index].Value = cbbox3.Text.ToString() + "(" + CsConst.WholeTextsList[934].sDisplayName + ")";
            }
            else if (cbPanleControl.Visible &&
                    (cbPanleControl.Text == CsConst.PanelControl[25] ||
                     cbPanleControl.Text == CsConst.PanelControl[26] ||
                     cbPanleControl.Text == CsConst.PanelControl[27] ||
                     cbPanleControl.Text == CsConst.PanelControl[28]))
            {
                if (cbbox3.SelectedIndex == 8)
                    dgvListA[6, index].Value = cbbox3.Text;
                else
                    dgvListA[6, index].Value = cbbox3.Text.ToString() + "(" + CsConst.WholeTextsList[934].sDisplayName + ")";
            }
            else
                dgvListA[6, index].Value = cbbox3.Text;
            #region
            if (dgvListA.SelectedRows == null || dgvListA.SelectedRows.Count == 0) return;
            string strTmp = dgvListA[6, index].Value.ToString();
            if (strTmp == null) strTmp = "N/A";
            if (dgvListA.SelectedRows.Count > 1)
            {
                for (int i = 0; i < dgvListA.SelectedRows.Count; i++)
                {
                    if (dgvListA.SelectedRows[i].Cells[3].Value.ToString() == dgvListA[3, index].Value.ToString() &&
                        dgvListA.SelectedRows[i].Cells[4].Value.ToString() == dgvListA[4, index].Value.ToString() &&
                        dgvListA.SelectedRows[i].Cells[5].Value.ToString() == dgvListA[5, index].Value.ToString())
                        dgvListA.SelectedRows[i].Cells[6].Value = strTmp;
                }
            }
            #endregion
        }

        void cbbox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dgvListA.CurrentRow.Index < 0) return;
            if (dgvListA.RowCount <= 0) return;
            txtbox3.Visible = false;
            cbbox3.Visible = false;
            int index = dgvListA.CurrentRow.Index;
            string str = dgvListA[5, index].Value.ToString();
            if (str.Contains("(")) str = str.Split('(')[0].ToString();
            string str3 = dgvListA[6, index].Value.ToString();
            if (str3.Contains("(")) str3 = str3.Split('(')[0].ToString();
            #region
            if (cbControlType.Text == CsConst.myPublicControlType[4].ControlTypeName || //通用开关
                cbControlType.Text == CsConst.myPublicControlType[6].ControlTypeName) //窗帘开关
            {
                dgvListA[5, index].Value = cbbox2.Text + "(" + CsConst.WholeTextsList[2529].sDisplayName + ")";
                if (cbControlType.Text == CsConst.myPublicControlType[6].ControlTypeName)
                {
                    if (cbbox2.SelectedIndex > 2)
                    {
                        txtbox1_TextChanged(null, null);
                    }
                    else
                    {
                        HDLSysPF.addcontrols(4, index, txtbox1, dgvListA);
                    }
                }
            }
            else if (cbControlType.Text == CsConst.myPublicControlType[8].ControlTypeName)//面板控制
            {
                if (cbPanleControl.Text ==CsConst.PanelControl[1] ||
                    cbPanleControl.Text ==CsConst.PanelControl[2] ||
                    cbPanleControl.Text ==CsConst.PanelControl[5] ||
                    cbPanleControl.Text ==CsConst.PanelControl[7] ||
                    cbPanleControl.Text ==CsConst.PanelControl[8] ||
                    cbPanleControl.Text ==CsConst.PanelControl[12] ||
                    cbPanleControl.Text == CsConst.PanelControl[21])
                {
                    dgvListA[5, index].Value = cbbox2.Text + "(" + CsConst.WholeTextsList[2529].sDisplayName + ")";
                }
                else if (cbPanleControl.Text == CsConst.PanelControl[17] ||
                         cbPanleControl.Text == CsConst.PanelControl[18] ||
                         cbPanleControl.Text == CsConst.PanelControl[19] ||
                         cbPanleControl.Text == CsConst.PanelControl[20])
                {
                    dgvListA[5, index].Value = cbbox2.Text + "(" + CsConst.mstrINIDefault.IniReadValue("public", "99874", "") + ")";
                }
                else if (cbPanleControl.Text ==  CsConst.PanelControl[13] ||
                         cbPanleControl.Text == CsConst.PanelControl[22])
                {
                    dgvListA[5, index].Value = cbbox2.Text + "(" + CsConst.mstrINIDefault.IniReadValue("public", "99876", "") + ")";
                }
                else if (cbPanleControl.Text ==  CsConst.PanelControl[14])
                {
                    dgvListA[5, index].Value = cbbox2.Text + "(" + CsConst.mstrINIDefault.IniReadValue("public", "99875", "") + ")";
                }
                else if (cbPanleControl.Text == CsConst.PanelControl[6] ||
                         cbPanleControl.Text == CsConst.PanelControl[9] ||
                         cbPanleControl.Text ==  CsConst.PanelControl[10] ||
                         cbPanleControl.Text ==  CsConst.PanelControl[11])
                {
                    dgvListA[5, index].Value = cbbox2.Text;
                }
                else if (cbPanleControl.Text == CsConst.PanelControl[25] ||
                         cbPanleControl.Text == CsConst.PanelControl[26] ||
                         cbPanleControl.Text == CsConst.PanelControl[27] ||
                         cbPanleControl.Text == CsConst.PanelControl[28])
                {
                    dgvListA[5, index].Value = cbbox2.Text + "(" + CsConst.mstrINIDefault.IniReadValue("public", "99874", "") + ")";
                }
                else if (cbPanleControl.Text == CsConst.PanelControl[29])
                {
                    dgvListA[5, index].Value = cbbox2.Text;
                }
                else if (cbPanleControl.Text == CsConst.PanelControl[30])
                {
                    dgvListA[5, index].Value = cbbox2.Text + "(" + CsConst.mstrINIDefault.IniReadValue("public", "99846", "") + ")";
                }
            }
            else if (cbControlType.Text == CsConst.myPublicControlType[12].ControlTypeName)//消防模块
            {
                dgvListA[5, index].Value = cbbox2.Text;
            }
            else if (cbControlType.Text == CsConst.myPublicControlType[13].ControlTypeName)//音乐模块
            {
                if (cbAudioControl.SelectedIndex == 0 || cbAudioControl.SelectedIndex == 1 ||
                    cbAudioControl.SelectedIndex == 2 || cbAudioControl.SelectedIndex == 3 ||
                    cbAudioControl.SelectedIndex == 4 || cbAudioControl.SelectedIndex == 6 ||
                    cbAudioControl.SelectedIndex == 7)
                {
                    dgvListA[5, index].Value = cbbox2.Text;
                }
            }
            else if (cbControlType.Text == CsConst.myPublicControlType[3].ControlTypeName)//时间开关
            {
                dgvListA[5, index].Value = cbbox2.Text + "(" + CsConst.WholeTextsList[2529].sDisplayName + ")";
            }
            else if (cbControlType.Text == CsConst.myPublicControlType[15].ControlTypeName)//连接页
            {
                dgvListA[5, index].Value = cbbox2.Text;
            }
            else if (cbControlType.Text == CsConst.myPublicControlType[21].ControlTypeName)//休眠模式
            {
                dgvListA[5, index].Value = cbbox2.Text + "(" + CsConst.WholeTextsList[2437].sDisplayName + ")";
                txtbox3.Visible = true;
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
                            HDLSysPF.addcontrols(6, index, txtbox3, dgvListA);
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
                        HDLSysPF.addcontrols(6, index, cbbox3, dgvListA);
                    }
                }
                else if (cbAudioControl.SelectedIndex == 6 || cbAudioControl.SelectedIndex == 7)//直接源播放 广播播放
                {
                    txtbox3.Visible = true;
                }
            }
            else if (cbPanleControl.Visible)
            {
                if ((cbPanleControl.Text == CsConst.PanelControl[6] ||//面板页面锁 
                     cbPanleControl.Text == CsConst.PanelControl[9] ||//按键锁
                     cbPanleControl.Text == CsConst.PanelControl[10] ||//控制按键状态
                     cbPanleControl.Text == CsConst.PanelControl[11] || //控制面板按键
                     cbPanleControl.Text == CsConst.PanelControl[21] ||//地热开关
                     cbPanleControl.Text == CsConst.PanelControl[22])) //地热模式
                {
                    cbbox3.Visible = true;
                }

                //Rub7
                Byte[] ACAndFloorheatingIDList = new Byte[] { 7, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28 };
                if (ACAndFloorheatingIDList.Contains((Byte)cbPanleControl.SelectedIndex))
                {
                    cbbox3.Items.Clear();
                    for (int i = 1; i <= 8; i++)
                        cbbox3.Items.Add(i.ToString());
                    cbbox3.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00049", ""));
                    HDLSysPF.addcontrols(6, index, cbbox3, dgvListA);
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
            if (dgvListA.SelectedRows == null || dgvListA.SelectedRows.Count == 0) return;
            string strTmp = dgvListA[5, index].Value.ToString();
            if (strTmp == null) strTmp = "N/A";
            if (dgvListA.SelectedRows.Count > 1)
            {
                for (int i = 0; i < dgvListA.SelectedRows.Count; i++)
                {
                    if (dgvListA.SelectedRows[i].Cells[3].Value.ToString() == dgvListA[3, index].Value.ToString() &&
                        dgvListA.SelectedRows[i].Cells[4].Value.ToString() == dgvListA[4, index].Value.ToString())
                        dgvListA.SelectedRows[i].Cells[5].Value = strTmp;
                }
            }
            #endregion
        }

        void cbbox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbbox2.Visible = false;
            txtbox3.Visible = false;
            txtbox4.Visible = false;
            if (dgvListA.CurrentRow.Index < 0) return;
            if (dgvListA.RowCount <= 0) return;
            int index = dgvListA.CurrentRow.Index;
            string str = dgvListA[4, index].Value.ToString();
            string str2 = dgvListA[5, index].Value.ToString();
            string str3 = dgvListA[6, index].Value.ToString();
            string str4 = dgvListA[7, index].Value.ToString();
            if (str.Contains("(")) str = str.Split('(')[0].ToString();
            if (str2.Contains("(")) str2 = str2.Split('(')[0].ToString();
            if (str3.Contains("(")) str3 = str3.Split('(')[0].ToString();
            if (str4.Contains("(")) str4 = str4.Split('(')[0].ToString();
            #region
            if (cbControlType.Text == CsConst.myPublicControlType[7].ControlTypeName)//GPRS控制
            {
                dgvListA[4, index].Value = cbbox1.Text;
                txtbox2.Text = str2;
                HDLSysPF.addcontrols(5, index, txtbox2, dgvListA);
            }
            else if (cbControlType.Text == CsConst.myPublicControlType[21].ControlTypeName)//休眠模式
            {
                dgvListA[4, index].Value = cbbox1.Text;
                if (cbbox1.SelectedIndex == 0)
                {
                    cbbox2.Items.Clear();
                    for (int i = 1; i <= 32; i++)
                        cbbox2.Items.Add(i.ToString());
                    cbbox2.Items.Add(CsConst.WholeTextsList[2434].sDisplayName);
                    HDLSysPF.addcontrols(5, index, cbbox2, dgvListA);
                    txtbox3.Text = str3;
                    HDLSysPF.addcontrols(6, index, txtbox3, dgvListA);

                    txtbox4.Text = str4;
                    HDLSysPF.addcontrols(7, index, txtbox4, dgvListA);
                }
                else
                {
                    dgvListA[5, index].Value = "N/A";
                    dgvListA[6, index].Value = "N/A";
                    dgvListA[7, index].Value = "N/A";
                }
            }
            #endregion
            #region
            if (cbbox2.Visible && cbbox2.Items.Count > 0)
            {
                if (!cbbox2.Items.Contains(str2))
                    cbbox2.SelectedIndex = 0;
                else
                    cbbox2.Text = str2;
            }
            #endregion
            #region
            if (cbbox2.Visible) cbbox2_SelectedIndexChanged(null, null);
            if (txtbox3.Visible) txtbox3_TextChanged(null, null);
            if (txtbox4.Visible) txtbox4_TextChanged(null, null);
            #endregion
            #region
            if (dgvListA.SelectedRows == null || dgvListA.SelectedRows.Count == 0) return;
            string strTmp = dgvListA[4, index].Value.ToString();
            if (strTmp == null) strTmp = "N/A";
            if (dgvListA.SelectedRows.Count > 1)
            {
                for (int i = 0; i < dgvListA.SelectedRows.Count; i++)
                {
                    if (dgvListA.SelectedRows[i].Cells[3].Value.ToString() == dgvListA[3, index].Value.ToString())
                        dgvListA.SelectedRows[i].Cells[4].Value = strTmp;
                }
            }

            #endregion
        }

        void cbAudioControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbbox2.Visible = false;
            cbbox3.Visible = false;
            txtbox2.Visible = false;
            txtbox3.Visible = false;
            int index = dgvListA.CurrentRow.Index;
            string str2 = dgvListA[5, index].Value.ToString();
            string str3 = dgvListA[6, index].Value.ToString();
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
                    for (int i = 1; i <= 6; i++)
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

                HDLSysPF.addcontrols(5, index, cbbox2, dgvListA);
                dgvListA[6, index].Value = "N/A";
                #endregion
            }
            else if (cbAudioControl.SelectedIndex == 5)//歌曲播放
            {
                #region
                txtbox2.Text = str2;
                HDLSysPF.addcontrols(5, index, txtbox2, dgvListA);

                txtbox3.Text = str3;
                HDLSysPF.addcontrols(6, index, txtbox3, dgvListA);
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
                HDLSysPF.addcontrols(5, index, cbbox2, dgvListA);
                txtbox3.Text = str3;
                HDLSysPF.addcontrols(6, index, txtbox3, dgvListA);
                #endregion
            }
            dgvListA[4, index].Value = cbAudioControl.Text;
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
        }

        void cbPanleControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbbox2.Visible = false;
            cbbox3.Visible = false;
            txtbox2.Visible = false;
            txtbox3.Visible = false;
            int index = dgvListA.CurrentRow.Index;
            string str2 = dgvListA[5, index].Value.ToString();
            string str3 = dgvListA[6, index].Value.ToString();
            if (str2.Contains("(")) str2 = str2.Split('(')[0].ToString();
            if (str3.Contains("(")) str3 = str3.Split('(')[0].ToString();
            if (cbPanleControl.Text == CsConst.PanelControl[0])//无效
            {
                #region
                dgvListA[5, index].Value = "N/A";
                dgvListA[6, index].Value = "N/A";
                #endregion
            }
            else if (cbPanleControl.Text == CsConst.PanelControl[1] ||//红外接收使能 
                     cbPanleControl.Text == CsConst.PanelControl[2] ||//背光开关
                     cbPanleControl.Text == CsConst.PanelControl[5] ||//面板全锁
                     cbPanleControl.Text == CsConst.PanelControl[7] ||//空调锁
                     cbPanleControl.Text == CsConst.PanelControl[8] ||//配置页面锁 
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
                HDLSysPF.addcontrols(5, index, cbbox2, dgvListA);
                dgvListA[6, index].Value = "N/A";
                if (cbPanleControl.Text ==  CsConst.PanelControl[12] ||//空调开关
                    cbPanleControl.Text ==  CsConst.PanelControl[13] ||//空调模式 
                    cbPanleControl.Text ==  CsConst.PanelControl[14])  //空调风速
                {
                    cbbox3.Items.Clear();
                    for (int i = 1; i <= 8; i++)
                        cbbox3.Items.Add(i.ToString());
                    cbbox3.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00049", ""));
                    HDLSysPF.addcontrols(6, index, cbbox3, dgvListA);
                }
                #endregion
            }
            else if (cbPanleControl.Text == CsConst.PanelControl[3] ||//背光灯亮度
                     cbPanleControl.Text == CsConst.PanelControl[4] ||//状态灯亮度
                     cbPanleControl.Text ==  CsConst.PanelControl[15] ||//空调升高温度 
                     cbPanleControl.Text ==  CsConst.PanelControl[16])//空调降低温度
            {
                #region
                txtbox2.Text = str2;
                HDLSysPF.addcontrols(5, index, txtbox2,dgvListA);
                dgvListA[6, index].Value = "N/A";
                #endregion
            }
            else if (cbPanleControl.Text == CsConst.PanelControl[6] ||//面板页面锁
                     cbPanleControl.Text == CsConst.PanelControl[9] ||//按键锁
                     cbPanleControl.Text ==  CsConst.PanelControl[10] ||//控制按键状态
                     cbPanleControl.Text ==  CsConst.PanelControl[11] ||//控制面板按键
                     cbPanleControl.Text == CsConst.PanelControl[21] ||//地热开关
                     cbPanleControl.Text == CsConst.PanelControl[22]) //地热模式
            {
                #region
                cbbox2.Items.Clear();
                cbbox3.Items.Clear();
                if (cbPanleControl.Text == CsConst.PanelControl[6])//面板页面锁
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
                else if (cbPanleControl.Text == CsConst.PanelControl[9])//按键锁
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
                else if (cbPanleControl.Text == CsConst.PanelControl[22])//地热模式
                {
                    #region
                    for (int i = 0; i <= 4; i++)
                        cbbox2.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "0007" + i.ToString(), ""));

                    for (int i = 1; i <= 8; i++)
                        cbbox3.Items.Add(i.ToString());
                    cbbox3.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00049", ""));
                    #endregion
                }
                HDLSysPF.addcontrols(5, index, cbbox2, dgvListA);
                HDLSysPF.addcontrols(6, index, cbbox3, dgvListA);
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
                HDLSysPF.addcontrols(5, index, cbbox2,dgvListA);
                dgvListA[6, index].Value = "N/A";

                cbbox3.Items.Clear();
                for (int i = 1; i <= 8; i++)
                    cbbox3.Items.Add(i.ToString());
                cbbox3.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00049", ""));
                HDLSysPF.addcontrols(6, index, cbbox3, dgvListA);
                #endregion
            }
            else if (cbPanleControl.Text == CsConst.PanelControl[23] ||//地热身高温度
                     cbPanleControl.Text == CsConst.PanelControl[24]) //地热降低温度
            {
                #region
                txtbox2.Text = str2;
                HDLSysPF.addcontrols(5, index, txtbox2,dgvListA);
                cbbox3.Items.Clear();
                for (int i = 1; i <= 8; i++)
                    cbbox3.Items.Add(i.ToString());
                cbbox3.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00049", ""));
                HDLSysPF.addcontrols(6, index, cbbox3,dgvListA);
                #endregion
            }
            else if (cbPanleControl.Text == CsConst.PanelControl[25] ||//地热普通温度
                     cbPanleControl.Text == CsConst.PanelControl[26] ||//地热白天温度
                     cbPanleControl.Text == CsConst.PanelControl[27] ||//地热夜晚温度 
                     cbPanleControl.Text == CsConst.PanelControl[28])  //地热离开温度
            {
                #region
                cbbox2.Items.Clear();
                for (int i = 5; i <= 35; i++)
                    cbbox2.Items.Add(i.ToString() + "C");
                for (int i = 41; i <= 95; i++)
                    cbbox2.Items.Add(i.ToString() + "F");
                HDLSysPF.addcontrols(5, index, cbbox2,dgvListA);

                cbbox3.Items.Clear();
                for (int i = 1; i <= 8; i++)
                    cbbox3.Items.Add(i.ToString());
                cbbox3.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00049", ""));
                HDLSysPF.addcontrols(6, index, cbbox3, dgvListA);
                #endregion
            }
            else if (cbPanleControl.Text == CsConst.PanelControl[29])//选择页
            {
                #region
                cbbox2.Items.Clear();
                for (int i = 1; i <= 7; i++)
                    cbbox2.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00048", "") + i.ToString());
                #endregion
            }
            else if (cbPanleControl.Text == CsConst.PanelControl[30])//感应目标键
            {
                #region
                cbbox2.Items.Clear();
                cbbox2.Items.Add(CsConst.WholeTextsList[1775].sDisplayName);
                for (int i = 1; i <= 32; i++)
                    cbbox2.Items.Add(i.ToString());
                #endregion
            }
            dgvListA[4, index].Value = cbPanleControl.Text;
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
        }

        void cbControlType_SelectedIndexChanged(object sender, EventArgs e)
        {
            setAllControlVisible(false);
            txtSub.Visible = true;
            txtDev.Visible = true;
            cbControlType.Visible = true;
            int index = dgvListA.CurrentRow.Index;
            string str1 = dgvListA[4, index].Value.ToString();
            string str2 = dgvListA[5, index].Value.ToString();
            string str3 = dgvListA[6, index].Value.ToString();
            if (dgvListA[7,index].Value == null) dgvListA[7,index].Value = "";
            string str4 = dgvListA[7, index].Value.ToString();
            if (str1.Contains('(')) str1 = str1.Split('(')[0].ToString();
            if (str2.Contains('(')) str2 = str2.Split('(')[0].ToString();
            if (str3.Contains('(')) str3 = str3.Split('(')[0].ToString();
            if (str4.Contains('(')) str4 = str4.Split('(')[0].ToString();
            if (cbControlType.Text == CsConst.myPublicControlType[0].ControlTypeName)//无效
            {
                #region
                if (dgvListA.SelectedRows.Count > 0)
                {
                    for (int i = 0; i < dgvListA.SelectedRows.Count; i++)
                    {
                        dgvListA.SelectedRows[i].Cells[3].Value = cbControlType.Items[0].ToString();
                        dgvListA[4, index].Value = "N/A";
                        dgvListA[5, index].Value = "N/A";
                        dgvListA[6, index].Value = "N/A";
                        dgvListA[7, index].Value = "N/A";
                    }
                }
                #endregion
            }
            else if (cbControlType.Text == CsConst.myPublicControlType[1].ControlTypeName ||//场景
                     cbControlType.Text == CsConst.myPublicControlType[2].ControlTypeName) //序列
            {
                #region
                txtbox1.Text = str1;
                HDLSysPF.addcontrols(4, index, txtbox1,dgvListA);

                txtbox2.Text = str2;
                HDLSysPF.addcontrols(5, index, txtbox2,dgvListA);
                 dgvListA[6, index].Value = "N/A";
                 dgvListA[7, index].Value = "N/A";
                #endregion
            }
            else if (cbControlType.Text == CsConst.myPublicControlType[4].ControlTypeName ||//通用开关
                     cbControlType.Text == CsConst.myPublicControlType[6].ControlTypeName || //窗帘开关 
                     cbControlType.Text == CsConst.myPublicControlType[12].ControlTypeName) //消防模块
            {
                #region
                txtbox1.Text = str1;
                HDLSysPF.addcontrols(4, index, txtbox1,dgvListA);

                cbbox2.Items.Clear();
                if (cbControlType.Text == CsConst.myPublicControlType[4].ControlTypeName)
                {
                    cbbox2.Items.AddRange(CsConst.Status);
                }
                else if (cbControlType.Text == CsConst.myPublicControlType[6].ControlTypeName)
                {
                    cbbox2.Items.AddRange(CsConst.CurtainModes.ToArray());
                    for (int i = 1; i <= 100; i++)
                        cbbox2.Items.Add(i.ToString() + "%");
                }
                else if (cbControlType.Text == CsConst.myPublicControlType[12].ControlTypeName)
                {
                    cbbox2.Items.AddRange(CsConst.PanelControl);
                }
                HDLSysPF.addcontrols(5, index, cbbox2,dgvListA);
                 dgvListA[6, index].Value = "N/A";
                 dgvListA[7, index].Value = "N/A";
                #endregion
            }
            else if (cbControlType.Text == CsConst.myPublicControlType[5].ControlTypeName)//单路调节
            {
                #region
                txtbox1.Text = str1;
                HDLSysPF.addcontrols(4, index, txtbox1,dgvListA);

                txtbox2.Text = str2;
                HDLSysPF.addcontrols(5, index, txtbox2,dgvListA);


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
                HDLSysPF.addcontrols(6, index, txtSeries, dgvListA);
                 dgvListA[7, index].Value = "N/A";
                #endregion
            }
            else if (cbControlType.Text == CsConst.myPublicControlType[7].ControlTypeName)//GPRS控制
            {
                #region
                cbbox1.Items.Clear();
                cbbox1.Items.AddRange(CsConst.GPRSControl);
                HDLSysPF.addcontrols(4, index, cbbox1, dgvListA);

                txtbox2.Text = str2;
                HDLSysPF.addcontrols(5, index, txtbox2, dgvListA);
                 dgvListA[6, index].Value = "N/A";
                 dgvListA[7, index].Value = "N/A";
                #endregion
            }
            else if (cbControlType.Text == CsConst.myPublicControlType[8].ControlTypeName)//面板控制
            {
                #region
                HDLSysPF.addcontrols(4, index, cbPanleControl, dgvListA);
                 dgvListA[7, index].Value = "N/A";
                #endregion
            }
            else if (cbControlType.Text == CsConst.myPublicControlType[10].ControlTypeName || //广播场景
                     cbControlType.Text == CsConst.myPublicControlType[11].ControlTypeName) //广播回路
            {
                #region
                if (cbControlType.Text == CsConst.myPublicControlType[10].ControlTypeName)
                {
                     dgvListA[4, index].Value = CsConst.WholeTextsList[2566].sDisplayName;
                     dgvListA[6, index].Value = "N/A";
                }
                else if (cbControlType.Text == CsConst.myPublicControlType[11].ControlTypeName)
                {
                     dgvListA[4, index].Value = CsConst.WholeTextsList[2567].sDisplayName;
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
                    HDLSysPF.addcontrols(6, index, txtSeries, dgvListA);
                }
                txtbox2.Text = str2;
                HDLSysPF.addcontrols(5, index, txtbox2, dgvListA);
                 dgvListA[7, index].Value = "N/A";
                #endregion
            }
            else if (cbControlType.Text == CsConst.myPublicControlType[13].ControlTypeName)//音乐播放
            {
                #region
                HDLSysPF.addcontrols(4, index, cbAudioControl, dgvListA);
                 dgvListA[7, index].Value = "N/A";
                #endregion
            }
            else if (cbControlType.Text == CsConst.myPublicControlType[3].ControlTypeName)//时间开关
            {
                #region
                txtbox1.Text = str1;
                HDLSysPF.addcontrols(4, index, txtbox1, dgvListA);

                cbbox2.Items.Clear();
                cbbox1.Items.AddRange(CsConst.UvSwitchStatus);
                HDLSysPF.addcontrols(5, index, cbbox2, dgvListA);
                dgvListA[6, index].Value = "N/A";
                dgvListA[7, index].Value = "N/A";
                #endregion
            }
            else if (cbControlType.Text == CsConst.myPublicControlType[14].ControlTypeName)//通用控制
            {
                 #region
                if (str1.Contains("/"))
                {
                    string strtmp = str1;
                    str1 = HDLPF.IsNumStringMode(strtmp.Split('/')[0].ToString(), 0, 255) + "/" +
                           HDLPF.IsNumStringMode(strtmp.Split('/')[1].ToString(), 0, 255);
                    uc1.Text = str1;
                }
                else
                {
                    uc1.Text = "0/0";
                }
                HDLSysPF.addcontrols(4, index, uc1,dgvListA);

                if (str2.Contains("/"))
                {
                    string strtmp = str2;
                    str2 = HDLPF.IsNumStringMode(strtmp.Split('/')[0].ToString(), 0, 255) + "/" +
                           HDLPF.IsNumStringMode(strtmp.Split('/')[1].ToString(), 0, 255);
                    uc2.Text = str1;
                }
                else
                {
                    uc2.Text = "0/0";
                }
                HDLSysPF.addcontrols(5, index, uc2,dgvListA);
                dgvListA[6, index].Value = "N/A";
                #endregion
            }
            else if (cbControlType.Text == CsConst.myPublicControlType[15].ControlTypeName) //连接页
            {
                #region
                txtbox1.Text = str1;
                HDLSysPF.addcontrols(4, index, txtbox1, dgvListA);

                cbbox2.Items.Clear();
                cbbox2.Items.Add(CsConst.WholeTextsList[1775].sDisplayName);
                cbbox2.Items.Add(CsConst.WholeTextsList[2497].sDisplayName);
                cbbox2.Items.Add(CsConst.WholeTextsList[2498].sDisplayName);
                HDLSysPF.addcontrols(5, index, cbbox2, dgvListA);
                dgvListA[6, index].Value = "N/A";
                dgvListA[7, index].Value = "N/A";
                #endregion
            }
            else if (cbControlType.Text == CsConst.myPublicControlType[18].ControlTypeName)//红外控制
            {
                #region
                txtbox1.Text = str1;
                HDLSysPF.addcontrols(4, index, txtbox1, dgvListA);
                txtbox2.Text = str2;
                HDLSysPF.addcontrols(5, index, txtbox2, dgvListA);
                IrCtrl.Text = HDLPF.IsNumStringMode(str3, 0, 255);
                HDLSysPF.addcontrols(6, index, IrCtrl, dgvListA);
                dgvListA[7, index].Value = "N/A";
                #endregion
            }
            else if (cbControlType.Text == CsConst.myPublicControlType[19].ControlTypeName)//逻辑灯调节
            {
                #region
                txtbox1.Text = str1;
                HDLSysPF.addcontrols(4, index, txtbox1, dgvListA);

                txtbox2.Text = str2;
                HDLSysPF.addcontrols(5, index, txtbox2, dgvListA);

                txtbox3.Text = str3;
                HDLSysPF.addcontrols(6, index, txtbox3, dgvListA);

                txtbox4.Text = str4;
                HDLSysPF.addcontrols(7, index, txtbox4, dgvListA);
                #endregion
            }
            else if (cbControlType.Text == CsConst.myPublicControlType[20].ControlTypeName)//逻辑场景
            {
                #region
                txtbox1.Text = str1;
                HDLSysPF.addcontrols(4, index, txtbox1, dgvListA);

                txtbox2.Text = str2;
                HDLSysPF.addcontrols(5, index, txtbox2, dgvListA);
                dgvListA[6, index].Value = "N/A";
                dgvListA[7, index].Value = "N/A";
                #endregion
            }
            else if (cbControlType.Text == CsConst.myPublicControlType[21].ControlTypeName)//休眠模式
            {
                #region
                cbbox1.Items.Clear();
                cbbox1.Items.Add(CsConst.WholeTextsList[757].sDisplayName);
                cbbox1.Items.Add(CsConst.WholeTextsList[1051].sDisplayName);
                HDLSysPF.addcontrols(4, index, cbbox1,dgvListA);
                #endregion
            }
            dgvListA[3, index].Value = cbControlType.Text;
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
            if (txtbox4.Visible) txtbox4_TextChanged(null, null);
            if (txtSeries.Visible) txtSeries_TextChanged(null, null);
            if (cbbox1.Visible) cbbox1_SelectedIndexChanged(null, null);
            if (cbbox2.Visible) cbbox2_SelectedIndexChanged(null, null);
            if (cbbox3.Visible) cbbox3_SelectedIndexChanged(null, null);
            if (cbPanleControl.Visible) cbPanleControl_SelectedIndexChanged(null, null);
            if (cbAudioControl.Visible) cbAudioControl_SelectedIndexChanged(null, null);
            #endregion
            ModifyMultilinesIfNeeds(dgvListA[3, index].Value.ToString(), 3);
        }

        private void txtFrm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar) || e.KeyChar == 8)
                e.Handled = false;
            else
                e.Handled = true;
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
            txtbox4.Visible = TF;
            txtSeries.Visible = TF;
            cbPanleControl.Visible = TF;
            cbAudioControl.Visible = TF;
            uc1.Visible = TF;
            uc2.Visible = TF;
            IrCtrl.Visible = TF;
        }

        private void dgvListA_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvListA.Rows.Count == 0) return;
            if (dgvListA.CurrentCell.RowIndex == -1 || dgvListA.CurrentCell.ColumnIndex == -1) return;
            try
            {               
                setAllControlVisible(false);
                if (e.ColumnIndex >= 1)
                {
                    txtSub.Text = dgvListA[1, e.RowIndex].Value.ToString();
                    HDLSysPF.addcontrols(1, e.RowIndex, txtSub, dgvListA);

                    txtDev.Text = dgvListA[2, e.RowIndex].Value.ToString();
                    HDLSysPF.addcontrols(2, e.RowIndex, txtDev, dgvListA);

                    cbControlType.Text = dgvListA[3, e.RowIndex].Value.ToString();
                    HDLSysPF.addcontrols(3, e.RowIndex, cbControlType, dgvListA);

                    txtSub_TextChanged(txtSub, null);
                    txtDev_TextChanged(txtDev, null);
                    cbControlType_SelectedIndexChanged(cbControlType, null);
                }
                else
                {
                    CsConst.iIsDragTemplateOrSimpleListOrCmd = 2;
                    if (CsConst.MyPublicCtrls != null && CsConst.MyPublicCtrls.Count > 0)
                    {
                        dgvListA.DoDragDrop("Paste", DragDropEffects.Copy);
                    }
                }
            }
            catch
            { }
        }

        private Byte[] GetReadyCMDDataFromDataGrid(int i)
        {
            byte[] arayTmp = new byte[9];
            try
            {
                arayTmp[0] = Convert.ToByte(buttonId + 1 + pageId * cbKey.Items.Count);
                if (HDLSysPF.IsNumeric(dgvListA[0, i].Value.ToString()))
                {
                    arayTmp[1] = Convert.ToByte(dgvListA[0, i].Value.ToString());
                }

                arayTmp[3] = Convert.ToByte(dgvListA[1, i].Value.ToString());
                arayTmp[4] = Convert.ToByte(dgvListA[2, i].Value.ToString());
                arayTmp[2] = ButtonControlType.ConvertorKeyControlTypeToPublicModeGroup(dgvListA[3, i].Value.ToString());
                Byte panelKeyType = arayTmp[2];

                if (!MSPUSenserDeviceTypeList.SensorsDeviceTypeList.Contains(mywdDevicerType)       // 传感器作为真假判断
                    && !Eightin1DeviceTypeList.HDL8in1DeviceType.Contains(mywdDevicerType)          // 8in1传感器
                    && !HotelMixModuleDeviceType.HDLRCUDeviceTypeLists.Contains(mywdDevicerType)   // RCU传感器
                    && !MS04DeviceTypeList.MS04NormalDeviceTypeList.Contains(mywdDevicerType) // 旧版MS04
                    && !MiniSenserDeviceTypeList.SensorsDeviceTypeList.Contains(mywdDevicerType) // 迷你传感器
                    && !Twelvein1DeviceTypeList.HDL12in1DeviceType.Contains(mywdDevicerType)
                    && !MS04DeviceTypeList.MS04IModuleWithCombinationONAndOFF.Contains(mywdDevicerType)) // 12in1
                {
                    if (rbShortOff.Visible && rbShortOff.Checked)
                        arayTmp[1] = Convert.ToByte(arayTmp[1] + 49);
                    else if (rbLongPressON.Checked && rbLongPressON.Visible)
                    {
                        arayTmp[1] = Convert.ToByte(arayTmp[1] + 49);
                    }
                    else if (rbLongPressON.Visible && rbLongPressON.Checked)
                    {
                        arayTmp[1] = Convert.ToByte(arayTmp[1] + 49);
                    }
                }
                else
                {
                    arayTmp[2] = DryControlType.ConvertorKeyControlTypeToPublicModeGroup(dgvListA[3, i].Value.ToString());
                    if ((HotelMixModuleDeviceType.HDLRCUDeviceTypeLists.Contains(mywdDevicerType) && pageId == 1) //酒店主机
                        || MS04DeviceTypeList.MS04DryContactWithE000PublicCMD.Contains(mywdDevicerType))  //新版MS04
                        arayTmp[2] = ButtonControlType.ConvertorKeyControlTypeToPublicModeGroup(dgvListA[3, i].Value.ToString());
                }

                string str1 = dgvListA[4, i].Value.ToString();
                string str2 = dgvListA[5, i].Value.ToString();
                string str3 = dgvListA[6, i].Value.ToString();
                if (str2 == CsConst.mstrInvalid) str2 = "0";
                if (str3 == CsConst.mstrInvalid) str3 = "0";
                if (dgvListA[7, i].Value == null) dgvListA[7, i].Value = "";
                string str4 = dgvListA[7, i].Value.ToString();
                if (str1.Contains("(")) str1 = str1.Split('(')[0].ToString();
                if (str2.Contains("(")) str2 = str2.Split('(')[0].ToString();
                if (str3.Contains("(")) str3 = str3.Split('(')[0].ToString();
                if (str4.Contains("(")) str4 = str4.Split('(')[0].ToString();
                if (panelKeyType == 85 || panelKeyType == 86)//场景 序列
                {
                    #region
                    arayTmp[5] = Convert.ToByte(str1);
                    arayTmp[6] = Convert.ToByte(str2);
                    #endregion
                }
                else if (panelKeyType == 88)//通用开关
                {
                    #region
                    arayTmp[5] = Convert.ToByte(str1);
                    if (str2 == CsConst.Status[0])
                        arayTmp[6] = 0;
                    else if (str2 == CsConst.Status[1])
                        arayTmp[6] = 255;
                    #endregion
                }
                else if (panelKeyType == 89)//单路调节
                {
                    #region
                    arayTmp[5] = Convert.ToByte(str1);
                    arayTmp[6] = Convert.ToByte(str2);
                    int intTmp = Convert.ToInt32(HDLPF.GetTimeFromString(str3, ':'));
                    arayTmp[7] = Convert.ToByte(intTmp / 256);
                    arayTmp[8] = Convert.ToByte(intTmp % 256);
                    #endregion
                }
                else if (panelKeyType == 100)//广播场景
                {
                    #region
                    arayTmp[2] = 85;
                    arayTmp[5] = 255;
                    arayTmp[6] = Convert.ToByte(str2);
                    #endregion
                }
                else if (panelKeyType == 101)//广播回路
                {
                    #region
                    arayTmp[2] = 89;
                    arayTmp[5] = 255;
                    arayTmp[6] = Convert.ToByte(str2);
                    int intTmp = Convert.ToInt32(HDLPF.GetTimeFromString(str3, ':'));
                    arayTmp[7] = Convert.ToByte(intTmp / 256);
                    arayTmp[8] = Convert.ToByte(intTmp % 256);
                    #endregion
                }
                else if (panelKeyType == 92)//窗帘开关
                {
                    #region
                    arayTmp[5] = Convert.ToByte(str1);
                    if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00036", "")) arayTmp[6] = 0;
                    else if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00037", "")) arayTmp[6] = 1;
                    else if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00038", "")) arayTmp[6] = 2;
                    else
                    {
                        str2 = str2.Replace("%", "");
                        arayTmp[6] = Convert.ToByte(str2);
                        arayTmp[5] = Convert.ToByte(Convert.ToByte(str1) + 16);
                    }
                    #endregion
                }
                else if (panelKeyType == 94)//GPRS
                {
                    #region
                    if (str1 == CsConst.mstrINIDefault.IniReadValue("public", "99862", "")) arayTmp[5] = 1;
                    else if (str1 == CsConst.mstrINIDefault.IniReadValue("public", "99863", "")) arayTmp[5] = 2;
                    else arayTmp[5] = 0;
                    arayTmp[6] = Convert.ToByte(str2);
                    #endregion
                }
                else if (panelKeyType == 95)//面板控制
                {
                    #region
                    arayTmp[5] = Convert.ToByte(HDLSysPF.getIDFromPanelControlTypeString(str1));
                    if (arayTmp[5] == 1 || arayTmp[5] == 11 ||
                        arayTmp[5] == 2 || arayTmp[5] == 12 ||
                        arayTmp[5] == 24 || arayTmp[5] == 3 ||
                        arayTmp[5] == 20)
                    {
                        if (str2 == CsConst.Status[0])
                            arayTmp[6] = 0;
                        else if (str2 == CsConst.Status[1])
                            arayTmp[6] = 1;
                    }
                    else if (arayTmp[5] == 13 || arayTmp[5] == 14)
                    {
                        arayTmp[6] = Convert.ToByte(str2);
                    }
                    else if (arayTmp[5] == 16 || arayTmp[5] == 15 ||
                             arayTmp[5] == 17 || arayTmp[5] == 18)
                    {
                        if (arayTmp[5] == 16)
                        {
                            if (str2.Contains(CsConst.mstrINIDefault.IniReadValue("public", "00048", "")))
                            {
                                str2 = str2.Replace(CsConst.mstrINIDefault.IniReadValue("public", "00048", ""), "");
                                arayTmp[6] = Convert.ToByte(str2);
                            }
                            else if (str2 == CsConst.WholeTextsList[1775].sDisplayName)
                                arayTmp[6] = 0;
                            else if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00049", ""))
                                arayTmp[6] = 255;
                        }
                        else if (arayTmp[5] == 15)
                        {
                            if (str2.Contains(CsConst.mstrINIDefault.IniReadValue("public", "99867", "")))
                            {
                                str2 = str2.Replace(CsConst.mstrINIDefault.IniReadValue("public", "99867", ""), "");
                                arayTmp[6] = Convert.ToByte(str2);
                            }
                            else if (str2 == CsConst.WholeTextsList[1775].sDisplayName)
                                arayTmp[6] = 0;
                            else if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00049", ""))
                                arayTmp[6] = 255;
                        }
                        else if (arayTmp[5] == 17)
                        {
                            if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00049", "")) arayTmp[6] = 255;
                            else if (str2.Contains(CsConst.mstrINIDefault.IniReadValue("public", "99867", ""))
                                && !str2.Contains("["))
                            {
                                str2 = str2.Replace(CsConst.mstrINIDefault.IniReadValue("public", "99867", ""), "");
                                arayTmp[6] = Convert.ToByte(str2);
                            }
                            else if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "99868", "")) arayTmp[6] = 101;
                            else if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "99869", "")) arayTmp[6] = 102;
                            else if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "99870", "")) arayTmp[6] = 103;
                            else if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "99871", "")) arayTmp[6] = 104;
                            else if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00049", "")) arayTmp[6] = 255;
                        }
                        else if (arayTmp[5] == 18)
                        {
                            if (str2.Contains(CsConst.mstrINIDefault.IniReadValue("public", "99867", "")))
                            {
                                str2 = str2.Replace(CsConst.mstrINIDefault.IniReadValue("public", "99867", ""), "");
                                arayTmp[6] = Convert.ToByte(str2);
                            }
                            else if (str2 == CsConst.WholeTextsList[1775].sDisplayName) arayTmp[6] = 0;
                        }
                        if (str3 == CsConst.mstrINIDefault.IniReadValue("public", "00042", "")) arayTmp[7] = 1;
                        else if (str3 == CsConst.WholeTextsList[1775].sDisplayName) arayTmp[7] = 0;
                    }
                    else if (arayTmp[5] == 6)
                    {
                        if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00050", "")) arayTmp[6] = 0;
                        else if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00051", "")) arayTmp[6] = 1;
                        else if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00052", "")) arayTmp[6] = 2;
                        else if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00053", "")) arayTmp[6] = 3;
                        else if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00054", "")) arayTmp[6] = 4;
                    }
                    else if (arayTmp[5] == 5)
                    {
                        if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00060", "")) arayTmp[6] = 0;
                        else if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00061", "")) arayTmp[6] = 1;
                        else if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00062", "")) arayTmp[6] = 2;
                        else if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00063", "")) arayTmp[6] = 3;
                    }
                    else if (arayTmp[5] == 9 || arayTmp[5] == 10 ||
                             arayTmp[5] == 22 || arayTmp[5] == 23)
                    {
                        arayTmp[6] = Convert.ToByte(str2);
                        if (arayTmp[5] == 22 || arayTmp[5] == 23)
                        {
                            if (str3 == CsConst.mstrINIDefault.IniReadValue("public", "00049", ""))
                                arayTmp[7] = 255;
                            else arayTmp[7] = Convert.ToByte(str3);
                        }
                    }
                    else if (arayTmp[5] == 4 || arayTmp[5] == 7 ||
                             arayTmp[5] == 8 || arayTmp[5] == 19)
                    {
                        if (str2.Contains("C")) str2 = str2.Replace("C", "");
                        if (str2.Contains("F")) str2 = str2.Replace("F", "");
                        arayTmp[6] = Convert.ToByte(str2);
                    }
                    else if (arayTmp[5] == 21)
                    {
                        if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00070", "")) arayTmp[6] = 1;
                        else if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00071", "")) arayTmp[6] = 2;
                        else if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00072", "")) arayTmp[6] = 3;
                        else if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00073", "")) arayTmp[6] = 4;
                        else if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00074", "")) arayTmp[6] = 5;

                        if (str3 == CsConst.mstrINIDefault.IniReadValue("public", "00049", ""))
                            arayTmp[7] = 255;
                        else arayTmp[7] = Convert.ToByte(str3);
                    }
                    else if (arayTmp[5] == 25 || arayTmp[5] == 26 ||
                             arayTmp[5] == 27 || arayTmp[5] == 28)
                    {
                        str2 = str2.Replace("C", "");
                        str2 = str2.Replace("F", "");
                        arayTmp[6] = Convert.ToByte(str2);
                        if (str3 == CsConst.mstrINIDefault.IniReadValue("public", "00049", ""))
                            arayTmp[7] = 255;
                        else arayTmp[7] = Convert.ToByte(str3);
                    }
                    else if (arayTmp[5] == 29)
                    {
                        str2 = str2.Replace(CsConst.mstrINIDefault.IniReadValue("public", "00048", ""), "");
                        arayTmp[6] = Convert.ToByte(str2);
                    }
                    else if (arayTmp[5] == 30)
                    {
                        arayTmp[6] = Convert.ToByte(str2);
                    }
                    #endregion
                    if ((arayTmp[5] >= 3 && arayTmp[5] <= 10) || (arayTmp[5] >= 20 && arayTmp[5] <= 28)) arayTmp[7] = Convert.ToByte(str3);
                }
                else if (panelKeyType == 102)//消防模块
                {
                    #region
                    arayTmp[5] = Convert.ToByte(str1);
                    if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00080", "")) arayTmp[6] = 1;
                    else if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00081", "")) arayTmp[6] = 2;
                    else if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00082", "")) arayTmp[6] = 3;
                    else if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00083", "")) arayTmp[6] = 4;
                    else if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00084", "")) arayTmp[6] = 5;
                    else if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00085", "")) arayTmp[6] = 6;
                    else if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00086", "")) arayTmp[6] = 7;
                    else if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00087", "")) arayTmp[6] = 8;
                    else if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00088", "")) arayTmp[6] = 9;
                    else if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00089", "")) arayTmp[6] = 10;
                    #endregion
                }
                else if (panelKeyType == 103)//音乐控制
                {
                    #region
                    if (str1 == cbAudioControl.Items[0].ToString())
                    {
                        arayTmp[5] = 1;
                        if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00101", "")) arayTmp[6] = 1;
                        else if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00102", "")) arayTmp[6] = 2;
                        else if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00103", "")) arayTmp[6] = 3;
                        else if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00104", "")) arayTmp[6] = 4;
                    }
                    else if (str1 == cbAudioControl.Items[1].ToString())
                    {
                        arayTmp[5] = 2;
                        if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00111", "")) arayTmp[6] = 1;
                        else if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00112", "")) arayTmp[6] = 2;
                        else if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00113", "")) arayTmp[6] = 3;
                        else if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00114", "")) arayTmp[6] = 4;
                    }
                    else if (str1 == cbAudioControl.Items[2].ToString())
                    {
                        arayTmp[5] = 3;
                        if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00121", "")) arayTmp[6] = 1;
                        else if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00122", "")) arayTmp[6] = 2;
                        else if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00123", "")) arayTmp[6] = 3;
                        else if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00124", "")) arayTmp[6] = 4;
                        else if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00125", "")) arayTmp[6] = 5;
                        else if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00126", "")) arayTmp[6] = 6;
                        if (arayTmp[6] == 3 || arayTmp[6] == 6)
                            arayTmp[7] = Convert.ToByte(str3);
                    }
                    else if (str1 == cbAudioControl.Items[3].ToString())
                    {
                        arayTmp[5] = 4;
                        if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00131", "")) arayTmp[6] = 1;
                        else if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00132", "")) arayTmp[6] = 2;
                        else if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00133", "")) arayTmp[6] = 3;
                        else if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00134", "")) arayTmp[6] = 4;
                    }
                    else if (str1 == cbAudioControl.Items[4].ToString())
                    {
                        arayTmp[5] = 5;
                        if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00141", "")) arayTmp[6] = 1;
                        else if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00142", "")) arayTmp[6] = 2;
                        else if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00143", "")) arayTmp[6] = 3;
                        if (arayTmp[6] == 1)
                        {
                            if (str3 == CsConst.mstrINIDefault.IniReadValue("public", "00044", ""))
                                arayTmp[7] = 1;
                            else if (str3 == CsConst.mstrINIDefault.IniReadValue("public", "00045", ""))
                                arayTmp[7] = 2;
                            else if (str3.Contains(CsConst.mstrINIDefault.IniReadValue("public", "99872", "")))
                            {
                                arayTmp[7] = 3;
                                str3 = str3.Split(':')[1].ToString();
                                arayTmp[8] = Convert.ToByte(79 - Convert.ToInt32(str3));
                            }
                        }
                        else if (arayTmp[6] == 2 || arayTmp[6] == 3)
                        {
                            if (str3 == CsConst.mstrINIDefault.IniReadValue("public", "00044", ""))
                                arayTmp[7] = 1;
                            else if (str3 == CsConst.mstrINIDefault.IniReadValue("public", "00045", ""))
                                arayTmp[7] = 2;
                        }
                    }
                    else if (str1 == cbAudioControl.Items[5].ToString())
                    {
                        arayTmp[5] = 6;
                        arayTmp[6] = Convert.ToByte(str2);
                        int intTmp = Convert.ToInt32(str3);
                        arayTmp[7] = Convert.ToByte(intTmp / 256);
                        arayTmp[8] = Convert.ToByte(intTmp % 256);
                    }
                    else if (str1 == cbAudioControl.Items[6].ToString() || str1 == cbAudioControl.Items[7].ToString())
                    {
                        if (str1 == cbAudioControl.Items[6].ToString())
                            arayTmp[5] = 7;
                        if (str1 == cbAudioControl.Items[7].ToString())
                            arayTmp[5] = 8;

                        if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00047", ""))
                            arayTmp[6] = 64;
                        else if (str2.Contains("SD"))
                            arayTmp[6] = Convert.ToByte(Convert.ToInt32(str2.Split(':')[1].ToString()) + 64);
                        else if (str2.Contains("FTP"))
                            arayTmp[6] = Convert.ToByte(Convert.ToInt32(str2.Split(':')[1].ToString()) + 128);
                        int intTmp = Convert.ToInt32(str3);
                        arayTmp[7] = Convert.ToByte(intTmp / 256);
                        arayTmp[8] = Convert.ToByte(intTmp % 256);
                    }
                    #endregion
                }
                else if (panelKeyType == 87)//时间开关
                {
                    #region
                    arayTmp[5] = Convert.ToByte(str1);
                    if (str2 == CsConst.Status[0])
                        arayTmp[6] = 0;
                    else if (str2 == CsConst.Status[1])
                        arayTmp[6] = 255;
                    #endregion
                }
                else if (panelKeyType == 104)//通用控制
                {
                    #region
                    arayTmp[5] = Convert.ToByte(str1.Split('/')[0].ToString());
                    arayTmp[6] = Convert.ToByte(str1.Split('/')[1].ToString());
                    arayTmp[7] = Convert.ToByte(str2.Split('/')[0].ToString());
                    arayTmp[8] = Convert.ToByte(str2.Split('/')[1].ToString());
                    #endregion
                }
                else if (panelKeyType == 105)//连接页
                {
                    #region
                    arayTmp[5] = Convert.ToByte(str1);
                    if (str2 == CsConst.WholeTextsList[2497].sDisplayName)
                        arayTmp[6] = 0;
                    else if (str2 == CsConst.WholeTextsList[2498].sDisplayName)
                        arayTmp[6] = 1;
                    else if (str2 == CsConst.WholeTextsList[1775].sDisplayName)
                        arayTmp[6] = 255;
                    #endregion
                }
                else if (panelKeyType == 108)//红外控制
                {
                    #region
                    arayTmp[5] = Convert.ToByte(str1);
                    arayTmp[6] = Convert.ToByte(str2);
                    arayTmp[7] = Convert.ToByte(str3);
                    #endregion
                }
                else if (panelKeyType == 109)//逻辑灯调节
                {
                    #region
                    arayTmp[5] = Convert.ToByte(str1);
                    arayTmp[6] = Convert.ToByte(str3);
                    arayTmp[7] = Convert.ToByte(str2);
                    arayTmp[8] = Convert.ToByte(str4);
                    #endregion
                }
                else if (panelKeyType == 110)//逻辑场景
                {
                    #region
                    arayTmp[5] = Convert.ToByte(str1);
                    arayTmp[6] = Convert.ToByte(str2);
                    #endregion
                }
                else if (panelKeyType == 112)//休眠模式
                {
                    #region
                    if (str1 == CsConst.WholeTextsList[1051].sDisplayName) arayTmp[5] = 0;
                    else if (str1 == CsConst.WholeTextsList[757].sDisplayName) arayTmp[5] = 1;
                    if (arayTmp[5] == 1)
                    {
                        if (str2 == CsConst.WholeTextsList[2434].sDisplayName) arayTmp[6] = 255;
                        else arayTmp[6] = Convert.ToByte(str2);
                        arayTmp[7] = Convert.ToByte(str3);
                        arayTmp[8] = Convert.ToByte(str4);
                    }
                    #endregion
                }

                TempCMD = new UVCMD.ControlTargets();
                TempCMD.ID = arayTmp[1];
                TempCMD.Type = arayTmp[2];
                TempCMD.SubnetID = arayTmp[3];
                TempCMD.DeviceID = arayTmp[4];
                TempCMD.Param1 = arayTmp[5];
                TempCMD.Param2 = arayTmp[6];
                TempCMD.Param3 = arayTmp[7];
                TempCMD.Param4 = arayTmp[8];
            }
            catch 
            {
                return arayTmp;
            }
            return arayTmp;
        }

        private void btnUpdateA_Click(object sender, EventArgs e)
        {
            if (dgvListA.RowCount == 0) return;
            Cursor.Current = Cursors.WaitCursor;
            btnUpdateA.Enabled = false;
            setAllControlVisible(false);

            try
            {
                ConvertorCommandListFromDatagridviewToPublicList(ref TempCMDGroup);

                if (CsConst.MyCopyDataGridView != null) // 直接复制到主界面
                {
                    HDLSysPF.CopyCMDToPublicBufferWaitPasteOrCopyToGrid(dgvListA);
                    this.DialogResult = System.Windows.Forms.DialogResult.OK;
                }
                else if (DLPPanelDeviceTypeList.HDLDLPPanelDeviceTypeList.Contains(mywdDevicerType) && buttonId == 101) // DLP 地热页面
                {
                    #region
                    if (buttonId == 101) // 地热额外目标
                    {
                        DLP TmpDLP = (DLP)MyActiveObject;
                        TmpDLP.DLPFH.AdvancedCommands = TempCMDGroup.ToArray();
                        TmpDLP.DLPFH.ModifyFloorheatingAdvancedCommandsGroup(SubNetID, DeviceID, mywdDevicerType);
                    }
                    #endregion
                }
                else if (NormalPanelDeviceTypeList.WirelessSimpleFloorHeatingDeviceTypeList.Contains(mywdDevicerType) && buttonId == 101) //ismart 地热页面
                {
                    #region
                    if (buttonId == 101) // 地热额外目标
                    {
                        Panel TmpDLP = (Panel)MyActiveObject;
                        TmpDLP.DLPFH.AdvancedCommands = TempCMDGroup.ToArray();
                        TmpDLP.DLPFH.ModifyFloorheatingAdvancedCommandsGroup(SubNetID, DeviceID, mywdDevicerType);
                    }
                    #endregion
                }
                else if (EnviroNewDeviceTypeList.EnviroNewPanelDeviceTypeList.Contains(mywdDevicerType)) // 彩色面板
                {
                    #region
                    if (buttonId == 101) // 地热额外目标
                    {
                        EnviroPanel TmpDLP = (EnviroPanel)MyActiveObject;
                        TmpDLP.MyHeat[remoteId].AdvancedCommands = TempCMDGroup.ToArray();
                        TmpDLP.MyHeat[remoteId].ModifyFloorheatingAdvancedCommandsGroup(SubNetID, DeviceID, mywdDevicerType);
                    }
                    #endregion
                }
                else if (TmpSensorBlock != null) // 传感器
                {
                    #region
                    if (rbShortOff.Checked == true)
                    {
                        TmpSensorBlock.SetUp = TempCMDGroup;
                        TmpSensorBlock.ModifyLogicTrueCommandsFromDevice(SubNetID, DeviceID, mywdDevicerType);
                    }
                    else if (rbLongPressON.Checked == true)
                    {
                        TmpSensorBlock.NoSetUp = TempCMDGroup;
                        TmpSensorBlock.ModifyLogicFalseCommandsFromDevice(SubNetID, DeviceID, mywdDevicerType);
                    }
                    #endregion
                }
                else if (LogicDeviceTypeList.LogicDeviceType.Contains(mywdDevicerType)) // 逻辑模块
                {
                    #region
                    if (buttonId == -1) return;
                    Byte ControllerID = Convert.ToByte(remoteId);
                    Byte ButtonID = Convert.ToByte(cbKey.Text);

                    TmpLogic = (Logic)MyActiveObject;
                    TmpLogic.MyDesign[ControllerID].ArmCmds[buttonId] = TempCMDGroup;
                    TmpLogic.SaveLogicBlockOneTableCommandsGroup(SubNetID, DeviceID, ControllerID, ButtonID);
                    #endregion
                }
                else if (MS04DeviceTypeList.HDLMS04DeviceTypeList.Contains(mywdDevicerType))        //旧版干结点  
                {
                    #region
                    if (buttonId == -1) return;
                    if (rbShortOff.Checked == true && rbShortOff.Visible == true)
                    {
                        ((MS04Key)TmpButton).KeyOffTargets = TempCMDGroup;
                        if (MS04DeviceTypeList.MS04IOModuleDeviceTypeList.Contains(mywdDevicerType)) // 新版干节点
                        {
                            ((MS04)MyActiveObject).ModifyIOModuleCommandsGroupFrmDeviceToBuf(SubNetID, DeviceID, mywdDevicerType, (Byte)buttonId, 0);
                        }
                        else if (MS04DeviceTypeList.MS04DryContactWithE000PublicCMD.Contains(mywdDevicerType)) // 新版干节点
                        {
                            ((MS04)MyActiveObject).UploadButtonRemarkAndCMDToDevice(SubNetID, DeviceID, mywdDevicerType, (Byte)buttonId, 0);
                        }
                        else
                        {
                            ((MS04)MyActiveObject).ModifyDryCommandsGroupFrmDeviceToBuf(SubNetID, DeviceID, mywdDevicerType, (Byte)buttonId, 0);
                        }
                    }
                    else
                    {
                        ((MS04Key)TmpButton).KeyTargets = TempCMDGroup;
                        if (MS04DeviceTypeList.MS04IOModuleDeviceTypeList.Contains(mywdDevicerType)) // 新版干节点
                        {
                            ((MS04)MyActiveObject).ModifyIOModuleCommandsGroupFrmDeviceToBuf(SubNetID, DeviceID, mywdDevicerType, (Byte)buttonId, 1);
                        }
                        else if (MS04DeviceTypeList.MS04DryContactWithE000PublicCMD.Contains(mywdDevicerType)) // 新版干节点
                        {
                            ((MS04)MyActiveObject).UploadButtonRemarkAndCMDToDevice(SubNetID, DeviceID, mywdDevicerType, (Byte)buttonId, 1);
                        }
                        else
                        {
                            ((MS04)MyActiveObject).ModifyDryCommandsGroupFrmDeviceToBuf(SubNetID, DeviceID, mywdDevicerType, (Byte)buttonId, 1);
                        }
                    }
                    #endregion
                }
                else if (Tmp12in1 != null) // 十二合一
                {
                    if (buttonId == -1) return;
                    #region
                    if (remoteId == 1) // 红外接收
                    {
                        Tmp12in1.IrReceiver[buttonId + pageId * 8].TargetInfo = TempCMDGroup;
                        Tmp12in1.ModifyIrReceiverCommandGroup(SubNetID, DeviceID, Tmp12in1.IrReceiver[buttonId + pageId * 8]);
                    }
                    else
                    {
                        #region
                        if (rbShortOff.Checked == true)
                        {
                            Tmp12in1.logic[buttonId].SetUp = TempCMDGroup;
                            Tmp12in1.ModifyLogicTrueCommandsFromDevice(SubNetID, DeviceID, mywdDevicerType, (Byte)(buttonId + 1), TempCMDGroup);
                        }
                        else if (rbLongPressON.Checked == true)
                        {
                            Tmp12in1.logic[buttonId].NoSetUp = TempCMDGroup;
                            Tmp12in1.ModifyLogicFalseCommandsFromDevice(SubNetID, DeviceID, mywdDevicerType, (Byte)(buttonId + 1), TempCMDGroup);
                        }
                        #endregion
                    }
                    #endregion
                }
                else if (TmpMHRcu != null) // 混合酒店主机
                {
                    if (buttonId == -1) return;
                    #region
                    if (pageId == 0) // 干节点
                    {
                        if (rbLongPressON.Checked == true && rbLongPressON.Visible == true)
                        {
                            ((MS04Key)TmpButton).KeyOffTargets = TempCMDGroup;
                            ((HDL_Buspro_Setup_Tool.MHRCU)TmpMHRcu).ModifyDryCommandsGroupFrmDeviceToBuf(SubNetID, DeviceID, mywdDevicerType, (Byte)buttonId, 0);
                        }
                        else
                        {
                            TmpButton.KeyTargets = TempCMDGroup;
                            ((HDL_Buspro_Setup_Tool.MHRCU)TmpMHRcu).ModifyDryCommandsGroupFrmDeviceToBuf(SubNetID, DeviceID, mywdDevicerType, (Byte)buttonId, 1);
                        }
                    }
                    else if (pageId == 1)
                    {
                        TmpMHRcu.myRS2BUS[buttonId].busTargets = TempCMDGroup;
                        MHRCU.Modify232ToBusCommandsGroupFrmDeviceToBuf(SubNetID, DeviceID, mywdDevicerType, (Byte)buttonId, TempCMDGroup);
                    }
                    else if (pageId == 2)
                    {
                        TmpMHRcu.my4852BUS[buttonId].busTargets = TempCMDGroup;
                        MHRCU.Modify232ToBusCommandsGroupFrmDeviceToBuf(SubNetID, DeviceID, mywdDevicerType, (Byte)buttonId, TempCMDGroup);
                    }
                    else if (pageId == 3)
                    {
                        if (rbLongPressON.Checked == true && rbLongPressON.Visible == true)
                        {
                            TmpMHRcu.myLogic[buttonId].NoSetup = TempCMDGroup;
                            HdlUdpPublicFunctions.ModifyLogicFalseCommandsFromDevice(SubNetID, DeviceID, mywdDevicerType, (Byte)buttonId, TmpMHRcu.myLogic[buttonId].NoSetup);
                        }
                        else
                        {
                            TmpMHRcu.myLogic[buttonId].Setup = TempCMDGroup;
                            HdlUdpPublicFunctions.ModifyLogicTrueCommandsFromDevice(SubNetID, DeviceID, mywdDevicerType, (Byte)buttonId, TmpMHRcu.myLogic[buttonId].Setup);
                        }
                    }
                    #endregion

                }
                else //直接保存
                {
                    #region
                    for (int i = 0; i < dgvListA.RowCount; i++)
                    {
                        Byte[] arayTmp = GetReadyCMDDataFromDataGrid(i);
                        Byte[] FinallyCMDGroup = null;
                        if (IPmoduleDeviceTypeList.RFIpModuleV2.Contains(mywdDevicerType)) //无线网关
                        {
                            FinallyCMDGroup = new Byte[10];
                            arayTmp.CopyTo(FinallyCMDGroup, 0);
                            FinallyCMDGroup[9] = (Byte)remoteId;
                        }
                        else if (NormalPanelDeviceTypeList.HDLNormalPanelDeviceTypeList.Contains(mywdDevicerType) // 面板
                              || MS04DeviceTypeList.MS04WirelessDeviceTypeList.Contains(mywdDevicerType)        //新版干结点  
                              || MHICDeviceTypeList.HDLCardReaderDeviceType.Contains(mywdDevicerType)) //新版插卡去电模块
                        {
                            FinallyCMDGroup = arayTmp;
                        }
                        else if (EnviroNewDeviceTypeList.EnviroNewPanelDeviceTypeList.Contains(mywdDevicerType)) //彩屏面板
                        {
                            FinallyCMDGroup = new Byte[arayTmp.Length + 3];
                            arayTmp.CopyTo(FinallyCMDGroup, 0);
                            FinallyCMDGroup[arayTmp.Length + 0] = 4;
                            FinallyCMDGroup[arayTmp.Length + 1] = 0;
                            FinallyCMDGroup[arayTmp.Length + 2] = 36;
                        }

                        if (CsConst.mySends.AddBufToSndList(FinallyCMDGroup, 0xE002, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(mywdDevicerType)) == false)
                        {
                            break;
                        }
                        else
                        {
                            TmpButton.KeyTargets[i] = TempCMD;
                            System.Threading.Thread.Sleep(10);
                        }
                    }
                    #endregion
                }
            }
            catch 
            {
                btnUpdateA.Enabled = true;
                Cursor.Current = Cursors.Default;
            }
            btnUpdateA.Enabled = true;
            Cursor.Current = Cursors.Default;
        }

        void ConvertorCommandListFromDatagridviewToPublicList(ref List<UVCMD.ControlTargets> TmpGroup)
        {
            TmpGroup = new List<UVCMD.ControlTargets>();
            if (dgvListA.Rows == null || dgvListA.Rows.Count == 0) return;
            for (int i = 0; i < dgvListA.RowCount; i++)
            {
                Byte[] arayTmp = GetReadyCMDDataFromDataGrid(i);
                TmpGroup.Add(TempCMD);
            }
        }

        private void copy_Click(object sender, EventArgs e)
        {
            if (dgvListA.Rows == null) return;
            if (dgvListA.SelectedRows == null) return;
            try
            {
                CsConst.MyPublicCtrls = new List<UVCMD.ControlTargets>();
                for (int iRowId = 0; iRowId < dgvListA.Rows.Count; iRowId++)
                {
                    if (dgvListA.Rows[iRowId].Selected)
                    {
                        Byte[] arayTmp = GetReadyCMDDataFromDataGrid(iRowId);
                        UVCMD.ControlTargets oCtrls = new UVCMD.ControlTargets();
                        oCtrls.SubnetID = arayTmp[3];
                        oCtrls.DeviceID = arayTmp[4];
                        oCtrls.Type = arayTmp[2];
                        oCtrls.Param1 = arayTmp[5];
                        oCtrls.Param2 = arayTmp[6];
                        oCtrls.Param3 = arayTmp[7];
                        oCtrls.Param4 = arayTmp[8];
                        CsConst.MyPublicCtrls.Add(oCtrls);
                    }
                }
            }
            catch
            { 
            }
        }

        private void paste_Click(object sender, EventArgs e)
        {
            if (CsConst.MyPublicCtrls == null || CsConst.MyPublicCtrls.Count == 0) return;
            int RowIndex = -1;
            if (dgvListA.SelectedRows != null && dgvListA.SelectedRows.Count !=0) RowIndex = dgvListA.SelectedRows[0].Index;
            setAllControlVisible(false);
            try
            {
                foreach (UVCMD.ControlTargets oCmd in CsConst.MyPublicCtrls)
                {
                    string strType = "";
                    strType = ButtonControlType.ConvertorKeyModeToPublicModeGroup(oCmd.Type);
                    if (MSPUSenserDeviceTypeList.SensorsDeviceTypeList.Contains(mywdDevicerType) || // 超声波
                        LogicDeviceTypeList.LogicDeviceType.Contains(mywdDevicerType) ||             // 逻辑模块
                        Eightin1DeviceTypeList.HDL8in1DeviceType.Contains(mywdDevicerType) ||// 8in1传感器
                        (TmpMHRcu != null && (pageId ==0 || pageId ==3)) || //酒店混合主机模块
                        (MS04DeviceTypeList.MS04NormalDeviceTypeList.Contains(mywdDevicerType)) || // 旧版MS04 
                        MiniSenserDeviceTypeList.SensorsDeviceTypeList.Contains(mywdDevicerType)) // 迷你传感器) // 特殊版本MS04
                    {
                        strType = DryControlType.ConvertorKeyModeToPublicModeGroup(oCmd.Type);
                    }

                    string strParam1 = "0", strParam2 = "0", strParam3 = "0", strParam4 = "0";
                    strParam1 = oCmd.Param1.ToString();
                    strParam2 = oCmd.Param2.ToString();
                    strParam3 = oCmd.Param3.ToString();
                    strParam4 = oCmd.Param4.ToString();
                    SetParams(ref strType, ref strParam1, ref strParam2, ref strParam3, ref strParam4);

                    if (dgvListA.Rows == null || dgvListA.Rows.Count ==0)
                    {
                        if (TmpButton != null)
                        {
                            if (TmpButton.KeyTargets == null) TmpButton.KeyTargets = new List<UVCMD.ControlTargets>();
                            TmpButton.KeyTargets.Add(oCmd);
                        }
                        RowIndex = 1;
                         object[] obj = new object[] { RowIndex.ToString(),oCmd.SubnetID,oCmd.DeviceID,strType
                                ,strParam1,strParam2,strParam3,strParam4};
                        dgvListA.Rows.Add(obj);
                    }
                    else if (RowIndex>dgvListA.Rows.Count -1) 
                    {
                        RowIndex = HDLSysPF.GetIndexFromBuffers(dgvListA,0);
                        object[] obj = new object[] { RowIndex.ToString(),oCmd.SubnetID,oCmd.DeviceID,strType
                                ,strParam1,strParam2,strParam3,strParam4};
                        dgvListA.Rows.Add(obj);
                        if (TmpButton != null) TmpButton.KeyTargets.Add(oCmd);
                    }
                    else
                    {
                        dgvListA.Rows[RowIndex].Cells[1].Value = oCmd.SubnetID.ToString();
                        dgvListA.Rows[RowIndex].Cells[2].Value = oCmd.DeviceID.ToString();
                        dgvListA.Rows[RowIndex].Cells[3].Value = strType;
                        dgvListA.Rows[RowIndex].Cells[4].Value = strParam1;
                        dgvListA.Rows[RowIndex].Cells[5].Value = strParam2;
                        dgvListA.Rows[RowIndex].Cells[6].Value = strParam3;
                        dgvListA.Rows[RowIndex].Cells[7].Value = strParam4;
                        if (TmpButton != null)
                        {
                            TmpButton.KeyTargets[RowIndex] = (UVCMD.ControlTargets)oCmd.Clone();
                        }
                        RowIndex++;
                    }
                }
            }
            catch
            {
            }
        }

        private void btnSavetemplate_Click(object sender, EventArgs e)
        {
            try
            {
                setAllControlVisible(false);
                if (TmpButton != null)
                {
                    btnUpdateA_Click(btnUpdateA, null);
                    ControlTemplates oTmp = ControlTemplates.AddNewTemplateToPublicGroup(TmpButton.Remark, TmpButton.KeyTargets);
                }
            }
            catch
            { 
            }
        }

        private void rbShort_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as RadioButton).Checked == false) return;
            try
            {
                dgvListA.Rows.Clear();
                if (MSPUSenserDeviceTypeList.SensorsDeviceTypeList.Contains(mywdDevicerType) // 传感器
                 || Eightin1DeviceTypeList.HDL8in1DeviceType.Contains(mywdDevicerType) // 8in1传感器
                 || MiniSenserDeviceTypeList.SensorsDeviceTypeList.Contains(mywdDevicerType) // 迷你传感器
                 || MSPUSenserDeviceTypeList.SensorsDeviceTypeList.Contains(mywdDevicerType)) // 无线红外超声波
                {
                    if (rbShortOff.Checked == false && rbLongPressON.Checked == false) rbShortOff.Checked = true;
                    if (MSPUSenserDeviceTypeList.SensorsDeviceTypeList.Contains(mywdDevicerType)) // 传感器
                    {
                        #region
                        MSPU TmpMspu = (MSPU)MyActiveObject;
                        TmpSensorBlock = TmpMspu.logic[buttonId];
                        #endregion
                    }
                    else if (Eightin1DeviceTypeList.HDL8in1DeviceType.Contains(mywdDevicerType)) // 8in1传感器
                    {
                        #region
                        MultiSensor TmpMspu = (MultiSensor)MyActiveObject;
                        TmpSensorBlock = TmpMspu.logic[buttonId];
                        #endregion
                    }
                    else if (TmpMiniSensor != null) // 迷你传感器
                    {
                        #region
                        TmpSensorBlock = TmpMiniSensor.logic[buttonId];
                        #endregion
                    }
                    else if (TmpWUSensor != null) // 无线红外传感器
                    {
                        #region
                        TmpSensorBlock = TmpWUSensor.logic[buttonId];
                        #endregion
                    }
                    if (TmpSensorBlock == null) return;
                    // 全部的逻辑功能块统一处理
                    #region
                    if (rbShortOff.Checked == true)
                    {
                        if (TmpSensorBlock.SetUp != null && TmpSensorBlock.SetUp.Count > 0)
                        {
                            foreach (UVCMD.ControlTargets oTmp in TmpSensorBlock.SetUp)
                            {
                                if (oTmp != null) DisplayCMDToDataGrid(oTmp);
                            }
                        }
                    }
                    else if (rbLongPressON.Checked == true)
                    {
                        if (TmpSensorBlock.NoSetUp != null && TmpSensorBlock.NoSetUp.Count > 0)
                        {
                            foreach (UVCMD.ControlTargets oTmp in TmpSensorBlock.NoSetUp)
                            {
                                if (oTmp != null) DisplayCMDToDataGrid(oTmp);
                            }
                        }
                    }
                    #endregion
                }                
                else if (MS04DeviceTypeList.HDLMS04DeviceTypeList.Contains(mywdDevicerType)) // 干结点
                {
                    #region
                    if (TmpButton is MS04Key)
                    {
                        if (rbShortOff.Checked == true)
                        {
                            if (((MS04Key)TmpButton).KeyOffTargets == null || ((MS04Key)TmpButton).KeyOffTargets.Count == 0) return;
                            foreach (UVCMD.ControlTargets oTmp in ((MS04Key)TmpButton).KeyOffTargets)
                            {
                                if (oTmp != null) DisplayCMDToDataGrid(oTmp);
                            }
                        }
                        else if (rbLongPressON.Checked == true)
                        {
                            if (((MS04Key)TmpButton).KeyTargets == null || ((MS04Key)TmpButton).KeyTargets.Count == 0) return;
                            foreach (UVCMD.ControlTargets oTmp in ((MS04Key)TmpButton).KeyTargets)
                            {
                                if (oTmp != null) DisplayCMDToDataGrid(oTmp);
                            }
                        }
                    }
                    #endregion
                }
                else if (Tmp12in1 != null)
                {
                    #region
                    if (rbShortOff.Checked == true)
                    {
                        if (Tmp12in1.logic[buttonId].SetUp != null && Tmp12in1.logic[buttonId].SetUp.Count > 0)
                        {
                            foreach (UVCMD.ControlTargets oTmp in Tmp12in1.logic[buttonId].SetUp)
                            {
                                if (oTmp != null) DisplayCMDToDataGrid(oTmp);
                            }
                        }
                    }
                    else if (rbLongPressON.Checked == true)
                    {
                        if (Tmp12in1.logic[buttonId].NoSetUp != null && Tmp12in1.logic[buttonId].NoSetUp.Count > 0)
                        {
                            foreach (UVCMD.ControlTargets oTmp in Tmp12in1.logic[buttonId].NoSetUp)
                            {
                                if (oTmp != null) DisplayCMDToDataGrid(oTmp);
                            }
                        }
                    }
                    #endregion
                }
                else if (TmpMHRcu != null) // 酒店混合主机
                {
                    #region
                    if (rbShortOff.Checked == true)
                    {
                        if (pageId == 0)
                        {
                            if (TmpButton.KeyTargets == null) return;
                            foreach (UVCMD.ControlTargets oTmp in ((MS04Key)TmpButton).KeyTargets)
                            {
                                if (oTmp != null) DisplayCMDToDataGrid(oTmp);
                            }
                        }
                        else if (pageId == 3)
                        {
                            if (TmpMHRcu.myLogic == null || TmpMHRcu.myLogic.Count == 0) return;
                            if (TmpMHRcu.myLogic[buttonId].Setup == null) return;
                            foreach (UVCMD.ControlTargets oTmp in TmpMHRcu.myLogic[buttonId].Setup)
                            {
                                if (oTmp != null) DisplayCMDToDataGrid(oTmp);
                            }
                        }
                    }
                    else if (rbLongPressON.Checked == true)
                    {
                        if (pageId == 0)
                        {
                            if (((MS04Key)TmpButton).KeyOffTargets == null) return;

                            foreach (UVCMD.ControlTargets oTmp in ((MS04Key)TmpButton).KeyOffTargets)
                            {
                                if (oTmp != null) DisplayCMDToDataGrid(oTmp);
                            }
                        }
                        else if (pageId == 3)
                        {
                            if (TmpMHRcu.myLogic == null || TmpMHRcu.myLogic.Count == 0) return;
                            if (TmpMHRcu.myLogic[buttonId].NoSetup == null) return;
                            foreach (UVCMD.ControlTargets oTmp in TmpMHRcu.myLogic[buttonId].NoSetup)
                            {
                                if (oTmp != null) DisplayCMDToDataGrid(oTmp);
                            }
                        }
                    }
                    #endregion
                }
                else if (TmpButton != null)
                {
                    DisplayCmdDirectlyWithDifferentDeviceType();
                }
            }
            catch { }
        }

        private void dgvListA_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {

        }

        private void dgvListA_DragDrop(object sender, DragEventArgs e)
        {
            if (CsConst.iIsDragTemplateOrSimpleListOrCmd == 2) return;
            if (e.Data.GetDataPresent(typeof(System.String)))
            {
                paste_Click(paste, null);
            }
        }

        private void dgvListA_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(System.String)))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }
    }
}
