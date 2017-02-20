namespace HDL_Buspro_Setup_Tool
{
    partial class FrmBusRS232TargetForMHRCU
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmBusRS232TargetForMHRCU));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel2 = new System.Windows.Forms.Panel();
            this.grpTarget = new System.Windows.Forms.GroupBox();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.dgvCommand = new System.Windows.Forms.DataGridView();
            this.panel6 = new System.Windows.Forms.Panel();
            this.lbHint = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnSure = new System.Windows.Forms.Button();
            this.txtTo = new System.Windows.Forms.TextBox();
            this.txtFrm = new System.Windows.Forms.TextBox();
            this.lbTo = new System.Windows.Forms.Label();
            this.lbFrm = new System.Windows.Forms.Label();
            this.lbTarget = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.grpBasic = new System.Windows.Forms.GroupBox();
            this.lbCommandID = new System.Windows.Forms.Label();
            this.lbCommand = new System.Windows.Forms.Label();
            this.lbRemarkValue = new System.Windows.Forms.Label();
            this.lbRemark = new System.Windows.Forms.Label();
            this.lbDevValue = new System.Windows.Forms.Label();
            this.lbDev = new System.Windows.Forms.Label();
            this.lbSubValue = new System.Windows.Forms.Label();
            this.lbSub = new System.Windows.Forms.Label();
            this.clA1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clA3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clA4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clA5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clA6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel2.SuspendLayout();
            this.grpTarget.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCommand)).BeginInit();
            this.panel6.SuspendLayout();
            this.panel1.SuspendLayout();
            this.grpBasic.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.grpTarget);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 76);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(942, 359);
            this.panel2.TabIndex = 5;
            // 
            // grpTarget
            // 
            this.grpTarget.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("grpTarget.BackgroundImage")));
            this.grpTarget.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.grpTarget.Controls.Add(this.panel4);
            this.grpTarget.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpTarget.Location = new System.Drawing.Point(0, 0);
            this.grpTarget.Name = "grpTarget";
            this.grpTarget.Size = new System.Drawing.Size(942, 359);
            this.grpTarget.TabIndex = 0;
            this.grpTarget.TabStop = false;
            this.grpTarget.Text = "Target Infomation";
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.panel5);
            this.panel4.Controls.Add(this.panel6);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(3, 23);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(936, 333);
            this.panel4.TabIndex = 1;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.dgvCommand);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(0, 35);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(936, 298);
            this.panel5.TabIndex = 3;
            // 
            // dgvCommand
            // 
            this.dgvCommand.AllowDrop = true;
            this.dgvCommand.AllowUserToAddRows = false;
            this.dgvCommand.AllowUserToDeleteRows = false;
            this.dgvCommand.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.AliceBlue;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Calibri", 12F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvCommand.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvCommand.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCommand.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.clA1,
            this.clA3,
            this.clA4,
            this.clA5,
            this.clA6});
            this.dgvCommand.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvCommand.EnableHeadersVisualStyles = false;
            this.dgvCommand.Location = new System.Drawing.Point(0, 0);
            this.dgvCommand.Name = "dgvCommand";
            this.dgvCommand.ReadOnly = true;
            this.dgvCommand.RowHeadersVisible = false;
            this.dgvCommand.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvCommand.Size = new System.Drawing.Size(936, 298);
            this.dgvCommand.TabIndex = 1;
            this.dgvCommand.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvCommand_CellClick);
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.lbHint);
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
            this.panel6.Size = new System.Drawing.Size(936, 35);
            this.panel6.TabIndex = 2;
            // 
            // lbHint
            // 
            this.lbHint.AutoSize = true;
            this.lbHint.ForeColor = System.Drawing.Color.Blue;
            this.lbHint.Location = new System.Drawing.Point(640, 9);
            this.lbHint.Name = "lbHint";
            this.lbHint.Size = new System.Drawing.Size(288, 19);
            this.lbHint.TabIndex = 18;
            this.lbHint.Text = "Command Delimiter is \" \" when type is Hex";
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(538, 4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 28);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnSure
            // 
            this.btnSure.Location = new System.Drawing.Point(451, 4);
            this.btnSure.Name = "btnSure";
            this.btnSure.Size = new System.Drawing.Size(71, 28);
            this.btnSure.TabIndex = 17;
            this.btnSure.Text = "Read";
            this.btnSure.UseVisualStyleBackColor = true;
            this.btnSure.Click += new System.EventHandler(this.btnSure_Click);
            // 
            // txtTo
            // 
            this.txtTo.Location = new System.Drawing.Point(366, 4);
            this.txtTo.Name = "txtTo";
            this.txtTo.Size = new System.Drawing.Size(75, 27);
            this.txtTo.TabIndex = 16;
            this.txtTo.Text = "1";
            this.txtTo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtFrm_KeyPress);
            this.txtTo.Leave += new System.EventHandler(this.txtTo_Leave);
            // 
            // txtFrm
            // 
            this.txtFrm.Location = new System.Drawing.Point(257, 4);
            this.txtFrm.Name = "txtFrm";
            this.txtFrm.Size = new System.Drawing.Size(75, 27);
            this.txtFrm.TabIndex = 15;
            this.txtFrm.Text = "1";
            this.txtFrm.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtFrm_KeyPress);
            this.txtFrm.Leave += new System.EventHandler(this.txtFrm_Leave);
            // 
            // lbTo
            // 
            this.lbTo.AutoSize = true;
            this.lbTo.Location = new System.Drawing.Point(338, 8);
            this.lbTo.Name = "lbTo";
            this.lbTo.Size = new System.Drawing.Size(24, 19);
            this.lbTo.TabIndex = 14;
            this.lbTo.Text = "To";
            // 
            // lbFrm
            // 
            this.lbFrm.AutoSize = true;
            this.lbFrm.Location = new System.Drawing.Point(198, 8);
            this.lbFrm.Name = "lbFrm";
            this.lbFrm.Size = new System.Drawing.Size(41, 19);
            this.lbFrm.TabIndex = 13;
            this.lbFrm.Text = "From";
            // 
            // lbTarget
            // 
            this.lbTarget.AutoSize = true;
            this.lbTarget.Location = new System.Drawing.Point(3, 8);
            this.lbTarget.Name = "lbTarget";
            this.lbTarget.Size = new System.Drawing.Size(185, 19);
            this.lbTarget.TabIndex = 12;
            this.lbTarget.Text = "Input Target Number(1-20):";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.grpBasic);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(942, 76);
            this.panel1.TabIndex = 4;
            // 
            // grpBasic
            // 
            this.grpBasic.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("grpBasic.BackgroundImage")));
            this.grpBasic.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.grpBasic.Controls.Add(this.lbCommandID);
            this.grpBasic.Controls.Add(this.lbCommand);
            this.grpBasic.Controls.Add(this.lbRemarkValue);
            this.grpBasic.Controls.Add(this.lbRemark);
            this.grpBasic.Controls.Add(this.lbDevValue);
            this.grpBasic.Controls.Add(this.lbDev);
            this.grpBasic.Controls.Add(this.lbSubValue);
            this.grpBasic.Controls.Add(this.lbSub);
            this.grpBasic.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpBasic.Location = new System.Drawing.Point(0, 0);
            this.grpBasic.Name = "grpBasic";
            this.grpBasic.Size = new System.Drawing.Size(942, 76);
            this.grpBasic.TabIndex = 0;
            this.grpBasic.TabStop = false;
            this.grpBasic.Text = "Basic Information";
            // 
            // lbCommandID
            // 
            this.lbCommandID.AutoSize = true;
            this.lbCommandID.Location = new System.Drawing.Point(184, 49);
            this.lbCommandID.Name = "lbCommandID";
            this.lbCommandID.Size = new System.Drawing.Size(17, 19);
            this.lbCommandID.TabIndex = 9;
            this.lbCommandID.Text = "0";
            // 
            // lbCommand
            // 
            this.lbCommand.AutoSize = true;
            this.lbCommand.Location = new System.Drawing.Point(7, 49);
            this.lbCommand.Name = "lbCommand";
            this.lbCommand.Size = new System.Drawing.Size(159, 19);
            this.lbCommand.TabIndex = 8;
            this.lbCommand.Text = "Current Command NO.:";
            // 
            // lbRemarkValue
            // 
            this.lbRemarkValue.AutoSize = true;
            this.lbRemarkValue.Location = new System.Drawing.Point(414, 23);
            this.lbRemarkValue.Name = "lbRemarkValue";
            this.lbRemarkValue.Size = new System.Drawing.Size(33, 19);
            this.lbRemarkValue.TabIndex = 5;
            this.lbRemarkValue.Text = "      ";
            // 
            // lbRemark
            // 
            this.lbRemark.AutoSize = true;
            this.lbRemark.Location = new System.Drawing.Point(351, 23);
            this.lbRemark.Name = "lbRemark";
            this.lbRemark.Size = new System.Drawing.Size(62, 19);
            this.lbRemark.TabIndex = 4;
            this.lbRemark.Text = "Remark:";
            // 
            // lbDevValue
            // 
            this.lbDevValue.AutoSize = true;
            this.lbDevValue.Location = new System.Drawing.Point(233, 23);
            this.lbDevValue.Name = "lbDevValue";
            this.lbDevValue.Size = new System.Drawing.Size(17, 19);
            this.lbDevValue.TabIndex = 3;
            this.lbDevValue.Text = "0";
            // 
            // lbDev
            // 
            this.lbDev.AutoSize = true;
            this.lbDev.Location = new System.Drawing.Point(156, 23);
            this.lbDev.Name = "lbDev";
            this.lbDev.Size = new System.Drawing.Size(75, 19);
            this.lbDev.TabIndex = 2;
            this.lbDev.Text = "Device ID:";
            // 
            // lbSubValue
            // 
            this.lbSubValue.AutoSize = true;
            this.lbSubValue.Location = new System.Drawing.Point(89, 23);
            this.lbSubValue.Name = "lbSubValue";
            this.lbSubValue.Size = new System.Drawing.Size(17, 19);
            this.lbSubValue.TabIndex = 1;
            this.lbSubValue.Text = "0";
            // 
            // lbSub
            // 
            this.lbSub.AutoSize = true;
            this.lbSub.Location = new System.Drawing.Point(7, 23);
            this.lbSub.Name = "lbSub";
            this.lbSub.Size = new System.Drawing.Size(75, 19);
            this.lbSub.TabIndex = 0;
            this.lbSub.Text = "Subnet ID:";
            // 
            // clA1
            // 
            this.clA1.HeaderText = "Index";
            this.clA1.Name = "clA1";
            this.clA1.ReadOnly = true;
            this.clA1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.clA1.Width = 80;
            // 
            // clA3
            // 
            this.clA3.HeaderText = "Time";
            this.clA3.Name = "clA3";
            this.clA3.ReadOnly = true;
            this.clA3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // clA4
            // 
            this.clA4.HeaderText = "Format";
            this.clA4.Name = "clA4";
            this.clA4.ReadOnly = true;
            this.clA4.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // clA5
            // 
            this.clA5.HeaderText = "Character String";
            this.clA5.Name = "clA5";
            this.clA5.ReadOnly = true;
            this.clA5.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.clA5.Width = 500;
            // 
            // clA6
            // 
            this.clA6.HeaderText = "End Character String";
            this.clA6.Name = "clA6";
            this.clA6.ReadOnly = true;
            this.clA6.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // FrmBusRS232TargetForMHRCU
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(942, 435);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Calibri", 12F);
            this.Name = "FrmBusRS232TargetForMHRCU";
            this.Text = "BUS-->RS232 Target of Current Command";
            this.Load += new System.EventHandler(this.FrmRS232BUS_Load);
            this.panel2.ResumeLayout(false);
            this.grpTarget.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvCommand)).EndInit();
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.grpBasic.ResumeLayout(false);
            this.grpBasic.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.GroupBox grpTarget;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Label lbHint;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnSure;
        private System.Windows.Forms.TextBox txtTo;
        private System.Windows.Forms.TextBox txtFrm;
        private System.Windows.Forms.Label lbTo;
        private System.Windows.Forms.Label lbFrm;
        private System.Windows.Forms.Label lbTarget;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox grpBasic;
        private System.Windows.Forms.Label lbRemarkValue;
        private System.Windows.Forms.Label lbRemark;
        private System.Windows.Forms.Label lbDevValue;
        private System.Windows.Forms.Label lbDev;
        private System.Windows.Forms.Label lbSubValue;
        private System.Windows.Forms.Label lbSub;
        private System.Windows.Forms.DataGridView dgvCommand;
        private System.Windows.Forms.Label lbCommandID;
        private System.Windows.Forms.Label lbCommand;
        private System.Windows.Forms.DataGridViewTextBoxColumn clA1;
        private System.Windows.Forms.DataGridViewTextBoxColumn clA3;
        private System.Windows.Forms.DataGridViewTextBoxColumn clA4;
        private System.Windows.Forms.DataGridViewTextBoxColumn clA5;
        private System.Windows.Forms.DataGridViewTextBoxColumn clA6;
    }
}