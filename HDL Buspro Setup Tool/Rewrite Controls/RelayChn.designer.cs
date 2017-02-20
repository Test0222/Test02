
partial class RelayChn
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
            this.Ron = new System.Windows.Forms.RadioButton();
            this.Roff = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // Ron
            // 
            this.Ron.Checked = true;
            this.Ron.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Ron.Location = new System.Drawing.Point(66, 4);
            this.Ron.Name = "Ron";
            this.Ron.Size = new System.Drawing.Size(51, 16);
            this.Ron.TabIndex = 0;
            this.Ron.Tag = "1";
            this.Ron.Text = "ON";
            this.Ron.UseVisualStyleBackColor = true;
            this.Ron.CheckedChanged += new System.EventHandler(this.Ron_CheckedChanged);
            // 
            // Roff
            // 
            this.Roff.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Roff.Location = new System.Drawing.Point(10, 3);
            this.Roff.Name = "Roff";
            this.Roff.Size = new System.Drawing.Size(50, 18);
            this.Roff.TabIndex = 1;
            this.Roff.Tag = "0";
            this.Roff.Text = "OFF";
            this.Roff.UseVisualStyleBackColor = true;
            this.Roff.CheckedChanged += new System.EventHandler(this.Roff_CheckedChanged);
            // 
            // RelayChn
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.Roff);
            this.Controls.Add(this.Ron);
            this.Font = new System.Drawing.Font("Times New Roman", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "RelayChn";
            this.Size = new System.Drawing.Size(120, 28);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RadioButton Ron;
        private System.Windows.Forms.RadioButton Roff;

    }

