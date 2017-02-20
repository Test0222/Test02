namespace HDL_Buspro_Setup_Tool
{
    partial class frmMAC
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMAC));
            this.panel1 = new System.Windows.Forms.Panel();
            this.grpMAC = new System.Windows.Forms.GroupBox();
            this.tbMACMan = new System.Windows.Forms.MaskedTextBox();
            this.tbMACAuto = new System.Windows.Forms.MaskedTextBox();
            this.btnGetDev = new System.Windows.Forms.Button();
            this.cboDevA = new System.Windows.Forms.ComboBox();
            this.lbDevA = new System.Windows.Forms.Label();
            this.numDev = new System.Windows.Forms.NumericUpDown();
            this.lbDevID = new System.Windows.Forms.Label();
            this.numSub = new System.Windows.Forms.NumericUpDown();
            this.lbInitial = new System.Windows.Forms.Label();
            this.btnInitial = new System.Windows.Forms.Button();
            this.tbHint3 = new System.Windows.Forms.TextBox();
            this.btnModify2 = new System.Windows.Forms.Button();
            this.tbHint2 = new System.Windows.Forms.TextBox();
            this.btnModify1 = new System.Windows.Forms.Button();
            this.tbHint1 = new System.Windows.Forms.TextBox();
            this.btnRead = new System.Windows.Forms.Button();
            this.lbMach = new System.Windows.Forms.Label();
            this.tbMAC = new System.Windows.Forms.TextBox();
            this.lbSubID = new System.Windows.Forms.Label();
            this.grpHint = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.tbPWD = new System.Windows.Forms.TextBox();
            this.lbPWD = new System.Windows.Forms.Label();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.panel1.SuspendLayout();
            this.grpMAC.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numDev)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSub)).BeginInit();
            this.grpHint.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.grpMAC);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 64);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(687, 235);
            this.panel1.TabIndex = 4;
            // 
            // grpMAC
            // 
            this.grpMAC.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.grpMAC.Controls.Add(this.tbMACMan);
            this.grpMAC.Controls.Add(this.tbMACAuto);
            this.grpMAC.Controls.Add(this.btnGetDev);
            this.grpMAC.Controls.Add(this.cboDevA);
            this.grpMAC.Controls.Add(this.lbDevA);
            this.grpMAC.Controls.Add(this.numDev);
            this.grpMAC.Controls.Add(this.lbDevID);
            this.grpMAC.Controls.Add(this.numSub);
            this.grpMAC.Controls.Add(this.lbInitial);
            this.grpMAC.Controls.Add(this.btnInitial);
            this.grpMAC.Controls.Add(this.tbHint3);
            this.grpMAC.Controls.Add(this.btnModify2);
            this.grpMAC.Controls.Add(this.tbHint2);
            this.grpMAC.Controls.Add(this.btnModify1);
            this.grpMAC.Controls.Add(this.tbHint1);
            this.grpMAC.Controls.Add(this.btnRead);
            this.grpMAC.Controls.Add(this.lbMach);
            this.grpMAC.Controls.Add(this.tbMAC);
            this.grpMAC.Controls.Add(this.lbSubID);
            this.grpMAC.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpMAC.Enabled = false;
            this.grpMAC.Location = new System.Drawing.Point(0, 0);
            this.grpMAC.Name = "grpMAC";
            this.grpMAC.Size = new System.Drawing.Size(687, 235);
            this.grpMAC.TabIndex = 2;
            this.grpMAC.TabStop = false;
            this.grpMAC.Text = "Select Device";
            // 
            // tbMACMan
            // 
            this.tbMACMan.Location = new System.Drawing.Point(126, 166);
            this.tbMACMan.Mask = "AA.AA.AA.AA.AA.AA.AA.AA";
            this.tbMACMan.Name = "tbMACMan";
            this.tbMACMan.PromptChar = ' ';
            this.tbMACMan.Size = new System.Drawing.Size(360, 22);
            this.tbMACMan.TabIndex = 158;
            this.tbMACMan.Text = "0000000000000000";
            this.tbMACMan.TextChanged += new System.EventHandler(this.tbMACMan_TextChanged);
            // 
            // tbMACAuto
            // 
            this.tbMACAuto.Location = new System.Drawing.Point(125, 133);
            this.tbMACAuto.Mask = "AA.AA.AA.AA.AA.AA.AA.AA";
            this.tbMACAuto.Name = "tbMACAuto";
            this.tbMACAuto.PromptChar = ' ';
            this.tbMACAuto.Size = new System.Drawing.Size(360, 22);
            this.tbMACAuto.TabIndex = 157;
            this.tbMACAuto.Text = "0000000000000000";
            this.tbMACAuto.TextChanged += new System.EventHandler(this.tbMACAuto_TextChanged);
            // 
            // btnGetDev
            // 
            this.btnGetDev.Location = new System.Drawing.Point(536, 20);
            this.btnGetDev.Name = "btnGetDev";
            this.btnGetDev.Size = new System.Drawing.Size(139, 29);
            this.btnGetDev.TabIndex = 156;
            this.btnGetDev.Text = "Get Device List";
            this.btnGetDev.UseVisualStyleBackColor = true;
            this.btnGetDev.Click += new System.EventHandler(this.btnGetDev_Click);
            // 
            // cboDevA
            // 
            this.cboDevA.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDevA.FormattingEnabled = true;
            this.cboDevA.Location = new System.Drawing.Point(126, 22);
            this.cboDevA.Name = "cboDevA";
            this.cboDevA.Size = new System.Drawing.Size(388, 22);
            this.cboDevA.TabIndex = 155;
            this.cboDevA.SelectedIndexChanged += new System.EventHandler(this.cboDevA_SelectedIndexChanged);
            // 
            // lbDevA
            // 
            this.lbDevA.AutoSize = true;
            this.lbDevA.ForeColor = System.Drawing.Color.Black;
            this.lbDevA.Location = new System.Drawing.Point(22, 27);
            this.lbDevA.Name = "lbDevA";
            this.lbDevA.Size = new System.Drawing.Size(82, 14);
            this.lbDevA.TabIndex = 154;
            this.lbDevA.Text = "Select Device:";
            // 
            // numDev
            // 
            this.numDev.Location = new System.Drawing.Point(374, 63);
            this.numDev.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numDev.Name = "numDev";
            this.numDev.Size = new System.Drawing.Size(140, 22);
            this.numDev.TabIndex = 153;
            // 
            // lbDevID
            // 
            this.lbDevID.AutoSize = true;
            this.lbDevID.ForeColor = System.Drawing.Color.Black;
            this.lbDevID.Location = new System.Drawing.Point(279, 65);
            this.lbDevID.Name = "lbDevID";
            this.lbDevID.Size = new System.Drawing.Size(61, 14);
            this.lbDevID.TabIndex = 152;
            this.lbDevID.Text = "Device ID:";
            // 
            // numSub
            // 
            this.numSub.Location = new System.Drawing.Point(125, 63);
            this.numSub.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numSub.Name = "numSub";
            this.numSub.Size = new System.Drawing.Size(140, 22);
            this.numSub.TabIndex = 151;
            // 
            // lbInitial
            // 
            this.lbInitial.Location = new System.Drawing.Point(121, 203);
            this.lbInitial.Name = "lbInitial";
            this.lbInitial.Size = new System.Drawing.Size(553, 22);
            this.lbInitial.TabIndex = 150;
            // 
            // btnInitial
            // 
            this.btnInitial.Location = new System.Drawing.Point(25, 197);
            this.btnInitial.Name = "btnInitial";
            this.btnInitial.Size = new System.Drawing.Size(87, 29);
            this.btnInitial.TabIndex = 149;
            this.btnInitial.Text = "Initial";
            this.btnInitial.UseVisualStyleBackColor = true;
            this.btnInitial.Click += new System.EventHandler(this.btnInitial_Click);
            // 
            // tbHint3
            // 
            this.tbHint3.AcceptsReturn = true;
            this.tbHint3.Location = new System.Drawing.Point(515, 166);
            this.tbHint3.MaxLength = 17;
            this.tbHint3.Name = "tbHint3";
            this.tbHint3.Size = new System.Drawing.Size(159, 22);
            this.tbHint3.TabIndex = 148;
            this.tbHint3.Text = "0";
            this.tbHint3.TextChanged += new System.EventHandler(this.tbHint3_TextChanged);
            // 
            // btnModify2
            // 
            this.btnModify2.Location = new System.Drawing.Point(25, 162);
            this.btnModify2.Name = "btnModify2";
            this.btnModify2.Size = new System.Drawing.Size(87, 29);
            this.btnModify2.TabIndex = 147;
            this.btnModify2.Text = "Manually";
            this.btnModify2.UseVisualStyleBackColor = true;
            this.btnModify2.Click += new System.EventHandler(this.btnModify2_Click);
            // 
            // tbHint2
            // 
            this.tbHint2.Location = new System.Drawing.Point(516, 134);
            this.tbHint2.MaxLength = 17;
            this.tbHint2.Name = "tbHint2";
            this.tbHint2.Size = new System.Drawing.Size(159, 22);
            this.tbHint2.TabIndex = 145;
            this.tbHint2.Text = "0";
            this.tbHint2.TextChanged += new System.EventHandler(this.tbHint2_TextChanged);
            // 
            // btnModify1
            // 
            this.btnModify1.Location = new System.Drawing.Point(25, 130);
            this.btnModify1.Name = "btnModify1";
            this.btnModify1.Size = new System.Drawing.Size(87, 29);
            this.btnModify1.TabIndex = 144;
            this.btnModify1.Text = "Auto Modify";
            this.btnModify1.UseVisualStyleBackColor = true;
            this.btnModify1.Click += new System.EventHandler(this.btnModify1_Click);
            // 
            // tbHint1
            // 
            this.tbHint1.Location = new System.Drawing.Point(516, 102);
            this.tbHint1.Name = "tbHint1";
            this.tbHint1.ReadOnly = true;
            this.tbHint1.Size = new System.Drawing.Size(159, 22);
            this.tbHint1.TabIndex = 142;
            // 
            // btnRead
            // 
            this.btnRead.Location = new System.Drawing.Point(25, 98);
            this.btnRead.Name = "btnRead";
            this.btnRead.Size = new System.Drawing.Size(87, 29);
            this.btnRead.TabIndex = 141;
            this.btnRead.Text = "Read MAC";
            this.btnRead.UseVisualStyleBackColor = true;
            this.btnRead.Click += new System.EventHandler(this.btnRead_Click);
            // 
            // lbMach
            // 
            this.lbMach.ForeColor = System.Drawing.Color.Black;
            this.lbMach.Location = new System.Drawing.Point(545, 65);
            this.lbMach.Name = "lbMach";
            this.lbMach.Size = new System.Drawing.Size(121, 16);
            this.lbMach.TabIndex = 140;
            this.lbMach.Text = "Machine Code";
            this.lbMach.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tbMAC
            // 
            this.tbMAC.Location = new System.Drawing.Point(125, 102);
            this.tbMAC.MaxLength = 23;
            this.tbMAC.Name = "tbMAC";
            this.tbMAC.ReadOnly = true;
            this.tbMAC.Size = new System.Drawing.Size(360, 22);
            this.tbMAC.TabIndex = 133;
            // 
            // lbSubID
            // 
            this.lbSubID.AutoSize = true;
            this.lbSubID.ForeColor = System.Drawing.Color.Black;
            this.lbSubID.Location = new System.Drawing.Point(22, 63);
            this.lbSubID.Name = "lbSubID";
            this.lbSubID.Size = new System.Drawing.Size(64, 14);
            this.lbSubID.TabIndex = 124;
            this.lbSubID.Text = "SubNet ID:";
            // 
            // grpHint
            // 
            this.grpHint.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.grpHint.Controls.Add(this.label2);
            this.grpHint.Controls.Add(this.btnOK);
            this.grpHint.Controls.Add(this.tbPWD);
            this.grpHint.Controls.Add(this.lbPWD);
            this.grpHint.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpHint.Location = new System.Drawing.Point(0, 0);
            this.grpHint.Name = "grpHint";
            this.grpHint.Size = new System.Drawing.Size(687, 64);
            this.grpHint.TabIndex = 3;
            this.grpHint.TabStop = false;
            this.grpHint.Text = "Good to Know";
            // 
            // label2
            // 
            this.label2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(3, 45);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(681, 16);
            this.label2.TabIndex = 143;
            this.label2.Text = "Hint: it is used for workers in HDL factory, it should be the machine code of you" +
    "r module. Thanks!";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(536, 13);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(138, 29);
            this.btnOK.TabIndex = 142;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // tbPWD
            // 
            this.tbPWD.Location = new System.Drawing.Point(125, 16);
            this.tbPWD.Name = "tbPWD";
            this.tbPWD.PasswordChar = '*';
            this.tbPWD.Size = new System.Drawing.Size(389, 22);
            this.tbPWD.TabIndex = 134;
            this.tbPWD.Text = "13922363033";
            // 
            // lbPWD
            // 
            this.lbPWD.AutoSize = true;
            this.lbPWD.ForeColor = System.Drawing.Color.Black;
            this.lbPWD.Location = new System.Drawing.Point(22, 19);
            this.lbPWD.Name = "lbPWD";
            this.lbPWD.Size = new System.Drawing.Size(62, 14);
            this.lbPWD.TabIndex = 125;
            this.lbPWD.Text = "Password:";
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.calculationWorker_DoWork);
            this.backgroundWorker1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.calculationWorker_ProgressChanged);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.calculationWorker_RunWorkerCompleted);
            // 
            // frmMAC
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(687, 299);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.grpHint);
            this.Font = new System.Drawing.Font("Calibri", 9F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmMAC";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Modify MAC";
            this.Load += new System.EventHandler(this.frmMAC_Load);
            this.panel1.ResumeLayout(false);
            this.grpMAC.ResumeLayout(false);
            this.grpMAC.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numDev)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSub)).EndInit();
            this.grpHint.ResumeLayout(false);
            this.grpHint.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpMAC;
        private System.Windows.Forms.Label lbMach;
        private System.Windows.Forms.TextBox tbMAC;
        private System.Windows.Forms.Label lbSubID;
        private System.Windows.Forms.TextBox tbHint2;
        private System.Windows.Forms.Button btnModify1;
        private System.Windows.Forms.TextBox tbHint1;
        private System.Windows.Forms.Button btnRead;
        private System.Windows.Forms.Button btnInitial;
        private System.Windows.Forms.TextBox tbHint3;
        private System.Windows.Forms.Button btnModify2;
        private System.Windows.Forms.Label lbInitial;
        private System.Windows.Forms.NumericUpDown numDev;
        private System.Windows.Forms.Label lbDevID;
        private System.Windows.Forms.NumericUpDown numSub;
        private System.Windows.Forms.GroupBox grpHint;
        private System.Windows.Forms.TextBox tbPWD;
        private System.Windows.Forms.Label lbPWD;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnGetDev;
        private System.Windows.Forms.ComboBox cboDevA;
        private System.Windows.Forms.Label lbDevA;
        private System.Windows.Forms.MaskedTextBox tbMACMan;
        private System.Windows.Forms.MaskedTextBox tbMACAuto;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;

    }
}