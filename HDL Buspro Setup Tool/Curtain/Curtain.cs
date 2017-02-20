using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.Xml;
using System.IO;
using System.Windows.Forms;

namespace HDL_Buspro_Setup_Tool
{
     [Serializable]
    public class Curtain : HdlDeviceBackupAndRestore
    {

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public int DIndex;
        public string DeviceName;
        public byte bytCurType; // 0 normal curtain or shutter or new type, can drag by hand or percent controlled
        public int intJogTime;
        public int intJogTime1;
        public int intJogTime2;
        public int intJogTime3;

        public byte bytInvert; // forward and backwork   or Backward forward
        public byte bytAutoMeasure; // 上电自动测速  上电自动关闭
        public byte bytDragMode; // 0 : no action, 1 long drag ; 2 short drag
        public int intDragLong;
        public int intDragShort;
        public int intDragSafe; 

        internal List<BasicCurtain> Curtains = null;
        public bool[] MyRead2UpFlags = new bool[2]; //前面四个表示读取 后面表示上传

        //<summary>
        //读取默认的动静传感器设置，将所有数据读取缓存
        //</summary>
        public void ReadDefaultInfo()
        {
            Curtains = new List<BasicCurtain>();
            
        }

        //<summary>
        //读取数据库面板设置，将所有数据读至缓存
        //</summary>
        public void ReadCurtainFrmDBTobuf(int intDIndex)
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
                        bytCurType = Convert.ToByte(str.Split('-')[0].ToString());
                        intJogTime = Convert.ToByte(str.Split('-')[1].ToString());
                        intJogTime1 = Convert.ToByte(str.Split('-')[2].ToString());
                        bytInvert = Convert.ToByte(str.Split('-')[3].ToString());
                        bytAutoMeasure = Convert.ToByte(str.Split('-')[4].ToString());
                        bytDragMode = Convert.ToByte(str.Split('-')[5].ToString());
                        intDragLong = Convert.ToByte(str.Split('-')[6].ToString());
                        intDragShort = Convert.ToByte(str.Split('-')[7].ToString());
                        intDragSafe = Convert.ToByte(str.Split('-')[8].ToString());
                    }
                    dr.Close();
                }
                #endregion

                #region
                Curtains = new List<BasicCurtain>();
                strsql = string.Format("select * from dbClassInfomation where DIndex={0} and ClassID={1} order by SenNum", DIndex, 1);
                dr = DataModule.SearchAResultSQLDB(strsql, CsConst.mstrCurPath);
                if (dr != null)
                {
                    while (dr.Read())
                    {
                        BasicCurtain temp = new BasicCurtain();
                        string str = dr.GetValue(5).ToString();
                        temp.runTime = Convert.ToByte(str.Split('-')[0].ToString());
                        temp.onDelay = Convert.ToByte(str.Split('-')[1].ToString());
                        temp.offDelay = Convert.ToByte(str.Split('-')[2].ToString());
                        temp.remark = dr.GetValue(4).ToString();
                        Curtains.Add(temp);
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
        public void SaveCurtainToDB()
        {
            try
            {
                #region
                string strsql = string.Format("delete * from dbClassInfomation where DIndex={0}", DIndex);
                DataModule.ExecuteSQLDatabase(strsql);

                string strDeviceRemark = "";
                if (DeviceName.Contains("\\")) strDeviceRemark = DeviceName.Split('\\')[1].ToString();
                string strParam = bytCurType.ToString() + "-" + intJogTime.ToString()
                                        + "-" + intJogTime1.ToString() + "-" + bytInvert.ToString()
                                        + "-" + bytAutoMeasure.ToString() + "-" + bytDragMode.ToString()
                                        + "-" + intDragLong.ToString() + "-" + intDragShort.ToString()
                                        + "-" + intDragSafe.ToString();
                strsql = string.Format("Insert into dbClassInfomation(DIndex,ClassID,ObjectID,SenNum,Remark,strParam1) values ({0},{1},{2},{3},'{4}','{5}')",
                         DIndex, 0, 0, 0, strDeviceRemark, strParam);
                DataModule.ExecuteSQLDatabase(strsql);
                #endregion

                #region
                if (Curtains != null)
                {
                    for (int i = 0; i < Curtains.Count; i++)
                    {
                        BasicCurtain temp = Curtains[i];
                        strParam = temp.runTime.ToString() + "-" + temp.onDelay.ToString()
                                        + "-" + temp.offDelay.ToString();
                        strsql = string.Format("Insert into dbClassInfomation(DIndex,ClassID,ObjectID,SenNum,Remark,strParam1) values ({0},{1},{2},{3},'{4}','{5}')",
                                 DIndex, 1, 0, 0, temp.remark, strParam);
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
        /// 上传设置到窗帘模块
        /// </summary>
        /// <param name="DIndex"></param>
        /// <param name="DevID"></param>
        /// <param name="DeviceType"></param>
        /// <param name="DevName"></param>
        /// <returns></returns>
        public bool UploaDeviceFromBufferToDevice(string DevName, int wdDeviceType)
        {
            byte bytI = 1;

            string strMainRemark = DevName.Split('\\')[1].Trim().Split('(')[0].Trim();
            DevName = DevName.Split('\\')[0].Trim();

            //保存basic informations
            byte bytSubID = byte.Parse(DevName.Split('-')[0].ToString());
            byte bytDevID = byte.Parse(DevName.Split('-')[1].ToString());
            byte[] ArayMain = new byte[20];

            if (HDLSysPF.ModifyDeviceMainRemark(bytSubID, bytDevID, strMainRemark, wdDeviceType) == false)
            {
                return false;
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(2);
            HDLUDP.TimeBetwnNext(20);
            #region
            if (CurtainDeviceType.NormalCurtainG1DeviceType.Contains(wdDeviceType) || CurtainDeviceType.CurtainG2DeviceType.Contains(wdDeviceType))
            {
                if (Curtains != null)
                {
                    foreach (BasicCurtain Tmp in Curtains)
                    {
                        Tmp.ModifyCurtainSetupInformation(bytSubID, bytDevID, bytI, wdDeviceType);
                        bytI++;
                    }
                }
            }
            else
            {

                if (CurtainDeviceType.NormalMotorCurtainDeviceType.Contains(wdDeviceType)) bytCurType = 2;
                else if (CurtainDeviceType.RollerCurtainDeviceType.Contains(wdDeviceType)) bytCurType = 2;
                else bytCurType = 1;
                //if new curtain, add jog time to it
                if (bytCurType == 1)
                {
                    byte[] arayTmp = new byte[3];
                    arayTmp[0] = 1;
                    arayTmp[1] = byte.Parse((intJogTime / 256).ToString());
                    arayTmp[2] = byte.Parse((intJogTime % 256).ToString());
                    if (CsConst.mySends.AddBufToSndList(arayTmp, 0x1C74, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType)) == false) return false;
                    HDLUDP.TimeBetwnNext(3);

                    //if new curtain, add jog time to it
                    arayTmp[0] = 2;
                    arayTmp[1] = byte.Parse((intJogTime1 / 256).ToString());
                    arayTmp[2] = byte.Parse((intJogTime1 % 256).ToString());
                    if (CsConst.mySends.AddBufToSndList(arayTmp, 0x1C74, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType)) == false) return false;
                    HDLUDP.TimeBetwnNext(20);

                    //if new curtain, add jog time to it
                    arayTmp[0] = 3;
                    arayTmp[1] = byte.Parse((intJogTime2 / 256).ToString());
                    arayTmp[2] = byte.Parse((intJogTime2 % 256).ToString());
                    if (CsConst.mySends.AddBufToSndList(arayTmp, 0x1C74, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType)) == false) return false;
                    HDLUDP.TimeBetwnNext(20);

                    //if new curtain, add jog time to it
                    arayTmp[0] = 4;
                    arayTmp[1] = byte.Parse((intJogTime3 / 256).ToString());
                    arayTmp[2] = byte.Parse((intJogTime3 % 256).ToString());
                    if (CsConst.mySends.AddBufToSndList(arayTmp, 0x1C74, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType)) == false) return false;
                    HDLUDP.TimeBetwnNext(20);
                }
                else if (bytCurType == 2)
                {
                    byte[] arayTmp = new byte[9];
                    arayTmp[0] = bytInvert;
                    arayTmp[1] = bytAutoMeasure;
                    arayTmp[2] = bytDragMode;
                    arayTmp[3] = byte.Parse((intDragLong * 100 / 256).ToString());
                    arayTmp[4] = byte.Parse((intDragLong * 100 % 256).ToString());
                    arayTmp[5] = byte.Parse((intDragShort * 100 / 256).ToString());
                    arayTmp[6] = byte.Parse((intDragShort * 100 % 256).ToString());
                    arayTmp[7] = byte.Parse((intDragSafe * 100 / 256).ToString());
                    arayTmp[8] = byte.Parse((intDragSafe * 100 % 256).ToString());

                    if (CurtainDeviceType.RollerCurtainDeviceType.Contains(wdDeviceType))
                        for (int i = 1; i < 9; i++) arayTmp[i] = 0;
                    if (CsConst.mySends.AddBufToSndList(arayTmp, 0x1F00, bytSubID, bytDevID, false, false, true, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType)) == true)
                    {
                        if (CsConst.myRevBuf[25] == 0xF5)
                        {
                            MessageBox.Show(CsConst.mstrINIDefault.IniReadValue("Public", "99606", ""), "",
                            MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                            return false;
                        }
                        CsConst.myRevBuf = new byte[1200];
                    }
                    else
                    {
                        MessageBox.Show(CsConst.mstrINIDefault.IniReadValue("Public", "99606", ""), "",
                            MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                        return false;
                    }
                    HDLUDP.TimeBetwnNext(3);
                }
            }
            #endregion
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(100,null);
            return true;
        }

        /// <summary>
        /// devname device name 
        /// </summary>
        /// <param name="DevName"></param>
        public void DownLoadInformationFrmDevice(string DevName, int wdDeviceType)
        {
            string strMainRemark = DevName.Split('\\')[1].Trim();
            DevName = DevName.Split('\\')[0].Trim();

            //保存basic informations网络信息
            byte bytSubID = byte.Parse(DevName.Split('-')[0].ToString());
            byte bytDevID = byte.Parse(DevName.Split('-')[1].ToString());

            byte[] ArayTmp = new byte[0];
            DeviceName = bytSubID.ToString() + "-" + bytDevID.ToString() + "\\" + HDLSysPF.ReadDeviceMainRemark(bytSubID, bytDevID);
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(5, null);

            int MaxValue = DeviceTypeList.GetMaxValueFromPublicModeGroup(wdDeviceType);

            ArayTmp = new byte[1];
            Curtains = new List<BasicCurtain>();
            // read curtain paramters 
            #region
            if (CurtainDeviceType.NormalCurtainG1DeviceType.Contains(wdDeviceType) || CurtainDeviceType.CurtainG2DeviceType.Contains(wdDeviceType))
            {
                for (byte bytI = 1; bytI <= MaxValue * 2; bytI++)
                {
                    ArayTmp[0] = bytI;
                    BasicCurtain oTmp = new BasicCurtain();
                    oTmp.ReadCurtainSetupInformation(bytSubID, bytDevID, bytI, wdDeviceType);
                    Curtains.Add(oTmp);
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(bytI * 45 / MaxValue, null);
                }
            }
            #endregion
            // read joggle time 
            if (CurtainDeviceType.CurtainG2DeviceType.Contains(wdDeviceType)) bytCurType = 1;
            else if (CurtainDeviceType.RollerCurtainDeviceType.Contains(wdDeviceType)) bytCurType = 2;
            else if (CurtainDeviceType.NormalMotorCurtainDeviceType.Contains(wdDeviceType)) bytCurType = 2;
            
            if (bytCurType == 1)
            {
                for (byte bytI = 1; bytI <= 4; bytI++)
                {
                    ArayTmp[0] = bytI;
                    BasicCurtain oTmp = new BasicCurtain();
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1C76, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType)) == true)
                    {
                        switch (bytI)
                        {
                            case 1: intJogTime = CsConst.myRevBuf[26] * 256 + CsConst.myRevBuf[27]; break;
                            case 2: intJogTime1 = CsConst.myRevBuf[26] * 256 + CsConst.myRevBuf[27]; break;
                            case 3: intJogTime2 = CsConst.myRevBuf[26] * 256 + CsConst.myRevBuf[27]; break;
                            case 4: intJogTime3 = CsConst.myRevBuf[26] * 256 + CsConst.myRevBuf[27]; break;
                        }
                        CsConst.myRevBuf = new byte[1200];
                    }
                    else return;
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(80 + bytI, null);
                }
            }
            else if (bytCurType == 2)
            {
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1F02, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType)) == true)
                {
                    bytInvert = CsConst.myRevBuf[25]; // forward and backwork   or Backward forward
                    bytAutoMeasure = CsConst.myRevBuf[26]; // 上电自动测速
                    bytDragMode = CsConst.myRevBuf[27]; // 0 : no action, 1 long drag ; 2 short drag
                    intDragLong = (CsConst.myRevBuf[28] * 256 + CsConst.myRevBuf[29]) / 100;
                    intDragShort = (CsConst.myRevBuf[30] * 256 + CsConst.myRevBuf[31]) / 100;
                    intDragSafe = (CsConst.myRevBuf[32] * 256 + CsConst.myRevBuf[33]) / 100;
                }
                else return;
                CsConst.myRevBuf = new byte[1200];
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(95, null);
            }
            MyRead2UpFlags[0] = true;
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(100, null);
        }

    }
}
