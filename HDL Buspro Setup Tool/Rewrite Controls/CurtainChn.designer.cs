
partial class CurtainChn
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.Rstop = new System.Windows.Forms.RadioButton();
            this.Rclose = new System.Windows.Forms.RadioButton();
            this.Ropen = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // Rstop
            // 
            this.Rstop.AutoSize = true;
            this.Rstop.Checked = true;
            this.Rstop.Location = new System.Drawing.Point(63, 8);
            this.Rstop.Name = "Rstop";
            this.Rstop.Size = new System.Drawing.Size(49, 18);
            this.Rstop.TabIndex = 0;
            this.Rstop.TabStop = true;
            this.Rstop.Tag = "0";
            this.Rstop.Text = "Stop";
            this.Rstop.UseVisualStyleBackColor = true;
            this.Rstop.CheckedChanged += new System.EventHandler(this.Ron_CheckedChanged);
            // 
            // Rclose
            // 
            this.Rclose.AutoSize = true;
            this.Rclose.Checked = true;
            this.Rclose.Location = new System.Drawing.Point(3, 8);
            this.Rclose.Name = "Rclose";
            this.Rclose.Size = new System.Drawing.Size(55, 18);
            this.Rclose.TabIndex = 1;
            this.Rclose.TabStop = true;
            this.Rclose.Tag = "2";
            this.Rclose.Text = "Close";
            this.Rclose.UseVisualStyleBackColor = true;
            // 
            // Ropen
            // 
            this.Ropen.AutoSize = true;
            this.Ropen.Checked = true;
            this.Ropen.Location = new System.Drawing.Point(120, 8);
            this.Ropen.Name = "Ropen";
            this.Ropen.Size = new System.Drawing.Size(54, 18);
            this.Ropen.TabIndex = 2;
            this.Ropen.TabStop = true;
            this.Ropen.Tag = "1";
            this.Ropen.Text = "Open";
            this.Ropen.UseVisualStyleBackColor = true;
            // 
            // CurtainChn
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.Ropen);
            this.Controls.Add(this.Rclose);
            this.Controls.Add(this.Rstop);
            this.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "CurtainChn";
            this.Size = new System.Drawing.Size(176, 28);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton Rstop;
        private System.Windows.Forms.RadioButton Rclose;
        private System.Windows.Forms.RadioButton Ropen;

    }

