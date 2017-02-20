using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Linq;


namespace HDL_Buspro_Setup_Tool
{
    public partial class FrmDownloadShow : Form
    {
        public FrmDownloadShow()
        {
            InitializeComponent();
        }

        public void SetProgressValue(int value)
        {
            this.probStatus.Value = value;
            this.lblProgressValue.Text = CsConst.mstrINIDefault.IniReadValue("public", "99596", "") + value.ToString() + "%";

            // 这里关闭，比较好，呵呵！
            btnStop.Enabled = !(value == this.probStatus.Maximum);
            if (value == this.probStatus.Maximum) this.Close();
        }



        private void FrmDownloadShow_Load(object sender, EventArgs e)
        {
            if (CsConst.MyUpload2Down == 0 || CsConst.MyUpload2Down == 2) this.Text = CsConst.mstrINIDefault.IniReadValue("public", "99595", "");
            else this.Text = CsConst.mstrINIDefault.IniReadValue("public", "99594", "");
            //chbExit.Checked = (CsConst.MyUPload2DownLists == null || (CsConst.MyUPload2DownLists != null && CsConst.MyUPload2DownLists.Count == 1));
            ReadThread();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy)
            {
                //CsConst.calculationWorker.CancelAsync();
                CsConst.calculationWorker.Dispose();
            }
            this.Close();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy)
            {
                //CsConst.calculationWorker.CancelAsync();
                CsConst.calculationWorker.Dispose();
                btnStop.Enabled = false;
            }
            btnRestart.Enabled = !(btnStop.Enabled);
        }

        private void ReadThread()
        {
            if (!this.IsDisposed)
            {
                Cursor.Current = Cursors.WaitCursor;

                CsConst.calculationWorker = new BackgroundWorker();
                CsConst.calculationWorker.DoWork += new DoWorkEventHandler(calculationWorker_DoWork);
                CsConst.calculationWorker.ProgressChanged += new ProgressChangedEventHandler(calculationWorker_ProgressChanged);
                CsConst.calculationWorker.WorkerReportsProgress = true;
                CsConst.calculationWorker.WorkerSupportsCancellation = true;
                CsConst.calculationWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(calculationWorker_RunWorkerCompleted);
                CsConst.calculationWorker.RunWorkerAsync();
                Cursor.Current = Cursors.Default;
            }
        }

        void calculationWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Cursor.Current = Cursors.Default;
            this.Close();
            //throw new NotImplementedException();
        }

        void calculationWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            try
            {
                //throw new NotImplementedException();
                if (e.UserState != null) this.lbName.Text = e.UserState.ToString();
                this.probStatus.Value = e.ProgressPercentage;
                this.lblProgressValue.Text = CsConst.mstrINIDefault.IniReadValue("public", "99596", "") + e.ProgressPercentage.ToString() + "%";
                btnStop.Enabled = !(e.ProgressPercentage == this.probStatus.Maximum);
                if (e.ProgressPercentage == 100)
                {
                    CsConst.isBackUpSucceed = true;
                }
                if (e.ProgressPercentage == this.probStatus.Maximum)
                {
                    //test
                    //CsConst.calculationWorker.CancelAsync();
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
            catch
            {
            }
        }

        void calculationWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if (CsConst.MyUPload2DownLists != null)
                {
                    foreach (byte[] ArayTmp in CsConst.MyUPload2DownLists)
                    {
                        //查找到设备

                        string strDeviceName = HDLSysPF.GetRemarkAccordingly(ArayTmp[0], ArayTmp[1], ArayTmp[5] * 256 + ArayTmp[6]);
                        // 开始上传下载
                        CsConst.calculationWorker.ReportProgress(0, strDeviceName);
                        int dIndex = 0;

                        if (ArayTmp.Length > 5) dIndex = ArayTmp[5] * 256 + ArayTmp[6];
                        int num1 = 1;
                        int num2 = 1;
                        int num3 = 1;
                        if (ArayTmp.Length >= 9)
                        {
                            num1 = Convert.ToInt32(ArayTmp[7]);
                            num2 = Convert.ToInt32(ArayTmp[8]);
                        }
                        if (ArayTmp.Length >= 10)
                        {
                            num1 = Convert.ToInt32(ArayTmp[7]);
                            num2 = Convert.ToInt32(ArayTmp[8]);
                            num3 = Convert.ToInt32(ArayTmp[9]);
                        }
                        DownloadOrUploadToDevices(ArayTmp[2] * 256 + ArayTmp[3], strDeviceName, ArayTmp[4], dIndex, num1, num2, num3);
                    }
                }
            }
            catch { }
        }

        public static void DownloadOrUploadToDevices(int wdDeviceType, string strName, int intActivePage, int dIndex, int num1, int num2, int num3)
        {
            int wdMaxValue = DeviceTypeList.GetMaxValueFromPublicModeGroup(wdDeviceType);
            try
            {
                if (IPmoduleDeviceTypeList.HDLIPModuleDeviceTypeLists.Contains(wdDeviceType)) // 是不是一端口交换机
                {
                    #region
                    IPModule TmpIP = null;
                    if (CsConst.myIPs != null)
                    {
                        foreach (IPModule Tmp in CsConst.myIPs)
                        {
                            if (Tmp.DIndex == dIndex)
                            {
                                TmpIP = Tmp;
                                break;
                            }
                        }
                    }

                    if (CsConst.myIPs == null || TmpIP == null)
                    {
                        TmpIP = new IPModule();
                        TmpIP.DeviceName = strName;
                        TmpIP.DIndex = dIndex;
                        if (CsConst.myIPs == null) CsConst.myIPs = new List<IPModule>(); CsConst.myIPs.Add(TmpIP);
                    }
                    if (CsConst.MyUpload2Down == 0)
                    {
                        TmpIP.DownloadZaudioFrmDeviceToBuf(TmpIP.DeviceName, intActivePage, wdDeviceType);
                    }
                    else if (CsConst.MyUpload2Down == 1) TmpIP.UploadCurtainInfosToDevice(TmpIP.DeviceName, intActivePage, wdDeviceType);

                    if (CsConst.isRestore)
                    {
                        TmpIP.remarkBackup = strName.Split('\\')[1].Trim();
                        String TmpDevName = strName.Split('\\')[0].Trim();

                        TmpIP.iIndexBackup = dIndex;
                        TmpIP.DeviceTypeBackup = wdDeviceType;
                        TmpIP.subnetIDBackup = Byte.Parse(TmpDevName.Split('-')[0].ToString());
                        TmpIP.deviceIDBackup = Byte.Parse(TmpDevName.Split('-')[1].ToString());

                        CsConst.myBackupLists.Add(TmpIP);
                    }
                    #endregion
                }
                else if (DimmerDeviceTypeList.HDLDimmerDeviceTypeList.Contains(wdDeviceType)) //是不是调光器
                {
                    #region
                    Dimmer TmpIP = null;
                    if (CsConst.myDimmers != null)
                    {
                        foreach (Dimmer Tmp in CsConst.myDimmers)
                        {
                            if (Tmp.DIndex == dIndex)
                            {
                                TmpIP = Tmp;
                                break;
                            }
                        }
                    }
                    if (CsConst.myDimmers == null || TmpIP == null)
                    {
                        TmpIP = new Dimmer();
                        TmpIP.DeviceName = strName;
                        TmpIP.DIndex = dIndex;
                        if (CsConst.myDimmers == null) CsConst.myDimmers = new List<Dimmer>(); CsConst.myDimmers.Add(TmpIP);
                    }
                    if (CsConst.MyUpload2Down == 0) TmpIP.DownloadDimmerInfosToBuffer(TmpIP.DeviceName, intActivePage, wdDeviceType);
                    else if (CsConst.MyUpload2Down == 1) TmpIP.UploadDimmerInfosToDevice(TmpIP.DeviceName, intActivePage, wdDeviceType);

                    if (CsConst.isRestore)
                    {
                        TmpIP.remarkBackup = strName.Split('\\')[1].Trim();
                        String TmpDevName = strName.Split('\\')[0].Trim();
                        TmpIP.iIndexBackup = dIndex;
                        TmpIP.DeviceTypeBackup = wdDeviceType;
                        TmpIP.subnetIDBackup = Byte.Parse(TmpDevName.Split('-')[0].ToString());
                        TmpIP.deviceIDBackup = Byte.Parse(TmpDevName.Split('-')[1].ToString());

                        CsConst.myBackupLists.Add(TmpIP);
                    }
                    #endregion
                }
                else if (RelayDeviceTypeList.HDLRelayDeviceTypeList.Contains(wdDeviceType)) // 是不是继电器模块
                {
                    #region
                    Relay TmpRelay = null;
                    if (CsConst.myRelays != null)
                    {
                        foreach (Relay Tmp in CsConst.myRelays)
                        {
                            if (Tmp.DIndex == dIndex)
                            {
                                TmpRelay = Tmp;
                                break;
                            }
                        }
                    }
                    if (CsConst.myRelays == null || TmpRelay == null)
                    {
                        TmpRelay = new Relay();
                        TmpRelay.DeviceName = strName;
                        TmpRelay.DIndex = dIndex;
                        if (CsConst.myRelays == null) CsConst.myRelays = new List<Relay>(); CsConst.myRelays.Add(TmpRelay);
                    }
                    if (CsConst.MyUpload2Down == 0) TmpRelay.DownloadRelayInfosToBuffer(TmpRelay.DeviceName, intActivePage, wdDeviceType);
                    else if (CsConst.MyUpload2Down == 1) TmpRelay.UploadRelayInfosToDevice(TmpRelay.DeviceName, intActivePage, wdDeviceType);

                    if (CsConst.isRestore)
                    {
                        TmpRelay.remarkBackup = strName.Split('\\')[1].Trim();
                        String TmpDevName = strName.Split('\\')[0].Trim();
                        TmpRelay.iIndexBackup = dIndex;
                        TmpRelay.DeviceTypeBackup = wdDeviceType;
                        TmpRelay.subnetIDBackup = Byte.Parse(TmpDevName.Split('-')[0].ToString());
                        TmpRelay.deviceIDBackup = Byte.Parse(TmpDevName.Split('-')[1].ToString());

                        CsConst.myBackupLists.Add(TmpRelay);
                    }
                    #endregion
                }
                else if (EnviroNewDeviceTypeList.EnviroNewPanelDeviceTypeList.Contains(wdDeviceType)) // 是不是新版彩屏面板
                {
                    #region
                    EnviroPanel TmpRelay = null;
                    if (CsConst.myEnviroPanels != null)
                    {
                        foreach (EnviroPanel Tmp in CsConst.myEnviroPanels)
                        {
                            if (Tmp.DIndex == dIndex)
                            {
                                TmpRelay = Tmp;
                                break;
                            }
                        }
                    }
                    if (CsConst.myEnviroPanels == null || TmpRelay == null)
                    {
                        TmpRelay = new EnviroPanel();
                        TmpRelay.DeviceName = strName;
                        TmpRelay.DIndex = dIndex;
                        if (CsConst.myEnviroPanels == null) CsConst.myEnviroPanels = new List<EnviroPanel>(); CsConst.myEnviroPanels.Add(TmpRelay);
                    }
                    if (CsConst.MyUpload2Down == 0) TmpRelay.DownloadColorDLPInfoToDevice(wdDeviceType, TmpRelay.DeviceName, intActivePage, 0, 0);
                    else if (CsConst.MyUpload2Down == 1) TmpRelay.UploadColorDLPInfoToDevice(wdDeviceType, TmpRelay.DeviceName, intActivePage, 0, 0);

                    if (CsConst.isRestore)
                    {
                        TmpRelay.remarkBackup = strName.Split('\\')[1].Trim();
                        String TmpDevName = strName.Split('\\')[0].Trim();
                        TmpRelay.iIndexBackup = dIndex;
                        TmpRelay.DeviceTypeBackup = wdDeviceType;
                        TmpRelay.subnetIDBackup = Byte.Parse(TmpDevName.Split('-')[0].ToString());
                        TmpRelay.deviceIDBackup = Byte.Parse(TmpDevName.Split('-')[1].ToString());

                        CsConst.myBackupLists.Add(TmpRelay);
                    }
                    #endregion
                }
                else if (EnviroDLPPanelDeviceTypeList.HDLEnviroDLPPanelDeviceTypeList.Contains(wdDeviceType)) // 是不是彩屏面板
                {
                    #region
                    ColorDLP TmpRelay = null;
                    if (CsConst.myColorPanels != null)
                    {
                        foreach (ColorDLP Tmp in CsConst.myColorPanels)
                        {
                            if (Tmp.DIndex == dIndex)
                            {
                                TmpRelay = Tmp;
                                break;
                            }
                        }
                    }
                    if (CsConst.myColorPanels == null || TmpRelay == null)
                    {
                        TmpRelay = new ColorDLP();
                        TmpRelay.DeviceName = strName;
                        TmpRelay.DIndex = dIndex;
                        if (CsConst.myColorPanels == null) CsConst.myColorPanels = new List<ColorDLP>(); CsConst.myColorPanels.Add(TmpRelay);
                    }
                    if (CsConst.MyUpload2Down == 0) TmpRelay.DownloadColorDLPInfoToDevice(wdDeviceType, TmpRelay.DeviceName, intActivePage);
                    else if (CsConst.MyUpload2Down == 1) TmpRelay.UploadColorDLPInfoToDevice(wdDeviceType, TmpRelay.DeviceName, intActivePage);

                    if (CsConst.isRestore)
                    {
                        TmpRelay.remarkBackup = strName.Split('\\')[1].Trim();
                        String TmpDevName = strName.Split('\\')[0].Trim();
                        TmpRelay.iIndexBackup = dIndex;
                        TmpRelay.DeviceTypeBackup = wdDeviceType;
                        TmpRelay.subnetIDBackup = Byte.Parse(TmpDevName.Split('-')[0].ToString());
                        TmpRelay.deviceIDBackup = Byte.Parse(TmpDevName.Split('-')[1].ToString());

                        CsConst.myBackupLists.Add(TmpRelay);
                    }
                    #endregion
                }
                else if (DLPPanelDeviceTypeList.HDLDLPPanelDeviceTypeList.Contains(wdDeviceType)) // 是不是DLP面板
                {
                    #region
                    DLP TmpRelay = null;
                    if (CsConst.myPanels != null)
                    {
                        List<DLP> TmpDLP = new List<DLP>();
                        //判断类型是不是DLP
                        foreach (Panel Tmp in CsConst.myPanels)
                        {
                            if (Tmp is DLP)
                            {
                                TmpDLP.Add((DLP)Tmp);
                            }
                        }
                        foreach (DLP Tmp in TmpDLP)
                        {
                            if (Tmp.DIndex == dIndex)
                            {
                                TmpRelay = Tmp;
                                break;
                            }
                        }
                    }

                    if (CsConst.myPanels == null || TmpRelay == null)
                    {
                        TmpRelay = new DLP();
                        TmpRelay.DeviceName = strName;
                        TmpRelay.DIndex = dIndex;
                        if (CsConst.myPanels == null) CsConst.myPanels = new List<Panel>(); CsConst.myPanels.Add(TmpRelay);
                    }

                    if (CsConst.MyUpload2Down == 0) TmpRelay.DownLoadInformationFrmDevice(TmpRelay.DeviceName, intActivePage, wdDeviceType, num1, num2);
                    else if (CsConst.MyUpload2Down == 1) TmpRelay.UploadPanelInfosToDevice(TmpRelay.DeviceName, intActivePage, wdDeviceType);

                    if (CsConst.isRestore)
                    {
                        TmpRelay.remarkBackup = strName.Split('\\')[1].Trim();
                        String TmpDevName = strName.Split('\\')[0].Trim();
                        TmpRelay.iIndexBackup = dIndex;
                        TmpRelay.DeviceTypeBackup = wdDeviceType;
                        TmpRelay.subnetIDBackup = Byte.Parse(TmpDevName.Split('-')[0].ToString());
                        TmpRelay.deviceIDBackup = Byte.Parse(TmpDevName.Split('-')[1].ToString());

                        CsConst.myBackupLists.Add(TmpRelay);
                    }
                    #endregion
                }
                else if (NormalPanelDeviceTypeList.HDLNormalPanelDeviceTypeList.Contains(wdDeviceType)) // 是不是面板
                {
                    #region
                    Panel TmpRelay = null;
                    if (CsConst.myPanels != null)
                    {
                        foreach (Panel Tmp in CsConst.myPanels)
                        {
                            if (Tmp.DIndex == dIndex)
                            {
                                TmpRelay = Tmp;
                                break;
                            }
                        }
                    }
                    if (CsConst.myPanels == null || TmpRelay == null)
                    {
                        TmpRelay = new Panel();
                        TmpRelay.DeviceName = strName;
                        TmpRelay.DIndex = dIndex;
                        if (CsConst.myPanels == null) CsConst.myPanels = new List<Panel>(); CsConst.myPanels.Add(TmpRelay);
                    }
                    if (CsConst.MyUpload2Down == 0) TmpRelay.DownLoadInformationFrmDevice(TmpRelay.DeviceName, intActivePage, wdDeviceType, num1, num2);
                    else if (CsConst.MyUpload2Down == 1) TmpRelay.UploadPanelInfosToDevice(TmpRelay.DeviceName, intActivePage, wdDeviceType);

                    if (CsConst.isRestore)
                    {
                        TmpRelay.remarkBackup = strName.Split('\\')[1].Trim();
                        String TmpDevName = strName.Split('\\')[0].Trim();
                        TmpRelay.iIndexBackup = dIndex;
                        TmpRelay.DeviceTypeBackup = wdDeviceType;
                        TmpRelay.subnetIDBackup = Byte.Parse(TmpDevName.Split('-')[0].ToString());
                        TmpRelay.deviceIDBackup = Byte.Parse(TmpDevName.Split('-')[1].ToString());

                        CsConst.myBackupLists.Add(TmpRelay);
                    }
                    #endregion
                }
                else if (CoolMasterDeviceTypeList.HDLCoolMasterDeviceTypeList.Contains(wdDeviceType)) // 是不是coolmaster
                {
                    #region
                    CoolMaster TmpRelay = null;
                    if (CsConst.myCoolMasters != null)
                    {
                        foreach (CoolMaster Tmp in CsConst.myCoolMasters)
                        {
                            if (Tmp.DIndex == dIndex)
                            {
                                TmpRelay = Tmp;
                                break;
                            }
                        }
                    }

                    if (CsConst.myPanels == null || TmpRelay == null)
                    {
                        TmpRelay = new CoolMaster();
                        TmpRelay.DeviceName = strName;
                        TmpRelay.DIndex = dIndex;
                        if (CsConst.myCoolMasters == null) CsConst.myCoolMasters = new List<CoolMaster>(); CsConst.myCoolMasters.Add(TmpRelay);
                    }
                    if (CsConst.MyUpload2Down == 0) TmpRelay.DownLoadInfoFrmDevice(TmpRelay.DeviceName, wdDeviceType, intActivePage, num1, num2);
                    else if (CsConst.MyUpload2Down == 1) TmpRelay.UploadInfosToDevice(TmpRelay.DeviceName, wdDeviceType, intActivePage);

                    if (CsConst.isRestore)
                    {
                        TmpRelay.remarkBackup = strName.Split('\\')[1].Trim();
                        String TmpDevName = strName.Split('\\')[0].Trim();
                        TmpRelay.iIndexBackup = dIndex;
                        TmpRelay.DeviceTypeBackup = wdDeviceType;
                        TmpRelay.subnetIDBackup = Byte.Parse(TmpDevName.Split('-')[0].ToString());
                        TmpRelay.deviceIDBackup = Byte.Parse(TmpDevName.Split('-')[1].ToString());

                        CsConst.myBackupLists.Add(TmpRelay);
                    }
                    #endregion
                }
                else if (MS04DeviceTypeList.MS04HotelMixModuleHasArea.Contains(wdDeviceType)) // 判断是不是干接点类型的混合模块
                {
                    #region
                    MixHotelModuleWithZone OIP = null;
                    if (CsConst.myDrys != null)
                    {
                        foreach (MS04 Tmp in CsConst.myDrys)
                        {
                            if (Tmp.DIndex == dIndex)
                            {
                                OIP = (MixHotelModuleWithZone)Tmp;
                                break;
                            }
                        }
                    }
                    if (CsConst.myDrys == null || OIP == null)
                    {
                        OIP = new MixHotelModuleWithZone();
                        OIP.DeviceName = strName;
                        OIP.DIndex = dIndex;
                        OIP.ReadDefaultInfo(wdDeviceType);
                        if (CsConst.myDrys == null) CsConst.myDrys = new List<MS04>(); CsConst.myDrys.Add(OIP);
                    }

                    if (CsConst.MyUpload2Down == 0) OIP.DownLoadInformationFrmDevice(OIP.DeviceName, wdDeviceType, intActivePage, 0, 0);
                    if (CsConst.MyUpload2Down == 1) OIP.UploadMS04ToDevice(OIP.DeviceName, wdDeviceType, intActivePage, 0, 0);

                    if (CsConst.isRestore)
                    {
                        OIP.remarkBackup = strName.Split('\\')[1].Trim();
                        String TmpDevName = strName.Split('\\')[0].Trim();
                        OIP.iIndexBackup = dIndex;
                        OIP.DeviceTypeBackup = wdDeviceType;
                        OIP.subnetIDBackup = Byte.Parse(TmpDevName.Split('-')[0].ToString());
                        OIP.deviceIDBackup = Byte.Parse(TmpDevName.Split('-')[1].ToString());

                        CsConst.myBackupLists.Add(OIP);
                    }
                    #endregion
                }
                else if (MS04DeviceTypeList.WirelessMS04WithOneCurtain.Contains(wdDeviceType)) // 判断是不是干接点类型的设备窗帘
                {
                    #region
                    MS04GenerationOneCurtain OIP = null;
                    if (CsConst.myDrys != null)
                    {
                        foreach (MS04 Tmp in CsConst.myDrys)
                        {
                            if (Tmp.DIndex == dIndex)
                            {
                                OIP = (MS04GenerationOneCurtain)Tmp;
                                break;
                            }
                        }
                    }
                    if (CsConst.myDrys == null || OIP == null)
                    {
                        OIP = new MS04GenerationOneCurtain();
                        OIP.DeviceName = strName;
                        OIP.DIndex = dIndex;
                        OIP.ReadDefaultInfo(wdDeviceType);
                        if (CsConst.myDrys == null) CsConst.myDrys = new List<MS04>(); CsConst.myDrys.Add(OIP);
                    }

                    if (CsConst.MyUpload2Down == 0) OIP.DownLoadInformationFrmDevice(OIP.DeviceName, wdDeviceType, intActivePage, 0, 0);
                    if (CsConst.MyUpload2Down == 1) OIP.UploadMS04ToDevice(OIP.DeviceName, wdDeviceType, intActivePage, 0, 0);

                    if (CsConst.isRestore)
                    {
                        OIP.remarkBackup = strName.Split('\\')[1].Trim();
                        String TmpDevName = strName.Split('\\')[0].Trim();
                        OIP.iIndexBackup = dIndex;
                        OIP.DeviceTypeBackup = wdDeviceType;
                        OIP.subnetIDBackup = Byte.Parse(TmpDevName.Split('-')[0].ToString());
                        OIP.deviceIDBackup = Byte.Parse(TmpDevName.Split('-')[1].ToString());

                        CsConst.myBackupLists.Add(OIP);
                    }
                    #endregion
                }
                else if (MS04DeviceTypeList.WirelessMS04WithRelayChn.Contains(wdDeviceType)) // 判断是不是干接点类型的设备继电器
                {
                    #region
                    MS04GenerationOne2R OIP = null;
                    if (CsConst.myDrys != null)
                    {
                        foreach (MS04 Tmp in CsConst.myDrys)
                        {
                            if (Tmp.DIndex == dIndex)
                            {
                                OIP = (MS04GenerationOne2R)Tmp;
                                break;
                            }
                        }
                    }
                    if (CsConst.myDrys == null || OIP == null)
                    {
                        OIP = new MS04GenerationOne2R();
                        OIP.DeviceName = strName;
                        OIP.DIndex = dIndex;
                        OIP.ReadDefaultInfo(wdDeviceType);
                        if (CsConst.myDrys == null) CsConst.myDrys = new List<MS04>(); CsConst.myDrys.Add(OIP);
                    }

                    if (CsConst.MyUpload2Down == 0) OIP.DownLoadInformationFrmDevice(OIP.DeviceName, wdDeviceType, intActivePage, 0, 0);
                    else if (CsConst.MyUpload2Down == 1) OIP.UploadMS04ToDevice(OIP.DeviceName, wdDeviceType, intActivePage, 0, 0);

                    if (CsConst.isRestore)
                    {
                        OIP.remarkBackup = strName.Split('\\')[1].Trim();
                        String TmpDevName = strName.Split('\\')[0].Trim();
                        OIP.iIndexBackup = dIndex;
                        OIP.DeviceTypeBackup = wdDeviceType;
                        OIP.subnetIDBackup = Byte.Parse(TmpDevName.Split('-')[0].ToString());
                        OIP.deviceIDBackup = Byte.Parse(TmpDevName.Split('-')[1].ToString());

                        CsConst.myBackupLists.Add(OIP);
                    }
                    #endregion
                }
                else if (MS04DeviceTypeList.WirelessMS04WithDimmerChn.Contains(wdDeviceType)) // 判断是不是干接点类型的设备调光器
                {
                    #region
                    MS04GenerationOneD OIP = null;
                    if (CsConst.myDrys != null)
                    {
                        foreach (MS04 Tmp in CsConst.myDrys)
                        {
                            if (Tmp.DIndex == dIndex)
                            {
                                OIP = (MS04GenerationOneD)Tmp;
                                break;
                            }
                        }
                    }
                    if (CsConst.myDrys == null || OIP == null)
                    {
                        OIP = new MS04GenerationOneD();
                        OIP.DeviceName = strName;
                        OIP.DIndex = dIndex;
                        OIP.ReadDefaultInfo(wdDeviceType);
                        if (CsConst.myDrys == null) CsConst.myDrys = new List<MS04>(); CsConst.myDrys.Add(OIP);
                    }

                    if (CsConst.MyUpload2Down == 0) OIP.DownLoadInformationFrmDevice(OIP.DeviceName, wdDeviceType, intActivePage, 0, 0);
                    else if (CsConst.MyUpload2Down == 1) OIP.UploadMS04ToDevice(OIP.DeviceName, wdDeviceType, intActivePage, 0, 0);

                    if (CsConst.isRestore)
                    {
                        OIP.remarkBackup = strName.Split('\\')[1].Trim();
                        String TmpDevName = strName.Split('\\')[0].Trim();
                        OIP.iIndexBackup = dIndex;
                        OIP.DeviceTypeBackup = wdDeviceType;
                        OIP.subnetIDBackup = Byte.Parse(TmpDevName.Split('-')[0].ToString());
                        OIP.deviceIDBackup = Byte.Parse(TmpDevName.Split('-')[1].ToString());

                        CsConst.myBackupLists.Add(OIP);
                    }
                    #endregion
                }
                else if (MS04DeviceTypeList.MS04IModuleWithTemperature.Contains(wdDeviceType)) // 判断是不是干接点类型的设备温度探头
                {
                    #region
                    MS04GenerationOneT OIP = null;
                    if (CsConst.myDrys != null)
                    {
                        foreach (MS04 Tmp in CsConst.myDrys)
                        {
                            if (Tmp.DIndex == dIndex)
                            {
                                OIP = (MS04GenerationOneT)Tmp;
                                break;
                            }
                        }
                    }
                    if (CsConst.myDrys == null || OIP == null)
                    {
                        OIP = new MS04GenerationOneT();
                        OIP.DeviceName = strName;
                        OIP.DIndex = dIndex;
                        OIP.ReadDefaultInfo(wdDeviceType);
                        if (CsConst.myDrys == null) CsConst.myDrys = new List<MS04>(); CsConst.myDrys.Add(OIP);
                    }

                    if (CsConst.MyUpload2Down == 0) OIP.DownLoadInformationFrmDevice(OIP.DeviceName, wdDeviceType, intActivePage, 0, 0);
                    else if (CsConst.MyUpload2Down == 1) OIP.UploadMS04ToDevice(OIP.DeviceName, wdDeviceType, intActivePage, 0, 0);

                    if (CsConst.isRestore)
                    {
                        OIP.remarkBackup = strName.Split('\\')[1].Trim();
                        String TmpDevName = strName.Split('\\')[0].Trim();
                        OIP.iIndexBackup = dIndex;
                        OIP.DeviceTypeBackup = wdDeviceType;
                        OIP.subnetIDBackup = Byte.Parse(TmpDevName.Split('-')[0].ToString());
                        OIP.deviceIDBackup = Byte.Parse(TmpDevName.Split('-')[1].ToString());

                        CsConst.myBackupLists.Add(OIP);
                    }
                    #endregion
                }
                else if (MS04DeviceTypeList.HDLMS04DeviceTypeList.Contains(wdDeviceType)) // 是不是MS04
                {
                    #region
                    MS04 TmpRelay = null;
                    if (CsConst.myDrys != null)
                    {
                        foreach (MS04 Tmp in CsConst.myDrys)
                        {
                            if (Tmp.DIndex == dIndex)
                            {
                                TmpRelay = Tmp;
                                break;
                            }
                        }
                    }
                    if (CsConst.myDrys == null || TmpRelay == null)
                    {
                        TmpRelay = new MS04();
                        TmpRelay.DeviceName = strName;
                        TmpRelay.DIndex = dIndex;
                        if (CsConst.myDrys == null) CsConst.myDrys = new List<MS04>(); CsConst.myDrys.Add(TmpRelay);
                    }
                    if (CsConst.MyUpload2Down == 0)
                    {
                        TmpRelay.DownLoadInformationFrmDevice(TmpRelay.DeviceName, wdDeviceType, intActivePage, 0, 0);
                        if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(100, null);
                    }
                    else if (CsConst.MyUpload2Down == 1) TmpRelay.UploadMS04ToDevice(TmpRelay.DeviceName, wdDeviceType, intActivePage, 0, 0);

                    if (CsConst.isRestore)
                    {
                        TmpRelay.remarkBackup = strName.Split('\\')[1].Trim();
                        String TmpDevName = strName.Split('\\')[0].Trim();
                        TmpRelay.iIndexBackup = dIndex;
                        TmpRelay.DeviceTypeBackup = wdDeviceType;
                        TmpRelay.subnetIDBackup = Byte.Parse(TmpDevName.Split('-')[0].ToString());
                        TmpRelay.deviceIDBackup = Byte.Parse(TmpDevName.Split('-')[1].ToString());

                        CsConst.myBackupLists.Add(TmpRelay);
                    }
                    #endregion
                }
                else if (CurtainDeviceType.HDLCurtainModuleDeviceType.Contains(wdDeviceType)) // 是不是窗帘模块
                {
                    #region
                    Curtain TmpRelay = null;
                    if (CsConst.myCurtains != null)
                    {
                        foreach (Curtain Tmp in CsConst.myCurtains)
                        {
                            if (Tmp.DIndex == dIndex)
                            {
                                TmpRelay = Tmp;
                                break;
                            }
                        }
                    }
                    if (CsConst.myCurtains == null || TmpRelay == null)
                    {
                        TmpRelay = new Curtain();
                        TmpRelay.DeviceName = strName;
                        TmpRelay.DIndex = dIndex;
                        if (CsConst.myCurtains == null) CsConst.myCurtains = new List<Curtain>(); CsConst.myCurtains.Add(TmpRelay);
                    }
                    if (CsConst.MyUpload2Down == 0) TmpRelay.DownLoadInformationFrmDevice(TmpRelay.DeviceName, wdDeviceType);
                    else if (CsConst.MyUpload2Down == 1) TmpRelay.UploaDeviceFromBufferToDevice(TmpRelay.DeviceName, wdDeviceType);

                    if (CsConst.isRestore)
                    {
                        TmpRelay.remarkBackup = strName.Split('\\')[1].Trim();
                        String TmpDevName = strName.Split('\\')[0].Trim();
                        TmpRelay.iIndexBackup = dIndex;
                        TmpRelay.DeviceTypeBackup = wdDeviceType;
                        TmpRelay.subnetIDBackup = Byte.Parse(TmpDevName.Split('-')[0].ToString());
                        TmpRelay.deviceIDBackup = Byte.Parse(TmpDevName.Split('-')[1].ToString());

                        CsConst.myBackupLists.Add(TmpRelay);
                    }
                    #endregion
                }
                else if (HAIModuleDeviceTypeList.HDLHAIModuleDeviceTypeLists.Contains(wdDeviceType)) // 是不是hai模块
                {
                    #region
                    HAI TmpRelay = null;
                    if (CsConst.myHais != null)
                    {
                        foreach (HAI Tmp in CsConst.myHais)
                        {
                            if (Tmp.DIndex == dIndex)
                            {
                                TmpRelay = Tmp;
                                break;
                            }
                        }
                    }
                    if (CsConst.myHais == null || TmpRelay == null)
                    {
                        TmpRelay = new HAI();
                        TmpRelay.DeviceName = strName;
                        TmpRelay.DIndex = dIndex;
                        if (CsConst.myHais == null) CsConst.myHais = new List<HAI>(); CsConst.myHais.Add(TmpRelay);
                    }
                    if (CsConst.MyUpload2Down == 0) TmpRelay.DownLoadInformationFrmDevice(TmpRelay.DeviceName);
                    else if (CsConst.MyUpload2Down == 1) TmpRelay.UploaDeviceFromBufferToDevice(TmpRelay.DeviceName);

                    if (CsConst.isRestore)
                    {
                        TmpRelay.remarkBackup = strName.Split('\\')[1].Trim();
                        String TmpDevName = strName.Split('\\')[0].Trim();
                        TmpRelay.iIndexBackup = dIndex;
                        TmpRelay.DeviceTypeBackup = wdDeviceType;
                        TmpRelay.subnetIDBackup = Byte.Parse(TmpDevName.Split('-')[0].ToString());
                        TmpRelay.deviceIDBackup = Byte.Parse(TmpDevName.Split('-')[1].ToString());

                        CsConst.myBackupLists.Add(TmpRelay);
                    }
                    #endregion
                }
                else if (HVACModuleDeviceTypeList.HDLHVACModuleDeviceTypeLists.Contains(wdDeviceType)) // 是不是hvac模块
                {
                    #region
                    HVAC TmpRelay = null;
                    if (CsConst.myHvacs != null)
                    {
                        foreach (HVAC Tmp in CsConst.myHvacs)
                        {
                            if (Tmp.DIndex == dIndex)
                            {
                                TmpRelay = Tmp;
                                break;
                            }
                        }
                    }
                    if (CsConst.myHvacs == null || TmpRelay == null)
                    {
                        TmpRelay = new HVAC();
                        TmpRelay.DeviceName = strName;
                        TmpRelay.DIndex = dIndex;
                        if (CsConst.myHvacs == null) CsConst.myHvacs = new List<HVAC>(); CsConst.myHvacs.Add(TmpRelay);
                    }
                    if (CsConst.MyUpload2Down == 0) TmpRelay.DownLoadInformationFrmDevice(TmpRelay.DeviceName, wdDeviceType, intActivePage);
                    else if (CsConst.MyUpload2Down == 1) TmpRelay.UploadHVACInfosToDevice(TmpRelay.DeviceName, wdDeviceType, intActivePage);

                    if (CsConst.isRestore)
                    {
                        TmpRelay.remarkBackup = strName.Split('\\')[1].Trim();
                        String TmpDevName = strName.Split('\\')[0].Trim();
                        TmpRelay.iIndexBackup = dIndex;
                        TmpRelay.DeviceTypeBackup = wdDeviceType;
                        TmpRelay.subnetIDBackup = Byte.Parse(TmpDevName.Split('-')[0].ToString());
                        TmpRelay.deviceIDBackup = Byte.Parse(TmpDevName.Split('-')[1].ToString());

                        CsConst.myBackupLists.Add(TmpRelay);
                    }
                    #endregion
                }
                else if (FloorheatingDeviceTypeList.HDLFloorHeatingDeviceType.Contains(wdDeviceType)) // 是不是FH模块
                {
                    #region
                    FH TmpRelay = null;
                    if (CsConst.myFHs != null)
                    {
                        foreach (FH Tmp in CsConst.myFHs)
                        {
                            if (Tmp.DIndex == dIndex)
                            {
                                TmpRelay = Tmp;
                                break;
                            }
                        }
                    }
                    if (CsConst.myFHs == null || TmpRelay == null)
                    {
                        TmpRelay = new FH();
                        TmpRelay.DeviceName = strName;
                        TmpRelay.DIndex = dIndex;
                        if (CsConst.myFHs == null) CsConst.myFHs = new List<FH>(); CsConst.myFHs.Add(TmpRelay);
                    }
                    if (CsConst.MyUpload2Down == 0) TmpRelay.DownloadFHInforsFrmDevice(TmpRelay.DeviceName, wdDeviceType, intActivePage, (Byte)num1, (Byte)num2);
                    else if (CsConst.MyUpload2Down == 1) TmpRelay.UploadFHInfosToDevice(TmpRelay.DeviceName, wdDeviceType, intActivePage);

                    if (CsConst.isRestore)
                    {
                        TmpRelay.remarkBackup = strName.Split('\\')[1].Trim();
                        String TmpDevName = strName.Split('\\')[0].Trim();
                        TmpRelay.iIndexBackup = dIndex;
                        TmpRelay.DeviceTypeBackup = wdDeviceType;
                        TmpRelay.subnetIDBackup = Byte.Parse(TmpDevName.Split('-')[0].ToString());
                        TmpRelay.deviceIDBackup = Byte.Parse(TmpDevName.Split('-')[1].ToString());

                        CsConst.myBackupLists.Add(TmpRelay);
                    }
                    #endregion
                }
                else if (SocketDeviceTypeList.HDLSocketDeviceTypeList.Contains(wdDeviceType)) // 是不是socket
                {
                    #region
                    RFSocket TmpRelay = null;
                    if (CsConst.mySockets != null)
                    {
                        foreach (RFSocket Tmp in CsConst.mySockets)
                        {
                            if (Tmp.DIndex == dIndex)
                            {
                                TmpRelay = Tmp;
                                break;
                            }
                        }
                    }
                    if (CsConst.mySockets == null || TmpRelay == null)
                    {
                        TmpRelay = new RFSocket();
                        TmpRelay.DeviceName = strName;
                        TmpRelay.DIndex = dIndex;
                        if (CsConst.mySockets == null) CsConst.mySockets = new List<RFSocket>(); CsConst.mySockets.Add(TmpRelay);
                    }
                    if (CsConst.MyUpload2Down == 0) TmpRelay.DownLoadInformationFrmDevice(TmpRelay.DeviceName, wdDeviceType, 0);
                    else if (CsConst.MyUpload2Down == 1) TmpRelay.UploadInfosToDevice(TmpRelay.DeviceName, wdDeviceType, 0);

                    if (CsConst.isRestore)
                    {
                        TmpRelay.remarkBackup = strName.Split('\\')[1].Trim();
                        String TmpDevName = strName.Split('\\')[0].Trim();
                        TmpRelay.iIndexBackup = dIndex;
                        TmpRelay.DeviceTypeBackup = wdDeviceType;
                        TmpRelay.subnetIDBackup = Byte.Parse(TmpDevName.Split('-')[0].ToString());
                        TmpRelay.deviceIDBackup = Byte.Parse(TmpDevName.Split('-')[1].ToString());

                        CsConst.myBackupLists.Add(TmpRelay);
                    }
                    #endregion
                }
                else if (MSPUSenserDeviceTypeList.SensorsDeviceTypeList.Contains(wdDeviceType)) // 是不是超声波设备
                {
                    #region
                    MSPU TmpRelay = null;
                    if (CsConst.myPUSensors != null)
                    {
                        foreach (MSPU Tmp in CsConst.myPUSensors)
                        {
                            if (Tmp.DIndex == dIndex)
                            {
                                TmpRelay = Tmp;
                                break;
                            }
                        }
                    }

                    if (CsConst.myPUSensors == null || TmpRelay == null)
                    {
                        TmpRelay = new MSPU();
                        TmpRelay.Devname = strName;
                        TmpRelay.DIndex = dIndex;
                        if (CsConst.myPUSensors == null) CsConst.myPUSensors = new List<MSPU>(); CsConst.myPUSensors.Add(TmpRelay);
                    }
                    if (CsConst.MyUpload2Down == 0) TmpRelay.DownloadMSPUInfoToDevice(TmpRelay.Devname, wdDeviceType, intActivePage);
                    else if (CsConst.MyUpload2Down == 1) TmpRelay.UploadMSPUInfoToDevice(TmpRelay.Devname, wdDeviceType, intActivePage, num1, num2);
                    if (CsConst.isRestore)
                    {
                        TmpRelay.remarkBackup = strName.Split('\\')[1].Trim();
                        String TmpDevName = strName.Split('\\')[0].Trim();
                        TmpRelay.iIndexBackup = dIndex;
                        TmpRelay.DeviceTypeBackup = wdDeviceType;
                        TmpRelay.subnetIDBackup = Byte.Parse(TmpDevName.Split('-')[0].ToString());
                        TmpRelay.deviceIDBackup = Byte.Parse(TmpDevName.Split('-')[1].ToString());

                        CsConst.myBackupLists.Add(TmpRelay);
                    }
                    #endregion
                }
                else if (SecurityDeviceTypeList.HDLSecurityDeviceTypeList.Contains(wdDeviceType)) // 是不是安防模块
                {
                    #region
                    Security TmpRelay = null;
                    if (CsConst.mySecurities != null)
                    {
                        foreach (Security Tmp in CsConst.mySecurities)
                        {
                            if (Tmp.DIndex == dIndex)
                            {
                                TmpRelay = Tmp;
                                break;
                            }
                        }
                    }

                    if (CsConst.myPUSensors == null || TmpRelay == null)
                    {
                        TmpRelay = new Security();
                        TmpRelay.DeviceName = strName;
                        TmpRelay.DIndex = dIndex;
                        if (CsConst.mySecurities == null) CsConst.mySecurities = new List<Security>(); CsConst.mySecurities.Add(TmpRelay);
                    }
                    if (CsConst.MyUpload2Down == 0) TmpRelay.DownLoadSecurityInformationFrmDevice(TmpRelay.DeviceName, wdDeviceType, intActivePage);
                    else if (CsConst.MyUpload2Down == 1) TmpRelay.UploadSecurityToDevice(TmpRelay.DeviceName, wdDeviceType, intActivePage);

                    if (CsConst.isRestore)
                    {
                        TmpRelay.remarkBackup = strName.Split('\\')[1].Trim();
                        String TmpDevName = strName.Split('\\')[0].Trim();
                        TmpRelay.iIndexBackup = dIndex;
                        TmpRelay.DeviceTypeBackup = wdDeviceType;
                        TmpRelay.subnetIDBackup = Byte.Parse(TmpDevName.Split('-')[0].ToString());
                        TmpRelay.deviceIDBackup = Byte.Parse(TmpDevName.Split('-')[1].ToString());

                        CsConst.myBackupLists.Add(TmpRelay);
                    }
                    #endregion
                }
                else if (MiniSenserDeviceTypeList.SensorsDeviceTypeList.Contains(wdDeviceType))//MINI超声波
                {
                    #region
                    MiniSensor oMINIUL = null;
                    if (CsConst.myMiniSensors != null)
                    {
                        foreach (MiniSensor oTmp in CsConst.myMiniSensors)
                        {
                            if (oTmp.DIndex == dIndex)
                            {
                                oMINIUL = oTmp;
                                break;
                            }
                        }
                    }
                    if (CsConst.myMiniSensors == null || oMINIUL == null)
                    {
                        oMINIUL.DIndex = dIndex;
                        oMINIUL.Devname = strName;
                        CsConst.myMiniSensors = new List<MiniSensor>(); CsConst.myMiniSensors.Add(oMINIUL);
                    }
                    if (CsConst.MyUpload2Down == 0) oMINIUL.DownloadMSPUInfoToDevice(oMINIUL.Devname, wdDeviceType, intActivePage);
                    else if (CsConst.MyUpload2Down == 1) oMINIUL.UploadMSPUInfoToDevice(oMINIUL.Devname, wdDeviceType, intActivePage, num1, num2);

                    if (CsConst.isRestore)
                    {
                        oMINIUL.remarkBackup = strName.Split('\\')[1].Trim();
                        String TmpDevName = strName.Split('\\')[0].Trim();
                        oMINIUL.iIndexBackup = dIndex;
                        oMINIUL.DeviceTypeBackup = wdDeviceType;
                        oMINIUL.subnetIDBackup = Byte.Parse(TmpDevName.Split('-')[0].ToString());
                        oMINIUL.deviceIDBackup = Byte.Parse(TmpDevName.Split('-')[1].ToString());

                        CsConst.myBackupLists.Add(oMINIUL);
                    }
                    #endregion
                }
                else if (AudioDeviceTypeList.AudioBoxDeviceTypeList.Contains(wdDeviceType)) // 所有的音乐盒子
                {
                    #region
                    MzBox oMINIUL = null;
                    if (CsConst.myzBoxs != null)
                    {
                        foreach (MzBox oTmp in CsConst.myzBoxs)
                        {
                            if (oTmp.DIndex == dIndex)
                            {
                                oMINIUL = oTmp;
                                break;
                            }
                        }
                    }
                    if (CsConst.myzBoxs == null || oMINIUL == null)
                    {
                        oMINIUL.DIndex = dIndex;
                        oMINIUL.Devname = strName;
                        CsConst.myzBoxs = new List<MzBox>(); CsConst.myzBoxs.Add(oMINIUL);
                    }
                    if (CsConst.MyUpload2Down == 0) oMINIUL.DownloadInfosToDevice(oMINIUL.Devname, wdDeviceType, intActivePage);
                    else if (CsConst.MyUpload2Down == 1) oMINIUL.UploadInfosToDevice(oMINIUL.Devname, wdDeviceType, intActivePage);

                    if (CsConst.isRestore)
                    {
                        oMINIUL.remarkBackup = strName.Split('\\')[1].Trim();
                        String TmpDevName = strName.Split('\\')[0].Trim();
                        oMINIUL.iIndexBackup = dIndex;
                        oMINIUL.DeviceTypeBackup = wdDeviceType;
                        oMINIUL.subnetIDBackup = Byte.Parse(TmpDevName.Split('-')[0].ToString());
                        oMINIUL.deviceIDBackup = Byte.Parse(TmpDevName.Split('-')[1].ToString());

                        CsConst.myBackupLists.Add(oMINIUL);
                    }
                    #endregion
                }
                else if (LogicDeviceTypeList.LogicDeviceType.Contains(wdDeviceType)) // 所有的逻辑模块
                {
                    #region
                    Logic oMINIUL = null;
                    if (CsConst.myLogics != null)
                    {
                        foreach (Logic oTmp in CsConst.myLogics)
                        {
                            if (oTmp.DIndex == dIndex)
                            {
                                oMINIUL = oTmp;
                                break;
                            }
                        }
                    }
                    if (CsConst.myzBoxs == null || oMINIUL == null)
                    {
                        oMINIUL.DIndex = dIndex;
                        oMINIUL.DevName = strName;
                        CsConst.myLogics = new List<Logic>(); CsConst.myLogics.Add(oMINIUL);
                    }
                    if (CsConst.MyUpload2Down == 0) oMINIUL.DownLoadSecurityInformationFrmDevice(oMINIUL.DevName, wdDeviceType, intActivePage);
                    else if (CsConst.MyUpload2Down == 1) oMINIUL.UploadSecurityToDevice(oMINIUL.DevName, wdDeviceType, intActivePage);

                    if (CsConst.isRestore)
                    {
                        oMINIUL.remarkBackup = strName.Split('\\')[1].Trim();
                        String TmpDevName = strName.Split('\\')[0].Trim();
                        oMINIUL.iIndexBackup = dIndex;
                        oMINIUL.DeviceTypeBackup = wdDeviceType;
                        oMINIUL.subnetIDBackup = Byte.Parse(TmpDevName.Split('-')[0].ToString());
                        oMINIUL.deviceIDBackup = Byte.Parse(TmpDevName.Split('-')[1].ToString());

                        CsConst.myBackupLists.Add(oMINIUL);
                    }
                    #endregion
                }
                else if (ConstMMC.MediaPBoxDeviceType.Contains(wdDeviceType)) // 所有的多媒体盒子
                {
                    #region
                    MMC oMINIUL = null;
                    if (CsConst.myMediaBoxes != null)
                    {
                        foreach (MMC oTmp in CsConst.myMediaBoxes)
                        {
                            if (oTmp.DIndex == dIndex)
                            {
                                oMINIUL = oTmp;
                                break;
                            }
                        }
                    }

                    if (CsConst.myMediaBoxes == null || oMINIUL == null)
                    {
                        oMINIUL.DIndex = dIndex;
                        oMINIUL.strName = strName;
                        CsConst.myMediaBoxes = new List<MMC>(); CsConst.myMediaBoxes.Add(oMINIUL);
                    }
                    if (CsConst.MyUpload2Down == 0) oMINIUL.DownloadRS232FrmDeviceToBuf(oMINIUL.strName, wdDeviceType, intActivePage);
                    else if (CsConst.MyUpload2Down == 1) oMINIUL.UploadMMCInfosToDevice(oMINIUL.strName, wdDeviceType, intActivePage);

                    if (CsConst.isRestore)
                    {
                        oMINIUL.remarkBackup = strName.Split('\\')[1].Trim();
                        String TmpDevName = strName.Split('\\')[0].Trim();
                        oMINIUL.iIndexBackup = dIndex;
                        oMINIUL.DeviceTypeBackup = wdDeviceType;
                        oMINIUL.subnetIDBackup = Byte.Parse(TmpDevName.Split('-')[0].ToString());
                        oMINIUL.deviceIDBackup = Byte.Parse(TmpDevName.Split('-')[1].ToString());

                        CsConst.myBackupLists.Add(oMINIUL);
                    }
                    #endregion
                }
                else if (Eightin1DeviceTypeList.HDL8in1DeviceType.Contains(wdDeviceType)) // 所有的8in1
                {
                    #region
                    MultiSensor oMINIUL = null;
                    if (CsConst.mysensor_8in1 != null)
                    {
                        foreach (MultiSensor oTmp in CsConst.mysensor_8in1)
                        {
                            if (oTmp.DIndex == dIndex)
                            {
                                oMINIUL = oTmp;
                                break;
                            }
                        }
                    }
                    if (CsConst.myMediaBoxes == null || oMINIUL == null)
                    {
                        oMINIUL.DIndex = dIndex;
                        oMINIUL.Devname = strName;
                        CsConst.mysensor_8in1 = new List<MultiSensor>(); CsConst.mysensor_8in1.Add(oMINIUL);
                    }
                    if (CsConst.MyUpload2Down == 0) oMINIUL.DownloadSensorInfosToDevice(oMINIUL.Devname, wdDeviceType, intActivePage, num1, num2);
                    else if (CsConst.MyUpload2Down == 1) oMINIUL.UploadSensorInfosToDevice(oMINIUL.Devname, wdDeviceType, intActivePage, num1, num2);

                    if (CsConst.isRestore)
                    {
                        oMINIUL.remarkBackup = strName.Split('\\')[1].Trim();
                        String TmpDevName = strName.Split('\\')[0].Trim();
                        oMINIUL.iIndexBackup = dIndex;
                        oMINIUL.DeviceTypeBackup = wdDeviceType;
                        oMINIUL.subnetIDBackup = Byte.Parse(TmpDevName.Split('-')[0].ToString());
                        oMINIUL.deviceIDBackup = Byte.Parse(TmpDevName.Split('-')[1].ToString());

                        CsConst.myBackupLists.Add(oMINIUL);
                    }
                    #endregion
                }
                else if (Twelvein1DeviceTypeList.HDL12in1DeviceType.Contains(wdDeviceType)) // 所有的12in1
                {
                    #region
                    Sensor_12in1 oMINIUL = null;
                    if (CsConst.mysensor_12in1 != null)
                    {
                        foreach (Sensor_12in1 oTmp in CsConst.mysensor_12in1)
                        {
                            if (oTmp.DIndex == dIndex)
                            {
                                oMINIUL = oTmp;
                                break;
                            }
                        }
                    }

                    if (CsConst.mysensor_12in1 == null || oMINIUL == null)
                    {
                        oMINIUL = new Sensor_12in1();
                        oMINIUL.DIndex = dIndex;
                        oMINIUL.Devname = strName;
                        CsConst.mysensor_12in1 = new List<Sensor_12in1>(); CsConst.mysensor_12in1.Add(oMINIUL);
                    }
                    if (CsConst.MyUpload2Down == 0) oMINIUL.DownloadSensorInfosToDevice(oMINIUL.Devname, wdDeviceType, intActivePage, num1, num2);
                    else if (CsConst.MyUpload2Down == 1) oMINIUL.UploadSensorInfosToDevice(oMINIUL.Devname, intActivePage);

                    if (CsConst.isRestore)
                    {
                        oMINIUL.remarkBackup = strName.Split('\\')[1].Trim();
                        String TmpDevName = strName.Split('\\')[0].Trim();
                        oMINIUL.iIndexBackup = dIndex;
                        oMINIUL.DeviceTypeBackup = wdDeviceType;
                        oMINIUL.subnetIDBackup = Byte.Parse(TmpDevName.Split('-')[0].ToString());
                        oMINIUL.deviceIDBackup = Byte.Parse(TmpDevName.Split('-')[1].ToString());

                        CsConst.myBackupLists.Add(oMINIUL);
                    }
                    #endregion
                }
                else if (HotelMixModuleDeviceType.HDLRCUDeviceTypeLists.Contains(wdDeviceType)) // 所有酒店混合模块
                {
                    #region
                    MHRCU oMINIUL = null;
                    if (CsConst.myRcuMixModules != null)
                    {
                        foreach (MHRCU oTmp in CsConst.myRcuMixModules)
                        {
                            if (oTmp.DIndex == dIndex)
                            {
                                oMINIUL = oTmp;
                                break;
                            }
                        }
                    }
                    if (CsConst.myRcuMixModules == null || oMINIUL == null)
                    {
                        oMINIUL.DIndex = dIndex;
                        oMINIUL.Devname = strName;
                        CsConst.myRcuMixModules = new List<MHRCU>(); CsConst.myRcuMixModules.Add(oMINIUL);
                    }
                    if (CsConst.MyUpload2Down == 0) oMINIUL.DownloadMHRCUInforsFrmDevice(oMINIUL.Devname, wdDeviceType, intActivePage, num1, num2);
                    else if (CsConst.MyUpload2Down == 1) oMINIUL.UploadPanelInfosToDevice(oMINIUL.Devname, wdDeviceType, intActivePage, num1, num2);

                    if (CsConst.isRestore)
                    {
                        oMINIUL.remarkBackup = strName.Split('\\')[1].Trim();
                        String TmpDevName = strName.Split('\\')[0].Trim();
                        oMINIUL.iIndexBackup = dIndex;
                        oMINIUL.DeviceTypeBackup = wdDeviceType;
                        oMINIUL.subnetIDBackup = Byte.Parse(TmpDevName.Split('-')[0].ToString());
                        oMINIUL.deviceIDBackup = Byte.Parse(TmpDevName.Split('-')[1].ToString());

                        CsConst.myBackupLists.Add(oMINIUL);
                    }
                    #endregion
                }
                else if (MHICDeviceTypeList.HDLCardReaderDeviceType.Contains(wdDeviceType)) // 所有插卡取电
                {
                    #region
                    MHIC oMINIUL = null;
                    if (CsConst.myCardReader != null)
                    {
                        foreach (MHIC oTmp in CsConst.myCardReader)
                        {
                            if (oTmp.DIndex == dIndex)
                            {
                                oMINIUL = oTmp;
                                break;
                            }
                        }
                    }

                    if (CsConst.myCardReader == null || oMINIUL == null)
                    {
                        oMINIUL.DIndex = dIndex;
                        oMINIUL.DeviceName = strName;
                        CsConst.myCardReader = new List<MHIC>(); CsConst.myCardReader.Add(oMINIUL);
                    }
                    if (CsConst.MyUpload2Down == 0) oMINIUL.DownLoadInfoFrmDevice(oMINIUL.DeviceName, wdDeviceType, intActivePage);
                    else if (CsConst.MyUpload2Down == 1) oMINIUL.UploadInfosToDevice(oMINIUL.DeviceName, wdDeviceType, intActivePage);
                    if (CsConst.isRestore)
                    {
                        oMINIUL.remarkBackup = strName.Split('\\')[1].Trim();
                        String TmpDevName = strName.Split('\\')[0].Trim();
                        oMINIUL.iIndexBackup = dIndex;
                        oMINIUL.DeviceTypeBackup = wdDeviceType;
                        oMINIUL.subnetIDBackup = Byte.Parse(TmpDevName.Split('-')[0].ToString());
                        oMINIUL.deviceIDBackup = Byte.Parse(TmpDevName.Split('-')[1].ToString());

                        CsConst.myBackupLists.Add(oMINIUL);
                    }
                    #endregion
                }
                else if (BacNetDeviceTypeList.HDLBacNetDeviceTypeList.Contains(wdDeviceType)) // bacnet设备类型
                {
                    #region
                    BacNet oMINIUL = null;
                    if (CsConst.myBacnets != null)
                    {
                        foreach (BacNet oTmp in CsConst.myBacnets)
                        {
                            if (oTmp.DIndex == dIndex)
                            {
                                oMINIUL = oTmp;
                                break;
                            }
                        }
                    }
                    if (CsConst.myBacnets == null || oMINIUL == null)
                    {
                        oMINIUL.DIndex = dIndex;
                        oMINIUL.DeviceName = strName;
                        CsConst.myBacnets = new List<BacNet>(); CsConst.myBacnets.Add(oMINIUL);
                    }
                    if (CsConst.MyUpload2Down == 0) oMINIUL.DownloadEIBInforsFrmDevice(oMINIUL.DeviceName, wdDeviceType, intActivePage, num1, num2);
                    else if (CsConst.MyUpload2Down == 1) oMINIUL.UploadCurtainInfosToDevice(oMINIUL.DeviceName, intActivePage);

                    if (CsConst.isRestore)
                    {
                        oMINIUL.remarkBackup = strName.Split('\\')[1].Trim();
                        String TmpDevName = strName.Split('\\')[0].Trim();
                        oMINIUL.iIndexBackup = dIndex;
                        oMINIUL.DeviceTypeBackup = wdDeviceType;
                        oMINIUL.subnetIDBackup = Byte.Parse(TmpDevName.Split('-')[0].ToString());
                        oMINIUL.deviceIDBackup = Byte.Parse(TmpDevName.Split('-')[1].ToString());

                        CsConst.myBackupLists.Add(oMINIUL);
                    }
                    #endregion
                }
                else if (DMXDeviceTypeList.HDLDMXDeviceTypeList.Contains(wdDeviceType)) // DMX 新版带逻辑灯
                {
                    #region
                    DMX oMINIUL = null;
                    if (CsConst.myDmxs != null)
                    {
                        foreach (DMX oTmp in CsConst.myDmxs)
                        {
                            if (oTmp.DIndex == dIndex)
                            {
                                oMINIUL = oTmp;
                                break;
                            }
                        }
                    }
                    if (CsConst.myDmxs == null || oMINIUL == null)
                    {
                        oMINIUL.DIndex = dIndex;
                        oMINIUL.DeviceName = strName;
                        CsConst.myDmxs = new List<DMX>(); CsConst.myDmxs.Add(oMINIUL);
                    }
                    if (CsConst.MyUpload2Down == 0) oMINIUL.DownloadDimmerInfosToBuffer(oMINIUL.DeviceName, wdDeviceType);
                    else if (CsConst.MyUpload2Down == 1) oMINIUL.UploadSettings(wdDeviceType);
                    if (CsConst.isRestore)
                    {
                        oMINIUL.remarkBackup = strName.Split('\\')[1].Trim();
                        String TmpDevName = strName.Split('\\')[0].Trim();
                        oMINIUL.iIndexBackup = dIndex;
                        oMINIUL.DeviceTypeBackup = wdDeviceType;
                        oMINIUL.subnetIDBackup = Byte.Parse(TmpDevName.Split('-')[0].ToString());
                        oMINIUL.deviceIDBackup = Byte.Parse(TmpDevName.Split('-')[1].ToString());

                        CsConst.myBackupLists.Add(oMINIUL);
                    }
                    #endregion
                }
                else if (T4SensorDeviceTypeList.HDLTsensorDeviceType.Contains(wdDeviceType)) // 四通道温度传感器
                {
                    #region
                    TempSensor oMINIUL = null;
                    if (CsConst.myTemperatureSensors != null)
                    {
                        foreach (TempSensor oTmp in CsConst.myTemperatureSensors)
                        {
                            if (oTmp.DIndex == dIndex)
                            {
                                oMINIUL = oTmp;
                                break;
                            }
                        }
                    }

                    if (CsConst.myTemperatureSensors == null || oMINIUL == null)
                    {
                        oMINIUL.DIndex = dIndex;
                        oMINIUL.Devname = strName;
                        CsConst.myTemperatureSensors = new List<TempSensor>(); CsConst.myTemperatureSensors.Add(oMINIUL);
                    }
                    if (CsConst.MyUpload2Down == 0) oMINIUL.DownLoadInformationFrmDevice(oMINIUL.Devname);
                    else if (CsConst.MyUpload2Down == 1) oMINIUL.UploadDimmerInfosToDevice(oMINIUL.Devname);

                    if (CsConst.isRestore)
                    {
                        oMINIUL.remarkBackup = strName.Split('\\')[1].Trim();
                        String TmpDevName = strName.Split('\\')[0].Trim();
                        oMINIUL.iIndexBackup = dIndex;
                        oMINIUL.DeviceTypeBackup = wdDeviceType;
                        oMINIUL.subnetIDBackup = Byte.Parse(TmpDevName.Split('-')[0].ToString());
                        oMINIUL.deviceIDBackup = Byte.Parse(TmpDevName.Split('-')[1].ToString());

                        CsConst.myBackupLists.Add(oMINIUL);
                    }
                    #endregion
                }
                else if (DSDeviceTypeList.DoorStationDeviceType.Contains(wdDeviceType)) //是不是门口机
                {
                    #region
                    DS oLogic = null;
                    if (CsConst.myDS != null)
                    {
                        foreach (DS oTmp in CsConst.myDS)
                        {
                            if (oTmp.DIndex == dIndex)
                            {
                                oLogic = oTmp;
                                break;
                            }
                        }
                    }
                    if (CsConst.myDS == null || oLogic == null)
                    {
                        oLogic = new DS();
                        oLogic.Devname = strName;
                        oLogic.DIndex = dIndex;
                        if (CsConst.myDS == null) CsConst.myDS = new List<DS>(); CsConst.myDS.Add(oLogic);
                    }

                    if (CsConst.MyUpload2Down == 0) oLogic.DownloadDSInforsFrmDevice(oLogic.Devname, wdDeviceType, intActivePage, num1, num2);
                    else if (CsConst.MyUpload2Down == 1) oLogic.UploadDSInfoToDevice(oLogic.Devname, wdDeviceType, intActivePage);

                    if (CsConst.isRestore)
                    {
                        oLogic.remarkBackup = strName.Split('\\')[1].Trim();
                        String TmpDevName = strName.Split('\\')[0].Trim();
                        oLogic.iIndexBackup = dIndex;
                        oLogic.DeviceTypeBackup = wdDeviceType;
                        oLogic.subnetIDBackup = Byte.Parse(TmpDevName.Split('-')[0].ToString());
                        oLogic.deviceIDBackup = Byte.Parse(TmpDevName.Split('-')[1].ToString());

                        CsConst.myBackupLists.Add(oLogic);
                    }
                    #endregion
                }
                else if (DSDeviceTypeList.NewDoorStationDeviceType.Contains(wdDeviceType))//新门口机
                {
                    #region
                    NewDS oNewDS = null;
                    if (CsConst.myNewDS != null)
                    {
                        foreach (NewDS oTmp in CsConst.myNewDS)
                        {
                            if (oTmp.DIndex == dIndex)
                            {
                                oNewDS = oTmp;
                                break;
                            }
                        }
                    }
                    if (CsConst.myNewDS == null || oNewDS == null)
                    {
                        oNewDS = new NewDS();
                        oNewDS.DIndex = dIndex;
                        oNewDS.Devname = strName;

                        if (CsConst.myNewDS == null) CsConst.myNewDS = new List<NewDS>();
                        CsConst.myNewDS.Add(oNewDS);
                    }

                    if (CsConst.MyUpload2Down == 0) oNewDS.DownLoadInfoFrmDevice(oNewDS.Devname, wdDeviceType, intActivePage, num1, num2);
                    else if (CsConst.MyUpload2Down == 1) oNewDS.UploadInfosToDevice(oNewDS.Devname, wdDeviceType, intActivePage);

                    if (CsConst.isRestore)
                    {
                        oNewDS.remarkBackup = strName.Split('\\')[1].Trim();
                        String TmpDevName = strName.Split('\\')[0].Trim();
                        oNewDS.iIndexBackup = dIndex;
                        oNewDS.DeviceTypeBackup = wdDeviceType;
                        oNewDS.subnetIDBackup = Byte.Parse(TmpDevName.Split('-')[0].ToString());
                        oNewDS.deviceIDBackup = Byte.Parse(TmpDevName.Split('-')[1].ToString());

                        CsConst.myBackupLists.Add(oNewDS);
                    }
                    #endregion
                }
                else if (IrModuleDeviceTypeList.HDNewIrModuleDeviceTypeLists.Contains(wdDeviceType))//判断是不是新的红外发射类型
                {
                    #region
                    NewIR send = null;
                    if (CsConst.myNewIR != null)
                    {
                        foreach (NewIR se in CsConst.myNewIR)
                        {
                            if (se.DIndex == dIndex)
                            {
                                send = se;
                                break;
                            }
                        }
                    }

                    if (CsConst.myNewIR == null || send == null)
                    {
                        send = new NewIR();
                        send.DIndex = dIndex;
                        send.strName = strName;
                        if (CsConst.myNewIR == null) CsConst.myNewIR = new List<NewIR>();
                        CsConst.myNewIR.Add(send);
                    }
                    if (CsConst.MyUpload2Down == 0) send.DownloadNewIRInfoFrmDevice(send.strName, wdDeviceType, intActivePage);
                    else if (CsConst.MyUpload2Down == 1) send.UploadNewIRInfosToDevice(send.strName, wdDeviceType, intActivePage);

                    if (CsConst.isRestore)
                    {
                        send.remarkBackup = strName.Split('\\')[1].Trim();
                        String TmpDevName = strName.Split('\\')[0].Trim();
                        send.iIndexBackup = dIndex;
                        send.DeviceTypeBackup = wdDeviceType;
                        send.subnetIDBackup = Byte.Parse(TmpDevName.Split('-')[0].ToString());
                        send.deviceIDBackup = Byte.Parse(TmpDevName.Split('-')[1].ToString());

                        CsConst.myBackupLists.Add(send);
                    }
                    #endregion
                }
                else if (Rs232DeviceTypeList.HDLRs232DeviceType.Contains(wdDeviceType))  //是不是RS232
                {
                    #region
                    RS232 rs = null;
                    if (CsConst.myRS232 != null)
                    {
                        foreach (RS232 rst in CsConst.myRS232)
                        {
                            if (rst.DIndex == dIndex)
                            {
                                rs = rst;
                                break;
                            }
                        }
                    }

                    if (CsConst.myRS232 == null || rs == null)
                    {
                        rs = new RS232();
                        rs.DIndex = dIndex;
                        rs.strName = strName;
                        if (CsConst.myRS232 == null) CsConst.myRS232 = new List<RS232>(); CsConst.myRS232.Add(rs);
                    }
                    if (CsConst.MyUpload2Down == 0) rs.DownloadRS232FrmDeviceToBuf(rs.strName, wdDeviceType, intActivePage, num1, num2);
                    else if (CsConst.MyUpload2Down == 1) rs.UploadRS232InfosToDevice(rs.strName, wdDeviceType, intActivePage);
                    if (CsConst.isRestore)
                    {
                        rs.remarkBackup = strName.Split('\\')[1].Trim();
                        String TmpDevName = strName.Split('\\')[0].Trim();
                        rs.iIndexBackup = dIndex;
                        rs.DeviceTypeBackup = wdDeviceType;
                        rs.subnetIDBackup = Byte.Parse(TmpDevName.Split('-')[0].ToString());
                        rs.deviceIDBackup = Byte.Parse(TmpDevName.Split('-')[1].ToString());

                        CsConst.myBackupLists.Add(rs);
                    }
                    #endregion
                }
                else if (SMSModuleDeviceTypeList.HDLGPRSModuleDeviceTypeLists.Contains(wdDeviceType)) //判断是不是GPRS类型的设备
                {
                    #region
                    GPRS TmpGPRS = null;
                    if (CsConst.myGPRS != null)
                    {
                        foreach (GPRS Tmp in CsConst.myGPRS)
                        {
                            if (Tmp.DIndex == dIndex)
                            {
                                TmpGPRS = Tmp;
                                break;
                            }
                        }
                    }

                    if (CsConst.myGPRS == null || TmpGPRS == null)
                    {
                        TmpGPRS = new GPRS();
                        TmpGPRS.DIndex = dIndex;
                        TmpGPRS.Devname = strName;
                        if (CsConst.myGPRS == null) CsConst.myGPRS = new List<GPRS>(); CsConst.myGPRS.Add(TmpGPRS);
                    }

                    if (CsConst.MyUpload2Down == 0) TmpGPRS.DownloadSMSFrmDeviceToBuf(TmpGPRS.Devname, wdDeviceType, intActivePage, num1, num2);
                    else if (CsConst.MyUpload2Down == 1) TmpGPRS.UploadSMSFrmDeviceToBuf(TmpGPRS.Devname, wdDeviceType, intActivePage);

                    if (CsConst.isRestore)
                    {
                        TmpGPRS.remarkBackup = strName.Split('\\')[1].Trim();
                        String TmpDevName = strName.Split('\\')[0].Trim();
                        TmpGPRS.iIndexBackup = dIndex;
                        TmpGPRS.DeviceTypeBackup = wdDeviceType;
                        TmpGPRS.subnetIDBackup = Byte.Parse(TmpDevName.Split('-')[0].ToString());
                        TmpGPRS.deviceIDBackup = Byte.Parse(TmpDevName.Split('-')[1].ToString());

                        CsConst.myBackupLists.Add(TmpGPRS);
                    }
                    #endregion
                }
            }
            catch { }
        }

        private void btnRestart_Click(object sender, EventArgs e)
        {
            ReadThread();
        }

        private void FrmDownloadShow_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy)
                {
                    CsConst.calculationWorker.Dispose();
                    CsConst.calculationWorker = null;
                }
                CsConst.isStopDealImageBackground = true;
                this.Dispose();
            }
            catch
            {
            }
        }

    }
}
