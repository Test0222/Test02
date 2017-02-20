using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;

namespace HDL_Buspro_Setup_Tool
{
    public class HVAC : ICloneable
    {
        public object Clone()
        {
            return this.MemberwiseClone();
        }
        public int DIndex;  ////工程内唯一编号
        public string DeviceName;
        public byte bytAutoRun;

        public byte bytModeONDelay; // 模式,风速开关延迟
        public byte bytModeOFFDelay;
        public byte bytFanONDelay;
        public byte bytFanOFFDelay;

        public byte bytCOrP; //电压还是电流输出
        public byte bytHigh;
        public byte bytMid;
        public byte bytLow;

        public byte[] bytSteps; //总共三个序列,每个序列四步 新的类型  140： 主从机 + 温度传感器 （开始为简单模式，复杂模式）

        public bool[] MyRead2UpFlags = new bool[2]{false,false}; //前面四个表示读取 后面表示上传

        ////<summary>
        ////读取默认的HVAC设置，将所有数据读取缓存
        ////</summary>
        public void ReadDefaultInfo()
        {
            bytAutoRun = 0;

            bytModeONDelay = 1; // 模式,风速开关延迟
            bytModeOFFDelay = 1;
            bytFanONDelay = 1;
            bytFanOFFDelay = 1;

            bytCOrP = 0; //电压还是电流输出
            bytHigh = 10;
            bytMid = 6;
            bytLow = 1;

            bytSteps = new byte[200]; //总共三个序列,每个序列四步
        }

        //<summary>
        //读取数据库里的HVAC的设置，将所有数据读取缓存
        //</summary>
        public void ReadInfoForDB(int id)
        {
            //read all channels information save them to the buffer
            string str = "select * from dbHVAC where DIndex=" + id.ToString();
            OleDbDataReader dr = DataModule.SearchAResultSQLDB(str);

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    bytAutoRun = dr.GetByte(1);

                    bytModeONDelay = dr.GetByte(2); // 模式,风速开关延迟
                    bytModeOFFDelay = dr.GetByte(3);
                    bytFanONDelay = dr.GetByte(4);
                    bytFanOFFDelay = dr.GetByte(5);

                    bytCOrP = dr.GetByte(6); //电压还是电流输出
                    bytHigh = dr.GetByte(7);
                    bytMid = dr.GetByte(8);
                    bytLow = dr.GetByte(9);

                    bytSteps = (byte[])dr[10];
                }
            }
            dr.Close();
        }

        //<summary>
        //将缓存里所有HVAC的设置存在数据库
        //</summary>
        public void SaveInfoToDb()
        {
            //// delete all old information and refresh the database
            string strsql = string.Format("delete from dbHVAC where DIndex=" + DIndex.ToString());
            DataModule.ExecuteSQLDatabase(strsql);

            strsql = @"Insert into dbHVAC(DIndex,bytAutoRun,bytModeONDelay,bytModeOFFDelay,bytFanONDelay,bytFanOFFDelay,bytCOrP,bytHigh,"
                          + "bytMid,bytLow,bytSteps) values(@DIndex,@bytAutoRun,@bytModeONDelay,@bytModeOFFDelay,@bytFanONDelay,@bytFanOFFDelay,"
                          + "@bytCOrP,@bytHigh,@bytMid,@bytLow,@bytSteps)";
            //创建一个OleDbConnection对象
            OleDbConnection conn;
            conn = new OleDbConnection(DataModule.ConString + CsConst.mstrDefaultPath);
            conn.Open();

            if (bytSteps == null) bytSteps = new byte[12];

            OleDbCommand cmd = new OleDbCommand(strsql, conn);
            ((OleDbParameter)cmd.Parameters.Add("@DIndex", OleDbType.Integer)).Value = DIndex;
            ((OleDbParameter)cmd.Parameters.Add("@bytAutoRun", OleDbType.VarChar)).Value = bytAutoRun;
            ((OleDbParameter)cmd.Parameters.Add("@bytModeONDelay", OleDbType.VarChar)).Value = bytModeONDelay;
            ((OleDbParameter)cmd.Parameters.Add("@bytModeOFFDelay", OleDbType.VarChar)).Value = bytModeOFFDelay;
            ((OleDbParameter)cmd.Parameters.Add("@bytFanONDelay", OleDbType.VarChar)).Value = bytFanONDelay;
            ((OleDbParameter)cmd.Parameters.Add("@bytFanOFFDelay", OleDbType.VarChar)).Value = bytFanOFFDelay;

            ((OleDbParameter)cmd.Parameters.Add("@bytCOrP", OleDbType.VarChar)).Value = bytCOrP;
            ((OleDbParameter)cmd.Parameters.Add("@bytHigh", OleDbType.VarChar)).Value = bytHigh;
            ((OleDbParameter)cmd.Parameters.Add("@bytMid", OleDbType.VarChar)).Value = bytMid;
            ((OleDbParameter)cmd.Parameters.Add("@bytLow", OleDbType.VarChar)).Value = bytLow;
            ((OleDbParameter)cmd.Parameters.Add("@bytSteps", OleDbType.Binary)).Value = bytSteps;

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (OleDbException exp)
            {
                MessageBox.Show(exp.ToString());
            }
            conn.Close();
        }

        /// <summary>
        /// 将调光模块设备上传
        /// </summary>
        /// <param name="DIndex"></param>
        /// <param name="DevID"></param>
        /// <param name="DeviceType"></param>
        /// <param name="DevName"></param>
        /// <returns></returns>
        public bool UploaDeviceFromBufferToDevice(string DevName, int wdDeviceType)
        {
            string strMainRemark = DevName.Split('\\')[1].Trim().Split('(')[0].Trim();
            DevName = DevName.Split('\\')[0].Trim();

            //保存basic informations
            byte bytSubID = byte.Parse(DevName.Split('-')[0].ToString());
            byte bytDevID = byte.Parse(DevName.Split('-')[1].ToString());

            byte[] ArayMain = new byte[20];
            if (HDLSysPF.ModifyDeviceMainRemark(bytSubID, bytDevID, strMainRemark, wdDeviceType)==false)
            {
                return false;
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(1);

            //保存自动运行的标志
            //byte[] arayTmp = new byte[1];
            //arayTmp[0] = bytAutoRun;

            //if (CsConst.mySends.AddBufToSndList(arayTmp, 0x018E, bytSubID, bytDevID, false) == false)
            //{
            //    MessageBox.Show("This module is not online or the address is another one, please check it !!");
            //    return false;
            //}
            //HDLUDP.TimeBetwnNext(arayTmp.Length);

            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(20, null);

            // modify the Delay
            byte[]  arayTmp = new byte[4];
            arayTmp[0] = bytFanONDelay;
            arayTmp[1] = bytFanOFFDelay;
            arayTmp[2] = bytModeONDelay;
            arayTmp[3] = bytModeOFFDelay;
            if (CsConst.mySends.AddBufToSndList(arayTmp, 0xE3F6, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType)) == false) return false;
            HDLUDP.TimeBetwnNext(arayTmp.Length);

            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(40, null);

            // modify the Current or power
            arayTmp = new byte[3];
            arayTmp[0] = bytHigh;
            arayTmp[1] = bytMid;
            arayTmp[2] = bytLow;
            if (CsConst.mySends.AddBufToSndList(arayTmp, 0xE3FA, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType)) == false) return false;
            HDLUDP.TimeBetwnNext(arayTmp.Length);

            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(50, null);


            if (HVACModuleDeviceTypeList.HVACG2ComplexRelayDeviceTypeLists.Contains(wdDeviceType)) //复杂模式
            {
                //修改温度传感器设置
                #region
                arayTmp = new byte[20];
                Array.Copy(bytSteps, 140, arayTmp, 0, 18); // 保存工作模式 掉电恢复

                if (CsConst.mySends.AddBufToSndList(arayTmp, 0x1C4C, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType)) == true)
                {
                    HDLUDP.TimeBetwnNext(10);
                }
                else return false;
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(70, null);
                #endregion

                if (bytSteps[140] == 1)  //主机工作方式
                {
                    #region
                    arayTmp = new byte[4];
                    Array.Copy(bytSteps, 0, arayTmp, 0, 4); // 工作模式标志    节能模式   运行时间

                    if (CsConst.mySends.AddBufToSndList(arayTmp, 0x1C98, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType)) == true)
                    {
                        HDLUDP.TimeBetwnNext(10);
                    }
                    else return false;
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(55, null);
                    #endregion

                    if (bytSteps[0] == 1) // complex working mode go on uploading 
                    {
                        #region
                        arayTmp = new byte[6];
                        Array.Copy(bytSteps, 20, arayTmp, 0, 6); //工作模式  掉电恢复
                        if (CsConst.mySends.AddBufToSndList(arayTmp, 0x1C9C, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType)) == true)
                        {
                            HDLUDP.TimeBetwnNext(10);
                        }
                        else return false;
                        if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(60, null);
                        #endregion

                        arayTmp = new byte[7];
                        for (byte i = 0; i <= 4; i++)
                        {
                            arayTmp[0] = i;
                            Array.Copy(bytSteps, 40 + i * 20, arayTmp, 1, 6); //工作模式  运行时间
                            #region
                            if (CsConst.mySends.AddBufToSndList(arayTmp, 0x1CA0, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType)) == true)
                            {
                                HDLUDP.TimeBetwnNext(10);
                            }
                            else return false;
                            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(65, null);
                            #endregion
                        }

                        #region
                        arayTmp = new byte[1];
                        Array.Copy(bytSteps, 160, arayTmp, 0, 1); // 从bus读取的时间间隔
                        if (CsConst.mySends.AddBufToSndList(arayTmp, 0x1C90, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType)) == true)
                        {
                            HDLUDP.TimeBetwnNext(10);
                        }
                        else return false;
                        if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(75, null);
                        #endregion

                        //修改主从机
                        #region
                        arayTmp = new byte[20];
                        Array.Copy(bytSteps, 180, arayTmp, 0, 20);
                        if (CsConst.mySends.AddBufToSndList(arayTmp, 0x1C44, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType)) == true)
                        {
                            HDLUDP.TimeBetwnNext(10);
                        }
                        else return false;
                        if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(80, null);
                        #endregion
                    }
                }
            }
            else if (HVACModuleDeviceTypeList.HVACG1NormalRelayDeviceTypeLists.Contains(wdDeviceType)) // 第一代HVAC
            {
                #region
                for (int intI = 0; intI < 3; intI++)
                {
                    arayTmp = new byte[7];
                    arayTmp[0] = 0xF8;
                    arayTmp[1] = Convert.ToByte((intI + 1).ToString());
                    for (int intJ = 0; intJ < 5; intJ++) arayTmp[2 + intJ] = bytSteps[intI * 5 + intJ];

                    if (CsConst.mySends.AddBufToSndList(arayTmp, 0xE3FE, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType)) == false) return false;
                    HDLUDP.TimeBetwnNext(arayTmp.Length);
                }
                #endregion
            }
            MyRead2UpFlags[1] = true;
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(100, null);
            return true;
        }

        /// <summary>
        /// subnet id + device id + remark
        /// </summary>
        /// <param name="DeName"></param>
        public void DownLoadInformationFrmDevice(string DevName,int wdDeviceType)
        {
            string strMainRemark = DevName.Split('\\')[1].Trim();
            DevName = DevName.Split('\\')[0].Trim();

            byte bytSubID = byte.Parse(DevName.Split('-')[0].ToString());
            byte bytDevID = byte.Parse(DevName.Split('-')[1].ToString());
            byte[] ArayTmp = null;

            // read on and off delay
            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xE3F4, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType)) == true)
            {
                bytFanONDelay = CsConst.myRevBuf[25];
                bytFanOFFDelay = CsConst.myRevBuf[26];
                bytModeONDelay = CsConst.myRevBuf[27];
                bytModeOFFDelay = CsConst.myRevBuf[28];
                CsConst.myRevBuf = new byte[1200];
                HDLUDP.TimeBetwnNext(10);
            }
            else
            {
                return ;
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(20, null);
            // read fan high middle low
            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xE3F8, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType)) == true)
            {
                bytHigh = CsConst.myRevBuf[25];
                bytMid = CsConst.myRevBuf[26];
                bytLow = CsConst.myRevBuf[27];
                CsConst.myRevBuf = new byte[1200];
                HDLUDP.TimeBetwnNext(10);
            }
            else return;
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(40, null);

            if (HVACModuleDeviceTypeList.HVACG2ComplexRelayDeviceTypeLists.Contains(wdDeviceType)) //复杂模式继电器
            {
                bytSteps = new byte[200];
                //读取温度传感器设置  主从机设置
                #region
                ArayTmp = null;
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1C4E, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType)) == true)
                {
                    Array.Copy(CsConst.myRevBuf, 25, bytSteps, 140, 18); // 保存工作模式 掉电恢复
                    CsConst.myRevBuf = new byte[1200];
                    HDLUDP.TimeBetwnNext(10);
                }
                else return;

                if (bytSteps[140] == 1) //complex working mode go on reading  
                {
                    #region
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1C9A, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType)) == true)
                    {
                        Array.Copy(CsConst.myRevBuf, 25, bytSteps, 0, 4); // 保存标志 节电使能位 工作时间
                        CsConst.myRevBuf = new byte[1200];
                        HDLUDP.TimeBetwnNext(10);
                    }
                    else return;
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(45, null);
                    #endregion

                    if (bytSteps[0] == 1) // complex working mode go on reading 
                    {
                        #region
                        if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1C9E, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType)) == true)
                        {
                            Array.Copy(CsConst.myRevBuf, 25, bytSteps, 20, 6); // 保存工作模式 掉电恢复
                            CsConst.myRevBuf = new byte[1200];
                            HDLUDP.TimeBetwnNext(10);
                        }
                        else return;
                        if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(45, null);
                        #endregion

                        ArayTmp = new byte[1];
                        for (byte i = 0; i <= 4; i++)
                        {
                            ArayTmp[0] = i;
                            #region
                            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1CA2, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType)) == true)
                            {
                                Array.Copy(CsConst.myRevBuf, 26, bytSteps, 40 + i * 20, 6); // 保存工作模式 掉电恢复
                                CsConst.myRevBuf = new byte[1200];
                                HDLUDP.TimeBetwnNext(10);
                            }
                            else return;
                            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(45, null);
                            #endregion
                        }

                        if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(55, null);               

                        #region
                        ArayTmp = null;
                        if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1C92, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType)) == true)
                        {
                            Array.Copy(CsConst.myRevBuf, 25, bytSteps, 160, 5); // 从bus读取的时间间隔
                            CsConst.myRevBuf = new byte[1200];
                            HDLUDP.TimeBetwnNext(10);
                        }
                        else return;
                        if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(60, null);
                        #endregion

                        //读取主从机
                        #region
                        ArayTmp = null;
                        if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1C46, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType)) == true)
                        {
                            Array.Copy(CsConst.myRevBuf, 25, bytSteps, 180, 20); // 从bus读取的时间间隔
                            CsConst.myRevBuf = new byte[1200];
                            HDLUDP.TimeBetwnNext(10);
                        }
                        else return;
                        if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(75, null);
                        #endregion
                    }
                }
                #endregion
            }
            else if (HVACModuleDeviceTypeList.HVACG1NormalRelayDeviceTypeLists.Contains(wdDeviceType)) // 普通继电器序列模式
            {
                // read relay setup
                #region
                ArayTmp = new byte[2];
                ArayTmp[0] = 0xF8;
                bytSteps = new byte[15];
                for (byte bytI = 1; bytI < 4; bytI++)
                {
                    ArayTmp[1] = bytI;
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xE3FC, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType)) == true)
                    {
                        for (byte bytJ = 0; bytJ < 5; bytJ++) { bytSteps[bytI * 3 + bytJ - 3] = CsConst.myRevBuf[27 + bytJ]; }
                        CsConst.myRevBuf = new byte[1200];
                        HDLUDP.TimeBetwnNext(10);

                        if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(40 + bytI * 20, null);
                    }
                    else return;
                }
                #endregion
            }
            MyRead2UpFlags[0] = true;
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(100, null);
        }
    }
}
