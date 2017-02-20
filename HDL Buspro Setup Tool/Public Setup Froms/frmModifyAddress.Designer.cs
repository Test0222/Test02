namespace HDL_Buspro_Setup_Tool
{
    partial class frmModifAddress
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
            this.btnOK = new System.Windows.Forms.Button();
            this.lbSum = new System.Windows.Forms.Label();
            this.tbsub = new System.Windows.Forms.TextBox();
            this.lbRmName = new System.Windows.Forms.Label();
            this.tbDev = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tbMAC = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.numDev = new System.Windows.Forms.NumericUpDown();
            this.numSub = new System.Windows.Forms.NumericUpDown();
            this.btnRead = new System.Windows.Forms.Button();
            this.btnModify1 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.gbMAC = new System.Windows.Forms.GroupBox();
            this.gbManual = new System.Windows.Forms.GroupBox();
            this.lbHint = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.numDev)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSub)).BeginInit();
            this.gbMAC.SuspendLayout();
            this.gbManual.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(252, 47);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(120, 24);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "Modify Address";
            this.btnOK.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // lbSum
            // 
            this.lbSum.AutoSize = true;
            this.lbSum.Location = new System.Drawing.Point(10, 53);
            this.lbSum.Name = "lbSum";
            this.lbSum.Size = new System.Drawing.Size(91, 14);
            this.lbSum.TabIndex = 3;
            this.lbSum.Text = "New SubNet ID:";
            // 
            // tbsub
            // 
            this.tbsub.Location = new System.Drawing.Point(123, 13);
            this.tbsub.Name = "tbsub";
            this.tbsub.ReadOnly = true;
            this.tbsub.Size = new System.Drawing.Size(105, 22);
            this.tbsub.TabIndex = 2;
            this.tbsub.TextChanged += new System.EventHandler(this.tbsub_TextChanged);
            // 
            // lbRmName
            // 
            this.lbRmName.AutoSize = true;
            this.lbRmName.Location = new System.Drawing.Point(6, 16);
            this.lbRmName.Name = "lbRmName";
            this.lbRmName.Size = new System.Drawing.Size(89, 14);
            this.lbRmName.TabIndex = 2;
            this.lbRmName.Text = " Old SubNet ID:";
            // 
            // tbDev
            // 
            this.tbDev.Location = new System.Drawing.Point(367, 13);
            this.tbDev.Name = "tbDev";
            this.tbDev.ReadOnly = true;
            this.tbDev.Size = new System.Drawing.Size(105, 22);
            this.tbDev.TabIndex = 6;
            this.tbDev.Text = "1";
            this.tbDev.TextChanged += new System.EventHandler(this.tbsub_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(246, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 14);
            this.label1.TabIndex = 7;
            this.label1.Text = " Old Device ID:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(249, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(88, 14);
            this.label2.TabIndex = 10;
            this.label2.Text = "New Device ID:";
            // 
            // tbMAC
            // 
            this.tbMAC.Enabled = false;
            this.tbMAC.Location = new System.Drawing.Point(123, 19);
            this.tbMAC.MaxLength = 23;
            this.tbMAC.Name = "tbMAC";
            this.tbMAC.ReadOnly = true;
            this.tbMAC.Size = new System.Drawing.Size(260, 22);
            this.tbMAC.TabIndex = 142;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 27);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(33, 14);
            this.label3.TabIndex = 143;
            this.label3.Text = "MAC:";
            // 
            // numDev
            // 
            this.numDev.Location = new System.Drawing.Point(367, 51);
            this.numDev.Maximum = new decimal(new int[] {
            254,
            0,
            0,
            0});
            this.numDev.Name = "numDev";
            this.numDev.Size = new System.Drawing.Size(106, 22);
            this.numDev.TabIndex = 1;
            this.numDev.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // numSub
            // 
            this.numSub.Location = new System.Drawing.Point(123, 50);
            this.numSub.Maximum = new decimal(new int[] {
            254,
            0,
            0,
            0});
            this.numSub.Name = "numSub";
            this.numSub.Size = new System.Drawing.Size(106, 22);
            this.numSub.TabIndex = 0;
            this.numSub.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numSub.ValueChanged += new System.EventHandler(this.numSub_ValueChanged);
            // 
            // btnRead
            // 
            this.btnRead.Location = new System.Drawing.Point(123, 27);
            this.btnRead.Name = "btnRead";
            this.btnRead.Size = new System.Drawing.Size(120, 24);
            this.btnRead.TabIndex = 0;
            this.btnRead.Text = "Show device";
            this.btnRead.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRead.UseVisualStyleBackColor = true;
            this.btnRead.Click += new System.EventHandler(this.btnRead_Click);
            // 
            // btnModify1
            // 
            this.btnModify1.Location = new System.Drawing.Point(252, 27);
            this.btnModify1.Name = "btnModify1";
            this.btnModify1.Size = new System.Drawing.Size(120, 24);
            this.btnModify1.TabIndex = 1;
            this.btnModify1.Text = "Update ID";
            this.btnModify1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnModify1.UseVisualStyleBackColor = true;
            this.btnModify1.Click += new System.EventHandler(this.btnModify1_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(123, 47);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(120, 24);
            this.button1.TabIndex = 159;
            this.button1.Text = "Read MAC";
            this.button1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // gbMAC
            // 
            this.gbMAC.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.gbMAC.Controls.Add(this.label3);
            this.gbMAC.Controls.Add(this.button1);
            this.gbMAC.Controls.Add(this.tbMAC);
            this.gbMAC.Controls.Add(this.btnOK);
            this.gbMAC.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbMAC.Location = new System.Drawing.Point(0, 0);
            this.gbMAC.Name = "gbMAC";
            this.gbMAC.Size = new System.Drawing.Size(484, 86);
            this.gbMAC.TabIndex = 160;
            this.gbMAC.TabStop = false;
            this.gbMAC.Text = "Modify Address By MAC ";
            // 
            // gbManual
            // 
            this.gbManual.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.gbManual.Controls.Add(this.lbHint);
            this.gbManual.Controls.Add(this.btnRead);
            this.gbManual.Controls.Add(this.btnModify1);
            this.gbManual.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbManual.Location = new System.Drawing.Point(0, 0);
            this.gbManual.Name = "gbManual";
            this.gbManual.Size = new System.Drawing.Size(484, 86);
            this.gbManual.TabIndex = 0;
            this.gbManual.TabStop = false;
            this.gbManual.Text = "Modify Address By Press Device Button";
            // 
            // lbHint
            // 
            this.lbHint.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lbHint.Font = new System.Drawing.Font("Calibri", 10F);
            this.lbHint.ForeColor = System.Drawing.Color.Red;
            this.lbHint.Location = new System.Drawing.Point(3, 61);
            this.lbHint.Name = "lbHint";
            this.lbHint.Size = new System.Drawing.Size(478, 22);
            this.lbHint.TabIndex = 0;
            this.lbHint.Text = "Hint: Please Long Press Device Button 3S";
            this.lbHint.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.lbRmName);
            this.panel1.Controls.Add(this.tbsub);
            this.panel1.Controls.Add(this.lbSum);
            this.panel1.Controls.Add(this.numDev);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.numSub);
            this.panel1.Controls.Add(this.tbDev);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(484, 86);
            this.panel1.TabIndex = 0;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.gbMAC);
            this.panel3.Controls.Add(this.gbManual);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 86);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(484, 86);
            this.panel3.TabIndex = 164;
            // 
            // frmModifAddress
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(484, 172);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Calibri", 9F);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(500, 210);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(500, 210);
            this.Name = "frmModifAddress";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmModifAddress_FormClosing);
            this.Load += new System.EventHandler(this.frmRemark_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numDev)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSub)).EndInit();
            this.gbMAC.ResumeLayout(false);
            this.gbMAC.PerformLayout();
            this.gbManual.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label lbSum;
        private System.Windows.Forms.TextBox tbsub;
        private System.Windows.Forms.Label lbRmName;
        private System.Windows.Forms.TextBox tbDev;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbMAC;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown numDev;
        private System.Windows.Forms.NumericUpDown numSub;
        private System.Windows.Forms.Button btnRead;
        private System.Windows.Forms.Button btnModify1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.GroupBox gbMAC;
        private System.Windows.Forms.GroupBox gbManual;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label lbHint;
    }
}