using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HDL_Buspro_Setup_Tool
{
    [Serializable]
    public class TemperatureSource
    {
        public Boolean enableBroadcast;
        public Byte broadcastToSubnetID;           //子网ID
        public Byte broadcastToDeviceID;        //设备ID
        public Byte channelID;                  //channel number 
        public Byte autoReadOrWaitBroadcast;    //auto read or wait broadcast
        public Byte adjustTemperature; // 温度补偿

        public TemperatureSource()
        {
            enableBroadcast = false;  
            broadcastToSubnetID = 0;
            broadcastToDeviceID = 0;
            channelID = 0;
            autoReadOrWaitBroadcast = 0;
            adjustTemperature = 10;
        }

        public void ReadTemperatureSensorSettingInformation(Byte SubNetID, Byte DeviceID, int DeviceType)
        {
            Byte[] ArayTmp = new Byte[] { channelID };
            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1C00, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
            {
                channelID = CsConst.myRevBuf[25];
                enableBroadcast = (CsConst.myRevBuf[26] ==1);
                broadcastToSubnetID = CsConst.myRevBuf[27];
                broadcastToDeviceID = CsConst.myRevBuf[28];
                adjustTemperature = CsConst.myRevBuf[29];
                CsConst.myRevBuf = new byte[1200];
            }
            else return;
        }

        public void ModifyTemperatureSensorSettingInformation(Byte SubNetID, Byte DeviceID, int DeviceType)
        {
            Byte[] ArayTmp = new Byte[] { channelID,Convert.ToByte(enableBroadcast),broadcastToSubnetID,broadcastToDeviceID,adjustTemperature };
            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1C02, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
            {
            }
            else return;
        }
    }


}
