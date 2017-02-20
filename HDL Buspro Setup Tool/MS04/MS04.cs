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
    public class MS04 : HdlDeviceBackupAndRestore
    {
        public int DIndex;  ////设备唯一编号
        public string DeviceName;
        internal List<Channel> ChnList;
        internal List<MS04Key> MSKeys = null;  //// 红外码库
        internal List<UVCMD.SecurityInfo> myKeySeu = null;
        internal List<InputRange> myRange;
        public Byte dimmerLow;
        public Boolean[] MyRead2UpFlags = new bool[16];
        public Byte[] arayUVStaus = new byte[36];
        public Byte bytVotoge;
        public byte bytChannelStatus;
        public Byte bytDoorBellRunTime;
        public Byte[] arayCurtain = new byte[6];
        public Byte[] arayOutput = new byte[2];
        [Serializable]
        internal class InputRange
        {
            public byte ID;
            public byte InputType;
            public byte Connection;
            public byte AnalogType;
            public byte Common;
            public byte Broadcat;
            public int Range1;
            public int Range2;
            public int Min;
            public int Max;
            public int Count;
            public int[] arayPoint;
        }

        //<summary>
        //读取默认的MS04设置，将所有数据读取缓存
        //</summary>
        public virtual void ReadDefaultInfo(int intDeviceType)
        {
            ChnList = new List<Channel>();
            MSKeys = new List<MS04Key>();
            myKeySeu = new List<UVCMD.SecurityInfo>();
            myRange = new List<InputRange>();
        }

        //<summary>
        //读取数据库面板设置，将所有数据读至缓存
        //</summary>
        public virtual void ReadMS04FrmDBTobuf(int DIndex, int wdMaxValue)
        {
            try
            {
                MSKeys = new List<MS04Key>();
                // read keys settings to buffer
                #region
                for (int intI = 0; intI < wdMaxValue; intI++)
                {
                    string str = string.Format("select * from dbMS04 where DIndex={0} and KeyID ={1} order by KeyStatus", DIndex, intI + 1);
                    OleDbDataReader dr = DataModule.SearchAResultSQLDB(str, CsConst.mstrCurPath);

                    if (dr == null) return;
                    MS04Key TmpKey = new MS04Key(); // 定义为空
                    while (dr.Read())
                    {
                        TmpKey.ID = dr.GetByte(1);
                        TmpKey.Remark = dr.GetString(2);
                        TmpKey.Mode = dr.GetByte(3);
                        TmpKey.bytEnable = dr.GetByte(4);
                        TmpKey.bytReflag = dr.GetByte(5); //要不要锁键
                        TmpKey.IsDimmer = dr.GetByte(6);
                        TmpKey.ONOFFDelay = (int[])dr[7];
                        TmpKey.SaveDimmer = dr.GetByte(8);

                        TmpKey.KeyTargets = new List<UVCMD.ControlTargets>();
                        ////read keys commands to buffer
                        #region
                        str = string.Format("select * from dbKeyTargets where DIndex ={0} and KeyIndex = {1}", DIndex, TmpKey.ID);
                        OleDbDataReader drKeyCmds = DataModule.SearchAResultSQLDB(str, CsConst.mstrCurPath);
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
                        drKeyCmds.Close();
                        #endregion

                    }
                    MSKeys.Add(TmpKey);
                    dr.Close();
                }
                #endregion

                //读取安防设置
                #region
                myKeySeu = new List<UVCMD.SecurityInfo>();
                string strsql = string.Format("select * from dbSeuOnly where DIndex={0} order by SeuID", DIndex);
                OleDbDataReader drSecu = DataModule.SearchAResultSQLDB(strsql, CsConst.mstrCurPath);
                if (drSecu != null)
                {
                    while (drSecu.Read())
                    {
                        UVCMD.SecurityInfo temp = new UVCMD.SecurityInfo();
                        temp.bytSeuID = drSecu.GetByte(8);
                        temp.bytTerms = drSecu.GetByte(1);
                        temp.strRemark = drSecu.GetString(9);
                        temp.bytSubID = drSecu.GetByte(2);
                        temp.bytDevID = drSecu.GetByte(3);
                        temp.bytArea = drSecu.GetByte(4);

                        myKeySeu.Add(temp);
                    }
                    drSecu.Close();
                }
                #endregion
            }
            catch
            {
            }

        }

        //<summary>
        //保存数据库面板设置，将所有数据保存
        //</summary>
        public virtual void SaveSendIRToDB(int intDeviceType)
        {
            try
            {
                //// delete all old information and refresh the database
                string strsql = string.Format("delete * from dbMS04 where DIndex={0}", DIndex);
                DataModule.ExecuteSQLDatabase(strsql);

                strsql = string.Format("delete * from dbKeyTargets where DIndex={0}", DIndex);
                DataModule.ExecuteSQLDatabase(strsql);

                strsql = string.Format("delete * from dbSeuOnly where DIndex={0}", DIndex);
                DataModule.ExecuteSQLDatabase(strsql);

                //// insert new commands to database
                if (MSKeys == null) return;
                for (int intJ = 0; intJ < MSKeys.Count; intJ++)
                {
                    MS04Key TmpKey = new MS04Key();
                    TmpKey = MSKeys[intJ];
                    String DelayString = TmpKey.ONOFFDelay[0].ToString() + "-" + TmpKey.ONOFFDelay[1];
                    #region                        
                    strsql = string.Format("Insert into dbMS04(DIndex,KeyID,Remark,KeyMode,IsEnable,IsReWork,WorkMode,DelayTime,IsSaveD) values "
                                            + "({0},{1},'{2}',{3},{4},{5},{6},'{7}',{8})", DIndex, TmpKey.ID, TmpKey.Remark,
                                            TmpKey.Mode, TmpKey.bytEnable, TmpKey.bytReflag, TmpKey.IsDimmer, DelayString, TmpKey.SaveDimmer);
                    DataModule.ExecuteSQLDatabase(strsql);

                    if (TmpKey.KeyTargets != null)
                    {
                        for (int intK = 0; intK < TmpKey.KeyTargets.Count; intK++)
                        {
                            UVCMD.ControlTargets TmpCmds = TmpKey.KeyTargets[intK];
                            ///// insert into all commands to database
                            strsql = string.Format("Insert into dbKeyTargets(DIndex,KeyIndex,objID,KeyFunType,SubNetID,DeviceID,FirstParameter,"
                                            + "SecondParameter,RunTimeMinute,RunTimeSecond,Ms04State) values ({0},{1},{2},{3},{4},{5},{6},{7},{8},{9})",
                                            DIndex, TmpKey.ID, TmpCmds.ID, TmpCmds.Type, TmpCmds.SubnetID, TmpCmds.DeviceID, TmpCmds.Param1,
                                            TmpCmds.Param2, TmpCmds.Param3, TmpCmds.Param4);
                            DataModule.ExecuteSQLDatabase(strsql);
                        }
                    }
                    #endregion
                }

                //save Fire Info
                #region
                if (myKeySeu != null)
                {
                    byte byt = 1;
                    foreach (UVCMD.SecurityInfo senfire in myKeySeu)
                    {
                        strsql = string.Format("insert into dbSeuOnly(DIndex,SeuID,Terms,SeuRemark,SubID,DevID,AreaNo)"
                            + "values({0},{1},{2},'{3}',{4},{5},{6})", DIndex, byt, senfire.bytTerms.ToString(), senfire.strRemark, senfire.bytSubID, senfire.bytDevID, senfire.bytArea);
                        DataModule.ExecuteSQLDatabase(strsql);
                        byt++;
                    }
                }
                #endregion
            }
            catch
            {
            }
        }

        public virtual void UploadMS04ToDevice(string strDevName, int intDeviceType, int intActivePage, int num1, int num2)// 0 mean all, else that tab only
        {
            string strMainRemark = strDevName.Split('\\')[1].Trim();
            String strTmpDevName = strDevName.Split('\\')[0].Trim();

            byte bytSubID = byte.Parse(strTmpDevName.Split('-')[0].ToString());
            byte bytDevID = byte.Parse(strTmpDevName.Split('-')[1].ToString());

            if (HDLSysPF.ModifyDeviceMainRemark(bytSubID,bytDevID,strMainRemark,intDeviceType) == false)
            {
                return;
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(1);

            int wdMaxValue = DeviceTypeList.GetMaxValueFromPublicModeGroup(intDeviceType);

            byte[] arayKeyEanble = new byte[wdMaxValue];
            byte[] arayKeyType = new byte[wdMaxValue];
            byte[] arayKeyMode = new byte[wdMaxValue];
            byte[] arayKeySave = new byte[wdMaxValue];
            byte[] arayKeyReflag = new byte[wdMaxValue];


            if (intActivePage == 0 || intActivePage == 3)
            {
                if (MS04DeviceTypeList.MS04HotelMixModule.Contains(intDeviceType))
                {
                    #region
                    byte[] arayLoadType = new byte[12];
                    byte[] arayOnDelay = new byte[12];
                    byte[] arayProDelay = new byte[12];
                    byte[] arayLimitL = new byte[12 + 1]; arayLimitL[0] = 0;
                    byte[] arayLimitH = new byte[12 + 1]; arayLimitH[0] = 1;
                    byte[] arayMaxLevel = new byte[12];
                    byte[] AutoCloseEnable = new byte[12];
                    byte[] AutoCloseDelay = new byte[12 * 2];
                    byte[] arayDimmingProfile = new byte[12 + 2]; arayDimmingProfile[13] = bytVotoge;
                    foreach (Channel ch in ChnList)
                    {   // modify the chns remark
                        byte[] arayRemark = new byte[21]; // 初始化数组
                        string strRemark = ch.Remark;
                        byte[] arayTmp = HDLUDP.StringToByte(strRemark);

                        arayRemark[0] = byte.Parse(ch.ID.ToString());
                        if (arayTmp != null) arayTmp.CopyTo(arayRemark, 1);
                        if (intActivePage == 0 || intActivePage == 3 || intActivePage == 8)
                        {
                            CsConst.mySends.AddBufToSndList(arayRemark, 0xF010, bytSubID, bytDevID, false, true, true, false);
                            CsConst.myRevBuf = new byte[1200];
                            HDLUDP.TimeBetwnNext(20);
                        }
                        else return;
                        arayOnDelay[ch.ID - 1] = byte.Parse(ch.PowerOnDelay.ToString());
                        arayProDelay[ch.ID - 1] = byte.Parse(ch.ProtectDealy.ToString());
                        arayLoadType[ch.ID - 1] = byte.Parse(ch.LoadType.ToString());
                        arayMaxLevel[ch.ID - 1] = byte.Parse(ch.MaxLevel.ToString());
                        arayLimitL[ch.ID] = byte.Parse(ch.MinValue.ToString());
                        arayLimitH[ch.ID] = byte.Parse(ch.MaxValue.ToString());
                    }
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(2);

                    // modify the load type
                    if (CsConst.mySends.AddBufToSndList(arayLoadType, 0xF014, bytSubID, bytDevID, false, true, true, false) == false) return;
                    CsConst.myRevBuf = new byte[1200];
                    HDLUDP.TimeBetwnNext(20);
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(3);

                    // modify the on Low limit
                    if (CsConst.mySends.AddBufToSndList(arayLimitL, 0xF018, bytSubID, bytDevID, false, true, true, false) == false) return;
                    CsConst.myRevBuf = new byte[1200];
                    HDLUDP.TimeBetwnNext(20);
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(4);

                    // modify the High Limit
                    if (CsConst.mySends.AddBufToSndList(arayLimitH, 0xF018, bytSubID, bytDevID, false, true, true, false) == false) return;
                    CsConst.myRevBuf = new byte[1200];
                    HDLUDP.TimeBetwnNext(20);
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(5);

                    // modify the max level 
                    if (CsConst.mySends.AddBufToSndList(arayMaxLevel, 0xF022, bytSubID, bytDevID, false, true, true, false) == false) return;
                    CsConst.myRevBuf = new byte[1200];
                    HDLUDP.TimeBetwnNext(20);
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(6);

                    // modify the on delay 
                    if (CsConst.mySends.AddBufToSndList(arayOnDelay, 0xF04F, bytSubID, bytDevID, false, true, true, false) == false) return;
                    CsConst.myRevBuf = new byte[1200];
                    HDLUDP.TimeBetwnNext(20);
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(7);

                    // modify the protect delay
                    if (CsConst.mySends.AddBufToSndList(arayProDelay, 0xF041, bytSubID, bytDevID, false, true, true, false) == false) return;
                    CsConst.myRevBuf = new byte[1200];
                    HDLUDP.TimeBetwnNext(20);
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(8);

                    if (CsConst.mySends.AddBufToSndList(arayDimmingProfile, 0x1804, bytSubID, bytDevID, false, true, true, false) == false) return;
                    CsConst.myRevBuf = new byte[1200];
                    HDLUDP.TimeBetwnNext(20);
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(9);
                    #endregion
                }
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(10);
            }

            if (intActivePage == 0 || intActivePage == 1)
            {
                #region
                for (int intI = 0; intI < wdMaxValue; intI++)
                {
                    arayKeyEanble[intI] = MSKeys[intI].bytEnable;
                    arayKeyMode[intI] = (byte)(MSKeys[intI].Mode);
                    arayKeyType[intI] = MSKeys[intI].IsDimmer;  //调光方向
                    arayKeySave[intI] = MSKeys[intI].SaveDimmer;
                    arayKeyReflag[intI] = MSKeys[intI].bytReflag;
                }

                if (intDeviceType != 116 && intDeviceType != 363)
                {
                    if (CsConst.mySends.AddBufToSndList(arayKeyEanble, 0x012A, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(intDeviceType)) == true)
                    {
                        CsConst.myRevBuf = new byte[1200];
                        HDLUDP.TimeBetwnNext(20);
                    }
                    else return;
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(11);
                }

                if (CsConst.mySends.AddBufToSndList(arayKeyMode, 0xD207, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(intDeviceType)) == true) // 按键类型机械电子
                {
                    CsConst.myRevBuf = new byte[1200];
                    HDLUDP.TimeBetwnNext(20);
                }
                else return;
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(12);

                if (intDeviceType != 116 && intDeviceType != 363)
                {
                    if (SaveButtonDimDirectionsToDeviceFrmBuf(bytSubID, bytDevID, intDeviceType) == false) return;

                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(13);

                    if (intDeviceType == 119 || intDeviceType == 137 || intDeviceType == 135 || intDeviceType == 121)
                    {
                        
                        if (SaveButtonDimFlagToDeviceFrmBuf(bytSubID, bytDevID, intDeviceType) == true)  //调光保存使能位
                        {
                            CsConst.myRevBuf = new byte[1200];
                            HDLUDP.TimeBetwnNext(20);
                        }
                        else return;
                        if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(14);
                    }

                    if (CsConst.mySends.AddBufToSndList(arayKeyReflag, 0x015A, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(intDeviceType)) == true)
                    {
                        CsConst.myRevBuf = new byte[1200];
                        HDLUDP.TimeBetwnNext(20);
                    }
                    else return;
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(15);

                    // write low Limit 
                    byte[] ArayTmp = new byte[1];
                    ArayTmp[0] = dimmerLow;
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xE0E2, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(intDeviceType)) == true)
                    {
                        CsConst.myRevBuf = new byte[1200];
                        HDLUDP.TimeBetwnNext(20);
                    }
                    else return;
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(16);
                }
                #endregion
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(20);
            }

            if (intActivePage == 0 || intActivePage == 1)
            {
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
                    if (CsConst.mySends.AddBufToSndList(arayKeyRemark, 0xD220, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(intDeviceType)) == true)
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
                           #region
                            if (intDeviceType == 116)
                            {
                                if (bytI != 1)
                                {
                                    arayKeyRemark = new byte[3];                     //延时
                                    arayKeyRemark[0] = Convert.ToByte(tmp.ID - 1);
                                    arayKeyRemark[1] = byte.Parse((tmp.ONOFFDelay[0] / 256).ToString());
                                    arayKeyRemark[2] = byte.Parse((tmp.ONOFFDelay[0] % 256).ToString());
                                    if (CsConst.mySends.AddBufToSndList(arayKeyRemark, 0xD20C, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(intDeviceType)) == true)
                                    {
                                        CsConst.myRevBuf = new byte[1200];
                                        HDLUDP.TimeBetwnNext(20);
                                    }
                                    else return;
                                }

                            }
                            else
                            {
                                arayKeyRemark = new byte[4];                     //延时
                                arayKeyRemark[0] = Convert.ToByte(tmp.ID - 1);
                                arayKeyRemark[1] = bytI;
                                arayKeyRemark[2] = byte.Parse((tmp.ONOFFDelay[bytI] / 256).ToString());
                                arayKeyRemark[3] = byte.Parse((tmp.ONOFFDelay[bytI] % 256).ToString());
                                if (CsConst.mySends.AddBufToSndList(arayKeyRemark, 0xD20C, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(intDeviceType)) == true)
                                {
                                    CsConst.myRevBuf = new byte[1200];
                                    HDLUDP.TimeBetwnNext(20);
                                }
                                else return;
                            }
                            #endregion
                        }

                        if (CsConst.isRestore)
                        {
                            if (MS04DeviceTypeList.MS04DryContactWithE000PublicCMD.Contains(intDeviceType))//无线新命令
                            {
                                tmp.UploadButtonRemarkAndCMDToDevice(bytSubID, bytDevID, intDeviceType, 0, 1);
                            }
                            else if (MS04DeviceTypeList.MS04IOModuleDeviceTypeList.Contains(intDeviceType))  //六路输入输出模块
                            {
                                ModifyIOModuleCommandsGroupFrmDeviceToBuf(bytSubID, bytDevID, intDeviceType, tmp.ID, 1);
                            }
                            else
                            {
                                ModifyDryCommandsGroupFrmDeviceToBuf(bytSubID, bytDevID, intDeviceType, tmp.ID, 0);
                            }
                        }
                        #endregion
                    }
                    else
                    {
                        #region
                        if (CsConst.isRestore)
                        {
                            if (MS04DeviceTypeList.MS04DryContactWithE000PublicCMD.Contains(intDeviceType))//无线新命令
                            {
                                tmp.UploadButtonRemarkAndCMDToDevice(bytSubID, bytDevID, intDeviceType, 0, 1);
                            }
                            else if (MS04DeviceTypeList.MS04IOModuleDeviceTypeList.Contains(intDeviceType))  //六路输入输出模块
                            {
                                ModifyIOModuleCommandsGroupFrmDeviceToBuf(bytSubID, bytDevID, intDeviceType, tmp.ID, 0);
                            }
                            else
                            {
                                ModifyDryCommandsGroupFrmDeviceToBuf(bytSubID, bytDevID, intDeviceType, tmp.ID,1);
                            }
                        }
                        #endregion
                    }
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(20 + bytCount * 10 / MSKeys.Count);
                    bytCount++;
                }
                #endregion
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(30);
            }

            if (intActivePage == 0 || intActivePage == 2)
            {
                #region
                if (intDeviceType != 136 && intDeviceType != 142 && intDeviceType != 352 && intDeviceType != 353
                    && intDeviceType != 116 && intDeviceType != 363)
                {
                    //安防功能
                    byte bytCount = 0;
                    if (myKeySeu != null && myKeySeu.Count != 0)
                    {
                        foreach (UVCMD.SecurityInfo oTmp in myKeySeu)
                        {
                            byte[] arayKeyRemark = new byte[22];
                            arayKeyRemark[0] = Convert.ToByte(oTmp.bytSeuID - 1 + wdMaxValue);
                            arayKeyRemark[1] =0;
                            byte[] arayTmp = new byte[20];
                            if (oTmp.strRemark != null)
                            {
                                arayTmp = HDLUDP.StringToByte(oTmp.strRemark);
                            }

                            HDLSysPF.CopyRemarkBufferToSendBuffer(arayTmp, arayKeyRemark, 2);
                            if (CsConst.mySends.AddBufToSndList(arayKeyRemark, 0xD220, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(intDeviceType)) == true)
                            {
                                CsConst.myRevBuf = new byte[1200];
                                HDLUDP.TimeBetwnNext(20);
                            }
                            else return;

                            byte[] ArayTmp = new byte[5];
                            //修改设置
                            ArayTmp[0] = oTmp.bytSeuID;
                            ArayTmp[1] = oTmp.bytTerms;
                            ArayTmp[2] = oTmp.bytSubID;
                            ArayTmp[3] = oTmp.bytDevID;
                            ArayTmp[4] = oTmp.bytArea;
                          
                            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x15DC, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(intDeviceType)) == true)
                            {
                                CsConst.myRevBuf = new byte[1200];
                                HDLUDP.TimeBetwnNext(20);
                            }
                            else return;
                            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(30 + (bytCount * 10 / myKeySeu.Count));
                            bytCount++;
                        }
                    }
                }
                #endregion
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(40);
            }

            if (intActivePage == 0 || intActivePage == 4)
            {
                if (MS04DeviceTypeList.MS04HotelMixModule.Contains(intDeviceType))
                {
                    #region
                    //上传电压
                    #region
                    byte[] arayDimmingProfile = new byte[12 + 2]; arayDimmingProfile[13] = bytVotoge;

                    if (CsConst.mySends.AddBufToSndList(arayDimmingProfile, 0x1804, bytSubID, bytDevID, false, true, true, false) == false) return;
                    CsConst.myRevBuf = new byte[1200];
                    HDLUDP.TimeBetwnNext(20);
                    #endregion
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(41);
                    //上传窗帘
                    #region
                    byte[] arayTmp = new byte[1];
                    arayTmp[0] = arayCurtain[0];
                    if (CsConst.mySends.AddBufToSndList(arayTmp, 0x1F20, bytSubID, bytDevID, false, true, true, false) == false) return;
                    CsConst.myRevBuf = new byte[1200];
                    HDLUDP.TimeBetwnNext(20);

                    arayTmp = new byte[3];
                    arayTmp[0] = 1;
                    arayTmp[1] = arayCurtain[1];
                    arayTmp[2] = arayCurtain[2];
                    if (CsConst.mySends.AddBufToSndList(arayTmp, 0xE802, bytSubID, bytDevID, false, true, true, false) == false) return;
                    CsConst.myRevBuf = new byte[1200];
                    HDLUDP.TimeBetwnNext(20);
                    #endregion
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(42);
                    //上传门铃
                    #region
                    arayTmp = new byte[1];
                    arayTmp[0] = bytDoorBellRunTime;
                    if (CsConst.mySends.AddBufToSndList(arayTmp, 0x1F24, bytSubID, bytDevID, false, true, true, false) == false) return;
                    CsConst.myRevBuf = new byte[1200];
                    HDLUDP.TimeBetwnNext(20);
                    #endregion
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(43);
                    //上传LED
                    #region
                    for (byte i = 1; i <= 3; i++)
                    {
                        arayTmp = new byte[8];
                        arayTmp[0] = i;
                        arayTmp[1] = 0;
                        Array.Copy(arayUVStaus, (i - 1) * 12, arayTmp, 2, 6);
                        if (CsConst.mySends.AddBufToSndList(arayTmp, 0x201A, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(intDeviceType)) == false) return;
                        CsConst.myRevBuf = new byte[1200];
                        HDLUDP.TimeBetwnNext(20);

                        arayTmp[1] = 1;
                        Array.Copy(arayUVStaus, (i - 1) * 12 + 6, arayTmp, 2, 6);
                        if (CsConst.mySends.AddBufToSndList(arayTmp, 0x201A, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(intDeviceType)) == false) return;
                        CsConst.myRevBuf = new byte[1200];
                        HDLUDP.TimeBetwnNext(20);
                        if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(43 + i);
                    }
                    #endregion
                    #endregion
                }
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(50);
            }

            if (intActivePage == 0 || intActivePage == 5)
            {
                if (MS04DeviceTypeList.MS04IOModuleDeviceTypeList.Contains(intDeviceType))
                {
                    #region
                    if (myRange != null)
                    {
                        for (int i = 0; i < myRange.Count; i++)
                        {
                            #region
                            InputRange temp = myRange[i];
                            if (temp.ID == num1 || CsConst.isRestore || num1 ==0)
                            {
                                byte[] arayTmp = new byte[16];
                                arayTmp[0] = Convert.ToByte(temp.ID);
                                arayTmp[1] = temp.InputType;
                                arayTmp[2] = temp.Connection;
                                arayTmp[3] = temp.AnalogType;
                                arayTmp[4] = temp.Common;
                                arayTmp[5] = 1;
                                arayTmp[6] = Convert.ToByte(temp.Range1 % 256);
                                arayTmp[7] = Convert.ToByte(temp.Range1 / 256);
                                arayTmp[8] = Convert.ToByte(temp.Range2 % 256);
                                arayTmp[9] = Convert.ToByte(temp.Range2 / 256);
                                if (temp.AnalogType == 0)
                                {
                                    int intTmp = 0;
                                    if (temp.Min < 0) intTmp = Math.Abs(temp.Min) | 32768;
                                    else intTmp = temp.Min;
                                    arayTmp[10] = Convert.ToByte(intTmp % 256);
                                    arayTmp[11] = Convert.ToByte(intTmp / 256);
                                    if (temp.Max < 0) intTmp = Math.Abs(temp.Max) | 32768;
                                    else intTmp = temp.Max;
                                    arayTmp[12] = Convert.ToByte(intTmp % 256);
                                    arayTmp[13] = Convert.ToByte(intTmp / 256);
                                }
                                else
                                {
                                    arayTmp[10] = Convert.ToByte(temp.Min % 256);
                                    arayTmp[11] = Convert.ToByte(temp.Min / 256);
                                    arayTmp[12] = Convert.ToByte(temp.Max % 256);
                                    arayTmp[13] = Convert.ToByte(temp.Max / 256);
                                }
                                arayTmp[14] = Convert.ToByte(temp.Count % 256);
                                arayTmp[15] = Convert.ToByte(temp.Count / 256);
                                CsConst.MyBlnNeedF8 = true;
                                if (CsConst.mySends.AddBufToSndList(arayTmp, 0x3515, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(intDeviceType)) == true)
                                {
                                    HDLUDP.TimeBetwnNext(70);
                                }
                                else
                                {
                                    CsConst.MyBlnNeedF8 = false;
                                    return;
                                }
                                CsConst.MyBlnNeedF8 = false;
                                if (temp.InputType == 2 || temp.InputType == 4)
                                {
                                    int BagCount = 0;
                                    BagCount = (temp.Count + 1) / 25;
                                    if ((temp.Count + 1) % 25 != 0) BagCount = BagCount + 1;
                                    byte byt = 0;
                                    for (int j = 0; j < BagCount; j++)
                                    {
                                        arayTmp = new byte[52];
                                        arayTmp[0] = Convert.ToByte(temp.ID);
                                        arayTmp[1] = byt;
                                        for (int k = 0; k < 25; k++)
                                        {
                                            if (k + j * 25 < temp.arayPoint.Length)
                                            {
                                                int intTmp = temp.arayPoint[k + j * 25];
                                                arayTmp[2 * k + 2] = Convert.ToByte(intTmp % 256);
                                                arayTmp[2 * k + 1 + 2] = Convert.ToByte(intTmp / 256);
                                            }
                                        }
                                        if (CsConst.mySends.AddBufToSndList(arayTmp, 0x351D, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(intDeviceType)) == true)
                                        {

                                            HDLUDP.TimeBetwnNext(70);
                                        }
                                        else return;
                                        byt++;
                                    }

                                }
                                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(50 + i);
                            }
                            #endregion

                            //修改常量保存标志
                            #region
                            if (myRange[i].InputType == 2 || myRange[i].InputType == 4) // 类比，有广播使能位选择
                            {
                                Byte[] arayReadBroadcastFlag = new Byte[8];
                                Byte bBroadcastFlag = myRange[i].Broadcat;
                                switch (myRange[i].AnalogType)
                                {
                                    case 0: arayReadBroadcastFlag = new Byte[] { 1, (Byte)(i + 1), bBroadcastFlag, 0, 0, 0, 0, 0 }; break;
                                    case 1: arayReadBroadcastFlag = new Byte[] { 0, (Byte)(i + 1), bBroadcastFlag, 0, 0, 0, 0, 0 }; break;
                                    case 2: arayReadBroadcastFlag = new Byte[] { 5, (Byte)(i + 1), bBroadcastFlag, 0, 0, 0, 0, 0 }; break;
                                    case 3: arayReadBroadcastFlag = new Byte[] { 4, (Byte)(i + 1), bBroadcastFlag, 0, 0, 0, 0, 0 }; break;
                                }
                                if (CsConst.mySends.AddBufToSndList(arayReadBroadcastFlag, 0xE446, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(intDeviceType)) == true)
                                {
                                    myRange[i].Broadcat = CsConst.myRevBuf[27];
                                    HDLUDP.TimeBetwnNext(1);
                                }
                            }
                            #endregion
                        }
                    }
                    #endregion
                }
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(60);
            }
            if (intActivePage == 0 || intActivePage == 6)
            {
                if (MS04DeviceTypeList.MS04IOModuleDeviceTypeList.Contains(intDeviceType))
                {
                    #region
                    byte[] arayTmp = new byte[2];
                    for (byte i = 0; i <= 1; i++)
                    {
                        arayTmp = new byte[2];
                        arayTmp[0] = Convert.ToByte(i + 7);
                        arayTmp[1] = arayOutput[i];
                        if (CsConst.mySends.AddBufToSndList(arayTmp, 0x3519, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(intDeviceType)) == true)
                        {

                            HDLUDP.TimeBetwnNext(70);
                        }
                        else return;
                        if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(60 + i);
                    }
                    arayTmp = new byte[1];
                    arayTmp[0] = bytChannelStatus;
                    if (CsConst.mySends.AddBufToSndList(arayTmp, 0x3522, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(intDeviceType)) == true)
                    {

                        HDLUDP.TimeBetwnNext(70);
                    }
                    else return;
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(62);
                    #endregion
                }
            }

            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(100);
        }

        /// <summary>
        /// strDevName device subnet id and device id
        /// </summary>
        /// <param name="strDevName"></param>
        public virtual void DownLoadInformationFrmDevice(string strDevName, int intDeviceType, int intActivePage, int num1, int num2)// 0 mean all, else that tab only
        {
            Boolean blnIsSuccess = false;
            if (strDevName == null) return;
            string strMainRemark = strDevName.Split('\\')[1].Trim();
            string strTmpDevName = strDevName.Split('\\')[0].Trim();

            byte bytSubID = byte.Parse(strTmpDevName.Split('-')[0].ToString());
            byte bytDevID = byte.Parse(strTmpDevName.Split('-')[1].ToString());
            byte[] ArayTmp = null;
            String Remark = HDLSysPF.ReadDeviceMainRemark(bytSubID, bytDevID);
            DeviceName = bytSubID.ToString() + "-" + bytDevID.ToString() + "\\" + Remark;

            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(1);
            int wdMaxValue = DeviceTypeList.GetMaxValueFromPublicModeGroup(intDeviceType);

            if (intActivePage == 0 || intActivePage == 3) //读取回路设置
            {
                if (MS04DeviceTypeList.MS04HotelMixModule.Contains(intDeviceType))
                {
                    #region
                    //读取回路备注
                    int ChannelCount = 12;
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

                        ArayTmp[0] = (byte)(i + 1);
                        if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xF00E, bytSubID, bytDevID, false, true, true, false) == true)
                        {
                            if (CsConst.myRevBuf != null)
                            {
                                byte[] arayRemark = new byte[20];
                                Array.Copy(CsConst.myRevBuf, 26, arayRemark, 0, 20);
                                ch.Remark = HDLPF.Byte2String(arayRemark);
                            }
                            CsConst.myRevBuf = new byte[1200];
                            HDLUDP.TimeBetwnNext(1);
                        }
                        else
                        {
                            return;
                        }
                        ChnList.Add(ch);
                    }
                    #endregion
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(2);

                    //读取回路信息
                    #region
                    ArayTmp = null;
                    // read load type
                    #region
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xF012, bytSubID, bytDevID, false, true, true, false) == true)
                    {
                        for (int intI = 0; intI < ChannelCount; intI++)
                        {
                            ChnList[intI].LoadType = CsConst.myRevBuf[25 + intI];

                            if (ChnList[intI].LoadType == 255 || ChnList[intI].LoadType > CsConst.LoadType.Length - 1) ChnList[intI].LoadType = 0;
                        }
                        CsConst.myRevBuf = new byte[1200];
                        HDLUDP.TimeBetwnNext(1);
                    }
                    else return;
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(3, null);
                    #endregion

                    // read low limit
                    #region
                    ArayTmp = new byte[1];
                    ArayTmp[0] = 0;
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xF016, bytSubID, bytDevID, false, true, true, false) == true)
                    {
                        for (int intI = 0; intI < ChannelCount; intI++)
                        {
                            ChnList[intI].MinValue = CsConst.myRevBuf[26 + intI];
                        }
                        CsConst.myRevBuf = new byte[1200];
                        HDLUDP.TimeBetwnNext(20);
                    }
                    else return;
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(4, null);
                    #endregion

                    // read high limit
                    #region
                    ArayTmp[0] = 1;
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xF016, bytSubID, bytDevID, false, true, true, false) == true)
                    {
                        for (int intI = 0; intI < ChannelCount; intI++)
                        {
                            ChnList[intI].MaxValue = CsConst.myRevBuf[26 + intI];
                        }
                        CsConst.myRevBuf = new byte[1200];
                        HDLUDP.TimeBetwnNext(20);
                    }
                    else return;
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(5, null);
                    #endregion

                    // read High Level
                    #region
                    ArayTmp = null;
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xF020, bytSubID, bytDevID, false, true, true, false) == true)
                    {
                        for (int intI = 0; intI < ChannelCount; intI++)
                        {
                            ChnList[intI].MaxLevel = CsConst.myRevBuf[25 + intI];
                        }
                        CsConst.myRevBuf = new byte[1200];
                        HDLUDP.TimeBetwnNext(10);
                    }
                    else return;
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(6, null);
                    #endregion

                    // read on delay
                    #region
                    ArayTmp = new byte[0];
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xF04D, bytSubID, bytDevID, false, true, true, false) == true)
                    {
                        for (int intI = 0; intI < ChannelCount; intI++)
                        {
                            ChnList[intI].PowerOnDelay = CsConst.myRevBuf[25 + intI];
                        }

                        HDLUDP.TimeBetwnNext(20);
                    }
                    else return;
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(7, null);
                    #endregion

                    // read protoct delay
                    #region
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xF03F, bytSubID, bytDevID, false, true, true, false) == true)
                    {
                        for (int intI = 0; intI < ChannelCount; intI++)
                        {
                            ChnList[intI].ProtectDealy = CsConst.myRevBuf[25 + intI];
                        }

                        HDLUDP.TimeBetwnNext(20);
                    }
                    else return;
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(8, null);
                    #endregion

                    #endregion
                    #endregion
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(10, null);
                    MyRead2UpFlags[2] = true;
                }
            }
            if (intActivePage == 0 || intActivePage == 1)
            {
                #region
                MSKeys = new List<MS04Key>();
                for (byte bytI = 0; bytI < wdMaxValue; bytI++)
                {
                    MS04Key oTmp = new MS04Key();
                    oTmp.ID = Convert.ToByte(bytI + 1);
                    MSKeys.Add(oTmp);
                }
                // read keys enable or not  and repeat flag
                if (intDeviceType != 116 && intDeviceType != 363)
                {
                    //读取按键使能
                    #region
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x0128, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(intDeviceType)) == true)
                    {
                        for (byte bytI = 0; bytI < wdMaxValue; bytI++)
                        {
                            if (CsConst.myRevBuf != null)
                            {
                                MSKeys[bytI].bytEnable = CsConst.myRevBuf[25 + bytI];
                            }
                        }
                        CsConst.myRevBuf = new byte[1200];
                        HDLUDP.TimeBetwnNext(1);
                        if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(11, null);
                    }
                    else return;
                    #endregion

                    //读取面板锁键使能
                    #region
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x0158, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(intDeviceType)) == true)
                    {
                        for (byte bytI = 0; bytI < wdMaxValue; bytI++)
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
                }

                // 读取模式
                #region
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xD205, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(intDeviceType)) == true)
                {
                    for (byte bytI = 0; bytI < wdMaxValue; bytI++)
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
                if (MS04DeviceTypeList.MS04GetTameredDeviceTypeList.Contains(intDeviceType))
                {
                    Byte[] ArayDimDirection = ReadButtonDimDirectionFrmDeviceToBuf(bytSubID,bytDevID);
                    for (byte bytI = 0; bytI < wdMaxValue; bytI++)
                    {
                        if (CsConst.myRevBuf != null)
                        {
                            MSKeys[bytI].IsDimmer = ArayDimDirection[bytI];
                        }
                    }
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(14, null);
                }
                #endregion

                // read dimmer or not 
                #region
                if (intDeviceType != 116 && intDeviceType != 363)
                {
                    if (intDeviceType == 119 || intDeviceType == 137 || intDeviceType == 135 || intDeviceType == 121)
                    {
                        Byte[] AraySaveDim = ReadButtonDimFlagFrmDeviceToBuf(bytSubID, bytDevID);
                        for (Byte bytI=0; bytI< MSKeys.Count;bytI++) // in AraySaveDim)
                        {
                            MSKeys[bytI].SaveDimmer = AraySaveDim[bytI];
                        }
                    }
                }
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(15, null);
                #endregion

                // read low Limit 
                #region
                if (intDeviceType != 116 && intDeviceType != 363)
                {
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xE0E0, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(intDeviceType)) == true)
                    {
                        dimmerLow = CsConst.myRevBuf[26];
                        CsConst.myRevBuf = new byte[1200];
                        HDLUDP.TimeBetwnNext(1);
                    }
                    else return;
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(16, null);
                }
                #endregion

                
                #endregion
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(20, null);


                //read all keys setup : remark delay targets
                #region
                for (byte bytI = 0; bytI < wdMaxValue; bytI++)
                {
                    #region
                    ArayTmp = new byte[2];
                    ArayTmp[0] = bytI;
                    ArayTmp[1] = 0;

                    // read remark  不管什么类型读取备注 读取公共的命令  即 ON 的 命令 目标
                    #region
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xD210, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(intDeviceType)) == true)
                    {
                        byte[] arayRemark = new byte[20];
                        HDLSysPF.CopyRemarkBufferFrmMyRevBuffer(CsConst.myRevBuf, arayRemark, 28);
                        MSKeys[bytI].Remark = HDLPF.Byte2String(arayRemark);
                        CsConst.myRevBuf = new byte[1200];
                        HDLUDP.TimeBetwnNext(10);
                    }
                    else return;
                    #endregion

                    //读取目标
                    #region
                    if (CsConst.isRestore)
                    {
                        if (MS04DeviceTypeList.MS04DryContactWithE000PublicCMD.Contains(intDeviceType))//无线新命令
                        {
                            MSKeys[bytI].ReadButtonRemarkAndCMDFromDevice(bytSubID, bytDevID, intDeviceType, 0, 1, false,0,0);
                        }
                        else if (MS04DeviceTypeList.MS04IOModuleDeviceTypeList.Contains(intDeviceType))  //六路输入输出模块
                        {
                            ReadIOModuleCommandsGroupFrmDeviceToBuf(bytSubID, bytDevID, intDeviceType, bytI, 1,0,0);
                        }
                        else
                        {
                            ReadDryCommandsGroupFrmDeviceToBuf(bytSubID, bytDevID, intDeviceType, bytI, 1,0,0);
                        }
                    }
                    #endregion

                    MSKeys[bytI].ONOFFDelay = new int[2];
                    //机械开关再读取
                    #region
                    if (MSKeys[bytI].Mode == 0)
                    {
                        //读取机械延时
                        #region
                        if (intDeviceType == 116)
                        {
                            ArayTmp = new byte[1];
                            ArayTmp[0] = bytI;
                            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xD218, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(intDeviceType)) == true)
                            {
                                MSKeys[bytI].ONOFFDelay[1] = CsConst.myRevBuf[27] * 256 + CsConst.myRevBuf[28];
                                CsConst.myRevBuf = new byte[1200];
                                HDLUDP.TimeBetwnNext(10);
                            }
                            else return;
                        }
                        else
                        {
                            ArayTmp = new byte[2];
                            ArayTmp[0] = bytI;
                            ArayTmp[1] = 1;
                            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xD218, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(intDeviceType)) == true)
                            {
                                MSKeys[bytI].ONOFFDelay[0] = CsConst.myRevBuf[28] * 256 + CsConst.myRevBuf[29];
                                CsConst.myRevBuf = new byte[1200];
                                HDLUDP.TimeBetwnNext(10);
                            }
                            else return;

                            // 新增一个keystate 用于存储OFF的信息
                            ArayTmp[1] = 0;
                            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xD218, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(intDeviceType)) == true)
                            {
                                MSKeys[bytI].ONOFFDelay[1] = CsConst.myRevBuf[28] * 256 + CsConst.myRevBuf[29];
                                CsConst.myRevBuf = new byte[1200];
                                HDLUDP.TimeBetwnNext(10);
                            }
                            else return;
                        }
                        #endregion


                        //读取目标
                        #region
                        if (CsConst.isRestore)
                        {
                            if (MS04DeviceTypeList.MS04DryContactWithE000PublicCMD.Contains(intDeviceType))//无线新命令
                            {
                                MSKeys[bytI].ReadButtonRemarkAndCMDFromDevice(bytSubID, bytDevID, intDeviceType, 0, 1, true,0,0);
                            }
                            else if (MS04DeviceTypeList.MS04IOModuleDeviceTypeList.Contains(intDeviceType))  //六路输入输出模块
                            {
                                ReadIOModuleCommandsGroupFrmDeviceToBuf(bytSubID, bytDevID, intDeviceType, bytI, 0,0,0);
                            }
                            else
                            {
                                ReadDryCommandsGroupFrmDeviceToBuf(bytSubID, bytDevID, intDeviceType, bytI, 0,0,0);
                            }
                        }
                        #endregion
                    }
                    #endregion
                    #endregion
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(20 + bytI * 10 / wdMaxValue, null);
                }
                #endregion
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(30, null);
                MyRead2UpFlags[0] = true;
            }

            if (intActivePage == 0 || intActivePage == 2)
            {
                //安防设置
                #region
                if (!(MS04DeviceTypeList.MS04G1WithoutSecurityDeviceTypeList.Contains(intDeviceType)))
                {
                    myKeySeu = new List<UVCMD.SecurityInfo>();
                    ArayTmp = new byte[1];
                    for (byte intI = 1; intI <= wdMaxValue; intI++)
                    {
                        ArayTmp = new byte[2];
                        ArayTmp[0] =Convert.ToByte(intI + wdMaxValue - 1);
                        //读取备注
                        UVCMD.SecurityInfo oTmp = new UVCMD.SecurityInfo();
                        #region
                        ArayTmp[1] = 0;

                        // read remark  不管什么类型读取备注 读取公共的命令  即 OFF 的 命令 目标
                        #region
                        if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xD210, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(intDeviceType)) == true)
                        {
                            byte[] arayRemark = new byte[20];
                            HDLSysPF.CopyRemarkBufferFrmMyRevBuffer(CsConst.myRevBuf, arayRemark, 28);
                            oTmp.strRemark = HDLPF.Byte2String(arayRemark);
                            CsConst.myRevBuf = new byte[1200];
                            HDLUDP.TimeBetwnNext(10);
                        }
                        else return;
                        #endregion

                        #endregion
                        //读取设置
                        ArayTmp[0] = intI;
                        if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x15DA, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(intDeviceType)) == true)
                        {
                            oTmp.bytSeuID = intI;
                            oTmp.bytTerms = CsConst.myRevBuf[26];
                            oTmp.bytSubID = CsConst.myRevBuf[27];
                            oTmp.bytDevID = CsConst.myRevBuf[28];
                            oTmp.bytArea = CsConst.myRevBuf[29];
                            CsConst.myRevBuf = new byte[1200];
                            HDLUDP.TimeBetwnNext(1);
                        }
                        else return;
                        myKeySeu.Add(oTmp);
                        if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(20 + intI * 10 / wdMaxValue, null);
                    }
                }
                #endregion
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(40, null);
                MyRead2UpFlags[1] = true;
            }

            if (intActivePage == 0 || intActivePage == 4)
            {
                if (MS04DeviceTypeList.MS04HotelMixModule.Contains(intDeviceType))
                {
                    //其他设置
                    #region
                    //读取电压
                    #region
                    ArayTmp = new byte[0];
                    ArayTmp = null;
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1802, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(intDeviceType)) == true)
                    {
                        bytVotoge = CsConst.myRevBuf[38];
                        CsConst.myRevBuf = new byte[1200];
                    }
                    else return;
                    #endregion
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(41, null);
                    //读取窗帘
                    #region
                    arayCurtain = new byte[6];
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1F18, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(intDeviceType)) == true)
                    {
                        Array.Copy(CsConst.myRevBuf, 25, arayCurtain, 0, 6);
                        CsConst.myRevBuf = new byte[1200];
                    }
                    else return;

                    ArayTmp = new byte[1];
                    for (Byte i = 1; i <= 2; i++)
                    {
                        ArayTmp[0] = i;
                        if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xE800, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(intDeviceType)) == true)
                        {
                            Array.Copy(CsConst.myRevBuf, 26, arayCurtain, 1 + (i - 1) * 3, 2);
                            CsConst.myRevBuf = new byte[1200];
                        }
                        else return;
                    }
                    #endregion
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(42, null);
                    //读取门铃
                    #region
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1F22, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(intDeviceType)) == true)
                    {
                        bytDoorBellRunTime = CsConst.myRevBuf[25];
                        CsConst.myRevBuf = new byte[1200];
                    }
                    else return;
                    #endregion
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(43, null);
                    //读取LED
                    #region
                    ArayTmp = new byte[2];
                    for (byte i = 1; i <= 3; i++)
                    {
                        ArayTmp[0] = i;
                        ArayTmp[1] = 0;
                        if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x2018, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(intDeviceType)) == true)
                        {
                            Array.Copy(CsConst.myRevBuf, 28, arayUVStaus, (i - 1) * 12, 6);
                            CsConst.myRevBuf = new byte[1200];
                        }
                        else return;

                        ArayTmp[1] = 1;
                        if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x2018, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(intDeviceType)) == true)
                        {
                            Array.Copy(CsConst.myRevBuf, 28, arayUVStaus, (i - 1) * 12 + 6, 6);
                            CsConst.myRevBuf = new byte[1200];
                        }
                        else return;
                        if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(20 + i, null);
                    }
                    #endregion
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(44, null);
                    #endregion
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(50, null);
                }
                MyRead2UpFlags[3] = true;
            }
            if (intActivePage == 0 || intActivePage == 5)
            {
                if (MS04DeviceTypeList.MS04IOModuleDeviceTypeList.Contains(intDeviceType))
                {
                    #region
                    myRange = new List<InputRange>();
                    //读取输入范围
                    #region
                    ArayTmp = new byte[1];
                    for (byte i = 1; i <= 6; i++)
                    {
                        #region
                        // 读取范围
                        ArayTmp[0] = i;
                        if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x3513, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(intDeviceType)) == true)
                        {
                            InputRange temp = new InputRange();
                            temp.arayPoint = new int[0];
                            temp.ID = i;
                            temp.InputType = CsConst.myRevBuf[26];
                            temp.Connection = CsConst.myRevBuf[27];
                            temp.AnalogType = CsConst.myRevBuf[28];
                            temp.Common = CsConst.myRevBuf[29];
                            temp.Broadcat = CsConst.myRevBuf[30];
                            temp.Range1 = CsConst.myRevBuf[31] + CsConst.myRevBuf[32] * 256;
                            temp.Range2 = CsConst.myRevBuf[33] + CsConst.myRevBuf[34] * 256;
                            if (temp.InputType == 2 || temp.InputType == 4)  //类比电流 类比电压
                            {
                                temp.Min = CsConst.myRevBuf[35] + CsConst.myRevBuf[36] * 256;
                                temp.Max = CsConst.myRevBuf[37] + CsConst.myRevBuf[38] * 256;
                                temp.Count = CsConst.myRevBuf[39] + CsConst.myRevBuf[40] * 256;
                                temp.arayPoint = new int[4096];
                            }
                            else
                            {
                                temp.Max = -1;
                                temp.Min = -1;
                            }

                            myRange.Add(temp);
                            HDLUDP.TimeBetwnNext(1);

                            //依次读取点设置  范围   "min  max"  "point count" 包号用0开始
                            #region
                            if (temp.Max != -1 && temp.Min != -1)
                            {
                                int iPointSum = temp.Count;  // 点总数
                                int iReadTimes = 0;

                                if (iPointSum <= 32) iReadTimes = 1;
                                else if (iPointSum % 32 == 0) iReadTimes = iPointSum / 32;
                                else iReadTimes = iPointSum / 32 + 1;

                                for (int j = 0; j < iReadTimes; j++)
                                {
                                    ArayTmp = new byte[2] { temp.ID, (Byte)j };
                                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x351B, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(intDeviceType)) == true)
                                    {
                                        for (int p = 0; p < 25; p++)
                                        {
                                            temp.arayPoint[j * 25 + p] = CsConst.myRevBuf[27 + 2 * p] + CsConst.myRevBuf[27 + 2 * p + 1] * 256;
                                        }

                                        HDLUDP.TimeBetwnNext(1);
                                    }
                                    else return;
                                }
                            }
                            #endregion
                            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(50 + i, null);
                        #endregion
                        }
                        //读取常量是否保存
                        #region
                        if (myRange[i - 1].InputType != 0) // 不是干节点类比，有广播使能位选择
                        {
                            Byte[] arayReadBroadcastFlag = new Byte[2];
                            switch (myRange[i - 1].AnalogType)
                            {
                                case 0: arayReadBroadcastFlag = new Byte[] { 1, (Byte)(i) }; break;
                                case 1: arayReadBroadcastFlag = new Byte[] { 0, (Byte)(i) }; break;
                                case 2: arayReadBroadcastFlag = new Byte[] { 5, (Byte)(i) }; break;
                                case 3: arayReadBroadcastFlag = new Byte[] { 4, (Byte)(i) }; break;
                            }
                            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xE444, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(intDeviceType)) == true)
                            {
                                myRange[i - 1].Broadcat = CsConst.myRevBuf[27];
                                HDLUDP.TimeBetwnNext(1);
                            }
                        }
                        #endregion
                    }
                    #endregion
                    #endregion
                }
                MyRead2UpFlags[4] = true;
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(60, null);
            }
             if (intActivePage == 0 || intActivePage == 6)
            {
                if (MS04DeviceTypeList.MS04IOModuleDeviceTypeList.Contains(intDeviceType))
                {
                    //读取输出类型
                    #region
                    if (intDeviceType == 363)
                    {
                        ArayTmp = new byte[1];
                        arayOutput = new byte[2];
                        for (byte i = 0; i <= 1; i++)
                        {
                            ArayTmp[0] = Convert.ToByte(i + 7);
                            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x3517, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(intDeviceType)) == true)
                            {
                                arayOutput[i] = CsConst.myRevBuf[26];

                                HDLUDP.TimeBetwnNext(1);
                            }
                            else return;
                            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(60 + i, null);
                        }
                        ArayTmp = new byte[0];
                        if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x3520, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(intDeviceType)) == true)
                        {
                            bytChannelStatus = CsConst.myRevBuf[25];

                            HDLUDP.TimeBetwnNext(1);
                            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(62, null);
                        }
                        else return;

                    }
                    #endregion
                }
                MyRead2UpFlags[5] = true;
            }
           // if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(100, null);
            blnIsSuccess = true;
            return;
        }

        public void ReadIOModuleCommandsGroupFrmDeviceToBuf(Byte bytSubID, Byte bytDevID, int DeviceType, Byte DryId, Byte DryOnOrOffStatus, Byte bStartCmd, Byte bToCmd)
        {
            try
            {
                #region
                byte bytTotalCMD = 20;
                switch (MSKeys[DryId].Mode)
                {
                    case 0:
                    case 4:
                    case 5:
                    case 6: bytTotalCMD = 20; break;  // 99 个目标
                    default: bytTotalCMD = 1; break;
                }

                if (bStartCmd == 0 && bToCmd == 0)
                {
                    bStartCmd = 1; bToCmd = bytTotalCMD;
                }

                Byte[]  ArayTmp = new byte[3];
                ArayTmp[0] = Convert.ToByte(DryId + 1);
                ArayTmp[1] = DryOnOrOffStatus;
                if (DryOnOrOffStatus == 0)
                    MSKeys[DryId].KeyOffTargets = new List<UVCMD.ControlTargets>();
                else
                    MSKeys[DryId].KeyTargets = new List<UVCMD.ControlTargets>();
                for (byte bytJ = 0; bytJ < bytTotalCMD; bytJ++)
                {
                    if (bytJ + 1 >= bStartCmd && bytJ + 1 <= bToCmd)
                    {
                        ArayTmp[2] = bytJ;
                        if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1CEE, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                        {
                            UVCMD.ControlTargets oCMD = new UVCMD.ControlTargets();
                            oCMD.ID = Convert.ToByte(CsConst.myRevBuf[27] + 1);
                            oCMD.Type = CsConst.myRevBuf[28];  //转换为正确的类型
                            oCMD.SubnetID = CsConst.myRevBuf[29];
                            oCMD.DeviceID = CsConst.myRevBuf[30];
                            oCMD.Param1 = CsConst.myRevBuf[31];
                            oCMD.Param2 = CsConst.myRevBuf[32];
                            oCMD.Param3 = CsConst.myRevBuf[33];
                            oCMD.Param4 = CsConst.myRevBuf[34];
                            CsConst.myRevBuf = new byte[1200];
                            MSKeys[DryId].KeyTargets.Add(oCMD);
                        }
                        else return;
                    }
                }
                #endregion
            }
            catch
            {}
        }

        public void ModifyIOModuleCommandsGroupFrmDeviceToBuf(Byte bytSubID, Byte bytDevID, int DeviceType, Byte DryId, Byte DryOnOrOffStatus)
        {
            List<UVCMD.ControlTargets> tmpCommandsGroup = new List<UVCMD.ControlTargets>();
            try
            {
                if (DryOnOrOffStatus == 1)
                {
                    tmpCommandsGroup = MSKeys[DryId].KeyTargets;
                }
                else
                {
                    tmpCommandsGroup = MSKeys[DryId].KeyOffTargets;
                }
                #region
                if (tmpCommandsGroup != null)
                {
                    foreach (UVCMD.ControlTargets TmpCmd in tmpCommandsGroup)
                    {
                        byte[] arayCMD = new byte[10];
                        arayCMD[0] = Convert.ToByte(MSKeys[DryId].ID);
                        arayCMD[1] = DryOnOrOffStatus;
                        arayCMD[2] = Convert.ToByte(TmpCmd.ID - 1);
                        arayCMD[3] = TmpCmd.Type;
                        arayCMD[4] = TmpCmd.SubnetID;
                        arayCMD[5] = TmpCmd.DeviceID;
                        arayCMD[6] = TmpCmd.Param1;
                        arayCMD[7] = TmpCmd.Param2;
                        arayCMD[8] = TmpCmd.Param3;   // save targets
                        arayCMD[9] = TmpCmd.Param4;
                        if (CsConst.mySends.AddBufToSndList(arayCMD, 0x1CEC, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                        {
                            CsConst.myRevBuf = new byte[1200];
                            HDLUDP.TimeBetwnNext(20);
                        }
                        else return;
                    }
                }
                #endregion
            }
            catch
            { }
        }

        public void ReadDryCommandsGroupFrmDeviceToBuf(Byte bytSubID, Byte bytDevID, int DeviceType, Byte DryId, Byte DryOnOrOffStatus, Byte bStartCmd, Byte bToCmd)
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
                    case 6:
                    case 9: bytTotalCMD = 99; break;  // 99 个目标
                    default: bytTotalCMD = 1; break;
                }

                if (bStartCmd == 0 && bToCmd == 0)
                {
                    bStartCmd = 1; bToCmd = bytTotalCMD;
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
                    if (bytJ + 1 >= bStartCmd && bytJ + 1 <= bToCmd)
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
                }
                #endregion
            }
            catch
            { }
        }

        public void ModifyDryCommandsGroupFrmDeviceToBuf(Byte bytSubID, Byte bytDevID, int DeviceType, Byte DryId, Byte DryOffOrOnStatus)
        {
            List<UVCMD.ControlTargets> tmpCommandsGroup = new List<UVCMD.ControlTargets>();
            try
            {
                if (DryOffOrOnStatus == 1)
                {
                   tmpCommandsGroup = MSKeys[DryId].KeyTargets;
                }
                else
                {
                   tmpCommandsGroup = MSKeys[DryId].KeyOffTargets;
                }

                if (tmpCommandsGroup != null)
                {
                    foreach (UVCMD.ControlTargets TmpCmd in tmpCommandsGroup)
                    {
                        byte[] arayCMD = new byte[10];
                        arayCMD[0] = Convert.ToByte(DryId);
                        arayCMD[1] = DryOffOrOnStatus;
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


        public Boolean ReadButtonRemarkAndCMDFromDevice(Byte SubNetID, Byte DeviceID, int DeviceType, Byte DryId, int iDryOnOrOffStatus, Byte bStartCmd, Byte bToCmd)
        {
            try
            {
                    //读取目标
                    byte bytTotalCMD = 0;
                    switch (MSKeys[DryId].Mode)
                    {
                        case 0:
                        case 4:
                        case 5:
                        case 6:
                        case 9: bytTotalCMD = 99; break;  // 99 个目标
                        default: bytTotalCMD = 1; break;
                    }

                    if (bStartCmd == 0 && bToCmd == 0)
                    {
                        bStartCmd = 1; bToCmd = bytTotalCMD;
                    }

                    if (bStartCmd == 0 && bToCmd == 0)
                    {
                        bStartCmd = 1; bToCmd = bytTotalCMD;
                    }
                    String strEnable = "";

                    Byte bStartCmdId = 0;
                    if (iDryOnOrOffStatus == 0) // 表示关的目标
                    {
                        if (MS04DeviceTypeList.MS04HotelMixModule.Contains(DeviceType))  //空间限制
                        {
                            bStartCmdId = 50;
                        }
                        else
                        {
                            bStartCmdId = 100;
                        }
                    }
                    Byte[] ArayTmp = new Byte[1];

                    //读取目标有效与否
                    #region
                    //if (blnIsSupportE474 == true)
                    //{
                    //    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xE474, SubNetID, DeviceID, false, false, true, false) == true)
                    //    {
                    //        int count1 = CsConst.myRevBuf[26];
                    //        int count2 = count1 / 8;
                    //        if (count1 % 8 != 0) count2 = count2 + 1;
                    //        byte[] arayEnable = new byte[count2];
                    //        Array.Copy(CsConst.myRevBuf, 27, arayEnable, 0, count2);

                    //        for (int i = 0; i < count2; i++)
                    //        {
                    //            string strTmp = GlobalClass.AddLeftZero(Convert.ToString(arayEnable[i], 2), 8);
                    //            for (int j = 7; j >= 0; j--)
                    //            {
                    //                string str = strTmp.Substring(j, 1);
                    //                strEnable = strEnable + str;
                    //            }
                    //        }
                    //        CsConst.myRevBuf = new byte[1200];
                    //    }
                    //}
                    #endregion

                    List<UVCMD.ControlTargets> arrCurrentReadingList = new List<UVCMD.ControlTargets>();
                    
                    for (byte byt = 1; byt <= bytTotalCMD; byt++)
                    {
                        if (byt >= bStartCmd && byt <= bToCmd)
                        {
                            #region
                            ArayTmp = new Byte[]{(Byte)(DryId + 1),(Byte)(byt + bStartCmdId)};

                            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xE000, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                            {
                                UVCMD.ControlTargets oCMD = new UVCMD.ControlTargets();
                                oCMD.ID = byt;
                                oCMD.Type = CsConst.myRevBuf[27];  //转换为正确的类型
                                oCMD.SubnetID = CsConst.myRevBuf[28];
                                oCMD.DeviceID = CsConst.myRevBuf[29];
                                oCMD.Param1 = CsConst.myRevBuf[30];
                                oCMD.Param2 = CsConst.myRevBuf[31];
                                oCMD.Param3 = CsConst.myRevBuf[32];
                                oCMD.Param4 = CsConst.myRevBuf[33];
                                arrCurrentReadingList.Add(oCMD);
                                CsConst.myRevBuf = new byte[1200];
                            }
                            else return false;
                            #endregion
                        }
                    }
                    if (iDryOnOrOffStatus == 1) // 干节点 开 统一处理
                    {
                        MSKeys[DryId].KeyTargets = arrCurrentReadingList;
                    }
                    else
                    {
                        MSKeys[DryId].KeyOffTargets = arrCurrentReadingList;
                    }
                }
            catch
            {
                return false;
            }
            return true;
        }

        public void UploadButtonRemarkAndCMDToDevice(Byte SubNetID, Byte DeviceID, int DeviceType,  Byte DryId,  int iDryOnOrOffStatus)
        {
            try
            {
                List<UVCMD.ControlTargets> arrCurrentReadingList = new List<UVCMD.ControlTargets>();
                Byte bStartCmdId = 0;
                if (iDryOnOrOffStatus == 1) // 干节点 开 统一处理
                {
                    arrCurrentReadingList = MSKeys[DryId].KeyTargets;
                }
                else
                {
                    if (MS04DeviceTypeList.MS04HotelMixModule.Contains(DeviceType)) // 酒店混合模块空间限制，只有1-48 ， 51-98
                    {
                        bStartCmdId = 50;
                    }
                    else
                    {
                        bStartCmdId = 100;
                    }
                    arrCurrentReadingList = MSKeys[DryId].KeyOffTargets;
                }

                if (arrCurrentReadingList != null)
                {
                    foreach (UVCMD.ControlTargets TmpCmd in arrCurrentReadingList)
                    {
                        if (TmpCmd.Type != 0 && TmpCmd.Type != 255)
                        {
                            byte[] arayCMD = new byte[9];
                            arayCMD[0] = (Byte)(DryId + 1);
                            arayCMD[1] = byte.Parse((TmpCmd.ID + bStartCmdId).ToString());
                            arayCMD[2] = TmpCmd.Type;
                            arayCMD[3] = TmpCmd.SubnetID;
                            arayCMD[4] = TmpCmd.DeviceID;
                            arayCMD[5] = TmpCmd.Param1;
                            arayCMD[6] = TmpCmd.Param2;
                            arayCMD[7] = TmpCmd.Param3;   // save targets
                            arayCMD[8] = TmpCmd.Param4;
                            
                            if (CsConst.mySends.AddBufToSndList(arayCMD, 0xE002, SubNetID, DeviceID, false, true, true, true) == false) return;
                            HDLUDP.TimeBetwnNext(arayCMD.Length);
                            CsConst.myRevBuf = new byte[1200];
                        }
                    }
                }
            }
            catch{}
        }
    }

}
