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
    public class LEDLibray
    {
        public string LEDName;//素材库灯名称
        public int intSumChns;
        public List<string> LEDChns; //素材库内灯的各回路信息
    }

    public class DMX : HdlDeviceBackupAndRestore
    {
        public int DIndex;
        public string Remark;
        public String DeviceName;

        public string strIP;
        public string strRouterIP;
        public string strMAC;
        public byte bytDMXChnID;
        public int intDMXPort;

        public Byte[] PowerSaving; // 是否节能

        public List<Chns> LoadTypes;
        public List<Areas> Areas512;

        public bool[] MyRead2UpFlags = new bool[8]; //上传下载标志

        public class Chns
        {
            public int intStart;
            public int intLibraryID;
            public string strRemark;
            public int ArrtiChang;
            public int MinValue;
            public int MaxValue;
            public int MaxLevel;
            public int intBelongs; // 属于哪个区域
            public List<Byte[]> arrLibraryParams = new List<byte[]>();
        }

        public class SceType
        {
            public int intSceID;
            public string Remark;
            public int intRunTime;
            public byte[] ArayLevel;
        }

        public class SeriesType : ICloneable
        {
            public object Clone()
            {
                SeriesType series = (SeriesType)this.MemberwiseClone(); // 浅复制
                series.SceneIDs = new List<byte>();
                foreach (var sc in SceneIDs)
                {
                    series.SceneIDs.Add(sc);
                }

                series.RunTimeH = new List<byte>();
                foreach (var sc in RunTimeH)
                {
                    series.RunTimeH.Add(sc);
                }

                series.RunTimeL = new List<byte>();
                foreach (var sc in RunTimeL)
                {
                    series.RunTimeL.Add(sc);
                }
                return (object)series;
                // return this.MemberwiseClone(); // 浅复制
                // return new SeriesType() as object; //深复制
            }

            public int SeriID;
            public string Remark;
            public int intStep;
            public int intTimes;
            public int intRunMode;
            public List<byte> SceneIDs;
            public List<byte> RunTimeH;
            public List<byte> RunTimeL;
        }

        public class Areas
        {
            public int AreaID;
            public string strRemark;
            public List<SceType> MySces;
            public List<SeriesType> MySers;
        }

        public void SaveDMXInformation()
        {
            String DevName = DeviceName.Split('\\')[0].Trim();
            //保存回路信息
            byte SubNetID = byte.Parse(DevName.Split('-')[0].ToString());
            byte DeviceID = byte.Parse(DevName.Split('-')[1].ToString());

            string strsql = "delete * from tmpDMX where ID =" + DIndex;
            DataModule.ExecuteSQLDatabase(strsql);

            string str = string.Format("insert into tmpDMX(ID,SubID,DevID,Remark,AddreIP,RoutIP,IPMAC,DMXChn,DMXPort) "
                       + " values({0},{1},{2},'{3}','{4}','{5}','{6}',{7},{8})",
                          DIndex, SubNetID, DeviceID, Remark, strIP, strRouterIP, strMAC, bytDMXChnID, intDMXPort);
            DataModule.ExecuteSQLDatabase(str);
        }

        public void SaveChannelsInformation()
        {
            string strsql = "delete * from tmpChns where ID = " + DIndex;
            DataModule.ExecuteSQLDatabase(strsql);

            strsql = "delete * from tmpArea where ID = " + DIndex;
            DataModule.ExecuteSQLDatabase(strsql);

            strsql = "delete * from tmpScene where ID = " + DIndex;
            DataModule.ExecuteSQLDatabase(strsql);

            strsql = "delete * from tmpSeries where ID = " + DIndex;
            DataModule.ExecuteSQLDatabase(strsql);
            

            if (LoadTypes != null)
            {
                int I = 1;
                foreach (Chns tmp in LoadTypes)
                {
                    string str = string.Format("insert into tmpChns(ID,HardID,ChnID,LoadType,Remark,Low,High,MaxValue,AreaID,ArrtiDim) values({0},{1},{2},{3},'{4}',{5},{6},{7},{8},{9})", DIndex, I, tmp.intStart, tmp.intLibraryID,
                                            tmp.strRemark, tmp.MinValue,tmp.MaxValue,tmp.MaxLevel, tmp.intBelongs, tmp.ArrtiChang);
                    
                    DataModule.ExecuteSQLDatabase(str);
                    I++;
                }
            }
        }

        public void SaveAreasInformation()
        {
            if (Areas512 != null)
            {
                foreach (Areas tmp in Areas512)
                {
                    string str = string.Format("select * from tmpArea where ID={0} and AreaID={1}", DIndex, tmp.AreaID);
                    if (DataModule.IsExitstInDatabase(str) == false)
                    {
                        str = string.Format("insert into tmpArea(ID,AreaID,Remark) values({0},{1},'{2}')", DIndex, tmp.AreaID, tmp.strRemark);
                    }
                    else
                    {
                        str = string.Format("update tmpArea set Remark='{0}' where ID={1} and AreaID ={2}", tmp.strRemark, DIndex, tmp.AreaID);
                    }
                    DataModule.ExecuteSQLDatabase(str);
                    SaveSceneInformation(tmp, tmp.AreaID);
                    SaveSeriesInformation(tmp, tmp.AreaID);
                }
            }
        }

        public void SaveSceneInformation(Areas oArea, int intI)
        {
            if (oArea.MySces != null)
            {
                foreach (SceType oSce in oArea.MySces)
                {
                    string str = "insert into tmpScene(ID,AreaID,SceID,Remark,RunTime,ChnLevel) values(@ID,@AreaID,@SceID,@Remark,@RunTime,@ChnLevel)";
                    OleDbConnection conn = new OleDbConnection(DataModule.ConString + CsConst.mstrCurPath);
                    conn.Open();

                    OleDbCommand cmd = new OleDbCommand(str, conn);
                    ((OleDbParameter)cmd.Parameters.Add("@ID", OleDbType.SmallInt)).Value = DIndex;
                    ((OleDbParameter)cmd.Parameters.Add("@AreaID", OleDbType.SmallInt)).Value = intI;
                    ((OleDbParameter)cmd.Parameters.Add("@SceID", OleDbType.SmallInt)).Value = oSce.intSceID;
                    ((OleDbParameter)cmd.Parameters.Add("@Remark", OleDbType.VarChar)).Value = oSce.Remark;
                    ((OleDbParameter)cmd.Parameters.Add("@RunTime", OleDbType.SmallInt)).Value = oSce.intRunTime;
                    ((OleDbParameter)cmd.Parameters.Add("@ChnLevel", OleDbType.Binary)).Value = oSce.ArayLevel;
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
            }

        }

        public void SaveSeriesInformation(Areas oArea, int intI)
        {
            if (oArea.MySers != null)
            {
                foreach (SeriesType oSce in oArea.MySers)
                {
                    string str = "insert into tmpSeries(ID,AreaID,SceID,Remark,intSteps,RepeatTimes,RunMode,SceNos,RunTimeH,RunTimeL) values(@ID,@AreaID,@SceID,@Remark,@intSteps,@RepeatTimes,@RunMode,@SceNos,@RunTimeH,@RunTimeL)";
                    OleDbConnection conn = new OleDbConnection(DataModule.ConString + CsConst.mstrCurPath);
                    conn.Open();

                    OleDbCommand cmd = new OleDbCommand(str, conn);
                    ((OleDbParameter)cmd.Parameters.Add("@ID", OleDbType.SmallInt)).Value = DIndex;
                    ((OleDbParameter)cmd.Parameters.Add("@AreaID", OleDbType.SmallInt)).Value = intI;
                    ((OleDbParameter)cmd.Parameters.Add("@SceID", OleDbType.SmallInt)).Value = oSce.SeriID;
                    ((OleDbParameter)cmd.Parameters.Add("@Remark", OleDbType.VarChar)).Value = oSce.Remark;

                    ((OleDbParameter)cmd.Parameters.Add("@intSteps", OleDbType.SmallInt)).Value = oSce.intStep;
                    ((OleDbParameter)cmd.Parameters.Add("@RepeatTimes", OleDbType.SmallInt)).Value = oSce.intTimes;
                    ((OleDbParameter)cmd.Parameters.Add("@RunMode", OleDbType.SmallInt)).Value = oSce.intRunMode;

                    byte[] bytTmp = new byte[oSce.intStep];
                    oSce.SceneIDs.CopyTo(bytTmp);

                    byte[] bytTmp1 = new byte[oSce.intStep];
                    oSce.RunTimeH.CopyTo(bytTmp1);

                    byte[] bytTmp2 = new byte[oSce.intStep];
                    oSce.RunTimeL.CopyTo(bytTmp2);

                    ((OleDbParameter)cmd.Parameters.Add("@SceNos", OleDbType.Binary)).Value = bytTmp;
                    ((OleDbParameter)cmd.Parameters.Add("@RunTimeH", OleDbType.Binary)).Value = bytTmp1;
                    ((OleDbParameter)cmd.Parameters.Add("@RunTimeL", OleDbType.Binary)).Value = bytTmp2;

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
            }

        }

        public void ReadDefaultSettings()
        {
            strIP = "192.168.010.250";
            strRouterIP = "192.168.010.001";
            strMAC = "H.D.L.085.085.085";
            bytDMXChnID = 0;
            intDMXPort = 4000;
        }

        public void ReadDMXInformation(int intDIndex)
        {
            String DevName = DeviceName.Split('\\')[0].Trim();
            //保存回路信息
            byte SubNetID = byte.Parse(DevName.Split('-')[0].ToString());
            byte DeviceID = byte.Parse(DevName.Split('-')[1].ToString());

            string str = "select * from tmpDMX where ID =" + intDIndex;

            OleDbDataReader dr = DataModule.SearchAResultSQLDB(str);

            if (dr != null)
            {
                while (dr.Read())
                {
                    SubNetID = dr.GetByte(1);
                    DeviceID = dr.GetByte(2);
                    Remark = dr.GetString(3);
                    strIP = dr.GetString(4);
                    strRouterIP = dr.GetString(5);
                    strMAC = dr.GetString(6);
                    bytDMXChnID = dr.GetByte(7);
                    intDMXPort = dr.GetInt16(8);
                }
                dr.Close();
            }
        }

        public void ReadChannelsInformation(int intDIndex)
        {
            LoadTypes = new List<Chns>();

            string str = "select * from tmpChns where ID =" + intDIndex + " order by HardID ";

            OleDbDataReader dr = DataModule.SearchAResultSQLDB(str);
            while (dr.Read())
            {
                Chns Tmp = new Chns();

                Tmp.intStart = dr.GetInt16(2);
                Tmp.intLibraryID = dr.GetInt16(3);
                Tmp.strRemark = dr.GetString(4).ToString();
                Tmp.MinValue = dr.GetInt16(5);
                Tmp.MaxValue = dr.GetInt16(6);
                Tmp.MaxLevel = dr.GetInt16(7);
                Tmp.intBelongs = dr.GetInt16(8);
                Tmp.ArrtiChang = dr.GetInt16(9);

                LoadTypes.Add(Tmp);
            }
            dr.Close();
        }

        public void ReadAreasInformation(int intDIndex)
        {
            Areas512 = new List<Areas>();
            string str = "select * from tmpArea where ID =" + intDIndex + " order by AreaID";

            OleDbDataReader dr = DataModule.SearchAResultSQLDB(str);

            int intI = 1;
            while (dr.Read())
            {
                Areas Tmp = new Areas();

                Tmp.AreaID = dr.GetInt16(1);
                Tmp.strRemark = dr.GetString(2);
                ReadSceneInformation(Tmp, intI, DIndex);
                ReadSeriesInformation(Tmp, intI, DIndex);
                Areas512.Add(Tmp);
                intI++;
            }
            dr.Close();
        }

        public void ReadSceneInformation(Areas oArea, int intI, int intDIndex)
        {
            if (oArea == null) return;
            oArea.MySces = new List<SceType>();

            string str = string.Format("select * from tmpScene where AreaID ={0} and ID = {1} order by SceID", intI, intDIndex);

            OleDbDataReader dr =  DataModule.SearchAResultSQLDB(str);
            while (dr.Read())
            {
                SceType Tmp = new SceType();
                Tmp.intSceID = dr.GetInt16(2);
                Tmp.Remark = dr.GetString(3);
                Tmp.intRunTime = dr.GetInt16(4);
                Tmp.ArayLevel = (byte[])dr[5];
                oArea.MySces.Add(Tmp);
            }
            dr.Close();
        }

        public void ReadSeriesInformation(Areas oArea, int intI, int intDIndex)
        {
            if (oArea == null) return;
            oArea.MySers = new List<SeriesType>();

            string str = string.Format("select * from tmpSeries where AreaID ={0} and ID = {1} order by SceID", intI, intDIndex);

            OleDbDataReader dr = DataModule.SearchAResultSQLDB(str);

            while (dr.Read())
            {
                SeriesType Tmp = new SeriesType();

                Tmp.SeriID = dr.GetInt16(2);
                Tmp.Remark = dr.GetString(3);
                Tmp.intStep = dr.GetInt16(4);
                Tmp.intTimes = dr.GetInt16(5);
                Tmp.intRunMode = dr.GetInt16(6);

                byte[] bytTmp = (byte[])dr[7];
                Tmp.SceneIDs = bytTmp.ToList();

                bytTmp = (byte[])dr[8];
                Tmp.RunTimeH = bytTmp.ToList();

                bytTmp = (byte[])dr[9];
                Tmp.RunTimeL = bytTmp.ToList();

                oArea.MySers.Add(Tmp);
            }
            dr.Close();
        }

        public void UploadSettings(int intDeviceType)
        {
            // 修改基本信息
            String DevName = DeviceName.Split('\\')[0].Trim();
            //保存回路信息
            byte bytSubID = byte.Parse(DevName.Split('-')[0].ToString());
            byte bytDevID = byte.Parse(DevName.Split('-')[1].ToString());

            byte[] ArayMain = new byte[20];
            string strRemark = Remark;

            if ( HDLSysPF.ModifyDeviceMainRemark(bytSubID, bytDevID, strRemark, intDeviceType) == false)
            {
                MessageBox.Show("This module is not online or the address is another one, please check it !!");
                return;
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(1);

            Byte[] arayTmp = new Byte[1];
            if (CsConst.mintDevicesHasIPNetworkDevType.Contains(intDeviceType))
            {
                string[] strTmp = strIP.Split('.');
                arayTmp = new byte[19];
                for (int i = 0; i < 4; i++) { arayTmp[i] = byte.Parse(strTmp[i].ToString()); }

                strTmp = strRouterIP.Split('.');
                for (int i = 0; i < 4; i++) { arayTmp[i + 4] = byte.Parse(strTmp[i].ToString()); }

                arayTmp[8] = 0x4D;
                arayTmp[9] = 0x49;
                arayTmp[10] = 0x52;
                strTmp = strMAC.Split('.');
                for (int i = 0; i < 3; i++) { arayTmp[i + 11] = byte.Parse(strTmp[3 + i].ToString()); }
                arayTmp[14] = 6000 / 256;
                arayTmp[15] = 6000 % 256;
                arayTmp[16] = bytDMXChnID;
                arayTmp[17] = byte.Parse((intDMXPort / 256).ToString());
                arayTmp[18] = byte.Parse((intDMXPort % 256).ToString());

                CsConst.mySends.AddBufToSndList(arayTmp, 0xF039, bytSubID, bytDevID, false, true, false, false);
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(2);
            }

            #region
            if (!DMXDeviceTypeList.HDLDMXDeviceTypeList.Contains(intDeviceType))
            {
                arayTmp = new byte[515];
                arayTmp[0] = 2;
                arayTmp[1] = 3;
                arayTmp[2] = 0;

                byte[] arayTmpH = new byte[515];
                arayTmpH[0] = 2;
                arayTmpH[1] = 3;
                arayTmpH[2] = 1;

                byte[] arayTmpM = new byte[514];
                arayTmpM[0] = 2;
                arayTmpM[1] = 3; 

                if (LoadTypes != null)
                {
                    foreach (DMX.Chns Tmp in LoadTypes)
                    {
                        for (int i = 0; i < CsConst.myLEDs[Tmp.intLibraryID].intSumChns; i++)
                        {
                            arayTmp[3 + Tmp.intStart - 1 + i] = (byte)Tmp.MinValue;
                            arayTmpH[3 + Tmp.intStart - 1 + i] = (byte)Tmp.MaxValue;
                            arayTmpM[2 + Tmp.intStart - 1 + i] = (byte)Tmp.MaxLevel;
                        }
                    }
                }
                CsConst.mySends.AddBufToSndList(arayTmp, 0x110A, bytSubID, bytDevID, (intDeviceType != 32), true, false, false);
                HDLUDP.TimeBetwnNext(20);

                CsConst.mySends.AddBufToSndList(arayTmpH, 0x110A, bytSubID, bytDevID, (intDeviceType != 32), true, false, false);
                HDLUDP.TimeBetwnNext(20);

                CsConst.mySends.AddBufToSndList(arayTmpM, 0x110E, bytSubID, bytDevID, (intDeviceType != 32), true, false, false);
                HDLUDP.TimeBetwnNext(20);

            }
            #endregion

            #region
            if (DMXDeviceTypeList.DMXHasPowerSavingFunction.Contains(intDeviceType)) // 是不是节能功能
            {
                if (PowerSaving == null) PowerSaving = new Byte[10];
                if (CsConst.mySends.AddBufToSndList(PowerSaving, 0xE13A, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(intDeviceType)) == true)
                {
                    CsConst.myRevBuf = new byte[1200];
                    HDLUDP.TimeBetwnNext(10);
                }
                else return;
            }
            #endregion

            //修改区域信息
            arayTmp = null;
            byte bytSumAreas = 0;
            int intTotal = 0;
            int intCMD = 0;
            if (!DMXDeviceTypeList.HDLDMXDeviceTypeList.Contains(intDeviceType)) // 如果不是48DMX表演控制器
            {
                arayTmp = new byte[512];
                arayTmp[0] = 2;
                arayTmp[1] = 3;
                bytSumAreas = 2;
                intCMD = 0x1102;
            }
            else
            {
                arayTmp = new byte[51];
                arayTmp[0] = bytSubID;
                arayTmp[1] = bytDevID;
                bytSumAreas = 2;
                intCMD = 0x0006;
            }
            // 修改备注，同时查找分区表
            if ((LoadTypes != null) && (LoadTypes.Count != 0))
            {  
                Byte bTemplateId = 1;
                foreach (DMX.Chns Tmp in LoadTypes)
                {
                    //修改逻辑灯备注
                    #region
                    if (DMXDeviceTypeList.HDLDMXDeviceTypeList.Contains(intDeviceType)) //不是512表演控制器
                    {
                        Byte[] AraySend = new Byte[21]; // 初始化数组
                        strRemark = Tmp.strRemark;
                        Byte[] ArayRemark = HDLUDP.StringToByte(strRemark);

                        AraySend[0] = Byte.Parse(Tmp.intStart.ToString());
                        Array.Copy(ArayRemark, 0, AraySend, 1, ArayRemark.Length);
                        CsConst.mySends.AddBufToSndList(AraySend, 0xF010, bytSubID, bytDevID, false, true, false, false);
                    }
                    else
                    {
                        Byte[] AraySend = new Byte[22]; // 初始化数组
                        strRemark = Tmp.strRemark;
                        byte[] ArayRemark = HDLUDP.StringToByte(strRemark);

                        AraySend[0] = byte.Parse((Tmp.intStart / 256).ToString());
                        AraySend[1] = byte.Parse((Tmp.intStart % 256).ToString());
                        Array.Copy(ArayRemark, 0, AraySend, 2, ArayRemark.Length);
                        CsConst.mySends.AddBufToSndList(AraySend, 0xF010, bytSubID, bytDevID, false, true, false, false);
                    }
                    #endregion

                    //修改逻辑灯素材库
                    #region
                    if (Tmp.arrLibraryParams != null)
                    {
                        Byte bLibraryId = 1;
                        foreach (Byte[] arrTmp in Tmp.arrLibraryParams)
                        {
                            Byte[] arrTmpSend = new Byte[arrTmp.Length + 3];
                            arrTmpSend[0] = bTemplateId;
                            arrTmpSend[1] = bLibraryId;
                            arrTmpSend[2] = (Byte)Tmp.intLibraryID;
                            Array.Copy(arrTmp, 0, arrTmpSend, 3,arrTmp.Length);
                            CsConst.mySends.AddBufToSndList(arrTmpSend, 0x3049, bytSubID, bytDevID, !DMXDeviceTypeList.HDLDMXDeviceTypeList.Contains(intDeviceType), true, false, false);
                            bLibraryId++;
                        }
                    }
                    #endregion
                    bTemplateId++;
                    if (Tmp.intBelongs != 0)
                    {
                        intTotal = (intTotal > Tmp.intBelongs) ? intTotal : Tmp.intBelongs;

                        arayTmp[bytSumAreas] = Byte.Parse(intTotal.ToString());
                        for (int i = 0; i < CsConst.myLEDs[Tmp.intLibraryID].intSumChns; i++)
                        {
                            arayTmp[bytSumAreas + Tmp.intStart + i] = byte.Parse(Tmp.intBelongs.ToString());
                        }
                    }
                }
            }

            CsConst.mySends.AddBufToSndList(arayTmp, intCMD, bytSubID, bytDevID, !DMXDeviceTypeList.HDLDMXDeviceTypeList.Contains(intDeviceType), true, false, false);
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(3);
            MyRead2UpFlags[4] = true;
            MyRead2UpFlags[5] = true;
            if ((Areas512 != null) || (Areas512.Count != 0))
            {
                byte intAreaID = 1;
                foreach (DMX.Areas oArea in Areas512)
                {
                    byte[] AraySend = new byte[21]; // 初始化数组
                    strRemark = oArea.strRemark;
                    byte[] ArayRemark = HDLUDP.StringToByte(strRemark);

                    AraySend[0] = intAreaID;
                    if (ArayRemark.Length <= 20)
                    {
                        Array.Copy(ArayRemark, 0, AraySend, 1, ArayRemark.Length);
                    }
                    else
                    {
                        Array.Copy(ArayRemark, 0, AraySend, 1, 20);
                    }
                    CsConst.mySends.AddBufToSndList(AraySend, 0xF00C, bytSubID, bytDevID, false, true, false, false);

                    //修改场景
                    #region

                    if ((oArea.MySces != null) && (oArea.MySces.Count != 0))
                    {
                        int bytSceID = 1;
                        foreach (DMX.SceType oSce in oArea.MySces)
                        {
                            if (intDeviceType != 32)
                            {
                                AraySend = new byte[23]; // 初始化数组
                                strRemark = oSce.Remark;
                                ArayRemark = HDLUDP.StringToByte(strRemark);

                                AraySend[0] = intAreaID;
                                AraySend[1] = byte.Parse((bytSceID / 256).ToString());
                                AraySend[2] = byte.Parse((bytSceID % 256).ToString());
                                Array.Copy(ArayRemark, 0, AraySend, 3, ArayRemark.Length);
                                CsConst.mySends.AddBufToSndList(AraySend, 0xF026, bytSubID, bytDevID, false, true, false, false);

                                AraySend = ReadSceneLevelToBuffer(oSce, intAreaID, bytSceID);
                                CsConst.mySends.AddBufToSndList(AraySend, 0x111A, bytSubID, bytDevID, true, true, false, false);
                            }
                            else
                            {
                                byte[] arayRemark1 = new byte[22];
                                arayRemark1[0] = Convert.ToByte(oArea.AreaID);
                                arayRemark1[1] = Convert.ToByte( oSce.intSceID);
                                arayTmp = HDLUDP.StringToByte(oSce.Remark);
                                arayTmp.CopyTo(arayRemark1, 2);
                                CsConst.mySends.AddBufToSndList(arayRemark1, 0xF026, bytSubID, bytDevID, false, true, false, false);
                                HDLUDP.TimeBetwnNext(arayRemark1.Length);

                                // modify scene running time and lights
                                byte[] araySce = new byte[4 + 48];
                                araySce[0] = Convert.ToByte(oArea.AreaID);
                                araySce[1] = Convert.ToByte(oSce.intSceID);
                                araySce[2] = byte.Parse((oSce.intRunTime / 256).ToString());
                                araySce[3] = byte.Parse((oSce.intRunTime % 256).ToString());

                                int intTmp = 0;

                                foreach (Chns ch in LoadTypes)
                                {   //添加所在区域的亮度值
                                    if (ch.intBelongs == oArea.AreaID)
                                    {
                                        araySce[4 + intTmp] = oSce.ArayLevel[ch.intStart - 1];
                                        intTmp++;
                                    }
                                }
                                CsConst.mySends.AddBufToSndList(araySce, 0x0008, bytSubID, bytDevID, false, true, false, false);
                                HDLUDP.TimeBetwnNext(araySce.Length);
                            }
                            bytSceID++;

                            // AraySend = new byte[512]
                        }
                    }
                    #endregion
                    MyRead2UpFlags[6] = true;
                    // 修改序列
                    #region
                    if ((oArea.MySers != null) && (oArea.MySers.Count != 0))
                    {
                        byte bytSeriesID = 1;
                        foreach (DMX.SeriesType oSeries in oArea.MySers)
                        {  // 增加新的序列
                            byte[] AraySendSeries = new byte[2];
                            AraySendSeries[0] = intAreaID;
                            AraySendSeries[1] = bytSeriesID;
                            CsConst.mySends.AddBufToSndList(AraySendSeries, 0xF069, bytSubID, bytDevID, false, true, false, false);
                            HDLUDP.TimeBetwnNext(AraySendSeries.Length);
                            //增加备注

                            AraySendSeries = new byte[22];
                            AraySendSeries[0] = intAreaID;
                            AraySendSeries[1] = bytSeriesID;
                            ArayRemark = HDLUDP.StringToByte(oSeries.Remark);
                            Array.Copy(ArayRemark, 0, AraySendSeries, 2, ArayRemark.Length);
                            CsConst.mySends.AddBufToSndList(AraySendSeries, 0xF030, bytSubID, bytDevID, false, true, false, false);
                            HDLUDP.TimeBetwnNext(AraySendSeries.Length);
                            //修改序列设置
                            AraySendSeries = new byte[5];
                            AraySendSeries[0] = intAreaID;
                            AraySendSeries[1] = bytSeriesID;
                            AraySendSeries[2] = byte.Parse(oSeries.intRunMode.ToString());
                            AraySendSeries[3] = byte.Parse(oSeries.intStep.ToString());
                            AraySendSeries[4] = byte.Parse(oSeries.intTimes.ToString());
                            CsConst.mySends.AddBufToSndList(AraySendSeries, 0x0018, bytSubID, bytDevID, false, true, false, false);
                            HDLUDP.TimeBetwnNext(AraySendSeries.Length);

                            if (oSeries.intStep != 0)
                            {
                                for (int i = 0; i < oSeries.intStep; i++)
                                {
                                    byte[] AraySeries = new byte[6];
                                    AraySeries[0] = intAreaID;
                                    AraySeries[1] = bytSeriesID;
                                    AraySeries[2] = byte.Parse((i + 1).ToString());
                                    AraySeries[3] = oSeries.SceneIDs[i];
                                    AraySeries[4] = oSeries.RunTimeH[i];
                                    AraySeries[5] = oSeries.RunTimeL[i];
                                    CsConst.mySends.AddBufToSndList(AraySeries, 0x0016, bytSubID, bytDevID, false, true, false, false);
                                    HDLUDP.TimeBetwnNext(AraySendSeries.Length);
                                }

                                //#region
                                //for (int i = 0; i < oSeries.intStep; i++)
                                //{
                                //    byte[] AraySeries = new byte[7];
                                //    AraySeries[0] = intAreaID;
                                //    AraySeries[1] = bytSeriesID;
                                //    AraySeries[2] = byte.Parse((i + 1).ToString());
                                //    AraySeries[3] = byte.Parse((oSeries.SceneIDs[i] / 256).ToString());
                                //    AraySeries[4] = byte.Parse((oSeries.SceneIDs[i] % 256).ToString());
                                //    AraySeries[5] = oSeries.RunTimeH[i];
                                //    AraySeries[6] = oSeries.RunTimeL[i];
                                //    CsConst.mySends.AddBufToSndList(AraySeries, 0x0016, bytSubID, bytDevID, false, true, false, false);
                                //    HDLUDP.TimeBetwnNext(AraySendSeries.Length);
                                //}
                                //#endregion
                            }
                        }
                    }
                    #endregion
                    MyRead2UpFlags[7] = true;
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(4 + intAreaID * 10);
                    intAreaID++;
                }
            }
        }

        private byte[] ReadSceneLevelToBuffer(DMX.SceType oSce, byte bytAreaID, int intSceID)
        {
            if (oSce == null) return null;
            int intTotalChns = 0;

            byte[] ArayTmp = new byte[512];

            foreach (DMX.Chns oChn in LoadTypes)
            {
                if (oChn.intBelongs == bytAreaID)
                {
                    Array.Copy(oSce.ArayLevel, oChn.intStart - 1, ArayTmp, intTotalChns, CsConst.myLEDs[oChn.intLibraryID].intSumChns);
                    intTotalChns += CsConst.myLEDs[oChn.intLibraryID].intSumChns;
                }
            }
            intTotalChns += 3;
            byte[] ArayResult = new byte[intTotalChns + 4];
            ArayResult[0] = byte.Parse(((intTotalChns + 2) / 256).ToString());
            ArayResult[1] = byte.Parse(((intTotalChns + 2) % 256).ToString());
            ArayResult[2] = bytAreaID;
            ArayResult[3] = byte.Parse((intSceID / 256).ToString());
            ArayResult[4] = byte.Parse((intSceID % 256).ToString());
            ArayResult[5] = byte.Parse((oSce.intRunTime / 256).ToString());
            ArayResult[6] = byte.Parse((oSce.intRunTime % 256).ToString());   

            Array.Copy(ArayTmp, 0, ArayResult, 7, intTotalChns - 3);
            return ArayResult;
        }

        /// <summary>
        /// 从设备读取当前的所有设置信息
        /// </summary>
        /// <param name="DeviceType"></param>
        /// <param name="DevName"></param>
        public bool DownloadDimmerInfosToBuffer(string DevName,int DeviceType)
        {
            string strMainRemark = DevName.Split('\\')[1].Trim();
            DevName = DevName.Split('\\')[0].Trim();

            byte bytSubID = byte.Parse(DevName.Split('-')[0].ToString());
            byte bytDevID = byte.Parse(DevName.Split('-')[1].ToString());

            CsConst.calculationWorker.ReportProgress(0);
            byte[] ArayTmp = null;
            Remark = HDLSysPF.ReadDeviceMainRemark(bytSubID, bytDevID);

            // read network paramters 
            #region
            if (CsConst.mintDevicesHasIPNetworkDevType.Contains(DeviceType))
            {
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xF037, bytSubID, bytDevID, false, true, false, false) == true)
                {
                    strIP = CsConst.myRevBuf[25].ToString("d3") + "." + CsConst.myRevBuf[26].ToString("d3")
                          + "." + CsConst.myRevBuf[27].ToString("d3") + "." + CsConst.myRevBuf[28].ToString("d3");
                    strRouterIP = CsConst.myRevBuf[29].ToString("d3") + "." + CsConst.myRevBuf[30].ToString("d3")
                          + "." + CsConst.myRevBuf[31].ToString("d3") + "." + CsConst.myRevBuf[32].ToString("d3");
                    strMAC = "H.D.L" + "." + CsConst.myRevBuf[36].ToString("d3") + "." + CsConst.myRevBuf[37].ToString("d3") + "." + CsConst.myRevBuf[38].ToString("d3");
                    intDMXPort = CsConst.myRevBuf[40] * 256 + CsConst.myRevBuf[41];
                    bytDMXChnID = CsConst.myRevBuf[39];
                    CsConst.myRevBuf = new byte[1200];
                }
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(1);
            }
            #endregion
            MyRead2UpFlags[0] = true;

            int wdMaxValue = 0;
            Byte[] arrTmpLogicIds = new Byte[1];
            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x3017, bytSubID, bytDevID, false, true, false, false))
            {
                arrTmpLogicIds = new Byte[CsConst.myRevBuf[25]];
                wdMaxValue = CsConst.myRevBuf[26];
                Array.Copy(CsConst.myRevBuf, 27, arrTmpLogicIds, 0, CsConst.myRevBuf[25]);
            }

            if (DMXDeviceTypeList.DMXHasPowerSavingFunction.Contains(DeviceType)) // 是不是节能功能
            {
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xE138, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                {
                    Array.Copy(CsConst.myRevBuf, 25, PowerSaving,0, 10);
                    CsConst.myRevBuf = new byte[1200];
                    HDLUDP.TimeBetwnNext(10);
                }
                else return false;
            }
            // 读取回路信息
            LoadTypes = new List<Chns>();
            #region
            for (int i = 0; i < wdMaxValue; i++)
            {
                Chns ch = new Chns();
                int[] arrLogicLightStartIdAndType = GetLogicLightStartChannelIdAndItsType((Byte)(i + 1), arrTmpLogicIds);
                ch.intStart = arrLogicLightStartIdAndType[0];
                ch.intLibraryID = arrLogicLightStartIdAndType[1];
                ch.MinValue = 0;
                ch.MaxValue = 100;
                ch.MaxLevel = 100;
                ch.intBelongs = 1;
                ArayTmp =new Byte[]{ (Byte)(i + 1)};
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x301B, bytSubID, bytDevID, false, true, false, false) == true)
                {
                    if (CsConst.myRevBuf != null)
                    {
                        Byte[] arayRemark = new Byte[20];
                        Array.Copy(CsConst.myRevBuf,26,arayRemark,0,20);
                        ch.strRemark = HDLPF.Byte22String(arayRemark,true);
                    }
                    CsConst.myRevBuf = new byte[1200];
                    HDLUDP.TimeBetwnNext(10);
                }
                else return false;

                for (Byte j = 1; j <= 4; j++)
                {
                    ArayTmp = new Byte[] { (Byte)(i + 1),j };
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x3047, bytSubID, bytDevID, false, true, false, false) == true)
                    {
                        if (CsConst.myRevBuf != null)
                        {
                            Byte[] arrTmpLibrary = new Byte[25];
                            Array.Copy(CsConst.myRevBuf, 28, arrTmpLibrary, 0, 25);
                            ch.arrLibraryParams.Add(arrTmpLibrary);
                        }
                        CsConst.myRevBuf = new byte[1200];
                        HDLUDP.TimeBetwnNext(10);
                    }
                }

                LoadTypes.Add(ch);
                CsConst.calculationWorker.ReportProgress(i * (50 / wdMaxValue), null);
            }
            ArayTmp = null;

            // read low limit 48 IPDMX
            #region
            if (DMXDeviceTypeList.HDLDMXDeviceTypeList.Contains(DeviceType)) //  DMX48
            {
                ArayTmp = new byte[1];
                ArayTmp[0] = 0;
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xF016, bytSubID, bytDevID, false, true, false, false) == true)
                {
                    for (int intI = 0; intI < wdMaxValue; intI++)
                    {
                        LoadTypes[intI].MinValue = CsConst.myRevBuf[26 + intI];
                    }
                    CsConst.myRevBuf = new byte[1200];
                    HDLUDP.TimeBetwnNext(50);
                }

                // read high limit
                ArayTmp[0] = 1;
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xF016, bytSubID, bytDevID, false, true, false, false) == true)
                {
                    for (int intI = 0; intI < wdMaxValue; intI++)
                    {
                        LoadTypes[intI].MaxValue = CsConst.myRevBuf[26 + intI];
                    }
                    CsConst.myRevBuf = new byte[1200];
                    HDLUDP.TimeBetwnNext(50);
                }

                // read High Level
                ArayTmp = null;
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xF020, bytSubID, bytDevID, false, true, false, false) == true)
                {
                    for (int intI = 0; intI < wdMaxValue; intI++)
                    {
                        LoadTypes[intI].MaxLevel = CsConst.myRevBuf[25 + intI];
                    }
                    CsConst.myRevBuf = new byte[1200];
                    HDLUDP.TimeBetwnNext(10);
                }
            }
            else   // 512DMX 
            {
                #region
                ArayTmp = new byte[3];
                ArayTmp[0] = 0;
                ArayTmp[1] = 1;
                ArayTmp[2] = 0;

                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1108, bytSubID, bytDevID, true, true, false, false) == true)
                {
                    for (int intI = 0; intI < wdMaxValue; intI++)
                    {
                        LoadTypes[intI].MinValue = CsConst.myRevBuf[28 + intI];
                    }
                    CsConst.myRevBuf = new byte[1200];
                    HDLUDP.TimeBetwnNext(50);
                }

                // read high limit
                ArayTmp[2] = 1;
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1108, bytSubID, bytDevID, true, true, false, false) == true)
                {
                    for (int intI = 0; intI < wdMaxValue; intI++)
                    {
                        LoadTypes[intI].MaxValue = CsConst.myRevBuf[28 + intI];
                    }
                    CsConst.myRevBuf = new byte[1200];
                    HDLUDP.TimeBetwnNext(50);
                }

                // read High Level
                ArayTmp = new byte[2];
                ArayTmp[0] = 0;
                ArayTmp[1] = 0;
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x110C, bytSubID, bytDevID, true, true, false, false) == true)
                {
                    for (int intI = 0; intI < wdMaxValue; intI++)
                    {
                        LoadTypes[intI].MaxLevel = CsConst.myRevBuf[27 + intI];
                    }
                    CsConst.myRevBuf = new byte[1200];
                    HDLUDP.TimeBetwnNext(10);
                }
                //渐变属性
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1110, bytSubID, bytDevID, true, true, false, false) == true)
                {
                    for (int intI = 0; intI < wdMaxValue; intI++)
                    {
                        LoadTypes[intI].ArrtiChang = CsConst.myRevBuf[27 + intI];
                    }
                    CsConst.myRevBuf = new byte[1200];
                    HDLUDP.TimeBetwnNext(10);
                }
                #endregion
            }
            #endregion
            #endregion
            MyRead2UpFlags[1] = true;
            // 读取区域信息
            Areas512 = new List<Areas>();

            // read area setup
            #region
            byte bytTalArea = 0;
            if (DMXDeviceTypeList.HDLDMXDeviceTypeList.Contains(DeviceType)) // 48 IPDMX
            {
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x0004, bytSubID, bytDevID, false, true, false, false) == true)
                {
                    wdMaxValue = CsConst.myRevBuf[28];
                    bytTalArea = CsConst.myRevBuf[27];
                    Byte[] arrTmpChn = new Byte[wdMaxValue];
                    Array.Copy(CsConst.myRevBuf, 29, arrTmpChn, 0, wdMaxValue);
                    GetAreaIdAndItsType(arrTmpChn);
                    CsConst.myRevBuf = new byte[1200];
                }
            }
            else
            {
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1100, bytSubID, bytDevID, false, true, false, false) == true)
                {
                    bytTalArea = CsConst.myRevBuf[31];
                    for (int intI = 0; intI < wdMaxValue; intI++)
                    {
                        LoadTypes[intI].intBelongs = CsConst.myRevBuf[32 + intI];
                    }
                    CsConst.myRevBuf = new byte[1200];
                }
            }
            MyRead2UpFlags[1] = true;
            for (byte intI = 0; intI < bytTalArea; intI++)
            {
                Areas area = new Areas();
                area.AreaID = (byte)(intI + 1);
                ArayTmp = new byte[1];
                ArayTmp[0] = (byte)(intI + 1);

                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xF00A, bytSubID, bytDevID, false, true, false, false) == true)
                {
                    byte[] arayRemark = new byte[20];
                    for (int intJ = 0; intJ < 20; intJ++) { arayRemark[intJ] = CsConst.myRevBuf[26 + intJ]; }
                    area.strRemark = HDLPF.Byte2String(arayRemark);
                }
                CsConst.myRevBuf = new byte[1200];
                // 读取场景信息
                area.MySces = new List<SceType>();
                #region
                int intAllSces = 32;
                if (!DMXDeviceTypeList.HDLDMXDeviceTypeList.Contains(DeviceType)) intAllSces = 100;
                for (int intJ = 0; intJ < intAllSces; intJ++)
                {
                    SceType scen = new SceType();
                    scen.intSceID = intJ + 1;

                    byte bytTmp = 0;
                    if (intAllSces > 255)
                    {
                        bytTmp = 1;
                        ArayTmp = new byte[3];
                        ArayTmp[0] = (byte)(intI + 1);
                        ArayTmp[1] = (byte)((intJ + 1) / 256);
                        ArayTmp[2] = (byte)((intJ + 1) % 256);
                    }
                    else
                    {
                        ArayTmp = new byte[2];
                        ArayTmp[0] = (byte)(intI + 1);
                        ArayTmp[1] = (byte)(intJ + 1);
                    }

                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xF024, bytSubID, bytDevID, false, true, false, false) == true)
                    {
                        byte[] arayRemark = new byte[20];
                        for (int intK = 0; intK < 20; intK++) { arayRemark[intK] = CsConst.myRevBuf[27 + bytTmp + intK]; }
                        scen.Remark = HDLPF.Byte2String(arayRemark);
                    }
                    CsConst.myRevBuf = new byte[1200];

                    if (!DMXDeviceTypeList.HDLDMXDeviceTypeList.Contains(DeviceType))
                    {
                        ArayTmp = new byte[] {0,3,(byte)(intI + 1),(byte)((intJ + 1) / 256),(byte)((intJ + 1) % 256)};

                        if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1118, bytSubID, bytDevID, true, true, false, false) == true)
                        {
                            scen.intRunTime = CsConst.myRevBuf[30] * 256 + CsConst.myRevBuf[31];
                            byte[] ArayByt = new byte[wdMaxValue];
                            for (int intK = 0; intK < wdMaxValue; intK++) { ArayByt[intK] = CsConst.myRevBuf[32 + intK]; }
                            scen.ArayLevel = ArayByt;

                            CsConst.myRevBuf = new byte[1200];
                        }
                    }
                    else
                    {
                        if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x0000, bytSubID, bytDevID, false, true, false, false) == true)
                        {
                            scen.intRunTime = CsConst.myRevBuf[27] * 256 + CsConst.myRevBuf[28];
                            byte[] ArayByt = new byte[wdMaxValue];
                            for (int intK = 0; intK < wdMaxValue; intK++) { ArayByt[intK] = CsConst.myRevBuf[29 + intK]; }
                            scen.ArayLevel = ArayByt;

                            CsConst.myRevBuf = new byte[1200];
                        }
                        
                    }
                    area.MySces.Add(scen);
                }
                #endregion
                MyRead2UpFlags[2] = true;
                //读取序列信息
                area.MySers = new List<SeriesType>();
                #region
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xF067, bytSubID, bytDevID, false, true, false, false) == true)
                {
                    byte bytTotalSeq = CsConst.myRevBuf[26];
                    CsConst.myRevBuf = new byte[1200];

                    for (byte bytI = 0; bytI < bytTotalSeq; bytI++)
                    {
                        SeriesType oSeq = new SeriesType();
                        oSeq.SeriID = (byte)(bytI + 1);

                        ArayTmp = new byte[2];
                        ArayTmp[0] = (byte)(intI + 1);
                        ArayTmp[1] = (byte)(bytI + 1);

                        if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xF028, bytSubID, bytDevID, false, true, false, false) == true)
                        {
                            byte[] arayRemark = new byte[20];
                            Array.Copy(CsConst.myRevBuf, 27, arayRemark, 0, 20);
                            oSeq.Remark = HDLPF.Byte2String(arayRemark);
                            CsConst.myRevBuf = new byte[1200];
                        }
                       
                        if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x0012, bytSubID, bytDevID, false, true, false, false) == true)
                        {
                            oSeq.intRunMode = CsConst.myRevBuf[27];
                            oSeq.intStep = CsConst.myRevBuf[28];
                            oSeq.intTimes = CsConst.myRevBuf[29];
                            oSeq.SceneIDs = new List<byte>();
                            oSeq.RunTimeH = new List<byte>();
                            oSeq.RunTimeL = new List<byte>();

                            CsConst.myRevBuf = new byte[1200];

                            for (int intJ = 0; intJ < oSeq.intStep; intJ++)
                            {
                                ArayTmp = new byte[3];
                                ArayTmp[0] = (byte)(intI + 1);
                                ArayTmp[1] = (byte)(bytI + 1);
                                ArayTmp[2] = (byte)(intJ + 1);

                                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x0014, bytSubID, bytDevID, false, true, false, false) == true)
                                {
                                    oSeq.SceneIDs.Add(CsConst.myRevBuf[28]);
                                    oSeq.RunTimeH.Add(CsConst.myRevBuf[29]);
                                    oSeq.RunTimeL.Add(CsConst.myRevBuf[30]);
                                    CsConst.myRevBuf = new byte[1200];
                                }
                            }
                        }
                        CsConst.myRevBuf = new byte[1200];
                        area.MySers.Add(oSeq);
                    }
                }
                #endregion
                MyRead2UpFlags[3] = true;
                Areas512.Add(area);

                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress( 50 + intI * (50 / wdMaxValue), null);
            }
            #endregion

            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(100);
            return true;
        }

        public int[] GetLogicLightStartChannelIdAndItsType(Byte bLogicId, Byte[] arrLogicTables)
        {
            int[] arrResult = new int[] { -1, -1 };
            Boolean bIsFoundLogicLight = false;
            try
            {
                for (int j = 0; j < arrLogicTables.Length; j++)
                {
                    if (arrLogicTables[j] == bLogicId)
                    {
                        if (bIsFoundLogicLight == false)
                        {
                            arrResult[0] = j + 1;
                            bIsFoundLogicLight = true;
                            arrResult[1] = 0;
                        }
                        else
                        {
                            arrResult[1] += 1;
                        }
                    }
                }
            }
            catch
            {
                return arrResult;
            }
            return arrResult;
        }

        public void GetAreaIdAndItsType(Byte[] arAreaAssignTables)
        {
            try
            {
                for (int i = 0; i < LoadTypes.Count; i++)
                {
                    int iStartChannelId = LoadTypes[i].intStart;
                    LoadTypes[i].intBelongs = arAreaAssignTables[iStartChannelId - 1];
                }
            }
            catch
            {
            }
        }
    }
}
