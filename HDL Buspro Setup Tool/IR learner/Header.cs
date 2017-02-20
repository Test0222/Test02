using System;
using System.Collections.Generic;
using System.Text;

namespace HDL_Buspro_Setup_Tool
{
    class Header
    {
        private int reportID;
        private int packetInfo;
        private int dataLength;

        public Header()
        {
            reportID = 1;
            packetInfo = 0;
            dataLength = 0;
        }
        /// <summary>
        /// Header
        /// </summary>
        /// <param name="packetInfo"></param>
        /// <param name="dataLength"></param>
        public Header(int packetInfo,int dataLength)
        {
            this.packetInfo = packetInfo;
            this.dataLength = dataLength;
            reportID = 1;
        }
        /// <summary>
        /// Header
        /// </summary>
        /// <param name="packetInfo"></param>
        /// <param name="dataLength"></param>
        /// <param name="reportID"></param>
        public Header(int packetInfo, int dataLength, int reportID)

        {
            this.packetInfo = packetInfo;
            this.dataLength = dataLength;
            this.reportID = reportID;
        }

        /// <summary>
        /// ����ͷ����Ϣ��16����
        /// </summary>
        /// <returns></returns>
        public string SetHeader()
        {
            string temp = "";
            temp += GlobalClass.AddLeftZero(reportID.ToString("X"), 2);
            temp += GlobalClass.AddLeftZero(packetInfo.ToString("X"), 2);
            temp += GlobalClass.AddLeftZero(dataLength.ToString("X"), 2);
            return temp;
        }
        /// <summary>
        /// ��ȡͷ����Ϣ��16����
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public string GetHeader(string data)
        {
            return GetHeader(GlobalClass.HexToByte(data));
        }
        /// <summary>
        /// ��ȡͷ����Ϣ
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public string GetHeader(byte[] data)
        {
            reportID = Convert.ToInt32(data[0]);
            packetInfo = Convert.ToInt32(data[1]);
            dataLength = Convert.ToInt32(data[2]);
            return GlobalClass.ByteToHex(data);
        }

        /// <summary>
        /// ��ȡReportID
        /// </summary>
        public int ReportID
        {
            get
            {
                return reportID;
            }
        }
        /// <summary>
        /// ��ȡPacketInfo
        /// </summary>
        public int PacketInfo
        {
            get
            {
                return packetInfo;
            }
        }
        /// <summary>
        /// ��ȡ���ݳ���
        /// </summary>
        public int DataLength
        {
            get
            {
                return dataLength;
            }
        }
    }
}
