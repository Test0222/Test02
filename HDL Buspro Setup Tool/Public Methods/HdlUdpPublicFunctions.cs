using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace HDL_Buspro_Setup_Tool
{
    class HdlUdpPublicFunctions
    {
        
        private static DataGridView oTmpDataGrid = null;
        public delegate void FlushClient(object[] tmp1); //代理
        public delegate void FlushClientDataGrid(List<String> tmp1, int rowId, int clomunId, Boolean bIsSuccess); //代理

        public static void ThreadFunction(object[] obj)
        {
            oTmpDataGrid.Rows.Add(obj);
        }

        public static void RefreshDataGridTableDisplay(List<String> obj, int rowId, int clomunId,Boolean bIsSuccess)
        {
            foreach (String Tmp in obj)
            {
                if (bIsSuccess == true)
                {
                    oTmpDataGrid.Rows[rowId].DefaultCellStyle.ForeColor = Color.Green;
                }
                else
                {
                    oTmpDataGrid.Rows[rowId].DefaultCellStyle.ForeColor = Color.Red;
                }

                oTmpDataGrid.Rows[rowId].Cells[clomunId].Value =Tmp; 

                clomunId++;
            }
        }

        public static Boolean LocateDeviceWhereItIs(Byte bSubNetId, Byte bDeviceId)
        {
            byte[] arayTmp = new byte[1];
            arayTmp[0] = 6;
            return CsConst.mySends.AddBufToSndList(arayTmp, 0xE442, bSubNetId, bDeviceId, false, false, true, false);
        }

        public static void StartSimpleSearchWay(int OperationCode, DataGridView dgOnline)
        {
            //初始化全部数据
            dgOnline.Rows.Clear();
            //UDPReceive.ClearQueueData();
            CsConst.myOnlines = new List<DevOnLine>();
            CsConst.simpleSearchDevicesList = new List<HdlDevice>();

            AddDeviceToList(OperationCode, dgOnline);

           // if (oTmpDataGrid != null) oTmpDataGrid.Dispose();
        }

        private static void AddDeviceToList(int OperationCode, DataGridView dgOnline)
        {
            Boolean BlnDealWithFunctionGroups = (OperationCode == 0xE548);
            try
            {
                CsConst.FastSearch = true;
                Byte RepeatSendTimes = 1;
                Random R1 = new Random();
                Random R2 = new Random();
                List<Byte> SearchDeviceAddressesList = new List<byte>();
                R1.Next();
                Byte[] RandomWhenSearch = new Byte[2];
                R1.NextBytes(RandomWhenSearch);
                Byte[] DataSendBuffer = new Byte[2 + SearchDeviceAddressesList.Count];
                RandomWhenSearch.CopyTo(DataSendBuffer, 0);
            RepeatSearch:
                if (SearchDeviceAddressesList != null && SearchDeviceAddressesList.Count >0)
                {
                    DataSendBuffer = new Byte[2 + SearchDeviceAddressesList.Count];
                    RandomWhenSearch.CopyTo(DataSendBuffer, 0);
                    SearchDeviceAddressesList.CopyTo(DataSendBuffer, 2);
                    SearchDeviceAddressesList = new List<byte>();
                }
                CsConst.mySends.AddBufToSndList(DataSendBuffer, OperationCode, 255, 255, false, false, false, false);
                // 广播搜索方式汇集
                #region
                DateTime d1, d2;
                
                d1 = DateTime.Now;
                d2 = DateTime.Now;
                while (UDPReceive.receiveQueue.Count > 0 || HDLSysPF.Compare(d2, d1) < 2000)
                {
                    d2 = DateTime.Now;
                    if (UDPReceive.receiveQueue.Count > 0)
                    {
                        byte[] readData = UDPReceive.receiveQueue.Dequeue();
                        if (readData.Length < 22 || readData.Length <= 45) continue;
                        if (readData[21] * 256 + readData[22] == OperationCode + 1)
                        {
                            int index = 1;
                            while (CsConst.AddedDevicesIndexGroup.Contains(index))
                            {
                                index++;
                            }

                            DevOnLine temp = new DevOnLine();
                            byte[] arayRemark = new byte[20];
                            HDLSysPF.CopyRemarkBufferFrmMyRevBuffer(readData, arayRemark, 25);

                            string strRemark = HDLPF.Byte2String(arayRemark);
                            temp.bytSub = readData[17];
                            temp.bytDev = readData[18];
                            temp.DevName = temp.bytSub.ToString() + "-" + temp.bytDev.ToString() + "\\" + strRemark.ToString();
                            temp.DeviceType = readData[19] * 256 + readData[20];
                            temp.strVersion = "Unread";
                            temp.intDIndex = index;

                            bool isAdd = true;
                            if (CsConst.myOnlines.Count > 0)
                            {
                                foreach (DevOnLine tmp in CsConst.myOnlines)
                                {
                                    if (temp.DevName == tmp.DevName && temp.bytSub == tmp.bytSub &&
                                        temp.bytDev == tmp.bytDev && temp.DeviceType == tmp.DeviceType)
                                    {
                                        isAdd = false;
                                        break;
                                    }
                                }
                            }

                            if (isAdd)
                            {
                                HDLSysPF.AddItsDefaultSettings(temp.DeviceType, temp.intDIndex, temp.DevName);
                                SearchDeviceAddressesList.Add(temp.bytSub);
                                SearchDeviceAddressesList.Add(temp.bytDev);
                                CsConst.myOnlines.Add(temp);
                                CsConst.AddedDevicesIndexGroup.Add(index);
                                if (BlnDealWithFunctionGroups)
                                {
                                    arayRemark = new byte[20];
                                    HDLSysPF.CopyRemarkBufferFrmMyRevBuffer(readData, arayRemark, 27);
                                    String tmpRemark = HDLPF.Byte2String(arayRemark);
                                    Byte subnetId = readData[17];
                                    Byte deviceId = readData[18];
                                    Int32 deviceType = readData[19] * 256 + readData[20];
                                    AddSimpleFUnctionListToPublicClasses(readData, tmpRemark, subnetId, deviceId,deviceType);
                                }
                                else
                                {
                                    #region
                                    if (dgOnline != null)
                                    {
                                        oTmpDataGrid = dgOnline;
                                        String[] strTmp = DeviceTypeList.GetDisplayInformationFromPublicModeGroup(temp.DeviceType);
                                        String deviceModel = strTmp[0];
                                        String deviceDescription = strTmp[1];

                                        if (temp.strVersion == null) temp.strVersion = "";
                                        string strVersion = temp.strVersion;
                                        if (strVersion == "") strVersion = "Unread";
                                        object[] obj = {false, HDL_Buspro_Setup_Tool.Properties.Resources.OK, dgOnline.RowCount+1, temp.bytSub, temp.bytDev, 
                                                           deviceModel, temp.DevName.Split('\\')[1].ToString(), 
                                                           deviceDescription, strVersion, temp.intDIndex.ToString(),""};

                                        InvokeDataGridWhenNeedsRefreshDisplay(oTmpDataGrid, obj);
                                    }
                                    #endregion
                                }
                            }
                        }
                    }
                }

                RepeatSendTimes++;
                if (RepeatSendTimes<=4) goto RepeatSearch;
                #endregion              
            }
        
            catch
            {
            }
            CsConst.FastSearch = false;
        }

        public static void InvokeDataGridWhenNeedsRefreshDisplay(DataGridView oTmpDataGrid, Object[] obj)
        {
            try
            {
                if (oTmpDataGrid.InvokeRequired)//等待异步
                {
                    FlushClient fc = new FlushClient(ThreadFunction);
                    oTmpDataGrid.Invoke(fc, new object[] { obj });
                }
                else
                {
                    oTmpDataGrid.Rows.Add(obj);
                }
            }
            catch { }
        }

        public static void UpdateDataGridWhenNeedsRefreshDisplay(DataGridView TmpDataGrid, List<String> obj,int rowId, int clomunId, Boolean bIsSuccess)
        {
            if (TmpDataGrid == null) return;
            oTmpDataGrid = TmpDataGrid;
            try
            {
                if (oTmpDataGrid.InvokeRequired)//等待异步
                {
                    FlushClientDataGrid fc = new FlushClientDataGrid(RefreshDataGridTableDisplay);
                    oTmpDataGrid.Invoke(fc, obj,rowId,clomunId,bIsSuccess);
                }
                else
                {
                    foreach (String Tmp in obj)
                    {
                        if (bIsSuccess == true)
                        {
                            oTmpDataGrid.Rows[rowId].DefaultCellStyle.ForeColor = Color.Green;
                        }
                        else
                        {
                            oTmpDataGrid.Rows[rowId].DefaultCellStyle.ForeColor = Color.Red;
                        }
                        oTmpDataGrid.Rows[rowId].Cells[clomunId].Value = Tmp.ToString();
                        clomunId++;
                    }
                }
            }
            catch { }
        }

        public static void OldVersionDevicesGetSimpleListManually()
        {
            if (CsConst.myOnlines == null || CsConst.myOnlines.Count == 0) return;
            try
            {
                CsConst.simpleSearchDevicesList = new List<HdlDevice>();
                foreach( DevOnLine oDev in CsConst.myOnlines)
                {
                    String sSimpleFunctionList = DeviceTypeList.GetSimpleFunctionListFromPublicModeGroup(oDev.DeviceType);
                    if (sSimpleFunctionList != "" && sSimpleFunctionList !="0 0 0" )
                    {
                        String[] arrSimpleFunctionList = sSimpleFunctionList.Split(' ').ToArray();
                        List<Byte> arrByteSimpleFunctionList = new List<Byte>();

                        foreach (String Tmp in arrSimpleFunctionList)
                        {
                            if (Tmp != "") arrByteSimpleFunctionList.Add(Convert.ToByte(Tmp));
                        }

                        AddSimpleFUnctionListFromDatabaseToPublicClasses(arrByteSimpleFunctionList.ToArray(),oDev.DevName,oDev.bytSub,oDev.bytDev,oDev.DeviceType);
                    }
                }
            }
            catch
            { }
        }

        private static void AddSimpleFUnctionListToPublicClasses(Byte[] FunctionsGroupE549Respond, String tmpRemark, Byte subnetId, Byte deviceId, int deviceType)
        {
            if (FunctionsGroupE549Respond == null) return;
            if (CsConst.simpleSearchDevicesList == null) CsConst.simpleSearchDevicesList = new List<HdlDevice>();
            try
            {
                Byte packetLength =(Byte)(FunctionsGroupE549Respond[16] - 20 - 13);

                for (Byte bTmp = 0; bTmp < packetLength / 3; bTmp++)
                {
                    Byte bigType = FunctionsGroupE549Respond[47 + bTmp * 3];

                    if (bigType == SimpleFunctionsList.lightBigType)
                    {
                        #region
                        if (SimpleFunctionsList.simpleLightsList == null) SimpleFunctionsList.simpleLightsList = new List<HdlSimpleLights>();
                        HdlSimpleLights temp =new HdlSimpleLights();
                        temp.subnetID = subnetId;
                        temp.deviceID = deviceId;
                        temp.remark = tmpRemark;
                        temp.DeviceType = deviceType;
                        temp.bigType = FunctionsGroupE549Respond[47 + bTmp * 3];
                        temp.smallType = FunctionsGroupE549Respond[48 + bTmp * 3];
                        temp.sumChns = FunctionsGroupE549Respond[49 + bTmp * 3];
                        temp.sumLightChns = new List<DimmerChannel>();
                        for (Byte bytChnID = 1; bytChnID <= temp.sumChns; bytChnID++)
                        {
                            DimmerChannel TmpLight = new DimmerChannel();
                            TmpLight.id = bytChnID;
                            temp.sumLightChns.Add(TmpLight);
                        }
                        CsConst.simpleSearchDevicesList.Add(temp);
                        #endregion
                    }
                    else if (bigType == SimpleFunctionsList.curtainBigType)
                    {
                        #region
                        if (SimpleFunctionsList.simpleCurtainsList == null) SimpleFunctionsList.simpleCurtainsList = new List<HdlSimpleCurtains>();
                        HdlSimpleCurtains temp = new HdlSimpleCurtains();
                        temp.subnetID = subnetId;
                        temp.deviceID = deviceId;
                        temp.remark = tmpRemark;
                        temp.DeviceType = deviceType;
                        temp.bigType = FunctionsGroupE549Respond[47 + bTmp * 3];
                        temp.smallType = FunctionsGroupE549Respond[48 + bTmp * 3];
                        temp.sumChns = FunctionsGroupE549Respond[49 + bTmp * 3];
                        temp.sumCurtainChns = new List<BasicCurtain>();
                        for (Byte bytChnID = 1; bytChnID <= temp.sumChns; bytChnID++)
                        {
                            BasicCurtain TmpLight = new BasicCurtain();
                            temp.sumCurtainChns.Add(TmpLight);
                        }
                        CsConst.simpleSearchDevicesList.Add(temp);
                        #endregion
                    }
                    else if (bigType == SimpleFunctionsList.sensorBigType)
                    {
                        #region
                        if (SimpleFunctionsList.simpleSensorsList == null) SimpleFunctionsList.simpleSensorsList = new List<HdlSimpleSensors>();
                        HdlSimpleSensors temp = new HdlSimpleSensors();
                        temp.subnetID = subnetId;
                        temp.deviceID = deviceId;
                        temp.remark = tmpRemark;
                        temp.DeviceType = deviceType;
                        temp.bigType = FunctionsGroupE549Respond[47 + bTmp * 3];
                        temp.smallType = FunctionsGroupE549Respond[48 + bTmp * 3];
                        temp.sumChns = FunctionsGroupE549Respond[49 + bTmp * 3];
                        temp.sensorParameters = new List<byte[]>();
                        for (Byte bytChnID = 1; bytChnID <= temp.sumChns; bytChnID++)
                        {
                            Byte[] TmpLight = new Byte[5];
                            temp.sensorParameters.Add(TmpLight);
                        }
                        CsConst.simpleSearchDevicesList.Add(temp);
                        #endregion
                    }
                    else if (bigType == SimpleFunctionsList.buttonBigType)
                    {
                        #region
                        if (SimpleFunctionsList.simpleButtonsList == null) SimpleFunctionsList.simpleButtonsList = new List<HdlSimpleButtons>();
                        HdlSimpleButtons temp = new HdlSimpleButtons();
                        temp.subnetID = subnetId;
                        temp.deviceID = deviceId;
                        temp.remark = tmpRemark;
                        temp.DeviceType = deviceType;
                        temp.bigType = FunctionsGroupE549Respond[47 + bTmp * 3];
                        temp.smallType = FunctionsGroupE549Respond[48 + bTmp * 3];
                        temp.sumChns = FunctionsGroupE549Respond[49 + bTmp * 3];
                        temp.sumButtonDrys = new List<HDLButton>();
                        for (Byte bytChnID = 1; bytChnID <= temp.sumChns; bytChnID++)
                        {
                            HDLButton TmpLight = new HDLButton();
                            temp.sumButtonDrys.Add(TmpLight);
                        }
                        CsConst.simpleSearchDevicesList.Add(temp);
                        #endregion
                    }                    
                }

            }
            catch
            { }
        }

        private static void AddSimpleFUnctionListFromDatabaseToPublicClasses(Byte[] FunctionsGroupE549Respond, String tmpRemark, Byte subnetId, Byte deviceId, int deviceType)
        {
            if (FunctionsGroupE549Respond == null) return;
            if (CsConst.simpleSearchDevicesList == null) CsConst.simpleSearchDevicesList = new List<HdlDevice>();
            try
            {
                Byte packetLength = (Byte)FunctionsGroupE549Respond.Length;

                for (Byte bTmp = 0; bTmp < packetLength / 3; bTmp++)
                {
                    Byte bigType = FunctionsGroupE549Respond[0 + bTmp * 3];

                    if (bigType == SimpleFunctionsList.lightBigType)
                    {
                        #region
                        if (SimpleFunctionsList.simpleLightsList == null) SimpleFunctionsList.simpleLightsList = new List<HdlSimpleLights>();
                        HdlSimpleLights temp = new HdlSimpleLights();
                        temp.subnetID = subnetId;
                        temp.deviceID = deviceId;
                        temp.remark = tmpRemark;
                        temp.DeviceType = deviceType;
                        temp.bigType = FunctionsGroupE549Respond[0 + bTmp * 3];
                        temp.smallType = FunctionsGroupE549Respond[1 + bTmp * 3];
                        temp.sumChns = FunctionsGroupE549Respond[2 + bTmp * 3];
                        temp.sumLightChns = new List<DimmerChannel>();
                        for (Byte bytChnID = 1; bytChnID <= temp.sumChns; bytChnID++)
                        {
                            DimmerChannel TmpLight = new DimmerChannel();
                            TmpLight.id = bytChnID;
                            temp.sumLightChns.Add(TmpLight);
                        }
                        CsConst.simpleSearchDevicesList.Add(temp);
                        #endregion
                    }
                    else if (bigType == SimpleFunctionsList.curtainBigType)
                    {
                        #region
                        if (SimpleFunctionsList.simpleCurtainsList == null) SimpleFunctionsList.simpleCurtainsList = new List<HdlSimpleCurtains>();
                        HdlSimpleCurtains temp = new HdlSimpleCurtains();
                        temp.subnetID = subnetId;
                        temp.deviceID = deviceId;
                        temp.remark = tmpRemark;
                        temp.DeviceType = deviceType;
                        temp.bigType = FunctionsGroupE549Respond[0 + bTmp * 3];
                        temp.smallType = FunctionsGroupE549Respond[1 + bTmp * 3];
                        temp.sumChns = FunctionsGroupE549Respond[2 + bTmp * 3];
                        temp.sumCurtainChns = new List<BasicCurtain>();
                        for (Byte bytChnID = 1; bytChnID <= temp.sumChns; bytChnID++)
                        {
                            BasicCurtain TmpLight = new BasicCurtain();
                            TmpLight.id = bytChnID;
                            temp.sumCurtainChns.Add(TmpLight);
                        }
                        CsConst.simpleSearchDevicesList.Add(temp);
                        #endregion
                    }
                    else if (bigType == SimpleFunctionsList.sensorBigType)
                    {
                        #region
                        if (SimpleFunctionsList.simpleSensorsList == null) SimpleFunctionsList.simpleSensorsList = new List<HdlSimpleSensors>();
                        HdlSimpleSensors temp = new HdlSimpleSensors();
                        temp.subnetID = subnetId;
                        temp.deviceID = deviceId;
                        temp.remark = tmpRemark;
                        temp.DeviceType = deviceType;
                        temp.bigType = FunctionsGroupE549Respond[0 + bTmp * 3];
                        temp.smallType = FunctionsGroupE549Respond[1 + bTmp * 3];
                        temp.sumChns = FunctionsGroupE549Respond[2 + bTmp * 3];
                        temp.sensorParameters = new List<byte[]>();
                        for (Byte bytChnID = 1; bytChnID <= temp.sumChns; bytChnID++)
                        {
                            Byte[] TmpLight = new Byte[5];
                            temp.sensorParameters.Add(TmpLight);
                        }
                        CsConst.simpleSearchDevicesList.Add(temp);
                        #endregion
                    }
                    else if (bigType == SimpleFunctionsList.buttonBigType)
                    {
                        #region
                        if (SimpleFunctionsList.simpleButtonsList == null) SimpleFunctionsList.simpleButtonsList = new List<HdlSimpleButtons>();
                        HdlSimpleButtons temp = new HdlSimpleButtons();
                        temp.subnetID = subnetId;
                        temp.deviceID = deviceId;
                        temp.remark = tmpRemark;
                        temp.DeviceType = deviceType;
                        temp.bigType = FunctionsGroupE549Respond[0 + bTmp * 3];
                        temp.smallType = FunctionsGroupE549Respond[1 + bTmp * 3];
                        temp.sumChns = FunctionsGroupE549Respond[2 + bTmp * 3];
                        temp.sumButtonDrys = new List<HDLButton>();
                        for (Byte bytChnID = 1; bytChnID <= temp.sumChns; bytChnID++)
                        {
                            HDLButton TmpLight = new HDLButton();
                            TmpLight.ID = bytChnID;
                            temp.sumButtonDrys.Add(TmpLight);
                        }
                        CsConst.simpleSearchDevicesList.Add(temp);
                        #endregion
                    }
                }

            }
            catch
            { }
        }

        public static void LocateDeviceWhereItIs(Int32 bigType, String deviceName)
        {
            if (bigType == -1) return;
            try
            {
                String TmpDevName = deviceName.Split('\\')[0].Trim();

                byte SubNetID = byte.Parse(TmpDevName.Split('-')[0].ToString());
                byte DeviceID = byte.Parse(TmpDevName.Split('-')[1].ToString());

                byte[] arayTmp = new byte[1];
                arayTmp[0] = 6;
                CsConst.mySends.AddBufToSndList(arayTmp, 0xE442, SubNetID, DeviceID, false, false, false, false);
            }
            catch
            { }
        }

        public static HdlDevice FindHdlDeviceWithNameAndBigType(Int32 bigType, String deviceName)
        {
             HdlDevice tmpDevice = null;
            if (bigType == -1) return null;
            try
            {
                String TmpDevName = deviceName.Split('\\')[0].Trim();
               
                byte SubNetID = byte.Parse(TmpDevName.Split('-')[0].ToString());
                byte DeviceID = byte.Parse(TmpDevName.Split('-')[1].ToString());

                foreach (HdlDevice temp in CsConst.simpleSearchDevicesList)
                {
                    if (temp.subnetID == SubNetID && temp.deviceID == DeviceID && bigType == temp.bigType)
                    {
                        tmpDevice = temp;
                        break;
                    }
                }
            }
            catch
            { }
            return tmpDevice;
        }


        public static void DisplaySimpleFunctionListToDataGridview(DataGridView dgOnline, Byte bigType)
        {
            if (CsConst.simpleSearchDevicesList == null || CsConst.simpleSearchDevicesList.Count ==0) return;
            if (dgOnline == null) return;
            oTmpDataGrid = dgOnline;
            oTmpDataGrid.Rows.Clear();
            try
            {
               foreach (HdlDevice temp in CsConst.simpleSearchDevicesList)
               {
                    Int32 iRowCount = 1;
                    if (temp.bigType == bigType)
                    {
                        for (Byte bChnId = 1; bChnId <= temp.sumChns; bChnId++)
                        {
                            String sHeadString = CsConst.sLightSmallTypesList[temp.smallType % 9];
                            String sCurrentStatus = "0";
                            if (bigType == 1) // light
                            {
                                sHeadString = CsConst.sLightSmallTypesList[temp.smallType % 3];
                                sCurrentStatus = "0";
                            }
                            else if (bigType == 2) // curtain
                            {
                                sHeadString = CsConst.sCurtainSmallTypesList[temp.smallType % 9];
                                sCurrentStatus =CsConst.CurtainModes[2];
                            }
                            else if (bigType == 4) // button or dry contact
                            {
                                sHeadString = CsConst.sButtonSmallTypesList[temp.smallType % 2];
                                sCurrentStatus = CsConst.Status[0];
                            }
                            else if (bigType == 5) // sensor 
                            {
                                sHeadString = CsConst.sSensorSmallTypesList[temp.smallType % 8];
                                sCurrentStatus = CsConst.Status[1];
                            }
                            object[] obj = {sHeadString, temp.subnetID.ToString() + "-" +temp.deviceID + "-" + temp.remark.ToString(), 
                                           "Locate", bChnId.ToString(),"Chn " + bChnId.ToString(), 0,100,sCurrentStatus,false};

                            if (oTmpDataGrid.InvokeRequired)//等待异步
                            {
                                FlushClient fc = new FlushClient(ThreadFunction);
                                oTmpDataGrid.Invoke(fc, new object[] { obj });
                            }
                            else
                            {
                                oTmpDataGrid.Rows.Add(obj);
                            }
                            iRowCount++;
                        }
                    }
                }
            }
            catch{}
        }

        public static List<UVCMD.ControlTargets> DownloadLogicTrueCommandsFromDevice(Byte SubNetID, Byte DeviceID, int DeviceType, Byte LogicId)
        {
            List<UVCMD.ControlTargets> SetUp = new List<UVCMD.ControlTargets>();
            try
            {
                Byte sumLogicCommandsInEveryBlock = 10;
                Int32 operationCode = 0x1612;
                if (Eightin1DeviceTypeList.HDL8in1DeviceType.Contains(DeviceType))
                {
                    operationCode = 0x1654;
                    sumLogicCommandsInEveryBlock = Eightin1DeviceTypeList.sumCommandsInEveryBlock;
                }

                //成立的触发目标
                #region
                SetUp = new List<UVCMD.ControlTargets>();
                for (Byte bytJ = 1; bytJ <= sumLogicCommandsInEveryBlock; bytJ++)
                {
                    Byte[] ArayTmp = new byte[2] { (Byte)(LogicId + 1), bytJ };
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, operationCode, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                    {
                        UVCMD.ControlTargets oCMD = new UVCMD.ControlTargets();
                        oCMD.ID = CsConst.myRevBuf[27];
                        oCMD.Type = CsConst.myRevBuf[28];  //转换为正确的类型

                        oCMD.SubnetID = CsConst.myRevBuf[29];
                        oCMD.DeviceID = CsConst.myRevBuf[30];
                        oCMD.Param1 = CsConst.myRevBuf[31];
                        oCMD.Param2 = CsConst.myRevBuf[32];
                        oCMD.Param3 = CsConst.myRevBuf[33];
                        oCMD.Param4 = CsConst.myRevBuf[34];
                        CsConst.myRevBuf = new byte[1200];
                        SetUp.Add(oCMD);
                        CsConst.myRevBuf = new byte[1200];
                        HDLUDP.TimeBetwnNext(1);
                    }
                    else return SetUp;
                }
                #endregion
            }
            catch
            { }
            return SetUp;
        }

        public static List<UVCMD.ControlTargets> DownloadLogicFalseCommandsFromDevice(Byte SubNetID, Byte DeviceID, int DeviceType, Byte LogicId)
        {
            List<UVCMD.ControlTargets> NoSetUp = new List<UVCMD.ControlTargets>();
            try
            {
                Byte sumLogicCommandsInEveryBlock = 10;
                Byte sumLogibBlockInModule = 24;
                Int32 operationCode = 0x1612;
                if (Eightin1DeviceTypeList.HDL8in1DeviceType.Contains(DeviceType))
                {
                    operationCode = 0x1654;
                    sumLogibBlockInModule = Eightin1DeviceTypeList.TotalLogic;
                    sumLogicCommandsInEveryBlock = Eightin1DeviceTypeList.sumCommandsInEveryBlock;
                }

                //不成立的触发目标
                #region
                NoSetUp = new List<UVCMD.ControlTargets>();
                for (Byte bytJ = 1; bytJ <= sumLogicCommandsInEveryBlock; bytJ++)
                {
                    Byte[] ArayTmp = new byte[2] { (Byte)(LogicId + sumLogibBlockInModule + 1), bytJ };
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, operationCode, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                    {
                        UVCMD.ControlTargets oCMD = new UVCMD.ControlTargets();
                        oCMD.ID = CsConst.myRevBuf[27];
                        oCMD.Type = CsConst.myRevBuf[28];  //转换为正确的类型

                        oCMD.SubnetID = CsConst.myRevBuf[29];
                        oCMD.DeviceID = CsConst.myRevBuf[30];
                        oCMD.Param1 = CsConst.myRevBuf[31];
                        oCMD.Param2 = CsConst.myRevBuf[32];
                        oCMD.Param3 = CsConst.myRevBuf[33];
                        oCMD.Param4 = CsConst.myRevBuf[34];
                        CsConst.myRevBuf = new byte[1200];
                        NoSetUp.Add(oCMD);
                        CsConst.myRevBuf = new byte[1200];
                        HDLUDP.TimeBetwnNext(1);
                    }
                    else return NoSetUp;
                }
                #endregion
            }
            catch { }
            return NoSetUp;
        }

        public static Boolean ModifyLogicTrueCommandsFromDevice(Byte SubNetID, Byte DeviceID, int DeviceType, Byte LogicId, List<UVCMD.ControlTargets> Setup)
        {
            Boolean blnIsSuccess = true;
            if (Setup == null) return blnIsSuccess;
            try
            {
                Int32 operationCode = 0x1614;
                if (Eightin1DeviceTypeList.HDL8in1DeviceType.Contains(DeviceType))
                {
                    operationCode = 0x1656;
                }

                if (Setup != null && Setup.Count != 0)
                {
                    foreach (UVCMD.ControlTargets TmpCmd in Setup)
                    {
                        byte[] arayCMD = new byte[9];
                        arayCMD[0] = (Byte)(LogicId + 1);
                        arayCMD[1] = byte.Parse(TmpCmd.ID.ToString());
                        arayCMD[2] = TmpCmd.Type;
                        arayCMD[3] = TmpCmd.SubnetID;
                        arayCMD[4] = TmpCmd.DeviceID;
                        arayCMD[5] = TmpCmd.Param1;
                        arayCMD[6] = TmpCmd.Param2;
                        arayCMD[7] = TmpCmd.Param3;   // save targets
                        arayCMD[8] = TmpCmd.Param4;
                        blnIsSuccess = CsConst.mySends.AddBufToSndList(arayCMD, operationCode, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType));
                    }
                }
            }
            catch { }
            return blnIsSuccess;
        }

        public static Boolean ModifyLogicFalseCommandsFromDevice(Byte SubNetID, Byte DeviceID, int DeviceType, Byte LogicId, List<UVCMD.ControlTargets> NoSetUp)
        {
            Boolean blnIsSuccess = true;
            try
            {
                Byte sumLogibBlockInModule = 24;
                Int32 operationCode = 0x1614;
                if (Eightin1DeviceTypeList.HDL8in1DeviceType.Contains(DeviceType))
                {
                    operationCode = 0x1656;
                    sumLogibBlockInModule = Eightin1DeviceTypeList.TotalLogic;
                }

                if (NoSetUp != null && NoSetUp.Count != 0)
                {
                    foreach (UVCMD.ControlTargets TmpCmd in NoSetUp)
                    {
                        byte[] arayCMD = new byte[9];
                        arayCMD[0] = (Byte)(LogicId + sumLogibBlockInModule + 1);
                        arayCMD[1] = byte.Parse(TmpCmd.ID.ToString());
                        arayCMD[2] = TmpCmd.Type;
                        arayCMD[3] = TmpCmd.SubnetID;
                        arayCMD[4] = TmpCmd.DeviceID;
                        arayCMD[5] = TmpCmd.Param1;
                        arayCMD[6] = TmpCmd.Param2;
                        arayCMD[7] = TmpCmd.Param3;   // save targets
                        arayCMD[8] = TmpCmd.Param4;
                        blnIsSuccess = CsConst.mySends.AddBufToSndList(arayCMD, operationCode, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType));
                    }
                }
            }
            catch { }
            return blnIsSuccess;
        }

        public static Boolean CheckDeviceIfCouldBeFound(int OperationCode, Byte bSunNetId, Byte bDeviceId)
        {
            Boolean BlnDealWithFunctionGroups = (OperationCode == 0xE548);
            Boolean bLnFound = false;
            Boolean bLnReFound = false;
            try
            {
                CsConst.FastSearch = true;
                Byte RepeatSendTimes = 1;
                Random R1 = new Random();
                Random R2 = new Random();
                List<Byte> SearchDeviceAddressesList = new List<byte>();
                R1.Next();
                Byte[] RandomWhenSearch = new Byte[2];
                R1.NextBytes(RandomWhenSearch);
                Byte[] DataSendBuffer = new Byte[2 + SearchDeviceAddressesList.Count];
                RandomWhenSearch.CopyTo(DataSendBuffer, 0);
            RepeatSearch:
                if (SearchDeviceAddressesList != null && SearchDeviceAddressesList.Count > 0)
                {
                    DataSendBuffer = new Byte[2 + SearchDeviceAddressesList.Count];
                    RandomWhenSearch.CopyTo(DataSendBuffer, 0);
                    SearchDeviceAddressesList.CopyTo(DataSendBuffer, 2);
                    SearchDeviceAddressesList = new List<byte>();
                }
                CsConst.mySends.AddBufToSndList(DataSendBuffer, OperationCode, 255, 255, false, false, false, false);
                // 广播搜索方式汇集
                #region
                DateTime d1, d2;

                d1 = DateTime.Now;
                d2 = DateTime.Now;
                while (UDPReceive.receiveQueue.Count > 0 || HDLSysPF.Compare(d2, d1) < 4000)
                {
                    d2 = DateTime.Now;
                    if (UDPReceive.receiveQueue.Count > 0)
                    {
                        byte[] readData = UDPReceive.receiveQueue.Dequeue();

                        if (readData[21] * 256 + readData[22] == OperationCode + 1)
                        {                            
                            if (readData[17] == bSunNetId && readData[18] == bDeviceId)
                            {
                                if (bLnFound == true) bLnReFound = true;
                                else
                                {
                                    bLnFound = true;
                                }
                            }

                            if (bLnFound)
                            {
                                SearchDeviceAddressesList.Add(bSunNetId);
                                SearchDeviceAddressesList.Add(bDeviceId);
                            }
                        }
                    }
                }

                RepeatSendTimes++;
                if (RepeatSendTimes <= 4) goto RepeatSearch;
                #endregion
                CsConst.FastSearch = false;
                if (bLnFound == true && bLnReFound == false) return true;
                else return false;
            }

            catch
            {
                return false;
            }
        }

        public static String ReadDeviceMacInformation(Byte bSunNetId, Byte bDeviceId,Boolean bIsReSend)
        {
            String sMacResult = "";
            try
            {
                if (CsConst.mySends.AddBufToSndList(null, 0xF003, bSunNetId, bDeviceId, false, false, bIsReSend, false) == true)  //读取MAC备注
                {
                    sMacResult = CsConst.myRevBuf[25].ToString("X2") + "."
                               + CsConst.myRevBuf[26].ToString("X2") + "."
                               + CsConst.myRevBuf[27].ToString("X2") + "."
                               + CsConst.myRevBuf[28].ToString("X2") + "."
                               + CsConst.myRevBuf[29].ToString("X2") + "."
                               + CsConst.myRevBuf[30].ToString("X2") + "."
                               + CsConst.myRevBuf[31].ToString("X2") + "."
                               + CsConst.myRevBuf[32].ToString("X2");
                    CsConst.myRevBuf = new byte[1200];
                }
            }
            catch
            {
                return sMacResult;
            }
            return sMacResult;
        }

        //public static String IpModuleReadDeviceMacInformation(Byte bSunNetId, Byte bDeviceId, Boolean bIsReSend)
        //{
        //    String sMacResult = "";
        //    try
        //    {
        //        Byte[] arrTmp = new Byte[4];
        //        Random R1 = new Random();
        //        R1.Next();
        //        R1.NextBytes(arrTmp);
        //        if (CsConst.mySends.AddBufToSndList(arrTmp, 0x13F0, 255, 255, false, false, bIsReSend, false) == true)  //读取MAC备注
        //        {
        //            DateTime d1, d2;

        //            d1 = DateTime.Now;
        //            d2 = DateTime.Now;
        //            while (UDPReceive.receiveQueue.Count > 0 || HDLSysPF.Compare(d2, d1) < 2000)
        //        {
        //            d2 = DateTime.Now;
        //            if (UDPReceive.receiveQueue.Count > 0)
        //            {
        //                byte[] readData = UDPReceive.receiveQueue.Dequeue();

        //                if (readData[21] * 256 + readData[22] == OperationCode + 1)
        //                { 
        //            sMacResult = CsConst.myRevBuf[25].ToString("X2") + "."
        //                       + CsConst.myRevBuf[26].ToString("X2") + "."
        //                       + CsConst.myRevBuf[27].ToString("X2") + "."
        //                       + CsConst.myRevBuf[28].ToString("X2") + "."
        //                       + CsConst.myRevBuf[29].ToString("X2") + "."
        //                       + CsConst.myRevBuf[30].ToString("X2") + "."
        //                       + CsConst.myRevBuf[31].ToString("X2") + "."
        //                       + CsConst.myRevBuf[32].ToString("X2");
        //                }
        //            }

        //        }
        //    }
        //    catch
        //    {
        //        return sMacResult;
        //    }
        //    return sMacResult;
        //}

        public static String ReadDeviceHardwareInformation(Byte bSunNetId, Byte bDeviceId)
        {
            String sHardwareInformation = "";
            try
            {
                if (CsConst.mySends.AddBufToSndList(null, 0x3024, bSunNetId, bDeviceId, false, false, true, false) == true)  //读取MAC备注
                {
                    Byte bPacketLength = CsConst.myRevBuf[16];
                    byte[] arayRemark = new byte[bPacketLength - 11];
                    Array.Copy(CsConst.myRevBuf, 25, arayRemark,0, bPacketLength - 11);
                    sHardwareInformation = HDLPF.Byte2String(arayRemark);
                    CsConst.myRevBuf = new byte[1200];
                }
            }
            catch
            {
                return sHardwareInformation;
            }
            return sHardwareInformation;
        }

        public static Boolean ModifyDeviceAddressWithTwoDifferentways(Byte bSubNetId, Byte bDeviceId,String sMacInformation,int iDeviceType,
                                                                      Byte bNewSubNetId, Byte bNewDeviceId, Byte bType) // mac = 0; press button = 1
        {
            Boolean bIsSuccess = false;
            try
            {
                Boolean bIsNeedPassword = false;

                Byte[] ArayTmp = new Byte[10];
                string[] ArayStr = new String[0];
                if ((sMacInformation == null || sMacInformation == "") && (bType == 0)) return bIsSuccess;
                else if (bType!=1)
                {
                    ArayStr = sMacInformation.Split('.');
                    for (int intI = 0; intI < ArayStr.Length; intI++) ArayTmp[intI] = Convert.ToByte(ArayStr[intI], 16);
                }
                
                #region
                // 是不是特殊版本的设备
                bIsNeedPassword = CsConst.SpecailAddress.Contains(iDeviceType); // 要不要加修改地址加密界面
                if (bIsNeedPassword) 
                {
                    bIsSuccess = HDLSysPF.SpecialModifyAddress(iDeviceType, 1, bSubNetId, bDeviceId, ArayTmp);
                    if (bIsSuccess == false) return bIsSuccess;
                }

                if (bType == 0) // 根据MAC修改地址
                {
                    bIsSuccess = ModifyAddressViaMAC(ArayStr,bSubNetId,bDeviceId,bNewSubNetId,bNewDeviceId);
                }
                else if (bType == 1) //  按住按键修改地址
                {
                    Byte[] arayTmp = new byte[2];
                    arayTmp[0] = bNewSubNetId;
                    arayTmp[1] = bNewDeviceId;
                    if (CsConst.mySends.AddBufToSndList(arayTmp, 0xE5F7, 255, 255, false, true, true, false)) //修改地址
                    {
                        bIsSuccess = true;
                    }
                }
                #endregion
 
            }
            catch
            {
                return bIsSuccess;
            }
            return bIsSuccess;
        }


        public static Boolean ModifyAddressViaMAC(String[] ArayStr, Byte bSubNetId, Byte bDeviceId, Byte bNewSubNetId, Byte bNewDeviceId)
        {
            Boolean bIsSuccess = false;
            try
            {
                Byte[] arayTmp = new byte[10];
                Byte[] ArayTmp = new byte[8];
                for (int intI = 0; intI < ArayStr.Length; intI++) ArayTmp[intI] = Convert.ToByte(ArayStr[intI], 16);
                Array.Copy(ArayTmp, 0, arayTmp, 0, 8);
                arayTmp[8] = bNewSubNetId;
                arayTmp[9] = bNewDeviceId;
                CsConst.ModifyDeviceAddressSubNetID = arayTmp[8];
                CsConst.ModifyDeviceAddressDeviceID = arayTmp[9];
                if (CsConst.mySends.AddBufToSndList(arayTmp, 0xF005, bSubNetId, bDeviceId, false, true, true, false)) //修改地址
                {
                    bIsSuccess = true;
                }
            }
            catch
            {
                return bIsSuccess;
            }
            return bIsSuccess;
        }

        public static Boolean IntialDeviceFactorySettings(Byte bSubNetId, Byte bDeviceId,String sMacInformation,int iDeviceType)
        {
            Boolean bIsSuccess = false;
            try
            {
                #region
                Cursor.Current = Cursors.WaitCursor;
                byte[] ArayTmp = new byte[12];

                ArayTmp[0] = 0;
                ArayTmp[1] = 0;

                string[] ArayStr = sMacInformation.Split('.');
                for (int intI = 0; intI < ArayStr.Length; intI++) ArayTmp[intI + 2] = Convert.ToByte(ArayStr[intI], 16);
                ArayTmp[10] = bSubNetId;
                ArayTmp[11] = (Byte)(bDeviceId + 1);

                CsConst.WaitMore = true;
                DateTime d1 = DateTime.Now;
                DateTime d2 = DateTime.Now;
                DateTime d3 = DateTime.Now;
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x3000, bSubNetId, bDeviceId, false, false, true, false) == true)
                {
                    int intTmp = CsConst.mintFactoryTime;
                    while (CsConst.mintFactoryTime != 0)
                    {
                        d2 = DateTime.Now;
                        if (CsConst.mintFactoryTime == 255 || CsConst.mintFactoryTime == 0) break;
                        if (HDLSysPF.Compare(d2, d3) >= 1000)
                        {
                            d3 = DateTime.Now;
                            intTmp = intTmp - 1;
                        }
                        if (intTmp <= 0 && CsConst.mintFactoryTime != 0)
                        {
                            return bIsSuccess;
                        }
                    }
                    bIsSuccess = true;
                }
                #endregion
            }
            catch
            {
                return bIsSuccess;
            }
            return bIsSuccess;
        }

        public static Boolean ModifyDeviceAddressAutomaticallyWithIpModule(Byte bSubNetId, Byte bDeviceId, String sMacInformation, int iDeviceType,Byte bNewSubNetId, Byte bNewDeviceId) // mac = 0; press button = 1
        {
            Boolean bIsSuccess = false;
            try
            {
                Byte[] ArayTmp = new Byte[10];
                if (sMacInformation == null || sMacInformation == "") return bIsSuccess;

                #region
                byte[] arayTmp = new byte[10];
                ArayTmp = new byte[8];
                string[] ArayStr = sMacInformation.Split('.');
                for (int intI = 0; intI < ArayStr.Length; intI++) ArayTmp[intI] = Convert.ToByte(ArayStr[intI], 16);
                Array.Copy(ArayTmp, 0, arayTmp, 0, 8);
                arayTmp[8] = bNewSubNetId;
                arayTmp[9] = bNewDeviceId;
                CsConst.ModifyDeviceAddressSubNetID = arayTmp[8];
                CsConst.ModifyDeviceAddressDeviceID = arayTmp[9];
                CsConst.mySends.AddBufToSndList(arayTmp, 0x13F2, bSubNetId, bDeviceId, false, false, false, false); //修改地址不需返回 再次读取测试

                System.Threading.Thread.Sleep(50);

                String sVersionName = HDLSysPF.ReadDeviceVersion(bNewSubNetId, bNewDeviceId,false);
                if (sVersionName != null && sVersionName != "")
                {
                    bIsSuccess = true;
                }                       
                #endregion

            }
            catch
            {
                return bIsSuccess;
            }
            return bIsSuccess;
        }
   
        ///广播读取MAC
        public static String ReadDeviceMacInformationViaBroadcast(Byte bSubNetId, Byte bDeviceId, int iDeviceType)
        {
            String sMacInformation = "";
            try
            {
                bool isGetMAC = false;
                CsConst.FastSearch = true;
                int SendTimes = 0;
                UDPReceive.ClearQueueData();
            Resend:
                
                CsConst.mySends.AddBufToSndList(null, 0xF003, 255, 255, false, false, false, false); //读取MAC备注
                DateTime d1 = DateTime.Now;
                DateTime d2 = DateTime.Now;

                while (SendTimes < 3)
                {
                    if (HDLSysPF.Compare(d2, d1) > 2000)
                    {
                        SendTimes = SendTimes + 1; goto Resend;
                    }
                    else
                    {
                        while (UDPReceive.receiveQueue.Count > 0)
                        {
                            byte[] readData = UDPReceive.receiveQueue.Dequeue();
                            if (readData[21] == 0xF0 && readData[22] == 0x04)
                            {
                                if (readData[17] == bSubNetId && readData[18] == bDeviceId && (readData[19] * 256 + readData[20]) == iDeviceType)
                                {
                                    sMacInformation = readData[25].ToString("X2") + "." + readData[26].ToString("X2") + "." + readData[27].ToString("X2") + "."
                                                + readData[28].ToString("X2") + "." + readData[29].ToString("X2") + "." + readData[30].ToString("X2") + "."
                                                + readData[31].ToString("X2") + "." + readData[32].ToString("X2");
                                    isGetMAC = true;
                                    goto Result;
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
                return sMacInformation;
            }
        Result:
            CsConst.FastSearch = false;
            Cursor.Current = Cursors.Default;
            return sMacInformation;
        }
    }

}
