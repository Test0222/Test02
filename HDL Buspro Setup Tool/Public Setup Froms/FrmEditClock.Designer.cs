namespace HDL_Buspro_Setup_Tool
{
    partial class FrmEditClock
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rb3 = new System.Windows.Forms.RadioButton();
            this.rb2 = new System.Windows.Forms.RadioButton();
            this.rb1 = new System.Windows.Forms.RadioButton();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.panel8 = new System.Windows.Forms.Panel();
            this.chbList = new System.Windows.Forms.CheckedListBox();
            this.panel7 = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.numCustom = new System.Windows.Forms.NumericUpDown();
            this.lbCustom = new System.Windows.Forms.Label();
            this.panel6 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.lbDate = new System.Windows.Forms.Label();
            this.numDate2 = new System.Windows.Forms.NumericUpDown();
            this.numDate1 = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.lbTime = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.numTime1 = new System.Windows.Forms.NumericUpDown();
            this.numTime2 = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnSave = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel8.SuspendLayout();
            this.panel7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numCustom)).BeginInit();
            this.panel6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numDate2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDate1)).BeginInit();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numTime1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTime2)).BeginInit();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(609, 91);
            this.panel1.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.groupBox1.Controls.Add(this.rb3);
            this.groupBox1.Controls.Add(this.rb2);
            this.groupBox1.Controls.Add(this.rb1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(609, 91);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Choose Ways";
            // 
            // rb3
            // 
            this.rb3.AutoSize = true;
            this.rb3.Location = new System.Drawing.Point(464, 41);
            this.rb3.Margin = new System.Windows.Forms.Padding(4);
            this.rb3.Name = "rb3";
            this.rb3.Size = new System.Drawing.Size(65, 18);
            this.rb3.TabIndex = 2;
            this.rb3.TabStop = true;
            this.rb3.Tag = "3";
            this.rb3.Text = "Custom";
            this.rb3.UseVisualStyleBackColor = true;
            this.rb3.CheckedChanged += new System.EventHandler(this.rb1_CheckedChanged);
            // 
            // rb2
            // 
            this.rb2.AutoSize = true;
            this.rb2.Location = new System.Drawing.Point(243, 41);
            this.rb2.Margin = new System.Windows.Forms.Padding(4);
            this.rb2.Name = "rb2";
            this.rb2.Size = new System.Drawing.Size(65, 18);
            this.rb2.TabIndex = 1;
            this.rb2.TabStop = true;
            this.rb2.Tag = "2";
            this.rb2.Text = "Weekly";
            this.rb2.UseVisualStyleBackColor = true;
            this.rb2.CheckedChanged += new System.EventHandler(this.rb1_CheckedChanged);
            // 
            // rb1
            // 
            this.rb1.AutoSize = true;
            this.rb1.Location = new System.Drawing.Point(43, 41);
            this.rb1.Margin = new System.Windows.Forms.Padding(4);
            this.rb1.Name = "rb1";
            this.rb1.Size = new System.Drawing.Size(52, 18);
            this.rb1.TabIndex = 0;
            this.rb1.TabStop = true;
            this.rb1.Tag = "1";
            this.rb1.Text = "Once";
            this.rb1.UseVisualStyleBackColor = true;
            this.rb1.CheckedChanged += new System.EventHandler(this.rb1_CheckedChanged);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.panel5);
            this.panel2.Controls.Add(this.panel4);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 91);
            this.panel2.Margin = new System.Windows.Forms.Padding(4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(609, 197);
            this.panel2.TabIndex = 1;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.panel8);
            this.panel5.Controls.Add(this.panel7);
            this.panel5.Controls.Add(this.panel6);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(0, 50);
            this.panel5.Margin = new System.Windows.Forms.Padding(4);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(609, 147);
            this.panel5.TabIndex = 6;
            // 
            // panel8
            // 
            this.panel8.Controls.Add(this.chbList);
            this.panel8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel8.Location = new System.Drawing.Point(0, 105);
            this.panel8.Margin = new System.Windows.Forms.Padding(4);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(609, 42);
            this.panel8.TabIndex = 7;
            // 
            // chbList
            // 
            this.chbList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chbList.FormattingEnabled = true;
            this.chbList.Location = new System.Drawing.Point(0, 0);
            this.chbList.Margin = new System.Windows.Forms.Padding(4);
            this.chbList.Name = "chbList";
            this.chbList.Size = new System.Drawing.Size(609, 42);
            this.chbList.TabIndex = 0;
            // 
            // panel7
            // 
            this.panel7.Controls.Add(this.label5);
            this.panel7.Controls.Add(this.numCustom);
            this.panel7.Controls.Add(this.lbCustom);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel7.Location = new System.Drawing.Point(0, 52);
            this.panel7.Margin = new System.Windows.Forms.Padding(4);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(609, 53);
            this.panel7.TabIndex = 6;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(396, 20);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 14);
            this.label5.TabIndex = 6;
            this.label5.Text = "(Hour)";
            // 
            // numCustom
            // 
            this.numCustom.Location = new System.Drawing.Point(179, 17);
            this.numCustom.Margin = new System.Windows.Forms.Padding(4);
            this.numCustom.Maximum = new decimal(new int[] {
            23,
            0,
            0,
            0});
            this.numCustom.Name = "numCustom";
            this.numCustom.Size = new System.Drawing.Size(197, 22);
            this.numCustom.TabIndex = 3;
            // 
            // lbCustom
            // 
            this.lbCustom.AutoSize = true;
            this.lbCustom.Location = new System.Drawing.Point(67, 20);
            this.lbCustom.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbCustom.Name = "lbCustom";
            this.lbCustom.Size = new System.Drawing.Size(86, 14);
            this.lbCustom.TabIndex = 0;
            this.lbCustom.Text = "Interval(Hour):";
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.label4);
            this.panel6.Controls.Add(this.lbDate);
            this.panel6.Controls.Add(this.numDate2);
            this.panel6.Controls.Add(this.numDate1);
            this.panel6.Controls.Add(this.label3);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel6.Location = new System.Drawing.Point(0, 0);
            this.panel6.Margin = new System.Windows.Forms.Padding(4);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(609, 52);
            this.panel6.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(396, 17);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(73, 14);
            this.label4.TabIndex = 5;
            this.label4.Text = "(Month:Day)";
            // 
            // lbDate
            // 
            this.lbDate.AutoSize = true;
            this.lbDate.Location = new System.Drawing.Point(67, 17);
            this.lbDate.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbDate.Name = "lbDate";
            this.lbDate.Size = new System.Drawing.Size(67, 14);
            this.lbDate.TabIndex = 0;
            this.lbDate.Text = "Date(M/D):";
            // 
            // numDate2
            // 
            this.numDate2.Location = new System.Drawing.Point(292, 15);
            this.numDate2.Margin = new System.Windows.Forms.Padding(4);
            this.numDate2.Maximum = new decimal(new int[] {
            31,
            0,
            0,
            0});
            this.numDate2.Name = "numDate2";
            this.numDate2.Size = new System.Drawing.Size(84, 22);
            this.numDate2.TabIndex = 4;
            // 
            // numDate1
            // 
            this.numDate1.Location = new System.Drawing.Point(179, 15);
            this.numDate1.Margin = new System.Windows.Forms.Padding(4);
            this.numDate1.Maximum = new decimal(new int[] {
            12,
            0,
            0,
            0});
            this.numDate1.Name = "numDate1";
            this.numDate1.Size = new System.Drawing.Size(84, 22);
            this.numDate1.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(271, 17);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(12, 14);
            this.label3.TabIndex = 3;
            this.label3.Text = "/";
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.lbTime);
            this.panel4.Controls.Add(this.label2);
            this.panel4.Controls.Add(this.numTime1);
            this.panel4.Controls.Add(this.numTime2);
            this.panel4.Controls.Add(this.label1);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Margin = new System.Windows.Forms.Padding(4);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(609, 50);
            this.panel4.TabIndex = 5;
            // 
            // lbTime
            // 
            this.lbTime.AutoSize = true;
            this.lbTime.Location = new System.Drawing.Point(67, 16);
            this.lbTime.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbTime.Name = "lbTime";
            this.lbTime.Size = new System.Drawing.Size(37, 14);
            this.lbTime.TabIndex = 0;
            this.lbTime.Text = "Time:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(396, 16);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(54, 14);
            this.label2.TabIndex = 4;
            this.label2.Text = "(HH:MM)";
            // 
            // numTime1
            // 
            this.numTime1.Location = new System.Drawing.Point(179, 14);
            this.numTime1.Margin = new System.Windows.Forms.Padding(4);
            this.numTime1.Maximum = new decimal(new int[] {
            23,
            0,
            0,
            0});
            this.numTime1.Name = "numTime1";
            this.numTime1.Size = new System.Drawing.Size(84, 22);
            this.numTime1.TabIndex = 1;
            // 
            // numTime2
            // 
            this.numTime2.Location = new System.Drawing.Point(292, 14);
            this.numTime2.Margin = new System.Windows.Forms.Padding(4);
            this.numTime2.Maximum = new decimal(new int[] {
            59,
            0,
            0,
            0});
            this.numTime2.Name = "numTime2";
            this.numTime2.Size = new System.Drawing.Size(84, 22);
            this.numTime2.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(271, 16);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(10, 14);
            this.label1.TabIndex = 2;
            this.label1.Text = ":";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnSave);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 288);
            this.panel3.Margin = new System.Windows.Forms.Padding(4);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(609, 54);
            this.panel3.TabIndex = 2;
            // 
            // btnSave
            // 
            this.btnSave.Image = global::HDL_Buspro_Setup_Tool.Properties.Resources.Save;
            this.btnSave.Location = new System.Drawing.Point(456, 9);
            this.btnSave.Margin = new System.Windows.Forms.Padding(4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(140, 32);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "Save";
            this.btnSave.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // FrmEditClock
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(609, 342);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(625, 380);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(625, 380);
            this.Name = "FrmEditClock";
            this.Text = "Edit Clock";
            this.Load += new System.EventHandler(this.FrmEditClock_Load);
            this.panel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel8.ResumeLayout(false);
            this.panel7.ResumeLayout(false);
            this.panel7.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numCustom)).EndInit();
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numDate2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDate1)).EndInit();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numTime1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTime2)).EndInit();
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rb3;
        private System.Windows.Forms.RadioButton rb2;
        private System.Windows.Forms.RadioButton rb1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label lbTime;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numTime1;
        private System.Windows.Forms.NumericUpDown numTime2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lbDate;
        private System.Windows.Forms.NumericUpDown numDate2;
        private System.Windows.Forms.NumericUpDown numDate1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.CheckedListBox chbList;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown numCustom;
        private System.Windows.Forms.Label lbCustom;
    }
}