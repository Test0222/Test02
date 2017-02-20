using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HDL_Buspro_Setup_Tool
{
    public class EnviroMusic
    {
        public Byte IconID; // 图标ID
        public String Remark; // 备注

        public byte ID;//音乐页号
        public byte Enable;//使能
        public byte CurrentZoneID;//当前区域ID
        public byte Type;//音乐类型
        public byte[] aryNetDevID = new byte[48];//24个区对应的子网ID,设备ID
        public UVCMD.ControlTargets[] Targets;//红外控制目标

        public EnviroMusic()
        {
            aryNetDevID = new Byte[48];

            Targets = new UVCMD.ControlTargets[14];
        }

        public Boolean ReadMusicSettingInformation(Byte SubNetID, Byte DeviceID, int DeviceType)
        {
            try
            {
                Byte[] ArayTmp = new Byte[] { ID };
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1930, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                {
                    Enable = CsConst.myRevBuf[25];
                    CurrentZoneID = CsConst.myRevBuf[26];
                    Type = CsConst.myRevBuf[27];
                    CsConst.myRevBuf = new byte[1200];
                }
                else return false;
                ArayTmp = new byte[2];
                ArayTmp[0] = 0;
                ArayTmp[1] = ID;
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1934, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                {
                    aryNetDevID = new byte[48];
                    Array.Copy(CsConst.myRevBuf, 27, aryNetDevID, 0, 24);
                    CsConst.myRevBuf = new byte[1200];
                }
                else return false;
                ArayTmp = new byte[2];
                ArayTmp[0] = 1;
                ArayTmp[1] = ID;
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1934, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                {
                    Array.Copy(CsConst.myRevBuf, 27, aryNetDevID, 24, 24);
                    CsConst.myRevBuf = new byte[1200];
                }
                else return false;
            }
            catch
            {
                return false;
            }
            return true;
        }

        public void ModifyMusicSettingInformation(Byte SubNetID, Byte DeviceID, int DeviceType)
        {
            #region
            byte[] arayTmp = new byte[4];
            arayTmp[0] = Enable;
            arayTmp[1] = CurrentZoneID;
            arayTmp[2] = Enable;
            arayTmp[3] = Type;
            if (CsConst.mySends.AddBufToSndList(arayTmp, 0x1932, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
            {
                CsConst.myRevBuf = new byte[1200];
            }
            else return;

            arayTmp = new byte[26];
            arayTmp[0] = 0;
            arayTmp[25] = ID;
            Array.Copy(aryNetDevID, 0, arayTmp, 1, 24);
            if (CsConst.mySends.AddBufToSndList(arayTmp, 0x1936, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
            {
                CsConst.myRevBuf = new byte[1200];
            }
            else return;
            arayTmp = new byte[26];
            arayTmp[0] = 1;
            arayTmp[25] = ID;
            Array.Copy(aryNetDevID, 24, arayTmp, 1, 24);
            if (CsConst.mySends.AddBufToSndList(arayTmp, 0x1936, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
            {
                CsConst.myRevBuf = new byte[1200];
            }
            else return;
            #endregion
        }

        public void ModifyMusicAdvancedCommandsGroup(Byte SubNetID, Byte DeviceID, int DeviceType)
        {
            if (Targets == null) return;
            // read advance commands when playing
            Byte CmdID = 0;
            foreach (UVCMD.ControlTargets TmpCmd in Targets)
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

        public Boolean ReadMusicAdvancedCommandsGroup(Byte SubNetID, Byte DeviceID, int DeviceType)
        {
            try
            {
                // read advance commands when playing
                Targets = new UVCMD.ControlTargets[14];
                Byte[] ArayTmp = new byte[2];
                if (CsConst.isRestore)
                {
                    for (byte bytI = 0; bytI <= 13; bytI++)
                    {
                        ArayTmp = new Byte[] { (Byte)(bytI), ID };
                        if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x195A, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                        {
                            Targets[bytI] = new UVCMD.ControlTargets();
                            Targets[bytI].ID = CsConst.myRevBuf[26];
                            Targets[bytI].Type = CsConst.myRevBuf[27];
                            Targets[bytI].SubnetID = CsConst.myRevBuf[28];
                            Targets[bytI].DeviceID = CsConst.myRevBuf[29];
                            Targets[bytI].Param1 = CsConst.myRevBuf[30];
                            Targets[bytI].Param2 = CsConst.myRevBuf[31];
                            Targets[bytI].Param3 = CsConst.myRevBuf[32];
                            Targets[bytI].Param4 = CsConst.myRevBuf[33];
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
                    ArayTmp[1] = 7;
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
