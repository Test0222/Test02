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
    public partial class frmQC : Form
    {
        private string MystrPath = null;

        private static int UpgradeIndex = -1;
        private static Byte bSubNetId;
        private static Byte bDeviceId;
        private static int iMyDeviceType;
        private static List<String> MyUpgradeDeviceInformation;

        public frmQC()
        {
            InitializeComponent();
            LoadControlsText.DisplayTextToFormWhenFirstShow(this);
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void frmUpgrade_Load(object sender, EventArgs e)
        {
            if (CsConst.myOnlines == null) return;
            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker1.WorkerSupportsCancellation = true;

            HDLSysPF.LoadDeviceListsWithDifferentSensorType(cboDevA, 255);
            if (cboDevA.Items.Count > 0)  cboDevA.SelectedIndex = 0;
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
            if (CsConst.myOnlines ==null || CsConst.myOnlines.Count ==0) return;
            tbLocationA.Text = "";
            numSub.Value = Convert.ToDecimal(cboDevA.Text.Split('\\')[0].ToString().Split('-')[0].ToString());
            numDevA.Value = Convert.ToDecimal(cboDevA.Text.Split('\\')[0].ToString().Split('-')[1].ToString());
            bSubNetId = Convert.ToByte(numSub.Value.ToString());
            bDeviceId = Convert.ToByte(numDevA.Value.ToString());
            iMyDeviceType = CsConst.myOnlines[cboDevA.SelectedIndex].DeviceType;
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
            tslValue.Text = "";
            tsbHint1.Text = "";
            Cursor.Current = Cursors.WaitCursor;
            byte bytSub = Convert.ToByte(numSub.Value);
            byte bytDev = Convert.ToByte(numDevA.Value);
            try
            {
                tslValue.Text = "0%";
                HintBar.Value = 0;
                HintBar.Visible = true;
                tslHint.Visible = true;
                tsbHint1.Visible = true;
                tslValue.Visible = true;

                if (dgvListA.Rows.Count == 0) return;
                MyUpgradeDeviceInformation = new List<string>();
                if (btnRead1A.Text == CsConst.mstrINIDefault.IniReadValue("Public", "99812", ""))
                {
                    btnRead1A.Text = CsConst.mstrINIDefault.IniReadValue("Public", "00036", "");
                }
                else if (btnRead1A.Text == CsConst.mstrINIDefault.IniReadValue("Public", "00036", ""))
                {
                    btnRead1A.Text = CsConst.mstrINIDefault.IniReadValue("Public", "99812", "");
                }

                for (int i=0; i < dgvListA.RowCount; i++) // (DataGridViewRow oTmp in dgvListA.Rows)
                {
                    dgvListA.Rows[i].Cells[3].Value = "";
                    if (dgvListA.Rows[i].Cells[0].Value.ToString().ToUpper() == "TRUE")
                    {
                        CsConst.MyBlnFinish = false;
                        string UpgradeDeviceInformation = dgvListA.Rows[i].Cells[1].Value.ToString() + "-"
                                                        + dgvListA.Rows[i].Cells[2].Value.ToString() + "-"
                                                        + dgvListA.Rows[i].Index;
                        MyUpgradeDeviceInformation.Add(UpgradeDeviceInformation);
                    }
                }

                if (backgroundWorker1.IsBusy == false || backgroundWorker1.CancellationPending) backgroundWorker1.RunWorkerAsync();
            }
            catch
            { }
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
            tbLocationA.Text = "";
        }

        private void numDevA_ValueChanged(object sender, EventArgs e)
        {
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
         
        }


        void calculationWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
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
                        CsConst.CurrentUpgradeSubnetID = bSubNetId;
                        CsConst.CurrentUpgradeDeviceID = bDeviceId; 
                        if (Tmp[2] !=null && Tmp[2]!="")
                        {
                              UpgradeIndex =Convert.ToInt32(Tmp[2]);
                        }
                        
                        worker.ReportProgress(0);
                        CheckFunctionsOneByOneInList(UpgradeIndex);
                        MyUpgradeDeviceInformation.RemoveAt(0);
                        worker.ReportProgress(UpgradeIndex * 10);
                    }
                }
                catch
                {
                    worker.CancelAsync();
                }
             }           
        }

        void CheckFunctionsOneByOneInList(int intRowIndex)
        {
            if (intRowIndex != -1)
            {
                if (dgvListA[2, intRowIndex].Value == null || dgvListA[2, intRowIndex].Value.ToString() == "") return;
            }

            try
            {
                Cursor.Current = Cursors.WaitCursor;
                HDLDeviceCheckFunctionsStepByStep(intRowIndex);
            }
            catch
            { }
           
            Cursor.Current = Cursors.Default;
        }

        void HDLDeviceCheckFunctionsStepByStep(int intRowIndex)
        {
            string str1 = "";
            try
            {
                CsConst.MyBlnFinish = false;
                DateTime d1 = DateTime.Now;
                DateTime d2 = DateTime.Now;
                CsConst.MyBlnWait15FE = true;
                #region
                Boolean bIsSuccess = false;
                String sTestResult = "通过";
                String sMacInformation = "";
                String sHardwareInformation = "";
                switch (intRowIndex)
                {
                    case 0: bIsSuccess = HdlUdpPublicFunctions.LocateDeviceWhereItIs(bSubNetId,bDeviceId); break; // 自动定位
                    case 1: bIsSuccess = HdlUdpPublicFunctions.CheckDeviceIfCouldBeFound(0x000E, bSubNetId, bDeviceId); break; //搜索设备
                    case 2: bIsSuccess = HdlUdpPublicFunctions.CheckDeviceIfCouldBeFound(0xE548, bSubNetId, bDeviceId); break;  // 简易编程搜索
                    case 3: 
                        CsConst.FastSearch = false;   // 读取版本信息
                        String sTmpVersion = HDLSysPF.ReadDeviceVersion(bSubNetId, bDeviceId,true);
                        if (sTmpVersion != null && sTmpVersion != "")
                        {
                            bIsSuccess = true;
                            sTestResult += " " + sTmpVersion;
                        }
                            break;
                    case 4: 
                        bIsSuccess = HDLSysPF.ModifyDeviceMainRemark(bSubNetId, bDeviceId, "测试备注 + " + bDeviceId.ToString(), -1);
                        if (bIsSuccess == true) sTestResult += "测试备注 + " + bDeviceId.ToString();
                        break; // 修改备注
                    case 5: 
                        sMacInformation = HdlUdpPublicFunctions.ReadDeviceMacInformation(bSubNetId, bDeviceId,false);  // 根据MAC修改地址
                        if (sMacInformation != "")
                        {
                             bIsSuccess = HdlUdpPublicFunctions.ModifyDeviceAddressWithTwoDifferentways(bSubNetId,bDeviceId,sMacInformation,
                                                           iMyDeviceType,bSubNetId,(Byte)(bDeviceId + 1),0);
                             if (bIsSuccess == true) bDeviceId = (Byte)(bDeviceId + 1);
                        }
                        break;
                    case 6:    // 一端口自动分配地址
                        sMacInformation = HdlUdpPublicFunctions.ReadDeviceMacInformation(bSubNetId, bDeviceId,false);  // 根据MAC修改地址
                        if (sMacInformation != "")
                        {
                            bIsSuccess = HdlUdpPublicFunctions.ModifyDeviceAddressAutomaticallyWithIpModule(bSubNetId, bDeviceId, sMacInformation,
                                                           iMyDeviceType, bSubNetId, (Byte)(bDeviceId + 1));
                            if (bIsSuccess == true) bDeviceId = (Byte)(bDeviceId + 1);
                        }
                        break;
                    case 7: 
                        DialogResult result = MessageBox.Show("请确认已经按住按键并且LED变红！", "问题",   // 按住按键修改地址
                                          MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                        if (result == DialogResult.No)
                        {
                            sTestResult = "测试未通过";
                        }
                        else if (result == DialogResult.Yes)
                        {
                            bIsSuccess = HdlUdpPublicFunctions.ModifyDeviceAddressWithTwoDifferentways(bSubNetId, bDeviceId, "",
                                                                                     iMyDeviceType, bSubNetId, (Byte)(bDeviceId + 1), 1);
                            if (bIsSuccess == true) bDeviceId = (Byte)(bDeviceId + 1);
                        }
                        break;
                    case 9: // 恢复出厂设置
                        sMacInformation = HdlUdpPublicFunctions.ReadDeviceMacInformation(bSubNetId, bDeviceId,false);  // 根据MAC修改地址
                        if (sMacInformation != "")
                        {
                            bIsSuccess = HdlUdpPublicFunctions.IntialDeviceFactorySettings(bSubNetId, bDeviceId, sMacInformation, iMyDeviceType);
                            if (bIsSuccess == true) bDeviceId = (Byte)(bDeviceId + 1);
                        }
                        break;
                    case 8: // 读取硬件信息
                        sHardwareInformation = HdlUdpPublicFunctions.ReadDeviceHardwareInformation(bSubNetId, bDeviceId);  // 根据MAC修改地址
                        if (sHardwareInformation != "")
                        {
                            bIsSuccess = true;
                        }
                        break;
                }

                if (intRowIndex == 0 && bIsSuccess == true)
                {
                    DialogResult result = MessageBox.Show("请确认是否可以找到设备！", "问题", 
                                          MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                    if (result == DialogResult.No)
                    {
                        sTestResult = "测试未通过";
                    }
                }
                else if (intRowIndex == 4 && bIsSuccess == true)
                {
                    sTestResult = "请在测试完成后断电确认是否保存";
                }
                else if (intRowIndex == 9 && bIsSuccess == true) //恢复出厂后地址修改
                {
                    CsConst.myOnlines[cboDevA.SelectedIndex].bytSub = bSubNetId;
                    CsConst.myOnlines[cboDevA.SelectedIndex].bytDev = bDeviceId;
                    CsConst.myOnlines[cboDevA.SelectedIndex].DevName = bSubNetId.ToString() + "-" + bDeviceId.ToString() + "\\" + "测试备注 + " + bDeviceId.ToString().ToString();
                    numDevA.Value = bDeviceId;
                    cboDevA.Items[cboDevA.SelectedIndex] = CsConst.myOnlines[cboDevA.SelectedIndex].DevName;                    
                }
                else if (bIsSuccess == false)
                {
                    sTestResult = "测试未通过";
                }
                List<String> Tmp = new List<string> { sTestResult, sHardwareInformation };
                HdlUdpPublicFunctions.UpdateDataGridWhenNeedsRefreshDisplay(dgvListA, Tmp, intRowIndex, 3, bIsSuccess);
                #endregion
            }
            catch
            {

            }
        }


        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {

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

        }

        private void lbDevA_Click(object sender, EventArgs e)
        {

        }

        private void frmQC_Shown(object sender, EventArgs e)
        {
            AddPublicDetectFunctionsListToGrid();
        }


        void AddPublicDetectFunctionsListToGrid()
        {
            dgvListA.Rows.Clear();
            List<String> sFunctionLists = new List<string>() { "设备定位","搜索设备","简易编程搜索","读取设备版本","修改设备备注",
                                                               "根据MAC修改设备ID","自动分配地址","按键修改ID","硬件信息读取","恢复出厂设置"};//,
                                                              // "最后一次修改记录","自动升级","手动升级"};
            try
            {
                foreach (String sTmpFunction in sFunctionLists)
                {
                    Object[] Tmp = new Object[] {true, dgvListA.RowCount+1,sTmpFunction,"",""};
                    dgvListA.Rows.Add(Tmp);
                }
            }
            catch
            { }
        }

    }
}
