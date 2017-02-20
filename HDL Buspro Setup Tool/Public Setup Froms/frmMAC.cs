using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HDL_Buspro_Setup_Tool
{
    public partial class frmMAC : Form
    {
        private bool isChange = false;
        public frmMAC()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (tbPWD.Text == null || tbPWD.Text == "") return;
            grpMAC.Enabled = (tbPWD.Text == "13922363033" || tbPWD.Text == "85521566");
        }

        private void btnRead_Click(object sender, EventArgs e)
        {
            readMAC();
        }

        private bool readMAC()
        {
            bool result = true;
            if (numSub.Value == 255 || numDev.Value == 255) return false;
            Cursor.Current = Cursors.WaitCursor;
            byte bytSub = Convert.ToByte(numSub.Value);
            byte bytDev = Convert.ToByte(numDev.Value);

            String sMacInformation = HdlUdpPublicFunctions.ReadDeviceMacInformation(bytSub, bytDev,false);

            if (sMacInformation !="")
            {
                tbMAC.Text = sMacInformation;
                string str = tbMAC.Text.Replace(".", "");
                tbHint1.Text = Convert.ToInt64(str, 16).ToString();
            }
            else
            {
                result = false;
            }
            Cursor.Current = Cursors.Default;
            return result;
        }

        private void frmMAC_Load(object sender, EventArgs e)
        {
            if (CsConst.myOnlines == null) return;

            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker1.WorkerSupportsCancellation = true;

            HDLSysPF.LoadDeviceListsWithDifferentSensorType(cboDevA, 255);
            if (cboDevA.Items.Count > 0)
                cboDevA.SelectedIndex = 0;
            IniFile iniFile = new IniFile(Application.StartupPath + @"\ini\Default.ini");
            tbMACAuto.Text = iniFile.IniReadValue("Serial", "Hex1", "00.00.00.00.00.00.00.00");
            tbHint2.Text = iniFile.IniReadValue("Serial", "Int1", "0");
            tbMACMan.Text = iniFile.IniReadValue("Serial", "Hex2", "00.00.00.00.00.00.00.00");
            tbHint3.Text = iniFile.IniReadValue("Serial", "Int2", "0");
        }

        void calculationWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                if (CsConst.myOnlines != null && CsConst.myOnlines.Count() > 0)
                {

                }
                btnInitial.Enabled = true;
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
            //下面的内容相当于线程要处理的内容。//注意：不要在此事件中和界面控件打交道
            while (worker.CancellationPending == false)
            {
                try
                {
                    Cursor.Current = Cursors.WaitCursor;
                    byte bytSub = Convert.ToByte(numSub.Value);
                    byte bytDev = Convert.ToByte(numDev.Value);
                    byte[] ArayTmp = new byte[12];

                    ArayTmp[0] = 0;
                    ArayTmp[1] = 0;

                    string[] ArayStr = tbMAC.Text.Split('.');
                    for (int intI = 0; intI < ArayStr.Length; intI++) ArayTmp[intI + 2] = Convert.ToByte(ArayStr[intI], 16);
                    ArayTmp[10] = bytSub;
                    ArayTmp[11] = bytDev;

                    CsConst.WaitMore = true;
                    DateTime d1 = DateTime.Now;
                    DateTime d2 = DateTime.Now;
                    DateTime d3 = DateTime.Now;
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x3000, bytSub, bytDev, false, false, true, false) == true)
                    {
                        int intTmp = CsConst.mintFactoryTime;
                        while (CsConst.mintFactoryTime != 0)
                        {
                            d2 = DateTime.Now;
                            if (CsConst.mintFactoryTime == 255 || CsConst.mintFactoryTime == 0) break;
                            lbInitial.Text = CsConst.mstrINIDefault.IniReadValue("Public", "99918", "") + ":" + intTmp.ToString();
                            if (HDLSysPF.Compare(d2, d3) >= 1000)
                            {
                                d3 = DateTime.Now;
                                intTmp = intTmp - 1;
                                lbInitial.Text = CsConst.mstrINIDefault.IniReadValue("Public", "99918", "") + ":" + intTmp.ToString();
                            }
                            if (intTmp <= 0 && CsConst.mintFactoryTime !=0)
                            {
                                lbInitial.Text = CsConst.mstrINIDefault.IniReadValue("Public", "99910", "");
                                return;
                            }
                        }
                        lbInitial.Text = CsConst.mstrINIDefault.IniReadValue("Public", "99909", "");
                    }
                    else
                    {
                        MessageBox.Show(CsConst.mstrINIDefault.IniReadValue("Public", "99917", ""));
                    }
                    worker.CancelAsync();
                }
                catch
                {
                    worker.CancelAsync();
                }
            }
        }

        private bool IsValidHex(string strValue)
        {
            bool result = true;
            string str = "abcdefABCDEF1234567890.";
            for (int i = 0; i < strValue.Length; i++)
            {
                string strTmp = strValue.Substring(i, 1).ToString();
                if (!str.Contains(strTmp))
                {
                    result = false;
                    break;
                }
            }
            return result;
        }

        private void btnModify1_Click(object sender, EventArgs e)
        {
            if (!IsValidHex(tbMACAuto.Text))
            {
                MessageBox.Show(CsConst.mstrINIDefault.IniReadValue("Public", "99604", ""), "", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                return;
            }
            
            if (numSub.Value == 255 || numDev.Value == 255) return;
            Cursor.Current = Cursors.WaitCursor;

            if (tbMACAuto.Text == null || tbMACAuto.Text == "")
            {
                tbMACAuto.Text = "00.00.00.00.00.00.00.00";
            }

            byte bytSub = Convert.ToByte(numSub.Value);
            byte bytDev = Convert.ToByte(numDev.Value);
            string[] ArayStr = tbMACAuto.Text.Split('.');
            byte[] ArayTmp = new byte[ArayStr.Length];

            for (int intI = 0; intI < ArayStr.Length; intI++) ArayTmp[intI] = Convert.ToByte(ArayStr[intI],16);

            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xF001, bytSub, bytDev, false, true, true, false) == true)
            {
                ulong num = Convert.ToUInt64(tbHint2.Text);
                num++;
                tbHint2.Text = num.ToString();
                CsConst.myRevBuf = new byte[1200];
            }
            Cursor.Current = Cursors.Default;
        }

        private void btnModify2_Click(object sender, EventArgs e)
        {
            if (numSub.Value == 255 || numDev.Value == 255) return;
            if (tbMACMan.Text == null || tbMACMan.Text == "") return;
            Cursor.Current = Cursors.WaitCursor;
            
            byte bytSub = Convert.ToByte(numSub.Value);
            byte bytDev = Convert.ToByte(numDev.Value);
            string[] ArayStr = tbMACMan.Text.Split('.');
            byte[] ArayTmp = new byte[ArayStr.Length];

            for (int intI = 0; intI < ArayStr.Length; intI++) ArayTmp[intI] = Convert.ToByte(ArayStr[intI], 16);

            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xF001, bytSub, bytDev, false, true, true,false) == true)
            {
                CsConst.myRevBuf = new byte[1200];
            }
            Cursor.Current = Cursors.Default;
        }

        private void tbHint3_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (isChange) return;
                if (tbHint3.Text == null || tbHint3.Text == "") return;
                isChange = true;
                string str = tbHint3.Text;
                string strHex = Convert.ToUInt64(str).ToString("X2");
                strHex = GlobalClass.AddLeftZero(strHex, 16);
                string strTmp = "";
                strTmp = strTmp + strHex.Substring(0, 2) + ".";
                strTmp = strTmp + strHex.Substring(2, 2) + ".";
                strTmp = strTmp + strHex.Substring(4, 2) + ".";
                strTmp = strTmp + strHex.Substring(6, 2) + ".";
                strTmp = strTmp + strHex.Substring(8, 2) + ".";
                strTmp = strTmp + strHex.Substring(10, 2) + ".";
                strTmp = strTmp + strHex.Substring(12, 2) + ".";
                strTmp = strTmp + strHex.Substring(14, 2);
                tbMACMan.Text = strTmp;
                IniFile iniFile = new IniFile(Application.StartupPath + @"\ini\Default.ini");
                iniFile.IniWriteValue("Serial", "Int2", tbHint3.Text.ToString());
                iniFile.IniWriteValue("Serial", "Hex2", tbMACMan.Text.ToString());
                isChange = false;
            }
            catch
            {
            }
            isChange = false;
        }

        private void btnInitial_Click(object sender, EventArgs e)
        {
            if (numSub.Value == 255 || numDev.Value == 255) return;
            if (tbMAC.Text == null || tbMAC.Text == "") 
            {
                btnRead_Click(btnRead, null);
            }

            lbInitial.Text = "";
            btnInitial.Enabled = false;
            backgroundWorker1.RunWorkerAsync();
            //ReadThread();  //增加线程，使得当前窗体的任何操作不被限制
        }

        private void tbHint2_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (isChange) return;
                if (tbHint2.Text == null || tbHint2.Text == "") return;
                isChange = true;
                string str = tbHint2.Text;
                string strHex = Convert.ToUInt64(str).ToString("X2");
                strHex = GlobalClass.AddLeftZero(strHex, 16);
                string strTmp = "";
                strTmp = strTmp + strHex.Substring(0, 2) + ".";
                strTmp = strTmp + strHex.Substring(2, 2) + ".";
                strTmp = strTmp + strHex.Substring(4, 2) + ".";
                strTmp = strTmp + strHex.Substring(6, 2) + ".";
                strTmp = strTmp + strHex.Substring(8, 2) + ".";
                strTmp = strTmp + strHex.Substring(10, 2) + ".";
                strTmp = strTmp + strHex.Substring(12, 2) + ".";
                strTmp = strTmp + strHex.Substring(14, 2);
                tbMACAuto.Text = strTmp;
                IniFile iniFile = new IniFile(Application.StartupPath + @"\ini\Default.ini");
                iniFile.IniWriteValue("Serial", "Int1", tbHint2.Text.ToString());
                iniFile.IniWriteValue("Serial", "Hex1", tbMACAuto.Text.ToString());
                isChange = false;
            }
            catch
            {
            }
            isChange = false;
            tbMACAuto_TextChanged(null, null);
        }

        private void tbHint2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar) || e.KeyChar == 8)
                e.Handled = false;
            else
                e.Handled = true;
        }

        private void tbHint3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar) || e.KeyChar == 8)
                e.Handled = false;
            else
                e.Handled = true;
        }

        private void cboDevA_SelectedIndexChanged(object sender, EventArgs e)
        {
            lbInitial.Text = "";
            numSub.Value = Convert.ToDecimal(cboDevA.Text.Split('\\')[0].ToString().Split('-')[0].ToString());
            numDev.Value = Convert.ToDecimal(cboDevA.Text.Split('\\')[0].ToString().Split('-')[1].ToString());
        }

        private void btnGetDev_Click(object sender, EventArgs e)
        {
            if (CsConst.myOnlines == null) return;
            HDLSysPF.LoadDeviceListsWithDifferentSensorType(cboDevA, 255);
            if (cboDevA.Items.Count > 0)
                cboDevA.SelectedIndex = 0;
        }

        private void tbMACAuto_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (isChange) return;
                isChange = true;
                string str = tbMACAuto.Text;
                string[] strAry = str.Split('.');
                str = "";
                for (int i = 0; i < strAry.Length; i++)
                {
                    string strTmp = strAry[i];
                    strTmp = strTmp.Replace(" ", "").Trim();
                    strTmp = GlobalClass.AddLeftZero(strTmp, 2);
                    str = str + strTmp;
                }
                tbHint2.Text = Convert.ToUInt64(str, 16).ToString();
                IniFile iniFile = new IniFile(Application.StartupPath + @"\ini\Default.ini");
                iniFile.IniWriteValue("Serial", "Hex1", tbMACAuto.Text.ToString());
                iniFile.IniWriteValue("Serial", "Int1", tbHint2.Text.ToString());
                isChange = false;
            }
            catch
            {
            }
            isChange = false;
        }

        private void tbMACMan_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (isChange) return;
                isChange = true;
                string str = tbMACMan.Text;
                string[] strAry = str.Split('.');
                str = "";
                for (int i = 0; i < strAry.Length; i++)
                {
                    string strTmp = strAry[i];
                    strTmp = strTmp.Replace(" ", "").Trim();
                    strTmp = GlobalClass.AddLeftZero(strTmp, 2);
                    str = str + strTmp;
                }
                tbHint3.Text = Convert.ToUInt64(str, 16).ToString();
                IniFile iniFile = new IniFile(Application.StartupPath + @"\ini\Default.ini");
                iniFile.IniWriteValue("Serial", "Hex2", tbMACMan.Text.ToString());
                iniFile.IniWriteValue("Serial", "Int2", tbHint3.Text.ToString());
                isChange = false;
            }
            catch
            {
            }
            isChange = false;
        }

    }
}
