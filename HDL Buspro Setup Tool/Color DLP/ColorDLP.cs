using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;

namespace HDL_Buspro_Setup_Tool
{
     [Serializable]
    public class ColorDLP : HdlDeviceBackupAndRestore
    {
        //基本信息
        public int DIndex;//设备唯一编号
        public string DeviceName;//子网ID 设备ID 备注
        public List<HDLButton> MyKeys;
        public List<EnviroAc> MyAC;
        public List<Curtain> MyCurtain;
        public List<EnviroFH> MyHeat;
        public List<EnviroMusic> MyMusic;
        public List<HDLButton> myScenes;
        public List<Sensor> mySensor;
        public bool isHasSensorSensitivity = false;
        public bool[] MyRead2UpFlags = new bool[20];
        public byte TemperatureType;
        public byte[] BasicInfo = new byte[20];//背光，调光低限，时间
        public byte[] BroadCastAry = new byte[20];
        public List<byte[]> arayPIC = new List<byte[]>();//数组的前三个byte代表起始地址

        [Serializable]
        public class Curtain
        {
            public byte CurtainType;//窗帘类型
            public byte PageID;//窗帘页号
            public byte KeyNo;//窗帘号
            public string Remark;//备注
            public byte SubnetID;//子网ID
            public byte DeviceID;//设备ID
            public byte Mode;//窗帘模式
            public byte CurtainNo;//窗帘号
            public byte CurtainSwitch;//窗帘开关
            public byte Param3;//参数3
        }
        [Serializable]
        public class Sensor
        {
            public byte ID;
            public byte[] arayPM;
        }

        ///<summary>
        ///读取默认的设置
        ///</summary>
        public void ReadDefaultInfo()
        {
            MyKeys = new List<HDLButton>();
            MyAC = new List<EnviroAc>();
            MyHeat = new List<EnviroFH>();
            MyCurtain = new List<Curtain>();
            MyMusic = new List<EnviroMusic>();
            mySensor = new List<Sensor>();
        }

        public void SaveToDataBase()
        {
            try
            {
                string strsql = string.Format("delete * from dbClassInfomation where DIndex={0}", DIndex);
                DataModule.ExecuteSQLDatabase(strsql);

                strsql = string.Format("delete * from dbKeyTargets where DIndex={0}", DIndex);
                DataModule.ExecuteSQLDatabase(strsql);
            }
            catch
            {
            }
        }

        public void ReadDataBase(int DIndex)
        {
            try
            {

            }
            catch
            {
            }
        }

        ///<summary>
        ///下载设置
        ///</summary>
        public virtual void DownloadColorDLPInfoToDevice(int DeviceType, string DevName, int intActivePage)
        {
            string strMainRemark = DevName.Split('\\')[1].Trim();
             DevName = DevName.Split('\\')[0].Trim();

            byte bytSubID = byte.Parse(DevName.Split('-')[0].ToString());
            byte bytDevID = byte.Parse(DevName.Split('-')[1].ToString());
            int wdMaxValue = DeviceTypeList.GetMaxValueFromPublicModeGroup(DeviceType);
            byte[] ArayTmp = new byte[0];
            
            String Remark =HDLSysPF.ReadDeviceMainRemark(bytSubID,bytDevID);
            DeviceName = bytSubID.ToString() + "-" + bytDevID.ToString() + "\\" + Remark;

            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(5);

            if (intActivePage == 0 || intActivePage == 1)
            {
                #region
                BasicInfo = new byte[20];
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xE010, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                {
                    BasicInfo[2] = CsConst.myRevBuf[25];//背光亮度
                    CsConst.myRevBuf = new byte[1200];
                }
                else return;
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xE138, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                {
                    BasicInfo[0] = CsConst.myRevBuf[25];//背光显示
                    BasicInfo[6] = CsConst.myRevBuf[26];//背光显示亮度
                    BasicInfo[7] = CsConst.myRevBuf[27];//返回页
                    BasicInfo[8] = CsConst.myRevBuf[28];//返回页延时
                    BasicInfo[1] = CsConst.myRevBuf[29];//按键声
                    BasicInfo[3] = CsConst.myRevBuf[33];//红外接近传感器使能
                    if (CsConst.myRevBuf[16] >= 0x15)
                    {
                        isHasSensorSensitivity = true;
                        BasicInfo[9] = CsConst.myRevBuf[34];
                    }
                    else
                    {
                       isHasSensorSensitivity = false;
                    }
                    CsConst.myRevBuf = new byte[1200];
                }
                else return;
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(6);
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xE0E0, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                {
                    BasicInfo[4] = CsConst.myRevBuf[27];//调光低限
                    BasicInfo[5] = CsConst.myRevBuf[29];//长按时间
                    BasicInfo[15] = CsConst.myRevBuf[28];//温度显示
                    BasicInfo[16] = CsConst.myRevBuf[30];//时间显示
                    CsConst.myRevBuf = new byte[1200];
                }
                else return;
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(7);
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xE120, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                {
                    TemperatureType = CsConst.myRevBuf[25];
                    CsConst.myRevBuf = new byte[1200];
                }
                else return;
                /*if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(8);
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xDA00, bytSubID, bytDevID, false, true) == true)
                {
                    Array.Copy(CsConst.myRevBuf, 26, BasicInfo, 6, 7);
                    CsConst.myRevBuf = new byte[1200];
                }*/
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(9);
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xE128, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                {
                    BasicInfo[13] = CsConst.myRevBuf[25];//日期类型
                    BasicInfo[14] = CsConst.myRevBuf[26];//日期格式
                    CsConst.myRevBuf = new byte[1200];
                }
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(10);
                #endregion
                MyRead2UpFlags[0] = true;
            }
            if (intActivePage == 0 || intActivePage == 2)
            {
                #region
                MyKeys =new List<HDLButton>();
                for (byte i = 0; i < 6; i++)//6页
                {
                    //读取按键模式
                    ArayTmp = new byte[3];
                    ArayTmp[0] = 0;
                    ArayTmp[1] = i;
                    ArayTmp[2] = 12;
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x199E, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                    {
                        for (byte j = 0; j < 11; j++)
                        {
                            HDLButton temp = new HDLButton();
                            temp.PageID = i;
                            temp.ID = Convert.ToByte(j + 1);
                            temp.KeyTargets = new List<UVCMD.ControlTargets>();
                            temp.Mode = CsConst.myRevBuf[28 + j];
                            Remark = "";
                            MyKeys.Add(temp);
                        }
                        CsConst.myRevBuf = new byte[1200];
                        
                        for (byte j = 1; j <= 11; j++)//12个按键
                        {
                            ArayTmp = new byte[4];
                            ArayTmp[0] = j;
                            ArayTmp[1] = 0;
                            ArayTmp[2] = i;
                            ArayTmp[3] = 12;
                            //读取按键备注
                            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xE004, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                            {
                                byte[] arayRemark = new byte[20];
                                Array.Copy(CsConst.myRevBuf, 26, arayRemark, 0, 20);
                                MyKeys[(i * 11) + (j - 1)].Remark = HDLPF.Byte2String(arayRemark);
                                CsConst.myRevBuf = new byte[1200];
                            }
                            else return;
                        }
                        //读取按键调光使能和保存
                        ArayTmp = new byte[3];
                        ArayTmp[0] = 0;
                        ArayTmp[1] = i;
                        ArayTmp[2] = 12;
                        if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x19A2, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                        {
                            for (byte j = 0; j < 11; j++)
                            {
                                byte bytTmp = CsConst.myRevBuf[28 + j];
                                string str = GlobalClass.IntToBit(Convert.ToInt32(bytTmp), 8);
                                if (str.Substring(3, 1) == "1") MyKeys[(i * 11) + j].IsDimmer = 1;
                                else MyKeys[(i * 11) + j].IsDimmer = 0;
                                if (str.Substring(7, 1) == "1") MyKeys[(i * 11) + j].SaveDimmer = 1;
                                else MyKeys[(i * 11) + j].SaveDimmer = 0;
                            }
                            CsConst.myRevBuf = new byte[1200];
                        }
                        else return;
                        
                        //读取按键互斥
                        ArayTmp = new byte[3];
                        ArayTmp[0] = 0;
                        ArayTmp[1] = i;
                        ArayTmp[2] = 12;
                        if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x19A6, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                        {
                            for (byte j = 0; j < 11; j++)
                            {
                                MyKeys[(i * 11) + j].bytMutex = CsConst.myRevBuf[28 + j];
                            }
                            CsConst.myRevBuf = new byte[1200];
                        }
                        else return;

                        #region
                        if (CsConst.isRestore)
                        {
                            for (int j = 1; j <= 11; j++)
                            {
                                byte bytTotalCMD = 0;
                                switch (MyKeys[i * 11 + j-1].Mode)
                                {
                                    case 0: bytTotalCMD = 0; break;
                                    case 1:
                                    case 2:
                                    case 3: bytTotalCMD = 1; break;
                                    case 4:
                                    case 5:
                                    case 7: bytTotalCMD = 9; break;
                                }

                                ArayTmp = new byte[5];
                                ArayTmp[0] = Convert.ToByte(j);
                                MyKeys[i * 11 + j - 1].KeyTargets = new List<UVCMD.ControlTargets>();
                                for (byte bytJ = 0; bytJ < bytTotalCMD; bytJ++)
                                {
                                    ArayTmp[1] = Convert.ToByte(bytJ + 1);
                                    ArayTmp[2] = 0;
                                    ArayTmp[3] = Convert.ToByte(i);
                                    ArayTmp[4] = 12;
                                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xE000, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                                    {
                                        UVCMD.ControlTargets oCMD = new UVCMD.ControlTargets();
                                        oCMD.ID = Convert.ToByte(CsConst.myRevBuf[26]);
                                        oCMD.Type = CsConst.myRevBuf[27];  //转换为正确的类型
                                        oCMD.SubnetID = CsConst.myRevBuf[28];
                                        oCMD.DeviceID = CsConst.myRevBuf[29];
                                        oCMD.Param1 = CsConst.myRevBuf[30];
                                        oCMD.Param2 = CsConst.myRevBuf[31];
                                        oCMD.Param3 = CsConst.myRevBuf[32];
                                        oCMD.Param4 = CsConst.myRevBuf[33];
                                        CsConst.myRevBuf = new byte[1200];
                                        MyKeys[i * 11 + j - 1].KeyTargets.Add(oCMD);
                                    }
                                    else return;
                                }
                            }
                        }
                        #endregion
                    
                    }
                    else return;
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(10 + i);
                }
                #endregion
                MyRead2UpFlags[1] = true;
            }
            if (intActivePage == 0 || intActivePage == 3)
            {
                #region
                BroadCastAry = new byte[20];
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xE0F8, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                {
                    Array.Copy(CsConst.myRevBuf, 25, BroadCastAry, 0, 5);
                }
                else return;
                MyAC = new List<EnviroAc>();
                byte[] arayTmp = new byte[1];
                arayTmp[0] = 2;
                byte[] arayVisible = new byte[9];
                if (CsConst.mySends.AddBufToSndList(arayTmp, 0x19B2, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                {
                    Array.Copy(CsConst.myRevBuf, 26, arayVisible, 0, 9);
                    CsConst.myRevBuf = new byte[1200];
                }
                else return;
                 //读取空调综合参数
                ArayTmp = new byte[1];
                for (byte i = 0; i < 9; i++)
                {
                    ArayTmp[0] = i;
                    EnviroAc temp = new EnviroAc();
                    if (temp.ReadDLPACPageSettings(bytSubID, bytDevID, DeviceType, i) == true)
                    {
                        ArayTmp = new byte[1];
                        ArayTmp[0] = Convert.ToByte(i);
                    }
                    else return;
                    
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(20 + i);
                    MyAC.Add(temp);
                }
                #endregion
                MyRead2UpFlags[2] = true;
            }
            if (intActivePage == 0 || intActivePage == 4)
            {
                #region
                MyCurtain = new List<Curtain>();
                //读取备注
                for (byte i = 0; i < 6; i++)
                {
                    if (i < 5)
                    {
                        for (byte j = 0; j < 4; j++)
                        {
                            ArayTmp = new byte[4];
                            ArayTmp[2] = i;
                            ArayTmp[3] = 12;
                            ArayTmp[0] = Convert.ToByte(j + 1);
                            ArayTmp[1] = 2;
                            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xE004, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                            {
                                Curtain temp = new Curtain();
                                byte[] arayRemark = new byte[20];
                                Array.Copy(CsConst.myRevBuf, 26, arayRemark, 0, 20);
                                temp.CurtainType = 2;
                                temp.PageID = Convert.ToByte(i);
                                temp.KeyNo = Convert.ToByte(j + 1);
                                Remark = HDLPF.Byte2String(arayRemark);
                                CsConst.myRevBuf = new byte[1200];
                                ArayTmp = new byte[5];
                                ArayTmp[0] = Convert.ToByte(j + 1);
                                ArayTmp[1] = 1;
                                ArayTmp[2] = 2;
                                ArayTmp[3] = i;
                                ArayTmp[4] = 12;
                                CsConst.myRevBuf = new byte[1200];
                                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xE000, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                                {
                                    temp.Mode = CsConst.myRevBuf[27];
                                    temp.SubnetID = CsConst.myRevBuf[28];
                                    temp.DeviceID = CsConst.myRevBuf[29];
                                    temp.CurtainNo = CsConst.myRevBuf[30];
                                    temp.CurtainSwitch = CsConst.myRevBuf[31];
                                    temp.Param3 = CsConst.myRevBuf[32];
                                }
                                else return;
                                MyCurtain.Add(temp);
                                CsConst.myRevBuf = new byte[1200];
                                
                            }
                            else return;
                        }

                        ArayTmp = new byte[3];
                        ArayTmp[0] = 2;
                        ArayTmp[1] = i;
                        ArayTmp[2] = 12;
                        if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x199E, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                        {
                            for (int j = 0; j < 4; j++)
                            {
                                MyCurtain[i * 4 + j].Mode = CsConst.myRevBuf[28 + j];
                            }
                        }
                        else return;
                    }
                    else
                    {
                        for (byte j = 0; j < 2; j++)
                        {
                            ArayTmp = new byte[4];
                            ArayTmp[2] = i;
                            ArayTmp[3] = 12;
                            ArayTmp[0] = Convert.ToByte(j+1);
                            ArayTmp[1] = 3;
                            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xE004, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                            {
                                Curtain temp = new Curtain();
                                byte[] arayRemark = new byte[20];
                                Array.Copy(CsConst.myRevBuf, 26, arayRemark, 0, 20);
                                temp.CurtainType = 3;
                                temp.PageID = Convert.ToByte(i);
                                temp.KeyNo = Convert.ToByte(j + 1);
                                Remark = HDLPF.Byte2String(arayRemark);
                                CsConst.myRevBuf = new byte[1200];
                                ArayTmp = new byte[5];
                                ArayTmp[0] = Convert.ToByte(j + 1);
                                ArayTmp[1] = 1;
                                ArayTmp[2] = 3;
                                ArayTmp[3] = 0;
                                ArayTmp[4] = 12;
                                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xE000, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                                {
                                    temp.Mode = CsConst.myRevBuf[27];
                                    temp.SubnetID = CsConst.myRevBuf[28];
                                    temp.DeviceID = CsConst.myRevBuf[29];
                                    temp.CurtainNo = CsConst.myRevBuf[30];
                                    temp.CurtainSwitch = CsConst.myRevBuf[31];
                                    temp.Param3 = CsConst.myRevBuf[32];
                                }
                                else return;
                                MyCurtain.Add(temp);
                                CsConst.myRevBuf = new byte[1200];
                            }
                            else return;
                        }

                        ArayTmp = new byte[3];
                        ArayTmp[0] = 3;
                        ArayTmp[1] = 0;
                        ArayTmp[2] = 12;
                        if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x199E, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                        {
                            for (int j = 0; j < 2; j++)
                            {
                                MyCurtain[i * 4 + j].Mode = CsConst.myRevBuf[28 + j];
                            }
                        }
                        else return;
                    }
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(30 + i);
                }
                #endregion
                MyRead2UpFlags[3] = true;
            }
            if (intActivePage == 0 || intActivePage == 5)
            {
                #region
                MyHeat = new List<EnviroFH>();
                ArayTmp = new byte[1];
                for (byte i = 0; i < 9; i++)
                {
                    ArayTmp[0] = i;
                    EnviroFH temp = new EnviroFH();
                    temp.ID = i;
                    temp.DownloadFloorheatingsettingFromDevice(bytSubID, bytDevID, DeviceType);

                    temp.ReadCurrentStatusFromFloorheatingModule(bytSubID, bytDevID, DeviceType);

                    temp.CalculationModeTargets = new byte[40];
                    if (CsConst.isRestore)
                    {
                        ArayTmp = new byte[1];
                        ArayTmp[0] = i;
                        if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1972, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true) //新版把目标独立出来
                        {
                            Array.Copy(CsConst.myRevBuf, 25, temp.CalculationModeTargets, 0, 34);
                            CsConst.myRevBuf = new byte[1200];
                            HDLUDP.TimeBetwnNext(1);
                        }
                        else return;
                    }
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(40 + i);
                    MyHeat.Add(temp);
                }
                #endregion
                MyRead2UpFlags[4] = true;
            }
            if (intActivePage == 0 || intActivePage == 6)
            {
                #region
                MyMusic = new List<EnviroMusic>();
                for (byte i = 0; i < 9; i++)
                {
                    ArayTmp = new byte[1];
                    ArayTmp[0] = i;
                    EnviroMusic temp = new EnviroMusic();
                    temp.ReadMusicSettingInformation(bytSubID, bytDevID, DeviceType);
                    temp.ReadMusicAdvancedCommandsGroup(bytSubID, bytDevID, DeviceType);
                    MyMusic.Add(temp);
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(50 + i);
                }
                #endregion
                MyRead2UpFlags[5] = true;
            }
            if (intActivePage == 0 || intActivePage == 7)
            {
                #region
                myScenes = new List<HDLButton>();
                for (byte i = 0; i < 2; i++)
                {
                    //读取按键模式
                    ArayTmp = new byte[3];
                    ArayTmp[0] = 1;
                    ArayTmp[1] = i;
                    ArayTmp[2] = 12;
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x199E, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                    {
                        for (byte j = 0; j < 12; j++)
                        {
                            HDLButton temp = new HDLButton();
                            temp.PageID = i;
                            temp.ID = Convert.ToByte(j + 1);
                            temp.KeyTargets = new List<UVCMD.ControlTargets>();
                            temp.Mode = CsConst.myRevBuf[28 + j];
                            Remark = "";
                            myScenes.Add(temp);
                        }
                        CsConst.myRevBuf = new byte[1200];
                       
                        for (byte j = 1; j <= 12; j++)//12个按键
                        {
                            ArayTmp = new byte[4];
                            ArayTmp[0] = j;
                            ArayTmp[1] = 1;
                            ArayTmp[2] = i;
                            ArayTmp[3] = 12;
                            //读取按键备注
                            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xE004, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                            {
                                byte[] arayRemark = new byte[20];
                                Array.Copy(CsConst.myRevBuf, 26, arayRemark, 0, 20);
                                myScenes[(i * 12) + (j - 1)].Remark = HDLPF.Byte2String(arayRemark);
                                CsConst.myRevBuf = new byte[1200];
                            }
                            else return;
                        }
                        //读取按键调光使能和保存
                        ArayTmp = new byte[3];
                        ArayTmp[0] = 1;
                        ArayTmp[1] = i;
                        ArayTmp[2] = 12;
                        if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x19A2, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                        {
                            for (byte j = 0; j < 12; j++)
                            {
                                byte bytTmp = CsConst.myRevBuf[28 + j];
                                string str = GlobalClass.IntToBit(Convert.ToInt32(bytTmp), 8);
                                if (str.Substring(3, 1) == "1") myScenes[(i * 12) + j].IsDimmer = 1;
                                else myScenes[(i * 12) + j].IsDimmer = 0;
                                if (str.Substring(7, 1) == "1") myScenes[(i * 12) + j].SaveDimmer = 1;
                                else myScenes[(i * 12) + j].SaveDimmer = 0;
                            }
                            CsConst.myRevBuf = new byte[1200];
                        }
                        else return;
                        
                        //读取按键互斥
                        ArayTmp = new byte[3];
                        ArayTmp[0] = 1;
                        ArayTmp[1] = i;
                        ArayTmp[2] = 12;
                        if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x19A6, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                        {
                            for (byte j = 0; j < 12; j++)
                            {
                                myScenes[(i * 12) + j].bytMutex = CsConst.myRevBuf[28 + j];
                            }
                            CsConst.myRevBuf = new byte[1200];
                        }
                        else return;

                        if (CsConst.isRestore)
                        {
                            for (int j = 1; j <= 12; j++)
                            {
                                byte bytTotalCMD = 0;
                                switch (myScenes[i * 12 + j - 1].Mode)
                                {
                                    case 0: bytTotalCMD = 0; break;
                                    case 4:
                                    case 5:
                                    case 7: bytTotalCMD = 99; break;
                                }

                                ArayTmp = new byte[5];
                                ArayTmp[0] = Convert.ToByte(j);
                                myScenes[i * 12 + j - 1].KeyTargets = new List<UVCMD.ControlTargets>();
                                for (byte bytJ = 0; bytJ < bytTotalCMD; bytJ++)
                                {
                                    ArayTmp[1] = Convert.ToByte(bytJ + 1);
                                    ArayTmp[2] = 1;
                                    ArayTmp[3] = Convert.ToByte(i);
                                    ArayTmp[4] = 12;
                                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xE000, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                                    {
                                        UVCMD.ControlTargets oCMD = new UVCMD.ControlTargets();
                                        oCMD.ID = Convert.ToByte(CsConst.myRevBuf[26]);
                                        oCMD.Type = CsConst.myRevBuf[27];  //转换为正确的类型
                                        oCMD.SubnetID = CsConst.myRevBuf[28];
                                        oCMD.DeviceID = CsConst.myRevBuf[29];
                                        oCMD.Param1 = CsConst.myRevBuf[30];
                                        oCMD.Param2 = CsConst.myRevBuf[31];
                                        oCMD.Param3 = CsConst.myRevBuf[32];
                                        oCMD.Param4 = CsConst.myRevBuf[33];
                                        CsConst.myRevBuf = new byte[1200];
                                        myScenes[i * 12 + j - 1].KeyTargets.Add(oCMD);
                                    }
                                    else return;
                                }
                            }
                        }
                    }
                    else return;
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(60 + i);
                }
                #endregion
                MyRead2UpFlags[6] = true;
            }
            if (intActivePage == 0 || intActivePage == 8)
            {
                #region
                mySensor = new List<Sensor>();
                if (DeviceType == 179 || DeviceType==181)
                {
                    for (byte i = 0; i < 7; i++)
                    {
                        byte[] arayTmp = new byte[3];
                        arayTmp[0] = 2;
                        arayTmp[1] = 0;
                        arayTmp[2] = i;
                        if (CsConst.mySends.AddBufToSndList(arayTmp, 0xE4FC, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                        {
                            Sensor temp = new Sensor();
                            temp.ID = i;
                            temp.arayPM = new byte[CsConst.myRevBuf[28] * 5 + 1];
                            temp.arayPM[0] = CsConst.myRevBuf[28];
                            Array.Copy(CsConst.myRevBuf, 29, temp.arayPM, 1, temp.arayPM.Length - 1);
                            mySensor.Add(temp);
                            CsConst.myRevBuf = new byte[1200];
                        }
                        else return;
                    }
                }
                #endregion
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(100);
        }
        /// <summary>
        ///上传信息到彩屏
        /// </summary>
        public virtual void UploadColorDLPInfoToDevice(int DeviceType, string DevName, int intActivePage)
        {
            String strMainRemark = DevName.Split('\\')[1].Trim().Split('(')[0].Trim();
            DevName = DevName.Split('\\')[0].Trim();
            //保存回路信息
            Byte bytSubID = Byte.Parse(DevName.Split('-')[0].ToString());
            Byte bytDevID = Byte.Parse(DevName.Split('-')[1].ToString());

            Byte[] ArayMain = new Byte[20];
            Byte[] arayTmpRemark = new Byte[20];
            if (HDLSysPF.ModifyDeviceMainRemark(bytSubID, bytDevID, strMainRemark, DeviceType) == false)
            {
                CsConst.myRevBuf = new byte[1200];
            }
            else return;
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(5, null);
            if (intActivePage == 0 || intActivePage == 1)
            {
                ArayMain = new byte[4];
                Array.Copy(BroadCastAry, ArayMain, 4);
                if (CsConst.mySends.AddBufToSndList(ArayMain, 0xE0FA, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                {
                    CsConst.myRevBuf = new byte[1200];
                }
                else return;
                #region
                ArayMain = new byte[10];
                ArayMain[0] = BasicInfo[0];
                ArayMain[1] = BasicInfo[6];
                ArayMain[2] = BasicInfo[7];
                ArayMain[3] = BasicInfo[8];
                ArayMain[4] = BasicInfo[1];
                ArayMain[6] = BasicInfo[2];
                ArayMain[8] = BasicInfo[3];
                ArayMain[9] = BasicInfo[9];
                if (CsConst.mySends.AddBufToSndList(ArayMain, 0xE13A, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                {
                    CsConst.myRevBuf = new byte[1200];
                }
                else return;
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(6, null);
                ArayMain = new byte[8];
                ArayMain[1] = BasicInfo[4];
                ArayMain[2] = BasicInfo[15];
                ArayMain[3] = BasicInfo[5];
                ArayMain[4] = BasicInfo[16];
                ArayMain[5] = 1;
                if (CsConst.mySends.AddBufToSndList(ArayMain, 0xE0E2, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                {
                    CsConst.myRevBuf = new byte[1200];
                }
                else return;
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(7, null);
                ArayMain = new byte[1];
                ArayMain[0] = TemperatureType;
                if (CsConst.mySends.AddBufToSndList(ArayMain, 0xE122, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                {
                    CsConst.myRevBuf = new byte[1200];
                }
                else return;
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(8, null);
                ArayMain = new byte[2];
                ArayMain[0] = BasicInfo[13];
                ArayMain[1] = BasicInfo[14];
                if (CsConst.mySends.AddBufToSndList(ArayMain, 0xE12A, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                {
                    CsConst.myRevBuf = new byte[1200];
                }
                else return;

                byte[] arayLCD = new byte[3];
                arayLCD[0] = BasicInfo[2];
                arayLCD[1] = 0;
                arayLCD[2] = 0;
                CsConst.mySends.AddBufToSndList(arayLCD, 0xE012, bytSubID, bytDevID, false, false, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType));
                HDLUDP.TimeBetwnNext(arayLCD.Length);
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(9, null);
                #endregion
            }

            if (intActivePage == 0 || intActivePage == 2)
            {
                #region
                if (CsConst.isRestore)
                {
                    if (MyKeys == null) return;
                    if (MyKeys.Count < 66) return;
                    for (int i = 0; i < 6; i++)
                    {
                        for (int j = 1; j <= 11; j++)
                        {
                            ArayMain = new byte[24];
                            string strRemark = MyKeys[i * 11 + j - 1].Remark;
                            ArayMain[0] = Convert.ToByte(j);
                            arayTmpRemark = HDLUDP.StringToByte(strRemark);
                            if (arayTmpRemark.Length > 20)
                            {
                                Array.Copy(arayTmpRemark, 0, ArayMain, 1, 20);
                            }
                            else
                            {
                                Array.Copy(arayTmpRemark, 0, ArayMain, 1, arayTmpRemark.Length);
                            }
                            ArayMain[21] = 0;
                            ArayMain[22] = Convert.ToByte(i);
                            ArayMain[23] = 12;
                            if (CsConst.mySends.AddBufToSndList(ArayMain, 0xE006, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                            {
                                CsConst.myRevBuf = new byte[1200];
                            }
                            else return;
                        }

                        ArayMain = new byte[15];
                        ArayMain[0] = 0;
                        ArayMain[1] = Convert.ToByte(i);
                        ArayMain[2] = 12;
                        for (int j = 0; j < MyKeys.Count; j++)
                        {
                            if (MyKeys[j].PageID == Convert.ToByte(i))
                            {
                                ArayMain[MyKeys[j].ID + 2] = MyKeys[j].Mode;
                            }
                        }
                        if (CsConst.mySends.AddBufToSndList(ArayMain, 0x19A0, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                        {
                            CsConst.myRevBuf = new byte[1200];
                        }
                        else return;

                        ArayMain = new byte[15];
                        ArayMain[0] = 0;
                        ArayMain[1] = Convert.ToByte(i);
                        ArayMain[2] = 12;
                        for (int j = 0; j < MyKeys.Count; j++)
                        {
                            if (MyKeys[j].PageID == Convert.ToByte(i))
                            {
                                byte dim = 0;
                                byte savedim = 0;
                                ArayMain[MyKeys[j].ID + 2] = Convert.ToByte(((dim << 4) & 0xF0) | (savedim & 0x0F));
                            }
                        }
                        if (CsConst.mySends.AddBufToSndList(ArayMain, 0x19A4, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                        {
                            CsConst.myRevBuf = new byte[1200];
                        }
                        else return;

                        ArayMain = new byte[15];
                        ArayMain[0] = 0;
                        ArayMain[1] = Convert.ToByte(i);
                        ArayMain[2] = 12;
                        for (int j = 0; j < MyKeys.Count; j++)
                        {
                            if (MyKeys[j].PageID == Convert.ToByte(i))
                            {
                                if (10 <= MyKeys[j].ID && MyKeys[j].ID <= 11)
                                {
                                    ArayMain[MyKeys[j].ID + 2] = MyKeys[j].bytMutex;
                                }
                            }
                        }
                        if (CsConst.mySends.AddBufToSndList(ArayMain, 0x19A8, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                        {
                            CsConst.myRevBuf = new byte[1200];
                        }
                        else return;

                        for (int j = 0; j < 11; j++)
                        {
                            if (MyKeys[i * 11 + j].KeyTargets == null) continue;
                            for (int k = 0; k < MyKeys[i * 11 + j].KeyTargets.Count; k++)
                            {
                                if (MyKeys[i * 11 + j].Mode == 1 || MyKeys[i * 11 + j].Mode == 2 ||
                                    MyKeys[i * 11 + j].Mode == 3 || MyKeys[i * 11 + j].Mode == 4 ||
                                    MyKeys[i * 11 + j].Mode == 5 || MyKeys[i * 11 + j].Mode == 7)
                                {
                                    if (MyKeys[i * 11 + j].KeyTargets[k].Type != 0 && MyKeys[i * 11 + j].KeyTargets[k].Type != 255)
                                    {
                                        byte[] arayCMD = new byte[12];
                                        arayCMD[0] = Convert.ToByte(j + 1);
                                        arayCMD[1] = Convert.ToByte(k + 1);
                                        arayCMD[2] = MyKeys[i * 11 + j].KeyTargets[k].Type;
                                        arayCMD[3] = MyKeys[i * 11 + j].KeyTargets[k].SubnetID;
                                        arayCMD[4] = MyKeys[i * 11 + j].KeyTargets[k].DeviceID;
                                        arayCMD[5] = MyKeys[i * 11 + j].KeyTargets[k].Param1;
                                        arayCMD[6] = MyKeys[i * 11 + j].KeyTargets[k].Param2;
                                        arayCMD[7] = MyKeys[i * 11 + j].KeyTargets[k].Param3;   // save targets
                                        arayCMD[8] = MyKeys[i * 11 + j].KeyTargets[k].Param4;
                                        arayCMD[9] = Convert.ToByte(0);
                                        arayCMD[10] = Convert.ToByte(i);
                                        arayCMD[11] = 12;
                                        if (CsConst.mySends.AddBufToSndList(arayCMD, 0xE002, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                                        {
                                            CsConst.myRevBuf = new byte[1200];
                                            HDLUDP.TimeBetwnNext(arayCMD.Length);
                                        }
                                        else return;
                                    }
                                }
                            }
                        }
                        if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(10+i, null);
                    }
                }
                #endregion
            }

            if (intActivePage == 0 || intActivePage == 3)
            {
                #region
                if (CsConst.isRestore)
                {
                    byte[] arayTmp = new byte[1];
                    arayTmp[0] = 2;
                    byte[] arayVisible = new byte[9];
                    if (CsConst.mySends.AddBufToSndList(arayTmp, 0x19B2, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                    {
                        Array.Copy(CsConst.myRevBuf, 26, arayVisible, 0, 9);
                        CsConst.myRevBuf = new byte[1200];
                    }
                    else return;
                    if (MyAC == null) return;
                    if (MyAC.Count < 9) return;
                    for (int i = 0; i < 9; i++)
                    {
                        byte[] ArayTmp = new byte[42];
                        ArayTmp[0] = Convert.ToByte(i);
                        ArayTmp[1] = MyAC[i].Enable;
                        ArayTmp[2] = MyAC[i].DesSubnetID;
                        ArayTmp[3] = MyAC[i].DesDevID;
                        ArayTmp[4] = MyAC[i].ACNum;
                        ArayTmp[5] = MyAC[i].ControlWays;
                        ArayTmp[6] = MyAC[i].OperationEnable;
                        ArayTmp[7] = MyAC[i].PowerOnRestoreState;
                        ArayTmp[11] = MyAC[i].FanEnable;
                        ArayTmp[12] = MyAC[i].FanEnergySaveEnable;
                        for (int j = 0; j < 11; j++)
                            ArayTmp[13 + j] = MyAC[i].FanParam[j];
                        for (int j = 0; j < 8; j++)
                            ArayTmp[24 + j] = MyAC[i].CoolParam[j];
                        for (int j = 0; j < 9; j++)
                            ArayTmp[32 + j] = MyAC[i].OutDoorParam[j];
                        ArayTmp[41] = TemperatureType;

                        if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x19AC, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                        {
                            CsConst.myRevBuf = new byte[1200];
                            if (MyAC[i].Enable == 1 && (arayVisible[i] == Convert.ToByte(i + 1)))
                            {
                                ArayTmp = new byte[10];
                                ArayTmp[0] = Convert.ToByte(i);
                                ArayTmp[1] = MyAC[i].ACSwitch;
                                ArayTmp[2] = MyAC[i].CoolingTemp;
                                ArayTmp[3] = MyAC[i].HeatingTemp;
                                ArayTmp[4] = MyAC[i].AutoTemp;
                                ArayTmp[5] = MyAC[i].DryTemp;
                                ArayTmp[6] = MyAC[i].ACMode;
                                ArayTmp[7] = MyAC[i].ACWind;
                                ArayTmp[8] = MyAC[i].ACFanEnable;
                                ArayTmp[9] = TemperatureType;
                                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x19B0, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                                {
                                    CsConst.myRevBuf = new byte[1200];
                                }
                                else return;
                            }
                        }
                        else return;
                        if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(16 + i, null);
                    }
                }
                #endregion
            }

            if (intActivePage == 0 || intActivePage == 4)
            {
                #region

                if (MyCurtain == null) return;
                if (MyCurtain.Count < 22) return;
                for (int i = 0; i < 6; i++)
                {
                    if (i < 5)
                    {
                        for (int j = 0; j < 4; j++)
                        {
                            byte[] ArayTmp = new byte[24];
                            ArayTmp[0] = Convert.ToByte(j + 1);
                            if (MyCurtain[i * 4 + j].Remark == null) MyCurtain[i * 4 + j].Remark = "";
                            arayTmpRemark = HDLUDP.StringToByte(MyCurtain[i * 4 + j].Remark);
                            if (arayTmpRemark.Length > 20)
                            {
                                Array.Copy(arayTmpRemark, 0, ArayTmp, 1, 20);
                            }
                            else
                            {
                                Array.Copy(arayTmpRemark, 0, ArayTmp, 1, arayTmpRemark.Length);
                            }
                            ArayTmp[21] = 2;
                            ArayTmp[22] = Convert.ToByte(i);
                            ArayTmp[23] = 12;
                            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xE006, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                            {
                                CsConst.myRevBuf = new byte[1200];
                            }
                            else return;

                            ArayTmp = new byte[12];
                            ArayTmp[0] = Convert.ToByte(j+1);
                            ArayTmp[1] = 1;
                            ArayTmp[2] = MyCurtain[i * 4 + j].Mode;
                            ArayTmp[3] = MyCurtain[i * 4 + j].SubnetID;
                            ArayTmp[4] = MyCurtain[i * 4 + j].DeviceID;
                            ArayTmp[5] = MyCurtain[i * 4 + j].CurtainNo;
                            ArayTmp[6] = MyCurtain[i * 4 + j].CurtainSwitch;
                            ArayTmp[9] = 2;
                            ArayTmp[10] = Convert.ToByte(i);
                            ArayTmp[11] = 12;
                            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xE002, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                            {
                                CsConst.myRevBuf = new byte[1200];
                            }
                            else return;
                        }
                        byte[] arayTmp = new byte[15];
                        arayTmp[0] = 2;
                        arayTmp[1] = Convert.ToByte(i);
                        arayTmp[2] = 12;
                        for (int j = 3; j < 7; j++) arayTmp[j] = 1;
                        arayTmp[7] = 4;
                        arayTmp[8] = 5;
                        if (CsConst.mySends.AddBufToSndList(arayTmp, 0x19A0, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                        {
                            CsConst.myRevBuf = new byte[1200];
                        }
                        else return;
                    }
                    else
                    {
                        for (int j = 0; j < 2; j++)
                        {
                            byte[] ArayTmp = new byte[24];
                            ArayTmp[0] = Convert.ToByte(j + 1);
                            if (MyCurtain[i * 4 + j].Remark == null) MyCurtain[i * 4 + j].Remark = "";
                            arayTmpRemark = HDLUDP.StringToByte(MyCurtain[i * 4 + j].Remark);
                            if (arayTmpRemark.Length > 20)
                            {
                                Array.Copy(arayTmpRemark, 0, ArayTmp, 1, 20);
                            }
                            else
                            {
                                Array.Copy(arayTmpRemark, 0, ArayTmp, 1, arayTmpRemark.Length);
                            }
                            ArayTmp[21] = 3;
                            ArayTmp[22] = Convert.ToByte(i);
                            ArayTmp[23] = 12;
                            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xE006, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                            {
                                CsConst.myRevBuf = new byte[1200];
                            }
                            else return;

                            ArayTmp = new byte[12];
                            ArayTmp[0] = Convert.ToByte(j+1);
                            ArayTmp[1] = 1;
                            ArayTmp[2] = MyCurtain[i * 4 + j].Mode;
                            ArayTmp[3] = MyCurtain[i * 4 + j].SubnetID;
                            ArayTmp[4] = MyCurtain[i * 4 + j].DeviceID;
                            ArayTmp[5] = MyCurtain[i * 4 + j].CurtainNo;
                            ArayTmp[6] = MyCurtain[i * 4 + j].CurtainSwitch;
                            ArayTmp[9] = 3;
                            ArayTmp[10] = 0;
                            ArayTmp[11] = 12;
                            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xE002, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                            {
                                CsConst.myRevBuf = new byte[1200];
                            }
                            else return;
                        }
                        byte[] arayTmp = new byte[15];
                        arayTmp[0] = 3;
                        arayTmp[1] = 0;
                        arayTmp[2] = 12;
                        for (int j = 3; j < 5; j++) arayTmp[j] = 1;
                        arayTmp[5] = 4;
                        arayTmp[6] = 5;
                        if (CsConst.mySends.AddBufToSndList(arayTmp, 0x19A0, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                        {
                            CsConst.myRevBuf = new byte[1200];
                        }
                        else return;
                    }
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(26 + i, null);
                }
                #endregion
            }

            if (intActivePage == 0 || intActivePage == 6)
            {
                #region
                if (CsConst.isRestore)
                {
                    if (MyMusic == null) return;
                    if (MyMusic.Count < 9) return;
                    for (int i = 0; i < 9; i++)
                    {
                        MyMusic[i].ModifyMusicSettingInformation(bytSubID, bytDevID, DeviceType);
                        MyMusic[i].ModifyMusicAdvancedCommandsGroup(bytSubID, bytDevID, DeviceType);
                        if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(32+i, null);
                    }
                }
                #endregion
            }

            if (intActivePage == 0 || intActivePage == 7)
            {
                #region
                if (CsConst.isRestore)
                {
                    if (myScenes == null) return;
                    if (myScenes.Count < 24) return;
                    for (int i = 0; i < 2; i++)
                    {
                        for (int j = 1; j <= 12; j++)
                        {
                            ArayMain = new byte[24];
                            string strRemark = myScenes[i * 12 + j - 1].Remark;
                            ArayMain[0] = Convert.ToByte(j);
                            arayTmpRemark = HDLUDP.StringToByte(strRemark);
                            if (arayTmpRemark.Length > 20)
                            {
                                Array.Copy(arayTmpRemark, 0, ArayMain, 1, 20);
                            }
                            else
                            {
                                Array.Copy(arayTmpRemark, 0, ArayMain, 1, arayTmpRemark.Length);
                            }
                            ArayMain[21] = 1;
                            ArayMain[22] = Convert.ToByte(i);
                            ArayMain[23] = 12;
                            if (CsConst.mySends.AddBufToSndList(ArayMain, 0xE006, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                            {
                                CsConst.myRevBuf = new byte[1200];
                            }
                            else return;
                        }

                        ArayMain = new byte[15];
                        ArayMain[0] = 1;
                        ArayMain[1] = Convert.ToByte(i);
                        ArayMain[2] = 12;
                        for (int j = 0; j < myScenes.Count; j++)
                        {
                            if (myScenes[j].PageID == Convert.ToByte(i))
                            {
                                ArayMain[myScenes[j].ID + 2] = myScenes[j].Mode;
                            }
                        }
                        if (CsConst.mySends.AddBufToSndList(ArayMain, 0x19A0, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                        {
                            CsConst.myRevBuf = new byte[1200];
                        }
                        else return;

                        ArayMain = new byte[15];
                        ArayMain[0] = 1;
                        ArayMain[1] = Convert.ToByte(i);
                        ArayMain[2] = 12;
                        for (int j = 0; j < myScenes.Count; j++)
                        {
                            if (myScenes[j].PageID == Convert.ToByte(i))
                            {
                                byte dim = myScenes[j].IsDimmer;
                                byte savedim = myScenes[j].SaveDimmer;
                                ArayMain[myScenes[j].ID + 2] = Convert.ToByte(((dim << 4) & 0xF0) | (savedim & 0x0F));
                            }
                        }
                        if (CsConst.mySends.AddBufToSndList(ArayMain, 0x19A4, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                        {
                            CsConst.myRevBuf = new byte[1200];
                        }
                        else return;

                        ArayMain = new byte[15];
                        ArayMain[0] = 1;
                        ArayMain[1] = Convert.ToByte(i);
                        ArayMain[2] = 12;
                        for (int j = 0; j < myScenes.Count; j++)
                        {
                            if (myScenes[j].PageID == Convert.ToByte(i))
                            {
                                ArayMain[myScenes[j].ID + 2] = myScenes[j].bytMutex;
                            }
                        }
                        if (CsConst.mySends.AddBufToSndList(ArayMain, 0x19A8, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                        {
                            CsConst.myRevBuf = new byte[1200];
                        }
                        else return;

                        for (int j = 0; j < 12; j++)
                        {
                            if (myScenes[i * 12 + j].KeyTargets == null) continue;
                            for (int k = 0; k < myScenes[i * 12 + j].KeyTargets.Count; k++)
                            {
                                if (myScenes[i * 12 + j].Mode == 4 || myScenes[i * 12 + j].Mode == 5 ||
                                    myScenes[i * 12 + j].Mode == 7)
                                {
                                    if (myScenes[i * 12 + j].KeyTargets[k].Type != 0 && myScenes[i * 12 + j].KeyTargets[k].Type != 255)
                                    {
                                        byte[] arayCMD = new byte[12];
                                        arayCMD[0] = Convert.ToByte(j + 1);
                                        arayCMD[1] = Convert.ToByte(k + 1);
                                        arayCMD[2] = myScenes[i * 12 + j].KeyTargets[k].Type;
                                        arayCMD[3] = myScenes[i * 12 + j].KeyTargets[k].SubnetID;
                                        arayCMD[4] = myScenes[i * 12 + j].KeyTargets[k].DeviceID;
                                        arayCMD[5] = myScenes[i * 12 + j].KeyTargets[k].Param1;
                                        arayCMD[6] = myScenes[i * 12 + j].KeyTargets[k].Param2;
                                        arayCMD[7] = myScenes[i * 12 + j].KeyTargets[k].Param3;   // save targets
                                        arayCMD[8] = myScenes[i * 12 + j].KeyTargets[k].Param4;
                                        arayCMD[9] = Convert.ToByte(1);
                                        arayCMD[10] = Convert.ToByte(i);
                                        arayCMD[11] = 12;
                                        if (CsConst.mySends.AddBufToSndList(arayCMD, 0xE002, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                                        {
                                            HDLUDP.TimeBetwnNext(arayCMD.Length);
                                        }
                                        else return;
                                    }
                                }
                            }
                        }
                        if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(40 + i, null);
                    }
                }
                #endregion
            }
            
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(100);
        }

        /// <summary>
        ///上传信息到彩屏
        /// </summary>
        public void UploadColorDLPIconsToDevice(int DeviceType, string DevName, int Type,byte[] arayaddress)
        {
            string strMainRemark = DevName.Split('\\')[1].Trim().Split('(')[0].Trim();
            DevName = DevName.Split('\\')[0].Trim();
            //保存回路信息
            byte bytSubID = byte.Parse(DevName.Split('-')[0].ToString());
            byte bytDevID = byte.Parse(DevName.Split('-')[1].ToString());
            int intMaxPacket = 0;
            int intPacketSize = 64;
            int intSendLength = 67;
            if (Type == 0 || Type == 1)//背景图片
            {
                #region
                for (int i = 0; i < arayPIC.Count; i++)
                {
                    byte[] arayTmp = arayPIC[i].ToArray();
                    if (arayTmp[0] == arayaddress[0] && arayTmp[1] == arayaddress[1] && arayTmp[2] == arayaddress[2])
                    {
                        if ((arayTmp.Length - 3) % intPacketSize != 0) intMaxPacket = Convert.ToInt32((arayTmp.Length - 3) / intPacketSize + 1);
                        else intMaxPacket = Convert.ToInt32((arayTmp.Length - 3) / intPacketSize);
                        for (int j = 0; j < intMaxPacket; j++)
                        {
                            if (CsConst.isStopDealImageBackground) break;
                            int intTmpAddress = arayaddress[0] * 256 * 256 + arayaddress[1] * 256 + arayaddress[2] + j * 64;
                            byte[] ArayFirmware = new byte[67];
                            ArayFirmware[0] = Convert.ToByte((intTmpAddress & 0xFF0000) >> 16);
                            ArayFirmware[1] = Convert.ToByte((intTmpAddress & 0xFF00) >> 8);
                            ArayFirmware[2] = Convert.ToByte((intTmpAddress % 256));
                            if ((j != intMaxPacket - 1) || ((arayTmp.Length - 3) % intPacketSize == 0))
                                Array.Copy(arayTmp, j * intPacketSize + 3, ArayFirmware, 3, intPacketSize);
                            else if ((arayTmp.Length-3) % intPacketSize != 0)
                            {
                                intSendLength = 3 + (arayTmp.Length - 3) % intPacketSize;
                                ArayFirmware = new byte[intSendLength];
        
                                ArayFirmware[0] = Convert.ToByte((intTmpAddress & 0xFF0000) >> 16);
                                ArayFirmware[1] = Convert.ToByte((intTmpAddress & 0xFF00) >> 8);
                                ArayFirmware[2] = Convert.ToByte((intTmpAddress % 256));
                                Array.Copy(arayTmp, j * intPacketSize + 3, ArayFirmware, 3, (arayTmp.Length-3) % intPacketSize);
                            }
                            if (CsConst.mySends.AddBufToSndList(ArayFirmware, 0xE46A, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                            {
                                HDLUDP.TimeBetwnNext(1);
                                CsConst.myRevBuf = new byte[1200];
                            }
                            else return;
                            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(30 * j / intMaxPacket);
                        }
                        break;
                    }
                }
                #endregion
            }
            else if (Type == 0 || Type == 2)//图标
            {
                if (arayaddress[0] == 0)//主界面图标
                {
                    #region
                    for (int intCount = 0; intCount < 11; intCount++)
                    {
                        int StartAddress = 1305600 + 4232 * intCount;
                        arayaddress[0] = Convert.ToByte(StartAddress / 256 / 256 % 256);
                        arayaddress[1] = Convert.ToByte(StartAddress / 256 % 256);
                        arayaddress[2] = Convert.ToByte(StartAddress % 256);
                        for (int i = 0; i < arayPIC.Count; i++)
                        {
                            byte[] arayTmp = arayPIC[i].ToArray();
                            if (arayTmp[0] == arayaddress[0] && arayTmp[1] == arayaddress[1] && arayTmp[2] == arayaddress[2])
                            {
                                if ((arayTmp.Length - 4) % intPacketSize != 0) intMaxPacket = Convert.ToInt32((arayTmp.Length - 4) / intPacketSize + 1);
                                else intMaxPacket = Convert.ToInt32((arayTmp.Length - 4) / intPacketSize);
                                for (int j = 0; j < intMaxPacket; j++)
                                {
                                    if (CsConst.isStopDealImageBackground) break;
                                    int intTmpAddress = arayaddress[0] * 256 * 256 + arayaddress[1] * 256 + arayaddress[2] + j * 64;
                                    byte[] ArayFirmware = new byte[67];
                                    ArayFirmware[0] = Convert.ToByte((intTmpAddress & 0xFF0000) >> 16);
                                    ArayFirmware[1] = Convert.ToByte((intTmpAddress & 0xFF00) >> 8);
                                    ArayFirmware[2] = Convert.ToByte((intTmpAddress % 256));
                                    if ((j != intMaxPacket - 1) || ((arayTmp.Length - 4) % intPacketSize == 0))
                                        Array.Copy(arayTmp, j * intPacketSize + 4, ArayFirmware, 3, intPacketSize);
                                    else if ((arayTmp.Length - 4) % intPacketSize != 0)
                                    {
                                        intSendLength = 3 + (arayTmp.Length - 4) % intPacketSize;
                                        ArayFirmware = new byte[intSendLength];

                                        ArayFirmware[0] = Convert.ToByte((intTmpAddress & 0xFF0000) >> 16);
                                        ArayFirmware[1] = Convert.ToByte((intTmpAddress & 0xFF00) >> 8);
                                        ArayFirmware[2] = Convert.ToByte((intTmpAddress % 256));
                                        Array.Copy(arayTmp, j * intPacketSize + 4, ArayFirmware, 3, (arayTmp.Length - 4) % intPacketSize);
                                    }
                                    if (CsConst.mySends.AddBufToSndList(ArayFirmware, 0xE46A, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                                    {
                                        HDLUDP.TimeBetwnNext(1);
                                        CsConst.myRevBuf = new byte[1200];
                                    }
                                    else return;
                                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(30 * intCount / 11 + 30);
                                }
                                break;
                            }
                        }
                    }
                    #endregion
                }
                else if (arayaddress[0] == 1)//灯光菜单图标
                {
                    #region
                    for (int intCount = 0; intCount < 8; intCount++)
                    {
                        int StartAddress = 1305600 + 4232 * (intCount+11);
                        arayaddress[0] = Convert.ToByte(StartAddress / 256 / 256 % 256);
                        arayaddress[1] = Convert.ToByte(StartAddress / 256 % 256);
                        arayaddress[2] = Convert.ToByte(StartAddress % 256);
                        for (int i = 0; i < arayPIC.Count; i++)
                        {
                            byte[] arayTmp = arayPIC[i].ToArray();
                            if (arayTmp[0] == arayaddress[0] && arayTmp[1] == arayaddress[1] && arayTmp[2] == arayaddress[2])
                            {
                                if ((arayTmp.Length - 4) % intPacketSize != 0) intMaxPacket = Convert.ToInt32((arayTmp.Length - 4) / intPacketSize + 1);
                                else intMaxPacket = Convert.ToInt32((arayTmp.Length - 4) / intPacketSize);
                                for (int j = 0; j < intMaxPacket; j++)
                                {
                                    if (CsConst.isStopDealImageBackground) break;
                                    int intTmpAddress = arayaddress[0] * 256 * 256 + arayaddress[1] * 256 + arayaddress[2] + j * 64;
                                    byte[] ArayFirmware = new byte[67];
                                    ArayFirmware[0] = Convert.ToByte((intTmpAddress & 0xFF0000) >> 16);
                                    ArayFirmware[1] = Convert.ToByte((intTmpAddress & 0xFF00) >> 8);
                                    ArayFirmware[2] = Convert.ToByte((intTmpAddress % 256));
                                    if ((j != intMaxPacket - 1) || ((arayTmp.Length - 4) % intPacketSize == 0))
                                        Array.Copy(arayTmp, j * intPacketSize + 4, ArayFirmware, 3, intPacketSize);
                                    else if ((arayTmp.Length - 4) % intPacketSize != 0)
                                    {
                                        intSendLength = 3 + (arayTmp.Length - 4) % intPacketSize;
                                        ArayFirmware = new byte[intSendLength];

                                        ArayFirmware[0] = Convert.ToByte((intTmpAddress & 0xFF0000) >> 16);
                                        ArayFirmware[1] = Convert.ToByte((intTmpAddress & 0xFF00) >> 8);
                                        ArayFirmware[2] = Convert.ToByte((intTmpAddress % 256));
                                        Array.Copy(arayTmp, j * intPacketSize + 4, ArayFirmware, 3, (arayTmp.Length - 4) % intPacketSize);
                                    }
                                    if (CsConst.mySends.AddBufToSndList(ArayFirmware, 0xE46A, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                                    {
                                        HDLUDP.TimeBetwnNext(1);
                                        CsConst.myRevBuf = new byte[1200];
                                    }
                                    else return;
                                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(30 * intCount / 8 + 30);
                                }
                                break;
                            }
                        }
                    }
                    #endregion
                }
                else if (arayaddress[0] == 2 || arayaddress[0] == 3 || arayaddress[0] == 4||
                         arayaddress[0] == 5 || arayaddress[0] == 6 || arayaddress[0] == 7)//灯光1-6页图标
                {
                    #region
                    byte SelectIndex = arayaddress[0];
                    for (int intCount = 0; intCount < 12; intCount++)
                    {
                        int StartAddress = 1305600 + 4232 * (intCount + 19 + 12 * (SelectIndex - 2));
                        arayaddress[0] = Convert.ToByte(StartAddress / 256 / 256 % 256);
                        arayaddress[1] = Convert.ToByte(StartAddress / 256 % 256);
                        arayaddress[2] = Convert.ToByte(StartAddress % 256);
                        for (int i = 0; i < arayPIC.Count; i++)
                        {
                            byte[] arayTmp = arayPIC[i].ToArray();
                            if (arayTmp[0] == arayaddress[0] && arayTmp[1] == arayaddress[1] && arayTmp[2] == arayaddress[2])
                            {
                                if ((arayTmp.Length - 4) % intPacketSize != 0) intMaxPacket = Convert.ToInt32((arayTmp.Length - 4) / intPacketSize + 1);
                                else intMaxPacket = Convert.ToInt32((arayTmp.Length - 4) / intPacketSize);
                                for (int j = 0; j < intMaxPacket; j++)
                                {
                                    if (CsConst.isStopDealImageBackground) break;
                                    int intTmpAddress = arayaddress[0] * 256 * 256 + arayaddress[1] * 256 + arayaddress[2] + j * 64;
                                    byte[] ArayFirmware = new byte[67];
                                    ArayFirmware[0] = Convert.ToByte((intTmpAddress & 0xFF0000) >> 16);
                                    ArayFirmware[1] = Convert.ToByte((intTmpAddress & 0xFF00) >> 8);
                                    ArayFirmware[2] = Convert.ToByte((intTmpAddress % 256));
                                    if ((j != intMaxPacket - 1) || ((arayTmp.Length - 4) % intPacketSize == 0))
                                        Array.Copy(arayTmp, j * intPacketSize + 4, ArayFirmware, 3, intPacketSize);
                                    else if ((arayTmp.Length - 4) % intPacketSize != 0)
                                    {
                                        intSendLength = 3 + (arayTmp.Length - 4) % intPacketSize;
                                        ArayFirmware = new byte[intSendLength];

                                        ArayFirmware[0] = Convert.ToByte((intTmpAddress & 0xFF0000) >> 16);
                                        ArayFirmware[1] = Convert.ToByte((intTmpAddress & 0xFF00) >> 8);
                                        ArayFirmware[2] = Convert.ToByte((intTmpAddress % 256));
                                        Array.Copy(arayTmp, j * intPacketSize + 4, ArayFirmware, 3, (arayTmp.Length - 4) % intPacketSize);
                                    }
                                    if (CsConst.mySends.AddBufToSndList(ArayFirmware, 0xE46A, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                                    {
                                        HDLUDP.TimeBetwnNext(1);
                                        CsConst.myRevBuf = new byte[1200];
                                    }
                                    else return;
                                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(30 * intCount / 11 + 30);
                                }
                                break;
                            }
                        }
                    }
                    #endregion
                }
                else if (arayaddress[0] == 8)//空调菜单图标
                {
                    #region
                    for (int intCount = 0; intCount < 12; intCount++)
                    {
                        int StartAddress = 1305600 + 4232 * (intCount + 91);
                        arayaddress[0] = Convert.ToByte(StartAddress / 256 / 256 % 256);
                        arayaddress[1] = Convert.ToByte(StartAddress / 256 % 256);
                        arayaddress[2] = Convert.ToByte(StartAddress % 256);
                        for (int i = 0; i < arayPIC.Count; i++)
                        {
                            byte[] arayTmp = arayPIC[i].ToArray();
                            if (arayTmp[0] == arayaddress[0] && arayTmp[1] == arayaddress[1] && arayTmp[2] == arayaddress[2])
                            {
                                if ((arayTmp.Length - 4) % intPacketSize != 0) intMaxPacket = Convert.ToInt32((arayTmp.Length - 4) / intPacketSize + 1);
                                else intMaxPacket = Convert.ToInt32((arayTmp.Length - 4) / intPacketSize);
                                for (int j = 0; j < intMaxPacket; j++)
                                {
                                    if (CsConst.isStopDealImageBackground) break;
                                    int intTmpAddress = arayaddress[0] * 256 * 256 + arayaddress[1] * 256 + arayaddress[2] + j * 64;
                                    byte[] ArayFirmware = new byte[67];
                                    ArayFirmware[0] = Convert.ToByte((intTmpAddress & 0xFF0000) >> 16);
                                    ArayFirmware[1] = Convert.ToByte((intTmpAddress & 0xFF00) >> 8);
                                    ArayFirmware[2] = Convert.ToByte((intTmpAddress % 256));
                                    if ((j != intMaxPacket - 1) || ((arayTmp.Length - 4) % intPacketSize == 0))
                                        Array.Copy(arayTmp, j * intPacketSize + 4, ArayFirmware, 3, intPacketSize);
                                    else if ((arayTmp.Length - 4) % intPacketSize != 0)
                                    {
                                        intSendLength = 3 + (arayTmp.Length - 4) % intPacketSize;
                                        ArayFirmware = new byte[intSendLength];

                                        ArayFirmware[0] = Convert.ToByte((intTmpAddress & 0xFF0000) >> 16);
                                        ArayFirmware[1] = Convert.ToByte((intTmpAddress & 0xFF00) >> 8);
                                        ArayFirmware[2] = Convert.ToByte((intTmpAddress % 256));
                                        Array.Copy(arayTmp, j * intPacketSize + 4, ArayFirmware, 3, (arayTmp.Length - 4) % intPacketSize);
                                    }
                                    if (CsConst.mySends.AddBufToSndList(ArayFirmware, 0xE46A, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                                    {
                                        HDLUDP.TimeBetwnNext(1);
                                        CsConst.myRevBuf = new byte[1200];
                                    }
                                    else return;
                                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(30 * intCount / 11 + 30);
                                }
                                break;
                            }
                        }
                    }
                    #endregion
                }
                else if (arayaddress[0] == 9)//窗帘菜单图标
                {
                    #region
                    for (int intCount = 0; intCount < 8; intCount++)
                    {
                        int StartAddress = 1305600 + 4232 * (intCount + 110);
                        arayaddress[0] = Convert.ToByte(StartAddress / 256 / 256 % 256);
                        arayaddress[1] = Convert.ToByte(StartAddress / 256 % 256);
                        arayaddress[2] = Convert.ToByte(StartAddress % 256);
                        for (int i = 0; i < arayPIC.Count; i++)
                        {
                            byte[] arayTmp = arayPIC[i].ToArray();
                            if (arayTmp[0] == arayaddress[0] && arayTmp[1] == arayaddress[1] && arayTmp[2] == arayaddress[2])
                            {
                                if ((arayTmp.Length - 4) % intPacketSize != 0) intMaxPacket = Convert.ToInt32((arayTmp.Length - 4) / intPacketSize + 1);
                                else intMaxPacket = Convert.ToInt32((arayTmp.Length - 4) / intPacketSize);
                                for (int j = 0; j < intMaxPacket; j++)
                                {
                                    if (CsConst.isStopDealImageBackground) break;
                                    int intTmpAddress = arayaddress[0] * 256 * 256 + arayaddress[1] * 256 + arayaddress[2] + j * 64;
                                    byte[] ArayFirmware = new byte[67];
                                    ArayFirmware[0] = Convert.ToByte((intTmpAddress & 0xFF0000) >> 16);
                                    ArayFirmware[1] = Convert.ToByte((intTmpAddress & 0xFF00) >> 8);
                                    ArayFirmware[2] = Convert.ToByte((intTmpAddress % 256));
                                    if ((j != intMaxPacket - 1) || ((arayTmp.Length - 4) % intPacketSize == 0))
                                        Array.Copy(arayTmp, j * intPacketSize + 4, ArayFirmware, 3, intPacketSize);
                                    else if ((arayTmp.Length - 4) % intPacketSize != 0)
                                    {
                                        intSendLength = 3 + (arayTmp.Length - 4) % intPacketSize;
                                        ArayFirmware = new byte[intSendLength];

                                        ArayFirmware[0] = Convert.ToByte((intTmpAddress & 0xFF0000) >> 16);
                                        ArayFirmware[1] = Convert.ToByte((intTmpAddress & 0xFF00) >> 8);
                                        ArayFirmware[2] = Convert.ToByte((intTmpAddress % 256));
                                        Array.Copy(arayTmp, j * intPacketSize + 4, ArayFirmware, 3, (arayTmp.Length - 4) % intPacketSize);
                                    }
                                    if (CsConst.mySends.AddBufToSndList(ArayFirmware, 0xE46A, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                                    {
                                        HDLUDP.TimeBetwnNext(1);
                                        CsConst.myRevBuf = new byte[1200];
                                    }
                                    else return;
                                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(30 * intCount / 8 + 30);
                                }
                                break;
                            }
                        }
                    }
                    #endregion
                }
                else if (arayaddress[0] == 10)//地热菜单图标
                {
                    #region
                    for (int intCount = 0; intCount < 12; intCount++)
                    {
                        int StartAddress = 1305600 + 4232 * (intCount + 121);
                        arayaddress[0] = Convert.ToByte(StartAddress / 256 / 256 % 256);
                        arayaddress[1] = Convert.ToByte(StartAddress / 256 % 256);
                        arayaddress[2] = Convert.ToByte(StartAddress % 256);
                        for (int i = 0; i < arayPIC.Count; i++)
                        {
                            byte[] arayTmp = arayPIC[i].ToArray();
                            if (arayTmp[0] == arayaddress[0] && arayTmp[1] == arayaddress[1] && arayTmp[2] == arayaddress[2])
                            {
                                if ((arayTmp.Length - 4) % intPacketSize != 0) intMaxPacket = Convert.ToInt32((arayTmp.Length - 4) / intPacketSize + 1);
                                else intMaxPacket = Convert.ToInt32((arayTmp.Length - 4) / intPacketSize);
                                for (int j = 0; j < intMaxPacket; j++)
                                {
                                    if (CsConst.isStopDealImageBackground) break;
                                    int intTmpAddress = arayaddress[0] * 256 * 256 + arayaddress[1] * 256 + arayaddress[2] + j * 64;
                                    byte[] ArayFirmware = new byte[67];
                                    ArayFirmware[0] = Convert.ToByte((intTmpAddress & 0xFF0000) >> 16);
                                    ArayFirmware[1] = Convert.ToByte((intTmpAddress & 0xFF00) >> 8);
                                    ArayFirmware[2] = Convert.ToByte((intTmpAddress % 256));
                                    if ((j != intMaxPacket - 1) || ((arayTmp.Length - 4) % intPacketSize == 0))
                                        Array.Copy(arayTmp, j * intPacketSize + 4, ArayFirmware, 3, intPacketSize);
                                    else if ((arayTmp.Length - 4) % intPacketSize != 0)
                                    {
                                        intSendLength = 3 + (arayTmp.Length - 4) % intPacketSize;
                                        ArayFirmware = new byte[intSendLength];

                                        ArayFirmware[0] = Convert.ToByte((intTmpAddress & 0xFF0000) >> 16);
                                        ArayFirmware[1] = Convert.ToByte((intTmpAddress & 0xFF00) >> 8);
                                        ArayFirmware[2] = Convert.ToByte((intTmpAddress % 256));
                                        Array.Copy(arayTmp, j * intPacketSize + 4, ArayFirmware, 3, (arayTmp.Length - 4) % intPacketSize);
                                    }
                                    if (CsConst.mySends.AddBufToSndList(ArayFirmware, 0xE46A, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                                    {
                                        HDLUDP.TimeBetwnNext(1);
                                        CsConst.myRevBuf = new byte[1200];
                                    }
                                    else return;
                                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(30 * intCount / 11 + 30);
                                }
                                break;
                            }
                        }
                    }
                    #endregion
                }
                else if (arayaddress[0] == 11)//音乐菜单图标
                {
                    #region
                    for (int intCount = 0; intCount < 12; intCount++)
                    {
                        int StartAddress = 1305600 + 4232 * (intCount + 137);
                        arayaddress[0] = Convert.ToByte(StartAddress / 256 / 256 % 256);
                        arayaddress[1] = Convert.ToByte(StartAddress / 256 % 256);
                        arayaddress[2] = Convert.ToByte(StartAddress % 256);
                        for (int i = 0; i < arayPIC.Count; i++)
                        {
                            byte[] arayTmp = arayPIC[i].ToArray();
                            if (arayTmp[0] == arayaddress[0] && arayTmp[1] == arayaddress[1] && arayTmp[2] == arayaddress[2])
                            {
                                if ((arayTmp.Length - 4) % intPacketSize != 0) intMaxPacket = Convert.ToInt32((arayTmp.Length - 4) / intPacketSize + 1);
                                else intMaxPacket = Convert.ToInt32((arayTmp.Length - 4) / intPacketSize);
                                for (int j = 0; j < intMaxPacket; j++)
                                {
                                    if (CsConst.isStopDealImageBackground) break;
                                    int intTmpAddress = arayaddress[0] * 256 * 256 + arayaddress[1] * 256 + arayaddress[2] + j * 64;
                                    byte[] ArayFirmware = new byte[67];
                                    ArayFirmware[0] = Convert.ToByte((intTmpAddress & 0xFF0000) >> 16);
                                    ArayFirmware[1] = Convert.ToByte((intTmpAddress & 0xFF00) >> 8);
                                    ArayFirmware[2] = Convert.ToByte((intTmpAddress % 256));
                                    if ((j != intMaxPacket - 1) || ((arayTmp.Length - 4) % intPacketSize == 0))
                                        Array.Copy(arayTmp, j * intPacketSize + 4, ArayFirmware, 3, intPacketSize);
                                    else if ((arayTmp.Length - 4) % intPacketSize != 0)
                                    {
                                        intSendLength = 3 + (arayTmp.Length - 4) % intPacketSize;
                                        ArayFirmware = new byte[intSendLength];

                                        ArayFirmware[0] = Convert.ToByte((intTmpAddress & 0xFF0000) >> 16);
                                        ArayFirmware[1] = Convert.ToByte((intTmpAddress & 0xFF00) >> 8);
                                        ArayFirmware[2] = Convert.ToByte((intTmpAddress % 256));
                                        Array.Copy(arayTmp, j * intPacketSize + 4, ArayFirmware, 3, (arayTmp.Length - 4) % intPacketSize);
                                    }
                                    if (CsConst.mySends.AddBufToSndList(ArayFirmware, 0xE46A, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                                    {
                                        HDLUDP.TimeBetwnNext(1);
                                        CsConst.myRevBuf = new byte[1200];
                                    }
                                    else return;
                                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(30 * intCount / 11 + 30);
                                }
                                break;
                            }
                        }
                    }
                    #endregion
                }
                else if (arayaddress[0] == 12 || arayaddress[0] == 13)//场景图标
                {
                    #region
                    byte SelectIndex = arayaddress[0];
                    for (int intCount = 0; intCount < 12; intCount++)
                    {
                        int StartAddress = 1305600 + 4232 * (intCount + 153 + 12 * (SelectIndex - 12));
                        arayaddress[0] = Convert.ToByte(StartAddress / 256 / 256 % 256);
                        arayaddress[1] = Convert.ToByte(StartAddress / 256 % 256);
                        arayaddress[2] = Convert.ToByte(StartAddress % 256);
                        for (int i = 0; i < arayPIC.Count; i++)
                        {
                            byte[] arayTmp = arayPIC[i].ToArray();
                            if (arayTmp[0] == arayaddress[0] && arayTmp[1] == arayaddress[1] && arayTmp[2] == arayaddress[2])
                            {
                                if ((arayTmp.Length - 4) % intPacketSize != 0) intMaxPacket = Convert.ToInt32((arayTmp.Length - 4) / intPacketSize + 1);
                                else intMaxPacket = Convert.ToInt32((arayTmp.Length - 4) / intPacketSize);
                                for (int j = 0; j < intMaxPacket; j++)
                                {
                                    if (CsConst.isStopDealImageBackground) break;
                                    int intTmpAddress = arayaddress[0] * 256 * 256 + arayaddress[1] * 256 + arayaddress[2] + j * 64;
                                    byte[] ArayFirmware = new byte[67];
                                    ArayFirmware[0] = Convert.ToByte((intTmpAddress & 0xFF0000) >> 16);
                                    ArayFirmware[1] = Convert.ToByte((intTmpAddress & 0xFF00) >> 8);
                                    ArayFirmware[2] = Convert.ToByte((intTmpAddress % 256));
                                    if ((j != intMaxPacket - 1) || ((arayTmp.Length - 4) % intPacketSize == 0))
                                        Array.Copy(arayTmp, j * intPacketSize + 4, ArayFirmware, 3, intPacketSize);
                                    else if ((arayTmp.Length - 4) % intPacketSize != 0)
                                    {
                                        intSendLength = 3 + (arayTmp.Length - 4) % intPacketSize;
                                        ArayFirmware = new byte[intSendLength];

                                        ArayFirmware[0] = Convert.ToByte((intTmpAddress & 0xFF0000) >> 16);
                                        ArayFirmware[1] = Convert.ToByte((intTmpAddress & 0xFF00) >> 8);
                                        ArayFirmware[2] = Convert.ToByte((intTmpAddress % 256));
                                        Array.Copy(arayTmp, j * intPacketSize + 4, ArayFirmware, 3, (arayTmp.Length - 4) % intPacketSize);
                                    }
                                    if (CsConst.mySends.AddBufToSndList(ArayFirmware, 0xE46A, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                                    {
                                        HDLUDP.TimeBetwnNext(1);
                                        CsConst.myRevBuf = new byte[1200];
                                    }
                                    else return;
                                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(30 * intCount / 12 + 30);
                                }
                                break;
                            }
                        }
                    }
                    #endregion
                }
            }
            else if (Type == 0 || Type == 3)//文字
            {
                if (arayaddress[0] == 0)//主界文字
                {
                    #region
                    for (int intCount = 0; intCount < 11; intCount++)
                    {
                        int StartAddress = 3075016 + 3840 * intCount;
                        arayaddress[0] = Convert.ToByte(StartAddress / 256 / 256 % 256);
                        arayaddress[1] = Convert.ToByte(StartAddress / 256 % 256);
                        arayaddress[2] = Convert.ToByte(StartAddress % 256);
                        for (int i = 0; i < arayPIC.Count; i++)
                        {
                            byte[] arayTmp = arayPIC[i].ToArray();
                            if (arayTmp[0] == arayaddress[0] && arayTmp[1] == arayaddress[1] && arayTmp[2] == arayaddress[2])
                            {
                                if ((arayTmp.Length - 4) % intPacketSize != 0) intMaxPacket = Convert.ToInt32((arayTmp.Length - 4) / intPacketSize + 1);
                                else intMaxPacket = Convert.ToInt32((arayTmp.Length - 4) / intPacketSize);
                                for (int j = 0; j < intMaxPacket; j++)
                                {
                                    if (CsConst.isStopDealImageBackground) break;
                                    int intTmpAddress = arayaddress[0] * 256 * 256 + arayaddress[1] * 256 + arayaddress[2] + j * 64;
                                    byte[] ArayFirmware = new byte[67];
                                    ArayFirmware[0] = Convert.ToByte((intTmpAddress & 0xFF0000) >> 16);
                                    ArayFirmware[1] = Convert.ToByte((intTmpAddress & 0xFF00) >> 8);
                                    ArayFirmware[2] = Convert.ToByte((intTmpAddress % 256));
                                    if ((j != intMaxPacket - 1) || ((arayTmp.Length - 4) % intPacketSize == 0))
                                        Array.Copy(arayTmp, j * intPacketSize + 4, ArayFirmware, 3, intPacketSize);
                                    else if ((arayTmp.Length - 4) % intPacketSize != 0)
                                    {
                                        intSendLength = 3 + (arayTmp.Length - 4) % intPacketSize;
                                        ArayFirmware = new byte[intSendLength];

                                        ArayFirmware[0] = Convert.ToByte((intTmpAddress & 0xFF0000) >> 16);
                                        ArayFirmware[1] = Convert.ToByte((intTmpAddress & 0xFF00) >> 8);
                                        ArayFirmware[2] = Convert.ToByte((intTmpAddress % 256));
                                        Array.Copy(arayTmp, j * intPacketSize + 4, ArayFirmware, 3, (arayTmp.Length - 4) % intPacketSize);
                                    }
                                    if (CsConst.mySends.AddBufToSndList(ArayFirmware, 0xE46A, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                                    {
                                        HDLUDP.TimeBetwnNext(1);
                                        CsConst.myRevBuf = new byte[1200];
                                    }
                                    else return;
                                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(30 * intCount / 11 + 30);
                                }
                                break;
                            }
                        }
                    }
                    #endregion
                }
                else if (arayaddress[0] == 1)//灯光菜单文字
                {
                    #region
                    for (int intCount = 0; intCount < 8; intCount++)
                    {
                        int StartAddress = 3075016 + 3840 * (12 + intCount); 
                        arayaddress[0] = Convert.ToByte(StartAddress / 256 / 256 % 256);
                        arayaddress[1] = Convert.ToByte(StartAddress / 256 % 256);
                        arayaddress[2] = Convert.ToByte(StartAddress % 256);
                        for (int i = 0; i < arayPIC.Count; i++)
                        {
                            byte[] arayTmp = arayPIC[i].ToArray();
                            if (arayTmp[0] == arayaddress[0] && arayTmp[1] == arayaddress[1] && arayTmp[2] == arayaddress[2])
                            {
                                if ((arayTmp.Length - 4) % intPacketSize != 0) intMaxPacket = Convert.ToInt32((arayTmp.Length - 4) / intPacketSize + 1);
                                else intMaxPacket = Convert.ToInt32((arayTmp.Length - 4) / intPacketSize);
                                for (int j = 0; j < intMaxPacket; j++)
                                {
                                    if (CsConst.isStopDealImageBackground) break;
                                    int intTmpAddress = arayaddress[0] * 256 * 256 + arayaddress[1] * 256 + arayaddress[2] + j * 64;
                                    byte[] ArayFirmware = new byte[67];
                                    ArayFirmware[0] = Convert.ToByte((intTmpAddress & 0xFF0000) >> 16);
                                    ArayFirmware[1] = Convert.ToByte((intTmpAddress & 0xFF00) >> 8);
                                    ArayFirmware[2] = Convert.ToByte((intTmpAddress % 256));
                                    if ((j != intMaxPacket - 1) || ((arayTmp.Length - 4) % intPacketSize == 0))
                                        Array.Copy(arayTmp, j * intPacketSize + 4, ArayFirmware, 3, intPacketSize);
                                    else if ((arayTmp.Length - 4) % intPacketSize != 0)
                                    {
                                        intSendLength = 3 + (arayTmp.Length - 4) % intPacketSize;
                                        ArayFirmware = new byte[intSendLength];

                                        ArayFirmware[0] = Convert.ToByte((intTmpAddress & 0xFF0000) >> 16);
                                        ArayFirmware[1] = Convert.ToByte((intTmpAddress & 0xFF00) >> 8);
                                        ArayFirmware[2] = Convert.ToByte((intTmpAddress % 256));
                                        Array.Copy(arayTmp, j * intPacketSize + 4, ArayFirmware, 3, (arayTmp.Length - 4) % intPacketSize);
                                    }
                                    if (CsConst.mySends.AddBufToSndList(ArayFirmware, 0xE46A, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                                    {
                                        HDLUDP.TimeBetwnNext(1);
                                        CsConst.myRevBuf = new byte[1200];
                                    }
                                    else return;
                                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(30 * intCount / 8 + 30);
                                }
                                break;
                            }
                        }
                    }
                    #endregion
                }
                else if (arayaddress[0] == 2 || arayaddress[0] == 3 || arayaddress[0] == 4 ||
                         arayaddress[0] == 5 || arayaddress[0] == 6 || arayaddress[0] == 7)//灯光1-6页图标
                {
                    #region
                    byte SelectIndex = arayaddress[0];
                    for (int intCount = 0; intCount < 12; intCount++)
                    {
                        int StartAddress = 3075016 + 3840 * (intCount + 20 + 12 * (SelectIndex - 2));
                        arayaddress[0] = Convert.ToByte(StartAddress / 256 / 256 % 256);
                        arayaddress[1] = Convert.ToByte(StartAddress / 256 % 256);
                        arayaddress[2] = Convert.ToByte(StartAddress % 256);
                        for (int i = 0; i < arayPIC.Count; i++)
                        {
                            byte[] arayTmp = arayPIC[i].ToArray();
                            if (arayTmp[0] == arayaddress[0] && arayTmp[1] == arayaddress[1] && arayTmp[2] == arayaddress[2])
                            {
                                if ((arayTmp.Length - 4) % intPacketSize != 0) intMaxPacket = Convert.ToInt32((arayTmp.Length - 4) / intPacketSize + 1);
                                else intMaxPacket = Convert.ToInt32((arayTmp.Length - 4) / intPacketSize);
                                for (int j = 0; j < intMaxPacket; j++)
                                {
                                    if (CsConst.isStopDealImageBackground) break;
                                    int intTmpAddress = arayaddress[0] * 256 * 256 + arayaddress[1] * 256 + arayaddress[2] + j * 64;
                                    byte[] ArayFirmware = new byte[67];
                                    ArayFirmware[0] = Convert.ToByte((intTmpAddress & 0xFF0000) >> 16);
                                    ArayFirmware[1] = Convert.ToByte((intTmpAddress & 0xFF00) >> 8);
                                    ArayFirmware[2] = Convert.ToByte((intTmpAddress % 256));
                                    if ((j != intMaxPacket - 1) || ((arayTmp.Length - 4) % intPacketSize == 0))
                                        Array.Copy(arayTmp, j * intPacketSize + 4, ArayFirmware, 3, intPacketSize);
                                    else if ((arayTmp.Length - 4) % intPacketSize != 0)
                                    {
                                        intSendLength = 3 + (arayTmp.Length - 4) % intPacketSize;
                                        ArayFirmware = new byte[intSendLength];

                                        ArayFirmware[0] = Convert.ToByte((intTmpAddress & 0xFF0000) >> 16);
                                        ArayFirmware[1] = Convert.ToByte((intTmpAddress & 0xFF00) >> 8);
                                        ArayFirmware[2] = Convert.ToByte((intTmpAddress % 256));
                                        Array.Copy(arayTmp, j * intPacketSize + 4, ArayFirmware, 3, (arayTmp.Length - 4) % intPacketSize);
                                    }
                                    if (CsConst.mySends.AddBufToSndList(ArayFirmware, 0xE46A, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                                    {
                                        HDLUDP.TimeBetwnNext(1);
                                        CsConst.myRevBuf = new byte[1200];
                                    }
                                    else return;
                                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(30 * intCount / 11 + 30);
                                }
                                break;
                            }
                        }
                    }
                    #endregion
                }
                else if (arayaddress[0] == 8)//空调菜单图标
                {
                    #region
                    for (int intCount = 0; intCount < 12; intCount++)
                    {
                        int StartAddress = 3075016 + 3840 * (intCount + 92);
                        arayaddress[0] = Convert.ToByte(StartAddress / 256 / 256 % 256);
                        arayaddress[1] = Convert.ToByte(StartAddress / 256 % 256);
                        arayaddress[2] = Convert.ToByte(StartAddress % 256);
                        for (int i = 0; i < arayPIC.Count; i++)
                        {
                            byte[] arayTmp = arayPIC[i].ToArray();
                            if (arayTmp[0] == arayaddress[0] && arayTmp[1] == arayaddress[1] && arayTmp[2] == arayaddress[2])
                            {
                                if ((arayTmp.Length - 4) % intPacketSize != 0) intMaxPacket = Convert.ToInt32((arayTmp.Length - 4) / intPacketSize + 1);
                                else intMaxPacket = Convert.ToInt32((arayTmp.Length - 4) / intPacketSize);
                                for (int j = 0; j < intMaxPacket; j++)
                                {
                                    if (CsConst.isStopDealImageBackground) break;
                                    int intTmpAddress = arayaddress[0] * 256 * 256 + arayaddress[1] * 256 + arayaddress[2] + j * 64;
                                    byte[] ArayFirmware = new byte[67];
                                    ArayFirmware[0] = Convert.ToByte((intTmpAddress & 0xFF0000) >> 16);
                                    ArayFirmware[1] = Convert.ToByte((intTmpAddress & 0xFF00) >> 8);
                                    ArayFirmware[2] = Convert.ToByte((intTmpAddress % 256));
                                    if ((j != intMaxPacket - 1) || ((arayTmp.Length - 4) % intPacketSize == 0))
                                        Array.Copy(arayTmp, j * intPacketSize + 4, ArayFirmware, 3, intPacketSize);
                                    else if ((arayTmp.Length - 4) % intPacketSize != 0)
                                    {
                                        intSendLength = 3 + (arayTmp.Length - 4) % intPacketSize;
                                        ArayFirmware = new byte[intSendLength];

                                        ArayFirmware[0] = Convert.ToByte((intTmpAddress & 0xFF0000) >> 16);
                                        ArayFirmware[1] = Convert.ToByte((intTmpAddress & 0xFF00) >> 8);
                                        ArayFirmware[2] = Convert.ToByte((intTmpAddress % 256));
                                        Array.Copy(arayTmp, j * intPacketSize + 4, ArayFirmware, 3, (arayTmp.Length - 4) % intPacketSize);
                                    }
                                    if (CsConst.mySends.AddBufToSndList(ArayFirmware, 0xE46A, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                                    {
                                        HDLUDP.TimeBetwnNext(1);
                                        CsConst.myRevBuf = new byte[1200];
                                    }
                                    else return;
                                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(30 * intCount / 11 + 30);
                                }
                                break;
                            }
                        }
                    }
                    #endregion
                }
                else if (arayaddress[0] == 9)//窗帘菜单图标
                {
                    #region
                    for (int intCount = 0; intCount < 8; intCount++)
                    {
                        int StartAddress = 3075016 + 3840 * (intCount + 107);
                        arayaddress[0] = Convert.ToByte(StartAddress / 256 / 256 % 256);
                        arayaddress[1] = Convert.ToByte(StartAddress / 256 % 256);
                        arayaddress[2] = Convert.ToByte(StartAddress % 256);
                        for (int i = 0; i < arayPIC.Count; i++)
                        {
                            byte[] arayTmp = arayPIC[i].ToArray();
                            if (arayTmp[0] == arayaddress[0] && arayTmp[1] == arayaddress[1] && arayTmp[2] == arayaddress[2])
                            {
                                if ((arayTmp.Length - 4) % intPacketSize != 0) intMaxPacket = Convert.ToInt32((arayTmp.Length - 4) / intPacketSize + 1);
                                else intMaxPacket = Convert.ToInt32((arayTmp.Length - 4) / intPacketSize);
                                for (int j = 0; j < intMaxPacket; j++)
                                {
                                    if (CsConst.isStopDealImageBackground) break;
                                    int intTmpAddress = arayaddress[0] * 256 * 256 + arayaddress[1] * 256 + arayaddress[2] + j * 64;
                                    byte[] ArayFirmware = new byte[67];
                                    ArayFirmware[0] = Convert.ToByte((intTmpAddress & 0xFF0000) >> 16);
                                    ArayFirmware[1] = Convert.ToByte((intTmpAddress & 0xFF00) >> 8);
                                    ArayFirmware[2] = Convert.ToByte((intTmpAddress % 256));
                                    if ((j != intMaxPacket - 1) || ((arayTmp.Length - 4) % intPacketSize == 0))
                                        Array.Copy(arayTmp, j * intPacketSize + 4, ArayFirmware, 3, intPacketSize);
                                    else if ((arayTmp.Length - 4) % intPacketSize != 0)
                                    {
                                        intSendLength = 3 + (arayTmp.Length - 4) % intPacketSize;
                                        ArayFirmware = new byte[intSendLength];

                                        ArayFirmware[0] = Convert.ToByte((intTmpAddress & 0xFF0000) >> 16);
                                        ArayFirmware[1] = Convert.ToByte((intTmpAddress & 0xFF00) >> 8);
                                        ArayFirmware[2] = Convert.ToByte((intTmpAddress % 256));
                                        Array.Copy(arayTmp, j * intPacketSize + 4, ArayFirmware, 3, (arayTmp.Length - 4) % intPacketSize);
                                    }
                                    if (CsConst.mySends.AddBufToSndList(ArayFirmware, 0xE46A, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                                    {
                                        HDLUDP.TimeBetwnNext(1);
                                        CsConst.myRevBuf = new byte[1200];
                                    }
                                    else return;
                                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(30 * intCount / 8 + 30);
                                }
                                break;
                            }
                        }
                    }
                    #endregion
                }
                else if (arayaddress[0] == 10)//地热菜单图标
                {
                    #region
                    for (int intCount = 0; intCount < 12; intCount++)
                    {
                        int StartAddress = 3075016 + 3840 * (intCount + 137);
                        arayaddress[0] = Convert.ToByte(StartAddress / 256 / 256 % 256);
                        arayaddress[1] = Convert.ToByte(StartAddress / 256 % 256);
                        arayaddress[2] = Convert.ToByte(StartAddress % 256);
                        for (int i = 0; i < arayPIC.Count; i++)
                        {
                            byte[] arayTmp = arayPIC[i].ToArray();
                            if (arayTmp[0] == arayaddress[0] && arayTmp[1] == arayaddress[1] && arayTmp[2] == arayaddress[2])
                            {
                                if ((arayTmp.Length - 4) % intPacketSize != 0) intMaxPacket = Convert.ToInt32((arayTmp.Length - 4) / intPacketSize + 1);
                                else intMaxPacket = Convert.ToInt32((arayTmp.Length - 4) / intPacketSize);
                                for (int j = 0; j < intMaxPacket; j++)
                                {
                                    if (CsConst.isStopDealImageBackground) break;
                                    int intTmpAddress = arayaddress[0] * 256 * 256 + arayaddress[1] * 256 + arayaddress[2] + j * 64;
                                    byte[] ArayFirmware = new byte[67];
                                    ArayFirmware[0] = Convert.ToByte((intTmpAddress & 0xFF0000) >> 16);
                                    ArayFirmware[1] = Convert.ToByte((intTmpAddress & 0xFF00) >> 8);
                                    ArayFirmware[2] = Convert.ToByte((intTmpAddress % 256));
                                    if ((j != intMaxPacket - 1) || ((arayTmp.Length - 4) % intPacketSize == 0))
                                        Array.Copy(arayTmp, j * intPacketSize + 4, ArayFirmware, 3, intPacketSize);
                                    else if ((arayTmp.Length - 4) % intPacketSize != 0)
                                    {
                                        intSendLength = 3 + (arayTmp.Length - 4) % intPacketSize;
                                        ArayFirmware = new byte[intSendLength];

                                        ArayFirmware[0] = Convert.ToByte((intTmpAddress & 0xFF0000) >> 16);
                                        ArayFirmware[1] = Convert.ToByte((intTmpAddress & 0xFF00) >> 8);
                                        ArayFirmware[2] = Convert.ToByte((intTmpAddress % 256));
                                        Array.Copy(arayTmp, j * intPacketSize + 4, ArayFirmware, 3, (arayTmp.Length - 4) % intPacketSize);
                                    }
                                    if (CsConst.mySends.AddBufToSndList(ArayFirmware, 0xE46A, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                                    {
                                        HDLUDP.TimeBetwnNext(1);
                                        CsConst.myRevBuf = new byte[1200];
                                    }
                                    else return;
                                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(30 * intCount / 11 + 30);
                                }
                                break;
                            }
                        }
                    }
                    #endregion
                }
                else if (arayaddress[0] == 11)//音乐菜单图标
                {
                    #region
                    for (int intCount = 0; intCount < 12; intCount++)
                    {
                        int StartAddress = 3075016 + 3840 * (intCount + 148);
                        arayaddress[0] = Convert.ToByte(StartAddress / 256 / 256 % 256);
                        arayaddress[1] = Convert.ToByte(StartAddress / 256 % 256);
                        arayaddress[2] = Convert.ToByte(StartAddress % 256);
                        for (int i = 0; i < arayPIC.Count; i++)
                        {
                            byte[] arayTmp = arayPIC[i].ToArray();
                            if (arayTmp[0] == arayaddress[0] && arayTmp[1] == arayaddress[1] && arayTmp[2] == arayaddress[2])
                            {
                                if ((arayTmp.Length - 4) % intPacketSize != 0) intMaxPacket = Convert.ToInt32((arayTmp.Length - 4) / intPacketSize + 1);
                                else intMaxPacket = Convert.ToInt32((arayTmp.Length - 4) / intPacketSize);
                                for (int j = 0; j < intMaxPacket; j++)
                                {
                                    if (CsConst.isStopDealImageBackground) break;
                                    int intTmpAddress = arayaddress[0] * 256 * 256 + arayaddress[1] * 256 + arayaddress[2] + j * 64;
                                    byte[] ArayFirmware = new byte[67];
                                    ArayFirmware[0] = Convert.ToByte((intTmpAddress & 0xFF0000) >> 16);
                                    ArayFirmware[1] = Convert.ToByte((intTmpAddress & 0xFF00) >> 8);
                                    ArayFirmware[2] = Convert.ToByte((intTmpAddress % 256));
                                    if ((j != intMaxPacket - 1) || ((arayTmp.Length - 4) % intPacketSize == 0))
                                        Array.Copy(arayTmp, j * intPacketSize + 4, ArayFirmware, 3, intPacketSize);
                                    else if ((arayTmp.Length - 4) % intPacketSize != 0)
                                    {
                                        intSendLength = 3 + (arayTmp.Length - 4) % intPacketSize;
                                        ArayFirmware = new byte[intSendLength];

                                        ArayFirmware[0] = Convert.ToByte((intTmpAddress & 0xFF0000) >> 16);
                                        ArayFirmware[1] = Convert.ToByte((intTmpAddress & 0xFF00) >> 8);
                                        ArayFirmware[2] = Convert.ToByte((intTmpAddress % 256));
                                        Array.Copy(arayTmp, j * intPacketSize + 4, ArayFirmware, 3, (arayTmp.Length - 4) % intPacketSize);
                                    }
                                    if (CsConst.mySends.AddBufToSndList(ArayFirmware, 0xE46A, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                                    {
                                        HDLUDP.TimeBetwnNext(1);
                                        CsConst.myRevBuf = new byte[1200];
                                    }
                                    else return;
                                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(30 * intCount / 11 + 30);
                                }
                                break;
                            }
                        }
                    }
                    #endregion
                }
                else if (arayaddress[0] == 12 || arayaddress[0] == 13)//场景图标
                {
                    #region
                    byte SelectIndex = arayaddress[0];
                    for (int intCount = 0; intCount < 12; intCount++)
                    {
                        int StartAddress = 3075016 + 3840 * (intCount + 159 + 12 * (SelectIndex - 12));
                        arayaddress[0] = Convert.ToByte(StartAddress / 256 / 256 % 256);
                        arayaddress[1] = Convert.ToByte(StartAddress / 256 % 256);
                        arayaddress[2] = Convert.ToByte(StartAddress % 256);
                        for (int i = 0; i < arayPIC.Count; i++)
                        {
                            byte[] arayTmp = arayPIC[i].ToArray();
                            if (arayTmp[0] == arayaddress[0] && arayTmp[1] == arayaddress[1] && arayTmp[2] == arayaddress[2])
                            {
                                if ((arayTmp.Length - 4) % intPacketSize != 0) intMaxPacket = Convert.ToInt32((arayTmp.Length - 4) / intPacketSize + 1);
                                else intMaxPacket = Convert.ToInt32((arayTmp.Length - 4) / intPacketSize);
                                for (int j = 0; j < intMaxPacket; j++)
                                {
                                    if (CsConst.isStopDealImageBackground) break;
                                    int intTmpAddress = arayaddress[0] * 256 * 256 + arayaddress[1] * 256 + arayaddress[2] + j * 64;
                                    byte[] ArayFirmware = new byte[67];
                                    ArayFirmware[0] = Convert.ToByte((intTmpAddress & 0xFF0000) >> 16);
                                    ArayFirmware[1] = Convert.ToByte((intTmpAddress & 0xFF00) >> 8);
                                    ArayFirmware[2] = Convert.ToByte((intTmpAddress % 256));
                                    if ((j != intMaxPacket - 1) || ((arayTmp.Length - 4) % intPacketSize == 0))
                                        Array.Copy(arayTmp, j * intPacketSize + 4, ArayFirmware, 3, intPacketSize);
                                    else if ((arayTmp.Length - 4) % intPacketSize != 0)
                                    {
                                        intSendLength = 3 + (arayTmp.Length - 4) % intPacketSize;
                                        ArayFirmware = new byte[intSendLength];

                                        ArayFirmware[0] = Convert.ToByte((intTmpAddress & 0xFF0000) >> 16);
                                        ArayFirmware[1] = Convert.ToByte((intTmpAddress & 0xFF00) >> 8);
                                        ArayFirmware[2] = Convert.ToByte((intTmpAddress % 256));
                                        Array.Copy(arayTmp, j * intPacketSize + 4, ArayFirmware, 3, (arayTmp.Length - 4) % intPacketSize);
                                    }
                                    if (CsConst.mySends.AddBufToSndList(ArayFirmware, 0xE46A, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                                    {
                                        HDLUDP.TimeBetwnNext(1);
                                        CsConst.myRevBuf = new byte[1200];
                                    }
                                    else return;
                                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(30 * intCount / 12 + 30);
                                }
                                break;
                            }
                        }
                    }
                    #endregion
                }
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(100);
        }
    }
}
