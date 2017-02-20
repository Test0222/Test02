using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HDL_Buspro_Setup_Tool
{
    [Serializable]
    public class ACSetting
    {
        public bool IsEnable; // 当前空调功能开启
        public DeviceInfo MyHVAC = null;    //HVAC子网ID
        public byte ACNo;    // 空调ID
        public byte ACType;   // 控制类型，新旧红外码之分

        public byte AcPowerOn;

        public byte bytAutoControl; // 自动控制
        public byte bytIRON;//红外上电控制
        public byte bytRunMode;    // 红外控制实际运行模式
        public byte bytSendIR;  //红外发射
        // 基本信息里面的结构

        public byte[] bytAryWind;//风速选择
        public byte[] bytAryMode;// 模式选择

        public byte bytSavePower; //省电模式
        public byte bytWind;      //要不要扫风

        public byte[] bytTempArea;  // 不同模式下的温度范围
        public byte bytTempSensor;  // 温度传感器模式类型，以内外哪个为准
        public byte[] TempSensors;  // 32ge byte 四个一组表示一个传感器

        public byte[] arayControl;//控制信息
        public byte bytPic = 0;//图片类型
        //public string[] strPics;//图片
        //public List<UVCMD.ControlTargets> KeyTargets;

        public ACSetting()
        {
            bytAryWind  = new Byte[5];
            bytAryMode = new Byte[6];
            bytTempArea = new Byte[] { 0,30,0,30,0,30,0,5,35};

            TempSensors = new Byte[32];
            arayControl = new Byte[20];
        }

        public Boolean UploadACSettingToDevice(Byte SubNetID, Byte DeviceID, int DeviceType, Byte TemperatureType, Byte FhLowLimit, Byte FhHighLimit, Byte AdjustValue)
        {
            Boolean blnIsSuccess = true;
            #region
            if (CsConst.mintDLPDeviceType.Contains(DeviceType))
            {
                if (this != null)
                {
                    byte[] bytTmp = new byte[1];//显示温度类型
                    bytTmp[0] = TemperatureType;
                    if (CsConst.mySends.AddBufToSndList(bytTmp, 0xE122, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == false) return false;
                    HDLUDP.TimeBetwnNext(1);
                    CsConst.myRevBuf = new byte[1200];

                    bytTmp = new byte[2];  //要不要省电模式，扫风
                    bytTmp[0] = bytSavePower;
                    bytTmp[1] = bytWind;
                    if (CsConst.mySends.AddBufToSndList(bytTmp, 0x1911, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == false) return false;
                    HDLUDP.TimeBetwnNext(2);
                    CsConst.myRevBuf = new byte[1200];

                    bytTmp = new byte[11]; // 温度范围
                    bytTmp[6] = TemperatureType;
                    for (byte bytI = 0; bytI < 6; bytI++) bytTmp[bytI] = bytTempArea[bytI];
                    bytTmp[7] = bytTempArea[6];
                    bytTmp[8] = bytTempArea[7];
                    bytTmp[9] = FhLowLimit;
                    bytTmp[10] = FhHighLimit;
                    if (CsConst.mySends.AddBufToSndList(bytTmp, 0x1902, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == false) return false;
                    HDLUDP.TimeBetwnNext(bytTmp.Length);
                    CsConst.myRevBuf = new byte[1200];

                    //模式和风速
                    blnIsSuccess = SaveHVACModeAndFan(SubNetID, DeviceID, DeviceType);

                    

                    //红外控制空调
                    blnIsSuccess = SaveIrControlHVACFlag(SubNetID, DeviceID, DeviceType);

                    //红外自动控制
                    blnIsSuccess = SaveIrAutoControlHVACFlag(SubNetID, DeviceID, DeviceType);

                    blnIsSuccess = SaveHVACAddressAndAdjustValue(SubNetID, DeviceID, DeviceType, AdjustValue);
                }
            }
            #endregion
            return blnIsSuccess;
        }      

        public void DownloadACSettingFromDevice(Byte SubNetID, Byte DeviceID, int DeviceType, ref Byte AdjustValue, ref Byte FhLowLimit, ref Byte FhHighLimit)
        {
            #region
            Byte[] ArayTmp = new Byte[0];

            MyHVAC = new DeviceInfo("0-0");
            arayControl = new byte[10];
            // HVAC basic setup
            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xE0E4, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)  // read temperature type
            {
                IsEnable = (CsConst.myRevBuf[26] == 1);

                MyHVAC = new DeviceInfo(CsConst.myRevBuf[27] + "-" + CsConst.myRevBuf[28] + @"\" + " ");    //HVAC子网ID
                AdjustValue = CsConst.myRevBuf[29];//补偿值
                ACNo = CsConst.myRevBuf[30];    // 空调ID
                ACType = CsConst.myRevBuf[31];   // 控制类型，新旧红外码之分
                bytIRON = CsConst.myRevBuf[32];//红外上电控制
                AcPowerOn = CsConst.myRevBuf[33];//上电保存
                CsConst.myRevBuf = new byte[1200];
                HDLUDP.TimeBetwnNext(10);
            }
            else return;

            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xE124, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)  // read mode
            {
                bytAryWind = new byte[5];
                for (byte bytI = 0; bytI < 5; bytI++) { bytAryWind[bytI] = CsConst.myRevBuf[25 + bytI]; }

                bytAryMode = new byte[6];
                for (byte bytI = 0; bytI < 5; bytI++) { bytAryMode[bytI] = CsConst.myRevBuf[30 + bytI]; }
                CsConst.myRevBuf = new byte[1200];
                HDLUDP.TimeBetwnNext(1);
            }
            else return;

            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1900, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)  // read temperature range
            {
                bytTempArea = new byte[8];
                for (byte bytI = 0; bytI < 6; bytI++) { bytTempArea[bytI] = CsConst.myRevBuf[25 + bytI]; }
                bytTempArea[6] = CsConst.myRevBuf[32];
                bytTempArea[7] = CsConst.myRevBuf[33];

                FhLowLimit = CsConst.myRevBuf[34];
                FhHighLimit = CsConst.myRevBuf[35];
                CsConst.myRevBuf = new byte[1200];
                HDLUDP.TimeBetwnNext(1);
            }
            else return;

            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1913, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)  // read temperature sensors
            {
                bytTempSensor = CsConst.myRevBuf[25];

                TempSensors = new byte[32];
                Array.Copy(CsConst.myRevBuf, 26, TempSensors, 0, 32);
                CsConst.myRevBuf = new byte[1200];
                HDLUDP.TimeBetwnNext(10);
            }
            else return;

            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x190F, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)  // read power saving or wind
            {
                bytSavePower = CsConst.myRevBuf[25];
                bytWind = CsConst.myRevBuf[26];
                CsConst.myRevBuf = new byte[1200];
                HDLUDP.TimeBetwnNext(10);
            }
            else return;

            if (CsConst.mySends.AddBufToSndList(null, 0xE0F0, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
            {//红外控制空调
                bytSendIR = CsConst.myRevBuf[25];
                HDLUDP.TimeBetwnNext(10);
            }
            else return;

            if (CsConst.mySends.AddBufToSndList(null, 0x1906, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
            {//红外自动控制
                bytRunMode = Convert.ToByte((CsConst.myRevBuf[25] >> 4) & 0x0F);
                bytAutoControl = Convert.ToByte(CsConst.myRevBuf[25] & 0x0F);
                CsConst.myRevBuf = new byte[1200];
                HDLUDP.TimeBetwnNext(10);
            }
            else return;
            #endregion
        }

        public Boolean SaveTemperatureSensor(Byte SubNetID, Byte DeviceID, int DeviceType)
        {
            Boolean blnIsSuccess = true;

            Byte[] bytTmp = new byte[33];   //温度传感器
            bytTmp[0] = bytTempSensor;
            if (bytTmp[0] != 0 && TempSensors != null)
            {
                TempSensors.CopyTo(bytTmp, 1);
            }
            blnIsSuccess = CsConst.mySends.AddBufToSndList(bytTmp, 0x1915, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType));
            HDLUDP.TimeBetwnNext(bytTmp.Length);
            CsConst.myRevBuf = new byte[1200];
            return blnIsSuccess;
        }
        
        public Boolean SaveHVACModeAndFan(Byte SubNetID, Byte DeviceID, int DeviceType)
        {
            Boolean blnIsSuccess = true;

            Byte[] ArayTmp = new byte[11];  // 显示模式 + 风速
            ArayTmp[0] = 0;
            byte bytIndex = 1;
            bytAryWind.CopyTo(ArayTmp, 0);
            bytAryMode.CopyTo(ArayTmp, 5);

            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(45);
            blnIsSuccess = CsConst.mySends.AddBufToSndList(ArayTmp, 0xE126, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType));
            HDLUDP.TimeBetwnNext(ArayTmp.Length);
            CsConst.myRevBuf = new byte[1200]; 

            return blnIsSuccess;
        }
        
        public Boolean SaveHVACAddressAndAdjustValue(Byte SubNetID, Byte DeviceID, int DeviceType, Byte AdjustValue)
        {
            Boolean blnIsSuccess = true;
            if (MyHVAC != null)
            {
                Byte[] bytTmp = new byte[8];   //HVAC基本设置
                bytTmp[0] = Convert.ToByte(IsEnable);
                bytTmp[1] = MyHVAC.SubnetID;
                bytTmp[2] = MyHVAC.DeviceID;
                bytTmp[3] = AdjustValue;
                bytTmp[4] = ACNo;
                bytTmp[5] = ACType;
                bytTmp[6] = bytIRON;
                bytTmp[7] = AcPowerOn;
                blnIsSuccess = CsConst.mySends.AddBufToSndList(bytTmp, 0xE0E6, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType));
                HDLUDP.TimeBetwnNext(bytTmp.Length);
                CsConst.myRevBuf = new byte[1200];               
            }
            return blnIsSuccess;
        }

        public Boolean SaveIrControlHVACFlag(Byte SubNetID, Byte DeviceID, int DeviceType)
        {
            Boolean blnIsSuccess = true;
            Byte[] ArayTmp = new byte[1]{bytSendIR};   //红外控制空调
            blnIsSuccess = CsConst.mySends.AddBufToSndList(ArayTmp, 0xE0F2, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType));
            CsConst.myRevBuf = new byte[1200];

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
            Byte[] ArayTmp = new byte[1];
            ArayTmp[0] = Convert.ToByte((bytRunMode << 4) + bytAutoControl);    //红外自动控制
            blnIsSuccess = CsConst.mySends.AddBufToSndList(ArayTmp, 0x1908, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType));
            CsConst.myRevBuf = new byte[1200];

            return blnIsSuccess;
        }

        public Boolean ReadDLPACPageCurrentStatus(Byte SubNetID, Byte DeviceID, int DeviceType)
        {
            Boolean blnIsSuccess = true;

            if (CsConst.mySends.AddBufToSndList(null, 0xE0EC, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
            {
                for (int i = 0; i < 10; i++)
                {
                    arayControl[i] = CsConst.myRevBuf[25 + i];
                }
                CsConst.myRevBuf = new byte[1200];
                HDLUDP.TimeBetwnNext(10);
            }
            else blnIsSuccess = false;
            return blnIsSuccess;
        }

        public Boolean ReadDLPACPTemperatureRange(Byte SubNetID, Byte DeviceID, int DeviceType, ref Byte FhLowLimit, ref Byte FhHighLimit)
        {
            Boolean blnIsSuccess = true;

            blnIsSuccess = CsConst.mySends.AddBufToSndList(null, 0x1900, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType));  // read temperature range
            if (blnIsSuccess)
            {
                 bytTempArea = new byte[8];
                 for (byte bytI = 0; bytI < 6; bytI++) { bytTempArea[bytI] = CsConst.myRevBuf[25 + bytI]; }
                 bytTempArea[6] = CsConst.myRevBuf[32];
                 bytTempArea[7] = CsConst.myRevBuf[33];

                 FhLowLimit = CsConst.myRevBuf[34];
                 FhHighLimit = CsConst.myRevBuf[35];
                 CsConst.myRevBuf = new byte[1200];
            }
            return blnIsSuccess;
        }
    }
}
