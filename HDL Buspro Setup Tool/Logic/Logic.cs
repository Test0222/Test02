using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.Xml;
using System.IO;
using System.Windows.Forms;

namespace HDL_Buspro_Setup_Tool
{
    [Serializable]
    public class Logic : HdlDeviceBackupAndRestore
    {
        public int DIndex;  ////设备唯一编号
        public string DevName;
        public byte[] LogicPos = new byte[50]; //存储位置经纬度
        public byte[] LogicSummerTime = new byte[50]; //存日光节约时间
        public byte[] DateTimeAry = new byte[7];
        public bool isBroadcastTime = false;


        public LogicBlock[] MyDesign = new LogicBlock[12]; //八个区域的准备信息

        public bool[] MyRead2UpFlags = new bool[] { false, false };

        public class LogicBlock   //大的逻辑块
        {
            public int TableID;
            public string Remark; // 备注用于干什么
            public byte[] TableIDs; //表示存储哪些表有效

            public List<byte[]> MyPins = new List<byte[]>(); //逻辑表内容  十二个逻辑表说明
            public string[] Remarks = new string[20]; //每个逻辑表的20个表备注
            public List<UVCMD.ControlTargets>[] ArmCmds; //受控目标 
        }


        //<summary>
        //读取默认的Logic设置，将所有数据读取缓存
        //</summary>
        public void ReadDefaultInfo(int intDeviceType)
        {
            LogicPos = new byte[50]; //存储位置经纬度
            MyDesign = new LogicBlock[12]; //八个区域的准备信息

            for (byte bytI = 0; bytI < 12; bytI++)
            {
                MyDesign[bytI] = new LogicBlock();
                MyDesign[bytI].TableIDs = new byte[21];
                MyDesign[bytI].Remark = "";
                MyDesign[bytI].ArmCmds = new List<UVCMD.ControlTargets>[20];
            }
        }

        //<summary>
        //读取数据库面板设置，将所有数据读至缓存
        //</summary>
        public void ReadSecurityFrmDBTobuf(int DIndex)
        {
            if (LogicPos == null) LogicPos = new byte[50]; //存储位置经纬度
            if (MyDesign == null) MyDesign = new LogicBlock[12]; //十二个逻辑块设置

            //读取经纬度
            #region
            string str = string.Format("select * from dbLogic where DIndex={0}", DIndex);
            OleDbDataReader dr = DataModule.SearchAResultSQLDB(str);
            if (dr != null)
            {
                while (dr.Read())
                {
                    LogicPos = (byte[])dr[1];
                    LogicSummerTime = (byte[])dr[2];
                    DateTimeAry = (byte[])dr[3];
                    isBroadcastTime = (bool)dr[4];
                }
                dr.Close();
            }
            #endregion

            for (int intI = 1; intI <= 12; intI++)
            {
                MyDesign[intI - 1] = new LogicBlock();
                //读取基本信息
                #region
                str = string.Format("select * from dbLogicTable where DIndex={0}", DIndex);
                dr = DataModule.SearchAResultSQLDB(str);
                if (dr != null)
                {
                    while (dr.Read())
                    {
                        MyDesign[intI - 1].Remark = dr.GetString(1);
                        MyDesign[intI - 1].TableIDs = (byte[])dr[2];
                        MyDesign[intI - 1].MyPins = new List<byte[]>();
                        MyDesign[intI - 1].Remarks = (string[])dr[3];
                        for (byte bytI = 0;bytI< 12;bytI++)
                        {
                            byte[] Tmp = (byte[])dr[4 + bytI];
                            if (Tmp != null) MyDesign[intI - 1].MyPins.Add(Tmp);
                        }
                    }
                }
                #endregion

                //读取目标设置 
                #region
                MyDesign[intI - 1].ArmCmds = new List<UVCMD.ControlTargets>[20];
                str = string.Format("select * from dbKeyTargets where DIndex ={0} and KeyIndex = {1} and  Ms04State >= 1 and Ms04State <= 20 order by objID", DIndex, intI);
                OleDbDataReader drKeyCmds = DataModule.SearchAResultSQLDB(str);
                if (drKeyCmds != null)
                {
                    while (drKeyCmds.Read())
                    {
                        int intIndex = drKeyCmds.GetInt16(10); //对应逻辑表号
                        UVCMD.ControlTargets TmpCmd = new UVCMD.ControlTargets();
                        TmpCmd.ID = drKeyCmds.GetByte(2);
                        TmpCmd.Type = drKeyCmds.GetByte(3);
                        TmpCmd.SubnetID = drKeyCmds.GetByte(4);
                        TmpCmd.DeviceID = drKeyCmds.GetByte(5);
                        TmpCmd.Param1 = drKeyCmds.GetByte(6);
                        TmpCmd.Param2 = drKeyCmds.GetByte(7);
                        TmpCmd.Param3 = drKeyCmds.GetByte(8);
                        TmpCmd.Param4 = drKeyCmds.GetByte(9);
                        TmpCmd.Hint = drKeyCmds.GetString(11);

                        if (MyDesign[intI - 1].ArmCmds[intIndex - 1] == null) MyDesign[intI - 1].ArmCmds[intIndex - 1] = new List<UVCMD.ControlTargets>();
                        MyDesign[intI - 1].ArmCmds[intIndex - 1].Add((UVCMD.ControlTargets)TmpCmd);
                    }
                    drKeyCmds.Close();
                }
            #endregion
            }
        }

        public void SaveHints(int tableID)
        {
            string strsql = "update dbLogicTable set Hints = @Hints where DIndex=@DIndex and TableID=@TableID";
            OleDbConnection conn = new OleDbConnection(DataModule.ConString + CsConst.mstrCurPath);
            conn.Open();
            OleDbCommand cmd = new OleDbCommand(strsql, conn);
            ((OleDbParameter)cmd.Parameters.Add("@DIndex", OleDbType.Integer)).Value = DIndex;
            ((OleDbParameter)cmd.Parameters.Add("@TableID", OleDbType.Integer)).Value = tableID;
            cmd.ExecuteNonQuery();
        }

        //<summary>
        //保存数据库面板设置，将所有数据保存
        //</summary>
        public void SaveSecurityToDB()
        {
            //// delete all old information and refresh the database
            string strsql = string.Format("delete * from dbLogic where DIndex={0}", DIndex);
            DataModule.ExecuteSQLDatabase(strsql);

            strsql = string.Format("delete * from dbLogicTable where DIndex={0}", DIndex);
            DataModule.ExecuteSQLDatabase(strsql);

            strsql = string.Format("delete * from dbKeyTargets where DIndex={0}", DIndex);
            DataModule.ExecuteSQLDatabase(strsql);

            //保存基本经纬度信息
            #region
            strsql = @"Insert into dbLogic(DIndex,LogicPos,LogicSummerTime,DateTimeAry,isBroadcastTime) " +
                   "values(@DIndex,@LogicPos,@LogicSummerTime,@DateTimeAry,@isBroadcastTime)";
            //创建一个OleDbConnection对象
            OleDbConnection conn;
            if (CsConst.mstrCurPath != null)
                conn = new OleDbConnection(DataModule.ConString + CsConst.mstrCurPath);
            else
                conn = new OleDbConnection(DataModule.ConString + CsConst.mstrDefaultPath);
            conn.Open();
            OleDbCommand cmd = new OleDbCommand(strsql, conn);
            ((OleDbParameter)cmd.Parameters.Add("@DIndex", OleDbType.Integer)).Value = DIndex;
            ((OleDbParameter)cmd.Parameters.Add("@LogicPos", OleDbType.Binary)).Value = LogicPos;
            ((OleDbParameter)cmd.Parameters.Add("@LogicSummerTime", OleDbType.Binary)).Value = LogicSummerTime;
            ((OleDbParameter)cmd.Parameters.Add("@DateTimeAry", OleDbType.Binary)).Value = DateTimeAry;
            ((OleDbParameter)cmd.Parameters.Add("@isBroadcastTime", OleDbType.Boolean)).Value = isBroadcastTime;
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

            for (int intI = 1; intI <= 12; intI++)
            {
                if (MyDesign[intI - 1] != null && MyDesign[intI - 1].TableIDs != null && MyDesign[intI - 1].MyPins != null && MyDesign[intI - 1].MyPins.Count == 0)
                {
                    //保存基本信息
                    #region
                    strsql = @"Insert into dbLogicTable(DIndex,TableID,Remark,TableIDs,MyPins1,MyPins2,MyPins3,MyPins4,MyPins5,MyPins6,MyPins7,MyPins8,MyPins9,MyPins10,"
                           + "MyPins11,MyPins12,MyPins13,MyPins14,MyPins15,MyPins16,MyPins17,MyPins18,MyPins19,MyPins20)"
                           + " values(@DIndex,@TableID,@Remark,@TableIDs,@MyPins1,@MyPins2,@MyPins3,@MyPins4,@MyPins5,@MyPins6,@MyPins7,@MyPins8,@MyPins9,@MyPins10,"
                           + "@MyPins11,@MyPins12,@MyPins13,@MyPins14,@MyPins15,@MyPins16,@MyPins17,@MyPins18,@MyPins19,@MyPins20)";
                    //创建一个OleDbConnection对象
                    if (CsConst.mstrCurPath != null)
                        conn = new OleDbConnection(DataModule.ConString + CsConst.mstrCurPath);
                    else
                        conn = new OleDbConnection(DataModule.ConString + CsConst.mstrDefaultPath);
                    conn.Open();
                    cmd = new OleDbCommand(strsql, conn);
                   
                    ((OleDbParameter)cmd.Parameters.Add("@DIndex", OleDbType.Integer)).Value = DIndex;
                    ((OleDbParameter)cmd.Parameters.Add("@TableID", OleDbType.Integer)).Value = intI;
                    ((OleDbParameter)cmd.Parameters.Add("@Remark", OleDbType.VarChar)).Value = MyDesign[intI - 1].Remark;
                    ((OleDbParameter)cmd.Parameters.Add("@TableIDs", OleDbType.Binary)).Value = MyDesign[intI - 1].TableIDs;
                    //((OleDbParameter)cmd.Parameters.Add("@Hints", OleDbType.ch)).Value = MyDesign[intI - 1].Remarks;

                    if (MyDesign[intI - 1].MyPins.Count >= 20)
                    {
                        for (byte bytI = 0; bytI < 20; bytI++)
                        {
                            ((OleDbParameter)cmd.Parameters.Add("@MyPins" + (bytI + 1).ToString(), OleDbType.Binary)).Value = MyDesign[intI - 1].MyPins[bytI];
                        }
                    }
                    if (MyDesign[intI - 1].MyPins.Count < 20)
                    {
                        byte[] Tmp = new byte[] { 0 };
                        for (int i = MyDesign[intI - 1].MyPins.Count; i < 20; i++)
                        {
                            ((OleDbParameter)cmd.Parameters.Add("@MyPins" + (i+1).ToString(), OleDbType.Binary)).Value = Tmp;
                        }
                    }
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch
                    {
                        //MessageBox.Show(exp.ToString());
                        conn.Close();
                    }
                    conn.Close();
                    #endregion

                    //保存目标设置  
                    #region
                    if (MyDesign[intI - 1].ArmCmds != null && MyDesign[intI - 1].ArmCmds.Length != 0)
                    {
                        for (int intJ = 0; intJ < MyDesign[intI - 1].ArmCmds.Length; intJ++) // b表ID
                        {

                            if (MyDesign[intI - 1].ArmCmds[intJ] != null && MyDesign[intI - 1].ArmCmds[intJ].Count != 0)
                            {
                                foreach (UVCMD.ControlTargets TmpCmds in MyDesign[intI - 1].ArmCmds[intJ])
                                {
                                    ///// insert into all commands to database
                                    strsql = string.Format("Insert into dbKeyTargets(DIndex,KeyIndex,objID,KeyFunType,SubNetID,DeviceID,FirstParameter,"
                                                   + "SecondParameter,RunTimeMinute,RunTimeSecond,Ms04State,StrTip) values ({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},'{11}')",
                                                   DIndex, intI, TmpCmds.ID, TmpCmds.Type, TmpCmds.SubnetID, TmpCmds.DeviceID, TmpCmds.Param1,
                                                   TmpCmds.Param2, TmpCmds.Param3, TmpCmds.Param4, intJ + 1, TmpCmds.Hint);
                                    DataModule.ExecuteSQLDatabase(strsql);
                                }
                            }

                        }
                    }
                    #endregion
                }
            }
        }

        public void UploadSecurityToDevice(string strDevName, int intDeviceType, int intActivePage)// 0 mean all, else that tab only
        {
            string strMainRemark = strDevName.Split('\\')[1].Trim().Split('(')[0].Trim();
            strDevName = strDevName.Split('\\')[0].Trim();
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
            if (CsConst.mySends.AddBufToSndList(ArayMain, 0x0010, bytSubID, bytDevID, false, true, true, false) == false)
            {
                return;
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(1);
            //上传基本信息
            byte[] ArayPos = new byte[10];
            if (intActivePage == 0)
            {
                #region
                Array.Copy(LogicPos, 0, ArayPos, 0, 10);
                if (CsConst.mySends.AddBufToSndList(ArayPos, 0xDA06, bytSubID, bytDevID, false, true, true, false) == false)
                {
                    return;
                }
                else
                {
                    CsConst.myRevBuf = new byte[1200];
                    HDLUDP.TimeBetwnNext(ArayPos.Length);
                }
                #endregion
            }
            if (intActivePage == 1 || intActivePage == 0)
            {
                //循环上传十二个block的信息
                for (byte intI = 1; intI <= 12; intI++)
                {
                    //修改备注
                    #region
                    byte[] ArayTmp = new byte[22];
                    ArayTmp[0] = 0;
                    ArayTmp[1] = intI;

                    arayTmpRemark = HDLUDP.StringToByte(strMainRemark);
                    if (arayTmpRemark.Length > 20)
                    {
                        Array.Copy(arayTmpRemark, 0, ArayTmp, 2, 20);
                    }
                    else
                    {
                        Array.Copy(arayTmpRemark, 0, ArayTmp, 2, arayTmpRemark.Length);
                    }

                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xDA2E, bytSubID, bytDevID, false, true, true, false) == true)
                    {
                        CsConst.myRevBuf = new byte[1200];
                        HDLUDP.TimeBetwnNext(ArayPos.Length);
                    }
                    else return;
                    #endregion
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(2 + (intI - 1) * 8);
                    //有多少个表
                    #region
                    ArayTmp = new byte[23];
                    ArayTmp[0] = 0;
                    ArayTmp[1] = intI;
                    Array.Copy(MyDesign[intI - 1].TableIDs, 0, ArayTmp, 2, 21);
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xDA16, bytSubID, bytDevID, false, true,true,  false) == true)
                    {
                        CsConst.myRevBuf = new byte[1200];
                        HDLUDP.TimeBetwnNext(ArayPos.Length);
                    }
                    else return;
                    #endregion
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(3 + (intI - 1) * 8);

                    //按照表依次保存数据
                    if (MyDesign[intI - 1].MyPins != null && MyDesign[intI - 1].MyPins.Count != 0)
                    {
                        #region
                        for (byte bytJ = 0; bytJ < MyDesign[intI - 1].TableIDs[0]; bytJ++)
                        {
                            byte bytTableID = MyDesign[intI - 1].TableIDs[1 + bytJ]; //表号 方便取对应的引脚数据

                            //表备注
                            ArayTmp = new byte[23];

                            ArayTmp[0] = 0;
                            ArayTmp[1] = intI;
                            ArayTmp[2] = bytTableID;
                            arayTmpRemark = HDLUDP.StringToByte(MyDesign[intI - 1].Remarks[bytJ]);
                            if (arayTmpRemark != null && arayTmpRemark.Length > 20)
                            {
                                Array.Copy(arayTmpRemark, 0, ArayTmp, 3, 20);
                            }
                            else if (arayTmpRemark != null && arayTmpRemark.Length <= 20)
                            {
                                Array.Copy(arayTmpRemark, 0, ArayTmp, 3, arayTmpRemark.Length);
                            }

                            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xDA3A, bytSubID, bytDevID, false, true, true, false) == true)
                            {
                                CsConst.myRevBuf = new byte[1200];
                                HDLUDP.TimeBetwnNext(ArayPos.Length);
                            }
                            else return;

                            //引脚有有效设置
                            if (MyDesign[intI - 1].MyPins.Count >= bytTableID && MyDesign[intI - 1].MyPins[bytTableID - 1] != null)
                            {
                                ArayTmp = new byte[53];

                                ArayTmp[0] = 0;
                                ArayTmp[1] = intI;
                                ArayTmp[2] = bytTableID;
                                Array.Copy(MyDesign[intI - 1].MyPins[bytJ], 0, ArayTmp, 3, 50);

                                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xDA1A, bytSubID, bytDevID, false, true, true, false) == true)
                                {
                                    CsConst.myRevBuf = new byte[1200];
                                    HDLUDP.TimeBetwnNext(ArayPos.Length);
                                }
                                else return;
                                SaveLogicBlockOneTableCommandsGroup(bytSubID, bytDevID, (Byte)(intI - 1), bytTableID);
                            }
                            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(4 + bytJ + (intI - 1) * 8);
                        }
                        #endregion
                    }
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(intI * 8);
                }
            }
            if (intActivePage == 2 || intActivePage == 0)
            {
                byte[] ArayTmp = new byte[7];
                for (int i = 0; i < 7; i++)
                {
                    ArayTmp[i] = DateTimeAry[i];
                }
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xDA02, bytSubID, bytDevID, false, true, true, false) == true)
                {
                    CsConst.myRevBuf = new byte[1200];
                }
                else return;

                ArayTmp = new byte[1];
                if (isBroadcastTime) ArayTmp[0] = 1;
                else ArayTmp[0] = 0;
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xDA42, bytSubID, bytDevID, false, true, true, false) == true)
                {
                    CsConst.myRevBuf = new byte[1200];
                }
                else return;

                ArayTmp = new byte[21];
                for (int i = 0; i < 21; i++)
                {
                    ArayTmp[i] = LogicPos[i];
                }
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xDA06, bytSubID, bytDevID, false, true, true, false) == true)
                {
                    CsConst.myRevBuf = new byte[1200];
                }
                else return;

                ArayTmp = new byte[16];
                for (int i = 0; i < 16; i++)
                {
                    ArayTmp[i] = LogicSummerTime[i];
                }
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xE492, bytSubID, bytDevID, false, true, true, false) == true)
                {
                    CsConst.myRevBuf = new byte[1200];
                }
                else return;
            }
            MyRead2UpFlags[1] = true;
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(100);
        }

        /// <summary>
        /// strDevName device subnet id and device id
        /// </summary>
        /// <param name="strDevName"></param>
        public void DownLoadSecurityInformationFrmDevice(string strDevName, int intDeviceType, int intActivePage)// 0 mean all, else that tab only
        {
            string strMainRemark = strDevName.Split('\\')[1].Trim();
            DevName = strDevName.Split('\\')[0].Trim();

            byte bytSubID = byte.Parse(DevName.Split('-')[0].ToString());
            byte bytDevID = byte.Parse(DevName.Split('-')[1].ToString());
            byte[] ArayTmp = new byte[0];
            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x000E, bytSubID, bytDevID, false, true, true, false) == false)
            {
                return;
            }

            LogicPos = new byte[50];
            MyDesign = new LogicBlock[12];
            ArayTmp = null;
            if (intActivePage == 0)
            {
                //读取基本信息
                #region
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xDA04, bytSubID, bytDevID, false, true, true, false) == true)
                {
                    if (CsConst.myRevBuf != null)
                    {
                        Array.Copy(CsConst.myRevBuf, 26, LogicPos, 0, 33);
                        CsConst.myRevBuf = new byte[1200];
                    }
                }
                else return;
                #endregion
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(1);
            if (intActivePage == 1 || intActivePage==0)
            {
                for (byte i = 1; i <= 12; i++) //循环读取多个区域的设置信息
                {
                    MyDesign[i - 1] = new LogicBlock();
                    ArayTmp = new byte[] { 0, i };
                    //读取备注
                    #region
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xDA2C, bytSubID, bytDevID, false, true, true, false) == true)
                    {
                        if (CsConst.myRevBuf != null)
                        {
                            byte[] arayRemark = new byte[20];
                            HDLSysPF.CopyRemarkBufferFrmMyRevBuffer(CsConst.myRevBuf, arayRemark, 28);
                            MyDesign[i - 1].Remark = HDLPF.Byte2String(arayRemark);

                            CsConst.myRevBuf = new byte[1200];
                        }
                    }
                    else return;
                    #endregion
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(2 + (i - 1) * 8);
                    //读取多少个表有效
                    MyDesign[i - 1].TableIDs = new byte[21];
                    #region
                    ArayTmp = new byte[] { 0, i };
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xDA14, bytSubID, bytDevID, false, true, true, false) == true)
                    {
                        if (CsConst.myRevBuf != null)
                        {
                            Array.Copy(CsConst.myRevBuf, 28, MyDesign[i - 1].TableIDs, 0, 21);
                            if (MyDesign[i - 1].TableIDs[0] > 20) MyDesign[i - 1].TableIDs[0] = 0;
                            CsConst.myRevBuf = new byte[1200];

                            //判断数据是不是有效  每个逻辑是20个表格 每个对应表ID
                            if (MyDesign[i - 1].TableIDs[0] > 20)
                            {
                                MyDesign[i - 1].TableIDs[0] = 0;
                            }
                            else
                            {
                                for (int intI = 0; intI < 20; intI++)
                                {
                                    if (MyDesign[i - 1].TableIDs[1 + intI] > 20)
                                    {
                                        MyDesign[i - 1].TableIDs[1 + intI]--;
                                        MyDesign[i - 1].TableIDs[1 + intI] = 0;
                                    }
                                }
                            }
                        }

                        if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(3 + (i - 1) * 8);

                        MyDesign[i - 1].MyPins = new List<byte[]>();
                        MyDesign[i - 1].ArmCmds = new List<UVCMD.ControlTargets>[20];
                        if (MyDesign[i - 1].TableIDs[0] != 0)
                        {
                            for (byte bytI = 1; bytI <= MyDesign[i - 1].TableIDs[0]; bytI++)
                            {
                                byte bytTableID = MyDesign[i - 1].TableIDs[bytI];
                                ArayTmp = new byte[] { 0, i, bytTableID };
                                //依次读取表内数据
                                #region
                                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xDA38, bytSubID, bytDevID, false, true, true, false) == true)
                                {
                                    if (CsConst.myRevBuf != null)
                                    {
                                        byte[] TmpPins = new byte[20];
                                        Array.Copy(CsConst.myRevBuf, 29, TmpPins, 0, 20);
                                        MyDesign[i - 1].Remarks[bytI - 1] = HDLPF.Byte2String(TmpPins);
                                        CsConst.myRevBuf = new byte[1200];
                                    }
                                }
                                else return;

                                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xDA18, bytSubID, bytDevID, false, true, true, false) == true)
                                {
                                    if (CsConst.myRevBuf != null)
                                    {
                                        byte[] TmpPins = new byte[50];
                                        Array.Copy(CsConst.myRevBuf, 29, TmpPins, 0, 50);
                                        MyDesign[i - 1].MyPins.Add(TmpPins);
                                        CsConst.myRevBuf = new byte[1200];
                                    }
                                }
                                else return;
                                #endregion
                                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(4 + bytI + (i - 1) * 8);
                                //目标设置
                                DownLoadLogicBlockOneTableCommandsGroup(bytSubID, bytDevID, (Byte)(i - 1), bytTableID, 0, 0);
                                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(8 * i);
                            }
                        }
                    }
                    else return;
                    #endregion
                }
            }
            if (intActivePage == 2 || intActivePage==0)
            {
                ArayTmp = new byte[0];
                DateTimeAry = new byte[7];
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xDA00, bytSubID, bytDevID, false, true, true, false))
                {
                    for (int i = 0; i < 7; i++)
                    {
                        DateTimeAry[i] = CsConst.myRevBuf[26 + i];
                    }
                    CsConst.myRevBuf = new byte[1200];
                }
                else return;

                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xDA40, bytSubID, bytDevID, false, true, true, false))
                {
                    if (CsConst.myRevBuf[26] == 1) isBroadcastTime = true;
                    else isBroadcastTime = false;
                    CsConst.myRevBuf = new byte[1200];
                }
                else return;

                LogicPos = new byte[50];
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xDA04, bytSubID, bytDevID, false, true, true, false))
                {
                    if (CsConst.myRevBuf[25] == 0xF8)
                    {
                        for (int i = 0; i < 33; i++)
                        {
                            LogicPos[i] = CsConst.myRevBuf[26 + i];
                        }
                    }
                    CsConst.myRevBuf = new byte[1200];
                }
                else return;

                LogicSummerTime = new byte[50];
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xE490, bytSubID, bytDevID, false, true, true,false))
                {
                    if (CsConst.myRevBuf[25] == 0xF8)
                    {
                        for (int i = 0; i < 36; i++)
                        {
                            LogicSummerTime[i] = CsConst.myRevBuf[26 + i];
                        }
                    }
                    CsConst.myRevBuf = new byte[1200];
                }
                else return;
            }
            MyRead2UpFlags[0] = true;
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(100,null);
        }

        /// <summary>
        /// 单独下载某个表格的目标
        /// </summary>
        /// <param name="bytSubID"></param>
        /// <param name="bytDevID"></param>
        /// <param name="bytLogicBlock"></param>
        /// <param name="bytTableID"></param>
        public void DownLoadLogicBlockOneTableCommandsGroup(Byte bytSubID, Byte bytDevID,Byte bytLogicBlock,Byte bytTableID,Byte bStartCmd, Byte bToCmd)
        {
            Byte bytI = 0;
            for (Byte TmpTableID = 1; TmpTableID <= MyDesign[bytLogicBlock].TableIDs[0]; TmpTableID++)
            {
                if (bytTableID == MyDesign[bytLogicBlock].TableIDs[TmpTableID])
                {
                    bytI = (Byte)(TmpTableID);
                    break;
                }
            }
            #region
            if (bStartCmd == 0 && bToCmd == 0)
            {
                bStartCmd = 1;
                bToCmd = 20;
            }
            MyDesign[bytLogicBlock].ArmCmds[bytI - 1] = new List<UVCMD.ControlTargets>();

            for (byte bytJ = 1; bytJ <= 20; bytJ++)
            {
                if (bytJ >= bStartCmd && bytJ <= bToCmd)
                {
                    Byte[] ArayTmp = new byte[] { 0, (Byte)(bytLogicBlock * 12 + bytTableID), bytJ };

                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xDA1C, bytSubID, bytDevID, false, true, true, false) == true)
                    {
                        UVCMD.ControlTargets oCMD = new UVCMD.ControlTargets();
                        oCMD.ID = CsConst.myRevBuf[28];
                        oCMD.Type = CsConst.myRevBuf[29];  //转换为正确的类型

                        oCMD.SubnetID = CsConst.myRevBuf[30];
                        oCMD.DeviceID = CsConst.myRevBuf[31];
                        oCMD.Param1 = CsConst.myRevBuf[32];
                        oCMD.Param2 = CsConst.myRevBuf[33];
                        oCMD.Param3 = CsConst.myRevBuf[34];
                        oCMD.Param4 = CsConst.myRevBuf[35];
                        //oCMD.Hint = HDLSysPF.GetHintForCMD(oCMD.SubnetID, oCMD.DeviceID, oCMD.Param1, oCMD.Param2, oCMD.Type);
                        CsConst.myRevBuf = new byte[1200];
                        MyDesign[bytLogicBlock].ArmCmds[bytI - 1].Add(oCMD);
                    }
                    else return;
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(5 + bytI + (bytLogicBlock - 1) * 8);
                }
            }
            #endregion
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(8 * bytLogicBlock);
        }

        /// <summary>
        /// 单独保存某个表格的目标
        /// </summary>
        /// <param name="bytSubID"></param>
        /// <param name="bytDevID"></param>
        /// <param name="bytLogicBlock"></param>
        /// <param name="bytTableID"></param>
        public void SaveLogicBlockOneTableCommandsGroup(Byte bytSubID, Byte bytDevID, Byte bytLogicBlock, Byte bytTableID)
        {
            Byte bytJ = 0;
            for (Byte TmpTableID = 1; TmpTableID <= MyDesign[bytLogicBlock].TableIDs[0]; TmpTableID++)
            {
                if (bytTableID == MyDesign[bytLogicBlock].TableIDs[TmpTableID])
                {
                    bytJ = (Byte)(TmpTableID);
                    break;
                }
            }
           if (bytJ !=0) bytJ--;
            #region
            //修改目标设置    
            if (MyDesign[bytLogicBlock].ArmCmds != null && MyDesign[bytLogicBlock].ArmCmds.Length != 0 && MyDesign[bytLogicBlock].ArmCmds[bytJ] != null)
            {
                #region
                if (MyDesign[bytLogicBlock].ArmCmds[bytJ].Count != 0)
                {
                    Byte[] ArayTmp = new Byte[10];
                    ArayTmp[0] = 0;
                    ArayTmp[1] = (byte)(bytLogicBlock * 12 + bytTableID);

                    foreach (UVCMD.ControlTargets TmpCmd in MyDesign[bytLogicBlock].ArmCmds[bytJ])
                    {
                        ArayTmp[2] = byte.Parse(TmpCmd.ID.ToString());
                        ArayTmp[3] = TmpCmd.Type;
                        ArayTmp[4] = TmpCmd.SubnetID;
                        ArayTmp[5] = TmpCmd.DeviceID;
                        ArayTmp[6] = TmpCmd.Param1;
                        ArayTmp[7] = TmpCmd.Param2;
                        ArayTmp[8] = TmpCmd.Param3;   // save targets
                        ArayTmp[9] = TmpCmd.Param4;
                        if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xDA1E, bytSubID, bytDevID, false, true, true, false) == false)
                        { return; }
                        HDLUDP.TimeBetwnNext(ArayTmp.Length);
                    }
                }
                #endregion
            }
            #endregion
        }
    }
}
