using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.Windows.Forms;

namespace HDL_Buspro_Setup_Tool
{
    [Serializable]
    public class MultiSensor : HdlDeviceBackupAndRestore
    {
        //基本信息
        public int DIndex;//设备唯一编号
        public string Devname;//子网ID 设备ID 备注
        public string Remark1;//干接点1备注
        public string Remark2;//干接点2备注
        public byte[] ONLEDs = new byte[5]; // 红外LED灯  工作指示灯的
        public byte[] EnableSensors = new byte[15]; // 留有预留 {温度传感器 亮度传感器  红外传感器 干接点一  干接点二   通用开关一 通用开关二  
        public byte[] EnableBroads = new byte[15];  //恒照度使能,恒照度亮度两个byte,kp两个byte,ki两个byte,周期,低限
        public byte[] ParamSensors = new byte[15];  //温度补偿值，红外灵敏度
        public byte[] ArayTmpMode = new byte[40];//红外接收模式
        public List<UVCMD.IRCode> IRCodes;  //红外发射
        public List<IRReceive> IrReceiver;   // 当接收到红外时 触发目标
        public List<SensorLogic> logic;
        public List<UVCMD.SecurityInfo> fireset;

        public bool[] MyRead2UpFlags = new bool[20];

        public class IRReceive
        {
            public int BtnNum;//按键号
            public string IRBtnRemark;//红外按键备注
            public byte IRBtnModel;//红外按键模式
            public List<UVCMD.ControlTargets> TargetInfo;//红外控制目标信息
        }

        ///<summary>
        ///读取默认的设置
        ///</summary>
        public void ReadDefaultInfo()
        {
            ONLEDs = new byte[] { 1, 1, 0, 0, 0 }; // 红外LED灯; // 电源工作指示灯

            EnableSensors = new byte[15]; // 留有预留 {温度传感器 亮度传感器  红外传感器  干接点一  干接点二   通用开关一 通用开关二 } 
            // 有六个预留
            EnableBroads = new byte[15];  // 预留广播使能位
            ArayTmpMode = new byte[40];//红外接收模式
            ParamSensors = new byte[15];  // 对应传感器参数设置
            if (IRCodes == null) IRCodes = new List<UVCMD.IRCode>();
            if (IrReceiver == null) IrReceiver = new List<IRReceive>();
            if (logic == null) logic = new List<SensorLogic>();
            if (fireset == null) fireset = new List<UVCMD.SecurityInfo>();
        }


        ///<summary>
        ///读取数据库面板设置，将所有数据读至缓存
        ///</summary>
        public void ReadSensorInfoFromDB(int DIndex)
        {
            try
            {
            }
            catch
            {
            }
        }

        ///<summary>
        ///保存数据库面板设置，将所有数据保存
        ///</summary>
        public void SaveSensorInfoToDB(int DIndex)
        {
            try
            {
            }
            catch
            {
            }
        }


        /// <summary>
        ///上传设置
        /// </summary>
        public bool UploadSensorInfosToDevice(string DevName, int DeviceType, int intActivePage, int num1, int num2)
        {
            string strMainRemark = DevName.Split('\\')[1].Trim().Split('(')[0].Trim();
            DevName = DevName.Split('\\')[0].Trim();
            //保存basic informations
            byte bytSubID = byte.Parse(DevName.Split('-')[0].ToString());
            byte bytDevID = byte.Parse(DevName.Split('-')[1].ToString());
            byte[] ArayMain = new byte[20];

            if (HDLSysPF.ModifyDeviceMainRemark(bytSubID, bytDevID, strMainRemark, DeviceType) == true)
            {
                HDLUDP.TimeBetwnNext(20);
            }
            else return false;
            byte[] ArayTmp = null;
            ///红外发射
            if (intActivePage == 0 || intActivePage == 1)
            {
                if (IRCodes != null && IRCodes.Count != 0)
                {
                    #region
                    byte byt = 0;
                    foreach (UVCMD.IRCode TmpIRcode in IRCodes)
                    {
                        byte[] arayRemark = new byte[20];
                        string strRemark = TmpIRcode.Remark1;
                        byte[] arayTmp2 = HDLUDP.StringToByte(strRemark);

                        if (arayTmp2.Length <= 20)
                            Array.Copy(arayTmp2, 0, arayRemark, 0, arayTmp2.Length);
                        else
                            Array.Copy(arayTmp2, 0, arayRemark, 0, 20);
                        byte[] araySendIR = new byte[12];
                        araySendIR[0] = byte.Parse((TmpIRcode.KeyID - 1).ToString());
                        araySendIR[1] = 0;
                        for (int K = 0; K <= 9; K++) araySendIR[2 + K] = arayRemark[K];
                        if (CsConst.mySends.AddBufToSndList(araySendIR, 0x1690, bytSubID, bytDevID, false, true, true, false) == true)
                        {
                            
                            HDLUDP.TimeBetwnNext(20);
                        }
                        else return false;

                        araySendIR[1] = 1;
                        for (int K = 0; K <= 9; K++) araySendIR[2 + K] = arayRemark[10 + K];

                        if (CsConst.mySends.AddBufToSndList(araySendIR, 0x1690, bytSubID, bytDevID, false, true, true, false) == true)
                        {
                            
                            HDLUDP.TimeBetwnNext(20);
                        }
                        else return false;
                        if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(byt * 10 / IRCodes.Count);
                        byt++;
                    }
                    #endregion
                }
            }

            //红外接收
            if (intActivePage == 0 || intActivePage == 2)
            {
                //红外接收设置
                #region
                if (IrReceiver != null && IrReceiver.Count != 0)
                {
                    for (int i = 0; i < IrReceiver.Count; i++)
                    {
                        ArayTmpMode[IrReceiver[i].BtnNum - 1] = IrReceiver[i].IRBtnModel;
                    }
                    if (CsConst.mySends.AddBufToSndList(ArayTmpMode, 0x1674, bytSubID, bytDevID, false, true, true, false) == true)
                    {
                        byte byt = 0;
                        
                        HDLUDP.TimeBetwnNext(20);
                        foreach (IRReceive oTmp in IrReceiver)
                        {
                            //修改备注
                            ArayTmp = new byte[21];
                            ArayTmp[0] = Convert.ToByte(oTmp.BtnNum);
                            string strRemark = oTmp.IRBtnRemark;
                            byte[] arayTmp2 = HDLUDP.StringToByte(strRemark);
                            if (arayTmp2.Length <= 20)
                                Array.Copy(arayTmp2, 0, ArayTmp, 1, arayTmp2.Length);
                            else
                                Array.Copy(arayTmp2, 0, ArayTmp, 1, 20);

                            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x167C, bytSubID, bytDevID, false, true, true, false) == true)
                            {
                                
                                HDLUDP.TimeBetwnNext(20);
                            }
                            else return false;
                            if (CsConst.isRestore)
                            {
                                if (oTmp.TargetInfo != null && oTmp.TargetInfo.Count != 0)
                                {
                                    foreach (UVCMD.ControlTargets TmpCmd in oTmp.TargetInfo)
                                    {
                                        if (TmpCmd.Type != 0 && TmpCmd.Type != 255)
                                        {
                                            byte[] arayCMD = new byte[9];
                                            arayCMD[0] = Convert.ToByte(oTmp.BtnNum);
                                            arayCMD[1] = byte.Parse(TmpCmd.ID.ToString());
                                            arayCMD[2] = TmpCmd.Type;
                                            arayCMD[3] = TmpCmd.SubnetID;
                                            arayCMD[4] = TmpCmd.DeviceID;
                                            arayCMD[5] = TmpCmd.Param1;
                                            arayCMD[6] = TmpCmd.Param2;
                                            arayCMD[7] = TmpCmd.Param3;   // save targets
                                            arayCMD[8] = TmpCmd.Param4;
                                            if (CsConst.mySends.AddBufToSndList(arayCMD, 0x1678, bytSubID, bytDevID, false, true, true, false) == true)
                                            {
                                                
                                                HDLUDP.TimeBetwnNext(20);
                                            }
                                            else return false;
                                        }
                                    }
                                }
                            }
                            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(byt * 10 / IRCodes.Count + 10);
                            byt++;
                        }
                    }
                    else return false;
                }
                #endregion
            }

            if (intActivePage == 0 || intActivePage == 3)
            {
                #region
                //LED灯开关设置
                #region
                ArayTmp = new byte[3];
                ArayTmp[0] = 2;
                Array.Copy(ONLEDs, 0, ArayTmp, 1, 2);
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xDB3C, bytSubID, bytDevID, false, true, true, false) == true)
                {
                    
                    HDLUDP.TimeBetwnNext(20);
                }else return false;
                #endregion
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(21);
                //修改使能位
                #region
                ArayTmp = new byte[7];
                Array.Copy(EnableSensors, 0, ArayTmp, 0, 7);
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1660, bytSubID, bytDevID, false, true, true, false) == true)
                {
                    
                    HDLUDP.TimeBetwnNext(20);
                }
                else return false;
                #endregion
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(22);
                //修改补偿值和灵敏度 
                #region
                ArayTmp = new byte[4];
                Array.Copy(ParamSensors, 0, ArayTmp, 0, 1); // 只有温度亮度
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1643, bytSubID, bytDevID, false, true, true, false) == true)//修改2014-12-04 原来0x1641 --> 0x1643
                {
                    
                    HDLUDP.TimeBetwnNext(20);
                }
                else return false;
                #endregion
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(23);
                //修改灵敏度
                #region
                ArayTmp = new byte[1];
                Array.Copy(ParamSensors, 1, ArayTmp, 0, 1);
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1670, bytSubID, bytDevID, false, true, true, false) == true)
                {
                    
                    HDLUDP.TimeBetwnNext(20);
                }
                else return false;
                #endregion
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(24);
                // 增加恒照度修改保存
                if (DeviceType == 329)
                {
                    #region
                    ArayTmp = new byte[9];
                    Array.Copy(EnableBroads, 0, ArayTmp, 0, 9);
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x16AB, bytSubID, bytDevID, false, true, true, false) == false)
                    {
                        
                        HDLUDP.TimeBetwnNext(20);
                    }
                    #endregion
                }
                #endregion
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(30);
            }

            if (intActivePage == 0 || intActivePage == 4)
            {
                //逻辑块功能设置
                #region
                // 修改24个逻辑的使能位
                if (logic != null && logic.Count != 0)
                {
                    ArayTmp = new byte[logic.Count];
                    for (byte bytJ = 0; bytJ < logic.Count; bytJ++) { ArayTmp[bytJ] = logic[bytJ].Enabled; }
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x165C, bytSubID, bytDevID, false, true, true, false) == true)
                    {

                        HDLUDP.TimeBetwnNext(20);

                        byte bytI = 1;
                        foreach (SensorLogic oLogic in logic)
                        {
                            #region
                            if (oLogic.ID == num1 || CsConst.isRestore)
                            {
                                //修改备注
                                #region
                                ArayTmp = new byte[21];
                                ArayTmp[0] = bytI;
                                string strRemark = oLogic.Remarklogic;
                                byte[] arayTmp2 = HDLUDP.StringToByte(strRemark);
                                if (arayTmp2.Length <= 20)
                                    Array.Copy(arayTmp2, 0, ArayTmp, 1, arayTmp2.Length);
                                else
                                    Array.Copy(arayTmp2, 0, ArayTmp, 1, 20);
                                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x164A, bytSubID, bytDevID, false, true, true, false) == true)
                                {
                                    
                                    HDLUDP.TimeBetwnNext(20);
                                }
                                else return false;
                                #endregion

                                //修改设置
                                #region
                                ArayTmp = new byte[21];
                                ArayTmp[0] = bytI;
                                ArayTmp[1] = oLogic.bytRelation;
                                int intTmp = 0;
                                for (byte bytJ = 0; bytJ <= 6; bytJ++) intTmp = intTmp | (oLogic.EnableSensors[bytJ] << bytJ);
                                ArayTmp[2] = (Byte)(intTmp);
                                ArayTmp[3] = (Byte)(oLogic.DelayTimeT / 256);
                                ArayTmp[4] = (Byte)(oLogic.DelayTimeT % 256);                                
                                Array.Copy(oLogic.Paramters, 0, ArayTmp, 5, 7);

                                ArayTmp[12] = 201;
                                ArayTmp[13] = 202;

                                if (oLogic.UV1 != null)
                                {
                                    ArayTmp[12] = oLogic.UV1.id;
                                    ArayTmp[13] = oLogic.UV1.condition;
                                }

                                if (oLogic.UV2 != null)
                                {
                                    ArayTmp[14] = oLogic.UV2.id;
                                    ArayTmp[15] = oLogic.UV2.condition;
                                }
                                ArayTmp[16] = Convert.ToByte(oLogic.DelayTimeF / 256);
                                ArayTmp[17] = Convert.ToByte(oLogic.DelayTimeF % 256);

                                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1652, bytSubID, bytDevID, false, true, true, false) == true)
                                {
                                    HDLUDP.TimeBetwnNext(20);
                                }
                                else return false;
                                #endregion

                                //目标配置
                                #region
                                if (CsConst.isRestore)
                                {
                                    //成立的触发目标
                                    oLogic.ModifyLogicTrueCommandsFromDevice(bytSubID, bytDevID, DeviceType);

                                    //不成立的触发目标
                                    oLogic.ModifyLogicFalseCommandsFromDevice(bytSubID, bytDevID, DeviceType);
                                }
                                #endregion
                            }
                            #endregion
                            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(30 + bytI);
                            bytI++;
                        }
                    }
                    else return false;
                }
                #endregion
            }

            if (intActivePage == 0 || intActivePage == 5)
            {
                //安防功能
                #region
                if (fireset != null && fireset.Count != 0)
                {
                    foreach (UVCMD.SecurityInfo oTmp in fireset)
                    {
                        ArayTmp = new byte[21];
                        ArayTmp[0] = oTmp.bytSeuID;
                        //修改备注

                        ArayTmp[0] = oTmp.bytSeuID;
                        string strRemark = oTmp.strRemark;
                        byte[] arayTmp2 = HDLUDP.StringToByte(strRemark);
                        Array.Copy(arayTmp2, 0, ArayTmp, 1, arayTmp2.Length);

                        if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1668, bytSubID, bytDevID, false, true, true, false) == true)
                        {
                            
                            HDLUDP.TimeBetwnNext(20);
                        }
                        else return false;

                        //修改设置
                        ArayTmp = new byte[9];
                        ArayTmp[0] = oTmp.bytSeuID;
                        ArayTmp[1] = oTmp.bytTerms;
                        ArayTmp[2] = oTmp.bytSubID;
                        ArayTmp[3] = oTmp.bytDevID;
                        ArayTmp[4] = oTmp.bytArea;

                        if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x166C, bytSubID, bytDevID, false, true, true, false) == true)
                        {
                            
                            HDLUDP.TimeBetwnNext(20);
                        }
                        else return false;
                        if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(10 * oTmp.bytSeuID / fireset.Count + 60);
                    }
                }
                #endregion
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(100);
            return true;
        }

        /// <summary>
        ///下载设置
        /// </summary>
        public bool DownloadSensorInfosToDevice(string DevName, int DeviceType, int intActivePage, int num1, int num2)
        {
            string strMainRemark = DevName.Split('\\')[1].Trim();
            DevName = DevName.Split('\\')[0].Trim();

            //保存basic informations网络信息
            byte bytSubID = byte.Parse(DevName.Split('-')[0].ToString());
            byte bytDevID = byte.Parse(DevName.Split('-')[1].ToString());
            byte[] ArayTmp = null;
            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x000E, bytSubID, bytDevID, false, true, true, false) == false)
            {
                return false;
            }
            else
            {
                byte[] arayRemark = new byte[20];
                for (int intI = 0; intI < 20; intI++) { arayRemark[intI] = CsConst.myRevBuf[25 + intI]; }
                Devname = bytSubID.ToString() + "-" + bytDevID.ToString() + "\\" + HDLPF.Byte2String(arayRemark);
                
                HDLUDP.TimeBetwnNext(1);
            }
            ///红外发射
            if (intActivePage == 0 || intActivePage == 1)
            {
                #region
                ArayTmp = new byte[2];
                IRCodes = new List<UVCMD.IRCode>();
                //读取红外有效性
                for (int i = num1; i <= num2; i++)
                {
                    UVCMD.IRCode temp = new UVCMD.IRCode();
                    temp.IRLength = 0;
                    temp.IRLoation = 0;
                    temp.KeyID = i;
                    temp.Codes = "";
                    temp.Remark1 = "";
                    temp.Enable = 0;
                    IRCodes.Add(temp);
                }
                ArayTmp = new byte[0];
                int intTmp = 0;
                byte ValidCount = 0;
                byte FirstKey = 0;
                byte LastSendCount = 0;
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x168A, bytSubID, bytDevID, false, true, true, false) == true)
                {
                    ValidCount = CsConst.myRevBuf[26];
                    FirstKey = CsConst.myRevBuf[27];
                    
                    HDLUDP.TimeBetwnNext(1);
                }
                else return false;
                if (ValidCount > 11)
                {
                    intTmp = ValidCount / 11;
                    if (ValidCount % 11 != 0)
                    {
                        intTmp = intTmp + 1;
                        LastSendCount = Convert.ToByte(ValidCount % 11);
                    }
                }
                else if (ValidCount != 0 && ValidCount <= 11) intTmp = 1;
                List<byte> listTmp = new List<byte>();
                for (int i = 0; i < intTmp; i++)
                {
                    ArayTmp = new byte[2];
                    ArayTmp[0] = FirstKey;
                    ArayTmp[1] = 11;
                    if (i == intTmp - 1 && LastSendCount != 0)
                    {
                        ArayTmp[1] = LastSendCount;
                    }
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x168C, bytSubID, bytDevID, false, true, true, false) == true)
                    {
                        FirstKey = CsConst.myRevBuf[40];
                        for (int j = 0; j < ArayTmp[1]; j++)
                        {
                            if (!listTmp.Contains(CsConst.myRevBuf[29 + j]))
                            {
                                listTmp.Add(CsConst.myRevBuf[29 + j]);
                            }
                        }
                        
                        HDLUDP.TimeBetwnNext(1);
                    }
                    else return false;

                }
                for (int i = 0; i < IRCodes.Count; i++)
                {
                    for (int j = 0; j < listTmp.Count; j++)
                    {
                        if ((IRCodes[i].KeyID - 1) == Convert.ToInt32(listTmp[j]))
                        {
                            IRCodes[i].Enable = 1;
                            break;
                        }
                    }
                }
                for (int i = 0; i < IRCodes.Count; i++)
                {
                    if (IRCodes[i].Enable == 1)
                    {
                        ArayTmp = new byte[2];
                        ArayTmp[0] = Convert.ToByte(IRCodes[i].KeyID - 1);
                        ArayTmp[1] = 0;
                        byte[] arayRemark = new byte[20];
                        if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x168E, bytSubID, bytDevID, false, true, true, false) == true)
                        {
                            Array.Copy(CsConst.myRevBuf, 29, arayRemark, 0, 10);
                            
                            HDLUDP.TimeBetwnNext(1);
                        }
                        else return false;
                        ArayTmp = new byte[2];
                        ArayTmp[0] = Convert.ToByte(IRCodes[i].KeyID - 1);
                        ArayTmp[1] = 1;
                        if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x168E, bytSubID, bytDevID, false, true, true, false) == true)
                        {
                            Array.Copy(CsConst.myRevBuf, 29, arayRemark, 10, 10);
                            
                            HDLUDP.TimeBetwnNext(1);
                        }
                        else return false;
                        IRCodes[i].Remark1 = HDLPF.Byte2String(arayRemark);
                    }
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(10 * (i + 1) / IRCodes.Count);
                }
                #endregion
                MyRead2UpFlags[0] = true;
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(10);
            // 红外接收
            if (intActivePage == 0 || intActivePage == 2)
            {
                #region
                IrReceiver = new List<IRReceive>();
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1672, bytSubID, bytDevID, false, true, true, false) == true)
                {
                    ArayTmpMode = new byte[40];
                    Array.Copy(CsConst.myRevBuf, 26, ArayTmpMode, 0, 40);
                    
                    HDLUDP.TimeBetwnNext(1);
                    for (int intI = num1; intI <= num2; intI++)
                    {
                        IRReceive oTmp = new IRReceive();
                        oTmp.BtnNum = intI;
                        oTmp.IRBtnModel = ArayTmpMode[intI - 1];
                        oTmp.TargetInfo = new List<UVCMD.ControlTargets>();
                        //读取按键备注
                        #region
                        ArayTmp = new byte[1];

                        ArayTmp[0] = Convert.ToByte(intI);
                        if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x167A, bytSubID, bytDevID, false, true, true, false) == true)
                        {
                            byte[] arayRemark = new byte[20];
                            for (int intJ = 0; intJ < 20; intJ++) { arayRemark[intJ] = CsConst.myRevBuf[27 + intJ]; }
                            oTmp.IRBtnRemark = HDLPF.Byte2String(arayRemark);
                            
                            HDLUDP.TimeBetwnNext(1);
                        }
                        else return false;
                        #endregion
                        //按键目标
                        #region
                        oTmp.TargetInfo = new List<UVCMD.ControlTargets>();
                        if (CsConst.isRestore)
                        {
                            byte bytTotalCMD = 1;
                            switch (oTmp.IRBtnModel)
                            {
                                case 0: bytTotalCMD = 1; break;
                                case 1:
                                case 2:
                                case 3:
                                case 6:
                                case 10: bytTotalCMD = 1; break;
                                case 4:
                                case 5:
                                case 7:
                                case 13: bytTotalCMD = 99; break;
                                case 11: bytTotalCMD = 98; break;
                                case 16:
                                case 17: bytTotalCMD = 49; break;
                            }
                            for (byte bytJ = 0; bytJ < bytTotalCMD; bytJ++)
                            {
                                ArayTmp = new byte[2];
                                ArayTmp[0] = (byte)(intI);
                                ArayTmp[1] = (byte)(bytJ + 1);
                                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1676, bytSubID, bytDevID, false, true, true, false) == true)
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
                                    
                                    oTmp.TargetInfo.Add(oCMD);
                                    
                                    HDLUDP.TimeBetwnNext(1);
                                }
                                else return false;
                            }
                        }
                        #endregion
                        IrReceiver.Add(oTmp);
                        if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(2 + intI / 2);
                    }
                }
                else return false;
                #endregion
                MyRead2UpFlags[1] = true;
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(20);
            //传感器设置
            if (intActivePage == 0 || intActivePage == 3 || intActivePage == 4)
            {
                #region
                if (intActivePage == 0 || intActivePage == 3)
                {
                    // read led灯使能位 
                    #region
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xDB3E, bytSubID, bytDevID, false, true, true, false) == true)
                    {
                        Array.Copy(CsConst.myRevBuf, 27, ONLEDs, 0, CsConst.myRevBuf[26]);
                        
                        HDLUDP.TimeBetwnNext(1);
                    }
                    else
                    {
                        return false;
                    }
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(21);
                    #endregion
                }
                if (intActivePage == 0 || intActivePage == 3 || intActivePage == 4)
                {
                    //读取使能位
                    #region
                    ArayTmp = null;
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x165E, bytSubID, bytDevID, false, true, true, false) == true)
                    {
                        Array.Copy(CsConst.myRevBuf, 26, EnableSensors, 0, 7);
                        
                        HDLUDP.TimeBetwnNext(1);
                    }
                    else return false;
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(23);
                    #endregion
                }
                if (intActivePage == 0 || intActivePage == 3)
                {
                    //读取温度补偿值
                    #region
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1641, bytSubID, bytDevID, false, true, true, false) == true)
                    {
                        Array.Copy(CsConst.myRevBuf, 26, ParamSensors, 0, 1);
                        
                        HDLUDP.TimeBetwnNext(1);
                    }
                    else return false;
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(24);
                    #endregion
                    //读取灵敏度
                    #region
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x166E, bytSubID, bytDevID, false, true, true, false) == true)
                    {
                        Array.Copy(CsConst.myRevBuf, 26, ParamSensors, 1, 1);
                        
                        HDLUDP.TimeBetwnNext(1);
                    }
                    else return false;
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(25);
                    #endregion
                    //恒照度读取
                    if (DeviceType == 329)
                    {
                        #region
                        if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x16A9, bytSubID, bytDevID, false, true, true, false) == true)
                        {
                            Array.Copy(CsConst.myRevBuf, 26, EnableBroads, 0, 9);
                            
                            HDLUDP.TimeBetwnNext(1);
                        }
                        else return false;
                        if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(26);
                        #endregion
                    }
                }
                #endregion
                MyRead2UpFlags[2] = true;
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(30);
            //逻辑块功能设置
            if (intActivePage == 0 || intActivePage == 4)
            {
                #region
                // 读取24个逻辑的使能位
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x165A, bytSubID, bytDevID, false, true, true, false) == true)
                {
                    logic = new List<SensorLogic>();
                    byte[] ArayTmpMode = new byte[24];
                    Array.Copy(CsConst.myRevBuf, 26, ArayTmpMode, 0, Eightin1DeviceTypeList.TotalLogic);
                    
                    HDLUDP.TimeBetwnNext(1);
                    for (int intI = 1; intI <= Eightin1DeviceTypeList.TotalLogic; intI++)
                    {
                        SensorLogic oTmp = new SensorLogic();
                        oTmp.ID = (byte)intI;
                        oTmp.EnableSensors = new byte[15];
                        oTmp.Paramters = new byte[20];
                        oTmp.Enabled = ArayTmpMode[intI - 1];
                        //无效则不读取
                        if (oTmp.Enabled != 1)
                        {
                            logic.Add(oTmp);
                            continue;
                        }
                        //读取备注
                        #region
                        ArayTmp = new byte[1];
                        ArayTmp[0] = Convert.ToByte(intI);
                        if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1648, bytSubID, bytDevID, false, true, true, false) == true)
                        {
                            byte[] arayRemark = new byte[20];
                            for (int intJ = 0; intJ < 20; intJ++) { arayRemark[intJ] = CsConst.myRevBuf[27 + intJ]; }
                            oTmp.Remarklogic = HDLPF.Byte2String(arayRemark);
                            
                            HDLUDP.TimeBetwnNext(1);
                        }
                        else return false;
                        #endregion
                        //读取设置
                        #region
                        if(CsConst.isRestore)
                        {
                            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1650, bytSubID, bytDevID, false, true, true, false) == true)
                            {
                                oTmp.bytRelation = CsConst.myRevBuf[27];
                                oTmp.EnableSensors = new byte[15];
                                int intTmp = CsConst.myRevBuf[28];
                                if (intTmp == 255) intTmp = 0;
                                //oTmp.EnableSensors
                                for (byte bytI = 0; bytI <= 6; bytI++)
                                {
                                    oTmp.EnableSensors[bytI] = Convert.ToByte((intTmp & (1 << bytI)) == (1 << bytI));
                                }
                                oTmp.DelayTimeT = CsConst.myRevBuf[29] * 256 + CsConst.myRevBuf[30];
                                Array.Copy(CsConst.myRevBuf, 31, oTmp.Paramters, 0, 7);
                                oTmp.UV1 = new UniversalSwitchSet();
                                oTmp.UV1.id = Convert.ToByte(HDLPF.IsNumStringMode(CsConst.myRevBuf[38].ToString(), 201, 248));
                                oTmp.UV1.condition = CsConst.myRevBuf[39];

                                oTmp.UV2 = new UniversalSwitchSet();
                                oTmp.UV2.id = Convert.ToByte(HDLPF.IsNumStringMode(CsConst.myRevBuf[40].ToString(), 201, 248));
                                if (oTmp.UV2.id == oTmp.UV1.id) oTmp.UV2.id = Convert.ToByte(oTmp.UV1.id + 1);
                                oTmp.UV2.condition = CsConst.myRevBuf[41];

                                oTmp.DelayTimeF = CsConst.myRevBuf[42] * 256 + CsConst.myRevBuf[43];
                                if (oTmp.DelayTimeT == 65535) oTmp.DelayTimeT = 0;
                                if (oTmp.DelayTimeF == 65535) oTmp.DelayTimeF = 0;
                                
                                HDLUDP.TimeBetwnNext(1);

                                ArayTmp = new byte[1];
                                ArayTmp[0] = oTmp.UV1.id;
                                if ((CsConst.mySends.AddBufToSndList(ArayTmp, 0x164C, bytSubID, bytDevID, false, false, true, false) == true))
                                {
                                    byte[] arayRemark = new byte[20];
                                    Array.Copy(CsConst.myRevBuf, 27, arayRemark, 0, arayRemark.Length);
                                    oTmp.UV1.remark = HDLPF.Byte2String(arayRemark);
                                    oTmp.UV1.isAutoOff = (CsConst.myRevBuf[47] == 1);
                                    if (CsConst.myRevBuf[48] * 256 + CsConst.myRevBuf[49] > 3600)
                                        oTmp.UV1.autoOffDelay = 1;
                                    else
                                        oTmp.UV1.autoOffDelay = CsConst.myRevBuf[48] * 256 + CsConst.myRevBuf[49];
                                    
                                    HDLUDP.TimeBetwnNext(1);
                                }

                                ArayTmp[0] = oTmp.UV2.id;
                                if ((CsConst.mySends.AddBufToSndList(ArayTmp, 0x164C, bytSubID, bytDevID, false, false, true, false) == true))
                                {
                                    byte[] arayRemark = new byte[20];
                                    Array.Copy(CsConst.myRevBuf, 27, arayRemark, 0, arayRemark.Length);
                                    oTmp.UV2.remark = HDLPF.Byte2String(arayRemark);
                                    oTmp.UV2.isAutoOff = (CsConst.myRevBuf[47] == 1);
                                    if (CsConst.myRevBuf[48] * 256 + CsConst.myRevBuf[49] > 3600)
                                        oTmp.UV2.autoOffDelay = 1;
                                    else
                                        oTmp.UV2.autoOffDelay = CsConst.myRevBuf[48] * 256 + CsConst.myRevBuf[49];
                                    
                                    HDLUDP.TimeBetwnNext(1);
                                }
                            }
                            else return false;
                        }
                        #endregion

                        //成立的触发目标
                        oTmp.DownloadLogicTrueCommandsFromDevice(bytSubID, bytDevID, DeviceType,0,0);
                        //不成立的触发目标
                        oTmp.DownloadLogicFalseCommandsFromDevice(bytSubID, bytDevID, DeviceType,0,0);
                        logic.Add(oTmp);
                        if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(24 + intI * 2);
                    }
                }
                else return false;
                #endregion
                MyRead2UpFlags[3] = true;
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(40);
            if (intActivePage == 0 || intActivePage == 5)
            {
                //安防设置
                #region
                fireset = new List<UVCMD.SecurityInfo>();
                ArayTmp = new byte[1];
                for (byte intI = 1; intI <= 3; intI++)
                {
                    ArayTmp[0] = intI;
                    //读取备注
                    #region
                    UVCMD.SecurityInfo oTmp = new UVCMD.SecurityInfo();
                    oTmp.bytSeuID = intI;

                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1666, bytSubID, bytDevID, false, true, true, false) == true)
                    {
                        byte[] arayRemark = new byte[20];
                        for (int intJ = 0; intJ < 20; intJ++) { arayRemark[intJ] = CsConst.myRevBuf[27 + intJ]; }
                        oTmp.strRemark = HDLPF.Byte2String(arayRemark);
                        
                        HDLUDP.TimeBetwnNext(1);
                    }
                    else return false;

                    #endregion
                    //读取设置
                    #region
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x166A, bytSubID, bytDevID, false, true, true, false) == true)
                    {
                        oTmp.bytTerms = CsConst.myRevBuf[27];
                        oTmp.bytSubID = CsConst.myRevBuf[28];
                        oTmp.bytDevID = CsConst.myRevBuf[29];
                        oTmp.bytArea = CsConst.myRevBuf[30];
                        
                        HDLUDP.TimeBetwnNext(1);
                    }
                    else return false;
                    fireset.Add(oTmp);
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(40 + (10 * intI / 4));
                    #endregion
                }
                #endregion
                MyRead2UpFlags[4] = true;
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(50);
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(100);
            return true;
        }

    }
}
