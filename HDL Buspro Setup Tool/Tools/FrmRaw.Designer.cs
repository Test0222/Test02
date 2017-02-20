namespace HDL_Buspro_Setup_Tool
{
    partial class FrmRaw
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
            this.button1 = new System.Windows.Forms.Button();
            this.lb1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.Pic = new System.Windows.Forms.PictureBox();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.lbPath = new System.Windows.Forms.TextBox();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Pic)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(257, 6);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(70, 24);
            this.button1.TabIndex = 2;
            this.button1.Text = "Save As";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // lb1
            // 
            this.lb1.AutoSize = true;
            this.lb1.Location = new System.Drawing.Point(3, 10);
            this.lb1.Name = "lb1";
            this.lb1.Size = new System.Drawing.Size(51, 14);
            this.lb1.TabIndex = 0;
            this.lb1.Text = "Save As:";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.lbPath);
            this.panel2.Controls.Add(this.button1);
            this.panel2.Controls.Add(this.lb1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 342);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(330, 38);
            this.panel2.TabIndex = 1;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.Pic);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(330, 342);
            this.panel3.TabIndex = 2;
            // 
            // Pic
            // 
            this.Pic.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Pic.Location = new System.Drawing.Point(0, 0);
            this.Pic.Name = "Pic";
            this.Pic.Size = new System.Drawing.Size(330, 342);
            this.Pic.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.Pic.TabIndex = 0;
            this.Pic.TabStop = false;
            this.Pic.DoubleClick += new System.EventHandler(this.Pic_DoubleClick);
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.Filter = "raw files (*.raw)|*.raw";
            // 
            // lbPath
            // 
            this.lbPath.Location = new System.Drawing.Point(71, 6);
            this.lbPath.Name = "lbPath";
            this.lbPath.Size = new System.Drawing.Size(180, 22);
            this.lbPath.TabIndex = 3;
            // 
            // FrmRaw
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(330, 380);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "FrmRaw";
            this.Text = "Pictrue Change to Raw File";
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Pic)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.PictureBox Pic;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label lb1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.TextBox lbPath;
    }
}