namespace HDL_Buspro_Setup_Tool
{
    partial class frmTool
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
            try
            {
                if (disposing && (components != null))
                {
                    components.Dispose();
                }
                base.Dispose(disposing);
            }
            catch
            {
            }
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.cl1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.cl2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clExplain = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.gbBasic = new System.Windows.Forms.GroupBox();
            this.chbMultiline = new System.Windows.Forms.CheckBox();
            this.btnOne = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.rbSdBig = new System.Windows.Forms.RadioButton();
            this.rbSdSmall = new System.Windows.Forms.RadioButton();
            this.label4 = new System.Windows.Forms.Label();
            this.txtTime = new System.Windows.Forms.TextBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.rbSdAscii = new System.Windows.Forms.RadioButton();
            this.rbSdHex = new System.Windows.Forms.RadioButton();
            this.rbSd10 = new System.Windows.Forms.RadioButton();
            this.lbMS = new System.Windows.Forms.Label();
            this.btnRepeat = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.lbInv = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.rbSd2 = new System.Windows.Forms.RadioButton();
            this.rbSd1 = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.rtbSends = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lbCMDH = new System.Windows.Forms.Label();
            this.cboCMD = new System.Windows.Forms.ComboBox();
            this.tbCMD = new System.Windows.Forms.TextBox();
            this.checkedListBox1 = new System.Windows.Forms.CheckedListBox();
            this.lbCMD = new System.Windows.Forms.Label();
            this.numDevType = new System.Windows.Forms.NumericUpDown();
            this.lbDevType = new System.Windows.Forms.Label();
            this.NumDevID = new System.Windows.Forms.NumericUpDown();
            this.lbDevID = new System.Windows.Forms.Label();
            this.NumSubID = new System.Windows.Forms.NumericUpDown();
            this.lbSubID = new System.Windows.Forms.Label();
            this.grpDes = new System.Windows.Forms.GroupBox();
            this.numDesDev = new System.Windows.Forms.TextBox();
            this.numDesSub = new System.Windows.Forms.TextBox();
            this.lbDesV2 = new System.Windows.Forms.Label();
            this.lbDesV1 = new System.Windows.Forms.Label();
            this.lbDesDev = new System.Windows.Forms.Label();
            this.lbDesSub = new System.Windows.Forms.Label();
            this.gbSend = new System.Windows.Forms.GroupBox();
            this.rbPCC = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.gbRev = new System.Windows.Forms.GroupBox();
            this.rtbRev = new System.Windows.Forms.RichTextBox();
            this.panel8 = new System.Windows.Forms.Panel();
            this.tbExplain = new System.Windows.Forms.TextBox();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.panel21 = new System.Windows.Forms.Panel();
            this.rb2 = new System.Windows.Forms.RadioButton();
            this.rb4 = new System.Windows.Forms.RadioButton();
            this.rb3 = new System.Windows.Forms.RadioButton();
            this.rb1 = new System.Windows.Forms.RadioButton();
            this.rb5 = new System.Windows.Forms.RadioButton();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.gbBasic.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numDevType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumDevID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumSubID)).BeginInit();
            this.grpDes.SuspendLayout();
            this.gbSend.SuspendLayout();
            this.gbRev.SuspendLayout();
            this.panel8.SuspendLayout();
            this.panel21.SuspendLayout();
            this.SuspendLayout();
            // 
            // cl1
            // 
            this.cl1.Text = "Time";
            this.cl1.Width = 120;
            // 
            // cl2
            // 
            this.cl2.Text = "Data";
            this.cl2.Width = 500;
            // 
            // clExplain
            // 
            this.clExplain.Text = "Description";
            this.clExplain.Width = 350;
            // 
            // gbBasic
            // 
            this.gbBasic.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.gbBasic.Controls.Add(this.chbMultiline);
            this.gbBasic.Controls.Add(this.btnOne);
            this.gbBasic.Controls.Add(this.panel2);
            this.gbBasic.Controls.Add(this.label4);
            this.gbBasic.Controls.Add(this.txtTime);
            this.gbBasic.Controls.Add(this.panel3);
            this.gbBasic.Controls.Add(this.lbMS);
            this.gbBasic.Controls.Add(this.btnRepeat);
            this.gbBasic.Controls.Add(this.label3);
            this.gbBasic.Controls.Add(this.lbInv);
            this.gbBasic.Controls.Add(this.panel4);
            this.gbBasic.Controls.Add(this.label2);
            this.gbBasic.Controls.Add(this.rtbSends);
            this.gbBasic.Controls.Add(this.label1);
            this.gbBasic.Controls.Add(this.lbCMDH);
            this.gbBasic.Controls.Add(this.cboCMD);
            this.gbBasic.Controls.Add(this.tbCMD);
            this.gbBasic.Controls.Add(this.checkedListBox1);
            this.gbBasic.Controls.Add(this.lbCMD);
            this.gbBasic.Font = new System.Drawing.Font("Calibri", 9F);
            this.gbBasic.Location = new System.Drawing.Point(267, 12);
            this.gbBasic.Name = "gbBasic";
            this.gbBasic.Size = new System.Drawing.Size(826, 195);
            this.gbBasic.TabIndex = 1;
            this.gbBasic.TabStop = false;
            this.gbBasic.Text = "Basic Settings";
            // 
            // chbMultiline
            // 
            this.chbMultiline.AutoSize = true;
            this.chbMultiline.Location = new System.Drawing.Point(538, 169);
            this.chbMultiline.Name = "chbMultiline";
            this.chbMultiline.Size = new System.Drawing.Size(86, 22);
            this.chbMultiline.TabIndex = 133;
            this.chbMultiline.Text = "Multiline";
            this.chbMultiline.UseVisualStyleBackColor = true;
            this.chbMultiline.CheckedChanged += new System.EventHandler(this.chbMultiline_CheckedChanged);
            // 
            // btnOne
            // 
            this.btnOne.Location = new System.Drawing.Point(772, 126);
            this.btnOne.Name = "btnOne";
            this.btnOne.Size = new System.Drawing.Size(48, 28);
            this.btnOne.TabIndex = 3;
            this.btnOne.Text = "Send";
            this.btnOne.UseVisualStyleBackColor = true;
            this.btnOne.Click += new System.EventHandler(this.btnOne_Click);
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.rbSdBig);
            this.panel2.Controls.Add(this.rbSdSmall);
            this.panel2.Location = new System.Drawing.Point(606, 126);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(160, 28);
            this.panel2.TabIndex = 128;
            // 
            // rbSdBig
            // 
            this.rbSdBig.AutoSize = true;
            this.rbSdBig.Location = new System.Drawing.Point(80, 4);
            this.rbSdBig.Name = "rbSdBig";
            this.rbSdBig.Size = new System.Drawing.Size(48, 22);
            this.rbSdBig.TabIndex = 1;
            this.rbSdBig.Text = "Big";
            this.rbSdBig.UseVisualStyleBackColor = true;
            // 
            // rbSdSmall
            // 
            this.rbSdSmall.AutoSize = true;
            this.rbSdSmall.Checked = true;
            this.rbSdSmall.Location = new System.Drawing.Point(3, 4);
            this.rbSdSmall.Name = "rbSdSmall";
            this.rbSdSmall.Size = new System.Drawing.Size(63, 22);
            this.rbSdSmall.TabIndex = 0;
            this.rbSdSmall.TabStop = true;
            this.rbSdSmall.Text = "Small";
            this.rbSdSmall.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label4.Location = new System.Drawing.Point(535, 135);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(72, 18);
            this.label4.TabIndex = 131;
            this.label4.Text = "Pack Type:";
            // 
            // txtTime
            // 
            this.txtTime.Location = new System.Drawing.Point(118, 167);
            this.txtTime.Name = "txtTime";
            this.txtTime.Size = new System.Drawing.Size(157, 26);
            this.txtTime.TabIndex = 4;
            this.txtTime.Text = "10";
            this.txtTime.TextChanged += new System.EventHandler(this.txtTime_TextChanged);
            this.txtTime.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtTime_KeyPress);
            this.txtTime.Leave += new System.EventHandler(this.txtTime_Leave);
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.rbSdAscii);
            this.panel3.Controls.Add(this.rbSdHex);
            this.panel3.Controls.Add(this.rbSd10);
            this.panel3.Location = new System.Drawing.Point(353, 126);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(160, 28);
            this.panel3.TabIndex = 122;
            // 
            // rbSdAscii
            // 
            this.rbSdAscii.AutoSize = true;
            this.rbSdAscii.Location = new System.Drawing.Point(102, 4);
            this.rbSdAscii.Name = "rbSdAscii";
            this.rbSdAscii.Size = new System.Drawing.Size(61, 22);
            this.rbSdAscii.TabIndex = 2;
            this.rbSdAscii.Text = "ASCII";
            this.rbSdAscii.UseVisualStyleBackColor = true;
            // 
            // rbSdHex
            // 
            this.rbSdHex.AutoSize = true;
            this.rbSdHex.Location = new System.Drawing.Point(46, 4);
            this.rbSdHex.Name = "rbSdHex";
            this.rbSdHex.Size = new System.Drawing.Size(53, 22);
            this.rbSdHex.TabIndex = 1;
            this.rbSdHex.Text = "Hex";
            this.rbSdHex.UseVisualStyleBackColor = true;
            // 
            // rbSd10
            // 
            this.rbSd10.AutoSize = true;
            this.rbSd10.Checked = true;
            this.rbSd10.Location = new System.Drawing.Point(3, 4);
            this.rbSd10.Name = "rbSd10";
            this.rbSd10.Size = new System.Drawing.Size(43, 22);
            this.rbSd10.TabIndex = 0;
            this.rbSd10.TabStop = true;
            this.rbSd10.Text = "10";
            this.rbSd10.UseVisualStyleBackColor = true;
            // 
            // lbMS
            // 
            this.lbMS.AutoSize = true;
            this.lbMS.Location = new System.Drawing.Point(281, 171);
            this.lbMS.Name = "lbMS";
            this.lbMS.Size = new System.Drawing.Size(37, 18);
            this.lbMS.TabIndex = 129;
            this.lbMS.Text = "(MS)";
            // 
            // btnRepeat
            // 
            this.btnRepeat.Location = new System.Drawing.Point(353, 166);
            this.btnRepeat.Name = "btnRepeat";
            this.btnRepeat.Size = new System.Drawing.Size(160, 24);
            this.btnRepeat.TabIndex = 5;
            this.btnRepeat.Text = "Start";
            this.btnRepeat.UseVisualStyleBackColor = true;
            this.btnRepeat.Click += new System.EventHandler(this.btnRepeat_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label3.Location = new System.Drawing.Point(284, 134);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(72, 18);
            this.label3.TabIndex = 130;
            this.label3.Text = "Data Type:";
            // 
            // lbInv
            // 
            this.lbInv.AutoSize = true;
            this.lbInv.Location = new System.Drawing.Point(16, 171);
            this.lbInv.Name = "lbInv";
            this.lbInv.Size = new System.Drawing.Size(60, 18);
            this.lbInv.TabIndex = 124;
            this.lbInv.Text = "Interval:";
            // 
            // panel4
            // 
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel4.Controls.Add(this.rbSd2);
            this.panel4.Controls.Add(this.rbSd1);
            this.panel4.Location = new System.Drawing.Point(118, 126);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(160, 28);
            this.panel4.TabIndex = 129;
            // 
            // rbSd2
            // 
            this.rbSd2.AutoSize = true;
            this.rbSd2.Location = new System.Drawing.Point(80, 5);
            this.rbSd2.Name = "rbSd2";
            this.rbSd2.Size = new System.Drawing.Size(44, 22);
            this.rbSd2.TabIndex = 0;
            this.rbSd2.Text = "\" \"";
            this.rbSd2.UseVisualStyleBackColor = true;
            // 
            // rbSd1
            // 
            this.rbSd1.AutoSize = true;
            this.rbSd1.Checked = true;
            this.rbSd1.Location = new System.Drawing.Point(7, 5);
            this.rbSd1.Name = "rbSd1";
            this.rbSd1.Size = new System.Drawing.Size(45, 22);
            this.rbSd1.TabIndex = 1;
            this.rbSd1.TabStop = true;
            this.rbSd1.Text = "\",\"";
            this.rbSd1.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label2.Location = new System.Drawing.Point(16, 137);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(66, 18);
            this.label2.TabIndex = 126;
            this.label2.Text = "Delimite:";
            // 
            // rtbSends
            // 
            this.rtbSends.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbSends.Location = new System.Drawing.Point(118, 53);
            this.rtbSends.Multiline = false;
            this.rtbSends.Name = "rtbSends";
            this.rtbSends.Size = new System.Drawing.Size(702, 67);
            this.rtbSends.TabIndex = 2;
            this.rtbSends.Text = "";
            this.toolTip1.SetToolTip(this.rtbSends, "Press S to send");
            this.rtbSends.TextChanged += new System.EventHandler(this.rtbSends_TextChanged);
            this.rtbSends.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.rtbSends_KeyPress);
            this.rtbSends.MouseDown += new System.Windows.Forms.MouseEventHandler(this.rtbSends_MouseDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label1.Location = new System.Drawing.Point(16, 56);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 18);
            this.label1.TabIndex = 125;
            this.label1.Text = "Append Data:";
            // 
            // lbCMDH
            // 
            this.lbCMDH.ForeColor = System.Drawing.Color.Red;
            this.lbCMDH.Location = new System.Drawing.Point(450, 21);
            this.lbCMDH.Name = "lbCMDH";
            this.lbCMDH.Size = new System.Drawing.Size(321, 25);
            this.lbCMDH.TabIndex = 124;
            // 
            // cboCMD
            // 
            this.cboCMD.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboCMD.FormattingEnabled = true;
            this.cboCMD.Items.AddRange(new object[] {
            "Custom",
            "Scene Control",
            "Sequence Control",
            "Single Channel Light control",
            "UV Switch ",
            "Curtain Switch ",
            "SMS Control"});
            this.cboCMD.Location = new System.Drawing.Point(118, 22);
            this.cboCMD.Name = "cboCMD";
            this.cboCMD.Size = new System.Drawing.Size(160, 26);
            this.cboCMD.TabIndex = 0;
            this.cboCMD.SelectedIndexChanged += new System.EventHandler(this.cboCMD_SelectedIndexChanged);
            // 
            // tbCMD
            // 
            this.tbCMD.Font = new System.Drawing.Font("Calibri", 9F);
            this.tbCMD.Location = new System.Drawing.Point(284, 23);
            this.tbCMD.Name = "tbCMD";
            this.tbCMD.Size = new System.Drawing.Size(160, 26);
            this.tbCMD.TabIndex = 1;
            this.tbCMD.Text = "E";
            this.tbCMD.TextChanged += new System.EventHandler(this.tbCMD_TextChanged);
            // 
            // checkedListBox1
            // 
            this.checkedListBox1.BackColor = System.Drawing.SystemColors.Control;
            this.checkedListBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.checkedListBox1.FormattingEnabled = true;
            this.checkedListBox1.Items.AddRange(new object[] {
            "定时广播PC时间",
            "接收所有网段数据"});
            this.checkedListBox1.Location = new System.Drawing.Point(190, 201);
            this.checkedListBox1.MultiColumn = true;
            this.checkedListBox1.Name = "checkedListBox1";
            this.checkedListBox1.Size = new System.Drawing.Size(355, 21);
            this.checkedListBox1.TabIndex = 116;
            // 
            // lbCMD
            // 
            this.lbCMD.AutoSize = true;
            this.lbCMD.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lbCMD.Location = new System.Drawing.Point(16, 26);
            this.lbCMD.Name = "lbCMD";
            this.lbCMD.Size = new System.Drawing.Size(109, 18);
            this.lbCMD.TabIndex = 103;
            this.lbCMD.Text = "Command(Hex):";
            // 
            // numDevType
            // 
            this.numDevType.Enabled = false;
            this.numDevType.Location = new System.Drawing.Point(606, 25);
            this.numDevType.Maximum = new decimal(new int[] {
            254,
            0,
            0,
            0});
            this.numDevType.Name = "numDevType";
            this.numDevType.Size = new System.Drawing.Size(160, 26);
            this.numDevType.TabIndex = 2;
            // 
            // lbDevType
            // 
            this.lbDevType.AutoSize = true;
            this.lbDevType.Enabled = false;
            this.lbDevType.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lbDevType.Location = new System.Drawing.Point(535, 29);
            this.lbDevType.Name = "lbDevType";
            this.lbDevType.Size = new System.Drawing.Size(83, 18);
            this.lbDevType.TabIndex = 94;
            this.lbDevType.Text = "DeviceType:";
            // 
            // NumDevID
            // 
            this.NumDevID.Enabled = false;
            this.NumDevID.Location = new System.Drawing.Point(353, 27);
            this.NumDevID.Maximum = new decimal(new int[] {
            254,
            0,
            0,
            0});
            this.NumDevID.Name = "NumDevID";
            this.NumDevID.Size = new System.Drawing.Size(160, 26);
            this.NumDevID.TabIndex = 1;
            // 
            // lbDevID
            // 
            this.lbDevID.AutoSize = true;
            this.lbDevID.Enabled = false;
            this.lbDevID.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lbDevID.Location = new System.Drawing.Point(284, 35);
            this.lbDevID.Name = "lbDevID";
            this.lbDevID.Size = new System.Drawing.Size(70, 18);
            this.lbDevID.TabIndex = 89;
            this.lbDevID.Text = "Device ID:";
            // 
            // NumSubID
            // 
            this.NumSubID.Enabled = false;
            this.NumSubID.Location = new System.Drawing.Point(118, 31);
            this.NumSubID.Maximum = new decimal(new int[] {
            254,
            0,
            0,
            0});
            this.NumSubID.Name = "NumSubID";
            this.NumSubID.Size = new System.Drawing.Size(157, 26);
            this.NumSubID.TabIndex = 0;
            // 
            // lbSubID
            // 
            this.lbSubID.AutoSize = true;
            this.lbSubID.Enabled = false;
            this.lbSubID.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lbSubID.Location = new System.Drawing.Point(16, 33);
            this.lbSubID.Name = "lbSubID";
            this.lbSubID.Size = new System.Drawing.Size(72, 18);
            this.lbSubID.TabIndex = 87;
            this.lbSubID.Text = "Subnet ID:";
            // 
            // grpDes
            // 
            this.grpDes.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.grpDes.Controls.Add(this.numDesDev);
            this.grpDes.Controls.Add(this.numDesSub);
            this.grpDes.Controls.Add(this.lbDesV2);
            this.grpDes.Controls.Add(this.lbDesV1);
            this.grpDes.Controls.Add(this.lbDesDev);
            this.grpDes.Controls.Add(this.lbDesSub);
            this.grpDes.Font = new System.Drawing.Font("Calibri", 9F);
            this.grpDes.Location = new System.Drawing.Point(12, 12);
            this.grpDes.Name = "grpDes";
            this.grpDes.Size = new System.Drawing.Size(239, 89);
            this.grpDes.TabIndex = 0;
            this.grpDes.TabStop = false;
            this.grpDes.Text = "Device";
            // 
            // numDesDev
            // 
            this.numDesDev.Location = new System.Drawing.Point(92, 53);
            this.numDesDev.Name = "numDesDev";
            this.numDesDev.Size = new System.Drawing.Size(91, 26);
            this.numDesDev.TabIndex = 1;
            // 
            // numDesSub
            // 
            this.numDesSub.Location = new System.Drawing.Point(92, 21);
            this.numDesSub.Name = "numDesSub";
            this.numDesSub.Size = new System.Drawing.Size(91, 26);
            this.numDesSub.TabIndex = 0;
            // 
            // lbDesV2
            // 
            this.lbDesV2.AutoSize = true;
            this.lbDesV2.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lbDesV2.Location = new System.Drawing.Point(190, 56);
            this.lbDesV2.Name = "lbDesV2";
            this.lbDesV2.Size = new System.Drawing.Size(46, 18);
            this.lbDesV2.TabIndex = 107;
            this.lbDesV2.Text = "(0x00)";
            // 
            // lbDesV1
            // 
            this.lbDesV1.AutoSize = true;
            this.lbDesV1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lbDesV1.Location = new System.Drawing.Point(190, 26);
            this.lbDesV1.Name = "lbDesV1";
            this.lbDesV1.Size = new System.Drawing.Size(46, 18);
            this.lbDesV1.TabIndex = 106;
            this.lbDesV1.Text = "(0x00)";
            // 
            // lbDesDev
            // 
            this.lbDesDev.AutoSize = true;
            this.lbDesDev.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lbDesDev.Location = new System.Drawing.Point(13, 56);
            this.lbDesDev.Name = "lbDesDev";
            this.lbDesDev.Size = new System.Drawing.Size(70, 18);
            this.lbDesDev.TabIndex = 104;
            this.lbDesDev.Text = "Device ID:";
            // 
            // lbDesSub
            // 
            this.lbDesSub.AutoSize = true;
            this.lbDesSub.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lbDesSub.Location = new System.Drawing.Point(13, 26);
            this.lbDesSub.Name = "lbDesSub";
            this.lbDesSub.Size = new System.Drawing.Size(72, 18);
            this.lbDesSub.TabIndex = 102;
            this.lbDesSub.Text = "Subnet ID:";
            // 
            // gbSend
            // 
            this.gbSend.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.gbSend.Controls.Add(this.rbPCC);
            this.gbSend.Controls.Add(this.button1);
            this.gbSend.Controls.Add(this.lbSubID);
            this.gbSend.Controls.Add(this.numDevType);
            this.gbSend.Controls.Add(this.NumSubID);
            this.gbSend.Controls.Add(this.lbDevType);
            this.gbSend.Controls.Add(this.lbDevID);
            this.gbSend.Controls.Add(this.NumDevID);
            this.gbSend.Font = new System.Drawing.Font("Calibri", 9F);
            this.gbSend.Location = new System.Drawing.Point(267, 210);
            this.gbSend.Name = "gbSend";
            this.gbSend.Size = new System.Drawing.Size(826, 58);
            this.gbSend.TabIndex = 122;
            this.gbSend.TabStop = false;
            // 
            // rbPCC
            // 
            this.rbPCC.AutoSize = true;
            this.rbPCC.Location = new System.Drawing.Point(19, 2);
            this.rbPCC.Name = "rbPCC";
            this.rbPCC.Size = new System.Drawing.Size(149, 22);
            this.rbPCC.TabIndex = 134;
            this.rbPCC.Text = "Custom PC Address";
            this.rbPCC.UseVisualStyleBackColor = true;
            this.rbPCC.CheckedChanged += new System.EventHandler(this.rbPCD_CheckedChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(772, 22);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(48, 24);
            this.button1.TabIndex = 133;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // gbRev
            // 
            this.gbRev.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.gbRev.Controls.Add(this.rtbRev);
            this.gbRev.Controls.Add(this.panel8);
            this.gbRev.Font = new System.Drawing.Font("Calibri", 9F);
            this.gbRev.Location = new System.Drawing.Point(12, 274);
            this.gbRev.Name = "gbRev";
            this.gbRev.Size = new System.Drawing.Size(1081, 324);
            this.gbRev.TabIndex = 2;
            this.gbRev.TabStop = false;
            this.gbRev.Text = "Receive Log";
            // 
            // rtbRev
            // 
            this.rtbRev.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbRev.Font = new System.Drawing.Font("Calibri", 10F);
            this.rtbRev.HideSelection = false;
            this.rtbRev.Location = new System.Drawing.Point(3, 47);
            this.rtbRev.Name = "rtbRev";
            this.rtbRev.Size = new System.Drawing.Size(1075, 274);
            this.rtbRev.TabIndex = 130;
            this.rtbRev.Text = "";
            // 
            // panel8
            // 
            this.panel8.Controls.Add(this.tbExplain);
            this.panel8.Controls.Add(this.btnExport);
            this.panel8.Controls.Add(this.btnClear);
            this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel8.Location = new System.Drawing.Point(3, 22);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(1075, 25);
            this.panel8.TabIndex = 130;
            // 
            // tbExplain
            // 
            this.tbExplain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbExplain.Font = new System.Drawing.Font("Calibri", 9F);
            this.tbExplain.ForeColor = System.Drawing.Color.Red;
            this.tbExplain.Location = new System.Drawing.Point(0, 0);
            this.tbExplain.Name = "tbExplain";
            this.tbExplain.Size = new System.Drawing.Size(901, 26);
            this.tbExplain.TabIndex = 130;
            // 
            // btnExport
            // 
            this.btnExport.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnExport.Location = new System.Drawing.Point(901, 0);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(87, 25);
            this.btnExport.TabIndex = 129;
            this.btnExport.Text = "Export List";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btnClear
            // 
            this.btnClear.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnClear.Location = new System.Drawing.Point(988, 0);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(87, 25);
            this.btnClear.TabIndex = 128;
            this.btnClear.Text = "Clear ";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // panel21
            // 
            this.panel21.Controls.Add(this.rb2);
            this.panel21.Controls.Add(this.rb4);
            this.panel21.Controls.Add(this.rb3);
            this.panel21.Controls.Add(this.rb1);
            this.panel21.Controls.Add(this.rb5);
            this.panel21.Font = new System.Drawing.Font("Calibri", 9F);
            this.panel21.ForeColor = System.Drawing.Color.Blue;
            this.panel21.Location = new System.Drawing.Point(12, 108);
            this.panel21.Name = "panel21";
            this.panel21.Size = new System.Drawing.Size(239, 160);
            this.panel21.TabIndex = 129;
            // 
            // rb2
            // 
            this.rb2.AutoSize = true;
            this.rb2.Location = new System.Drawing.Point(13, 73);
            this.rb2.Name = "rb2";
            this.rb2.Size = new System.Drawing.Size(141, 22);
            this.rb2.TabIndex = 2;
            this.rb2.Tag = "2";
            this.rb2.Text = "The Object Device";
            this.rb2.UseVisualStyleBackColor = true;
            this.rb2.CheckedChanged += new System.EventHandler(this.rb1_CheckedChanged);
            // 
            // rb4
            // 
            this.rb4.AutoSize = true;
            this.rb4.Location = new System.Drawing.Point(13, 105);
            this.rb4.Name = "rb4";
            this.rb4.Size = new System.Drawing.Size(147, 22);
            this.rb4.TabIndex = 4;
            this.rb4.Tag = "4";
            this.rb4.Text = "Device + Command";
            this.rb4.UseVisualStyleBackColor = true;
            this.rb4.CheckedChanged += new System.EventHandler(this.rb1_CheckedChanged);
            // 
            // rb3
            // 
            this.rb3.AutoSize = true;
            this.rb3.Location = new System.Drawing.Point(13, 40);
            this.rb3.Name = "rb3";
            this.rb3.Size = new System.Drawing.Size(162, 22);
            this.rb3.TabIndex = 1;
            this.rb3.Tag = "3";
            this.rb3.Text = "The Object Command";
            this.rb3.UseVisualStyleBackColor = true;
            this.rb3.CheckedChanged += new System.EventHandler(this.rb1_CheckedChanged);
            // 
            // rb1
            // 
            this.rb1.AutoSize = true;
            this.rb1.Checked = true;
            this.rb1.Location = new System.Drawing.Point(13, 12);
            this.rb1.Name = "rb1";
            this.rb1.Size = new System.Drawing.Size(148, 22);
            this.rb1.TabIndex = 0;
            this.rb1.TabStop = true;
            this.rb1.Tag = "1";
            this.rb1.Text = "No Addresses Limit";
            this.rb1.UseVisualStyleBackColor = true;
            this.rb1.CheckedChanged += new System.EventHandler(this.rb1_CheckedChanged);
            // 
            // rb5
            // 
            this.rb5.AutoSize = true;
            this.rb5.Location = new System.Drawing.Point(13, 135);
            this.rb5.Name = "rb5";
            this.rb5.Size = new System.Drawing.Size(57, 22);
            this.rb5.TabIndex = 3;
            this.rb5.Tag = "5";
            this.rb5.Text = "Stop";
            this.rb5.UseVisualStyleBackColor = true;
            this.rb5.CheckedChanged += new System.EventHandler(this.rb5_CheckedChanged);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.btnOne_Click);
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.Filter = "TXT File|*.txt";
            this.saveFileDialog1.RestoreDirectory = true;
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            // 
            // frmTool
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1102, 610);
            this.Controls.Add(this.panel21);
            this.Controls.Add(this.gbSend);
            this.Controls.Add(this.gbRev);
            this.Controls.Add(this.gbBasic);
            this.Controls.Add(this.grpDes);
            this.Font = new System.Drawing.Font("Calibri", 12F);
            this.Name = "frmTool";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Command Test";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmTool_FormClosing);
            this.Load += new System.EventHandler(this.frmTool_Load);
            this.gbBasic.ResumeLayout(false);
            this.gbBasic.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numDevType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumDevID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumSubID)).EndInit();
            this.grpDes.ResumeLayout(false);
            this.grpDes.PerformLayout();
            this.gbSend.ResumeLayout(false);
            this.gbSend.PerformLayout();
            this.gbRev.ResumeLayout(false);
            this.panel8.ResumeLayout(false);
            this.panel8.PerformLayout();
            this.panel21.ResumeLayout(false);
            this.panel21.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbBasic;
        private System.Windows.Forms.NumericUpDown NumDevID;
        private System.Windows.Forms.Label lbDevID;
        private System.Windows.Forms.NumericUpDown NumSubID;
        private System.Windows.Forms.Label lbSubID;
        private System.Windows.Forms.NumericUpDown numDevType;
        private System.Windows.Forms.Label lbDevType;
        private System.Windows.Forms.Label lbCMD;
        private System.Windows.Forms.GroupBox gbRev;
        private System.Windows.Forms.CheckedListBox checkedListBox1;
        private System.Windows.Forms.GroupBox grpDes;
        private System.Windows.Forms.Label lbDesV2;
        private System.Windows.Forms.Label lbDesV1;
        private System.Windows.Forms.Label lbDesDev;
        private System.Windows.Forms.Label lbDesSub;
        private System.Windows.Forms.ComboBox cboCMD;
        private System.Windows.Forms.TextBox tbCMD;
        private System.Windows.Forms.GroupBox gbSend;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.RadioButton rbSd2;
        private System.Windows.Forms.RadioButton rbSd1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.RadioButton rb1;
        private System.Windows.Forms.RadioButton rb2;
        private System.Windows.Forms.RadioButton rb3;
        private System.Windows.Forms.RadioButton rb4;
        private System.Windows.Forms.RadioButton rb5;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.RadioButton rbSdBig;
        private System.Windows.Forms.RadioButton rbSdSmall;
        private System.Windows.Forms.RadioButton rbSdAscii;
        private System.Windows.Forms.RadioButton rbSdHex;
        private System.Windows.Forms.RadioButton rbSd10;
        private System.Windows.Forms.Label lbCMDH;
        private System.Windows.Forms.RichTextBox rtbRev;
        private System.Windows.Forms.ColumnHeader cl1;
        private System.Windows.Forms.ColumnHeader cl2;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.RichTextBox rtbSends;
        private System.Windows.Forms.Button btnRepeat;
        private System.Windows.Forms.Label lbInv;
        private System.Windows.Forms.Panel panel21;
        private System.Windows.Forms.ColumnHeader clExplain;
        private System.Windows.Forms.TextBox tbExplain;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label lbMS;
        private System.Windows.Forms.TextBox txtTime;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnOne;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckBox rbPCC;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.CheckBox chbMultiline;
        private System.Windows.Forms.TextBox numDesSub;
        private System.Windows.Forms.TextBox numDesDev;
    }
}