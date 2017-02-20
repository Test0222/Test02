using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.Windows.Forms;

namespace HDL_Buspro_Setup_Tool
{
    [Serializable]
    public class GPRS : HdlDeviceBackupAndRestore
    {
        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public int DIndex;
        public string Devname;//子网ID 设备ID 备注
        public string strIP;  // net work information
        public string strRouterIP;
        public string strMAC;

        public DateTime strDate; // current date 
        public DateTime strTime; // current time adjust it to standrad one

        public string strSevCentre; // server center
        public string strCode; // country code
        public string strIMEI; // IMEI code for network , unique for all 

        public bool blnTo485;

        public List<SMSControls> MyControls;
        public List<SendSMS> MySendSMS;

        public bool[] MyRead2UpFlags = new bool[] { false, false, false,false };

        public class SMSControls
        {
            public int ID;
            public string strRemark;
            public string strSMSContent;
            public List<PhoneInF> MyVerify;
            public bool blnIsVerify;
            public bool blnReply;
            public List<UVCMD.ControlTargets> MyTargets;
        }

        public class PhoneInF
        {
            public int ID;
            public string Remark;
            public string PhoneNum;
            public string strSMS; // 对于控制来说，这个无意义； 对于发送来说，这个表示发送的内容
            public bool Valid;
        }

        public class SendSMS
        {
            public int ID;
            public string strRemark;
            public List<PhoneInF> MyGuests;
        }

        ///<summary>
        ///读取默认的设置
        ///</summary>
        public void ReadDefaultInfo()
        {

            if (MyControls == null) MyControls = new List<SMSControls>();
            if (MySendSMS == null) MySendSMS = new List<SendSMS>();
            SMSControls oSMSControl = new SMSControls();
            oSMSControl.MyVerify = new List<PhoneInF>();
            oSMSControl.MyTargets = new List<UVCMD.ControlTargets>();
            SendSMS oSendSMS = new SendSMS();
            oSendSMS.MyGuests = new List<PhoneInF>();
        }
        ///<summary>
        ///读取数据库面板设置，将所有数据读至缓存
        ///</summary>
        public void ReadSensorInfoFromDB(int DIndex)
        {
            //read GPRS Info
            #region
            string str = string.Format("select * from dbGPRSbasic where DIndex={0}", DIndex);
            OleDbDataReader dr = DataModule.SearchAResultSQLDB(str, CsConst.mstrCurPath);
            if (dr == null) return;
            while (dr.Read())
            {
                strIP = dr.GetString(1);
                strRouterIP = dr.GetString(2);
                strMAC = dr.GetString(3);
                strSevCentre = dr.GetString(4);
                strCode = dr.GetString(5);
                strIMEI = dr.GetString(6);
                blnTo485 = dr.GetBoolean(7);
            }
            dr.Close();
            #endregion

            //read GPRS Control Info
            #region
            MyControls = new List<SMSControls>();
            str = string.Format("select * from dbGPRSControls where DIndex={0} order by SenNum", DIndex);
            dr = DataModule.SearchAResultSQLDB(str, CsConst.mstrCurPath);
            if (dr != null)
            {
                while (dr.Read())
                {
                    SMSControls temp = new SMSControls();
                    temp.MyVerify = new List<PhoneInF>();
                    temp.MyTargets = new List<UVCMD.ControlTargets>();
                    temp.ID = dr.GetInt16(1);
                    temp.strRemark = dr.GetString(2);
                    temp.strSMSContent = dr.GetString(3);
                    temp.blnIsVerify = dr.GetBoolean(4);
                    temp.blnReply = dr.GetBoolean(5);

                    #region
                    if (temp.blnIsVerify == true)
                    {
                        str = string.Format("select * from dbGPRSVerify where DIndex={0} and SenNum ={1} order by VerifyNum", DIndex, temp.ID);
                        OleDbDataReader drVerify = DataModule.SearchAResultSQLDB(str, CsConst.mstrCurPath);
                        if (drVerify != null)
                        {
                            while (drVerify.Read())
                            {
                                PhoneInF tmp = new PhoneInF();
                                tmp.ID = drVerify.GetInt16(2);
                                tmp.Remark = drVerify.GetString(3);
                                tmp.PhoneNum = drVerify.GetString(4);
                                temp.MyVerify.Add(tmp);
                            }
                            drVerify.Close();
                        }
                    }
                    #endregion

                    str = string.Format("select * from dbKeyTargets where DIndex ={0} and KeyIndex = {1} order by objID", DIndex, temp.ID);
                    OleDbDataReader drKeyCmds = DataModule.SearchAResultSQLDB(str, CsConst.mstrCurPath);
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
                            temp.MyTargets.Add(TmpCmd);
                        }
                        drKeyCmds.Close();
                        MyControls.Add(temp);
                    }
                }
                dr.Close();
            }
            #endregion

            //read GPRS SMS Send Info
            #region
            MySendSMS = new List<SendSMS>();
            str = string.Format("select * from dbGPRSSendSMS where DIndex = {0} order by SenNum", DIndex);
            dr = DataModule.SearchAResultSQLDB(str, CsConst.mstrCurPath);
            if (dr != null)
            {
                while (dr.Read())
                {
                    SendSMS temp = new SendSMS();
                    temp.MyGuests = new List<PhoneInF>();
                    temp.ID = dr.GetInt16(1);
                    temp.strRemark = dr.GetString(2);

                    str = string.Format("select * from dbGPRSSendSMSInfo where DIndex={0} and SenNum ={1} order by TargetNum", DIndex, temp.ID);
                    OleDbDataReader drSend = DataModule.SearchAResultSQLDB(str, CsConst.mstrCurPath);
                    if (drSend != null)
                    {
                        while (drSend.Read())
                        {
                            PhoneInF tmp = new PhoneInF();
                            tmp.ID = drSend.GetInt16(2);
                            tmp.Remark = drSend.GetString(3);
                            tmp.PhoneNum = drSend.GetString(4);
                            tmp.strSMS = drSend.GetString(5);
                            temp.MyGuests.Add(tmp);
                        }
                        drSend.Close();
                    }
                    MySendSMS.Add(temp);
                }
                dr.Close();
            }
            #endregion
        }

        ///<summary>
        ///保存数据库面板设置，将所有数据保存
        ///</summary>
        public void SaveSensorInfoToDB()
        {
            //delete GPRS Info
            #region
            string strsql = string.Format("delete * from dbGPRSbasic where DIndex={0}", DIndex);
            DataModule.ExecuteSQLDatabase(strsql);

            //delete GPRS Control Info
            strsql = string.Format("delete * from dbGPRSControls where DIndex ={0}", DIndex);
            DataModule.ExecuteSQLDatabase(strsql);

            strsql = string.Format("delete * from dbGPRSVerify where DIndex={0}", DIndex);
            DataModule.ExecuteSQLDatabase(strsql);

            strsql = string.Format("delete * from dbGPRSTargets where DIndex={0}", DIndex);
            DataModule.ExecuteSQLDatabase(strsql);

            strsql = string.Format("delete * form dbGPRSSendSMS where DIndex={0}", DIndex);
            DataModule.ExecuteSQLDatabase(strsql);

            strsql = string.Format("delete * from dbGPRSSendSMSInfo where DIndex={0}", DIndex);
            DataModule.ExecuteSQLDatabase(strsql);
            #endregion

            //save GPRS Info
            #region
            strsql = string.Format("insert into dbGPRSbasic(DIndex,strIP,strRouterIP,strMAC,strSevCentre,strCode,strIMEI,blnTo485)values({0},'{1}','{2}','{3}','{4}','{5}','{6}',{7})",
                                   DIndex, strIP, strRouterIP, strMAC, strSevCentre, strCode, strIMEI, blnTo485);
            DataModule.ExecuteSQLDatabase(strsql);
            #endregion

            //save GPRS SMS Control Info
            #region
            if (MyControls != null)
            {
                foreach (SMSControls temp in MyControls)
                {
                    strsql = string.Format("insert into dbGPRSControls(DIndex,SenNum,strRemark,strSMSContent,blnIsVerify,blnReply)values({0},{1},'{2}',"
                             + "'{3}',{4},{5})", DIndex, temp.ID, temp.strRemark, temp.strSMSContent, temp.blnIsVerify, temp.blnReply);
                    DataModule.ExecuteSQLDatabase(strsql);
                    if (temp.MyVerify != null)
                    {
                        for (int i = 0; i < temp.MyVerify.Count; i++)
                        {
                            PhoneInF tmp = temp.MyVerify[i];
                            strsql = string.Format("insert into dbGPRSVerify(DIndex,SenNum,VerifyNum,Remark,PhoneNum)values({0},{1},{2},'{3}','{4}')", DIndex, temp.ID, i,
                                tmp.Remark, tmp.PhoneNum);
                            DataModule.ExecuteSQLDatabase(strsql);
                        }
                    }
                    if (temp.MyTargets != null)
                    {
                        for (int i = 0; i < temp.MyTargets.Count; i++)
                        {
                            UVCMD.ControlTargets TmpCmds = temp.MyTargets[i];
                            strsql = string.Format("Insert into dbKeyTargets(DIndex,KeyIndex,objID,KeyFunType,SubNetID,DeviceID,FirstParameter,"
                                       + "SecondParameter,RunTimeMinute,RunTimeSecond,Ms04State) values ({0},{1},'{2}',{3},{4},{5},{6},{7},{8},{9},{10})",
                                       DIndex, temp.ID, TmpCmds.ID, TmpCmds.Type, TmpCmds.SubnetID, TmpCmds.DeviceID, TmpCmds.Param1,
                                       TmpCmds.Param2, TmpCmds.Param3, TmpCmds.Param4, 0);
                            DataModule.ExecuteSQLDatabase(strsql);
                        }
                    }
                }
            }
            #endregion

            //save GPRS SMS Send Info
            #region
            if (MySendSMS != null)
            {
                foreach (SendSMS temp in MySendSMS)
                {
                    strsql = string.Format("insert into dbGPRSSendSMS(DIndex,SenNum,Remark)values({0},{1},'{2}')", DIndex, temp.ID, temp.strRemark);
                    DataModule.ExecuteSQLDatabase(strsql);
                    if (temp.MyGuests != null)
                    {
                        for (int i = 0; i < temp.MyGuests.Count; i++)
                        {
                            PhoneInF tmp = temp.MyGuests[i];
                            strsql = string.Format("insert into dbGPRSSendSMSInfo(DIndex,SenNum,TargetNum,Remark,PhoneNum,SMS)values({0},{1},{2},'{3}','{4}','{5}')", DIndex,
                                temp.ID, i + 1, tmp.Remark, tmp.PhoneNum, tmp.strSMS);
                            DataModule.ExecuteSQLDatabase(strsql);
                        }
                    }
                }
            }
            #endregion
        }

        /// <summary>
        /// download all information from hardware 
        /// </summary>
        /// <param name="DevName"></param>
        public void DownloadSMSFrmDeviceToBuf(string DevName, int wdDeviceType, int intActivePage,int num1,int num2)// 0 mean all, else that tab only
        {
            string strMainRemark = DevName.Split('\\')[1].Trim();
            DevName = DevName.Split('\\')[0].Trim();

            //保存basic informations网络信息
            byte bytSubID = byte.Parse(DevName.Split('-')[0].ToString());
            byte bytDevID = byte.Parse(DevName.Split('-')[1].ToString());

            // read basic paramters 
            byte[] ArayTmp = null;

            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x000E, bytSubID, bytDevID, false, true, true, false) == true)
            {
                byte[] arayRemark = new byte[20];
                for (int intI = 0; intI < 20; intI++) { arayRemark[intI] = CsConst.myRevBuf[25 + intI]; }
                Devname = bytSubID.ToString() + "-" + bytDevID.ToString() + "\\" + HDLPF.Byte2String(arayRemark);
                
            }
            else return;
            
            ArayTmp = new byte[3];
            ArayTmp[0] = 0;
            ArayTmp[1] = 1;
            if (intActivePage == 0 || intActivePage == 1)
            {
                if (CsConst.isRestore)
                {
                    num1 = 0;
                    num2 = 99;
                    if (wdDeviceType == 4001) num2 = 90;
                }
                MyControls = new List<SMSControls>();
                #region
                for (byte bytI = (byte)num1; bytI < num2; bytI++)
                {
                    ArayTmp[2] = bytI;
                    SMSControls oTmpSMS = new SMSControls();
                    oTmpSMS.ID = bytI;
                    oTmpSMS.MyVerify = new List<PhoneInF>();
                    oTmpSMS.MyTargets = new List<UVCMD.ControlTargets>();

                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xC017, bytSubID, bytDevID, true, true, true, false) == true)
                    {
                        // 20 bytes remark
                        byte[] arayRemark = new byte[20];
                        for (int intI = 0; intI < 20; intI++) { arayRemark[intI] = CsConst.myRevBuf[28 + intI]; }
                        oTmpSMS.strRemark = HDLPF.Byte2String(arayRemark);

                        // 30 bytes sms content unicode
                        arayRemark = new byte[32];
                        for (int intI = 0; intI < 32; intI++) { arayRemark[intI] = CsConst.myRevBuf[48 + intI]; }
                        oTmpSMS.strSMSContent = HDLPF.Byte22String(arayRemark, true);

                        // 判断是否验证手机号码
                        #region
                        oTmpSMS.blnIsVerify = (CsConst.myRevBuf[896] == 0x11);
                        oTmpSMS.blnReply = (CsConst.myRevBuf[897] == 0x11);

                        for (int intJ = 0; intJ < 12; intJ++)
                        {
                            PhoneInF oPhone = new PhoneInF();
                            arayRemark = new byte[20];
                            for (int intI = 0; intI < 20; intI++) { arayRemark[intI] = CsConst.myRevBuf[80 + intJ * 36 + intI]; }
                            oPhone.Remark = HDLPF.Byte2String(arayRemark);

                            arayRemark = new byte[16];
                            for (int intI = 0; intI < 16; intI++)
                            {
                                if (CsConst.myRevBuf[100 + intJ * 36 + intI] != 0x45)
                                    arayRemark[intI] = CsConst.myRevBuf[100 + intJ * 36 + intI];
                                else
                                    arayRemark[intI] = 0;
                            }
                            oPhone.PhoneNum = HDLPF.Byte2String(arayRemark);
                            oTmpSMS.MyVerify.Add(oPhone);
                        }

                        #endregion

                        // 判断是否存在控制目标
                        #region
                        int intTmpST = 512;
                        for (int intI = 0; intI < 48; intI++)
                        {
                            //if (CsConst.myRevBuf[intTmpST + intI * 8 + 7] == 0xAA) //只保留有效
                            //{
                            UVCMD.ControlTargets oCMD = new UVCMD.ControlTargets();

                            oCMD.SubnetID = CsConst.myRevBuf[intTmpST + intI * 8];
                            oCMD.DeviceID = CsConst.myRevBuf[intTmpST + intI * 8 + 1];
                            oCMD.Type = CsConst.myRevBuf[intTmpST + intI * 8 + 2];
                            //oCMD.Type = HDLSysPF.GetIDAccordinglyRightControlTypeSMS(CsConst.myRevBuf[intTmpST + intI * 8 + 2]);  //转换为正确的类型
                            oCMD.Param1 = CsConst.myRevBuf[intTmpST + intI * 8 + 3];
                            oCMD.Param2 = CsConst.myRevBuf[intTmpST + intI * 8 + 4];
                            oCMD.Param3 = CsConst.myRevBuf[intTmpST + intI * 8 + 5];
                            oCMD.Param4 = CsConst.myRevBuf[intTmpST + intI * 8 + 6];
                            oCMD.IsValid = (CsConst.myRevBuf[intTmpST + intI * 8 + 7] == 0xAA);
                            oTmpSMS.MyTargets.Add(oCMD);
                            //}
                        }
                        #endregion
                        MyControls.Add(oTmpSMS);
                    }
                    else return;
                    
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(bytI / 2, null);
                    HDLUDP.TimeBetwnNext(20);
                }
                #endregion
                MyRead2UpFlags[0] = true;
            }

            if (intActivePage == 0 || intActivePage == 2)
            {
                // send message when needs
                #region
                ArayTmp = new byte[4];
                ArayTmp[0] = 0;
                ArayTmp[1] = 2;

                MySendSMS = new List<SendSMS>();
                if (CsConst.isRestore)
                {
                    num1 = 0;
                    num2 = 20;
                    if (wdDeviceType == 4001) num2 = 90;
                }
                for (byte bytI = (byte)num1; bytI < num2; bytI++)
                {
                    ArayTmp[2] = bytI;
                    byte[] TmpSMS = new byte[1794];

                    SendSMS oTmpSMS = new SendSMS();
                    oTmpSMS.ID = bytI;
                    oTmpSMS.MyGuests = new List<PhoneInF>();

                    if (wdDeviceType == 4001)
                    {
                        if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xC060, bytSubID, bytDevID, true, true, true, false) == true)
                        {
                            Array.Copy(CsConst.myRevBuf, 29, TmpSMS, 0, 598);
                            
                            HDLUDP.TimeBetwnNext(20);
                        }
                        else return;
                    }
                    else
                    {
                        for (int intI = 0; intI < 3; intI++) //分三次读取数据
                        {
                            ArayTmp[3] = Convert.ToByte(intI);
                            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xC060, bytSubID, bytDevID, true, true, true, false) == true)
                            {
                                Array.Copy(CsConst.myRevBuf, 29, TmpSMS, intI * 598, 598);
                                
                                HDLUDP.TimeBetwnNext(20);
                            }
                            else return;
                        }
                    }
                    // 20 bytes remark
                    byte[] arayRemark = new byte[20];
                    for (int intI = 0; intI < 20; intI++) { arayRemark[intI] = TmpSMS[intI]; }
                    oTmpSMS.strRemark = HDLPF.Byte2String(arayRemark);

                    // 判断接收的手机号码
                    #region
                    int num = 10;
                    if (wdDeviceType == 4001) num = 3;
                    for (int intJ = 0; intJ < num; intJ++)
                    {
                        PhoneInF oPhone = new PhoneInF();
                        if (TmpSMS[200 + intJ * 177] == 0xAA) oPhone.Valid = true;
                        else oPhone.Valid = false;
                        oPhone.ID = Convert.ToByte(intJ);
                        arayRemark = new byte[20];
                        for (int intI = 0; intI < 20; intI++) { arayRemark[intI] = TmpSMS[24 + intJ * 177 + intI]; }
                        oPhone.Remark = HDLPF.Byte2String(arayRemark);

                        arayRemark = new byte[16];
                        for (int intI = 0; intI < 16; intI++)
                        {
                            if (TmpSMS[44 + intJ * 177 + intI] != 0x45)
                                arayRemark[intI] = TmpSMS[44 + intJ * 177 + intI];
                            else
                                arayRemark[intI] = 0;
                        }
                        oPhone.PhoneNum = HDLPF.Byte2String(arayRemark);

                        arayRemark = new byte[140];
                        for (int intI = 0; intI < 140; intI++) { arayRemark[intI] = TmpSMS[60 + intJ * 177 + intI]; }
                        oPhone.strSMS = HDLPF.Byte22String(arayRemark, true);
                        oTmpSMS.MyGuests.Add(oPhone);
                    }
                    #endregion

                    MySendSMS.Add(oTmpSMS);
                    if (CsConst.calculationWorker != null) CsConst.calculationWorker.ReportProgress(50 + bytI / 2, null);
                }
                #endregion
                MyRead2UpFlags[1] = true;
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(100, null);
        }

        /// <summary>
        /// Upload all information from hardware 
        /// </summary>
        /// <param name="DevName"></param>
        public void UploadSMSFrmDeviceToBuf(string DevName, int wdDeviceType, int intActivePage)// 0 mean all, else that tab only
        {
            string strRemark = DevName.Split('\\')[1].Trim().Split('(')[0].Trim();
            DevName = DevName.Split('\\')[0].Trim();

            //保存basic informations网络信息
            byte bytSubID = byte.Parse(DevName.Split('-')[0].ToString());
            byte bytDevID = byte.Parse(DevName.Split('-')[1].ToString());

            byte[] ArayMain = new byte[20];
            byte[] arayTmp = HDLUDP.StringToByte(strRemark);
            if (arayTmp.Length > 20)
            {
                Array.Copy(arayTmp, 0, ArayMain, 0, 20);
            }
            else
            {
                Array.Copy(arayTmp, 0, ArayMain, 0, arayTmp.Length);
            }
            if (CsConst.isRestore)
            {
                if (CsConst.mySends.AddBufToSndList(ArayMain, 0x0010, bytSubID, bytDevID, false, true, true, false) == true)
                {
                    HDLUDP.TimeBetwnNext(20);
                }
                else return;
            }
            //save receive message and control HDL devices
            if (intActivePage == 0 || intActivePage == 1)
            {
                #region
                byte bytID = 0;
                if (MyControls != null)
                {
                    foreach (SMSControls oTmpSMS in MyControls)
                    {
                        ArayMain = new byte[871];
                        //添加备注到缓存
                        ArayMain[0] = Convert.ToByte(oTmpSMS.ID);
                        strRemark = oTmpSMS.strRemark;
                        byte[] ArayRemark = HDLUDP.StringToByte(strRemark);
                        if (ArayRemark.Length <= 20)
                            Array.Copy(ArayRemark, 0, ArayMain, 1, ArayRemark.Length);
                        else
                            Array.Copy(ArayRemark, 0, ArayMain, 1, 20);
                        //添加短信内容到缓存
                        ArayRemark = HDLUDP.StringTo2Byte(oTmpSMS.strSMSContent, true);
                        Array.Copy(ArayRemark, 0, ArayMain, 21, ArayRemark.Length);

                        //添加12组信息到缓存 类似与初始化数据
                        for (int intI = 0; intI < 12; intI++)
                        {
                            for (int intJ = 0; intJ < 20; intJ++) { ArayMain[53 + intI * 36 + intJ] = 0; }
                            for (int intJ = 0; intJ < 16; intJ++) { ArayMain[73 + intI * 36 + intJ] = 0x45; }
                        }

                        if (oTmpSMS.MyVerify != null)
                        {
                            int intID = 0;
                            foreach (PhoneInF oPhone in oTmpSMS.MyVerify)
                            {
                                ArayRemark = HDLUDP.StringToByte(oPhone.Remark);
                                if (ArayRemark.Length <= 20)
                                    Array.Copy(ArayRemark, 0, ArayMain, 53 + intID * 36, ArayRemark.Length);
                                else
                                    Array.Copy(ArayRemark, 0, ArayMain, 53 + intID * 36, 20);

                                ArayRemark = HDLUDP.StringToByte(oPhone.PhoneNum);
                                if (ArayRemark.Length <= 16)
                                    Array.Copy(ArayRemark, 0, ArayMain, 73 + intID * 36, ArayRemark.Length);
                                else
                                    Array.Copy(ArayRemark, 0, ArayMain, 73 + intID * 36, 16);
                                intID++;
                            }
                        }

                        //先将48个目标变为无效
                        for (int intI = 0; intI < 48; intI++)
                        {
                            ArayMain[492 + intI * 8] = 0x55;
                        }

                        if (oTmpSMS.MyTargets != null && oTmpSMS.MyTargets.Count != 0)
                        {
                            int intID = 0;
                            foreach (UVCMD.ControlTargets oCMD in oTmpSMS.MyTargets)
                            {
                                ArayMain[485 + intID * 8] = oCMD.SubnetID;
                                ArayMain[486 + intID * 8] = oCMD.DeviceID;
                                ArayMain[487 + intID * 8] = oCMD.Type;

                                ArayMain[488 + intID * 8] = oCMD.Param1;
                                ArayMain[489 + intID * 8] = oCMD.Param2;
                                ArayMain[490 + intID * 8] = oCMD.Param3;
                                ArayMain[491 + intID * 8] = oCMD.Param4;
                                if (oCMD.IsValid == true) ArayMain[492 + intID * 8] = 0xAA;
                                else ArayMain[492 + intID * 8] = 0x55;

                                intID++;
                            }
                        }
                        if (oTmpSMS.blnIsVerify)
                            ArayMain[869] = 0x11;
                        else
                            ArayMain[869] = 0x10;

                        if (oTmpSMS.blnReply)
                            ArayMain[870] = 0x11;
                        else
                            ArayMain[870] = 0x10;

                        //由于是大包 增加包头处理
                        arayTmp = new byte[873];
                        arayTmp[0] = 871 / 256;
                        arayTmp[1] = 871 % 256;
                        Array.Copy(ArayMain, 0, arayTmp, 2, 871);
                        if (CsConst.mySends.AddBufToSndList(arayTmp, 0xC019, bytSubID, bytDevID, true, true, true, false) == true)
                        {
                            
                            HDLUDP.TimeBetwnNext(20);
                        }
                        else return;
                        bytID++;
                        if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(bytID / 2, null);
                        
                    }
                }
                #endregion
            }

            //// send message when needs
            if (intActivePage == 0 || intActivePage == 2)
            {
                #region
                if (MySendSMS != null && MySendSMS.Count != 0)
                {
                    foreach (SendSMS oSend in MySendSMS)
                    {
                        ArayMain = new byte[1794];  // 用于存放数据
                        // 20 bytes remark
                        byte[] ArayRemark = HDLUDP.StringToByte(oSend.strRemark);
                        if (ArayRemark.Length <= 20)
                            Array.Copy(ArayRemark, 0, ArayMain, 0, ArayRemark.Length);
                        else
                            Array.Copy(ArayRemark, 0, ArayMain, 0, 20);
                        // 判断接收的手机号码
                        #region
                        for (int intJ = 0; intJ < 10; intJ++)
                        {
                            ArayMain[200 + intJ * 177] = 0x55;
                        }
                        if (oSend.MyGuests != null && oSend.MyGuests.Count != 0)
                        {
                            int intID = 0;
                            foreach (PhoneInF oPhone in oSend.MyGuests)
                            {
                                if (oPhone.Valid) ArayMain[200 + intID * 177] = 0xAA;
                                ArayRemark = HDLUDP.StringToByte(oPhone.Remark);
                                if (ArayRemark.Length <= 20)
                                    Array.Copy(ArayRemark, 0, ArayMain, 24 + intID * 177, ArayRemark.Length);
                                else
                                    Array.Copy(ArayRemark, 0, ArayMain, 24 + intID * 177, 20);
                                ArayRemark = HDLUDP.StringToByte(oPhone.PhoneNum);
                                if (ArayRemark.Length <= 16)
                                    Array.Copy(ArayRemark, 0, ArayMain, 44 + intID * 177, ArayRemark.Length);
                                else
                                    Array.Copy(ArayRemark, 0, ArayMain, 44 + intID * 177, 16);

                                ArayRemark = HDLUDP.StringTo2Byte(oPhone.strSMS, true);
                                Array.Copy(ArayRemark, 0, ArayMain, 60 + intID * 177, ArayRemark.Length);

                                intID++;
                            }
                        }
                        #endregion

                        //发送数据
                        byte[] ArayTmp = new byte[602];

                        ArayTmp[0] = Convert.ToByte(600 / 256);
                        ArayTmp[1] = Convert.ToByte(600 % 256);

                        if (wdDeviceType == 4001)
                        {
                            ArayTmp[2] = Convert.ToByte(oSend.ID);
                            Array.Copy(ArayMain,  0, ArayTmp, 4, 598);

                            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xC062, bytSubID, bytDevID, true, true, true, false) == true)
                            {
                                HDLUDP.TimeBetwnNext(20);
                            }
                            else return;
                        }
                        else
                        {
                            for (int intI = 0; intI < 3; intI++)
                            {
                                ArayTmp[2] = Convert.ToByte(oSend.ID);
                                ArayTmp[3] = Convert.ToByte(intI);
                                Array.Copy(ArayMain, intI * 598, ArayTmp, 4, 598);

                                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xC062, bytSubID, bytDevID, true, true, true, false) == true)
                                {
                                    HDLUDP.TimeBetwnNext(20);
                                }
                                else return;
                            }
                        }
                        if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(50 + (oSend.ID / 2), null);
                    }
                }
                #endregion
            }

            //更新缓存
            arayTmp = null;
            if (CsConst.mySends.AddBufToSndList(arayTmp, 0xC01B, bytSubID, bytDevID, true, false, true, false) == true)
            { }


            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(100, null);
        }
    }
}
