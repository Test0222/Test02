using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;

namespace HDL_Buspro_Setup_Tool
{
    public class CoolMasterNet : CoolMaster
    {
        public string strIP;
        public bool Enable;
        public int Port;

        public bool UploadInfosToDeviceNetVersion(string DevNam, int wdDeviceType, int intActivePage)
        {
            string strMainRemark = DevNam.Split('\\')[1].Trim();
            DevNam = DevNam.Split('\\')[0].Trim();
            byte bytSubID = byte.Parse(DevNam.Split('-')[0].ToString());
            byte bytDevID = byte.Parse(DevNam.Split('-')[1].ToString());

            byte[] ArayMain = new byte[20];
            byte[] arayTmpRemark = HDLUDP.StringToByte(strMainRemark);
            //ruby test
            if (arayTmpRemark.Length > 20)
            {
                Array.Copy(arayTmpRemark, 0, ArayMain, 0, 20);
            }
            else
            {
                Array.Copy(arayTmpRemark, 0, ArayMain, 0, arayTmpRemark.Length);
            }

            if (CsConst.mySends.AddBufToSndList(ArayMain, 0x0010, bytSubID, bytDevID, false, true, true, false) == false)
            {
                return false;
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(5, null);
            if (intActivePage == 0 || intActivePage == 1)
            {
                byte[] ArayTmp = new byte[8];
                ArayTmp[0] = 0;
                if (Enable)
                    ArayTmp[1] = 1;
                ArayTmp[2] = Convert.ToByte(strIP.Split('.')[0].ToString());
                ArayTmp[3] = Convert.ToByte(strIP.Split('.')[1].ToString());
                ArayTmp[4] = Convert.ToByte(strIP.Split('.')[2].ToString());
                ArayTmp[5] = Convert.ToByte(strIP.Split('.')[3].ToString());
                ArayTmp[6] = Convert.ToByte(Port / 256);
                ArayTmp[7] = Convert.ToByte(Port % 256);
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1386, bytSubID, bytDevID, false, true, true, false) == true)
                {
                    CsConst.myRevBuf = new byte[1200];
                    HDLUDP.TimeBetwnNext(ArayTmp.Length);
                }
                else return false;
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(10, null);
                for (int i = 0; i < myACSetting.Count; i++)
                {
                    ArayTmp = new byte[8];
                    ArayTmp[0] = myACSetting[i].ID;
                    ArayTmp[1] = myACSetting[i].Enable;
                    ArayTmp[2] = myACSetting[i].ACNO;
                    ArayTmp[3] = myACSetting[i].GroupID;
                    if (myACSetting[i].arayACinfo == null || myACSetting[i].arayACinfo.Length < 3)
                    {
                        myACSetting[i].arayACinfo = new byte[3];
                        myACSetting[i].arayACinfo[0] = 41;
                    }
                    Array.Copy(myACSetting[i].arayACinfo, 0, ArayTmp, 4, 3);
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1386, bytSubID, bytDevID, false, true, true, false) == true)
                    {
                        CsConst.myRevBuf = new byte[1200];
                        HDLUDP.TimeBetwnNext(ArayTmp.Length);
                        if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(10 + i, null);
                    }
                    else return false;
                }
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(100, null);
            return true;
        }

        public bool DownLoadInfoFrmDeviceNetVersion(string DevNam, int wdDeviceType, int intActivePage, int num1, int num2)
        {
            string strMainRemark = DevNam.Split('\\')[1].Trim();
            DevNam = DevNam.Split('\\')[0].Trim();
            byte bytSubID = byte.Parse(DevNam.Split('-')[0].ToString());
            byte bytDevID = byte.Parse(DevNam.Split('-')[1].ToString());
            byte[] ArayTmp = null;
            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x000E, bytSubID, bytDevID, false, true, true, false) == false)
            {
                return false;
            }
            else
            {
                byte[] arayRemark = new byte[20];
                for (int intI = 0; intI < 20; intI++) { arayRemark[intI] = CsConst.myRevBuf[25 + intI]; }
                DeviceName = bytSubID.ToString() + "-" + bytDevID.ToString() + "\\" + HDLPF.Byte2String(arayRemark);
                CsConst.myRevBuf = new byte[1200];
            }
            if (CsConst.isRestore)
            {
                num1 = 1;
                num2 = 64;
            }
            if (intActivePage == 0 || intActivePage == 1)
            {
                ArayTmp = new byte[1];
                ArayTmp[0] = 0;
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1388, bytSubID, bytDevID, false, true, true, false) == true)
                {
                    Enable = (CsConst.myRevBuf[26] == 1);
                    strIP = CsConst.myRevBuf[27].ToString("D3") + "." + CsConst.myRevBuf[28].ToString("D3") + "."
                          + CsConst.myRevBuf[29].ToString("D3") + "." + CsConst.myRevBuf[30].ToString("D3");
                    Port = CsConst.myRevBuf[31] * 256 + CsConst.myRevBuf[32];
                }
                else return false;
                if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(5, null);
                myACSetting = new List<ThirdPartAC>();
                for (int i = num1; i <= num2; i++)
                {
                    ArayTmp = new byte[1];
                    ArayTmp[0] = Convert.ToByte(i);
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1388, bytSubID, bytDevID, false, true, true, false) == true)
                    {
                        ThirdPartAC temp = new ThirdPartAC();
                        temp.ID = Convert.ToByte(i);
                        temp.Enable = CsConst.myRevBuf[26];
                        temp.ACNO = CsConst.myRevBuf[27];
                        temp.GroupID = CsConst.myRevBuf[28];
                        temp.arayACinfo = new byte[3];
                        Array.Copy(CsConst.myRevBuf, 29, temp.arayACinfo, 0, 3);
                        myACSetting.Add(temp);
                    }
                    if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(5 + i, null);
                }
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(100, null);
            return true;
        }
    }
}
