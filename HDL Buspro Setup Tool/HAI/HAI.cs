using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.Windows.Forms;

namespace HDL_Buspro_Setup_Tool
{
    [Serializable]
    public class HAI : HdlDeviceBackupAndRestore
    {
        public int DIndex;//设备唯一编号
        //基本信息
        public string DeviceName;//子网ID 设备ID 备注

        internal List<Unit> units;
        internal List<Scene> scen;
        internal List<ButtonStatus> buttonstatus;

        public bool[] MyRead2UpFlags = new bool[2] { false, false }; //前面四个表示读取 后面表示上传

        //单元
        public class Unit
        {
            public byte bytID;
            public string Command;
            public string Remark;
            public UVCMD.ControlTargets oUnit; // 存放一个单路调节命令 
        }
        //场景
        public class Scene
        {
            public byte bytID;
            public string Command;
            public string Remark;
            public UVCMD.ControlTargets oUnit; // 存放一个场景控制命令 
        }
        //按键状态
        public class ButtonStatus
        {
            public byte bytID;
            public string Command;
            public string Remark;
            public UVCMD.ControlTargets oUnit; // 存放一个通用开关命令 
        }


        ///<summary>
        ///读取默认的设置
        ///</summary>
        public void ReadDefaultInfo()
        {
            if (units == null) units = new List<Unit>();
            if (scen == null) scen = new List<Scene>();
            if (buttonstatus == null) buttonstatus = new List<ButtonStatus>();
        }

        ///<summary>
        ///读取数据库面板设置，将所有数据读至缓存
        ///</summary>
        public void ReadHaiInfoFromDB(int DIndex)
        {
            //read HAI Unit inofomation
            units = new List<Unit>();
            #region
            string str = string.Format("select * from dbKeyTargets where DIndex ={0} and KeyIndex = {1} order by objID", DIndex, 1);
            OleDbDataReader drKeyCmds = DataModule.SearchAResultSQLDB(str);
            if (drKeyCmds != null)
            {
                while (drKeyCmds.Read())
                {
                    Unit oTmp = new Unit();
                    oTmp.bytID = drKeyCmds.GetByte(2);
                    UVCMD.ControlTargets TmpCmd = new UVCMD.ControlTargets();
                    oTmp.Command = "^A" + String.Format("{0:D3} ", drKeyCmds.GetByte(2));
                    TmpCmd.ID = 1;
                    TmpCmd.Type = drKeyCmds.GetByte(3);
                    TmpCmd.SubnetID = drKeyCmds.GetByte(4);
                    TmpCmd.DeviceID = drKeyCmds.GetByte(5);
                    TmpCmd.Param1 = drKeyCmds.GetByte(6);
                    TmpCmd.Param2 = drKeyCmds.GetByte(7);
                    TmpCmd.Param3 = drKeyCmds.GetByte(8);
                    TmpCmd.Param4 = drKeyCmds.GetByte(9);
                    oTmp.oUnit = TmpCmd;
                    units.Add(oTmp);
                }
            }
            #endregion

            scen = new List<Scene>();
            //read HAI Scene infomation 
            #region
            str = string.Format("select * from dbKeyTargets where DIndex ={0} and KeyIndex = {1} order by objID", DIndex, 2);
            drKeyCmds = DataModule.SearchAResultSQLDB(str);
            if (drKeyCmds != null)
            {
                while (drKeyCmds.Read())
                {
                    Scene oTmp = new Scene();
                    oTmp.bytID = drKeyCmds.GetByte(2);
                    UVCMD.ControlTargets TmpCmd = new UVCMD.ControlTargets();
                    oTmp.Command = "^C" + String.Format("{0:D3} ", drKeyCmds.GetByte(2));
                    TmpCmd.ID = 1;
                    TmpCmd.Type = drKeyCmds.GetByte(3);
                    TmpCmd.SubnetID = drKeyCmds.GetByte(4);
                    TmpCmd.DeviceID = drKeyCmds.GetByte(5);
                    TmpCmd.Param1 = drKeyCmds.GetByte(6);
                    TmpCmd.Param2 = drKeyCmds.GetByte(7);
                    TmpCmd.Param3 = drKeyCmds.GetByte(8);
                    TmpCmd.Param4 = drKeyCmds.GetByte(9);
                    oTmp.oUnit = TmpCmd;
                    scen.Add(oTmp);
                }
            }
            #endregion

            buttonstatus = new List<ButtonStatus>();
            ///read HAI buttonstatus infomation
            #region
            str = string.Format("select * from dbKeyTargets where DIndex ={0} and KeyIndex = {1} order by objID", DIndex, 3);
            drKeyCmds = DataModule.SearchAResultSQLDB(str);
            if (drKeyCmds != null)
            {
                while (drKeyCmds.Read())
                {
                    ButtonStatus oTmp = new ButtonStatus();
                    oTmp.bytID = drKeyCmds.GetByte(2);
                    UVCMD.ControlTargets TmpCmd = new UVCMD.ControlTargets();
                    oTmp.Command = String.Format("{0:D3} ", drKeyCmds.GetByte(2)) + " Button Tab/Press<-->OFF/ON";
                    TmpCmd.ID = 1;
                    TmpCmd.Type = drKeyCmds.GetByte(3);
                    TmpCmd.SubnetID = drKeyCmds.GetByte(4);
                    TmpCmd.DeviceID = drKeyCmds.GetByte(5);
                    TmpCmd.Param1 = drKeyCmds.GetByte(6);
                    TmpCmd.Param2 = drKeyCmds.GetByte(7);
                    TmpCmd.Param3 = drKeyCmds.GetByte(8);
                    TmpCmd.Param4 = drKeyCmds.GetByte(9);

                    oTmp.oUnit = TmpCmd;
                    buttonstatus.Add(oTmp);
                }
            }
            #endregion
        }
      

        ///<summary>
        ///保存数据库面板设置，将所有数据保存
        ///</summary>
        public void saveHaiInfoToDB(int DIndex)
        {
            /// delete all old information and refresh the database
            string strsql = string.Format("delete from dbKeyTargets where  DIndex={0}", DIndex);
            DataModule.ExecuteSQLDatabase(strsql);

            ///save save basic setup of HAI
            #region
            if (units != null && units.Count != 0)
            {
                foreach (Unit oTmp in units)
                {
                    if (oTmp.oUnit != null)
                    {
                        UVCMD.ControlTargets TmpCmds = oTmp.oUnit;
                        strsql = string.Format("Insert into dbKeyTargets(DIndex,KeyIndex,objID,KeyFunType,SubNetID,DeviceID,FirstParameter,"
                                                   + "SecondParameter,RunTimeMinute,RunTimeSecond,Ms04State) values ({0},{1},'{2}',{3},{4},{5},{6},{7},{8},{9},{10})",
                                                   DIndex, 1, oTmp.bytID, TmpCmds.Type, TmpCmds.SubnetID, TmpCmds.DeviceID, TmpCmds.Param1,
                                                   TmpCmds.Param2, TmpCmds.Param3, TmpCmds.Param4, 0);
                        DataModule.ExecuteSQLDatabase(strsql);
                    }
                }
            }
            #endregion

            //save infomation of Hai Scene
            #region
            if (scen != null && scen.Count != 0)
            {
                foreach (Scene oTmp in scen)
                {
                    if (oTmp.oUnit != null)
                    {
                        UVCMD.ControlTargets TmpCmds = oTmp.oUnit;
                        strsql = string.Format("Insert into dbKeyTargets(DIndex,KeyIndex,objID,KeyFunType,SubNetID,DeviceID,FirstParameter,"
                                                   + "SecondParameter,RunTimeMinute,RunTimeSecond,Ms04State,StrTip) values ({0},{1},'{2}',{3},{4},{5},{6},{7},{8},{9},{10})",
                                                   DIndex, 2, oTmp.bytID, TmpCmds.Type, TmpCmds.SubnetID, TmpCmds.DeviceID, TmpCmds.Param1,
                                                   TmpCmds.Param2, TmpCmds.Param3, TmpCmds.Param4, 0);
                        DataModule.ExecuteSQLDatabase(strsql);
                    }
                }
            }
            #endregion

            //save infomation of Hai ButtonStatus
            #region
            if (buttonstatus != null && buttonstatus.Count != 0)
            {
                foreach (ButtonStatus oTmp in buttonstatus)
                {
                    if (oTmp.oUnit != null)
                    {
                        UVCMD.ControlTargets TmpCmds = oTmp.oUnit;
                        strsql = string.Format("Insert into dbKeyTargets(DIndex,KeyIndex,objID,KeyFunType,SubNetID,DeviceID,FirstParameter,"
                                                   + "SecondParameter,RunTimeMinute,RunTimeSecond,Ms04State) values ({0},{1},'{2}',{3},{4},{5},{6},{7},{8},{9},{10})",
                                                   DIndex, 3, oTmp.bytID, TmpCmds.Type, TmpCmds.SubnetID, TmpCmds.DeviceID, TmpCmds.Param1,
                                                   TmpCmds.Param2, TmpCmds.Param3, TmpCmds.Param4, 0);
                        DataModule.ExecuteSQLDatabase(strsql);
                    }
                }
            }
            #endregion
        }

        /// <summary>
        ///上传设置
        /// </summary>
        public bool UploaDeviceFromBufferToDevice(string DevName)
        {
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

            if (CsConst.mySends.AddBufToSndList(ArayMain, 0x0010, bytSubID, bytDevID, false, true, true,false) == false)
            {
                return false;
            }

            //HAI unit to single light control
            #region
            if (units != null && units.Count != 0)
            {
                foreach (Unit oTmp in units)
                {
                    if (oTmp.oUnit != null)
                    {
                        UVCMD.ControlTargets TmpCmd = oTmp.oUnit;
                        byte[] arayCMD = new byte[10];
                        arayCMD[0] = 1;  //表示类型 HAI单元
                        arayCMD[1] = oTmp.bytID;
                        arayCMD[2] = 0;
                        arayCMD[3] = TmpCmd.Type;
                        arayCMD[4] = TmpCmd.SubnetID;
                        arayCMD[5] = TmpCmd.DeviceID;
                        arayCMD[6] = TmpCmd.Param1;
                        arayCMD[7] = TmpCmd.Param2;
                        arayCMD[8] = TmpCmd.Param3;   // save targets
                        arayCMD[9] = TmpCmd.Param4;
                        if (CsConst.mySends.AddBufToSndList(arayCMD, 0x023A, bytSubID, bytDevID, false, true, true, false) == false) return false;
                        HDLUDP.TimeBetwnNext(arayCMD.Length);
                    }
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(oTmp.bytID / 6, null);
                }
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(33, null);
            #endregion

            //HAI scene to scene
            #region
            if (scen != null && scen.Count != 0)
            {
                foreach (Scene oTmp in scen)
                {
                    if (oTmp.oUnit != null)
                    {
                        UVCMD.ControlTargets TmpCmd = oTmp.oUnit;
                        byte[] arayCMD = new byte[10];
                        arayCMD[0] = 2;  //表示类型 HAI单元
                        arayCMD[1] = oTmp.bytID;
                        arayCMD[2] = 0;
                        arayCMD[3] = TmpCmd.Type;
                        arayCMD[4] = TmpCmd.SubnetID;
                        arayCMD[5] = TmpCmd.DeviceID;
                        arayCMD[6] = TmpCmd.Param1;
                        arayCMD[7] = TmpCmd.Param2;
                        arayCMD[8] = TmpCmd.Param3;   // save targets
                        arayCMD[9] = TmpCmd.Param4;
                        if (CsConst.mySends.AddBufToSndList(arayCMD, 0x023A, bytSubID, bytDevID, false, true, true, false) == false) return false;
                        HDLUDP.TimeBetwnNext(arayCMD.Length);
                    }
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(33 + oTmp.bytID / 6, null);
                }
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(66, null);
            #endregion

            //HAI buttons to UV Switch
            #region
            if (buttonstatus != null && buttonstatus.Count != 0)
            {
                foreach (ButtonStatus oTmp in buttonstatus)
                {
                    if (oTmp.oUnit != null)
                    {
                        UVCMD.ControlTargets TmpCmd = oTmp.oUnit;
                        byte[] arayCMD = new byte[52];
                        arayCMD[0] = 3;  //表示类型 HAI单元
                        arayCMD[1] = oTmp.bytID;
                        arayCMD[2] = TmpCmd.Type;
                        arayCMD[3] = TmpCmd.SubnetID;
                        arayCMD[4] = TmpCmd.DeviceID;
                        arayCMD[5] = TmpCmd.Param1;
                        if (CsConst.mySends.AddBufToSndList(arayCMD, 0x023A, bytSubID, bytDevID, false, true, true, false) == false) return false;
                        HDLUDP.TimeBetwnNext(arayCMD.Length);
                    }
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(66 + oTmp.bytID / 6, null);
                }
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(100, null);
            #endregion
            MyRead2UpFlags[1] = true;
            return true;

        }

        /// <summary>
        ///下载设置
        /// </summary>
        public bool DownLoadInformationFrmDevice(string DevName)
        {
            string strMainRemark = DevName.Split('\\')[1].Trim();
            DevName = DevName.Split('\\')[0].Trim();
            //保存basic informations
            byte bytSubID = byte.Parse(DevName.Split('-')[0].ToString());
            byte bytDevID = byte.Parse(DevName.Split('-')[1].ToString());
            byte[] ArayMain = new byte[20];
            byte[] arayTmpRemark = HDLUDP.StringToByte(strMainRemark);
            Array.Copy(arayTmpRemark, 0, ArayMain, 0, arayTmpRemark.Length);

            if (CsConst.mySends.AddBufToSndList(ArayMain, 0x000E, bytSubID, bytDevID, false, true, true, false) == false)
            {
                return false;
            }
            else
            {
                byte[] arayRemark = new byte[20];
                for (int intI = 0; intI < 20; intI++) { arayRemark[intI] = CsConst.myRevBuf[25 + intI]; }
                DeviceName = bytSubID.ToString() + "-" + bytDevID.ToString() + "\\" + HDLPF.Byte2String(arayRemark);
            }

            //HAI unit to single light control
            #region
            units = new List<Unit>();

            for (byte bytI = 1 ;bytI<= 192;bytI++)
            {
                Unit oTmp = new Unit();
                oTmp.bytID = bytI;
                oTmp.Command = "^A" + String.Format("{0:D3} ", bytI);

                ArayMain = new byte[3];
                ArayMain[0] = 1;
                ArayMain[1] = bytI;
                ArayMain[2] = 0;

                if (CsConst.mySends.AddBufToSndList(ArayMain, 0x0238, bytSubID, bytDevID, false, true, true, false) == true)
                {
                    UVCMD.ControlTargets oCMD = new UVCMD.ControlTargets();
                    oCMD.ID = 1;
                    oCMD.Type = CsConst.myRevBuf[28];  //转换为正确的类型

                    if (oCMD.Type != 3 && oCMD.Type < 13)
                    {
                        oCMD.SubnetID = CsConst.myRevBuf[29];
                        oCMD.DeviceID = CsConst.myRevBuf[30];
                        oCMD.Param1 = CsConst.myRevBuf[31];
                        oCMD.Param2 = CsConst.myRevBuf[32];
                        oCMD.Param3 = CsConst.myRevBuf[33];
                        oCMD.Param4 = CsConst.myRevBuf[34];
                        
                        CsConst.myRevBuf = new byte[1200];
                        oTmp.oUnit = oCMD;
                    }
                }
                else return false;
                units.Add(oTmp);
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(bytI / 6, null);
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(33, null);
            #endregion

            //HAI scene to scene
            #region
            scen = new List<Scene>();

            for (byte bytI = 1; bytI <= 254; bytI++)
            {
                Scene oTmp = new Scene();
                oTmp.bytID = bytI;
                oTmp.Command = "^C" + String.Format("{0:D3} ", bytI);

                ArayMain = new byte[3];
                ArayMain[0] = 2;
                ArayMain[1] = bytI;
                ArayMain[2] = 0;

                if (CsConst.mySends.AddBufToSndList(ArayMain, 0x0238, bytSubID, bytDevID, false, true, true, false) == true)
                {
                    UVCMD.ControlTargets oCMD = new UVCMD.ControlTargets();
                    oCMD.ID = 1;
                    oCMD.Type = CsConst.myRevBuf[28];  //转换为正确的类型

                    if (oCMD.Type != 3 && oCMD.Type < 13)
                    {
                        oCMD.SubnetID = CsConst.myRevBuf[29];
                        oCMD.DeviceID = CsConst.myRevBuf[30];
                        oCMD.Param1 = CsConst.myRevBuf[31];
                        oCMD.Param2 = CsConst.myRevBuf[32];
                        oCMD.Param3 = CsConst.myRevBuf[33];
                        oCMD.Param4 = CsConst.myRevBuf[34];
                        CsConst.myRevBuf = new byte[1200];
                        oTmp.oUnit = oCMD;
                    }
                }
                else return false;
                scen.Add(oTmp);
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(33 + bytI / 6, null);
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(66, null);
            #endregion

            //HAI buttons to UV switch
            #region
            buttonstatus = new List<ButtonStatus>();

            for (byte bytI = 1; bytI <= 192; bytI++)
            {
                ButtonStatus oTmp = new ButtonStatus();
                oTmp.bytID = bytI;
                oTmp.Command = String.Format("{0:D3} ", bytI) + " Button Tab/Press<-->OFF/ON";

                ArayMain = new byte[2];
                ArayMain[0] = 3;
                ArayMain[1] = bytI;

                if (CsConst.mySends.AddBufToSndList(ArayMain, 0x0238, bytSubID, bytDevID, false, true, true, false) == true)
                {
                    UVCMD.ControlTargets oCMD = new UVCMD.ControlTargets();
                    oCMD.ID = 1;
                    if (CsConst.myRevBuf[27] == 1) oCMD.Type = 2;  //转换为正确的类型
                    else oCMD.Type = 3;

                    if (oCMD.Type != 3)
                    {
                        oCMD.SubnetID = CsConst.myRevBuf[28];
                        oCMD.DeviceID = CsConst.myRevBuf[29];
                        oCMD.Param1 = CsConst.myRevBuf[30];
                        oCMD.Param2 = 0;
                        oCMD.Param3 = 0;
                        oCMD.Param4 = 0;
                        CsConst.myRevBuf = new byte[1200];
                        oTmp.oUnit = oCMD;
                    }
                }
                else return false;
                buttonstatus.Add(oTmp);
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(66 + bytI / 6, null);
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(100, null);
            #endregion
            MyRead2UpFlags[0] = true;
            return true;

        }

    }
}
