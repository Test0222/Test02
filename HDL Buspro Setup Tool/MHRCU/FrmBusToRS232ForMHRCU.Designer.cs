namespace HDL_Buspro_Setup_Tool
{
    partial class FrmBusToRS232ForMHRCU
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel2 = new System.Windows.Forms.Panel();
            this.grpTarget = new System.Windows.Forms.GroupBox();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.dgvUV = new System.Windows.Forms.DataGridView();
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
            this.cl11 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cl21 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cl31 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BUSRS232Status = new System.Windows.Forms.DataGridViewTextBoxColumn();
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
            this.panel2.Location = new System.Drawing.Point(0, 52);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(625, 393);
            this.panel2.TabIndex = 5;
            // 
            // grpTarget
            // 
            this.grpTarget.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.grpTarget.Controls.Add(this.panel4);
            this.grpTarget.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpTarget.Location = new System.Drawing.Point(0, 0);
            this.grpTarget.Name = "grpTarget";
            this.grpTarget.Size = new System.Drawing.Size(625, 393);
            this.grpTarget.TabIndex = 0;
            this.grpTarget.TabStop = false;
            this.grpTarget.Text = "Command Infomation";
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.panel5);
            this.panel4.Controls.Add(this.panel6);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(3, 18);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(619, 372);
            this.panel4.TabIndex = 1;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.dgvUV);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(0, 38);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(619, 334);
            this.panel5.TabIndex = 3;
            // 
            // dgvUV
            // 
            this.dgvUV.AllowUserToAddRows = false;
            this.dgvUV.AllowUserToDeleteRows = false;
            this.dgvUV.AllowUserToResizeRows = false;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.AliceBlue;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvUV.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
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
            this.dgvUV.Size = new System.Drawing.Size(619, 334);
            this.dgvUV.TabIndex = 83;
            this.dgvUV.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvUV_CellClick);
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
            this.panel6.Location = new System.Drawing.Point(0, 0);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(619, 38);
            this.panel6.TabIndex = 2;
            // 
            // btnSave
            // 
            this.btnSave.Image = global::HDL_Buspro_Setup_Tool.Properties.Resources.Save;
            this.btnSave.Location = new System.Drawing.Point(509, 5);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(71, 28);
            this.btnSave.TabIndex = 0;
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnSure
            // 
            this.btnSure.Image = global::HDL_Buspro_Setup_Tool.Properties.Resources.Refresh;
            this.btnSure.Location = new System.Drawing.Point(421, 5);
            this.btnSure.Name = "btnSure";
            this.btnSure.Size = new System.Drawing.Size(71, 28);
            this.btnSure.TabIndex = 17;
            this.btnSure.UseVisualStyleBackColor = true;
            this.btnSure.Click += new System.EventHandler(this.btnSure_Click);
            // 
            // txtTo
            // 
            this.txtTo.Location = new System.Drawing.Point(336, 7);
            this.txtTo.Name = "txtTo";
            this.txtTo.Size = new System.Drawing.Size(75, 22);
            this.txtTo.TabIndex = 16;
            this.txtTo.Text = "1";
            this.txtTo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtFrm_KeyPress);
            this.txtTo.Leave += new System.EventHandler(this.txtTo_TextChanged);
            // 
            // txtFrm
            // 
            this.txtFrm.Location = new System.Drawing.Point(226, 7);
            this.txtFrm.Name = "txtFrm";
            this.txtFrm.Size = new System.Drawing.Size(75, 22);
            this.txtFrm.TabIndex = 15;
            this.txtFrm.Text = "1";
            this.txtFrm.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtFrm_KeyPress);
            this.txtFrm.Leave += new System.EventHandler(this.txtFrm_TextChanged);
            // 
            // lbTo
            // 
            this.lbTo.AutoSize = true;
            this.lbTo.Location = new System.Drawing.Point(308, 10);
            this.lbTo.Name = "lbTo";
            this.lbTo.Size = new System.Drawing.Size(19, 14);
            this.lbTo.TabIndex = 14;
            this.lbTo.Text = "To";
            // 
            // lbFrm
            // 
            this.lbFrm.AutoSize = true;
            this.lbFrm.Location = new System.Drawing.Point(181, 10);
            this.lbFrm.Name = "lbFrm";
            this.lbFrm.Size = new System.Drawing.Size(34, 14);
            this.lbFrm.TabIndex = 13;
            this.lbFrm.Text = "From";
            // 
            // lbTarget
            // 
            this.lbTarget.AutoSize = true;
            this.lbTarget.Location = new System.Drawing.Point(3, 10);
            this.lbTarget.Name = "lbTarget";
            this.lbTarget.Size = new System.Drawing.Size(133, 14);
            this.lbTarget.TabIndex = 12;
            this.lbTarget.Text = "Input UV Number(1-49):";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.grpBasic);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(625, 52);
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
            this.grpBasic.Location = new System.Drawing.Point(0, 0);
            this.grpBasic.Name = "grpBasic";
            this.grpBasic.Size = new System.Drawing.Size(625, 52);
            this.grpBasic.TabIndex = 0;
            this.grpBasic.TabStop = false;
            this.grpBasic.Text = "Basic Information";
            // 
            // lbRemarkValue
            // 
            this.lbRemarkValue.AutoSize = true;
            this.lbRemarkValue.Location = new System.Drawing.Point(416, 23);
            this.lbRemarkValue.Name = "lbRemarkValue";
            this.lbRemarkValue.Size = new System.Drawing.Size(25, 14);
            this.lbRemarkValue.TabIndex = 5;
            this.lbRemarkValue.Text = "      ";
            // 
            // lbRemark
            // 
            this.lbRemark.AutoSize = true;
            this.lbRemark.Location = new System.Drawing.Point(353, 23);
            this.lbRemark.Name = "lbRemark";
            this.lbRemark.Size = new System.Drawing.Size(51, 14);
            this.lbRemark.TabIndex = 4;
            this.lbRemark.Text = "Remark:";
            // 
            // lbDevValue
            // 
            this.lbDevValue.AutoSize = true;
            this.lbDevValue.Location = new System.Drawing.Point(235, 23);
            this.lbDevValue.Name = "lbDevValue";
            this.lbDevValue.Size = new System.Drawing.Size(13, 14);
            this.lbDevValue.TabIndex = 3;
            this.lbDevValue.Text = "0";
            // 
            // lbDev
            // 
            this.lbDev.AutoSize = true;
            this.lbDev.Location = new System.Drawing.Point(158, 23);
            this.lbDev.Name = "lbDev";
            this.lbDev.Size = new System.Drawing.Size(61, 14);
            this.lbDev.TabIndex = 2;
            this.lbDev.Text = "Device ID:";
            // 
            // lbSubValue
            // 
            this.lbSubValue.AutoSize = true;
            this.lbSubValue.Location = new System.Drawing.Point(85, 23);
            this.lbSubValue.Name = "lbSubValue";
            this.lbSubValue.Size = new System.Drawing.Size(13, 14);
            this.lbSubValue.TabIndex = 1;
            this.lbSubValue.Text = "0";
            // 
            // lbSub
            // 
            this.lbSub.AutoSize = true;
            this.lbSub.Location = new System.Drawing.Point(8, 23);
            this.lbSub.Name = "lbSub";
            this.lbSub.Size = new System.Drawing.Size(63, 14);
            this.lbSub.TabIndex = 0;
            this.lbSub.Text = "Subnet ID:";
            // 
            // cl11
            // 
            this.cl11.HeaderText = "ID";
            this.cl11.Name = "cl11";
            this.cl11.ReadOnly = true;
            this.cl11.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.cl11.Width = 32;
            // 
            // cl21
            // 
            this.cl21.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.cl21.HeaderText = "Remark";
            this.cl21.Name = "cl21";
            this.cl21.ReadOnly = true;
            this.cl21.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // clType
            // 
            this.clType.HeaderText = "Type";
            this.clType.Name = "clType";
            this.clType.ReadOnly = true;
            this.clType.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.clType.Width = 160;
            // 
            // cl31
            // 
            this.cl31.HeaderText = "UV No.";
            this.cl31.Name = "cl31";
            this.cl31.ReadOnly = true;
            this.cl31.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.cl31.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // BUSRS232Status
            // 
            this.BUSRS232Status.HeaderText = "Status";
            this.BUSRS232Status.Name = "BUSRS232Status";
            this.BUSRS232Status.ReadOnly = true;
            this.BUSRS232Status.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.BUSRS232Status.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // FrmBusToRS232ForMHRCU
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(625, 445);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "FrmBusToRS232ForMHRCU";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "BUS-->RS232 Command";
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