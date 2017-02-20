using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HDL_Buspro_Setup_Tool
{
    public class EnviroAc
    {
        public Byte IconID; // 图标ID
        public String Remark; // 备注
        public byte ID;//空调页号
        public byte Enable;//使能 0关闭 1开启
        public byte DesSubnetID;//目标子网ID
        public byte DesDevID;//目标设备ID
        public byte ACNum;//空调号
        public byte ControlWays;//空调控制命令选择，0旧，1新
        public byte OperationEnable;//运算使能，0关闭，1开启
        public byte PowerOnRestoreState;//上电开关状态，0关，1保持上次状态 
        public byte IROperationEnable;//红外运算控制使能，0禁止，1允许
        public byte IRControlEnable;//空调红外控制使能，0禁止，1允许
        public byte IRPowerOnControlEnable;//空调红外上电控制使能，0禁止，1开启
        public byte FanEnable;//扫风使能
        public byte FanEnergySaveEnable;//风机节能使能，0不省电，1省电
        public byte[] FanParam = new byte[20];//风速类型(0,自动，1高风，2中风，3低风),风速种类存储器1，风速种类存储器2，风速种类存储器3，风速种类存储器4
        //模式类型（0制冷，1制热，2通风，3自动，4除湿）模式种类存储器1，模式种类存储器2，模式种类存储器3，模式种类存储器4，模式种类存储器5
        public byte[] CoolParam = new byte[20];//制冷温度范围低限，高限，制热温度范围低限，高限，自动温度范围低限，高限，除湿温度范围低限，高限
        public byte[] OutDoorParam = new byte[20];//空调温度源（0本机，1外部，2两者平均），外部温度源1使能（0关，1收广播，2度）
        //外部温度源1子网地址,外部温度源1设备地址，外部温度源1通道号，外部温度源2使能
        //外部温度源2子网地址，外部温度源2设备地址，外部温度源2通道号，

        public byte ACSwitch;//空调开关（1开0关）
        public byte CoolingTemp;//制冷控制温度
        public byte HeatingTemp;//制热控制温度
        public byte AutoTemp;//自动控制温度
        public byte DryTemp;//除湿控制温度
        public byte ACMode;//空调模式(0制冷，1制热，2通风，3自动，4除湿)
        public byte ACWind;//空调风速(0自动，1高风，2中风，3低风)
        public byte ACFanEnable;//扫风使能（0不扫风，1扫风）
        public byte WorkingMode;//实际工作状态
        public byte WorkingWind;//实际工作风速
        public byte WorkingFan;//实际扫风状态
        public byte EnviromentTemp;//环境温度

        public EnviroAc()
        {
            FanParam = new Byte[20];

            OutDoorParam = new Byte[20];
            CoolParam = new Byte[20];
        }

        public Boolean UploadACSettingToDevice(Byte SubNetID, Byte DeviceID, int DeviceType, Byte TemperatureType, Byte FhLowLimit, Byte FhHighLimit, Byte AdjustValue)
        {
            Boolean blnIsSuccess = true;
           
            return blnIsSuccess;
        }      

        public void DownloadACSettingFromDevice(Byte SubNetID, Byte DeviceID, int DeviceType, ref Byte AdjustValue, ref Byte FhLowLimit, ref Byte FhHighLimit)
        {
            
        }

        public Boolean SaveTemperatureSensor(Byte SubNetID, Byte DeviceID, int DeviceType)
        {
            Boolean blnIsSuccess = true;

            
            return blnIsSuccess;
        }
        
        public Boolean SaveHVACModeAndFan(Byte SubNetID, Byte DeviceID, int DeviceType)
        {
            Boolean blnIsSuccess = true;

            

            return blnIsSuccess;
        }
        
        public Boolean SaveHVACAddressAndAdjustValue(Byte SubNetID, Byte DeviceID, int DeviceType, Byte AdjustValue)
        {
            Boolean blnIsSuccess = true;
            
            return blnIsSuccess;
        }

        public Boolean SaveIrControlHVACFlag(Byte SubNetID, Byte DeviceID, int DeviceType)
        {
            Boolean blnIsSuccess = true;
            
            return blnIsSuccess;
        }

        /// <summary>
        /// 红外自动控制
        /// </summary>
        /// <param name="SubNetID"></param>
        /// <param name="DeviceID"></param>
        /// <param name="DeviceType"></param>
        /// <returns></returns>
        public Boolean SaveIrAutoControlHVACFlag(Byte SubNetID, Byte DeviceID, int DeviceType)
        {
            Boolean blnIsSuccess = true;
            
            return blnIsSuccess;
        }

        public Boolean ReadButtonRemark(byte SubNetID, byte DeviceID, int DeviceType, bool isShowMessage)
        {
            try
            {
                byte[] ArayTmp = null;
                int CMD = 0xE004;
                if (EnviroNewDeviceTypeList.EnviroNewPanelDeviceTypeList.Contains(DeviceType))
                {
                    ArayTmp = new byte[4];
                    ArayTmp[0] = Convert.ToByte(ID + 1);
                    ArayTmp[1] = 5;
                    ArayTmp[2] = 0;
                    ArayTmp[3] = 9;
                }
                //读取备注
                if (CsConst.mySends.AddBufToSndList(ArayTmp, CMD, SubNetID, DeviceID, false, isShowMessage, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                {
                    byte[] arayRemark = new byte[20];
                    Array.Copy(CsConst.myRevBuf, 26, arayRemark, 0, 20);
                    Remark = HDLPF.Byte2String(arayRemark);
                    HDLUDP.TimeBetwnNext(1);
                }
                else return false;
            }
            catch
            {
                return false;
            }
            return true;
        }


        public Boolean ReadDLPACPageSettings(Byte SubNetID, Byte DeviceID, int DeviceType, Byte AcNo)
        {
            Boolean blnIsSuccess = true;
            ID = AcNo;
            Byte[] ArayTmp = new Byte[] { AcNo};
            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x19AA, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
            {
                Enable = CsConst.myRevBuf[26];
                DesSubnetID = CsConst.myRevBuf[27];
                DesDevID = CsConst.myRevBuf[28];
                ACNum = CsConst.myRevBuf[29];
                if (ACNum < 1 || ACNum > 128) ACNum = 1;
                ControlWays = CsConst.myRevBuf[30];
                OperationEnable = CsConst.myRevBuf[31];
                PowerOnRestoreState = CsConst.myRevBuf[32];
                IROperationEnable = CsConst.myRevBuf[33];
                IRControlEnable = CsConst.myRevBuf[34];
                IRPowerOnControlEnable = CsConst.myRevBuf[35];
                FanEnable = CsConst.myRevBuf[36];
                FanEnergySaveEnable = CsConst.myRevBuf[37];
                FanParam = new byte[20];
                Array.Copy(CsConst.myRevBuf, 38, FanParam, 0, 11);
                CoolParam = new byte[20];
                Array.Copy(CsConst.myRevBuf, 49, CoolParam, 0, 8);
                OutDoorParam = new byte[20];
                Array.Copy(CsConst.myRevBuf, 57, OutDoorParam, 0, 9);
                CsConst.myRevBuf = new byte[1200];
            }
            return blnIsSuccess;
        }

        public Boolean ReadDLPACPageCurrentStatus(Byte SubNetID, Byte DeviceID, int DeviceType)
        {
            Boolean blnIsSuccess = true;
            Byte[] ArayTmp = new byte[1];
            ArayTmp[0] = ID;

            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x19AE, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
            {
                ACSwitch = CsConst.myRevBuf[26];
                CoolingTemp = CsConst.myRevBuf[27];
                HeatingTemp = CsConst.myRevBuf[28];
                AutoTemp = CsConst.myRevBuf[29];
                DryTemp = CsConst.myRevBuf[30];
                ACMode = CsConst.myRevBuf[31];
                ACWind = CsConst.myRevBuf[32];
                ACFanEnable = CsConst.myRevBuf[33];
                WorkingMode = CsConst.myRevBuf[35];
                WorkingWind = CsConst.myRevBuf[36];
                WorkingFan = CsConst.myRevBuf[37];
                EnviromentTemp = CsConst.myRevBuf[38];
                CsConst.myRevBuf = new byte[1200];
            }
            return blnIsSuccess;
        }

        public Boolean ReadDLPACPTemperatureRange(Byte SubNetID, Byte DeviceID, int DeviceType, ref Byte FhLowLimit, ref Byte FhHighLimit)
        {
            Boolean blnIsSuccess = true;

            blnIsSuccess = CsConst.mySends.AddBufToSndList(null, 0x1900, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType));  // read temperature range
            
            return blnIsSuccess;
        }
    }
}
