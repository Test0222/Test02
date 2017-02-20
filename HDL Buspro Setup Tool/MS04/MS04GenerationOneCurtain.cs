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
    public class MS04GenerationOneCurtain : MS04
    {
        internal BasicCurtain Chans; //12个回路       

        //<summary>
        //读取默认的MHIOU设置，将所有数据读取缓存
        //</summary>
        public override void ReadDefaultInfo(int intDeviceType)
        {
            base.ReadDefaultInfo(intDeviceType);

            Chans = new BasicCurtain();
        }

        //<summary>
        //读取数据库面板设置，将所有数据读至缓存
        //</summary>
        public override void ReadMS04FrmDBTobuf(int DIndex, int wdMaxValue)
        {
            base.ReadMS04FrmDBTobuf(DIndex, wdMaxValue);

            Chans = new BasicCurtain();
            #region
            String strsql = string.Format("select * from dbClassInfomation where DIndex={0} and ClassID={1} order by SenNum", DIndex, 1);
            OleDbDataReader dr = DataModule.SearchAResultSQLDB(strsql, CsConst.mstrCurPath);
            if (dr != null)
            {
                while (dr.Read())
                {
                    string str = dr.GetValue(5).ToString();
                    Chans.runTime = Convert.ToByte(str.Split('-')[0].ToString());
                    Chans.onDelay = Convert.ToByte(str.Split('-')[1].ToString());
                    Chans.offDelay = Convert.ToByte(str.Split('-')[2].ToString());
                    Chans.remark = dr.GetValue(4).ToString();
                }
                dr.Close();
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
                String strParam = Chans.runTime.ToString() + "-" + Chans.onDelay.ToString()
                                + "-" + Chans.offDelay.ToString();
                String strsql = string.Format("Insert into dbClassInfomation(DIndex,ClassID,ObjectID,SenNum,Remark,strParam1) values ({0},{1},{2},{3},'{4}','{5}')",
                            DIndex, 1, 0, 0, Chans.remark, strParam);
                DataModule.ExecuteSQLDatabase(strsql);
                #endregion
            }
        }

        public override void UploadMS04ToDevice(string strDevName, int intDeviceType, int intActivePage, int num1, int num2)// 0 mean all, else that tab only
        {
            string strMainRemark = strDevName.Split('\\')[1].Trim();
            String strTmpDevName = strDevName.Split('\\')[0].Trim();
            byte bytSubID = byte.Parse(strTmpDevName.Split('-')[0].ToString());
            byte bytDevID = byte.Parse(strTmpDevName.Split('-')[1].ToString());

            base.UploadMS04ToDevice(strDevName, intDeviceType, intActivePage, 0, 0);

            if (intActivePage == 0 || intActivePage == 2)
            {
                //modify channel information 
                #region
                Chans.ModifyCurtainSetupInformation(bytSubID, bytDevID, 1, intDeviceType);

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
            byte[] ArayTmp = null;

            base.DownLoadInformationFrmDevice(strDevName, intDeviceType, intActivePage, 0, 0);

            // 读取回路信息
            Chans = new BasicCurtain();
            Chans.ReadCurtainSetupInformation(bytSubID, bytDevID, 1, intDeviceType);
            MyRead2UpFlags[0] = true;
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(100, null);
            BlnIsSuccess = true;
            return ;
        }


    }

}
