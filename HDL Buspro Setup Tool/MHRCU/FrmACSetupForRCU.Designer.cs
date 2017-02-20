namespace HDL_Buspro_Setup_Tool
{
    partial class FrmACSetupForRCU
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmACSetupForRCU));
            this.panel6 = new System.Windows.Forms.Panel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabTemp = new System.Windows.Forms.TabPage();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.grpAC = new System.Windows.Forms.GroupBox();
            this.chbListMode = new System.Windows.Forms.CheckedListBox();
            this.lbMode = new System.Windows.Forms.Label();
            this.chbListFAN = new System.Windows.Forms.CheckedListBox();
            this.lbFAN = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cbTemp = new System.Windows.Forms.ComboBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chbPower = new System.Windows.Forms.CheckBox();
            this.chbWind = new System.Windows.Forms.CheckBox();
            this.tabRange = new System.Windows.Forms.TabPage();
            this.grp4 = new System.Windows.Forms.GroupBox();
            this.lbHValue4 = new System.Windows.Forms.Label();
            this.sbH4 = new System.Windows.Forms.HScrollBar();
            this.lbLValue4 = new System.Windows.Forms.Label();
            this.sbL4 = new System.Windows.Forms.HScrollBar();
            this.lbH4 = new System.Windows.Forms.Label();
            this.lbL4 = new System.Windows.Forms.Label();
            this.grp3 = new System.Windows.Forms.GroupBox();
            this.lbHValue3 = new System.Windows.Forms.Label();
            this.sbH3 = new System.Windows.Forms.HScrollBar();
            this.lbLValue3 = new System.Windows.Forms.Label();
            this.sbL3 = new System.Windows.Forms.HScrollBar();
            this.lbH3 = new System.Windows.Forms.Label();
            this.lbL3 = new System.Windows.Forms.Label();
            this.grp2 = new System.Windows.Forms.GroupBox();
            this.lbHValue2 = new System.Windows.Forms.Label();
            this.sbH2 = new System.Windows.Forms.HScrollBar();
            this.lbLValue2 = new System.Windows.Forms.Label();
            this.sbL2 = new System.Windows.Forms.HScrollBar();
            this.lbH2 = new System.Windows.Forms.Label();
            this.lbL2 = new System.Windows.Forms.Label();
            this.grp1 = new System.Windows.Forms.GroupBox();
            this.lbHValue1 = new System.Windows.Forms.Label();
            this.sbH1 = new System.Windows.Forms.HScrollBar();
            this.lbLValue1 = new System.Windows.Forms.Label();
            this.sbL1 = new System.Windows.Forms.HScrollBar();
            this.lbH1 = new System.Windows.Forms.Label();
            this.lbL1 = new System.Windows.Forms.Label();
            this.panel5 = new System.Windows.Forms.Panel();
            this.btnSave = new System.Windows.Forms.Button();
            this.panel6.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabTemp.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.grpAC.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tabRange.SuspendLayout();
            this.grp4.SuspendLayout();
            this.grp3.SuspendLayout();
            this.grp2.SuspendLayout();
            this.grp1.SuspendLayout();
            this.panel5.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.tabControl1);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel6.Location = new System.Drawing.Point(0, 0);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(517, 329);
            this.panel6.TabIndex = 6;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabTemp);
            this.tabControl1.Controls.Add(this.tabRange);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(517, 329);
            this.tabControl1.TabIndex = 0;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tabTemp
            // 
            this.tabTemp.BackColor = System.Drawing.SystemColors.Control;
            this.tabTemp.Controls.Add(this.panel3);
            this.tabTemp.Controls.Add(this.panel2);
            this.tabTemp.Location = new System.Drawing.Point(4, 28);
            this.tabTemp.Name = "tabTemp";
            this.tabTemp.Padding = new System.Windows.Forms.Padding(3);
            this.tabTemp.Size = new System.Drawing.Size(509, 297);
            this.tabTemp.TabIndex = 0;
            this.tabTemp.Text = "Temperature Model";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.panel4);
            this.panel3.Controls.Add(this.panel1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(3, 3);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(503, 194);
            this.panel3.TabIndex = 2;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.grpAC);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(0, 55);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(503, 139);
            this.panel4.TabIndex = 2;
            // 
            // grpAC
            // 
            this.grpAC.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("grpAC.BackgroundImage")));
            this.grpAC.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.grpAC.Controls.Add(this.chbListMode);
            this.grpAC.Controls.Add(this.lbMode);
            this.grpAC.Controls.Add(this.chbListFAN);
            this.grpAC.Controls.Add(this.lbFAN);
            this.grpAC.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpAC.Location = new System.Drawing.Point(0, 0);
            this.grpAC.Name = "grpAC";
            this.grpAC.Size = new System.Drawing.Size(503, 139);
            this.grpAC.TabIndex = 0;
            this.grpAC.TabStop = false;
            this.grpAC.Text = "Air-Condition Control Infomation";
            // 
            // chbListMode
            // 
            this.chbListMode.FormattingEnabled = true;
            this.chbListMode.Items.AddRange(new object[] {
            "Cooling",
            "Heating",
            "FAN",
            "Auto",
            "Dry"});
            this.chbListMode.Location = new System.Drawing.Point(316, 29);
            this.chbListMode.Name = "chbListMode";
            this.chbListMode.Size = new System.Drawing.Size(120, 76);
            this.chbListMode.TabIndex = 3;
            // 
            // lbMode
            // 
            this.lbMode.AutoSize = true;
            this.lbMode.Location = new System.Drawing.Point(253, 29);
            this.lbMode.Name = "lbMode";
            this.lbMode.Size = new System.Drawing.Size(50, 19);
            this.lbMode.TabIndex = 2;
            this.lbMode.Text = "Mode:";
            // 
            // chbListFAN
            // 
            this.chbListFAN.FormattingEnabled = true;
            this.chbListFAN.Items.AddRange(new object[] {
            "Auto",
            "High",
            "Medium",
            "Low"});
            this.chbListFAN.Location = new System.Drawing.Point(96, 29);
            this.chbListFAN.Name = "chbListFAN";
            this.chbListFAN.Size = new System.Drawing.Size(120, 76);
            this.chbListFAN.TabIndex = 1;
            // 
            // lbFAN
            // 
            this.lbFAN.AutoSize = true;
            this.lbFAN.Location = new System.Drawing.Point(6, 29);
            this.lbFAN.Name = "lbFAN";
            this.lbFAN.Size = new System.Drawing.Size(81, 19);
            this.lbFAN.TabIndex = 0;
            this.lbFAN.Text = "FAN Speed:";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(503, 55);
            this.panel1.TabIndex = 1;
            // 
            // groupBox1
            // 
            this.groupBox1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("groupBox1.BackgroundImage")));
            this.groupBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.cbTemp);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(503, 55);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 19);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(129, 19);
            this.label4.TabIndex = 2;
            this.label4.Text = "Temperature Type:";
            // 
            // cbTemp
            // 
            this.cbTemp.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbTemp.FormattingEnabled = true;
            this.cbTemp.Items.AddRange(new object[] {
            "C",
            "F"});
            this.cbTemp.Location = new System.Drawing.Point(137, 16);
            this.cbTemp.Name = "cbTemp";
            this.cbTemp.Size = new System.Drawing.Size(87, 27);
            this.cbTemp.TabIndex = 3;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.groupBox2);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(3, 197);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(503, 97);
            this.panel2.TabIndex = 1;
            // 
            // groupBox2
            // 
            this.groupBox2.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("groupBox2.BackgroundImage")));
            this.groupBox2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.groupBox2.Controls.Add(this.chbPower);
            this.groupBox2.Controls.Add(this.chbWind);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(503, 97);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            // 
            // chbPower
            // 
            this.chbPower.AutoSize = true;
            this.chbPower.Location = new System.Drawing.Point(9, 17);
            this.chbPower.Name = "chbPower";
            this.chbPower.Size = new System.Drawing.Size(115, 23);
            this.chbPower.TabIndex = 0;
            this.chbPower.Text = "Power-Saving";
            this.chbPower.UseVisualStyleBackColor = true;
            // 
            // chbWind
            // 
            this.chbWind.AutoSize = true;
            this.chbWind.Location = new System.Drawing.Point(9, 44);
            this.chbWind.Name = "chbWind";
            this.chbWind.Size = new System.Drawing.Size(117, 23);
            this.chbWind.TabIndex = 1;
            this.chbWind.Text = "Windsweeper";
            this.chbWind.UseVisualStyleBackColor = true;
            // 
            // tabRange
            // 
            this.tabRange.BackColor = System.Drawing.SystemColors.Control;
            this.tabRange.Controls.Add(this.grp4);
            this.tabRange.Controls.Add(this.grp3);
            this.tabRange.Controls.Add(this.grp2);
            this.tabRange.Controls.Add(this.grp1);
            this.tabRange.Location = new System.Drawing.Point(4, 28);
            this.tabRange.Name = "tabRange";
            this.tabRange.Padding = new System.Windows.Forms.Padding(3);
            this.tabRange.Size = new System.Drawing.Size(509, 297);
            this.tabRange.TabIndex = 1;
            this.tabRange.Text = "Temp Range";
            // 
            // grp4
            // 
            this.grp4.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("grp4.BackgroundImage")));
            this.grp4.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.grp4.Controls.Add(this.lbHValue4);
            this.grp4.Controls.Add(this.sbH4);
            this.grp4.Controls.Add(this.lbLValue4);
            this.grp4.Controls.Add(this.sbL4);
            this.grp4.Controls.Add(this.lbH4);
            this.grp4.Controls.Add(this.lbL4);
            this.grp4.Dock = System.Windows.Forms.DockStyle.Top;
            this.grp4.Location = new System.Drawing.Point(3, 226);
            this.grp4.Name = "grp4";
            this.grp4.Size = new System.Drawing.Size(503, 77);
            this.grp4.TabIndex = 3;
            this.grp4.TabStop = false;
            this.grp4.Text = "Dry Range";
            // 
            // lbHValue4
            // 
            this.lbHValue4.AutoSize = true;
            this.lbHValue4.Location = new System.Drawing.Point(401, 51);
            this.lbHValue4.Name = "lbHValue4";
            this.lbHValue4.Size = new System.Drawing.Size(18, 19);
            this.lbHValue4.TabIndex = 11;
            this.lbHValue4.Text = "C";
            // 
            // sbH4
            // 
            this.sbH4.LargeChange = 1;
            this.sbH4.Location = new System.Drawing.Point(103, 51);
            this.sbH4.Name = "sbH4";
            this.sbH4.Size = new System.Drawing.Size(275, 17);
            this.sbH4.TabIndex = 10;
            this.sbH4.ValueChanged += new System.EventHandler(this.sbL1_ValueChanged);
            // 
            // lbLValue4
            // 
            this.lbLValue4.AutoSize = true;
            this.lbLValue4.Location = new System.Drawing.Point(401, 26);
            this.lbLValue4.Name = "lbLValue4";
            this.lbLValue4.Size = new System.Drawing.Size(18, 19);
            this.lbLValue4.TabIndex = 9;
            this.lbLValue4.Text = "C";
            // 
            // sbL4
            // 
            this.sbL4.LargeChange = 1;
            this.sbL4.Location = new System.Drawing.Point(103, 24);
            this.sbL4.Name = "sbL4";
            this.sbL4.Size = new System.Drawing.Size(275, 17);
            this.sbL4.TabIndex = 8;
            this.sbL4.ValueChanged += new System.EventHandler(this.sbL1_ValueChanged);
            // 
            // lbH4
            // 
            this.lbH4.AutoSize = true;
            this.lbH4.Location = new System.Drawing.Point(18, 51);
            this.lbH4.Name = "lbH4";
            this.lbH4.Size = new System.Drawing.Size(80, 19);
            this.lbH4.TabIndex = 7;
            this.lbH4.Text = "High-Limit:";
            // 
            // lbL4
            // 
            this.lbL4.AutoSize = true;
            this.lbL4.Location = new System.Drawing.Point(18, 24);
            this.lbL4.Name = "lbL4";
            this.lbL4.Size = new System.Drawing.Size(76, 19);
            this.lbL4.TabIndex = 6;
            this.lbL4.Text = "Low-Limit:";
            // 
            // grp3
            // 
            this.grp3.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("grp3.BackgroundImage")));
            this.grp3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.grp3.Controls.Add(this.lbHValue3);
            this.grp3.Controls.Add(this.sbH3);
            this.grp3.Controls.Add(this.lbLValue3);
            this.grp3.Controls.Add(this.sbL3);
            this.grp3.Controls.Add(this.lbH3);
            this.grp3.Controls.Add(this.lbL3);
            this.grp3.Dock = System.Windows.Forms.DockStyle.Top;
            this.grp3.Location = new System.Drawing.Point(3, 154);
            this.grp3.Name = "grp3";
            this.grp3.Size = new System.Drawing.Size(503, 72);
            this.grp3.TabIndex = 2;
            this.grp3.TabStop = false;
            this.grp3.Text = "Auto Range";
            // 
            // lbHValue3
            // 
            this.lbHValue3.AutoSize = true;
            this.lbHValue3.Location = new System.Drawing.Point(401, 48);
            this.lbHValue3.Name = "lbHValue3";
            this.lbHValue3.Size = new System.Drawing.Size(18, 19);
            this.lbHValue3.TabIndex = 11;
            this.lbHValue3.Text = "C";
            // 
            // sbH3
            // 
            this.sbH3.LargeChange = 1;
            this.sbH3.Location = new System.Drawing.Point(103, 48);
            this.sbH3.Name = "sbH3";
            this.sbH3.Size = new System.Drawing.Size(275, 17);
            this.sbH3.TabIndex = 10;
            this.sbH3.ValueChanged += new System.EventHandler(this.sbL1_ValueChanged);
            // 
            // lbLValue3
            // 
            this.lbLValue3.AutoSize = true;
            this.lbLValue3.Location = new System.Drawing.Point(401, 22);
            this.lbLValue3.Name = "lbLValue3";
            this.lbLValue3.Size = new System.Drawing.Size(18, 19);
            this.lbLValue3.TabIndex = 9;
            this.lbLValue3.Text = "C";
            // 
            // sbL3
            // 
            this.sbL3.LargeChange = 1;
            this.sbL3.Location = new System.Drawing.Point(103, 21);
            this.sbL3.Name = "sbL3";
            this.sbL3.Size = new System.Drawing.Size(275, 17);
            this.sbL3.TabIndex = 8;
            this.sbL3.ValueChanged += new System.EventHandler(this.sbL1_ValueChanged);
            // 
            // lbH3
            // 
            this.lbH3.AutoSize = true;
            this.lbH3.Location = new System.Drawing.Point(18, 48);
            this.lbH3.Name = "lbH3";
            this.lbH3.Size = new System.Drawing.Size(80, 19);
            this.lbH3.TabIndex = 7;
            this.lbH3.Text = "High-Limit:";
            // 
            // lbL3
            // 
            this.lbL3.AutoSize = true;
            this.lbL3.Location = new System.Drawing.Point(18, 21);
            this.lbL3.Name = "lbL3";
            this.lbL3.Size = new System.Drawing.Size(76, 19);
            this.lbL3.TabIndex = 6;
            this.lbL3.Text = "Low-Limit:";
            // 
            // grp2
            // 
            this.grp2.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("grp2.BackgroundImage")));
            this.grp2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.grp2.Controls.Add(this.lbHValue2);
            this.grp2.Controls.Add(this.sbH2);
            this.grp2.Controls.Add(this.lbLValue2);
            this.grp2.Controls.Add(this.sbL2);
            this.grp2.Controls.Add(this.lbH2);
            this.grp2.Controls.Add(this.lbL2);
            this.grp2.Dock = System.Windows.Forms.DockStyle.Top;
            this.grp2.Location = new System.Drawing.Point(3, 82);
            this.grp2.Name = "grp2";
            this.grp2.Size = new System.Drawing.Size(503, 72);
            this.grp2.TabIndex = 1;
            this.grp2.TabStop = false;
            this.grp2.Text = "Heating Range";
            // 
            // lbHValue2
            // 
            this.lbHValue2.AutoSize = true;
            this.lbHValue2.Location = new System.Drawing.Point(401, 47);
            this.lbHValue2.Name = "lbHValue2";
            this.lbHValue2.Size = new System.Drawing.Size(18, 19);
            this.lbHValue2.TabIndex = 11;
            this.lbHValue2.Text = "C";
            // 
            // sbH2
            // 
            this.sbH2.LargeChange = 1;
            this.sbH2.Location = new System.Drawing.Point(103, 47);
            this.sbH2.Name = "sbH2";
            this.sbH2.Size = new System.Drawing.Size(275, 17);
            this.sbH2.TabIndex = 10;
            this.sbH2.ValueChanged += new System.EventHandler(this.sbL1_ValueChanged);
            // 
            // lbLValue2
            // 
            this.lbLValue2.AutoSize = true;
            this.lbLValue2.Location = new System.Drawing.Point(401, 20);
            this.lbLValue2.Name = "lbLValue2";
            this.lbLValue2.Size = new System.Drawing.Size(18, 19);
            this.lbLValue2.TabIndex = 9;
            this.lbLValue2.Text = "C";
            // 
            // sbL2
            // 
            this.sbL2.LargeChange = 1;
            this.sbL2.Location = new System.Drawing.Point(103, 20);
            this.sbL2.Name = "sbL2";
            this.sbL2.Size = new System.Drawing.Size(275, 17);
            this.sbL2.TabIndex = 8;
            this.sbL2.ValueChanged += new System.EventHandler(this.sbL1_ValueChanged);
            // 
            // lbH2
            // 
            this.lbH2.AutoSize = true;
            this.lbH2.Location = new System.Drawing.Point(18, 47);
            this.lbH2.Name = "lbH2";
            this.lbH2.Size = new System.Drawing.Size(80, 19);
            this.lbH2.TabIndex = 7;
            this.lbH2.Text = "High-Limit:";
            // 
            // lbL2
            // 
            this.lbL2.AutoSize = true;
            this.lbL2.Location = new System.Drawing.Point(18, 20);
            this.lbL2.Name = "lbL2";
            this.lbL2.Size = new System.Drawing.Size(76, 19);
            this.lbL2.TabIndex = 6;
            this.lbL2.Text = "Low-Limit:";
            // 
            // grp1
            // 
            this.grp1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("grp1.BackgroundImage")));
            this.grp1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.grp1.Controls.Add(this.lbHValue1);
            this.grp1.Controls.Add(this.sbH1);
            this.grp1.Controls.Add(this.lbLValue1);
            this.grp1.Controls.Add(this.sbL1);
            this.grp1.Controls.Add(this.lbH1);
            this.grp1.Controls.Add(this.lbL1);
            this.grp1.Dock = System.Windows.Forms.DockStyle.Top;
            this.grp1.Location = new System.Drawing.Point(3, 3);
            this.grp1.Name = "grp1";
            this.grp1.Size = new System.Drawing.Size(503, 79);
            this.grp1.TabIndex = 0;
            this.grp1.TabStop = false;
            this.grp1.Text = "Cooling Range";
            // 
            // lbHValue1
            // 
            this.lbHValue1.AutoSize = true;
            this.lbHValue1.Location = new System.Drawing.Point(401, 50);
            this.lbHValue1.Name = "lbHValue1";
            this.lbHValue1.Size = new System.Drawing.Size(18, 19);
            this.lbHValue1.TabIndex = 5;
            this.lbHValue1.Text = "C";
            // 
            // sbH1
            // 
            this.sbH1.LargeChange = 1;
            this.sbH1.Location = new System.Drawing.Point(103, 50);
            this.sbH1.Name = "sbH1";
            this.sbH1.Size = new System.Drawing.Size(275, 17);
            this.sbH1.TabIndex = 4;
            this.sbH1.ValueChanged += new System.EventHandler(this.sbL1_ValueChanged);
            // 
            // lbLValue1
            // 
            this.lbLValue1.AutoSize = true;
            this.lbLValue1.Location = new System.Drawing.Point(401, 23);
            this.lbLValue1.Name = "lbLValue1";
            this.lbLValue1.Size = new System.Drawing.Size(18, 19);
            this.lbLValue1.TabIndex = 3;
            this.lbLValue1.Text = "C";
            // 
            // sbL1
            // 
            this.sbL1.LargeChange = 1;
            this.sbL1.Location = new System.Drawing.Point(103, 23);
            this.sbL1.Name = "sbL1";
            this.sbL1.Size = new System.Drawing.Size(275, 17);
            this.sbL1.TabIndex = 2;
            this.sbL1.ValueChanged += new System.EventHandler(this.sbL1_ValueChanged);
            // 
            // lbH1
            // 
            this.lbH1.AutoSize = true;
            this.lbH1.Location = new System.Drawing.Point(18, 50);
            this.lbH1.Name = "lbH1";
            this.lbH1.Size = new System.Drawing.Size(80, 19);
            this.lbH1.TabIndex = 1;
            this.lbH1.Text = "High-Limit:";
            // 
            // lbL1
            // 
            this.lbL1.AutoSize = true;
            this.lbL1.Location = new System.Drawing.Point(18, 23);
            this.lbL1.Name = "lbL1";
            this.lbL1.Size = new System.Drawing.Size(76, 19);
            this.lbL1.TabIndex = 0;
            this.lbL1.Text = "Low-Limit:";
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.btnSave);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel5.Location = new System.Drawing.Point(0, 329);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(517, 40);
            this.panel5.TabIndex = 5;
            // 
            // btnSave
            // 
            this.btnSave.Image = global::HDL_Buspro_Setup_Tool.Properties.Resources.Save;
            this.btnSave.Location = new System.Drawing.Point(374, 3);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(140, 32);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "Save";
            this.btnSave.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // FrmACSetupForRCU
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(517, 369);
            this.Controls.Add(this.panel6);
            this.Controls.Add(this.panel5);
            this.Font = new System.Drawing.Font("Calibri", 12F);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmACSetupForRCU";
            this.Text = "FrmACSetup";
            this.Load += new System.EventHandler(this.FrmACSetup_Load);
            this.panel6.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabTemp.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.grpAC.ResumeLayout(false);
            this.grpAC.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.tabRange.ResumeLayout(false);
            this.grp4.ResumeLayout(false);
            this.grp4.PerformLayout();
            this.grp3.ResumeLayout(false);
            this.grp3.PerformLayout();
            this.grp2.ResumeLayout(false);
            this.grp2.PerformLayout();
            this.grp1.ResumeLayout(false);
            this.grp1.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabTemp;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.GroupBox grpAC;
        private System.Windows.Forms.CheckedListBox chbListMode;
        private System.Windows.Forms.Label lbMode;
        private System.Windows.Forms.CheckedListBox chbListFAN;
        private System.Windows.Forms.Label lbFAN;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox chbPower;
        private System.Windows.Forms.CheckBox chbWind;
        private System.Windows.Forms.TabPage tabRange;
        private System.Windows.Forms.GroupBox grp4;
        private System.Windows.Forms.Label lbHValue4;
        private System.Windows.Forms.HScrollBar sbH4;
        private System.Windows.Forms.Label lbLValue4;
        private System.Windows.Forms.HScrollBar sbL4;
        private System.Windows.Forms.Label lbH4;
        private System.Windows.Forms.Label lbL4;
        private System.Windows.Forms.GroupBox grp3;
        private System.Windows.Forms.Label lbHValue3;
        private System.Windows.Forms.HScrollBar sbH3;
        private System.Windows.Forms.Label lbLValue3;
        private System.Windows.Forms.HScrollBar sbL3;
        private System.Windows.Forms.Label lbH3;
        private System.Windows.Forms.Label lbL3;
        private System.Windows.Forms.GroupBox grp2;
        private System.Windows.Forms.Label lbHValue2;
        private System.Windows.Forms.HScrollBar sbH2;
        private System.Windows.Forms.Label lbLValue2;
        private System.Windows.Forms.HScrollBar sbL2;
        private System.Windows.Forms.Label lbH2;
        private System.Windows.Forms.Label lbL2;
        private System.Windows.Forms.GroupBox grp1;
        private System.Windows.Forms.Label lbHValue1;
        private System.Windows.Forms.HScrollBar sbH1;
        private System.Windows.Forms.Label lbLValue1;
        private System.Windows.Forms.HScrollBar sbL1;
        private System.Windows.Forms.Label lbH1;
        private System.Windows.Forms.Label lbL1;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cbTemp;
    }
}