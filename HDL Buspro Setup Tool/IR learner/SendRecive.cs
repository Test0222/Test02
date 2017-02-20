using System;
using System.Collections.Generic;
using System.Text;

namespace HDL_Buspro_Setup_Tool
{
    public class SendRecive
    {
        private Header myHeader;
        private ReportHeader myReport;
        private CEMI myCEMI;
        private NPDU myNPDU;
        public SendRecive()
        {
            myHeader = null;
            myReport = null;
            myCEMI = null;
            myNPDU = null;
        }
        /// <summary>
        /// 读内存信息
        /// </summary>
        /// <param name="desAddress"></param>
        /// <param name="sendTime"></param>
        /// <param name="readAddress"></param>
        /// <param name="readLength"></param>
        /// <returns></returns>
        public string SendReadMemory(int desAddress, int sendTime,int readAddress, int readLength)
        {
            myNPDU = new NPDU(3, 8, readLength,GlobalClass.AddLeftZero(readAddress.ToString("X"),4), true, sendTime);
            myCEMI = new CEMI(0x11, 0xbc, 0x60, 0, desAddress);
            myReport = new ReportHeader(10 + myNPDU.Length);
            myHeader = new Header(0x13, myReport.HeadLength + myReport.BodyLength);
            return myHeader.SetHeader() + myReport.SetReportHeader() +
                myCEMI.SetCEMI() + myNPDU.SetNPDU();
        }

        /// <summary>
        /// 写内存信息
        /// </summary>
        /// <param name="desAddress"></param>
        /// <param name="sendTime"></param>
        /// <param name="readAddress"></param>
        /// <param name="readLength"></param>
        /// <param name="writeData"></param>
        /// <returns></returns>
        public string SendWriteMemory(int desAddress, int sendTime, int readAddress, int readLength, string writeData)
        {
             myNPDU = new NPDU((writeData.Length / 2) + 3, 10, readLength,GlobalClass.AddLeftZero(readAddress.ToString("X"),4)+ writeData,true,sendTime);
             myCEMI = new CEMI(0x11, 0xbc, 0x60, 0, desAddress);
             myReport = new ReportHeader(10 + myNPDU.Length);
             myHeader = new Header(0x13, myReport.HeadLength + myReport.BodyLength);
             return myHeader.SetHeader() + myReport.SetReportHeader() + myCEMI.SetCEMI() + myNPDU.SetNPDU();
        }
        /// <summary>
        /// 写对象值信息
        /// </summary>
        /// <param name="desAddress"></param>
        /// <param name="apci"></param>
        /// <param name="apciData"></param>
        /// <returns></returns>
        public string SendWriteValue(int desAddress, int apciData)
        {
            myNPDU = new NPDU(1, 2, apciData, "");
            myCEMI = new CEMI(0x11, 0xbc, 0xe0, 0, desAddress);
            myReport = new ReportHeader(10 + myNPDU.Length);
            myHeader = new Header(0x13, myReport.HeadLength + myReport.BodyLength);
            return myHeader.SetHeader() + myReport.SetReportHeader() + myCEMI.SetCEMI() + myNPDU.SetNPDU();
        }
        /// <summary>
        /// 读对象值信息
        /// </summary>
        /// <param name="desAddress"></param>
        /// <returns></returns>
        public string SendReadValue(int desAddress)
        {
            myNPDU = new NPDU(1, 0, 0, "");
            myCEMI = new CEMI(0x11, 0xbc, 0xe0, 0, desAddress);
            myReport = new ReportHeader(10 + myNPDU.Length);
            myHeader = new Header(0x13, myReport.HeadLength + myReport.BodyLength);
            return myHeader.SetHeader() + myReport.SetReportHeader() + myCEMI.SetCEMI() + myNPDU.SetNPDU();
        }
        /// <summary>
        /// 发送连接请求，或者断开连接请求
        /// </summary>
        /// <param name="isConn"></param>
        /// <returns></returns>
        public string SendConn(int desAddress, bool isConn)
        {
            myCEMI = new CEMI(0x11, 0xbc, 0x60, 0, desAddress);
            myReport = new ReportHeader(10);
            myHeader = new Header(0x15, 0x12);
            if (isConn)
            {
                return myHeader.SetHeader() + myReport.SetReportHeader() + myCEMI.SetCEMI() + "0080";
            }
            else
            {
                return myHeader.SetHeader() + myReport.SetReportHeader() + myCEMI.SetCEMI() + "0081";
            }

        }
        /// <summary>
        /// 发送返回ACK
        /// </summary>
        /// <param name="desAddress"></param>
        /// <param name="sendTime"></param>
        /// <returns></returns>
        public string SendBackACK(int desAddress)
        {
            myCEMI = new CEMI(0x11, 0xbc, 0x60, 0, desAddress);
            myReport = new ReportHeader(10);
            myHeader = new Header(0x13, 0x12);
            return myHeader.SetHeader() + myReport.SetReportHeader() + myCEMI.SetCEMI();
        }
    }
}
