namespace HDL_Buspro_Setup_Tool
{
    partial class frmRefreshFlash
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmRefreshFlash));
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.tslHint = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsl2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.HintBar = new System.Windows.Forms.ToolStripProgressBar();
            this.tsbl4 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsbHint1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabAuto = new System.Windows.Forms.TabPage();
            this.gbListA = new System.Windows.Forms.GroupBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dgvListA = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1 = new System.Windows.Forms.Panel();
            this.chbOption = new System.Windows.Forms.CheckBox();
            this.btnUpdateA = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.gbDevA = new System.Windows.Forms.GroupBox();
            this.StartPos = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.btnFindA = new System.Windows.Forms.Button();
            this.btnRead1A = new System.Windows.Forms.Button();
            this.lbModelVA = new System.Windows.Forms.Label();
            this.lbModelA = new System.Windows.Forms.Label();
            this.btnAddA = new System.Windows.Forms.Button();
            this.tbLocationA = new System.Windows.Forms.TextBox();
            this.lbOpenA = new System.Windows.Forms.Label();
            this.numDevA = new System.Windows.Forms.NumericUpDown();
            this.lbDevIDA = new System.Windows.Forms.Label();
            this.numSub = new System.Windows.Forms.NumericUpDown();
            this.lbDevMA = new System.Windows.Forms.Label();
            this.btnReadA = new System.Windows.Forms.Button();
            this.cboDevA = new System.Windows.Forms.ComboBox();
            this.lbDevA = new System.Windows.Forms.Label();
            this.tabManual = new System.Windows.Forms.TabPage();
            this.gbManual = new System.Windows.Forms.GroupBox();
            this.btnGetDevice = new System.Windows.Forms.Button();
            this.EndAddress = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.StartAddress = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnUpdateM = new System.Windows.Forms.Button();
            this.tbLocM = new System.Windows.Forms.TextBox();
            this.lbFileM = new System.Windows.Forms.Label();
            this.btnReadM = new System.Windows.Forms.Button();
            this.cboDevM = new System.Windows.Forms.ComboBox();
            this.lbDevM = new System.Windows.Forms.Label();
            this.btnHelp = new System.Windows.Forms.Button();
            this.statusStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabAuto.SuspendLayout();
            this.gbListA.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvListA)).BeginInit();
            this.panel1.SuspendLayout();
            this.gbDevA.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.StartPos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDevA)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSub)).BeginInit();
            this.tabManual.SuspendLayout();
            this.gbManual.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tslHint,
            this.tsl2,
            this.HintBar,
            this.tsbl4,
            this.tsbHint1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 495);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 16, 0);
            this.statusStrip1.Size = new System.Drawing.Size(808, 26);
            this.statusStrip1.TabIndex = 41;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // tslHint
            // 
            this.tslHint.AutoSize = false;
            this.tslHint.Name = "tslHint";
            this.tslHint.Size = new System.Drawing.Size(150, 21);
            this.tslHint.Text = "Current Device:";
            // 
            // tsl2
            // 
            this.tsl2.Name = "tsl2";
            this.tsl2.Size = new System.Drawing.Size(13, 21);
            this.tsl2.Text = "|";
            // 
            // HintBar
            // 
            this.HintBar.AutoSize = false;
            this.HintBar.Name = "HintBar";
            this.HintBar.Size = new System.Drawing.Size(233, 20);
            this.HintBar.Visible = false;
            // 
            // tsbl4
            // 
            this.tsbl4.Name = "tsbl4";
            this.tsbl4.Size = new System.Drawing.Size(0, 21);
            // 
            // tsbHint1
            // 
            this.tsbHint1.AutoSize = false;
            this.tsbHint1.Name = "tsbHint1";
            this.tsbHint1.Size = new System.Drawing.Size(160, 21);
            this.tsbHint1.Text = "toolStripStatusLabel1";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabAuto);
            this.tabControl1.Controls.Add(this.tabManual);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(808, 495);
            this.tabControl1.TabIndex = 42;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tabAuto
            // 
            this.tabAuto.BackColor = System.Drawing.SystemColors.Control;
            this.tabAuto.Controls.Add(this.gbListA);
            this.tabAuto.Controls.Add(this.gbDevA);
            this.tabAuto.Location = new System.Drawing.Point(4, 23);
            this.tabAuto.Name = "tabAuto";
            this.tabAuto.Padding = new System.Windows.Forms.Padding(3);
            this.tabAuto.Size = new System.Drawing.Size(800, 468);
            this.tabAuto.TabIndex = 0;
            this.tabAuto.Text = "Automatic Upgrade";
            // 
            // gbListA
            // 
            this.gbListA.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.gbListA.Controls.Add(this.panel2);
            this.gbListA.Controls.Add(this.panel1);
            this.gbListA.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbListA.Location = new System.Drawing.Point(3, 168);
            this.gbListA.Name = "gbListA";
            this.gbListA.Size = new System.Drawing.Size(794, 297);
            this.gbListA.TabIndex = 1;
            this.gbListA.TabStop = false;
            this.gbListA.Text = "Upgrade List";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.dgvListA);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(3, 18);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(788, 232);
            this.panel2.TabIndex = 145;
            // 
            // dgvListA
            // 
            this.dgvListA.AllowUserToAddRows = false;
            this.dgvListA.AllowUserToDeleteRows = false;
            this.dgvListA.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.AliceBlue;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvListA.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvListA.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvListA.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3,
            this.Column4,
            this.Column5});
            this.dgvListA.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvListA.EnableHeadersVisualStyles = false;
            this.dgvListA.Location = new System.Drawing.Point(0, 0);
            this.dgvListA.Name = "dgvListA";
            this.dgvListA.RowHeadersVisible = false;
            this.dgvListA.RowHeadersWidth = 10;
            this.dgvListA.RowTemplate.Height = 23;
            this.dgvListA.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvListA.Size = new System.Drawing.Size(788, 232);
            this.dgvListA.TabIndex = 139;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "";
            this.Column1.Name = "Column1";
            this.Column1.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Column1.Width = 32;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "Subnet ID";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            this.Column2.Width = 85;
            // 
            // Column3
            // 
            this.Column3.HeaderText = "Device ID";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            this.Column3.Width = 85;
            // 
            // Column4
            // 
            this.Column4.HeaderText = "Status";
            this.Column4.Name = "Column4";
            this.Column4.ReadOnly = true;
            this.Column4.Width = 80;
            // 
            // Column5
            // 
            this.Column5.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column5.HeaderText = "Upgrade File";
            this.Column5.Name = "Column5";
            this.Column5.ReadOnly = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.chbOption);
            this.panel1.Controls.Add(this.btnUpdateA);
            this.panel1.Controls.Add(this.btnDelete);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(3, 250);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(788, 44);
            this.panel1.TabIndex = 144;
            // 
            // chbOption
            // 
            this.chbOption.AutoSize = true;
            this.chbOption.Location = new System.Drawing.Point(8, 13);
            this.chbOption.Name = "chbOption";
            this.chbOption.Size = new System.Drawing.Size(114, 18);
            this.chbOption.TabIndex = 140;
            this.chbOption.Text = "Select All/ None";
            this.chbOption.UseVisualStyleBackColor = true;
            this.chbOption.CheckedChanged += new System.EventHandler(this.chbOption_CheckedChanged);
            // 
            // btnUpdateA
            // 
            this.btnUpdateA.Location = new System.Drawing.Point(664, 9);
            this.btnUpdateA.Name = "btnUpdateA";
            this.btnUpdateA.Size = new System.Drawing.Size(118, 32);
            this.btnUpdateA.TabIndex = 141;
            this.btnUpdateA.Text = "Upgrade";
            this.btnUpdateA.UseVisualStyleBackColor = true;
            this.btnUpdateA.Click += new System.EventHandler(this.btnUpdateA_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(177, 6);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(170, 32);
            this.btnDelete.TabIndex = 143;
            this.btnDelete.Text = "Delete Selected Devices";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // gbDevA
            // 
            this.gbDevA.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.gbDevA.Controls.Add(this.btnHelp);
            this.gbDevA.Controls.Add(this.StartPos);
            this.gbDevA.Controls.Add(this.label3);
            this.gbDevA.Controls.Add(this.btnFindA);
            this.gbDevA.Controls.Add(this.btnRead1A);
            this.gbDevA.Controls.Add(this.lbModelVA);
            this.gbDevA.Controls.Add(this.lbModelA);
            this.gbDevA.Controls.Add(this.btnAddA);
            this.gbDevA.Controls.Add(this.tbLocationA);
            this.gbDevA.Controls.Add(this.lbOpenA);
            this.gbDevA.Controls.Add(this.numDevA);
            this.gbDevA.Controls.Add(this.lbDevIDA);
            this.gbDevA.Controls.Add(this.numSub);
            this.gbDevA.Controls.Add(this.lbDevMA);
            this.gbDevA.Controls.Add(this.btnReadA);
            this.gbDevA.Controls.Add(this.cboDevA);
            this.gbDevA.Controls.Add(this.lbDevA);
            this.gbDevA.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbDevA.Location = new System.Drawing.Point(3, 3);
            this.gbDevA.Name = "gbDevA";
            this.gbDevA.Size = new System.Drawing.Size(794, 165);
            this.gbDevA.TabIndex = 0;
            this.gbDevA.TabStop = false;
            this.gbDevA.Text = "Select Device";
            // 
            // StartPos
            // 
            this.StartPos.Location = new System.Drawing.Point(544, 61);
            this.StartPos.Maximum = new decimal(new int[] {
            8388608,
            0,
            0,
            0});
            this.StartPos.Name = "StartPos";
            this.StartPos.Size = new System.Drawing.Size(117, 22);
            this.StartPos.TabIndex = 141;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.Blue;
            this.label3.Location = new System.Drawing.Point(490, 64);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 14);
            this.label3.TabIndex = 140;
            this.label3.Text = "Start:";
            // 
            // btnFindA
            // 
            this.btnFindA.Location = new System.Drawing.Point(667, 95);
            this.btnFindA.Name = "btnFindA";
            this.btnFindA.Size = new System.Drawing.Size(28, 26);
            this.btnFindA.TabIndex = 139;
            this.btnFindA.Text = "...";
            this.btnFindA.UseVisualStyleBackColor = true;
            this.btnFindA.Click += new System.EventHandler(this.btnFindA_Click);
            // 
            // btnRead1A
            // 
            this.btnRead1A.Location = new System.Drawing.Point(667, 62);
            this.btnRead1A.Name = "btnRead1A";
            this.btnRead1A.Size = new System.Drawing.Size(118, 26);
            this.btnRead1A.TabIndex = 138;
            this.btnRead1A.Text = "Read Device Type";
            this.btnRead1A.UseVisualStyleBackColor = true;
            this.btnRead1A.Click += new System.EventHandler(this.btnRead1A_Click);
            // 
            // lbModelVA
            // 
            this.lbModelVA.Location = new System.Drawing.Point(156, 126);
            this.lbModelVA.Name = "lbModelVA";
            this.lbModelVA.Size = new System.Drawing.Size(505, 26);
            this.lbModelVA.TabIndex = 136;
            // 
            // lbModelA
            // 
            this.lbModelA.AutoSize = true;
            this.lbModelA.ForeColor = System.Drawing.Color.Black;
            this.lbModelA.Location = new System.Drawing.Point(8, 129);
            this.lbModelA.Name = "lbModelA";
            this.lbModelA.Size = new System.Drawing.Size(84, 14);
            this.lbModelA.TabIndex = 135;
            this.lbModelA.Text = "Device Model:";
            // 
            // btnAddA
            // 
            this.btnAddA.Location = new System.Drawing.Point(695, 95);
            this.btnAddA.Name = "btnAddA";
            this.btnAddA.Size = new System.Drawing.Size(90, 26);
            this.btnAddA.TabIndex = 134;
            this.btnAddA.Text = "Add Device";
            this.btnAddA.UseVisualStyleBackColor = true;
            this.btnAddA.Click += new System.EventHandler(this.btnAddA_Click);
            // 
            // tbLocationA
            // 
            this.tbLocationA.Location = new System.Drawing.Point(154, 96);
            this.tbLocationA.Name = "tbLocationA";
            this.tbLocationA.Size = new System.Drawing.Size(506, 22);
            this.tbLocationA.TabIndex = 133;
            // 
            // lbOpenA
            // 
            this.lbOpenA.AutoSize = true;
            this.lbOpenA.ForeColor = System.Drawing.Color.Black;
            this.lbOpenA.Location = new System.Drawing.Point(8, 98);
            this.lbOpenA.Name = "lbOpenA";
            this.lbOpenA.Size = new System.Drawing.Size(67, 14);
            this.lbOpenA.TabIndex = 132;
            this.lbOpenA.Text = "Select File:";
            // 
            // numDevA
            // 
            this.numDevA.Location = new System.Drawing.Point(355, 61);
            this.numDevA.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numDevA.Name = "numDevA";
            this.numDevA.ReadOnly = true;
            this.numDevA.Size = new System.Drawing.Size(117, 22);
            this.numDevA.TabIndex = 131;
            this.numDevA.ValueChanged += new System.EventHandler(this.numDevA_ValueChanged);
            // 
            // lbDevIDA
            // 
            this.lbDevIDA.AutoSize = true;
            this.lbDevIDA.ForeColor = System.Drawing.Color.Black;
            this.lbDevIDA.Location = new System.Drawing.Point(278, 63);
            this.lbDevIDA.Name = "lbDevIDA";
            this.lbDevIDA.Size = new System.Drawing.Size(61, 14);
            this.lbDevIDA.TabIndex = 130;
            this.lbDevIDA.Text = "Device ID:";
            // 
            // numSub
            // 
            this.numSub.Location = new System.Drawing.Point(154, 62);
            this.numSub.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numSub.Name = "numSub";
            this.numSub.ReadOnly = true;
            this.numSub.Size = new System.Drawing.Size(117, 22);
            this.numSub.TabIndex = 129;
            this.numSub.ValueChanged += new System.EventHandler(this.numSub_ValueChanged);
            // 
            // lbDevMA
            // 
            this.lbDevMA.AutoSize = true;
            this.lbDevMA.ForeColor = System.Drawing.Color.Black;
            this.lbDevMA.Location = new System.Drawing.Point(8, 64);
            this.lbDevMA.Name = "lbDevMA";
            this.lbDevMA.Size = new System.Drawing.Size(118, 14);
            this.lbDevMA.TabIndex = 128;
            this.lbDevMA.Text = "Manually SubNet ID:";
            // 
            // btnReadA
            // 
            this.btnReadA.Location = new System.Drawing.Point(667, 25);
            this.btnReadA.Name = "btnReadA";
            this.btnReadA.Size = new System.Drawing.Size(118, 26);
            this.btnReadA.TabIndex = 126;
            this.btnReadA.Text = "Get Device List";
            this.btnReadA.UseVisualStyleBackColor = true;
            this.btnReadA.Click += new System.EventHandler(this.btnReadA_Click);
            // 
            // cboDevA
            // 
            this.cboDevA.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDevA.FormattingEnabled = true;
            this.cboDevA.Location = new System.Drawing.Point(154, 25);
            this.cboDevA.Name = "cboDevA";
            this.cboDevA.Size = new System.Drawing.Size(506, 22);
            this.cboDevA.TabIndex = 125;
            this.cboDevA.SelectedIndexChanged += new System.EventHandler(this.cboDevA_SelectedIndexChanged);
            // 
            // lbDevA
            // 
            this.lbDevA.AutoSize = true;
            this.lbDevA.ForeColor = System.Drawing.Color.Black;
            this.lbDevA.Location = new System.Drawing.Point(8, 26);
            this.lbDevA.Name = "lbDevA";
            this.lbDevA.Size = new System.Drawing.Size(82, 14);
            this.lbDevA.TabIndex = 124;
            this.lbDevA.Text = "Select Device:";
            // 
            // tabManual
            // 
            this.tabManual.BackColor = System.Drawing.SystemColors.Control;
            this.tabManual.Controls.Add(this.gbManual);
            this.tabManual.Location = new System.Drawing.Point(4, 22);
            this.tabManual.Name = "tabManual";
            this.tabManual.Padding = new System.Windows.Forms.Padding(3);
            this.tabManual.Size = new System.Drawing.Size(800, 469);
            this.tabManual.TabIndex = 1;
            this.tabManual.Text = "Read Flash";
            // 
            // gbManual
            // 
            this.gbManual.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.gbManual.Controls.Add(this.btnGetDevice);
            this.gbManual.Controls.Add(this.EndAddress);
            this.gbManual.Controls.Add(this.label2);
            this.gbManual.Controls.Add(this.StartAddress);
            this.gbManual.Controls.Add(this.label1);
            this.gbManual.Controls.Add(this.btnUpdateM);
            this.gbManual.Controls.Add(this.tbLocM);
            this.gbManual.Controls.Add(this.lbFileM);
            this.gbManual.Controls.Add(this.btnReadM);
            this.gbManual.Controls.Add(this.cboDevM);
            this.gbManual.Controls.Add(this.lbDevM);
            this.gbManual.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbManual.Location = new System.Drawing.Point(3, 3);
            this.gbManual.Name = "gbManual";
            this.gbManual.Size = new System.Drawing.Size(794, 129);
            this.gbManual.TabIndex = 2;
            this.gbManual.TabStop = false;
            this.gbManual.Text = "Select Device";
            // 
            // btnGetDevice
            // 
            this.btnGetDevice.Location = new System.Drawing.Point(723, 23);
            this.btnGetDevice.Name = "btnGetDevice";
            this.btnGetDevice.Size = new System.Drawing.Size(118, 26);
            this.btnGetDevice.TabIndex = 146;
            this.btnGetDevice.Text = "Get Device List";
            this.btnGetDevice.UseVisualStyleBackColor = true;
            this.btnGetDevice.Click += new System.EventHandler(this.btnReadA_Click);
            // 
            // EndAddress
            // 
            this.EndAddress.Location = new System.Drawing.Point(467, 57);
            this.EndAddress.Name = "EndAddress";
            this.EndAddress.Size = new System.Drawing.Size(193, 22);
            this.EndAddress.TabIndex = 145;
            this.EndAddress.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.StartAddress_KeyPress);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(355, 59);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 14);
            this.label2.TabIndex = 144;
            this.label2.Text = "End Address:";
            // 
            // StartAddress
            // 
            this.StartAddress.Location = new System.Drawing.Point(154, 56);
            this.StartAddress.Name = "StartAddress";
            this.StartAddress.Size = new System.Drawing.Size(193, 22);
            this.StartAddress.TabIndex = 143;
            this.StartAddress.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.StartAddress_KeyPress);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(30, 56);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 14);
            this.label1.TabIndex = 142;
            this.label1.Text = "Start Address:";
            // 
            // btnUpdateM
            // 
            this.btnUpdateM.Location = new System.Drawing.Point(723, 88);
            this.btnUpdateM.Name = "btnUpdateM";
            this.btnUpdateM.Size = new System.Drawing.Size(118, 26);
            this.btnUpdateM.TabIndex = 134;
            this.btnUpdateM.Text = "Read";
            this.btnUpdateM.UseVisualStyleBackColor = true;
            this.btnUpdateM.Click += new System.EventHandler(this.btnUpdateM_Click);
            // 
            // tbLocM
            // 
            this.tbLocM.Location = new System.Drawing.Point(154, 88);
            this.tbLocM.Name = "tbLocM";
            this.tbLocM.Size = new System.Drawing.Size(506, 22);
            this.tbLocM.TabIndex = 133;
            // 
            // lbFileM
            // 
            this.lbFileM.AutoSize = true;
            this.lbFileM.ForeColor = System.Drawing.Color.Black;
            this.lbFileM.Location = new System.Drawing.Point(30, 91);
            this.lbFileM.Name = "lbFileM";
            this.lbFileM.Size = new System.Drawing.Size(70, 14);
            this.lbFileM.TabIndex = 132;
            this.lbFileM.Text = "Select Path:";
            // 
            // btnReadM
            // 
            this.btnReadM.Location = new System.Drawing.Point(723, 56);
            this.btnReadM.Name = "btnReadM";
            this.btnReadM.Size = new System.Drawing.Size(118, 26);
            this.btnReadM.TabIndex = 126;
            this.btnReadM.Text = "OK";
            this.btnReadM.UseVisualStyleBackColor = true;
            this.btnReadM.Click += new System.EventHandler(this.btnReadM_Click);
            // 
            // cboDevM
            // 
            this.cboDevM.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDevM.FormattingEnabled = true;
            this.cboDevM.Location = new System.Drawing.Point(154, 25);
            this.cboDevM.Name = "cboDevM";
            this.cboDevM.Size = new System.Drawing.Size(506, 22);
            this.cboDevM.TabIndex = 125;
            // 
            // lbDevM
            // 
            this.lbDevM.AutoSize = true;
            this.lbDevM.ForeColor = System.Drawing.Color.Black;
            this.lbDevM.Location = new System.Drawing.Point(30, 27);
            this.lbDevM.Name = "lbDevM";
            this.lbDevM.Size = new System.Drawing.Size(46, 14);
            this.lbDevM.TabIndex = 124;
            this.lbDevM.Text = "Device:";
            // 
            // btnHelp
            // 
            this.btnHelp.Location = new System.Drawing.Point(667, 127);
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new System.Drawing.Size(118, 26);
            this.btnHelp.TabIndex = 142;
            this.btnHelp.Text = "Help";
            this.btnHelp.UseVisualStyleBackColor = true;
            this.btnHelp.Click += new System.EventHandler(this.btnHelp_Click);
            // 
            // frmRefreshFlash
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(808, 521);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.statusStrip1);
            this.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmRefreshFlash";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Firmware Upgrade";
            this.Load += new System.EventHandler(this.frmUpgrade_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabAuto.ResumeLayout(false);
            this.gbListA.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvListA)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.gbDevA.ResumeLayout(false);
            this.gbDevA.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.StartPos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDevA)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSub)).EndInit();
            this.tabManual.ResumeLayout(false);
            this.gbManual.ResumeLayout(false);
            this.gbManual.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel tslHint;
        private System.Windows.Forms.ToolStripStatusLabel tsl2;
        private System.Windows.Forms.ToolStripStatusLabel tsbHint1;
        private System.Windows.Forms.ToolStripProgressBar HintBar;
        private System.Windows.Forms.ToolStripStatusLabel tsbl4;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabAuto;
        private System.Windows.Forms.GroupBox gbDevA;
        private System.Windows.Forms.TabPage tabManual;
        private System.Windows.Forms.ComboBox cboDevA;
        private System.Windows.Forms.Label lbDevA;
        private System.Windows.Forms.Button btnReadA;
        private System.Windows.Forms.NumericUpDown numDevA;
        private System.Windows.Forms.Label lbDevIDA;
        private System.Windows.Forms.NumericUpDown numSub;
        private System.Windows.Forms.Label lbDevMA;
        private System.Windows.Forms.TextBox tbLocationA;
        private System.Windows.Forms.Label lbOpenA;
        private System.Windows.Forms.Button btnAddA;
        private System.Windows.Forms.Label lbModelVA;
        private System.Windows.Forms.Label lbModelA;
        private System.Windows.Forms.GroupBox gbListA;
        private System.Windows.Forms.Button btnUpdateA;
        private System.Windows.Forms.CheckBox chbOption;
        private System.Windows.Forms.DataGridView dgvListA;
        private System.Windows.Forms.Button btnFindA;
        private System.Windows.Forms.Button btnRead1A;
        private System.Windows.Forms.GroupBox gbManual;
        private System.Windows.Forms.Button btnUpdateM;
        private System.Windows.Forms.TextBox tbLocM;
        private System.Windows.Forms.Label lbFileM;
        private System.Windows.Forms.Button btnReadM;
        private System.Windows.Forms.ComboBox cboDevM;
        private System.Windows.Forms.Label lbDevM;
        private System.Windows.Forms.TextBox EndAddress;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox StartAddress;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown StartPos;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnGetDevice;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.Button btnHelp;
    }
}