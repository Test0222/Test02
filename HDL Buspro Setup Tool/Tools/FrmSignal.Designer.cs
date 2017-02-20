namespace HDL_Buspro_Setup_Tool
{
    partial class FrmSignal
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmSignal));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnAdd = new System.Windows.Forms.Button();
            this.lb2 = new System.Windows.Forms.Label();
            this.numDev = new System.Windows.Forms.NumericUpDown();
            this.numSub = new System.Windows.Forms.NumericUpDown();
            this.lb3 = new System.Windows.Forms.Label();
            this.lb1 = new System.Windows.Forms.Label();
            this.panel5 = new System.Windows.Forms.Panel();
            this.btnExport = new System.Windows.Forms.Button();
            this.lbSingal = new System.Windows.Forms.Label();
            this.lb10 = new System.Windows.Forms.Label();
            this.lbLost = new System.Windows.Forms.Label();
            this.lb9 = new System.Windows.Forms.Label();
            this.lbReceiveCount = new System.Windows.Forms.Label();
            this.lb8 = new System.Windows.Forms.Label();
            this.lbSendCount = new System.Windows.Forms.Label();
            this.lb7 = new System.Windows.Forms.Label();
            this.grp2 = new System.Windows.Forms.GroupBox();
            this.NumCount = new System.Windows.Forms.NumericUpDown();
            this.txtTime = new System.Windows.Forms.TextBox();
            this.lb6 = new System.Windows.Forms.Label();
            this.lb5 = new System.Windows.Forms.Label();
            this.lb4 = new System.Windows.Forms.Label();
            this.btnStart = new System.Windows.Forms.Button();
            this.grp1 = new System.Windows.Forms.GroupBox();
            this.dgvDevice = new System.Windows.Forms.DataGridView();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.grp4 = new System.Windows.Forms.GroupBox();
            this.dgvCommands = new System.Windows.Forms.RichTextBox();
            this.cl1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cl2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CL3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cl4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cl5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cl6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cl7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cl8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numDev)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSub)).BeginInit();
            this.panel5.SuspendLayout();
            this.grp2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumCount)).BeginInit();
            this.grp1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDevice)).BeginInit();
            this.grp4.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnAdd);
            this.panel1.Controls.Add(this.lb2);
            this.panel1.Controls.Add(this.numDev);
            this.panel1.Controls.Add(this.numSub);
            this.panel1.Controls.Add(this.lb3);
            this.panel1.Controls.Add(this.lb1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(714, 43);
            this.panel1.TabIndex = 3;
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAdd.Image = ((System.Drawing.Image)(resources.GetObject("btnAdd.Image")));
            this.btnAdd.Location = new System.Drawing.Point(539, 9);
            this.btnAdd.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(112, 25);
            this.btnAdd.TabIndex = 160;
            this.btnAdd.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnAdd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // lb2
            // 
            this.lb2.AutoSize = true;
            this.lb2.Location = new System.Drawing.Point(151, 16);
            this.lb2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lb2.Name = "lb2";
            this.lb2.Size = new System.Drawing.Size(63, 14);
            this.lb2.TabIndex = 156;
            this.lb2.Text = "Subnet ID:";
            // 
            // numDev
            // 
            this.numDev.Location = new System.Drawing.Point(417, 12);
            this.numDev.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.numDev.Maximum = new decimal(new int[] {
            254,
            0,
            0,
            0});
            this.numDev.Name = "numDev";
            this.numDev.Size = new System.Drawing.Size(80, 22);
            this.numDev.TabIndex = 159;
            this.numDev.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // numSub
            // 
            this.numSub.Location = new System.Drawing.Point(220, 12);
            this.numSub.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.numSub.Maximum = new decimal(new int[] {
            254,
            0,
            0,
            0});
            this.numSub.Name = "numSub";
            this.numSub.Size = new System.Drawing.Size(80, 22);
            this.numSub.TabIndex = 158;
            this.numSub.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lb3
            // 
            this.lb3.AutoSize = true;
            this.lb3.Location = new System.Drawing.Point(353, 16);
            this.lb3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lb3.Name = "lb3";
            this.lb3.Size = new System.Drawing.Size(61, 14);
            this.lb3.TabIndex = 157;
            this.lb3.Text = "Device ID:";
            // 
            // lb1
            // 
            this.lb1.AutoSize = true;
            this.lb1.Location = new System.Drawing.Point(11, 16);
            this.lb1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lb1.Name = "lb1";
            this.lb1.Size = new System.Drawing.Size(124, 14);
            this.lb1.TabIndex = 3;
            this.lb1.Text = "Manually Add Device:";
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.btnExport);
            this.panel5.Controls.Add(this.lbSingal);
            this.panel5.Controls.Add(this.lb10);
            this.panel5.Controls.Add(this.lbLost);
            this.panel5.Controls.Add(this.lb9);
            this.panel5.Controls.Add(this.lbReceiveCount);
            this.panel5.Controls.Add(this.lb8);
            this.panel5.Controls.Add(this.lbSendCount);
            this.panel5.Controls.Add(this.lb7);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel5.Location = new System.Drawing.Point(2, 18);
            this.panel5.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(710, 32);
            this.panel5.TabIndex = 2;
            // 
            // btnExport
            // 
            this.btnExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExport.Location = new System.Drawing.Point(537, 1);
            this.btnExport.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(112, 25);
            this.btnExport.TabIndex = 130;
            this.btnExport.Text = "Export List";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // lbSingal
            // 
            this.lbSingal.AutoSize = true;
            this.lbSingal.Location = new System.Drawing.Point(488, 9);
            this.lbSingal.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbSingal.Name = "lbSingal";
            this.lbSingal.Size = new System.Drawing.Size(13, 14);
            this.lbSingal.TabIndex = 7;
            this.lbSingal.Text = "0";
            // 
            // lb10
            // 
            this.lb10.AutoSize = true;
            this.lb10.Location = new System.Drawing.Point(398, 9);
            this.lb10.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lb10.Name = "lb10";
            this.lb10.Size = new System.Drawing.Size(90, 14);
            this.lb10.TabIndex = 6;
            this.lb10.Text = "Average Singal:";
            // 
            // lbLost
            // 
            this.lbLost.AutoSize = true;
            this.lbLost.Location = new System.Drawing.Point(295, 9);
            this.lbLost.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbLost.Name = "lbLost";
            this.lbLost.Size = new System.Drawing.Size(13, 14);
            this.lbLost.TabIndex = 5;
            this.lbLost.Text = "0";
            // 
            // lb9
            // 
            this.lb9.AutoSize = true;
            this.lb9.Location = new System.Drawing.Point(254, 9);
            this.lb9.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lb9.Name = "lb9";
            this.lb9.Size = new System.Drawing.Size(32, 14);
            this.lb9.TabIndex = 4;
            this.lb9.Text = "Lost:";
            // 
            // lbReceiveCount
            // 
            this.lbReceiveCount.AutoSize = true;
            this.lbReceiveCount.Location = new System.Drawing.Point(176, 9);
            this.lbReceiveCount.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbReceiveCount.Name = "lbReceiveCount";
            this.lbReceiveCount.Size = new System.Drawing.Size(13, 14);
            this.lbReceiveCount.TabIndex = 3;
            this.lbReceiveCount.Text = "0";
            // 
            // lb8
            // 
            this.lb8.AutoSize = true;
            this.lb8.Location = new System.Drawing.Point(110, 9);
            this.lb8.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lb8.Name = "lb8";
            this.lb8.Size = new System.Drawing.Size(59, 14);
            this.lb8.TabIndex = 2;
            this.lb8.Text = "Received:";
            // 
            // lbSendCount
            // 
            this.lbSendCount.AutoSize = true;
            this.lbSendCount.Location = new System.Drawing.Point(52, 8);
            this.lbSendCount.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbSendCount.Name = "lbSendCount";
            this.lbSendCount.Size = new System.Drawing.Size(13, 14);
            this.lbSendCount.TabIndex = 1;
            this.lbSendCount.Text = "0";
            // 
            // lb7
            // 
            this.lb7.AutoSize = true;
            this.lb7.Location = new System.Drawing.Point(8, 8);
            this.lb7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lb7.Name = "lb7";
            this.lb7.Size = new System.Drawing.Size(37, 14);
            this.lb7.TabIndex = 0;
            this.lb7.Text = "Send:";
            // 
            // grp2
            // 
            this.grp2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.grp2.Controls.Add(this.NumCount);
            this.grp2.Controls.Add(this.txtTime);
            this.grp2.Controls.Add(this.lb6);
            this.grp2.Controls.Add(this.lb5);
            this.grp2.Controls.Add(this.lb4);
            this.grp2.Controls.Add(this.btnStart);
            this.grp2.Dock = System.Windows.Forms.DockStyle.Top;
            this.grp2.Location = new System.Drawing.Point(0, 229);
            this.grp2.Margin = new System.Windows.Forms.Padding(4);
            this.grp2.Name = "grp2";
            this.grp2.Padding = new System.Windows.Forms.Padding(4);
            this.grp2.Size = new System.Drawing.Size(714, 46);
            this.grp2.TabIndex = 0;
            this.grp2.TabStop = false;
            this.grp2.Text = "Test";
            // 
            // NumCount
            // 
            this.NumCount.Location = new System.Drawing.Point(115, 18);
            this.NumCount.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.NumCount.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.NumCount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.NumCount.Name = "NumCount";
            this.NumCount.Size = new System.Drawing.Size(73, 22);
            this.NumCount.TabIndex = 134;
            this.NumCount.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // txtTime
            // 
            this.txtTime.Location = new System.Drawing.Point(275, 18);
            this.txtTime.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.txtTime.Name = "txtTime";
            this.txtTime.Size = new System.Drawing.Size(68, 22);
            this.txtTime.TabIndex = 133;
            this.txtTime.Text = "100";
            this.txtTime.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtTime_KeyPress);
            this.txtTime.Leave += new System.EventHandler(this.txtTime_Leave);
            // 
            // lb6
            // 
            this.lb6.AutoSize = true;
            this.lb6.Location = new System.Drawing.Point(347, 20);
            this.lb6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lb6.Name = "lb6";
            this.lb6.Size = new System.Drawing.Size(31, 14);
            this.lb6.TabIndex = 132;
            this.lb6.Text = "(MS)";
            // 
            // lb5
            // 
            this.lb5.AutoSize = true;
            this.lb5.Location = new System.Drawing.Point(206, 20);
            this.lb5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lb5.Name = "lb5";
            this.lb5.Size = new System.Drawing.Size(52, 14);
            this.lb5.TabIndex = 131;
            this.lb5.Text = "Interval:";
            // 
            // lb4
            // 
            this.lb4.AutoSize = true;
            this.lb4.Location = new System.Drawing.Point(8, 19);
            this.lb4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lb4.Name = "lb4";
            this.lb4.Size = new System.Drawing.Size(98, 14);
            this.lb4.TabIndex = 1;
            this.lb4.Text = "Command Count:";
            // 
            // btnStart
            // 
            this.btnStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStart.Location = new System.Drawing.Point(539, 14);
            this.btnStart.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(112, 25);
            this.btnStart.TabIndex = 0;
            this.btnStart.Text = "Start Test";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // grp1
            // 
            this.grp1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.grp1.Controls.Add(this.dgvDevice);
            this.grp1.Dock = System.Windows.Forms.DockStyle.Top;
            this.grp1.Location = new System.Drawing.Point(0, 43);
            this.grp1.Margin = new System.Windows.Forms.Padding(4);
            this.grp1.Name = "grp1";
            this.grp1.Padding = new System.Windows.Forms.Padding(4);
            this.grp1.Size = new System.Drawing.Size(714, 186);
            this.grp1.TabIndex = 0;
            this.grp1.TabStop = false;
            this.grp1.Text = "Online Wireless Device";
            // 
            // dgvDevice
            // 
            this.dgvDevice.AllowUserToAddRows = false;
            this.dgvDevice.AllowUserToDeleteRows = false;
            this.dgvDevice.AllowUserToResizeRows = false;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.AliceBlue;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvDevice.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvDevice.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDevice.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.cl1,
            this.cl2,
            this.CL3,
            this.cl4,
            this.cl5,
            this.cl6,
            this.cl7,
            this.cl8});
            this.dgvDevice.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvDevice.EnableHeadersVisualStyles = false;
            this.dgvDevice.Location = new System.Drawing.Point(4, 19);
            this.dgvDevice.Margin = new System.Windows.Forms.Padding(4);
            this.dgvDevice.MultiSelect = false;
            this.dgvDevice.Name = "dgvDevice";
            this.dgvDevice.RowHeadersVisible = false;
            this.dgvDevice.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvDevice.Size = new System.Drawing.Size(706, 163);
            this.dgvDevice.TabIndex = 3;
            this.dgvDevice.SelectionChanged += new System.EventHandler(this.dgvDevice_SelectionChanged);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.Filter = "TXT File|*.txt";
            this.saveFileDialog1.RestoreDirectory = true;
            // 
            // grp4
            // 
            this.grp4.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.grp4.Controls.Add(this.dgvCommands);
            this.grp4.Controls.Add(this.panel5);
            this.grp4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grp4.Location = new System.Drawing.Point(0, 275);
            this.grp4.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.grp4.Name = "grp4";
            this.grp4.Padding = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.grp4.Size = new System.Drawing.Size(714, 228);
            this.grp4.TabIndex = 0;
            this.grp4.TabStop = false;
            this.grp4.Text = "Received Commands";
            // 
            // dgvCommands
            // 
            this.dgvCommands.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvCommands.Location = new System.Drawing.Point(2, 50);
            this.dgvCommands.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.dgvCommands.Name = "dgvCommands";
            this.dgvCommands.Size = new System.Drawing.Size(710, 175);
            this.dgvCommands.TabIndex = 0;
            this.dgvCommands.Text = "";
            // 
            // cl1
            // 
            this.cl1.HeaderText = "ID";
            this.cl1.Name = "cl1";
            this.cl1.ReadOnly = true;
            this.cl1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.cl1.Width = 60;
            // 
            // cl2
            // 
            this.cl2.HeaderText = "Subnet ID";
            this.cl2.Name = "cl2";
            this.cl2.ReadOnly = true;
            this.cl2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.cl2.Width = 80;
            // 
            // CL3
            // 
            this.CL3.HeaderText = "Device ID";
            this.CL3.Name = "CL3";
            this.CL3.ReadOnly = true;
            this.CL3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.CL3.Width = 80;
            // 
            // cl4
            // 
            this.cl4.HeaderText = "Name";
            this.cl4.Name = "cl4";
            this.cl4.ReadOnly = true;
            this.cl4.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.cl4.Width = 200;
            // 
            // cl5
            // 
            this.cl5.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.cl5.HeaderText = "Description";
            this.cl5.Name = "cl5";
            this.cl5.ReadOnly = true;
            this.cl5.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // cl6
            // 
            this.cl6.HeaderText = "Model";
            this.cl6.Name = "cl6";
            this.cl6.Width = 150;
            // 
            // cl7
            // 
            this.cl7.HeaderText = "DeviceType";
            this.cl7.Name = "cl7";
            this.cl7.ReadOnly = true;
            this.cl7.Visible = false;
            this.cl7.Width = 60;
            // 
            // cl8
            // 
            this.cl8.HeaderText = "DIndex";
            this.cl8.Name = "cl8";
            this.cl8.ReadOnly = true;
            this.cl8.Visible = false;
            this.cl8.Width = 60;
            // 
            // FrmSignal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(714, 503);
            this.Controls.Add(this.grp4);
            this.Controls.Add(this.grp2);
            this.Controls.Add(this.grp1);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FrmSignal";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Signal Strength Test";
            this.Load += new System.EventHandler(this.FrmSingnal_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numDev)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSub)).EndInit();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.grp2.ResumeLayout(false);
            this.grp2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumCount)).EndInit();
            this.grp1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDevice)).EndInit();
            this.grp4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox grp2;
        private System.Windows.Forms.GroupBox grp1;
        private System.Windows.Forms.DataGridView dgvDevice;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Label lbSingal;
        private System.Windows.Forms.Label lb10;
        private System.Windows.Forms.Label lbLost;
        private System.Windows.Forms.Label lb9;
        private System.Windows.Forms.Label lbReceiveCount;
        private System.Windows.Forms.Label lb8;
        private System.Windows.Forms.Label lbSendCount;
        private System.Windows.Forms.Label lb7;
        private System.Windows.Forms.Label lb4;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.TextBox txtTime;
        private System.Windows.Forms.Label lb6;
        private System.Windows.Forms.Label lb5;
        private System.Windows.Forms.NumericUpDown NumCount;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Label lb1;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Label lb2;
        private System.Windows.Forms.NumericUpDown numDev;
        private System.Windows.Forms.NumericUpDown numSub;
        private System.Windows.Forms.Label lb3;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.GroupBox grp4;
        private System.Windows.Forms.RichTextBox dgvCommands;
        private System.Windows.Forms.DataGridViewTextBoxColumn cl1;
        private System.Windows.Forms.DataGridViewTextBoxColumn cl2;
        private System.Windows.Forms.DataGridViewTextBoxColumn CL3;
        private System.Windows.Forms.DataGridViewTextBoxColumn cl4;
        private System.Windows.Forms.DataGridViewTextBoxColumn cl5;
        private System.Windows.Forms.DataGridViewTextBoxColumn cl6;
        private System.Windows.Forms.DataGridViewTextBoxColumn cl7;
        private System.Windows.Forms.DataGridViewTextBoxColumn cl8;

    }
}