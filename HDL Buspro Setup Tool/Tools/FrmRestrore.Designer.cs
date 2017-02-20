namespace HDL_Buspro_Setup_Tool
{
    partial class FrmRestrore
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmRestrore));
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnModify = new System.Windows.Forms.Button();
            this.lbPathValue = new System.Windows.Forms.Label();
            this.lbPath = new System.Windows.Forms.Label();
            this.dgvListA = new System.Windows.Forms.DataGridView();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnUpdateA = new System.Windows.Forms.Button();
            this.chbOption = new System.Windows.Forms.CheckBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.cl0 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.cl1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cl2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cl3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cl4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cl5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cl6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cl7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cl8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvListA)).BeginInit();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnModify);
            this.panel1.Controls.Add(this.lbPathValue);
            this.panel1.Controls.Add(this.lbPath);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(901, 46);
            this.panel1.TabIndex = 0;
            // 
            // btnModify
            // 
            this.btnModify.Location = new System.Drawing.Point(748, 10);
            this.btnModify.Name = "btnModify";
            this.btnModify.Size = new System.Drawing.Size(150, 28);
            this.btnModify.TabIndex = 2;
            this.btnModify.Text = "Modify File Path";
            this.btnModify.UseVisualStyleBackColor = true;
            this.btnModify.Click += new System.EventHandler(this.btnModify_Click);
            // 
            // lbPathValue
            // 
            this.lbPathValue.AutoSize = true;
            this.lbPathValue.Location = new System.Drawing.Point(148, 16);
            this.lbPathValue.Name = "lbPathValue";
            this.lbPathValue.Size = new System.Drawing.Size(37, 14);
            this.lbPathValue.TabIndex = 1;
            this.lbPathValue.Text = "          ";
            // 
            // lbPath
            // 
            this.lbPath.AutoSize = true;
            this.lbPath.Location = new System.Drawing.Point(12, 16);
            this.lbPath.Name = "lbPath";
            this.lbPath.Size = new System.Drawing.Size(100, 14);
            this.lbPath.TabIndex = 0;
            this.lbPath.Text = "Current File Path:";
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
            this.cl0,
            this.cl1,
            this.cl2,
            this.cl3,
            this.cl4,
            this.cl5,
            this.cl6,
            this.cl7,
            this.cl8});
            this.dgvListA.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvListA.EnableHeadersVisualStyles = false;
            this.dgvListA.Location = new System.Drawing.Point(0, 0);
            this.dgvListA.MultiSelect = false;
            this.dgvListA.Name = "dgvListA";
            this.dgvListA.RowHeadersVisible = false;
            this.dgvListA.RowHeadersWidth = 10;
            this.dgvListA.RowTemplate.Height = 23;
            this.dgvListA.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvListA.Size = new System.Drawing.Size(901, 310);
            this.dgvListA.TabIndex = 140;
            this.dgvListA.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvListA_CellClick);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnDelete);
            this.panel2.Controls.Add(this.btnUpdateA);
            this.panel2.Controls.Add(this.chbOption);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 356);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(901, 39);
            this.panel2.TabIndex = 1;
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(167, 7);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(150, 28);
            this.btnDelete.TabIndex = 145;
            this.btnDelete.Text = "Delete Selected Devices";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnUpdateA
            // 
            this.btnUpdateA.Location = new System.Drawing.Point(748, 7);
            this.btnUpdateA.Name = "btnUpdateA";
            this.btnUpdateA.Size = new System.Drawing.Size(150, 28);
            this.btnUpdateA.TabIndex = 144;
            this.btnUpdateA.Text = "Restore Selected Devices";
            this.btnUpdateA.UseVisualStyleBackColor = true;
            this.btnUpdateA.Click += new System.EventHandler(this.btnUpdateA_Click);
            // 
            // chbOption
            // 
            this.chbOption.AutoSize = true;
            this.chbOption.Location = new System.Drawing.Point(21, 12);
            this.chbOption.Name = "chbOption";
            this.chbOption.Size = new System.Drawing.Size(114, 18);
            this.chbOption.TabIndex = 143;
            this.chbOption.Text = "Select All/ None";
            this.chbOption.UseVisualStyleBackColor = true;
            this.chbOption.CheckedChanged += new System.EventHandler(this.chbOption_CheckedChanged);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.dgvListA);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 46);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(901, 310);
            this.panel3.TabIndex = 2;
            // 
            // cl0
            // 
            this.cl0.HeaderText = "";
            this.cl0.Name = "cl0";
            this.cl0.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.cl0.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.cl0.Width = 32;
            // 
            // cl1
            // 
            this.cl1.HeaderText = "Subnet ID";
            this.cl1.Name = "cl1";
            this.cl1.Width = 85;
            // 
            // cl2
            // 
            this.cl2.HeaderText = "Device ID";
            this.cl2.Name = "cl2";
            this.cl2.Width = 85;
            // 
            // cl3
            // 
            this.cl3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.cl3.HeaderText = "Name";
            this.cl3.Name = "cl3";
            this.cl3.ReadOnly = true;
            // 
            // cl4
            // 
            this.cl4.HeaderText = "Description";
            this.cl4.Name = "cl4";
            this.cl4.ReadOnly = true;
            this.cl4.Width = 200;
            // 
            // cl5
            // 
            this.cl5.HeaderText = "Model";
            this.cl5.Name = "cl5";
            this.cl5.ReadOnly = true;
            this.cl5.Width = 150;
            // 
            // cl6
            // 
            this.cl6.HeaderText = "Restrore status";
            this.cl6.Name = "cl6";
            this.cl6.ReadOnly = true;
            this.cl6.Width = 150;
            // 
            // cl7
            // 
            this.cl7.HeaderText = "DIndex";
            this.cl7.Name = "cl7";
            // 
            // cl8
            // 
            this.cl8.HeaderText = "DeviceType";
            this.cl8.Name = "cl8";
            this.cl8.Visible = false;
            // 
            // FrmRestrore
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(901, 395);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmRestrore";
            this.Text = "Restrore";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmRestrore_FormClosing);
            this.Load += new System.EventHandler(this.FrmRestrore_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvListA)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.DataGridView dgvListA;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnUpdateA;
        private System.Windows.Forms.CheckBox chbOption;
        private System.Windows.Forms.Button btnModify;
        private System.Windows.Forms.Label lbPathValue;
        private System.Windows.Forms.Label lbPath;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.DataGridViewCheckBoxColumn cl0;
        private System.Windows.Forms.DataGridViewTextBoxColumn cl1;
        private System.Windows.Forms.DataGridViewTextBoxColumn cl2;
        private System.Windows.Forms.DataGridViewTextBoxColumn cl3;
        private System.Windows.Forms.DataGridViewTextBoxColumn cl4;
        private System.Windows.Forms.DataGridViewTextBoxColumn cl5;
        private System.Windows.Forms.DataGridViewTextBoxColumn cl6;
        private System.Windows.Forms.DataGridViewTextBoxColumn cl7;
        private System.Windows.Forms.DataGridViewTextBoxColumn cl8;
    }
}