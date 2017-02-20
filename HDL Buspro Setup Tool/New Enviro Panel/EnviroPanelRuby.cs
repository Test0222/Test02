using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;

namespace HDL_Buspro_Setup_Tool
{ 
    public class EnviroPanel
    {
        public Byte TotalPages; //总共多少页
        public Byte MainPage;  // 返回的主界面

        //基本信息
        public int DIndex;//设备唯一编号
        public string DeviceName;//子网ID 设备ID 备注
        public List<HDLButton> MyKeys;
        public List<EnviroAc> MyAC;
        public List<EnviroFH> MyHeat;
        public List<EnviroMusic> MyMusic;
        public bool isHasSensorSensitivity = false;
        public bool[] MyRead2UpFlags = new bool[20];
        public byte TemperatureType;
        public byte[] BasicInfo = new byte[20];//背光，调光低限，时间
        public byte[] BroadCastAry = new byte[20];
        public List<byte[]> arayPIC = new List<byte[]>();//数组的前三个byte代表起始地址

        public List<IconInfo> IconsInPage;

        public const Byte ButtonSum = 36;
        public List<Byte> PublicAvaibleIconArray = null;

        public class IconInfo
        {
            public byte TotalPages;
            public byte PageID;
            public byte Index;
            public byte IconID;
            public string Remark;
        }


        ///<summary>
        ///读取默认的设置
        ///</summary>
        public void ReadDefaultInfo()
        {
            MyKeys = new List<HDLButton>();
            MyAC = new List<EnviroAc>();
            MyHeat = new List<EnviroFH>();
            MyMusic = new List<EnviroMusic>();
        }

        public void SaveToDataBase()
        {
            try
            {
                string strsql = string.Format("delete * from dbClassInfomation where DIndex={0}", DIndex);
                DataModule.ExecuteSQLDatabase(strsql);

                strsql = string.Format("delete * from dbKeyTargets where DIndex={0}", DIndex);
                DataModule.ExecuteSQLDatabase(strsql);

                #region
                if (MyAC != null)
                {
                    for (int i = 0; i < MyAC.Count; i++)
                    {
                        EnviroAc tmpAC = MyAC[i];
                        string strRemark = "";
                        string strParam = tmpAC.ID.ToString() + "-" + tmpAC.Enable.ToString() + "-" + tmpAC.DesSubnetID.ToString() + "-" + tmpAC.DesDevID.ToString()
                                        + "-" + tmpAC.ACNum.ToString() + "-" + tmpAC.ControlWays.ToString() + "-" + tmpAC.OperationEnable.ToString()
                                        + "-" + tmpAC.PowerOnRestoreState.ToString() + "-" + tmpAC.IROperationEnable.ToString()
                                        + "-" + tmpAC.IRControlEnable.ToString() + "-" + tmpAC.IRPowerOnControlEnable.ToString()
                                        + "-" + tmpAC.FanEnable.ToString() + "-" + tmpAC.FanEnergySaveEnable.ToString() + "-" + tmpAC.ACSwitch.ToString()
                                        + "-" + tmpAC.CoolingTemp.ToString() + "-" + tmpAC.HeatingTemp.ToString() + "-" + tmpAC.AutoTemp.ToString()
                                        + "-" + tmpAC.DryTemp.ToString() + "-" + tmpAC.ACMode.ToString() + "-" + tmpAC.ACWind.ToString()
                                        + "-" + tmpAC.ACFanEnable.ToString();

                        strsql = @"Insert into dbClassInfomation(DIndex,ClassID,ObjectID,SenNum,Remark,strParam1,byteAry1,byteAry2,byteAry3)"
                          + "values(@DIndex,@ClassID,@ObjectID,@SenNum,@Remark,@strParam1,@byteAry1,@byteAry2,@byteAry3)";
                        //创建一个OleDbConnection对象
                        OleDbConnection conn;

                        conn = new OleDbConnection(DataModule.ConString + CsConst.mstrDefaultPath);
                        //OleDbConnection conn = new OleDbConnection(DataModule.ConString + CsConst.mstrCurPath);
                        conn.Open();
                        OleDbCommand cmd = new OleDbCommand(strsql, conn);
                        ((OleDbParameter)cmd.Parameters.Add("@DIndex", OleDbType.Integer)).Value = DIndex;
                        ((OleDbParameter)cmd.Parameters.Add("@ClassID", OleDbType.Integer)).Value = 1;
                        ((OleDbParameter)cmd.Parameters.Add("@ObjectID", OleDbType.Integer)).Value = 0;
                        ((OleDbParameter)cmd.Parameters.Add("@SenNum", OleDbType.Integer)).Value = i;
                        ((OleDbParameter)cmd.Parameters.Add("@Remark", OleDbType.VarChar)).Value = strRemark;
                        ((OleDbParameter)cmd.Parameters.Add("@strParam1", OleDbType.VarChar)).Value = strParam;
                        ((OleDbParameter)cmd.Parameters.Add("@byteAry1", OleDbType.Binary)).Value = tmpAC.FanParam;
                        ((OleDbParameter)cmd.Parameters.Add("@byteAry2", OleDbType.Binary)).Value = tmpAC.CoolParam;
                        ((OleDbParameter)cmd.Parameters.Add("@byteAry3", OleDbType.Binary)).Value = tmpAC.OutDoorParam;
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
                }
                #endregion

                #region
                if (MyHeat != null)
                {
                    for (int i = 0; i < MyHeat.Count; i++)
                    {
                        EnviroFH tmpHeat = MyHeat[i];
                        string strRemark = "";
                        string strParam = tmpHeat.ID.ToString() + "-" + tmpHeat.HeatEnable.ToString() + "-" + tmpHeat.SourceTemp.ToString()
                                        + "-" + tmpHeat.PIDEnable.ToString() + "-" + tmpHeat.OutputType.ToString() + "-" + tmpHeat.minPWM.ToString()
                                        + "-" + tmpHeat.maxPWM.ToString() + "-" + tmpHeat.Speed.ToString()
                                        + "-" + tmpHeat.Cycle.ToString() + "-" + tmpHeat.Switch.ToString()
                                        + "-" + tmpHeat.ProtectTemperature.ToString() + "-" + tmpHeat.ControlMode.ToString()
                                        + "-" + tmpHeat.HeatType.ToString() + "-" + tmpHeat.CompenValue.ToString() + "-" + tmpHeat.WorkingSwitch.ToString()
                                        + "-" + tmpHeat.DesSubnetID.ToString() + "-" + tmpHeat.DesDeviceID.ToString() + "-" + tmpHeat.Channel.ToString()
                                        + "-" + tmpHeat.minTemp.ToString() + "-" + tmpHeat.maxTemp.ToString() + "-" + tmpHeat.CurrentTemp.ToString()
                                        + "-" + tmpHeat.WorkingTempMode.ToString();

                        strsql = @"Insert into dbClassInfomation(DIndex,ClassID,ObjectID,SenNum,Remark,strParam1,byteAry1,byteAry2,byteAry3,byteAry4,byteAry5,byteAry6)"
                          + "values(@DIndex,@ClassID,@ObjectID,@SenNum,@Remark,@strParam1,@byteAry1,@byteAry2,@byteAry3,@byteAry4,@byteAry5,@byteAry6)";
                        //创建一个OleDbConnection对象
                        OleDbConnection conn;
                        conn = new OleDbConnection(DataModule.ConString + CsConst.mstrDefaultPath);
                        //OleDbConnection conn = new OleDbConnection(DataModule.ConString + CsConst.mstrCurPath);
                        conn.Open();
                        OleDbCommand cmd = new OleDbCommand(strsql, conn);
                        ((OleDbParameter)cmd.Parameters.Add("@DIndex", OleDbType.Integer)).Value = DIndex;
                        ((OleDbParameter)cmd.Parameters.Add("@ClassID", OleDbType.Integer)).Value = 3;
                        ((OleDbParameter)cmd.Parameters.Add("@ObjectID", OleDbType.Integer)).Value = 0;
                        ((OleDbParameter)cmd.Parameters.Add("@SenNum", OleDbType.Integer)).Value = i;
                        ((OleDbParameter)cmd.Parameters.Add("@Remark", OleDbType.VarChar)).Value = strRemark;
                        ((OleDbParameter)cmd.Parameters.Add("@strParam1", OleDbType.VarChar)).Value = strParam;
                        ((OleDbParameter)cmd.Parameters.Add("@byteAry1", OleDbType.Binary)).Value = tmpHeat.OutDoorParam;
                        ((OleDbParameter)cmd.Parameters.Add("@byteAry2", OleDbType.Binary)).Value = tmpHeat.SourceParam;
                        ((OleDbParameter)cmd.Parameters.Add("@byteAry3", OleDbType.Binary)).Value = tmpHeat.ModeAry;
                        ((OleDbParameter)cmd.Parameters.Add("@byteAry4", OleDbType.Binary)).Value = tmpHeat.TimeAry;
                        ((OleDbParameter)cmd.Parameters.Add("@byteAry5", OleDbType.Binary)).Value = tmpHeat.SysEnable;
                        ((OleDbParameter)cmd.Parameters.Add("@byteAry6", OleDbType.Binary)).Value = tmpHeat.ModeTemp;
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
                }
                #endregion

                #region
                if (MyMusic != null)
                {
                    for (int i = 0; i < MyMusic.Count; i++)
                    {
                        EnviroMusic tmpMusic = MyMusic[i];
                        string strRemark = "";
                        string strParam = tmpMusic.ID.ToString() + "-" + tmpMusic.Enable.ToString() + "-"
                                        + tmpMusic.CurrentZoneID.ToString() + "-" + tmpMusic.Type.ToString();
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
                        ((OleDbParameter)cmd.Parameters.Add("@SenNum", OleDbType.Integer)).Value = i;
                        ((OleDbParameter)cmd.Parameters.Add("@Remark", OleDbType.VarChar)).Value = strRemark;
                        ((OleDbParameter)cmd.Parameters.Add("@strParam1", OleDbType.VarChar)).Value = strParam;
                        ((OleDbParameter)cmd.Parameters.Add("@byteAry1", OleDbType.Binary)).Value = tmpMusic.aryNetDevID;
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
                }
                #endregion
            }
            catch
            {
            }
        }

        public void ReadDataBase(int DIndex)
        {
            try
            {
                #region
                string strsql = string.Format("select * from dbClassInfomation where DIndex={0} and ClassID={1} order by SenNum", DIndex, 0);
                OleDbDataReader dr = DataModule.SearchAResultSQLDB(strsql, CsConst.mstrCurPath);
                MyKeys = new List<HDLButton>();
                if (dr != null)
                {
                    while (dr.Read())
                    {
                        HDLButton temp = new HDLButton();
                        string str = dr.GetValue(5).ToString();
                        temp.ID = Convert.ToByte(str.Split('-')[1].ToString());
                        temp.Mode = Convert.ToByte(str.Split('-')[2].ToString());
                        temp.IsDimmer = Convert.ToByte(str.Split('-')[3].ToString());
                        temp.SaveDimmer = Convert.ToByte(str.Split('-')[4].ToString());
                        temp.bytMutex = Convert.ToByte(str.Split('-')[5].ToString());
                        temp.Remark = dr.GetValue(4).ToString();
                        str = string.Format("select * from dbKeyTargets where DIndex ={0} and KeyIndex = {1} and Ms04State = {2}", DIndex, temp.ID,0);
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
                        MyKeys.Add(temp);
                    }
                    dr.Close();
                }

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
                strsql = string.Format("select * from dbClassInfomation where DIndex={0} and ClassID=1 order by SenNum", DIndex);
                dr = DataModule.SearchAResultSQLDB(strsql, CsConst.mstrCurPath);
                MyAC = new List<EnviroAc>();
                if (dr != null)
                {
                    while (dr.Read())
                    {
                        EnviroAc temp = new EnviroAc();
                        string str = dr.GetValue(5).ToString();
                        temp.ID = Convert.ToByte(str.Split('-')[0].ToString());
                        temp.Enable = Convert.ToByte(str.Split('-')[1].ToString());
                        temp.DesSubnetID = Convert.ToByte(str.Split('-')[2].ToString());
                        temp.DesDevID = Convert.ToByte(str.Split('-')[3].ToString());
                        temp.ACNum = Convert.ToByte(str.Split('-')[4].ToString());
                        temp.ControlWays = Convert.ToByte(str.Split('-')[5].ToString());
                        temp.OperationEnable = Convert.ToByte(str.Split('-')[6].ToString());
                        temp.PowerOnRestoreState = Convert.ToByte(str.Split('-')[7].ToString());
                        temp.IROperationEnable = Convert.ToByte(str.Split('-')[8].ToString());
                        temp.IRControlEnable = Convert.ToByte(str.Split('-')[9].ToString());
                        temp.IRPowerOnControlEnable = Convert.ToByte(str.Split('-')[10].ToString());
                        temp.FanEnable = Convert.ToByte(str.Split('-')[11].ToString());
                        temp.FanEnergySaveEnable = Convert.ToByte(str.Split('-')[12].ToString());
                        temp.ACSwitch = Convert.ToByte(str.Split('-')[13].ToString());
                        temp.CoolingTemp = Convert.ToByte(str.Split('-')[14].ToString());
                        temp.HeatingTemp = Convert.ToByte(str.Split('-')[15].ToString());
                        temp.AutoTemp = Convert.ToByte(str.Split('-')[16].ToString());
                        temp.DryTemp = Convert.ToByte(str.Split('-')[17].ToString());
                        temp.ACMode = Convert.ToByte(str.Split('-')[18].ToString());
                        temp.ACWind = Convert.ToByte(str.Split('-')[19].ToString());
                        temp.ACFanEnable = Convert.ToByte(str.Split('-')[20].ToString());
                        temp.FanParam = (byte[])dr[6];
                        temp.CoolParam = (byte[])dr[7];
                        temp.OutDoorParam = (byte[])dr[8];
                        MyAC.Add(temp);
                    }
                    dr.Close();
                }
                #endregion

                #region
                strsql = string.Format("select * from dbClassInfomation where DIndex={0} and ClassID=3 order by SenNum", DIndex);
                dr = DataModule.SearchAResultSQLDB(strsql, CsConst.mstrCurPath);
                MyHeat = new List<EnviroFH>();
                if (dr != null)
                {
                    while (dr.Read())
                    {
                        EnviroFH temp = new EnviroFH();
                        string str = dr.GetValue(5).ToString();
                        temp.ID = Convert.ToByte(str.Split('-')[0].ToString());
                        temp.HeatEnable = Convert.ToByte(str.Split('-')[1].ToString());
                        temp.SourceTemp = Convert.ToByte(str.Split('-')[2].ToString());
                        temp.PIDEnable = Convert.ToByte(str.Split('-')[3].ToString());
                        temp.OutputType = Convert.ToByte(str.Split('-')[4].ToString());
                        temp.minPWM = Convert.ToByte(str.Split('-')[5].ToString());
                        temp.maxPWM = Convert.ToByte(str.Split('-')[6].ToString());
                        temp.Speed = Convert.ToByte(str.Split('-')[7].ToString());
                        temp.Cycle = Convert.ToByte(str.Split('-')[8].ToString());
                        temp.Switch = Convert.ToByte(str.Split('-')[9].ToString());
                        temp.ProtectTemperature = Convert.ToByte(str.Split('-')[10].ToString());
                        temp.ControlMode = Convert.ToByte(str.Split('-')[11].ToString());
                        temp.HeatType = Convert.ToByte(str.Split('-')[12].ToString());
                        temp.CompenValue = Convert.ToByte(str.Split('-')[13].ToString());
                        temp.WorkingSwitch = Convert.ToByte(str.Split('-')[14].ToString());
                        temp.DesSubnetID = Convert.ToByte(str.Split('-')[15].ToString());
                        temp.DesDeviceID = Convert.ToByte(str.Split('-')[16].ToString());
                        temp.Channel = Convert.ToByte(str.Split('-')[17].ToString());
                        temp.minTemp = Convert.ToByte(str.Split('-')[18].ToString());
                        temp.maxTemp = Convert.ToByte(str.Split('-')[19].ToString());
                        temp.CurrentTemp = Convert.ToByte(str.Split('-')[20].ToString());
                        temp.WorkingTempMode = Convert.ToByte(str.Split('-')[20].ToString());
                        temp.OutDoorParam = (byte[])dr[6];
                        temp.SourceParam = (byte[])dr[7];
                        temp.ModeAry = (byte[])dr[8];
                        temp.TimeAry = (byte[])dr[9];
                        temp.SysEnable = (byte[])dr[10];
                        temp.ModeTemp = (byte[])dr[11];
                        MyHeat.Add(temp);
                    }
                    dr.Close();
                }
                #endregion

                #region
                strsql = string.Format("select * from dbClassInfomation where DIndex={0} and ClassID=4 order by SenNum", DIndex);
                dr = DataModule.SearchAResultSQLDB(strsql, CsConst.mstrCurPath);
                MyMusic = new List<EnviroMusic>();
                if (dr != null)
                {
                    while (dr.Read())
                    {
                        EnviroMusic temp = new EnviroMusic();
                        string str = dr.GetValue(5).ToString();
                        temp.ID = Convert.ToByte(str.Split('-')[0].ToString());
                        temp.Enable = Convert.ToByte(str.Split('-')[1].ToString());
                        temp.CurrentZoneID = Convert.ToByte(str.Split('-')[2].ToString());
                        temp.Type = Convert.ToByte(str.Split('-')[3].ToString());
                        temp.aryNetDevID = (byte[])dr[6];
                        MyMusic.Add(temp);
                    }
                    dr.Close();
                }
                #endregion
            }
            catch
            {
            }
        }

        ///<summary>
        ///下载设置
        ///</summary>
        public void DownloadColorDLPInfoToDevice(int DeviceType, string DevName, int intActivePage)
        {
            string strMainRemark = DevName.Split('\\')[1].Trim();
            DevName = DevName.Split('\\')[0].Trim();

            byte bytSubID = byte.Parse(DevName.Split('-')[0].ToString());
            byte bytDevID = byte.Parse(DevName.Split('-')[1].ToString());
            int wdMaxValue = int.Parse(CsConst.mstrINIDefault.IniReadValue("DeviceType" + DeviceType.ToString(), "MaxValue", "0"));
            byte[] ArayTmp = new byte[0];

            String Remark = HDLSysPF.ReadDeviceMainRemark(bytSubID, bytDevID);
            DeviceName = bytSubID.ToString() + "-" + bytDevID.ToString() + "\\" + Remark;

            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(5);

            ReadColorPagesInformation(bytSubID, bytDevID, DeviceType);

            PublicAvaibleIconArray = new List<byte>();
            foreach (Byte[] Tmp in IconsInPage)
            {
                foreach (Byte TmpIcon in Tmp)
                {
                    if (TmpIcon != 0) PublicAvaibleIconArray.Add(TmpIcon);
                }
            }


            if (intActivePage == 0 || intActivePage == 1)
            {
                #region
                BasicInfo = new byte[20];
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xE010, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                {
                    BasicInfo[2] = CsConst.myRevBuf[25];//背光亮度
                    CsConst.myRevBuf = new byte[1200];
                }
                else return;
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xE138, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                {
                    BasicInfo[0] = CsConst.myRevBuf[25];//背光显示
                    BasicInfo[6] = CsConst.myRevBuf[26];//背光显示亮度
                    BasicInfo[7] = CsConst.myRevBuf[27];//返回页
                    BasicInfo[8] = CsConst.myRevBuf[28];//返回页延时
                    BasicInfo[1] = CsConst.myRevBuf[29];//按键声
                    BasicInfo[3] = CsConst.myRevBuf[33];//红外接近传感器使能
                    if (CsConst.myRevBuf[16] >= 0x15)
                    {
                        isHasSensorSensitivity = true;
                        BasicInfo[9] = CsConst.myRevBuf[34];
                    }
                    else
                    {
                        isHasSensorSensitivity = false;
                    }
                    CsConst.myRevBuf = new byte[1200];
                }
                else return;
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(6);
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xE0E0, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                {
                    BasicInfo[4] = CsConst.myRevBuf[27];//调光低限
                    BasicInfo[5] = CsConst.myRevBuf[29];//长按时间
                    BasicInfo[15] = CsConst.myRevBuf[28];//温度显示
                    BasicInfo[16] = CsConst.myRevBuf[30];//时间显示
                    CsConst.myRevBuf = new byte[1200];
                }
                else return;
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(7);
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xE120, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                {
                    TemperatureType = CsConst.myRevBuf[25];
                    CsConst.myRevBuf = new byte[1200];
                }
                else return;
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(9);

                BroadCastAry = new byte[20];
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xE0F8, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                {
                    Array.Copy(CsConst.myRevBuf, 25, BroadCastAry, 0, 5);
                }
                else return;

                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xE128, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                {
                    BasicInfo[13] = CsConst.myRevBuf[25];//日期类型
                    BasicInfo[14] = CsConst.myRevBuf[26];//日期格式
                    CsConst.myRevBuf = new byte[1200];
                }
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(10);
                #endregion
                MyRead2UpFlags[0] = true;
            }
            else if (intActivePage == 0 || intActivePage == 2)
            {
                #region
                MyKeys = new List<HDLButton>();
                //读取按键模式
                ArayTmp = new byte[3] { 4, 0, ButtonSum };
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x199E, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                {
                    #region
                    for (byte j = 0; j < ButtonSum; j++)
                    {
                        HDLButton temp = new HDLButton();
                        temp.ID = Convert.ToByte(j + 1);
                        temp.KeyTargets = new List<UVCMD.ControlTargets>();
                        temp.Mode = CsConst.myRevBuf[28 + j];
                        temp.Remark = "";
                        MyKeys.Add(temp);
                    }
                    CsConst.myRevBuf = new byte[1200];
                    #endregion
                    //读取按键调光使能和保存
                    #region
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x19A2, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                    {
                        for (byte j = 0; j < ButtonSum; j++)
                        {
                            byte bytTmp = CsConst.myRevBuf[28 + j];
                            string str = GlobalClass.IntToBit(Convert.ToInt32(bytTmp), 8);
                            if (str.Substring(3, 1) == "1") MyKeys[j].IsDimmer = 1;
                            else MyKeys[j].IsDimmer = 0;
                            if (str.Substring(7, 1) == "1") MyKeys[j].SaveDimmer = 1;
                            else MyKeys[j].SaveDimmer = 0;
                        }
                        CsConst.myRevBuf = new byte[1200];
                    }
                    else return;
                    #endregion

                    //读取按键互斥
                    #region
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x19A6, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                    {
                        for (byte j = 0; j < ButtonSum; j++)
                        {
                            MyKeys[j].bytMutex = CsConst.myRevBuf[28 + j];
                        }
                        CsConst.myRevBuf = new byte[1200];
                    }
                    else return;
                    #endregion

                    #region
                    if (CsConst.isBackUp)
                    {
                        for (int j = 1; j <= ButtonSum; j++)
                        {
                            byte bytTotalCMD = 0;
                            if (!PublicAvaibleIconArray.Contains((Byte)(j + EnviroNewDeviceTypeList.ButtonStart - 1))) continue;
                            switch (MyKeys[j - 1].Mode)
                            {
                                case 0: bytTotalCMD = 0; break;
                                case 1:
                                case 2:
                                case 3: bytTotalCMD = 1; break;
                                case 4:
                                case 5:
                                case 7: bytTotalCMD = 9; break;
                            }

                            ArayTmp = new byte[5];
                            ArayTmp[0] = Convert.ToByte(j);
                            MyKeys[j - 1].KeyTargets = new List<UVCMD.ControlTargets>();
                            for (byte bytJ = 0; bytJ < bytTotalCMD; bytJ++)
                            {
                                ArayTmp[1] = Convert.ToByte(bytJ + 1);
                                ArayTmp[2] = 4;
                                ArayTmp[3] = 0;
                                ArayTmp[4] = 36;
                                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xE000, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
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
                                    CsConst.myRevBuf = new byte[1200];
                                    MyKeys[j - 1].KeyTargets.Add(oCMD);
                                }
                                else return;
                            }
                            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(10 + j);
                        }
                    }
                    #endregion                   
                }
                #endregion
                MyRead2UpFlags[1] = true;
            }
            else if (intActivePage == 0 || intActivePage == 3)
            {
                #region               
                MyAC = new List<EnviroAc>();
                //读取空调综合参数
                ArayTmp = new byte[1];
                for (byte i = 0; i < 9; i++)
                {
                    if (!PublicAvaibleIconArray.Contains((Byte)(i+ EnviroNewDeviceTypeList.ACStart))) continue;
                    ArayTmp[0] = i;
                    EnviroAc temp = new EnviroAc();
                    if (temp.ReadDLPACPageSettings(bytSubID, bytDevID,DeviceType,i) == false) return;

                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(20 + i);
                    MyAC.Add(temp);
                }
                #endregion
                MyRead2UpFlags[2] = true;
            }
            else if (intActivePage == 0 || intActivePage == 4) // d地热界面
            {
                #region
                MyHeat = new List<EnviroFH>();
                ArayTmp = new byte[1];
                for (byte i = 0; i < 9; i++)
                {
                    if (!PublicAvaibleIconArray.Contains((Byte)(i + EnviroNewDeviceTypeList.FHStart ))) continue;
                    ArayTmp[0] = i;
                    EnviroFH temp = new EnviroFH();
                    temp.ID = i;
                    temp.DownloadFloorheatingsettingFromDevice(bytSubID, bytDevID, DeviceType);

                    temp.ReadCurrentStatusFromFloorheatingModule(bytSubID, bytDevID, DeviceType);

                    temp.CalculationModeTargets = new byte[40];
                    if (CsConst.isBackUp)
                    {
                        temp.ReadSalveChannelWhenSlaveMode(bytSubID, bytDevID, DeviceType);
                    }

                    if (temp.HeatEnable!=0) temp.ReadFloorheatingAdvancedCommandsGroup(bytSubID, bytDevID, DeviceType);
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(40 + i);
                    MyHeat.Add(temp);
                }
                #endregion
                MyRead2UpFlags[4] = true;
            }
            else if (intActivePage == 0 || intActivePage == 5)  //音乐
            {
                #region
                MyMusic = new List<EnviroMusic>();
                for (byte i = 0; i < 9; i++)
                {
                    if (!PublicAvaibleIconArray.Contains((Byte)(i + EnviroNewDeviceTypeList.MusicStart))) continue;
                    ArayTmp = new byte[1];
                    ArayTmp[0] = i;
                    EnviroMusic temp = new EnviroMusic();
                    temp.ReadMusicSettingInformation(bytSubID, bytDevID, DeviceType);
                    temp.ReadMusicAdvancedCommandsGroup(bytSubID, bytDevID, DeviceType);
                    MyMusic.Add(temp);
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(50 + i);
                }
                #endregion
                MyRead2UpFlags[5] = true;
            }
           // base.DownloadColorDLPInfoToDevice(DeviceType, DeviceName, intActivePage);
        }
        /// <summary>
        ///上传信息到彩屏
        /// </summary>
        public void UploadColorDLPInfoToDevice(int DeviceType, string DevName, int intActivePage)
        {
            string strMainRemark = DevName.Split('\\')[1].Trim().Split('(')[0].Trim();
            DevName = DevName.Split('\\')[0].Trim();
            //保存回路信息
            byte SubNetID = byte.Parse(DevName.Split('-')[0].ToString());
            byte DeviceID = byte.Parse(DevName.Split('-')[1].ToString());

            //修改总共多少页 每页的图标是什么
            ModifyColorPagesInformation(SubNetID, DeviceID, DeviceType);
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(5, null);

            Byte[] ArayMain = null;
            if (intActivePage == 0 || intActivePage == 1)  //基本信息页面
            {
                ArayMain = new byte[4];
                Array.Copy(BroadCastAry, ArayMain, 4);
                if (CsConst.mySends.AddBufToSndList(ArayMain, 0xE0FA, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                {
                    CsConst.myRevBuf = new byte[1200];
                }
                else return;

                #region
                ArayMain = new byte[10];
                ArayMain[0] = BasicInfo[0];
                ArayMain[1] = BasicInfo[6];
                ArayMain[2] = BasicInfo[7];
                ArayMain[3] = BasicInfo[8];
                ArayMain[4] = BasicInfo[1];
                ArayMain[6] = BasicInfo[2];
                ArayMain[8] = BasicInfo[3];
                ArayMain[9] = BasicInfo[9];
                if (CsConst.mySends.AddBufToSndList(ArayMain, 0xE13A, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                {
                    CsConst.myRevBuf = new byte[1200];
                }
                else return;
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(6, null);
                ArayMain = new byte[8];
                ArayMain[1] = BasicInfo[4];
                ArayMain[2] = BasicInfo[15];
                ArayMain[3] = BasicInfo[5];
                ArayMain[4] = BasicInfo[16];
                ArayMain[5] = 1;
                if (CsConst.mySends.AddBufToSndList(ArayMain, 0xE0E2, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                {
                    CsConst.myRevBuf = new byte[1200];
                }
                else return;
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(7, null);
                ArayMain = new byte[1];
                ArayMain[0] = TemperatureType;
                if (CsConst.mySends.AddBufToSndList(ArayMain, 0xE122, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                {
                    CsConst.myRevBuf = new byte[1200];
                }
                else return;
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(8, null);
                ArayMain = new byte[2];
                ArayMain[0] = BasicInfo[13];
                ArayMain[1] = BasicInfo[14];
                if (CsConst.mySends.AddBufToSndList(ArayMain, 0xE12A, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                {
                    CsConst.myRevBuf = new byte[1200];
                }
                else return;

                byte[] arayLCD = new byte[3];
                arayLCD[0] = BasicInfo[2];
                arayLCD[1] = 0;
                arayLCD[2] = 0;
                CsConst.mySends.AddBufToSndList(arayLCD, 0xE012, SubNetID, DeviceID, false, false, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType));
                HDLUDP.TimeBetwnNext(arayLCD.Length);
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(9, null);
                #endregion
            }
            else if (intActivePage == 0 || intActivePage == 2) // 按键配置
            {
                #region
                    if (MyKeys == null) return;

                    for (int j = 1; j <= MyKeys.Count; j++)
                    {
                        ArayMain = new byte[24];
                        string strRemark = MyKeys[j - 1].Remark;
                        ArayMain[0] = Convert.ToByte(j);
                        Byte[] arayTmpRemark = HDLUDP.StringToByte(strRemark);
                        HDLSysPF.CopyRemarkBufferToSendBuffer(arayTmpRemark, ArayMain, 1);

                        ArayMain[21] = 4;
                        ArayMain[22] = 0;
                        ArayMain[23] = 36;
                        if (CsConst.mySends.AddBufToSndList(ArayMain, 0xE006, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                        {
                            CsConst.myRevBuf = new byte[1200];
                        }
                        else return;

                        if (MyKeys[j - 1].KeyTargets == null) continue;
                        for (int k = 0; k < MyKeys[j - 1].KeyTargets.Count; k++)
                        {
                            byte[] arayCMD = new byte[12];
                            arayCMD[0] = Convert.ToByte(j + 1);
                            arayCMD[1] = Convert.ToByte(k + 1);
                            arayCMD[2] = MyKeys[j - 1].KeyTargets[k].Type;
                            arayCMD[3] = MyKeys[j - 1].KeyTargets[k].SubnetID;
                            arayCMD[4] = MyKeys[j - 1].KeyTargets[k].DeviceID;
                            arayCMD[5] = MyKeys[j - 1].KeyTargets[k].Param1;
                            arayCMD[6] = MyKeys[j - 1].KeyTargets[k].Param2;
                            arayCMD[7] = MyKeys[j - 1].KeyTargets[k].Param3;   // save targets
                            arayCMD[8] = MyKeys[j - 1].KeyTargets[k].Param4;
                            arayCMD[9] = 4;
                            arayCMD[10] = 0;
                            arayCMD[11] = 36;
                            if (CsConst.mySends.AddBufToSndList(arayCMD, 0xE002, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                            {
                                CsConst.myRevBuf = new byte[1200];
                                HDLUDP.TimeBetwnNext(arayCMD.Length);
                            }
                            else return;
                        }
                        if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(10 + j, null);
                    }

                    Byte[] ArayButtonMode = new Byte[MyKeys.Count + 3];
                    Byte[] AraySaveAndDimmable = new Byte[MyKeys.Count + 3];
                    Byte[] ArayMutex = new Byte[MyKeys.Count + 3];

                    Byte[] ArayTmpHead = new Byte[] { 4, 0, 36 };

                    for (int j = 0; j < MyKeys.Count; j++)
                    {
                        ArayButtonMode[j + 3] = MyKeys[j].Mode;
                        AraySaveAndDimmable[j + 3] = Convert.ToByte(((MyKeys[j].IsDimmer << 4) & 0xF0) | (MyKeys[j].SaveDimmer & 0x0F));
                        ArayMutex[j + 3] = MyKeys[j].bytMutex;
                    }

                    ArayTmpHead.CopyTo(ArayButtonMode, 0);
                    if (CsConst.mySends.AddBufToSndList(ArayButtonMode, 0x19A0, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                    {
                        CsConst.myRevBuf = new byte[1200];
                    }
                    else return;

                    ArayTmpHead.CopyTo(AraySaveAndDimmable, 0);
                    if (CsConst.mySends.AddBufToSndList(AraySaveAndDimmable, 0x19A4, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                    {
                        CsConst.myRevBuf = new byte[1200];
                    }
                    else return;

                    ArayTmpHead.CopyTo(ArayMutex, 0);
                    if (CsConst.mySends.AddBufToSndList(ArayMutex, 0x19A8, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                    {
                        CsConst.myRevBuf = new byte[1200];
                    }
                    else return;                    
                #endregion
            }
            else if (intActivePage == 0 || intActivePage == 3)  // 空调
            {
                #region
                if (CsConst.isRestore)
                {
                    byte[] arayTmp = new byte[1];
                    arayTmp[0] = 2;
                    byte[] arayVisible = new byte[9];
                    if (CsConst.mySends.AddBufToSndList(arayTmp, 0x19B2, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                    {
                        Array.Copy(CsConst.myRevBuf, 26, arayVisible, 0, 9);
                        CsConst.myRevBuf = new byte[1200];
                    }
                    else return;
                    if (MyAC == null) return;
                    if (MyAC.Count < 9) return;
                    for (int i = 0; i < 9; i++)
                    {
                        byte[] ArayTmp = new byte[42];
                        ArayTmp[0] = Convert.ToByte(i);
                        ArayTmp[1] = MyAC[i].Enable;
                        ArayTmp[2] = MyAC[i].DesSubnetID;
                        ArayTmp[3] = MyAC[i].DesDevID;
                        ArayTmp[4] = MyAC[i].ACNum;
                        ArayTmp[5] = MyAC[i].ControlWays;
                        ArayTmp[6] = MyAC[i].OperationEnable;
                        ArayTmp[7] = MyAC[i].PowerOnRestoreState;
                        ArayTmp[11] = MyAC[i].FanEnable;
                        ArayTmp[12] = MyAC[i].FanEnergySaveEnable;
                        for (int j = 0; j < 11; j++)
                            ArayTmp[13 + j] = MyAC[i].FanParam[j];
                        for (int j = 0; j < 8; j++)
                            ArayTmp[24 + j] = MyAC[i].CoolParam[j];
                        for (int j = 0; j < 9; j++)
                            ArayTmp[32 + j] = MyAC[i].OutDoorParam[j];
                        ArayTmp[41] = TemperatureType;

                        if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x19AC, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                        {
                            CsConst.myRevBuf = new byte[1200];
                            if (MyAC[i].Enable == 1 && (arayVisible[i] == Convert.ToByte(i + 1)))
                            {
                                ArayTmp = new byte[10];
                                ArayTmp[0] = Convert.ToByte(i);
                                ArayTmp[1] = MyAC[i].ACSwitch;
                                ArayTmp[2] = MyAC[i].CoolingTemp;
                                ArayTmp[3] = MyAC[i].HeatingTemp;
                                ArayTmp[4] = MyAC[i].AutoTemp;
                                ArayTmp[5] = MyAC[i].DryTemp;
                                ArayTmp[6] = MyAC[i].ACMode;
                                ArayTmp[7] = MyAC[i].ACWind;
                                ArayTmp[8] = MyAC[i].ACFanEnable;
                                ArayTmp[9] = TemperatureType;
                                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x19B0, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                                {
                                    CsConst.myRevBuf = new byte[1200];
                                }
                                else return;
                            }
                        }
                        else return;
                        if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(16 + i, null);
                    }
                }
                #endregion
            }
            else if (intActivePage == 0 || intActivePage == 4) // 地热
            {
                if (MyHeat == null) return;
                foreach (EnviroFH TmpFh in MyHeat)
                {
                    TmpFh.UploadFloorheatingSettingToDevice(SubNetID, DeviceID, DeviceType, TemperatureType);
                }
            }
            else if (intActivePage == 0 || intActivePage == 5) // 音乐页面
            {
                #region
                if (CsConst.isRestore)
                {
                    if (MyMusic == null) return;
                    if (MyMusic.Count < 9) return;
                    for (int i = 0; i < 9; i++)
                    {
                        MyMusic[i].ModifyMusicSettingInformation(SubNetID, DeviceID, DeviceType);
                        MyMusic[i].ModifyMusicAdvancedCommandsGroup(SubNetID, DeviceID, DeviceType);
                        if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(32 + i, null);
                    }
                }
                #endregion
            }
            
            //base.UploadColorDLPInfoToDevice(DeviceType, DeviceName, intActivePage);

            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(100);
        }

        public void ModifyColorPagesInformation(Byte SubNetID, Byte DeviceID, int DeviceType)
        {
            #region
            Byte[] ArayTmp = new Byte[] { TotalPages, MainPage };
            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x19B8, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == false)
            {
                return;
            }

            Byte PageID = 20;
            foreach (Byte[] Tmp in IconsInPage)
            {
                ArayTmp = new Byte[Tmp.Length + 1];
                ArayTmp[0] = PageID;
                Tmp.CopyTo(ArayTmp, 1);
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x19B4, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == false)
                {
                    return;
                }
                PageID++;
            }
            #endregion
        }

        public Boolean ReadColorPagesInformation(Byte SubNetID, Byte DeviceID, int DeviceType)
        {
            Boolean bIsSuccess = false;
            try
            {
                byte[] ArayTmp = new byte[0];
                //读取总共多少页 每页的图标是什么
                #region
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x19B6, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == false)
                {
                    return bIsSuccess;
                }
                else
                {
                    TotalPages = CsConst.myRevBuf[25];
                    MainPage = CsConst.myRevBuf[26];
                    CsConst.myRevBuf = new byte[1200];
                    bIsSuccess = true;
                }

                IconsInPage = new List<byte[]>();
                for (int i = 0; i < TotalPages; i++)
                {
                    ArayTmp = new Byte[] { (Byte)(20 + i) };
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x19B2, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == false)
                    {
                        bIsSuccess = false;
                    }
                    else
                    {
                        Byte[] TmpIconsList = new Byte[16];
                        Array.Copy(CsConst.myRevBuf, 26, TmpIconsList, 0, 16);
                        CsConst.myRevBuf = new byte[1200];
                        IconsInPage.Add(TmpIconsList);
                        bIsSuccess = true;
                    }
                }
                #endregion
            }
            catch
            { }
            return bIsSuccess;
        }

    }
}
