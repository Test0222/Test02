using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HDL_Buspro_Setup_Tool
{
    [Serializable]
    public class NewDS : HdlDeviceBackupAndRestore
    {
        public int DIndex;//设备唯一编号
        public bool[] MyRead2UpFlags = new bool[10]; //
        //基本信息
        public string Devname;//子网ID 设备ID 备注
        public List<CardInfo> MyCardInfo;
        public List<History> MyHistory;
        public int HistoryCount;
        public string strNO;//呼叫号
        public string strPassword;
        public Byte bEnableCard; //  刷卡使能标志
        public byte[] arayBasic = new byte[5];//卡平常色 +刷卡后色  触摸平常色+ 触摸后颜色 +蜂鸣器使能 0/1   +
        public byte[] arayInfo = new byte[20];//关门延时，继电器输出，按键灵敏度,铃声，音量 剩下预留
        public byte[] arayCall = new byte[20];//4个区域代码，本地呼叫使能，通话时间，服务器呼叫使能，服务器地址
        
        public class CardInfo
        {
            public byte UIDL;
            public byte[] UID;
            public byte CardType;
            public int CardNum;
            public int BuildingNO;
            public int UnitNO;
            public int RoomNO;
            public byte[] arayDate;
            public byte[] arayPhone;
            public byte[] arayName;
            public string Remark;
        }

        public class History
        {
            public int ID;
            public byte[] arayDate;
            public byte Type;
            public byte[] arayInfo;
        }
        //<summary>
        //读取默认设置
        //</summary>
        public void ReadDefaultInfo()
        {
            MyCardInfo = new List<CardInfo>();
            MyHistory = new List<History>();
        }

        //<summary>
        //读取数据信息
        //</summary>
        public void ReadDataFrmDBTobuf(int DIndex)
        {
            try
            {
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
            byte[] arayTmp = new byte[20];

            if (HDLSysPF.ModifyDeviceMainRemark(bytSubID, bytDevID, strMainRemark,wdDeviceType) == true)
            {
                HDLUDP.TimeBetwnNext(20);
            }
            else return false;
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(5, null);
            if (intActivePage == 0 || intActivePage == 1)
            {
                if (strNO == "") strNO = "0000";
                if (strPassword == "") strPassword = "000000";
                if (strNO.Length < 4) strNO = GlobalClass.AddLeftZero(strNO, 4);
                if (strPassword.Length < 4) strPassword = GlobalClass.AddLeftZero(strPassword, 6);
                arayTmp = HDLUDP.StringToByte(strNO + strPassword);
                if (CsConst.mySends.AddBufToSndList(arayTmp, 0x352F, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType)) == true)
                {
                    
                    HDLUDP.TimeBetwnNext(20);
                }
                else return false;
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(6, null);


                if (CsConst.mySends.AddBufToSndList(arayBasic, 0x3527, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType)) == true)
                {
                    
                    HDLUDP.TimeBetwnNext(20);
                }
                else return false;
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(6, null);

                arayTmp = new byte[2];
                arayTmp[0] = arayInfo[0];
                arayTmp[1] = arayInfo[1];
                if (CsConst.mySends.AddBufToSndList(arayTmp, 0x350E, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType)) == true)
                {
                    
                    HDLUDP.TimeBetwnNext(20);
                }
                else return false;
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(7, null);

                arayTmp = new byte[11];
                Array.Copy(arayCall, 0, arayTmp, 0, 11);
                if (CsConst.mySends.AddBufToSndList(arayTmp, 0x138C, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType)) == true)
                {
                    
                    HDLUDP.TimeBetwnNext(20);
                }
                else return false;
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(8, null);
                
                arayTmp = new byte[2];
                arayTmp[0] = arayInfo[3];
                arayTmp[1] = arayInfo[4];
                if (CsConst.mySends.AddBufToSndList(arayTmp, 0x353D, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType)) == true)
                {
                    HDLUDP.TimeBetwnNext(20);
                }
                else return false;

                arayTmp = new byte[2];
                arayTmp[0] = arayInfo[5];
                arayTmp[1] = arayInfo[6];
                if (CsConst.mySends.AddBufToSndList(arayTmp, 0x3340, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType)) == true)
                {
                    HDLUDP.TimeBetwnNext(20);
                }
                else return false;

                arayTmp = new byte[1]{ arayInfo[2]};
                if (CsConst.mySends.AddBufToSndList(arayTmp, 0x3535, bytSubID, bytDevID, false, false, true, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType)) == true)
                {
                    HDLUDP.TimeBetwnNext(20);
                }

                arayTmp = new byte[1] { bEnableCard};
                if (CsConst.mySends.AddBufToSndList(arayTmp, 0x3346, bytSubID, bytDevID, false, false, true, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType)) == true)
                {
                    HDLUDP.TimeBetwnNext(20);
                }
            }

            if (CsConst.isRestore)
            {
                if (intActivePage == 0 || intActivePage == 2)
                {
                    for (int i = 0; i < MyCardInfo.Count; i++)
                    {
                        if (MyCardInfo[i].CardType != 0 && MyCardInfo[i].CardType != 0xFF)
                        {
                            CardInfo tmp = MyCardInfo[i];
                             arayTmp = new byte[64];
                            arayTmp[0] = tmp.UIDL;
                            Array.Copy(tmp.UID, 0, arayTmp, 1, tmp.UIDL);
                            arayTmp[11] = tmp.CardType;
                            arayTmp[12] = Convert.ToByte(tmp.CardNum / 256);
                            arayTmp[13] = Convert.ToByte(tmp.CardNum % 256);
                            arayTmp[14] = Convert.ToByte(tmp.BuildingNO / 256);
                            arayTmp[15] = Convert.ToByte(tmp.BuildingNO % 256);
                            arayTmp[16] = Convert.ToByte(tmp.UnitNO / 256);
                            arayTmp[17] = Convert.ToByte(tmp.UnitNO % 256);
                            arayTmp[18] = Convert.ToByte(tmp.RoomNO / 256);
                            arayTmp[19] = Convert.ToByte(tmp.RoomNO % 256);
                            Array.Copy(tmp.arayDate, 0, arayTmp, 20, tmp.arayDate.Length);
                            Array.Copy(tmp.arayName, 0, arayTmp, 25, tmp.arayName.Length);
                            Array.Copy(tmp.arayPhone, 0, arayTmp, 35, tmp.arayPhone.Length);

                            Byte[] arayTmpRemark = HDLUDP.StringToByte(tmp.Remark);
                            if (arayTmpRemark.Length > 18)
                            {
                                Array.Copy(arayTmpRemark, 0, arayTmp, 46, 18);
                            }
                            else
                            {
                                Array.Copy(arayTmpRemark, 0, arayTmp, 46, arayTmpRemark.Length);
                            }

                            if (CsConst.mySends.AddBufToSndList(arayTmp, 0x3518, bytSubID, bytDevID, false, false, true, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType)) == true)
                            {
                                
                                HDLUDP.TimeBetwnNext(20);
                            }
                            else return false;
                            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(15 + i, null);
                        }
                    }
                }
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(100, null);
            return true;
        }

        public bool DownLoadInfoFrmDevice(string DevNam, int wdDeviceType, int intActivePage, int num1, int num2)
        {
            string strMainRemark = DevNam.Split('\\')[1].Trim();
            DevNam = DevNam.Split('\\')[0].Trim();
            byte bytSubID = byte.Parse(DevNam.Split('-')[0].ToString());
            byte bytDevID = byte.Parse(DevNam.Split('-')[1].ToString());
            byte[] ArayTmp = null;
            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x000E, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType)) == true)
            {
                byte[] arayRemark = new byte[20];
                for (int intI = 0; intI < 20; intI++) { arayRemark[intI] = CsConst.myRevBuf[25 + intI]; }
                DevNam = bytSubID.ToString() + "-" + bytDevID.ToString() + "\\" + HDLPF.Byte2String(arayRemark);

                HDLUDP.TimeBetwnNext(1);
            }
            else return false;

            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(5, null);
            if (intActivePage == 0 || intActivePage == 1)
            {
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x352D, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType)) == true)
                {
                    byte[] arayRemark = new byte[4];
                    Array.Copy(CsConst.myRevBuf, 25, arayRemark, 0, 4);
                    strNO = HDLPF.Byte2String(arayRemark);
                    arayRemark = new byte[6];
                    Array.Copy(CsConst.myRevBuf, 29, arayRemark, 0, 6);
                    strPassword = HDLPF.Byte2String(arayRemark);
                    if (strNO == "") strNO = "0000";
                    if (strNO.Length < 4) strNO = GlobalClass.AddLeftZero(strNO, 4);
                    
                    HDLUDP.TimeBetwnNext(1);
                }
                else return false;
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(6, null);

                arayBasic = new byte[] { 7, 7, 7, 7, 0 };
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x3525, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType)) == true)
                {
                    Array.Copy(CsConst.myRevBuf, 25, arayBasic, 0, 5);
                    if (arayBasic[0] < 1 || arayBasic[0] > 7) arayBasic[0] = 7;
                    if (arayBasic[1] < 1 || arayBasic[1] > 7) arayBasic[1] = 7;
                    if (arayBasic[2] < 1 || arayBasic[2] > 7) arayBasic[2] = 7;
                    if (arayBasic[3] < 1 || arayBasic[3] > 7) arayBasic[3] = 7;
                    
                    HDLUDP.TimeBetwnNext(1);
                }
                else return false;
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(7, null);

                arayInfo = new byte[20];
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x350C, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType)) == true)
                {
                    arayInfo[0] = CsConst.myRevBuf[25];
                    arayInfo[1] = CsConst.myRevBuf[26];
                    
                    HDLUDP.TimeBetwnNext(1);
                }
                else return false;
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(8, null);

                arayCall = new byte[20];
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x138E, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType)) == true)
                {
                    Array.Copy(CsConst.myRevBuf, 25, arayCall, 0, 11);
                    HDLUDP.TimeBetwnNext(1);
                }
                else return false;
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(9, null);
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x3533, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType)) == true)
                {
                    arayInfo[2] = CsConst.myRevBuf[25];
                    HDLUDP.TimeBetwnNext(1);
                }
                else return false;

                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x353B, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType)) == true)
                {
                    arayInfo[3] = CsConst.myRevBuf[25];
                    arayInfo[4] = CsConst.myRevBuf[26];
                    HDLUDP.TimeBetwnNext(1);
                }
                else return false;

                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x3342, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType)) == true)
                {
                    arayInfo[5] = CsConst.myRevBuf[25];
                    arayInfo[6] = CsConst.myRevBuf[26];
                    arayInfo[7] = CsConst.myRevBuf[27];
                    arayInfo[8] = CsConst.myRevBuf[28];
                    arayInfo[9] = CsConst.myRevBuf[29];
                    HDLUDP.TimeBetwnNext(1);
                }
                else return false;

                //刷卡使能位
                if (CsConst.mySends.AddBufToSndList(null, 0x3344, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType)) == true)
                {
                    bEnableCard = CsConst.myRevBuf[25];
                    HDLUDP.TimeBetwnNext(1);
                }
                else return false;
            }
            if (intActivePage == 0 || intActivePage == 2)
            {
                MyCardInfo = new List<CardInfo>();
                for (int i = 0; i < 32; i++)
                {
                    CardInfo temp = new CardInfo();
                    temp.UID = new byte[10];
                    temp.UIDL = 0;
                    temp.CardType = 0;
                    temp.CardNum = Convert.ToByte(i + 1);
                    temp.BuildingNO = 0;
                    temp.RoomNO = 0;
                    temp.UnitNO = 0;
                    temp.arayDate = new byte[5];
                    temp.arayPhone = new byte[11];
                    temp.arayName = new byte[10];
                    temp.Remark = "";
                    MyCardInfo.Add(temp);
                }

                ArayTmp = new byte[4];
                ArayTmp[0] = 0;
                ArayTmp[1] = 0;
                ArayTmp[2] = 0;
                ArayTmp[3] = 32;
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xE474, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType)) == true)
                {
                    byte[] arayValid = new byte[4];
                    Array.Copy(CsConst.myRevBuf, 29, arayValid, 0, 4);
                    string strEnable = "";
                    for (int i = 0; i < 4; i++)
                    {
                        string strTmp = GlobalClass.AddLeftZero(Convert.ToString(arayValid[i], 2), 8);
                        for (int j = 7; j >= 0; j--)
                        {
                            string str = strTmp.Substring(j, 1);
                            strEnable = strEnable + str;
                        }
                    }
                    for (int i = 0; i < 32; i++)
                    {
                        if (strEnable.Substring(i, 1) == "1")
                        {
                            ArayTmp = new byte[2];
                            ArayTmp[0] = 0;
                            ArayTmp[1] = Convert.ToByte(i + 1);
                            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x3516, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType)) == true)
                            {
                                MyCardInfo[i].UIDL = CsConst.myRevBuf[26];
                                Array.Copy(CsConst.myRevBuf, 27, MyCardInfo[i].UID, 0, 10);
                                MyCardInfo[i].CardType = CsConst.myRevBuf[37];
                                MyCardInfo[i].BuildingNO = CsConst.myRevBuf[40] * 256 + CsConst.myRevBuf[41];
                                MyCardInfo[i].UnitNO = CsConst.myRevBuf[42] * 256 + CsConst.myRevBuf[43];
                                MyCardInfo[i].RoomNO = CsConst.myRevBuf[44] * 256 + CsConst.myRevBuf[45];
                                Array.Copy(CsConst.myRevBuf, 46, MyCardInfo[i].arayDate, 0, 5);
                                Array.Copy(CsConst.myRevBuf, 51, MyCardInfo[i].arayName, 0, 10);
                                Array.Copy(CsConst.myRevBuf, 61, MyCardInfo[i].arayPhone, 0, 11);
                                byte[] arayRemark = new byte[18];
                                for (int intI = 0; intI < 18; intI++) { arayRemark[intI] = CsConst.myRevBuf[72 + intI]; }
                                MyCardInfo[i].Remark = HDLPF.Byte2String(arayRemark);
                                
                                HDLUDP.TimeBetwnNext(1);
                            }
                            else return false;
                            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(10 + i, null);
                        }
                    }
                }
                else return false;
                MyRead2UpFlags[1] = true;
            }
            if (intActivePage == 0 || intActivePage == 3)
            {
                MyHistory = new List<History>();
                ArayTmp = new byte[2];
                ArayTmp[0] = 0;
                ArayTmp[1] = 1;
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x3512, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType)) == true)
                {
                    if (CsConst.myRevBuf[25] == 0xF8)
                    {
                        HistoryCount = CsConst.myRevBuf[28] * 256 + CsConst.myRevBuf[29];
                    }
                    else
                    {
                        HistoryCount = 0;
                    }
                        
                    HDLUDP.TimeBetwnNext(1);
                    if (HistoryCount > 0)
                    {
                        for (int i = num1; i <= num2; i++)
                        {
                            ArayTmp[0] = Convert.ToByte(i / 256);
                            ArayTmp[1] = Convert.ToByte(i % 256);
                            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x3512, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType)) == true)
                            {
                                if (CsConst.myRevBuf[25] == 0xF8)
                                {
                                    History temp = new History();
                                    temp.ID = i;
                                    temp.arayDate = new byte[6];
                                    Array.Copy(CsConst.myRevBuf, 30, temp.arayDate, 0, 6);
                                    temp.Type = CsConst.myRevBuf[36];
                                    temp.arayInfo = new byte[25];
                                    Array.Copy(CsConst.myRevBuf, 37, temp.arayInfo, 0, 25);
                                    MyHistory.Add(temp);

                                    HDLUDP.TimeBetwnNext(1);
                                }
                                else return false;
                            }
                            else return false;

                            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(50 + (i - num1) * 40 / (num2 - num1+1), null);

                        }
                    }
                }
                else return false;
                MyRead2UpFlags[2] = true;
        }
        if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(100, null);
        return true;
        }
    }
}
