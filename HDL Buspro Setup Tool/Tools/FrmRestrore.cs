using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Runtime.Serialization.Formatters.Binary;

namespace HDL_Buspro_Setup_Tool
{
    public partial class FrmRestrore : Form
    {
        string strPath = "";
        TextBox txtSub = new TextBox();
        TextBox txtDev = new TextBox();
        public FrmRestrore()
        {
            InitializeComponent();
            LoadControlsText.DisplayTextToFormWhenFirstShow(this);
        }

        public FrmRestrore(string strpath)
        {
            InitializeComponent();
            this.strPath = strpath;
            CsConst.mstrCurPath = strPath;
            txtSub.KeyPress += txtFrm_KeyPress;
            txtDev.KeyPress += txtFrm_KeyPress;
            txtSub.TextChanged += txtSub_TextChanged;
            txtDev.TextChanged += txtDev_TextChanged;
            dgvListA.Controls.Add(txtDev);
            dgvListA.Controls.Add(txtSub);
            txtDev.Visible = false;
            txtSub.Visible = false;
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
            }
        }

        private void txtFrm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar) || e.KeyChar == 8)
                e.Handled = false;
            else
                e.Handled = true;
        }

        private void chbOption_CheckedChanged(object sender, EventArgs e)
        {
            if (dgvListA.Rows.Count == 0) return;

            for (int intI = 0; intI < dgvListA.Rows.Count; intI++)
            {
                dgvListA[0, intI].Value = chbOption.Checked;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvListA.Rows.Count == 0) return;
            List<int> selctList = new List<int>();
            for (int i = dgvListA.RowCount - 1; i >= 0; i--)
            {
                if (dgvListA[0, i].Value.ToString().ToLower() == "true")
                {
                    selctList.Add(i);
                }
            }

            foreach (int i in selctList)
            {
                dgvListA.Rows.RemoveAt(i);
            }
        }

        private void btnUpdateA_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                this.Enabled = false;
                CsConst.isRestore = true;
                
                for (int i = 0; i < dgvListA.RowCount; i++)
                {
                    if (dgvListA[0, i].Value.ToString().ToUpper() == "TRUE")
                    {
                        CsConst.isBackUpSucceed = false;
                        int DIndex = Convert.ToInt32(dgvListA[7, i].Value.ToString());
                        int Devicetype = Convert.ToInt32(dgvListA[8, i].Value.ToString());
                        byte SubNetID = Convert.ToByte(dgvListA[1, i].Value.ToString());
                        byte DevID = Convert.ToByte(dgvListA[2, i].Value.ToString());
                        string Remark = dgvListA[3, i].Value.ToString();
                        string strName = SubNetID.ToString() + "-" + DevID.ToString() + "\\" + Remark;
                        CsConst.RestoreRemark = Remark;
                        CsConst.MyUPload2DownLists = new List<byte[]>();
                        byte[] ArayRelay = new byte[] { SubNetID, DevID, (byte)(Devicetype / 256), (byte)(Devicetype % 256), 0, 
                                                        (byte)(DIndex / 256),(byte)(DIndex % 256)};
                        CsConst.MyUPload2DownLists.Add(ArayRelay);
                        CsConst.MyUpload2Down = 1;
                        CopyBackupsBufferToPublicDevices(CsConst.myBackupLists[i]);
                        FrmDownloadShow Frm = new FrmDownloadShow();
                        Frm.ShowDialog();
                        if (CsConst.isBackUpSucceed)
                            dgvListA[6, i].Value = CsConst.mstrINIDefault.IniReadValue("Public", "99934", "");
                        else
                            dgvListA[6, i].Value = CsConst.mstrINIDefault.IniReadValue("Public", "99935", "");
                        System.Threading.Thread.Sleep(1000);
                    }
                }
            }
            catch
            {
            }
            Cursor.Current = Cursors.Default;
            this.Enabled = true;
            CsConst.isRestore = false;
        }

        void GetBackupsDevicesListFromFile(String strPath)
        {
            try
            {
                CsConst.myBackupLists = new List<HdlDeviceBackupAndRestore>();
                if (File.Exists(strPath))
                {
                    FileStream fs = new FileStream(strPath, FileMode.Open);
                    BinaryFormatter bf = new BinaryFormatter();
                    CsConst.myBackupLists = bf.Deserialize(fs) as List<HdlDeviceBackupAndRestore>;
                    fs.Close();
                }
            }
            catch
            { }
        }

        private void FrmRestrore_Load(object sender, EventArgs e)
        {
            try
            {
                lbPathValue.Text = strPath;
                DisplayBackupDevicesListToGrid();
            }
            catch
            {
            }
        }

        void DisplayBackupDevicesListToGrid()
        {
            try
            {
                GetBackupsDevicesListFromFile(strPath);
               // CsConst.myDimmers
                if (CsConst.myBackupLists != null && CsConst.myBackupLists.Count != 0)
                {
                    foreach (HdlDeviceBackupAndRestore oTmpDevice in CsConst.myBackupLists)
                    {
                        int DeviceType = oTmpDevice.DeviceTypeBackup;
                        String[] ModelDescription = DeviceTypeList.GetDisplayInformationFromPublicModeGroup(DeviceType);
                        object[] obj = new object[] { false,
                                                    oTmpDevice.subnetIDBackup.ToString(),
                                                    oTmpDevice.deviceIDBackup.ToString(),
                                                    oTmpDevice.remarkBackup.ToString(),
                                                    ModelDescription[0],
                                                    ModelDescription[1], CsConst.mstrINIDefault.IniReadValue("Public", "99933", ""),
                                                    oTmpDevice.iIndexBackup.ToString(),DeviceType.ToString()};
                        dgvListA.Rows.Add(obj);
                    }
                }
            }
            catch
            { }
        }

        private void dgvListA_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtSub.Visible = false;
            txtDev.Visible = false;
            if (e.RowIndex >= 0)
            {
                addcontrols(1, e.RowIndex, txtSub);
                txtSub.Text = dgvListA[1, e.RowIndex].Value.ToString().Trim();

                txtDev.Text = dgvListA[2, e.RowIndex].Value.ToString().Trim();
                addcontrols(2, e.RowIndex, txtDev);
            }
        }

        private void addcontrols(int col, int row, Control con)
        {
            con.Visible = true;
            con.Show();
            Rectangle rect = dgvListA.GetCellDisplayRectangle(col, row, true);
            con.Size = rect.Size;
            con.Top = rect.Top;
            con.Left = rect.Left;
        }

        private void FrmRestrore_FormClosing(object sender, FormClosingEventArgs e)
        {
        }

        private void btnModify_Click(object sender, EventArgs e)
        {
            string strFileName = HDLPF.OpenFileDialog("dat files (*.dat)|*.dat", "dat Files");
            if (strFileName == null) return;
            this.strPath = strFileName;
            CsConst.mstrCurPath = strPath;
            lbPathValue.Text = strFileName;
            dgvListA.Rows.Clear();
            try
            {
                DisplayBackupDevicesListToGrid();
            }
            catch
            {
            }
            
        }

        public static void CopyBackupsBufferToPublicDevices(HdlDeviceBackupAndRestore oDevice)
        {
            int wdDeviceType = oDevice.DeviceTypeBackup;
            try
            {
                if (IPmoduleDeviceTypeList.HDLIPModuleDeviceTypeLists.Contains(wdDeviceType)) // 是不是一端口交换机
                {
                    #region
                    IPModule TmpIP = (IPModule)oDevice;
                    Boolean bIsHasDeviceInList = false;
                    if (CsConst.myIPs != null)
                    {
                        for (int i = 0; i < CsConst.myIPs.Count; i++)
                        {
                            if (CsConst.myIPs[i].DIndex == TmpIP.DIndex)
                            {
                                bIsHasDeviceInList = true;
                                CsConst.myIPs[i] = TmpIP;
                                break;
                            }
                        }
                    }
                    else if (CsConst.myIPs == null || bIsHasDeviceInList == false)
                    {
                        if (CsConst.myIPs == null) CsConst.myIPs = new List<IPModule>(); CsConst.myIPs.Add(TmpIP);
                    }
                    #endregion
                }
                else if (DimmerDeviceTypeList.HDLDimmerDeviceTypeList.Contains(wdDeviceType)) //是不是调光器
                {
                    #region
                    Dimmer TmpIP = (Dimmer)oDevice;
                    Boolean bIsHasDeviceInList = false;
                    if (CsConst.myDimmers != null)
                    {
                        for (int i = 0; i < CsConst.myDimmers.Count; i++)
                        {
                            if (CsConst.myDimmers[i].DIndex == TmpIP.DIndex)
                            {
                                bIsHasDeviceInList = true;
                                CsConst.myDimmers[i] = TmpIP;
                                break;
                            }
                        }
                    }
                    else if (CsConst.myDimmers == null || bIsHasDeviceInList == false)
                    {
                        if (CsConst.myDimmers == null) CsConst.myDimmers = new List<Dimmer>(); CsConst.myDimmers.Add(TmpIP);
                    }
                    #endregion
                }
                else if (RelayDeviceTypeList.HDLRelayDeviceTypeList.Contains(wdDeviceType)) // 是不是继电器模块
                {
                    #region
                    Relay TmpIP = (Relay)oDevice;
                    Boolean bIsHasDeviceInList = false;
                    if (CsConst.myRelays != null)
                    {
                        for (int i = 0; i < CsConst.myRelays.Count; i++)
                        {
                            if (CsConst.myRelays[i].DIndex == TmpIP.DIndex)
                            {
                                bIsHasDeviceInList = true;
                                CsConst.myRelays[i] = TmpIP;
                                break;
                            }
                        }
                    }
                    else if (CsConst.myRelays == null || bIsHasDeviceInList == false)
                    {
                        if (CsConst.myRelays == null) CsConst.myRelays = new List<Relay>(); CsConst.myRelays.Add(TmpIP);
                    }
                    #endregion
                }
                else if (EnviroNewDeviceTypeList.EnviroNewPanelDeviceTypeList.Contains(wdDeviceType)) // 是不是新版彩屏面板
                {
                    #region
                    EnviroPanel TmpIP = (EnviroPanel)oDevice;
                    Boolean bIsHasDeviceInList = false;
                    if (CsConst.myEnviroPanels != null)
                    {
                        for (int i = 0; i < CsConst.myEnviroPanels.Count; i++)
                        {
                            if (CsConst.myEnviroPanels[i].DIndex == TmpIP.DIndex)
                            {
                                bIsHasDeviceInList = true;
                                CsConst.myEnviroPanels[i] = TmpIP;
                                break;
                            }
                        }
                    }
                    else if (CsConst.myEnviroPanels == null || bIsHasDeviceInList == false)
                    {
                        if (CsConst.myEnviroPanels == null) CsConst.myEnviroPanels = new List<EnviroPanel>(); CsConst.myEnviroPanels.Add(TmpIP);
                    }
                    #endregion
                }
                else if (EnviroDLPPanelDeviceTypeList.HDLEnviroDLPPanelDeviceTypeList.Contains(wdDeviceType)) // 是不是彩屏面板
                {
                    #region
                    ColorDLP TmpIP = (ColorDLP)oDevice;
                    Boolean bIsHasDeviceInList = false;
                    if (CsConst.myColorPanels != null)
                    {
                        for (int i = 0; i < CsConst.myColorPanels.Count; i++)
                        {
                            if (CsConst.myColorPanels[i].DIndex == TmpIP.DIndex)
                            {
                                bIsHasDeviceInList = true;
                                CsConst.myColorPanels[i] = TmpIP;
                                break;
                            }
                        }
                    }
                    else if (CsConst.myColorPanels == null || bIsHasDeviceInList == false)
                    {
                        if (CsConst.myColorPanels == null) CsConst.myColorPanels = new List<ColorDLP>(); CsConst.myColorPanels.Add(TmpIP);
                    }
                    #endregion
                }
                else if (DLPPanelDeviceTypeList.HDLDLPPanelDeviceTypeList.Contains(wdDeviceType)) // 是不是DLP面板
                {
                    #region
                    DLP TmpIP = (DLP)oDevice;
                    Boolean bIsHasDeviceInList = false;
                    if (CsConst.myPanels != null)
                    {
                        for (int i = 0; i < CsConst.myPanels.Count; i++)
                        {
                            if (CsConst.myPanels[i].DIndex == TmpIP.DIndex)
                            {
                                bIsHasDeviceInList = true;
                                CsConst.myPanels[i] = TmpIP;
                                break;
                            }
                        }
                    }
                    else if (CsConst.myPanels == null || bIsHasDeviceInList == false)
                    {
                        if (CsConst.myPanels == null) CsConst.myPanels = new List<Panel>(); CsConst.myPanels.Add(TmpIP);
                    }    

                   
                    #endregion
                }
                else if (NormalPanelDeviceTypeList.HDLNormalPanelDeviceTypeList.Contains(wdDeviceType)) // 是不是面板
                {
                    #region
                    Panel TmpIP = (Panel)oDevice;
                    Boolean bIsHasDeviceInList = false;
                    if (CsConst.myPanels != null)
                    {
                        for (int i = 0; i < CsConst.myPanels.Count; i++)
                        {
                            if (CsConst.myPanels[i].DIndex == TmpIP.DIndex)
                            {
                                bIsHasDeviceInList = true;
                                CsConst.myPanels[i] = TmpIP;
                                break;
                            }
                        }
                    }
                    else if (CsConst.myPanels == null || bIsHasDeviceInList == false)
                    {
                        if (CsConst.myPanels == null) CsConst.myPanels = new List<Panel>(); CsConst.myPanels.Add(TmpIP);
                    }
                    #endregion
                }
                else if (CoolMasterDeviceTypeList.HDLCoolMasterDeviceTypeList.Contains(wdDeviceType)) // 是不是coolmaster
                {
                    #region
                    CoolMaster TmpIP = (CoolMaster)oDevice;
                    Boolean bIsHasDeviceInList = false;
                    if (CsConst.myPanels != null)
                    {
                        for (int i = 0; i < CsConst.myCoolMasters.Count; i++)
                        {
                            if (CsConst.myCoolMasters[i].DIndex == TmpIP.DIndex)
                            {
                                bIsHasDeviceInList = true;
                                CsConst.myCoolMasters[i] = TmpIP;
                                break;
                            }
                        }
                    }
                    else if (CsConst.myCoolMasters == null || bIsHasDeviceInList == false)
                    {
                        if (CsConst.myCoolMasters == null) CsConst.myCoolMasters = new List<CoolMaster>(); CsConst.myCoolMasters.Add(TmpIP);
                    }
                    #endregion
                }
                else if (MS04DeviceTypeList.WirelessMS04WithOneCurtain.Contains(wdDeviceType)) // 判断是不是干接点类型的设备窗帘
                {
                    #region
                    MS04GenerationOneCurtain TmpIP = (MS04GenerationOneCurtain)oDevice;
                    Boolean bIsHasDeviceInList = false;
                    if (CsConst.myDrys != null)
                    {
                        for (int i = 0; i < CsConst.myDrys.Count; i++)
                        {
                            if (CsConst.myDrys[i].DIndex == TmpIP.DIndex)
                            {
                                bIsHasDeviceInList = true;
                                CsConst.myDrys[i] = TmpIP;
                                break;
                            }
                        }
                    }
                    else if (CsConst.myDrys == null || bIsHasDeviceInList == false)
                    {
                        if (CsConst.myDrys == null) CsConst.myDrys = new List<MS04>(); CsConst.myDrys.Add(TmpIP);
                    }
                    #endregion
                }
                else if (MS04DeviceTypeList.WirelessMS04WithRelayChn.Contains(wdDeviceType)) // 判断是不是干接点类型的设备继电器
                {
                    #region
                    MS04GenerationOne2R TmpIP = (MS04GenerationOne2R)oDevice;
                    Boolean bIsHasDeviceInList = false;
                    if (CsConst.myDrys != null)
                    {
                        for (int i = 0; i < CsConst.myDrys.Count; i++)
                        {
                            if (CsConst.myDrys[i].DIndex == TmpIP.DIndex)
                            {
                                bIsHasDeviceInList = true;
                                CsConst.myDrys[i] = TmpIP;
                                break;
                            }
                        }
                    }
                    else if (CsConst.myDrys == null || bIsHasDeviceInList == false)
                    {
                        if (CsConst.myDrys == null) CsConst.myDrys = new List<MS04>(); CsConst.myDrys.Add(TmpIP);
                    }
                    #endregion
                }
                else if (MS04DeviceTypeList.WirelessMS04WithDimmerChn.Contains(wdDeviceType)) // 判断是不是干接点类型的设备调光器
                {
                    #region
                    MS04GenerationOneD TmpIP = (MS04GenerationOneD)oDevice;
                    Boolean bIsHasDeviceInList = false;
                    if (CsConst.myDrys != null)
                    {
                        for (int i = 0; i < CsConst.myDrys.Count; i++)
                        {
                            if (CsConst.myDrys[i].DIndex == TmpIP.DIndex)
                            {
                                bIsHasDeviceInList = true;
                                CsConst.myDrys[i] = TmpIP;
                                break;
                            }
                        }
                    }
                    else if (CsConst.myDrys == null || bIsHasDeviceInList == false)
                    {
                        if (CsConst.myDrys == null) CsConst.myDrys = new List<MS04>(); CsConst.myDrys.Add(TmpIP);
                    }                    
                    #endregion
                }
                else if (MS04DeviceTypeList.MS04IModuleWithTemperature.Contains(wdDeviceType)) // 判断是不是干接点类型的设备温度探头
                {
                    #region
                    MS04GenerationOneT TmpIP = (MS04GenerationOneT)oDevice;
                    Boolean bIsHasDeviceInList = false;
                    if (CsConst.myDrys != null)
                    {
                        for (int i = 0; i < CsConst.myDrys.Count; i++)
                        {
                            if (CsConst.myDrys[i].DIndex == TmpIP.DIndex)
                            {
                                bIsHasDeviceInList = true;
                                CsConst.myDrys[i] = TmpIP;
                                break;
                            }
                        }
                    }
                    else if (CsConst.myDrys == null || bIsHasDeviceInList == false)
                    {
                        if (CsConst.myDrys == null) CsConst.myDrys = new List<MS04>(); CsConst.myDrys.Add(TmpIP);
                    }    
                    #endregion
                }
                else if (MS04DeviceTypeList.HDLMS04DeviceTypeList.Contains(wdDeviceType)) // 是不是MS04
                {
                    #region
                    MS04 TmpIP = (MS04)oDevice;
                    Boolean bIsHasDeviceInList = false;
                    if (CsConst.myDrys != null)
                    {
                        for (int i = 0; i < CsConst.myDrys.Count; i++)
                        {
                            if (CsConst.myDrys[i].DIndex == TmpIP.DIndex)
                            {
                                bIsHasDeviceInList = true;
                                CsConst.myDrys[i] = TmpIP;
                                break;
                            }
                        }
                    }
                    else if (CsConst.myDrys == null || bIsHasDeviceInList == false)
                    {
                        if (CsConst.myDrys == null) CsConst.myDrys = new List<MS04>(); CsConst.myDrys.Add(TmpIP);
                    }    
                    #endregion
                }
                else if (CurtainDeviceType.HDLCurtainModuleDeviceType.Contains(wdDeviceType)) // 是不是窗帘模块
                {
                    #region
                    Curtain TmpIP = (Curtain)oDevice;
                    Boolean bIsHasDeviceInList = false;
                    if (CsConst.myCurtains != null)
                    {
                        for (int i = 0; i < CsConst.myCurtains.Count; i++)
                        {
                            if (CsConst.myCurtains[i].DIndex == TmpIP.DIndex)
                            {
                                bIsHasDeviceInList = true;
                                CsConst.myCurtains[i] = TmpIP;
                                break;
                            }
                        }
                    }
                    else if (CsConst.myCurtains == null || bIsHasDeviceInList == false)
                    {
                        if (CsConst.myCurtains == null) CsConst.myCurtains = new List<Curtain>(); CsConst.myCurtains.Add(TmpIP);
                    }   
                    #endregion
                }
                else if (HAIModuleDeviceTypeList.HDLHAIModuleDeviceTypeLists.Contains(wdDeviceType)) // 是不是hai模块
                {
                    #region
                    HAI TmpIP = (HAI)oDevice;
                    Boolean bIsHasDeviceInList = false;
                    if (CsConst.myHais != null)
                    {
                        for (int i = 0; i < CsConst.myHais.Count; i++)
                        {
                            if (CsConst.myHais[i].DIndex == TmpIP.DIndex)
                            {
                                bIsHasDeviceInList = true;
                                CsConst.myHais[i] = TmpIP;
                                break;
                            }
                        }
                    }
                    else if (CsConst.myHais == null || bIsHasDeviceInList == false)
                    {
                        if (CsConst.myHais == null) CsConst.myHais = new List<HAI>(); CsConst.myHais.Add(TmpIP);
                    }   
                    #endregion
                }
                else if (HVACModuleDeviceTypeList.HDLHVACModuleDeviceTypeLists.Contains(wdDeviceType)) // 是不是hvac模块
                {
                    #region
                    HVAC TmpIP = (HVAC)oDevice;
                    Boolean bIsHasDeviceInList = false;
                    if (CsConst.myHvacs != null)
                    {
                        for (int i = 0; i < CsConst.myHvacs.Count; i++)
                        {
                            if (CsConst.myHvacs[i].DIndex == TmpIP.DIndex)
                            {
                                bIsHasDeviceInList = true;
                                CsConst.myHvacs[i] = TmpIP;
                                break;
                            }
                        }
                    }
                    else if (CsConst.myHvacs == null || bIsHasDeviceInList == false)
                    {
                        if (CsConst.myHvacs == null) CsConst.myHvacs = new List<HVAC>(); CsConst.myHvacs.Add(TmpIP);
                    }   
                    #endregion
                }
                else if (FloorheatingDeviceTypeList.HDLFloorHeatingDeviceType.Contains(wdDeviceType)) // 是不是FH模块
                {
                    #region
                    FH TmpIP = (FH)oDevice;
                    Boolean bIsHasDeviceInList = false;
                    if (CsConst.myFHs != null)
                    {
                        for (int i = 0; i < CsConst.myFHs.Count; i++)
                        {
                            if (CsConst.myFHs[i].DIndex == TmpIP.DIndex)
                            {
                                bIsHasDeviceInList = true;
                                CsConst.myFHs[i] = TmpIP;
                                break;
                            }
                        }
                    }
                    else if (CsConst.myFHs == null || bIsHasDeviceInList == false)
                    {
                        if (CsConst.myFHs == null) CsConst.myFHs = new List<FH>(); CsConst.myFHs.Add(TmpIP);
                    }   
                    #endregion
                }
                else if (SocketDeviceTypeList.HDLSocketDeviceTypeList.Contains(wdDeviceType)) // 是不是socket
                {
                    #region
                    RFSocket TmpIP = (RFSocket)oDevice;
                    Boolean bIsHasDeviceInList = false;
                    if (CsConst.mySockets != null)
                    {
                        for (int i = 0; i < CsConst.mySockets.Count; i++)
                        {
                            if (CsConst.mySockets[i].DIndex == TmpIP.DIndex)
                            {
                                bIsHasDeviceInList = true;
                                CsConst.mySockets[i] = TmpIP;
                                break;
                            }
                        }
                    }
                    else if (CsConst.mySockets == null || bIsHasDeviceInList == false)
                    {
                        if (CsConst.mySockets == null) CsConst.mySockets = new List<RFSocket>(); CsConst.mySockets.Add(TmpIP);
                    }   
                    #endregion
                }
                else if (MSPUSenserDeviceTypeList.SensorsDeviceTypeList.Contains(wdDeviceType)) // 是不是超声波设备
                {
                    #region
                    MSPU TmpIP = (MSPU)oDevice;
                    Boolean bIsHasDeviceInList = false;
                    if (CsConst.myPUSensors != null)
                    {
                        for (int i = 0; i < CsConst.myPUSensors.Count; i++)
                        {
                            if (CsConst.myPUSensors[i].DIndex == TmpIP.DIndex)
                            {
                                bIsHasDeviceInList = true;
                                CsConst.myPUSensors[i] = TmpIP;
                                break;
                            }
                        }
                    }
                    else if (CsConst.myPUSensors == null || bIsHasDeviceInList == false)
                    {
                        if (CsConst.myPUSensors == null) CsConst.myPUSensors = new List<MSPU>(); CsConst.myPUSensors.Add(TmpIP);
                    }   
                    #endregion
                }
                else if (SecurityDeviceTypeList.HDLSecurityDeviceTypeList.Contains(wdDeviceType)) // 是不是安防模块
                {
                    #region
                    Security TmpIP = (Security)oDevice;
                    Boolean bIsHasDeviceInList = false;
                    if (CsConst.mySecurities != null)
                    {
                        for (int i = 0; i < CsConst.mySecurities.Count; i++)
                        {
                            if (CsConst.mySecurities[i].DIndex == TmpIP.DIndex)
                            {
                                bIsHasDeviceInList = true;
                                CsConst.mySecurities[i] = TmpIP;
                                break;
                            }
                        }
                    }
                    else if (CsConst.mySecurities == null || bIsHasDeviceInList == false)
                    {
                        if (CsConst.mySecurities == null) CsConst.mySecurities = new List<Security>(); CsConst.mySecurities.Add(TmpIP);
                    }   
                    #endregion
                }
                else if (MiniSenserDeviceTypeList.SensorsDeviceTypeList.Contains(wdDeviceType))//MINI超声波
                {
                    #region
                    MiniSensor TmpIP = (MiniSensor)oDevice;
                    Boolean bIsHasDeviceInList = false;
                    if (CsConst.myMiniSensors != null)
                    {
                        for (int i = 0; i < CsConst.myMiniSensors.Count; i++)
                        {
                            if (CsConst.myMiniSensors[i].DIndex == TmpIP.DIndex)
                            {
                                bIsHasDeviceInList = true;
                                CsConst.myMiniSensors[i] = TmpIP;
                                break;
                            }
                        }
                    }
                    else if (CsConst.myMiniSensors == null || bIsHasDeviceInList == false)
                    {
                        if (CsConst.myMiniSensors == null) CsConst.myMiniSensors = new List<MiniSensor>(); CsConst.myMiniSensors.Add(TmpIP);
                    }  
                    #endregion
                }
                else if (AudioDeviceTypeList.AudioBoxDeviceTypeList.Contains(wdDeviceType)) // 所有的音乐盒子
                {
                    #region
                    MzBox TmpIP = (MzBox)oDevice;
                    Boolean bIsHasDeviceInList = false;
                    if (CsConst.myzBoxs != null)
                    {
                        for (int i = 0; i < CsConst.myzBoxs.Count; i++)
                        {
                            if (CsConst.myzBoxs[i].DIndex == TmpIP.DIndex)
                            {
                                bIsHasDeviceInList = true;
                                CsConst.myzBoxs[i] = TmpIP;
                                break;
                            }
                        }
                    }
                    else if (CsConst.myzBoxs == null || bIsHasDeviceInList == false)
                    {
                        if (CsConst.myzBoxs == null) CsConst.myzBoxs = new List<MzBox>(); CsConst.myzBoxs.Add(TmpIP);
                    }  
                    #endregion
                }
                else if (LogicDeviceTypeList.LogicDeviceType.Contains(wdDeviceType)) // 所有的逻辑模块
                {
                    #region
                    Logic TmpIP = (Logic)oDevice;
                    Boolean bIsHasDeviceInList = false;
                    if (CsConst.myLogics != null)
                    {
                        for (int i = 0; i < CsConst.myLogics.Count; i++)
                        {
                            if (CsConst.myLogics[i].DIndex == TmpIP.DIndex)
                            {
                                bIsHasDeviceInList = true;
                                CsConst.myLogics[i] = TmpIP;
                                break;
                            }
                        }
                    }
                    else if (CsConst.myLogics == null || bIsHasDeviceInList == false)
                    {
                        if (CsConst.myLogics == null) CsConst.myLogics = new List<Logic>(); CsConst.myLogics.Add(TmpIP);
                    }  
                    #endregion
                }
                else if (ConstMMC.MediaPBoxDeviceType.Contains(wdDeviceType)) // 所有的多媒体盒子
                {
                    #region
                    MMC TmpIP = (MMC)oDevice;
                    Boolean bIsHasDeviceInList = false;
                    if (CsConst.myMediaBoxes != null)
                    {
                        for (int i = 0; i < CsConst.myMediaBoxes.Count; i++)
                        {
                            if (CsConst.myMediaBoxes[i].DIndex == TmpIP.DIndex)
                            {
                                bIsHasDeviceInList = true;
                                CsConst.myMediaBoxes[i] = TmpIP;
                                break;
                            }
                        }
                    }
                    else if (CsConst.myMediaBoxes == null || bIsHasDeviceInList == false)
                    {
                        if (CsConst.myMediaBoxes == null) CsConst.myMediaBoxes = new List<MMC>(); CsConst.myMediaBoxes.Add(TmpIP);
                    }  
                    #endregion
                }
                else if (Eightin1DeviceTypeList.HDL8in1DeviceType.Contains(wdDeviceType)) // 所有的8in1
                {
                    #region
                    MultiSensor TmpIP = (MultiSensor)oDevice;
                    Boolean bIsHasDeviceInList = false;
                    if (CsConst.mysensor_8in1 != null)
                    {
                        for (int i = 0; i < CsConst.mysensor_8in1.Count; i++)
                        {
                            if (CsConst.mysensor_8in1[i].DIndex == TmpIP.DIndex)
                            {
                                bIsHasDeviceInList = true;
                                CsConst.mysensor_8in1[i] = TmpIP;
                                break;
                            }
                        }
                    }
                    else if (CsConst.mysensor_8in1 == null || bIsHasDeviceInList == false)
                    {
                        if (CsConst.mysensor_8in1 == null) CsConst.mysensor_8in1 = new List<MultiSensor>(); CsConst.mysensor_8in1.Add(TmpIP);
                    }  
                    #endregion
                }
                else if (Twelvein1DeviceTypeList.HDL12in1DeviceType.Contains(wdDeviceType)) // 所有的12in1
                {
                    #region
                    Sensor_12in1 TmpIP = (Sensor_12in1)oDevice;
                    Boolean bIsHasDeviceInList = false;
                    if (CsConst.mysensor_12in1 != null)
                    {
                        for (int i = 0; i < CsConst.mysensor_12in1.Count; i++)
                        {
                            if (CsConst.mysensor_12in1[i].DIndex == TmpIP.DIndex)
                            {
                                bIsHasDeviceInList = true;
                                CsConst.mysensor_12in1[i] = TmpIP;
                                break;
                            }
                        }
                    }
                    else if (CsConst.mysensor_12in1 == null || bIsHasDeviceInList == false)
                    {
                        if (CsConst.mysensor_12in1 == null) CsConst.mysensor_12in1 = new List<Sensor_12in1>(); CsConst.mysensor_12in1.Add(TmpIP);
                    }  
                    #endregion
                }
                else if (HotelMixModuleDeviceType.HDLRCUDeviceTypeLists.Contains(wdDeviceType)) // 所有酒店混合模块
                {
                    #region
                    MHRCU TmpIP = (MHRCU)oDevice;
                    Boolean bIsHasDeviceInList = false;
                    if (CsConst.myRcuMixModules != null)
                    {
                        for (int i = 0; i < CsConst.myRcuMixModules.Count; i++)
                        {
                            if (CsConst.myRcuMixModules[i].DIndex == TmpIP.DIndex)
                            {
                                bIsHasDeviceInList = true;
                                CsConst.myRcuMixModules[i] = TmpIP;
                                break;
                            }
                        }
                    }
                    else if (CsConst.myRcuMixModules == null || bIsHasDeviceInList == false)
                    {
                        if (CsConst.myRcuMixModules == null) CsConst.myRcuMixModules = new List<MHRCU>(); CsConst.myRcuMixModules.Add(TmpIP);
                    }  
                    #endregion
                }
                else if (MHICDeviceTypeList.HDLCardReaderDeviceType.Contains(wdDeviceType)) // 所有插卡取电
                {
                    #region
                    MHIC TmpIP = (MHIC)oDevice;
                    Boolean bIsHasDeviceInList = false;
                    if (CsConst.myCardReader != null)
                    {
                        for (int i = 0; i < CsConst.myCardReader.Count; i++)
                        {
                            if (CsConst.myCardReader[i].DIndex == TmpIP.DIndex)
                            {
                                bIsHasDeviceInList = true;
                                CsConst.myCardReader[i] = TmpIP;
                                break;
                            }
                        }
                    }
                    else if (CsConst.myCardReader == null || bIsHasDeviceInList == false)
                    {
                        if (CsConst.myCardReader == null) CsConst.myCardReader = new List<MHIC>(); CsConst.myCardReader.Add(TmpIP);
                    }  
                    #endregion
                }
                else if (BacNetDeviceTypeList.HDLBacNetDeviceTypeList.Contains(wdDeviceType)) // bacnet设备类型
                {
                    #region
                    BacNet TmpIP = (BacNet)oDevice;
                    Boolean bIsHasDeviceInList = false;
                    if (CsConst.myBacnets != null)
                    {
                        for (int i = 0; i < CsConst.myBacnets.Count; i++)
                        {
                            if (CsConst.myBacnets[i].DIndex == TmpIP.DIndex)
                            {
                                bIsHasDeviceInList = true;
                                CsConst.myBacnets[i] = TmpIP;
                                break;
                            }
                        }
                    }
                    else if (CsConst.myBacnets == null || bIsHasDeviceInList == false)
                    {
                        if (CsConst.myBacnets == null) CsConst.myBacnets = new List<BacNet>(); CsConst.myBacnets.Add(TmpIP);
                    }  
                    #endregion
                }
                else if (DMXDeviceTypeList.HDLDMXDeviceTypeList.Contains(wdDeviceType)) // DMX 新版带逻辑灯
                {
                    #region
                    DMX TmpIP = (DMX)oDevice;
                    Boolean bIsHasDeviceInList = false;
                    if (CsConst.myDmxs != null)
                    {
                        for (int i = 0; i < CsConst.myDmxs.Count; i++)
                        {
                            if (CsConst.myDmxs[i].DIndex == TmpIP.DIndex)
                            {
                                bIsHasDeviceInList = true;
                                CsConst.myDmxs[i] = TmpIP;
                                break;
                            }
                        }
                    }
                    else if (CsConst.myDmxs == null || bIsHasDeviceInList == false)
                    {
                        if (CsConst.myDmxs == null) CsConst.myDmxs = new List<DMX>(); CsConst.myDmxs.Add(TmpIP);
                    }  
                    #endregion
                }
                else if (T4SensorDeviceTypeList.HDLTsensorDeviceType.Contains(wdDeviceType)) // 四通道温度传感器
                {
                    #region
                    TempSensor TmpIP = (TempSensor)oDevice;
                    Boolean bIsHasDeviceInList = false;
                    if (CsConst.myTemperatureSensors != null)
                    {
                        for (int i = 0; i < CsConst.myTemperatureSensors.Count; i++)
                        {
                            if (CsConst.myTemperatureSensors[i].DIndex == TmpIP.DIndex)
                            {
                                bIsHasDeviceInList = true;
                                CsConst.myTemperatureSensors[i] = TmpIP;
                                break;
                            }
                        }
                    }
                    else if (CsConst.myTemperatureSensors == null || bIsHasDeviceInList == false)
                    {
                        if (CsConst.myTemperatureSensors == null) CsConst.myTemperatureSensors = new List<TempSensor>(); CsConst.myTemperatureSensors.Add(TmpIP);
                    }  
                    #endregion
                }
                else if (DSDeviceTypeList.DoorStationDeviceType.Contains(wdDeviceType)) //是不是门口机
                {
                    #region
                    DS TmpIP = (DS)oDevice;
                    Boolean bIsHasDeviceInList = false;
                    if (CsConst.myDS != null)
                    {
                        for (int i = 0; i < CsConst.myDS.Count; i++)
                        {
                            if (CsConst.myDS[i].DIndex == TmpIP.DIndex)
                            {
                                bIsHasDeviceInList = true;
                                CsConst.myDS[i] = TmpIP;
                                break;
                            }
                        }
                    }
                    else if (CsConst.myDS == null || bIsHasDeviceInList == false)
                    {
                        if (CsConst.myDS == null) CsConst.myDS = new List<DS>(); CsConst.myDS.Add(TmpIP);
                    }  
                    #endregion
                }
                else if (DSDeviceTypeList.NewDoorStationDeviceType.Contains(wdDeviceType))//新门口机
                {
                    #region
                    NewDS TmpIP = (NewDS)oDevice;
                    Boolean bIsHasDeviceInList = false;
                    if (CsConst.myNewDS != null)
                    {
                        for (int i = 0; i < CsConst.myNewDS.Count; i++)
                        {
                            if (CsConst.myNewDS[i].DIndex == TmpIP.DIndex)
                            {
                                bIsHasDeviceInList = true;
                                CsConst.myNewDS[i] = TmpIP;
                                break;
                            }
                        }
                    }
                    else if (CsConst.myNewDS == null || bIsHasDeviceInList == false)
                    {
                        if (CsConst.myNewDS == null) CsConst.myNewDS = new List<NewDS>(); CsConst.myNewDS.Add(TmpIP);
                    }  
                    #endregion
                }
                else if (IrModuleDeviceTypeList.HDNewIrModuleDeviceTypeLists.Contains(wdDeviceType))//判断是不是新的红外发射类型
                {
                    #region
                    NewIR TmpIP = (NewIR)oDevice;
                    Boolean bIsHasDeviceInList = false;
                    if (CsConst.myNewIR != null)
                    {
                        for (int i = 0; i < CsConst.myNewIR.Count; i++)
                        {
                            if (CsConst.myNewIR[i].DIndex == TmpIP.DIndex)
                            {
                                bIsHasDeviceInList = true;
                                CsConst.myNewIR[i] = TmpIP;
                                break;
                            }
                        }
                    }
                    else if (CsConst.myNewIR == null || bIsHasDeviceInList == false)
                    {
                        if (CsConst.myNewIR == null) CsConst.myNewIR = new List<NewIR>(); CsConst.myNewIR.Add(TmpIP);
                    }  
                    #endregion
                }
                else if (Rs232DeviceTypeList.HDLRs232DeviceType.Contains(wdDeviceType))  //是不是RS232
                {
                    #region
                    RS232 TmpIP = (RS232)oDevice;
                    Boolean bIsHasDeviceInList = false;
                    if (CsConst.myRS232 != null)
                    {
                        for (int i = 0; i < CsConst.myRS232.Count; i++)
                        {
                            if (CsConst.myRS232[i].DIndex == TmpIP.DIndex)
                            {
                                bIsHasDeviceInList = true;
                                CsConst.myRS232[i] = TmpIP;
                                break;
                            }
                        }
                    }
                    else if (CsConst.myRS232 == null || bIsHasDeviceInList == false)
                    {
                        if (CsConst.myRS232 == null) CsConst.myRS232 = new List<RS232>(); CsConst.myRS232.Add(TmpIP);
                    }  
                    #endregion
                }
            }
            catch { }
        }

    }
}
