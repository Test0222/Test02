using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace HDL_Buspro_Setup_Tool
{
    [Serializable]
    [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public class BasicCurtain
    {
        public Int32 id;
        public String remark;
        public Int32 runTime;
        public Int32 onDelay;
        public Int32 offDelay;
        public Byte[] curStatus; //当前状态


        public BasicCurtain()
        {
            remark = "";
            runTime = 0;
            onDelay = 0;
            offDelay = 0;
        }

        public Boolean ModifyCurtainSetupInformation(Byte SubNetID, Byte DeviceID,Byte CurtainID,int wdDeviceType)
        {
            Boolean blnSuccess = true;
            try
            {
                if (MS04DeviceTypeList.WirelessMS04WithOneCurtain.Contains(wdDeviceType)) // 无线窗帘
                {
                    blnSuccess = ModifyWirelessCurtainSetupInformation(SubNetID, DeviceID, CurtainID, wdDeviceType);
                }
                else
                {
                    blnSuccess = ModifyNormalCurtainSetupInformation(SubNetID, DeviceID, CurtainID, wdDeviceType);
                }

                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(20 * CurtainID);
            }
            catch( Exception ex)
            {

            }
            return blnSuccess;
        }

        public Boolean ModifyNormalCurtainSetupInformation(Byte SubNetID, Byte DeviceID, Byte CurtainID, int wdDeviceType)
        {
            Boolean blnSuccess = true;
            try
            {
                byte[] arayTmp = new byte[21];
                arayTmp[0] = CurtainID;

                byte[] arayTmp1 = HDLUDP.StringToByte(remark);
                arayTmp1.CopyTo(arayTmp, 1);

                if (CsConst.mySends.AddBufToSndList(arayTmp, 0xF010, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType)) == false) return false;

                arayTmp = new byte[3];
                arayTmp[0] = CurtainID;
                arayTmp[1] = byte.Parse((runTime / 256).ToString());   // modify valid or not
                arayTmp[2] = byte.Parse((runTime % 256).ToString());
                if (CsConst.mySends.AddBufToSndList(arayTmp, 0xE802, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType)) == false) return false;
                HDLUDP.TimeBetwnNext(20);

                arayTmp[1] = byte.Parse((onDelay / 256).ToString());   // modify valid or not
                arayTmp[2] = byte.Parse((onDelay % 256).ToString());
                if (CsConst.mySends.AddBufToSndList(arayTmp, 0xE807, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType)) == false) return false;
                HDLUDP.TimeBetwnNext(20);

                if (wdDeviceType != 707)
                {
                    arayTmp[1] = byte.Parse((offDelay / 256).ToString());      // modify off delay
                    arayTmp[2] = byte.Parse((offDelay % 256).ToString());
                    if (CsConst.mySends.AddBufToSndList(arayTmp, 0xE80B, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType)) == false) return false;
                }
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(20 * CurtainID);
            }
            catch (Exception ex)
            {

            }
            return blnSuccess;
        }

        public Boolean ModifyWirelessCurtainSetupInformation(Byte SubNetID, Byte DeviceID, Byte CurtainID, int wdDeviceType)
        {
            Boolean blnSuccess = true;
            try
            {
                byte[] arayTmp = new byte[21];
                arayTmp[0] = CurtainID;

                byte[] arayTmp1 = HDLUDP.StringToByte(remark);
                HDLSysPF.CopyRemarkBufferToSendBuffer(arayTmp1, arayTmp, 1);

                if (CsConst.mySends.AddBufToSndList(arayTmp, 0x3328, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType)) == false) return false;

                arayTmp = new byte[3];
                arayTmp[0] = CurtainID;
                arayTmp[1] = byte.Parse((runTime / 256).ToString());   // modify valid or not
                arayTmp[2] = byte.Parse((runTime % 256).ToString());
                if (CsConst.mySends.AddBufToSndList(arayTmp, 0x3320, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType)) == false) return false;
                HDLUDP.TimeBetwnNext(20);

                arayTmp = new byte[4];
                arayTmp[0] = Convert.ToByte(onDelay / 256);
                arayTmp[1] = Convert.ToByte(onDelay % 256);
                arayTmp[2] = Convert.ToByte(offDelay / 256);
                arayTmp[3] = Convert.ToByte(onDelay % 256);
                blnSuccess = CsConst.mySends.AddBufToSndList(arayTmp, 0xF04F, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType));
                HDLUDP.TimeBetwnNext(20);
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(20 * CurtainID);
            }
            catch (Exception ex)
            {

            }
            return blnSuccess;
        }

        public Boolean ReadCurtainSetupInformation(Byte SubNetID, Byte DeviceID, Byte CurtainID, int wdDeviceType)
        {
            Boolean blnIsSuccess = true;

            if (MS04DeviceTypeList.WirelessMS04WithOneCurtain.Contains(wdDeviceType)) // 无线窗帘模块
            {
                blnIsSuccess = ReadWirelessCurtainSetupInformation(SubNetID, DeviceID, CurtainID, wdDeviceType);
            }
            else
            {
                blnIsSuccess = ReadNormalCurtainSetupInformation(SubNetID, DeviceID, CurtainID, wdDeviceType);
            }
          
            return blnIsSuccess;
        }

        public Boolean ReadNormalCurtainSetupInformation(Byte SubNetID, Byte DeviceID, Byte CurtainID, int wdDeviceType)
        {
            Boolean blnIsSuccess = true;
            Byte[] ArayTmp = new Byte[] { CurtainID };
            int OperationCode = 0xF00E;

            if (CsConst.mySends.AddBufToSndList(ArayTmp, OperationCode, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType)) == true)
            {
                byte[] arayRemark = new byte[20];
                HDLSysPF.CopyRemarkBufferFrmMyRevBuffer(CsConst.myRevBuf, arayRemark, 26);
                remark = HDLPF.Byte2String(arayRemark);
                CsConst.myRevBuf = new byte[1200];
            }
            else
            {
                return false;
            }

            OperationCode = 0xE800;
            if (CsConst.mySends.AddBufToSndList(ArayTmp, OperationCode, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType)) == true)
            {
                runTime = CsConst.myRevBuf[26] * 256 + CsConst.myRevBuf[27];
                CsConst.myRevBuf = new byte[1200];
            }
            else return false;

            OperationCode = 0xE805;
            if (CsConst.mySends.AddBufToSndList(ArayTmp, OperationCode, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType)) == true)
            {
                onDelay = CsConst.myRevBuf[26] * 256 + CsConst.myRevBuf[27];
                CsConst.myRevBuf = new byte[1200];
            }
            else return false;

            if (wdDeviceType != 707 && !(MS04DeviceTypeList.WirelessMS04WithOneCurtain.Contains(wdDeviceType))) //有开关延迟的
            {
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xE809, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType)) == true)
                {
                    offDelay = CsConst.myRevBuf[26] * 256 + CsConst.myRevBuf[27];
                    CsConst.myRevBuf = new byte[1200];
                }
                else return false;
            }
            return blnIsSuccess;
        }

        public Boolean ReadWirelessCurtainSetupInformation(Byte SubNetID, Byte DeviceID, Byte CurtainID, int wdDeviceType)
        {
            Boolean blnIsSuccess = true;
            Byte[] ArayTmp = new Byte[] { CurtainID };
            //int OperationCode = 0x332A;

            //if (CsConst.mySends.AddBufToSndList(ArayTmp, OperationCode, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType)) == true)
            //{
            //    byte[] arayRemark = new byte[20];
            //    HDLSysPF.CopyRemarkBufferFrmMyRevBuffer(CsConst.myRevBuf, arayRemark, 26);
            //    remark = HDLPF.Byte2String(arayRemark);
            //    CsConst.myRevBuf = new byte[1200];
            //}
            //else
            //{
            //    return false;
            //}

            //running time
            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x3322, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType)) == true)
            {
                runTime = CsConst.myRevBuf[25] * 256 + CsConst.myRevBuf[26];
                CsConst.myRevBuf = new byte[1200];
            }
            else return false;

            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xF04D, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(wdDeviceType)) == true)
            {
                onDelay = CsConst.myRevBuf[25] * 256 + CsConst.myRevBuf[26];
                offDelay = CsConst.myRevBuf[27] * 256 + CsConst.myRevBuf[28];
                CsConst.myRevBuf = new byte[1200];
            }
            else return false;

            return blnIsSuccess;
        }
    }
}
