namespace HDL_Buspro_Setup_Tool
{
    partial class FrmBoundRate
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
            this.lbRate = new System.Windows.Forms.Label();
            this.cbRate = new System.Windows.Forms.ComboBox();
            this.lbBit = new System.Windows.Forms.Label();
            this.cbBit = new System.Windows.Forms.ComboBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnRead = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lbRate
            // 
            this.lbRate.AutoSize = true;
            this.lbRate.Location = new System.Drawing.Point(16, 15);
            this.lbRate.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbRate.Name = "lbRate";
            this.lbRate.Size = new System.Drawing.Size(66, 14);
            this.lbRate.TabIndex = 0;
            this.lbRate.Text = "Baud Rate:";
            // 
            // cbRate
            // 
            this.cbRate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbRate.FormattingEnabled = true;
            this.cbRate.Items.AddRange(new object[] {
            "1200",
            "2400",
            "4800",
            "9600",
            "14400",
            "19200",
            "38400",
            "57600",
            "115200"});
            this.cbRate.Location = new System.Drawing.Point(171, 12);
            this.cbRate.Margin = new System.Windows.Forms.Padding(4);
            this.cbRate.Name = "cbRate";
            this.cbRate.Size = new System.Drawing.Size(153, 22);
            this.cbRate.TabIndex = 1;
            // 
            // lbBit
            // 
            this.lbBit.AutoSize = true;
            this.lbBit.Location = new System.Drawing.Point(16, 58);
            this.lbBit.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbBit.Name = "lbBit";
            this.lbBit.Size = new System.Drawing.Size(127, 14);
            this.lbBit.TabIndex = 2;
            this.lbBit.Text = "Stop Bit and Parity Bit:";
            // 
            // cbBit
            // 
            this.cbBit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbBit.FormattingEnabled = true;
            this.cbBit.Location = new System.Drawing.Point(171, 55);
            this.cbBit.Margin = new System.Windows.Forms.Padding(4);
            this.cbBit.Name = "cbBit";
            this.cbBit.Size = new System.Drawing.Size(153, 22);
            this.cbBit.TabIndex = 3;
            // 
            // btnSave
            // 
            this.btnSave.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.Image = global::HDL_Buspro_Setup_Tool.Properties.Resources.Save;
            this.btnSave.Location = new System.Drawing.Point(262, 92);
            this.btnSave.Margin = new System.Windows.Forms.Padding(4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(100, 24);
            this.btnSave.TabIndex = 4;
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnRead
            // 
            this.btnRead.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRead.Image = global::HDL_Buspro_Setup_Tool.Properties.Resources.Refresh;
            this.btnRead.Location = new System.Drawing.Point(126, 92);
            this.btnRead.Margin = new System.Windows.Forms.Padding(4);
            this.btnRead.Name = "btnRead";
            this.btnRead.Size = new System.Drawing.Size(100, 24);
            this.btnRead.TabIndex = 6;
            this.btnRead.UseVisualStyleBackColor = true;
            this.btnRead.Click += new System.EventHandler(this.btnRead_Click);
            // 
            // FrmBoundRate
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(378, 125);
            this.Controls.Add(this.btnRead);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.cbBit);
            this.Controls.Add(this.lbBit);
            this.Controls.Add(this.cbRate);
            this.Controls.Add(this.lbRate);
            this.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmBoundRate";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Config Seiral Port";
            this.Load += new System.EventHandler(this.FrmBoundRate_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbRate;
        private System.Windows.Forms.ComboBox cbRate;
        private System.Windows.Forms.Label lbBit;
        private System.Windows.Forms.ComboBox cbBit;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnRead;
    }
}