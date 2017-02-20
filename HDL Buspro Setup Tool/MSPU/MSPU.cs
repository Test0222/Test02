using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HDL_Buspro_Setup_Tool
{
    [Serializable]
    public class MSPU : HdlDeviceBackupAndRestore
    {
        //基本信息
        public int DIndex;//设备唯一编号
        public string Devname;//子网ID 设备ID 备注
        public byte[] ONLEDs = new byte[5];  // 红外LED灯  // 电源工作指示灯
        public byte[] EnableSensors = new byte[15]; // 留有预留 {温度传感器 亮度传感器  红外传感器 超声波传感器  按键  通用开关一 通用开关二 } 
        public byte[] ParamSensors = new byte[15];  // 对应传感器参数设置 温度补偿值 亮度补偿值，红外灵敏度，超声波灵敏度 
        public byte[] SimulateEnable = new byte[15];//模拟测试使能 温度传感器模拟使能 亮度传感器模拟使能 红外传感器模拟使能 超声波传感器模拟使能
        public byte[] ParamSimulate = new byte[15];//温度模拟值 亮度模拟值，红外模拟值，超声波模拟值
        public List<UVCMD.SecurityInfo> fireset;
        public List<SensorLogic> logic;
        public bool[] MyRead2UpFlags = new bool[6];
        //通用开关
        [Serializable]
        public class UVSet
        {
            public byte UvNum;//开关号
            public string UvRemark;//备注
            public byte UVCondition;
            public bool AutoOff;//自动关闭
            public int OffTime;//自动关闭时间
            public byte state;
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

            if (CsConst.mySends.AddBufToSndList(ArayMain, 0x0010, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
            {
                CsConst.myRevBuf = new byte[1200];
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
                    CsConst.myRevBuf = new byte[1200];
                    HDLUDP.TimeBetwnNext(20);
                }
                else return;
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(6);
                //传感器使能
                ArayTmp = new byte[12];
                ArayTmp[0] = EnableSensors[0];
                ArayTmp[1] = EnableSensors[1];
                ArayTmp[5] = EnableSensors[2];
                ArayTmp[6] = EnableSensors[4];
                ArayTmp[11] = EnableSensors[3];
                ArayTmp[8] = EnableSensors[5];
                ArayTmp[9] = EnableSensors[6];
                ArayTmp[10] = EnableSensors[7];
                
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x161E, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                {
                    CsConst.myRevBuf = new byte[1200];
                    HDLUDP.TimeBetwnNext(20);
                }
                else return;
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(7);
                //补偿值
                ArayTmp = new byte[14];
                ArayTmp[0] = 1;
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
                    CsConst.myRevBuf = new byte[1200];
                    HDLUDP.TimeBetwnNext(20); 
                }
                else return;

                //温发裕特殊需要
                #region
                ArayTmp = new Byte[] { 1,100};
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x3502, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                {
                    Cursor.Current = Cursors.Default;
                    return;
                }
                #endregion
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(8);
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
                        CsConst.myRevBuf = new byte[1200];
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
                                CsConst.myRevBuf = new byte[1200];
                                HDLUDP.TimeBetwnNext(20);
                            }
                            else return;
                            if (oLogic.ID == num1 || CsConst.isRestore)
                            {
                                //修改设置
                                oLogic.ModifyOnlyLogicSettingFromDevice(bytSubID, bytDevID, DeviceType);
                                //成立的触发目标
                                if (CsConst.isRestore)
                                {
                                    oLogic.ModifyLogicTrueCommandsFromDevice(bytSubID, bytDevID, DeviceType);

                                    //不成立的触发目标
                                    oLogic.ModifyLogicFalseCommandsFromDevice(bytSubID, bytDevID, DeviceType);
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
                            CsConst.myRevBuf = new byte[1200];
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
                            CsConst.myRevBuf = new byte[1200];
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
            
            String Remark = HDLSysPF.ReadDeviceMainRemark(bytSubID,bytDevID);
            Devname = bytSubID.ToString() + "-" + bytDevID.ToString() + "\\" + Remark;
            
            if (intActivePage == 0 || intActivePage == 1)
            {
                #region
                //读取LED状态
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xDB3E, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                {
                    ONLEDs = new byte[5];
                    ONLEDs[0] = CsConst.myRevBuf[27];
                    ONLEDs[1] = CsConst.myRevBuf[28];
                    CsConst.myRevBuf = new byte[1200];
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
                    CsConst.myRevBuf = new byte[1200];
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
                    CsConst.myRevBuf = new byte[1200];
                    HDLUDP.TimeBetwnNext(1);
                }
                else return;
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(3, null);
                #endregion
                MyRead2UpFlags[1] = true;
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
                    CsConst.myRevBuf = new byte[1200];
                    HDLUDP.TimeBetwnNext(1);
                    for (int intI = 1; intI <= 24; intI++)
                    {
                        SensorLogic oTmp = new SensorLogic();
                        oTmp.ID = (byte)intI;
                        oTmp.Enabled = ArayTmpMode[intI - 1];
                        oTmp.SetUp = new List<UVCMD.ControlTargets>();
                        oTmp.NoSetUp = new List<UVCMD.ControlTargets>();
                        #region

                        //读取备注
                        ArayTmp = new byte[1];
                        ArayTmp[0] = Convert.ToByte(intI);

                        if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1606, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                        {
                            byte[] arayRemark = new byte[20];
                            for (int intJ = 0; intJ < 20; intJ++) { arayRemark[intJ] = CsConst.myRevBuf[27 + intJ]; }
                            oTmp.Remarklogic = HDLPF.Byte2String(arayRemark);
                            CsConst.myRevBuf = new byte[1200];
                            HDLUDP.TimeBetwnNext(1);
                        }
                        else return;
                        if (oTmp.Enabled == 1) // 有效才读取信息
                        {
                            if (CsConst.isRestore)
                            {
                                //读取设置
                                oTmp.DownloadOnlyLogicSettingFromDevice(bytSubID, bytDevID, DeviceType);
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
                                    CsConst.myRevBuf = new byte[1200];
                                    HDLUDP.TimeBetwnNext(1);
                                }
                                else return;
                                if ((CsConst.mySends.AddBufToSndList(ArayTmp, 0xE018, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true))
                                {
                                    oTmp.UV1.state = CsConst.myRevBuf[26];
                                    CsConst.myRevBuf = new byte[1200];
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
                                    CsConst.myRevBuf = new byte[1200];
                                    HDLUDP.TimeBetwnNext(1);
                                }
                                else return;
                                if ((CsConst.mySends.AddBufToSndList(ArayTmp, 0xE018, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true))
                                {
                                    oTmp.UV2.state = CsConst.myRevBuf[26];
                                    CsConst.myRevBuf = new byte[1200];
                                    HDLUDP.TimeBetwnNext(1);
                                }
                                else return;
                                #endregion
                                oTmp.DownloadLogicTrueCommandsFromDevice(bytSubID, bytDevID, DeviceType,0,0);
                                //不成立的触发目标
                                oTmp.DownloadLogicFalseCommandsFromDevice(bytSubID, bytDevID, DeviceType,0,0);
                            }
                        }
                        logic.Add(oTmp);
                        #endregion
                        if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(10 + intI);
                    }
                }
                else return;
                #endregion
                MyRead2UpFlags[2] = true;
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(20);
            if (intActivePage == 0 || intActivePage == 3)
            {
                fireset = new List<UVCMD.SecurityInfo>();
                ArayTmp = new byte[1];
                #region
                for (byte intI = 3; intI <= 4; intI++)
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
                                CsConst.myRevBuf = new byte[1200];
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
                        CsConst.myRevBuf = new byte[1200];
                        HDLUDP.TimeBetwnNext(1);
                    }
                    else return;
                    fireset.Add(oTmp);
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(35 + intI);
                }
                #endregion
                MyRead2UpFlags[3] = true;
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
                    CsConst.myRevBuf = new byte[1200];
                    HDLUDP.TimeBetwnNext(1);
                }
                else return;
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(31, null);
                #endregion
                MyRead2UpFlags[4] = true;
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(40);
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(100);
        }
    }
}
