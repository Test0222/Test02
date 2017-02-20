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
    public class MS04GenerationOneT : MS04
    {
        internal List<TemperatureSource> temperatureSensors; //最多个回路       

        //<summary>
        //读取默认的MHIOU设置，将所有数据读取缓存
        //</summary>
        public override void ReadDefaultInfo(int intDeviceType)
        {
            base.ReadDefaultInfo(intDeviceType);

            temperatureSensors = new List<TemperatureSource>();
        }

        //<summary>
        //读取数据库面板设置，将所有数据读至缓存
        //</summary>
        public override void ReadMS04FrmDBTobuf(int DIndex, int wdMaxValue)
        {
            base.ReadMS04FrmDBTobuf(DIndex, wdMaxValue);

            if (temperatureSensors != null && temperatureSensors.Count > 0)
            {
                foreach (TemperatureSource tmp in temperatureSensors)
                {
                    //tmp.ReadTemperatureSensorSettingInformation(Byte
                }
            }
            temperatureSensors = new List<TemperatureSource>();
            #region
            //read all channels information save them to the buffer
            
            #endregion
        }

        //<summary>
        //保存数据库面板设置，将所有数据保存
        //</summary>
        public override void SaveSendIRToDB(int intDeviceType)
        {
            base.SaveSendIRToDB(intDeviceType);

            //insert new channel information
            if (temperatureSensors != null)
            {
               
            }
        }

        public override void UploadMS04ToDevice(string strDevName, int intDeviceType, int intActivePage, int num1, int num2)// 0 mean all, else that tab only
        {
            string strMainRemark = strDevName.Split('\\')[1].Trim().Split('(')[0].Trim();
            strDevName = strDevName.Split('\\')[0].Trim();
            byte bytSubID = byte.Parse(strDevName.Split('-')[0].ToString());
            byte bytDevID = byte.Parse(strDevName.Split('-')[1].ToString());

            base.UploadMS04ToDevice(strDevName, intDeviceType, intActivePage,0,0);

            if (intActivePage == 0 || intActivePage == 1)
            {
                //modify channel information 
                #region
                if (temperatureSensors != null)
                {
                    foreach (TemperatureSource tmp in temperatureSensors)
                    {
                        tmp.ModifyTemperatureSensorSettingInformation(bytSubID, bytDevID, intDeviceType);
                    }
                }
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
            if (strDevName == null) return;
            string strMainRemark = strDevName.Split('\\')[1].Trim();
            String TmpDeviceName = strDevName.Split('\\')[0].Trim();

            byte bytSubID = byte.Parse(TmpDeviceName.Split('-')[0].ToString());
            byte bytDevID = byte.Parse(TmpDeviceName.Split('-')[1].ToString());

            base.DownLoadInformationFrmDevice(strDevName, intDeviceType, intActivePage,0,0);

            // 读取回路信息
            temperatureSensors = new List<TemperatureSource>();
            if (MSKeys == null || MSKeys.Count == 0) return;
            for (Byte bId = 0; bId < MSKeys.Count; bId++)
            {
                #region
                if (MSKeys[bId].Mode == 100)
                {
                    TemperatureSource tmp = new TemperatureSource();
                    tmp.channelID = MSKeys[bId].ID;
                    tmp.ReadTemperatureSensorSettingInformation(bytSubID, bytDevID, intDeviceType);
                    temperatureSensors.Add(tmp);
                }
                #endregion
            }
            MyRead2UpFlags[0] = true;
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(100, null);
        }


    }

}
