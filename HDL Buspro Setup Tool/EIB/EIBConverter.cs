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
    public class EIBConverter : HdlDeviceBackupAndRestore
    {
        public int DIndex;//设备唯一编号
        //基本信息
        public string Devname;//子网ID 设备ID 备注
        public string Address;//物理地址   区域地址 /  线地址  /  物理地址

        internal List<OtherInfo> otherInfo;

        public bool[] MyRead2UpFlags = new bool[2] { false, false }; //前面四个表示读取 后面表示上传
        //配置信息
        internal class OtherInfo
        {
            public string GroupAddress;//组地址
            public byte Type;//转换类型
            public string strDevName; // 子网ID和设备ID

            public byte ControlType; // 转换的控制类型
            public byte Param1;//参数1
            public byte Param2;//参数2
            public byte Param3;//参数3
            public byte Param4;//参数4
        }

        ///<summary>
        ///读取默认的设置
        ///</summary>
        public void ReadDefaultInfo()
        {
            if (otherInfo == null) otherInfo = new List<OtherInfo>();
        }

        ///<summary>
        ///读取数据库面板设置，将所有数据读至缓存
        ///</summary>
        public void ReadDevieceInfoFromDB(int DIndex)
        {
            //read basic information of EIB
            #region
            string str = "select * from dbDeviceInfo where DIndex=" + DIndex.ToString();
            OleDbDataReader dr = DataModule.SearchAResultSQLDB(str);

            if (dr == null) return;
            if (dr.HasRows)
            {
                while (dr.Read())
                {

                    Address = dr.GetString(2);
                }
                dr.Close();
            }
            #endregion

            //read EIB to bus
            #region

            str = string.Format("select * from dbEIBDev where DIndex={0}", DIndex);
            dr = DataModule.SearchAResultSQLDB(str);
            otherInfo = new List<OtherInfo>();
            if (dr == null) return;
            while (dr.Read())
            {
                OtherInfo tmp = new OtherInfo();
                tmp.GroupAddress = dr.GetString(2);
                tmp.ControlType = dr.GetByte(3);
                tmp.Type = dr.GetByte(4);
                tmp.Param1 = dr.GetByte(5);
                tmp.Param2 = dr.GetByte(6);
                tmp.Param3 = dr.GetByte(7);
                tmp.Param4 = dr.GetByte(8);
                tmp.strDevName = dr.GetString(9);
                otherInfo.Add(tmp);
            }
            dr.Close();
            #endregion

        }
        
        ///<summary>
        ///保存数据库面板设置，将所有数据保存
        ///</summary>
        public void SaveDevieceInfoToDB(int DIndex)
        {
            /// delete all old information and refresh the database
            string strsql = string.Format("delete from dbDeviceInfo where  DIndex={0}", DIndex);
            DataModule.ExecuteSQLDatabase(strsql);

            strsql = string.Format("delete from dbEIBDev where  DIndex={0}", DIndex);
            DataModule.ExecuteSQLDatabase(strsql);

            ///save save basic setup of EIB
            #region
            strsql = string.Format("insert into dbDeviceInfo(DIndex,Devname,Address) values({0},'{1}','{2}')", DIndex, Devname, Address);
            DataModule.ExecuteSQLDatabase(strsql);
            #endregion

            //save bus to EIB command
            #region

            if (otherInfo == null) return;
            byte bytI = 0;
            for (int i = 0; i < otherInfo.Count; i++)
            {
                bytI++;
                OtherInfo temp = otherInfo[i];
                System.Diagnostics.Debug.WriteLine(otherInfo[1].Param4);
                string sql = "insert into dbEIBDev(DIndex,CurNo,GroupAddress,ControlType,Type,Param1,Param2,Param3,Param4,DestDev) values(@DIndex,@CurNo,@GroupAddress,@ControlType,@Type,@Param1,@Param2,@Param3,@Param4,@DestDev)";
                //创建一个OleDbConnection对象
                OleDbConnection conn;
                conn = new OleDbConnection(DataModule.ConString + CsConst.mstrDefaultPath);
                conn.Open();
                OleDbCommand cmdTmp = new OleDbCommand(sql, conn);
                ((OleDbParameter)cmdTmp.Parameters.Add("@DIndex", OleDbType.Integer)).Value = DIndex;
                ((OleDbParameter)cmdTmp.Parameters.Add("@CurNo", OleDbType.Char)).Value = i;
                ((OleDbParameter)cmdTmp.Parameters.Add("@GroupAddress", OleDbType.VarChar)).Value = temp.GroupAddress;
                ((OleDbParameter)cmdTmp.Parameters.Add("@ControlType", OleDbType.Char)).Value = temp.ControlType;
                ((OleDbParameter)cmdTmp.Parameters.Add("@Type", OleDbType.Char)).Value = temp.Type;
                ((OleDbParameter)cmdTmp.Parameters.Add("@Param1", OleDbType.Char)).Value = temp.Param1;
                ((OleDbParameter)cmdTmp.Parameters.Add("@Param2", OleDbType.Char)).Value = temp.Param2;
                ((OleDbParameter)cmdTmp.Parameters.Add("@Param3", OleDbType.Char)).Value = temp.Param3;
                ((OleDbParameter)cmdTmp.Parameters.Add("@Param4", OleDbType.Char)).Value = temp.Param4;
                ((OleDbParameter)cmdTmp.Parameters.Add("@DestDev", OleDbType.VarChar)).Value = temp.strDevName;
                try
                {
                    cmdTmp.ExecuteNonQuery();

                }
                catch (OleDbException exp)
                {
                    MessageBox.Show(exp.ToString());
                }
                conn.Close();

            }
            #endregion
        }

        /// <summary>
        ///上传设置
        /// </summary>
        public bool UploadCurtainInfosToDevice(string DevName)
        {
            string strMainRemark = DevName.Split('\\')[1].Trim().Split('(')[0].Trim();
            DevName = DevName.Split('\\')[0].Trim();
            //保存basic informations
            byte bytSubID = byte.Parse(DevName.Split('-')[0].ToString());
            byte bytDevID = byte.Parse(DevName.Split('-')[1].ToString());
            byte[] ArayMain = new byte[20];
            byte[] arayTmpRemark = HDLUDP.StringToByte(strMainRemark);
            if (arayTmpRemark.Length > 20)
            {
                Array.Copy(arayTmpRemark, 0, ArayMain, 0, 20);
            }
            else
            {
                Array.Copy(arayTmpRemark, 0, ArayMain, 0, arayTmpRemark.Length);
            }

            if (CsConst.mySends.AddBufToSndList(ArayMain, 0x0010, bytSubID, bytDevID, false, true, true, false) == false)
            {
                return false;
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(1);
            // 修改基本信息物理地址设置
            byte[] ArayTmp = new byte[2];
            string[] ArayStr = Address.Split('/').ToArray();

            ArayTmp[0] = Convert.ToByte(Convert.ToByte(ArayStr[0]) << 4 | Convert.ToByte(ArayStr[1]));
            ArayTmp[1] = Convert.ToByte(ArayStr[2]);

            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xF257, bytSubID, bytDevID, false, true, true, false) == true)
            {
                CsConst.myRevBuf = new byte[1200];
            }
            else return false;
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(2);
            // 修改转换列表
            if (otherInfo != null && otherInfo.Count != 0)
            {
                byte bytID = 0;
                ArayTmp = new byte[11];
                foreach (OtherInfo oTmp in otherInfo)
                {
                    ArayTmp[0] = bytID;
                    ArayStr = oTmp.GroupAddress.Split('/').ToArray();

                    ArayTmp[1] = Convert.ToByte(Convert.ToByte(ArayStr[0]) << 3 | Convert.ToByte(ArayStr[1]));
                    ArayTmp[2] = Convert.ToByte(ArayStr[2]);
                    ArayTmp[3] = oTmp.Type;
                    ArayTmp[4] = Convert.ToByte(oTmp.strDevName.Split('/')[0].ToString());
                    ArayTmp[5] = Convert.ToByte(oTmp.strDevName.Split('/')[1].ToString());
                    ArayTmp[6] = oTmp.ControlType;
                    ArayTmp[7] = oTmp.Param1;
                    ArayTmp[8] = oTmp.Param2;
                    ArayTmp[9] = oTmp.Param3;
                    ArayTmp[10] = oTmp.Param4;
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xC082, bytSubID, bytDevID, false, true, true, false) == true)
                    {
                        CsConst.myRevBuf = new byte[1200];
                    }
                    else return false;
                    bytID++;
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(3 + 100 / otherInfo.Count * bytID);
                } 
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(100);
            MyRead2UpFlags[1] = true;
            return true;
        }


        /// <summary>
        /// 下载所有数据
        /// </summary>
        /// <param name="DevName"></param>
        /// <returns></returns>
        public bool DownloadEIBInforsFrmDevice(string DevName)
        {
            string strMainRemark = DevName.Split('\\')[1].Trim();
            DevName = DevName.Split('\\')[0].Trim();

            byte bytSubID = byte.Parse(DevName.Split('-')[0].ToString());
            byte bytDevID = byte.Parse(DevName.Split('-')[1].ToString());

            byte[] ArayTmp = new byte[0];
            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x000E, bytSubID, bytDevID, false, true, true, false) == false)
            {
                return false;
            }
            else
            {
                byte[] arayRemark = new byte[20];
                for (int intI = 0; intI < 20; intI++) { arayRemark[intI] = CsConst.myRevBuf[25 + intI]; }
                DevName = bytSubID.ToString() + "-" + bytDevID.ToString() + "\\" + HDLPF.Byte2String(arayRemark);
                CsConst.myRevBuf = new byte[1200];
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(1);
            // 读取基本信息物理地址设置
            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xF255, bytSubID, bytDevID, false, true, true, false) == true)
            {
                Address = (CsConst.myRevBuf[25] & 0xF0 >> 4).ToString() + " / "
                        + (CsConst.myRevBuf[25] & 0x0F).ToString()
                        + CsConst.myRevBuf[26].ToString();
                CsConst.myRevBuf = new byte[1200];
            }
            else return false;
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(2);
           
            // 读取转换信息列表
            #region
            otherInfo = new List<OtherInfo>();           
            for (int i = 0; i < 254; i++)
            {
                ArayTmp = new byte[1];
                OtherInfo ch = new OtherInfo();
               
                ArayTmp[0] = (byte)(i);
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xC080, bytSubID, bytDevID, false, true, true, false) == true)
                {
                    if (CsConst.myRevBuf != null)
                    {
                        ch.GroupAddress = (CsConst.myRevBuf[26] & 0x78 >> 3).ToString()
                                        + (CsConst.myRevBuf[26] & 0x07).ToString()
                                        + CsConst.myRevBuf[27];
                        ch.Type = CsConst.myRevBuf[28];
                        ch.strDevName = CsConst.myRevBuf[29].ToString() + " / " + CsConst.myRevBuf[30].ToString();
                        ch.ControlType = CsConst.myRevBuf[31];
                        ch.Param1 = CsConst.myRevBuf[32];
                        ch.Param2 = CsConst.myRevBuf[33];
                        ch.Param3 = CsConst.myRevBuf[34];
                        ch.Param4 = CsConst.myRevBuf[35];
                    }
                    CsConst.myRevBuf = new byte[1200];
                    HDLUDP.TimeBetwnNext(10);
                    otherInfo.Add(ch);
                }
                else return false;
                 if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(i / 3, null);
            }
            #endregion
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(100, null);
            MyRead2UpFlags[0] = true;
            return true;
        }

    }
}
