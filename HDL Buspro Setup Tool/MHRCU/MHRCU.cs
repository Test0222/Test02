using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HDL_Buspro_Setup_Tool
{
     [Serializable]
    public class MHRCU : HdlDeviceBackupAndRestore
    {
        public object Clone()
        {
            return this.MemberwiseClone();
        }
        public int DIndex;//设备唯一编号
        public bool[] MyRead2UpFlags = new bool[20]; //
        public bool isOutput;//场景现场输出
        //基本信息
        public string Devname;//子网ID 设备ID 备注

        //通信设置参数
        public byte dimmerLow;
        public byte[] bytAryExclusion = new byte[8];
        public byte[] arayTime = new byte[20];

        public byte byt232Paut; // 波特率
        public byte byt232Paity; // 停止位  校验位

        internal List<Channel> ChnList;
        internal List<Area> Areas;
        internal List<Curtain> Curtains;
        internal List<MS04Key> MSKeys = null;
        internal List<UVCMD.SecurityInfo> myKeySeu = null;
        internal MYHVAC myHVAC = null;
        internal List<Rs232ToBus> myRS2BUS; // Rs232 to bus commands 
        internal List<BUS2RS> myBUS2RS; // bus to  RS232 commands
        internal List<Rs232ToBus> my4852BUS; // standrad bus 485 to bus commands 
        internal List<BUS2RS> myBUS2485; // bus to  standrad bus commands
        public List<byte[]> myFilter;
        internal List<RCULogic> myLogic;
        [Serializable]
        public class BUS2RS
        {
            public byte ID;
            public string Remark;
            public byte bytType;
            public byte bytParam1;
            public byte bytParam2;
            public List<Rs232Param> RS232CMD;
        }
        [Serializable]
        internal class Channel
        {
            public int ID;
            public string Remark;
            public int LoadType;
            public int MinValue;
            public int MaxValue;
            public int MaxLevel;
            public int PowerOnDelay;
            public byte ProtectDealy;
            public byte intBelongs;
            public int AutoClose;//楼梯灯自动关闭
            public bool stairsEnable;//楼梯灯使能
        }
        [Serializable]
        internal class Area
        {
            public byte ID;
            public string Remark;
            public List<Scene> Scen;
            public byte bytDefaultSce = 0;  // 上电恢复 FF 表示掉电前场景
        }
        [Serializable]
        internal class Scene
        {
            public byte ID;
            public string Remark;
            public int Time;
            public List<byte> light;
        }
        [Serializable]
        internal class Curtain
        {
            public int ID;
            public int StartDelay;
            public int CloseDelay;
            public int Runtime;
            public bool Enable;
        }
        [Serializable]
        internal class MYHVAC
        {
            public byte Interval;
            public byte[] arayTest;
            public byte[] arayTime;
            public byte[] arayHost;
            public byte[] arayProtect;
            public byte[] arayMode;
            public byte[] arayRelay;
            public byte[] araComplex;
        }
        [Serializable]
        internal class RCULogic
        {
            public byte ID;
            public string Remark;
            public byte Enable;
            public byte Relation;
            public int ConditionEnable;
            public int TrueTime;
            public int FalseTime;
            public byte[] arayTime1;
            public byte[] arayTime2;
            public byte DryNum1;
            public byte Dry1;
            public byte DryNum2;
            public byte Dry2;
            public byte UV1;
            public byte UV2;
            public byte UVCondition1;
            public byte UVCondition2;
            public byte LogicNO;
            public byte LogicState;
            public List<UVCMD.ControlTargets> Setup;
            public List<UVCMD.ControlTargets> NoSetup;
        }

        //<summary>
        //读取数据库面板设置，将所有数据读至缓存
        //</summary>
        public void ReadDataFrmDBTobuf(int intDIndex,int DeviceType)
        {
            try
            {
                #region
                string strsql = string.Format("select * from dbClassInfomation where DIndex={0} and ClassID={1} order by SenNum", DIndex, 0);
                OleDbDataReader dr = DataModule.SearchAResultSQLDB(strsql, CsConst.mstrCurPath);
                ChnList = new List<Channel>();
                if (dr != null)
                {
                    while (dr.Read())
                    {
                        Channel temp = new Channel();
                        string str = dr.GetValue(5).ToString();
                        temp.ID = Convert.ToInt32(str.Split('-')[0].ToString());
                        temp.LoadType = Convert.ToInt32(str.Split('-')[1].ToString());
                        temp.MinValue = Convert.ToInt32(str.Split('-')[2].ToString());
                        temp.MaxValue = Convert.ToInt32(str.Split('-')[3].ToString());
                        temp.MaxLevel = Convert.ToInt32(str.Split('-')[4].ToString());
                        temp.PowerOnDelay = Convert.ToInt32(str.Split('-')[5].ToString());
                        temp.ProtectDealy = Convert.ToByte(str.Split('-')[6].ToString());
                        temp.intBelongs = Convert.ToByte(str.Split('-')[7].ToString());
                        temp.AutoClose = Convert.ToInt32(str.Split('-')[8].ToString());
                        temp.stairsEnable = Convert.ToBoolean(str.Split('-')[9].ToString());
                        temp.Remark = dr.GetValue(4).ToString();
                        ChnList.Add(temp);
                    }
                    dr.Close();
                }
                #endregion

                #region
                strsql = string.Format("select * from dbClassInfomation where DIndex={0} and ClassID={1} order by SenNum", DIndex, 1);
                dr = DataModule.SearchAResultSQLDB(strsql, CsConst.mstrCurPath);
                Areas = new List<Area>();
                if (dr != null)
                {
                    while (dr.Read())
                    {
                        Area temp = new Area();
                        string str = dr.GetValue(5).ToString();
                        temp.ID = Convert.ToByte(str.Split('-')[0].ToString());
                        temp.bytDefaultSce = Convert.ToByte(str.Split('-')[1].ToString());
                        temp.Remark = dr.GetValue(4).ToString();
                        temp.Scen = new List<Scene>();
                        string str1 = string.Format("select * from dbClassInfomation where DIndex = {0} and ClassID = {1} and ObjectID = {2} order by SenNum", DIndex, 2, temp.ID);
                        OleDbDataReader dr1 = DataModule.SearchAResultSQLDB(str1, CsConst.mstrCurPath);
                        if (dr1 != null)
                        {
                            while (dr1.Read())
                            {
                                Scene temp1 = new Scene();
                                string strTmp1 = dr1.GetValue(5).ToString();
                                temp1.ID = Convert.ToByte(strTmp1.Split('-')[0].ToString());
                                temp1.Time = Convert.ToInt32(strTmp1.Split('-')[1].ToString());
                                temp1.Remark = dr1.GetValue(4).ToString();
                                temp1.light = ((byte[])dr1[6]).ToList();
                                temp.Scen.Add(temp1);
                            }
                            dr1.Close();
                        }
                        
                        Areas.Add(temp);
                    }
                    dr.Close();
                }
                #endregion

                #region
                strsql = string.Format("select * from dbClassInfomation where DIndex={0} and ClassID={1} order by SenNum", DIndex, 3);
                dr = DataModule.SearchAResultSQLDB(strsql, CsConst.mstrCurPath);
                Curtains = new List<Curtain>();
                if (dr != null)
                {
                    while (dr.Read())
                    {
                        Curtain temp = new Curtain();
                        string str = dr.GetValue(5).ToString();
                        temp.ID = Convert.ToByte(str.Split('-')[0].ToString());
                        temp.StartDelay = Convert.ToInt32(str.Split('-')[1].ToString());
                        temp.CloseDelay = Convert.ToInt32(str.Split('-')[2].ToString());
                        temp.Runtime = Convert.ToInt32(str.Split('-')[3].ToString());
                        temp.Enable = Convert.ToBoolean(str.Split('-')[4].ToString());
                        Curtains.Add(temp);
                    }
                    dr.Close();
                }
                #endregion

                #region
                strsql = string.Format("select * from dbClassInfomation where DIndex={0} and ClassID={1} order by SenNum", DIndex, 4);
                dr = DataModule.SearchAResultSQLDB(strsql, CsConst.mstrCurPath);
                myKeySeu = new List<UVCMD.SecurityInfo>();
                if (dr != null)
                {
                    while (dr.Read())
                    {
                        UVCMD.SecurityInfo temp = new UVCMD.SecurityInfo();
                        string str = dr.GetValue(5).ToString();
                        temp.bytSeuID = Convert.ToByte(str.Split('-')[0].ToString());
                        temp.bytTerms = Convert.ToByte(str.Split('-')[1].ToString());
                        temp.bytSubID = Convert.ToByte(str.Split('-')[2].ToString());
                        temp.bytDevID = Convert.ToByte(str.Split('-')[3].ToString());
                        temp.bytArea = Convert.ToByte(str.Split('-')[4].ToString());
                        myKeySeu.Add(temp);
                    }
                    dr.Close();
                }
                #endregion

                #region
                myRS2BUS = new List<Rs232ToBus>();
                strsql = string.Format("select * from dbClassInfomation where DIndex={0} and ClassID={1} order by SenNum", DIndex, 5);
                dr = DataModule.SearchAResultSQLDB(strsql, CsConst.mstrCurPath);
                if (dr != null)
                {
                    while (dr.Read())
                    {
                        Rs232ToBus temp = new Rs232ToBus();
                        temp.rs232Param = new Rs232Param();
                        temp.busTargets = new List<UVCMD.ControlTargets>();
                        string str = dr.GetValue(5).ToString();
                        temp.ID = Convert.ToByte(str.Split('-')[0].ToString());
                        temp.rs232Param.enable  = Convert.ToByte(str.Split('-')[1].ToString());
                        temp.rs232Param.type = Convert.ToByte(str.Split('-')[2].ToString());
                       // temp.rs232Param.rsCmd = ((byte[])dr[6]);
                        temp.remark = dr.GetValue(4).ToString();
                        string str1 = string.Format("select * from dbKeyTargets where DIndex ={0} and KeyIndex = {1} and Ms04State = {2}", DIndex, temp.ID, 2);
                        OleDbDataReader drKeyCmds = DataModule.SearchAResultSQLDB(str1, CsConst.mstrCurPath);
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
                                temp.busTargets.Add(TmpCmd);
                            }
                            drKeyCmds.Close();
                        }
                        myRS2BUS.Add(temp);
                    }
                    dr.Close();
                }
                #endregion

                #region
                myBUS2RS = new List<BUS2RS>();
                strsql = string.Format("select * from dbClassInfomation where DIndex={0} and ClassID={1} order by SenNum", DIndex, 6);
                dr = DataModule.SearchAResultSQLDB(strsql, CsConst.mstrCurPath);
                if (dr != null)
                {
                    while (dr.Read())
                    {
                        BUS2RS temp = new BUS2RS();
                        string str = dr.GetValue(5).ToString();
                        temp.ID = Convert.ToByte(str.Split('-')[0].ToString());
                        temp.bytType = Convert.ToByte(str.Split('-')[1].ToString());
                        temp.bytParam1 = Convert.ToByte(str.Split('-')[2].ToString());
                        temp.bytParam2 = Convert.ToByte(str.Split('-')[3].ToString());
                        temp.Remark = dr.GetValue(4).ToString();
                        temp.RS232CMD = new List<Rs232Param>();
                        string str1 = string.Format("select * from dbClassInfomation where DIndex = {0} and ClassID = {1} and ObjectID = {2} order by SenNum", DIndex, 7, temp.ID);
                        OleDbDataReader drKeyCmds = DataModule.SearchAResultSQLDB(str1, CsConst.mstrCurPath);
                        if (drKeyCmds != null)
                        {
                            while (drKeyCmds.Read())
                            {
                                str = drKeyCmds.GetValue(5).ToString();
                                Rs232Param tmp = new Rs232Param();
                                tmp.Index = Convert.ToByte(drKeyCmds.GetValue(3).ToString());
                                tmp.enable = Convert.ToByte(str.Split('-')[0].ToString());
                                tmp.type = Convert.ToByte(str.Split('-')[1].ToString());
                               // tmp.rsCmd = ((byte[])drKeyCmds[6]);
                                temp.RS232CMD.Add(tmp);
                            }
                            drKeyCmds.Close();
                        }
                        myBUS2RS.Add(temp);
                    }
                    dr.Close();
                }
                #endregion

                #region
                myFilter = new List<byte[]>();
                strsql = string.Format("select * from dbClassInfomation where DIndex={0} and ClassID={1} order by SenNum", DIndex, 8);
                dr = DataModule.SearchAResultSQLDB(strsql, CsConst.mstrCurPath);
                if (dr != null)
                {
                    while (dr.Read())
                    {
                        byte[] temp;
                        string str = dr.GetValue(5).ToString();
                        temp = ((byte[])dr[6]);
                        myFilter.Add(temp);
                    }
                    dr.Close();
                }
                #endregion

                

                #region
                strsql = string.Format("select * from dbClassInfomation where DIndex={0} and ClassID={1} order by SenNum", DIndex, 9);
                dr = DataModule.SearchAResultSQLDB(strsql, CsConst.mstrCurPath);
                if (dr != null)
                {
                    while (dr.Read())
                    {
                        string str = dr.GetValue(5).ToString();
                        byt232Paut = Convert.ToByte(str.Split('-')[0].ToString());
                        byt232Paity = Convert.ToByte(str.Split('-')[1].ToString());
                        dimmerLow = Convert.ToByte(str.Split('-')[2].ToString());
                        bytAryExclusion = ((byte[])dr[6]);
                        arayTime = ((byte[])dr[7]);
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
        /// 保存数据库面板设置，将所有数据保存
        /// </summary>
        public void SaveDataToDB()
        {
            try
            {
                string strsql = string.Format("delete * from dbClassInfomation where DIndex={0}", DIndex);
                DataModule.ExecuteSQLDatabase(strsql);

                strsql = string.Format("delete * from dbKeyTargets where DIndex={0}", DIndex);
                DataModule.ExecuteSQLDatabase(strsql);

                strsql = string.Format("delete * from dbMS04 where DIndex={0}", DIndex);
                DataModule.ExecuteSQLDatabase(strsql);

                #region
                if (ChnList != null)
                {
                    for (int i = 0; i < ChnList.Count; i++)
                    {
                        Channel tmpChn = ChnList[i];
                        string strParam = tmpChn.ID.ToString() + "-" + tmpChn.LoadType.ToString() + "-"
                                        + tmpChn.MinValue.ToString() + "-" + tmpChn.MaxValue.ToString() + "-"
                                        + tmpChn.MaxLevel.ToString() + "-" + tmpChn.PowerOnDelay.ToString() + "-"
                                        + tmpChn.ProtectDealy.ToString() + "-" + tmpChn.intBelongs.ToString() + "-"
                                        + tmpChn.AutoClose.ToString() + "-" + tmpChn.stairsEnable.ToString();
                        strsql = string.Format("Insert into dbClassInfomation(DIndex,ClassID,ObjectID,SenNum,Remark,strParam1) values ({0},{1},{2},{3},'{4}','{5}')",
                                 DIndex, 0, 0, i, tmpChn.Remark, strParam);
                        DataModule.ExecuteSQLDatabase(strsql);

                    }
                }
                #endregion

                #region
                if (Areas != null)
                {
                    for (int i = 0; i < Areas.Count; i++)
                    {
                        Area tmpArea = Areas[i];
                        string strParam = tmpArea.ID.ToString() + "-" + tmpArea.bytDefaultSce.ToString();
                        strsql = string.Format("Insert into dbClassInfomation(DIndex,ClassID,ObjectID,SenNum,Remark,strParam1) values ({0},{1},{2},{3},'{4}','{5}')",
                                 DIndex, 1, 0, i, tmpArea.Remark, strParam);
                        DataModule.ExecuteSQLDatabase(strsql);
                        if (tmpArea.Scen != null)
                        {
                            for (int j = 0; j < tmpArea.Scen.Count; j++)
                            {
                                if (tmpArea.Scen[j].light == null) tmpArea.Scen[j].light = new List<byte>();
                                byte[] arayTmp = tmpArea.Scen[j].light.ToArray();
                                if (arayTmp == null) arayTmp = new byte[0];
                                string strRemark = tmpArea.Scen[j].Remark;
                                if (strRemark == null) strRemark = "";
                                strParam = tmpArea.Scen[j].ID.ToString() + "-" + tmpArea.Scen[j].Time.ToString();
                                strsql = @"Insert into dbClassInfomation(DIndex,ClassID,ObjectID,SenNum,Remark,strParam1,byteAry1)"
                                  + " values(@DIndex,@ClassID,@ObjectID,@SenNum,@Remark,@strParam1,@byteAry1)";
                                //创建一个OleDbConnection对象
                                OleDbConnection conn;
                                conn = new OleDbConnection(DataModule.ConString + CsConst.mstrDefaultPath);
                                //OleDbConnection conn = new OleDbConnection(DataModule.ConString + CsConst.mstrCurPath);
                                conn.Open();
                                OleDbCommand cmd = new OleDbCommand(strsql, conn);
                                ((OleDbParameter)cmd.Parameters.Add("@DIndex", OleDbType.Integer)).Value = DIndex;
                                ((OleDbParameter)cmd.Parameters.Add("@ClassID", OleDbType.Integer)).Value = 2;
                                ((OleDbParameter)cmd.Parameters.Add("@ObjectID", OleDbType.Integer)).Value = tmpArea.ID;
                                ((OleDbParameter)cmd.Parameters.Add("@SenNum", OleDbType.Integer)).Value = j;
                                ((OleDbParameter)cmd.Parameters.Add("@Remark", OleDbType.VarChar)).Value = strRemark;
                                ((OleDbParameter)cmd.Parameters.Add("@strParam1", OleDbType.VarChar)).Value = strParam;
                                ((OleDbParameter)cmd.Parameters.Add("@byteAry1", OleDbType.Binary)).Value = arayTmp;
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
                    }
                }
                #endregion

                #region
                if (Curtains != null)
                {
                    for (int i = 0; i < Curtains.Count; i++)
                    {
                        Curtain tmpChn = Curtains[i];
                        string strParam = tmpChn.ID.ToString() + "-" + tmpChn.StartDelay.ToString() + "-"
                                        + tmpChn.CloseDelay.ToString() + "-" + tmpChn.Runtime.ToString() + "-"
                                        + tmpChn.Enable.ToString();
                        strsql = string.Format("Insert into dbClassInfomation(DIndex,ClassID,ObjectID,SenNum,Remark,strParam1) values ({0},{1},{2},{3},'{4}','{5}')",
                                 DIndex, 3, 0, i, "", strParam);
                        DataModule.ExecuteSQLDatabase(strsql);

                    }
                }
                #endregion

                #region
                if (myKeySeu != null)
                {
                    for (int i = 0; i < myKeySeu.Count; i++)
                    {
                        UVCMD.SecurityInfo tmpChn = myKeySeu[i];
                        string strParam = tmpChn.bytSeuID.ToString() + "-" + tmpChn.bytTerms.ToString() + "-"
                                        + tmpChn.bytSubID.ToString() + "-"
                                        + tmpChn.bytDevID.ToString() + "-" + tmpChn.bytArea.ToString();
                        strsql = string.Format("Insert into dbClassInfomation(DIndex,ClassID,ObjectID,SenNum,Remark,strParam1) values ({0},{1},{2},{3},'{4}','{5}')",
                                 DIndex, 4, 0, i, "", strParam);
                        DataModule.ExecuteSQLDatabase(strsql);

                    }
                }
                #endregion

                #region
                if (myRS2BUS != null)
                {
                    for (int i = 0; i < myRS2BUS.Count; i++)
                    {
                        Rs232ToBus tmpChn = myRS2BUS[i];
                        if (tmpChn.rs232Param.rsCmd == null) tmpChn.rs232Param.rsCmd = "";
                        string strParam = tmpChn.ID.ToString() + "-" + tmpChn.rs232Param.enable.ToString() + "-"
                                        + tmpChn.rs232Param.type.ToString();
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
                        ((OleDbParameter)cmd.Parameters.Add("@SenNum", OleDbType.Integer)).Value = tmpChn.ID;
                        ((OleDbParameter)cmd.Parameters.Add("@Remark", OleDbType.VarChar)).Value = tmpChn.remark;
                        ((OleDbParameter)cmd.Parameters.Add("@strParam1", OleDbType.VarChar)).Value = strParam;
                        ((OleDbParameter)cmd.Parameters.Add("@byteAry1", OleDbType.Binary)).Value = tmpChn.rs232Param.rsCmd;
                        try
                        {
                            cmd.ExecuteNonQuery();
                        }
                        catch
                        {
                            conn.Close();
                        }
                        conn.Close();
                        if (tmpChn.busTargets != null)
                        {
                            for (int j = 0; j < tmpChn.busTargets.Count; j++)
                            {
                                UVCMD.ControlTargets TmpCmds = tmpChn.busTargets[j];
                                ///// insert into all commands to database
                                strsql = string.Format("Insert into dbKeyTargets(DIndex,KeyIndex,objID,KeyFunType,SubNetID,DeviceID,FirstParameter,"
                                                + "SecondParameter,RunTimeMinute,RunTimeSecond,Ms04State) values ({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10})",
                                               DIndex, tmpChn.ID, TmpCmds.ID, TmpCmds.Type, TmpCmds.SubnetID, TmpCmds.DeviceID, TmpCmds.Param1,
                                               TmpCmds.Param2, TmpCmds.Param3, TmpCmds.Param4, 2);
                                DataModule.ExecuteSQLDatabase(strsql);
                            }
                        }
                    }
                }
                #endregion

                #region
                if (myBUS2RS != null)
                {
                    for (int i = 0; i < myBUS2RS.Count; i++)
                    {
                        BUS2RS tmpChn = myBUS2RS[i];
                        string strParam = tmpChn.ID.ToString() + "-" + tmpChn.bytType.ToString() + "-"
                                        + tmpChn.bytParam1.ToString() + "-" + tmpChn.bytParam2.ToString();
                        strsql = string.Format("Insert into dbClassInfomation(DIndex,ClassID,ObjectID,SenNum,Remark,strParam1) values ({0},{1},{2},{3},'{4}','{5}')",
                                 DIndex, 6, 0, i, "", strParam);
                        DataModule.ExecuteSQLDatabase(strsql);
                        if (tmpChn.RS232CMD != null)
                        {
                            for (int j = 0; j < tmpChn.RS232CMD.Count; j++)
                            {
                                Rs232Param tmp = tmpChn.RS232CMD[j];
                                if (tmp.rsCmd == null) tmp.rsCmd = ""; // new byte[0];
                                strParam = tmp.enable.ToString() + "-" + tmp.type.ToString();
                                strsql = @"Insert into dbClassInfomation(DIndex,ClassID,ObjectID,SenNum,Remark,strParam1,byteAry1)"
                                         + " values(@DIndex,@ClassID,@ObjectID,@SenNum,@Remark,@strParam1,@byteAry1)";
                                //创建一个OleDbConnection对象
                                OleDbConnection conn;

                                conn = new OleDbConnection(DataModule.ConString + CsConst.mstrDefaultPath);
                                //OleDbConnection conn = new OleDbConnection(DataModule.ConString + CsConst.mstrCurPath);
                                conn.Open();
                                OleDbCommand cmd = new OleDbCommand(strsql, conn);
                                ((OleDbParameter)cmd.Parameters.Add("@DIndex", OleDbType.Integer)).Value = DIndex;
                                ((OleDbParameter)cmd.Parameters.Add("@ClassID", OleDbType.Integer)).Value = 7;
                                ((OleDbParameter)cmd.Parameters.Add("@ObjectID", OleDbType.Integer)).Value = tmpChn.ID;
                                ((OleDbParameter)cmd.Parameters.Add("@SenNum", OleDbType.Integer)).Value = tmp.Index;
                                ((OleDbParameter)cmd.Parameters.Add("@Remark", OleDbType.VarChar)).Value = "";
                                ((OleDbParameter)cmd.Parameters.Add("@strParam1", OleDbType.VarChar)).Value = strParam;
                                ((OleDbParameter)cmd.Parameters.Add("@byteAry1", OleDbType.Binary)).Value = tmp.rsCmd;
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
                    }
                }
                #endregion

                #region
                if (myFilter != null)
                {
                    for (int i = 0; i < myFilter.Count; i++)
                    {
                        byte[] tmpChn = myFilter[i];
                        string strParam = "";
                        strsql = @"Insert into dbClassInfomation(DIndex,ClassID,ObjectID,SenNum,Remark,strParam1,byteAry1)"
                                 + " values(@DIndex,@ClassID,@ObjectID,@SenNum,@Remark,@strParam1,@byteAry1)";
                        //创建一个OleDbConnection对象
                        OleDbConnection conn;
                        conn = new OleDbConnection(DataModule.ConString + CsConst.mstrDefaultPath);
                        //OleDbConnection conn = new OleDbConnection(DataModule.ConString + CsConst.mstrCurPath);
                        conn.Open();
                        OleDbCommand cmd = new OleDbCommand(strsql, conn);
                        ((OleDbParameter)cmd.Parameters.Add("@DIndex", OleDbType.Integer)).Value = DIndex;
                        ((OleDbParameter)cmd.Parameters.Add("@ClassID", OleDbType.Integer)).Value = 8;
                        ((OleDbParameter)cmd.Parameters.Add("@ObjectID", OleDbType.Integer)).Value = 0;
                        ((OleDbParameter)cmd.Parameters.Add("@SenNum", OleDbType.Integer)).Value = i;
                        ((OleDbParameter)cmd.Parameters.Add("@Remark", OleDbType.VarChar)).Value = "";
                        ((OleDbParameter)cmd.Parameters.Add("@strParam1", OleDbType.VarChar)).Value = strParam;
                        ((OleDbParameter)cmd.Parameters.Add("@byteAry1", OleDbType.Binary)).Value = tmpChn;
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
                string strParamBasic = byt232Paut.ToString() + "-" + byt232Paity.ToString() + "-" + dimmerLow.ToString();
                if (bytAryExclusion == null) bytAryExclusion = new byte[8];
                if (arayTime == null) arayTime = new byte[20];

                strsql = @"Insert into dbClassInfomation(DIndex,ClassID,ObjectID,SenNum,Remark,strParam1,byteAry1,byteAry2)"
                         + " values(@DIndex,@ClassID,@ObjectID,@SenNum,@Remark,@strParam1,@byteAry1,@byteAry2)";
                //创建一个OleDbConnection对象
                OleDbConnection connBasic;
                connBasic = new OleDbConnection(DataModule.ConString + CsConst.mstrDefaultPath);
                //OleDbConnection conn = new OleDbConnection(DataModule.ConString + CsConst.mstrCurPath);
                connBasic.Open();
                OleDbCommand cmdBasic = new OleDbCommand(strsql, connBasic);
                ((OleDbParameter)cmdBasic.Parameters.Add("@DIndex", OleDbType.Integer)).Value = DIndex;
                ((OleDbParameter)cmdBasic.Parameters.Add("@ClassID", OleDbType.Integer)).Value = 9;
                ((OleDbParameter)cmdBasic.Parameters.Add("@ObjectID", OleDbType.Integer)).Value = 0;
                ((OleDbParameter)cmdBasic.Parameters.Add("@SenNum", OleDbType.Integer)).Value = 0;
                ((OleDbParameter)cmdBasic.Parameters.Add("@Remark", OleDbType.VarChar)).Value = "";
                ((OleDbParameter)cmdBasic.Parameters.Add("@strParam1", OleDbType.VarChar)).Value = strParamBasic;
                ((OleDbParameter)cmdBasic.Parameters.Add("@byteAry1", OleDbType.Binary)).Value = bytAryExclusion;
                ((OleDbParameter)cmdBasic.Parameters.Add("@byteAry2", OleDbType.Binary)).Value = arayTime;
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
            ChnList = new List<Channel>();
            Areas = new List<Area>();
            Curtains = new List<Curtain>();
            MSKeys = new List<MS04Key>();
            myKeySeu = new List<UVCMD.SecurityInfo>();
            myHVAC = new MYHVAC();
            myRS2BUS = new List<Rs232ToBus>();
            myBUS2RS = new List<BUS2RS>();
            my4852BUS = new List<Rs232ToBus>();
            myBUS2485 = new List<BUS2RS>();
            myFilter = new List<byte[]>();
            myLogic = new List<RCULogic>();
        }

        public void DownloadMHRCUInforsFrmDevice(string strDevName, int deviceType, int intActivePage, int num1, int num2)
        {
            string strMainRemark = strDevName.Split('\\')[1].Trim();
            strDevName = strDevName.Split('\\')[0].Trim();

            byte bytSubID = byte.Parse(strDevName.Split('-')[0].ToString());
            byte bytDevID = byte.Parse(strDevName.Split('-')[1].ToString());

            byte[] ArayTmp = new byte[0];
            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x000E, bytSubID, bytDevID, false, true, true, false) == true)
            {
                byte[] arayRemark = new byte[20];
                for (int intI = 0; intI < 20; intI++) { arayRemark[intI] = CsConst.myRevBuf[25 + intI]; }
                Devname = bytSubID.ToString() + "-" + bytDevID.ToString() + "\\" + HDLPF.Byte2String(arayRemark);
                
                HDLUDP.TimeBetwnNext(1);
            }
            else return;
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(1, null);
            if (intActivePage == 0 || intActivePage == 2 || intActivePage == 3 || intActivePage == 4 || intActivePage==8)
            {
                //读取回路备注
                int ChannelCount = 22;
                if (deviceType == 3503 || deviceType == 3504) ChannelCount = 20;
                ChnList = new List<Channel>();
                #region
                ArayTmp = new byte[1];
                for (int i = 0; i < ChannelCount; i++)
                {
                    Channel ch = new Channel();
                    ch.ID = i + 1;
                    ch.LoadType = 0;
                    ch.MinValue = 0;
                    ch.MaxValue = 100;
                    ch.MaxLevel = 100;
                    ch.PowerOnDelay = 0;
                    ch.ProtectDealy = 0;
                    ch.stairsEnable = false;
                    ch.AutoClose = 0;

                    ArayTmp[0] = (byte)(i + 1);
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xF00E, bytSubID, bytDevID, false, true, true, false) == true)
                    {
                        if (CsConst.myRevBuf != null)
                        {
                            byte[] arayRemark = new byte[20];
                            for (int intI = 0; intI < 20; intI++) { arayRemark[intI] = CsConst.myRevBuf[26 + intI]; }
                            ch.Remark = HDLPF.Byte2String(arayRemark);
                        }
                        
                        HDLUDP.TimeBetwnNext(1);
                        if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(1 + i * 10 / ChannelCount, null);
                    }
                    else
                    {
                        return;
                    }
                    ChnList.Add(ch);
                }
                #endregion
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(10, null);

            if (intActivePage == 0 || intActivePage == 2 || intActivePage == 4)
            {
                //读取区域信息
                #region
                int ChannelCount = 22;
                if (deviceType == 3503 || deviceType == 3504) ChannelCount = 20;
                Areas = new List<Area>();
                int bytTalArea = 0;
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x0004, bytSubID, bytDevID, false, true, true, false) == true)
                {
                    bytTalArea = Convert.ToInt32(CsConst.myRevBuf[29]);
                    for (int intI = 0; intI < ChannelCount; intI++)
                    {
                        ChnList[intI].intBelongs = CsConst.myRevBuf[30 + intI];
                        if (bytTalArea == 0) ChnList[intI].intBelongs = 0;
                        if (ChnList[intI].intBelongs > ChannelCount) ChnList[intI].intBelongs = 0;
                    }
                    

                    for (int intI = 0; intI < bytTalArea; intI++)
                    {
                        Area area = new Area();
                        area.ID = (byte)(intI + 1);
                        ArayTmp = new byte[1];
                        ArayTmp[0] = (byte)(intI + 1);
                        //区域备注
                        if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xF00A, bytSubID, bytDevID, false, true, true, false) == true)
                        {
                            byte[] arayRemark = new byte[20];
                            for (int intJ = 0; intJ < 20; intJ++) { arayRemark[intJ] = CsConst.myRevBuf[26 + intJ]; }
                            area.Remark = HDLPF.Byte2String(arayRemark);
                        }
                        else return;
                        

                        // 读取场景信息
                        area.Scen = new List<Scene>();
                        if (intActivePage == 0 || intActivePage == 4)
                        {
                            #region
                            //读取各个区域场景信息
                            for (byte bytI = 0; bytI < 13; bytI++)
                            {

                                Scene scen = new Scene();
                                scen.ID = (byte)(bytI);

                                ArayTmp = new byte[2];
                                ArayTmp[0] = (byte)(intI + 1);
                                ArayTmp[1] = (byte)(bytI);

                                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xF024, bytSubID, bytDevID, false, true, true, false) == true)
                                {
                                    byte[] arayRemark = new byte[20];
                                    for (int intJ = 0; intJ < 20; intJ++) { arayRemark[intJ] = CsConst.myRevBuf[27 + intJ]; }
                                    scen.Remark = HDLPF.Byte2String(arayRemark);
                                }
                                else return;
                                
                                HDLUDP.TimeBetwnNext(1);

                                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x0000, bytSubID, bytDevID, false, true, true, false) == true)
                                {
                                    scen.Time = CsConst.myRevBuf[27] * 256 + CsConst.myRevBuf[28];
                                    byte[] ArayByt = new byte[ChannelCount];
                                    for (int intJ = 0; intJ < ChannelCount; intJ++) { ArayByt[intJ] = CsConst.myRevBuf[29 + intJ]; }
                                    scen.light = ArayByt.ToList();
                                }
                                else return;
                                
                                HDLUDP.TimeBetwnNext(1);
                                area.Scen.Add(scen);
                            }
                            #endregion
                            MyRead2UpFlags[3] = true;
                        }
                        Areas.Add(area);
                        if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(10 + intI, null);
                    }
                    //读取各个区域场景掉电恢复
                    if (intActivePage == 0 || intActivePage == 4)
                    {
                        if (Areas != null && Areas.Count > 0)
                        {
                            ArayTmp = new byte[0];
                            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xF051, bytSubID, bytDevID, false, true, true, false) == true)
                            {
                                for (int i = 0; i < Areas.Count; i++)
                                {
                                    Areas[i].bytDefaultSce = CsConst.myRevBuf[25 + i];
                                    if (Areas[i].bytDefaultSce == 0) Areas[i].bytDefaultSce = 255;
                                }
                                
                                HDLUDP.TimeBetwnNext(1);
                            }
                            else return;

                            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xF055, bytSubID, bytDevID, false, true, true, false) == true)
                            {
                                for (int i = 0; i < Areas.Count; i++)
                                {
                                    if (Areas[i].bytDefaultSce != 255)
                                    {
                                        Areas[i].bytDefaultSce = CsConst.myRevBuf[25 + i];
                                    }
                                }
                                
                                HDLUDP.TimeBetwnNext(1);
                            }
                        }
                    }
                }
                else return;
                #endregion
                if (intActivePage == 2)
                    MyRead2UpFlags[1] = true;
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(20, null);
            if (intActivePage == 0 || intActivePage == 3)
            {
                //读取回路信息
                #region
                int ChannelCount = 22;
                if (deviceType == 3503 || deviceType == 3504) ChannelCount = 20;
                ArayTmp = null;
                // read load type
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xF012, bytSubID, bytDevID, false, true, true, false) == true)
                {
                    for (int intI = 0; intI < ChannelCount; intI++)
                    {
                        ChnList[intI].LoadType = CsConst.myRevBuf[25 + intI];

                        if (ChnList[intI].LoadType == 255 || ChnList[intI].LoadType > 7) ChnList[intI].LoadType = 0;
                    }
                    
                    HDLUDP.TimeBetwnNext(1);
                }
                else return;
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(21, null);
                // read low limit
                ArayTmp = new byte[1];
                ArayTmp[0] = 0;
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xF016, bytSubID, bytDevID, false, true, true, false) == true)
                {
                    for (int intI = 0; intI < ChannelCount; intI++)
                    {
                        ChnList[intI].MinValue = CsConst.myRevBuf[26 + intI];
                    }
                    
                    HDLUDP.TimeBetwnNext(1);
                }
                else return;
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(22, null);
                // read high limit
                ArayTmp[0] = 1;
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xF016, bytSubID, bytDevID, false, true, true, false) == true)
                {
                    for (int intI = 0; intI < ChannelCount; intI++)
                    {
                        ChnList[intI].MaxValue = CsConst.myRevBuf[26 + intI];
                    }
                    
                    HDLUDP.TimeBetwnNext(1);
                }
                else return;
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(23, null);
                // read High Level
                ArayTmp = null;
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xF020, bytSubID, bytDevID, false, true, true, false) == true)
                {
                    for (int intI = 0; intI < ChannelCount; intI++)
                    {
                        ChnList[intI].MaxLevel = CsConst.myRevBuf[25 + intI];
                    }
                    
                    HDLUDP.TimeBetwnNext(1);
                }
                else return;
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(24, null);
                // read on delay
                ArayTmp = new byte[1];
                ArayTmp[0] = 3;
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xE908, bytSubID, bytDevID, false, true, true, false) == true)
                {
                    for (int intI = 0; intI < ChannelCount; intI++)
                    {
                        ChnList[intI].PowerOnDelay = CsConst.myRevBuf[26 + intI];
                    }
                    
                    HDLUDP.TimeBetwnNext(1);
                }
                else return;
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(25, null);
                // read protoct delay
                ArayTmp[0] = 4;
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xE908, bytSubID, bytDevID, false, true, true, false) == true)
                {
                    for (int intI = 0; intI < ChannelCount; intI++)
                    {
                        ChnList[intI].ProtectDealy = CsConst.myRevBuf[26 + intI];
                    }
                    
                    HDLUDP.TimeBetwnNext(1);
                }
                else return;
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(26, null);
                #endregion
                MyRead2UpFlags[2] = true;
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(30, null);
            if (intActivePage == 0 || intActivePage == 4 || intActivePage == 5)
            {
                //读取窗帘回路信息
                #region
                ArayTmp = null;
                Curtains = new List<Curtain>();
                int CurtainCount = 4;

                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1F18, bytSubID, bytDevID, false, true, true, false) == true)
                {
                    if (CsConst.myRevBuf[16] == 0x0F) CurtainCount = 4;
                    else if (CsConst.myRevBuf[16] == 0x13) CurtainCount = 8;
                    for (int intI = 0; intI < CurtainCount; intI++)
                    {
                        Curtain temp = new Curtain();
                        temp.ID = intI + 1;
                        if (CsConst.myRevBuf[25 + intI] == 1)
                        {
                            temp.Enable = true;
                        }
                        else
                        {
                            temp.Enable = false;
                        }
                        Curtains.Add(temp);
                    }
                    
                    HDLUDP.TimeBetwnNext(1);
                }
                else return;
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(31, null);
                //读取窗帘延时，运行时间
                #region
                if (intActivePage == 0 || intActivePage == 5)
                {
                    for (int i = 0; i < Curtains.Count; i++)
                    {
                        Curtain temp = Curtains[i];
                        if (temp.Enable)
                        {
                            //读取开启延时
                            ArayTmp = new byte[1];
                            ArayTmp[0] = Convert.ToByte(temp.ID);
                            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xE800, bytSubID, bytDevID, false, true, true, false) == true)
                            {
                                temp.StartDelay = CsConst.myRevBuf[26] * 256 + CsConst.myRevBuf[27];
                                
                                HDLUDP.TimeBetwnNext(1);
                            }
                            else return;
                            //读取关闭延时
                            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xE809, bytSubID, bytDevID, false, true, true, false) == true)
                            {
                                temp.CloseDelay = CsConst.myRevBuf[25] * 256 + CsConst.myRevBuf[26];
                                
                                HDLUDP.TimeBetwnNext(1);
                            }
                            else return;
                            //读取运行时间
                            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xE804, bytSubID, bytDevID, false, true, true, false) == true)
                            {
                                temp.Runtime = CsConst.myRevBuf[26] * 256 + CsConst.myRevBuf[27];
                                
                                HDLUDP.TimeBetwnNext(1);
                            }
                            else return;
                            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(31 + i, null);
                        }
                    }
                    MyRead2UpFlags[4] = true;
                }
                #endregion
                #endregion
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(40, null);
            //干接点信息
            if (intActivePage == 0 || intActivePage == 6)
            {
                #region
                int DryCount = 24;
                if (deviceType == 3503 || deviceType == 3504) DryCount = 20;
                MSKeys = new List<MS04Key>();
                // read keys enable or not  and repeat flag

                //读取按键使能
                #region
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x0128, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(deviceType)) == true)
                {
                    for (byte bytI = 0; bytI < DryCount; bytI++)
                    {
                        MS04Key oTmp = new MS04Key();
                        oTmp.ID = Convert.ToByte(bytI + 1);
                        if (CsConst.myRevBuf != null)
                        {
                            oTmp.bytEnable = CsConst.myRevBuf[25 + bytI];
                        }
                        MSKeys.Add(oTmp);
                    }
                    CsConst.myRevBuf = new byte[1200];
                    HDLUDP.TimeBetwnNext(1);
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(11, null);
                }
                else return;
                #endregion

                //读取面板锁键使能
                #region
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x0158, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(deviceType)) == true)
                {
                    for (byte bytI = 0; bytI < DryCount; bytI++)
                    {
                        if (CsConst.myRevBuf != null)
                        {
                            MSKeys[bytI].bytReflag = CsConst.myRevBuf[25 + bytI];
                            MSKeys[bytI].Mode = 3;
                        }
                    }
                    CsConst.myRevBuf = new byte[1200];
                    HDLUDP.TimeBetwnNext(1);
                }
                else return;
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(12, null);
                #endregion

                // 读取模式
                #region
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xD205, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(deviceType)) == true)
                {
                    for (byte bytI = 0; bytI < DryCount; bytI++)
                    {
                        if (CsConst.myRevBuf != null)
                        {
                            MSKeys[bytI].Mode = CsConst.myRevBuf[26 + bytI];
                        }
                    }
                    CsConst.myRevBuf = new byte[1200];
                    HDLUDP.TimeBetwnNext(1);
                }
                else return;
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(13, null);
                #endregion

                // read dimming mode or switch mode d230
                #region
               Byte[] ArayDimDirection = ReadButtonDimDirectionFrmDeviceToBuf(bytSubID, bytDevID);
                for (byte bytI = 0; bytI < DryCount; bytI++)
                {
                    if (CsConst.myRevBuf != null)
                    {
                        MSKeys[bytI].IsDimmer = ArayDimDirection[bytI];
                    }
                }
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(14, null);
                #endregion

                // read dimmer or not 
                #region
                Byte[] AraySaveDim = ReadButtonDimFlagFrmDeviceToBuf(bytSubID, bytDevID);
                for (Byte bytI = 0; bytI < MSKeys.Count; bytI++) // in AraySaveDim)
                {
                    MSKeys[bytI].SaveDimmer = AraySaveDim[bytI];
                }
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(15, null);
                #endregion

                // read low Limit 
                #region
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xE0E0, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(deviceType)) == true)
                {
                    dimmerLow = CsConst.myRevBuf[26];
                    CsConst.myRevBuf = new byte[1200];
                    HDLUDP.TimeBetwnNext(1);
                }
                else return;
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(16, null);
                #endregion

                //read all keys setup : remark delay targets
                #region
                for (byte bytI = 0; bytI < DryCount; bytI++)
                {
                    ArayTmp = new byte[2];
                    ArayTmp[0] = bytI;
                    ArayTmp[1] = 0;

                    // read remark  不管什么类型读取备注 读取公共的命令  即 ON 的 命令 目标
                    #region
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xD210, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(deviceType)) == true)
                    {
                        byte[] arayRemark = new byte[20];
                        HDLSysPF.CopyRemarkBufferFrmMyRevBuffer(CsConst.myRevBuf, arayRemark, 28);
                        MSKeys[bytI].Remark = HDLPF.Byte2String(arayRemark);
                        CsConst.myRevBuf = new byte[1200];
                        HDLUDP.TimeBetwnNext(10);
                    }
                    else return;
                    #endregion

                    ReadDryCommandsGroupFrmDeviceToBuf(bytSubID, bytDevID, deviceType, bytI, 1);

                    if (MSKeys[bytI].Mode == 0)
                    {
                        ReadDryCommandsGroupFrmDeviceToBuf(bytSubID, bytDevID, deviceType, bytI, 0);
                    }
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(20 + bytI * 10 / DryCount, null);
                }
                #endregion
                MyRead2UpFlags[5] = true;
                #endregion
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(50, null);
            if (intActivePage == 0 || intActivePage == 7)
            {
                #region
                int DryCount = 24;
                if (deviceType == 3503 || deviceType == 3504) DryCount = 20;
                //安防设置
                myKeySeu = new List<UVCMD.SecurityInfo>();
                ArayTmp = new byte[1];
                
                for (byte intI = 1; intI <= DryCount; intI++)
                {
                    ArayTmp[0] = intI;
                    //读取备注
                    UVCMD.SecurityInfo oTmp = new UVCMD.SecurityInfo();
                    oTmp.bytSeuID = intI;
                    //读取设置
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x15DA, bytSubID, bytDevID, false, true, true, false) == true)
                    {
                        oTmp.bytTerms = CsConst.myRevBuf[26];
                        oTmp.bytSubID = CsConst.myRevBuf[27];
                        oTmp.bytDevID = CsConst.myRevBuf[28];
                        oTmp.bytArea = CsConst.myRevBuf[29];
                        myKeySeu.Add(oTmp);
                        
                        HDLUDP.TimeBetwnNext(1);
                    }
                    else return;
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(50 + (intI * 10 / DryCount), null);
                }
                #endregion
                MyRead2UpFlags[6] = true;
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(60, null);
            //继电器新功能
            if (intActivePage == 0 || intActivePage == 8)
            {
                #region
                if (ChnList == null) return;
                if (ChnList.Count > 16)
                {
                    ArayTmp = new byte[0];
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xE0E4, bytSubID, bytDevID, false, true, true, false) == true)
                    {
                        for (int i = 0; i < 16; i++)
                        {
                            if (CsConst.myRevBuf[25 + i] == 1)
                                ChnList[i].stairsEnable = true;
                            else
                                ChnList[i].stairsEnable = false;
                        }
                        
                        HDLUDP.TimeBetwnNext(1);
                    }
                    else return;
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(61);
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xE0E8, bytSubID, bytDevID, false, true, true, false) == true)
                    {
                        for (int i = 0; i < 16; i++)
                        {
                            ChnList[i].AutoClose = CsConst.myRevBuf[25 + (2 * i)] * 256 + CsConst.myRevBuf[25 + ((2 * i) + 1)];
                        }
                        
                        HDLUDP.TimeBetwnNext(1);
                    }
                    else return;
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(62);
                    bytAryExclusion = new byte[8];
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1700, bytSubID, bytDevID, false, true, true, false) == true)
                    {
                        for (int i = 0; i < 8; i++)
                        {
                            bytAryExclusion[i] = CsConst.myRevBuf[25 + i];
                        }
                        
                        HDLUDP.TimeBetwnNext(1);
                    }
                    else return;
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(63);
                }
                #endregion
                MyRead2UpFlags[7] = true;
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(70, null);
            //HVAC
            if (intActivePage == 0 || intActivePage == 9)
            {
                #region
                /*
                if (deviceType == 3502)
                {
                    myHVAC = new MYHVAC();
                    myHVAC.arayTest = new byte[3];
                    myHVAC.arayTime = new byte[4];
                    myHVAC.arayHost = new byte[20];
                    myHVAC.arayProtect = new byte[40];
                    myHVAC.arayMode = new byte[4];
                    myHVAC.arayRelay = new byte[6];
                    myHVAC.araComplex = new byte[35];
                    ArayTmp = new byte[0];
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1C96, bytSubID, bytDevID, false, true, true) == true)
                    {
                        Array.Copy(CsConst.myRevBuf, 25, myHVAC.arayTest, 0, 3);
                        HDLUDP.TimeBetwnNext(10);
                        
                    }
                    else return;
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(90);
                   
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xE3F4, bytSubID, bytDevID, false, true, true) == true)
                    {
                        Array.Copy(CsConst.myRevBuf, 25, myHVAC.arayTime, 0, 4);
                        HDLUDP.TimeBetwnNext(10);
                        
                    }
                    else return;
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(91);

                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1C46, bytSubID, bytDevID, false, true, true) == true)
                    {
                        Array.Copy(CsConst.myRevBuf, 25, myHVAC.arayHost, 0, 19);
                        HDLUDP.TimeBetwnNext(10);
                        
                    }
                    else return;
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(92);

                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1C4E, bytSubID, bytDevID, false, true, true) == true)
                    {
                        Array.Copy(CsConst.myRevBuf, 25, myHVAC.arayProtect, 0, 36);
                        HDLUDP.TimeBetwnNext(10);
                        
                    }
                    else return;

                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1C92, bytSubID, bytDevID, false, true, true) == true)
                    {
                        myHVAC.Interval = CsConst.myRevBuf[25];
                        HDLUDP.TimeBetwnNext(10);
                        
                    }
                    else return;

                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(93);

                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1C9A, bytSubID, bytDevID, false, true, true) == true)
                    {
                        Array.Copy(CsConst.myRevBuf, 25, myHVAC.arayMode, 0, 4);
                        HDLUDP.TimeBetwnNext(10);
                        
                    }
                    else return;
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(94);

                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1C9E, bytSubID, bytDevID, false, true, true) == true)
                    {
                        Array.Copy(CsConst.myRevBuf, 25, myHVAC.arayRelay, 0, 6);
                        HDLUDP.TimeBetwnNext(10);
                        
                    }
                    else return;
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(95);
                    ArayTmp = new byte[1];
                    for (byte i = 0; i < 5; i++)
                    {
                        ArayTmp[0] = i;
                        if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1CA2, bytSubID, bytDevID, false, true, true) == true)
                        {
                            Array.Copy(CsConst.myRevBuf, 25, myHVAC.araComplex, i * 7, 7);
                            HDLUDP.TimeBetwnNext(10);
                            
                        }
                        else return;
                    }
                }*/
                #endregion
                MyRead2UpFlags[8] = true;
            }
            if (intActivePage == 0 || intActivePage == 10)
            {
                #region
                if (deviceType == 3503 || deviceType == 3504)
                {
                    myRS2BUS = new List<Rs232ToBus>();
                    if (CsConst.isRestore)
                    {
                        num1 = 1;
                        num2 = 49;
                    }
                    for (int i = num1; i <= num2; i++)
                    {
                        Rs232ToBus temp = new Rs232ToBus();
                        temp.rs232Param = new Rs232Param();
                        ArayTmp = new byte[1];
                        ArayTmp[0] = Convert.ToByte(i);
                        if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xE410, bytSubID, bytDevID, false, true, true, false) == true)
                        {
                            temp.ID = Convert.ToByte(i);
                            temp.rs232Param.enable = CsConst.myRevBuf[27];
                            temp.rs232Param.type = CsConst.myRevBuf[28];
                           // temp.rs232Param.rsCmd = new byte[33 + 1];
                           // Array.Copy(CsConst.myRevBuf, 29, temp.TmpRS232.RSCMD, 0, temp.TmpRS232.RSCMD.Length);
                            
                            HDLUDP.TimeBetwnNext(1);
                        }
                        else return;
                        if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xE418, bytSubID, bytDevID, false, true, true, false) == true)
                        {
                            byte[] arayRemark = new byte[20];
                            for (int intI = 0; intI < 20; intI++) { arayRemark[intI] = CsConst.myRevBuf[27 + intI]; }
                            temp.remark = HDLPF.Byte2String(arayRemark);
                            
                            HDLUDP.TimeBetwnNext(1);
                        }
                        else return;
                        temp.busTargets = Read232ToBusCommandsGroupFrmDeviceToBuf(bytSubID, bytDevID, deviceType, (Byte)(i-1));
                        myRS2BUS.Add(temp);
                        if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(70 + (i * 10 / (num2)));
                    }
                }
                #endregion
                MyRead2UpFlags[9] = true;
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(80, null);
            if (intActivePage == 0 || intActivePage == 11)
            {
                #region
                if (deviceType == 3503 || deviceType == 3504)
                {
                    myBUS2RS = new List<BUS2RS>();
                    if (CsConst.isRestore)
                    {
                        num1 = 1;
                        num2 = 49;
                    }
                    for (int i = num1; i <= num2; i++)
                    {
                        BUS2RS temp = new BUS2RS();
                        temp.ID = Convert.ToByte(i);
                        ArayTmp = new byte[1];
                        ArayTmp[0] = Convert.ToByte(i);
                        if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xE420, bytSubID, bytDevID, false, true, true, false) == true)
                        {
                            temp.bytType = CsConst.myRevBuf[27];
                            temp.bytParam1 = CsConst.myRevBuf[28];
                            temp.bytParam2 = CsConst.myRevBuf[29];
                            
                            HDLUDP.TimeBetwnNext(1);
                        }
                        else return;

                        if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xE428, bytSubID, bytDevID, false, true, true, false) == true)
                        {
                            byte[] arayRemark = new byte[20];
                            for (int intI = 0; intI < 20; intI++) { arayRemark[intI] = CsConst.myRevBuf[27 + intI]; }
                            temp.Remark = HDLPF.Byte2String(arayRemark);
                            
                            HDLUDP.TimeBetwnNext(1);
                        }
                        else return;

                        temp.RS232CMD = new List<Rs232Param>(); // List<RS232Param>();
                        if (CsConst.isRestore)
                        {
                            for (int j = 1; j <= 20; j++)
                            {
                                ArayTmp = new byte[2];
                                ArayTmp[0] = Convert.ToByte(i);
                                ArayTmp[1] = Convert.ToByte(j);

                                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xE424, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(deviceType)) == true)
                                {
                                    Rs232Param tmp = new Rs232Param();
                                    tmp.Index = Convert.ToByte(j);
                                    tmp.enable = CsConst.myRevBuf[28];
                                    tmp.type = CsConst.myRevBuf[29];
                                    //tmp.rsCmd = new byte[33 + 1];
                                    //Array.Copy(CsConst.myRevBuf, 30, tmp.RSCMD, 0, tmp.RSCMD.Length);
                                    //temp.RS232CMD.Add(tmp);
                                    
                                    HDLUDP.TimeBetwnNext(1);
                                }
                                else return;
                            }
                        }
                        myBUS2RS.Add(temp);
                        if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(80 + (i * 10 / num2));
                    }
                }
                #endregion
                MyRead2UpFlags[10] = true;
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(90, null);
            if (intActivePage == 0 || intActivePage == 12)
            {
                #region
                if (deviceType == 3503 || deviceType == 3504)
                {
                    myFilter = new List<byte[]>();
                    for (int i = 0; i < 16; i++)
                    {
                        byte[] arayFilter = new byte[4];
                        ArayTmp = new byte[1];
                        ArayTmp[0] = Convert.ToByte(i);
                        if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1CBC, bytSubID, bytDevID, false, true, true, false) == true)
                        {
                            Array.Copy(CsConst.myRevBuf, 25, arayFilter, 0, 4);
                            myFilter.Add(arayFilter);
                            
                            HDLUDP.TimeBetwnNext(1);
                        }
                        else return;
                        if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(90 + (i * 5 / 16));
                    }
                }
                #endregion
                MyRead2UpFlags[11] = true;
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(95, null);
            if (intActivePage == 0 || intActivePage == 13)
            {
                #region
                if (deviceType == 3504)
                {
                    myLogic = new List<RCULogic>();
                    arayTime = new byte[20];
                    byte[] arayTmp = new byte[0];
                    if (CsConst.mySends.AddBufToSndList(arayTmp, 0xDA00, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(deviceType)) == true)
                    {
                        Array.Copy(CsConst.myRevBuf, 26, arayTime, 0, 6);
                        
                        HDLUDP.TimeBetwnNext(1);
                    }
                    else return;
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(96, null);
                    if (CsConst.mySends.AddBufToSndList(arayTmp, 0xDA40, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(deviceType)) == true)
                    {
                        arayTime[7] = CsConst.myRevBuf[26];
                        
                        HDLUDP.TimeBetwnNext(1);
                    }
                    else return;
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(97, null);
                    if (CsConst.mySends.AddBufToSndList(arayTmp, 0x1618, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(deviceType)) == true)
                    {
                        for (byte i = 1; i <= 24; i++)
                        {
                            RCULogic temp = new RCULogic();
                            temp.ID = i;
                            temp.arayTime1 = new byte[11];
                            temp.arayTime2 = new byte[11];
                            temp.Enable = CsConst.myRevBuf[25 + i];
                            myLogic.Add(temp);
                        }
                        
                        HDLUDP.TimeBetwnNext(1);
                    }
                    else return;

                    for (byte i = 1; i <= 24; i++)
                    {
                        RCULogic temp = myLogic[i - 1];
                        if (temp.Enable != 1) continue;
                        arayTmp = new byte[1];
                        arayTmp[0] = i;
                        if (CsConst.mySends.AddBufToSndList(arayTmp, 0x1606, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(deviceType)) == true)
                        {
                            byte[] arayRemark = new byte[20];
                            for (int intI = 0; intI < 20; intI++) { arayRemark[intI] = CsConst.myRevBuf[27 + intI]; }
                            temp.Remark = HDLPF.Byte2String(arayRemark);
                            
                            HDLUDP.TimeBetwnNext(1);
                        }
                        else return;

                        if (CsConst.isRestore)
                        {
                            if (CsConst.mySends.AddBufToSndList(arayTmp, 0x1704, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(deviceType)) == true)
                            {
                                temp.Relation = CsConst.myRevBuf[27];
                                temp.ConditionEnable = CsConst.myRevBuf[28] *256+ CsConst.myRevBuf[29];
                                if (temp.ConditionEnable == 65535) temp.ConditionEnable = 0;
                                temp.TrueTime = CsConst.myRevBuf[30] * 256 + CsConst.myRevBuf[31];
                                temp.FalseTime = CsConst.myRevBuf[32] * 256 + CsConst.myRevBuf[33];
                                Array.Copy(CsConst.myRevBuf, 34, temp.arayTime1, 0,11);
                                Array.Copy(CsConst.myRevBuf, 45, temp.arayTime2, 0,11);
                                temp.DryNum1 = CsConst.myRevBuf[56];
                                temp.Dry1 = CsConst.myRevBuf[57];
                                temp.DryNum2 = CsConst.myRevBuf[58];
                                temp.Dry2 = CsConst.myRevBuf[59];
                                temp.UV1 = CsConst.myRevBuf[60];
                                temp.UVCondition1 = CsConst.myRevBuf[61];
                                temp.UV2 = CsConst.myRevBuf[62];
                                temp.UVCondition2 = CsConst.myRevBuf[63];
                                temp.LogicNO = CsConst.myRevBuf[64];
                                temp.LogicState = CsConst.myRevBuf[65];
                                
                                HDLUDP.TimeBetwnNext(1);
                            }
                            else return;
                        }
                        if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(97 + (i * 2) / 24, null);
                    }
                }
                #endregion
                MyRead2UpFlags[12] = true;
            }
            if (intActivePage == 0 || intActivePage == 14)
            {
                #region
                if (deviceType == 3504)
                {
                    my4852BUS = new List<Rs232ToBus>();
                    if (CsConst.isRestore)
                    {
                        num1 = 1;
                        num2 = 49;
                    }
                    for (int i = num1; i <= num2; i++)
                    {
                        Rs232ToBus temp = new Rs232ToBus();
                        temp.rs232Param = new Rs232Param();
                        ArayTmp = new byte[1];
                        ArayTmp[0] = Convert.ToByte(i);
                        if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xDA51, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(deviceType)) == true)
                        {
                            temp.ID = Convert.ToByte(i);
                            temp.rs232Param.enable = CsConst.myRevBuf[27];
                            temp.rs232Param.type = CsConst.myRevBuf[28];
                            //temp.rs232Param.rsCmd = new byte[33 + 1];
                            //Array.Copy(CsConst.myRevBuf, 29, temp.TmpRS232.RSCMD, 0, temp.TmpRS232.RSCMD.Length);
                            
                            HDLUDP.TimeBetwnNext(1);
                        }
                        else return;

                        if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xDA59, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(deviceType)) == true)
                        {
                            byte[] arayRemark = new byte[20];
                            for (int intI = 0; intI < 20; intI++) { arayRemark[intI] = CsConst.myRevBuf[27 + intI]; }
                            temp.remark = HDLPF.Byte2String(arayRemark);
                            
                            HDLUDP.TimeBetwnNext(1);
                        }
                        else return;
                        temp.busTargets = Read485ToBusCommandsGroupFrmDeviceToBuf(bytSubID, bytDevID, deviceType, (Byte)(i- 1));
                        my4852BUS.Add(temp);
                        if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(80 * i / ((num2 + 1) - num1));
                    }
                }
                #endregion
                MyRead2UpFlags[13] = true;
            }
            if (intActivePage == 0 || intActivePage == 15)
            {
                #region
                if (deviceType == 3504)
                {
                    myBUS2485 = new List<BUS2RS>();
                    if (CsConst.isRestore)
                    {
                        num1 = 1;
                        num2 = 49;
                    }
                    for (int i = num1; i <= num2; i++)
                    {
                        BUS2RS temp = new BUS2RS();
                        temp.ID = Convert.ToByte(i);
                        ArayTmp = new byte[1];
                        ArayTmp[0] = Convert.ToByte(i);
                        if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xDA61, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(deviceType)) == true)
                        {
                            temp.bytType = CsConst.myRevBuf[27];
                            temp.bytParam1 = CsConst.myRevBuf[28];
                            temp.bytParam2 = CsConst.myRevBuf[29];
                            
                            HDLUDP.TimeBetwnNext(1);
                        }
                        else return;

                        if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xDA69, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(deviceType)) == true)
                        {
                            byte[] arayRemark = new byte[20];
                            for (int intI = 0; intI < 20; intI++) { arayRemark[intI] = CsConst.myRevBuf[27 + intI]; }
                            temp.Remark = HDLPF.Byte2String(arayRemark);
                            
                            HDLUDP.TimeBetwnNext(1);
                        }
                        else return;
                        temp.RS232CMD = new List<Rs232Param>();
                        if (CsConst.isRestore)
                        {
                            int num = 20;

                            for (int j = 1; j <= num; j++)
                            {
                                ArayTmp = new byte[2];
                                ArayTmp[0] = Convert.ToByte(i);
                                ArayTmp[1] = Convert.ToByte(j);

                                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xDA65, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(deviceType)) == true)
                                {
                                    Rs232Param tmp = new Rs232Param();
                                    tmp.Index = Convert.ToByte(j);
                                    tmp.enable = CsConst.myRevBuf[28];
                                    tmp.type = CsConst.myRevBuf[29];
                                   // tmp.rsCmd = new byte[33 + 1];
                                    //Array.Copy(CsConst.myRevBuf, 30, tmp.RSCMD, 0, tmp.RSCMD.Length);
                                    temp.RS232CMD.Add(tmp);
                                    
                                    HDLUDP.TimeBetwnNext(1);
                                }
                                else return;
                            }
                        }
                        myBUS2485.Add(temp);
                        if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(90 + 10 * i / ((num2 + 1) - num1));
                    }
                }
                #endregion
                MyRead2UpFlags[14] = true;
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(100);
        }

        public void UploadPanelInfosToDevice(string strDevName, int deviceType, int intActivePage, int num1, int num2)
        {
            string strMainRemark = strDevName.Split('\\')[1].Trim().Split('(')[0].Trim();
            strDevName = strDevName.Split('\\')[0].Trim();
            //保存回路信息
            byte bytSubID = byte.Parse(strDevName.Split('-')[0].ToString());
            byte bytDevID = byte.Parse(strDevName.Split('-')[1].ToString());

            int wdMaxValue = 22;
            if (deviceType == 3503 || deviceType==3504) wdMaxValue = 20;

            byte[] arayRemark = new byte[21];// used for restore remark

            if (HDLSysPF.ModifyDeviceMainRemark(bytSubID, bytDevID, strMainRemark, deviceType) == true)
            {
                HDLUDP.TimeBetwnNext(20);
            }
            else return;
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(1);

            if (Areas == null) Areas = new List<Area>();
            byte[] arayArea = new byte[wdMaxValue + 3];
            arayArea[0] = bytSubID;
            arayArea[1] = bytDevID;
            arayArea[2] = (byte)Areas.Count;

            byte[] arayLoadType = new byte[wdMaxValue];
            byte[] arayOnDelay = new byte[wdMaxValue + 1]; arayOnDelay[0] = 3;
            byte[] arayProDelay = new byte[wdMaxValue + 1]; arayProDelay[0] = 4;
            byte[] arayLimitL = new byte[wdMaxValue + 1]; arayLimitL[0] = 0;
            byte[] arayLimitH = new byte[wdMaxValue + 1]; arayLimitH[0] = 1;
            byte[] arayMaxLevel = new byte[wdMaxValue];
            byte[] AutoCloseEnable = new byte[wdMaxValue];
            byte[] AutoCloseDelay = new byte[wdMaxValue * 2];
           //上传回路备注
            #region
            if (intActivePage == 0 || intActivePage == 3 || intActivePage==8)
            {
                foreach (Channel ch in ChnList)
                {   // modify the chns remark
                    arayRemark = new byte[21]; // 初始化数组
                    string strRemark = ch.Remark;
                    byte[] arayTmp = HDLUDP.StringToByte(strRemark);

                    arayRemark[0] = byte.Parse(ch.ID.ToString());
                    if (arayTmp != null) arayTmp.CopyTo(arayRemark, 1);
                    if (intActivePage == 0 || intActivePage == 3 || intActivePage == 8)
                    {
                        CsConst.mySends.AddBufToSndList(arayRemark, 0xF010, bytSubID, bytDevID, false, true, true, false);
                        
                         HDLUDP.TimeBetwnNext(20);
                    }
                    else return;
                    arayLoadType[ch.ID - 1] = byte.Parse(ch.LoadType.ToString());
                    arayOnDelay[ch.ID] = byte.Parse(ch.PowerOnDelay.ToString());
                    arayProDelay[ch.ID] = byte.Parse(ch.ProtectDealy.ToString());
                    arayMaxLevel[ch.ID - 1] = byte.Parse(ch.MaxLevel.ToString());
                    arayLimitL[ch.ID] = byte.Parse(ch.MinValue.ToString());
                    arayLimitH[ch.ID] = byte.Parse(ch.MaxValue.ToString());
                    if (ch.stairsEnable)
                        AutoCloseEnable[ch.ID - 1] = 1;
                    AutoCloseDelay[2*(ch.ID - 1)] = Convert.ToByte(ch.AutoClose / 256);
                    AutoCloseDelay[2*(ch.ID - 1)+1] = Convert.ToByte(ch.AutoClose % 256);
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(1 + ((ch.ID - 1) * 10) / ChnList.Count);
                }
            }
            #endregion
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(10);
            //上传回路信息
            #region
            if (intActivePage == 0 || intActivePage == 3)
            {
                // modify the load type
                if (CsConst.mySends.AddBufToSndList(arayLoadType, 0xF014, bytSubID, bytDevID, false, true, true, false) == true)
                {
                    
                    HDLUDP.TimeBetwnNext(20);
                }else return;

                // modify the on Low limit
                if (CsConst.mySends.AddBufToSndList(arayLimitL, 0xF018, bytSubID, bytDevID, false, true, true, false) == true)
                {
                    
                    HDLUDP.TimeBetwnNext(20);
                }else return;

                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(11);

                // modify the High Limit
                if (CsConst.mySends.AddBufToSndList(arayLimitH, 0xF018, bytSubID, bytDevID, false, true, true, false) == true)
                {
                    
                    HDLUDP.TimeBetwnNext(20);
                }else return;

                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(12);

                // modify the max level 
                if (CsConst.mySends.AddBufToSndList(arayMaxLevel, 0xF022, bytSubID, bytDevID, false, true, true, false) == true)
                {
                    
                    HDLUDP.TimeBetwnNext(20);
                }else return;

                // modify the on delay 
                if (CsConst.mySends.AddBufToSndList(arayOnDelay, 0xE90A, bytSubID, bytDevID, false, true, true, false) == true)
                {
                    
                    HDLUDP.TimeBetwnNext(20);
                }else return;

                // modify the protect delay
                if (CsConst.mySends.AddBufToSndList(arayProDelay, 0xE90A, bytSubID, bytDevID, false, true, true, false) == true)
                {
                    
                    HDLUDP.TimeBetwnNext(20);
                }else return;
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(13);
                MyRead2UpFlags[4] = true;
            }
            #endregion
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(20);
            //上传区域信息
            #region
            if (intActivePage == 0 || intActivePage == 2)
            {

                foreach (Channel ch in ChnList)
                {
                    arayArea[2 + ch.ID] = byte.Parse(ch.intBelongs.ToString());
                }

                CsConst.mySends.AddBufToSndList(arayArea, 0x0006, bytSubID, bytDevID, false, false, true, false);
                foreach (Area area in Areas)
                {
                    //区域备注
                    arayRemark = new byte[21]; // 初始化数组
                    string strRemark = area.Remark;
                    byte[] arayTmp = HDLUDP.StringToByte(strRemark);

                    arayRemark[0] = area.ID;
                    arayTmp.CopyTo(arayRemark, 1);
                    if (CsConst.mySends.AddBufToSndList(arayRemark, 0xF00C, bytSubID, bytDevID, false, true, true, false) == true)
                    {
                        
                        HDLUDP.TimeBetwnNext(20);
                    }else return;
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(20 + ((area.ID - 1) * 10) / Areas.Count);
                }
            }
            #endregion
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(30);
            //上传场景信息
            #region
            if (intActivePage == 0 || intActivePage == 4)
            {
                if (Areas == null) return;
                byte[] bytSceFlag = new byte[Areas.Count];
                byte[] bytSceIDs = new byte[Areas.Count];
                for (int i = 0; i < Areas.Count; i++)
                {
                    if (Areas[i].bytDefaultSce != 255)
                    {
                        bytSceFlag[i] = 1;
                        bytSceIDs[i] = Areas[i].bytDefaultSce;
                    }
                }
                if (CsConst.mySends.AddBufToSndList(bytSceFlag, 0xF053, bytSubID, bytDevID, false, true, true, false) == true)
                {
                    
                    HDLUDP.TimeBetwnNext(20);
                }else return;

                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(31);
                if (CsConst.mySends.AddBufToSndList(bytSceIDs, 0xF057, bytSubID, bytDevID, false, true, true, false) == true)
                {
                    
                    HDLUDP.TimeBetwnNext(20);
                }else return;
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(32);
                foreach (Area area in Areas)
                {
                    #region
                    if (area.Scen == null) return;
                    foreach (Scene scen in area.Scen)
                    {
                        string strRemark = scen.Remark;
                        byte[] arayTmp = HDLUDP.StringToByte(strRemark);

                        byte[] arayRemark1 = new byte[22];
                        arayRemark1[0] = area.ID;
                        arayRemark1[1] = scen.ID;
                        if (arayTmp != null) arayTmp.CopyTo(arayRemark1, 2);
                        if (CsConst.mySends.AddBufToSndList(arayRemark1, 0xF026, bytSubID, bytDevID, false, true, true, false) == true)
                        {
                            
                            HDLUDP.TimeBetwnNext(20); 
                        }else return;

                        // modify scene running time and lights
                        byte[] araySce = new byte[4 + wdMaxValue];
                        araySce[0] = area.ID;
                        araySce[1] = scen.ID;
                        araySce[2] = byte.Parse((scen.Time / 256).ToString());
                        araySce[3] = byte.Parse((scen.Time % 256).ToString());

                        int intTmp = 0;
                        foreach (Channel ch in ChnList)
                        {   //添加所在区域的亮度值
                            if (ch.intBelongs == area.ID)
                            {
                                araySce[4 + intTmp] = scen.light[ch.ID - 1];
                                intTmp++;
                            }
                        }
                        if (CsConst.mySends.AddBufToSndList(araySce, 0x0008, bytSubID, bytDevID, false, true, true, false) == true)
                        {
                            
                            HDLUDP.TimeBetwnNext(20);
                        }
                        else return;
                    }
                    #endregion
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(32 + ((area.ID - 1) * 8) / Areas.Count);
                }
            }
            #endregion
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(40);
            //上传窗帘信息
            #region
            if (intActivePage == 0 || intActivePage == 5)
            {
                //窗帘回路使能
                if (Curtains != null)
                {
                    byte[] ArayTmp = new byte[4];
                    if (Curtains.Count > 4) ArayTmp = new byte[8];
                    if (Curtains.Count > 0)
                    {
                        for (int i = 0; i < Curtains.Count; i++)
                        {
                            if (Curtains[i].Enable) ArayTmp[i] = 1;
                        }
                    }
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1F20, bytSubID, bytDevID, false, true, true, false) == true)
                    {
                        
                        HDLUDP.TimeBetwnNext(20);
                    }else return;
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(41);
                    //窗帘延时
                    for (int i = 0; i < Curtains.Count; i++)
                    {
                        if (Curtains[i].Enable)
                        {
                            //打开延时
                            ArayTmp = new byte[3];
                            ArayTmp[0] = Convert.ToByte(i + 1);
                            ArayTmp[1] = Convert.ToByte(Curtains[i].StartDelay / 256);
                            ArayTmp[2] = Convert.ToByte(Curtains[i].StartDelay % 256);
                            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xE807, bytSubID, bytDevID, false, true, true, false) == true)
                            {
                                
                                HDLUDP.TimeBetwnNext(20);
                            }else return;
                            //关闭延时
                            ArayTmp[1] = Convert.ToByte(Curtains[i].CloseDelay / 256);
                            ArayTmp[2] = Convert.ToByte(Curtains[i].CloseDelay % 256);
                            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xE80B, bytSubID, bytDevID, false, true, true, false) == true)
                            {
                                
                                HDLUDP.TimeBetwnNext(20);
                            }else return;
                            //运行时间
                            ArayTmp[1] = Convert.ToByte(Curtains[i].Runtime / 256);
                            ArayTmp[2] = Convert.ToByte(Curtains[i].Runtime % 256);
                            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xE802, bytSubID, bytDevID, false, true, true, false) == true)
                            {
                                
                                HDLUDP.TimeBetwnNext(20);
                            }else return;
                            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(41 + i / 2);
                        }
                    }
                }
            }
            #endregion
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(45);
            //上传干接点信息
            if (intActivePage == 0 || intActivePage == 6)
            {
                byte[] arayKeyEanble = new byte[MSKeys.Count];
                byte[] arayKeyType = new byte[MSKeys.Count];
                byte[] arayKeyMode = new byte[MSKeys.Count];
                byte[] arayKeySave = new byte[MSKeys.Count];
                byte[] arayKeyReflag = new byte[MSKeys.Count];
                #region
                for (int intI = 0; intI < wdMaxValue; intI++)
                {
                    arayKeyEanble[intI] = MSKeys[intI].bytEnable;
                    arayKeyMode[intI] = (byte)(MSKeys[intI].Mode);
                    arayKeyType[intI] = MSKeys[intI].IsDimmer;  //调光方向
                    arayKeySave[intI] = MSKeys[intI].SaveDimmer;
                    arayKeyReflag[intI] = MSKeys[intI].bytReflag;
                }

                if (CsConst.mySends.AddBufToSndList(arayKeyEanble, 0x012A, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(deviceType)) == true)
                {
                    CsConst.myRevBuf = new byte[1200];
                    HDLUDP.TimeBetwnNext(20);
                }
                else return;
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(11);

                if (CsConst.mySends.AddBufToSndList(arayKeyMode, 0xD207, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(deviceType)) == true) // 按键类型机械电子
                {
                    CsConst.myRevBuf = new byte[1200];
                    HDLUDP.TimeBetwnNext(20);
                }
                else return;
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(12);

                if (SaveButtonDimDirectionsToDeviceFrmBuf(bytSubID, bytDevID, deviceType) == false) return;

                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(13);

                if (SaveButtonDimFlagToDeviceFrmBuf(bytSubID, bytDevID, deviceType) == true)  //调光保存使能位
                {
                    CsConst.myRevBuf = new byte[1200];
                    HDLUDP.TimeBetwnNext(20);
                }
                else return;
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(14);

                if (CsConst.mySends.AddBufToSndList(arayKeyReflag, 0x015A, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(deviceType)) == true)
                {
                    CsConst.myRevBuf = new byte[1200];
                    HDLUDP.TimeBetwnNext(20);
                }
                else return;
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(15);

                // write low Limit 
                byte[] ArayTmp = new byte[1];
                ArayTmp[0] = dimmerLow;
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xE0E2, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(deviceType)) == true)
                {
                    CsConst.myRevBuf = new byte[1200];
                    HDLUDP.TimeBetwnNext(20);
                }
                else return;
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(16);
                #endregion

                #region
                byte bytCount = 0;
                foreach (MS04Key tmp in MSKeys)
                {
                    byte[] arayKeyRemark = new byte[22];
                    arayKeyRemark[0] = Convert.ToByte(tmp.ID - 1);
                    arayKeyRemark[1] = 0;
                    byte[] arayTmp = new byte[20];
                    if (tmp.Remark != null)
                    {
                        arayTmp = HDLUDP.StringToByte(tmp.Remark);
                    }

                    HDLSysPF.CopyRemarkBufferToSendBuffer(arayTmp, arayKeyRemark, 2);
                    if (CsConst.mySends.AddBufToSndList(arayKeyRemark, 0xD220, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(deviceType)) == true)
                    {
                        CsConst.myRevBuf = new byte[1200];
                        HDLUDP.TimeBetwnNext(20);
                    }
                    else return;

                    if (tmp.Mode == 0) //0 表示机械开关
                    {
                        #region
                        for (byte bytI = 0; bytI < 2; bytI++) // 两个状态依次保存
                        {
                            if (bytI != 1)
                            {
                                arayKeyRemark = new byte[3];                     //延时
                                arayKeyRemark[0] = Convert.ToByte(tmp.ID - 1);
                                arayKeyRemark[1] = byte.Parse((tmp.ONOFFDelay[0] / 256).ToString());
                                arayKeyRemark[2] = byte.Parse((tmp.ONOFFDelay[0] % 256).ToString());
                                if (CsConst.mySends.AddBufToSndList(arayKeyRemark, 0xD20C, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(deviceType)) == true)
                                {
                                    CsConst.myRevBuf = new byte[1200];
                                    HDLUDP.TimeBetwnNext(20);
                                }
                                else return;
                            }
                        }

                        if (CsConst.isRestore)
                        {
                            if (tmp.KeyTargets != null)
                            {
                                ModifyDryCommandsGroupFrmDeviceToBuf(bytSubID, bytDevID, deviceType, tmp.ID, 0);
                            }
                        }
                    #endregion
                    }
                    else
                    {
                        #region
                        if (CsConst.isRestore)
                        {
                            if (tmp.KeyTargets != null)
                            {
                                if (tmp.KeyTargets != null)
                                {
                                    ModifyDryCommandsGroupFrmDeviceToBuf(bytSubID, bytDevID, deviceType, tmp.ID, 1);
                                }
                            }
                        }
                        #endregion
                    }
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(20 + bytCount * 10 / MSKeys.Count);
                    bytCount++;
                }
                #endregion
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(55);
            //上传安防设置
            if (intActivePage == 0 || intActivePage == 7)
            {
                //安防功能
                #region
                if (myKeySeu != null && myKeySeu.Count != 0)
                {
                    foreach (UVCMD.SecurityInfo oTmp in myKeySeu)
                    {
                        byte[] ArayTmp = new byte[5];
                        //修改设置
                        ArayTmp[0] = oTmp.bytSeuID;
                        ArayTmp[1] = oTmp.bytTerms;
                        ArayTmp[2] = oTmp.bytSubID;
                        ArayTmp[3] = oTmp.bytDevID;
                        ArayTmp[4] = oTmp.bytArea;

                        if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x15DC, bytSubID, bytDevID, false, true, true, false) == true)
                        {
                            
                            HDLUDP.TimeBetwnNext(20);
                        }
                        else return;
                        if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(55 + oTmp.bytSeuID * 10 / myKeySeu.Count);
                    }
                }
                #endregion
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(65);
            //上传继电器新功能
            if (intActivePage == 0 || intActivePage == 8)
            {
                #region
                //使能
                if (CsConst.mySends.AddBufToSndList(AutoCloseEnable, 0xE0E6, bytSubID, bytDevID, false, true, true, false) == true)
                {
                    
                    HDLUDP.TimeBetwnNext(20);
                }else return;
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(66);
                //延时
                if (CsConst.mySends.AddBufToSndList(AutoCloseDelay, 0xE0EA, bytSubID, bytDevID, false, true, true, false) == true)
                {
                    
                    HDLUDP.TimeBetwnNext(20);
                }else return;
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(67);
                //互斥
                if (CsConst.mySends.AddBufToSndList(bytAryExclusion, 0x1702, bytSubID, bytDevID, false, true, true, false) == true)
                {
                    
                    HDLUDP.TimeBetwnNext(20);
                }else return;
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(68);
                #endregion
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(70);
            //HVAC
            if (intActivePage == 0 || intActivePage == 9)
            {
                #region
                /*
                if (deviceType == 3502)
                {
                    if (CsConst.isRestore)
                    {
                        if (myHVAC != null)
                        {
                            if (CsConst.mySends.AddBufToSndList(myHVAC.arayTest, 0x1C94, bytSubID, bytDevID, false, true, true) == false) return;
                            HDLUDP.TimeBetwnNext(myHVAC.arayTest.Length);
                            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(89);

                            if (CsConst.mySends.AddBufToSndList(myHVAC.arayTime, 0xE3F4, bytSubID, bytDevID, false, true, true) == false) return;
                            HDLUDP.TimeBetwnNext(myHVAC.arayTime.Length);
                            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(90);

                            if (CsConst.mySends.AddBufToSndList(myHVAC.arayHost, 0x1C44, bytSubID, bytDevID, false, true, true) == false) return;
                            HDLUDP.TimeBetwnNext(myHVAC.arayHost.Length);
                            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(91);

                            byte[] arayTmp = new byte[31];
                            Array.Copy(myHVAC.arayProtect, 0, arayTmp, 0, 16);
                            Array.Copy(myHVAC.arayProtect, 23, arayTmp, 21, 10);
                            if (CsConst.mySends.AddBufToSndList(arayTmp, 0x1C4C, bytSubID, bytDevID, false, true, true) == false) return;
                            HDLUDP.TimeBetwnNext(arayTmp.Length);
                            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(92);

                            arayTmp = new byte[1];
                            arayTmp[0] = myHVAC.Interval;
                            if (CsConst.mySends.AddBufToSndList(arayTmp, 0x1C90, bytSubID, bytDevID, false, true, true) == false) return;
                            HDLUDP.TimeBetwnNext(arayTmp.Length);
                            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(93);
                        }
                    }
                }*/
                #endregion
            }

            if (intActivePage == 0 || intActivePage == 10)
            {
                #region
                if (deviceType == 3503 || deviceType == 3504)
                {
                    if (myRS2BUS != null)
                    {
                        for (int i = 0; i < myRS2BUS.Count; i++)
                        {
                            byte[] ArayTmp = new byte[37];
                            ArayTmp[0] = Convert.ToByte(myRS2BUS[i].ID);
                            ArayTmp[1] = Convert.ToByte(myRS2BUS[i].rs232Param.enable);
                            ArayTmp[2] = Convert.ToByte(myRS2BUS[i].rs232Param.type);
                            //Array.Copy(myRS2BUS[i].TmpRS232.RSCMD, 0, ArayTmp, 3, myRS2BUS[i].TmpRS232.RSCMD.Length);
                            if (ArayTmp[1] != 0xFF && ArayTmp[36] != 0xFF)
                            {
                                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xE412, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(deviceType)) == true)
                                {
                                    
                                    HDLUDP.TimeBetwnNext(20);
                                }
                                else return;
                            }

                            ArayTmp = new byte[21];
                            ArayTmp[0] = Convert.ToByte(myRS2BUS[i].ID);
                            arayRemark = HDLUDP.StringToByte(myRS2BUS[i].remark);
                            if (arayRemark.Length <= 20)
                                arayRemark.CopyTo(ArayTmp, 1);
                            else
                                Array.Copy(arayRemark, 0, ArayTmp, 1, 20);
                            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xE41A, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(deviceType)) == true)
                            {
                                
                                 HDLUDP.TimeBetwnNext(20);
                            }
                            else return;
                            Modify232ToBusCommandsGroupFrmDeviceToBuf(bytSubID, bytDevID, deviceType, (Byte)i, myRS2BUS[i].busTargets);
                            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(70 + 10 * i / myRS2BUS.Count);
                        }
                    }
                }
                #endregion
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(80);
            if (intActivePage == 0 || intActivePage == 11)
            {
                #region
                if (deviceType == 3503 || deviceType == 3504)
                {
                    if (myBUS2RS != null)
                    {
                        for (int i = 0; i < myBUS2RS.Count; i++)
                        {
                            byte[] arayTmp = new byte[4];
                            arayTmp[0] = Convert.ToByte(myBUS2RS[i].ID);
                            arayTmp[1] = myBUS2RS[i].bytType;
                            arayTmp[2] = myBUS2RS[i].bytParam1;
                            arayTmp[3] = myBUS2RS[i].bytParam2;
                            if (CsConst.mySends.AddBufToSndList(arayTmp, 0xE422, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(deviceType)) == true)
                            {
                                
                                 HDLUDP.TimeBetwnNext(20);
                            }
                            else return;

                            arayTmp = new byte[21];
                            arayTmp[0] = Convert.ToByte(myBUS2RS[i].ID);
                            arayRemark = HDLUDP.StringToByte(myBUS2RS[i].Remark);
                            if (arayRemark.Length <= 20)
                                arayRemark.CopyTo(arayTmp, 1);
                            else
                                Array.Copy(arayRemark, 0, arayTmp, 1, 20);
                            if (CsConst.mySends.AddBufToSndList(arayTmp, 0xE42A, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(deviceType)) == true)
                            {
                                
                                 HDLUDP.TimeBetwnNext(20);
                            }
                            else return;
                            if (CsConst.isRestore)
                            {
                                if (myBUS2RS[i].RS232CMD != null)
                                {
                                    for (int j = 0; j < myBUS2RS[i].RS232CMD.Count; j++)
                                    {
                                        byte[] ArayTmp = new byte[33 + 5];
                                        ArayTmp[0] = Convert.ToByte(myBUS2RS[i].ID);
                                        ArayTmp[1] = Convert.ToByte(myBUS2RS[i].RS232CMD[j].Index);
                                        ArayTmp[2] = Convert.ToByte(myBUS2RS[i].RS232CMD[j].enable);
                                        ArayTmp[3] = Convert.ToByte(myBUS2RS[i].RS232CMD[j].type);
                                        //Array.Copy(myBUS2RS[i].RS232CMD[j].rsCmd, 0, ArayTmp, 4, 34);
                                        if (ArayTmp[2] != 0xFF && ArayTmp[37] != 0xFF)
                                        {
                                            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xE426, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(deviceType)) == true)
                                            {
                                                
                                                System.Threading.Thread.Sleep(20);
                                            }
                                            else return;
                                        }
                                    }
                                }
                            }
                            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(80 + 10 * i / myBUS2RS.Count);
                        }
                    }
                }
                #endregion
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(90);
            if (intActivePage == 0 || intActivePage == 12)
            {
                #region
                if (deviceType == 3503 || deviceType == 3504)
                {
                    if (myFilter != null)
                    {
                        for (int i = 0; i < myFilter.Count; i++)
                        {
                            byte[] arayTmp = new byte[4];
                            arayTmp = myFilter[i];
                            if (CsConst.mySends.AddBufToSndList(arayTmp, 0x1CBA, bytSubID, bytDevID, false, true, true,false) == true)
                            {
                                
                                 HDLUDP.TimeBetwnNext(20);
                            }
                            else return;
                            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(90 + (i * 5 / myFilter.Count));
                        }
                    }
                }
                #endregion
            }
            if (intActivePage == 0 || intActivePage == 13)
            {
                #region
                if (deviceType == 3504 &&!CsConst.isRestore)
                {
                    byte[] ArayTmp = new byte[7];
                    for (int i = 0; i < 7; i++)
                    {
                        ArayTmp[i] = arayTime[i];
                    }
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xDA02, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(deviceType)) == true)
                    {
                        
                        HDLUDP.TimeBetwnNext(20);
                    }
                    else return;
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(95);
                    if (intActivePage == 0 || intActivePage == 12)
                    ArayTmp = new byte[1];
                    ArayTmp[0] = arayTime[7];
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xDA42, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(deviceType)) == true)
                    {
                        
                        HDLUDP.TimeBetwnNext(20);
                    }
                    else return;
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(96);
                    ArayTmp = new byte[24];
                    for (int i = 0; i < myLogic.Count; i++)
                    {
                        ArayTmp[i] = myLogic[i].Enable;
                    }
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x161A, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(deviceType)) == true)
                    {
                        
                        HDLUDP.TimeBetwnNext(20);
                    }
                    else return;
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(97);
                    for (int i = 0; i < myLogic.Count; i++)
                    {
                        ArayTmp = new byte[21];
                        ArayTmp[0] = myLogic[i].ID;
                        Byte[] arayTmpRemark = HDLUDP.StringToByte(myLogic[i].Remark);
                        if (arayTmpRemark.Length > 20)
                        {
                            Array.Copy(arayTmpRemark, 0, ArayTmp, 1, 20);
                        }
                        else
                        {
                            Array.Copy(arayTmpRemark, 0, ArayTmp, 1, arayTmpRemark.Length);
                        }
                        if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1608, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(deviceType)) == true)
                        {
                            
                            HDLUDP.TimeBetwnNext(20);
                        }
                        else return;
                        if (myLogic[i].ID == Convert.ToByte(num1) || CsConst.isRestore)
                        {
                            ArayTmp = new byte[40];
                            ArayTmp[0] = myLogic[i].ID;
                            ArayTmp[1] = myLogic[i].Relation;
                            ArayTmp[2] = Convert.ToByte(myLogic[i].ConditionEnable / 256);
                            ArayTmp[3] = Convert.ToByte(myLogic[i].ConditionEnable % 256);
                            ArayTmp[4] = Convert.ToByte(myLogic[i].TrueTime / 256);
                            ArayTmp[5] = Convert.ToByte(myLogic[i].TrueTime % 256);
                            ArayTmp[6] = Convert.ToByte(myLogic[i].FalseTime / 256);
                            ArayTmp[7] = Convert.ToByte(myLogic[i].FalseTime % 256);
                            Array.Copy(myLogic[i].arayTime1, 0, ArayTmp, 8, myLogic[i].arayTime1.Length);
                            Array.Copy(myLogic[i].arayTime2, 0, ArayTmp, 19, myLogic[i].arayTime2.Length);
                            ArayTmp[30] = myLogic[i].DryNum1;
                            ArayTmp[31] = myLogic[i].Dry1;
                            ArayTmp[32] = myLogic[i].DryNum2;
                            ArayTmp[33] = myLogic[i].Dry2;
                            ArayTmp[34] = myLogic[i].UV1;
                            ArayTmp[35] = myLogic[i].UVCondition1;
                            ArayTmp[36] = myLogic[i].UV2;
                            ArayTmp[37] = myLogic[i].UVCondition2;
                            ArayTmp[38] = myLogic[i].LogicNO;
                            ArayTmp[39] = myLogic[i].LogicState;
                            CsConst.MyBlnNeedF8 = true;
                            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1706, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(deviceType)) == true)
                            {
                                
                                HDLUDP.TimeBetwnNext(20);
                            }
                            else return;
                        }
                    }
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(98);
                }

                #endregion
            }
            if (intActivePage == 0 || intActivePage == 14)
            {
                #region
                if (deviceType == 3504)
                {
                    if (my4852BUS != null)
                    {
                        for (int i = 0; i < my4852BUS.Count; i++)
                        {
                            byte[] ArayTmp = new byte[37];
                            ArayTmp[0] = Convert.ToByte(my4852BUS[i].ID);
                            ArayTmp[1] = Convert.ToByte(my4852BUS[i].rs232Param.enable);
                            ArayTmp[2] = Convert.ToByte(my4852BUS[i].rs232Param.type);
                           // Array.Copy(my4852BUS[i].TmpRS232.RSCMD, 0, ArayTmp, 3, my4852BUS[i].TmpRS232.RSCMD.Length);
                            if (ArayTmp[1] != 0xFF && ArayTmp[36] != 0xFF)
                            {
                                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xDA53, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(deviceType)) == true)
                                {
                                    
                                    HDLUDP.TimeBetwnNext(20);
                                }
                                else return;
                            }

                            ArayTmp = new byte[21];
                            ArayTmp[0] = Convert.ToByte(my4852BUS[i].ID);
                            arayRemark = HDLUDP.StringToByte(my4852BUS[i].remark);
                            if (arayRemark.Length <= 20)
                                arayRemark.CopyTo(ArayTmp, 1);
                            else
                                Array.Copy(arayRemark, 0, ArayTmp, 1, 20);
                            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xDA5B, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(deviceType)) == true)
                            {
                                
                                HDLUDP.TimeBetwnNext(20);
                            }
                            else return;
                            Modify485ToBusCommandsGroupFrmDeviceToBuf(bytSubID, bytDevID, deviceType, (Byte)i, my4852BUS[i].busTargets);
                            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(80 + 10 * i / myRS2BUS.Count);
                        }
                    }
                }
                #endregion
            }
            if (intActivePage == 0 || intActivePage == 15)
            {
                #region
                if (deviceType == 3504)
                {
                    if (myBUS2485 != null)
                    {
                        for (int i = 0; i < myBUS2485.Count; i++)
                        {
                            byte[] arayTmp = new byte[4];
                            arayTmp[0] = Convert.ToByte(myBUS2485[i].ID);
                            arayTmp[1] = myBUS2485[i].bytType;
                            arayTmp[2] = myBUS2485[i].bytParam1;
                            arayTmp[3] = myBUS2485[i].bytParam2;
                            if (CsConst.mySends.AddBufToSndList(arayTmp, 0xDA63, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(deviceType)) == true)
                            {
                                
                                HDLUDP.TimeBetwnNext(20);
                            }
                            else return;

                            arayTmp = new byte[21];
                            arayTmp[0] = Convert.ToByte(myBUS2485[i].ID);
                            arayRemark = HDLUDP.StringToByte(myBUS2485[i].Remark);
                            if (arayRemark.Length <= 20)
                                arayRemark.CopyTo(arayTmp, 1);
                            else
                                Array.Copy(arayRemark, 0, arayTmp, 1, 20);
                            if (CsConst.mySends.AddBufToSndList(arayTmp, 0xDA6B, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(deviceType)) == true)
                            {
                                
                                HDLUDP.TimeBetwnNext(20);
                            }
                            else return;
                            if (CsConst.isRestore)
                            {
                                if (myBUS2485[i].RS232CMD != null)
                                {
                                    for (int j = 0; j < myBUS2485[i].RS232CMD.Count; j++)
                                    {
                                        byte[] ArayTmp = new byte[33 + 5];
                                        ArayTmp[0] = Convert.ToByte(myBUS2485[i].ID);
                                        ArayTmp[1] = Convert.ToByte(myBUS2485[i].RS232CMD[j].Index);
                                        ArayTmp[2] = Convert.ToByte(myBUS2485[i].RS232CMD[j].enable);
                                        ArayTmp[3] = Convert.ToByte(myBUS2485[i].RS232CMD[j].type);
                                        //Array.Copy(myBUS2485[i].RS232CMD[j].rsCmd, 0, ArayTmp, 4, 34);
                                        if (ArayTmp[2] != 0xFF && ArayTmp[37] != 0xFF)
                                        {
                                            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xDA67, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(deviceType)) == true)
                                            {
                                                
                                                HDLUDP.TimeBetwnNext(20);
                                            }
                                            else return;
                                        }
                                    }
                                }
                            }
                            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(90 + 10 * i / myBUS2RS.Count);
                        }
                    }
                }
                #endregion
            }
            
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(100);
        }

        public void ReadDryCommandsGroupFrmDeviceToBuf(Byte bytSubID, Byte bytDevID, int DeviceType, Byte DryId, Byte DryOnOrOffStatus)
        {
            try
            {
                #region
                byte bytTotalCMD = 0;
                switch (MSKeys[DryId].Mode)
                {
                    case 0:
                    case 4:
                    case 5:
                    case 6: bytTotalCMD = 49; break;  // 99 个目标
                    default: bytTotalCMD = 1; break;
                }

                Byte[] ArayTmp = new byte[3];
                ArayTmp[0] = DryId;
                ArayTmp[1] = DryOnOrOffStatus;
                if (DryOnOrOffStatus == 0)
                    MSKeys[DryId].KeyOffTargets = new List<UVCMD.ControlTargets>();
                else 
                    MSKeys[DryId].KeyTargets = new List<UVCMD.ControlTargets>();
                for (byte bytJ = 0; bytJ < bytTotalCMD; bytJ++)
                {
                    ArayTmp[2] = bytJ;

                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xD21C, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                    {
                        UVCMD.ControlTargets oCMD = new UVCMD.ControlTargets();
                        oCMD.ID = Convert.ToByte(CsConst.myRevBuf[28] + 1);
                        oCMD.Type = CsConst.myRevBuf[29];  //转换为正确的类型
                        oCMD.SubnetID = CsConst.myRevBuf[30];
                        oCMD.DeviceID = CsConst.myRevBuf[31];
                        oCMD.Param1 = CsConst.myRevBuf[32];
                        oCMD.Param2 = CsConst.myRevBuf[33];
                        oCMD.Param3 = CsConst.myRevBuf[34];
                        oCMD.Param4 = CsConst.myRevBuf[35];
                        CsConst.myRevBuf = new byte[1200];
                        if (DryOnOrOffStatus == 0)
                        {
                            MSKeys[DryId].KeyOffTargets.Add(oCMD);
                        }
                        else
                        {
                            MSKeys[DryId].KeyTargets.Add(oCMD);
                        }
                    }
                    else return;
                }
                #endregion
            }
            catch
            { }
        }

        public void ModifyDryCommandsGroupFrmDeviceToBuf(Byte bytSubID, Byte bytDevID, int DeviceType, Byte DryId, Byte DryOnOrOffStatus)
        {
            List<UVCMD.ControlTargets> tmpCommandsGroup = new List<UVCMD.ControlTargets>();
            try
            {
                if (DryOnOrOffStatus == 0)
                {
                    tmpCommandsGroup = MSKeys[DryId].KeyOffTargets;
                }
                else
                {
                    tmpCommandsGroup = MSKeys[DryId].KeyTargets;
                }

                if (tmpCommandsGroup != null)
                {
                    foreach (UVCMD.ControlTargets TmpCmd in tmpCommandsGroup)
                    {
                        byte[] arayCMD = new byte[10];
                        arayCMD[0] = Convert.ToByte(DryId);
                        arayCMD[1] = DryOnOrOffStatus;
                        arayCMD[2] = Convert.ToByte(TmpCmd.ID - 1);
                        arayCMD[3] = TmpCmd.Type;
                        arayCMD[4] = TmpCmd.SubnetID;
                        arayCMD[5] = TmpCmd.DeviceID;
                        arayCMD[6] = TmpCmd.Param1;
                        arayCMD[7] = TmpCmd.Param2;
                        arayCMD[8] = TmpCmd.Param3;   // save targets
                        arayCMD[9] = TmpCmd.Param4;
                        if (CsConst.mySends.AddBufToSndList(arayCMD, 0xD21E, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                        {
                            CsConst.myRevBuf = new byte[1200];
                            HDLUDP.TimeBetwnNext(20);
                        }
                        else return;
                    }
                }
            }
            catch
            { }
        }

        public static List<UVCMD.ControlTargets> Read232ToBusCommandsGroupFrmDeviceToBuf(Byte bytSubID, Byte bytDevID, int DeviceType, Byte cmdId)
        {
            List<UVCMD.ControlTargets> tmpResult = new List<UVCMD.ControlTargets>();
            try
            {
                #region
                for (byte j = 1; j <= 10; j++)
                {
                    byte[] arayTmp = new byte[2];
                    arayTmp[0] = Convert.ToByte(cmdId + 1);
                    arayTmp[1] = j;
                    if (CsConst.mySends.AddBufToSndList(arayTmp, 0xE414, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                    {
                        UVCMD.ControlTargets oCMD = new UVCMD.ControlTargets();
                        oCMD.ID = j;
                        oCMD.Type = CsConst.myRevBuf[28];  //转换为正确的类型
                        oCMD.SubnetID = CsConst.myRevBuf[29];
                        oCMD.DeviceID = CsConst.myRevBuf[30];
                        oCMD.Param1 = CsConst.myRevBuf[31];
                        oCMD.Param2 = CsConst.myRevBuf[32];
                        oCMD.Param3 = CsConst.myRevBuf[33];
                        oCMD.Param4 = CsConst.myRevBuf[34];
                        tmpResult.Add(oCMD);

                        System.Threading.Thread.Sleep(1);
                    }
                    else
                    {
                        break;
                    }
                }
                #endregion
            }
            catch
            {
                return tmpResult;
            }
            return tmpResult;
        }

        public static void Modify232ToBusCommandsGroupFrmDeviceToBuf(Byte bytSubID, Byte bytDevID, int DeviceType, Byte cmdId, List<UVCMD.ControlTargets> tempRs232ToBus)
        {
            try
            {
                #region
                if (tempRs232ToBus != null)
                {
                    foreach (UVCMD.ControlTargets TmpCmd in tempRs232ToBus)
                    {
                        if (TmpCmd.Type != 0 && TmpCmd.Type != 255)
                        {
                            byte[] arayCMD = new byte[9];
                            arayCMD[0] = Convert.ToByte(cmdId + 1);
                            arayCMD[1] = Convert.ToByte(TmpCmd.ID);
                            arayCMD[2] = TmpCmd.Type;
                            arayCMD[3] = TmpCmd.SubnetID;
                            arayCMD[4] = TmpCmd.DeviceID;
                            arayCMD[5] = TmpCmd.Param1;
                            arayCMD[6] = TmpCmd.Param2;
                            arayCMD[7] = TmpCmd.Param3;   // save targets
                            arayCMD[8] = TmpCmd.Param4;
                            if (CsConst.mySends.AddBufToSndList(arayCMD, 0xE416, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                            {

                                HDLUDP.TimeBetwnNext(20);
                            }
                            else return;
                        }
                    }
                }
                #endregion
            }
            catch
            {
                return;
            }
            return;
        }

        public static List<UVCMD.ControlTargets> Read485ToBusCommandsGroupFrmDeviceToBuf(Byte bytSubID, Byte bytDevID, int DeviceType, Byte cmdId)
        {
            List<UVCMD.ControlTargets> tmpResult = new List<UVCMD.ControlTargets>();
            try
            {
                #region
                int num = 10;

                for (byte j = 1; j <= num; j++)
                {
                    byte[] arayTmp = new byte[2];
                    arayTmp[0] = Convert.ToByte(cmdId + 1);
                    arayTmp[1] = j;
                    if (CsConst.mySends.AddBufToSndList(arayTmp, 0xDA55, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                    {
                        UVCMD.ControlTargets oCMD = new UVCMD.ControlTargets();
                        oCMD.ID = j;
                        oCMD.Type = CsConst.myRevBuf[28];  //转换为正确的类型
                        oCMD.SubnetID = CsConst.myRevBuf[29];
                        oCMD.DeviceID = CsConst.myRevBuf[30];
                        oCMD.Param1 = CsConst.myRevBuf[31];
                        oCMD.Param2 = CsConst.myRevBuf[32];
                        oCMD.Param3 = CsConst.myRevBuf[33];
                        oCMD.Param4 = CsConst.myRevBuf[34];
                        tmpResult.Add(oCMD);

                        System.Threading.Thread.Sleep(1);
                    }
                    else
                    {
                        break;
                    }
                }
                #endregion
            }
            catch
            {
                return tmpResult;
            }
            return tmpResult;
        }

        public static void Modify485ToBusCommandsGroupFrmDeviceToBuf(Byte bytSubID, Byte bytDevID, int DeviceType, Byte cmdId, List<UVCMD.ControlTargets> tempRs232ToBus)
        {
            try
            {
                #region
                if (tempRs232ToBus != null)
                {
                    foreach (UVCMD.ControlTargets TmpCmd in tempRs232ToBus)
                    {
                        if (TmpCmd.Type != 0 && TmpCmd.Type != 255)
                        {
                            byte[] arayCMD = new byte[9];
                            arayCMD[0] = Convert.ToByte(cmdId + 1);
                            arayCMD[1] = Convert.ToByte(TmpCmd.ID);
                            arayCMD[2] = TmpCmd.Type;
                            arayCMD[3] = TmpCmd.SubnetID;
                            arayCMD[4] = TmpCmd.DeviceID;
                            arayCMD[5] = TmpCmd.Param1;
                            arayCMD[6] = TmpCmd.Param2;
                            arayCMD[7] = TmpCmd.Param3;   // save targets
                            arayCMD[8] = TmpCmd.Param4;
                            if (CsConst.mySends.AddBufToSndList(arayCMD, 0xDA57, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                            {

                                HDLUDP.TimeBetwnNext(20);
                            }
                            else return;
                        }
                    }
                }
                #endregion
            }
            catch
            {
                return;
            }
            return;
        }

        public Byte[] ReadButtonDimFlagFrmDeviceToBuf(Byte bytSubID, Byte bytDevID)
        {
            Byte[] arayKeyDim = new Byte[MSKeys.Count];

            if (CsConst.mySends.AddBufToSndList(null, 0xE134, bytSubID, bytDevID, false, true, true, true) == true)
            {
                for (int intJ = 0; intJ < MSKeys.Count; intJ++)
                {
                    arayKeyDim[intJ] = CsConst.myRevBuf[25 + intJ];
                }
                CsConst.myRevBuf = new byte[1200];
            }
            return arayKeyDim;
        }

        public Byte[] ReadButtonDimDirectionFrmDeviceToBuf(Byte bytSubID, Byte bytDevID)
        {
            Byte[] arayKeyDim = new Byte[MSKeys.Count];

            if (CsConst.mySends.AddBufToSndList(null, 0xD230, bytSubID, bytDevID, false, true, true, true) == true)
            {
                for (int intJ = 0; intJ < MSKeys.Count; intJ++)
                {
                    arayKeyDim[intJ] = CsConst.myRevBuf[26 + intJ];
                }
                CsConst.myRevBuf = new byte[1200];
            }
            return arayKeyDim;
        }

        public Boolean SaveButtonDimFlagToDeviceFrmBuf(Byte bytSubID, Byte bytDevID, int DeviceType)
        {
            Boolean blnSuccessModify = false;
            Byte[] arayKeyMode = new Byte[MSKeys.Count];
            for (Byte bytI = 0; bytI < MSKeys.Count; bytI++)
            {
                // key  dimmer flag
                HDLButton oTmpButton = MSKeys[bytI];
                arayKeyMode[bytI] = oTmpButton.SaveDimmer;
            }

            // upload dimmer flag
            blnSuccessModify = CsConst.mySends.AddBufToSndList(arayKeyMode, 0xE136, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType));
            return blnSuccessModify;
        }

        public Boolean SaveButtonDimDirectionsToDeviceFrmBuf(Byte bytSubID, Byte bytDevID, int DeviceType)
        {
            Boolean blnSuccessModify = false;
            Byte[] arayKeyMode = new Byte[MSKeys.Count];
            for (Byte bytI = 0; bytI < MSKeys.Count; bytI++)
            {
                // key  dimmer flag
                HDLButton oTmpButton = MSKeys[bytI];
                arayKeyMode[bytI] = oTmpButton.IsDimmer;
            }
            blnSuccessModify = CsConst.mySends.AddBufToSndList(arayKeyMode, 0xD232, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType));//要不要调光
            return blnSuccessModify;
        }


    }
}
