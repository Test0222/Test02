using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.Xml;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Linq;


namespace HDL_Buspro_Setup_Tool
{
     [Serializable]
    public class DLP : Panel
    {
        public DeviceInfo BrdDev;   // 广播到的设备ID
        public Byte[] BasicInfo;

        internal ACSetting DLPAC;
        internal BGMusic DLPMusic;
        internal FloorHeating DLPFH;

          [Serializable]
        internal class BGMusic
        {
            public byte bytEnable;    //如果启用显示，不启用选择类型
            public byte bytMusicType;
            public byte bytCurZone;  // zone no.
            public byte[] ArayDevs = new byte[48];

            public UVCMD.ControlTargets[] KeyTargets = new UVCMD.ControlTargets[14];
        }

        /// <summary>
        /// upload panel information to device
        /// </summary>
        /// <param name="DIndex"></param>
        /// <param name="DevID"></param>
        /// <param name="DeviceType"></param>
        /// <param name="DevName"></param>
        /// <returns></returns>
        public override bool UploadPanelInfosToDevice(string DevName, int intActivePage, int DeviceType)// 0 mean all, else that tab only
        {
            //保存回路信息
            string strMainRemark = DevName.Split('\\')[1].Trim();
            string TmpDevName = DevName.Split('\\')[0].Trim();
            byte SubNetID = byte.Parse(TmpDevName.Split('-')[0].ToString());
            byte DeviceID = byte.Parse(TmpDevName.Split('-')[1].ToString());

            int wdMaxValue = DeviceTypeList.GetMaxValueFromPublicModeGroup(DeviceType);

            base.UploadPanelInfosToDevice(DeviceName, intActivePage, DeviceType);

            Byte FhLowLimit = 5;
            Byte FhHighLimit = 35;
            if (DLPFH == null)
            {
                DLPFH = new FloorHeating();
            }
            else
            {
                FhLowLimit = DLPFH.minTemp;
                FhHighLimit = DLPFH.maxTemp;
            }

            if (intActivePage == 0 || intActivePage == 3)//空调窗体
            {
                if (DLPAC != null) DLPAC.UploadACSettingToDevice(SubNetID, DeviceID, DeviceType, bytTempType, FhLowLimit, FhHighLimit, AdjustValue);
            }
            else if (intActivePage == 0 || intActivePage == 4) //音乐窗体
            {
                #region
                byte[] ArayTmp = new byte[3];
                ArayTmp[0] = DLPMusic.bytEnable;
                ArayTmp[1] = DLPMusic.bytCurZone;
                ArayTmp[2] = DLPMusic.bytMusicType;
                CsConst.mySends.AddBufToSndList(ArayTmp, 0x1932, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType));
                HDLUDP.TimeBetwnNext(ArayTmp.Length);

                ArayTmp = new byte[25];
                ArayTmp[0] = 0;
                Array.Copy(DLPMusic.ArayDevs, 0, ArayTmp, 1, 24);

                CsConst.mySends.AddBufToSndList(ArayTmp, 0x1936, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType));
                HDLUDP.TimeBetwnNext(ArayTmp.Length);
                Byte CmdID = 0;
                foreach (UVCMD.ControlTargets TmpCmd in DLPMusic.KeyTargets)
                {
                    if (TmpCmd != null)
                    {
                        if (TmpCmd.ID == 0) TmpCmd.ID = 1;
                        byte[] arayCMD = new byte[8];
                        arayCMD[0] = CmdID;
                        arayCMD[1] = TmpCmd.Type;
                        arayCMD[2] = TmpCmd.SubnetID;
                        arayCMD[3] = TmpCmd.DeviceID;
                        arayCMD[4] = TmpCmd.Param1;
                        arayCMD[5] = TmpCmd.Param2;
                        arayCMD[6] = TmpCmd.Param3;   // save targets
                        arayCMD[7] = TmpCmd.Param4;
                        CsConst.mySends.AddBufToSndList(arayCMD, 0x195C, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType));
                        HDLUDP.TimeBetwnNext(arayCMD.Length);
                    }
                    CmdID++;
                }
              
                #endregion
                MyRead2UpFlags[4] = true;
            }
            else if (intActivePage == 0 || intActivePage == 5)  //地热窗体
            {
                if (DLPAC == null)
                {
                    DLPAC = new ACSetting();
                    Byte TempOldLowlimit = 5;
                    Byte TempOldHighLimit = 35;
                    DLPAC.ReadDLPACPTemperatureRange(SubNetID, DeviceID, DeviceType,ref TempOldLowlimit,ref TempOldHighLimit);
                }
                DLPFH.UploadFloorheatingSettingToDevice(SubNetID, DeviceID, DeviceType, DLPAC.bytTempArea);
            }
            else if (intActivePage == 0 || intActivePage == 9) // 触摸14按键面板
            {
                // LEd颜色
                #region
                if (CsConst.mintMPTLDeviceType.Contains(DeviceType))
                {
                    if (arayButtonColor != null)
                    {
                        if (arayButtonColor.Length == 387 && arayButtonColor[0] == 7)
                        {
                            Byte[] ArayMain = null;
                            if (DeviceType == 170 || DeviceType == 172 || DeviceType == 175)
                            {
                                ArayMain = new byte[9];
                                ArayMain[0] = 0;
                                Array.Copy(arayButtonColor, 1, ArayMain, 1, 8);
                                if (CsConst.mySends.AddBufToSndList(ArayMain, 0x198E, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == false) return false;
                                CsConst.myRevBuf = new byte[1200];
                                HDLUDP.TimeBetwnNext(20);
                            }
                            else
                            {
                                ArayMain = new byte[5];
                                ArayMain[0] = 0;
                                Array.Copy(arayButtonColor, 1, ArayMain, 1, 5);
                                if (CsConst.mySends.AddBufToSndList(ArayMain, 0x198E, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == false) return false;
                                CsConst.myRevBuf = new byte[1200];
                                HDLUDP.TimeBetwnNext(20);
                            }
                            for (int i = 1; i <= 7; i++)
                            {
                                ArayMain = new byte[55];
                                ArayMain[0] = Convert.ToByte(i);
                                Array.Copy(arayButtonColor, (i - 1) * 54 + 9, ArayMain, 1, 54);
                                if (CsConst.mySends.AddBufToSndList(ArayMain, 0x198E, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == false) return false;
                                CsConst.myRevBuf = new byte[1200];
                                HDLUDP.TimeBetwnNext(20);
                            }
                        }
                    }
                }
                #endregion
                //白平衡
                #region
                if (arayButtonBalance[0] > 0)
                {
                    byte[] arayTmp = new byte[arayButtonBalance[0] * 3];
                    Array.Copy(arayButtonBalance, 1, arayTmp, 0, arayButtonBalance.Length - 1);
                    if (CsConst.mySends.AddBufToSndList(arayTmp, 0x199A, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                    {
                        CsConst.myRevBuf = new byte[1200];
                        HDLUDP.TimeBetwnNext(10);
                    }
                    else return false;
                }
                #endregion
                //休眠
                #region
                if (DeviceType == 175 || DeviceType == 180 || DeviceType == 5004)
                {
                    if (araySleep != null)
                    {
                        byte[] arayTmp = new byte[5];
                        Array.Copy(araySleep, 1, arayTmp, 0, 5);
                        if (CsConst.mySends.AddBufToSndList(arayTmp, 0xE4F8, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                        {
                            CsConst.myRevBuf = new byte[1200];
                        }
                    }
                }
                #endregion

                //基本信息
                #region
                //2014-12-25 新增休眠模式（状态灯关闭延迟、红外感应按键号）
                Byte[] bytTmp = new byte[8];    // 显示要不要跳转，跳转时间
                bytTmp[0] = otherInfo.bytBacklightTime;
                bytTmp[1] = otherInfo.bytBacklight;
                bytTmp[2] = otherInfo.bytGotoPage;
                bytTmp[3] = otherInfo.bytGotoTime;
                bytTmp[4] = 0;
                bytTmp[5] = otherInfo.AutoLock;
                bytTmp[6] = 0;
                bytTmp[7] = 0;
                if (DeviceType == 165)
                {
                    bytTmp[4] = otherInfo.SoundClick;
                    bytTmp[5] = otherInfo.AutoLock;
                    bytTmp[6] = otherInfo.bytLigthDelayState;
                    bytTmp[7] = otherInfo.IRCloseTargets;
                }
                if (DeviceType == 168 || DeviceType == 170 || DeviceType == 172 || DeviceType == 175)
                {
                    bytTmp = new byte[9];    // 显示要不要跳转，跳转时间
                    bytTmp[0] = otherInfo.bytBacklightTime;
                    bytTmp[1] = otherInfo.bytBacklight;
                    bytTmp[2] = otherInfo.bytGotoPage;
                    bytTmp[3] = otherInfo.bytGotoTime;
                    bytTmp[4] = otherInfo.SoundClick;
                    bytTmp[5] = otherInfo.AutoLock;
                    bytTmp[6] = otherInfo.bytLigthDelayState;
                    bytTmp[7] = otherInfo.IRCloseTargets;
                    bytTmp[8] = otherInfo.IRCloseSensor;
                    if (otherInfo.isHasSensorSensitivity)
                    {
                        bytTmp = new byte[10];    // 显示要不要跳转，跳转时间
                        bytTmp[0] = otherInfo.bytBacklightTime;
                        bytTmp[1] = otherInfo.bytBacklight;
                        bytTmp[2] = otherInfo.bytGotoPage;
                        bytTmp[3] = otherInfo.bytGotoTime;
                        bytTmp[4] = otherInfo.SoundClick;
                        bytTmp[5] = otherInfo.AutoLock;
                        bytTmp[6] = otherInfo.bytLigthDelayState;
                        bytTmp[7] = otherInfo.IRCloseTargets;
                        bytTmp[8] = otherInfo.IRCloseSensor;
                        bytTmp[9] = otherInfo.CloseToSensorSensitivity;
                    }
                }

                if (CsConst.mySends.AddBufToSndList(bytTmp, 0xE13A, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == false) return false;
                HDLUDP.TimeBetwnNext(otherInfo.bytAryShowPage.Length);
                CsConst.myRevBuf = new byte[1200];
                #endregion
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(100);
            return true;
        }
        
        public override void DownLoadInformationFrmDevice(string DevName, int intActivePage, int DeviceType, int num1, int num2)// 0 mean all, else that tab only
        {
            string strMainRemark = DevName.Split('\\')[1].Trim();
            String TmpDevName = DevName.Split('\\')[0].Trim();

            byte SubNetID = byte.Parse(TmpDevName.Split('-')[0].ToString());
            byte DeviceID = byte.Parse(TmpDevName.Split('-')[1].ToString());
            byte[] ArayTmp = null;
            int wdMaxValue = DeviceTypeList.GetMaxValueFromPublicModeGroup(DeviceType);

            base.DownLoadInformationFrmDevice(DeviceName, intActivePage, DeviceType, num1, num2);
            
            IsBrdTemp = (arayTempBroadCast[1] ==1);
            String sBrdName = arayTempBroadCast[2].ToString() + "-" + arayTempBroadCast[3].ToString();
            BrdDev = new DeviceInfo(sBrdName);


            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(25);
            Byte FhLowLimit = 5;
            Byte FhHighLimit = 35;

            DLPAC = new ACSetting();
            if (intActivePage == 0 || intActivePage == 2)
            {
                ModifyPageVisibleOrNotInformation(SubNetID, DeviceID, DeviceType);
            }

            if (intActivePage == 0 || intActivePage == 3)
            {
                DLPAC.DownloadACSettingFromDevice(SubNetID, DeviceID, DeviceType, ref AdjustValue, ref FhLowLimit,ref FhHighLimit);

                if (DLPFH == null) DLPFH = new FloorHeating();
                DLPFH.minTemp = FhLowLimit;
                DLPFH.maxTemp = FhHighLimit;
            }
            else if (intActivePage == 0 || intActivePage == 4)
            {
                // music setup
                #region
                DLPMusic = new BGMusic();
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1930, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                {
                    DLPMusic.bytEnable = CsConst.myRevBuf[25]; //背光时间 ，永远显示：0，其他 10-99s
                    DLPMusic.bytCurZone = CsConst.myRevBuf[26];     //背光亮度
                    DLPMusic.bytMusicType = CsConst.myRevBuf[27];      //不跳转0，其他页面1-7
                    //otherInfo.bytGotoTime = CsConst.myRevBuf[28];      //20-150s
                    CsConst.myRevBuf = new byte[1200];
                }
                DLPMusic.ArayDevs = new byte[48];

                ArayTmp = new byte[1];
                for (byte bytI = 0; bytI < 2; bytI++)
                {
                    ArayTmp[0] = bytI;
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1934, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                    {
                        Array.Copy(CsConst.myRevBuf, 27, DLPMusic.ArayDevs, bytI * 24, 24);
                        CsConst.myRevBuf = new byte[1200];
                    }
                }

                // read advance commands when playing
                DLPMusic.KeyTargets = new UVCMD.ControlTargets[14];
                ArayTmp = new byte[1];
                Byte bTotalMusic = 8;
                if (DLPPanelDeviceTypeList.DLPWithNewMusicSources.Contains(DeviceType)) // new music sources 
                    bTotalMusic = 14;
                for (byte bytI = 0; bytI < bTotalMusic; bytI++)
                {
                    ArayTmp[0] = bytI;
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x195A, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                    {
                        DLPMusic.KeyTargets[bytI] = new UVCMD.ControlTargets();
                        DLPMusic.KeyTargets[bytI].ID = CsConst.myRevBuf[26];
                        DLPMusic.KeyTargets[bytI].Type = CsConst.myRevBuf[27];
                        DLPMusic.KeyTargets[bytI].SubnetID = CsConst.myRevBuf[28];
                        DLPMusic.KeyTargets[bytI].DeviceID = CsConst.myRevBuf[29];
                        DLPMusic.KeyTargets[bytI].Param1 = CsConst.myRevBuf[30];
                        DLPMusic.KeyTargets[bytI].Param2 = CsConst.myRevBuf[31];
                        DLPMusic.KeyTargets[bytI].Param3 = CsConst.myRevBuf[32];
                        DLPMusic.KeyTargets[bytI].Param4 = CsConst.myRevBuf[33];

                        CsConst.myRevBuf = new byte[1200];
                    }
                }

                #endregion
            }
            else if (intActivePage == 0 || intActivePage == 5) // 地热页面
            {
                DLPFH = new FloorHeating();

                if (DLPAC == null) DLPAC = new ACSetting();
                DLPFH.DownloadFloorheatingsettingFromDevice(SubNetID,DeviceID,DeviceType,ref DLPAC.bytTempArea); 
            }
            else if (intActivePage == 0 || intActivePage == 9) // LED颜色和快捷键页面
            {
                //页面跳转信息
                #region
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xE138, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                {
                    otherInfo.bytBacklightTime = CsConst.myRevBuf[25]; //背光时间 ，永远显示：0，其他 10-99s
                    otherInfo.bytBacklight = CsConst.myRevBuf[26];     //背光亮度
                    otherInfo.bytGotoPage = CsConst.myRevBuf[27];      //不跳转0，其他页面1-7
                    otherInfo.bytGotoTime = CsConst.myRevBuf[28];      //20-150s
                    otherInfo.AutoLock = CsConst.myRevBuf[30];
                    if (CsConst.mintMPTLDeviceType.Contains(DeviceType))
                    {
                        otherInfo.bytLigthDelayState = CsConst.myRevBuf[31];
                        otherInfo.IRCloseTargets = CsConst.myRevBuf[32];
                        otherInfo.IRCloseSensor = CsConst.myRevBuf[33];
                        if (CsConst.myRevBuf[16] >= 0x15)
                        {
                            otherInfo.isHasSensorSensitivity = true;
                            otherInfo.CloseToSensorSensitivity = CsConst.myRevBuf[34];
                        }
                        else
                        {
                            otherInfo.isHasSensorSensitivity = false;
                        }
                    }
                    CsConst.myRevBuf = new byte[1200];
                }
                #endregion

                //白平衡
                #region
                ArayTmp = null;
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1998, SubNetID, DeviceID, false, false, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                {
                    int num = wdMaxValue;
                    if (CsConst.mintMPTLDeviceType.Contains(DeviceType))
                    {
                        arayButtonBalance = new byte[12 * 3 + 1];
                        num = 12;
                    }
                    for (int i = 1; i <= num; i++)
                    {
                        arayButtonBalance[0] = Convert.ToByte(i);
                        Array.Copy(CsConst.myRevBuf, 25 + (i - 1) * 3, arayButtonBalance, (i - 1) * 3 + 1, 3);
                    }
                }
                #endregion

                //睡眠模式
                #region
                if (DeviceType == 175 || DeviceType == 180 || DeviceType == 5004)
                {
                    araySleep = new byte[6];
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xE4FA, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                    {
                        araySleep[0] = 1;
                        Array.Copy(CsConst.myRevBuf, 25, araySleep, 1, 5);
                        CsConst.myRevBuf = new byte[1200];
                    }
                }
                MyRead2UpFlags[9] = true;
                #endregion

                // LED颜色和快捷键
                #region
                arayButtonColor = new byte[387];
                for (int i = 0; i <= 7; i++)
                {
                    ArayTmp = new byte[1];
                    ArayTmp[0] = Convert.ToByte(i);
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x198C, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                    {
                        arayButtonColor[0] = Convert.ToByte(i);
                        if (i == 0)
                            Array.Copy(CsConst.myRevBuf, 26, arayButtonColor, 1, 8);
                        else
                            Array.Copy(CsConst.myRevBuf, 26, arayButtonColor, (i - 1) * 54 + 9, 54);
                        CsConst.myRevBuf = new byte[1200];
                        HDLUDP.TimeBetwnNext(10);
                    }
                }
                #endregion
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(100);
        }


        public void ModifyPageVisibleOrNotInformation(Byte SubNetID, Byte DeviceID, int DeviceType)
        {
            if (DLPPanelDeviceTypeList.HDLDLPPanelDeviceTypeList.Contains(DeviceType))
            {
                if (otherInfo.bytAryShowPage != null)  //显示页面信息
                {
                    if (CsConst.mySends.AddBufToSndList(otherInfo.bytAryShowPage, 0xE12E, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == false)  return ;
                    HDLUDP.TimeBetwnNext(otherInfo.bytAryShowPage.Length);
                    CsConst.myRevBuf = new byte[1200];
                }
            }
        }
    }
}
