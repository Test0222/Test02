using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace HDL_Buspro_Setup_Tool
{
    [Serializable]
    [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    /// <summary>
    /// 按键目标读取和修改 统一放置
    /// </summary>
    public class HDLButton
    {
        public byte ID;
        public string Remark;
        public byte Mode;
        public byte IsLEDON;
        public byte IsDimmer;
        public byte SaveDimmer;
        public byte bytMutex; //是不是互斥
        public byte byteLink;//按键联动

        public Byte IconID; // 图标ID
        public Byte PageID; // 页号

        public List<UVCMD.ControlTargets> KeyTargets;

        public Byte[] curStatus; //当前状态

        public Boolean ReadButtonRemarkAndCMDFromDevice(Byte SubNetID, Byte DeviceID, int DeviceType, int RemoteAddress, Byte ReadOnlyRemarkorCMD, Boolean blnIsSupportE474,
                                                        Byte bStartCmd, Byte bToCmd)
        {
            try
            {
                Byte bTmpStartRead = 0;
                if (ReadOnlyRemarkorCMD == 0 || ReadOnlyRemarkorCMD == 255)
                {
                    Remark = ReadButtonRemark(SubNetID, DeviceID, DeviceType, RemoteAddress);
                }

                if (Mode == 0) return true;

                if (ReadOnlyRemarkorCMD == 1 || ReadOnlyRemarkorCMD == 255)
                {
                    //读取目标
                    byte bytTotalCMD = 1;
                    switch (Mode)
                    {
                        case 1:
                        case 2:
                        case 3:
                        case 6:
                        case 10: bytTotalCMD = 1; break;
                        case 4:
                        case 5:
                        case 7:
                        case 13: bytTotalCMD = 15; break;
                        case 11: bytTotalCMD = 15; break;
                        case 16:
                        case 17: bytTotalCMD = 15; break;
                        case 14: bytTotalCMD = 49; break;
                    }

                    if (bStartCmd == 0 && bToCmd == 0)
                    {
                        bStartCmd = 1; bToCmd = bytTotalCMD;
                    }
                    String strEnable = "";

                    Byte ExtraLength = 0;
                    Byte[] ArayTmp = new Byte[1 + ExtraLength];
                    if (IPmoduleDeviceTypeList.RFIpModuleV2.Contains(DeviceType))
                    {
                        ExtraLength = 1;
                        ArayTmp = new Byte[1 + ExtraLength];
                        ArayTmp[0] = ID;                        
                        ArayTmp[ExtraLength] = (Byte)RemoteAddress;
                    }
                    else if (EnviroNewDeviceTypeList.EnviroNewPanelDeviceTypeList.Contains(DeviceType)) // 新版彩屏面板
                    {
                        ExtraLength = 3;
                        ArayTmp = new Byte[1 + ExtraLength];
                        ArayTmp[0] = ID;
                        ArayTmp[1] = 4;
                        ArayTmp[2] = 0;
                        ArayTmp[3] = 36;
                    }
                    else if (EnviroDLPPanelDeviceTypeList.HDLEnviroDLPPanelDeviceTypeList.Contains(DeviceType)) // 旧版彩屏面板
                    {
                        ExtraLength = 3;
                        ArayTmp = new Byte[1 + ExtraLength];
                        ArayTmp[0] = ID;
                        ArayTmp[1] = 4;
                        ArayTmp[2] = 0;
                        ArayTmp[3] = 36;
                    }

                    //读取目标有效与否
                    #region
                    if (blnIsSupportE474 == true)
                    {
                        if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xE474, SubNetID, DeviceID, false, false, true, false) == true)
                        {
                            int count1 = CsConst.myRevBuf[26];
                            int count2 = count1 / 8;
                            if (count1 % 8 != 0) count2 = count2 + 1;
                            byte[] arayEnable = new byte[count2];
                            Array.Copy(CsConst.myRevBuf, 27, arayEnable, 0, count2);

                            for (int i = 0; i < count2; i++)
                            {
                                string strTmp = GlobalClass.AddLeftZero(Convert.ToString(arayEnable[i], 2), 8);
                                for (int j = 7; j >= 0; j--)
                                {
                                    string str = strTmp.Substring(j, 1);
                                    strEnable = strEnable + str;
                                }
                            }
                            CsConst.myRevBuf = new byte[1200];
                        }
                    }
                    #endregion

                    if (MS04DeviceTypeList.MS04DryContactWithE000PublicCMD.Contains(DeviceType)) // 干节点统一处理
                    {
                        if (Mode == 9 || Mode == 0) // 组合开关分开放置目标  或者机械开关
                            bytTotalCMD = 199;
                    }
                    else
                    {
                        Byte[] oTMpModeList = new Byte[]{8,9,10,11,12,14,15,16,17,18,19,21};
                        if (oTMpModeList.Contains(Mode) && bStartCmd >=50) // j机械开关或者其他类别请添加在此处
                        {
                            bTmpStartRead = 49; // 不初始化整个目标组合
                        }
                        else
                        {
                            KeyTargets = new List<UVCMD.ControlTargets>();
                        }
                    }
                    for (int byt = 1 + bTmpStartRead; byt <= bytTotalCMD + bTmpStartRead; byt++)
                    {
                        if (byt >= bStartCmd && byt <= bToCmd)
                        {
                            #region
                            bool isRead = true;
                            if (strEnable != "")
                            {
                                if (strEnable.Length >= byt)
                                {
                                    if (strEnable.Substring(byt - 1, 1) == "0")
                                        isRead = false;
                                }
                            }
                            if (isRead)
                            {
                                byte[] arayTmp = new byte[2 + ExtraLength];
                                arayTmp[0] = ID;
                                arayTmp[1] = (Byte)byt;
                                if (ExtraLength != 0)
                                {
                                    if (ExtraLength == 3)
                                    {
                                        arayTmp[2] = 4;
                                        arayTmp[3] = 0;
                                        arayTmp[4] = 36;
                                    }
                                    else
                                    {
                                        arayTmp[2] = (Byte)RemoteAddress;
                                    }
                                }

                                if (CsConst.mySends.AddBufToSndList(arayTmp, 0xE000, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
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
                                    if (KeyTargets.Count < oCMD.ID)
                                    {
                                        KeyTargets.Add(oCMD);
                                    }
                                    else
                                    {
                                        KeyTargets[oCMD.ID - 1] = oCMD;
                                    }
                                    CsConst.myRevBuf = new byte[1200];
                                }
                                else return false;
                            }
                            #endregion
                        }
                    }
                }
            }
            catch 
            {
                return false;
            }
            return true;
        }

        public void UploadButtonRemarkAndCMDToDevice(Byte SubNetID, Byte DeviceID, int DeviceType, int RemoteAddress, Byte ReadOnlyRemarkorCMD)
        {
           if (ReadOnlyRemarkorCMD ==0)  ModifyButtonRemark(SubNetID,DeviceID,DeviceType,RemoteAddress);
            if (Mode == 0 || Mode > 30) return;
            Byte ExtraLength = 0;
            if (IPmoduleDeviceTypeList.RFIpModuleV2.Contains(DeviceType)) ExtraLength = 1;
            else if (EnviroNewDeviceTypeList.EnviroNewPanelDeviceTypeList.Contains(DeviceType)) ExtraLength = 3;

            if (KeyTargets != null)
            {
                foreach (UVCMD.ControlTargets TmpCmd in KeyTargets)
                {
                    if (TmpCmd.Type != 0 && TmpCmd.Type != 255)
                    {
                        byte[] arayCMD = new byte[9 + ExtraLength];
                        arayCMD[0] = ID;
                        arayCMD[1] = byte.Parse(TmpCmd.ID.ToString());
                        arayCMD[2] = TmpCmd.Type;
                        arayCMD[3] = TmpCmd.SubnetID;
                        arayCMD[4] = TmpCmd.DeviceID;
                        arayCMD[5] = TmpCmd.Param1;
                        arayCMD[6] = TmpCmd.Param2;
                        arayCMD[7] = TmpCmd.Param3;   // save targets
                        arayCMD[8] = TmpCmd.Param4;
                        if (ExtraLength != 0)
                        {
                            if (ExtraLength == 3)
                            {
                                arayCMD[6 + ExtraLength] = 4;
                                arayCMD[7 + ExtraLength] = 0;
                                arayCMD[8 + ExtraLength] = 36;
                            }
                            else
                            {
                                arayCMD[8 + ExtraLength] = (Byte)RemoteAddress;
                            }
                        }
                        if (CsConst.mySends.AddBufToSndList(arayCMD, 0xE002, SubNetID, DeviceID, false, true, true, true) == false) return;
                        HDLUDP.TimeBetwnNext(arayCMD.Length);
                        CsConst.myRevBuf = new byte[1200];
                    }
                }
            }
        }

        public String ReadButtonRemark(Byte SubNetID, Byte DeviceID, int DeviceType, int RemoteAddress)
        {
            String currentRemark = "";
            Byte ExtraLength = 0;
            if (IPmoduleDeviceTypeList.RFIpModuleV2.Contains(DeviceType)) ExtraLength = 1; 
            Byte[] ArayTmp = new Byte[1 + 1];
            ArayTmp[0] = ID;
            if (ExtraLength !=0) ArayTmp[1] = (Byte)RemoteAddress;
            //读取备注
            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xE004, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
            {
                byte[] arayRemark = new byte[20];
                Array.Copy(CsConst.myRevBuf, 26, arayRemark, 0, 20);
                currentRemark = HDLPF.Byte2String(arayRemark);
                CsConst.myRevBuf = new byte[1200];
            }
            return currentRemark;
        }

        public void ModifyButtonRemark(Byte SubNetID, Byte DeviceID, int DeviceType, int RemoteAddress)
        {
            Byte ExtraLength = 0;
            if (IPmoduleDeviceTypeList.RFIpModuleV2.Contains(DeviceType)) ExtraLength = 1;
            byte[] arayRemark = new byte[21 + ExtraLength];

            if (ExtraLength != 0) arayRemark[21 + ExtraLength - 1] = (Byte)RemoteAddress;

            byte[] arayTmp = HDLUDP.StringToByte(Remark);
            arayRemark[0] = ID;

            arayTmp.CopyTo(arayRemark, 1);
            if (CsConst.mySends.AddBufToSndList(arayRemark, 0xE006, SubNetID, DeviceID, false, true, true, true)) return;
        }
    }
    [Serializable]
    public class MS04Key : HDLButton
    {
        public byte bytEnable;  // 当期按键有没有启用
        public byte bytReflag;  // 当期按键有没有重复触发使能
        public int[] ONOFFDelay = new int[] {0,0 }; // 开关延迟
        public List<UVCMD.ControlTargets> KeyOffTargets = null;
        public Byte[] AvoidCut; // 防拆
    }
}
