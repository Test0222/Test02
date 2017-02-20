using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HDL_Buspro_Setup_Tool
{
    public class SensorLogic
    {
         //逻辑关系
        public byte ID;
        public string Remarklogic;//逻辑备注
        public byte Enabled;
        public byte bytRelation; // 关系
        public byte[] EnableSensors = new byte[15]; // 温度传感器 亮度传感器  红外传感器  超声波传感器  按键  通用开关一 通用开关二  以逻辑块为条件} 
        public byte[] Paramters = new byte[20];//最低温度 最高温度 最低亮度，最高亮度，红外传感器，超声波传感器，按键 逻辑号，逻辑状态
        public UniversalSwitchSet UV1;//通用开关1
        public UniversalSwitchSet UV2;//通用开关2
        public List<UVCMD.ControlTargets> SetUp;//成立目标
        public List<UVCMD.ControlTargets> NoSetUp;//不成立目标
        public int DelayTimeT;//成立延迟时间
        public int DelayTimeF;//不成立延迟时间

        public SensorLogic()
        {
            EnableSensors = new Byte[20];

            Paramters = new Byte[20];
            UV1 = new UniversalSwitchSet();
            UV2 = new UniversalSwitchSet();
        }

        public void DownloadLogicSettingFromDevice(Byte SubNetID, Byte DeviceID, int DeviceType, Boolean ReadCommands)
        {
            DownloadOnlyLogicSettingFromDevice(SubNetID,DeviceID,DeviceType);
            if (ReadCommands)
            {
                DownloadLogicTrueCommandsFromDevice(SubNetID, DeviceID, DeviceType,0,0);
                DownloadLogicFalseCommandsFromDevice(SubNetID, DeviceID, DeviceType,0,0);
            }
        }

        public void DownloadOnlyLogicSettingFromDevice(Byte SubNetID, Byte DeviceID, int DeviceType)
        {
            //读取设置
            #region
            Byte[] ArayTmp = new Byte[] { ID};
            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x160E, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
            {
                bytRelation = CsConst.myRevBuf[27];
                EnableSensors = new byte[15];
                int intTmp = CsConst.myRevBuf[28] * 256 + CsConst.myRevBuf[29];
                for (byte i = 0; i < 12; i++)
                {
                    if ((intTmp & (1 << i)) == (1 << i))
                    {
                        switch (i)
                        {
                            case 0: EnableSensors[0] = 1; break;
                            case 1: EnableSensors[1] = 1; break;
                            case 5: EnableSensors[2] = 1; break;
                            case 6: EnableSensors[4] = 1; break;
                            case 8: EnableSensors[5] = 1; break;
                            case 9: EnableSensors[6] = 1; break;
                            case 10: EnableSensors[7] = 1; break;
                            case 11: EnableSensors[3] = 1; break;
                        }
                    }
                }

                DelayTimeT = CsConst.myRevBuf[30] * 256 + CsConst.myRevBuf[31];
                DelayTimeF = CsConst.myRevBuf[32] * 256 + CsConst.myRevBuf[33];
                Array.Copy(CsConst.myRevBuf, 34, Paramters, 0, 6);
                Paramters[6] = CsConst.myRevBuf[46];
                Paramters[7] = CsConst.myRevBuf[55];
                Paramters[8] = CsConst.myRevBuf[47];
                UV1 = new UniversalSwitchSet();
                if (201 <= CsConst.myRevBuf[49] && CsConst.myRevBuf[49] <= 248)
                    UV1.id = CsConst.myRevBuf[49];
                else
                    UV1.id = 201;
                if (CsConst.myRevBuf[50] <= 1)
                    UV1.condition = CsConst.myRevBuf[50];
                UV2 = new UniversalSwitchSet();
                if (201 <= CsConst.myRevBuf[51] && CsConst.myRevBuf[51] <= 248)
                    UV2.id = CsConst.myRevBuf[51];
                else
                {
                    UV2.id = 201;
                    if (UV1.id == 201) UV2.id = 202;
                }
                if (CsConst.myRevBuf[52] <= 1)
                    UV2.condition = CsConst.myRevBuf[52];
                Paramters[9] = CsConst.myRevBuf[53];
                Paramters[10] = CsConst.myRevBuf[54];
                CsConst.myRevBuf = new byte[1200];
                HDLUDP.TimeBetwnNext(1);
            }
            else return;
            #endregion
        }

        public void DownloadLogicTrueCommandsFromDevice(Byte SubNetID, Byte DeviceID, int DeviceType, Byte bStartCmd, Byte bToCmd)
        {
            Byte sumLogicCommandsInEveryBlock = 20;
            Int32 operationCode = 0x1612;
            if (Eightin1DeviceTypeList.HDL8in1DeviceType.Contains(DeviceType))
            {
                operationCode = 0x1654;
                sumLogicCommandsInEveryBlock = Eightin1DeviceTypeList.sumCommandsInEveryBlock;
            }
            if (bStartCmd == 0 && bToCmd == 0)
            {
                bStartCmd = 1;
                bToCmd = sumLogicCommandsInEveryBlock;
            }

            //成立的触发目标
            #region
            SetUp = new List<UVCMD.ControlTargets>();
            for (Byte bytJ = 1; bytJ <= sumLogicCommandsInEveryBlock; bytJ++)
            {
                if (bytJ >= bStartCmd && bytJ <= bToCmd)
                {
                    Byte[] ArayTmp = new byte[2] { ID, bytJ };
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, operationCode, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                    {
                        UVCMD.ControlTargets oCMD = new UVCMD.ControlTargets();
                        oCMD.ID = CsConst.myRevBuf[27];
                        oCMD.Type = CsConst.myRevBuf[28];  //转换为正确的类型

                        oCMD.SubnetID = CsConst.myRevBuf[29];
                        oCMD.DeviceID = CsConst.myRevBuf[30];
                        oCMD.Param1 = CsConst.myRevBuf[31];
                        oCMD.Param2 = CsConst.myRevBuf[32];
                        oCMD.Param3 = CsConst.myRevBuf[33];
                        oCMD.Param4 = CsConst.myRevBuf[34];
                        CsConst.myRevBuf = new byte[1200];
                        SetUp.Add(oCMD);
                        CsConst.myRevBuf = new byte[1200];
                        HDLUDP.TimeBetwnNext(1);
                    }
                    else return;
                }
            }
            #endregion
        }

        public void DownloadLogicFalseCommandsFromDevice(Byte SubNetID, Byte DeviceID, int DeviceType,Byte bStartCmd, Byte bToCmd)
        {
            try
            {
                Byte sumLogicCommandsInEveryBlock = 20;
                Byte sumLogibBlockInModule = 24;
                Int32 operationCode = 0x1612;
                if (Eightin1DeviceTypeList.HDL8in1DeviceType.Contains(DeviceType))
                {
                    operationCode = 0x1654;
                    sumLogibBlockInModule = Eightin1DeviceTypeList.TotalLogic;
                    sumLogicCommandsInEveryBlock = Eightin1DeviceTypeList.sumCommandsInEveryBlock;
                }

                if (bStartCmd == 0 && bToCmd == 0)
                {
                    bStartCmd = 1;
                    bToCmd = sumLogicCommandsInEveryBlock;
                }

                //不成立的触发目标
                #region
                NoSetUp = new List<UVCMD.ControlTargets>();
                for (Byte bytJ = 1; bytJ <= sumLogicCommandsInEveryBlock; bytJ++)
                {
                    if (bytJ >= bStartCmd && bytJ <= bToCmd)
                    {
                        Byte[] ArayTmp = new byte[2] { (Byte)(ID + sumLogibBlockInModule), bytJ };
                        if (CsConst.mySends.AddBufToSndList(ArayTmp, operationCode, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                        {
                            UVCMD.ControlTargets oCMD = new UVCMD.ControlTargets();
                            oCMD.ID = CsConst.myRevBuf[27];
                            oCMD.Type = CsConst.myRevBuf[28];  //转换为正确的类型

                            oCMD.SubnetID = CsConst.myRevBuf[29];
                            oCMD.DeviceID = CsConst.myRevBuf[30];
                            oCMD.Param1 = CsConst.myRevBuf[31];
                            oCMD.Param2 = CsConst.myRevBuf[32];
                            oCMD.Param3 = CsConst.myRevBuf[33];
                            oCMD.Param4 = CsConst.myRevBuf[34];
                            CsConst.myRevBuf = new byte[1200];
                            NoSetUp.Add(oCMD);
                            CsConst.myRevBuf = new byte[1200];
                            HDLUDP.TimeBetwnNext(1);
                        }
                        else return;
                    }
                }
                #endregion
            }
            catch { }
        }

        public Boolean UploadLogicSettingToDevice(Byte SubNetID, Byte DeviceID, int DeviceType, Boolean ReadCommands)
        {
            Boolean blnIsSuccess = true;
            blnIsSuccess = ModifyOnlyLogicSettingFromDevice(SubNetID, DeviceID, DeviceType);
            blnIsSuccess = ModifyLogicTrueCommandsFromDevice(SubNetID, DeviceID, DeviceType);
            blnIsSuccess = ModifyLogicFalseCommandsFromDevice(SubNetID, DeviceID, DeviceType);
            return blnIsSuccess;
        }

        public Boolean ModifyOnlyLogicSettingFromDevice(Byte SubNetID, Byte DeviceID, int DeviceType)
        {
            //修改设置
            Boolean blnIsSuccess = true;
            //修改设置
            Byte[] ArayTmp = new byte[30];
            ArayTmp[0] = ID;
            ArayTmp[1] = bytRelation;
            int intTmp = 0;
            for (int i = 0; i < 8; i++)
            {
                switch (i)
                {
                    case 0: intTmp = intTmp | (EnableSensors[i] << 0); break;
                    case 1: intTmp = intTmp | (EnableSensors[i] << 1); break;
                    case 2: intTmp = intTmp | (EnableSensors[i] << 5); break;
                    case 3: intTmp = intTmp | (EnableSensors[i] << 11); break;
                    case 4: intTmp = intTmp | (EnableSensors[i] << 6); break;
                    case 5: intTmp = intTmp | (EnableSensors[i] << 8); break;
                    case 6: intTmp = intTmp | (EnableSensors[i] << 9); break;
                    case 7: intTmp = intTmp | (EnableSensors[i] << 10); break;
                }
            }
            ArayTmp[2] = (Byte)(intTmp / 256);
            ArayTmp[3] = (Byte)(intTmp % 256);
            ArayTmp[4] = (Byte)(DelayTimeT / 256);
            ArayTmp[5] = (Byte)(DelayTimeT % 256);
            ArayTmp[6] = (Byte)(DelayTimeF / 256);
            ArayTmp[7] = (Byte)(DelayTimeF % 256);
            Array.Copy(Paramters, 0, ArayTmp, 8, 6);
            ArayTmp[14] = 20;
            ArayTmp[15] = 20;

            ArayTmp[20] = Paramters[6];
            ArayTmp[29] = Paramters[7];
            ArayTmp[21] = Paramters[8];

            if (UV1 != null)
            {
                ArayTmp[23] = UV1.id;
                ArayTmp[24] = UV1.condition;
            }

            if (UV2 != null)
            {
                ArayTmp[25] = UV2.id;
                ArayTmp[26] = UV2.condition;
            }
            ArayTmp[27] = Paramters[9];
            ArayTmp[28] = Paramters[10];

            blnIsSuccess = CsConst.mySends.AddBufToSndList(ArayTmp, 0x1610, SubNetID,DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType));
            return blnIsSuccess;
        }

        public Boolean ModifyLogicTrueCommandsFromDevice(Byte SubNetID, Byte DeviceID, int DeviceType)
        {
            Boolean blnIsSuccess = true;
            try
            {
                Int32 operationCode = 0x1614;
                if (Eightin1DeviceTypeList.HDL8in1DeviceType.Contains(DeviceType))
                {
                    operationCode = 0x1656;
                }

                if (SetUp != null && SetUp.Count != 0)
                {
                    foreach (UVCMD.ControlTargets TmpCmd in SetUp)
                    {
                        byte[] arayCMD = new byte[9];
                        arayCMD[0] = ID;
                        arayCMD[1] = byte.Parse(TmpCmd.ID.ToString());
                        arayCMD[2] = TmpCmd.Type;
                        arayCMD[3] = TmpCmd.SubnetID;
                        arayCMD[4] = TmpCmd.DeviceID;
                        arayCMD[5] = TmpCmd.Param1;
                        arayCMD[6] = TmpCmd.Param2;
                        arayCMD[7] = TmpCmd.Param3;   // save targets
                        arayCMD[8] = TmpCmd.Param4;
                        blnIsSuccess = CsConst.mySends.AddBufToSndList(arayCMD, operationCode, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType));
                    }
                }
            }
            catch { }
            return blnIsSuccess;
        }

        public Boolean ModifyLogicFalseCommandsFromDevice(Byte SubNetID, Byte DeviceID, int DeviceType)
        {
            Boolean blnIsSuccess = true;
            try
            {
                Byte sumLogibBlockInModule = 24;
                Int32 operationCode = 0x1614;
                if (Eightin1DeviceTypeList.HDL8in1DeviceType.Contains(DeviceType))
                {
                    operationCode = 0x1656;
                    sumLogibBlockInModule = Eightin1DeviceTypeList.TotalLogic;
                }

                if (NoSetUp != null && NoSetUp.Count != 0)
                {
                    foreach (UVCMD.ControlTargets TmpCmd in NoSetUp)
                    {
                        byte[] arayCMD = new byte[9];
                        arayCMD[0] = (Byte)(ID + sumLogibBlockInModule);
                        arayCMD[1] = byte.Parse(TmpCmd.ID.ToString());
                        arayCMD[2] = TmpCmd.Type;
                        arayCMD[3] = TmpCmd.SubnetID;
                        arayCMD[4] = TmpCmd.DeviceID;
                        arayCMD[5] = TmpCmd.Param1;
                        arayCMD[6] = TmpCmd.Param2;
                        arayCMD[7] = TmpCmd.Param3;   // save targets
                        arayCMD[8] = TmpCmd.Param4;
                        blnIsSuccess = CsConst.mySends.AddBufToSndList(arayCMD, operationCode, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType));
                    }
                }
            }
            catch { }
            return blnIsSuccess;
        }
    }
}
