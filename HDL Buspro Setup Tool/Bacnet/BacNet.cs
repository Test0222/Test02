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
    public class BacNet : HdlDeviceBackupAndRestore
    {
        public int DIndex;//设备唯一编号
        //基本信息
        public string DeviceName;//子网ID 设备ID 备注
        public int Address;//Dev ID
        public int ValidCount;
        public int intPort; // bacnet通讯端口
        public bool blnSwitch; // 是否启用交换机功能 个数底部默认

        public bool[] MyRead2UpFlags = new bool[] { false, false };
        internal List<OtherInfo> otherInfo;
        //配置信息
        internal class OtherInfo
        {
            public byte ID;//ID
            public byte[] BackNetIDAry=new byte[4];//4个byte的地址
            public int Type;//转换类型
            public string strDevName; // 子网ID和设备ID
            public byte Param1;//参数1
            public byte Param2;//参数2
            public byte Param3;//参数3
            public byte Param4;//参数4
            public string Remark; //备注 40个byte
        }

        ///<summary>
        ///读取默认的设置
        ///</summary>
        public void ReadDefaultInfo()
        {
            Address = 2502;
            intPort = 47808; // bacnet通讯端口
            blnSwitch = false; // 是否启用交换机功能 个数底部默认
            if (otherInfo == null) otherInfo = new List<OtherInfo>();
        }

        ///<summary>
        ///读取数据库面板设置，将所有数据读至缓存
        ///</summary>
        public void ReadDevieceInfoFromDB(int intDIndex)
        {
            //read basic information of bacnet
            #region
            string str = "select * from dbBasicBac where DIndex=" + intDIndex.ToString();
            OleDbDataReader dr = DataModule.SearchAResultSQLDB(str, CsConst.mstrCurPath);

            if (dr == null) return;
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    DeviceName = dr.GetString(1);

                    ValidCount = Convert.ToInt32(dr.GetValue(5).ToString());
                    intPort = dr.GetInt16(6);
                    blnSwitch =  (dr.GetByte(7) == 1);
                    Address = dr.GetInt16(8);
                }
                dr.Close();
            }
            #endregion

            //read bacnet to bus
            #region

            str = string.Format("select * from dbBackNet where DIndex={0}", intDIndex);
            dr = DataModule.SearchAResultSQLDB(str, CsConst.mstrCurPath);
            otherInfo = new List<OtherInfo>();
            if (dr == null) return;
            while (dr.Read())
            {
                OtherInfo tmp = new OtherInfo();
                tmp.ID = dr.GetByte(1);
                tmp.BackNetIDAry = (byte[])dr[2];
                tmp.Type = dr.GetInt32(3);
                tmp.strDevName = dr.GetString(4);
                tmp.Param1 = dr.GetByte(5);
                tmp.Param2 = dr.GetByte(6);
                tmp.Param3 = dr.GetByte(7);
                tmp.Param4 = dr.GetByte(8);
                tmp.Remark = dr.GetString(9);
                otherInfo.Add(tmp);
            }
            dr.Close();
            #endregion

        }
        
        ///<summary>
        ///保存数据库面板设置，将所有数据保存
        ///</summary>
        public void SaveDevieceInfoToDB()
        {
            /// delete all old information and refresh the database
            string strsql = string.Format("delete from dbBasicBac where  DIndex={0}", DIndex);
            DataModule.ExecuteSQLDatabase(strsql);

            strsql = string.Format("delete from dbBackNet where  DIndex={0}", DIndex);
            DataModule.ExecuteSQLDatabase(strsql);

            ///save save basic setup of EIB
            #region
            strsql = string.Format("insert into dbBasicBac(DIndex,Remark,strIP,strRouterIP,strMAC,strMaskIP,bytPort,bytSwitch,bytDevID) values({0},'{1}','{2}','{3}','{4}','{5}',{6},{7},{8})",
                                    DIndex, DeviceName, "", "", "", ValidCount.ToString(), intPort, Convert.ToByte(blnSwitch), Address);
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
                string sql = "insert into dbBackNet(DIndex,CurNo,BackNetIDAry,Type,strDevName,Param1,Param2,Param3,Param4,Remark) values(@DIndex,@CurNo,@BackNetIDAry,@Type,@strDevName,@Param1,@Param2,@Param3,@Param4,@Remark)";
                //创建一个OleDbConnection对象
                OleDbConnection conn;
                conn = new OleDbConnection(DataModule.ConString + CsConst.mstrDefaultPath);
                conn.Open();
                OleDbCommand cmdTmp = new OleDbCommand(sql, conn);
                ((OleDbParameter)cmdTmp.Parameters.Add("@DIndex", OleDbType.Integer)).Value = DIndex;
                ((OleDbParameter)cmdTmp.Parameters.Add("@CurNo", OleDbType.Char)).Value = temp.ID;
                ((OleDbParameter)cmdTmp.Parameters.Add("@BackNetIDAry", OleDbType.Binary)).Value = temp.BackNetIDAry;
                ((OleDbParameter)cmdTmp.Parameters.Add("@Type", OleDbType.Integer)).Value = temp.Type;
                ((OleDbParameter)cmdTmp.Parameters.Add("@strDevName", OleDbType.VarChar)).Value = temp.strDevName;
                ((OleDbParameter)cmdTmp.Parameters.Add("@Param1", OleDbType.Char)).Value = temp.Param1;
                ((OleDbParameter)cmdTmp.Parameters.Add("@Param2", OleDbType.Char)).Value = temp.Param2;
                ((OleDbParameter)cmdTmp.Parameters.Add("@Param3", OleDbType.Char)).Value = temp.Param3;
                ((OleDbParameter)cmdTmp.Parameters.Add("@Param4", OleDbType.Char)).Value = temp.Param4;
                ((OleDbParameter)cmdTmp.Parameters.Add("@Remark", OleDbType.VarChar)).Value = temp.Remark;
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
        public bool UploadCurtainInfosToDevice(string DevName, int intActivePage)
        {
            string strMainRemark = DevName.Split('\\')[1].Trim().Split('(')[0].Trim();
            DevName = DevName.Split('\\')[0].Trim();
            //保存basic informations
            byte bytSubID = byte.Parse(DevName.Split('-')[0].ToString());
            byte bytDevID = byte.Parse(DevName.Split('-')[1].ToString());
            byte[] ArayMain = new byte[20];
            byte[] arayTmpRemark = HDLUDP.StringToByte(strMainRemark);
            byte[] ArayTmp;
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

            if (intActivePage == 0)
            {
                // 修改基本信息设备ID bacnet标准设置
                #region
                ArayTmp = new byte[4];
                ArayTmp[0] = Convert.ToByte(Address >> 24);
                ArayTmp[1] = Convert.ToByte(Address >> 16 & 0x00FF);
                ArayTmp[2] = Convert.ToByte(Address >> 8 & 0x0000FF);
                ArayTmp[3] = Convert.ToByte(Address % 256);
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xBAD0, bytSubID, bytDevID, false, true, true, false) == true)
                {
                    CsConst.myRevBuf = new byte[1200];
                }
                else return false;
                #endregion

                //bacnet 端口 IP设置
                #region
                ArayTmp = new byte[7];
                if (blnSwitch) ArayTmp[0] = 1;
                ArayTmp[5] = Convert.ToByte(intPort / 256);
                ArayTmp[6] = Convert.ToByte(intPort % 256);
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x180F, bytSubID, bytDevID, false, true, true, false) == true)
                {
                    CsConst.myRevBuf = new byte[1200];
                }
                else return false;
                #endregion
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(2);
            // 修改转换列表
            if (intActivePage == 0 || intActivePage == 1)
            {
                if (otherInfo != null && otherInfo.Count != 0)
                {
                    byte bytID = 0;
                    if (CsConst.isRestore)
                    {
                        ArayTmp = new byte[2];
                        ArayTmp[0] = Convert.ToByte(ValidCount / 256);
                        ArayTmp[1] = Convert.ToByte(ValidCount % 256);

                        if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xBAC8, bytSubID, bytDevID, false, true, true, false) == true)
                        {
                            CsConst.myRevBuf = new byte[1200];
                        }
                        else return false;
                    }
                    #region
                    foreach (OtherInfo oTmp in otherInfo)
                    {
                        ArayTmp = new byte[65];
                        ArayTmp[0] = oTmp.ID;
                        ArayTmp[1] = oTmp.BackNetIDAry[0];
                        ArayTmp[2] = oTmp.BackNetIDAry[1];
                        ArayTmp[3] = oTmp.BackNetIDAry[2];
                        ArayTmp[4] = oTmp.BackNetIDAry[3];

                        #region
                        switch (oTmp.Type)
                        {
                            case 0x000C:///场景 广播场景
                                ArayTmp[5] = 0x000C % 256;
                                ArayTmp[6] = 0x000C / 256;
                                ArayTmp[7] = 0x000D % 256;
                                ArayTmp[8] = 0x000D / 256;
                                ArayTmp[9] = 0x0002 % 256;
                                ArayTmp[10] = 0x0002 / 256;
                                ArayTmp[11] = 0x0003 % 256;
                                ArayTmp[12] = 0x0003 / 256;
                                break;
                            case 0xE014://序列
                                ArayTmp[5] = 0xE014 % 256;
                                ArayTmp[6] = 0xE014 / 256;
                                ArayTmp[7] = 0xE015 % 256;
                                ArayTmp[8] = 0xE015 / 256;
                                ArayTmp[9] = 0x001A % 256;
                                ArayTmp[10] = 0x001A/ 256;
                                ArayTmp[11] = 0x001B % 256;
                                ArayTmp[12] = 0x001B / 256;
                                break;
                            case 0x0033://单路调节 广播回路
                                ArayTmp[5] = 0x0033 % 256;
                                ArayTmp[6] = 0x0033 / 256;
                                ArayTmp[7] = 0x0034 % 256;
                                ArayTmp[8] = 0x0034 / 256;
                                ArayTmp[9] = 0x0031 % 256;
                                ArayTmp[10] = 0x0031 / 256;
                                ArayTmp[11] = 0x0032 % 256;
                                ArayTmp[12] = 0x0032 / 256;
                                break;
                            case 0xE018://通用开关
                                ArayTmp[5] = 0xE018 % 256;
                                ArayTmp[6] = 0xE018 / 256;
                                ArayTmp[7] = 0xE019 % 256;
                                ArayTmp[8] = 0xE019 / 256;
                                ArayTmp[9] = 0xE01C % 256;
                                ArayTmp[10] = 0xE01C / 256;
                                ArayTmp[11] = 0xE01D % 256;
                                ArayTmp[12] = 0xE01D / 256;
                                break;
                            case 0xF112://时间开关
                                ArayTmp[5] = 0xF112 % 256;
                                ArayTmp[6] = 0xF112 / 256;
                                ArayTmp[7] = 0xF113 % 256;
                                ArayTmp[8] = 0xF113 / 256;
                                ArayTmp[9] = 0xF116 % 256;
                                ArayTmp[10] = 0xF116 / 256;
                                ArayTmp[11] = 0xF117 % 256;
                                ArayTmp[12] = 0xF117 / 256;
                                break;
                            case 0xE3E2://窗帘开关
                                ArayTmp[5] = 0xE3E2 % 256;
                                ArayTmp[6] = 0xE3E2 / 256;
                                ArayTmp[7] = 0xE3E3 % 256;
                                ArayTmp[8] = 0xE3E3 / 256;
                                ArayTmp[9] = 0xE3E0 % 256;
                                ArayTmp[10] = 0xE3E0 / 256;
                                ArayTmp[11] = 0xE3E1 % 256;
                                ArayTmp[12] = 0xE3E1 / 256;
                                break;
                            case 0xE3D6://GPRS控制
                                ArayTmp[5] = 0xE3D6 % 256;
                                ArayTmp[6] = 0xE3D6 / 256;
                                ArayTmp[7] = 0xE3D7 % 256;
                                ArayTmp[8] = 0xE3D7 / 256;
                                ArayTmp[9] = 0xE3D4 % 256;
                                ArayTmp[10] = 0xE3D4 / 256;
                                ArayTmp[11] = 0xE3D5 % 256;
                                ArayTmp[12] = 0xE3D5 / 256;
                                break;
                            case 0xE3DA://面板控制
                                ArayTmp[5] = 0xE3DA % 256;
                                ArayTmp[6] = 0xE3DA / 256;
                                ArayTmp[7] = 0xE3DB % 256;
                                ArayTmp[8] = 0xE3DB / 256;
                                ArayTmp[9] = 0xE3D8 % 256;
                                ArayTmp[10] = 0xE3D8 / 256;
                                ArayTmp[11] = 0xE3D9 % 256;
                                ArayTmp[12] = 0xE3D9 / 256;
                                break;
                            case 0x011E://消防控制
                                ArayTmp[5] = 0x011E % 256;
                                ArayTmp[6] = 0x011E / 256;
                                ArayTmp[7] = 0x011F % 256;
                                ArayTmp[8] = 0x011F / 256;
                                ArayTmp[9] = 0x0104 % 256;
                                ArayTmp[10] = 0x0104 / 256;
                                ArayTmp[11] = 0x0105 % 256;
                                ArayTmp[12] = 0x0105 / 256;
                                break;
                            case 0x16A4://通用控制
                                ArayTmp[5] = 0x16A4 % 256;
                                ArayTmp[6] = 0x16A4 / 256;
                                ArayTmp[7] = 0x16A5 % 256;
                                ArayTmp[8] = 0x16A5 / 256;
                                ArayTmp[9] = 0x16A6 % 256;
                                ArayTmp[10] = 0x16A6 / 256;
                                ArayTmp[11] = 0x16A7 % 256;
                                ArayTmp[12] = 0x16A7 / 256;
                                break;
                            case 0x15CE://干接点
                                ArayTmp[5] = 0x15CE % 256;
                                ArayTmp[6] = 0x15CE / 256;
                                ArayTmp[7] = 0x15CF % 256;
                                ArayTmp[8] = 0x15CF / 256;
                                ArayTmp[9] = 0x15CE % 256;
                                ArayTmp[10] = 0x15CE / 256;
                                ArayTmp[11] = 0x15CF % 256;
                                ArayTmp[12] = 0x15CF / 256;
                                break;
                            case 0xE440://模拟值输出
                                ArayTmp[5] = 0x40;
                                ArayTmp[6] = 0xE4;
                                ArayTmp[7] = 0x41;
                                ArayTmp[8] = 0xE4;
                                ArayTmp[9] = 0x42;
                                ArayTmp[10] = 0xE4;
                                ArayTmp[11] = 0x43;
                                ArayTmp[12] = 0xE4;
                                break;
                            case 0xE444://模拟量设置
                                ArayTmp[5] = 0x44;
                                ArayTmp[6] = 0xE4;
                                ArayTmp[7] = 0x45;
                                ArayTmp[8] = 0xE4;
                                ArayTmp[9] = 0x46;
                                ArayTmp[10] = 0xE4;
                                ArayTmp[11] = 0x47;
                                ArayTmp[12] = 0xE4;
                                break;
                            case 0xE3E7://温度读取（1 byte）
                                ArayTmp[5] = 0xE3E7 % 256;
                                ArayTmp[6] = 0xE3E7 / 256;
                                ArayTmp[7] = 0xE3E8 % 256;
                                ArayTmp[8] = 0xE3E8 / 256;
                                ArayTmp[9] = 0xE3E5 % 256;
                                ArayTmp[10] = 0xE3E5 / 256;
                                ArayTmp[11] = 0xE3E5 % 256;
                                ArayTmp[12] = 0xE3E5 / 256;
                                break;
                            case 0x1948://温度读取（4 byte）
                                ArayTmp[5] = 0x1948 % 256;
                                ArayTmp[6] = 0x1948 / 256;
                                ArayTmp[7] = 0x1949 % 256;
                                ArayTmp[8] = 0x1949 / 256;
                                ArayTmp[9] = 0xE3E5 % 256;
                                ArayTmp[10] = 0xE3E5 / 256;
                                ArayTmp[11] = 0xE3E5 % 256;
                                ArayTmp[12] = 0xE3E5 / 256;
                                break;
                            case 0xA008://DALI灯状态
                                ArayTmp[5] = 0xA008 % 256;
                                ArayTmp[6] = 0xA008 / 256;
                                ArayTmp[7] = 0xA009 % 256;
                                ArayTmp[8] = 0xA009 / 256;
                                ArayTmp[9] = 0xA008 % 256;
                                ArayTmp[10] = 0xA008 / 256;
                                ArayTmp[11] = 0xA009 % 256;
                                ArayTmp[12] = 0xA009 / 256;
                                break;
                        }
                        #endregion
                        ArayTmp[13] = Convert.ToByte(oTmp.strDevName.Split('/')[0].ToString());
                        ArayTmp[14] = Convert.ToByte(oTmp.strDevName.Split('/')[1].ToString());
                        ArayTmp[15] = oTmp.Param1;
                        ArayTmp[16] = oTmp.Param2;
                        ArayTmp[17] = oTmp.Param3;
                        ArayTmp[18] = oTmp.Param4;
                        ArayTmp[20] = 1;
                        ArayTmp[21] = 20;
                        ArayTmp[22] = 0;

                        arayTmpRemark = new byte[40];
                        arayTmpRemark = HDLUDP.StringToByte(oTmp.Remark);
                        if (arayTmpRemark.Length > 40)
                        {
                            Array.Copy(arayTmpRemark, 0, ArayTmp, 23, 40);
                        }
                        else
                        {
                            Array.Copy(arayTmpRemark, 0, ArayTmp, 23, arayTmpRemark.Length);
                        }


                        if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xBAC2, bytSubID, bytDevID, false, true, true, false) == true)
                        {
                            CsConst.myRevBuf = new byte[1200];
                        }
                        else return false;
                        bytID++;
                        if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(100 / otherInfo.Count * bytID);
                    }
                    #endregion
                }
            }

            MyRead2UpFlags[1] = true;
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(100);
            return true;
        }


        /// <summary>
        /// 下载所有数据
        /// </summary>
        /// <param name="DevName"></param>
        /// <returns></returns>
        public bool DownloadEIBInforsFrmDevice(string DevName, int wdDeviceType, int intActivePage, int num1, int num2)
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
                DevName = bytSubID.ToString() + "-" + bytDevID.ToString() + "-" + HDLPF.Byte2String(arayRemark);
                CsConst.myRevBuf = new byte[1200];
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(1);

            if (intActivePage == 0)
            {
                // 读取基本信息bacnet地址设置
                #region
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xBACE, bytSubID, bytDevID, false, true, true, false) == true)
                {
                    Address = CsConst.myRevBuf[25] * 256 * 256 * 256
                            + CsConst.myRevBuf[26] * 256 * 256
                            + CsConst.myRevBuf[27] * 256
                            + CsConst.myRevBuf[28];
                    CsConst.myRevBuf = new byte[1200];
                }
                else return false;
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(3);
                #endregion

                //读取bacnet端口
                #region
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x180D, bytSubID, bytDevID, false, true, true, false) == true)
                {
                    blnSwitch = (CsConst.myRevBuf[26] == 1);
                    intPort = CsConst.myRevBuf[31] * 256 + CsConst.myRevBuf[32];
                }
                else return false;
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(3);
                #endregion

                // 读取转换信息列表
                //读取bacnet个数
                #region

                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xBAC6, bytSubID, bytDevID, false, true, true, false) == true)
                {
                    ValidCount = CsConst.myRevBuf[25] * 256 + CsConst.myRevBuf[26];
                    CsConst.myRevBuf = new byte[1200];
                }
                else return false;
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(3);
                #endregion

            }
            if (intActivePage == 0 || intActivePage == 1)
            {
                #region
                ArayTmp = new byte[1];
                otherInfo = new List<OtherInfo>();
                for (int i = num1; i <= num2; i++)
                {
                    OtherInfo ch = new OtherInfo();

                    ArayTmp[0] = (byte)(i);
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xBAC4, bytSubID, bytDevID, false, true, true, false) == true)
                    {
                        if (CsConst.myRevBuf != null)
                        {
                            ch.ID = CsConst.myRevBuf[26];
                            ch.BackNetIDAry[0] = CsConst.myRevBuf[27];
                            ch.BackNetIDAry[1] = CsConst.myRevBuf[28];
                            ch.BackNetIDAry[2] = CsConst.myRevBuf[29];
                            ch.BackNetIDAry[3] = CsConst.myRevBuf[30];
                            ch.Type = CsConst.myRevBuf[32] * 256 + CsConst.myRevBuf[31];
                            ch.strDevName = CsConst.myRevBuf[39].ToString() + " / " + CsConst.myRevBuf[40].ToString();
                            ch.Param1 = CsConst.myRevBuf[41];
                            ch.Param2 = CsConst.myRevBuf[42];
                            ch.Param3 = CsConst.myRevBuf[43];
                            ch.Param4 = CsConst.myRevBuf[44];
                            byte[] arayRemark = new byte[40];
                            for (int intJ = 0; intJ < 40; intJ++) { arayRemark[intJ] = CsConst.myRevBuf[49 + intJ]; }
                            ch.Remark = HDLPF.Byte2String(arayRemark);
                        }
                        CsConst.myRevBuf = new byte[1200];
                        HDLUDP.TimeBetwnNext(10);
                        otherInfo.Add(ch);
                    }
                    else return false;
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(i / 3, null);
                }
                #endregion
            }
            MyRead2UpFlags[0] = true;
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(100, null);
            return true;
        }

    }
}
