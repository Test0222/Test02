namespace HDL_Buspro_Setup_Tool
{
    partial class FrmInfrared
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cbType = new System.Windows.Forms.ComboBox();
            this.lbType = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnSave = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.grpTarget = new System.Windows.Forms.GroupBox();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.dgvTarget = new System.Windows.Forms.DataGridView();
            this.cl1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cl2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cl3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cl4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cl5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cl6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cl7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cl8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.grpTarget.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTarget)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.cbType);
            this.panel1.Controls.Add(this.lbType);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(975, 47);
            this.panel1.TabIndex = 0;
            // 
            // cbType
            // 
            this.cbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbType.FormattingEnabled = true;
            this.cbType.Location = new System.Drawing.Point(125, 12);
            this.cbType.Name = "cbType";
            this.cbType.Size = new System.Drawing.Size(288, 22);
            this.cbType.TabIndex = 1;
            this.cbType.SelectedIndexChanged += new System.EventHandler(this.cbType_SelectedIndexChanged);
            // 
            // lbType
            // 
            this.lbType.AutoSize = true;
            this.lbType.Location = new System.Drawing.Point(12, 15);
            this.lbType.Name = "lbType";
            this.lbType.Size = new System.Drawing.Size(56, 14);
            this.lbType.TabIndex = 0;
            this.lbType.Text = "Function:";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnSave);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 372);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(975, 39);
            this.panel2.TabIndex = 1;
            // 
            // btnSave
            // 
            this.btnSave.Image = global::HDL_Buspro_Setup_Tool.Properties.Resources.Save;
            this.btnSave.Location = new System.Drawing.Point(823, 4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(140, 32);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "Save";
            this.btnSave.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.grpTarget);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 47);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(975, 325);
            this.panel3.TabIndex = 2;
            // 
            // grpTarget
            // 
            this.grpTarget.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.grpTarget.Controls.Add(this.panel4);
            this.grpTarget.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpTarget.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpTarget.Location = new System.Drawing.Point(0, 0);
            this.grpTarget.Name = "grpTarget";
            this.grpTarget.Size = new System.Drawing.Size(975, 325);
            this.grpTarget.TabIndex = 1;
            this.grpTarget.TabStop = false;
            this.grpTarget.Text = "Targets";
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.panel5);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(3, 18);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(969, 304);
            this.panel4.TabIndex = 1;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.dgvTarget);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Font = new System.Drawing.Font("Calibri", 12F);
            this.panel5.Location = new System.Drawing.Point(0, 0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(969, 304);
            this.panel5.TabIndex = 3;
            // 
            // dgvTarget
            // 
            this.dgvTarget.AllowUserToAddRows = false;
            this.dgvTarget.AllowUserToDeleteRows = false;
            this.dgvTarget.AllowUserToResizeColumns = false;
            this.dgvTarget.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.AliceBlue;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvTarget.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvTarget.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTarget.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.cl1,
            this.cl2,
            this.cl3,
            this.cl4,
            this.cl5,
            this.cl6,
            this.cl7,
            this.cl8});
            this.dgvTarget.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvTarget.EnableHeadersVisualStyles = false;
            this.dgvTarget.Location = new System.Drawing.Point(0, 0);
            this.dgvTarget.Name = "dgvTarget";
            this.dgvTarget.ReadOnly = true;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvTarget.RowHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvTarget.RowHeadersVisible = false;
            this.dgvTarget.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvTarget.Size = new System.Drawing.Size(969, 304);
            this.dgvTarget.TabIndex = 0;
            this.dgvTarget.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvTarget_CellClick);
            // 
            // cl1
            // 
            this.cl1.HeaderText = "Function";
            this.cl1.Name = "cl1";
            this.cl1.ReadOnly = true;
            this.cl1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.cl1.Width = 180;
            // 
            // cl2
            // 
            this.cl2.HeaderText = "Subnet ID";
            this.cl2.Name = "cl2";
            this.cl2.ReadOnly = true;
            this.cl2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.cl2.Width = 80;
            // 
            // cl3
            // 
            this.cl3.HeaderText = "Device ID";
            this.cl3.Name = "cl3";
            this.cl3.ReadOnly = true;
            this.cl3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.cl3.Width = 80;
            // 
            // cl4
            // 
            this.cl4.HeaderText = "Type";
            this.cl4.Name = "cl4";
            this.cl4.ReadOnly = true;
            this.cl4.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.cl4.Width = 200;
            // 
            // cl5
            // 
            this.cl5.HeaderText = "Param1";
            this.cl5.Name = "cl5";
            this.cl5.ReadOnly = true;
            this.cl5.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.cl5.Width = 130;
            // 
            // cl6
            // 
            this.cl6.HeaderText = "Param2";
            this.cl6.Name = "cl6";
            this.cl6.ReadOnly = true;
            this.cl6.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.cl6.Width = 130;
            // 
            // cl7
            // 
            this.cl7.HeaderText = "Param3";
            this.cl7.Name = "cl7";
            this.cl7.ReadOnly = true;
            this.cl7.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.cl7.Width = 130;
            // 
            // cl8
            // 
            this.cl8.HeaderText = "Param4";
            this.cl8.Name = "cl8";
            this.cl8.ReadOnly = true;
            this.cl8.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.cl8.Visible = false;
            // 
            // FrmInfrared
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(975, 411);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmInfrared";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = " IR Targets";
            this.Load += new System.EventHandler(this.FrmInfrared_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.grpTarget.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTarget)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ComboBox cbType;
        private System.Windows.Forms.Label lbType;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.GroupBox grpTarget;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.DataGridView dgvTarget;
        private System.Windows.Forms.Button btnSave;
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