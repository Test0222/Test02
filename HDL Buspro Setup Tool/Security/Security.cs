using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.Xml;
using System.IO;
using System.Windows.Forms;
using System.Linq;

namespace HDL_Buspro_Setup_Tool
{
    [Serializable]
    public class Security : HdlDeviceBackupAndRestore
    {
        public int DIndex;  ////设备唯一编号
        public string DeviceName;

        public ReadyThings[] myReadyThings8 = new ReadyThings[8]; //八个区域的准备信息
        public ArmMode[] myArms8; // 八个区域的arm命令
        public AlarmModes[] myAlarms8; // 八个区域的报警命令
        public Vacations[] myVacs8;    //八个区域的假期命令
        [Serializable]
        public class ReadyThings   //准备工作
        {
            public byte bytEnable; // 是不是启用当前区域
            public byte bytExitDelay; // 出门延迟
            public byte bytMore;  // 是不是设置多人离开模式
            public byte[] DetectDoors = new byte[320];  //每个给定20个byte 固定 以便于后面增加 有一个位置表示总数
            public byte[] ArmModules = new byte[50]; // 用于保存布防的模块
            public List<UVCMD.ControlTargets>[] DoorWarns = new List<UVCMD.ControlTargets>[16]; //门窗没关好提醒  0
            public List<UVCMD.ControlTargets>[] LeaveCmds = new List<UVCMD.ControlTargets>[8]; //出门提示音 倒计时开始 最后五秒提醒 倒计时结束   1      
        }
        [Serializable]
        public class ArmMode        //布防模式
        {
            public byte[] bytFlag = new byte[4]; //是不是出门同步触发
            public List<UVCMD.ControlTargets>[] ArmCmds = new List<UVCMD.ControlTargets>[6]; //布防目标 
            public byte[] AutoArmDisarm = new byte[60];  // 固定给30个bytes存储自动布防   30个自动撤防
        }
        [Serializable]
        public class AlarmModes     // 报警设置
        {
            public byte bytEnterDelay; // 进门延迟   
            public byte bytReset; //重启延迟
            public byte bytSirnDelay; // 蜂鸣器延迟  //0表示不启用
            public List<UVCMD.ControlTargets>[] AlarmCmds = new List<UVCMD.ControlTargets>[23]; //报警设置目标 
        }
        [Serializable]
        public class Vacations      //假期报警
        {
            public byte[] SetupTime = new byte[80];// 用于存储时间设置
            public List<UVCMD.ControlTargets>[] VacationCmds = new List<UVCMD.ControlTargets>[8]; //假期设置目标 
        }

        public bool[] MyRead2UpFlags = new bool[12]; //前面四个表示读取 后面表示上传 
        //<summary>
        //读取默认的MS04设置，将所有数据读取缓存
        //</summary>
        public void ReadDefaultInfo(int intDeviceType)
        {
            myReadyThings8 = new ReadyThings[8]; //八个区域的准备信息
            myArms8 = new ArmMode[8]; // 八个区域的arm命令
            myAlarms8 = new AlarmModes[8]; // 八个区域的报警命令
            myVacs8 = new Vacations[8];    //八个区域的假期命令
        }

        //<summary>
        //读取数据库面板设置，将所有数据读至缓存
        //</summary>
        public void ReadSecurityFrmDBTobuf(int DIndex)
        {
            if (myReadyThings8 == null) myReadyThings8 = new ReadyThings[8]; //八个区域的准备信息
            if (myArms8 == null) myArms8 = new ArmMode[8]; // 八个区域的arm命令
            if (myAlarms8 == null) myAlarms8 = new AlarmModes[8]; // 八个区域的报警命令
            if (myVacs8 ==null) myVacs8 = new Vacations[8];    //八个区域的假期命令

            for (int intI = 1; intI <= 8; intI++)
            {
                //读取基本信息
                #region
                string str = string.Format("select * from dbSecurity where DIndex={0} and AreaID = {1}", DIndex,intI);
                OleDbDataReader dr = DataModule.SearchAResultSQLDB(str);
                if (dr != null)
                {
                    while (dr.Read())
                    {
                        myReadyThings8[intI - 1].bytExitDelay = dr.GetByte(2);
                        myReadyThings8[intI - 1].bytMore = dr.GetByte(3);
                        myReadyThings8[intI - 1].DetectDoors =(byte[])dr[4];
                        myReadyThings8[intI - 1].ArmModules = (byte[])dr[5];
                        myReadyThings8[intI - 1].bytEnable = dr.GetByte(11); //新增使能位

                        myArms8[intI - 1].bytFlag = (byte[])dr[6];
                        myArms8[intI - 1].AutoArmDisarm = (byte[])dr[7];

                        myAlarms8[intI - 1].bytEnterDelay = dr.GetByte(8);
                        myAlarms8[intI - 1].bytReset = dr.GetByte(9);
                        myAlarms8[intI - 1].bytSirnDelay = dr.GetByte(12); //新增蜂鸣器使能位

                        myVacs8[intI - 1].SetupTime = (byte[])dr[10];
                    }
                }
                #endregion

                //读取目标设置 
                //读取门窗提醒   +   出门提示音相关提醒
                #region
                myReadyThings8[intI - 1].DoorWarns = new List<UVCMD.ControlTargets>[16];
                myReadyThings8[intI - 1].LeaveCmds = new List<UVCMD.ControlTargets>[8];
                str = string.Format("select * from dbKeyTargets where DIndex ={0} and KeyIndex = {1} and Ms04State <= 16 order by objID", DIndex, intI);
                OleDbDataReader drKeyCmds = DataModule.SearchAResultSQLDB(str);
                if (drKeyCmds != null)
                {
                    while (drKeyCmds.Read())
                    {
                        int intIndex = drKeyCmds.GetInt16(10) - 1; 
                        UVCMD.ControlTargets TmpCmd = new UVCMD.ControlTargets();
                        TmpCmd.ID = drKeyCmds.GetByte(2);
                        TmpCmd.Type = drKeyCmds.GetByte(3);
                        TmpCmd.SubnetID = drKeyCmds.GetByte(4);
                        TmpCmd.DeviceID = drKeyCmds.GetByte(5);
                        TmpCmd.Param1 = drKeyCmds.GetByte(6);
                        TmpCmd.Param2 = drKeyCmds.GetByte(7);
                        TmpCmd.Param3 = drKeyCmds.GetByte(8);
                        TmpCmd.Param4 = drKeyCmds.GetByte(9);

                        if (myReadyThings8[intI - 1].DoorWarns[intIndex] == null) myReadyThings8[intI - 1].DoorWarns[intIndex] = new List<UVCMD.ControlTargets>();
                        myReadyThings8[intI - 1].DoorWarns[intIndex].Add((UVCMD.ControlTargets)TmpCmd);
                    }
                    drKeyCmds.Close();
                }

                str = string.Format("select * from dbKeyTargets where DIndex ={0} and KeyIndex = {1} and  Ms04State >= 17 and Ms04State <= 20 order by objID", DIndex, dr.GetInt16(1));
                drKeyCmds = DataModule.SearchAResultSQLDB(str);
                if (drKeyCmds != null)
                {
                    while (drKeyCmds.Read())
                    {
                        int intIndex = drKeyCmds.GetInt16(10) - 17; 
                        UVCMD.ControlTargets TmpCmd = new UVCMD.ControlTargets();
                        TmpCmd.ID = drKeyCmds.GetByte(2);
                        TmpCmd.Type = drKeyCmds.GetByte(3);
                        TmpCmd.SubnetID = drKeyCmds.GetByte(4);
                        TmpCmd.DeviceID = drKeyCmds.GetByte(5);
                        TmpCmd.Param1 = drKeyCmds.GetByte(6);
                        TmpCmd.Param2 = drKeyCmds.GetByte(7);
                        TmpCmd.Param3 = drKeyCmds.GetByte(8);
                        TmpCmd.Param4 = drKeyCmds.GetByte(9);

                        if (myReadyThings8[intI - 1].DoorWarns[intIndex] == null) myReadyThings8[intI - 1].DoorWarns[intIndex] = new List<UVCMD.ControlTargets>();
                        myReadyThings8[intI - 1].DoorWarns[intIndex].Add((UVCMD.ControlTargets)TmpCmd);
                    }
                    drKeyCmds.Close();
                }
            #endregion

                //读取布防设置目标
                #region
                myArms8[intI - 1].ArmCmds = new List<UVCMD.ControlTargets>[6];
                str = string.Format("select * from dbKeyTargets where DIndex ={0} and KeyIndex = {1} and Ms04State >= 21 and Ms04State <= 26 order by objID", DIndex, intI);
                drKeyCmds = DataModule.SearchAResultSQLDB(str);
                if (drKeyCmds != null)
                {
                    while (drKeyCmds.Read())
                    {
                        int intIndex = drKeyCmds.GetInt16(10) - 21;
                        UVCMD.ControlTargets TmpCmd = new UVCMD.ControlTargets();
                        TmpCmd.ID = drKeyCmds.GetByte(2);
                        TmpCmd.Type = drKeyCmds.GetByte(3);
                        TmpCmd.SubnetID = drKeyCmds.GetByte(4);
                        TmpCmd.DeviceID = drKeyCmds.GetByte(5);
                        TmpCmd.Param1 = drKeyCmds.GetByte(6);
                        TmpCmd.Param2 = drKeyCmds.GetByte(7);
                        TmpCmd.Param3 = drKeyCmds.GetByte(8);
                        TmpCmd.Param4 = drKeyCmds.GetByte(9);

                        if (myArms8[intI - 1].ArmCmds[intIndex] == null) myArms8[intI - 1].ArmCmds[intIndex] = new List<UVCMD.ControlTargets>();
                        myArms8[intI - 1].ArmCmds[intIndex].Add(TmpCmd);
                    }
                    drKeyCmds.Close();
                }
                #endregion

                //读取报警目标
                #region
                myAlarms8[intI - 1].AlarmCmds = new List<UVCMD.ControlTargets>[23];
                str = string.Format("select * from dbKeyTargets where DIndex ={0} and KeyIndex = {1} and Ms04State >= 27 and Ms04State <= 49 order by objID", DIndex, intI);
                drKeyCmds = DataModule.SearchAResultSQLDB(str);
                if (drKeyCmds != null)
                {
                    while (drKeyCmds.Read())
                    {
                        int intIndex = drKeyCmds.GetInt16(10) - 27;
                        UVCMD.ControlTargets TmpCmd = new UVCMD.ControlTargets();
                        TmpCmd.ID = drKeyCmds.GetByte(2);
                        TmpCmd.Type = drKeyCmds.GetByte(3);
                        TmpCmd.SubnetID = drKeyCmds.GetByte(4);
                        TmpCmd.DeviceID = drKeyCmds.GetByte(5);
                        TmpCmd.Param1 = drKeyCmds.GetByte(6);
                        TmpCmd.Param2 = drKeyCmds.GetByte(7);
                        TmpCmd.Param3 = drKeyCmds.GetByte(8);
                        TmpCmd.Param4 = drKeyCmds.GetByte(9);

                        if (myAlarms8[intI - 1].AlarmCmds[intIndex] == null) myAlarms8[intI - 1].AlarmCmds[intIndex] = new List<UVCMD.ControlTargets>();
                        myAlarms8[intI - 1].AlarmCmds[intIndex].Add(TmpCmd);
                    }
                    drKeyCmds.Close();
                }
                #endregion

                //读取假期特殊处理
                #region
                myVacs8[intI - 1].VacationCmds = new List<UVCMD.ControlTargets>[8];
                str = string.Format("select * from dbKeyTargets where DIndex ={0} and KeyIndex = {1} and Ms04State >= 50 order by objID", DIndex, intI);
                drKeyCmds = DataModule.SearchAResultSQLDB(str);
                if (drKeyCmds != null)
                {
                    while (drKeyCmds.Read())
                    {
                        int intIndex = drKeyCmds.GetInt16(10) - 27;
                        UVCMD.ControlTargets TmpCmd = new UVCMD.ControlTargets();
                        TmpCmd.ID = drKeyCmds.GetByte(2);
                        TmpCmd.Type = drKeyCmds.GetByte(3);
                        TmpCmd.SubnetID = drKeyCmds.GetByte(4);
                        TmpCmd.DeviceID = drKeyCmds.GetByte(5);
                        TmpCmd.Param1 = drKeyCmds.GetByte(6);
                        TmpCmd.Param2 = drKeyCmds.GetByte(7);
                        TmpCmd.Param3 = drKeyCmds.GetByte(8);
                        TmpCmd.Param4 = drKeyCmds.GetByte(9);

                        if (myVacs8[intI - 1].VacationCmds[intIndex] == null) myVacs8[intI - 1].VacationCmds[intIndex] = new List<UVCMD.ControlTargets>();
                        myVacs8[intI - 1].VacationCmds[intIndex].Add(TmpCmd);
                    }
                    drKeyCmds.Close();
                }
                #endregion
            }
        }

        //<summary>
        //保存数据库面板设置，将所有数据保存
        //</summary>
        public void SaveSecurityToDB()
        {
            //// delete all old information and refresh the database
            string strsql = string.Format("delete * from dbSecurity where DIndex={0}", DIndex);
            DataModule.ExecuteSQLDatabase(strsql);

            strsql = string.Format("delete * from dbKeyTargets where DIndex={0}", DIndex);
            DataModule.ExecuteSQLDatabase(strsql);

            for (int intI = 1; intI <= 8; intI++)
            {
                //保存基本信息
                #region
                strsql = @"Insert into dbSecurity(DIndex,AreaID,ExitDelay,MorePeople,DetectDoor,ArmModules,bytFlag,AutoArmDisarm,EnterDelay,Reset,SetupTime,Enable2Not,SirnDelay)"
                              + "values(@DIndex,@AreaID,@ExitDelay,@MorePeople,@DetectDoor,@ArmModules,@bytFlag,@AutoArmDisarm,@EnterDelay,@Reset,@SetupTime,@Enable2Not,@SirnDelay)";
                //创建一个OleDbConnection对象
                OleDbConnection conn;
                conn = new OleDbConnection(DataModule.ConString + CsConst.mstrDefaultPath);
                conn.Open();
                OleDbCommand cmd = new OleDbCommand(strsql, conn);
                ((OleDbParameter)cmd.Parameters.Add("@DIndex", OleDbType.Integer)).Value = DIndex;
                ((OleDbParameter)cmd.Parameters.Add("@AreaID", OleDbType.VarChar)).Value = intI;
                ((OleDbParameter)cmd.Parameters.Add("@ExitDelay", OleDbType.VarChar)).Value = myReadyThings8[intI - 1].bytExitDelay;
                ((OleDbParameter)cmd.Parameters.Add("@MorePeople", OleDbType.VarChar)).Value = myReadyThings8[intI - 1].bytMore;
                ((OleDbParameter)cmd.Parameters.Add("@DetectDoor", OleDbType.Binary)).Value = myReadyThings8[intI - 1].DetectDoors;

                ((OleDbParameter)cmd.Parameters.Add("@ArmModules", OleDbType.Binary)).Value = myReadyThings8[intI - 1].ArmModules;
                ((OleDbParameter)cmd.Parameters.Add("@bytFlag", OleDbType.Binary)).Value = myArms8[intI - 1].bytFlag;
                ((OleDbParameter)cmd.Parameters.Add("@AutoArmDisarm", OleDbType.Binary)).Value = myArms8[intI - 1].AutoArmDisarm;
                ((OleDbParameter)cmd.Parameters.Add("@EnterDelay", OleDbType.VarChar)).Value = myAlarms8[intI - 1].bytEnterDelay;
                ((OleDbParameter)cmd.Parameters.Add("@Reset", OleDbType.VarChar)).Value = myAlarms8[intI - 1].bytReset;
                ((OleDbParameter)cmd.Parameters.Add("@SetupTime", OleDbType.Binary)).Value = myVacs8[intI - 1].SetupTime;
                ((OleDbParameter)cmd.Parameters.Add("@Enable2Not", OleDbType.VarChar)).Value = myReadyThings8[intI - 1].bytEnable;
                ((OleDbParameter)cmd.Parameters.Add("@SirnDelay", OleDbType.VarChar)).Value = myAlarms8[intI - 1].bytSirnDelay;
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (OleDbException exp)
                {
                    MessageBox.Show(exp.ToString());
                    conn.Close();
                }
                conn.Close();
                #endregion

                //保存目标设置  //保存门窗提醒   +   出门提示音相关提醒
                #region
                if (myReadyThings8[intI - 1].DoorWarns != null)
                {
                    for (int intJ = 0; intJ < 16; intJ++)
                    {
                        if (myReadyThings8[intI - 1].DoorWarns[intJ] != null && myReadyThings8[intI - 1].DoorWarns[intJ].Count != 0)
                        {
                            foreach (UVCMD.ControlTargets TmpCmds in myReadyThings8[intI - 1].DoorWarns[intJ])
                            {
                                ///// insert into all commands to database
                                strsql = string.Format("Insert into dbKeyTargets(DIndex,KeyIndex,objID,KeyFunType,SubNetID,DeviceID,FirstParameter,"
                                               + "SecondParameter,RunTimeMinute,RunTimeSecond,Ms04State) values ({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10})",
                                               DIndex, intI, TmpCmds.ID, TmpCmds.Type, TmpCmds.SubnetID, TmpCmds.DeviceID, TmpCmds.Param1,
                                               TmpCmds.Param2, TmpCmds.Param3, TmpCmds.Param4, intJ + 1);
                                DataModule.ExecuteSQLDatabase(strsql);
                            }
                        }
                    }
                }
                #endregion

                #region
                if (myReadyThings8[intI - 1].LeaveCmds != null)
                {
                    for (int intJ = 0; intJ < 4; intJ++)
                    {
                        if (myReadyThings8[intI - 1].LeaveCmds[intJ] != null && myReadyThings8[intI - 1].LeaveCmds[intJ].Count != 0)
                        {
                            foreach (UVCMD.ControlTargets TmpCmds in myReadyThings8[intI - 1].LeaveCmds[intJ])
                            {
                                ///// insert into all commands to database
                                strsql = string.Format("Insert into dbKeyTargets(DIndex,KeyIndex,objID,KeyFunType,SubNetID,DeviceID,FirstParameter,"
                                               + "SecondParameter,RunTimeMinute,RunTimeSecond,Ms04State) values ({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10})",
                                               DIndex, intI, TmpCmds.ID, TmpCmds.Type, TmpCmds.SubnetID, TmpCmds.DeviceID, TmpCmds.Param1,
                                               TmpCmds.Param2, TmpCmds.Param3, TmpCmds.Param4, intJ + 17);
                                DataModule.ExecuteSQLDatabase(strsql);
                            }
                        }
                    }
                }
                #endregion

                ////保存布防设置目标
                #region
                if (myArms8[intI - 1].ArmCmds != null)
                {
                    for (int intJ = 0; intJ < 6; intJ++)
                    {
                        if (myArms8[intI - 1].ArmCmds[intJ] != null && myArms8[intI - 1].ArmCmds[intJ].Count != 0)
                        {
                            foreach (UVCMD.ControlTargets TmpCmds in myArms8[intI - 1].ArmCmds[intJ])
                            {
                                ///// insert into all commands to database
                                strsql = string.Format("Insert into dbKeyTargets(DIndex,KeyIndex,objID,KeyFunType,SubNetID,DeviceID,FirstParameter,"
                                               + "SecondParameter,RunTimeMinute,RunTimeSecond,Ms04State) values ({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10})",
                                               DIndex, intI, TmpCmds.ID, TmpCmds.Type, TmpCmds.SubnetID, TmpCmds.DeviceID, TmpCmds.Param1,
                                               TmpCmds.Param2, TmpCmds.Param3, TmpCmds.Param4, intJ + 21);
                                DataModule.ExecuteSQLDatabase(strsql);
                            }
                        }
                    }
                }
                #endregion

                ////保存报警设置目标
                #region
                if (myAlarms8[intI - 1].AlarmCmds != null)
                {
                    for (int intJ = 0; intJ < 6; intJ++)
                    {
                        if (myAlarms8[intI - 1].AlarmCmds[intJ] != null && myAlarms8[intI - 1].AlarmCmds[intJ].Count != 0)
                        {
                            foreach (UVCMD.ControlTargets TmpCmds in myAlarms8[intI - 1].AlarmCmds[intJ])
                            {
                                ///// insert into all commands to database
                                strsql = string.Format("Insert into dbKeyTargets(DIndex,KeyIndex,objID,KeyFunType,SubNetID,DeviceID,FirstParameter,"
                                               + "SecondParameter,RunTimeMinute,RunTimeSecond,Ms04State) values ({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10})",
                                               DIndex, intI, TmpCmds.ID, TmpCmds.Type, TmpCmds.SubnetID, TmpCmds.DeviceID, TmpCmds.Param1,
                                               TmpCmds.Param2, TmpCmds.Param3, TmpCmds.Param4, intJ + 27);
                                DataModule.ExecuteSQLDatabase(strsql);
                            }
                        }
                    }
                }
                #endregion

                ////保存假期设置目标
                #region
                if (myVacs8[intI - 1].VacationCmds != null)
                {
                    for (int intJ = 0; intJ < 6; intJ++)
                    {
                        if (myVacs8[intI - 1].VacationCmds[intJ] != null && myVacs8[intI - 1].VacationCmds[intJ].Count != 0)
                        {
                            foreach (UVCMD.ControlTargets TmpCmds in myVacs8[intI - 1].VacationCmds[intJ])
                            {
                                ///// insert into all commands to database
                                strsql = string.Format("Insert into dbKeyTargets(DIndex,KeyIndex,objID,KeyFunType,SubNetID,DeviceID,FirstParameter,"
                                               + "SecondParameter,RunTimeMinute,RunTimeSecond,Ms04State) values ({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10})",
                                               DIndex, intI, TmpCmds.ID, TmpCmds.Type, TmpCmds.SubnetID, TmpCmds.DeviceID, TmpCmds.Param1,
                                               TmpCmds.Param2, TmpCmds.Param3, TmpCmds.Param4, intJ + 50);
                                DataModule.ExecuteSQLDatabase(strsql);
                            }
                        }
                    }
                }
                #endregion
            }
        }

        public void UploadSecurityToDevice(String strDevName, int intDeviceType, int intActivePage)// 0 mean all, else that tab only
        {
            string strMainRemark = "";
            string[] StrTmp = strDevName.Split('\\');
            if (StrTmp.Length ==2) 
            strMainRemark =  StrTmp[1].Trim().Split('(')[0].Trim(); // strDevName.Split('\\')[1].Trim().Split('(')[0].Trim();
            strDevName = StrTmp[0].Trim();
            byte bytSubID = byte.Parse(strDevName.Split('-')[0].ToString());
            byte bytDevID = byte.Parse(strDevName.Split('-')[1].ToString());

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
            if (CsConst.mySends.AddBufToSndList(ArayMain, 0x0010, bytSubID, bytDevID, false, true, true,CsConst.minAllWirelessDeviceType.Contains(intDeviceType)) == false)
            {
                return;
            }
            //循环上传八个区域的信息
            byte[] ArayDelay = new byte[7]; //用于存放延迟
            for (byte intI = 1; intI <= 1; intI++)
            {
                //上传基本信息
                ArayDelay[0] = intI;
                ArayDelay[4] = myReadyThings8[intI - 1].bytExitDelay; //基本延迟信息
                ArayDelay[6] = myReadyThings8[intI - 1].bytMore;

                //修改目标设置    出门提示音相关提醒
                #region
                if (myReadyThings8[intI - 1] != null)
                {
                    byte[] ArayTmp = new byte[22];
                    if (intActivePage == 0 || intActivePage == 1)
                    {
                        ArayTmp[0] = intI;
                        ArayTmp[1] = myReadyThings8[intI - 1].bytEnable; //基本信息 使能位和备注

                        if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x024A, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(intDeviceType)) == false)
                        {
                            return;
                        }
                        //修改目标设置    出门提示音相关提醒
                        #region
                        if (myReadyThings8[intI - 1].LeaveCmds.Length != 0)
                        {
                            ArayTmp = new byte[10];
                            ArayTmp[0] = intI;
                            byte bytTmpID = 0;
                            int command = 0x024E;
                            foreach (List<UVCMD.ControlTargets> oTmp in myReadyThings8[intI - 1].LeaveCmds)
                            {
                                if (oTmp != null && oTmp.Count != 0)
                                {
                                    if (bytTmpID == 0)
                                    {
                                        ArayTmp[1] = 0;
                                        command = 0x1306;
                                    }
                                    else
                                    {
                                        ArayTmp[1] = (byte)(bytTmpID + 2);
                                        command = 0x024E;
                                    }
                                    ArayTmp[1] = bytTmpID;
                                    foreach (UVCMD.ControlTargets TmpCmd in oTmp)
                                    {
                                        byte[] arayCMD = new byte[9];
                                        ArayTmp[2] = byte.Parse(TmpCmd.ID.ToString());
                                        ArayTmp[3] = TmpCmd.Type;
                                        ArayTmp[4] = TmpCmd.SubnetID;
                                        ArayTmp[5] = TmpCmd.DeviceID;
                                        ArayTmp[6] = TmpCmd.Param1;
                                        ArayTmp[7] = TmpCmd.Param2;
                                        ArayTmp[8] = TmpCmd.Param3;   // save targets
                                        ArayTmp[9] = TmpCmd.Param4;
                                        if (CsConst.mySends.AddBufToSndList(ArayTmp, command, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(intDeviceType)) == false)
                                        { return; }
                                        HDLUDP.TimeBetwnNext(arayCMD.Length);
                                    }
                                }
                                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(bytTmpID * 5);
                                bytTmpID++;
                            }
                        }
                        #endregion

                        //布防的设备列表
                        #region
                        if (myReadyThings8[intI - 1].DetectDoors.Length != 0 && myReadyThings8[intI - 1].DetectDoors[0] != 0)
                        {
                            ArayTmp = new byte[24];
                            ArayTmp[0] = intI;
                            for (byte bytJ = 1; bytJ <= myReadyThings8[intI - 1].DetectDoors[0]; bytJ++)
                            {
                                ArayTmp[1] = bytJ;
                                ArayTmp[2] = myReadyThings8[intI - 1].DetectDoors[1 + (bytJ - 1) * 2];
                                ArayTmp[3] = myReadyThings8[intI - 1].DetectDoors[2 + (bytJ - 1) * 2];
                                arayTmpRemark = HDLUDP.StringToByte(strMainRemark);
                                if (arayTmpRemark.Length > 20)
                                {
                                    Array.Copy(arayTmpRemark, 0, ArayTmp, 4, 20);
                                }
                                else
                                {
                                    Array.Copy(arayTmpRemark, 0, ArayTmp, 4, arayTmpRemark.Length);
                                }
                                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x15D3, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(intDeviceType)) == false)
                                {
                                    return;
                                }
                                HDLUDP.TimeBetwnNext(ArayTmp.Length);
                                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(20 + bytJ * 10);
                            }
                        }
                        #endregion

                        //检测的门窗列表和报警设置
                        #region
                        if (myReadyThings8[intI - 1].DetectDoors.Length != 0 && myReadyThings8[intI - 1].DetectDoors[50] != 0)
                        {
                            ArayTmp = new byte[12];
                            ArayTmp[0] = intI;
                            for (byte bytJ = 1; bytJ <= myReadyThings8[intI - 1].DetectDoors[50]; bytJ++)
                            {
                                ArayTmp[1] = bytJ;
                                Array.Copy(myReadyThings8[intI - 1].DetectDoors, 51 + (bytJ - 1) * 10, ArayTmp, 2, 10);
                                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x0130, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(intDeviceType)) == false)
                                {
                                    return;
                                }
                                HDLUDP.TimeBetwnNext(ArayTmp.Length);
                                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(20 + bytJ * 3);

                                if (myReadyThings8[intI - 1].DoorWarns != null && myReadyThings8[intI - 1].DoorWarns.Length != 0)
                                {
                                    if (myReadyThings8[intI - 1].DoorWarns[bytJ] != null && myReadyThings8[intI - 1].DoorWarns[bytJ].Count != 0)
                                    {
                                        ArayTmp = new byte[10];
                                        ArayTmp[0] = intI;
                                        ArayTmp[1] = bytJ;

                                        foreach (UVCMD.ControlTargets TmpCmd in myReadyThings8[intI - 1].DoorWarns[bytJ])
                                        {
                                            byte[] arayCMD = new byte[9];
                                            ArayTmp[2] = byte.Parse(TmpCmd.ID.ToString());
                                            ArayTmp[3] = TmpCmd.Type;
                                            ArayTmp[4] = TmpCmd.SubnetID;
                                            ArayTmp[5] = TmpCmd.DeviceID;
                                            ArayTmp[6] = TmpCmd.Param1;
                                            ArayTmp[7] = TmpCmd.Param2;
                                            ArayTmp[8] = TmpCmd.Param3;   // save targets
                                            ArayTmp[9] = TmpCmd.Param4;
                                            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1302, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(intDeviceType)) == false) return;
                                            HDLUDP.TimeBetwnNext(arayCMD.Length);
                                        }
                                    }
                                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(20 + bytJ * 5);
                                }
                            }
                        }
                        #endregion
                    }
                }
                #endregion
                MyRead2UpFlags[6] = true;
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(20);

                //arm 命令上传
                if (intActivePage == 0 || intActivePage == 2)
                {
                    #region
                    if (myArms8[intI - 1].ArmCmds != null && myArms8[intI - 1].ArmCmds.Length != 0)
                    {
                        for (byte bytJ = 1; bytJ <= myArms8[intI - 1].ArmCmds.Length; bytJ++)
                        {
                            if (myArms8[intI - 1].ArmCmds[bytJ -1] != null && myArms8[intI - 1].ArmCmds[bytJ -1].Count != 0)
                            {
                                byte[] ArayTmp = new byte[3];
                                ArayTmp[0] = intI;
                                ArayTmp[1] = bytJ;
                                ArayTmp[2] = myArms8[intI - 1].bytFlag[bytJ - 1];

                                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x0266, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(intDeviceType)) == false)
                                { return; }
                                HDLUDP.TimeBetwnNext(ArayTmp.Length);

                                ArayTmp = new byte[10];
                                ArayTmp[0] = intI;
                                ArayTmp[1] = bytJ;
                                foreach (UVCMD.ControlTargets TmpCmd in myReadyThings8[intI - 1].DoorWarns[bytJ -1])
                                {
                                    byte[] arayCMD = new byte[9];
                                    ArayTmp[2] = byte.Parse(TmpCmd.ID.ToString());
                                    ArayTmp[3] = TmpCmd.Type;
                                    ArayTmp[4] = TmpCmd.SubnetID;
                                    ArayTmp[5] = TmpCmd.DeviceID;
                                    ArayTmp[6] = TmpCmd.Param1;
                                    ArayTmp[7] = TmpCmd.Param2;
                                    ArayTmp[8] = TmpCmd.Param3;   // save targets
                                    ArayTmp[9] = TmpCmd.Param4;
                                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x026A, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(intDeviceType)) == false)
                                    { return; }
                                    HDLUDP.TimeBetwnNext(ArayTmp.Length);
                                }
                            }
                            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(40 + bytJ * 5);
                        }
                    }
                    #endregion
                    MyRead2UpFlags[7] = true;
                }
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(40);
                //自动布防撤防
                if (intActivePage == 0 || intActivePage == 3)
                {
                    #region
                    if (myArms8[intI - 1].AutoArmDisarm != null)
                    {
                        if (myArms8[intI - 1].AutoArmDisarm[0] != 0) //自动布防
                        {
                            byte[] ArayTmp = new byte[6];
                            ArayTmp[0] = intI;
                            for (byte bytJ = 1; bytJ <= myArms8[intI - 1].AutoArmDisarm[0]; bytJ++)
                            {
                                ArayTmp[1] = bytJ;
                                Array.Copy(myArms8[intI - 1].AutoArmDisarm, 1 + (bytJ - 1) * 4, ArayTmp, 2, 4);

                                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x0110, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(intDeviceType)) == false)
                                {
                                    return;
                                }
                                HDLUDP.TimeBetwnNext(ArayTmp.Length);
                            }
                        }

                        if (myArms8[intI - 1].AutoArmDisarm[20] != 0) //自动撤防
                        {
                            byte[] ArayTmp = new byte[6];
                            ArayTmp[0] = intI;
                            for (byte bytJ = 1; bytJ <= myArms8[intI - 1].AutoArmDisarm[20]; bytJ++)
                            {
                                ArayTmp[1] = (byte)(bytJ + 3);
                                Array.Copy(myArms8[intI - 1].AutoArmDisarm, 21 + (bytJ - 1) * 4, ArayTmp, 2, 4);

                                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x0110, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(intDeviceType)) == false)
                                {
                                    return;
                                }
                                HDLUDP.TimeBetwnNext(ArayTmp.Length);
                            }
                        }
                    }
                    #endregion
                    MyRead2UpFlags[8] = true;
                }
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(60);

                //报警设置上传
                if (intActivePage == 0 || intActivePage == 4)
                {
                    #region
                    if (myAlarms8[intI - 1] != null)
                    {
                        if (myAlarms8[intI - 1].bytSirnDelay >= 10)
                        {
                            ArayDelay[1] = 1;
                            ArayDelay[2] = myAlarms8[intI - 1].bytSirnDelay;
                        }
                        ArayDelay[3] = myAlarms8[intI - 1].bytEnterDelay;
                        ArayDelay[5] = myAlarms8[intI - 1].bytReset;

                        if (intActivePage == 0 || intActivePage == 3)
                        {
                            if (myAlarms8[intI - 1].AlarmCmds != null && myAlarms8[intI - 1].AlarmCmds.Length != 0)
                            {
                                for (byte bytJ = 1; bytJ <= myAlarms8[intI - 1].AlarmCmds.Length; bytJ++)
                                {
                                    if (myAlarms8[intI - 1].AlarmCmds[bytJ -1] != null && myAlarms8[intI - 1].AlarmCmds[bytJ -1].Count != 0)
                                    {
                                        byte[] ArayTmp = new byte[10];
                                        ArayTmp[0] = intI;
                                        int command = 0;
                                        int intTmp = 0;
                                        if (bytJ == 1)
                                        {
                                            command = 0x0262;
                                        }
                                        else if (bytJ <= 9)
                                        {
                                            command = 0x024E;
                                            intTmp = 1;
                                            ArayTmp[1] = (byte)(bytJ + 5);
                                        }
                                        else
                                        {
                                            command = 0x026e;
                                        }
                                        foreach (UVCMD.ControlTargets TmpCmd in myAlarms8[intI - 1].AlarmCmds[bytJ -1])
                                        {
                                            byte[] arayCMD = new byte[9];
                                            ArayTmp[1 + intTmp] = byte.Parse(TmpCmd.ID.ToString());
                                            ArayTmp[2 + intTmp] = TmpCmd.Type;
                                            ArayTmp[3 + intTmp] = TmpCmd.SubnetID;
                                            ArayTmp[4 + intTmp] = TmpCmd.DeviceID;
                                            ArayTmp[5 + intTmp] = TmpCmd.Param1;
                                            ArayTmp[6 + intTmp] = TmpCmd.Param2;
                                            ArayTmp[7 + intTmp] = TmpCmd.Param3;   // save targets
                                            ArayTmp[8 + intTmp] = TmpCmd.Param4;
                                            if (CsConst.mySends.AddBufToSndList(ArayTmp, command, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(intDeviceType)) == false) return;
                                            HDLUDP.TimeBetwnNext(arayCMD.Length);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    #endregion
                    MyRead2UpFlags[9] = true;
                }
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(80);

                //修改所有延迟信息
                if (CsConst.mySends.AddBufToSndList(ArayDelay, 0x0252, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(intDeviceType)) == false)
                {
                    return;
                }
                MyRead2UpFlags[10] = true;
                //假期设置
                if (intActivePage == 0 || intActivePage == 5)
                {
                    #region
                    if (myVacs8[intI - 1].SetupTime != null && myVacs8[intI - 1].SetupTime[0] != 0)
                    {
                        byte[] ArayTmp = new byte[9];
                        ArayTmp[0] = intI;
                        for (byte bytI = 1; bytI <= myVacs8[intI - 1].SetupTime[0]; bytI++)
                        {
                            ArayTmp[1] = bytI;
                            ArayTmp[2] = 1;
                            Array.Copy(myVacs8[intI - 1].SetupTime, 1 + (bytI - 1) * 6, ArayTmp, 3,6);
                            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x0114, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(intDeviceType)) == false)
                            {
                                return;
                            }

                            if (myVacs8[intI - 1].VacationCmds[bytI] != null && myVacs8[intI - 1].VacationCmds[bytI].Count != 0)
                            {
                                foreach (UVCMD.ControlTargets TmpCmd in myVacs8[intI - 1].VacationCmds[bytI])
                                {
                                    byte[] arayCMD = new byte[9];
                                    ArayTmp[2] = byte.Parse(TmpCmd.ID.ToString());
                                    ArayTmp[3] = TmpCmd.Type;
                                    ArayTmp[4] = TmpCmd.SubnetID;
                                    ArayTmp[5] = TmpCmd.DeviceID;
                                    ArayTmp[6] = TmpCmd.Param1;
                                    ArayTmp[7] = TmpCmd.Param2;
                                    ArayTmp[8] = TmpCmd.Param3;   // save targets
                                    ArayTmp[9] = TmpCmd.Param4;
                                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x026A, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(intDeviceType)) == false)
                                    { return; }
                                    HDLUDP.TimeBetwnNext(ArayTmp.Length);
                                }
                            }
                        }
                    }
                    #endregion
                    MyRead2UpFlags[11] = true;
                }
            }
                
           if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(100);
        }

        /// <summary>
        /// strDevName device subnet id and device id
        /// </summary>
        /// <param name="strDevName"></param>
        public void DownLoadSecurityInformationFrmDevice(string strDevName, int intDeviceType, int intActivePage)// 0 mean all, else that tab only
        {
            string strMainRemark = strDevName.Split('\\')[1].Trim();
            String TmpDeviceName = strDevName.Split('\\')[0].Trim();

            byte bytSubID = byte.Parse(TmpDeviceName.Split('-')[0].ToString());
            byte bytDevID = byte.Parse(TmpDeviceName.Split('-')[1].ToString());
            
            //if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(100, null);

            myReadyThings8 = new ReadyThings[8];
            myArms8 = new ArmMode[8];
            myAlarms8 = new AlarmModes[8];
            myVacs8 = new Vacations[8];

            for (int i = 0; i < 1; i++) //循环读取多个区域的设置信息
            {
                myReadyThings8[i] = new ReadyThings();
                myArms8[i] = new ArmMode();
                myAlarms8[i] = new AlarmModes();
                myVacs8[i] = new Vacations();
                if (intActivePage == 0 || intActivePage == 1)
                {
                    byte[] ArayTmp = new byte[] { 1 };
                    //读取基本配置
                    #region
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x0250, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(intDeviceType)) == true)
                    {
                        if (CsConst.myRevBuf != null)
                        {
                            if (CsConst.myRevBuf[26] == 1) myAlarms8[i].bytSirnDelay = CsConst.myRevBuf[27];
                            else myAlarms8[i].bytSirnDelay = 0;

                            myAlarms8[i].bytEnterDelay = CsConst.myRevBuf[28];
                            myAlarms8[i].bytReset = CsConst.myRevBuf[30];

                            myReadyThings8[i].bytExitDelay = CsConst.myRevBuf[29];
                            myReadyThings8[i].bytMore = CsConst.myRevBuf[31];
                            CsConst.myRevBuf = new byte[1200];
                        }
                    }
                    else return;
                    #endregion
                    MyRead2UpFlags[0] = true;

                    //读取布防之前的目标操作
                    myReadyThings8[i].LeaveCmds = new List<UVCMD.ControlTargets>[8];
                    #region
                    for (byte bytI = 0; bytI <= 4; bytI++) //循环读取四个特殊提醒模式
                    {
                        myReadyThings8[i].LeaveCmds[bytI] = new List<UVCMD.ControlTargets>();
                        for (byte bytJ = 1;bytJ <=8;bytJ++)
                        {
                            ArayTmp = new byte[] { 1 ,bytI,bytJ};
                            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1304, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(intDeviceType)) == true)
                            {
                                UVCMD.ControlTargets oCMD = new UVCMD.ControlTargets();
                                oCMD.ID = CsConst.myRevBuf[28];
                                oCMD.Type = CsConst.myRevBuf[29];

                                if (oCMD.Type != 3)
                                {
                                    oCMD.SubnetID = CsConst.myRevBuf[30];
                                    oCMD.DeviceID = CsConst.myRevBuf[31];
                                    oCMD.Param1 = CsConst.myRevBuf[32];
                                    oCMD.Param2 = CsConst.myRevBuf[33];
                                    oCMD.Param3 = CsConst.myRevBuf[34];
                                    oCMD.Param4 = CsConst.myRevBuf[35];
                                    CsConst.myRevBuf = new byte[1200];
                                    myReadyThings8[i].LeaveCmds[bytI].Add(oCMD);
                                }
                            }
                            else return;
                       }
                        if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(5);
                    }
                    #endregion

                    #region
                    for (byte bytI = 3; bytI <= 5; bytI++) //循环读取四个特殊提醒模式
                    {
                        myReadyThings8[i].LeaveCmds[bytI + 2] = new List<UVCMD.ControlTargets>();
                        for (byte bytJ = 1; bytJ <= 8; bytJ++)
                        {
                            ArayTmp = new byte[] { 1, bytI, bytJ };
                            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x024C, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(intDeviceType)) == true)
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
                                    CsConst.myRevBuf = new byte[1200];
                                    myReadyThings8[i].LeaveCmds[bytI + 2].Add(oCMD);
                                }
                            }
                            else return;
                        }
                    }
                    #endregion

                    //bypass 设置
                    #region
                    byte bytTotal = 0;
                     myReadyThings8[i].DetectDoors = new byte[201];
                    myReadyThings8[i].DoorWarns = new List<UVCMD.ControlTargets>[16];
                    for (byte bytI = 1; bytI <= 16; bytI++)
                    {
                        ArayTmp = new byte[] { 1,bytI };
                        if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x012E, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(intDeviceType)) == true)
                        {
                            if (CsConst.myRevBuf[28] == 1)
                            {
                                bytTotal += 1;
                                myReadyThings8[i].DetectDoors[0] = bytTotal;
                                Array.Copy(CsConst.myRevBuf, 28, myReadyThings8[i].DetectDoors, (bytTotal - 1) * 20 + 1, 10);
                                CsConst.myRevBuf = new byte[1200];

                                //读取命令
                                #region
                                myReadyThings8[i].DoorWarns[bytI - 1] = new List<UVCMD.ControlTargets>();
                                for (byte bytJ = 1; bytJ <= 8; bytJ++)
                                {
                                    ArayTmp = new byte[] { 1, bytI, bytJ };
                                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1300, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(intDeviceType)) == true)
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
                                            CsConst.myRevBuf = new byte[1200];
                                            myReadyThings8[i].DoorWarns[bytI - 1].Add(oCMD);
                                        }
                                    }
                                    else return;
                                }
                                #endregion
                            }
                        }
                        else return;
                    }
                    #endregion
                }
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(20);

                if (intActivePage == 0 || intActivePage == 2)
                {
                    //布防目标的读取
                    #region
                    myArms8[i].ArmCmds = new List<UVCMD.ControlTargets>[6];
                    for (byte bytI = 1; bytI <= 6; bytI++) //循环读取四个特殊提醒模式
                    {
                        myArms8[i].ArmCmds[bytI - 1] = new List<UVCMD.ControlTargets>();
                        for (byte bytJ = 1; bytJ <= 8; bytJ++)
                        {
                            byte[] ArayTmp = new byte[] { 1, bytI, bytJ };
                            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x0268, bytSubID, bytDevID, false, true, true, false) == true)
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
                                    CsConst.myRevBuf = new byte[1200];
                                    myArms8[i].ArmCmds[bytI - 1].Add(oCMD);
                                }
                            }
                            else return;
                        }
                    }
                    #endregion
                    //布防的设备列表读取
                    myReadyThings8[i].ArmModules = new byte[481];
                    byte bytTotal = 1;
                    #region
                    for (byte bytI = 1; bytI <= 16; bytI++)
                    {
                        byte[] ArayTmp = new byte[] { 1, bytI };
                        if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x15D5, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(intDeviceType)) == true)
                        {
                            //增加无效的判断 自己设置总数
                            if (CsConst.myRevBuf[27] != 255 && CsConst.myRevBuf[28] != 255)
                            {
                                myReadyThings8[i].ArmModules[0] = bytTotal;
                                Array.Copy(CsConst.myRevBuf, 27, myReadyThings8[i].ArmModules, (bytI - 1) * 30 + 1, 22);
                                CsConst.myRevBuf = new byte[1200];
                                bytTotal++;
                            }
                        }
                        else return;
                    }
                    #endregion
                    MyRead2UpFlags[1] = true;
                }
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(40);

                if (intActivePage == 0 || intActivePage == 3)
                {
                    //读取自动布防撤防
                    myArms8[i].AutoArmDisarm = new byte[60];
                    #region
                    for (byte bytI = 1; bytI <= 6; bytI++)
                    {
                        byte[] ArayTmp = new byte[] { 1, bytI };
                        if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x010E, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(intDeviceType)) == true)
                        {
                            Array.Copy(CsConst.myRevBuf, 27, myArms8[i].AutoArmDisarm, (bytI - 1) * 10, 4);
                            CsConst.myRevBuf = new byte[1200];
                        }
                        else return;
                    }
                    #endregion
                    MyRead2UpFlags[2] = true;
                }
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(60);

                if (intActivePage == 0 || intActivePage == 4)
                {
                    //读取报警设置
                    myAlarms8[i].AlarmCmds = new List<UVCMD.ControlTargets>[23];

                    // 1 自动拨号设置
                    #region
                    myAlarms8[i].AlarmCmds[0] = new List<UVCMD.ControlTargets>();
                    for (byte bytJ = 1; bytJ <= 8; bytJ++)
                    {
                        byte[] ArayTmp = new byte[] { 1,  bytJ };
                        if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x0260, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(intDeviceType)) == true)
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
                                CsConst.myRevBuf = new byte[1200];
                                myAlarms8[i].AlarmCmds[0].Add(oCMD);
                            }
                        }
                        else return;
                    }
                    #endregion

                    //报警蜂鸣器设置
                    #region
                    for (byte bytI = 6; bytI <= 13; bytI++)
                    {
                        myAlarms8[i].AlarmCmds[bytI - 5] = new List<UVCMD.ControlTargets>();
                        for (byte bytJ = 1; bytJ <= 8; bytJ++)
                        {
                            byte[] ArayTmp = new byte[] { 1,bytI, bytJ };
                            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x024C, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(intDeviceType)) == true)
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
                                    CsConst.myRevBuf = new byte[1200];
                                    myAlarms8[i].AlarmCmds[bytI - 5].Add(oCMD);
                                }
                            }
                            else return;
                        }
                    }
                    #endregion

                    //不同报警类型的目标
                    #region
                    for (byte bytI = 1; bytI <= 14; bytI++)
                    {
                        myAlarms8[i].AlarmCmds[bytI + 8] = new List<UVCMD.ControlTargets>();
                        for (byte bytJ = 1; bytJ <= 8; bytJ++)
                        {
                            byte[] ArayTmp = new byte[] { 1, bytI, bytJ };
                            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x026C, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(intDeviceType)) == true)
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
                                    CsConst.myRevBuf = new byte[1200];
                                    myAlarms8[i].AlarmCmds[bytI + 8].Add(oCMD);
                                }
                            }
                            else return;
                        }
                    }
                    #endregion
                    MyRead2UpFlags[3] = true;
                    MyRead2UpFlags[4] = true;
                }
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(80);

                if (intActivePage == 0 || intActivePage == 5)
                {
                    //读取假期设置
                    #region
                    myVacs8[i].SetupTime = new byte[80];
                    myVacs8[i].VacationCmds = new List<UVCMD.ControlTargets>[8];
                    for (byte bytI = 1; bytI <= 8; bytI++)
                    {
                        byte[] ArayTmp = new byte[] { 1, bytI };
                        if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x0112, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(intDeviceType)) == true)
                        {
                            if (CsConst.myRevBuf[27] == 1)
                            {
                                Array.Copy(CsConst.myRevBuf, 27, myVacs8[i].SetupTime, (bytI - 1) * 10, 7);
                                CsConst.myRevBuf = new byte[1200];

                                //读取命令 开
                                #region
                                myVacs8[i].VacationCmds[bytI - 1] = new List<UVCMD.ControlTargets>();
                                for (byte bytJ = 1; bytJ <= 8; bytJ++)
                                {
                                    ArayTmp = new byte[] { 1, bytI, 0, bytJ };
                                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x0116, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(intDeviceType)) == true)
                                    {
                                        UVCMD.ControlTargets oCMD = new UVCMD.ControlTargets();
                                        oCMD.ID = CsConst.myRevBuf[29];
                                        oCMD.Type = CsConst.myRevBuf[30];  //转换为正确的类型

                                        if (oCMD.Type != 3)
                                        {
                                            oCMD.SubnetID = CsConst.myRevBuf[31];
                                            oCMD.DeviceID = CsConst.myRevBuf[32];
                                            oCMD.Param1 = CsConst.myRevBuf[33];
                                            oCMD.Param2 = CsConst.myRevBuf[34];
                                            oCMD.Param3 = CsConst.myRevBuf[35];
                                            oCMD.Param4 = CsConst.myRevBuf[36];
                                            CsConst.myRevBuf = new byte[1200];
                                            myVacs8[i].VacationCmds[bytI - 1].Add(oCMD);
                                        }
                                    }
                                    else return;
                                }
                                #endregion

                                //读取命令 关
                                #region
                                for (byte bytJ = 1; bytJ <= 8; bytJ++)
                                {
                                    ArayTmp = new byte[] { 1, bytI, 1, bytJ };
                                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x0116, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(intDeviceType)) == true)
                                    {
                                        UVCMD.ControlTargets oCMD = new UVCMD.ControlTargets();
                                        oCMD.ID = CsConst.myRevBuf[29];
                                        oCMD.Type = CsConst.myRevBuf[30];  //转换为正确的类型

                                        if (oCMD.Type != 3)
                                        {
                                            oCMD.SubnetID = CsConst.myRevBuf[31];
                                            oCMD.DeviceID = CsConst.myRevBuf[32];
                                            oCMD.Param1 = CsConst.myRevBuf[33];
                                            oCMD.Param2 = CsConst.myRevBuf[34];
                                            oCMD.Param3 = CsConst.myRevBuf[35];
                                            oCMD.Param4 = CsConst.myRevBuf[36];
                                            CsConst.myRevBuf = new byte[1200];
                                            myVacs8[i].VacationCmds[bytI - 1].Add(oCMD);
                                        }
                                    }
                                    else return;
                                }
                                #endregion
                            }
                        }
                        else return;
                    }
                    #endregion
                    MyRead2UpFlags[5] = true;
                }
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(100,null);
        }

    }
}
