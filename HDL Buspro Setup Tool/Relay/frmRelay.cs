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
    public partial class frmRelay : Form
    {
        [DllImport("user32.dll")]
        public static extern Boolean FlashWindow(IntPtr hwnd, Boolean bInvert);
        private int MyActivePage = 0; //按页面上传下载
        private Point Position = new Point(0,0);
        private Relay oRe = null;
        private string myDevName = null;
        private int mywdDevicerType = 0;

        private TimeText txtONDelay = new TimeText(".");
        private TimeText txtScene = new TimeText(":");
        private TimeMs txtSeries = new TimeMs();
        private TimeMs txtCurtainRuntime = new TimeMs();

        private Byte currentAreaId;
        private Byte currentSceneId;
        private Byte currentSerieId;

        private int mintIDIndex = -1;
        private int mintSeriesMode = 0;

        private int mintCopySeriesID = -1;
        private bool mblnIsShowing = true;
        private int myintRowIndex = -1; // 当前行列
        private int myintColIndex = -1;
        private bool MyBlnReading = false;
        private byte SubNetID;
        private byte DeviceID;
        private bool isSelectSeq = false;
        public frmRelay()
        {
            InitializeComponent();
        }

        public frmRelay(Relay oRelay, string strName, int intDIndex, int wdDeviceType)
        {
            InitializeComponent();

            this.oRe = oRelay;
            this.myDevName = strName;
            this.mintIDIndex = intDIndex;
            this.mywdDevicerType = wdDeviceType;
            string strDevName = strName.Split('\\')[0].ToString();

            HDLSysPF.DisplayDeviceNameModeDescription(strName, mywdDevicerType, cboDevice, tbModel, tbDescription);

            SubNetID = Convert.ToByte(strDevName.Split('-')[0]);
            DeviceID = Convert.ToByte(strDevName.Split('-')[1]);
            tsl3.Text = strName;
        }

        void InitialFormCtrlsTextOrItems()
        {
            cbType.Items.AddRange(CsConst.LoadType);

            cbSeqMode.Items.Clear();
            for (int i = 0; i < 5; i++)
                cbSeqMode.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "0018" + i.ToString(), ""));
            cbSeriesRepeat.Items.Clear();
            cbSeriesRepeat.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99895", ""));
            for (int intI = 0; intI < 99; intI++)
            {
                cbSeriesRepeat.Items.Add(intI + 1);
            }
            txtONDelay = new TimeText(".");
            DgChns.Controls.Add(txtONDelay);
            txtONDelay.TextChanged += txtONDelay_TextChanged;
            txtONDelay.Visible = false;

            cbSeqMode.Items.Clear();
            for (int i = 0; i < 5; i++)
                cbSeqMode.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "0018" + i.ToString(), ""));
            cbstep.Items.Clear();
            cbSeriesRepeat.Items.Clear();
            cbSeriesRepeat.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99895", ""));
            for (int intI = 0; intI < 99; intI++)
            {
                if (intI <= 12)
                    cbstep.Items.Add(intI.ToString());
                cbSeriesRepeat.Items.Add(intI + 1);
            }

            tvZone.HideSelection = false;
            //自已绘制
            this.tvZone.DrawMode = TreeViewDrawMode.OwnerDrawText;
            this.tvZone.DrawNode += new DrawTreeNodeEventHandler(treeView_DrawNode);

            tvSeries.HideSelection = false;
            //自已绘制
            this.tvSeries.DrawMode = TreeViewDrawMode.OwnerDrawText;
            this.tvSeries.DrawNode += new DrawTreeNodeEventHandler(treeView_DrawNode);

            toolStrip1.Visible = (CsConst.MyEditMode == 0);
            tvZone.HideSelection = false;
            //自已绘制
            this.tvZone.DrawMode = TreeViewDrawMode.OwnerDrawText;
            this.tvZone.DrawNode += new DrawTreeNodeEventHandler(treeView_DrawNode);

            tvSeries.HideSelection = false;
            //自已绘制
            this.tvSeries.DrawMode = TreeViewDrawMode.OwnerDrawText;
            this.tvSeries.DrawNode += new DrawTreeNodeEventHandler(treeView_DrawNode);

            dgvCurtain.Controls.Add(txtCurtainRuntime);
            txtCurtainRuntime.TextChanged += new EventHandler(txtCurtainRuntime_TextChangedForCurtain);
        }

        void txtCurtainRuntime_TextChangedForCurtain(object sender, EventArgs e)
        {
            if (oRe == null) return;
            if (oRe.Chans == null || oRe.Chans.Count == 0) return;

            if ((dgvCurtain.CurrentCell.RowIndex == -1) || (dgvCurtain.CurrentCell.ColumnIndex == -1)) return;
            if (txtCurtainRuntime.Visible)
            {
                dgvCurtain[2, dgvCurtain.CurrentCell.RowIndex].Value = HDLPF.GetStringFrmTimeMs(int.Parse(txtCurtainRuntime.Text.ToString()));
            }
            ModifyMultilinesIfNeeds(dgvCurtain[2, dgvCurtain.CurrentCell.RowIndex].Value.ToString(), 2, dgvCurtain);
        }

        private void frmRelay_Load(object sender, EventArgs e)
        {
            MyBlnReading = true;
            InitialFormCtrlsTextOrItems();
            MyBlnReading = false;
        }


        void SetCtrlsVisbleWithDifferentDeviceType()
        {
            if (oRe == null) return;

            Boolean bIsHasLoadType = !RelayDeviceTypeList.RelayWithOutLoadTypeReading.Contains(mywdDevicerType);
            Boolean bIshHasBroadcastFlag = RelayDeviceTypeList.RelayHasBroadcastFlag.Contains(mywdDevicerType); 
            Boolean NewRelayModule = RelayDeviceTypeList.RelayModuleV2StairGroup.Contains(mywdDevicerType);
            Boolean RelayHasCurtainGroup = RelayDeviceTypeList.RelayModuleWorkAsCurtainControlGroup.Contains(mywdDevicerType);
            Boolean HasOffDelay = RelayDeviceTypeList.RelayModuleV2OffDelay.Contains(mywdDevicerType);
            Boolean HasNoSequences = RelayDeviceTypeList.RelayWithoutSequences.Contains(mywdDevicerType);
            Boolean bIsHasAcFunction = RelayDeviceTypeList.RelayThreeChnBaseWithACFunction.Contains(mywdDevicerType);

            cbType.Visible = bIsHasLoadType;
            chbBroadcast.Visible = bIshHasBroadcastFlag;
            if (NewRelayModule != true) tab1.TabPages.Remove(tabStair);
            else
            {
                RelayExclusion temp = new RelayExclusion(this.mywdDevicerType, oRe, myDevName);
                Dock = DockStyle.Fill;
                tabStair.Controls.Add(temp);
            }

            clOffDelay.Visible = HasOffDelay;
            cl7.Visible = HasOffDelay;
            clEnable.Visible = RelayDeviceTypeList.RelayModuleWithButtonEnableGroup.Contains(mywdDevicerType);
            if (RelayHasCurtainGroup == false) tab1.TabPages.Remove(tabCurtain);
            if (HasNoSequences == true) tab1.TabPages.Remove(tp4);
            if (bIsHasAcFunction == false) tab1.TabPages.Remove(tabAC);
            else
            {
                tab1.TabPages.Remove(tabZone);
                tab1.TabPages.Remove(tabScenes);
            }
        }

        void LoadBasicInformationToForm()
        {
            try
            {
                if (oRe == null) return;
                if (oRe.Chans == null) return;

                this.Text = myDevName;
                tsl3.Text = myDevName;
                DgChns.Rows.Clear();

                foreach (RelayChannel ch in oRe.Chans)
                {
                    object[] boj = new object[] { ch.ID, ch.Remark,ch.bEnableChn, cbType.Items[ch.LoadType], 
                        HDLPF.GetStringFromTime(ch.OnDelay, "."), ch.ProtectDelay, 
                        HDLPF.GetStringFromTime(ch.OFFDelay, "."), ch.OFFProtectDelay, 
                        false};
                    DgChns.Rows.Add(boj);
                }
            }
            catch
            {
            }
        }

        private void DgChns_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {

        }

        void txtONDelay_TextChanged(object sender, EventArgs e)
        {
            if (MyBlnReading) return;
            if (oRe == null) return;
            if (oRe.Chans == null) return;

            if ((DgChns.CurrentCell.RowIndex == -1) || (DgChns.CurrentCell.ColumnIndex == -1)) return;
            if (txtScene.Visible)
            {
                if (DgChns.CurrentCell.ColumnIndex == 4 || DgChns.CurrentCell.ColumnIndex == 6)
                {
                    DgChns[DgChns.CurrentCell.ColumnIndex, DgChns.CurrentCell.RowIndex].Value = HDLPF.GetStringFromTime(int.Parse(txtONDelay.Text.ToString()), ".");
                    string str = DgChns[DgChns.CurrentCell.ColumnIndex, DgChns.CurrentCell.RowIndex].Value.ToString();
                    ModifyMultilinesIfNeeds(str, DgChns.CurrentCell.ColumnIndex, DgChns);
                }
            }
        }

        void ModifyMultilinesIfNeeds(string strTmp, int ColumnIndex, DataGridView dgv)
        {
            if (dgv.SelectedRows == null || dgv.SelectedRows.Count == 0) return;
            if (strTmp == null) strTmp = "";
            if (dgv.SelectedRows.Count > 1)
            {
                for (int i = 0; i < dgv.SelectedRows.Count; i++)
                {
                    dgv.SelectedRows[i].Cells[ColumnIndex].Value = strTmp;
                }
            }
        }

        private void DgChns_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (oRe == null) return;
                if (oRe.Chans == null) return;
                if ((e.RowIndex == -1) || (e.ColumnIndex == -1)) return;
                if (DgChns[e.ColumnIndex, e.RowIndex].Value == null) DgChns[e.ColumnIndex, e.RowIndex].Value = "";

                for (int i = 0; i < DgChns.SelectedRows.Count; i++)
                {
                    string strTmp = "";
                    switch (e.ColumnIndex)
                    {
                        case 1:
                            strTmp = DgChns[1, DgChns.SelectedRows[i].Index].Value.ToString();
                            DgChns[1, DgChns.SelectedRows[i].Index].Value = strTmp;
                            oRe.Chans[DgChns.SelectedRows[i].Index].Remark = DgChns[1, DgChns.SelectedRows[i].Index].Value.ToString();
                            break;
                        case 2:
                            strTmp = DgChns[2, DgChns.SelectedRows[i].Index].Value.ToString().ToLower();
                            DgChns[2, DgChns.SelectedRows[i].Index].Value = strTmp;
                            if (strTmp == "true") oRe.Chans[DgChns.SelectedRows[i].Index].bEnableChn = 1;
                            else oRe.Chans[DgChns.SelectedRows[i].Index].bEnableChn = 0;
                            break;
                        case 3:
                            oRe.Chans[DgChns.SelectedRows[i].Index].LoadType = cbType.Items.IndexOf(DgChns[3, DgChns.SelectedRows[i].Index].Value.ToString());
                            break;
                        case 4:
                            DgChns[4, DgChns.SelectedRows[i].Index].Value = HDLPF.GetStringFromTime(int.Parse(txtONDelay.Text.ToString()), ".");
                            oRe.Chans[DgChns.SelectedRows[i].Index].OnDelay = int.Parse(HDLPF.GetTimeFromString(DgChns[4, DgChns.SelectedRows[i].Index].Value.ToString(), '.'));
                            break;
                        case 5:
                            strTmp = DgChns[5, DgChns.SelectedRows[i].Index].Value.ToString();
                            DgChns[5, DgChns.SelectedRows[i].Index].Value = HDLPF.IsNumStringMode(strTmp, 0, 60);
                            oRe.Chans[DgChns.SelectedRows[i].Index].ProtectDelay = int.Parse(DgChns[5, DgChns.SelectedRows[i].Index].Value.ToString());
                            break;
                        case 6:
                            DgChns[6, DgChns.SelectedRows[i].Index].Value = HDLPF.GetStringFromTime(int.Parse(txtONDelay.Text.ToString()), ".");
                            oRe.Chans[DgChns.SelectedRows[i].Index].OFFDelay = int.Parse(HDLPF.GetTimeFromString(DgChns[6, DgChns.SelectedRows[i].Index].Value.ToString(), '.'));
                            break;
                        case 7:
                            strTmp = DgChns[7, DgChns.SelectedRows[i].Index].Value.ToString();
                            DgChns[7, DgChns.SelectedRows[i].Index].Value = HDLPF.IsNumStringMode(strTmp, 0, 60);
                            oRe.Chans[DgChns.SelectedRows[i].Index].OFFProtectDelay = int.Parse(DgChns[7, DgChns.SelectedRows[i].Index].Value.ToString());
                            break;
                        case 8:
                            string strName = myDevName.Split('\\')[0].ToString();
                            byte bytSubID = byte.Parse(strName.Split('-')[0]);
                            byte bytDevID = byte.Parse(strName.Split('-')[1]);

                            byte[] bytTmp = new byte[4];
                            bytTmp[0] = (byte)(DgChns.SelectedRows[i].Index + 1);
                            bytTmp[2] = 0;
                            bytTmp[3] = 0;

                            if (DgChns[8, DgChns.SelectedRows[i].Index].Value.ToString().ToLower() == "true") bytTmp[1] = 100;
                            else bytTmp[1] = 0;

                            Cursor.Current = Cursors.WaitCursor;
                            if (CsConst.mySends.AddBufToSndList(bytTmp, 0x0031, bytSubID, bytDevID, false, false, false, CsConst.minAllWirelessDeviceType.Contains(mywdDevicerType)) == false)
                                Cursor.Current = Cursors.Default;
                            break;
                    }
                    DgChns.SelectedRows[i].Cells[e.ColumnIndex].Value = DgChns[e.ColumnIndex, e.RowIndex].Value.ToString();
                }
            }
            catch
            {
            }
        }

        delegate void SetvalueHandle(int rowIndex);

        private void Setvalue(int rowIndex)
        {
            DgChns.CellValueChanged -= new DataGridViewCellEventHandler(DgChns_CellValueChanged);
            System.Threading.Thread.Sleep(50);
            List<int> values = new List<int>();

            for (int i = 0; i < DgChns.Rows.Count; i++)
            {
                if (DgChns[5, i].Value.ToString().ToLower() == "true")
                {
                    if (i != rowIndex)
                        values.Add(i);
                }
            }
            DgChns.Rows.Clear();
            LoadBasicInformationToForm();
            for (int i = 0; i < DgChns.Rows.Count; i++)
            {
                if (values.Contains(i))
                {
                    DgChns[5, i].Value = true;
                }
            }
            DgChns[0, 0].Selected = false;
            DgChns[5, rowIndex].Selected = true;

            DgChns.CellValueChanged += new DataGridViewCellEventHandler(DgChns_CellValueChanged);
        }

        private void tab1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CsConst.MyEditMode == 0)
            {
                UpdateDisplayInformationAccordingly(null, null);
            }
            else if (CsConst.MyEditMode == 1)
            {
                if (tab1.SelectedTab.Name == "tp1") MyActivePage = 1;
                else if (tab1.SelectedTab.Name == "tabZone") MyActivePage = 2;
                else if (tab1.SelectedTab.Name == "tabScenes") MyActivePage = 3;
                else if (tab1.SelectedTab.Name == "tp4") MyActivePage = 4;
                else if (tab1.SelectedTab.Name == "tabStair") MyActivePage = 5;
                else if (tab1.SelectedTab.Name == "tabAC") MyActivePage = 6;
                else if (tab1.SelectedTab.Name == "tabCurtain") MyActivePage = 7;
                
                if (oRe.MyRead2UpFlags[MyActivePage - 1] == false)
                {
                    tsbDown_Click(tsbDown, null);
                }
                else
                {
                    UpdateDisplayInformationAccordingly(null, null);
                }
            }            
        }

        void ShowAreasToListview(object sender, byte bytCh2Sce2Series)
        {
            try
            {
                ((TreeView)sender).Nodes.Clear();

                if (oRe == null) return;
                if (oRe.Areas == null) return;

                foreach (Relay.Area oArea in oRe.Areas)
                {
                    TreeNode OND = ((TreeView)sender).Nodes.Add(oArea.ID.ToString(), oArea.ID + "-" + oArea.Remark, 0, 0);

                    switch (bytCh2Sce2Series)
                    {
                        case 1:
                            if (oRe.Chans != null)
                            {
                                for (int intI = 0; intI < oRe.Chans.Count; intI++)
                                {
                                    if (oRe.Chans[intI].intBelongs == (oArea.ID))
                                        OND.Nodes.Add(null, (intI + 1).ToString() + "-" + oRe.Chans[intI].Remark, 1, 1);
                                }
                            }
                            break;
                        case 2:
                            if (oArea.Scen != null)
                            {
                                for (int intI = 0; intI < oArea.Scen.Count; intI++)
                                {
                                    OND.Nodes.Add(null, oArea.Scen[intI].ID + "-" + oArea.Scen[intI].Remark, 1, 1);
                                }
                            }
                            break;
                        case 3:
                            if (oArea.Seq != null)
                            {
                                for (int intI = 0; intI < oArea.Seq.Count; intI++)
                                {
                                    OND.Nodes.Add(null, oArea.Seq[intI].ID + "-" + oArea.Seq[intI].Remark, 1, 1);
                                }
                            }
                            break;
                    }
                }
                if (((TreeView)sender).Nodes != null && ((TreeView)sender).Nodes.Count > 0)
                {
                    TreeNode oNode = ((TreeView)sender).Nodes[currentAreaId];
                    oNode.ExpandAll();
                    if (bytCh2Sce2Series == 2)
                    {
                        tvScene_AfterSelect(sender, null);
                    }
                }
            }
            catch
            {
            }
        }

        void DeleteNodeInAreaForm(string strName)
        {
            int intIndex = int.Parse(strName.Split('-')[0].ToString());
            oRe.Chans[intIndex - 1].intBelongs = 0;
            resetChnsWaittingAllocation();
        }

        private void resetChnsWaittingAllocation()
        {
            dgvZoneChn1.Rows.Clear();
            for (int i = 0; i < oRe.Chans.Count; i++)
            {
                RelayChannel chnTmp = oRe.Chans[i];
                if (chnTmp.intBelongs == 0)
                {
                    string strChn = "";

                    strChn = (i + 1).ToString();
                    object[] obj = new object[] { strChn, chnTmp.Remark, false };
                    dgvZoneChn1.Rows.Add(obj);

                }
            }
        }

        int AddNewSceneToArea(TreeNode Tmp)
        {
            // 添加到缓存
            if (oRe.Areas[Tmp.Index].Scen == null)
            {
                oRe.Areas[Tmp.Index].Scen = new List<Relay.Scene>();
            }

            List<int> oTmp = new List<int>(); //取出所有已用的场景号
            for (int i = 0; i < oRe.Areas[Tmp.Index].Scen.Count; i++)
            {
                oTmp.Add(oRe.Areas[Tmp.Index].Scen[i].ID);
            }
            //查找位置，替换buffer
            int intSceID = 1;
            while (oTmp.Contains(intSceID))
            {
                intSceID++;
            }

            Relay.Scene oSce = new Relay.Scene();
            oSce.ID = byte.Parse(intSceID.ToString());
            oSce.Remark = "Scene";
            oSce.light = new byte[oRe.Chans.Count].ToList();
            oSce.Time = 0;
            oRe.Areas[Tmp.Index].Scen.Add(oSce);

            return intSceID;
        }

        private void tvSeries_MouseDown(object sender, MouseEventArgs e)
        {
            isSelectSeq = true;
            txtSeries.Visible = false;
            TreeNode oNode = tvSeries.GetNodeAt(e.Location);
        }

        void ShowSequenceSceneInformation(TreeNode oNode)
        {
            if (oNode == null) return;
            if (oRe.Areas == null) return;

            bool blnIsNewSqe = false;
            #region
            if (oNode.Level == 0)
            {
                blnIsNewSqe = true;
                currentAreaId = byte.Parse(oNode.Text.Split('-')[0].ToString());
            }
            else
            {
                byte bytCurSqeID = byte.Parse(oNode.Parent.Text.Split('-')[0].ToString());
                if (bytCurSqeID != currentAreaId)
                {
                    blnIsNewSqe = true;
                    currentAreaId = byte.Parse(oNode.Parent.Text.Split('-')[0].ToString());
                }
            }

            Relay.Area oArea = null;
            foreach (Relay.Area Tmp in oRe.Areas)
            {
                if (Tmp.ID == currentAreaId)
                {
                    oArea = Tmp;
                    break;
                }
            }
            if (oArea == null) return;

            #endregion
            DgvSeries.Rows.Clear();
            if (blnIsNewSqe && oArea.Scen != null) // 选择的区域
            {
                SelectScens.Items.Clear();
                if (oArea.Scen != null)
                {
                    foreach (Relay.Scene oSce in oArea.Scen)
                    {
                        SelectScens.Items.Add(oSce.ID + "-" + oSce.Remark);
                    }
                }
            }

            if (oNode.Level != 0)
            {
                TreeNode Tmp = oNode.Parent;
                if (oArea.Scen == null) return;
                if (oArea.Scen.Count == 0) return;
                if (oArea.Seq == null) return;

                currentSerieId = (Byte)oNode.Index;

                tbRemarkSeries.Text = oNode.Text.Split('-')[1].ToString();

                Relay.Sequence oSeries = oArea.Seq[oNode.Index];

                cbSeriesRepeat.SelectedIndex = oSeries.Times;

                if (oSeries.Mode == 4) cbSeqMode.SelectedIndex = 0;
                else if (oSeries.Mode == 3) cbSeqMode.SelectedIndex = 1;
                else if (oSeries.Mode == 2) cbSeqMode.SelectedIndex = 2;
                else if (oSeries.Mode == 1) cbSeqMode.SelectedIndex = 3;
                else if (oSeries.Mode == 0) cbSeqMode.SelectedIndex = 4;
                else cbSeqMode.SelectedIndex = 0;
                cbstep.Items.Clear();
                int wdMaxValue = DeviceTypeList.GetMaxValueFromPublicModeGroup(mywdDevicerType);
                for (int i = 0; i <= wdMaxValue; i++)
                    cbstep.Items.Add((i).ToString());
                cbstep.Text = oSeries.Steps.ToString();
                ShowSequenceStepsToForm(oArea, oSeries);
            }
            mblnIsShowing = false;
            isSelectSeq = false;
        }

        private void ShowSequenceStepsToForm(Relay.Area oArea, Relay.Sequence oSeries)
        {
            if (oSeries == null) return;
            DgvSeries.Rows.Clear();
            for (int intI = 0; intI < oSeries.Steps; intI++)
            {
                bool blnAdded = false;
                for (int intJ = 0; intJ < oArea.Scen.Count; intJ++)
                {
                    if (oArea.Scen[intJ].ID == oSeries.SceneIDs[intI]) //选择场景显示
                    {
                        string strRemark = oArea.Scen[intJ].Remark;
                        object[] obj = { intI + 1, oSeries.SceneIDs[intI] + "-" + strRemark,
                                    HDLPF.GetStringFrmTimeMs(oSeries.RunTimeH[intI] * 256 + oSeries.RunTimeL[intI])};
                        DgvSeries.Rows.Add(obj);
                        blnAdded = true;
                        break;
                    }
                }
                if (blnAdded == false)
                {
                    object[] obj = { intI + 1, oArea.Scen[0].ID+"-"+oArea.Scen[0].Remark,
                                     HDLPF.GetStringFrmTimeMs(oSeries.RunTimeH[intI] * 256 + oSeries.RunTimeL[intI])};
                    DgvSeries.Rows.Add(obj);
                }
            }
        }


        int AddNewSeriesToForm(byte bytID, int intAreaID)  // default is 0, paste is 1
        {
            // 获取一个未使用的序列号
            if (oRe.Areas[intAreaID].Seq == null)
            {
                oRe.Areas[intAreaID].Seq = new List<Relay.Sequence>();
            }

            List<int> TmpSerID = new List<int>(); //取出所有已用的场景号
            for (int i = 0; i < oRe.Areas[intAreaID].Seq.Count; i++)
            {
                TmpSerID.Add(oRe.Areas[intAreaID].Seq[i].ID);
            }
            //查找位置，替换buffer
            int intSceID = 1;
            while (TmpSerID.Contains(intSceID))
            {
                intSceID++;
            }

            Relay.Sequence oSeries = new Relay.Sequence();

            if (bytID == 0)  //默认添加
            {

                int intNeedAdd = cbstep.SelectedIndex + 1;
                if (DgvSeries.RowCount != 0)
                {
                    intNeedAdd = cbstep.SelectedIndex + 1 - DgvSeries.RowCount;
                }

                for (int intI = 0; intI < intNeedAdd; intI++)
                {
                    string strRemark = oRe.Areas[intAreaID].Scen[0].Remark;
                    object[] obj = { DgvSeries.RowCount + 1, SelectScens.Items[0], "0:0.0" };
                    DgvSeries.Rows.Add(obj);
                }

                //更新到缓存
                oSeries.ID = byte.Parse(intSceID.ToString());
                oSeries.Remark = tbRemarkSeries.Text;
                oSeries.Steps = byte.Parse((cbstep.SelectedIndex).ToString());
                oSeries.Times = byte.Parse(cbSeriesRepeat.SelectedIndex.ToString());
                oSeries.Mode = byte.Parse(mintSeriesMode.ToString());
                oSeries.bytLastStep = 0;

                oSeries.SceneIDs = new List<byte>();
                oSeries.RunTimeH = new List<byte>();
                oSeries.RunTimeL = new List<byte>();

                for (int intI = 0; intI < oSeries.Steps; intI++)
                {
                    oSeries.SceneIDs.Add(0);
                    oSeries.RunTimeH.Add(0);
                    oSeries.RunTimeL.Add(0);
                }
            }
            else if (bytID == 1)  // 复制的其他序列的设置
            {
                //更新到缓存
                oSeries = (Relay.Sequence)oRe.Areas[intAreaID].Seq[mintCopySeriesID].Clone();
                oSeries.ID = byte.Parse(intSceID.ToString());
            }

            //添加到窗体
            TreeNode oTmp = tvSeries.Nodes[intAreaID];
            oTmp = oTmp.Nodes.Add(null,intSceID.ToString() + "-" + oSeries.Remark,1,1);
            tvSeries.SelectedNode = oTmp;

            if (oRe.Areas[intAreaID].Seq == null)
            {
                oRe.Areas[intAreaID].Seq = new List<Relay.Sequence>();
            }
            oRe.Areas[intAreaID].Seq.Add(oSeries);

            currentAreaId = (Byte)(oRe.Areas[intAreaID].Seq.Count - 1);

            return intSceID;
        }

        private void rb1_Click(object sender, EventArgs e)
        {

        }

        private void rb1_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked == true)
            {
                mintSeriesMode = int.Parse(((RadioButton)sender).Tag.ToString());
            }
        }

        private void DgvSeries_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (e.ColumnIndex == 2)
            {
                DgvSeries.Controls.Add(txtSeries);
                txtSeries.Text = HDLPF.GetTimeFrmStringMs(DgvSeries[2, e.RowIndex].Value.ToString());
                txtSeries.Show();
                txtSeries.Visible = true;
                Rectangle rect = DgvSeries.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true);
                txtSeries.Size = rect.Size;
                txtSeries.Top = rect.Top;
                txtSeries.Left = rect.Left;
                txtSeries.TextChanged += new EventHandler(txtSeries_TextChanged);
            }
            else
            {
                txtSeries.Visible = false;
            }
        }

        void txtSeries_TextChanged(object sender, EventArgs e)
        {
            if ((DgvSeries.CurrentCell.RowIndex == -1) || (DgvSeries.CurrentCell.ColumnIndex == -1)) return;
            if (txtSeries.Visible)
            {
                DgvSeries[2, DgvSeries.CurrentCell.RowIndex].Value = HDLPF.GetStringFrmTimeMs(int.Parse(txtSeries.Text.ToString()));
                // DgMode_CellValueChanged(DgMode, e);
            }
            //throw new NotImplementedException();
        }

        private void DgvSeries_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (mblnIsShowing) return;
            if (e.RowIndex == -1) return;
            if (e.ColumnIndex == -1) return;
            if (DgvSeries.RowCount == 0) return;
            if (tvSeries.SelectedNode == null || tvSeries.SelectedNode.Parent == null)
                return;
            try
            {
                int intAreaID = tvSeries.SelectedNode.Parent.Index;
                int intIndex = e.RowIndex;

                if (e.ColumnIndex == 1)
                {
                    int intTmp = int.Parse(DgvSeries[1, e.RowIndex].Value.ToString().Split('-')[0].ToString());
                    oRe.Areas[intAreaID].Seq[currentSerieId].SceneIDs[intIndex] = byte.Parse(intTmp.ToString());
                }
                else if (e.ColumnIndex == 2)
                {
                    DgvSeries[2, e.RowIndex].Value = HDLPF.GetStringFrmTimeMs(int.Parse(txtSeries.Text.ToString()));
                    oRe.Areas[intAreaID].Seq[currentSerieId].RunTimeH[intIndex] = byte.Parse((int.Parse(txtSeries.Text) / 256).ToString());
                    oRe.Areas[intAreaID].Seq[currentSerieId].RunTimeL[intIndex] = byte.Parse((int.Parse(txtSeries.Text) % 256).ToString());
                }
            }
            catch { }
        }

        private void DgChns_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            if (oRe == null) return;
            Cursor.Current = Cursors.WaitCursor;
            oRe.SaveInfoToDb();
            Cursor.Current = Cursors.Default;
        }


        void SetVisableForDownOrUpload(bool blnIsEnable)
        {
            tsbDown.Enabled = blnIsEnable;
            toolStripLabel2.Enabled = blnIsEnable;

            if (blnIsEnable == true && tsbar.Value != 0)
            {
                HintScene.Visible = true;
                HintScene.Text = "";
            }
            else
            {
                tsbar.Value = 0;
                tsbl4.Text = "0";
                HintScene.Visible = false;
            }

            if (tsbar.Value == 100) HintScene.Text = "Fully Success!";
            tsbar.Visible = !blnIsEnable;
            tsbl4.Visible = !blnIsEnable;
        }

        private void DgChns_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            DgChns.CommitEdit(DataGridViewDataErrorContexts.Commit);
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
                DropNode.Nodes.Add(DragNode);
                int intLoadID = int.Parse(DragNode.Text.Split('-')[0].ToString());
                oRe.Chans[intLoadID - 1].intBelongs = int.Parse(DropNode.Text.Split('-')[0].ToString());
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

        private void toolStripButton11_Click(object sender, EventArgs e)
        {
            if (oRe == null) return;
        }

        private void tmSame_Click(object sender, EventArgs e)
        {
            DataGridView oDG = null;
            switch (tab1.SelectedIndex)
            {
                case 0: oDG = DgChns; break;
            }

            if (oDG.SelectedCells == null) return;
            if (oDG.SelectedRows == null) return;
            if (myintRowIndex == -1) return;
            if (myintColIndex < 1) return;
            string strTmp = oDG[myintColIndex, myintRowIndex].Value.ToString();

            int intTag = Convert.ToInt16((sender as ToolStripMenuItem).Tag.ToString());
            UpdateBufferWhenchanged(intTag, strTmp, oDG);
        }

        void UpdateBufferWhenchanged(int intTag, string strTmp, DataGridView oDGV)
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

        private void button5_Click(object sender, EventArgs e)
        {
            if (oRe == null) return;
            if (oRe.Areas == null) return;
            if (tvSeries.SelectedNode == null) return;
            if (tvSeries.SelectedNode.Level == 0) return;

            byte bytAreaID = Convert.ToByte(tvSeries.SelectedNode.Parent.Text.Split('-')[0].ToString());
            byte bytSceID = byte.Parse(tvSeries.SelectedNode.Text.Split('-')[0].ToString());

            string strName = myDevName.Split('\\')[0].ToString();
            byte bytSubID = byte.Parse(strName.Split('-')[0]);
            byte bytDevID = byte.Parse(strName.Split('-')[1]);

            byte[] bytTmp = new byte[2];
            bytTmp[0] = bytAreaID;
            if (((Button)sender).Tag == null || ((Button)sender).Tag.ToString() != "0")
            {
                bytTmp[1] = bytSceID;
                ((Button)sender).Tag = "0";
            }
            else if (((Button)sender).Tag.ToString() == "0")
            {
                bytTmp[1] = 0;
                ((Button)sender).Tag = null;
            }
            CsConst.mySends.AddBufToSndList(bytTmp, 0x001A, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(mywdDevicerType));
        }




        private void tsbDown_Click(object sender, EventArgs e)
        {
            try
            {
                txtONDelay.Visible = false;
                byte bytTag = Convert.ToByte(((ToolStripButton)sender).Tag.ToString());
                bool blnShowMsg = (CsConst.MyEditMode != 1);
                if (HDLPF.UploadOrReadGoOn(this.Text, bytTag, blnShowMsg))
                {
                    Cursor.Current = Cursors.WaitCursor;
                    // ReadDownLoadThread();  //增加线程，使得当前窗体的任何操作不被限制

                    CsConst.MyUPload2DownLists = new List<byte[]>();

                    string strName = myDevName.Split('\\')[0].ToString();
                    byte bytSubID = byte.Parse(strName.Split('-')[0]);
                    byte bytDevID = byte.Parse(strName.Split('-')[1]);

                    byte[] ArayRelay = new byte[] { bytSubID, bytDevID, (byte)(mywdDevicerType / 256), (byte)(mywdDevicerType % 256)
                        , (byte)(MyActivePage),(byte)(mintIDIndex/256),(byte)(mintIDIndex%256)  };
                    CsConst.MyUPload2DownLists.Add(ArayRelay);
                    CsConst.MyUpload2Down = Convert.ToByte(((ToolStripButton)sender).Tag.ToString());
                    FrmDownloadShow Frm = new FrmDownloadShow();
                    if (CsConst.MyUpload2Down==0) Frm.FormClosing += new FormClosingEventHandler(UpdateDisplayInformationAccordingly);
                    Frm.ShowDialog();
                }
            }
            catch
            {
            }
        }


        void UpdateDisplayInformationAccordingly(object sender, FormClosingEventArgs e)
        {
            #region
            if (tab1.SelectedTab.Name == "tp1")
            {
                DgChns.Rows.Clear();
                LoadBasicInformationToForm();
            }
            else if (tab1.SelectedTab.Name == "tabZone")
            {
                ShowAreasToListview(tvZone, 1);
                resetChnsWaittingAllocation();
            }
            else if (tab1.SelectedTab.Name == "tabScenes")
            {
                ShowAreasToListview(tvScene, 2);
               // showSenceInfo();
            }
            else if (tab1.SelectedTab.Name == "tp4")
            {//显示区域
                ShowAreasToListview(tvSeries, 3);
            }
            else if (tab1.SelectedTab.Name == "tabAC")
            {
                showSlaveInfo();
            }
            else if (tab1.SelectedTab.Name == "tabStair")
            {
                showNewFunction();
            }
            else if (tab1.SelectedTab.Name == "tabCurtain")
            {
                showCurtainsInfo();
            }
            #endregion
            Cursor.Current = Cursors.Default;
            this.BringToFront();
        }

        private void showNewFunction()
        {
            tabStair.Controls.Clear();
            RelayExclusion temp = new RelayExclusion(this.mywdDevicerType, oRe, myDevName);
            Dock = DockStyle.Fill;
            tabStair.Controls.Add(temp);
        }

        private void showSlaveInfo()
        {
            try
            {
                if (oRe == null) return;
                if (oRe.SlaveInfo == null) return;
                if (oRe.SlaveInfo.Count <= 0) return;
                MyBlnReading = true;
                byte[] arayTmp = oRe.SlaveInfo[0];
                if (arayTmp[0] == 1) chbMaster.Checked = true;
                else chbMaster.Checked = false;
                dgvSlave.Rows.Clear();
                for (int i = 0; i < 8; i++)
                {
                    object[] obj = new object[] { dgvSlave.RowCount + 1, arayTmp[i * 2 + 1].ToString(), arayTmp[i * 2 + 2].ToString() };
                    dgvSlave.Rows.Add(obj);
                }
            }
            catch
            {
            }
            MyBlnReading = false;
        }


        private void showCurtainsInfo()
        {
            try
            {
                txtCurtainRuntime.Visible = false;
                if (oRe == null) return;
                if (oRe.Chans == null || oRe.Chans.Count ==0) return;
                MyBlnReading = true;
                dgvCurtain.Rows.Clear();

                Byte maxCurtainCount = Convert.ToByte(oRe.Chans.Count / 2);
                for (int i = 1; i <= maxCurtainCount; i++)
                {
                    Boolean bIsEnableCurtain = (oRe.Chans[(i - 1) * 2].bEnableCurtain == 1);
                    int Runtime = oRe.Chans[(i - 1) *2].onTime;
                    string strHint = i.ToString() + "(" + CsConst.mstrINIDefault.IniReadValue("public", "00000", "") + " " +
                        ((maxCurtainCount - i + 1) * 2 - 1).ToString() + " & " + ((maxCurtainCount - i + 1) * 2).ToString() + ")";

                    object[] obj = new object[] { strHint, bIsEnableCurtain, HDLPF.GetStringFrmTimeMs(Runtime) };
                    dgvCurtain.Rows.Add(obj);
                }
            }
            catch
            {
            }
            MyBlnReading = false;
        }

        private void showSenceInfo()
        {
            try
            {
                if (oRe == null) return;
                if (oRe.Areas == null) return;

                if (oRe.Areas.Count > 0)
                {
                    bool isEmpty = false;
                    for (int i = 0; i < oRe.Areas.Count; i++)
                    {
                        Relay.Area temp = oRe.Areas[i];
                        if (temp.Scen == null || temp.Scen.Count == 0)
                        {
                            isEmpty = true;
                            break;
                        }
                    }
                    if (isEmpty)
                    {
                        tsbDown_Click(tsbDown, null);
                    }
                    else
                    {
                       // if (cbSence.SelectedIndex < 0) cbSence.SelectedIndex = 0;
                        cbSence_SelectedIndexChanged(null, null);
                    }
                }
            }
            catch
            {
            }
        }

        private void showZoneInfo()
        {
            try
            {
                tvZone.Nodes.Clear();
                dgvZoneChn1.Rows.Clear();
                if (oRe == null) return;
                if (oRe.Areas != null && oRe.Areas.Count > 0)
                {
                    foreach (Relay.Area oArea in oRe.Areas)
                    {
                        TreeNode OND = tvZone.Nodes.Add(oArea.ID.ToString(), (tvZone.Nodes.Count + 1).ToString() + "-" + oArea.Remark, 1, 1);

                        if (oRe.Chans != null)
                        {
                            for (int intI = 0; intI < oRe.Chans.Count; intI++)
                            {
                                if (oRe.Chans[intI].intBelongs == (oArea.ID))
                                {
                                    OND.Nodes.Add(null, (intI + 1).ToString() + "-" + oRe.Chans[intI].Remark, 0, 0);
                                }
                            }
                        }
                    }
                }

                if (oRe.Chans != null && oRe.Chans.Count > 0)
                {
                    for (int i = 0; i < oRe.Chans.Count; i++)
                    {
                        RelayChannel chnTmp = oRe.Chans[i];
                        if (chnTmp.intBelongs == 0)
                        {
                            string strChn = "";
                            strChn = (i + 1).ToString();
                            object[] obj = new object[] { strChn, chnTmp.Remark, false };
                            dgvZoneChn1.Rows.Add(obj);
                        }
                    }
                }
                if (tvZone.Nodes.Count > 0) tvZone.SelectedNode = tvZone.Nodes[0];//选中
                lb22.Text = tvZone.Nodes.Count.ToString();
                lb11.Text = dgvZoneChn1.Rows.Count.ToString();
            }
            catch
            {
            }
        }


        private void treeView_DrawNode(object sender, DrawTreeNodeEventArgs e)
        {
            try
            {
                if ((e.State & TreeNodeStates.Selected) != 0)
                {

                    e.Graphics.FillRectangle(Brushes.Blue, e.Bounds.X - 5, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
                    Font nodeFont = e.Node.NodeFont;
                    if (nodeFont == null) nodeFont = ((TreeView)sender).Font;

                    // Draw the node text.
                    e.Graphics.DrawString(e.Node.Text, nodeFont, Brushes.White,
                        Rectangle.Inflate(new Rectangle(e.Bounds.X, e.Bounds.Y, e.Bounds.Width + 10, e.Bounds.Height), 5, -6));
                }
                else
                {
                    e.DrawDefault = false;
                    e.Graphics.DrawString(e.Node.Text, ((TreeView)sender).Font, Brushes.Black,
                        Rectangle.Inflate(new Rectangle(e.Bounds.X, e.Bounds.Y, e.Bounds.Width + 10, e.Bounds.Height), 5, -6));
                }

                /*if ((e.State & TreeNodeStates.Focused) != 0)
                {
                    using (Pen focusPen = new Pen(Color.Black))
                    {
                        focusPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
                        e.Graphics.DrawRectangle(focusPen, e.Bounds.X - 5, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
                    }
                }*/
            }
            catch
            {
            }
        }

        private void frmRelay_Shown(object sender, EventArgs e)
        {
            SetCtrlsVisbleWithDifferentDeviceType();
            if (CsConst.MyEditMode == 0)
            {
                LoadBasicInformationToForm();
            }
            else if (CsConst.MyEditMode == 1) //在线模式
            {
                MyActivePage = 1;
                if (oRe.MyRead2UpFlags[0] == false)
                {
                    tsbDown_Click(tsbDown, null);
                }
                else
                {
                    LoadBasicInformationToForm();
                }

                if (chbBroadcast.Visible == true)
                {
                    if (CsConst.mySends.AddBufToSndList(null, 0x1806, SubNetID, DeviceID, false, false, true, CsConst.minAllWirelessDeviceType.Contains(mywdDevicerType)) == true)
                    {
                        oRe.bytRepartS = CsConst.myRevBuf[25];
                        chbBroadcast.Visible = true;
                        chbBroadcast.Checked = (oRe.bytRepartS == 1);
                        CsConst.myRevBuf = new byte[1200];
                        HDLUDP.TimeBetwnNext(1);
                    }
                    else
                    {
                        chbBroadcast.Visible = false;
                    }
                }
            }
        }

        private void tmUpload_Click(object sender, EventArgs e)
        {
            tsbDown_Click(toolStripLabel2, null);
        }

        private void tmRead_Click_1(object sender, EventArgs e)
        {
            tsbDown_Click(tsbDown, null);
        }

        private void DgvSeries_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (mblnIsShowing) return;
            if (e.RowIndex == -1) return;
            if (e.ColumnIndex == -1) return;
            if (DgvSeries.RowCount == 0) return;
            if (DgvSeries.SelectedRows == null || DgvSeries.SelectedRows.Count == 0) return;

            int intAreaID = tvSeries.SelectedNode.Parent.Index;

            for (int i = 0; i < DgvSeries.SelectedRows.Count; i++)
            {
                DgvSeries.SelectedRows[i].Cells[e.ColumnIndex].Value = DgvSeries[e.ColumnIndex, e.RowIndex].Value.ToString();

                int intIndex = DgvSeries.SelectedRows[i].Index;

                if (e.ColumnIndex == 1)
                {
                    int intTmp = int.Parse(DgvSeries[1, e.RowIndex].Value.ToString().Split('-')[0].ToString());
                    oRe.Areas[intAreaID].Seq[currentSerieId].SceneIDs[intIndex] = byte.Parse(intTmp.ToString());
                }
                else if (e.ColumnIndex == 2)
                {
                    DgvSeries[2, e.RowIndex].Value = HDLPF.GetStringFrmTimeMs(int.Parse(txtSeries.Text.ToString()));
                    oRe.Areas[intAreaID].Seq[currentSerieId].RunTimeH[intIndex] = byte.Parse((int.Parse(txtSeries.Text) / 256).ToString());
                    oRe.Areas[intAreaID].Seq[currentSerieId].RunTimeL[intIndex] = byte.Parse((int.Parse(txtSeries.Text) % 256).ToString());
                }
            }
        }

        private void chbBroadcast_CheckedChanged(object sender, EventArgs e)
        {
            if (oRe == null) return;
            if (CsConst.MyEditMode == 1)
            {
                byte[] ArayTmp=new byte[1];
                if (chbBroadcast.Checked) ArayTmp[0] = 1;
                else ArayTmp[0] = 0;
                Cursor.Current = Cursors.WaitCursor;
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1808, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(mywdDevicerType)) == true)
                {
                    if (chbBroadcast.Checked) oRe.bytRepartS = 1;
                    else oRe.bytRepartS = 0;
                }
                Cursor.Current = Cursors.Default;
            }
        }


        private void btnAddZone_Click(object sender, EventArgs e)
        {
            if (oRe == null || oRe.Chans == null) return;
            if (tvZone.Nodes.Count >= oRe.Chans.Count) return;
            if (dgvZoneChn1.RowCount <= 0) return;
            int ID = 1;
            int max = 1;
            int min = 1;
            if (oRe.Areas == null) oRe.Areas = new List<Relay.Area>();
            if (oRe.Areas.Count > 0)
            {
                foreach (Relay.Area a in oRe.Areas)
                {
                    if (a.ID >= max)
                        max = a.ID;
                    if (a.ID < min && a.ID != 0)
                        min = a.ID;
                }
                if (min == max)
                {
                    if (min == 1)
                    {
                        ID = 2;
                    }
                    else
                    {
                        ID = 1;
                    }
                }
                else if (max > min)
                {
                    for (int i = min; i <= max; i++)
                    {
                        bool isSure = true;
                        for (int j = 0; j < oRe.Areas.Count; j++)
                        {
                            if (oRe.Areas[j].ID == i)
                            {
                                isSure = false;
                                break;
                            }
                        }
                        if (isSure)
                        {
                            ID = i;
                            break;
                        }
                    }
                    if (ID <= max)
                    {
                        ID = max + 1;
                    }
                }
            }
            else
            {
                ID = 1;
            }

            Relay.Area temp = new Relay.Area();
            temp.ID = Convert.ToByte(ID);
            temp.Remark = txtZoneRemark.Text;
            temp.Scen = new List<Relay.Scene>();
            temp.Seq = new List<Relay.Sequence>();
            for (int j = 0; j <= 12; j++)
            {
                Relay.Scene oSce = new Relay.Scene();
                oSce.ID = (byte)j;
                oSce.Remark = "Scene" + j.ToString();
                oSce.light = new byte[oRe.Chans.Count].ToList();
                oSce.Time = 0;
                temp.Scen.Add(oSce);
            }
            oRe.MyRead2UpFlags[5] = true;
            temp.bytDefaultSce = 0;
            oRe.Areas.Add(temp);
            tvZone.Nodes.Add(ID.ToString(), (tvZone.Nodes.Count + 1).ToString() + "-" + temp.Remark, 1, 1);
            lb22.Text = tvZone.Nodes.Count.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (tvZone.Nodes == null) return;
            if (oRe == null) return;
            if (oRe.Areas == null) return;
            if (tvZone.SelectedNode == null) return;
            TreeNode node = tvZone.SelectedNode;
            if (node.Level == 0)
            {
                int intAreaID = tvZone.SelectedNode.Index;
                for (int i = intAreaID + 1; i < oRe.Areas.Count; i++)
                {
                    for (int j = 0; j < oRe.Chans.Count; j++)
                    {
                        if (oRe.Chans[j].intBelongs == oRe.Areas[i].ID)
                        {
                            oRe.Chans[j].intBelongs = Convert.ToByte(oRe.Areas[i].ID - 1);
                        }
                    }
                    oRe.Areas[i].ID = Convert.ToByte(oRe.Areas[i].ID - 1);
                }
                oRe.Areas.RemoveAt(intAreaID);

                foreach (TreeNode Tmp in tvZone.SelectedNode.Nodes)
                {
                    DeleteNodeInAreaForm(Tmp.Text);
                }
                tvZone.SelectedNode.Remove();

                for (int i = 0; i < tvZone.Nodes.Count; i++)
                {
                    string str = tvZone.Nodes[i].Text.Split('-')[1];
                    tvZone.Nodes[i].Text = (i + 1).ToString() + "-" + str;
                }
                lb22.Text = tvZone.Nodes.Count.ToString();
            }
            oRe.MyRead2UpFlags[5] = true;
        }

        private void btn1_Click(object sender, EventArgs e)
        {
            if (tvZone.SelectedNode != null && tvZone.SelectedNode.Level == 0)
            {
                foreach (DataGridViewRow Tmp in dgvZoneChn1.SelectedRows)
                {
                    if (Tmp.Selected)  // 确定被选中，添加至区域列表
                    {
                        int intLoadID = int.Parse(Tmp.Cells[0].Value.ToString());
                        oRe.Chans[intLoadID - 1].intBelongs = Convert.ToByte(tvZone.SelectedNode.Name.ToString());
                        tvZone.SelectedNode.Nodes.Insert(0, null, Tmp.Cells[0].Value.ToString() + "-" + oRe.Chans[intLoadID - 1].Remark, 0, 0);
                        dgvZoneChn1.Rows.Remove(Tmp);
                    }
                }
                SortNodesByText(tvZone.SelectedNode);
            }
            else if (tvZone.SelectedNode != null && tvZone.SelectedNode.Level == 1)
            {
                foreach (DataGridViewRow Tmp in dgvZoneChn1.SelectedRows)
                {
                    if (Tmp.Selected)  // 确定被选中，添加至区域列表
                    {
                        int intLoadID = int.Parse(Tmp.Cells[0].Value.ToString());
                        oRe.Chans[intLoadID - 1].intBelongs = Convert.ToByte(tvZone.SelectedNode.Parent.Name.ToString());
                        tvZone.SelectedNode.Parent.Nodes.Insert(0, null, Tmp.Cells[0].Value.ToString() + "-" + oRe.Chans[intLoadID - 1].Remark, 0, 0);
                        dgvZoneChn1.Rows.Remove(Tmp);
                    }
                }
                SortNodesByText(tvZone.SelectedNode.Parent);
            }
            lb11.Text = dgvZoneChn1.Rows.Count.ToString();
            oRe.MyRead2UpFlags[5] = true;
        }

        private void SortNodesByText(TreeNode node)
        {
            TreeNodeCollection NodeCollection = node.Nodes;
            TreeNode[] NodeAry = new TreeNode[NodeCollection.Count];
            int[,] arayChannelIDAndIndex = new int[2, NodeCollection.Count];
            for (int i = 0; i < NodeCollection.Count; i++)
            {
                arayChannelIDAndIndex[0, i] = Convert.ToInt32(NodeCollection[i].Text.Split('-')[0].ToString());
                arayChannelIDAndIndex[1, i] = i;
                NodeAry[i] = new TreeNode();
                NodeAry[i] = NodeCollection[i];
            }
            for (int i = 0; i < arayChannelIDAndIndex.Length / 2; i++)
            {
                int temp1 = 0;
                int temp2 = 0;
                temp1 = arayChannelIDAndIndex[0, i];
                temp2 = arayChannelIDAndIndex[1, i];
                for (int j = i + 1; j < arayChannelIDAndIndex.Length / 2; j++)
                {
                    int temp3 = arayChannelIDAndIndex[0, j];
                    int temp4 = arayChannelIDAndIndex[1, j];
                    if (arayChannelIDAndIndex[0, j] < arayChannelIDAndIndex[0, i])
                    {
                        arayChannelIDAndIndex[0, i] = temp3;
                        arayChannelIDAndIndex[1, i] = temp4;
                        arayChannelIDAndIndex[0, j] = temp1;
                        arayChannelIDAndIndex[1, j] = temp2;
                    }
                }
            }
            node.Nodes.Clear();
            for (int i = 0; i < arayChannelIDAndIndex.Length / 2; i++)
            {
                node.Nodes.Add(NodeAry[arayChannelIDAndIndex[1, i]]);
            }
        }

        private void btn2_Click(object sender, EventArgs e)
        {
            if (tvZone.Nodes == null) return;
            if (oRe == null) return;
            if (oRe.Areas == null) return;
            if (tvZone.SelectedNode == null) return;
            TreeNode node = tvZone.SelectedNode;
            if (node.Level == 1)
            {
                DeleteNodeInAreaForm(node.Text);
                if (tvZone.SelectedNode.Parent.Nodes.Count == 1)
                {
                    DialogResult result = MessageBox.Show("Do you want to remove the blank area as well?", "Warm Hint"
                                                , MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                    if (result == DialogResult.OK)
                    {
                        int intAreaID = tvZone.SelectedNode.Parent.Index;
                        oRe.Areas.RemoveAt(intAreaID);
                        tvZone.SelectedNode.Parent.Remove();
                    }
                }
                else tvZone.SelectedNode.Remove();
            }
            else if (node.Level == 0)
            {
                foreach (TreeNode Tmp in tvZone.SelectedNode.Nodes)
                {
                    DeleteNodeInAreaForm(Tmp.Text);
                }
                tvZone.SelectedNode.Nodes.Clear();
            }
            lb11.Text = dgvZoneChn1.Rows.Count.ToString();
            oRe.MyRead2UpFlags[5] = true;
        }

        private void tvZone_ItemDrag(object sender, ItemDragEventArgs e)
        {
            DoDragDrop(e.Item, DragDropEffects.Move);
        }

        private void tvZone_DragDrop(object sender, DragEventArgs e)
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
                DropNode.Nodes.Add(DragNode);
                int intLoadID = int.Parse(DragNode.Text.Split('-')[0].ToString());
                oRe.Chans[intLoadID - 1].intBelongs = byte.Parse(DropNode.Name.ToString());
            }
            if (DropNode == null) return;
            // 如果目标节点不存在，即拖拽的位置不存在节点，那么就将被拖拽节点放在根节点之
        }

        private void tvZone_DragEnter(object sender, DragEventArgs e)
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

        private void tvZone_MouseDown(object sender, MouseEventArgs e)
        {
            TreeNode oNode = tvZone.GetNodeAt(e.X, e.Y);
            if (oNode == null) return;
            MyBlnReading = true;
            if (oNode.Level == 1) oNode = oNode.Parent;
            txtZoneRemark.Text = oNode.Text.Split('-')[1].ToString();
            MyBlnReading = false;
        }

        private void txtZoneRemark_TextChanged(object sender, EventArgs e)
        {
            if (tvZone.Nodes == null) return;
            if (oRe == null) return;
            if (oRe.Areas == null) return;
            if (tvZone.SelectedNode == null) return;
            if (MyBlnReading) return;
            TreeNode node = tvZone.SelectedNode;
            if (node.Level == 0)
            {
                oRe.Areas[node.Index].Remark = txtZoneRemark.Text.Trim();
                node.Text = (node.Index + 1).ToString() + "-" + txtZoneRemark.Text.Trim();
            }
            else if (node.Level == 1)
            {
                node = node.Parent;
                oRe.Areas[node.Index].Remark = txtZoneRemark.Text.Trim();
                node.Text = (node.Index + 1).ToString() + "-" + txtZoneRemark.Text.Trim();
            }
        }

        private void dgvZoneChn1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if ((e.RowIndex == -1) || (e.ColumnIndex == -1)) return;
            for (int i = 0; i < dgvZoneChn1.SelectedRows.Count; i++)
            {
                dgvZoneChn1.SelectedRows[i].Cells[e.ColumnIndex].Value = dgvZoneChn1[e.ColumnIndex, e.RowIndex].Value.ToString();
                switch (e.ColumnIndex)
                {
                    case 2:
                        string strName = myDevName.Split('\\')[0].ToString();
                        byte bytSubID = byte.Parse(strName.Split('-')[0]);
                        byte bytDevID = byte.Parse(strName.Split('-')[1]);

                        byte[] bytTmp = new byte[4];
                        bytTmp[0] = (byte)(dgvZoneChn1.SelectedRows[i].Index + 1);
                        bytTmp[2] = 0;
                        bytTmp[3] = 0;

                        if (dgvZoneChn1[2, dgvZoneChn1.SelectedRows[i].Index].Value.ToString().ToLower() == "true")
                            bytTmp[1] = 100;
                        else bytTmp[1] = 0;

                        Cursor.Current = Cursors.WaitCursor;
                        if (CsConst.mySends.AddBufToSndList(bytTmp, 0x0031, bytSubID, bytDevID, false, true, true,CsConst.minAllWirelessDeviceType.Contains(mywdDevicerType)) == false)
                        {
                        }

                        Cursor.Current = Cursors.Default;
                        break;
                }
            }
        }

        private void dgvZoneChn1_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            lb11.Text = dgvZoneChn1.RowCount.ToString();
        }

        private void dgvZoneChn1_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            lb11.Text = dgvZoneChn1.RowCount.ToString();
        }

        private void dgvZoneChn1_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            dgvZoneChn1.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void chbSceneOutput_CheckedChanged(object sender, EventArgs e)
        {
           
        }

        private void SceneRuntime_TextChanged(object sender, EventArgs e)
        {
            //if (cbSence.Items.Count < 0 || cbSence.SelectedIndex < 0) return;
            //if (isChangeScene) return;
            //string str = "";
            //str = SceneRuntime.Text;
            //oRe.Areas[cbSence.SelectedIndex].Scen[cbChooseScene.SelectedIndex].Time = Convert.ToInt32(str);
        }

        private void txtSceneRemark_TextChanged(object sender, EventArgs e)
        {
            //if (cbSence.Items.Count < 0 || cbSence.SelectedIndex < 0) return;
            //cbChooseScene.Items[cbChooseScene.SelectedIndex] = (cbChooseScene.SelectedIndex).ToString() + "-" + txtSceneRemark.Text;
            //oRe.Areas[cbSence.SelectedIndex].Scen[cbChooseScene.SelectedIndex].Remark = txtSceneRemark.Text.Trim();
        }

        private void cbChooseScene_SelectedIndexChanged(object sender, EventArgs e)
        {
            //ScenesSelectIndex = cbChooseScene.SelectedIndex;
            //isChangeScene = true;
            //txtSceneRemark.Text = cbChooseScene.Text.Split('-')[1].ToString();
            //Relay.Scene temp = oRe.Areas[cbSence.SelectedIndex].Scen[cbChooseScene.SelectedIndex];
            //SceneRuntime.Text = Time.ToString();
            //List<ChnSenceInfomation> Scene = new List<ChnSenceInfomation>();
            //panel11.Controls.Clear();
            //for (int i = 0; i < oRe.Chans.Count; i++)
            //{
            //    if (oRe.Chans[i].intBelongs == cbSence.SelectedIndex + 1)
            //    {
            //        ChnSenceInfomation tmp = new ChnSenceInfomation();
            //        tmp.ID = Convert.ToByte(oRe.Chans[i].ID);
            //        tmp.light = oRe.Areas[cbSence.SelectedIndex].Scen[cbChooseScene.SelectedIndex].light[i];
            //        tmp.Remark = oRe.Chans[i].Remark;
            //        tmp.type = 2;
            //        if (cbChooseScene.SelectedIndex == 0) tmp.light = 0;
            //        Scene.Add(tmp);
            //    }
            //}
            //SenceDesign sencedesign = new SenceDesign(panel11, Scene, mywdDevicerType, cbSence.SelectedIndex, cbChooseScene.SelectedIndex, oRe, myDevName);
            //isChangeScene = false;
        }

        private void txtSceneRestore_TextChanged(object sender, EventArgs e)
        {
            //if (cbSence.Items.Count < 0 || cbSence.SelectedIndex < 0) return;
            //string str = txtSceneRestore.Text;
            //if (GlobalClass.IsNumeric(str))
            //{
            //    Relay.Area temp = oRe.Areas[cbSence.SelectedIndex];
            //    str = HDLPF.IsNumStringMode(str, 0, 255);
            //    bytDefaultSce = byte.Parse(str);
            //}
        }

        private void rbScenes1_CheckedChanged(object sender, EventArgs e)
        {
            //if (cbSence.Items.Count < 0 || cbSence.SelectedIndex < 0) return;
            //if (rbScenes1.Checked)
            //{
            //    Relay.Area temp = oRe.Areas[cbSence.SelectedIndex];
            //    txtSceneRestore.Visible = false;
            //    bytDefaultSce = 255;
            //}
            //else if (rbScenes2.Checked) txtSceneRestore.Visible = true;
        }

        private void cbSence_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (oRe == null) return;
            //if (oRe.Areas == null) return;
            //Relay.Area temp = oRe.Areas[cbSence.SelectedIndex];
            //if (bytDefaultSce == 255)
            //{
            //    rbScenes1.Checked = true;
            //}
            //else
            //{
            //    rbScenes2.Checked = true;
            //    txtSceneRestore.Text = bytDefaultSce.ToString();
            //}
            //cbChooseScene.Items.Clear();
            //if (Scen == null) Scen = new List<Relay.Scene>();
            //for (int i = 0; i < Scen.Count; i++)
            //{
            //    cbChooseScene.Items.Add((i).ToString() + "-" + Scen[i].Remark.ToString());
            //}
            //if (ScenesSelectIndex < cbChooseScene.Items.Count)
            //    cbChooseScene.SelectedIndex = ScenesSelectIndex;
            //if (cbChooseScene.SelectedIndex < 0) cbChooseScene.SelectedIndex = 0;
            //cbChooseScene_SelectedIndexChanged(null, null);
        }

        private void tbRemarkSeries_TextChanged(object sender, EventArgs e)
        {
            btnRefresh_Click(null, null);
        }

        private void cbstep_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnRefresh_Click(null, null);
        }

        private void cbSeriesRepeat_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnRefresh_Click(null, null);
        }

        private void cbSeqMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnRefresh_Click(null, null);
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            if (isSelectSeq) return;
            if (oRe == null) return;
            if (oRe.Areas == null) return;
            if (cbstep.Text == "") return;
            if (cbSeriesRepeat.Text == "") return;
            if (tvSeries.SelectedNode == null) return;
            TreeNode oNode = tvSeries.SelectedNode;
            if (oNode.Level == 0) return;

            if (oRe.Areas[oNode.Parent.Index].Seq[oNode.Index] == null) //添加新序列
            {
                AddNewSeriesToForm(0, oNode.Parent.Index);
            }
            else  // 原有基础更新
            {
                oRe.Areas[oNode.Parent.Index].Seq[oNode.Index].Remark = tbRemarkSeries.Text;
                oRe.Areas[oNode.Parent.Index].Seq[oNode.Index].Steps = byte.Parse((cbstep.SelectedIndex).ToString());
                oRe.Areas[oNode.Parent.Index].Seq[oNode.Index].Times = byte.Parse(cbSeriesRepeat.SelectedIndex.ToString());
                if (cbSeqMode.SelectedIndex == 0) oRe.Areas[oNode.Parent.Index].Seq[oNode.Index].Mode = 4;
                else if (cbSeqMode.SelectedIndex == 1) oRe.Areas[oNode.Parent.Index].Seq[oNode.Index].Mode = 3;
                else if (cbSeqMode.SelectedIndex == 2) oRe.Areas[oNode.Parent.Index].Seq[oNode.Index].Mode = 2;
                else if (cbSeqMode.SelectedIndex == 3) oRe.Areas[oNode.Parent.Index].Seq[oNode.Index].Mode = 1;
                else if (cbSeqMode.SelectedIndex == 4) oRe.Areas[oNode.Parent.Index].Seq[oNode.Index].Mode = 0;
                oNode.Text = oNode.Text.Split('-')[0] + "-" + tbRemarkSeries.Text;

                ShowSequenceStepsToForm(oRe.Areas[oNode.Parent.Index], oRe.Areas[oNode.Parent.Index].Seq[oNode.Index]);
            }
            oNode.Text = oNode.Text.Split('-')[0].ToString() + "-" + tbRemarkSeries.Text;
        }



        private void btnSaveSequence_Click(object sender, EventArgs e)
        {
            tsbDown_Click(toolStripLabel2, null);
        }

        private void btnSaveScenes_Click(object sender, EventArgs e)
        {
            tsbDown_Click(toolStripLabel2, null);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            tsbDown_Click(toolStripLabel2, null);
        }

        private void btnSaveChn_Click(object sender, EventArgs e)
        {
            tsbDown_Click(toolStripLabel2, null);
        }

        private void DgvSeries_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnSaveAndClose2_Click(object sender, EventArgs e)
        {
            btnSave_Click(null, null);
            this.Close();
        }

        private void btnSaveAndClose1_Click(object sender, EventArgs e)
        {
            btnSaveChn_Click(null, null);
            this.Close();
        }

        private void btnSaveAndClose3_Click(object sender, EventArgs e)
        {
            btnSaveScenes_Click(null, null);
            this.Close();
        }

        private void btnSaveAndClose4_Click(object sender, EventArgs e)
        {
            btnSaveSequence_Click(null, null);
            this.Close();
        }

        private void DgChns_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            MyBlnReading = true;
            txtONDelay.Visible = false;
            if (e.RowIndex >= 0)
            {
                if (e.ColumnIndex == 4 || e.ColumnIndex == 6)
                {
                    string strTmp = HDLPF.GetTimeFromString(DgChns[e.ColumnIndex, e.RowIndex].Value.ToString(), '.');
                    txtONDelay.Text = strTmp;
                    addcontrols(e.ColumnIndex, e.RowIndex, txtONDelay, DgChns);
                    txtONDelay_TextChanged(txtONDelay, null);
                }
            }
            MyBlnReading = false;
        }

        private void addcontrols(int col, int row, Control con, DataGridView dgv)
        {
            con.Show();
            con.Visible = true;
            Rectangle rect = dgv.GetCellDisplayRectangle(col, row, true);
            con.Size = rect.Size;
            con.Top = rect.Top;
            con.Left = rect.Left;
        }

        private void DgChns_SizeChanged(object sender, EventArgs e)
        {
           
        }

        private void DgvSeries_SizeChanged(object sender, EventArgs e)
        {
            HDLSysPF.setDataGridViewColumnsWidth(DgvSeries);
        }

        private void btnRef1_Click(object sender, EventArgs e)
        {
            tsbDown_Click(tsbDown, null);
        }

        private void btnSave1_Click(object sender, EventArgs e)
        {
            tmUpload_Click(tmRead, null);
        }

        private void btnSaveAndClose1_Click_1(object sender, EventArgs e)
        {
            tmUpload_Click(tmRead, null);
            this.Close();
        }

        private void tvScene_MouseDown(object sender, MouseEventArgs e)
        {
            mblnIsShowing = true;
            dgScene.Rows.Clear();

            TreeNode oNode = tvScene.GetNodeAt(e.Location);            
        }

        void ShowScenenChannelInformation(TreeNode oNode)
        {
            if (oNode == null) return;
            if (oRe.Areas == null) return;
            if (oNode.Level == 0) return;

            currentAreaId = (Byte)oNode.Parent.Index;
            currentSceneId = (Byte)oNode.Index;

            HDLSysPF.ListAllSceneNameToCombobox(oRe, currentAreaId, cbRestoreList, true);
            tbSceneName.Text = oRe.Areas[currentAreaId].Scen[currentSceneId].Remark;
            cbRestoreList.SelectedIndex = oRe.Areas[currentAreaId].bytDefaultSce;
            tbRunningTime.Text = Convert.ToString(oRe.Areas[currentAreaId].Scen[currentSceneId].Time);

            showSenceInfo(currentAreaId, currentSceneId);
            mblnIsShowing = false;
            chbOutputS_CheckedChanged(chbOutputS, null);
        }

        private void showSenceInfo(Byte AreaID, Byte SceneID)
        {
            if (oRe == null) return;
            if (oRe.Areas == null) return;

            if (oRe.Areas.Count < AreaID + 1) return;
            dgScene.Rows.Clear();
            Relay.Area oArea = oRe.Areas[AreaID];

            if (oArea.Scen == null || oArea.Scen.Count == 0) return;
            if (oArea.Scen.Count < SceneID + 1) return;

            Relay.Scene oSce = oArea.Scen[SceneID];
            if (SceneID == 0) oSce.light = (new Byte[oRe.Chans.Count]).ToList();

            for (int i = 0; i < oRe.Chans.Count; i++)
            {
                if (oRe.Chans[i].intBelongs == oArea.ID)
                {
                    if (oSce.light[i]> 100) oSce.light[i] = 100;
                    Object[] obj = new Object[] { oRe.Chans[i].ID, oRe.Chans[i].Remark, cboStatus.Items[ oSce.light[i] / 100]};
                    dgScene.Rows.Add(obj);
                }
            }

        }

        private void tbSceneName_TextChanged(object sender, EventArgs e)
        {
            if (mblnIsShowing) return;
            if (tvScene.SelectedNode == null) return;
            if (tvScene.SelectedNode.Level == 0) return;
            TreeNode oNode = tvScene.SelectedNode;

            oNode.Text = (oNode.Index).ToString() + "-" + tbSceneName.Text;
            oRe.Areas[currentAreaId].Scen[currentSceneId].Remark = tbSceneName.Text.Trim();
        }

        private void cbRestoreList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (mblnIsShowing) return;
            oRe.Areas[currentAreaId].bytDefaultSce = Convert.ToByte(cbRestoreList.SelectedIndex);
        }

        private void tbRunningTime_TextChanged(object sender, EventArgs e)
        {
            if (mblnIsShowing) return;
            oRe.Areas[currentAreaId].Scen[currentSceneId].Time = Convert.ToInt32(tbRunningTime.Text);
        }

        private void testToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (oRe == null) return;
            if (oRe.Areas == null) return;
            if (tvSeries.SelectedNode == null) return;
            if (tvSeries.SelectedNode.Level == 0) return;

            byte[] bytTmp = new byte[2];
            bytTmp[0] = currentAreaId;
            if (((ToolStripMenuItem)sender).Tag == null || ((ToolStripMenuItem)sender).Tag.ToString() != "0")
            {
                bytTmp[1] = (Byte)(currentSerieId + 1);
                ((ToolStripMenuItem)sender).Tag = "0";
            }
            else if (((ToolStripMenuItem)sender).Tag.ToString() == "0")
            {
                bytTmp[1] = 0;
                ((ToolStripMenuItem)sender).Tag = null;
            }

            CsConst.mySends.AddBufToSndList(bytTmp, 0x001A, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(mywdDevicerType));
        }

        private void tvSeries_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (tvSeries.SelectedNode == null) return;
            ShowSequenceSceneInformation(tvSeries.SelectedNode);
        }

        private void tvScene_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (tvScene.SelectedNode == null) return;
            ShowScenenChannelInformation(tvScene.SelectedNode);
        }

        private void chbOutputS_CheckedChanged(object sender, EventArgs e)
        {
            oRe.isOutput = chbOutputS.Checked;
            if (chbOutputS.Checked == true)
            {
                if (oRe == null) return;
                if (oRe.Areas == null) return;

                Byte TotalChnNumber = (Byte)oRe.Chans.Count;
                byte[] bytTmp = new byte[2 + TotalChnNumber];
                bytTmp[0] = (Byte)(currentAreaId + 1);
                bytTmp[1] = (Byte)(currentSceneId + 1);
                Array.Copy(oRe.Areas[currentAreaId].Scen[currentSceneId].light.ToArray(), 0, bytTmp, 2, TotalChnNumber);
                CsConst.mySends.AddBufToSndList(bytTmp, 0xF074, SubNetID, DeviceID, false, false, false, CsConst.minAllWirelessDeviceType.Contains(mywdDevicerType));
            }
        }

        private void dgScene_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (tvScene.SelectedNode == null) return;
            if (e.RowIndex == -1) return;
            if (e.ColumnIndex == -1) return;
            if (dgScene.RowCount == 0) return;

            string Status = dgScene[2, e.RowIndex].Value.ToString();
            for (int i = 0; i < dgScene.SelectedRows.Count; i++)
            {
                if (e.ColumnIndex == 2)
                {
                    dgScene[2, dgScene.SelectedRows[i].Index].Value = Status;
                    Byte currentIntensity = Convert.ToByte(cboStatus.Items.IndexOf(Status));
                    Byte ChnID = Convert.ToByte(dgScene[0, e.RowIndex].Value.ToString());
                    oRe.Areas[currentAreaId].Scen[currentSceneId].light[ChnID - 1] = (Byte)(currentIntensity * 100);
                }
            }
            chbOutputS_CheckedChanged(chbOutputS, null);
        }

        private void dgScene_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            dgScene.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void chbSyn_CheckedChanged(object sender, EventArgs e)
        {
            if (mblnIsShowing) return;
            if (chbSyn.Checked == true)
            {
                for (int i = 0; i < oRe.Areas[currentAreaId].Scen.Count; i++)
                {
                    oRe.Areas[currentAreaId].Scen[i].Time = Convert.ToInt32(tbRunningTime.Text);
                }
                chbSyn.Checked = false;
                MessageBox.Show("SYN Running Done!", "Hint");
            }
        }

        private void dgvCurtain_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                txtCurtainRuntime.Visible = false;
                string strTmp = HDLPF.GetTimeFrmStringMs(dgvCurtain[2, e.RowIndex].Value.ToString());
                txtCurtainRuntime.Text = strTmp;
                HDLSysPF.addcontrols(2, e.RowIndex, txtCurtainRuntime, dgvCurtain);
            }
            catch
            {
            }
        }

        private void dgvCurtain_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (oRe == null) return;
                if (oRe.Chans == null || oRe.Chans.Count ==0) return;
                if ((e.RowIndex == -1) || (e.ColumnIndex == -1)) return;
                if (dgvCurtain[e.ColumnIndex, e.RowIndex].Value == null) dgvCurtain[e.ColumnIndex, e.RowIndex].Value = "";
                for (int i = 0; i < dgvCurtain.SelectedRows.Count; i++)
                {
                    dgvCurtain.SelectedRows[i].Cells[e.ColumnIndex].Value = dgvCurtain[e.ColumnIndex, e.RowIndex].Value.ToString();
                    int iRowId = dgvCurtain.SelectedRows[i].Index;
                    switch (e.ColumnIndex)
                    {
                        case 1:
                            if (dgvCurtain[e.ColumnIndex,iRowId].Value.ToString().ToLower() == "true")
                                oRe.Chans[iRowId * 2].bEnableCurtain = 1;
                            else
                                oRe.Chans[iRowId * 2].bEnableCurtain = 0;
                            break;
                        case 2:
                            dgvCurtain[2, dgvCurtain.SelectedRows[i].Index].Value = HDLPF.GetStringFrmTimeMs(int.Parse(txtCurtainRuntime.Text.ToString()));
                            int Runtime = int.Parse(txtCurtainRuntime.Text.ToString());
                            oRe.Chans[iRowId * 2].onTime = Runtime;
                            break;
                    }
                }
            }
            catch
            {
            }
        }

        private void dgvSlave_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (MyBlnReading) return;
                if (oRe == null) return;
                if (oRe.SlaveInfo == null) return;
                if (oRe.SlaveInfo.Count <= 0) return;
                if ((e.RowIndex == -1) || (e.ColumnIndex == -1)) return;
                if (dgvSlave[e.ColumnIndex, e.RowIndex].Value == null) dgvSlave[e.ColumnIndex, e.RowIndex].Value = "";
                byte[] arayTmp = oRe.SlaveInfo[0];
                for (int i = 0; i < dgvSlave.SelectedRows.Count; i++)
                {
                    dgvSlave.SelectedRows[i].Cells[e.ColumnIndex].Value = dgvSlave[e.ColumnIndex, e.RowIndex].Value.ToString();
                    string strTmp = "";
                    switch (e.ColumnIndex)
                    {
                        case 1:
                            strTmp = dgvSlave[1, dgvSlave.SelectedRows[i].Index].Value.ToString();
                            dgvSlave[1, dgvSlave.SelectedRows[i].Index].Value = HDLPF.IsNumStringMode(strTmp, 0, 255);
                            arayTmp[dgvSlave.SelectedRows[i].Index * 2 + 1] = Convert.ToByte(dgvSlave[1, dgvSlave.SelectedRows[i].Index].Value.ToString());
                            break;
                        case 2:
                            strTmp = dgvSlave[2, dgvSlave.SelectedRows[i].Index].Value.ToString();
                            dgvSlave[2, dgvSlave.SelectedRows[i].Index].Value = HDLPF.IsNumStringMode(strTmp, 0, 255);
                            arayTmp[dgvSlave.SelectedRows[i].Index * 2 + 2] = Convert.ToByte(dgvSlave[2, dgvSlave.SelectedRows[i].Index].Value.ToString());
                            break;

                    }
                }
            }
            catch
            {
            }
        }

        private void dgvSlave_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            dgvSlave.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }
    }
}
