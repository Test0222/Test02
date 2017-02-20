namespace HDL_Buspro_Setup_Tool
{
    partial class frmHAI
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmHAI));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tbDefault = new System.Windows.Forms.ToolStripButton();
            this.tbSave = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton7 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton9 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton10 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbDown = new System.Windows.Forms.ToolStripButton();
            this.tbUpload = new System.Windows.Forms.ToolStripLabel();
            this.toolStripButton11 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsl3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsbar = new System.Windows.Forms.ToolStripProgressBar();
            this.tsbl4 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsbHint = new System.Windows.Forms.ToolStripStatusLabel();
            this.tabHAI = new System.Windows.Forms.TabControl();
            this.tabBasic = new System.Windows.Forms.TabPage();
            this.gbButtons = new System.Windows.Forms.GroupBox();
            this.tvButtons = new System.Windows.Forms.TreeView();
            this.CmsCopy = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.addCommandsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.txtNum2 = new System.Windows.Forms.TextBox();
            this.lbButtons = new System.Windows.Forms.Label();
            this.btnAdd2 = new System.Windows.Forms.Button();
            this.gbScene = new System.Windows.Forms.GroupBox();
            this.tvScenes = new System.Windows.Forms.TreeView();
            this.txtNum1 = new System.Windows.Forms.TextBox();
            this.lbSces = new System.Windows.Forms.Label();
            this.btnAdd1 = new System.Windows.Forms.Button();
            this.gbUnit = new System.Windows.Forms.GroupBox();
            this.tvUnit = new System.Windows.Forms.TreeView();
            this.txtNum = new System.Windows.Forms.TextBox();
            this.lbHAI = new System.Windows.Forms.Label();
            this.btnAdd = new System.Windows.Forms.Button();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.toolStrip3 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.cboDevice = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.tbModel = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.tbDescription = new System.Windows.Forms.ToolStripLabel();
            this.toolStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.tabHAI.SuspendLayout();
            this.tabBasic.SuspendLayout();
            this.gbButtons.SuspendLayout();
            this.CmsCopy.SuspendLayout();
            this.gbScene.SuspendLayout();
            this.gbUnit.SuspendLayout();
            this.toolStrip3.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Font = new System.Drawing.Font("Arial", 8.25F);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tbDefault,
            this.tbSave,
            this.toolStripButton7,
            this.toolStripSeparator4,
            this.toolStripButton9,
            this.toolStripButton10,
            this.toolStripSeparator5,
            this.tsbDown,
            this.tbUpload,
            this.toolStripButton11,
            this.toolStripSeparator1});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(760, 25);
            this.toolStrip1.TabIndex = 84;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tbDefault
            // 
            this.tbDefault.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbDefault.Image = global::HDL_Buspro_Setup_Tool.Properties.Resources.新建;
            this.tbDefault.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbDefault.Name = "tbDefault";
            this.tbDefault.Size = new System.Drawing.Size(23, 22);
            this.tbDefault.Text = "&New";
            this.tbDefault.ToolTipText = "Load Default";
            this.tbDefault.Click += new System.EventHandler(this.tbDefault_Click);
            // 
            // tbSave
            // 
            this.tbSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbSave.Image = global::HDL_Buspro_Setup_Tool.Properties.Resources.保存;
            this.tbSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbSave.Name = "tbSave";
            this.tbSave.Size = new System.Drawing.Size(23, 22);
            this.tbSave.Text = "&Save";
            this.tbSave.ToolTipText = "Save ";
            this.tbSave.Click += new System.EventHandler(this.tbSave_Click);
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
            // tbUpload
            // 
            this.tbUpload.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbUpload.Image = global::HDL_Buspro_Setup_Tool.Properties.Resources.上传;
            this.tbUpload.Name = "tbUpload";
            this.tbUpload.Size = new System.Drawing.Size(16, 22);
            this.tbUpload.Tag = "1";
            this.tbUpload.Text = "toolStripLabel1";
            this.tbUpload.ToolTipText = "Upload";
            this.tbUpload.Click += new System.EventHandler(this.tsbDown_Click);
            // 
            // toolStripButton11
            // 
            this.toolStripButton11.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton11.Image = global::HDL_Buspro_Setup_Tool.Properties.Resources.帮助;
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
            // statusStrip1
            // 
            this.statusStrip1.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripStatusLabel2,
            this.tsl3,
            this.tsbar,
            this.tsbl4,
            this.tsbHint});
            this.statusStrip1.Location = new System.Drawing.Point(0, 475);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(760, 30);
            this.statusStrip1.TabIndex = 85;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.AutoSize = false;
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(150, 25);
            this.toolStripStatusLabel1.Text = "Current Device:";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(13, 25);
            this.toolStripStatusLabel2.Text = "|";
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
            // 
            // tabHAI
            // 
            this.tabHAI.Controls.Add(this.tabBasic);
            this.tabHAI.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabHAI.Location = new System.Drawing.Point(0, 50);
            this.tabHAI.Name = "tabHAI";
            this.tabHAI.SelectedIndex = 0;
            this.tabHAI.Size = new System.Drawing.Size(760, 425);
            this.tabHAI.TabIndex = 88;
            // 
            // tabBasic
            // 
            this.tabBasic.Controls.Add(this.gbButtons);
            this.tabBasic.Controls.Add(this.gbScene);
            this.tabBasic.Controls.Add(this.gbUnit);
            this.tabBasic.Location = new System.Drawing.Point(4, 23);
            this.tabBasic.Name = "tabBasic";
            this.tabBasic.Padding = new System.Windows.Forms.Padding(3);
            this.tabBasic.Size = new System.Drawing.Size(752, 398);
            this.tabBasic.TabIndex = 0;
            this.tabBasic.Text = "Basic Information";
            this.tabBasic.UseVisualStyleBackColor = true;
            // 
            // gbButtons
            // 
            this.gbButtons.Controls.Add(this.tvButtons);
            this.gbButtons.Controls.Add(this.txtNum2);
            this.gbButtons.Controls.Add(this.lbButtons);
            this.gbButtons.Controls.Add(this.btnAdd2);
            this.gbButtons.Dock = System.Windows.Forms.DockStyle.Left;
            this.gbButtons.Location = new System.Drawing.Point(495, 3);
            this.gbButtons.Name = "gbButtons";
            this.gbButtons.Size = new System.Drawing.Size(246, 392);
            this.gbButtons.TabIndex = 90;
            this.gbButtons.TabStop = false;
            this.gbButtons.Text = "Buttons<->HDL UV Switches";
            // 
            // tvButtons
            // 
            this.tvButtons.ContextMenuStrip = this.CmsCopy;
            this.tvButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tvButtons.Location = new System.Drawing.Point(3, 50);
            this.tvButtons.Name = "tvButtons";
            this.tvButtons.Size = new System.Drawing.Size(240, 339);
            this.tvButtons.TabIndex = 90;
            this.tvButtons.Tag = "2";
            this.tvButtons.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tvUnit_MouseDown);
            // 
            // CmsCopy
            // 
            this.CmsCopy.Font = new System.Drawing.Font("Arial", 8.25F);
            this.CmsCopy.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparator2,
            this.toolStripMenuItem1,
            this.addCommandsToolStripMenuItem});
            this.CmsCopy.Name = "CmsCopy";
            this.CmsCopy.Size = new System.Drawing.Size(150, 38);
            this.CmsCopy.Opening += new System.ComponentModel.CancelEventHandler(this.CmsCopy_Opening);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(146, 6);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(146, 6);
            // 
            // addCommandsToolStripMenuItem
            // 
            this.addCommandsToolStripMenuItem.Name = "addCommandsToolStripMenuItem";
            this.addCommandsToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.addCommandsToolStripMenuItem.Text = "Add commands";
            this.addCommandsToolStripMenuItem.Click += new System.EventHandler(this.addCommandsToolStripMenuItem_Click);
            // 
            // txtNum2
            // 
            this.txtNum2.Location = new System.Drawing.Point(122, 22);
            this.txtNum2.Name = "txtNum2";
            this.txtNum2.Size = new System.Drawing.Size(79, 22);
            this.txtNum2.TabIndex = 89;
            this.txtNum2.Text = "1";
            // 
            // lbButtons
            // 
            this.lbButtons.AutoSize = true;
            this.lbButtons.ForeColor = System.Drawing.Color.Blue;
            this.lbButtons.Location = new System.Drawing.Point(21, 25);
            this.lbButtons.Name = "lbButtons";
            this.lbButtons.Size = new System.Drawing.Size(77, 14);
            this.lbButtons.TabIndex = 88;
            this.lbButtons.Text = "HAI Buttons :";
            // 
            // btnAdd2
            // 
            this.btnAdd2.Location = new System.Drawing.Point(214, 22);
            this.btnAdd2.Name = "btnAdd2";
            this.btnAdd2.Size = new System.Drawing.Size(24, 26);
            this.btnAdd2.TabIndex = 87;
            this.btnAdd2.Tag = "3";
            this.btnAdd2.Text = "+";
            this.btnAdd2.UseVisualStyleBackColor = true;
            this.btnAdd2.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // gbScene
            // 
            this.gbScene.Controls.Add(this.tvScenes);
            this.gbScene.Controls.Add(this.txtNum1);
            this.gbScene.Controls.Add(this.lbSces);
            this.gbScene.Controls.Add(this.btnAdd1);
            this.gbScene.Dock = System.Windows.Forms.DockStyle.Left;
            this.gbScene.Location = new System.Drawing.Point(249, 3);
            this.gbScene.Name = "gbScene";
            this.gbScene.Size = new System.Drawing.Size(246, 392);
            this.gbScene.TabIndex = 89;
            this.gbScene.TabStop = false;
            this.gbScene.Text = "Scenes<->HDL Scenes";
            // 
            // tvScenes
            // 
            this.tvScenes.ContextMenuStrip = this.CmsCopy;
            this.tvScenes.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tvScenes.Location = new System.Drawing.Point(3, 54);
            this.tvScenes.Name = "tvScenes";
            this.tvScenes.Size = new System.Drawing.Size(240, 335);
            this.tvScenes.TabIndex = 90;
            this.tvScenes.Tag = "1";
            this.tvScenes.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tvUnit_MouseDown);
            // 
            // txtNum1
            // 
            this.txtNum1.Location = new System.Drawing.Point(122, 22);
            this.txtNum1.Name = "txtNum1";
            this.txtNum1.Size = new System.Drawing.Size(79, 22);
            this.txtNum1.TabIndex = 89;
            this.txtNum1.Text = "1";
            // 
            // lbSces
            // 
            this.lbSces.AutoSize = true;
            this.lbSces.ForeColor = System.Drawing.Color.Blue;
            this.lbSces.Location = new System.Drawing.Point(21, 25);
            this.lbSces.Name = "lbSces";
            this.lbSces.Size = new System.Drawing.Size(73, 14);
            this.lbSces.TabIndex = 88;
            this.lbSces.Text = "HAI Scenes :";
            // 
            // btnAdd1
            // 
            this.btnAdd1.Location = new System.Drawing.Point(214, 22);
            this.btnAdd1.Name = "btnAdd1";
            this.btnAdd1.Size = new System.Drawing.Size(24, 26);
            this.btnAdd1.TabIndex = 87;
            this.btnAdd1.Tag = "2";
            this.btnAdd1.Text = "+";
            this.btnAdd1.UseVisualStyleBackColor = true;
            this.btnAdd1.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // gbUnit
            // 
            this.gbUnit.Controls.Add(this.tvUnit);
            this.gbUnit.Controls.Add(this.txtNum);
            this.gbUnit.Controls.Add(this.lbHAI);
            this.gbUnit.Controls.Add(this.btnAdd);
            this.gbUnit.Dock = System.Windows.Forms.DockStyle.Left;
            this.gbUnit.Location = new System.Drawing.Point(3, 3);
            this.gbUnit.Name = "gbUnit";
            this.gbUnit.Size = new System.Drawing.Size(246, 392);
            this.gbUnit.TabIndex = 88;
            this.gbUnit.TabStop = false;
            this.gbUnit.Text = "Units<->HDL Lights";
            // 
            // tvUnit
            // 
            this.tvUnit.ContextMenuStrip = this.CmsCopy;
            this.tvUnit.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tvUnit.Location = new System.Drawing.Point(3, 50);
            this.tvUnit.Name = "tvUnit";
            this.tvUnit.Size = new System.Drawing.Size(240, 339);
            this.tvUnit.TabIndex = 90;
            this.tvUnit.Tag = "0";
            this.tvUnit.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tvUnit_MouseDown);
            // 
            // txtNum
            // 
            this.txtNum.Location = new System.Drawing.Point(114, 22);
            this.txtNum.Name = "txtNum";
            this.txtNum.Size = new System.Drawing.Size(79, 22);
            this.txtNum.TabIndex = 89;
            this.txtNum.Text = "1";
            // 
            // lbHAI
            // 
            this.lbHAI.AutoSize = true;
            this.lbHAI.ForeColor = System.Drawing.Color.Blue;
            this.lbHAI.Location = new System.Drawing.Point(21, 25);
            this.lbHAI.Name = "lbHAI";
            this.lbHAI.Size = new System.Drawing.Size(64, 14);
            this.lbHAI.TabIndex = 88;
            this.lbHAI.Text = "HAI Units :";
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(214, 22);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(24, 26);
            this.btnAdd.TabIndex = 87;
            this.btnAdd.Tag = "1";
            this.btnAdd.Text = "+";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // treeView1
            // 
            this.treeView1.Location = new System.Drawing.Point(334, 37);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(283, 328);
            this.treeView1.TabIndex = 73;
            // 
            // toolStrip3
            // 
            this.toolStrip3.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1,
            this.toolStripSeparator3,
            this.cboDevice,
            this.toolStripSeparator6,
            this.tbModel,
            this.toolStripSeparator7,
            this.tbDescription});
            this.toolStrip3.Location = new System.Drawing.Point(0, 25);
            this.toolStrip3.Name = "toolStrip3";
            this.toolStrip3.Size = new System.Drawing.Size(760, 25);
            this.toolStrip3.TabIndex = 92;
            this.toolStrip3.Text = "toolStrip3";
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton1.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripButton1.ForeColor = System.Drawing.Color.Blue;
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(51, 22);
            this.toolStripButton1.Text = "Device:";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.ForeColor = System.Drawing.Color.Blue;
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // cboDevice
            // 
            this.cboDevice.AutoSize = false;
            this.cboDevice.ForeColor = System.Drawing.Color.Blue;
            this.cboDevice.Name = "cboDevice";
            this.cboDevice.Size = new System.Drawing.Size(300, 25);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.ForeColor = System.Drawing.Color.Blue;
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(6, 25);
            // 
            // tbModel
            // 
            this.tbModel.ForeColor = System.Drawing.Color.Blue;
            this.tbModel.Name = "tbModel";
            this.tbModel.Size = new System.Drawing.Size(90, 22);
            this.tbModel.Text = "toolStripLabel1";
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.ForeColor = System.Drawing.Color.Blue;
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(6, 25);
            // 
            // tbDescription
            // 
            this.tbDescription.ForeColor = System.Drawing.Color.Blue;
            this.tbDescription.Name = "tbDescription";
            this.tbDescription.Size = new System.Drawing.Size(90, 22);
            this.tbDescription.Text = "toolStripLabel2";
            // 
            // frmHAI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(760, 505);
            this.Controls.Add(this.tabHAI);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.toolStrip3);
            this.Controls.Add(this.toolStrip1);
            this.Font = new System.Drawing.Font("Calibri", 9F);
            this.Name = "frmHAI";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.frmHAI_Load);
            this.Shown += new System.EventHandler(this.frmHAI_Shown);
            this.SizeChanged += new System.EventHandler(this.frmHAI_SizeChanged);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.tabHAI.ResumeLayout(false);
            this.tabBasic.ResumeLayout(false);
            this.gbButtons.ResumeLayout(false);
            this.gbButtons.PerformLayout();
            this.CmsCopy.ResumeLayout(false);
            this.gbScene.ResumeLayout(false);
            this.gbScene.PerformLayout();
            this.gbUnit.ResumeLayout(false);
            this.gbUnit.PerformLayout();
            this.toolStrip3.ResumeLayout(false);
            this.toolStrip3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tbDefault;
        private System.Windows.Forms.ToolStripButton tbSave;
        private System.Windows.Forms.ToolStripButton toolStripButton7;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripButton toolStripButton9;
        private System.Windows.Forms.ToolStripButton toolStripButton10;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripButton tsbDown;
        private System.Windows.Forms.ToolStripLabel tbUpload;
        private System.Windows.Forms.ToolStripButton toolStripButton11;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripStatusLabel tsl3;
        private System.Windows.Forms.ToolStripProgressBar tsbar;
        private System.Windows.Forms.ToolStripStatusLabel tsbl4;
        private System.Windows.Forms.ToolStripStatusLabel tsbHint;
        private System.Windows.Forms.TabControl tabHAI;
        private System.Windows.Forms.TabPage tabBasic;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.GroupBox gbUnit;
        private System.Windows.Forms.TreeView tvUnit;
        private System.Windows.Forms.TextBox txtNum;
        private System.Windows.Forms.Label lbHAI;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.GroupBox gbScene;
        private System.Windows.Forms.TreeView tvScenes;
        private System.Windows.Forms.TextBox txtNum1;
        private System.Windows.Forms.Label lbSces;
        private System.Windows.Forms.Button btnAdd1;
        private System.Windows.Forms.GroupBox gbButtons;
        private System.Windows.Forms.TreeView tvButtons;
        private System.Windows.Forms.TextBox txtNum2;
        private System.Windows.Forms.Label lbButtons;
        private System.Windows.Forms.Button btnAdd2;
        private System.Windows.Forms.ContextMenuStrip CmsCopy;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem addCommandsToolStripMenuItem;
        private System.Windows.Forms.ToolStrip toolStrip3;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripComboBox cboDevice;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripLabel tbModel;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripLabel tbDescription;
    }
}