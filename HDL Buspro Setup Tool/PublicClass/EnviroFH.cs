using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HDL_Buspro_Setup_Tool
{
    public class EnviroFH
    {
        public Byte IconID; // 图标ID
        public String Remark; // 备注

            public byte ID;//地热页号
            public byte HeatEnable;//0关，1开
            public byte SourceTemp;//地热温度源（0，本身，1外界，2两者平均）
            public byte[] OutDoorParam = new byte[20];//外温源1使能，子网ID,设备ID，通道，外温源2使能，子网ID,设备ID，通道
            public byte[] SourceParam = new byte[20];//最高温度源使能，子网ID,设备ID，通道，室外温度使能，子网ID，设备ID，通道
            public byte PIDEnable;//0非PID，1PID
            public byte OutputType;//输出信号(0继电器，1PWM Value)
            public byte minPWM;//最小PWM
            public byte maxPWM;//最大PWM
            public byte Speed;//速度lower,low,defaul,high,higher
            public byte Cycle;//1,2,3,5,7,10,15,20;单位分钟
            public byte[] ModeAry = new byte[5];//0普通，1白天，2夜晚，3离开，4时间，bit位(0关，1开)
            public byte Switch;//0关，1开
            public byte[] TimeAry = new byte[4];//白天开始时间1,2，晚上开始时间3,4
            public byte ProtectTemperature;//地热最高温度
            public byte ControlMode;//控制模式
            public byte HeatType;//0地热，1地冷，2自动
            public byte[] SysEnable = new byte[4];//同步发送，同步接收开关，同步接收模式，同步接收温度
            public byte CompenValue;//自动模式触发温度偏移值
            public byte WorkingSwitch;//工作开关
            public byte DesSubnetID;//目标子网ID
            public byte DesDeviceID;//目标设备ID
            public byte Channel;//通道
            public byte minTemp;//最低温度
            public byte maxTemp;//最高温度
            public byte CurrentTemp;//当前温度
            public byte WorkingTempMode;//工作温度模式(1普通，2白天，3夜晚，4离开，5时间)
            public byte[] ModeTemp = new byte[4];//普通温度，白天，夜晚温度，离开温度
            public UVCMD.ControlTargets[] AdvancedCommands;//开关附加目标
            public byte[] CalculationModeTargets = new byte[40];//控制与运算8个目标信息

        public EnviroFH()
        {
            OutDoorParam = new Byte[20];
            SourceParam = new Byte[20];
            ModeAry = new Byte[] { 1,1,1,1,1 };

            TimeAry = new Byte[4];
            SysEnable = new Byte[20];
            ModeTemp = new Byte[4];
        }

        public Boolean UploadFloorheatingSettingToDevice(Byte SubNetID, Byte DeviceID, int DeviceType, Byte TemperatureType)
        {
            Boolean blnIsSuccess = true;
            #region
            byte[] arayTmp = new byte[44];
            arayTmp[0] = HeatEnable;
            arayTmp[1] = SourceTemp;
            if (OutDoorParam == null) OutDoorParam = new byte[8];
            Array.Copy(OutDoorParam, 0, arayTmp, 2, 8);
            if (SourceParam == null) SourceParam = new byte[8];
            Array.Copy(SourceParam, 10, arayTmp, 10, 8);
            arayTmp[18] = PIDEnable;
            arayTmp[19] = OutputType;
            arayTmp[20] = minPWM;
            arayTmp[21] = maxPWM;
            arayTmp[22] = Speed;
            arayTmp[23] = Cycle;
            string str1 = "000";
            string str2 = "0";
            string str3 = "0";
            string str4 = "0";
            string str5 = "0";
            string str6 = "0";
            if (ModeAry == null) ModeAry = new byte[5];
            if (ModeAry[0] == 1) str2 = "1";
            if (ModeAry[1] == 1) str3 = "1";
            if (ModeAry[2] == 1) str4 = "1";
            if (ModeAry[3] == 1) str5 = "1";
            if (ModeAry[4] == 1) str6 = "1";
            string str = str1 + str2 + str3 + str4 + str5 + str6;
            arayTmp[24] = Convert.ToByte(GlobalClass.BitToInt(str));
            arayTmp[25] = Switch;
            if (TimeAry == null) TimeAry = new byte[4];
            Array.Copy(TimeAry, 0, arayTmp, 26, 4);
            arayTmp[30] = ProtectTemperature;
            arayTmp[32] = ControlMode;
            arayTmp[33] = HeatType; 
            if (SysEnable == null) SysEnable = new byte[4];
            arayTmp[34] = SysEnable[0];
            str1 = "00000";
            str2 = "0";
            str3 = "0";
            str4 = "0";
            if (SysEnable[1] == 1) str2 = "1";
            if (SysEnable[2] == 1) str3 = "1";
            if (SysEnable[3] == 1) str4 = "1";
            str = str1 + str2 + str3 + str4;
            arayTmp[35] = Convert.ToByte(GlobalClass.BitToInt(str));
            arayTmp[36] = CompenValue;
            arayTmp[37] = DesSubnetID;
            arayTmp[38] =  DesDeviceID;
            arayTmp[39] = Channel;
            arayTmp[40] = minTemp;
            arayTmp[41] = maxTemp;
            arayTmp[42] = TemperatureType;
            arayTmp[43] = Convert.ToByte(ID);
            if (CsConst.mySends.AddBufToSndList(arayTmp, 0x1942, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
            {
                CsConst.myRevBuf = new byte[1200];
            }
            else return false;
            HDLUDP.TimeBetwnNext(arayTmp.Length);
            #endregion
            return blnIsSuccess;
        }

        public Boolean DownloadFloorheatingsettingFromDevice(Byte SubNetID, Byte DeviceID, int DeviceType)
        {
            Byte[] ArayTmp = new byte[1]{(Byte)ID};
            try
            {
                // floor heating 
                #region
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1940, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                {
                    AdvancedCommands = new UVCMD.ControlTargets[8];
                    HeatEnable = CsConst.myRevBuf[25];
                    SourceTemp = CsConst.myRevBuf[26];
                    OutDoorParam = new byte[20];
                    Array.Copy(CsConst.myRevBuf, 27, OutDoorParam, 0, 8);
                    SourceParam = new byte[20];
                    Array.Copy(CsConst.myRevBuf, 35, SourceParam, 0, 8);
                    PIDEnable = CsConst.myRevBuf[43];
                    OutputType = CsConst.myRevBuf[44];
                    minPWM = CsConst.myRevBuf[45];
                    maxPWM = CsConst.myRevBuf[46];
                    Speed = CsConst.myRevBuf[47];
                    Cycle = CsConst.myRevBuf[48];
                    ModeAry = new byte[] { 0, 0, 0, 0, 0 };
                    string strTmp = GlobalClass.IntToBit(Convert.ToInt32(CsConst.myRevBuf[49]), 8);
                    if (strTmp.Substring(7, 1) == "1") ModeAry[0] = 1;
                    if (strTmp.Substring(6, 1) == "1") ModeAry[1] = 1;
                    if (strTmp.Substring(5, 1) == "1") ModeAry[2] = 1;
                    if (strTmp.Substring(4, 1) == "1") ModeAry[3] = 1;
                    if (strTmp.Substring(3, 1) == "1") ModeAry[4] = 1;
                    Switch = CsConst.myRevBuf[50];
                    TimeAry = new byte[4];
                    Array.Copy(CsConst.myRevBuf, 51, TimeAry, 0, 4);
                    ProtectTemperature = CsConst.myRevBuf[55];
                    ControlMode = CsConst.myRevBuf[57];
                    HeatType = CsConst.myRevBuf[58];
                    SysEnable = new byte[4];
                    if (CsConst.myRevBuf[59] == 1) SysEnable[0] = 1;
                    strTmp = GlobalClass.IntToBit(Convert.ToInt32(CsConst.myRevBuf[60]), 8);
                    if (strTmp.Substring(7, 1) == "1") SysEnable[1] = 1;
                    if (strTmp.Substring(6, 1) == "1") SysEnable[2] = 1;
                    if (strTmp.Substring(5, 1) == "1") SysEnable[3] = 1;
                    CompenValue = CsConst.myRevBuf[61];
                    DesSubnetID = CsConst.myRevBuf[62];
                    DesDeviceID = CsConst.myRevBuf[63];
                    Channel = CsConst.myRevBuf[64];
                    minTemp = CsConst.myRevBuf[65];
                    maxTemp = CsConst.myRevBuf[66];
                    CsConst.myRevBuf = new byte[1200];
                }
                else return false;


                CalculationModeTargets = new byte[40];
                if (CsConst.isRestore)
                {
                    ReadSalveChannelWhenSlaveMode(SubNetID, DeviceID, DeviceType);
                }
                #endregion
            }
            catch {
                return false;
            }
            return true;
        }

        public void ModifyCurrentStatusFromFloorheatingModule(Byte SubNetID, Byte DeviceID, int DeviceType, Byte TemperatureType) 
        {
             Byte[] arayTmp = new byte[8];
             arayTmp[0] = TemperatureType;
             arayTmp[1] = WorkingSwitch;
             arayTmp[2] = WorkingTempMode;
             Array.Copy(ModeTemp, 0, arayTmp, 3, 4);
             arayTmp[7] = ID;
             if (CsConst.mySends.AddBufToSndList(arayTmp, 0x1946, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
            {
                CsConst.myRevBuf = new byte[1200];
            }
        }

        public Boolean ReadCurrentStatusFromFloorheatingModule(Byte SubNetID, Byte DeviceID, int DeviceType)
        {
            try
            {
                Byte[] ArayTmp = new Byte[] { ID };
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1944, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                {
                    CurrentTemp = CsConst.myRevBuf[26];
                    WorkingSwitch = CsConst.myRevBuf[27];
                    WorkingTempMode = CsConst.myRevBuf[28];
                    ModeTemp = new byte[4];
                    Array.Copy(CsConst.myRevBuf, 29, ModeTemp, 0, 4);
                    CsConst.myRevBuf = new byte[1200];
                }
                else return false;
            }
            catch { return false; }
            return true;
        }

        public Boolean ReadSalveChannelWhenSlaveMode(Byte SubNetID, Byte DeviceID, int DeviceType)
        {
            try
            {
                if (ControlMode == 0) // 从机时读取
                {
                    Byte[] ArayTmp = new byte[1];
                    ArayTmp[0] = ID;
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1972, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true) //新版把目标独立出来
                    {
                        Array.Copy(CsConst.myRevBuf, 25, CalculationModeTargets, 0, 34);
                        CsConst.myRevBuf = new byte[1200];
                        HDLUDP.TimeBetwnNext(1);
                    }
                    else return false;
                }
            }
            catch
            {
                return false;
            }
            return true;
        }


        public void ModifyFloorheatingAdvancedCommandsGroup(Byte SubNetID, Byte DeviceID, int DeviceType)
        {
            if (AdvancedCommands == null) return;
            // read advance commands when playing
            Byte CmdID = 100;
            foreach (UVCMD.ControlTargets TmpCmd in AdvancedCommands)
            {
                if (TmpCmd != null)
                {
                    byte[] arayCMD = new byte[9];
                    arayCMD[0] = CmdID;
                    arayCMD[1] = TmpCmd.Type;
                    arayCMD[2] = TmpCmd.SubnetID;
                    arayCMD[3] = TmpCmd.DeviceID;
                    arayCMD[4] = TmpCmd.Param1;
                    arayCMD[5] = TmpCmd.Param2;
                    arayCMD[6] = TmpCmd.Param3;   // save targets
                    arayCMD[7] = TmpCmd.Param4;
                    arayCMD[8] = ID;
                    CsConst.mySends.AddBufToSndList(arayCMD, 0x195C, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType));
                    HDLUDP.TimeBetwnNext(arayCMD.Length);
                }
                CmdID++;
            }
        }

        public Boolean ReadFloorheatingAdvancedCommandsGroup(Byte SubNetID, Byte DeviceID, int DeviceType)
        {
            try
            {
                // read advance commands when playing
                AdvancedCommands = new UVCMD.ControlTargets[8];
                Byte[] ArayTmp = new byte[2];
                if (CsConst.isRestore)
                {
                    for (byte bytI = 0; bytI <= 7; bytI++)
                    {
                        ArayTmp = new Byte[] { (Byte)(100 + bytI), ID };
                        if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x195A, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                        {
                            AdvancedCommands[bytI] = new UVCMD.ControlTargets();
                            AdvancedCommands[bytI].ID = CsConst.myRevBuf[26];
                            AdvancedCommands[bytI].Type = CsConst.myRevBuf[27];
                            AdvancedCommands[bytI].SubnetID = CsConst.myRevBuf[28];
                            AdvancedCommands[bytI].DeviceID = CsConst.myRevBuf[29];
                            AdvancedCommands[bytI].Param1 = CsConst.myRevBuf[30];
                            AdvancedCommands[bytI].Param2 = CsConst.myRevBuf[31];
                            AdvancedCommands[bytI].Param3 = CsConst.myRevBuf[32];
                            AdvancedCommands[bytI].Param4 = CsConst.myRevBuf[33];
                            CsConst.myRevBuf = new byte[1200];
                            HDLUDP.TimeBetwnNext(10);
                        }
                        else return false; 
                    }
                }
            }
            catch 
            {
                return false;
            }
            return true;
        }

        public Boolean ReadButtonRemark(byte SubNetID, byte DeviceID, int DeviceType, bool isShowMessage)
        {
            try
            {
                byte[] ArayTmp = null;
                int CMD = 0xE004;
                if (EnviroNewDeviceTypeList.EnviroNewPanelDeviceTypeList.Contains(DeviceType))
                {
                    ArayTmp = new byte[4];
                    ArayTmp[0] = Convert.ToByte(ID + 1);
                    ArayTmp[1] = 6;
                    ArayTmp[2] = 0;
                    ArayTmp[3] = 9;
                }
                //读取备注
                if (CsConst.mySends.AddBufToSndList(ArayTmp, CMD, SubNetID, DeviceID, false, isShowMessage, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                {
                    byte[] arayRemark = new byte[20];
                    Array.Copy(CsConst.myRevBuf, 26, arayRemark, 0, 20);
                    Remark = HDLPF.Byte2String(arayRemark);
                    HDLUDP.TimeBetwnNext(1);
                }
                else return false;
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}
