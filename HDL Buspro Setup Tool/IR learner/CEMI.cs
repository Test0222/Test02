using System;
using System.Collections.Generic;
using System.Text;

namespace HDL_Buspro_Setup_Tool
{
    public class CEMI
    {
        private int mc;
        private int addLength;
        private string addInfo;
        private int ctrl1;
        private int ctrl2;
        private int srcAddress;
        private int desAddress;
        public CEMI()
        {
            mc = 0;
            addLength = 0;
            addInfo = "";
            ctrl1 = 0;
            ctrl2 = 0;
            srcAddress = 0;
            desAddress = 0;
        }
        public CEMI(int mc, int ctrl1, int ctrl2, int srcAddress, int desAddress)
        {
            this.mc = mc;
            this.ctrl1 = ctrl1;
            this.ctrl2 = ctrl2;
            this.srcAddress = srcAddress;
            this.desAddress = desAddress;
            addLength = 0;
            addInfo = "";
        }
        public CEMI(int mc, AddInfo ad, int ctrl1, int ctrl2, int srcAddress, int desAddress)
        {
            this.mc = mc;
            this.ctrl1 = ctrl1;
            this.ctrl2 = ctrl2;
            this.srcAddress = srcAddress;
            this.desAddress = desAddress;
            this.addLength = ad.Length;
            this.addInfo = ad.Info;
        }
        /// <summary>
        /// 设置CEMI
        /// </summary>
        /// <returns></returns>
        public string SetCEMI()
        {
            string temp = "";
            temp += GlobalClass.AddLeftZero(mc.ToString("X"), 2);
            temp += GlobalClass.AddLeftZero(addLength.ToString("X"), 2);
            temp += GlobalClass.AddLeftZero(addInfo, addLength);
            temp += GlobalClass.AddLeftZero(ctrl1.ToString("X"), 2);
            temp += GlobalClass.AddLeftZero(ctrl2.ToString("X"), 2);
            temp += GlobalClass.AddLeftZero(srcAddress.ToString("X"), 4);
            temp += GlobalClass.AddLeftZero(desAddress.ToString("X"), 4);
            return temp;
        }
        /// <summary>
        /// 获取CEMI
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public string GetCEMI(byte[] data)
        {
            mc = Convert.ToInt32(data[0]);
            addLength = Convert.ToInt32(data[1]);
            addInfo = GlobalClass.ByteToHex(data, 2, addLength);
            ctrl1=Convert.ToInt32(data[2+addLength]);
            ctrl2=Convert.ToInt32(data[3+addLength]);
            srcAddress = GlobalClass.ByteToInt32(data, 4 + addLength, 2);
            desAddress = GlobalClass.ByteToInt32(data, 6 + addLength, 2);
            return GlobalClass.ByteToHex(data);
        }
        /// <summary>
        /// 获取CEMI
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public string GetCEMI(string data)
        {
            return GetCEMI(GlobalClass.HexToByte(data));
        }
        /// <summary>
        /// 获取MC
        /// </summary>
        public int MC
        {
            get
            {
                return mc;
            }
        }
        /// <summary>
        /// 获取增加长度
        /// </summary>
        public int AddLength
        {
            get
            {
                return addLength;
            }

        }
        /// <summary>
        /// 获取增加信息
        /// </summary>
        public string AddInfo
        {
            get
            {
                return addInfo;
            }
        }
        /// <summary>
        /// 获取Ctrl1
        /// </summary>
        public int Ctrl1
        {
            get
            {
                return ctrl1;
            }
        }
        /// <summary>
        /// 获取Ctrl2
        /// </summary>
        public int Ctrl2
        {
            get
            {
                return ctrl2;
            }
        }
        /// <summary>
        /// 获取或者获取源地址
        /// </summary>
        public int SrcAddress
        {
            get
            {
                return srcAddress;
            }
            set
            {
                srcAddress = value;
            }
        }
        /// <summary>
        /// 获取目的地址
        /// </summary>
        public int DesAddress
        {
            get
            {

                return desAddress;
            }
            set
            {
                desAddress = value;
            }
        }
        /// <summary>
        /// 获取目的地址
        /// </summary>
        /// <returns></returns>
        public string GetDesAddress()
        {
            return GlobalClass.AddLeftZero(desAddress.ToString("X"), 4);
        }
    }

    public class AddInfo
    {
        private int length;

        public int Length
        {
            get
            {
                return length;
            }
        }

        public string GetAddInfo()
        {
            return "";
        }
        public string Info
        {
            get
            {
                return GetAddInfo(); 
            }
        }
    }

    public class NPDU
    {
        private int length;
        private int apci;
        private int apciData;
        private string data;
        private bool isOneToOne;
        private int sendTime;
        public NPDU()
        {
            length = 0;
            apci = 0;
            apciData = 0;
            data = "";
            isOneToOne = false;
            sendTime = 0;
        }
        public NPDU(int length, int apci, int apciData, string data)
        {
            this.length = length;
            this.apci = apci;
            this.apciData = apciData;
            this.data = data;
            isOneToOne = false;
            sendTime = 0;
        }

        public NPDU(int length, int apci, int apciData, string data, bool isOneToOne,int sendTime)
        {
            this.length = length;
            this.apci = apci;
            this.apciData = apciData;
            this.data = data;
            this.isOneToOne = isOneToOne;
            this.sendTime = sendTime;
        }
        /// <summary>
        /// 设置NPDU
        /// </summary>
        /// <returns></returns>
        public string SetNPDU()
        {
            string temp = "";
            temp += GlobalClass.AddLeftZero(length.ToString("X"), 2);
            string _tpci = GlobalClass.IntToBit(apci, 4);
            if (!isOneToOne)
            {
                temp += GlobalClass.AddLeftZero(GlobalClass.BitToInt(_tpci.Substring(0, 2)).ToString("X"), 2);
            }
            else
            {
                temp += GlobalClass.AddLeftZero(GlobalClass.BitToInt("01"+GlobalClass.IntToBit(sendTime,4) + _tpci.Substring(0, 2)).ToString("X"), 2);
            }
            temp += GlobalClass.AddLeftZero(GlobalClass.BitToInt(_tpci.Substring(2, 2) + GlobalClass.IntToBit(apciData, 6)).ToString("X"), 2);
            temp += data;
            return temp;
        }
        /// <summary>
        /// 获取NPDU
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public string GetNPDU(byte[] data)
        {
            length = Convert.ToInt32(data[0]);
            apci = GlobalClass.BitToInt(GlobalClass.IntToBit(Convert.ToInt32(data[1]), 8).Substring(6, 2)
            + GlobalClass.IntToBit(Convert.ToInt32(data[2]), 8).Substring(0, 2));
            apciData = GlobalClass.BitToInt(GlobalClass.IntToBit(Convert.ToInt32(data[2]), 8).Substring(2, 6));
           this.data = GlobalClass.ByteToHex(data, 3, length - 1);
            return GlobalClass.ByteToHex(data);
        }
        /// <summary>
        /// 获取NPDU
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public string GetNPDU(string data)
        {
            return GetNPDU(GlobalClass.HexToByte(data));
        }

        /// <summary>
        /// 获取长度
        /// </summary>
        public int Length
        {
            get
            {
                return length;
            }
        }
        /// <summary>
        /// 获取apci
        /// </summary>
        public int Apci
        {
            get
            {
                return apci;
            }
            set
            {
                apci = value;
            }
        }
        /// <summary>
        /// 获取apciData
        /// </summary>
        public int ApaiData
        {
            get
            {
                return apciData;
            }
            set
            {
                apci = value;
            }
        }
        /// <summary>
        /// 获取Data
        /// </summary>
        public string Data
        {
            get
            {
                return data;
            }
            set
            {
                data = value;
            }
        }
    }
}
