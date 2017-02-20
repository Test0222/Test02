using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;

namespace HDL_Buspro_Setup_Tool
{
    [Serializable]
    public class EnviroPanel : HdlDeviceBackupAndRestore
    {
        public byte TotalPages; //总共多少页
        public byte MainPage;  // 返回的主界面

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
        public List<IconInfo> IconsInPage; //每个图标可以放置在多个页面
        public byte[] arayKeyMode = new byte[36];
        public const byte ButtonSum = 36;

        public List<Byte> PublicAvaibleIconArray = null;
        [Serializable]
        public class IconInfo
        {
            public Byte PageID;
            public String Remark;
            public Byte[] arrIconLists = new Byte[12];
        }


        ///<summary>
        ///读取默认的设置
        ///</summary>
        public void ReadDefaultInfo()
        {
            IconsInPage = new List<IconInfo>();
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
                if (BasicInfo == null) BasicInfo = new byte[20];
                if (BroadCastAry == null) BroadCastAry = new byte[20];
                string strRemarkBasic = "";
                string strParamBasic = TotalPages.ToString() + "-" + MainPage.ToString() + "-" + isHasSensorSensitivity.ToString() + "-" +
                                       TemperatureType.ToString();
                strsql = @"Insert into dbClassInfomation(DIndex,ClassID,ObjectID,SenNum,Remark,strParam1,byteAry1,byteAry2,byteAry3)"
                  + " values(@DIndex,@ClassID,@ObjectID,@SenNum,@Remark,@strParam1,@byteAry1,byteAry2,byteAry3)";
                //创建一个OleDbConnection对象
                OleDbConnection connBasic;
                connBasic = new OleDbConnection(DataModule.ConString + CsConst.mstrDefaultPath);
                //OleDbConnection conn = new OleDbConnection(DataModule.ConString + CsConst.mstrCurPath);
                connBasic.Open();
                OleDbCommand cmdBasic = new OleDbCommand(strsql, connBasic);
                ((OleDbParameter)cmdBasic.Parameters.Add("@DIndex", OleDbType.Integer)).Value = DIndex;
                ((OleDbParameter)cmdBasic.Parameters.Add("@ClassID", OleDbType.Integer)).Value = 0;
                ((OleDbParameter)cmdBasic.Parameters.Add("@ObjectID", OleDbType.Integer)).Value = 0;
                ((OleDbParameter)cmdBasic.Parameters.Add("@SenNum", OleDbType.Integer)).Value = 0;
                ((OleDbParameter)cmdBasic.Parameters.Add("@Remark", OleDbType.VarChar)).Value = strRemarkBasic;
                ((OleDbParameter)cmdBasic.Parameters.Add("@strParam1", OleDbType.VarChar)).Value = strParamBasic;
                ((OleDbParameter)cmdBasic.Parameters.Add("@byteAry1", OleDbType.Binary)).Value = BasicInfo;
                ((OleDbParameter)cmdBasic.Parameters.Add("@byteAry2", OleDbType.Binary)).Value = BroadCastAry;
                ((OleDbParameter)cmdBasic.Parameters.Add("@byteAry3", OleDbType.Binary)).Value = arayKeyMode;
                try
                {
                    cmdBasic.ExecuteNonQuery();
                }
                catch
                {
                    connBasic.Close();
                }
                connBasic.Close();
                #endregion

                //#region
                //if (IconsInPage != null)
                //{
                //    for (int i = 0; i < IconsInPage.Count; i++)
                //    {
                //        IconInfo tmpIcon = IconsInPage[i];
                //        string strRemark = tmpIcon.Remark;
                //        string strParam = tmpIcon.TotalPages.ToString() + "-" + tmpIcon.PageID.ToString() + "-" + tmpIcon.Index.ToString()
                //                        + "-" + tmpIcon.IconID.ToString();

                //        strsql = @"Insert into dbClassInfomation(DIndex,ClassID,ObjectID,SenNum,Remark,strParam1)"
                //          + "values(@DIndex,@ClassID,@ObjectID,@SenNum,@Remark,@strParam1)";
                //        //创建一个OleDbConnection对象
                //        OleDbConnection conn;
                //        conn = new OleDbConnection(DataModule.ConString + CsConst.mstrDefaultPath);
                //        //OleDbConnection conn = new OleDbConnection(DataModule.ConString + CsConst.mstrCurPath);
                //        conn.Open();
                //        OleDbCommand cmd = new OleDbCommand(strsql, conn);
                //        ((OleDbParameter)cmd.Parameters.Add("@DIndex", OleDbType.Integer)).Value = DIndex;
                //        ((OleDbParameter)cmd.Parameters.Add("@ClassID", OleDbType.Integer)).Value = 1;
                //        ((OleDbParameter)cmd.Parameters.Add("@ObjectID", OleDbType.Integer)).Value = 0;
                //        ((OleDbParameter)cmd.Parameters.Add("@SenNum", OleDbType.Integer)).Value = i;
                //        ((OleDbParameter)cmd.Parameters.Add("@Remark", OleDbType.VarChar)).Value = strRemark;
                //        ((OleDbParameter)cmd.Parameters.Add("@strParam1", OleDbType.VarChar)).Value = strParam;

                //        try
                //        {
                //            cmd.ExecuteNonQuery();
                //        }
                //        catch
                //        {
                //            conn.Close();
                //        }
                //        conn.Close();
                //    }
                //}
                //#endregion

                #region
                if (MyKeys != null)
                {
                    for (int i = 0; i < MyKeys.Count; i++)
                    {
                        HDLButton tmpKey = MyKeys[i];
                        string strParam = tmpKey.PageID.ToString() + "-" + tmpKey.ID.ToString() + "-" + tmpKey.IconID.ToString()
                                        + "-" + tmpKey.Mode.ToString() + "-" + tmpKey.IsLEDON.ToString() + "-" + tmpKey.IsDimmer.ToString()
                                        + "-" + tmpKey.SaveDimmer.ToString() + "-" + tmpKey.bytMutex.ToString() + "-" + tmpKey.byteLink.ToString();
                        strsql = string.Format("Insert into dbClassInfomation(DIndex,ClassID,ObjectID,SenNum,Remark,strParam1) values ({0},{1},{2},{3},'{4}','{5}')",
                                 DIndex, 2, 0, i, tmpKey.Remark, strParam);
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
                                               TmpCmds.Param2, TmpCmds.Param3, TmpCmds.Param4, tmpKey.PageID);
                                DataModule.ExecuteSQLDatabase(strsql);
                            }
                        }
                    }
                }
                #endregion

                #region
                if (MyAC != null)
                {
                    for (int i = 0; i < MyAC.Count; i++)
                    {
                        EnviroAc tmpAC = MyAC[i];
                        string strRemark = "";
                        string strParam = tmpAC.IconID.ToString() + "-" + tmpAC.ID.ToString() + "-" + tmpAC.Enable.ToString() + "-" + tmpAC.DesSubnetID.ToString()
                                        + "-" + tmpAC.DesDevID.ToString() + "-" + tmpAC.ACNum.ToString() + "-" + tmpAC.ControlWays.ToString()
                                        + "-" + tmpAC.OperationEnable.ToString() + "-" + tmpAC.PowerOnRestoreState.ToString()
                                        + "-" + tmpAC.IROperationEnable.ToString() + "-" + tmpAC.IRControlEnable.ToString()
                                        + "-" + tmpAC.IRPowerOnControlEnable.ToString() + "-" + tmpAC.FanEnable.ToString() + "-" + tmpAC.FanEnergySaveEnable.ToString()
                                        + "-" + tmpAC.ACSwitch.ToString() + "-" + tmpAC.CoolingTemp.ToString() + "-" + tmpAC.HeatingTemp.ToString()
                                        + "-" + tmpAC.AutoTemp.ToString() + "-" + tmpAC.DryTemp.ToString() + "-" + tmpAC.ACMode.ToString()
                                        + "-" + tmpAC.ACWind.ToString() + "-" + tmpAC.ACFanEnable.ToString() + "-" + tmpAC.WorkingMode.ToString()
                                        + "-" + tmpAC.WorkingWind.ToString() + "-" + tmpAC.WorkingFan.ToString() + "-" + tmpAC.EnviromentTemp.ToString();
                        strsql = @"Insert into dbClassInfomation(DIndex,ClassID,ObjectID,SenNum,Remark,strParam1,byteAry1,byteAry2,byteAry3)"
                          + "values(@DIndex,@ClassID,@ObjectID,@SenNum,@Remark,@strParam1,@byteAry1,@byteAry2,@byteAry3)";
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
                        string strParam = tmpHeat.IconID.ToString() + "-" + tmpHeat.ID.ToString() + "-" + tmpHeat.HeatEnable.ToString()
                                        + "-" + tmpHeat.SourceTemp.ToString() + "-" + tmpHeat.PIDEnable.ToString() + "-" + tmpHeat.OutputType.ToString()
                                        + "-" + tmpHeat.minPWM.ToString() + "-" + tmpHeat.maxPWM.ToString()
                                        + "-" + tmpHeat.Speed.ToString() + "-" + tmpHeat.Cycle.ToString()
                                        + "-" + tmpHeat.Switch.ToString() + "-" + tmpHeat.ProtectTemperature.ToString()
                                        + "-" + tmpHeat.ControlMode.ToString() + "-" + tmpHeat.HeatType.ToString() + "-" + tmpHeat.CompenValue.ToString()
                                        + "-" + tmpHeat.WorkingSwitch.ToString() + "-" + tmpHeat.DesSubnetID.ToString() + "-" + tmpHeat.DesDeviceID.ToString()
                                        + "-" + tmpHeat.Channel.ToString() + "-" + tmpHeat.minTemp.ToString() + "-" + tmpHeat.maxTemp.ToString()
                                        + "-" + tmpHeat.CurrentTemp.ToString() + "-" + tmpHeat.WorkingTempMode.ToString();

                        strsql = @"Insert into dbClassInfomation(DIndex,ClassID,ObjectID,SenNum,Remark,strParam1,byteAry1,byteAry2,byteAry3,byteAry4,byteAry5,byteAry6,byteAry7)"
                          + "values(@DIndex,@ClassID,@ObjectID,@SenNum,@Remark,@strParam1,@byteAry1,@byteAry2,@byteAry3,@byteAry4,@byteAry5,@byteAry6,@byteAry7)";
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
                        ((OleDbParameter)cmd.Parameters.Add("@byteAry1", OleDbType.Binary)).Value = tmpHeat.OutDoorParam;
                        ((OleDbParameter)cmd.Parameters.Add("@byteAry2", OleDbType.Binary)).Value = tmpHeat.SourceParam;
                        ((OleDbParameter)cmd.Parameters.Add("@byteAry3", OleDbType.Binary)).Value = tmpHeat.ModeAry;
                        ((OleDbParameter)cmd.Parameters.Add("@byteAry4", OleDbType.Binary)).Value = tmpHeat.TimeAry;
                        ((OleDbParameter)cmd.Parameters.Add("@byteAry5", OleDbType.Binary)).Value = tmpHeat.SysEnable;
                        ((OleDbParameter)cmd.Parameters.Add("@byteAry6", OleDbType.Binary)).Value = tmpHeat.ModeTemp;
                        ((OleDbParameter)cmd.Parameters.Add("@byteAry7", OleDbType.Binary)).Value = tmpHeat.CalculationModeTargets;
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
                        string strParam = tmpMusic.IconID.ToString() + "-" + tmpMusic.ID.ToString() + "-"
                                        + tmpMusic.Enable.ToString() + "-" + tmpMusic.CurrentZoneID.ToString() + "-"
                                        + tmpMusic.Type.ToString();
                        strsql = @"Insert into dbClassInfomation(DIndex,ClassID,ObjectID,SenNum,Remark,strParam1,byteAry1)"
                          + " values(@DIndex,@ClassID,@ObjectID,@SenNum,@Remark,@strParam1,@byteAry1)";
                        //创建一个OleDbConnection对象
                        OleDbConnection conn;
                        conn = new OleDbConnection(DataModule.ConString + CsConst.mstrDefaultPath);
                        //OleDbConnection conn = new OleDbConnection(DataModule.ConString + CsConst.mstrCurPath);
                        conn.Open();
                        OleDbCommand cmd = new OleDbCommand(strsql, conn);
                        ((OleDbParameter)cmd.Parameters.Add("@DIndex", OleDbType.Integer)).Value = DIndex;
                        ((OleDbParameter)cmd.Parameters.Add("@ClassID", OleDbType.Integer)).Value = 5;
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
                string strsql = string.Format("select * from dbClassInfomation where DIndex={0} and ClassID=0 order by SenNum", DIndex);
                OleDbDataReader dr = DataModule.SearchAResultSQLDB(strsql, CsConst.mstrCurPath);
                if (dr != null)
                {
                    while (dr.Read())
                    {
                        string str = dr.GetValue(5).ToString();
                        TotalPages = Convert.ToByte(str.Split('-')[0].ToString());
                        MainPage = Convert.ToByte(str.Split('-')[1].ToString());
                        isHasSensorSensitivity = Convert.ToBoolean(str.Split('-')[2].ToString());
                        TemperatureType = Convert.ToByte(str.Split('-')[3].ToString());
                        BasicInfo = (byte[])dr[6];
                        BroadCastAry = (byte[])dr[7];
                        arayKeyMode = (byte[])dr[8];
                    }
                    dr.Close();
                }
                #endregion

                //#region
                //strsql = string.Format("select * from dbClassInfomation where DIndex={0} and ClassID={1} order by SenNum", DIndex, 1);
                //dr = DataModule.SearchAResultSQLDB(strsql, CsConst.mstrCurPath);
                //IconsInPage = new List<IconInfo>();
                //if (dr != null)
                //{
                //    while (dr.Read())
                //    {
                //        IconInfo temp = new IconInfo();
                //        string str = dr.GetValue(5).ToString();
                //        temp.TotalPages = Convert.ToByte(str.Split('-')[0].ToString());
                //        temp.PageID = Convert.ToByte(str.Split('-')[1].ToString());
                //        temp.Index = Convert.ToByte(str.Split('-')[2].ToString());
                //        temp.IconID = Convert.ToByte(str.Split('-')[3].ToString());
                //        temp.Remark = dr.GetValue(4).ToString();
                //        IconsInPage.Add(temp);
                //    }
                //    dr.Close();
                //}
                //#endregion

                #region
                strsql = string.Format("select * from dbClassInfomation where DIndex={0} and ClassID={1} order by SenNum", DIndex, 2);
                dr = DataModule.SearchAResultSQLDB(strsql, CsConst.mstrCurPath);
                MyKeys = new List<HDLButton>();
                if (dr != null)
                {
                    while (dr.Read())
                    {
                        HDLButton temp = new HDLButton();
                        string str = dr.GetValue(5).ToString();
                        temp.PageID = Convert.ToByte(str.Split('-')[0].ToString());
                        temp.ID = Convert.ToByte(str.Split('-')[1].ToString());
                        temp.IconID = Convert.ToByte(str.Split('-')[2].ToString());
                        temp.Mode = Convert.ToByte(str.Split('-')[3].ToString());
                        temp.IsLEDON = Convert.ToByte(str.Split('-')[4].ToString());
                        temp.IsDimmer = Convert.ToByte(str.Split('-')[5].ToString());
                        temp.SaveDimmer = Convert.ToByte(str.Split('-')[6].ToString());
                        temp.bytMutex = Convert.ToByte(str.Split('-')[7].ToString());
                        temp.byteLink = Convert.ToByte(str.Split('-')[8].ToString());
                        temp.Remark = dr.GetValue(4).ToString();
                        str = string.Format("select * from dbKeyTargets where DIndex ={0} and KeyIndex = {1} and Ms04State = {2}", DIndex, temp.ID,temp.PageID);
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
                #endregion

                #region
                strsql = string.Format("select * from dbClassInfomation where DIndex={0} and ClassID=3 order by SenNum", DIndex);
                dr = DataModule.SearchAResultSQLDB(strsql, CsConst.mstrCurPath);
                MyAC = new List<EnviroAc>();
                if (dr != null)
                {
                    while (dr.Read())
                    {
                        EnviroAc temp = new EnviroAc();
                        string str = dr.GetValue(5).ToString();
                        temp.IconID = Convert.ToByte(str.Split('-')[0].ToString());
                        temp.ID = Convert.ToByte(str.Split('-')[1].ToString());
                        temp.Enable = Convert.ToByte(str.Split('-')[2].ToString());
                        temp.DesSubnetID = Convert.ToByte(str.Split('-')[3].ToString());
                        temp.DesDevID = Convert.ToByte(str.Split('-')[4].ToString());
                        temp.ACNum = Convert.ToByte(str.Split('-')[5].ToString());
                        temp.ControlWays = Convert.ToByte(str.Split('-')[6].ToString());
                        temp.OperationEnable = Convert.ToByte(str.Split('-')[7].ToString());
                        temp.PowerOnRestoreState = Convert.ToByte(str.Split('-')[8].ToString());
                        temp.IROperationEnable = Convert.ToByte(str.Split('-')[9].ToString());
                        temp.IRControlEnable = Convert.ToByte(str.Split('-')[10].ToString());
                        temp.IRPowerOnControlEnable = Convert.ToByte(str.Split('-')[11].ToString());
                        temp.FanEnable = Convert.ToByte(str.Split('-')[12].ToString());
                        temp.FanEnergySaveEnable = Convert.ToByte(str.Split('-')[13].ToString());
                        temp.ACSwitch = Convert.ToByte(str.Split('-')[14].ToString());
                        temp.CoolingTemp = Convert.ToByte(str.Split('-')[15].ToString());
                        temp.HeatingTemp = Convert.ToByte(str.Split('-')[16].ToString());
                        temp.AutoTemp = Convert.ToByte(str.Split('-')[17].ToString());
                        temp.DryTemp = Convert.ToByte(str.Split('-')[18].ToString());
                        temp.ACMode = Convert.ToByte(str.Split('-')[19].ToString());
                        temp.ACWind = Convert.ToByte(str.Split('-')[20].ToString());
                        temp.ACFanEnable = Convert.ToByte(str.Split('-')[21].ToString());
                        temp.WorkingMode = Convert.ToByte(str.Split('-')[22].ToString());
                        temp.WorkingWind = Convert.ToByte(str.Split('-')[23].ToString());
                        temp.WorkingFan = Convert.ToByte(str.Split('-')[24].ToString());
                        temp.EnviromentTemp = Convert.ToByte(str.Split('-')[25].ToString());
                        temp.FanParam = (byte[])dr[6];
                        temp.CoolParam = (byte[])dr[7];
                        temp.OutDoorParam = (byte[])dr[8];
                        MyAC.Add(temp);
                    }
                    dr.Close();
                }
                #endregion

                #region
                strsql = string.Format("select * from dbClassInfomation where DIndex={0} and ClassID=4 order by SenNum", DIndex);
                dr = DataModule.SearchAResultSQLDB(strsql, CsConst.mstrCurPath);
                MyHeat = new List<EnviroFH>();
                if (dr != null)
                {
                    while (dr.Read())
                    {
                        EnviroFH temp = new EnviroFH();
                        string str = dr.GetValue(5).ToString();
                        temp.IconID = Convert.ToByte(str.Split('-')[0].ToString());
                        temp.ID = Convert.ToByte(str.Split('-')[1].ToString());
                        temp.HeatEnable = Convert.ToByte(str.Split('-')[2].ToString());
                        temp.SourceTemp = Convert.ToByte(str.Split('-')[3].ToString());
                        temp.PIDEnable = Convert.ToByte(str.Split('-')[4].ToString());
                        temp.OutputType = Convert.ToByte(str.Split('-')[5].ToString());
                        temp.minPWM = Convert.ToByte(str.Split('-')[6].ToString());
                        temp.maxPWM = Convert.ToByte(str.Split('-')[7].ToString());
                        temp.Speed = Convert.ToByte(str.Split('-')[8].ToString());
                        temp.Cycle = Convert.ToByte(str.Split('-')[9].ToString());
                        temp.Switch = Convert.ToByte(str.Split('-')[10].ToString());
                        temp.ProtectTemperature = Convert.ToByte(str.Split('-')[11].ToString());
                        temp.ControlMode = Convert.ToByte(str.Split('-')[12].ToString());
                        temp.HeatType = Convert.ToByte(str.Split('-')[13].ToString());
                        temp.CompenValue = Convert.ToByte(str.Split('-')[14].ToString());
                        temp.WorkingSwitch = Convert.ToByte(str.Split('-')[15].ToString());
                        temp.DesSubnetID = Convert.ToByte(str.Split('-')[16].ToString());
                        temp.DesDeviceID = Convert.ToByte(str.Split('-')[17].ToString());
                        temp.Channel = Convert.ToByte(str.Split('-')[18].ToString());
                        temp.minTemp = Convert.ToByte(str.Split('-')[19].ToString());
                        temp.maxTemp = Convert.ToByte(str.Split('-')[20].ToString());
                        temp.CurrentTemp = Convert.ToByte(str.Split('-')[21].ToString());
                        temp.WorkingTempMode = Convert.ToByte(str.Split('-')[22].ToString());
                        temp.OutDoorParam = (byte[])dr[6];
                        temp.SourceParam = (byte[])dr[7];
                        temp.ModeAry = (byte[])dr[8];
                        temp.TimeAry = (byte[])dr[9];
                        temp.SysEnable = (byte[])dr[10];
                        temp.ModeTemp = (byte[])dr[11];
                        temp.CalculationModeTargets = (byte[])dr[12];
                        MyHeat.Add(temp);
                    }
                    dr.Close();
                }
                #endregion

                #region
                strsql = string.Format("select * from dbClassInfomation where DIndex={0} and ClassID=5 order by SenNum", DIndex);
                dr = DataModule.SearchAResultSQLDB(strsql, CsConst.mstrCurPath);
                MyMusic = new List<EnviroMusic>();
                if (dr != null)
                {
                    while (dr.Read())
                    {
                        EnviroMusic temp = new EnviroMusic();
                        string str = dr.GetValue(5).ToString();
                        temp.IconID = Convert.ToByte(str.Split('-')[0].ToString());
                        temp.ID = Convert.ToByte(str.Split('-')[1].ToString());
                        temp.Enable = Convert.ToByte(str.Split('-')[2].ToString());
                        temp.CurrentZoneID = Convert.ToByte(str.Split('-')[3].ToString());
                        temp.Type = Convert.ToByte(str.Split('-')[4].ToString());
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
        public void DownloadColorDLPInfoToDevice(int DeviceType, string DevName, int intActivePage,int num1,int num2)
        {
            string strMainRemark = DevName.Split('\\')[1].Trim();
            DevName = DevName.Split('\\')[0].Trim();

            byte bytSubID = byte.Parse(DevName.Split('-')[0].ToString());
            byte bytDevID = byte.Parse(DevName.Split('-')[1].ToString());
            int wdMaxValue = DeviceTypeList.GetMaxValueFromPublicModeGroup(DeviceType);
            byte[] ArayTmp = new byte[0];

            string Remark = HDLSysPF.ReadDeviceMainRemark(bytSubID, bytDevID);
            DeviceName = bytSubID.ToString() + "-" + bytDevID.ToString() + "\\" + Remark;

            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(1);

            if (intActivePage == 0 || intActivePage == 1)
            {
                #region
                if (ReadColorPagesInformation(bytSubID, bytDevID, DeviceType))
                {
                    HDLUDP.TimeBetwnNext(1);
                }
                else return;

                PublicAvaibleIconArray = new List<byte>();
                foreach (IconInfo Tmp in IconsInPage)
                {
                    foreach (Byte TmpIcon in Tmp.arrIconLists)
                    {
                        if (TmpIcon != 0) PublicAvaibleIconArray.Add(TmpIcon);
                    }
                }

                BasicInfo = new byte[20];
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xE010, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                {
                    BasicInfo[2] = CsConst.myRevBuf[25];//背光亮度
                    HDLUDP.TimeBetwnNext(1);
                }
                else return;
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(2);
              
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
                    HDLUDP.TimeBetwnNext(1);
                }
                else return;
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(3);

                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xE0E0, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                {
                    BasicInfo[4] = CsConst.myRevBuf[27];//调光低限
                    BasicInfo[5] = CsConst.myRevBuf[29];//长按时间
                    BasicInfo[15] = CsConst.myRevBuf[28];//温度显示
                    BasicInfo[16] = CsConst.myRevBuf[30];//时间显示
                    HDLUDP.TimeBetwnNext(1);
                }
                else return;
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(4);

                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xE120, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                {
                    TemperatureType = CsConst.myRevBuf[25];
                    HDLUDP.TimeBetwnNext(1);
                }
                else return;
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(5);

                BroadCastAry = new byte[20];
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xE0F8, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                {
                    Array.Copy(CsConst.myRevBuf, 25, BroadCastAry, 0, 5);
                    HDLUDP.TimeBetwnNext(1);
                }
                else return;
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(6);

                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xE128, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                {
                    BasicInfo[13] = CsConst.myRevBuf[25];//日期类型
                    BasicInfo[14] = CsConst.myRevBuf[26];//日期格式
                    HDLUDP.TimeBetwnNext(1);
                }
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(7);
                #endregion
                MyRead2UpFlags[0] = true;
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(10);
            if (intActivePage == 0 || intActivePage == 2)
            {
                #region
                MyKeys = new List<HDLButton>();
                //读取按键模式
                #region
                ArayTmp = new byte[3] { 4, 0, ButtonSum };
                arayKeyMode = new byte[36];
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x199E, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                {
                    Array.Copy(CsConst.myRevBuf, 28, arayKeyMode, 0, 36);
                    HDLUDP.TimeBetwnNext(1);
                }

                for (int i = 0; i < arayKeyMode.Length; i++)
                {
                    HDLButton temp = new HDLButton();
                    temp.ID = Convert.ToByte(i + 1);
                    temp.IconID = Convert.ToByte(i + 3);
                    temp.Mode = arayKeyMode[i];
                    temp.Remark = "";
                    temp.KeyTargets = new List<UVCMD.ControlTargets>();
                    MyKeys.Add(temp);
                }
                #endregion
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(11);
                //读取按键调光使能和保存
                #region
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x19A2, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                {
                    for (byte j = 0; j < MyKeys.Count; j++)
                    {
                        byte bytTmp = CsConst.myRevBuf[(MyKeys[j].ID - 1) + 28];
                        string str = GlobalClass.IntToBit(Convert.ToInt32(bytTmp), 8);
                        if (str.Substring(3, 1) == "1") MyKeys[j].IsDimmer = 1;
                        else MyKeys[j].IsDimmer = 0;
                        if (str.Substring(7, 1) == "1") MyKeys[j].SaveDimmer = 1;
                        else MyKeys[j].SaveDimmer = 0;
                    }
                    HDLUDP.TimeBetwnNext(1);
                }
                else return;
                #endregion
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(12);
                //读取按键互斥
                #region
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x19A6, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                {
                    for (byte j = 0; j < MyKeys.Count; j++)
                    {
                        MyKeys[j].bytMutex = CsConst.myRevBuf[(MyKeys[j].ID - 1) + 28];
                    }
                    HDLUDP.TimeBetwnNext(1);
                }
                else return;
                #endregion
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(13);
                //读取按键备注
                #region
                for (int i = 0; i < MyKeys.Count; i++)
                {
                    if (MyKeys[i].PageID == num1 || CsConst.isRestore)
                    {
                        MyKeys[i].ReadButtonRemark(bytSubID, bytDevID, DeviceType,0);
                    }
                }
                #endregion
                //读取目标
                #region
                if (CsConst.isRestore)
                {
                    for (int i = 0; i < MyKeys.Count; i++)
                    {
                        if (MyKeys[i].ReadButtonRemarkAndCMDFromDevice(bytSubID, bytDevID, DeviceType, 0, 0, false,0,0))
                        {
                            HDLUDP.TimeBetwnNext(1);
                            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(20 + 10 * i / MyKeys.Count);
                        }
                        else return;
                    }
                }
                #endregion
              
                #endregion
                MyRead2UpFlags[1] = true;
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(30);
            if (intActivePage == 0 || intActivePage == 3)
            {
                #region
                MyAC = new List<EnviroAc>();
                foreach (Byte bTmpIconId in PublicAvaibleIconArray)
                {
                    if (39 <= bTmpIconId && bTmpIconId <= 47)
                    {
                        EnviroAc temp = new EnviroAc();
                        temp.Remark = "";
                        temp.IconID = bTmpIconId;
                        temp.ID = Convert.ToByte(bTmpIconId - 39);
                        MyAC.Add(temp);
                        if (MyAC.Count >= 9) break;
                    }
                }
                //读取空调综合参数
                for (byte i = 0; i < MyAC.Count; i++)
                {
                    if (MyAC[i].ID == (num1-1) || CsConst.isRestore)
                    {
                        if (MyAC[i].ReadButtonRemark(bytSubID, bytDevID, DeviceType, true) == false) return;
                        EnviroAc temp = MyAC[i];
                        if (temp.ReadDLPACPageSettings(bytSubID, bytDevID, DeviceType,temp.ID) == false) return;
                        if (temp.Enable == 1)
                        {
                            if (temp.ReadDLPACPageCurrentStatus(bytSubID, bytDevID, DeviceType) == false) return;
                        }
                        if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(30 + i);
                        HDLUDP.TimeBetwnNext(1);
                    }
                }
                #endregion
                MyRead2UpFlags[2] = true;
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(40);
            if (intActivePage == 0 || intActivePage == 4) // d地热界面
            {
                #region
                MyHeat = new List<EnviroFH>();
                foreach (Byte bTmpIconId in PublicAvaibleIconArray)
                {
                    if (48 <= bTmpIconId && bTmpIconId <= 56)
                    {
                        EnviroFH temp = new EnviroFH();
                        temp.IconID = bTmpIconId;
                        temp.ID = Convert.ToByte(bTmpIconId - 48);
                        temp.Remark = "";
                        temp.AdvancedCommands = new UVCMD.ControlTargets[16];
                        temp.CalculationModeTargets=new byte[40];
                        MyHeat.Add(temp);
                        if (MyHeat.Count >= 9) break;
                    }
                }
                for (byte i = 0; i < MyHeat.Count; i++)
                {
                    if (MyHeat[i].ID == (num1-1) || CsConst.isRestore)
                    {
                        if (MyHeat[i].ReadButtonRemark(bytSubID, bytDevID, DeviceType, true) == false) return;
                        EnviroFH temp = MyHeat[i];
                        if (temp.DownloadFloorheatingsettingFromDevice(bytSubID, bytDevID, DeviceType) == false) return;
                        if (temp.ReadCurrentStatusFromFloorheatingModule(bytSubID, bytDevID, DeviceType) == false) return;
                        if (CsConst.isRestore)
                        {
                            if (temp.ReadSalveChannelWhenSlaveMode(bytSubID, bytDevID, DeviceType) == false) return;
                            if (temp.ReadFloorheatingAdvancedCommandsGroup(bytSubID, bytDevID, DeviceType) == false) return;
                        }
                        if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(40 + i);
                    }
                }
                #endregion
                MyRead2UpFlags[3] = true;
            }
            if (intActivePage == 0 || intActivePage == 5)  //音乐
            {
                #region
                MyMusic = new List<EnviroMusic>();
                foreach (Byte bTmpIconId in PublicAvaibleIconArray)
                {
                    if (57 <= bTmpIconId && bTmpIconId <= 65)
                    {
                        EnviroMusic temp = new EnviroMusic();
                        temp.IconID = bTmpIconId;
                        temp.ID = Convert.ToByte(bTmpIconId - 57);
                        temp.Remark = "";
                        MyMusic.Add(temp);
                        if (MyMusic.Count >= 9) break;
                    }
                }
                for (byte i = 0; i < MyMusic.Count; i++)
                {
                    if (MyMusic[i].ID == (num1-1) || CsConst.isRestore)
                    {
                        if (MyMusic[i].ReadButtonRemark(bytSubID, bytDevID, DeviceType, true) == false) return;
                        EnviroMusic temp = MyMusic[i];
                        if (temp.ReadMusicSettingInformation(bytSubID, bytDevID, DeviceType) == false) return;
                        if (temp.ReadMusicAdvancedCommandsGroup(bytSubID, bytDevID, DeviceType) == false) return;
                        if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(50 + i);
                    }
                }
                #endregion
                MyRead2UpFlags[4] = true;
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(60);
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(100);
        }
        /// <summary>
        ///上传信息到彩屏
        /// </summary>
        public void UploadColorDLPInfoToDevice(int DeviceType, string DevName, int intActivePage,int num1,int num2)
        {
            string strMainRemark = DevName.Split('\\')[1].Trim().Split('(')[0].Trim();
            DevName = DevName.Split('\\')[0].Trim();
            //保存回路信息
            byte SubNetID = byte.Parse(DevName.Split('-')[0].ToString());
            byte DeviceID = byte.Parse(DevName.Split('-')[1].ToString());
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

            if (CsConst.mySends.AddBufToSndList(ArayMain, 0x0010, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
            {

                HDLUDP.TimeBetwnNext(20);
            }
            else return;
            if (CsConst.isRestore)
            {
                ModifyColorPagesInformation(SubNetID, DeviceID, DeviceType);
            }
            if (intActivePage == 0 || intActivePage == 1)  //基本信息页面
            {
                ArayMain = new byte[4];
                Array.Copy(BroadCastAry, ArayMain, 4);
                if (CsConst.mySends.AddBufToSndList(ArayMain, 0xE0FA, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                {
                    HDLUDP.TimeBetwnNext(20);
                }
                else return;
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(1, null);
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
                    HDLUDP.TimeBetwnNext(20);
                }
                else return;
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(2, null);
                ArayMain = new byte[8];
                ArayMain[1] = BasicInfo[4];
                ArayMain[2] = BasicInfo[15];
                ArayMain[3] = BasicInfo[5];
                ArayMain[4] = BasicInfo[16];
                ArayMain[5] = 1;
                if (CsConst.mySends.AddBufToSndList(ArayMain, 0xE0E2, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                {
                    HDLUDP.TimeBetwnNext(20);
                }
                else return;
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(3, null);
                ArayMain = new byte[1];
                ArayMain[0] = TemperatureType;
                if (CsConst.mySends.AddBufToSndList(ArayMain, 0xE122, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                {
                    HDLUDP.TimeBetwnNext(20);
                }
                else return;
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(4, null);
                ArayMain = new byte[2];
                ArayMain[0] = BasicInfo[13];
                ArayMain[1] = BasicInfo[14];
                if (CsConst.mySends.AddBufToSndList(ArayMain, 0xE12A, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                {
                    HDLUDP.TimeBetwnNext(20);
                }
                else return;
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(5, null);
                byte[] arayLCD = new byte[3];
                arayLCD[0] = BasicInfo[2];
                arayLCD[1] = 0;
                arayLCD[2] = 0;
                CsConst.mySends.AddBufToSndList(arayLCD, 0xE012, SubNetID, DeviceID, false, false, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType));
                HDLUDP.TimeBetwnNext(20);
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(6, null);
                #endregion
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(10, null);
            if (intActivePage == 0 || intActivePage == 2) // 按键配置
            {
                #region
                if (MyKeys == null) return;
                byte[] ArayButtonMode = new byte[MyKeys.Count + 3];
                byte[] AraySaveAndDimmable = new byte[MyKeys.Count + 3];
                byte[] ArayMutex = new byte[MyKeys.Count + 3];

                byte[] ArayTmpHead = new byte[] { 4, 0, 36 };

                for (int j = 0; j < MyKeys.Count; j++)
                {
                    ArayButtonMode[j + 3] = MyKeys[j].Mode;
                    AraySaveAndDimmable[j + 3] = Convert.ToByte(((MyKeys[j].IsDimmer << 4) & 0xF0) | (MyKeys[j].SaveDimmer & 0x0F));
                    ArayMutex[j + 3] = MyKeys[j].bytMutex;
                }
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(11, null);
                ArayTmpHead.CopyTo(ArayButtonMode, 0);
                if (CsConst.mySends.AddBufToSndList(ArayButtonMode, 0x19A0, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                {
                    HDLUDP.TimeBetwnNext(20);
                }
                else return;
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(12, null);
                ArayTmpHead.CopyTo(AraySaveAndDimmable, 0);
                if (CsConst.mySends.AddBufToSndList(AraySaveAndDimmable, 0x19A4, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                {
                    HDLUDP.TimeBetwnNext(20);
                }
                else return;
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(13, null);
                ArayTmpHead.CopyTo(ArayMutex, 0);
                if (CsConst.mySends.AddBufToSndList(ArayMutex, 0x19A8, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                {
                    HDLUDP.TimeBetwnNext(20);
                }
                else return;
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(14, null);
                for (int j = 1; j <= MyKeys.Count; j++)
                {
                    if (MyKeys[j - 1].PageID == num1 || CsConst.isRestore)
                    {
                        ArayMain = new byte[24];
                        string strRemark = MyKeys[j - 1].Remark;
                        ArayMain[0] = Convert.ToByte(j);
                        arayTmpRemark = HDLUDP.StringToByte(strRemark);
                        if (arayTmpRemark.Length > 20)
                        {
                            Array.Copy(arayTmpRemark, 0, ArayMain, 1, 20);
                        }
                        else
                        {
                            Array.Copy(arayTmpRemark, 0, ArayMain, 1, arayTmpRemark.Length);
                        }
                        ArayMain[21] = 4;
                        ArayMain[22] = 0;
                        ArayMain[23] = 36;
                        if (CsConst.mySends.AddBufToSndList(ArayMain, 0xE006, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                        {
                            HDLUDP.TimeBetwnNext(20);
                        }
                        else return;

                        if (CsConst.isRestore)
                        {
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
                                    HDLUDP.TimeBetwnNext(20);
                                }
                                else return;
                            }
                            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(15 + j, null);
                        }
                    }
                }
                #endregion
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(30, null);
            if (intActivePage == 0 || intActivePage == 3)  // 空调
            {
                #region
                if (CsConst.isRestore)
                {
                    if (MyAC == null) return;
                    for (int i = 0; i < MyAC.Count; i++)
                    {
                        byte[] ArayTmp = new byte[42];
                        ArayTmp[0] = Convert.ToByte(MyAC[i].ID);
                        ArayTmp[1] = MyAC[i].Enable;
                        ArayTmp[2] = MyAC[i].DesSubnetID;
                        ArayTmp[3] = MyAC[i].DesDevID;
                        ArayTmp[4] = MyAC[i].ACNum;
                        ArayTmp[5] = MyAC[i].ControlWays;
                        ArayTmp[6] = MyAC[i].OperationEnable;
                        ArayTmp[7] = MyAC[i].PowerOnRestoreState;
                        ArayTmp[8] = MyAC[i].IROperationEnable;
                        ArayTmp[9] = MyAC[i].IRControlEnable;
                        ArayTmp[10] = MyAC[i].IRPowerOnControlEnable;
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
                            HDLUDP.TimeBetwnNext(20);
                            if (MyAC[i].Enable == 1 )
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
                                    HDLUDP.TimeBetwnNext(20);
                                }
                                else return;
                            }
                        }
                        else return;
                        if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(30 + i, null);
                    }
                }
                #endregion
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(40, null);
            if (intActivePage == 0 || intActivePage == 4) // 地热
            {
                #region
                if (MyHeat == null) return;
                byte byt = 1;
                foreach (EnviroFH TmpFh in MyHeat)
                {
                    if (TmpFh.UploadFloorheatingSettingToDevice(SubNetID, DeviceID, DeviceType, TemperatureType) == false) return;
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(40 + byt, null);
                    byt++;
                }
                #endregion
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(40, null);
            if (intActivePage == 0 || intActivePage == 5) // 音乐页面
            {
                #region
                if (MyMusic == null) return;
                for (int i = 0; i < MyMusic.Count; i++)
                {
                    MyMusic[i].ModifyMusicSettingInformation(SubNetID, DeviceID, DeviceType);
                    MyMusic[i].ModifyMusicAdvancedCommandsGroup(SubNetID, DeviceID, DeviceType);
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(40 + i, null);
                }

                #endregion
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(50, null);
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(100);
        }

        /// <summary>
        ///上传信息到彩屏
        /// </summary>
        public void UploadColorDLPIconsToDevice(int DeviceType, string DevName, int Type, byte[] arayaddress)
        {
            string strMainRemark = DevName.Split('\\')[1].Trim().Split('(')[0].Trim();
            DevName = DevName.Split('\\')[0].Trim();
            //保存回路信息
            byte bytSubID = byte.Parse(DevName.Split('-')[0].ToString());
            byte bytDevID = byte.Parse(DevName.Split('-')[1].ToString());
            int intMaxPacket = 0;
            int intPacketSize = 64;
            int intSendLength = 67;
            if (Type == 0 || Type == 1)//背景图片
            {
                #region
                if (arayaddress[0] <= 6)
                {
                    int StartAddress = 567652 + 65280 * arayaddress[0];
                    arayaddress[0] = Convert.ToByte(StartAddress / 256 / 256 % 256);
                    arayaddress[1] = Convert.ToByte(StartAddress / 256 % 256);
                    arayaddress[2] = Convert.ToByte(StartAddress % 256);
                    for (int i = 0; i < arayPIC.Count; i++)
                    {
                        byte[] arayTmp = arayPIC[i].ToArray();
                        if (arayTmp[0] == arayaddress[0] && arayTmp[1] == arayaddress[1] && arayTmp[2] == arayaddress[2])
                        {

                            if ((arayTmp.Length - 3) % intPacketSize != 0) intMaxPacket = Convert.ToInt32((arayTmp.Length - 3) / intPacketSize + 1);
                            else intMaxPacket = Convert.ToInt32((arayTmp.Length - 3) / intPacketSize);
                            for (int j = 0; j < intMaxPacket; j++)
                            {
                                if (CsConst.isStopDealImageBackground) break;
                                int intTmpAddress = arayaddress[0] * 256 * 256 + arayaddress[1] * 256 + arayaddress[2] + j * 64;
                                byte[] ArayFirmware = new byte[67];
                                ArayFirmware[0] = Convert.ToByte((intTmpAddress & 0xFF0000) >> 16);
                                ArayFirmware[1] = Convert.ToByte((intTmpAddress & 0xFF00) >> 8);
                                ArayFirmware[2] = Convert.ToByte((intTmpAddress % 256));
                                if ((j != intMaxPacket - 1) || ((arayTmp.Length - 3) % intPacketSize == 0))
                                    Array.Copy(arayTmp, j * intPacketSize + 3, ArayFirmware, 3, intPacketSize);
                                else if ((arayTmp.Length - 3) % intPacketSize != 0)
                                {
                                    intSendLength = 3 + (arayTmp.Length - 3) % intPacketSize;
                                    ArayFirmware = new byte[intSendLength];

                                    ArayFirmware[0] = Convert.ToByte((intTmpAddress & 0xFF0000) >> 16);
                                    ArayFirmware[1] = Convert.ToByte((intTmpAddress & 0xFF00) >> 8);
                                    ArayFirmware[2] = Convert.ToByte((intTmpAddress % 256));
                                    Array.Copy(arayTmp, j * intPacketSize + 3, ArayFirmware, 3, (arayTmp.Length - 3) % intPacketSize);
                                }
                                if (CsConst.mySends.AddBufToSndList(ArayFirmware, 0xE46A, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                                {
                                    HDLUDP.TimeBetwnNext(20);

                                }
                                else return;
                                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(90 * j / intMaxPacket);
                            }
                            break;
                        }
                    }
                }
                #endregion
            }
            if (Type == 0 || Type == 2)//图标
            {
                #region
                byte PageID = arayaddress[0];
                byte byt = 0;
                for (int i = 0; i < IconsInPage.Count; i++)
                {
                    foreach (Byte bTmpIconList in IconsInPage[i].arrIconLists)
                    {
                        int StartAddress = 0;
                        int IconID = bTmpIconList;
                        if (IconID == 1)
                        {
                            StartAddress = 1416292;
                        }
                        else if (3 <= IconID && IconID <= 38)
                        {
                            StartAddress = 1416292 + 4232 * 9 + (IconID - 3) * 4232;
                        }
                        else if (39 <= IconID && IconID <= 47)
                        {
                            StartAddress = 1416292 + 4232 * 9 + 4232 * 36 + (IconID - 39) * 4232;
                        }
                        else if (48 <= IconID && IconID <= 56)
                        {
                            StartAddress = 1416292 + 4232 * 9 + 4232 * 36 + 4232 * 9 + 4232 * 8 + (IconID - 48) * 4232;
                        }
                        else if (57 <= IconID && IconID <= 65)
                        {
                            StartAddress = 1416292 + 4232 * 9 + 4232 * 36 + 4232 * 9 + 4232 * 8 + 4232 * 9 + 4232 * 10 + (IconID - 57) * 4232;
                        }
                        arayaddress[0] = Convert.ToByte(StartAddress / 256 / 256 % 256);
                        arayaddress[1] = Convert.ToByte(StartAddress / 256 % 256);
                        arayaddress[2] = Convert.ToByte(StartAddress % 256);
                        for (int j = 0; j < arayPIC.Count; j++)
                        {
                            byte[] arayTmp = arayPIC[j].ToArray();
                            if (arayTmp[0] == arayaddress[0] && arayTmp[1] == arayaddress[1] && arayTmp[2] == arayaddress[2])
                            {
                                if ((arayTmp.Length - 4) % intPacketSize != 0) intMaxPacket = Convert.ToInt32((arayTmp.Length - 4) / intPacketSize + 1);
                                else intMaxPacket = Convert.ToInt32((arayTmp.Length - 4) / intPacketSize);
                                for (int k = 0; k < intMaxPacket; k++)
                                {
                                    if (CsConst.isStopDealImageBackground) break;
                                    int intTmpAddress = arayaddress[0] * 256 * 256 + arayaddress[1] * 256 + arayaddress[2] + k * 64;
                                    byte[] ArayFirmware = new byte[67];
                                    ArayFirmware[0] = Convert.ToByte((intTmpAddress & 0xFF0000) >> 16);
                                    ArayFirmware[1] = Convert.ToByte((intTmpAddress & 0xFF00) >> 8);
                                    ArayFirmware[2] = Convert.ToByte((intTmpAddress % 256));
                                    if ((k != intMaxPacket - 1) || ((arayTmp.Length - 4) % intPacketSize == 0))
                                        Array.Copy(arayTmp, k * intPacketSize + 4, ArayFirmware, 3, intPacketSize);
                                    else if ((arayTmp.Length - 4) % intPacketSize != 0)
                                    {
                                        intSendLength = 3 + (arayTmp.Length - 4) % intPacketSize;
                                        ArayFirmware = new byte[intSendLength];

                                        ArayFirmware[0] = Convert.ToByte((intTmpAddress & 0xFF0000) >> 16);
                                        ArayFirmware[1] = Convert.ToByte((intTmpAddress & 0xFF00) >> 8);
                                        ArayFirmware[2] = Convert.ToByte((intTmpAddress % 256));
                                        Array.Copy(arayTmp, k * intPacketSize + 4, ArayFirmware, 3, (arayTmp.Length - 4) % intPacketSize);
                                    }
                                    if (CsConst.mySends.AddBufToSndList(ArayFirmware, 0xE46A, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                                    {
                                        HDLUDP.TimeBetwnNext(20);
                                    }
                                    else return;
                                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(((90 / 12) * k) / intPacketSize + (90 * byt / 12));
                                }
                                break;
                            }
                        }
                        byt++;
                        if (byt >= 12) break;
                    }
                }
                #endregion
            }
            if (Type == 0 || Type == 3)//文字
            {
                #region
                byte PageID = arayaddress[0];
                byte byt = 0;
                for (int i = 0; i < IconsInPage.Count; i++)
                {
                    foreach( Byte bTmpIconId in IconsInPage[i].arrIconLists)
                    {
                        int StartAddress = 0;
                        int IconID = bTmpIconId;
                        if (IconID == 1)
                        {
                            StartAddress = 2259124;
                        }
                        if (IconID == 2)
                        {
                            StartAddress = 2259124 + 3840 * 9;
                        }
                        else if (3 <= IconID && IconID <= 38)
                        {
                            StartAddress = 2259124 + 3840 * 10 + (IconID - 3) * 3840;
                        }
                        else if (39 <= IconID && IconID <= 47)
                        {
                            StartAddress = 2259124 + 3840 * 10 + 3840 * 36 + (IconID - 39) * 3840;
                        }
                        else if (48 <= IconID && IconID <= 56)
                        {
                            StartAddress = 2259124 + 3840 * 10 + 3840 * 36 + 3840 * 9 + (IconID - 48) * 3840;
                        }
                        else if (57 <= IconID && IconID <= 65)
                        {
                            StartAddress = 2259124 + 3840 * 10 + 3840 * 36 + 3840 * 9 + 3840 * 9 + (IconID - 57) * 3840;
                        }
                        arayaddress[0] = Convert.ToByte(StartAddress / 256 / 256 % 256);
                        arayaddress[1] = Convert.ToByte(StartAddress / 256 % 256);
                        arayaddress[2] = Convert.ToByte(StartAddress % 256);
                        for (int j = 0; j < arayPIC.Count; j++)
                        {
                            byte[] arayTmp = arayPIC[j].ToArray();
                            if (arayTmp[0] == arayaddress[0] && arayTmp[1] == arayaddress[1] && arayTmp[2] == arayaddress[2])
                            {
                                if ((arayTmp.Length - 4) % intPacketSize != 0) intMaxPacket = Convert.ToInt32((arayTmp.Length - 4) / intPacketSize + 1);
                                else intMaxPacket = Convert.ToInt32((arayTmp.Length - 4) / intPacketSize);
                                for (int k = 0; k < intMaxPacket; k++)
                                {
                                    if (CsConst.isStopDealImageBackground) break;
                                    int intTmpAddress = arayaddress[0] * 256 * 256 + arayaddress[1] * 256 + arayaddress[2] + k * 64;
                                    byte[] ArayFirmware = new byte[67];
                                    ArayFirmware[0] = Convert.ToByte((intTmpAddress & 0xFF0000) >> 16);
                                    ArayFirmware[1] = Convert.ToByte((intTmpAddress & 0xFF00) >> 8);
                                    ArayFirmware[2] = Convert.ToByte((intTmpAddress % 256));
                                    if ((k != intMaxPacket - 1) || ((arayTmp.Length - 4) % intPacketSize == 0))
                                        Array.Copy(arayTmp, k * intPacketSize + 4, ArayFirmware, 3, intPacketSize);
                                    else if ((arayTmp.Length - 4) % intPacketSize != 0)
                                    {
                                        intSendLength = 3 + (arayTmp.Length - 4) % intPacketSize;
                                        ArayFirmware = new byte[intSendLength];

                                        ArayFirmware[0] = Convert.ToByte((intTmpAddress & 0xFF0000) >> 16);
                                        ArayFirmware[1] = Convert.ToByte((intTmpAddress & 0xFF00) >> 8);
                                        ArayFirmware[2] = Convert.ToByte((intTmpAddress % 256));
                                        Array.Copy(arayTmp, k * intPacketSize + 4, ArayFirmware, 3, (arayTmp.Length - 4) % intPacketSize);
                                    }
                                    if (CsConst.mySends.AddBufToSndList(ArayFirmware, 0xE46A, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                                    {
                                        HDLUDP.TimeBetwnNext(20);
                                    }
                                    else return;
                                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(((90 / 12) * k) / intPacketSize + (90 * byt / 12));
                                }
                                break;
                            }
                        }
                        byt++;
                        if (byt >= 12) break;
                    }
                }
                #endregion
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(100);
        }

        public bool ModifyColorPagesInformation(byte SubNetID, byte DeviceID, int DeviceType)
        {
            #region
            byte[] ArayTmp = new byte[2];
            ArayTmp[0] = TotalPages;
            ArayTmp[1] = MainPage;
            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x19B8, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
            {
                HDLUDP.TimeBetwnNext(20);
            }
            else return false;

            for (byte i = 0; i < TotalPages; i++)
            {
                ArayTmp = new byte[17];
                ArayTmp[0] = Convert.ToByte((20 + i));

                byte bytTmp = 0;
                Array.Copy(IconsInPage[i].arrIconLists,0, ArayTmp, 1, 12);

                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x19B4, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                {
                    HDLUDP.TimeBetwnNext(20);
                }
                else return false;

            }
            return true;
            #endregion
        }

        public bool ReadColorPagesInformation(byte SubNetID, byte DeviceID, int DeviceType)
        {
            byte[] ArayTmp = new byte[0];
            //读取总共多少页 每页的图标是什么
            #region
            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x19B6, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
            {
                TotalPages = CsConst.myRevBuf[25];
                MainPage = CsConst.myRevBuf[26];
                HDLUDP.TimeBetwnNext(1);
            }
            else return false;

            IconsInPage = new List<IconInfo>();
            for (int i = 0; i < TotalPages; i++)
            {
                ArayTmp = new byte[] { (byte)(20 + i) };
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x19B2, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                {
                    IconInfo temp = new IconInfo();
                    temp.PageID = (byte)(i + 1);
                    temp.arrIconLists = new Byte[12];
                    for (byte j = 1; j <= 12; j++)
                    {
                       temp.arrIconLists[j - 1] = CsConst.myRevBuf[25 + j];
                    }
                    temp.Remark = "";
                    IconsInPage.Add(temp);
                    HDLUDP.TimeBetwnNext(1);
                }
                else return false;
            }
            #endregion
            return true;
        }

        public List<Byte> GetIconListFrmSomePage(Byte bPageId)
        {
            List<Byte> arrResult = null;
            try
            {
                if (TotalPages == 0) return arrResult;
                if (IconsInPage == null || IconsInPage.Count == 0) return arrResult;
                if (bPageId > IconsInPage.Count) return arrResult;

                arrResult = IconsInPage[bPageId].arrIconLists.ToList();  
            }
            catch
            {
                return arrResult;
            }
            return arrResult;
        }

    }
}
