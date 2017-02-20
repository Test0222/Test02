namespace HDL_Buspro_Setup_Tool
{
    partial class BackNetID
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
            this.cbID = new System.Windows.Forms.ComboBox();
            this.txtID = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // cbID
            // 
            this.cbID.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbID.FormattingEnabled = true;
            this.cbID.Items.AddRange(new object[] {
            "AI",
            "AO",
            "AV",
            "BI",
            "BO",
            "BV",
            "MI",
            "MO",
            "MV",
            "Others"});
            this.cbID.Location = new System.Drawing.Point(0, 0);
            this.cbID.Name = "cbID";
            this.cbID.Size = new System.Drawing.Size(55, 20);
            this.cbID.TabIndex = 0;
            this.cbID.SelectedIndexChanged += new System.EventHandler(this.cbID_SelectedIndexChanged);
            // 
            // txtID
            // 
            this.txtID.Location = new System.Drawing.Point(58, 1);
            this.txtID.Name = "txtID";
            this.txtID.Size = new System.Drawing.Size(55, 21);
            this.txtID.TabIndex = 1;
            this.txtID.TextChanged += new System.EventHandler(this.txtID_TextChanged);
            // 
            // BackNetID
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.txtID);
            this.Controls.Add(this.cbID);
            this.Name = "BackNetID";
            this.Size = new System.Drawing.Size(115, 22);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cbID;
        private System.Windows.Forms.TextBox txtID;
    }
}
