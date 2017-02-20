using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace HDL_Buspro_Setup_Tool
{
    partial class frmMain : Form
    {
         private delegate void FlushClient(object[] tmp1); //代理
         private static Byte currentSelectDeviceSubnetId = 0;
         private static Byte currentSelectDeviceDeviceId = 0;
         private static int currentSelectDeviceDindex = -1;
        
         private static int SearchType = -1; // 0=0x000E; 1 = 0xE548

         public  frmMain()
        {
            InitializeComponent();

            Control.CheckForIllegalCrossThreadCalls = false;

            CsConst.mstrINIDefault = new IniFile(Application.StartupPath + @"\ini\Default.ini");

            CsConst.myLocalIP = HDLPF.GetLocalIP();
            HDLPF.GetRightIPAndPort();

            //在线模式初始化socket
            HDLSysPF.ReadInfoFromIniFile();
            SetVisiableAccordinglyEditMode();
            if (CsConst.MyEditMode == 1) CsConst.mySends.IniTheSocket(CsConst.myLocalIP);
            if (CsConst.bIsAutoUpgrade == true) autoReadVersionToolStripMenuItem_Click(autoUpgradeToolStripMenuItem, null);
        }

         private void tbOption_Click(object sender, EventArgs e)
         {
            frmSetup frmSet = new frmSetup();
            frmSet.FormClosing += new FormClosingEventHandler(UpdateFormVisibleAccordinglyMode);

            frmSet.Show(); 
         }

        void UpdateFormVisibleAccordinglyMode(object sender, FormClosingEventArgs e)
        {
            if (CsConst.myLocalIP == null) return;
            tspIP.Text = CsConst.myLocalIP;
            SetVisiableAccordinglyEditMode();
            if (CsConst.myintProxy == 0)
            {
                tspIP.Text = CsConst.mstrINIDefault.IniReadValue("", "00290", "") + "-" + CsConst.myLocalIP;
            }
            else if (CsConst.myintProxy == 1)
            {
                tspIP.Text = CsConst.mstrINIDefault.IniReadValue("", "00291", "") + "-" + CsConst.myLocalIP;
            }
            else if (CsConst.myintProxy == 2)
            {
                tspIP.Text = CsConst.mstrINIDefault.IniReadValue("", "00292", "");
            }

        }

        void SetVisiableAccordinglyEditMode()
        {

            if (CsConst.MyProgramMode == 0 || CsConst.MyProgramMode == -1) // 表示经典
            {
                tbFast.Visible = (CsConst.MyEditMode == 1);
                tsbExport.Visible = (CsConst.MyEditMode == 1);

                dgOnline.Visible = true;
            }
            else if (CsConst.MyProgramMode == 1) //表示简单模式
            {

            }
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker1.WorkerSupportsCancellation = true;
            CsConst.mstrDefaultPath = Application.StartupPath + @"\Database\Easy_Design.mdb";
            LoadControlsText.LoadButtonCoontrolTypeFromDatabaseToPublicClass();
            HDLSysPF.GetWeekdaysFromPublicStruct();
            //LoadControlsText.CheckAndLoadControlsNew();
            LoadControlsText.LoadControlsTextIdListFromXML();
            LoadControlsText.DisplayTextToFormWhenFirstShow(this);
                        
            CsConst.mstrINIDefault = new IniFile(Application.StartupPath + @"\ini\LAN0.ini");
            ButtonMode.LoadButtonModeFromDatabaseToPublicClass();
            DryMode.LoadButtonModeFromDatabaseToPublicClass();
            ButtonControlType.LoadButtonCoontrolTypeFromDatabaseToPublicClass();
            DryControlType.LoadButtonCoontrolTypeFromDatabaseToPublicClass();
            DeviceTypeList.LoadButtonCoontrolTypeFromDatabaseToPublicClass();
            ControlTemplates.ReadAllGroupCommandsFrmDatabase();

            
            HDLSysPF.AutoScale((Form)sender);

            HDLSysPF.AddedDevicesIndexToAPublicListInt();
            GetDeviceList();
           // dgOnline.RowCount = 28;

           
        }

        private void frmMain_Shown(object sender, EventArgs e)
        {
            tspIP.Text = CsConst.myLocalIP.ToString();
            this.Text = CsConst.softwareverson;
        }

        private void tbFast_Click(object sender, EventArgs e)
        {
            CsConst.AddedDevicesIndexGroup = new List<int>();
            try
            {
                CsConst.myIPs = null;
                CsConst.myRelays = null;
                CsConst.myDimmers = null;
                CsConst.myBacnets = null;
                CsConst.myPanels = null; // 普通面板
                CsConst.myCameras = null; // 摄像头或者
                CsConst.myColorPanels = null;//表示所有的彩色DLP
                CsConst.myEnviroPanels = null; // 新版面板
                CsConst.myCoolMasters = null; // coolmaster
                CsConst.myDrys = null; // 所有MS04
                CsConst.myCurtains = null; // 所有的窗帘模块
                CsConst.myHais = null; // 所有的HAI模块
                CsConst.myHvacs = null; // 所有的HVAC 模块
                CsConst.myFHs = null; // 所有的FH模块
                CsConst.myDmxs = null; // 所有的FH模块
                CsConst.mySockets = null; // 所以无线模块
                CsConst.myPUSensors = null; // 所有超声波传感器
                CsConst.mySecurities = null; // 所有的安防模块
                CsConst.mysensor_8in1 = null; // 所有8in1
                CsConst.mysensor_12in1 = null; // 所有12in1
                CsConst.myMiniSensors = null; // 所有迷你传感器
                CsConst.myzBoxs = null; // 所有的音乐播放器
                CsConst.myLogics = null; // 所有的逻辑模块
                CsConst.myMediaBoxes = null; // 所有多媒体播放器
                CsConst.myRcuMixModules = null; // 所有酒店混合模块

                tbFast.Enabled = false;
                dgOnline.Enabled = false;
                SearchType = 0x000E;
                backgroundWorker1.RunWorkerAsync();
            }
            catch
            { }
        }


        private void CmdTest_Click(object sender, EventArgs e)
        {
            bool isOpen = HDLSysPF.IsAlreadyOpenedForm("frmTool");

            if (isOpen== false)
            {
                frmTool frmTest = new frmTool();
                frmTest.Show();
            }
        }

        void calculationWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                if (CsConst.myOnlines != null && CsConst.myOnlines.Count() > 0)
                {
                    UpdateDataGridViewOfOnlineDevice();
                }
                CsConst.FastSearch = false;
                tbFast.Enabled = true;
                dgOnline.Enabled = true;
                toolStripStatusLabelDeviceCountShow.Text = Convert.ToString(dgOnline.Rows.Count);
            }
            catch
            {
            }
        }

        void calculationWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            try
            {

            }
            catch
            {
            }
        }

        void calculationWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            CsConst.FastSearch = true;
            //下面的内容相当于线程要处理的内容。//注意：不要在此事件中和界面控件打交道
            while (worker.CancellationPending == false)
            {
                try
                {
                    HdlUdpPublicFunctions.StartSimpleSearchWay(SearchType,dgOnline);
                    worker.CancelAsync();
                }
                catch
                {
                    worker.CancelAsync();
                }
            }
        }

        private void ThreadFunction(object[] obj)
        {
            dgOnline.Rows.Add(obj);
        }


        void GetSubnetIdAndDeviceIdFromSelectedOnlineDevice()
        {
             DataGridView dgv = new DataGridView();
            if (tabSim.SelectedTab.Name == "tabDev") dgv = this.dgOnline;
            if (dgv.SelectedRows.Count != 1) return;
            if (dgv.CurrentRow.Index < 0) return;
            currentSelectDeviceSubnetId = byte.Parse(dgv.SelectedCells[CsConst.MainFormSubNetIDStartFrom].Value.ToString());
            currentSelectDeviceDeviceId = byte.Parse(dgv.SelectedCells[CsConst.MainFormSubNetIDStartFrom + 1].Value.ToString());
            currentSelectDeviceDindex = Int32.Parse(dgv.SelectedCells[9].Value.ToString()) - 1;  
        }

        private void ftmFind_Click(object sender, EventArgs e)
        {
            GetSubnetIdAndDeviceIdFromSelectedOnlineDevice();
            HdlUdpPublicFunctions.LocateDeviceWhereItIs(currentSelectDeviceSubnetId, currentSelectDeviceDeviceId);
        }


        private void tsm1_Click(object sender, EventArgs e)
        {
            // 读取版本信息
            DataGridView dgv = new DataGridView();
            if (tabSim.SelectedTab.Name == "tabDev") dgv = this.dgOnline;
            if (dgv.RowCount == 0) return;
            if (dgv.SelectedRows == null) return;
            if (dgv.SelectedRows.Count != 1) return;
            
            if (CsConst.MyEditMode == 1) // online mode
            {
                ReadVersionOfSomeRow(dgv.CurrentRow.Index, dgv);
            }
        }

        private void ReadVersionOfSomeRow(int row, DataGridView dgv)
        {
            Cursor.Current = Cursors.WaitCursor;
            string strVersion = null;
            GetSubnetIdAndDeviceIdFromSelectedOnlineDevice();

            int DeviceType = CsConst.myOnlines[currentSelectDeviceDindex].DeviceType;
            if (CsConst.mySends.AddBufToSndList(null, 0xEFFD, currentSelectDeviceSubnetId, currentSelectDeviceDeviceId, false, true, true, CsConst.mintWIFIDeviceType.Contains(DeviceType)) == true)
            {
                byte[] bytTmp = new byte[22];
                for (int i = 0; i < 22; i++)
                {
                    bytTmp[i] = CsConst.myRevBuf[i + 25];
                }
                strVersion = HDLPF.Byte2String(bytTmp);
                if (strVersion == "MIRACLE") strVersion = "V01.01";
                if (tabSim.SelectedTab.Name == "tabDev")
                {
                    CsConst.myOnlines[currentSelectDeviceDindex].strVersion = strVersion;
                    dgOnline[8, row].Value = strVersion;
                }

            }
            Cursor.Current = Cursors.Default;
        }

        private void tsm3_Click(object sender, EventArgs e)
        {
            try
            {
                DataGridView dgv = new DataGridView();
                if (tabSim.SelectedTab.Name == "tabDev")
                {
                    dgv = this.dgOnline;
                }
                if (dgv.SelectedRows.Count != 1) return;

                Cursor.Current = Cursors.WaitCursor;
                CsConst.MyTmpName = new List<string>();
                int wdDeviceType = CsConst.myOnlines[currentSelectDeviceDindex].DeviceType;
                frmModifAddress frmNew = new frmModifAddress(currentSelectDeviceSubnetId, currentSelectDeviceDeviceId, wdDeviceType);
                if (frmNew.ShowDialog() == DialogResult.OK)
                {
                    UpdateAddressAfterModify();
                }
                Cursor.Current = Cursors.Default;
            }
            catch
            {
            }
        }

        void UpdateAddressAfterModify()
        {
            if (CsConst.MyTmpName == null || CsConst.MyTmpName.Count == 0) return;

            if (CsConst.MyTmpName != null && CsConst.MyTmpName.Count != 0)
            {
                dgOnline.SelectedCells[CsConst.MainFormSubNetIDStartFrom].Value = CsConst.MyTmpName[0];
                dgOnline.SelectedCells[CsConst.MainFormSubNetIDStartFrom + 1].Value = CsConst.MyTmpName[1];
                int intDIndex = CsConst.myOnlines[currentSelectDeviceDindex].intDIndex;

                if (CsConst.MyEditMode == 1)
                {
                    if (tabSim.SelectedTab.Name == "tabDev")
                    {
                        CsConst.myOnlines[currentSelectDeviceDindex].bytSub = Convert.ToByte(CsConst.MyTmpName[0]);
                        CsConst.myOnlines[currentSelectDeviceDindex].bytDev = Convert.ToByte(CsConst.MyTmpName[1]);
                        CsConst.myOnlines[currentSelectDeviceDindex].DevName = CsConst.MyTmpName[0] + "-" + CsConst.MyTmpName[1] + @"\"
                                                                             + CsConst.myOnlines[currentSelectDeviceDindex].DevName.Split('\\')[1].ToString();
                        HDLSysPF.UpdateAddressesFromPublicStructs(intDIndex, CsConst.myOnlines[currentSelectDeviceDindex].DevName);
                    }
                    else if (tabSim.SelectedTab.Name == "tabRF")
                    {

                    }
                }
                CsConst.MyTmpName = new List<string>();
            }
        }

        protected override bool ProcessCmdKey(ref  System.Windows.Forms.Message msg, System.Windows.Forms.Keys keyData)
        {
            if (keyData == (Keys.F2))
            {
                tsm1_Click (tsm1, null);
                return true;
            }
            else if (keyData == Keys.Delete)
            {
                DeleteOneDevice_Click(DeleteOneDevice, null);
                return true;
            }
            else if (keyData == (Keys.Control | Keys.A))
            {
                DelAll_Click(DelAll, null);
                return true;
            }
            else if (keyData == (Keys.Control | Keys.T))
            {
                CmdTest_Click(CmdTest, null);
                return true;
            }
            else if (keyData == (Keys.Control | Keys.E))
            {
                tbTemplates_Click(tbTemplates, null);
                return true;
            }
            else if (keyData == (Keys.Control | Keys.X))
            {
                tsbFunctiontemplate_Click(tsbFunctiontemplate, null);
                return true;
            }

            return base.ProcessCmdKey(ref   msg, keyData);
        }

        private void DeleteOneDevice_Click(object sender, EventArgs e)
        {
            DataGridView dgv = new DataGridView();
            int intDIndex = 0;
            GetSubnetIdAndDeviceIdFromSelectedOnlineDevice();
            if (tabSim.SelectedTab.Name == "tabDev")
            {
                dgv = this.dgOnline;
                intDIndex = Convert.ToInt16(dgv.SelectedRows[0].Cells[9].Value.ToString());
            }

            if (CsConst.MyEditMode == 0) // 表示整个工程编辑模式
            {
                //dgOnline.Rows.RemoveAt(rowindex);
            }
            else
            {
                if (tabSim.SelectedTab.Name == "tabDev")
                {
                    if (CsConst.myOnlines == null || CsConst.myOnlines.Count == 0) return;

                    CsConst.AddedDevicesIndexGroup.Remove(CsConst.myOnlines[currentSelectDeviceDindex].intDIndex);
                    CsConst.myOnlines.RemoveAt(currentSelectDeviceDindex);
                    // update the others id that displayed
                    UpdateIdAfterDeleteOneDevice(currentSelectDeviceDindex);
                }
                else if (tabSim.SelectedTab.Name == "tabRF")
                {
                }
                dgv.Rows.RemoveAt(currentSelectDeviceDindex);
            }
            toolStripStatusLabelDeviceCountShow.Text = dgv.RowCount.ToString();
        }


        void UpdateIdAfterDeleteOneDevice(int rowId)
        {
            if (dgOnline.Rows.Count == 0) return;
            if (rowId == dgOnline.RowCount -1) return;
            int currentDeleteId = Convert.ToInt32(dgOnline[2,rowId].Value.ToString());
            for (int i = rowId + 1; i < dgOnline.RowCount; i++)
            {
                dgOnline[2, i].Value = currentDeleteId.ToString();
                currentDeleteId++;
            }
        }

        private void DelAll_Click(object sender, EventArgs e)
        {
            DataGridView dgv = new DataGridView();
            if (tabSim.SelectedTab.Name == "tabDev") dgv = this.dgOnline;
            if (CsConst.MyEditMode == 0) // 表示整个工程编辑模式
            {
            }
            else
            {
                if (tabSim.SelectedTab.Name == "tabDev")
                {
                    if (CsConst.myOnlines == null || CsConst.myOnlines.Count == 0) return;
                    CsConst.myOnlines = new List<DevOnLine>();
                    CsConst.AddedDevicesIndexGroup = new List<int>();
                }
                else if (tabSim.SelectedTab.Name == "tabRF")
                {
                    
                }
                dgv.Rows.Clear();
            }
            toolStripStatusLabelDeviceCountShow.Text = dgv.RowCount.ToString();
        }

        private void autoReadVersionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataGridView dgv = new DataGridView();
            if (tabSim.SelectedTab.Name == "tabDev") dgv = this.dgOnline;
            if (dgv.Rows == null || dgv.Rows.Count == 0) return;

            for (int i = 0; i < dgv.Rows.Count; i++)
            {
                dgv.Rows[i].Selected = true;
                ReadVersionOfSomeRow(i, dgv);
            }
        }

        private void tsm2_Click(object sender, EventArgs e)
        {
            try
            {
                DataGridView dgv = new DataGridView();
                if (tabSim.SelectedTab.Name == "tabDev")
                {
                    dgv = this.dgOnline;
                }
                Cursor.Current = Cursors.WaitCursor;
                string strRemark = dgv.SelectedCells[CsConst.MainFormSubNetIDStartFrom + 3].Value.ToString();

                CsConst.MyTmpName = new List<string>();
                frmRemark frmNew = new frmRemark(currentSelectDeviceSubnetId, currentSelectDeviceDeviceId, strRemark);
                if (frmNew.ShowDialog() == DialogResult.OK)
                {
                    if (CsConst.MyTmpName == null || CsConst.MyTmpName.Count == 0) return;

                    if (CsConst.MyTmpName != null && CsConst.MyTmpName.Count != 0)
                    {
                        dgv.SelectedCells[CsConst.MainFormSubNetIDStartFrom + 3].Value = CsConst.MyTmpName[0];

                        if (CsConst.MyEditMode == 1)
                        {
                            if (tabSim.SelectedTab.Name == "tabDev")
                            {
                                CsConst.myOnlines[currentSelectDeviceDindex].DevName = CsConst.myOnlines[currentSelectDeviceDindex].DevName.Split('\\')[0].ToString() + "\\" + CsConst.MyTmpName[0];
                            }
                            else if (tabSim.SelectedTab.Name == "tabRF")
                            {
                            }
                        }
                    }
                
                    CsConst.MyTmpName = new List<string>();
                }
            }
            catch
            {
            }
        }

        private void dgOnline_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                if ((sender as DataGridView).SelectedCells == null) return;
                if (e.RowIndex < 0) return;
                string strTmp = (sender as DataGridView).SelectedRows[0].Cells[4].Value.ToString();
                if (strTmp == null || strTmp == "") return;
                Cursor.Current = Cursors.WaitCursor;
                DataGridView dgv = new DataGridView();
                if (tabSim.SelectedTab.Name == "tabDev")
                {
                    dgv = this.dgOnline;
                }
                if (dgv.SelectedRows.Count != 1) return;

                Cursor.Current = Cursors.WaitCursor;
                GetSubnetIdAndDeviceIdFromSelectedOnlineDevice();
                currentSelectDeviceDindex = Convert.ToInt32(dgv[9, e.RowIndex].Value.ToString()) - 1;
                int wdDeviceType = CsConst.myOnlines[currentSelectDeviceDindex].DeviceType;
                int DIndex = CsConst.myOnlines[currentSelectDeviceDindex].intDIndex;
                string strName = CsConst.myOnlines[currentSelectDeviceDindex].DevName;

                if (e.ColumnIndex == 3 || e.ColumnIndex == 4) tsm3_Click(tsm3, null);
                else if (e.ColumnIndex == 6) tsm2_Click(tsm2, null);
                else if (e.ColumnIndex == 8) tsm1_Click(tsm1, null);
                else
                {
                    Form frmTmp = null;
                    CsConst.isRestore = true;
                    frmTmp = HDLSysPF.OpenRightFormAccordingItsDeviceType(strName, wdDeviceType, DIndex);
                    HDLSysPF.FormShow(frmTmp, strName);
                }
            }
            catch
            {
            }
        }

        private void tsbMAC_Click(object sender, EventArgs e)
        {
            bool isOpen = HDLSysPF.IsAlreadyOpenedForm("frmMAC");

            if (isOpen == false)
            {
                frmMAC frmModify = new frmMAC();
                frmModify.Show();
            }
        }

        private void upgradeDeviceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Boolean isOpen =  HDLSysPF.IsAlreadyOpenedForm("frmUpgrade");

            if (isOpen == false)
            {
                frmUpgrade frmUpdate = new frmUpgrade();
                frmUpdate.Show();
            }
        }

        private void frmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            ControlTemplates.SaveAllGroupCommandsFrmDatabase();
            if (CsConst.myOnlines != null && CsConst.myOnlines.Count > 0) 
            {
                if (MessageBox.Show(CsConst.WholeTextsList[2403].sDisplayName, "", MessageBoxButtons.OKCancel, MessageBoxIcon.Information,
                    MessageBoxDefaultButton.Button1) == DialogResult.OK)
                {

                    string strFileName = Application.StartupPath + @"\ini\List.txt";
                    if (!File.Exists(strFileName))
                    {
                        FileStream fs1 = new FileStream(strFileName, FileMode.Create, FileAccess.ReadWrite);//创建写入文件                 
                        StreamWriter sw = new StreamWriter(fs1);
                        for (int i = 0; i < CsConst.myOnlines.Count; i++)
                        {
                            byte[] arayTmp = HDLSysPF.StructToBytes(CsConst.myOnlines[i]);
                            string str = "";
                            for (int j = 0; j < arayTmp.Length; j++)
                            {
                                str = str + arayTmp[j].ToString("X") + " ";
                            }
                            str = str.Trim();
                            sw.WriteLine(str);//开始写入值      
                        }
                        sw.Close();
                        fs1.Close();
                    }
                }
                else
                {
                    string strFileName = Application.StartupPath + @"\ini\List.txt";
                    if (File.Exists(strFileName)) File.WriteAllLines(strFileName, null);
                }
            }
            if (CsConst.calculationWorker != null) CsConst.calculationWorker.Dispose();
            this.Dispose();
        }

        private void GetDeviceList()
        {
            try
            {
                dgOnline.Rows.Clear();
                CsConst.myOnlines = new List<DevOnLine>();
                string strFileName = Application.StartupPath + @"\ini\List.txt";
                if (File.Exists(strFileName))
                {
                    StreamReader sr = new StreamReader(strFileName, Encoding.Default);
                    String line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        string str = line.ToString();
                        str = str.Trim();
                        string[] strList = str.Split(' ');
                        Byte[] arayTmp = new Byte[strList.Length];
                        for (int i = 0; i < strList.Length; i++)
                        {
                            arayTmp[i] = Convert.ToByte(strList[i], 16);
                        }
                        object obj = HDLSysPF.BytesToStruct(arayTmp, typeof(DevOnLine));
                        DevOnLine temp = (DevOnLine)obj;
                        CsConst.myOnlines.Add(temp);
                       // HDLSysPF.AddItsDefaultSettings(DeviceType, intDIndex, DevName);
                    }
                    sr.Dispose();
                }
                if (CsConst.myOnlines != null && CsConst.myOnlines.Count() > 0)
                {
                    UpdateDataGridViewOfOnlineDevice();
                }
                File.Delete(strFileName);
            }
            catch
            {
            }
        }

        private void UpdateDataGridViewOfOnlineDevice()
        {
            dgOnline.Rows.Clear();
            dgOnline.BringToFront();
            #region
            if (CsConst.myOnlines == null) return;
            if (CsConst.myOnlines.Count == 0) return;

            int[,] arayTmp = new int[CsConst.myOnlines.Count, 3];
            for (int i = 0; i < CsConst.myOnlines.Count; i++)
            {
                arayTmp[i, 0] = CsConst.myOnlines[i].bytSub;
                arayTmp[i, 1] = CsConst.myOnlines[i].bytDev;
                arayTmp[i, 2] = Convert.ToInt32(i);
            }

            #region
            for (int i = 0; i < arayTmp.Length / 3; i++)
            {
                for (int j = 0; j < (arayTmp.Length / 3) - i - 1; j++)
                {
                    if ((arayTmp[j, 0] > arayTmp[j + 1, 0]) ||
                       ((arayTmp[j, 0] == arayTmp[j + 1, 0]) &&
                        (arayTmp[j, 1] > arayTmp[j + 1, 1])))
                    {
                        int Tmp0 = arayTmp[j, 0];
                        int Tmp1 = arayTmp[j, 1];
                        int Tmp2 = arayTmp[j, 2];
                        arayTmp[j, 0] = arayTmp[j + 1, 0];
                        arayTmp[j, 1] = arayTmp[j + 1, 1];
                        arayTmp[j, 2] = arayTmp[j + 1, 2];
                        arayTmp[j + 1, 0] = Tmp0;
                        arayTmp[j + 1, 1] = Tmp1;
                        arayTmp[j + 1, 2] = Tmp2;
                    }
                }
            }
            #endregion

            for (int i = 0; i < CsConst.myOnlines.Count; i++)
            {
                DevOnLine tmp = CsConst.myOnlines[arayTmp[i, 2]];
                if (tmp.DevName == "") return;
                Image oimg1 = HDL_Buspro_Setup_Tool.Properties.Resources.OK;

                String[] strTmp = DeviceTypeList.GetDisplayInformationFromPublicModeGroup(tmp.DeviceType);
                String deviceModel = strTmp[0];
                String deviceDescription = strTmp[1];

                if (tmp.strVersion == null) tmp.strVersion = "";
                string strVersion = tmp.strVersion;
                if (strVersion == "") strVersion = "Unread";
                object[] obj = { false, oimg1, dgOnline.RowCount+1,tmp.bytSub, tmp.bytDev, deviceModel, tmp.DevName.Split('\\')[1].ToString(), 
                                   deviceDescription,strVersion, tmp.intDIndex.ToString(),""};
                dgOnline.Rows.Add(obj);
            }
            #endregion
        }

        private void frmMain_SizeChanged(object sender, EventArgs e)
        {
            HDLSysPF.AutoScale((Form)sender);
        }

        private void tbHelp_Click(object sender, EventArgs e)
        {

            //List<String> StringFrmIni = new List<string>();

            //String[] Tmp = CsConst.mstrINIDefault.IniReadAllSections();

            //foreach (String sTmp in Tmp)
            //{
            //    if (sTmp != "")
            //    {
            //        String[] sValues = CsConst.mstrINIDefault.IniReadValuesInALlSectionStr(sTmp);

            //        StringFrmIni.AddRange(sValues.ToArray());
            //    }
            //}

            //LoadControlsText.LoadButtonCoontrolTypeFromDatabaseToPublicClass();
            
            //foreach (String sTmp in StringFrmIni)
            //{
            //    if (sTmp != "")
            //    {
            //        LoadControlsText.GetIfExistedTextAlready(sTmp, sTmp.Length + 4);
            //    }
            //}
            //MessageBox.Show("done");
            //LoadControlsText.SaveDatatoDB();
            //获取全部窗体的控件 存储到数据库
            //LoadControlsText tmp = new LoadControlsText();
            //tmp.CheckAndLoadControlsNew();
            //TimeZoneInfo.GetSystemTimeZones();
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {

        }

        private void controllerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool isOpen = HDLSysPF.IsAlreadyOpenedForm("frmRemote");

            if (isOpen == false)
            {
                frmRemote frmRControl = new frmRemote();
                frmRControl.Show();
            }
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {

        }

        private void DlpImage_Click(object sender, EventArgs e)
        {
            Boolean isOpen = HDLSysPF.IsAlreadyOpenedForm("FrmEditPanlePicture");

            if (isOpen == false)
            {
                FrmEditPanlePicture frmtemp = new FrmEditPanlePicture();
                frmtemp.Show();
            } 
        }

        private void cameraToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool isOpen = HDLSysPF.IsAlreadyOpenedForm("frmCamera");

            if (isOpen == false)
            {
                frmCamera frmTest = new frmCamera();
                frmTest.Show();
            }
        }

        private void ADV1_Click(object sender, EventArgs e)
        {
            Boolean isOpen = HDLSysPF.IsAlreadyOpenedForm("FrmADVSearchDevice");

            if (isOpen == false)
            {
                FrmADVSearchDevice frmUpdate = new FrmADVSearchDevice();
                frmUpdate.Show();
            }
        }

        private void ADV3_Click(object sender, EventArgs e)
        {
            try 
            {
                CsConst.MyTmpName = new List<string>();
                frmModifAddress frmNew = new frmModifAddress();
                if (frmNew.ShowDialog() == DialogResult.OK)
                {
                    UpdateAddressAfterModify();
                }
            }
            catch
            {
            }
        }

        private void loadTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Boolean isOpen = HDLSysPF.IsAlreadyOpenedForm("frmGScenes");

            if (isOpen == false)
            {
                frmGScenes frmUpdate = new frmGScenes();
                frmUpdate.Show();
            }
        }

        private void tsbExport_Click(object sender, EventArgs e)
        {
            try
            {
                HDLSysPF.CopyCMDToPublicBufferWaitPasteOrCopyToGrid(dgOnline);

                Boolean isOpen = HDLSysPF.IsAlreadyOpenedForm("FrmBackup");

                if (isOpen == false)
                {
                    FrmBackup frmBackups = new FrmBackup();
                    frmBackups.Show();
                }
            }
            catch
            {

            }
            CsConst.isRestore = false;
            this.Enabled = true;
        }

        private void tbUpload_Click(object sender, EventArgs e)
        {
            if (CsConst.MyEditMode == 1)
            {
                string strFileName = HDLPF.OpenFileDialog("dat files (*.dat)|*.dat", "dat Files");
                if (strFileName == null) return;

                Form form = null;
                bool isOpen = true;
                foreach (Form frm in Application.OpenForms)
                {
                    if (frm.Name == "FrmRestrore")
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
                if (isOpen)
                {
                    FrmRestrore frmTmp = new FrmRestrore(strFileName);
                    frmTmp.Show();
                }
            }
        }

        private void toolColorDLP_Click(object sender, EventArgs e)
        {
            Boolean isOpen = HDLSysPF.IsAlreadyOpenedForm("frmRefreshFlash");

            if (isOpen == false)
            {
                frmRefreshFlash frmUpdate = new frmRefreshFlash();
                frmUpdate.ShowDialog();
            }
        }

        private void iRLearnerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Boolean isOpen = HDLSysPF.IsAlreadyOpenedForm("frmIRlearner");

            if (isOpen == false)
            {
                frmIRlearner frmUpdate = new frmIRlearner();
                frmUpdate.ShowDialog();
            }
        }

        private void tsmSignal_Click(object sender, EventArgs e)
        {
            Boolean isOpen = HDLSysPF.IsAlreadyOpenedForm("FrmSingnal");

            if (isOpen == false)
            {
                FrmSignal frmRControl = new FrmSignal();
                frmRControl.Show();
            }
        }

        private void searchFunctionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Boolean isOpen = HDLSysPF.IsAlreadyOpenedForm("frmFunctionLists");

            if (isOpen == false)
            {
                frmFunctionLists frmUpdate = new frmFunctionLists();
                frmUpdate.Dock = DockStyle.Right;
                frmUpdate.Show();
            }
           // HdlUdpPublicFunctions.StartSimpleSearchWay();
        }

        private void qCToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Boolean isOpen = HDLSysPF.IsAlreadyOpenedForm("frmQC");

            if (isOpen == false)
            {
                frmQC frmUpdate = new frmQC();
                frmUpdate.ShowDialog();
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {

        }

        private void tbTemplates_Click(object sender, EventArgs e)
        {
            try
            {
                Boolean isOpen = HDLSysPF.IsAlreadyOpenedForm("FrmBackup");

                if (isOpen == false)
                {
                    frmCmdTemplates frmBackups = new frmCmdTemplates(false);
                    frmBackups.Show();
                }
            }
            catch
            {

            }
            CsConst.isRestore = false;
            this.Enabled = true;
        }

        private void aboutHDLBusproSetupToolToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool isOpen = HDLSysPF.IsAlreadyOpenedForm("FrmAbout");

            if (isOpen == false)
            {
                FrmAbout frmTmp = new FrmAbout();
                frmTmp.ShowDialog();
            }
        }

        private void autoUpgradeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (autoUpgradeToolStripMenuItem.Checked == true)
            {
                ServerMain Tmp = new ServerMain();
                Tmp.Show();

                Tmp.FormClosing += new FormClosingEventHandler(UpdateDisplayInformationAccordingly);
            }
        }


        void UpdateDisplayInformationAccordingly(object sender, FormClosingEventArgs e)
        {
            try
            {
                IniFile iniFile = new IniFile(Application.StartupPath + @"\ini\Default.ini");

                String sCurrentSoftwareVersion = iniFile.IniReadValue("AutoUpgrade", "Software", "");

                if (String.Compare(sCurrentSoftwareVersion, CsConst.softwareverson) > 0)
                {
                    if (MessageBox.Show("There is a newer version avaible on network, are you going to install it now?", "", MessageBoxButtons.OKCancel, 
                        MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.OK)
                    {

                    }
                }
            }
            catch
            { }
        }

        private void tsbFunctiontemplate_Click(object sender, EventArgs e)
        {
           
        }

        private void dimProfile_Click(object sender, EventArgs e)
        {
            try
            {
                Boolean isOpen = HDLSysPF.IsAlreadyOpenedForm("frmDimTemplates");

                if (isOpen == false)
                {
                    frmDimTemplates frmUpdate = new frmDimTemplates();
                    frmUpdate.ShowDialog();
                }
            }
            catch
            { }
        }

        private void tsbFunctiontemplate_ButtonClick(object sender, EventArgs e)
        {

        }

        private void searchFunctionsToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            Boolean isOpen = HDLSysPF.IsAlreadyOpenedForm("frmFunctionLists");

            if (isOpen == false)
            {
                frmFunctionLists frmUpdate = new frmFunctionLists();
                frmUpdate.Dock = DockStyle.Right;
                frmUpdate.Show();
            }
        }

        private void exportFunctionsManuallyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Boolean isOpen = HDLSysPF.IsAlreadyOpenedForm("frmManualFunctionLists");

            if (isOpen == false)
            {
                frmManualFunctionLists frmUpdate = new frmManualFunctionLists();
                frmUpdate.Dock = DockStyle.Right;
                frmUpdate.Show();
            }
        }

        private void testToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Boolean isOpen = HDLSysPF.IsAlreadyOpenedForm("frmTest");

            if (isOpen == false)
            {
                frmTest frmUpdate = new frmTest();
                frmUpdate.Show();
            }
        }

        private void iPControlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Boolean isOpen = HDLSysPF.IsAlreadyOpenedForm("frmSA");

            if (isOpen == false)
            {
                frmSA frmUpdate = new frmSA();
                frmUpdate.Show();
            }
        }

        private void dgOnline_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void tsbUpgrade_ButtonClick(object sender, EventArgs e)
        {

        }

        private void tsbTool_Click(object sender, EventArgs e)
        {

        }
    }
}
