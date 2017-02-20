using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;

namespace HDL_Buspro_Setup_Tool
{
    [Serializable]
   public class TempSensor:HdlDeviceBackupAndRestore
   {
        public int DIndex;  ////工程内唯一编号
        public string Devname;
        internal List<Chn> Chans;

        public bool[] MyRead2UpFlags = new bool[2]; //前面四个表示读取 后面表示上传

        [Serializable]
        internal class Chn
        {
            public byte bytTheType; // 电阻类型 0表示关闭此通道，返回温度值为0
                                    //1表示HDL热敏电阻， 表示HDL热敏电阻，默认设置
                                    //2表示M公司的热敏电阻，除了0之外表示开启当前通道
                                    //3表示可以输入分度表的,只有这种模式下才能保存R0，R10，R25 
            public byte[] bytRValue;

            public bool blnBroadTemp;
            public byte bytSubID; //子网ID
            public byte bytDevID; //设备ID
            public byte byAdjustVal; //温度补偿
        }

        ////<summary>
        ////读取默认的温度传感器设置，将所有数据读取缓存
        ////</summary>
        public void ReadDefaultInfo(int wdMaxValue)
        {
            ////set all channel to default 0,未定义，0.0 ，0.0
            Chans = new List<Chn>();

            for (int i = 0; i < wdMaxValue; i++)
            {
                Chn ch = new Chn();
                ch.bytTheType = 1;
                ch.bytRValue = new byte[]{29100 / 256, 29100 % 256,18570 / 256,18570 % 256, 10000 / 256, 10000 % 256};

                ch.blnBroadTemp = false;
                ch.bytSubID = 255;
                ch.bytDevID = 255;
                ch.byAdjustVal = 0;

                Chans.Add(ch);
            }
        }

        //<summary>
        //读取数据库里的调光模块的设置，将所有数据读取缓存
        //</summary>
        public void ReadInfoForDB(int id)
        {
            Chans = new List<Chn>();

            //read all channels information save them to the buffer
            string str = "select * from dbTempSensor where DIndex=" + id.ToString() + " order by ChnID";
            OleDbDataReader dr =DataModule.SearchAResultSQLDB(str);

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    Chn Tmp = new Chn();
                    Tmp.bytTheType = dr.GetByte(2);

                    Tmp.bytRValue = new byte[6];
                    Tmp.bytRValue[0] = byte.Parse((dr.GetInt16(3) / 256).ToString());
                    Tmp.bytRValue[1] = byte.Parse((dr.GetInt16(3) % 256).ToString());
                    Tmp.bytRValue[2] = byte.Parse((dr.GetInt16(4) / 256).ToString());
                    Tmp.bytRValue[3] = byte.Parse((dr.GetInt16(4) % 256).ToString());
                    Tmp.bytRValue[4] = byte.Parse((dr.GetInt16(5) / 256).ToString());
                    Tmp.bytRValue[5] = byte.Parse((dr.GetInt16(5) % 256).ToString());

                    Tmp.blnBroadTemp = dr.GetBoolean(6);
                    Tmp.bytSubID = dr.GetByte(7);
                    Tmp.bytDevID = dr.GetByte(8);
                    Tmp.byAdjustVal = dr.GetByte(9);

                    Chans.Add(Tmp);
                }
            }
            dr.Close();
        }

        //<summary>
        //将缓存里所有调光模块的设置存在数据库
        //</summary>
        public void SaveInfoToDb()
        {
            byte bytChnID = 1;
            foreach (Chn ch in Chans)
            {
                string str = "select * from dbTempSensor where DIndex=" + DIndex.ToString() + " and ChnID = " + bytChnID.ToString();
                if (DataModule.IsExitstInDatabase(str) == false)
                {
                    str = string.Format("insert into dbTempSensor(DIndex,ChnID,ThemType,R0,R10,R25,blnBrdTemp,SubNetID,DevID,AdjustValue) values({0},{1},{2},{3},{4},{5},{6},{7},{8},{9})",
                        DIndex, bytChnID, ch.bytTheType, ch.bytRValue[0] * 256 + ch.bytRValue[1], ch.bytRValue[2] * 256 + ch.bytRValue[3],
                         ch.bytRValue[4] * 256 + ch.bytRValue[5],ch.blnBroadTemp,ch.bytSubID,ch.bytDevID,ch.byAdjustVal);
                }
                else
                {
                    str = string.Format("update dbTempSensor set ThemType={0},R0={1},R10={2},R25={3},blnBrdTemp={4},SubNetID={5},DevID{6},AdjustValue={7} where DIndex={8} and and ChnID ={9}",
                        ch.bytTheType, ch.bytRValue[0] * 256 + ch.bytRValue[1], ch.bytRValue[2] * 256 + ch.bytRValue[3],
                        ch.bytRValue[4] * 256 + ch.bytRValue[5], ch.blnBroadTemp, ch.bytSubID, ch.bytDevID, ch.byAdjustVal, DIndex, bytChnID);
                }
                DataModule.ExecuteSQLDatabase(str);
                bytChnID++;
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
        public bool UploadDimmerInfosToDevice(string DevName)
        {
            //保存回路信息
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
                MessageBox.Show("This module is not online or the address is another one, please check it !!");
                return false;
            }

            byte bytChID = 1;
            byte[] arayTmp = new byte[16];

            foreach (Chn ch in Chans)
            {   // modify the chns remark
                byte[] arayRemark = new byte[8]; // 初始化数组
                arayRemark[0] = bytChID;
                arayRemark[1] = ch.bytTheType;
                arayRemark[2] = ch.bytRValue[0];
                arayRemark[3] = ch.bytRValue[1];
                arayRemark[4] = ch.bytRValue[2];
                arayRemark[5] = ch.bytRValue[3];
                arayRemark[6] = ch.bytRValue[4];
                arayRemark[7] = ch.bytRValue[5];

                CsConst.mySends.AddBufToSndList(arayRemark, 0x1C06, bytSubID, bytDevID, false,true,true,false);
                HDLUDP.TimeBetwnNext(arayRemark.Length);

                if (ch.blnBroadTemp == true)
                {
                    arayTmp[(bytChID - 1) * 4] = 1;
                }
                else
                {
                    arayTmp[(bytChID - 1) * 4] = 0;
                }
                arayTmp[(bytChID - 1) * 4 + 1] = ch.bytSubID;
                arayTmp[(bytChID - 1) * 4 + 2] = ch.bytDevID;
                arayTmp[(bytChID - 1) * 4 + 3] = ch.byAdjustVal;
                
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(bytChID * 25, null);
                bytChID++;
            }

            // modify the load type
            CsConst.mySends.AddBufToSndList(arayTmp, 0xF014, bytSubID, bytDevID, false, true, true, false);
            HDLUDP.TimeBetwnNext(arayTmp.Length);
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(100, null);
            MyRead2UpFlags[1] = true;
            return true;
        }

       /// <summary>
       /// device name subnet id  + device id  + remark
       /// </summary>
       /// <param name="DevName"></param>
        public void DownLoadInformationFrmDevice(string DevName)
        {
            string strMainRemark = DevName.Split('\\')[1].Trim();
            DevName = DevName.Split('\\')[0].Trim();

            byte bytSubID = byte.Parse(DevName.Split('-')[0].ToString());
            byte bytDevID = byte.Parse(DevName.Split('-')[1].ToString());
            byte[] ArayTmp = new byte[1];
            
            Chans = new List<Chn>();
            #region
            for (byte bytI = 1; bytI < 5; bytI++)
            {
                Chn oTmp = new Chn();
                ArayTmp[0] = bytI; 
                // read broadcast informations
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1C00, bytSubID, bytDevID, false, true, true, false) == true)
                {
                    if (CsConst.myRevBuf != null)
                    {
                        oTmp.blnBroadTemp = (CsConst.myRevBuf[26] == 1);
                        oTmp.bytSubID = CsConst.myRevBuf[27];
                        oTmp.bytDevID = CsConst.myRevBuf[27];
                        oTmp.byAdjustVal = CsConst.myRevBuf[27];
                    }
                    CsConst.myRevBuf = new byte[1200];
                    HDLUDP.TimeBetwnNext(10);
                }
                else
                {
                    MessageBox.Show("This module is not online or the address is another one, please check it !!");
                    return;
                }

                // read other setups
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1C04, bytSubID, bytDevID, false, true, true, false) == true)
                {
                    if (CsConst.myRevBuf != null)
                    {
                        oTmp.bytTheType = CsConst.myRevBuf[26];
                        oTmp.bytRValue = new byte[6];

                        for (int intI = 0; intI < 6; intI++) { oTmp.bytRValue[intI] = CsConst.myRevBuf[27 + intI]; }
                    }
                    CsConst.myRevBuf = new byte[1200];
                    HDLUDP.TimeBetwnNext(10);
                }
               if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(bytI * 25, null);
               Chans.Add(oTmp);
            }
            #endregion
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(100, null);
            MyRead2UpFlags[0] = true;
        }
    }
}
