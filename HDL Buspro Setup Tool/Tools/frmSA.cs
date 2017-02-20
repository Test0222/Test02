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
    public partial class frmSA : Form
    {
        private string MystrPath = null;

        private static int UpgradeIndex = -1;
        private static Byte bSubNetId;
        private static Byte bDeviceId;
        private static int iMyDeviceType;
        private static List<String> MyUpgradeDeviceInformation;

        public frmSA()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void frmUpgrade_Load(object sender, EventArgs e)
        {
            if (CsConst.myOnlines == null) return;

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
            numSub.Value = Convert.ToDecimal(cboDevA.Text.Split('\\')[0].ToString().Split('-')[0].ToString());
            numDevA.Value = Convert.ToDecimal(cboDevA.Text.Split('\\')[0].ToString().Split('-')[1].ToString());
            bSubNetId = Convert.ToByte(numSub.Value.ToString());
            bDeviceId = Convert.ToByte(numDevA.Value.ToString());
            iMyDeviceType = CsConst.myOnlines[cboDevA.SelectedIndex].DeviceType;
        }

        private void btnFindA_Click(object sender, EventArgs e)
        {
            if (dgvListA.Rows.Count == 0) return;
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                foreach (DataGridViewRow oRow in dgvListA.Rows)
                {
                    Byte[] arayTmpRemark = new Byte[22];
                    arayTmpRemark[1] = Convert.ToByte(oRow.Cells[1].Value.ToString());
                    if (oRow.Cells[2].Value.ToString() == "TX") arayTmpRemark[0] = 0;
                    else arayTmpRemark[0] = 1;

                    String strMainRemark = oRow.Cells[3].Value.ToString();
                    Byte[] arayRemark = HDLUDP.StringToByte(strMainRemark);
                    //ruby test
                    HDLSysPF.CopyRemarkBufferToSendBuffer(arayRemark, arayTmpRemark, 2);

                    if (CsConst.mySends.AddBufToSndList(arayTmpRemark, 0xE422, bSubNetId, bDeviceId, false, true, true, false) == false)
                    {
                        return;
                    }
                }
            }
            catch
            { }
            Cursor.Current = Cursors.Default;
        }

        private void btnRead1A_Click(object sender, EventArgs e)
        {
            if (numSub.Value == 255 || numDevA.Value == 255) return;
            if (numSub.Value == 0 && numDevA.Value == 0) return;
            
            Cursor.Current = Cursors.WaitCursor;
            bSubNetId = Convert.ToByte(numSub.Value);
            bDeviceId = Convert.ToByte(numDevA.Value);
            dgvListA.Rows.Clear();
            try
            {
                // 读取TX 接着读取RX
                for (Byte i=1; i <= 24; i++) // (DataGridViewRow oTmp in dgvListA.Rows)
                {
                    Byte[] arayTmp = new Byte[2] {0,i };

                    if (CsConst.mySends.AddBufToSndList(arayTmp, 0xE420, bSubNetId, bDeviceId, false, true, true, false))
                    {
                        Byte[] arayTmpRemark = new Byte[20];
                        Array.Copy(CsConst.myRevBuf, 25, arayTmpRemark, 0, 20);
                        String sSerialNumber = HDLPF.Byte2String(arayTmpRemark);
                        if (sSerialNumber != null && sSerialNumber != "")
                        {
                            Object[] obj = new Object[] { true, i, "TX", sSerialNumber };
                            dgvListA.Rows.Add(obj);
                        }
                    }
                    else return;
                }

                for (Byte i = 1; i <= 24; i++) // (DataGridViewRow oTmp in dgvListA.Rows)
                {
                    Byte[] arayTmp = new Byte[2] { 1,i };

                    if (CsConst.mySends.AddBufToSndList(arayTmp, 0xE420, bSubNetId, bDeviceId, false, true, true, false))
                    {
                        Byte[] arayTmpRemark = new Byte[20];
                        Array.Copy(CsConst.myRevBuf, 25, arayTmpRemark, 0, 20);
                        String sSerialNumber = HDLPF.Byte2String(arayTmpRemark);
                        if (sSerialNumber != null && sSerialNumber != "")
                        {
                            Object[] obj = new Object[] { true, i, "RX", sSerialNumber };
                            dgvListA.Rows.Add(obj);
                        }
                    }
                    else return;
                }
            }
            catch
            { }
            Cursor.Current = Cursors.Default;
        }

        private void btnAddA_Click(object sender, EventArgs e)
        {
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
            object[] obj = new object[] { true, bytSub, bytDev, CsConst.mstrINIDefault.IniReadValue("public", "99927", "") };
            dgvListA.Rows.Add(obj); 
        }

        private void numSub_ValueChanged(object sender, EventArgs e)
        {
            
        }

        private void numDevA_ValueChanged(object sender, EventArgs e)
        {
            
        }

        private void chbOption_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void btnUpdateA_Click(object sender, EventArgs e)
        {
         
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
            
        }

        public static byte GetIndexFromBuffers(DataGridView oDg, int columneID,String sType)
        {
            byte bytResult = 1;
            if (oDg.Rows == null) return bytResult;
            try
            {
                List<Byte> UsedID = new List<byte>();
                foreach (DataGridViewRow TmpRow in oDg.Rows)
                {
                    if (TmpRow.Cells[2].Value.ToString() == sType)
                    {
                        UsedID.Add(Convert.ToByte(TmpRow.Cells[columneID].Value.ToString()));
                    }
                }

                while (UsedID.Contains(bytResult))
                {
                    bytResult++;
                }
            }
            catch
            {
                return bytResult;
            }
            return bytResult;
        }

        private void btnSure_Click(object sender, EventArgs e)
        {
            if (dgvListA.RowCount >= 28) return;
            if (TX.Checked == false && RX.Checked == false) return;
            try
            {
                Byte bTotalAdd =Convert.ToByte(txtFrm.Value.ToString());
                while (bTotalAdd != 0)
                {
                    String sType = "TX";
                    if (RX.Checked == true) sType = "RX";
                    Byte CmdID = GetIndexFromBuffers(dgvListA, 1, sType);
                    Object[] Tmp = new Object[] { true, CmdID.ToString(), sType, "" };
                    dgvListA.Rows.Add(Tmp);
                    bTotalAdd--;
                }
            }
            catch
            { }
        }

        private void txtFrm_TextChanged(object sender, EventArgs e)
        {

        }

    }
}
