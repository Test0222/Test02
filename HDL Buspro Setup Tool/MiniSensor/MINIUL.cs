using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HDL_Buspro_Setup_Tool
{
    [Serializable]
    public class MiniSensor : HdlDeviceBackupAndRestore
    {
        //基本信息
        public int DIndex;//设备唯一编号
        public string Devname;//子网ID 设备ID 备注
        public byte[] ONLEDs = new byte[5];  // 超声波指示灯
        public byte[] EnableSensors = new byte[15]; // 留有预留 {预留 亮度传感器  预留 超声波传感器  预留  通用开关一 通用开关二 逻辑} 
        public byte[] ParamSensors = new byte[15];  // 对应传感器参数设置 预留 预留，预留，超声波灵敏度 
        public byte[] SimulateEnable = new byte[15];//模拟测试使能 预留 亮度传感器模拟使能 预留 超声波传感器模拟使能
        public byte[] ParamSimulate = new byte[15];//预留 亮度模拟值，预留，超声波模拟值
        public byte[] EnableBroads = new byte[15];  //恒照度使能,恒照度亮度两个byte,kp两个byte,ki两个byte,周期,低限
        public List<UVCMD.SecurityInfo> fireset;
        public List<SensorLogic> logic;
        public bool[] MyRead2UpFlags = new bool[6];

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

        ///<summary>
        ///读取默认的设置
        ///</summary>
        public void ReadDefaultInfo()
        {

        }

        /// <summary>
        ///上传设置
        /// </summary>
        public void UploadMSPUInfoToDevice(string DevName, int DeviceType, int intActivePage, int num1, int num2)
        {
            string strMainRemark = DevName.Split('\\')[1].Trim().Split('(')[0].Trim();
            DevName = DevName.Split('\\')[0].Trim();
            //保存回路信息
            byte bytSubID = byte.Parse(DevName.Split('-')[0].ToString());
            byte bytDevID = byte.Parse(DevName.Split('-')[1].ToString());

            byte[] arayRemark = new byte[21];// used for restore remark

            if ( HDLSysPF.ModifyDeviceMainRemark(bytSubID, bytDevID, strMainRemark, DeviceType))
            {
                HDLUDP.TimeBetwnNext(20);
            }
            else return;
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(5);
            byte[] ArayTmp = null;
            if (intActivePage == 0 || intActivePage == 1)
            {
                #region
                //LED状态
                ArayTmp = new byte[3];
                ArayTmp[0] = 2;
                ArayTmp[1] = ONLEDs[0];
                ArayTmp[2] = ONLEDs[1];
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xDB3C, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                {
                    
                    HDLUDP.TimeBetwnNext(20);
                }
                else return;
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(6);
                //传感器使能
                ArayTmp = new byte[12];
                ArayTmp[1] = EnableSensors[1];
                ArayTmp[11] = EnableSensors[3];
                ArayTmp[8] = EnableSensors[5];
                ArayTmp[9] = EnableSensors[6];
                ArayTmp[10] = EnableSensors[7];

                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x161E, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                {
                    
                    HDLUDP.TimeBetwnNext(20);
                }
                else return;
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(7);
                //补偿值
                ArayTmp = new byte[14];
                ArayTmp[0] = ParamSensors[0];
                ArayTmp[1] = ParamSensors[1];
                ArayTmp[6] = 2;
                ArayTmp[7] = 5;
                ArayTmp[8] = 2;
                ArayTmp[9] = 2;
                ArayTmp[10] = 2;
                ArayTmp[12] = ParamSensors[2];
                ArayTmp[13] = ParamSensors[3];
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1602, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                {
                    
                    HDLUDP.TimeBetwnNext(20);
                }
                else return;
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(8);
                // 增加恒照度修改保存
                if (DeviceType == 326)
                {
                    ArayTmp = new byte[9];
                    Array.Copy(EnableBroads, 0, ArayTmp, 0, 9);
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x16AB, bytSubID, bytDevID, false, true, true, false) == true)
                    {
                        
                        HDLUDP.TimeBetwnNext(20);
                    }
                    else return;
                }
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(9);
                #endregion
            }

            if (intActivePage == 0 || intActivePage == 2)
            {
                //逻辑块功能设置
                #region
                // 修改24个逻辑的使能位
                if (logic != null && logic.Count != 0)
                {
                    ArayTmp = new byte[logic.Count];
                    for (byte bytJ = 0; bytJ < logic.Count - 1; bytJ++) { ArayTmp[bytJ] = logic[bytJ].Enabled; }
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x161A, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                    {
                        if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(10);
                        
                        HDLUDP.TimeBetwnNext(20);
                        byte bytI = 1;
                        foreach (SensorLogic oLogic in logic)
                        {
                            #region

                            //修改备注
                            ArayTmp = new byte[21];
                            ArayTmp[0] = bytI;
                            string strRemark = oLogic.Remarklogic;
                            if (strRemark == null) strRemark = "";
                            byte[] arayTmp2 = HDLUDP.StringToByte(strRemark);
                            Array.Copy(arayTmp2, 0, ArayTmp, 1, arayTmp2.Length);
                            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1608, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                            {
                                
                                HDLUDP.TimeBetwnNext(20);
                            }
                            else return;
                            if (oLogic.ID == num1 || CsConst.isRestore)
                            {
                                //修改设置
                                ArayTmp = new byte[30];
                                ArayTmp[0] = bytI;
                                ArayTmp[1] = oLogic.bytRelation;
                                int intTmp = 0;
                                for (int i = 0; i < 8; i++)
                                {
                                    switch (i)
                                    {
                                        case 0: intTmp = intTmp | (oLogic.EnableSensors[i] << 0); break;
                                        case 1: intTmp = intTmp | (oLogic.EnableSensors[i] << 1); break;
                                        case 2: intTmp = intTmp | (oLogic.EnableSensors[i] << 5); break;
                                        case 3: intTmp = intTmp | (oLogic.EnableSensors[i] << 11); break;
                                        case 4: intTmp = intTmp | (oLogic.EnableSensors[i] << 6); break;
                                        case 5: intTmp = intTmp | (oLogic.EnableSensors[i] << 8); break;
                                        case 6: intTmp = intTmp | (oLogic.EnableSensors[i] << 9); break;
                                        case 7: intTmp = intTmp | (oLogic.EnableSensors[i] << 10); break;
                                    }
                                }
                                ArayTmp[2] = (Byte)(intTmp / 256);
                                ArayTmp[3] = (Byte)(intTmp % 256);
                                ArayTmp[4] = (Byte)(oLogic.DelayTimeT / 256);
                                ArayTmp[5] = (Byte)(oLogic.DelayTimeT % 256);
                                ArayTmp[6] = (Byte)(oLogic.DelayTimeF / 256);
                                ArayTmp[7] = (Byte)(oLogic.DelayTimeF % 256);
                                Array.Copy(oLogic.Paramters, 0, ArayTmp, 8, 6);
                                ArayTmp[14] = 20;
                                ArayTmp[15] = 20;

                                ArayTmp[20] = 0;
                                ArayTmp[29] = oLogic.Paramters[7];
                                ArayTmp[21] = 0;

                                if (oLogic.UV1 != null)
                                {
                                    ArayTmp[23] = oLogic.UV1.id;
                                    ArayTmp[24] = oLogic.UV1.condition;
                                }

                                if (oLogic.UV2 != null)
                                {
                                    ArayTmp[25] = oLogic.UV2.id;
                                    ArayTmp[26] = oLogic.UV2.condition;
                                }
                                ArayTmp[27] = oLogic.Paramters[9];
                                ArayTmp[28] = oLogic.Paramters[10];

                                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1610, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                                {
                                    
                                    HDLUDP.TimeBetwnNext(20);
                                }
                                else return;
                                //成立的触发目标
                                if (CsConst.isRestore)
                                {
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
                                            if (CsConst.mySends.AddBufToSndList(arayCMD, 0x1614, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                                            {
                                                
                                                HDLUDP.TimeBetwnNext(20);
                                            }
                                            else return;

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
                                            if (CsConst.mySends.AddBufToSndList(arayCMD, 0x1614, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                                            {
                                                
                                                HDLUDP.TimeBetwnNext(20);
                                            }
                                            else return;
                                        }
                                    }
                                }
                            }
                            #endregion
                            bytI++;
                            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(10 + bytI);
                        }
                    }
                    else return;
                }
                #endregion
            }

            if (intActivePage == 0 || intActivePage == 3)
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
                        if (strRemark == null) strRemark = "";
                        byte[] arayTmp2 = HDLUDP.StringToByte(strRemark);
                        Array.Copy(arayTmp2, 0, ArayTmp, 1, arayTmp2.Length);

                        if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1626, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                        {
                            
                            HDLUDP.TimeBetwnNext(20);
                        }
                        else return;

                        //修改设置
                        ArayTmp = new byte[9];
                        ArayTmp[0] = oTmp.bytSeuID;
                        ArayTmp[1] = oTmp.bytTerms;
                        ArayTmp[2] = oTmp.bytSubID;
                        ArayTmp[3] = oTmp.bytDevID;
                        ArayTmp[4] = oTmp.bytArea;

                        if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x162A, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                        {
                            
                            HDLUDP.TimeBetwnNext(20);
                        }
                        else return;
                        if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(40);
                    }
                }
                #endregion
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(100);
        }

        /// <summary>
        ///下载设置
        /// </summary>
        public void DownloadMSPUInfoToDevice(string DevName, int DeviceType, int intActivePage)
        {
            string strMainRemark = DevName.Split('\\')[1].Trim();
            DevName = DevName.Split('\\')[0].Trim();

            byte bytSubID = byte.Parse(DevName.Split('-')[0].ToString());
            byte bytDevID = byte.Parse(DevName.Split('-')[1].ToString());
            int wdMaxValue = DeviceTypeList.GetMaxValueFromPublicModeGroup(DeviceType);
            byte[] ArayTmp = new byte[0];
            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x000E, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
            {
                byte[] arayRemark = new byte[20];
                for (int intI = 0; intI < 20; intI++) { arayRemark[intI] = CsConst.myRevBuf[25 + intI]; }
                Devname = bytSubID.ToString() + "-" + bytDevID.ToString() + "\\" + HDLPF.Byte2String(arayRemark);
                
                HDLUDP.TimeBetwnNext(1);
            }
            else return;

            if (intActivePage == 0 || intActivePage == 1)
            {
                #region
                ONLEDs = new byte[5];
                //读取LED状态
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xDB3E, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                {
                    ONLEDs = new byte[5];
                    ONLEDs[0] = CsConst.myRevBuf[27];
                    
                    HDLUDP.TimeBetwnNext(1);
                }
                else return;
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(1, null);
                //读取传感器使能
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x161C, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                {
                    EnableSensors = new byte[15];
                    EnableSensors[0] = CsConst.myRevBuf[26];
                    EnableSensors[1] = CsConst.myRevBuf[27];
                    EnableSensors[2] = CsConst.myRevBuf[31];
                    EnableSensors[3] = CsConst.myRevBuf[37];
                    EnableSensors[4] = CsConst.myRevBuf[32];
                    EnableSensors[5] = CsConst.myRevBuf[34];
                    EnableSensors[6] = CsConst.myRevBuf[35];
                    EnableSensors[7] = CsConst.myRevBuf[36];
                    
                    HDLUDP.TimeBetwnNext(1);
                }
                else return;
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(2, null);
                //读取补偿值
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1600, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                {
                    ParamSensors = new byte[15];
                    ParamSensors[0] = CsConst.myRevBuf[26];
                    ParamSensors[1] = CsConst.myRevBuf[27];
                    ParamSensors[2] = CsConst.myRevBuf[38];
                    ParamSensors[3] = CsConst.myRevBuf[39];
                    
                    HDLUDP.TimeBetwnNext(1);
                }
                else return;
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(3);
                //恒照度读取
                if (DeviceType == 326)
                {
                    EnableBroads = new byte[15];
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x16A9, bytSubID, bytDevID, false, true, true, false) == true)
                    {
                        Array.Copy(CsConst.myRevBuf, 26, EnableBroads, 0, 9);
                        
                        HDLUDP.TimeBetwnNext(1);
                    }
                    else return;
                }
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(4);
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(5, null);
                #endregion
                MyRead2UpFlags[0] = true;
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(10);
            if (intActivePage == 0 || intActivePage == 2)
            {
                //逻辑块功能设置
                #region
                // 读取24个逻辑的使能位
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1618, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                {
                    logic = new List<SensorLogic>();
                    byte[] ArayTmpMode = new byte[24];
                    Array.Copy(CsConst.myRevBuf, 26, ArayTmpMode, 0, 24);
                    
                    HDLUDP.TimeBetwnNext(1);
                    for (int intI = 1; intI <= 24; intI++)
                    {
                        SensorLogic oTmp = new SensorLogic();
                        oTmp.ID = (byte)intI;
                        oTmp.Enabled = ArayTmpMode[intI - 1];
                        oTmp.SetUp = new List<UVCMD.ControlTargets>();
                        oTmp.NoSetUp = new List<UVCMD.ControlTargets>();
                        if (oTmp.Enabled != 1)
                        {
                            logic.Add(oTmp);
                            continue;
                        }
                        #region

                        //读取备注
                        ArayTmp = new byte[1];
                        ArayTmp[0] = Convert.ToByte(intI);

                        if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1606, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                        {
                            byte[] arayRemark = new byte[20];
                            for (int intJ = 0; intJ < 20; intJ++) { arayRemark[intJ] = CsConst.myRevBuf[27 + intJ]; }
                            oTmp.Remarklogic = HDLPF.Byte2String(arayRemark);
                            
                            HDLUDP.TimeBetwnNext(1);
                        }
                        else return;
                        if (CsConst.isRestore)
                        {
                            //读取设置
                            #region
                            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x160E, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                            {
                                oTmp.bytRelation = CsConst.myRevBuf[27];
                                oTmp.EnableSensors = new byte[15];
                                int intTmp = CsConst.myRevBuf[28] * 256 + CsConst.myRevBuf[29];
                                if (intTmp == 65535) intTmp = 0;
                                for (byte i = 0; i < 12; i++)
                                {
                                    if ((intTmp & (1 << i)) == (1 << i))
                                    {
                                        switch (i)
                                        {
                                            case 0: oTmp.EnableSensors[0] = 1; break;
                                            case 1: oTmp.EnableSensors[1] = 1; break;
                                            case 5: oTmp.EnableSensors[2] = 1; break;
                                            case 6: oTmp.EnableSensors[4] = 1; break;
                                            case 8: oTmp.EnableSensors[5] = 1; break;
                                            case 9: oTmp.EnableSensors[6] = 1; break;
                                            case 10: oTmp.EnableSensors[7] = 1; break;
                                            case 11: oTmp.EnableSensors[3] = 1; break;
                                        }
                                    }
                                }

                                oTmp.DelayTimeT = CsConst.myRevBuf[30] * 256 + CsConst.myRevBuf[31];
                                oTmp.DelayTimeF = CsConst.myRevBuf[32] * 256 + CsConst.myRevBuf[33];
                                if (oTmp.DelayTimeT == 65535) oTmp.DelayTimeT = 0;
                                if (oTmp.DelayTimeF == 65535) oTmp.DelayTimeF = 0;
                                Array.Copy(CsConst.myRevBuf, 34, oTmp.Paramters, 0, 6);
                                oTmp.Paramters[6] = CsConst.myRevBuf[46];
                                oTmp.Paramters[7] = CsConst.myRevBuf[55];
                                oTmp.Paramters[8] = CsConst.myRevBuf[47];
                                oTmp.UV1 = new UniversalSwitchSet();
                                if (201 <= CsConst.myRevBuf[49] && CsConst.myRevBuf[49] <= 248)
                                    oTmp.UV1.id = CsConst.myRevBuf[49];
                                else
                                    oTmp.UV1.id = 201;
                                if (CsConst.myRevBuf[50] <= 1)
                                    oTmp.UV1.condition = CsConst.myRevBuf[50];
                                oTmp.UV2 = new UniversalSwitchSet();
                                if (201 <= CsConst.myRevBuf[51] && CsConst.myRevBuf[51] <= 248)
                                    oTmp.UV2.id = CsConst.myRevBuf[51];
                                else
                                {
                                    oTmp.UV2.id = 201;
                                    if (oTmp.UV1.id == 201) oTmp.UV2.id = 202;
                                }
                                if (CsConst.myRevBuf[52] <= 1)
                                    oTmp.UV2.condition = CsConst.myRevBuf[52];
                                oTmp.Paramters[9] = CsConst.myRevBuf[53];
                                oTmp.Paramters[10] = CsConst.myRevBuf[54];
                                
                                HDLUDP.TimeBetwnNext(1);
                            }
                            else return;
                            #endregion
                            //读取通用开关信息
                            #region
                            ArayTmp[0] = oTmp.UV1.id;
                            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x160A, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                            {
                                byte[] arayRemark = new byte[20];
                                for (int i = 0; i < 20; i++) { arayRemark[i] = CsConst.myRevBuf[27 + i]; }
                                oTmp.UV1.remark = HDLPF.Byte2String(arayRemark);
                                if (CsConst.myRevBuf[47] == 1) oTmp.UV1.isAutoOff = true;
                                if (CsConst.myRevBuf[48] * 256 + CsConst.myRevBuf[49] > 3600) oTmp.UV1.autoOffDelay = 1;
                                else oTmp.UV1.autoOffDelay = CsConst.myRevBuf[48] * 256 + CsConst.myRevBuf[49];
                                
                                HDLUDP.TimeBetwnNext(1);
                            }
                            else return;
                            if ((CsConst.mySends.AddBufToSndList(ArayTmp, 0xE018, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true))
                            {
                                oTmp.UV1.state = CsConst.myRevBuf[26];
                                
                                HDLUDP.TimeBetwnNext(1);
                            }
                            else return;
                            ArayTmp[0] = oTmp.UV2.id;
                            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x160A, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                            {
                                byte[] arayRemark = new byte[20];
                                for (int i = 0; i < 20; i++) { arayRemark[i] = CsConst.myRevBuf[27 + i]; }
                                oTmp.UV2.remark = HDLPF.Byte2String(arayRemark);
                                if (CsConst.myRevBuf[47] == 1) oTmp.UV2.isAutoOff = true;
                                if (CsConst.myRevBuf[48] * 256 + CsConst.myRevBuf[49] > 3600) oTmp.UV2.autoOffDelay = 1;
                                else oTmp.UV2.autoOffDelay = CsConst.myRevBuf[48] * 256 + CsConst.myRevBuf[49];
                                
                                HDLUDP.TimeBetwnNext(1);
                            }
                            else return;
                            if ((CsConst.mySends.AddBufToSndList(ArayTmp, 0xE018, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true))
                            {
                                oTmp.UV2.state = CsConst.myRevBuf[26];
                                
                            }
                            else return;
                            #endregion
                            //成立的触发目标
                            #region
                            for (byte bytJ = 0; bytJ < 20; bytJ++)
                            {
                                ArayTmp = new byte[2];
                                ArayTmp[0] = (byte)(intI);
                                ArayTmp[1] = (byte)(bytJ + 1);
                                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1612, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                                {
                                    UVCMD.ControlTargets oCMD = new UVCMD.ControlTargets();
                                    oCMD.ID = CsConst.myRevBuf[27];
                                    oCMD.Type = CsConst.myRevBuf[28];  //转换为正确的类型

                                    if (oCMD.Type != 3)
                                    {
                                        oCMD.SubnetID = CsConst.myRevBuf[29];
                                        oCMD.DeviceID = CsConst.myRevBuf[30];
                                        oCMD.Param1 = CsConst.myRevBuf[31];
                                        oCMD.Param2 = CsConst.myRevBuf[32];
                                        oCMD.Param3 = CsConst.myRevBuf[33];
                                        oCMD.Param4 = CsConst.myRevBuf[34];
                                        
                                        oTmp.SetUp.Add(oCMD);
                                    }
                                    
                                    HDLUDP.TimeBetwnNext(1);
                                }
                                else return;
                            }
                            #endregion
                            //不成立的触发目标
                            #region
                            for (byte bytJ = 0; bytJ < 20; bytJ++)
                            {
                                ArayTmp = new byte[2];
                                ArayTmp[0] = (byte)(intI + 24);
                                ArayTmp[1] = (byte)(bytJ + 1);

                                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1612, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                                {
                                    UVCMD.ControlTargets oCMD = new UVCMD.ControlTargets();
                                    oCMD.ID = CsConst.myRevBuf[27];
                                    oCMD.Type = CsConst.myRevBuf[28];  //转换为正确的类型

                                    if (oCMD.Type != 3)
                                    {
                                        oCMD.SubnetID = CsConst.myRevBuf[29];
                                        oCMD.DeviceID = CsConst.myRevBuf[30];
                                        oCMD.Param1 = CsConst.myRevBuf[31];
                                        oCMD.Param2 = CsConst.myRevBuf[32];
                                        oCMD.Param3 = CsConst.myRevBuf[33];
                                        oCMD.Param4 = CsConst.myRevBuf[34];
                                        
                                        oTmp.NoSetUp.Add(oCMD);
                                    }
                                    
                                    HDLUDP.TimeBetwnNext(1);
                                }
                                else return;
                            }
                            #endregion
                        }
                        logic.Add(oTmp);
                        #endregion
                        if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(10 + intI);
                    }
                }
                else return;
                #endregion
                MyRead2UpFlags[1] = true;
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(20);
            if (intActivePage == 0 || intActivePage == 3)
            {
                fireset = new List<UVCMD.SecurityInfo>();
                ArayTmp = new byte[1];
                #region
                for (byte intI = 1; intI <= 1; intI++)
                {
                    ArayTmp[0] = intI;
                    //读取备注
                    UVCMD.SecurityInfo oTmp = new UVCMD.SecurityInfo();
                    oTmp.bytSeuID = intI;
                    switch (intI)
                    {
                        default:
                            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1624, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                            {
                                byte[] arayRemark = new byte[20];
                                for (int intJ = 0; intJ < 20; intJ++) { arayRemark[intJ] = CsConst.myRevBuf[27 + intJ]; }
                                oTmp.strRemark = HDLPF.Byte2String(arayRemark);
                                
                                HDLUDP.TimeBetwnNext(1);
                            }
                            else return;
                            break;
                    }
                    //读取设置
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1628, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                    {
                        oTmp.bytTerms = CsConst.myRevBuf[27];
                        oTmp.bytSubID = CsConst.myRevBuf[28];
                        oTmp.bytDevID = CsConst.myRevBuf[29];
                        oTmp.bytArea = CsConst.myRevBuf[30];
                        
                        HDLUDP.TimeBetwnNext(1);
                    }
                    else return;
                    fireset.Add(oTmp);
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(35 + intI);
                }
                #endregion
                MyRead2UpFlags[2] = true;
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(30);
            if (intActivePage == 0 || intActivePage == 4)
            {
                #region
                //读取模拟测试
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1620, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                {
                    SimulateEnable = new byte[15];
                    SimulateEnable[0] = CsConst.myRevBuf[26];
                    SimulateEnable[1] = CsConst.myRevBuf[27];
                    SimulateEnable[2] = CsConst.myRevBuf[28];
                    SimulateEnable[3] = CsConst.myRevBuf[32];
                    SimulateEnable[4] = CsConst.myRevBuf[40];
                    ParamSimulate = new byte[15];
                    ParamSimulate[0] = CsConst.myRevBuf[33];
                    ParamSimulate[1] = CsConst.myRevBuf[34];
                    ParamSimulate[2] = CsConst.myRevBuf[35];
                    ParamSimulate[3] = CsConst.myRevBuf[39];
                    ParamSimulate[4] = CsConst.myRevBuf[41];
                    
                    HDLUDP.TimeBetwnNext(1);
                }
                else return;
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(31, null);
                #endregion
                MyRead2UpFlags[3] = true;
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(40);
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(100);
        }
    }
}
