using System;
using System.Collections.Generic;
using System.Text;

namespace HDL_Buspro_Setup_Tool
{
    public class UVCMD
    {
         [Serializable]
        public class ControlTargets
        {
            public byte ID;
            public int intStatus;
            public byte SubnetID;
            public byte DeviceID;
            public byte Type;
            public byte Param1;
            public byte Param2;
            public byte Param3;
            public byte Param4;
            public bool IsValid = true;
            public String Hint;
            public object Clone()
            {
                ControlTargets TmpCtrls = (ControlTargets)this.MemberwiseClone(); // 浅复制
                return (object)TmpCtrls;
            }
        }
        [Serializable]
        public class IRCode
        {
            public int KeyID;
            public int IRLength;
            public int IRLoation; // 红外库表示行号
            public string Remark1;
            public string Remark2;
            public string Codes;
            public byte Enable;//有效性,1有效
        }

        /// <summary>
        ///  通用的安防模式设置
        /// </summary>
         [Serializable]
        public class SecurityInfo
        {
            public byte bytSeuID;
            public string strRemark;
            public byte bytTerms; 
            public byte bytSubID;
            public byte bytDevID;
            public byte bytArea;
            public byte bytSeuMode;
            public int bytKinds;
            public byte bytDelay;

        }

         [Serializable]
        ///
        /// 河东红外码库
        ///设备下各个品牌红外码信息
        public class DeviceAllIRInfo
        {
            public int ID;//设备ID{1：投影机，2：风扇，3：机顶盒，4：影碟机(DVD,EVD,VCD)，5电视机，6：空调}
            public int brandCount;///品牌个数
            public List<brandInfo> brand;
            public string DevRemark;//设备备注
        }
        [Serializable]
        public class brandInfo
        {
            public int brandID;///品牌ID
            public string brandRemark;////品牌备注
            public int IRcount;///红外个数
            public int[] IRIndexIntAry;///各个红外索引信息
            // public List<UVCMD.IRCode> IRInfo;///各个红外信息
        }
         [Serializable]
        public class SimpleSearchList
        {
           public  byte[] Address = new byte[2];
           public byte ChnAll;// how many channels or lights
           public byte FunctionType;  // simple way Type (big)
           public byte DetailType;  // simple way Type (small)
           public byte TotalCmds;  // how many commands in total
        }
    }
}
