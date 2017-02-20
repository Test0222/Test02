using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Data.OleDb;
using System.Windows.Forms;

namespace HDL_Buspro_Setup_Tool
{
    [Serializable]
    public class MMC : HdlDeviceBackupAndRestore
    {
        public int DIndex;  ////设备唯一编号
        public string strName;
        public string strIP;  // net work information
        public string strRouterIP;
        public string strMaskIP; // 子网掩码
        public string strMAC;

        public string strdry1;//干接点1备注
        public string strdry2;//干接点2备注
        public string strdry3;//干接点3备注
        public string strdry4;//干接点4备注

        public string strrelay1;//继电器1备注
        public string strrelay2;//继电器2备注

        public byte[] ArayCom1 = new byte[20]; // com 1 的配置信息 预留后面 
        public byte[] ArayCom2 = new byte[20]; // com 2 的配置信息 预留后面
        public byte[] ArayCom3 = new byte[20]; // com 3 的配置信息 预留后面
        public byte[] ArayCom4 = new byte[20]; // com 4 的配置信息 预留后面

        public List<NewIRCode> IRCodes = null;  //// 红外码库
        public byte[] Reserved = new byte[50];///电流检测

        public List<RS2BUS> myRS2BUS; // Rs232 to bus commands 
        public List<BUS2RS> myBUS2RS; // bus to  RS232 commands

        public List<Sequence> mySeries; // 序列信息

        public bool[] MyRead2UpFlags = new bool[10];

        // 232 控制bus 
        [Serializable]
        public class RS2BUS
        {
            public int ID;
            public string Remark;
            public RS232Param TmpRS232;
            public List<UVCMD.ControlTargets> BUSCMD;
        }

        // bus 控制RS232
        [Serializable]
        public class BUS2RS
        {
            public byte ID;
            public string Remark;
            public byte bytType;
            public byte bytParam1;
            public byte bytParam2;
            public List<RS232Param> RS232CMD;
        }

        //RS232 控制BUS 
        [Serializable]
        public class RS232Param  //公共的232结构  头  格式   内容
        {
            public byte ID;
            public byte RSNo;//RS No.
            public byte Length; // 信息内容长度
            public byte bytType;  // hex or ascii
            public string RSCMD;  // Rs232 commands 
            public byte bytEndStr; // 结束符
        }

        //序列
        [Serializable]
        public class Sequence
        {
            public byte ID;//区域ID
            public string Remark;///区域备注
            public byte Enable;  //// 区域使能
            public byte[] steps = new byte[16*320];///步骤
        }

        [Serializable]
        public class Step
        {
            public byte ID;//步骤ID
            public byte Type;//类型
            public byte param1;//参数1
            public byte param2;//参数2
            public byte param3;//参数3
            public byte param4;//参数4
            public int Delay;//延时
        }
        [Serializable]
        public class NewIRCode
        {
            public int KeyID;
            public byte DevID;
            public int IRIndex;
            public int IRLength;
            public string Remark;
            public string Codes;
        }

        //<summary>
        //读取默认的多媒体设置，将所有数据读取缓存
        //</summary>
        public void ReadDefaultInfo()
        {
            strIP = "192.168.10.250"; // net work information
            strRouterIP = "192.168.10.1";
            strMaskIP = "255.255.255.0"; // 子网掩码
            strMAC = "H.D.L.85.85.85";
            strdry1 = strdry2 = strdry3 = strdry4 = strrelay1=strrelay2 = "";

            ArayCom1 = new byte[20];
            ArayCom1[0] = 4;//波特率
            ArayCom1[1] = 0;//校验位
            ArayCom1[2] = 0;//数据位
            ArayCom1[3] = 0;//停止位

            ArayCom2 = new byte[20];
            ArayCom2[0] = 4;
            ArayCom2[1] = 0;
            ArayCom2[2] = 0;
            ArayCom2[3] = 0;

            IRCodes = new List<NewIRCode>(); //红外码库
            Reserved = new byte[50];
            if (myBUS2RS == null) myBUS2RS = new List<BUS2RS>();
            if (IRCodes == null) IRCodes = new List<NewIRCode>();

            if (mySeries == null) mySeries = new List<Sequence>();
        }

        //<summary>
        //读取数据库多媒体设置，将所有数据读至缓存
        //</summary>
        public void ReadMMCFrmDBTobuf(int DIndex)
        {
            //read basic information of MMC
            #region
            string str = "select * from dbBasicMMC where DIndex=" + DIndex.ToString();
            OleDbDataReader dr = DataModule.SearchAResultSQLDB(str, CsConst.mstrCurPath);

            if (dr == null) return;
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    strName = dr.GetString(1);
                    strIP = dr.GetString(2);  // internet information
                    strRouterIP = dr.GetString(3);
                    strMAC = dr.GetString(4);
                    strMaskIP = dr.GetString(5);

                    ArayCom1 = (byte[])dr[6];
                    ArayCom2 = (byte[])dr[7];
                    strdry1 = dr.GetString(8);
                    strdry2 = dr.GetString(9);
                    strdry3 = dr.GetString(10);
                    strdry4 = dr.GetString(11);
                    strrelay1 = dr.GetString(12);
                    strrelay2 = dr.GetString(13);
                    Reserved = (byte[])dr[14];
                }
                dr.Close();
            }

            #endregion
            //read RS232 to bus setups  485 to bus
            #region
            //myRS2BUS = new List<RS2BUS>();
            //myRS2BUS = new List<RS2BUS>();
            str = string.Format("select * from dbDevToBus where DIndex={0} order by CMD", DIndex);
            dr = DataModule.SearchAResultSQLDB(str, CsConst.mstrCurPath);

            if (dr == null) return;
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    RS2BUS oRev = new RS2BUS();
                    oRev.ID = dr.GetByte(1);
                    oRev.Remark = dr.GetString(3);
                    oRev.TmpRS232 = new RS232Param();
                    //oRev.TmpRS232.blnEnable = dr.GetByte(4);
                    oRev.TmpRS232.bytType = dr.GetByte(5);
                    oRev.TmpRS232.RSCMD = dr.GetString(6);
                    //oRev.TmpRS232.Endchar = dr.GetByte(7);
                    byte bytCMD = dr.GetByte(1);
                    byte bytTmp = dr.GetByte(8); // 1 表示com1, 2 com 2 

                    // read all commands then recevie this RS232 or 485
                    #region
                    str = string.Format("select * from dbKeyTargets where DIndex ={0} and KeyIndex = {1} and Ms04State = {2} order by objID", DIndex, bytCMD, bytTmp);
                    OleDbDataReader drKeyCmds = DataModule.SearchAResultSQLDB(str, CsConst.mstrCurPath);
                    oRev.BUSCMD = new List<UVCMD.ControlTargets>();
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
                        oRev.BUSCMD.Add(TmpCmd);
                    }
                    drKeyCmds.Close();
                    #endregion

                    if (bytTmp == 1) // com1
                    {
                        //myRS2BUS1.Add(oRev);
                    }
                    if (bytTmp == 2)  // com2
                    {
                        //myRS2BUS2.Add(oRev);
                    }
                }
                dr.Close();
            }
            #endregion
            // read bus to 232 or bus to standrad 485
            #region

            //myBUS2RS1 = new List<BUS2RS>();
            //myBUS2RS2 = new List<BUS2RS>();
            str = string.Format("select * from dbBusToElse where DIndex={0} order by CmdID", DIndex);
            dr = DataModule.SearchAResultSQLDB(str, CsConst.mstrCurPath);
            if (dr == null) return;
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    BUS2RS oRev = new BUS2RS();
                    oRev.ID = dr.GetByte(1);
                    oRev.Remark = dr.GetString(2);
                    oRev.bytType = dr.GetByte(3);
                    oRev.bytParam1 = dr.GetByte(4);
                    oRev.bytParam2 = dr.GetByte(5);
                    byte bytTmp = dr.GetByte(6); // 3 com1, 4 表示com2

                    oRev.RS232CMD = new List<RS232Param>();
                    // read all commands then recevie this Uv switch
                    #region

                    str = string.Format("select * from dbBus2R4 where DIndex={0} and CMD = {1} and byt2Or4 = {2} order by SenNum", DIndex, oRev.ID, bytTmp);
                    OleDbDataReader drCMD = DataModule.SearchAResultSQLDB(str, CsConst.mstrCurPath);

                    oRev.RS232CMD = new List<RS232Param>();

                    if (drCMD != null)
                    {
                        ;
                        if (drCMD.HasRows)
                        {
                            while (drCMD.Read())
                            {
                                RS232Param TmpRS232 = new RS232Param();
                                //TmpRS232.blnEnable = drCMD.GetByte(3);
                                TmpRS232.bytType = drCMD.GetByte(4);
                                TmpRS232.RSCMD = drCMD.GetString(5);
                                oRev.RS232CMD.Add(TmpRS232);
                            }
                            drCMD.Close();
                        }
                    }
                    #endregion

                    if (bytTmp == 3) // com1
                    {
                        //myBUS2RS.Add(oRev);
                    }
                    else   // com2
                    {
                        //myBUS2RS.Add(oRev);
                    }
                }
                dr.Close();
            }
            #endregion

            //read IR codes from database
            #region
            str = "select * from dbIRModule where DIndex=" + DIndex.ToString() + " order by KeyID";
            dr = DataModule.SearchAResultSQLDB(str, CsConst.mstrCurPath);

            IRCodes = new List<NewIRCode>();
            if (dr != null)
            {
                while (dr.Read())
                {
                    NewIRCode tmp = new NewIRCode();
                    DIndex = dr.GetInt16(0);
                    tmp.KeyID = dr.GetInt16(1);
                    tmp.Remark = dr.GetValue(2).ToString();
                    tmp.Codes = dr.GetValue(3).ToString();
                    tmp.IRLength = dr.GetInt16(4);
                    IRCodes.Add(tmp);
                }
            }
            dr.Close();
            #endregion

            //序列
            #region
            str = "select * from dbDevSeqsDR where ID=" + DIndex.ToString() + " and AreaID=1 order by SequenceID";
            OleDbDataReader drSeq = DataModule.SearchAResultSQLDB(str, CsConst.mstrCurPath);
            mySeries = new List<Sequence>();
            while (drSeq.Read())
            {
                Sequence seq = new Sequence();
                seq.ID = drSeq.GetByte(2);
                seq.Remark = drSeq.GetString(3);
                

                mySeries.Add(seq);
            }
            drSeq.Close();
            #endregion
        }

        //<summary>
        //保存数据库面板设置，将所有数据保存
        //</summary>
        public void SaveMMCToDB()
        {
            //// delete all old information and refresh the database
            string strsql = string.Format("delete from dbBasicMMC where  DIndex={0}", DIndex);
            DataModule.ExecuteSQLDatabase(strsql);

            strsql = string.Format("delete from dbBus2R4 where  DIndex={0}", DIndex); // 通用开关对应的RS232 指令集合
            DataModule.ExecuteSQLDatabase(strsql);

            strsql = string.Format("delete from dbDevToBus where  DIndex={0}", DIndex); // RS232 指令集合
            DataModule.ExecuteSQLDatabase(strsql);

            strsql = string.Format("delete from dbKeyTargets where  DIndex={0}", DIndex); // 指令对应的HDL命令集合 
            DataModule.ExecuteSQLDatabase(strsql);

            strsql = string.Format("delete from dbBusToElse where DIndex={0}", DIndex); // 通用开关列表集合
            DataModule.ExecuteSQLDatabase(strsql);

            strsql = string.Format("delete from dbIRModule where DIndex={0}", DIndex); // 删除红外码库
            DataModule.ExecuteSQLDatabase(strsql);
            // save basic setup of mmc
            #region
            strsql = "insert into dbBasicMMC(DIndex,Remark,strIP,strRouterIP,strMAC,strMaskIP,ArayCom1,ArayCom2,strdry1," +
            "strdry2,strdry3,strdry4,strrelay1,strrelay2,Reserved) values(@DIndex,@Remark,@strIP,@strRouterIP,@strMAC,@strMaskIP,@ArayCom1,@ArayCom2,@strdry1,"
            + "@strdry2,@strdry3,@strdry4,@strrelay1,@strrelay2,@Reserved)";

            OleDbConnection conn = new OleDbConnection(DataModule.ConString + CsConst.mstrDefaultPath);
            conn.Open();

            OleDbCommand cmd = new OleDbCommand(strsql, conn);
            ((OleDbParameter)cmd.Parameters.Add("@DIndex", OleDbType.Char)).Value = DIndex;
            ((OleDbParameter)cmd.Parameters.Add("@Remark", OleDbType.VarChar)).Value = strName;
            ((OleDbParameter)cmd.Parameters.Add("@strIP", OleDbType.VarChar)).Value = strIP;
            ((OleDbParameter)cmd.Parameters.Add("@strRouterIP", OleDbType.VarChar)).Value = strRouterIP;
            ((OleDbParameter)cmd.Parameters.Add("@strMAC", OleDbType.VarChar)).Value = strMAC;
            ((OleDbParameter)cmd.Parameters.Add("@strMaskIP", OleDbType.VarChar)).Value = strMaskIP;

            ((OleDbParameter)cmd.Parameters.Add("@ArayCom1", OleDbType.Binary)).Value = ArayCom1;
            ((OleDbParameter)cmd.Parameters.Add("@ArayCom2", OleDbType.Binary)).Value = ArayCom2;
            ((OleDbParameter)cmd.Parameters.Add("@strdry1", OleDbType.VarChar)).Value = strdry1;
            ((OleDbParameter)cmd.Parameters.Add("@strdry2", OleDbType.VarChar)).Value = strdry2;
            ((OleDbParameter)cmd.Parameters.Add("@strdry3", OleDbType.VarChar)).Value = strdry3;
            ((OleDbParameter)cmd.Parameters.Add("@strdry4", OleDbType.VarChar)).Value = strdry4;
            ((OleDbParameter)cmd.Parameters.Add("@strrelay1", OleDbType.VarChar)).Value = strrelay1;
            ((OleDbParameter)cmd.Parameters.Add("@strrelay2", OleDbType.VarChar)).Value = strrelay2;
            ((OleDbParameter)cmd.Parameters.Add("@Reserved", OleDbType.Binary)).Value = Reserved;
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (OleDbException exp)
            {
                MessageBox.Show(exp.ToString());
            }
            conn.Close();
            #endregion
            // save Rs232 --> bus command  com1
            #region
            if (myRS2BUS != null )
            {
                byte bytI = 1;
                foreach (RS2BUS oRs232 in myRS2BUS)
                {
                    if (oRs232.TmpRS232 != null)
                    {
                        RS232Param tmp = oRs232.TmpRS232;
                        strsql = string.Format("insert into dbDevToBus(DIndex,CMD,objID,Remark,Head,Format,Strings,Endchar,R2R4orS2S4) values({0},{1},{2},'{3}',{4},{5},'{6}',{7},{8})",
                                       DIndex, (Byte)oRs232.ID, 0, oRs232.Remark, "", tmp.bytType, tmp.RSCMD, "", 1);
                        DataModule.ExecuteSQLDatabase(strsql);
                    }

                    // save all trigger bus coomands to tarets table
                    #region
                    if (oRs232.BUSCMD != null && oRs232.BUSCMD.Count != 0)
                    {
                        for (int i = 0; i < oRs232.BUSCMD.Count; i++)
                        {
                            UVCMD.ControlTargets TmpCmds = oRs232.BUSCMD[i];
                            ///// insert into all commands to database
                            strsql = string.Format("Insert into dbKeyTargets(DIndex,KeyIndex,objID,KeyFunType,SubNetID,DeviceID,FirstParameter,"
                                           + "SecondParameter,RunTimeMinute,RunTimeSecond,Ms04State) values ({0},{1},'{2}',{3},{4},{5},{6},{7},{8},{9},{10})",
                                           DIndex, oRs232.ID, TmpCmds.ID, TmpCmds.Type, TmpCmds.SubnetID, TmpCmds.DeviceID, TmpCmds.Param1,
                                           TmpCmds.Param2, TmpCmds.Param3, TmpCmds.Param4, 1);
                            DataModule.ExecuteSQLDatabase(strsql);
                        }
                    }

                    #endregion
                    bytI++;
                }
            }
            #endregion

            //save bus to RS232 command
            #region
            if (myBUS2RS != null )
            {
                foreach (BUS2RS oBus in myBUS2RS)
                {
                    strsql = string.Format("insert into dbBusToElse(DIndex,CmdID,Remark,Type,Param1,Param2,Is232No485) values({0},{1},'{2}',{3},{4},{5},{6})",
                                   DIndex, oBus.ID, oBus.Remark, oBus.bytType, oBus.bytParam1, oBus.bytParam2, 3);
                    DataModule.ExecuteSQLDatabase(strsql);

                    // save all trigger bus coomands to tarets table
                    #region
                    if (oBus.RS232CMD != null && oBus.RS232CMD.Count != 0)
                    {
                        for (int i = 0; i < oBus.RS232CMD.Count; i++)
                        {
                            RS232Param TmpRS = oBus.RS232CMD[i];
                            ///// insert into all commands to database

                            strsql = string.Format("insert into dbBus2R4(DIndex,CMD,SenNum,bytParam1,bytParam2,Strings,EndChar,byt2Or4) values({0},{1},{2},{3},{4},'{5}',{6},{7})",
                                   DIndex, oBus.ID, i, "", TmpRS.bytType, TmpRS.RSCMD, "", 3);
                            DataModule.ExecuteSQLDatabase(strsql);
                        }
                    }
                    #endregion
                }
            }
            #endregion

            //save ir codes
            #region
            if (IRCodes != null)
            {
                //// insert new commands to database
                for (int intJ = 0; intJ < IRCodes.Count; intJ++)
                {
                    NewIRCode TmpIR = IRCodes[intJ];
                    ///// insert into all commands to database
                    strsql = string.Format("Insert into dbIRModule(DIndex,KeyID,Remark,Codes,IRLength) values({0},{1},'{2}','{3}',{4})",
                            DIndex, TmpIR.KeyID, TmpIR.Remark, TmpIR.Codes, TmpIR.IRLength);
                    DataModule.ExecuteSQLDatabase(strsql);

                }
            }
            #endregion

            // save sequence 
            #region
            if (mySeries != null && mySeries.Count != 0)
            {
                foreach (Sequence seq in mySeries)
                {
                    string str = "insert into dbDevSeqsDR(ID,AreaID,SequenceID,Remark,Steps) values(@ID,@AreaID,@SequenceID,@Remark,@Steps)";

                    conn = new OleDbConnection(DataModule.ConString + CsConst.mstrDefaultPath);
                    conn.Open();

                    cmd = new OleDbCommand(str, conn);
                    ((OleDbParameter)cmd.Parameters.Add("@ID", OleDbType.SmallInt)).Value = DIndex;
                    ((OleDbParameter)cmd.Parameters.Add("@AreaID", OleDbType.SmallInt)).Value = 1;
                    ((OleDbParameter)cmd.Parameters.Add("@SequenceID", OleDbType.SmallInt)).Value = seq.ID;
                    ((OleDbParameter)cmd.Parameters.Add("@Remark", OleDbType.VarChar)).Value = seq.Remark;
                    ((OleDbParameter)cmd.Parameters.Add("@Steps", OleDbType.VarBinary)).Value = seq.steps.ToArray();
                    
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (OleDbException exp)
                    {
                        MessageBox.Show(exp.ToString());
                    }
                    conn.Close();
                }
            }
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
        public bool UploadMMCInfosToDevice(string DevName, int intDeviceType, int intActivePage)
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

            if (CsConst.mySends.AddBufToSndList(ArayMain, 0x0010, bytSubID, bytDevID, false, true, true, false) == false)
            {
                return false;
            }

            // 修改串口com1的配置信息 
            byte[] ArayTmp;
            #region
            if (intActivePage == 0)
            {
                for (byte bytI = 0; bytI < 4; bytI++)
                {
                    ArayTmp = new byte[6];
                    ArayTmp[0] = 1;
                    ArayTmp[1] = bytI;
                    switch (bytI)
                    {
                        case 0: Array.Copy(ArayCom1, 0, ArayTmp, 2, 4); break;
                        case 1: Array.Copy(ArayCom2, 0, ArayTmp, 2, 4); break;
                        case 2: Array.Copy(ArayCom3, 0, ArayTmp, 2, 4); break;
                        case 3: Array.Copy(ArayCom4, 0, ArayTmp, 2, 4); break;
                    }
                    Array.Copy(ArayCom1, 0, ArayTmp, 2, 4);
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1370, bytSubID, bytDevID, false, true, true, false) == false)
                    {
                        return false;
                    }
                    //修改干接点备注
                    #region
                    ArayTmp = new byte[22];
                    ArayTmp[0] = 7;
                    ArayTmp[1] = (byte)(bytI + 1);
                    string strRemark = "";
                    switch (bytI)
                    {
                        case 0: strRemark = strdry1; break;
                        case 1: strRemark = strdry2; break;
                        case 2: strRemark = strdry3; break;
                        case 3: strRemark = strdry4; break;
                    }
                    if (strRemark == null) strRemark = "";
                    byte[] arayRemark = HDLUDP.StringToByte(strRemark);

                    arayRemark.CopyTo(ArayTmp, 2);
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1370, bytSubID, bytDevID, false, true, true, false) == false)
                    {
                        return false;
                    }
                    #endregion
                }

            #endregion

                //修改继电器备注
                #region
                for (byte bytI = 1; bytI < 3; bytI++)
                {
                    ArayTmp = new byte[22];
                    ArayTmp[0] = 8;
                    ArayTmp[1] = 1;
                    string strRemark = "";
                    switch (bytI)
                    {
                        case 1: strRemark = strrelay1; break;
                        case 2: strRemark = strrelay2; break;
                    }
                    if (strRemark != null)
                    {
                        byte[] arayRemark = HDLUDP.StringToByte(strRemark);
                        arayRemark.CopyTo(ArayTmp, 2);
                    }
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1370, bytSubID, bytDevID, false, true, true, false) == false)
                    {
                        return false;
                    }
                }
            }
            #endregion
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(2);
            if (intActivePage == 0 || intActivePage == 1)
            {
                // 232 to bus 
                #region
                byte bytJ = 1;
                if (myRS2BUS != null)
                {
                    #region
                    foreach (RS2BUS oTmp in myRS2BUS)
                    {
                        ArayTmp = new byte[37];
                        if (oTmp.TmpRS232 == null) continue;
                        ArayTmp[0] = 4;
                        ArayTmp[1] = bytJ;
                        if (oTmp.TmpRS232.RSCMD == null || oTmp.TmpRS232.RSCMD == "") //无效
                        {
                            ArayTmp[2] = 0;
                        }
                        else
                        {
                            ArayTmp[2] = Convert.ToByte(oTmp.TmpRS232.RSCMD.Length);
                            ArayTmp[3] = oTmp.TmpRS232.RSNo;
                            ArayTmp[4] = oTmp.TmpRS232.bytType;

                            if (oTmp.TmpRS232.bytType == 1)
                            {
                                byte[] arayTmp = HDLUDP.StringToByte(oTmp.TmpRS232.RSCMD);
                                arayTmp.CopyTo(ArayTmp, 5);
                            }
                            else if (oTmp.TmpRS232.bytType == 2)
                            {
                                string[] Tmp = oTmp.TmpRS232.RSCMD.Split(' ');
                                for (byte byti = 0; byti < Tmp.Length; byti++) { ArayTmp[5 + byti] = HDLPF.StringToByte(Tmp[byti]); };
                            }
                        }

                        if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1370, bytSubID, bytDevID, false, true, true, false) == false)
                        {
                            return false;
                        }

                        // 修改备注
                        #region
                        ArayTmp = new byte[23];
                        ArayTmp[0] = 5;
                        ArayTmp[1] = bytJ;
                        if (oTmp.Remark == null) oTmp.Remark = "";
                        byte[] arayRemark = HDLUDP.StringToByte(oTmp.Remark);

                        arayRemark.CopyTo(ArayTmp, 2);
                        if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1370, bytSubID, bytDevID, false, true, true, false) == false)
                        {
                            return false;
                        }
                        #endregion

                        //修改目标
                        #region
                        byte bytK = 1;
                        if (oTmp.BUSCMD != null && oTmp.BUSCMD.Count != 0)
                        {
                            foreach (UVCMD.ControlTargets TmpCmd in oTmp.BUSCMD)
                            {
                                byte[] arayCMD = new byte[10];
                                arayCMD[0] = 6;
                                arayCMD[1] = bytJ;
                                arayCMD[2] = bytK;
                                arayCMD[3] = TmpCmd.Type;
                                arayCMD[4] = TmpCmd.SubnetID;
                                arayCMD[5] = TmpCmd.DeviceID;
                                arayCMD[6] = TmpCmd.Param1;
                                arayCMD[7] = TmpCmd.Param2;
                                arayCMD[8] = TmpCmd.Param3;   // save targets
                                arayCMD[9] = TmpCmd.Param4;
                                if (CsConst.mySends.AddBufToSndList(arayCMD, 0x1370, bytSubID, bytDevID, false, true, true, false) == true)
                                {
                                    //HDLUDP.TimeBetwnNext(arayCMD.Length);
                                    bytK++;
                                }
                                else return false;
                            }
                        }
                        for (byte bytTmp = bytK; bytTmp <= 8; bytTmp++)
                        {
                            byte[] arayCMD = new byte[10];
                            arayCMD[0] = 6;
                            arayCMD[1] = bytJ;
                            arayCMD[2] = bytTmp;
                            arayCMD[3] = 3;
                            if (CsConst.mySends.AddBufToSndList(arayCMD, 0x1370, bytSubID, bytDevID, false, true, true, false) == true)
                            {
                                //HDLUDP.TimeBetwnNext(arayCMD.Length);
                            }
                            else return false;
                        }
                        #endregion
                        if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(2 + bytJ / 20);
                        bytJ++;
                    }
                    #endregion
                }
                #endregion
                for (byte bytTmp = bytJ; bytTmp <= 200; bytTmp++)
                {
                    ArayTmp = new byte[37];
                    ArayTmp[0] = 4;
                    ArayTmp[1] = bytTmp;
                    ArayTmp[2] = 0;

                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1370, bytSubID, bytDevID, false, true, true, false) == false)
                    {
                        return false;
                    }  
                }
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(20);
            }

            if (intActivePage == 0 || intActivePage == 2)
            {
                //修改bus到232的集合
                #region
                byte bytI = 1;
                if (myBUS2RS != null && myBUS2RS.Count != 0)
                {
                    foreach (BUS2RS oTmp in myBUS2RS)
                    {
                        byte[] ArayTest = new byte[25];
                        // 修改备注 参数
                        #region
                        ArayTest[0] = 3;
                        ArayTest[1] = bytI;
                        ArayTest[2] = oTmp.bytType;
                        ArayTest[3] = oTmp.bytParam1;
                        ArayTest[4] = oTmp.bytParam2;

                        if (oTmp.Remark == null) oTmp.Remark = "";
                        byte[] arayRemark = HDLUDP.StringToByte(oTmp.Remark);

                        arayRemark.CopyTo(ArayTest, 5);
                        if (CsConst.mySends.AddBufToSndList(ArayTest, 0x1370, bytSubID, bytDevID, false, true, true, false) == false)
                        {
                            return false;
                        }
                        #endregion

                        byte bytK = 1;
                        if (oTmp.RS232CMD != null && oTmp.RS232CMD.Count != 0)
                        {
                            #region
                            foreach (RS232Param oRS232 in oTmp.RS232CMD)
                            {
                                ArayTmp = new byte[38];
                                ArayTmp[0] = 2;
                                ArayTmp[1] = bytI;
                                ArayTmp[2] = bytK;
                                if (oRS232.RSCMD == null || oRS232.RSCMD == "") //无效
                                {
                                    ArayTmp[3] = 0;
                                }
                                else
                                {
                                    ArayTmp[3] = Convert.ToByte(oRS232.RSCMD.Length);
                                    ArayTmp[4] = oRS232.RSNo;
                                    ArayTmp[5] = oRS232.bytType;

                                    if (oRS232.bytType == 1)
                                    {
                                        byte[] arayTmp = HDLUDP.StringToByte(oRS232.RSCMD);
                                        arayTmp.CopyTo(ArayTmp, 6);
                                    }
                                    else if (oRS232.bytType == 2)
                                    {
                                        string[] Tmp = oRS232.RSCMD.Split(',');
                                        for (byte byti = 0; byti < Tmp.Length; byti++) { ArayTmp[6 + byti] = HDLPF.StringToByte(Tmp[byti]); };
                                    }
                                }

                                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1370, bytSubID, bytDevID, false, true, true, false) == false)
                                {
                                    return false;
                                }
                                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(20 + bytK / 20);
                                bytK++;
                            }
                            #endregion
                        }

                        for (byte bytTmp = bytK; bytTmp <= ConstMMC.MaxUVSwitchCount; bytTmp++)
                        {
                            ArayTmp = new byte[38];
                            ArayTmp[0] = 2;
                            ArayTmp[1] = bytI;
                            ArayTmp[2] = bytTmp;
                            ArayTmp[3] = 0;

                            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1370, bytSubID, bytDevID, false, true, true, false) == false)
                            {
                                return false;
                            }
                        }
                        bytI++;
                    }
                }
                #endregion
                for (byte bytTmp = bytI; bytTmp <= 100; bytTmp++)
                {
                    ArayTmp = new byte[25];
                    ArayTmp[0] = 3;
                    ArayTmp[1] = bytTmp;
                    ArayTmp[2] = 0;

                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1370, bytSubID, bytDevID, false, true, true, false) == false)
                    {
                        return false;
                    }
                }
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(40);
            }


            if (intActivePage == 0 || intActivePage == 3) //红外库
            {
                // add a array for key id, because ir codes could not be removed, it could make the setup already existed works in wrong way
                List<byte> TmpKeyID = new List<byte>();
                if (IRCodes != null && IRCodes.Count != 0)
                {
                    #region
                    for (int i = 0; i < IRCodes.Count; i++)
                    {
                        NewIRCode otmp = IRCodes[i];
                        byte[] ArayUpload = new byte[26];
                        ArayUpload[0] = (byte)otmp.KeyID;
                        ArayUpload[1] = 0;
                        ArayUpload[2] = otmp.DevID;
                        ArayUpload[3] = (byte)(otmp.IRIndex / 256);
                        ArayUpload[4] = (byte)(otmp.IRIndex % 256);
                        ArayUpload[5] = (byte)otmp.IRLength;

                        if (otmp.Remark != null)
                        {
                            arayTmpRemark = HDLUDP.StringToByte(otmp.Remark);
                            if (arayTmpRemark.Length > 20)
                            {
                                Array.Copy(arayTmpRemark, 0, ArayUpload, 6, 20);
                            }
                            else
                            {
                                Array.Copy(arayTmpRemark, 0, ArayUpload, 6, arayTmpRemark.Length);
                            }
                        }

                        if (CsConst.mySends.AddBufToSndList(ArayUpload, 0x137A, bytSubID, bytDevID, false, true, true, false) == true) //请求空间
                        {
                            CsConst.myRevBuf = new byte[1200];
                            if (CsConst.isRestore)
                            {
                                byte[] arayCodes = GlobalClass.HexToByte(otmp.Codes);
                                if (arayCodes.Length > 62)
                                {
                                    int Count = arayCodes.Length / 62;
                                    if (arayCodes.Length % 124 != 0) Count = Count + 1;
                                    for (int j = 0; j < Count; j++)
                                    {
                                        if (arayCodes.Length % 62 != 0)
                                        {
                                            if (i == (Count - 1))
                                            {
                                                ArayTmp = new byte[2 + arayCodes.Length % 124];
                                                ArayTmp[0] = Convert.ToByte(otmp.KeyID);
                                                ArayTmp[1] = Convert.ToByte(j + 1);
                                                for (int k = 0; k < arayCodes.Length % 62; k++) ArayTmp[2 + k] = arayCodes[j * 62 + k];
                                            }
                                            else
                                            {
                                                ArayTmp = new byte[2 + 62];
                                                ArayTmp[0] = Convert.ToByte(otmp.KeyID);
                                                ArayTmp[1] = Convert.ToByte(j + 1);
                                                for (int k = 0; k < 62; k++) ArayTmp[2 + k] = arayCodes[j * 62 + k];
                                            }
                                        }
                                        else
                                        {
                                            ArayTmp = new byte[2 + 62];
                                            ArayTmp[0] = Convert.ToByte(otmp.KeyID);
                                            ArayTmp[1] = Convert.ToByte(j + 1);
                                            for (int k = 0; k < 62; k++) ArayTmp[2 + k] = arayCodes[j * 62 + k]; ;
                                        }
                                        if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x137A, bytSubID, bytDevID, false, true, true, false) == true)
                                        {
                                            CsConst.myRevBuf = new byte[1200];
                                        }
                                        else
                                        {
                                            return false;
                                        }
                                    }
                                }
                            }
                            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(40 + i);
                        }
                        else return false;
                    }
                    #endregion
                }
            }

            if (intActivePage == 0 || intActivePage == 4) //电流检测
            {
                if (Reserved != null || Reserved.Length != 0)
                {
                    byte[] ArayTest = new byte[5];
                    // 修改备注 参数
                    #region
                    for (byte bytI = 1; bytI <= 5; bytI++)
                    {
                        ArayTest[0] = 9;
                        Array.Copy(Reserved, (bytI - 1) * 10, ArayTest, 1, 4);

                        if (CsConst.mySends.AddBufToSndList(ArayTest, 0x1370, bytSubID, bytDevID, false, true, true, false) == false)
                        {
                            return false;
                        }
                    }
                    #endregion
                }
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(85);
            }

            if (intActivePage == 0 || intActivePage == 5)  // 序列
            {
                #region
                ArayTmp = new byte[9];
                ArayTmp[0] = 0x0B;
                for (int i = 0; i < 1; i++)
                {
                    ArayTmp[i+1] = mySeries[i].Enable;
                }

                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1370, bytSubID, bytDevID, false, true, true, false) == true)
                {
                }
                else return false;

                for (int i = 0; i < 1; i++) //暂时只上传一个区域的信息
                {
                    if (mySeries[i].Enable == 1)
                    {
                        for (int j = 0; j < 16; j++)
                        {
                            ArayTmp = new byte[12];
                            ArayTmp[0] = 0x0C;
                            ArayTmp[1] = Convert.ToByte(i + 1);
                            ArayTmp[2] = Convert.ToByte(j + 1);
                            if (mySeries[i] != null && mySeries[i].steps != null && mySeries[i].steps.Length != 0)
                            {
                                for (int intI = 0; intI < 16; intI++)
                                {
                                    ArayTmp[3] = Convert.ToByte(intI + 1);

                                    Array.Copy(mySeries[i].steps,i* 320 + intI * 10 + 1, ArayTmp, 4, 7);

                                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1370, bytSubID, bytDevID, false, true, true, false) == true)
                                    {
                                    }
                                    else return false;
                                }
                            }
                            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(84 + j);
                        }
                    }
                }
                
                #endregion
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(99);
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(100);
            return true;
        }

        /// <summary>
        /// devname device name 
        /// </summary>
        /// <param name="DevName"></param>
        public bool DownloadRS232FrmDeviceToBuf(string DevName, int intDeviceType, int intActivePage)
        {
            string strMainRemark = DevName.Split('\\')[1].Trim();
            DevName = DevName.Split('\\')[0].Trim();

            byte bytSubID = byte.Parse(DevName.Split('-')[0].ToString());
            byte bytDevID = byte.Parse(DevName.Split('-')[1].ToString());

            byte[] ArayTmp = new byte[0];
            string TmpRemark= HDLSysPF.ReadDeviceMainRemark(bytSubID, bytDevID);

            strName = bytSubID.ToString() + "-" + bytDevID.ToString() + "\\" + TmpRemark;

            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(1);

            if (intActivePage == 0 || intActivePage == 1)
            {
                // 232 to bus
                #region
                myRS2BUS = new List<RS2BUS>();
                ArayTmp = new byte[2];
                if (CsConst.isRestore)
                {
                    for (byte bytI = 1; bytI <= 200; bytI++)
                    {
                        RS2BUS oTmp = new RS2BUS();
                        oTmp.TmpRS232 = new RS232Param();
                        ArayTmp[0] = 0x04;
                        ArayTmp[1] = bytI;

                        //读取RS2BUS信息
                        #region
                        if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1372, bytSubID, bytDevID, false, true, true, false) == true)
                        {
                            oTmp.ID = bytI;
                            oTmp.TmpRS232.Length = CsConst.myRevBuf[27];
                            if (oTmp.TmpRS232.Length > 32) oTmp.TmpRS232.Length = 32;

                            oTmp.TmpRS232.RSNo = CsConst.myRevBuf[28];
                            oTmp.TmpRS232.bytType = CsConst.myRevBuf[29];
                            byte[] arayRSCMD = new byte[32];
                            Array.Copy(CsConst.myRevBuf, 30, arayRSCMD, 0, oTmp.TmpRS232.Length);

                            if (CsConst.myRevBuf[29] == 0) oTmp.TmpRS232.RSCMD = "";
                            else if (CsConst.myRevBuf[29] == 1)
                            {
                                oTmp.TmpRS232.RSCMD = HDLPF.Byte2String(arayRSCMD);
                            }
                            else if (CsConst.myRevBuf[29] == 2)
                            {
                                for (int i = 0; i < oTmp.TmpRS232.Length; i++)
                                {
                                    oTmp.TmpRS232.RSCMD = oTmp.TmpRS232.RSCMD + arayRSCMD[i].ToString("X2") + ",";
                                }
                            }

                            CsConst.myRevBuf = new byte[1200];
                        }
                        else return false;
                        #endregion

                        if (oTmp.TmpRS232.Length != 0)
                        {
                            ArayTmp[0] = 0x05;
                            ArayTmp[1] = bytI;
                            // 读取备注
                            #region
                            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1372, bytSubID, bytDevID, false, true, true, false) == true)
                            {
                                byte[] arayRemark = new byte[20];
                                for (int intJ = 0; intJ < 20; intJ++) { arayRemark[intJ] = CsConst.myRevBuf[27 + intJ]; }
                                oTmp.Remark = HDLPF.Byte2String(arayRemark);
                            }
                            else return false;
                            #endregion

                            //读取bus指令集合
                            #region
                            ArayTmp = new byte[3];
                            ArayTmp[0] = 0x06;
                            ArayTmp[1] = bytI;
                            oTmp.BUSCMD = new List<UVCMD.ControlTargets>();
                            for (byte bytJ = 1; bytJ <= 8; bytJ++)
                            {
                                ArayTmp[2] = bytJ;
                                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1372, bytSubID, bytDevID, false, true, true, false) == true)
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
                                    oTmp.BUSCMD.Add(oCMD);
                                    CsConst.myRevBuf = new byte[1200];
                                }
                                else return false;
                            }
                            #endregion
                            myRS2BUS.Add(oTmp);
                        }
                        if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(bytI / 10);

                    }
                }
                #endregion
                MyRead2UpFlags[0] = true;
            }
            
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(20);

            if (intActivePage == 0 || intActivePage == 2)
            {
                // bus to 232
                #region
                myBUS2RS = new List<BUS2RS>();
                ArayTmp = new byte[2];
                ArayTmp[0] = 3;
                for (byte bytI = 1; bytI <= ConstMMC.MaxUVSwitchCount; bytI++)
                {
                    ArayTmp[1] = bytI;
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1372, bytSubID, bytDevID, false, true, true, false) == true)
                    {
                        BUS2RS oTmp = new BUS2RS();
                        oTmp.ID = bytI;
                        oTmp.bytType = CsConst.myRevBuf[27];
                        oTmp.bytParam1 = CsConst.myRevBuf[28];
                        oTmp.bytParam2 = CsConst.myRevBuf[29];

                        byte[] arayRemark = new byte[20];
                        Array.Copy(CsConst.myRevBuf, 30, arayRemark, 0, 20);
                        oTmp.Remark = HDLPF.Byte2String(arayRemark);

                        if (oTmp.bytType == 0x01) //通用开关
                        {
                            byte[] ArayTest = new byte[3];
                            ArayTest[0] = 2;
                            ArayTest[1] = bytI;
                            //读取指令集合
                            #region
                            oTmp.RS232CMD = new List<RS232Param>();
                            for (byte bytJ = 1; bytJ <= ConstMMC.EveryUVMaxRS232CMDCount; bytJ++)
                            {
                                ArayTest[2] = bytJ;
                                if (CsConst.mySends.AddBufToSndList(ArayTest, 0x1372, bytSubID, bytDevID, false, true, true, false) == true)
                                {
                                    RS232Param oTmp232 = new RS232Param();
                                    oTmp232.RSNo = CsConst.myRevBuf[29];
                                    oTmp232.bytType = CsConst.myRevBuf[30];

                                    if (CsConst.myRevBuf[64] > 32) CsConst.myRevBuf[64] = 32;

                                    arayRemark = new byte[CsConst.myRevBuf[64]];
                                    Array.Copy(CsConst.myRevBuf, 31, arayRemark, 0, arayRemark.Length);

                                    oTmp232.RSCMD = "";

                                    if (oTmp232.bytType != 0) // invalid no need to do
                                    {
                                        if (oTmp232.bytType == 1)  // ascii
                                            oTmp232.RSCMD = HDLPF.Byte2String(arayRemark);
                                        else if (oTmp232.bytType == 2) // hex
                                        {
                                            for (int i = 0; i < arayRemark.Length; i++)
                                            {
                                                oTmp232.RSCMD = oTmp232.RSCMD + arayRemark[i].ToString("X2") + ",";
                                            }
                                            oTmp232.RSCMD = oTmp232.RSCMD.Substring(0, oTmp232.RSCMD.Length - 1);
                                        }
                                        oTmp.RS232CMD.Add(oTmp232);
                                    }
                                }
                                else return false;
                            }
                            #endregion
                            myBUS2RS.Add(oTmp);
                        }
                        CsConst.myRevBuf = new byte[1200];
                        if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(20 + bytI / 20);
                    }
                    else return false;
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(20 + bytI / 10);
                }
                #endregion
                MyRead2UpFlags[1] = true;
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(40);

            if (intActivePage == 0 || intActivePage == 3) // 红外码
            {
                IRCodes = new List<NewIRCode>();
                #region
                for (byte bytI = 1; bytI <= ConstMMC.MaxIRDeviceCount; bytI++) //读取红外码
                {
                    byte[] ArayTest = new byte[2];
                    ArayTest[0] = bytI;
                    ArayTest[1] = 0;
                    if (CsConst.mySends.AddBufToSndList(ArayTest, 0x137C, bytSubID, bytDevID, false, true, true, false) == true)
                    {
                        //if (CsConst.myRevBuf[27] <= 6 && CsConst.myRevBuf[30] > 0 && CsConst.myRevBuf[30] <= 64)  //有效 接着读取
                        //{
                        NewIRCode temp = new NewIRCode();
                        temp.KeyID = bytI;
                        temp.DevID = CsConst.myRevBuf[27];
                        temp.IRIndex = CsConst.myRevBuf[28] * 256 + CsConst.myRevBuf[29];
                        temp.IRLength = CsConst.myRevBuf[30];
                        byte[] arayRemark = new byte[20];
                        for (int intI = 0; intI < 20; intI++) { arayRemark[intI] = CsConst.myRevBuf[32 + intI]; };
                        temp.Remark = HDLPF.Byte2String(arayRemark);
                        temp.Codes = "";
                        IRCodes.Add(temp);
                        CsConst.myRevBuf = new byte[1200];
                        //}
                    }
                    else return false;
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(40 + bytI);
                }
                #endregion
                MyRead2UpFlags[2] = true;
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(84);

            if (intActivePage == 0 || intActivePage == 4) // 电流检测
            {
                Reserved = new byte[50];
                ArayTmp = new byte[2];
                ArayTmp[0] = 9;
                for (byte bytI = 1; bytI <= 5; bytI++)
                {
                    ArayTmp[1] = bytI;
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1372, bytSubID, bytDevID, false, true, true, false) == true)
                    {
                        Array.Copy(CsConst.myRevBuf, 26, Reserved, (bytI - 1) * 10, 4);
                        CsConst.myRevBuf = new byte[1200];
                    }
                    else return false;
                }
                MyRead2UpFlags[3] = true;
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(85);

            if (intActivePage == 0 || intActivePage == 5)  //默认只读取一个区域
            {
                //Sequences
                mySeries=new List<Sequence>();
                mySeries.Add(new Sequence());
                ArayTmp = new byte[1];
                ArayTmp[0] = 0x0B;
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1372, bytSubID, bytDevID, false, true, true, false) == true)
                {
                    for (byte bytI = 0; bytI < 1; bytI++)
                    {
                        mySeries[bytI] = new Sequence();
                        mySeries[bytI].ID = Convert.ToByte(bytI + 1);
                        mySeries[bytI].Remark = "";
                        mySeries[bytI].Enable = CsConst.myRevBuf[26 + bytI];
                    }
                    CsConst.myRevBuf = new byte[1200];
                }
                else return false;

                #region
                for (byte i = 0; i < 1; i++)
                {
                     if (mySeries[i].Enable == 1)
                    {
                        mySeries[i].steps = new byte[16 * 320];
                        for (byte bytJ = 0; bytJ < 16; bytJ++)
                        {
                            ArayTmp = new byte[4];
                            ArayTmp[0] = 0x0C;
                            ArayTmp[1] = Convert.ToByte(i + 1);
                            ArayTmp[2] = Convert.ToByte(bytJ + 1);
                            
                            for (byte j = 1; j <= 32; j++)
                            {
                                ArayTmp[3] = j;
                                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1372, bytSubID, bytDevID, false, true, true, false) == true)
                                {
                                    Array.Copy(CsConst.myRevBuf, 28, mySeries[i].steps, bytJ * 320 + (j - 1) * 10, 10);
                                    CsConst.myRevBuf = new byte[1200];
                                }
                                else return false;
                                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(86 + j / 3);
                            }
                        }
                    }
                    else
                    {
                        mySeries[i].steps = new byte[16 * 320];
                        for (byte bytJ = 0; bytJ < 16; bytJ++)
                        {
                            for (byte j = 1; j <= 32; j++)
                            {
                                byte[] temp = new byte[] { j, 0, 0, 0, 0, 0, 0 };
                                Array.Copy(temp, 0, mySeries[i].steps, bytJ * 320 + (j - 1) * 10, 7);
                            }
                        }
                   }
                }
                #endregion
                MyRead2UpFlags[4] = true;
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(99);

            //读取基本信息 网络信息
            if (intActivePage == 0)
            {
                // 读取串口232的配置信息
                #region
                for (int i = 0; i < 4; i++)
                {
                    ArayTmp = new byte[2];
                    ArayTmp[0] = 0x01;
                    ArayTmp[1] = Convert.ToByte(i);
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1372, bytSubID, bytDevID, false, true, true, false) == true)
                    {
                        switch (i)
                        {
                            case 0: Array.Copy(CsConst.myRevBuf, 27, ArayCom1, 0, 4); break;
                            case 1: Array.Copy(CsConst.myRevBuf, 27, ArayCom2, 0, 4); break;
                            case 2: Array.Copy(CsConst.myRevBuf, 27, ArayCom3, 0, 4); break;
                            case 3: Array.Copy(CsConst.myRevBuf, 27, ArayCom4, 0, 4); break;
                        }
                    }
                    else return false;
                }
                #endregion

                //读取继电器 干接点备注
                #region
                ArayTmp = new byte[2];
                ArayTmp[0] = 0x07;
                for (int i = 1; i <= 4; i++)
                {
                    ArayTmp[1] = Convert.ToByte(i);
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1372, bytSubID, bytDevID, false, true, true, false) == true)
                    {
                        byte[] arayRemark = new byte[20];
                        Array.Copy(CsConst.myRevBuf, 27, arayRemark, 0, 20);
                        string strRemark = HDLPF.Byte2String(arayRemark);

                        if (i == 1) strdry1 = strRemark;
                        else if (i == 2) strdry2 = strRemark;
                        else if (i == 3) strdry3 = strRemark;
                        else if (i == 4) strdry4 = strRemark;
                    }
                    else return false;
                }
                ArayTmp[0] = 0x08;
                for (int i = 1; i <= 2; i++)
                {
                    ArayTmp[1] = Convert.ToByte(i);
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1372, bytSubID, bytDevID, false, true, true, false) == true)
                    {
                        byte[] arayRemark = new byte[20];
                        Array.Copy(CsConst.myRevBuf, 27, arayRemark, 0, 20);
                        string strRemark = HDLPF.Byte2String(arayRemark);
                        if (i == 1) strrelay1 = strRemark;
                        else if (i == 2) strrelay2 = strRemark;
                    }
                    else return false;
                }
                #endregion
            } 
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(100);
            return true;
        }
    }
}
