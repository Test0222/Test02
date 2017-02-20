namespace HDL_Buspro_Setup_Tool
{
    partial class DIY
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
            this.gbPreview = new System.Windows.Forms.GroupBox();
            this.ClearIcons = new System.Windows.Forms.Button();
            this.chbFrame = new System.Windows.Forms.CheckBox();
            this.gbDisplay = new System.Windows.Forms.GroupBox();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.btnRight = new System.Windows.Forms.Button();
            this.btnDown = new System.Windows.Forms.Button();
            this.btnTop = new System.Windows.Forms.Button();
            this.btnLeft = new System.Windows.Forms.Button();
            this.txt4 = new System.Windows.Forms.TextBox();
            this.txt3 = new System.Windows.Forms.TextBox();
            this.txt2 = new System.Windows.Forms.TextBox();
            this.txt1 = new System.Windows.Forms.TextBox();
            this.btnFont = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.chbInvert = new System.Windows.Forms.CheckBox();
            this.lbF1 = new System.Windows.Forms.Label();
            this.lbF2 = new System.Windows.Forms.Label();
            this.gbPreview.SuspendLayout();
            this.gbDisplay.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.SuspendLayout();
            // 
            // gbPreview
            // 
            this.gbPreview.Controls.Add(this.ClearIcons);
            this.gbPreview.Controls.Add(this.chbFrame);
            this.gbPreview.Controls.Add(this.gbDisplay);
            this.gbPreview.Controls.Add(this.chbInvert);
            this.gbPreview.Controls.Add(this.lbF1);
            this.gbPreview.Controls.Add(this.lbF2);
            this.gbPreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbPreview.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbPreview.Location = new System.Drawing.Point(0, 0);
            this.gbPreview.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.gbPreview.Name = "gbPreview";
            this.gbPreview.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.gbPreview.Size = new System.Drawing.Size(210, 415);
            this.gbPreview.TabIndex = 9;
            this.gbPreview.TabStop = false;
            this.gbPreview.Text = "Preview";
            // 
            // ClearIcons
            // 
            this.ClearIcons.Location = new System.Drawing.Point(133, 91);
            this.ClearIcons.Name = "ClearIcons";
            this.ClearIcons.Size = new System.Drawing.Size(68, 23);
            this.ClearIcons.TabIndex = 25;
            this.ClearIcons.Text = "Blank GB";
            this.ClearIcons.UseVisualStyleBackColor = true;
            this.ClearIcons.Click += new System.EventHandler(this.ClearIcons_Click);
            // 
            // chbFrame
            // 
            this.chbFrame.AutoSize = true;
            this.chbFrame.Location = new System.Drawing.Point(134, 64);
            this.chbFrame.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chbFrame.Name = "chbFrame";
            this.chbFrame.Size = new System.Drawing.Size(67, 18);
            this.chbFrame.TabIndex = 24;
            this.chbFrame.Text = "Framed";
            this.chbFrame.UseVisualStyleBackColor = true;
            this.chbFrame.CheckedChanged += new System.EventHandler(this.chbFrame_CheckedChanged);
            // 
            // gbDisplay
            // 
            this.gbDisplay.Controls.Add(this.numericUpDown1);
            this.gbDisplay.Controls.Add(this.btnRight);
            this.gbDisplay.Controls.Add(this.btnDown);
            this.gbDisplay.Controls.Add(this.btnTop);
            this.gbDisplay.Controls.Add(this.btnLeft);
            this.gbDisplay.Controls.Add(this.txt4);
            this.gbDisplay.Controls.Add(this.txt3);
            this.gbDisplay.Controls.Add(this.txt2);
            this.gbDisplay.Controls.Add(this.txt1);
            this.gbDisplay.Controls.Add(this.btnFont);
            this.gbDisplay.Controls.Add(this.btnClear);
            this.gbDisplay.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.gbDisplay.Location = new System.Drawing.Point(3, 179);
            this.gbDisplay.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.gbDisplay.Name = "gbDisplay";
            this.gbDisplay.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.gbDisplay.Size = new System.Drawing.Size(204, 234);
            this.gbDisplay.TabIndex = 10;
            this.gbDisplay.TabStop = false;
            this.gbDisplay.Text = "Buttons Display";
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(76, 187);
            this.numericUpDown1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(38, 22);
            this.numericUpDown1.TabIndex = 11;
            this.numericUpDown1.Value = new decimal(new int[] {
            13,
            0,
            0,
            0});
            this.numericUpDown1.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged_1);
            // 
            // btnRight
            // 
            this.btnRight.Image = global::HDL_Buspro_Setup_Tool.Properties.Resources.箭头右;
            this.btnRight.Location = new System.Drawing.Point(120, 184);
            this.btnRight.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnRight.Name = "btnRight";
            this.btnRight.Size = new System.Drawing.Size(34, 23);
            this.btnRight.TabIndex = 10;
            this.btnRight.Tag = "1";
            this.btnRight.UseVisualStyleBackColor = true;
            this.btnRight.Click += new System.EventHandler(this.btnRight_Click);
            // 
            // btnDown
            // 
            this.btnDown.Image = global::HDL_Buspro_Setup_Tool.Properties.Resources.箭头下;
            this.btnDown.Location = new System.Drawing.Point(76, 211);
            this.btnDown.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnDown.Name = "btnDown";
            this.btnDown.Size = new System.Drawing.Size(38, 23);
            this.btnDown.TabIndex = 9;
            this.btnDown.Tag = "3";
            this.btnDown.UseVisualStyleBackColor = true;
            this.btnDown.Click += new System.EventHandler(this.btnDown_Click);
            // 
            // btnTop
            // 
            this.btnTop.Image = global::HDL_Buspro_Setup_Tool.Properties.Resources.箭头上;
            this.btnTop.Location = new System.Drawing.Point(76, 160);
            this.btnTop.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnTop.Name = "btnTop";
            this.btnTop.Size = new System.Drawing.Size(38, 23);
            this.btnTop.TabIndex = 8;
            this.btnTop.Tag = "2";
            this.btnTop.UseVisualStyleBackColor = true;
            this.btnTop.Click += new System.EventHandler(this.btnTop_Click);
            // 
            // btnLeft
            // 
            this.btnLeft.Image = global::HDL_Buspro_Setup_Tool.Properties.Resources.箭头左;
            this.btnLeft.Location = new System.Drawing.Point(39, 184);
            this.btnLeft.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnLeft.Name = "btnLeft";
            this.btnLeft.Size = new System.Drawing.Size(34, 23);
            this.btnLeft.TabIndex = 7;
            this.btnLeft.Tag = "<";
            this.btnLeft.UseVisualStyleBackColor = true;
            this.btnLeft.Click += new System.EventHandler(this.btnLeft_Click);
            // 
            // txt4
            // 
            this.txt4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txt4.Location = new System.Drawing.Point(34, 127);
            this.txt4.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txt4.Multiline = true;
            this.txt4.Name = "txt4";
            this.txt4.Size = new System.Drawing.Size(120, 31);
            this.txt4.TabIndex = 6;
            this.txt4.Tag = "4";
            this.txt4.TextChanged += new System.EventHandler(this.txt1_TextChanged);
            // 
            // txt3
            // 
            this.txt3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txt3.Location = new System.Drawing.Point(34, 92);
            this.txt3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txt3.Multiline = true;
            this.txt3.Name = "txt3";
            this.txt3.Size = new System.Drawing.Size(120, 31);
            this.txt3.TabIndex = 5;
            this.txt3.Tag = "3";
            this.txt3.TextChanged += new System.EventHandler(this.txt1_TextChanged);
            // 
            // txt2
            // 
            this.txt2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txt2.Location = new System.Drawing.Point(34, 57);
            this.txt2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txt2.Multiline = true;
            this.txt2.Name = "txt2";
            this.txt2.Size = new System.Drawing.Size(120, 31);
            this.txt2.TabIndex = 4;
            this.txt2.Tag = "2";
            this.txt2.TextChanged += new System.EventHandler(this.txt1_TextChanged);
            // 
            // txt1
            // 
            this.txt1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txt1.Location = new System.Drawing.Point(34, 21);
            this.txt1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txt1.Multiline = true;
            this.txt1.Name = "txt1";
            this.txt1.Size = new System.Drawing.Size(120, 32);
            this.txt1.TabIndex = 3;
            this.txt1.Tag = "1";
            this.txt1.Text = "looooo";
            this.txt1.TextChanged += new System.EventHandler(this.txt1_TextChanged);
            // 
            // btnFont
            // 
            this.btnFont.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFont.Location = new System.Drawing.Point(12, 211);
            this.btnFont.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnFont.Name = "btnFont";
            this.btnFont.Size = new System.Drawing.Size(65, 23);
            this.btnFont.TabIndex = 2;
            this.btnFont.Text = "Font...";
            this.btnFont.UseVisualStyleBackColor = true;
            this.btnFont.Click += new System.EventHandler(this.btnFont_Click);
            // 
            // btnClear
            // 
            this.btnClear.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClear.Location = new System.Drawing.Point(112, 211);
            this.btnClear.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(65, 23);
            this.btnClear.TabIndex = 1;
            this.btnClear.Text = "Clear All";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // chbInvert
            // 
            this.chbInvert.AutoSize = true;
            this.chbInvert.Location = new System.Drawing.Point(134, 37);
            this.chbInvert.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chbInvert.Name = "chbInvert";
            this.chbInvert.Size = new System.Drawing.Size(57, 18);
            this.chbInvert.TabIndex = 20;
            this.chbInvert.Text = "Invert";
            this.chbInvert.UseVisualStyleBackColor = true;
            this.chbInvert.CheckedChanged += new System.EventHandler(this.chbInvert_CheckedChanged);
            // 
            // lbF1
            // 
            this.lbF1.AllowDrop = true;
            this.lbF1.BackColor = System.Drawing.Color.White;
            this.lbF1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbF1.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbF1.ForeColor = System.Drawing.Color.Black;
            this.lbF1.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.lbF1.Location = new System.Drawing.Point(50, 36);
            this.lbF1.Name = "lbF1";
            this.lbF1.Size = new System.Drawing.Size(77, 132);
            this.lbF1.TabIndex = 19;
            this.lbF1.Tag = "8";
            this.lbF1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbF1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lbF1_MouseDoubleClick);
            // 
            // lbF2
            // 
            this.lbF2.AllowDrop = true;
            this.lbF2.AutoEllipsis = true;
            this.lbF2.BackColor = System.Drawing.Color.White;
            this.lbF2.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbF2.ForeColor = System.Drawing.Color.Black;
            this.lbF2.Location = new System.Drawing.Point(34, 36);
            this.lbF2.Name = "lbF2";
            this.lbF2.Size = new System.Drawing.Size(80, 132);
            this.lbF2.TabIndex = 23;
            this.lbF2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // DIY
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gbPreview);
            this.Name = "DIY";
            this.Size = new System.Drawing.Size(210, 415);
            this.gbPreview.ResumeLayout(false);
            this.gbPreview.PerformLayout();
            this.gbDisplay.ResumeLayout(false);
            this.gbDisplay.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbPreview;
        private System.Windows.Forms.GroupBox gbDisplay;
        private System.Windows.Forms.TextBox txt4;
        private System.Windows.Forms.TextBox txt3;
        private System.Windows.Forms.TextBox txt2;
        private System.Windows.Forms.TextBox txt1;
        private System.Windows.Forms.Button btnFont;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Label lbF2;
        private System.Windows.Forms.Label lbF1;
        private System.Windows.Forms.CheckBox chbInvert;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Button btnRight;
        private System.Windows.Forms.Button btnDown;
        private System.Windows.Forms.Button btnTop;
        private System.Windows.Forms.Button btnLeft;
        private System.Windows.Forms.CheckBox chbFrame;
        private System.Windows.Forms.Button ClearIcons;
    }
}
