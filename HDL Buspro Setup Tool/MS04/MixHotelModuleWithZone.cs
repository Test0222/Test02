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
    [Serializable]
    public class MixHotelModuleWithZone : MS04
    {
        internal List<Area> Areas; //分区列表      

        [Serializable]
        internal class Area
        {
            public byte ID;
            public string Remark;
            public List<Scene> Scen;
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

        //<summary>
        //读取默认的MHIOU设置，将所有数据读取缓存
        //</summary>
        public override void ReadDefaultInfo(int intDeviceType)
        {
            base.ReadDefaultInfo(intDeviceType);

            Areas = new List<Area>();
        }

        public override void UploadMS04ToDevice(string strDevName, int DeviceType, int intActivePage, int num1, int num2)// 0 mean all, else that tab only
        {
            String strTmpDevName = strDevName.Split('\\')[0].Trim();
            byte bytSubID = byte.Parse(strTmpDevName.Split('-')[0].ToString());
            byte bytDevID = byte.Parse(strTmpDevName.Split('-')[1].ToString());
            
            try
            {
                base.UploadMS04ToDevice(strDevName, DeviceType, intActivePage, 0, 0);
                if (ChnList == null || ChnList.Count == 0) return;
                int wdMaxValue = ChnList.Count;

                if (intActivePage == 0 || intActivePage == 7 || intActivePage == 8)
                {
                    //修改区域信息
                    #region
                    if (intActivePage == 7 || (intActivePage ==8 && (Areas == null || Areas.Count ==0)))
                    {
                        byte[] arayArea = new byte[wdMaxValue + 3];
                        arayArea[0] = bytSubID;
                        arayArea[1] = bytDevID;

                        if (Areas == null || Areas.Count == 0)
                            arayArea[2] = 1;
                        else
                        {
                            arayArea[2] = (byte)Areas.Count;

                            foreach (Channel chn in ChnList)
                            {
                                arayArea[2 + chn.ID] = Convert.ToByte(chn.intBelongs);
                            }
                        }

                        // modify area first or nothing can be go on after that
                        if (CsConst.mySends.AddBufToSndList(arayArea, 0x0006, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == false) return;
                        CsConst.myRevBuf = new byte[1200];
                        HDLUDP.TimeBetwnNext(10);
                    }
                    #endregion

                    #region
                    Byte bytI = 0;
                    Byte[] arayRemark = new Byte[0];
                    Byte[] bytSceFlag = new Byte[Areas.Count];
                    Byte[] bytSceIDs = new Byte[Areas.Count];
                    foreach (Area area in Areas)
                    {
                        // modify area remark
                        arayRemark = new byte[21]; // 初始化数组
                        string strRemark = area.Remark;
                        byte[] arayTmp = HDLUDP.StringToByte(strRemark);
                        arayRemark[0] = area.ID;
                        arayTmp.CopyTo(arayRemark, 1);
                        if (CsConst.mySends.AddBufToSndList(arayRemark, 0xF00C, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == false) return;
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
                    if (CsConst.mySends.AddBufToSndList(bytSceFlag, 0xF053, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == false) return;
                    CsConst.myRevBuf = new byte[1200];
                    HDLUDP.TimeBetwnNext(20);

                    if (CsConst.mySends.AddBufToSndList(bytSceIDs, 0xF057, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == false) return;
                    CsConst.myRevBuf = new byte[1200];
                    HDLUDP.TimeBetwnNext(20);
                    MyRead2UpFlags[6] = true;
                    #endregion
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(30);

                    if (intActivePage == 8)
                    {
                        foreach (Area area in Areas)
                        {    // scene
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
                                if (CsConst.mySends.AddBufToSndList(arayRemark1, 0xF026, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == false) return;
                                CsConst.myRevBuf = new byte[1200];
                                HDLUDP.TimeBetwnNext(20);

                                // modify scene running time and lights
                                byte[] araySce = new byte[4 + wdMaxValue];
                                araySce[0] = area.ID;
                                araySce[1] = scen.ID;
                                araySce[2] = byte.Parse((scen.Time / 256).ToString());
                                araySce[3] = byte.Parse((scen.Time % 256).ToString());

                                int intTmp = 0;
                                foreach (Channel ch in ChnList)
                                {   //添加所在区域的亮度值
                                    if (ch.intBelongs == area.ID)
                                    {
                                        araySce[4 + intTmp] = scen.light[ch.ID - 1];
                                        intTmp++;
                                    }
                                }

                                if (CsConst.mySends.AddBufToSndList(araySce, 0x0008, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == false) return;
                                CsConst.myRevBuf = new byte[1200];
                                HDLUDP.TimeBetwnNext(20);
                                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(30 + bytI * 10 / area.Scen.Count);
                                bytI++;
                            }
                            #endregion
                            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(40);
                        }
                    }
                }
            }
            catch { }

            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(100);
        }

        /// <summary>
        /// strDevName device subnet id and device id
        /// </summary>
        /// <param name="strDevName"></param>
        public override void DownLoadInformationFrmDevice(string strDevName, int DeviceType, int intActivePage, int num1, int num2)// 0 mean all, else that tab only
        {
            if (strDevName == null) return;
            string strMainRemark = strDevName.Split('\\')[1].Trim();
            String TmpDeviceName = strDevName.Split('\\')[0].Trim();

            byte bytSubID = byte.Parse(TmpDeviceName.Split('-')[0].ToString());
            byte bytDevID = byte.Parse(TmpDeviceName.Split('-')[1].ToString());

            base.DownLoadInformationFrmDevice(strDevName, DeviceType, intActivePage, 0, 0);

            // 读取回路信息          
            Byte[] ArayTmp = new Byte[0];
            if (ChnList == null || ChnList.Count == 0) return;
            int wdMaxValue = ChnList.Count;
            
            if (intActivePage == 0 || intActivePage == 7 || intActivePage == 8)
            {
                // 读取区域信息
                #region
                Areas = new List<Area>();
                byte bytTalArea =0;
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x0004, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                {
                    bytTalArea = CsConst.myRevBuf[29];
                    if (bytTalArea == 255) bytTalArea = 0;
                    if (bytTalArea > wdMaxValue) bytTalArea = 0;
                    for (int intI = 0; intI < wdMaxValue; intI++)
                    {
                        ChnList[intI].intBelongs = CsConst.myRevBuf[30 + intI];
                    }
                    CsConst.myRevBuf = new byte[1200];
                    HDLUDP.TimeBetwnNext(1);
                    if (bytTalArea == 0)
                    {
                        for (int intI = 0; intI < ChnList.Count; intI++)
                        {
                            ChnList[intI].intBelongs = 0;
                        }
                    }
                }
                else return;
                #endregion

                #region
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
                    else return;
                    CsConst.myRevBuf = new byte[1200];
                    HDLUDP.TimeBetwnNext(1);
                    area.Scen = new List<Scene>();
                    Areas.Add(area);
                }
                #endregion
                if (intActivePage == 0 || intActivePage == 8)
                {
                    for (byte intI = 0; intI < bytTalArea; intI++)
                    {
                        // 读取场景信息
                        Areas[intI].Scen = new List<Scene>();
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
                                    else return;
                                    Areas[intI].Scen.Add(scen);
                                    CsConst.myRevBuf = new byte[1200];
                                    HDLUDP.TimeBetwnNext(1);
                                }
                            }
                            else return;
                               
                        }
                        #endregion

                        //表示掉电前状态
                        #region
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
                                else return;

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
                        #endregion

                        if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(20 + 60 + intI / 13, null);
                    }
                    MyRead2UpFlags[7] = true;
                }

                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(60, null);
            }

            MyRead2UpFlags[0] = true;
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(100, null);
        }


    }

}
