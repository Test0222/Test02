
partial class ConstantLux
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
            this.chbEnable = new System.Windows.Forms.CheckBox();
            this.NumLux = new System.Windows.Forms.NumericUpDown();
            this.lbCycle = new System.Windows.Forms.Label();
            this.cboCycle = new System.Windows.Forms.ComboBox();
            this.NumKp = new System.Windows.Forms.ComboBox();
            this.Kp = new System.Windows.Forms.Label();
            this.NumKi = new System.Windows.Forms.ComboBox();
            this.Ki = new System.Windows.Forms.Label();
            this.lbLow = new System.Windows.Forms.Label();
            this.sbLow = new System.Windows.Forms.HScrollBar();
            this.lbLowValue = new System.Windows.Forms.Label();
            this.lbHint = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.NumLux)).BeginInit();
            this.SuspendLayout();
            // 
            // chbEnable
            // 
            this.chbEnable.AutoSize = true;
            this.chbEnable.Location = new System.Drawing.Point(3, 13);
            this.chbEnable.Name = "chbEnable";
            this.chbEnable.Size = new System.Drawing.Size(135, 18);
            this.chbEnable.TabIndex = 0;
            this.chbEnable.Text = "Enable Constant Lux";
            this.chbEnable.UseVisualStyleBackColor = true;
            this.chbEnable.CheckedChanged += new System.EventHandler(this.chbEnable_CheckedChanged);
            // 
            // NumLux
            // 
            this.NumLux.Enabled = false;
            this.NumLux.Location = new System.Drawing.Point(133, 10);
            this.NumLux.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.NumLux.Name = "NumLux";
            this.NumLux.Size = new System.Drawing.Size(96, 22);
            this.NumLux.TabIndex = 1;
            this.NumLux.ValueChanged += new System.EventHandler(this.NumLux_ValueChanged);
            // 
            // lbCycle
            // 
            this.lbCycle.AutoSize = true;
            this.lbCycle.Location = new System.Drawing.Point(3, 42);
            this.lbCycle.Name = "lbCycle";
            this.lbCycle.Size = new System.Drawing.Size(117, 14);
            this.lbCycle.TabIndex = 2;
            this.lbCycle.Text = "Control cycle(0.1-5s):";
            // 
            // cboCycle
            // 
            this.cboCycle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboCycle.Enabled = false;
            this.cboCycle.FormattingEnabled = true;
            this.cboCycle.Location = new System.Drawing.Point(133, 39);
            this.cboCycle.Name = "cboCycle";
            this.cboCycle.Size = new System.Drawing.Size(96, 22);
            this.cboCycle.TabIndex = 3;
            this.cboCycle.SelectedIndexChanged += new System.EventHandler(this.cboCycle_SelectedIndexChanged);
            // 
            // NumKp
            // 
            this.NumKp.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.NumKp.Enabled = false;
            this.NumKp.FormattingEnabled = true;
            this.NumKp.Location = new System.Drawing.Point(46, 90);
            this.NumKp.Name = "NumKp";
            this.NumKp.Size = new System.Drawing.Size(55, 22);
            this.NumKp.TabIndex = 5;
            this.NumKp.SelectedIndexChanged += new System.EventHandler(this.NumKp_SelectedIndexChanged);
            // 
            // Kp
            // 
            this.Kp.AutoSize = true;
            this.Kp.Location = new System.Drawing.Point(3, 93);
            this.Kp.Name = "Kp";
            this.Kp.Size = new System.Drawing.Size(23, 14);
            this.Kp.TabIndex = 4;
            this.Kp.Text = "Kp:";
            // 
            // NumKi
            // 
            this.NumKi.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.NumKi.Enabled = false;
            this.NumKi.FormattingEnabled = true;
            this.NumKi.Location = new System.Drawing.Point(166, 93);
            this.NumKi.Name = "NumKi";
            this.NumKi.Size = new System.Drawing.Size(63, 22);
            this.NumKi.TabIndex = 7;
            this.NumKi.SelectedIndexChanged += new System.EventHandler(this.NumKi_SelectedIndexChanged);
            // 
            // Ki
            // 
            this.Ki.AutoSize = true;
            this.Ki.Location = new System.Drawing.Point(130, 96);
            this.Ki.Name = "Ki";
            this.Ki.Size = new System.Drawing.Size(20, 14);
            this.Ki.TabIndex = 6;
            this.Ki.Text = "Ki:";
            // 
            // lbLow
            // 
            this.lbLow.AutoSize = true;
            this.lbLow.Location = new System.Drawing.Point(3, 69);
            this.lbLow.Name = "lbLow";
            this.lbLow.Size = new System.Drawing.Size(86, 14);
            this.lbLow.TabIndex = 8;
            this.lbLow.Text = "Low Limit Dim:";
            // 
            // sbLow
            // 
            this.sbLow.Enabled = false;
            this.sbLow.LargeChange = 1;
            this.sbLow.Location = new System.Drawing.Point(133, 69);
            this.sbLow.Name = "sbLow";
            this.sbLow.Size = new System.Drawing.Size(96, 19);
            this.sbLow.TabIndex = 9;
            this.sbLow.Scroll += new System.Windows.Forms.ScrollEventHandler(this.sbLow_Scroll);
            this.sbLow.ValueChanged += new System.EventHandler(this.sbLow_ValueChanged);
            // 
            // lbLowValue
            // 
            this.lbLowValue.AutoSize = true;
            this.lbLowValue.Location = new System.Drawing.Point(232, 69);
            this.lbLowValue.Name = "lbLowValue";
            this.lbLowValue.Size = new System.Drawing.Size(13, 14);
            this.lbLowValue.TabIndex = 10;
            this.lbLowValue.Text = "0";
            // 
            // lbHint
            // 
            this.lbHint.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lbHint.Font = new System.Drawing.Font("Times New Roman", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbHint.ForeColor = System.Drawing.Color.Blue;
            this.lbHint.Location = new System.Drawing.Point(0, 119);
            this.lbHint.Name = "lbHint";
            this.lbHint.Size = new System.Drawing.Size(257, 36);
            this.lbHint.TabIndex = 11;
            this.lbHint.Text = "Tip: only works with Logic 1, when it is true and its first command is \"Single li" +
    "ght control\".";
            // 
            // ConstantLux
            // 
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.lbHint);
            this.Controls.Add(this.lbLowValue);
            this.Controls.Add(this.sbLow);
            this.Controls.Add(this.lbLow);
            this.Controls.Add(this.NumKi);
            this.Controls.Add(this.Ki);
            this.Controls.Add(this.NumKp);
            this.Controls.Add(this.Kp);
            this.Controls.Add(this.cboCycle);
            this.Controls.Add(this.lbCycle);
            this.Controls.Add(this.NumLux);
            this.Controls.Add(this.chbEnable);
            this.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "ConstantLux";
            this.Size = new System.Drawing.Size(257, 155);
            this.Load += new System.EventHandler(this.ConstantLux_Load);
            ((System.ComponentModel.ISupportInitialize)(this.NumLux)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox chbEnable;
        private System.Windows.Forms.NumericUpDown NumLux;
        private System.Windows.Forms.Label lbCycle;
        private System.Windows.Forms.ComboBox cboCycle;
        private System.Windows.Forms.ComboBox NumKp;
        private System.Windows.Forms.Label Kp;
        private System.Windows.Forms.ComboBox NumKi;
        private System.Windows.Forms.Label Ki;
        private System.Windows.Forms.Label lbLow;
        private System.Windows.Forms.HScrollBar sbLow;
        private System.Windows.Forms.Label lbLowValue;
        private System.Windows.Forms.Label lbHint;



    }

