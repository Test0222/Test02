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
    public class SendIR:ICloneable
    {
        public object Clone()
        {
            return this.MemberwiseClone();
        }
        public int DIndex;  ////设备唯一编号
        public string strName;
        public byte RemoteEnable;
        public byte mbytOne;
        public byte mbytTwo;
        public List<UVCMD.IRCode> IRCodes = null;  //// 红外码库
        public bool[] MyRead2UpFlags = new bool[6]; //前面三个表示读取 后面表示上传

        //<summary>
        //读取默认的动静传感器设置，将所有数据读取缓存
        //</summary>
        public void ReadDefaultInfo()
        {
            IRCodes = new List<UVCMD.IRCode>();
        }

        //<summary>
        //读取数据库面板设置，将所有数据读至缓存
        //</summary>
        public void ReadIRcodesFrmDBTobuf(int DIndex)
        {
            try
            {
                #region
                string strsql = string.Format("select * from dbClassInfomation where DIndex={0} and ClassID={1} order by SenNum", DIndex, 0);
                OleDbDataReader dr = DataModule.SearchAResultSQLDB(strsql, CsConst.mstrCurPath);

                if (dr != null)
                {
                    while (dr.Read())
                    {
                        string str = dr.GetValue(5).ToString();
                        RemoteEnable = Convert.ToByte(str.Split('-')[0].ToString());
                        mbytOne = Convert.ToByte(str.Split('-')[1].ToString());
                        mbytTwo = Convert.ToByte(str.Split('-')[2].ToString());
                        string strRemark = dr.GetValue(4).ToString();
                        strName = strName.Split('\\')[0].ToString() + "\\"
                                + strRemark;
                    }
                    dr.Close();
                }
                #endregion

                #region
                strsql = string.Format("select * from dbClassInfomation where DIndex={0} and ClassID={1} order by SenNum", DIndex, 1);
                dr = DataModule.SearchAResultSQLDB(strsql, CsConst.mstrCurPath);
                IRCodes = new List<UVCMD.IRCode>();
                if (dr != null)
                {
                    while (dr.Read())
                    {
                        string strRemark = dr.GetValue(4).ToString();
                        string str = dr.GetValue(5).ToString();
                        UVCMD.IRCode temp = new UVCMD.IRCode();
                        temp.KeyID = Convert.ToInt32(str.Split('-')[0].ToString());
                        temp.IRLength = Convert.ToInt32(str.Split('-')[1].ToString());
                        temp.IRLoation = Convert.ToInt32(str.Split('-')[2].ToString());
                        temp.Enable = Convert.ToByte(str.Split('-')[3].ToString());
                        temp.Codes = str.Split('-')[4].ToString();
                        temp.Remark1 = strRemark;
                        IRCodes.Add(temp);
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
        //保存数据库面板设置，将所有数据保存
        //</summary>
        public void SaveSendIRToDB()
        {
            try
            {
                string strsql = string.Format("delete * from dbClassInfomation where DIndex={0}", DIndex);
                DataModule.ExecuteSQLDatabase(strsql);

                #region
                string strDeviceRemark = "";
                if (strName.Contains("\\")) strDeviceRemark = strName.Split('\\')[1].ToString();
                string strBasic = RemoteEnable.ToString() + "-" + mbytOne.ToString()
                                + "-" + mbytTwo.ToString();
                strsql = string.Format("Insert into dbClassInfomation(DIndex,ClassID,ObjectID,SenNum,Remark,strParam1) values ({0},{1},{2},{3},'{4}','{5}')",
                         DIndex, 0, 0, 0, strDeviceRemark, strBasic);
                DataModule.ExecuteSQLDatabase(strsql);
                #endregion

                #region
                if (IRCodes != null)
                {
                    for (int i = 0; i < IRCodes.Count; i++)
                    {
                        UVCMD.IRCode temp = IRCodes[i];
                        string strParam = temp.KeyID.ToString() + "-" + temp.IRLength.ToString()
                                        + "-" + temp.IRLoation.ToString() + "-" + temp.Enable.ToString()
                                        + "-" + temp.Codes.ToString();
                        strsql = string.Format("Insert into dbClassInfomation(DIndex,ClassID,ObjectID,SenNum,Remark,strParam1) values ({0},{1},{2},{3},'{4}','{5}')",
                                 DIndex, 1, 0, i, temp.Remark1, strParam);
                        DataModule.ExecuteSQLDatabase(strsql);
                    }
                }
                #endregion
            }
            catch
            {
            }
        }

        /// <summary>
        ///upload all information to IR sender
        /// </summary>
        /// <param name="DIndex"></param>
        /// <param name="DevID"></param>
        /// <param name="DeviceType"></param>
        /// <param name="DevName"></param>
        /// <returns></returns>
        public bool UploadPanelInfosToDevice(string DevNam, int wdDeviceType, int intActivePage)
        {
            string strMainRemark = DevNam.Split('\\')[1].Trim();
            DevNam = DevNam.Split('\\')[0].Trim();
            byte bytSubID = byte.Parse(DevNam.Split('-')[0].ToString());
            byte bytDevID = byte.Parse(DevNam.Split('-')[1].ToString());

            byte[] ArayMain = new byte[20];
            byte[] arayTmpRemark = HDLUDP.StringToByte(strMainRemark);
            //ruby test
            if (arayTmpRemark.Length > 20)
            {
                Array.Copy(arayTmpRemark, 0, ArayMain, 0, 20);
            }
            else
            {
                Array.Copy(arayTmpRemark, 0, ArayMain, 0, arayTmpRemark.Length);
            }

            if (CsConst.mySends.AddBufToSndList(ArayMain, 0x0010, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType)) == false)
            {
                return false;
            }

            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(5);
            if (intActivePage == 0 || intActivePage == 1)
            {
                #region
                if (wdDeviceType == 306 || wdDeviceType == 319 || wdDeviceType == 313)
                {
                    byte[] arayTmp = new byte[2];
                    arayTmp[0] = 255;
                    arayTmp[1] = RemoteEnable;
                    if (CsConst.mySends.AddBufToSndList(arayTmp, 0xE01C, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType)) == true)
                    {
                        CsConst.myRevBuf = new byte[1200];
                        HDLUDP.TimeBetwnNext(arayTmp.Length);
                    }
                }

                if (IRCodes == null) return false;
                int count = IRCodes.Count;
                int Int = 0;
                foreach (UVCMD.IRCode TmpIRcode in IRCodes)
                {
                    Int++;
                    if (TmpIRcode.Codes != "" && TmpIRcode.Codes != null)
                    {
                        string[] strTmp = TmpIRcode.Codes.Split(';');
                        //上传红外码
                        #region
                        byte[] arayTmp1 = new byte[3];
                        arayTmp1[0] = byte.Parse((TmpIRcode.KeyID - 1).ToString());
                        int IRSize = Convert.ToInt32(strTmp[0].Split(' ')[2] + strTmp[0].Split(' ')[3], 16);
                        arayTmp1[1] = byte.Parse((IRSize / 256).ToString());
                        arayTmp1[2] = byte.Parse((IRSize % 256).ToString());    // ask for enough space for IR code
                        if (CsConst.mySends.AddBufToSndList(arayTmp1, 0xD900, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType)) == true)
                        {
                            HDLUDP.TimeBetwnNext(arayTmp1.Length);

                            for (int i = 0; i < strTmp.Length; i++)
                            {
                                byte[] arayIR = new byte[16];

                                string[] araystrIR = strTmp[i].Split(' ');
                                for (int j = 0; j < 16; j++)
                                {
                                    arayIR[j] = HDLPF.StringToByte(araystrIR[j].ToString());
                                }
                                if (CsConst.mySends.AddBufToSndList(arayIR, 0xD906, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType)) == true)
                                {
                                    HDLUDP.TimeBetwnNext(arayIR.Length);
                                }
                                else return false;
                            }

                            byte[] arayRemark = new byte[20];
                            string strRemark = TmpIRcode.Remark1;
                            byte[] arayTmp2 = HDLUDP.StringToByte(strRemark);
                            //arayTmp2.CopyTo(arayRemark, 0);
                            if (arayTmp2.Length > 20)
                            {
                                Array.Copy(arayTmp2, 0, arayRemark, 0, 20);
                            }
                            else
                            {
                                Array.Copy(arayTmp2, 0, arayRemark, 0, arayTmp2.Length);
                            }
                            byte[] araySendIR = new byte[12];
                            araySendIR[0] = byte.Parse((TmpIRcode.KeyID - 1).ToString());
                            araySendIR[1] = 0; //save the remark first time
                            for (int K = 0; K <= 9; K++) araySendIR[2 + K] = arayRemark[K];
                            if (CsConst.mySends.AddBufToSndList(araySendIR, 0xD90E, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType)) == true)
                            {
                                HDLUDP.TimeBetwnNext(araySendIR.Length);
                            }
                            else return false;

                            araySendIR[1] = 1;    //save the remark then
                            for (int K = 0; K <= 9; K++) araySendIR[2 + K] = arayRemark[10 + K];

                            if (CsConst.mySends.AddBufToSndList(araySendIR, 0xD90E, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType)) == true)
                            {
                                HDLUDP.TimeBetwnNext(araySendIR.Length);
                            }
                            else return false;
                            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(Int * (90 / count) + 5, null);
                        }
                        else return false;
                        #endregion
                    }
                }
                #endregion
            }
            if (intActivePage == 0)
            {
                #region
                if (CsConst.isRestore)
                {
                    byte[] ArayTmp = new byte[2];
                    //发送第一个开关
                    ArayTmp[0] = 0;
                    ArayTmp[1] = mbytOne;
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xD960, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType)) == true)
                    {
                        HDLUDP.TimeBetwnNext(ArayTmp.Length);
                    }
                    else return false;
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(91);
                    //发送第二个开关
                    ArayTmp[0] = 1;
                    if (mbytTwo != 0)
                    {
                        ArayTmp[1] = mbytTwo;
                    }
                    else
                    {
                        ArayTmp[1] = 0;
                    }
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xD960, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType)) == true)
                    {
                        HDLUDP.TimeBetwnNext(ArayTmp.Length);
                    }
                    else return false;
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(92);
                }
                #endregion
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(100);
            return true;
        }

        /// <summary>
        /// download all ir codes from devices 
        /// </summary>
        /// <param name="DevName"></param>
        /// <param name="wdDeviceType"></param>
        /// <returns></returns>
        public void DownLoadIRInfoFrmDevice(string DevNam, int wdDeviceType, int intActivePage, int num1, int num2)
        {
            string strMainRemark = DevNam.Split('\\')[1].Trim();
            DevNam = DevNam.Split('\\')[0].Trim();
            byte bytSubID = byte.Parse(DevNam.Split('-')[0].ToString());
            byte bytDevID = byte.Parse(DevNam.Split('-')[1].ToString());

            byte[] ArayTmp = null;
            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x000E, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType)) == false)
            {
                return;
            }
            else
            {
                byte[] arayRemark = new byte[20];
                for (int intI = 0; intI < 20; intI++) { arayRemark[intI] = CsConst.myRevBuf[25 + intI]; }
                strName = bytSubID.ToString() + "-" + bytDevID.ToString() + "\\" + HDLPF.Byte2String(arayRemark);
                CsConst.myRevBuf = new byte[1200];
            }
            if (CsConst.isRestore)
            {
                num1 = 1;
                num2 = 249;
            }
            if (intActivePage == 0 || intActivePage == 1)
            {
                IRCodes = new List<UVCMD.IRCode>();
                #region
                if (wdDeviceType == 306 || wdDeviceType == 319 || wdDeviceType == 313)
                {
                    ArayTmp = new byte[1];
                    ArayTmp[0] = 255;
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xE017, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType)) == true)
                    {
                        RemoteEnable = CsConst.myRevBuf[26];
                        CsConst.myRevBuf = new byte[1200];
                    }
                }
                HDLUDP.TimeBetwnNext(ArayTmp.Length);
                ArayTmp = new byte[2];
                IRCodes = new List<UVCMD.IRCode>();
                //读取红外有效性
                for (int i = num1; i <= num2; i++)
                {
                    UVCMD.IRCode temp = new UVCMD.IRCode();
                    temp.IRLength = 0;
                    temp.IRLoation = 0;
                    temp.KeyID = i;
                    temp.Codes = "";
                    temp.Remark1 = "";
                    temp.Enable = 0;
                    IRCodes.Add(temp);
                }
                ArayTmp = new byte[0];
                int intTmp = 0;
                byte ValidCount = 0;
                byte FirstKey = 0;
                byte LastSendCount = 0;
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xd914, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType)) == true)
                {
                    ValidCount = CsConst.myRevBuf[26];
                    FirstKey = CsConst.myRevBuf[27];
                    CsConst.myRevBuf = new byte[1200];
                }
                else return;
                if (ValidCount > 11)
                {
                    intTmp = ValidCount / 11;
                    if (ValidCount % 11 != 0)
                    {
                        intTmp = intTmp + 1;
                        LastSendCount = Convert.ToByte(ValidCount % 11);
                    }
                }
                else if (ValidCount != 0 && ValidCount <= 11) intTmp = 1;
                List<byte> listTmp = new List<byte>();
                for (int i = 0; i < intTmp; i++)
                {
                    ArayTmp = new byte[2];
                    ArayTmp[0] = FirstKey;
                    ArayTmp[1] = 11;
                    if (i == intTmp - 1 && LastSendCount != 0)
                    {
                        ArayTmp[1] = LastSendCount;
                    }
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xd916, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType)) == true)
                    {
                        FirstKey = CsConst.myRevBuf[40];
                        for (int j = 0; j < ArayTmp[1]; j++)
                        {
                            if (!listTmp.Contains(CsConst.myRevBuf[29 + j]))
                            {
                                listTmp.Add(CsConst.myRevBuf[29 + j]);
                            }
                        }
                        CsConst.myRevBuf = new byte[1200];
                    }
                    else return;
                    HDLUDP.TimeBetwnNext(ArayTmp.Length);
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(i + 1);
                }
                for (int i = 0; i < IRCodes.Count; i++)
                {
                    for (int j = 0; j < listTmp.Count; j++)
                    {
                        if ((IRCodes[i].KeyID - 1) == Convert.ToInt32(listTmp[j]))
                        {
                            IRCodes[i].Enable = 1;
                            break;
                        }
                    }
                }
                for (int i = 0; i < IRCodes.Count; i++)
                {
                    if (IRCodes[i].Enable == 1)
                    {
                        ArayTmp = new byte[2];
                        ArayTmp[0] = Convert.ToByte(IRCodes[i].KeyID - 1);
                        ArayTmp[1] = 0;
                        byte[] arayRemark = new byte[20];
                        if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xD90C, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType)) == true)
                        {
                            Array.Copy(CsConst.myRevBuf,29,arayRemark,0,10);
                            CsConst.myRevBuf = new byte[1200];
                        }
                        else return;
                        HDLUDP.TimeBetwnNext(ArayTmp.Length);
                        ArayTmp = new byte[2];
                        ArayTmp[0] = Convert.ToByte(IRCodes[i].KeyID - 1);
                        ArayTmp[1] = 1;
                        if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xD90C, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType)) == true)
                        {
                            Array.Copy(CsConst.myRevBuf, 29, arayRemark, 10, 10);
                            CsConst.myRevBuf = new byte[1200];
                        }
                        else return;
                        HDLUDP.TimeBetwnNext(ArayTmp.Length);
                        IRCodes[i].Remark1 = HDLPF.Byte2String(arayRemark);

                        if (CsConst.isRestore)
                        {
                            ArayTmp = new byte[1];
                            ArayTmp[0] = Convert.ToByte(IRCodes[i].KeyID - 1);
                            int Length = 0;
                            int SendCount = 0;
                            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xD908, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType)) == true)
                            {
                                Length = CsConst.myRevBuf[26] * 256 + CsConst.myRevBuf[27];
                                CsConst.myRevBuf = new byte[1200];
                            }
                            else return;
                            Length = Length - 12;
                            SendCount = Length / 14;
                            SendCount = SendCount + 1;
                            if (Length % 14 != 0) SendCount = SendCount + 1;
                            string strCodes = "";
                            for (int j = 0; j < SendCount; j++)
                            {
                                ArayTmp = new byte[0];
                                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xD90A, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType)) == true)
                                {
                                    string str = "84 ";
                                    for (int k = 0; k < 15; k++)
                                    {
                                        str = str + CsConst.myRevBuf[26 + k].ToString("X2") + " ";
                                    }
                                    str = str.Trim();
                                    if (j < (SendCount - 1)) str = str + ";";
                                    strCodes = strCodes + str;
                                    CsConst.myRevBuf = new byte[1200];
                                }
                                HDLUDP.TimeBetwnNext(20);
                            }
                            IRCodes[i].Codes = strCodes;
                        }
                    }
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(i * (65 / (IRCodes.Count)) + 25);
                }

                if (wdDeviceType == 302 || wdDeviceType == 306 || wdDeviceType == 313 || wdDeviceType == 319)
                {
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xd962, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType)) == true)
                    {
                        mbytOne = CsConst.myRevBuf[25];
                        mbytTwo = CsConst.myRevBuf[26];
                        CsConst.myRevBuf = new byte[1200];
                    }
                    else return;
                }
                #endregion
                MyRead2UpFlags[0] = true;
            }
            
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(100);
        }

    }

}
