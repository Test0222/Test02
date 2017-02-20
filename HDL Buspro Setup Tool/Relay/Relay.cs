using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.Xml;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;


namespace HDL_Buspro_Setup_Tool
{
     [Serializable]
    public class Relay : HdlDeviceBackupAndRestore
    {
        private bool isHaveOFFDelay = false;
        public bool isHaveBroadCast = false;
        public  int DIndex; //// 设备在工程内唯一编号
        public string DeviceName;
        public byte bytRepartS; // weather broadcast status or not 1 or 0
        public bool isOutput;//场景现场输出
        public byte[] bytAryExclusion;
        internal List<RelayChannel> Chans;
        internal List<Area> Areas;
        internal List<Byte[]> SlaveInfo;

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
            public byte Times; //运行次数
            public byte Steps;//总共多少步
            public List<byte> SceneIDs;
            public List<byte> RunTimeH;
            public List<byte> RunTimeL;
            public byte bytLastStep;  // 1表示停留在最后一步  0表示回复序列前状态默认为0

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

        public bool[] MyRead2UpFlags = new bool[8]; //前面四个表示读取 后面表示上传

        ////<summary>
        ////读取默认的继电器设置，将所有数据读取缓存
        ////</summary>
        public void ReadDefaultInfo(int intDeviceType)
        {
            #region
            int wdMaxValue = DeviceTypeList.GetMaxValueFromPublicModeGroup(intDeviceType);

            bytRepartS = 0;
            Chans = new List<RelayChannel>();
            Areas = new List<Area>();

            Area area = new Area();
            area.ID = 1;
            area.Remark = "Area 1";

            for (int i = 0; i < wdMaxValue; i++)
            {
                RelayChannel ch = new RelayChannel();
                ch.ID = i + 1;
                ch.Remark = "Chn " + (i + 1).ToString();
                ch.OnDelay = 0;
                ch.ProtectDelay = 0;
                ch.intBelongs = 1;
                ch.bytOnStair = 0;
                ch.intONTime = 0;
                Chans.Add(ch);
            }


            // wdMaxvalue + 1 scenes open lights one by one and all at last
            area.Scen = new List<Scene>();
            for (int j = 0; j <= 12; j++)
            {
                Scene sc = new Scene();
                sc.ID = byte.Parse(j.ToString());
                sc.Remark = "Scene " + j.ToString();
                sc.Time = 0;
                sc.light = new List<byte>();
                for (int k = 0; k < wdMaxValue; k++)
                {
                    if (j == 0) sc.light.Add(0); 
                    else if (j == wdMaxValue + 1) sc.light.Add(100);
                    else
                    {
                        if (k == j-1)
                            sc.light.Add(100);
                        else
                            sc.light.Add(0);
                    }
                }
                area.Scen.Add(sc);
            }

            // add sequence for all total 1
            area.Seq = new List<Sequence>();
            Sequence se = new Sequence();
            se.ID = 1;
            se.Mode = 2;
            se.Remark = "Sequence 1";
            se.SceneIDs = new List<byte>();
            se.RunTimeH = new List<byte>();
            se.RunTimeL = new List<byte>();
            se.bytLastStep = 0;

            se.Times = 1;
            se.Steps = (byte)wdMaxValue;
            for (byte j = 1; j <= wdMaxValue; j++)
            {
                se.SceneIDs.Add(j);
                se.RunTimeH.Add(0);
                se.RunTimeL.Add(1);
            }
            area.Seq.Add(se);
            Areas.Add(area);

            bytAryExclusion = new Byte[wdMaxValue / 2];
            #endregion
        }


        //<summary>
        //读取数据库里的调光模块的设置，将所有数据读取缓存
        //</summary>
        public void ReadInfoForDB(int id)
        {
            try
            {
                #region
                string strsql = string.Format("select * from dbClassInfomation where DIndex={0} and ClassID={1} order by SenNum", DIndex, 0);
                OleDbDataReader dr = DataModule.SearchAResultSQLDB(strsql, CsConst.mstrCurPath);
                Chans = new List<RelayChannel>();
                if (dr != null)
                {
                    while (dr.Read())
                    {
                        RelayChannel temp = new RelayChannel();
                        string str = dr.GetValue(5).ToString();
                        temp.ID = Convert.ToByte(str.Split('-')[0].ToString());
                        temp.LoadType = Convert.ToByte(str.Split('-')[1].ToString());
                        temp.OnDelay = Convert.ToByte(str.Split('-')[2].ToString());
                        temp.ProtectDelay = Convert.ToByte(str.Split('-')[3].ToString());
                        temp.intBelongs = Convert.ToByte(str.Split('-')[4].ToString());
                        temp.bytOnStair = Convert.ToByte(str.Split('-')[5].ToString());
                        temp.intONTime = Convert.ToByte(str.Split('-')[6].ToString());
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
                                temp2.bytLastStep = Convert.ToByte(strTmp2.Split('-')[4].ToString());
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
                        bytRepartS = Convert.ToByte(str.Split('-')[0].ToString());
                        isOutput = Convert.ToBoolean(str.Split('-')[1].ToString());
                        isHaveOFFDelay = Convert.ToBoolean(str.Split('-')[2].ToString());
                        isHaveBroadCast = Convert.ToBoolean(str.Split('-')[3].ToString());
                        bytAryExclusion = ((byte[])dr[6]);
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
        //将缓存里所有继电器的设置存在数据库
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
                        RelayChannel tmpChn = Chans[i];
                        string strParam = tmpChn.ID.ToString() + "-" + tmpChn.LoadType.ToString() + "-"
                                        + tmpChn.OnDelay.ToString() + "-" + tmpChn.ProtectDelay.ToString() + "-"
                                        + tmpChn.intBelongs.ToString() + "-" + tmpChn.bytOnStair.ToString() + "-"
                                        + tmpChn.intONTime.ToString() + "-" + tmpChn.OFFDelay.ToString() + "-"
                                        + tmpChn.OFFProtectDelay.ToString();
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
                            for (int j = 0; j < tmpArea.Scen.Count; j++)
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
                                         + "-" + tmpArea.Seq[j].Times.ToString() + "-" + tmpArea.Seq[j].Steps.ToString()
                                         + "-" + tmpArea.Seq[j].bytLastStep.ToString();
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
                if (bytAryExclusion == null) bytAryExclusion = new byte[0];
                string strParamBasic = bytRepartS.ToString() + "-" + isOutput.ToString()
                                + "-" + isHaveOFFDelay.ToString() + "-" + isHaveBroadCast.ToString();
                strsql = @"Insert into dbClassInfomation(DIndex,ClassID,ObjectID,SenNum,Remark,strParam1,byteAry1)"
                  + " values(@DIndex,@ClassID,@ObjectID,@SenNum,@Remark,@strParam1,@byteAry1)";
                //创建一个OleDbConnection对象
                OleDbConnection connBasic;
                connBasic = new OleDbConnection(DataModule.ConString + CsConst.mstrDefaultPath);
                //OleDbConnection conn = new OleDbConnection(DataModule.ConString + CsConst.mstrCurPath);
                connBasic.Open();
                OleDbCommand cmdBaic = new OleDbCommand(strsql, connBasic);
                ((OleDbParameter)cmdBaic.Parameters.Add("@DIndex", OleDbType.Integer)).Value = DIndex;
                ((OleDbParameter)cmdBaic.Parameters.Add("@ClassID", OleDbType.Integer)).Value = 4;
                ((OleDbParameter)cmdBaic.Parameters.Add("@ObjectID", OleDbType.Integer)).Value = 0;
                ((OleDbParameter)cmdBaic.Parameters.Add("@SenNum", OleDbType.Integer)).Value = 0;
                ((OleDbParameter)cmdBaic.Parameters.Add("@Remark", OleDbType.VarChar)).Value = "";
                ((OleDbParameter)cmdBaic.Parameters.Add("@strParam1", OleDbType.VarChar)).Value = strParamBasic;
                ((OleDbParameter)cmdBaic.Parameters.Add("@byteAry1", OleDbType.Binary)).Value = bytAryExclusion;
                #endregion
            }
            catch
            {
            }
        }

        public void SaveScenesInfo()
        {
            try
            {
                string strsql = string.Format("delete * from dbClassInfomation where DIndex={0}", DIndex);
                DataModule.ExecuteSQLDatabase(strsql);

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
                            for (int j = 0; j < tmpArea.Scen.Count; j++)
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
                    }
                }
                #endregion
            }
            catch
            {
            }
        }

        public void ReadScenesInfo()
        {
            try
            {
                #region
                string strsql = string.Format("select * from dbClassInfomation where DIndex={0} and ClassID={1} order by SenNum", DIndex, 1);
                OleDbDataReader dr = DataModule.SearchAResultSQLDB(strsql);
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
                        OleDbDataReader dr1 = DataModule.SearchAResultSQLDB(str1);
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
                        Areas.Add(temp);
                    }
                    dr.Close();
                }
                #endregion
            }
            catch
            {
            }
        }

        public bool UploadRelayInfosToDevice(string DevName, int intActivePage,int DeviceType)// 0 mean all, else that tab only
        {
            string strMainRemark = DevName.Split('\\')[1].Trim();
            DevName = DevName.Split('\\')[0].Trim();
            //保存回路信息
            byte bytSubID = byte.Parse(DevName.Split('-')[0].ToString());
            byte bytDevID = byte.Parse(DevName.Split('-')[1].ToString());

            int wdMaxValue =  DeviceTypeList.GetMaxValueFromPublicModeGroup(DeviceType);

            byte[] arayRemark = new byte[21];// used for restore remark
            byte[] ArayMain = new byte[20];

            if (HDLSysPF.ModifyDeviceMainRemark(bytSubID,bytDevID,strMainRemark,DeviceType) == true)
            {
                CsConst.myRevBuf = new byte[1200];
                HDLUDP.TimeBetwnNext(20);
            }
            else return false;
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(1);

            Byte[] arayLoadType = new Byte[wdMaxValue];
            Byte[] arayOnDelay = new Byte[wdMaxValue];
            Byte[] arayProDelay = new Byte[wdMaxValue];
            Byte[] arayOFFDelay = new Byte[wdMaxValue];
            Byte[] arayOFFProDelay = new Byte[wdMaxValue];
            Byte[] arayEnableStair = new Byte[wdMaxValue];
            Byte[] arayEnableRelayButton = new Byte[wdMaxValue + 1];
            arayEnableRelayButton[0] = (Byte)wdMaxValue;

            int[] arayONTime = new int[wdMaxValue];

            if (intActivePage == 0)
            {
                if (isHaveBroadCast)
                {
                    byte[] arayTmp = new byte[1];
                    arayTmp[0] = bytRepartS;
                    if (CsConst.mySends.AddBufToSndList(arayTmp, 0x1808, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == false) return false;
                    CsConst.myRevBuf = new byte[1200];
                    HDLUDP.TimeBetwnNext(20);
                }
            }
            #region
            byte bytI = 0;
            foreach (RelayChannel ch in Chans)
            {
                if (intActivePage == 0 || intActivePage == 5 || intActivePage == 1)
                {
                    // modify the chns remark
                    arayRemark = new byte[21]; // 初始化数组
                    string strRemark = ch.Remark;
                    byte[] arayTmp = HDLUDP.StringToByte(strRemark);

                    arayRemark[0] = byte.Parse(ch.ID.ToString());
                    if (arayTmp != null) arayTmp.CopyTo(arayRemark, 1);
                    if (CsConst.mySends.AddBufToSndList(arayRemark, 0xF010, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == false) return false;
                    CsConst.myRevBuf = new byte[1200];
                    HDLUDP.TimeBetwnNext(20);
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress((bytI + 1) * 10 / Chans.Count, null);
                    bytI++;
                }
                if (ch.LoadType == -1 || ch.LoadType > 20) ch.LoadType = 0;
                arayLoadType[ch.ID - 1] = byte.Parse(ch.LoadType.ToString());
                arayOnDelay[ch.ID - 1] = byte.Parse(ch.OnDelay.ToString());
                arayProDelay[ch.ID - 1] = byte.Parse(ch.ProtectDelay.ToString());
                arayOFFDelay[ch.ID - 1] = byte.Parse(ch.OFFDelay.ToString());
                arayOFFProDelay[ch.ID - 1] = byte.Parse(ch.OFFProtectDelay.ToString());
                arayEnableStair[ch.ID - 1] = ch.bytOnStair;
                arayONTime[ch.ID - 1] = ch.intONTime;
                arayEnableRelayButton[ch.ID] = ch.bEnableChn;
            }
            #endregion

            if (intActivePage == 0 || intActivePage == 1)
            {
                #region
                // modify the load type
                if (!RelayDeviceTypeList.RelayWithOutLoadTypeReading.Contains(DeviceType))
                {
                    if (CsConst.mySends.AddBufToSndList(arayLoadType, 0xF014, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == false) return false;
                    CsConst.myRevBuf = new byte[1200];
                    HDLUDP.TimeBetwnNext(20);
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(11, null);
                }
                // modify the on delay 
                if (CsConst.mySends.AddBufToSndList(arayOnDelay, 0xF04F, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == false) return false;
                CsConst.myRevBuf = new byte[1200];
                HDLUDP.TimeBetwnNext(20);
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(12, null);
                // modify the protect delay
                if (CsConst.mySends.AddBufToSndList(arayProDelay, 0xF041, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == false) return false;
                CsConst.myRevBuf = new byte[1200];
                HDLUDP.TimeBetwnNext(20);
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(13, null);
                if (isHaveOFFDelay)
                {
                    if (CsConst.mySends.AddBufToSndList(arayOFFDelay, 0xF086, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == false) return false;
                    CsConst.myRevBuf = new byte[1200];
                    HDLUDP.TimeBetwnNext(20);
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(12, null);
                    // modify the protect delay
                    if (CsConst.mySends.AddBufToSndList(arayOFFProDelay, 0xF082, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == false) return false;
                    CsConst.myRevBuf = new byte[1200];
                    HDLUDP.TimeBetwnNext(20);
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(13, null);
                }

                if (RelayDeviceTypeList.RelayModuleWithButtonEnableGroup.Contains(DeviceType))
                {
                    // modify the enable button press
                    if (CsConst.mySends.AddBufToSndList(arayEnableRelayButton, 0x1F56, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == false) return false;
                    CsConst.myRevBuf = new byte[1200];
                    HDLUDP.TimeBetwnNext(10);
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(14, null);
                }
                #endregion
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(20, null);
            }

            if (intActivePage == 0 || intActivePage == 2 || intActivePage == 3)
            {
                if (intActivePage != 3)
                {
                    byte[] arayArea = new byte[wdMaxValue + 3];
                    arayArea[0] = bytSubID;
                    arayArea[1] = bytDevID;

                    if (Areas == null || Areas.Count == 0)
                        arayArea[2] = 1;
                    else
                    {
                        arayArea[2] = (byte)Areas.Count;

                        foreach (RelayChannel chn in Chans)
                        {
                            arayArea[2 + chn.ID] = Convert.ToByte(chn.intBelongs);
                        }
                    }

                    // modify area first or nothing can be go on after that
                    if (CsConst.mySends.AddBufToSndList(arayArea, 0x0006, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == false) return false;
                    CsConst.myRevBuf = new byte[1200];
                    HDLUDP.TimeBetwnNext(10);
                }
                #region
                bytI = 0;
                byte[] bytSceFlag = new byte[Areas.Count];
                byte[] bytSceIDs = new byte[Areas.Count];
                foreach (Area area in Areas)
                {
                    // modify area remark
                    arayRemark = new byte[21]; // 初始化数组
                    string strRemark = area.Remark;
                    byte[] arayTmp = HDLUDP.StringToByte(strRemark);
                    arayRemark[0] = area.ID;
                    arayTmp.CopyTo(arayRemark, 1);
                    if (CsConst.mySends.AddBufToSndList(arayRemark, 0xF00C, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == false) return false;
                    CsConst.myRevBuf = new byte[1200];
                    HDLUDP.TimeBetwnNext(20);

                    if (area.bytDefaultSce != 13)
                    {
                        bytSceFlag[bytI] = 1;
                        bytSceIDs[bytI] = area.bytDefaultSce;
                    }
                    bytI++;
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(20 + 10 * bytI / Areas.Count);
                }
                if (CsConst.mySends.AddBufToSndList(bytSceFlag, 0xF053, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == false) return false;
                CsConst.myRevBuf = new byte[1200];
                HDLUDP.TimeBetwnNext(20);

                if (CsConst.mySends.AddBufToSndList(bytSceIDs, 0xF057, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == false) return false;
                CsConst.myRevBuf = new byte[1200];
                HDLUDP.TimeBetwnNext(20);
                #endregion
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(30);
            }

            // modify the scene one by one remark and other settings

            byte[] ArayStopAt = new byte[24];
            foreach (Area area in Areas)
            {    // scene
                if (intActivePage == 0 || intActivePage == 3)
                {
                    #region
                    bytI = 1;
                    foreach (Scene scen in area.Scen)
                    {
                        string strRemark = scen.Remark;
                        byte[] arayTmp = HDLUDP.StringToByte(strRemark);

                        byte[] arayRemark1 = new byte[22];
                        arayRemark1[0] = area.ID;
                        arayRemark1[1] = scen.ID;
                        if (arayTmp != null) arayTmp.CopyTo(arayRemark1, 2);
                        if (CsConst.mySends.AddBufToSndList(arayRemark1, 0xF026, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == false) return false;
                        CsConst.myRevBuf = new byte[1200];
                        HDLUDP.TimeBetwnNext(20);

                        // modify scene running time and lights
                        byte[] araySce = new byte[4 + wdMaxValue];
                        araySce[0] = area.ID;
                        araySce[1] = scen.ID;
                        araySce[2] = byte.Parse((scen.Time / 256).ToString());
                        araySce[3] = byte.Parse((scen.Time % 256).ToString());

                        int intTmp = 0;
                        foreach (RelayChannel ch in Chans)
                        {   //添加所在区域的亮度值
                            if (ch.intBelongs == area.ID)
                            {
                                araySce[4 + intTmp] = scen.light[ch.ID - 1];
                                intTmp++;
                            }
                        }

                        if (CsConst.mySends.AddBufToSndList(araySce, 0x0008, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == false) return false;
                        CsConst.myRevBuf = new byte[1200];
                        HDLUDP.TimeBetwnNext(20);
                        if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(30 + bytI * 10 / area.Scen.Count);
                        bytI++;
                    }
                    #endregion
                }
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(40);
                //序列
                if (intActivePage == 0 || intActivePage == 4)
                {
                    #region
                    bytI = 1;
                    foreach (Sequence seq in area.Seq)
                    {
                        ArayStopAt[(area.ID - 1) * 2 + seq.ID - 1] = seq.bytLastStep;
                        string strRemark = seq.Remark;
                        byte[] arayTmp = HDLUDP.StringToByte(strRemark);

                        byte[] arayRemark1 = new byte[22];
                        arayRemark1[0] = area.ID;
                        arayRemark1[1] = seq.ID;
                        arayTmp.CopyTo(arayRemark1, 2);
                        if (CsConst.mySends.AddBufToSndList(arayRemark1, 0xF030, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == false) return false;
                        CsConst.myRevBuf = new byte[1200];
                        HDLUDP.TimeBetwnNext(20);

                        // modify seq mode and step
                        byte[] araySce = new byte[5];
                        araySce[0] = area.ID;
                        araySce[1] = seq.ID;
                        araySce[2] = seq.Mode;
                        araySce[3] = seq.Steps;
                        araySce[4] = seq.Times;
                        if (CsConst.mySends.AddBufToSndList(araySce, 0x0018, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == false) return false;
                        CsConst.myRevBuf = new byte[1200];
                        HDLUDP.TimeBetwnNext(20);


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
                            CsConst.myRevBuf = new byte[1200];
                            HDLUDP.TimeBetwnNext(20);
                            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(40 + bytI * i * 10 / (seq.Steps * area.Seq.Count));
                        }
                        bytI++;
                    }
                    #endregion
                }
                if (intActivePage == 0 || intActivePage == 4)
                {
                    if (CsConst.mintNewRelayFunction.Contains(DeviceType))
                    {
                        if (CsConst.mySends.AddBufToSndList(ArayStopAt, 0xE0EE, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == false) return false;
                        CsConst.myRevBuf = new byte[1200];
                        HDLUDP.TimeBetwnNext(20);
                    }
                }
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(50);
            }


            if (intActivePage == 0 || intActivePage == 5)
            {
                //if it is new version , have to upload broadcast flag and stair on time setups
                if (CsConst.mintNewRelayFunction.Contains(DeviceType)) // 新型继电器
                {
                    #region

                    // modify the enable stair function
                    if (CsConst.mySends.AddBufToSndList(arayEnableStair, 0xE0E6, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == false) return false;
                    CsConst.myRevBuf = new byte[1200];
                    HDLUDP.TimeBetwnNext(20);
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(51);

                    // modify the enable stair function
                    byte[] ArayONTimes = new byte[wdMaxValue * 2];
                    for (int i = 0; i < arayONTime.Length; i++)
                    {
                        ArayONTimes[i * 2] = (byte)(arayONTime[i] / 256);
                        ArayONTimes[i * 2 + 1] = (byte)(arayONTime[i] % 256);
                    }
                    if (CsConst.mySends.AddBufToSndList(ArayONTimes, 0xE0EA, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == false) return false;
                    CsConst.myRevBuf = new byte[1200];
                    HDLUDP.TimeBetwnNext(20);
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(52);
                   
                    if (!RelayDeviceTypeList.RelayThreeChnBaseWithACFunction.Contains(DeviceType))
                    {
                        if (CsConst.mySends.AddBufToSndList(bytAryExclusion, 0xE0E2, bytSubID, bytDevID, false, true, true, false) == false) return false;
                        CsConst.myRevBuf = new byte[1200];
                        HDLUDP.TimeBetwnNext(20);
                        if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(53);
                    }
                    #endregion
                }
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(60);
            }

            if (intActivePage == 0 || intActivePage == 6)
            {
                #region
                if (RelayDeviceTypeList.RelayThreeChnBaseWithACFunction.Contains(DeviceType)) 
                {
                    if (SlaveInfo != null && SlaveInfo.Count > 0)
                    {
                        ArayMain = SlaveInfo[0];
                        if (CsConst.mySends.AddBufToSndList(ArayMain, 0xF08A, bytSubID, bytDevID, false, true, true, false) == true)
                        {
                            HDLUDP.TimeBetwnNext(20);
                        }
                        else return false;
                    }
                }
                #endregion
            }

            if (intActivePage == 0 || intActivePage == 7) // 窗帘相关设置
            {
                Byte CurtainChnCount = Convert.ToByte(wdMaxValue / 2);
                for (byte i = 1; i <= CurtainChnCount; i++)
                {
                    ArayMain = new byte[5];
                    ArayMain[0] = i;
                    ArayMain[1] = Chans[(i -1) * 2].bEnableCurtain;
                    ArayMain[2] = Convert.ToByte(Chans[(i -1) * 2].onTime / 256);
                    ArayMain[3] = Convert.ToByte(Chans[(i - 1) * 2].onTime % 256);
                    ArayMain[4] = 0;

                    if (CsConst.mySends.AddBufToSndList(ArayMain, 0x1F52, bytSubID, bytDevID, false, false, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                    {
                        HDLUDP.TimeBetwnNext(20);
                    }
                    else return false;
                }
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(100);
            return true;
        }

        /// <summary>
        /// 从设备读取当前的所有设置信息
        /// </summary>
        /// <param name="DeviceType"></param>
        /// <param name="DevName"></param>
        public bool DownloadRelayInfosToBuffer(string DevName, int intActivePage,int DeviceType)// 0 mean all, else that tab only
        {
            string strMainRemark = DevName.Split('\\')[1].Trim();
            DevName = DevName.Split('\\')[0].Trim();

            byte bytSubID = byte.Parse(DevName.Split('-')[0].ToString());
            byte bytDevID = byte.Parse(DevName.Split('-')[1].ToString());

            int wdMaxValue = DeviceTypeList.GetMaxValueFromPublicModeGroup(DeviceType);

            byte[] ArayTmp = null;
            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x000E, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == false)
            {
                return false;
            }
            else
            {
                byte[] arayRemark = new byte[20];
                for (int intI = 0; intI < 20; intI++) { arayRemark[intI] = CsConst.myRevBuf[25 + intI]; }
                DeviceName = bytSubID.ToString() + "-" + bytDevID.ToString() + "\\" + HDLPF.Byte2String(arayRemark);
                CsConst.myRevBuf = new byte[1200];
                HDLUDP.TimeBetwnNext(1);
            }
            ArayTmp = new byte[1];
            // read broadcast state 
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(1, null);
            if (intActivePage == 0 || intActivePage == 1)
            {
                // 读取回路信息
                Chans = new List<RelayChannel>();
                #region
                for (int i = 0; i < wdMaxValue; i++)
                {
                    RelayChannel ch = new RelayChannel();
                    ch.ID = i + 1;
                    ch.LoadType = 0;
                    ch.OnDelay = 0;
                    ch.ProtectDelay = 0;
                    ch.intBelongs = 0;

                    ArayTmp[0] = (byte)(i + 1);
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xF00E, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                    {
                        if (CsConst.myRevBuf != null)
                        {
                            byte[] arayRemark = new byte[20];
                            for (int intI = 0; intI < 20; intI++) { arayRemark[intI] = CsConst.myRevBuf[26 + intI]; }
                            ch.Remark = HDLPF.Byte2String(arayRemark);
                        }
                        CsConst.myRevBuf = new byte[1200];
                        HDLUDP.TimeBetwnNext(1);
                    }
                    else
                    {
                        return false;
                    }
                    Chans.Add(ch);
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress((i + 1) * 10 / wdMaxValue, null);
                }

                ArayTmp = null;
                // read load type
                if (!RelayDeviceTypeList.RelayWithOutLoadTypeReading.Contains(DeviceType))
                {
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xF012, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                    {
                        for (int intI = 0; intI < wdMaxValue; intI++)
                        {
                            Chans[intI].LoadType = CsConst.myRevBuf[25 + intI];
                            if (Chans[intI].LoadType == 255 || Chans[intI].LoadType > CsConst.LoadType.Length - 1) Chans[intI].LoadType = 0;
                        }
                        CsConst.myRevBuf = new byte[1200];
                        HDLUDP.TimeBetwnNext(1);
                    }
                    else return false;
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(11, null);
                }
                // read on delay
                ArayTmp = null;
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xF04D, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                {
                    for (int intI = 0; intI < wdMaxValue; intI++)
                    {
                        Chans[intI].OnDelay = CsConst.myRevBuf[25 + intI];
                    }
                    CsConst.myRevBuf = new byte[1200];
                    HDLUDP.TimeBetwnNext(1);
                }
                else return false;
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(12, null);
                
                // read protoct delay
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xF03F, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                {
                    for (int intI = 0; intI < wdMaxValue; intI++)
                    {
                        Chans[intI].ProtectDelay = CsConst.myRevBuf[25 + intI];
                    }
                    CsConst.myRevBuf = new byte[1200];
                    HDLUDP.TimeBetwnNext(1);
                }
                else return false;
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(13, null);

                isHaveOFFDelay = false;
                #region
                if (RelayDeviceTypeList.RelayModuleV2OffDelay.Contains(DeviceType))
                {
                     ArayTmp = null;
                     if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xF084, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                     {
                         isHaveOFFDelay = true;
                         for (int intI = 0; intI < wdMaxValue; intI++)
                         {
                             Chans[intI].OFFDelay = CsConst.myRevBuf[25 + intI];
                         }
                         CsConst.myRevBuf = new byte[1200];
                         HDLUDP.TimeBetwnNext(1);
                         if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(14, null);

                         if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xF080, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                         {
                             for (int intI = 0; intI < wdMaxValue; intI++)
                             {
                                 Chans[intI].OFFProtectDelay = CsConst.myRevBuf[25 + intI];
                             }
                             CsConst.myRevBuf = new byte[1200];
                             HDLUDP.TimeBetwnNext(1);
                             if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(15, null);
                         }
                     }
                }
                #endregion
                // enable button on relay surfaces
                #region
                if (RelayDeviceTypeList.RelayModuleWithButtonEnableGroup.Contains(DeviceType))
                {
                    if (CsConst.mySends.AddBufToSndList(null, 0x1F54, bytSubID, bytDevID, false, false, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                    {
                        for (int intI = 0; intI < CsConst.myRevBuf[25]; intI++)
                        {
                            Chans[intI].bEnableChn = CsConst.myRevBuf[26 + intI];
                        }
                    }
                    else return false;
                }
                #endregion
                #endregion
                MyRead2UpFlags[0] = true;
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(20, null);
            }

            if (intActivePage == 0 || intActivePage == 2 || intActivePage == 3 || intActivePage == 4)
            {
                // 读取区域信息
                Areas = new List<Area>();
                #region
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x0004, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                {
                    byte bytTalArea = CsConst.myRevBuf[29];
                    if (bytTalArea == 255) bytTalArea = 0;
                    if (bytTalArea > wdMaxValue) bytTalArea = 0;
                    for (int intI = 0; intI < wdMaxValue; intI++)
                    {
                        Chans[intI].intBelongs = CsConst.myRevBuf[30 + intI];
                    }
                    CsConst.myRevBuf = new byte[1200];
                    HDLUDP.TimeBetwnNext(1);
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
                        HDLUDP.TimeBetwnNext(1);
                       
                        if (intActivePage == 0 || intActivePage == 3 || intActivePage == 4)
                        {
                            // 读取场景信息
                            area.Scen = new List<Scene>();
                            #region
                            for (byte bytI = 0; bytI < 13; bytI++)
                            {
                                ArayTmp = new byte[2];
                                ArayTmp[0] = (byte)(intI + 1);
                                ArayTmp[1] = (byte)(bytI);
                                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xF024, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                                {
                                    if (CsConst.myRevBuf[25] == (intI + 1) && CsConst.myRevBuf[26] == bytI)
                                    {
                                        Scene scen = new Scene();
                                        scen.ID = (byte)(bytI);
                                        byte[] arayRemark = new byte[20];
                                        for (int intJ = 0; intJ < 20; intJ++) { arayRemark[intJ] = CsConst.myRevBuf[27 + intJ]; }
                                        scen.Remark = HDLPF.Byte2String(arayRemark);
                                        CsConst.myRevBuf = new byte[1200];
                                        HDLUDP.TimeBetwnNext(1);

                                        if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x0000, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                                        {
                                            scen.Time = CsConst.myRevBuf[27] * 256 + CsConst.myRevBuf[28];
                                            byte[] ArayByt = new byte[wdMaxValue];
                                            for (int intJ = 0; intJ < wdMaxValue; intJ++) { ArayByt[intJ] = CsConst.myRevBuf[29 + intJ]; }
                                            scen.light = ArayByt.ToList();
                                        }
                                        else return false;
                                        area.Scen.Add(scen);
                                        CsConst.myRevBuf = new byte[1200];
                                        HDLUDP.TimeBetwnNext(1);
                                    }
                                }
                                else return false;
                                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(20 + ((intI) * 40) / (bytTalArea) + (((((intI + 1) * 40) / (bytTalArea)) - ((intI) * 40) / (bytTalArea)) * bytI / 13), null);
                            }
                            #endregion

                            if (intActivePage == 0 || intActivePage == 3)
                            {
                                if (Areas != null && Areas.Count > 0)
                                {
                                    ArayTmp = new byte[0];
                                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xF051, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                                    {
                                        for (int i = 0; i < Areas.Count; i++)
                                        {
                                            Areas[i].bytDefaultSce = CsConst.myRevBuf[25 + i];
                                            if (Areas[i].bytDefaultSce == 0 || Areas[i].bytDefaultSce == 255)
                                            {
                                                Areas[i].bytDefaultSce = 13; // 255 表示掉电前状态
                                            }
                                        }
                                        CsConst.myRevBuf = new byte[1200];
                                        HDLUDP.TimeBetwnNext(1);
                                    }
                                    else return false;

                                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xF055, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                                    {
                                        for (int i = 0; i < Areas.Count; i++)
                                        {
                                            if (Areas[i].bytDefaultSce != 255)
                                            {
                                                Areas[i].bytDefaultSce = CsConst.myRevBuf[25 + i];
                                            }
                                        }
                                        CsConst.myRevBuf = new byte[1200];
                                        HDLUDP.TimeBetwnNext(1);
                                    }
                                }
                            }

                            if (intActivePage == 0 || intActivePage == 4)
                            {
                                //读取序列信息
                                area.Seq = new List<Sequence>();
                                #region
                                for (byte bytI = 0; bytI < 2; bytI++)
                                {
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
                                        oSeq.SceneIDs = new List<byte>();
                                        oSeq.RunTimeH = new List<byte>();
                                        oSeq.RunTimeL = new List<byte>();
                                        if (oSeq.Steps > wdMaxValue) oSeq.Steps = Convert.ToByte(wdMaxValue);
                                        CsConst.myRevBuf = new byte[1200];

                                        for (int intJ = 0; intJ < wdMaxValue; intJ++)
                                        {
                                            ArayTmp = new byte[3];
                                            ArayTmp[0] = (byte)(intI + 1);
                                            ArayTmp[1] = (byte)(bytI + 1);
                                            ArayTmp[2] = (byte)(intJ + 1);

                                            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x0014, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                                            {
                                                oSeq.SceneIDs.Add(CsConst.myRevBuf[28]);
                                                oSeq.RunTimeH.Add(CsConst.myRevBuf[29]);
                                                oSeq.RunTimeL.Add(CsConst.myRevBuf[30]);
                                                CsConst.myRevBuf = new byte[1200];
                                            }
                                            else return false;
                                        }
                                    }
                                    else return false;
                                    area.Seq.Add(oSeq);
                                    CsConst.myRevBuf = new byte[1200];
                                    HDLUDP.TimeBetwnNext(1);
                                }
                            }
                            #endregion
                        }
                        Areas.Add(area);
                        if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(20 + (intI + 1) * 40 / bytTalArea, null);
                    }
                    MyRead2UpFlags[1] = true;
                    if (intActivePage == 3)
                    {
                        MyRead2UpFlags[2] = true;
                        SaveScenesInfo();
                    }
                    else if (intActivePage == 4) MyRead2UpFlags[3] = true;

                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(60, null);
                }
                else return false;
                #endregion
            }

            if (intActivePage == 0 || intActivePage == 5)
            {
                if (CsConst.mintNewRelayFunction.Contains(DeviceType)) // 新型继电器
                {
                    #region
                    // read enable stair or not and on time 
                    if (CsConst.mySends.AddBufToSndList(null, 0xE0E4, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                    {
                        for (int intI = 0; intI < wdMaxValue; intI++)
                        {
                            Chans[intI].bytOnStair = CsConst.myRevBuf[25 + intI];
                        }
                        CsConst.myRevBuf = new byte[1200];
                        HDLUDP.TimeBetwnNext(1);
                    }
                    else return false;
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(61, null);
                   
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xE0E8, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                    {
                        for (int intI = 0; intI < wdMaxValue; intI++)
                        {
                            Chans[intI].intONTime = CsConst.myRevBuf[25 + intI * 2] * 256 + CsConst.myRevBuf[26 + intI * 2];
                        }
                        CsConst.myRevBuf = new byte[1200];
                        HDLUDP.TimeBetwnNext(1);
                    }
                    else return false;
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(62, null);
                    
                    bytAryExclusion = new byte[wdMaxValue / 2];
                    if (!RelayDeviceTypeList.RelayThreeChnBaseWithACFunction.Contains(DeviceType))
                    {
                        if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xE0E0, bytSubID, bytDevID, false, true, true, false) == true)
                        {
                            for (int i = 0; i < bytAryExclusion.Length; i++)
                            {
                                bytAryExclusion[i] = CsConst.myRevBuf[25 + i];
                            }
                            CsConst.myRevBuf = new byte[1200];
                            HDLUDP.TimeBetwnNext(1);
                        }
                        else return false;
                        if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(63, null);
                    }
                    #endregion
                }
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(70, null);
                MyRead2UpFlags[4] = true;
            }

            if (intActivePage == 0 || intActivePage == 6)
            {
                #region
                if (RelayDeviceTypeList.RelayThreeChnBaseWithACFunction.Contains(DeviceType))
                {
                    SlaveInfo = new List<byte[]>();
                    if (CsConst.mySends.AddBufToSndList(null, 0xF088, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                    {
                        byte[] araySlave = new byte[17];
                        Array.Copy(CsConst.myRevBuf, 25, araySlave, 0, araySlave.Length);
                        SlaveInfo.Add(araySlave);
                        HDLUDP.TimeBetwnNext(1);
                    }
                    else return false;
                }
                #endregion
            }

            if (intActivePage == 0 || intActivePage == 7)
            {
                #region
                if (RelayDeviceTypeList.RelayModuleWorkAsCurtainControlGroup.Contains(DeviceType))
                {
                    byte CurtainChnCount = Convert.ToByte(wdMaxValue / 2);
                    for (byte i = 1; i <= CurtainChnCount; i++)
                    {
                        ArayTmp = new byte[1];
                        ArayTmp[0] = i;
                        if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1F50, bytSubID, bytDevID, false, false, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                        {
                            Chans[(i - 1) * 2].bEnableCurtain = CsConst.myRevBuf[26];
                            Chans[(i - 1) * 2].onTime = CsConst.myRevBuf[27] * 256 + CsConst.myRevBuf[28];
                            HDLUDP.TimeBetwnNext(1);
                        }
                        else return false;
                    }
                }
                #endregion
            }

            if (intActivePage == 0)
            {
                isHaveBroadCast = false;
                if (RelayDeviceTypeList.RelayHasBroadcastFlag.Contains(DeviceType)) // 新型继电器
                {
                    if (CsConst.mySends.AddBufToSndList(null, 0x1806, bytSubID, bytDevID, false, false, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                    {
                        isHaveBroadCast = true;
                        bytRepartS = CsConst.myRevBuf[25];
                        CsConst.myRevBuf = new byte[1200];
                        HDLUDP.TimeBetwnNext(1);
                    }
                }
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(100);
            return true;
        }
    }

}
