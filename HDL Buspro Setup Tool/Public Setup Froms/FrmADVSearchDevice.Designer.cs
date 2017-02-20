namespace HDL_Buspro_Setup_Tool
{
    partial class FrmADVSearchDevice
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmADVSearchDevice));
            this.panel1 = new System.Windows.Forms.Panel();
            this.lbM = new System.Windows.Forms.Label();
            this.lbA = new System.Windows.Forms.Label();
            this.txtMDev = new System.Windows.Forms.TextBox();
            this.lbMDevice = new System.Windows.Forms.Label();
            this.txtMSub = new System.Windows.Forms.TextBox();
            this.lbMSub = new System.Windows.Forms.Label();
            this.btnManual = new System.Windows.Forms.Button();
            this.btnADV = new System.Windows.Forms.Button();
            this.txtADev2 = new System.Windows.Forms.TextBox();
            this.txtADev1 = new System.Windows.Forms.TextBox();
            this.txtASub = new System.Windows.Forms.TextBox();
            this.lbTo = new System.Windows.Forms.Label();
            this.lbDev = new System.Windows.Forms.Label();
            this.lbSub = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnStop = new System.Windows.Forms.Button();
            this.lbCurrentDevValue = new System.Windows.Forms.Label();
            this.lbCurrentDev = new System.Windows.Forms.Label();
            this.lbCurrentSubValue = new System.Windows.Forms.Label();
            this.lbCurrentSub = new System.Windows.Forms.Label();
            this.lbCurrent = new System.Windows.Forms.Label();
            this.Process = new System.Windows.Forms.ProgressBar();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dgvResult = new System.Windows.Forms.DataGridView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.cl1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.cl2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cl3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cl4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cl5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cl6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DeviceType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvResult)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lbM);
            this.panel1.Controls.Add(this.lbA);
            this.panel1.Controls.Add(this.txtMDev);
            this.panel1.Controls.Add(this.lbMDevice);
            this.panel1.Controls.Add(this.txtMSub);
            this.panel1.Controls.Add(this.lbMSub);
            this.panel1.Controls.Add(this.btnManual);
            this.panel1.Controls.Add(this.btnADV);
            this.panel1.Controls.Add(this.txtADev2);
            this.panel1.Controls.Add(this.txtADev1);
            this.panel1.Controls.Add(this.txtASub);
            this.panel1.Controls.Add(this.lbTo);
            this.panel1.Controls.Add(this.lbDev);
            this.panel1.Controls.Add(this.lbSub);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(910, 85);
            this.panel1.TabIndex = 0;
            // 
            // lbM
            // 
            this.lbM.AutoSize = true;
            this.lbM.Location = new System.Drawing.Point(8, 51);
            this.lbM.Name = "lbM";
            this.lbM.Size = new System.Drawing.Size(61, 14);
            this.lbM.TabIndex = 16;
            this.lbM.Text = "Manually:";
            // 
            // lbA
            // 
            this.lbA.AutoSize = true;
            this.lbA.Location = new System.Drawing.Point(8, 10);
            this.lbA.Name = "lbA";
            this.lbA.Size = new System.Drawing.Size(55, 14);
            this.lbA.TabIndex = 15;
            this.lbA.Text = "Advance:";
            // 
            // txtMDev
            // 
            this.txtMDev.Location = new System.Drawing.Point(463, 42);
            this.txtMDev.Name = "txtMDev";
            this.txtMDev.Size = new System.Drawing.Size(100, 22);
            this.txtMDev.TabIndex = 14;
            this.txtMDev.Text = "0";
            this.txtMDev.TextChanged += new System.EventHandler(this.txtMDev_TextChanged);
            this.txtMDev.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtASub_KeyPress);
            this.txtMDev.Leave += new System.EventHandler(this.txtMDev_Leave);
            // 
            // lbMDevice
            // 
            this.lbMDevice.AutoSize = true;
            this.lbMDevice.Location = new System.Drawing.Point(332, 47);
            this.lbMDevice.Name = "lbMDevice";
            this.lbMDevice.Size = new System.Drawing.Size(61, 14);
            this.lbMDevice.TabIndex = 13;
            this.lbMDevice.Text = "Device ID:";
            // 
            // txtMSub
            // 
            this.txtMSub.Location = new System.Drawing.Point(191, 44);
            this.txtMSub.Name = "txtMSub";
            this.txtMSub.Size = new System.Drawing.Size(100, 22);
            this.txtMSub.TabIndex = 12;
            this.txtMSub.Text = "0";
            this.txtMSub.TextChanged += new System.EventHandler(this.txtMSub_TextChanged);
            this.txtMSub.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtASub_KeyPress);
            this.txtMSub.Leave += new System.EventHandler(this.txtMSub_Leave);
            // 
            // lbMSub
            // 
            this.lbMSub.AutoSize = true;
            this.lbMSub.Location = new System.Drawing.Point(98, 50);
            this.lbMSub.Name = "lbMSub";
            this.lbMSub.Size = new System.Drawing.Size(63, 14);
            this.lbMSub.TabIndex = 11;
            this.lbMSub.Text = "Subnet ID:";
            // 
            // btnManual
            // 
            this.btnManual.Location = new System.Drawing.Point(768, 44);
            this.btnManual.Name = "btnManual";
            this.btnManual.Size = new System.Drawing.Size(124, 28);
            this.btnManual.TabIndex = 10;
            this.btnManual.Text = "Manually Add";
            this.btnManual.UseVisualStyleBackColor = true;
            this.btnManual.Click += new System.EventHandler(this.btnManual_Click);
            // 
            // btnADV
            // 
            this.btnADV.Location = new System.Drawing.Point(770, 3);
            this.btnADV.Name = "btnADV";
            this.btnADV.Size = new System.Drawing.Size(124, 28);
            this.btnADV.TabIndex = 9;
            this.btnADV.Text = "Advance Search";
            this.btnADV.UseVisualStyleBackColor = true;
            this.btnADV.Click += new System.EventHandler(this.btnADV_Click);
            // 
            // txtADev2
            // 
            this.txtADev2.Location = new System.Drawing.Point(616, 6);
            this.txtADev2.Name = "txtADev2";
            this.txtADev2.Size = new System.Drawing.Size(100, 22);
            this.txtADev2.TabIndex = 8;
            this.txtADev2.Text = "0";
            this.txtADev2.TextChanged += new System.EventHandler(this.txtADev2_TextChanged);
            this.txtADev2.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtASub_KeyPress);
            this.txtADev2.Leave += new System.EventHandler(this.txtADev2_Leave);
            // 
            // txtADev1
            // 
            this.txtADev1.Location = new System.Drawing.Point(463, 6);
            this.txtADev1.Name = "txtADev1";
            this.txtADev1.Size = new System.Drawing.Size(100, 22);
            this.txtADev1.TabIndex = 7;
            this.txtADev1.Text = "0";
            this.txtADev1.TextChanged += new System.EventHandler(this.txtADev1_TextChanged);
            this.txtADev1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtASub_KeyPress);
            this.txtADev1.Leave += new System.EventHandler(this.txtADev1_Leave);
            // 
            // txtASub
            // 
            this.txtASub.Location = new System.Drawing.Point(191, 6);
            this.txtASub.Name = "txtASub";
            this.txtASub.Size = new System.Drawing.Size(100, 22);
            this.txtASub.TabIndex = 6;
            this.txtASub.Text = "0";
            this.txtASub.TextChanged += new System.EventHandler(this.txtASub_TextChanged);
            this.txtASub.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtASub_KeyPress);
            this.txtASub.Leave += new System.EventHandler(this.txtASub_Leave);
            // 
            // lbTo
            // 
            this.lbTo.AutoSize = true;
            this.lbTo.Location = new System.Drawing.Point(575, 10);
            this.lbTo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbTo.Name = "lbTo";
            this.lbTo.Size = new System.Drawing.Size(19, 14);
            this.lbTo.TabIndex = 4;
            this.lbTo.Text = "To";
            // 
            // lbDev
            // 
            this.lbDev.AutoSize = true;
            this.lbDev.Location = new System.Drawing.Point(332, 10);
            this.lbDev.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbDev.Name = "lbDev";
            this.lbDev.Size = new System.Drawing.Size(91, 14);
            this.lbDev.TabIndex = 2;
            this.lbDev.Text = "Device ID From:";
            // 
            // lbSub
            // 
            this.lbSub.AutoSize = true;
            this.lbSub.Location = new System.Drawing.Point(98, 10);
            this.lbSub.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbSub.Name = "lbSub";
            this.lbSub.Size = new System.Drawing.Size(63, 14);
            this.lbSub.TabIndex = 0;
            this.lbSub.Text = "Subnet ID:";
            // 
            // groupBox2
            // 
            this.groupBox2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.groupBox2.Controls.Add(this.btnStop);
            this.groupBox2.Controls.Add(this.lbCurrentDevValue);
            this.groupBox2.Controls.Add(this.lbCurrentDev);
            this.groupBox2.Controls.Add(this.lbCurrentSubValue);
            this.groupBox2.Controls.Add(this.lbCurrentSub);
            this.groupBox2.Controls.Add(this.lbCurrent);
            this.groupBox2.Controls.Add(this.Process);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox2.Location = new System.Drawing.Point(0, 85);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox2.Size = new System.Drawing.Size(910, 86);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Process";
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(770, 20);
            this.btnStop.Margin = new System.Windows.Forms.Padding(4);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(124, 28);
            this.btnStop.TabIndex = 6;
            this.btnStop.Text = "Stop Search";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // lbCurrentDevValue
            // 
            this.lbCurrentDevValue.AutoSize = true;
            this.lbCurrentDevValue.Location = new System.Drawing.Point(460, 25);
            this.lbCurrentDevValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbCurrentDevValue.Name = "lbCurrentDevValue";
            this.lbCurrentDevValue.Size = new System.Drawing.Size(13, 14);
            this.lbCurrentDevValue.TabIndex = 5;
            this.lbCurrentDevValue.Text = "0";
            // 
            // lbCurrentDev
            // 
            this.lbCurrentDev.AutoSize = true;
            this.lbCurrentDev.Location = new System.Drawing.Point(355, 25);
            this.lbCurrentDev.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbCurrentDev.Name = "lbCurrentDev";
            this.lbCurrentDev.Size = new System.Drawing.Size(61, 14);
            this.lbCurrentDev.TabIndex = 4;
            this.lbCurrentDev.Text = "Device ID:";
            // 
            // lbCurrentSubValue
            // 
            this.lbCurrentSubValue.AutoSize = true;
            this.lbCurrentSubValue.Location = new System.Drawing.Point(285, 25);
            this.lbCurrentSubValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbCurrentSubValue.Name = "lbCurrentSubValue";
            this.lbCurrentSubValue.Size = new System.Drawing.Size(13, 14);
            this.lbCurrentSubValue.TabIndex = 3;
            this.lbCurrentSubValue.Text = "0";
            // 
            // lbCurrentSub
            // 
            this.lbCurrentSub.AutoSize = true;
            this.lbCurrentSub.Location = new System.Drawing.Point(179, 25);
            this.lbCurrentSub.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbCurrentSub.Name = "lbCurrentSub";
            this.lbCurrentSub.Size = new System.Drawing.Size(63, 14);
            this.lbCurrentSub.TabIndex = 2;
            this.lbCurrentSub.Text = "Subnet ID:";
            // 
            // lbCurrent
            // 
            this.lbCurrent.AutoSize = true;
            this.lbCurrent.Location = new System.Drawing.Point(13, 25);
            this.lbCurrent.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbCurrent.Name = "lbCurrent";
            this.lbCurrent.Size = new System.Drawing.Size(88, 14);
            this.lbCurrent.TabIndex = 1;
            this.lbCurrent.Text = "Current Search:";
            // 
            // Process
            // 
            this.Process.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.Process.Location = new System.Drawing.Point(4, 54);
            this.Process.Margin = new System.Windows.Forms.Padding(4);
            this.Process.Name = "Process";
            this.Process.Size = new System.Drawing.Size(902, 28);
            this.Process.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.groupBox1.Controls.Add(this.dgvResult);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 171);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(910, 241);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Result";
            // 
            // dgvResult
            // 
            this.dgvResult.AllowUserToAddRows = false;
            this.dgvResult.AllowUserToDeleteRows = false;
            this.dgvResult.AllowUserToResizeColumns = false;
            this.dgvResult.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.AliceBlue;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvResult.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvResult.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvResult.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.cl1,
            this.cl2,
            this.cl3,
            this.cl4,
            this.cl5,
            this.cl6,
            this.DeviceType});
            this.dgvResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvResult.EnableHeadersVisualStyles = false;
            this.dgvResult.Location = new System.Drawing.Point(4, 19);
            this.dgvResult.Margin = new System.Windows.Forms.Padding(4);
            this.dgvResult.Name = "dgvResult";
            this.dgvResult.RowHeadersVisible = false;
            this.dgvResult.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvResult.Size = new System.Drawing.Size(902, 218);
            this.dgvResult.TabIndex = 0;
            this.dgvResult.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvResult_CellMouseDoubleClick);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "1453215663_button_ok.png");
            this.imageList1.Images.SetKeyName(1, "1454138575_close_square_black.png");
            // 
            // cl1
            // 
            this.cl1.HeaderText = "Status";
            this.cl1.Name = "cl1";
            this.cl1.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.cl1.Width = 60;
            // 
            // cl2
            // 
            this.cl2.HeaderText = "Subnet ID";
            this.cl2.Name = "cl2";
            this.cl2.ReadOnly = true;
            this.cl2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // cl3
            // 
            this.cl3.HeaderText = "Device ID";
            this.cl3.Name = "cl3";
            this.cl3.ReadOnly = true;
            this.cl3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // cl4
            // 
            this.cl4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.cl4.HeaderText = "Name";
            this.cl4.Name = "cl4";
            this.cl4.ReadOnly = true;
            this.cl4.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // cl5
            // 
            this.cl5.HeaderText = "Description";
            this.cl5.Name = "cl5";
            this.cl5.ReadOnly = true;
            this.cl5.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.cl5.Width = 250;
            // 
            // cl6
            // 
            this.cl6.HeaderText = "Model";
            this.cl6.Name = "cl6";
            this.cl6.ReadOnly = true;
            this.cl6.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.cl6.Width = 150;
            // 
            // DeviceType
            // 
            this.DeviceType.HeaderText = "DeviceType";
            this.DeviceType.Name = "DeviceType";
            this.DeviceType.Visible = false;
            // 
            // FrmADVSearchDevice
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(910, 412);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmADVSearchDevice";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Advance Search";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmADVSearchDevice_FormClosing);
            this.Load += new System.EventHandler(this.FrmADVSearchDevice_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvResult)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lbTo;
        private System.Windows.Forms.Label lbDev;
        private System.Windows.Forms.Label lbSub;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label lbCurrentDevValue;
        private System.Windows.Forms.Label lbCurrentDev;
        private System.Windows.Forms.Label lbCurrentSubValue;
        private System.Windows.Forms.Label lbCurrentSub;
        private System.Windows.Forms.Label lbCurrent;
        private System.Windows.Forms.ProgressBar Process;
        private System.Windows.Forms.DataGridView dgvResult;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Label lbM;
        private System.Windows.Forms.Label lbA;
        private System.Windows.Forms.TextBox txtMDev;
        private System.Windows.Forms.Label lbMDevice;
        private System.Windows.Forms.TextBox txtMSub;
        private System.Windows.Forms.Label lbMSub;
        private System.Windows.Forms.Button btnManual;
        private System.Windows.Forms.Button btnADV;
        private System.Windows.Forms.TextBox txtADev2;
        private System.Windows.Forms.TextBox txtADev1;
        private System.Windows.Forms.TextBox txtASub;
        private System.Windows.Forms.DataGridViewCheckBoxColumn cl1;
        private System.Windows.Forms.DataGridViewTextBoxColumn cl2;
        private System.Windows.Forms.DataGridViewTextBoxColumn cl3;
        private System.Windows.Forms.DataGridViewTextBoxColumn cl4;
        private System.Windows.Forms.DataGridViewTextBoxColumn cl5;
        private System.Windows.Forms.DataGridViewTextBoxColumn cl6;
        private System.Windows.Forms.DataGridViewTextBoxColumn DeviceType;
    }
}