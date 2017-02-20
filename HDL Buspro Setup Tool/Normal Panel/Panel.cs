using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.Xml;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Linq;


namespace HDL_Buspro_Setup_Tool
{
     [Serializable]
    public class Panel : HdlDeviceBackupAndRestore
    {
        public string DeviceName;

        public int DIndex;
        public byte Backlight; ////背光灯
        public byte Ledlight;  ////状态灯
        public byte IRON;  //// 接收红外使能位
        public byte LlimitDimmer;  ////调光最低限
        public byte LockKeys;
        public byte LTime; ////长按多长时间
        public byte DTime;///双击时间
        public byte bytTimeFormat;//时间类型
        public byte bytDateFormat;//日期格式
        public byte bytTempType;//温度类型  0 摄氏度 1 华氏度
        public byte AdjustValue; //补偿值
       
        public bool BlnIsNewWirelessPanel;//是否是新的无线面板
        public byte[] bytWirelessConnectInfo = new byte[28];//28个储存信息,信息顺序为状态，连接状态，5个地址MAC
        public byte[] BaseInfomation = new byte[17 + 1];
        public byte[] BaseTempInfomation = new byte[24 + 1];
        public byte[] arayKeyDouble = new byte[16];
        public byte[] arayTempBroadCast = new byte[6];//温度广播设置，第一个参数为1时表示含有此功能
        public byte[] arayButtonSensitiVity;//灵敏度
        public byte[] arayButtonColor;//颜色
        public byte[] arayButtonBalance;//白平衡
        public byte TimeFontSize;//时间字体大小
        public byte TemperatureSource;//温度源
        public byte[] IconOFF;   // 存储正常状态的图标
        public byte[] IconON;    // 存储开状态的图标，
        public bool IsBrdTemp;  // 是否广播温度
        public byte[] araySleep;

        public byte LcdDisplayFlag; //上电是否切换 

        byte[] arayKeyMode;
        byte[] arayKeyDimmer;
        byte[] arayKeyLED;
        byte[] arayKeyMutex;
        byte[] arayLink;

        internal List<HDLButton> PanelKey;
        internal OtherInfo otherInfo;

        internal FloorHeating DLPFH;

        internal Channel[] myPanelDim = new Channel[12];

        public byte[] bytChn = new byte[3]; //无线模块调光回路保护温度

        public bool[] MyRead2UpFlags = new bool[14];

        [Serializable]
        internal class OtherInfo
        {
            public byte[] bytAryShowPage; //页面显示
            public byte bytBacklightTime; //背光时间 ，永远显示：0，其他 10-99s
            public byte bytBacklight;     //背光亮度
            public byte bytGotoPage;      //不跳转0，其他页面1-7
            public byte bytGotoTime;      //20-150s
            public byte bytDisTime;       //显示时间
            public byte bytDisTemp;       //显示温度
            public byte bytLigthDelayState;//十四按键面板 状态灯延时亮度
            public byte IRCloseTargets;//十四按键面板 红外接近按键目标
            public byte IRCloseSensor;//十四按键面板 红外接近传感器使能
            public byte SoundClick;//十四按键面板按键声
            public bool isHasSensorSensitivity = false;
            public byte CloseToSensorSensitivity;//接近传感器使能
            public byte AutoLock;
        }

        //<summary>
        //读取默认的面板设置，将所有数据读取缓存
        //</summary>
        public void ReadDefaultInfo(int DeviceType)
        {
            #region
            int wdMaxValue = int.Parse(CsConst.mstrINIDefault.IniReadValue("DeviceType" + DeviceType.ToString(), "MaxValue", "0"));
            Boolean BlnRoundButton = (NormalPanelDeviceTypeList.NormalWIFIRoundButtonPanelDeviceType.Contains(DeviceType) ||
                                    NormalPanelDeviceTypeList.NormalRoundButtonPanelDeviceType.Contains(DeviceType));
            Boolean BlnTouchGlass = (NormalPanelDeviceTypeList.NormalWIFIThouchPanelDeviceType.Contains(DeviceType) ||
                                     NormalPanelDeviceTypeList.NormalTouchButtonPanelDeviceType.Contains(DeviceType));

            byte bytPages = 1;
            if (CsConst.mintDLPDeviceType.Contains(DeviceType)) bytPages = 4;

            wdMaxValue = wdMaxValue * bytPages;

            Backlight = 100;
            Ledlight = 100;
            IRON = 1;
            LlimitDimmer = 0;
            LockKeys = 0;
            LTime = 3;
            // 读取按键的相关信息
            PanelKey = new List<HDLButton>();
            for (int intI = 0; intI < wdMaxValue; intI++)
            {
                HDLButton TmpKey = new HDLButton();
                TmpKey.ID = Convert.ToByte((intI + 1).ToString());
                TmpKey.Remark = "Button " + (intI + 1).ToString();
                TmpKey.Mode = 3;
                TmpKey.IsLEDON = 0;
                TmpKey.IsDimmer = 1;
                TmpKey.SaveDimmer = 0;

                TmpKey.KeyTargets = null;

                PanelKey.Add(TmpKey);
            }
           

            otherInfo = new OtherInfo();
            otherInfo.bytAryShowPage = new byte[7];
            Boolean BlnHasWireless = NormalPanelDeviceTypeList.NormalWIFIPanelDeviceType.Contains(DeviceType);

            if (BlnHasWireless)
            {
                for (int i = 0; i < myPanelDim.Length; i++)
                {
                    myPanelDim[i] = new Channel();
                    myPanelDim[i].ID = i + 1;
                    myPanelDim[i].Remark = "Chn " + (i + 1).ToString();
                    myPanelDim[i].LoadType = 0;
                    myPanelDim[i].MinValue = 0;
                    myPanelDim[i].MaxValue = 100;
                    myPanelDim[i].MaxLevel = 100;
                    myPanelDim[i].CurveID = 0;
                }
                bytChn = new byte[3];
            }
            bytChn = new byte[3];
            #endregion
        }

        //<summary>
        //读取数据库面板设置，将所有数据读至缓存
        //</summary>
        public void ReadPanelFrmDBTobuf(int intDIndex)
        {
            #region
            try
            {
                #region
                string strsql = string.Format("select * from dbClassInfomation where DIndex={0} and ClassID={1} order by SenNum", DIndex, 0);
                OleDbDataReader dr = DataModule.SearchAResultSQLDB(strsql, CsConst.mstrCurPath);
                PanelKey = new List<HDLButton>();
                if (dr != null)
                {
                    while (dr.Read())
                    {
                        HDLButton temp = new HDLButton();
                        string str = dr.GetValue(5).ToString();
                        temp.ID = Convert.ToByte(str.Split('-')[0].ToString());
                        temp.Mode = Convert.ToByte(str.Split('-')[1].ToString());
                        temp.IsLEDON = Convert.ToByte(str.Split('-')[2].ToString());
                        temp.IsDimmer = Convert.ToByte(str.Split('-')[3].ToString());
                        temp.SaveDimmer = Convert.ToByte(str.Split('-')[4].ToString());
                        temp.bytMutex = Convert.ToByte(str.Split('-')[5].ToString());
                        temp.byteLink = Convert.ToByte(str.Split('-')[6].ToString());
                        temp.Remark = dr.GetValue(4).ToString();
                        str = string.Format("select * from dbKeyTargets where DIndex ={0} and KeyIndex = {1} and Ms04State = {2}", DIndex, temp.ID, 0);
                        OleDbDataReader drKeyCmds = DataModule.SearchAResultSQLDB(str, CsConst.mstrCurPath);
                        temp.KeyTargets = new List<UVCMD.ControlTargets>();
                        while (drKeyCmds.Read())
                        {
                            UVCMD.ControlTargets TmpCmd = new UVCMD.ControlTargets();
                            TmpCmd.ID = drKeyCmds.GetByte(2);
                            TmpCmd.Type = drKeyCmds.GetByte(3);
                            TmpCmd.SubnetID = drKeyCmds.GetByte(4);
                            TmpCmd.DeviceID = drKeyCmds.GetByte(5);
                            TmpCmd.Param1 = drKeyCmds.GetByte(6);
                            TmpCmd.Param2 = drKeyCmds.GetByte(7);
                            TmpCmd.Param3 = drKeyCmds.GetByte(8);
                            TmpCmd.Param4 = drKeyCmds.GetByte(9);
                            temp.KeyTargets.Add(TmpCmd);
                        }
                        drKeyCmds.Close();
                        PanelKey.Add(temp);
                    }
                    dr.Close();
                }
                #endregion

                #region
                strsql = string.Format("select * from dbBasicInfo where DIndex={0}", DIndex);
                dr = DataModule.SearchAResultSQLDB(strsql, CsConst.mstrCurPath);
                if (dr != null)
                {
                    while (dr.Read())
                    {
                        string str = dr.GetValue(1).ToString();
                        DeviceName = DeviceName.Split('\\')[0].ToString() + "\\"
                                + str.Split('\\')[1].ToString();
                    }
                    dr.Close();
                }
                #endregion

                #region
                strsql = string.Format("select * from dbClassInfomation where DIndex={0} and ClassID=6 order by SenNum", DIndex);
                dr = DataModule.SearchAResultSQLDB(strsql, CsConst.mstrCurPath);
                myPanelDim = new Channel[0];
                List<Channel> listTmp = new List<Channel>();
                if (dr != null)
                {
                    while (dr.Read())
                    {
                        Channel[] arayTmp= new Channel[1];
                        arayTmp[0] = new Channel();
                        string str = dr.GetValue(5).ToString();
                        arayTmp[0].ID = Convert.ToByte(str.Split('-')[0].ToString());
                        arayTmp[0].LoadType = Convert.ToByte(str.Split('-')[1].ToString());
                        arayTmp[0].MinValue = Convert.ToByte(str.Split('-')[2].ToString());
                        arayTmp[0].MaxValue = Convert.ToByte(str.Split('-')[3].ToString());
                        arayTmp[0].MaxLevel = Convert.ToByte(str.Split('-')[4].ToString());
                        arayTmp[0].CurveID = Convert.ToByte(str.Split('-')[5].ToString());
                        arayTmp[0].Remark = dr.GetValue(4).ToString();
                        listTmp.Add(arayTmp[0]);
                    }
                    dr.Close();
                    myPanelDim = listTmp.ToArray();
                }
                #endregion
            }
            catch
            {
            }
            #endregion
        }

        ///<summary>
       /// 保存数据库面板设置，将所有数据保存
       /// </summary>
        public void SavePanelToDB()
        {
            #region
            try
            {
                string strsql = string.Format("delete * from dbClassInfomation where DIndex={0}", DIndex);
                DataModule.ExecuteSQLDatabase(strsql);

                strsql = string.Format("delete * from dbKeyTargets where DIndex={0}", DIndex);
                DataModule.ExecuteSQLDatabase(strsql);

                #region
                if (PanelKey != null)
                {
                    for (int i = 0; i < PanelKey.Count; i++)
                    {
                        HDLButton tmpKey = PanelKey[i];
                        string strParam = tmpKey.ID.ToString() + "-" + tmpKey.Mode.ToString() + "-"
                                        + tmpKey.IsLEDON.ToString() + "-" + tmpKey.IsDimmer.ToString() + "-"
                                        + tmpKey.SaveDimmer.ToString() + "-" + tmpKey.bytMutex.ToString()+"-"
                                        + tmpKey.byteLink.ToString();
                        strsql = string.Format("Insert into dbClassInfomation(DIndex,ClassID,ObjectID,SenNum,Remark,strParam1) values ({0},{1},{2},{3},'{4}','{5}')",
                                 DIndex, 0, 0, i, tmpKey.Remark, strParam);
                        DataModule.ExecuteSQLDatabase(strsql);
                        if (tmpKey.KeyTargets != null)
                        {
                            for (int intK = 0; intK < tmpKey.KeyTargets.Count; intK++)
                            {
                                UVCMD.ControlTargets TmpCmds = tmpKey.KeyTargets[intK];
                                ///// insert into all commands to database
                                strsql = string.Format("Insert into dbKeyTargets(DIndex,KeyIndex,objID,KeyFunType,SubNetID,DeviceID,FirstParameter,"
                                                + "SecondParameter,RunTimeMinute,RunTimeSecond,Ms04State) values ({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10})",
                                               DIndex, tmpKey.ID, TmpCmds.ID, TmpCmds.Type, TmpCmds.SubnetID, TmpCmds.DeviceID, TmpCmds.Param1,
                                               TmpCmds.Param2, TmpCmds.Param3, TmpCmds.Param4, 0);
                                DataModule.ExecuteSQLDatabase(strsql);
                            }
                        }
                    }
                }
                #endregion

                #region
                if (otherInfo != null)
                {
                    if (otherInfo.bytAryShowPage == null) otherInfo.bytAryShowPage = new byte[7];
                    string strRemark = "";
                    string strParam = otherInfo.bytBacklightTime.ToString() + "-" + otherInfo.bytBacklight.ToString()
                                    + "-" + otherInfo.bytGotoPage.ToString() + "-" + otherInfo.bytGotoTime.ToString()
                                    + "-" + otherInfo.bytDisTime.ToString() + "-" + otherInfo.bytDisTemp.ToString()
                                    + "-" + otherInfo.bytLigthDelayState.ToString() + "-" + otherInfo.IRCloseTargets.ToString()
                                    + "-" + otherInfo.IRCloseSensor.ToString() + "-" + otherInfo.SoundClick.ToString();
                    strsql = @"Insert into dbClassInfomation(DIndex,ClassID,ObjectID,SenNum,Remark,strParam1,byteAry1)"
                      + " values(@DIndex,@ClassID,@ObjectID,@SenNum,@Remark,@strParam1,@byteAry1)";
                    //创建一个OleDbConnection对象
                    OleDbConnection conn;
                    conn = new OleDbConnection(DataModule.ConString + CsConst.mstrDefaultPath);
                    //OleDbConnection conn = new OleDbConnection(DataModule.ConString + CsConst.mstrCurPath);
                    conn.Open();
                    OleDbCommand cmd = new OleDbCommand(strsql, conn);
                    ((OleDbParameter)cmd.Parameters.Add("@DIndex", OleDbType.Integer)).Value = DIndex;
                    ((OleDbParameter)cmd.Parameters.Add("@ClassID", OleDbType.Integer)).Value = 4;
                    ((OleDbParameter)cmd.Parameters.Add("@ObjectID", OleDbType.Integer)).Value = 0;
                    ((OleDbParameter)cmd.Parameters.Add("@SenNum", OleDbType.Integer)).Value = 0;
                    ((OleDbParameter)cmd.Parameters.Add("@Remark", OleDbType.VarChar)).Value = strRemark;
                    ((OleDbParameter)cmd.Parameters.Add("@strParam1", OleDbType.VarChar)).Value = strParam;
                    ((OleDbParameter)cmd.Parameters.Add("@byteAry1", OleDbType.Binary)).Value = otherInfo.bytAryShowPage;
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch
                    {
                        conn.Close();
                    }
                    conn.Close();
                }
                #endregion

                #region
                if (myPanelDim != null)
                {
                    for (int i = 0; i < myPanelDim.Length; i++)
                    {
                        Channel temp = myPanelDim[i];
                        string strParam = temp.ID.ToString() + "-" + temp.LoadType.ToString() + "-"
                                        + temp.MinValue.ToString() + "-" + temp.MaxValue.ToString() + "-"
                                        + temp.MaxLevel.ToString() + "-" + temp.CurveID.ToString();
                        strsql = string.Format("Insert into dbClassInfomation(DIndex,ClassID,ObjectID,SenNum,Remark,strParam1) values ({0},{1},{2},{3},'{4}','{5}')",
                                    DIndex, 6, 0, i, temp.Remark, strParam);
                        DataModule.ExecuteSQLDatabase(strsql);
                    }
                }
                #endregion
            }
            catch
            {
            }
            #endregion
        }

        public void SaveOnlyButtonsToDB()
        {
            if (PanelKey == null || PanelKey.Count == 0) return;
            #region
            //// insert panel information to database
            for (int intJ = 0; intJ < PanelKey.Count; intJ++)
            {
                HDLButton TmpKey = PanelKey[intJ]; // 保存按键的基本信息
                string strsql = "Insert into dbPanelKey(DIndex,KeyID,Remark,Mode,IsDimmer,SaveDimmer,LEDStatus,IconON,IconOFF)"
                       + " values(@DIndex,@KeyID,@Remark,@Mode,@IsDimmer,@SaveDimmer,@LEDStatus,@IconON,@IconOFF)";

                //创建一个OleDbConnection对象
                OleDbConnection conn;

                conn = new OleDbConnection(DataModule.ConString + CsConst.mstrDefaultPath);
                conn.Open();

                OleDbCommand cmd = new OleDbCommand(strsql, conn);
                ((OleDbParameter)cmd.Parameters.Add("@DIndex", OleDbType.Integer)).Value = DIndex;
                ((OleDbParameter)cmd.Parameters.Add("@KeyID", OleDbType.Char)).Value = TmpKey.ID;
                ((OleDbParameter)cmd.Parameters.Add("@Remark", OleDbType.VarChar)).Value = TmpKey.Remark;
                ((OleDbParameter)cmd.Parameters.Add("@Mode", OleDbType.Char)).Value = TmpKey.Mode;
                ((OleDbParameter)cmd.Parameters.Add("@IsDimmer", OleDbType.Char)).Value = TmpKey.IsDimmer;
                ((OleDbParameter)cmd.Parameters.Add("@SaveDimmer", OleDbType.Char)).Value = TmpKey.SaveDimmer;
                ((OleDbParameter)cmd.Parameters.Add("@LEDStatus", OleDbType.Char)).Value = TmpKey.IsLEDON;
                byte[] arayTmp = new byte[1];
                ((OleDbParameter)cmd.Parameters.Add("@IconON", OleDbType.Binary)).Value = arayTmp;
                ((OleDbParameter)cmd.Parameters.Add("@IconOFF", OleDbType.Binary)).Value = arayTmp;
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (OleDbException exp)
                {
                    MessageBox.Show(exp.ToString());
                }
                conn.Close();


                if (TmpKey.KeyTargets != null)
                {
                    for (int i = 0; i < TmpKey.KeyTargets.Count; i++)
                    {
                        UVCMD.ControlTargets TmpCmds = TmpKey.KeyTargets[i];
                        ///// insert into all commands to database
                        strsql = string.Format("Insert into dbKeyTargets(DIndex,KeyIndex,objID,KeyFunType,SubNetID,DeviceID,FirstParameter,"
                                       + "SecondParameter,RunTimeMinute,RunTimeSecond,Ms04State) values ({0},{1},'{2}',{3},{4},{5},{6},{7},{8},{9},{10})",
                                       DIndex, TmpKey.ID, TmpCmds.ID, TmpCmds.Type, TmpCmds.SubnetID, TmpCmds.DeviceID, TmpCmds.Param1,
                                       TmpCmds.Param2, TmpCmds.Param3, TmpCmds.Param4, 0);
                        DataModule.ExecuteSQLDatabase(strsql);
                    }
                }
            }
            #endregion 
        }

        public void ReadOnlyButtonsFromDB(int intDIndex)
        {
            #region
            PanelKey = new List<HDLButton>();
            string str = string.Format("select * from dbPanelKey where DIndex={0} order by KeyID", intDIndex);
            OleDbDataReader dr = DataModule.SearchAResultSQLDB(str);
            if (dr != null)
            {
                while (dr.Read())
                {
                    HDLButton TmpKey = new HDLButton();
                    TmpKey.ID = dr.GetByte(1);
                    TmpKey.Remark = dr.GetValue(2).ToString();
                    TmpKey.Mode = byte.Parse(dr.GetValue(3).ToString());
                    TmpKey.IsDimmer = dr.GetByte(4);
                    TmpKey.SaveDimmer = dr.GetByte(5);
                    TmpKey.IsLEDON = dr.GetByte(6);

                    ////read keys commands to buffer
                    str = string.Format("select * from dbKeyTargets where DIndex ={0} and KeyIndex = {1}", intDIndex, dr.GetByte(1));
                    OleDbDataReader drKeyCmds = DataModule.SearchAResultSQLDB(str);
                    TmpKey.KeyTargets = new List<UVCMD.ControlTargets>();
                    if (drKeyCmds != null)
                    {
                        while (drKeyCmds.Read())
                        {
                            UVCMD.ControlTargets TmpCmd = new UVCMD.ControlTargets();
                            TmpCmd.ID = drKeyCmds.GetByte(2);
                            TmpCmd.Type = drKeyCmds.GetByte(3);
                            TmpCmd.SubnetID = drKeyCmds.GetByte(4);
                            TmpCmd.DeviceID = drKeyCmds.GetByte(5);
                            TmpCmd.Param1 = drKeyCmds.GetByte(6);
                            TmpCmd.Param2 = drKeyCmds.GetByte(7);
                            TmpCmd.Param3 = drKeyCmds.GetByte(8);
                            TmpCmd.Param4 = drKeyCmds.GetByte(9);
                            TmpKey.KeyTargets.Add(TmpCmd);
                        }
                        PanelKey.Add(TmpKey);
                        drKeyCmds.Close();
                    }
                }
                dr.Close();
            }
            #endregion
        }

        /// <summary>
        /// upload panel information to device
        /// </summary>
        /// <param name="DIndex"></param>
        /// <param name="DevID"></param>
        /// <param name="DeviceType"></param>
        /// <param name="DevName"></param>
        /// <returns></returns>
        public virtual bool UploadPanelInfosToDevice(string DevName, int intActivePage,int DeviceType)// 0 mean all, else that tab only
        {
            //保存回路信息
            string strMainRemark = DevName.Split('\\')[1].Trim();
            DevName = DevName.Split('\\')[0].Trim();
            byte bytSubID = byte.Parse(DevName.Split('-')[0].ToString());
            byte bytDevID = byte.Parse(DevName.Split('-')[1].ToString());

            int wdMaxValue = DeviceTypeList.GetMaxValueFromPublicModeGroup(DeviceType);

            byte[] ArayMain = new byte[20];
            byte[] arayTmpRemark = HDLUDP.StringToByte(strMainRemark);
            //ruby test
            HDLSysPF.CopyRemarkBufferToSendBuffer(arayTmpRemark, ArayMain, 0);

            if (CsConst.mySends.AddBufToSndList(ArayMain, 0x0010, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == false)
            {
                return false;
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(1);

            byte[] arayIRValid = new byte[8];

            if (DLPPanelDeviceTypeList.HDLDLPPanelDeviceTypeList.Contains(DeviceType))
            {
                arayIRValid[0] = IRON;      // modify IR and low dimmer limit
                arayIRValid[1] = LlimitDimmer;
                arayIRValid[2] = otherInfo.bytDisTemp;
                arayIRValid[3] = LTime;
                arayIRValid[4] = otherInfo.bytDisTime;
                arayIRValid[5] = DTime;
                arayIRValid[6] = TimeFontSize;
                arayIRValid[7] = TemperatureSource;
            }
            else if (NormalPanelDeviceTypeList.NormalTouchButtonPanelOldIcDeviceType.Contains(DeviceType) ||
                     NormalPanelDeviceTypeList.NormalWIFIThouchPanelDeviceType.Contains(DeviceType) ||
                     NormalPanelDeviceTypeList.NormalRoundButtonPanelDeviceType.Contains(DeviceType) ||
                     NormalPanelDeviceTypeList.PanelBigButtonDeviceList.Contains(DeviceType) ||
                     NormalPanelDeviceTypeList.PanelKorayPanelWithOutTemperature.Contains(DeviceType))
            {
                arayIRValid[0] = IRON;      // modify IR and low dimmer limit
                arayIRValid[1] = LlimitDimmer;
                arayIRValid[2] = LTime;
                arayIRValid[5] = DTime;
            }
            else
            {
                arayIRValid[0] = IRON;      // modify IR and low dimmer limit
                arayIRValid[1] = LlimitDimmer;
                if (LTime < 3) LTime = 3;
                arayIRValid[3] = LTime;
                if (DTime < 3) DTime = 3;
                arayIRValid[5] = DTime;
            }


            if (CsConst.mySends.AddBufToSndList(arayIRValid, 0xE0E2, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == false) return false;
            HDLUDP.TimeBetwnNext(arayIRValid.Length);

            if (arayTempBroadCast != null && arayTempBroadCast.Length == 6)
            {
                if (arayTempBroadCast[0] == 1)
                {
                    byte[] arayBroadCast = new byte[4];
                    Array.Copy(arayTempBroadCast, 1, arayBroadCast, 0, 4);
                    if (CsConst.mySends.AddBufToSndList(arayBroadCast, 0xE0FA, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == false) return false;
                    HDLUDP.TimeBetwnNext(arayBroadCast.Length);
                }
            }

            byte[] arayLCD = new byte[3];
            arayLCD[0] = Backlight;
            arayLCD[1] = Ledlight;
            arayLCD[2] = LockKeys;
            if (DLPPanelDeviceTypeList.HDLDLPPanelDeviceTypeList.Contains(DeviceType))
            {
                if (LockKeys > 1) LockKeys = 1;
                arayLCD = new byte[]{Backlight,Ledlight,LockKeys,0,1,0,0,0,0,0,0};
            }
            CsConst.mySends.AddBufToSndList(arayLCD, 0xE012, bytSubID, bytDevID, false, false, false, CsConst.minAllWirelessDeviceType.Contains(DeviceType));

            if (intActivePage == 0 || intActivePage == 1)
            {
                #region
                // upload key settings
                arayKeyMode = new byte[PanelKey.Count]; //按键模式
                arayKeyLED = new byte[PanelKey.Count];  // LED 开关
                arayKeyDimmer = new byte[PanelKey.Count];  // 要不要调光，要不要保存调光
                arayKeyMutex = new byte[PanelKey.Count]; // 按键是不是互斥 
                arayLink = new byte[PanelKey.Count];//按键联动
                byte[] arayRemark = new byte[21];// used for restore remark // modify key remarks

                foreach (HDLButton TmpKey in PanelKey)
                {
                    // key mode and dimmer valid
                    arayKeyMode[TmpKey.ID - 1] = TmpKey.Mode;
                    arayKeyMutex[TmpKey.ID - 1] = TmpKey.bytMutex;
                    arayLink[TmpKey.ID - 1] = TmpKey.byteLink;
                    arayKeyDimmer[TmpKey.ID - 1] = byte.Parse(((TmpKey.IsDimmer << 4) + TmpKey.SaveDimmer).ToString());
                    arayKeyLED[TmpKey.ID - 1] = (byte)(TmpKey.IsLEDON);
  
                    if (CsConst.isRestore)
                    {
                        
                    }
                    TmpKey.UploadButtonRemarkAndCMDToDevice(bytSubID, bytDevID, DeviceType, -1, 0);
                    if (TmpKey.ID + 2 < 15)
                        if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(2 + TmpKey.ID);
                }
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(15);

                SaveButtonModeToDeviceFrmBuf(bytSubID, bytDevID, DeviceType);

                // upload key status and dimming save valid or not
                SaveButtonDimFlagToDeviceFrmBuf(bytSubID, bytDevID, DeviceType);

                // upload LED status
                SaveButtonLEDEnableToDeviceFrmBuf(bytSubID, bytDevID, DeviceType);

                SaveButtonMutuxToDeviceFrmBuf(bytSubID, bytDevID, DeviceType);

                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(20);
                if (CsConst.mintNewDLPFHSetupDeviceType.Contains(DeviceType))
                {
                    if (arayKeyDouble == null) arayKeyDouble = new byte[16];
                    if (CsConst.mySends.AddBufToSndList(arayKeyDouble, 0x196C, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == false) return false;
                    HDLUDP.TimeBetwnNext(wdMaxValue);
                    CsConst.myRevBuf = new byte[1200];
                }
                if (!CsConst.mintDLPDeviceType.Contains(DeviceType))
                {
                    if (CsConst.mySends.AddBufToSndList(arayLink, 0xE14a, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == false) return false;
                    HDLUDP.TimeBetwnNext(wdMaxValue);
                    CsConst.myRevBuf = new byte[1200];
                }
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(25);
                #endregion
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(30);
            if (intActivePage == 0 || intActivePage == 2)
            {              
                #region                
                if (IsBrdTemp)
                {
                    if (CsConst.mySends.AddBufToSndList(arayTempBroadCast, 0xE0FA, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == false) return false;
                    CsConst.myRevBuf = new byte[1200];
                }
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(32);
                //2014-12-25 新增休眠模式（状态灯关闭延迟、红外感应按键号）
                if (DLPPanelDeviceTypeList.HDLDLPPanelDeviceTypeList.Contains(DeviceType))
                {
                    #region
                    Byte[] bytTmp = new Byte[] { Convert.ToByte(bytTempType) };
                    if (CsConst.mySends.AddBufToSndList(bytTmp, 0xE122, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                    {
                        CsConst.myRevBuf = new Byte[1200];
                    }
                    #endregion

                    ModifyJumpPagesInformationAfterDelay(bytSubID, bytDevID, DeviceType);
                }

                if (NormalPanelDeviceTypeList.WirelessSimpleFloorHeatingDeviceTypeList.Contains(DeviceType))
                {
                    if (otherInfo != null)
                    {
                        if (otherInfo.bytAryShowPage != null)  //显示页面信息
                        {
                            byte[] arayTmp = new byte[6];
                            Array.Copy(otherInfo.bytAryShowPage, 0, arayTmp, 0, 6);
                            if (CsConst.mySends.AddBufToSndList(arayTmp, 0x3606, bytSubID, bytDevID, false, false, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                            {
                                HDLUDP.TimeBetwnNext(20);
                            }
                            else return false;
                        }
                    }

                    ArayMain = new byte[1];
                    ArayMain[0] = LcdDisplayFlag;
                    if (CsConst.mySends.AddBufToSndList(ArayMain, 0x360A, bytSubID, bytDevID, false, false, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)))
                    {
                        HDLUDP.TimeBetwnNext(20);
                    }
                    else return false;
                }
                #endregion
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(40);
            if (intActivePage == 0 || intActivePage == 3)
            {

            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(50);
            if (intActivePage == 0 || intActivePage == 4)
            {

            }

            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(60);
            if (intActivePage == 0 || intActivePage == 5)
            {
                if (DLPPanelDeviceTypeList.HDLDLPPanelDeviceTypeList.Contains(DeviceType))
                {
                    Byte[] TempOldHighLimit = new Byte[] { 5, 30, 5, 30, 5, 30, 5, 30 };
                    DLPFH.UploadFloorheatingSettingToDevice(bytSubID, bytDevID, DeviceType, TempOldHighLimit);
                }
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(70);
            if (intActivePage == 0 || intActivePage == 6)
            {
                if (myPanelDim == null) return false;
                Boolean BlnHasWireless = NormalPanelDeviceTypeList.NormalNewWIFIPanelDeviceType.Contains(DeviceType);
                //2013 12 27 new add wireless channels setup
                #region
                if (BlnHasWireless)
                {
                    byte[] arayRemark = new byte[21];// used for restore remark
                    byte[] arayLoadType = new byte[myPanelDim.Length];
                    byte[] arayLimitL = new byte[myPanelDim.Length + 1]; arayLimitL[0] = 0;
                    byte[] arayLimitH = new byte[myPanelDim.Length + 1]; arayLimitH[0] = 1;
                    byte[] arayMaxLevel = new byte[myPanelDim.Length];

                    foreach (Channel ch in myPanelDim)
                    {   // modify the chns remark
                        arayRemark = new byte[21]; // 初始化数组
                        string strRemark = ch.Remark;
                        byte[] arayTmp = HDLUDP.StringToByte(strRemark);

                        arayRemark[0] = byte.Parse(ch.ID.ToString());
                        arayTmp.CopyTo(arayRemark, 1);

                        if (CsConst.mySends.AddBufToSndList(arayRemark, 0xF010, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == false) return false;
                        HDLUDP.TimeBetwnNext(arayRemark.Length);
                        CsConst.myRevBuf = new byte[1200];

                        arayLoadType[ch.ID - 1] = byte.Parse(ch.LoadType.ToString());
                        arayMaxLevel[ch.ID - 1] = byte.Parse(ch.MaxLevel.ToString());
                        arayLimitL[ch.ID] = byte.Parse(ch.MinValue.ToString());
                        arayLimitH[ch.ID] = byte.Parse(ch.MaxValue.ToString());
                        // arayArea[2 + ch.ID] = byte.Parse(ch.CurveID.ToString());
                    }

                    // modify the load type
                    //if (CsConst.mySends.AddBufToSndList(arayLoadType, 0xF014, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == false) return false;
                    //HDLUDP.TimeBetwnNext(arayLoadType.Length);
                    //CsConst.myRevBuf = new byte[1200];

                    // modify the on Low limit
                    if (CsConst.mySends.AddBufToSndList(arayLimitL, 0xF018, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == false) return false;
                    HDLUDP.TimeBetwnNext(arayLimitL.Length);
                    CsConst.myRevBuf = new byte[1200];

                    // modify the High Limit
                    //if (CsConst.mySends.AddBufToSndList(arayLimitH, 0xF018, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == false) return false;
                    //HDLUDP.TimeBetwnNext(arayLimitH.Length);
                    //CsConst.myRevBuf = new byte[1200];

                    // modify the max level 
                    if (CsConst.mySends.AddBufToSndList(arayMaxLevel, 0xF022, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == false) return false;
                    HDLUDP.TimeBetwnNext(arayMaxLevel.Length);
                    CsConst.myRevBuf = new byte[1200];
                }
                #endregion
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(80);
            if (intActivePage == 0 || intActivePage == 7)
            {
                #region
                //修改保护温度
                BlnIsNewWirelessPanel = DLPPanelDeviceTypeList.WirelessDLPDeviceTypeList.Contains(DeviceType) ||
                                        NormalPanelDeviceTypeList.NormalWIFIPanelDeviceType.Contains(DeviceType);   
                if (BlnIsNewWirelessPanel)
                {
                    byte[] arayTmp = new byte[bytChn[0]];
                    if (arayTmp.Length > 0)
                    {
                        for(int i=0;i<arayTmp.Length;i++)
                        {
                            arayTmp[i] = bytChn[i + 1];
                        }
                    }
                    else
                    {
                        arayTmp = new byte[2];
                        arayTmp[0] = 80;
                        arayTmp[1] = 0;
                    }
                    if (CsConst.mySends.AddBufToSndList(arayTmp, 0x198A, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == false) return false;
                    HDLUDP.TimeBetwnNext(arayTmp.Length);
                    CsConst.myRevBuf = new byte[1200];
                    if (BaseTempInfomation != null && BaseTempInfomation[BaseTempInfomation.Length - 1] == 1)
                    {
                        if (BaseTempInfomation.Length >= 24)
                        {
                            for (int i = 0; i < 4; i++)
                            {
                                arayTmp = new byte[6];
                                Array.Copy(BaseTempInfomation, i * 6, arayTmp, 0, 6);
                                if (CsConst.mySends.AddBufToSndList(arayTmp, 0x1C02, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == false) return false;
                                CsConst.myRevBuf = new byte[1200];
                            }
                        }
                    }
                }
                #endregion
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(90);
            if (intActivePage == 0 || intActivePage == 8)
            {
                #region
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(91);
                Boolean BlnRoundPanel = NormalPanelDeviceTypeList.NormalWIFIRoundButtonPanelDeviceType.Contains(DeviceType) ||
                                        NormalPanelDeviceTypeList.NormalRoundButtonPanelDeviceType.Contains(DeviceType);
                Boolean BlnTouchGlass = NormalPanelDeviceTypeList.NormalWIFIThouchPanelDeviceType.Contains(DeviceType) ||
                                        NormalPanelDeviceTypeList.NormalTouchButtonPanelDeviceType.Contains(DeviceType);

                if ((BlnRoundPanel || BlnTouchGlass) && (!NormalPanelDeviceTypeList.IranSpeicalTouchButtonPanelDeviceType.Contains(DeviceType)))
                {
                    for (int i = 1; i <= wdMaxValue; i++)
                    {
                        byte[] arayTmp = new byte[7];
                        arayTmp[0] = Convert.ToByte(i);
                        Array.Copy(arayButtonColor, (i - 1) * 6, arayTmp, 1, 6);
                        if (CsConst.mySends.AddBufToSndList(arayTmp, 0xE14E, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == false) return false;
                        HDLUDP.TimeBetwnNext(arayTmp.Length);
                        CsConst.myRevBuf = new byte[1200];
                    }
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(92);
                }

                if (arayButtonBalance[0] > 0)
                {
                    byte[] arayTmp = new byte[arayButtonBalance[0] * 3];
                    Array.Copy(arayButtonBalance, 1, arayTmp, 0, arayButtonBalance.Length - 1);
                    if (CsConst.mySends.AddBufToSndList(arayTmp, 0x199A, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                    {
                        CsConst.myRevBuf = new byte[1200];
                        HDLUDP.TimeBetwnNext(10);
                    }
                    else return false;
                }

                if (BlnTouchGlass)
                {
                    byte[] arayTmp = new byte[wdMaxValue];
                    Array.Copy(arayButtonSensitiVity, 0, arayTmp, 0, wdMaxValue);
                    if (CsConst.mySends.AddBufToSndList(arayTmp, 0xE152, bytSubID, bytDevID, false, false, false, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == false) return false;
                    HDLUDP.TimeBetwnNext(arayTmp.Length);
                    CsConst.myRevBuf = new byte[1200];
                }
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(93);
                #endregion
            }
            if (intActivePage == 0 || intActivePage == 9)
            {
               
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(95);
            if (intActivePage == 0 || intActivePage == 10)
            {
                #region
                if (DeviceType == 175 || DeviceType == 180 || DeviceType == 5004)
                {
                    if (araySleep != null)
                    {
                        byte[] arayTmp = new byte[5];
                        Array.Copy(araySleep, 1, arayTmp, 0, 5);
                        if (CsConst.mySends.AddBufToSndList(arayTmp, 0xE4F8, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                        {
                            CsConst.myRevBuf = new byte[1200];
                        }
                    }
                }
                #endregion
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(100);
            return true;
        }

        public Boolean ModifyJumpPagesInformationAfterDelay(Byte SubNetID, Byte DeviceID, int DeviceType)
        {
            Boolean BlnIsSuccess = false;

            Byte[] bytTmp = new byte[8];    // 显示要不要跳转，跳转时间
            bytTmp[0] = otherInfo.bytBacklightTime;
            bytTmp[1] = otherInfo.bytBacklight;
            bytTmp[2] = otherInfo.bytGotoPage;
            bytTmp[3] = otherInfo.bytGotoTime;
            bytTmp[4] = 0;
            bytTmp[5] = otherInfo.AutoLock;
            bytTmp[6] = 0;
            bytTmp[7] = 0;
            if (DeviceType == 165)
            {
                bytTmp[4] = otherInfo.SoundClick;
                bytTmp[5] = otherInfo.AutoLock;
                bytTmp[6] = otherInfo.bytLigthDelayState;
                bytTmp[7] = otherInfo.IRCloseTargets;
            }
            else if (DeviceType == 167) // 旧版DLP面板
            {
                if (otherInfo.bytGotoTime < 20) otherInfo.bytGotoTime = 20;
                bytTmp = new byte[6];    // 显示要不要跳转，跳转时间
                bytTmp[0] = otherInfo.bytBacklightTime;
                bytTmp[1] = otherInfo.bytBacklight;
                bytTmp[2] = otherInfo.bytGotoPage;
                bytTmp[3] = otherInfo.bytGotoTime;
                bytTmp[4] = 0;
                bytTmp[5] = otherInfo.AutoLock;
            }
            else if (DeviceType == 168 || DeviceType == 170 || DeviceType == 172 || DeviceType == 175)
            {
                bytTmp = new byte[9];    // 显示要不要跳转，跳转时间
                bytTmp[0] = otherInfo.bytBacklightTime;
                bytTmp[1] = otherInfo.bytBacklight;
                bytTmp[2] = otherInfo.bytGotoPage;
                bytTmp[3] = otherInfo.bytGotoTime;
                bytTmp[4] = otherInfo.SoundClick;
                bytTmp[5] = otherInfo.AutoLock;
                bytTmp[6] = otherInfo.bytLigthDelayState;
                bytTmp[7] = otherInfo.IRCloseTargets;
                bytTmp[8] = otherInfo.IRCloseSensor;
                if (otherInfo.isHasSensorSensitivity)
                {
                    bytTmp = new byte[10];    // 显示要不要跳转，跳转时间
                    bytTmp[0] = otherInfo.bytBacklightTime;
                    bytTmp[1] = otherInfo.bytBacklight;
                    bytTmp[2] = otherInfo.bytGotoPage;
                    bytTmp[3] = otherInfo.bytGotoTime;
                    bytTmp[4] = otherInfo.SoundClick;
                    bytTmp[5] = otherInfo.AutoLock;
                    bytTmp[6] = otherInfo.bytLigthDelayState;
                    bytTmp[7] = otherInfo.IRCloseTargets;
                    bytTmp[8] = otherInfo.IRCloseSensor;
                    bytTmp[9] = otherInfo.CloseToSensorSensitivity;
                }
            }

            BlnIsSuccess = CsConst.mySends.AddBufToSndList(bytTmp, 0xE13A, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType));
            HDLUDP.TimeBetwnNext(otherInfo.bytAryShowPage.Length);
            CsConst.myRevBuf = new byte[1200];
            return BlnIsSuccess;
        }

        /// <summary>
        /// 上传图标函数处理
        /// </summary>
        /// <param name="DeviceType"></param>
        /// <param name="DevName"></param>
        /// <returns></returns>
        public bool UploadPanelIconsToDevice(int DeviceType, string DevName)
        {
            string strMainRemark = DevName.Split('\\')[1].Trim();
            DevName = DevName.Split('\\')[0].Trim();
            byte bytSubID = byte.Parse(DevName.Split('-')[0].ToString());
            byte bytDevID = byte.Parse(DevName.Split('-')[1].ToString());
            int num = 64;
            if (CsConst.mintMPTLDeviceType.Contains(DeviceType))
            {
                num = 96;
            }
            int byt = 0;
            if (IconOFF != null)
            {
                for (byte i = 0; i < 4; i++)
                {
                    for (byte j = 0; j < 8; j++)
                    {
                        if (IconOFF[(IconOFF.Length - 1 - 32) + (i * 8) + j + 1] == 1)
                        {
                            for (byte k = 0; k < (num / 8); k++)
                            {
                                if (CsConst.isStopDealImageBackground) break;
                                byte[] arayTmp = new byte[22];
                                arayTmp[0] = i;
                                arayTmp[1] = Convert.ToByte((j * (num / 8) + k));
                                Array.Copy(IconOFF, ((i * num * 20) + ((j * (num / 8) + k)) * 20), arayTmp, 2, 20);
                                if (CsConst.mySends.AddBufToSndList(arayTmp, 0xE118, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                                {
                                    HDLUDP.TimeBetwnNext(5);
                                    CsConst.myRevBuf = new byte[1200];
                                }
                                else return false;
                                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress((40 * byt) / (4 * num));
                                byt = byt + 1;
                            }
                        }
                    }
                }
            }

            if (IconON != null)
            {
                byt = 0;
                for (byte i = 4; i < 8; i++)
                {
                    for (byte j = 0; j < 8; j++)
                    {
                        if (IconON[(IconON.Length - 1 - 32) + ((i - 4) * 8) + j + 1] == 1)
                        {
                            for (byte k = 0; k < (num / 8); k++)
                            {
                                if (CsConst.isStopDealImageBackground) break;
                                byte[] arayTmp = new byte[22];
                                arayTmp[0] = i;
                                arayTmp[1] = Convert.ToByte((j * (num / 8) + k));
                                Array.Copy(IconON, (((i - 4) * num * 20) + ((j * (num / 8) + k)) * 20), arayTmp, 2, 20);
                                if (CsConst.mySends.AddBufToSndList(arayTmp, 0xE118, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                                {
                                    HDLUDP.TimeBetwnNext(5);
                                    CsConst.myRevBuf = new byte[1200];
                                }
                                else return false;
                                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(((45 * byt) / (4 * num)) + 50);
                                byt = byt + 1;
                            }
                        }
                    }
                }
            }
              
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(100);
            return true;
        }


        /// <summary>
        /// 下载图标函数处理
        /// </summary>
        /// <param name="DeviceType"></param>
        /// <param name="DevName"></param>
        /// <returns></returns>
        public bool DownloadPanelIconsToDevice(int DeviceType, string DevName)
        {
            string strMainRemark = DevName.Split('\\')[1].Trim();
            DevName = DevName.Split('\\')[0].Trim();
            byte bytSubID = byte.Parse(DevName.Split('-')[0].ToString());
            byte bytDevID = byte.Parse(DevName.Split('-')[1].ToString());

            int wdMaxValue = DeviceTypeList.GetMaxValueFromPublicModeGroup(DeviceType);

            if (CsConst.mintNewDLPFHSetupDeviceType.Contains(DeviceType))
            {
                if (CsConst.mintMPTLDeviceType.Contains(DeviceType))
                {
                    if (IconOFF == null && IconON == null)
                    {
                        IconOFF = new byte[7680 + 1 + 32];
                        IconON = new byte[7680 + 1 + 32];
                    }
                }
                else
                {
                    if (IconOFF == null && IconON == null)
                    {
                        IconOFF = new byte[5120 + 1 + 32];
                        IconON = new byte[5120 + 1 + 32];
                    }
                }
            }
            else
            {
                if (IconOFF == null && IconON == null)
                {
                    IconOFF = new byte[5120 + 1 + 32];
                    IconON = new byte[5120 + 1 + 32];
                }
            }
            int num = 64;
            if (CsConst.mintMPTLDeviceType.Contains(DeviceType))
            {
                num = 96;
            }
            int byt = 0;
            for (byte i = 0; i < 4; i++)
            {
                if (CsConst.DownloadPictruePageID == 255 || ((i + 1) == CsConst.DownloadPictruePageID))
                {
                    for (byte j = 0; j < num; j++)
                    {
                        if (CsConst.isStopDealImageBackground) break;
                        byte[] arayTmp = new byte[2];
                        arayTmp[0] = i;
                        arayTmp[1] = j;
                        if (CsConst.mySends.AddBufToSndList(arayTmp, 0x194C, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                        {
                            Array.Copy(CsConst.myRevBuf, 27, IconOFF, ((i * 20 * num) + j * 20), 20);
                            HDLUDP.TimeBetwnNext(5);
                            CsConst.myRevBuf = new byte[1200];
                        }
                        else return false;
                        if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress((40 * byt) / (4 * num));
                        byt = byt + 1;
                    }
                }
            }
            IconOFF[IconOFF.Length - 1 - 32] = 1;
            byt = 0;
            for (byte i = 4; i < 8; i++)
            {
                if (CsConst.DownloadPictruePageID == 255 || ((i - 3) == CsConst.DownloadPictruePageID))
                {
                    for (byte j = 0; j < num; j++)
                    {
                        if (CsConst.isStopDealImageBackground) break;
                        byte[] arayTmp = new byte[2];
                        arayTmp[0] = i;
                        arayTmp[1] = j;
                        if (CsConst.mySends.AddBufToSndList(arayTmp, 0x194C, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                        {
                            Array.Copy(CsConst.myRevBuf, 27, IconON, (((i - 4) * 20 * num) + j * 20), 20);
                            HDLUDP.TimeBetwnNext(5);
                            CsConst.myRevBuf = new byte[1200];

                        }
                        else return false;
                        if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(((45 * byt) / (4 * num)) + 50);
                        byt = byt + 1;
                    }
                }
            }
            IconON[IconON.Length - 1 - 32] = 1;
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(100);
            return true;
        }

        /// <summary>
        /// deviceType, normal panel or DLP 
        /// </summary>
        /// <param name="DeviceType"></param>
        /// <param name="DevName"></param>
        public virtual void DownLoadInformationFrmDevice(string DevName, int intActivePage, int DeviceType, int num1, int num2)// 0 mean all, else that tab only
        {
            string strMainRemark = DevName.Split('\\')[1].Trim();
            DevName = DevName.Split('\\')[0].Trim();

            byte bytSubID = byte.Parse(DevName.Split('-')[0].ToString());
            byte bytDevID = byte.Parse(DevName.Split('-')[1].ToString());
            byte[] ArayTmp = null;

            Boolean BlnHasWireless =  CsConst.mintWIFIPanelDeviceType.Contains(DeviceType);
            Boolean BlnRoundButton = (NormalPanelDeviceTypeList.NormalWIFIRoundButtonPanelDeviceType.Contains(DeviceType) ||
                                      NormalPanelDeviceTypeList.NormalRoundButtonPanelDeviceType.Contains(DeviceType));
            Boolean BlnTouchGlass = (NormalPanelDeviceTypeList.NormalWIFIThouchPanelDeviceType.Contains(DeviceType) ||
                                     NormalPanelDeviceTypeList.NormalTouchButtonPanelDeviceType.Contains(DeviceType));
            Boolean BlnIsNewWirelessPanel = (CsConst.mintNewWIFIPanelDeviceType.Contains(DeviceType));

            DeviceName = bytSubID.ToString() + "-" + bytDevID.ToString() + "\\" + HDLSysPF.ReadDeviceMainRemark(bytSubID, bytDevID);

            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(5);
            int wdMaxValue = DeviceTypeList.GetMaxValueFromPublicModeGroup(DeviceType);

            byte bytPages = 1;
            if (CsConst.mintDLPDeviceType.Contains(DeviceType)) bytPages = 4;

            wdMaxValue = wdMaxValue * bytPages;

            //DLP和面板基本信息公共部分
            if (intActivePage == 0 || intActivePage == 2)
            {
                #region
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xE010, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                {
                    Backlight = CsConst.myRevBuf[25];
                    Ledlight = CsConst.myRevBuf[26];
                    LockKeys = CsConst.myRevBuf[27];
                    CsConst.myRevBuf = new byte[1200];
                }
                else return;
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(6);
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xE0E0, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                {
                    if (DLPPanelDeviceTypeList.HDLDLPPanelDeviceTypeList.Contains(DeviceType))
                    {
                        otherInfo = new OtherInfo();
                        otherInfo.bytAryShowPage = new Byte[7];
                        IRON = CsConst.myRevBuf[26];
                        LlimitDimmer = CsConst.myRevBuf[27];
                        LTime = CsConst.myRevBuf[29];
                        otherInfo.bytDisTemp = CsConst.myRevBuf[28];
                        otherInfo.bytDisTime = CsConst.myRevBuf[30];
                        DTime = CsConst.myRevBuf[31];
                        TimeFontSize = CsConst.myRevBuf[32];
                        TemperatureSource = CsConst.myRevBuf[33];

                        #region
                        if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xE12C, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                        {
                            for (byte bytI = 0; bytI < 7; bytI++) { otherInfo.bytAryShowPage[bytI] = CsConst.myRevBuf[25 + bytI]; }
                            CsConst.myRevBuf = new byte[1200];
                        }
                        else return;
                        #endregion
                    }
                    else if (NormalPanelDeviceTypeList.NormalTouchButtonPanelOldIcDeviceType.Contains(DeviceType) ||
                             NormalPanelDeviceTypeList.NormalWIFIThouchPanelDeviceType.Contains(DeviceType) ||
                             NormalPanelDeviceTypeList.NormalRoundButtonPanelDeviceType.Contains(DeviceType) ||
                             NormalPanelDeviceTypeList.PanelBigButtonDeviceList.Contains(DeviceType) ||
                             NormalPanelDeviceTypeList.PanelKorayPanelWithOutTemperature.Contains(DeviceType))
                    {
                        IRON = CsConst.myRevBuf[26];
                        LlimitDimmer = CsConst.myRevBuf[27];
                        LTime = CsConst.myRevBuf[28];
                        DTime = CsConst.myRevBuf[29];
                    }
                    else
                    {
                        IRON = CsConst.myRevBuf[26];
                        LlimitDimmer = CsConst.myRevBuf[27];
                        LTime = CsConst.myRevBuf[29];
                        DTime = CsConst.myRevBuf[31];
                    }
                    CsConst.myRevBuf = new byte[1200];
                }
                else return;
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(7);

                if ((!NormalPanelDeviceTypeList.PanelKorayPanelWithOutTemperature.Contains(DeviceType) && 
                     !NormalPanelDeviceTypeList.TouchPanelGenerationOneWithOutTemperature.Contains(DeviceType)) ||
                     DeviceType==5003)
                {
                    arayTempBroadCast = new byte[6];
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xE0F8, bytSubID, bytDevID, false, false, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                    {
                        arayTempBroadCast[0] = 1;
                        Array.Copy(CsConst.myRevBuf, 25, arayTempBroadCast, 1, 5);
                        CsConst.myRevBuf = new byte[1200];
                        HDLUDP.TimeBetwnNext(10);
                    }
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(8);
                }

                #region
                if (NormalPanelDeviceTypeList.WirelessSimpleFloorHeatingDeviceTypeList.Contains(DeviceType))
                {
                    if (CsConst.mySends.AddBufToSndList(null, 0x3608, bytSubID, bytDevID, false, false, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                    {
                        LcdDisplayFlag = CsConst.myRevBuf[25];
                        HDLUDP.TimeBetwnNext(1);
                    }
                    else return;

                    if (CsConst.mySends.AddBufToSndList(null, 0x3604, bytSubID, bytDevID, false, false, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                    {
                        for (byte bytI = 0; bytI < 6; bytI++) { otherInfo.bytAryShowPage[bytI] = CsConst.myRevBuf[25 + bytI]; }
                        HDLUDP.TimeBetwnNext(1);
                    }
                    else return;
                }
                #endregion
                if (DLPPanelDeviceTypeList.HDLDLPPanelDeviceTypeList.Contains(DeviceType) || NormalPanelDeviceTypeList.WirelessSimpleFloorHeatingDeviceTypeList.Contains(DeviceType))
                {
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xE138, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                    {
                        otherInfo.bytBacklightTime = CsConst.myRevBuf[25]; //背光时间 ，永远显示：0，其他 10-99s
                        otherInfo.bytBacklight = CsConst.myRevBuf[26];     //背光亮度
                        otherInfo.bytGotoPage = CsConst.myRevBuf[27];      //不跳转0，其他页面1-7
                        otherInfo.bytGotoTime = CsConst.myRevBuf[28];      //20-150s
                        otherInfo.AutoLock = CsConst.myRevBuf[30];
                        if (CsConst.mintMPTLDeviceType.Contains(DeviceType))
                        {
                            otherInfo.bytLigthDelayState = CsConst.myRevBuf[31];
                            otherInfo.IRCloseTargets = CsConst.myRevBuf[32];
                            otherInfo.IRCloseSensor = CsConst.myRevBuf[33];
                            if (CsConst.myRevBuf[16] >= 0x15)
                            {
                                otherInfo.isHasSensorSensitivity = true;
                                otherInfo.CloseToSensorSensitivity = CsConst.myRevBuf[34];
                            }
                            else
                            {
                                otherInfo.isHasSensorSensitivity = false;
                            }
                        }
                        CsConst.myRevBuf = new byte[1200];
                        HDLUDP.TimeBetwnNext(10);
                    }
                    else return;
                }
                #endregion
            }

            // read basic information status + other ir 
            arayKeyMode = new byte[wdMaxValue]; //按键模式
            arayKeyLED = new byte[wdMaxValue];  // LED 开关
            arayKeyDimmer = new byte[wdMaxValue];  // 要不要调光，要不要保存调光
            byte[] AraySDim = new byte[wdMaxValue];
            byte[] ArayEDim = new byte[wdMaxValue];
            arayKeyMutex = new byte[wdMaxValue]; // 按键是不是互斥 
            arayLink = new byte[wdMaxValue];//按键联动

            if (intActivePage == 0 || intActivePage == 1)
            {
                if (intActivePage == 0)
                {
                    num1 = 0;
                    num2 = wdMaxValue;
                }
                // read all keys information
                PanelKey = new List<HDLButton>();
                #region
                for (int intI = num1; intI < num2; intI++)
                {
                    ArayTmp = new byte[1];

                    AraySDim[intI] = (byte)(arayKeyDimmer[intI] & 1);
                    ArayEDim[intI] = (byte)(arayKeyDimmer[intI] >> 4 & 1);

                    HDLButton oKey = new HDLButton();
                    oKey.ID = Convert.ToByte(intI + 1);

                    oKey.Mode = arayKeyMode[intI];
                    oKey.IsLEDON = arayKeyLED[intI];
                    oKey.IsDimmer = ArayEDim[intI];
                    oKey.SaveDimmer = AraySDim[intI];
                    oKey.bytMutex = arayKeyMutex[intI]; //是不是互斥
                    oKey.byteLink = arayLink[intI];
                    PanelKey.Add(oKey);
                }
                #endregion

                //读取更多设置
                #region
                arayKeyMode = ReadButtonModeFrmDeviceToBuf(bytSubID, bytDevID);
                if (arayKeyMode == null) return;

                arayKeyLED = ReadButtonLEDFrmDeviceToBuf(bytSubID, bytDevID);
                arayKeyDimmer = ReadButtonDimFlagFrmDeviceToBuf(bytSubID, bytDevID);

                arayKeyMutex = ReadButtonMutuxFrmDeviceToBuf(bytSubID, bytDevID);

                if (!CsConst.mintDLPDeviceType.Contains(DeviceType))  // 液晶面板或者无线面板
                {
                    arayLink = ReadButtonLinkableFrmDeviceToBuf(bytSubID, bytDevID);
                }
                #endregion

                for (int intI = num1; intI < num2; intI++)
                {
                    HDLButton oKey = PanelKey[intI];

                    AraySDim[intI] = (byte)(arayKeyDimmer[intI] & 1);
                    ArayEDim[intI] = (byte)(arayKeyDimmer[intI] >> 4 & 1);

                    oKey.Mode = arayKeyMode[intI];
                    oKey.IsLEDON = arayKeyLED[intI];
                    oKey.IsDimmer = ArayEDim[intI];
                    oKey.SaveDimmer = AraySDim[intI];
                    oKey.bytMutex = arayKeyMutex[intI]; //是不是互斥
                    oKey.byteLink = arayLink[intI];
                    oKey.ReadButtonRemarkAndCMDFromDevice(bytSubID, bytDevID, DeviceType, -1, 255, true, 0, 0);
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(35 + intI);
                }

                MyRead2UpFlags[0] = true;
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(70);
            //2013 12 27 新增回路下载
            if (intActivePage == 0 || intActivePage == 5) // 地热页面
            {
                if (DLPPanelDeviceTypeList.HDLDLPPanelDeviceTypeList.Contains(DeviceType) || NormalPanelDeviceTypeList.WirelessSimpleFloorHeatingDeviceTypeList.Contains(DeviceType))
                {
                    DLPFH = new FloorHeating();
                    Byte[] arayAcTemperatureRange = new Byte[] { 5, 30, 5, 30, 5, 30, 5, 30, 1 };
                    DLPFH.DownloadFloorheatingsettingFromDevice(bytSubID, bytDevID, DeviceType, ref arayAcTemperatureRange);
                }
            }
            if (intActivePage == 0 || intActivePage == 6)
            {
                #region
                ArayTmp = new byte[1];
                if (BlnHasWireless)
                {
                    myPanelDim = new Channel[3];
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xE470, bytSubID, bytDevID, false, false, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                    {
                        int count = CsConst.myRevBuf[28] + CsConst.myRevBuf[31] + CsConst.myRevBuf[35] + CsConst.myRevBuf[39];
                        myPanelDim = new Channel[count];
                        if (count == 0) myPanelDim = new Channel[3];
                    }
                    else
                    {
                        if (BlnIsNewWirelessPanel) myPanelDim = new Channel[12];
                        else myPanelDim = new Channel[3];
                    }
                    for (int i = 0; i < myPanelDim.Length; i++)
                    {
                        myPanelDim[i] = new Channel();
                        myPanelDim[i].ID = (Byte)(i + 1);

                        ArayTmp[0] = (byte)(i + 1);
                        if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xF00E, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                        {
                            if (CsConst.myRevBuf != null)
                            {
                                byte[] arayRemark = new byte[20];
                                for (int intI = 0; intI < 20; intI++) { arayRemark[intI] = CsConst.myRevBuf[26 + intI]; }
                                myPanelDim[i].Remark = HDLPF.Byte2String(arayRemark);
                            }
                            CsConst.myRevBuf = new byte[1200];
                            HDLUDP.TimeBetwnNext(10);
                        }
                        else return;
                    }
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(71);
                    ArayTmp = null;
                    // read load type
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xF012, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                    {
                        for (int intI = 0; intI < myPanelDim.Length; intI++)
                        {
                            myPanelDim[intI].LoadType = CsConst.myRevBuf[25 + intI];

                            if (myPanelDim[intI].LoadType == 255) myPanelDim[intI].LoadType = 0;
                        }
                        CsConst.myRevBuf = new byte[1200];
                        HDLUDP.TimeBetwnNext(10);
                    }
                    else return;
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(72);
                    // read low limit
                    ArayTmp = new byte[1];
                    ArayTmp[0] = 0;
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xF016, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                    {
                        for (int intI = 0; intI < myPanelDim.Length; intI++)
                        {
                            myPanelDim[intI].MinValue = CsConst.myRevBuf[26 + intI];
                        }
                        CsConst.myRevBuf = new byte[1200];
                        HDLUDP.TimeBetwnNext(20);
                    }
                    else return;
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(73);

                    // read High Level
                    ArayTmp = null;
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xF020, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                    {
                        for (int intI = 0; intI < myPanelDim.Length; intI++)
                        {
                            myPanelDim[intI].MaxLevel = CsConst.myRevBuf[25 + intI];
                        }
                        CsConst.myRevBuf = new byte[1200];
                        HDLUDP.TimeBetwnNext(10);
                    }
                    else return;
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(74);
                }
                #endregion
                MyRead2UpFlags[5] = true;
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(75);
            //调光地址读取
            if (intActivePage == 0 || intActivePage == 7)
            {
                #region
                //读取保护温度
                if (BlnIsNewWirelessPanel)
                {
                    ArayTmp = null;
                    bytChn = new byte[3];
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1988, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                    {
                        if (CsConst.myRevBuf[16] == 13) bytChn[0] = 2;
                        else bytChn[0] = 1;
                        bytChn[1] = CsConst.myRevBuf[25];
                        bytChn[2] = CsConst.myRevBuf[26];
                        CsConst.myRevBuf = new byte[1200];
                        HDLUDP.TimeBetwnNext(10);
                    }
                    else return;
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1994, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                    {
                        bytWirelessConnectInfo = new byte[28];
                        for (int i = 0; i < 28; i++) bytWirelessConnectInfo[i] = CsConst.myRevBuf[25 + i];
                        CsConst.myRevBuf = new byte[1200];
                        HDLUDP.TimeBetwnNext(10);
                    }
                    else return;

                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(76);

                    ArayTmp = new byte[0];
                    BaseInfomation = new byte[17 + 1];
                    BaseTempInfomation = new byte[24 + 1];

                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xE470, bytSubID, bytDevID, false, false, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                    {
                        Array.Copy(CsConst.myRevBuf, 25, BaseInfomation, 0, 17);
                        BaseInfomation[17] = 1;
                        CsConst.myRevBuf = new byte[1200];
                        HDLUDP.TimeBetwnNext(10);
                    }

                    for (int i = 0; i < 4; i++)
                    {
                        if (BaseInfomation[i * 4 + 1] - 1 ==3) // 带温度的电源底座 
                        {
                            ArayTmp = new byte[1];
                            ArayTmp[0] = Convert.ToByte(i + 2);
                            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1C00, bytSubID, bytDevID, false, false, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                            {
                                BaseTempInfomation[24] = 1;
                                Array.Copy(CsConst.myRevBuf, 25, BaseTempInfomation, (i - 2) * 6, 6);
                                CsConst.myRevBuf = new byte[1200];
                                HDLUDP.TimeBetwnNext(10);
                            }
                            else
                            {
                                BaseTempInfomation[24] = 0;
                                break;
                            }
                        }
                    }
                }
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(77);
                #endregion
                MyRead2UpFlags[6] = true;
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(80);
            }
            if (intActivePage == 0 || intActivePage == 8)
            {
                #region
                arayButtonSensitiVity = new byte[wdMaxValue];
                arayButtonColor = new byte[wdMaxValue * 6];
                arayButtonBalance = new byte[wdMaxValue * 3 + 1];
                if (BlnTouchGlass)//灵敏度
                {
                    ArayTmp = null;
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xE150, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                    {
                        Array.Copy(CsConst.myRevBuf, 25, arayButtonSensitiVity, 0, wdMaxValue);
                        CsConst.myRevBuf = new byte[1200];
                        HDLUDP.TimeBetwnNext(10);
                    }
                    else return;
                }
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(81);
                if ((BlnTouchGlass || BlnRoundButton) && (!NormalPanelDeviceTypeList.IranSpeicalTouchButtonPanelDeviceType.Contains(DeviceType)))//颜色
                {
                    ArayTmp = new byte[1];
                    for (int i = 1; i <= wdMaxValue; i++)
                    {
                        ArayTmp[0] = Convert.ToByte(i);
                        if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xE14C, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                        {
                            Array.Copy(CsConst.myRevBuf, 26, arayButtonColor, (i - 1) * 6, 6);
                            CsConst.myRevBuf = new byte[1200];
                            HDLUDP.TimeBetwnNext(10);
                        }
                        else return;
                    }
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(82);
                }

                if (NormalPanelDeviceTypeList.PanelHasNoBalanceLightDeviceType.Contains(DeviceType) == false)
                {
                    ArayTmp = null;
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1998, bytSubID, bytDevID, false, false, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                    {
                        int num = wdMaxValue;
                        if (CsConst.mintMPTLDeviceType.Contains(DeviceType))
                        {
                            arayButtonBalance = new byte[12 * 3 + 1];
                            num = 12;
                        }
                        for (int i = 1; i <= num; i++)
                        {
                            arayButtonBalance[0] = Convert.ToByte(i);
                            Array.Copy(CsConst.myRevBuf, 25 + (i - 1) * 3, arayButtonBalance, (i - 1) * 3 + 1, 3);
                        }
                    }
                }
                CsConst.myRevBuf = new byte[1200];
                HDLUDP.TimeBetwnNext(10);
                MyRead2UpFlags[7] = true;
                #endregion
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(90);
            if (intActivePage == 0 || intActivePage == 9)
            {
       
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(95);
            if (intActivePage == 0 || intActivePage == 10)
            {
                #region
                if (DeviceType == 175 || DeviceType == 180 || DeviceType == 5004)
                {
                    araySleep = new byte[6];
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xE4FA, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                    {
                        araySleep[0] = 1;
                        Array.Copy(CsConst.myRevBuf, 25, araySleep, 1, 5);
                        CsConst.myRevBuf = new byte[1200];
                    }
                }
                MyRead2UpFlags[10] = true;
                #endregion
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(99);
        }

        public void PanelButtonModifyLEDColorInformation(Byte SubNetID, Byte DeviceID, Byte ButtonID, int DeviceType)
        {
            byte[] arayTmp = new byte[7];
            arayTmp[0] = ButtonID;
            Array.Copy(arayButtonColor, (ButtonID - 1) * 6, arayTmp, 1, 6);
            if (CsConst.mySends.AddBufToSndList(arayTmp, 0xE14E, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
            {
                HDLUDP.TimeBetwnNext(arayTmp.Length);
                CsConst.myRevBuf = new byte[1200];
            }
        }

        public Byte[] ReadButtonDimFlagFrmDeviceToBuf(Byte bytSubID, Byte bytDevID)
        {
            Byte[] arayKeyDim = new Byte[PanelKey.Count];

            if (CsConst.mySends.AddBufToSndList(null, 0xE134, bytSubID, bytDevID, false, true, true, true) == true)
            {
                for (int intJ = 0; intJ < PanelKey.Count; intJ++)
                {
                    arayKeyDim[intJ] = CsConst.myRevBuf[25 + intJ];
                }
                CsConst.myRevBuf = new byte[1200];
            }
            return arayKeyDim;
        }

        public Byte[] ReadButtonModeFrmDeviceToBuf(Byte bytSubID, Byte bytDevID)
        {
            Byte[] arayKeyMode = new Byte[PanelKey.Count];

            if (CsConst.mySends.AddBufToSndList(null, 0xE008, bytSubID, bytDevID, false, true, true, true) == true)
            {
                Array.Copy(CsConst.myRevBuf, 25, arayKeyMode, 0, PanelKey.Count);
                CsConst.myRevBuf = new byte[1200];
            }
            return arayKeyMode;
        }

        public Byte[] ReadButtonLEDFrmDeviceToBuf(Byte bytSubID, Byte bytDevID)
        {
            Byte[] arayKeyLED = new Byte[PanelKey.Count];
            if (CsConst.mySends.AddBufToSndList(null, 0xE130, bytSubID, bytDevID, false, true, true, true) == true)
            {
                Array.Copy(CsConst.myRevBuf, 25, arayKeyLED, 0, PanelKey.Count);
                CsConst.myRevBuf = new byte[1200];
            }
            return arayKeyLED;
        }

        public Byte[] ReadButtonMutuxFrmDeviceToBuf(Byte SubNetID, Byte DeviceID)
        {
            Byte[] arayKeyMutux = new Byte[PanelKey.Count];
            if (CsConst.mySends.AddBufToSndList(null, 0xE320, SubNetID, DeviceID, false, true, true, false) == true)
            {
                Array.Copy(CsConst.myRevBuf,25,arayKeyMutux,0,PanelKey.Count);
                CsConst.myRevBuf = new byte[1200];
            }
            return arayKeyMutux;
        }

        public Byte[] ReadButtonLinkableFrmDeviceToBuf(Byte SubNetID, Byte DeviceID)
        {
            Byte[] ArayLink = new Byte[PanelKey.Count];
            if (CsConst.mySends.AddBufToSndList(null, 0xE148, SubNetID, DeviceID, false, true, true,false) == true)
            {
                Array.Copy(CsConst.myRevBuf, 25, arayLink, 0, arayLink.Length);
                CsConst.myRevBuf = new byte[1200];
            }
            return arayLink;
        }

        public Boolean SaveButtonModeToDeviceFrmBuf(Byte bytSubID, Byte bytDevID, int DeviceType)
        {
            Boolean blnSuccessModify = false;
            Byte[] arayKeyMode = new Byte[PanelKey.Count];
            for (Byte bytI = 0; bytI < PanelKey.Count; bytI++)
            {
                // key mode
                HDLButton oTmpButton = PanelKey[bytI];
                arayKeyMode[bytI] = oTmpButton.Mode;
            }

            // upload all key mode
            blnSuccessModify = CsConst.mySends.AddBufToSndList(arayKeyMode, 0xE00A, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType));
            return blnSuccessModify;
        }

        public Boolean SaveButtonDimFlagToDeviceFrmBuf(Byte bytSubID, Byte bytDevID, int DeviceType)
        {
            Boolean blnSuccessModify = false;
            Byte[] arayKeyMode = new Byte[PanelKey.Count];
            for (Byte bytI = 0; bytI < PanelKey.Count; bytI++)
            {
                // key  dimmer flag
                HDLButton oTmpButton = PanelKey[bytI];
                arayKeyMode[bytI] = (Byte)((oTmpButton.IsDimmer << 4) + oTmpButton.SaveDimmer);
            }

            // upload dimmer flag
            blnSuccessModify = CsConst.mySends.AddBufToSndList(arayKeyMode, 0xE136, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType));
            return blnSuccessModify;
        }

        public Boolean SaveButtonLEDEnableToDeviceFrmBuf(Byte bytSubID, Byte bytDevID, int DeviceType)
        {
            Boolean blnSuccessModify = false;
            Byte[] arayKeyLED = new Byte[PanelKey.Count];
            for (Byte bytI = 0; bytI < PanelKey.Count; bytI++)
            {
                HDLButton oTmpButton = PanelKey[bytI];
                arayKeyLED[bytI] = (Byte)(oTmpButton.IsLEDON);
            }

            blnSuccessModify = CsConst.mySends.AddBufToSndList(arayKeyLED, 0xE132, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType));
            return blnSuccessModify;
        }

        public Boolean SaveButtonMutuxToDeviceFrmBuf(Byte bytSubID, Byte bytDevID, int DeviceType)
        {
            Boolean blnSuccessModify = false;
            Byte[] arayKeyMutux = new Byte[PanelKey.Count];
            for (Byte bytI = 0; bytI < PanelKey.Count; bytI++)
            {
                HDLButton oTmpButton = PanelKey[bytI];
                arayKeyMutux[bytI] = (Byte)(oTmpButton.bytMutex);
            }

            blnSuccessModify = CsConst.mySends.AddBufToSndList(arayKeyMutux, 0xE322, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType));
            return blnSuccessModify;
        }

        public Boolean SaveButtonLinkageToDeviceFrmBuf(Byte bytSubID, Byte bytDevID, int DeviceType)
        {
            Boolean blnSuccessModify = false;
            Byte[] arayKeyLink = new Byte[PanelKey.Count];
            for (Byte bytI = 0; bytI < PanelKey.Count; bytI++)
            {
                HDLButton oTmpButton = PanelKey[bytI];
                arayKeyLink[bytI] = (Byte)(oTmpButton.byteLink);
            }

            blnSuccessModify = CsConst.mySends.AddBufToSndList(arayKeyLink, 0xE14A, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType));
            return blnSuccessModify;
        }
    }
}
