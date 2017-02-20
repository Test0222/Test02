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
    public class MS04GenerationOneD : MS04
    {
        internal List<DimmerChannelGenerationTwo> Chans; //12个回路       

        //<summary>
        //读取默认的MHIOU设置，将所有数据读取缓存
        //</summary>
        public override void ReadDefaultInfo(int intDeviceType)
        {
            base.ReadDefaultInfo(intDeviceType);

            Chans = new List<DimmerChannelGenerationTwo>();
            #region
            foreach (DimmerChannel oChn in Chans)
            {
                oChn.remark = "Relay Chn ";
                oChn.loadType = 0;
                oChn.minValue = 0;
                oChn.maxValue = 100;
                oChn.maxLevel = 100;
                oChn.belongArea = 0;
            }
            #endregion
        }

        //<summary>
        //读取数据库面板设置，将所有数据读至缓存
        //</summary>
        public override void ReadMS04FrmDBTobuf(int DIndex, int wdMaxValue)
        {
            base.ReadMS04FrmDBTobuf(DIndex, wdMaxValue);

            Chans = new List<DimmerChannelGenerationTwo>();
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

                drChns.Close();
            }
            #endregion
        }

        //<summary>
        //保存数据库面板设置，将所有数据保存
        //</summary>
        public override void SaveSendIRToDB(int intDeviceType)
        {
            base.SaveSendIRToDB(intDeviceType);

            int wdMaxValue = DeviceTypeList.GetMaxValueFromPublicModeGroup(intDeviceType);
            Chans = new List<DimmerChannelGenerationTwo>();

            for (int i = 0; i < wdMaxValue; i++)
            {
                DimmerChannel oChan = Chans[i];
                string str = "";
                string chid = "", remark = "", loadType = "", MinValue = "", MaxValue = "", MaxLevel = "";
                string temp = "", Belongs = "";
                if (Chans == null) return;
                temp = "-";


                chid += "1" + temp;
                remark += oChan.remark + temp;
                loadType += oChan.loadType.ToString() + temp;
                MinValue += oChan.minValue.ToString() + temp;
                MaxValue += oChan.maxValue.ToString() + temp;
                MaxLevel += oChan.maxLevel.ToString() + temp;
                Belongs += oChan.belongArea.ToString() + temp;

                str = "insert into dbDevChnsDR(ID,ChannelID,Remark,LoadType,Low,High,MaxValue,AreaID) values(" + "1".ToString() + ",'" + chid
                    + "','" + remark + "','" + loadType + "','" + MinValue + "','" + MaxValue + "','" + MaxLevel + "','" + Belongs + "')";

                DataModule.ExecuteSQLDatabase(str);
            }
        }

        public override void UploadMS04ToDevice(string strDevName, int intDeviceType, int intActivePage, int num1, int num2)// 0 mean all, else that tab only
        {
            string strMainRemark = strDevName.Split('\\')[1].Trim().Split('(')[0].Trim();
            String TmpstrDevName = strDevName.Split('\\')[0].Trim();
            byte bytSubID = byte.Parse(TmpstrDevName.Split('-')[0].ToString());
            byte bytDevID = byte.Parse(TmpstrDevName.Split('-')[1].ToString());

            base.UploadMS04ToDevice(strDevName, intDeviceType, intActivePage,0,0);

            if (intActivePage == 0 || intActivePage == 3)
            {
                //modify channel information 
                #region
                byte[] arayRemark = new byte[21];// used for restore remark
                byte[] arayDimPro = new byte[Chans.Count];
                byte[] arayLimitL = new byte[1 + Chans.Count]; arayLimitL[0] = 0;
                byte[] arayLimitH = new byte[1 + Chans.Count]; arayLimitH[0] = 1;
                byte[] arayMaxLevel = new byte[Chans.Count];
                int startChId = 0;
                foreach (DimmerChannelGenerationTwo oChn in Chans)
                {
                    arayDimPro[0 + startChId] = byte.Parse(oChn.dimmingProfile.ToString());
                    arayMaxLevel[0 + startChId] = byte.Parse(oChn.maxLevel.ToString());
                    arayLimitL[1 + startChId] = byte.Parse(oChn.minValue.ToString());
                    arayLimitH[1 + startChId] = byte.Parse(oChn.maxValue.ToString());

                    // modify the chns remark
                    arayRemark = new byte[21]; // 初始化数组
                    string strRemark = oChn.remark;
                    if (oChn.remark == null) strRemark = "";
                    byte[] arayTmp = HDLUDP.StringToByte(strRemark);

                    arayRemark[0] = 1;
                    HDLSysPF.CopyRemarkBufferToSendBuffer(arayTmp, arayRemark, 1);
                    CsConst.mySends.AddBufToSndList(arayRemark, 0xF010, bytSubID, bytDevID, false, false, true, false);
                    HDLUDP.TimeBetwnNext(arayRemark.Length);
                    startChId++;
                }

                // modify the on Low limit
                CsConst.mySends.AddBufToSndList(arayLimitL, 0xF018, bytSubID, bytDevID, false, false, true, false);
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(3);

                // modify the High Limit
                CsConst.mySends.AddBufToSndList(arayLimitH, 0xF018, bytSubID, bytDevID, false, false, true, false);
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(4);

                // modify the max level 
                CsConst.mySends.AddBufToSndList(arayMaxLevel, 0xF022, bytSubID, bytDevID, false, false, true, false);
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(5);

                //modify dimmng profile
                if (CsConst.mySends.AddBufToSndList(arayDimPro, 0x3364, bytSubID, bytDevID, false, true, true, true) == false) return;
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(9);
                HDLUDP.TimeBetwnNext(20);
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
            string TmpDeviceName = strDevName.Split('\\')[0].Trim();

            byte bytSubID = byte.Parse(TmpDeviceName.Split('-')[0].ToString());
            byte bytDevID = byte.Parse(TmpDeviceName.Split('-')[1].ToString());
            byte[] ArayTmp = new Byte[1];

            base.DownLoadInformationFrmDevice(strDevName, intDeviceType, intActivePage,0,0);

            int wdMaxValue = DeviceTypeList.GetMaxValueFromPublicModeGroup(intDeviceType); 

            if (intActivePage == 0 || intActivePage == 3)
            {
                // 读取回路信息
                Chans = new List<DimmerChannelGenerationTwo>();
                #region

                for (int i = 0; i < wdMaxValue; i++)
                {
                    DimmerChannelGenerationTwo oChn = new DimmerChannelGenerationTwo();
                    oChn.loadType = 0;
                    oChn.minValue = 0;
                    oChn.maxValue = 100;
                    oChn.maxLevel = 100;

                    ArayTmp[0] = (byte)(i + 1);
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xF00E, bytSubID, bytDevID, false, false, false, false) == true)
                    {
                        if (CsConst.myRevBuf != null)
                        {
                            byte[] arayRemark = new byte[20];
                            HDLSysPF.CopyRemarkBufferFrmMyRevBuffer(CsConst.myRevBuf, arayRemark, 26);
                            oChn.remark = HDLPF.Byte2String(arayRemark);
                        }
                        CsConst.myRevBuf = new byte[1200];
                        HDLUDP.TimeBetwnNext(10);
                    }
                    Chans.Add(oChn);
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress((i + 95), null);
                }

                ArayTmp = null;

                // read low limit
                ArayTmp = new byte[1];
                ArayTmp[0] = 0;
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xF016, bytSubID, bytDevID, false, false, true, false) == true)
                {
                    for (int i = 0; i < wdMaxValue; i++)
                    {
                        Chans[i].minValue = CsConst.myRevBuf[25];
                    }
                    CsConst.myRevBuf = new byte[1200];
                    HDLUDP.TimeBetwnNext(50);
                }

                // read high limit
                ArayTmp[0] = 1;
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xF016, bytSubID, bytDevID, false, false, true, false) == true)
                {
                    for (int i = 0; i < wdMaxValue; i++)
                    {
                        Chans[i].maxValue = CsConst.myRevBuf[25];
                    }
                    CsConst.myRevBuf = new byte[1200];
                    HDLUDP.TimeBetwnNext(50);
                }

                // read High Level
                ArayTmp = null;
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xF020, bytSubID, bytDevID, false, false, true, false) == true)
                {
                    for (int i = 0; i < wdMaxValue; i++)
                    {
                        Chans[i].maxLevel = CsConst.myRevBuf[25];
                    }
                    CsConst.myRevBuf = new byte[1200];
                    HDLUDP.TimeBetwnNext(10);
                }

                //读取调光曲线
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x3366, bytSubID, bytDevID, false, true, true, true) == true)
                {
                    if (CsConst.myRevBuf != null)
                    {
                        for (int i = 0; i < wdMaxValue; i++)
                        {
                            Chans[i].dimmingProfile = CsConst.myRevBuf[25 + i];
                        }
                    }
                    CsConst.myRevBuf = new byte[1200];
                    HDLUDP.TimeBetwnNext(10);
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(39, null);
                }
            }
            #endregion

            MyRead2UpFlags[0] = true;
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(100, null);
            BlnIsSuccess = true;
            return;
        }


    }

}
