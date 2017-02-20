using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Drawing.Imaging;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Runtime.InteropServices;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace HDL_Buspro_Setup_Tool
{
    class HDLSysPF
    {
        public static void UpdateAddressesFromPublicStructs(int intDIndex, String strName)
        {
            try
            {
                if (CsConst.myDimmers != null && CsConst.myDimmers.Count > 0)
                {
                    foreach (Dimmer temp in CsConst.myDimmers)
                    {
                        if (temp.DIndex == intDIndex)
                        {
                            temp.DeviceName = strName;
                            break;
                        }
                    }
                }
                else if (CsConst.myRelays != null && CsConst.myRelays.Count > 0)
                {
                    foreach (Relay temp in CsConst.myRelays)
                    {
                        if (temp.DIndex == intDIndex)
                        {
                            temp.DeviceName = strName;
                            break;
                        }
                    }
                }
                if (CsConst.myDrys != null && CsConst.myDrys.Count > 0)
                {
                    foreach (MS04 temp in CsConst.myDrys)
                    {
                        if (temp.DIndex == intDIndex)
                        {
                            temp.DeviceName = strName;
                            break;
                        }
                    }
                }
                if (CsConst.myPanels != null && CsConst.myPanels.Count > 0)
                {
                    foreach (Panel temp in CsConst.myPanels)
                    {
                        if (temp.DIndex == intDIndex)
                        {
                            temp.DeviceName = strName;
                            break;
                        }
                    }
                }
                if (CsConst.myDmxs != null && CsConst.myDmxs.Count > 0)
                {
                    foreach (DMX temp in CsConst.myDmxs)
                    {
                        if (temp.DIndex == intDIndex)
                        {
                            temp.DeviceName = strName;
                            break;
                        }
                    }
                }
                if (CsConst.myCurtains != null && CsConst.myCurtains.Count > 0)
                {
                    foreach (Curtain temp in CsConst.myCurtains)
                    {
                        if (temp.DIndex == intDIndex)
                        {
                            temp.DeviceName = strName;
                            break;
                        }
                    }
                }
                if (CsConst.myzBoxs != null && CsConst.myzBoxs.Count > 0)
                {
                    foreach (MzBox temp in CsConst.myzBoxs)
                    {
                        if (temp.DIndex == intDIndex)
                        {
                            temp.Devname = strName;
                            break;
                        }
                    }
                }
                if (CsConst.myHvacs != null && CsConst.myHvacs.Count > 0)
                {
                    foreach (HVAC temp in CsConst.myHvacs)
                    {
                        if (temp.DIndex == intDIndex)
                        {
                            temp.DeviceName = strName;
                            break;
                        }
                    }
                }
                if (CsConst.myIPs != null && CsConst.myIPs.Count > 0)
                {
                    foreach (IPModule temp in CsConst.myIPs)
                    {
                        if (temp.DIndex == intDIndex)
                        {
                            temp.DeviceName = strName;
                            break;
                        }
                    }
                }
                if (CsConst.mysensor_8in1 != null && CsConst.mysensor_8in1.Count > 0)
                {
                    foreach (MultiSensor temp in CsConst.mysensor_8in1)
                    {
                        if (temp.DIndex == intDIndex)
                        {
                            temp.Devname = strName;
                            break;
                        }
                    }
                }
                if (CsConst.mysensor_12in1 != null && CsConst.mysensor_12in1.Count > 0)
                {
                    foreach (Sensor_12in1 temp in CsConst.mysensor_12in1)
                    {
                        if (temp.DIndex == intDIndex)
                        {
                            temp.Devname = strName;
                            break;
                        }
                    }
                }
                if (CsConst.myRS232 != null && CsConst.myRS232.Count > 0)
                {
                    foreach (RS232 temp in CsConst.myRS232)
                    {
                        if (temp.DIndex == intDIndex)
                        {
                            temp.strName = strName;
                            break;
                        }
                    }
                }
                if (CsConst.myHais != null && CsConst.myHais.Count > 0)
                {
                    foreach (HAI temp in CsConst.myHais)
                    {
                        if (temp.DIndex == intDIndex)
                        {
                            temp.DeviceName = strName;
                            break;
                        }
                    }
                }
                if (CsConst.myMediaBoxes != null && CsConst.myMediaBoxes.Count > 0)
                {
                    foreach (MMC temp in CsConst.myMediaBoxes)
                    {
                        if (temp.DIndex == intDIndex)
                        {
                            temp.strName = strName;
                            break;
                        }
                    }
                }
                if (CsConst.mySecurities != null && CsConst.mySecurities.Count > 0)
                {
                    foreach (Security temp in CsConst.mySecurities)
                    {
                        if (temp.DIndex == intDIndex)
                        {
                            temp.DeviceName = strName;
                            break;
                        }
                    }
                }
                if (CsConst.myLogics != null && CsConst.myLogics.Count > 0)
                {
                    foreach (Logic temp in CsConst.myLogics)
                    {
                        if (temp.DIndex == intDIndex)
                        {
                            temp.DevName = strName;
                            break;
                        }
                    }
                }
                if (CsConst.myBacnets != null && CsConst.myBacnets.Count > 0)
                {
                    foreach (BacNet temp in CsConst.myBacnets)
                    {
                        if (temp.DIndex == intDIndex)
                        {
                            temp.DeviceName = strName;
                            break;
                        }
                    }
                }
                if (CsConst.myFHs != null && CsConst.myFHs.Count > 0)
                {
                    foreach (FH temp in CsConst.myFHs)
                    {
                        if (temp.DIndex == intDIndex)
                        {
                            temp.DeviceName = strName;
                            break;
                        }
                    }
                }
                if (CsConst.myDS != null && CsConst.myDS.Count > 0)
                {
                    foreach (DS temp in CsConst.myDS)
                    {
                        if (temp.DIndex == intDIndex)
                        {
                            temp.Devname = strName;
                            break;
                        }
                    }
                }
                if (CsConst.myRcuMixModules != null && CsConst.myRcuMixModules.Count > 0)
                {
                    foreach (MHRCU temp in CsConst.myRcuMixModules)
                    {
                        if (temp.DIndex == intDIndex)
                        {
                            temp.Devname = strName;
                            break;
                        }
                    }
                }
                if (CsConst.myPUSensors != null && CsConst.myPUSensors.Count > 0)
                {
                    foreach (MSPU temp in CsConst.myPUSensors)
                    {
                        if (temp.DIndex == intDIndex)
                        {
                            temp.Devname = strName;
                            break;
                        }
                    }
                }
                if (CsConst.myNewIR != null && CsConst.myNewIR.Count > 0)
                {
                    foreach (NewIR temp in CsConst.myNewIR)
                    {
                        if (temp.DIndex == intDIndex)
                        {
                            temp.strName = strName;
                            break;
                        }
                    }
                }
                if (CsConst.myColorPanels != null && CsConst.myColorPanels.Count > 0)
                {
                    foreach (ColorDLP temp in CsConst.myColorPanels)
                    {
                        if (temp.DIndex == intDIndex)
                        {
                            temp.DeviceName = strName;
                            break;
                        }
                    }
                }

                if (CsConst.myCurtains != null && CsConst.myCurtains.Count > 0)
                {
                    foreach (Curtain temp in CsConst.myCurtains)
                    {
                        if (temp.DIndex == intDIndex)
                        {
                            temp.DeviceName = strName;
                            break;
                        }
                    }
                }

                if (CsConst.myCoolMasters != null && CsConst.myCoolMasters.Count > 0)
                {
                    foreach (CoolMaster temp in CsConst.myCoolMasters)
                    {
                        if (temp.DIndex == intDIndex)
                        {
                            temp.DeviceName = strName;
                            break;
                        }
                    }
                }
                if (CsConst.myNewDS != null && CsConst.myNewDS.Count > 0)
                {
                    foreach (NewDS temp in CsConst.myNewDS)
                    {
                        if (temp.DIndex == intDIndex)
                        {
                            temp.Devname = strName;
                            break;
                        }
                    }
                }                
                if (CsConst.mysensor_7in1 != null && CsConst.mysensor_7in1.Count > 0)
                {
                    foreach (Sensor_7in1 temp in CsConst.mysensor_7in1)
                    {
                        if (temp.DIndex == intDIndex)
                        {
                            temp.Devname = strName;
                            break;
                        }
                    }
                }
                if (CsConst.myMiniSensors != null && CsConst.myMiniSensors.Count > 0)
                {
                    foreach (MiniSensor temp in CsConst.myMiniSensors)
                    {
                        if (temp.DIndex == intDIndex)
                        {
                            temp.Devname = strName;
                            break;
                        }
                    }
                }
                if (CsConst.myEnviroPanels != null && CsConst.myEnviroPanels.Count > 0)
                {
                    foreach (EnviroPanel temp in CsConst.myEnviroPanels)
                    {
                        if (temp.DIndex == intDIndex)
                        {
                            temp.DeviceName = strName;
                            break;
                        }
                    }
                }
            }
            catch
            { }
        }

        public static void GetWeekdaysFromPublicStruct()
        {
            if (CsConst.WholeTextsList == null || CsConst.WholeTextsList.Count == 0) return;
            try
            { 
                CsConst.Weekdays = new String[7];
                CsConst.Weekdays[0] = CsConst.WholeTextsList[2162].sDisplayName;
                for (int i = 0; i < 6; i++) CsConst.Weekdays[1 + i] = CsConst.WholeTextsList[2156 + i].sDisplayName;

                CsConst.Status = new String[2];
                CsConst.Status[0] = CsConst.WholeTextsList[80].sDisplayName;
                CsConst.Status[1] = CsConst.WholeTextsList[197].sDisplayName;

                CsConst.UvSwitchStatus = new String[2];
                CsConst.UvSwitchStatus[0] = CsConst.WholeTextsList[893].sDisplayName;
                CsConst.UvSwitchStatus[1] = CsConst.WholeTextsList[2073].sDisplayName;


                CsConst.StatusAC = new String[5];
                CsConst.StatusAC[0] = CsConst.WholeTextsList[259].sDisplayName;
                CsConst.StatusAC[1] = CsConst.WholeTextsList[260].sDisplayName;
                CsConst.StatusAC[2] = CsConst.WholeTextsList[719].sDisplayName;
                CsConst.StatusAC[3] = CsConst.WholeTextsList[261].sDisplayName;
                CsConst.StatusAC[4] = CsConst.WholeTextsList[262].sDisplayName;


                CsConst.StatusFAN = new String[4];
                CsConst.StatusFAN[0] = CsConst.WholeTextsList[1681].sDisplayName;
                CsConst.StatusFAN[1] = CsConst.WholeTextsList[1089].sDisplayName;
                CsConst.StatusFAN[2] = CsConst.WholeTextsList[1088].sDisplayName;
                CsConst.StatusFAN[3] = CsConst.WholeTextsList[1087].sDisplayName;

                CsConst.CurtainModes = new List<string>();
                CsConst.CurtainModes.Add(CsConst.WholeTextsList[1386].sDisplayName);
                CsConst.CurtainModes.Add(CsConst.WholeTextsList[2073].sDisplayName);
                CsConst.CurtainModes.Add(CsConst.WholeTextsList[893].sDisplayName);

                CsConst.CurtainRelayState = new string[] { CsConst.CurtainModes[2], CsConst.CurtainModes[1], CsConst.CurtainModes[0] };

               CsConst.NewIRLibraryDeviceType = new String[5];
               CsConst.NewIRLibraryDeviceType[0] = CsConst.WholeTextsList[1469].sDisplayName;
               CsConst.NewIRLibraryDeviceType[1] = CsConst.WholeTextsList[1468].sDisplayName;
               CsConst.NewIRLibraryDeviceType[2] = CsConst.WholeTextsList[1467].sDisplayName;
               CsConst.NewIRLibraryDeviceType[3] = CsConst.WholeTextsList[1465].sDisplayName;
               CsConst.NewIRLibraryDeviceType[4] = CsConst.WholeTextsList[251].sDisplayName;

               CsConst.strWirelessAddressState = new String[2];
               CsConst.strWirelessAddressState[0]= CsConst.WholeTextsList[2764].sDisplayName;
               CsConst.strWirelessAddressState[1] = CsConst.WholeTextsList[2765].sDisplayName;


               CsConst.strWirelessAddressConnection = new String[2];
               CsConst.strWirelessAddressConnection[0] = CsConst.WholeTextsList[2501].sDisplayName;
               CsConst.strWirelessAddressConnection[1] = CsConst.WholeTextsList[2204].sDisplayName;

               CsConst.security = new String[10];
               CsConst.security[0] = CsConst.WholeTextsList[2089].sDisplayName;
               CsConst.security[1] = CsConst.WholeTextsList[2087].sDisplayName;
               CsConst.security[2] = CsConst.WholeTextsList[895].sDisplayName;
               CsConst.security[3] = CsConst.WholeTextsList[2090].sDisplayName;
               CsConst.security[4] = CsConst.WholeTextsList[2086].sDisplayName;
               for (int i = 0; i < 5; i++) CsConst.security[5 + i] = CsConst.WholeTextsList[2091 + i].sDisplayName;

               CsConst.MusicControl = new String[8];
               for (int i = 0; i < 8 ; i++) CsConst.MusicControl[i] = CsConst.WholeTextsList[2096 + i].sDisplayName;

                CsConst.GPRSControl = new String[3];
                CsConst.GPRSControl[0] = CsConst.WholeTextsList[1690].sDisplayName;
                CsConst.GPRSControl[1] = CsConst.WholeTextsList[2526].sDisplayName;
                CsConst.GPRSControl[2] = CsConst.WholeTextsList[2527].sDisplayName;

                CsConst.PanelControl = new String[33];
                CsConst.PanelControl[0] = CsConst.WholeTextsList[1775].sDisplayName;
                for (int i = 0; i < 21; i++) CsConst.PanelControl[i + 1] = CsConst.WholeTextsList[1944 + i].sDisplayName;
                CsConst.PanelControl[22] = CsConst.WholeTextsList[219].sDisplayName;
                for (int i = 23; i < 33; i++) CsConst.PanelControl[i] = CsConst.WholeTextsList[1944 + i - 2].sDisplayName;

                CsConst.LoadType = new String[13];
                for (int i = 0; i < 4; i++) CsConst.LoadType[i] = CsConst.WholeTextsList[2607 + i].sDisplayName;
                CsConst.LoadType[4] = CsConst.WholeTextsList[2072].sDisplayName;
                CsConst.LoadType[5] = CsConst.WholeTextsList[2611].sDisplayName;
                CsConst.LoadType[6] = CsConst.WholeTextsList[2612].sDisplayName;
                CsConst.LoadType[7] = CsConst.WholeTextsList[727].sDisplayName;
                CsConst.LoadType[8] = CsConst.WholeTextsList[1310].sDisplayName;
                for (int i = 0; i < 4; i++) CsConst.LoadType[9 + i] = CsConst.WholeTextsList[2613 + i].sDisplayName;
            }
            catch
            { }
        }

        public static void setRS232ModeByDB(int devicetype, ComboBox cb)
        {
            OleDbDataReader dr = null;
            try
            {
                string strsql = "select * from defRS232ToBUSModeMusic where ";
                cb.Items.Clear();
                if (devicetype == 1008 || devicetype == 1009 || devicetype == 1013 || devicetype == 1016 || devicetype == 1004)
                {
                    strsql = strsql + "ID<=2 order by ID";
                }
                else if (devicetype == 1017)
                {
                    strsql = strsql + "ID>=3 and ID<=7 order by ID";
                }
                else if (devicetype == 1031)
                {
                    strsql = strsql + "ID=7 or ID=25 order by ID";
                }
                else if (devicetype == 1030)
                {
                    strsql = strsql + "ID>=3 and ID<=6 order by ID";
                }
                else if (devicetype == 1014 || devicetype == 1018 || devicetype == 1027)
                {
                    strsql = strsql + "ID<=5 order by ID";
                }
                else if (devicetype == 1019 || devicetype == 1033 || devicetype == 3503 || devicetype == 1039 || devicetype == 3504)
                {
                    for (int i = 0; i < 4; i++)
                        cb.Items.Add(CsConst.mstrINIDefault.IniReadValue("public", "0050" + i.ToString(), ""));
                    if (devicetype == 1033 || devicetype == 3503 || devicetype == 1039 || devicetype == 3504)
                        cb.Items.Add(CsConst.mstrINIDefault.IniReadValue("public", "00504", ""));
                    return;
                }
                else if (devicetype == 1032)
                {
                    strsql = strsql + "ID>=8 and ID<19 or ID=26 order by ID";
                }
                else if (devicetype == 1020)
                {
                    strsql = strsql + "ID>=8 and ID<19 order by ID";
                }
                else if (devicetype == 1034)
                {
                    strsql = strsql + "ID>=8 and ID<19 or ID=26 or ID=28 or ID=29 or ID=30 or ID=34 or ID=35 order by ID";
                }
                else if (devicetype == 1035)
                {
                    strsql = strsql + "ID=31 order by ID";
                }
                else if (devicetype == 1036)
                {
                    strsql = strsql + "ID=32 or ID=33 order by ID";
                }
                else if (devicetype == 1029)
                {
                    strsql = strsql + "ID>=19 order by ID";
                }
                else if (devicetype == 1040)
                {
                    strsql = strsql + "ID=37 order by ID";
                }
                else if (devicetype == 1041 )
                {
                    strsql = strsql + "ID=38 or ID=39 or ID=40 order by ID";
                }
                dr = DataModule.SearchAResultSQLDB(strsql);
                if (dr != null)
                {
                    while (dr.Read())
                    {
                        if (CsConst.iLanguageId == 1) cb.Items.Add(dr.GetValue(1).ToString());
                        else if (CsConst.iLanguageId == 0) cb.Items.Add(dr.GetValue(2).ToString());
                    }
                    dr.Close();
                }
            }
            catch
            {
                dr.Close();
            }
        }

        public static int GetRS232ModeByString(string str)
        {
            int ID = 0;
            OleDbDataReader dr = null;
            try
            {
                string strsql = string.Format("select ID from defRS232ToBUSModeMusic where NoteInChn='{0}' or NoteInEng='{1}'",
                    str, str);
                dr = DataModule.SearchAResultSQLDB(strsql);
                if (dr != null)
                {
                    while (dr.Read())
                    {
                        ID = Convert.ToInt32(dr.GetValue(0));
                    }
                }
                dr.Close();
            }
            catch
            {
            }
            return ID;
        }

        public static int GetSelectedIndexWhichContainedPartString(ComboBox oCob, String sItem)
        {
            int sDateTime = -1;
            int iFinalTimeZone = -1;
            try
            {
                if (oCob.Items == null || oCob.Items.Count ==0) return 0;

                foreach (String sTmp in oCob.Items)
                {
                    if (sTmp.Contains(sItem))
                    {
                        return iFinalTimeZone + 1;
                    }
                    iFinalTimeZone++;
                }
                if (sDateTime == -1) sDateTime = 0;
            }
            catch
            {
                if (sDateTime == -1) sDateTime = 0;
                return sDateTime;
            }
            return sDateTime;
        }

        public static String ReadDeviceDateTime(Byte SubNetID, Byte DeviceID)
        {
            String sDateTime = "";
            try
            {
                if (CsConst.mySends.AddBufToSndList(null, 0xDA00, SubNetID, DeviceID, false, true, true, false) == true)
                {
                    byte bytYear = CsConst.myRevBuf[26];
                    byte bytMonth = CsConst.myRevBuf[27];
                    byte bytDay = CsConst.myRevBuf[28];
                    byte bytHour = CsConst.myRevBuf[29];
                    byte bytMinute = CsConst.myRevBuf[30];
                    byte bytSecond = CsConst.myRevBuf[31];

                    if (bytHour > 23) bytHour = 0;
                    if (bytMinute > 59) bytMinute = 0;
                    if (bytSecond > 59) bytSecond = 0;
                    if (bytMonth > 12) bytMonth = 1;
                    if (bytDay > 31) bytDay = 1;

                    DateTime d = new DateTime(bytYear  + 2000, Convert.ToInt32(bytMonth), Convert.ToInt32(bytDay),
                                                               Convert.ToInt32(bytHour), Convert.ToInt32(bytMinute), Convert.ToInt32(bytSecond));

                    sDateTime = d.Date.ToString(); // ToLongDateString();
                }
            }
            catch
            {
                return sDateTime;
            }
            return sDateTime;
        }

        #region 返回组装好的json字符串
        public static String StructToJsonToString(int iId, String sName, int iTypeIds,int iLevel)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("\"information\":");
            sb.Append("{");
            sb.Append("\"id\":" + iId.ToString() + ",");
            sb.Append("\"name\":\"" + sName + "\",");
            sb.Append("\"category\":" + iTypeIds +  ",");
            sb.Append("\"level\":" + iLevel);
            sb.Append("}");            
            return sb.ToString();
        }

        public static String StructHeadToJsonToString(int iId, String sName)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("\"id\":" + iId.ToString() + ",");
            sb.Append("\"name\":\"" + sName + "\",");
            sb.Append("\"services\":" );
            sb.Append("[");
            return sb.ToString();
        }
        #endregion 

        public static String SimpleListConvertToJsonString()
        {
            StringBuilder sJson = new StringBuilder();
            if (CsConst.simpleSearchDevicesList == null || CsConst.simpleSearchDevicesList.Count == 0) return sJson.ToString();
            try
            {
                sJson.Append("{");
                sJson.Append("\"services\":");
                sJson.Append("[");
                //String sLight = "\"type\":\"" + "light" + "\",";
               // String sCurtain = "\"type\":\"" + "curtain" + "\",";
               // String sButton = "\"type\":\"" + "curtain" + "\",";
                Boolean bLight = false, bCurtain = false, bButton = false;
                String sJsonContentHead = String.Format("'{0} : ['", "services");
                Boolean bIsHas = false;
                foreach (HdlDevice temp in CsConst.simpleSearchDevicesList)
                {
                    if (temp is HdlSimpleLights)
                    {
                        bLight = true;
                        foreach (DimmerChannel oChn in ((HdlSimpleLights)temp).sumLightChns)
                        {
                            if (oChn.curStatus != null && oChn.curStatus.Length >= 2 && oChn.curStatus[1] == 0xF8)
                            {
                                if (bIsHas == false) bIsHas = true;
                                sJson.Append("{");
                                String sLight = "\"type\":\"" + "light" + "\",";
                                int iId = Convert.ToInt32(temp.subnetID.ToString("X2") + temp.deviceID.ToString("X2") + oChn.id.ToString("X2"),16);
                                sLight += StructToJsonToString(iId, oChn.remark, temp.bigType * 256 + temp.smallType, 0);
                                sJson.Append(sLight);
                                sJson.Append("},");
                            }
                        }
                    }
                    else if (temp is HdlSimpleCurtains)
                    {
                        bCurtain = true;
                        int iTmpCurtainId = 1;
                        foreach (BasicCurtain oChn in ((HdlSimpleCurtains)temp).sumCurtainChns)
                        {                            
                            if (oChn.curStatus != null && oChn.curStatus.Length >= 2 && oChn.curStatus[1] == 0xF8)
                            {
                                if (bIsHas == false) bIsHas = true;
                                sJson.Append("{");
                                String sCurtain = "\"type\":\"" + "curtain" + "\",";
                                int iId = Convert.ToInt32(temp.subnetID.ToString("X2") + temp.deviceID.ToString("X2") + iTmpCurtainId.ToString("X2"), 16);
                                sCurtain += StructToJsonToString(iId, oChn.remark, temp.bigType * 256 + temp.smallType, 0);
                                sJson.Append(sCurtain);
                                sJson.Append("},");
                            }
                            iTmpCurtainId++;
                        }
                    }
                    else if (temp is HdlSimpleButtons)
                    {
                        bButton = true;
                        foreach (HDLButton oChn in ((HdlSimpleButtons)temp).sumButtonDrys)
                        {
                            if (oChn.curStatus != null && oChn.curStatus.Length >= 2 && oChn.curStatus[1] == 0xF8)
                            {
                                if (bIsHas == false) bIsHas = true;
                                sJson.Append("{");
                                String sButton = "\"type\":\"" + "curtain" + "\",";
                                int iId = Convert.ToInt32(temp.subnetID.ToString("X2") + temp.deviceID.ToString("X2") + oChn.ID.ToString("X2"), 16);
                                sButton += StructToJsonToString(iId, oChn.Remark, temp.bigType * 256 + temp.smallType, 0);
                                sJson.Append(sButton);
                                sJson.Append("},");
                            }
                        }
                    }
                }
                sJson.Remove(sJson.Length - 1, 1);
                sJson.Append("]");
                sJson.Append("}");

                if (bIsHas == false) sJson = new StringBuilder();
            }
            catch
            {
                return sJson.ToString();
            }
            return sJson.ToString();
        }

        public static String GlobleSceneConvertToJsonString()
        {
            if (CsConst.myTemplates == null || CsConst.myTemplates.Count == 0) return "";
            StringBuilder sJson = new StringBuilder();
            Boolean bIsHas = false;
            try
            {
                sJson.Append("{");
                sJson.Append("\"scenes\":");
                sJson.Append("[");

                Boolean bLight = false, bCurtain = false, bButton = false;
                String sJsonContentHead = "";

                foreach (ControlTemplates temp in CsConst.myTemplates)
                {                    
                    if (temp.GpCMD == null || temp.GpCMD.Count == 0) continue;
                    if (temp.isSelected == false) continue;
                    if (bIsHas == false) bIsHas = true;

                    sJson.Append("{");
                    String sButton = "";
                    sButton += StructHeadToJsonToString(temp.ID, temp.Name);
                    sJson.Append(sButton);
                    foreach (UVCMD.ControlTargets oTmp in temp.GpCMD)
                    {
                        #region
                        if (oTmp.Type == 89) // 单路调节
                        {
                            int iId = Convert.ToInt32(oTmp.SubnetID.ToString("X2") + oTmp.DeviceID.ToString("X2") + oTmp.Param1.ToString("X2"), 16);
                            String sName = "light";
                            #region
                            StringBuilder sb = new StringBuilder();
                            sb.Append("{");
                            sb.Append("\"id\":" + iId.ToString() + ",");
                            sb.Append("\"type\":\"" + sName + "\",");
                            sb.Append("\"level\":" + oTmp.Param2);
                            sb.Append("},");
                            sButton = sb.ToString();
                            #endregion
                        }
                        else if (oTmp.Type == 92) // 窗帘
                        {
                            int iId = Convert.ToInt32(oTmp.SubnetID.ToString("X2") + oTmp.DeviceID.ToString("X2") + oTmp.Param1.ToString("X2"), 16);
                            String sName = "curtain";
                            #region
                            StringBuilder sb = new StringBuilder();
                            sb.Append("{");
                            sb.Append("\"id\":" + iId.ToString() + ",");
                            sb.Append("\"type\":\"" + sName + "\",");
                            sb.Append("\"status\":\"" + CsConst.CurtainModes[oTmp.Param2].ToLower() + "\"");
                            sb.Append("},");
                            sButton = sb.ToString();
                            #endregion
                        }
                        else if (oTmp.Type == 88) // 通用开关
                        {
                            int iId = Convert.ToInt32(oTmp.SubnetID.ToString("X2") + oTmp.DeviceID.ToString("X2") + oTmp.Param1.ToString("X2"), 16);
                            String sName = "infrared";
                            #region
                            StringBuilder sb = new StringBuilder();
                            sb.Append("{");
                            sb.Append("\"id\":" + iId.ToString() + ",");
                            sb.Append("\"type\":\"" + sName + "\",");
                            sb.Append("\"status\":\"" + CsConst.UvSwitchStatus[oTmp.Param2 / 255].ToLower() + "\"");
                            sb.Append("},");
                            sButton = sb.ToString();
                            #endregion
                        }
                        else if (oTmp.Type == 85) // 场景
                        {
                            int iId = Convert.ToInt32(oTmp.SubnetID.ToString("X2") + oTmp.DeviceID.ToString("X2") + oTmp.Param1.ToString("X2"), 16);
                            String sName = "scene";
                            #region
                            StringBuilder sb = new StringBuilder();
                            sb.Append("{");
                            sb.Append("\"id\":" + iId.ToString() + ",");
                            sb.Append("\"type\":\"" + sName + "\",");
                            sb.Append("\"status\":\"" + CsConst.CurtainModes[oTmp.Param2] + "\"");
                            sb.Append("},");
                            sButton = sb.ToString();
                            #endregion
                        }
                        #endregion
                        sJson.Append(sButton);
                    }
                    sJson.Remove(sJson.Length - 1, 1);
                    sJson.Append("]");
                    sJson.Append("},");
                }
                sJson.Remove(sJson.Length - 1, 1);
                sJson.Append("]");
                sJson.Append("}");
                if (bIsHas == false) sJson = new StringBuilder();
            }
            catch
            {
                return sJson.ToString();
            }
            return sJson.ToString();
        }

        public static byte GetDayofMonth(byte Month, byte DayWeek, byte Week)
        {
            byte resulyDay = 1;
            try
            {
                int Year = DateTime.Now.Year;
                DateTime CurDate = new DateTime(Year, Month, 1);
                byte CurDay = Convert.ToByte(CurDate.AddMonths(1).AddDays(-1).Day);
                byte WeekID = 0;
                for (byte i = 1; i <= CurDay; i++)
                {
                    CurDate = new DateTime(Year, Month, i);
                    if (Convert.ToByte(CurDate.DayOfWeek.GetHashCode().ToString()) == DayWeek)
                    {
                        WeekID = Convert.ToByte(WeekID + 1);
                        resulyDay = i;
                    }
                    if (1 <= Week && Week <= 4)
                    {
                        if (Week == WeekID) break;
                    }
                }
            }
            catch
            {
            }
            return resulyDay;
        }

        public static List<Byte[]> CreatePublicPinTemplatesListByteArrayFromLogic(Logic oLogic)
        {
            if (oLogic == null || oLogic.MyDesign == null || oLogic.MyDesign.Length == 0) return null;
            List<Byte[]> LogicTmpPinLists = new List<byte[]>();
            try
            {
                foreach (Logic.LogicBlock oTmp in oLogic.MyDesign)
                {
                    if (oTmp.MyPins != null && oTmp.MyPins.Count > 0)
                    {
                        foreach (Byte[] TmpFourPins in oTmp.MyPins)
                        {
                            if (TmpFourPins != null && TmpFourPins.Length > 0)
                            {
                                for (Byte PinID = 0; PinID < 4; PinID++)
                                {
                                    Byte[] SinglePin = new Byte[8];
                                    Array.Copy(TmpFourPins, 18 + PinID * 8, SinglePin, 0, 8);
                                    if (SinglePin[0] <= 1) continue;
                                    if (HDLPF.IsHasSameBufferArrayInList(SinglePin,LogicTmpPinLists) == false)  LogicTmpPinLists.Add(SinglePin);
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
                return LogicTmpPinLists;
            }
            return LogicTmpPinLists;
        }

        public static void DisplayLogicTemplates(List<Byte[]> oLogic, ListView oLv,Boolean SinglePin)
        {
            oLv.Items.Clear();
            if (oLogic == null || oLogic.Count == 0) return;
            List<String> LogicTmpPinLists = new List<string>();
            try
            {
                foreach (Byte[] TempPins in oLogic)
                {
                    string[] ArayHint = HDLSysPF.GetDescriptionsFromBuffer(TempPins, SinglePin);
                    foreach (String TmpHint in ArayHint)
                    {
                        if (TmpHint == null || TmpHint == "") continue;
                        if (LogicTmpPinLists.Contains(TmpHint)) continue;
                        LogicTmpPinLists.Add(TmpHint);
                        ListViewItem tmp = new ListViewItem();
                        tmp.SubItems.Add((oLv.Items.Count + 1).ToString());
                        tmp.SubItems.Add(TmpHint);
                        oLv.Items.Add(tmp);
                    }
                }
            }
            catch
            { }
        }

        //获取取第index位  
        public static int GetBit(byte b, int index)
        {
            return ((b & (1 << index)) > 0) ? 1 : 0;
        }
        //将第index位设为1  
        public static byte SetBit(byte b, int index)
        {
            return (byte)(b | (1 << index));
        }
        //将第index位设为0   
        public static byte ClearBit(byte b, int index)
        {
            return (byte)(b & (byte.MaxValue - (1 << index)));
        }
        //将第index位取反   
        public static byte ReverseBit(byte b, int index)
        {
            return (byte)(b ^ (byte)(1 << index));
        }

        public static string[] GetDescriptionsFromBuffer(byte[] oTmpLogicPins)
        {
            if (oTmpLogicPins == null) return null;

            string[] Result = new string[4];
            for (byte bytI = 0; bytI <= 3; bytI++)
            {
                string strTmp = "";
                byte bytStart = (byte)(18 + bytI * 8);
                if (oTmpLogicPins[bytStart] < 0 || oTmpLogicPins[bytStart] > 14)
                    oTmpLogicPins[bytStart] = 0;
                strTmp = CsConst.LogicHints[oTmpLogicPins[bytStart]];
                switch (oTmpLogicPins[bytStart])
                {
                    case 2:  //年类型
                        #region
                        if (oTmpLogicPins[bytStart + 1] == 1) //指定年
                        {
                            strTmp += " " + (2000 + oTmpLogicPins[bytStart + 2]).ToString();
                        }
                        else if (oTmpLogicPins[bytStart + 1] == 2) //指定某天某年
                        {
                            strTmp += " Y/M/D" + (2000 + oTmpLogicPins[bytStart + 2]).ToString() + "/"
                                                      + oTmpLogicPins[bytStart + 3].ToString() + "/"
                                                      + oTmpLogicPins[bytStart + 4].ToString();
                        }
                        else if (oTmpLogicPins[bytStart + 1] == 3) //指定哪年到哪年
                        {
                            strTmp += " " + (2000 + oTmpLogicPins[bytStart + 2]).ToString() + "-" + (2000 + oTmpLogicPins[bytStart + 3]).ToString();
                        }
                        else if (oTmpLogicPins[bytStart + 1] == 2) //指定某天某年
                        {
                            strTmp += " Y/M/D" + (2000 + oTmpLogicPins[bytStart + 2]).ToString() + "/"
                                                      + oTmpLogicPins[bytStart + 3].ToString() + "/"
                                                      + oTmpLogicPins[bytStart + 4].ToString() + "-"
                                                      + (2000 + oTmpLogicPins[bytStart + 5]).ToString() + "/"
                                                      + oTmpLogicPins[bytStart + 6].ToString() + "/"
                                                      + oTmpLogicPins[bytStart + 7].ToString();
                        }
                        break;
                        #endregion

                    case 3: //日期类型
                        #region
                        if (oTmpLogicPins[bytStart + 1] == 1) //指定哪天
                        {
                            strTmp += " M/D" + (oTmpLogicPins[bytStart + 2]).ToString() + "/"
                                             + oTmpLogicPins[bytStart + 3].ToString();
                        }
                        else if (oTmpLogicPins[bytStart + 1] == 2) //从什么时候到什么时候
                        {
                            strTmp += " M/D" + (2000 + oTmpLogicPins[bytStart + 2]).ToString() + "/"
                                                      + oTmpLogicPins[bytStart + 3].ToString() + "-"
                                                      + oTmpLogicPins[bytStart + 4].ToString() + "/"
                                                      + oTmpLogicPins[bytStart + 5].ToString();
                        }
                        else if (oTmpLogicPins[bytStart + 1] == 3) //每月
                        {
                            strTmp += " Every Month " + oTmpLogicPins[bytStart + 3].ToString() + "-" + oTmpLogicPins[bytStart + 5].ToString();
                        }
                        break;
                        #endregion

                    case 4: //星期类型
                        #region
                        if (oTmpLogicPins[bytStart + 1] == 1) //指定星期几
                        {
                            strTmp += " " + CsConst.Weekdays[oTmpLogicPins[bytStart + 2]];
                        }
                        else if (oTmpLogicPins[bytStart + 1] == 2) //星期几到几
                        {
                            strTmp += " from " + oTmpLogicPins[bytStart + 2].ToString() + "-"
                                               + oTmpLogicPins[bytStart + 3].ToString();
                        }
                        break;
                        #endregion

                    case 5: //时间类型
                        #region
                        if (oTmpLogicPins[bytStart + 1] == 1) //指定时间
                        {
                            if (oTmpLogicPins[bytStart + 2] == 1) // 时间点
                            {
                                strTmp += " " + oTmpLogicPins[bytStart + 3].ToString() + ":"
                                              + oTmpLogicPins[bytStart + 4].ToString();
                            }
                            else if (oTmpLogicPins[bytStart + 2] == 2) // 日出
                            {
                                if (oTmpLogicPins[bytStart + 3] >= 0x80) //之后
                                {
                                    strTmp += " Sunrise " + (oTmpLogicPins[bytStart + 3] - 0x80).ToString() + ":"
                                                          + oTmpLogicPins[bytStart + 4].ToString() + " Later";
                                }
                                else if (oTmpLogicPins[bytStart + 3] >= 0x40) //之后
                                {
                                    strTmp += " Sunrise " + (oTmpLogicPins[bytStart + 3] - 0x40).ToString() + ":"
                                                          + oTmpLogicPins[bytStart + 4].ToString() + " Earlier";
                                }
                            }
                            else if (oTmpLogicPins[bytStart + 2] == 3) // 日落
                            {
                                if (oTmpLogicPins[bytStart + 3] >= 0x80) //之后
                                {
                                    strTmp += " Sunset " + (oTmpLogicPins[bytStart + 3] - 0x80).ToString() + ":"
                                                          + oTmpLogicPins[bytStart + 4].ToString() + " Later";
                                }
                                else if (oTmpLogicPins[bytStart + 3] >= 0x40) //之后
                                {
                                    strTmp += " Sunset " + (oTmpLogicPins[bytStart + 3] - 0x40).ToString() + ":"
                                                          + oTmpLogicPins[bytStart + 4].ToString() + " Earlier";
                                }
                            }
                        }
                        else if (oTmpLogicPins[bytStart + 1] == 2) //从什么时候到什么时候
                        {
                            strTmp += " " + oTmpLogicPins[bytStart + 3].ToString() + ":"
                                          + oTmpLogicPins[bytStart + 4].ToString() + "-"
                                          + oTmpLogicPins[bytStart + 6].ToString() + ":"
                                          + oTmpLogicPins[bytStart + 7].ToString();
                        }
                        break;
                        #endregion
                    case 6: strTmp += " " + oTmpLogicPins[bytStart + 3].ToString() + " " + CsConst.Status[oTmpLogicPins[bytStart + 4] / 255]; break;

                    case 8:
                    case 9:
                    case 7: strTmp = oTmpLogicPins[bytStart + 1].ToString() + "-" + oTmpLogicPins[bytStart + 2].ToString() +" " + strTmp + " " +
                                     oTmpLogicPins[bytStart + 3].ToString() + "-" + oTmpLogicPins[bytStart + 4].ToString(); break;

                    case 10: strTmp = oTmpLogicPins[bytStart + 1].ToString() + "-" + oTmpLogicPins[bytStart + 2].ToString() + " " + strTmp + " " +
                                      oTmpLogicPins[bytStart + 3].ToString() + " " + CsConst.Status[oTmpLogicPins[bytStart + 4]/255]; break;

                    case 11: strTmp = oTmpLogicPins[bytStart + 1].ToString() + "-" + oTmpLogicPins[bytStart + 2].ToString() + " " + strTmp + " " +
                                      oTmpLogicPins[bytStart + 3].ToString() + " " + CsConst.Status[oTmpLogicPins[bytStart + 4] / 255]; break;

                    case 12: strTmp = oTmpLogicPins[bytStart + 1].ToString() + "-" + oTmpLogicPins[bytStart + 2].ToString() + " " + strTmp + " " +
                                      oTmpLogicPins[bytStart + 3].ToString() + " " + CsConst.CurtainModes[oTmpLogicPins[bytStart + 4]]; break;

                    case 13: strTmp = oTmpLogicPins[bytStart + 1].ToString() + "-" + oTmpLogicPins[bytStart + 2].ToString() + " " + strTmp + " " +
                                     CsConst.PanelControl[oTmpLogicPins[bytStart + 3]]; break;

                    case 14: strTmp = oTmpLogicPins[bytStart + 1].ToString() + "-" + oTmpLogicPins[bytStart + 2].ToString() + " " + strTmp + " " +
                                     oTmpLogicPins[bytStart + 3] + " " + CsConst.security[oTmpLogicPins[bytStart + 4]]; break;
                    default:
                        strTmp = ""; break;

                }
                Result[bytI] = strTmp;
            }
            return Result;
        }

        public static string[] GetDescriptionsFromBuffer(byte[] oTmpLogicPins, Boolean SinglePin)
        {
            if (oTmpLogicPins == null) return null;

            string[] Result = new string[4];

            string strTmp = "";
            byte bytStart = 0;
            if (oTmpLogicPins[bytStart] < 0 || oTmpLogicPins[bytStart] > 14)
                oTmpLogicPins[bytStart] = 0;
            strTmp = CsConst.LogicHints[oTmpLogicPins[bytStart]];
            switch (oTmpLogicPins[bytStart])
            {
                case 2:  //年类型
                    #region
                    if (oTmpLogicPins[bytStart + 1] == 1) //指定年
                    {
                        strTmp += " " + (2000 + oTmpLogicPins[bytStart + 2]).ToString();
                    }
                    else if (oTmpLogicPins[bytStart + 1] == 2) //指定某天某年
                    {
                        strTmp += " Y/M/D" + (2000 + oTmpLogicPins[bytStart + 2]).ToString() + "/"
                                                    + oTmpLogicPins[bytStart + 3].ToString() + "/"
                                                    + oTmpLogicPins[bytStart + 4].ToString();
                    }
                    else if (oTmpLogicPins[bytStart + 1] == 3) //指定哪年到哪年
                    {
                        strTmp += " " + (2000 + oTmpLogicPins[bytStart + 2]).ToString() + "-" + (2000 + oTmpLogicPins[bytStart + 3]).ToString();
                    }
                    else if (oTmpLogicPins[bytStart + 1] == 2) //指定某天某年
                    {
                        strTmp += " Y/M/D" + (2000 + oTmpLogicPins[bytStart + 2]).ToString() + "/"
                                                    + oTmpLogicPins[bytStart + 3].ToString() + "/"
                                                    + oTmpLogicPins[bytStart + 4].ToString() + "-"
                                                    + (2000 + oTmpLogicPins[bytStart + 5]).ToString() + "/"
                                                    + oTmpLogicPins[bytStart + 6].ToString() + "/"
                                                    + oTmpLogicPins[bytStart + 7].ToString();
                    }
                    break;
                    #endregion

                case 3: //日期类型
                    #region
                    if (oTmpLogicPins[bytStart + 1] == 1) //指定哪天
                    {
                        strTmp += " M/D" + (oTmpLogicPins[bytStart + 2]).ToString() + "/"
                                            + oTmpLogicPins[bytStart + 3].ToString();
                    }
                    else if (oTmpLogicPins[bytStart + 1] == 2) //从什么时候到什么时候
                    {
                        strTmp += " M/D" + (2000 + oTmpLogicPins[bytStart + 2]).ToString() + "/"
                                                    + oTmpLogicPins[bytStart + 3].ToString() + "-"
                                                    + oTmpLogicPins[bytStart + 4].ToString() + "/"
                                                    + oTmpLogicPins[bytStart + 5].ToString();
                    }
                    else if (oTmpLogicPins[bytStart + 1] == 3) //每月
                    {
                        strTmp += " Every Month " + oTmpLogicPins[bytStart + 3].ToString() + "-" + oTmpLogicPins[bytStart + 5].ToString();
                    }
                    break;
                    #endregion

                case 4: //星期类型
                    #region
                    if (oTmpLogicPins[bytStart + 1] == 1) //指定星期几
                    {
                        strTmp += " " + CsConst.Weekdays[oTmpLogicPins[bytStart + 2]];
                    }
                    else if (oTmpLogicPins[bytStart + 1] == 2) //星期几到几
                    {
                        strTmp += " from " + oTmpLogicPins[bytStart + 2].ToString() + "-"
                                            + oTmpLogicPins[bytStart + 3].ToString();
                    }
                    break;
                    #endregion

                case 5: //时间类型
                    #region
                    if (oTmpLogicPins[bytStart + 1] == 1) //指定时间
                    {
                        if (oTmpLogicPins[bytStart + 2] == 1) // 时间点
                        {
                            strTmp += " " + oTmpLogicPins[bytStart + 3].ToString() + ":"
                                            + oTmpLogicPins[bytStart + 4].ToString();
                        }
                        else if (oTmpLogicPins[bytStart + 2] == 2) // 日出
                        {
                            if (oTmpLogicPins[bytStart + 3] >= 0x80) //之后
                            {
                                strTmp += " Sunrise " + (oTmpLogicPins[bytStart + 3] - 0x80).ToString() + ":"
                                                        + oTmpLogicPins[bytStart + 4].ToString() + " Later";
                            }
                            else if (oTmpLogicPins[bytStart + 3] >= 0x40) //之后
                            {
                                strTmp += " Sunrise " + (oTmpLogicPins[bytStart + 3] - 0x40).ToString() + ":"
                                                        + oTmpLogicPins[bytStart + 4].ToString() + " Earlier";
                            }
                        }
                        else if (oTmpLogicPins[bytStart + 2] == 3) // 日落
                        {
                            if (oTmpLogicPins[bytStart + 3] >= 0x80) //之后
                            {
                                strTmp += " Sunset " + (oTmpLogicPins[bytStart + 3] - 0x80).ToString() + ":"
                                                        + oTmpLogicPins[bytStart + 4].ToString() + " Later";
                            }
                            else if (oTmpLogicPins[bytStart + 3] >= 0x40) //之后
                            {
                                strTmp += " Sunset " + (oTmpLogicPins[bytStart + 3] - 0x40).ToString() + ":"
                                                        + oTmpLogicPins[bytStart + 4].ToString() + " Earlier";
                            }
                        }
                    }
                    else if (oTmpLogicPins[bytStart + 1] == 2) //从什么时候到什么时候
                    {
                        strTmp += " " + oTmpLogicPins[bytStart + 3].ToString() + ":"
                                        + oTmpLogicPins[bytStart + 4].ToString() + "-"
                                        + oTmpLogicPins[bytStart + 6].ToString() + ":"
                                        + oTmpLogicPins[bytStart + 7].ToString();
                    }
                    break;
                    #endregion
                case 6: strTmp += " " + oTmpLogicPins[bytStart + 3].ToString() + " " + CsConst.Status[oTmpLogicPins[bytStart + 4] / 255]; break;

                case 8:
                case 9:
                case 7: strTmp = oTmpLogicPins[bytStart + 1].ToString() + "-" + oTmpLogicPins[bytStart + 2].ToString() + " " + strTmp + " " +
                                    oTmpLogicPins[bytStart + 3].ToString() + "-" + oTmpLogicPins[bytStart + 4].ToString(); break;

                case 10: strTmp = oTmpLogicPins[bytStart + 1].ToString() + "-" + oTmpLogicPins[bytStart + 2].ToString() + " " + strTmp + " " +
                                    oTmpLogicPins[bytStart + 3].ToString() + " " + CsConst.Status[oTmpLogicPins[bytStart + 4]/255]; break;

                case 11: strTmp = oTmpLogicPins[bytStart + 1].ToString() + "-" + oTmpLogicPins[bytStart + 2].ToString() + " " + strTmp + " " +
                                    oTmpLogicPins[bytStart + 3].ToString() + " " + CsConst.Status[oTmpLogicPins[bytStart + 4] / 255]; break;

                case 12: strTmp = oTmpLogicPins[bytStart + 1].ToString() + "-" + oTmpLogicPins[bytStart + 2].ToString() + " " + strTmp + " " +
                                    oTmpLogicPins[bytStart + 3].ToString() + " " + CsConst.CurtainModes[oTmpLogicPins[bytStart + 4]]; break;

                case 13: strTmp = oTmpLogicPins[bytStart + 1].ToString() + "-" + oTmpLogicPins[bytStart + 2].ToString() + " " + strTmp + " " +
                                    CsConst.PanelControl[oTmpLogicPins[bytStart + 3]]; break;

                case 14: strTmp = oTmpLogicPins[bytStart + 1].ToString() + "-" + oTmpLogicPins[bytStart + 2].ToString() + " " + strTmp + " " +
                                    oTmpLogicPins[bytStart + 3] + " " + CsConst.security[oTmpLogicPins[bytStart + 4]]; break;
                case 1: strTmp = "Linked to " + oTmpLogicPins[bytStart + 1].ToString(); break;

                default:
                    strTmp = ""; break;
            }

            Result[0] = strTmp;

            return Result;
        }

        /// <summary>
        /// 根据不同类型设置设备列表的加载
        /// </summary>
        public static void DisplayDevicesListAccordingly(byte bytType, ComboBox cboDev1)
        {
            if (CsConst.MyEditMode == 0) // 工程模式
            { }
            else if (CsConst.MyEditMode == 1) // 在线模式
            {
                switch (bytType)
                {
                    case 7:  //温度传感器
                        cboDev1.Items.Clear();
                        #region
                        if (CsConst.myOnlines == null || CsConst.myOnlines.Count == 0) return;
                        foreach (DevOnLine oTmp in CsConst.myOnlines)
                        {
                            int wdDeviceType = oTmp.DeviceType;
                            if (CsConst.mintDLPDeviceType.Contains(wdDeviceType) || Eightin1DeviceTypeList.HDL8in1DeviceType.Contains(wdDeviceType)
                             || Twelvein1DeviceTypeList.HDL12in1DeviceType.Contains(wdDeviceType) 
                             || T4SensorDeviceTypeList.HDLTsensorDeviceType.Contains(wdDeviceType))
                            {
                                cboDev1.Items.Add(oTmp.bytSub.ToString() + @"-" + oTmp.bytDev.ToString() + @"\" + oTmp.DevName);
                            }
                        }
                        #endregion
                        break;
                    case 8: // 场景序列
                    case 9:
                    case 11:
                        cboDev1.Items.Clear();
                        #region
                        if (CsConst.myOnlines == null || CsConst.myOnlines.Count == 0) return;
                        foreach (DevOnLine oTmp in CsConst.myOnlines)
                        {
                            int wdDeviceType = oTmp.DeviceType;
                            if (RelayDeviceTypeList.HDLRelayDeviceTypeList.Contains(wdDeviceType) || DimmerDeviceTypeList.HDLDimmerDeviceTypeList.Contains(wdDeviceType))
                            {
                                cboDev1.Items.Add(oTmp.bytSub.ToString() + @"-" + oTmp.bytDev.ToString() + @"\" + oTmp.DevName);
                            }
                        }
                        #endregion
                        break;
                    case 10: // 通用开关
                        cboDev1.Items.Clear();
                        #region
                        if (CsConst.myOnlines == null || CsConst.myOnlines.Count == 0) return;
                        foreach (DevOnLine oTmp in CsConst.myOnlines)
                        {
                            int wdDeviceType = oTmp.DeviceType;
                            if (CsConst.minRs232DeviceType.Contains(wdDeviceType) || CsConst.minLogicDeviceType.Contains(wdDeviceType) ||
                                Twelvein1DeviceTypeList.HDL12in1DeviceType.Contains(wdDeviceType))
                            {
                                cboDev1.Items.Add(oTmp.bytSub.ToString() + @"-" + oTmp.bytDev.ToString() + @"\" + oTmp.DevName);
                            }
                        }
                        #endregion
                        break;
                    case 12:  //窗帘开关
                        cboDev1.Items.Clear();
                        #region
                        if (CsConst.myOnlines == null || CsConst.myOnlines.Count == 0) return;
                        foreach (DevOnLine oTmp in CsConst.myOnlines)
                        {
                            int wdDeviceType = oTmp.DeviceType;
                            if (CurtainDeviceType.HDLCurtainModuleDeviceType.Contains(wdDeviceType))
                            {
                                cboDev1.Items.Add(oTmp.bytSub.ToString() + @"-" + oTmp.bytDev.ToString() + @"\" + oTmp.DevName);
                            }
                        }
                        #endregion
                        break;
                    case 13:  //面板控制类型
                        cboDev1.Items.Clear();
                        #region
                        if (CsConst.myOnlines == null || CsConst.myOnlines.Count == 0) return;
                        foreach (DevOnLine oTmp in CsConst.myOnlines)
                        {
                            int wdDeviceType = oTmp.DeviceType;
                            if (CsConst.minAllPanelDeviceType.Contains(wdDeviceType))
                            {
                                cboDev1.Items.Add(oTmp.bytSub.ToString() + @"-" + oTmp.bytDev.ToString() + @"\" + oTmp.DevName);
                            }
                        }
                        #endregion
                        break;
                    case 14:  //安防命令
                        cboDev1.Items.Clear();
                        #region
                        if (CsConst.myOnlines == null || CsConst.myOnlines.Count == 0) return;
                        foreach (DevOnLine oTmp in CsConst.myOnlines)
                        {
                            int wdDeviceType = oTmp.DeviceType;
                            if (CsConst.mintSecurityType.Contains(wdDeviceType))
                            {
                                cboDev1.Items.Add(oTmp.bytSub.ToString() + @"-" + oTmp.bytDev.ToString() + @"\" + oTmp.DevName);
                            }
                        }
                        #endregion
                        break;
                }
            }
        }

        public static void DisplayDeviceNameModeDescription(String DeviceName, int DeviceType, ToolStripComboBox cboDevice, ToolStripLabel DeviceModel, ToolStripLabel DeviceDescription)
        {
            if (DeviceType == -1) return;
            cboDevice.Text = DeviceName;
            String[] strTmp = DeviceTypeList.GetDisplayInformationFromPublicModeGroup(DeviceType);

            String deviceModel = strTmp[0];
            String deviceDescription = strTmp[1];

            DeviceModel.Text = deviceModel;
            DeviceDescription.Text = deviceDescription;
        }

        public static bool IsNumeric(string value)
        {
            return Regex.IsMatch(value, @"^[+-]?\d*[.]?\d*$");
        }

        public static void CopyCMDToPublicBufferWaitPasteOrCopyToGrid(DataGridView dgvListA)
        {
            CsConst.MyCopyDataGridView = new List<object[]>();
            if (dgvListA.Rows == null || dgvListA.Rows.Count == 0) return;
            foreach (DataGridViewRow oDgRow in dgvListA.Rows)
            {
                List<Object> Tmp = new List<object>();

                foreach (DataGridViewCell TmpCell in oDgRow.Cells)
                {
                    if (TmpCell.Value == null) TmpCell.Value = "";
                    Tmp.Add(TmpCell.Value.ToString());
                }
                CsConst.MyCopyDataGridView.Add(Tmp.ToArray());
            }
        }

        public static void PasteCMDToPublicBufferWaitPasteOrCopyToGrid(DataGridView dgvListA)
        {
            if (CsConst.MyCopyDataGridView == null) return;
            dgvListA.Rows.Clear();
            foreach (Object[] oDgRow in CsConst.MyCopyDataGridView)
            {
                dgvListA.Rows.Add(oDgRow);
            }
        }

        public static String ReadDeviceMainRemark(Byte SubNetID, Byte DeviceID)
        {
            String Remark = "";
            Byte[] ArayTmp = null;
            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x000E, SubNetID, DeviceID, false, true, true, false) == false)
            {
                return Remark;
            }
            else
            {
                byte[] arayRemark = new byte[20];
                CopyRemarkBufferFrmMyRevBuffer(CsConst.myRevBuf, arayRemark, 25);
                Remark = HDLPF.Byte2String(arayRemark);
                CsConst.myRevBuf = new byte[1200];
            }
            return Remark;
        }

        public static Boolean ModifyDeviceMainRemark(Byte SubNetID, Byte DeviceID, String MainRemark, int DeviceType)
        {
            Boolean blnIsSuccess = false;
            //修改波特率
            Byte[] ArayMain = new Byte[20];
            byte[] arayTmpRemark = HDLUDP.StringToByte(MainRemark);

            CopyRemarkBufferToSendBuffer(arayTmpRemark, ArayMain, 0);

            blnIsSuccess = CsConst.mySends.AddBufToSndList(ArayMain, 0x0010, SubNetID, DeviceID, false, true, true,
                                        CsConst.minAllWirelessDeviceType.Contains(DeviceType));

            return blnIsSuccess;
        }

        public static Boolean ModifyDeviceBaudRateVersion(Byte SubNetID, Byte DeviceID,Boolean IsSpecial)
        {
            Boolean blnIsSuccess = false;
            //修改波特率
            Byte[] Tmp = new Byte[] {3,1};
            if (IsSpecial == true) Tmp[0] = 8;
            blnIsSuccess = CsConst.mySends.AddBufToSndList(Tmp, 0xE41E, SubNetID, DeviceID, false, true, true, false);
            return blnIsSuccess;
        }

        public static String ReadDeviceVersion(Byte SubNetID, Byte DeviceID, Boolean bIsShowNessage)
        {
            String currentVersion = "";

            //读取备注
            if (CsConst.mySends.AddBufToSndList(null, 0xEFFD, SubNetID, DeviceID, false, bIsShowNessage, bIsShowNessage, false) == true)
            {
                if (CsConst.myRevBuf[16] == 0) return "";
                int Length = CsConst.myRevBuf[16] - 11;
                byte[] arayRemark = new byte[Length];
                Array.Copy(CsConst.myRevBuf, 25, arayRemark, 0, Length);
                currentVersion = HDLPF.Byte2String(arayRemark);
                CsConst.myRevBuf = new byte[1200];
            }
            return currentVersion;
        }

        public static void ReadLoadType()
        {
            CsConst.myLEDs = new List<LEDLibray>();

            string str = "select * from defChnsLoad order by ID";

            OleDbDataReader dr = DataModule.SearchAResultSQLDB(str);

            if (dr != null)
            {
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        LEDLibray Tmp = new LEDLibray();

                        Tmp.LEDName = dr.GetString(2).ToString();
                        Tmp.intSumChns = dr.GetInt16(3);
                        string[] ArayStr = dr.GetString(4).Split('@');
                        Tmp.LEDChns = ArayStr.ToList();
                        CsConst.myLEDs.Add(Tmp);
                    }
                    dr.Close();
                }
            }
        }

        public static void SaveLoadType()
        {
            string strsql = "delete * from DefLoadType";
            DataModule.ExecuteSQLDatabase(strsql);

            if (CsConst.myLEDs != null)
            {
                int I = 1;
                foreach (LEDLibray tmp in CsConst.myLEDs)
                {
                    string strDetails = null;
                    for (int i = 0; i < tmp.intSumChns; i++)
                    {
                        strDetails = strDetails + tmp.LEDChns[i] + "@";
                    }
                    string str = "select * from DefLoadType where Remark=" + tmp.LEDName;
                    if (DataModule.IsExitstInDatabase(str) == false)
                    {
                        str = string.Format("insert into DefLoadType(ID,Remark,HasChns,Details) values({0},'{1}',{2},'{3}')", I, tmp.LEDName, tmp.intSumChns, strDetails);
                    }
                    else
                    {
                        str = "update dbDevChnsDR set HasChns='" + tmp.intSumChns + "',Details='" + strDetails + "' where Remark=" + tmp.LEDName;
                    }
                    DataModule.ExecuteSQLDatabase(str);
                    I++;
                }
            }

        }

        /// <summary>
        /// 12in1 函数处理开始
        /// </summary>
        /// <param name="oTv"></param>
        /// <param name="oMotion"></param>
        #region
        public static void ShowSensorsKeysTargetsInformation(TreeView oTv, Sensor_12in1 o12in1, byte bytCheck)
        {
            if (o12in1 == null) return;
            if (o12in1.logic == null) return;
            oTv.Nodes.Clear();
            foreach (Sensor_12in1.Logic oLogic in o12in1.logic)
            {
                TreeNode oNode = oTv.Nodes.Add(null, oLogic.ID.ToString() + "-" + oLogic.Remarklogic, 5, 5);
                if (oLogic.Enabled == 1)
                {
                    oNode.ImageIndex = 6;
                    oNode.SelectedImageIndex = 6;
                }
                else
                {
                    oNode.ImageIndex = 5;
                    oNode.SelectedImageIndex = 5;
                }
            }
        }
        #endregion

        public static void addIR(TreeView oTv, SendIR tempSend, bool blnRead)
        {
            oTv.Nodes.Clear();
            try
            {
                string strsql = "select * from tblRemoteDevice";
                OleDbDataReader drIR = DataModule.SearchAResultSQLDB(strsql);
                tempSend.IRCodes = new List<UVCMD.IRCode>();
                if (drIR != null)
                {
                    while (drIR.Read())
                    {
                        TreeNode Node = oTv.Nodes.Add(drIR.GetValue(0).ToString(), drIR.GetValue(1).ToString(), 0, 0);

                        if (blnRead)
                        {
                            #region
                            string strCode = string.Format("select * from tblRemoteCode where RemoteDeviceID ={0} order by ID ", drIR.GetValue(0));

                            OleDbDataReader drCode = DataModule.SearchAResultSQLDB(strCode);

                            if (drCode != null)
                            {
                                while (drCode.Read())
                                {
                                    UVCMD.IRCode Tmp = new UVCMD.IRCode();
                                    Tmp.KeyID = Convert.ToInt32(drCode.GetValue(0));
                                    Tmp.IRLoation = Convert.ToInt32(drIR.GetValue(0));
                                    Tmp.Remark1 = drIR.GetValue(1).ToString();
                                    Tmp.Remark2 = drCode.GetValue(2).ToString();
                                    Tmp.Codes = drCode.GetValue(3).ToString();
                                    Tmp.IRLength = int.Parse(drCode.GetValue(4).ToString());
                                    TreeNode TmpNode = Node.Nodes.Add(Tmp.KeyID.ToString(), drCode.GetString(2), 1, 1);
                                    tempSend.IRCodes.Add(Tmp);
                                }
                                drCode.Close();
                            }
                            #endregion
                        }
                        else
                        {
                            #region
                            if (tempSend.IRCodes == null) return;
                            for (int intI = 0; intI < tempSend.IRCodes.Count; intI++)
                            {
                                if (tempSend.IRCodes[intI].Remark1.Equals(Node.Text))
                                {
                                    TreeNode TmpNode = Node.Nodes.Add(tempSend.IRCodes[intI].KeyID.ToString(), tempSend.IRCodes[intI].Remark2, 1, 1);
                                }
                            }
                            #endregion
                        }
                    }
                    drIR.Close();

                }
            }
            catch
            {

            }
        }

        public static Byte[] CopyRemarkBufferFrmMyRevBuffer(Byte[] RevBuffer, Byte[] RemarkBuffer, Byte StartPosition)
        {
            if (RevBuffer != null)
            {
                if (RevBuffer.Length >= StartPosition + 20)
                {
                    Array.Copy(RevBuffer, StartPosition, RemarkBuffer, 0, 20);
                }
                else
                {
                    Array.Copy(RevBuffer, StartPosition, RemarkBuffer, 0, RevBuffer.Length - StartPosition);
                }
            }
            return RemarkBuffer;
        }

        public static Byte[] CopyRemarkBufferToSendBuffer(Byte[] RemarkBuffer, Byte[] SendBuffer, Byte StartPosition)
        {
            if (RemarkBuffer != null)
            {
                if (RemarkBuffer.Length <= 20) RemarkBuffer.CopyTo(SendBuffer, StartPosition);
                else Array.Copy(RemarkBuffer, 0, SendBuffer, StartPosition, 20);
            }
            return SendBuffer;
        }

        /// <summary>
        /// 下载按键模式后转换成标准按键模式的下标
        /// </summary>
        /// <param name="bytKeyMode"></param>
        /// <param name="DeviceType"></param>
        /// <returns></returns>      
        public static byte ConvertorKeyModesToPublicModeGroupAccordinglyDeviceType(byte bytKeyMode, int DeviceType)
        {
            byte bytResult = 0;
            switch (bytKeyMode)
            {
                // case 0: bytResult = 0; break;
                case 1: bytResult = 2; break;
                case 2: bytResult = 3; break;
                case 3: bytResult = 1; break;
                case 4: bytResult = 10; break;
                case 5: bytResult = 6; break;
                case 6: bytResult = 17; break;
            }
            return bytResult;
        }      

        public static void LoadButtonModeWithDifferentDeviceType(object oCbo, int DeviceType)
        {
            ComboBox TmpCbo = null;
            DataGridViewComboBoxColumn TmpDataCbo = null;

            if (oCbo is ComboBox) TmpCbo = (ComboBox)oCbo;
            else if (oCbo is DataGridViewComboBoxColumn) TmpDataCbo = (DataGridViewComboBoxColumn)oCbo; 

           String[] TmpKeyMode = ButtonMode.DisplayKeyModesAccordinglyDeviceType(DeviceType);
           if (TmpCbo != null)
           {
               TmpCbo.Items.AddRange(TmpKeyMode);
           }
           else if (TmpDataCbo != null)
           {
               TmpDataCbo.Items.AddRange(TmpKeyMode);
           }
        }

        public static void LoadDrynModeWithDifferentDeviceType(object oCbo, int DeviceType)
        {
            ComboBox TmpCbo = null;
            DataGridViewComboBoxColumn TmpDataCbo = null;

            if (oCbo is ComboBox) TmpCbo = (ComboBox)oCbo;
            else if (oCbo is DataGridViewComboBoxColumn) TmpDataCbo = (DataGridViewComboBoxColumn)oCbo;

            String[] TmpKeyMode = DryMode.DisplayKeyModesAccordinglyDeviceType(DeviceType);
            if (TmpCbo != null)
            {
                TmpCbo.Items.AddRange(TmpKeyMode);
            }
            else if (TmpDataCbo != null)
            {
                TmpDataCbo.Items.AddRange(TmpKeyMode);
            }
        }

        public static void LoadDeviceListsWithDifferentSensorType(ComboBox cboDevice, Byte bDeviceType) //0 : 温度; 1 干结点； 2：HVAC 3: 按键 干结点 ; 255 ： 全部
        {
            if (CsConst.myOnlines == null) return;
            cboDevice.Items.Clear();
            foreach (DevOnLine oTmp in CsConst.myOnlines)
            {
                switch (bDeviceType)
                {
                    case CsConst.HvacGroup: 
                        if (HVACModuleDeviceTypeList.HDLHVACModuleDeviceTypeLists.Contains(oTmp.DeviceType))
                        {
                            cboDevice.Items.Add(oTmp.DevName);
                        }
                        break;
                    case CsConst.TemperatureGroup:
                        if (DLPPanelDeviceTypeList.HDLDLPPanelDeviceTypeList.Contains(oTmp.DeviceType)) // DLP
                        {
                            cboDevice.Items.Add(oTmp.DevName);
                        }
                        break;
                    case CsConst.DryContactGroup:
                        if (MS04DeviceTypeList.HDLMS04DeviceTypeList.Contains(oTmp.DeviceType))
                        {
                            cboDevice.Items.Add(oTmp.DevName);
                        }
                        break;
                    case CsConst.ButtonSetupGroup:
                        if (MS04DeviceTypeList.HDLMS04DeviceTypeList.Contains(oTmp.DeviceType) 
                         || NormalPanelDeviceTypeList.HDLNormalPanelDeviceTypeList.Contains(oTmp.DeviceType)
                         || IPmoduleDeviceTypeList.RFIpModuleV2.Contains(oTmp.DeviceType)
                         || HotelMixModuleDeviceType.HDLRCUDeviceTypeLists.Contains(oTmp.DeviceType))
                        {
                            cboDevice.Items.Add(oTmp.DevName);
                        }
                        break;
                    case CsConst.AllDeviceGroup: cboDevice.Items.Add(oTmp.DevName);
                        break;
                }
                         
            }
        }

        public static byte GetIndexFromBuffers(DataGridView oDg, int columneID)
        {
            byte bytResult = 1;
            if (oDg.Rows == null) return bytResult;
            try
            {
                List<Byte> UsedID = new List<byte>();
                foreach (DataGridViewRow TmpRow in oDg.Rows)
                {
                    UsedID.Add(Convert.ToByte(TmpRow.Cells[columneID].Value.ToString()));
                }

                while (UsedID.Contains(bytResult))
                {
                    bytResult++;
                }
            }
            catch
            {
                return bytResult;
            }
            return bytResult;
        }

        public static int UseStringGetIndexFrmOnlineList(String DeviceName)
        {
            int ID = -1;
            if (CsConst.myOnlines == null || CsConst.myOnlines.Count == 0) return ID;
           
            foreach (DevOnLine oTmp in CsConst.myOnlines)
            {
                ID++;
                if (oTmp.DevName == DeviceName)
                {
                    return ID; 
                }
            }
            return ID;
        }

        public static void addcontrols(int col, int row, Control con, DataGridView oDG)
        {
            con.Show();
            con.Visible = true;
            Rectangle rect = oDG.GetCellDisplayRectangle(col, row, true);
            con.Size = rect.Size;
            con.Top = rect.Top;
            con.Left = rect.Left;
        }

        public static void ModifyMultilinesIfNeeds(string strTmp, int ColumnIndex, DataGridView dgv)
        {
            if (dgv.SelectedRows == null || dgv.SelectedRows.Count == 0) return;
            if (strTmp == null) strTmp = "";
            // change the value in selected more than one line
            if (dgv.SelectedRows.Count > 1)
            {
                for (int i = 0; i < dgv.SelectedRows.Count; i++)
                {
                    dgv.SelectedRows[i].Cells[ColumnIndex].Value = strTmp;
                }
            }
        }

        public static Byte GetButtonTotalCMDFromPublicModeList(Byte ButtonMode)
        {
            Byte TotalCMD = 0;
            switch (ButtonMode)
            {
                case 1:
                case 2:
                case 3:
                case 10: TotalCMD = 1; break;
                case 4:
                case 5:
                case 7:
                case 6:
                case 9:
                case 13: TotalCMD = 15; break;
                case 11: TotalCMD = 15; break;
                case 16:
                case 17: TotalCMD = 15; break;
                case 14: TotalCMD = 49; break;
            }

            return TotalCMD;
        }


        /// <summary>
        /// 添加TextBox到Dgv
        /// </summary>
        /// <param name="strTableName"></param>
        /// <param name="intID"></param>
        public static void AddTextBoxToDGV(TextBox Text, DataGridView Dgv, int ColIndex, int RowIndex)
        {
            Text.Visible = true;
            Rectangle rect = Dgv.GetCellDisplayRectangle(ColIndex, RowIndex, true);
            Text.Size = rect.Size;
            Text.Top = rect.Top;
            Text.Left = rect.Left;
            string str = Dgv[ColIndex, RowIndex].Value.ToString();
            if (str.Contains("("))
                Text.Text = str.Split('(')[0].ToString();
            else
            {
                if (!GlobalClass.IsNumeric(str))
                    Text.Text = "1";
                else
                    Text.Text = str;
            }
        }

        /// <summary>
        /// 添加Combobox到Dgv
        /// </summary>
        /// <param name="strTableName"></param>
        /// <param name="intID"></param>
        public static void AddComboboxToDGV(ComboBox cb, DataGridView Dgv, int ColIndex, int RowIndex)
        {
            cb.Visible = true;
            Rectangle rect = Dgv.GetCellDisplayRectangle(ColIndex, RowIndex, true);
            cb.Size = rect.Size;
            cb.Top = rect.Top;
            cb.Left = rect.Left;
            string str = Dgv[ColIndex, RowIndex].Value.ToString();
            cb.SelectedIndex = cb.Items.IndexOf(str);
        }

        /// <summary>
        /// 从数据库添加项到combobox
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="combobox"></param>
        public static void AddItemtoCbFromDB(string sql, ComboBox cb)
        {
            string result = "";
            try
            {
                cb.Items.Clear();
                OleDbDataReader dr = DataModule.SearchAResultSQLDB(sql);
                if (dr == null)
                {
                    result = "";
                    return;
                }
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        result = dr.GetValue(0).ToString();
                        cb.Items.Add(result);
                    }
                    dr.Close();
                }
            }
            catch
            {

            }
        }

        /// <summary>
        /// 查找备注
        /// </summary>
        /// <param name="strTableName"></param>
        /// <param name="intID"></param>
        public static string selectNoteResultFromDB(string strTableName, int intID)
        {
            string result = "";
            string sql = "";
            try
            {
                if (CsConst.iLanguageId == 0)
                {
                    sql = "select NoteInEng from " + strTableName + " where ID=" + intID.ToString();
                }
                else if (CsConst.iLanguageId == 1)
                {
                    sql = "select NoteInChn from " + strTableName + " where ID=" + intID.ToString();
                }
                OleDbDataReader dr = DataModule.SearchAResultSQLDB(sql);
                if (dr == null)
                {
                    result = "";
                    return result;
                }
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        result = dr.GetValue(0).ToString();
                    }
                    dr.Close();
                }
            }
            catch
            {
                result = "";
            }
            return result;
        }

        public static Boolean IsAlreadyOpenedForm(string FormName)
        {
            bool isOpen = false;
            foreach (Form frm in Application.OpenForms)
            {
                if (frm.Name == FormName)
                {
                    frm.BringToFront();
                    isOpen = true;
                    frm.Show();
                    if (frm.WindowState == FormWindowState.Minimized) frm.WindowState = FormWindowState.Normal;
                    break;
                }
            }
            return isOpen;
        }

        public static ushort PackCRCNew(byte[] arayPtrBuf, int intBufLen)
        {
            ushort[] crctab = new ushort[16]
	        { 
		        0xe1ce,0x1021,0x3063,0xc18c,0x50a5,0xa14a,0x70e7,0x8108,
                0x60c6,0x9129,0x4084,0xb16b,0x2042,0xd1ad,0xf1ef,0x0000
	        };

            ushort crc = 0;
            ushort dat = 0;

            for (int i = 0; i < intBufLen; i++)
            {
                dat = (ushort)(crc >> 12);
                crc <<= 4;
                crc ^= crctab[dat ^ (arayPtrBuf[i] & 0x0f)];
                dat = (ushort)(crc >> 12);
                crc <<= 4;
                crc ^= crctab[dat ^ (arayPtrBuf[i] >> 4)];
            }
            return (crc);
        }

        public static byte[] AddressNewPWDSetup(byte[] ArayMAC, byte[] ArayRandom)
        {
            int intCRC1 = 0;

            byte[] arayOnceAgain = new byte[14];
            ArayMAC.CopyTo(arayOnceAgain, 0);

            for (byte intI = 1; intI < 4; intI++)
            {
                bool blnGetResult = false;
                byte bytHigh = 0;
                byte bytLow = 0;
                #region
                switch (intI)
                {
                    case 1:
                        bytHigh = ArayRandom[11];
                        bytLow = ArayRandom[10]; break;
                    case 2:
                        bytHigh = ArayRandom[9];
                        bytLow = ArayRandom[14]; break;
                    case 3:
                        bytHigh = ArayRandom[13];
                        bytLow = ArayRandom[12]; break;
                }
                #endregion
                byte[] arayTmp = new byte[2];
                for (byte bytI = 0; bytI < 255; bytI++)
                {
                    arayTmp[0] = bytI;
                    for (byte bytJ = 0; bytJ < 255; bytJ++)
                    {
                        arayTmp[1] = bytJ;
                        intCRC1 = PackCRCNew(arayTmp, 2);
                        if (intCRC1 / 256 == bytHigh && intCRC1 % 256 == bytLow)
                        {
                            blnGetResult = true;
                            #region
                            switch (intI)
                            {
                                case 1:
                                    arayOnceAgain[10] = arayTmp[0];
                                    arayOnceAgain[9] = arayTmp[1]; break;
                                case 2:
                                    arayOnceAgain[8] = arayTmp[0];
                                    arayOnceAgain[13] = arayTmp[1]; break;
                                case 3:
                                    arayOnceAgain[12] = arayTmp[0];
                                    arayOnceAgain[11] = arayTmp[1]; break;
                            }
                            #endregion
                            break;
                        }
                    }
                    if (blnGetResult) break;
                }
            }

            for (byte bytI = 8; bytI <= 13; bytI++) { arayOnceAgain[bytI] = (byte)(arayOnceAgain[bytI] ^ 0xAA); }
            return arayOnceAgain;
        }

        public static byte[] encryption(byte[] SessionKey, byte SessionLen, byte[] KeyCode, byte KeyCodeLen)
        {
            byte sk = 0;
            byte session = 0;
            byte flag = 0;
            byte temp1 = 0;
            byte temp2 = 0;
            byte temp = 0;
            byte i = 0;
            byte j = SessionLen;

            while (j != 0)
            {
                if (flag != 0)
                {
                    session = (byte)(session + SessionKey[i]);
                    flag = 0;
                }
                else
                {
                    session = (byte)(session - SessionKey[i]);
                    flag = 1;
                }
                i = (byte)(i + 1);
                j = (byte)(j - 1);
            }
            sk = (byte)(session % 15);
            flag = 0;
            i = 0;
            j = KeyCodeLen;
            while (j != 0)
            {
                temp1 = (byte)(KeyCode[i] + (session % j));
                temp1 = (byte)((255 - temp1) + 1);
                temp2 = (byte)(temp1 & 0xf0);
                temp1 = (byte)(temp1 & 0x0f);
                switch (sk)
                {
                    case 0: temp = (byte)((temp2 >> 4) | (temp1 << 4)); break;
                    case 1: temp = (byte)((temp2 + j * 2) | (temp1 + j % 4)); break;
                    case 2: temp = (byte)((temp2 + j % 5) | (temp1 + 1)); break;
                    case 3: temp = (byte)((temp2 >> 2) | (temp1 << 2)); break;
                    case 4: temp = (byte)((temp2 - 5) | (temp1 + 4)); break;
                    case 5: temp = (byte)((temp2 % 7) | (temp1 + 4)); break;
                    case 6: temp = (byte)((temp2 + j) | (temp1)); break;
                    case 7: temp = (byte)((temp2 - j) | (temp1 + j % 2)); break;
                    case 8: temp = (byte)((255 - temp2) | (temp1 + j % 6)); break;
                    case 9: temp = (byte)((temp2 + j % 3) | (temp1 + j)); break;
                    case 10: temp = (byte)((temp2) | (temp1 + 1)); break;
                    case 11: temp = (byte)((temp2) | (temp1)); break;
                    case 12: temp = (byte)((temp2 - j) | (temp1 + 4)); break;
                    case 13: temp = (byte)((temp2) | (temp1 + j)); break;
                    case 14: temp = (byte)((temp2 - 2) | (temp1 - 4)); break;
                    default: temp = (byte)((temp2 >> 4) | (temp1 << 4)); break;
                }
                KeyCode[i] = temp;
                j = (byte)(j - 1);
                i = (byte)(i + 1);
            }
            return KeyCode;
        }

        public static bool SpecialModifyAddress(int wdDeviceType, int intType, byte bytSubID, byte bytDevID, byte[] ArayTmp)
        {
            bool result = true;
            if (intType == 0) // 进入特殊模式修改地址
            {
                bytSubID = 255;
                bytDevID = 255;
                ArayTmp = new byte[8];
            }

            if (CsConst.SpecailAddress.Contains(wdDeviceType)) // 要不要加修改地址加密界面
            {
                byte[] ArayRondom = new byte[15]; //放置收回的随机数
                ArayTmp.CopyTo(ArayRondom, 0);

                //发送MAC 
                bool isRead = false;
                UDPReceive.ClearQueueData();
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xFE03, bytSubID, bytDevID, false, false, true, false) == true)
                {
                    for (int i = 0; i < UDPReceive.receiveQueue.Count; i++)
                    {
                        byte[] readData = UDPReceive.receiveQueue.ToArray()[i];
                        if (readData[21] == 0xFE && readData[22] == 0x04)
                        {
                            for (int intI = 0; intI < 6; intI++) { ArayRondom[9 + intI] = readData[25 + intI]; }
                            isRead = true;
                            break;
                        }
                    }
                    CsConst.myRevBuf = new byte[1200];
                }
                if (!isRead) return false;

                // 验证随机数再加
                ArayRondom = AddressNewPWDSetup(ArayTmp, ArayRondom);
                UDPReceive.ClearQueueData();
                isRead = false;
                //验证数据 
                if (CsConst.mySends.AddBufToSndList(ArayRondom, 0xFE05, bytSubID, bytDevID, false, false, true, false) == true)
                {
                    for (int i = 0; i < UDPReceive.receiveQueue.Count; i++)
                    {
                        byte[] readData = UDPReceive.receiveQueue.ToArray()[i];
                        if (readData[21] == 0xFE && readData[22] == 0x06)
                        {
                            for (int intI = 0; intI < 6; intI++) { ArayRondom[8 + intI] = readData[25]; }
                            isRead = true;
                            break;
                        }
                    }
                    CsConst.myRevBuf = new byte[1200];
                }
                if (!isRead) return false;
            }
            return result;
        }

        public static void AddedDevicesIndexToAPublicListInt()
        {
            CsConst.AddedDevicesIndexGroup = new List<int>();
            if (CsConst.myOnlines != null)
            {
                if (CsConst.myOnlines.Count > 0)
                {
                    for (int i = 0; i < CsConst.myOnlines.Count; i++)
                    {
                        int intTmp = CsConst.myOnlines[i].intDIndex;
                        CsConst.AddedDevicesIndexGroup.Add(intTmp);
                    }
                }
            }
        }

        public static void ReadInfoFromIniFile()
        {
            IniFile iniFile = new IniFile(Application.StartupPath + @"\ini\Default.ini");
            //IP
            CsConst.myLocalIP = iniFile.IniReadValue("ProgramMode", "IP", "");
            if (CsConst.mstrActiveIPs == null) CsConst.myLocalIP = "0.0.0.0";
            if (CsConst.myLocalIP == "" || !CsConst.mstrActiveIPs.Contains(CsConst.myLocalIP))
            {
                CsConst.myLocalIP = CsConst.mstrActiveIPs[0];
            }

            //子网ID 设备ID
            string strTemp = iniFile.IniReadValue("WizardSetup", "SubnetID", "253");
            if (strTemp == "")
                CsConst.mbytLocalSubNetID = 253;

            iniFile.IniWriteValue("WizardSetup", "SubnetID", Convert.ToString(CsConst.mbytLocalSubNetID));

            strTemp = iniFile.IniReadValue("WizardSetup", "DeviceID", "254");
            if (strTemp == "")
                CsConst.mbytLocalDeviceID = 254;
            else
                CsConst.mbytLocalDeviceID = Convert.ToByte(strTemp);

            //设置模式
            strTemp = iniFile.IniReadValue("WizardSetup", "SetupMode", "");
            if (strTemp == "")
                CsConst.MyEditMode = 1;
            else
                CsConst.MyEditMode = Convert.ToByte(strTemp);

            //上传和下载模式
            strTemp = iniFile.IniReadValue("WizardSetup", "DownloadMode", "");
            String sIsAutoUpgradeSoftware = iniFile.IniReadValue("AutoUpgrade","IsAuto","");

            if (sIsAutoUpgradeSoftware == "true")
            {
                CsConst.bIsAutoUpgrade = true;
            }
        }

        public static void AutoScale(Form frm)
        {
            frm.Font = new Font(new Font("Calibri",9), FontStyle.Regular);
            frm.Tag = frm.Width.ToString() + "," + frm.Height.ToString();
            frm.SizeChanged += new EventHandler(frm_SizeChanged);
        }

        public static void setDataGridViewColumnsWidth(DataGridView dgv)
        {
            try
            {
                if (dgv.ColumnCount > 0)
                {
                    int Width = 0;
                    double[] arayPercent;
                    int ValidCount = 0;
                    for (int i = 0; i < dgv.ColumnCount; i++)
                    {
                        if (dgv.Columns[i].Visible)
                        {
                            Width = Width + dgv.Columns[i].Width;
                            ValidCount = ValidCount + 1;
                        }
                    }
                    arayPercent = new double[ValidCount];
                    int byt = 0;
                    for (int i = 0; i < dgv.ColumnCount; i++)
                    {
                        if (dgv.Columns[i].Visible)
                        {
                            arayPercent[byt] = Math.Round(Convert.ToDouble(dgv.Columns[i].Width) / Convert.ToDouble(Width), 2);
                            byt++;
                        }
                    }
                    byt = 0;
                    int num = 10;
                    if (dgv.ColumnCount <= 5) num = 20;
                    for (int i = 0; i < dgv.ColumnCount; i++)
                    {
                        if (dgv.Columns[i].Visible)
                        {
                            dgv.Columns[i].Width = Convert.ToInt32(dgv.Width * arayPercent[byt]) - num;
                            byt++;
                        }
                    }
                }
            }
            catch
            {
            }
        }

        public static void frm_SizeChanged(object sender, EventArgs e)
        {
            try
            {
                string[] tmp = ((Form)sender).Tag.ToString().Split(',');
                float width = (float)((Form)sender).Width / (float)Convert.ToInt16(tmp[0]);
                float heigth = (float)((Form)sender).Height / (float)Convert.ToInt16(tmp[1]);
                if (Math.Round(width) == 0 || Math.Round(heigth) == 0) return;
                ((Form)sender).Tag = ((Form)sender).Width.ToString() + "," + ((Form)sender).Height;

                foreach (Control control in ((Form)sender).Controls)
                {
                    control.Scale(new SizeF(width, heigth));
                }
            }
            catch 
            { }
        }

        public static int Compare(DateTime dt1, DateTime dt2)
        {
            return ((dt1.Hour * 60 + dt1.Minute) * 60 + dt1.Second) * 1000 + dt1.Millisecond -
                (((dt2.Hour * 60 + dt2.Minute) * 60 + dt2.Second) * 1000 + dt2.Millisecond);
        }

        public static void ShowTimeoutMessage()
        {
            string str = "Timeout";
            str = CsConst.WholeTextsList[2417].sDisplayName;
            MessageBox.Show(str);
        }

        public static void WriteInfoToIniFile()
        {
            IniFile iniFile = new IniFile(Application.StartupPath + @"\ini\Default.ini");
            //是否显示窗体
            iniFile.IniWriteValue("WizardSetup", "ShowWhenStart", Convert.ToString(CsConst.MyShowWizardorNot));

            //IP
            iniFile.IniWriteValue("ProgramMode", "IP", CsConst.myLocalIP);

            //子网ID 设备ID
            iniFile.IniWriteValue("WizardSetup", "SubnetID", Convert.ToString(CsConst.mbytLocalSubNetID));
            iniFile.IniWriteValue("WizardSetup", "DeviceID", Convert.ToString(CsConst.mbytLocalDeviceID));

            //设置模式
            iniFile.IniWriteValue("WizardSetup", "SetupMode", Convert.ToString(CsConst.MyEditMode));
        }

        public static Byte[] StructToBytes(Object structure)
        {
            Int32 size = Marshal.SizeOf(structure);
            IntPtr buffer = Marshal.AllocHGlobal(size);
            try
            {
                Marshal.StructureToPtr(structure, buffer, false);
                Byte[] bytes = new Byte[size];
                Marshal.Copy(buffer, bytes, 0, size);
                Marshal.FreeHGlobal(buffer);
                return bytes;
            }
            catch
            {
                return null;
            }
        }

        public static Object BytesToStruct(Byte[] bytes, Type strcutType)
        {
            Int32 size = Marshal.SizeOf(strcutType);
            IntPtr buffer = Marshal.AllocHGlobal(size);
            if (size > bytes.Length)
            {
                return null;
            }

            try
            {
                Marshal.Copy(bytes, 0, buffer, size);
                object obj = Marshal.PtrToStructure(buffer, strcutType);
                Marshal.FreeHGlobal(buffer);
                return obj;
            }
            catch
            {
                return null;
            }
        }

        public static void FormShow(Form frm, string strName)
        {
            if (frm == null) return;

           //h if (HDLSysPF.IsAlreadyOpenedForm(strName))
            frm.Text = strName;
            frm.StartPosition = FormStartPosition.CenterScreen;
            frm.WindowState = FormWindowState.Normal;
            HDLSysPF.AutoScale(frm);
           // LoadControlsText.DisplayTextToFormWhenFirstShow(frm);
            frm.BringToFront();
            frm.Show();
        }

        public static void ListAllSceneNameToCombobox(Object TmpObj, Byte AreaID, ComboBox ocbo, Boolean AddPowerReset)
        {
            ocbo.Items.Clear();
            if (TmpObj is Dimmer)
            {
                Dimmer oDimmer = (Dimmer)TmpObj;
                if (oDimmer == null) return;
                if (oDimmer.Areas == null || oDimmer.Areas.Count == 0) return;

                foreach (Dimmer.Scene oSce in oDimmer.Areas[AreaID].Scen)
                {
                    ocbo.Items.Add(oSce.ID.ToString() + "-" + oSce.Remark);
                }
            }
            else if (TmpObj is Relay)
            {
                Relay oDimmer = (Relay)TmpObj;
                if (oDimmer == null) return;
                if (oDimmer.Areas == null || oDimmer.Areas.Count == 0) return;

                foreach (Relay.Scene oSce in oDimmer.Areas[AreaID].Scen)
                {
                    ocbo.Items.Add(oSce.ID.ToString() + "-" + oSce.Remark);
                }               
            }
            if (AddPowerReset) ocbo.Items.Add("Scene before power off");
        }

       public static void ModifyMultilinesIfNeeds(DataGridView oDg, string strTmp, int ColumnIndex)
        {
            if (oDg.SelectedRows == null || oDg.SelectedRows.Count == 0) return;
            if (strTmp == null) strTmp = "";
            // change the value in selected more than one line
            if (oDg.SelectedRows.Count > 1)
            {
                for (int i = 0; i < oDg.SelectedRows.Count; i++)
                {
                    oDg.SelectedRows[i].Cells[ColumnIndex].Value = strTmp;
                }
            }
        }

        /// <summary>
        /// 获取设备备注
        /// </summary>
        /// <param name="bytSubID"></param>
        /// <param name="bytDevID"></param>
        /// <returns></returns>
        public static string GetRemarkAccordingly(byte bytSubID, byte bytDevID, int index)
        {
            string strResult = bytSubID + "-" + bytDevID + @"\";

            if (CsConst.MyEditMode == 1) //在线模式
            {

                if (CsConst.myOnlines == null || CsConst.myOnlines.Count == 0) return strResult + CsConst.MyUnnamed;
                string strMainRemark = "";
                foreach (DevOnLine oDev in CsConst.myOnlines)
                {
                    #region
                    if (index != -1)
                    {
                        if (oDev.intDIndex == index && oDev.bytSub == bytSubID && oDev.bytDev == bytDevID)
                        {
                            strMainRemark = oDev.DevName.Split('\\')[1].Trim();
                            break;
                        }
                    }
                    else
                    {
                        if (oDev.bytSub == bytSubID && oDev.bytDev == bytDevID)
                        {
                            strMainRemark = oDev.DevName.Split('\\')[1].Trim();
                            return strResult + strMainRemark;
                        }
                    }
                    #endregion
                }
                if (strMainRemark == "" && index !=-1) strMainRemark = CsConst.MyUnnamed;
                else if (strMainRemark == "") strMainRemark = CsConst.strWirelessAddressConnection[0]; 

                return strResult + strMainRemark;

            }
            return strResult;
        }



        /// <summary>
        /// Get object with devicetype and dindex
        /// </summary>
        /// <param name="strName"></param>
        /// <param name="wdDeviceType"></param>
        /// <param name="dIndex"></param>
        /// <returns></returns>
        public static Object FindRightObjectAccordingItsDeviceType(string strName, int wdDeviceType, int dIndex)
        {
            Object oTmpObject = null;
            #region
            if (DimmerDeviceTypeList.HDLDimmerDeviceTypeList.Contains(wdDeviceType)) // 判断是不是调光设备的类型
            {
                #region
                if (CsConst.myDimmers != null)
                {
                    foreach (Dimmer di in CsConst.myDimmers)
                    {
                        if (di.DIndex == dIndex)
                        {
                            oTmpObject = di;
                            break;
                        }
                    }
                }
                #endregion
            }
            else if (RelayDeviceTypeList.HDLRelayDeviceTypeList.Contains(wdDeviceType)) //判断是不是继电器类
            {
                #region
                if (CsConst.myRelays != null)
                {
                    foreach (Relay Tmp in CsConst.myRelays)
                    {
                        if (Tmp.DIndex == dIndex)
                        {
                            oTmpObject = Tmp;
                            break;
                        }
                    }
                }                
                #endregion
            }
            else if (IrModuleDeviceTypeList.HDNewIrModuleDeviceTypeLists.Contains(wdDeviceType))//是不是新型红外发射模块
            {

            }
            else if (CsConst.mintAllIRDeviceType.Contains(wdDeviceType)) // 判断是不是所有的红外发射类型
            {

            }
            else if (DMXDeviceTypeList.HDLDMXDeviceTypeList.Contains(wdDeviceType)) // 判断是不是所有的DMX
            {
                #region
                if (CsConst.myDmxs != null)
                {
                    foreach (DMX di in CsConst.myDmxs)
                    {
                        if (di.DIndex == dIndex)
                        {
                            oTmpObject = di;
                            break;
                        }
                    }
                }
                #endregion
            }
            else if (CurtainDeviceType.HDLCurtainModuleDeviceType.Contains(wdDeviceType)) // 判断是不是所有的窗帘模块
            {
                #region
                if (CsConst.myCurtains != null)
                {
                    foreach (Curtain Tmp in CsConst.myCurtains)
                    {
                        if (Tmp.DIndex == dIndex)
                        {
                            oTmpObject = Tmp;
                            break;
                        }
                    }
                }
                #endregion
            }
            else if (CsConst.mintMotionDeviceType.Contains(wdDeviceType))  // 判断是不是所有的动静传感器类型
            {

            }
            else if (HVACModuleDeviceTypeList.HDLHVACModuleDeviceTypeLists.Contains(wdDeviceType))      //判断是不是所有HVAC的设备类型
            {
                #region
                if (CsConst.myHvacs != null)
                {
                    foreach (HVAC Tmp in CsConst.myHvacs)
                    {
                        if (Tmp.DIndex == dIndex)
                        {
                            oTmpObject = Tmp;
                            break;
                        }
                    }
                }
                #endregion
            }
            else if (EnviroNewDeviceTypeList.EnviroNewPanelDeviceTypeList.Contains(wdDeviceType)) // 是不是新版彩屏面板
            {
                #region
                if (CsConst.myEnviroPanels != null)
                {
                    foreach (EnviroPanel Tmp in CsConst.myEnviroPanels)
                    {
                        if (Tmp.DIndex == dIndex)
                        {
                            oTmpObject = Tmp;
                            break;
                        }
                    }
                }               
                #endregion
            }
            else if (EnviroDLPPanelDeviceTypeList.HDLEnviroDLPPanelDeviceTypeList.Contains(wdDeviceType)) // 是不是彩屏面板
            {
                #region
                if (CsConst.myColorPanels != null)
                {
                    foreach (ColorDLP Tmp in CsConst.myColorPanels)
                    {
                        if (Tmp.DIndex == dIndex)
                        {
                            oTmpObject = Tmp;
                            break;
                        }
                    }
                }               
                #endregion
            }
            else if (DLPPanelDeviceTypeList.HDLDLPPanelDeviceTypeList.Contains(wdDeviceType))  //判断是不是DLP面板类型的设备
            {
                #region
                if (CsConst.myPanels != null)
                {
                    foreach (DLP Tmp in CsConst.myPanels)
                    {
                        if (Tmp.DIndex == dIndex)
                        {
                            oTmpObject = Tmp;
                            break;
                        }
                    }
                }                
                #endregion
            }
            else if (NormalPanelDeviceTypeList.HDLNormalPanelDeviceTypeList.Contains(wdDeviceType))  //判断是不是面板类型的设备
            {
                #region
                if (CsConst.myPanels != null)
                {
                    foreach (Panel Tmp in CsConst.myPanels)
                    {
                        if (Tmp.DIndex == dIndex)
                        {
                            oTmpObject = Tmp;
                            break;
                        }
                    }
                }
                #endregion
            }
            else if (MS04DeviceTypeList.WirelessMS04WithOneCurtain.Contains(wdDeviceType)) // 判断是不是干接点类型的设备窗帘
            {
                #region
                if (CsConst.myDrys != null)
                {
                    foreach (MS04 Tmp in CsConst.myDrys)
                    {
                        if (Tmp.DIndex == dIndex)
                        {
                            oTmpObject = Tmp;
                            break;
                        }
                    }
                }
                #endregion
            }
            else if (MS04DeviceTypeList.WirelessMS04WithRelayChn.Contains(wdDeviceType)) // 判断是不是干接点类型的设备继电器
            {
                #region
                if (CsConst.myDrys != null)
                {
                    foreach (MS04 Tmp in CsConst.myDrys)
                    {
                        if (Tmp.DIndex == dIndex)
                        {
                            oTmpObject = Tmp;
                            break;
                        }
                    }
                }
                #endregion
            }
            else if (MS04DeviceTypeList.WirelessMS04WithDimmerChn.Contains(wdDeviceType)) // 判断是不是干接点类型的设备调光器
            {
                #region
                if (CsConst.myDrys != null)
                {
                    foreach (MS04 Tmp in CsConst.myDrys)
                    {
                        if (Tmp.DIndex == dIndex)
                        {
                            oTmpObject = (MS04GenerationOneD)Tmp;
                            break;
                        }
                    }
                }
                #endregion
            }
            else if (MS04DeviceTypeList.MS04IModuleWithTemperature.Contains(wdDeviceType)) // 判断是不是干接点类型的设备带温度传感器
            {
                #region
                if (CsConst.myDrys != null)
                {
                    foreach (MS04 Tmp in CsConst.myDrys)
                    {
                        if (Tmp.DIndex == dIndex)
                        {
                            oTmpObject = (MS04GenerationOneT)Tmp;
                            break;
                        }
                    }
                }
                #endregion
            }
            else if (MS04DeviceTypeList.HDLMS04DeviceTypeList.Contains(wdDeviceType)) //判断是不是干接点类型的设备
            {
                #region
                if (CsConst.myDrys != null)
                {
                    foreach (MS04 Tmp in CsConst.myDrys)
                    {
                        if (Tmp.DIndex == dIndex)
                        {
                            oTmpObject = Tmp;
                            break;
                        }
                    }
                }
                #endregion
            }
            else if (AudioDeviceTypeList.AudioBoxDeviceTypeList.Contains(wdDeviceType)) // 判断是不是背景音乐设备的类型
            {
                #region
                if (CsConst.myzBoxs != null)
                {
                    foreach (MzBox Tmp in CsConst.myzBoxs)
                    {
                        if (Tmp.DIndex == dIndex)
                        {
                            oTmpObject = Tmp;
                            break;
                        }
                    }
                }
                #endregion
            }
            else if (CsConst.mintSPADeviceType.Contains(wdDeviceType)) // 判断是不是SPA专用面板的类型
            {

            }
            else if (IPmoduleDeviceTypeList.HDLIPModuleDeviceTypeLists.Contains(wdDeviceType))   //是不是IP module 模块
            {
                #region
                if (CsConst.myIPs != null)
                {
                    foreach (IPModule Tmp in CsConst.myIPs)
                    {
                        if (Tmp.DIndex == dIndex)
                        {
                            oTmpObject = Tmp;
                            break;
                        }
                    }
                }
                #endregion
            }
            else if (Eightin1DeviceTypeList.HDL8in1DeviceType.Contains(wdDeviceType))   //是不是八合一 模块
            {
                #region
                if (CsConst.mysensor_8in1 != null)
                {
                    foreach (MultiSensor Tmp in CsConst.mysensor_8in1)
                    {
                        if (Tmp.DIndex == dIndex)
                        {
                            oTmpObject = Tmp;
                            break;
                        }
                    }
                }
                #endregion
            }
            else if (CsConst.minRs232DeviceType.Contains(wdDeviceType)) // 是不是Rs232 模块
            {

            }
            else if (Twelvein1DeviceTypeList.HDL12in1DeviceType.Contains(wdDeviceType))   //是不是12合一 模块
            {

            }
            else if (CsConst.mintEIBDeviceType.Contains(wdDeviceType))  //是不是EIB转换器模块
            {

            }
            else if (SMSModuleDeviceTypeList.HDLGPRSModuleDeviceTypeLists.Contains(wdDeviceType))  //是不是GPRS模块
            {

            }
            else if (HAIModuleDeviceTypeList.HDLHAIModuleDeviceTypeLists.Contains(wdDeviceType))  //是不是HAI模块
            {
                #region
                if (CsConst.myHais != null)
                {
                    foreach (HAI Tmp in CsConst.myHais)
                    {
                        if (Tmp.DIndex == dIndex)
                        {
                            oTmpObject = Tmp;
                            break;
                        }
                    }
                }
                #endregion
            }
            else if (ConstMMC.MediaPBoxDeviceType.Contains(wdDeviceType))///是不是多媒体播放设备
            {
                #region
                if (CsConst.myMediaBoxes != null)
                {
                    foreach (MMC Tmp in CsConst.myMediaBoxes)
                    {
                        if (Tmp.DIndex == dIndex)
                        {
                            oTmpObject = Tmp;
                            break;
                        }
                    }
                }
                #endregion
            }
            else if (SecurityDeviceTypeList.HDLSecurityDeviceTypeList.Contains(wdDeviceType))  /// 是不是安防模块设备
            {
                #region
                if (CsConst.mySecurities != null)
                {
                    foreach (Security Tmp in CsConst.mySecurities)
                    {
                        if (Tmp.DIndex == dIndex)
                        {
                            oTmpObject = Tmp;
                            break;
                        }
                    }
                }
                #endregion
            }
            else if (LogicDeviceTypeList.LogicDeviceType.Contains(wdDeviceType))  //是不是逻辑模块
            {
                #region
                if (CsConst.myLogics != null)
                {
                    foreach (Logic Tmp in CsConst.myLogics)
                    {
                        if (Tmp.DIndex == dIndex)
                        {
                            oTmpObject = Tmp;
                            break;
                        }
                    }
                }
                #endregion
            }
            else if (BacNetDeviceTypeList.HDLBacNetDeviceTypeList.Contains(wdDeviceType))  // 是不是bacnet模块
            {
                #region
                if (CsConst.myBacnets != null)
                {
                    foreach (BacNet Tmp in CsConst.myBacnets)
                    {
                        if (Tmp.DIndex == dIndex)
                        {
                            oTmpObject = Tmp;
                            break;
                        }
                    }
                }
                #endregion
            }
            else if (FloorheatingDeviceTypeList.HDLFloorHeatingDeviceType.Contains(wdDeviceType))  // 是不是地热模块
            {
                #region
                if (CsConst.myFHs != null)
                {
                    foreach (FH Tmp in CsConst.myFHs)
                    {
                        if (Tmp.DIndex == dIndex)
                        {
                            oTmpObject = Tmp;
                            break;
                        }
                    }
                }
                #endregion
            }
            else if (DSDeviceTypeList.HDLDSDeviceTypeList.Contains(wdDeviceType))//是不是10寸屏室外机
            {

            }
            else if (HotelMixModuleDeviceType.HDLRCUDeviceTypeLists.Contains(wdDeviceType))//是不是客房控制器
            {
                #region
                if (CsConst.myRcuMixModules != null)
                {
                    foreach (MHRCU Tmp in CsConst.myRcuMixModules)
                    {
                        if (Tmp.DIndex == dIndex)
                        {
                            oTmpObject = Tmp;
                            break;
                        }
                    }
                }
                #endregion
            }
            else if (MSPUSenserDeviceTypeList.SensorsDeviceTypeList.Contains(wdDeviceType))//红外超声波传感器
            {
                #region
                if (CsConst.myPUSensors != null)
                {
                    foreach (MSPU Tmp in CsConst.myPUSensors)
                    {
                        if (Tmp.DIndex == dIndex)
                        {
                            oTmpObject = Tmp;
                            break;
                        }
                    }
                }
                #endregion

            }
            else if (EnviroDLPPanelDeviceTypeList.HDLEnviroDLPPanelDeviceTypeList.Contains(wdDeviceType))//彩色DLP
            {

            }
            else if (CoolMasterDeviceTypeList.HDLCoolMasterDeviceTypeList.Contains(wdDeviceType))//coolmaster
            {
                #region
                if (CsConst.myCoolMasters != null)
                {
                    foreach (CoolMaster Tmp in CsConst.myCoolMasters)
                    {
                        if (Tmp.DIndex == dIndex)
                        {
                            oTmpObject = Tmp;
                            break;
                        }
                    }
                }
                #endregion
            }
            else if (MiniSenserDeviceTypeList.SensorsDeviceTypeList.Contains(wdDeviceType))//MINI超声波
            {
                #region
                if (CsConst.myMiniSensors != null)
                {
                    foreach (MiniSensor oTmp in CsConst.myMiniSensors)
                    {
                        if (oTmp.DIndex == dIndex)
                        {
                            oTmpObject = oTmp;
                            break;
                        }
                    }
                }
                #endregion
            }
            else if (CsConst.minDoorBellDeviceType.Contains(wdDeviceType))//门铃
            {

            }
            else if (CsConst.minCardReaderDeviceType.Contains(wdDeviceType))//插卡取电
            {

            }
            else if (CsConst.mintHotelACDeviceType.Contains(wdDeviceType))//酒店空调面板
            {

            }
            else if (CsConst.minMSPUBRFDeviceType.Contains(wdDeviceType))//电池装传感器
            {

            }
            else if (DSDeviceTypeList.NewDoorStationDeviceType.Contains(wdDeviceType))//新门口机
            {

            }
            else if (SocketDeviceTypeList.HDLSocketDeviceTypeList.Contains(wdDeviceType))//无线插座
            {
                #region
                if (CsConst.mySockets != null)
                {
                    foreach (RFSocket oTmp in CsConst.mySockets)
                    {
                        if (oTmp.DIndex == dIndex)
                        {
                            oTmpObject = oTmp;
                            break;
                        }
                    }
                }
                #endregion
            }
            else if (CameraNvrDeviceType.cameraThirdPartDeviceType.Contains(wdDeviceType)) // 第三方摄像头
            {
                #region
                if (CsConst.myCameras != null)
                {
                    foreach (Camera oTmp in CsConst.myCameras)
                    {
                        if (oTmp.dIndex == dIndex)
                        {
                            oTmpObject = oTmp;
                            break;
                        }
                    }
                }
                #endregion
            }
            else if (CsConst.minFANControllerDeviceType.Contains(wdDeviceType))//风扇控制器
            {

            }
            else if (CsConst.minBrazilWirelessPanelDeviceType.Contains(wdDeviceType))//巴西无线面板
            {

            }
            else if (CsConst.minSensor_7in1DeviceType.Contains(wdDeviceType))//7合一
            {

            }
            else if (CsConst.SonosConvertorDeviceType.Contains(wdDeviceType)) // sonos 转换器
            {
            }
            #endregion
            return oTmpObject;
        }


        public static Form OpenRightFormAccordingItsDeviceType(string strName, int wdDeviceType, int dIndex)
        {
            Form frmTmp = null;
            try
            {                
                #region
                if (DimmerDeviceTypeList.HDLDimmerDeviceTypeList.Contains(wdDeviceType)) // 判断是不是调光设备的类型
                {
                    #region
                    Dimmer tempDi = null;
                    if (CsConst.myDimmers != null)
                    {
                        foreach (Dimmer di in CsConst.myDimmers)
                        {
                            if (di.DIndex == dIndex)
                            {
                                tempDi = di;
                                break;
                            }
                        }
                    }
                    if (CsConst.myDimmers == null || tempDi == null)
                    {
                        tempDi = new Dimmer();
                        tempDi.DeviceName = strName;
                        tempDi.DIndex = dIndex;
                        if (CsConst.myDimmers == null) CsConst.myDimmers = new List<Dimmer>();
                        CsConst.myDimmers.Add(tempDi);
                    }

                    if (tempDi.DeviceName != strName) tempDi.DeviceName = strName;

                    if (CsConst.mintHMIXDeviceType.Contains(wdDeviceType)) //如果是酒店专用模块进入特殊窗体
                    {
                        /*foreach (Form frm in Application.OpenForms)
                        {
                            if (frm.Name == "frmHMIX" && frm.Text == strName)
                            {
                                frmTmp = frm;
                                break;
                            }
                        }
                        if (frmTmp == null) frmTmp = new frmHMIX(tempDi, strName, wdDeviceType, dIndex);*/
                    }
                    else
                    {
                        foreach (Form frm in Application.OpenForms)
                        {
                            if (frm.Name == "frmDimmer" && frm.Text == strName)
                            {
                                frmTmp = frm;
                                break;
                            }
                        }
                        if (frmTmp == null) frmTmp = new frmDimmer(tempDi, strName, wdDeviceType, dIndex);
                    }
                    #endregion
                }
                else if (RelayDeviceTypeList.HDLRelayDeviceTypeList.Contains(wdDeviceType)) //判断是不是继电器类
                {
                    #region
                    Relay OIP = null;
                    if (CsConst.myRelays != null)
                    {
                        foreach (Relay Tmp in CsConst.myRelays)
                        {
                            if (Tmp.DIndex == dIndex)
                            {
                                OIP = Tmp;
                                break;
                            }
                        }
                    }
                    if (CsConst.myRelays == null || OIP == null)
                    {
                        OIP = new Relay();
                        OIP.DeviceName = strName;
                        OIP.DIndex = dIndex;
                        OIP.ReadDefaultInfo(wdDeviceType);
                        if (CsConst.myRelays == null) CsConst.myRelays = new List<Relay>(); CsConst.myRelays.Add(OIP);
                    }
                    if (OIP.DeviceName != strName) OIP.DeviceName = strName;
                    foreach (Form frm in Application.OpenForms)
                    {
                        if (frm.Name == "frmRelay" && frm.Text == strName)
                        {
                            frmTmp = frm;
                            break;
                        }
                    }
                    if (frmTmp == null) frmTmp = new frmRelay(OIP, strName, dIndex, wdDeviceType);
                    #endregion
                }
                else if (IrModuleDeviceTypeList.HDNewIrModuleDeviceTypeLists.Contains(wdDeviceType))//是不是新型红外发射模块
                {
                    #region
                    NewIR send = null;
                    if (CsConst.myNewIR != null)
                    {
                        foreach (NewIR ir in CsConst.myNewIR)
                        {
                            if (ir.DIndex == dIndex)
                            {
                                send = ir;
                            }
                        }
                    }
                    else
                    {
                        send.DIndex = dIndex;
                        send.strName = strName;
                        CsConst.myNewIR = new List<NewIR>(); CsConst.myNewIR.Add(send);
                    }
                    foreach (Form frm in Application.OpenForms)
                    {
                        if (frm.Name == "FrmNewIREmitor" && frm.Text == strName)
                        {
                            frmTmp = frm;
                            break;
                        }
                    }
                    if (frmTmp == null) frmTmp = new FrmNewIREmitor(send, strName, dIndex, wdDeviceType);
                    #endregion
                }
                else if (CsConst.mintAllIRDeviceType.Contains(wdDeviceType)) // 判断是不是所有的红外发射类型
                {
                    #region
                    #endregion
                }
                else if (DMXDeviceTypeList.HDLDMXDeviceTypeList.Contains(wdDeviceType)) // 判断是不是所有的DMX
                {
                    #region
                    DMX tempDi = null;
                    if (CsConst.myDmxs != null)
                    {
                        foreach (DMX di in CsConst.myDmxs)
                        {
                            if (di.DIndex == dIndex)
                            {
                                tempDi = di;
                                break;
                            }
                        }
                    }
                    if (CsConst.myDmxs == null || tempDi == null)
                    {
                        tempDi = new DMX();
                        tempDi.DeviceName = strName;
                        tempDi.DIndex = dIndex;
                        if (CsConst.myDmxs == null) CsConst.myDmxs = new List<DMX>();
                        CsConst.myDmxs.Add(tempDi);
                    }
                    if (tempDi.DeviceName != strName) tempDi.DeviceName = strName;
                    foreach (Form frm in Application.OpenForms)
                    {
                        if (frm.Name == "frmDMX" && frm.Text == strName)
                        {
                            frmTmp = frm;
                            break;
                        }
                    }
                    if (frmTmp == null) frmTmp = new frmDMX(tempDi, strName, wdDeviceType, dIndex);
                    #endregion
                }
                else if (CurtainDeviceType.HDLCurtainModuleDeviceType.Contains(wdDeviceType)) // 判断是不是所有的窗帘模块
                {
                    #region
                    Curtain OIP = null;
                    if (CsConst.myCurtains != null)
                    {
                        foreach (Curtain Tmp in CsConst.myCurtains)
                        {
                            if (Tmp.DIndex == dIndex)
                            {
                                OIP = Tmp;
                                break;
                            }
                        }
                    }
                    if (CsConst.myCurtains == null || OIP == null)
                    {
                        OIP = new Curtain();
                        OIP.DeviceName = strName;
                        OIP.DIndex = dIndex;
                        OIP.ReadDefaultInfo();
                        if (CsConst.myCurtains == null) CsConst.myCurtains = new List<Curtain>();
                        CsConst.myCurtains.Add(OIP);
                    }
                    if (OIP.DeviceName != strName) OIP.DeviceName = strName;
                    foreach (Form frm in Application.OpenForms)
                    {
                        if (frm.Name == "frmRelay" && frm.Text == strName)
                        {
                            frmTmp = frm;
                            break;
                        }
                    }
                    if (frmTmp == null) frmTmp = new frmCurtain(OIP, strName, dIndex, wdDeviceType);
                    #endregion
                }
                else if (CsConst.mintMotionDeviceType.Contains(wdDeviceType))  // 判断是不是所有的动静传感器类型
                {

                }
                else if (HVACModuleDeviceTypeList.HDLHVACModuleDeviceTypeLists.Contains(wdDeviceType))      //判断是不是所有HVAC的设备类型
                {
                    #region
                    HVAC OIP = null;
                    if (CsConst.myHvacs != null)
                    {
                        foreach (HVAC Tmp in CsConst.myHvacs)
                        {
                            if (Tmp.DIndex == dIndex)
                            {
                                OIP = Tmp;
                                break;
                            }
                        }
                    }
                    if (CsConst.myHvacs == null || OIP == null)
                    {
                        OIP = new HVAC();
                        OIP.DeviceName = strName;
                        OIP.DIndex = dIndex;
                        OIP.ReadDefaultInfo();
                        if (CsConst.myHvacs == null) CsConst.myHvacs = new List<HVAC>(); CsConst.myHvacs.Add(OIP);
                    }
                    if (OIP.DeviceName != strName) OIP.DeviceName = strName;
                    foreach (Form frm in Application.OpenForms)
                    {
                        if (frm.Name == "frmHVAC" && frm.Text == strName)
                        {
                            frmTmp = frm;
                            break;
                        }
                    }
                    if (frmTmp == null) frmTmp = new frmHVAC(OIP, strName, dIndex, wdDeviceType);
                    #endregion
                }
                else if (EnviroNewDeviceTypeList.EnviroNewPanelDeviceTypeList.Contains(wdDeviceType)) // 是不是新版彩屏面板
                {
                    #region
                    EnviroPanel OIP = null;
                    if (CsConst.myEnviroPanels != null)
                    {
                        foreach (EnviroPanel Tmp in CsConst.myEnviroPanels)
                        {
                            if (Tmp.DIndex == dIndex)
                            {
                                OIP = Tmp;
                                break;
                            }
                        }
                    }
                    if (CsConst.myEnviroPanels == null || OIP == null)
                    {
                        OIP = new EnviroPanel();
                        OIP.DeviceName = strName;
                        OIP.DIndex = dIndex;
                        OIP.ReadDefaultInfo();
                        if (CsConst.myEnviroPanels == null) CsConst.myEnviroPanels = new List<EnviroPanel>();
                        CsConst.myEnviroPanels.Add(OIP);
                    }

                    foreach (Form frm in Application.OpenForms)
                    {
                        if (frm.Name == "frmDLP" && frm.Text == strName)
                        {
                            frmTmp = frm;
                            break;
                        }
                    }
                    if (frmTmp == null) frmTmp = new frmEnviro(OIP, strName, wdDeviceType, dIndex);
                    #endregion
                }
                else if (EnviroDLPPanelDeviceTypeList.HDLEnviroDLPPanelDeviceTypeList.Contains(wdDeviceType)) // 是不是彩屏面板
                {
                    #region
                    ColorDLP OIP = null;
                    if (CsConst.myColorPanels != null)
                    {
                        foreach (ColorDLP Tmp in CsConst.myColorPanels)
                        {
                            if (Tmp.DIndex == dIndex)
                            {
                                OIP = Tmp;
                                break;
                            }
                        }
                    }
                    if (CsConst.myColorPanels == null || OIP == null)
                    {
                        OIP = new ColorDLP();
                        OIP.DeviceName = strName;
                        OIP.DIndex = dIndex;
                        OIP.ReadDefaultInfo();
                        if (CsConst.myColorPanels == null) CsConst.myColorPanels = new List<ColorDLP>(); CsConst.myColorPanels.Add(OIP);
                    }

                    foreach (Form frm in Application.OpenForms)
                    {
                        if (frm.Name == "frmDLP" && frm.Text == strName)
                        {
                            frmTmp = frm;
                            break;
                        }
                    }
                    if (frmTmp == null) frmTmp = new FrmColorDLP(OIP, strName, wdDeviceType, dIndex);
                    #endregion
                }
                else if (DLPPanelDeviceTypeList.HDLDLPPanelDeviceTypeList.Contains(wdDeviceType))  //判断是不是DLP面板类型的设备
                {
                    #region
                    DLP OIP = null;
                    if (CsConst.myPanels != null)
                    {
                        foreach (Panel Tmp in CsConst.myPanels)
                        {
                            if (Tmp is DLP)
                            {
                                if (Tmp.DIndex == dIndex)
                                {
                                    OIP = (DLP)Tmp;
                                    break;
                                }
                            }
                        }
                    }
                    if (CsConst.myPanels == null || OIP == null)
                    {
                        OIP = new DLP();
                        OIP.DeviceName = strName;
                        OIP.DIndex = dIndex;
                        OIP.ReadDefaultInfo(wdDeviceType);
                        if (CsConst.myPanels == null) CsConst.myPanels = new List<Panel>(); CsConst.myPanels.Add(OIP);
                    }

                    foreach (Form frm in Application.OpenForms)
                    {
                        if (frm.Name == "frmDLP" && frm.Text == strName)
                        {
                            frmTmp = frm;
                            break;
                        }
                    }
                    if (frmTmp == null) frmTmp = new frmDLP(OIP, strName, wdDeviceType, dIndex);
                    #endregion
                }
                else if (NormalPanelDeviceTypeList.HDLNormalPanelDeviceTypeList.Contains(wdDeviceType))  //判断是不是面板类型的设备
                {
                    #region
                    Panel OIP = null;
                    if (CsConst.myPanels != null)
                    {
                        foreach (Panel Tmp in CsConst.myPanels)
                        {
                            if (Tmp.DIndex == dIndex)
                            {
                                OIP = Tmp;
                                break;
                            }
                        }
                    }
                    if (CsConst.myPanels == null || OIP == null)
                    {
                        OIP = new Panel();
                        OIP.DeviceName = strName;
                        OIP.DIndex = dIndex;
                        OIP.ReadDefaultInfo(wdDeviceType);
                        if (CsConst.myPanels == null) CsConst.myPanels = new List<Panel>(); CsConst.myPanels.Add(OIP);
                    }

                    foreach (Form frm in Application.OpenForms)
                    {
                        if (frm.Name == "frmPanel" && frm.Text == strName)
                        {
                            frmTmp = frm;
                            break;
                        }
                    }
                    if (frmTmp == null) frmTmp = new frmPanel(OIP, strName, wdDeviceType, dIndex);
                    #endregion
                }
                else if (MS04DeviceTypeList.WirelessMS04WithOneCurtain.Contains(wdDeviceType)) // 判断是不是干接点类型的设备窗帘
                {
                    #region
                    MS04GenerationOneCurtain OIP = null;
                    if (CsConst.myDrys != null)
                    {
                        foreach (MS04 Tmp in CsConst.myDrys)
                        {
                            if (Tmp.DIndex == dIndex)
                            {
                                OIP = (MS04GenerationOneCurtain)Tmp;
                                break;
                            }
                        }
                    }
                    if (CsConst.myDrys == null || OIP == null)
                    {
                        OIP = new MS04GenerationOneCurtain();
                        OIP.DeviceName = strName;
                        OIP.DIndex = dIndex;
                        OIP.ReadDefaultInfo(wdDeviceType);
                        if (CsConst.myDrys == null) CsConst.myDrys = new List<MS04>(); CsConst.myDrys.Add(OIP);
                    }

                    foreach (Form frm in Application.OpenForms)
                    {
                        if (frm.Name == "frmMS04" && frm.Text == strName)
                        {
                            frmTmp = frm;
                            break;
                        }
                    }
                    if (frmTmp == null) frmTmp = new frmMS04(OIP, strName, wdDeviceType, dIndex);
                    #endregion
                }
                else if (MS04DeviceTypeList.WirelessMS04WithRelayChn.Contains(wdDeviceType)) // 判断是不是干接点类型的设备继电器
                {
                    #region
                    MS04GenerationOne2R OIP = null;
                    if (CsConst.myDrys != null)
                    {
                        foreach (MS04 Tmp in CsConst.myDrys)
                        {
                            if (Tmp.DIndex == dIndex)
                            {
                                OIP = (MS04GenerationOne2R)Tmp;
                                break;
                            }
                        }
                    }
                    if (CsConst.myDrys == null || OIP == null)
                    {
                        OIP = new MS04GenerationOne2R();
                        OIP.DeviceName = strName;
                        OIP.DIndex = dIndex;
                        OIP.ReadDefaultInfo(wdDeviceType);
                        if (CsConst.myDrys == null) CsConst.myDrys = new List<MS04>(); CsConst.myDrys.Add(OIP);
                    }

                    foreach (Form frm in Application.OpenForms)
                    {
                        if (frm.Name == "frmMS04" && frm.Text == strName)
                        {
                            frmTmp = frm;
                            break;
                        }
                    }
                    if (frmTmp == null) frmTmp = new frmMS04(OIP, strName, wdDeviceType, dIndex);
                    #endregion
                }
                else if (MS04DeviceTypeList.WirelessMS04WithDimmerChn.Contains(wdDeviceType)) // 判断是不是干接点类型的设备调光器
                {
                    #region
                    MS04GenerationOneD OIP = null;
                    if (CsConst.myDrys != null)
                    {
                        foreach (MS04 Tmp in CsConst.myDrys)
                        {
                            if (Tmp.DIndex == dIndex)
                            {
                                OIP = (MS04GenerationOneD)Tmp;
                                break;
                            }
                        }
                    }
                    if (CsConst.myDrys == null || OIP == null)
                    {
                        OIP = new MS04GenerationOneD();
                        OIP.DeviceName = strName;
                        OIP.DIndex = dIndex;
                        OIP.ReadDefaultInfo(wdDeviceType);
                        if (CsConst.myDrys == null) CsConst.myDrys = new List<MS04>(); CsConst.myDrys.Add(OIP);
                    }

                    foreach (Form frm in Application.OpenForms)
                    {
                        if (frm.Name == "frmMS04" && frm.Text == strName)
                        {
                            frmTmp = frm;
                            break;
                        }
                    }
                    if (frmTmp == null) frmTmp = new frmMS04(OIP, strName, wdDeviceType, dIndex);
                    #endregion
                }
                else if (MS04DeviceTypeList.MS04IModuleWithTemperature.Contains(wdDeviceType)) // 判断是不是干接点类型的设备带温度传感器
                {
                    #region
                    MS04GenerationOneT OIP = null;
                    if (CsConst.myDrys != null)
                    {
                        foreach (MS04 Tmp in CsConst.myDrys)
                        {
                            if (Tmp.DIndex == dIndex)
                            {
                                OIP = (MS04GenerationOneT)Tmp;
                                break;
                            }
                        }
                    }
                    if (CsConst.myDrys == null || OIP == null)
                    {
                        OIP = new MS04GenerationOneT();
                        OIP.DeviceName = strName;
                        OIP.DIndex = dIndex;
                        OIP.ReadDefaultInfo(wdDeviceType);
                        if (CsConst.myDrys == null) CsConst.myDrys = new List<MS04>(); CsConst.myDrys.Add(OIP);
                    }

                    foreach (Form frm in Application.OpenForms)
                    {
                        if (frm.Name == "frmMS04" && frm.Text == strName)
                        {
                            frmTmp = frm;
                            break;
                        }
                    }
                    if (frmTmp == null) frmTmp = new frmMS04(OIP, strName, wdDeviceType, dIndex);
                    #endregion
                }
                else if (MS04DeviceTypeList.HDLMS04DeviceTypeList.Contains(wdDeviceType)) //判断是不是干接点类型的设备
                {
                    #region
                    MS04 OIP = null;
                    if (CsConst.myDrys != null)
                    {
                        foreach (MS04 Tmp in CsConst.myDrys)
                        {
                            if (Tmp.DIndex == dIndex)
                            {
                                OIP = Tmp;
                                break;
                            }
                        }
                    }
                    if (CsConst.myDrys == null || OIP == null)
                    {
                        OIP = new MS04();
                        OIP.DeviceName = strName;
                        OIP.DIndex = dIndex;
                        OIP.ReadDefaultInfo(wdDeviceType);
                        if (CsConst.myDrys == null) CsConst.myDrys = new List<MS04>(); CsConst.myDrys.Add(OIP);
                    }

                    foreach (Form frm in Application.OpenForms)
                    {
                        if (frm.Name == "frmPanel" && frm.Text == strName)
                        {
                            frmTmp = frm;
                            break;
                        }
                    }
                    if (frmTmp == null) frmTmp = new frmMS04(OIP, strName, wdDeviceType, dIndex);
                    #endregion
                }
                else if (AudioDeviceTypeList.AudioBoxDeviceTypeList.Contains(wdDeviceType)) // 判断是不是背景音乐设备的类型
                {
                    #region
                    MzBox OIP = null;
                    if (CsConst.myzBoxs != null)
                    {
                        foreach (MzBox Tmp in CsConst.myzBoxs)
                        {
                            if (Tmp.DIndex == dIndex)
                            {
                                OIP = Tmp;
                                break;
                            }
                        }
                    }
                    if (CsConst.myzBoxs == null || OIP == null)
                    {
                        OIP = new MzBox();
                        OIP.Devname = strName;
                        OIP.DIndex = dIndex;
                        OIP.ReadDefaultInfo();
                        if (CsConst.myzBoxs == null) CsConst.myzBoxs = new List<MzBox>(); CsConst.myzBoxs.Add(OIP);
                    }

                    foreach (Form frm in Application.OpenForms)
                    {
                        if (frm.Name == "FrmMzBox" && frm.Text == strName)
                        {
                            frmTmp = frm;
                            break;
                        }
                    }
                    if (frmTmp == null) frmTmp = new FrmMzBox(OIP, strName, wdDeviceType, dIndex);
                    #endregion
                }
                else if (CsConst.mintSPADeviceType.Contains(wdDeviceType)) // 判断是不是SPA专用面板的类型
                {

                }
                else if (IPmoduleDeviceTypeList.HDLIPModuleDeviceTypeLists.Contains(wdDeviceType))   //是不是IP module 模块
                {
                    #region
                    IPModule OIP = null;
                    if (CsConst.myIPs != null)
                    {
                        foreach (IPModule Tmp in CsConst.myIPs)
                        {
                            if (Tmp.DIndex == dIndex)
                            {
                                OIP = Tmp;
                                break;
                            }
                        }
                    }
                    if (CsConst.myIPs == null || OIP == null)
                    {
                        OIP = new IPModule();
                        OIP.DeviceName = strName;
                        OIP.DIndex = dIndex;
                        OIP.ReadDefaultInfo();
                        if (CsConst.myIPs == null) CsConst.myIPs = new List<IPModule>(); CsConst.myIPs.Add(OIP);
                    }

                    foreach (Form frm in Application.OpenForms)
                    {
                        if (frm.Name == "frmIPMod" && frm.Text == strName)
                        {
                            frmTmp = frm;
                            break;
                        }
                    }
                    if (frmTmp == null) frmTmp = new frmIPMod(OIP, strName, dIndex, wdDeviceType);
                    #endregion
                }
                else if (Eightin1DeviceTypeList.HDL8in1DeviceType.Contains(wdDeviceType))   //是不是八合一 模块
                {
                    #region
                    MultiSensor OIP = null;
                    if (CsConst.mysensor_8in1 != null)
                    {
                        foreach (MultiSensor Tmp in CsConst.mysensor_8in1)
                        {
                            if (Tmp.DIndex == dIndex)
                            {
                                OIP = Tmp;
                                break;
                            }
                        }
                    }
                    if (CsConst.mysensor_8in1 == null || OIP == null)
                    {
                        OIP = new MultiSensor();
                        OIP.Devname = strName;
                        OIP.DIndex = dIndex;
                        OIP.ReadDefaultInfo();
                        if (CsConst.mysensor_8in1 == null) CsConst.mysensor_8in1 = new List<MultiSensor>(); CsConst.mysensor_8in1.Add(OIP);
                    }

                    foreach (Form frm in Application.OpenForms)
                    {
                        if (frm.Name == "frm8in1" && frm.Text == strName)
                        {
                            frmTmp = frm;
                            break;
                        }
                    }
                    if (frmTmp == null) frmTmp = new frm8in1(OIP, strName, dIndex, wdDeviceType);
                    #endregion
                }
                else if (Twelvein1DeviceTypeList.HDL12in1DeviceType.Contains(wdDeviceType))   //是不是12合一 模块
                {
                    #region
                    Sensor_12in1 OIP = null;
                    if (CsConst.mysensor_12in1 != null)
                    {
                        foreach (Sensor_12in1 Tmp in CsConst.mysensor_12in1)
                        {
                            if (Tmp.DIndex == dIndex)
                            {
                                OIP = Tmp;
                                break;
                            }
                        }
                    }
                    if (CsConst.mysensor_12in1 == null || OIP == null)
                    {
                        OIP = new Sensor_12in1();
                        OIP.Devname = strName;
                        OIP.DIndex = dIndex;
                        OIP.ReadDefaultInfo();
                        if (CsConst.mysensor_12in1 == null) CsConst.mysensor_12in1 = new List<Sensor_12in1>(); CsConst.mysensor_12in1.Add(OIP);
                    }

                    foreach (Form frm in Application.OpenForms)
                    {
                        if (frm.Name == "frm12in1" && frm.Text == strName)
                        {
                            frmTmp = frm;
                            break;
                        }
                    }
                    if (frmTmp == null) frmTmp = new frm12in1(OIP, strName, dIndex, wdDeviceType);
                    #endregion
                }
                else if (CsConst.mintEIBDeviceType.Contains(wdDeviceType))  //是不是EIB转换器模块
                {

                }
                else if (SMSModuleDeviceTypeList.HDLGPRSModuleDeviceTypeLists.Contains(wdDeviceType))  //是不是GPRS模块
                {
                    #region
                    GPRS ogprs = new GPRS();
                    if (CsConst.myGPRS != null)
                    {
                        foreach (GPRS Tmp in CsConst.myGPRS)
                        {
                            if (Tmp.DIndex == dIndex)
                            {
                                ogprs = Tmp;
                                break;
                            }
                        }
                    }
                    else
                    {
                        ogprs.DIndex = dIndex;
                        ogprs.Devname = strName;
                        CsConst.myGPRS = new List<GPRS>(); CsConst.myGPRS.Add(ogprs);
                    }
                    if (ogprs == null) ogprs.ReadDefaultInfo();

                    foreach (Form frm in Application.OpenForms)
                    {
                        if (frm.Name == "frmGPRS" && frm.Text == strName)
                        {
                            frmTmp = frm;
                            break;
                        }
                    }
                    if (frmTmp == null) frmTmp = new frmGPRS(ogprs, strName, dIndex, wdDeviceType);
                    #endregion
                }
                else if (HAIModuleDeviceTypeList.HDLHAIModuleDeviceTypeLists.Contains(wdDeviceType))  //是不是HAI模块
                {
                    #region
                    HAI OIP = null;
                    if (CsConst.myHais != null)
                    {
                        foreach (HAI Tmp in CsConst.myHais)
                        {
                            if (Tmp.DIndex == dIndex)
                            {
                                OIP = Tmp;
                                break;
                            }
                        }
                    }
                    if (CsConst.myHais == null || OIP == null)
                    {
                        OIP = new HAI();
                        OIP.DeviceName = strName;
                        OIP.DIndex = dIndex;
                        OIP.ReadDefaultInfo();
                        if (CsConst.myHais == null) CsConst.myHais = new List<HAI>(); CsConst.myHais.Add(OIP);
                    }

                    foreach (Form frm in Application.OpenForms)
                    {
                        if (frm.Name == "frmIPMod" && frm.Text == strName)
                        {
                            frmTmp = frm;
                            break;
                        }
                    }
                    if (frmTmp == null) frmTmp = new frmHAI(OIP, strName, dIndex, wdDeviceType);
                    #endregion
                }
                else if (ConstMMC.MediaPBoxDeviceType.Contains(wdDeviceType))///是不是多媒体播放设备
                {
                    #region
                    MMC OIP = null;
                    if (CsConst.myMediaBoxes != null)
                    {
                        foreach (MMC Tmp in CsConst.myMediaBoxes)
                        {
                            if (Tmp.DIndex == dIndex)
                            {
                                OIP = Tmp;
                                break;
                            }
                        }
                    }
                    if (CsConst.myMediaBoxes == null || OIP == null)
                    {
                        OIP = new MMC();
                        OIP.strName = strName;
                        OIP.DIndex = dIndex;
                        OIP.ReadDefaultInfo();
                        if (CsConst.myMediaBoxes == null) CsConst.myMediaBoxes = new List<MMC>(); CsConst.myMediaBoxes.Add(OIP);
                    }

                    foreach (Form frm in Application.OpenForms)
                    {
                        if (frm.Name == "frmMC" && frm.Text == strName)
                        {
                            frmTmp = frm;
                            break;
                        }
                    }
                    if (frmTmp == null) frmTmp = new frmMC(OIP, strName, dIndex, wdDeviceType);
                    #endregion
                }
                else if (SecurityDeviceTypeList.HDLSecurityDeviceTypeList.Contains(wdDeviceType))  /// 是不是安防模块设备
                {
                    #region
                    Security OIP = null;
                    if (CsConst.mySecurities != null)
                    {
                        foreach (Security Tmp in CsConst.mySecurities)
                        {
                            if (Tmp.DIndex == dIndex)
                            {
                                OIP = Tmp;
                                break;
                            }
                        }
                    }
                    if (CsConst.mySecurities == null || OIP == null)
                    {
                        OIP = new Security();
                        OIP.DeviceName = strName;
                        OIP.DIndex = dIndex;
                        OIP.ReadDefaultInfo(wdDeviceType);
                        if (CsConst.mySecurities == null) CsConst.mySecurities = new List<Security>(); CsConst.mySecurities.Add(OIP);
                    }

                    foreach (Form frm in Application.OpenForms)
                    {
                        if (frm.Name == "frmIPMod" && frm.Text == strName)
                        {
                            frmTmp = frm;
                            break;
                        }
                    }
                    if (frmTmp == null) frmTmp = new frmSecurity(OIP, strName, dIndex, wdDeviceType);
                    #endregion
                }
                else if (LogicDeviceTypeList.LogicDeviceType.Contains(wdDeviceType))  //是不是逻辑模块
                {
                    #region
                    Logic OIP = null;
                    if (CsConst.myLogics != null)
                    {
                        foreach (Logic Tmp in CsConst.myLogics)
                        {
                            if (Tmp.DIndex == dIndex)
                            {
                                OIP = Tmp;
                                break;
                            }
                        }
                    }
                    if (CsConst.myLogics == null || OIP == null)
                    {
                        OIP = new Logic();
                        OIP.DevName = strName;
                        OIP.DIndex = dIndex;
                        OIP.ReadDefaultInfo(wdDeviceType);
                        if (CsConst.myLogics == null) CsConst.myLogics = new List<Logic>(); CsConst.myLogics.Add(OIP);
                    }

                    foreach (Form frm in Application.OpenForms)
                    {
                        if (frm.Name == "frmLogic" && frm.Text == strName)
                        {
                            frmTmp = frm;
                            break;
                        }
                    }
                    if (frmTmp == null) frmTmp = new frmLogic(OIP, strName, dIndex, wdDeviceType);
                    #endregion
                }
                else if (BacNetDeviceTypeList.HDLBacNetDeviceTypeList.Contains(wdDeviceType))  // 是不是bacnet模块
                {
                    #region
                    BacNet OIP = null;
                    if (CsConst.myBacnets != null)
                    {
                        foreach (BacNet Tmp in CsConst.myBacnets)
                        {
                            if (Tmp.DIndex == dIndex)
                            {
                                OIP = Tmp;
                                break;
                            }
                        }
                    }
                    if (CsConst.myBacnets == null || OIP == null)
                    {
                        OIP = new BacNet();
                        OIP.DeviceName = strName;
                        OIP.DIndex = dIndex;
                        OIP.ReadDefaultInfo();
                        if (CsConst.myBacnets == null) CsConst.myBacnets = new List<BacNet>(); CsConst.myBacnets.Add(OIP);
                    }

                    foreach (Form frm in Application.OpenForms)
                    {
                        if (frm.Name == "frmIPMod" && frm.Text == strName)
                        {
                            frmTmp = frm;
                            break;
                        }
                    }
                    if (frmTmp == null) frmTmp = new frmBacNet(OIP, strName, dIndex, wdDeviceType);
                    #endregion
                }
                else if (FloorheatingDeviceTypeList.HDLFloorHeatingDeviceType.Contains(wdDeviceType))  // 是不是地热模块
                {
                    #region
                    FH OIP = null;
                    if (CsConst.myFHs != null)
                    {
                        foreach (FH Tmp in CsConst.myFHs)
                        {
                            if (Tmp.DIndex == dIndex)
                            {
                                OIP = Tmp;
                                break;
                            }
                        }
                    }
                    if (CsConst.myFHs == null || OIP == null)
                    {
                        OIP = new FH();
                        OIP.DeviceName = strName;
                        OIP.DIndex = dIndex;
                        OIP.ReadDefaultInfo();
                        if (CsConst.myFHs == null) CsConst.myFHs = new List<FH>(); CsConst.myFHs.Add(OIP);
                    }

                    foreach (Form frm in Application.OpenForms)
                    {
                        if (frm.Name == "frmIPMod" && frm.Text == strName)
                        {
                            frmTmp = frm;
                            break;
                        }
                    }
                    if (frmTmp == null) frmTmp = new frmFH(OIP, strName, dIndex, wdDeviceType);
                    #endregion
                }
                else if (DSDeviceTypeList.DoorStationDeviceType.Contains(wdDeviceType))//是不是10寸屏室外机
                {
                    #region
                    DS oDS = null;
                    if (CsConst.myDS != null)
                    {
                        foreach (DS Tmp in CsConst.myDS)
                        {
                            if (Tmp.DIndex == dIndex)
                            {
                                oDS = Tmp;
                                break;
                            }
                        }
                    }
                    if (CsConst.myDS == null || oDS == null)
                    {
                        oDS = new DS();
                        oDS.Devname = strName;
                        oDS.DIndex = dIndex;
                        if (CsConst.myDS == null) CsConst.myDS = new List<DS>(); CsConst.myDS.Add(oDS);
                    }

                    if (oDS == null) oDS.ReadDefaultInfoForDS();
                    foreach (Form frm in Application.OpenForms)
                    {
                        if (frm.Name == "frmDS" && frm.Text == strName)
                        {
                            frmTmp = frm;
                            break;
                        }
                    }
                    if (frmTmp == null) frmTmp = new frmDS(oDS, strName, dIndex, wdDeviceType);
                    #endregion
                }
                else if (DSDeviceTypeList.NewDoorStationDeviceType.Contains(wdDeviceType))//新门口机
                {
                    #region
                    NewDS oNewDS = null;
                    if (CsConst.myNewDS != null)
                    {
                        foreach (NewDS oTmp in CsConst.myNewDS)
                        {
                            if (oTmp.DIndex == dIndex)
                            {
                                oNewDS = oTmp;
                                break;
                            }
                        }
                    }
                    if (CsConst.myNewDS == null || oNewDS == null)
                    {
                        oNewDS = new NewDS();
                        oNewDS.DIndex = dIndex;
                        oNewDS.Devname = strName;

                       if (CsConst.myNewDS == null) CsConst.myNewDS = new List<NewDS>(); CsConst.myNewDS.Add(oNewDS);
                    }

                    foreach (Form frm in Application.OpenForms)
                    {
                        if (frm.Name == "FrmNewDS" && frm.Text == strName)
                        {
                            frmTmp = frm;
                            break;
                        }
                    }
                    if (frmTmp == null) frmTmp = new FrmNewDS(oNewDS, strName, dIndex, wdDeviceType);
                    #endregion
                }
                else if (HotelMixModuleDeviceType.HDLRCUDeviceTypeLists.Contains(wdDeviceType))//是不是客房控制器
                {
                    #region
                    MHRCU OIP = null;
                    if (CsConst.myRcuMixModules != null)
                    {
                        foreach (MHRCU Tmp in CsConst.myRcuMixModules)
                        {
                            if (Tmp.DIndex == dIndex)
                            {
                                OIP = Tmp;
                                break;
                            }
                        }
                    }
                    if (CsConst.myRcuMixModules == null || OIP == null)
                    {
                        OIP = new MHRCU();
                        OIP.Devname = strName;
                        OIP.DIndex = dIndex;
                        OIP.ReadDefaultInfo();
                        if (CsConst.myRcuMixModules == null) CsConst.myRcuMixModules = new List<MHRCU>(); CsConst.myRcuMixModules.Add(OIP);
                    }

                    foreach (Form frm in Application.OpenForms)
                    {
                        if (frm.Name == "FrmMHRCU" && frm.Text == strName)
                        {
                            frmTmp = frm;
                            break;
                        }
                    }
                    if (frmTmp == null) frmTmp = new FrmMHRCU(OIP, strName, dIndex, wdDeviceType);
                    #endregion
                }
                else if (MSPUSenserDeviceTypeList.SensorsDeviceTypeList.Contains(wdDeviceType))//红外超声波传感器
                {
                    #region
                    MSPU OIP = null;
                    if (CsConst.myPUSensors != null)
                    {
                        foreach (MSPU Tmp in CsConst.myPUSensors)
                        {
                            if (Tmp.DIndex == dIndex)
                            {
                                OIP = Tmp;
                                break;
                            }
                        }
                    }
                    if (CsConst.myPUSensors == null || OIP == null)
                    {
                        OIP = new MSPU();
                        OIP.Devname = strName;
                        OIP.DIndex = dIndex;
                        OIP.ReadDefaultInfo();
                        if (CsConst.myPUSensors == null) CsConst.myPUSensors = new List<MSPU>(); CsConst.myPUSensors.Add(OIP);
                    }

                    foreach (Form frm in Application.OpenForms)
                    {
                        if (frm.Name == "FrmMSPU" && frm.Text == strName)
                        {
                            frmTmp = frm;
                            break;
                        }
                    }
                    if (frmTmp == null) frmTmp = new FrmMSPU(OIP, strName, dIndex, wdDeviceType);
                    #endregion
                }
                else if (EnviroDLPPanelDeviceTypeList.HDLEnviroDLPPanelDeviceTypeList.Contains(wdDeviceType))//彩色DLP
                {

                }
                else if (CoolMasterDeviceTypeList.HDLCoolMasterDeviceTypeList.Contains(wdDeviceType))//coolmaster
                {
                    #region
                    CoolMaster OIP = null;
                    if (CsConst.myCoolMasters != null)
                    {
                        foreach (CoolMaster Tmp in CsConst.myCoolMasters)
                        {
                            if (Tmp.DIndex == dIndex)
                            {
                                OIP = Tmp;
                                break;
                            }
                        }
                    }
                    if (CsConst.myCoolMasters == null || OIP == null)
                    {
                        OIP = new CoolMaster();
                        OIP.DeviceName = strName;
                        OIP.DIndex = dIndex;
                        OIP.ReadDefaultInfo();
                        if (CsConst.myCoolMasters == null) CsConst.myCoolMasters = new List<CoolMaster>(); CsConst.myCoolMasters.Add(OIP);
                    }

                    foreach (Form frm in Application.OpenForms)
                    {
                        if (frm.Name == "frmIPMod" && frm.Text == strName)
                        {
                            frmTmp = frm;
                            break;
                        }
                    }
                    if (frmTmp == null) frmTmp = new FrmCoolMaster(OIP, strName, dIndex, wdDeviceType);
                    #endregion
                }
                else if (MiniSenserDeviceTypeList.SensorsDeviceTypeList.Contains(wdDeviceType))//MINI超声波
                {
                    #region
                    MiniSensor oMINIUL = null;
                    if (CsConst.myMiniSensors != null)
                    {
                        foreach (MiniSensor oTmp in CsConst.myMiniSensors)
                        {
                            if (oTmp.DIndex == dIndex)
                            {
                                oMINIUL = oTmp;
                                break;
                            }
                        }
                    }
                    if (CsConst.myMiniSensors == null || oMINIUL == null)
                    {
                        oMINIUL = new MiniSensor();
                        oMINIUL.DIndex = dIndex;
                        oMINIUL.Devname = strName;
                        if (CsConst.myMiniSensors == null) CsConst.myMiniSensors = new List<MiniSensor>(); CsConst.myMiniSensors.Add(oMINIUL);
                    }
                    foreach (Form frm in Application.OpenForms)
                    {
                        if (frm.Name == "FrmMiniSensor" && frm.Text == strName)
                        {
                            frmTmp = frm;
                            break;
                        }
                    }
                    if (frmTmp == null) frmTmp = new FrmMiniSensor(oMINIUL, strName, dIndex, wdDeviceType);
                    #endregion
                }
                else if (CsConst.minDoorBellDeviceType.Contains(wdDeviceType))//门铃
                {

                }
                else if (MHICDeviceTypeList.HDLCardReaderDeviceType.Contains(wdDeviceType))//插卡取电
                {
                    #region
                    MHIC oMINIUL = null;
                    if (CsConst.myCardReader != null)
                    {
                        foreach (MHIC oTmp in CsConst.myCardReader)
                        {
                            if (oTmp.DIndex == dIndex)
                            {
                                oMINIUL = oTmp;
                                break;
                            }
                        }
                    }
                    if (CsConst.myCardReader == null || oMINIUL == null)
                    {
                        oMINIUL = new MHIC();
                        oMINIUL.DIndex = dIndex;
                        oMINIUL.DeviceName = strName;
                       if (CsConst.myCardReader == null) CsConst.myCardReader = new List<MHIC>(); CsConst.myCardReader.Add(oMINIUL);
                    }
                    foreach (Form frm in Application.OpenForms)
                    {
                        if (frm.Name == "FrmMHIC" && frm.Text == strName)
                        {
                            frmTmp = frm;
                            break;
                        }
                    }
                    if (frmTmp == null) frmTmp = new FrmMHIC(oMINIUL, strName, dIndex, wdDeviceType);
                    #endregion
                }
                else if (CsConst.mintHotelACDeviceType.Contains(wdDeviceType))//酒店空调面板
                {

                }
                else if (CsConst.minMSPUBRFDeviceType.Contains(wdDeviceType))//电池装传感器
                {

                }
                else if (DSDeviceTypeList.NewDoorStationDeviceType.Contains(wdDeviceType))//新门口机
                {

                }
                else if (SocketDeviceTypeList.HDLSocketDeviceTypeList.Contains(wdDeviceType))//无线插座
                {
                    #region
                    RFSocket oCamera = null;

                    if (CsConst.mySockets != null)
                    {
                        foreach (RFSocket oTmp in CsConst.mySockets)
                        {
                            if (oTmp.DIndex == dIndex)
                            {
                                oCamera = oTmp;
                                break;
                            }
                        }
                    }
                    if (CsConst.mySockets == null || oCamera == null)
                    {
                        oCamera = new RFSocket();
                        oCamera.DIndex = dIndex;
                        oCamera.DeviceName = strName;

                        if (CsConst.mySockets == null) CsConst.mySockets = new List<RFSocket>(); CsConst.mySockets.Add(oCamera);
                    }

                    foreach (Form frm in Application.OpenForms)
                    {
                        if (frm.Name == "frmSocket" && frm.Text == strName)
                        {
                            frmTmp = frm;
                            break;
                        }
                    }
                    if (frmTmp == null) frmTmp = new frmSocket(oCamera, strName, wdDeviceType, dIndex);
                    #endregion
                }
                else if (CameraNvrDeviceType.cameraThirdPartDeviceType.Contains(wdDeviceType)) // 第三方摄像头
                {
                    #region
                    Camera oCamera = null;
                    if (CameraNvrDeviceType.nvrThridPartDeviceType.Contains(wdDeviceType))
                        oCamera = new Nvr();
                    if (CsConst.myCameras != null)
                    {
                        foreach (Camera oTmp in CsConst.myCameras)
                        {
                            if (oTmp.dIndex == dIndex)
                            {
                                oCamera = oTmp;
                                break;
                            }
                        }
                    }
                    if (CsConst.myCameras == null || oCamera == null)
                    {
                        oCamera = new Camera();
                        oCamera.dIndex = dIndex;
                        oCamera.Devname = strName;

                        if (CsConst.myCameras == null) CsConst.myCameras = new List<Camera>(); CsConst.myCameras.Add(oCamera);
                    }

                    foreach (Form frm in Application.OpenForms)
                    {
                        if (frm.Name == "frmCameraNvr" && frm.Text == strName)
                        {
                            frmTmp = frm;
                            break;
                        }
                    }
                    if (frmTmp == null) frmTmp = new frmCameraNvr(oCamera, strName, dIndex, wdDeviceType);
                    #endregion
                }
                else if (CsConst.minFANControllerDeviceType.Contains(wdDeviceType))//风扇控制器
                {

                }
                else if (CsConst.minBrazilWirelessPanelDeviceType.Contains(wdDeviceType))//巴西无线面板
                {

                }
                else if (CsConst.minSensor_7in1DeviceType.Contains(wdDeviceType))//7合一
                {
                    #region
                    Sensor_7in1 OIP = null;
                    if (CsConst.mysensor_7in1 != null)
                    {
                        foreach (Sensor_7in1 Tmp in CsConst.mysensor_7in1)
                        {
                            if (Tmp.DIndex == dIndex)
                            {
                                OIP = Tmp;
                                break;
                            }
                        }
                    }
                    if (CsConst.mysensor_7in1 == null || OIP == null)
                    {
                        OIP = new Sensor_7in1();
                        OIP.Devname = strName;
                        OIP.DIndex = dIndex;
                        OIP.ReadDefaultInfo();
                        if (CsConst.mysensor_7in1 == null) CsConst.mysensor_7in1 = new List<Sensor_7in1>(); CsConst.mysensor_7in1.Add(OIP);
                    }

                    foreach (Form frm in Application.OpenForms)
                    {
                        if (frm.Name == "frm7in1" && frm.Text == strName)
                        {
                            frmTmp = frm;
                            break;
                        }
                    }
                    if (frmTmp == null) frmTmp = new frm7in1(OIP, strName, dIndex, wdDeviceType);
                    #endregion
                }
                else if (CsConst.SonosConvertorDeviceType.Contains(wdDeviceType)) // sonos 转换器
                {
                    #region
                    foreach (Form frm in Application.OpenForms)
                    {
                        if (frm.Name == "frmNetModule" && frm.Text == strName)
                        {
                            frmTmp = frm;
                            break;
                        }
                    }
                    if (frmTmp == null) frmTmp = new frmNetModule(strName, wdDeviceType);
                    #endregion
                }
                else if (T4SensorDeviceTypeList.HDLTsensorDeviceType.Contains(wdDeviceType)) // 四通道温度传感器
                {
                    #region
                    TempSensor OIP = null;
                    if (CsConst.myTemperatureSensors != null)
                    {
                        foreach (TempSensor Tmp in CsConst.myTemperatureSensors)
                        {
                            if (Tmp.DIndex == dIndex)
                            {
                                OIP = Tmp;
                                break;
                            }
                        }
                    }
                    if (CsConst.myTemperatureSensors == null || OIP == null)
                    {
                        OIP = new TempSensor();
                        OIP.Devname = strName;
                        OIP.DIndex = dIndex;
                        OIP.ReadDefaultInfo(4);
                        if (CsConst.myTemperatureSensors == null) CsConst.myTemperatureSensors = new List<TempSensor>(); CsConst.myTemperatureSensors.Add(OIP);
                    }

                    foreach (Form frm in Application.OpenForms)
                    {
                        if (frm.Name == "frm7in1" && frm.Text == strName)
                        {
                            frmTmp = frm;
                            break;
                        }
                    }
                    if (frmTmp == null) frmTmp = new frm4Temp(OIP, strName, dIndex, wdDeviceType);
                    #endregion
                }
                else if (Rs232DeviceTypeList.HDLRs232DeviceType.Contains(wdDeviceType)) // 是不是Rs232 模块
                {
                    #region
                    RS232 Tmp232 = null;
                    if (CsConst.myRS232 != null)
                    {
                        foreach (RS232 Tmp in CsConst.myRS232)
                        {
                            if (Tmp.DIndex == dIndex)
                            {
                                Tmp232 = Tmp;
                                break;
                            }
                        }
                    }
                    if (CsConst.myRS232 == null || Tmp232 == null)
                    {
                        Tmp232 = new RS232();
                        Tmp232.DIndex = dIndex;
                        Tmp232.strName = strName;
                        if (CsConst.myRS232 ==null) CsConst.myRS232 = new List<RS232>(); CsConst.myRS232.Add(Tmp232);
                    }
                    if (Tmp232 == null) Tmp232.ReadDefaultInfo();

                    foreach (Form frm in Application.OpenForms)
                    {
                        if (frm.Name == "frmRS232" && frm.Text == strName)
                        {
                            frmTmp = frm;
                            break;
                        }
                    }
                    if (frmTmp == null) frmTmp = new frmRS232(Tmp232, strName, dIndex, wdDeviceType);
                    #endregion
                }
                #endregion
            }
            catch 
            {
                return frmTmp;
            }
            return frmTmp;
        }


        /// <summary>
        /// 添加设备 默认设置 增加结构体
        /// </summary>
        /// <param name="intDeviceType"></param>
        /// <param name="intDIndex"></param>
        /// <param name="strDevName"></param>
        public static void AddItsDefaultSettings(int wdDeviceType, int dIndex, string strDevName)
        {
            #region
            if (DimmerDeviceTypeList.HDLDimmerDeviceTypeList.Contains(wdDeviceType)) // 判断是不是调光设备的类型
            {
                #region
                Dimmer tempDi = null;
                if (CsConst.myDimmers != null)
                {
                    foreach (Dimmer di in CsConst.myDimmers)
                    {
                        if (di.DIndex == dIndex)
                        {
                            tempDi = di;
                            break;
                        }
                    }
                }
                if (CsConst.myDimmers == null || tempDi == null)
                {
                    tempDi = new Dimmer();
                    tempDi.DeviceName = strDevName;
                    tempDi.DIndex = dIndex;
                    if (CsConst.myDimmers == null) CsConst.myDimmers = new List<Dimmer>();
                    CsConst.myDimmers.Add(tempDi);
                }

                if (tempDi == null)
                {
                    tempDi.ReadDefaultInfo(wdDeviceType);
                }
                #endregion
            }
            else if (RelayDeviceTypeList.HDLRelayDeviceTypeList.Contains(wdDeviceType)) //判断是不是继电器类
            {
                #region
                Relay OIP = null;
                if (CsConst.myRelays != null)
                {
                    foreach (Relay Tmp in CsConst.myRelays)
                    {
                        if (Tmp.DIndex == dIndex)
                        {
                            OIP = Tmp;
                            break;
                        }
                    }
                }
                if (CsConst.myRelays == null || OIP == null)
                {
                    OIP = new Relay();
                    OIP.DeviceName = strDevName;
                    OIP.DIndex = dIndex;
                    OIP.ReadDefaultInfo(wdDeviceType);
                    if (CsConst.myRelays == null) CsConst.myRelays = new List<Relay>(); CsConst.myRelays.Add(OIP);
                }
                #endregion
            }
            else if (IrModuleDeviceTypeList.HDNewIrModuleDeviceTypeLists.Contains(wdDeviceType))//是不是新型红外发射模块
            {

            }
            else if (CsConst.mintAllIRDeviceType.Contains(wdDeviceType)) // 判断是不是所有的红外发射类型
            {

            }
            else if (DMXDeviceTypeList.HDLDMXDeviceTypeList.Contains(wdDeviceType)) // 判断是不是所有的DMX
            {
                #region
                DMX tempDi = null;
                if (CsConst.myDmxs != null)
                {
                    foreach (DMX di in CsConst.myDmxs)
                    {
                        if (di.DIndex == dIndex)
                        {
                            tempDi = di;
                            break;
                        }
                    }
                }
                if (CsConst.myDmxs == null || tempDi == null)
                {
                    tempDi = new DMX();
                    tempDi.DeviceName = strDevName;
                    tempDi.DIndex = dIndex;
                    if (CsConst.myDmxs == null) CsConst.myDmxs = new List<DMX>();
                    CsConst.myDmxs.Add(tempDi);
                }

                #endregion
            }
            else if (CurtainDeviceType.HDLCurtainModuleDeviceType.Contains(wdDeviceType)) // 判断是不是所有的窗帘模块
            {
                #region
                Curtain OIP = null;
                if (CsConst.myCurtains != null)
                {
                    foreach (Curtain Tmp in CsConst.myCurtains)
                    {
                        if (Tmp.DIndex == dIndex)
                        {
                            OIP = Tmp;
                            break;
                        }
                    }
                }
                if (CsConst.myCurtains == null || OIP == null)
                {
                    OIP = new Curtain();
                    OIP.DeviceName = strDevName;
                    OIP.DIndex = dIndex;
                    OIP.ReadDefaultInfo();
                    if (CsConst.myCurtains == null) CsConst.myCurtains = new List<Curtain>();
                    CsConst.myCurtains.Add(OIP);
                }
                #endregion
            }
            else if (CsConst.mintMotionDeviceType.Contains(wdDeviceType))  // 判断是不是所有的动静传感器类型
            {

            }
            else if (HVACModuleDeviceTypeList.HDLHVACModuleDeviceTypeLists.Contains(wdDeviceType))      //判断是不是所有HVAC的设备类型
            {
                #region
                HVAC OIP = null;
                if (CsConst.myHvacs != null)
                {
                    foreach (HVAC Tmp in CsConst.myHvacs)
                    {
                        if (Tmp.DIndex == dIndex)
                        {
                            OIP = Tmp;
                            break;
                        }
                    }
                }
                if (CsConst.myHvacs == null || OIP == null)
                {
                    OIP = new HVAC();
                    OIP.DeviceName = strDevName;
                    OIP.DIndex = dIndex;
                    OIP.ReadDefaultInfo();
                    if (CsConst.myHvacs == null) CsConst.myHvacs = new List<HVAC>(); CsConst.myHvacs.Add(OIP);
                }
                #endregion
            }
            else if (EnviroNewDeviceTypeList.EnviroNewPanelDeviceTypeList.Contains(wdDeviceType)) // 是不是新版彩屏面板
            {
                #region
                EnviroPanel OIP = null;
                if (CsConst.myEnviroPanels != null)
                {
                    foreach (EnviroPanel Tmp in CsConst.myEnviroPanels)
                    {
                        if (Tmp.DIndex == dIndex)
                        {
                            OIP = Tmp;
                            break;
                        }
                    }
                }
                if (CsConst.myColorPanels == null || OIP == null)
                {
                    OIP = new EnviroPanel();
                    OIP.DeviceName = strDevName;
                    OIP.DIndex = dIndex;
                    OIP.ReadDefaultInfo();
                    if (CsConst.myEnviroPanels == null) CsConst.myEnviroPanels = new List<EnviroPanel>();
                    CsConst.myEnviroPanels.Add(OIP);
                }
                #endregion
            }
            else if (EnviroDLPPanelDeviceTypeList.HDLEnviroDLPPanelDeviceTypeList.Contains(wdDeviceType)) // 是不是彩屏面板
            {
                #region
                ColorDLP OIP = null;
                if (CsConst.myColorPanels != null)
                {
                    foreach (ColorDLP Tmp in CsConst.myColorPanels)
                    {
                        if (Tmp.DIndex == dIndex)
                        {
                            OIP = Tmp;
                            break;
                        }
                    }
                }
                if (CsConst.myColorPanels == null || OIP == null)
                {
                    OIP = new ColorDLP();
                    OIP.DeviceName = strDevName;
                    OIP.DIndex = dIndex;
                    OIP.ReadDefaultInfo();
                    if (CsConst.myColorPanels == null) CsConst.myColorPanels = new List<ColorDLP>(); CsConst.myColorPanels.Add(OIP);
                }
                #endregion
            }
            else if (DLPPanelDeviceTypeList.HDLDLPPanelDeviceTypeList.Contains(wdDeviceType))  //判断是不是DLP面板类型的设备
            {
                #region
                DLP OIP = null;
                if (CsConst.myPanels != null)
                {
                    foreach (Panel Tmp in CsConst.myPanels)
                    {
                        if (Tmp.DIndex == dIndex)
                        {
                            OIP = (DLP)Tmp;
                            break;
                        }
                    }
                }
                if (CsConst.myPanels == null || OIP == null)
                {
                    OIP = new DLP();
                    OIP.DeviceName = strDevName;
                    OIP.DIndex = dIndex;
                    OIP.ReadDefaultInfo(wdDeviceType);
                    if (CsConst.myPanels == null) CsConst.myPanels = new List<Panel>(); CsConst.myPanels.Add(OIP);
                }
                #endregion
            }
            else if (NormalPanelDeviceTypeList.HDLNormalPanelDeviceTypeList.Contains(wdDeviceType))  //判断是不是面板类型的设备
            {
                #region
                Panel OIP = null;
                if (CsConst.myPanels != null)
                {
                    foreach (Panel Tmp in CsConst.myPanels)
                    {
                        if (Tmp.DIndex == dIndex)
                        {
                            OIP = Tmp;
                            break;
                        }
                    }
                }
                if (CsConst.myPanels == null || OIP == null)
                {
                    OIP = new Panel();
                    OIP.DeviceName = strDevName;
                    OIP.DIndex = dIndex;
                    OIP.ReadDefaultInfo(wdDeviceType);
                    if (CsConst.myPanels == null) CsConst.myPanels = new List<Panel>(); CsConst.myPanels.Add(OIP);
                }
                #endregion
            }
            else if (MS04DeviceTypeList.MS04HotelMixModuleHasArea.Contains(wdDeviceType)) // 判断是不是干接点类型的混合模块
            {
                #region
                MixHotelModuleWithZone OIP = null;
                if (CsConst.myDrys != null)
                {
                    foreach (MS04 Tmp in CsConst.myDrys)
                    {
                        if (Tmp.DIndex == dIndex)
                        {
                            OIP = (MixHotelModuleWithZone)OIP;
                            break;
                        }
                    }
                }
                if (CsConst.myDrys == null || OIP == null)
                {
                    OIP = new MixHotelModuleWithZone();
                    OIP.DeviceName = strDevName;
                    OIP.DIndex = dIndex;
                    OIP.ReadDefaultInfo(wdDeviceType);
                    if (CsConst.myDrys == null) CsConst.myDrys = new List<MS04>(); CsConst.myDrys.Add(OIP);
                }
                #endregion
            }
            else if (MS04DeviceTypeList.WirelessMS04WithOneCurtain.Contains(wdDeviceType)) // 判断是不是干接点类型的设备窗帘
            {
                #region
                MS04GenerationOneCurtain OIP = null;
                if (CsConst.myDrys != null)
                {
                    foreach (MS04 Tmp in CsConst.myDrys)
                    {
                        if (Tmp.DIndex == dIndex)
                        {
                            OIP = OIP;
                            break;
                        }
                    }
                }
                if (CsConst.myDrys == null || OIP == null)
                {
                    OIP = new MS04GenerationOneCurtain();
                    OIP.DeviceName = strDevName;
                    OIP.DIndex = dIndex;
                    OIP.ReadDefaultInfo(wdDeviceType);
                    if (CsConst.myDrys == null) CsConst.myDrys = new List<MS04>(); CsConst.myDrys.Add(OIP);
                }
                #endregion
            }
            else if (MS04DeviceTypeList.WirelessMS04WithRelayChn.Contains(wdDeviceType)) // 判断是不是干接点类型的设备继电器
            {
                #region
                MS04GenerationOne2R OIP = null;
                if (CsConst.myDrys != null)
                {
                    foreach (MS04 Tmp in CsConst.myDrys)
                    {
                        if (Tmp.DIndex == dIndex)
                        {
                            OIP = OIP;
                            break;
                        }
                    }
                }
                if (CsConst.myDrys == null || OIP == null)
                {
                    OIP = new MS04GenerationOne2R();
                    OIP.DeviceName = strDevName;
                    OIP.DIndex = dIndex;
                    OIP.ReadDefaultInfo(wdDeviceType);
                    if (CsConst.myDrys == null) CsConst.myDrys = new List<MS04>(); CsConst.myDrys.Add(OIP);
                }

                #endregion
            }
            else if (MS04DeviceTypeList.WirelessMS04WithDimmerChn.Contains(wdDeviceType)) // 判断是不是干接点类型的设备调光器
            {
                #region
                MS04GenerationOneD OIP = null;
                if (CsConst.myDrys != null)
                {
                    foreach (MS04 Tmp in CsConst.myDrys)
                    {
                        if (Tmp.DIndex == dIndex)
                        {
                            OIP = (MS04GenerationOneD)Tmp;
                            break;
                        }
                    }
                }
                if (CsConst.myDrys == null || OIP == null)
                {
                    OIP = new MS04GenerationOneD();
                    OIP.DeviceName = strDevName;
                    OIP.DIndex = dIndex;
                    OIP.ReadDefaultInfo(wdDeviceType);
                    if (CsConst.myDrys == null) CsConst.myDrys = new List<MS04>(); CsConst.myDrys.Add(OIP);
                }
                #endregion
            }
            else if (MS04DeviceTypeList.MS04IModuleWithTemperature.Contains(wdDeviceType)) // 判断是不是干接点类型的设备带温度传感器
            {
                #region
                MS04GenerationOneT OIP = null;
                if (CsConst.myDrys != null)
                {
                    foreach (MS04 Tmp in CsConst.myDrys)
                    {
                        if (Tmp.DIndex == dIndex)
                        {
                            OIP = (MS04GenerationOneT)Tmp;
                            break;
                        }
                    }
                }
                if (CsConst.myDrys == null || OIP == null)
                {
                    OIP = new MS04GenerationOneT();
                    OIP.DeviceName = strDevName;
                    OIP.DIndex = dIndex;
                    OIP.ReadDefaultInfo(wdDeviceType);
                    if (CsConst.myDrys == null) CsConst.myDrys = new List<MS04>(); CsConst.myDrys.Add(OIP);
                }
                #endregion
            }
            else if (MS04DeviceTypeList.HDLMS04DeviceTypeList.Contains(wdDeviceType)) //判断是不是干接点类型的设备
            {
                #region
                MS04 OIP = null;
                if (CsConst.myDrys != null)
                {
                    foreach (MS04 Tmp in CsConst.myDrys)
                    {
                        if (Tmp.DIndex == dIndex)
                        {
                            OIP = Tmp;
                            break;
                        }
                    }
                }
                if (CsConst.myDrys == null || OIP == null)
                {
                    OIP = new MS04();
                    OIP.DeviceName = strDevName;
                    OIP.DIndex = dIndex;
                    OIP.ReadDefaultInfo(wdDeviceType);
                    if (CsConst.myDrys == null) CsConst.myDrys = new List<MS04>(); CsConst.myDrys.Add(OIP);
                }
                #endregion
            }
            else if (AudioDeviceTypeList.AudioBoxDeviceTypeList.Contains(wdDeviceType)) // 判断是不是背景音乐设备的类型
            {
                #region
                MzBox OIP = null;
                if (CsConst.myzBoxs != null)
                {
                    foreach (MzBox Tmp in CsConst.myzBoxs)
                    {
                        if (Tmp.DIndex == dIndex)
                        {
                            OIP = Tmp;
                            break;
                        }
                    }
                }
                if (CsConst.myzBoxs == null || OIP == null)
                {
                    OIP = new MzBox();
                    OIP.Devname = strDevName;
                    OIP.DIndex = dIndex;
                    OIP.ReadDefaultInfo();
                    if (CsConst.myzBoxs == null) CsConst.myzBoxs = new List<MzBox>(); CsConst.myzBoxs.Add(OIP);
                }
                #endregion
            }
            else if (CsConst.mintSPADeviceType.Contains(wdDeviceType)) // 判断是不是SPA专用面板的类型
            {

            }
            else if (IPmoduleDeviceTypeList.HDLIPModuleDeviceTypeLists.Contains(wdDeviceType))   //是不是IP module 模块
            {
                #region
                IPModule OIP = new IPModule();
                if (CsConst.myIPs != null)
                {
                    foreach (IPModule Tmp in CsConst.myIPs)
                    {
                        if (Tmp.DIndex == dIndex)
                        {
                            OIP = Tmp;
                            break;
                        }
                    }
                }
                if (CsConst.myIPs == null || OIP == null)
                {
                    OIP = new IPModule();
                    OIP.DeviceName = strDevName;
                    OIP.DIndex = dIndex;
                    OIP.ReadDefaultInfo();
                    if (CsConst.myIPs == null) CsConst.myIPs = new List<IPModule>(); CsConst.myIPs.Add(OIP);
                }
                #endregion
            }
            else if (Eightin1DeviceTypeList.HDL8in1DeviceType.Contains(wdDeviceType))   //是不是八合一 模块
            {
                #region
                MultiSensor OIP = new MultiSensor();
                if (CsConst.mysensor_8in1 != null)
                {
                    foreach (MultiSensor Tmp in CsConst.mysensor_8in1)
                    {
                        if (Tmp.DIndex == dIndex)
                        {
                            OIP = Tmp;
                            break;
                        }
                    }
                }
                if (CsConst.mysensor_8in1 == null || OIP == null)
                {
                    OIP = new MultiSensor();
                    OIP.Devname = strDevName;
                    OIP.DIndex = dIndex;
                    OIP.ReadDefaultInfo();
                    if (CsConst.mysensor_8in1 == null) CsConst.mysensor_8in1 = new List<MultiSensor>(); CsConst.mysensor_8in1.Add(OIP);
                }
                #endregion
            }
            else if (CsConst.minRs232DeviceType.Contains(wdDeviceType)) // 是不是Rs232 模块
            {

            }
            else if (Twelvein1DeviceTypeList.HDL12in1DeviceType.Contains(wdDeviceType))   //是不是12合一 模块
            {

            }
            else if (CsConst.mintEIBDeviceType.Contains(wdDeviceType))  //是不是EIB转换器模块
            {

            }
            else if (SMSModuleDeviceTypeList.HDLGPRSModuleDeviceTypeLists.Contains(wdDeviceType))  //是不是GPRS模块
            {

            }
            else if (HAIModuleDeviceTypeList.HDLHAIModuleDeviceTypeLists.Contains(wdDeviceType))  //是不是HAI模块
            {
                #region
                HAI OIP = null;
                if (CsConst.myHais != null)
                {
                    foreach (HAI Tmp in CsConst.myHais)
                    {
                        if (Tmp.DIndex == dIndex)
                        {
                            OIP = Tmp;
                            break;
                        }
                    }
                }
                if (CsConst.myHais == null || OIP == null)
                {
                    OIP = new HAI();
                    OIP.DeviceName = strDevName;
                    OIP.DIndex = dIndex;
                    OIP.ReadDefaultInfo();
                    if (CsConst.myHais == null) CsConst.myHais = new List<HAI>(); CsConst.myHais.Add(OIP);
                }
                #endregion
            }
            else if (ConstMMC.MediaPBoxDeviceType.Contains(wdDeviceType))///是不是多媒体播放设备
            {
                #region
                MMC OIP = null;
                if (CsConst.myMediaBoxes != null)
                {
                    foreach (MMC Tmp in CsConst.myMediaBoxes)
                    {
                        if (Tmp.DIndex == dIndex)
                        {
                            OIP = Tmp;
                            break;
                        }
                    }
                }
                if (CsConst.myMediaBoxes == null || OIP == null)
                {
                    OIP = new MMC();
                    OIP.strName = strDevName;
                    OIP.DIndex = dIndex;
                    OIP.ReadDefaultInfo();
                    if (CsConst.myMediaBoxes == null) CsConst.myMediaBoxes = new List<MMC>(); CsConst.myMediaBoxes.Add(OIP);
                }
                #endregion
            }
            else if (SecurityDeviceTypeList.HDLSecurityDeviceTypeList.Contains(wdDeviceType))  /// 是不是安防模块设备
            {
                #region
                Security OIP = null;
                if (CsConst.mySecurities != null)
                {
                    foreach (Security Tmp in CsConst.mySecurities)
                    {
                        if (Tmp.DIndex == dIndex)
                        {
                            OIP = Tmp;
                            break;
                        }
                    }
                }
                if (CsConst.mySecurities == null || OIP == null)
                {
                    OIP = new Security();
                    OIP.DeviceName = strDevName;
                    OIP.DIndex = dIndex;
                    OIP.ReadDefaultInfo(wdDeviceType);
                    if (CsConst.mySecurities == null) CsConst.mySecurities = new List<Security>(); CsConst.mySecurities.Add(OIP);
                }
                #endregion
            }
            else if (LogicDeviceTypeList.LogicDeviceType.Contains(wdDeviceType))  //是不是逻辑模块
            {
                #region
                Logic OIP = null;
                if (CsConst.myLogics != null)
                {
                    foreach (Logic Tmp in CsConst.myLogics)
                    {
                        if (Tmp.DIndex == dIndex)
                        {
                            OIP = Tmp;
                            break;
                        }
                    }
                }
                if (CsConst.myLogics == null || OIP == null)
                {
                    OIP = new Logic();
                    OIP.DevName = strDevName;
                    OIP.DIndex = dIndex;
                    OIP.ReadDefaultInfo(wdDeviceType);
                    if (CsConst.myLogics == null) CsConst.myLogics = new List<Logic>(); CsConst.myLogics.Add(OIP);
                }
                #endregion
            }
            else if (BacNetDeviceTypeList.HDLBacNetDeviceTypeList.Contains(wdDeviceType))  // 是不是bacnet模块
            {
                #region
                BacNet OIP = null;
                if (CsConst.myBacnets != null)
                {
                    foreach (BacNet Tmp in CsConst.myBacnets)
                    {
                        if (Tmp.DIndex == dIndex)
                        {
                            OIP = Tmp;
                            break;
                        }
                    }
                }
                if (CsConst.myBacnets == null || OIP == null)
                {
                    OIP = new BacNet();
                    OIP.DeviceName = strDevName;
                    OIP.DIndex = dIndex;
                    OIP.ReadDefaultInfo();
                    if (CsConst.myBacnets == null) CsConst.myBacnets = new List<BacNet>(); CsConst.myBacnets.Add(OIP);
                }
                #endregion
            }
            else if (FloorheatingDeviceTypeList.HDLFloorHeatingDeviceType.Contains(wdDeviceType))  // 是不是地热模块
            {
                #region
                FH OIP = null;
                if (CsConst.myFHs != null)
                {
                    foreach (FH Tmp in CsConst.myFHs)
                    {
                        if (Tmp.DIndex == dIndex)
                        {
                            OIP = Tmp;
                            break;
                        }
                    }
                }
                if (CsConst.myFHs == null || OIP == null)
                {
                    OIP = new FH();
                    OIP.DeviceName = strDevName;
                    OIP.DIndex = dIndex;
                    OIP.ReadDefaultInfo();
                    if (CsConst.myFHs == null) CsConst.myFHs = new List<FH>(); CsConst.myFHs.Add(OIP);
                }
                #endregion
            }
            else if (DSDeviceTypeList.HDLDSDeviceTypeList.Contains(wdDeviceType))//是不是10寸屏室外机
            {

            }
            else if (HotelMixModuleDeviceType.HDLRCUDeviceTypeLists.Contains(wdDeviceType))//是不是客房控制器
            {
                #region
                MHRCU OIP = null;
                if (CsConst.myRcuMixModules != null)
                {
                    foreach (MHRCU Tmp in CsConst.myRcuMixModules)
                    {
                        if (Tmp.DIndex == dIndex)
                        {
                            OIP = Tmp;
                            break;
                        }
                    }
                }
                if (CsConst.myRcuMixModules == null || OIP == null)
                {
                    OIP = new MHRCU();
                    OIP.Devname = strDevName;
                    OIP.DIndex = dIndex;
                    OIP.ReadDefaultInfo();
                    if (CsConst.myRcuMixModules == null) CsConst.myRcuMixModules = new List<MHRCU>(); CsConst.myRcuMixModules.Add(OIP);
                }
                #endregion
            }
            else if (MSPUSenserDeviceTypeList.SensorsDeviceTypeList.Contains(wdDeviceType))//红外超声波传感器
            {
                #region
                MSPU OIP = null;
                if (CsConst.myPUSensors != null)
                {
                    foreach (MSPU Tmp in CsConst.myPUSensors)
                    {
                        if (Tmp.DIndex == dIndex)
                        {
                            OIP = Tmp;
                            break;
                        }
                    }
                }
                if (CsConst.myPUSensors == null || OIP == null)
                {
                    OIP = new MSPU();
                    OIP.Devname = strDevName;
                    OIP.DIndex = dIndex;
                    OIP.ReadDefaultInfo();
                    if (CsConst.myPUSensors == null) CsConst.myPUSensors = new List<MSPU>(); CsConst.myPUSensors.Add(OIP);
                }
                #endregion

            }
            else if (EnviroDLPPanelDeviceTypeList.HDLEnviroDLPPanelDeviceTypeList.Contains(wdDeviceType))//彩色DLP
            {

            }
            else if (CoolMasterDeviceTypeList.HDLCoolMasterDeviceTypeList.Contains(wdDeviceType))//coolmaster
            {
                #region
                CoolMaster OIP = null;
                if (CsConst.myCoolMasters != null)
                {
                    foreach (CoolMaster Tmp in CsConst.myCoolMasters)
                    {
                        if (Tmp.DIndex == dIndex)
                        {
                            OIP = Tmp;
                            break;
                        }
                    }
                }
                if (CsConst.myCoolMasters == null || OIP == null)
                {
                    OIP = new CoolMaster();
                    OIP.DeviceName = strDevName;
                    OIP.DIndex = dIndex;
                    OIP.ReadDefaultInfo();
                    if (CsConst.myCoolMasters == null) CsConst.myCoolMasters = new List<CoolMaster>(); CsConst.myCoolMasters.Add(OIP);
                }
                #endregion
            }
            else if (MiniSenserDeviceTypeList.SensorsDeviceTypeList.Contains(wdDeviceType))//MINI超声波
            {
                #region
                MiniSensor oMINIUL = new MiniSensor();
                if (CsConst.myMiniSensors != null)
                {
                    foreach (MiniSensor oTmp in CsConst.myMiniSensors)
                    {
                        if (oTmp.DIndex == dIndex)
                        {
                            oMINIUL = oTmp;
                            break;
                        }
                    }
                }
                else
                {
                    oMINIUL.DIndex = dIndex;
                    oMINIUL.Devname = strDevName;
                    CsConst.myMiniSensors = new List<MiniSensor>(); CsConst.myMiniSensors.Add(oMINIUL);
                }
                #endregion
            }
            else if (CsConst.minDoorBellDeviceType.Contains(wdDeviceType))//门铃
            {

            }
            else if (CsConst.minCardReaderDeviceType.Contains(wdDeviceType))//插卡取电
            {

            }
            else if (CsConst.mintHotelACDeviceType.Contains(wdDeviceType))//酒店空调面板
            {

            }
            else if (CsConst.minMSPUBRFDeviceType.Contains(wdDeviceType))//电池装传感器
            {

            }
            else if (DSDeviceTypeList.NewDoorStationDeviceType.Contains(wdDeviceType))//新门口机
            {

            }
            else if (SocketDeviceTypeList.HDLSocketDeviceTypeList.Contains(wdDeviceType))//无线插座
            {
                #region
                RFSocket oCamera = null;

                if (CsConst.mySockets != null)
                {
                    foreach (RFSocket oTmp in CsConst.mySockets)
                    {
                        if (oTmp.DIndex == dIndex)
                        {
                            oCamera = oTmp;
                            break;
                        }
                    }
                }
                if (CsConst.mySockets == null || oCamera == null)
                {
                    oCamera = new RFSocket();
                    oCamera.DIndex = dIndex;
                    oCamera.DeviceName = strDevName;

                    if (CsConst.mySockets == null) CsConst.mySockets = new List<RFSocket>(); CsConst.mySockets.Add(oCamera);
                }
                #endregion
            }
            else if (CameraNvrDeviceType.cameraThirdPartDeviceType.Contains(wdDeviceType)) // 第三方摄像头
            {
                #region
                Camera oCamera = null;
                if (CameraNvrDeviceType.nvrThridPartDeviceType.Contains(wdDeviceType))
                    oCamera = new Nvr();
                if (CsConst.myCameras != null)
                {
                    foreach (Camera oTmp in CsConst.myCameras)
                    {
                        if (oTmp.dIndex == dIndex)
                        {
                            oCamera = oTmp;
                            break;
                        }
                    }
                }
                if (CsConst.myCameras == null || oCamera == null)
                {
                    oCamera = new Camera();
                    oCamera.dIndex = dIndex;
                    oCamera.Devname = strDevName;

                    if (CsConst.myCameras == null) CsConst.myCameras = new List<Camera>(); CsConst.myCameras.Add(oCamera);
                }
                #endregion
            }
            else if (CsConst.minFANControllerDeviceType.Contains(wdDeviceType))//风扇控制器
            {

            }
            else if (CsConst.minBrazilWirelessPanelDeviceType.Contains(wdDeviceType))//巴西无线面板
            {

            }
            else if (CsConst.minSensor_7in1DeviceType.Contains(wdDeviceType))//7合一
            {

            }
            else if (CsConst.SonosConvertorDeviceType.Contains(wdDeviceType)) // sonos 转换器
            {

            }
            #endregion
        }

        public static string InquirePanelControTypeStringFromDB(int ID)
        {
            string reSult = CsConst.WholeTextsList[1775].sDisplayName;
            OleDbDataReader dr = null;
            try
            {
                string sql = "select * from tmpAirControlTypeForPanelControl where ID = " + ID.ToString();
                dr = DataModule.SearchAResultSQLDB(sql);
                if (dr != null)
                {
                    while (dr.Read())
                    {
                        if (CsConst.iLanguageId == 1) reSult = dr.GetValue(1).ToString();
                        else if (CsConst.iLanguageId == 0) reSult = dr.GetValue(2).ToString();
                    }
                    dr.Close();
                }
            }
            catch
            {
                dr.Close();
            }
            return reSult;
        }


        public static void AddControlTypeToControl(ComboBox cb, int deviceType)
        {

            OleDbDataReader dr = null;
            try
            {
                string str = "";
                cb.Items.Clear();
                string sql = "select * from defKeyFunType ";
                if (CsConst.minSameControlOfDryCont.Contains(deviceType))
                {
                    sql = sql + "where KeyFunType <= 103 and KeyFunType <> 87 and KeyFunType <> 93 and KeyFunType <> 96";
                }
                else if (CsConst.minMs04DeviceType.Contains(deviceType) && deviceType != 5900)
                {
                    sql = sql + "where KeyFunType <= 103 and KeyFunType <> 87 and KeyFunType <> 93 and KeyFunType <> 96";
                }
                else if (EnviroDLPPanelDeviceTypeList.HDLEnviroDLPPanelDeviceTypeList.Contains(deviceType))
                {
                    sql = sql + "where KeyFunType <= 103 and KeyFunType <> 87 and KeyFunType <> 93 and KeyFunType <> 96";
                }
                else if (SMSModuleDeviceTypeList.HDLGPRSModuleDeviceTypeLists.Contains(deviceType))
                {
                    sql = sql + "where KeyFunType <= 103 and KeyFunType <> 87 and KeyFunType <> 93 and KeyFunType <> 94 and KeyFunType <> 96";
                }
                else if (ConstMMC.MediaPBoxDeviceType.Contains(deviceType))
                {
                    sql = sql + "where KeyFunType <= 103 and KeyFunType <> 87 and KeyFunType <> 93 and KeyFunType <> 96";
                }
                else if (CsConst.minAllPanelDeviceType.Contains(deviceType))
                {
                    if (CsConst.mintDLPDeviceType.Contains(deviceType))
                    {
                        if (CsConst.mintNewDLPFHSetupDeviceType.Contains(deviceType))
                            sql = sql + "where KeyFunType <> 96 and KeyFunType <> 106 and KeyFunType <> 107 ";
                        else
                            sql = sql + "where KeyFunType <= 108 and KeyFunType <> 96 and KeyFunType <> 106 and KeyFunType <> 105 and KeyFunType <> 107";

                    }
                    else
                    {
                        sql = sql + "where KeyFunType <= 108 and KeyFunType <> 96 and KeyFunType <> 106 and KeyFunType <> 105 and KeyFunType <> 107";
                    }
                }
                else
                {
                    sql = sql + "where KeyFunType <= 103 and KeyFunType <> 87 and KeyFunType <> 93 and KeyFunType <> 96";
                }
                
                dr = DataModule.SearchAResultSQLDB(sql);
                if (dr != null)
                {
                    while (dr.Read())
                    {
                        if (CsConst.iLanguageId == 1) str = dr.GetValue(1).ToString();
                        else if (CsConst.iLanguageId == 0) str = dr.GetValue(2).ToString();
                        cb.Items.Add(str);
                    }
                    dr.Close();
                }
            }
            catch
            {
                dr.Close();
            }
        }

        public static void getPanlControlType(ComboBox cb, int devicetype)
        {
            OleDbDataReader dr = null;
            try
            {
                string str = "";
                cb.Items.Clear();
                string sql = "select * from tmpAirControlTypeForPanelControl ";
                if (CsConst.minSameControlOfDryCont.Contains(devicetype))
                {
                    sql = sql + "where ID <= 24 order by IndexValue";
                }
                else if (CsConst.minMs04DeviceType.Contains(devicetype) && devicetype != 5900)
                {
                    sql = sql + "where ID <= 24 order by IndexValue";
                }
                else if (EnviroDLPPanelDeviceTypeList.HDLEnviroDLPPanelDeviceTypeList.Contains(devicetype))
                {
                    sql = sql + "where ID <= 24 order by IndexValue";
                }
                else if (SMSModuleDeviceTypeList.HDLGPRSModuleDeviceTypeLists.Contains(devicetype))
                {
                    sql = sql + "where ID <= 12 order by IndexValue";
                }
                else if (ConstMMC.MediaPBoxDeviceType.Contains(devicetype))
                {
                    sql = sql + "where ID <= 24 order by IndexValue";
                }
                else if (CsConst.minAllPanelDeviceType.Contains(devicetype))
                {
                    if (CsConst.mintDLPDeviceType.Contains(devicetype))
                    {
                        if (CsConst.mintNewDLPFHSetupDeviceType.Contains(devicetype))
                        {
                            if (devicetype == 168 || devicetype == 170 || devicetype == 172)
                                sql = sql + "where ID <= 30 order by IndexValue";
                            else
                                sql = sql + "where ID <= 29 order by IndexValue";
                        }
                        else
                        {
                            sql = sql + "where ID <= 24 order by IndexValue";
                        }
                    }
                    else
                        sql = sql + "where ID <= 28 order by IndexValue";
                }
                else if (CsConst.minCardReaderDeviceType.Contains(devicetype))
                {
                    sql = sql + "where ID <= 24 order by IndexValue";
                }
                else if (CsConst.minRs232DeviceType.Contains(devicetype))
                {
                    sql = sql + "where ID <= 24 order by IndexValue";
                }
                else if (Eightin1DeviceTypeList.HDL8in1DeviceType.Contains(devicetype) || CsConst.minSensor_7in1DeviceType.Contains(devicetype))
                {
                    sql = sql + "where ID <= 24 order by IndexValue";
                }
                else
                {
                    sql = sql + " order by IndexValue";
                }

                dr = DataModule.SearchAResultSQLDB(sql);
                if (dr != null)
                {
                    while (dr.Read())
                    {
                        if (CsConst.iLanguageId == 1) str = dr.GetValue(1).ToString();
                        else if (CsConst.iLanguageId == 0) str = dr.GetValue(2).ToString();
                        cb.Items.Add(str);
                    }
                    dr.Close();
                }
            }
            catch
            {
                dr.Close();
            }
        }

        public static int getIDFromPanelControlTypeString(string strType)
        {
            int ID = 0;
            OleDbDataReader dr = null;
            try
            {
                string sql = "select * from tmpAirControlTypeForPanelControl";
                dr = DataModule.SearchAResultSQLDB(sql);
                if (dr != null)
                {
                    while (dr.Read())
                    {
                        if (dr.GetValue(1).ToString() == strType || dr.GetValue(2).ToString() == strType)
                        {
                            ID = Convert.ToInt32(dr.GetValue(0));
                            break;
                        }
                    }
                    dr.Close();
                }
            }
            catch
            {
                ID = 0;
                dr.Close();
            }
            return ID;
        }

        // 读入欲转换的图片并转成为 WritableBitmap www.it165.net 03.
        public static System.Drawing.Image ConvertToInvert(System.Drawing.Image img)
        {
            if (img == null) return null;
            System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(img);
            for (int y = 0; y < bitmap.Height; y++)
            {
                for (int x = 0; x < bitmap.Width; x++)
                {
                    // 取得每一个 pixel 09.            
                    var pixel = bitmap.GetPixel(x, y);
                    // 负片效果 将其反转 12.            
                    System.Drawing.Color newColor = System.Drawing.Color.FromArgb(pixel.A, 255 - pixel.R, 255 - pixel.G, 255 - pixel.B);
                    bitmap.SetPixel(x, y, newColor);
                }
            }
            return bitmap;
        }

    }
}
