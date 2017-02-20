using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Threading;
using System.Runtime.Serialization.Formatters.Binary;

namespace HDL_Buspro_Setup_Tool
{
    public partial class frmManualFunctionLists : Form
    {
        private Byte[] MyCMDBufferCopy = new byte[10];

        private static List<UVCMD.SimpleSearchList> MyTmpSearchList = null;

        private delegate void FlushClient(); // 代理
        private delegate void FlushClientDataGrid(object[] tmp1); //代理
        Thread thread = null;
        private SingleChn oChn = null; // 新增回路控件
        private TimeText oTime = null; // 新增运行时间控件
        private RelayChn oRelay = null; //新增继电器的开关状态
        private CurtainChn oCurtain = null; //新增加窗帘状态
        private RGB oRGB = null; // 新增加颜色选择块
        private RGBW oRGBW = null; //新增加彩色灯
        private CCT oCCT = null; //新增加CCT
        private PictureBox myTmpPic = new PictureBox();
        private ComboBox myIRType = new ComboBox(); // 红外码类型
        private ComboBox myIRCodes = new ComboBox(); // 红外码列表
        private Boolean bReSearchSimpleList = false;

        private Int32 currentSelectedBigType = -1;
        private Byte bSelectedSubNetId = 0;
        private Byte bSelectedDeviceId = 0;

        public frmManualFunctionLists()
        {
            InitializeComponent();
        }

        private void frmEMain_Load(object sender, EventArgs e)
        {
            try
            {
                if (CsConst.myOnlines == null || CsConst.myOnlines.Count == 0) return;

                foreach (DevOnLine temp in CsConst.myOnlines)
                {
                    String[] strTmp = DeviceTypeList.GetDisplayInformationFromPublicModeGroup(temp.DeviceType);
                    String deviceModel = strTmp[0];
                    String deviceDescription = strTmp[1];

                    object[] obj = {false, HDL_Buspro_Setup_Tool.Properties.Resources.OK, dgOnline.RowCount+1, temp.bytSub, temp.bytDev, 
                                                           deviceModel, temp.DevName.Split('\\')[1].ToString(), 
                                                           deviceDescription, "", temp.intDIndex.ToString(),""};

                    dgOnline.Rows.Add(obj);
                }
                HdlUdpPublicFunctions.OldVersionDevicesGetSimpleListManually();

                //if (bReSearchSimpleList == false)
                //{
                //    if (File.Exists(@"C:\casatuneZones.dat") == true)
                //    {
                //        FileStream fs = new FileStream(@"C:\casatuneZones.dat", FileMode.Open);
                //        BinaryFormatter bf = new BinaryFormatter();
                //        if (CsConst.simpleSearchDevicesList == null)
                //            CsConst.simpleSearchDevicesList = new List<HdlDevice>();
                //        CsConst.simpleSearchDevicesList = bf.Deserialize(fs) as List<HdlDevice>;
                //        fs.Close();
                //    }
                //}
                //else
                //{
                //    HdlUdpPublicFunctions.OldVersionDevicesGetSimpleListManually();
                //}
            }
            catch
            {
                
            }
        }

        private void btnLights_Click(object sender, EventArgs e)
        {
            LightLists.Rows.Clear();
            Button Tmp = ((Button)sender);
            if (Tmp.Tag == null) return;
            Byte ButtonTag = Convert.ToByte(Tmp.Tag.ToString());
            currentSelectedBigType = ButtonTag;
            try
            {
                DisplaySimpleFunctionListToDataGridview(LightLists, ButtonTag);
                LightLists.Enabled = true;
                LightLists.AddSpanHeader(0, 16, "oDimmer.DeviceName");
                LightLists.MergeColumnNames = new List<string>();
                LightLists.MergeColumnNames.Add("smallType1");
                LightLists.MergeColumnNames.Add("Location1");
                LightLists.MergeColumnNames.Add("DeviceName1");
            }
            catch
            { }
        }

        public static void DisplaySimpleFunctionListToDataGridview(RowMergeView dgOnline, Byte bigType)
        {
            if (CsConst.simpleSearchDevicesList == null || CsConst.simpleSearchDevicesList.Count == 0) return;
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
                            String sRemark = "";
                            if (bigType == 1) // light
                            {
                                sHeadString = CsConst.sLightSmallTypesList[temp.smallType % 3];
                                sCurrentStatus = "0";
                                sRemark = ((HdlSimpleLights)(temp)).sumLightChns[bChnId - 1].remark;
                            }
                            else if (bigType == 2) // curtain
                            {
                                sHeadString = CsConst.sCurtainSmallTypesList[temp.smallType % 9];
                                sCurrentStatus = CsConst.CurtainModes[2];
                                sRemark = ((HdlSimpleCurtains)(temp)).sumCurtainChns[bChnId - 1].remark;
                            }
                            else if (bigType == 4) // button or dry contact
                            {
                                sHeadString = CsConst.sButtonSmallTypesList[temp.smallType % 2];
                                sCurrentStatus = CsConst.Status[0];
                                sRemark = ((HdlSimpleButtons)(temp)).sumButtonDrys[bChnId - 1].Remark;
                            }
                            else if (bigType == 5) // sensor 
                            {
                                sHeadString = CsConst.sSensorSmallTypesList[temp.smallType % 8];
                                sCurrentStatus = CsConst.Status[1];
                                sRemark = temp.remark;
                            }
                            object[] obj = {true,sHeadString, temp.subnetID.ToString() + "-" +temp.deviceID + "-" + temp.remark.ToString(), 
                                           "Locate", bChnId.ToString(),sRemark, 0,100,sCurrentStatus,false};

                            if (oTmpDataGrid.InvokeRequired)//等待异步
                            {
                                FlushClientDataGrid fc = new FlushClientDataGrid(ThreadFunctionFlashDataGrid);
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
            catch { }
            oTmpDataGrid = null;
        }

        public static void DisplayOneDevicesSimpleFunctionListToDataGridview(RowMergeView dgOnline, Byte bSubNetId, Byte bDeviceId)
        {
            if (CsConst.simpleSearchDevicesList == null || CsConst.simpleSearchDevicesList.Count == 0) return;
            if (dgOnline == null) return;
            oTmpDataGrid = dgOnline;
            oTmpDataGrid.Rows.Clear();
            try
            {
                foreach (HdlDevice temp in CsConst.simpleSearchDevicesList)
                {
                    Int32 iRowCount = 1;
                    if (temp.subnetID == bSubNetId && temp.deviceID == bDeviceId)
                    {
                        for (Byte bChnId = 1; bChnId <= temp.sumChns; bChnId++)
                        {
                            String sHeadString = CsConst.sLightSmallTypesList[temp.smallType % 9];
                            String sCurrentStatus = "0";
                            String sRemark = "";
                            Boolean bIsSelected = false;
                            if (temp.bigType == 1) // light
                            {
                                sHeadString = CsConst.sLightSmallTypesList[temp.smallType % 3];
                                sCurrentStatus = "0";
                                sRemark = ((HdlSimpleLights)(temp)).sumLightChns[bChnId - 1].remark;
                                if (((HdlSimpleLights)(temp)).sumLightChns[bChnId - 1].curStatus == null || 
                                    ((HdlSimpleLights)(temp)).sumLightChns[bChnId - 1].curStatus.Length <2) bIsSelected = false;
                                else 
                                    bIsSelected = (((HdlSimpleLights)(temp)).sumLightChns[bChnId - 1].curStatus[1] ==0xF8);
                            }
                            else if (temp.bigType == 2) // curtain
                            {
                                sHeadString = CsConst.sCurtainSmallTypesList[temp.smallType % 9];
                                sCurrentStatus = CsConst.CurtainModes[2];
                                sRemark = ((HdlSimpleCurtains)(temp)).sumCurtainChns[bChnId - 1].remark;
                                if (((HdlSimpleCurtains)(temp)).sumCurtainChns[bChnId - 1].curStatus == null ||
                                    ((HdlSimpleCurtains)(temp)).sumCurtainChns[bChnId - 1].curStatus.Length < 2) bIsSelected = false;
                                else
                                    bIsSelected = (((HdlSimpleCurtains)(temp)).sumCurtainChns[bChnId - 1].curStatus[1] == 0xF8);
                            }
                            else if (temp.bigType == 4) // button or dry contact
                            {
                                sHeadString = CsConst.sButtonSmallTypesList[temp.smallType % 2];
                                sCurrentStatus = CsConst.Status[0];
                                sRemark = ((HdlSimpleButtons)(temp)).sumButtonDrys[bChnId - 1].Remark;
                                if (((HdlSimpleButtons)(temp)).sumButtonDrys[bChnId - 1].curStatus == null ||
                                    ((HdlSimpleButtons)(temp)).sumButtonDrys[bChnId - 1].curStatus.Length < 2) bIsSelected = false;
                                else
                                    bIsSelected = (((HdlSimpleButtons)(temp)).sumButtonDrys[bChnId - 1].curStatus[1] == 0xF8);
                            }
                            else if (temp.bigType == 5) // sensor 
                            {
                                sHeadString = CsConst.sSensorSmallTypesList[temp.smallType % 8];
                                sCurrentStatus = CsConst.Status[1];
                                sRemark = temp.remark;
                            }
                            object[] obj = {bIsSelected,sHeadString, temp.subnetID.ToString() + "-" +temp.deviceID + "-" + temp.remark.ToString(), 
                                           bChnId.ToString(),sRemark};

                            if (oTmpDataGrid.InvokeRequired)//等待异步
                            {
                                FlushClientDataGrid fc = new FlushClientDataGrid(ThreadFunctionFlashDataGrid);
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
                oTmpDataGrid.MergeColumnNames = new List<string>();
                oTmpDataGrid.MergeColumnNames.Add("smallType1");
                oTmpDataGrid.MergeColumnNames.Add("Location1");
                oTmpDataGrid.MergeColumnNames.Add("DeviceName1");
            }
            catch { }
            oTmpDataGrid = null;
        }

        public static void ThreadFunctionFlashDataGrid(object[] obj)
        {
            oTmpDataGrid.Rows.Add(obj);
        }

        private static RowMergeView oTmpDataGrid = null;

        private void btnSave_Click(object sender, EventArgs e)
        {
            ModifySearchingDifferentType();
        }

        void ModifySearchingDifferentType()
        {
            if (MyTmpSearchList == null || MyTmpSearchList.Count == 0) return;
            int intRowIndex = 0;
            foreach (UVCMD.SimpleSearchList oTmp in MyTmpSearchList)
            {
                for (byte i = 1; i <= oTmp.ChnAll; i++)
                {
                    int intID = Convert.ToInt32(LightLists[0, intRowIndex].Value.ToString()); 
                    //Modify information
                    if (oTmp.FunctionType == 1) // lights
                    {
                        #region
                        switch (oTmp.DetailType)
                        {
                            case 0:
                                byte[] ArayTmp = new byte[26];
                                ArayTmp[0] = 1;
                                ArayTmp[1] =oTmp.DetailType;
                                ArayTmp[2] = i;
                                byte[] arayTmpRemark = HDLUDP.StringToByte(LightLists[4, intRowIndex].Value.ToString());
                                //ruby test
                                if (arayTmpRemark.Length > 20)
                                {
                                    Array.Copy(arayTmpRemark, 0, ArayTmp, 3, 20);
                                }
                                else
                                {
                                    Array.Copy(arayTmpRemark, 0, ArayTmp, 3, arayTmpRemark.Length);
                                }
                                ArayTmp[23] = byte.Parse(LightLists[5, intRowIndex].Value.ToString());
                                ArayTmp[24] = byte.Parse(LightLists[6, intRowIndex].Value.ToString());
                                ArayTmp[25] = 100;
                                CsConst.replytimes = 3;
                                CsConst.mySends.AddBufToSndList(ArayTmp, 0xE44C, oTmp.Address[0], oTmp.Address[01], false, false, false,false);//少于3次发送命令
                                break;
                            case 1:
                                ArayTmp = new byte[23];
                                ArayTmp[0] = 1;
                                ArayTmp[1] = 1;
                                ArayTmp[2] = i;
                                arayTmpRemark = HDLUDP.StringToByte(LightLists[4, intRowIndex].Value.ToString());
                                //ruby test
                                if (arayTmpRemark.Length > 20)
                                {
                                    Array.Copy(arayTmpRemark, 0, ArayTmp, 3, 20);
                                }
                                else
                                {
                                    Array.Copy(arayTmpRemark, 0, ArayTmp, 3, arayTmpRemark.Length);
                                }
                                CsConst.replytimes = 3;
                                CsConst.mySends.AddBufToSndList(ArayTmp, 0xE44C, oTmp.Address[0], oTmp.Address[01], false, false, false,false);//少于3次发送命令
                                break;
                        }
                        #endregion
                    }
                    else if (oTmp.FunctionType == 2) // curtain
                    {

                    }
                    else if (oTmp.FunctionType == 4) // button or dry contact
                    {
                        #region
                        switch (oTmp.DetailType)
                        {
                            case 0: 
                            case 1:
                                byte[] ArayTmp = new byte[24];
                                ArayTmp[0] = 4;
                                ArayTmp[1] = oTmp.DetailType;
                                ArayTmp[2] = (byte)intID;
                                byte[] arayTmpRemark = HDLUDP.StringToByte(LightLists[4, intRowIndex].Value.ToString());
                                //ruby test
                                if (arayTmpRemark.Length > 20)
                                {
                                    Array.Copy(arayTmpRemark, 0, ArayTmp, 3, 20);
                                }
                                else
                                {
                                    Array.Copy(arayTmpRemark, 0, ArayTmp, 3, arayTmpRemark.Length);
                                }
                                ArayTmp[23] = 20;
                                CsConst.replytimes = 3;
                                CsConst.mySends.AddBufToSndList(ArayTmp, 0xE44C, oTmp.Address[0], oTmp.Address[01], false, false, false,false);//少于3次发送命令
                                break;
                        }
                        #endregion
                    }
                    else if (oTmp.FunctionType == 5) // sensor
                    {
                        #region
                        byte[] ArayTmp = new byte[23];
                        ArayTmp[0] = 5;
                        ArayTmp[1] = oTmp.DetailType;
                        ArayTmp[2] = (byte)intID;
                        byte[] arayTmpRemark = HDLUDP.StringToByte(LightLists[4, intRowIndex].Value.ToString());
                        //ruby test
                        if (arayTmpRemark.Length > 20)
                        {
                            Array.Copy(arayTmpRemark, 0, ArayTmp, 3, 20);
                        }
                        else
                        {
                            Array.Copy(arayTmpRemark, 0, ArayTmp, 3, arayTmpRemark.Length);
                        }
                        CsConst.replytimes = 3;
                        if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xE44C, oTmp.Address[0], oTmp.Address[01], false, false, false, false) == true)//少于3次发送命令
                        {
                            HDLUDP.TimeBetwnNext(10);
                        }
                        #endregion
                    }
                    else if (oTmp.FunctionType == 9) // Music box
                    {
                        #region
                        byte[] ArayTmp = new byte[24];
                        ArayTmp[0] = 9;
                        ArayTmp[1] = 0;
                        ArayTmp[2] = (byte)intID;
                        byte[] arayTmpRemark = HDLUDP.StringToByte(LightLists[4, intRowIndex].Value.ToString());
                        //ruby test
                        if (arayTmpRemark.Length > 20)
                        {
                            Array.Copy(arayTmpRemark, 0, ArayTmp, 3, 20);
                        }
                        else
                        {
                            Array.Copy(arayTmpRemark, 0, ArayTmp, 3, arayTmpRemark.Length);
                        }
                        ArayTmp[23] = 20;
                        CsConst.replytimes = 3;
                        CsConst.mySends.AddBufToSndList(ArayTmp, 0xE44C, oTmp.Address[0], oTmp.Address[01], false, false, false, false);//少于3次发送命令
                        #endregion
                    }
                    intRowIndex++;
                }
            }
            
        }

        private void LightLists_MouseDown(object sender, MouseEventArgs e)
        {
            if (((RowMergeView)sender).Rows.Count == 0) return;
            if (((RowMergeView)sender).CurrentCell.RowIndex == -1 || ((RowMergeView)sender).CurrentCell.ColumnIndex == -1) return;
            try
            {
                if (((RowMergeView)sender).CurrentCell.ColumnIndex == 3)
                {
                    if (((RowMergeView)sender).SelectedRows == null || ((RowMergeView)sender).CurrentRow.Index == -1) return;
                    CsConst.MyPublicCtrls = new List<UVCMD.ControlTargets>();
                    for (int iRowId = 0; iRowId < ((RowMergeView)sender).RowCount; iRowId++)
                    {
                        if (((RowMergeView)sender).Rows[iRowId].Selected == true)
                        //foreach (DataGridViewRow oDgRow in dgvListA.SelectedRows)
                        {
                            Byte bRowID = (Byte)iRowId;
                            Byte bChnId = Convert.ToByte(((RowMergeView)sender)[3, bRowID].Value.ToString());
                            String deviceName = ((RowMergeView)sender)[1, bRowID].Value.ToString();
                            if (deviceName == "" || deviceName == null) return;
                            HdlDevice tmpDevice = HdlUdpPublicFunctions.FindHdlDeviceWithNameAndBigType(currentSelectedBigType, deviceName);
                            if (tmpDevice != null)
                            {
                                UVCMD.ControlTargets tmp = tmpDevice.GetCommandFrmBasicInformation(bChnId);
                                CsConst.MyPublicCtrls.Add(tmp);
                            }
                        }
                    }
                    CsConst.iIsDragTemplateOrSimpleListOrCmd = 1;
                    ((RowMergeView)sender).DoDragDrop("Copy", DragDropEffects.Copy);
                }
            }
            catch { }
        }


        private void LightLists_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(System.String)))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            CsConst.MySimpleSearchQuene = new List<byte[]>(); 
            CsConst.MyBlnStartSimpleSearch = true;

            if (thread == null)
            {
                thread = new Thread(CrossThreadFlush);
                thread.IsBackground = true;
                thread.Start();
            }
            else if (thread.IsBackground == false)
            {
                thread.IsBackground = true;
                thread.Resume();
            }
            else if (thread.IsAlive == true)
            {
                thread.Suspend();
                thread.IsBackground = false;
            } 

        }

        private void CrossThreadFlush()
        {
            FlushClient fc = new FlushClient(ThreadFunction);
            this.Invoke(fc); //通过代理调用刷新方法

            while (thread.IsAlive)
            {
                //将sleep和无限循环放在等待异步的外面
                ThreadFunction();
                //Thread.Sleep(1);
            }
        }

        private void ThreadFunction()
        {
            if (CsConst.MySimpleSearchQuene != null && CsConst.MySimpleSearchQuene.Count != 0)
            {
                byte[] Tmp = CsConst.MySimpleSearchQuene[0];
                if (Tmp == null) return;
                bool blnIsAdd = false;
                if ((Tmp[21] * 256 + Tmp[22] == 0xE456) ||(Tmp[21] * 256 + Tmp[22] == 0xE457))
                {
                    blnIsAdd = true;
                }
                //display to form and will get feedback randomly
                #region
                if (blnIsAdd == true)
                {
                    string strTmp = DateTime.Now.ToString() + " : ";
                    for (byte bytI = 0; bytI < Tmp[16]; bytI++)
                    {
                        strTmp = strTmp + Tmp[16 + bytI].ToString("X2") + " ";
                    }
                    //this.rtbRev.AppendText(strTmp + '\n');
                    
                    //answer to panel to check the status
                    if (Tmp[21] * 256 + Tmp[22] == 0xE456)
                    {
                        switch (Tmp[26])
                        {
                            case 1: strTmp = " Short Press"; break;
                            case 2: strTmp = " Long Press"; break;
                            case 3: strTmp = " Release "; break;
                            case 4: strTmp = " Double Click"; break;
                            case 5: strTmp = " Dim Up"; break;
                            case 6: strTmp = " Dim Down"; break;
                        }
                      //  this.rtbRev.AppendText(strTmp + '\n');
                        #region
                        Random TmpRandom = new Random();
                        TmpRandom.Next();
                        int intTmp = TmpRandom.Next(7);
                        byte[] ArayTmp = new byte[30];
                        ArayTmp[0] = Tmp[25];

                        if (Tmp[26] == 2 || Tmp[26] ==5)
                        {                            
                            for (byte i = 0; i <= 10; i++)
                            {
                                ArayTmp[1] = 2;
                                ArayTmp[2] =(byte)(i * 10);
                                ArayTmp[3] = 100;
                                ArayTmp[4] = 100;
                                ArayTmp[5] = 100;
                                ArayTmp[6] = 100;
                                ArayTmp[7] = 0;
                                ArayTmp[8] = 0;

                                CsConst.mySends.AddBufToSndList(ArayTmp, 0xE457, 1, 122, false, false, false, false);
                                HDLUDP.TimeBetwnNext(10);
                            }
                        }
                        else if (Tmp[26] == 2 || Tmp[26] == 6)
                        {
                            for (byte i = 0; i <= 10; i++)
                            {
                                ArayTmp[1] = 2;
                                ArayTmp[2] = (byte)(100 - i * 10);
                                ArayTmp[3] = 100;
                                ArayTmp[4] = 100;
                                ArayTmp[5] = 100;
                                ArayTmp[6] = 100;
                                ArayTmp[7] = 0;
                                ArayTmp[8] = 0;

                                CsConst.mySends.AddBufToSndList(ArayTmp, 0xE457, 1, 122, false, false, false, false);
                                HDLUDP.TimeBetwnNext(10);
                            }
                        }
                        else if (Tmp[26] != 3)
                        {
                            #region
                            switch (intTmp)  //function  light level  led level  RGB   icon  content
                            {
                                case 0: ArayTmp[1] = 0;
                                    ArayTmp[2] = 100;
                                    ArayTmp[3] = 100;
                                    ArayTmp[4] = 100;
                                    ArayTmp[5] = 100;
                                    ArayTmp[6] = 100;
                                    ArayTmp[7] = 0;
                                    ArayTmp[8] = 1; break;  //on off on led 100 on icon

                                case 1: ArayTmp[1] = 0;
                                    ArayTmp[2] = 0;
                                    ArayTmp[3] = 0;
                                    ArayTmp[4] = 100;
                                    ArayTmp[5] = 100;
                                    ArayTmp[6] = 100;
                                    ArayTmp[7] = 0;
                                    ArayTmp[8] = 1; break;  //on off on led 10 on icon

                                case 2: ArayTmp[1] = 0;
                                    ArayTmp[2] = 100;
                                    ArayTmp[3] = 0;
                                    ArayTmp[4] = 100;
                                    ArayTmp[5] = 100;
                                    ArayTmp[6] = 100;
                                    ArayTmp[7] = 0;
                                    ArayTmp[8] = 0; break;  //on off on led 100 on icon

                                case 3: ArayTmp[1] = 0;
                                    ArayTmp[2] = 0;
                                    ArayTmp[3] = 100;
                                    ArayTmp[4] = 100;
                                    ArayTmp[5] = 100;
                                    ArayTmp[6] = 100;
                                    ArayTmp[7] = 0;
                                    ArayTmp[8] = 0; break;  //on off on led 100 on icon

                                case 4: ArayTmp[1] = 1;
                                    ArayTmp[2] = 100;
                                    ArayTmp[3] = 100;
                                    ArayTmp[4] = 100;
                                    ArayTmp[5] = 100;
                                    ArayTmp[6] = 100;
                                    ArayTmp[7] = 0;
                                    ArayTmp[8] = 0; break;  //on off on led 100 on icon

                                case 5: ArayTmp[1] = 1;
                                    ArayTmp[2] = 0;
                                    ArayTmp[3] = 100;
                                    ArayTmp[4] = 100;
                                    ArayTmp[5] = 100;
                                    ArayTmp[6] = 100;
                                    ArayTmp[7] = 0;
                                    ArayTmp[8] = 0; break;  //on off on led 100 on icon

                                case 6: ArayTmp[1] = 1;
                                    ArayTmp[2] = 0;
                                    ArayTmp[3] = 0;
                                    ArayTmp[4] = 100;
                                    ArayTmp[5] = 100;
                                    ArayTmp[6] = 100;
                                    ArayTmp[7] = 0;
                                    ArayTmp[8] = 0; break;  //on off on led 100 on icon

                                default:
                                    ArayTmp[1] = 1;
                                    ArayTmp[2] = 100;
                                    ArayTmp[3] = 0;
                                    ArayTmp[4] = 100;
                                    ArayTmp[5] = 100;
                                    ArayTmp[6] = 100;
                                    ArayTmp[7] = 0;
                                    ArayTmp[8] = 0; break;  //on off on led 100 on icon
                            }
                            #endregion
                            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xE457, Tmp[17], Tmp[18], false, false, false, false) == false)
                            {
                                // HDLUDP.TimeBetwnNext(10);
                            }
                        }
                        
                        #endregion
                    }
                }
                #endregion
                CsConst.MySimpleSearchQuene.Remove(Tmp);
            }
        }

        private void frmFunctionLists_Shown(object sender, EventArgs e)
        {

        }

        private void LightLists_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //(RowMergeView)sender
            if (((RowMergeView)sender).Rows.Count == 0) return;
            if (((RowMergeView)sender).CurrentCell.RowIndex == -1 || ((RowMergeView)sender).CurrentCell.ColumnIndex == -1) return;
            if (currentSelectedBigType == -1) return;
            String deviceName = ((RowMergeView)sender)[1, ((RowMergeView)sender).CurrentCell.RowIndex].Value.ToString();
            if (deviceName == "" || deviceName == null) return;
            HdlUdpPublicFunctions.LocateDeviceWhereItIs(currentSelectedBigType, deviceName);
        }

        private void LightLists_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            //if (LampLists[e.ColumnIndex, e.RowIndex].Value.ToString() == CsConst.mstrInvalid) return;
            if (e.ColumnIndex == -1 || e.RowIndex == -1) return;
            if (((RowMergeView)sender).RowCount == 0) return;
            if (oChn != null) oChn.Visible = false;
            if (oTime != null) oTime.Visible = false;
            if (oRelay != null) oRelay.Visible = false;
            if (oCurtain != null) oCurtain.Visible = false;
            if (oRGB != null) oRGB.Visible = false;
            if (oRGBW != null) oRGBW.Visible = false;
            if (oCCT != null) oCCT.Visible = false;
            if (myIRCodes != null) myIRCodes.Visible = false;
            if (myIRType != null) myIRType.Visible = false;
            RowMergeView gridTmpTable = ((RowMergeView)sender);
            try
            {
                int intLoad = currentSelectedBigType;
                int intIndex = Convert.ToInt32(gridTmpTable[3, e.RowIndex].Value.ToString());
                String deviceName = gridTmpTable[1, gridTmpTable.CurrentCell.RowIndex].Value.ToString();
                if (deviceName == "" || deviceName == null) return;
                HdlDevice tmpDevice = HdlUdpPublicFunctions.FindHdlDeviceWithNameAndBigType(currentSelectedBigType, deviceName);

                Rectangle rect = new Rectangle();
                if (e.ColumnIndex < 7) return;

                if (gridTmpTable[5, e.RowIndex].Value == null || gridTmpTable[5, e.RowIndex].Value.ToString() == "")
                    gridTmpTable[5, e.RowIndex].Value = "0";
                if (tmpDevice.bigType == 3) // 红外码
                {
                    #region
                    string strTmp = gridTmpTable[7, e.RowIndex].Value.ToString();

                    gridTmpTable.Controls.Add(myIRType);
                    rect = ((RowMergeView)sender).GetCellDisplayRectangle(7, e.RowIndex, true);
                    myIRType.Size = rect.Size;
                    myIRType.Top = rect.Top;
                    myIRType.Left = rect.Left;
                    myIRType.Show();
                    myIRType.Visible = true;

                    if (strTmp == CsConst.mstrInvalid) myIRType.SelectedIndex = 0;
                    else myIRType.Text = strTmp;
                    #endregion
                }
                else if (tmpDevice.bigType == 1) // 灯类型分类
                {
                    #region
                    // 表示灯类型
                    if (tmpDevice.smallType == 2)  //cct  // 冷暖光调节
                    {
                        string strTmp = gridTmpTable[7, e.RowIndex].Value.ToString();
                        if (strTmp == CsConst.mstrInvalid || strTmp.Contains("/") == false) strTmp = "50 / 2700K";
                        int intLevel = Convert.ToInt16(strTmp.Split('/')[0].ToString());
                        oCCT = new CCT(strTmp.Split('/')[1].ToString(), intLevel);

                        gridTmpTable.Controls.Add(oCCT);
                        rect = gridTmpTable.GetCellDisplayRectangle(7, e.RowIndex, true);
                        oCCT.Size = rect.Size;
                        oCCT.Top = rect.Top;
                        oCCT.Left = rect.Left;
                        oCCT.Show();
                        oCCT.Visible = true;
                    }
                    else if (tmpDevice.smallType == 3) //rgb  颜色
                    {
                        Color oTmp = gridTmpTable.Rows[e.RowIndex].Cells[7].Style.BackColor;
                        oRGB = new RGB(oTmp);

                        gridTmpTable.Controls.Add(oRGB);
                        rect = gridTmpTable.GetCellDisplayRectangle(7, e.RowIndex, true);
                        oRGB.Size = rect.Size;
                        oRGB.Top = rect.Top;
                        oRGB.Left = rect.Left;
                        oRGB.Show();
                        oRGB.Visible = true;
                    }
                    else if (tmpDevice.smallType == 4)  //rgbw  颜色
                    {
                        string strTmp = gridTmpTable[7, e.RowIndex].Value.ToString();
                        if (strTmp == CsConst.mstrInvalid) strTmp = "100";

                        Color oTmp = gridTmpTable.Rows[e.RowIndex].Cells[7].Style.BackColor;
                        int intLevel = Convert.ToInt16(strTmp);

                        oRGBW = new RGBW(oTmp, intLevel);

                        gridTmpTable.Controls.Add(oRGBW);
                        rect = gridTmpTable.GetCellDisplayRectangle(7, e.RowIndex, true);
                        oRGBW.Size = rect.Size;
                        oRGBW.Top = rect.Top;
                        oRGBW.Left = rect.Left;
                        oRGBW.Show();
                        oRGBW.Visible = true;
                    }
                    else if (tmpDevice.smallType == 1) //继电器只开关
                    {
                        Byte bRelayStatus = HDLPF.GetIndexFromBufferLists(CsConst.Status,gridTmpTable[7, e.RowIndex].Value.ToString());
                        oRelay = new RelayChn(intIndex);
                        gridTmpTable.Controls.Add(oRelay);
                        rect = gridTmpTable.GetCellDisplayRectangle(7, e.RowIndex, true);
                        oRelay.Size = rect.Size;
                        oRelay.Top = rect.Top;
                        oRelay.Left = rect.Left;
                        oRelay.Show();
                        oRelay.Visible = true;
                        oRelay._RelayID = bRelayStatus;
                        oRelay.TextChanged += oRelay_TextChanged;
                    }
                    else if (tmpDevice.smallType == 0 || tmpDevice.smallType == 8) //dimming 调光值
                    {
                        oChn = new SingleChn();

                        gridTmpTable.Controls.Add(oChn);
                        rect = gridTmpTable.GetCellDisplayRectangle(7, e.RowIndex, true);
                        oChn.Size = rect.Size;
                        oChn.Top = rect.Top;
                        oChn.Left = rect.Left;
                        oChn.Show();
                        oChn.Visible = true;

                        oChn.Text = gridTmpTable[7, e.RowIndex].Value.ToString();
                        oChn.TextChanged += oChn_TextChanged;
                    }
                    #endregion
                }
                else if (tmpDevice.bigType == 2)  // 窗帘
                {
                }
                else if (tmpDevice.bigType == 4) // 按键
                {
                    Byte bRelayStatus = HDLPF.GetIndexFromBufferLists(CsConst.Status, gridTmpTable[7, e.RowIndex].Value.ToString());
                    oRelay = new RelayChn(intIndex);
                    gridTmpTable.Controls.Add(oRelay);
                    rect = gridTmpTable.GetCellDisplayRectangle(7, e.RowIndex, true);
                    oRelay.Size = rect.Size;
                    oRelay.Top = rect.Top;
                    oRelay.Left = rect.Left;
                    oRelay.Show();
                    oRelay.Visible = true;
                    oRelay._RelayID = bRelayStatus;
                    oRelay.TextChanged += oRelay_TextChanged;
                }
            }
            catch { }
        }

        void oChn_TextChanged(object sender, EventArgs e)
        {
            if (LightLists.CurrentRow.Index < 0) return;
            if (LightLists.RowCount <= 0) return;
            
            for (int i = 0; i < LightLists.SelectedRows.Count; i++)
            {
                Byte bRowID = (Byte)LightLists.SelectedRows[i].Index;
                Byte bChnId = Convert.ToByte(LightLists[3, bRowID].Value.ToString());
                String deviceName = LightLists[1, bRowID].Value.ToString();
                if (deviceName == "" || deviceName == null) return;
                HdlDevice tmpDevice = HdlUdpPublicFunctions.FindHdlDeviceWithNameAndBigType(currentSelectedBigType, deviceName);

                LightLists[7, bRowID].Value = oChn.Text;
                tmpDevice.onSiteOutput((Byte)bChnId);
            }
        }

        void oRelay_TextChanged(object sender, EventArgs e)
        {
            RowMergeView gridTmpTable = LightLists;
            if (gridTmpTable.CurrentRow.Index < 0) return;
            if (gridTmpTable.RowCount <= 0) return;

            try
            {
                for (int i = 0; i < gridTmpTable.SelectedRows.Count; i++)
                {
                    Byte bRowID = (Byte)gridTmpTable.SelectedRows[i].Index;
                    Byte bChnId = Convert.ToByte(gridTmpTable[3, bRowID].Value.ToString());
                    String deviceName = gridTmpTable[1, bRowID].Value.ToString();
                    if (deviceName == "" || deviceName == null) return;
                    HdlDevice tmpDevice = HdlUdpPublicFunctions.FindHdlDeviceWithNameAndBigType(currentSelectedBigType, deviceName);

                    gridTmpTable[7, bRowID].Value = oRelay.Text;
                    tmpDevice.onSiteOutput((Byte)bChnId);
                }
            }
            catch { }
        }

       private void LampLists_CellEndEdit(object sender, DataGridViewCellEventArgs e)
       {
           try
           {
               // 获取结构体
               int intLoad = currentSelectedBigType;
               RowMergeView gridTmpTable = ((RowMergeView)sender);
               Byte bChnId = Convert.ToByte(gridTmpTable[3, e.RowIndex].Value.ToString());
               String deviceName = gridTmpTable[1, gridTmpTable.CurrentCell.RowIndex].Value.ToString();
               if (deviceName == "" || deviceName == null) return;
               HdlDevice tmpDevice = HdlUdpPublicFunctions.FindHdlDeviceWithNameAndBigType(currentSelectedBigType, deviceName);
               if (e.ColumnIndex < 7) return;

               string strTmp = CsConst.mstrInvalid;
               if (oRGB != null && oRGB.Visible)
               {
                   strTmp = string.Empty;
                   gridTmpTable[7, e.RowIndex].Value = strTmp;
                   gridTmpTable.Rows[e.RowIndex].Cells[7].Style.BackColor = oRGB._myColor;
               }
               else if (oRGBW != null && oRGBW.Visible)
               {
                   strTmp = oRGBW.Light.ToString();
                   gridTmpTable[7, e.RowIndex].Value = strTmp;
                   gridTmpTable.Rows[e.RowIndex].Cells[7].Style.BackColor = oRGBW._myColor;
               }
               else if (oCCT != null && oCCT.Visible)
               {
                   strTmp = oCCT.Light.ToString();
                   gridTmpTable[7, e.RowIndex].Value = strTmp + " / " + oCCT._channelID.ToString();
               }
               else if (myIRType != null && myIRType.Visible)
               {
                   strTmp = myIRType.Text;
                   gridTmpTable[7, e.RowIndex].Value = strTmp;
               }
               else if (oChn != null && oChn.Visible)
               {
                   strTmp = oChn.Text.ToString();
                   tmpDevice.UpdateHdlDeviceFrmBasicInformation(bChnId, strTmp);
               }
               else if (oRelay != null && oRelay.Visible)
               {
                   strTmp = CsConst.Status[oRelay._RelayID];
                   tmpDevice.UpdateHdlDeviceFrmBasicInformation(bChnId, (oRelay._RelayID * 100).ToString());
               }
               else if (oCurtain != null && oCurtain.Visible)
               {
                   strTmp = CsConst.CurtainModes[oCurtain._CurtainID];
                   tmpDevice.UpdateHdlDeviceFrmBasicInformation(bChnId, oCurtain._CurtainID.ToString());
               }
               else if (myIRCodes != null && myIRCodes.Visible)
               {
                   strTmp = myIRCodes.Text;
               }
               gridTmpTable[7, e.RowIndex].Value = strTmp;

               if (oTime != null && oTime.Visible)
               {
                   gridTmpTable[e.ColumnIndex, e.RowIndex].Value = HDLPF.GetStringFromTime(int.Parse(oTime.Text.ToString()), ":");
               }
           }
           catch
           { }
        }

       private void LightLists_CurrentCellDirtyStateChanged(object sender, EventArgs e)
       {
           LightLists.CommitEdit(DataGridViewDataErrorContexts.Commit);
       }

       private void tabSim_SelectedIndexChanged(object sender, EventArgs e)
       {
           try
           {
               TabControl Tmp = ((TabControl)sender);
               TabPage tabTmpPage = Tmp.SelectedTab;
               if (tabTmpPage.Tag == null) return;
               Byte ButtonTag = Convert.ToByte(tabTmpPage.Tag.ToString());
               currentSelectedBigType = ButtonTag;
               try
               {
                   RowMergeView gridTmpTable = LightLists;
                   DisplaySimpleFunctionListToDataGridview(gridTmpTable, ButtonTag);
                   gridTmpTable.MergeColumnNames = new List<string>();
                   gridTmpTable.MergeColumnNames.Add("smallType" + ButtonTag.ToString());
                   gridTmpTable.MergeColumnNames.Add("Location" + ButtonTag.ToString());
                   gridTmpTable.MergeColumnNames.Add("DeviceName" + ButtonTag.ToString());
               }
               catch
               { }
           }
           catch
           { }
       }

       private void btnSaveFunctionList_Click(object sender, EventArgs e)
       {
           bReSearchSimpleList = true;
           Cursor.Current = Cursors.WaitCursor;
           try
           {
               RowMergeView gridTmpTable = LightLists;
               SelectedChnsSimpleFunctionListToDataGridview(gridTmpTable, bSelectedSubNetId, bSelectedDeviceId);
           }
           catch
           { }
           Cursor.Current = Cursors.Default;
      
       }

       private void button1_Click(object sender, EventArgs e)
       {
           
       }


       public void ReadDeviceRemarkDisplaySimpleFunctionListToDataGridview(RowMergeView dgOnline, Byte bSelectedSubNetId, Byte bSelectedDeviceId)
       {
           if (CsConst.simpleSearchDevicesList == null || CsConst.simpleSearchDevicesList.Count == 0) return;
           if (dgOnline == null) return;
           RowMergeView gridTmpTable = dgOnline;
           try
           {
               for (int i = 0; i < gridTmpTable.RowCount; i++)
               {
                   int intLoad = currentSelectedBigType;
                   Byte bChnId = Convert.ToByte(gridTmpTable[3, i].Value.ToString());
                   String deviceName = gridTmpTable[2, i].Value.ToString();
                   if (deviceName == "" || deviceName == null) return;                  

                   foreach (HdlDevice temp in CsConst.simpleSearchDevicesList)
                   {
                       if (temp.subnetID == bSelectedSubNetId && temp.deviceID == bSelectedDeviceId)
                       {
                           Byte[] ArayTmp = new Byte[] { bChnId };
                           if (temp.bigType == 1) // light
                           {
                               #region
                               HdlSimpleLights oTmp = (HdlSimpleLights)(temp);
                               if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xF00E, temp.subnetID, temp.deviceID, false, true, true,
                                   CsConst.minAllWirelessDeviceType.Contains(temp.DeviceType)) == true)
                               {
                                   if (CsConst.myRevBuf != null)
                                   {
                                       byte[] arayRemark = new byte[20];
                                       Array.Copy(CsConst.myRevBuf, 26, arayRemark, 0, 20);
                                       oTmp.sumLightChns[bChnId - 1].remark = HDLPF.Byte2String(arayRemark);
                                       gridTmpTable[4, i].Value = oTmp.sumLightChns[bChnId - 1].remark;
                                       CsConst.myRevBuf = new byte[1200];
                                   }
                               }
                               #endregion
                           }
                           else if (temp.bigType == 2) // curtain
                           {
                               #region
                               HdlSimpleCurtains oTmp = (HdlSimpleCurtains)(temp);
                               int iOperationCode = 0xF00E;
                               int iTmpStart = 1;
                               if (CurtainDeviceType.NormalMotorCurtainDeviceType.Contains(temp.DeviceType) ||
                                   CurtainDeviceType.RollerCurtainDeviceType.Contains(temp.DeviceType)) // 电机 没有备注的读取
                               {
                                   iOperationCode = 0x000E;
                                   iTmpStart = 0;
                               }
                               else
                               {
                                   if (bChnId == 2 && CurtainDeviceType.NormalCurtainG1DeviceType.Contains(temp.DeviceType))// 普通窗帘读取第三回路备注
                                   {
                                       ArayTmp = new Byte[] { 3};
                                   }
                               }
                               if (CsConst.mySends.AddBufToSndList(ArayTmp, iOperationCode, temp.subnetID, temp.deviceID, false, true, true,
                                   CsConst.minAllWirelessDeviceType.Contains(temp.DeviceType)) == true)
                               {
                                   byte[] arayRemark = new byte[20];
                                   HDLSysPF.CopyRemarkBufferFrmMyRevBuffer(CsConst.myRevBuf, arayRemark, (Byte)(25 + iTmpStart));
                                   oTmp.sumCurtainChns[bChnId - 1].remark = HDLPF.Byte2String(arayRemark);
                                   gridTmpTable[4, i].Value = oTmp.sumCurtainChns[bChnId - 1].remark;
                                   CsConst.myRevBuf = new byte[1200];
                               }
                               #endregion
                           }
                           else if (temp.bigType == 4) // button or dry contact
                           {
                               #region
                               HdlSimpleButtons oTmp = (HdlSimpleButtons)(temp);
                               if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xE004, temp.subnetID, temp.deviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(temp.DeviceType)) == true)
                               {
                                   byte[] arayRemark = new byte[20];
                                   Array.Copy(CsConst.myRevBuf, 26, arayRemark, 0, 20);
                                   oTmp.sumButtonDrys[bChnId - 1].Remark = HDLPF.Byte2String(arayRemark);
                                   gridTmpTable[4, i].Value = oTmp.sumButtonDrys[bChnId - 1].Remark;
                                   CsConst.myRevBuf = new byte[1200];
                               }
                               #endregion
                           }
                           else if (temp.bigType == 5) // sensor 
                           {

                           }
                           break;
                       }
                   }
               }
           }
           catch { }
       }

       public void SelectedChnsSimpleFunctionListToDataGridview(RowMergeView dgOnline, Byte bSelectedSubNetId, Byte bSelectedDeviceId)
       {
           if (CsConst.simpleSearchDevicesList == null || CsConst.simpleSearchDevicesList.Count == 0) return;
           if (dgOnline == null) return;
           RowMergeView gridTmpTable = dgOnline;
           try
           {
               for (int i = 0; i < gridTmpTable.RowCount; i++)
               {
                   int intLoad = currentSelectedBigType;
                   Byte bChnId = Convert.ToByte(gridTmpTable[3, i].Value.ToString());
                   String deviceName = gridTmpTable[2, i].Value.ToString();
                   if (deviceName == "" || deviceName == null) return;
                   Byte bSelected = 0xF5;
                   String sRemark = "";
                   if (gridTmpTable[4, i].Value != null) sRemark = gridTmpTable[4, i].Value.ToString();
                   if (gridTmpTable[0, i].Value.ToString().ToLower() == "true")
                       bSelected = 0xF8;
                   foreach (HdlDevice temp in CsConst.simpleSearchDevicesList)
                   {
                       if (temp.subnetID == bSelectedSubNetId && temp.deviceID == bSelectedDeviceId)
                       {
                           if (temp.bigType == 1) // light
                           {
                               #region
                               HdlSimpleLights oTmp = (HdlSimpleLights)(temp);
                               if (oTmp.sumLightChns[bChnId -1].curStatus == null || oTmp.sumLightChns[bChnId -1].curStatus.Length < 2) 
                               {
                                   oTmp.sumLightChns[bChnId - 1].curStatus = new Byte[] { 0,bSelected};                                   
                               }
                               else
                               {
                                   oTmp.sumLightChns[bChnId - 1].curStatus[1] = bSelected;
                               }
                               oTmp.sumLightChns[bChnId - 1].remark = sRemark;
                               break;
                               #endregion
                           }
                           else if (temp.bigType == 2) // curtain
                           {
                               #region
                               HdlSimpleCurtains oTmp = (HdlSimpleCurtains)(temp);
                               if (oTmp.sumCurtainChns[bChnId - 1].curStatus == null || oTmp.sumCurtainChns[bChnId - 1].curStatus.Length < 2)
                               {
                                   oTmp.sumCurtainChns[bChnId - 1].curStatus = new Byte[] { 0, bSelected };
                               }
                               else
                               {
                                   oTmp.sumCurtainChns[bChnId - 1].curStatus[1] = bSelected;
                               }
                               oTmp.sumCurtainChns[bChnId - 1].remark = sRemark;
                               break;
                               #endregion
                           }
                           else if (temp.bigType == 4) // button or dry contact
                           {
                               #region
                               HdlSimpleButtons oTmp = (HdlSimpleButtons)(temp);
                               if (oTmp.sumButtonDrys[bChnId - 1].curStatus == null || oTmp.sumButtonDrys[bChnId - 1].curStatus.Length< 2)
                               {
                                   oTmp.sumButtonDrys[bChnId - 1].curStatus = new Byte[] { 0, bSelected };
                               }
                               else
                               {
                                   oTmp.sumButtonDrys[bChnId - 1].curStatus[1] = bSelected;
                               }
                               oTmp.sumButtonDrys[bChnId - 1].Remark = sRemark;
                               break;
                               #endregion
                           }
                           else if (temp.bigType == 5) // sensor 
                           {

                           }
                       }
                   }
               }
           }
           catch { }
       }

       private void btnReadRemark_Click(object sender, EventArgs e)
       {
           bReSearchSimpleList = true;
           Cursor.Current = Cursors.WaitCursor;
           try
           { 
               RowMergeView gridTmpTable = LightLists;
               ReadDeviceRemarkDisplaySimpleFunctionListToDataGridview(gridTmpTable, bSelectedSubNetId,bSelectedDeviceId);
           }
           catch
           { }
           Cursor.Current = Cursors.Default;
       }

       private void btnRebuild_Click(object sender, EventArgs e)
       {
           try
           {
               bReSearchSimpleList = false;
               if (CsConst.simpleSearchDevicesList != null && CsConst.simpleSearchDevicesList.Count != 0)
               {
                   FileStream fs = new FileStream(@"C:\casatuneZones.dat", FileMode.Create);
                   BinaryFormatter bf = new BinaryFormatter();
                   if (CsConst.simpleSearchDevicesList != null) bf.Serialize(fs, CsConst.simpleSearchDevicesList);
                   fs.Close();

                   //XmlHelper.XmlSerializeToFile(CsConst.simpleSearchDevicesList, @"C:\test.xml", Encoding.UTF8);
               }
           }
           catch
           { }
       }

       private void btnExport_Click(object sender, EventArgs e)
       {
           Boolean isOpen = HDLSysPF.IsAlreadyOpenedForm("frmTirdPart");

           if (isOpen == false)
           {
               frmTirdPart frmUpdate = new frmTirdPart();
               frmUpdate.Dock = DockStyle.Right;
               frmUpdate.Show();
           }
       }

       private void dgOnline_CellClick(object sender, DataGridViewCellEventArgs e)
       {
           if ((sender as DataGridView).SelectedCells == null) return;
           if (e.RowIndex < 0) return;
           string strTmp = (sender as DataGridView).SelectedRows[0].Cells[4].Value.ToString();
           if (strTmp == null || strTmp == "") return;
           Cursor.Current = Cursors.WaitCursor;

           try
           {
               bSelectedSubNetId = byte.Parse(dgOnline.SelectedCells[CsConst.MainFormSubNetIDStartFrom].Value.ToString());
               bSelectedDeviceId = byte.Parse(dgOnline.SelectedCells[CsConst.MainFormSubNetIDStartFrom + 1].Value.ToString());

               DisplayOneDevicesSimpleFunctionListToDataGridview(LightLists, bSelectedSubNetId, bSelectedDeviceId);
           }
           catch
           { }
       }

       private void btnBuildList_Click(object sender, EventArgs e)
       {
           RowMergeView gridTmpTable = LightLists;
           try
           {
               HdlUdpPublicFunctions.OldVersionDevicesGetSimpleListManually();
               DisplaySimpleFunctionListToDataGridview(gridTmpTable, (Byte)currentSelectedBigType);
               gridTmpTable.MergeColumnNames = new List<string>();
               gridTmpTable.MergeColumnNames.Add("smallType" + currentSelectedBigType.ToString());
               gridTmpTable.MergeColumnNames.Add("Location" + currentSelectedBigType.ToString());
               gridTmpTable.MergeColumnNames.Add("DeviceName" + currentSelectedBigType.ToString());
           }
           catch
           { }
       }

       private void btnAddScene_Click(object sender, EventArgs e)
       {
           try
           {
               Boolean isOpen = HDLSysPF.IsAlreadyOpenedForm("FrmBackup");

               if (isOpen == false)
               {
                   frmCmdTemplates frmBackups = new frmCmdTemplates(true);
                   frmBackups.ShowDialog();
               }
           }
           catch
           {

           }
       }

       private void LightLists_CellEndEdit(object sender, DataGridViewCellEventArgs e)
       {

       }

    }
}

