using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices; 

namespace HDL_Buspro_Setup_Tool
{
    /// <summary>
    /// 在线设备列表
    /// </summary>
    [Serializable]
    [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public class DevOnLine
    {
        public Byte bytSub;
        public Byte bytDev;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 30)]
        public String DevName; // 设备的子网ID -设备ID\\备注
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 30)]
        public String MAC;
        public Int32 DeviceType;
        public Int32 intDIndex; // 设备的Index
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 30)]
        public String strVersion; //版本信息
    }

     [Serializable]
    public class ChnSenceInfomation
    {
        public Byte ID;
        public Byte type;
        public String Remark;
        public Byte light;
        public Boolean isCurtainChannel;
    }

     [Serializable]
    public class Channel
    {
        public Int32 ID;
        public String Remark;
        public Int32 LoadType;
        public Int32 MinValue;
        public Int32 MaxValue;
        public Int32 MaxLevel;
        public Int32 CurveID;

        public int intBelongs; //属于哪个区

        public int PowerOnDelay; // mix 混合模块的开延迟
        public Byte ProtectDealy; // mix 保护延迟
    }

     [Serializable]
    public class SimpleFunction
    {
        public Int32 id;
        public Byte bigFunctionType;
        public String Remark;
        public Byte[] detailsParameters;
    }

     [Serializable]
    public class ThirdPartAC
    {
        public Byte ID;
        public Byte Enable;
        public Byte ACNO;
        public Byte CoolMasterAddress;
        public Byte GroupID;
        public String Remark;
        public Byte[] arayACinfo;
    }

     [Serializable]
    public class DeviceInfo
    {
        public Byte SubnetID;
        public Byte DeviceID;
        public String StrName;
        public Byte Other;//在地热中可以代表通道ID

        public DeviceInfo(string strName)
        {
            this.StrName = strName;
            if (strName.Contains("\\"))
            {
                string strTmp = strName.Split('\\')[0].Trim();
                this.SubnetID = byte.Parse(strTmp.Split('-')[0].Trim());
                this.DeviceID = byte.Parse(strTmp.Split('-')[1].Trim());
            }
            else
            {
                this.SubnetID = byte.Parse(strName.Split('-')[0].Trim());
                this.DeviceID = byte.Parse(strName.Split('-')[1].Trim());
            }
        }
    }

     [Serializable]
    public class SensorInfo
    {
        public Byte ValidType;//制冷制热传感器表示是否有效；地板和室外传感器用于表示关闭 接收广播 主动读取
        public Byte SubnetID;
        public Byte DeviceID;
        public Byte Channel;
        public Byte Other = 0;//制冷制热 传感器设置表示是否取反；地板传感器设置表示高限值
    }
}
