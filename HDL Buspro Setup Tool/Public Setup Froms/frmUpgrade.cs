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
    public partial class frmUpgrade : Form
    {
        private string MystrPath = null;

        private static int UpgradeIndex = -1;
        private static List<String> MyUpgradeDeviceInformation = null;
        private static Boolean NeedAutoEnterUpgradeMode = false;

        public frmUpgrade()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
            LoadControlsText.DisplayTextToFormWhenFirstShow(this);
        }

        private void frmUpgrade_Load(object sender, EventArgs e)
        {
            if (CsConst.myOnlines == null) return;
            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker1.WorkerSupportsCancellation = true;

            HDLSysPF.LoadDeviceListsWithDifferentSensorType(cboDevA, 255);
            if (cboDevA.Items.Count > 0)  cboDevA.SelectedIndex = 0;
            Timer.Enabled = true;
        }

        private void btnReadA_Click(object sender, EventArgs e)
        {
            if (CsConst.myOnlines == null) return;
            HintBar.Visible = false;
            tslValue.Text = "";
            tsbHint1.Text = "";
            HDLSysPF.LoadDeviceListsWithDifferentSensorType(cboDevA, 255);
            if (cboDevA.Items.Count > 0)
                cboDevA.SelectedIndex = 0;
        }

        private void cboDevA_SelectedIndexChanged(object sender, EventArgs e)
        {
            lbModelVA.Text = "";
            tbLocationA.Text = "";
            numSub.Value = Convert.ToDecimal(cboDevA.Text.Split('\\')[0].ToString().Split('-')[0].ToString());
            numDevA.Value = Convert.ToDecimal(cboDevA.Text.Split('\\')[0].ToString().Split('-')[1].ToString());
            CsConst.MyBlnWait15FE = false;
            //btnRead1A_Click(btnRead1A, null);
        }

        private void btnFindA_Click(object sender, EventArgs e)
        {
            string strFileName = HDLPF.OpenFileDialog("bin files (*.bin)|*.bin", "Bin Files");
            if (strFileName == null) return;
            tbLocationA.Text = strFileName;
        }

        private void btnRead1A_Click(object sender, EventArgs e)
        {
            if (numSub.Value == 255 || numDevA.Value == 255) return;
            //HintBar.Visible = false;
            tslValue.Text = "";
            tsbHint1.Text = "";
            Cursor.Current = Cursors.WaitCursor;
            byte bytSub = Convert.ToByte(numSub.Value);
            byte bytDev = Convert.ToByte(numDevA.Value);
            byte[] ArayTmp = null;

            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xF81A, bytSub, bytDev, false, true, true, false) == true)
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
            MystrPath = tbLocationA.Text;
            if (MystrPath == null || MystrPath == "")
            {
                MessageBox.Show(CsConst.mstrINIDefault.IniReadValue("Public", "99925", ""));
                return;
            }
            if (File.Exists(MystrPath) == false)
            {
                MessageBox.Show(CsConst.mstrINIDefault.IniReadValue("Public", "99926", ""));
                return;
            }
            HintBar.Visible = false;
            tslValue.Text = "";
            tsbHint1.Text = "";
            byte bytSub = 0;
            byte bytDev = 0;

            bytSub = Convert.ToByte(numSub.Value);
            bytDev = Convert.ToByte(numDevA.Value);
            
            if (dgvListA.Rows.Count != 0)
            {
                foreach (DataGridViewRow oDv in dgvListA.Rows)
                {
                    if (Convert.ToByte(oDv.Cells[1].Value.ToString()) == bytSub && Convert.ToByte(oDv.Cells[2].Value.ToString()) == bytDev)
                    {
                        return;
                    }
                }
            }
            object[] obj = new object[] { true, bytSub, bytDev, CsConst.mstrINIDefault.IniReadValue("public", "99927", "")
                         , tbLocationA.Text };
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
            tslValue.Text = "0%";
            HintBar.Value = 0;
            HintBar.Visible = true;
            tslHint.Visible = true;
            tsbHint1.Visible = true;
            tslValue.Visible = true;

            NeedAutoEnterUpgradeMode = true;
            if (dgvListA.Rows.Count == 0) return;
            MyUpgradeDeviceInformation = new List<string>();
            foreach (DataGridViewRow oTmp in dgvListA.Rows)
            {
                if (oTmp.Cells[0].Value.ToString().ToUpper() == "TRUE")
                {
                    CsConst.MyBlnFinish = false;
                    string UpgradeDeviceInformation = oTmp.Cells[1].Value.ToString() + "-"
                                                    + oTmp.Cells[2].Value.ToString() + "-"
                                                    + oTmp.Index;
                    MyUpgradeDeviceInformation.Add(UpgradeDeviceInformation);
                }
            }
            
            if (backgroundWorker1.IsBusy == false || backgroundWorker1.CancellationPending) backgroundWorker1.RunWorkerAsync();           
        }


        void calculationWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                cboDevM.Items.Clear();
                CsConst.MyBlnFinish = false;
                backgroundWorker1.CancelAsync();
            }
            catch
            {
            }
        }

        void calculationWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //throw new NotImplementedException();
            try
            {
                if (HintBar.ProgressBar != null)
                {
                    HintBar.ProgressBar.Value = e.ProgressPercentage;
                    tslValue.Text = HintBar.ProgressBar.Value.ToString() + "%";
                }
            }
            catch
            {
            }
        }

        void calculationWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            //下面的内容相当于线程要处理的内容。//注意：不要在此事件中和界面控件打交道
             while (worker.CancellationPending == false)
            {
                try
                {
                    if (MyUpgradeDeviceInformation == null || MyUpgradeDeviceInformation.Count == 0) 
                    {
                        worker.ReportProgress(100);
                        worker.CancelAsync();
                    }
                    else 
                    {
                        string CurrentUpgradeDeviceInformation = MyUpgradeDeviceInformation[0];
                        string[] Tmp = CurrentUpgradeDeviceInformation.Split('-');
                        CsConst.CurrentUpgradeSubnetID = Convert.ToByte(Tmp[0]);
                        CsConst.CurrentUpgradeDeviceID = Convert.ToByte(Tmp[1]); 
                        if (Tmp.Length==3&& Tmp[2] !=null && Tmp[2]!="")
                        {
                              UpgradeIndex =Convert.ToInt32(Tmp[2]);
                        }
                        
                        worker.ReportProgress(0);
                        UpgradeDeviceInList(UpgradeIndex);
                        MyUpgradeDeviceInformation.RemoveAt(0);
                    }
                }
                catch
                {
                    worker.CancelAsync();
                }
             }           
        }

        void UpgradeDeviceInList(int intRowIndex)
        {
            if (intRowIndex != -1)
            {
                if (dgvListA[4, intRowIndex].Value == null || dgvListA[4, intRowIndex].Value.ToString() == "") return;
            }

            Cursor.Current = Cursors.WaitCursor;
            byte[] ArayTmp = null;
            if (NeedAutoEnterUpgradeMode == true)
            {
                MystrPath = dgvListA[4, intRowIndex].Value.ToString();

                DateTime d1 = DateTime.Now;
                DateTime d2 = DateTime.Now;
                CsConst.mySends.AddBufToSndList(ArayTmp, 0xF81A, CsConst.CurrentUpgradeSubnetID, CsConst.CurrentUpgradeDeviceID, false, false, false, false);

                for (int i = 0; i < 4; i++)
                    CsConst.mySends.AddBufToSndList(ArayTmp, 0xF81A, CsConst.CurrentUpgradeSubnetID, CsConst.CurrentUpgradeDeviceID, false, false, false, false);

                tsbHint1.Text = CsConst.CurrentUpgradeSubnetID.ToString() + "-" + CsConst.CurrentUpgradeDeviceID.ToString();
            }

            HDLDeviceUploadFirmwareStepByStep();

            if (NeedAutoEnterUpgradeMode == true)
            {
                if (CsConst.MyBlnFinish)
                    dgvListA[3, intRowIndex].Value = CsConst.mstrINIDefault.IniReadValue("public", "99929", "");
                else if (!CsConst.MyBlnFinish)
                    dgvListA[3, intRowIndex].Value = CsConst.mstrINIDefault.IniReadValue("public", "99928", "");
                backgroundWorker1.ReportProgress(100);
            }
            else
            {
                MessageBox.Show(CsConst.mstrINIDefault.IniReadValue("public", "99929", ""));
            }
            Cursor.Current = Cursors.Default;
        }       

        void HDLDeviceUploadFirmwareStepByStep()
        {
            string str1 = "";
            try
            {
                CsConst.MyBlnFinish = false;
                FileStream fs = new FileStream(MystrPath, FileMode.Open, FileAccess.Read);//创建文件流

                byte[] source = new byte[fs.Length];

                fs.Read(source, 0, source.Length);
                fs.Flush();
                fs.Close();

                str1 = source.Length.ToString();
                DateTime d1 = DateTime.Now;
                DateTime d2 = DateTime.Now;
                UDPReceive.receiveQueueForUpgrade.Clear();
                CsConst.MyBlnWait15FE = true;
                //再次接收到长度发送文件
                #region
                while (CsConst.MyBlnFinish == false)
                {
                    while (UDPReceive.receiveQueueForUpgrade != null && UDPReceive.receiveQueueForUpgrade.Count > 0)
                    {
                        Byte[] revBuf = UDPReceive.receiveQueueForUpgrade.Dequeue();
                        if (revBuf[17] == CsConst.CurrentUpgradeSubnetID && revBuf[18] == CsConst.CurrentUpgradeDeviceID)
                        {
                            int DeviceType = revBuf[19] * 256 + revBuf[20];
                            int intMaxPacket = 0;
                            int intPacketSize = 0;

                            if (DeviceType == 65498)
                                intPacketSize = 1024;
                            else
                                intPacketSize = 64;

                            if (source.Length % intPacketSize != 0) intMaxPacket = Convert.ToInt32(source.Length / intPacketSize + 1);
                            else intMaxPacket = Convert.ToInt32(source.Length / intPacketSize);

                            //回复文件大小
                            int UpgradeID = revBuf[25] * 256 + revBuf[26];
                            if (UpgradeID == 0)
                            {
                                #region
                                Byte[] ArayTmp = new Byte[4];
                                ArayTmp[0] = Convert.ToByte((intMaxPacket & 0xFF000000) >> 24);
                                ArayTmp[1] = Convert.ToByte((intMaxPacket & 0xFF0000) >> 16);
                                ArayTmp[2] = Convert.ToByte((intMaxPacket & 0xFF00) >> 8);
                                ArayTmp[3] = Convert.ToByte(intMaxPacket & 0xFF);

                                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x15FF, CsConst.CurrentUpgradeSubnetID, CsConst.CurrentUpgradeDeviceID, (intPacketSize == 1024), false, false, false) == true)
                                {
                                    UDPReceive.ClearQueueDataForUpgrade();
                                }

                                #endregion
                            }
                            else if (UpgradeID != 0xF8F8)
                            {
                                byte[] ArayFirmware = new byte[intPacketSize + 2];
                                ArayFirmware[0] = Convert.ToByte((UpgradeID & 0xFF00) >> 8);
                                ArayFirmware[1] = Convert.ToByte(UpgradeID & 0xFF);

                                if (UpgradeID < intMaxPacket) Array.Copy(source, (UpgradeID - 1) * intPacketSize, ArayFirmware, 2, intPacketSize);
                                else if (UpgradeID == intMaxPacket)
                                {
                                    if (source.Length % intPacketSize == 0)
                                        Array.Copy(source, (UpgradeID - 1) * intPacketSize, ArayFirmware, 2, intPacketSize);
                                    else
                                    {
                                        ArayFirmware = new byte[source.Length % intPacketSize + 2];
                                        ArayFirmware[0] = Convert.ToByte((UpgradeID & 0xFF00) >> 8);
                                        ArayFirmware[1] = Convert.ToByte(UpgradeID & 0xFF);
                                        Array.Copy(source, (UpgradeID - 1) * intPacketSize, ArayFirmware, 2, source.Length % intPacketSize);
                                    }
                                }

                                CsConst.mySends.AddBufToSndList(ArayFirmware, 0x15FF, CsConst.CurrentUpgradeSubnetID, CsConst.CurrentUpgradeDeviceID, (intPacketSize > 80), false, false, false);
                                UDPReceive.ClearQueueDataForUpgrade();

                                if (backgroundWorker1 != null && backgroundWorker1.IsBusy) backgroundWorker1.ReportProgress(UpgradeID * 100 / intMaxPacket);
                            }
                            else CsConst.MyBlnFinish = true;

                            d1 = DateTime.Now;
                        }
                    }
                }
                #endregion
            }
            catch
            {

            }
        }

        private void btnReadM_Click(object sender, EventArgs e)
        {
            cboDevM.Items.Clear();
            Timer.Enabled = true;
            lbModelVM.Text="";
            tsbHint1.Text = "";
            HintBar.Visible = false;
            cboDevM.Items.Clear();
            lbModelVM.Text = "";
            tslValue.Text = "";
        }

        private void btnUpdateM_Click(object sender, EventArgs e)
        {
            Timer.Enabled = false;
            UDPReceive.receiveQueueForUpgrade.Clear();

            MystrPath = tbLocM.Text;
            if (MystrPath == null || MystrPath == "")
            {
                MessageBox.Show(CsConst.mstrINIDefault.IniReadValue("Public", "99925", ""));
                return;
            }
            if (File.Exists(MystrPath) == false)
            {
                MessageBox.Show(CsConst.mstrINIDefault.IniReadValue("Public", "99926", ""));
                return;
            }
            tslValue.Text = "0%";
            HintBar.Value = 0;
            HintBar.Visible = true;
            tslValue.Visible = true;
           
           if (backgroundWorker1.IsBusy == false || backgroundWorker1.CancellationPending) backgroundWorker1.RunWorkerAsync();
           // ReadThread();  //增加线程，使得当前窗体的任何操作不被限制
        }

        private void btnOpenM_Click(object sender, EventArgs e)
        {
            string strFileName = HDLPF.OpenFileDialog("bin files (*.bin)|*.bin","Bin Files");
            if (strFileName == null) return;
            tbLocM.Text = strFileName;
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
             Timer.Enabled =(tabControl1.SelectedIndex == 1);
             CsConst.MyBlnWait15FE = Timer.Enabled;
        }

        private void frmUpgrade_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                CsConst.isStopDealImageBackground = true;
                if (backgroundWorker1 != null && backgroundWorker1.IsBusy)
                {
                    backgroundWorker1.CancelAsync();
                    backgroundWorker1 = null;
                }
                this.Dispose();
            }
            catch
            {
            }
        }

        private void cboDevM_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboDevM.Items.Count > 0 && cboDevM.SelectedIndex >= 0)
            {
                //lbModelVM.Text = CsConst.arayUpgrade[cboDevM.SelectedIndex].Split('-')[4].ToString();
                tsbHint1.Text = cboDevM.Text;
                string DeviceName = cboDevM.Text;
                CsConst.MyBlnWait15FE = false;
                MyUpgradeDeviceInformation = new List<string>();
                MyUpgradeDeviceInformation.Add(DeviceName);
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 1)
            {
                if (cboDevM.SelectedIndex == 0) Timer.Enabled = false;
                while (UDPReceive.receiveQueueForUpgrade != null && UDPReceive.receiveQueueForUpgrade.Count > 0)
                {
                    Byte[] revBuf = UDPReceive.receiveQueueForUpgrade.Dequeue();
                    string str = "";
                    str = revBuf[17].ToString() + "-" + revBuf[18].ToString();//子网ID 设备ID
                    if (!cboDevM.Items.Contains(str))
                    {
                        cboDevM.Items.Add(str);
                    }
                }

            }
            if (cboDevM.Items.Count > 0 && cboDevM.Text != "")
            {
                cboDevM.SelectedIndex = 0;
                Timer.Enabled = false;
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

        private void lbDevA_Click(object sender, EventArgs e)
        {

        }

    }
}
