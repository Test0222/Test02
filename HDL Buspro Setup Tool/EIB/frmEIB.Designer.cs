namespace HDL_Buspro_Setup_Tool
{
    partial class frmEIB
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmEIB));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsbDefault = new System.Windows.Forms.ToolStripButton();
            this.tsbSave = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton7 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton9 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton10 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbDown = new System.Windows.Forms.ToolStripButton();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton11 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tabEIB = new System.Windows.Forms.TabControl();
            this.tabLists = new System.Windows.Forms.TabPage();
            this.btnAutoAdd = new System.Windows.Forms.Button();
            this.txtNum = new System.Windows.Forms.NumericUpDown();
            this.lbAdd = new System.Windows.Forms.Label();
            this.DgvList = new System.Windows.Forms.DataGridView();
            this.cl1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cl2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clType = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.clDevices = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clSub = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clDev = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clParam1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clParam2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clParam3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clFlag = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.tsl1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsl2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsl3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsbar = new System.Windows.Forms.ToolStripProgressBar();
            this.tsbl4 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsbHint = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStrip3 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton5 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.cboDevice = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.tbModel = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.tbDescription = new System.Windows.Forms.ToolStripLabel();
            this.tbAddress = new System.Windows.Forms.TextBox();
            this.lbAddress = new System.Windows.Forms.Label();
            this.toolStrip1.SuspendLayout();
            this.tabEIB.SuspendLayout();
            this.tabLists.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtNum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DgvList)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.toolStrip3.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Font = new System.Drawing.Font("Arial", 8.25F);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbDefault,
            this.tsbSave,
            this.toolStripButton7,
            this.toolStripSeparator4,
            this.toolStripButton9,
            this.toolStripButton10,
            this.toolStripSeparator5,
            this.tsbDown,
            this.toolStripLabel2,
            this.toolStripButton11,
            this.toolStripSeparator1});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(815, 25);
            this.toolStrip1.TabIndex = 41;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tsbDefault
            // 
            this.tsbDefault.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbDefault.Image = global::HDL_Buspro_Setup_Tool.Properties.Resources.新建;
            this.tsbDefault.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbDefault.Name = "tsbDefault";
            this.tsbDefault.Size = new System.Drawing.Size(23, 22);
            this.tsbDefault.Text = "&New";
            this.tsbDefault.ToolTipText = "Load Default";
            this.tsbDefault.Click += new System.EventHandler(this.tsbDefault_Click);
            // 
            // tsbSave
            // 
            this.tsbSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbSave.Image = global::HDL_Buspro_Setup_Tool.Properties.Resources.保存;
            this.tsbSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbSave.Name = "tsbSave";
            this.tsbSave.Size = new System.Drawing.Size(23, 22);
            this.tsbSave.Text = "&Save";
            this.tsbSave.ToolTipText = "Save ";
            this.tsbSave.Click += new System.EventHandler(this.tsbSave_Click);
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
            this.toolStripButton9.Image = global::HDL_Buspro_Setup_Tool.Properties.Resources.复制;
            this.toolStripButton9.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton9.Name = "toolStripButton9";
            this.toolStripButton9.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton9.Text = "&Copy";
            // 
            // toolStripButton10
            // 
            this.toolStripButton10.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton10.Image = global::HDL_Buspro_Setup_Tool.Properties.Resources.粘贴;
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
            // tsbDown
            // 
            this.tsbDown.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbDown.Image = global::HDL_Buspro_Setup_Tool.Properties.Resources.下载;
            this.tsbDown.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbDown.Name = "tsbDown";
            this.tsbDown.Size = new System.Drawing.Size(23, 22);
            this.tsbDown.Tag = "0";
            this.tsbDown.Text = "Read";
            this.tsbDown.ToolTipText = "Read";
            this.tsbDown.Click += new System.EventHandler(this.tsbDown_Click);
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripLabel2.Image = global::HDL_Buspro_Setup_Tool.Properties.Resources.上传;
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(23, 22);
            this.toolStripLabel2.Tag = "1";
            this.toolStripLabel2.Text = "toolStripLabel1";
            this.toolStripLabel2.ToolTipText = "Upload";
            this.toolStripLabel2.Click += new System.EventHandler(this.tsbDown_Click);
            // 
            // toolStripButton11
            // 
            this.toolStripButton11.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton11.Image = global::HDL_Buspro_Setup_Tool.Properties.Resources.帮助;
            this.toolStripButton11.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton11.Name = "toolStripButton11";
            this.toolStripButton11.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton11.Text = "Help";
            this.toolStripButton11.Click += new System.EventHandler(this.toolStripButton11_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // tabEIB
            // 
            this.tabEIB.Controls.Add(this.tabLists);
            this.tabEIB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabEIB.Location = new System.Drawing.Point(0, 50);
            this.tabEIB.Name = "tabEIB";
            this.tabEIB.SelectedIndex = 0;
            this.tabEIB.Size = new System.Drawing.Size(815, 453);
            this.tabEIB.TabIndex = 69;
            // 
            // tabLists
            // 
            this.tabLists.Controls.Add(this.tbAddress);
            this.tabLists.Controls.Add(this.lbAddress);
            this.tabLists.Controls.Add(this.btnAutoAdd);
            this.tabLists.Controls.Add(this.txtNum);
            this.tabLists.Controls.Add(this.lbAdd);
            this.tabLists.Controls.Add(this.DgvList);
            this.tabLists.Location = new System.Drawing.Point(4, 23);
            this.tabLists.Name = "tabLists";
            this.tabLists.Padding = new System.Windows.Forms.Padding(3);
            this.tabLists.Size = new System.Drawing.Size(807, 426);
            this.tabLists.TabIndex = 0;
            this.tabLists.Text = "EIB Settings";
            this.tabLists.UseVisualStyleBackColor = true;
            // 
            // btnAutoAdd
            // 
            this.btnAutoAdd.Location = new System.Drawing.Point(517, 18);
            this.btnAutoAdd.Name = "btnAutoAdd";
            this.btnAutoAdd.Size = new System.Drawing.Size(89, 22);
            this.btnAutoAdd.TabIndex = 92;
            this.btnAutoAdd.Text = "+";
            this.btnAutoAdd.UseVisualStyleBackColor = true;
            this.btnAutoAdd.Click += new System.EventHandler(this.btnAutoAdd_Click);
            // 
            // txtNum
            // 
            this.txtNum.Location = new System.Drawing.Point(371, 18);
            this.txtNum.Maximum = new decimal(new int[] {
            4000,
            0,
            0,
            0});
            this.txtNum.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.txtNum.Name = "txtNum";
            this.txtNum.Size = new System.Drawing.Size(120, 22);
            this.txtNum.TabIndex = 90;
            this.txtNum.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lbAdd
            // 
            this.lbAdd.AutoSize = true;
            this.lbAdd.ForeColor = System.Drawing.Color.Blue;
            this.lbAdd.Location = new System.Drawing.Point(271, 22);
            this.lbAdd.Name = "lbAdd";
            this.lbAdd.Size = new System.Drawing.Size(94, 14);
            this.lbAdd.TabIndex = 88;
            this.lbAdd.Text = "Add Commands:";
            // 
            // DgvList
            // 
            this.DgvList.AllowUserToAddRows = false;
            this.DgvList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DgvList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.cl1,
            this.cl2,
            this.clType,
            this.clDevices,
            this.clSub,
            this.clDev,
            this.clParam1,
            this.clParam2,
            this.clParam3,
            this.clFlag});
            this.DgvList.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.DgvList.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.DgvList.Location = new System.Drawing.Point(3, 46);
            this.DgvList.Name = "DgvList";
            this.DgvList.RowHeadersWidth = 10;
            this.DgvList.RowTemplate.Height = 23;
            this.DgvList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.DgvList.Size = new System.Drawing.Size(801, 377);
            this.DgvList.TabIndex = 0;
            this.DgvList.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.DgvList_CellMouseDown);
            this.DgvList.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.DgvList_CellValueChanged);
            this.DgvList.CurrentCellDirtyStateChanged += new System.EventHandler(this.DgvList_CurrentCellDirtyStateChanged);
            this.DgvList.SelectionChanged += new System.EventHandler(this.DgvList_SelectionChanged);
            // 
            // cl1
            // 
            this.cl1.HeaderText = "Index";
            this.cl1.Name = "cl1";
            this.cl1.ReadOnly = true;
            this.cl1.Width = 64;
            // 
            // cl2
            // 
            this.cl2.HeaderText = "EIB Address";
            this.cl2.Name = "cl2";
            // 
            // clType
            // 
            this.clType.HeaderText = "Control Type";
            this.clType.Name = "clType";
            this.clType.ReadOnly = true;
            this.clType.Width = 120;
            // 
            // clDevices
            // 
            this.clDevices.HeaderText = "HDL Device";
            this.clDevices.Name = "clDevices";
            this.clDevices.Width = 5;
            // 
            // clSub
            // 
            this.clSub.HeaderText = "SubNet ID";
            this.clSub.Name = "clSub";
            this.clSub.Width = 88;
            // 
            // clDev
            // 
            this.clDev.HeaderText = "Device ID";
            this.clDev.Name = "clDev";
            this.clDev.Width = 88;
            // 
            // clParam1
            // 
            this.clParam1.HeaderText = "Param 1 ";
            this.clParam1.Name = "clParam1";
            this.clParam1.ReadOnly = true;
            this.clParam1.Width = 80;
            // 
            // clParam2
            // 
            this.clParam2.HeaderText = "Param 2";
            this.clParam2.Name = "clParam2";
            this.clParam2.ReadOnly = true;
            this.clParam2.Width = 80;
            // 
            // clParam3
            // 
            this.clParam3.HeaderText = "Param 3";
            this.clParam3.Name = "clParam3";
            this.clParam3.ReadOnly = true;
            this.clParam3.Width = 80;
            // 
            // clFlag
            // 
            this.clFlag.HeaderText = "HDL<--> EIB";
            this.clFlag.Items.AddRange(new object[] {
            "None",
            "HDL-->EIB",
            "EIB --> HDL"});
            this.clFlag.Name = "clFlag";
            this.clFlag.Width = 80;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsl1,
            this.tsl2,
            this.tsl3,
            this.tsbar,
            this.tsbl4,
            this.tsbHint});
            this.statusStrip1.Location = new System.Drawing.Point(0, 503);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(815, 30);
            this.statusStrip1.TabIndex = 82;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // tsl1
            // 
            this.tsl1.AutoSize = false;
            this.tsl1.Name = "tsl1";
            this.tsl1.Size = new System.Drawing.Size(150, 25);
            this.tsl1.Text = "Current Device:";
            // 
            // tsl2
            // 
            this.tsl2.Name = "tsl2";
            this.tsl2.Size = new System.Drawing.Size(13, 25);
            this.tsl2.Text = "|";
            // 
            // tsl3
            // 
            this.tsl3.AutoSize = false;
            this.tsl3.Name = "tsl3";
            this.tsl3.Size = new System.Drawing.Size(160, 25);
            this.tsl3.Text = "toolStripStatusLabel1";
            // 
            // tsbar
            // 
            this.tsbar.Name = "tsbar";
            this.tsbar.Size = new System.Drawing.Size(100, 24);
            this.tsbar.Visible = false;
            // 
            // tsbl4
            // 
            this.tsbl4.Name = "tsbl4";
            this.tsbl4.Size = new System.Drawing.Size(0, 25);
            // 
            // tsbHint
            // 
            this.tsbHint.Name = "tsbHint";
            this.tsbHint.Size = new System.Drawing.Size(0, 25);
            this.tsbHint.Visible = false;
            // 
            // toolStrip3
            // 
            this.toolStrip3.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton5,
            this.toolStripSeparator2,
            this.cboDevice,
            this.toolStripSeparator7,
            this.tbModel,
            this.toolStripSeparator8,
            this.tbDescription});
            this.toolStrip3.Location = new System.Drawing.Point(0, 25);
            this.toolStrip3.Name = "toolStrip3";
            this.toolStrip3.Size = new System.Drawing.Size(815, 25);
            this.toolStrip3.TabIndex = 94;
            this.toolStrip3.Text = "toolStrip3";
            // 
            // toolStripButton5
            // 
            this.toolStripButton5.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton5.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripButton5.ForeColor = System.Drawing.Color.Blue;
            this.toolStripButton5.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton5.Name = "toolStripButton5";
            this.toolStripButton5.Size = new System.Drawing.Size(51, 22);
            this.toolStripButton5.Text = "Device:";
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
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.ForeColor = System.Drawing.Color.Blue;
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new System.Drawing.Size(6, 25);
            // 
            // tbDescription
            // 
            this.tbDescription.ForeColor = System.Drawing.Color.Blue;
            this.tbDescription.Name = "tbDescription";
            this.tbDescription.Size = new System.Drawing.Size(91, 22);
            this.tbDescription.Text = "toolStripLabel2";
            // 
            // tbAddress
            // 
            this.tbAddress.Location = new System.Drawing.Point(109, 18);
            this.tbAddress.Name = "tbAddress";
            this.tbAddress.Size = new System.Drawing.Size(120, 22);
            this.tbAddress.TabIndex = 94;
            this.tbAddress.Tag = "2";
            // 
            // lbAddress
            // 
            this.lbAddress.AutoSize = true;
            this.lbAddress.Font = new System.Drawing.Font("Arial", 8.25F);
            this.lbAddress.ForeColor = System.Drawing.Color.Blue;
            this.lbAddress.Location = new System.Drawing.Point(13, 22);
            this.lbAddress.Name = "lbAddress";
            this.lbAddress.Size = new System.Drawing.Size(94, 14);
            this.lbAddress.TabIndex = 93;
            this.lbAddress.Text = "Physical Address:";
            // 
            // frmEIB
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(815, 533);
            this.Controls.Add(this.tabEIB);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.toolStrip3);
            this.Controls.Add(this.toolStrip1);
            this.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "frmEIB";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmEIB";
            this.Load += new System.EventHandler(this.frmEIB_Load);
            this.Shown += new System.EventHandler(this.frmEIB_Shown);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.tabEIB.ResumeLayout(false);
            this.tabLists.ResumeLayout(false);
            this.tabLists.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtNum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DgvList)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.toolStrip3.ResumeLayout(false);
            this.toolStrip3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsbDefault;
        private System.Windows.Forms.ToolStripButton tsbSave;
        private System.Windows.Forms.ToolStripButton toolStripButton7;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripButton toolStripButton9;
        private System.Windows.Forms.ToolStripButton toolStripButton10;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripButton toolStripButton11;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.TabControl tabEIB;
        private System.Windows.Forms.TabPage tabLists;
        private System.Windows.Forms.DataGridView DgvList;
        private System.Windows.Forms.Button btnAutoAdd;
        private System.Windows.Forms.NumericUpDown txtNum;
        private System.Windows.Forms.Label lbAdd;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel tsl1;
        private System.Windows.Forms.ToolStripStatusLabel tsl2;
        private System.Windows.Forms.ToolStripStatusLabel tsl3;
        private System.Windows.Forms.ToolStripProgressBar tsbar;
        private System.Windows.Forms.ToolStripStatusLabel tsbl4;
        private System.Windows.Forms.ToolStripStatusLabel tsbHint;
        private System.Windows.Forms.ToolStripButton tsbDown;
        private System.Windows.Forms.DataGridViewTextBoxColumn cl1;
        private System.Windows.Forms.DataGridViewTextBoxColumn cl2;
        private System.Windows.Forms.DataGridViewComboBoxColumn clType;
        private System.Windows.Forms.DataGridViewTextBoxColumn clDevices;
        private System.Windows.Forms.DataGridViewTextBoxColumn clSub;
        private System.Windows.Forms.DataGridViewTextBoxColumn clDev;
        private System.Windows.Forms.DataGridViewTextBoxColumn clParam1;
        private System.Windows.Forms.DataGridViewTextBoxColumn clParam2;
        private System.Windows.Forms.DataGridViewTextBoxColumn clParam3;
        private System.Windows.Forms.DataGridViewComboBoxColumn clFlag;
        private System.Windows.Forms.ToolStripButton toolStripLabel2;
        private System.Windows.Forms.ToolStrip toolStrip3;
        private System.Windows.Forms.ToolStripButton toolStripButton5;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripComboBox cboDevice;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripLabel tbModel;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private System.Windows.Forms.ToolStripLabel tbDescription;
        private System.Windows.Forms.TextBox tbAddress;
        private System.Windows.Forms.Label lbAddress;
    }
}