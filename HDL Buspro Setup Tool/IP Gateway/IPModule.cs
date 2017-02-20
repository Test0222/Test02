using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.Xml;
using System.IO;
using System.Windows.Forms;

namespace HDL_Buspro_Setup_Tool
{
    [Serializable]
    public class IPModule : HdlDeviceBackupAndRestore
    {
        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public int DIndex;
        public string DeviceName; // 子网ID，设备ID，备注
        public NetworkInformation netWork;

        public byte bytWorkType;
        public string strGroup;
        public string strPrjName;
        public string strUser;
        public string strPWD;
        public string strServer1;
        public int intPort1;
        public string strServer2;
        public int intPort2;
        public byte bytTimer;
        public byte bytEnDHCP;
        internal List<RFBlock> MyBlocks; // 拦截列表

        public Byte[] RemoteControllers = new Byte[8];

        internal HDLButton[] MyRemoteControllers; // 第一个遥控器按键列表

        public bool[] MyRead2UpFlags = new bool[6] { false, false, false, false,false,false };

        // 无线网关
        public byte bytStatus; //当前状态
        public byte bytCFre;
        public byte bytChns;
        public byte[] bytPWD;
        public int intSSID;  //  SSID
        public byte bytGateSubID;
        public byte bytGateDevID;

        public byte isEnable;  // mesh or bridge
        public byte bytPassSub; // 通过的子网ID  255表示全部

        //<summary>
        //读取默认的IP module 设置，将所有数据读取缓存
        //</summary>
        public void ReadDefaultInfo()
        {
            netWork = new NetworkInformation(); 

             bytWorkType = 0; //0 局域网，1 p2p, 2 server
             strGroup = CsConst.MyUnnamed ;
             strPrjName = CsConst.MyUnnamed ;
             strUser = "user";
             strPWD = "user";
             strServer1 = "115.29.251.24";
             intPort1 = 9999;
             strServer2 = "59.41.254.6";
             intPort2 = 9999;
             bytTimer = 1;
             bytEnDHCP = 0;
        }

        //<summary>
        //读取IP设置，将所有数据读至缓存
        //</summary>
        public void ReadCurtainFrmDBTobuf(int intDIndex)
        {
            // read current device type new or old one
            string str = "select * from dbIPModule where DIndex=" + intDIndex.ToString();
            OleDbDataReader dr = DataModule.SearchAResultSQLDB(str, CsConst.mstrCurPath);
            
            if (dr == null) return;
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    bytWorkType = dr.GetByte(5); //0 局域网，1 p2p, 2 server
                    strGroup = dr.GetString(6);
                    strPrjName = dr.GetString(7);
                    strUser = dr.GetString(8);
                    strPWD = dr.GetString(9);
                    strServer1 = dr.GetString(10);
                    intPort1 = dr.GetInt32(11);
                    strServer2 = dr.GetString(12);
                    intPort2 = dr.GetInt32(13);
                    bytTimer = dr.GetByte(14);
                    bytEnDHCP = dr.GetByte(15); 
                }
                dr.Close();
            }
        }

        //<summary>
        //保存IP设置，将所有数据保存
        //</summary>
        public void SaveCurtainToDB()
        {
            //// delete all old information and refresh the database
            string strsql = string.Format("delete * from dbIPModule where DIndex=" + DIndex.ToString());
            DataModule.ExecuteSQLDatabase(strsql);

            //// insert curtain Type
            //strsql = string.Format("insert into dbIPModule(DIndex,strIP,strRouterIP,strMaskIP,strMAC,bytWorkType,strGroup,strPrjName,strUser,strPWD,strServer1,intPort1,strServer2,intPort2,"
            //       + "bytTimer,bytEnDHCP ) values({0},'{1}','{2}','{3}','{4}',{5},'{6}','{7}','{8}','{9}','{10}',{11},'{12}',{13},{14},{15})", DIndex,strIP,strRouterIP,strMaskIP,strMAC,
            //         bytWorkType,strGroup,strPrjName,strUser,strPWD,strServer1,intPort1,strServer2,intPort2,bytTimer,bytEnDHCP);
            //DataModule.ExecuteSQLDatabase(strsql);
        }


        /// <summary>
        /// 上传设置到窗帘模块
        /// </summary>
        /// <param name="DIndex"></param>
        /// <param name="DevID"></param>
        /// <param name="DeviceType"></param>
        /// <param name="DevName"></param>
        /// <returns></returns>
        public bool UploadCurtainInfosToDevice(string DevName, int PageIndex, int DeviceType)
        {
            string strMainRemark = DevName.Split('\\')[1].Trim().Split('(')[0].Trim();
            String TmpDevName = DevName.Split('\\')[0].Trim();

            //保存basic informations
            byte SubNetID = byte.Parse(TmpDevName.Split('-')[0].ToString());
            byte DeviceID = byte.Parse(TmpDevName.Split('-')[1].ToString());
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

            if (CsConst.mySends.AddBufToSndList(ArayMain, 0x0010, SubNetID, DeviceID, false, true, true, false) == false)
            {
                return false;
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(10, null);
            if (PageIndex == 0 || PageIndex == 1)
            {
                netWork.ModifyNetworkInfomation(SubNetID, DeviceID);
                #region
                byte[] arayTmp = new byte[57];
                arayTmp[0] = bytWorkType;

                arayTmpRemark = HDLUDP.StringToByte(strGroup);
                Array.Copy(arayTmpRemark, 0, arayTmp, 1, arayTmpRemark.Length);
                arayTmpRemark = HDLUDP.StringToByte(strPrjName);
                Array.Copy(arayTmpRemark, 0, arayTmp, 21, arayTmpRemark.Length);
                arayTmpRemark = HDLUDP.StringToByte(strUser);
                Array.Copy(arayTmpRemark, 0, arayTmp, 41, arayTmpRemark.Length);
                arayTmpRemark = HDLUDP.StringToByte(strPWD);
                Array.Copy(arayTmpRemark, 0, arayTmp, 49, arayTmpRemark.Length);

                if (CsConst.mySends.AddBufToSndList(arayTmp, 0x3003, SubNetID, DeviceID, false, true, true, false) == false)
                {
                    return false;
                }
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(20, null);

                arayTmp = new byte[14];
                string[] strTmp = strServer1.Split('.');
                for (int i = 0; i < 4; i++) { arayTmp[i] = byte.Parse(strTmp[i].ToString()); }
                arayTmp[4] = (byte)(intPort1 / 256);
                arayTmp[5] = (byte)(intPort1 % 256);

                strTmp = strServer2.Split('.');
                for (int i = 0; i < 4; i++) { arayTmp[i + 6] = byte.Parse(strTmp[i].ToString()); }
                arayTmp[10] = (byte)(intPort2 / 256);
                arayTmp[11] = (byte)(intPort2 % 256);
                arayTmp[12] = bytEnDHCP;
                arayTmp[13] = bytTimer;


                if (CsConst.mySends.AddBufToSndList(arayTmp, 0x3005, SubNetID, DeviceID, false, true, true, false) == false)
                {
                    return false;
                }
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(30, null);
                #endregion
            }

            if (PageIndex == 0 || PageIndex == 2)
            {
                //上传拦截列表
                #region
                byte bytI = 0;
                if (MyBlocks != null)
                {
                    foreach (RFBlock Tmp in MyBlocks)
                    {
                        Tmp.ModifyfBlockSetupInformation(SubNetID, DeviceID,bytI);
                        if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(50 + bytI);
                        bytI++;
                    }
                }
                #endregion
            }

            if (PageIndex == 0 || PageIndex == 3) // 无线遥控器或者无源开关
            {
                #region
                // 上传无源开关的地址
                byte[] RemoteAddress = new Byte[11];
                RemoteControllers.CopyTo(RemoteAddress, 3);
                if (CsConst.mySends.AddBufToSndList(RemoteAddress, 0xE012, SubNetID, DeviceID, false, true, true, false)) return false;

                Byte ValidRemoteNumber = 4;
                //判断地址是不是全部一样 一样保存一个即可
                if (((RemoteAddress[3] == RemoteAddress[5]) && (RemoteAddress[5] == RemoteAddress[7]) && (RemoteAddress[7] == RemoteAddress[9])) &&
                    ((RemoteAddress[4] == RemoteAddress[6]) && (RemoteAddress[6] == RemoteAddress[8]) && (RemoteAddress[8] == RemoteAddress[10])))
                {
                    ValidRemoteNumber = 1;
                }

                for (Byte i = 0; i < ValidRemoteNumber; i++)
                {
                    //无效地址不保存
                    if (RemoteAddress[3 + i * 2] == 0 && RemoteAddress[4 + i * 2] == 0) continue;
                    if (RemoteAddress[3 + i * 2] == 255 && RemoteAddress[4 + i * 2] == 255) continue;

                    Byte[] arayKeyMode = new Byte[IPmoduleDeviceTypeList.HowManyButtonsEachPage];
                    Byte[] arayKeyMutex = new Byte[IPmoduleDeviceTypeList.HowManyButtonsEachPage];
                    Byte[] arayKeyDimmer = new Byte[IPmoduleDeviceTypeList.HowManyButtonsEachPage];
                    Byte[] arayKeyLED = new Byte[IPmoduleDeviceTypeList.HowManyButtonsEachPage];

                    for (int j = 0; j < IPmoduleDeviceTypeList.HowManyButtonsEachPage; j++)
                    {
                        HDLButton TmpKey = MyRemoteControllers[i * IPmoduleDeviceTypeList.HowManyButtonsEachPage + j];
                        // key mode and dimmer valid
                        arayKeyMode[TmpKey.ID - 1] = TmpKey.Mode;
                        arayKeyMutex[TmpKey.ID - 1] = TmpKey.bytMutex;
                        arayKeyDimmer[TmpKey.ID - 1] = byte.Parse(((TmpKey.IsDimmer << 4) + TmpKey.SaveDimmer).ToString());
                        arayKeyLED[TmpKey.ID - 1] = (byte)(TmpKey.IsLEDON);

                        if (TmpKey.Mode == 0 || TmpKey.Mode > 30) continue;

                        TmpKey.UploadButtonRemarkAndCMDToDevice(SubNetID, DeviceID, DeviceType, i + 1, 255);
                        if (TmpKey.ID + 2 < 15)
                            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(2 + TmpKey.ID);
                    }
                    // upload all key mode
                    if (CsConst.mySends.AddBufToSndList(arayKeyMode, 0xE00A, SubNetID, DeviceID, false, true, true, true) == false) return false;
                    HDLUDP.TimeBetwnNext(64);

                    SaveButtonDimFlagToDeviceFrmBuf(i, SubNetID, DeviceID, DeviceType);
                    CsConst.myRevBuf = new byte[1200];
                }
                #endregion
            }

            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(100, null);
            return true;
        }

        /// <summary>
        /// devname device name 
        /// </summary>
        /// <param name="DevName"></param>
        public void DownloadZaudioFrmDeviceToBuf(string DevName, int PageIndex,int DeviceType)
        {
            string strMainRemark = DevName.Split('\\')[1].Trim();
            String TmpDevName = DevName.Split('\\')[0].Trim();

            //保存basic informations网络信息
            byte SubNetID = byte.Parse(TmpDevName.Split('-')[0].ToString());
            byte DeviceID = byte.Parse(TmpDevName.Split('-')[1].ToString());
            byte[] ArayTmp = null;

            String TmpRemark = HDLSysPF.ReadDeviceMainRemark(SubNetID, DeviceID);
            DeviceName = SubNetID.ToString() + "-" + DeviceID.ToString() + "\\" + TmpRemark;

            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(10);
            if (PageIndex == 0 || PageIndex == 1)
            {
                if (netWork == null) netWork = new NetworkInformation();
                netWork.readNetworkInfomation(SubNetID, DeviceID);
                #region
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x3007, SubNetID, DeviceID, false, true, true, false) == true)
                {
                    bytWorkType = CsConst.myRevBuf[25];

                    byte[] arayRemark = new byte[20];
                    for (int intI = 0; intI < 20; intI++) { arayRemark[intI] = CsConst.myRevBuf[26 + intI]; }
                    strGroup = HDLPF.Byte2String(arayRemark);
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(20);
                    arayRemark = new byte[20];
                    for (int intI = 0; intI < 20; intI++) { arayRemark[intI] = CsConst.myRevBuf[46 + intI]; }
                    strPrjName = HDLPF.Byte2String(arayRemark);
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(30);
                    arayRemark = new byte[8];
                    for (int intI = 0; intI < 8; intI++) { arayRemark[intI] = CsConst.myRevBuf[66 + intI]; }
                    strUser = HDLPF.Byte2String(arayRemark);

                    arayRemark = new byte[8];
                    for (int intI = 0; intI < 8; intI++) { arayRemark[intI] = CsConst.myRevBuf[74 + intI]; }
                    strPWD = HDLPF.Byte2String(arayRemark);
                    CsConst.myRevBuf = new byte[1200];
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(40);
                }
                else return;

                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x3009, SubNetID, DeviceID, false, true, true, false) == true)
                {
                    strServer1 = CsConst.myRevBuf[25].ToString("d3") + "." + CsConst.myRevBuf[26].ToString("d3")
                          + "." + CsConst.myRevBuf[27].ToString("d3") + "." + CsConst.myRevBuf[28].ToString("d3");
                    intPort1 = CsConst.myRevBuf[29] * 256 + CsConst.myRevBuf[30];

                    strServer2 = CsConst.myRevBuf[31].ToString("d3") + "." + CsConst.myRevBuf[32].ToString("d3")
                          + "." + CsConst.myRevBuf[33].ToString("d3") + "." + CsConst.myRevBuf[34].ToString("d3");
                    intPort2 = CsConst.myRevBuf[35] * 256 + CsConst.myRevBuf[36];

                    bytEnDHCP = CsConst.myRevBuf[37];
                    bytTimer = CsConst.myRevBuf[38];
                    CsConst.myRevBuf = new byte[1200];
                }
                else return;
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(50);
                #endregion
                MyRead2UpFlags[0] = true;
            }

            if (PageIndex == 0 || PageIndex == 2)
            {
                if (!IPmoduleDeviceTypeList.IpModuleV3TimeZoneUrl.Contains(DeviceType)) //bus版不支持子网过滤
                {
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1D46, SubNetID, DeviceID, false, true, true, false) == true)
                    {
                        isEnable = CsConst.myRevBuf[25];
                        bytPassSub = CsConst.myRevBuf[26];
                    }
                }
                //读取拦截列表
                #region
                MyBlocks = new List<RFBlock>();
                for (byte bytI = 0; bytI < 16; bytI++)
                {
                    RFBlock Tmp = new RFBlock();
                    if ( Tmp.ReadRfBlockSetupInformation(SubNetID,DeviceID,bytI) == true)
                    {
                        MyBlocks.Add(Tmp);
                    }
                    else return;
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(50+ bytI);
                }
                #endregion
                MyRead2UpFlags[1] = true;
            }

            //下载无线遥控器地址 
            if (PageIndex == 0 || PageIndex == 3)
            {
                Byte[] RemoteAddress = new Byte[11];
                MyRemoteControllers = new HDLButton[48 * 4];
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xE010, SubNetID, DeviceID, false, true, true, false) == true)
                {
                    Array.Copy(CsConst.myRevBuf, 28, RemoteControllers, 0, 8);
                    RemoteControllers.CopyTo(RemoteAddress,3);
                }

                Byte ValidRemoteNumber = 4;
                //判断地址是不是全部一样 一样保存一个即可
                if (((RemoteAddress[3] == RemoteAddress[5]) && (RemoteAddress[5] == RemoteAddress[7]) && (RemoteAddress[7] == RemoteAddress[9])) &&
                    ((RemoteAddress[4] == RemoteAddress[6]) && (RemoteAddress[6] == RemoteAddress[8]) && (RemoteAddress[8] == RemoteAddress[10])))
                {
                    ValidRemoteNumber = 1;
                }

                //测试按键是不是支持目标有效无效的读取
                Boolean blnIsSupportE474 = false;
                blnIsSupportE474 = CsConst.mySends.AddBufToSndList(ArayTmp, 0xE474, SubNetID, DeviceID, false, false, true, false);

                for (Byte i = 0; i < ValidRemoteNumber; i++)
                {
                    //无效地址不保存
                    if (RemoteAddress[3 + i * 2] == 0 && RemoteAddress[4 + i * 2] == 0) continue;
                    if (RemoteAddress[3 + i * 2] == 255 && RemoteAddress[4 + i * 2] == 255) continue;
                    //读取模式是否有效
                    Byte[] arayKeyMode = new byte[IPmoduleDeviceTypeList.HowManyButtonsEachPage]; //按键模式
                    Byte[] arayKeyDimmer = new byte[IPmoduleDeviceTypeList.HowManyButtonsEachPage];  // 要不要调光，要不要保存调光

                    arayKeyMode = ReadButtonModeFrmDeviceToBuf(i, SubNetID, DeviceID);
                    arayKeyDimmer = ReadButtonDimFlagFrmDeviceToBuf(i, SubNetID, DeviceID); 

                    if (arayKeyMode == null) return;

                    for (int j = 0; j < 48; j++)
                    {
                        HDLButton oKey = MyRemoteControllers[IPmoduleDeviceTypeList.HowManyButtonsEachPage * i +j];
                        oKey = new HDLButton();
                        oKey.ID = Convert.ToByte(j + 1);
                        oKey.Mode = arayKeyMode[j];
                        oKey.IsDimmer = (Byte) (arayKeyDimmer[j] >>4);
                        oKey.SaveDimmer = (Byte)(arayKeyDimmer[j] & 0x01);

                        oKey.ReadButtonRemarkAndCMDFromDevice(SubNetID, DeviceID, DeviceType, i, 255, blnIsSupportE474,0,0);
                        if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(60 + j / 2);
                        MyRemoteControllers[IPmoduleDeviceTypeList.HowManyButtonsEachPage * i + j] = oKey;
                    }
                }
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(100);
        }

        public Byte[] ReadButtonDimFlagFrmDeviceToBuf(Byte RemoteControllerID, Byte bytSubID, Byte sourceDeviceIDvID)
        {
            Byte[] ArayTmp = new Byte[1];
            ArayTmp[0] = RemoteControllerID;
            Byte[] arayKeyDim = new Byte[IPmoduleDeviceTypeList.HowManyButtonsEachPage];

            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xE134, bytSubID, sourceDeviceIDvID, false, true, true, true) == true)
            {
                for (int intJ = 0; intJ < IPmoduleDeviceTypeList.HowManyButtonsEachPage; intJ++)
                {
                    arayKeyDim[intJ] = CsConst.myRevBuf[25 + intJ];
                }
                CsConst.myRevBuf = new byte[1200];
            }
            return arayKeyDim;
        }

        public Byte[] ReadButtonModeFrmDeviceToBuf(Byte RemoteControllerID,Byte bytSubID,Byte sourceDeviceIDvID)
        {
            Byte[] ArayTmp = new Byte[1];
            ArayTmp[0] = RemoteControllerID;
            Byte[] arayKeyMode = new Byte[IPmoduleDeviceTypeList.HowManyButtonsEachPage];

            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xE008, bytSubID, sourceDeviceIDvID, false, true, true, true) == true)
            {
                for (int intJ = 0; intJ < IPmoduleDeviceTypeList.HowManyButtonsEachPage; intJ++)
                {
                    arayKeyMode[intJ] = CsConst.myRevBuf[25 + intJ];
                }
                CsConst.myRevBuf = new byte[1200];
            }
            return arayKeyMode;
        }

        public Boolean SaveButtonModeToDeviceFrmBuf(Byte RemoteControllerID, Byte bytSubID, Byte sourceDeviceIDvID, int DeviceType)
        {
            Boolean blnSuccessModify = false;
            Byte[] arayKeyMode = new Byte[IPmoduleDeviceTypeList.HowManyButtonsEachPage + 1];
            arayKeyMode[IPmoduleDeviceTypeList.HowManyButtonsEachPage] = RemoteControllerID;
            for (Byte bytI = 0; bytI < IPmoduleDeviceTypeList.HowManyButtonsEachPage; bytI++)
            {
                // key mode
                HDLButton oTmpButton = MyRemoteControllers[RemoteControllerID * IPmoduleDeviceTypeList.HowManyButtonsEachPage + bytI];
                arayKeyMode[bytI] = oTmpButton.Mode; 
            }

            // upload all key mode
            blnSuccessModify =CsConst.mySends.AddBufToSndList(arayKeyMode, 0xE00A, bytSubID, sourceDeviceIDvID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType));
            return blnSuccessModify;
        }

        public Boolean SaveButtonDimFlagToDeviceFrmBuf(Byte RemoteControllerID, Byte bytSubID, Byte sourceDeviceIDvID, int DeviceType)
        {
            Boolean blnSuccessModify = false;
            Byte[] arayKeyMode = new Byte[IPmoduleDeviceTypeList.HowManyButtonsEachPage + 1];
            arayKeyMode[IPmoduleDeviceTypeList.HowManyButtonsEachPage] = RemoteControllerID;
            for (Byte bytI = 0; bytI < IPmoduleDeviceTypeList.HowManyButtonsEachPage; bytI++)
            {
                // key mode
                HDLButton oTmpButton = MyRemoteControllers[RemoteControllerID * IPmoduleDeviceTypeList.HowManyButtonsEachPage + bytI];
                arayKeyMode[bytI] = (Byte)((oTmpButton.IsDimmer << 4) + oTmpButton.SaveDimmer) ;
            }

            // upload all key mode
            blnSuccessModify = CsConst.mySends.AddBufToSndList(arayKeyMode, 0xE136, bytSubID, sourceDeviceIDvID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType));
            return blnSuccessModify;
        }

    }
}
