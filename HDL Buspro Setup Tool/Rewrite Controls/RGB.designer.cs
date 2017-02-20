
partial class RGB
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
            this.btnMore = new System.Windows.Forms.Button();
            this.Pcolor = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // btnMore
            // 
            this.btnMore.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnMore.Location = new System.Drawing.Point(98, 0);
            this.btnMore.Name = "btnMore";
            this.btnMore.Size = new System.Drawing.Size(31, 25);
            this.btnMore.TabIndex = 1;
            this.btnMore.Text = ">>";
            this.btnMore.UseVisualStyleBackColor = true;
            this.btnMore.Click += new System.EventHandler(this.btnMore_Click);
            // 
            // Pcolor
            // 
            this.Pcolor.BackColor = System.Drawing.Color.Black;
            this.Pcolor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Pcolor.Location = new System.Drawing.Point(0, 0);
            this.Pcolor.Name = "Pcolor";
            this.Pcolor.Size = new System.Drawing.Size(98, 25);
            this.Pcolor.TabIndex = 2;
            this.Pcolor.Paint += new System.Windows.Forms.PaintEventHandler(this.Pcolor_Paint);
            // 
            // RGB
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.Pcolor);
            this.Controls.Add(this.btnMore);
            this.Font = new System.Drawing.Font("Times New Roman", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "RGB";
            this.Size = new System.Drawing.Size(129, 25);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnMore;
        private System.Windows.Forms.Panel Pcolor;

    }

