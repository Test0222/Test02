using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;

namespace HDL_Buspro_Setup_Tool
{
    [Serializable]
    public class HVAC : HdlDeviceBackupAndRestore
    {
        public int DIndex;  ////工程内唯一编号
        public string DeviceName;
        public byte[] aryAirTime = new byte[20];
        public byte[] arayWindEle = new byte[20];
        public byte TemperatureType;
        public byte[] bytAryWindAndMode=new byte[11];//风速模式选择
        public byte bytSavePower; //省电模式
        public byte bytWind;      //要不要扫风
        public byte[] bytTempArea=new byte[11];  // 不同模式下的温度范围
        public byte[] AryACSetup = new byte[50];
        public byte[] AryHost = new byte[50];

        public byte[] bytSteps; //总共三个序列,每个序列四步 新的类型  140： 主从机 + 温度传感器 （开始为简单模式，复杂模式）

        public bool[] MyRead2UpFlags = new bool[10] { false, false, false, false, false, false, false, false, false, false };
        internal MYHVAC myHVAC = new MYHVAC();
        [Serializable]
        internal class MYHVAC
        {
            public byte Interval;
            public byte[] arayTest;
            public byte[] arayProtect;
            public byte[] arayMode;
            public byte[] arayRelay;
            public byte[] araComplex;
        }

        ////<summary>
        ////读取默认的HVAC设置，将所有数据读取缓存
        ////</summary>
        public void ReadDefaultInfo()
        {
            myHVAC = new MYHVAC();
        }

        //<summary>
        //读取数据库里的HVAC的设置，将所有数据读取缓存
        //</summary>
        public void ReadInfoForDB(int id)
        {
           
        }

        //<summary>
        //将缓存里所有HVAC的设置存在数据库
        //</summary>
        public void SaveInfoToDb()
        {
           
        }

        /// <summary>
        /// 将调光模块设备上传
        /// </summary>
        /// <param name="DIndex"></param>
        /// <param name="DevID"></param>
        /// <param name="DeviceType"></param>
        /// <param name="DevName"></param>
        /// <returns></returns>
        public bool UploadHVACInfosToDevice(string DevName, int wdDeviceType, int intPage)
        {
            string strMainRemark = DevName.Split('\\')[1].Trim().Split('(')[0].Trim();
            DevName = DevName.Split('\\')[0].Trim();

            byte bytSubID = byte.Parse(DevName.Split('-')[0].ToString());
            byte bytDevID = byte.Parse(DevName.Split('-')[1].ToString());

            byte[] ArayMain = new byte[20];

            if (HDLSysPF.ModifyDeviceMainRemark(bytSubID, bytDevID, strMainRemark,wdDeviceType) == true)
            {
                HDLUDP.TimeBetwnNext(20);
            }
            else return false;
            
            byte[] arayTmp = null;
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(1);
            if (intPage == 0 || intPage == 1)
            {
                #region
                arayTmp = new byte[5];
                Array.Copy(aryAirTime, 0, arayTmp, 0, 5);
                if (CsConst.mySends.AddBufToSndList(arayTmp, 0xE3F6, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType)) == true)
                {
                    HDLUDP.TimeBetwnNext(20);
                }
                else return false;
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(2);

                Array.Copy(arayWindEle, 0, arayTmp, 0, 4);
                if (CsConst.mySends.AddBufToSndList(arayTmp, 0xE3FA, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType)) == true)
                {
                    HDLUDP.TimeBetwnNext(20);
                }
                else return false;
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(8);
                #endregion
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(10);
            if (intPage == 0 || intPage == 2)
            {
                #region
                if (CsConst.isRestore)
                {
                    if (HVACModuleDeviceTypeList.HVACG2ComplexRelayDeviceTypeLists.Contains(wdDeviceType))
                    {
                        arayTmp = new byte[1];
                        arayTmp[0] = TemperatureType;
                        if (CsConst.mySends.AddBufToSndList(arayTmp, 0xE122, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType)) == true)
                        {
                            HDLUDP.TimeBetwnNext(20);
                        }
                        else return false;
                        if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(11);
                        arayTmp = new byte[11];
                        Array.Copy(bytAryWindAndMode, 0, arayTmp, 0, arayTmp.Length);
                        if (CsConst.mySends.AddBufToSndList(arayTmp, 0xE126, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType)) == true)
                        {
                            HDLUDP.TimeBetwnNext(20);
                        }
                        else return false;
                        if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(12);
                        arayTmp = new byte[11];
                        Array.Copy(bytTempArea, 0, arayTmp, 0, 11);
                        if (CsConst.mySends.AddBufToSndList(arayTmp, 0x1902, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType)) == true)
                        {
                            HDLUDP.TimeBetwnNext(20);
                        }
                        else return false;
                        if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(13);
                        arayTmp = new byte[2];
                        arayTmp[0] = bytSavePower;
                        arayTmp[1] = bytWind;
                        if (CsConst.mySends.AddBufToSndList(arayTmp, 0x1911, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType)) == true)
                        {
                            HDLUDP.TimeBetwnNext(20);
                        }
                        else return false;
                        if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(14);
                    }
                }
                if (HVACModuleDeviceTypeList.HVACG2ComplexRelayHostDeviceTypeLists.Contains(wdDeviceType))
                {
                    arayTmp = new byte[13];
                    Array.Copy(AryACSetup, 0, arayTmp, 0, 13);
                    if (CsConst.mySends.AddBufToSndList(arayTmp, 0x1CA4, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType)) == true)
                    {
                        HDLUDP.TimeBetwnNext(20);
                    }
                    else return false;
                }
                #endregion
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(20);
            if (intPage == 0 || intPage == 3)
            {
                #region
                if (HVACModuleDeviceTypeList.HVACG2ComplexRelayHostDeviceTypeLists.Contains(wdDeviceType))
                {
                    arayTmp = new byte[27];
                    Array.Copy(AryHost, 0, arayTmp, 0, 27);
                    if (CsConst.mySends.AddBufToSndList(arayTmp, 0x3507, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType)) == true)
                    {
                        HDLUDP.TimeBetwnNext(20);
                    }
                    else return false;
                }
                else
                {
                    arayTmp = new byte[19];
                    Array.Copy(AryHost, 0, arayTmp, 0, 19);
                    if (CsConst.mySends.AddBufToSndList(arayTmp, 0x1C44, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType)) == true)
                    {
                        HDLUDP.TimeBetwnNext(20);
                    }
                    else return false;
                }
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(21);
                #endregion
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(30);
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(100, null);
            return true;
        }

        /// <summary>
        /// subnet id + device id + remark
        /// </summary>
        /// <param name="DeName"></param>
        public bool DownLoadInformationFrmDevice(string DevName, int wdDeviceType, int intPage)
        {
            string strMainRemark = DevName.Split('\\')[1].Trim();
            DevName = DevName.Split('\\')[0].Trim();

            byte bytSubID = byte.Parse(DevName.Split('-')[0].ToString());
            byte bytDevID = byte.Parse(DevName.Split('-')[1].ToString());
            byte[] ArayTmp = null;
            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x000E, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType)) == true)
            {
                byte[] arayRemark = new byte[20];
                for (int intI = 0; intI < 20; intI++) { arayRemark[intI] = CsConst.myRevBuf[25 + intI]; }
                DeviceName = bytSubID.ToString() + "-" + bytDevID.ToString() + "\\" + HDLPF.Byte2String(arayRemark);
                HDLUDP.TimeBetwnNext(1);
            }
            else return false;
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(1, null);

            if (intPage == 0 || intPage == 1)
            {
                #region
                aryAirTime = new byte[20];
                arayWindEle = new byte[20];
                myHVAC.arayTest = new byte[3];
                //延迟时间
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xE3F4, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType)) == true)
                {
                    Array.Copy(CsConst.myRevBuf, 25, aryAirTime, 0, 6);
                    HDLUDP.TimeBetwnNext(1);
                }
                else return false;
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(2, null);
                //风速三级输出
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xE3F8, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType)) == true)
                {
                    Array.Copy(CsConst.myRevBuf, 25, arayWindEle, 0, 4);
                }
                #endregion
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(3, null);
                if (wdDeviceType == 106 || HVACModuleDeviceTypeList.HVACG2ComplexRelayDeviceTypeLists.Contains(wdDeviceType))
                {
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1C96, bytSubID, bytDevID, false, true, true, false) == true)
                    {
                        Array.Copy(CsConst.myRevBuf, 25, myHVAC.arayTest, 0, 3);
                        HDLUDP.TimeBetwnNext(1);
                    }
                    else return false;
                }
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(4, null);
                MyRead2UpFlags[0] = true;
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(10, null);
            if (intPage == 0 || intPage == 2)
            {
                #region
                if (HVACModuleDeviceTypeList.HVACG2ComplexRelayDeviceTypeLists.Contains(wdDeviceType))
                {
                    if (CsConst.mySends.AddBufToSndList(null, 0xE120, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType)) == true)
                    {
                        TemperatureType = CsConst.myRevBuf[25];
                        HDLUDP.TimeBetwnNext(1);
                    }
                    else return false;
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(11, null);
                    if (CsConst.mySends.AddBufToSndList(null, 0xE124, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType)) == true)
                    {
                        bytAryWindAndMode = new byte[11];
                        Array.Copy(CsConst.myRevBuf, 25, bytAryWindAndMode, 0, 11);
                        HDLUDP.TimeBetwnNext(1);
                    }
                    else return false;
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(12, null);
                    if (CsConst.mySends.AddBufToSndList(null, 0x1900, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType)) == true)
                    {
                        bytTempArea = new byte[11];
                        Array.Copy(CsConst.myRevBuf, 25, bytTempArea, 0, 11);
                        HDLUDP.TimeBetwnNext(1);
                    }
                    else return false;
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(13, null);
                    if (CsConst.mySends.AddBufToSndList(null, 0x190F, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType)) == true)
                    {
                        bytSavePower = CsConst.myRevBuf[25];
                        bytWind = CsConst.myRevBuf[26];
                        HDLUDP.TimeBetwnNext(1);
                    }
                    else return false;
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(14, null);
                }
                AryACSetup = new byte[50];
                if (wdDeviceType == 733 || wdDeviceType == 737)
                {
                    if (CsConst.mySends.AddBufToSndList(null, 0x1CA6, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType)) == true)
                    {
                        Array.Copy(CsConst.myRevBuf, 25, AryACSetup, 0, 17);
                        HDLUDP.TimeBetwnNext(1);
                    }
                    else return false;
                }
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(15, null);
                #endregion
                MyRead2UpFlags[1] = true;
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(20, null);
            if (intPage == 0 || intPage == 3)
            {
                #region
                AryHost = new byte[50];
                if (HVACModuleDeviceTypeList.HVACG2ComplexRelayHostDeviceTypeLists.Contains(wdDeviceType))
                {
                    if (CsConst.mySends.AddBufToSndList(null, 0x1C46, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType)) == true)
                    {
                        Array.Copy(CsConst.myRevBuf, 25, AryHost, 0, CsConst.myRevBuf[16] - 11);
                        HDLUDP.TimeBetwnNext(1);
                    }
                    else return false;
                }
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(21, null);
                #endregion
                MyRead2UpFlags[2] = true;
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(30, null);
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(100, null);
            return true;
        }
    }
}
