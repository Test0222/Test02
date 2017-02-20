namespace HDL_Buspro_Setup_Tool
{
    partial class FrmDownloadShow
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
            this.lblProgressValue = new System.Windows.Forms.Label();
            this.probStatus = new System.Windows.Forms.ProgressBar();
            this.lbName = new System.Windows.Forms.Label();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnRestart = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblProgressValue
            // 
            this.lblProgressValue.Location = new System.Drawing.Point(232, 15);
            this.lblProgressValue.Name = "lblProgressValue";
            this.lblProgressValue.Size = new System.Drawing.Size(78, 24);
            this.lblProgressValue.TabIndex = 5;
            this.lblProgressValue.Text = "label2";
            this.lblProgressValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // probStatus
            // 
            this.probStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.probStatus.Location = new System.Drawing.Point(10, 47);
            this.probStatus.Name = "probStatus";
            this.probStatus.Size = new System.Drawing.Size(315, 22);
            this.probStatus.TabIndex = 4;
            // 
            // lbName
            // 
            this.lbName.Location = new System.Drawing.Point(12, 15);
            this.lbName.Name = "lbName";
            this.lbName.Size = new System.Drawing.Size(202, 24);
            this.lbName.TabIndex = 10;
            // 
            // btnExit
            // 
            this.btnExit.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnExit.Location = new System.Drawing.Point(235, 82);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(75, 28);
            this.btnExit.TabIndex = 7;
            this.btnExit.Text = "Exit";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnStop
            // 
            this.btnStop.Enabled = false;
            this.btnStop.Location = new System.Drawing.Point(40, 82);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(75, 28);
            this.btnStop.TabIndex = 6;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnRestart
            // 
            this.btnRestart.Enabled = false;
            this.btnRestart.Location = new System.Drawing.Point(139, 82);
            this.btnRestart.Name = "btnRestart";
            this.btnRestart.Size = new System.Drawing.Size(75, 28);
            this.btnRestart.TabIndex = 11;
            this.btnRestart.Text = "Try Again";
            this.btnRestart.UseVisualStyleBackColor = true;
            this.btnRestart.Click += new System.EventHandler(this.btnRestart_Click);
            // 
            // FrmDownloadShow
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(341, 120);
            this.Controls.Add(this.btnRestart);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.lbName);
            this.Controls.Add(this.lblProgressValue);
            this.Controls.Add(this.probStatus);
            this.Font = new System.Drawing.Font("Calibri", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmDownloadShow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Downloading...";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmDownloadShow_FormClosing);
            this.Load += new System.EventHandler(this.FrmDownloadShow_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblProgressValue;
        private System.Windows.Forms.ProgressBar probStatus;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Label lbName;
        private System.Windows.Forms.Button btnRestart;
    }
}