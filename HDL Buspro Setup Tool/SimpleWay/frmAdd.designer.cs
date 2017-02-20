namespace HDL_Buspro_Setup_Tool
{
    partial class frmAdd
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
            this.lbRmName = new System.Windows.Forms.Label();
            this.tbRmName = new System.Windows.Forms.TextBox();
            this.lbSum = new System.Windows.Forms.Label();
            this.ndSum = new System.Windows.Forms.NumericUpDown();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.ndSum)).BeginInit();
            this.SuspendLayout();
            // 
            // lbRmName
            // 
            this.lbRmName.AutoSize = true;
            this.lbRmName.Location = new System.Drawing.Point(19, 18);
            this.lbRmName.Name = "lbRmName";
            this.lbRmName.Size = new System.Drawing.Size(45, 14);
            this.lbRmName.TabIndex = 0;
            this.lbRmName.Text = " Name:";
            // 
            // tbRmName
            // 
            this.tbRmName.Location = new System.Drawing.Point(86, 15);
            this.tbRmName.Name = "tbRmName";
            this.tbRmName.Size = new System.Drawing.Size(149, 22);
            this.tbRmName.TabIndex = 1;
            this.tbRmName.Text = "<Press Name>";
            // 
            // lbSum
            // 
            this.lbSum.AutoSize = true;
            this.lbSum.Location = new System.Drawing.Point(19, 56);
            this.lbSum.Name = "lbSum";
            this.lbSum.Size = new System.Drawing.Size(66, 14);
            this.lbSum.TabIndex = 2;
            this.lbSum.Text = "How many:";
            // 
            // ndSum
            // 
            this.ndSum.Location = new System.Drawing.Point(86, 54);
            this.ndSum.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.ndSum.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.ndSum.Name = "ndSum";
            this.ndSum.Size = new System.Drawing.Size(149, 22);
            this.ndSum.TabIndex = 3;
            this.ndSum.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(79, 92);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 25);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(160, 92);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(75, 25);
            this.btnExit.TabIndex = 5;
            this.btnExit.Text = "Cancel";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // frmAdd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(264, 129);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.ndSum);
            this.Controls.Add(this.lbSum);
            this.Controls.Add(this.tbRmName);
            this.Controls.Add(this.lbRmName);
            this.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "frmAdd";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "New Add";
            this.Load += new System.EventHandler(this.frmAdd_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ndSum)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbRmName;
        private System.Windows.Forms.TextBox tbRmName;
        private System.Windows.Forms.Label lbSum;
        private System.Windows.Forms.NumericUpDown ndSum;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnExit;
    }
}