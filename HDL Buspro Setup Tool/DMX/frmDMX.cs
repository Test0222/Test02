using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace HDL_Buspro_Setup_Tool
{
    public partial class frmDMX : Form
    {
        [DllImport("user32.dll")]
        public static extern Boolean FlashWindow(IntPtr hwnd, Boolean bInvert);

        private Point Position = new Point(0,0);
        private DMX oDMX = null;
        private Byte SubnetID;
        private Byte DeviceID;
        private string myStrName = null;
        private int mintIDIndex = 0;
        private int mintDeviceType = 0;
        private int mintMaxvalue = 0; // 最大值
        private bool mblnIsDelete = false;
        private bool mblnIsShowing =false ;
        private byte mbytCurSqeID = 0; //当前选中的序列号
        private int myintRowIndex = -1; // 当前行列
        private int myintColIndex = -1;

        private int MyActivePage = 0; //按页面上传下载

        private SingleChn oChn = null; // 新增回路控件
        private RGB oRGB = null; // 新增加颜色选择块
        private RGBW oRGBW = null; //新增加彩色灯
        private CCT oCCT = null; //新增加CCT

        private List<Byte[]> bytAry = new List<byte[]>();

        public frmDMX()
        {
            InitializeComponent();
        }

        public frmDMX(DMX TmpDMX, string strName, int intDeviceType, int intIndex)
        {
            InitializeComponent();
            this.oDMX = TmpDMX;
            this.mintIDIndex = intIndex;
            this.myStrName = strName;

            string strDevName = strName.Split('\\')[0].ToString();
            SubnetID = Convert.ToByte(strDevName.Split('-')[0]);
            DeviceID = Convert.ToByte(strDevName.Split('-')[1]);

            this.mintDeviceType = intDeviceType;
            this.mintMaxvalue = DeviceTypeList.GetMaxValueFromPublicModeGroup(intDeviceType);
            
            tsbLabel3.Text = strName;

            HDLSysPF.DisplayDeviceNameModeDescription(strName, mintDeviceType, cboDevice, tbModel, tbDescription);
        }

        private int mintSeriesMode = 0;
        private int mintCurSeries = 0;
        private int mintCopySeriesID = -1;
        TimeText txtTime = new TimeText(":");
        TimeMs txtSeries = new TimeMs();
        List<int> mArayChnListInCurrentArea = new List<int>();

        private void tbName_TextChanged(object sender, EventArgs e)
        {
            if (tbName.Text == null) return;
        }

        private void btAdd_Click(object sender, EventArgs e)
        {
            if (tbName.Text == null) return;
            if (cbSteps.Text == "") return;
            try
            {
                int intTotal = int.Parse(cbSteps.Text.ToString());
                if (intTotal == 0) return;

                string strLEDName = tbName.Text.ToString();

                LEDLibray LEDtmp = null;

                if (CsConst.myLEDs != null) // 检测有没有相同的素材出现
                {
                    foreach (LEDLibray tmp in CsConst.myLEDs)
                    {
                        if (tmp.LEDName == strLEDName)
                        {
                            LEDtmp = new LEDLibray();
                            LEDtmp = tmp;
                        }
                    }
                }
                else
                {
                    CsConst.myLEDs = new List<LEDLibray>();
                }

                if (LEDtmp != null) // 判断当前素材库是不是存在
                {
                    string strTmp = string.Format("There has a LED with name {0} already, replace it or not", strLEDName);
                    if (MessageBox.Show(strTmp, "Confirm Information", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.OK)
                    {
                        LEDtmp.LEDName = strLEDName;
                        LEDtmp.intSumChns = intTotal;
                        LEDtmp.LEDChns = new List<string>();
                        for (int intI = 0; intI < intTotal; intI++) LEDtmp.LEDChns.Add("Chn. " + (intI + 1).ToString());
                    }
                }
                else   //不存在要添加进去
                {
                    LEDtmp = new LEDLibray();
                    LEDtmp.LEDName = strLEDName;
                    LEDtmp.intSumChns = intTotal;
                    LEDtmp.LEDChns = new List<string>();
                    for (int intI = 0; intI < intTotal; intI++) LEDtmp.LEDChns.Add("Chn. " + (intI + 1).ToString());

                    CsConst.myLEDs.Add(LEDtmp);

                    cbCurDev.Items.Add(tbName.Text.ToString());
                }

                dgLights.RowCount = LEDtmp.intSumChns;
                cbCurDev.Text = tbName.Text;
                for (int intI = 0; intI < intTotal; intI++)
                {
                    dgLights[0, intI].Value = (intI + 1).ToString();
                    dgLights[1, intI].Value = "Chn " + (intI + 1).ToString();
                }
            }
            catch
            { }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (cbCurDev.Text == null) return;

            string strLEDName = cbCurDev.Text.ToString();

            if (CsConst.myLEDs != null) // 检测有没有相同的素材出现
            {
                foreach (LEDLibray tmp in CsConst.myLEDs)
                {
                    if (tmp.LEDName == strLEDName)
                    {
                        CsConst.myLEDs.Remove(tmp);
                        dgLights.Rows.Clear();
                        cbCurDev.Items.Remove(strLEDName);
                        break;
                    }
                }
            }
        }

        void ShowLEDLibrayToListview()
        {
            tvLights.Nodes.Clear();
            cbCurDev.Items.Clear();

            if (CsConst.myLEDs == null) return;

            foreach (LEDLibray tmp in CsConst.myLEDs)
            {
                TreeNode OND = tvLights.Nodes.Add(tmp.LEDName, tmp.LEDName, 0,0);
                cbCurDev.Items.Add(tmp.LEDName);
                for (int intI = 0; intI < tmp.intSumChns; intI++) OND.Nodes.Add(tmp.LEDChns[intI], tmp.LEDChns[intI], 1,1);
            }
        }

        void ShowAreasToListview(object sender,bool BlnChns)
        {
            ((TreeView)sender).Nodes.Clear();

            if (oDMX == null) return;
            if (tc1.SelectedIndex == 1) ((TreeView)sender).Nodes.Add(null, "Ungrouped");
            if (oDMX.Areas512 == null) return;
            for (int I = 0; I < oDMX.Areas512.Count; I++)
            {
                TreeNode OND = ((TreeView)sender).Nodes.Add(null,oDMX.Areas512[I].AreaID + "-" +  oDMX.Areas512[I].strRemark, 0, 0);

                if (BlnChns == true)
                {
                    for (int intI = 0; intI < oDMX.LoadTypes.Count; intI++)
                    {
                        if (oDMX.LoadTypes[intI].intBelongs == oDMX.Areas512[I].AreaID)
                            OND.Nodes.Add(null, (intI + 1).ToString() + "--" +
                                          oDMX.LoadTypes[intI].strRemark, 1, 1);
                    }
                }
                else
                {
                    if (oDMX.Areas512[I].MySers !=null)
                    {
                        for (int intI = 0; intI < oDMX.Areas512[I].MySers.Count; intI++)
                        {
                            OND.Nodes.Add(null, oDMX.Areas512[I].MySers[intI].SeriID.ToString() + "-" +
                                                oDMX.Areas512[I].MySers[intI].Remark, 1, 1);
                        }
                    }

                    ((TreeView)sender).SelectedNode = ((TreeView)sender).TopNode;
                }
            }
        }

        void ShowSceneToListview(object sender)
        {
            ((TreeView)sender).Nodes.Clear();

            if (oDMX == null) return;
            if (oDMX.Areas512 == null) return;

            for (int I = 0; I < oDMX.Areas512.Count; I++)
            {
                TreeNode OND = ((TreeView)sender).Nodes.Add(null, oDMX.Areas512[I].AreaID + "-" + oDMX.Areas512[I].strRemark, 0, 0);

                if (oDMX.Areas512[I].MySces != null)
                {
                    for (int intI = 0; intI < oDMX.Areas512[I].MySces.Count ; intI++)
                    {
                        OND.Nodes.Add(null,oDMX.Areas512[I].MySces[intI].intSceID.ToString() + "--"+
                            oDMX.Areas512[I].MySces[intI].Remark, 1, 1);
                    }
                }
            }
        }


        void InitialFormCtrlsTextOrItems()
        {
            cbSeriesRepeat.Items.Add("Never stop");
            for (int intI = 0; intI < 99; intI++)
            {
                cbstep.Items.Add(intI + 1);
                cbSeriesRepeat.Items.Add(intI + 1);
            }

            panel5.Controls.Add(txtTime);
            txtTime.Text = "0:0";
            txtTime.Show();
            txtTime.Top = 525;
            txtTime.Left = 81;
            txtTime.Visible = true;

            #region
            DgChns.Columns[0].Width = (int)(DgChns.Width * 0.08);
            DgChns.Columns[1].Width = (int)(DgChns.Width * 0.12);
            DgChns.Columns[2].Width = (int)(DgChns.Width * 0.2);
            DgChns.Columns[3].Width = (int)(DgChns.Width * 0.12);
            DgChns.Columns[4].Width = (int)(DgChns.Width * 0.12);
            DgChns.Columns[5].Width = (int)(DgChns.Width * 0.12);
            DgChns.Columns[6].Width = (int)(DgChns.Width * 0.12);

            DgAreas.Columns[0].Width = (int)(DgAreas.Width * 0.1);
            DgAreas.Columns[1].Width = (int)(DgAreas.Width * 0.2);
            DgAreas.Columns[2].Width = (int)(DgAreas.Width * 0.32);
            DgAreas.Columns[3].Width = (int)(DgAreas.Width * 0.32);

            DgScene.Columns[0].Width = (int)(DgScene.Width * 0.16);
            DgScene.Columns[1].Width = (int)(DgScene.Width * 0.54);
            DgScene.Columns[2].Width = (int)(DgScene.Width * 0.24);
            #endregion

            tabControl1.SelectedTab = tpM;

            HDLSysPF.ReadLoadType();
            ShowLEDLibrayToListview();
        }


        private void frmMain_Load(object sender, EventArgs e)
        {
            InitialFormCtrlsTextOrItems();
        }

        void SetCtrlsVisbleWithDifferentDeviceType()
        {
            gbLCD.Visible = DMXDeviceTypeList.DMXHasPowerSavingFunction.Contains(mintDeviceType);
            panNetwork.Visible = CsConst.mintDevicesHasIPNetworkDevType.Contains(mintDeviceType); 
        }

        private void AddLoadTypeFromLibrary(TreeNode OND)
        {
            string strLoad = OND.Text.ToString();

            LEDLibray TmpLib = new LEDLibray();
            DMX.Chns Tmp = new DMX.Chns();

            TmpLib = CsConst.myLEDs[OND.Index];

            if (oDMX.LoadTypes == null)
            {
                oDMX.LoadTypes = new List<DMX.Chns>();
            }

            int intStartAdd = 1;
            string str="";
            string str1 = "";
            if (DgChns.RowCount > 0)
            {
                str = DgChns[2, DgChns.RowCount - 1].Value.ToString();
                str1 = DgChns[1, DgChns.RowCount - 1].Value.ToString();
            }
            if (str == "R")
                intStartAdd = (Convert.ToInt16(str1) + 1);
            if (str == "RGBDS")
                intStartAdd = (Convert.ToInt16(str1) + 5);
            if (str == "RGB")
                intStartAdd = (Convert.ToInt16(str1) + 3);
            if (str == "RGBAW")
                intStartAdd = (Convert.ToInt16(str1) + 5);
            if (str == "RGBW")
                intStartAdd = (Convert.ToInt16(str1) + 4);
            if (str == "RGBWSD")
                intStartAdd = (Convert.ToInt16(str1) + 6);

            if (intStartAdd > mintMaxvalue || intStartAdd + TmpLib.intSumChns - 1> mintMaxvalue) return;

            Tmp.intLibraryID = OND.Index;
            Tmp.strRemark = "DMX Light " + (oDMX.LoadTypes.Count + 1).ToString();
            Tmp.MinValue = 0;
            Tmp.MaxValue = 255;
            Tmp.MaxLevel = 255;
            Tmp.intStart = intStartAdd;
            Tmp.ArrtiChang = 1;
            Tmp.intBelongs = 0;
            oDMX.LoadTypes.Add(Tmp);

            object[] obj = { oDMX.LoadTypes.Count, intStartAdd.ToString(), strLoad, Tmp.strRemark,Tmp.MinValue,Tmp.MaxValue,Tmp.MaxLevel, AttriChange.Items[1] };
            DgChns.Rows.Add(obj);
            tc1_SelectedIndexChanged(null, null);
        }

        private void btnArea_Click(object sender, EventArgs e)
        {
            if (oDMX == null) return;
            if ((tbArea.Text == "") || (tbArea.Text == null)) return;

            DMX.Areas oTmpArea = new DMX.Areas();
           
            // 查看有没有选中的区域
            TreeNode OTN = null;
            bool BlnNewAdd = false;

            int intAreaID = 1;

            foreach (TreeNode oNode in tvAreas.Nodes)
            {
                if (oNode != tvAreas.Nodes[0])
                {
                    if (oNode.Text.Split('-')[1].ToString() == tbArea.Text)
                    {
                        {
                            OTN = oNode;
                            intAreaID = int.Parse(oNode.Text.Split('-')[0].ToString());
                            break;
                        }
                    }
                }
            }

            if (OTN == null) 
            {
                BlnNewAdd = true;
                intAreaID = GetNewIDForArea();
                OTN = tvAreas.Nodes.Add(intAreaID + "-"+ tbArea.Text);
            }

            if (BlnNewAdd == true)
            {
                oTmpArea.strRemark = tbArea.Text;
                oTmpArea.AreaID = intAreaID;

                if (oDMX.Areas512 == null)
                {
                    oDMX.Areas512 = new List<DMX.Areas>();
                }
                oDMX.Areas512.Add(oTmpArea);
            }
            oDMX.MyRead2UpFlags[5] = false;
            oDMX.MyRead2UpFlags[6] = false;
            oDMX.MyRead2UpFlags[2] = false;
            oDMX.MyRead2UpFlags[3] = false;
        }

        int GetNewIDForArea()
        {
            // 添加到缓存
            if (oDMX.Areas512 == null)
            {
                oDMX.Areas512 = new List<DMX.Areas>();
                return 1;
            }

            List<int> oTmp = new List<int>(); //取出所有已用的场景号
            for (int i = 0; i < oDMX.Areas512.Count; i++)
            {
                oTmp.Add(oDMX.Areas512[i].AreaID);
            }
            //查找位置，替换buffer
            int intAreaID = 1;
            while (oTmp.Contains(intAreaID))
            {
                intAreaID++;
            }

            return intAreaID;
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowLEDLibrayToListview();
        }

        void UpdateDisplayInformationAccordingly(object sender, FormClosingEventArgs e)
        {
            DisplayTabAccordingly();
            Cursor.Current = Cursors.Default;
        }

        private void tc1_SelectedIndexChanged(object sender, EventArgs e)
        {
            mblnIsShowing = true;
            try
            {
                if (CsConst.MyEditMode == 1)
                {
                    if (oDMX.MyRead2UpFlags[tc1.SelectedIndex] == false && tc1.SelectedIndex !=4)
                    {
                        MyActivePage = tc1.SelectedIndex + 1;
                        tslRead_Click(tslRead, null);
                        MyActivePage = 0;
                    }
                    else
                    {
                        DisplayTabAccordingly();
                    }
                }
            }
            catch { mblnIsShowing = false; }
            mblnIsShowing = false; 
        }

        void DisplayTabAccordingly()
        {
            if (tc1.SelectedIndex == 0) // 逻辑灯界面
            {
                ShowChannelsInformation();
            }
            else if (tc1.SelectedIndex == 1)  // 区域设置
            {
                #region
                if (oDMX == null) return;
                ShowAreasToListview(tvAreas, true);
                DgAreas.Rows.Clear();
                if (oDMX.LoadTypes == null) return;
                for (int i = 0; i < oDMX.LoadTypes.Count; i++)
                {
                    int intTmp = oDMX.LoadTypes[i].intLibraryID;
                    if (intTmp < CsConst.myLEDs.Count)
                    {
                        object[] obj = { i+1,oDMX.LoadTypes[i].intStart,CsConst.myLEDs[intTmp].LEDName,
                                         oDMX.LoadTypes[i].strRemark };
                        DgAreas.Rows.Add(obj);


                        if (oDMX.LoadTypes[i].intBelongs == 0)
                        {
                            tvAreas.Nodes[0].Nodes.Add(null, (i + 1).ToString() + "--" + oDMX.LoadTypes[i].strRemark, 1, 1);
                        }
                    }
                }
                #endregion
            }
            else if (tc1.SelectedIndex == 2)  // 场景设置
            {
                #region
                tvArea2.Nodes.Clear();
                ShowSceneToListview(TvScene);
                panel6.Controls.Clear();
                #endregion
            }
            else if (tc1.SelectedIndex == 3)  // 序列设置
            {//显示区域
                #region
                ShowAreasToListview(tvSeries, false);
                if (oDMX.Areas512 == null) return;
                if (oDMX.Areas512.Count == 0) return;
                if (oDMX.Areas512 != null && oDMX.Areas512[0].MySces != null)
                {
                    foreach (DMX.SceType oSce in oDMX.Areas512[0].MySces)
                    {
                        SelectScens.Items.Add(oSce.intSceID + "-" + oSce.Remark);
                    }
                }
                #endregion
            }
            else if (tc1.SelectedIndex == 4) // 逻辑灯素材库配置
            {
                #region
                ShowChannelsInformationInLogicTable();
                #endregion
            }
        }

        void ShowChannelsInformationInLogicTable()
        {
            girdLogicLight.Rows.Clear();
            try
            {
                if (oDMX != null)
                {
                    if (oDMX.LoadTypes != null)
                    {
                        int I = 1;
                        foreach (DMX.Chns Tmp in oDMX.LoadTypes)
                        {
                            object[] obj = null;
                            if (CsConst.myLEDs.Count == 0 || CsConst.myLEDs.Count < Tmp.intLibraryID)
                            {
                                obj = new object[] { I, Tmp.intStart, "", Tmp.strRemark};
                            }
                            else
                            {
                                obj = new object[] { I, Tmp.intStart, CsConst.myLEDs[Tmp.intLibraryID].LEDName, Tmp.strRemark };
                            }
                            girdLogicLight.Rows.Add(obj);
                            I++;
                        }
                    }
                }
            }
            catch { }
        }

        private void tvArea2_MouseDown(object sender, MouseEventArgs e)
        {
            TreeNode Tmp = tvArea2.GetNodeAt(e.Location);

            if (Tmp == null) return;
            try
            {
                if (Tmp.StateImageIndex != 3)
                {
                    Tmp.StateImageIndex = 3;
                    //右边显示对应的回路和备注
                }
                else
                {
                    Tmp.StateImageIndex = 2;
                }

                ShowDevicesChns(Tmp);

                tvArea2.Refresh();
            }
            catch { }
        }

        private void ShowDevicesChns(TreeNode Tmp)
        {
            if (Tmp == null) return;
            if (CsConst.myLEDs == null) return;
            this.panel6.Controls.Clear();
            try
            {
                int intLoadID = int.Parse(Tmp.Text.Split('-')[0].ToString()); //第几个负载

                int intTmp = oDMX.LoadTypes[intLoadID - 1].intLibraryID;
                LEDLibray TmpLoad = CsConst.myLEDs[intTmp];

                for (int intI = 0; intI < TmpLoad.intSumChns; intI++)
                {
                    // 计算待添加设备的位置
                    int localY = (intI / 9) * 180 + 10;
                    int localX = (intI % 9) * 60 + 8;
                    LogicChannel devPic = new LogicChannel(TmpLoad.LEDChns[intI], intI);
                    devPic.Location = new Point(localX, localY);
                    devPic.Tag = intI;
                    devPic.Name = "Dev" + (intI + 1).ToString();
                    devPic.Light = CsConst.ArayLevel[oDMX.LoadTypes[intLoadID - 1].intStart - 1 + intI];
                    devPic.LightValueChanged += new EventHandler(UpdateSceneLevelBuffer);
                    devPic.BringToFront();
                    this.panel6.Controls.Add(devPic);
                }
            }
            catch { }
        }

        void ch_LightValueChanged(object sender, EventArgs e)
        {
            if (CsConst.myLEDs == null) return;
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                byte[] bytAry = new byte[4];
                bytAry[0] = 0;
                bytAry[2] = 0;
                bytAry[3] = 0;
                bytAry[1] = 100;
                Cursor.Current = Cursors.Default;
            }
            catch
            {
                MessageBox.Show("DevInfos_CellEndEdit error");
            }
        }

        private void tbSubID_TextChanged(object sender, EventArgs e)
        {
            if (oDMX == null)
            {
                oDMX = new DMX();
            }

            string strTmp = ((TextBox)sender).Text.ToString();
            byte bytTag = byte.Parse(((TextBox)sender).Tag.ToString());
            byte bytTmp = 0;
            switch (bytTag)
            {
                case 0:
                    break;
                case 1:
                    break;
                case 2:
                    oDMX.Remark = strTmp;
                    break;
                case 3:
                    if (strTmp == null)
                    {
                        strTmp = "0";
                        ((TextBox)sender).Text = strTmp;
                    }

                    if (byte.TryParse(strTmp, out bytTmp))
                    {
                        oDMX.bytDMXChnID = bytTmp;
                    }
                    else
                    {
                        ((TextBox)sender).Text = oDMX.bytDMXChnID.ToString();
                    }
                    break;
                case 4:
                    if (strTmp == null)
                    {
                        strTmp = "4000";
                        ((TextBox)sender).Text = strTmp;
                    }
                    oDMX.intDMXPort = int.Parse(strTmp);

                    if (oDMX.intDMXPort > 4000)
                    {
                        oDMX.intDMXPort = 4000;
                        ((TextBox)sender).Text = "4000";
                    }
                    break;
                     
            }
        }

        private void txtSwiIP_TextChanged(object sender, EventArgs e)
        {
            if (oDMX == null)
            {
                oDMX = new DMX ();
            }

            string strTmp = ((MaskedTextBox)sender).Text.ToString();
            byte bytTag = byte.Parse(((MaskedTextBox)sender).Tag.ToString());

            if (strTmp == null)
            {
                strTmp = "192.168.10.250";
                ((MaskedTextBox)sender).Text = strTmp;
            }
            
            switch (bytTag)
            {
                case 0:
                    oDMX.strIP = strTmp;
                    break;
                case 1:
                    oDMX.strRouterIP = strTmp;
                    break;
                case 2:
                    oDMX.strMAC = strTmp;
                    break;
            }
        }

        private void dgLights_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == -1) return;
            if (e.RowIndex == -1) return;
            if (CsConst.myLEDs == null) return;
            if (cbCurDev.Text == null) return;

            string strLEDName = cbCurDev.Text.ToString();

            foreach (LEDLibray tmp in CsConst.myLEDs)
            {
                if (tmp.LEDName == strLEDName)
                {
                    tmp.LEDChns[e.RowIndex] = dgLights[1, e.RowIndex].Value.ToString();
                    break;
                }
            }
        }

        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            string strsql = "delete * from tmpDMX";
            DataModule.ExecuteSQLDatabase(strsql);

            strsql = "delete * from tmpArea";
            DataModule.ExecuteSQLDatabase(strsql);
            strsql = "delete * from tmpScene";
            DataModule.ExecuteSQLDatabase(strsql);

            strsql = "delete * from tmpSeries";
            DataModule.ExecuteSQLDatabase(strsql);

            //保存数据  1， 保存素材库
            HDLSysPF.SaveLoadType();
            //1，保存基本信息
            oDMX.SaveChannelsInformation();

            oDMX.SaveDMXInformation();
           
            //2,保存区域信息
            oDMX.SaveAreasInformation();
            Cursor.Current = Cursors.Default;
        }

        private void openToolStripButton_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            HDLSysPF.ReadLoadType();
            ShowLEDLibrayToListview();
            ShowChannelsInformation();

            if (oDMX == null)
            {
                oDMX = new DMX();
                oDMX.strIP = txtSwiIP.Text.ToString();
                oDMX.strRouterIP = txtSwRoutIP.Text.ToString();
                oDMX.strMAC = mtbMAC.Text.ToString();
                oDMX.bytDMXChnID = byte.Parse(tbDMXID.Text.ToString());
                oDMX.intDMXPort = int.Parse(tbPort.Text.ToString());
            }
            else
            {
                txtSwiIP.Text = oDMX.strIP ;
                txtSwRoutIP.Text = oDMX.strRouterIP;
                mtbMAC.Text = oDMX.strMAC ;
                tbDMXID.Text = oDMX.bytDMXChnID.ToString();
                tbPort.Text = oDMX.intDMXPort.ToString();
            }

            Cursor.Current = Cursors.Default;
        }

        void ShowChannelsInformation()
        {
            DgChns.Rows.Clear();
            try
            {
                #region
                if (oDMX != null)
                {
                    if (oDMX.LoadTypes != null)
                    {
                        int I = 1;
                        foreach (DMX.Chns Tmp in oDMX.LoadTypes)
                        {
                            object[] obj = null;
                            if (CsConst.myLEDs.Count == 0 || CsConst.myLEDs.Count < Tmp.intLibraryID)
                            {
                                obj = new object[] { I, Tmp.intStart, "", Tmp.strRemark, Tmp.MinValue, Tmp.MaxValue, Tmp.MaxLevel, AttriChange.Items[Tmp.ArrtiChang] };
                            }
                            else
                            {
                                obj = new object[] { I, Tmp.intStart, CsConst.myLEDs[Tmp.intLibraryID].LEDName, Tmp.strRemark, Tmp.MinValue, Tmp.MaxValue, Tmp.MaxLevel, AttriChange.Items[Tmp.ArrtiChang] };
                            }
                            DgChns.Rows.Add(obj);
                            I++;
                        }
                    }
                }
                #endregion
            }
            catch 
            { }
        }

        void UpdateSceneLevelBuffer(object sender, EventArgs e)
        {
            int intStart = 0;
            TreeNode Tmp = TvScene.SelectedNode;
            if (Tmp == null) return;
            if (Tmp.Level != 0) Tmp = Tmp.Parent;
            try
            {
                if (rbSame.Checked == true)
                {
                    foreach (LogicChannel TmpCh in panel6.Controls)
                    {
                        TmpCh.Text = ((LogicChannel)sender).Light.ToString();
                    }
                }

                foreach (TreeNode TmpNode in tvArea2.Nodes)
                {
                    if (TmpNode.StateImageIndex == 3)
                    {
                        int intLoadID = int.Parse(TmpNode.Text.Split('-')[0].ToString());
                        intStart = oDMX.LoadTypes[intLoadID - 1].intStart;
                        int intTmp = int.Parse(((LogicChannel)sender).Tag.ToString());

                        int intLibraryID = oDMX.LoadTypes[intLoadID - 1].intLibraryID;

                        if (intTmp <= CsConst.myLEDs[intLibraryID].intSumChns) //下标超界，直接跳过，向下兼容
                        {
                            CsConst.ArayLevel[intStart - 1 + intTmp] = byte.Parse(((LogicChannel)sender).Light.ToString());
                        }
                    }
                }
            }
            catch { }
        }

        private void tvAreas_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            if (e.Node == null) return;
            if (e.Label == null) return;
            if (e.Node.Level != 0) return;
            if (oDMX.Areas512 == null) return;

            int intLevel = e.Node.Index;
            if (e.Label.Split('-')[1] == null)
            {
                MessageBox.Show("The format should be area id and its name.");
                return;
            }
            oDMX.Areas512[intLevel].strRemark = e.Label.Split('-')[1].ToString();
           // e.Label = oDMX.Areas512[intLevel].AreaID + e.Label.ToString();
        }

        private void btnDelSce_Click(object sender, EventArgs e)
        {
            if (tbRed.RowCount == 0) return;
            if (tbRed.SelectedRows.Count == 0) return;
            if (TvScene.SelectedNode == null) return;
            if (oDMX.Areas512 == null) return;

            txtSeries.Visible = false;

            TreeNode oNode = TvScene.SelectedNode;
            if (oNode.Level != 0)
            {
                oNode = oNode.Parent;
            }
            if (oDMX.Areas512[oNode.Level].MySces == null) return;

            foreach (DataGridViewRow r in tbRed.SelectedRows)
            {
                if (r.Selected)
                {
                    oDMX.Areas512[TvScene.SelectedNode.Level].MySces.RemoveAt(r.Index);
                    oNode.Nodes.RemoveAt(r.Index);
                    tbRed.Rows.Remove(r);
                }
            }
            oDMX.MyRead2UpFlags[6] = false;
            oDMX.MyRead2UpFlags[2] = false;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            mblnIsShowing = true;
            if (tbRed.RowCount == 0) return;
            if (tbRed.SelectedRows.Count == 0) return;
            if (TvScene.SelectedNode == null) return;
            if (oDMX.Areas512 == null) return;
            TreeNode oNode = TvScene.SelectedNode;
            if (oNode.Level != 0)
            {
                oNode = oNode.Parent;
            }
            if (oDMX.Areas512[oNode.Level].MySces == null) return;
            try
            {
                foreach (DataGridViewRow r in tbRed.SelectedRows)
                {
                    if (r.Selected)
                    {
                        if (r.Cells[1].Value == null) r.Cells[1].Value = "";
                        if (r.Cells[2].Value == null) r.Cells[2].Value = "0:0";
                        oDMX.Areas512[oNode.Level].MySces[r.Index].Remark = r.Cells[1].Value.ToString();
                        if (mintDeviceType == 20 || mintDeviceType == 21 || mintDeviceType == 24)
                        {
                            oDMX.Areas512[oNode.Level].MySces[r.Index].intRunTime = int.Parse(HDLPF.GetTimeFrmStringMs(r.Cells[2].Value.ToString()));
                        }
                        else
                        {
                            oDMX.Areas512[oNode.Level].MySces[r.Index].intRunTime = int.Parse(HDLPF.GetTimeFromString(r.Cells[2].Value.ToString(), ':'));
                        }
                        oDMX.Areas512[oNode.Level].MySces[r.Index].ArayLevel = (byte[])CsConst.ArayLevel.Clone();

                        for (int i = 0; i < mArayChnListInCurrentArea.Count; i++)
                        {
                            tbRed[3 + i, r.Index].Value = CsConst.ArayLevel[mArayChnListInCurrentArea[i] - 1];
                        }
                    }
                }
            }
            catch { }
            oDMX.MyRead2UpFlags[6] = false;
            oDMX.MyRead2UpFlags[3] = false;
            mblnIsShowing = false;
        }


        private void btnAddSeires_Click(object sender, EventArgs e)
        {
            if (oDMX == null) return;
            if (oDMX.Areas512 == null) return;
            if (tbRemarkSeries.Text == "") return;
            if (cbstep.Text == "") return;
            if (cbSeriesRepeat.Text == "") return;
            if (tvSeries.SelectedNode == null) return;

            TreeNode oNode = tvSeries.SelectedNode;
            if (oNode.Level != 0) { oNode = oNode.Parent; }

            if (oDMX.Areas512[oNode.Index].MySces == null) return;
            if (oDMX.Areas512[oNode.Index].MySces.Count == 0) return;

            AddNewSeriesToForm(0,oNode.Index);
        }

        int AddNewSeriesToForm(byte bytID,int intAreaID)  // default is 0, paste is 1
        {
            // 获取一个未使用的序列号
            if (oDMX.Areas512[intAreaID].MySers == null)
            {
                oDMX.Areas512[intAreaID].MySers = new List<DMX.SeriesType>();
            }

            List<int> TmpSerID = new List<int>(); //取出所有已用的场景号
            for (int i = 0; i < oDMX.Areas512[intAreaID].MySers.Count; i++)
            {
                TmpSerID.Add(oDMX.Areas512[intAreaID].MySers[i].SeriID);
            }
            //查找位置，替换buffer
            int intSceID = 1;
            while (TmpSerID.Contains(intSceID))
            {
                intSceID++;
            }

            DMX.SeriesType oSeries = new DMX.SeriesType();

            if (bytID == 0)  //默认添加
            {

                int intNeedAdd = cbstep.SelectedIndex + 1;
                if (DgScene.RowCount != 0)
                {
                    intNeedAdd = cbstep.SelectedIndex + 1 - DgScene.RowCount;
                }

                for (int intI = 0; intI < intNeedAdd; intI++)
                {
                    string strRemark = oDMX.Areas512[intAreaID].MySces[0].Remark;
                    object[] obj = { DgScene.RowCount + 1, SelectScens.Items[0], "0:0.0" };
                    DgScene.Rows.Add(obj);
                }

                //更新到缓存
                oSeries.SeriID = intSceID;
                oSeries.Remark = tbRemarkSeries.Text;
                oSeries.intStep = cbstep.SelectedIndex + 1;
                oSeries.intTimes = cbSeriesRepeat.SelectedIndex;
                oSeries.intRunMode = mintSeriesMode;

                oSeries.SceneIDs = new List<byte>();
                oSeries.RunTimeH = new List<byte>();
                oSeries.RunTimeL = new List<byte>();

                for (int intI = 0; intI < oSeries.intStep; intI++)
                {
                    oSeries.SceneIDs.Add(0);
                    oSeries.RunTimeH.Add(0);
                    oSeries.RunTimeL.Add(0);
                }
            }
            else if (bytID == 1)  // 复制的其他序列的设置
            {
                //更新到缓存
                oSeries =(DMX.SeriesType)oDMX.Areas512[intAreaID].MySers[mintCopySeriesID].Clone();
                oSeries.SeriID = intSceID;
            }

            //添加到窗体
            TreeNode oTmp = tvSeries.Nodes[intAreaID];
            oTmp = oTmp.Nodes.Add(intSceID.ToString() + "-" + oSeries.Remark);
            tvSeries.SelectedNode = oTmp;

            if (oDMX.Areas512[intAreaID].MySers == null)
            {
                oDMX.Areas512[intAreaID].MySers = new List<DMX.SeriesType>();
            }
            oDMX.Areas512[intAreaID].MySers.Add(oSeries);

            mintCurSeries = oDMX.Areas512[intAreaID].MySers.Count - 1;

            return intSceID;
        }

        private void DgScene_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void DgScene_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (e.ColumnIndex == 2)
            {
                ((DataGridView)sender).Controls.Add(txtSeries);
                txtSeries.Show();
                txtSeries.Visible = true;
                Rectangle rect = ((DataGridView)sender).GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true);
                txtSeries.Size = rect.Size;
                txtSeries.Top = rect.Top;
                txtSeries.Left = rect.Left;

                txtSeries.Text = HDLPF.GetTimeFrmStringMs(((DataGridView)sender)[2, e.RowIndex].Value.ToString());
                txtSeries.TextChanged += new EventHandler(txtSeries_TextChanged);
            }
            else
            {
                txtSeries.Visible = false;
            }
        }

        void txtSeries_TextChanged(object sender, EventArgs e)
        {
            if ((DgScene.CurrentCell.RowIndex == -1) || (DgScene.CurrentCell.ColumnIndex == -1)) return;
            if (txtSeries.Visible)
            {
                DgScene[2, DgScene.CurrentCell.RowIndex].Value = HDLPF.GetStringFrmTimeMs(int.Parse(txtSeries.Text.ToString()));
                // DgMode_CellValueChanged(DgMode, e);
            }
            //throw new NotImplementedException();
        }

        private void TvScene_MouseDown(object sender, MouseEventArgs e)
        {
            TreeNode oNode = TvScene.GetNodeAt(e.Location);
            if (oNode == null) return;
            if (oDMX.Areas512 == null) return;
            txtSeries.Visible = false;
            mblnIsShowing = true;
            if (oNode.Level == 0)
            {
                CsConst.ArayLevel = new byte[mintMaxvalue];
                tvArea2.Nodes.Clear();
                for (int intI = 0; intI < oDMX.LoadTypes.Count; intI++)
                {
                    if (oDMX.LoadTypes[intI].intBelongs == oDMX.Areas512[oNode.Index].AreaID)
                    {
                        tvArea2.Nodes.Add(null, (intI + 1).ToString() + "--" +
                                      oDMX.LoadTypes[intI].strRemark, 1, 1);
                    }
                }
                DisplayAllChannelsValues(oNode);
            }
            else
            {
                if (mintMaxvalue != oNode.Parent.Index)
                {
                    DisplayAllChannelsValues(oNode.Parent);
                }
                for (int intI = 0; intI < tbRed.RowCount; intI++) tbRed.Rows[intI].Selected = (intI == oNode.Index);
            }
            mblnIsShowing = false;
        }

        void DisplayAllChannelsValues(TreeNode oNode)
        {
            tbRed.Rows.Clear();
            tbRed.ColumnCount =0;
            if (oDMX.Areas512 == null) return;
            if (oDMX.Areas512[oNode.Index] == null) return;
            if (oDMX.LoadTypes == null) return;
            mblnIsShowing = true;
            //显示所有列的信息 运行时间和回路数
            tbRed.Columns.Add("Clm0", "ID");
            tbRed.Columns[0].ReadOnly = true;
            tbRed.Columns.Add("Clm1", "Remark");
            tbRed.Columns.Add("Clm2", "Run Time(mm:ss)");

            mArayChnListInCurrentArea = new List<int>();
            foreach (DMX.Chns oChn in oDMX.LoadTypes)
            {
                if (oChn.intBelongs == oDMX.Areas512[oNode.Index].AreaID)
                {
                    for (int I = 0; I < CsConst.myLEDs[oChn.intLibraryID].intSumChns; I++)
                    {
                        tbRed.Columns.Add("Clm" + (I+3).ToString(), oChn.strRemark + "-"+ (I+1).ToString());
                        mArayChnListInCurrentArea.Add(oChn.intStart + I);
                    }
                }
            }

            if (oDMX.Areas512[oNode.Index].MySces != null)
            {
                if (oDMX.Areas512[oNode.Index].MySces.Count != 0)
                {
                    tbRed.RowCount = oDMX.Areas512[oNode.Index].MySces.Count;
                }
            }

            if (tbRed.RowCount != 0)
            {
                int intRowIndex = 0;
                foreach (DMX.SceType oSce in oDMX.Areas512[oNode.Index].MySces)
                {
                    tbRed[0, intRowIndex].Value = oSce.intSceID;
                    tbRed[1, intRowIndex].Value = oSce.Remark;
                    
                    if (mintDeviceType == 20 || mintDeviceType == 21 || mintDeviceType == 24)
                    {
                        tbRed[2, intRowIndex].Value = HDLPF.GetStringFrmTimeMs(oSce.intRunTime);
                    }
                    else
                    {
                        tbRed[2, intRowIndex].Value = (oSce.intRunTime / 60).ToString() + ":" + (oSce.intRunTime % 60).ToString();
                    }

                    if (oSce.ArayLevel == null || oSce.ArayLevel.Length == 0)
                    {
                        mblnIsShowing = false;
                        return;
                    }
                    for (int i = 0; i < mArayChnListInCurrentArea.Count; i++) { tbRed[3 + i, intRowIndex].Value = oSce.ArayLevel[mArayChnListInCurrentArea[i] - 1]; }
                    intRowIndex++;
                }
            }
            mblnIsShowing = false;
        }

        private void tvSeries_MouseDown(object sender, MouseEventArgs e)
        {
            mblnIsDelete = false;
            DgScene.RowCount = 0;
            txtSeries.Visible = false;
            TreeNode oNode = tvSeries.GetNodeAt(e.Location);
            if (oNode == null) return;
            if (tvSeries.Nodes == null) return;
            if (oDMX.Areas512 == null) return;

            bool blnIsNewSqe = false;
            #region
            DMX.Areas oArea = null;
            if (oNode.Level == 0)
            {
                blnIsNewSqe = true;
                mbytCurSqeID = byte.Parse(oNode.Text.Split('-')[0].ToString());
                oArea = oDMX.Areas512[oNode.Index];
            }
            else
            {
                byte bytCurSqeID = byte.Parse(oNode.Parent.Text.Split('-')[0].ToString());
                if (bytCurSqeID != mbytCurSqeID)
                {
                    blnIsNewSqe = true;
                    mbytCurSqeID = byte.Parse(oNode.Parent.Text.Split('-')[0].ToString());
                }
                oArea = oDMX.Areas512[oNode.Parent.Index];
            }

            if (oArea == null) return;
            #endregion

            if (blnIsNewSqe && oArea.MySces != null) // 选择的区域
            {
                SelectScens.Items.Clear();
                if (oArea.MySces != null)
                {
                    foreach (DMX.SceType oSce in oArea.MySces)
                    {
                        SelectScens.Items.Add(oSce.intSceID + "-" + oSce.Remark);
                    }
                }
            }

            if (oNode.Level != 0)
            {
                if (oDMX.Areas512 == null) return;

                TreeNode Tmp = oNode.Parent;
                if (oDMX.Areas512[Tmp.Index].MySces == null) return;
                if (oDMX.Areas512[Tmp.Index].MySces.Count == 0) return;

                int intLevel = oNode.Index;

                mintCurSeries = intLevel;

                tbRemarkSeries.Text = oNode.Text.Split('-')[1].ToString();

                DMX.SeriesType oSeries = oDMX.Areas512[Tmp.Index].MySers[intLevel];
                cbstep.Text = oSeries.intStep.ToString();
                cbSeriesRepeat.SelectedIndex = oSeries.intTimes;

                switch (oSeries.intRunMode)
                {
                    case 3: rb1.Checked = true;
                        break;
                    case 2: rb2.Checked = true;
                        break;
                    case 1: rb3.Checked = true;
                        break;
                    case 0: rb4.Checked = true;
                        break;
                }
                ShowSequenceStepsToForm(oArea, oSeries);
            }
            mblnIsDelete = true;
        }

        private void ShowSequenceStepsToForm(DMX.Areas oArea, DMX.SeriesType oSeries)
        {
            if (oSeries == null) return;
            DgScene.Rows.Clear();
            for (int intI = 0; intI < oSeries.intStep; intI++)
            {
                bool blnAdded = false;
                for (int intJ = 0; intJ < oArea.MySces.Count; intJ++)
                {
                    if (oArea.MySces[intJ].intSceID == oSeries.SceneIDs[intI]) //选择场景显示
                    {
                        string strRemark = oArea.MySces[intJ].Remark;
                        object[] obj = { intI + 1, oSeries.SceneIDs[intI] + "-" + strRemark,
                                     HDLPF.GetStringFrmTimeMs(oSeries.RunTimeH[intI] * 256 + oSeries.RunTimeL[intI])};
                        DgScene.Rows.Add(obj);
                        blnAdded = true;
                        break;
                    }
                }
                if (blnAdded == false)
                {
                    object[] obj = { intI + 1, oArea.MySces[0].intSceID+"-"+oArea.MySces[0].Remark,
                                     HDLPF.GetStringFrmTimeMs(oSeries.RunTimeH[intI] * 256 + oSeries.RunTimeL[intI])};
                    DgScene.Rows.Add(obj);
                }

            }
        }

        private void rb1_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked == true)
            {
                mintSeriesMode = int.Parse(((RadioButton)sender).Tag.ToString());
            }
        }

        private void DgScene_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            TreeNode oNode = tvSeries.SelectedNode;
            if (oNode == null) return;
            if (oNode.Level == 0) return;
            if (oDMX.Areas512 == null) return;
            if (oDMX.Areas512[oNode.Parent.Index].MySces == null) return;
            if (mblnIsShowing) return;

            TreeNode Tmp = oNode.Parent;

            int intLevel = oNode.Index;

            if (mblnIsDelete == true)
            {
                oDMX.Areas512[Tmp.Index].MySers[intLevel].intStep--;
                oDMX.Areas512[Tmp.Index].MySers[intLevel].SceneIDs.RemoveAt(e.RowIndex);
                oDMX.Areas512[Tmp.Index].MySers[intLevel].RunTimeH.RemoveAt(e.RowIndex);
                oDMX.Areas512[Tmp.Index].MySers[intLevel].RunTimeL.RemoveAt(e.RowIndex);
            }
        }

        private void TM1_Click(object sender, EventArgs e)
        {
            if (tvLights.SelectedNode == null) return;
            if (tvLights.SelectedNode.Level != 0) return;

            int intTag = int.Parse(((ToolStripMenuItem)sender).Tag.ToString());

            for (int intI = 0; intI < intTag; intI++)
            {
                AddLoadTypeFromLibrary(tvLights.SelectedNode);
            }
        }

        private void rbOn_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked == false) return;

            byte bytTag = byte.Parse(((RadioButton)sender).Tag.ToString());

            foreach (LogicChannel Tmp in this.panel6.Controls)
            {
                Tmp.Tag = bytTag;
            }

            foreach (DataGridViewRow Tmp in tbRed.SelectedRows)
            {
                if (Tmp.Selected)  // 确定被选中，添加至区域列表
                {
                    for (int i = 0; i < mArayChnListInCurrentArea.Count; i++) 
                    {
                        tbRed[3 + i, Tmp.Index].Value = bytTag;
                        CsConst.ArayLevel[mArayChnListInCurrentArea[i] - 1] = byte.Parse(tbRed[3 + i, Tmp.Index].Value.ToString());
                    }
                }
            }
        }

        private void toolStripLabel1_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            oDMX.UploadSettings(mintDeviceType);
            Cursor.Current = Cursors.Default;
        }

        private byte[] ReadSceneLevelToBuffer(DMX.SceType oSce, byte bytAreaID,int intSceID)
        {
            if (oSce == null) return null;
            int intTotalChns = 0;

            byte[] ArayTmp = new byte[512];

            foreach (DMX.Chns oChn in oDMX.LoadTypes)
            {
                if (oChn.intBelongs == bytAreaID)
                {
                    Array.Copy(oSce.ArayLevel, oChn.intStart - 1, ArayTmp, intTotalChns, CsConst.myLEDs[oChn.intLibraryID].intSumChns);
                    intTotalChns += CsConst.myLEDs[oChn.intLibraryID].intSumChns; 
                }
            }
            intTotalChns += 3;
            byte[] ArayResult = new byte[intTotalChns + 2];
            ArayResult[0] = byte.Parse((intTotalChns / 256).ToString());
            ArayResult[1] = byte.Parse((intTotalChns % 256).ToString());
            ArayResult[2] = bytAreaID;
            ArayResult[3] = byte.Parse((intSceID / 256).ToString());
            ArayResult[4] = byte.Parse((intSceID % 256).ToString());

            Array.Copy(ArayTmp, 0, ArayResult, 5, intTotalChns-3);
            return ArayResult;
        }

        private void DgChns_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            if (oDMX == null) return;
            if (oDMX.LoadTypes == null) return;
            if (mblnIsShowing == true) return;
            try
            {
                oDMX.LoadTypes.RemoveAt(e.RowIndex);
                for (int i = 0; i < DgChns.Rows.Count; i++)
                {
                    DgChns[0, i].Value = i + 1;
                }
            }
            catch { }
        }

        private void tsDel_Click(object sender, EventArgs e)
        {
            if (oDMX == null) return;
            if (oDMX.Areas512 == null) return;
            if (tvAreas.SelectedNode == null) return;
            if (oDMX.LoadTypes == null) return;
            if (tvAreas.SelectedNode == null) return;
            if (tvAreas.SelectedNode.Level == 0 && tvAreas.SelectedNode.Index == 0) return;
            TreeNode oNode = tvAreas.SelectedNode;
            string strName = tvAreas.SelectedNode.Text.ToString();
            if (tvAreas.SelectedNode.Level != 0)  //删除回路
            {
                DeleteNodeInAreaForm(strName);
                tvAreas.SelectedNode.Remove();
                tvAreas.Nodes[0].Nodes.Add(oNode);
            }
            else            //删除区域
            {
                int intAreaID = tvAreas.SelectedNode.Index;
                oDMX.Areas512.RemoveAt(intAreaID - 1);

                foreach (TreeNode Tmp in tvAreas.SelectedNode.Nodes)
                {
                    DeleteNodeInAreaForm(Tmp.Text);
                    tvAreas.Nodes[0].Nodes.Add((TreeNode)Tmp.Clone());
                }
                tvAreas.SelectedNode.Remove();
            }
        }

        void DeleteNodeInAreaForm(string strName)
        {
            int intIndex = int.Parse(strName.Split('-')[0].ToString());
            oDMX.LoadTypes[intIndex - 1].intBelongs = 0;
        }

        private void btnAddSce_Click(object sender, EventArgs e)
        {
            if (oDMX == null) return;
            if (oDMX.Areas512 == null) return;

            if (TvScene.SelectedNode == null) return;
           
            TreeNode Tmp = TvScene.SelectedNode;
            if (TvScene.SelectedNode.Level != 0)
            {
                Tmp = Tmp.Parent;
            }

            int intSceID = AddNewSceneToArea(Tmp);
            //添加到窗体
            Tmp.Nodes.Add(intSceID.ToString() + "--Scene");
            DisplayAllChannelsValues(Tmp);
            oDMX.MyRead2UpFlags[6] = false;
            oDMX.MyRead2UpFlags[2] = false;
        }

        int AddNewSceneToArea(TreeNode Tmp)
        {
            // 添加到缓存
            if (oDMX.Areas512[Tmp.Index].MySces == null)
            {
                oDMX.Areas512[Tmp.Index].MySces = new List<DMX.SceType>();
            }

            List<int> oTmp = new List<int>(); //取出所有已用的场景号
            for (int i = 0; i < oDMX.Areas512[Tmp.Index].MySces.Count; i++)
            {
                oTmp.Add(oDMX.Areas512[Tmp.Index].MySces[i].intSceID);
            }
            //查找位置，替换buffer
            int intSceID = 1;
            while (oTmp.Contains(intSceID))
            {
                intSceID++;
            }

            DMX.SceType oSce = new DMX.SceType();
            oSce.intSceID = intSceID;
            oSce.Remark = "Scene";
            oSce.ArayLevel = new byte[mintMaxvalue];
            oSce.ArayLevel = (byte[])CsConst.ArayLevel.Clone();
            oSce.intRunTime = int.Parse(txtTime.Text.ToString());
            oDMX.Areas512[Tmp.Index].MySces.Add(oSce);

            return intSceID;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (oDMX == null) return;
            if (oDMX.Areas512 == null) return;
            if (tvSeries.SelectedNode == null) return;
            if (tvSeries.SelectedNode.Level == 0) return;

            byte bytAreaID = Convert.ToByte(tvSeries.SelectedNode.Parent.Text.Split('-')[0].ToString());
            byte bytSceID = byte.Parse(tvSeries.SelectedNode.Text.Split('-')[0].ToString());

            string strName = myStrName.Split('\\')[0].ToString();
            byte bytSubID = byte.Parse(strName.Split('-')[0]);
            byte bytDevID = byte.Parse(strName.Split('-')[1]);

            byte[] bytTmp = new byte[2];
            bytTmp[0] = bytAreaID;
            bytTmp[1] = bytSceID;

            CsConst.mySends.AddBufToSndList(bytTmp, 0x001A, bytSubID, bytDevID, false, true, false, false);
        }

        private void DgChns_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == -1) return;
            if (e.RowIndex == -1) return;
            try
            {
                if (DgChns[e.ColumnIndex, e.RowIndex].Value == null) DgChns[e.ColumnIndex, e.RowIndex].Value = "";
                for (int i = 0; i < DgChns.SelectedRows.Count; i++)
                {
                    string strTmp = "";
                    int intTmp = oDMX.LoadTypes[e.RowIndex].intStart;
                    if (int.TryParse(DgChns[1, e.RowIndex].Value.ToString(), out intTmp))
                    {
                        oDMX.LoadTypes[e.RowIndex].intStart = intTmp;
                    }
                    else
                    {
                        DgChns[1, e.RowIndex].Value = oDMX.LoadTypes[e.RowIndex].intStart;
                    }

                    if (e.ColumnIndex == 4 || e.ColumnIndex == 5 || e.ColumnIndex == 6)
                    {
                        DgChns[e.ColumnIndex, e.RowIndex].Value = HDLPF.IsNumStringMode(DgChns[e.ColumnIndex, e.RowIndex].Value.ToString(), 0, 255);
                    }

                    oDMX.LoadTypes[e.RowIndex].strRemark = DgChns[3, e.RowIndex].Value.ToString();

                    oDMX.LoadTypes[e.RowIndex].MinValue = byte.Parse(DgChns[4, e.RowIndex].Value.ToString());
                    oDMX.LoadTypes[e.RowIndex].MaxValue = Convert.ToByte(DgChns[5, e.RowIndex].Value.ToString());
                    oDMX.LoadTypes[e.RowIndex].MaxLevel = byte.Parse(DgChns[6, e.RowIndex].Value.ToString());

                    oDMX.LoadTypes[e.RowIndex].ArrtiChang = AttriChange.Items.IndexOf(DgChns[7, e.RowIndex].Value.ToString());
                    oDMX.MyRead2UpFlags[4] = false;
                }
            }
            catch { }
        }

        private void tvAreas_MouseDown(object sender, MouseEventArgs e)
        {
            TreeNode oNode = tvAreas.GetNodeAt(e.X,e.Y);
            if (oNode == null) return;
            if (oNode.Level != 0)
            {
                oNode = oNode.Parent;
            }
            if (oNode == tvAreas.Nodes[0]) return;
            tbArea.Text = oNode.Text.Split('-')[1].ToString();
        }

        private void tsmCopySce_Click(object sender, EventArgs e)
        {
            if (oDMX == null) return;
            if (oDMX.Areas512 == null) return;
            if (tvSeries.SelectedNode == null) return;
            if (tvSeries.SelectedNode.Level == 0) return;

            mintCopySeriesID = tvSeries.SelectedNode.Index;
        }

        private void tsmDeleteSce_Click(object sender, EventArgs e)
        {
            if (oDMX == null) return;
            if (oDMX.Areas512 == null) return;
            if (tvSeries.SelectedNode == null) return;

            DgScene.Rows.Clear();
            string strName = tvSeries.SelectedNode.Text.ToString();
            if (tvSeries.SelectedNode.Level != 0)  //删除序列
            {
                int intAreaID = tvSeries.SelectedNode.Parent.Index;
                if (oDMX.Areas512[intAreaID].MySers != null)
                {
                    oDMX.Areas512[intAreaID].MySers.RemoveAt(tvSeries.SelectedNode.Index);
                    tvSeries.SelectedNode.Remove();
                }
            }
        }

        private void tsmPasteSce_Click(object sender, EventArgs e)
        {
            if (oDMX == null) return;
            if (oDMX.Areas512 == null) return;
            if (tvSeries.SelectedNode == null) return;
            if (mintCopySeriesID == -1) return;

            TreeNode oNode = tvSeries.SelectedNode;
            if (oNode.Level != 0 )
            {
                oNode = oNode.Parent;
            }
            AddNewSeriesToForm(1, oNode.Index);
        }

        private void cbCurDev_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbCurDev.Items == null) return;
            if (cbCurDev.SelectedIndex == -1) return;
            if (CsConst.myLEDs == null) return;

            dgLights.Rows.Clear();

            for (int i = 0; i < CsConst.myLEDs[cbCurDev.SelectedIndex].intSumChns; i++)
            {
                object[] obj = {i + 1, CsConst.myLEDs[cbCurDev.SelectedIndex].LEDChns[i]};
                dgLights.Rows.Add(obj);
            }
        }

        private void tsl1_Click(object sender, EventArgs e)
        {
            if (tvArea2.SelectedNode == null) return;

            #region
            Color oColor = new Color();
            int intTag = int.Parse(((ToolStripButton)sender).Tag.ToString());

            switch (intTag)
            {
                case 0: oColor = Color.Red;
                    break;
                case 1: oColor = Color.Lime;
                    break;
                case 2: oColor = Color.Blue;
                    break;
                case 3: oColor = Color.Yellow;
                    break;
                case 4: oColor = Color.Magenta;
                    break;
                case 5: oColor = Color.Cyan;
                    break;
                case 6: oColor = Color.White;
                    break;
                case 7: oColor = Color.Black;
                    break;
                case 8:
                    ColorDialog MyDialog = new ColorDialog();
                    MyDialog.AllowFullOpen = true;
                    MyDialog.ShowHelp = true;
                    MyDialog.Color = oColor;
                    MyDialog.SolidColorOnly = true;


                    if (MyDialog.ShowDialog() == DialogResult.OK)
                    {
                        oColor = MyDialog.Color;
                    }
                    break;
            }
            #endregion

            foreach (SingleChn TmpCh in panel6.Controls)
            {
                switch (TmpCh.Tag.ToString())
                {
                    case "0":
                       // TmpCh.LightValueChanged -= new EventHandler(UpdateSceneLevelBuffer);
                        TmpCh.Text = oColor.R.ToString();
                       // TmpCh.LightValueChanged += new EventHandler(UpdateSceneLevelBuffer);
                        break;
                    case "1":
                       // TmpCh.LightValueChanged -= new EventHandler(UpdateSceneLevelBuffer);
                        TmpCh.Text = oColor.G.ToString();
                       // TmpCh.LightValueChanged += new EventHandler(UpdateSceneLevelBuffer);
                        break;
                    case "2":
                      //  TmpCh.LightValueChanged += new EventHandler(UpdateSceneLevelBuffer);
                        TmpCh.Text = oColor.B.ToString();
                        break;
                }
            }
        }

        private void tbRed_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (txtSeries.Visible == false && txtTime.Visible == false) return;
            if (tbRed.RowCount == 0) return;
            if (tbRed.SelectedRows.Count == 0) return;
            if (TvScene.SelectedNode == null) return;
            if (oDMX.Areas512 == null) return;

            if (mintDeviceType == 20 || mintDeviceType == 21 || mintDeviceType == 24)
            {
                tbRed[2, e.RowIndex].Value = HDLPF.GetStringFrmTimeMs(int.Parse(txtSeries.Text.ToString()));
            }
            else
            {
                tbRed[2, e.RowIndex].Value = HDLPF.GetStringFromTime(int.Parse(txtTime.Text.ToString()), ":");
            }
        }

        private void tbRed_RowsRemoved2(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            if (tbRed.RowCount == 0) return;
            if (tbRed.SelectedRows.Count == 0) return;
            if (TvScene.SelectedNode == null) return;
            if (oDMX.Areas512 == null) return;

            TreeNode oNode = TvScene.SelectedNode;
            if (oNode.Level != 0)
            {
                oNode = oNode.Parent;
            }
            if (oDMX.Areas512[oNode.Index].MySces == null) return;

            oDMX.Areas512[oNode.Index].MySces.RemoveAt(e.RowIndex);
            oNode.Nodes.RemoveAt(e.RowIndex);
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            if (oDMX == null) return;
            oDMX.ReadDefaultSettings();

            tc1.SelectedIndex = 0;

            ShowChannelsInformation();

            if (oDMX == null)
            {
                oDMX = new DMX();
                oDMX.strIP = txtSwiIP.Text.ToString();
                oDMX.strRouterIP = txtSwRoutIP.Text.ToString();
                oDMX.strMAC = mtbMAC.Text.ToString();
                oDMX.bytDMXChnID = byte.Parse(tbDMXID.Text.ToString());
                oDMX.intDMXPort = int.Parse(tbPort.Text.ToString());
            }
            else
            {
                txtSwiIP.Text = oDMX.strIP;
                txtSwRoutIP.Text = oDMX.strRouterIP;
                mtbMAC.Text = oDMX.strMAC;
                tbDMXID.Text = oDMX.bytDMXChnID.ToString();
                tbPort.Text = oDMX.intDMXPort.ToString();
            }

        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            if (oDMX == null) return;
            //1，保存基本信息
            Cursor.Current = Cursors.WaitCursor;
            oDMX.SaveChannelsInformation();

            oDMX.SaveDMXInformation();

            //2,保存区域信息
            oDMX.SaveAreasInformation();

            Cursor.Current = Cursors.Default;
        }

        private void toolStripLabel2_Click(object sender, EventArgs e)
        {

        }

        void SetVisableForDownOrUpload(bool blnIsEnable)
        {
            tslRead.Enabled = blnIsEnable;
            toolStripLabel2.Enabled = blnIsEnable;

            if (blnIsEnable == true && tsbar.Value != 0)
            {
                tsbHint.Visible = true;
                tsbHint.Text = "";
            }
            else
            {
                tsbar.Value = 0;
                tsbl4.Text = "0";
                tsbHint.Visible = false;
            }

            if (tsbar.Value == 100) tsbHint.Text = "Fully Success!";
            tsbar.Visible = !blnIsEnable;
            tsbl4.Visible = !blnIsEnable;
        }

        private void ReadThread()
        {
            Cursor.Current = Cursors.WaitCursor;
            CsConst.calculationWorker = new BackgroundWorker();
            CsConst.calculationWorker.DoWork += new DoWorkEventHandler(calculationWorker_DoWork);
            CsConst.calculationWorker.ProgressChanged += new ProgressChangedEventHandler(calculationWorker_ProgressChanged);
            CsConst.calculationWorker.WorkerReportsProgress = true;
            CsConst.calculationWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(calculationWorker_RunWorkerCompleted);
            CsConst.calculationWorker.RunWorkerAsync();
            Cursor.Current = Cursors.Default;
        }

        void calculationWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            SetVisableForDownOrUpload(true);
            MyActivePage = 0;
            Cursor.Current = Cursors.Default;
            //throw new NotImplementedException();
        }

        void calculationWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //throw new NotImplementedException();
            tsbar.Visible = true;
            tsbLabel4.Visible = true;
            tsbar1.ProgressBar.Value = e.ProgressPercentage;
            tsbLabel4.Text = e.ProgressPercentage + " %";
        }

        void calculationWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            //throw new NotImplementedException();
            if (oDMX == null) return;
            Cursor.Current = Cursors.WaitCursor;
            oDMX.UploadSettings(mintDeviceType);
        }

            //if (oDMX == null) return;
            //Cursor.Current = Cursors.WaitCursor;
            //oDMX.UploadSettings(mintDeviceType);
            ////Localization.SaveLanguage(frmDMX, Localization.LanguageType.English);
            //Cursor.Current = Cursors.Default;

        private void frmDMX_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                FlashWindow(this.Handle, true);
            }
        }

        private void chbOutput_CheckedChanged(object sender, EventArgs e)
        {
            TreeNode Tmp = TvScene.SelectedNode;
            if (Tmp == null) return;
            if (chbOutput.Checked == true)
            {
                if (CsConst.ArayLevel == null) return;
                //CsConst.ArayLevel = new byte[CsConst.ArayLevel.Length];
                byte[] bytTmp = null;
                #region
                if (DMXDeviceTypeList.HDLDMXDeviceTypeList.Contains(mintDeviceType)) // 48DMX
                {
                    bytTmp = new byte[tbRed.ColumnCount - 1];
                    bytTmp[0] = byte.Parse(Tmp.Text.Split('-')[0].ToString());
                    bytTmp[1] = 1;

                    for (int i = 3; i < tbRed.ColumnCount; i++)
                    {
                        int intCurID = int.Parse(tbRed.Columns[i].HeaderText.Split('-')[1].ToString());
                        bytTmp[i - 1] = (byte)(CsConst.ArayLevel[intCurID - 1] * 100 / 256);
                    }
                    bytTmp = CsConst.mySends.PackAndSend(bytTmp, 0xF074, SubnetID, DeviceID, false);
                    CsConst.mySends.SendBufToRemote(bytTmp, CsConst.myDestIP);
                }
                else if (mintDeviceType == 20 || mintDeviceType == 21) //240 channels DMX
                {
                    bytTmp = new byte[tbRed.ColumnCount + 3];
                    bytTmp[0] = byte.Parse((bytTmp.Length / 256).ToString());
                    bytTmp[1] = byte.Parse((bytTmp.Length % 256).ToString());
                    bytTmp[2] = byte.Parse(Tmp.Text.Split('-')[0].ToString());
                    bytTmp[3] = 0;
                    bytTmp[4] = 1;

                    for (int i = 3; i < tbRed.ColumnCount; i++)
                    {
                        int intCurID = int.Parse(tbRed.Columns[i].HeaderText.Split('-')[1].ToString());
                        bytTmp[i + 2] = CsConst.ArayLevel[intCurID - 1];
                    }

                    if (mintDeviceType == 20)
                        bytTmp = CsConst.mySends.PackAndSend(bytTmp, 0x100C, SubnetID, DeviceID, true);
                    else
                        bytTmp = CsConst.mySends.PackAndSend(bytTmp, 0x1120, SubnetID, DeviceID, true);
                    CsConst.mySends.SendBufToRemote(bytTmp, CsConst.myDestIP);
                }
                #endregion
            }
        }

        private void tvAreas_ItemDrag(object sender, ItemDragEventArgs e)
        {
            DoDragDrop(e.Item, DragDropEffects.Move);
        }

        private void tvAreas_DragDrop(object sender, DragEventArgs e)
        {
            TreeNode myNode = null;
            if (e.Data.GetDataPresent(typeof(TreeNode)))
            {
                myNode = (TreeNode)(e.Data.GetData(typeof(TreeNode)));
            }
            else
            {
                MessageBox.Show("error");
            }

            Position.X = e.X;
            Position.Y = e.Y;
            Position = ((TreeView)sender).PointToClient(Position);
            TreeNode DropNode = ((TreeView)sender).GetNodeAt(Position);
            // 1.目标节点不是空。2.目标节点不是被拖拽接点的字节点。3.目标节点不是被拖拽节点本身
            if (DropNode != null && DropNode.Parent != myNode && DropNode != myNode && DropNode.Level != myNode.Level)
            {
                TreeNode DragNode = myNode;
                // 将被拖拽节点从原来位置删除。
                myNode.Remove();
                // 在目标节点下增加被拖拽节点
                int intLoadID = int.Parse(DragNode.Text.Split('-')[0].ToString());

                string strNo = HDLPF.GetNumFromString(DropNode.Text.Split('-')[0].ToString());
                if (strNo == "") strNo = "0";

                oDMX.LoadTypes[intLoadID - 1].intBelongs = int.Parse(strNo);
                DropNode.Nodes.Add(DragNode);
            }
            if (DropNode == null) return;
            // 如果目标节点不存在，即拖拽的位置不存在节点，那么就将被拖拽节点放在根节点之
        }

        private void tvAreas_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(TreeNode)))
            {
                e.Effect = DragDropEffects.Move;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void tbRed_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (mblnIsShowing) return;
            if (tbRed.RowCount == 0) return;
            if (tbRed.SelectedRows.Count == 0) return;
            if (TvScene.SelectedNode == null) return;
            if (oDMX.Areas512 == null) return;
            TreeNode oNode = TvScene.SelectedNode;
            if (oNode.Level != 0)
            {
                oNode = oNode.Parent;
            }
            if (oDMX.Areas512[oNode.Index].MySces == null) return;
            try
            {
                #region
                foreach (DataGridViewRow r in tbRed.SelectedRows)
                {
                    if (r.Selected)
                    {
                        string strTmp = null;
                        switch (e.ColumnIndex)
                        {
                            case 0: break;
                            case 1:
                                if (tbRed[1, e.RowIndex].Value != null) strTmp = tbRed[1, e.RowIndex].Value.ToString();
                                else strTmp = "";
                                tbRed[1, e.RowIndex].Value = HDLPF.IsRightStringMode(strTmp);
                                oDMX.Areas512[oNode.Index].MySces[e.RowIndex].Remark = tbRed[1, e.RowIndex].Value.ToString();
                                break;
                            case 2:
                                if (mintDeviceType == 20 || mintDeviceType == 21 || mintDeviceType == 24)
                                {
                                    tbRed[2, e.RowIndex].Value = HDLPF.GetStringFrmTimeMs(int.Parse(txtSeries.Text.ToString()));
                                    oDMX.Areas512[oNode.Index].MySces[e.RowIndex].intRunTime = int.Parse(HDLPF.GetTimeFrmStringMs(tbRed[2, e.RowIndex].Value.ToString()));
                                }
                                else
                                {
                                    tbRed[2, e.RowIndex].Value = HDLPF.GetStringFromTime(int.Parse(txtTime.Text.ToString()), ":");
                                    oDMX.Areas512[oNode.Index].MySces[e.RowIndex].intRunTime = int.Parse(HDLPF.GetTimeFromString(tbRed[2, e.RowIndex].Value.ToString(), ':'));
                                }
                                break;
                            default:
                                if (mintDeviceType == 20 || mintDeviceType == 21 || mintDeviceType == 24)
                                {
                                    oDMX.Areas512[oNode.Index].MySces[e.RowIndex].intRunTime = int.Parse(HDLPF.GetTimeFrmStringMs(r.Cells[2].Value.ToString()));
                                }
                                else
                                {
                                    oDMX.Areas512[oNode.Index].MySces[e.RowIndex].intRunTime = int.Parse(HDLPF.GetTimeFromString(r.Cells[2].Value.ToString(), ':'));
                                }
                                for (int i = 0; i < mArayChnListInCurrentArea.Count; i++)
                                {
                                    if (tbRed[3 + i, e.RowIndex].Value == null) tbRed[3 + i, e.RowIndex].Value = 0;
                                    tbRed[3 + i, e.RowIndex].Value = HDLPF.IsNumStringMode(tbRed[3 + i, e.RowIndex].Value.ToString(), 0, 255);
                                    CsConst.ArayLevel[mArayChnListInCurrentArea[i] - 1] = byte.Parse(tbRed[3 + i, e.RowIndex].Value.ToString());
                                }
                                oDMX.Areas512[oNode.Index].MySces[e.RowIndex].ArayLevel = (byte[])CsConst.ArayLevel.Clone();
                                break;
                        }
                    }
                }
                #endregion
            }
            catch { }
        }

        private void tbRed_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == 2)
                {
                    if (mintDeviceType == 20 || mintDeviceType == 21 || mintDeviceType == 24)
                    {
                        ((DataGridView)sender).Controls.Add(txtSeries);
                        txtSeries.Show();
                        txtSeries.Visible = true;
                        Rectangle rect = ((DataGridView)sender).GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true);
                        txtSeries.Size = rect.Size;
                        txtSeries.Top = rect.Top;
                        txtSeries.Left = rect.Left;

                        txtSeries.Text = HDLPF.GetTimeFrmStringMs(((DataGridView)sender)[2, e.RowIndex].Value.ToString());
                    }
                    else
                    {
                        ((DataGridView)sender).Controls.Add(txtTime);
                        txtTime.Show();
                        txtTime.Visible = true;
                        Rectangle rect = ((DataGridView)sender).GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true);
                        txtTime.Size = rect.Size;
                        txtTime.Top = rect.Top;
                        txtTime.Left = rect.Left;
                        if (((DataGridView)sender)[2, e.RowIndex].Value == null) ((DataGridView)sender)[2, e.RowIndex].Value = "0:0";
                        txtTime.Text = HDLPF.GetTimeFromString(((DataGridView)sender)[2, e.RowIndex].Value.ToString(), ':');
                    }
                }
                else
                {
                    txtSeries.Visible = false;
                    txtTime.Visible = false;
                }
            }
            catch { }
        }

        private void frmDMX_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Dispose();
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tvLights_MouseDoubleClick(tvLights,null);
        }

        private void tmSame_Click(object sender, EventArgs e)
        {
            DataGridView oDG = null; 
            switch (tc1.SelectedIndex)
            {
                case 0: oDG = DgChns; break;
                case 2: oDG = tbRed; break;
            }

            if (oDG.SelectedCells == null) return;
            if (oDG.SelectedRows == null) return;
            if (myintRowIndex == -1) return;
            if (myintColIndex == -1) return;
            string strTmp = oDG[myintColIndex, myintRowIndex].Value.ToString();

            int intTag = Convert.ToInt16((sender as ToolStripMenuItem).Tag.ToString());
            UpdateBufferWhenchanged(intTag, strTmp, oDG);
        }

        void UpdateBufferWhenchanged(int intTag,string strTmp, DataGridView oDGV)
        {
            if (oDGV.SelectedCells == null) return;
            if (oDGV.SelectedRows == null) return;
            string strNo = HDLPF.GetNumFromString(strTmp);
            if (strNo == "") strNo = "0";

            switch (intTag)
            {
                case 0:
                    foreach (DataGridViewRow r in oDGV.SelectedRows)
                    {
                        if (r.Selected & r.Index != myintRowIndex)
                        {
                            oDGV[myintColIndex, r.Index].Value = strTmp;
                        }
                    }
                    break;
                case 1:
                    int intTmp = Convert.ToInt16(strNo);
                    foreach (DataGridViewRow r in oDGV.Rows)
                    {
                        if (r.Selected & r.Index != myintRowIndex)
                        {
                            intTmp++;
                            oDGV[myintColIndex, r.Index].Value = strTmp.Replace(strNo, intTmp.ToString());
                        }
                    }
                    break;
                case 2:
                    intTmp = Convert.ToInt16(strNo);
                    foreach (DataGridViewRow r in oDGV.Rows)
                    {
                        if (r.Selected & r.Index != myintRowIndex)
                        {
                            intTmp--;
                            if (intTmp <= 0) intTmp = 255;
                            oDGV[myintColIndex, r.Index].Value = strTmp.Replace(strNo, intTmp.ToString());
                        }
                    }
                    break;
            }
        }

        private void DgChns_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            DgChns.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void DgChns_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
             //点右键时高亮整行,并在当前鼠标位置弹出子菜单
            if (e.RowIndex == -1) return;
            if (TvScene.SelectedNode == null) return;
            TreeNode oNode = TvScene.SelectedNode;
            if (oNode.Level == 1) oNode = oNode.Parent;
            try
            {
                CsConst.ArayLevel = oDMX.Areas512[oNode.Index].MySces[e.RowIndex].ArayLevel;
                if (e.Button == MouseButtons.Right)
                {
                    if ((e.RowIndex >= 0) & ((sender as DataGridView).RowCount > 0))  //如果行数不为0
                    {
                        // DgChns.CurrentCell = DgChns.Rows[e.RowIndex].Cells[e.ColumnIndex];
                        myintRowIndex = e.RowIndex;
                        myintColIndex = e.ColumnIndex;
                        cm4.Show(MousePosition.X, MousePosition.Y);
                    }
                }
            }
            catch { }
        }

        private void DgChns_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void rbGroup_CheckedChanged(object sender, EventArgs e)
        {
            int intTag  =int.Parse((sender as RadioButton).Tag.ToString());

            tbArea.Enabled = (intTag == 1);
            btnArea.Enabled = (intTag == 1);
            tvAreas.Enabled = (intTag == 1);

            if (intTag == 0)
            {
                foreach (DMX.Chns oChn in oDMX.LoadTypes)
                {
                    oChn.intBelongs = 1;
                }
                lbSce.Text = "Groups Information";
            }
            else lbSce.Text = "Scenes Information";
        }

        private void picBox_MouseDown(object sender, MouseEventArgs e)
        {
            Cursor.Current = Cursors.Hand;
            Bitmap myBitmap = new Bitmap(picBox.Image);
            Color oColor = myBitmap.GetPixel(e.X, e.Y);

            foreach (LogicChannel TmpCh in panel6.Controls)
            {
                switch (TmpCh.Tag.ToString())
                {
                    case "0":
                        TmpCh.Light = Convert.ToByte(oColor.R.ToString());
                        break;
                    case "1":
                        TmpCh.Light = Convert.ToByte(oColor.G.ToString());
                        break;
                    case "2":
                        TmpCh.Light = Convert.ToByte(oColor.B.ToString());
                        break;
                    case "3":
                        TmpCh.Light = Convert.ToByte(oColor.A.ToString());
                        break;
                }

            }
            chbOutput_CheckedChanged(chbOutput, null);
            Cursor.Current = Cursors.Default;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (tvArea2.SelectedNode == null) return;

            #region
            Color oColor = new Color();

            ColorDialog MyDialog = new ColorDialog();
            MyDialog.AllowFullOpen = true;
            MyDialog.ShowHelp = true;
            MyDialog.Color = oColor;
            MyDialog.SolidColorOnly = true;
            #endregion

            if (MyDialog.ShowDialog() == DialogResult.OK)
            {
                oColor = MyDialog.Color;

                foreach (SingleChn TmpCh in panel6.Controls)
                {
                    switch (TmpCh.Tag.ToString())
                    {
                        case "0":
                            // TmpCh.LightValueChanged -= new EventHandler(UpdateSceneLevelBuffer);
                            TmpCh.Text = oColor.R.ToString();
                            // TmpCh.LightValueChanged += new EventHandler(UpdateSceneLevelBuffer);
                            break;
                        case "1":
                            // TmpCh.LightValueChanged -= new EventHandler(UpdateSceneLevelBuffer);
                            TmpCh.Text = oColor.G.ToString();
                            // TmpCh.LightValueChanged += new EventHandler(UpdateSceneLevelBuffer);
                            break;
                        case "2":
                            //  TmpCh.LightValueChanged += new EventHandler(UpdateSceneLevelBuffer);
                            TmpCh.Text = oColor.B.ToString();
                            break;
                    }
                }
            }
        }

        private void copySceneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (mblnIsShowing) return;
            if (tbRed.RowCount == 0) return;
            if (tbRed.SelectedRows.Count == 0) return;
            if (TvScene.SelectedNode == null) return;
            if (oDMX.Areas512 == null) return;
            TreeNode oNode = TvScene.SelectedNode;
            if (oNode.Level != 0)
            {
                oNode = oNode.Parent;
            }
            if (oDMX.Areas512[oNode.Index].MySces == null) return;
            if (tbRed.CurrentRow.Index == -1) return;
            List<int> selectRowsIndex = new List<int>();
            bytAry = new List<byte[]>();
            for (int i = 0; i < tbRed.RowCount; i++)
            {
                if (tbRed.Rows[i].Selected)
                    selectRowsIndex.Add(i);
            }
            for (int i = 0; i < selectRowsIndex.Count; i++)
            {
                CsConst.ArayLevel = (byte[])oDMX.Areas512[oNode.Index].MySces[selectRowsIndex[i]].ArayLevel.Clone();
                bytAry.Add(CsConst.ArayLevel);
            }
        }

        private void tmiAll_Click(object sender, EventArgs e)
        {
            if (tvArea2.Nodes == null) return;
            int intTag = Convert.ToInt16((sender as ToolStripMenuItem).Tag.ToString());

            foreach (TreeNode oNode in tvArea2.Nodes)
            {
                switch (intTag) 
                {
                    case 0:oNode.StateImageIndex = 3;break;
                    case 1:oNode.StateImageIndex = 2;break;
                    case 2: if (oNode.StateImageIndex == 2) oNode.StateImageIndex = 3;
                        else oNode.StateImageIndex = 2; break;
                }
                
            }
        }


        private void uNselectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tvArea2.Nodes == null) return;
            if (tvArea2.SelectedNode == null) return;
            tvArea2.SelectedNode.StateImageIndex = 2;
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            if (tvArea2.Nodes == null) return;
            if (tvArea2.SelectedNode == null) return;
            tvArea2.SelectedNode.StateImageIndex = 3;
        }

        private void tbAdd_Click(object sender, EventArgs e)
        {
            if (tvAreas.SelectedNode == null) return;
            if (tvAreas.SelectedNode.Level == 1) return;
            if (tvAreas.SelectedNode.Index == 0) return;
            if (DgAreas.SelectedRows == null) return;
            int index = DgAreas.CurrentRow.Index;

            try
            {
                string strNo = HDLPF.GetNumFromString(tvAreas.SelectedNode.Text.Split('-')[0].ToString());

                foreach (DataGridViewRow Tmp in DgAreas.SelectedRows)
                {
                    if (Tmp.Selected)  // 确定被选中，添加至区域列表
                    {
                        int intLoadID = int.Parse(Tmp.Cells[0].Value.ToString());
                        if (oDMX.LoadTypes[intLoadID - 1].intBelongs != Convert.ToInt16(strNo))
                        {
                            oDMX.LoadTypes[intLoadID - 1].intBelongs = Convert.ToByte(strNo);
                            tvAreas.SelectedNode.Nodes.Insert(0, (intLoadID).ToString() + "--" + oDMX.LoadTypes[Tmp.Index].strRemark);
                        }
                    }
                }

                tvAreas.Nodes[0].Nodes.Clear();
                for (int i = 0; i < oDMX.LoadTypes.Count; i++)
                {
                    if (oDMX.LoadTypes[i].intBelongs == 0)
                    {
                        tvAreas.Nodes[0].Nodes.Add(null, (i + 1).ToString() + "--" + oDMX.LoadTypes[i].strRemark, 1, 1);
                    }
                }
                tc1_SelectedIndexChanged(null, null);
                DgAreas.CurrentCell = DgAreas.Rows[index].Cells[0];
            }
            catch { }
        }

        private void toolStripButton11_Click(object sender, EventArgs e)
        {
            if (oDMX == null) return;
        }

        private void dgLights_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnMArea_Click(object sender, EventArgs e)
        {
            if (tbArea.Text == "" || tbArea.Text == null) return;
            if (oDMX == null) return;
            if (tvAreas.Nodes == null) return;
            if (tvAreas.SelectedNode == null) return;
            if (tvAreas.SelectedNode.Level == 1) return;

            TreeNode oNode = tvAreas.SelectedNode;
            if (oNode.Index == 0) return;
            oDMX.Areas512[oNode.Index-1].strRemark = tbArea.Text;
            oNode.Text = oNode.Text.Split('-')[0] + "-" + tbArea.Text;
           
        }

        private void tvLights_MouseDown(object sender, MouseEventArgs e)
        {
            
        }

        private void txtSwiIP_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void tslRead_Click(object sender, EventArgs e)
        {
            byte bytTag = Convert.ToByte(((ToolStripButton)sender).Tag.ToString());
            bool blnShowMsg = (CsConst.MyEditMode != 1);
            if (HDLPF.UploadOrReadGoOn(this.Text, bytTag, blnShowMsg))
            {
                Cursor.Current = Cursors.WaitCursor;
                // SetVisableForDownOrUpload(false);
                // ReadDownLoadThread();  //增加线程，使得当前窗体的任何操作不被限制

                CsConst.MyUPload2DownLists = new List<byte[]>();

                string strName = myStrName.Split('\\')[0].ToString();
                byte bytSubID = byte.Parse(strName.Split('-')[0]);
                byte bytDevID = byte.Parse(strName.Split('-')[1]);

                byte[] ArayRelay = new byte[] { bytSubID, bytDevID, (byte)(mintDeviceType / 256), (byte)(mintDeviceType % 256),
                    (byte)(tc1.SelectedIndex + 1),(byte)(mintIDIndex / 256), (byte)(mintIDIndex % 256), };
                      CsConst.MyUPload2DownLists.Add(ArayRelay);

                CsConst.MyUpload2Down = Convert.ToByte(((ToolStripButton)sender).Tag.ToString());
                FrmDownloadShow Frm = new FrmDownloadShow();
                Frm.FormClosing += new FormClosingEventHandler(UpdateDisplayInformationAccordingly);
                Frm.ShowDialog();
            }
        }

        private void pasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (oDMX == null) return;
            if (oDMX.Areas512 == null) return;
            if (CsConst.ArayLevel == null) return;
            if (TvScene.SelectedNode == null) return;

            TreeNode Tmp = TvScene.SelectedNode;
            if (TvScene.SelectedNode.Level != 0)
            {
                Tmp = Tmp.Parent;
            }
            for (int i = 0; i < bytAry.Count; i++)
            {
                int intSceID = AddNewSceneToArea(Tmp);
                //DMX.SceType oSce = new DMX.SceType();
                //oSce.intSceID = intSceID;
                //oSce.Remark = "Scene";
                //oSce.ArayLevel = new byte[mintMaxvalue];
                //oSce.ArayLevel = (byte[])bytAry[i].Clone();
                //oSce.intRunTime = int.Parse(txtTime.Text.ToString());
                //oDMX.Areas512[Tmp.Index].MySces.Add(oSce);
                
                //添加到窗体
                Tmp.Nodes.Add(intSceID.ToString() + "--Scene");
                DisplayAllChannelsValues(Tmp);
            }
        }

        private void tsl6_Click(object sender, EventArgs e)
        {

        }

        private void tvLights_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            TreeNode oNode = tvLights.GetNodeAt(e.Location);
            if (oNode == null) return;
            if (oNode.Level != 0) return;

            AddLoadTypeFromLibrary(oNode);
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            if (oDMX == null) return;
            if (oDMX.Areas512 == null) return;
            if (tbRemarkSeries.Text == "") return;
            if (cbstep.Text == "") return;
            if (cbSeriesRepeat.Text == "") return;
            if (tvSeries.SelectedNode == null) return;
            TreeNode oNode = tvSeries.SelectedNode;
            mblnIsShowing = true;
            if (oNode.Level == 0) return;

            if (oDMX.Areas512[oNode.Parent.Index].MySers[oNode.Index] == null) //添加新序列
            {
                AddNewSeriesToForm(0, oNode.Parent.Index);
            }
            else  // 原有基础更新
            {
                oDMX.Areas512[oNode.Parent.Index].MySers[oNode.Index].Remark = tbRemarkSeries.Text;
                oDMX.Areas512[oNode.Parent.Index].MySers[oNode.Index].intStep = byte.Parse((cbstep.SelectedIndex + 1).ToString());
                oDMX.Areas512[oNode.Parent.Index].MySers[oNode.Index].intTimes = byte.Parse(cbSeriesRepeat.SelectedIndex.ToString());
                oDMX.Areas512[oNode.Parent.Index].MySers[oNode.Index].intRunMode = byte.Parse(mintSeriesMode.ToString());
                oNode.Text = oNode.Text.Split('-')[0] + "-" + tbRemarkSeries.Text;
                // 多增加 少减少
                if (cbstep.SelectedIndex + 1 > oDMX.Areas512[oNode.Parent.Index].MySers[oNode.Index].SceneIDs.Count)
                {
                    int intTmp = cbstep.SelectedIndex + 1 - oDMX.Areas512[oNode.Parent.Index].MySers[oNode.Index].SceneIDs.Count;
                    for (int intI = 0; intI < intTmp; intI++)
                    {
                        oDMX.Areas512[oNode.Parent.Index].MySers[oNode.Index].SceneIDs.Add(1);
                        oDMX.Areas512[oNode.Parent.Index].MySers[oNode.Index].RunTimeH.Add(0);
                        oDMX.Areas512[oNode.Parent.Index].MySers[oNode.Index].RunTimeL.Add(0);
                    }
                }
                else if (cbstep.SelectedIndex + 1 < oDMX.Areas512[oNode.Parent.Index].MySers[oNode.Index].SceneIDs.Count)
                {
                    int intTmp = oDMX.Areas512[oNode.Parent.Index].MySers[oNode.Index].SceneIDs.Count - cbstep.SelectedIndex - 1;
                    oDMX.Areas512[oNode.Parent.Index].MySers[oNode.Index].SceneIDs.RemoveRange(cbstep.SelectedIndex + 1, intTmp);
                    oDMX.Areas512[oNode.Parent.Index].MySers[oNode.Index].RunTimeH.RemoveRange(cbstep.SelectedIndex + 1, intTmp);
                    oDMX.Areas512[oNode.Parent.Index].MySers[oNode.Index].RunTimeL.RemoveRange(cbstep.SelectedIndex + 1, intTmp);
                }
                ShowSequenceStepsToForm(oDMX.Areas512[oNode.Parent.Index], oDMX.Areas512[oNode.Parent.Index].MySers[oNode.Index]);
            }
            mblnIsShowing = false;
        }

        private void tmRead_Click(object sender, EventArgs e)
        {
            if (((ToolStripMenuItem)sender).Tag == null) return;
            byte bytTag = Convert.ToByte(((ToolStripMenuItem)sender).Tag.ToString());
            MyActivePage = tc1.SelectedIndex + 1;

            if (bytTag == 0) // 下载
            {
                tslRead_Click(tslRead, null);
            }
            else
            {
                toolStripLabel2_Click(toolStripLabel2, null);
            }
        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void label14_Click(object sender, EventArgs e)
        {

        }

        private void frmDMX_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (CsConst.MyEditMode == 1) // 在线模式
            {
                if (CsConst.MyEditMode == 0 || CsConst.MyEditMode == 1)
                {
                    if (oDMX.MyRead2UpFlags[1] == false)
                    {
                        tslRead_Click(toolStripLabel2, null);
                    }
                }
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy)
            {
                CsConst.calculationWorker.Dispose();
                CsConst.calculationWorker = null;
            }

        }

        private void frmDMX_Shown(object sender, EventArgs e)
        {
            //openToolStripButton_Click(null, null);
            SetCtrlsVisbleWithDifferentDeviceType();
            Cursor.Current = Cursors.WaitCursor;
            if (CsConst.MyEditMode == 0)
            {
                ShowChannelsInformation();

                if (oDMX == null)
                {
                    oDMX.strIP = txtSwiIP.Text.ToString();
                    oDMX.strRouterIP = txtSwRoutIP.Text.ToString();
                    oDMX.strMAC = mtbMAC.Text.ToString();
                    oDMX.bytDMXChnID = byte.Parse(tbDMXID.Text.ToString());
                    oDMX.intDMXPort = int.Parse(tbPort.Text.ToString());
                }
                else
                {
                    txtSwiIP.Text = oDMX.strIP;
                    txtSwRoutIP.Text = oDMX.strRouterIP;
                    mtbMAC.Text = oDMX.strMAC;
                    tbDMXID.Text = oDMX.bytDMXChnID.ToString();
                    tbPort.Text = oDMX.intDMXPort.ToString();
                }                
            }
            else if (CsConst.MyEditMode == 1)
            {
                MyActivePage = 1;

                if (oDMX.MyRead2UpFlags[0] == false)
                {
                    tslRead_Click(tslRead, null);
                    oDMX.MyRead2UpFlags[0] = true;
                }
                else
                {
                    ShowChannelsInformation();

                    if (gbLCD.Visible == true)
                    {
                        if (oDMX.PowerSaving == null) oDMX.PowerSaving = new Byte[10];
                        if (oDMX.PowerSaving[0] == 0) rbPowerOn.Checked = true;
                        else
                        {
                            rbPowerOff.Checked = true;
                            tbLCDelay.Value = oDMX.PowerSaving[0];
                        }
                        rbPowerOn_CheckedChanged(null, null);
                    }
                }
                MyActivePage = 0;
            }
            Cursor.Current = Cursors.Default;
        }

        private void tbRed_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            tbRed.CommitEdit(DataGridViewDataErrorContexts.Commit);
            ShowDevicesChns(tvArea2.SelectedNode);
        }

        private void DgScene_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            DgScene.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void DgScene_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1) return;
            if (e.ColumnIndex == -1) return;
            if (((DataGridView)sender).RowCount == 0) return;
            int intAreaID = tvSeries.SelectedNode.Index;

            if (tvSeries.SelectedNode.Level == 1)
            {
                intAreaID = tvSeries.SelectedNode.Parent.Index;
            }
            try
            {
                for (int i = 0; i < DgScene.SelectedRows.Count; i++)
                {
                    DgScene.SelectedRows[i].Cells[e.ColumnIndex].Value = DgScene[e.ColumnIndex, e.RowIndex].Value.ToString();

                    int intIndex = DgScene.SelectedRows[i].Index;
                    if (e.ColumnIndex == 1)
                    {
                        int intTmp = int.Parse(DgScene[1, intIndex].Value.ToString().Split('-')[0].ToString());
                        oDMX.Areas512[intAreaID].MySers[mintCurSeries].SceneIDs[intIndex] = byte.Parse(intTmp.ToString());
                    }
                    else if (e.ColumnIndex == 2)
                    {
                        DgScene[2, intIndex].Value = HDLPF.GetStringFrmTimeMs(int.Parse(txtSeries.Text.ToString()));
                        oDMX.Areas512[intAreaID].MySers[mintCurSeries].RunTimeH[intIndex] = byte.Parse((int.Parse(txtSeries.Text) / 256).ToString());
                        oDMX.Areas512[intAreaID].MySers[mintCurSeries].RunTimeL[intIndex] = byte.Parse((int.Parse(txtSeries.Text) % 256).ToString());
                    }
                    oDMX.MyRead2UpFlags[7] = false;
                    oDMX.MyRead2UpFlags[3] = false;
                }
            }
            catch { }
        }

        private void girdLogicLight_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1 || e.ColumnIndex == -1) return;
            if (oDMX.LoadTypes == null || oDMX.LoadTypes.Count == 0) return;
            try
            {
                //gridLibrary.Rows.Clear();
                Byte bLogicId = Convert.ToByte(e.RowIndex.ToString());
                int bLibraryId = oDMX.LoadTypes[bLogicId].intLibraryID;
                if (oDMX.LoadTypes[bLogicId].arrLibraryParams == null) return;
                if (CsConst.myLEDs == null) return;
                this.panLibrary.Controls.Clear();

                Byte bCurrentId = 0;
                foreach (Byte[] arrTmpLibrary in oDMX.LoadTypes[bLogicId].arrLibraryParams)
                {
                     if (arrTmpLibrary == null) return;
                     Object[] oTmp = null;
                     switch (bLibraryId + 1)
                     {
                         case 1:
                             #region
                             SingleChn devPic = new SingleChn();
                             devPic.Size = new System.Drawing.Size(200, 24);
                             devPic.Location = new Point(64, 30 + bCurrentId * 60);
                             devPic.Tag = bCurrentId;
                             devPic.Name = "Dev" + (bCurrentId + 1).ToString();
                             devPic.Text = arrTmpLibrary[0].ToString();
                             panLibrary.Controls.Add(devPic);
                             #endregion
                             break;
                         case 2: 
                             #region
                             CCT devCCT = new CCT("2700K",arrTmpLibrary[0]);
                             devCCT.Size = new System.Drawing.Size(200, 24);
                             devCCT.Location = new Point(64, 30 + bCurrentId * 60);
                             devCCT.Tag = bCurrentId;
                             devCCT.Name = "Dev" + (bCurrentId + 1).ToString();
                             panLibrary.Controls.Add(devCCT);
                             #endregion
                             break;
                         case 3:
                             Color cTmpColor = Color.FromArgb(arrTmpLibrary[0], arrTmpLibrary[1], arrTmpLibrary[2]);
                             #region
                             RGB devRGB = new RGB(cTmpColor);
                             devRGB.Size = new System.Drawing.Size(200, 24);
                             devRGB.Location = new Point(64, 30 + bCurrentId * 60);
                             devRGB.Tag = bCurrentId;
                             devRGB.Name = "Dev" + (bCurrentId + 1).ToString();
                             panLibrary.Controls.Add(devRGB);
                             #endregion
                             break;
                         case 4:
                         case 5:
                             cTmpColor = Color.FromArgb(arrTmpLibrary[0], arrTmpLibrary[1], arrTmpLibrary[2]);
                             #region
                             RGBW devRGBW = new RGBW(cTmpColor, arrTmpLibrary[4]);
                             devRGBW.Size = new System.Drawing.Size(200, 24);
                             devRGBW.Location = new Point(64, 30 + bCurrentId * 60);
                             devRGBW.Tag = bCurrentId;
                             devRGBW.Name = "Dev" + (bCurrentId + 1).ToString();
                             panLibrary.Controls.Add(devRGBW);
                             #endregion
                             break;
                     }
                     bCurrentId++;
                }
            }
            catch
            { }
        }

        void UpdateLibraryData(Byte bLIbraryId)
        {
            if (oDMX.LoadTypes == null || oDMX.LoadTypes.Count == 0) return;
            try
            {
                //gridLibrary.Rows.Clear();
                Byte bLogicId = Convert.ToByte(bLIbraryId.ToString());
                int bLibraryId = oDMX.LoadTypes[bLogicId].intLibraryID;
                foreach (Control sender in panLibrary.Controls)
                {
                    Byte bTag = Convert.ToByte((sender as Control).Tag.ToString());
                    if (oDMX.LoadTypes[bLogicId].arrLibraryParams == null)
                    {
                        oDMX.LoadTypes[bLogicId].arrLibraryParams = new List<byte[]>();
                        for (int i = 0; i < 4; i++) oDMX.LoadTypes[bLogicId].arrLibraryParams.Add(new Byte[10]);
                    }
                    if (sender is SingleChn)
                    {
                        Byte[] arrTmp = new Byte[10];
                        arrTmp[0] = Convert.ToByte((sender as SingleChn).Text.ToString());
                        oDMX.LoadTypes[bLogicId].arrLibraryParams[bTag] = arrTmp;
                    }
                    else if (sender is CCT)
                    {
                        Byte[] arrTmp = new Byte[10];
                        Byte bTmpPercent = Convert.ToByte((sender as CCT).Light.ToString());
                        arrTmp[0] = Convert.ToByte((27 * (150 - bTmpPercent) / 256).ToString());
                        arrTmp[1] = Convert.ToByte((27 * (150 - bTmpPercent) % 256).ToString());
                        oDMX.LoadTypes[bLogicId].arrLibraryParams[bTag] = arrTmp;
                    }
                    else if (sender is RGB)
                    {
                        Byte[] arrTmp = new Byte[10];
                        arrTmp[0] = Convert.ToByte((sender as RGB)._myColor.R.ToString());
                        arrTmp[1] = Convert.ToByte((sender as RGB)._myColor.G.ToString());
                        arrTmp[2] = Convert.ToByte((sender as RGB)._myColor.B.ToString());
                        oDMX.LoadTypes[bLogicId].arrLibraryParams[bTag] = arrTmp;
                    }
                    else if (sender is RGBW)
                    {
                        Byte[] arrTmp = new Byte[10];
                        arrTmp[0] = Convert.ToByte((sender as RGBW)._myColor.R.ToString());
                        arrTmp[1] = Convert.ToByte((sender as RGBW)._myColor.G.ToString());
                        arrTmp[2] = Convert.ToByte((sender as RGBW)._myColor.B.ToString());
                        arrTmp[3] = Convert.ToByte((sender as RGBW).Light.ToString());
                        oDMX.LoadTypes[bLogicId].arrLibraryParams[bTag] = arrTmp;
                    }
                }
            }
            catch
            { }
            //throw new NotImplementedException();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (girdLogicLight.CurrentCell.RowIndex == -1) return;

            try
            {
                Byte bLibraryId = Convert.ToByte(girdLogicLight.CurrentCell.RowIndex);

                UpdateLibraryData(bLibraryId);
            }
            catch
            { }
        }

        private void rbPowerOn_CheckedChanged(object sender, EventArgs e)
        {
            if (oDMX == null) return;
            if (oDMX.PowerSaving == null) oDMX.PowerSaving = new Byte[10];

            try
            {
                lbLCDelay.Visible = rbPowerOff.Checked;
                tbLCDelay.Visible = rbPowerOff.Checked;
                ls1.Visible = rbPowerOff.Checked;

                if (rbPowerOn.Checked)
                    oDMX.PowerSaving[0] = 0;
                else
                    oDMX.PowerSaving[0] = (Byte)tbLCDelay.Value;
            }
            catch
            { }
        }
    }
}
