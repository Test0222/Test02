using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HDL_Buspro_Setup_Tool
{
    [Serializable]
    public class NewIR:HdlDeviceBackupAndRestore
    {
        public object Clone()
        {
            return this.MemberwiseClone();
        }
        public int DIndex;  ////设备唯一编号
        public string strName;
        public List<NewIRCode> IRCodes = null;  //// 红外码库
        public List<UVCMD.IRCode> IRKeys = null;  //// 红外码库
        public List<Sequence> Sequences = null;
        public List<Clock> Clocks = null;
        public List<Scene> Scenes = null;
        public bool[] MyRead2UpFlags = new bool[8]; //前面四个表示读取 后面表示上传
        public byte[] arayTime;
        public byte[] arayBrocast;

        [Serializable]
        public class NewIRCode
        {
            public int KeyID;
            public byte DevID;
            public int IRIndex;
            public int IRLength;
            public string Remark;
            public string Codes;
            public List<UVCMD.IRCode> KeyCodes = null;  //// 红外码库
        }

        [Serializable]
        public class Sequence
        {
            public byte ID;
            public string Remark;
            public byte Enable;
            public List<Step> Steps;
        }

        [Serializable]
        public class Step
        {
            public byte DevID;//设备ID，0为无线
            public byte KeyNo;//按键号
            public byte ReSendDelay;//重发延时
            public byte ReSendTimes;//重发次数
            public byte StepDelay;//步骤延时
            public byte StateAndChannle;//开关状态和通道
        }

        [Serializable]
        public class Clock
        {
            public byte ID;
            public byte Enable;
            public byte Type;
            public byte[] arayParam = new byte[5];
            public byte SceneID;
        }

        [Serializable]
        public class Scene
        {
            public byte ID;
            public List<UVCMD.ControlTargets> Targets;
        }

        //<summary>
        //读取默认设置，将所有数据读取缓存
        //</summary>
        public void ReadDefaultInfo()
        {
            IRCodes = new List<NewIRCode>();
            Sequences = new List<Sequence>();
            IRKeys = new List<UVCMD.IRCode>();
        }

        public bool UploadNewIRInfosToDevice(string DevName, int intDeviceType, int intActivePage)
        {
            //保存basic informations
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
            if (CsConst.isRestore)
            {
                if (CsConst.mySends.AddBufToSndList(ArayMain, 0x0010, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(intDeviceType)) == true)
                {
                    HDLUDP.TimeBetwnNext(20);
                }
                else return false;
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(5);
            byte[] ArayTmp = new byte[0];
            if (intActivePage == 0 || intActivePage == 1)
            {
                if (IRCodes != null && IRCodes.Count != 0)
                {
                    #region
                    for (int i = 0; i < IRCodes.Count; i++)  //IRCodes.Count
                    {
                        NewIRCode temp = IRCodes[i];
                        ArayTmp = new byte[26];
                        if (intDeviceType == 1301 || intDeviceType == 1300 || intDeviceType == 6100)
                            ArayTmp = new byte[27];
                        ArayTmp[0] = Convert.ToByte(temp.KeyID);
                        ArayTmp[1] = 0;
                        ArayTmp[2] = temp.DevID;
                        ArayTmp[3] = (byte)(temp.IRIndex / 256);
                        ArayTmp[4] = (byte)(temp.IRIndex % 256);
                        ArayTmp[5] = (byte)temp.IRLength;
                        arayTmpRemark = new Byte[20];
                        arayTmpRemark = HDLUDP.StringToByte(temp.Remark);
                        if (arayTmpRemark.Length > 20)
                        {
                            Array.Copy(arayTmpRemark, 0, ArayTmp, 6, 20);
                        }
                        else
                        {
                            Array.Copy(arayTmpRemark, 0, ArayTmp, 6, arayTmpRemark.Length);
                        }

                        if (intDeviceType == 1301 || intDeviceType == 1300)
                        {
                            if (temp.DevID < 4)
                            {
                                ArayTmp[26] = 0;
                            }
                            else ArayTmp[26] = 1;
                            if (temp.DevID != 5)
                            {
                                ArayTmp[0] = Convert.ToByte(ArayTmp[0] - 4);
                            }
                        }
                        else if (intDeviceType == 6100)
                        {
                            if (i < 3 && temp.DevID == 255)
                            {
                                ArayTmp[2] = 1;
                            }
                            else if (temp.DevID == 255)
                            {
                                ArayTmp[2] = 0;
                            }
                            // 空调设置标识，固定存储为1
                            if (i < 3)
                            {
                                ArayTmp[26] = 1;
                            }
                            else ArayTmp[26] = 0;
                        }

                        if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(5 + i);
                        if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x137A, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(intDeviceType)) == true) //请求空间
                        {
                            byte[] arayCodes = GlobalClass.HexToByte(temp.Codes);
                            if (arayCodes.Length > 62)
                            {
                                #region
                                int Count = arayCodes.Length / 62;
                                if (arayCodes.Length % 124 != 0) Count = Count + 1;
                                for (int j = 0; j < Count; j++)
                                {
                                    if (arayCodes.Length % 62 != 0)
                                    {
                                        if (i == (Count - 1))
                                        {
                                            ArayTmp = new byte[2 + arayCodes.Length % 124];
                                            ArayTmp[0] = Convert.ToByte(temp.KeyID);
                                            ArayTmp[1] = Convert.ToByte(j + 1);
                                            for (int k = 0; k < arayCodes.Length % 62; k++) ArayTmp[2 + k] = arayCodes[j * 62 + k];
                                        }
                                        else
                                        {
                                            ArayTmp = new byte[2 + 62];
                                            ArayTmp[0] = Convert.ToByte(temp.KeyID);
                                            ArayTmp[1] = Convert.ToByte(j + 1);
                                            for (int k = 0; k < 62; k++) ArayTmp[2 + k] = arayCodes[j * 62 + k];
                                        }
                                    }
                                    else
                                    {
                                        ArayTmp = new byte[2 + 62];
                                        ArayTmp[0] = Convert.ToByte(temp.KeyID);
                                        ArayTmp[1] = Convert.ToByte(j + 1);
                                        for (int k = 0; k < 62; k++) ArayTmp[2 + k] = arayCodes[j * 62 + k]; ;
                                    }
                                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x137A, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(intDeviceType)) == true)
                                    {

                                    }
                                    else
                                    {
                                        return false;
                                    }
                                }
                                #endregion
                            }
                            else if (arayCodes.Length > 0)
                            {
                                #region
                                ArayTmp = new byte[2 + arayCodes.Length];
                                ArayTmp[0] = Convert.ToByte(temp.KeyID);
                                ArayTmp[1] = 1;

                                arayCodes.CopyTo(ArayTmp, 2);
                                // 上传试红外码
                                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x137A, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(intDeviceType)) == true)
                                {
                                }
                                #endregion
                            }
                        }
                    }
                    #endregion
                }
                if (intDeviceType == 729 || intDeviceType == 1300)
                {
                    ArayTmp = new byte[4];
                    for (int i = 0; i < 4; i++)
                    {
                        ArayTmp[i] = arayBrocast[i];
                    }
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xE0FA, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(intDeviceType)) == true)
                    {

                    }
                    else return false;
                }
                if (intDeviceType == 6100)
                {
                    ArayTmp = new byte[7];
                    for (int i = 0; i < 7; i++)
                    {
                        ArayTmp[i] = arayTime[i];
                    }
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xDA02, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(intDeviceType)) == true)
                    {

                    }
                    else return false;
                }
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(40);
            if (intActivePage == 0 || intActivePage == 6)
            {
                #region
                if (intDeviceType == 6100)
                {

                    for (int i = 0; i < Clocks.Count; i++)
                    {
                        ArayTmp = new byte[9];
                        ArayTmp[0] = Clocks[i].ID;
                        ArayTmp[1] = Clocks[i].Enable;
                        ArayTmp[2] = Clocks[i].Type;
                        Array.Copy(Clocks[i].arayParam, 0, ArayTmp, 3, 5);
                        ArayTmp[8] = Clocks[i].SceneID;
                        if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xE46C, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(intDeviceType)) == true)
                        {

                            HDLUDP.TimeBetwnNext(20);
                        }
                        else return false;
                        if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(40 + i);
                    }
                    if (CsConst.isRestore)
                    {
                        if (Scenes != null && Scenes.Count == 8)
                        {
                            #region
                            for (int i = 1; i <= 8; i++)
                            {
                                Scene temp = Scenes[i - 1];
                                if (temp.Targets != null && temp.Targets.Count > 0)
                                {
                                    foreach (UVCMD.ControlTargets TmpCmd in temp.Targets)
                                    {
                                        if (TmpCmd.Type != 0 && TmpCmd.Type != 255)
                                        {
                                            byte[] arayCMD = new byte[9];
                                            arayCMD[0] = Convert.ToByte(i);
                                            arayCMD[1] = Convert.ToByte(TmpCmd.ID);
                                            arayCMD[2] = TmpCmd.Type;
                                            arayCMD[3] = TmpCmd.SubnetID;
                                            arayCMD[4] = TmpCmd.DeviceID;
                                            arayCMD[5] = TmpCmd.Param1;
                                            arayCMD[6] = TmpCmd.Param2;
                                            arayCMD[7] = TmpCmd.Param3;   // save targets
                                            arayCMD[8] = TmpCmd.Param4;
                                            if (CsConst.mySends.AddBufToSndList(arayCMD, 0x1400, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(intDeviceType)) == true)
                                            {

                                                HDLUDP.TimeBetwnNext(20);
                                            }
                                            else return false;
                                        }
                                    }
                                }
                                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(60 + i);
                            }
                            #endregion
                        }
                    }
                }
                #endregion
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(100);
            return true;
        }

        public bool DownloadNewIRInfoFrmDevice(string DevName, int intDeviceType, int intActivePage)
        {
            string strMainRemark = DevName.Split('\\')[1].Trim();
            DevName = DevName.Split('\\')[0].Trim();

            byte bytSubID = byte.Parse(DevName.Split('-')[0].ToString());
            byte bytDevID = byte.Parse(DevName.Split('-')[1].ToString());

            byte[] ArayTmp = new byte[0];
            String sRemark = HDLSysPF.ReadDeviceMainRemark(bytSubID, bytDevID);
            
            strName = bytSubID.ToString() + "-" + bytDevID.ToString() + "\\" + sRemark;

            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(1);

            if (intActivePage == 0 || intActivePage == 1)
            {
                arayBrocast = new byte[5];
                arayTime = new byte[7];
                ArayTmp = new byte[2];
                IRCodes = new List<NewIRCode>();
                //读取28红外备注
                int MaxIRDevice = 28;
                if (intDeviceType == 6100) MaxIRDevice = 10;
                if (intDeviceType == 1301 || intDeviceType == 1300 || intDeviceType == 6100) ArayTmp = new byte[3];
                for (int i = 0; i < MaxIRDevice; i++)
                {
                    ArayTmp[0] = Convert.ToByte(i + 1);
                    ArayTmp[1] = 0;
                    if (intDeviceType == 1301 || intDeviceType == 1300)
                    {
                        if (i >= 4)
                        {
                            ArayTmp[0] = Convert.ToByte(ArayTmp[0] - 4);
                            ArayTmp[2] = 0;
                        }
                        else ArayTmp[2] = 1;
                    }
                    else if (intDeviceType == 6100)
                    {
                        if (i >= 3)
                        {
                            ArayTmp[0] = Convert.ToByte(ArayTmp[0] - 3);
                            ArayTmp[2] = 0;
                        }
                        else ArayTmp[2] = 1;
                    }
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x137C, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(intDeviceType)) == true)
                    {
                        NewIRCode temp = new NewIRCode();
                        temp.KeyID = ArayTmp[0];
                        temp.DevID = CsConst.myRevBuf[27];
                        temp.IRIndex = CsConst.myRevBuf[28] * 256 + CsConst.myRevBuf[29];
                        temp.IRLength = CsConst.myRevBuf[30];
                        temp.KeyCodes = new List<UVCMD.IRCode>();
                        byte[] arayRemark = new byte[20];
                        for (int intI = 0; intI < 20; intI++) { arayRemark[intI] = CsConst.myRevBuf[32 + intI]; };
                        temp.Remark = HDLPF.Byte2String(arayRemark);
                        temp.Codes = "";
                        if (temp.DevID >= 6) temp.Remark = "";
                        if (temp.IRLength != 0 && temp.IRLength != 255)
                        {
                            ArayTmp[1] = 1;
                            ArayTmp[2] = (Byte)temp.IRLength;
                            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x137C, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(intDeviceType)) == true)
                            {
                                for (int intI = 0; intI < temp.IRLength; intI++)
                                    temp.Codes = temp.Codes + CsConst.myRevBuf[27 + intI].ToString("X2") + " ";
                            }
                        }
                        IRCodes.Add(temp);
                    }
                    else return false;

                    HDLUDP.TimeBetwnNext(1);
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(1 + i);
                }

                if (intDeviceType != 1301)
                {
                    ArayTmp = new byte[0];
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xE0F8, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(intDeviceType)) == true)
                    {
                        Array.Copy(CsConst.myRevBuf, 25, arayBrocast, 0, 5);

                        HDLUDP.TimeBetwnNext(1);
                    }
                    else return false;
                }

                if (intDeviceType == 6100)
                {
                    ArayTmp = new byte[0];
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xDA00, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(intDeviceType)) == true)
                    {
                        Array.Copy(CsConst.myRevBuf, 26, arayTime, 0, 6);

                        HDLUDP.TimeBetwnNext(1);
                    }
                    else return false;
                }
                MyRead2UpFlags[0] = true;
            }

            if (intActivePage == 0 || intActivePage == 2)
            {
                if (CsConst.isRestore)
                {

                }
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(40);
            if (intActivePage == 0 || intActivePage == 3)
            {
                if (CsConst.isRestore)
                {

                }
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(50);
            if (intActivePage == 0 || intActivePage == 4)
            {
                if (CsConst.isRestore)
                {

                }
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(60);
            if (intActivePage == 0 || intActivePage == 5)
            {

            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(70);
            if (intActivePage == 0 || intActivePage == 6)
            {
                #region
                if (intDeviceType == 6100)
                {
                    Clocks = new List<Clock>();
                    Scenes = new List<Scene>();



                    for (int i = 1; i <= 16; i++)
                    {
                        ArayTmp = new byte[1];
                        ArayTmp[0] = Convert.ToByte(i);
                        if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xE46E, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(intDeviceType)) == true)
                        {
                            Clock temp = new Clock();
                            temp.ID = Convert.ToByte(i);
                            temp.Enable = CsConst.myRevBuf[26];
                            temp.Type = CsConst.myRevBuf[27];
                            if (temp.Type > 1) temp.Type = 0;
                            temp.arayParam = new byte[5];
                            Array.Copy(CsConst.myRevBuf, 28, temp.arayParam, 0, 5);
                            temp.SceneID = CsConst.myRevBuf[33];
                            if (temp.SceneID > 16 || temp.SceneID < 1) temp.SceneID = 1;
                            Clocks.Add(temp);

                        }
                        else return false;
                    }

                    if (CsConst.isRestore)
                    {
                        for (int i = 1; i <= 8; i++)
                        {
                            Scene temp = new Scene();
                            temp.ID = Convert.ToByte(i);
                            temp.Targets = new List<UVCMD.ControlTargets>();

                            ArayTmp = new byte[2];
                            ArayTmp[0] = Convert.ToByte(i);
                            for (int j = 1; j <= 32; j++)
                            {
                                ArayTmp[1] = Convert.ToByte(j);
                                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1402, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(intDeviceType)) == true)
                                {
                                    UVCMD.ControlTargets tmp = new UVCMD.ControlTargets();
                                    tmp.ID = Convert.ToByte(CsConst.myRevBuf[26]);
                                    tmp.Type = CsConst.myRevBuf[27];  //转换为正确的类型
                                    tmp.SubnetID = CsConst.myRevBuf[28];
                                    tmp.DeviceID = CsConst.myRevBuf[29];
                                    tmp.Param1 = CsConst.myRevBuf[30];
                                    tmp.Param2 = CsConst.myRevBuf[31];
                                    tmp.Param3 = CsConst.myRevBuf[32];
                                    tmp.Param4 = CsConst.myRevBuf[33];
                                    temp.Targets.Add(tmp);
                                    Scenes.Add(temp);

                                }
                                else return false;
                            }
                        }
                    }
                }
                #endregion
                MyRead2UpFlags[5] = true;
            }

            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(100);
            return true;
        }
    }
}
