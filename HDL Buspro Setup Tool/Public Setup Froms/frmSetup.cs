using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Net; 

namespace HDL_Buspro_Setup_Tool
{
     partial class frmSetup : Form
    {
        public  frmSetup()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
        }


        private void frmSetup_Load(object sender, EventArgs e)
        {
            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker1.WorkerSupportsCancellation = true;  

            IniFile iniFile = new IniFile(Application.StartupPath + @"\ini\Default.ini");
            tbGroup.Text = iniFile.IniReadValue("Remote", "Group", "");
            LoadControlsText.DisplayTextToFormWhenFirstShow(this);

            cbIPs.Items.Clear();
            foreach (string strTmp in CsConst.mstrActiveIPs)
            {
                cbIPs.Items.Add(strTmp);
            }
            cbIPs.SelectedItem = CsConst.myLocalIP;
            tbPC1.Text = CsConst.mbytLocalSubNetID.ToString();
            tbPC2.Text = CsConst.mbytLocalDeviceID.ToString();
            cboType.SelectedIndex = Convert.ToInt32(iniFile.IniReadValue("Remote", "Index", "0"));
            CsConst.MyEnterProjectWay = cboType.SelectedIndex;
            CustomIP.Text = iniFile.IniReadValue("Remote", "CustomIP", "115.29.251.24");
            tbPWD.Text = iniFile.IniReadValue("Remote", "Password", "");
            cbIP.SelectedIndex = 0;
            cboEdit.SelectedIndex = CsConst.MyEditMode;
        }     

        private void btnTest_Click(object sender, EventArgs e)
        {
            btnTest.Enabled = false;
            if (cboType.SelectedIndex == 0)
            {
                HDLPF.GetRightIPAndPort();
                CsConst.mySends.IniTheSocket(CsConst.myLocalIP);
                this.Close();
            }
            else if (cboType.SelectedIndex == 1)
            {
                #region
                try
                {
                    string strIPTmp = tbPIP.Text;
                    string[] araystring = strIPTmp.Split('.');
                    if (araystring.Length != 4)
                    {
                        this.rtbHistory.AppendText(CsConst.WholeTextsList[2167].sDisplayName + '\n');
                        MessageBox.Show(CsConst.WholeTextsList[2167].sDisplayName, "", MessageBoxButtons.OK,
                            MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                        return;
                    }
                    for (int i = 0; i < 4; i++)
                    {
                        int bytTmp = Convert.ToInt32(araystring[i]);
                        if (bytTmp > 255)
                        {
                            this.rtbHistory.AppendText(CsConst.WholeTextsList[2167].sDisplayName + '\n');
                            MessageBox.Show(CsConst.WholeTextsList[2167].sDisplayName, "", MessageBoxButtons.OK,
                            MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                            return;
                        }
                    }
                    int PortTmp = Convert.ToInt32(tbPort.Text);
                    HDLUDP.ConstPort = PortTmp;
                    CsConst.myDestIP = strIPTmp;
                    CsConst.myintProxy = 1;

                    this.Close();
                }
                catch
                {
                    this.rtbHistory.AppendText(CsConst.WholeTextsList[2167].sDisplayName + '\n');
                    MessageBox.Show(CsConst.WholeTextsList[2167].sDisplayName, "", MessageBoxButtons.OK,
                            MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                }
                #endregion
            }
            else if (cboType.SelectedIndex == 2 || cboType.SelectedIndex == 3)
            {
                RemoteConnection();
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            btnSearch.Enabled = false;
            lvLists.Items.Clear();
            if (tbGroup.Text == "" || tbGroup.Text == null) return;

            // 判断是不是URL并且URL内容不为空
            if (tbUrl.Visible && tbUrl.Text != null)
            {
                string web = tbUrl.Text;
                IPHostEntry host = Dns.GetHostByName(web);
                IPAddress ip = host.AddressList[0];
                CustomIP.Text = ip.ToString();
            }

            this.rtbHistory.AppendText(CsConst.WholeTextsList[2168].sDisplayName + '\n');
            CsConst.mySends.IniTheSocket(CsConst.myLocalIP);
            if (CustomIP.Text != null && CustomIP.Text != "")
            {
                if (backgroundWorker1.IsBusy == false) backgroundWorker1.RunWorkerAsync();
            }
            //SearchRemoteNote();
        }

         private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            //不要直接使用组件实例名称（backgroundWorker1),因为有多个BackgroundWorker时，
            //直接使用会产生耦合问题，应该通过下面的转换使用它
            BackgroundWorker worker = sender as BackgroundWorker;
            //下面的内容相当于线程要处理的内容。//注意：不要在此事件中和界面控件打交道
            while (worker.CancellationPending == false)
            {
                try
                {                  
                    byte[] arayTmp = new byte[29];
                    string strRemark = tbGroup.Text.Trim();
                    if (strRemark == "") arayTmp[0] = 1;
                    else arayTmp[0] = 0;
                    byte[] arayRemark = new byte[0];
                    arayRemark = HDLUDP.StringToByte(strRemark);
                    if (arayRemark.Length <= 20)
                        Array.Copy(arayRemark, 0, arayTmp, 9, arayRemark.Length);
                    else
                        Array.Copy(arayRemark, 0, arayTmp, 9, 20);
                    if (cboType.SelectedIndex == 2)
                        CsConst.myDestIP = cbIP.Text;
                    else if (cboType.SelectedIndex == 3)
                            CsConst.myDestIP = CustomIP.Text;
                    HDLUDP.ConstPort = 9999;
                    if (CsConst.mySends.AddBufToSndList(arayTmp, 0x3011, 255, 255, false, false, true, true) == false)
                    {
                        Cursor.Current = Cursors.Default;
                        this.rtbHistory.AppendText(CsConst.WholeTextsList[2175].sDisplayName + '\n');
                    }
                    else
                    {
                        this.rtbHistory.AppendText(CsConst.WholeTextsList[2269].sDisplayName + '\n');

                        if (CsConst.myRevBuf[26] == 0xF8)
                        {
                            int intAll = CsConst.myRevBuf[27] * 256 + CsConst.myRevBuf[28];
                            CsConst.myRevBuf = new byte[1200];
                            arayTmp = new byte[23];
                            if (arayRemark.Length <= 20)
                                Array.Copy(arayRemark, 0, arayTmp, 1, arayRemark.Length);
                            else
                                Array.Copy(arayRemark, 0, arayTmp, 1, 20);
                            for (int i = 1; i <= intAll; i++)
                            {
                                arayTmp[21] = (byte)(i / 256);
                                arayTmp[22] = (byte)(i % 256);
                                if (CsConst.mySends.AddBufToSndList(arayTmp, 0x3013, 255, 255, false, false, true, false) == false)
                                {
                                    continue;
                                }
                                else
                                {
                                    ListViewItem tmp = new ListViewItem();
                                    if (CsConst.myRevBuf[82] == 1)
                                    {
                                        tmp.ImageIndex = 1;
                                        tmp.StateImageIndex = 1;
                                    }
                                    else
                                    {
                                        tmp.ImageIndex = 0;
                                        tmp.StateImageIndex = 0;
                                    }
                                    tmp.SubItems.Add((lvLists.Items.Count + 1).ToString());

                                    byte[] arayRemarkTmp = new byte[20];
                                    for (int intJ = 0; intJ < 20; intJ++) { arayRemarkTmp[intJ] = CsConst.myRevBuf[48 + intJ]; }
                                    tmp.SubItems.Add(HDLPF.Byte2String(arayRemarkTmp));

                                    arayRemarkTmp = new byte[8];
                                    for (int intJ = 0; intJ < 8; intJ++) { arayRemarkTmp[intJ] = CsConst.myRevBuf[68 + intJ]; }
                                    tmp.SubItems.Add(HDLPF.Byte2String(arayRemarkTmp));
                                    tmp.SubItems.Add(CsConst.myRevBuf[76] + "."
                                                    + CsConst.myRevBuf[77] + "."
                                                    + CsConst.myRevBuf[78] + "."
                                                    + CsConst.myRevBuf[79]);
                                    if (CsConst.myRevBuf[80] * 256 + CsConst.myRevBuf[81] == 0) tmp.ImageIndex = 0;
                                    else tmp.ImageIndex = 1;
                                    tmp.SubItems.Add((CsConst.myRevBuf[80] * 256 + CsConst.myRevBuf[81]).ToString());
                                    lvLists.Items.Add(tmp);
                                    CsConst.myRevBuf = new byte[1200];
                                }
                            }
                        }
                        else
                        {
                            Cursor.Current = Cursors.Default;
                            this.rtbHistory.AppendText(CsConst.WholeTextsList[2172].sDisplayName + '\n');
                            return;
                        }
                        break;
                    }
                    worker.CancelAsync();
                }
                catch
                {
                }
            }
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            try
            {
            }
            catch
            {
            }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                if (lvLists.Items != null && lvLists.Items.Count > 0)
                {
                    lvLists_ItemSelectionChanged(lvLists, new ListViewItemSelectionChangedEventArgs(lvLists.Items[0], 0, true));
                }
                Cursor.Current = Cursors.Default;
                btnSearch.Enabled = true;
            }
            catch
            {
            }
        }

        private void RemoteConnection()
        {
            CsConst.calculationWorker = new BackgroundWorker();
            CsConst.calculationWorker.DoWork += new DoWorkEventHandler(calculationWorker_DoWork);
            CsConst.calculationWorker.ProgressChanged += new ProgressChangedEventHandler(calculationWorker_ProgressChanged);
            CsConst.calculationWorker.WorkerReportsProgress = true;
            CsConst.calculationWorker.WorkerSupportsCancellation = true;
            CsConst.calculationWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(calculationWorker_RunWorkerCompleted);
            CsConst.calculationWorker.RunWorkerAsync();
        }

        void calculationWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                btnTest.Enabled = true;
                
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
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (tbName.Text == null || tbName.Text == "") return;
                if (tbPIP.Text == null || tbPIP.Text == "") return;
                if (tbPort.Text == null || tbPort.Text == "") return;
                if (tbUser.Text == null || tbUser.Text == "") return;
                if (tbPWD.Text == null || tbPWD.Text == "") return;
                this.rtbHistory.AppendText(CsConst.WholeTextsList[2270].sDisplayName+ '\n');
                string strname = tbUser.Text.Trim();
                string strPassword = tbPWD.Text.Trim();
                #region
                byte[] ArayTmp = new byte[12];
                ArayTmp[0] = byte.Parse(CsConst.myLocalIP.Split('.')[0].ToString());
                ArayTmp[1] = byte.Parse(CsConst.myLocalIP.Split('.')[1].ToString());
                ArayTmp[2] = byte.Parse(CsConst.myLocalIP.Split('.')[2].ToString());
                ArayTmp[3] = byte.Parse(CsConst.myLocalIP.Split('.')[3].ToString());
                ArayTmp[4] = 0x17;
                ArayTmp[5] = 0x70;
                ArayTmp[6] = Convert.ToByte(tbPIP.Text.Split('.')[0]);
                ArayTmp[7] = Convert.ToByte(tbPIP.Text.Split('.')[1]);
                ArayTmp[8] = Convert.ToByte(tbPIP.Text.Split('.')[2]);
                ArayTmp[9] = Convert.ToByte(tbPIP.Text.Split('.')[3]);
                ArayTmp[10] = Convert.ToByte(Convert.ToInt32(tbPort.Text) / 256);
                ArayTmp[11] = Convert.ToByte(Convert.ToInt32(tbPort.Text) % 256);
                //尝试连接一端口 5-3次
                CsConst.myDestIP = tbPIP.Text;
                HDLUDP.ConstPort = Convert.ToInt32(tbPort.Text);
                for (int i = 0; i <= 10; i++)
                {
                    CsConst.mySends.SendBufToRemote(ArayTmp, CsConst.myDestIP);
                }

                //发往服务器尝试交换端口
                CsConst.myDestIP = cbIP.Text;
                HDLUDP.ConstPort = 9999;
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x301D, 255, 255, false, false, true, false) == true)
                {
                }
                //尝试再次发往一端口交换机
                CsConst.myDestIP = tbPIP.Text;
                HDLUDP.ConstPort = Convert.ToInt32(tbPort.Text);

                if (VerifyPasswordToIpmoduleDirectly() == false) // (CsConst.mySends.AddBufToSndList(ArayTmp, 0x301D, 255, 255, false, false, true, true) == false)
                {
                    #region
                    //转服务器发送
                    this.rtbHistory.AppendText(CsConst.WholeTextsList[2271].sDisplayName + '\n');
                    CsConst.myDestIP = cbIP.Text;
                    HDLUDP.ConstPort = 9999;
                    ArayTmp = new byte[7];
                    ArayTmp[0] = Convert.ToByte(tbPIP.Text.Split('.')[0]);
                    ArayTmp[1] = Convert.ToByte(tbPIP.Text.Split('.')[1]);
                    ArayTmp[2] = Convert.ToByte(tbPIP.Text.Split('.')[2]);
                    ArayTmp[3] = Convert.ToByte(tbPIP.Text.Split('.')[3]);
                    ArayTmp[4] = Convert.ToByte(Convert.ToInt32(tbPort.Text) / 256);
                    ArayTmp[5] = Convert.ToByte(Convert.ToInt32(tbPort.Text) % 256);
                    ArayTmp[6] = 1;
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x301F, 255, 255, false, false, true, true) == false)
                    {
                        this.rtbHistory.AppendText(CsConst.WholeTextsList[2273].sDisplayName + '\n');
                        HDLPF.GetRightIPAndPort();
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                    else
                    {
                        if (VerifyPasswordToIpmoduleDirectly() == true)
                        {
                            Cursor.Current = Cursors.Default;
                            this.Close();
                        }
                    }
                    #endregion
                }
                else
                {
                    Cursor.Current = Cursors.Default;
                    this.Close();
                }
                #endregion
                Cursor.Current = Cursors.Default;
            }
            catch
            {
            }
        }


        private Boolean VerifyPasswordToIpmoduleDirectly()
        {
            Boolean BlnIsSuccess = false;
            //验证密码
            this.rtbHistory.AppendText(CsConst.WholeTextsList[2272].sDisplayName + '\n');
            CsConst.myRevBuf = new byte[1200];
            byte[] ArayPWD = new byte[66];
            byte[] ArayName = new byte[8];
            byte[] arayTmpPWD = HDLUDP.StringToByte(tbName.Text);
            Array.Copy(arayTmpPWD, 0, ArayPWD, 0, arayTmpPWD.Length);  // remark of project

            arayTmpPWD = HDLUDP.StringToByte(tbUser.Text);
            Array.Copy(arayTmpPWD, 0, ArayPWD, 20, arayTmpPWD.Length); // user name

            arayTmpPWD = new byte[8];// {0xA8,0x53,0xC0,0x6C,0xE0,0x8E,0xF8,0xA8 };
            Random rd = new Random();
            for (int intI = 0; intI < 4; intI++)    // Iterate
            {
                int i = rd.Next(65535);

                arayTmpPWD[intI * 2] = (byte)(i / 256);
                arayTmpPWD[intI * 2 + 1] = (byte)(i % 256);
            }
            Array.Copy(arayTmpPWD, 0, ArayPWD, 36, arayTmpPWD.Length);

            byte[] arayPWD = HDLUDP.StringToByte(tbPWD.Text);
            arayPWD.CopyTo(ArayName, 0);
            ArayName = HDLSysPF.encryption(arayTmpPWD, 8, ArayName, 8);// password

            Array.Copy(ArayName, 0, ArayPWD, 28, ArayName.Length);

            ArayPWD[44] = (byte)(System.DateTime.Now.Year - 2000);
            ArayPWD[45] = (byte)(System.DateTime.Now.Month);
            ArayPWD[46] = (byte)(System.DateTime.Now.Day);

            ArayPWD[47] = (byte)(System.DateTime.Now.Hour);
            ArayPWD[48] = (byte)(System.DateTime.Now.Minute);
            ArayPWD[49] = (byte)(System.DateTime.Now.Second);

            ArayName = HDLUDP.StringToByte("Easy Design");
            Array.Copy(ArayName, 0, ArayPWD, 50, ArayName.Length);

            ArayName = HDLUDP.StringToByte("V1.65");
            Array.Copy(ArayName, 0, ArayPWD, 60, ArayName.Length);
            if (CsConst.mySends.AddBufToSndList(ArayPWD, 0x300F, 255, 255, false, false, true, false) == false)
            {
                this.rtbHistory.AppendText(CsConst.WholeTextsList[2275].sDisplayName + '\n');
                HDLPF.GetRightIPAndPort();
                Cursor.Current = Cursors.Default;
                return BlnIsSuccess;
            }
            else
            {
                if (CsConst.myRevBuf[25] == 0xF8)
                {
                    CsConst.myRevBuf = new byte[1200];
                    this.rtbHistory.AppendText(CsConst.WholeTextsList[2274].sDisplayName + '\n');
                    CsConst.myintProxy = 2;
                    BlnIsSuccess = true;                    
                }
                else
                {
                    this.rtbHistory.AppendText(CsConst.WholeTextsList[2275].sDisplayName + '\n');
                    HDLPF.GetRightIPAndPort();
                    Cursor.Current = Cursors.Default;
                    BlnIsSuccess = false;
                }
            }
            return BlnIsSuccess;
        }

        private void lvLists_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (lvLists.Items == null || lvLists.Items.Count == 0) return;
            if (e.Item == null) return;

            tbName.Text = e.Item.SubItems[2].Text.ToString();
            tbUser.Text = e.Item.SubItems[3].Text.ToString();
            tbPIP.Text = e.Item.SubItems[4].Text.ToString();
            tbPort.Text = e.Item.SubItems[5].Text.ToString();
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            if (tabSetups.SelectedIndex == 0)
            {
                if (tbPC1.Text == null || tbPC1.Text == "") return;
                if (tbPC2.Text == null || tbPC2.Text == "") return;
                if (cbIPs.SelectedIndex == -1) return;

                string strTmp = tbPC1.Text;
                CsConst.mbytLocalSubNetID = Convert.ToByte(HDLPF.IsNumStringMode(strTmp, 0, 254));
                strTmp = tbPC2.Text;
                CsConst.mbytLocalDeviceID = Convert.ToByte(HDLPF.IsNumStringMode(strTmp, 0, 254));
                CsConst.myLocalIP = cbIPs.Text;
                CsConst.MyEditMode = Convert.ToByte(cboEdit.SelectedIndex);
                CsConst.mstrINIDefault.IniWriteValue("ProgramMode", "IP", CsConst.myLocalIP);
                HDLPF.GetRightIPAndPort();
                CsConst.myintProxy = 0;
                HDLUDP.ConstPort = 6000;
                CsConst.mySends.IniTheSocket(CsConst.myLocalIP);
                HDLSysPF.WriteInfoToIniFile();
                this.Close();
            }
            else if (tabSetups.SelectedIndex == 1)
            {
                btnTest_Click(btnTest, null);
            }
            
        }

        private void cboType_SelectedIndexChanged(object sender, EventArgs e)
        {
            Byte bytTag = Convert.ToByte(cboType.SelectedIndex.ToString());
            Boolean NeedServerIP = (bytTag >= 2);
            tbGroup.Enabled = NeedServerIP;
            btnSearch.Enabled = NeedServerIP;
            tbPIP.Enabled = (bytTag == 1);
           // tbPort.Enabled = (bytTag == 1);
            tbPWD.Enabled = NeedServerIP;
            btnSearch.Enabled = NeedServerIP;
            cbIP.Enabled = NeedServerIP;
            lvLists.Enabled = NeedServerIP;
            label4.Visible = NeedServerIP;
            CustomIP.Visible = (bytTag == 3);
            cbIP.Visible = (bytTag == 2);
            IniFile iniFile = new IniFile(Application.StartupPath + @"\ini\Default.ini");
            tbUrl.Visible = (cboType.SelectedIndex == 4);
            if (cboType.SelectedIndex == 1)
            {
                tbPIP.Text = iniFile.IniReadValue("Remote", "PTP1", "192.168.10.250");
                tbPort.Text = iniFile.IniReadValue("Remote", "PTP2", "6000");
            }
            else if (cboType.SelectedIndex == 2 || cboType.SelectedIndex == 3)
            {
                if (lvLists.Items != null && lvLists.Items.Count > 0)
                {
                    lvLists.Items[0].Selected = true;
                    lvLists_ItemSelectionChanged(lvLists, new ListViewItemSelectionChangedEventArgs(lvLists.Items[0], 0, true));
                }
            }
            iniFile.IniWriteValue("Remote", "Index", cboType.SelectedIndex.ToString());
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            btnApply_Click(btnApply, null);
        }

        private void tbPort_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar) || e.KeyChar == 8)
                e.Handled = false;
            else
                e.Handled = true;
        }

        private void tbPort_TextChanged(object sender, EventArgs e)
        {
            if (tbPort.Text.Length > 0)
            {
                string str = tbPort.Text;
                tbPort.Text = HDLPF.IsNumStringMode(str, 0, 65535);
                tbPort.SelectionStart = tbPort.Text.Length;
            }
            if (cboType.SelectedIndex == 1)
            {
                IniFile iniFile = new IniFile(Application.StartupPath + @"\ini\Default.ini");
                iniFile.IniWriteValue("Remote", "PTP2", tbPort.Text.ToString());
            }
        }

        private void rtbHistory_TextChanged(object sender, EventArgs e)
        {

        }

        private void tbGroup_TextChanged(object sender, EventArgs e)
        {
            IniFile iniFile = new IniFile(Application.StartupPath + @"\ini\Default.ini");
            iniFile.IniWriteValue("Remote", "Group", tbGroup.Text.ToString());
        }

        private void tbPIP_TextChanged(object sender, EventArgs e)
        {
            if (cboType.SelectedIndex == 1)
            {
                IniFile iniFile = new IniFile(Application.StartupPath + @"\ini\Default.ini");
                iniFile.IniWriteValue("Remote", "PTP1", tbPIP.Text.ToString());
            }
        }

        private void tbPWD_TextChanged(object sender, EventArgs e)
        {
            IniFile iniFile = new IniFile(Application.StartupPath + @"\ini\Default.ini");
            iniFile.IniWriteValue("Remote", "Password", tbPWD.Text.ToString());
        }
    }
}
