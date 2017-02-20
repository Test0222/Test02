using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace HDL_Buspro_Setup_Tool
{
    [Serializable]
    [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public class HdlDevice
    {
        public Byte subnetID;
        public Byte deviceID;
        public String remark;
        public Int32 DeviceType;
        public Byte bigType;
        public Byte smallType;
        public Byte sumChns;

        public void SetToDefault()
        {
            subnetID = 0;
            deviceID = 0;
            remark = "";
            DeviceType = 0;
            bigType = 0;
            smallType = 0;
        }

        public void onSiteOutput(Byte bChannelId)
        {
            try
            {
                if (this is HdlSimpleLights)
                {
                    HdlSimpleLights oTmp = (HdlSimpleLights)(this);
                    Byte[] arrSendBuf = new Byte[4]{bChannelId,oTmp.sumLightChns[bChannelId -1].curStatus[0],0,0};
                    CsConst.mySends.AddBufToSndList(arrSendBuf, 0x0031, subnetID, deviceID, false, false, false, false);
                }
                else if (this is HdlSimpleCurtains)
                {
                    HdlSimpleCurtains oTmp = (HdlSimpleCurtains)(this);
                    Byte[] arrSendBuf = new Byte[4] { bChannelId, oTmp.sumCurtainChns[bChannelId - 1].curStatus[0], 0, 0 };
                    CsConst.mySends.AddBufToSndList(arrSendBuf, 0xE3E0, subnetID, deviceID, false, false, false, false);
                }
                else if (this is HdlSimpleButtons)
                {
                    HdlSimpleButtons oTmp = (HdlSimpleButtons)(this);
                    Byte[] arrSendBuf = new Byte[4] {18, bChannelId, oTmp.sumButtonDrys[bChannelId - 1].curStatus[0], 0 };
                    CsConst.mySends.AddBufToSndList(arrSendBuf, 0xE3D8, subnetID, deviceID, false, false, false, false);
                }
            }
            catch
            { }
        }

        public UVCMD.ControlTargets GetCommandFrmBasicInformation(Byte bChannelId)
        {
            UVCMD.ControlTargets tmp = new UVCMD.ControlTargets();
            try
            {
                tmp.SubnetID = subnetID;
                tmp.DeviceID = deviceID;
                tmp.Param1 = bChannelId;
                switch (bigType)
                {
                    case 1:
                     if (smallType ==0 || smallType ==1)
                     {
                          if (this is HdlSimpleLights)
                          {
                               HdlSimpleLights oTmp = (HdlSimpleLights)(this);
                               tmp.Type = 89;
                               tmp.Param2 = oTmp.sumLightChns[bChannelId - 1].curStatus[0];                                
                         }
                     }
                     break;
                }
            }
            catch
            {
                return tmp;
            }
            return tmp;
        }

        public void UpdateHdlDeviceFrmBasicInformation(Byte bChannelId, String sTmpText)
        {
            try
            {
                switch (bigType)
                {
                    case 1:  //灯光
                        if (smallType == 0 || smallType == 1)
                        {
                            if (this is HdlSimpleLights)
                            {
                                HdlSimpleLights oTmp = (HdlSimpleLights)(this);
                                if (oTmp.sumLightChns[bChannelId - 1].curStatus == null)
                                    oTmp.sumLightChns[bChannelId - 1].curStatus = new Byte[1];
                                oTmp.sumLightChns[bChannelId - 1].curStatus[0] = Convert.ToByte(sTmpText);
                            }
                        }
                        break;
                    case 2: // 窗帘
                        if (this is HdlSimpleCurtains)
                        {
                            HdlSimpleCurtains oTmp = (HdlSimpleCurtains)(this);
                            if (oTmp.sumCurtainChns[bChannelId - 1].curStatus == null)
                                oTmp.sumCurtainChns[bChannelId - 1].curStatus = new Byte[1];
                            oTmp.sumCurtainChns[bChannelId - 1].curStatus[0] = Convert.ToByte(sTmpText);
                        }
                        break;
                    case 4: //  按键或者干节点
                        if (this is HdlSimpleButtons)
                        {
                            HdlSimpleButtons oTmp = (HdlSimpleButtons)(this);
                            if (oTmp.sumButtonDrys[bChannelId - 1].curStatus == null)
                                oTmp.sumButtonDrys[bChannelId - 1].curStatus = new Byte[1];
                            oTmp.sumButtonDrys[bChannelId - 1].curStatus[0] = Convert.ToByte(sTmpText);
                        }
                        break;
                }
            }
            catch
            {
            }
        }
    }

    [Serializable]
    [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public class HdlSimpleLights : HdlDevice
    {
        public List<DimmerChannel> sumLightChns = null;
    }

    [Serializable]
    [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public class HdlSimpleCurtains : HdlDevice
    {
        public List<BasicCurtain> sumCurtainChns = null;
    }

    [Serializable]
    [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public class HdlSimpleButtons : HdlDevice
    {
        public List<HDLButton> sumButtonDrys = null;
    }

    [Serializable]
    [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public class HdlSimpleSensors : HdlDevice
    {
        public List<Byte[]> sensorParameters = null;
    }
}
