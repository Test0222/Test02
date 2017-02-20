namespace HDL_Buspro_Setup_Tool
{
    partial class DateAndTime
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lbDate = new System.Windows.Forms.Label();
            this.cbDate = new System.Windows.Forms.DateTimePicker();
            this.lbTime = new System.Windows.Forms.Label();
            this.Num1 = new System.Windows.Forms.NumericUpDown();
            this.lb1 = new System.Windows.Forms.Label();
            this.Num2 = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.Num3 = new System.Windows.Forms.NumericUpDown();
            this.btnPC = new System.Windows.Forms.Button();
            this.btnRead = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.lbWeek = new System.Windows.Forms.Label();
            this.lbTimeHint = new System.Windows.Forms.Label();
            this.lbCenter = new System.Windows.Forms.Label();
            this.txt1 = new System.Windows.Forms.TextBox();
            this.lbCode = new System.Windows.Forms.Label();
            this.txt2 = new System.Windows.Forms.TextBox();
            this.lbIMEI = new System.Windows.Forms.Label();
            this.lbIMEIValue = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.Num1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Num2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Num3)).BeginInit();
            this.SuspendLayout();
            // 
            // lbDate
            // 
            this.lbDate.AutoSize = true;
            this.lbDate.ForeColor = System.Drawing.Color.Blue;
            this.lbDate.Location = new System.Drawing.Point(3, 8);
            this.lbDate.Name = "lbDate";
            this.lbDate.Size = new System.Drawing.Size(36, 14);
            this.lbDate.TabIndex = 0;
            this.lbDate.Text = "Date:";
            // 
            // cbDate
            // 
            this.cbDate.Location = new System.Drawing.Point(47, 5);
            this.cbDate.Name = "cbDate";
            this.cbDate.Size = new System.Drawing.Size(189, 22);
            this.cbDate.TabIndex = 1;
            this.cbDate.ValueChanged += new System.EventHandler(this.cbDate_ValueChanged);
            // 
            // lbTime
            // 
            this.lbTime.AutoSize = true;
            this.lbTime.ForeColor = System.Drawing.Color.Blue;
            this.lbTime.Location = new System.Drawing.Point(3, 52);
            this.lbTime.Name = "lbTime";
            this.lbTime.Size = new System.Drawing.Size(37, 14);
            this.lbTime.TabIndex = 2;
            this.lbTime.Text = "Time:";
            // 
            // Num1
            // 
            this.Num1.Location = new System.Drawing.Point(47, 49);
            this.Num1.Maximum = new decimal(new int[] {
            23,
            0,
            0,
            0});
            this.Num1.Name = "Num1";
            this.Num1.Size = new System.Drawing.Size(39, 22);
            this.Num1.TabIndex = 3;
            // 
            // lb1
            // 
            this.lb1.AutoSize = true;
            this.lb1.Location = new System.Drawing.Point(90, 54);
            this.lb1.Name = "lb1";
            this.lb1.Size = new System.Drawing.Size(10, 14);
            this.lb1.TabIndex = 4;
            this.lb1.Text = ":";
            // 
            // Num2
            // 
            this.Num2.Location = new System.Drawing.Point(105, 50);
            this.Num2.Maximum = new decimal(new int[] {
            59,
            0,
            0,
            0});
            this.Num2.Name = "Num2";
            this.Num2.Size = new System.Drawing.Size(39, 22);
            this.Num2.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(149, 55);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(10, 14);
            this.label1.TabIndex = 6;
            this.label1.Text = ":";
            // 
            // Num3
            // 
            this.Num3.Location = new System.Drawing.Point(165, 50);
            this.Num3.Maximum = new decimal(new int[] {
            59,
            0,
            0,
            0});
            this.Num3.Name = "Num3";
            this.Num3.Size = new System.Drawing.Size(39, 22);
            this.Num3.TabIndex = 7;
            // 
            // btnPC
            // 
            this.btnPC.ForeColor = System.Drawing.Color.Blue;
            this.btnPC.Location = new System.Drawing.Point(165, 75);
            this.btnPC.Name = "btnPC";
            this.btnPC.Size = new System.Drawing.Size(66, 28);
            this.btnPC.TabIndex = 8;
            this.btnPC.Text = "PC Time";
            this.btnPC.UseVisualStyleBackColor = true;
            this.btnPC.Click += new System.EventHandler(this.btnPC_Click);
            // 
            // btnRead
            // 
            this.btnRead.ForeColor = System.Drawing.Color.Blue;
            this.btnRead.Location = new System.Drawing.Point(88, 190);
            this.btnRead.Name = "btnRead";
            this.btnRead.Size = new System.Drawing.Size(66, 28);
            this.btnRead.TabIndex = 9;
            this.btnRead.Text = "Read";
            this.btnRead.UseVisualStyleBackColor = true;
            this.btnRead.Click += new System.EventHandler(this.btnRead_Click);
            // 
            // btnSave
            // 
            this.btnSave.ForeColor = System.Drawing.Color.Blue;
            this.btnSave.Location = new System.Drawing.Point(165, 190);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(66, 28);
            this.btnSave.TabIndex = 10;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // lbWeek
            // 
            this.lbWeek.AutoSize = true;
            this.lbWeek.ForeColor = System.Drawing.Color.Blue;
            this.lbWeek.Location = new System.Drawing.Point(51, 30);
            this.lbWeek.Name = "lbWeek";
            this.lbWeek.Size = new System.Drawing.Size(43, 14);
            this.lbWeek.TabIndex = 11;
            this.lbWeek.Text = "星期一";
            // 
            // lbTimeHint
            // 
            this.lbTimeHint.AutoSize = true;
            this.lbTimeHint.ForeColor = System.Drawing.Color.Blue;
            this.lbTimeHint.Location = new System.Drawing.Point(47, 79);
            this.lbTimeHint.Name = "lbTimeHint";
            this.lbTimeHint.Size = new System.Drawing.Size(69, 14);
            this.lbTimeHint.TabIndex = 12;
            this.lbTimeHint.Text = "(HH:MM:SS)";
            // 
            // lbCenter
            // 
            this.lbCenter.AutoSize = true;
            this.lbCenter.ForeColor = System.Drawing.Color.Red;
            this.lbCenter.Location = new System.Drawing.Point(3, 108);
            this.lbCenter.Name = "lbCenter";
            this.lbCenter.Size = new System.Drawing.Size(85, 14);
            this.lbCenter.TabIndex = 13;
            this.lbCenter.Text = "Service center:";
            // 
            // txt1
            // 
            this.txt1.Location = new System.Drawing.Point(88, 105);
            this.txt1.Name = "txt1";
            this.txt1.Size = new System.Drawing.Size(143, 22);
            this.txt1.TabIndex = 14;
            this.txt1.Text = "+8613800200500";
            // 
            // lbCode
            // 
            this.lbCode.AutoSize = true;
            this.lbCode.ForeColor = System.Drawing.Color.Red;
            this.lbCode.Location = new System.Drawing.Point(3, 143);
            this.lbCode.Name = "lbCode";
            this.lbCode.Size = new System.Drawing.Size(82, 14);
            this.lbCode.TabIndex = 15;
            this.lbCode.Text = "Country  code:";
            // 
            // txt2
            // 
            this.txt2.Location = new System.Drawing.Point(88, 140);
            this.txt2.Name = "txt2";
            this.txt2.Size = new System.Drawing.Size(143, 22);
            this.txt2.TabIndex = 16;
            this.txt2.Text = "86";
            // 
            // lbIMEI
            // 
            this.lbIMEI.AutoSize = true;
            this.lbIMEI.ForeColor = System.Drawing.Color.Red;
            this.lbIMEI.Location = new System.Drawing.Point(3, 174);
            this.lbIMEI.Name = "lbIMEI";
            this.lbIMEI.Size = new System.Drawing.Size(34, 14);
            this.lbIMEI.TabIndex = 17;
            this.lbIMEI.Text = "IMEI:";
            // 
            // lbIMEIValue
            // 
            this.lbIMEIValue.AutoSize = true;
            this.lbIMEIValue.ForeColor = System.Drawing.Color.Red;
            this.lbIMEIValue.Location = new System.Drawing.Point(47, 174);
            this.lbIMEIValue.Name = "lbIMEIValue";
            this.lbIMEIValue.Size = new System.Drawing.Size(13, 14);
            this.lbIMEIValue.TabIndex = 18;
            this.lbIMEIValue.Text = "0";
            // 
            // DateAndTime
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lbIMEIValue);
            this.Controls.Add(this.lbIMEI);
            this.Controls.Add(this.txt2);
            this.Controls.Add(this.lbCode);
            this.Controls.Add(this.txt1);
            this.Controls.Add(this.lbCenter);
            this.Controls.Add(this.lbTimeHint);
            this.Controls.Add(this.lbWeek);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnRead);
            this.Controls.Add(this.btnPC);
            this.Controls.Add(this.Num3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Num2);
            this.Controls.Add(this.lb1);
            this.Controls.Add(this.Num1);
            this.Controls.Add(this.lbTime);
            this.Controls.Add(this.cbDate);
            this.Controls.Add(this.lbDate);
            this.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "DateAndTime";
            this.Size = new System.Drawing.Size(235, 224);
            ((System.ComponentModel.ISupportInitialize)(this.Num1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Num2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Num3)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbDate;
        private System.Windows.Forms.DateTimePicker cbDate;
        private System.Windows.Forms.Label lbTime;
        private System.Windows.Forms.NumericUpDown Num1;
        private System.Windows.Forms.Label lb1;
        private System.Windows.Forms.NumericUpDown Num2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown Num3;
        private System.Windows.Forms.Button btnPC;
        private System.Windows.Forms.Button btnRead;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label lbWeek;
        private System.Windows.Forms.Label lbTimeHint;
        private System.Windows.Forms.Label lbCenter;
        private System.Windows.Forms.TextBox txt1;
        private System.Windows.Forms.Label lbCode;
        private System.Windows.Forms.TextBox txt2;
        private System.Windows.Forms.Label lbIMEI;
        private System.Windows.Forms.Label lbIMEIValue;
    }
}
