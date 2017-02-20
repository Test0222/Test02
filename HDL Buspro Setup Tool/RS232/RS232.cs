using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.Xml;
using System.IO;
using System.Windows.Forms;

namespace HDL_Buspro_Setup_Tool
{
    [Serializable]
    public class RS232 : HdlDeviceBackupAndRestore
    {
        public int DIndex;  ////设备唯一编号
        public string strName;

        public byte byt232Paut; // 波特率
        public byte byt232Paity; // 停止位  校验位

        public byte byt485Paut; // 波特率
        public byte byt485Paity; // 停止位  校验位

        public byte bytMode; // 转换模式 232 485 音乐或者其他
        public string SomfyID;//尚飞窗帘ID
        public byte Time;//空调查询间隔
        public byte Count;//空调总数
        public byte[] ArayExtra = new byte[50]; // 预留空间


        public List<Rs232ToBus> myRS2BUS; // Rs232 to bus commands 
        public List<BUS2RS> myBUS2RS; // bus to  RS232 commands
        public List<Rs232ToBus> my4852BUS; // standrad bus 485 to bus commands 
        public List<BUS2RS> myBUS2485; // bus to  standrad bus commands
        public List<byte[]> myFilter;
        public List<byte[]> myComMax;
        public List<byte[]> myAC;
        public List<byte[]> myCurTain;

        public bool[] MyRead2UpFlags = new bool[20]; //前面四个表示读取 后面表示上传
  
        // bus 控制RS232
        public class BUS2RS
        {
            public byte ID;
            public string Remark;
            public byte bytType;
            public byte bytParam1;
            public byte bytParam2;
            public byte bytParam3;
            public byte bytParam4;
            public List<Rs232Param> RS232CMD;
        }   


        //<summary>
        //读取默认的动静传感器设置，将所有数据读取缓存
        //</summary>
        public void ReadDefaultInfo()
        {

            byt232Paut = 2; // 波特率
            byt232Paity = 1; // 停止位  校验位
            byt485Paut = 2; // 波特率
            byt485Paity = 1; // 停止位  校验位
            bytMode = 0; // 转换模式 232 485 音乐或者其他

            if (myRS2BUS == null) myRS2BUS = new List<Rs232ToBus>();
            Rs232ToBus temp1 = new Rs232ToBus();
            temp1.rs232Param = new Rs232Param();
            if (myBUS2485 == null) myBUS2485 = new List<BUS2RS>();
            BUS2RS temp2 = new BUS2RS();
            temp2.RS232CMD = new List<Rs232Param>();
            if (my4852BUS == null) my4852BUS = new List<Rs232ToBus>();
            if (myBUS2RS == null) myBUS2RS = new List<BUS2RS>();
            if (myFilter == null) myFilter = new List<byte[]>();
            if (myComMax == null) myComMax = new List<byte[]>();
            if (myAC == null) myAC = new List<byte[]>();
            if (myCurTain == null) myCurTain = new List<byte[]>();
        }

        //<summary>
        //读取数据库面板设置，将所有数据读至缓存
        //</summary>
        public void ReadRS232FrmDBTobuf(int DIndex)
        {
            #region
            myRS2BUS = new List<Rs232ToBus>();
            string strsql = string.Format("select * from dbClassInfomation where DIndex={0} and ClassID={1} order by SenNum", DIndex, 0);
            OleDbDataReader dr = DataModule.SearchAResultSQLDB(strsql, CsConst.mstrCurPath);
            if (dr != null)
            {
                while (dr.Read())
                {
                    Rs232ToBus temp = new Rs232ToBus();
                    temp.rs232Param = new Rs232Param();
                    temp.busTargets = new List<UVCMD.ControlTargets>();
                    string str = dr.GetValue(5).ToString();
                    temp.ID = Convert.ToByte(str.Split('-')[0].ToString());
                    temp.rs232Param.enable = Convert.ToByte(str.Split('-')[1].ToString());
                    temp.rs232Param.type = Convert.ToByte(str.Split('-')[2].ToString());
                    //temp.rs232Param.rsCmd = ((byte[])dr[6]);
                    temp.remark = dr.GetValue(4).ToString();
                    string str1 = string.Format("select * from dbKeyTargets where DIndex ={0} and KeyIndex = {1} and Ms04State = {2}", DIndex, temp.ID, 1);
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
            strsql = string.Format("select * from dbClassInfomation where DIndex={0} and ClassID={1} order by SenNum", DIndex, 1);
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
                    temp.RS232CMD = new List<Rs232Param>(); // List<RS232Param>();
                    string str1 = string.Format("select * from dbClassInfomation where DIndex = {0} and ClassID = {1} and ObjectID = {2} order by SenNum", DIndex, 2, temp.ID);
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
                            tmp.rsCmd = "";
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
            my4852BUS = new List<Rs232ToBus>();
            strsql = string.Format("select * from dbClassInfomation where DIndex={0} and ClassID={1} order by SenNum", DIndex, 3);
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
                    temp.rs232Param.enable = Convert.ToByte(str.Split('-')[1].ToString());
                    temp.rs232Param.type = Convert.ToByte(str.Split('-')[2].ToString());
                    //temp.rs232Param.rsCmd = ((byte[])dr[6]);
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
                    my4852BUS.Add(temp);
                }
                dr.Close();
            }
            #endregion

            #region
            myBUS2485 = new List<BUS2RS>();
            strsql = string.Format("select * from dbClassInfomation where DIndex={0} and ClassID={1} order by SenNum", DIndex, 4);
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
                    string str1 = string.Format("select * from dbClassInfomation where DIndex = {0} and ClassID = {1} and ObjectID = {2} order by SenNum", DIndex, 5, temp.ID);
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
                            tmp.rsCmd = "";
                            temp.RS232CMD.Add(tmp);
                        }
                        drKeyCmds.Close();
                    }
                    myBUS2485.Add(temp);
                }
                dr.Close();
            }
            #endregion

            #region
            myFilter = new List<byte[]>();
            strsql = string.Format("select * from dbClassInfomation where DIndex={0} and ClassID={1} order by SenNum", DIndex, 6);
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
            strsql = string.Format("select * from dbClassInfomation where DIndex={0} and ClassID={1} order by SenNum", DIndex, 7);
            dr = DataModule.SearchAResultSQLDB(strsql, CsConst.mstrCurPath);
            if (dr != null)
            {
                while (dr.Read())
                {
                    string str = dr.GetValue(5).ToString();
                    byt232Paut = Convert.ToByte(str.Split('-')[0].ToString());
                    byt232Paity = Convert.ToByte(str.Split('-')[1].ToString());
                    byt485Paut = Convert.ToByte(str.Split('-')[2].ToString());
                    byt485Paity = Convert.ToByte(str.Split('-')[3].ToString());
                    bytMode = Convert.ToByte(str.Split('-')[4].ToString());
                    SomfyID = Convert.ToString(str.Split('-')[5].ToString());
                    ArayExtra = ((byte[])dr[6]);             
                }
                dr.Close();
            }
            #endregion
        }

        //<summary>
        //保存数据库面板设置，将所有数据保存
        //</summary>
        public void SaveRS232ToDB()
        {
            string strsql = string.Format("delete * from dbClassInfomation where DIndex={0}", DIndex);
            DataModule.ExecuteSQLDatabase(strsql);

            strsql = string.Format("delete * from dbKeyTargets where DIndex={0}", DIndex);
            DataModule.ExecuteSQLDatabase(strsql);

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
                    ((OleDbParameter)cmd.Parameters.Add("@ClassID", OleDbType.Integer)).Value = 0;
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
                                           TmpCmds.Param2, TmpCmds.Param3, TmpCmds.Param4, 1);
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
                             DIndex, 1, 0, i, tmpChn.Remark, strParam);
                    DataModule.ExecuteSQLDatabase(strsql);
                    if (tmpChn.RS232CMD != null)
                    {
                        for (int j = 0; j < tmpChn.RS232CMD.Count; j++)
                        {
                            Rs232Param tmp = tmpChn.RS232CMD[j];
                            if (tmp.rsCmd == null) tmp.rsCmd = "";
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
                            ((OleDbParameter)cmd.Parameters.Add("@ClassID", OleDbType.Integer)).Value = 2;
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
            if (my4852BUS != null)
            {
                for (int i = 0; i < my4852BUS.Count; i++)
                {
                    Rs232ToBus tmpChn = my4852BUS[i];
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
                    ((OleDbParameter)cmd.Parameters.Add("@ClassID", OleDbType.Integer)).Value = 3;
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
            if (myBUS2485 != null)
            {
                for (int i = 0; i < myBUS2485.Count; i++)
                {
                    BUS2RS tmpChn = myBUS2485[i];
                    string strParam = tmpChn.ID.ToString() + "-" + tmpChn.bytType.ToString() + "-"
                                    + tmpChn.bytParam1.ToString() + "-" + tmpChn.bytParam2.ToString();
                    strsql = string.Format("Insert into dbClassInfomation(DIndex,ClassID,ObjectID,SenNum,Remark,strParam1) values ({0},{1},{2},{3},'{4}','{5}')",
                             DIndex, 4, 0, i, tmpChn.Remark, strParam);
                    DataModule.ExecuteSQLDatabase(strsql);
                    if (tmpChn.RS232CMD != null)
                    {
                        for (int j = 0; j < tmpChn.RS232CMD.Count; j++)
                        {
                            Rs232Param tmp = tmpChn.RS232CMD[j];
                            if (tmp.rsCmd == null) tmp.rsCmd = "";
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
                            ((OleDbParameter)cmd.Parameters.Add("@ClassID", OleDbType.Integer)).Value = 5;
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
                    ((OleDbParameter)cmd.Parameters.Add("@ClassID", OleDbType.Integer)).Value = 6;
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
            if (SomfyID == null || SomfyID == "") SomfyID = "0000";
            string strParamBasic = byt232Paut.ToString() + "-" + byt232Paity.ToString() + "-" + byt485Paut.ToString()+"-"+
                                   byt485Paity.ToString() + "-" + bytMode.ToString() + "-" + SomfyID.ToString();
            if (ArayExtra == null) ArayExtra = new byte[50];

            strsql = @"Insert into dbClassInfomation(DIndex,ClassID,ObjectID,SenNum,Remark,strParam1,byteAry1)"
                     + " values(@DIndex,@ClassID,@ObjectID,@SenNum,@Remark,@strParam1,@byteAry1)";
            //创建一个OleDbConnection对象
            OleDbConnection connBasic;
            connBasic = new OleDbConnection(DataModule.ConString + CsConst.mstrDefaultPath);
            //OleDbConnection conn = new OleDbConnection(DataModule.ConString + CsConst.mstrCurPath);
            connBasic.Open();
            OleDbCommand cmdBasic = new OleDbCommand(strsql, connBasic);
            ((OleDbParameter)cmdBasic.Parameters.Add("@DIndex", OleDbType.Integer)).Value = DIndex;
            ((OleDbParameter)cmdBasic.Parameters.Add("@ClassID", OleDbType.Integer)).Value = 7;
            ((OleDbParameter)cmdBasic.Parameters.Add("@ObjectID", OleDbType.Integer)).Value = 0;
            ((OleDbParameter)cmdBasic.Parameters.Add("@SenNum", OleDbType.Integer)).Value = 0;
            ((OleDbParameter)cmdBasic.Parameters.Add("@Remark", OleDbType.VarChar)).Value = "";
            ((OleDbParameter)cmdBasic.Parameters.Add("@strParam1", OleDbType.VarChar)).Value = strParamBasic;
            ((OleDbParameter)cmdBasic.Parameters.Add("@byteAry1", OleDbType.Binary)).Value = ArayExtra;
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

        /// <summary>
        ///upload all information to IR sender
        /// </summary>
        /// <param name="DIndex"></param>
        /// <param name="DevID"></param>
        /// <param name="DeviceType"></param>
        /// <param name="DevName"></param>
        /// <returns></returns>
        public bool UploadRS232InfosToDevice(string DevName, int intDeviceType, int intActivePage)// 0 mean all, else that tab only
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
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(1);
            byte[] ArayTmp = new byte[2];
            if (intActivePage == 0 || intActivePage == 1)
            {
                // 修改串口232的配置信息
                #region
                ArayTmp[0] = byt232Paut;
                ArayTmp[1] = byt232Paity;
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xE41E, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(intDeviceType)) == true)
                {
                    
                    HDLUDP.TimeBetwnNext(20);
                }
                else return false;
                #endregion
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(2);
                
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(3);
                // 修改模式和转发标志
                #region
                if (intDeviceType == 1028 || intDeviceType == 1037||intDeviceType == 1042)
                {
                    ArayTmp = new byte[3];
                    while (SomfyID.Length < 6) SomfyID = "0" + SomfyID;
                    ArayTmp[0] = Convert.ToByte(Convert.ToInt32(SomfyID.Substring(0, 2), 16));
                    ArayTmp[1] = Convert.ToByte(Convert.ToInt32(SomfyID.Substring(2, 2), 16));
                    ArayTmp[2] = Convert.ToByte(Convert.ToInt32(SomfyID.Substring(4, 2), 16));
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1F52, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(intDeviceType)) == true)
                    {
                        
                        HDLUDP.TimeBetwnNext(20);
                    }
                    else return false;
                }
                else
                {
                    ArayTmp = new byte[1];
                    ArayTmp[0] = bytMode;
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xE42E, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(intDeviceType)) == true)
                    {
                        
                        HDLUDP.TimeBetwnNext(20);
                    }
                    else return false;

                    if (intDeviceType == 1018 || intDeviceType == 1027)
                    {
                        #region
                        ArayTmp = new byte[3];
                        Array.Copy(ArayExtra, 0, ArayTmp, 0, 3);
                        if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x16AF, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(intDeviceType)) == true)
                        {
                            
                            HDLUDP.TimeBetwnNext(20);
                        }
                        else return false;
                        #endregion
                        
                    }
                }
                #endregion
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(4);
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(10);

            if (intActivePage == 0 || intActivePage == 2)
            {
                #region
                if (myRS2BUS != null)
                {
                    for (int i = 0; i < myRS2BUS.Count; i++)
                    {
                        ArayTmp = new byte[37];
                        if (intDeviceType == 1041)
                        {
                            ArayTmp = new byte[38];
                            ArayTmp[0] = Convert.ToByte(myRS2BUS[i].ID / 256);
                            ArayTmp[1] = Convert.ToByte(myRS2BUS[i].ID % 256);
                            ArayTmp[2] = Convert.ToByte(myRS2BUS[i].rs232Param.enable);
                            ArayTmp[3] = Convert.ToByte(myRS2BUS[i].rs232Param.type);
                           // Array.Copy(myRS2BUS[i].TmpRS232.RSCMD, 0, ArayTmp, 4, myRS2BUS[i].TmpRS232.RSCMD.Length);
                        }
                        else
                        {
                            ArayTmp[0] = Convert.ToByte(myRS2BUS[i].ID);
                            ArayTmp[1] = Convert.ToByte(myRS2BUS[i].rs232Param.enable);
                            ArayTmp[2] = Convert.ToByte(myRS2BUS[i].rs232Param.type);
                            //Array.Copy(myRS2BUS[i].TmpRS232.RSCMD, 0, ArayTmp, 3, myRS2BUS[i].TmpRS232.RSCMD.Length);
                        }
                        if (intDeviceType == 1041)
                        {
                            if (ArayTmp[2] != 0xFF && ArayTmp[37] != 0xFF)
                            {
                                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xE412, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(intDeviceType)) == true)
                                {

                                    HDLUDP.TimeBetwnNext(20);
                                }
                                else return false;
                            }
                        }
                        else
                        {
                            if (ArayTmp[1] != 0xFF && ArayTmp[36] != 0xFF)
                            {
                                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xE412, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(intDeviceType)) == true)
                                {

                                    HDLUDP.TimeBetwnNext(20);
                                }
                                else return false;
                            }
                        }

                        ArayTmp = new byte[21];
                        if (intDeviceType == 1041)
                        {
                            ArayTmp = new byte[22];
                            ArayTmp[0] = Convert.ToByte(myRS2BUS[i].ID / 256);
                            ArayTmp[1] = Convert.ToByte(myRS2BUS[i].ID % 256);
                            byte[] arayRemark = HDLUDP.StringToByte(myRS2BUS[i].remark);
                            if (arayRemark.Length <= 20)
                                arayRemark.CopyTo(ArayTmp, 2);
                            else
                                Array.Copy(arayRemark, 0, ArayTmp, 2, 20);
                        }
                        else
                        {
                            ArayTmp[0] = Convert.ToByte(myRS2BUS[i].ID);
                            byte[] arayRemark = HDLUDP.StringToByte(myRS2BUS[i].remark);
                            if (arayRemark.Length <= 20)
                                arayRemark.CopyTo(ArayTmp, 1);
                            else
                                Array.Copy(arayRemark, 0, ArayTmp, 1, 20);
                        }
                        if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xE41A, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(intDeviceType)) == true)
                        {
                            
                            HDLUDP.TimeBetwnNext(20);
                        }
                        else return false;
                        if (CsConst.isRestore)
                        {
                            if (myRS2BUS[i].busTargets != null)
                            {
                                foreach (UVCMD.ControlTargets TmpCmd in myRS2BUS[i].busTargets)
                                {
                                    if (TmpCmd.Type != 0 && TmpCmd.Type != 255)
                                    {
                                        byte[] arayCMD = new byte[9];
                                        if (intDeviceType == 1041)
                                        {
                                            arayCMD = new byte[10];
                                            arayCMD[0] = Convert.ToByte(myRS2BUS[i].ID / 256);
                                            arayCMD[1] = Convert.ToByte(myRS2BUS[i].ID % 256);
                                            arayCMD[2] = Convert.ToByte(TmpCmd.ID);
                                            arayCMD[3] = TmpCmd.Type;
                                            arayCMD[4] = TmpCmd.SubnetID;
                                            arayCMD[5] = TmpCmd.DeviceID;
                                            arayCMD[6] = TmpCmd.Param1;
                                            arayCMD[7] = TmpCmd.Param2;
                                            arayCMD[8] = TmpCmd.Param3;   // save targets
                                            arayCMD[9] = TmpCmd.Param4;
                                        }
                                        else
                                        {
                                            arayCMD[0] = Convert.ToByte(myRS2BUS[i].ID);
                                            arayCMD[1] = Convert.ToByte(TmpCmd.ID);
                                            arayCMD[2] = TmpCmd.Type;
                                            arayCMD[3] = TmpCmd.SubnetID;
                                            arayCMD[4] = TmpCmd.DeviceID;
                                            arayCMD[5] = TmpCmd.Param1;
                                            arayCMD[6] = TmpCmd.Param2;
                                            arayCMD[7] = TmpCmd.Param3;   // save targets
                                            arayCMD[8] = TmpCmd.Param4;
                                        }
                                        if (CsConst.mySends.AddBufToSndList(arayCMD, 0xE416, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(intDeviceType)) == true)
                                        {
                                            
                                            HDLUDP.TimeBetwnNext(20);
                                        }
                                        else return false;
                                    }
                                }
                            }
                        }
                        if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(10 + 10 * i / myRS2BUS.Count);
                    }
                }
                #endregion
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(20);
            if (intActivePage == 0 || intActivePage == 3)
            {
                #region
                if (myBUS2RS != null)
                {
                    for (int i = 0; i < myBUS2RS.Count; i++)
                    {
                        byte[] arayTmp = new byte[4];
                        arayTmp[0] = Convert.ToByte(myBUS2RS[i].ID);
                        arayTmp[1] = myBUS2RS[i].bytType;
                        arayTmp[2] = myBUS2RS[i].bytParam1;
                        arayTmp[3] = myBUS2RS[i].bytParam2;
                        
                        if (CsConst.mySends.AddBufToSndList(arayTmp, 0xE422, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(intDeviceType)) == true)
                        {
                            
                            HDLUDP.TimeBetwnNext(20);
                        }
                        else return false;

                        arayTmp = new byte[21];
                        arayTmp[0] = Convert.ToByte(myBUS2RS[i].ID);
                        byte[] arayRemark = HDLUDP.StringToByte(myBUS2RS[i].Remark);
                        if (arayRemark.Length <= 20)
                            arayRemark.CopyTo(arayTmp, 1);
                        else
                            Array.Copy(arayRemark, 0, arayTmp, 1, 20);
                        if (CsConst.mySends.AddBufToSndList(arayTmp, 0xE42A, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(intDeviceType)) == true)
                        {
                            
                            HDLUDP.TimeBetwnNext(20);
                        }
                        else return false;
                        if (CsConst.isRestore)
                        {
                            if (myBUS2RS[i].RS232CMD != null)
                            {
                                for (int j = 0; j < myBUS2RS[i].RS232CMD.Count; j++)
                                {
                                    ArayTmp = new byte[33 + 5];
                                    ArayTmp[0] = Convert.ToByte(myBUS2RS[i].ID);
                                    ArayTmp[1] = Convert.ToByte(myBUS2RS[i].RS232CMD[j].Index);
                                    ArayTmp[2] = Convert.ToByte(myBUS2RS[i].RS232CMD[j].enable);
                                    ArayTmp[3] = Convert.ToByte(myBUS2RS[i].RS232CMD[j].type);
                                   // Array.Copy(myBUS2RS[i].RS232CMD[j].RSCMD, 0, ArayTmp, 4, 34);
                                    if (ArayTmp[2] != 0xFF && ArayTmp[37] != 0xFF)
                                    {
                                        if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xE426, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(intDeviceType)) == true)
                                        {
                                            
                                            System.Threading.Thread.Sleep(50);
                                        }
                                        else return false;
                                    }
                                }
                            }
                        }
                        if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(20 + 10 * i / myBUS2RS.Count);
                    }
                }
                #endregion
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(30);
            if (intActivePage == 0 || intActivePage == 4)
            {
                #region
                if (my4852BUS != null)
                {
                    for (int i = 0; i < my4852BUS.Count; i++)
                    {
                        ArayTmp = new byte[37];
                        ArayTmp[0] = Convert.ToByte(my4852BUS[i].ID);
                        ArayTmp[1] = Convert.ToByte(my4852BUS[i].rs232Param.enable);
                        ArayTmp[2] = Convert.ToByte(my4852BUS[i].rs232Param.type);
                       // Array.Copy(my4852BUS[i].rs232Param.rsCmd, 0, ArayTmp, 3, my4852BUS[i].TmpRS232.RSCMD.Length);
                        if (ArayTmp[1] != 0xFF && ArayTmp[36] != 0xFF)
                        {
                            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xDA53, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(intDeviceType)) == true)
                            {
                                
                                HDLUDP.TimeBetwnNext(20);
                            }
                            else return false;
                        }

                        ArayTmp = new byte[21];
                        ArayTmp[0] = Convert.ToByte(my4852BUS[i].ID);
                        byte[] arayRemark = HDLUDP.StringToByte(my4852BUS[i].remark);
                        if (arayRemark.Length <= 20)
                            arayRemark.CopyTo(ArayTmp, 1);
                        else
                            Array.Copy(arayRemark, 0, ArayTmp, 1, 20);
                        if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xDA5B, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(intDeviceType)) == true)
                        {
                            
                            HDLUDP.TimeBetwnNext(20);
                        }
                        else return false;
                        if (CsConst.isRestore)
                        {
                            if (my4852BUS[i].busTargets != null)
                            {
                                foreach (UVCMD.ControlTargets TmpCmd in my4852BUS[i].busTargets)
                                {
                                    if (TmpCmd.Type != 0 && TmpCmd.Type != 255)
                                    {
                                        byte[] arayCMD = new byte[9];
                                        arayCMD[0] = Convert.ToByte(my4852BUS[i].ID);
                                        arayCMD[1] = Convert.ToByte(TmpCmd.ID);
                                        arayCMD[2] = TmpCmd.Type;
                                        arayCMD[3] = TmpCmd.SubnetID;
                                        arayCMD[4] = TmpCmd.DeviceID;
                                        arayCMD[5] = TmpCmd.Param1;
                                        arayCMD[6] = TmpCmd.Param2;
                                        arayCMD[7] = TmpCmd.Param3;   // save targets
                                        arayCMD[8] = TmpCmd.Param4;
                                        if (CsConst.mySends.AddBufToSndList(arayCMD, 0xDA57, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(intDeviceType)) == true)
                                        {
                                            
                                            HDLUDP.TimeBetwnNext(20);
                                        }
                                        else return false;
                                    }
                                }
                            }
                        }
                        if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(30 + 10 * i / myRS2BUS.Count);
                    }
                }
                #endregion
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(40);
            if (intActivePage == 0 || intActivePage == 5)
            {
                #region
                if (myBUS2485 != null)
                {
                    for (int i = 0; i < myBUS2485.Count; i++)
                    {
                        byte[] arayTmp = new byte[4];
                        arayTmp[0] = Convert.ToByte(myBUS2485[i].ID);
                        arayTmp[1] = myBUS2485[i].bytType;
                        arayTmp[2] = myBUS2485[i].bytParam1;
                        arayTmp[3] = myBUS2485[i].bytParam2;
                        if (CsConst.mySends.AddBufToSndList(arayTmp, 0xDA63, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(intDeviceType)) == true)
                        {
                            
                            HDLUDP.TimeBetwnNext(20);
                        }
                        else return false;

                        arayTmp = new byte[21];
                        arayTmp[0] = Convert.ToByte(myBUS2485[i].ID);
                        byte[] arayRemark = HDLUDP.StringToByte(myBUS2485[i].Remark);
                        if (arayRemark.Length <= 20)
                            arayRemark.CopyTo(arayTmp, 1);
                        else
                            Array.Copy(arayRemark, 0, arayTmp, 1, 20);
                        if (CsConst.mySends.AddBufToSndList(arayTmp, 0xDA6B, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(intDeviceType)) == true)
                        {
                            
                            HDLUDP.TimeBetwnNext(20);
                        }
                        else return false;
                        if (CsConst.isRestore)
                        {
                            if (myBUS2485[i].RS232CMD != null)
                            {
                                for (int j = 0; j < myBUS2485[i].RS232CMD.Count; j++)
                                {
                                    ArayTmp = new byte[33 + 5];
                                    ArayTmp[0] = Convert.ToByte(myBUS2485[i].ID);
                                    ArayTmp[1] = Convert.ToByte(myBUS2485[i].RS232CMD[j].Index);
                                    ArayTmp[2] = Convert.ToByte(myBUS2485[i].RS232CMD[j].enable);
                                    ArayTmp[3] = Convert.ToByte(myBUS2485[i].RS232CMD[j].type);
                                   // Array.Copy(myBUS2485[i].RS232CMD[j].RSCMD, 0, ArayTmp, 4, 34);
                                    if (ArayTmp[2] != 0xFF && ArayTmp[37] != 0xFF)
                                    {
                                        if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xDA67, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(intDeviceType)) == true)
                                        {
                                            
                                            System.Threading.Thread.Sleep(50);
                                        }
                                        else return false;
                                    }
                                }
                            }
                        }
                        if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(40 + 10 * i / myBUS2RS.Count);
                    }
                }
                #endregion
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(50);
            if (intActivePage == 0 || intActivePage == 6)
            {
                #region
                if (myFilter != null)
                {
                    for (int i = 0; i < myFilter.Count; i++)
                    {
                        ArayTmp = new byte[4];
                        ArayTmp = myFilter[i];
                        if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1CBA, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(intDeviceType)) == true)
                        {
                            
                            HDLUDP.TimeBetwnNext(10);
                        }
                        else return false;
                        if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(50 + (i / 2));
                    }
                }
                #endregion
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(60);
            if (intActivePage == 0 || intActivePage == 7)
            {
                #region
                if (intDeviceType == 1039)
                {
                    if (myComMax != null)
                    {
                        for (int i = 0; i < myComMax.Count; i++)
                        {
                            ArayTmp = new byte[7];
                            Array.Copy(myComMax[i], 0, ArayTmp, 0, 7);
                            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1CC0, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(intDeviceType)) == true)
                            {
                                
                                HDLUDP.TimeBetwnNext(20);
                            }
                            else return false;
                            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(60 + 10 * i / myComMax.Count);
                        }
                    }
                }
                #endregion
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(70);
            if (intActivePage == 0 || intActivePage == 8)
            {
                #region
                if (intDeviceType == 1034 || intDeviceType == 1004)
                {
                    ArayTmp = new byte[1];
                    ArayTmp[0] = Time;
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x310E, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(intDeviceType)) == true)
                    {
                        HDLUDP.TimeBetwnNext(20);
                    }
                    else return false;
                }
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(71);

                if (myAC != null && myAC.Count > 0)
                {                    
                    ArayTmp = new byte[1];
                    ArayTmp[0] = Count;
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x3102, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(intDeviceType)) == true)
                    {
                        HDLUDP.TimeBetwnNext(20);
                    }
                    else return false;
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(72);
                    int iCommand = 0x3106;
                    if (intDeviceType == 1004) iCommand = 0x3132;
                    for (int i = 0; i < myAC.Count; i++)
                    {
                        ArayTmp = myAC[i];
                        if (CsConst.mySends.AddBufToSndList(ArayTmp, iCommand, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(intDeviceType)) == true)
                        {

                            HDLUDP.TimeBetwnNext(20);
                        }
                        else return false;
                        if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(72 + (i / (myAC.Count + 2)));
                    }
                }
                #endregion
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(80);
            if (intActivePage == 0 || intActivePage == 9)
            {
                #region
                if (intDeviceType == 1037 || intDeviceType == 1042)
                {
                    if (myCurTain != null)
                    {
                        for (int i = 0; i < myCurTain.Count; i++)
                        {
                            ArayTmp = myCurTain[i];
                            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1F56, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(intDeviceType)) == true)
                            {
                                HDLUDP.TimeBetwnNext(20);
                            }
                            else return false;
                            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(80 + 10 * i / myCurTain.Count);
                        }
                    }
                }
                #endregion
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(100);
            return true;
        }

        /// <summary>
        /// devname device name 
        /// </summary>
        /// <param name="DevName"></param>
        public void DownloadRS232FrmDeviceToBuf(string DevName, int intDeviceType, int intActivePage, int num1, int num2)// 0 mean all, else that tab only
        {
            string strMainRemark = DevName.Split('\\')[1].Trim();
            DevName = DevName.Split('\\')[0].Trim();

            //保存basic informations网络信息
            byte bytSubID = byte.Parse(DevName.Split('-')[0].ToString());
            byte bytDevID = byte.Parse(DevName.Split('-')[1].ToString());
            int wdMaxValue = DeviceTypeList.GetMaxValueFromPublicModeGroup(intDeviceType);
            byte[] ArayTmp = null;
            strName = bytSubID.ToString() + "-" + bytDevID.ToString() + "\\" + HDLSysPF.ReadDeviceMainRemark(bytSubID, bytDevID);
           
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(1);
            if (intActivePage == 0 || intActivePage == 1)
            {
                ArayExtra = new byte[50];
                #region
                // 读取串口232的配置信息
                #region
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xE41C, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(intDeviceType)) == true)
                {
                    byt232Paut = CsConst.myRevBuf[26];
                    byt232Paity = CsConst.myRevBuf[27];
                    
                    HDLUDP.TimeBetwnNext(1);
                }
                else return;
                #endregion
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(2);
                
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(3);
                if (intDeviceType == 1028 || intDeviceType == 1037||intDeviceType == 1042)
                {
                    #region
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1F50, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(intDeviceType)) == true)
                    {
                        SomfyID = GlobalClass.AddLeftZero(CsConst.myRevBuf[25].ToString("X"), 2) +
                                  GlobalClass.AddLeftZero(CsConst.myRevBuf[26].ToString("X"), 2) +
                                  GlobalClass.AddLeftZero(CsConst.myRevBuf[27].ToString("X"), 2);

                        HDLUDP.TimeBetwnNext(1);
                    }
                    else return;
                    #endregion
                }
                else
                {
                    // 读取模式和转发标志
                    #region
                    if (intDeviceType == 1018 || intDeviceType == 1027)
                    {
                        #region
                        if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x16AD, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(intDeviceType)) == true)
                        {
                            Array.Copy(CsConst.myRevBuf, 26, ArayExtra, 0, 3);

                            HDLUDP.TimeBetwnNext(1);
                        }
                        else return;
                        #endregion
                    }

                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xE42C, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(intDeviceType)) == true)
                    {
                        bytMode = CsConst.myRevBuf[26];

                        HDLUDP.TimeBetwnNext(1);
                    }
                    else return;

                    #endregion
                }
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(4);
                #endregion
                MyRead2UpFlags[0] = true;
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(10);
            if (intActivePage == 0 || intActivePage == 2) // 232 到bus
            {
                #region
                if (intDeviceType != 1028 && intDeviceType != 1037 && intDeviceType != 1039 && intDeviceType != 1042)
                {
                    myRS2BUS = new List<Rs232ToBus>();
                    if (CsConst.isRestore)
                    {
                        num1 = 1;
                        if (intDeviceType == 1016 || intDeviceType == 1019 || intDeviceType == 1014 || intDeviceType == 1033 ||
                            intDeviceType == 1018 || intDeviceType == 1027)
                        {
                            num2 = 200;
                            if (intDeviceType == 1018 || intDeviceType == 1027 || intDeviceType == 1019)
                            {
                                if (ArayExtra[0] == 0)
                                {
                                    num2 = 99;
                                }
                                else if (ArayExtra[0] == 1)
                                {
                                    num2 = 200;
                                }
                                else if (ArayExtra[0] == 2)
                                {
                                    num2 = 0;
                                }
                            }
                        }
                        else if (intDeviceType == 1041)
                        {
                            num2 = 320;
                        }
                        else
                        {
                            num2 = 99;
                        }
                    }
                    for (int i = num1; i <= num2; i++)
                    {
                        Rs232ToBus temp = new Rs232ToBus();
                        temp.rs232Param = new Rs232Param();
                        ArayTmp = new byte[1];
                        if (intDeviceType == 1041)
                        {
                            ArayTmp = new byte[2];
                            ArayTmp[0] = Convert.ToByte(i / 256);
                            ArayTmp[1] = Convert.ToByte(i % 256);
                        }
                        else
                        {
                            ArayTmp[0] = Convert.ToByte(i);
                        }
                        //读取使能位
                        #region
                        if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xE410, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(intDeviceType)) == true)
                        {
                            temp.ID = i;
                            if (intDeviceType == 1041)
                            {
                                temp.rs232Param.enable = CsConst.myRevBuf[28];
                                temp.rs232Param.type = CsConst.myRevBuf[29];
                                temp.rs232Param.rsCmd = "";
                                if (CsConst.myRevBuf[62] > 33) CsConst.myRevBuf[62] = 0;
                                byte[] arayRemark = new byte[CsConst.myRevBuf[62]];
                                for (int intJ = 0; intJ < arayRemark.Length; intJ++) { arayRemark[intJ] = CsConst.myRevBuf[29 + intJ]; }
                               // temp.rs232Param.RSCMD = HDLPF.Byte2String(arayRemark);
                            }
                            else
                            {
                                temp.rs232Param.enable = CsConst.myRevBuf[27];
                                temp.rs232Param.type = CsConst.myRevBuf[28];
                                temp.rs232Param.rsCmd = "";
                               // Array.Copy(CsConst.myRevBuf, 29, temp.TmpRS232.RSCMD, 0, temp.TmpRS232.RSCMD.Length);
                            }
                            HDLUDP.TimeBetwnNext(1);
                        }
                        else return;
                        #endregion

                        if (temp.rs232Param.enable == 1)  //有效增加功能的读取
                        {
                            // 读取备注
                            #region
                            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xE418, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(intDeviceType)) == true)
                            {
                                byte[] arayRemark = new byte[20];
                                if (intDeviceType == 1041)
                                {
                                    for (int intI = 0; intI < 20; intI++) { arayRemark[intI] = CsConst.myRevBuf[28 + intI]; }
                                }
                                else
                                {
                                    for (int intI = 0; intI < 20; intI++) { arayRemark[intI] = CsConst.myRevBuf[27 + intI]; }
                                }
                                temp.remark = HDLPF.Byte2String(arayRemark);
                                HDLUDP.TimeBetwnNext(1);
                            }
                            else return;
                            temp.busTargets = new List<UVCMD.ControlTargets>();
                            if (CsConst.isRestore)
                            {
                                int num = 20;
                                if (intDeviceType == 1006 || intDeviceType == 1007 || intDeviceType == 1008 ||
                                    intDeviceType == 1009 || intDeviceType == 1016 || intDeviceType == 1019 ||
                                    intDeviceType == 1027 || intDeviceType == 1013 || intDeviceType == 1014 ||
                                    intDeviceType == 1018 || intDeviceType == 1033)
                                {
                                    num = 20;
                                    if (intDeviceType == 1018 || intDeviceType == 1027)
                                    {
                                        if (ArayExtra[0] == 0)
                                        {
                                            num = 10;
                                        }
                                    }
                                }
                                else
                                {
                                    num = 48;
                                }
                                int CMD = 0xE414;
                                if (intDeviceType == 1005)
                                {
                                    CMD = 0xE405;
                                }
                                else if (intDeviceType == 1006 || intDeviceType == 1007 || intDeviceType == 1008 ||
                                         intDeviceType == 1009 || intDeviceType == 1016 || intDeviceType == 1013 ||
                                         intDeviceType == 1014 || intDeviceType == 1018 || intDeviceType == 1019 ||
                                         intDeviceType == 1027 || intDeviceType == 1033)
                                {
                                    CMD = 0xE414;
                                }
                                for (byte j = 1; j <= num; j++)
                                {
                                    byte[] arayTmp = new byte[2];
                                    if (intDeviceType == 1041)
                                    {
                                        arayTmp = new byte[3];
                                        arayTmp[0] = Convert.ToByte(i / 256);
                                        arayTmp[1] = Convert.ToByte(i % 256);
                                        arayTmp[2] = j;
                                    }
                                    else
                                    {
                                        arayTmp[0] = Convert.ToByte(i);
                                        arayTmp[1] = j;
                                    }
                                    if (CsConst.mySends.AddBufToSndList(arayTmp, CMD, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(intDeviceType)) == true)
                                    {
                                        UVCMD.ControlTargets oCMD = new UVCMD.ControlTargets();
                                        oCMD.ID = j;
                                        if (intDeviceType == 1041)
                                        {
                                            oCMD.Type = CsConst.myRevBuf[29];  //转换为正确的类型
                                            oCMD.SubnetID = CsConst.myRevBuf[30];
                                            oCMD.DeviceID = CsConst.myRevBuf[31];
                                            oCMD.Param1 = CsConst.myRevBuf[32];
                                            oCMD.Param2 = CsConst.myRevBuf[33];
                                            oCMD.Param3 = CsConst.myRevBuf[34];
                                            oCMD.Param4 = CsConst.myRevBuf[35];
                                        }
                                        else
                                        {
                                            oCMD.Type = CsConst.myRevBuf[28];  //转换为正确的类型
                                            oCMD.SubnetID = CsConst.myRevBuf[29];
                                            oCMD.DeviceID = CsConst.myRevBuf[30];
                                            oCMD.Param1 = CsConst.myRevBuf[31];
                                            oCMD.Param2 = CsConst.myRevBuf[32];
                                            oCMD.Param3 = CsConst.myRevBuf[33];
                                            oCMD.Param4 = CsConst.myRevBuf[34];
                                        }
                                        temp.busTargets.Add(oCMD);
                                        HDLUDP.TimeBetwnNext(1);
                                    }
                                    else return;
                                }
                            }
                            #endregion
                            myRS2BUS.Add(temp);
                        }
                        if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(10 + 10 * (i - num1) / ((num2 + 1) - num1));
                    }
                }
                #endregion
                MyRead2UpFlags[1] = true;
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(20);
            if (intActivePage == 0 || intActivePage == 3)   //bus到232
            {
                #region
                myBUS2RS = new List<BUS2RS>();
                if (intDeviceType != 1028 && intDeviceType != 1037 && intDeviceType != 1039 && intDeviceType != 1041 && intDeviceType != 1042)
                {
                    if (CsConst.isRestore)
                    {
                        num1 = 1;
                        if (intDeviceType == 1016 || intDeviceType == 1019 || intDeviceType == 1014 || intDeviceType == 1033 ||
                            intDeviceType == 1018 || intDeviceType == 1027)
                        {
                            num2 = 200;
                            if (intDeviceType == 1018 || intDeviceType == 1027 || intDeviceType == 1019)
                            {
                                if (ArayExtra[0] == 0)
                                {
                                    num2 = 99;
                                }
                                else if (ArayExtra[0] == 1)
                                {
                                    num2 = 200;
                                }
                                else if (ArayExtra[0] == 2)
                                {
                                    num2 = 0;
                                }
                            }
                        }
                        else
                            num2 = 99;
                    }
                    for (int i = num1; i <= num2; i++)
                    {
                        BUS2RS temp = new BUS2RS();
                        temp.ID = Convert.ToByte(i);
                        ArayTmp = new byte[1];
                        ArayTmp[0] = Convert.ToByte(i);
                        //读取有效位
                        #region
                        if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xE420, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(intDeviceType)) == true)
                        {
                            temp.bytType = CsConst.myRevBuf[27];
                            temp.bytParam1 = CsConst.myRevBuf[28];
                            temp.bytParam2 = CsConst.myRevBuf[29];
                            temp.bytParam3 = CsConst.myRevBuf[30];
                            temp.bytParam4 = CsConst.myRevBuf[31];
                            HDLUDP.TimeBetwnNext(1);
                        }
                        else return;
                        #endregion

                        //有效接着读取备注和字符串
                        if (temp.bytType != 0)
                        {
                            #region
                            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xE428, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(intDeviceType)) == true)
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
                                if (intDeviceType == 1006 || intDeviceType == 1007 || intDeviceType == 1008 ||
                                    intDeviceType == 1009 || intDeviceType == 1016 || intDeviceType == 1019 ||
                                    intDeviceType == 1027 || intDeviceType == 1013 || intDeviceType == 1014 ||
                                    intDeviceType == 1018 || intDeviceType == 1033)
                                {
                                    num = 20;
                                    if (intDeviceType == 1018 || intDeviceType == 1027)
                                    {
                                        if (ArayExtra[0] == 0)
                                        {
                                            num = 10;
                                        }
                                        else if (ArayExtra[0] == 1 || ArayExtra[0] == 2)
                                        {
                                            num = 20;
                                        }
                                    }
                                }
                                else
                                {
                                    num = 48;
                                }
                                for (int j = 1; j <= num; j++)
                                {
                                    ArayTmp = new byte[2];
                                    ArayTmp[0] = Convert.ToByte(i);
                                    ArayTmp[1] = Convert.ToByte(j);

                                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xE424, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(intDeviceType)) == true)
                                    {
                                        Rs232Param tmp = new Rs232Param();
                                        tmp.Index = Convert.ToByte(j);
                                        tmp.enable = CsConst.myRevBuf[28];
                                        tmp.type = CsConst.myRevBuf[29];
                                        tmp.rsCmd = "";
                                        //Array.Copy(CsConst.myRevBuf, 30, tmp.RSCMD, 0, tmp.RSCMD.Length);
                                        temp.RS232CMD.Add(tmp);

                                        HDLUDP.TimeBetwnNext(1);
                                    }
                                    else return;
                                }
                            }
                            #endregion
                        }
                        myBUS2RS.Add(temp);
                        if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(20 + 10 * (i - num1) / ((num2 + 1) - num1));
                    }
                }
                #endregion
                MyRead2UpFlags[2] = true;
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(30);
            if (intActivePage == 0 || intActivePage == 4)
            {
                #region
                my4852BUS = new List<Rs232ToBus>();
                if (intDeviceType == 1013 || intDeviceType == 1014 || intDeviceType == 1018 || intDeviceType == 1027)
                {
                    if (CsConst.isRestore)
                    {
                        num1 = 1;
                        if (intDeviceType == 1014 || intDeviceType == 1018 || intDeviceType == 1027)
                        {
                            num2 = 200;
                            if (intDeviceType == 1018 || intDeviceType == 1027)
                            {
                                if (ArayExtra[0] == 0)
                                {
                                    num2 = 99;
                                }
                                else if (ArayExtra[0] == 1)
                                {
                                    num2 = 0;
                                }
                                else if (ArayExtra[0] == 2)
                                {
                                    num2 = 200;
                                }
                            }
                        }
                        else
                            num2 = 99;
                    }
                    for (int i = num1; i <= num2; i++)
                    {
                        Rs232ToBus temp = new Rs232ToBus();
                        temp.rs232Param = new Rs232Param();
                        ArayTmp = new byte[1];
                        ArayTmp[0] = Convert.ToByte(i);
                        if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xDA51, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(intDeviceType)) == true)
                        {
                            temp.ID = Convert.ToByte(i);
                            temp.rs232Param.enable = CsConst.myRevBuf[27];
                            temp.rs232Param.type = CsConst.myRevBuf[28];
                           // temp.rs232Param.rsCmd = new byte[wdMaxValue + 1];
                           // Array.Copy(CsConst.myRevBuf, 29, temp.TmpRS232.RSCMD, 0, temp.TmpRS232.RSCMD.Length);
                            
                            HDLUDP.TimeBetwnNext(1);
                        }
                        else return;

                        if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xDA59, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(intDeviceType)) == true)
                        {
                            byte[] arayRemark = new byte[20];
                            for (int intI = 0; intI < 20; intI++) { arayRemark[intI] = CsConst.myRevBuf[27 + intI]; }
                            temp.remark = HDLPF.Byte2String(arayRemark);
                            
                            HDLUDP.TimeBetwnNext(1);
                        }
                        else return;
                        temp.busTargets = new List<UVCMD.ControlTargets>();
                        if (CsConst.isRestore)
                        {
                            int num = 20;
                            if (intDeviceType == 1006 || intDeviceType == 1007 || intDeviceType == 1008 ||
                                intDeviceType == 1009 || intDeviceType == 1016 || intDeviceType == 1019 ||
                                intDeviceType == 1027 || intDeviceType == 1013 || intDeviceType == 1014 ||
                                intDeviceType == 1018 || intDeviceType == 1033)
                            {
                                num = 20;
                                if (intDeviceType == 1018 || intDeviceType == 1027)
                                {
                                    if (ArayExtra[0] == 0)
                                    {
                                        num = 10;
                                    }
                                    else if (ArayExtra[0] == 1 || ArayExtra[0] == 2)
                                    {
                                        num = 20;
                                    }
                                }
                            }
                            else
                            {
                                num = 48;
                            }
                            for (byte j = 1; j <= num; j++)
                            {
                                byte[] arayTmp = new byte[2];
                                arayTmp[0] = Convert.ToByte(i);
                                arayTmp[1] = j;
                                if (CsConst.mySends.AddBufToSndList(arayTmp, 0xDA55, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(intDeviceType)) == true)
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
                                    temp.busTargets.Add(oCMD);
                                    
                                    System.Threading.Thread.Sleep(1);
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                        my4852BUS.Add(temp);
                        if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(30 + 10 * (i - num1) / ((num2 + 1) - num1));
                    }
                }
                #endregion
                MyRead2UpFlags[3] = true;
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(40);
            if (intActivePage == 0 || intActivePage == 5)
            {
                #region
                myBUS2485 = new List<BUS2RS>();
                if (intDeviceType != 1028 && intDeviceType != 1037 && intDeviceType != 1039 && intDeviceType != 1041 && intDeviceType != 1042)
                {
                    if (CsConst.isRestore)
                    {
                        num1 = 1;
                        if (intDeviceType == 1014 || intDeviceType == 1018 || intDeviceType == 1027)
                        {
                            num2 = 200;
                            if (intDeviceType == 1018 || intDeviceType == 1027)
                            {
                                if (ArayExtra[0] == 0)
                                {
                                    num2 = 99;
                                }
                                else if (ArayExtra[0] == 1)
                                {
                                    num2 = 0;
                                }
                                else if (ArayExtra[0] == 2)
                                {
                                    num2 = 200;
                                }
                                
                            }
                        }
                        else
                            num2 = 99;
                    }
                    for (int i = num1; i <= num2; i++)
                    {
                        BUS2RS temp = new BUS2RS();
                        temp.ID = Convert.ToByte(i);
                        ArayTmp = new byte[1];
                        ArayTmp[0] = Convert.ToByte(i);
                        if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xDA61, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(intDeviceType)) == true)
                        {
                            temp.bytType = CsConst.myRevBuf[27];
                            temp.bytParam1 = CsConst.myRevBuf[28];
                            temp.bytParam2 = CsConst.myRevBuf[29];
                            
                            HDLUDP.TimeBetwnNext(1);
                        }
                        else return;

                        if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xDA69, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(intDeviceType)) == true)
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
                            if (intDeviceType == 1006 || intDeviceType == 1007 || intDeviceType == 1008 ||
                                intDeviceType == 1009 || intDeviceType == 1016 || intDeviceType == 1019 ||
                                intDeviceType == 1027 || intDeviceType == 1013 || intDeviceType == 1014 ||
                                intDeviceType == 1018 || intDeviceType == 1033)
                            {
                                num = 20;
                                if (intDeviceType == 1018 || intDeviceType == 1027)
                                {
                                    if (ArayExtra[0] == 0)
                                    {
                                        num = 10;
                                    }
                                    else if (ArayExtra[0] == 1 || ArayExtra[0] == 2)
                                    {
                                        num = 20;
                                    }
                                }
                            }
                            else
                            {
                                num = 48;
                            }
                            for (int j = 1; j <= num; j++)
                            {
                                ArayTmp = new byte[2];
                                ArayTmp[0] = Convert.ToByte(i);
                                ArayTmp[1] = Convert.ToByte(j);

                                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xDA65, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(intDeviceType)) == true)
                                {
                                    Rs232Param tmp = new Rs232Param();
                                    tmp.Index = Convert.ToByte(j);
                                    tmp.enable = CsConst.myRevBuf[28];
                                    tmp.type = CsConst.myRevBuf[29];
                                    //tmp.rsCmd = new byte[33 + 1];
                                   // Array.Copy(CsConst.myRevBuf, 30, tmp.RSCMD, 0, tmp.RSCMD.Length);
                                    temp.RS232CMD.Add(tmp);
                                    
                                    HDLUDP.TimeBetwnNext(1);
                                }
                                else return;
                            }
                        }
                        myBUS2485.Add(temp);
                        if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(40 + 10 * (i-num1) / ((num2 + 1) - num1));
                    }
                }
                #endregion
                MyRead2UpFlags[4] = true;
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(50);
            if (intActivePage == 0 || intActivePage == 6)
            {
                #region
                myFilter = new List<byte[]>();
                if (intDeviceType != 1027 && intDeviceType != 1039 && intDeviceType!=1041)
                {
                    for (int i = 0; i < 16; i++)
                    {
                        byte[] arayFilter = new byte[4];
                        ArayTmp = new byte[1];
                        ArayTmp[0] = Convert.ToByte(i);
                        if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1CBC, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(intDeviceType)) == true)
                        {
                            Array.Copy(CsConst.myRevBuf, 25, arayFilter, 0, 4);
                            myFilter.Add(arayFilter);
                            
                            HDLUDP.TimeBetwnNext(1);
                        }
                        else return;
                        if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(50 + (i / 2));
                    }
                }
                #endregion
                MyRead2UpFlags[5] = true;
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(60);
            if (intActivePage == 0 || intActivePage == 7)
            {
                #region

                #endregion
                MyRead2UpFlags[6] = true;
            }
            if (intActivePage == 0 || intActivePage == 8)
            {
                #region
                if (intDeviceType == 1034 || intDeviceType == 1041 || intDeviceType == 1004)
                {
                    ArayTmp = new byte[0];
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x310C, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(intDeviceType)) == true)
                    {
                        Time = CsConst.myRevBuf[25];
                        if (intDeviceType == 1041)
                        {
                            if (Time < 50) Time = 50;
                        }
                        else if (intDeviceType == 1004)
                        {
                            if (Time > 50) Time = 50;
                            if (Time < 10) Time = 10;
                        }
                        HDLUDP.TimeBetwnNext(1);
                    }
                    else return;
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(61);
                }

                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x3100, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(intDeviceType)) == true)
                {
                    Count = CsConst.myRevBuf[25];

                    HDLUDP.TimeBetwnNext(1);
                }
                else return;

                myAC = new List<byte[]>();
                int iCommand = 0x3104;
                if (intDeviceType == 1004) iCommand = 0x3130;
                for (int i = 0; i < Count; i++)
                {
                    ArayTmp = new byte[1];
                    ArayTmp[0] = Convert.ToByte(i + 1);
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, iCommand, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(intDeviceType)) == true)
                    {
                        byte[] arayAC = new byte[CsConst.myRevBuf[16] - 11];
                        Array.Copy(CsConst.myRevBuf, 25, arayAC, 0, arayAC.Length);
                        myAC.Add(arayAC);

                        HDLUDP.TimeBetwnNext(1);
                    }
                    else return;
                }
                #endregion
                MyRead2UpFlags[7] = true;
            }

            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(70);
            if (intActivePage == 0 || intActivePage == 9)
            {
                #region
                myCurTain = new List<byte[]>();
                if (intDeviceType == 1037|| intDeviceType == 1042)
                {
                    if (CsConst.isRestore)
                    {
                        num1 = 1;
                        num2 = 50;
                    }
                    for (int i = num1; i <= num2; i++)
                    {
                        ArayTmp = new byte[1];
                        ArayTmp[0] = Convert.ToByte(i);
                        if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1F54, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(intDeviceType)) == true)
                        {
                            byte[] arayCurtain = new byte[3];
                            Array.Copy(CsConst.myRevBuf, 25, arayCurtain, 0, 3);
                            myCurTain.Add(arayCurtain);
                        }
                        else return;
                        if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(70 + 10 * (i - num1) / (num2 - num1 + 1));
                    }
                }
                #endregion
                MyRead2UpFlags[8] = true;
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(70);
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(100);
        }

    }

}
