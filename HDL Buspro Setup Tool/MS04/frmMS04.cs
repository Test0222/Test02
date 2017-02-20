using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace HDL_Buspro_Setup_Tool
{
    public partial class frmMS04 : Form
    {
        private MS04 myDry = null;
        private string MyDevName = null;
        private int mywdDevicerType = 0;
        private int mintIDIndex = -1;
        private int MyActivePage = 0; //按页面上传下载
        private int selectedDryId = -1;

        private Byte SubNetID;
        private Byte DeviceID;
        private Byte currentAreaId;
        private Byte currentSceneId;

        private Point Position = new Point(0, 0);
        private TimeText txtONDelay = new TimeText(".");
        private TimeText RelayDelay = new TimeText(".");
        TimeText txtScene = new TimeText(":");
        TimeText CurtainDelay = new TimeText(":");
        Curve2DII cuv2D = null;

        private String[] strKey;
        private String strScale = "";
        private String strRange1 = "";
        private String strRange2 = "";
        private int strMin = -1;
        private int strMax = -1;
        private String stStart = "";

        private bool MyBlnReading = false;
        private bool isLoadCure = false;
        private bool isLoadStart = false;
        private bool isChangeSacle = false;

        private byte[] MyBufferProperties = new byte[20];
        private ComboBox cbDimmingCurve = new ComboBox();

        private MixHotelModuleWithZone mixModuleTmp = null;

        public frmMS04()
        {
            InitializeComponent();
        }

        public frmMS04(MS04 TmpMS04, string strName, int intDeviceType, int intDIndex)
        {
            InitializeComponent();

            this.myDry = TmpMS04;
            this.MyDevName = strName;
            this.mywdDevicerType = intDeviceType;
            this.mintIDIndex = intDIndex;
            string strDevName = strName.Split('\\')[0].ToString();

            HDLSysPF.DisplayDeviceNameModeDescription(strName, mywdDevicerType, cboDevice, tbModel, tbDescription);

            SubNetID = Convert.ToByte(strDevName.Split('-')[0]);
            DeviceID = Convert.ToByte(strDevName.Split('-')[1]);
            tsl3.Text = strName;
        }

        void cbDimmingCurve_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (myDry == null) return;
            if (myDry.ChnList == null) return;
            if ((DgChns.CurrentCell.RowIndex == -1) || (DgChns.CurrentCell.ColumnIndex == -1)) return;
            if (cbDimmingCurve.Visible)
            {
                DgChns[8, DgChns.CurrentCell.RowIndex].Value = cbDimmingCurve.Text;
                ModifyMultilinesIfNeeds(DgChns[8, DgChns.CurrentCell.RowIndex].Value.ToString(), 8, DgChns);
            }
        }

        void InitialFormCtrlsTextOrItems()
        {
            cbType.Items.Clear();
            cbType.Items.AddRange(CsConst.LoadType);

            #region
            txtONDelay = new TimeText(".");
            cbDimmingCurve = new ComboBox();
            cbDimmingCurve.Items.Clear();
            cbDimmingCurve.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99801", "") + " 1.0");
            cbDimmingCurve.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99801", "") + " 1.5");
            cbDimmingCurve.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99801", "") + " 2.0");
            cbDimmingCurve.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99801", "") + " 3.0");
            DgChns.Controls.Add(cbDimmingCurve);
            DgChns.Controls.Add(txtONDelay);
            txtONDelay.TextChanged += txtONDelay_TextChanged;
            cbDimmingCurve.DropDownStyle = ComboBoxStyle.DropDownList;
            cbDimmingCurve.SelectedIndexChanged += cbDimmingCurve_SelectedIndexChanged;
            cbDimmingCurve.Visible = false;
            txtONDelay.Visible = false;
            #endregion

            cbInputType.Items.Clear();
            cbInputType.Items.Add(CsConst.mstrINIDefault.IniReadValue("public", "00560", ""));
            cbInputType.Items.Add(CsConst.mstrINIDefault.IniReadValue("public", "00561", ""));
            cbInputType.Items.Add(CsConst.mstrINIDefault.IniReadValue("public", "00562", ""));
            cbInputType.Items.Add(CsConst.mstrINIDefault.IniReadValue("public", "00563", ""));
            cbInputType.Items.Add(CsConst.mstrINIDefault.IniReadValue("public", "00564", ""));

            cbAnalog.Items.Clear();
            cbAnalog.Items.Add(CsConst.mstrINIDefault.IniReadValue("public", "00570", ""));
            cbAnalog.Items.Add(CsConst.mstrINIDefault.IniReadValue("public", "00571", ""));
            cbAnalog.Items.Add(CsConst.mstrINIDefault.IniReadValue("public", "00572", ""));
            cbAnalog.Items.Add(CsConst.mstrINIDefault.IniReadValue("public", "00573", ""));

            cbLED.Items.Clear();
            cbLED.Items.Add(CsConst.mstrINIDefault.IniReadValue("public", "00270", ""));
            cbLED.Items.Add(CsConst.mstrINIDefault.IniReadValue("public", "00271", ""));
            cbLED.Items.Add(CsConst.mstrINIDefault.IniReadValue("public", "00272", ""));

            HDLSysPF.LoadDrynModeWithDifferentDeviceType(clK3, mywdDevicerType);

            clChn9.Items.Clear();
            clChn9.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99894", "") + " 1.0");
            clChn9.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99894", "") + " 1.5");
            clChn9.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99894", "") + " 2.0");
            clChn9.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99894", "") + " 3.0");


            cboDryO1.Items.Clear();
            cboDryO1.Items.Add(CsConst.mstrINIDefault.IniReadValue("public", "99593", ""));
            cboDryO1.Items.Add(CsConst.mstrINIDefault.IniReadValue("public", "99592", ""));
            cboDryO2.Items.Clear();
            cboDryO2.Items.Add(CsConst.mstrINIDefault.IniReadValue("public", "99593", ""));
            cboDryO2.Items.Add(CsConst.mstrINIDefault.IniReadValue("public", "99592", ""));


            int wdMaxValue = DeviceTypeList.GetMaxValueFromPublicModeGroup(mywdDevicerType);
            cbEnable.Items.Clear();cbLock.Items.Clear();
            for (int i = 1; i <= wdMaxValue; i++)
            {
                cbEnable.Items.Add("Dry " + i.ToString());
                cbLock.Items.Add("Dry " + i.ToString());
            }

            dgvKey.Controls.Add(txtScene);
            txtScene.Visible = false;
            txtScene.TextChanged += txtScene_TextChanged;

            dgRelay.Controls.Add(RelayDelay);
            RelayDelay.Visible = false;
            RelayDelay.TextChanged += txtScene_TextChanged;

            dgCurtain.Controls.Add(CurtainDelay);
            CurtainDelay.Visible = false;
            CurtainDelay.TextChanged += txtScene_TextChanged;

        }

        void RelayDelay_TextChanged(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
        }

        void txtScene_TextChanged(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            if (myDry == null) return;
            if (myDry.MSKeys == null) return;            
            
            if (tabMs04.SelectedTab.Name == "tbKeys")
            {
                if (txtScene.Visible)
                {
                    if ((dgvKey.CurrentCell.RowIndex == -1) || (dgvKey.CurrentCell.ColumnIndex == -1)) return;
                    dgvKey[dgvKey.CurrentCell.ColumnIndex, dgvKey.CurrentCell.RowIndex].Value = HDLPF.GetStringFromTime(int.Parse(txtScene.Text.ToString()), ":");
                    ModifyMultilinesIfNeeds(dgvKey[dgvKey.CurrentCell.ColumnIndex, dgvKey.CurrentCell.RowIndex].Value.ToString(), dgvKey.CurrentCell.ColumnIndex, dgvKey);
                }
            }
            else if (tabMs04.SelectedTab.Name == "tabCurtain")
            {
                if (CurtainDelay.Visible)
                {
                    if ((dgCurtain.CurrentCell.RowIndex == -1) || (dgCurtain.CurrentCell.ColumnIndex == -1)) return;
                    dgCurtain[dgCurtain.CurrentCell.ColumnIndex, dgCurtain.CurrentCell.RowIndex].Value = HDLPF.GetStringFromTime(int.Parse(CurtainDelay.Text.ToString()), ":");
                    ModifyMultilinesIfNeeds(dgCurtain[dgCurtain.CurrentCell.ColumnIndex, dgCurtain.CurrentCell.RowIndex].Value.ToString(), dgCurtain.CurrentCell.ColumnIndex, dgCurtain);
                }
            }
            else if (tabMs04.SelectedTab.Name == "tabRelay")
            {
                if (RelayDelay.Visible)
                {
                    if ((dgRelay.CurrentCell.RowIndex == -1) || (dgRelay.CurrentCell.ColumnIndex == -1)) return;
                    dgRelay[dgRelay.CurrentCell.ColumnIndex, dgRelay.CurrentCell.RowIndex].Value = HDLPF.GetStringFromTime(int.Parse(RelayDelay.Text.ToString()), ".");
                    ModifyMultilinesIfNeeds(dgRelay[dgRelay.CurrentCell.ColumnIndex, dgRelay.CurrentCell.RowIndex].Value.ToString(), dgRelay.CurrentCell.ColumnIndex, dgRelay);
                }
            }
        }

        private void frmMS04_Load(object sender, EventArgs e)
        {
            InitialFormCtrlsTextOrItems();
        }

        void SetCtrlsVisbleWithDifferentDeviceType()
        {
            toolStrip1.Visible = (CsConst.MyEditMode == 0);

            Boolean HasNoSecurity = MS04DeviceTypeList.MS04G1WithoutSecurityDeviceTypeList.Contains(mywdDevicerType);
            Boolean IsIOFunction = MS04DeviceTypeList.MS04IOModuleDeviceTypeList.Contains(mywdDevicerType);
            Boolean IsHasTamperd = MS04DeviceTypeList.MS04GetTameredDeviceTypeList.Contains(mywdDevicerType);
            Boolean IsHasCurtain = MS04DeviceTypeList.WirelessMS04WithOneCurtain.Contains(mywdDevicerType);

            Boolean IsHasRelay = MS04DeviceTypeList.WirelessMS04WithRelayChn.Contains(mywdDevicerType);

            Boolean IsHasDimmer = MS04DeviceTypeList.WirelessMS04WithDimmerChn.Contains(mywdDevicerType) ||
                                  MS04DeviceTypeList.MS04HotelMixModule.Contains(mywdDevicerType);

            Boolean IsHasDimDirection = !MS04DeviceTypeList.MS04IOModuleDeviceTypeList.Contains(mywdDevicerType);

            Boolean IsHasAreaSceneInformation = MS04DeviceTypeList.MS04HotelMixModuleHasArea.Contains(mywdDevicerType);

           // Boolean IsHasTemperature = MS04DeviceTypeList.MS04IModuleWithTemperature.Contains(mywdDevicerType);

            if (HasNoSecurity)
            {
                tabMs04.TabPages.Remove(tabSecurity);
            }

            if (IsIOFunction == false)
            {
                tabMs04.TabPages.Remove(tabIOModule);
                tabMs04.TabPages.Remove(tabIoNewFunctions);
            }
            else
            {
                gpBasicSetup.Visible = false;
                dgvKey.Dock = DockStyle.Top;
            }

            if (IsHasCurtain == false)
            {
                tabMs04.TabPages.Remove(tabCurtain);
            }

            if (IsHasDimmer == false) tabMs04.TabPages.Remove(tabChn);
            else if (MS04DeviceTypeList.MS04HotelMixModule.Contains(mywdDevicerType))
            {
                MixOnDelay.Visible = true;
                MixProtectDelay.Visible = true;
            }

            if (IsHasAreaSceneInformation == false)
            {
                tabMs04.TabPages.Remove(tabArea);
                tabMs04.TabPages.Remove(tabScenes);
            }

            if (IsHasRelay == false) tabMs04.TabPages.Remove(tabRelay);
            btnMode.Visible = IsHasDimDirection;
            gbCheckit.Visible = IsHasTamperd;
            if (!MS04DeviceTypeList.MS04HotelMixModule.Contains(mywdDevicerType))
            {
                tabMs04.TabPages.Remove(tabOther);
            }
           // gbTemperature.Visible = IsHasTemperature;
        }

        private void newToolStripButton_Click(object sender, EventArgs e)
        {
            if (myDry == null) return;
            Cursor.Current = Cursors.WaitCursor;
            myDry.ReadDefaultInfo(mywdDevicerType);

            tabMs04.SelectedIndex = 0;
            //ShowKeyToBasicPage();
            Cursor.Current = Cursors.Default;
        }

        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            if (myDry == null) return;
            Cursor.Current = Cursors.WaitCursor;
            myDry.SaveSendIRToDB(mywdDevicerType);
            Cursor.Current = Cursors.Default;
        }

        void SetActivePageIndexInGlobleUnit()
        {
            if (tabMs04.SelectedTab.Name == "tbKeys") MyActivePage = 1;
            else if (tabMs04.SelectedTab.Name == "tabSecurity") MyActivePage = 2;
            else if (tabMs04.SelectedTab.Name == "tabChn") MyActivePage = 3;
            else if (tabMs04.SelectedTab.Name == "tabOther") MyActivePage = 4;
            else if (tabMs04.SelectedTab.Name == "tabCurtain") MyActivePage = 6;
            else if (tabMs04.SelectedTab.Name == "tabRelay") MyActivePage = 6;
            else if (tabMs04.SelectedTab.Name == "tabIOModule") MyActivePage = 5;
            else if (tabMs04.SelectedTab.Name == "tabIoNewFunctions") MyActivePage = 6;
            else if (tabMs04.SelectedTab.Name == "tabArea") MyActivePage = 7; // 印尼模块读取分区
            else if (tabMs04.SelectedTab.Name == "tabScenes") MyActivePage = 8; // 印尼模块读取分区
        }

        private void tabMs04_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetActivePageIndexInGlobleUnit();

            if (CsConst.MyEditMode == 1 && myDry.MyRead2UpFlags[MyActivePage - 1] == false)
            {
                tsbDown_Click(tsbDown, null);
            }
            else
            {
                UpdateDisplayInformationAccordingly(null, null);
            }
        }

        private void showOtherInfo()
        {
            try
            {
                MyBlnReading = true;
                if (myDry == null) return;
                if (myDry.arayCurtain == null) return;
                if (myDry.bytVotoge == 1) radioButton110V.Checked = true;
                else if (myDry.bytVotoge == 0) radioButton220V.Checked = true;

                if (myDry.arayCurtain[0] == 1) chbCurtain1.Checked = true;
                else chbCurtain1.Checked = false;
                if (myDry.arayCurtain[3] == 1) chbCurtain2.Checked = true;
                else chbCurtain2.Checked = false;
                int num = myDry.arayCurtain[1] * 256 + myDry.arayCurtain[2];
                TimeCurtain1.Text = Convert.ToString(num);
                num = myDry.arayCurtain[4] * 256 + myDry.arayCurtain[5];
                TimeCurtain2.Text = Convert.ToString(num);

                numDoorbell.Value = myDry.bytDoorBellRunTime;
                if (cbLED.SelectedIndex < 0) cbLED.SelectedIndex = 0;
            }
            catch
            {
            }
            MyBlnReading = false;
        }

        private void frmMS04_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Dispose();
        }

        void ShowAreasToListview(object sender, byte bytCh2Sce2Series)
        {
            if (myDry == null) return;
            if (myDry.ChnList == null || myDry.ChnList.Count  == 0) return;
            try
            {
                ((TreeView)sender).Nodes.Clear();
                if (myDry is MixHotelModuleWithZone) 
                {
                    mixModuleTmp = (MixHotelModuleWithZone)myDry;
                    foreach (MixHotelModuleWithZone.Area oArea in mixModuleTmp.Areas)
                    {
                        TreeNode OND = ((TreeView)sender).Nodes.Add(oArea.ID.ToString(), oArea.ID + "-" + oArea.Remark, 0, 0);

                        switch (bytCh2Sce2Series)
                        {
                            case 1:
                                for (int intI = 0; intI < mixModuleTmp.ChnList.Count; intI++)
                                {
                                    if (mixModuleTmp.ChnList[intI].intBelongs == (oArea.ID))
                                        OND.Nodes.Add(null, (intI + 1).ToString() + "-" + mixModuleTmp.ChnList[intI].Remark, 1, 1);
                                }
                                break;
                            case 2:
                                if (oArea.Scen != null)
                                {
                                    for (int intI = 0; intI < oArea.Scen.Count; intI++)
                                    {
                                        OND.Nodes.Add(null, oArea.Scen[intI].ID + "-" + oArea.Scen[intI].Remark, 1, 1);
                                    }
                                }
                                break;
                        }
                    }
                }

            }
            catch
            {
            }
        }

        private void resetChnsWaittingAllocation()
        {
            if (myDry.ChnList == null || myDry.ChnList.Count == 0) return;
            try
            {
                dgvZoneChn1.Rows.Clear();
                for (int i = 0; i < myDry.ChnList.Count; i++)
                {
                    Channel chnTmp = myDry.ChnList[i];
                    if (chnTmp.intBelongs == 0)
                    {
                        string strChn = "";

                        strChn = (i + 1).ToString();
                        object[] obj = new object[] { strChn, chnTmp.Remark, false };
                        dgvZoneChn1.Rows.Add(obj);
                    }
                }
            }
            catch { }
        }

        private void helpToolStripButton_Click(object sender, EventArgs e)
        {
            if (myDry == null) return;
        }

        private void tsbDown_Click(object sender, EventArgs e)
        {
            try
            {
                txtONDelay.Visible = false;
                cbDimmingCurve.Visible = false;
                byte bytTag = Convert.ToByte(((ToolStripButton)sender).Tag.ToString());
                bool blnShowMsg = (CsConst.MyEditMode != 1);
                if (HDLPF.UploadOrReadGoOn(this.Text, bytTag, blnShowMsg))
                {
                    Cursor.Current = Cursors.WaitCursor;
                    // SetVisableForDownOrUpload(false);

                    SetActivePageIndexInGlobleUnit();

                    // ReadDownLoadThread();  //增加线程，使得当前窗体的任何操作不被限制

                    CsConst.MyUPload2DownLists = new List<byte[]>();

                    byte[] ArayRelay = new byte[] { SubNetID, DeviceID, (byte)(mywdDevicerType / 256), (byte)(mywdDevicerType % 256)
                        , (byte)MyActivePage,(byte)(mintIDIndex/256),(byte)(mintIDIndex%256)  };
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
            switch (tabMs04.SelectedTab.Name)
            {
                case "tbKeys": ShowKeysInformationPanel(); break;
                case "tabSecurity": DisplaySecuritySetup(); break;
                case "tabChn": showChannelsInfo(); break;
                case "tabOther": showOtherInfo(); break;
                case "tabIO": showInputAndOutput(); break;
                case "tabCurtain": ShowCurtainChannelInformation(); break;
                case "tabRelay": LoadBasicInformationToForm(); break;
                case "tabIOModule": cbChannel_SelectedIndexChanged(cbChannel, null); break;
                case "tabIoNewFunctions": showOutputAndOther(); break;
                case "tabArea": ShowAreasToListview(tvZone,1);
                                resetChnsWaittingAllocation();
                                break;
                case "tabScenes": ShowAreasToListview(tvScene, 2);
                                break;   
            }
            this.BringToFront();
        }


        private void showOutputAndOther()
        {
            try
            {
                MyBlnReading = true;
                if (myDry == null) return;
                if (myDry.arayOutput[0] < cboDryO1.Items.Count)
                    cboDryO1.SelectedIndex = myDry.arayOutput[0];
                if (myDry.arayOutput[1] < cboDryO2.Items.Count)
                    cboDryO2.SelectedIndex = myDry.arayOutput[1];
                for (int i = 0; i < 8; i++)
                {
                    if (HDLSysPF.GetBit(myDry.bytChannelStatus, i) == 1) chbList.SetItemChecked(i, true);
                    else chbList.SetItemChecked(i, false);
                }
            }
            catch
            {
            }
            MyBlnReading = false;
        }

        void LoadBasicInformationToForm()
        {
            try
            {
                if (myDry is MS04GenerationOne2R)
                {
                    MyBlnReading = true;
                    MS04GenerationOne2R Tmp = (MS04GenerationOne2R)myDry;
                    {
                        if (Tmp == null) return;
                        if (Tmp.Chans == null) return;

                        dgRelay.Rows.Clear();

                        foreach (RelayChannel ch in Tmp.Chans)
                        {
                            object[] boj = new object[] { dgRelay.RowCount + 1, ch.Remark, cbType.Items[ch.LoadType], 
                                            HDLPF.GetStringFromTime(ch.OnDelay, "."), ch.ProtectDelay, 
                                            HDLPF.GetStringFromTime(ch.OFFDelay, "."), ch.OFFProtectDelay, 
                                            false};
                            dgRelay.Rows.Add(boj);
                        }
                    }
                }
            }
            catch
            {
            }
        }

        void ShowCurtainChannelInformation()
        {
            try
            {
                dgCurtain.Rows.Clear();
                if (myDry == null) return;
                if (myDry is MS04GenerationOneCurtain)
                {
                    MyBlnReading = true;
                    MS04GenerationOneCurtain Tmp = (MS04GenerationOneCurtain)myDry;
                    {
                        object[] obj = new object[] { DgChns.RowCount + 1, Tmp.Chans.remark,
                                                                   HDLPF.GetStringFromTime(Tmp.Chans.onDelay, ":"),
                                                                   HDLPF.GetStringFromTime(Tmp.Chans.offDelay,":"),
                                                                   HDLPF.GetStringFromTime(Tmp.Chans.runTime,":"),
                                                                   false};
                        dgCurtain.Rows.Add(obj);
                    }
                }
            }
            catch
            {
            }
            MyBlnReading = false;
        }

        private void showInputAndOutput()
        {
            try
            {
                MyBlnReading = true;
                if (myDry == null) return;
                if (myDry.myRange == null) return;
                if (cbChannel.Items.Count > 0 && cbChannel.SelectedIndex < 0) cbChannel.SelectedIndex = 0;

            }
            catch
            {
            }
            MyBlnReading = false;
        }

        private void showChannelsInfo()
        {
            DgChns.Rows.Clear();
            int ChnID = 1;
            try
            {
                if (myDry is MS04GenerationOneD)
                {
                    MyBlnReading = true;
                    MS04GenerationOneD Tmp = (MS04GenerationOneD)myDry;
                    {
                        if (Tmp == null) return;
                        if (Tmp.Chans == null) return;
                       
                        foreach (DimmerChannelGenerationTwo chnTmp in Tmp.Chans)
                        {
                            object[] obj = new object[] { ChnID.ToString() , chnTmp.remark,chnTmp.minValue.ToString(),chnTmp.maxValue.ToString(),chnTmp.maxLevel.ToString(),
                                                    clChn9.Items[chnTmp.dimmingProfile],false};
                            DgChns.Rows.Add(obj);
                            ChnID++;
                        }
                    }
                }
                else if (MS04DeviceTypeList.MS04HotelMixModule.Contains(mywdDevicerType)) //酒店混合模块
                {
                    if (myDry == null) return;
                    if (myDry.ChnList == null) return;

                    foreach (Channel chnTmp in myDry.ChnList)
                    {
                        if (chnTmp.ID > 2)
                        {
                            String strChn = chnTmp.ID.ToString() + "-" + CsConst.WholeTextsList[727].sDisplayName;
                            object[] obj = new object[] { strChn, chnTmp.Remark, "N/A","N/A","N/A",clChn9.Items[0],HDLPF.GetStringFromTime(chnTmp.PowerOnDelay,"."), 
                                                    chnTmp.ProtectDealy,false};
                            DgChns.Rows.Add(obj);
                        }
                        else if (chnTmp.ID <= 2)
                        {
                            String strChn = chnTmp.ID.ToString() + "-" + CsConst.WholeTextsList[2244].sDisplayName;
                            object[] obj = new object[] { strChn, chnTmp.Remark, chnTmp.MinValue.ToString(),chnTmp.MaxValue.ToString(),chnTmp.MaxLevel.ToString(),
                                                     clChn9.Items[chnTmp.CurveID],"N/A","N/A",false};
                            DgChns.Rows.Add(obj);
                        }
                    }
                }
            }
            catch
            {
            }
            MyBlnReading = false;
        }

        void ShowKeysInformationPanel()
        {
            try
            {
                if (myDry == null) return;
                if (myDry.MSKeys == null) return;
                dgvKey.Rows.Clear();
                Byte ButtonID = 1;
                for (int i = 0; i < myDry.MSKeys.Count; i++)
                {
                    MS04Key TempKey = myDry.MSKeys[i];
                    String DryModeTmp = DryMode.ConvertorKeyModeToPublicModeGroup(TempKey.Mode);
                    int TmpMode = clK3.Items.IndexOf(DryModeTmp);

                    if (TempKey.Mode == 0 || TmpMode == -1)
                    {
                        String OnDelay = HDLPF.GetStringFromTime(TempKey.ONOFFDelay[0], ":");
                        String OffDelay = HDLPF.GetStringFromTime(TempKey.ONOFFDelay[1], ":");
                        object[] obj = new object[] { ButtonID, TempKey.Remark, DryModeTmp, OnDelay, OffDelay, false }; //strDimming, strSaveDimmingValue, strLED,strMutex,strDouble
                        dgvKey.Rows.Add(obj);
                    }
                    else
                    {
                        object[] obj = new object[] { ButtonID, TempKey.Remark, DryModeTmp, CsConst.mstrInvalid, CsConst.mstrInvalid, false }; //strDimming, strSaveDimmingValue, strLED,strMutex,strDouble
                        int RowID = dgvKey.Rows.Add(obj);
                        dgvKey[3, RowID].ReadOnly = true;
                        dgvKey[4, RowID].ReadOnly = true;
                    }

                    sbMin.Value = myDry.dimmerLow;
                    sbMin_ValueChanged(sbMin, null);
                    if (i < cbEnable.Items.Count)
                    {
                        cbEnable.SetItemChecked(i, (TempKey.bytEnable == 1));
                        cbLock.SetItemChecked(i, (TempKey.bytReflag == 1));
                    }
                    
                    ButtonID++;
                }               
            }
            catch
            {
            }
            MyBlnReading = false;
        }


        private void DisplaySecuritySetup()
        {
            try
            {
                MyBlnReading = true;
                dgvSec.Rows.Clear();
                if (myDry.myKeySeu != null && myDry.myKeySeu.Count != 0)
                {
                    for (int i = 0; i < myDry.myKeySeu.Count; i++)
                    {
                        UVCMD.SecurityInfo reader = myDry.myKeySeu[i];
                        object[] obj = new object[]{dgvSec.RowCount + 1, reader.strRemark, (reader.bytTerms==1),
                            reader.strRemark, reader.bytSubID.ToString(), reader.bytDevID.ToString(), reader.bytArea.ToString(),false};

                        dgvSec.Rows.Add(obj);
                    }
                }
            }
            catch
            {
            }
            MyBlnReading = false;
        }

        private void frmMS04_Shown(object sender, EventArgs e)
        {
            SetCtrlsVisbleWithDifferentDeviceType(); 
            if (CsConst.MyEditMode == 1) //在线模式
            {
                
                if (myDry != null && myDry.MyRead2UpFlags[tabMs04.SelectedIndex] == false)
                {
                    tsbDown_Click(tsbDown, null);
                }
                else
                {
                    UpdateDisplayInformationAccordingly(null,null);
                }
            }
        }

        private void dgvSec_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (MyBlnReading) return;
                if (myDry == null) return;
                if (myDry.myKeySeu == null) return;
                if (e.RowIndex == -1) return;
                if (e.ColumnIndex == -1) return;
                if (dgvSec[e.ColumnIndex, e.RowIndex].Value == null) dgvSec[e.ColumnIndex, e.RowIndex].Value = "";
                for (int i = 0; i < dgvSec.SelectedRows.Count; i++)
                {
                    UVCMD.SecurityInfo tempfire = myDry.myKeySeu[dgvSec.SelectedRows[i].Index];
                    switch (e.ColumnIndex)
                    {
                        case 2:
                            tempfire.bytTerms = Convert.ToByte(dgvSec[2, dgvSec.SelectedRows[i].Index].Value.ToString().ToLower() == "true");
                            break;
                        case 3:
                            string strTmp = dgvSec[3, dgvSec.SelectedRows[i].Index].Value.ToString();
                            tempfire.strRemark = strTmp;
                            break;
                        case 4:
                            strTmp = dgvSec[4, dgvSec.SelectedRows[i].Index].Value.ToString();
                            dgvSec[4, dgvSec.SelectedRows[i].Index].Value = HDLPF.IsNumStringMode(strTmp, 0, 254);
                            tempfire.bytSubID = byte.Parse(dgvSec[4, dgvSec.SelectedRows[i].Index].Value.ToString());
                            break;
                        case 5:
                            strTmp = dgvSec[5, dgvSec.SelectedRows[i].Index].Value.ToString();
                            dgvSec[5, dgvSec.SelectedRows[i].Index].Value = HDLPF.IsNumStringMode(strTmp, 0, 254);
                            tempfire.bytDevID = byte.Parse(dgvSec[5, dgvSec.SelectedRows[i].Index].Value.ToString());
                            break;
                        case 6:
                            strTmp = dgvSec[6, dgvSec.SelectedRows[i].Index].Value.ToString();
                            dgvSec[6, dgvSec.SelectedRows[i].Index].Value = HDLPF.IsNumStringMode(strTmp, 0, 254);
                            tempfire.bytArea = byte.Parse(dgvSec[6, dgvSec.SelectedRows[i].Index].Value.ToString());
                            break;
                    }
                }
                if (e.ColumnIndex == 2)
                {
                    myDry.myKeySeu[e.RowIndex].bytTerms = Convert.ToByte(dgvSec[2, e.RowIndex].Value.ToString().ToLower() == "true");
                }
                if (e.ColumnIndex == 3)
                {
                    string strTmp = dgvSec[3, e.RowIndex].Value.ToString();
                    myDry.myKeySeu[e.RowIndex].strRemark = strTmp;
                }
                if (e.ColumnIndex == 4)
                {
                    string strTmp = dgvSec[4, e.RowIndex].Value.ToString();
                    dgvSec[4, e.RowIndex].Value = HDLPF.IsNumStringMode(strTmp, 0, 254);
                    myDry.myKeySeu[e.RowIndex].bytSubID = byte.Parse(dgvSec[4, e.RowIndex].Value.ToString());
                }
                if (e.ColumnIndex == 5)
                {
                    string strTmp = dgvSec[5, e.RowIndex].Value.ToString();
                    dgvSec[5, e.RowIndex].Value = HDLPF.IsNumStringMode(strTmp, 0, 254);
                    myDry.myKeySeu[e.RowIndex].bytDevID = byte.Parse(dgvSec[5, e.RowIndex].Value.ToString());
                }
                if (e.ColumnIndex == 6)
                {
                    string strTmp = dgvSec[6, e.RowIndex].Value.ToString();
                    dgvSec[6, e.RowIndex].Value = HDLPF.IsNumStringMode(strTmp, 0, 254);
                    myDry.myKeySeu[e.RowIndex].bytArea = byte.Parse(dgvSec[6, e.RowIndex].Value.ToString());
                }
            }
            catch
            {
            }
        }

        private void dgvSec_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (myDry == null) return;
            if (myDry.myKeySeu == null) return;
            if (e.RowIndex == -1) return;
            if (e.ColumnIndex == -1) return;

            dgvSec[7, e.RowIndex].ReadOnly = (dgvSec[2, e.RowIndex].Value.ToString().ToLower() == "false");
        }

        private void dgvSec_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            dgvSec.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }


        private void tmRead_Click(object sender, EventArgs e)
        {
            tsbDown_Click(tsbDown, null);
        }

        private void tmUpload_Click(object sender, EventArgs e)
        {
            tsbDown_Click(UploadSetup, null);
        }

        private void btnSaveOther_Click(object sender, EventArgs e)
        {
            tsbDown_Click(UploadSetup, null);
        }

        private void btnSavePage2_Click(object sender, EventArgs e)
        {
            tsbDown_Click(UploadSetup, null);
        }

        private void btnSaveAndClose2_Click(object sender, EventArgs e)
        {
            btnSavePage2_Click(null, null);
            this.Close();
        }

        private void DgChns_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (MyBlnReading) return;
                if (myDry == null) return;
                if (myDry.ChnList == null) return;
                if ((e.RowIndex == -1) || (e.ColumnIndex == -1)) return;
                if (DgChns[e.ColumnIndex, e.RowIndex].Value == null) DgChns[e.ColumnIndex, e.RowIndex].Value = "";
                MS04GenerationOneD oRe =null;
                if (myDry is MS04GenerationOneD)
                {
                    oRe = (MS04GenerationOneD)myDry;
                }
                else if (MS04DeviceTypeList.MS04HotelMixModule.Contains(mywdDevicerType)) // 酒店混合模块
                {
                    if (myDry.ChnList == null) return;
                }

               #region
                for (int i = 0; i < DgChns.SelectedRows.Count; i++)
                {
                    DgChns.SelectedRows[i].Cells[e.ColumnIndex].Value = DgChns[e.ColumnIndex, e.RowIndex].Value.ToString();

                    string strTmp = "";
                    switch (e.ColumnIndex)
                    {
                        case 1:
                            strTmp = DgChns[1, DgChns.SelectedRows[i].Index].Value.ToString();
                            DgChns[1, DgChns.SelectedRows[i].Index].Value = HDLPF.IsRightStringMode(strTmp);

                            if (oRe != null)
                            {
                                oRe.Chans[DgChns.SelectedRows[i].Index].remark = DgChns[1, DgChns.SelectedRows[i].Index].Value.ToString();
                            }
                            else
                            {
                                myDry.ChnList[DgChns.SelectedRows[i].Index].Remark = DgChns[1, DgChns.SelectedRows[i].Index].Value.ToString();
                            }
                            break;
                        case 2:
                            strTmp = DgChns[2, DgChns.SelectedRows[i].Index].Value.ToString();
                            if (oRe != null)
                            {
                                oRe.Chans[DgChns.SelectedRows[i].Index].minValue = int.Parse(HDLPF.IsNumStringMode(strTmp, 0, 100));
                            }
                            else
                            {
                                myDry.ChnList[DgChns.SelectedRows[i].Index].MinValue = int.Parse(HDLPF.IsNumStringMode(strTmp, 0, 100));
                            }
                            break;
                        case 3:
                            strTmp = DgChns[3, DgChns.SelectedRows[i].Index].Value.ToString();
                            if (oRe != null)
                            {
                                oRe.Chans[DgChns.SelectedRows[i].Index].maxValue = int.Parse(HDLPF.IsNumStringMode(strTmp, 0, 100));
                            }
                            else
                            {
                                myDry.ChnList[DgChns.SelectedRows[i].Index].MaxValue = int.Parse(HDLPF.IsNumStringMode(strTmp, 0, 100));
                            }
                            break;
                        case 4:
                            strTmp = DgChns[4, DgChns.SelectedRows[i].Index].Value.ToString();
                            if (oRe != null)
                            {
                                oRe.Chans[DgChns.SelectedRows[i].Index].maxLevel = int.Parse(HDLPF.IsNumStringMode(strTmp, 0, 100));
                            }
                            else
                            {
                                myDry.ChnList[DgChns.SelectedRows[i].Index].MaxLevel = int.Parse(HDLPF.IsNumStringMode(strTmp, 0, 100));
                            }
                            break;
                        case 5:
                            int dimmerLevel = clChn9.Items.IndexOf(DgChns[5, DgChns.SelectedRows[i].Index].Value.ToString());
                            if (oRe != null)
                            {
                                oRe.Chans[DgChns.SelectedRows[i].Index].dimmingProfile = Convert.ToByte(dimmerLevel);
                            }
                            else
                            {
                                myDry.ChnList[DgChns.SelectedRows[i].Index].CurveID = Convert.ToByte(dimmerLevel); 
                            }
                            break;
                        case 6:
                            DgChns[6, DgChns.SelectedRows[i].Index].Value = HDLPF.GetStringFromTime(int.Parse(txtONDelay.Text.ToString()), ".");
                            myDry.ChnList[DgChns.SelectedRows[i].Index].PowerOnDelay = int.Parse(HDLPF.GetTimeFromString(DgChns[6, DgChns.SelectedRows[i].Index].Value.ToString(), '.'));
                            break;
                        case 7:
                            strTmp = DgChns[7, DgChns.SelectedRows[i].Index].Value.ToString();
                            DgChns[7, DgChns.SelectedRows[i].Index].Value = HDLPF.IsNumStringMode(strTmp, 0, 60);
                            myDry.ChnList[DgChns.SelectedRows[i].Index].ProtectDealy = byte.Parse((HDLPF.IsNumStringMode(DgChns[7, DgChns.SelectedRows[i].Index].Value.ToString(), 0, 60)));
                            break;
                        case 8:

                            byte[] bytTmp = new byte[4];
                            bytTmp[0] = (byte)(DgChns.SelectedRows[i].Index + 1);
                            bytTmp[2] = 0;
                            bytTmp[3] = 0;
                            if (DgChns[3, DgChns.SelectedRows[i].Index].Value.ToString().ToLower() == "true")
                                bytTmp[1] = 100;
                            else bytTmp[1] = 0;

                            Cursor.Current = Cursors.WaitCursor;
                            CsConst.mySends.AddBufToSndList(bytTmp, 0x0031, SubNetID, DeviceID, false, true, true, false);
                            Cursor.Current = Cursors.Default;
                            break;
                    }
                }                    
            }
            #endregion
            catch
            {
            }
        }

        private void DgChns_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            DgChns.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        void txtONDelay_TextChanged(object sender, EventArgs e)
        {
            if (myDry == null) return;
            if (myDry.ChnList == null) return;
            if ((DgChns.CurrentCell.RowIndex == -1) || (DgChns.CurrentCell.ColumnIndex == -1)) return;
            if (txtONDelay.Visible)
            {
                DgChns[6, DgChns.CurrentCell.RowIndex].Value = HDLPF.GetStringFromTime(int.Parse(txtONDelay.Text.ToString()), ".");
                ModifyMultilinesIfNeeds(DgChns[6, DgChns.CurrentCell.RowIndex].Value.ToString(), 6, DgChns);
            }
        }

        private void DgChns_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtONDelay.Visible = false;
            cbDimmingCurve.Visible = false;
            if (e.RowIndex >= 0)
            {
                if (e.RowIndex >= 2)
                {
                    if (e.ColumnIndex == 6)
                    {
                        addcontrols(6, e.RowIndex, txtONDelay, DgChns);
                        txtONDelay.Text = HDLPF.GetTimeFromString(DgChns[6, e.RowIndex].Value.ToString(), '.'); 
                    }
                }
                else
                {
                    if (e.ColumnIndex == 8)
                    {
                        addcontrols(8, e.RowIndex, cbDimmingCurve, DgChns);
                        cbDimmingCurve.SelectedIndex = cbDimmingCurve.Items.IndexOf(DgChns[8, e.RowIndex].Value.ToString());
                    }
                }
            }
            if (txtONDelay.Visible) txtONDelay_TextChanged(null, null);
            if (cbDimmingCurve.Visible) cbDimmingCurve_SelectedIndexChanged(null, null);
        }

        private void addcontrols(int col, int row, Control con, DataGridView dgv)
        {
            con.Show();
            con.Visible = true;
            Rectangle rect = dgv.GetCellDisplayRectangle(col, row, true);
            con.Size = rect.Size;
            con.Top = rect.Top;
            con.Left = rect.Left;
        }


        void ModifyMultilinesIfNeeds(string strTmp, int ColumnIndex, DataGridView dgv)
        {
            if (dgv.SelectedRows == null || dgv.SelectedRows.Count == 0) return;
            if (strTmp == null) strTmp = "";
            // change the value in selected more than one line
            if (dgv.SelectedRows.Count > 1)
            {
                for (int i = 0; i < dgv.SelectedRows.Count; i++)
                {
                    dgv.SelectedRows[i].Cells[ColumnIndex].Value = strTmp;
                }
            }
        }

        private void rbOFF_CheckedChanged(object sender, EventArgs e)
        {
            MyBlnReading = true;
            try
            {
                if (rbON.Checked == false && rbOFF.Checked == false) rbON.Checked = true;
                int ID = cbLED.SelectedIndex;
                if (rbON.Checked)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        if (myDry.arayUVStaus[ID * 12 + i + 6] <= 3)
                        {
                            RadioButton temp = this.Controls.Find("rb" + (i + 1).ToString() + "UV" + (myDry.arayUVStaus[ID * 12 + i + 6]).ToString(), true)[0] as RadioButton;
                            temp.Checked = true;
                        }
                        else
                        {
                            RadioButton temp = this.Controls.Find("rb" + (i + 1).ToString() + "UV3", true)[0] as RadioButton;
                            temp.Checked = true;
                        }
                    }
                }
                else if (rbOFF.Checked)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        if (myDry.arayUVStaus[ID * 12 + i] <= 3)
                        {
                            RadioButton temp = this.Controls.Find("rb" + (i + 1).ToString() + "UV" + (myDry.arayUVStaus[ID * 12 + i]).ToString(), true)[0] as RadioButton;
                            temp.Checked = true;
                        }
                        else
                        {
                            RadioButton temp = this.Controls.Find("rb" + (i + 1).ToString() + "UV3", true)[0] as RadioButton;
                            temp.Checked = true;
                        }
                    }
                }
            }
            catch
            {
                MyBlnReading = false;
            }
            MyBlnReading = false;
        }

        private void cbLED_SelectedIndexChanged(object sender, EventArgs e)
        {
            rbOFF_CheckedChanged(null, null);
        }

        private void radioButton220V_CheckedChanged(object sender, EventArgs e)
        {
            if (MyBlnReading) return;
            if (myDry == null) return;
            if (radioButton110V.Checked) myDry.bytVotoge = 1;
            else if (radioButton220V.Checked) myDry.bytVotoge = 0;
        }

        private void TimeCurtain_TextChanged(object sender, EventArgs e)
        {
            if (MyBlnReading) return;
            if (myDry == null) return;
            if (myDry.arayCurtain == null) return;
            try
            {
                Byte bytTag = Convert.ToByte(((TimeMs)sender).Tag.ToString());
                int num = Convert.ToInt32(((TimeMs)sender).Text);
                myDry.arayCurtain[1 + bytTag * 3] = Convert.ToByte(num / 256);
                myDry.arayCurtain[2 + bytTag * 3] = Convert.ToByte(num % 256);
            }
            catch
            { }
        }

        private void chbCurtain_CheckedChanged(object sender, EventArgs e)
        {
            if (MyBlnReading) return;
            if (myDry == null) return;
            if (myDry.arayCurtain == null) return;
            try
            {
                Byte bytTag = Convert.ToByte(((CheckBox)sender).Tag.ToString());
                if (((CheckBox)sender).Checked == true)
                    myDry.arayCurtain[bytTag] = 1;
                else
                    myDry.arayCurtain[bytTag] = 0;
            }
            catch
            {}
        }

        private void numDoorbell_ValueChanged(object sender, EventArgs e)
        {
            if (MyBlnReading) return;
            if (myDry == null) return;
            myDry.bytDoorBellRunTime = Convert.ToByte(numDoorbell.Value);
        }

        private void cbO1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (MyBlnReading) return;
            if (myDry == null) return;
            int Tag = Convert.ToInt32((sender as ComboBox).Tag);
            myDry.arayOutput[Tag] = Convert.ToByte((sender as ComboBox).SelectedIndex);
        }

        private void txtR1_TextChanged(object sender, EventArgs e)
        {
            if (MyBlnReading) return;
            if (myDry == null) return;
            int Tag = Convert.ToInt32((sender as TextBox).Tag);
            ComboBox temp1 = this.Controls.Find("cbC" + (Tag + 1).ToString(), true)[0] as ComboBox;
            if (temp1.SelectedIndex == 0)
            {
                myDry.myRange[Tag].arayPoint[0] = 0;
                myDry.myRange[Tag].arayPoint[1] = 0;
            }
            else
            {
                if ((sender as TextBox).Text.Length > 0)
                {
                    int num = Convert.ToInt32((sender as TextBox).Text);
                    myDry.myRange[Tag].arayPoint[0] = Convert.ToByte(num % 256);
                    myDry.myRange[Tag].arayPoint[1] = Convert.ToByte(num / 256);
                }
            }
            
        }

        private void txtE1_TextChanged(object sender, EventArgs e)
        {
            if (MyBlnReading) return;
            if (myDry == null) return;
            int Tag = Convert.ToInt32((sender as TextBox).Tag);
            ComboBox temp1 = this.Controls.Find("cbC" + (Tag + 1).ToString(), true)[0] as ComboBox;
            if (temp1.SelectedIndex == 0)
            {
                myDry.myRange[Tag].arayPoint[2] = 0;
                myDry.myRange[Tag].arayPoint[3] = 0;
            }
            else
            {
                if ((sender as TextBox).Text.Length > 0)
                {
                    int num = Convert.ToInt32((sender as TextBox).Text);
                    myDry.myRange[Tag].arayPoint[2] = Convert.ToByte(num % 256);
                    myDry.myRange[Tag].arayPoint[3] = Convert.ToByte(num / 256);
                }
            }
        }

        private void txtE5_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar) || e.KeyChar == 8)
                e.Handled = false;
            else
                e.Handled = true;
        }

        private void txtR1_Leave(object sender, EventArgs e)
        {
            string str = (sender as TextBox).Text;
            int Tag = Convert.ToInt32((sender as TextBox).Tag);
            TextBox temp = this.Controls.Find("txtE" + (Tag + 1).ToString(), true)[0] as TextBox;
            int num = Convert.ToInt32(temp.Text);
            ComboBox cb1 = this.Controls.Find("cbC" + (Tag + 1).ToString(), true)[0] as ComboBox;
            if (cb1.SelectedIndex == 3 || cb1.SelectedIndex == 4)
                (sender as TextBox).Text = HDLPF.IsNumStringMode(str, 4000, num);
            else if (cb1.SelectedIndex == 0)
                (sender as TextBox).Text = "0";
            else
                (sender as TextBox).Text = HDLPF.IsNumStringMode(str, 0, num);
            (sender as TextBox).SelectionStart = (sender as TextBox).Text.Length;
        }

        private void txtE1_Leave(object sender, EventArgs e)
        {
            string str = (sender as TextBox).Text;
            int Tag = Convert.ToInt32((sender as TextBox).Tag);
            TextBox temp = this.Controls.Find("txtR" + (Tag + 1).ToString(), true)[0] as TextBox;
            ComboBox cb1 = this.Controls.Find("cbC" + (Tag + 1).ToString(), true)[0] as ComboBox;
            ComboBox cb2 = this.Controls.Find("cbA" + (Tag + 1).ToString(), true)[0] as ComboBox;
            int num = Convert.ToInt32(temp.Text);
            if (cb1.SelectedIndex == 1 || cb1.SelectedIndex == 2)
            {
                (sender as TextBox).Text = HDLPF.IsNumStringMode(str, 0, 10000);
            }
            else if (cb1.SelectedIndex == 3 || cb1.SelectedIndex == 4)
            {
                (sender as TextBox).Text = HDLPF.IsNumStringMode(str, 4000, 20000);
            }
            else
            {
                (sender as TextBox).Text = "0";
            }
            (sender as TextBox).SelectionStart = (sender as TextBox).Text.Length;
        }

        private void btnRefreshPage_Click(object sender, EventArgs e)
        {
            tsbDown_Click(tsbDown, null);
        }

        private void btnSaveBasic_Click(object sender, EventArgs e)
        {
            try
            {
                if (tabMs04.SelectedTab.Name == "tabIOModule")
                {
                    if (myDry == null) return;
                    if (myDry.myRange == null) return;
                    if (myDry.myRange.Count < cbChannel.Items.Count) return;
                    myDry.myRange[cbChannel.SelectedIndex].Common = 0;
                    myDry.myRange[cbChannel.SelectedIndex].InputType = Convert.ToByte(cbInputType.SelectedIndex);
                    if (chbBroadcast.Checked)
                        myDry.myRange[cbChannel.SelectedIndex].Broadcat = 2;
                    else
                        myDry.myRange[cbChannel.SelectedIndex].Broadcat = 0;
                    myDry.myRange[cbChannel.SelectedIndex].Min = Convert.ToInt32(txtMin.Text);
                    myDry.myRange[cbChannel.SelectedIndex].Max = Convert.ToInt32(txtMax.Text);

                    #region
                    if (cbInputType.SelectedIndex == 2 || cbInputType.SelectedIndex == 4)
                    {
                        myDry.myRange[cbChannel.SelectedIndex].Range1 = Convert.ToInt32(cbRange1.SelectedIndex);
                        myDry.myRange[cbChannel.SelectedIndex].Range2 = Convert.ToInt32(cbRange2.SelectedIndex);
                        myDry.myRange[cbChannel.SelectedIndex].AnalogType = Convert.ToByte(cbAnalog.SelectedIndex);
                        myDry.myRange[cbChannel.SelectedIndex].Count = Convert.ToInt32(cbScale.Text);
                        if (cbShare.Visible && cbShare.SelectedIndex > 0)
                        {
                            if (cbShare.SelectedIndex > 0)
                                myDry.myRange[cbChannel.SelectedIndex].Common = Convert.ToByte(cbShare.Text);
                            else
                                myDry.myRange[cbChannel.SelectedIndex].Common = 0;
                        }
                        else
                        {
                            myDry.myRange[cbChannel.SelectedIndex].Common = 0;
                        }

                        for (int i = 0; i < dgvCurve.ColumnCount; i++)
                        {
                            int intTmp = Convert.ToInt32(dgvCurve[i, 0].Value.ToString());
                            if (cbStart.Visible && cbStart.Items.Count > 0 && cbStart.SelectedIndex > 0)
                            {
                                int index = cbRange1.Items.IndexOf(cbStart.Text);
                                if (Convert.ToInt32(dgvCurve.Columns[i].Tag) == 65535)
                                {
                                    intTmp = (int)(intTmp | (1 << 15));
                                    myDry.myRange[cbChannel.SelectedIndex].arayPoint[index + i] = intTmp;
                                }
                                else
                                {
                                    myDry.myRange[cbChannel.SelectedIndex].arayPoint[index + i] = intTmp;
                                }
                            }
                            else
                            {
                                if (Convert.ToInt32(dgvCurve.Columns[i].Tag) == 65535)
                                {
                                    intTmp = (int)(intTmp | (1 << 15));
                                    myDry.myRange[cbChannel.SelectedIndex].arayPoint[i] = intTmp;
                                }
                                else
                                {
                                    myDry.myRange[cbChannel.SelectedIndex].arayPoint[i] = intTmp;
                                }
                            }
                        }
                    }
                    else if (cbInputType.SelectedIndex == 1 || cbInputType.SelectedIndex == 3)
                    {
                        myDry.myRange[cbChannel.SelectedIndex].Range1 = Convert.ToInt32(txtRange1.Text);
                        myDry.myRange[cbChannel.SelectedIndex].Range2 = Convert.ToInt32(txtRange2.Text);
                    }
                    #endregion 
                }

                tsbDown_Click(UploadSetup, null);
            }
            catch 
            { }
        }

        private void btnSaveAndClose_Click_1(object sender, EventArgs e)
        {
            tsbDown_Click(UploadSetup, null);
            this.Close();
        }

        private void dgvKey_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            txtScene.Visible = false;
            if (dgvKey.SelectedRows == null) return;
            if (dgvKey.Rows == null) return;
            selectedDryId = Convert.ToByte(dgvKey[0, e.RowIndex].Value.ToString());
            if (e.RowIndex >= 0)
            {
                Byte Mode = DryMode.ConvertorKeyModesToPublicModeGroup(dgvKey[2, e.RowIndex].Value.ToString());
                dgvKey[3, e.RowIndex].ReadOnly = (Mode != 0);
                dgvKey[4, e.RowIndex].ReadOnly = (Mode != 0);
                if (dgvKey[3, e.RowIndex].ReadOnly == true)
                {
                    dgvKey[3, e.RowIndex].Value = CsConst.mstrInvalid;
                    dgvKey[4, e.RowIndex].Value = CsConst.mstrInvalid;
                }

                if (e.ColumnIndex == 3 || e.ColumnIndex == 4)
                {
                    if (dgvKey[e.ColumnIndex, e.RowIndex].Value.ToString() == CsConst.mstrInvalid) return;
                    txtScene.Text = HDLPF.GetTimeFromString(dgvKey[e.ColumnIndex, e.RowIndex].Value.ToString(), ':');
                    HDLSysPF.addcontrols(e.ColumnIndex, e.RowIndex, txtScene, dgvKey);
                }

                gbTemperature.Visible = (Mode == 100);
                if (gbTemperature.Visible == true)
                {
                    MS04GenerationOneT Tmp = (MS04GenerationOneT)myDry;
                    MyBlnReading = true;
                    foreach (TemperatureSource temperatureSensor in Tmp.temperatureSensors)
                    {
                        if (temperatureSensor.channelID == selectedDryId)
                        {
                            chbBroadCastT.Checked = temperatureSensor.enableBroadcast;
                            txtBroadSub.Text = temperatureSensor.broadcastToSubnetID.ToString();
                            txtBroadDev.Text = temperatureSensor.broadcastToDeviceID.ToString();
                            if (temperatureSensor.adjustTemperature >= sbAdjust.Minimum && temperatureSensor.adjustTemperature <= sbAdjust.Maximum)
                                sbAdjust.Value = Convert.ToInt32(temperatureSensor.adjustTemperature);
                            if (HDLSysPF.GetBit(temperatureSensor.adjustTemperature, 7) == 1)
                                lbCurTempValue.Text = "-" + ((temperatureSensor.adjustTemperature & (byte.MaxValue - (1 << 7)))).ToString() + "C";
                            else
                                lbCurTempValue.Text = temperatureSensor.adjustTemperature.ToString() + "C";
                            sbAdjust_ValueChanged(null, null);
                            break;
                        }
                    }
                    MyBlnReading = false;
                }
            }
        }

        private void dgvKey_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            dgvKey.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void sbMin_ValueChanged(object sender, EventArgs e)
        {
            lbv4.Text = sbMin.Value.ToString();
            if (MyBlnReading) return;
            if (myDry == null) return;
            myDry.dimmerLow = byte.Parse(sbMin.Value.ToString());
        }

        private void cbEnable_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cbEnable_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (MyBlnReading) return;
            if (myDry == null) return;
            if (myDry.MSKeys == null || myDry.MSKeys.Count ==0) return;

            myDry.MSKeys[e.Index].bytEnable = Convert.ToByte(e.NewValue); 
        }

        private void cbLock_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (MyBlnReading) return;
            if (myDry == null) return;
            if (myDry.MSKeys == null || myDry.MSKeys.Count == 0) return;

            myDry.MSKeys[e.Index].bytReflag = Convert.ToByte(e.NewValue); 
        }

        private void btnMode_Click(object sender, EventArgs e)
        {
            frmButtonSetup ButtonConfigration = new frmButtonSetup(myDry, MyDevName, null);
            ButtonConfigration.ShowDialog();
        }

        private void btnCMD_Click(object sender, EventArgs e)
        {
            Byte[] PageID = new Byte[4];
            if (dgvKey.SelectedRows != null && dgvKey.SelectedRows.Count > 0)
            {
                PageID[0] = (Byte)dgvKey.SelectedRows[0].Index;
            }
            frmCmdSetup CmdSetup = new frmCmdSetup(myDry, MyDevName, mywdDevicerType, PageID);
            CmdSetup.Show();
        }

        private void dgCurtain_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 2 || e.ColumnIndex == 3 || e.ColumnIndex == 4 || e.ColumnIndex == 5)
            {
                 if (dgCurtain[e.ColumnIndex, e.RowIndex].Value.ToString() == CsConst.mstrInvalid) return;
                 CurtainDelay.Text = HDLPF.GetTimeFromString(dgCurtain[e.ColumnIndex, e.RowIndex].Value.ToString(), ':');
                 HDLSysPF.addcontrols(e.ColumnIndex, e.RowIndex, CurtainDelay, dgCurtain);
            }
            else
            {
                CurtainDelay.Visible = false;
            }
        }

        private void dgCurtain_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (myDry == null) return;
                if ((e.RowIndex == -1) || (e.ColumnIndex == -1)) return;
                if (dgCurtain[e.ColumnIndex, e.RowIndex].Value == null) return;
                if (dgCurtain.SelectedRows.Count == 0) return;

                if (dgCurtain[e.ColumnIndex, e.RowIndex].Value == null) dgCurtain[e.ColumnIndex, e.RowIndex].Value = "";
                if (myDry is MS04GenerationOneCurtain)
                {
                    MyBlnReading = true;
                    MS04GenerationOneCurtain Tmp = (MS04GenerationOneCurtain)myDry;

                    for (int i = 0; i < dgCurtain.SelectedRows.Count; i++)
                    {
                        string strTmp = "";
                        int RowID = dgCurtain.SelectedRows[i].Index;
                        switch (e.ColumnIndex)
                        {

                            case 1:
                                strTmp = dgCurtain[e.ColumnIndex, RowID].Value.ToString();
                                Tmp.Chans.remark = dgCurtain[e.ColumnIndex, RowID].Value.ToString();
                                break;
                            case 2:
                                int DelayTime = HDLPF.GetTimeIntegerFromString(dgCurtain[e.ColumnIndex, RowID].Value.ToString(), ':');
                                Tmp.Chans.onDelay = DelayTime;
                                break;
                            case 3:
                                DelayTime = HDLPF.GetTimeIntegerFromString(dgCurtain[e.ColumnIndex, RowID].Value.ToString(), ':');
                                Tmp.Chans.offDelay = DelayTime;
                                break;

                            case 4:
                                DelayTime = HDLPF.GetTimeIntegerFromString(dgCurtain[e.ColumnIndex, RowID].Value.ToString(), ':');
                                Tmp.Chans.runTime = DelayTime;
                                break;
                            case 5:
                                byte[] bytTmp = new byte[2];
                                bytTmp[0] = (byte)(dgCurtain.SelectedRows[i].Index + 1);
                                bytTmp[2] = 255;

                                Cursor.Current = Cursors.WaitCursor;
                                if (CsConst.mySends.AddBufToSndList(bytTmp, 0xE01C, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(mywdDevicerType)) == false)
                                {
                                    //DgChns.BeginInvoke(new SetvalueHandle(Setvalue), e.RowIndex);
                                }

                                Cursor.Current = Cursors.Default;
                                break;
                        }
                        DgChns.SelectedRows[i].Cells[e.ColumnIndex].Value = DgChns[e.ColumnIndex, e.RowIndex].Value.ToString();
                    }
                }
            }
            catch
            {
            }
        }

        private void dgRelay_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            MyBlnReading = true;
            RelayDelay.Visible = false;
            if (e.RowIndex >= 0)
            {
                if (e.ColumnIndex == 3 || e.ColumnIndex == 5)
                {
                    string strTmp = HDLPF.GetTimeFromString(dgRelay[e.ColumnIndex, e.RowIndex].Value.ToString(), '.');
                    RelayDelay.Text = strTmp;
                    addcontrols(e.ColumnIndex, e.RowIndex, RelayDelay, dgRelay);
                }
            }
            MyBlnReading = false;
        }

        private void dgRelay_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (myDry == null) return;
            if ((e.RowIndex == -1) || (e.ColumnIndex == -1)) return;
            if (dgRelay[e.ColumnIndex, e.RowIndex].Value == null) dgRelay[e.ColumnIndex, e.RowIndex].Value = "";

            try
            {
                if (myDry is MS04GenerationOne2R)
                {
                    MS04GenerationOne2R oRe = (MS04GenerationOne2R)myDry;
                    if (oRe == null) return;
                    if (oRe.Chans == null) return;

                    //界面处理
                    #region
                    for (int i = 0; i < dgRelay.SelectedRows.Count; i++)
                    {
                        string strTmp = "";
                        int RowID = dgRelay.SelectedRows[i].Index;
                        switch (e.ColumnIndex)
                        {
                            case 1:
                                strTmp = DgChns[1, RowID].Value.ToString();
                                dgRelay[1, DgChns.SelectedRows[i].Index].Value = strTmp;
                                oRe.Chans[RowID].Remark = dgRelay[1, RowID].Value.ToString();
                                break;
                            case 2:
                                oRe.Chans[RowID].LoadType = cbType.Items.IndexOf(DgChns[2, RowID].Value.ToString());
                                break;
                            case 3:
                                int TmpDelay = Convert.ToInt32(RelayDelay.Text);
                                dgRelay[3, RowID].Value = HDLPF.GetStringFromTime(TmpDelay, ".");
                                oRe.Chans[RowID].OnDelay = Convert.ToInt32(RelayDelay.Text);
                                break;
                            case 4:
                                strTmp = DgChns[4, RowID].Value.ToString();
                                dgRelay[4, RowID].Value = HDLPF.IsNumStringMode(strTmp, 0, 60);
                                oRe.Chans[RowID].ProtectDelay = int.Parse(DgChns[4, RowID].Value.ToString());
                                break;
                            case 5:
                                dgRelay[5, RowID].Value = HDLPF.GetStringFromTime(int.Parse(RelayDelay.Text.ToString()), ".");
                                oRe.Chans[RowID].OFFDelay = int.Parse(HDLPF.GetTimeFromString(dgRelay[5, RowID].Value.ToString(), '.'));
                                break;
                            case 6:
                                strTmp = DgChns[6, RowID].Value.ToString();
                                dgRelay[6, RowID].Value = HDLPF.IsNumStringMode(strTmp, 0, 60);
                                oRe.Chans[RowID].OFFProtectDelay = int.Parse(DgChns[6, RowID].Value.ToString());
                                break;
                            case 7:
                                byte[] bytTmp = new byte[4];
                                bytTmp[0] = (byte)(RowID + 1);
                                bytTmp[2] = 0;
                                bytTmp[3] = 0;

                                if (dgRelay[7, RowID].Value.ToString().ToLower() == "true") bytTmp[1] = 100;
                                else bytTmp[1] = 0;

                                Cursor.Current = Cursors.WaitCursor;
                                if (CsConst.mySends.AddBufToSndList(bytTmp, 0x0031, SubNetID, DeviceID, false, false, false, CsConst.minAllWirelessDeviceType.Contains(mywdDevicerType)) == false)
                                    Cursor.Current = Cursors.Default;
                                break;
                        }
                        dgRelay.SelectedRows[i].Cells[e.ColumnIndex].Value = dgRelay[e.ColumnIndex, e.RowIndex].Value.ToString();
                    }
                    #endregion
                }
            }
            catch
            {
            }
        }

        private void dgvKey_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (myDry == null) return;
                if (myDry.MSKeys == null) return;
                if ((e.RowIndex == -1) || (e.ColumnIndex == -1)) return;
                if (dgvKey[e.ColumnIndex, e.RowIndex].Value == null) return;
                if (dgvKey.SelectedRows.Count == 0) return;

                if (dgvKey[e.ColumnIndex, e.RowIndex].Value == null) dgvKey[e.ColumnIndex, e.RowIndex].Value = "";
                String Remark = dgvKey[e.ColumnIndex, e.RowIndex].Value.ToString();

                for (int i = 0; i < dgvKey.SelectedRows.Count; i++)
                {
                    dgvKey.SelectedRows[i].Cells[e.ColumnIndex].Value = Remark;

                    string strTmp = "";
                    int RowID = dgvKey.SelectedRows[i].Index;
                    switch (e.ColumnIndex)
                    {
                        case 1:
                            strTmp = dgvKey[e.ColumnIndex, RowID].Value.ToString();
                            myDry.MSKeys[RowID].Remark = dgvKey[e.ColumnIndex, RowID].Value.ToString();
                            break;
                        case 2:
                            myDry.MSKeys[RowID].Mode = DryMode.ConvertorKeyModesToPublicModeGroup(dgvKey[e.ColumnIndex, RowID].Value.ToString());
                            dgvKey[3, RowID].ReadOnly = (myDry.MSKeys[RowID].Mode != 0);
                            dgvKey[4, RowID].ReadOnly = (myDry.MSKeys[RowID].Mode != 0);
                            if (myDry.MSKeys[RowID].Mode == 0)
                            {
                                String OnDelay = HDLPF.GetStringFromTime(myDry.MSKeys[RowID].ONOFFDelay[0], ":");
                                String OffDelay = HDLPF.GetStringFromTime(myDry.MSKeys[RowID].ONOFFDelay[1], ":");
                                dgvKey[3, RowID].Value = OnDelay;
                                dgvKey[4, RowID].Value = OffDelay;
                            }
                            else
                            {
                                dgvKey[3, e.RowIndex].Value = CsConst.mstrInvalid;
                                dgvKey[4, e.RowIndex].Value = CsConst.mstrInvalid;
                            }
                            // add new temperature if needs
                            addNewTemperatureSensorToClass(RowID);
                            break;
                        case 3:
                            int DelayTime = HDLPF.GetTimeIntegerFromString(dgvKey[e.ColumnIndex, RowID].Value.ToString(), ':');
                            myDry.MSKeys[RowID].ONOFFDelay[0] = DelayTime;
                            break;

                        case 4:
                            DelayTime = HDLPF.GetTimeIntegerFromString(dgvKey[e.ColumnIndex, RowID].Value.ToString(), ':');
                            myDry.MSKeys[RowID].ONOFFDelay[1] = DelayTime;
                            break;
                        case 5:
                            byte[] bytTmp = new byte[4];
                            bytTmp[1] = (byte)(dgvKey.SelectedRows[i].Index + 1);
                            bytTmp[0] = 18;
                            bytTmp[3] = 0;


                            if (dgvKey[e.ColumnIndex, dgvKey.SelectedRows[i].Index].Value.ToString().ToLower() == "true")
                                bytTmp[2] = 255;
                            else
                                bytTmp[2] = 0;

                            Cursor.Current = Cursors.WaitCursor;
                            if (CsConst.mySends.AddBufToSndList(bytTmp, 0xE3D8, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(mywdDevicerType)) == false)
                            {
                                //DgChns.BeginInvoke(new SetvalueHandle(Setvalue), e.RowIndex);
                            }

                            Cursor.Current = Cursors.Default;
                            break;
                    }
                }
            }
            catch
            {
            }
        }

        void addNewTemperatureSensorToClass(int RowID)
        {
            if (RowID == -1) return;
            try 
            {
                if (myDry is MS04GenerationOneT)
                {
                    Boolean isNeedAdd = true;
                    MS04GenerationOneT Tmp = (MS04GenerationOneT)myDry;

                    foreach (TemperatureSource temperatureSensor in Tmp.temperatureSensors)
                    {
                        if (temperatureSensor.channelID == RowID)
                        {
                            isNeedAdd = false;
                            break;
                        }
                    }
                    TemperatureSource tmp = new TemperatureSource();
                    tmp.channelID = Convert.ToByte((RowID +1).ToString());
                    if (Tmp.temperatureSensors == null) Tmp.temperatureSensors = new List<TemperatureSource>();
                    Tmp.temperatureSensors.Add(tmp);
                }
            }
            catch 
            { }
        }

        private void chbBroadCast_CheckedChanged(object sender, EventArgs e)
        {
            if (MyBlnReading) return;
            if (myDry == null) return;
            if (selectedDryId == -1 ) return;
            if (myDry is MS04GenerationOneT)
            {
                MS04GenerationOneT Tmp = (MS04GenerationOneT)myDry;
                foreach (TemperatureSource temperatureSensor in Tmp.temperatureSensors)
                {
                    if (temperatureSensor.channelID == selectedDryId)
                    {
                        temperatureSensor.enableBroadcast = chbBroadCastT.Checked;
                        break;
                    }
                }
            }
        }

        private void txtBroadSub_TextChanged(object sender, EventArgs e)
        {
            if (MyBlnReading) return;
            if (myDry == null) return;
            if (selectedDryId == -1) return;
            if (myDry is MS04GenerationOneT)
            {
                if (txtBroadSub.Text == null || txtBroadSub.Text == "") txtBroadSub.Text = "0";
                if (txtBroadDev.Text == null || txtBroadDev.Text == "") txtBroadDev.Text = "0";
                MS04GenerationOneT Tmp = (MS04GenerationOneT)myDry;

                foreach (TemperatureSource temperatureSensor in Tmp.temperatureSensors)
                {
                    if (temperatureSensor.channelID == selectedDryId)
                    {
                        temperatureSensor.broadcastToSubnetID = Convert.ToByte(txtBroadSub.Text);
                        temperatureSensor.broadcastToDeviceID= Convert.ToByte(txtBroadDev.Text);
                        break;
                    }
                }
            }
        }

        private void sbAdjust_ValueChanged(object sender, EventArgs e)
        {
            lbAdjust.Text = (Convert.ToInt32(sbAdjust.Value) - 10).ToString();
            if (MyBlnReading) return;
            if (myDry == null) return;
            if (selectedDryId == -1) return;
            if (myDry is MS04GenerationOneT)
            {
                MS04GenerationOneT Tmp = (MS04GenerationOneT)myDry;

                foreach (TemperatureSource temperatureSensor in Tmp.temperatureSensors)
                {
                    if (temperatureSensor.channelID == selectedDryId)
                    {
                        temperatureSensor.adjustTemperature = Convert.ToByte(sbAdjust.Value);
                        break;
                    }
                }
            }
        }

        private void cbChannel_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbChannel.SelectedIndex == -1) cbChannel.SelectedIndex = 0;
            try
            {
                if (MyBlnReading) return;
                if (myDry == null) return;
                if (myDry.myRange == null) return;
                MS04.InputRange temp = myDry.myRange[cbChannel.SelectedIndex];
                if (temp.InputType < cbInputType.Items.Count) cbInputType.SelectedIndex = temp.InputType;
                else cbInputType.SelectedIndex = 0;
                chbBroadcast.Checked = (temp.Broadcat != 0);

                if (temp.Connection == 1)
                    lbDryValue.Text = CsConst.mstrINIDefault.IniReadValue("public", "99578", "");
                else
                    lbDryValue.Text = CsConst.mstrINIDefault.IniReadValue("public", "99577", "");

                if (temp.AnalogType < cbAnalog.Items.Count) cbAnalog.SelectedIndex = temp.AnalogType;
                else cbAnalog.SelectedIndex = 0;
                if (cbInputType.SelectedIndex == 2 || cbInputType.SelectedIndex == 4)
                {
                    if (temp.Count >= 0 && temp.Count <= 4096)
                    {
                        if (cbAnalog.SelectedIndex == 0)
                        {
                            txtMin.Text = HDLPF.IsNumStringMode(temp.Min.ToString(), -30000, 30000);
                            txtMax.Text = HDLPF.IsNumStringMode(temp.Max.ToString(), -30000, 30000);
                        }
                        else if (cbAnalog.SelectedIndex == 1)
                        {
                            txtMin.Text = HDLPF.IsNumStringMode(temp.Min.ToString(), 0, 60000);
                            txtMax.Text = HDLPF.IsNumStringMode(temp.Max.ToString(), 0, 60000);
                        }
                        else if (cbAnalog.SelectedIndex == 2)
                        {
                            txtMin.Text = HDLPF.IsNumStringMode(temp.Min.ToString(), 0, 100);
                            txtMax.Text = HDLPF.IsNumStringMode(temp.Max.ToString(), 0, 100);
                        }
                        else if (cbAnalog.SelectedIndex == 3)
                        {
                            txtMin.Text = HDLPF.IsNumStringMode(temp.Min.ToString(), 0, 60000);
                            txtMax.Text = HDLPF.IsNumStringMode(temp.Max.ToString(), 0, 60000);
                        }
                    }
                }
                else if (cbInputType.SelectedIndex == 1)
                {
                    txtMin.Text = "0";
                    txtMax.Text = "10000";
                    txtRange1.Visible = true;
                    txtRange2.Visible = true;
                    txtRange1.Text = temp.Range1.ToString();
                    txtRange2.Text = temp.Range2.ToString();
                }
                else if (cbInputType.SelectedIndex == 3)
                {
                    txtMin.Text = "4000";
                    txtMax.Text = "20000";
                    txtRange1.Visible = true;
                    txtRange2.Visible = true;
                    txtRange1.Text = temp.Range1.ToString();
                    txtRange2.Text = temp.Range2.ToString();
                }
                if (cbInputType.SelectedIndex == 2 || cbInputType.SelectedIndex == 4)
                {
                    isLoadCure = true;
                    isChangeSacle = true;
                    if (cbScale.Items.Count > 0)
                    {
                        cbScale.SelectedIndex = cbScale.Items.IndexOf(temp.Count.ToString());
                    }
                    cbRange1.SelectedIndex = temp.Range1;
                    cbRange2.SelectedIndex = temp.Range2;
                }
            }
            catch
            {
            }
            MyBlnReading = false;
        }

        private void txtMin_Leave(object sender, EventArgs e)
        {
            try
            {
                if (MyBlnReading) return;
                if (myDry == null) return;
                if (myDry.myRange == null) return;
                if (strMin == txtMin.Value && strMax == txtMax.Value && !isLoadCure) return;
                isChangeSacle = true;
                Cursor.Current = Cursors.WaitCursor;
                MyBlnReading = true;
                strMin = Convert.ToInt32(txtMin.Value);
                strMax = Convert.ToInt32(txtMax.Value);
                if (strMin == -1) txtMin.Value =txtMin.Minimum;
                if (strMax == -1) txtMax.Value = txtMax.Maximum;

                int Min = Convert.ToInt32(txtMin.Text);
                int Max = Convert.ToInt32(txtMax.Text);
                if (Min > Max)  txtMin.Value = txtMin.Minimum;
            }
            catch
            {
            }
            cbScale_SelectedIndexChanged(null, null);
            MyBlnReading = false;
            isLoadCure = false;
            isChangeSacle = false;
            Cursor.Current = Cursors.Default;
        }

        private void txtMin_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar) || e.KeyChar == 8 || e.KeyChar == 45)
                e.Handled = false;
            else
                e.Handled = true;
        }

        private void cbScale_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbScale.SelectedIndex == -1) cbScale.SelectedIndex = 0;
            try
            {
                strScale = cbScale.Text;
                int Count = Convert.ToInt32(strScale) + 1;

                if (Count > 200)
                {
                    #region
                    isLoadStart = true;
                    cbStart.Items.Clear();
                    lbStart.Visible = true;
                    cbStart.Visible = true;
                    cbStart.Enabled = true;
                    cbStart.Items.Add(cbRange1.Items[0].ToString());
                    int intTmp = 0;
                    for (int i = 0; i < cbRange1.Items.Count; i++)
                    {
                        intTmp++;
                        if (intTmp > 200)
                        {
                            intTmp = 1;
                            cbStart.Items.Add(cbRange1.Items[i].ToString());
                        }
                    }
                    if (stStart != "")
                    {
                        cbStart.SelectedIndex = cbStart.Items.IndexOf(stStart);
                    }
                    else
                    {
                        if (cbStart.Visible && cbStart.Items.Count > 0 && cbStart.SelectedIndex < 0) cbStart.SelectedIndex = 0;
                    }
                    isLoadStart = false;
                    #endregion
                }
                else
                {
                    lbStart.Visible = false;
                    cbStart.Visible = false;
                }
                if (MyBlnReading) return;
                if (!isChangeSacle)
                {
                    int intTmpnum = 0;

                    if (myDry.myRange[cbChannel.SelectedIndex].arayPoint == null ||
                         myDry.myRange[cbChannel.SelectedIndex].arayPoint.Length != Count)
                    {
                        myDry.myRange[cbChannel.SelectedIndex].arayPoint = new int[Count];
                    }

                    int iTmpValue = (int)((txtMax.Value - txtMin.Value) / (Count -1));
                    for (int intI = 0; intI < Count; intI++)
                    {
                        myDry.myRange[cbChannel.SelectedIndex].arayPoint[intI] = intI * iTmpValue;
                    }
                }
                DisplayTableValuesAndDrawing();
                isLoadCure = true;
            }
            catch
            {
            }
            isLoadStart = false;
        }

        private void cbInputType_SelectedIndexChanged(object sender, EventArgs e)
        {
            lbShare.Visible = false;
            cbShare.Visible = false;
            Boolean bIsHasRanges = (cbInputType.SelectedIndex == 2 || cbInputType.SelectedIndex == 4);
            Boolean bIsDryContact = cbInputType.SelectedIndex == 0;

            chbBroadcast.Visible = !bIsDryContact;

            panel14.Enabled = bIsHasRanges;

            lbRange1.Visible = !bIsDryContact;
            lbRange2.Visible = !bIsDryContact;

            lbDryValue.Enabled = bIsDryContact;
            cbAnalog.Enabled = bIsHasRanges;
            cbShare.Enabled = bIsHasRanges;
            txtMin.Enabled = bIsHasRanges;
            txtMax.Enabled = bIsHasRanges;
            cbScale.Enabled = bIsHasRanges;
            cbStart.Enabled = bIsHasRanges;
            
            cbRange1.Visible = bIsHasRanges;
            cbRange2.Visible = bIsHasRanges;
            picList.Visible = bIsHasRanges;
            dgvCurve.Visible = bIsHasRanges;

            txtRange1.Visible = ((cbInputType.SelectedIndex == 1) || (cbInputType.SelectedIndex == 3));
            txtRange2.Visible = ((cbInputType.SelectedIndex == 1) || (cbInputType.SelectedIndex == 3));            

            if (cbInputType.SelectedIndex == 1)
            {
                txtMin.Text = "0";
                txtMax.Text = "10000";
            }
            else if (cbInputType.SelectedIndex == 2)
            {
                cbAnalog_SelectedIndexChanged(null, null);
            }
            else if (cbInputType.SelectedIndex == 3)
            {
                txtMin.Text = "4000";
                txtMax.Text = "20000";
            }
            else if (cbInputType.SelectedIndex == 4)
            {
                cbAnalog_SelectedIndexChanged(null, null);
            }
        }

        private void cbAnalog_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbAnalog.Items.Count > 0 && cbAnalog.SelectedIndex ==-1) cbAnalog.SelectedIndex = 0;
            try
            {
                if (MyBlnReading) return;
                MS04.InputRange temp = myDry.myRange[cbChannel.SelectedIndex];
                // 要不要获取其他的曲线
                #region
                cbShare.Items.Clear();
                bool isAdd = true;
                for (int i = 0; i < myDry.myRange.Count; i++)
                {
                    if (myDry.myRange[i].ID != temp.ID)
                    {
                        if (myDry.myRange[i].InputType == temp.InputType && myDry.myRange[i].AnalogType == temp.AnalogType)
                        {
                            if (isAdd)
                            {
                                cbShare.Items.Add(CsConst.mstrINIDefault.IniReadValue("public", "99573", ""));
                                isAdd = false;
                            }
                            cbShare.Items.Add(myDry.myRange[i].ID.ToString());
                        }
                    }
                }
                if (cbShare.Items.Count > 0)
                {
                    lbShare.Visible = true;
                    cbShare.Visible = true;
                    if (cbShare.SelectedIndex < 0) cbShare.SelectedIndex = 0;
                }
                else
                {
                    lbShare.Visible = false;
                    cbShare.Visible = false;
                }
                #endregion
                //根据类型更新大小范围和限制
                #region
                cbScale.Items.Clear();
                cbRange1.Items.Clear();
                cbRange2.Items.Clear();
                if (cbAnalog.SelectedIndex == 0 || cbAnalog.SelectedIndex == 2)
                {
                    txtMin.Minimum = 0;
                    txtMax.Maximum = 100;
                    for (int i = 1; i <= 100; i++)
                    {
                        
                        cbScale.Items.Add(i.ToString());
                        cbRange1.Items.Add(i.ToString());
                        cbRange2.Items.Add(i.ToString());
                    }
                }
                else if (cbAnalog.SelectedIndex == 1)
                {
                    txtMin.Minimum = 0;
                    txtMax.Maximum = 5000;
                    for (int i = 1; i <= 5000; i++)
                    {
                        cbScale.Items.Add(i.ToString());
                        cbRange1.Items.Add(i.ToString());
                        cbRange2.Items.Add(i.ToString());
                    }
                }
                else if (cbAnalog.SelectedIndex == 3)
                {
                    txtMin.Minimum = 0;
                    txtMax.Maximum = 200;
                    for (int i = 1; i <= 200; i++)
                    {
                        cbScale.Items.Add(i.ToString());
                        cbRange1.Items.Add(i.ToString());
                        cbRange2.Items.Add(i.ToString());
                    }
                }
                #endregion
                isLoadCure = true;
            }
            catch
            {
            }
        }

        private void cboDryO1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (MyBlnReading) return;
            if (myDry == null) return;
            int Tag = Convert.ToInt32((sender as ComboBox).Tag);
            myDry.arayOutput[Tag] = Convert.ToByte((sender as ComboBox).SelectedIndex);
        }

        private void chbList_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            try
            {
                if (MyBlnReading) return;
                if (myDry == null) return;
                if (e.NewValue == CheckState.Checked)
                    myDry.bytChannelStatus = HDLSysPF.SetBit(myDry.bytChannelStatus, e.Index);
                else
                    myDry.bytChannelStatus = HDLSysPF.ClearBit(myDry.bytChannelStatus, e.Index);
            }
            catch
            {
            }
        }

        void DisplayTableValuesAndDrawing()
        {
            if (cbChannel.SelectedIndex == -1) return;
            if (cbInputType.SelectedIndex != 2 && cbInputType.SelectedIndex != 4) return;
            if (cbScale.SelectedIndex == -1) return;

            int iTmpPointsNum = Convert.ToInt32(cbScale.Text.ToString()) + 1;
            String[] strKeyTmp = new string[iTmpPointsNum];  // X 值
            float[] arayValueTmp = new float[iTmpPointsNum]; //Y值

            try
            {
                //准备工作
                #region
                dgvCurve.Columns.Clear();
                int MaxX = 0;
                int MinX = 0;
                int MaxY = Convert.ToInt32(txtMax.Value);
                int MinY = Convert.ToInt32(txtMin.Value);
                int[] TmpPoints = myDry.myRange[cbChannel.SelectedIndex].arayPoint;
                
                if (cbAnalog.SelectedIndex == 0)
                {
                    MaxX = 30000;
                }
                else if (cbAnalog.SelectedIndex == 1 || cbAnalog.SelectedIndex == 3)
                {
                    MaxX = 60000;
                }
                else if (cbAnalog.SelectedIndex == 2)
                {
                    MaxX = 100;
                }

                float fTmpAverage = (MaxX - MinX) / (TmpPoints.Length - 1);
                for (int i = 0; i < iTmpPointsNum; i++)
                {
                    strKeyTmp[i] = (fTmpAverage * i).ToString();  // X
                    arayValueTmp[i] = myDry.myRange[cbChannel.SelectedIndex].arayPoint[i];  // Y

                    DataGridViewTextBoxColumn temp = new DataGridViewTextBoxColumn();
                    temp.HeaderText = strKeyTmp[i].ToString();
                    temp.Name = i.ToString();
                    temp.ReadOnly = true;
                    temp.Width = 100;
                    temp.Tag = 0;
                    temp.SortMode = DataGridViewColumnSortMode.NotSortable;
                    dgvCurve.Columns.Add(temp);
                }
                #endregion

                dgvCurve.Rows.Clear();
              
                object[] obj = new object[TmpPoints.Length];

                for (int i = 0; i < TmpPoints.Length; i++)
                {
                    obj[i] = (TmpPoints[i]).ToString();
                }
                dgvCurve.Rows.Add(obj);
                // 2017 01 16 test
                cuv2D = new Curve2DII(Pic.Width, Pic.Height, strKeyTmp, arayValueTmp, MaxX,MaxY,MinX, MinY );
                String sIntputType = cbInputType.Text.ToString();
                cuv2D.XAxisText = sIntputType.Substring(sIntputType.Length - 3, 2);

                String sType = cbAnalog.Text.ToString();
                cuv2D.YAxisText = sType.Substring(sType.Length - 4, 3).Trim();
                cuv2D.Fit();
                picList.Image = cuv2D.CreateImage();
            }
            catch
            { }
        }

        private void cbStart_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                stStart = cbStart.Text;
                if (isLoadStart) return;
                if (MyBlnReading) return;
                isLoadCure = true;
            }
            catch
            {
            }
        }

        private void cbRange1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cbRange1.SelectedIndex > cbRange2.SelectedIndex) cbRange2.SelectedIndex = cbRange1.SelectedIndex;
                strRange1 = cbRange1.Text;
                strRange2 = cbRange2.Text;
            }
            catch
            {
            }
        }


        private void btnRefStatus_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                for (byte i = 1; i <= 6; i++)
                {
                    byte[] arayTmp = new byte[1];
                    arayTmp[0] = i;
                    if (CsConst.mySends.AddBufToSndList(arayTmp, 0x3524, SubNetID, DeviceID, false, false, true, CsConst.minAllWirelessDeviceType.Contains(mywdDevicerType)) == true)
                    {
                        if (CsConst.myRevBuf[25] == i)
                        {
                            Label tmp1 = this.Controls.Find("lbChannelType" + i.ToString(), true)[0] as Label;
                            Label tmp2 = this.Controls.Find("lbChannelStatus" + i.ToString(), true)[0] as Label;
                            Label tmp3 = this.Controls.Find("lbAnalogType" + i.ToString(), true)[0] as Label;
                            tmp1.Text = CsConst.mstrINIDefault.IniReadValue("public", "0063" + CsConst.myRevBuf[26].ToString(), "");
                            if (CsConst.myRevBuf[26] == 2 || CsConst.myRevBuf[26] == 4)
                            {
                                tmp3.Text = CsConst.mstrINIDefault.IniReadValue("public", "0064" + CsConst.myRevBuf[27].ToString(), "");
                                string str = "";

                                if (CsConst.myRevBuf[27] == 0)
                                {
                                    if (HDLSysPF.GetBit(CsConst.myRevBuf[29], 7) == 1)
                                    {
                                        str = "-" + (CsConst.myRevBuf[28] + HDLSysPF.ClearBit(CsConst.myRevBuf[29], 7) * 256).ToString() + "C";
                                    }
                                    else
                                    {
                                        str = (CsConst.myRevBuf[28] + CsConst.myRevBuf[29] * 256).ToString() + "C";
                                    }
                                }
                                else if (CsConst.myRevBuf[27] == 1)
                                {
                                    str = (CsConst.myRevBuf[28] + CsConst.myRevBuf[29] * 256).ToString() + "Lux";
                                }
                                else if (CsConst.myRevBuf[27] == 2)
                                {
                                    str = (CsConst.myRevBuf[28] + CsConst.myRevBuf[29] * 256).ToString() + "RH";
                                }
                                else if (CsConst.myRevBuf[27] == 3)
                                {
                                    str = (CsConst.myRevBuf[28] + CsConst.myRevBuf[29] * 256).ToString() + "Kpa";
                                }
                                tmp2.Text = str;
                            }
                            else
                            {
                                string str = "";
                                if (CsConst.myRevBuf[26] == 0)
                                {
                                    if (CsConst.myRevBuf[28] == 1) str = CsConst.mstrINIDefault.IniReadValue("public", "99578", "");
                                    else str = CsConst.mstrINIDefault.IniReadValue("public", "99577", "");
                                }
                                else if (CsConst.myRevBuf[26] == 1)
                                {
                                    str = (CsConst.myRevBuf[29] * 256 + CsConst.myRevBuf[28]).ToString() + "(mV)";
                                }
                                else if (CsConst.myRevBuf[26] == 3)
                                {
                                    str = (CsConst.myRevBuf[29] * 256 + CsConst.myRevBuf[28]).ToString() + "(uA)";
                                }
                                tmp2.Text = CsConst.mstrINIDefault.IniReadValue("public", "99568", "") + ":" + str;
                                tmp3.Text = "";
                            }
                        }
                        else
                        {
                            Label tmp1 = this.Controls.Find("lbChannelType" + i.ToString(), true)[0] as Label;
                            Label tmp2 = this.Controls.Find("lbChannelStatus" + i.ToString(), true)[0] as Label;
                            Label tmp3 = this.Controls.Find("lbAnalogType" + i.ToString(), true)[0] as Label;
                            tmp1.Text = "";
                            tmp2.Text = "";
                            tmp3.Text = "";
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

        private void chbUpdata_CheckedChanged(object sender, EventArgs e)
        {
            timerSensor.Enabled = chbUpdata.Checked;
        }

        private void timerSensor_Tick(object sender, EventArgs e)
        {
            btnRefStatus_Click(btnRefStatus, null);
        }

        private void rb1UV0_CheckedChanged(object sender, EventArgs e)
        {
            if (MyBlnReading) return;
            if (myDry == null) return;
            if (myDry.arayUVStaus == null) myDry.arayUVStaus = new byte[36];
            try
            {
                int ID = cbLED.SelectedIndex;
                int Tag = Convert.ToInt32((sender as RadioButton).Tag);
                if (rbON.Checked)
                {
                    if ((sender as RadioButton).Checked)
                        myDry.arayUVStaus[ID * 12 + 6] = Convert.ToByte(Tag);
                }
                else if (rbOFF.Checked)
                {
                    if ((sender as RadioButton).Checked)
                        myDry.arayUVStaus[ID * 12] = Convert.ToByte(Tag);

                }
            }
            catch
            { }
        }

        private void rb2UV0_CheckedChanged(object sender, EventArgs e)
        {
            if (MyBlnReading) return;
            if (myDry == null) return;
            if (myDry.arayUVStaus == null) myDry.arayUVStaus = new byte[36];
            try
            {
                int ID = cbLED.SelectedIndex;
                int Tag = Convert.ToInt32((sender as RadioButton).Tag);
                if (rbON.Checked)
                {
                    if ((sender as RadioButton).Checked)
                        myDry.arayUVStaus[ID * 12 + 6 + 1] = Convert.ToByte(Tag);
                }
                else if (rbOFF.Checked)
                {
                    if ((sender as RadioButton).Checked)
                        myDry.arayUVStaus[ID * 12 + 1] = Convert.ToByte(Tag);

                }
            }
            catch
            { }
        }

        private void rb3UV0_CheckedChanged(object sender, EventArgs e)
        {
            if (MyBlnReading) return;
            if (myDry == null) return;
            if (myDry.arayUVStaus == null) myDry.arayUVStaus = new byte[36];
            try
            {
                int ID = cbLED.SelectedIndex;
                int Tag = Convert.ToInt32((sender as RadioButton).Tag);
                if (rbON.Checked)
                {
                    if ((sender as RadioButton).Checked)
                        myDry.arayUVStaus[ID * 12 + 6 + 2] = Convert.ToByte(Tag);
                }
                else if (rbOFF.Checked)
                {
                    if ((sender as RadioButton).Checked)
                        myDry.arayUVStaus[ID * 12 + 2] = Convert.ToByte(Tag);

                }
            }
            catch
            { }
        }

        private void txtZoneRemark_TextChanged(object sender, EventArgs e)
        {
            if (tvZone.Nodes == null) return;
            if (mixModuleTmp == null) return;
            if (mixModuleTmp.Areas == null) return;
            if (tvZone.SelectedNode == null) return;
            if (MyBlnReading) return;
            try
            {
                TreeNode node = tvZone.SelectedNode;
                if (node.Level == 0)
                {
                    mixModuleTmp.Areas[node.Index].Remark = txtZoneRemark.Text.Trim();
                    node.Text = (node.Index + 1).ToString() + "-" + txtZoneRemark.Text.Trim();
                }
                else if (node.Level == 1)
                {
                    node = node.Parent;
                    mixModuleTmp.Areas[node.Index].Remark = txtZoneRemark.Text.Trim();
                    node.Text = (node.Index + 1).ToString() + "-" + txtZoneRemark.Text.Trim();
                }
            }
            catch
            { }
        }

        private void btnAddZone_Click(object sender, EventArgs e)
        {
            if (mixModuleTmp == null || mixModuleTmp.ChnList == null) return;
            if (tvZone.Nodes.Count >= mixModuleTmp.ChnList.Count) return;
            if (dgvZoneChn1.RowCount <= 0) return;
            try
            {
                int ID = 1;
                if (mixModuleTmp.Areas == null) mixModuleTmp.Areas = new List<MixHotelModuleWithZone.Area>();
                if (mixModuleTmp.Areas.Count > 0)
                {
                    List<int> bTmpAreaIds = new List<int>();
                    foreach (MixHotelModuleWithZone.Area a in mixModuleTmp.Areas)
                    {
                        bTmpAreaIds.Add(a.ID);
                    }
                    while (bTmpAreaIds.Contains(ID))
                    {
                        ID++;
                    }
                }
                else
                {
                    ID = 1;
                }

                MixHotelModuleWithZone.Area temp = new MixHotelModuleWithZone.Area();
                temp.ID = Convert.ToByte(ID);
                temp.Remark = txtZoneRemark.Text;
                temp.Scen = new List<MixHotelModuleWithZone.Scene>();
                for (int j = 0; j <= 12; j++)
                {
                    MixHotelModuleWithZone.Scene oSce = new MixHotelModuleWithZone.Scene();
                    oSce.ID = (byte)j;
                    oSce.Remark = "Scene" + j.ToString();
                    oSce.light = new byte[mixModuleTmp.ChnList.Count].ToList();
                    oSce.Time = 0;
                    temp.Scen.Add(oSce);
                }
                mixModuleTmp.MyRead2UpFlags[5] = true;
                temp.bytDefaultSce = 0;
                mixModuleTmp.Areas.Add(temp);
                tvZone.Nodes.Add(ID.ToString(), (tvZone.Nodes.Count + 1).ToString() + "-" + temp.Remark, 0, 0);
                lbAreaZoneValue.Text = tvZone.Nodes.Count.ToString();
            }
            catch { }
        }

        private void btnDelZone_Click(object sender, EventArgs e)
        {
            if (tvZone.Nodes == null) return;
            if (mixModuleTmp == null) return;
            if (mixModuleTmp.Areas == null) return;
            if (tvZone.SelectedNode == null) return;
            try
            {
                TreeNode node = tvZone.SelectedNode;
                if (node.Level == 0)
                {
                    int intAreaID = tvZone.SelectedNode.Index;
                    for (int i = intAreaID + 1; i < mixModuleTmp.Areas.Count; i++)
                    {
                        for (int j = 0; j < mixModuleTmp.ChnList.Count; j++)
                        {
                            if (mixModuleTmp.ChnList[j].intBelongs == mixModuleTmp.Areas[i].ID)
                            {
                                mixModuleTmp.ChnList[j].intBelongs = Convert.ToByte(mixModuleTmp.Areas[i].ID - 1);
                            }
                        }
                        mixModuleTmp.Areas[i].ID = Convert.ToByte(mixModuleTmp.Areas[i].ID - 1);
                    }
                    mixModuleTmp.Areas.RemoveAt(intAreaID);

                    tvZone.SelectedNode.Remove();

                    for (int i = 0; i < tvZone.Nodes.Count; i++)
                    {
                        string str = tvZone.Nodes[i].Text.Split('-')[1];
                        tvZone.Nodes[i].Text = (i + 1).ToString() + "-" + str;
                    }
                    lbAreaZoneValue.Text = tvZone.Nodes.Count.ToString();
                }
            }
            catch { }
        }

        private void btn1_Click(object sender, EventArgs e)
        {
            if (tvZone.SelectedNode != null && tvZone.SelectedNode.Level == 0)
            {
                foreach (DataGridViewRow Tmp in dgvZoneChn1.SelectedRows)
                {
                    if (Tmp.Selected)  // 确定被选中，添加至区域列表
                    {
                        int intLoadID = int.Parse(Tmp.Cells[0].Value.ToString());
                        mixModuleTmp.ChnList[intLoadID - 1].intBelongs = Convert.ToByte(tvZone.SelectedNode.Name.ToString());
                        tvZone.SelectedNode.Nodes.Insert(0, null, Tmp.Cells[0].Value.ToString() + "-" + mixModuleTmp.ChnList[intLoadID - 1].Remark, 1, 1);
                        dgvZoneChn1.Rows.Remove(Tmp);
                    }
                }
            }
            else if (tvZone.SelectedNode != null && tvZone.SelectedNode.Level == 1)
            {
                foreach (DataGridViewRow Tmp in dgvZoneChn1.SelectedRows)
                {
                    if (Tmp.Selected)  // 确定被选中，添加至区域列表
                    {
                        int intLoadID = int.Parse(Tmp.Cells[0].Value.ToString());
                        mixModuleTmp.ChnList[intLoadID - 1].intBelongs = Convert.ToByte(tvZone.SelectedNode.Parent.Name.ToString());
                        tvZone.SelectedNode.Parent.Nodes.Insert(0, null, Tmp.Cells[0].Value.ToString() + "-" + mixModuleTmp.ChnList[intLoadID - 1].Remark, 1, 1);
                        dgvZoneChn1.Rows.Remove(Tmp);
                    }
                }
            }
            lbChnZoneValue.Text = dgvZoneChn1.Rows.Count.ToString();
        }

        private void btn2_Click(object sender, EventArgs e)
        {
            if (tvZone.Nodes == null) return;
            if (mixModuleTmp== null) return;
            if (mixModuleTmp.Areas == null) return;
            if (tvZone.SelectedNode == null) return;
            TreeNode node = tvZone.SelectedNode;
            if (node.Level == 1)
            {
                DeleteNodeInAreaForm(node.Text);
                if (tvZone.SelectedNode.Parent.Nodes.Count == 1)
                {
                    DialogResult result = MessageBox.Show("Do you want to remove the blank area as well?", "Warm Hint"
                                                , MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                    if (result == DialogResult.OK)
                    {
                        int intAreaID = tvZone.SelectedNode.Parent.Index;
                        mixModuleTmp.Areas.RemoveAt(intAreaID);
                        tvZone.SelectedNode.Parent.Remove();
                    }
                }
                else tvZone.SelectedNode.Remove();
            }
            else if (node.Level == 0)
            {
                foreach (TreeNode Tmp in tvZone.SelectedNode.Nodes)
                {
                    DeleteNodeInAreaForm(Tmp.Text);
                }
                tvZone.SelectedNode.Nodes.Clear();
            }
            lbChnZoneValue.Text = dgvZoneChn1.Rows.Count.ToString();
        }

        void DeleteNodeInAreaForm(string strName)
        {
            int intIndex = int.Parse(strName.Split('-')[0].ToString());
            mixModuleTmp.ChnList[intIndex - 1].intBelongs = 0;
            resetChnsWaittingAllocation();
        }

        private void tvZone_MouseDown(object sender, MouseEventArgs e)
        {
            TreeNode oNode = tvZone.GetNodeAt(e.X, e.Y);
            if (oNode == null) return;
            MyBlnReading = true;
            if (oNode.Level == 1) oNode = oNode.Parent;
            txtZoneRemark.Text = oNode.Text.Split('-')[1].ToString();
            MyBlnReading = false;
        }

        private void tvScene_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (tvScene.SelectedNode == null) return;
            ShowScenenChannelInformation(tvScene.SelectedNode);
        }

        void ShowScenenChannelInformation(TreeNode oNode)
        {
            if (oNode == null) return;
            if (mixModuleTmp.Areas == null) return;
            if (oNode.Level == 0) return;
            if (MyBlnReading) return;
            try
            {
                currentAreaId = (Byte)oNode.Parent.Index;
                currentSceneId = (Byte)oNode.Index;

                HDLSysPF.ListAllSceneNameToCombobox(mixModuleTmp, currentAreaId, cbRestoreList, true);
                tbSceneName.Text = mixModuleTmp.Areas[currentAreaId].Scen[currentSceneId].Remark;
                cbRestoreList.SelectedIndex = mixModuleTmp.Areas[currentAreaId].bytDefaultSce;
                tbRunningTime.Text = Convert.ToString(mixModuleTmp.Areas[currentAreaId].Scen[currentSceneId].Time);

                showSenceInfo(currentAreaId, currentSceneId);
                MyBlnReading = false;
            }
            catch { }
        }

        private void showSenceInfo(Byte AreaID, Byte SceneID)
        {
            if (mixModuleTmp == null) return;
            if (mixModuleTmp.Areas == null) return;

            if (mixModuleTmp.Areas.Count < AreaID + 1) return;

            try
            {
                dgScene.Rows.Clear();
                MixHotelModuleWithZone.Area oArea = mixModuleTmp.Areas[AreaID];

                if (oArea.Scen == null || oArea.Scen.Count == 0) return;
                if (oArea.Scen.Count < SceneID + 1) return;

                MixHotelModuleWithZone.Scene oSce = oArea.Scen[SceneID];
                if (SceneID == 0) oSce.light = (new Byte[mixModuleTmp.ChnList.Count]).ToList();

                for (int i = 0; i < mixModuleTmp.ChnList.Count; i++)
                {
                    if (mixModuleTmp.ChnList[i].intBelongs == oArea.ID)
                    {
                        if (oSce.light[i] > 100) oSce.light[i] = 100;
                        Object[] obj = new Object[] { mixModuleTmp.ChnList[i].ID, mixModuleTmp.ChnList[i].Remark, cboStatus.Items[oSce.light[i] / 100] };
                        dgScene.Rows.Add(obj);
                    }
                }
            }
            catch { }

        }

        private void tbSceneName_TextChanged(object sender, EventArgs e)
        {
            if (MyBlnReading) return;
            if (tvScene.SelectedNode == null) return;
            if (tvScene.SelectedNode.Level == 0) return;
            TreeNode oNode = tvScene.SelectedNode;

            oNode.Text = (oNode.Index).ToString() + "-" + tbSceneName.Text;
            mixModuleTmp.Areas[currentAreaId].Scen[currentSceneId].Remark = tbSceneName.Text.Trim();
        }

        private void cbRestoreList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (MyBlnReading) return;
            mixModuleTmp.Areas[currentAreaId].bytDefaultSce = Convert.ToByte(cbRestoreList.SelectedIndex);
        }

        private void tbRunningTime_TextChanged(object sender, EventArgs e)
        {
            if (MyBlnReading) return;
            mixModuleTmp.Areas[currentAreaId].Scen[currentSceneId].Time = Convert.ToInt32(tbRunningTime.Text);
        }

        private void chbSyn_CheckedChanged(object sender, EventArgs e)
        {
            if (MyBlnReading) return;
            if (chbSyn.Checked == true)
            {
                for (int i = 0; i < mixModuleTmp.Areas[currentAreaId].Scen.Count; i++)
                {
                    mixModuleTmp.Areas[currentAreaId].Scen[i].Time = Convert.ToInt32(tbRunningTime.Text);
                }
                chbSyn.Checked = false;
                MessageBox.Show("SYN Running Done!", "Hint");
            }
        }

        private void dgScene_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (tvScene.SelectedNode == null) return;
            if (e.RowIndex == -1) return;
            if (e.ColumnIndex == -1) return;
            if (dgScene.RowCount == 0) return;

            string Status = dgScene[2, e.RowIndex].Value.ToString();
            for (int i = 0; i < dgScene.SelectedRows.Count; i++)
            {
                if (e.ColumnIndex == 2)
                {
                    dgScene[2, dgScene.SelectedRows[i].Index].Value = Status;
                    Byte currentIntensity = Convert.ToByte(cboStatus.Items.IndexOf(Status));
                    Byte ChnID = Convert.ToByte(dgScene[0, e.RowIndex].Value.ToString());
                    mixModuleTmp.Areas[currentAreaId].Scen[currentSceneId].light[ChnID - 1] = (Byte)(currentIntensity * 100);
                }
            }
        }
    }
}
