namespace HDL_Buspro_Setup_Tool
{
    partial class frmHvacRelay
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
            this.rbD = new System.Windows.Forms.RadioButton();
            this.rbC = new System.Windows.Forms.RadioButton();
            this.DgMode = new System.Windows.Forms.DataGridView();
            this.cm1 = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.cm2 = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.cm3 = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.clDelay = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnOK = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cboDevice = new System.Windows.Forms.ComboBox();
            this.lbDevice = new System.Windows.Forms.Label();
            this.btnFind = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.DgMode)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // rbD
            // 
            this.rbD.AutoSize = true;
            this.rbD.Checked = true;
            this.rbD.Location = new System.Drawing.Point(107, 55);
            this.rbD.Name = "rbD";
            this.rbD.Size = new System.Drawing.Size(97, 18);
            this.rbD.TabIndex = 74;
            this.rbD.TabStop = true;
            this.rbD.Text = "Simple Mode";
            this.rbD.UseVisualStyleBackColor = true;
            this.rbD.CheckedChanged += new System.EventHandler(this.rbD_CheckedChanged);
            // 
            // rbC
            // 
            this.rbC.AutoSize = true;
            this.rbC.Location = new System.Drawing.Point(294, 57);
            this.rbC.Name = "rbC";
            this.rbC.Size = new System.Drawing.Size(104, 18);
            this.rbC.TabIndex = 75;
            this.rbC.Text = "Advance Mode";
            this.rbC.UseVisualStyleBackColor = true;
            // 
            // DgMode
            // 
            this.DgMode.AllowUserToAddRows = false;
            this.DgMode.AllowUserToDeleteRows = false;
            this.DgMode.BackgroundColor = System.Drawing.SystemColors.Control;
            this.DgMode.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DgMode.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.cm1,
            this.cm2,
            this.cm3,
            this.clDelay});
            this.DgMode.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.DgMode.Location = new System.Drawing.Point(15, 81);
            this.DgMode.Name = "DgMode";
            this.DgMode.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.DgMode.RowHeadersWidth = 145;
            this.DgMode.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.DgMode.Size = new System.Drawing.Size(468, 240);
            this.DgMode.TabIndex = 83;
            this.DgMode.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.DgMode_CellBeginEdit);
            this.DgMode.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.DgMode_CellEndEdit);
            this.DgMode.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.DgMode_CellValueChanged);
            this.DgMode.CurrentCellDirtyStateChanged += new System.EventHandler(this.DgMode_CurrentCellDirtyStateChanged);
            // 
            // cm1
            // 
            this.cm1.HeaderText = "Mode I";
            this.cm1.Items.AddRange(new object[] {
            "OFF",
            "ON",
            "N/A"});
            this.cm1.Name = "cm1";
            this.cm1.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.cm1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.cm1.Width = 80;
            // 
            // cm2
            // 
            this.cm2.HeaderText = "Mode II";
            this.cm2.Items.AddRange(new object[] {
            "OFF",
            "ON",
            "N/A"});
            this.cm2.Name = "cm2";
            this.cm2.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.cm2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.cm2.Width = 80;
            // 
            // cm3
            // 
            this.cm3.HeaderText = "Mode III";
            this.cm3.Items.AddRange(new object[] {
            "OFF",
            "ON",
            "N/A"});
            this.cm3.Name = "cm3";
            this.cm3.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.cm3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.cm3.Width = 80;
            // 
            // clDelay
            // 
            this.clDelay.HeaderText = "Delay";
            this.clDelay.Name = "clDelay";
            this.clDelay.Width = 80;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(399, 327);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(84, 26);
            this.btnOK.TabIndex = 84;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cboDevice);
            this.groupBox2.Controls.Add(this.lbDevice);
            this.groupBox2.Controls.Add(this.btnFind);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(499, 40);
            this.groupBox2.TabIndex = 104;
            this.groupBox2.TabStop = false;
            // 
            // cboDevice
            // 
            this.cboDevice.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDevice.FormattingEnabled = true;
            this.cboDevice.Location = new System.Drawing.Point(107, 11);
            this.cboDevice.Name = "cboDevice";
            this.cboDevice.Size = new System.Drawing.Size(286, 22);
            this.cboDevice.TabIndex = 10;
            this.cboDevice.SelectedIndexChanged += new System.EventHandler(this.cboDevice_SelectedIndexChanged);
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
            this.btnFind.Location = new System.Drawing.Point(399, 10);
            this.btnFind.Name = "btnFind";
            this.btnFind.Size = new System.Drawing.Size(84, 23);
            this.btnFind.TabIndex = 1;
            this.btnFind.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 57);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 14);
            this.label1.TabIndex = 105;
            this.label1.Text = "Work Mode:";
            // 
            // frmHvacRelay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(499, 365);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.DgMode);
            this.Controls.Add(this.rbC);
            this.Controls.Add(this.rbD);
            this.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "frmHvacRelay";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "HVAC Work Mode";
            this.Load += new System.EventHandler(this.frmHvI_Load);
            this.Shown += new System.EventHandler(this.frmHvacRelay_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.DgMode)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton rbD;
        private System.Windows.Forms.RadioButton rbC;
        private System.Windows.Forms.DataGridView DgMode;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.DataGridViewComboBoxColumn cm1;
        private System.Windows.Forms.DataGridViewComboBoxColumn cm2;
        private System.Windows.Forms.DataGridViewComboBoxColumn cm3;
        private System.Windows.Forms.DataGridViewTextBoxColumn clDelay;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox cboDevice;
        private System.Windows.Forms.Label lbDevice;
        private System.Windows.Forms.Button btnFind;
        private System.Windows.Forms.Label label1;
    }
}