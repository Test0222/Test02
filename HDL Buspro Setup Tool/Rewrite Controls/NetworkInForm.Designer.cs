namespace HDL_Buspro_Setup_Tool
{
    partial class NetworkInForm
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.Dns1 = new HDL_Buspro_Setup_Tool.IPAddressNew();
            this.DNS = new HDL_Buspro_Setup_Tool.IPAddressNew();
            this.btnModify = new System.Windows.Forms.Button();
            this.btnRead = new System.Windows.Forms.Button();
            this.tbPort = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.txt6 = new System.Windows.Forms.TextBox();
            this.txt5 = new System.Windows.Forms.TextBox();
            this.txt4 = new System.Windows.Forms.TextBox();
            this.txt1 = new System.Windows.Forms.TextBox();
            this.lbAuto = new System.Windows.Forms.Label();
            this.MaskIP = new HDL_Buspro_Setup_Tool.IPAddressNew();
            this.txtSwRoutIP = new HDL_Buspro_Setup_Tool.IPAddressNew();
            this.txtSwiIP = new HDL_Buspro_Setup_Tool.IPAddressNew();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lbDns = new System.Windows.Forms.Label();
            this.lbDns1 = new System.Windows.Forms.Label();
            this.chbDns = new System.Windows.Forms.CheckBox();
            this.chbAuto = new System.Windows.Forms.CheckBox();
            this.lbport = new System.Windows.Forms.Label();
            this.lbMAC = new System.Windows.Forms.Label();
            this.lbMask = new System.Windows.Forms.Label();
            this.lbRouterIP = new System.Windows.Forms.Label();
            this.lbIP = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.Dns1);
            this.groupBox1.Controls.Add(this.DNS);
            this.groupBox1.Controls.Add(this.btnModify);
            this.groupBox1.Controls.Add(this.btnRead);
            this.groupBox1.Controls.Add(this.tbPort);
            this.groupBox1.Controls.Add(this.panel1);
            this.groupBox1.Controls.Add(this.lbAuto);
            this.groupBox1.Controls.Add(this.MaskIP);
            this.groupBox1.Controls.Add(this.txtSwRoutIP);
            this.groupBox1.Controls.Add(this.txtSwiIP);
            this.groupBox1.Controls.Add(this.panel2);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(243, 312);
            this.groupBox1.TabIndex = 101;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Network Information";
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // Dns1
            // 
            this.Dns1.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Dns1.iIPType = 1;
            this.Dns1.IPAddressValue = "...";
            this.Dns1.Location = new System.Drawing.Point(95, 242);
            this.Dns1.Name = "Dns1";
            this.Dns1.Size = new System.Drawing.Size(120, 22);
            this.Dns1.TabIndex = 108;
            this.Dns1.MouseEnter += new System.EventHandler(this.txtSwiIP_TextChanged);
            // 
            // DNS
            // 
            this.DNS.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DNS.iIPType = 1;
            this.DNS.IPAddressValue = "...";
            this.DNS.Location = new System.Drawing.Point(95, 214);
            this.DNS.Name = "DNS";
            this.DNS.Size = new System.Drawing.Size(120, 22);
            this.DNS.TabIndex = 107;
            this.DNS.MouseEnter += new System.EventHandler(this.txtSwiIP_TextChanged);
            // 
            // btnModify
            // 
            this.btnModify.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnModify.ForeColor = System.Drawing.Color.Black;
            this.btnModify.Image = global::HDL_Buspro_Setup_Tool.Properties.Resources.Save;
            this.btnModify.Location = new System.Drawing.Point(157, 281);
            this.btnModify.Name = "btnModify";
            this.btnModify.Size = new System.Drawing.Size(56, 25);
            this.btnModify.TabIndex = 5;
            this.btnModify.UseVisualStyleBackColor = true;
            this.btnModify.Click += new System.EventHandler(this.btnModify_Click);
            // 
            // btnRead
            // 
            this.btnRead.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRead.ForeColor = System.Drawing.Color.Black;
            this.btnRead.Image = global::HDL_Buspro_Setup_Tool.Properties.Resources.Refresh;
            this.btnRead.Location = new System.Drawing.Point(90, 281);
            this.btnRead.Name = "btnRead";
            this.btnRead.Size = new System.Drawing.Size(64, 25);
            this.btnRead.TabIndex = 4;
            this.btnRead.UseVisualStyleBackColor = true;
            this.btnRead.Click += new System.EventHandler(this.btnRead_Click);
            // 
            // tbPort
            // 
            this.tbPort.Enabled = false;
            this.tbPort.Location = new System.Drawing.Point(93, 137);
            this.tbPort.Name = "tbPort";
            this.tbPort.Size = new System.Drawing.Size(120, 22);
            this.tbPort.TabIndex = 3;
            this.tbPort.Tag = "4";
            this.tbPort.Text = "6000";
            this.tbPort.TextChanged += new System.EventHandler(this.txtSwiIP_TextChanged);
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.txt6);
            this.panel1.Controls.Add(this.txt5);
            this.panel1.Controls.Add(this.txt4);
            this.panel1.Controls.Add(this.txt1);
            this.panel1.Location = new System.Drawing.Point(93, 106);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(122, 22);
            this.panel1.TabIndex = 106;
            // 
            // txt6
            // 
            this.txt6.Dock = System.Windows.Forms.DockStyle.Left;
            this.txt6.Location = new System.Drawing.Point(98, 0);
            this.txt6.Name = "txt6";
            this.txt6.Size = new System.Drawing.Size(24, 22);
            this.txt6.TabIndex = 2;
            this.txt6.Tag = "4";
            this.txt6.Text = "255";
            this.txt6.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txt6.TextChanged += new System.EventHandler(this.txtSwiIP_TextChanged);
            // 
            // txt5
            // 
            this.txt5.Dock = System.Windows.Forms.DockStyle.Left;
            this.txt5.Location = new System.Drawing.Point(74, 0);
            this.txt5.Name = "txt5";
            this.txt5.Size = new System.Drawing.Size(24, 22);
            this.txt5.TabIndex = 1;
            this.txt5.Tag = "4";
            this.txt5.Text = "255";
            this.txt5.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txt5.TextChanged += new System.EventHandler(this.txtSwiIP_TextChanged);
            // 
            // txt4
            // 
            this.txt4.Dock = System.Windows.Forms.DockStyle.Left;
            this.txt4.Location = new System.Drawing.Point(50, 0);
            this.txt4.Name = "txt4";
            this.txt4.Size = new System.Drawing.Size(24, 22);
            this.txt4.TabIndex = 2;
            this.txt4.Tag = "4";
            this.txt4.Text = "255";
            this.txt4.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txt4.TextChanged += new System.EventHandler(this.txtSwiIP_TextChanged);
            // 
            // txt1
            // 
            this.txt1.Dock = System.Windows.Forms.DockStyle.Left;
            this.txt1.Enabled = false;
            this.txt1.Location = new System.Drawing.Point(0, 0);
            this.txt1.Name = "txt1";
            this.txt1.Size = new System.Drawing.Size(50, 22);
            this.txt1.TabIndex = 1;
            this.txt1.Tag = "4";
            this.txt1.Text = "48:44:4C:";
            this.txt1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lbAuto
            // 
            this.lbAuto.AutoSize = true;
            this.lbAuto.Location = new System.Drawing.Point(90, 166);
            this.lbAuto.Name = "lbAuto";
            this.lbAuto.Size = new System.Drawing.Size(87, 14);
            this.lbAuto.TabIndex = 91;
            this.lbAuto.Text = "Auto get IP fail";
            this.lbAuto.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbAuto.Visible = false;
            // 
            // MaskIP
            // 
            this.MaskIP.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MaskIP.iIPType = 1;
            this.MaskIP.IPAddressValue = "...";
            this.MaskIP.Location = new System.Drawing.Point(93, 78);
            this.MaskIP.Name = "MaskIP";
            this.MaskIP.Size = new System.Drawing.Size(120, 22);
            this.MaskIP.TabIndex = 2;
            this.MaskIP.MouseEnter += new System.EventHandler(this.txtSwiIP_TextChanged);
            // 
            // txtSwRoutIP
            // 
            this.txtSwRoutIP.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSwRoutIP.iIPType = 1;
            this.txtSwRoutIP.IPAddressValue = "...";
            this.txtSwRoutIP.Location = new System.Drawing.Point(93, 50);
            this.txtSwRoutIP.Name = "txtSwRoutIP";
            this.txtSwRoutIP.Size = new System.Drawing.Size(120, 22);
            this.txtSwRoutIP.TabIndex = 1;
            this.txtSwRoutIP.MouseEnter += new System.EventHandler(this.txtSwiIP_TextChanged);
            // 
            // txtSwiIP
            // 
            this.txtSwiIP.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSwiIP.iIPType = 1;
            this.txtSwiIP.IPAddressValue = "...";
            this.txtSwiIP.Location = new System.Drawing.Point(93, 21);
            this.txtSwiIP.Name = "txtSwiIP";
            this.txtSwiIP.Size = new System.Drawing.Size(120, 22);
            this.txtSwiIP.TabIndex = 0;
            this.txtSwiIP.MouseEnter += new System.EventHandler(this.txtSwiIP_TextChanged);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.lbDns);
            this.panel2.Controls.Add(this.lbDns1);
            this.panel2.Controls.Add(this.chbDns);
            this.panel2.Controls.Add(this.chbAuto);
            this.panel2.Controls.Add(this.lbport);
            this.panel2.Controls.Add(this.lbMAC);
            this.panel2.Controls.Add(this.lbMask);
            this.panel2.Controls.Add(this.lbRouterIP);
            this.panel2.Controls.Add(this.lbIP);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel2.Location = new System.Drawing.Point(3, 18);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(85, 291);
            this.panel2.TabIndex = 102;
            // 
            // lbDns
            // 
            this.lbDns.Dock = System.Windows.Forms.DockStyle.Top;
            this.lbDns.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lbDns.Location = new System.Drawing.Point(0, 224);
            this.lbDns.Name = "lbDns";
            this.lbDns.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.lbDns.Size = new System.Drawing.Size(85, 28);
            this.lbDns.TabIndex = 81;
            this.lbDns.Text = "DNS I:";
            this.lbDns.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbDns1
            // 
            this.lbDns1.Dock = System.Windows.Forms.DockStyle.Top;
            this.lbDns1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lbDns1.Location = new System.Drawing.Point(0, 196);
            this.lbDns1.Name = "lbDns1";
            this.lbDns1.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.lbDns1.Size = new System.Drawing.Size(85, 28);
            this.lbDns1.TabIndex = 82;
            this.lbDns1.Text = "DNS II:";
            this.lbDns1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // chbDns
            // 
            this.chbDns.Dock = System.Windows.Forms.DockStyle.Top;
            this.chbDns.Location = new System.Drawing.Point(0, 168);
            this.chbDns.Name = "chbDns";
            this.chbDns.Padding = new System.Windows.Forms.Padding(12, 0, 0, 0);
            this.chbDns.Size = new System.Drawing.Size(85, 28);
            this.chbDns.TabIndex = 111;
            this.chbDns.Text = "DNS";
            this.chbDns.UseVisualStyleBackColor = true;
            this.chbDns.Visible = false;
            this.chbDns.CheckedChanged += new System.EventHandler(this.chbDns_CheckedChanged);
            // 
            // chbAuto
            // 
            this.chbAuto.Dock = System.Windows.Forms.DockStyle.Top;
            this.chbAuto.Location = new System.Drawing.Point(0, 140);
            this.chbAuto.Name = "chbAuto";
            this.chbAuto.Padding = new System.Windows.Forms.Padding(12, 0, 0, 0);
            this.chbAuto.Size = new System.Drawing.Size(85, 28);
            this.chbAuto.TabIndex = 1;
            this.chbAuto.Text = "DHCP";
            this.chbAuto.UseVisualStyleBackColor = true;
            this.chbAuto.Visible = false;
            this.chbAuto.CheckedChanged += new System.EventHandler(this.chbAuto_CheckedChanged);
            // 
            // lbport
            // 
            this.lbport.Dock = System.Windows.Forms.DockStyle.Top;
            this.lbport.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lbport.Location = new System.Drawing.Point(0, 112);
            this.lbport.Name = "lbport";
            this.lbport.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.lbport.Size = new System.Drawing.Size(85, 28);
            this.lbport.TabIndex = 80;
            this.lbport.Text = "Port:";
            this.lbport.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbMAC
            // 
            this.lbMAC.Dock = System.Windows.Forms.DockStyle.Top;
            this.lbMAC.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lbMAC.Location = new System.Drawing.Point(0, 84);
            this.lbMAC.Name = "lbMAC";
            this.lbMAC.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.lbMAC.Size = new System.Drawing.Size(85, 28);
            this.lbMAC.TabIndex = 2;
            this.lbMAC.Text = "IP MAC:";
            this.lbMAC.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbMask
            // 
            this.lbMask.Dock = System.Windows.Forms.DockStyle.Top;
            this.lbMask.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lbMask.Location = new System.Drawing.Point(0, 56);
            this.lbMask.Name = "lbMask";
            this.lbMask.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.lbMask.Size = new System.Drawing.Size(85, 28);
            this.lbMask.TabIndex = 3;
            this.lbMask.Text = "Mask IP:";
            this.lbMask.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbRouterIP
            // 
            this.lbRouterIP.Dock = System.Windows.Forms.DockStyle.Top;
            this.lbRouterIP.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lbRouterIP.Location = new System.Drawing.Point(0, 28);
            this.lbRouterIP.Name = "lbRouterIP";
            this.lbRouterIP.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.lbRouterIP.Size = new System.Drawing.Size(85, 28);
            this.lbRouterIP.TabIndex = 4;
            this.lbRouterIP.Text = "Router IP:";
            this.lbRouterIP.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbIP
            // 
            this.lbIP.Dock = System.Windows.Forms.DockStyle.Top;
            this.lbIP.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lbIP.Location = new System.Drawing.Point(0, 0);
            this.lbIP.Name = "lbIP";
            this.lbIP.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.lbIP.Size = new System.Drawing.Size(85, 28);
            this.lbIP.TabIndex = 5;
            this.lbIP.Text = "IP:";
            this.lbIP.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // NetworkInForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Calibri", 9F);
            this.Name = "NetworkInForm";
            this.Size = new System.Drawing.Size(243, 312);
            this.Load += new System.EventHandler(this.NetworkInForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnModify;
        private System.Windows.Forms.Button btnRead;
        private System.Windows.Forms.TextBox tbPort;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox txt6;
        private System.Windows.Forms.TextBox txt5;
        private System.Windows.Forms.TextBox txt4;
        private System.Windows.Forms.TextBox txt1;
        private System.Windows.Forms.Label lbAuto;
        private IPAddressNew MaskIP;
        private IPAddressNew txtSwRoutIP;
        private IPAddressNew txtSwiIP;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.CheckBox chbAuto;
        private System.Windows.Forms.Label lbport;
        private System.Windows.Forms.Label lbMAC;
        private System.Windows.Forms.Label lbMask;
        private System.Windows.Forms.Label lbRouterIP;
        private System.Windows.Forms.Label lbIP;
        private System.Windows.Forms.Label lbDns1;
        private System.Windows.Forms.Label lbDns;
        private IPAddressNew Dns1;
        private IPAddressNew DNS;
        private System.Windows.Forms.CheckBox chbDns;

    }

}
