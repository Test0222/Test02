namespace HDL_Buspro_Setup_Tool
{
    partial class frmSA
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSA));
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.tslHint = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsl2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsbHint1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.HintBar = new System.Windows.Forms.ToolStripProgressBar();
            this.tsbl4 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tslValue = new System.Windows.Forms.ToolStripStatusLabel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabAuto = new System.Windows.Forms.TabPage();
            this.gbListA = new System.Windows.Forms.GroupBox();
            this.txtFrm = new System.Windows.Forms.NumericUpDown();
            this.TX = new System.Windows.Forms.RadioButton();
            this.RX = new System.Windows.Forms.RadioButton();
            this.btnSure = new System.Windows.Forms.Button();
            this.lbTarget = new System.Windows.Forms.Label();
            this.dgvListA = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnFindA = new System.Windows.Forms.Button();
            this.gbDevA = new System.Windows.Forms.GroupBox();
            this.btnRead1A = new System.Windows.Forms.Button();
            this.numDevA = new System.Windows.Forms.NumericUpDown();
            this.lbDevIDA = new System.Windows.Forms.Label();
            this.numSub = new System.Windows.Forms.NumericUpDown();
            this.lbDevMA = new System.Windows.Forms.Label();
            this.btnGetDev = new System.Windows.Forms.Button();
            this.cboDevA = new System.Windows.Forms.ComboBox();
            this.lbDevA = new System.Windows.Forms.Label();
            this.statusStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabAuto.SuspendLayout();
            this.gbListA.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtFrm)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvListA)).BeginInit();
            this.panel1.SuspendLayout();
            this.gbDevA.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numDevA)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSub)).BeginInit();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Font = new System.Drawing.Font("Arial", 8.25F);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tslHint,
            this.tsl2,
            this.tsbHint1,
            this.HintBar,
            this.tsbl4,
            this.tslValue});
            this.statusStrip1.Location = new System.Drawing.Point(0, 495);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 16, 0);
            this.statusStrip1.Size = new System.Drawing.Size(677, 26);
            this.statusStrip1.TabIndex = 41;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // tslHint
            // 
            this.tslHint.AutoSize = false;
            this.tslHint.Name = "tslHint";
            this.tslHint.Size = new System.Drawing.Size(150, 21);
            this.tslHint.Text = "Current Device:";
            // 
            // tsl2
            // 
            this.tsl2.Name = "tsl2";
            this.tsl2.Size = new System.Drawing.Size(9, 21);
            this.tsl2.Text = "|";
            // 
            // tsbHint1
            // 
            this.tsbHint1.AutoSize = false;
            this.tsbHint1.Name = "tsbHint1";
            this.tsbHint1.Size = new System.Drawing.Size(160, 21);
            // 
            // HintBar
            // 
            this.HintBar.AutoSize = false;
            this.HintBar.Name = "HintBar";
            this.HintBar.Size = new System.Drawing.Size(233, 20);
            this.HintBar.Visible = false;
            // 
            // tsbl4
            // 
            this.tsbl4.Name = "tsbl4";
            this.tsbl4.Size = new System.Drawing.Size(0, 21);
            // 
            // tslValue
            // 
            this.tslValue.AutoSize = false;
            this.tslValue.Name = "tslValue";
            this.tslValue.Size = new System.Drawing.Size(30, 21);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabAuto);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(677, 495);
            this.tabControl1.TabIndex = 42;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tabAuto
            // 
            this.tabAuto.BackColor = System.Drawing.SystemColors.Control;
            this.tabAuto.Controls.Add(this.gbListA);
            this.tabAuto.Controls.Add(this.gbDevA);
            this.tabAuto.Location = new System.Drawing.Point(4, 23);
            this.tabAuto.Name = "tabAuto";
            this.tabAuto.Padding = new System.Windows.Forms.Padding(3);
            this.tabAuto.Size = new System.Drawing.Size(669, 468);
            this.tabAuto.TabIndex = 0;
            this.tabAuto.Text = "Ip control boxes list";
            // 
            // gbListA
            // 
            this.gbListA.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.gbListA.Controls.Add(this.txtFrm);
            this.gbListA.Controls.Add(this.TX);
            this.gbListA.Controls.Add(this.RX);
            this.gbListA.Controls.Add(this.btnSure);
            this.gbListA.Controls.Add(this.lbTarget);
            this.gbListA.Controls.Add(this.dgvListA);
            this.gbListA.Controls.Add(this.panel1);
            this.gbListA.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbListA.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbListA.ForeColor = System.Drawing.Color.Blue;
            this.gbListA.Location = new System.Drawing.Point(3, 89);
            this.gbListA.Name = "gbListA";
            this.gbListA.Size = new System.Drawing.Size(663, 376);
            this.gbListA.TabIndex = 1;
            this.gbListA.TabStop = false;
            this.gbListA.Text = "List";
            // 
            // txtFrm
            // 
            this.txtFrm.Location = new System.Drawing.Point(300, 19);
            this.txtFrm.Maximum = new decimal(new int[] {
            24,
            0,
            0,
            0});
            this.txtFrm.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.txtFrm.Name = "txtFrm";
            this.txtFrm.Size = new System.Drawing.Size(71, 23);
            this.txtFrm.TabIndex = 155;
            this.txtFrm.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // TX
            // 
            this.TX.AutoSize = true;
            this.TX.Location = new System.Drawing.Point(154, 21);
            this.TX.Name = "TX";
            this.TX.Size = new System.Drawing.Size(38, 19);
            this.TX.TabIndex = 154;
            this.TX.Text = "TX";
            this.TX.UseVisualStyleBackColor = true;
            // 
            // RX
            // 
            this.RX.AutoSize = true;
            this.RX.Checked = true;
            this.RX.Location = new System.Drawing.Point(235, 21);
            this.RX.Name = "RX";
            this.RX.Size = new System.Drawing.Size(39, 19);
            this.RX.TabIndex = 153;
            this.RX.TabStop = true;
            this.RX.Text = "RX";
            this.RX.UseVisualStyleBackColor = true;
            // 
            // btnSure
            // 
            this.btnSure.Location = new System.Drawing.Point(377, 18);
            this.btnSure.Name = "btnSure";
            this.btnSure.Size = new System.Drawing.Size(120, 24);
            this.btnSure.TabIndex = 152;
            this.btnSure.Text = "OK";
            this.btnSure.UseVisualStyleBackColor = true;
            this.btnSure.Click += new System.EventHandler(this.btnSure_Click);
            // 
            // lbTarget
            // 
            this.lbTarget.AutoSize = true;
            this.lbTarget.Location = new System.Drawing.Point(41, 23);
            this.lbTarget.Name = "lbTarget";
            this.lbTarget.Size = new System.Drawing.Size(71, 15);
            this.lbTarget.TabIndex = 147;
            this.lbTarget.Text = "Add Source:";
            // 
            // dgvListA
            // 
            this.dgvListA.AllowUserToAddRows = false;
            this.dgvListA.AllowUserToDeleteRows = false;
            this.dgvListA.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.AliceBlue;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvListA.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvListA.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvListA.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3,
            this.Column5});
            this.dgvListA.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dgvListA.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgvListA.EnableHeadersVisualStyles = false;
            this.dgvListA.Location = new System.Drawing.Point(3, 49);
            this.dgvListA.MultiSelect = false;
            this.dgvListA.Name = "dgvListA";
            this.dgvListA.RowHeadersVisible = false;
            this.dgvListA.RowHeadersWidth = 10;
            this.dgvListA.RowTemplate.Height = 23;
            this.dgvListA.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvListA.Size = new System.Drawing.Size(657, 284);
            this.dgvListA.TabIndex = 146;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "";
            this.Column1.Name = "Column1";
            this.Column1.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Column1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.Column1.Width = 32;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "ID";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            this.Column2.Width = 64;
            // 
            // Column3
            // 
            this.Column3.HeaderText = "Type";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            this.Column3.Width = 200;
            // 
            // Column5
            // 
            this.Column5.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column5.HeaderText = "Remark";
            this.Column5.Name = "Column5";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnFindA);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(3, 333);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(657, 40);
            this.panel1.TabIndex = 143;
            // 
            // btnFindA
            // 
            this.btnFindA.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFindA.Image = global::HDL_Buspro_Setup_Tool.Properties.Resources.Save;
            this.btnFindA.Location = new System.Drawing.Point(510, 8);
            this.btnFindA.Name = "btnFindA";
            this.btnFindA.Size = new System.Drawing.Size(138, 26);
            this.btnFindA.TabIndex = 139;
            this.btnFindA.Text = "...";
            this.btnFindA.UseVisualStyleBackColor = true;
            this.btnFindA.Click += new System.EventHandler(this.btnFindA_Click);
            // 
            // gbDevA
            // 
            this.gbDevA.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.gbDevA.Controls.Add(this.btnRead1A);
            this.gbDevA.Controls.Add(this.numDevA);
            this.gbDevA.Controls.Add(this.lbDevIDA);
            this.gbDevA.Controls.Add(this.numSub);
            this.gbDevA.Controls.Add(this.lbDevMA);
            this.gbDevA.Controls.Add(this.btnGetDev);
            this.gbDevA.Controls.Add(this.cboDevA);
            this.gbDevA.Controls.Add(this.lbDevA);
            this.gbDevA.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbDevA.Location = new System.Drawing.Point(3, 3);
            this.gbDevA.Name = "gbDevA";
            this.gbDevA.Size = new System.Drawing.Size(663, 86);
            this.gbDevA.TabIndex = 0;
            this.gbDevA.TabStop = false;
            this.gbDevA.Text = "Select Device";
            // 
            // btnRead1A
            // 
            this.btnRead1A.Location = new System.Drawing.Point(513, 48);
            this.btnRead1A.Name = "btnRead1A";
            this.btnRead1A.Size = new System.Drawing.Size(138, 26);
            this.btnRead1A.TabIndex = 138;
            this.btnRead1A.Text = "OK";
            this.btnRead1A.UseVisualStyleBackColor = true;
            this.btnRead1A.Click += new System.EventHandler(this.btnRead1A_Click);
            // 
            // numDevA
            // 
            this.numDevA.Location = new System.Drawing.Point(377, 50);
            this.numDevA.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numDevA.Name = "numDevA";
            this.numDevA.Size = new System.Drawing.Size(120, 22);
            this.numDevA.TabIndex = 131;
            this.numDevA.ValueChanged += new System.EventHandler(this.numDevA_ValueChanged);
            // 
            // lbDevIDA
            // 
            this.lbDevIDA.AutoSize = true;
            this.lbDevIDA.ForeColor = System.Drawing.Color.Black;
            this.lbDevIDA.Location = new System.Drawing.Point(297, 54);
            this.lbDevIDA.Name = "lbDevIDA";
            this.lbDevIDA.Size = new System.Drawing.Size(61, 14);
            this.lbDevIDA.TabIndex = 130;
            this.lbDevIDA.Text = "Device ID:";
            // 
            // numSub
            // 
            this.numSub.Location = new System.Drawing.Point(154, 50);
            this.numSub.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numSub.Name = "numSub";
            this.numSub.Size = new System.Drawing.Size(120, 22);
            this.numSub.TabIndex = 129;
            this.numSub.ValueChanged += new System.EventHandler(this.numSub_ValueChanged);
            // 
            // lbDevMA
            // 
            this.lbDevMA.AutoSize = true;
            this.lbDevMA.ForeColor = System.Drawing.Color.Black;
            this.lbDevMA.Location = new System.Drawing.Point(41, 54);
            this.lbDevMA.Name = "lbDevMA";
            this.lbDevMA.Size = new System.Drawing.Size(64, 14);
            this.lbDevMA.TabIndex = 128;
            this.lbDevMA.Text = "SubNet ID:";
            // 
            // btnGetDev
            // 
            this.btnGetDev.Location = new System.Drawing.Point(513, 20);
            this.btnGetDev.Name = "btnGetDev";
            this.btnGetDev.Size = new System.Drawing.Size(138, 26);
            this.btnGetDev.TabIndex = 126;
            this.btnGetDev.Text = "Refresh Device List";
            this.btnGetDev.UseVisualStyleBackColor = true;
            this.btnGetDev.Click += new System.EventHandler(this.btnReadA_Click);
            // 
            // cboDevA
            // 
            this.cboDevA.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDevA.FormattingEnabled = true;
            this.cboDevA.Location = new System.Drawing.Point(154, 22);
            this.cboDevA.Name = "cboDevA";
            this.cboDevA.Size = new System.Drawing.Size(343, 22);
            this.cboDevA.TabIndex = 125;
            this.cboDevA.SelectedIndexChanged += new System.EventHandler(this.cboDevA_SelectedIndexChanged);
            // 
            // lbDevA
            // 
            this.lbDevA.AutoSize = true;
            this.lbDevA.ForeColor = System.Drawing.Color.Black;
            this.lbDevA.Location = new System.Drawing.Point(41, 26);
            this.lbDevA.Name = "lbDevA";
            this.lbDevA.Size = new System.Drawing.Size(46, 14);
            this.lbDevA.TabIndex = 124;
            this.lbDevA.Text = "Device:";
            this.lbDevA.Click += new System.EventHandler(this.lbDevA_Click);
            // 
            // frmSA
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(677, 521);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.statusStrip1);
            this.Font = new System.Drawing.Font("Calibri", 9F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmSA";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Ip Control Helper";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmUpgrade_FormClosing);
            this.Load += new System.EventHandler(this.frmUpgrade_Load);
            this.Shown += new System.EventHandler(this.frmQC_Shown);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabAuto.ResumeLayout(false);
            this.gbListA.ResumeLayout(false);
            this.gbListA.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtFrm)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvListA)).EndInit();
            this.panel1.ResumeLayout(false);
            this.gbDevA.ResumeLayout(false);
            this.gbDevA.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numDevA)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSub)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel tslHint;
        private System.Windows.Forms.ToolStripStatusLabel tsl2;
        private System.Windows.Forms.ToolStripStatusLabel tsbHint1;
        private System.Windows.Forms.ToolStripProgressBar HintBar;
        private System.Windows.Forms.ToolStripStatusLabel tsbl4;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabAuto;
        private System.Windows.Forms.GroupBox gbDevA;
        private System.Windows.Forms.ComboBox cboDevA;
        private System.Windows.Forms.Label lbDevA;
        private System.Windows.Forms.Button btnGetDev;
        private System.Windows.Forms.NumericUpDown numDevA;
        private System.Windows.Forms.Label lbDevIDA;
        private System.Windows.Forms.NumericUpDown numSub;
        private System.Windows.Forms.Label lbDevMA;
        private System.Windows.Forms.GroupBox gbListA;
        private System.Windows.Forms.Button btnRead1A;
        private System.Windows.Forms.ToolStripStatusLabel tslValue;
        private System.Windows.Forms.DataGridView dgvListA;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnFindA;
        private System.Windows.Forms.Button btnSure;
        private System.Windows.Forms.Label lbTarget;
        private System.Windows.Forms.RadioButton TX;
        private System.Windows.Forms.RadioButton RX;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.NumericUpDown txtFrm;
    }
}