namespace HDL_Buspro_Setup_Tool
{
    partial class frmCamera
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.lvSensors = new System.Windows.Forms.ListView();
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.cm1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cboDevicePath = new System.Windows.Forms.ComboBox();
            this.lbDevice = new System.Windows.Forms.Label();
            this.btnFind = new System.Windows.Forms.Button();
            this.BtnSaveExit = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.cm1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl1.Location = new System.Drawing.Point(0, 48);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(427, 322);
            this.tabControl1.TabIndex = 102;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.lvSensors);
            this.tabPage1.Location = new System.Drawing.Point(4, 23);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(419, 295);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Camera Available";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // lvSensors
            // 
            this.lvSensors.CheckBoxes = true;
            this.lvSensors.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader2,
            this.columnHeader4,
            this.columnHeader1});
            this.lvSensors.ContextMenuStrip = this.cm1;
            this.lvSensors.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvSensors.FullRowSelect = true;
            this.lvSensors.HideSelection = false;
            this.lvSensors.Location = new System.Drawing.Point(3, 3);
            this.lvSensors.Name = "lvSensors";
            this.lvSensors.Size = new System.Drawing.Size(413, 289);
            this.lvSensors.TabIndex = 102;
            this.lvSensors.UseCompatibleStateImageBehavior = false;
            this.lvSensors.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "ID";
            this.columnHeader2.Width = 48;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Chn";
            this.columnHeader4.Width = 48;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Remark";
            this.columnHeader1.Width = 280;
            // 
            // cm1
            // 
            this.cm1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyToolStripMenuItem});
            this.cm1.Name = "cm1";
            this.cm1.Size = new System.Drawing.Size(138, 26);
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
            this.copyToolStripMenuItem.Text = "Copy";
            this.copyToolStripMenuItem.Click += new System.EventHandler(this.copyToolStripMenuItem_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cboDevicePath);
            this.groupBox2.Controls.Add(this.lbDevice);
            this.groupBox2.Controls.Add(this.btnFind);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(432, 47);
            this.groupBox2.TabIndex = 103;
            this.groupBox2.TabStop = false;
            // 
            // cboDevicePath
            // 
            this.cboDevicePath.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDevicePath.FormattingEnabled = true;
            this.cboDevicePath.Location = new System.Drawing.Point(70, 11);
            this.cboDevicePath.Name = "cboDevicePath";
            this.cboDevicePath.Size = new System.Drawing.Size(212, 22);
            this.cboDevicePath.TabIndex = 10;
            // 
            // lbDevice
            // 
            this.lbDevice.AutoSize = true;
            this.lbDevice.Location = new System.Drawing.Point(12, 15);
            this.lbDevice.Name = "lbDevice";
            this.lbDevice.Size = new System.Drawing.Size(46, 14);
            this.lbDevice.TabIndex = 9;
            this.lbDevice.Text = "Device:";
            // 
            // btnFind
            // 
            this.btnFind.Image = global::HDL_Buspro_Setup_Tool.Properties.Resources.Refresh;
            this.btnFind.Location = new System.Drawing.Point(294, 12);
            this.btnFind.Name = "btnFind";
            this.btnFind.Size = new System.Drawing.Size(126, 23);
            this.btnFind.TabIndex = 1;
            this.btnFind.UseVisualStyleBackColor = true;
            // 
            // BtnSaveExit
            // 
            this.BtnSaveExit.Image = global::HDL_Buspro_Setup_Tool.Properties.Resources.Save;
            this.BtnSaveExit.Location = new System.Drawing.Point(294, 376);
            this.BtnSaveExit.Name = "BtnSaveExit";
            this.BtnSaveExit.Size = new System.Drawing.Size(129, 24);
            this.BtnSaveExit.TabIndex = 104;
            this.BtnSaveExit.Text = "Save && Close";
            this.BtnSaveExit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.BtnSaveExit.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            this.btnSave.Image = global::HDL_Buspro_Setup_Tool.Properties.Resources.Save;
            this.btnSave.Location = new System.Drawing.Point(163, 376);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(119, 24);
            this.btnSave.TabIndex = 105;
            this.btnSave.UseVisualStyleBackColor = true;
            // 
            // frmCamera
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(432, 407);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.BtnSaveExit);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.tabControl1);
            this.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "frmCamera";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Current cameras avaible in system";
            this.Load += new System.EventHandler(this.frmTSensor_Load);
            this.Shown += new System.EventHandler(this.frmTSensor_Shown);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.cm1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.ContextMenuStrip cm1;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.ListView lvSensors;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox cboDevicePath;
        private System.Windows.Forms.Label lbDevice;
        private System.Windows.Forms.Button btnFind;
        private System.Windows.Forms.Button BtnSaveExit;
        private System.Windows.Forms.Button btnSave;

    }
}