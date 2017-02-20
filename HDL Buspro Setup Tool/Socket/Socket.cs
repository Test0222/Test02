using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HDL_Buspro_Setup_Tool
{
    [Serializable]
    public class RFSocket :HdlDeviceBackupAndRestore
    {
        public string DeviceName;
        public int DIndex;
        public BasicInfo MyBasicInfo = new BasicInfo();
        public bool[] MyRead2UpFlags = new bool[14]; //

        [Serializable]
        public class BasicInfo
        {
            public byte[] arayElectri = new byte[13];
            public byte[] arayAlarm = new byte[8];
            public byte[] araAdjustV = new byte[2];
            public byte[] araAdjustE = new byte[2];
            public byte[] araAdjustP = new byte[2];
        }

         //<summary>
        //读取默认的面板设置，将所有数据读取缓存
        //</summary>
        public void ReadDefaultInfo()
        {
            MyBasicInfo = new BasicInfo();
        }

        //<summary>
        //读取数据库面板设置，将所有数据读至缓存
        //</summary>
        public void ReadPanelFrmDBTobuf(int intDIndex)
        {
            try
            {
            }
            catch
            {
            }
        }

         ///<summary>
       /// 保存数据库面板设置，将所有数据保存
       /// </summary>
        public void SavePanelToDB()
        {
            try
            {
            }
            catch
            {
            }
        }

        public bool UploadInfosToDevice(string DevName, int DeviceType, int intActivePage)// 0 mean all, else that tab only
        {
            //保存回路信息
            string strMainRemark = DevName.Split('\\')[1].Trim();
            String TmpDevName = DevName.Split('\\')[0].Trim();
            byte bytSubID = byte.Parse(TmpDevName.Split('-')[0].ToString());
            byte bytDevID = byte.Parse(TmpDevName.Split('-')[1].ToString());

            int wdMaxValue = DeviceTypeList.GetMaxValueFromPublicModeGroup(DeviceType);

            byte[] ArayMain = new byte[20];
            byte[] arayTmpRemark = HDLUDP.StringToByte(strMainRemark);
            HDLSysPF.CopyRemarkBufferToSendBuffer(arayTmpRemark, ArayMain, 0);

            if (CsConst.mySends.AddBufToSndList(ArayMain, 0x0010, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == false)
            {
                return false;
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(5);

            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(100);
            return true;
        }

        public bool DownLoadInformationFrmDevice(string DevName, int DeviceType, int intActivePage)// 0 mean all, else that tab only
        {
            string strMainRemark = DevName.Split('\\')[1].Trim();
            String TmpDevName = DevName.Split('\\')[0].Trim();

            byte bytSubID = byte.Parse(TmpDevName.Split('-')[0].ToString());
            byte bytDevID = byte.Parse(TmpDevName.Split('-')[1].ToString());
            byte[] ArayTmp = null;
            int wdMaxValue = DeviceTypeList.GetMaxValueFromPublicModeGroup(DeviceType);

            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x000E, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == false)
            {
                return false;
            }
            else
            {
                byte[] arayRemark = new byte[20];
                HDLSysPF.CopyRemarkBufferFrmMyRevBuffer(CsConst.myRevBuf, arayRemark, 25);
                DeviceName = bytSubID.ToString() + "-" + bytDevID.ToString() + "\\" + HDLPF.Byte2String(arayRemark);
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(5);



            if (intActivePage == 0 || intActivePage == 1)
            {
                MyBasicInfo.arayElectri = new byte[13];
                MyBasicInfo.arayAlarm = new byte[8];
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1D80, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                {
                    Array.Copy(CsConst.myRevBuf, 25, MyBasicInfo.arayElectri, 0, 13);
                    HDLUDP.TimeBetwnNext(10);
                    CsConst.myRevBuf = new byte[1200];
                }
                else return false;
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(10);

                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1D90, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                {
                    Array.Copy(CsConst.myRevBuf, 26, MyBasicInfo.arayAlarm, 0, 8);
                    HDLUDP.TimeBetwnNext(10);
                    CsConst.myRevBuf = new byte[1200];
                }
                else return false;
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(15);
                MyRead2UpFlags[0] = true;
            }

            if (intActivePage == 0 || intActivePage == 2)
            {
                MyBasicInfo.araAdjustV = new byte[2];
                MyBasicInfo.araAdjustE = new byte[2];
                MyBasicInfo.araAdjustP = new byte[2];
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1D84, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                {
                    Array.Copy(CsConst.myRevBuf, 26, MyBasicInfo.araAdjustV, 0, 2);
                    HDLUDP.TimeBetwnNext(1);
                    CsConst.myRevBuf = new byte[1200];
                }
                else return false;
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(20);

                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1D88, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                {
                    Array.Copy(CsConst.myRevBuf, 26, MyBasicInfo.araAdjustE, 0, 2);
                    HDLUDP.TimeBetwnNext(1);
                    CsConst.myRevBuf = new byte[1200];
                }
                else return false;
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(25);

                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1D8C, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                {
                    Array.Copy(CsConst.myRevBuf, 26, MyBasicInfo.araAdjustP, 0, 2);
                    HDLUDP.TimeBetwnNext(1);
                    CsConst.myRevBuf = new byte[1200];
                }
                else return false;
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(30);
                MyRead2UpFlags[1] = true;
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(100);
            return true;
        }
    }
}
