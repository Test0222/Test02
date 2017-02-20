namespace HDL_Buspro_Setup_Tool
{
    partial class UnivasalControl
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
            this.txt1 = new System.Windows.Forms.TextBox();
            this.lb = new System.Windows.Forms.Label();
            this.txt2 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // txt1
            // 
            this.txt1.Dock = System.Windows.Forms.DockStyle.Left;
            this.txt1.Location = new System.Drawing.Point(0, 0);
            this.txt1.Name = "txt1";
            this.txt1.Size = new System.Drawing.Size(55, 22);
            this.txt1.TabIndex = 0;
            this.txt1.TextChanged += new System.EventHandler(this.txt1_TextChanged);
            this.txt1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txt1_KeyPress);
            // 
            // lb
            // 
            this.lb.Location = new System.Drawing.Point(59, 3);
            this.lb.Name = "lb";
            this.lb.Size = new System.Drawing.Size(12, 14);
            this.lb.TabIndex = 1;
            this.lb.Text = "/";
            // 
            // txt2
            // 
            this.txt2.Dock = System.Windows.Forms.DockStyle.Right;
            this.txt2.Location = new System.Drawing.Point(75, 0);
            this.txt2.Name = "txt2";
            this.txt2.Size = new System.Drawing.Size(55, 22);
            this.txt2.TabIndex = 2;
            this.txt2.TextChanged += new System.EventHandler(this.txt2_TextChanged);
            this.txt2.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txt1_KeyPress);
            // 
            // UnivasalControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.txt2);
            this.Controls.Add(this.lb);
            this.Controls.Add(this.txt1);
            this.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "UnivasalControl";
            this.Size = new System.Drawing.Size(130, 22);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txt1;
        private System.Windows.Forms.Label lb;
        private System.Windows.Forms.TextBox txt2;
    }
}
