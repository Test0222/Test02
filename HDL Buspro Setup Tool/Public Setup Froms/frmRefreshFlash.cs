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
    public partial class frmRefreshFlash : Form
    {
        private static int MbytFlag = -1;
        private static int MyintCurTimes = -1;

        private static string MystrPath = null;
        private static byte MybytSubID = 255;
        private static byte MybytDevID = 255;

        public frmRefreshFlash()
        {
            InitializeComponent();
            if (CsConst.iLanguageId == 1)
            {
   
            }

        }

        private void frmUpgrade_Load(object sender, EventArgs e)
        {
            if (CsConst.myOnlines == null) return;
            cboDevA.Items.Clear();
            cboDevM.Items.Clear();
            foreach (DevOnLine oTmp in CsConst.myOnlines)
            {
                if (EnviroDLPPanelDeviceTypeList.HDLEnviroDLPPanelDeviceTypeList.Contains(oTmp.DeviceType))
                {
                    cboDevA.Items.Add(oTmp.DevName);
                    cboDevM.Items.Add(oTmp.DevName);
                }
            }
            if (cboDevA.Items.Count > 0)
            {
                cboDevA.SelectedIndex = 0;
                cboDevM.SelectedIndex = 0;
            }
        }

        private void btnReadA_Click(object sender, EventArgs e)
        {
            if (CsConst.myOnlines == null) return;
            HintBar.Visible = false;
            tsbHint1.Text = "";
            cboDevA.Items.Clear();
            cboDevM.Items.Clear();
            foreach (DevOnLine oTmp in CsConst.myOnlines)
            {
                if (EnviroDLPPanelDeviceTypeList.HDLEnviroDLPPanelDeviceTypeList.Contains(oTmp.DeviceType))
                {
                    cboDevA.Items.Add(oTmp.DevName);
                    cboDevM.Items.Add(oTmp.DevName);
                }
            }
            if (cboDevA.Items.Count > 0)
            {
                cboDevA.SelectedIndex = 0;
                cboDevM.SelectedIndex = 0;
            }
        }

        private void cboDevA_SelectedIndexChanged(object sender, EventArgs e)
        {
            lbModelVA.Text = "";
            tbLocationA.Text = "";
            numSub.Value = Convert.ToDecimal(cboDevA.Text.Split('\\')[0].ToString().Split('-')[0].ToString());
            numDevA.Value = Convert.ToDecimal(cboDevA.Text.Split('\\')[0].ToString().Split('-')[1].ToString());
        }

        private void btnFindA_Click(object sender, EventArgs e)
        {                                             
            string strFileName = HDLPF.OpenFileDialog("raw files (*.raw)|*.raw", "raw Files");
            if (strFileName == null) return;
            tbLocationA.Text = strFileName;
        }

        private void btnRead1A_Click(object sender, EventArgs e)
        {
            if (numSub.Value == 255 || numDevA.Value == 255) return;

            Cursor.Current = Cursors.WaitCursor;
            byte bytSub = Convert.ToByte(numSub.Value);
            byte bytDev = Convert.ToByte(numDevA.Value);
            byte[] ArayTmp = null;
            MbytFlag = 1;
            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xF81A, bytSub, bytDev, false, false,false,false) == true)
            {
                byte[] arayRemark = new byte[20];
                for (int intI = 0; intI < 20; intI++) { arayRemark[intI] = CsConst.myRevBuf[25 + intI]; }
                lbModelVA.Text = HDLPF.Byte2String(arayRemark);
                CsConst.myRevBuf = new byte[1200];
            }
            Cursor.Current = Cursors.Default;
        }

        private void btnAddA_Click(object sender, EventArgs e)
        {
            if (tbLocationA.Text == null || tbLocationA.Text == "") return;
            //if (MbytFlag == -1) return;
            if (tbLocationA.Text == null || tbLocationA.Text == "") return;

            byte bytSub = 0;
            byte bytDev = 0;
            if (MbytFlag == 0) // 从在线设备选择
            {
                bytSub = CsConst.myOnlines[cboDevA.SelectedIndex].bytSub;
                bytDev = CsConst.myOnlines[cboDevA.SelectedIndex].bytDev;
            }
            else
            {
                bytSub = Convert.ToByte(numSub.Value);
                bytDev = Convert.ToByte(numDevA.Value);
            }

            if (dgvListA.Rows.Count != 0)
            {
                foreach (DataGridViewRow oDv in dgvListA.Rows)
                {
                    if (Convert.ToByte(oDv.Cells[1].Value.ToString()) == bytSub &&
                        Convert.ToByte(oDv.Cells[2].Value.ToString()) == bytDev &&
                        oDv.Cells[4].Value.ToString() == tbLocationA.Text.Trim())
                    {
                        //oDv.Cells[3].Value = "Waiting";
                        //oDv.Cells[4].Value = tbLocationA.Text;
                        return;
                    }
                }
            }
            object[] obj =  new object[] { true,bytSub,bytDev, "Waiting", tbLocationA.Text };
            dgvListA.Rows.Add(obj); 
        }

        private void numSub_ValueChanged(object sender, EventArgs e)
        {
            lbModelVA.Text = "";
            tbLocationA.Text = "";
        }

        private void numDevA_ValueChanged(object sender, EventArgs e)
        {
            lbModelVA.Text = "";
            tbLocationA.Text = "";
        }

        private void chbOption_CheckedChanged(object sender, EventArgs e)
        {
            if (dgvListA.Rows.Count == 0) return;

            for (int intI = 0; intI < dgvListA.Rows.Count; intI++)
            {
                dgvListA[0, intI].Value = chbOption.Checked;
            }
        }

        private void btnUpdateA_Click(object sender, EventArgs e)
        {
            tslHint.Visible = true;
            tsbHint1.Visible = true;
            CsConst.MyStartORAskMore = 0;
            ReadThread();  //增加线程，使得当前窗体的任何操作不被限制
            
            HintBar.Visible = true;
            tslHint.Visible = true;
        }

        private void ReadThread()
        {
            Cursor.Current = Cursors.WaitCursor;
            CsConst.calculationWorker = new BackgroundWorker();
            CsConst.calculationWorker.DoWork += new DoWorkEventHandler(calculationWorker_DoWork);
            CsConst.calculationWorker.ProgressChanged += new ProgressChangedEventHandler(calculationWorker_ProgressChanged);
            CsConst.calculationWorker.WorkerReportsProgress = true;
            CsConst.calculationWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(calculationWorker_RunWorkerCompleted);
            CsConst.calculationWorker.RunWorkerAsync();
            Cursor.Current = Cursors.Default;
        }

        void calculationWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            HintBar.Visible = false;
            tsbHint1.Visible = false;
            tslHint.Visible = false;
            CsConst.MyStartORAskMore = 0;
        }

        void calculationWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //throw new NotImplementedException();
            HintBar.ProgressBar.Value = e.ProgressPercentage;
            tsbHint1.Text = HintBar.ProgressBar.Value.ToString() + "%";
            if (e.UserState != null)
            {
                tslHint.Text = (string)e.UserState;
            }
         
        }

        void calculationWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            //throw new NotImplementedException();
            CsConst.calculationWorker.ReportProgress(0);
            if (tabControl1.SelectedIndex == 0)
            {
                if (dgvListA.Rows.Count == 0) return;
                foreach (DataGridViewRow oTmp in dgvListA.Rows)
                {
                    if (oTmp.Cells[0].Value.ToString().ToUpper() == "TRUE")
                    {
                        CsConst.MyBlnWait15FE = false;                        
                        UpgradeDeviceInList(oTmp.Index);
                    }
                }
            }
            else if (tabControl1.SelectedIndex == 1)
            {
                ReadFlashFirmwareStepByStep();
            }
            MyintCurTimes = -1;
            CsConst.MyUpgradeSubNetID = -1;
            CsConst.MyUpgradeDeviceID = -1;
        }

        void UpgradeDeviceInList(int intRowIndex)
        {
            if (intRowIndex == -1) return;
            if (dgvListA[1, intRowIndex].Value == null || dgvListA[1, intRowIndex].Value.ToString() == "") return;
            if (dgvListA[2, intRowIndex].Value == null || dgvListA[2, intRowIndex].Value.ToString() == "") return;
            if (dgvListA[4, intRowIndex].Value == null || dgvListA[4, intRowIndex].Value.ToString() == "") return;

            Cursor.Current = Cursors.WaitCursor;
            byte bytSub =  Convert.ToByte(dgvListA[1, intRowIndex].Value .ToString());
            byte bytDev =  Convert.ToByte(dgvListA[2, intRowIndex].Value .ToString());
            MystrPath = dgvListA[4, intRowIndex].Value.ToString();
            MybytSubID = bytSub;
            MybytDevID = bytDev;

            //增加调频
            if (HDLSysPF.ModifyDeviceBaudRateVersion(MybytSubID, MybytDevID, true) == true)
            {
                HDLSysPF.ModifyDeviceBaudRateVersion(MybytSubID, 0, true);
            }

            if (UploadFirmwareStepByStep())
                dgvListA[3, intRowIndex].Value = CsConst.mstrINIDefault.IniReadValue("public", "99929", "");
            else
                dgvListA[3, intRowIndex].Value = CsConst.mstrINIDefault.IniReadValue("public", "99928", "");

            //增加调频
            //if (HDLSysPF.ModifyDeviceBaudRateVersion(MybytSubID, MybytDevID, false) == true)
            //{
            //    HDLSysPF.ModifyDeviceBaudRateVersion(MybytSubID, 0, false);
            //}

            Cursor.Current = Cursors.Default;
        }

        private bool UploadFirmwareStepByStep()
        {
            if (MystrPath == null || MystrPath == "") return false;
            if (File.Exists(MystrPath) == false) return false;
            bool Result = true;
            FileStream fs = new FileStream(MystrPath, FileMode.Open, FileAccess.Read);//创建文件流

            byte[] source = new byte[fs.Length];

            fs.Read(source, 0, source.Length);
            fs.Flush();
            fs.Close();

            int intMaxPacket = -1;
            int intPacketSize = 64;
            int intStartAddress = Convert.ToInt32(StartPos.Value);
            #region
            if (source.Length % intPacketSize != 0) intMaxPacket = Convert.ToInt32((source.Length) / intPacketSize + 1);
            else intMaxPacket = Convert.ToInt32((source.Length) / intPacketSize);

            //再次接收到长度发送文件        
            for (int i = 0; i < intMaxPacket; i++)
            {
                Byte[] ArayFirmware = new byte[67];
                if (i != intMaxPacket - 1)
                {
                    ArayFirmware = new byte[67];
                    int intTmpAddress = intStartAddress + i * 64;
                    ArayFirmware[0] = Convert.ToByte((intTmpAddress & 0xFF0000) >> 16);
                    ArayFirmware[1] = Convert.ToByte((intTmpAddress & 0xFF00) >> 8);
                    ArayFirmware[2] = Convert.ToByte((intTmpAddress % 256));
                    if (i != intMaxPacket - 1) Array.Copy(source, i * intPacketSize, ArayFirmware, 3, intPacketSize);
                }
                else
                {
                    ArayFirmware = new byte[3 + source.Length % intPacketSize];
                    int intTmpAddress = intStartAddress + i * 64;
                    ArayFirmware[0] = Convert.ToByte((intTmpAddress & 0xFF0000) >> 16);
                    ArayFirmware[1] = Convert.ToByte((intTmpAddress & 0xFF00) >> 8);
                    ArayFirmware[2] = Convert.ToByte((intTmpAddress % 256));
                    Array.Copy(source, i * intPacketSize, ArayFirmware, 3, source.Length % intPacketSize);
                }

                if (CsConst.mySends.AddBufToSndList(ArayFirmware, 0xE46A, MybytSubID, MybytDevID, (intPacketSize > 80), false, true,false) == true)
                {

                }
                else
                {
                    Result = false;
                    break;
                }
                if (MyintCurTimes == intMaxPacket)
                {
                }
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(i * 100 / intMaxPacket);
            }
            #endregion
            CsConst.MyStartORAskMore = -1;
            return Result;
        }

        void ReadFlashFirmwareStepByStep()
        {
            if (MystrPath == null || MystrPath == "") return;
            if (File.Exists(MystrPath) == false) return;

           // FileStream fs = new FileStream(MystrPath, FileMode.Open, FileAccess.Write);//创建文件流

            int intMaxPacket = -1;
            int intPacketSize = 64;
            int intStartAddress = Convert.ToInt32(StartAddress.Text.ToString());
            int intEndAddress = Convert.ToInt32(EndAddress.Text.ToString());
            byte[] source = new byte[intEndAddress - intStartAddress];
            #region
            if ((intEndAddress - intStartAddress) % intPacketSize != 0) intMaxPacket = Convert.ToInt32((intEndAddress - intStartAddress) / intPacketSize + 1);
            else intMaxPacket = Convert.ToInt32((intEndAddress - intStartAddress) / intPacketSize);

            //再次接收到长度发送文件        
            for (int i = 0; i < intMaxPacket; i++)
            {
                Byte[] ArayFirmware = new byte[4];
                int intTmpAddress = intStartAddress + i * 64;
                ArayFirmware[0] = Convert.ToByte((intTmpAddress & 0xFF0000) >> 16);
                ArayFirmware[1] = Convert.ToByte((intTmpAddress & 0xFF00) >> 8);
                ArayFirmware[2] = Convert.ToByte((intTmpAddress % 256));
                if (i != intMaxPacket - 1) ArayFirmware[3] = 64;
                else if ((intEndAddress - intStartAddress) % intPacketSize != 0)
                    ArayFirmware[3] = Convert.ToByte((intEndAddress - intStartAddress) % intPacketSize);
                else ArayFirmware[3] = 64;

                if (CsConst.mySends.AddBufToSndList(ArayFirmware, 0xE468, MybytSubID, MybytDevID, (intPacketSize > 80), false, true,false) == true)
                {
                    Array.Copy(CsConst.myRevBuf, 28, source, i * intPacketSize, ArayFirmware[3]);
                }
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(i * 100 / intMaxPacket);
            }
            //BinaryWriter writer = new BinaryWriter(fs);
           // writer.Write(source, 0, source.Length);
            File.WriteAllBytes(MystrPath, source);
           // fs.Close();
            #endregion
            CsConst.MyStartORAskMore = -1;
        }

        private void btnReadM_Click(object sender, EventArgs e)
        {
            if (CsConst.myRevBuf == null) return;
            CsConst.MyBlnWait15FE = true;
            CsConst.WaitMore = false;

            if (cboDevM.Text != null && cboDevM.Text != "")
            {
                MybytSubID = CsConst.myOnlines[cboDevM.SelectedIndex].bytSub;
                MybytDevID = CsConst.myOnlines[cboDevM.SelectedIndex].bytDev;
              //  cboDevM.SelectedIndex = 0;
                // 1 select a path
                string strFileName = HDLPF.SaveFileDialogAccordingly("Save To", "raw files (*.raw)|*.raw");
                if (strFileName == null) return;
                
                MystrPath = strFileName;
                tbLocM.Text = MystrPath;
            }
        }

        private void btnUpdateM_Click(object sender, EventArgs e)
        {
            if (MystrPath == null || MystrPath == "") return;
            CsConst.WaitMore = true;
            //UploadFirmwareStepByStep();
            tslHint.Visible = true;
            tsbHint1.Visible = true;
            HintBar.Visible = true;
            ReadThread();  //增加线程，使得当前窗体的任何操作不被限制
        }

        private void btnOpenM_Click(object sender, EventArgs e)
        {
            string strFileName = HDLPF.OpenFileDialog("bin files (*.bin)|*.bin","Bin Files");
            if (strFileName == null) return;
            tbLocM.Text = strFileName;
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy)
            {
                tabControl1.SelectedIndex = (tabControl1.SelectedIndex + 1) % 2;
                return;
            }
            CsConst.MyBlnWait15FE = (tabControl1.SelectedIndex == 1);
        }

        private void StartAddress_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar) || e.KeyChar == 8)
                e.Handled = false;
            else
                e.Handled = true;
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

        private void btnHelp_Click(object sender, EventArgs e)
        {
            bool isOpen = HDLSysPF.IsAlreadyOpenedForm("FrmRaw");

            if (isOpen == false)
            {
                FrmRaw frmModify = new FrmRaw();
                frmModify.Show();
            }
        }

    }
}
