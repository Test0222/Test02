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
    public class MS04GenerationOne2R : MS04
    {
        internal List<RelayChannel> Chans; //12个回路       

        //<summary>
        //读取默认的MHIOU设置，将所有数据读取缓存
        //</summary>
        public override void ReadDefaultInfo(int intDeviceType)
        {
            base.ReadDefaultInfo(intDeviceType);
            int wdMaxValue = DeviceTypeList.GetMaxValueFromPublicModeGroup(intDeviceType);
            Chans = new List<RelayChannel>();

            for (int i = 0; i < wdMaxValue; i++)
            {
                RelayChannel Chn = new RelayChannel();
                #region
                Chn.Remark = "Relay Chn ";
                Chn.LoadType = 0;
                Chn.OnDelay = 0;
                Chn.ProtectDelay = 0;
                Chn.intBelongs = 0;
                #endregion
            }
        }

        //<summary>
        //读取数据库面板设置，将所有数据读至缓存
        //</summary>
        public override void ReadMS04FrmDBTobuf(int DIndex, int wdMaxValue)
        {
            base.ReadMS04FrmDBTobuf(DIndex, wdMaxValue);

            Chans = new List<RelayChannel>();
            #region
            //read all channels information save them to the buffer
            string strsql = "select * from dbDevChnsDR where ID=" + DIndex.ToString() + " order by ChannelID";
            OleDbDataReader drChns = DataModule.SearchAResultSQLDB(strsql);
            if (drChns != null && drChns.HasRows)
            {
                drChns.Read();

                string[] chanID = drChns.GetString(1).Split('-');
                string[] remark = drChns.GetString(2).Split('-');
                string[] loadType = drChns.GetString(3).Split('-');
                string[] MinValue = drChns.GetString(4).Split('-');
                string[] MaxValue = drChns.GetString(5).Split('-');
                string[] MaxLevel = drChns.GetString(6).Split('-');
                string[] Belongs = drChns.GetString(7).Split('-');
                //回路
                for (int i = 0; i < chanID.Length; i++)
                {
                    RelayChannel Ch = new RelayChannel();
                    Ch.Remark = remark[0];
                    Ch.LoadType = int.Parse(loadType[0]);
                    Ch.OnDelay = int.Parse(MinValue[0]);
                    Ch.ProtectDelay = int.Parse(MaxValue[0]);
                    Chans.Add(Ch);
                    drChns.Close();
                }
            }
            #endregion
        }

        //<summary>
        //保存数据库面板设置，将所有数据保存
        //</summary>
        public override void SaveSendIRToDB(int intDeviceType) 
        {
            base.SaveSendIRToDB(intDeviceType);

            //insert new channel information
            if (Chans != null)
            {
                #region
                string str = "";
                string chid = "", remark = "", loadType = "", MinValue = "", MaxValue = "", MaxLevel = "";
                string temp = "", Belongs = "";
                byte bytI = 0;
                foreach (RelayChannel ch in Chans)
                {
                    chid += bytI.ToString() + temp;
                    remark += ch.Remark + temp;
                    loadType += ch.LoadType.ToString() + temp;
                    MinValue += ch.OnDelay.ToString() + temp;
                    MaxValue += ch.ProtectDelay.ToString() + temp;
                    bytI++;
                }

                str = "insert into dbDevChnsDR(ID,ChannelID,Remark,LoadType,Low,High,MaxValue,AreaID) values(" + DIndex.ToString() + ",'" + chid
                    + "','" + remark + "','" + loadType + "','" + MinValue + "','" + MaxValue + "','" + MaxLevel + "','" + Belongs + "')";

                DataModule.ExecuteSQLDatabase(str);
                #endregion
            }
        }

        public override void UploadMS04ToDevice(string strDevName, int intDeviceType, int intActivePage, int num1, int num2)// 0 mean all, else that tab only
        {
            string strMainRemark = strDevName.Split('\\')[1].Trim().Split('(')[0].Trim();
            string strTmpDevName = strDevName.Split('\\')[0].Trim();
            byte bytSubID = byte.Parse(strTmpDevName.Split('-')[0].ToString());
            byte bytDevID = byte.Parse(strTmpDevName.Split('-')[1].ToString());

            base.UploadMS04ToDevice(strDevName, intDeviceType, intActivePage, 0, 0);

            if (intActivePage == 0 || intActivePage == 7)
            {               
                //modify channel information 
                #region
                byte[] arayRemark = new byte[21];// used for restore remark
                byte[] arayLoadType = new byte[1];
                byte[] arayLimitL = new byte[2]; arayLimitL[0] = 0;
                byte[] arayLimitH = new byte[2]; arayLimitH[0] = 1;
                byte[] arayMaxLevel = new byte[1];
                byte bytI = 1;
                foreach (RelayChannel ch in Chans)
                {
                    // modify the chns remark
                    arayRemark = new byte[21]; // 初始化数组
                    string strRemark = ch.Remark;
                    if (ch.Remark == null) strRemark = "";
                    byte[] arayTmp = HDLUDP.StringToByte(strRemark);

                    arayRemark[0] = bytI;
                    HDLSysPF.CopyRemarkBufferToSendBuffer(arayTmp, arayRemark,  1);

                    CsConst.mySends.AddBufToSndList(arayRemark, 0x3318, bytSubID, bytDevID, false, false, false, false);
                    HDLUDP.TimeBetwnNext(arayRemark.Length);


                    arayLoadType[0] = byte.Parse(ch.OFFDelay.ToString());
                    arayMaxLevel[0] = byte.Parse(ch.OnDelay.ToString());
                    arayLimitL[0] = byte.Parse(ch.ProtectDelay.ToString());
                    bytI++;
                }

                // modify the off delay
                CsConst.mySends.AddBufToSndList(arayLoadType, 0xF086, bytSubID, bytDevID, false, false, false, false);
                HDLUDP.TimeBetwnNext(arayLoadType.Length);

                // modify the on delay 
                CsConst.mySends.AddBufToSndList(arayMaxLevel, 0xF04F, bytSubID, bytDevID, false, false, false, false);
                HDLUDP.TimeBetwnNext(arayLoadType.Length);

                // modify the protect delay
                CsConst.mySends.AddBufToSndList(arayLimitL, 0xF041, bytSubID, bytDevID, false, false, false, false);
                HDLUDP.TimeBetwnNext(arayLoadType.Length);


                MyRead2UpFlags[1] = true;
                #endregion
            }

            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(100);
        }

        /// <summary>
        /// strDevName device subnet id and device id
        /// </summary>
        /// <param name="strDevName"></param>
        public override void DownLoadInformationFrmDevice(string strDevName, int intDeviceType, int intActivePage, int num1, int num2)// 0 mean all, else that tab only
        {
            Boolean BlnIsSuccess = false;
            if (strDevName == null) return;
            string strMainRemark = strDevName.Split('\\')[1].Trim();
            DeviceName = strDevName.Split('\\')[0].Trim();

            byte bytSubID = byte.Parse(DeviceName.Split('-')[0].ToString());
            byte bytDevID = byte.Parse(DeviceName.Split('-')[1].ToString());
            byte[] ArayTmp = new Byte[1];

            base.DownLoadInformationFrmDevice(strDevName, intDeviceType, intActivePage,0,0);

            // 读取回路信息
            Chans = new List<RelayChannel>();
            #region
            int wdMaxValue = 1;
            for (int i = 0; i < wdMaxValue; i++)
            {
                RelayChannel Ch = new RelayChannel();
                Ch.LoadType = 0;
                Ch.OnDelay = 0;
                Ch.ProtectDelay = 0;

                ArayTmp[0] = (byte)(i + 1);
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x331A, bytSubID, bytDevID, false, false, true, false) == true)
                {
                    if (CsConst.myRevBuf != null)
                    {
                        byte[] arayRemark = new byte[20];
                        HDLSysPF.CopyRemarkBufferFrmMyRevBuffer(CsConst.myRevBuf, arayRemark, 26);
                        Ch.Remark = HDLPF.Byte2String(arayRemark);
                    }
                    CsConst.myRevBuf = new byte[1200];
                }
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(i * (50 / wdMaxValue), null);
                Chans.Add(Ch);
            }
            ArayTmp = null;
            // read off delay
            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xF084, bytSubID, bytDevID, false, false, true, true) == true)
            {
                for (int intI = 0; intI < wdMaxValue; intI++)
                {
                    Chans[intI].OFFDelay = CsConst.myRevBuf[25 + intI];
                    if (Chans[intI].LoadType == 255 || Chans[intI].LoadType > CsConst.LoadType.Length - 1) Chans[intI].LoadType = 0;
                }
                CsConst.myRevBuf = new byte[1200];
            }

            // read on delay
            ArayTmp = null;
            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xF04D, bytSubID, bytDevID, false, false, true, true) == true)
            {
                for (int intI = 0; intI < wdMaxValue; intI++)
                {
                    Chans[intI].OnDelay = CsConst.myRevBuf[25 + intI];
                }
                CsConst.myRevBuf = new byte[1200];
            }

            // read protoct delay
            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xF03F, bytSubID, bytDevID, false, false, true, false) == true)
            {
                for (int intI = 0; intI < wdMaxValue; intI++)
                {
                    Chans[intI].ProtectDelay = CsConst.myRevBuf[25 + intI];
                }
                CsConst.myRevBuf = new byte[1200];
            }

            #endregion
            MyRead2UpFlags[0] = true;
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(100, null);
            BlnIsSuccess = true;
            return;
        }


    }

}
