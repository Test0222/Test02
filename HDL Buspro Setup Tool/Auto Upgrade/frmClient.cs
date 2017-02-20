using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Microsoft.Win32;

namespace HDL_Buspro_Setup_Tool
{
    public partial class ServerMain : Form
    {
        int ReceiveUpdateTimes = 0;
        public String sPath = Application.StartupPath + @"\Firmware & software update list.xls";

        public ServerMain()
        {
            InitializeComponent();
            TextBox.CheckForIllegalCrossThreadCalls = false;
        }

        Thread threadClient = null; // 创建用于接收服务端消息的 线程；
        Socket sockClient = null;

        static List<VersionInformation> currentDevicesList = null;

        private void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                IPAddress ip = IPAddress.Parse(txtIp.Text.Trim());
                IPEndPoint endPoint = new IPEndPoint(ip, int.Parse(txtPort.Text.Trim()));
                sockClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                try
                {
                    ShowMsg("与服务器连接中……");
                    sockClient.Connect(endPoint);

                }
                catch (SocketException se)
                {
                    ShowMsg(se.Message);
                    AutoConnect.Enabled = true;
                    return;
                    //this.Close();
                }
                ShowMsg("与服务器连接成功！！！");
                AutoConnect.Enabled = false;
                //更新用户名过去
                string strMsg = txtName.Text;
                byte[] arrMsg = System.Text.Encoding.UTF8.GetBytes(strMsg);
                byte[] arrSendMsg = new byte[arrMsg.Length + 1];
                arrSendMsg[0] = 2; // 用来表示发送的是消息数据
                Buffer.BlockCopy(arrMsg, 0, arrSendMsg, 1, arrMsg.Length);
                sockClient.Send(arrSendMsg); // 发送消息；

                threadClient = new Thread(RecMsg);
                threadClient.IsBackground = true;
                threadClient.Start();
            }
            catch { }

            //txtMsgSend.Text = "PCB.xlsx";
            //btnSendMsg_Click(btnSendMsg, null);
        }

        void RecMsg()
        {
            while (true)
            {
                // 定义一个2M的缓存区；
                byte[] arrMsgRec = new byte[1024 * 1024 * 2];
                // 将接受到的数据存入到输入  arrMsgRec中；
                int length = -1;
                try
                {
                    length = sockClient.Receive(arrMsgRec); // 接收数据，并返回数据的长度；
                }
                catch (SocketException se)
                {
                    ShowMsg("异常；" + se.Message);
                    return;
                }
                catch (Exception e)
                {
                    ShowMsg("异常："+e.Message);
                    return;
                }
                if (arrMsgRec[0] == 0) // 表示接收到的是消息数据；
                {
                    string strMsg = System.Text.Encoding.UTF8.GetString(arrMsgRec, 1, length-1);// 将接受到的字节数据转化成字符串；
                   // ShowMsg(strMsg);

                    //追加到buffer
                    UpdateNewChangesToStruct(strMsg);

                    ReceiveUpdateTimes++;
                    this.notifyicon.BalloonTipText = this.Text + " - " + ReceiveUpdateTimes.ToString();
                    this.notifyicon.ShowBalloonTip(5000);
                }
                if (arrMsgRec[0] == 1) // 表示接收到的是文件数据；
                {
                    try
                    {
                        Boolean bIsStartComparing = false;
                         //在上边的 sfd.ShowDialog（） 的括号里边一定要加上 this 否则就不会弹出 另存为 的对话框，而弹出的是本类的其他窗口，，这个一定要注意！！！【解释：加了this的sfd.ShowDialog(this)，“另存为”窗口的指针才能被SaveFileDialog的对象调用，若不加thisSaveFileDialog 的对象调用的是本类的其他窗口了，当然不弹出“另存为”窗口。】
                        if (txtMsgSend.Text.Contains(".xls"))
                        {
                            txtMsgSend.Text = txtMsgSend.Text.Replace(".xls", " new.xls");
                            bIsStartComparing = true;
                        }

                        String sFilePath = Application.StartupPath + @"\" + txtMsgSend.Text;

                        string fileSavePath = sFilePath;// 获得文件保存的路径；
                        // 创建文件流，然后根据路径创建文件；
                        using (FileStream fs = new FileStream(fileSavePath, FileMode.Create))
                        {
                            fs.Write(arrMsgRec, 1, length - 1);
                            fs.Dispose();
                            ShowMsg("文件保存成功：" + fileSavePath);
                            CsConst.bIsDownloadSuccess = true;
                            
                        }
                        if (bIsStartComparing)
                        {
                            CompareTheTwoStructsAndDownloadTheLastestFirmware();
                        }
                    }
                    catch (Exception aaa)
                    {
                        MessageBox.Show(aaa.Message);
                    }
                }
            }
        }

        void CompareTheTwoStructsAndDownloadTheLastestFirmware()
        {
            try
            {
                DateTime t1, t2;
                String sTmpPath = Application.StartupPath + @"\Firmware & software update list new.xls";
                #region
                GridView2.DataSource = PublicMethods.LoadDataFromExcel(sTmpPath);
                GridView2.Columns[0].Width = Convert.ToInt32(GridView2.Width * 0.12);
                GridView2.Columns[1].Width = Convert.ToInt32(GridView2.Width * 0.12);
                GridView2.Columns[2].Width = Convert.ToInt32(GridView2.Width * 0.3);
                GridView2.Columns[3].Width = Convert.ToInt32(GridView2.Width * 0.3);
                GridView2.Columns[4].Width = Convert.ToInt32(GridView2.Width * 0.1);
                GridView2.Columns[5].Width = Convert.ToInt32(GridView2.Width * 0.1);

                List<VersionInformation> currentDevicesListTmp = new List<VersionInformation>();
                for (int i = 0; i < GridView2.RowCount; i++)
                {
                    if (GridView2[5, i].Value == null || GridView2[0, i].Value.ToString() == "") continue;

                    VersionInformation vTmp = new VersionInformation();
                    vTmp.sModel = GridView2[0, i].Value.ToString();
                    vTmp.sVersion = GridView2[1, i].Value.ToString();
                    vTmp.sProduction = GridView2[2, i].Value.ToString();
                    vTmp.sFirmwareName = GridView2[3, i].Value.ToString();
                    vTmp.sSerialId = GridView2[5, i].Value.ToString();
                    currentDevicesListTmp.Add(vTmp);
                }
                #endregion
                if (currentDevicesList == null || currentDevicesList.Count == 0)
                {
                    for (int i = 0; i < currentDevicesListTmp.Count; i++)
                    {
                        t1 = DateTime.Now;
                        int sendTimes = 1;	//已重发次数
                        String sFirmwareName = currentDevicesListTmp[i].sFirmwareName;
                        txtMsgSend.Text = sFirmwareName;
                        btnSendMsg_Click(btnSendMsg, null);
                        currentDevicesListTmp[i].bProcess = 100;
                        while (CsConst.bIsDownloadSuccess != true)
                        {
                            t2 = DateTime.Now;
                            int TimeBetw = HDLSysPF.Compare(t2, t1);
                            if (TimeBetw >= CsConst.replySpanTimes + CsConst.MoreDelay)
                            {
                                btnSendMsg_Click(btnSendMsg, null);
                                t1 = DateTime.Now;
                            }
                            if (sendTimes >= CsConst.replytimes)
                            {
                                currentDevicesListTmp[i].bProcess = 0;
                                CsConst.bIsDownloadSuccess = true;
                                sendTimes++;
                            }
                        }
                    }
                }
                else
                {
                    if (currentDevicesListTmp != null || currentDevicesListTmp.Count != 0)
                    {
                        for (int i = 0; i < currentDevicesListTmp.Count; i++)
                        {
                            foreach (VersionInformation sTmpSerialVersion in currentDevicesList)
                            {
                                //依次下载所需更新的文件
                                #region
                                CsConst.bIsDownloadSuccess = false;
                                String sFirmwareName = "";
                                int sendTimes = 1;	//已重发次数
                                if ((sTmpSerialVersion.sSerialId == currentDevicesListTmp[i].sSerialId) &&
                                    (sTmpSerialVersion.sFirmwareName == currentDevicesListTmp[i].sFirmwareName)) break;

                                if (sTmpSerialVersion.sSerialId == currentDevicesListTmp[i].sSerialId)
                                {
                                    t1 = DateTime.Now;
                                    sFirmwareName = currentDevicesListTmp[i].sFirmwareName;
                                    txtMsgSend.Text = sFirmwareName;
                                    btnSendMsg_Click(btnSendMsg, null);
                                    currentDevicesListTmp[i].bProcess = 100;
                                    while (CsConst.bIsDownloadSuccess != true)
                                    {
                                        t2 = DateTime.Now;
                                        int TimeBetw = HDLSysPF.Compare(t2, t1);
                                        if (TimeBetw >= CsConst.replySpanTimes + CsConst.MoreDelay)
                                        {
                                            btnSendMsg_Click(btnSendMsg, null);
                                            t1 = DateTime.Now;
                                        }
                                        if (sendTimes >= CsConst.replytimes)
                                        {
                                            currentDevicesListTmp[i].bProcess = 0;
                                            CsConst.bIsDownloadSuccess = true;
                                            sendTimes++;
                                        }
                                    }
                                    break;
                                }
                                #endregion
                            }
                        }
                    }
                }
                //update buffer 
                currentDevicesList = currentDevicesListTmp;

                FileStream fs = new FileStream(Application.StartupPath + @"\FirmwareLists.dat", FileMode.Create);
                BinaryFormatter bf = new BinaryFormatter();
                if (currentDevicesList != null) bf.Serialize(fs, currentDevicesList);
                fs.Close();

                // 删除旧文件 改名字新文件
                File.Delete(sTmpPath);

                this.Close();

            }
            catch { }
        }

        void UpdateNewChangesToStruct(string UpdateInformation)
        {
            if (UpdateInformation == null || UpdateInformation == "") return;
            if (UpdateInformation.Contains("_") == false) return;
            if (string.IsNullOrEmpty(sPath))
            {

            }
            else
            {
                // 用文件流打开用户要发送的文件；
                FileInfo fi = new FileInfo(sPath);
                String strMsg = fi.LastWriteTime.ToString().ToString();
                String[] arrTmp = UpdateInformation.Split('_');

                if (strMsg == arrTmp[0]) return;

                IniFile Tmp = new IniFile(Application.StartupPath + @"\ini\Default.ini");

                Tmp.IniWriteValue("AutoUpgrade", "Software", arrTmp[1]);
                txtMsgSend.Text = "Firmware & software update list.xls";
                btnSendMsg_Click(btnSendMsg, null);
            }   

           // FileStream fs = new FileStream(UpdateInformation, FileMode.Create);
        }

        void ShowMsg(string str)
        {
            tsbHint.Text = str + "\r\n";
        }

         // 发送消息；
        private void btnSendMsg_Click(object sender, EventArgs e)
        {
            string strMsg = txtMsgSend.Text.Trim();
                          
            byte[] arrMsg = System.Text.Encoding.UTF8.GetBytes(strMsg);
            byte[] arrSendMsg = new byte[arrMsg.Length + 1];
            arrSendMsg[0] = 0; // 用来表示发送的是消息数据
            Buffer.BlockCopy(arrMsg, 0, arrSendMsg, 1, arrMsg.Length);
            sockClient.Send(arrSendMsg); // 发送消息；
            ShowMsg(strMsg);
        }

        private void ServerMain_Load(object sender, EventArgs e)
        {
            string startup = Application.ExecutablePath; //取得程序路径
            int pp = startup.LastIndexOf("\\");
            startup = startup.Substring(0, pp);
            string icon = startup + "\\下载.ico";
            //3.一定为notifyIcon1其设置图标，否则无法显示在通知栏。或者在其属性中设置
            notifyicon.Icon = new Icon(icon);

            // read from excel or from file we saved
            //ReadEverythingFromExcel();
            if (File.Exists(Application.StartupPath + @"\FirmwareLists.dat") == true)
            {
                FileStream fs = new FileStream(Application.StartupPath + @"\FirmwareLists.dat", FileMode.Open);
                BinaryFormatter bf = new BinaryFormatter();
                currentDevicesList = bf.Deserialize(fs) as List<VersionInformation>;
                fs.Close();
            }

            this.notifyicon.Text = this.Text;
            this.notifyicon.DoubleClick += notifyIcon1_DoubleClick;

            // notifyIcon1_DoubleClick;
            btnConnect_Click(btnConnect, null);
        }

        private void AutoConnect_Tick(object sender, EventArgs e)
        {
            btnConnect_Click(btnConnect, null);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            //if (checkBox1.Checked) //设置开机自启动  
            //{
            //    MessageBox.Show("设置开机自启动，需要修改注册表", "提示");
            //    string path = Application.ExecutablePath;
            //    RegistryKey rk = Registry.LocalMachine;
            //    RegistryKey rk2 = rk.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run");
            //    rk2.SetValue("JcShutdown", path);
            //    rk2.Close();
            //    rk.Close();
            //}
            //else //取消开机自启动  
            //{
            //    MessageBox.Show("取消开机自启动，需要修改注册表", "提示");
            //    string path = Application.ExecutablePath;
            //    RegistryKey rk = Registry.LocalMachine;
            //    RegistryKey rk2 = rk.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run");
            //    rk2.DeleteValue("JcShutdown", false);
            //    rk2.Close();
            //    rk.Close();
            //}
        }

        private void ServerMain_SizeChanged(object sender, EventArgs e)
        {
            //判断是否选择的是最小化按钮 
            if (WindowState == FormWindowState.Minimized)
            {
                //托盘显示图标等于托盘图标对象 
                //注意notifyIcon1是控件的名字而不是对象的名字 
                //隐藏任务栏区图标 
                this.ShowInTaskbar = false;
                //图标显示在托盘区 
                notifyicon.Visible = true;
            }

        }

        #region 还原窗体
        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            //判断是否已经最小化于托盘 
            if (WindowState == FormWindowState.Minimized)
            {
                //还原窗体显示 
                WindowState = FormWindowState.Normal;
                //激活窗体并给予它焦点 
                this.Activate();
                //任务栏区显示图标 
                this.ShowInTaskbar = true;
                //托盘区图标隐藏 
                notifyicon.Visible = false;
                tabControl1.SelectedIndex = 1;
            }
        }
        #endregion

        void ReadEverythingFromExcel()
        {
            try
            {
                #region
                GridView1.DataSource = PublicMethods.LoadDataFromExcel(sPath);
                GridView1.Columns[0].Width = Convert.ToInt32(GridView1.Width * 0.12);
                GridView1.Columns[1].Width = Convert.ToInt32(GridView1.Width * 0.12);
                GridView1.Columns[2].Width = Convert.ToInt32(GridView1.Width * 0.3);
                GridView1.Columns[3].Width = Convert.ToInt32(GridView1.Width * 0.3);
                GridView1.Columns[4].Width = Convert.ToInt32(GridView1.Width * 0.1);
                GridView1.Columns[5].Width = Convert.ToInt32(GridView1.Width * 0.1);

                currentDevicesList = new List<VersionInformation>();
                for (int i = 0; i < GridView1.RowCount; i++)
                {
                    if (GridView1[5, i].Value == null || GridView1[0, i].Value.ToString() == "") continue;

                    VersionInformation vTmp = new VersionInformation();
                    vTmp.sModel = GridView1[0, i].Value.ToString();
                    vTmp.sVersion = GridView1[1, i].Value.ToString();
                    vTmp.sProduction = GridView1[2, i].Value.ToString();
                    vTmp.sFirmwareName = GridView1[3, i].Value.ToString();
                    vTmp.sSerialId = GridView1[5, i].Value.ToString();

                    currentDevicesList.Add(vTmp);
                }
                #endregion
            }
            catch { }
        }

        private void Exit_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("你确定要退出程序吗？", "确认", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.OK)
            {
                notifyicon.Visible = false;
                this.Close();
                this.Dispose();
            }
        }

        private void Hide_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void ShowIcon_Click(object sender, EventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
            this.Activate();
        }
    }
}
