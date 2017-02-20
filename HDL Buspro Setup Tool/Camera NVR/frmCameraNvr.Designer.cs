namespace HDL_Buspro_Setup_Tool
{
    partial class frmCameraNvr
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCameraNvr));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tbnew = new System.Windows.Forms.ToolStripButton();
            this.tbSave = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton7 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton9 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton10 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.tslRead = new System.Windows.Forms.ToolStripButton();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton11 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.panel2 = new System.Windows.Forms.Panel();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsl31 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStripStatusLabel4 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsbHint = new System.Windows.Forms.ToolStripStatusLabel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabCamera = new System.Windows.Forms.TabPage();
            this.panel7 = new System.Windows.Forms.Panel();
            this.userNameCtrl = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.passwordCtrl = new System.Windows.Forms.TextBox();
            this.tabNvr = new System.Windows.Forms.TabPage();
            this.panel6 = new System.Windows.Forms.Panel();
            this.cameraListCtrl = new System.Windows.Forms.DataGridView();
            this.ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IP = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Port = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Chn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UserName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Password = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnHelp = new System.Windows.Forms.Button();
            this.btnRef1 = new System.Windows.Forms.Button();
            this.btnSaveAndClose1 = new System.Windows.Forms.Button();
            this.btnSave1 = new System.Windows.Forms.Button();
            this.toolStrip3 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton13 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.cboDevice = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.tbModel = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.tbDescription = new System.Windows.Forms.ToolStripLabel();
            this.toolStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabCamera.SuspendLayout();
            this.panel7.SuspendLayout();
            this.tabNvr.SuspendLayout();
            this.panel6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cameraListCtrl)).BeginInit();
            this.panel3.SuspendLayout();
            this.toolStrip3.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tbnew,
            this.tbSave,
            this.toolStripButton7,
            this.toolStripSeparator4,
            this.toolStripButton9,
            this.toolStripButton10,
            this.toolStripSeparator5,
            this.tslRead,
            this.toolStripLabel2,
            this.toolStripButton11,
            this.toolStripSeparator1});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(775, 25);
            this.toolStrip1.TabIndex = 80;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tbnew
            // 
            this.tbnew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbnew.Image = global::HDL_Buspro_Setup_Tool.Properties.Resources.OK;
            this.tbnew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbnew.Name = "tbnew";
            this.tbnew.Size = new System.Drawing.Size(23, 22);
            this.tbnew.Text = "&New";
            this.tbnew.ToolTipText = "Load Default";
            // 
            // tbSave
            // 
            this.tbSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbSave.Image = global::HDL_Buspro_Setup_Tool.Properties.Resources.OK;
            this.tbSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbSave.Name = "tbSave";
            this.tbSave.Size = new System.Drawing.Size(23, 22);
            this.tbSave.Text = "&Save";
            this.tbSave.ToolTipText = "Save ";
            // 
            // toolStripButton7
            // 
            this.toolStripButton7.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton7.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton7.Image")));
            this.toolStripButton7.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton7.Name = "toolStripButton7";
            this.toolStripButton7.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton7.Text = "&Print";
            this.toolStripButton7.Visible = false;
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButton9
            // 
            this.toolStripButton9.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton9.Image = global::HDL_Buspro_Setup_Tool.Properties.Resources.OK;
            this.toolStripButton9.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton9.Name = "toolStripButton9";
            this.toolStripButton9.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton9.Text = "&Copy";
            // 
            // toolStripButton10
            // 
            this.toolStripButton10.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton10.Image = global::HDL_Buspro_Setup_Tool.Properties.Resources.OK;
            this.toolStripButton10.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton10.Name = "toolStripButton10";
            this.toolStripButton10.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton10.Text = "&Paste";
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 25);
            // 
            // tslRead
            // 
            this.tslRead.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tslRead.Font = new System.Drawing.Font("Times New Roman", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tslRead.Image = global::HDL_Buspro_Setup_Tool.Properties.Resources.OK;
            this.tslRead.ImageTransparentColor = System.Drawing.Color.Silver;
            this.tslRead.Name = "tslRead";
            this.tslRead.Size = new System.Drawing.Size(23, 22);
            this.tslRead.Tag = "0";
            this.tslRead.Text = "Read";
            this.tslRead.ToolTipText = "Read";
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripLabel2.Image = global::HDL_Buspro_Setup_Tool.Properties.Resources.OK;
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(23, 22);
            this.toolStripLabel2.Tag = "1";
            this.toolStripLabel2.Text = "toolStripLabel1";
            this.toolStripLabel2.ToolTipText = "Upload";
            // 
            // toolStripButton11
            // 
            this.toolStripButton11.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton11.Image = global::HDL_Buspro_Setup_Tool.Properties.Resources.OK;
            this.toolStripButton11.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton11.Name = "toolStripButton11";
            this.toolStripButton11.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton11.Text = "He&lp";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // panel2
            // 
            this.panel2.Location = new System.Drawing.Point(25, 15);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(263, 240);
            this.panel2.TabIndex = 4;
            // 
            // statusStrip1
            // 
            this.statusStrip1.AutoSize = false;
            this.statusStrip1.Font = new System.Drawing.Font("Calibri", 9F);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripStatusLabel2,
            this.tsl31,
            this.toolStripProgressBar1,
            this.toolStripStatusLabel4,
            this.tsbHint});
            this.statusStrip1.Location = new System.Drawing.Point(0, 496);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(775, 26);
            this.statusStrip1.TabIndex = 83;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.AutoSize = false;
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(150, 21);
            this.toolStripStatusLabel1.Text = "Current Device:";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(13, 21);
            this.toolStripStatusLabel2.Text = "|";
            // 
            // tsl31
            // 
            this.tsl31.AutoSize = false;
            this.tsl31.Name = "tsl31";
            this.tsl31.Size = new System.Drawing.Size(160, 21);
            this.tsl31.Text = "tsl3";
            // 
            // toolStripProgressBar1
            // 
            this.toolStripProgressBar1.Name = "toolStripProgressBar1";
            this.toolStripProgressBar1.Size = new System.Drawing.Size(100, 20);
            this.toolStripProgressBar1.Visible = false;
            // 
            // toolStripStatusLabel4
            // 
            this.toolStripStatusLabel4.Name = "toolStripStatusLabel4";
            this.toolStripStatusLabel4.Size = new System.Drawing.Size(0, 21);
            // 
            // tsbHint
            // 
            this.tsbHint.Name = "tsbHint";
            this.tsbHint.Size = new System.Drawing.Size(124, 21);
            this.tsbHint.Text = "toolStripStatusLabel3";
            this.tsbHint.Visible = false;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabCamera);
            this.tabControl1.Controls.Add(this.tabNvr);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 50);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(775, 406);
            this.tabControl1.TabIndex = 84;
            // 
            // tabCamera
            // 
            this.tabCamera.BackColor = System.Drawing.SystemColors.Control;
            this.tabCamera.Controls.Add(this.panel7);
            this.tabCamera.Font = new System.Drawing.Font("Calibri", 9F);
            this.tabCamera.Location = new System.Drawing.Point(4, 23);
            this.tabCamera.Name = "tabCamera";
            this.tabCamera.Padding = new System.Windows.Forms.Padding(3);
            this.tabCamera.Size = new System.Drawing.Size(767, 379);
            this.tabCamera.TabIndex = 0;
            this.tabCamera.Text = "More Setup";
            // 
            // panel7
            // 
            this.panel7.Controls.Add(this.panel2);
            this.panel7.Controls.Add(this.userNameCtrl);
            this.panel7.Controls.Add(this.label1);
            this.panel7.Controls.Add(this.label2);
            this.panel7.Controls.Add(this.passwordCtrl);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel7.Font = new System.Drawing.Font("Calibri", 9F);
            this.panel7.Location = new System.Drawing.Point(3, 3);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(761, 373);
            this.panel7.TabIndex = 117;
            // 
            // userNameCtrl
            // 
            this.userNameCtrl.Location = new System.Drawing.Point(126, 261);
            this.userNameCtrl.MaxLength = 16;
            this.userNameCtrl.Name = "userNameCtrl";
            this.userNameCtrl.Size = new System.Drawing.Size(149, 22);
            this.userNameCtrl.TabIndex = 50;
            this.userNameCtrl.Tag = "2";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Calibri", 9F);
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(27, 263);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 14);
            this.label1.TabIndex = 51;
            this.label1.Text = "User name:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Calibri", 9F);
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(27, 300);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 14);
            this.label2.TabIndex = 53;
            this.label2.Text = "Password:";
            // 
            // passwordCtrl
            // 
            this.passwordCtrl.Location = new System.Drawing.Point(126, 298);
            this.passwordCtrl.MaxLength = 32;
            this.passwordCtrl.Name = "passwordCtrl";
            this.passwordCtrl.Size = new System.Drawing.Size(149, 22);
            this.passwordCtrl.TabIndex = 52;
            this.passwordCtrl.Tag = "2";
            // 
            // tabNvr
            // 
            this.tabNvr.BackColor = System.Drawing.SystemColors.Control;
            this.tabNvr.Controls.Add(this.panel6);
            this.tabNvr.Location = new System.Drawing.Point(4, 23);
            this.tabNvr.Name = "tabNvr";
            this.tabNvr.Padding = new System.Windows.Forms.Padding(3);
            this.tabNvr.Size = new System.Drawing.Size(767, 379);
            this.tabNvr.TabIndex = 1;
            this.tabNvr.Text = "More Setup";
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.cameraListCtrl);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel6.Location = new System.Drawing.Point(3, 3);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(761, 373);
            this.panel6.TabIndex = 120;
            // 
            // cameraListCtrl
            // 
            this.cameraListCtrl.AllowUserToAddRows = false;
            this.cameraListCtrl.AllowUserToDeleteRows = false;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.AliceBlue;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Calibri", 9F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.cameraListCtrl.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.cameraListCtrl.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.cameraListCtrl.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ID,
            this.IP,
            this.Port,
            this.Chn,
            this.UserName,
            this.Password});
            this.cameraListCtrl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cameraListCtrl.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.cameraListCtrl.EnableHeadersVisualStyles = false;
            this.cameraListCtrl.Location = new System.Drawing.Point(0, 0);
            this.cameraListCtrl.Name = "cameraListCtrl";
            this.cameraListCtrl.RowHeadersVisible = false;
            this.cameraListCtrl.RowHeadersWidth = 10;
            this.cameraListCtrl.RowTemplate.Height = 23;
            this.cameraListCtrl.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.cameraListCtrl.Size = new System.Drawing.Size(761, 373);
            this.cameraListCtrl.TabIndex = 53;
            this.cameraListCtrl.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.cameraListCtrl_CellMouseDoubleClick);
            // 
            // ID
            // 
            this.ID.HeaderText = "ID";
            this.ID.Name = "ID";
            this.ID.ReadOnly = true;
            this.ID.Width = 48;
            // 
            // IP
            // 
            this.IP.HeaderText = "IP";
            this.IP.Name = "IP";
            this.IP.Width = 150;
            // 
            // Port
            // 
            this.Port.HeaderText = "Port";
            this.Port.Name = "Port";
            // 
            // Chn
            // 
            this.Chn.HeaderText = "Chn.";
            this.Chn.Name = "Chn";
            this.Chn.Width = 48;
            // 
            // UserName
            // 
            this.UserName.HeaderText = "UserName";
            this.UserName.Name = "UserName";
            this.UserName.Width = 200;
            // 
            // Password
            // 
            this.Password.HeaderText = "Password";
            this.Password.Name = "Password";
            this.Password.Width = 200;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnHelp);
            this.panel3.Controls.Add(this.btnRef1);
            this.panel3.Controls.Add(this.btnSaveAndClose1);
            this.panel3.Controls.Add(this.btnSave1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 456);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(775, 40);
            this.panel3.TabIndex = 116;
            // 
            // btnHelp
            // 
            this.btnHelp.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnHelp.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnHelp.Image = global::HDL_Buspro_Setup_Tool.Properties.Resources.帮助;
            this.btnHelp.Location = new System.Drawing.Point(48, 2);
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new System.Drawing.Size(140, 32);
            this.btnHelp.TabIndex = 124;
            this.btnHelp.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnHelp.UseVisualStyleBackColor = true;
            this.btnHelp.Visible = false;
            this.btnHelp.Click += new System.EventHandler(this.btnHelp_Click);
            // 
            // btnRef1
            // 
            this.btnRef1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnRef1.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRef1.Image = global::HDL_Buspro_Setup_Tool.Properties.Resources.Refresh;
            this.btnRef1.Location = new System.Drawing.Point(210, 3);
            this.btnRef1.Name = "btnRef1";
            this.btnRef1.Size = new System.Drawing.Size(140, 32);
            this.btnRef1.TabIndex = 123;
            this.btnRef1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnRef1.UseVisualStyleBackColor = true;
            this.btnRef1.Click += new System.EventHandler(this.btnRead_Click);
            // 
            // btnSaveAndClose1
            // 
            this.btnSaveAndClose1.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSaveAndClose1.Image = global::HDL_Buspro_Setup_Tool.Properties.Resources.Save;
            this.btnSaveAndClose1.Location = new System.Drawing.Point(579, 3);
            this.btnSaveAndClose1.Name = "btnSaveAndClose1";
            this.btnSaveAndClose1.Size = new System.Drawing.Size(140, 32);
            this.btnSaveAndClose1.TabIndex = 122;
            this.btnSaveAndClose1.Text = "Save && Close";
            this.btnSaveAndClose1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSaveAndClose1.UseVisualStyleBackColor = true;
            this.btnSaveAndClose1.Click += new System.EventHandler(this.button2_Click);
            // 
            // btnSave1
            // 
            this.btnSave1.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave1.Image = global::HDL_Buspro_Setup_Tool.Properties.Resources.Save;
            this.btnSave1.Location = new System.Drawing.Point(393, 3);
            this.btnSave1.Name = "btnSave1";
            this.btnSave1.Size = new System.Drawing.Size(140, 32);
            this.btnSave1.TabIndex = 121;
            this.btnSave1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSave1.UseVisualStyleBackColor = true;
            this.btnSave1.Click += new System.EventHandler(this.btnModify_Click);
            // 
            // toolStrip3
            // 
            this.toolStrip3.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton13,
            this.toolStripSeparator2,
            this.cboDevice,
            this.toolStripSeparator7,
            this.tbModel,
            this.toolStripSeparator3,
            this.tbDescription});
            this.toolStrip3.Location = new System.Drawing.Point(0, 25);
            this.toolStrip3.Name = "toolStrip3";
            this.toolStrip3.Size = new System.Drawing.Size(775, 25);
            this.toolStrip3.TabIndex = 117;
            this.toolStrip3.Text = "toolStrip3";
            // 
            // toolStripButton13
            // 
            this.toolStripButton13.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton13.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripButton13.ForeColor = System.Drawing.Color.Blue;
            this.toolStripButton13.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton13.Name = "toolStripButton13";
            this.toolStripButton13.Size = new System.Drawing.Size(51, 22);
            this.toolStripButton13.Text = "Device:";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.ForeColor = System.Drawing.Color.Blue;
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // cboDevice
            // 
            this.cboDevice.AutoSize = false;
            this.cboDevice.ForeColor = System.Drawing.Color.Blue;
            this.cboDevice.Name = "cboDevice";
            this.cboDevice.Size = new System.Drawing.Size(300, 25);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.ForeColor = System.Drawing.Color.Blue;
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(6, 25);
            // 
            // tbModel
            // 
            this.tbModel.ForeColor = System.Drawing.Color.Blue;
            this.tbModel.Name = "tbModel";
            this.tbModel.Size = new System.Drawing.Size(91, 22);
            this.tbModel.Text = "toolStripLabel1";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.ForeColor = System.Drawing.Color.Blue;
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // tbDescription
            // 
            this.tbDescription.ForeColor = System.Drawing.Color.Blue;
            this.tbDescription.Name = "tbDescription";
            this.tbDescription.Size = new System.Drawing.Size(91, 22);
            this.tbDescription.Text = "toolStripLabel2";
            // 
            // frmCameraNvr
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(775, 522);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.toolStrip3);
            this.Controls.Add(this.toolStrip1);
            this.Font = new System.Drawing.Font("Calibri", 9F);
            this.Name = "frmCameraNvr";
            this.Text = "frmCameraNvr";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmCameraNvr_FormClosing);
            this.Load += new System.EventHandler(this.frmCameraNvr_Load);
            this.Shown += new System.EventHandler(this.frmCameraNvr_Shown);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabCamera.ResumeLayout(false);
            this.panel7.ResumeLayout(false);
            this.panel7.PerformLayout();
            this.tabNvr.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cameraListCtrl)).EndInit();
            this.panel3.ResumeLayout(false);
            this.toolStrip3.ResumeLayout(false);
            this.toolStrip3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tbnew;
        private System.Windows.Forms.ToolStripButton tbSave;
        private System.Windows.Forms.ToolStripButton toolStripButton7;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripButton toolStripButton9;
        private System.Windows.Forms.ToolStripButton toolStripButton10;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripButton tslRead;
        private System.Windows.Forms.ToolStripButton toolStripLabel2;
        private System.Windows.Forms.ToolStripButton toolStripButton11;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripStatusLabel tsl31;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel4;
        private System.Windows.Forms.ToolStripStatusLabel tsbHint;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabCamera;
        private System.Windows.Forms.TextBox passwordCtrl;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox userNameCtrl;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabPage tabNvr;
        private System.Windows.Forms.DataGridView cameraListCtrl;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button btnRef1;
        private System.Windows.Forms.Button btnSaveAndClose1;
        private System.Windows.Forms.Button btnSave1;
        private System.Windows.Forms.DataGridViewTextBoxColumn ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn IP;
        private System.Windows.Forms.DataGridViewTextBoxColumn Port;
        private System.Windows.Forms.DataGridViewTextBoxColumn Chn;
        private System.Windows.Forms.DataGridViewTextBoxColumn UserName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Password;
        private System.Windows.Forms.Button btnHelp;
        private System.Windows.Forms.ToolStrip toolStrip3;
        private System.Windows.Forms.ToolStripButton toolStripButton13;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripComboBox cboDevice;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripLabel tbModel;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripLabel tbDescription;

    }
}