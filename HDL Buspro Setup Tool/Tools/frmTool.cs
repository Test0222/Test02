using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.IO;

namespace HDL_Buspro_Setup_Tool
{
    public partial class frmTool : Form
    {
        private delegate void FlushClient(); //代理
        private static byte mbyTmptLocalSubNetID = 1;
        private static byte mbyTmptLocalDeviceID = 254;
        private static int mintTmpLocalDeviceType = 0xFFFE;
        Thread thread = null;
        private bool isLoad = false;
        public frmTool()
        {
            InitializeComponent();
        }

        private void frmTool_Load(object sender, EventArgs e)
        {
            HDLSysPF.AutoScale((Form)sender);
            isLoad = true;
            
            mbyTmptLocalSubNetID = CsConst.mbytLocalSubNetID;
            mbyTmptLocalDeviceID = CsConst.mbytLocalDeviceID;
            mintTmpLocalDeviceType = CsConst.mintLocalDeviceType;
            //cl2.Width = rtbRev.Width - 130;
            
            if (CsConst.iLanguageId == 1)
            {
                cboCMD.Items.Clear();
                cboCMD.Items.AddRange(CsConst.strAryCMD);
            }
            cboCMD.SelectedIndex = 0;
            
            IniFile iniFile = new IniFile(Application.StartupPath + @"\ini\Default.ini");
            numDesSub.Text = iniFile.IniReadValue("Tool", "SubNetID", "0");
            numDesDev.Text = iniFile.IniReadValue("Tool", "DeviceID", "0");
            tbCMD.Text = Convert.ToString(iniFile.IniReadValue("Tool", "Command", "E"));
            txtTime.Text = Convert.ToString(iniFile.IniReadValue("Tool", "Time", "10"));
            string rb = Convert.ToString(iniFile.IniReadValue("Tool", "RB", "1"));
            switch (rb)
            {
                case "1": rb1.Checked=true; break;
                case "2": rb2.Checked = true; break;
                case "3": rb3.Checked = true; break;
                case "4": rb4.Checked = true; break;
                case "5": rb5.Checked = true; break;
            }
            isLoad = false;
            numDesSub_ValueChanged(numDesSub, null);
            numDesSub_ValueChanged(numDesDev, null);

            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker1.WorkerSupportsCancellation = true;  

            if (rb1.Checked)
                rb1_CheckedChanged(rb1, null);
            if (rb2.Checked)
                rb1_CheckedChanged(rb2, null);
            if (rb3.Checked)
                rb1_CheckedChanged(rb3, null);
            if (rb4.Checked)
                rb1_CheckedChanged(rb4, null);
            if (rb5.Checked)
                rb1_CheckedChanged(rb5, null);
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
                    CrossThreadFlush();
                }
                catch
                {
                }
            }
        }

        private void CrossThreadFlush()
        {
            FlushClient fc = new FlushClient(ThreadFunction);
            this.Invoke(fc); //通过代理调用刷新方法

            while (thread.IsAlive)
            {
                //将sleep和无限循环放在等待异步的外面
                ThreadFunction();
                Thread.Sleep(1);
            }
            
        }

        private void ThreadFunction()
        {
            if (this.rtbRev.InvokeRequired)//等待异步
            {
                FlushClient fc = new FlushClient(ThreadFunction);
                this.Invoke(fc); //通过代理调用刷新方法
            }
            else
            {
                try
                {
                    if (CsConst.MyQuene != null && CsConst.MyQuene.Count > 0)
                    {
                        byte[] Tmp = CsConst.MyQuene[0];
                        if (Tmp == null) return;
                        if (rb3.Checked && tbCMD.Text == "") return;
                        if (rb4.Checked && tbCMD.Text == "") return;
                        bool blnIsAdd = false;
                        if (rb1.Checked == true ||  //无地址
                           (rb2.Checked == true && Tmp[17] == Convert.ToByte(numDesSub.Text) && Tmp[18] == Convert.ToByte(numDesDev.Text)) ||
                           (rb2.Checked == true && Tmp[23] == Convert.ToByte(numDesSub.Text) && Tmp[24] == Convert.ToByte(numDesDev.Text)) || //目标设备发的，发给目标设备的
                           (rb3.Checked == true && Tmp[21] * 256 + Tmp[22] == Convert.ToInt32(tbCMD.Text, 16)) ||
                           (rb4.Checked == true && Tmp[21] * 256 + Tmp[22] == Convert.ToInt32(tbCMD.Text, 16) && Tmp[17] == Convert.ToByte(numDesSub.Text) && Tmp[18] == Convert.ToByte(numDesDev.Text)) ||
                           (rb4.Checked == true && Tmp[21] * 256 + Tmp[22] == Convert.ToInt32(tbCMD.Text, 16) && Tmp[23] == Convert.ToByte(numDesSub.Text) && Tmp[24] == Convert.ToByte(numDesDev.Text)))
                        {
                            blnIsAdd = true;
                        }
                        
                        if (blnIsAdd == true)
                        {
                            string strTmp = DateTime.Now.Hour.ToString("D2") + ":" +
                                            DateTime.Now.Minute.ToString("D2") + ":" +
                                            DateTime.Now.Second.ToString("D2") + ":" +
                                            DateTime.Now.Millisecond.ToString("D3")+" : ";

                            string str1 = "";//长度
                            string str2 = "";//源子网ID设备ID
                            string str3 = "";//设备类型
                            string str4 = "";//操作码
                            string str5 = "";//目标子网ID设备ID
                            string str6 = "";//数据
                            string strCRC = "";
                            str1 = Tmp[16].ToString("X2") + " ";
                            str2 = Tmp[17].ToString("X2") + " " + Tmp[18].ToString("X2") + " ";
                            str3 = Tmp[19].ToString("X2") + " " + Tmp[20].ToString("X2") + " ";
                            str4 = Tmp[21].ToString("X2") + " " + Tmp[22].ToString("X2") + " ";
                            str5 = Tmp[23].ToString("X2") + " " + Tmp[24].ToString("X2") + " ";
                            if (Tmp[16] != 255)
                            {
                                int DataCount = Tmp[16] - 11;
                                for (int i = 0; i < DataCount; i++)
                                    str6 = str6 + Tmp[25 + i].ToString("X2") + " ";
                                strCRC = Tmp[25 + (Tmp[16] - 11)].ToString("X2") + " " + Tmp[25 + (Tmp[16] - 11) + 1].ToString("X2");
                            }
                            else
                            {
                                int DataCount = Tmp[25] * 256 + Tmp[26] + 2;
                                for (int i = 0; i < DataCount; i++)
                                    str6 = str6 + Tmp[25 + i].ToString("X2") + " ";
                            }
                            string strCMD = Tmp[21].ToString("X2") + Tmp[22].ToString("X2");
                            string strTmp3 = CsConst.mstrINIDefault.IniReadValue("0x" + strCMD, "Eng", "");
                            //如果涉及温度读取特殊处理
                            #region
                            if (strCMD == "E3E5")
                            {
                                Byte[] Temperature4bytes = new Byte[4];
                                Array.Copy(Tmp, 27, Temperature4bytes, 0, 4);
                                strTmp3 += ":" + ByteToFloat(Temperature4bytes).ToString();
                            }
                            else if (strCMD == "1949")
                            {
                                Byte[] Temperature4bytes = new Byte[4];
                                Array.Copy(Tmp, 26, Temperature4bytes, 0, 4);
                                strTmp3 += ":" + ByteToFloat(Temperature4bytes).ToString();
                            }
                            else if (strCMD == "E3E8" || strCMD == "1947")
                            {
                                if (Tmp[26] >= 128) strTmp3 = "-" + (Tmp[26] - 128);
                                else strTmp3 = Tmp[26].ToString();
                            }
                            else if (strCMD == "E01C" || strCMD == "E01D")
                            {
                                strTmp3 = CsConst.WholeTextsList[2398] + " " + Tmp[25] + " " + CsConst.Status[Tmp[26] % 254];
                            }
                            #endregion
                            rtbRev.AppendText((rtbRev.Lines.Length + 1).ToString("D5")+"  " + strTmp);
                            ShowText(str1, rtbRev.TextLength, str1.Length, Color.Pink);
                            ShowText(str2, rtbRev.TextLength, str2.Length, Color.Green);
                            ShowText(str3, rtbRev.TextLength, str3.Length, Color.Blue);
                            ShowText(str4, rtbRev.TextLength, str4.Length, Color.DarkRed);
                            ShowText(str5, rtbRev.TextLength, str5.Length, Color.Green);
                            ShowText(str6, rtbRev.TextLength, str6.Length, Color.Red);
                            ShowText(strCRC, rtbRev.TextLength, strCRC.Length, Color.Black);
                            rtbRev.AppendText("  " + strTmp3);
                            rtbRev.AppendText("\r\n");

                        }
                        CsConst.MyQuene.RemoveAt(0);
                    }
                }
                catch
                {
                }
            }
        }

        private void ShowText(string sText, int nStart, int nLength, Color color)         
        {
            rtbRev.AppendText(sText);
            rtbRev.Select(nStart, nLength);    // 需要修改颜色的部分             
            rtbRev.SelectionColor = color;     // 颜色                    
        }

        private void frmTool_FormClosing(object sender, FormClosingEventArgs e)
        {
            CsConst.MyBlnCapture = false;
            if (thread != null && thread.IsAlive)
            {
                thread.Abort();
                thread = null;
            }
        }

        private void numDesSub_ValueChanged(object sender, EventArgs e)
        {
            if (isLoad) return;
            if (((TextBox)sender).Tag == null || ((TextBox)sender).Tag.ToString() == "") return;
            byte bytTag = Convert.ToByte(((TextBox)sender).Tag.ToString());

            switch (bytTag)
            {
                case 1: lbDesV1.Text = "(0x" + Convert.ToByte(((TextBox)sender).Text).ToString("X2") + ")"; break;
                case 2: lbDesV2.Text = "(0x" + Convert.ToByte(((TextBox)sender).Text).ToString("X2") + ")"; break;
            }
            IniFile iniFile = new IniFile(Application.StartupPath + @"\ini\Default.ini");
            iniFile.IniWriteValue("Tool", "SubNetID", numDesSub.Text.ToString());
            iniFile.IniWriteValue("Tool", "DeviceID", numDesDev.Text.ToString());
        }

        private void cboCMD_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboCMD.SelectedIndex == -1) return;
            tbCMD.Enabled = (cboCMD.SelectedIndex == 0);
            switch (cboCMD.SelectedIndex)
            {
                case 0: lbCMDH.Text = "";break;
                case 1: tbCMD.Text = "0002"; ;lbCMDH.Text = "Area No, Scene No."; break;
                case 2: tbCMD.Text = "001A"; ;lbCMDH.Text = "Area No, Sequence No."; break;
                case 3: tbCMD.Text = "0031"; ;lbCMDH.Text = "Channel No.,Level, Running Time(H), Running Time(L)"; break;
                case 4: tbCMD.Text = "E01C"; ;lbCMDH.Text = "Switch No, Status(0:OFF; 255: ON)."; break;
                case 5: tbCMD.Text = "E3E0"; ;lbCMDH.Text = "Curtain No, Status(0:Stop;1:Open;2:Close)."; break;
                case 6: tbCMD.Text = "E3D4"; ;lbCMDH.Text = "1(Default),Group No."; break;
            }
        }

        private void rbPCD_CheckedChanged(object sender, EventArgs e)
        {
            lbSubID.Enabled = rbPCC.Checked;
            lbDevID.Enabled = rbPCC.Checked;
            lbDevType.Enabled = rbPCC.Checked;
            NumSubID.Enabled = rbPCC.Checked;
            NumDevID.Enabled = rbPCC.Checked;
            numDevType.Enabled = rbPCC.Checked;

            if (rbPCC.Checked == false)
            {
                CsConst.mbytLocalSubNetID = mbyTmptLocalDeviceID;
                CsConst.mbytLocalDeviceID = mbyTmptLocalDeviceID;
                CsConst.mintLocalDeviceType = mintTmpLocalDeviceType;
            }
            else if (rbPCC.Checked == true)
            {
                mbyTmptLocalDeviceID = CsConst.mbytLocalSubNetID ;
                mbyTmptLocalDeviceID = CsConst.mbytLocalDeviceID ;
                mintTmpLocalDeviceType = CsConst.mintLocalDeviceType ;  //临时存放默认设置

                // 更新当前设备为最新
                CsConst.mbytLocalSubNetID = Convert.ToByte(NumSubID.Value);
                CsConst.mbytLocalDeviceID = Convert.ToByte(NumDevID.Value); ;
                CsConst.mintLocalDeviceType = Convert.ToByte(numDevType.Value);
            }

        }

        private void btnOne_Click(object sender, EventArgs e)
        {
            if (tbCMD.Text == null || tbCMD.Text == "") return;
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                string strSend = rtbSends.Text;
                //if (rtbSends.Lines.Length != 0) strSend = rtbSends.Lines[0].Clone().ToString().Trim();
                //准备工作
                byte bytSub = Convert.ToByte(numDesSub.Text);
                byte bytDev = Convert.ToByte(numDesDev.Text);
                int intCMD = Convert.ToInt32(tbCMD.Text, 16);
                byte[] ArayTmp = null;
                if (strSend != "" && strSend != null) // 如果为空直接发送
                {
                    if (rbSdAscii.Checked == true) // ascii 码发送数据
                    {
                        ArayTmp = HDLUDP.StringToByte(strSend);
                        for (int i = 0; i < ArayTmp.Length; i++)
                        {
                            if (ArayTmp[i] == 0x0A) ArayTmp[i] = 0x0D;
                        }
                    }
                    else if (rbSdHex.Checked == true)  // 十六进制数字发送，判断分隔符
                    {
                        char strPos = ',';
                        if (rbSd2.Checked == true) strPos = ' ';
                        string[] ArayStr = strSend.Trim().Split(strPos);
                        int NullCount = 0;
                        for (int i = 0; i < ArayStr.Length; i++)
                        {
                            if (ArayStr[i] == null || ArayStr[i] == "" || ArayStr[i].Trim() == "") NullCount = NullCount + 1;
                        }
                        ArayTmp = new byte[ArayStr.Length - NullCount];
                        int intI = 0;
                        for (int i = 0; i < ArayStr.Length; i++) 
                        {
                            if (ArayStr[i] != null && ArayStr[i] != "" && ArayStr[i].Trim() != "")
                            {
                                ArayTmp[intI] = Convert.ToByte(ArayStr[i], 16);
                                intI++;
                            }
                        }
                    }
                    else if (rbSd10.Checked == true)  // 十六进制数字发送，判断分隔符
                    {
                        char strPos = ',';
                        if (rbSd2.Checked == true) strPos = ' ';
                        string[] ArayStr = strSend.Trim().Split(strPos);
                        int NullCount = 0;
                        for (int i = 0; i < ArayStr.Length; i++)
                        {
                            if (ArayStr[i] == null || ArayStr[i] == "" || ArayStr[i].Trim() == "") NullCount = NullCount + 1;
                        }
                        ArayTmp = new byte[ArayStr.Length - NullCount];
                        int intI = 0;
                        for (int i = 0; i < ArayStr.Length; i++) 
                        {
                            if (ArayStr[i] != null && ArayStr[i] != "" && ArayStr[i].Trim() != "")
                            {
                                ArayTmp[intI] = Convert.ToByte(ArayStr[i], 10);
                                intI++;
                            }
                        }
                    }
                }
                ArayTmp = CsConst.mySends.PackAndSend(ArayTmp, intCMD, bytSub, bytDev, false);
                CsConst.mySends.SendBufToRemote(ArayTmp, CsConst.myDestIP);
            }
            catch
            {
            }
            Cursor.Current = Cursors.Default;
        }

        private void rb5_CheckedChanged(object sender, EventArgs e)
        {
            CsConst.MyBlnCapture = false;
            backgroundWorker1.CancelAsync(); 
        }

        private void rb1_CheckedChanged(object sender, EventArgs e) 
        {
            if (isLoad) return;
            IniFile iniFile = new IniFile(Application.StartupPath + @"\ini\Default.ini");
            iniFile.IniWriteValue("Tool", "RB", (sender as RadioButton).Tag.ToString());
            CsConst.MyQuene = new List<byte[]>();
            CsConst.MyBlnCapture = true;
            if (backgroundWorker1.IsBusy == false)
            {
                backgroundWorker1.RunWorkerAsync();
            }
        }


        private void btnClear_Click(object sender, EventArgs e)
        {
            rtbRev.Text = "";
        }

        /// <summary>
        /// 将byte数组转为浮点数
        /// </summary>
        /// <param name="bResponse">byte数组</param>
        /// <returns></returns>
        public static float ByteToFloat(byte[] bResponse)
        {
            if (bResponse.Length < 4 || bResponse.Length > 4)
            {
                //throw new NotEnoughDataInBufferException(data.length(), 8);
                return 0;
            }
            else
            {
                byte[] intBuffer = new byte[4];
                //将byte数组的前后两个字节的高低位换过来
                intBuffer[0] = bResponse[0];
                intBuffer[1] = bResponse[1];
                intBuffer[2] = bResponse[2];
                intBuffer[3] = bResponse[3];
                return BitConverter.ToSingle(intBuffer, 0);
            }
        }



        public String TranferCMDToTextString(string strCMD, string strExplain)
        {
            String strResult = string.Empty;

            if (strCMD == null || strCMD == "") return strResult;

            string[] TmpCommandLine = strCMD.Split(' ').ToArray();
            Int32 DeviceType = Convert.ToInt16("0x" + TmpCommandLine[3]+TmpCommandLine[4],16);

            if (strExplain == null || strExplain == "")
            {
                strExplain = TmpCommandLine[5] + TmpCommandLine[6];
            }
            strResult = Convert.ToByte(TmpCommandLine[1],16) + "-" 
                      + Convert.ToByte(TmpCommandLine[2],16) + "("
                      + CsConst.mstrINIDefault.IniReadValue("DeviceType" + DeviceType.ToString(), "Model", "") + ") Send Command to "
                      + Convert.ToByte(TmpCommandLine[7],16) + "-" 
                      + Convert.ToByte(TmpCommandLine[8],16) + " " + strExplain + " ("
                      + strCMD + ")";

            return strResult;
        }

        private void btnRepeat_Click(object sender, EventArgs e)
        {
            timer1.Interval =  Convert.ToInt32(txtTime.Text);
            timer1.Enabled = !timer1.Enabled;
            //gbBasic.Enabled = !timer1.Enabled;
            grpDes.Enabled = !timer1.Enabled;
            rtbSends.Enabled = !timer1.Enabled;
            btnOne.Enabled = !timer1.Enabled;
            txtTime.Enabled = !timer1.Enabled;
            if (btnRepeat.Text == CsConst.mstrINIDefault.IniReadValue("Public", "99812", ""))
            {
                btnRepeat.Text = CsConst.mstrINIDefault.IniReadValue("Public", "00036", "");
            }
            else if(btnRepeat.Text == CsConst.mstrINIDefault.IniReadValue("Public", "00036", ""))
            {
                btnRepeat.Text = CsConst.mstrINIDefault.IniReadValue("Public", "99812", "");
            }
        }

        private void txtTime_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar) || e.KeyChar == 8)
                e.Handled = false;
            else
                e.Handled = true;
        }

        private void txtTime_Leave(object sender, EventArgs e)
        {
            try
            {
                string str = txtTime.Text;
                txtTime.Text = HDLPF.IsNumStringMode(str, 1, 86400000);
                txtTime.SelectionStart = txtTime.Text.Length;
            }
            catch
            {
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    string strFileName = saveFileDialog1.FileName.ToString();
                    if (!File.Exists(strFileName))            
                    {
                        FileStream fs1 = new FileStream(strFileName, FileMode.Create, FileAccess.Write);//创建写入文件                 
                        StreamWriter sw = new StreamWriter(fs1);
                        for (int i = 0; i < rtbRev.Lines.Length; i++)
                        {
                            sw.WriteLine(rtbRev.Lines[i].ToString());//开始写入值      
                        }
                                  
                        sw.Close();                
                        fs1.Close();
                    }            
                    else            
                    {
                        FileStream fs = new FileStream(strFileName, FileMode.Open, FileAccess.Write);                
                        StreamWriter sr = new StreamWriter(fs);
                        for (int i = 0; i < rtbRev.Lines.Length; i++)
                        {
                            sr.WriteLine(rtbRev.Lines[i].ToString());//开始写入值      
                        }              
                        sr.Close();               
                        fs.Close();           
                    }
                }
            }
            catch
            {
            }
            Cursor.Current = Cursors.Default;
        }

        private void tbCMD_TextChanged(object sender, EventArgs e)
        {
            if (isLoad) return;
            IniFile iniFile = new IniFile(Application.StartupPath + @"\ini\Default.ini");
            iniFile.IniWriteValue("Tool", "Command", tbCMD.Text.ToString());
        }

        private void txtTime_TextChanged(object sender, EventArgs e)
        {
            if (isLoad) return;
            IniFile iniFile = new IniFile(Application.StartupPath + @"\ini\Default.ini");
            iniFile.IniWriteValue("Tool", "Time", txtTime.Text.ToString());
        }

        private void rtbSends_KeyPress(object sender, KeyPressEventArgs e)
        {
            toolTip1.Show("", rtbSends);
            if (chbMultiline.Checked == false)
            {
                if (e.KeyChar == (char)13)
                {
                    rtbSends.SelectAll();
                    btnOne_Click(btnOne, null);
                }
            }
        }

        private void rtbSends_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string strSend = rtbSends.Text;
                if (rbSdHex.Checked == true)  // 十六进制数字发送，判断分隔符
                {
                    char strPos = ',';
                    if (rbSd2.Checked == true) strPos = ' ';
                    string[] ArayStr = strSend.Trim().Split(strPos);
                    int NullCount = 0;
                    for (int i = 0; i < ArayStr.Length; i++)
                    {
                        if (ArayStr[i] == null || ArayStr[i] == "" || ArayStr[i].Trim() == "") NullCount = NullCount + 1;
                    }
                    for (int i = 0; i < ArayStr.Length; i++)
                    {
                        if (ArayStr[i] != null && ArayStr[i] != "" && ArayStr[i].Trim() != "")
                        {
                            if (!(IsIllegalHexadecimal(ArayStr[i])) ||
                                (Convert.ToInt32(ArayStr[i], 16) > 255))
                            {
                                toolTip1.Show(ArayStr[i] + " " + CsConst.mstrINIDefault.IniReadValue("Public", "99613", ""), rtbSends, rtbSends.PointToClient(MousePosition));
                            }
                        }
                    }
                }
                else if (rbSd10.Checked == true)
                {
                    char strPos = ',';
                    if (rbSd2.Checked == true) strPos = ' ';
                    string[] ArayStr = strSend.Trim().Split(strPos);
                    int NullCount = 0;
                    for (int i = 0; i < ArayStr.Length; i++)
                    {
                        if (ArayStr[i] == null || ArayStr[i] == "" || ArayStr[i].Trim() == "") NullCount = NullCount + 1;
                    }
                    for (int i = 0; i < ArayStr.Length; i++)
                    {
                        if (ArayStr[i] != null && ArayStr[i] != "" && ArayStr[i].Trim() != "")
                        {
                            if (!HDLPF.IsRightNumStringMode(ArayStr[i],0,255) ||
                                (Convert.ToInt32(ArayStr[i]) > 255))
                            {
                                toolTip1.Show(ArayStr[i] + " " + CsConst.mstrINIDefault.IniReadValue("Public", "99613", ""), rtbSends, rtbSends.PointToClient(MousePosition));
                            }
                        }
                    }
                }
            }
            catch
            {
            }
        }

        private bool IsIllegalHexadecimal(string hex) 
        { 
            IList<char> HexSet = new List<char>(){ '0','1','2','3','4','5','6','7','8','9','A','B','C','D','E','F','a','b','c','d','e','f' };   
            foreach (char item in hex) 
            { 
                if(!HexSet.Contains(item))                    
                return false; 
            }
            return true; 
        }

        private void rtbSends_MouseDown(object sender, MouseEventArgs e)
        {
            toolTip1.Show("", rtbSends);
        }

        private void chbMultiline_CheckedChanged(object sender, EventArgs e)
        {
            rtbSends.Multiline = chbMultiline.Checked;
        }

        private void numDesSub_KeyPress(object sender, KeyPressEventArgs e)
        {
            
        }

        private void numDesSub_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == (char)Keys.Tab)
            {
                ((NumericUpDown)sender).Focus();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
