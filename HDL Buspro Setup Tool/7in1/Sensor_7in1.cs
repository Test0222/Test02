using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HDL_Buspro_Setup_Tool
{
     [Serializable]
    public class Sensor_7in1 : HdlDeviceBackupAndRestore
    {
        //基本信息
        public int DIndex;//设备唯一编号
        public string Devname;//子网ID 设备ID 备注
        public byte[] ONLEDs = new byte[5]; // 红外LED灯  红外超声波
        public byte[] EnableSensors = new byte[15]; // 留有预留 {温度传感器 亮度传感器  预留 预留 预留 红外传感器  干接点一  干接点二   通用开关一 通用开关二  以逻辑块为条件 预留 预留 预留 预留} 
        public byte[] EnableBroads = new byte[15];  //恒照度使能,恒照度亮度两个byte,kp两个byte,ki两个byte,周期,低限
        public byte[] ParamSensors = new byte[15];  //温度补偿值(第一个byte)，红外灵敏度(第十三个byte)
        public byte[] SimulateEnable = new byte[15];//模拟测试使能 温度传感器模拟使能 亮度传感器模拟使能 红外传感器模拟使能
        public byte[] ParamSimulate = new byte[15];//温度模拟值 亮度模拟值，红外模拟值
        public List<Logic> logic;
        public List<UVCMD.SecurityInfo> fireset;
        public bool[] MyRead2UpFlags = new bool[10];

        //通用开关
        public class UVSet
        {
            public byte UvNum;//开关号
            public string UvRemark;//备注
            public byte UVCondition;
            public bool AutoOff;//自动关闭
            public int OffTime;//自动关闭时间
            public byte state;
        }

        //逻辑关系
        public class Logic
        {
            public byte ID;
            public string Remarklogic;//逻辑备注
            public byte Enabled;
            public byte bytRelation; // 关系
            public byte[] EnableSensors = new byte[15]; // 温度传感器 亮度传感器 预留 预留 预留 红外传感器   干接点一  干接点二   通用开关一 通用开关二  以逻辑块为条件}   // 有六个预留
            public byte[] Paramters = new byte[20];//最低温度，最高温度，最低亮度(2个)，最高亮度(2个)，红外传感器(第13个byte)，干接点一(第14个byte) 干接点二(第15个byte) 逻辑号(第16个byte) 逻辑条件(第17个byte)
            public UVSet UV1;//通用开关1
            public UVSet UV2;//通用开关2
            public List<UVCMD.ControlTargets> SetUp;//成立目标
            public List<UVCMD.ControlTargets> NoSetUp;//不成立目标
            public int DelayTimeT;//成立延迟时间
            public int DelayTimeF;//不成立延迟时间

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
            ParamSensors = new byte[15];  // 对应传感器参数设置
            if (logic == null) logic = new List<Logic>();
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
        public void SaveSensorInfoToDB()
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
        public bool UploadSensorInfosToDevice(string DevName, int intActivePage,int devicetype, int num1, int num2)// 0 mean all, else that tab only
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

            if (CsConst.mySends.AddBufToSndList(ArayMain, 0x0010, bytSubID, bytDevID, false, true, true, false) == true)
            {
                
                HDLUDP.TimeBetwnNext(20);
            }
            else return false;
            byte[] ArayTmp = null;
            if (intActivePage == 0 || intActivePage == 1)
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
                }
                else return false;
                #endregion
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(2);

                //修改使能位
                #region
                ArayTmp = new byte[12];
                Array.Copy(EnableSensors, 0, ArayTmp, 0, 12);
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x161E, bytSubID, bytDevID, false, true, true, false) == true)
                {
                    
                    HDLUDP.TimeBetwnNext(20);
                }
                else return false;
                #endregion
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(3);
               
                //修改补偿值和灵敏度 
                #region
                ArayTmp = new byte[14];
                Array.Copy(ParamSensors, 0, ArayTmp, 0, 14); 
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1602, bytSubID, bytDevID, false, true, true, false) == true)
                {
                    
                    HDLUDP.TimeBetwnNext(20);
                }
                else return false;
                #endregion
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(4);

                // 增加恒照度修改保存
                #region
                if (devicetype == 328)
                {
                    ArayTmp = new byte[9];
                    Array.Copy(EnableBroads, 0, ArayTmp, 0, 9);
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x16AB, bytSubID, bytDevID, false, true, true, false) == true)
                    {
                        
                        HDLUDP.TimeBetwnNext(20);
                    }
                    else return false;
                }
                #endregion
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(5);
               
                #endregion
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(10);
            if (intActivePage == 0 || intActivePage == 2)
            {
                #region
                // 修改24个逻辑的使能位
                if (logic != null && logic.Count != 0)
                {
                    ArayTmp = new byte[logic.Count];
                    for (byte bytJ = 0; bytJ < logic.Count; bytJ++) { ArayTmp[bytJ] = logic[bytJ].Enabled; }
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x161A, bytSubID, bytDevID, false, true, true, false) == true)
                    {
                        
                        HDLUDP.TimeBetwnNext(20);

                        byte bytI = 1;
                        foreach (Logic oLogic in logic)
                        {
                            #region
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
                            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1608, bytSubID, bytDevID, false, true, true, false) == true)
                            {
                                
                                HDLUDP.TimeBetwnNext(20);
                            }
                            else return false;
                            #endregion
                            if (oLogic.ID == num1 || CsConst.isRestore)
                            {
                                //修改设置
                                #region
                                ArayTmp = new byte[33];
                                ArayTmp[0] = bytI;
                                ArayTmp[1] = oLogic.bytRelation;
                                int intTmp = 0;
                                for (byte bytJ = 0; bytJ <= 10; bytJ++) intTmp = intTmp | (oLogic.EnableSensors[bytJ] << bytJ);
                                ArayTmp[2] = (Byte)(intTmp / 256);
                                ArayTmp[3] = (Byte)(intTmp % 256);
                                ArayTmp[4] = (Byte)(oLogic.DelayTimeT / 256);
                                ArayTmp[5] = (Byte)(oLogic.DelayTimeT % 256);
                                ArayTmp[6] = (Byte)(oLogic.DelayTimeF / 256);
                                ArayTmp[7] = (Byte)(oLogic.DelayTimeF % 256);
                                Array.Copy(oLogic.Paramters, 0, ArayTmp, 8, 15);
                                ArayTmp[14] = 0x14;
                                ArayTmp[15] = 0x14;
                                for (int byt = 16; byt <= 19; byt++)
                                    ArayTmp[byt] = 0;
                                ArayTmp[23] = 201;
                                ArayTmp[24] = 202;

                                if (oLogic.UV1 != null)
                                {
                                    ArayTmp[23] = oLogic.UV1.UvNum;
                                    ArayTmp[24] = oLogic.UV1.UVCondition;
                                }

                                if (oLogic.UV2 != null)
                                {
                                    ArayTmp[25] = oLogic.UV2.UvNum;
                                    ArayTmp[26] = oLogic.UV2.UVCondition;
                                }
                                ArayTmp[27] = oLogic.Paramters[15];
                                ArayTmp[28] = oLogic.Paramters[16];

                                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1610, bytSubID, bytDevID, false, true, true, false) == true)
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
                                    if (oLogic.SetUp != null && oLogic.SetUp.Count != 0)
                                    {
                                        foreach (UVCMD.ControlTargets TmpCmd in oLogic.SetUp)
                                        {
                                            byte[] arayCMD = new byte[9];
                                            arayCMD[0] = bytI;
                                            arayCMD[1] = byte.Parse(TmpCmd.ID.ToString());
                                            arayCMD[2] = TmpCmd.Type;
                                            arayCMD[3] = TmpCmd.SubnetID;
                                            arayCMD[4] = TmpCmd.DeviceID;
                                            arayCMD[5] = TmpCmd.Param1;
                                            arayCMD[6] = TmpCmd.Param2;
                                            arayCMD[7] = TmpCmd.Param3;   // save targets
                                            arayCMD[8] = TmpCmd.Param4;
                                            if (CsConst.mySends.AddBufToSndList(arayCMD, 0x1614, bytSubID, bytDevID, false, true, true, false) == true)
                                            {
                                                
                                                HDLUDP.TimeBetwnNext(20);
                                            }
                                            else return false;
                                        }
                                    }

                                    //不成立的触发目标
                                    if (oLogic.NoSetUp != null && oLogic.NoSetUp.Count != 0)
                                    {
                                        foreach (UVCMD.ControlTargets TmpCmd in oLogic.NoSetUp)
                                        {
                                            byte[] arayCMD = new byte[9];
                                            arayCMD[0] = Convert.ToByte(bytI + 24);
                                            arayCMD[1] = byte.Parse(TmpCmd.ID.ToString());
                                            arayCMD[2] = TmpCmd.Type;
                                            arayCMD[3] = TmpCmd.SubnetID;
                                            arayCMD[4] = TmpCmd.DeviceID;
                                            arayCMD[5] = TmpCmd.Param1;
                                            arayCMD[6] = TmpCmd.Param2;
                                            arayCMD[7] = TmpCmd.Param3;   // save targets
                                            arayCMD[8] = TmpCmd.Param4;
                                            if (CsConst.mySends.AddBufToSndList(arayCMD, 0x16146, bytSubID, bytDevID, false, true, true, false) == true)
                                            {
                                                
                                                HDLUDP.TimeBetwnNext(20);
                                            }
                                            else return false;
                                        }
                                    }
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
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(20);
            if (intActivePage == 0 || intActivePage == 3)
            {
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

                        if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1626, bytSubID, bytDevID, false, true, true, false) == true)
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

                        if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x162A, bytSubID, bytDevID, false, true, true, false) == true)
                        {
                            
                            HDLUDP.TimeBetwnNext(20);
                        }
                        else return false;
                        if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(10 * oTmp.bytSeuID / fireset.Count + 60);
                    }
                }
                #endregion
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(30);
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(100);
            return true;
        }

        /// <summary>
        ///下载设置
        /// </summary>
        public bool DownloadSensorInfosToDevice(string DevName, int intActivePage, int wdDeviceType, int num1, int num2) // 0 mean all, else that tab only
        {
            string strMainRemark = DevName.Split('\\')[1].Trim();
            DevName = DevName.Split('\\')[0].Trim();

            //保存basic informations网络信息
            byte bytSubID = byte.Parse(DevName.Split('-')[0].ToString());
            byte bytDevID = byte.Parse(DevName.Split('-')[1].ToString());
            byte[] ArayTmp = null;
            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x000E, bytSubID, bytDevID, false, true, true, false) == true)
            {
                byte[] arayRemark = new byte[20];
                for (int intI = 0; intI < 20; intI++) { arayRemark[intI] = CsConst.myRevBuf[25 + intI]; }
                Devname = bytSubID.ToString() + "-" + bytDevID.ToString() + "\\" + HDLPF.Byte2String(arayRemark);
                
                HDLUDP.TimeBetwnNext(1);
            }
            else return false;
            

            //传感器设置
            if (intActivePage == 0 || intActivePage == 1 || intActivePage == 2)
            {
                #region
                if (intActivePage == 0 || intActivePage == 1)
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
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(1);
                    #endregion
                }
                if (intActivePage == 0 || intActivePage == 1 || intActivePage == 2)
                {
                    //读取使能位
                    #region
                    ArayTmp = null;
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x161C, bytSubID, bytDevID, false, true, true, false) == true)
                    {
                        Array.Copy(CsConst.myRevBuf, 26, EnableSensors, 0, 12);
                        
                        HDLUDP.TimeBetwnNext(1);
                    }
                    else return false;
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(2);
                    #endregion
                }
                if (intActivePage == 0 || intActivePage == 1)
                {
                    //读取补偿值 十二合一额外命令读写
                    #region
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1600, bytSubID, bytDevID, false, true, true, false) == true)
                    {
                        Array.Copy(CsConst.myRevBuf, 26, ParamSensors, 0, 14);
                        
                        HDLUDP.TimeBetwnNext(1);
                    }
                    else return false;
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(3);
                    #endregion
                    
                    //恒照度读取
                    #region
                    if (wdDeviceType == 328)
                    {
                        EnableBroads = new byte[15];
                        if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x16A9, bytSubID, bytDevID, false, true, true, false) == true)
                        {
                            Array.Copy(CsConst.myRevBuf, 26, EnableBroads, 0, 9);
                            
                            HDLUDP.TimeBetwnNext(1);
                        }
                        else return false;
                    }
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(5);
                    #endregion
                }
                #endregion
                MyRead2UpFlags[0] = true;
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(10);
            //逻辑块功能设置
            if (intActivePage == 0 || intActivePage == 2)
            {
                #region
                // 读取24个逻辑的使能位
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1618, bytSubID, bytDevID, false, true, true, false) == true)
                {
                    logic = new List<Logic>();
                    byte[] ArayTmpMode = new byte[24];
                    Array.Copy(CsConst.myRevBuf, 26, ArayTmpMode, 0, 24);
                    
                    HDLUDP.TimeBetwnNext(1);
                    for (int intI = 1; intI <= 24; intI++)
                    {
                        Logic oTmp = new Logic();
                        oTmp.ID = (byte)intI;
                        oTmp.Enabled = ArayTmpMode[intI - 1];
                        oTmp.UV1 = new UVSet();
                        oTmp.UV2 = new UVSet();
                        //读取备注
                        #region
                        ArayTmp = new byte[1];
                        ArayTmp[0] = Convert.ToByte(intI);
                        if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1606, bytSubID, bytDevID, false, true, true, false) == true)
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
                        if (CsConst.isRestore)
                        {
                            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x160E, bytSubID, bytDevID, false, true, true, false) == true)
                            {
                                oTmp.bytRelation = CsConst.myRevBuf[27];
                                oTmp.EnableSensors = new byte[15];
                                oTmp.Paramters = new byte[20];
                                int intTmp = CsConst.myRevBuf[28] * 256 + CsConst.myRevBuf[29];
                                if (intTmp == 65535) intTmp = 0;
                                //oTmp.EnableSensors
                                for (byte bytI = 0; bytI <= 10; bytI++)
                                {
                                    oTmp.EnableSensors[bytI] = Convert.ToByte((intTmp & (1 << bytI)) == (1 << bytI));
                                }
                                oTmp.DelayTimeT = CsConst.myRevBuf[30] * 256 + CsConst.myRevBuf[31];
                                oTmp.DelayTimeF = CsConst.myRevBuf[32] * 256 + CsConst.myRevBuf[33];
                                if (oTmp.DelayTimeT == 65535) oTmp.DelayTimeT = 0;
                                if (oTmp.DelayTimeF == 65535) oTmp.DelayTimeF = 0;
                                Array.Copy(CsConst.myRevBuf, 34, oTmp.Paramters, 0, 15);
                                oTmp.UV1 = new UVSet();
                                oTmp.UV1.UvNum = Convert.ToByte(HDLPF.IsNumStringMode(CsConst.myRevBuf[49].ToString(), 201, 248));
                                oTmp.UV1.UVCondition = CsConst.myRevBuf[50];

                                oTmp.UV2 = new UVSet();
                                oTmp.UV2.UvNum = Convert.ToByte(HDLPF.IsNumStringMode(CsConst.myRevBuf[51].ToString(), 201, 248));
                                if (oTmp.UV2.UvNum == oTmp.UV1.UvNum) oTmp.UV2.UvNum = Convert.ToByte(oTmp.UV1.UvNum + 1);
                                oTmp.UV2.UVCondition = CsConst.myRevBuf[52];
                                oTmp.Paramters[15] = CsConst.myRevBuf[53];
                                oTmp.Paramters[16] = CsConst.myRevBuf[54];
                                
                                HDLUDP.TimeBetwnNext(1);
                                if (CsConst.isRestore)
                                {
                                    ArayTmp = new byte[1];
                                    ArayTmp[0] = oTmp.UV1.UvNum;
                                    if ((CsConst.mySends.AddBufToSndList(ArayTmp, 0x160A, bytSubID, bytDevID, false, false, true, false) == true))
                                    {
                                        byte[] arayRemark = new byte[20];
                                        Array.Copy(CsConst.myRevBuf, 27, arayRemark, 0, arayRemark.Length);
                                        oTmp.UV1.UvRemark = HDLPF.Byte2String(arayRemark);
                                        oTmp.UV1.AutoOff = (CsConst.myRevBuf[47] == 1);
                                        if (CsConst.myRevBuf[48] * 256 + CsConst.myRevBuf[49] > 3600)
                                            oTmp.UV1.OffTime = 1;
                                        else
                                            oTmp.UV1.OffTime = CsConst.myRevBuf[48] * 256 + CsConst.myRevBuf[49];
                                        
                                        HDLUDP.TimeBetwnNext(1);
                                    }

                                    ArayTmp[0] = oTmp.UV2.UvNum;
                                    if ((CsConst.mySends.AddBufToSndList(ArayTmp, 0x160A, bytSubID, bytDevID, false, false, true, false) == true))
                                    {
                                        byte[] arayRemark = new byte[20];
                                        Array.Copy(CsConst.myRevBuf, 27, arayRemark, 0, arayRemark.Length);
                                        oTmp.UV2.UvRemark = HDLPF.Byte2String(arayRemark);
                                        oTmp.UV2.AutoOff = (CsConst.myRevBuf[47] == 1);
                                        if (CsConst.myRevBuf[48] * 256 + CsConst.myRevBuf[49] > 3600)
                                            oTmp.UV2.OffTime = 1;
                                        else
                                            oTmp.UV2.OffTime = CsConst.myRevBuf[48] * 256 + CsConst.myRevBuf[49];
                                        
                                        HDLUDP.TimeBetwnNext(1);
                                    }
                                }
                            }
                            else return false;
                        }
                        #endregion
                        //成立的触发目标
                        #region
                        oTmp.SetUp = new List<UVCMD.ControlTargets>();
                        if (CsConst.isRestore)
                        {
                            for (byte bytJ = 0; bytJ < 20; bytJ++)
                            {
                                ArayTmp = new byte[2];
                                ArayTmp[0] = (byte)(intI);
                                ArayTmp[1] = (byte)(bytJ + 1);
                                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1612, bytSubID, bytDevID, false, true, true, false) == true)
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
                                    
                                    oTmp.SetUp.Add(oCMD);
                                    
                                    HDLUDP.TimeBetwnNext(1);
                                }
                                else return false;
                            }
                        }
                        #endregion
                        //不成立的触发目标
                        #region
                        oTmp.NoSetUp = new List<UVCMD.ControlTargets>();
                        if (CsConst.isRestore)
                        {
                            for (byte bytJ = 0; bytJ < 20; bytJ++)
                            {
                                ArayTmp = new byte[2];
                                ArayTmp[0] = (byte)(intI + 24);
                                ArayTmp[1] = (byte)(bytJ + 1);

                                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1612, bytSubID, bytDevID, false, true, true, false) == true)
                                {
                                    UVCMD.ControlTargets oCMD = new UVCMD.ControlTargets();
                                    oCMD.ID = CsConst.myRevBuf[27];
                                    oCMD.Type = CsConst.myRevBuf[28];
                                    oCMD.SubnetID = CsConst.myRevBuf[29];
                                    oCMD.DeviceID = CsConst.myRevBuf[30];
                                    oCMD.Param1 = CsConst.myRevBuf[31];
                                    oCMD.Param2 = CsConst.myRevBuf[32];
                                    oCMD.Param3 = CsConst.myRevBuf[33];
                                    oCMD.Param4 = CsConst.myRevBuf[34];
                                    
                                    oTmp.NoSetUp.Add(oCMD);
                                    
                                    HDLUDP.TimeBetwnNext(1);
                                }
                                else return false;
                            }
                        }
                        #endregion
                        logic.Add(oTmp);
                        if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(10 + intI * 10 / 24);
                    }
                }
                else return false;
                #endregion
                MyRead2UpFlags[1] = true;
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(20);
            if (intActivePage == 0 || intActivePage == 3)
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

                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1624, bytSubID, bytDevID, false, true, true, false) == true)
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
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1628, bytSubID, bytDevID, false, true, true, false) == true)
                    {
                        oTmp.bytTerms = CsConst.myRevBuf[27];
                        oTmp.bytSubID = CsConst.myRevBuf[28];
                        oTmp.bytDevID = CsConst.myRevBuf[29];
                        oTmp.bytArea = CsConst.myRevBuf[30];
                        
                        HDLUDP.TimeBetwnNext(1);
                    }
                    else return false;
                    fireset.Add(oTmp);
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(20 + (10 * intI / 3));
                    #endregion
                }
                #endregion
                MyRead2UpFlags[2] = true;
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(30);
            if (intActivePage == 0 || intActivePage == 4)
            {
                //模拟信息
                #region
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1620, bytSubID, bytDevID, false, true, true, false) == true)
                {
                    SimulateEnable = new byte[15];
                    SimulateEnable[0] = CsConst.myRevBuf[26];
                    SimulateEnable[1] = CsConst.myRevBuf[27];
                    SimulateEnable[2] = CsConst.myRevBuf[28];
                    SimulateEnable[3] = CsConst.myRevBuf[32];
                    ParamSimulate = new byte[15];
                    ParamSimulate[0] = CsConst.myRevBuf[33];
                    ParamSimulate[1] = CsConst.myRevBuf[34];
                    ParamSimulate[2] = CsConst.myRevBuf[35];
                    ParamSimulate[3] = CsConst.myRevBuf[39];
                    
                    HDLUDP.TimeBetwnNext(1);
                }
                else return false;
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(31);
                #endregion
                MyRead2UpFlags[3] = true;
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(40);
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(100);
            return true;
        }
    }
}
