using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Xml;
using System.IO;

namespace HDL_Buspro_Setup_Tool
{
    [Serializable]
    public class DS:NewDS
    {
        public byte[] arayBrightness = new byte[20];//亮度设置
        public byte[] arayPassword = new byte[48];
        public int RoomCount;
        internal List<RoomInfo> confiinfo;
        public List<byte[]> ImageList = new List<byte[]>();
        //配置信息
        internal class RoomInfo
        {
            public int ID;//ID
            public UInt64 RoomNum;//房间号
            public string Address;//地址
            public string Remark;//备注
        }


        ///<summary>
        ///读取数据库面板设置，将所有数据读至缓存
        ///</summary>
        public void ReadDevieceInfoFromDB(int DIndex)
        {
            
        }

        ///<summary>
        ///保存数据库面板设置，将所有数据保存
        ///</summary>
        public void SaveDevieceInfoToDB(int DIndex)
        {
            
        }

        ///<summary>
        ///读取默认的设置
        ///</summary>
        public void ReadDefaultInfoForDS()
        {
            if (confiinfo == null) confiinfo = new List<RoomInfo>();
            if (MyCardInfo == null) MyCardInfo = new List<CardInfo>();
            if (MyHistory == null) MyHistory = new List<History>();
            
        }

        /// <summary>
        ///上传设置
        /// </summary>
        public bool UploadDSInfoToDevice(string DevName, int intDeviceType, int intActivePage)
        {
            string strMainRemark = DevName.Split('\\')[1].Trim().Split('(')[0].Trim();
            DevName = DevName.Split('\\')[0].Trim();
            //保存basic informations
            byte bytSubID = byte.Parse(DevName.Split('-')[0].ToString());
            byte bytDevID = byte.Parse(DevName.Split('-')[1].ToString());
            byte[] ArayMain = new byte[20];
            byte[] arayTmpRemark = HDLUDP.StringToByte(strMainRemark);
            byte[] arayTmp;
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
                if (CsConst.mySends.AddBufToSndList(ArayMain, 0x0010, bytSubID, bytDevID, false, true, true, false) == true)
                {

                    HDLUDP.TimeBetwnNext(20);
                }
                else return false;
            }
            if (intActivePage == 0 || intActivePage == 1)
            {
                #region
                if (strNO == "") strNO = "0000";
                if (strPassword == "") strPassword = "000000";
                if (strNO.Length < 4) strNO = GlobalClass.AddLeftZero(strNO, 4);
                if (strPassword.Length < 4) strPassword = GlobalClass.AddLeftZero(strPassword, 6);
                arayTmp = HDLUDP.StringToByte(strNO + strPassword);
                if (CsConst.mySends.AddBufToSndList(arayTmp, 0x352F, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(intDeviceType)) == true)
                {
                    HDLUDP.TimeBetwnNext(20);
                }
                else return false;
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(1, null);


                if (CsConst.mySends.AddBufToSndList(arayBasic, 0x3527, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(intDeviceType)) == true)
                {

                    HDLUDP.TimeBetwnNext(20);
                }
                else return false;
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(2, null);

                arayTmp = new byte[2];
                arayTmp[0] = arayInfo[0];
                arayTmp[1] = arayInfo[1];
                if (CsConst.mySends.AddBufToSndList(arayTmp, 0x350E, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(intDeviceType)) == true)
                {

                    HDLUDP.TimeBetwnNext(20);
                }
                else return false;
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(3, null);

                arayTmp = new byte[11];
                Array.Copy(arayCall, 0, arayTmp, 0, 11);
                if (CsConst.mySends.AddBufToSndList(arayTmp, 0x138C, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(intDeviceType)) == true)
                {

                    HDLUDP.TimeBetwnNext(20);
                }
                else return false;
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(4, null);
                arayTmp = new byte[5];
                Array.Copy(arayBrightness,0,arayTmp,0,5);
                if (CsConst.mySends.AddBufToSndList(arayTmp, 0x353D, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(intDeviceType)) == true)
                {
                    HDLUDP.TimeBetwnNext(20);
                }
                else return false;
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(5, null);

                for (int i = 0; i < 4; i++)
                {
                    arayTmp = new byte[12];
                    Array.Copy(arayPassword, i * 12, arayTmp, 0, 12);
                    if (CsConst.mySends.AddBufToSndList(arayTmp, 0x3506, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(intDeviceType)) == true)
                    {
                        HDLUDP.TimeBetwnNext(20);
                    }
                    else return false;
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(5 + i, null);
                }
                #endregion
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(10, null);
            if (intActivePage == 0 || intActivePage == 2 || intActivePage == 5 || intActivePage == 6)
            {
                #region
                arayTmp = new byte[2];
                arayTmp[0] = Convert.ToByte(RoomCount / 256);
                arayTmp[1] = Convert.ToByte(RoomCount % 256);
                if (CsConst.mySends.AddBufToSndList(arayTmp, 0x351E, bytSubID, bytDevID, false, true, true, false) == true)
                {
                    HDLUDP.TimeBetwnNext(20);
                }
                else return false;
                for (int i = 0; i < confiinfo.Count; i++)
                {
                    if (intActivePage == 0 || intActivePage == 2 || intActivePage == 6)
                    {
                        RoomInfo temp = confiinfo[i];
                        arayTmp = new byte[30];
                        arayTmp[0] = Convert.ToByte(temp.ID / 256);
                        arayTmp[1] = Convert.ToByte(temp.ID % 256);
                        string strHex = temp.RoomNum.ToString("X2");
                        strHex = GlobalClass.AddLeftZero(strHex, 8);
                        arayTmp[2] = Convert.ToByte(strHex.Substring(0, 2), 16);
                        arayTmp[3] = Convert.ToByte(strHex.Substring(2, 2), 16);
                        arayTmp[4] = Convert.ToByte(strHex.Substring(4, 2), 16);
                        arayTmp[5] = Convert.ToByte(strHex.Substring(6, 2), 16);
                        arayTmp[6] = 0;
                        arayTmp[7] = 0;
                        arayTmp[8] = 0;
                        arayTmp[9] = 0;
                        arayTmpRemark = HDLUDP.StringToByte(temp.Remark);
                        if (arayTmpRemark.Length > 20)
                        {
                            Array.Copy(arayTmpRemark, 0, arayTmp, 10, 20);
                        }
                        else
                        {
                            Array.Copy(arayTmpRemark, 0, arayTmp, 10, arayTmpRemark.Length);
                        }
                        if (CsConst.mySends.AddBufToSndList(arayTmp, 0x3502, bytSubID, bytDevID, false, true, true, false) == true)
                        {

                            HDLUDP.TimeBetwnNext(20);
                        }
                        else return false;
                    }
                    if (intActivePage == 0 || intActivePage == 5 || intActivePage == 6)
                    {
                        for (int j = 0; j < ImageList.Count; j++)
                        {
                            byte[] arayImageTmp = ImageList[j];
                            if (arayImageTmp[0] == Convert.ToByte(confiinfo[i].ID / 256) &&
                                arayImageTmp[1] == Convert.ToByte(confiinfo[i].ID % 256))
                            {
                                if (arayImageTmp[322] == 1)
                                {
                                    arayTmp = new byte[67];
                                    arayTmp[0] = Convert.ToByte(confiinfo[i].ID / 256);
                                    arayTmp[1] = Convert.ToByte(confiinfo[i].ID % 256);
                                    for (byte k = 0; k < 5; k++)
                                    {
                                        arayTmp[2] = k;
                                        Array.Copy(arayImageTmp, k * 64, arayTmp, 3, 64);
                                        if (CsConst.mySends.AddBufToSndList(arayTmp, 0x3541, bytSubID, bytDevID, false, true, true, false) == true)
                                        {
                                            HDLUDP.TimeBetwnNext(20);
                                        }
                                        else return false;
                                    }
                                    arayImageTmp[322] = 0;
                                }
                                break;
                            }
                        }
                    }
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(10 * i / confiinfo.Count + 10, null);
                }
                #endregion
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(20, null);

            if (intActivePage == 0 || intActivePage == 3)
            {
                #region
                if (CsConst.isRestore)
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

                            arayTmpRemark = HDLUDP.StringToByte(tmp.Remark);
                            if (arayTmpRemark.Length > 18)
                            {
                                Array.Copy(arayTmpRemark, 0, arayTmp, 46, 18);
                            }
                            else
                            {
                                Array.Copy(arayTmpRemark, 0, arayTmp, 46, arayTmpRemark.Length);
                            }

                            if (CsConst.mySends.AddBufToSndList(arayTmp, 0x3518, bytSubID, bytDevID, false, false, true, CsConst.minAllWirelessDeviceType.Contains(intDeviceType)) == true)
                            {

                                HDLUDP.TimeBetwnNext(20);
                            }
                            else return false;
                            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(20 + i, null);
                        }
                    }
                }
                #endregion
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(100);
            return true;
        }

        /// <summary>
        /// 下载所有数据
        /// </summary>
        /// <param name="DevName"></param>
        /// <returns></returns>
        public bool DownloadDSInforsFrmDevice(string DevName, int intDeviceType, int intActivePage,int num1,int num2)
        {
            string strMainRemark = DevName.Split('\\')[1].Trim();
            DevName = DevName.Split('\\')[0].Trim();

            byte bytSubID = byte.Parse(DevName.Split('-')[0].ToString());
            byte bytDevID = byte.Parse(DevName.Split('-')[1].ToString());

            byte[] ArayTmp = new byte[0];
            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x000E, bytSubID, bytDevID, false, true, true, false) == true)
            {
                byte[] arayRemark = new byte[20];
                for (int intI = 0; intI < 20; intI++) { arayRemark[intI] = CsConst.myRevBuf[25 + intI]; }
                Devname = bytSubID.ToString() + "-" + bytDevID.ToString() + "\\" + HDLPF.Byte2String(arayRemark);
                
            }
            else return false;
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(1);
            if (intActivePage == 0 || intActivePage == 1)
            {
                #region
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x352D, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(intDeviceType)) == true)
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
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(2, null);

                arayBasic = new byte[] { 7, 7, 7, 7, 0 };
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x3525, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(intDeviceType)) == true)
                {
                    Array.Copy(CsConst.myRevBuf, 25, arayBasic, 0, 5);
                    if (arayBasic[0] < 1 || arayBasic[0] > 7) arayBasic[0] = 7;
                    if (arayBasic[1] < 1 || arayBasic[1] > 7) arayBasic[1] = 7;
                    if (arayBasic[2] < 1 || arayBasic[2] > 7) arayBasic[2] = 7;
                    if (arayBasic[3] < 1 || arayBasic[3] > 7) arayBasic[3] = 7;

                    HDLUDP.TimeBetwnNext(1);
                }
                else return false;
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(3, null);

                arayInfo = new byte[20];
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x350C, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(intDeviceType)) == true)
                {
                    arayInfo[0] = CsConst.myRevBuf[25];
                    arayInfo[1] = CsConst.myRevBuf[26];
                    HDLUDP.TimeBetwnNext(1);
                }
                else return false;
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(4, null);

                arayCall = new byte[20];
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x138E, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(intDeviceType)) == true)
                {
                    Array.Copy(CsConst.myRevBuf, 25, arayCall, 0, 11);
                    HDLUDP.TimeBetwnNext(1);
                }
                else return false;
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(5, null);

                arayBrightness = new byte[20];
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x353B, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(intDeviceType)) == true)
                {
                    Array.Copy(CsConst.myRevBuf, 25, arayBrightness, 0, 5);
                    HDLUDP.TimeBetwnNext(1);
                }
                else return false;
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(6, null);
                for (byte i = 0; i < 4; i++)
                {
                    ArayTmp = new byte[1];
                    ArayTmp[0] = i;
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x3504, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(intDeviceType)) == true)
                    {
                        Array.Copy(CsConst.myRevBuf, 26, arayPassword, i * 12, 12);
                        HDLUDP.TimeBetwnNext(1);
                    }
                    else return false;
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(6 + i, null);
                }
                #endregion
                MyRead2UpFlags[0] = true;
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(10, null);
            if (intActivePage == 0 || intActivePage == 2 || intActivePage == 5 || intActivePage == 6)
            {
                #region
                if (ImageList == null) ImageList = new List<byte[]>();
                confiinfo = new List<RoomInfo>();
                if (CsConst.mySends.AddBufToSndList(null, 0x351C, bytSubID, bytDevID, false, true, true, false) == true)
                {
                    RoomCount = CsConst.myRevBuf[25] * 256 + CsConst.myRevBuf[26];
                    if (RoomCount > 100) RoomCount = 100;
                }
                else return false;
                if (CsConst.isRestore)
                {
                    num1 = 1;
                    num2 = 100;
                }
                for (int i = num1; i <= num2; i++)
                {
                    ArayTmp = new byte[2];
                    ArayTmp[0] = Convert.ToByte(i / 256);
                    ArayTmp[1] = Convert.ToByte(i % 256);
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x3500, bytSubID, bytDevID, false, true, true, false) == true)
                    {
                        RoomInfo temp = new RoomInfo();
                        temp.ID = i;
                        string strHex = GlobalClass.AddLeftZero(CsConst.myRevBuf[28].ToString("X"), 2) +
                                        GlobalClass.AddLeftZero(CsConst.myRevBuf[29].ToString("X"), 2) +
                                        GlobalClass.AddLeftZero(CsConst.myRevBuf[30].ToString("X"), 2) +
                                        GlobalClass.AddLeftZero(CsConst.myRevBuf[31].ToString("X"), 2);
                        temp.RoomNum = Convert.ToUInt64(strHex, 16);
                        temp.Address = CsConst.myRevBuf[32].ToString() + "." + CsConst.myRevBuf[33].ToString() +
                                       CsConst.myRevBuf[34].ToString() + "." + CsConst.myRevBuf[35].ToString();
                        byte[] arayRemark = new byte[20];
                        Array.Copy(CsConst.myRevBuf, 36, arayRemark, 0, 20);
                        temp.Remark = HDLPF.Byte2String(arayRemark);
                        confiinfo.Add(temp);
                        HDLUDP.TimeBetwnNext(1);
                    }
                    else return false;
                    if (intActivePage == 0 || intActivePage == 5 || intActivePage == 6)
                    {
                        byte[] arayImge = new byte[323];
                        ArayTmp = new byte[3];
                        ArayTmp[0] = Convert.ToByte(i / 256);
                        ArayTmp[1] = Convert.ToByte(i % 256);
                        arayImge[0] = ArayTmp[0];
                        arayImge[1] = ArayTmp[1];
                        for (byte j = 0; j < 5; j++)
                        {
                            ArayTmp[2] = j;
                            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x353F, bytSubID, bytDevID, false, true, true, false) == true)
                            {
                                Array.Copy(CsConst.myRevBuf, 29, arayImge, 64 * j + 2, 64);
                                HDLUDP.TimeBetwnNext(1);
                            }
                            else return false;
                        }
                        for (int j = 0; j < ImageList.Count; j++)
                        {
                            byte[] arayImageTmp = ImageList[j];
                            if (arayImageTmp[0] == Convert.ToByte(i / 256) &&
                                arayImageTmp[1] == Convert.ToByte(i % 256))
                            {
                                ImageList.RemoveAt(j);
                                break;
                            }
                        }
                        ImageList.Add(arayImge);
                    }
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(((i - num1) * 20) / (num2 - num1 + 1) + 10, null);
                }
                #endregion
                MyRead2UpFlags[1] = true;
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(30, null);
            if (intActivePage == 0 || intActivePage == 3)
            {
                #region
                MyCardInfo = new List<CardInfo>();
                for (int i = 0; i < 1000; i++)
                {
                    CardInfo temp = new CardInfo();
                    temp.UID = new byte[10];
                    temp.UIDL = 0;
                    temp.CardType = 0;
                    temp.CardNum = Convert.ToInt32(i + 1);
                    temp.BuildingNO = 0;
                    temp.RoomNO = 0;
                    temp.UnitNO = 0;
                    temp.arayDate = new byte[5];
                    temp.arayPhone = new byte[11];
                    temp.arayName = new byte[10];
                    temp.Remark = "";
                    MyCardInfo.Add(temp);
                }
                string strEnable = "";
                ArayTmp = new byte[4];
                ArayTmp[0] = 0;
                ArayTmp[1] = 0;
                ArayTmp[2] = Convert.ToByte(500 / 256);
                ArayTmp[3] = Convert.ToByte(500 % 256);
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xE474, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(intDeviceType)) == true)
                {
                    int MaxCount = 0;
                    byte[] arayValid = new byte[63];
                    Array.Copy(CsConst.myRevBuf, 29, arayValid, 0, 63);
                    for (int i = 0; i < 63; i++)
                    {
                        string strTmp = GlobalClass.AddLeftZero(Convert.ToString(arayValid[i], 2), 8);
                        for (int j = 7; j >= 0; j--)
                        {
                            if (MaxCount >= 500) break;
                            string str = strTmp.Substring(j, 1);
                            strEnable = strEnable + str;
                            MaxCount = MaxCount + 1;
                        }
                    }
                }
                ArayTmp = new byte[4];
                ArayTmp[0] = Convert.ToByte(500 / 256);
                ArayTmp[1] = Convert.ToByte(500 % 256);
                ArayTmp[2] = Convert.ToByte(500 / 256);
                ArayTmp[3] = Convert.ToByte(500 % 256);
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xE474, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(intDeviceType)) == true)
                {
                    int MaxCount = 0;
                    byte[] arayValid = new byte[63];
                    Array.Copy(CsConst.myRevBuf, 29, arayValid, 0, 63);
                    for (int i = 0; i < 63; i++)
                    {
                        string strTmp = GlobalClass.AddLeftZero(Convert.ToString(arayValid[i], 2), 8);
                        for (int j = 7; j >= 0; j--)
                        {
                            if (MaxCount >= 500) break;
                            string str = strTmp.Substring(j, 1);
                            strEnable = strEnable + str;
                        }
                    }
                }
                System.Diagnostics.Debug.WriteLine(strEnable.Length.ToString());
                for (int i = 0; i < 1000; i++)
                {
                    if (strEnable.Substring(i, 1) == "1")
                    {
                        ArayTmp = new byte[2];
                        ArayTmp[0] = Convert.ToByte((i + 1)/256);
                        ArayTmp[1] = Convert.ToByte((i + 1)%256);
                        if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x3516, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(intDeviceType)) == true)
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

                MyRead2UpFlags[2] = true;
                #endregion
            }
            if (intActivePage == 0 || intActivePage == 4)
            {
                if (!CsConst.isRestore)
                {
                    #region
                    MyHistory = new List<History>();
                    ArayTmp = new byte[2];
                    ArayTmp[0] = 0;
                    ArayTmp[1] = 1;
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x3512, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(intDeviceType)) == true)
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
                                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x3512, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(intDeviceType)) == true)
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

                                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(50 + (i - num1) * 40 / (num2 - num1), null);

                            }
                        }
                    }
                    else return false;
                    #endregion
                }
                MyRead2UpFlags[3] = true;
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(40, null);
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(100, null);
           
            return true;
        }
    }
}
