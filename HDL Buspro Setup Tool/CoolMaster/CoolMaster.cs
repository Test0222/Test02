using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;

namespace HDL_Buspro_Setup_Tool
{
    [Serializable]
    public class CoolMaster : HdlDeviceBackupAndRestore
    {
        public int DIndex;  ////设备唯一编号
        public string DeviceName;
        public bool[] MyRead2UpFlags = new bool[2];

        public List<ThirdPartAC> myACSetting;     

        //<summary>
        //读取默认设置
        //</summary>
        public void ReadDefaultInfo()
        {
            myACSetting = new List<ThirdPartAC>();
        }

        //<summary>
        //读取数据信息
        //</summary>
        public void ReadDataFrmDBTobuf(int DIndex)
        {
            try
            {
                #region
                string strsql = string.Format("select * from dbClassInfomation where DIndex={0} and ClassID={1} order by SenNum", DIndex, 0);
                OleDbDataReader dr = DataModule.SearchAResultSQLDB(strsql, CsConst.mstrCurPath);
                myACSetting = new List<ThirdPartAC>();
                if (dr != null)
                {
                    while (dr.Read())
                    {
                        ThirdPartAC temp = new ThirdPartAC();
                        string str = dr.GetValue(5).ToString();
                        temp.ID = Convert.ToByte(str.Split('-')[0].ToString());
                        temp.Enable = Convert.ToByte(str.Split('-')[1].ToString());
                        temp.ACNO = Convert.ToByte(str.Split('-')[2].ToString());
                        temp.CoolMasterAddress = Convert.ToByte(str.Split('-')[3].ToString());
                        temp.GroupID = Convert.ToByte(str.Split('-')[4].ToString());
                        temp.arayACinfo = (byte[])dr[6];
                        temp.Remark = dr.GetValue(4).ToString();
                        myACSetting.Add(temp);
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
                #region
                string strsql = string.Format("delete * from dbClassInfomation where DIndex={0}", DIndex);
                DataModule.ExecuteSQLDatabase(strsql);
                if (myACSetting != null)
                {
                    for (int i = 0; i < myACSetting.Count; i++)
                    {
                        ThirdPartAC temp = myACSetting[i];
                        string strParam = temp.ID.ToString() + "-" + temp.Enable.ToString() + "-" + temp.ACNO.ToString() + "-" +
                                          temp.CoolMasterAddress.ToString() + "-" + temp.GroupID.ToString();
                        strsql = @"Insert into dbClassInfomation(DIndex,ClassID,ObjectID,SenNum,Remark,strParam1,byteAry1)"
                                  + " values(@DIndex,@ClassID,@ObjectID,@SenNum,@Remark,@strParam1,@byteAry1)";
                        OleDbConnection conn;
                        conn = new OleDbConnection(DataModule.ConString + CsConst.mstrDefaultPath);
                        //OleDbConnection conn = new OleDbConnection(DataModule.ConString + CsConst.mstrCurPath);
                        conn.Open();
                        OleDbCommand cmd = new OleDbCommand(strsql, conn);
                        ((OleDbParameter)cmd.Parameters.Add("@DIndex", OleDbType.Integer)).Value = DIndex;
                        ((OleDbParameter)cmd.Parameters.Add("@ClassID", OleDbType.Integer)).Value = 0;
                        ((OleDbParameter)cmd.Parameters.Add("@ObjectID", OleDbType.Integer)).Value = 0;
                        ((OleDbParameter)cmd.Parameters.Add("@SenNum", OleDbType.Integer)).Value = i;
                        ((OleDbParameter)cmd.Parameters.Add("@Remark", OleDbType.VarChar)).Value = temp.Remark;
                        ((OleDbParameter)cmd.Parameters.Add("@strParam1", OleDbType.VarChar)).Value = strParam;
                        ((OleDbParameter)cmd.Parameters.Add("@byteAry1", OleDbType.Binary)).Value = temp.arayACinfo;
                        try
                        {
                            cmd.ExecuteNonQuery();
                        }
                        catch
                        {
                            conn.Close();
                        }
                        conn.Close();
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

            byte[] ArayMain = new byte[20];
            byte[] arayTmpRemark = HDLUDP.StringToByte(strMainRemark);
            byte[] ArayTmp = null;
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
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(5, null);
            if (intActivePage == 0 || intActivePage == 1)
            {
                for (int i = 0; i < myACSetting.Count; i++)
                {
                    ArayTmp = new byte[15];
                    ArayTmp[0] = myACSetting[i].ID;
                    ArayTmp[1] = myACSetting[i].ACNO;
                    ArayTmp[2] = myACSetting[i].Enable;
                    ArayTmp[3] = myACSetting[i].CoolMasterAddress;
                    ArayTmp[4] = myACSetting[i].GroupID;
                    Array.Copy(myACSetting[i].arayACinfo, 0, ArayTmp, 5, 10);
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x0242, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType)) == true)
                    {
                        CsConst.myRevBuf = new byte[1200];
                        HDLUDP.TimeBetwnNext(ArayTmp.Length);
                    }
                    else return false;

                    ArayTmp = new byte[21];
                    ArayTmp[0] = myACSetting[i].ID;
                    byte[] arayRemark = HDLUDP.StringToByte(myACSetting[i].Remark);
                    if (arayRemark.Length > 20)
                        Array.Copy(arayRemark, 0, ArayTmp, 1, 20);
                    else
                        Array.Copy(arayRemark, 0, ArayTmp, 1, arayRemark.Length);
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x0246, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType)) == true)
                    {
                        CsConst.myRevBuf = new byte[1200];
                        HDLUDP.TimeBetwnNext(ArayTmp.Length);
                    }
                    else return false;
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(6 + i, null);
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
            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x000E, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType)) == false)
            {
                return false;
            }
            else
            {
                byte[] arayRemark = new byte[20];
                for (int intI = 0; intI < 20; intI++) { arayRemark[intI] = CsConst.myRevBuf[25 + intI]; }
                DeviceName = bytSubID.ToString() + "-" + bytDevID.ToString() + "\\" + HDLPF.Byte2String(arayRemark);
                CsConst.myRevBuf = new byte[1200];
            }
            if (CsConst.isRestore)
            {
                num1 = 1;
                num2 = 64;
            }
            if (intActivePage == 0 || intActivePage == 1)
            {
                myACSetting = new List<ThirdPartAC>();
                for (int i = num1; i <= num2; i++)
                {
                    ArayTmp = new byte[1];
                    ArayTmp[0] = Convert.ToByte(i);
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x0240, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType)) == true)
                    {
                        ThirdPartAC temp = new ThirdPartAC();
                        temp.ID = Convert.ToByte(i);
                        temp.ACNO = CsConst.myRevBuf[26];
                        temp.Enable = CsConst.myRevBuf[27];
                        temp.CoolMasterAddress = CsConst.myRevBuf[28];
                        temp.GroupID = CsConst.myRevBuf[29];
                        temp.arayACinfo = new byte[10];
                        Array.Copy(CsConst.myRevBuf, 30, temp.arayACinfo, 0, 10);
                        CsConst.myRevBuf = new byte[1200];
                        HDLUDP.TimeBetwnNext(ArayTmp.Length);
                        if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x0244, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType)) == true)
                        {
                            byte[] arayRemark = new byte[20];
                            Array.Copy(CsConst.myRevBuf, 26, arayRemark, 0, 20);
                            temp.Remark = HDLPF.Byte2String(arayRemark);
                            CsConst.myRevBuf = new byte[1200];
                            HDLUDP.TimeBetwnNext(ArayTmp.Length);
                        }
                        else return false;
                        myACSetting.Add(temp);
                    }
                    else return false;
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(i, null);
                }
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(100, null);
            return true;
        }
    }
}
