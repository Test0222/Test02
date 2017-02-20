using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;

namespace HDL_Buspro_Setup_Tool
{
    [Serializable]
    public class MzBox : HdlDeviceBackupAndRestore
    {
        //基本信息
        public int DIndex;//设备唯一编号
        public string Devname;//子网ID 设备ID 备注
        public bool[] MyRead2UpFlags = new bool[20];

        public byte[] arayEnable = new byte[20];
        public byte[] arayPA = new byte[20];
        public byte[] arayOther = new byte[20];
        public string strMastrIP;
        public byte MasterGroupNo;
        public byte AudioINCarrier;
        public byte PDIFCarrier;
        public string PhysicalAddress;
        public FTP myFTP;
        public List<FM> myFM;
        public List<AudioIN> myAudioIN;
        public List<AudioIN> myPDIF;
        public byte[,] arayObjectInEIB;
        public byte[,] arayUniteInEIB;
        public byte[,] arayGroupAddressInEIB;
        public class FTP
        {
            public string IP;
            public string strName;
            public string strPassword;
            public byte Encode;
            public string strPath;
        }

        public class FM
        {
            public byte ID;
            public int Frequence;
            public string strRemark;
            public byte Enable;
        }

        public class AudioIN
        {
            public byte ID;
            public int Length;
            public byte Enable;
            public string strCode;
        }

        ///<summary>
        ///读取默认的设置
        ///</summary>
        public void ReadDefaultInfo()
        {
            myFTP = new FTP();
            myFM = new List<FM>();
            myAudioIN = new List<AudioIN>();
            myPDIF = new List<AudioIN>();
        }

        ///<summary>
        ///读取数据库面板设置，将所有数据读至缓存
        ///</summary>
        public void ReadSensorInfoFromDB(int DIndex)
        {
        }


        public List<string[]> ReadKNXTargets()
        {
            List<string[]> Targets = new List<string[]>();
            try
            {
                string strsql = "select * from delAudioEIBObj order by ID";
                OleDbDataReader dr = DataModule.SearchAResultSQLDB(strsql);
                
                if (dr != null)
                {
                    while (dr.Read())
                    {
                        string[] strAry = new string[5];
                        strAry[0] = dr.GetValue(0).ToString();
                        strAry[1] = dr.GetValue(1).ToString();
                        strAry[2] = dr.GetValue(2).ToString();
                        strAry[3] = dr.GetValue(3).ToString();
                        strAry[4] = dr.GetValue(4).ToString();
                        Targets.Add(strAry);
                    }
                    dr.Close();
                }
            }
            catch
            {
            }
            return Targets;
        }


        ///<summary>
        ///保存数据库面板设置，将所有数据保存
        ///</summary>
        public void SaveSensorInfoToDB()
        {
        }

        /// <summary>
        ///上传设置
        /// </summary>
        public bool UploadInfosToDevice(string DevName, int DeviceType, int intActivePage)// 0 mean all, else that tab only
        {

            string strMainRemark = DevName.Split('\\')[1].Trim().Split('(')[0].Trim();
            String sTmpDevName = DevName.Split('\\')[0].Trim();
            //保存basic informations
            byte bytSubID = byte.Parse(sTmpDevName.Split('-')[0].ToString());
            byte bytDevID = byte.Parse(sTmpDevName.Split('-')[1].ToString());
            byte[] ArayMain = new byte[20];
            if (HDLSysPF.ModifyDeviceMainRemark(bytSubID, bytDevID, strMainRemark, DeviceType) == false) return false;
            byte[] ArayTmp = null;
            if (intActivePage == 0 || intActivePage == 1)
            {
                #region
                for (byte i = 1; i <= 6; i++)
                {
                    if (AudioDeviceTypeList.DinRailAudioDeviceType.Contains(DeviceType) && (i == 4)) continue;
                    ArayTmp = new byte[3];
                    ArayTmp[0] = i;
                    ArayTmp[1] = 0;
                    ArayTmp[2] = arayEnable[i - 1];
                    if (i == 6)
                    {
                        ArayTmp[0] = 0x15;
                    }
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1360, bytSubID, bytDevID, false, true, true, false) == true)
                    {

                        HDLUDP.TimeBetwnNext(20);
                    }
                    else return false;
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(i);
                }

                ArayTmp = new byte[9];
                ArayTmp[0] = 6;
                ArayTmp[1] = 0;
                Array.Copy(arayPA, 0, ArayTmp, 2, 7);
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1360, bytSubID, bytDevID, false, true, true, false) == true)
                {

                    HDLUDP.TimeBetwnNext(20);
                }
                else return false;
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(7);
                if (!AudioDeviceTypeList.DinRailAudioDeviceType.Contains(DeviceType))
                {
                    for (byte i = 1; i <= 3; i++)
                    {
                        ArayTmp = new byte[3];
                        ArayTmp[0] = 6;
                        ArayTmp[1] = i;
                        ArayTmp[2] = arayOther[i - 1];
                        if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1360, bytSubID, bytDevID, false, true, true, false) == true)
                        {

                            HDLUDP.TimeBetwnNext(20);
                        }
                        else return false;
                    }

                    ArayTmp = new byte[7];
                    ArayTmp[0] = 6;
                    ArayTmp[1] = 4;
                    ArayTmp[2] = MasterGroupNo;
                    ArayTmp[3] = Convert.ToByte(strMastrIP.Split('.')[0].ToString());
                    ArayTmp[4] = Convert.ToByte(strMastrIP.Split('.')[1].ToString());
                    ArayTmp[5] = Convert.ToByte(strMastrIP.Split('.')[2].ToString());
                    ArayTmp[6] = Convert.ToByte(strMastrIP.Split('.')[3].ToString());
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1360, bytSubID, bytDevID, false, true, true, false) == true)
                    {

                        HDLUDP.TimeBetwnNext(20);
                    }
                    else return false;
                }

                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(8);
                #endregion

                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(10);
            }
            else if (intActivePage == 0 || intActivePage == 2)
            {
                #region
                if (myFTP != null)
                {
                    ArayTmp = new byte[31];
                    ArayTmp[0] = 2;
                    ArayTmp[1] = 1;
                    ArayTmp[2] = Convert.ToByte(myFTP.IP.Split('.')[0].ToString());
                    ArayTmp[3] = Convert.ToByte(myFTP.IP.Split('.')[1].ToString());
                    ArayTmp[4] = Convert.ToByte(myFTP.IP.Split('.')[2].ToString());
                    ArayTmp[5] = Convert.ToByte(myFTP.IP.Split('.')[3].ToString());
                    ArayTmp[6] = myFTP.Encode;
                    byte[] arayUser = HDLUDP.StringToByte(myFTP.strName);
                    if (arayUser.Length > 16)
                        Array.Copy(arayUser, 0, ArayTmp, 7, 16);
                    else
                        Array.Copy(arayUser, 0, ArayTmp, 7, arayUser.Length);
                    byte[] arayPassWord = HDLUDP.StringToByte(myFTP.strPassword);
                    if (arayPassWord.Length > 8)
                        Array.Copy(arayPassWord, 0, ArayTmp, 23, 8);
                    else
                        Array.Copy(arayPassWord, 0, ArayTmp, 23, arayPassWord.Length);
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1360, bytSubID, bytDevID, false, true, true, false) == true)
                    {

                        HDLUDP.TimeBetwnNext(20);
                    }
                    else return false;
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(11);

                    ArayTmp = new byte[42];
                    ArayTmp[0] = 2;
                    ArayTmp[1] = 2;
                    myFTP.strPath = myFTP.strPath.Replace('/', '\\');
                    byte[] arayPath = HDLUDP.StringToByte(myFTP.strPath);
                    if (arayPath.Length > 40)
                        Array.Copy(arayPath, 0, ArayTmp, 2, 40);
                    else
                        Array.Copy(arayPath, 0, ArayTmp, 2, arayPath.Length);
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1360, bytSubID, bytDevID, false, true, true, false) == true)
                    {

                        HDLUDP.TimeBetwnNext(20);
                    }
                    else return false;
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(12);
                }
                #endregion

                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(20);
            }
            else if (intActivePage == 0 || intActivePage == 3)
            {
                #region
                if (myFM != null)
                {
                    for (int i = 0; i < myFM.Count; i++)
                    {
                        ArayTmp = new byte[46];
                        ArayTmp[0] = 3;
                        ArayTmp[1] = 1;
                        ArayTmp[2] = myFM[i].ID;
                        ArayTmp[3] = myFM[i].Enable;
                        ArayTmp[4] = Convert.ToByte(myFM[i].Frequence / 256);
                        ArayTmp[5] = Convert.ToByte(myFM[i].Frequence % 256);
                        byte[] arayRemark = HDLUDP.StringTo2Byte(myFM[i].strRemark, true);
                        if (arayRemark.Length > 40)
                            Array.Copy(arayRemark, 0, ArayTmp, 6, 40);
                        else
                            Array.Copy(arayRemark, 0, ArayTmp, 6, arayRemark.Length);
                        if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1360, bytSubID, bytDevID, false, true, true, false) == true)
                        {

                            HDLUDP.TimeBetwnNext(20);
                        }
                        else return false;
                        if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(20 + (10 * i) / myFM.Count);
                    }
                }
                #endregion

                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(30);
            }
            else if (intActivePage == 0 || intActivePage == 4)
            {
                #region
                ArayTmp = new byte[3];
                ArayTmp[0] = 5;
                ArayTmp[1] = 1;
                ArayTmp[2] = AudioINCarrier;
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1360, bytSubID, bytDevID, false, true, true, false) == true)
                {

                    HDLUDP.TimeBetwnNext(20);
                }
                else return false;
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(31);
                if (myAudioIN != null)
                {
                    for (int i = 0; i < myAudioIN.Count; i++)
                    {
                        if (myAudioIN[i].Enable == 1 && myAudioIN[i].strCode != null && myAudioIN[i].strCode != "")
                        {
                            String strCode = myAudioIN[i].strCode;
                            String[] arayCode = strCode.Split(' ');
                            ArayTmp = new Byte[arayCode.Length + 4];
                            ArayTmp[0] = 129;
                            ArayTmp[1] = 0;
                            ArayTmp[2] = myAudioIN[i].ID;
                            ArayTmp[3] = 1;
                            for (int j = 0; j < arayCode.Length; j++)
                            {
                                if (arayCode[j] == "" || arayCode[j] == null) continue;
                                ArayTmp[4 + j] = Convert.ToByte(arayCode[j], 16);
                            }
                            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1364, bytSubID, bytDevID, true, true, true, false) == true)
                            {

                                HDLUDP.TimeBetwnNext(20);
                            }
                            else return false;
                            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(32 + i);
                        }
                    }
                }
                #endregion

                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(40);
            }
            else if (intActivePage == 0 || intActivePage == 5)
            {
                #region
                    ArayTmp = new byte[3];
                    ArayTmp[0] = 0x15;
                    ArayTmp[1] = 1;
                    ArayTmp[2] = PDIFCarrier;
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1360, bytSubID, bytDevID, false, true, true, false) == true)
                    {
                        HDLUDP.TimeBetwnNext(20);
                    }
                    else return false;
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(41);
                    if (myPDIF != null)
                    {
                        for (int i = 0; i < myPDIF.Count; i++)
                        {
                            if (myPDIF[i].Enable == 1 && myPDIF[i].strCode != null && myPDIF[i].strCode != "")
                            {
                                string strCode = myPDIF[i].strCode;
                                string[] arayCode = strCode.Split(' ');
                                ArayTmp = new byte[4 + arayCode.Length];
                                ArayTmp[0] = 129;
                                ArayTmp[1] = 1;
                                ArayTmp[2] = myPDIF[i].ID;
                                ArayTmp[3] = 1;
                                for (int j = 0; j < arayCode.Length; j++)
                                {
                                    ArayTmp[4 + j] = Convert.ToByte(arayCode[j], 16);
                                }
                                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1364, bytSubID, bytDevID, true, true, true, false) == true)
                                {
                                    HDLUDP.TimeBetwnNext(20);
                                }
                                else return false;
                                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(42 + i);
                            }
                        }
                    }
                #endregion

                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(50);
            }
            else if (intActivePage == 0 || intActivePage == 6)
            {
                #region
                if (DeviceType != 906)
                {
                    int LenGroupAddress = (arayGroupAddressInEIB.Length / 2);
                    int LenUnite = (arayUniteInEIB.Length / 4);
                    int LenObject = (arayObjectInEIB.Length / 2);
                    string str1 = GlobalClass.AddLeftZero(Convert.ToString(Convert.ToInt32(PhysicalAddress.Split('.')[0].ToString()), 2), 4);
                    string str2 = GlobalClass.AddLeftZero(Convert.ToString(Convert.ToInt32(PhysicalAddress.Split('.')[1].ToString()), 2), 4);
                    string strTmp = str1 + str2;
                    byte bytPhysicalAddress1 = Convert.ToByte(strTmp, 2);
                    byte bytPhysicalAddress2 = Convert.ToByte(PhysicalAddress.Split('.')[2].ToString());
                    int TotalLen = LenGroupAddress * 2 + 4;
                    int Times = TotalLen / 10;
                    int LastTimeNum = 0;
                    if (TotalLen % 10 != 0)
                    {
                        Times = Times + 1;
                        LastTimeNum = Times % 10;
                    }
                    for (int i = 0; i < Times; i++)
                    {
                        if (i == 0)
                        {
                            ArayTmp = new byte[13];
                            ArayTmp[0] = 0;
                            ArayTmp[1] = 0;
                            ArayTmp[2] = 10;
                            ArayTmp[3] = Convert.ToByte(TotalLen / 256);
                            ArayTmp[4] = Convert.ToByte(TotalLen % 256);
                            ArayTmp[5] = bytPhysicalAddress1;
                            ArayTmp[6] = bytPhysicalAddress2;
                            for (int j = 0; j < 3; j++)
                            {
                                ArayTmp[7 + 2 * j] = arayGroupAddressInEIB[j, 0];
                                ArayTmp[8 + 2 * j] = arayGroupAddressInEIB[j, 1];
                            }
                        }
                        else
                        {
                            if (LastTimeNum != 0 && i == (Times - 1))
                            {
                                int LenTmp = LastTimeNum / 2;
                                if (LenTmp == 1)
                                {
                                    ArayTmp = new byte[5];
                                    ArayTmp[0] = Convert.ToByte((i * 10) / 256);
                                    ArayTmp[1] = Convert.ToByte((i * 10) % 256);
                                    ArayTmp[2] = 2;
                                    ArayTmp[3] = arayGroupAddressInEIB[LenGroupAddress - 1, 0];
                                    ArayTmp[4] = arayGroupAddressInEIB[LenGroupAddress - 1, 1];
                                }
                                else
                                {
                                    ArayTmp = new byte[LenTmp * 2 + 3];
                                    ArayTmp[0] = Convert.ToByte((i * 10) / 256);
                                    ArayTmp[1] = Convert.ToByte((i * 10) % 256);
                                    ArayTmp[2] = Convert.ToByte(LenTmp * 2);
                                    for (int j = 0; j < LenTmp; j++)
                                    {
                                        ArayTmp[3 + 2 * j] = arayGroupAddressInEIB[LenGroupAddress - 1 - (LenTmp - 1 - j), 0];
                                        ArayTmp[4 + 2 * j] = arayGroupAddressInEIB[LenGroupAddress - 1 - (LenTmp - 1 - j), 1];
                                    }
                                }
                            }
                            else
                            {
                                ArayTmp = new byte[13];
                                ArayTmp[0] = Convert.ToByte((i * 10) / 256);
                                ArayTmp[1] = Convert.ToByte((i * 10) % 256);
                                ArayTmp[2] = 10;
                                for (int j = 0; j < 5; j++)
                                {
                                    ArayTmp[3 + 2 * j] = arayGroupAddressInEIB[3 + 5 * (i - 1) + j, 0];
                                    ArayTmp[4 + 2 * j] = arayGroupAddressInEIB[3 + 5 * (i - 1) + j, 1];
                                }
                            }
                        }
                        if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x4000, bytSubID, bytDevID, false, true, true, false) == true)
                        {
                            
                            HDLUDP.TimeBetwnNext(20);
                        }
                        else return false;
                    }
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(53);

                    int AddressIndex = TotalLen;
                    TotalLen = LenUnite * 4 + 2;
                    LastTimeNum = 0;
                    Times = (TotalLen - 10) / 10;
                    Times = Times + 1;
                    if ((TotalLen - 10) % 8 != 0)
                    {
                        Times = Times + 1;
                        LastTimeNum = TotalLen % 10;
                    }
                    for (int i = 0; i < Times; i++)
                    {
                        if (i == 0)
                        {
                            ArayTmp = new byte[13];
                            ArayTmp[0] = Convert.ToByte(AddressIndex / 256);
                            ArayTmp[1] = Convert.ToByte(AddressIndex % 256);
                            ArayTmp[2] = 10;
                            ArayTmp[3] = Convert.ToByte(TotalLen / 256);
                            ArayTmp[4] = Convert.ToByte(TotalLen % 256);
                            for (int j = 0; j < 2; j++)
                            {
                                ArayTmp[5 + 4 * j] = arayUniteInEIB[j, 0];
                                ArayTmp[6 + 4 * j] = arayUniteInEIB[j, 1];
                                ArayTmp[7 + 4 * j] = arayUniteInEIB[j, 2];
                                ArayTmp[8 + 4 * j] = arayUniteInEIB[j, 3];
                            }
                        }
                        else
                        {
                            if (LastTimeNum != 0 && i == (Times - 1))
                            {
                                int LenTmp = LastTimeNum / 4;
                                if (LenTmp == 1)
                                {
                                    ArayTmp = new byte[7];
                                    ArayTmp[0] = Convert.ToByte((AddressIndex + i * 8) / 256);
                                    ArayTmp[1] = Convert.ToByte((AddressIndex + i * 8) % 256);
                                    ArayTmp[2] = 4;
                                    ArayTmp[3] = arayUniteInEIB[(LenUnite - 1), 0];
                                    ArayTmp[4] = arayUniteInEIB[(LenUnite - 1), 1];
                                    ArayTmp[5] = arayUniteInEIB[(LenUnite - 1), 2];
                                    ArayTmp[6] = arayUniteInEIB[(LenUnite - 1), 3];
                                }
                                else
                                {
                                    ArayTmp = new byte[LenTmp * 4 + 3];
                                    ArayTmp[0] = Convert.ToByte((AddressIndex + i * 8) / 256);
                                    ArayTmp[1] = Convert.ToByte((AddressIndex + i * 8) % 256);
                                    ArayTmp[2] = Convert.ToByte(LenTmp * 4);
                                    for (int j = 0; j < LenTmp; j++)
                                    {
                                        ArayTmp[3 + 4 * j] = arayUniteInEIB[(LenUnite - 1) - (LenTmp - 1 - j), 0];
                                        ArayTmp[4 + 4 * j] = arayUniteInEIB[(LenUnite - 1) - (LenTmp - 1 - j), 1];
                                        ArayTmp[5 + 4 * j] = arayUniteInEIB[(LenUnite - 1) - (LenTmp - 1 - j), 2];
                                        ArayTmp[6 + 4 * j] = arayUniteInEIB[(LenUnite - 1) - (LenTmp - 1 - j), 3];
                                    }
                                }
                            }
                            else
                            {
                                ArayTmp = new byte[11];
                                ArayTmp[0] = Convert.ToByte((AddressIndex + i * 8 + 2) / 256);
                                ArayTmp[1] = Convert.ToByte((AddressIndex + i * 8 + 2) % 256);
                                ArayTmp[2] = 8;
                                for (int j = 0; j < 2; j++)
                                {
                                    ArayTmp[3 + 4 * j] = arayUniteInEIB[2 + 2 * (i - 1) + j, 0];
                                    ArayTmp[4 + 4 * j] = arayUniteInEIB[2 + 2 * (i - 1) + j, 1];
                                    ArayTmp[5 + 4 * j] = arayUniteInEIB[2 + 2 * (i - 1) + j, 2];
                                    ArayTmp[6 + 4 * j] = arayUniteInEIB[2 + 2 * (i - 1) + j, 3];
                                }
                            }
                        }
                        if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x4000, bytSubID, bytDevID, false, true, true, false) == true)
                        {
                            
                            HDLUDP.TimeBetwnNext(20);
                        }
                        else return false;
                    }
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(56);
                    AddressIndex = AddressIndex + TotalLen;
                    TotalLen = LenObject * 2 + 2;
                    LastTimeNum = 0;
                    Times = TotalLen / 10;
                    if (TotalLen % 10 != 0)
                    {
                        Times = Times + 1;
                        LastTimeNum = TotalLen % 10;
                    }
                    for (int i = 0; i < Times; i++)
                    {
                        if (i == 0)
                        {
                            ArayTmp = new byte[13];
                            ArayTmp[0] = Convert.ToByte(AddressIndex / 256);
                            ArayTmp[1] = Convert.ToByte(AddressIndex % 256);
                            ArayTmp[2] = 10;
                            ArayTmp[3] = Convert.ToByte(TotalLen / 256);
                            ArayTmp[4] = Convert.ToByte(TotalLen % 256);
                            for (int j = 0; j < 4; j++)
                            {
                                ArayTmp[5 + 2 * j] = arayObjectInEIB[j, 0];
                                ArayTmp[6 + 2 * j] = arayObjectInEIB[j, 1];
                            }
                        }
                        else
                        {
                            if (LastTimeNum != 0 && i == (Times - 1))
                            {
                                int LenTmp = LastTimeNum / 2;
                                if (LenTmp == 1)
                                {
                                    ArayTmp = new byte[5];
                                    ArayTmp[0] = Convert.ToByte((AddressIndex + i * 10) / 256);
                                    ArayTmp[1] = Convert.ToByte((AddressIndex + i * 10) % 256);
                                    ArayTmp[2] = 2;
                                    ArayTmp[3] = arayObjectInEIB[LenObject - 1, 0];
                                    ArayTmp[4] = arayObjectInEIB[LenObject - 1, 1];
                                }
                                else
                                {
                                    ArayTmp = new byte[LenTmp * 2 + 3];
                                    ArayTmp[0] = Convert.ToByte((AddressIndex + i * 10) / 256);
                                    ArayTmp[1] = Convert.ToByte((AddressIndex + i * 10) % 256);
                                    ArayTmp[2] = Convert.ToByte(LenTmp * 2);
                                    for (int j = 0; j < LenTmp; j++)
                                    {
                                        ArayTmp[3 + 2 * j] = arayObjectInEIB[(LenObject - 1) - (LenTmp - 1 - j), 0];
                                        ArayTmp[4 + 2 * j] = arayObjectInEIB[(LenObject - 1) - (LenTmp - 1 - j), 1];
                                    }
                                }
                            }
                            else
                            {
                                ArayTmp = new byte[13];
                                ArayTmp[0] = Convert.ToByte((AddressIndex + i * 10) / 256);
                                ArayTmp[1] = Convert.ToByte((AddressIndex + i * 10) % 256);
                                ArayTmp[2] = 10;
                                for (int j = 0; j < 5; j++)
                                {
                                    ArayTmp[3 + 2 * j] = arayObjectInEIB[3 + 5 * (i - 1) + j, 0];
                                    ArayTmp[4 + 2 * j] = arayObjectInEIB[3 + 5 * (i - 1) + j, 1];
                                }
                            }
                        }
                        if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x4000, bytSubID, bytDevID, false, true, true, false) == true)
                        {
                            
                            HDLUDP.TimeBetwnNext(20);
                        }
                        else return false;
                    }
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(59);
                }
                #endregion
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(60);
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(100);
            return true;
        }

        /// <summary>
        ///下载设置
        /// </summary>
        public bool DownloadInfosToDevice(string DevName, int wdDeviceType, int intActivePage) // 0 mean all, else that tab only
        {
            string strMainRemark = DevName.Split('\\')[1].Trim();
            String sTmpDevName = DevName.Split('\\')[0].Trim();

            //保存basic informations网络信息
            byte bytSubID = byte.Parse(sTmpDevName.Split('-')[0].ToString());
            byte bytDevID = byte.Parse(sTmpDevName.Split('-')[1].ToString());
            byte[] ArayTmp = null;
            String DeviceRemark = HDLSysPF.ReadDeviceMainRemark(bytSubID, bytDevID);

            Devname = bytSubID.ToString() + "-" + bytDevID.ToString() + "\\" + DeviceRemark;

            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(1);
            if (intActivePage == 0 || intActivePage == 1)
            {
                #region
                arayEnable = new byte[20];
                arayPA = new byte[20];
                arayOther = new byte[20];
                for (byte i = 1; i <= 5; i++)
                {
                    if (AudioDeviceTypeList.DinRailAudioDeviceType.Contains(wdDeviceType) && i == 4) continue;
                    ArayTmp = new byte[2];
                    ArayTmp[0] = i;
                    ArayTmp[1] = 0;
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1362, bytSubID, bytDevID, false, true, true, false) == true)
                    {
                        arayEnable[i - 1] = CsConst.myRevBuf[27];

                        HDLUDP.TimeBetwnNext(1);
                    }
                    else return false;
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(1 + i);
                }
                if (!AudioDeviceTypeList.DinRailAudioDeviceType.Contains(wdDeviceType))
                {
                    ArayTmp = new byte[2];
                    ArayTmp[0] = 0x15;
                    ArayTmp[1] = 0;
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1362, bytSubID, bytDevID, false, true, true, false) == true)
                    {
                        arayEnable[5] = CsConst.myRevBuf[27];

                        HDLUDP.TimeBetwnNext(1);
                    }
                    else return false;
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(8);
                }


                ArayTmp = new byte[2];
                ArayTmp[0] = 6;
                ArayTmp[1] = 0;
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1362, bytSubID, bytDevID, false, true, true, false) == true)
                {
                    Array.Copy(CsConst.myRevBuf, 27, arayPA, 0, 7);

                    HDLUDP.TimeBetwnNext(1);
                }
                else return false;
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(9);
                if (!AudioDeviceTypeList.DinRailAudioDeviceType.Contains(wdDeviceType))
                {
                    for (byte i = 1; i <= 3; i++)
                    {
                        ArayTmp = new byte[2];
                        ArayTmp[0] = 6;
                        ArayTmp[1] = i;
                        if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1362, bytSubID, bytDevID, false, true, true, false) == true)
                        {
                            arayOther[i - 1] = CsConst.myRevBuf[27];

                            HDLUDP.TimeBetwnNext(1);
                        }
                        else return false;
                    }

                    if (AudioDeviceTypeList.AudioBoxHasPartyGroup.Contains(wdDeviceType))
                    {
                        ArayTmp = new byte[2];
                        ArayTmp[0] = 6;
                        ArayTmp[1] = 4;
                        if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1362, bytSubID, bytDevID, false, true, true, false) == true)
                        {
                            MasterGroupNo = CsConst.myRevBuf[27];
                            strMastrIP = CsConst.myRevBuf[28].ToString() + "." + CsConst.myRevBuf[29].ToString() + "." +
                                         CsConst.myRevBuf[30].ToString() + "." + CsConst.myRevBuf[31].ToString();

                            HDLUDP.TimeBetwnNext(1);
                        }
                        else return false;
                    }
                }
                #endregion
                MyRead2UpFlags[0] = true;

                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(10);
            }
            else if (intActivePage == 0 || intActivePage == 2)
            {
                #region
                myFTP = new FTP();
                ArayTmp = new byte[2];
                ArayTmp[0] = 2;
                ArayTmp[1] = 1;
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1362, bytSubID, bytDevID, false, true, true, false) == true)
                {
                    myFTP.IP = CsConst.myRevBuf[27].ToString() + "." + CsConst.myRevBuf[28].ToString() + "." +
                               CsConst.myRevBuf[29].ToString() + "." + CsConst.myRevBuf[30].ToString();
                    byte[] arayUser = new byte[16];
                    byte[] arayPassword = new byte[8];
                    myFTP.Encode = CsConst.myRevBuf[31];
                    Array.Copy(CsConst.myRevBuf, 32, arayUser, 0, arayUser.Length);
                    Array.Copy(CsConst.myRevBuf, 48, arayPassword, 0, arayPassword.Length);
                    myFTP.strName = HDLPF.Byte2String(arayUser);
                    myFTP.strPassword = HDLPF.Byte2String(arayPassword);

                    HDLUDP.TimeBetwnNext(1);
                }
                else return false;
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(11);
                ArayTmp = new byte[2];
                ArayTmp[0] = 2;
                ArayTmp[1] = 2;
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1362, bytSubID, bytDevID, false, true, true, false) == true)
                {
                    byte[] arayPath = new byte[40];
                    Array.Copy(CsConst.myRevBuf, 27, arayPath, 0, arayPath.Length);
                    myFTP.strPath = HDLPF.Byte2String(arayPath);

                    HDLUDP.TimeBetwnNext(1);
                }
                else return false;
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(12);
                #endregion
                MyRead2UpFlags[1] = true;

                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(20);
            }
            else if (intActivePage == 0 || intActivePage == 3)
            {
                #region
                myFM = new List<FM>();
                for (byte i = 0; i < 50; i++)
                {
                    ArayTmp = new byte[3];
                    ArayTmp[0] = 3;
                    ArayTmp[1] = 1;
                    ArayTmp[2] = i;
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1362, bytSubID, bytDevID, false, true, true, false) == true)
                    {
                        FM temp = new FM();
                        temp.ID = i;
                        temp.Enable = CsConst.myRevBuf[28];
                        temp.Frequence = CsConst.myRevBuf[29] * 256 + CsConst.myRevBuf[30];
                        byte[] arayRemark = new byte[40];
                        Array.Copy(CsConst.myRevBuf, 31, arayRemark, 0, 40);
                        temp.strRemark = HDLPF.Byte22String(arayRemark, true);
                        myFM.Add(temp);

                        HDLUDP.TimeBetwnNext(1);
                        if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress((i / 50) * 10 + 20);
                    }
                    else return false;
                }
                #endregion
                MyRead2UpFlags[2] = true;

                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(30);
            }
            else if (intActivePage == 0 || intActivePage == 4)
            {
                #region
                ArayTmp = new byte[2];
                ArayTmp[0] = 5;
                ArayTmp[1] = 1;
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1362, bytSubID, bytDevID, false, true, true, false) == true)
                {
                    AudioINCarrier = CsConst.myRevBuf[27];

                    HDLUDP.TimeBetwnNext(1);
                }
                else return false;
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(31);
                myAudioIN = new List<AudioIN>();
                int num = 7;
                if (wdDeviceType == 906) num = 6;
                for (byte i = 0; i < num; i++)
                {
                    ArayTmp = new byte[3] { 0x82, 0, i };
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1364, bytSubID, bytDevID, true, true, true, false) == true)
                    {
                        AudioIN temp = new AudioIN();
                        temp.ID = i;
                        temp.Enable = CsConst.myRevBuf[30];
                        if (temp.Enable == 1)
                        {
                            temp.Length = CsConst.myRevBuf[31] * 256 + CsConst.myRevBuf[32];
                            temp.strCode = "";
                            for (int j = 0; j < temp.Length; j++)
                            {
                                temp.strCode = temp.strCode + GlobalClass.AddLeftZero(CsConst.myRevBuf[33 + j].ToString("X"), 2) + " ";
                            }
                            temp.strCode = temp.strCode.Trim();
                        }
                        else
                        {
                            temp.strCode = "";
                        }
                        myAudioIN.Add(temp);

                        HDLUDP.TimeBetwnNext(1);
                    }
                    else return false;
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(31 + i);
                }
                #endregion
                MyRead2UpFlags[3] = true;

                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(40);
            }
            else if (intActivePage == 0 || intActivePage == 5)
            {
                #region
                ArayTmp = new byte[2];
                ArayTmp[0] = 0x15;
                ArayTmp[1] = 1;
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1362, bytSubID, bytDevID, false, true, true, false) == true)
                {
                    PDIFCarrier = CsConst.myRevBuf[27];

                    HDLUDP.TimeBetwnNext(1);
                }
                else return false;
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(41);
                myPDIF = new List<AudioIN>();
                int num = 7;
                if (wdDeviceType == 906) num = 6;
                for (byte i = 0; i < num; i++)
                {
                    ArayTmp = new byte[3] { 0x82, 1, i };
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1364, bytSubID, bytDevID, true, true, true, false) == true)
                    {
                        AudioIN temp = new AudioIN();
                        temp.ID = i;
                        temp.Enable = CsConst.myRevBuf[30];
                        if (temp.Enable == 1)
                        {
                            temp.Length = CsConst.myRevBuf[31] * 256 + CsConst.myRevBuf[32];
                            temp.strCode = "";
                            for (int j = 0; j < temp.Length; j++)
                            {
                                temp.strCode = temp.strCode + GlobalClass.AddLeftZero(CsConst.myRevBuf[33 + j].ToString("X"), 2) + " ";
                            }
                            temp.strCode = temp.strCode.Trim();
                        }
                        else
                        {
                            temp.strCode = "";
                        }
                        myPDIF.Add(temp);

                        HDLUDP.TimeBetwnNext(1);
                    }
                    else return false;
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(41 + i);
                }
                #endregion
                MyRead2UpFlags[4] = true;

                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(50);
            }
            else if (intActivePage == 0 || intActivePage == 6)
            {
                #region
                if (wdDeviceType != 906)
                {
                    int Length1 = 0;
                    int Length2 = 0;
                    int Length3 = 0;
                    int LenObject = 0;
                    PhysicalAddress = "";
                    ArayTmp = new byte[3];
                    ArayTmp[0] = 0;
                    ArayTmp[1] = 0;
                    ArayTmp[2] = 10;
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x4002, bytSubID, bytDevID, false, true, true, false) == true)
                    {
                        string str = GlobalClass.AddLeftZero(Convert.ToString(CsConst.myRevBuf[30], 2), 8);
                        string str1 = str.Substring(0, 4);
                        string str2 = str.Substring(4, 4);
                        int int1 = Convert.ToInt32(str1, 2);
                        int int2 = Convert.ToInt32(str2, 2);
                        PhysicalAddress = int1.ToString() + "." + int2.ToString() + "." + CsConst.myRevBuf[31].ToString();
                        
                        HDLUDP.TimeBetwnNext(1);
                    }
                    else return false;
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(51);

                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x4002, bytSubID, bytDevID, false, true, true, false) == true)
                    {
                        Length1 = CsConst.myRevBuf[28] * 256 + CsConst.myRevBuf[29];
                        
                        HDLUDP.TimeBetwnNext(1);
                    }
                    else return false;

                    ArayTmp = new byte[3];
                    ArayTmp[0] = Convert.ToByte((Length1 + 2) / 256);
                    ArayTmp[1] = Convert.ToByte((Length1 + 2) % 256);
                    ArayTmp[2] = 10;
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x4002, bytSubID, bytDevID, false, true, true, false) == true)
                    {
                        Length2 = CsConst.myRevBuf[28] * 256 + CsConst.myRevBuf[29];
                        
                        HDLUDP.TimeBetwnNext(1);
                    }
                    else return false;

                    Length3 = Length1 + Length2 + 4;
                    ArayTmp = new byte[3];
                    ArayTmp[0] = Convert.ToByte(Length3 / 256);
                    ArayTmp[1] = Convert.ToByte(Length3 % 256);
                    ArayTmp[2] = 10;
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x4002, bytSubID, bytDevID, false, true, true, false) == true)
                    {
                        LenObject = CsConst.myRevBuf[28] * 256 + CsConst.myRevBuf[29];
                        LenObject = LenObject / 2;
                        
                        HDLUDP.TimeBetwnNext(1);
                    }
                    else return false;
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(52);
                    arayObjectInEIB = new byte[LenObject, 2];
                    for (int i = 0; i < LenObject; i++)
                    {
                        arayObjectInEIB[i, 0] = Convert.ToByte(i / 256);
                        arayObjectInEIB[i, 1] = Convert.ToByte(i % 256);
                    }
                    if (Length1 == 0) return true;
                    int intLenGroupAddress = (Length1 / 2) - 1;
                    arayGroupAddressInEIB = new byte[intLenGroupAddress, 2];
                    int intTimes = Length1 / 10;
                    if ((Length1 % 10) != 0) intTimes = intTimes + 1;
                    for (int i = 0; i < intTimes; i++)
                    {
                        ArayTmp = new byte[3];
                        ArayTmp[0] = Convert.ToByte((i * 10) / 256);
                        ArayTmp[1] = Convert.ToByte((i * 10) % 256);
                        ArayTmp[2] = 10;
                        if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x4002, bytSubID, bytDevID, false, true, true, false) == true)
                        {
                            if (i == 0)
                            {
                                for (int j = 0; j < 3; j++)
                                {
                                    arayGroupAddressInEIB[j, 0] = CsConst.myRevBuf[32 + (2 * j)];
                                    arayGroupAddressInEIB[j, 1] = CsConst.myRevBuf[33 + (2 * j)];
                                }
                            }
                            else
                            {
                                for (int j = 0; j < 5; j++)
                                {
                                    if (j + i * 5 - 2 >= intLenGroupAddress) break;
                                    arayGroupAddressInEIB[j + i * 5 - 2, 0] = CsConst.myRevBuf[28 + (2 * j)];
                                    arayGroupAddressInEIB[j + i * 5 - 2, 1] = CsConst.myRevBuf[29 + (2 * j)];
                                }
                            }
                            
                            HDLUDP.TimeBetwnNext(1);
                        }
                        else return false;
                    }
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(53);

                    int intLenUnite = Length2 / 4;
                    arayUniteInEIB = new byte[intLenUnite, 4];
                    intTimes = Length2 / 8;
                    if ((intTimes % 8) != 0) intTimes = intTimes + 1;
                    for (int i = 0; i < intTimes; i++)
                    {
                        ArayTmp = new byte[3];
                        if (i == 0)
                        {
                            ArayTmp[0] = Convert.ToByte((Length1 + 2) / 256);
                            ArayTmp[1] = Convert.ToByte((Length1 + 2) % 256);
                            ArayTmp[2] = 10;
                        }
                        else
                        {
                            ArayTmp[0] = Convert.ToByte((Length1 + (i * 8) + 4) / 256);
                            ArayTmp[1] = Convert.ToByte((Length1 + (i * 8) + 4) % 256);
                            ArayTmp[2] = 8;
                        }
                        if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x4002, bytSubID, bytDevID, false, true, true, false) == true)
                        {
                            if (i == 0)
                            {
                                for (int j = 0; j < 2; j++)
                                {
                                    arayUniteInEIB[j, 0] = CsConst.myRevBuf[30 + (4 * j)];
                                    arayUniteInEIB[j, 1] = CsConst.myRevBuf[31 + (4 * j)];
                                    arayUniteInEIB[j, 2] = CsConst.myRevBuf[32 + (4 * j)];
                                    arayUniteInEIB[j, 3] = CsConst.myRevBuf[33 + (4 * j)];
                                }
                            }
                            else
                            {
                                for (int j = 0; j < 2; j++)
                                {
                                    if ((j + i * 2) > ((arayUniteInEIB.Length / 4) - 1)) break;
                                    arayUniteInEIB[j + i * 2, 0] = CsConst.myRevBuf[28 + (4 * j)];
                                    arayUniteInEIB[j + i * 2, 1] = CsConst.myRevBuf[29 + (4 * j)];
                                    arayUniteInEIB[j + i * 2, 2] = CsConst.myRevBuf[30 + (4 * j)];
                                    arayUniteInEIB[j + i * 2, 3] = CsConst.myRevBuf[31 + (4 * j)];
                                }
                            }
                            
                            HDLUDP.TimeBetwnNext(1);
                        }
                        else return false;
                    }
                }
                #endregion
                MyRead2UpFlags[5] = true;
            }

            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(60);
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(100);
            return true;
        }
    }
}
