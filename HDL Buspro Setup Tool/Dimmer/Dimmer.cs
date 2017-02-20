using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.Xml;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace HDL_Buspro_Setup_Tool
{
    [Serializable]
    public class Dimmer : HdlDeviceBackupAndRestore 
    {        
        public int DIndex;  ////工程内唯一编号
        public string DeviceName;

        public Byte BroadcastChannelStatus;
        public Byte LoadType;
        public Byte DimmingMode;
        public bool isOutput;//场景现场输出
        public bool isModifyScenesSyn;//同步修改场景
        internal List<Channel> Chans;
        internal List<Area> Areas;

        public bool[] MyRead2UpFlags = new bool[8]; //前面四个表示读取 后面表示上传

        [Serializable]
        internal class Channel
        {
            public int ID;
            public string Remark;
            public int LoadType;
            public int MinValue;
            public int MaxValue;
            public int MaxLevel;      // DALI 掉电前亮度
            public int intBelongs;
            public Byte dimmingProfile; 
            public Byte outPutType;    // DALI 上电亮度
            public Byte DimmingMode;
            public Byte levelIfNoPower; // DALI应急灯
            public Byte levelWhenPowerOn; // DALi上电亮度
        }

        [Serializable]
        internal class Area
        {
            public byte ID;
            public string Remark;
            public List<Scene> Scen;
            public List<Sequence> Seq;
            public byte bytDefaultSce = 0;  // 上电恢复 FF 表示掉电前场景
        }

        [Serializable]
        internal class Scene
        {
            public byte ID;
            public string Remark;
            public int Time;
            public List<byte> light;
        }

        [Serializable]
        internal class Sequence
        {
            public byte ID;
            public string Remark;
            public byte Mode;
            public byte Times;  ////running times
            public byte Steps;  //// how many steps

            public List<byte> SceneIDs;
            public List<byte> RunTimeH;
            public List<byte> RunTimeL;

            public object Clone()
            {
                Sequence series = (Sequence)this.MemberwiseClone(); // 浅复制
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
        }

        ////<summary>
        ////读取默认的调光模块的设置，将所有数据读取缓存
        ////</summary>
        public void ReadDefaultInfo(int intDeviceType)
        {
            ////set all channel to default 0,未定义，0.0 ，0.0
            int wdMaxValue = DeviceTypeList.GetMaxValueFromPublicModeGroup(intDeviceType);

            Chans = new List<Channel>();
            Areas = new List<Area>();

            Area area = new Area();
            area.ID = 1;
            area.Remark = "Area 1";

            for (int i = 0; i < wdMaxValue; i++)
            {
                Channel ch = new Channel();
                ch.ID = i + 1;
                ch.Remark = "Chn " + (i + 1).ToString();
                ch.LoadType = 0;
                ch.MinValue = 0;
                ch.MaxValue = 100;
                ch.MaxLevel = 100;
                ch.intBelongs = 1;
                Chans.Add(ch);
            }

            // wdMaxvalue + 1 scenes open lights one by one and all at last
            area.Scen = new List<Scene>();//default scene setups
            #region
            for (int j = 0; j <= 12; j++)
            {
                Scene sc = new Scene();
                sc.ID = byte.Parse(j.ToString());
                sc.Remark = "Scene " + j.ToString();
                sc.Time = 0;
                sc.light = new List<byte>();

                if (CsConst.MyDimmerTemplate == 0)
                {
                    #region
                    for (int k = 0; k < wdMaxValue; k++)
                    {
                        if (j == 0) { sc.light.Add(0); }
                        else
                        {
                            if (j == wdMaxValue + 1) sc.light.Add(100);
                            else
                            {
                                if (k == j - 1) sc.light.Add(100);
                                else sc.light.Add(0);
                            }
                        }
                    }
                    #endregion
                }
                else if (CsConst.MyDimmerTemplate == 1) // norway database
                {
                    for (int k = 0; k < wdMaxValue; k++)
                    {
                        switch (j)
                        {
                            case 0: sc.Remark = "Off"; sc.light.Add(0); break;
                            case 1: sc.Remark = "Minimum"; sc.light.Add(25); break;
                            case 2: sc.Remark = "Medium"; sc.light.Add(50); break;
                            case 3: sc.Remark = "Maximum"; sc.light.Add(100); break;
                            case 4:
                            case 5:
                            case 6:
                            case 7: sc.Remark = ""; sc.light.Add(0); break;
                            case 8: sc.Remark = "Normal"; sc.light.Add(100); break;
                            case 9: sc.Remark = "Night"; sc.light.Add(0); break;
                            case 10: sc.Remark = "Away"; sc.light.Add(0); break;
                            case 11: sc.Remark = "Vacation"; sc.light.Add(0); break;
                            case 12: sc.Remark = "Powerfailure"; sc.light.Add(0); break;
                        }
                    }
                }
                area.Scen.Add(sc);
            }
            #endregion
            // add sequence for all total 1
            area.Seq = new List<Sequence>();
            Sequence se = new Sequence();
            se.ID = 1;
            se.Mode = 2;
            se.Remark = "Sequence 1";
            se.Steps = Convert.ToByte(wdMaxValue.ToString(),10);
            se.Times = 1;
            se.SceneIDs = new List<byte>();
            se.RunTimeH = new List<byte>();
            se.RunTimeL = new List<byte>();

            for (int i = 0; i < wdMaxValue; i++) 
            { 
                se.SceneIDs.Add(Convert.ToByte((i+1).ToString(),10));
                se.RunTimeH.Add(0);
                se.RunTimeL.Add(1);
            };
            
            area.Seq.Add(se);
            Areas.Add(area);
        }


        //<summary>
        //读取数据库里的调光模块的设置，将所有数据读取缓存
        //</summary>
        public void  ReadInfoForDB(int id)
        {
            try
            {
                #region
                string strsql = string.Format("select * from dbClassInfomation where DIndex={0} and ClassID={1} order by SenNum", DIndex, 0);
                OleDbDataReader dr = DataModule.SearchAResultSQLDB(strsql, CsConst.mstrCurPath);
                Chans = new List<Channel>();
                if (dr != null)
                {
                    while (dr.Read())
                    {
                        Channel temp = new Channel();
                        string str = dr.GetValue(5).ToString();
                        temp.ID = Convert.ToByte(str.Split('-')[0].ToString());
                        LoadType = Convert.ToByte(str.Split('-')[1].ToString());
                        temp.MinValue = Convert.ToByte(str.Split('-')[2].ToString());
                        temp.MaxValue = Convert.ToByte(str.Split('-')[3].ToString());
                        temp.MaxLevel = Convert.ToByte(str.Split('-')[4].ToString());
                        temp.intBelongs = Convert.ToByte(str.Split('-')[5].ToString());
                        temp.dimmingProfile = Convert.ToByte(str.Split('-')[6].ToString());
                        temp.outPutType = Convert.ToByte(str.Split('-')[7].ToString());
                        DimmingMode = Convert.ToByte(str.Split('-')[8].ToString());
                        temp.Remark = dr.GetValue(4).ToString();
                        Chans.Add(temp);
                    }
                    dr.Close();
                }
                #endregion

                #region
                strsql = string.Format("select * from dbClassInfomation where DIndex={0} and ClassID={1} order by SenNum", DIndex, 1);
                dr = DataModule.SearchAResultSQLDB(strsql, CsConst.mstrCurPath);
                Areas = new List<Area>();
                if (dr != null)
                {
                    while (dr.Read())
                    {
                        Area temp = new Area();
                        string str = dr.GetValue(5).ToString();
                        temp.ID = Convert.ToByte(str.Split('-')[0].ToString());
                        temp.bytDefaultSce = Convert.ToByte(str.Split('-')[1].ToString());
                        temp.Remark = dr.GetValue(4).ToString();
                        temp.Scen = new List<Scene>();
                        temp.Seq = new List<Sequence>();
                        string str1 = string.Format("select * from dbClassInfomation where DIndex = {0} and ClassID = {1} and ObjectID = {2} order by SenNum", DIndex, 2, temp.ID);
                        OleDbDataReader dr1 = DataModule.SearchAResultSQLDB(str1, CsConst.mstrCurPath);
                        if (dr1 != null)
                        {
                            while (dr1.Read())
                            {
                                Scene temp1 = new Scene();
                                string strTmp1 = dr1.GetValue(5).ToString();
                                temp1.ID = Convert.ToByte(strTmp1.Split('-')[0].ToString());
                                temp1.Time = Convert.ToByte(strTmp1.Split('-')[1].ToString());
                                temp1.Remark = dr1.GetValue(4).ToString();
                                temp1.light = ((byte[])dr1[6]).ToList();
                                temp.Scen.Add(temp1);
                            }
                            dr1.Close();
                        }
                        string str2 = string.Format("select * from dbClassInfomation where DIndex = {0} and ClassID = {1} and ObjectID = {2} order by SenNum", DIndex, 3, temp.ID);
                        OleDbDataReader dr2 = DataModule.SearchAResultSQLDB(str2, CsConst.mstrCurPath);
                        if (dr2 != null)
                        {
                            while (dr2.Read())
                            {
                                Sequence temp2 = new Sequence();
                                string strTmp2 = dr2.GetValue(5).ToString();
                                temp2.ID = Convert.ToByte(strTmp2.Split('-')[0].ToString());
                                temp2.Mode = Convert.ToByte(strTmp2.Split('-')[1].ToString());
                                temp2.Times = Convert.ToByte(strTmp2.Split('-')[2].ToString());
                                temp2.Steps = Convert.ToByte(strTmp2.Split('-')[3].ToString());
                                temp2.Remark = dr2.GetValue(4).ToString();
                                temp2.SceneIDs = ((byte[])dr2[6]).ToList();
                                temp2.RunTimeH = ((byte[])dr2[7]).ToList();
                                temp2.RunTimeL = ((byte[])dr2[8]).ToList();
                                temp.Seq.Add(temp2);
                            }
                            dr2.Close();
                        }
                        Areas.Add(temp);
                    }
                    dr.Close();
                }
                #endregion

                #region
                strsql = string.Format("select * from dbClassInfomation where DIndex={0} and ClassID={1} order by SenNum", DIndex, 4);
                dr = DataModule.SearchAResultSQLDB(strsql, CsConst.mstrCurPath);
                if (dr != null)
                {
                    while (dr.Read())
                    {
                        string str = dr.GetValue(5).ToString();
                        string strRemark = dr.GetValue(4).ToString();
                        BroadcastChannelStatus = Convert.ToByte(str.Split('-')[0].ToString());
                        LoadType = Convert.ToByte(str.Split('-')[1].ToString());
                        DimmingMode = Convert.ToByte(str.Split('-')[2].ToString());
                        DeviceName = DeviceName.Split('\\')[0].ToString() + "\\"
                                + strRemark;
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
        //将缓存里所有调光模块的设置存在数据库
        //</summary>
        public void SaveInfoToDb()
        {
            try
            {
                string strsql = string.Format("delete * from dbClassInfomation where DIndex={0}", DIndex);
                DataModule.ExecuteSQLDatabase(strsql);

                strsql = string.Format("delete * from dbKeyTargets where DIndex={0}", DIndex);
                DataModule.ExecuteSQLDatabase(strsql);

                #region
                if (Chans != null)
                {
                    for (int i = 0; i < Chans.Count; i++)
                    {
                        Channel tmpChn = Chans[i];
                        string strParam = tmpChn.ID.ToString() + "-" + tmpChn.LoadType.ToString() + "-"
                                        + tmpChn.MinValue.ToString() + "-" + tmpChn.MaxValue.ToString() + "-"
                                        + tmpChn.MaxLevel.ToString() + "-" + tmpChn.intBelongs.ToString() + "-"
                                        + tmpChn.dimmingProfile.ToString() + "-" + tmpChn.outPutType.ToString() + "-" 
                                        + tmpChn.DimmingMode.ToString();
                        strsql = string.Format("Insert into dbClassInfomation(DIndex,ClassID,ObjectID,SenNum,Remark,strParam1) values ({0},{1},{2},{3},'{4}','{5}')",
                                 DIndex, 0, 0, i, tmpChn.Remark, strParam);
                        DataModule.ExecuteSQLDatabase(strsql);
                        
                    }
                }
                #endregion

                #region
                if (Areas != null)
                {
                    for (int i = 0; i < Areas.Count; i++)
                    {
                        Area tmpArea = Areas[i];
                        string strParam = tmpArea.ID.ToString() + "-" + tmpArea.bytDefaultSce.ToString();
                        strsql = string.Format("Insert into dbClassInfomation(DIndex,ClassID,ObjectID,SenNum,Remark,strParam1) values ({0},{1},{2},{3},'{4}','{5}')",
                                 DIndex, 1, 0, i, tmpArea.Remark, strParam);
                        DataModule.ExecuteSQLDatabase(strsql);
                        if (tmpArea.Scen != null)
                        {
                            for (int j = 0; j < tmpArea.Scen.Count;j++ )
                            {
                                if (tmpArea.Scen[j].light == null) tmpArea.Scen[j].light = new List<byte>();
                                byte[] arayTmp = tmpArea.Scen[j].light.ToArray();
                                if (arayTmp == null) arayTmp = new byte[0];
                                string strRemark = tmpArea.Scen[j].Remark;
                                if (strRemark == null) strRemark = "";
                                strParam = tmpArea.Scen[j].ID.ToString() + "-" + tmpArea.Scen[j].Time.ToString();
                                strsql = @"Insert into dbClassInfomation(DIndex,ClassID,ObjectID,SenNum,Remark,strParam1,byteAry1)"
                                  + " values(@DIndex,@ClassID,@ObjectID,@SenNum,@Remark,@strParam1,@byteAry1)";
                                //创建一个OleDbConnection对象
                                OleDbConnection conn;
                                conn = new OleDbConnection(DataModule.ConString + CsConst.mstrDefaultPath);
                                //OleDbConnection conn = new OleDbConnection(DataModule.ConString + CsConst.mstrCurPath);
                                conn.Open();
                                OleDbCommand cmd = new OleDbCommand(strsql, conn);
                                ((OleDbParameter)cmd.Parameters.Add("@DIndex", OleDbType.Integer)).Value = DIndex;
                                ((OleDbParameter)cmd.Parameters.Add("@ClassID", OleDbType.Integer)).Value = 2;
                                ((OleDbParameter)cmd.Parameters.Add("@ObjectID", OleDbType.Integer)).Value = tmpArea.ID;
                                ((OleDbParameter)cmd.Parameters.Add("@SenNum", OleDbType.Integer)).Value = j;
                                ((OleDbParameter)cmd.Parameters.Add("@Remark", OleDbType.VarChar)).Value = strRemark;
                                ((OleDbParameter)cmd.Parameters.Add("@strParam1", OleDbType.VarChar)).Value = strParam;
                                ((OleDbParameter)cmd.Parameters.Add("@byteAry1", OleDbType.Binary)).Value = arayTmp;
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
                        if (tmpArea.Seq != null)
                        {
                            for (int j = 0; j < tmpArea.Seq.Count; j++)
                            {
                                if (tmpArea.Seq[j].SceneIDs == null) tmpArea.Seq[j].SceneIDs = new List<byte>();
                                if (tmpArea.Seq[j].RunTimeH == null) tmpArea.Seq[j].RunTimeH = new List<byte>();
                                if (tmpArea.Seq[j].RunTimeL == null) tmpArea.Seq[j].RunTimeL = new List<byte>();
                                byte[] arayTmp1 = tmpArea.Seq[j].SceneIDs.ToArray();
                                byte[] arayTmp2 = tmpArea.Seq[j].RunTimeH.ToArray();
                                byte[] arayTmp3 = tmpArea.Seq[j].RunTimeL.ToArray();
                                if (arayTmp1 == null) arayTmp1 = new byte[0];
                                string strRemark = tmpArea.Scen[j].Remark;
                                if (strRemark == null) strRemark = "";
                                strParam = tmpArea.Seq[j].ID.ToString() + "-" + tmpArea.Seq[j].Mode.ToString()
                                         + "-" + tmpArea.Seq[j].Times.ToString() + "-" + tmpArea.Seq[j].Steps.ToString();
                                strsql = @"Insert into dbClassInfomation(DIndex,ClassID,ObjectID,SenNum,Remark,strParam1,byteAry1,byteAry2,byteAry3)"
                                  + " values(@DIndex,@ClassID,@ObjectID,@SenNum,@Remark,@strParam1,@byteAry1,@byteAry2,@byteAry3)";
                                //创建一个OleDbConnection对象
                                OleDbConnection conn;
                                conn = new OleDbConnection(DataModule.ConString + CsConst.mstrDefaultPath);
                                //OleDbConnection conn = new OleDbConnection(DataModule.ConString + CsConst.mstrCurPath);
                                conn.Open();
                                OleDbCommand cmd = new OleDbCommand(strsql, conn);
                                ((OleDbParameter)cmd.Parameters.Add("@DIndex", OleDbType.Integer)).Value = DIndex;
                                ((OleDbParameter)cmd.Parameters.Add("@ClassID", OleDbType.Integer)).Value = 3;
                                ((OleDbParameter)cmd.Parameters.Add("@ObjectID", OleDbType.Integer)).Value = tmpArea.ID;
                                ((OleDbParameter)cmd.Parameters.Add("@SenNum", OleDbType.Integer)).Value = j;
                                ((OleDbParameter)cmd.Parameters.Add("@Remark", OleDbType.VarChar)).Value = strRemark;
                                ((OleDbParameter)cmd.Parameters.Add("@strParam1", OleDbType.VarChar)).Value = strParam;
                                ((OleDbParameter)cmd.Parameters.Add("@byteAry1", OleDbType.Binary)).Value = arayTmp1;
                                ((OleDbParameter)cmd.Parameters.Add("@byteAry2", OleDbType.Binary)).Value = arayTmp2;
                                ((OleDbParameter)cmd.Parameters.Add("@byteAry3", OleDbType.Binary)).Value = arayTmp3;
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
                    }
                }
                #endregion

                #region
                string strDeviceRemark = "";
                if (DeviceName.Contains("\\")) strDeviceRemark = DeviceName.Split('\\')[1].ToString();
                string strBasic = BroadcastChannelStatus.ToString() + "-" + LoadType.ToString()
                                + "-" + DimmingMode.ToString();
                strsql = string.Format("Insert into dbClassInfomation(DIndex,ClassID,ObjectID,SenNum,Remark,strParam1) values ({0},{1},{2},{3},'{4}','{5}')",
                         DIndex, 4, 0, 0, strDeviceRemark, strBasic);
                DataModule.ExecuteSQLDatabase(strsql);
                #endregion
            }
            catch
            {
            }
        }

        /// <summary>
        /// 将调光模块设备上传
        /// </summary>
        /// <param name="DIndex"></param>
        /// <param name="DevID"></param>
        /// <param name="DeviceType"></param>
        /// <param name="DevName"></param>
        /// <returns></returns>
        public bool UploadDimmerInfosToDevice(string DevName, int intActivePage,int DeviceType)// 0 mean all, else that tab only
        {
            string strMainRemark = DevName.Split('\\')[1].Trim().Split('(')[0].Trim();
            DevName = DevName.Split('\\')[0].Trim();
            //保存回路信息
            byte bytSubID = byte.Parse(DevName.Split('-')[0].ToString());
            byte bytDevID = byte.Parse(DevName.Split('-')[1].ToString());

            Boolean isHasDimmingProfileInEachChannel = DimmerDeviceTypeList.DimmerModuleV2DimCurve.Contains(DeviceType);

            int wdMaxValue = DeviceTypeList.GetMaxValueFromPublicModeGroup(DeviceType);

            byte[] ArayMain = new byte[20];

            HDLSysPF.ModifyDeviceMainRemark(bytSubID, bytDevID, strMainRemark, DeviceType);

            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy)
                CsConst.calculationWorker.ReportProgress(1);

            if (isHasDimmingProfileInEachChannel)
            {
                ArayMain = new byte[2];
                ArayMain[0] = BroadcastChannelStatus;
                ArayMain[1] = LoadType;
                CsConst.mySends.AddBufToSndList(ArayMain, 0x1808, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType));
                HDLUDP.TimeBetwnNext(10);
            }

            Byte[] arayRemark = new byte[21];// used for restore remark
            Byte[] arayLoadType = new byte[wdMaxValue];
            Byte[] arayLimitL = new byte[wdMaxValue + 1]; arayLimitL[0] = 0;
            Byte[] arayLimitH = new byte[wdMaxValue + 1]; arayLimitH[0] = 1;
            Byte[] arayMaxLevel = new byte[wdMaxValue];
            Byte[] arayDimmingProfile = new byte[wdMaxValue + 1];
            Byte[] arayLevelIfNoPower = new Byte[wdMaxValue + 1]; arayLevelIfNoPower[0] = 2;
            Byte[] arayLevelWhenPowerOn = new Byte[wdMaxValue + 1]; arayLevelWhenPowerOn[0] = 3;
            if (isHasDimmingProfileInEachChannel) arayDimmingProfile = new byte[wdMaxValue + 1 + wdMaxValue];
            if (isHasDimmingProfileInEachChannel || DeviceType == 44 || DeviceType == 45)
            {
                arayDimmingProfile[0] = DimmingMode;
            }

            if (intActivePage == 0 || intActivePage == 1)
            {
                #region
                foreach (Channel ch in Chans)
                {   // modify the chns remark
                    arayRemark = new byte[21]; // 初始化数组
                    string strRemark = ch.Remark;
                    if (ch.Remark == null) strRemark = "";
                    if (strRemark.Length > 20)
                        strRemark = strRemark.Substring(0, 20);
                    byte[] arayTmp = HDLUDP.StringToByte(strRemark);

                    arayRemark[0] = byte.Parse(ch.ID.ToString());
                    arayTmp.CopyTo(arayRemark, 1);

                    if (CsConst.mySends.AddBufToSndList(arayRemark, 0xF010, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == false) return false;
                    HDLUDP.TimeBetwnNext(arayRemark.Length);

                    if (DeviceType == 455)
                    {
                        byte[] ArayTmp = new byte[8];
                        ArayTmp[0] = 1;
                        ArayTmp[1] = 1;
                        String TmpVersion = HDLSysPF.ReadDeviceVersion(bytSubID, bytDevID,false);
                        String[] Tmp = TmpVersion.Split('_').ToArray();

                        if (string.Compare(Tmp[2], "2016") >= 0) ArayTmp[1] = 0;
                        
                        ArayTmp[2] = (byte)(ch.ID);
                        ArayTmp[3] = 2;
                        ArayTmp[4] = 1;
                        ArayTmp[5] = 4;
                        ArayTmp[6] = ch.dimmingProfile;
                        ArayTmp[7] = ch.outPutType;
                        if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xE466, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == false) return false;
                    }

                    arayLoadType[ch.ID - 1] = byte.Parse(ch.LoadType.ToString());
                    arayMaxLevel[ch.ID - 1] = byte.Parse(ch.MaxLevel.ToString());
                    arayLimitL[ch.ID] = byte.Parse(ch.MinValue.ToString());
                    arayLimitH[ch.ID] = byte.Parse(ch.MaxValue.ToString());
                    arayLevelIfNoPower[ch.ID] = ch.levelIfNoPower;
                    arayLevelWhenPowerOn[ch.ID] = ch.levelWhenPowerOn;
          
                    if (isHasDimmingProfileInEachChannel)
                    {
                        arayDimmingProfile[ch.ID] = ch.dimmingProfile;
                        arayDimmingProfile[ch.ID + wdMaxValue] = ch.DimmingMode;
                    }
                }

                // modify the on Low limit
                if (CsConst.mySends.AddBufToSndList(arayLimitL, 0xF018, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == false) return false;
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(3);

                // modify the High Limit
                if (CsConst.mySends.AddBufToSndList(arayLimitH, 0xF018, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == false) return false;
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(4);

                if (DimmerDeviceTypeList.DALIDimmerDeviceTypeLists.Contains(DeviceType)) //DALI设备
                {
                    // modify the level when power failure
                    if (CsConst.mySends.AddBufToSndList(arayLevelIfNoPower, 0xF018, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == false) return false;
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(6);

                    // modify the level when power on 
                    if (CsConst.mySends.AddBufToSndList(arayLevelWhenPowerOn, 0xF018, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == false) return false;
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(8);
                }
                else 
                {
                    // modify the max level 
                    if (CsConst.mySends.AddBufToSndList(arayMaxLevel, 0xF022, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == false) return false;
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(5);


                    // Dimming Profile
                    if (isHasDimmingProfileInEachChannel)
                    {
                        if (CsConst.mySends.AddBufToSndList(arayDimmingProfile, 0x1804, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == false) return false;
                    }
                        // modify the load type
                    if (CsConst.mySends.AddBufToSndList(arayLoadType, 0xF014, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == false) return false;
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(2);
                }
                #endregion
                MyRead2UpFlags[4] = true;
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(8);
            //// save area information
            if (intActivePage == 0 || intActivePage == 2)
            {
                #region
                byte bytI = 0;
                byte[] AreasARY = new byte[3 + wdMaxValue];
                AreasARY[0] = bytSubID;
                AreasARY[1] = bytDevID;
                AreasARY[2] = Convert.ToByte(Areas.Count);

                if (Areas == null || Areas.Count == 0)
                    AreasARY[2] = 1;
                else
                {
                    AreasARY[2] = (byte)Areas.Count;

                    foreach (Channel chn in Chans)
                    {
                        AreasARY[2 + chn.ID] = Convert.ToByte(chn.intBelongs);
                    }
                }

                if (CsConst.mySends.AddBufToSndList(AreasARY, 0x0006, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == false) return false;
                foreach (Area area in Areas)
                {
                    // modify area remark
                    arayRemark = new Byte[21]; // 初始化数组
                    string strRemark = area.Remark;
                    byte[] arayTmp = HDLUDP.StringToByte(strRemark);

                    arayRemark[0] = area.ID;
                    arayRemark = HDLSysPF.CopyRemarkBufferToSendBuffer(arayTmp, arayRemark, 1);

                    if (CsConst.mySends.AddBufToSndList(arayRemark, 0xF00C, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == false) return false;
                    HDLUDP.TimeBetwnNext(arayRemark.Length);

                }

                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(10 + bytI * 50 / wdMaxValue);

                #endregion
                MyRead2UpFlags[5] = true;
            }

           if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(16);
            // modify the scene one by one remark and other settings
            if (Areas == null) return true;

            int areaNum = 1;
            byte[] bytSceIDs = new byte[Areas.Count];
            byte[] bytSceFlag = new byte[Areas.Count];
            foreach (Area area in Areas)
            {    // scene
                if (area.bytDefaultSce != 13)
                {
                    bytSceFlag[area.ID - 1] = 1;
                    bytSceIDs[area.ID - 1] = area.bytDefaultSce;
                }
                if (intActivePage == 0 || intActivePage == 3)
                {
                    #region
                    if (area.Scen == null) return true;
                    foreach (Scene scen in area.Scen)
                    {
                        string strRemark = scen.Remark;
                        byte[] arayTmp = HDLUDP.StringToByte(strRemark);

                        byte[] arayRemark1 = new byte[22];
                        arayRemark1[0] = area.ID;
                        arayRemark1[1] = scen.ID;
                        if (arayTmp != null) arayTmp.CopyTo(arayRemark1, 2);
                        if (CsConst.mySends.AddBufToSndList(arayRemark1, 0xF026, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == false) return false;
                        HDLUDP.TimeBetwnNext(10);

                        // modify scene running time and lights
                        byte[] araySce = new byte[4 + wdMaxValue];
                        araySce[0] = area.ID;
                        araySce[1] = scen.ID;
                        araySce[2] = byte.Parse((scen.Time / 256).ToString());
                        araySce[3] = byte.Parse((scen.Time % 256).ToString());

                        int intTmp = 0;
                        foreach (Channel ch in Chans)
                        {   //添加所在区域的亮度值
                            if (ch.intBelongs == area.ID)
                            {
                                if (scen.light != null && scen.light.Count >= ch.ID)
                                {
                                    araySce[4 + intTmp] = scen.light[ch.ID - 1];
                                    intTmp++;
                                }
                            }
                        }
                        if (CsConst.mySends.AddBufToSndList(araySce, 0x0008, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == false) return false;
                        HDLUDP.TimeBetwnNext(50);

                        if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(45 +  area.ID);
                    }
                    #endregion
                    MyRead2UpFlags[5] = true;
                    MyRead2UpFlags[6] = true;
                }

                //序列
                if (intActivePage == 0 || intActivePage == 4)
                {
                    #region
                    if (area.Seq == null) return true;

                    if (DeviceType == 36 && areaNum > 1)
                        continue;
                    areaNum++;

                    foreach (Sequence seq in area.Seq)
                    {
                        string strRemark = seq.Remark;
                        byte[] arayTmp = HDLUDP.StringToByte(strRemark);

                        byte[] arayRemark1 = new byte[22];
                        arayRemark1[0] = area.ID;
                        arayRemark1[1] = seq.ID;
                        arayTmp.CopyTo(arayRemark1, 2);
                        if (CsConst.mySends.AddBufToSndList(arayRemark1, 0xF030, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == false) return false;
                        HDLUDP.TimeBetwnNext(10);

                        // modify seq mode and step
                        byte[] araySce = new byte[5];
                        araySce[0] = area.ID;
                        araySce[1] = seq.ID;
                        araySce[2] = seq.Mode;
                        araySce[3] = seq.Steps;
                        araySce[4] = seq.Times;
                        if (CsConst.mySends.AddBufToSndList(araySce, 0x0018, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == false) return false;
                        HDLUDP.TimeBetwnNext(4);

                        // modify every step 
                        
                        for (int i = 0; i < seq.Steps; i++)
                        {
                            byte[] arayStep = new byte[6];
                            arayStep[0] = area.ID;
                            arayStep[1] = seq.ID;
                            arayStep[2] = byte.Parse((i + 1).ToString());
                            arayStep[3] = seq.SceneIDs[i];
                            arayStep[4] = seq.RunTimeH[i];
                            arayStep[5] = seq.RunTimeL[i];

                            if (CsConst.mySends.AddBufToSndList(arayStep, 0x0016, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == false) return false;
                        }
                        if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(60 + area.ID);
                    }
                    #endregion
                    MyRead2UpFlags[5] = true;
                    MyRead2UpFlags[7] = true;
                }
            }
            if (intActivePage == 0 || intActivePage == 3)
            {
                if (CsConst.mySends.AddBufToSndList(bytSceFlag, 0xF053, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == false) return false;
                HDLUDP.TimeBetwnNext(10);
                if (CsConst.mySends.AddBufToSndList(bytSceIDs, 0xF057, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == false) return false;
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(100);
            return true;
        }

        /// <summary>
        /// 从设备读取当前的所有设置信息
        /// </summary>
        /// <param name="DeviceType"></param>
        /// <param name="DevName"></param>
        public bool DownloadDimmerInfosToBuffer(string DevName, int intActivePage,int DeviceType)// 0 mean all, else that tab only
        {
            string strMainRemark = DevName.Split('\\')[1].Trim();
            DevName = DevName.Split('\\')[0].Trim();

            byte bytSubID = byte.Parse(DevName.Split('-')[0].ToString());
            byte bytDevID = byte.Parse(DevName.Split('-')[1].ToString());

            byte[] ArayTmp = new byte[0];
            Boolean isHasDimmingProfileInEachChannel = DimmerDeviceTypeList.DimmerModuleV2DimCurve.Contains(DeviceType);
            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x000E, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == false)
            {
                return false;
            }
            else
            {
                byte[] arayRemark = new byte[20];
                Array.Copy(CsConst.myRevBuf, 25, arayRemark, 0, 20);
                DeviceName = bytSubID.ToString() + "-" + bytDevID.ToString() + "\\" + HDLPF.Byte2String(arayRemark);
                CsConst.myRevBuf = new byte[1200];
            }
            LoadType = 255;
            if (isHasDimmingProfileInEachChannel)
            {
                if (CsConst.mySends.AddBufToSndList(null, 0x1806, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                {
                    BroadcastChannelStatus = CsConst.myRevBuf[26];
                    if (CsConst.myRevBuf[16] <= 0x0D)
                        LoadType = 255;
                    else if (CsConst.myRevBuf[16] >= 0x0E)
                        LoadType = CsConst.myRevBuf[27];
                    CsConst.myRevBuf = new byte[1200];
                }
            }

            int wdMaxValue = DeviceTypeList.GetMaxValueFromPublicModeGroup(DeviceType);
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(5, null);
            
            // 读取回路信息
            if (intActivePage == 0 || intActivePage == 1)
            {
                Chans = new List<Channel>();
                #region

                for (int i = 0; i < wdMaxValue; i++)
                {
                    Channel ch = new Channel();
                    ch.ID = i + 1;
                    ch.LoadType = 0;
                    ch.MinValue = 0;
                    ch.MaxValue = 100;
                    ch.MaxLevel = 100;
                    ch.intBelongs = 1;

                    ArayTmp = new byte[1];
                    ArayTmp[0] = (byte)(i + 1);
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xF00E, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                    {
                        if (CsConst.myRevBuf != null)
                        {
                            byte[] arayRemark = new byte[20];
                            Array.Copy(CsConst.myRevBuf, 26, arayRemark, 0, 20);
                            ch.Remark = HDLPF.Byte2String(arayRemark);
                        }
                        CsConst.myRevBuf = new byte[1200];
                        HDLUDP.TimeBetwnNext(10);
                    }
                    else
                    {
                        return false;
                    }
                    if (DeviceType == 455)
                    {
                        ArayTmp = new byte[3];
                        ArayTmp[0] = 1;
                        ArayTmp[1] = 1;
                        ArayTmp[2] = (byte)(i + 1);
                        if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xE464, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                        {
                            if (CsConst.myRevBuf != null)
                            {
                                ch.dimmingProfile = CsConst.myRevBuf[31];
                                ch.outPutType = CsConst.myRevBuf[32];
                                if (ch.outPutType > 2) ch.outPutType = 0;
                            }
                        }
                        else return false;
                    }

                    Chans.Add(ch);
                   if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy)  CsConst.calculationWorker.ReportProgress(i * (25 / wdMaxValue)+5, null);
                }
                ArayTmp = null;

                // read low limit
                ArayTmp = new byte[1];
                ArayTmp[0] = 0;
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xF016, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                {
                    for (int intI = 0; intI < wdMaxValue; intI++)
                    {
                        Chans[intI].MinValue = CsConst.myRevBuf[26 + intI];
                        if (Chans[intI].MinValue > 100) Chans[intI].MinValue = 100;
                    }
                    CsConst.myRevBuf = new byte[1200];
                }
                else return false;

                // read high limit
                ArayTmp[0] = 1;
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xF016, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                {
                    for (int intI = 0; intI < wdMaxValue; intI++)
                    {
                        Chans[intI].MaxValue = CsConst.myRevBuf[26 + intI];
                        if (Chans[intI].MaxValue > 100) Chans[intI].MinValue = 100;
                    }
                    CsConst.myRevBuf = new byte[1200];
                }
                else return false;

                // read High Level
                if (!DimmerDeviceTypeList.DALIDimmerDeviceTypeLists.Contains(DeviceType))
                {
                    // read load type
                    #region
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xF012, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                    {
                        for (int intI = 0; intI < wdMaxValue; intI++)
                        {
                            Chans[intI].LoadType = CsConst.myRevBuf[25 + intI];

                            if (Chans[intI].LoadType == 255 || Chans[intI].LoadType > CsConst.LoadType.Length - 1) Chans[intI].LoadType = 0;
                        }
                        CsConst.myRevBuf = new byte[1200];
                    }
                    else return false;
                    #endregion

                    // 最大值 调光曲线
                    #region
                    ArayTmp = null;
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xF020, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                    {
                        for (int intI = 0; intI < wdMaxValue; intI++)
                        {
                            Chans[intI].MaxLevel = CsConst.myRevBuf[25 + intI];
                        }
                        CsConst.myRevBuf = new byte[1200];
                    }
                    else return false;

                    // Read Dimming Profile
                    if (isHasDimmingProfileInEachChannel)
                    {

                        isHasDimmingProfileInEachChannel = false;
                        ArayTmp = null;
                        if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1802, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                        {
                            int DataLen = CsConst.myRevBuf[16];
                            if (DataLen > (9 + 1 + 2 + 1 + wdMaxValue)) isHasDimmingProfileInEachChannel = true;
                            DimmingMode = CsConst.myRevBuf[26];
                            if (isHasDimmingProfileInEachChannel)
                            {
                                for (int intI = 0; intI < wdMaxValue; intI++)
                                {
                                    Chans[intI].DimmingMode = CsConst.myRevBuf[27 + wdMaxValue + intI];
                                }
                                for (int intI = 0; intI < wdMaxValue; intI++)
                                {
                                    Chans[intI].dimmingProfile = CsConst.myRevBuf[27 + intI];
                                }
                            }
                            else
                            {
                                for (int intI = 0; intI < wdMaxValue; intI++)
                                {
                                    Chans[intI].dimmingProfile = CsConst.myRevBuf[27 + intI];
                                }
                            }
                        }
                        else return false;
                    }
                    #endregion
                }
                else
                {
                    // 掉电前亮度    上电亮度
                    #region
                    ArayTmp[0] = 3;
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xF016, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                    {
                        for (int intI = 0; intI < wdMaxValue; intI++)
                        {
                            Chans[intI].levelIfNoPower = CsConst.myRevBuf[26 + intI];
                            if (Chans[intI].levelIfNoPower > 100) Chans[intI].levelIfNoPower = 100;
                        }
                        CsConst.myRevBuf = new byte[1200];
                    }
                    else return false;

                    ArayTmp[0] = 4;
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xF016, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                    {
                        for (int intI = 0; intI < wdMaxValue; intI++)
                        {
                            Chans[intI].levelWhenPowerOn = CsConst.myRevBuf[26 + intI];
                            if (Chans[intI].levelWhenPowerOn > 100) Chans[intI].levelWhenPowerOn = 100;
                        }
                        CsConst.myRevBuf = new byte[1200];
                    }
                    else return false;
                    #endregion
                }
                #endregion
                MyRead2UpFlags[0] = true;
            }
            // 读取区域信息
            if (intActivePage == 0 || intActivePage == 2 || intActivePage == 3 || intActivePage == 4)
            {
                Areas = new List<Area>();
                #region
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x0004, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                {
                    Byte iTmpStart = 4;
                    if (DimmerDeviceTypeList.DALIDimmerDeviceTypeLists.Contains(DeviceType)) iTmpStart =0;
                    byte bytTalArea = CsConst.myRevBuf[25 + iTmpStart];
                    if (bytTalArea == 255) bytTalArea = 0;
                    if (bytTalArea > wdMaxValue) bytTalArea = 0;
                    for (int intI = 0; intI < Chans.Count; intI++)
                    {
                        Chans[intI].intBelongs = CsConst.myRevBuf[26 + iTmpStart + intI];
                    }
                    CsConst.myRevBuf = new byte[1200];
                    if (bytTalArea == 0)
                    {
                        for (int intI = 0; intI < Chans.Count; intI++)
                        {
                            Chans[intI].intBelongs = 0;
                        }
                    }
                    for (byte intI = 0; intI < bytTalArea; intI++)
                    {
                        Area area = new Area();
                        area.ID = (byte)(intI + 1);
                        ArayTmp = new byte[1];
                        ArayTmp[0] = (byte)(intI + 1);

                        if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xF00A, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                        {
                            byte[] arayRemark = new byte[20];
                            for (int intJ = 0; intJ < 20; intJ++) { arayRemark[intJ] = CsConst.myRevBuf[26 + intJ]; }
                            area.Remark = HDLPF.Byte2String(arayRemark);
                        }
                        else return false;
                        CsConst.myRevBuf = new byte[1200];
                        
                        // 读取场景信息
                        if (intActivePage == 0 || intActivePage == 3 || intActivePage == 4)
                        {
                            area.Scen = new List<Scene>();
                            #region
                            for (byte bytI = 0; bytI <= 12; bytI++)
                            {
                                Scene scen = new Scene();
                                scen.ID = bytI;

                                ArayTmp = new byte[2];
                                ArayTmp[0] = (byte)(intI + 1);
                                ArayTmp[1] = bytI;

                                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xF024, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                                {
                                    byte[] arayRemark = new byte[20];
                                    for (int intJ = 0; intJ < 20; intJ++) { arayRemark[intJ] = CsConst.myRevBuf[27 + intJ]; }
                                    scen.Remark = HDLPF.Byte2String(arayRemark);
                                }
                                else return false;
                                CsConst.myRevBuf = new byte[1200];

                                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x0000, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                                {
                                    scen.Time = CsConst.myRevBuf[27] * 256 + CsConst.myRevBuf[28];
                                    byte[] ArayByt = new byte[wdMaxValue];
                                    for (int intJ = 0; intJ < wdMaxValue; intJ++) { ArayByt[intJ] = CsConst.myRevBuf[29 + intJ]; if (ArayByt[intJ] > 100) ArayByt[intJ] = 100; }
                                    scen.light = ArayByt.ToList();
                                };
                                CsConst.myRevBuf = new byte[1200];
                                
                                area.Scen.Add(scen);
                            }
                           
                            #endregion
                            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(30 + intI, null);
                            MyRead2UpFlags[1] = true;
                            MyRead2UpFlags[2] = true;
                        }

                        if (intActivePage == 0 || intActivePage == 4)
                        {
                            //读取序列信息 读取当前哪个区域有序列，和序列数目
                            area.Seq = new List<Sequence>();
                            bool blnIsOnlyAreaHasSeries = DimmerDeviceTypeList.DimmerModuleV1233.Contains(DeviceType);
 
                            byte bytAreaHasSeries = 0;
                            byte bytAllSeries = 2;

                            #region
                            if (blnIsOnlyAreaHasSeries == true)
                            {
                                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x001C, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                                {
                                    bytAreaHasSeries = CsConst.myRevBuf[25];
                                    bytAllSeries = 6;
                                }
                                else return false;
                            }

                            if ((blnIsOnlyAreaHasSeries && bytAreaHasSeries == intI + 1) || blnIsOnlyAreaHasSeries == false)
                            {
                                for (byte bytI = 0; bytI < bytAllSeries; bytI++)
                                {
                                    #region
                                    Sequence oSeq = new Sequence();
                                    oSeq.ID = (byte)(bytI + 1);

                                    ArayTmp = new byte[2];
                                    ArayTmp[0] = (byte)(intI + 1);
                                    ArayTmp[1] = (byte)(bytI + 1);

                                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xF028, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                                    {
                                        byte[] arayRemark = new byte[20];
                                        for (int intJ = 0; intJ < 20; intJ++) { arayRemark[intJ] = CsConst.myRevBuf[27 + intJ]; }
                                        oSeq.Remark = HDLPF.Byte2String(arayRemark);
                                    }
                                    else return false;
                                    CsConst.myRevBuf = new byte[1200];

                                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x0012, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                                    {
                                        oSeq.ID = (byte)(bytI + 1);
                                        oSeq.Mode = CsConst.myRevBuf[27];
                                        oSeq.Steps = CsConst.myRevBuf[28];
                                        oSeq.Times = CsConst.myRevBuf[29];
                                        oSeq.SceneIDs = new List<byte>() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                                        oSeq.RunTimeH = new List<byte>() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                                        oSeq.RunTimeL = new List<byte>() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

                                        CsConst.myRevBuf = new byte[1200];
                                        for (int intJ = 0; intJ < oSeq.Steps; intJ++)
                                        {
                                            ArayTmp = new byte[3];
                                            ArayTmp[0] = (byte)(intI + 1);
                                            ArayTmp[1] = (byte)(bytI + 1);
                                            ArayTmp[2] = (byte)(intJ + 1);

                                            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x0014, bytSubID, bytDevID, false, false, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                                            {
                                                oSeq.SceneIDs[intJ] = CsConst.myRevBuf[28];
                                                oSeq.RunTimeH[intJ] = CsConst.myRevBuf[29];
                                                oSeq.RunTimeL[intJ] = CsConst.myRevBuf[30];
                                                CsConst.myRevBuf = new byte[1200];
                                            }
                                            else
                                            {
                                                break;
                                            }
                                        }
                                    };
                                    CsConst.myRevBuf = new byte[1200];
                                    area.Seq.Add(oSeq);
                                    #endregion
                                }
                            }
                            #endregion
                            if ((70 + intI) < 100)
                            {
                                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(70 + intI, null);
                            }
                            MyRead2UpFlags[1] = true;
                            MyRead2UpFlags[3] = true;
                        }
                        Areas.Add(area);
                    }
                    if (intActivePage == 0 || intActivePage == 3)
                    {
                        if (Areas != null && Areas.Count > 0)
                        {
                            Boolean NeedReadSceneID = false;
                            ArayTmp = new byte[0];
                            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xF051, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                            {
                                for (int i = 0; i < Areas.Count; i++)
                                {
                                    Areas[i].bytDefaultSce = CsConst.myRevBuf[25 + i];
                                    if (Areas[i].bytDefaultSce == 0 || Areas[i].bytDefaultSce ==255)
                                    {
                                        Areas[i].bytDefaultSce = 13; // 255 表示掉电前状态
                                    }
                                    else if (NeedReadSceneID == false)
                                    {
                                        NeedReadSceneID = true;
                                    }
                                }
                                CsConst.myRevBuf = new byte[1200];
                            }
                            else return false;

                            if (NeedReadSceneID)  //指定场景
                            {
                                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xF055, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                                {
                                    for (int i = 0; i < Areas.Count; i++)
                                    {
                                        if (Areas[i].bytDefaultSce != 13)
                                        {
                                            Areas[i].bytDefaultSce = CsConst.myRevBuf[25 + i];
                                        }
                                    }
                                    CsConst.myRevBuf = new byte[1200];
                                }
                            }
                        }
                    }
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(100);
                }
                else return false;
                #endregion
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(100);
            return true;
        }
    }

}
