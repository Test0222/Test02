namespace HDL_Buspro_Setup_Tool
{
    partial class IRControl
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
            this.cb1 = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // txt1
            // 
            this.txt1.Dock = System.Windows.Forms.DockStyle.Left;
            this.txt1.Location = new System.Drawing.Point(0, 0);
            this.txt1.Name = "txt1";
            this.txt1.Size = new System.Drawing.Size(60, 21);
            this.txt1.TabIndex = 0;
            this.txt1.TextChanged += new System.EventHandler(this.txt1_TextChanged);
            this.txt1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txt1_KeyPress);
            // 
            // lb
            // 
            this.lb.AutoSize = true;
            this.lb.Location = new System.Drawing.Point(66, 3);
            this.lb.Name = "lb";
            this.lb.Size = new System.Drawing.Size(11, 12);
            this.lb.TabIndex = 1;
            this.lb.Text = "/";
            // 
            // cb1
            // 
            this.cb1.Dock = System.Windows.Forms.DockStyle.Right;
            this.cb1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb1.FormattingEnabled = true;
            this.cb1.Location = new System.Drawing.Point(84, 0);
            this.cb1.Name = "cb1";
            this.cb1.Size = new System.Drawing.Size(66, 20);
            this.cb1.TabIndex = 2;
            this.cb1.SelectedIndexChanged += new System.EventHandler(this.cb1_SelectedIndexChanged);
            // 
            // IRControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cb1);
            this.Controls.Add(this.lb);
            this.Controls.Add(this.txt1);
            this.Name = "IRControl";
            this.Size = new System.Drawing.Size(150, 21);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txt1;
        private System.Windows.Forms.Label lb;
        private System.Windows.Forms.ComboBox cb1;
    }
}
