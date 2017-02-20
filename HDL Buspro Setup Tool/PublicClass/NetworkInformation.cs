using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace HDL_Buspro_Setup_Tool
{
    public class NetworkInformation
    {
        public Byte[] ipAddress;
        public Byte[] routerIp;
        public Byte[] macAddress;
        public Byte[] ipGateway;
        public Byte[] port;   // 端口
        public Boolean dhcp; // 是否自动获取IP
        public Byte currentState;
        public Boolean dnsAuto; // 是否自动获取DNS
        public Byte[] dnsOne;
        public Byte[] dnsTwo; 

        public NetworkInformation()
        {
            ipAddress = new Byte[4] { 0, 0, 0, 0 };
            routerIp = new Byte[4] { 0, 0, 0, 0 };
            macAddress = new Byte[6] {0x48,0x44,0x4C, 0, 0, 0 };
            ipGateway = new Byte[4] { 0, 0, 0, 0 };
            port = new Byte[2] { 0,0};
            dhcp = false;
            dnsAuto = false;
            dnsOne = new Byte[4] { 0,0,0,0};
            dnsTwo = new Byte[4] { 0, 0, 0, 0 };
        }

        public void readNetworkInfomation(Byte SubNetID, Byte DeviceID)
        {
            try
            {
                byte[] ArayTmp = new byte[0];
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xF037, SubNetID, DeviceID, false, false, true, false) == true)
                {
                    int wdDeviceType = CsConst.myRevBuf[19] * 256 + CsConst.myRevBuf[20];

                    Array.Copy(CsConst.myRevBuf, 25, ipAddress, 0, 4);
                    Array.Copy(CsConst.myRevBuf, 29, routerIp, 0, 4);
                    Array.Copy(CsConst.myRevBuf, 36, macAddress, 3, 3);
                    Array.Copy(CsConst.myRevBuf, 39, port, 0, 2);
                    Array.Copy(CsConst.myRevBuf, 41, ipGateway, 0, 4);

                    dhcp = (CsConst.myRevBuf[45] == 1);
                    currentState = CsConst.myRevBuf[46];
                    dnsAuto = (CsConst.myRevBuf[47] == 1);

                    Array.Copy(CsConst.myRevBuf, 48, dnsOne, 0, 4);
                    Array.Copy(CsConst.myRevBuf, 52, dnsTwo, 0, 4);

                    CsConst.myRevBuf = new byte[1200];
                }
                else
                {
                    return;
                }
            }
            catch
            {
            }
        }

        public void ModifyNetworkInfomation(Byte SubNetID, Byte DeviceID)
        {
            try
            {
                byte[] ArayTmp = new byte[31];

                ipAddress.CopyTo(ArayTmp,0);
                routerIp.CopyTo(ArayTmp,4);
                macAddress.CopyTo(ArayTmp,8);
                port.CopyTo(ArayTmp,14);
                ipGateway.CopyTo(ArayTmp,16);
                ArayTmp[20] = Convert.ToByte(dhcp);
                ArayTmp[22] = Convert.ToByte(dnsAuto);

                Array.Copy(dnsOne, 0, ArayTmp, 23, 4);
                Array.Copy(dnsTwo, 0, ArayTmp, 27, 4);

                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xF039, SubNetID, DeviceID, false, true, true, false) == true)
                {
                }
                else
                {
                    return;
                }
            }
            catch
            {
            }
        }       
    }
}
