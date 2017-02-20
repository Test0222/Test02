namespace HDL_Buspro_Setup_Tool
{
    partial class frmTSensor
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.lvSensors = new System.Windows.Forms.ListView();
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lbHint = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cboDevice = new System.Windows.Forms.ComboBox();
            this.lbDevice = new System.Windows.Forms.Label();
            this.btnFind = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.BtnSaveExit = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tabControl1.Location = new System.Drawing.Point(0, 47);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(490, 378);
            this.tabControl1.TabIndex = 102;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.lvSensors);
            this.tabPage1.Controls.Add(this.lbHint);
            this.tabPage1.Location = new System.Drawing.Point(4, 23);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(482, 351);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Sensors Avaiable";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // lvSensors
            // 
            this.lvSensors.CheckBoxes = true;
            this.lvSensors.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader1});
            this.lvSensors.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvSensors.FullRowSelect = true;
            this.lvSensors.HideSelection = false;
            this.lvSensors.Location = new System.Drawing.Point(3, 35);
            this.lvSensors.Name = "lvSensors";
            this.lvSensors.Size = new System.Drawing.Size(476, 313);
            this.lvSensors.TabIndex = 102;
            this.lvSensors.UseCompatibleStateImageBehavior = false;
            this.lvSensors.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "ID";
            this.columnHeader2.Width = 48;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Name";
            this.columnHeader3.Width = 140;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Chn";
            this.columnHeader4.Width = 48;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Remark";
            this.columnHeader1.Width = 140;
            // 
            // lbHint
            // 
            this.lbHint.Dock = System.Windows.Forms.DockStyle.Top;
            this.lbHint.ForeColor = System.Drawing.Color.Blue;
            this.lbHint.Location = new System.Drawing.Point(3, 3);
            this.lbHint.Name = "lbHint";
            this.lbHint.Size = new System.Drawing.Size(476, 32);
            this.lbHint.TabIndex = 103;
            this.lbHint.Text = "label1";
            this.lbHint.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cboDevice);
            this.groupBox2.Controls.Add(this.lbDevice);
            this.groupBox2.Controls.Add(this.btnFind);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(490, 47);
            this.groupBox2.TabIndex = 105;
            this.groupBox2.TabStop = false;
            // 
            // cboDevice
            // 
            this.cboDevice.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDevice.FormattingEnabled = true;
            this.cboDevice.Location = new System.Drawing.Point(107, 13);
            this.cboDevice.Name = "cboDevice";
            this.cboDevice.Size = new System.Drawing.Size(286, 22);
            this.cboDevice.TabIndex = 10;
            this.cboDevice.SelectedIndexChanged += new System.EventHandler(this.cboDevice_SelectedIndexChanged);
            // 
            // lbDevice
            // 
            this.lbDevice.AutoSize = true;
            this.lbDevice.Location = new System.Drawing.Point(12, 17);
            this.lbDevice.Name = "lbDevice";
            this.lbDevice.Size = new System.Drawing.Size(46, 14);
            this.lbDevice.TabIndex = 9;
            this.lbDevice.Text = "Device:";
            // 
            // btnFind
            // 
            this.btnFind.Image = global::HDL_Buspro_Setup_Tool.Properties.Resources.Refresh;
            this.btnFind.Location = new System.Drawing.Point(399, 12);
            this.btnFind.Name = "btnFind";
            this.btnFind.Size = new System.Drawing.Size(84, 27);
            this.btnFind.TabIndex = 1;
            this.btnFind.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            this.btnSave.Image = global::HDL_Buspro_Setup_Tool.Properties.Resources.Save;
            this.btnSave.Location = new System.Drawing.Point(203, 432);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(119, 28);
            this.btnSave.TabIndex = 107;
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // BtnSaveExit
            // 
            this.BtnSaveExit.Image = global::HDL_Buspro_Setup_Tool.Properties.Resources.Save;
            this.BtnSaveExit.Location = new System.Drawing.Point(354, 432);
            this.BtnSaveExit.Name = "BtnSaveExit";
            this.BtnSaveExit.Size = new System.Drawing.Size(129, 28);
            this.BtnSaveExit.TabIndex = 106;
            this.BtnSaveExit.Text = "Save && Close";
            this.BtnSaveExit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.BtnSaveExit.UseVisualStyleBackColor = true;
            this.BtnSaveExit.Click += new System.EventHandler(this.BtnSaveExit_Click);
            // 
            // frmTSensor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(490, 474);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.BtnSaveExit);
            this.Controls.Add(this.groupBox2);
            this.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "frmTSensor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Current temperature sensors avaible in system";
            this.Load += new System.EventHandler(this.frmTSensor_Load);
            this.Shown += new System.EventHandler(this.frmTSensor_Shown);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.ListView lvSensors;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox cboDevice;
        private System.Windows.Forms.Label lbDevice;
        private System.Windows.Forms.Button btnFind;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button BtnSaveExit;
        private System.Windows.Forms.Label lbHint;

    }
}