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
    public partial class frmDimmer : Form
    {
        [DllImport("user32.dll")]
        public static extern Boolean FlashWindow(IntPtr hwnd, Boolean bInvert);

        private Point Position = new Point(0, 0);

        private Dimmer oDimmer = null;
        private Byte SubnetID;
        private Byte DeviceID;
        private String mystrName = null;
        private Int32 mywdDevicerType = 0;
        private Int32 myintDIndex = -1;

        private Byte currentAreaId;
        private Byte currentSceneId;
        private Byte currentSerieId = 0;

        private Int32 mintSeriesMode = 0;
        
        private Int32 mintCopySeriesID = -1;
        private Boolean mblnIsShowing = false;

        TimeText txtScene = new TimeText(":");
        TimeMs txtSeries = new TimeMs();
        private TextBox txtRemark = new TextBox();
        private TextBox txtMinLimit = new TextBox();
        private TextBox txtMaxLimit = new TextBox();
        private TextBox txtMaxLevel = new TextBox();
        private ComboBox cbType = new ComboBox();
        private ComboBox cbProfile = new ComboBox();
        private ComboBox cbOutputType = new ComboBox();
        private SingleChn SceneChn = new SingleChn();
        private Boolean isChangeScene = false;
        private Boolean isClick = false;
        private Boolean isSelectSeq = false;
        private Boolean MyBlnReading = false;
        private Boolean isRead = false;
        private NetworkInForm networkInForm1 = null;

        private List<Byte> arrCurrentActiveDaliAddress = new List<byte>();

        public frmDimmer()
        {
            InitializeComponent();
        }

        public frmDimmer(Dimmer oDim, string strName, int intDeviceType, int intDIndex)
        {
            InitializeComponent();
            this.oDimmer = oDim;
            this.mystrName = strName;
            this.mywdDevicerType = intDeviceType;
            this.myintDIndex = intDIndex;
            string strDevName = strName.Split('\\')[0].ToString();

            HDLSysPF.DisplayDeviceNameModeDescription(strName, mywdDevicerType, cboDevice, tbModel, tbDescription);

            SubnetID = Convert.ToByte(strDevName.Split('-')[0]);
            DeviceID = Convert.ToByte(strDevName.Split('-')[1]);
            tsl3.Text = strName;
        }

        void InitialFormCtrlsTextOrItems()
        {
            cbType.Items.Clear();
            cbType.Items.AddRange(CsConst.LoadType);
            cbProfile.Items.Clear();
            cbProfile.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99894", "") + " 1.0");
            cbProfile.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99894", "") + " 1.5");
            cbProfile.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99894", "") + " 2.0");
            cbProfile.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99894", "") + " 3.0");

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
            #region
            if (DimmerDeviceTypeList.DimmerMRDAHas0V1V.Contains(mywdDevicerType))
            {
                cl8.HeaderText = CsConst.mstrINIDefault.IniReadValue("Public", "99611", "");
                cbOutputType.Items.Clear();
                cbOutputType.Items.Add("0-10V");
                cbOutputType.Items.Add("1-10V");
            }
            else if (DimmerDeviceTypeList.DimmerMRDAHasSeperateTraidingLeading.Contains(mywdDevicerType))
            {
                cbOutputType.Items.Clear();
                cbOutputType.Items.Add(CsConst.WholeTextsList[632].sDisplayName);
                cbOutputType.Items.Add(CsConst.WholeTextsList[631].sDisplayName);
            }
            else
            {
                cbOutputType.Items.Add(CsConst.mstrInvalid);
            }

            DgvSeries.Columns[0].Width = (int)(DgvSeries.Width * 0.16);
            DgvSeries.Columns[1].Width = (int)(DgvSeries.Width * 0.54);
            DgvSeries.Columns[2].Width = (int)(DgvSeries.Width * 0.24);
            #endregion

            tvZone.HideSelection = false;
            //自已绘制
            this.tvZone.DrawMode = TreeViewDrawMode.OwnerDrawText;
            this.tvZone.DrawNode += new DrawTreeNodeEventHandler(treeView_DrawNode);

            tvSeries.HideSelection = false;
            //自已绘制
            this.tvSeries.DrawMode = TreeViewDrawMode.OwnerDrawText;
            this.tvSeries.DrawNode += new DrawTreeNodeEventHandler(treeView_DrawNode);

            cbType.DropDownStyle = ComboBoxStyle.DropDownList;
            cbProfile.DropDownStyle = ComboBoxStyle.DropDownList;
            cbOutputType.DropDownStyle = ComboBoxStyle.DropDownList;


            txtMaxLevel.KeyPress += txtMaxLevel_KeyPress;
            txtMinLimit.KeyPress += txtMaxLevel_KeyPress;
            txtMaxLimit.KeyPress += txtMaxLevel_KeyPress;

            //setAllControlVisible(false);
            DgChns.Controls.Add(txtRemark);
            DgChns.Controls.Add(txtMaxLimit);
            DgChns.Controls.Add(txtMinLimit);
            DgChns.Controls.Add(txtMaxLevel);
            DgChns.Controls.Add(cbType);
            DgChns.Controls.Add(cbProfile);
            DgChns.Controls.Add(cbOutputType);

            dgScene.Controls.Add(SceneChn);

            txtMaxLevel.TextChanged += txtMaxLevel_TextChanged;
            txtRemark.TextChanged += txtRemark_TextChanged;
            txtMinLimit.TextChanged += txtMinLimit_TextChanged;
            txtMaxLimit.TextChanged += txtMaxLimit_TextChanged;
            cbType.SelectedIndexChanged += cbType_SelectedIndexChanged;
            cbOutputType.SelectedIndexChanged += cbOutputType_SelectedIndexChanged;
            cbProfile.SelectedIndexChanged += cbProfile_SelectedIndexChanged;
        }

        private void frmDimmer_Load(object sender, EventArgs e)
        {
            isRead = true;
            InitialFormCtrlsTextOrItems();
            isRead = false;
        }

        void SetCtrlsVisbleWithDifferentDeviceType()
        {
            if (oDimmer == null) return;

            //0-10V 没有序列界面
            if (DimmerDeviceTypeList.DimmerWithoutSequences.Contains(mywdDevicerType)) 
              tab1.TabPages.Remove(tp4);

            Boolean NormalDimmerFirstVersion = (DimmerDeviceTypeList.DimmerModuleV1233.Contains(mywdDevicerType));
            Boolean TrailingLeading = DimmerDeviceTypeList.DimmerModuleHasTrailingLeading.Contains(mywdDevicerType);
            Boolean HasDimmingCurve = DimmerDeviceTypeList.DimmerModuleV2DimCurve.Contains(mywdDevicerType);
            Boolean HasVoltageSelection = DimmerDeviceTypeList.DimmerModuleHasVoltageSelection.Contains(mywdDevicerType);
            Boolean Has0V1VOutput = DimmerDeviceTypeList.DimmerMRDAHas0V1V.Contains(mywdDevicerType) ||
                                    DimmerDeviceTypeList.DimmerMRDAHasSeperateTraidingLeading.Contains(mywdDevicerType);
            Boolean HasNetworkInformation = DimmerDeviceTypeList.HDLDMXDeviceType.Contains(mywdDevicerType); // DMX模块
            Boolean bIsDaliDimmerModules = DimmerDeviceTypeList.DALIDimmerDeviceTypeLists.Contains(mywdDevicerType); // Dali模块

            clPowerOnLevel.Visible = bIsDaliDimmerModules;
            clFailLevel.Visible = bIsDaliDimmerModules;
            cboDaliTime.Visible = bIsDaliDimmerModules;
            if (HasNetworkInformation)
            {
                networkInForm1 = new NetworkInForm(SubnetID, DeviceID, mywdDevicerType);
                panBasic.Controls.Add(networkInForm1);
                networkInForm1.Dock = DockStyle.Top;
                networkInForm1.Visible = true;
            }

            pnBroadcast.Visible = !NormalDimmerFirstVersion;
            groupBoxDimmingMode.Visible = TrailingLeading;
            VoltageSelection.Visible = HasVoltageSelection;
            columnDimmingProfile.Visible = HasDimmingCurve;

            toolStrip1.Visible = (CsConst.MyEditMode == 0);

            cl8.Visible = Has0V1VOutput;

            if (bIsDaliDimmerModules)
            {
                panBasic.Visible = false;
                cl5.Visible = false;
            }
            else
            {
                tab1.TabPages.Remove(tabDali);
                tab1.TabPages.Remove(tabDaliStatus);
            }
            panBasic.Visible = !NormalDimmerFirstVersion;
        }

        void EditableModeSetCtrlsUnvisible(Boolean TF)
        {
            txtRemark.Visible = TF;
            txtMaxLimit.Visible = TF;
            txtMinLimit.Visible = TF;
            txtMaxLevel.Visible = TF;
            cbType.Visible = TF;
            cbProfile.Visible = TF;
            cbOutputType.Visible = TF;
            SceneChn.Visible = TF;
        }

        void cbProfile_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DgChns.CurrentRow.Index < 0) return;
            if (DgChns.RowCount <= 0) return;
            int index = DgChns.CurrentRow.Index;
            DgChns[6, index].Value = cbProfile.Text;
            HDLSysPF.ModifyMultilinesIfNeeds(DgChns, DgChns[6, index].Value.ToString(), 6);
            DgChns_CellValueChanged(null, null);
        }

        void cbOutputType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DgChns.CurrentRow.Index < 0) return;
            if (DgChns.RowCount <= 0) return;
            int index = DgChns.CurrentRow.Index;
            DgChns[7, index].Value = cbOutputType.Text;
            HDLSysPF.ModifyMultilinesIfNeeds(DgChns, DgChns[7, index].Value.ToString(), 7);
            DgChns_CellValueChanged(null, null);
        }

        void cbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DgChns.CurrentRow.Index < 0) return;
            if (DgChns.RowCount <= 0) return;
            int index = DgChns.CurrentRow.Index;
            DgChns[2, index].Value = cbType.Text;
            HDLSysPF.ModifyMultilinesIfNeeds(DgChns, DgChns[2, index].Value.ToString(),2);
            DgChns_CellValueChanged(null, null);
        }

        void txtMaxLimit_TextChanged(object sender, EventArgs e)
        {
            if (DgChns.CurrentRow.Index < 0) return;
            if (DgChns.RowCount <= 0) return;
            int index = DgChns.CurrentRow.Index;
            if (txtMaxLimit.Text.Length > 0)
            {
                txtMaxLimit.Text = HDLPF.IsNumStringMode(txtMaxLimit.Text, 0, 100);
                txtMaxLimit.SelectionStart = txtMaxLimit.Text.Length;
                DgChns[4, index].Value = txtMaxLimit.Text;
                HDLSysPF.ModifyMultilinesIfNeeds(DgChns, DgChns[4, index].Value.ToString(), 4);
                DgChns_CellValueChanged(null, null);
            }
        }

        void txtMinLimit_TextChanged(object sender, EventArgs e)
        {
            if (DgChns.CurrentRow.Index < 0) return;
            if (DgChns.RowCount <= 0) return;
            int index = DgChns.CurrentRow.Index;
            if (txtMinLimit.Text.Length > 0)
            {
                txtMinLimit.Text = HDLPF.IsNumStringMode(txtMinLimit.Text, 0, 100);
                txtMinLimit.SelectionStart = txtMinLimit.Text.Length;
                DgChns[3, index].Value = txtMinLimit.Text;
                HDLSysPF.ModifyMultilinesIfNeeds(DgChns, DgChns[3, index].Value.ToString(), 3);
                DgChns_CellValueChanged(null, null);
            }
        }

        void txtRemark_TextChanged(object sender, EventArgs e)
        {
            if (DgChns.CurrentRow.Index < 0) return;
            if (DgChns.RowCount <= 0) return;
            int index = DgChns.CurrentRow.Index;
            txtRemark.SelectionStart = txtRemark.Text.Length;
            DgChns[1, index].Value = txtRemark.Text;
            HDLSysPF.ModifyMultilinesIfNeeds(DgChns, DgChns[1, index].Value.ToString(), 1);
            DgChns_CellValueChanged(null, null);
        }

        void ModifySceneMultilinesIfNeeds(string strTmp, int ColumnIndex)
        {
            if (dgScene.SelectedRows == null || dgScene.SelectedRows.Count == 0) return;
            if (strTmp == null) strTmp = "";
            // change the value in selected more than one line
            if (dgScene.SelectedRows.Count > 1)
            {
                for (int i = 0; i < dgScene.SelectedRows.Count; i++)
                {
                    dgScene.SelectedRows[i].Cells[ColumnIndex].Value = strTmp;
                }
            }
        }

        void txtMaxLevel_TextChanged(object sender, EventArgs e)
        {
            if (DgChns.CurrentRow.Index < 0) return;
            if (DgChns.RowCount <= 0) return;
            int index = DgChns.CurrentRow.Index;
            if (txtMaxLevel.Text.Length > 0)
            {
                txtMaxLevel.Text = HDLPF.IsNumStringMode(txtMaxLevel.Text, 0, 100);
                txtMaxLevel.SelectionStart = txtMaxLevel.Text.Length;
                DgChns[5, index].Value = txtMaxLevel.Text;
                HDLSysPF.ModifyMultilinesIfNeeds(DgChns, DgChns[5, index].Value.ToString(),5);
            }
        }

        void txtMaxLevel_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar) || e.KeyChar == 8)
                e.Handled = false;
            else
                e.Handled = true;
        }

        void UpdateBasicInfoOfLeftPanel()
        {
            isRead = true;
            chbBroadcast.Checked = oDimmer.BroadcastChannelStatus == 1 ? true : false;
            if (oDimmer.LoadType == 255)
            {
                VoltageSelection.Visible = false;
            }
            else if (oDimmer.LoadType <= 1)
            {
                VoltageSelection.Visible = true;
                radioButton220V.Checked = oDimmer.LoadType == 0 ? true : false;
                radioButton110V.Checked = oDimmer.LoadType == 0 ? false : true;
            }
            radioButtonTrailing.Checked = oDimmer.DimmingMode == 0 ? true : false;
            radioButtonLeading.Checked = oDimmer.DimmingMode == 0 ? false : true;
            isRead = false;
        }

        void LoadBasicInformationToForm()
        {
            EditableModeSetCtrlsUnvisible(false);

            string strTmp = CsConst.mstrINIDefault.IniReadValue("Public", "00031", "");
            //cl13.Items.Clear(); cl13.Items.AddRange(CsConst.LoadType);
            if (oDimmer == null) return;
            if (oDimmer.Chans == null) return;
            DgChns.Rows.Clear();
            foreach (Dimmer.Channel ch in oDimmer.Chans)
            {
                object []boj;
                if (DimmerDeviceTypeList.DimmerModuleHasTrailingLeading.Contains(mywdDevicerType))
                    boj = new object[] { ch.ID, ch.Remark, cbType.Items[ch.LoadType], ch.MinValue, ch.MaxValue, ch.MaxLevel,
                        cbProfile.Items[ch.dimmingProfile],cbOutputType.Items[ch.DimmingMode],false };
                else if (mywdDevicerType == 455)
                {
                    boj = new object[] { ch.ID, ch.Remark, cbType.Items[ch.LoadType], ch.MinValue, ch.MaxValue, ch.MaxLevel,
                        cbProfile.Items[ch.dimmingProfile],cbOutputType.Items[ch.outPutType],false};
                }
                else
                    boj = new object[] { ch.ID, ch.Remark, cbType.Items[ch.LoadType], ch.MinValue, ch.MaxValue, ch.MaxLevel, 
                                         cbProfile.Items[ch.dimmingProfile], cbOutputType.Items[ch.DimmingMode],ch.levelIfNoPower,ch.levelWhenPowerOn, false };
                DgChns.Rows.Add(boj);
            }
            UpdateBasicInfoOfLeftPanel();
        }
        delegate void SetvalueHandle(int rowIndex);

        private void Setvalue(int rowIndex)
        {
            DgChns.CellValueChanged -= new DataGridViewCellEventHandler(DgChns_CellValueChanged);
            System.Threading.Thread.Sleep(50);
            List<int> values = new List<int>();

            for (int i = 0; i < DgChns.Rows.Count; i++)
            {
                if (DgChns[6, i].Value.ToString().ToLower() == "true")
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
                    DgChns[6, i].Value = true;
                }
            }
            DgChns[0, 0].Selected = false;
            DgChns[6, rowIndex].Selected = true;

            DgChns.CellValueChanged += new DataGridViewCellEventHandler(DgChns_CellValueChanged);
        }

        private void showZoneInfo()
        {
            //chbSyn.Checked = false;
            oDimmer.isModifyScenesSyn = false;
           // chbSceneOutput.Checked = false;
            oDimmer.isOutput = false;
            tvZone.Nodes.Clear();
            dgvZoneChn1.Rows.Clear();
            if (oDimmer == null) return;
            if (oDimmer.Areas == null) return;
            if (oDimmer.Areas.Count > 0)
            {
                foreach (Dimmer.Area oArea in oDimmer.Areas)
                {
                    TreeNode OND = tvZone.Nodes.Add(oArea.ID.ToString(), (tvZone.Nodes.Count + 1).ToString() + "-" + oArea.Remark, 1, 1);

                    if (oDimmer.Chans != null)
                    {
                        for (int intI = 0; intI < oDimmer.Chans.Count; intI++)
                        {
                            if (oDimmer.Chans[intI].intBelongs == (oArea.ID))
                            {
                                OND.Nodes.Add(null, (intI + 1).ToString() + "-" + oDimmer.Chans[intI].Remark, 0, 0);
                            }
                        }
                    }
                }
            }

            if (oDimmer.Chans != null && oDimmer.Chans.Count > 0)
            {
                for (int i = 0; i < oDimmer.Chans.Count; i++)
                {
                    Dimmer.Channel chnTmp = oDimmer.Chans[i];
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
            tvZone.ExpandAll();
            lb22.Text = tvZone.Nodes.Count.ToString();
            lb11.Text = dgvZoneChn1.Rows.Count.ToString();
        }

        private void tab1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CsConst.MyEditMode == 0)
            {
                UpdateDisplayInformationAccordingly(null, null);
            }
            else if (CsConst.MyEditMode == 1)
            {
                int intActivePage = tab1.SelectedIndex + 1;
                if (oDimmer.MyRead2UpFlags[intActivePage - 1] == false)
                {
                    tsbDown_Click(tsbDown, null);
                    oDimmer.MyRead2UpFlags[intActivePage - 1] = true;
                }
                else
                {
                    UpdateDisplayInformationAccordingly(null, null);
                }
            }
            panel16.Visible = (tab1.SelectedTab.Name != "tabDali");
            btnSave4.Visible = (tab1.SelectedTab.Name != "tabDaliStatus");
            btnSaveAndClose4.Visible = (tab1.SelectedTab.Name != "tabDaliStatus");
            tmiDefault.Visible = (tab1.SelectedIndex == 2);
            tmiSce.Visible = (tab1.SelectedIndex == 2);
            outputSceneToolStripMenuItem.Visible = (tab1.SelectedIndex == 2);
            toolStripSeparator2.Visible = (tab1.SelectedIndex == 2);
            toolStripMenuItem1.Visible = (tab1.SelectedIndex == 2);
        }

        void ShowAreasToListview(object sender, byte bytCh2Sce2Series)
        {
            ((TreeView)sender).Nodes.Clear();

            if (oDimmer == null) return;
            if (oDimmer.Areas == null) return;

            foreach (Dimmer.Area oArea in oDimmer.Areas)
            {
                switch (bytCh2Sce2Series)
                {
                    case 1:
                        TreeNode OND1 = ((TreeView)sender).Nodes.Add(null, oArea.ID + "-" + oArea.Remark, 1, 1);
                        if (oDimmer.Chans != null)
                        {
                            for (int intI = 0; intI < oDimmer.Chans.Count; intI++)
                            {
                                if (oDimmer.Chans[intI].intBelongs == (oArea.ID))
                                {
                                    OND1.Nodes.Add(null, (intI + 1).ToString() + "-" + oDimmer.Chans[intI].Remark, 0, 0);

                                }
                            }
                        }
                        break;
                    case 2:
                        TreeNode OND2 = ((TreeView)sender).Nodes.Add(null, oArea.ID + "-" + oArea.Remark, 1, 1);
                        if (oArea.Scen != null)
                        {
                            for (int intI = 0; intI < oArea.Scen.Count; intI++)
                            {
                                OND2.Nodes.Add(null, oArea.Scen[intI].ID + "-" + oArea.Scen[intI].Remark, 0, 0);
                            }
                        }
                        break;
                    case 3:
                        if (CsConst.minOnlyOnesequenceDeviceType.Contains(mywdDevicerType))
                        {
                            if (oArea.ID <= 1)
                            {
                                TreeNode OND3 = ((TreeView)sender).Nodes.Add(null, oArea.ID + "-" + oArea.Remark, 1, 1);
                                if (oArea.Seq != null)
                                {
                                    for (int intI = 0; intI < oArea.Seq.Count; intI++)
                                    {
                                        OND3.Nodes.Add(null, oArea.Seq[intI].ID + "-" + oArea.Seq[intI].Remark, 0, 0);
                                    }
                                }
                            }
                        }
                        else
                        {
                            TreeNode OND3 = ((TreeView)sender).Nodes.Add(null, oArea.ID + "-" + oArea.Remark, 1, 1);
                            if (oArea.Seq != null)
                            {
                                for (int intI = 0; intI < oArea.Seq.Count; intI++)
                                {
                                    OND3.Nodes.Add(null, oArea.Seq[intI].ID + "-" + oArea.Seq[intI].Remark, 0, 0);
                                }
                            }
                        }
                        break;
                }
            }
            if (((TreeView)sender).Nodes != null && ((TreeView)sender).Nodes.Count > currentAreaId - 1 && currentAreaId!=0)
            {
                TreeNode oNode = ((TreeView)sender).Nodes[currentAreaId - 1];
                oNode.ExpandAll();
                if (bytCh2Sce2Series == 2)
                {
                    tvScene_AfterSelect(sender, null);
                }
            }
           // oNode.Nodes[currentSceneId].
        }

        private void tsDel_Click(object sender, EventArgs e)
        {
            /*if (tvAreas.Nodes == null) return;
            if (oDimmer == null) return;
            if (oDimmer.Areas == null) return;
            if (tvAreas.SelectedNode == null) return;

            string strName = tvAreas.SelectedNode.Text.ToString();
            if (tvAreas.SelectedNode.Level != 0)  //删除回路
            {
                strName = tvAreas.SelectedNode.Text.ToString();
                DeleteNodeInAreaForm(strName);

                tvAreas.Nodes.Remove((TreeNode)tvAreas.SelectedNode);
                tvAreas.SelectedNodes.Remove(tvAreas.SelectedNode);
            }
            else            //删除区域
            {
                int intAreaID = tvAreas.SelectedNode.Index;
                oDimmer.Areas.RemoveAt(intAreaID);

                foreach (TreeNode Tmp in tvAreas.SelectedNode.Nodes)
                {
                    DeleteNodeInAreaForm(Tmp.Text);
                }
                tvAreas.SelectedNode.Remove();
            }*/
        }

        void DeleteNodeInAreaForm(string strName)
        {
            int intIndex = int.Parse(strName.Split('-')[0].ToString());
            oDimmer.Chans[intIndex - 1].intBelongs = 0;
            resetChnsWaittingAllocation();
        }

        private void resetChnsWaittingAllocation()
        {
            dgvZoneChn1.Rows.Clear();
            for (int i = 0; i < oDimmer.Chans.Count; i++)
            {
                Dimmer.Channel chnTmp = oDimmer.Chans[i];
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
            if (oDimmer.Areas[Tmp.Index].Scen == null)
            {
                oDimmer.Areas[Tmp.Index].Scen = new List<Dimmer.Scene>();
            }

            List<int> oTmp = new List<int>(); //取出所有已用的场景号
            for (int i = 0; i < oDimmer.Areas[Tmp.Index].Scen.Count; i++)
            {
                oTmp.Add(oDimmer.Areas[Tmp.Index].Scen[i].ID);
            }
            //查找位置，替换buffer
            int intSceID = 1;
            while (oTmp.Contains(intSceID))
            {
                intSceID++;
            }

            Dimmer.Scene oSce = new Dimmer.Scene();
            oSce.ID = byte.Parse(intSceID.ToString());
            oSce.Remark = "Scene";
            oSce.light = new byte[oDimmer.Chans.Count].ToList();
            oSce.Time = 0;
            oDimmer.Areas[Tmp.Index].Scen.Add(oSce);

            return intSceID;
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

        private void rb1_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked == true)
            {
                mintSeriesMode = int.Parse(((RadioButton)sender).Tag.ToString());
            }
        }

        private void tvSeries_MouseDown(object sender, MouseEventArgs e)
        {
            isSelectSeq = true;
            mblnIsShowing = true;
            txtSeries.Visible = false;

            TreeNode oNode = tvSeries.GetNodeAt(e.Location);
            if (oNode == null) return;
            if (oDimmer.Areas == null) return;

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

            #endregion
            Dimmer.Area oArea = null;
            foreach (Dimmer.Area Tmp in oDimmer.Areas)
            {
                if (Tmp.ID == currentAreaId)
                {
                    oArea = Tmp;
                    break;
                }
            }
            if (oArea == null) return;

            if (blnIsNewSqe && oArea.Scen != null) // 选择的区域
            {
                SelectScens.Items.Clear();
                if (oArea.Scen != null)
                {
                    foreach (Dimmer.Scene oSce in oArea.Scen)
                    {
                        SelectScens.Items.Add(oSce.ID + "-" + oSce.Remark);
                    }
                }
            }
            if (oNode.Level != 0)
            {
                currentSerieId = Byte.Parse(oNode.Text.Split('-')[0].ToString());

                tbRemarkSeries.Text = oNode.Text.Split('-')[1].ToString();

                Dimmer.Sequence oSeries = oArea.Seq[oNode.Index];
                cbstep.Text = oSeries.Steps.ToString();
                cbSeriesRepeat.SelectedIndex = oSeries.Times;

                if (oSeries.Mode == 4) cbSeqMode.SelectedIndex = 0;
                else if (oSeries.Mode == 3) cbSeqMode.SelectedIndex = 1;
                else if (oSeries.Mode == 2) cbSeqMode.SelectedIndex = 2;
                else if (oSeries.Mode == 1) cbSeqMode.SelectedIndex = 3;
                else if (oSeries.Mode == 0) cbSeqMode.SelectedIndex = 4;
                else cbSeqMode.SelectedIndex = 0;
                ShowSequenceStepsToForm(oArea, oSeries);
            }
            isSelectSeq = false;
        }

        private void ShowSequenceStepsToForm(Dimmer.Area oArea, Dimmer.Sequence oSeries)
        {
            if (oSeries == null) return;
            DgvSeries.Rows.Clear();
            //if (oSeries.Steps != oSeries.SceneIDs.Count) oSeries.Steps = (byte)oSeries.SceneIDs.Count;

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
            mblnIsShowing = false;
        }

        private void btnAddSeires_Click(object sender, EventArgs e)
        {
            if (oDimmer == null) return;
            if (oDimmer.Areas == null) return;
            //if (tbRemarkSeries.Text == "") return;
            if (cbstep.Text == "") return;
            if (cbSeriesRepeat.Text == "") return;
            if (tvSeries.SelectedNode == null) return;

            if (tbRemarkSeries.Text == "")
                tbRemarkSeries.Text = CsConst.MyUnnamed;

            TreeNode oNode = tvSeries.SelectedNode;
            if (oNode.Level != 0) { oNode = oNode.Parent; }
            if (oNode.Nodes != null && oNode.Nodes.Count == 6) return;
            AddNewSeriesToForm(0, oNode.Index);
        }

        int AddNewSeriesToForm(byte bytID, int intAreaID)  // default is 0, paste is 1
        {
            if (cbSeriesRepeat.SelectedIndex == -1) cbSeriesRepeat.SelectedIndex = 0;
            // 获取一个未使用的序列号
            if (oDimmer.Areas[intAreaID].Seq == null)
            {
                oDimmer.Areas[intAreaID].Seq = new List<Dimmer.Sequence>();
            }

            List<int> TmpSerID = new List<int>(); //取出所有已用的场景号
            for (int i = 0; i < oDimmer.Areas[intAreaID].Seq.Count; i++)
            {
                TmpSerID.Add(oDimmer.Areas[intAreaID].Seq[i].ID);
            }
            //查找位置，替换buffer
            int intSceID = 1;
            while (TmpSerID.Contains(intSceID))
            {
                intSceID++;
            }

            Dimmer.Sequence oSeries = new Dimmer.Sequence();

            if (bytID == 0)  //默认添加
            {

                int intNeedAdd = cbstep.SelectedIndex;
                if (DgvSeries.RowCount != 0)
                {
                    intNeedAdd = cbstep.SelectedIndex - DgvSeries.RowCount;
                }

                for (int intI = 0; intI < intNeedAdd; intI++)
                {
                    string strRemark = oDimmer.Areas[intAreaID].Scen[0].Remark;
                    object[] obj = { DgvSeries.RowCount + 1, SelectScens.Items[0], "0:0.0" };
                    DgvSeries.Rows.Add(obj);
                }

                //更新到缓存
                oSeries.ID = byte.Parse(intSceID.ToString());
                oSeries.Remark = tbRemarkSeries.Text;
                oSeries.Steps = byte.Parse((cbstep.SelectedIndex).ToString());
                oSeries.Times = byte.Parse(cbSeriesRepeat.SelectedIndex.ToString());
                oSeries.Mode = byte.Parse(mintSeriesMode.ToString());

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
                oSeries = (Dimmer.Sequence)oDimmer.Areas[intAreaID].Seq[mintCopySeriesID].Clone();
                oSeries.ID = byte.Parse(intSceID.ToString());
            }

            //添加到窗体
            TreeNode oTmp = tvSeries.Nodes[intAreaID];
            oTmp = oTmp.Nodes.Add(intSceID.ToString() + "-" + oSeries.Remark);
            tvSeries.SelectedNode = oTmp;

            if (oDimmer.Areas[intAreaID].Seq == null)
            {
                oDimmer.Areas[intAreaID].Seq = new List<Dimmer.Sequence>();
            }
            oDimmer.Areas[intAreaID].Seq.Add(oSeries);

            currentSerieId = (Byte)(oDimmer.Areas[intAreaID].Seq.Count - 1);

            return intSceID;
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
                txtSeries.TextChanged += new EventHandler(txtSeries_TextChanged);
                txtSeries.Left = rect.Left;
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


        private void tsmDeleteSce_Click(object sender, EventArgs e)
        {
            if (oDimmer == null) return;
            if (oDimmer.Areas == null) return;
            if (tvSeries.SelectedNode == null) return;

            DgvSeries.Rows.Clear();
            txtSeries.Visible = false;
            string strName = tvSeries.SelectedNode.Text.ToString();
            if (tvSeries.SelectedNode.Level != 0)  //删除序列
            {
                int intAreaID = byte.Parse(tvSeries.SelectedNode.Parent.Text.Split('-')[0].ToString());
                if (oDimmer.Areas[intAreaID - 1].Seq != null)
                {
                    byte bytTmp = byte.Parse(tvSeries.SelectedNode.Text.Split('-')[0].ToString());
                    oDimmer.Areas[intAreaID - 1].Seq.RemoveAt(tvSeries.SelectedNode.Index);
                    tvSeries.SelectedNode.Remove();
                }
            }
        }

        private void tsmCopySce_Click(object sender, EventArgs e)
        {
            if (oDimmer == null) return;
            if (oDimmer.Areas == null) return;
            if (tvSeries.SelectedNode == null) return;
            if (tvSeries.SelectedNode.Level == 0) return;

            mintCopySeriesID = tvSeries.SelectedNode.Index;
        }

        private void tsmPasteSce_Click(object sender, EventArgs e)
        {
            if (oDimmer == null) return;
            if (oDimmer.Areas == null) return;
            if (tvSeries.SelectedNode == null) return;
            if (mintCopySeriesID == -1) return;

            TreeNode oNode = tvSeries.SelectedNode;
            if (oNode.Level != 0)
            {
                oNode = oNode.Parent;
            }
            AddNewSeriesToForm(1, oNode.Index);
        }

        private void DgChns_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            DgChns.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void DgvSeries_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (tvSeries.SelectedNode == null) return;
            if (mblnIsShowing) return;
            if (e.RowIndex == -1) return;
            if (e.ColumnIndex == -1) return;
            if (DgvSeries.RowCount == 0) return;

            int intAreaID = Convert.ToInt16(tvSeries.SelectedNode.Parent.Text.Split('-')[0].ToString()) - 1;
            int intIndex = e.RowIndex;

            if (e.ColumnIndex == 1)
            {
                int intTmp = int.Parse(DgvSeries[1, e.RowIndex].Value.ToString().Split('-')[0].ToString());
                oDimmer.Areas[intAreaID].Seq[currentSerieId - 1].SceneIDs[intIndex] = byte.Parse(intTmp.ToString());
            }
            else if (e.ColumnIndex == 2)
            {
                DgvSeries[2, e.RowIndex].Value = HDLPF.GetStringFrmTimeMs(int.Parse(txtSeries.Text.ToString()));
                oDimmer.Areas[intAreaID].Seq[currentSerieId - 1].RunTimeH[intIndex] = byte.Parse((int.Parse(txtSeries.Text) / 256).ToString());
                oDimmer.Areas[intAreaID].Seq[currentSerieId - 1].RunTimeL[intIndex] = byte.Parse((int.Parse(txtSeries.Text) % 256).ToString());
            }
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            if (oDimmer == null) return;
            Cursor.Current = Cursors.WaitCursor;
            oDimmer.SaveInfoToDb();
            Cursor.Current = Cursors.Default;
        }

        void SetVisableForDownOrUpload(bool blnIsEnable)
        {
            tsbDown.Enabled = blnIsEnable;
            tsbUpload.Enabled = blnIsEnable;

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

        private void DgvSeries_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            if (DgvSeries.RowCount == 0) return;
            if (e == null) return;
            if (tvSeries.SelectedNode == null) return;
            if (oDimmer.Areas == null) return;

            TreeNode oNode = tvSeries.SelectedNode;
            if (oNode.Level == 0) return;

            byte bytAreaID = byte.Parse(oNode.Text.Split('-')[0].ToString()); // 取区号
            if (oNode.Level != 0)
            {
                bytAreaID = byte.Parse(oNode.Parent.Text.Split('-')[0].ToString()); // 取区号
            }

            Dimmer.Area oArea = null;

            foreach (Dimmer.Area Tmp in oDimmer.Areas)
            {
                if (Tmp.ID == bytAreaID)
                {
                    oArea = Tmp;
                    break;
                }
            }
            if (oArea == null) return;
            if (oArea.Scen == null) return;

            oArea.Seq[oNode.Index].SceneIDs.RemoveAt(e.RowIndex);
            oArea.Seq[oNode.Index].RunTimeH.RemoveAt(e.RowIndex);
            oArea.Seq[oNode.Index].RunTimeL.RemoveAt(e.RowIndex);
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            if (isSelectSeq) return;
            if (oDimmer == null) return;
            if (oDimmer.Areas == null) return;
            //if (tbRemarkSeries.Text == "") return;
            if (cbstep.Text == "") return;
            if (cbSeriesRepeat.Text == "") return;
            if (tvSeries.SelectedNode == null) return;
            TreeNode oNode = tvSeries.SelectedNode;
            if (oNode.Level == 0) return;

            if (oDimmer.Areas[oNode.Parent.Index].Seq[oNode.Index] == null) //添加新序列
            {
                AddNewSeriesToForm(0, oNode.Parent.Index);
            }
            else  // 原有基础更新
            {
                oDimmer.Areas[oNode.Parent.Index].Seq[oNode.Index].Remark = tbRemarkSeries.Text;
                oDimmer.Areas[oNode.Parent.Index].Seq[oNode.Index].Steps = byte.Parse((cbstep.SelectedIndex).ToString());
                oDimmer.Areas[oNode.Parent.Index].Seq[oNode.Index].Times = byte.Parse(cbSeriesRepeat.SelectedIndex.ToString());
                if (cbSeqMode.SelectedIndex == 0) oDimmer.Areas[oNode.Parent.Index].Seq[oNode.Index].Mode = 4;
                else if (cbSeqMode.SelectedIndex == 1) oDimmer.Areas[oNode.Parent.Index].Seq[oNode.Index].Mode = 3;
                else if (cbSeqMode.SelectedIndex == 2) oDimmer.Areas[oNode.Parent.Index].Seq[oNode.Index].Mode = 2;
                else if (cbSeqMode.SelectedIndex == 3) oDimmer.Areas[oNode.Parent.Index].Seq[oNode.Index].Mode = 1;
                else if (cbSeqMode.SelectedIndex == 4) oDimmer.Areas[oNode.Parent.Index].Seq[oNode.Index].Mode = 0;
                oNode.Text = oNode.Text.Split('-')[0] + "-" + tbRemarkSeries.Text;
                ShowSequenceStepsToForm(oDimmer.Areas[oNode.Parent.Index], oDimmer.Areas[oNode.Parent.Index].Seq[oNode.Index]);
            }
            oNode.Text = oNode.Text.Split('-')[0].ToString() + "-" + tbRemarkSeries.Text;
        }

        private void frmDimmer_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                FlashWindow(this.Handle, true);
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
                DropNode.Nodes.Add(DragNode);
                int intLoadID = int.Parse(DragNode.Text.Split('-')[0].ToString());
                oDimmer.Chans[intLoadID - 1].intBelongs = int.Parse(DropNode.Text.Split('-')[0].ToString());
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


        private void tmiDefault_Click(object sender, EventArgs e)
        {

        }

        private void frmDimmer_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Dispose();
        }

        private void toolStripButton11_Click(object sender, EventArgs e)
        {
            if (oDimmer == null) return;
        }


        private void btnSeq_Click(object sender, EventArgs e)
        {
            if (oDimmer == null) return;
            if (oDimmer.Areas == null) return;
            if (tvSeries.SelectedNode == null) return;
            if (tvSeries.SelectedNode.Level == 0) return;

            byte bytAreaID = Convert.ToByte(tvSeries.SelectedNode.Parent.Text.Split('-')[0].ToString());
            byte bytSceID = byte.Parse(tvSeries.SelectedNode.Text.Split('-')[0].ToString());

            string strName = mystrName.Split('\\')[0].ToString();
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

            CsConst.mySends.AddBufToSndList(bytTmp, 0x001A, bytSubID, bytDevID, false, true, true,CsConst.minAllWirelessDeviceType.Contains(mywdDevicerType));
        }


        private void DgChns_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (oDimmer == null) return;
                if (oDimmer.Chans == null) return;
                if ((e.RowIndex == -1) || (e.ColumnIndex == -1)) return;
                if (DgChns[e.ColumnIndex, e.RowIndex].Value == null) return;
                if (DgChns.SelectedRows.Count == 0) return;
                if (isClick) return;
                if (DgChns[e.ColumnIndex, e.RowIndex].Value == null) DgChns[e.ColumnIndex, e.RowIndex].Value = "";
                for (int i = 0; i < DgChns.SelectedRows.Count; i++)
                {
                    string strTmp = "";
                    switch (e.ColumnIndex + 1)
                    {
                        case 2:
                            strTmp = DgChns[e.ColumnIndex, DgChns.SelectedRows[i].Index].Value.ToString();
                            oDimmer.Chans[DgChns.SelectedRows[i].Index].Remark = DgChns[e.ColumnIndex, DgChns.SelectedRows[i].Index].Value.ToString();
                            break;
                        case 3:
                            oDimmer.Chans[DgChns.SelectedRows[i].Index].LoadType = cbType.Items.IndexOf(DgChns[e.ColumnIndex, DgChns.SelectedRows[i].Index].Value.ToString());
                            break;
                        case 4:
                            strTmp = DgChns[e.ColumnIndex, DgChns.SelectedRows[i].Index].Value.ToString();
                            oDimmer.Chans[DgChns.SelectedRows[i].Index].MinValue = int.Parse(HDLPF.IsNumStringMode(strTmp, 0, 100));
                            break;
                        case 5:
                            strTmp = DgChns[e.ColumnIndex, DgChns.SelectedRows[i].Index].Value.ToString();
                            oDimmer.Chans[DgChns.SelectedRows[i].Index].MaxValue = int.Parse(HDLPF.IsNumStringMode(strTmp, 0, 100));
                            break;
                        case 6:
                            strTmp = DgChns[e.ColumnIndex, DgChns.SelectedRows[i].Index].Value.ToString();
                            oDimmer.Chans[DgChns.SelectedRows[i].Index].MaxLevel = int.Parse(HDLPF.IsNumStringMode(strTmp, 0, 100));
                            break;
                        case 11:
                            byte[] bytTmp = new byte[4];
                            bytTmp[0] = (byte)(DgChns.SelectedRows[i].Index + 1);
                            bytTmp[2] = 0;
                            bytTmp[3] = 0;


                            if (DgChns[e.ColumnIndex, DgChns.SelectedRows[i].Index].Value.ToString().ToLower() == "true")
                                bytTmp[1] = Convert.ToByte(DgChns[5, DgChns.SelectedRows[i].Index].Value.ToString());
                            else
                                bytTmp[1] = 0;

                            Cursor.Current = Cursors.WaitCursor;
                            if (CsConst.mySends.AddBufToSndList(bytTmp, 0x0031, SubnetID,DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(mywdDevicerType)) == false)
                            {
                                //DgChns.BeginInvoke(new SetvalueHandle(Setvalue), e.RowIndex);
                            }

                            Cursor.Current = Cursors.Default;
                            break;
                        case 7:
                            strTmp = DgChns[e.ColumnIndex, DgChns.SelectedRows[i].Index].Value.ToString();
                            oDimmer.Chans[DgChns.SelectedRows[i].Index].dimmingProfile = Convert.ToByte(cbProfile.Items.IndexOf(strTmp));
                            break;
                        case 8:
                            strTmp = DgChns[e.ColumnIndex, DgChns.SelectedRows[i].Index].Value.ToString();
                            if (mywdDevicerType == 455)
                                oDimmer.Chans[DgChns.SelectedRows[i].Index].outPutType = Convert.ToByte(cbOutputType.Items.IndexOf(strTmp));
                            else
                                oDimmer.Chans[DgChns.SelectedRows[i].Index].DimmingMode = Convert.ToByte(cbOutputType.Items.IndexOf(strTmp));
                            break;
                        case 9: //DALI 掉电时亮度
                            strTmp = DgChns[e.ColumnIndex, DgChns.SelectedRows[i].Index].Value.ToString();
                            oDimmer.Chans[DgChns.SelectedRows[i].Index].levelIfNoPower = Byte.Parse(HDLPF.IsNumStringMode(strTmp, 0, 100));
                            break;
                        case 10:
                            strTmp = DgChns[e.ColumnIndex, DgChns.SelectedRows[i].Index].Value.ToString();
                            oDimmer.Chans[DgChns.SelectedRows[i].Index].levelWhenPowerOn = Byte.Parse(HDLPF.IsNumStringMode(strTmp, 0, 100));
                            break;
                    }
                    DgChns.SelectedRows[i].Cells[e.ColumnIndex].Value = DgChns[e.ColumnIndex, e.RowIndex].Value.ToString();
                }
            }
            catch
            {
            }
        }

        private void tsbDown_Click(object sender, EventArgs e)
        {
            try
            {
                EditableModeSetCtrlsUnvisible(false);
                Byte bytTag = Convert.ToByte(((ToolStripButton)sender).Tag.ToString());
                Boolean blnShowMsg = (CsConst.MyEditMode != 1);
                if (tab1.SelectedTab.Name == "tabDali") return;
                if (HDLPF.UploadOrReadGoOn(this.Text, bytTag, blnShowMsg))
                {
                    Cursor.Current = Cursors.WaitCursor;
                    // SetVisableForDownOrUpload(false);
                    // ReadDownLoadThread();  //增加线程，使得当前窗体的任何操作不被限制

                    CsConst.MyUPload2DownLists = new List<byte[]>();

                    string strName = mystrName.Split('\\')[0].ToString();
                    byte bytSubID = byte.Parse(strName.Split('-')[0]);
                    byte bytDevID = byte.Parse(strName.Split('-')[1]);

                    byte[] ArayRelay = new byte[] { bytSubID, bytDevID, (byte)(mywdDevicerType / 256), (byte)(mywdDevicerType % 256),
                        (byte)(tab1.SelectedIndex + 1),(byte)(myintDIndex / 256), (byte)(myintDIndex % 256), };
                    CsConst.MyUPload2DownLists.Add(ArayRelay);
                    CsConst.MyUpload2Down = Convert.ToByte(((ToolStripButton)sender).Tag.ToString());
                    FrmDownloadShow Frm = new FrmDownloadShow();
                    if (CsConst.MyUpload2Down == 0) Frm.FormClosing += new FormClosingEventHandler(UpdateDisplayInformationAccordingly);
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
            if (tab1.SelectedTab.Name =="tp1")
            {
                DgChns.Rows.Clear();
                UpdateBasicInfoOfLeftPanel();
                LoadBasicInformationToForm();
            }
            else if (tab1.SelectedTab.Name == "tabZone")
            {
                showZoneInfo();
            }
            else if (tab1.SelectedTab.Name == "tabScene")
            {
                ShowAreasToListview(tvScene, 2);
            }
            else if (tab1.SelectedTab.Name == "tp4")
            {//显示区域
                ShowAreasToListview(tvSeries, 3);
            }
            #endregion
            Cursor.Current = Cursors.Default;
            this.BringToFront();
        }

        private void showSenceInfo(Byte AreaID, Byte SceneID)
        {
            if (oDimmer == null) return;
            if (oDimmer.Areas == null) return;

            if (oDimmer.Areas.Count < AreaID + 1) return;
            
            Dimmer.Area oArea = oDimmer.Areas[AreaID];

            if (oArea.Scen == null || oArea.Scen.Count == 0) return;
            if (oArea.Scen.Count < SceneID + 1) return;

            Dimmer.Scene oSce = oArea.Scen[SceneID];
            if (SceneID == 0) oSce.light = (new Byte[oDimmer.Chans.Count]).ToList();

            for (int i = 0; i < oDimmer.Chans.Count; i++)
            {
                if (oDimmer.Chans[i].intBelongs == oArea.ID)
                {
                    Object[] obj = new Object[] { oDimmer.Chans[i].ID,oDimmer.Chans[i].Remark,oSce.light[i]};
                    dgScene.Rows.Add(obj);
                }
            }

        }

        private void frmDimmer_Shown(object sender, EventArgs e)
        {
            SetCtrlsVisbleWithDifferentDeviceType(); 
            if (CsConst.MyEditMode == 0)
            {
                LoadBasicInformationToForm();
            }
            else if (CsConst.MyEditMode == 1) //在线模式
            {
                toolStrip1.Visible = false;
                if (oDimmer.MyRead2UpFlags[0] == false)
                {
                    tsbDown_Click(tsbDown, null);
                }
                else
                {
                    LoadBasicInformationToForm();
                }
            }
        }

        private void DgvSeries_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (mblnIsShowing) return;
                if (e.RowIndex == -1) return;
                if (e.ColumnIndex == -1) return;
                if (DgvSeries.RowCount == 0) return;
                if (DgvSeries.SelectedRows == null || DgvSeries.SelectedRows.Count == 0) return;
                if (DgvSeries[e.ColumnIndex, e.RowIndex].Value == null) DgvSeries[e.ColumnIndex, e.RowIndex].Value = "";
                int intAreaID = tvSeries.SelectedNode.Parent.Index;

                for (int i = 0; i < DgvSeries.SelectedRows.Count; i++)
                {
                    DgvSeries.SelectedRows[i].Cells[e.ColumnIndex].Value = DgvSeries[e.ColumnIndex, e.RowIndex].Value.ToString();

                    int intIndex = DgvSeries.SelectedRows[i].Index;

                    if (e.ColumnIndex == 1)
                    {
                        int intTmp = int.Parse(DgvSeries[1, DgvSeries.SelectedRows[i].Index].Value.ToString().Split('-')[0].ToString());
                        oDimmer.Areas[intAreaID].Seq[currentSerieId - 1].SceneIDs[intIndex] = byte.Parse(intTmp.ToString());
                    }
                    else if (e.ColumnIndex == 2)
                    {
                        DgvSeries[2, DgvSeries.SelectedRows[i].Index].Value = HDLPF.GetStringFrmTimeMs(int.Parse(txtSeries.Text.ToString()));
                        oDimmer.Areas[intAreaID].Seq[currentSerieId - 1].RunTimeH[intIndex] = byte.Parse((int.Parse(txtSeries.Text) / 256).ToString());
                        oDimmer.Areas[intAreaID].Seq[currentSerieId - 1].RunTimeL[intIndex] = byte.Parse((int.Parse(txtSeries.Text) % 256).ToString());
                    }
                }
                if (e.ColumnIndex == 1)
                {
                    int intTmp = int.Parse(DgvSeries[1, e.RowIndex].Value.ToString().Split('-')[0].ToString());
                    oDimmer.Areas[intAreaID].Seq[currentSerieId - 1].SceneIDs[e.RowIndex] = byte.Parse(intTmp.ToString());
                }
                if (e.ColumnIndex == 2)
                {
                    DgvSeries[2, e.RowIndex].Value = HDLPF.GetStringFrmTimeMs(int.Parse(txtSeries.Text.ToString()));
                    oDimmer.Areas[intAreaID].Seq[currentSerieId - 1].RunTimeH[e.RowIndex] = byte.Parse((int.Parse(txtSeries.Text) / 256).ToString());
                    oDimmer.Areas[intAreaID].Seq[currentSerieId - 1].RunTimeL[e.RowIndex] = byte.Parse((int.Parse(txtSeries.Text) % 256).ToString());
                }
            }
            catch
            {
            }
        }

        private void DgvSeries_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            DgvSeries.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            /*Cursor.Current = Cursors.WaitCursor;
            frmPrint frmView = new frmPrint(0, 0, myintDIndex);
            frmView.Show();
            Cursor.Current = Cursors.Default;
            */
        }

        private void tmRead_Click(object sender, EventArgs e)
        {
            tsbDown_Click(tsbDown, null);
        }

        private void frmDimmer_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Dispose();
        }

        private void tmUpload_Click(object sender, EventArgs e)
        {
            tsbDown_Click(tsbUpload, null);
        }

        private void chbBroadcast_CheckedChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                byte[] ArayMain = new byte[1];
                if (VoltageSelection.Visible) ArayMain = new byte[2];
                if (chbBroadcast.Checked)
                    ArayMain[0] = 1;
                if (radioButton110V.Checked && VoltageSelection.Visible)
                    ArayMain[1] = 1;
                if (CsConst.mySends.AddBufToSndList(ArayMain, 0x1808, SubnetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(mywdDevicerType)) == true)
                {
                    if (oDimmer != null)
                    {
                        oDimmer.BroadcastChannelStatus = ArayMain[0];
                        if (VoltageSelection.Visible)
                            oDimmer.LoadType = ArayMain[1];
                    }
                }
            }
            catch { }
            Cursor.Current = Cursors.Default;
        }

        private void btnAddZone_Click(object sender, EventArgs e)
        {
            if (oDimmer == null) return;
            if (oDimmer.Chans == null || oDimmer.Chans.Count == 0) return;
            if (tvZone.Nodes.Count >= oDimmer.Chans.Count) return;
            if (dgvZoneChn1.RowCount <= 0) return;
            int ID = 1;
            int max = 1;
            int min = 1;
            if (oDimmer.Areas.Count > 0)
            {
                foreach (Dimmer.Area a in oDimmer.Areas)
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
                        for (int j = 0; j < oDimmer.Areas.Count; j++)
                        {
                            if (oDimmer.Areas[j].ID == i)
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


            Dimmer.Area temp = new Dimmer.Area();
            temp.ID = Convert.ToByte(ID);
            temp.Remark = "";
            temp.Scen = new List<Dimmer.Scene>();
            for (int j = 0; j <= 12; j++)
            {
                Dimmer.Scene oSce = new Dimmer.Scene();
                oSce.ID = (byte)j;
                oSce.Remark = "Scene" + j.ToString();
                oSce.light = new byte[oDimmer.Chans.Count].ToList();
                oSce.Time = 0;
                temp.Scen.Add(oSce);
            }
            temp.bytDefaultSce = 0;
            oDimmer.Areas.Add(temp);
            tvZone.Nodes.Add(temp.ID.ToString(), (tvZone.Nodes.Count + 1).ToString() + "-" + temp.Remark, 1, 1);
            lb22.Text = tvZone.Nodes.Count.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (tvZone.Nodes == null) return;
            if (oDimmer == null) return;
            if (oDimmer.Areas == null) return;
            if (tvZone.SelectedNode == null) return;
            TreeNode node = tvZone.SelectedNode;
            if (node.Level == 0)
            {
                int intAreaID = tvZone.SelectedNode.Index;
                for (int i = intAreaID + 1; i < oDimmer.Areas.Count; i++)
                {
                    for (int j = 0; j < oDimmer.Chans.Count; j++)
                    {
                        if (oDimmer.Chans[j].intBelongs == oDimmer.Areas[i].ID)
                        {
                            oDimmer.Chans[j].intBelongs = Convert.ToByte(oDimmer.Areas[i].ID - 1);
                        }
                    }
                    oDimmer.Areas[i].ID = Convert.ToByte(oDimmer.Areas[i].ID - 1);
                }
                oDimmer.Areas.RemoveAt(intAreaID);

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
        }

        private void btn2_Click(object sender, EventArgs e)
        {
            if (tvZone.Nodes == null) return;
            if (oDimmer == null) return;
            if (oDimmer.Areas == null) return;
            if (tvZone.SelectedNode == null) return;
            TreeNode node = tvZone.SelectedNode;
            if (node.Level == 1)
            {
                DeleteNodeInAreaForm(node.Text);
                if (tvZone.SelectedNode.Parent.Nodes.Count == 1)
                {
                    DialogResult result = MessageBox.Show("Do you want to remove the blank area as well?","Warm Hint"
                                                , MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                    if (result == DialogResult.OK)
                    {
                        int intAreaID = tvZone.SelectedNode.Parent.Index;
                        oDimmer.Areas.RemoveAt(intAreaID);
                        tvZone.SelectedNode.Parent.Remove();
                    }
                } else tvZone.SelectedNode.Remove();
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

        private void btn1_Click(object sender, EventArgs e)
        {
            if (tvZone.SelectedNode != null && tvZone.SelectedNode.Level == 0)
            {
                foreach (DataGridViewRow Tmp in dgvZoneChn1.SelectedRows)
                {
                    if (Tmp.Selected)  // 确定被选中，添加至区域列表
                    {
                        int intLoadID = int.Parse(Tmp.Cells[0].Value.ToString());
                        oDimmer.Chans[intLoadID - 1].intBelongs = Convert.ToByte(tvZone.SelectedNode.Name.ToString());
                        tvZone.SelectedNode.Nodes.Insert(0, null, Tmp.Cells[0].Value.ToString() + "-" + oDimmer.Chans[intLoadID - 1].Remark, 0, 0);
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
                        oDimmer.Chans[intLoadID - 1].intBelongs = Convert.ToByte(tvZone.SelectedNode.Parent.Name.ToString());
                        tvZone.SelectedNode.Parent.Nodes.Insert(0, null, Tmp.Cells[0].Value.ToString() + "-" + oDimmer.Chans[intLoadID - 1].Remark, 0, 0);
                        dgvZoneChn1.Rows.Remove(Tmp);
                    }
                }
                SortNodesByText(tvZone.SelectedNode.Parent);
            }
            lb11.Text = dgvZoneChn1.Rows.Count.ToString();
        }

        private void SortNodesByText(TreeNode node)
        {
            TreeNodeCollection NodeCollection = node.Nodes;
            TreeNode[] NodeAry = new TreeNode[NodeCollection.Count];
            int[,] arayChannelIDAndIndex = new int[2, NodeCollection.Count];
            try
            {
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
            catch
            { }
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
                        string strName =mystrName.Split('\\')[0].ToString();
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
                        if (CsConst.mySends.AddBufToSndList(bytTmp, 0x0031, bytSubID, bytDevID, false, false, false, CsConst.minAllWirelessDeviceType.Contains(mywdDevicerType)) == false)
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
            //oDimmer.isOutput = chbSceneOutput.Checked;
            //if (!chbSceneOutput.Checked)
            //{
            //    if (oDimmer == null) return;
            //    if (oDimmer.Areas == null) return;
            //    if (cbSence.Items.Count <= 0) return;
            //    byte bytAreaID = Convert.ToByte(cbSence.Text.Split('-')[0].ToString());
            //    byte bytSceID = Convert.ToByte(cbChooseScene.SelectedIndex);
            //    string strName = mystrName.Split('\\')[0].ToString();
            //    byte bytSubID = byte.Parse(strName.Split('-')[0]);
            //    byte bytDevID = byte.Parse(strName.Split('-')[1]);
            //    byte[] bytTmp = new byte[2];
            //    bytTmp[0] = bytAreaID;
            //    bytTmp[1] = bytSceID;
            //    CsConst.mySends.AddBufToSndList(bytTmp, 0xF076, bytSubID, bytDevID, false, false, false,CsConst.minAllWirelessDeviceType.Contains(mywdDevicerType));
            //}
        }

        private void SceneRuntime_TextChanged(object sender, EventArgs e)
        {
            //if (cbSence.Items.Count < 0 || cbSence.SelectedIndex < 0) return;
            //if (isChangeScene) return;
            //string str = "";
            //str = SceneRuntime.Text;
            //oDimmer.Areas[cbSence.SelectedIndex].Scen[cbChooseScene.SelectedIndex].Time = Convert.ToInt32(str);
        }

        private void cbChooseScene_SelectedIndexChanged(object sender, EventArgs e)
        {
            //chbSyn.Checked = false;
            //isChangeScene = true;
            //chbSceneOutput.Checked = false;
            //oDimmer.isOutput = false;
            //oDimmer.isModifyScenesSyn = false;
            //txtSceneRemark.Text = cbChooseScene.Text.Split('-')[1].ToString();
            //Dimmer.Scene temp = oDimmer.Areas[cbSence.SelectedIndex].Scen[cbChooseScene.SelectedIndex];
            //SceneRuntime.Text = temp.Time.ToString();
            //List<ChnSenceInfomation> Scene = new List<ChnSenceInfomation>();
            //panel11.Controls.Clear();
            //for (int i = 0; i < oDimmer.Chans.Count; i++)
            //{
            //    if (oDimmer.Chans[i].intBelongs == cbSence.SelectedIndex + 1)
            //    {
            //        ChnSenceInfomation tmp = new ChnSenceInfomation();
            //        tmp.ID = Convert.ToByte(oDimmer.Chans[i].ID);
            //        tmp.light = oDimmer.Areas[cbSence.SelectedIndex].Scen[cbChooseScene.SelectedIndex].light[i];
            //        tmp.Remark = oDimmer.Chans[i].Remark;
            //        tmp.type = 1;
            //        if (cbChooseScene.SelectedIndex == 0) tmp.light = 0;
            //        Scene.Add(tmp);
            //    }
            //}
            //SenceDesign sencedesign = new SenceDesign(panel11, Scene, mywdDevicerType, cbSence.SelectedIndex, cbChooseScene.SelectedIndex, oDimmer, mystrName);
            //isChangeScene = false;
        }

        private void cbSence_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (oDimmer == null) return;
            //if (oDimmer.Areas == null) return;
            //Dimmer.Area temp = oDimmer.Areas[cbSence.SelectedIndex];
            //if (temp.bytDefaultSce == 255)
            //{
            //    rbScenes1.Checked = true;
            //}
            //else
            //{
            //    rbScenes2.Checked = true;
            //    txtSceneRestore.Text = temp.bytDefaultSce.ToString();
            //}
            //cbChooseScene.Items.Clear();
            //if (temp.Scen == null) temp.Scen = new List<Dimmer.Scene>();
            //for (int i = 0; i < temp.Scen.Count; i++)
            //{
            //    cbChooseScene.Items.Add((i).ToString() + "-" + temp.Scen[i].Remark.ToString());
            //}
            //if (cbChooseScene.Items.Count > 0)
            //{
            //    cbChooseScene.SelectedIndex = 0;
            //    cbChooseScene_SelectedIndexChanged(null, null);
            //}
        }

        private void rbScenes1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void txtSceneRestore_TextChanged(object sender, EventArgs e)
        {

        }

        private void DgChns_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            isClick = true;
            EditableModeSetCtrlsUnvisible(false);
            try
            {
                if (DgChns[e.ColumnIndex, e.RowIndex].Value == null) return;
                if (e.RowIndex >= 0)
                {
                    if (e.ColumnIndex == 1)
                    {
                        txtRemark.Text = DgChns[e.ColumnIndex, e.RowIndex].Value.ToString();
                        HDLSysPF.addcontrols(e.ColumnIndex, e.RowIndex, txtRemark, DgChns);
                        if (txtRemark.Visible) txtRemark_TextChanged(null, null);
                        txtRemark.Focus();
                    }
                    else if (e.ColumnIndex == 2)
                    {
                        cbType.Text = DgChns[e.ColumnIndex, e.RowIndex].Value.ToString();
                        HDLSysPF.addcontrols(e.ColumnIndex, e.RowIndex, cbType, DgChns);
                    }
                    else if (e.ColumnIndex == 3)
                    {
                        txtMinLimit.Text = DgChns[e.ColumnIndex, e.RowIndex].Value.ToString();
                        HDLSysPF.addcontrols(e.ColumnIndex, e.RowIndex, txtMinLimit, DgChns);
                    }
                    else if (e.ColumnIndex == 4)
                    {
                        txtMaxLimit.Text = DgChns[e.ColumnIndex, e.RowIndex].Value.ToString();
                        HDLSysPF.addcontrols(e.ColumnIndex, e.RowIndex, txtMaxLimit, DgChns);
                    }
                    else if (e.ColumnIndex == 5)
                    {
                        txtMaxLevel.Text = DgChns[e.ColumnIndex, e.RowIndex].Value.ToString();
                        HDLSysPF.addcontrols(e.ColumnIndex, e.RowIndex, txtMaxLevel, DgChns);
                    }
                    else if (e.ColumnIndex == 6)
                    {
                        cbProfile.Text = DgChns[e.ColumnIndex, e.RowIndex].Value.ToString();
                        HDLSysPF.addcontrols(e.ColumnIndex, e.RowIndex, cbProfile, DgChns);
                    }
                    else if (e.ColumnIndex == 7)
                    {
                        cbOutputType.Text = DgChns[e.ColumnIndex, e.RowIndex].Value.ToString();
                        HDLSysPF.addcontrols(e.ColumnIndex, e.RowIndex, cbOutputType, DgChns);
                    }
                    if (cbType.Visible) cbType_SelectedIndexChanged(null, null);
                    if (cbOutputType.Visible) cbOutputType_SelectedIndexChanged(null, null);
                    if (cbProfile.Visible) cbProfile_SelectedIndexChanged(null, null);
                    if (txtMaxLevel.Visible) txtMaxLevel_TextChanged(null, null);
                    if (txtMaxLimit.Visible) txtMaxLimit_TextChanged(null, null);
                    if (txtMinLimit.Visible) txtMinLimit_TextChanged(null, null);
                }
            }
            catch
            { }
            isClick = false;
        }

        private void DgChns_Scroll(object sender, ScrollEventArgs e)
        {
            EditableModeSetCtrlsUnvisible(false);
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

        private void btnSaveChn_Click(object sender, EventArgs e)
        {
            tsbDown_Click(tsbUpload, null);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            tsbDown_Click(tsbUpload, null);
        }

        private void btnSaveScenes_Click(object sender, EventArgs e)
        {
            tsbDown_Click(tsbUpload, null);
        }

        private void btnSaveSequence_Click(object sender, EventArgs e)
        {
            tsbDown_Click(tsbUpload, null);
        }

        private void btnSaveAndClose1_Click(object sender, EventArgs e)
        {
            btnSaveChn_Click(null, null);
            this.Close();
        }

        private void btnSaveAndClose2_Click(object sender, EventArgs e)
        {
            btnSave_Click(null, null);
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

        private void txtZoneRemark_TextChanged(object sender, EventArgs e)
        {
            if (tvZone.Nodes == null) return;
            if (oDimmer == null) return;
            if (oDimmer.Areas == null) return;
            if (tvZone.SelectedNode == null) return;
            if (MyBlnReading) return;
            TreeNode node = tvZone.SelectedNode;
            if (node.Level == 0)
            {
                oDimmer.Areas[node.Index].Remark = txtZoneRemark.Text.Trim();
                node.Text = (node.Index + 1).ToString() + "-" + txtZoneRemark.Text.Trim();
            }
            else if (node.Level == 1)
            {
                node = node.Parent;
                oDimmer.Areas[node.Index].Remark = txtZoneRemark.Text.Trim();
                node.Text = (node.Index + 1).ToString() + "-" + txtZoneRemark.Text.Trim();
            }
        }

        private void btnRef4_Click(object sender, EventArgs e)
        {
            if (tab1.SelectedTab.Name != "tabDaliStatus")
            {
                tsbDown_Click(tsbDown, null);
            }
            else
            {
                RefreshDallastsCurrentStatus();
            }
        }


        void RefreshDallastsCurrentStatus()
        {
            if (oDimmer == null) return;
            Cursor.Current = Cursors.WaitCursor;
            lvDaliStatus.Items.Clear();
            try
            {
                if (arrCurrentActiveDaliAddress == null || arrCurrentActiveDaliAddress.Count == 0)
                    ReadAvaibleDaliBallastsAddressesList();

                Byte[] ArayMain =  new Byte[]{0};
                   
                if (CsConst.mySends.AddBufToSndList(ArayMain, 0xA008, SubnetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(mywdDevicerType)) == true)
                {
                    Byte[] arrReadBackAddressesList = new Byte[CsConst.myRevBuf[16] - 11];
                    Array.Copy(CsConst.myRevBuf,25,arrReadBackAddressesList,0,arrReadBackAddressesList.Length);

                    if (arrCurrentActiveDaliAddress.Count == 0) return;
                    #region
                        foreach (Byte bTmp in arrCurrentActiveDaliAddress)
                        {
                            ListViewItem oLv = new ListViewItem();
                            oLv.SubItems.Add(bTmp.ToString());
                            String sIndexs = Convert.ToString(arrReadBackAddressesList[bTmp -1],2);
                            for (Byte bytJ = 0; bytJ <= 5; bytJ++)
                            {
                                if (sIndexs[bytJ].ToString() == "1")
                                {
                                    if (CsConst.iLanguageId == 1)
                                    {
                                        oLv.SubItems.Add(CsConst.mstrDALIFail[bytJ]);
                                    }
                                    else if (CsConst.iLanguageId == 0)
                                    {
                                        oLv.SubItems.Add(CsConst.mstrDALIFailEng[bytJ]);
                                    }
                                }
                                else
                                {
                                    if (CsConst.iLanguageId == 1)
                                    {
                                        oLv.SubItems.Add(CsConst.mstrDALISuccess[bytJ]);
                                    }
                                    else if (CsConst.iLanguageId == 0)
                                    {
                                        oLv.SubItems.Add(CsConst.mstrDALISuccessEng[bytJ]);
                                    }
                                }
                            }
                        }
                        #endregion
                }
            }
            catch
            {
                Cursor.Current = Cursors.Default;
            }
            Cursor.Current = Cursors.Default;
        }

        void ReadAvaibleDaliBallastsAddressesList()
        {
            arrCurrentActiveDaliAddress = new List<byte>();

            try
            {
                Byte[] ArayMain = new Byte[] { 1};
                // 获取有效的设备地址
                if (CsConst.mySends.AddBufToSndList(ArayMain, 0xA008, SubnetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(mywdDevicerType)) == true)
                {
                    Byte[] arrReadBackAddressesList = new Byte[CsConst.myRevBuf[16] - 11];
                    Array.Copy(CsConst.myRevBuf, 25, arrReadBackAddressesList, 0, arrReadBackAddressesList.Length);

                    #region
                    Byte bAddressIndex = 1;
                    foreach (Byte bTmp in arrReadBackAddressesList)
                    {
                        if (bTmp == 255) arrCurrentActiveDaliAddress.Add(bAddressIndex);
                        bAddressIndex++;
                    }
                    #endregion
                }
            }
            catch
            { 
            }
        }

        private void btnSave4_Click(object sender, EventArgs e)
        {
            tsbDown_Click(tsbUpload, null);
        }

        private void btnSaveAndClose4_Click_1(object sender, EventArgs e)
        {
            tsbDown_Click(tsbUpload, null);
            this.Close();
        }

        void ShowScenChannelInformation(TreeNode oNode)
        {
            isChangeScene = true;
            mblnIsShowing = true;
            dgScene.Rows.Clear();

            if (oNode == null) return;
            if (oDimmer.Areas == null) return;
            if (oNode.Level == 0)
            {
                currentAreaId = (Byte)oNode.Index;
            }
            else
            {
                currentAreaId = (Byte)oNode.Parent.Index;
                currentSceneId = (Byte)oNode.Index;
            }
            HDLSysPF.ListAllSceneNameToCombobox(oDimmer, currentAreaId, cbRestoreList, true);
            if (oDimmer.Areas[currentAreaId].bytDefaultSce > cbRestoreList.Items.Count)
                cbRestoreList.SelectedIndex = 13;
            else 
                cbRestoreList.SelectedIndex = oDimmer.Areas[currentAreaId].bytDefaultSce;
            SceneChn.Visible = false;
           
            tbSceneName.Text = oDimmer.Areas[currentAreaId].Scen[currentSceneId].Remark;
            tbRunningTime.Text = Convert.ToString(oDimmer.Areas[currentAreaId].Scen[currentSceneId].Time);
            if (cboDaliTime.Visible)
            {
                 if (oDimmer.Areas[currentAreaId].Scen[currentSceneId].Time>16) oDimmer.Areas[currentAreaId].Scen[currentSceneId].Time =0;
                 cboDaliTime.SelectedIndex = oDimmer.Areas[currentAreaId].Scen[currentSceneId].Time;
            }


            showSenceInfo(currentAreaId, currentSceneId);
            isChangeScene = false;
            mblnIsShowing = false;
            chbOutputS_CheckedChanged(chbOutputS, null);
        }

        private void tvScene_MouseDown(object sender, MouseEventArgs e)
        {
            isChangeScene = true;
            mblnIsShowing = true;
            dgScene.Rows.Clear();

            TreeNode oNode = tvScene.GetNodeAt(e.Location);

        }

        private void dgScene_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (tvScene.SelectedNode == null) return;
            if (e.RowIndex == -1) return;
            if (e.ColumnIndex == -1) return;
            if (dgScene.RowCount == 0) return;

            for (int i = 0; i < dgScene.SelectedRows.Count; i++)
            {
                if (e.ColumnIndex == 2)
                {
                    Byte currentIntensity = Convert.ToByte(SceneChn.Text.ToString());
                    dgScene[2, e.RowIndex].Value = currentIntensity.ToString();
                    Byte ChnID = Convert.ToByte(dgScene[0, e.RowIndex].Value.ToString());
                    oDimmer.Areas[currentAreaId].Scen[currentSceneId].light[ChnID - 1] = currentIntensity;
                }
            }
        }

        void SceneChn_TextChanged(object sender, EventArgs e)
        {
            if ((dgScene.CurrentCell.RowIndex == -1) || (dgScene.CurrentCell.ColumnIndex == -1)) return;
            if (SceneChn.Visible)
            {
                dgScene[2, dgScene.CurrentCell.RowIndex].Value = SceneChn.Text.ToString();
                chbOutputS_CheckedChanged(chbOutputS, null);
            }
        }


        private void dgScene_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            dgScene.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void dgScene_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (e.ColumnIndex == 2)
                {
                    SceneChn.Text = dgScene[2, e.RowIndex].Value.ToString();
                    HDLSysPF.addcontrols(2, e.RowIndex, SceneChn, dgScene);
                    SceneChn.TextChanged += SceneChn_TextChanged;
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
            oDimmer.Areas[currentAreaId].Scen[currentSceneId].Remark = tbSceneName.Text.Trim();
        }

        private void cbRestoreList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (mblnIsShowing) return;
            oDimmer.Areas[currentAreaId].bytDefaultSce = Convert.ToByte(cbRestoreList.SelectedIndex);
        }

        private void tbRunningTime_TextChanged(object sender, EventArgs e)
        {
            if (mblnIsShowing) return;
            oDimmer.Areas[currentAreaId].Scen[currentSceneId].Time= Convert.ToInt32(tbRunningTime.Text); 
        }

        private void dgScene_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (mblnIsShowing) return;
                if (tvScene.SelectedNode == null) return;
                if (e.RowIndex == -1) return;
                if (e.ColumnIndex == -1) return;
                if (dgScene.RowCount == 0) return;

                Byte currentIntensity = Convert.ToByte(dgScene[e.ColumnIndex, e.RowIndex].Value.ToString());

                for (int i = 0; i < dgScene.SelectedRows.Count; i++)
                {
                    dgScene.SelectedRows[i].Cells[e.ColumnIndex].Value = currentIntensity.ToString();

                    int intIndex = dgScene.SelectedRows[i].Index;

                    if (e.ColumnIndex == 2)
                    {
                         Byte ChnID = Convert.ToByte(dgScene[0, intIndex].Value.ToString());
                         oDimmer.Areas[currentAreaId].Scen[currentSceneId].light[ChnID - 1] = currentIntensity;
                    }
                }
                
            }
            catch
            {
            }
        }

        private void chbOutputS_CheckedChanged(object sender, EventArgs e)
        {
            oDimmer.isOutput = chbOutputS.Checked;
            if (chbOutputS.Checked == true)
            {
                if (oDimmer == null) return;
                if (oDimmer.Areas == null) return;

                Byte TotalChnNumber = (Byte)oDimmer.Chans.Count;
                byte[] bytTmp = new byte[2 +TotalChnNumber] ;
                bytTmp[0] = (Byte)(currentAreaId + 1);
                bytTmp[1] = (Byte)(currentSceneId + 1);
                Array.Copy(oDimmer.Areas[currentAreaId].Scen[currentSceneId].light.ToArray(), 0, bytTmp, 2, TotalChnNumber);
                CsConst.mySends.AddBufToSndList(bytTmp, 0xF074, SubnetID, DeviceID, false, false, false,CsConst.minAllWirelessDeviceType.Contains(mywdDevicerType));
            }
        }

        private void testToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (oDimmer == null) return;
            if (oDimmer.Areas == null) return;
            if (tvSeries.SelectedNode == null) return;
            if (tvSeries.SelectedNode.Level == 0) return;

            byte[] bytTmp = new byte[2];
            bytTmp[0] = currentAreaId;
            if (((ToolStripMenuItem)sender).Tag == null || ((ToolStripMenuItem)sender).Tag.ToString() != "0")
            {
                bytTmp[1] = currentSerieId;
                ((ToolStripMenuItem)sender).Tag = "0";
            }
            else if (((ToolStripMenuItem)sender).Tag.ToString() == "0")
            {
                bytTmp[1] = 0;
                ((ToolStripMenuItem)sender).Tag = null;
            }

            CsConst.mySends.AddBufToSndList(bytTmp, 0x001A, SubnetID, DeviceID, false, true, true,CsConst.minAllWirelessDeviceType.Contains(mywdDevicerType));
        }

        private void tvScene_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {

        }

        private void tvScene_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (tvScene.SelectedNode == null) return;

            TreeNode oNode = tvScene.SelectedNode;
            ShowScenChannelInformation(oNode);
        }

        private void frmDimmer_FormClosed_1(object sender, FormClosedEventArgs e)
        {

        }

        private void chbSyn_CheckedChanged_1(object sender, EventArgs e)
        {

        }

        private void chbSyn_CheckedChanged(object sender, EventArgs e)
        {
            if (mblnIsShowing) return;
            if (chbSyn.Checked == true)
            {
                for (int i = 0; i < oDimmer.Areas[currentAreaId].Scen.Count; i++)
                {
                    oDimmer.Areas[currentAreaId].Scen[i].Time = Convert.ToByte(tbRunningTime.Text);
                }
                chbSyn.Checked = false;
                MessageBox.Show("SYN Running Done!","Hint");
            }
            
        }

        private void radioButtonLeading_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonLeading.Checked)
            {
                oDimmer.DimmingMode = 1;
            }
            else 
            {
                oDimmer.DimmingMode = 0;
            }
        }

        private void chbSyn_MouseLeave(object sender, EventArgs e)
        {
            if (chbSyn.Checked == false) HintScene.Text = "";
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btnView_Click(object sender, EventArgs e)
        {
            ClearDaliBallastsAddressesList();
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                ReadAvaibleDaliBallastsAddressesList();

                if (arrCurrentActiveDaliAddress == null || arrCurrentActiveDaliAddress.Count == 0) return;
                foreach (Byte bTmpAddress in arrCurrentActiveDaliAddress)
                {
                    lvAddresses.Groups[0].Items.Add(bTmpAddress.ToString());
                }
            }
            catch
            {
                Cursor.Current = Cursors.Default;
            }
            Cursor.Current = Cursors.Default;
        }

        void ClearDaliBallastsAddressesList()
        {
            for (int i = 0; i < lvAddresses.Groups.Count; i++)
            {
                lvAddresses.Groups[i].Items.Clear();
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                ClearDaliBallastsAddressesList();
            }
            catch
            { }
        }

        private void btnDelAddress_Click(object sender, EventArgs e)
        {
            if (tbDelAddress.Text == null || tbDelAddress.Text == "") return;
            Byte bTmpDaliAddress = Convert.ToByte(tbDelAddress.Text.ToString());
            try
            {
                Byte[] arrSendBuffer = new Byte[] { bTmpDaliAddress,255};

                if (CsConst.mySends.AddBufToSndList(arrSendBuffer, 0xA020, SubnetID, DeviceID, false, true, true, 
                    CsConst.minAllWirelessDeviceType.Contains(mywdDevicerType)) == true)
                {
                }
            }
            catch
            { }
        }

        private void btnNewAddress_Click(object sender, EventArgs e)
        {
            if (tbOldAddress.Text == null || tbOldAddress.Text == "") return;
            if (tbNewAddress.Text == null || tbNewAddress.Text == "") return;
            
            Byte bTmpDaliAddress = Convert.ToByte(tbOldAddress.Text.ToString());
            Byte bNewDaliAddress = Convert.ToByte(tbNewAddress.Text.ToString());
            try
            {
                Byte[] arrSendBuffer = new Byte[] { bTmpDaliAddress, bNewDaliAddress };

                if (CsConst.mySends.AddBufToSndList(arrSendBuffer, 0xA020, SubnetID, DeviceID, false, true, true,
                    CsConst.minAllWirelessDeviceType.Contains(mywdDevicerType)) == true)
                {
                }
            }
            catch
            { }
        }

        private void btnDuplicate_Click(object sender, EventArgs e)
        {
            if (tbDuplicate.Text == null || tbDuplicate.Text == "") return;
            Byte bTmpDaliAddress = Convert.ToByte(tbDuplicate.Text.ToString());
            try
            {
                Byte[] arrSendBuffer = new Byte[] { bTmpDaliAddress, 2 };

                if (CsConst.mySends.AddBufToSndList(arrSendBuffer, 0xA006, SubnetID, DeviceID, false, true, true,
                    CsConst.minAllWirelessDeviceType.Contains(mywdDevicerType)) == true)
                {
                }
            }
            catch
            { }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                Byte[] arrSendBuffer = new Byte[] { 0, 1 };

                if (CsConst.mySends.AddBufToSndList(arrSendBuffer, 0xA006, SubnetID, DeviceID, false, true, true,
                    CsConst.minAllWirelessDeviceType.Contains(mywdDevicerType)) == true)
                {
                }
            }
            catch
            { }
        }

        private void btnInitial_Click(object sender, EventArgs e)
        {
            try
            {
                Byte[] arrSendBuffer = new Byte[] { 0, 0,0 };

                if (CsConst.mySends.AddBufToSndList(arrSendBuffer, 0xA006, SubnetID, DeviceID, false, true, true,
                    CsConst.minAllWirelessDeviceType.Contains(mywdDevicerType)) == true)
                {
                }
            }
            catch
            { }
        }

        private void btnFlick_Click(object sender, EventArgs e)
        {
            if (cboArea.Text == null || cboArea.Text == "") return;
            Byte bTmpDaliAddress = Convert.ToByte(cboArea.SelectedIndex.ToString());  //2017/02/08 修改这里,cboArea添加items集合   【仍不可以手动输入Area号】
            //Byte bTmpDaliAddress = Convert.ToByte(cboArea.SelectedItem);       
            try
            {
                Boolean bIsSuccess = EnterSetupModeBeforeFlicking();

                if (bIsSuccess)
                {
                    Byte[] arrSendBuffer = new Byte[] { (Byte)(bTmpDaliAddress + 64) };

                    if (CsConst.mySends.AddBufToSndList(arrSendBuffer, 0xA014, SubnetID, DeviceID, false, true, true,
                        CsConst.minAllWirelessDeviceType.Contains(mywdDevicerType)) == true)
                    {
                    }
                }
            }
            catch
            { } 
        }

        Boolean EnterSetupModeBeforeFlicking()
        {
            Boolean bIsSuccess = false;
            try
            {
                bIsSuccess = CsConst.mySends.AddBufToSndList(null, 0xA014, SubnetID, DeviceID, false, true, true,
                   CsConst.minAllWirelessDeviceType.Contains(mywdDevicerType));
            }
            catch
            {
                return bIsSuccess;
            }
            return bIsSuccess;
        }

        private void tsbSelectAll_Click(object sender, EventArgs e)
        {
           // if (tl
        }

        private void tsbFlickAll_Click(object sender, EventArgs e)
        {
            if (cboArea.Text == null || cboArea.Text == "") return;
            Byte bTmpDaliAddress = Convert.ToByte(cboArea.SelectedIndex.ToString());
            try
            {
                Boolean bIsSuccess = EnterSetupModeBeforeFlicking();

                if (bIsSuccess)
                {
                    Byte[] arrSendBuffer = new Byte[] { 255 };

                    if (CsConst.mySends.AddBufToSndList(arrSendBuffer, 0xA014, SubnetID, DeviceID, false, true, true,
                        CsConst.minAllWirelessDeviceType.Contains(mywdDevicerType)) == true)
                    {
                    }
                }
            }
            catch
            { } 
        }

        private void tsbFlick_Click(object sender, EventArgs e)
        {
            if (cboArea.Text == null || cboArea.Text == "") return;
            Byte bTmpDaliAddress = Convert.ToByte(cboArea.SelectedIndex.ToString());
            try
            {
                Boolean bIsSuccess = EnterSetupModeBeforeFlicking();

                if (bIsSuccess)
                {
                    Byte[] arrSendBuffer = new Byte[] { 255 };

                    if (CsConst.mySends.AddBufToSndList(arrSendBuffer, 0xA014, SubnetID, DeviceID, false, true, true,
                        CsConst.minAllWirelessDeviceType.Contains(mywdDevicerType)) == true)
                    {
                    }
                }
            }
            catch
            { } 

        }

        private void cboDaliTime_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            oDimmer.Areas[currentAreaId].Scen[currentSceneId].Time = (Byte)cboDaliTime.SelectedIndex;
        }
    }
}
