namespace HDL_Buspro_Setup_Tool
{
    partial class FrmCalibrationLux
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
            this.pic2 = new System.Windows.Forms.Button();
            this.pic1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtLux = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // pic2
            // 
            this.pic2.Location = new System.Drawing.Point(174, 44);
            this.pic2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.pic2.Name = "pic2";
            this.pic2.Size = new System.Drawing.Size(100, 24);
            this.pic2.TabIndex = 18;
            this.pic2.TabStop = false;
            this.pic2.Text = "Close";
            this.pic2.Click += new System.EventHandler(this.pic2_Click);
            // 
            // pic1
            // 
            this.pic1.Location = new System.Drawing.Point(18, 44);
            this.pic1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.pic1.Name = "pic1";
            this.pic1.Size = new System.Drawing.Size(100, 24);
            this.pic1.TabIndex = 17;
            this.pic1.TabStop = false;
            this.pic1.Text = "Save";
            this.pic1.Click += new System.EventHandler(this.pic1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(13, 10);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 15);
            this.label1.TabIndex = 19;
            this.label1.Text = "Input Lux(0-100):";
            // 
            // txtLux
            // 
            this.txtLux.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLux.Location = new System.Drawing.Point(172, 7);
            this.txtLux.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtLux.Name = "txtLux";
            this.txtLux.Size = new System.Drawing.Size(89, 23);
            this.txtLux.TabIndex = 20;
            this.txtLux.Text = "10";
            this.txtLux.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtLux_KeyPress);
            this.txtLux.Leave += new System.EventHandler(this.txtLux_Leave);
            // 
            // FrmCalibrationLux
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(298, 73);
            this.Controls.Add(this.txtLux);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pic2);
            this.Controls.Add(this.pic1);
            this.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmCalibrationLux";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Adjust Lux";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button pic2;
        private System.Windows.Forms.Button pic1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtLux;
    }
}