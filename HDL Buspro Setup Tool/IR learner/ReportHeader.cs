using System;
using System.Collections.Generic;
using System.Text;

namespace HDL_Buspro_Setup_Tool
{
    public class ReportHeader
    {
        private int version;
        private int headLength;
        private int bodyLength;
        private int protocolID;
        private int emiID;
        private int manuCode;

        public ReportHeader()
        {
            version = 0;
            headLength = 0;
            bodyLength = 0;
            protocolID = 0;
            emiID = 0;
            manuCode = 0;
        }
        /// <summary>
        /// ReportHeader
        /// </summary>
        /// <param name="bodyLength"></param>
        public ReportHeader(int bodyLength)
        {
            this.bodyLength = bodyLength;
            version = 0;
            headLength = 8;
            protocolID = 1;
            emiID = 3;
            manuCode = 0;
        }
        /// <summary>
        /// ReportHeader
        /// </summary>
        /// <param name="bodyLength"></param>
        /// <param name="version"></param>
        /// <param name="headLength"></param>
        /// <param name="protocolID"></param>
        /// <param name="emiID"></param>
        /// <param name="manuCode"></param>
        public ReportHeader(int bodyLength,int version,int headLength,int protocolID,int emiID,int manuCode)
        {
            this.bodyLength = bodyLength;
            this.version = version;
            this.headLength = headLength;
            this.protocolID = protocolID;
            this.emiID = emiID;
            this.manuCode = manuCode;
        }
        /// <summary>
        /// 设置ReportHeader
        /// </summary>
        /// <returns></returns>
        public string SetReportHeader()
        {
            string temp = "";
            temp += GlobalClass.AddLeftZero(version.ToString("X"), 2);
            temp += GlobalClass.AddLeftZero(headLength.ToString("X"), 2);
            temp += GlobalClass.AddLeftZero(bodyLength.ToString("X"), 4);
            temp += GlobalClass.AddLeftZero(protocolID.ToString("X"), 2);
            temp += GlobalClass.AddLeftZero(emiID.ToString("X"), 2);
            temp += GlobalClass.AddLeftZero(manuCode.ToString("X"), 4);
            return temp;
        }
        /// <summary>
        /// 获取ReportHeader
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public string GetReportHeader(byte[] data)
        {
            version = Convert.ToInt32(data[0]);
            headLength = Convert.ToInt32(data[1]);
            bodyLength = GlobalClass.ByteToInt32(data, 2, 2);
            protocolID = Convert.ToInt32(data[4]);
            emiID = Convert.ToInt32(data[5]);
            manuCode = GlobalClass.ByteToInt32(data, 6, 2);
            return GlobalClass.ByteToHex(data);
        }
        /// <summary>
        /// 获取ReportHeader
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public string GetReportHeader(string data)
        {
            return GetReportHeader(GlobalClass.HexToByte(data));
        }

        /// <summary>
        /// 获取Verison
        /// </summary>
        public int VerSion
        {
            get
            {
                return version;
            }
        }
        /// <summary>
        /// 获取头部长度
        /// </summary>
        public int HeadLength
        {
            get
            {
                return headLength;
            }
        }
        /// <summary>
        /// 获取cemi长度
        /// </summary>
        public int BodyLength
        {
            get
            {
                return bodyLength;
            }
        }
        /// <summary>
        /// 获取ProtocolID
        /// </summary>
        public int ProtocolID
        {
            get
            {
                return protocolID;
            }
        }
        /// <summary>
        /// 获取EMIid
        /// </summary>
        public int EmiID
        {
            get
            {
                return emiID;
            }

        }
        /// <summary>
        /// id获取ManuCode
        /// </summary>
        public int ManuCode
        {
            get
            {
                return manuCode;
            }
        }
    }
}
