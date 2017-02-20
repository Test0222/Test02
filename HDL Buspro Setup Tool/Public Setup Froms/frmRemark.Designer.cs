namespace HDL_Buspro_Setup_Tool
{
    partial class frmRemark
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
            this.components = new System.ComponentModel.Container();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.lbSum = new System.Windows.Forms.Label();
            this.tbRmName = new System.Windows.Forms.TextBox();
            this.lbRmName = new System.Windows.Forms.Label();
            this.tbNew = new System.Windows.Forms.TextBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(199, 101);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(120, 25);
            this.btnExit.TabIndex = 5;
            this.btnExit.Text = "Cancel";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(24, 101);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(120, 25);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // lbSum
            // 
            this.lbSum.AutoSize = true;
            this.lbSum.Location = new System.Drawing.Point(10, 65);
            this.lbSum.Name = "lbSum";
            this.lbSum.Size = new System.Drawing.Size(69, 14);
            this.lbSum.TabIndex = 3;
            this.lbSum.Text = "New Name:";
            // 
            // tbRmName
            // 
            this.tbRmName.Location = new System.Drawing.Point(99, 18);
            this.tbRmName.Name = "tbRmName";
            this.tbRmName.ReadOnly = true;
            this.tbRmName.Size = new System.Drawing.Size(233, 22);
            this.tbRmName.TabIndex = 1;
            this.tbRmName.Text = "<Press Name>";
            // 
            // lbRmName
            // 
            this.lbRmName.AutoSize = true;
            this.lbRmName.Location = new System.Drawing.Point(10, 22);
            this.lbRmName.Name = "lbRmName";
            this.lbRmName.Size = new System.Drawing.Size(67, 14);
            this.lbRmName.TabIndex = 2;
            this.lbRmName.Text = " Old Name:";
            // 
            // tbNew
            // 
            this.tbNew.Location = new System.Drawing.Point(99, 62);
            this.tbNew.MaxLength = 20;
            this.tbNew.Name = "tbNew";
            this.tbNew.Size = new System.Drawing.Size(233, 22);
            this.tbNew.TabIndex = 0;
            this.tbNew.Text = "<Press Name>";
            this.toolTip1.SetToolTip(this.tbNew, "<Press Name>");
            // 
            // toolTip1
            // 
            this.toolTip1.AutoPopDelay = 1000;
            this.toolTip1.InitialDelay = 500;
            this.toolTip1.IsBalloon = true;
            this.toolTip1.ReshowDelay = 100;
            // 
            // frmRemark
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(344, 138);
            this.Controls.Add(this.tbNew);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.lbSum);
            this.Controls.Add(this.tbRmName);
            this.Controls.Add(this.lbRmName);
            this.Font = new System.Drawing.Font("Calibri", 9F);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(360, 176);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(360, 176);
            this.Name = "frmRemark";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Rename Device";
            this.Load += new System.EventHandler(this.frmRemark_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label lbSum;
        private System.Windows.Forms.TextBox tbRmName;
        private System.Windows.Forms.Label lbRmName;
        private System.Windows.Forms.TextBox tbNew;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}