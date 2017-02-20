namespace HDL_Buspro_Setup_Tool
{
    partial class FrmChnAndKey
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
            this.cbKey = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.cbChannel = new System.Windows.Forms.ComboBox();
            this.lbChannel = new System.Windows.Forms.Label();
            this.pic2 = new System.Windows.Forms.Button();
            this.pic1 = new System.Windows.Forms.Button();
            this.panel9 = new System.Windows.Forms.Panel();
            this.cbWind = new System.Windows.Forms.ComboBox();
            this.lbWind = new System.Windows.Forms.Label();
            this.TempValue = new System.Windows.Forms.Label();
            this.cbFan = new System.Windows.Forms.ComboBox();
            this.lbFAN = new System.Windows.Forms.Label();
            this.sbTemp = new System.Windows.Forms.HScrollBar();
            this.lbTemp = new System.Windows.Forms.Label();
            this.cbMode = new System.Windows.Forms.ComboBox();
            this.lbMode = new System.Windows.Forms.Label();
            this.cbPower = new System.Windows.Forms.ComboBox();
            this.lbPower = new System.Windows.Forms.Label();
            this.panel9.SuspendLayout();
            this.SuspendLayout();
            // 
            // cbKey
            // 
            this.cbKey.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbKey.FormattingEnabled = true;
            this.cbKey.Location = new System.Drawing.Point(109, 42);
            this.cbKey.Margin = new System.Windows.Forms.Padding(4);
            this.cbKey.Name = "cbKey";
            this.cbKey.Size = new System.Drawing.Size(181, 27);
            this.cbKey.TabIndex = 12;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(17, 46);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(62, 19);
            this.label8.TabIndex = 11;
            this.label8.Text = "Key No.:";
            // 
            // cbChannel
            // 
            this.cbChannel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbChannel.FormattingEnabled = true;
            this.cbChannel.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4"});
            this.cbChannel.Location = new System.Drawing.Point(109, 5);
            this.cbChannel.Margin = new System.Windows.Forms.Padding(4);
            this.cbChannel.Name = "cbChannel";
            this.cbChannel.Size = new System.Drawing.Size(181, 27);
            this.cbChannel.TabIndex = 10;
            // 
            // lbChannel
            // 
            this.lbChannel.AutoSize = true;
            this.lbChannel.Location = new System.Drawing.Point(17, 9);
            this.lbChannel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbChannel.Name = "lbChannel";
            this.lbChannel.Size = new System.Drawing.Size(66, 19);
            this.lbChannel.TabIndex = 9;
            this.lbChannel.Text = "Channel:";
            // 
            // pic2
            // 
            this.pic2.Location = new System.Drawing.Point(274, 103);
            this.pic2.Name = "pic2";
            this.pic2.Size = new System.Drawing.Size(120, 40);
            this.pic2.TabIndex = 16;
            this.pic2.TabStop = false;
            this.pic2.Text = "Close";
            this.pic2.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // pic1
            // 
            this.pic1.Location = new System.Drawing.Point(34, 103);
            this.pic1.Name = "pic1";
            this.pic1.Size = new System.Drawing.Size(120, 40);
            this.pic1.TabIndex = 15;
            this.pic1.TabStop = false;
            this.pic1.Text = "Test";
            this.pic1.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // panel9
            // 
            this.panel9.Controls.Add(this.cbWind);
            this.panel9.Controls.Add(this.lbWind);
            this.panel9.Controls.Add(this.TempValue);
            this.panel9.Controls.Add(this.cbFan);
            this.panel9.Controls.Add(this.lbFAN);
            this.panel9.Controls.Add(this.sbTemp);
            this.panel9.Controls.Add(this.lbTemp);
            this.panel9.Controls.Add(this.cbMode);
            this.panel9.Controls.Add(this.lbMode);
            this.panel9.Controls.Add(this.cbPower);
            this.panel9.Controls.Add(this.lbPower);
            this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel9.Location = new System.Drawing.Point(0, 0);
            this.panel9.Name = "panel9";
            this.panel9.Size = new System.Drawing.Size(474, 97);
            this.panel9.TabIndex = 35;
            this.panel9.Visible = false;
            // 
            // cbWind
            // 
            this.cbWind.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbWind.FormattingEnabled = true;
            this.cbWind.Location = new System.Drawing.Point(333, 33);
            this.cbWind.Name = "cbWind";
            this.cbWind.Size = new System.Drawing.Size(100, 27);
            this.cbWind.TabIndex = 10;
            // 
            // lbWind
            // 
            this.lbWind.AutoSize = true;
            this.lbWind.Location = new System.Drawing.Point(273, 36);
            this.lbWind.Name = "lbWind";
            this.lbWind.Size = new System.Drawing.Size(47, 19);
            this.lbWind.TabIndex = 9;
            this.lbWind.Text = "Wind:";
            // 
            // TempValue
            // 
            this.TempValue.AutoSize = true;
            this.TempValue.Location = new System.Drawing.Point(330, 74);
            this.TempValue.Name = "TempValue";
            this.TempValue.Size = new System.Drawing.Size(34, 19);
            this.TempValue.TabIndex = 8;
            this.TempValue.Text = "16C";
            // 
            // cbFan
            // 
            this.cbFan.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbFan.FormattingEnabled = true;
            this.cbFan.Location = new System.Drawing.Point(128, 33);
            this.cbFan.Name = "cbFan";
            this.cbFan.Size = new System.Drawing.Size(100, 27);
            this.cbFan.TabIndex = 5;
            // 
            // lbFAN
            // 
            this.lbFAN.AutoSize = true;
            this.lbFAN.Location = new System.Drawing.Point(7, 38);
            this.lbFAN.Name = "lbFAN";
            this.lbFAN.Size = new System.Drawing.Size(38, 19);
            this.lbFAN.TabIndex = 4;
            this.lbFAN.Text = "FAN:";
            // 
            // sbTemp
            // 
            this.sbTemp.LargeChange = 1;
            this.sbTemp.Location = new System.Drawing.Point(128, 73);
            this.sbTemp.Maximum = 30;
            this.sbTemp.Name = "sbTemp";
            this.sbTemp.Size = new System.Drawing.Size(189, 17);
            this.sbTemp.TabIndex = 7;
            this.sbTemp.Value = 16;
            this.sbTemp.ValueChanged += new System.EventHandler(this.sbTemp_ValueChanged);
            // 
            // lbTemp
            // 
            this.lbTemp.AutoSize = true;
            this.lbTemp.Location = new System.Drawing.Point(7, 72);
            this.lbTemp.Name = "lbTemp";
            this.lbTemp.Size = new System.Drawing.Size(95, 19);
            this.lbTemp.TabIndex = 6;
            this.lbTemp.Text = "Temperature:";
            // 
            // cbMode
            // 
            this.cbMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbMode.FormattingEnabled = true;
            this.cbMode.Location = new System.Drawing.Point(333, 3);
            this.cbMode.Name = "cbMode";
            this.cbMode.Size = new System.Drawing.Size(100, 27);
            this.cbMode.TabIndex = 3;
            // 
            // lbMode
            // 
            this.lbMode.AutoSize = true;
            this.lbMode.Location = new System.Drawing.Point(270, 6);
            this.lbMode.Name = "lbMode";
            this.lbMode.Size = new System.Drawing.Size(50, 19);
            this.lbMode.TabIndex = 2;
            this.lbMode.Text = "Mode:";
            // 
            // cbPower
            // 
            this.cbPower.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbPower.FormattingEnabled = true;
            this.cbPower.Location = new System.Drawing.Point(128, 3);
            this.cbPower.Name = "cbPower";
            this.cbPower.Size = new System.Drawing.Size(100, 27);
            this.cbPower.TabIndex = 1;
            // 
            // lbPower
            // 
            this.lbPower.AutoSize = true;
            this.lbPower.Location = new System.Drawing.Point(7, 8);
            this.lbPower.Name = "lbPower";
            this.lbPower.Size = new System.Drawing.Size(100, 19);
            this.lbPower.TabIndex = 0;
            this.lbPower.Text = "Power source:";
            // 
            // FrmChnAndKey
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(474, 149);
            this.Controls.Add(this.panel9);
            this.Controls.Add(this.pic2);
            this.Controls.Add(this.pic1);
            this.Controls.Add(this.cbKey);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.cbChannel);
            this.Controls.Add(this.lbChannel);
            this.Font = new System.Drawing.Font("Calibri", 12F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmChnAndKey";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FrmChnAndKey";
            this.Load += new System.EventHandler(this.FrmChnAndKey_Load);
            this.panel9.ResumeLayout(false);
            this.panel9.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cbKey;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox cbChannel;
        private System.Windows.Forms.Label lbChannel;
        private System.Windows.Forms.Button pic2;
        private System.Windows.Forms.Button pic1;
        private System.Windows.Forms.Panel panel9;
        private System.Windows.Forms.ComboBox cbWind;
        private System.Windows.Forms.Label lbWind;
        private System.Windows.Forms.Label TempValue;
        private System.Windows.Forms.HScrollBar sbTemp;
        private System.Windows.Forms.Label lbTemp;
        private System.Windows.Forms.ComboBox cbFan;
        private System.Windows.Forms.Label lbFAN;
        private System.Windows.Forms.ComboBox cbMode;
        private System.Windows.Forms.Label lbMode;
        private System.Windows.Forms.ComboBox cbPower;
        private System.Windows.Forms.Label lbPower;
    }
}