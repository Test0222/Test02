namespace HDL_Buspro_Setup_Tool
{
    partial class FrmBusToRS232
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
            this.panel2 = new System.Windows.Forms.Panel();
            this.grpTarget = new System.Windows.Forms.GroupBox();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.dgvUV = new System.Windows.Forms.DataGridView();
            this.cl11 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cl21 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cl31 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BUSRS232Status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel6 = new System.Windows.Forms.Panel();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnSure = new System.Windows.Forms.Button();
            this.txtTo = new System.Windows.Forms.TextBox();
            this.txtFrm = new System.Windows.Forms.TextBox();
            this.lbTo = new System.Windows.Forms.Label();
            this.lbFrm = new System.Windows.Forms.Label();
            this.lbTarget = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.grpBasic = new System.Windows.Forms.GroupBox();
            this.lbRemarkValue = new System.Windows.Forms.Label();
            this.lbRemark = new System.Windows.Forms.Label();
            this.lbDevValue = new System.Windows.Forms.Label();
            this.lbDev = new System.Windows.Forms.Label();
            this.lbSubValue = new System.Windows.Forms.Label();
            this.lbSub = new System.Windows.Forms.Label();
            this.panel2.SuspendLayout();
            this.grpTarget.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUV)).BeginInit();
            this.panel6.SuspendLayout();
            this.panel1.SuspendLayout();
            this.grpBasic.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.grpTarget);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 49);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(915, 396);
            this.panel2.TabIndex = 5;
            // 
            // grpTarget
            // 
            this.grpTarget.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.grpTarget.Controls.Add(this.panel4);
            this.grpTarget.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpTarget.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold);
            this.grpTarget.Location = new System.Drawing.Point(0, 0);
            this.grpTarget.Name = "grpTarget";
            this.grpTarget.Size = new System.Drawing.Size(915, 396);
            this.grpTarget.TabIndex = 0;
            this.grpTarget.TabStop = false;
            this.grpTarget.Text = "Command infomation";
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.panel5);
            this.panel4.Controls.Add(this.panel6);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(3, 23);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(909, 370);
            this.panel4.TabIndex = 1;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.dgvUV);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Font = new System.Drawing.Font("Calibri", 12F);
            this.panel5.Location = new System.Drawing.Point(0, 38);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(909, 332);
            this.panel5.TabIndex = 3;
            // 
            // dgvUV
            // 
            this.dgvUV.AllowUserToAddRows = false;
            this.dgvUV.AllowUserToDeleteRows = false;
            this.dgvUV.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.AliceBlue;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Calibri", 12F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvUV.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvUV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvUV.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.cl11,
            this.cl21,
            this.clType,
            this.cl31,
            this.BUSRS232Status});
            this.dgvUV.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvUV.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgvUV.EnableHeadersVisualStyles = false;
            this.dgvUV.Location = new System.Drawing.Point(0, 0);
            this.dgvUV.Name = "dgvUV";
            this.dgvUV.ReadOnly = true;
            this.dgvUV.RowHeadersVisible = false;
            this.dgvUV.RowHeadersWidth = 10;
            this.dgvUV.RowTemplate.Height = 23;
            this.dgvUV.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvUV.Size = new System.Drawing.Size(909, 332);
            this.dgvUV.TabIndex = 83;
            this.dgvUV.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvUV_CellClick);
            // 
            // cl11
            // 
            this.cl11.HeaderText = "Index";
            this.cl11.Name = "cl11";
            this.cl11.ReadOnly = true;
            this.cl11.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.cl11.Width = 80;
            // 
            // cl21
            // 
            this.cl21.HeaderText = "Remark";
            this.cl21.Name = "cl21";
            this.cl21.ReadOnly = true;
            this.cl21.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.cl21.Width = 250;
            // 
            // clType
            // 
            this.clType.HeaderText = "Type";
            this.clType.Name = "clType";
            this.clType.ReadOnly = true;
            this.clType.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.clType.Width = 180;
            // 
            // cl31
            // 
            this.cl31.HeaderText = "UV NO.";
            this.cl31.Name = "cl31";
            this.cl31.ReadOnly = true;
            this.cl31.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.cl31.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.cl31.Width = 180;
            // 
            // BUSRS232Status
            // 
            this.BUSRS232Status.HeaderText = "Status";
            this.BUSRS232Status.Name = "BUSRS232Status";
            this.BUSRS232Status.ReadOnly = true;
            this.BUSRS232Status.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.BUSRS232Status.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.BUSRS232Status.Width = 180;
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.btnSave);
            this.panel6.Controls.Add(this.btnSure);
            this.panel6.Controls.Add(this.txtTo);
            this.panel6.Controls.Add(this.txtFrm);
            this.panel6.Controls.Add(this.lbTo);
            this.panel6.Controls.Add(this.lbFrm);
            this.panel6.Controls.Add(this.lbTarget);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel6.Font = new System.Drawing.Font("Calibri", 12F);
            this.panel6.Location = new System.Drawing.Point(0, 0);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(909, 38);
            this.panel6.TabIndex = 2;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(613, 5);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(140, 28);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnSure
            // 
            this.btnSure.Location = new System.Drawing.Point(434, 5);
            this.btnSure.Name = "btnSure";
            this.btnSure.Size = new System.Drawing.Size(140, 28);
            this.btnSure.TabIndex = 17;
            this.btnSure.Text = "Read";
            this.btnSure.UseVisualStyleBackColor = true;
            this.btnSure.Click += new System.EventHandler(this.btnSure_Click);
            // 
            // txtTo
            // 
            this.txtTo.Location = new System.Drawing.Point(349, 7);
            this.txtTo.MaxLength = 3;
            this.txtTo.Name = "txtTo";
            this.txtTo.Size = new System.Drawing.Size(75, 27);
            this.txtTo.TabIndex = 16;
            this.txtTo.Text = "1";
            this.txtTo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtFrm_KeyPress);
            this.txtTo.Leave += new System.EventHandler(this.txtTo_TextChanged);
            // 
            // txtFrm
            // 
            this.txtFrm.Location = new System.Drawing.Point(239, 7);
            this.txtFrm.MaxLength = 3;
            this.txtFrm.Name = "txtFrm";
            this.txtFrm.Size = new System.Drawing.Size(75, 27);
            this.txtFrm.TabIndex = 15;
            this.txtFrm.Text = "1";
            this.txtFrm.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtFrm_KeyPress);
            this.txtFrm.Leave += new System.EventHandler(this.txtFrm_TextChanged);
            // 
            // lbTo
            // 
            this.lbTo.AutoSize = true;
            this.lbTo.Location = new System.Drawing.Point(321, 10);
            this.lbTo.Name = "lbTo";
            this.lbTo.Size = new System.Drawing.Size(24, 19);
            this.lbTo.TabIndex = 14;
            this.lbTo.Text = "To";
            // 
            // lbFrm
            // 
            this.lbFrm.AutoSize = true;
            this.lbFrm.Location = new System.Drawing.Point(187, 10);
            this.lbFrm.Name = "lbFrm";
            this.lbFrm.Size = new System.Drawing.Size(41, 19);
            this.lbFrm.TabIndex = 13;
            this.lbFrm.Text = "From";
            // 
            // lbTarget
            // 
            this.lbTarget.AutoSize = true;
            this.lbTarget.Location = new System.Drawing.Point(3, 10);
            this.lbTarget.Name = "lbTarget";
            this.lbTarget.Size = new System.Drawing.Size(169, 19);
            this.lbTarget.TabIndex = 12;
            this.lbTarget.Text = "Input UV number(1-200):";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.grpBasic);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(915, 49);
            this.panel1.TabIndex = 4;
            // 
            // grpBasic
            // 
            this.grpBasic.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.grpBasic.Controls.Add(this.lbRemarkValue);
            this.grpBasic.Controls.Add(this.lbRemark);
            this.grpBasic.Controls.Add(this.lbDevValue);
            this.grpBasic.Controls.Add(this.lbDev);
            this.grpBasic.Controls.Add(this.lbSubValue);
            this.grpBasic.Controls.Add(this.lbSub);
            this.grpBasic.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpBasic.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold);
            this.grpBasic.Location = new System.Drawing.Point(0, 0);
            this.grpBasic.Name = "grpBasic";
            this.grpBasic.Size = new System.Drawing.Size(915, 49);
            this.grpBasic.TabIndex = 0;
            this.grpBasic.TabStop = false;
            this.grpBasic.Text = "Basic information";
            // 
            // lbRemarkValue
            // 
            this.lbRemarkValue.AutoSize = true;
            this.lbRemarkValue.Font = new System.Drawing.Font("Calibri", 12F);
            this.lbRemarkValue.Location = new System.Drawing.Point(414, 21);
            this.lbRemarkValue.Name = "lbRemarkValue";
            this.lbRemarkValue.Size = new System.Drawing.Size(33, 19);
            this.lbRemarkValue.TabIndex = 5;
            this.lbRemarkValue.Text = "      ";
            // 
            // lbRemark
            // 
            this.lbRemark.AutoSize = true;
            this.lbRemark.Font = new System.Drawing.Font("Calibri", 12F);
            this.lbRemark.Location = new System.Drawing.Point(351, 21);
            this.lbRemark.Name = "lbRemark";
            this.lbRemark.Size = new System.Drawing.Size(62, 19);
            this.lbRemark.TabIndex = 4;
            this.lbRemark.Text = "Remark:";
            // 
            // lbDevValue
            // 
            this.lbDevValue.AutoSize = true;
            this.lbDevValue.Font = new System.Drawing.Font("Calibri", 12F);
            this.lbDevValue.Location = new System.Drawing.Point(233, 21);
            this.lbDevValue.Name = "lbDevValue";
            this.lbDevValue.Size = new System.Drawing.Size(17, 19);
            this.lbDevValue.TabIndex = 3;
            this.lbDevValue.Text = "0";
            // 
            // lbDev
            // 
            this.lbDev.AutoSize = true;
            this.lbDev.Font = new System.Drawing.Font("Calibri", 12F);
            this.lbDev.Location = new System.Drawing.Point(156, 21);
            this.lbDev.Name = "lbDev";
            this.lbDev.Size = new System.Drawing.Size(75, 19);
            this.lbDev.TabIndex = 2;
            this.lbDev.Text = "Device ID:";
            // 
            // lbSubValue
            // 
            this.lbSubValue.AutoSize = true;
            this.lbSubValue.Font = new System.Drawing.Font("Calibri", 12F);
            this.lbSubValue.Location = new System.Drawing.Point(83, 21);
            this.lbSubValue.Name = "lbSubValue";
            this.lbSubValue.Size = new System.Drawing.Size(17, 19);
            this.lbSubValue.TabIndex = 1;
            this.lbSubValue.Text = "0";
            // 
            // lbSub
            // 
            this.lbSub.AutoSize = true;
            this.lbSub.Font = new System.Drawing.Font("Calibri", 12F);
            this.lbSub.Location = new System.Drawing.Point(6, 21);
            this.lbSub.Name = "lbSub";
            this.lbSub.Size = new System.Drawing.Size(75, 19);
            this.lbSub.TabIndex = 0;
            this.lbSub.Text = "Subnet ID:";
            // 
            // FrmBusToRS232
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(915, 445);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Calibri", 12F);
            this.Name = "FrmBusToRS232";
            this.Text = "BUS-->RS232 Commands";
            this.Load += new System.EventHandler(this.FrmMCBUS_Load);
            this.panel2.ResumeLayout(false);
            this.grpTarget.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvUV)).EndInit();
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.grpBasic.ResumeLayout(false);
            this.grpBasic.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpBasic;
        private System.Windows.Forms.Label lbRemarkValue;
        private System.Windows.Forms.Label lbRemark;
        private System.Windows.Forms.Label lbDevValue;
        private System.Windows.Forms.Label lbDev;
        private System.Windows.Forms.Label lbSubValue;
        private System.Windows.Forms.Label lbSub;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.GroupBox grpTarget;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnSure;
        private System.Windows.Forms.TextBox txtTo;
        private System.Windows.Forms.TextBox txtFrm;
        private System.Windows.Forms.Label lbTo;
        private System.Windows.Forms.Label lbFrm;
        private System.Windows.Forms.Label lbTarget;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridView dgvUV;
        private System.Windows.Forms.DataGridViewTextBoxColumn cl11;
        private System.Windows.Forms.DataGridViewTextBoxColumn cl21;
        private System.Windows.Forms.DataGridViewTextBoxColumn clType;
        private System.Windows.Forms.DataGridViewTextBoxColumn cl31;
        private System.Windows.Forms.DataGridViewTextBoxColumn BUSRS232Status;
    }
}