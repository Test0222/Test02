namespace HDL_Buspro_Setup_Tool
{
    partial class frmDimmer
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDimmer));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.ListViewGroup listViewGroup1 = new System.Windows.Forms.ListViewGroup("Existed Addresses", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup2 = new System.Windows.Forms.ListViewGroup("New Added", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup3 = new System.Windows.Forms.ListViewGroup("Duplcate Addresses", System.Windows.Forms.HorizontalAlignment.Left);
            this.tab1 = new System.Windows.Forms.TabControl();
            this.cmNew = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tmRead = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
            this.tp1 = new System.Windows.Forms.TabPage();
            this.DgChns = new System.Windows.Forms.DataGridView();
            this.cl1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cl2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cl3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cl4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cl5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cl6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnDimmingProfile = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cl8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clFailLevel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clPowerOnLevel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cl0 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.panBasic = new System.Windows.Forms.Panel();
            this.groupBoxDimmingMode = new System.Windows.Forms.GroupBox();
            this.radioButtonTrailing = new System.Windows.Forms.RadioButton();
            this.radioButtonLeading = new System.Windows.Forms.RadioButton();
            this.VoltageSelection = new System.Windows.Forms.GroupBox();
            this.radioButton110V = new System.Windows.Forms.RadioButton();
            this.radioButton220V = new System.Windows.Forms.RadioButton();
            this.pnBroadcast = new System.Windows.Forms.Panel();
            this.chbBroadcast = new System.Windows.Forms.CheckBox();
            this.tabZone = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dgvZoneChn1 = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewCheckBoxColumn1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btn2 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.btn1 = new System.Windows.Forms.Button();
            this.btnAddZone = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tvZone = new System.Windows.Forms.TreeView();
            this.img2 = new System.Windows.Forms.ImageList(this.components);
            this.panel7 = new System.Windows.Forms.Panel();
            this.lb22 = new System.Windows.Forms.Label();
            this.lb11 = new System.Windows.Forms.Label();
            this.lb2 = new System.Windows.Forms.Label();
            this.lb1 = new System.Windows.Forms.Label();
            this.txtZoneRemark = new System.Windows.Forms.TextBox();
            this.lb3 = new System.Windows.Forms.Label();
            this.tabScene = new System.Windows.Forms.TabPage();
            this.gbScene = new System.Windows.Forms.GroupBox();
            this.dgScene = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewCheckBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.tvScene = new System.Windows.Forms.TreeView();
            this.panel3 = new System.Windows.Forms.Panel();
            this.cboDaliTime = new System.Windows.Forms.ComboBox();
            this.chbSyn = new System.Windows.Forms.CheckBox();
            this.cbRestoreList = new System.Windows.Forms.ComboBox();
            this.RestoreScene = new System.Windows.Forms.Label();
            this.chbOutputS = new System.Windows.Forms.CheckBox();
            this.tbRunningTime = new HDL_Buspro_Setup_Tool.TimeText();
            this.RunningTime = new System.Windows.Forms.Label();
            this.tbSceneName = new System.Windows.Forms.TextBox();
            this.lbSceneName = new System.Windows.Forms.Label();
            this.tp4 = new System.Windows.Forms.TabPage();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.DgvSeries = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SelectScens = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.Column7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.tvSeries = new System.Windows.Forms.TreeView();
            this.cmSequence = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmDeleteSce = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmCopySce = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmPasteSce = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.testToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imgs = new System.Windows.Forms.ImageList(this.components);
            this.panel9 = new System.Windows.Forms.Panel();
            this.cbSeqMode = new System.Windows.Forms.ComboBox();
            this.lbSeqMode = new System.Windows.Forms.Label();
            this.cbSeriesRepeat = new System.Windows.Forms.ComboBox();
            this.label18 = new System.Windows.Forms.Label();
            this.cbstep = new System.Windows.Forms.ComboBox();
            this.label17 = new System.Windows.Forms.Label();
            this.tbRemarkSeries = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.tabDali = new System.Windows.Forms.TabPage();
            this.gbDaliManagement = new System.Windows.Forms.GroupBox();
            this.cboArea = new System.Windows.Forms.ComboBox();
            this.lbArea = new System.Windows.Forms.Label();
            this.btnFlick = new System.Windows.Forms.Button();
            this.tbNewAddress = new System.Windows.Forms.TextBox();
            this.lbNewAddress = new System.Windows.Forms.Label();
            this.tbOldAddress = new System.Windows.Forms.TextBox();
            this.lbOldAddress = new System.Windows.Forms.Label();
            this.tbDelAddress = new System.Windows.Forms.TextBox();
            this.lbDelAddress = new System.Windows.Forms.Label();
            this.tbDuplicate = new System.Windows.Forms.TextBox();
            this.lbDuplcate = new System.Windows.Forms.Label();
            this.lbAdd = new System.Windows.Forms.Label();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnView = new System.Windows.Forms.Button();
            this.lbInitial = new System.Windows.Forms.Label();
            this.btnNewAddress = new System.Windows.Forms.Button();
            this.btnDelAddress = new System.Windows.Forms.Button();
            this.btnDuplicate = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnInitial = new System.Windows.Forms.Button();
            this.lvAddresses = new System.Windows.Forms.ListView();
            this.cmDali = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsbFlick = new System.Windows.Forms.ToolStripMenuItem();
            this.tsbFlickAll = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator10 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbSelectAll = new System.Windows.Forms.ToolStripMenuItem();
            this.tabDaliStatus = new System.Windows.Forms.TabPage();
            this.lvDaliStatus = new System.Windows.Forms.ListView();
            this.chAddress = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chStatus = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chLoad = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chOnOff = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chDimRange = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chDim = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chReset = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.cmChnScene = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tmSame = new System.Windows.Forms.ToolStripMenuItem();
            this.tmAscend = new System.Windows.Forms.ToolStripMenuItem();
            this.tmdescend = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tmiSce = new System.Windows.Forms.ToolStripMenuItem();
            this.tmiDefault = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.outputSceneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton5 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton6 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton7 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton9 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton10 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbDown = new System.Windows.Forms.ToolStripButton();
            this.tsbUpload = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton11 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.tsl1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsl2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsl3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsbar = new System.Windows.Forms.ToolStripProgressBar();
            this.tsbl4 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsbHint = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.HintScene = new System.Windows.Forms.ToolStripStatusLabel();
            this.panel16 = new System.Windows.Forms.Panel();
            this.btnRef4 = new System.Windows.Forms.Button();
            this.btnSaveAndClose4 = new System.Windows.Forms.Button();
            this.btnSave4 = new System.Windows.Forms.Button();
            this.toolStrip3 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.cboDevice = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.tbModel = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.tbDescription = new System.Windows.Forms.ToolStripLabel();
            this.tab1.SuspendLayout();
            this.cmNew.SuspendLayout();
            this.tp1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DgChns)).BeginInit();
            this.panBasic.SuspendLayout();
            this.groupBoxDimmingMode.SuspendLayout();
            this.VoltageSelection.SuspendLayout();
            this.pnBroadcast.SuspendLayout();
            this.tabZone.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvZoneChn1)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel7.SuspendLayout();
            this.tabScene.SuspendLayout();
            this.gbScene.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgScene)).BeginInit();
            this.groupBox6.SuspendLayout();
            this.panel3.SuspendLayout();
            this.tp4.SuspendLayout();
            this.groupBox8.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DgvSeries)).BeginInit();
            this.groupBox7.SuspendLayout();
            this.cmSequence.SuspendLayout();
            this.panel9.SuspendLayout();
            this.tabDali.SuspendLayout();
            this.gbDaliManagement.SuspendLayout();
            this.cmDali.SuspendLayout();
            this.tabDaliStatus.SuspendLayout();
            this.cmChnScene.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.panel16.SuspendLayout();
            this.toolStrip3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tab1
            // 
            this.tab1.ContextMenuStrip = this.cmNew;
            this.tab1.Controls.Add(this.tp1);
            this.tab1.Controls.Add(this.tabZone);
            this.tab1.Controls.Add(this.tabScene);
            this.tab1.Controls.Add(this.tp4);
            this.tab1.Controls.Add(this.tabDali);
            this.tab1.Controls.Add(this.tabDaliStatus);
            this.tab1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tab1.Location = new System.Drawing.Point(0, 28);
            this.tab1.Name = "tab1";
            this.tab1.SelectedIndex = 0;
            this.tab1.Size = new System.Drawing.Size(881, 426);
            this.tab1.TabIndex = 1;
            this.tab1.SelectedIndexChanged += new System.EventHandler(this.tab1_SelectedIndexChanged);
            // 
            // cmNew
            // 
            this.cmNew.Font = new System.Drawing.Font("Arial", 8.25F);
            this.cmNew.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.cmNew.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tmRead,
            this.toolStripSeparator9});
            this.cmNew.Name = "cmNew";
            this.cmNew.Size = new System.Drawing.Size(412, 32);
            // 
            // tmRead
            // 
            this.tmRead.Name = "tmRead";
            this.tmRead.Size = new System.Drawing.Size(411, 22);
            this.tmRead.Tag = "0";
            this.tmRead.Text = "Refresh(Clear memory data and reread from device )";
            this.tmRead.Click += new System.EventHandler(this.tmRead_Click);
            // 
            // toolStripSeparator9
            // 
            this.toolStripSeparator9.Name = "toolStripSeparator9";
            this.toolStripSeparator9.Size = new System.Drawing.Size(408, 6);
            // 
            // tp1
            // 
            this.tp1.BackColor = System.Drawing.SystemColors.Control;
            this.tp1.Controls.Add(this.DgChns);
            this.tp1.Controls.Add(this.panBasic);
            this.tp1.Location = new System.Drawing.Point(4, 27);
            this.tp1.Name = "tp1";
            this.tp1.Padding = new System.Windows.Forms.Padding(3);
            this.tp1.Size = new System.Drawing.Size(873, 395);
            this.tp1.TabIndex = 0;
            this.tp1.Text = "Basic Information";
            // 
            // DgChns
            // 
            this.DgChns.AllowUserToAddRows = false;
            this.DgChns.AllowUserToDeleteRows = false;
            this.DgChns.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.AliceBlue;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Calibri", 9F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.DgChns.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.DgChns.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DgChns.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.cl1,
            this.cl2,
            this.cl3,
            this.cl4,
            this.cl5,
            this.cl6,
            this.columnDimmingProfile,
            this.cl8,
            this.clFailLevel,
            this.clPowerOnLevel,
            this.cl0});
            this.DgChns.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DgChns.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.DgChns.EnableHeadersVisualStyles = false;
            this.DgChns.Location = new System.Drawing.Point(253, 3);
            this.DgChns.Name = "DgChns";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Calibri", 9F);
            dataGridViewCellStyle6.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.DgChns.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.DgChns.RowHeadersVisible = false;
            this.DgChns.RowHeadersWidth = 10;
            this.DgChns.RowTemplate.Height = 23;
            this.DgChns.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.DgChns.Size = new System.Drawing.Size(617, 389);
            this.DgChns.TabIndex = 8;
            this.DgChns.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DgChns_CellClick);
            this.DgChns.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.DgChns_CellValueChanged);
            this.DgChns.CurrentCellDirtyStateChanged += new System.EventHandler(this.DgChns_CurrentCellDirtyStateChanged);
            this.DgChns.Scroll += new System.Windows.Forms.ScrollEventHandler(this.DgChns_Scroll);
            // 
            // cl1
            // 
            this.cl1.HeaderText = "ID";
            this.cl1.Name = "cl1";
            this.cl1.ReadOnly = true;
            this.cl1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.cl1.Width = 32;
            // 
            // cl2
            // 
            this.cl2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle2.NullValue = "\"\"";
            this.cl2.DefaultCellStyle = dataGridViewCellStyle2;
            this.cl2.HeaderText = "Name";
            this.cl2.MaxInputLength = 20;
            this.cl2.Name = "cl2";
            this.cl2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // cl3
            // 
            this.cl3.HeaderText = "Load Type";
            this.cl3.Name = "cl3";
            this.cl3.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.cl3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // cl4
            // 
            dataGridViewCellStyle3.Format = "N0";
            dataGridViewCellStyle3.NullValue = "0";
            this.cl4.DefaultCellStyle = dataGridViewCellStyle3;
            this.cl4.HeaderText = "Low Limit";
            this.cl4.Name = "cl4";
            this.cl4.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.cl4.Width = 85;
            // 
            // cl5
            // 
            dataGridViewCellStyle4.Format = "N0";
            dataGridViewCellStyle4.NullValue = "0";
            this.cl5.DefaultCellStyle = dataGridViewCellStyle4;
            this.cl5.HeaderText = "High Limit";
            this.cl5.Name = "cl5";
            this.cl5.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.cl5.Width = 85;
            // 
            // cl6
            // 
            dataGridViewCellStyle5.Format = "N0";
            dataGridViewCellStyle5.NullValue = "0";
            this.cl6.DefaultCellStyle = dataGridViewCellStyle5;
            this.cl6.HeaderText = "Max Level";
            this.cl6.Name = "cl6";
            this.cl6.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.cl6.Width = 85;
            // 
            // columnDimmingProfile
            // 
            this.columnDimmingProfile.HeaderText = "Dimming Profile";
            this.columnDimmingProfile.Name = "columnDimmingProfile";
            this.columnDimmingProfile.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.columnDimmingProfile.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.columnDimmingProfile.Width = 85;
            // 
            // cl8
            // 
            this.cl8.HeaderText = "Output type";
            this.cl8.Name = "cl8";
            this.cl8.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.cl8.Width = 85;
            // 
            // clFailLevel
            // 
            this.clFailLevel.HeaderText = "Fail Level";
            this.clFailLevel.Name = "clFailLevel";
            // 
            // clPowerOnLevel
            // 
            this.clPowerOnLevel.HeaderText = "Power On Level";
            this.clPowerOnLevel.Name = "clPowerOnLevel";
            // 
            // cl0
            // 
            this.cl0.HeaderText = "ON";
            this.cl0.Name = "cl0";
            this.cl0.Width = 60;
            // 
            // panBasic
            // 
            this.panBasic.BackColor = System.Drawing.Color.Transparent;
            this.panBasic.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panBasic.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panBasic.Controls.Add(this.groupBoxDimmingMode);
            this.panBasic.Controls.Add(this.VoltageSelection);
            this.panBasic.Controls.Add(this.pnBroadcast);
            this.panBasic.Dock = System.Windows.Forms.DockStyle.Left;
            this.panBasic.Location = new System.Drawing.Point(3, 3);
            this.panBasic.Name = "panBasic";
            this.panBasic.Size = new System.Drawing.Size(250, 389);
            this.panBasic.TabIndex = 41;
            // 
            // groupBoxDimmingMode
            // 
            this.groupBoxDimmingMode.BackColor = System.Drawing.SystemColors.Control;
            this.groupBoxDimmingMode.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.groupBoxDimmingMode.Controls.Add(this.radioButtonTrailing);
            this.groupBoxDimmingMode.Controls.Add(this.radioButtonLeading);
            this.groupBoxDimmingMode.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxDimmingMode.Location = new System.Drawing.Point(0, 90);
            this.groupBoxDimmingMode.Name = "groupBoxDimmingMode";
            this.groupBoxDimmingMode.Size = new System.Drawing.Size(248, 45);
            this.groupBoxDimmingMode.TabIndex = 75;
            this.groupBoxDimmingMode.TabStop = false;
            this.groupBoxDimmingMode.Text = "Dimming Mode";
            // 
            // radioButtonTrailing
            // 
            this.radioButtonTrailing.AutoSize = true;
            this.radioButtonTrailing.Location = new System.Drawing.Point(142, 21);
            this.radioButtonTrailing.Name = "radioButtonTrailing";
            this.radioButtonTrailing.Size = new System.Drawing.Size(107, 22);
            this.radioButtonTrailing.TabIndex = 71;
            this.radioButtonTrailing.TabStop = true;
            this.radioButtonTrailing.Tag = "0";
            this.radioButtonTrailing.Text = "Trailing Edge";
            this.radioButtonTrailing.UseVisualStyleBackColor = true;
            // 
            // radioButtonLeading
            // 
            this.radioButtonLeading.AutoSize = true;
            this.radioButtonLeading.Location = new System.Drawing.Point(6, 21);
            this.radioButtonLeading.Name = "radioButtonLeading";
            this.radioButtonLeading.Size = new System.Drawing.Size(110, 22);
            this.radioButtonLeading.TabIndex = 71;
            this.radioButtonLeading.TabStop = true;
            this.radioButtonLeading.Tag = "1";
            this.radioButtonLeading.Text = "Leading Edge";
            this.radioButtonLeading.UseVisualStyleBackColor = true;
            this.radioButtonLeading.CheckedChanged += new System.EventHandler(this.radioButtonLeading_CheckedChanged);
            // 
            // VoltageSelection
            // 
            this.VoltageSelection.Controls.Add(this.radioButton110V);
            this.VoltageSelection.Controls.Add(this.radioButton220V);
            this.VoltageSelection.Dock = System.Windows.Forms.DockStyle.Top;
            this.VoltageSelection.Location = new System.Drawing.Point(0, 45);
            this.VoltageSelection.Name = "VoltageSelection";
            this.VoltageSelection.Size = new System.Drawing.Size(248, 45);
            this.VoltageSelection.TabIndex = 71;
            this.VoltageSelection.TabStop = false;
            this.VoltageSelection.Text = "Voltage Choose";
            // 
            // radioButton110V
            // 
            this.radioButton110V.AutoSize = true;
            this.radioButton110V.Location = new System.Drawing.Point(142, 21);
            this.radioButton110V.Name = "radioButton110V";
            this.radioButton110V.Size = new System.Drawing.Size(59, 22);
            this.radioButton110V.TabIndex = 77;
            this.radioButton110V.TabStop = true;
            this.radioButton110V.Tag = "2";
            this.radioButton110V.Text = "110V";
            this.radioButton110V.UseVisualStyleBackColor = true;
            this.radioButton110V.CheckedChanged += new System.EventHandler(this.chbBroadcast_CheckedChanged);
            // 
            // radioButton220V
            // 
            this.radioButton220V.AutoSize = true;
            this.radioButton220V.Location = new System.Drawing.Point(13, 21);
            this.radioButton220V.Name = "radioButton220V";
            this.radioButton220V.Size = new System.Drawing.Size(59, 22);
            this.radioButton220V.TabIndex = 76;
            this.radioButton220V.TabStop = true;
            this.radioButton220V.Tag = "1";
            this.radioButton220V.Text = "220V";
            this.radioButton220V.UseVisualStyleBackColor = true;
            this.radioButton220V.CheckedChanged += new System.EventHandler(this.chbBroadcast_CheckedChanged);
            // 
            // pnBroadcast
            // 
            this.pnBroadcast.Controls.Add(this.chbBroadcast);
            this.pnBroadcast.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnBroadcast.Location = new System.Drawing.Point(0, 0);
            this.pnBroadcast.Name = "pnBroadcast";
            this.pnBroadcast.Size = new System.Drawing.Size(248, 45);
            this.pnBroadcast.TabIndex = 76;
            // 
            // chbBroadcast
            // 
            this.chbBroadcast.Location = new System.Drawing.Point(15, 12);
            this.chbBroadcast.Margin = new System.Windows.Forms.Padding(50, 8, 0, 8);
            this.chbBroadcast.Name = "chbBroadcast";
            this.chbBroadcast.Size = new System.Drawing.Size(222, 25);
            this.chbBroadcast.TabIndex = 74;
            this.chbBroadcast.Tag = "0";
            this.chbBroadcast.Text = "Broadcast channels states every 5s ";
            this.chbBroadcast.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chbBroadcast.UseVisualStyleBackColor = false;
            this.chbBroadcast.CheckedChanged += new System.EventHandler(this.chbBroadcast_CheckedChanged);
            // 
            // tabZone
            // 
            this.tabZone.BackColor = System.Drawing.SystemColors.Control;
            this.tabZone.Controls.Add(this.groupBox2);
            this.tabZone.Controls.Add(this.groupBox3);
            this.tabZone.Controls.Add(this.groupBox1);
            this.tabZone.Controls.Add(this.panel7);
            this.tabZone.Location = new System.Drawing.Point(4, 27);
            this.tabZone.Name = "tabZone";
            this.tabZone.Padding = new System.Windows.Forms.Padding(3);
            this.tabZone.Size = new System.Drawing.Size(873, 395);
            this.tabZone.TabIndex = 5;
            this.tabZone.Text = "Zone Setting";
            // 
            // groupBox2
            // 
            this.groupBox2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.groupBox2.Controls.Add(this.dgvZoneChn1);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(351, 65);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(519, 327);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Channels Waitting Allocation";
            // 
            // dgvZoneChn1
            // 
            this.dgvZoneChn1.AllowDrop = true;
            this.dgvZoneChn1.AllowUserToAddRows = false;
            this.dgvZoneChn1.AllowUserToDeleteRows = false;
            this.dgvZoneChn1.AllowUserToOrderColumns = true;
            this.dgvZoneChn1.AllowUserToResizeRows = false;
            this.dgvZoneChn1.BackgroundColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = System.Drawing.Color.AliceBlue;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Calibri", 9F);
            dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvZoneChn1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
            this.dgvZoneChn1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvZoneChn1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewCheckBoxColumn1});
            this.dgvZoneChn1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvZoneChn1.EnableHeadersVisualStyles = false;
            this.dgvZoneChn1.Location = new System.Drawing.Point(3, 22);
            this.dgvZoneChn1.Name = "dgvZoneChn1";
            this.dgvZoneChn1.RowHeadersVisible = false;
            this.dgvZoneChn1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvZoneChn1.Size = new System.Drawing.Size(513, 302);
            this.dgvZoneChn1.TabIndex = 0;
            this.dgvZoneChn1.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvZoneChn1_CellValueChanged);
            this.dgvZoneChn1.CurrentCellDirtyStateChanged += new System.EventHandler(this.dgvZoneChn1_CurrentCellDirtyStateChanged);
            this.dgvZoneChn1.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.dgvZoneChn1_RowsAdded);
            this.dgvZoneChn1.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.dgvZoneChn1_RowsRemoved);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "Chn No.";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataGridViewTextBoxColumn1.Width = 80;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn2.HeaderText = "Name";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dataGridViewCheckBoxColumn1
            // 
            this.dataGridViewCheckBoxColumn1.HeaderText = "ON";
            this.dataGridViewCheckBoxColumn1.Name = "dataGridViewCheckBoxColumn1";
            this.dataGridViewCheckBoxColumn1.Width = 80;
            // 
            // groupBox3
            // 
            this.groupBox3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.groupBox3.Controls.Add(this.btn2);
            this.groupBox3.Controls.Add(this.button2);
            this.groupBox3.Controls.Add(this.btn1);
            this.groupBox3.Controls.Add(this.btnAddZone);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox3.Location = new System.Drawing.Point(225, 65);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(126, 327);
            this.groupBox3.TabIndex = 5;
            this.groupBox3.TabStop = false;
            // 
            // btn2
            // 
            this.btn2.Location = new System.Drawing.Point(7, 124);
            this.btn2.Name = "btn2";
            this.btn2.Size = new System.Drawing.Size(112, 23);
            this.btn2.TabIndex = 1;
            this.btn2.Text = "----->>>";
            this.btn2.UseVisualStyleBackColor = true;
            this.btn2.Click += new System.EventHandler(this.btn2_Click);
            // 
            // button2
            // 
            this.button2.AllowDrop = true;
            this.button2.Location = new System.Drawing.Point(7, 63);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(112, 25);
            this.button2.TabIndex = 3;
            this.button2.Text = "Delete Zone";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // btn1
            // 
            this.btn1.Location = new System.Drawing.Point(7, 95);
            this.btn1.Name = "btn1";
            this.btn1.Size = new System.Drawing.Size(112, 23);
            this.btn1.TabIndex = 0;
            this.btn1.Text = "<<<-----";
            this.btn1.UseVisualStyleBackColor = true;
            this.btn1.Click += new System.EventHandler(this.btn1_Click);
            // 
            // btnAddZone
            // 
            this.btnAddZone.AllowDrop = true;
            this.btnAddZone.Location = new System.Drawing.Point(7, 32);
            this.btnAddZone.Name = "btnAddZone";
            this.btnAddZone.Size = new System.Drawing.Size(112, 25);
            this.btnAddZone.TabIndex = 2;
            this.btnAddZone.Text = "Add Zone";
            this.btnAddZone.UseVisualStyleBackColor = true;
            this.btnAddZone.Click += new System.EventHandler(this.btnAddZone_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.groupBox1.Controls.Add(this.tvZone);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox1.Location = new System.Drawing.Point(3, 65);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(222, 327);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "All Zone";
            // 
            // tvZone
            // 
            this.tvZone.AllowDrop = true;
            this.tvZone.BackColor = System.Drawing.SystemColors.Window;
            this.tvZone.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvZone.DrawMode = System.Windows.Forms.TreeViewDrawMode.OwnerDrawText;
            this.tvZone.ImageIndex = 0;
            this.tvZone.ImageList = this.img2;
            this.tvZone.Indent = 28;
            this.tvZone.Location = new System.Drawing.Point(3, 22);
            this.tvZone.Name = "tvZone";
            this.tvZone.SelectedImageIndex = 0;
            this.tvZone.Size = new System.Drawing.Size(216, 302);
            this.tvZone.TabIndex = 0;
            this.tvZone.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tvZone_MouseDown);
            // 
            // img2
            // 
            this.img2.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("img2.ImageStream")));
            this.img2.TransparentColor = System.Drawing.Color.Transparent;
            this.img2.Images.SetKeyName(0, "1452157136_13.png");
            this.img2.Images.SetKeyName(1, "Blue House.ico");
            this.img2.Images.SetKeyName(2, "Light2.ICO");
            this.img2.Images.SetKeyName(3, "Relay on.bmp");
            this.img2.Images.SetKeyName(4, "NOSELECT.ico");
            this.img2.Images.SetKeyName(5, "SELECT.ico");
            this.img2.Images.SetKeyName(6, "1452498770_shutdown.png");
            this.img2.Images.SetKeyName(7, "1452498752_059_CircledOff.png");
            // 
            // panel7
            // 
            this.panel7.Controls.Add(this.lb22);
            this.panel7.Controls.Add(this.lb11);
            this.panel7.Controls.Add(this.lb2);
            this.panel7.Controls.Add(this.lb1);
            this.panel7.Controls.Add(this.txtZoneRemark);
            this.panel7.Controls.Add(this.lb3);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel7.Location = new System.Drawing.Point(3, 3);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(867, 62);
            this.panel7.TabIndex = 3;
            // 
            // lb22
            // 
            this.lb22.AutoSize = true;
            this.lb22.Location = new System.Drawing.Point(130, 33);
            this.lb22.Name = "lb22";
            this.lb22.Size = new System.Drawing.Size(15, 18);
            this.lb22.TabIndex = 7;
            this.lb22.Text = "0";
            // 
            // lb11
            // 
            this.lb11.AutoSize = true;
            this.lb11.Location = new System.Drawing.Point(257, 7);
            this.lb11.Name = "lb11";
            this.lb11.Size = new System.Drawing.Size(15, 18);
            this.lb11.TabIndex = 6;
            this.lb11.Text = "0";
            // 
            // lb2
            // 
            this.lb2.AutoSize = true;
            this.lb2.Location = new System.Drawing.Point(8, 33);
            this.lb2.Name = "lb2";
            this.lb2.Size = new System.Drawing.Size(103, 18);
            this.lb2.TabIndex = 5;
            this.lb2.Text = "All Zone Count:";
            // 
            // lb1
            // 
            this.lb1.AutoSize = true;
            this.lb1.Location = new System.Drawing.Point(8, 7);
            this.lb1.Name = "lb1";
            this.lb1.Size = new System.Drawing.Size(228, 18);
            this.lb1.TabIndex = 4;
            this.lb1.Text = "Waitting Allocation Channels Count:";
            // 
            // txtZoneRemark
            // 
            this.txtZoneRemark.Location = new System.Drawing.Point(485, 30);
            this.txtZoneRemark.Name = "txtZoneRemark";
            this.txtZoneRemark.Size = new System.Drawing.Size(222, 26);
            this.txtZoneRemark.TabIndex = 1;
            this.txtZoneRemark.TextChanged += new System.EventHandler(this.txtZoneRemark_TextChanged);
            // 
            // lb3
            // 
            this.lb3.AutoSize = true;
            this.lb3.Location = new System.Drawing.Point(395, 33);
            this.lb3.Name = "lb3";
            this.lb3.Size = new System.Drawing.Size(83, 18);
            this.lb3.TabIndex = 0;
            this.lb3.Text = "Zone Name:";
            // 
            // tabScene
            // 
            this.tabScene.BackColor = System.Drawing.SystemColors.Control;
            this.tabScene.Controls.Add(this.gbScene);
            this.tabScene.Controls.Add(this.groupBox6);
            this.tabScene.Location = new System.Drawing.Point(4, 27);
            this.tabScene.Name = "tabScene";
            this.tabScene.Padding = new System.Windows.Forms.Padding(3);
            this.tabScene.Size = new System.Drawing.Size(873, 395);
            this.tabScene.TabIndex = 2;
            this.tabScene.Text = "Scene Setting";
            // 
            // gbScene
            // 
            this.gbScene.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.gbScene.Controls.Add(this.dgScene);
            this.gbScene.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbScene.Location = new System.Drawing.Point(283, 3);
            this.gbScene.Name = "gbScene";
            this.gbScene.Size = new System.Drawing.Size(587, 389);
            this.gbScene.TabIndex = 19;
            this.gbScene.TabStop = false;
            this.gbScene.Text = "Channel Information";
            // 
            // dgScene
            // 
            this.dgScene.AllowDrop = true;
            this.dgScene.AllowUserToAddRows = false;
            this.dgScene.AllowUserToDeleteRows = false;
            this.dgScene.AllowUserToOrderColumns = true;
            this.dgScene.AllowUserToResizeRows = false;
            this.dgScene.BackgroundColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = System.Drawing.Color.AliceBlue;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Calibri", 9F);
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgScene.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle8;
            this.dgScene.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgScene.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn3,
            this.dataGridViewTextBoxColumn4,
            this.dataGridViewCheckBoxColumn2});
            this.dgScene.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgScene.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgScene.EnableHeadersVisualStyles = false;
            this.dgScene.Location = new System.Drawing.Point(3, 22);
            this.dgScene.Name = "dgScene";
            this.dgScene.RowHeadersVisible = false;
            this.dgScene.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgScene.Size = new System.Drawing.Size(581, 364);
            this.dgScene.TabIndex = 0;
            this.dgScene.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dgScene_CellBeginEdit);
            this.dgScene.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgScene_CellEndEdit);
            this.dgScene.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgScene_CellValueChanged);
            this.dgScene.CurrentCellDirtyStateChanged += new System.EventHandler(this.dgScene_CurrentCellDirtyStateChanged);
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.HeaderText = "Chn No.";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            this.dataGridViewTextBoxColumn3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataGridViewTextBoxColumn3.Width = 80;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn4.HeaderText = "Name";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly = true;
            this.dataGridViewTextBoxColumn4.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dataGridViewCheckBoxColumn2
            // 
            this.dataGridViewCheckBoxColumn2.HeaderText = "Intensity";
            this.dataGridViewCheckBoxColumn2.Name = "dataGridViewCheckBoxColumn2";
            this.dataGridViewCheckBoxColumn2.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewCheckBoxColumn2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataGridViewCheckBoxColumn2.Width = 120;
            // 
            // groupBox6
            // 
            this.groupBox6.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.groupBox6.Controls.Add(this.tvScene);
            this.groupBox6.Controls.Add(this.panel3);
            this.groupBox6.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox6.Location = new System.Drawing.Point(3, 3);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(280, 389);
            this.groupBox6.TabIndex = 18;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "All Zone";
            // 
            // tvScene
            // 
            this.tvScene.AllowDrop = true;
            this.tvScene.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvScene.HideSelection = false;
            this.tvScene.ImageIndex = 0;
            this.tvScene.ImageList = this.img2;
            this.tvScene.Indent = 28;
            this.tvScene.Location = new System.Drawing.Point(3, 22);
            this.tvScene.Name = "tvScene";
            this.tvScene.SelectedImageIndex = 0;
            this.tvScene.Size = new System.Drawing.Size(274, 219);
            this.tvScene.TabIndex = 0;
            this.tvScene.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvScene_AfterSelect);
            this.tvScene.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tvScene_NodeMouseClick);
            this.tvScene.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tvScene_MouseDown);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.cboDaliTime);
            this.panel3.Controls.Add(this.chbSyn);
            this.panel3.Controls.Add(this.cbRestoreList);
            this.panel3.Controls.Add(this.RestoreScene);
            this.panel3.Controls.Add(this.chbOutputS);
            this.panel3.Controls.Add(this.tbRunningTime);
            this.panel3.Controls.Add(this.RunningTime);
            this.panel3.Controls.Add(this.tbSceneName);
            this.panel3.Controls.Add(this.lbSceneName);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(3, 241);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(274, 145);
            this.panel3.TabIndex = 17;
            // 
            // cboDaliTime
            // 
            this.cboDaliTime.FormattingEnabled = true;
            this.cboDaliTime.Items.AddRange(new object[] {
            "0",
            "0.71",
            "1",
            "1.41",
            "2",
            "2.83",
            "4",
            "5.66",
            "8",
            "11.31",
            "16",
            "22.63",
            "32",
            "45.25",
            "64",
            "90.51"});
            this.cboDaliTime.Location = new System.Drawing.Point(111, 76);
            this.cboDaliTime.Name = "cboDaliTime";
            this.cboDaliTime.Size = new System.Drawing.Size(148, 26);
            this.cboDaliTime.TabIndex = 16;
            this.cboDaliTime.Visible = false;
            this.cboDaliTime.SelectedIndexChanged += new System.EventHandler(this.cboDaliTime_SelectedIndexChanged);
            // 
            // chbSyn
            // 
            this.chbSyn.AutoSize = true;
            this.chbSyn.Location = new System.Drawing.Point(111, 112);
            this.chbSyn.Name = "chbSyn";
            this.chbSyn.Size = new System.Drawing.Size(142, 22);
            this.chbSyn.TabIndex = 15;
            this.chbSyn.Text = "SYN Running Time";
            this.chbSyn.UseVisualStyleBackColor = true;
            this.chbSyn.CheckedChanged += new System.EventHandler(this.chbSyn_CheckedChanged);
            this.chbSyn.MouseLeave += new System.EventHandler(this.chbSyn_MouseLeave);
            // 
            // cbRestoreList
            // 
            this.cbRestoreList.FormattingEnabled = true;
            this.cbRestoreList.Location = new System.Drawing.Point(111, 44);
            this.cbRestoreList.Name = "cbRestoreList";
            this.cbRestoreList.Size = new System.Drawing.Size(148, 26);
            this.cbRestoreList.TabIndex = 14;
            this.cbRestoreList.SelectedIndexChanged += new System.EventHandler(this.cbRestoreList_SelectedIndexChanged);
            // 
            // RestoreScene
            // 
            this.RestoreScene.AutoSize = true;
            this.RestoreScene.Location = new System.Drawing.Point(3, 47);
            this.RestoreScene.Name = "RestoreScene";
            this.RestoreScene.Size = new System.Drawing.Size(109, 18);
            this.RestoreScene.TabIndex = 13;
            this.RestoreScene.Text = "Recovery Scene:";
            // 
            // chbOutputS
            // 
            this.chbOutputS.AutoSize = true;
            this.chbOutputS.Location = new System.Drawing.Point(6, 112);
            this.chbOutputS.Name = "chbOutputS";
            this.chbOutputS.Size = new System.Drawing.Size(122, 22);
            this.chbOutputS.TabIndex = 12;
            this.chbOutputS.Text = "Output On Site";
            this.chbOutputS.UseVisualStyleBackColor = true;
            this.chbOutputS.CheckedChanged += new System.EventHandler(this.chbOutputS_CheckedChanged);
            // 
            // tbRunningTime
            // 
            this.tbRunningTime.Location = new System.Drawing.Point(111, 76);
            this.tbRunningTime.Name = "tbRunningTime";
            this.tbRunningTime.Size = new System.Drawing.Size(148, 26);
            this.tbRunningTime.TabIndex = 9;
            this.tbRunningTime.Text = "0";
            this.tbRunningTime.TextChanged += new System.EventHandler(this.tbRunningTime_TextChanged);
            // 
            // RunningTime
            // 
            this.RunningTime.AutoSize = true;
            this.RunningTime.Location = new System.Drawing.Point(3, 79);
            this.RunningTime.Name = "RunningTime";
            this.RunningTime.Size = new System.Drawing.Size(97, 18);
            this.RunningTime.TabIndex = 8;
            this.RunningTime.Text = "Running Time:";
            // 
            // tbSceneName
            // 
            this.tbSceneName.Location = new System.Drawing.Point(111, 12);
            this.tbSceneName.Name = "tbSceneName";
            this.tbSceneName.Size = new System.Drawing.Size(148, 26);
            this.tbSceneName.TabIndex = 1;
            this.tbSceneName.TextChanged += new System.EventHandler(this.tbSceneName_TextChanged);
            // 
            // lbSceneName
            // 
            this.lbSceneName.AutoSize = true;
            this.lbSceneName.Location = new System.Drawing.Point(3, 15);
            this.lbSceneName.Name = "lbSceneName";
            this.lbSceneName.Size = new System.Drawing.Size(89, 18);
            this.lbSceneName.TabIndex = 0;
            this.lbSceneName.Text = "Scene Name:";
            // 
            // tp4
            // 
            this.tp4.BackColor = System.Drawing.SystemColors.Control;
            this.tp4.Controls.Add(this.groupBox8);
            this.tp4.Controls.Add(this.groupBox7);
            this.tp4.Location = new System.Drawing.Point(4, 27);
            this.tp4.Name = "tp4";
            this.tp4.Padding = new System.Windows.Forms.Padding(3);
            this.tp4.Size = new System.Drawing.Size(873, 395);
            this.tp4.TabIndex = 3;
            this.tp4.Text = "Sequence Setting";
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.DgvSeries);
            this.groupBox8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox8.Location = new System.Drawing.Point(283, 3);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(587, 389);
            this.groupBox8.TabIndex = 20;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "Current Sequence Information";
            // 
            // DgvSeries
            // 
            this.DgvSeries.AllowUserToAddRows = false;
            this.DgvSeries.AllowUserToDeleteRows = false;
            this.DgvSeries.AllowUserToResizeRows = false;
            this.DgvSeries.BackgroundColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.BackColor = System.Drawing.Color.AliceBlue;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("Calibri", 9F);
            dataGridViewCellStyle9.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.DgvSeries.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle9;
            this.DgvSeries.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DgvSeries.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn5,
            this.SelectScens,
            this.Column7});
            this.DgvSeries.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DgvSeries.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.DgvSeries.EnableHeadersVisualStyles = false;
            this.DgvSeries.Location = new System.Drawing.Point(3, 22);
            this.DgvSeries.Name = "DgvSeries";
            this.DgvSeries.RowHeadersVisible = false;
            this.DgvSeries.RowHeadersWidth = 10;
            this.DgvSeries.RowTemplate.Height = 23;
            this.DgvSeries.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.DgvSeries.Size = new System.Drawing.Size(581, 364);
            this.DgvSeries.TabIndex = 6;
            this.DgvSeries.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.DgvSeries_CellBeginEdit);
            this.DgvSeries.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.DgvSeries_CellEndEdit);
            this.DgvSeries.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.DgvSeries_CellValueChanged);
            this.DgvSeries.CurrentCellDirtyStateChanged += new System.EventHandler(this.DgvSeries_CurrentCellDirtyStateChanged);
            this.DgvSeries.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.DgvSeries_RowsRemoved);
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.HeaderText = "Step No.";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.ReadOnly = true;
            this.dataGridViewTextBoxColumn5.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTextBoxColumn5.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataGridViewTextBoxColumn5.Width = 80;
            // 
            // SelectScens
            // 
            this.SelectScens.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.SelectScens.HeaderText = "Scene Name";
            this.SelectScens.Name = "SelectScens";
            this.SelectScens.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // Column7
            // 
            this.Column7.HeaderText = "Duration";
            this.Column7.Name = "Column7";
            this.Column7.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Column7.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column7.Width = 160;
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.tvSeries);
            this.groupBox7.Controls.Add(this.panel9);
            this.groupBox7.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox7.Location = new System.Drawing.Point(3, 3);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(280, 389);
            this.groupBox7.TabIndex = 19;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "All Zone";
            // 
            // tvSeries
            // 
            this.tvSeries.BackColor = System.Drawing.SystemColors.Window;
            this.tvSeries.ContextMenuStrip = this.cmSequence;
            this.tvSeries.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvSeries.DrawMode = System.Windows.Forms.TreeViewDrawMode.OwnerDrawText;
            this.tvSeries.ImageIndex = 0;
            this.tvSeries.ImageList = this.img2;
            this.tvSeries.Indent = 28;
            this.tvSeries.Location = new System.Drawing.Point(3, 22);
            this.tvSeries.Name = "tvSeries";
            this.tvSeries.SelectedImageIndex = 0;
            this.tvSeries.Size = new System.Drawing.Size(274, 225);
            this.tvSeries.StateImageList = this.imgs;
            this.tvSeries.TabIndex = 8;
            this.tvSeries.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tvSeries_MouseDown);
            // 
            // cmSequence
            // 
            this.cmSequence.Font = new System.Drawing.Font("Arial", 8.25F);
            this.cmSequence.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.cmSequence.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmDeleteSce,
            this.toolStripSeparator8,
            this.tsmCopySce,
            this.tsmPasteSce,
            this.toolStripMenuItem2,
            this.testToolStripMenuItem});
            this.cmSequence.Name = "cMS1";
            this.cmSequence.Size = new System.Drawing.Size(235, 104);
            // 
            // tsmDeleteSce
            // 
            this.tsmDeleteSce.Name = "tsmDeleteSce";
            this.tsmDeleteSce.ShortcutKeys = System.Windows.Forms.Keys.Delete;
            this.tsmDeleteSce.Size = new System.Drawing.Size(234, 22);
            this.tsmDeleteSce.Text = "Delete Sequence";
            this.tsmDeleteSce.Visible = false;
            this.tsmDeleteSce.Click += new System.EventHandler(this.tsmDeleteSce_Click);
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new System.Drawing.Size(231, 6);
            // 
            // tsmCopySce
            // 
            this.tsmCopySce.Name = "tsmCopySce";
            this.tsmCopySce.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.tsmCopySce.Size = new System.Drawing.Size(234, 22);
            this.tsmCopySce.Tag = "5";
            this.tsmCopySce.Text = "Copy Sequence";
            this.tsmCopySce.Click += new System.EventHandler(this.tsmCopySce_Click);
            // 
            // tsmPasteSce
            // 
            this.tsmPasteSce.Name = "tsmPasteSce";
            this.tsmPasteSce.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
            this.tsmPasteSce.Size = new System.Drawing.Size(234, 22);
            this.tsmPasteSce.Text = "Paste Sequence";
            this.tsmPasteSce.Click += new System.EventHandler(this.tsmPasteSce_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(231, 6);
            // 
            // testToolStripMenuItem
            // 
            this.testToolStripMenuItem.Name = "testToolStripMenuItem";
            this.testToolStripMenuItem.Size = new System.Drawing.Size(234, 22);
            this.testToolStripMenuItem.Text = "Test";
            this.testToolStripMenuItem.Click += new System.EventHandler(this.testToolStripMenuItem_Click);
            // 
            // imgs
            // 
            this.imgs.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgs.ImageStream")));
            this.imgs.TransparentColor = System.Drawing.Color.Transparent;
            this.imgs.Images.SetKeyName(0, "Light2.ICO");
            this.imgs.Images.SetKeyName(1, "Relay on.bmp");
            this.imgs.Images.SetKeyName(2, "NOSELECT.ico");
            this.imgs.Images.SetKeyName(3, "SELECT.ico");
            // 
            // panel9
            // 
            this.panel9.Controls.Add(this.cbSeqMode);
            this.panel9.Controls.Add(this.lbSeqMode);
            this.panel9.Controls.Add(this.cbSeriesRepeat);
            this.panel9.Controls.Add(this.label18);
            this.panel9.Controls.Add(this.cbstep);
            this.panel9.Controls.Add(this.label17);
            this.panel9.Controls.Add(this.tbRemarkSeries);
            this.panel9.Controls.Add(this.label14);
            this.panel9.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel9.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panel9.Location = new System.Drawing.Point(3, 247);
            this.panel9.Name = "panel9";
            this.panel9.Size = new System.Drawing.Size(274, 139);
            this.panel9.TabIndex = 7;
            // 
            // cbSeqMode
            // 
            this.cbSeqMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSeqMode.Font = new System.Drawing.Font("Calibri", 9F);
            this.cbSeqMode.FormattingEnabled = true;
            this.cbSeqMode.Location = new System.Drawing.Point(111, 108);
            this.cbSeqMode.Name = "cbSeqMode";
            this.cbSeqMode.Size = new System.Drawing.Size(148, 26);
            this.cbSeqMode.TabIndex = 66;
            this.cbSeqMode.SelectedIndexChanged += new System.EventHandler(this.cbSeqMode_SelectedIndexChanged);
            // 
            // lbSeqMode
            // 
            this.lbSeqMode.AutoSize = true;
            this.lbSeqMode.Font = new System.Drawing.Font("Calibri", 9F);
            this.lbSeqMode.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lbSeqMode.Location = new System.Drawing.Point(6, 111);
            this.lbSeqMode.Name = "lbSeqMode";
            this.lbSeqMode.Size = new System.Drawing.Size(48, 18);
            this.lbSeqMode.TabIndex = 65;
            this.lbSeqMode.Text = "Mode:";
            // 
            // cbSeriesRepeat
            // 
            this.cbSeriesRepeat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSeriesRepeat.Font = new System.Drawing.Font("Calibri", 9F);
            this.cbSeriesRepeat.FormattingEnabled = true;
            this.cbSeriesRepeat.Location = new System.Drawing.Point(111, 76);
            this.cbSeriesRepeat.Name = "cbSeriesRepeat";
            this.cbSeriesRepeat.Size = new System.Drawing.Size(148, 26);
            this.cbSeriesRepeat.TabIndex = 63;
            this.cbSeriesRepeat.SelectedIndexChanged += new System.EventHandler(this.cbSeriesRepeat_SelectedIndexChanged);
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Font = new System.Drawing.Font("Calibri", 9F);
            this.label18.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label18.Location = new System.Drawing.Point(3, 79);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(76, 18);
            this.label18.TabIndex = 62;
            this.label18.Text = "Run Times:";
            // 
            // cbstep
            // 
            this.cbstep.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbstep.Font = new System.Drawing.Font("Calibri", 9F);
            this.cbstep.FormattingEnabled = true;
            this.cbstep.Location = new System.Drawing.Point(111, 44);
            this.cbstep.Name = "cbstep";
            this.cbstep.Size = new System.Drawing.Size(148, 26);
            this.cbstep.TabIndex = 61;
            this.cbstep.SelectedIndexChanged += new System.EventHandler(this.cbstep_SelectedIndexChanged);
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("Calibri", 9F);
            this.label17.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label17.Location = new System.Drawing.Point(3, 47);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(114, 18);
            this.label17.TabIndex = 60;
            this.label17.Text = "How Many Steps:";
            // 
            // tbRemarkSeries
            // 
            this.tbRemarkSeries.Font = new System.Drawing.Font("Calibri", 9F);
            this.tbRemarkSeries.Location = new System.Drawing.Point(111, 12);
            this.tbRemarkSeries.MaxLength = 20;
            this.tbRemarkSeries.Name = "tbRemarkSeries";
            this.tbRemarkSeries.Size = new System.Drawing.Size(148, 26);
            this.tbRemarkSeries.TabIndex = 52;
            this.tbRemarkSeries.TextChanged += new System.EventHandler(this.tbRemarkSeries_TextChanged);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Calibri", 9F);
            this.label14.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label14.Location = new System.Drawing.Point(3, 15);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(113, 18);
            this.label14.TabIndex = 51;
            this.label14.Text = "Sequence Name:";
            // 
            // tabDali
            // 
            this.tabDali.Controls.Add(this.gbDaliManagement);
            this.tabDali.Location = new System.Drawing.Point(4, 27);
            this.tabDali.Name = "tabDali";
            this.tabDali.Padding = new System.Windows.Forms.Padding(3);
            this.tabDali.Size = new System.Drawing.Size(873, 395);
            this.tabDali.TabIndex = 6;
            this.tabDali.Text = "DALI Ballasts Management";
            this.tabDali.UseVisualStyleBackColor = true;
            // 
            // gbDaliManagement
            // 
            this.gbDaliManagement.Controls.Add(this.cboArea);
            this.gbDaliManagement.Controls.Add(this.lbArea);
            this.gbDaliManagement.Controls.Add(this.btnFlick);
            this.gbDaliManagement.Controls.Add(this.tbNewAddress);
            this.gbDaliManagement.Controls.Add(this.lbNewAddress);
            this.gbDaliManagement.Controls.Add(this.tbOldAddress);
            this.gbDaliManagement.Controls.Add(this.lbOldAddress);
            this.gbDaliManagement.Controls.Add(this.tbDelAddress);
            this.gbDaliManagement.Controls.Add(this.lbDelAddress);
            this.gbDaliManagement.Controls.Add(this.tbDuplicate);
            this.gbDaliManagement.Controls.Add(this.lbDuplcate);
            this.gbDaliManagement.Controls.Add(this.lbAdd);
            this.gbDaliManagement.Controls.Add(this.btnClear);
            this.gbDaliManagement.Controls.Add(this.btnView);
            this.gbDaliManagement.Controls.Add(this.lbInitial);
            this.gbDaliManagement.Controls.Add(this.btnNewAddress);
            this.gbDaliManagement.Controls.Add(this.btnDelAddress);
            this.gbDaliManagement.Controls.Add(this.btnDuplicate);
            this.gbDaliManagement.Controls.Add(this.btnAdd);
            this.gbDaliManagement.Controls.Add(this.btnInitial);
            this.gbDaliManagement.Controls.Add(this.lvAddresses);
            this.gbDaliManagement.Location = new System.Drawing.Point(6, 6);
            this.gbDaliManagement.Name = "gbDaliManagement";
            this.gbDaliManagement.Size = new System.Drawing.Size(850, 390);
            this.gbDaliManagement.TabIndex = 0;
            this.gbDaliManagement.TabStop = false;
            this.gbDaliManagement.Text = "Ballast Addresses List";
            // 
            // cboArea
            // 
            this.cboArea.FormattingEnabled = true;
            this.cboArea.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8"});
            this.cboArea.Location = new System.Drawing.Point(443, 37);
            this.cboArea.Name = "cboArea";
            this.cboArea.Size = new System.Drawing.Size(72, 26);
            this.cboArea.TabIndex = 22;
            // 
            // lbArea
            // 
            this.lbArea.AutoSize = true;
            this.lbArea.Location = new System.Drawing.Point(326, 40);
            this.lbArea.Name = "lbArea";
            this.lbArea.Size = new System.Drawing.Size(57, 18);
            this.lbArea.TabIndex = 20;
            this.lbArea.Text = "Area ID:";
            // 
            // btnFlick
            // 
            this.btnFlick.Location = new System.Drawing.Point(710, 31);
            this.btnFlick.Name = "btnFlick";
            this.btnFlick.Size = new System.Drawing.Size(120, 23);
            this.btnFlick.TabIndex = 19;
            this.btnFlick.Tag = "0";
            this.btnFlick.Text = "Flick";
            this.btnFlick.UseVisualStyleBackColor = true;
            this.btnFlick.Click += new System.EventHandler(this.btnFlick_Click);
            // 
            // tbNewAddress
            // 
            this.tbNewAddress.Location = new System.Drawing.Point(625, 311);
            this.tbNewAddress.Name = "tbNewAddress";
            this.tbNewAddress.Size = new System.Drawing.Size(72, 26);
            this.tbNewAddress.TabIndex = 17;
            // 
            // lbNewAddress
            // 
            this.lbNewAddress.AutoSize = true;
            this.lbNewAddress.Location = new System.Drawing.Point(532, 314);
            this.lbNewAddress.Name = "lbNewAddress";
            this.lbNewAddress.Size = new System.Drawing.Size(94, 18);
            this.lbNewAddress.TabIndex = 16;
            this.lbNewAddress.Text = "New Address:";
            // 
            // tbOldAddress
            // 
            this.tbOldAddress.Location = new System.Drawing.Point(443, 310);
            this.tbOldAddress.Name = "tbOldAddress";
            this.tbOldAddress.Size = new System.Drawing.Size(72, 26);
            this.tbOldAddress.TabIndex = 15;
            // 
            // lbOldAddress
            // 
            this.lbOldAddress.AutoSize = true;
            this.lbOldAddress.Location = new System.Drawing.Point(326, 314);
            this.lbOldAddress.Name = "lbOldAddress";
            this.lbOldAddress.Size = new System.Drawing.Size(87, 18);
            this.lbOldAddress.TabIndex = 14;
            this.lbOldAddress.Text = "Old Address:";
            // 
            // tbDelAddress
            // 
            this.tbDelAddress.Location = new System.Drawing.Point(443, 260);
            this.tbDelAddress.Name = "tbDelAddress";
            this.tbDelAddress.Size = new System.Drawing.Size(72, 26);
            this.tbDelAddress.TabIndex = 13;
            // 
            // lbDelAddress
            // 
            this.lbDelAddress.AutoSize = true;
            this.lbDelAddress.Location = new System.Drawing.Point(326, 264);
            this.lbDelAddress.Name = "lbDelAddress";
            this.lbDelAddress.Size = new System.Drawing.Size(62, 18);
            this.lbDelAddress.TabIndex = 12;
            this.lbDelAddress.Text = "Address:";
            // 
            // tbDuplicate
            // 
            this.tbDuplicate.Location = new System.Drawing.Point(443, 210);
            this.tbDuplicate.Name = "tbDuplicate";
            this.tbDuplicate.Size = new System.Drawing.Size(72, 26);
            this.tbDuplicate.TabIndex = 11;
            // 
            // lbDuplcate
            // 
            this.lbDuplcate.AutoSize = true;
            this.lbDuplcate.Location = new System.Drawing.Point(326, 214);
            this.lbDuplcate.Name = "lbDuplcate";
            this.lbDuplcate.Size = new System.Drawing.Size(124, 18);
            this.lbDuplcate.TabIndex = 10;
            this.lbDuplcate.Text = "Duplicate Address:";
            // 
            // lbAdd
            // 
            this.lbAdd.AutoSize = true;
            this.lbAdd.Location = new System.Drawing.Point(326, 164);
            this.lbAdd.Name = "lbAdd";
            this.lbAdd.Size = new System.Drawing.Size(272, 18);
            this.lbAdd.TabIndex = 9;
            this.lbAdd.Text = "b.Add addresses for new installed ballasts.";
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(181, 343);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(120, 23);
            this.btnClear.TabIndex = 8;
            this.btnClear.Text = "Clear List";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnView
            // 
            this.btnView.Location = new System.Drawing.Point(27, 343);
            this.btnView.Name = "btnView";
            this.btnView.Size = new System.Drawing.Size(120, 23);
            this.btnView.TabIndex = 7;
            this.btnView.Text = "View";
            this.btnView.UseVisualStyleBackColor = true;
            this.btnView.Click += new System.EventHandler(this.btnView_Click);
            // 
            // lbInitial
            // 
            this.lbInitial.Location = new System.Drawing.Point(326, 71);
            this.lbInitial.Name = "lbInitial";
            this.lbInitial.Size = new System.Drawing.Size(371, 75);
            this.lbInitial.TabIndex = 6;
            this.lbInitial.Text = resources.GetString("lbInitial.Text");
            this.lbInitial.Click += new System.EventHandler(this.label1_Click);
            // 
            // btnNewAddress
            // 
            this.btnNewAddress.Location = new System.Drawing.Point(710, 310);
            this.btnNewAddress.Name = "btnNewAddress";
            this.btnNewAddress.Size = new System.Drawing.Size(120, 23);
            this.btnNewAddress.TabIndex = 5;
            this.btnNewAddress.Text = "Modify";
            this.btnNewAddress.UseVisualStyleBackColor = true;
            this.btnNewAddress.Click += new System.EventHandler(this.btnNewAddress_Click);
            // 
            // btnDelAddress
            // 
            this.btnDelAddress.Location = new System.Drawing.Point(710, 260);
            this.btnDelAddress.Name = "btnDelAddress";
            this.btnDelAddress.Size = new System.Drawing.Size(120, 23);
            this.btnDelAddress.TabIndex = 4;
            this.btnDelAddress.Text = "Delete";
            this.btnDelAddress.UseVisualStyleBackColor = true;
            this.btnDelAddress.Click += new System.EventHandler(this.btnDelAddress_Click);
            // 
            // btnDuplicate
            // 
            this.btnDuplicate.Location = new System.Drawing.Point(710, 210);
            this.btnDuplicate.Name = "btnDuplicate";
            this.btnDuplicate.Size = new System.Drawing.Size(120, 23);
            this.btnDuplicate.TabIndex = 3;
            this.btnDuplicate.Text = "Duplicate Address Rebuild";
            this.btnDuplicate.UseVisualStyleBackColor = true;
            this.btnDuplicate.Click += new System.EventHandler(this.btnDuplicate_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(710, 160);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(120, 23);
            this.btnAdd.TabIndex = 2;
            this.btnAdd.Text = "New Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnInitial
            // 
            this.btnInitial.Location = new System.Drawing.Point(710, 101);
            this.btnInitial.Name = "btnInitial";
            this.btnInitial.Size = new System.Drawing.Size(120, 23);
            this.btnInitial.TabIndex = 1;
            this.btnInitial.Text = "Initialization";
            this.btnInitial.UseVisualStyleBackColor = true;
            this.btnInitial.Click += new System.EventHandler(this.btnInitial_Click);
            // 
            // lvAddresses
            // 
            this.lvAddresses.ContextMenuStrip = this.cmDali;
            listViewGroup1.Header = "Existed Addresses";
            listViewGroup1.Name = "listViewGroup1";
            listViewGroup2.Header = "New Added";
            listViewGroup2.Name = "listViewGroup2";
            listViewGroup3.Header = "Duplcate Addresses";
            listViewGroup3.Name = "listViewGroup3";
            this.lvAddresses.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            listViewGroup1,
            listViewGroup2,
            listViewGroup3});
            this.lvAddresses.Location = new System.Drawing.Point(6, 18);
            this.lvAddresses.Name = "lvAddresses";
            this.lvAddresses.Size = new System.Drawing.Size(306, 319);
            this.lvAddresses.TabIndex = 0;
            this.lvAddresses.UseCompatibleStateImageBehavior = false;
            this.lvAddresses.View = System.Windows.Forms.View.Tile;
            // 
            // cmDali
            // 
            this.cmDali.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.cmDali.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbFlick,
            this.tsbFlickAll,
            this.toolStripSeparator10,
            this.tsbSelectAll});
            this.cmDali.Name = "cmDali";
            this.cmDali.Size = new System.Drawing.Size(147, 82);
            // 
            // tsbFlick
            // 
            this.tsbFlick.Name = "tsbFlick";
            this.tsbFlick.Size = new System.Drawing.Size(146, 24);
            this.tsbFlick.Text = "Flick";
            this.tsbFlick.Click += new System.EventHandler(this.tsbFlick_Click);
            // 
            // tsbFlickAll
            // 
            this.tsbFlickAll.Name = "tsbFlickAll";
            this.tsbFlickAll.Size = new System.Drawing.Size(146, 24);
            this.tsbFlickAll.Text = "Flick All";
            this.tsbFlickAll.Click += new System.EventHandler(this.tsbFlickAll_Click);
            // 
            // toolStripSeparator10
            // 
            this.toolStripSeparator10.Name = "toolStripSeparator10";
            this.toolStripSeparator10.Size = new System.Drawing.Size(143, 6);
            // 
            // tsbSelectAll
            // 
            this.tsbSelectAll.Name = "tsbSelectAll";
            this.tsbSelectAll.Size = new System.Drawing.Size(146, 24);
            this.tsbSelectAll.Text = "Select All";
            this.tsbSelectAll.Click += new System.EventHandler(this.tsbSelectAll_Click);
            // 
            // tabDaliStatus
            // 
            this.tabDaliStatus.Controls.Add(this.lvDaliStatus);
            this.tabDaliStatus.Location = new System.Drawing.Point(4, 27);
            this.tabDaliStatus.Name = "tabDaliStatus";
            this.tabDaliStatus.Padding = new System.Windows.Forms.Padding(3);
            this.tabDaliStatus.Size = new System.Drawing.Size(873, 395);
            this.tabDaliStatus.TabIndex = 7;
            this.tabDaliStatus.Text = "Ballast Status";
            this.tabDaliStatus.UseVisualStyleBackColor = true;
            // 
            // lvDaliStatus
            // 
            this.lvDaliStatus.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chAddress,
            this.chStatus,
            this.chLoad,
            this.chOnOff,
            this.chDimRange,
            this.chDim,
            this.chReset});
            this.lvDaliStatus.Dock = System.Windows.Forms.DockStyle.Left;
            this.lvDaliStatus.FullRowSelect = true;
            this.lvDaliStatus.HideSelection = false;
            this.lvDaliStatus.Location = new System.Drawing.Point(3, 3);
            this.lvDaliStatus.Name = "lvDaliStatus";
            this.lvDaliStatus.Size = new System.Drawing.Size(709, 389);
            this.lvDaliStatus.TabIndex = 0;
            this.lvDaliStatus.UseCompatibleStateImageBehavior = false;
            this.lvDaliStatus.View = System.Windows.Forms.View.Details;
            // 
            // chAddress
            // 
            this.chAddress.Text = "Dali Address";
            this.chAddress.Width = 85;
            // 
            // chStatus
            // 
            this.chStatus.Text = "Ballast Status";
            this.chStatus.Width = 100;
            // 
            // chLoad
            // 
            this.chLoad.Text = "Load Status";
            this.chLoad.Width = 100;
            // 
            // chOnOff
            // 
            this.chOnOff.Text = "ON/OFF";
            this.chOnOff.Width = 100;
            // 
            // chDimRange
            // 
            this.chDimRange.Text = "In Dimming Range";
            this.chDimRange.Width = 120;
            // 
            // chDim
            // 
            this.chDim.Text = "Dimming";
            this.chDim.Width = 100;
            // 
            // chReset
            // 
            this.chReset.Text = "Reset";
            this.chReset.Width = 100;
            // 
            // cmChnScene
            // 
            this.cmChnScene.Font = new System.Drawing.Font("Arial", 8.25F);
            this.cmChnScene.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.cmChnScene.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tmSame,
            this.tmAscend,
            this.tmdescend,
            this.toolStripSeparator2,
            this.tmiSce,
            this.tmiDefault,
            this.toolStripMenuItem1,
            this.outputSceneToolStripMenuItem});
            this.cmChnScene.Name = "cm4";
            this.cmChnScene.Size = new System.Drawing.Size(220, 172);
            // 
            // tmSame
            // 
            this.tmSame.Name = "tmSame";
            this.tmSame.Size = new System.Drawing.Size(219, 26);
            this.tmSame.Tag = "0";
            this.tmSame.Text = "Sync Column";
            this.tmSame.Visible = false;
            // 
            // tmAscend
            // 
            this.tmAscend.Name = "tmAscend";
            this.tmAscend.Size = new System.Drawing.Size(219, 26);
            this.tmAscend.Tag = "1";
            this.tmAscend.Text = "Ascending from";
            // 
            // tmdescend
            // 
            this.tmdescend.Name = "tmdescend";
            this.tmdescend.Size = new System.Drawing.Size(219, 26);
            this.tmdescend.Tag = "2";
            this.tmdescend.Text = "Descending from";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(216, 6);
            // 
            // tmiSce
            // 
            this.tmiSce.CheckOnClick = true;
            this.tmiSce.Image = global::HDL_Buspro_Setup_Tool.Properties.Resources.Save;
            this.tmiSce.Name = "tmiSce";
            this.tmiSce.Size = new System.Drawing.Size(219, 26);
            this.tmiSce.Tag = "0";
            this.tmiSce.Text = "Run when power on";
            // 
            // tmiDefault
            // 
            this.tmiDefault.Name = "tmiDefault";
            this.tmiDefault.Size = new System.Drawing.Size(219, 26);
            this.tmiDefault.Tag = "255";
            this.tmiDefault.Text = "Scene when power off";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(216, 6);
            // 
            // outputSceneToolStripMenuItem
            // 
            this.outputSceneToolStripMenuItem.Name = "outputSceneToolStripMenuItem";
            this.outputSceneToolStripMenuItem.Size = new System.Drawing.Size(219, 26);
            this.outputSceneToolStripMenuItem.Text = "Test";
            // 
            // toolStrip1
            // 
            this.toolStrip1.Font = new System.Drawing.Font("Arial", 8.25F);
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton5,
            this.toolStripButton6,
            this.toolStripButton7,
            this.toolStripSeparator4,
            this.toolStripButton9,
            this.toolStripButton10,
            this.toolStripSeparator5,
            this.tsbDown,
            this.tsbUpload,
            this.toolStripButton11,
            this.toolStripSeparator1});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(881, 27);
            this.toolStrip1.TabIndex = 39;
            this.toolStrip1.Text = "toolStrip1";
            this.toolStrip1.Visible = false;
            // 
            // toolStripButton5
            // 
            this.toolStripButton5.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton5.Image = global::HDL_Buspro_Setup_Tool.Properties.Resources.Save;
            this.toolStripButton5.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton5.Name = "toolStripButton5";
            this.toolStripButton5.Size = new System.Drawing.Size(24, 24);
            this.toolStripButton5.Text = "New";
            this.toolStripButton5.ToolTipText = "Load Default";
            this.toolStripButton5.Click += new System.EventHandler(this.toolStripButton5_Click);
            // 
            // toolStripButton6
            // 
            this.toolStripButton6.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton6.Image = global::HDL_Buspro_Setup_Tool.Properties.Resources.Refresh;
            this.toolStripButton6.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton6.Name = "toolStripButton6";
            this.toolStripButton6.Size = new System.Drawing.Size(24, 24);
            this.toolStripButton6.Text = "Save";
            this.toolStripButton6.ToolTipText = "Save ";
            this.toolStripButton6.Click += new System.EventHandler(this.toolStripButton6_Click);
            // 
            // toolStripButton7
            // 
            this.toolStripButton7.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton7.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton7.Name = "toolStripButton7";
            this.toolStripButton7.Size = new System.Drawing.Size(23, 24);
            this.toolStripButton7.Text = "&Print";
            this.toolStripButton7.Visible = false;
            this.toolStripButton7.Click += new System.EventHandler(this.toolStripButton7_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 27);
            // 
            // toolStripButton9
            // 
            this.toolStripButton9.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton9.Image = global::HDL_Buspro_Setup_Tool.Properties.Resources.Save;
            this.toolStripButton9.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton9.Name = "toolStripButton9";
            this.toolStripButton9.Size = new System.Drawing.Size(24, 24);
            this.toolStripButton9.Text = "Copy";
            this.toolStripButton9.Visible = false;
            // 
            // toolStripButton10
            // 
            this.toolStripButton10.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton10.Image = global::HDL_Buspro_Setup_Tool.Properties.Resources.Refresh;
            this.toolStripButton10.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton10.Name = "toolStripButton10";
            this.toolStripButton10.Size = new System.Drawing.Size(24, 24);
            this.toolStripButton10.Text = "Paste";
            this.toolStripButton10.Visible = false;
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 27);
            // 
            // tsbDown
            // 
            this.tsbDown.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbDown.Image = global::HDL_Buspro_Setup_Tool.Properties.Resources.Refresh;
            this.tsbDown.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbDown.Name = "tsbDown";
            this.tsbDown.Size = new System.Drawing.Size(24, 24);
            this.tsbDown.Tag = "0";
            this.tsbDown.Text = "Read";
            this.tsbDown.ToolTipText = "Read";
            this.tsbDown.Click += new System.EventHandler(this.tsbDown_Click);
            // 
            // tsbUpload
            // 
            this.tsbUpload.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbUpload.Image = global::HDL_Buspro_Setup_Tool.Properties.Resources.Refresh;
            this.tsbUpload.Name = "tsbUpload";
            this.tsbUpload.Size = new System.Drawing.Size(24, 24);
            this.tsbUpload.Tag = "1";
            this.tsbUpload.Text = "Upload";
            this.tsbUpload.ToolTipText = "Upload";
            this.tsbUpload.Click += new System.EventHandler(this.tsbDown_Click);
            // 
            // toolStripButton11
            // 
            this.toolStripButton11.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton11.Image = global::HDL_Buspro_Setup_Tool.Properties.Resources.OK;
            this.toolStripButton11.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton11.Name = "toolStripButton11";
            this.toolStripButton11.Size = new System.Drawing.Size(24, 24);
            this.toolStripButton11.Text = "Help";
            this.toolStripButton11.Click += new System.EventHandler(this.toolStripButton11_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Font = new System.Drawing.Font("Calibri", 9F);
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsl1,
            this.tsl2,
            this.tsl3,
            this.tsbar,
            this.tsbl4,
            this.tsbHint,
            this.toolStripStatusLabel1,
            this.HintScene});
            this.statusStrip1.Location = new System.Drawing.Point(0, 494);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(881, 28);
            this.statusStrip1.TabIndex = 40;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // tsl1
            // 
            this.tsl1.AutoSize = false;
            this.tsl1.Name = "tsl1";
            this.tsl1.Size = new System.Drawing.Size(150, 23);
            this.tsl1.Text = "Current Device:";
            // 
            // tsl2
            // 
            this.tsl2.Name = "tsl2";
            this.tsl2.Size = new System.Drawing.Size(15, 23);
            this.tsl2.Text = "|";
            // 
            // tsl3
            // 
            this.tsl3.AutoSize = false;
            this.tsl3.Name = "tsl3";
            this.tsl3.Size = new System.Drawing.Size(160, 23);
            // 
            // tsbar
            // 
            this.tsbar.Name = "tsbar";
            this.tsbar.Size = new System.Drawing.Size(100, 22);
            this.tsbar.Visible = false;
            // 
            // tsbl4
            // 
            this.tsbl4.Name = "tsbl4";
            this.tsbl4.Size = new System.Drawing.Size(0, 23);
            // 
            // tsbHint
            // 
            this.tsbHint.Name = "tsbHint";
            this.tsbHint.Size = new System.Drawing.Size(0, 23);
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(15, 23);
            this.toolStripStatusLabel1.Text = "|";
            // 
            // HintScene
            // 
            this.HintScene.AutoSize = false;
            this.HintScene.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HintScene.ForeColor = System.Drawing.Color.Red;
            this.HintScene.Name = "HintScene";
            this.HintScene.Size = new System.Drawing.Size(120, 23);
            this.HintScene.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // panel16
            // 
            this.panel16.Controls.Add(this.btnRef4);
            this.panel16.Controls.Add(this.btnSaveAndClose4);
            this.panel16.Controls.Add(this.btnSave4);
            this.panel16.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel16.Location = new System.Drawing.Point(0, 454);
            this.panel16.Name = "panel16";
            this.panel16.Size = new System.Drawing.Size(881, 40);
            this.panel16.TabIndex = 42;
            // 
            // btnRef4
            // 
            this.btnRef4.Image = global::HDL_Buspro_Setup_Tool.Properties.Resources.Refresh;
            this.btnRef4.Location = new System.Drawing.Point(210, 6);
            this.btnRef4.Name = "btnRef4";
            this.btnRef4.Size = new System.Drawing.Size(140, 25);
            this.btnRef4.TabIndex = 26;
            this.btnRef4.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnRef4.UseVisualStyleBackColor = true;
            this.btnRef4.Click += new System.EventHandler(this.btnRef4_Click);
            // 
            // btnSaveAndClose4
            // 
            this.btnSaveAndClose4.Image = global::HDL_Buspro_Setup_Tool.Properties.Resources.Save;
            this.btnSaveAndClose4.Location = new System.Drawing.Point(581, 6);
            this.btnSaveAndClose4.Name = "btnSaveAndClose4";
            this.btnSaveAndClose4.Size = new System.Drawing.Size(140, 25);
            this.btnSaveAndClose4.TabIndex = 25;
            this.btnSaveAndClose4.Text = "Save && Close";
            this.btnSaveAndClose4.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSaveAndClose4.UseVisualStyleBackColor = true;
            this.btnSaveAndClose4.Click += new System.EventHandler(this.btnSaveAndClose4_Click_1);
            // 
            // btnSave4
            // 
            this.btnSave4.Image = global::HDL_Buspro_Setup_Tool.Properties.Resources.Save;
            this.btnSave4.Location = new System.Drawing.Point(399, 6);
            this.btnSave4.Name = "btnSave4";
            this.btnSave4.Size = new System.Drawing.Size(140, 25);
            this.btnSave4.TabIndex = 24;
            this.btnSave4.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSave4.UseVisualStyleBackColor = true;
            this.btnSave4.Click += new System.EventHandler(this.btnSave4_Click);
            // 
            // toolStrip3
            // 
            this.toolStrip3.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1,
            this.toolStripSeparator3,
            this.cboDevice,
            this.toolStripSeparator7,
            this.tbModel,
            this.toolStripSeparator6,
            this.tbDescription});
            this.toolStrip3.Location = new System.Drawing.Point(0, 0);
            this.toolStrip3.Name = "toolStrip3";
            this.toolStrip3.Size = new System.Drawing.Size(881, 28);
            this.toolStrip3.TabIndex = 95;
            this.toolStrip3.Text = "toolStrip3";
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton1.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripButton1.ForeColor = System.Drawing.Color.Blue;
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(67, 25);
            this.toolStripButton1.Text = "Device:";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.ForeColor = System.Drawing.Color.Blue;
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 28);
            // 
            // cboDevice
            // 
            this.cboDevice.AutoSize = false;
            this.cboDevice.ForeColor = System.Drawing.Color.Blue;
            this.cboDevice.Name = "cboDevice";
            this.cboDevice.Size = new System.Drawing.Size(300, 28);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.ForeColor = System.Drawing.Color.Blue;
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(6, 28);
            // 
            // tbModel
            // 
            this.tbModel.ForeColor = System.Drawing.Color.Blue;
            this.tbModel.Name = "tbModel";
            this.tbModel.Size = new System.Drawing.Size(116, 25);
            this.tbModel.Text = "toolStripLabel1";
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.ForeColor = System.Drawing.Color.Blue;
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(6, 28);
            // 
            // tbDescription
            // 
            this.tbDescription.ForeColor = System.Drawing.Color.Blue;
            this.tbDescription.Name = "tbDescription";
            this.tbDescription.Size = new System.Drawing.Size(116, 25);
            this.tbDescription.Text = "toolStripLabel2";
            // 
            // frmDimmer
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(881, 522);
            this.Controls.Add(this.tab1);
            this.Controls.Add(this.panel16);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.toolStrip3);
            this.Controls.Add(this.toolStrip1);
            this.Font = new System.Drawing.Font("Calibri", 9F);
            this.Name = "frmDimmer";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmDimmer_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmDimmer_FormClosed_1);
            this.Load += new System.EventHandler(this.frmDimmer_Load);
            this.Shown += new System.EventHandler(this.frmDimmer_Shown);
            this.SizeChanged += new System.EventHandler(this.frmDimmer_SizeChanged);
            this.tab1.ResumeLayout(false);
            this.cmNew.ResumeLayout(false);
            this.tp1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.DgChns)).EndInit();
            this.panBasic.ResumeLayout(false);
            this.groupBoxDimmingMode.ResumeLayout(false);
            this.groupBoxDimmingMode.PerformLayout();
            this.VoltageSelection.ResumeLayout(false);
            this.VoltageSelection.PerformLayout();
            this.pnBroadcast.ResumeLayout(false);
            this.tabZone.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvZoneChn1)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.panel7.ResumeLayout(false);
            this.panel7.PerformLayout();
            this.tabScene.ResumeLayout(false);
            this.gbScene.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgScene)).EndInit();
            this.groupBox6.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.tp4.ResumeLayout(false);
            this.groupBox8.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.DgvSeries)).EndInit();
            this.groupBox7.ResumeLayout(false);
            this.cmSequence.ResumeLayout(false);
            this.panel9.ResumeLayout(false);
            this.panel9.PerformLayout();
            this.tabDali.ResumeLayout(false);
            this.gbDaliManagement.ResumeLayout(false);
            this.gbDaliManagement.PerformLayout();
            this.cmDali.ResumeLayout(false);
            this.tabDaliStatus.ResumeLayout(false);
            this.cmChnScene.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.panel16.ResumeLayout(false);
            this.toolStrip3.ResumeLayout(false);
            this.toolStrip3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tab1;
        private System.Windows.Forms.TabPage tp1;
        private System.Windows.Forms.TabPage tabScene;
        private System.Windows.Forms.TabPage tp4;
        private System.Windows.Forms.DataGridView DgvSeries;
        private System.Windows.Forms.Panel panel9;
        private System.Windows.Forms.ComboBox cbstep;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TreeView tvSeries;
        private System.Windows.Forms.ContextMenuStrip cmSequence;
        private System.Windows.Forms.ToolStripMenuItem tsmDeleteSce;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private System.Windows.Forms.ToolStripMenuItem tsmCopySce;
        private System.Windows.Forms.ToolStripMenuItem tsmPasteSce;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButton5;
        private System.Windows.Forms.ToolStripButton toolStripButton6;
        private System.Windows.Forms.ToolStripButton toolStripButton7;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripButton toolStripButton9;
        private System.Windows.Forms.ToolStripButton toolStripButton10;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripButton toolStripButton11;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ContextMenuStrip cmChnScene;
        private System.Windows.Forms.ToolStripMenuItem tmiSce;
        private System.Windows.Forms.ToolStripMenuItem tmiDefault;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel tsl1;
        private System.Windows.Forms.Panel panBasic;
        private System.Windows.Forms.ToolStripMenuItem tmSame;
        private System.Windows.Forms.ToolStripMenuItem tmAscend;
        private System.Windows.Forms.ToolStripMenuItem tmdescend;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem outputSceneToolStripMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel tsl2;
        private System.Windows.Forms.ToolStripStatusLabel tsl3;
        private System.Windows.Forms.ToolStripProgressBar tsbar;
        private System.Windows.Forms.ToolStripStatusLabel tsbl4;
        private System.Windows.Forms.ToolStripButton tsbDown;
        private System.Windows.Forms.ToolStripStatusLabel tsbHint;
        private System.Windows.Forms.ComboBox cbSeriesRepeat;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.TextBox tbRemarkSeries;
        private System.Windows.Forms.ContextMenuStrip cmNew;
        private System.Windows.Forms.ToolStripMenuItem tmRead;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator9;
        private System.Windows.Forms.ToolStripButton tsbUpload;
        private System.Windows.Forms.CheckBox chbBroadcast;
        private System.Windows.Forms.GroupBox groupBoxDimmingMode;
        private System.Windows.Forms.RadioButton radioButton110V;
        private System.Windows.Forms.RadioButton radioButton220V;
        private System.Windows.Forms.ImageList imgs;
        private System.Windows.Forms.TabPage tabZone;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Label lb22;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label lb11;
        private System.Windows.Forms.Label lb2;
        private System.Windows.Forms.Label lb1;
        private System.Windows.Forms.Button btnAddZone;
        private System.Windows.Forms.TextBox txtZoneRemark;
        private System.Windows.Forms.Label lb3;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btn2;
        private System.Windows.Forms.Button btn1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TreeView tvZone;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DataGridView dgvZoneChn1;
        private System.Windows.Forms.ComboBox cbSeqMode;
        private System.Windows.Forms.Label lbSeqMode;
        private System.Windows.Forms.RadioButton radioButtonLeading;
        private System.Windows.Forms.ImageList img2;
        private System.Windows.Forms.RadioButton radioButtonTrailing;
        private System.Windows.Forms.GroupBox VoltageSelection;
        private System.Windows.Forms.Panel panel16;
        private System.Windows.Forms.Button btnRef4;
        private System.Windows.Forms.Button btnSaveAndClose4;
        private System.Windows.Forms.Button btnSave4;
        private System.Windows.Forms.GroupBox gbScene;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.TreeView tvScene;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.TextBox tbSceneName;
        private System.Windows.Forms.Label lbSceneName;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.GroupBox groupBox8;
        private System.Windows.Forms.DataGridView DgChns;
        private System.Windows.Forms.Label RunningTime;
        private TimeText tbRunningTime;
        private System.Windows.Forms.CheckBox chbOutputS;
        private System.Windows.Forms.ComboBox cbRestoreList;
        private System.Windows.Forms.Label RestoreScene;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewComboBoxColumn SelectScens;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column7;
        private System.Windows.Forms.Panel pnBroadcast;
        private System.Windows.Forms.DataGridView dgScene;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewCheckBoxColumn2;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem testToolStripMenuItem;
        private System.Windows.Forms.CheckBox chbSyn;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel HintScene;
        private System.Windows.Forms.ToolStrip toolStrip3;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripComboBox cboDevice;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripLabel tbModel;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripLabel tbDescription;
        private System.Windows.Forms.DataGridViewTextBoxColumn cl1;
        private System.Windows.Forms.DataGridViewTextBoxColumn cl2;
        private System.Windows.Forms.DataGridViewTextBoxColumn cl3;
        private System.Windows.Forms.DataGridViewTextBoxColumn cl4;
        private System.Windows.Forms.DataGridViewTextBoxColumn cl5;
        private System.Windows.Forms.DataGridViewTextBoxColumn cl6;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnDimmingProfile;
        private System.Windows.Forms.DataGridViewTextBoxColumn cl8;
        private System.Windows.Forms.DataGridViewTextBoxColumn clFailLevel;
        private System.Windows.Forms.DataGridViewTextBoxColumn clPowerOnLevel;
        private System.Windows.Forms.DataGridViewCheckBoxColumn cl0;
        private System.Windows.Forms.TabPage tabDali;
        private System.Windows.Forms.GroupBox gbDaliManagement;
        private System.Windows.Forms.Label lbInitial;
        private System.Windows.Forms.Button btnNewAddress;
        private System.Windows.Forms.Button btnDelAddress;
        private System.Windows.Forms.Button btnDuplicate;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnInitial;
        private System.Windows.Forms.ListView lvAddresses;
        private System.Windows.Forms.TabPage tabDaliStatus;
        private System.Windows.Forms.ListView lvDaliStatus;
        private System.Windows.Forms.ColumnHeader chAddress;
        private System.Windows.Forms.ColumnHeader chStatus;
        private System.Windows.Forms.ColumnHeader chLoad;
        private System.Windows.Forms.ColumnHeader chOnOff;
        private System.Windows.Forms.ColumnHeader chDimRange;
        private System.Windows.Forms.ColumnHeader chDim;
        private System.Windows.Forms.ColumnHeader chReset;
        private System.Windows.Forms.TextBox tbNewAddress;
        private System.Windows.Forms.Label lbNewAddress;
        private System.Windows.Forms.TextBox tbOldAddress;
        private System.Windows.Forms.Label lbOldAddress;
        private System.Windows.Forms.TextBox tbDelAddress;
        private System.Windows.Forms.Label lbDelAddress;
        private System.Windows.Forms.TextBox tbDuplicate;
        private System.Windows.Forms.Label lbDuplcate;
        private System.Windows.Forms.Label lbAdd;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnView;
        private System.Windows.Forms.ComboBox cboArea;
        private System.Windows.Forms.Label lbArea;
        private System.Windows.Forms.Button btnFlick;
        private System.Windows.Forms.ContextMenuStrip cmDali;
        private System.Windows.Forms.ToolStripMenuItem tsbFlick;
        private System.Windows.Forms.ToolStripMenuItem tsbSelectAll;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator10;
        private System.Windows.Forms.ToolStripMenuItem tsbFlickAll;
        private System.Windows.Forms.ComboBox cboDaliTime;
    }
}

