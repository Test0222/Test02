using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.Windows.Forms;

namespace HDL_Buspro_Setup_Tool
{
     [Serializable]
    public class Sensor_12in1 : HdlDeviceBackupAndRestore
    {
        //基本信息
        public int DIndex;//设备唯一编号
        public string Devname;//子网ID 设备ID 备注
        public byte[] ONLEDs = new byte[5]; // 红外LED灯  红外超声波
        public byte[] EnableSensors = new byte[15]; // 留有预留 {温度传感器 亮度传感器  红外传感器  超声波传感器  干接点一  干接点二   通用开关一 通用开关二  以逻辑块为条件} 
                                                    // 有六个预留
        public byte[] EnableBroads = new byte[15];  //恒照度使能,恒照度亮度两个byte,kp两个byte,ki两个byte,周期,低限
        public byte[] ParamSensors = new byte[15];  //温度补偿值，亮度补偿值，红外灵敏度，超声波灵敏度
        public byte[] SimulateEnable = new byte[15];//模拟测试使能 温度传感器模拟使能 亮度传感器模拟使能 红外传感器模拟使能 超声波传感器模拟使能
        public byte[] ParamSimulate = new byte[15];//温度模拟值 亮度模拟值，红外模拟值，超声波模拟值
        public byte[] ArayTmpMode = new byte[40];//红外接收模式
        public List<UVCMD.IRCode> IRCodes;  //红外发射
        public List<IRReceive> IrReceiver;   // 接收红外控制目标
        public List<Logic> logic;
        public List<UVCMD.SecurityInfo> fireset;
        public RelaySetup[] MyRelays = new RelaySetup[2]; //新增两路继电器设置

        public bool[] MyRead2UpFlags = new bool[10];
        //继电器设置
          [Serializable]
        public class RelaySetup
        {
            public string RelayName;
            public byte[] Setup = new byte[20];
        }
         [Serializable]
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
          [Serializable]
        public class Logic
        {
            public Byte ID;
            public String Remarklogic;//逻辑备注
            public Byte Enabled;
            public Byte bPowerOnFlag; // 上电标识
            public Byte bytRelation; // 关系
            public Byte[] EnableSensors = new byte[15]; // 温度传感器 亮度传感器  红外传感器  超声波传感器  干接点一  干接点二   通用开关一 通用开关二  以逻辑块为条件}   // 有六个预留

            public Byte[] Paramters = new byte[20];//最低温度，最高温度，最低亮度(2)，最高亮度(2)，红外传感器，超声波传感器，干接点一 干接点二

            public UVSet UV1;//通用开关1
            public UVSet UV2;//通用开关2
            public List<UVCMD.ControlTargets> SetUp;//成立目标
            public List<UVCMD.ControlTargets> NoSetUp;//不成立目标
            public int DelayTimeT;//成立延迟时间
            public int DelayTimeF;//不成立延迟时间
            
        }

        //IR Receive
          [Serializable]
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

            ParamSensors = new byte[15];  // 对应传感器参数设置
            if (IRCodes == null) IRCodes = new List<UVCMD.IRCode>();
            if (IrReceiver == null) IrReceiver = new List<IRReceive>();
            if (logic == null) logic = new List<Logic>();
            if (fireset == null) fireset = new List<UVCMD.SecurityInfo>();
            MyRelays = new RelaySetup[2];
            MyRelays[0] = new RelaySetup();
            MyRelays[1] = new RelaySetup();
        }


        ///<summary>
        ///读取数据库面板设置，将所有数据读至缓存
        ///</summary>
        public void ReadSensorInfoFromDB(int DIndex)
        {
            #region
            try
            {
                //红外接收
                #region
                string strsql = string.Format("select * from dbClassInfomation where DIndex={0} and ClassID={1} order by SenNum", DIndex, 0);
                OleDbDataReader dr = DataModule.SearchAResultSQLDB(strsql, CsConst.mstrCurPath);
                IrReceiver = new List<IRReceive>();
                if (dr != null)
                {
                    while (dr.Read())
                    {
                        IRReceive temp = new IRReceive();
                        string str = dr.GetValue(5).ToString();
                        temp.BtnNum = Convert.ToByte(str.Split('-')[0].ToString());
                        temp.IRBtnModel = Convert.ToByte(str.Split('-')[1].ToString());
                        temp.IRBtnRemark = dr.GetValue(4).ToString();
                        str = string.Format("select * from dbKeyTargets where DIndex ={0} and KeyIndex = {1} and Ms04State = {2}", DIndex, temp.BtnNum, 0);
                        OleDbDataReader drKeyCmds = DataModule.SearchAResultSQLDB(str, CsConst.mstrCurPath);
                        temp.TargetInfo = new List<UVCMD.ControlTargets>();
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
                            temp.TargetInfo.Add(TmpCmd);
                        }
                        drKeyCmds.Close();
                        IrReceiver.Add(temp);
                    }
                    dr.Close();
                }
                #endregion

                //逻辑
                #region
                strsql = string.Format("select * from dbClassInfomation where DIndex={0} and ClassID={1} order by SenNum", DIndex, 1);
                dr = DataModule.SearchAResultSQLDB(strsql, CsConst.mstrCurPath);
                logic = new List<Logic>();
                if (dr != null)
                {
                    while (dr.Read())
                    {
                        Logic temp = new Logic();
                        temp.UV1 = new UVSet();
                        temp.UV2 = new UVSet();
                        string str = dr.GetValue(5).ToString();
                        temp.ID = Convert.ToByte(str.Split('-')[0].ToString());
                        temp.Enabled = Convert.ToByte(str.Split('-')[1].ToString());
                        temp.bytRelation = Convert.ToByte(str.Split('-')[2].ToString());
                        temp.UV1.UvNum = Convert.ToByte(str.Split('-')[3].ToString());
                        temp.UV1.UVCondition = Convert.ToByte(str.Split('-')[4].ToString());
                        temp.UV1.OffTime = Convert.ToByte(str.Split('-')[5].ToString());
                        temp.UV1.AutoOff = Convert.ToBoolean(str.Split('-')[6].ToString());
                        temp.UV2.UvNum = Convert.ToByte(str.Split('-')[7].ToString());
                        temp.UV2.UVCondition = Convert.ToByte(str.Split('-')[8].ToString());
                        temp.UV2.OffTime = Convert.ToByte(str.Split('-')[9].ToString());
                        temp.UV2.AutoOff = Convert.ToBoolean(str.Split('-')[10].ToString());
                        temp.DelayTimeT = Convert.ToByte(str.Split('-')[11].ToString());
                        temp.DelayTimeF = Convert.ToByte(str.Split('-')[12].ToString());
                        temp.Remarklogic = dr.GetValue(4).ToString();
                        EnableSensors = (byte[])dr[6];
                        temp.Paramters = (byte[])dr[7];
                        temp.UV1.UvRemark = dr.GetValue(16).ToString();
                        temp.UV2.UvRemark = dr.GetValue(17).ToString();
                        temp.SetUp = new List<UVCMD.ControlTargets>();
                        temp.NoSetUp = new List<UVCMD.ControlTargets>();

                        str = string.Format("select * from dbKeyTargets where DIndex ={0} and KeyIndex = {1} and Ms04State = {2}", DIndex, temp.ID, 1);
                        OleDbDataReader drKeyCmds1 = DataModule.SearchAResultSQLDB(str, CsConst.mstrCurPath);
                        while (drKeyCmds1.Read())
                        {
                            UVCMD.ControlTargets TmpCmd = new UVCMD.ControlTargets();
                            TmpCmd.ID = drKeyCmds1.GetByte(2);
                            TmpCmd.Type = drKeyCmds1.GetByte(3);
                            TmpCmd.SubnetID = drKeyCmds1.GetByte(4);
                            TmpCmd.DeviceID = drKeyCmds1.GetByte(5);
                            TmpCmd.Param1 = drKeyCmds1.GetByte(6);
                            TmpCmd.Param2 = drKeyCmds1.GetByte(7);
                            TmpCmd.Param3 = drKeyCmds1.GetByte(8);
                            TmpCmd.Param4 = drKeyCmds1.GetByte(9);
                            temp.SetUp.Add(TmpCmd);
                        }
                        drKeyCmds1.Close();

                        str = string.Format("select * from dbKeyTargets where DIndex ={0} and KeyIndex = {1} and Ms04State = {2}", DIndex, temp.ID, 2);
                        OleDbDataReader drKeyCmds2 = DataModule.SearchAResultSQLDB(str, CsConst.mstrCurPath);
                        while (drKeyCmds1.Read())
                        {
                            UVCMD.ControlTargets TmpCmd = new UVCMD.ControlTargets();
                            TmpCmd.ID = drKeyCmds2.GetByte(2);
                            TmpCmd.Type = drKeyCmds2.GetByte(3);
                            TmpCmd.SubnetID = drKeyCmds2.GetByte(4);
                            TmpCmd.DeviceID = drKeyCmds2.GetByte(5);
                            TmpCmd.Param1 = drKeyCmds2.GetByte(6);
                            TmpCmd.Param2 = drKeyCmds2.GetByte(7);
                            TmpCmd.Param3 = drKeyCmds2.GetByte(8);
                            TmpCmd.Param4 = drKeyCmds2.GetByte(9);
                            temp.NoSetUp.Add(TmpCmd);
                        }
                        drKeyCmds2.Close();
                        logic.Add(temp);
                    }
                    dr.Close();
                }
                #endregion

                //安防
                #region
                strsql = string.Format("select * from dbClassInfomation where DIndex={0} and ClassID={1} order by SenNum", DIndex, 2);
                dr = DataModule.SearchAResultSQLDB(strsql, CsConst.mstrCurPath);
                fireset = new List<UVCMD.SecurityInfo>();
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
                        temp.strRemark = dr.GetValue(4).ToString();
                        fireset.Add(temp);
                    }
                    dr.Close();
                }
                #endregion

                //继电器
                #region
                strsql = string.Format("select * from dbClassInfomation where DIndex={0} and ClassID={1} order by SenNum", DIndex, 3);
                dr = DataModule.SearchAResultSQLDB(strsql, CsConst.mstrCurPath);
                MyRelays = new RelaySetup[2];
                if (dr != null)
                {
                    byte byt = 0;
                    while (dr.Read())
                    {
                        RelaySetup temp = new RelaySetup();
                        temp.RelayName = dr.GetValue(4).ToString();
                        temp.Setup = (byte[])dr[6];
                        MyRelays[byt] = temp;
                        byt++;
                    }
                    dr.Close();
                }
                #endregion

                //基本信息
                #region
                strsql = string.Format("select * from dbClassInfomation where DIndex={0} and ClassID={1} order by SenNum", DIndex, 4);
                dr = DataModule.SearchAResultSQLDB(strsql, CsConst.mstrCurPath);
                if (dr != null)
                {
                    while (dr.Read())
                    {
                        ONLEDs = (byte[])dr[6];
                        EnableSensors = (byte[])dr[7];
                        EnableBroads = (byte[])dr[8];
                        ParamSensors = (byte[])dr[9];
                        SimulateEnable = (byte[])dr[10];
                        ParamSimulate = (byte[])dr[11];
                        ArayTmpMode = (byte[])dr[12];
                    }
                    dr.Close();
                }
                #endregion
            }
            catch
            {
            }
            #endregion
        }

        ///<summary>
        ///保存数据库面板设置，将所有数据保存
        ///</summary>
        public void SaveSensorInfoToDB()
        {
            #region
            try
            {
                string strsql = string.Format("delete * from dbClassInfomation where DIndex={0}", DIndex);
                DataModule.ExecuteSQLDatabase(strsql);

                strsql = string.Format("delete * from dbKeyTargets where DIndex={0}", DIndex);
                DataModule.ExecuteSQLDatabase(strsql);

                //红外接收
                #region
                if (IrReceiver != null)
                {
                    for (int i = 0; i < IrReceiver.Count; i++)
                    {
                        IRReceive tmpKey = IrReceiver[i];
                        string strParam = tmpKey.BtnNum.ToString() + "-" + tmpKey.IRBtnModel.ToString();
                        strsql = string.Format("Insert into dbClassInfomation(DIndex,ClassID,ObjectID,SenNum,Remark,strParam1) values ({0},{1},{2},{3},'{4}','{5}')",
                                 DIndex, 0, 0, i, tmpKey.IRBtnRemark, strParam);
                        DataModule.ExecuteSQLDatabase(strsql);
                        if (tmpKey.TargetInfo != null)
                        {
                            for (int intK = 0; intK < tmpKey.TargetInfo.Count; intK++)
                            {
                                UVCMD.ControlTargets TmpCmds = tmpKey.TargetInfo[intK];
                                ///// insert into all commands to database
                                strsql = string.Format("Insert into dbKeyTargets(DIndex,KeyIndex,objID,KeyFunType,SubNetID,DeviceID,FirstParameter,"
                                                + "SecondParameter,RunTimeMinute,RunTimeSecond,Ms04State) values ({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10})",
                                               DIndex, tmpKey.BtnNum, TmpCmds.ID, TmpCmds.Type, TmpCmds.SubnetID, TmpCmds.DeviceID, TmpCmds.Param1,
                                               TmpCmds.Param2, TmpCmds.Param3, TmpCmds.Param4, 0);
                                DataModule.ExecuteSQLDatabase(strsql);
                            }
                        }
                    }
                }
                #endregion

                //逻辑
                #region
                if (logic != null)
                {
                    for (int i = 0; i < logic.Count; i++)
                    {
                        byte[] arayTmp = new byte[1];
                        if (logic[i].EnableSensors == null) logic[i].EnableSensors = new byte[15];
                        if (logic[i].Paramters == null) logic[i].Paramters = new byte[15];
                        if (logic[i].UV1 == null) logic[i].UV1 = new UVSet();
                        if (logic[i].UV2 == null) logic[i].UV2 = new UVSet();

                        string strRemark = logic[i].Remarklogic;
                        string strParam = logic[i].ID.ToString() + "-" + logic[i].Enabled.ToString()
                                        + "-" + logic[i].bytRelation.ToString() + "-" + logic[i].UV1.UvNum.ToString()
                                        + "-" + logic[i].UV1.UVCondition.ToString() + "-" + logic[i].UV1.OffTime.ToString()
                                        + "-" + logic[i].UV1.AutoOff.ToString() + "-" + logic[i].UV2.UvNum.ToString()
                                        + "-" + logic[i].UV2.UVCondition.ToString() + "-" + logic[i].UV2.OffTime.ToString()
                                        + "-" + logic[i].UV2.AutoOff.ToString() + "-" + logic[i].DelayTimeT.ToString()
                                        + "-" + logic[i].DelayTimeF.ToString();
                        string struv1 = logic[i].UV1.UvRemark;
                        string struv2 = logic[i].UV2.UvRemark;
                        strsql = @"Insert into dbClassInfomation(DIndex,ClassID,ObjectID,SenNum,Remark,strParam1,byteAry1,byteAry2,byteAry3,byteAry4,byteAry5,@byteAry6,@byteAry7,@byteAry8,@byteAry9,@byteAry10,@strTemp1,@strTemp2)"
                          + "values(@DIndex,@ClassID,@ObjectID,@SenNum,@Remark,@strParam1,@byteAry1,@byteAry2,@byteAry3,@byteAry4,@byteAry5,@byteAry6,@byteAry7,@byteAry8,@byteAry9,@byteAry10,@strTemp1,@strTemp2)";
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
                        ((OleDbParameter)cmd.Parameters.Add("@byteAry1", OleDbType.Binary)).Value = logic[i].EnableSensors;
                        ((OleDbParameter)cmd.Parameters.Add("@byteAry2", OleDbType.Binary)).Value = logic[i].Paramters;
                        ((OleDbParameter)cmd.Parameters.Add("@byteAry3", OleDbType.Binary)).Value = arayTmp;
                        ((OleDbParameter)cmd.Parameters.Add("@byteAry4", OleDbType.Binary)).Value = arayTmp;
                        ((OleDbParameter)cmd.Parameters.Add("@byteAry5", OleDbType.Binary)).Value = arayTmp;
                        ((OleDbParameter)cmd.Parameters.Add("@byteAry6", OleDbType.Binary)).Value = arayTmp;
                        ((OleDbParameter)cmd.Parameters.Add("@byteAry7", OleDbType.Binary)).Value = arayTmp;
                        ((OleDbParameter)cmd.Parameters.Add("@byteAry8", OleDbType.Binary)).Value = arayTmp;
                        ((OleDbParameter)cmd.Parameters.Add("@byteAry9", OleDbType.Binary)).Value = arayTmp;
                        ((OleDbParameter)cmd.Parameters.Add("@byteAry10", OleDbType.Binary)).Value = arayTmp;
                        ((OleDbParameter)cmd.Parameters.Add("@strTemp1", OleDbType.VarChar)).Value = struv1;
                        ((OleDbParameter)cmd.Parameters.Add("@strTemp2", OleDbType.VarChar)).Value = struv2;
                        try
                        {
                            cmd.ExecuteNonQuery();
                        }
                        catch
                        {
                            conn.Close();
                        }
                        conn.Close();

                        if (logic[i].SetUp != null)
                        {
                            for (int intK = 0; intK < logic[i].SetUp.Count; intK++)
                            {
                                UVCMD.ControlTargets TmpCmds = logic[i].SetUp[intK];
                                strsql = string.Format("Insert into dbKeyTargets(DIndex,KeyIndex,objID,KeyFunType,SubNetID,DeviceID,FirstParameter,"
                                                + "SecondParameter,RunTimeMinute,RunTimeSecond,Ms04State) values ({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10})",
                                               DIndex, logic[i].ID, TmpCmds.ID, TmpCmds.Type, TmpCmds.SubnetID, TmpCmds.DeviceID, TmpCmds.Param1,
                                               TmpCmds.Param2, TmpCmds.Param3, TmpCmds.Param4, 1);
                                DataModule.ExecuteSQLDatabase(strsql);
                            }
                        }

                        if (logic[i].NoSetUp != null)
                        {
                            for (int intK = 0; intK < logic[i].NoSetUp.Count; intK++)
                            {
                                UVCMD.ControlTargets TmpCmds = logic[i].NoSetUp[intK];
                                strsql = string.Format("Insert into dbKeyTargets(DIndex,KeyIndex,objID,KeyFunType,SubNetID,DeviceID,FirstParameter,"
                                                + "SecondParameter,RunTimeMinute,RunTimeSecond,Ms04State) values ({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10})",
                                               DIndex, logic[i].ID, TmpCmds.ID, TmpCmds.Type, TmpCmds.SubnetID, TmpCmds.DeviceID, TmpCmds.Param1,
                                               TmpCmds.Param2, TmpCmds.Param3, TmpCmds.Param4, 1);
                                DataModule.ExecuteSQLDatabase(strsql);
                            }
                        }
                    }
                }
                #endregion

                //安防
                #region
                if (fireset != null)
                {
                    for (int i = 0; i < fireset.Count; i++)
                    {
                        UVCMD.SecurityInfo temp = fireset[i];
                        string strParam = temp.bytSeuID.ToString() + "-" + temp.bytTerms.ToString() + "-"
                                        + temp.bytSubID.ToString() + "-" + temp.bytDevID.ToString() + "-"
                                        + temp.bytArea.ToString();
                        strsql = string.Format("Insert into dbClassInfomation(DIndex,ClassID,ObjectID,SenNum,Remark,strParam1) values ({0},{1},{2},{3},'{4}','{5}')",
                                 DIndex, 2, 0, i, temp.strRemark, strParam);
                        DataModule.ExecuteSQLDatabase(strsql);
                    }
                }
                #endregion

                //继电器
                #region
                if (MyRelays != null)
                {
                    for (int i = 0; i < MyRelays.Length; i++)
                    {
                        RelaySetup temp = MyRelays[i];
                        
                        strsql = @"Insert into dbClassInfomation(DIndex,ClassID,ObjectID,SenNum,Remark,strParam1,byteAry1,byteAry2,byteAry3,byteAry4,byteAry5,@byteAry6,@byteAry7,@byteAry8,@byteAry9,@byteAry10,@strTemp1,@strTemp2)"
                          + "values(@DIndex,@ClassID,@ObjectID,@SenNum,@Remark,@strParam1,@byteAry1,@byteAry2,@byteAry3,@byteAry4,@byteAry5,@byteAry6,@byteAry7,@byteAry8,@byteAry9,@byteAry10,@strTemp1,@strTemp2)";
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
                        ((OleDbParameter)cmd.Parameters.Add("@Remark", OleDbType.VarChar)).Value = temp.RelayName;
                        ((OleDbParameter)cmd.Parameters.Add("@strParam1", OleDbType.VarChar)).Value = "";
                        ((OleDbParameter)cmd.Parameters.Add("@byteAry1", OleDbType.Binary)).Value = temp.Setup;
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

                //基本信息
                #region
                if (ONLEDs == null) ONLEDs = new byte[5];
                if (EnableSensors == null) EnableSensors = new byte[15];
                if (EnableBroads == null) EnableBroads = new byte[15];
                if (ParamSensors == null) ParamSensors = new byte[15];
                if (SimulateEnable == null) SimulateEnable = new byte[15];
                if (ParamSimulate == null) ParamSimulate = new byte[15];
                if (ArayTmpMode == null) ArayTmpMode = new byte[15];
                string strRemarkBasic = "";
                string strParamBasic = "";

                strsql = @"Insert into dbClassInfomation(DIndex,ClassID,ObjectID,SenNum,Remark,strParam1,byteAry1,byteAry2,byteAry3,byteAry4,byteAry5,byteAry6,byteAry7)"
                  + " values(@DIndex,@ClassID,@ObjectID,@SenNum,@Remark,@strParam1,@byteAry1,@byteAry2,@byteAry3,@byteAry4,@byteAry5,@byteAry6,@byteAry7)";
                //创建一个OleDbConnection对象
                OleDbConnection connBasic;

                connBasic = new OleDbConnection(DataModule.ConString + CsConst.mstrDefaultPath);
                //OleDbConnection conn = new OleDbConnection(DataModule.ConString + CsConst.mstrCurPath);
                connBasic.Open();
                OleDbCommand cmdBaic = new OleDbCommand(strsql, connBasic);
                ((OleDbParameter)cmdBaic.Parameters.Add("@DIndex", OleDbType.Integer)).Value = DIndex;
                ((OleDbParameter)cmdBaic.Parameters.Add("@ClassID", OleDbType.Integer)).Value = 4;
                ((OleDbParameter)cmdBaic.Parameters.Add("@ObjectID", OleDbType.Integer)).Value = 0;
                ((OleDbParameter)cmdBaic.Parameters.Add("@SenNum", OleDbType.Integer)).Value = 0;
                ((OleDbParameter)cmdBaic.Parameters.Add("@Remark", OleDbType.VarChar)).Value = strRemarkBasic;
                ((OleDbParameter)cmdBaic.Parameters.Add("@strParam1", OleDbType.VarChar)).Value = strParamBasic;
                ((OleDbParameter)cmdBaic.Parameters.Add("@byteAry1", OleDbType.Binary)).Value = ONLEDs;
                ((OleDbParameter)cmdBaic.Parameters.Add("@byteAry2", OleDbType.Binary)).Value = EnableSensors;
                ((OleDbParameter)cmdBaic.Parameters.Add("@byteAry3", OleDbType.Binary)).Value = EnableBroads;
                ((OleDbParameter)cmdBaic.Parameters.Add("@byteAry4", OleDbType.Binary)).Value = ParamSensors;
                ((OleDbParameter)cmdBaic.Parameters.Add("@byteAry5", OleDbType.Binary)).Value = SimulateEnable;
                ((OleDbParameter)cmdBaic.Parameters.Add("@byteAry6", OleDbType.Binary)).Value = ParamSimulate;
                ((OleDbParameter)cmdBaic.Parameters.Add("@byteAry7", OleDbType.Binary)).Value = ArayTmpMode;
                try
                {
                    cmdBaic.ExecuteNonQuery();
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
            #endregion
        }

        /// <summary>
        ///上传设置
        /// </summary>
        public bool UploadSensorInfosToDevice(string DevName, int intActivePage)// 0 mean all, else that tab only
        {
            string strMainRemark = DevName.Split('\\')[1].Trim().Split('(')[0].Trim();
            DevName = DevName.Split('\\')[0].Trim();
            //保存basic informations
            byte bytSubID = byte.Parse(DevName.Split('-')[0].ToString());
            byte bytDevID = byte.Parse(DevName.Split('-')[1].ToString());
            byte[] ArayMain = new byte[20];
            byte[] arayTmpRemark = HDLUDP.StringToByte(strMainRemark);
            if (arayTmpRemark.Length >20) 
            {
                Array.Copy(arayTmpRemark, 0, ArayMain, 0, 20);
            }
            else
            {
                Array.Copy(arayTmpRemark, 0, ArayMain, 0, arayTmpRemark.Length);
            }

            if (CsConst.mySends.AddBufToSndList(ArayMain, 0x0010, bytSubID, bytDevID, false, true, true, false) == true)
            {
                CsConst.myRevBuf = new byte[1200];
                HDLUDP.TimeBetwnNext(20);
            }
            else return false;

            byte[] ArayTmp = new byte[3];
            
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
                            CsConst.myRevBuf = new byte[1200];
                            HDLUDP.TimeBetwnNext(20);
                        }
                        else return false;

                        araySendIR[1] = 1;  
                        for (int K = 0; K <= 9; K++) araySendIR[2 + K] = arayRemark[10 + K];

                        if (CsConst.mySends.AddBufToSndList(araySendIR, 0x1690, bytSubID, bytDevID, false, true, true, false) == true)
                        {
                            CsConst.myRevBuf = new byte[1200];
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
                        CsConst.myRevBuf = new byte[1200];
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
                                CsConst.myRevBuf = new byte[1200];
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
                                                CsConst.myRevBuf = new byte[1200];
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
                ArayTmp[0] = 2;
                Array.Copy(ONLEDs, 0, ArayTmp, 1, 2);
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xDB3C, bytSubID, bytDevID, false, true, true, false) == true)
                {
                    CsConst.myRevBuf = new byte[1200];
                    HDLUDP.TimeBetwnNext(20);
                } return false;
                #endregion
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(21);
                //修改使能位
                #region
                ArayTmp = new byte[9];
                Array.Copy(EnableSensors, 0, ArayTmp, 0, 9);
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1660, bytSubID, bytDevID, false, true, true, false) == true)
                {
                    CsConst.myRevBuf = new byte[1200];
                    HDLUDP.TimeBetwnNext(20);
                }
                else return false;
                #endregion
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(22);
                //修改补偿值和灵敏度 
                #region
                ArayTmp = new byte[4];
                Array.Copy(ParamSensors, 0, ArayTmp, 0, 2); // 只有温度亮度
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1643, bytSubID, bytDevID, false, true, true, false) == true)//修改2014-12-04 原来0x1641 --> 0x1643
                {
                    CsConst.myRevBuf = new byte[1200];
                    HDLUDP.TimeBetwnNext(20);
                }
                else return false;
                #endregion
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(23);
                //修改灵敏度
                #region
                ArayTmp = new byte[2];
                Array.Copy(ParamSensors, 2, ArayTmp, 0, 2);
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1670, bytSubID, bytDevID, false, true, true, false) == true)
                {
                    CsConst.myRevBuf = new byte[1200];
                    HDLUDP.TimeBetwnNext(20);
                }
                else return false;
                #endregion
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(24);
                // 增加恒照度修改保存
                #region
                ArayTmp = new byte[9];
                Array.Copy(EnableBroads, 0, ArayTmp, 0, 9);
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x16AB, bytSubID, bytDevID, false, true, true, false) == false)
                {
                    CsConst.myRevBuf = new byte[1200];
                    HDLUDP.TimeBetwnNext(20);
                }
                #endregion
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
                        CsConst.myRevBuf = new byte[1200];
                        HDLUDP.TimeBetwnNext(20);

                        byte bytI = 1;
                        foreach (Logic oLogic in logic)
                        {
                            #region
                            if (oLogic.Enabled != 0)
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
                                    CsConst.myRevBuf = new byte[1200];
                                    HDLUDP.TimeBetwnNext(20);
                                }
                                else return false;
                                #endregion

                                //修改设置
                                #region
                                ArayTmp = new byte[33];
                                ArayTmp[0] = bytI;
                                ArayTmp[1] = oLogic.bytRelation;
                                int intTmp = 0;
                                for (byte bytJ = 0; bytJ <= 8; bytJ++) intTmp = intTmp | (oLogic.EnableSensors[bytJ] << bytJ);
                                ArayTmp[2] = (Byte)(intTmp / 256);
                                ArayTmp[3] = (Byte)(intTmp % 256);
                                ArayTmp[4] = (Byte)(oLogic.DelayTimeT / 256);
                                ArayTmp[5] = (Byte)(oLogic.DelayTimeT % 256);
                                Array.Copy(oLogic.Paramters, 0, ArayTmp, 6, 10);

                                ArayTmp[16] = 201;
                                ArayTmp[18] = 202;

                                if (oLogic.UV1 != null)
                                {
                                    ArayTmp[16] = oLogic.UV1.UvNum;
                                    ArayTmp[17] = oLogic.UV1.UVCondition;
                                }

                                if (oLogic.UV2 != null)
                                {
                                    ArayTmp[18] = oLogic.UV2.UvNum;
                                    ArayTmp[19] = oLogic.UV2.UVCondition;
                                }
                                ArayTmp[20] = Convert.ToByte(oLogic.DelayTimeF / 256);
                                ArayTmp[21] = Convert.ToByte(oLogic.DelayTimeF % 256);
                                ArayTmp[22] = oLogic.Paramters[10];
                                ArayTmp[23] = oLogic.Paramters[11];

                                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1652, bytSubID, bytDevID, false, true, true, false) == true)
                                {
                                    CsConst.myRevBuf = new byte[1200];
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
                                            if (CsConst.mySends.AddBufToSndList(arayCMD, 0x1656, bytSubID, bytDevID, false, true, true, false) == true)
                                            {
                                                CsConst.myRevBuf = new byte[1200];
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
                                            if (CsConst.mySends.AddBufToSndList(arayCMD, 0x1656, bytSubID, bytDevID, false, true, true, false) == true)
                                            {
                                                CsConst.myRevBuf = new byte[1200];
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
                            CsConst.myRevBuf = new byte[1200];
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
                            CsConst.myRevBuf = new byte[1200];
                            HDLUDP.TimeBetwnNext(20);
                        }
                        else return false;
                        if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(10 * oTmp.bytSeuID / fireset.Count + 60);
                    }
                }
                #endregion
            }

            if (intActivePage == 0 || intActivePage == 6)
            {
                //继电器设置
                #region
                if (MyRelays != null)
                {
                    byte bytI = 1;
                    foreach (RelaySetup oTmp in MyRelays)
                    {
                        ArayTmp = new byte[21];
                        ArayTmp[0] = bytI;

                        //修改备注
                        string strRemark = oTmp.RelayName;
                        if (strRemark != null && strRemark != "")
                        {
                            byte[] arayTmp2 = HDLUDP.StringToByte(strRemark);
                            Array.Copy(arayTmp2, 0, ArayTmp, 1, arayTmp2.Length);
                        }

                        if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1684, bytSubID, bytDevID, false, true, true, false) == true)
                        {
                            CsConst.myRevBuf = new byte[1200];
                            HDLUDP.TimeBetwnNext(20);
                        }
                        else return false;
                        //修改设置
                        ArayTmp = new byte[8];
                        ArayTmp[0] = bytI;
                        Array.Copy(oTmp.Setup, 1, ArayTmp, 1, 7);

                        if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1680, bytSubID, bytDevID, false, true, true, false) == true)
                        {
                            CsConst.myRevBuf = new byte[1200];
                            HDLUDP.TimeBetwnNext(20);
                        }
                        else return false;
                        if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(70 + 10 * bytI / MyRelays.Length);
                        bytI++;
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
        public bool DownloadSensorInfosToDevice(string DevName, int wdDeviceType, int intActivePage, int num1, int num2) // 0 mean all, else that tab only
        {
            string strMainRemark = DevName.Split('\\')[1].Trim();
            DevName = DevName.Split('\\')[0].Trim();

            //保存basic informations网络信息
            byte bytSubID = byte.Parse(DevName.Split('-')[0].ToString());
            byte bytDevID = byte.Parse(DevName.Split('-')[1].ToString());
            byte[] ArayTmp = null;

            String sRemark = HDLSysPF.ReadDeviceMainRemark(bytSubID, bytDevID);

            Devname = bytSubID.ToString() + "-" + bytDevID.ToString() + "\\" + sRemark;
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
                    CsConst.myRevBuf = new byte[1200];
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
                        CsConst.myRevBuf = new byte[1200];
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
                            CsConst.myRevBuf = new byte[1200];
                            HDLUDP.TimeBetwnNext(1);
                        }
                        else return false;
                        ArayTmp = new byte[2];
                        ArayTmp[0] = Convert.ToByte(IRCodes[i].KeyID - 1);
                        ArayTmp[1] = 1;
                        if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x168E, bytSubID, bytDevID, false, true, true, false) == true)
                        {
                            Array.Copy(CsConst.myRevBuf, 29, arayRemark, 10, 10);
                            CsConst.myRevBuf = new byte[1200];
                            HDLUDP.TimeBetwnNext(1);
                        }
                        else return false;
                        IRCodes[i].Remark1 = HDLPF.Byte2String(arayRemark);
                    }
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(10 * (i+1) / IRCodes.Count);
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
                    CsConst.myRevBuf = new byte[1200];
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
                            CsConst.myRevBuf = new byte[1200];
                            HDLUDP.TimeBetwnNext(1);
                        }
                        else return false;
                        #endregion
                        //按键目标
                        oTmp.TargetInfo = ReadIrReceiverCommandGroup(bytSubID, bytDevID, oTmp.IRBtnModel,(Byte) intI);
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
                        CsConst.myRevBuf = new byte[1200];
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
                        Array.Copy(CsConst.myRevBuf, 26, EnableSensors, 0, 9);
                        CsConst.myRevBuf = new byte[1200];
                        HDLUDP.TimeBetwnNext(1);
                    }
                    else return false;
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(23);
                    #endregion
                }
                if (intActivePage == 0 || intActivePage == 3)
                {
                    //读取补偿值 十二合一额外命令读写
                    #region
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1641, bytSubID, bytDevID, false, true, true, false) == true)
                    {
                        Array.Copy(CsConst.myRevBuf, 26, ParamSensors, 0, 4);
                        CsConst.myRevBuf = new byte[1200];
                        HDLUDP.TimeBetwnNext(1);
                    }
                    else return false;
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(24);
                    #endregion
                    //读取灵敏度
                    #region
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x166E, bytSubID, bytDevID, false, true, true, false) == true)
                    {
                        Array.Copy(CsConst.myRevBuf, 26, ParamSensors, 2, 2);
                        CsConst.myRevBuf = new byte[1200];
                        HDLUDP.TimeBetwnNext(1);
                    }
                    else return false;
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(25);
                    #endregion
                    //恒照度读取
                    #region
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x16A9, bytSubID, bytDevID, false, true, true, false) == true)
                    {
                        Array.Copy(CsConst.myRevBuf, 26, EnableBroads, 0, 9);
                        CsConst.myRevBuf = new byte[1200];
                        HDLUDP.TimeBetwnNext(1);
                    }
                    else return false;
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(26);
                    #endregion
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
                    logic = new List<Logic>();
                    Byte[] ArayTmpMode = new Byte[24];
                    Byte[] arrPowerOnFlag = new Byte[24];

                    Array.Copy(CsConst.myRevBuf, 26, ArayTmpMode, 0, 24);
                    CsConst.myRevBuf = new byte[1200];
                    HDLUDP.TimeBetwnNext(1);

                    //读取上电状态
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x169E, bytSubID, bytDevID, false, true, true, false) == true)
                    {
                        Array.Copy(CsConst.myRevBuf, 26, arrPowerOnFlag, 0, 24);
                    }

                    for (int intI = 1; intI <= 24; intI++)
                    {
                        Logic oTmp = new Logic();
                        oTmp.ID = (byte)intI;
                        oTmp.Enabled = ArayTmpMode[intI - 1];
                        oTmp.bPowerOnFlag = arrPowerOnFlag[intI - 1];
                        //读取备注
                        #region
                        ArayTmp = new byte[1];
                        ArayTmp[0] = Convert.ToByte(intI);
                        if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1648, bytSubID, bytDevID, false, true, true, false) == true)
                        {
                            byte[] arayRemark = new byte[20];
                            for (int intJ = 0; intJ < 20; intJ++) { arayRemark[intJ] = CsConst.myRevBuf[27 + intJ]; }
                            oTmp.Remarklogic = HDLPF.Byte2String(arayRemark);
                            CsConst.myRevBuf = new byte[1200];
                            HDLUDP.TimeBetwnNext(1);
                        }
                        else return false;
                        #endregion
                        //读取设置
                        if (oTmp.Enabled != 1) // 无效不进一步读取其他信息
                        {
                            logic.Add(oTmp);
                            continue;
                        }
                        #region
                        if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1650, bytSubID, bytDevID, false, true, true, false) == true)
                        {
                            oTmp.bytRelation = CsConst.myRevBuf[27];
                            oTmp.EnableSensors = new byte[15];
                            int intTmp = CsConst.myRevBuf[28] * 256 + CsConst.myRevBuf[29];
                            //oTmp.EnableSensors
                            for (byte bytI = 0; bytI <= 8; bytI++)
                            {
                                oTmp.EnableSensors[bytI] = Convert.ToByte((intTmp & (1 << bytI)) == (1 << bytI));
                            }

                            oTmp.DelayTimeT = CsConst.myRevBuf[30] * 256 + CsConst.myRevBuf[31];
                            Array.Copy(CsConst.myRevBuf, 32, oTmp.Paramters, 0, 10);
                            oTmp.UV1 = new UVSet();
                            oTmp.UV1.UvNum = Convert.ToByte(HDLPF.IsNumStringMode(CsConst.myRevBuf[42].ToString(),201,248));
                            oTmp.UV1.UVCondition = CsConst.myRevBuf[43];

                            oTmp.UV2 = new UVSet();
                            oTmp.UV2.UvNum = Convert.ToByte(HDLPF.IsNumStringMode(CsConst.myRevBuf[44].ToString(), 201, 248));
                            if (oTmp.UV2.UvNum == oTmp.UV1.UvNum) oTmp.UV2.UvNum = Convert.ToByte(oTmp.UV1.UvNum + 1);
                            oTmp.UV2.UVCondition = CsConst.myRevBuf[45];

                            oTmp.DelayTimeF = CsConst.myRevBuf[46] * 256 + CsConst.myRevBuf[47];
                            oTmp.Paramters[10] = CsConst.myRevBuf[48];
                            oTmp.Paramters[11] = CsConst.myRevBuf[49];
                            CsConst.myRevBuf = new byte[1200];
                            HDLUDP.TimeBetwnNext(1);
                            if (CsConst.isRestore)
                            {
                                ArayTmp = new byte[1];
                                ArayTmp[0] = oTmp.UV1.UvNum;
                                if ((CsConst.mySends.AddBufToSndList(ArayTmp, 0x164C, bytSubID, bytDevID, false, false, true, false) == true))
                                {
                                    byte[] arayRemark = new byte[20];
                                    Array.Copy(CsConst.myRevBuf, 27, arayRemark, 0, arayRemark.Length);
                                    oTmp.UV1.UvRemark = HDLPF.Byte2String(arayRemark);
                                    oTmp.UV1.AutoOff = (CsConst.myRevBuf[47] == 1);
                                    if (CsConst.myRevBuf[48] * 256 + CsConst.myRevBuf[49] > 3600)
                                        oTmp.UV1.OffTime = 1;
                                    else
                                        oTmp.UV1.OffTime = CsConst.myRevBuf[48] * 256 + CsConst.myRevBuf[49];
                                    CsConst.myRevBuf = new byte[1200];
                                    HDLUDP.TimeBetwnNext(1);
                                }

                                ArayTmp[0] = oTmp.UV2.UvNum;
                                if ((CsConst.mySends.AddBufToSndList(ArayTmp, 0x164C, bytSubID, bytDevID, false, false, true, false) == true))
                                {
                                    byte[] arayRemark = new byte[20];
                                    Array.Copy(CsConst.myRevBuf, 27, arayRemark, 0, arayRemark.Length);
                                    oTmp.UV2.UvRemark = HDLPF.Byte2String(arayRemark);
                                    oTmp.UV2.AutoOff = (CsConst.myRevBuf[47] == 1);
                                    if (CsConst.myRevBuf[48] * 256 + CsConst.myRevBuf[49] > 3600)
                                        oTmp.UV2.OffTime = 1;
                                    else
                                        oTmp.UV2.OffTime = CsConst.myRevBuf[48] * 256 + CsConst.myRevBuf[49];
                                    CsConst.myRevBuf = new byte[1200];
                                    HDLUDP.TimeBetwnNext(1);
                                }
                            }
                        }
                        else return false;
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
                                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1654, bytSubID, bytDevID, false, true, true, false) == true)
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
                                    CsConst.myRevBuf = new byte[1200];
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

                                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1654, bytSubID, bytDevID, false, true, true, false) == true)
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
                                    CsConst.myRevBuf = new byte[1200];
                                    HDLUDP.TimeBetwnNext(1);
                                }
                                else return false;
                            }
                        }
                        #endregion
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
                for (byte intI = 1; intI <= 4; intI++)
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
                        CsConst.myRevBuf = new byte[1200];
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
                        CsConst.myRevBuf = new byte[1200];
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
            if (intActivePage == 0 || intActivePage == 6)
            {
                //继电器信息
                #region
                MyRelays = new RelaySetup[2];
                ArayTmp = new byte[1];
                for (byte bytI = 1; bytI < 3; bytI++)
                {
                    ArayTmp[0] = bytI;
                    MyRelays[bytI - 1] = new RelaySetup();
                    MyRelays[bytI - 1].Setup = new byte[20];
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1682, bytSubID, bytDevID, false, true, true, false) == true)
                    {
                        byte[] arayRemark = new byte[20];
                        for (int intJ = 0; intJ < 20; intJ++) { arayRemark[intJ] = CsConst.myRevBuf[27 + intJ]; }
                        MyRelays[bytI - 1].RelayName = HDLPF.Byte2String(arayRemark);
                        CsConst.myRevBuf = new byte[1200];
                        HDLUDP.TimeBetwnNext(1);
                    }
                    else return false;

                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x167E, bytSubID, bytDevID, false, true, true, false) == true)
                    {
                        Array.Copy(CsConst.myRevBuf, 26, MyRelays[bytI - 1].Setup, 0, 8);
                        CsConst.myRevBuf = new byte[1200];
                        HDLUDP.TimeBetwnNext(1);
                    }
                    else return false;
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(50 + (bytI * 10 / 2));
                }
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x0033, bytSubID, bytDevID, false, false, true, false) == true)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        MyRelays[i].Setup[9] = CsConst.myRevBuf[26 + i];
                    }
                    CsConst.myRevBuf = new byte[1200];
                    HDLUDP.TimeBetwnNext(1);
                }
                #endregion
                MyRead2UpFlags[5] = true;
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(60);
            if (intActivePage == 0 || intActivePage == 7)
            {
                //模拟信息
                #region
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1662, bytSubID, bytDevID, false, true, true, false) == true)
                {
                    SimulateEnable = new byte[15];
                    SimulateEnable[0] = CsConst.myRevBuf[26];
                    SimulateEnable[1] = CsConst.myRevBuf[27];
                    SimulateEnable[2] = CsConst.myRevBuf[28];
                    SimulateEnable[3] = CsConst.myRevBuf[29];
                    SimulateEnable[4] = CsConst.myRevBuf[30];
                    ParamSimulate = new byte[15];
                    ParamSimulate[0] = CsConst.myRevBuf[31];
                    ParamSimulate[1] = CsConst.myRevBuf[32];
                    ParamSimulate[2] = CsConst.myRevBuf[33];
                    ParamSimulate[3] = CsConst.myRevBuf[34];
                    ParamSimulate[4] = CsConst.myRevBuf[35];
                    CsConst.myRevBuf = new byte[1200];
                    HDLUDP.TimeBetwnNext(1);
                }
                else return false;
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(61);
                #endregion
                MyRead2UpFlags[6] = true;
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(70);
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(100);
            return true;
        }


        public List<UVCMD.ControlTargets> ReadIrReceiverCommandGroup(Byte bytSubID,Byte bytDevID, Byte bButtonMode, Byte bButtonId)
        {
            List<UVCMD.ControlTargets> oTmp = new List<UVCMD.ControlTargets>();
            try
            {
                #region
                if (CsConst.isRestore)
                {
                    byte bytTotalCMD = 1;
                    switch (bButtonMode)
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
                        Byte[] ArayTmp = new byte[2];
                        ArayTmp[0] = bButtonId;
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
                            CsConst.myRevBuf = new byte[1200];
                            oTmp.Add(oCMD);
                            CsConst.myRevBuf = new byte[1200];
                            HDLUDP.TimeBetwnNext(1);
                        }
                        else return oTmp;
                    }
                }
                #endregion
            }
            catch
            {
                return oTmp;
            }
            return oTmp;
        }

        public Boolean ModifyIrReceiverCommandGroup(Byte bytSubID, Byte bytDevID, IRReceive oTmp)
        {
            Boolean bIsSuccess = false;
            try
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
                            bIsSuccess = CsConst.mySends.AddBufToSndList(arayCMD, 0x1678, bytSubID, bytDevID, false, true, true, false);
                            if (bIsSuccess == true)
                            {
                                CsConst.myRevBuf = new byte[1200];
                                HDLUDP.TimeBetwnNext(20);
                            }
                        }
                    }
                }
            }
            catch
            {
                return bIsSuccess;
            }
            return bIsSuccess;
        }

        public List<UVCMD.ControlTargets> DownloadLogicTrueCommandsFromDevice(Byte SubNetID, Byte DeviceID, int DeviceType, Byte bLogicId, Byte bStartCmd, Byte bToCmd)
        {
            Byte sumLogicCommandsInEveryBlock = 20;
            Int32 operationCode = 0x1654;
                
            if (bStartCmd == 0 && bToCmd == 0)
            {
                bStartCmd = 1;
                bToCmd = sumLogicCommandsInEveryBlock;
            }
            List<UVCMD.ControlTargets> SetUp = new List<UVCMD.ControlTargets>();
            try
            {
                //成立的触发目标
                #region
                for (Byte bytJ = 1; bytJ <= sumLogicCommandsInEveryBlock; bytJ++)
                {
                    if (bytJ >= bStartCmd && bytJ <= bToCmd)
                    {
                        Byte[] ArayTmp = new byte[2] { bLogicId, bytJ };
                        if (CsConst.mySends.AddBufToSndList(ArayTmp, operationCode, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
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
                            CsConst.myRevBuf = new byte[1200];
                            SetUp.Add(oCMD);
                            CsConst.myRevBuf = new byte[1200];
                            HDLUDP.TimeBetwnNext(1);
                        }
                        else return SetUp;
                    }
                }
                #endregion
            }
            catch
            {
                return SetUp;
            }
            return SetUp;
        }

        public List<UVCMD.ControlTargets> DownloadLogicFalseCommandsFromDevice(Byte SubNetID, Byte DeviceID, int DeviceType, Byte bLogicId, Byte bStartCmd, Byte bToCmd)
        {
            List<UVCMD.ControlTargets> NoSetUp = new List<UVCMD.ControlTargets>();
            try
            {
                Byte sumLogicCommandsInEveryBlock = 20;
                Byte sumLogibBlockInModule = 24;
                Int32 operationCode = 0x1654;

                if (bStartCmd == 0 && bToCmd == 0)
                {
                    bStartCmd = 1;
                    bToCmd = sumLogicCommandsInEveryBlock;
                }
                //不成立的触发目标
                #region
                for (Byte bytJ = 1; bytJ <= sumLogicCommandsInEveryBlock; bytJ++)
                {
                    if (bytJ >= bStartCmd && bytJ <= bToCmd)
                    {
                        Byte[] ArayTmp = new byte[2] { (Byte)(bLogicId + sumLogibBlockInModule), bytJ };
                        if (CsConst.mySends.AddBufToSndList(ArayTmp, operationCode, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
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
                            CsConst.myRevBuf = new byte[1200];
                            NoSetUp.Add(oCMD);
                            CsConst.myRevBuf = new byte[1200];
                            HDLUDP.TimeBetwnNext(1);
                        }
                        else return NoSetUp;
                    }
                }
                #endregion

            }
            catch 
            {
                return NoSetUp;
            }
            return NoSetUp;
        }

        public Boolean ModifyLogicTrueCommandsFromDevice(Byte SubNetID, Byte DeviceID, int DeviceType,Byte bLogicId, List<UVCMD.ControlTargets> SetUp)
        {
            Boolean blnIsSuccess = true;
            try
            {
                Int32 operationCode =  0x1656;

                if (SetUp != null && SetUp.Count != 0)
                {
                    foreach (UVCMD.ControlTargets TmpCmd in SetUp)
                    {
                        byte[] arayCMD = new byte[9];
                        arayCMD[0] = bLogicId;
                        arayCMD[1] = byte.Parse(TmpCmd.ID.ToString());
                        arayCMD[2] = TmpCmd.Type;
                        arayCMD[3] = TmpCmd.SubnetID;
                        arayCMD[4] = TmpCmd.DeviceID;
                        arayCMD[5] = TmpCmd.Param1;
                        arayCMD[6] = TmpCmd.Param2;
                        arayCMD[7] = TmpCmd.Param3;   // save targets
                        arayCMD[8] = TmpCmd.Param4;
                        blnIsSuccess = CsConst.mySends.AddBufToSndList(arayCMD, operationCode, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType));
                    }
                }
            }
            catch { }
            return blnIsSuccess;
        }

        public Boolean ModifyLogicFalseCommandsFromDevice(Byte SubNetID, Byte DeviceID, int DeviceType,Byte bLogicId, List<UVCMD.ControlTargets> NoSetUp)
        {
            Boolean blnIsSuccess = true;
            try
            {
                Byte sumLogibBlockInModule = 24;
                Int32 operationCode = 0x1656;

                if (NoSetUp != null && NoSetUp.Count != 0)
                {
                    foreach (UVCMD.ControlTargets TmpCmd in NoSetUp)
                    {
                        byte[] arayCMD = new byte[9];
                        arayCMD[0] = (Byte)(bLogicId + sumLogibBlockInModule);
                        arayCMD[1] = byte.Parse(TmpCmd.ID.ToString());
                        arayCMD[2] = TmpCmd.Type;
                        arayCMD[3] = TmpCmd.SubnetID;
                        arayCMD[4] = TmpCmd.DeviceID;
                        arayCMD[5] = TmpCmd.Param1;
                        arayCMD[6] = TmpCmd.Param2;
                        arayCMD[7] = TmpCmd.Param3;   // save targets
                        arayCMD[8] = TmpCmd.Param4;
                        blnIsSuccess = CsConst.mySends.AddBufToSndList(arayCMD, operationCode, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType));
                    }
                }
            }
            catch { }
            return blnIsSuccess;
        }
    }
}
