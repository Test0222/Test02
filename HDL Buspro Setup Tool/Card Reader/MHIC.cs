using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;

namespace HDL_Buspro_Setup_Tool
{
    [Serializable]
    public class MHIC : HdlDeviceBackupAndRestore
    {
        public int DIndex;  ////设备唯一编号
        public string DeviceName;
        public bool[] MyRead2UpFlags = new bool[3];

        public byte Backlight; ////背光灯
        public byte Ledlight;  ////状态灯
        public byte[] arayButtonColor;//颜色
        public byte[] arayButtonBalance;//白平衡
        public byte[] arayButtonSensitiVity;//灵敏度
        public byte[] arayHotel;//酒店信息

        internal List<Key> myKeySetting;

        [Serializable]
        internal class Key : HDLButton
        {
            public int Delay;
        }

        //<summary>
        //读取默认设置
        //</summary>
        public void ReadDefaultInfo()
        {
            myKeySetting = new List<Key>();
        }

        //<summary>
        //读取数据信息
        //</summary>
        public void ReadDataFrmDBTobuf(int DIndex)
        {
            try
            {
                #region
                string strsql = string.Format("select * from dbBasicInfo where DIndex={0}", DIndex);
                OleDbDataReader dr = DataModule.SearchAResultSQLDB(strsql, CsConst.mstrCurPath);
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
                strsql = string.Format("select * from dbClassInfomation where DIndex={0} and ClassID=0 order by SenNum", DIndex);
                dr = DataModule.SearchAResultSQLDB(strsql, CsConst.mstrCurPath);
                if (dr != null)
                {
                    while (dr.Read())
                    {
                        string str = dr.GetValue(5).ToString();
                        Backlight = Convert.ToByte(str.Split('-')[0].ToString());
                        Ledlight = Convert.ToByte(str.Split('-')[1].ToString());
                        arayButtonColor = (byte[])dr[6];
                        arayButtonBalance = (byte[])dr[7];
                        arayButtonSensitiVity = (byte[])dr[8];
                        arayHotel = (byte[])dr[9];
                    }
                    dr.Close();
                }
                #endregion

                #region
                strsql = string.Format("select * from dbClassInfomation where DIndex={0} and ClassID={1} order by SenNum", DIndex, 1);
                dr = DataModule.SearchAResultSQLDB(strsql, CsConst.mstrCurPath);
                myKeySetting = new List<Key>();
                if (dr != null)
                {
                    while (dr.Read())
                    {
                        Key temp = new Key();
                        string str = dr.GetValue(5).ToString();
                        temp.ID = Convert.ToByte(str.Split('-')[0].ToString());
                        temp.Mode = Convert.ToByte(str.Split('-')[1].ToString());
                        temp.Delay = Convert.ToByte(str.Split('-')[2].ToString());
                        temp.Remark = dr.GetValue(4).ToString();
                        str = string.Format("select * from dbKeyTargets where DIndex ={0} and KeyIndex = {1} and Ms04State = {2}", DIndex, temp.ID, 0);
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
                        myKeySetting.Add(temp);
                    }
                    dr.Close();
                }
                #endregion
            }
            catch
            {
            }
        }

        //<summary>
        //保存数据
        //</summary>
        public void SaveDataToDB()
        {
            try
            {
                string strsql = string.Format("delete * from dbClassInfomation where DIndex={0}", DIndex);
                DataModule.ExecuteSQLDatabase(strsql);

                strsql = string.Format("delete * from dbKeyTargets where DIndex={0}", DIndex);
                DataModule.ExecuteSQLDatabase(strsql);

                #region
                string strRemarkBasic = "";
                string strParamBasic = Backlight.ToString() + "-" + Ledlight.ToString();

                if (arayButtonColor == null) arayButtonColor = new byte[0];
                if (arayButtonBalance == null) arayButtonBalance = new byte[0];
                if (arayButtonSensitiVity == null) arayButtonSensitiVity = new byte[0];
                if (arayHotel == null) arayHotel = new byte[13];
                strsql = @"Insert into dbClassInfomation(DIndex,ClassID,ObjectID,SenNum,Remark,strParam1,byteAry1,byteAry2,byteAry3,byteAry4)"
                  + " values(@DIndex,@ClassID,@ObjectID,@SenNum,@Remark,@strParam1,@byteAry1,@byteAry2,@byteAry3,@byteAry4)";
                //创建一个OleDbConnection对象
                OleDbConnection connBasic;
                connBasic = new OleDbConnection(DataModule.ConString + CsConst.mstrDefaultPath);
                //OleDbConnection conn = new OleDbConnection(DataModule.ConString + CsConst.mstrCurPath);
                connBasic.Open();
                OleDbCommand cmdBaic = new OleDbCommand(strsql, connBasic);
                ((OleDbParameter)cmdBaic.Parameters.Add("@DIndex", OleDbType.Integer)).Value = DIndex;
                ((OleDbParameter)cmdBaic.Parameters.Add("@ClassID", OleDbType.Integer)).Value = 0;
                ((OleDbParameter)cmdBaic.Parameters.Add("@ObjectID", OleDbType.Integer)).Value = 0;
                ((OleDbParameter)cmdBaic.Parameters.Add("@SenNum", OleDbType.Integer)).Value = 0;
                ((OleDbParameter)cmdBaic.Parameters.Add("@Remark", OleDbType.VarChar)).Value = strRemarkBasic;
                ((OleDbParameter)cmdBaic.Parameters.Add("@strParam1", OleDbType.VarChar)).Value = strParamBasic;
                ((OleDbParameter)cmdBaic.Parameters.Add("@byteAry1", OleDbType.Binary)).Value = arayButtonColor;
                ((OleDbParameter)cmdBaic.Parameters.Add("@byteAry2", OleDbType.Binary)).Value = arayButtonBalance;
                ((OleDbParameter)cmdBaic.Parameters.Add("@byteAry3", OleDbType.Binary)).Value = arayButtonSensitiVity;
                ((OleDbParameter)cmdBaic.Parameters.Add("@byteAry4", OleDbType.Binary)).Value = arayHotel;
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

                #region
                if (myKeySetting != null)
                {
                    for (int i = 0; i < myKeySetting.Count; i++)
                    {
                        string strRemark = myKeySetting[i].Remark;
                        string strParam = myKeySetting[i].ID.ToString() + "-" + myKeySetting[i].Mode.ToString() + "-" +
                                          myKeySetting[i].Delay.ToString();
                        strsql = string.Format("Insert into dbClassInfomation(DIndex,ClassID,ObjectID,SenNum,Remark,strParam1) values ({0},{1},{2},{3},'{4}','{5}')",
                                    DIndex, 1, 0, i, strRemark, strParam);
                        DataModule.ExecuteSQLDatabase(strsql);
                        if (myKeySetting[i].KeyTargets != null)
                        {
                            for (int intK = 0; intK < myKeySetting[i].KeyTargets.Count; intK++)
                            {
                                UVCMD.ControlTargets TmpCmds = myKeySetting[i].KeyTargets[intK];
                                ///// insert into all commands to database
                                strsql = string.Format("Insert into dbKeyTargets(DIndex,KeyIndex,objID,KeyFunType,SubNetID,DeviceID,FirstParameter,"
                                                + "SecondParameter,RunTimeMinute,RunTimeSecond,Ms04State) values ({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10})",
                                               DIndex, myKeySetting[i].ID, TmpCmds.ID, TmpCmds.Type, TmpCmds.SubnetID, TmpCmds.DeviceID, TmpCmds.Param1,
                                               TmpCmds.Param2, TmpCmds.Param3, TmpCmds.Param4, 0);
                                DataModule.ExecuteSQLDatabase(strsql);
                            }
                        }
                    }
                }
                #endregion
            }
            catch
            {
            }
        }


        public bool UploadInfosToDevice(string DevNam, int wdDeviceType, int intActivePage)
        {
            string strMainRemark = DevNam.Split('\\')[1].Trim();
            DevNam = DevNam.Split('\\')[0].Trim();
            byte bytSubID = byte.Parse(DevNam.Split('-')[0].ToString());
            byte bytDevID = byte.Parse(DevNam.Split('-')[1].ToString());
            int wdMaxValue = DeviceTypeList.GetMaxValueFromPublicModeGroup(wdDeviceType);
            byte[] ArayMain = new byte[20];
            byte[] arayTmpRemark = HDLUDP.StringToByte(strMainRemark);
            byte[] ArayTmp = null;
            if (HDLSysPF.ModifyDeviceMainRemark(bytSubID, bytDevID, strMainRemark, wdDeviceType) == false) return false;

            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(5, null);
            if (intActivePage == 0 || intActivePage == 1)
            {
                #region
                byte[] arayLED = new byte[11];
                arayLED[0] = Backlight;   // modify LED and backlight 
                arayLED[1] = Ledlight;
                if (CsConst.mySends.AddBufToSndList(arayLED, 0xE012, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType)) == true)
                {
                    
                    HDLUDP.TimeBetwnNext(20);
                }
                else return false;

                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(10, null);
                if (CsConst.mySends.AddBufToSndList(arayHotel, 0x2016, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType)) == true)
                {
                    
                    HDLUDP.TimeBetwnNext(20);
                }
                else return false;
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(15, null);
                #endregion
            }

            if (intActivePage == 0 || intActivePage == 2)
            {
                #region
                byte[] arayKeyMode = new byte[wdMaxValue];
                byte[] arayRemark = new byte[21];
                byte[] arayDely = new byte[wdMaxValue*2];
                foreach (Key TmpKey in myKeySetting)
                {
                    arayRemark = new byte[21];
                    TmpKey.UploadButtonRemarkAndCMDToDevice(bytSubID, bytDevID, wdDeviceType,-1, 255);
                    
                    arayDely[(TmpKey.ID - 1) * 2] = Convert.ToByte(TmpKey.Delay / 256);
                    arayDely[(TmpKey.ID - 1) * 2 + 1] = Convert.ToByte(TmpKey.Delay % 256);
                    arayKeyMode[TmpKey.ID - 1] = TmpKey.Mode;

                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(25 + TmpKey.ID, null);
                }
                if (CsConst.mySends.AddBufToSndList(arayKeyMode, 0xE00A, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType)) == true)
                {
                    
                    HDLUDP.TimeBetwnNext(20);
                }
                else return false;
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(40, null);
                if (wdDeviceType != 3058 && wdDeviceType != 3060)
                {
                    if (CsConst.mySends.AddBufToSndList(arayDely, 0x2026, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType)) == true)
                    {
                        
                        HDLUDP.TimeBetwnNext(20);
                    }
                    else return false;
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(45, null);
                }
                #endregion
            }

            if (intActivePage == 0 || intActivePage == 3)
            {
                #region
                if (wdDeviceType == 3076 || wdDeviceType == 3078 || wdDeviceType==3080)
                {
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(91);
                    int maxLength = wdMaxValue;
                    if (wdDeviceType == 3076 || wdDeviceType == 3078 || wdDeviceType == 3080) maxLength = 3;
                    for (int i = 1; i <= maxLength; i++)
                    {
                        ArayTmp = new byte[7];
                        ArayTmp[0] = Convert.ToByte(i);
                        Array.Copy(arayButtonColor, (i - 1) * 6, ArayTmp, 1, 6);
                        if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xE14E, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType)) == true)
                        {
                            
                            HDLUDP.TimeBetwnNext(20);
                        }
                        else return false;
                    }
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(92);


                    if (arayButtonBalance[0] >= 0)
                    {
                        ArayTmp = new byte[arayButtonBalance[0] * 3];
                        Array.Copy(arayButtonBalance, 1, ArayTmp, 0, arayButtonBalance.Length - 1);
                        if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x199A, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType)) == true)
                        {
                            
                            HDLUDP.TimeBetwnNext(20);
                        }
                        else return false;
                    }


                    ArayTmp = new byte[maxLength];
                    Array.Copy(arayButtonSensitiVity, 0, ArayTmp, 0, maxLength);
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xE152, bytSubID, bytDevID, false, false, false, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType)) == true)
                    {
                        
                        HDLUDP.TimeBetwnNext(20);
                    }
                    else return false;

                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(93);
                }
                #endregion
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(100, null);
            return true;
        }

        public bool DownLoadInfoFrmDevice(string DevNam, int wdDeviceType, int intActivePage)
        {
            string strMainRemark = DevNam.Split('\\')[1].Trim();
            DevNam = DevNam.Split('\\')[0].Trim();
            byte bytSubID = byte.Parse(DevNam.Split('-')[0].ToString());
            byte bytDevID = byte.Parse(DevNam.Split('-')[1].ToString());
            byte[] ArayTmp = null;
            int wdMaxValue = DeviceTypeList.GetMaxValueFromPublicModeGroup(wdDeviceType);

            DeviceName = bytSubID.ToString() + "-" + bytDevID.ToString() + "\\" + HDLSysPF.ReadDeviceMainRemark(bytSubID, bytDevID);
            
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(5, null);
            if (intActivePage == 0 || intActivePage == 1)
            {
                #region
                arayHotel = new byte[13];
                ArayTmp = new byte[0];
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xE010, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType)) == true)
                {
                    Backlight = CsConst.myRevBuf[25];
                    Ledlight = CsConst.myRevBuf[26];
                    
                    HDLUDP.TimeBetwnNext(1);
                }
                else return false;
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(10, null);
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x2014, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType)) == true)
                {
                    Array.Copy(CsConst.myRevBuf, 25, arayHotel, 0, 13);
                    
                    HDLUDP.TimeBetwnNext(1);
                }
                else return false;
                #endregion
                MyRead2UpFlags[0] = true;
            }

            byte[] ArayMode = new byte[wdMaxValue]; //按键模式
            byte[] ArayDelay = new byte[wdMaxValue * 2];
            if (intActivePage == 0 || intActivePage == 2)
            {
                #region
                myKeySetting = new List<Key>();
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xE008, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType)) == true)
                {
                    if (wdDeviceType == 3076)
                    {
                        for (int intJ = 0; intJ < wdMaxValue; intJ++)
                        {
                            ArayMode[intJ] = CsConst.myRevBuf[25 + intJ];
                        }
                    }
                    else if (wdDeviceType == 3078 || wdDeviceType==3080)
                    {
                        for (int intJ = 0; intJ < wdMaxValue; intJ++)
                        {
                            ArayMode[intJ] = CsConst.myRevBuf[25 + intJ];
                        }
                    }
                    else if (wdDeviceType == 3064 || wdDeviceType == 3065 || wdDeviceType == 3068 || wdDeviceType == 3069)
                    {
                        for (int intJ = 0; intJ < wdMaxValue; intJ++)
                        {
                            ArayMode[intJ] = CsConst.myRevBuf[25 + intJ];
                        }
                    }
                    
                    HDLUDP.TimeBetwnNext(1);
                }
                else return false;
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(15, null);

                if (wdDeviceType != 3058 )
                {
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x2024, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType)) == true)
                    {
                        Array.Copy(CsConst.myRevBuf, 26, ArayDelay, 0, ArayDelay.Length);
                        
                        HDLUDP.TimeBetwnNext(1);
                    }
                    else return false;
                }
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(20, null);

                for (int intI = 0; intI < wdMaxValue; intI++)
                {
                    ArayTmp = new byte[1];

                    Key oKey = new Key();
                    oKey.ID = (Byte)(intI + 1);
                    ArayTmp[0] = oKey.ID;

                    if (oKey.ReadButtonRemarkAndCMDFromDevice(bytSubID, bytDevID, wdDeviceType, -1, 255, false, 0, 0) == false) return false;
                    
                    oKey.Delay = ArayDelay[intI * 2] * 256 + ArayDelay[(intI * 2) + 1];
                    oKey.Mode = ArayMode[intI];
                    oKey.KeyTargets = new List<UVCMD.ControlTargets>();

                    if (oKey.Mode == 10 || oKey.Mode == 14)
                    {
                        for (byte byt = 50; byt <= 98; byt++)
                        {
                            byte[] arayTmp = new byte[2];
                            arayTmp[0] = Convert.ToByte(oKey.ID);
                            arayTmp[1] = byt;
                            if (CsConst.mySends.AddBufToSndList(arayTmp, 0xE000, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType)) == true)
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
                                    
                                oKey.KeyTargets.Add(oCMD);
                                HDLUDP.TimeBetwnNext(1);
                            }
                            else return false;                                
                        }
                    }
                    myKeySetting.Add(oKey);
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(30 + 5 * intI, null);
                }
                #endregion
                MyRead2UpFlags[1] = true;
            }

            if (intActivePage == 0 || intActivePage == 3)
            {
                #region
                if (wdDeviceType == 3076 || wdDeviceType == 3078 || wdDeviceType == 3080)
                {
                    if (wdDeviceType == 3076 || wdDeviceType == 3078 || wdDeviceType == 3080) wdMaxValue = 3;
                    arayButtonSensitiVity = new byte[wdMaxValue];
                    arayButtonColor = new byte[wdMaxValue * 6];
                    arayButtonBalance = new byte[wdMaxValue * 3 + 1];


                    ArayTmp = null;
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xE150, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType)) == true)
                    {
                        Array.Copy(CsConst.myRevBuf, 25, arayButtonSensitiVity, 0, wdMaxValue);
                        
                        HDLUDP.TimeBetwnNext(1);
                    }
                    else return false;

                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(81);

                    ArayTmp = new byte[1];
                    for (int i = 1; i <= wdMaxValue; i++)
                    {
                        ArayTmp[0] = Convert.ToByte(i);
                        if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xE14C, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType)) == true)
                        {
                            Array.Copy(CsConst.myRevBuf, 26, arayButtonColor, (i - 1) * 6, 6);
                            
                            HDLUDP.TimeBetwnNext(1);
                        }
                        else return false;
                    }
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(82);


                    ArayTmp = null;
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1998, bytSubID, bytDevID, false, false, true, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType)) == true)
                    {
                        for (int i = 1; i <= wdMaxValue; i++)
                        {
                            arayButtonBalance[0] = Convert.ToByte(i);
                            Array.Copy(CsConst.myRevBuf, 25 + (i - 1) * 3, arayButtonBalance, (i - 1) * 3 + 1, 3);
                        }
                        
                        HDLUDP.TimeBetwnNext(1);
                    }
                    MyRead2UpFlags[2] = true;
                }
                #endregion
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(100, null);
            return true;
        }
    }
}
