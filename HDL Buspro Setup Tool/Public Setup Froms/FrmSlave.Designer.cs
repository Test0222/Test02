namespace HDL_Buspro_Setup_Tool
{
    partial class FrmSlave
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.dgvSlave = new System.Windows.Forms.DataGridView();
            this.clS1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clS2 = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.clS3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clS4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.panel4 = new System.Windows.Forms.Panel();
            this.dgvSyn = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel3 = new System.Windows.Forms.Panel();
            this.chbEnable = new System.Windows.Forms.CheckBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnSave = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSlave)).BeginInit();
            this.tabPage2.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSyn)).BeginInit();
            this.panel3.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(4);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(804, 329);
            this.tabControl1.TabIndex = 0;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage1.Controls.Add(this.dgvSlave);
            this.tabPage1.Location = new System.Drawing.Point(4, 23);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(4);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(4);
            this.tabPage1.Size = new System.Drawing.Size(796, 302);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Additional AC Page";
            // 
            // dgvSlave
            // 
            this.dgvSlave.AllowUserToAddRows = false;
            this.dgvSlave.AllowUserToDeleteRows = false;
            this.dgvSlave.AllowUserToResizeRows = false;
            this.dgvSlave.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.AliceBlue;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvSlave.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvSlave.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSlave.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.clS1,
            this.clS2,
            this.clS3,
            this.clS4});
            this.dgvSlave.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvSlave.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgvSlave.EnableHeadersVisualStyles = false;
            this.dgvSlave.Location = new System.Drawing.Point(4, 4);
            this.dgvSlave.Margin = new System.Windows.Forms.Padding(4);
            this.dgvSlave.Name = "dgvSlave";
            this.dgvSlave.RowHeadersVisible = false;
            this.dgvSlave.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSlave.Size = new System.Drawing.Size(788, 294);
            this.dgvSlave.TabIndex = 0;
            this.dgvSlave.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvSlave_CellValueChanged);
            this.dgvSlave.CurrentCellDirtyStateChanged += new System.EventHandler(this.dgvSlave_CurrentCellDirtyStateChanged);
            // 
            // clS1
            // 
            this.clS1.FillWeight = 50.95542F;
            this.clS1.HeaderText = "Slave NO.";
            this.clS1.Name = "clS1";
            this.clS1.ReadOnly = true;
            this.clS1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // clS2
            // 
            this.clS2.FillWeight = 101.7395F;
            this.clS2.HeaderText = "Status";
            this.clS2.Name = "clS2";
            this.clS2.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // clS3
            // 
            this.clS3.FillWeight = 117.342F;
            this.clS3.HeaderText = "Subnet ID";
            this.clS3.Name = "clS3";
            this.clS3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // clS4
            // 
            this.clS4.FillWeight = 129.9631F;
            this.clS4.HeaderText = "Device ID";
            this.clS4.Name = "clS4";
            this.clS4.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.panel4);
            this.tabPage2.Controls.Add(this.panel3);
            this.tabPage2.Location = new System.Drawing.Point(4, 23);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(4);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(4);
            this.tabPage2.Size = new System.Drawing.Size(796, 302);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Synchronous Control";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.dgvSyn);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(4, 46);
            this.panel4.Margin = new System.Windows.Forms.Padding(4);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(788, 252);
            this.panel4.TabIndex = 3;
            // 
            // dgvSyn
            // 
            this.dgvSyn.AllowUserToAddRows = false;
            this.dgvSyn.AllowUserToDeleteRows = false;
            this.dgvSyn.AllowUserToResizeColumns = false;
            this.dgvSyn.AllowUserToResizeRows = false;
            this.dgvSyn.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.AliceBlue;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvSyn.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dgvSyn.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSyn.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewTextBoxColumn3});
            this.dgvSyn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvSyn.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgvSyn.EnableHeadersVisualStyles = false;
            this.dgvSyn.Location = new System.Drawing.Point(0, 0);
            this.dgvSyn.Margin = new System.Windows.Forms.Padding(4);
            this.dgvSyn.Name = "dgvSyn";
            this.dgvSyn.RowHeadersVisible = false;
            this.dgvSyn.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSyn.Size = new System.Drawing.Size(788, 252);
            this.dgvSyn.TabIndex = 1;
            this.dgvSyn.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvSyn_CellValueChanged);
            this.dgvSyn.CurrentCellDirtyStateChanged += new System.EventHandler(this.dgvSyn_CurrentCellDirtyStateChanged);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.FillWeight = 38.21656F;
            this.dataGridViewTextBoxColumn1.HeaderText = "DLP";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.FillWeight = 119.6551F;
            this.dataGridViewTextBoxColumn2.HeaderText = "Subnet ID";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.FillWeight = 142.1284F;
            this.dataGridViewTextBoxColumn3.HeaderText = "Device ID";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.SystemColors.Control;
            this.panel3.Controls.Add(this.chbEnable);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(4, 4);
            this.panel3.Margin = new System.Windows.Forms.Padding(4);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(788, 42);
            this.panel3.TabIndex = 2;
            // 
            // chbEnable
            // 
            this.chbEnable.AutoSize = true;
            this.chbEnable.Location = new System.Drawing.Point(8, 12);
            this.chbEnable.Margin = new System.Windows.Forms.Padding(4);
            this.chbEnable.Name = "chbEnable";
            this.chbEnable.Size = new System.Drawing.Size(138, 18);
            this.chbEnable.TabIndex = 1;
            this.chbEnable.Text = "Enable/Disable Sync";
            this.chbEnable.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.tabControl1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(804, 329);
            this.panel1.TabIndex = 1;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnSave);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 329);
            this.panel2.Margin = new System.Windows.Forms.Padding(4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(804, 41);
            this.panel2.TabIndex = 2;
            // 
            // btnSave
            // 
            this.btnSave.Image = global::HDL_Buspro_Setup_Tool.Properties.Resources.Save;
            this.btnSave.Location = new System.Drawing.Point(651, 4);
            this.btnSave.Margin = new System.Windows.Forms.Padding(4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(140, 32);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "Save";
            this.btnSave.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // FrmSlave
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(804, 370);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(820, 408);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(820, 408);
            this.Name = "FrmSlave";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = " Additional AC Page And Synchronous Control";
            this.Load += new System.EventHandler(this.FrmSlave_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSlave)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSyn)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.DataGridView dgvSlave;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.DataGridView dgvSyn;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.CheckBox chbEnable;
        private System.Windows.Forms.DataGridViewTextBoxColumn clS1;
        private System.Windows.Forms.DataGridViewComboBoxColumn clS2;
        private System.Windows.Forms.DataGridViewTextBoxColumn clS3;
        private System.Windows.Forms.DataGridViewTextBoxColumn clS4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
    }
}