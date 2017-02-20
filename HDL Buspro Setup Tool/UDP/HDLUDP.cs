using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using System.Collections;
using System.IO;
using System.Linq;

namespace HDL_Buspro_Setup_Tool
{
    /// <summary>
    /// clsUdphj 的摘要说明。
    /// </summary>
    public class HDLUDP
    {
        [DllImport("crc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Check_crc(byte[] ptr, Int32 len);

        [DllImport("crc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Pack_crc(byte[] ptr, Int32 len);


        //=======================变量定义
        /// <summary>
        /// 环境监控端口
        /// </summary>
        public static Int32 ConstPort = 6000;

        public Socket hjSocket;	//socket实例，
        bool UdpStart = false;	//监控是否已经开始

        public Queue mSndQue = new Queue();

        public bool isConnected = false;	//是否正常


        byte waitNetid = 0;	//等待回应的网络id
        byte waitDevid = 0;	//等待回应的设备id
        int waitCommand = 0;	//等待回应的命令
        public static int waitDeviceType = 0; // 等待回应的设备类型
        bool watiforResponse = false;	//是否有数据回应

        byte[] asyncRevBuf = new byte[1200];
        byte[] asyncRevDmxBuf = new byte[1200];

        bool proSndBol = false;	//hjSend线程定义

        //场景字符串.
        public int curRead = 0;	//当前的场号.
        public string ReadMemo = null;

        //建立ping线程
        //============窗体定时器============================
        //功能1:定时检查网络连接
        private void SysCheck(object state)
        {
            //if (time_mutex == null) time_mutex = new object();
            //if (!Monitor.TryEnter(time_mutex, 100)) return;
            while (true)
            {
                try
                {
                    //clsWeb.CheckConnect();
                }
                catch (Exception) { }
                Thread.Sleep(1000);
            }
            //Monitor.Exit(time_mutex);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hjcommand">命令</param>
        /// <param name="svalue">附加数据包</param>
        public virtual void OnRxChar(int hjcommand, byte[] svalue) { }

        /// <summary>
        /// 异步发送过程
        /// </summary>
        /// <param name="iar"></param>
        private void OnEndSendCallback(IAsyncResult iar)
        {
            try
            {
                if (hjSocket != null && UdpStart)
                    hjSocket.EndSendTo(iar);
            }
            catch { }
        }

        /// <summary>
        /// 异步接收主处理过程
        /// </summary>
        /// <param name="state"></param>
        private void OnRecvCallback(IAsyncResult iar)
        {
            EndPoint ep = new IPEndPoint(IPAddress.Any, HDLUDP.ConstPort);
           // EndPoint ep = new IPEndPoint(IPAddress.Parse(CsConst.myDestIP), HDLUDP.ConstPort);
            IPEndPoint ipp = (IPEndPoint)ep;
            if (hjSocket != null)
            {
                int Datalen = 0;
                try
                {
                    Datalen = hjSocket.EndReceiveFrom(iar, ref ep);//终结异步接收
                    this.RevBuf(asyncRevBuf, ipp.Address.ToString(), ipp.Port.ToString());//将得到的数据包放到getBuf中处理
                }
                catch
                {
                    this.closeHJ();
                    return;
                }
                if (Datalen > 0)
                {
                    try
                    {                       
                        EndPoint epip = new IPEndPoint(IPAddress.Any, 0);
                        hjSocket.BeginReceiveFrom(asyncRevBuf, 0, asyncRevBuf.Length, SocketFlags.None, ref epip, new AsyncCallback(OnRecvCallback), "udp");//开始新的异步接
                       // this.RevBuf(asyncRevBuf, ipp.Address.ToString(), ipp.Port.ToString());//将得到的数据包放到getBuf中处理
                    }
                    catch
                    {
                        //MessageBox.Show(ex.ToString());
                    }

                }
            }
        }

        //=================处理数据======================
        void RevBuf(byte[] buf, string ip, string port)
        {
            try
            {
                string mb = "HDLMIRACLE";
                //根据协议标准分别处理。
                try
                {
                    if (System.Text.ASCIIEncoding.Default.GetString(buf, 4, 10) == mb)
                    {
                        DealWithRevDatas(buf); return;
                    }
                }
                catch { }
            }
            catch (Exception) { }
        }

        /// <summary>
        /// 关闭环境监控
        /// </summary>
        public void closeHJ()
        {
            try
            {
                isConnected = false;
                UdpStart = false;
                try
                {
                    proSndBol = false;
                    if (hjSocket != null)
                    {
                        hjSocket.Close();
                        hjSocket.Dispose();
                    }
                }
                catch { }
            }
            catch { }
        }

        /// <summary>
        /// 初始化环境控制
        /// </summary>
        /// <returns>成功初始化返回真，否则返回否</returns>
        public bool IniTheSocket(string tmpip)
        {
            try
            {
                CsConst.myLocalIP = tmpip;
                if (hjSocket == null)
                {
                    hjSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                    EndPoint ipep = new IPEndPoint(IPAddress.Parse(CsConst.myLocalIP), HDLUDP.ConstPort);
                    hjSocket.Bind(ipep);

                    EndPoint epip = new IPEndPoint(IPAddress.Parse(CsConst.myDestIP), HDLUDP.ConstPort);
                    hjSocket.BeginReceiveFrom(asyncRevBuf, 0, asyncRevBuf.Length, SocketFlags.None, ref epip, new AsyncCallback(OnRecvCallback), "udp");//开始新的异步接			
                }
                else
                {
                    this.closeHJ();
                    Thread.Sleep(200);
                    hjSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                    EndPoint ipep = new IPEndPoint(IPAddress.Parse(CsConst.myLocalIP), HDLUDP.ConstPort);
                    hjSocket.Bind(ipep);
                    EndPoint epip = new IPEndPoint(IPAddress.Parse(CsConst.myDestIP), HDLUDP.ConstPort);
                    hjSocket.BeginReceiveFrom(asyncRevBuf, 0, asyncRevBuf.Length, SocketFlags.None, ref epip, new AsyncCallback(OnRecvCallback), "udp");//开始新的异步接			
                }
                UdpStart = true;
                isConnected = true;
                proSndBol = true;
            }
            catch
            {
                UdpStart = false; isConnected = false; return false;
            }
            return true;
        }


        static void RecvThread()
        {
            UdpClient client = new UdpClient(7000);
            client.JoinMulticastGroup(IPAddress.Parse("224.0.168.188"));
            IPEndPoint multicast = new IPEndPoint(IPAddress.Parse("224.0.168.188"), 6000);
            while (true)
            {
                byte[] buf = client.Receive(ref multicast);
                //  string msg = "Receive from :" + Encoding.Default.GetString(buf);
                //MessageBox.Show(buf[0].ToString());
            }
        }

        /// <summary>
        /// 校验码验证
        /// </summary>
        /// <param name="pack">待验证的数据包</param>
        /// <returns>返回字符串</returns>
        public string crcpack(byte[] pack)
        {
            Pack_crc(pack, Convert.ToByte(pack.Length - 2));
            return ByteToString(pack);
        }

        /// <summary>
        /// byte转为字符串
        /// </summary>
        /// <param name="InBytes"></param>
        /// <returns></returns>
        public static string ByteToString(byte[] InBytes)
        {
            if (InBytes == null) return null;
            string StringOut = "";
            for (int i = 0; i < InBytes.Length; i++)
            {
                StringOut = StringOut + String.Format("{0:X2} ", InBytes[i]);
            }
            return StringOut;
        }

        /// <summary>
        /// 清空发送列表中的数据
        /// </summary>
        public void ClearBufInList()
        {
            mSndQue.Clear();
        }

        /// <summary>
        /// 环境数据打包
        /// </summary>
        /// <param name="broad"></param>
        /// <param name="dataPack"></param>
        /// <param name="command"></param>
        /// <param name="desNet"></param>
        /// <param name="desDevice"></param>
        /// <returns></returns>
        public byte[] PackAndSend(byte[] dataPack, int command, byte DesSubID, byte DesDevID, bool blnIsBigPack)
        {
            byte[] sPack = null;

            // 数据包头
            Byte[] PacketHead = new Byte[16]; 
            PacketHead[0] = byte.Parse(CsConst.myLocalIP.Split('.')[0].ToString());
            PacketHead[1] = byte.Parse(CsConst.myLocalIP.Split('.')[1].ToString());
            PacketHead[2] = byte.Parse(CsConst.myLocalIP.Split('.')[2].ToString());
            PacketHead[3] = byte.Parse(CsConst.myLocalIP.Split('.')[3].ToString());
            byte[] Signal = System.Text.ASCIIEncoding.Default.GetBytes("HDLMIRACLE");
            Array.Copy(Signal, 0, PacketHead, 4, 10);

            PacketHead[14] = 0xAA;
            PacketHead[15] = 0xAA;

            // 协议中间部分
            byte[] DataMiddlePart = new byte[9];
            DataMiddlePart[1] = CsConst.mbytLocalSubNetID;//本机子网ID;
            DataMiddlePart[2] = CsConst.mbytLocalDeviceID;//本机设备ID；
            DataMiddlePart[3] = Convert.ToByte(CsConst.mintLocalDeviceType / 256);
            DataMiddlePart[4] = Convert.ToByte(CsConst.mintLocalDeviceType % 256);
            DataMiddlePart[5] = Convert.ToByte(command / 256);
            DataMiddlePart[6] = Convert.ToByte(command % 256);
            DataMiddlePart[7] = DesSubID;
            DataMiddlePart[8] = DesDevID;

            //中间部分 + 附加数据 + CRC
            int DataPacketLength = 0;
            if (dataPack != null) DataPacketLength = Convert.ToInt32(dataPack.Length);

            Byte[] crcPack = null;
            if (blnIsBigPack == false)
            {
                DataMiddlePart[0] =(Byte)(DataPacketLength + 9 + 2);
                crcPack = new Byte[DataPacketLength + 9 + 2];
                DataMiddlePart.CopyTo(crcPack, 0);
                if (DataPacketLength >0) Array.Copy(dataPack, 0, crcPack, 9, DataPacketLength);

                Pack_crc(crcPack, crcPack.Length - 2);
            }
            else if (blnIsBigPack == true)
            {
                DataMiddlePart[0] = 0xFF;
                crcPack = new Byte[DataPacketLength + 9 + 2];
                DataMiddlePart.CopyTo(crcPack, 0);
                crcPack[9] = Convert.ToByte(DataPacketLength / 256);
                crcPack[10] = Convert.ToByte(DataPacketLength % 256);
                if (DataPacketLength > 0)  Array.Copy(dataPack, 0, crcPack, 11, DataPacketLength);
            }
            //CRC后+ 头返回
            sPack = new Byte[crcPack.Length + 16];
            PacketHead.CopyTo(sPack, 0);
            Array.Copy(crcPack, 0, sPack, 16, crcPack.Length);
            return sPack;

        }
        /// <summary>
        /// 添加新数据到缓冲区并发送
        /// </summary>
        public bool AddBufToSndList(byte[] dataPack, int command, byte DesSubID, byte DesDevID, bool blnIsbig, bool blnIsShowMessage, bool isReSend, bool isWireless)
        {
            DateTime t1, t2;
            bool blnIsRely = true;
            byte[] SendBuf = PackAndSend(dataPack, command, DesSubID, DesDevID, blnIsbig);
            byte[] tmpBuf = null;

            tmpBuf = new byte[SendBuf.Length];

            Array.Copy(SendBuf, 0, tmpBuf, 0, tmpBuf.Length);

            if (tmpBuf[21] * 256 + tmpBuf[22] == 0xD9E0) CsConst.MoreDelay = 4000;
            else CsConst.MoreDelay = 0;

            if (isWireless)
            {
                CsConst.replySpanTimes = 500;
                CsConst.replytimes = 5;
            }
            else
            {
                CsConst.replySpanTimes = 1000;
                CsConst.replytimes = 4;
            }

            #region
            if (command == 0xF81A && CsConst.MyBlnWait15FE == false)
            {
                waitCommand = 0xF81B;
            }
            else if ((command == 0xF81A && CsConst.MyBlnWait15FE == true))
            {
                waitCommand = 0x15FE;
                CsConst.MoreDelay = -1500;
            }
            else if (command == 0x15FF)
            {
                waitCommand = 0x15FE;
                CsConst.replytimes = 0;
            }
            else if (command == 0xE448) //simple search start
            {
                CsConst.MyBlnStartSimpleSearch = true;
            }
            else if (command == 0xE46A || command == 0x0007)  //写flash  DALI 分区
            {
                waitCommand = tmpBuf[21] * 256 + tmpBuf[22] + 1;
                CsConst.replySpanTimes = 2000;
                CsConst.replytimes = 6;
            }
            else
            {
                waitCommand = tmpBuf[21] * 256 + tmpBuf[22] + 1;
            }

            if (command == 0x1D3E)
            {
                waitDevid = tmpBuf[27];
                waitNetid = tmpBuf[26];
            }
            else
            {
                waitDevid = tmpBuf[24];
                waitNetid = tmpBuf[23];
            }
            #endregion

            this.watiforResponse = (command != 0x1970 && command != 0xF065 && command != 0xE457 && command != 0x13F2 && command != 0x15FE);	//打开等待回应信号
            int sendTimes = 1;	//已重发次数
           // if (CsConst.MyBlnCapture == true) CsConst.MyQuene.Add(tmpBuf);
            this.SendBufToRemote(tmpBuf, CsConst.myDestIP);//少于3次发送命令

            t1 = DateTime.Now;
            if (CsConst.FastSearch == true) //全面搜索特殊处理
            {
                CsConst.MoreDelay = 1000;               
                watiforResponse = false;
            }
            else      // 普通命令的读写
            {
                while (watiforResponse)
                {
                    t2 = DateTime.Now;
                    int TimeBetw = HDLSysPF.Compare(t2, t1);
                    if (TimeBetw >= CsConst.replySpanTimes + CsConst.MoreDelay)
                    {
                        if (!isReSend)
                        {
                            blnIsRely = false;
                            return blnIsRely;
                        }
                        this.SendBufToRemote(tmpBuf, CsConst.myDestIP);//少于3次发送命令
                        sendTimes = sendTimes + 1;
                        t1 = DateTime.Now;
                    }
                    
                    if (sendTimes >= CsConst.replytimes)
                    {
                        this.watiforResponse = false;
                        blnIsRely = false;
                        if (blnIsShowMessage)
                            HDLSysPF.ShowTimeoutMessage();
                        return blnIsRely;	//超过2次退出
                    }
                }
            }
            return blnIsRely;
        }

        /// <summary>
        /// 发送数据线程
        /// </summary>
        void ProSendBufOneByOne()
        {
            byte[] tmpBuf = null;	//设置临时缓冲区
            //byte[] ProBuf=null;	//处理好准备待发的缓冲区
            DateTime t1, t2;
            while (proSndBol)
            {               
                try
                {
                    if (mSndQue.Count > 0)
                    {
                        byte[] newBuf = (byte[])mSndQue.Dequeue();
                        if (newBuf == null) continue;
                        tmpBuf = new byte[newBuf[16] + 16];
                        Array.Copy(newBuf, 0, tmpBuf, 0, tmpBuf.Length);

                        waitCommand = tmpBuf[21] * 256 + tmpBuf[22] + 1;
                        waitDevid = tmpBuf[23];
                        waitNetid = tmpBuf[24];

                        if (tmpBuf[21] * 256 + tmpBuf[22] == 0xD9E0) CsConst.MoreDelay = 4000;
                        else CsConst.MoreDelay = 0;

                        this.watiforResponse = true;	//打开等待回应信号
                        int sendTimes = 1;	//已重发次数
                        this.SendBufToRemote(tmpBuf, CsConst.myDestIP);//少于3次发送命令

                        t1 = DateTime.Now;
                        while (watiforResponse)
                        {
                            t2 = DateTime.Now;
                            int TimeBetw = HDLSysPF.Compare(t2, t1);
                            if (TimeBetw >= CsConst.replySpanTimes + CsConst.MoreDelay)
                            {
                                this.SendBufToRemote(tmpBuf, CsConst.myDestIP);//少于3次发送命令
                                sendTimes = sendTimes + 1;
                                t1 = DateTime.Now;
                            }
                            if (sendTimes >= CsConst.replytimes)
                            {
                                this.watiforResponse = false;
                                break;	//超过2次退出
                            }
                        }
                        this.watiforResponse = false;
                    }
                }
                catch { }
            }
        }

        /// <summary>
        /// 环境数据打包
        /// </summary>
        /// <param name="broad"></param>
        /// <param name="dataPack"></param>
        /// <param name="command"></param>
        /// <param name="desNet"></param>
        /// <param name="desDevice"></param>
        /// <returns></returns>
        public byte[] pack(int broad, byte[] dataPack, int command, byte desNet, byte desDevice)
        {
            byte[] sPack = new byte[128];
            sPack[0] = byte.Parse(CsConst.myLocalIP.Split('.')[0].ToString());
            sPack[1] = byte.Parse(CsConst.myLocalIP.Split('.')[1].ToString());
            sPack[2] = byte.Parse(CsConst.myLocalIP.Split('.')[2].ToString());
            sPack[3] = byte.Parse(CsConst.myLocalIP.Split('.')[3].ToString());

            Array.Copy(StringToByte(CsConst.myLocalIP), 0, sPack, 0, 4);
            byte[] Signal = System.Text.ASCIIEncoding.Default.GetBytes("HDLMIRACLE");
            Array.Copy(Signal, 0, sPack, 4, 10);
            sPack[14] = Convert.ToByte(broad / 256);
            sPack[15] = Convert.ToByte(broad % 256);
            byte[] crcPack = null;
            int leng = 11;
            if (dataPack != null)
            {
                leng = dataPack.Length + leng;
            }
            crcPack = new byte[leng];
            if (CsConst.BigPacket == true)
            {
                crcPack[0] = 0xFF;
            }
            else
            {
                crcPack[0] = byte.Parse(leng.ToString());
            }
            CsConst.BigPacket = false;
            //crcPack[0] = 0xFF;   //ff,大包控制协议}//byte.Parse(leng.ToString());
            crcPack[1] = CsConst.mbytLocalSubNetID;//StringToByte(ipByte())[2];
            crcPack[2] = CsConst.mbytLocalDeviceID;//StringToByte(ipByte())[3];
            crcPack[3] = 0xFF;  //我本机类型 2 byte
            crcPack[4] = 0xFD;//byte.Parse(Convert.ToInt32("0xFD",16).ToString());
            crcPack[5] = Convert.ToByte(command / 256);
            crcPack[6] = Convert.ToByte(command % 256);
            crcPack[7] = desNet;
            crcPack[8] = desDevice;//byte.Parse(Convert.ToInt32("0XFF",16).ToString());
            if (dataPack != null)
                Array.Copy(dataPack, 0, crcPack, 9, dataPack.Length);
            crcPack[crcPack.Length - 2] = 0;
            crcPack[crcPack.Length - 1] = 0;
            Pack_crc(crcPack, crcPack.Length - 2);
            Array.Copy(crcPack, 0, sPack, 16, crcPack.Length);
            byte[] outBuf = new byte[16 + crcPack.Length];
            Array.Copy(sPack, 0, outBuf, 0, outBuf.Length);
            return outBuf;
        }


        /// <summary>
        /// 字符串转为byte
        /// </summary>
        /// <param name="InString"></param>
        /// <returns></returns>
        public static Byte[] StringToByte(String InString)
        {
            Byte[] ByteOut = null;
            try
            {
                ByteOut = System.Text.Encoding.Default.GetBytes(InString);
            }
            catch { }
            return ByteOut;
        }

        /// <summary>
        /// 字符串转为byte
        /// </summary>
        /// <param name="InString"></param>
        /// <returns></returns>
        public static Byte[] StringTo2Byte(String InString, Boolean blnVerse)
        {
            Byte[] ByteOut = null;
            try
            {
                ByteOut = System.Text.Encoding.Unicode.GetBytes(InString);

                if (blnVerse)
                {
                    for (int intI = 0; intI < ByteOut.Length / 2; intI++)
                    {
                        byte bytVale = ByteOut[intI * 2];
                        ByteOut[intI * 2] = ByteOut[intI * 2 + 1];
                        ByteOut[intI * 2 + 1] = bytVale;
                    }
                }
            }
            catch { }
            return ByteOut;
        }

        /// <summary>
        /// 从数据中提取附加数据包
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public byte[] getPack(byte[] stream)
        {
            int dPackLen = stream[16];
            byte[] mDPack = new byte[dPackLen];//从流中截取有用数据包
            Array.Copy(stream, 16, mDPack, 0, dPackLen);

            if (DataCRC(mDPack, dPackLen - 2))
            {
                return mDPack;
            }
            else
            {
                return null;
            }
        }


        //========处理设备返回信息========
        private void proDevBuf(byte[] buf)
        {
            try
            {
                string sndIp = string.Format("{0}.{1}.{2}.{3}", buf[0], buf[1], buf[2], buf[3]);
                //来自/发送到  设备类型:　ip : 数据包:
                if (buf[7] == 3) return;
                if (buf[8] == 3)
                {
                    int command = buf[9] * 256 + buf[10];
                    if (watiforResponse && this.waitDevid == buf[7])
                    {
                        this.watiforResponse = false;
                        return;
                    }
                }
            }
            catch
            {
                //MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// 接收线程
        /// </summary>
        private void DealWithRevDatas(byte[] buf)
        {
            byte[] revBuf = buf;
            try
            {
                if ((revBuf[14] != 0xAA) & (revBuf[15] != 0xAA) & (revBuf[15] != 0x55)) return;
                if (CsConst.myRevBuf == null) CsConst.myRevBuf = new byte[1200];
                string[] ipHeader = CsConst.myLocalIP.Split('.');  ////A B C 类地址广播
                int mhjCommand = revBuf[21] * 256 + revBuf[22];
                int devType = revBuf[19] * 256 + revBuf[20];                

                int netid = revBuf[17];	//设备子网id
                int devid = revBuf[18];	//设备号

                int wdDeviceType = revBuf[19] * 256 + revBuf[20];
                int intBigSize = 0; // 大包数据长度默认为0
                if (revBuf[16] == 0xFF)
                {
                    intBigSize = revBuf[25] * 256 + revBuf[26] + 27; // 长度 + 协议头 
                }

                #region
                if (CsConst.MyBlnCapture == true)
                {
                    CsConst.myRevBuf = new byte[1200];

                    if (revBuf[16] == 0xFF)
                    {
                        Array.Copy(revBuf, 0, CsConst.myRevBuf, 0, intBigSize);
                    }
                    else
                    {
                        Array.Copy(revBuf, 0, CsConst.myRevBuf, 0, revBuf[16] + 17);// copy to the public buffer
                    }
                    CsConst.MyQuene.Add(CsConst.myRevBuf);
                }
                #endregion
                if (CsConst.bStartSimpleTesting == true)
                {
                    byte[] arayTmp = new byte[revBuf[16] + 17];
                    Array.Copy(revBuf, 0, arayTmp, 0, revBuf[16] + 17);
                    UDPReceive.OnReceive(arayTmp);
                    return;
                }

                if ((revBuf[17] == CsConst.mbytLocalSubNetID) & (revBuf[18] == CsConst.mbytLocalDeviceID)) return;
                // 公共的升级或者其他指令
                #region
                if (CsConst.NeedAppendToPublicBufferCommands.Contains(waitCommand) && waitCommand == mhjCommand && waitNetid == revBuf[17] && waitDevid == revBuf[18])
                {
                    if (intBigSize > 0)
                    {
                        byte[] arayTmp = new byte[revBuf[25] * 256 + revBuf[26] + 27];
                        Array.Copy(revBuf, 0, arayTmp, 0, arayTmp.Length);
                        UDPReceive.OnReceiveForBuffer(arayTmp);
                        this.watiforResponse = false;
                    }
                    else
                    {
                        byte[] arayTmp = new byte[revBuf[16] + 17];
                        Array.Copy(revBuf, 0, arayTmp, 0, revBuf[16] + 17);
                        UDPReceive.OnReceiveForBuffer(arayTmp);
                        //revBuf.CopyTo(CsConst.myRevBuf, 0);
                        if (CsConst.MyBlnNeedF8)
                        {
                            if (revBuf[25] == 0xF8)
                            {
                                CsConst.MyBlnNeedF8 = false;
                                this.watiforResponse = false;
                            }
                        }
                        else
                        {
                            this.watiforResponse = false;
                        }
                    }
                }
                #endregion
                if (CsConst.FastSearch)
                {
                    if (mhjCommand == 0x000F || mhjCommand == 0xF004 || mhjCommand == 0x1D35 || mhjCommand == 0xE549)//广播搜索设备
                    {
                        byte[] arayTmp = new byte[revBuf[16] + 17];
                        Array.Copy(revBuf, 0, arayTmp, 0, revBuf[16] + 17);
                        UDPReceive.OnReceive(arayTmp);
                        return;
                    }
                }
                else
                {
                   // if (this.watiforResponse == false) return;
                    if (mhjCommand == 0x3001)
                    {
                        CsConst.mintFactoryTime = revBuf[28];
                        if (this.watiforResponse == true) this.watiforResponse = false;
                        return;
                    }
                    else if (mhjCommand == 0x301E)
                    {
                        Byte[] TmpExcahngePort = new Byte[12];
                        TmpExcahngePort[0] = byte.Parse(CsConst.myLocalIP.Split('.')[0].ToString());
                        TmpExcahngePort[1] = byte.Parse(CsConst.myLocalIP.Split('.')[1].ToString());
                        TmpExcahngePort[2] = byte.Parse(CsConst.myLocalIP.Split('.')[2].ToString());
                        TmpExcahngePort[3] = byte.Parse(CsConst.myLocalIP.Split('.')[3].ToString());
                        TmpExcahngePort[4] = 0x17;
                        TmpExcahngePort[5] = 0x70;
                        Array.Copy(CsConst.myRevBuf, 25, TmpExcahngePort, 6, 6);
                        this.AddBufToSndList(TmpExcahngePort, 0x301D, 255, 255, false, false, false, false);
                        ConstPort = CsConst.myRevBuf[29] * 256 + CsConst.myRevBuf[30];
                        string TmpIp= CsConst.myRevBuf[25].ToString() + "."
                                    + CsConst.myRevBuf[26].ToString() + "."
                                    + CsConst.myRevBuf[27].ToString() + "."
                                    + CsConst.myRevBuf[28].ToString();
                        for (int i = 0; i <= 4; i++)
                        {
                            this.SendBufToRemote(null, TmpIp);
                        }
                    }
                    else if (mhjCommand == 0x1365 && revBuf[17] == waitNetid && revBuf[18] == waitDevid)
                    {
                        #region
                        if (revBuf[16] == 0xFF)
                        {
                            if (revBuf[27] == 12)
                            {
                                if (revBuf[28] == 0xFF)
                                {
                                    AudioDeviceTypeList.isEndReceiveFile = true;
                                    return;
                                }
                                byte[] arayTmp = new byte[revBuf[25] * 256 + revBuf[26]];
                                Array.Copy(revBuf, 27, arayTmp, 0, arayTmp.Length);
                                UDPReceive.OnReceiveForAudio(arayTmp);
                                return;
                            }
                            else if (revBuf[27] == 13)
                            {
                                byte[] arayTmp = new byte[revBuf[25] * 256 + revBuf[26] - 12];
                                Array.Copy(revBuf, 39, arayTmp, 0, arayTmp.Length);
                                UDPReceive.OnReceiveForAudio(arayTmp);
                                return;
                            }
                            else //if (revBuf[27] == 11)
                            {
                                revBuf.CopyTo(CsConst.myRevBuf, 0);// copy to the public buffer
                                this.watiforResponse = false;
                                return;
                            }
                        }
                        return;
                        #endregion
                    }
                    else if (waitCommand == mhjCommand && (mhjCommand == 0x300E || mhjCommand == 0x3010 || mhjCommand == 0x3012 ||
                                                      mhjCommand == 0x3014 || mhjCommand == 0x301E || mhjCommand == 0x3020))
                    {
                        revBuf.CopyTo(CsConst.myRevBuf, 0);// copy to the public buffer
                        this.watiforResponse = false;
                        return;
                    }
                    else if ((mhjCommand == 0xE449) && revBuf[17] == waitNetid && revBuf[18] == waitDevid && watiforResponse == false)
                    {
                        byte[] arayTmp = new byte[revBuf[16] + 17];
                        Array.Copy(revBuf, 0, arayTmp, 0, revBuf[16] + 17);
                        UDPReceive.OnReceive(arayTmp);
                        this.watiforResponse = false;
                        return;
                    }
                    else if (CsConst.MyBlnWait15FE ==true && mhjCommand == 0x15FE)  //被动等待接收15FE的命令 第一次接收
                    {
                        byte[] arayTmp = new byte[revBuf[16] + 17];
                        Array.Copy(revBuf, 0, arayTmp, 0, revBuf[16] + 17);
                        UDPReceive.OnReceiveForUpgrade(arayTmp);
                        return;
                    }
                    else if (waitCommand == 0xF006)
                    {
                        if (revBuf[17] == CsConst.ModifyDeviceAddressSubNetID && revBuf[18] == CsConst.ModifyDeviceAddressDeviceID)
                        {
                            this.watiforResponse = false;
                            return;
                        }
                    }
                    else if (waitCommand == 0xE5F6 || waitCommand == 0xE5F8 || waitCommand == 0xFE04 || waitCommand == 0xFE06 || waitCommand == 0x1385)
                    {
                        byte[] arayTmp = new byte[revBuf[16] + 17];
                        Array.Copy(revBuf, 0, arayTmp, 0, revBuf[16] + 17);
                        UDPReceive.OnReceive(arayTmp);
                        this.watiforResponse = false;
                        return;
                    }
                    else if (waitCommand == 0xFE06)
                    {
                        if (revBuf[25] == 0xF8)
                        {
                            this.watiforResponse = false;
                            return;
                        }
                    }
                    else if (waitCommand == 0x13F5) //自动修改地址
                    {
                        CsConst.AutoAddress = revBuf[25];
                        return;
                    }
                    else if (mhjCommand == 0xE3E1)
                    {
                        if (CsConst.isAutoRefreshCurtainPercent)
                        {
                            byte[] arayTmp = new byte[revBuf[16] + 17];
                            Array.Copy(revBuf, 0, arayTmp, 0, revBuf[16] + 17);
                            UDPReceive.OnReceiveForCurtain(arayTmp);
                            return;
                        }
                    }
                    else if (mhjCommand == 0x1D51)
                    {

                        byte[] arayTmp = new byte[revBuf[16] + 17];
                        Array.Copy(revBuf, 0, arayTmp, 0, revBuf[16] + 17);
                        UDPReceive.OnReceiveForSingnal(arayTmp);
                        return;
                    }
                    else if (watiforResponse == true && CsConst.MyBlnNeedF8 && CsConst.MybytNeedParm1 != -1 && CsConst.MybytNeedParm2 != -1
                        && CsConst.MybytNeedParm3 != -1 && CsConst.mbytCurUsingSubNetID == revBuf[17]
                        && CsConst.mbytCurUsingDevNetID == revBuf[18] && waitCommand == mhjCommand) //是不是F8 有参数匹配才确认 第一个 第二个  第三个参数匹配
                    {
                        if (CsConst.myRevBuf == null) CsConst.myRevBuf = new byte[1200];
                        if (revBuf[25] == 0xF8 && CsConst.MybytNeedParm1 == revBuf[26] && CsConst.MybytNeedParm2 == revBuf[27] && CsConst.MybytNeedParm3 == revBuf[28])
                        {
                            if (revBuf[16] == 0xFF)
                            {
                                Array.Copy(revBuf, 0, CsConst.myRevBuf, 0, revBuf[25] * 256 + revBuf[26] + 27);
                            }
                            else
                            {
                                Array.Copy(revBuf, 0, CsConst.myRevBuf, 0, revBuf[16] + 17);// copy to the public buffer
                            }
                            if (watiforResponse == true) this.watiforResponse = false;
                            CsConst.MyBlnNeedF8 = false;
                            CsConst.MybytNeedParm1 = -1;
                            CsConst.MybytNeedParm2 = -1;
                            CsConst.MybytNeedParm3 = -1;
                            return;
                        }
                    }
                    else if (watiforResponse == true && CsConst.MyBlnNeedF8 && CsConst.MybytNeedParm1 != -1 && CsConst.MybytNeedParm2 != -1 && CsConst.mbytLocalSubNetID == revBuf[23]
                        && CsConst.mbytLocalDeviceID == revBuf[24] && waitCommand == mhjCommand) //是不是F8 有参数匹配才确认 第一个 第二个 参数匹配
                    {
                        if (CsConst.myRevBuf == null) CsConst.myRevBuf = new byte[1200];
                        if (revBuf[25] == 0xF8 && CsConst.MybytNeedParm1 == revBuf[26] && CsConst.MybytNeedParm2 == revBuf[27])
                        {
                            if (revBuf[16] == 0xFF)
                            {
                                Array.Copy(revBuf, 0, CsConst.myRevBuf, 0, revBuf[25] * 256 + revBuf[26] + 27);
                            }
                            else
                            {
                                Array.Copy(revBuf, 0, CsConst.myRevBuf, 0, revBuf[16] + 17);// copy to the public buffer
                            }
                            this.watiforResponse = false;
                            CsConst.MyBlnNeedF8 = false;
                            CsConst.MybytNeedParm1 = -1;
                            CsConst.MybytNeedParm2 = -1;
                            return;
                        }
                    }
                    else if (watiforResponse == true && CsConst.MybytNeedParm1 != -1 && CsConst.MybytNeedParm2 != -1 && waitCommand == mhjCommand) //是不是F8 有参数匹配才确认 第一个 第二个 参数匹配
                    {
                        if (CsConst.myRevBuf == null) CsConst.myRevBuf = new byte[1200];
                        if (CsConst.MybytNeedParm1 == revBuf[25] && CsConst.MybytNeedParm2 == revBuf[26])
                        {
                            if (revBuf[16] == 0xFF)
                            {
                                Array.Copy(revBuf, 0, CsConst.myRevBuf, 0, revBuf[25] * 256 + revBuf[26] + 27);
                            }
                            else
                            {
                                Array.Copy(revBuf, 0, CsConst.myRevBuf, 0, revBuf[16] + 17);// copy to the public buffer
                            }
                            this.watiforResponse = false;
                            CsConst.MybytNeedParm1 = -1;
                            CsConst.MybytNeedParm2 = -1;
                            return;
                        }
                    }
                    else if (watiforResponse == true && CsConst.MyBlnNeedF8 && CsConst.MybytNeedParm1 != -1 && CsConst.mbytLocalSubNetID == revBuf[23]
                             && CsConst.mbytLocalDeviceID == revBuf[24] && waitCommand == mhjCommand) //是不是F8 有参数匹配才确认 第一个参数匹配
                    {
                        if (CsConst.myRevBuf == null) CsConst.myRevBuf = new byte[1200];
                        if (CsConst.myRevBuf[25] == 0xF8 && CsConst.MybytNeedParm1 == CsConst.myRevBuf[26])
                        {
                            if (revBuf[16] == 0xFF)
                            {
                                Array.Copy(revBuf, 0, CsConst.myRevBuf, 0, revBuf[25] * 256 + revBuf[26] + 27);
                            }
                            else
                            {
                                Array.Copy(revBuf, 0, CsConst.myRevBuf, 0, revBuf[16] + 17);// copy to the public buffer
                            }
                            if (watiforResponse == true) this.watiforResponse = false;
                            CsConst.MyBlnNeedF8 = false;
                            CsConst.MybytNeedParm1 = -1;
                            CsConst.MybytNeedParm2 = -1;
                            return;
                        }
                    }
                    else if (watiforResponse == true && CsConst.MyBlnNeedF8 && CsConst.mbytCurUsingSubNetID == revBuf[17]
                        && CsConst.mbytCurUsingDevNetID == revBuf[18] && waitCommand == mhjCommand) //是不是F8 有参数匹配才确认
                    {
                        if (CsConst.myRevBuf == null) CsConst.myRevBuf = new byte[1200];
                        if (CsConst.myRevBuf[25] == 0xF8)
                        {
                            if (revBuf[16] == 0xFF)
                            {
                                Array.Copy(revBuf, 0, CsConst.myRevBuf, 0, revBuf[25] * 256 + revBuf[26] + 27);
                            }
                            else
                            {
                                Array.Copy(revBuf, 0, CsConst.myRevBuf, 0, revBuf[16] + 17);// copy to the public buffer
                            }
                            if (watiforResponse == true) this.watiforResponse = false;
                            return;
                        }
                    }
                    else if (watiforResponse == true && CsConst.mbytCurUsingSubNetID == revBuf[17]
                           && CsConst.mbytCurUsingDevNetID == revBuf[18] && waitCommand == mhjCommand) //是不是只有参数匹配一才确认
                    {
                        if (CsConst.myRevBuf == null) CsConst.myRevBuf = new byte[1200];
                        if (CsConst.myRevBuf[25] == CsConst.MybytNeedParm1)
                        {
                            if (revBuf[16] == 0xFF)
                            {
                                Array.Copy(revBuf, 0, CsConst.myRevBuf, 0, revBuf[25] * 256 + revBuf[26] + 27);
                            }
                            else
                            {
                                Array.Copy(revBuf, 0, CsConst.myRevBuf, 0, revBuf[16] + 17);// copy to the public buffer
                            }
                            if (watiforResponse == true) this.watiforResponse = false;
                            return;
                        }
                    }
                    else if (waitCommand == mhjCommand && waitDevid == revBuf[18] && waitNetid == revBuf[17])
                    {
                        revBuf.CopyTo(CsConst.myRevBuf, 0);// copy to the public buffer
                        this.watiforResponse = false;
                    }
                    return;
                }
            }
            catch
            {
            }
        }


        /// <summary>
        /// 有目的地的发送数据给设备
        /// </summary>
        /// <param name="buf"></param>
        /// <param name="ip"></param>
        public void SendBufToRemote(byte[] buf, string ip)
        {
            if (hjSocket == null) IniTheSocket(CsConst.myLocalIP);

            try
            {
                if (CsConst.MyBlnCapture == true && CsConst.MyEnterProjectWay != 0) CsConst.MyQuene.Add(buf);
                EndPoint ep = new IPEndPoint(IPAddress.Parse(ip), ConstPort);
                hjSocket.BeginSendTo(buf, 0, buf.Length, SocketFlags.None, ep, new AsyncCallback(this.OnEndSendCallback), "Ok");
            }
            catch
            { }
        }


        #region crctab
        private static ushort[] crctab = new ushort[256]
            {  
                0x0000, 0x1021, 0x2042, 0x3063, 0x4084, 0x50a5, 0x60c6, 0x70e7,
		        0x8108, 0x9129, 0xa14a, 0xb16b, 0xc18c, 0xd1ad, 0xe1ce, 0xf1ef,
		        0x1231, 0x0210, 0x3273, 0x2252, 0x52b5, 0x4294, 0x72f7, 0x62d6,
		        0x9339, 0x8318, 0xb37b, 0xa35a, 0xd3bd, 0xc39c, 0xf3ff, 0xe3de,
		        0x2462, 0x3443, 0x0420, 0x1401, 0x64e6, 0x74c7, 0x44a4, 0x5485,
		        0xa56a, 0xb54b, 0x8528, 0x9509, 0xe5ee, 0xf5cf, 0xc5ac, 0xd58d,
		        0x3653, 0x2672, 0x1611, 0x0630, 0x76d7, 0x66f6, 0x5695, 0x46b4,
		        0xb75b, 0xa77a, 0x9719, 0x8738, 0xf7df, 0xe7fe, 0xd79d, 0xc7bc,
		        0x48c4, 0x58e5, 0x6886, 0x78a7, 0x0840, 0x1861, 0x2802, 0x3823,
		        0xc9cc, 0xd9ed, 0xe98e, 0xf9af, 0x8948, 0x9969, 0xa90a, 0xb92b,
		        0x5af5, 0x4ad4, 0x7ab7, 0x6a96, 0x1a71, 0x0a50, 0x3a33, 0x2a12,
		        0xdbfd, 0xcbdc, 0xfbbf, 0xeb9e, 0x9b79, 0x8b58, 0xbb3b, 0xab1a,
		        0x6ca6, 0x7c87, 0x4ce4, 0x5cc5, 0x2c22, 0x3c03, 0x0c60, 0x1c41,
		        0xedae, 0xfd8f, 0xcdec, 0xddcd, 0xad2a, 0xbd0b, 0x8d68, 0x9d49,
		        0x7e97, 0x6eb6, 0x5ed5, 0x4ef4, 0x3e13, 0x2e32, 0x1e51, 0x0e70,
		        0xff9f, 0xefbe, 0xdfdd, 0xcffc, 0xbf1b, 0xaf3a, 0x9f59, 0x8f78,
		        0x9188, 0x81a9, 0xb1ca, 0xa1eb, 0xd10c, 0xc12d, 0xf14e, 0xe16f,
		        0x1080, 0x00a1, 0x30c2, 0x20e3, 0x5004, 0x4025, 0x7046, 0x6067,
		        0x83b9, 0x9398, 0xa3fb, 0xb3da, 0xc33d, 0xd31c, 0xe37f, 0xf35e,
		        0x02b1, 0x1290, 0x22f3, 0x32d2, 0x4235, 0x5214, 0x6277, 0x7256,
		        0xb5ea, 0xa5cb, 0x95a8, 0x8589, 0xf56e, 0xe54f, 0xd52c, 0xc50d,
		        0x34e2, 0x24c3, 0x14a0, 0x0481, 0x7466, 0x6447, 0x5424, 0x4405,
		        0xa7db, 0xb7fa, 0x8799, 0x97b8, 0xe75f, 0xf77e, 0xc71d, 0xd73c,
		        0x26d3, 0x36f2, 0x0691, 0x16b0, 0x6657, 0x7676, 0x4615, 0x5634,
		        0xd94c, 0xc96d, 0xf90e, 0xe92f, 0x99c8, 0x89e9, 0xb98a, 0xa9ab,
		        0x5844, 0x4865, 0x7806, 0x6827, 0x18c0, 0x08e1, 0x3882, 0x28a3,
		        0xcb7d, 0xdb5c, 0xeb3f, 0xfb1e, 0x8bf9, 0x9bd8, 0xabbb, 0xbb9a,
		        0x4a75, 0x5a54, 0x6a37, 0x7a16, 0x0af1, 0x1ad0, 0x2ab3, 0x3a92,
		        0xfd2e, 0xed0f, 0xdd6c, 0xcd4d, 0xbdaa, 0xad8b, 0x9de8, 0x8dc9,
		        0x7c26, 0x6c07, 0x5c64, 0x4c45, 0x3ca2, 0x2c83, 0x1ce0, 0x0cc1,
		        0xef1f, 0xff3e, 0xcf5d, 0xdf7c, 0xaf9b, 0xbfba, 0x8fd9, 0x9ff8,
		        0x6e17, 0x7e36, 0x4e55, 0x5e74, 0x2e93, 0x3eb2, 0x0ed1, 0x1ef0
            };
        #endregion

        private ushort xcrc(ushort crc, byte cp)
        {
            ushort t1 = 0, t2 = 0, t3 = 0, t4 = 0, t5 = 0, t6 = 0;
            t1 = (ushort)(crc >> 8);
            t2 = (ushort)(t1 & 0xff);
            t3 = (ushort)(cp & 0xff);
            t4 = (ushort)(crc << 8);
            t5 = (ushort)(t2 ^ t3);
            t6 = (ushort)(crctab[t5] ^ t4);
            return t6;
        }


        public bool DataCRC(byte[] bufout, int count)
        {
            ushort crc16 = 0;
            byte i;
            for (i = 0; i < (count - 2); i++)
                crc16 = xcrc(crc16, bufout[i]);

            if ((bufout[count - 2] == (byte)(crc16 >> 8)) && (bufout[count - 1] == (byte)(crc16 & 0xff)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        #region By ARP

        byte[] PubKey = new byte[] { 6, 1, 8, 8, 1, 6, 1, 9, 7, 7, 0, 9, 1, 4, 6, 7, 9 };


        public static string GetLocalIP()
        {
            string strIP = "";
            IPAddress[] arrIPAddresses = Dns.GetHostAddresses(Dns.GetHostName());
            foreach (IPAddress ip in arrIPAddresses)
            {
                if (ip.AddressFamily.Equals(AddressFamily.InterNetwork))
                {
                    strIP = ip.ToString();
                    break;
                }
            }
            return strIP;
        }

        #endregion


        /// <summary>
        /// 发送数据之间的间隔
        /// </summary>
        /// <param name="bytLen"></param>
        public static void TimeBetwnNext(int bytLen)
        {
            Thread.Sleep(bytLen * 10);
        }

    }
}
