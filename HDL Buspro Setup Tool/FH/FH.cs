using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Xml;
using System.IO;

namespace HDL_Buspro_Setup_Tool
{
    [Serializable]
    public class FH : HdlDeviceBackupAndRestore
    {
        public int DIndex;//设备唯一编号
        //基本信息
        public string DeviceName;//子网ID 设备ID 备注
        public bool[] MyRead2UpFlags = new bool[14];
        public byte[] arayDateTime = new byte[10];
        public byte[] araySummer = new byte[30];
        public byte[] arayOutdoor = new byte[10];
        public byte[] araySynChannel = new byte[10];
        public byte[] arayHost = new byte[20];
        public byte FHTargetEnable;
        public byte RelayTargetEnable;
        public byte PumpsEnable;
        internal List<FHeating> myHeating;
        internal List<FHTargets> myTargets1;
        internal List<FHTargets> myTargets2;
        internal List<Pumps> myPumps;
        //配置信息
        internal class FHeating
        {
            public byte ID;
            public string strRemark;
            public byte[] arayWorkControl = new byte[20];
            public byte ReceiveSyn;
            public byte[] arayWorkSetting = new byte[20];
            public byte[] araySensorSetting = new byte[30];
            public byte[] arayFlush = new byte[20];
        }

        internal class FHTargets
        {
            public byte ID;
            public byte StatusIndex;
            public List<UVCMD.ControlTargets> Targets;
        }

        internal class Pumps
        {
            public byte ID;
            public byte Enable;
            public byte ChooseChns;
            public int TrueDelay;
            public int FalseDelay;
            public List<UVCMD.ControlTargets> TrueTargets;
            public List<UVCMD.ControlTargets> FalseTargets;
        }

        ///<summary>
        ///读取默认的设置
        ///</summary>
        public void ReadDefaultInfo()
        {
            myHeating = new List<FHeating>();
            myTargets1 = new List<FHTargets>();
            myTargets2 = new List<FHTargets>();
            myPumps = new List<Pumps>();
        }

        ///<summary>
        ///读取数据库面板设置，将所有数据读至缓存
        ///</summary>
        public void ReadDevieceInfoFromDB(int DIndex)
        {


        }

        ///<summary>
        ///保存数据库面板设置，将所有数据保存
        ///</summary>
        public void SaveDevieceInfoToDB(int DIndex)
        {

        }

        /// <summary>
        ///上传设置
        /// </summary>
        public bool UploadFHInfosToDevice(string DevName, int wdDeviceType, int intActivePage)
        {
            string strMainRemark = DevName.Split('\\')[1].Trim().Split('(')[0].Trim();
            DevName = DevName.Split('\\')[0].Trim();
            //保存basic informations
            byte bytSubID = byte.Parse(DevName.Split('-')[0].ToString());
            byte bytDevID = byte.Parse(DevName.Split('-')[1].ToString());
            byte[] ArayMain = new byte[20];
            byte[] arayTmpRemark = HDLUDP.StringToByte(strMainRemark);
            if (arayTmpRemark.Length > 20)
            {
                Array.Copy(arayTmpRemark, 0, ArayMain, 0, 20);
            }
            else
            {
                Array.Copy(arayTmpRemark, 0, ArayMain, 0, arayTmpRemark.Length);
            }

            if (CsConst.mySends.AddBufToSndList(ArayMain, 0x0010, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType)) == true)
            {
                HDLUDP.TimeBetwnNext(20);
            }
            else return false;
            byte[] ArayTmp = null;
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(1);
            if (intActivePage == 0 || intActivePage == 1)
            {
                #region
                ArayTmp = new byte[7];
                Array.Copy(arayDateTime, 0, ArayTmp, 0, 7);
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xD99C, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType)) == true)
                {
                    HDLUDP.TimeBetwnNext(20);
                }
                else return false;
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(2);

                ArayTmp = new byte[2];
                Array.Copy(arayDateTime, 7, ArayTmp, 0, 2);
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1C10, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType)) == true)
                {
                    HDLUDP.TimeBetwnNext(20);
                }
                else return false;
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(3);

                ArayTmp = new byte[17];
                Array.Copy(araySummer, 0, ArayTmp, 0, 16);
                ArayTmp[1] = Convert.ToByte(DateTime.Now.Year / 256);
                ArayTmp[2] = Convert.ToByte(DateTime.Now.Year % 256);
                ArayTmp[4] = HDLSysPF.GetDayofMonth(ArayTmp[3], ArayTmp[7], Convert.ToByte(ArayTmp[6] + 1));
                ArayTmp[9] = Convert.ToByte(DateTime.Now.Year / 256);
                ArayTmp[10] = Convert.ToByte(DateTime.Now.Year % 256);
                ArayTmp[12] = HDLSysPF.GetDayofMonth(ArayTmp[11], ArayTmp[15], Convert.ToByte(ArayTmp[14] + 1));
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1D18, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType)) == true)
                {
                    HDLUDP.TimeBetwnNext(20);
                }
                else return false;
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(4);

                if (CsConst.isRestore)
                {
                    if (wdDeviceType == 211 || wdDeviceType == 208 || wdDeviceType == 209)
                    {
                        ArayTmp = new byte[4];
                        Array.Copy(arayOutdoor, 0, ArayTmp, 0, 4);
                        if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1D04, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType)) == true)
                        {
                            HDLUDP.TimeBetwnNext(20);
                        }
                        else return false;
                    }
                }
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(5);
                #endregion
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(10);
            if (intActivePage == 0 || intActivePage == 2)
            {
                #region
                if (myHeating != null)
                {
                    for (int i = 0; i < myHeating.Count; i++)
                    {
                        FHeating temp = myHeating[i];
                        ArayTmp[0] = temp.ID;
                        arayTmpRemark = HDLUDP.StringTo2Byte(temp.strRemark, true);
                        if (arayTmpRemark.Length > 20)
                        {
                            Array.Copy(arayTmpRemark, 0, ArayTmp, 1, 20);
                        }
                        else
                        {
                            Array.Copy(arayTmpRemark, 0, ArayTmp, 1, arayTmpRemark.Length);
                        }
                        if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1D00, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType)) == true)
                        {
                            HDLUDP.TimeBetwnNext(20);
                        }
                        else return false;
                        if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(10 + i * 5 + 1);

                        ArayTmp = new byte[10];
                        ArayTmp[0] = temp.ID;
                        Array.Copy(temp.arayWorkControl, 0, ArayTmp, 1, 7);
                        if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1C5C, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType)) == true)
                        {
                            Array.Copy(CsConst.myRevBuf, 26, temp.arayWorkControl, 0, 13);
                            HDLUDP.TimeBetwnNext(1);
                        }
                        else return false;
                        if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(10 + i * 5 + 2);

                        ArayTmp = new byte[18];
                        ArayTmp[0] = temp.ID;
                        Array.Copy(temp.araySensorSetting, 0, ArayTmp, 1, 17);
                        if (wdDeviceType == 210)
                        {
                            ArayTmp = new byte[14];
                            Array.Copy(temp.araySensorSetting, 0, ArayTmp, 1, 13);
                        }
                        if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1C54, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType)) == true)
                        {
                            HDLUDP.TimeBetwnNext(20);
                        }
                        else return false;

                        if (wdDeviceType == 211 || wdDeviceType == 208 || wdDeviceType == 209)
                        {
                            ArayTmp = new byte[2];
                            ArayTmp[0] = temp.ID;
                            Array.Copy(temp.arayWorkSetting, 10, ArayTmp, 0, 1);
                            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1D08, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType)) == true)
                            {
                                HDLUDP.TimeBetwnNext(20);
                            }
                            else return false;
                        }
                        if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(10 + i * 5 + 3);
                        ArayTmp = new byte[11];
                        ArayTmp[0] = temp.ID;
                        Array.Copy(temp.arayWorkSetting, 0, ArayTmp, 1, 10);
                        if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1C58, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType)) == true)
                        {
                            HDLUDP.TimeBetwnNext(20);
                        }
                        else return false;
                        ArayTmp = new byte[12];
                        ArayTmp[0] = temp.ID;
                        Array.Copy(temp.arayFlush, 0, ArayTmp, 1, 11);
                        if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1C60, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType)) == true)
                        {
                            HDLUDP.TimeBetwnNext(20);
                        }
                        else return false;
                    }
                }
                #endregion
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(40);
            if (intActivePage == 0 || intActivePage == 3)
            {
                #region
                ArayTmp = new byte[6];
                Array.Copy(araySynChannel, 0, ArayTmp, 0, 6);
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1C6c, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType)) == true)
                {
                    HDLUDP.TimeBetwnNext(20);
                }
                else return false;
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(41);

                ArayTmp = new byte[15];
                Array.Copy(arayHost, 0, ArayTmp, 0, 15);
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1CE4, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType)) == true)
                {
                    HDLUDP.TimeBetwnNext(20);
                }
                else return false;
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(42);
                #endregion
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(50);
            if (intActivePage == 0 || intActivePage == 4 || intActivePage == 6)
            {
                #region
                if (CsConst.isRestore)
                {
                    if (myTargets1 != null)
                    {
                        for (int i = 0; i < myTargets1.Count; i++)
                        {
                            FHTargets temp = myTargets1[i];
                            ArayTmp = new byte[10];
                            ArayTmp[0] = temp.ID;
                            ArayTmp[1] = temp.StatusIndex;
                            for (int j = 0; j < temp.Targets.Count; j++)
                            {
                                ArayTmp[2] = temp.Targets[j].ID;
                                ArayTmp[3] = temp.Targets[j].Type;
                                ArayTmp[4] = temp.Targets[j].SubnetID;
                                ArayTmp[5] = temp.Targets[j].DeviceID;
                                ArayTmp[6] = temp.Targets[j].Param1;
                                ArayTmp[7] = temp.Targets[j].Param2;
                                ArayTmp[8] = temp.Targets[j].Param3;
                                ArayTmp[9] = temp.Targets[j].Param4;
                                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1CE0, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType)) == true)
                                {
                                    HDLUDP.TimeBetwnNext(20);
                                }
                                else return false;
                            }
                            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(50 + i);
                        }
                    }
                }
                #endregion
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(60);
            if (intActivePage == 0 || intActivePage == 5 || intActivePage == 6)
            {
                #region
                if (CsConst.isRestore)
                {
                    if (myTargets2 != null)
                    {
                        for (int i = 0; i < myTargets2.Count; i++)
                        {
                            FHTargets temp = myTargets2[i];
                            ArayTmp = new byte[10];
                            ArayTmp[0] = temp.ID;
                            ArayTmp[1] = temp.StatusIndex;
                            for (int j = 0; j < temp.Targets.Count; j++)
                            {
                                ArayTmp[2] = temp.Targets[j].ID;
                                ArayTmp[3] = temp.Targets[j].Type;
                                ArayTmp[4] = temp.Targets[j].SubnetID;
                                ArayTmp[5] = temp.Targets[j].DeviceID;
                                ArayTmp[6] = temp.Targets[j].Param1;
                                ArayTmp[7] = temp.Targets[j].Param2;
                                ArayTmp[8] = temp.Targets[j].Param3;
                                ArayTmp[9] = temp.Targets[j].Param4;
                                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1CE0, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType)) == true)
                                {
                                    HDLUDP.TimeBetwnNext(20);
                                }
                                else return false;
                            }
                            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(60 + i);
                        }
                    }
                }
                #endregion
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(70);
            if (intActivePage == 0 || intActivePage == 7)
            {
                #region
                if (myPumps != null)
                {
                    ArayTmp = new byte[1];
                    ArayTmp[0] = PumpsEnable;
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1D20, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType)) == true)
                    {
                        HDLUDP.TimeBetwnNext(20);
                    }
                    else return false;

                    for (int i = 0; i < myPumps.Count; i++)
                    {
                        Pumps temp = myPumps[i];
                        ArayTmp = new byte[7];
                        ArayTmp[0] = temp.ID;
                        ArayTmp[1] = temp.Enable;
                        ArayTmp[2] = temp.ChooseChns;
                        if (temp.ID == 6) ArayTmp[2] = 63;
                        ArayTmp[3] = Convert.ToByte(temp.TrueDelay / 256);
                        ArayTmp[4] = Convert.ToByte(temp.TrueDelay % 256);
                        ArayTmp[5] = Convert.ToByte(temp.FalseDelay / 256);
                        ArayTmp[6] = Convert.ToByte(temp.FalseDelay % 256);
                        if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1D24, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType)) == true)
                        {
                            HDLUDP.TimeBetwnNext(20);
                        }
                        else return false;
                        if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(70 + i);
                    }
                }
                #endregion
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(100);
            return true;
        }


        /// <summary>
        /// 下载所有数据
        /// </summary>
        /// <param name="DevName"></param>
        /// <returns></returns>
        public bool DownloadFHInforsFrmDevice(string DevName, int wdDeviceType, int intActivePage, byte num1, byte num2)
        {
            string strMainRemark = DevName.Split('\\')[1].Trim();
            DevName = DevName.Split('\\')[0].Trim();

            byte bytSubID = byte.Parse(DevName.Split('-')[0].ToString());
            byte bytDevID = byte.Parse(DevName.Split('-')[1].ToString());

            byte[] ArayTmp = new byte[0];
            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x000E, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType)) == true)
            {
                byte[] arayRemark = new byte[20];
                for (int intI = 0; intI < 20; intI++) { arayRemark[intI] = CsConst.myRevBuf[25 + intI]; }
                DevName = bytSubID.ToString() + "-" + bytDevID.ToString() + "\\" + HDLPF.Byte2String(arayRemark);

            }
            else return false;
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(1);

            if (intActivePage == 0 || intActivePage == 1)
            {
                #region
                arayDateTime = new byte[10];
                araySummer = new byte[30];
                arayOutdoor = new byte[10];
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xD99E, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType)) == true)
                {
                    Array.Copy(CsConst.myRevBuf, 25, arayDateTime, 0, 7);
                    HDLUDP.TimeBetwnNext(1);
                }
                else return false;
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(2);
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1C12, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType)) == true)
                {
                    Array.Copy(CsConst.myRevBuf, 25, arayDateTime, 7, 2);
                    HDLUDP.TimeBetwnNext(1);
                }
                else return false;
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(3);
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1D1A, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType)) == true)
                {
                    Array.Copy(CsConst.myRevBuf, 26, araySummer, 0, 23);
                    HDLUDP.TimeBetwnNext(1);
                }
                else return false;
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(4);
                if (wdDeviceType == 211 || wdDeviceType == 208 || wdDeviceType == 209)
                {
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1D06, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType)) == true)
                    {
                        Array.Copy(CsConst.myRevBuf, 25, arayOutdoor, 0, 5);
                        HDLUDP.TimeBetwnNext(1);
                    }
                    else return false;
                }
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(5);
                #endregion
                MyRead2UpFlags[0] = true;
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(10);
            //读取6回路状态
            if (intActivePage == 0 || intActivePage == 2)
            {
                #region
                myHeating = new List<FHeating>();
                for (byte i = 1; i <= 6; i++)
                {
                    FHeating temp = new FHeating();
                    temp.ID = i;
                    temp.arayWorkControl = new byte[20];
                    temp.arayFlush = new byte[20];
                    temp.araySensorSetting = new byte[30];
                    temp.arayWorkSetting = new byte[20];
                    if (CsConst.isRestore || (i >= num1 && i <= num2))
                    {
                        ArayTmp = new byte[1];
                        ArayTmp[0] = i;
                        if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1D02, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType)) == true)
                        {
                            byte[] arayRemark = new byte[20];
                            for (int intI = 0; intI < 20; intI++) { arayRemark[intI] = CsConst.myRevBuf[26 + intI]; }
                            temp.strRemark = HDLPF.Byte22String(arayRemark, true);
                            HDLUDP.TimeBetwnNext(1);
                        }
                        else return false;
                        if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(10 + (i - 1) * 5 + 1);

                        if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1C5E, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType)) == true)
                        {
                            Array.Copy(CsConst.myRevBuf, 26, temp.arayWorkControl, 0, 13);
                            HDLUDP.TimeBetwnNext(1);
                        }
                        else return false;
                        if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(10 + (i - 1) * 5 + 2);

                        if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1C5A, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType)) == true)
                        {
                            Array.Copy(CsConst.myRevBuf, 26, temp.arayWorkSetting, 0, 10);
                            HDLUDP.TimeBetwnNext(1);
                        }
                        else return false;
                        if (wdDeviceType == 211 || wdDeviceType == 208 || wdDeviceType == 209)
                        {
                            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1D0A, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType)) == true)
                            {
                                Array.Copy(CsConst.myRevBuf, 26, temp.arayWorkSetting, 10, 1);
                                HDLUDP.TimeBetwnNext(1);
                            }
                            else return false;
                        }
                        if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(10 + (i - 1) * 5 + 3);


                        if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1C56, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType)) == true)
                        {
                            Array.Copy(CsConst.myRevBuf, 26, temp.araySensorSetting, 0, 20);
                            HDLUDP.TimeBetwnNext(1);
                        }
                        else return false;

                        if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1C62, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType)) == true)
                        {
                            Array.Copy(CsConst.myRevBuf, 26, temp.arayFlush, 0, 11);
                            HDLUDP.TimeBetwnNext(1);
                        }
                        else return false;
                        if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(10 + (i - 1) * 5 + 4);
                        myHeating.Add(temp);
                    }
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(10 + i * 5);
                }
                #endregion
                MyRead2UpFlags[1] = true;
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(40);
            //读取主从机功能
            if (intActivePage == 0 || intActivePage == 3)
            {
                #region
                araySynChannel = new byte[10];
                arayHost = new byte[20];
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1C6E, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType)) == true)
                {
                    Array.Copy(CsConst.myRevBuf, 25, araySynChannel, 0, 6);
                    HDLUDP.TimeBetwnNext(1);
                }
                else return false;
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(41);
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1CE6, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType)) == true)
                {
                    Array.Copy(CsConst.myRevBuf, 25, arayHost, 0, 15);
                    HDLUDP.TimeBetwnNext(1);
                }
                else return false;
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(42);
                #endregion
                MyRead2UpFlags[2] = true;
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(50);
             // 读取额外目标设置
            if (intActivePage == 0 || intActivePage == 4 || intActivePage == 6)
            {
                #region
                byte Chn = Convert.ToByte(num1 >> 4);
                byte Statue = Convert.ToByte(num2 >> 4);
                myTargets1 = new List<FHTargets>();
                for (byte i = 1; i <= 6; i++)
                {
                    if (CsConst.isRestore || i == Chn)
                    {
                        for (byte j = 0; j < 4; j++)
                        {
                            if (CsConst.isRestore || j == Statue)
                            {
                                FHTargets temp = new FHTargets();
                                temp.ID = i;
                                temp.StatusIndex = j;
                                temp.Targets = new List<UVCMD.ControlTargets>();
                                ArayTmp = new byte[3];
                                ArayTmp[0] = i;
                                ArayTmp[1] = j;
                                for (byte k = 0; k < 5; k++)
                                {
                                    ArayTmp[2] = k;
                                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1CE2, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType)) == true)
                                    {
                                        UVCMD.ControlTargets oCMD = new UVCMD.ControlTargets();
                                        oCMD.ID = Convert.ToByte(CsConst.myRevBuf[27]);
                                        oCMD.Type = CsConst.myRevBuf[28];  //转换为正确的类型
                                        oCMD.SubnetID = CsConst.myRevBuf[29];
                                        oCMD.DeviceID = CsConst.myRevBuf[30];
                                        oCMD.Param1 = CsConst.myRevBuf[31];
                                        oCMD.Param2 = CsConst.myRevBuf[32];
                                        oCMD.Param3 = CsConst.myRevBuf[33];
                                        oCMD.Param4 = CsConst.myRevBuf[34];
                                        temp.Targets.Add(oCMD);
                                        HDLUDP.TimeBetwnNext(1);
                                    }
                                    else return false;
                                }
                                myTargets1.Add(temp);
                            }
                        }
                    }
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(50 + i);
                }
                if ((wdDeviceType == 211 || wdDeviceType == 208 || wdDeviceType == 209) && intActivePage != 6)
                {
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1CEA, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType)) == true)
                    {
                        FHTargetEnable = CsConst.myRevBuf[25];
                        RelayTargetEnable = CsConst.myRevBuf[26];
                    }
                    else return false;
                }
                #endregion
                MyRead2UpFlags[5] = true;
                MyRead2UpFlags[3] = true;
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(60);
            if (intActivePage == 0 || intActivePage == 5 || intActivePage == 6)
            {
                #region
                if (wdDeviceType == 211 || wdDeviceType == 208 || wdDeviceType == 209)
                {
                    #region
                    byte Chn = Convert.ToByte(num1 & 0x0F);
                    byte Statue = Convert.ToByte(num2 & 0x0F);
                    myTargets2 = new List<FHTargets>();
                    for (byte i = 1; i <= 6; i++)
                    {
                        if (CsConst.isRestore || i == Chn)
                        {
                            for (byte j = 0; j < 2; j++)
                            {
                                if (CsConst.isRestore || j == Statue)
                                {
                                    FHTargets temp = new FHTargets();
                                    temp.ID = i;
                                    temp.StatusIndex = j;
                                    temp.Targets = new List<UVCMD.ControlTargets>();
                                    ArayTmp = new byte[3];
                                    ArayTmp[0] = i;
                                    ArayTmp[1] = j;
                                    for (byte k = 0; k < 5; k++)
                                    {
                                        ArayTmp[2] = k;
                                        if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1CEE, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType)) == true)
                                        {
                                            UVCMD.ControlTargets oCMD = new UVCMD.ControlTargets();
                                            oCMD.ID = Convert.ToByte(CsConst.myRevBuf[27]);
                                            oCMD.Type = CsConst.myRevBuf[28];  //转换为正确的类型
                                            oCMD.SubnetID = CsConst.myRevBuf[29];
                                            oCMD.DeviceID = CsConst.myRevBuf[30];
                                            oCMD.Param1 = CsConst.myRevBuf[31];
                                            oCMD.Param2 = CsConst.myRevBuf[32];
                                            oCMD.Param3 = CsConst.myRevBuf[33];
                                            oCMD.Param4 = CsConst.myRevBuf[34];
                                            temp.Targets.Add(oCMD);
                                            HDLUDP.TimeBetwnNext(1);
                                        }
                                        else return false;
                                    }
                                    myTargets2.Add(temp);
                                }
                            }
                        }
                        if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(60 + i);
                    }

                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1CEA, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType)) == true)
                    {
                        FHTargetEnable = CsConst.myRevBuf[25];
                        RelayTargetEnable = CsConst.myRevBuf[26];
                    }
                    else return false;
                    #endregion
                }
                #endregion
                MyRead2UpFlags[5] = true;
                MyRead2UpFlags[4] = true;
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(70);
            if (intActivePage == 0 || intActivePage == 7)
            {
                #region
                if (wdDeviceType == 208)
                {
                    myPumps = new List<Pumps>();
                    if (CsConst.mySends.AddBufToSndList(null, 0x1D22, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType)) == true)
                    {
                        PumpsEnable = CsConst.myRevBuf[25];
                        HDLUDP.TimeBetwnNext(1);
                    }
                    else return false;
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(71);
                    for (byte i = 0; i <= 6; i++)
                    {
                        ArayTmp = new byte[1];
                        ArayTmp[0] = i;
                        if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1D26, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType)) == true)
                        {
                            Pumps temp = new Pumps();
                            temp.TrueTargets = new List<UVCMD.ControlTargets>();
                            temp.FalseTargets = new List<UVCMD.ControlTargets>();
                            temp.ID = i;
                            temp.Enable = CsConst.myRevBuf[26];
                            temp.ChooseChns = CsConst.myRevBuf[27];
                            temp.TrueDelay = CsConst.myRevBuf[28] * 256 + CsConst.myRevBuf[29];
                            temp.FalseDelay = CsConst.myRevBuf[30] * 256 + CsConst.myRevBuf[31];
                            if (temp.TrueDelay > 600) temp.TrueDelay = 600;
                            if (temp.FalseDelay > 600) temp.FalseDelay = 600;
                            myPumps.Add(temp);
                            HDLUDP.TimeBetwnNext(1);
                        }
                        else return false;
                    }
                }
                #endregion
                MyRead2UpFlags[6] = true;
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(100, null);
            return true;
        }

    }
}
