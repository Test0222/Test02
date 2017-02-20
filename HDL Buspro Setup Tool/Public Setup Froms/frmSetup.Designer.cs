namespace HDL_Buspro_Setup_Tool
{
    partial class frmSetup
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSetup));
            this.tabSetups = new System.Windows.Forms.TabControl();
            this.tabSystem = new System.Windows.Forms.TabPage();
            this.label2 = new System.Windows.Forms.Label();
            this.cboEdit = new System.Windows.Forms.ComboBox();
            this.lbEdit = new System.Windows.Forms.Label();
            this.cbIPs = new System.Windows.Forms.ComboBox();
            this.lbActive = new System.Windows.Forms.Label();
            this.tbPC2 = new System.Windows.Forms.TextBox();
            this.lbPC2 = new System.Windows.Forms.Label();
            this.tbPC1 = new System.Windows.Forms.TextBox();
            this.lbPC1 = new System.Windows.Forms.Label();
            this.tabNetwork = new System.Windows.Forms.TabPage();
            this.tbPIP = new HDL_Buspro_Setup_Tool.IPAddressNew();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cboType = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.lvLists = new System.Windows.Forms.ListView();
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.img = new System.Windows.Forms.ImageList(this.components);
            this.rtbHistory = new System.Windows.Forms.RichTextBox();
            this.btnTest = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.tbPWD = new System.Windows.Forms.TextBox();
            this.lbPWD = new System.Windows.Forms.Label();
            this.tbUser = new System.Windows.Forms.TextBox();
            this.lbUser = new System.Windows.Forms.Label();
            this.tbPort = new System.Windows.Forms.TextBox();
            this.lbPort = new System.Windows.Forms.Label();
            this.lbPIP = new System.Windows.Forms.Label();
            this.tbName = new System.Windows.Forms.TextBox();
            this.lbname = new System.Windows.Forms.Label();
            this.tbGroup = new System.Windows.Forms.TextBox();
            this.lbGroup = new System.Windows.Forms.Label();
            this.CustomIP = new HDL_Buspro_Setup_Tool.IPAddressNew();
            this.tbUrl = new System.Windows.Forms.TextBox();
            this.cbIP = new System.Windows.Forms.ComboBox();
            this.btnApply = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.tabSetups.SuspendLayout();
            this.tabSystem.SuspendLayout();
            this.tabNetwork.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabSetups
            // 
            this.tabSetups.Controls.Add(this.tabSystem);
            this.tabSetups.Controls.Add(this.tabNetwork);
            this.tabSetups.Dock = System.Windows.Forms.DockStyle.Top;
            this.tabSetups.Location = new System.Drawing.Point(0, 0);
            this.tabSetups.Name = "tabSetups";
            this.tabSetups.SelectedIndex = 0;
            this.tabSetups.Size = new System.Drawing.Size(605, 541);
            this.tabSetups.TabIndex = 1;
            // 
            // tabSystem
            // 
            this.tabSystem.BackColor = System.Drawing.SystemColors.Control;
            this.tabSystem.Controls.Add(this.label2);
            this.tabSystem.Controls.Add(this.cboEdit);
            this.tabSystem.Controls.Add(this.lbEdit);
            this.tabSystem.Controls.Add(this.cbIPs);
            this.tabSystem.Controls.Add(this.lbActive);
            this.tabSystem.Controls.Add(this.tbPC2);
            this.tabSystem.Controls.Add(this.lbPC2);
            this.tabSystem.Controls.Add(this.tbPC1);
            this.tabSystem.Controls.Add(this.lbPC1);
            this.tabSystem.Location = new System.Drawing.Point(4, 23);
            this.tabSystem.Name = "tabSystem";
            this.tabSystem.Padding = new System.Windows.Forms.Padding(3);
            this.tabSystem.Size = new System.Drawing.Size(597, 514);
            this.tabSystem.TabIndex = 1;
            this.tabSystem.Text = "Settings";
            // 
            // label2
            // 
            this.label2.ForeColor = System.Drawing.Color.Red;
            this.label2.Location = new System.Drawing.Point(15, 174);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(576, 38);
            this.label2.TabIndex = 77;
            this.label2.Text = "Hint:Please select the right ip which has been connected to system or the one you" +
    " are going to use to enter the system.";
            // 
            // cboEdit
            // 
            this.cboEdit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboEdit.Enabled = false;
            this.cboEdit.Font = new System.Drawing.Font("Calibri", 9F);
            this.cboEdit.Items.AddRange(new object[] {
            "Whole Project",
            "Work Online"});
            this.cboEdit.Location = new System.Drawing.Point(138, 130);
            this.cboEdit.Name = "cboEdit";
            this.cboEdit.Size = new System.Drawing.Size(192, 22);
            this.cboEdit.TabIndex = 76;
            // 
            // lbEdit
            // 
            this.lbEdit.AutoSize = true;
            this.lbEdit.Font = new System.Drawing.Font("Calibri", 9F);
            this.lbEdit.ForeColor = System.Drawing.Color.Blue;
            this.lbEdit.Location = new System.Drawing.Point(21, 134);
            this.lbEdit.Name = "lbEdit";
            this.lbEdit.Size = new System.Drawing.Size(75, 14);
            this.lbEdit.TabIndex = 75;
            this.lbEdit.Text = "Setup Mode:";
            // 
            // cbIPs
            // 
            this.cbIPs.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbIPs.Font = new System.Drawing.Font("Calibri", 9F);
            this.cbIPs.Location = new System.Drawing.Point(138, 97);
            this.cbIPs.Name = "cbIPs";
            this.cbIPs.Size = new System.Drawing.Size(192, 22);
            this.cbIPs.TabIndex = 74;
            // 
            // lbActive
            // 
            this.lbActive.AutoSize = true;
            this.lbActive.Font = new System.Drawing.Font("Calibri", 9F);
            this.lbActive.ForeColor = System.Drawing.Color.Blue;
            this.lbActive.Location = new System.Drawing.Point(21, 101);
            this.lbActive.Name = "lbActive";
            this.lbActive.Size = new System.Drawing.Size(56, 14);
            this.lbActive.TabIndex = 73;
            this.lbActive.Text = "Select IP:";
            // 
            // tbPC2
            // 
            this.tbPC2.Font = new System.Drawing.Font("Calibri", 9F);
            this.tbPC2.Location = new System.Drawing.Point(138, 65);
            this.tbPC2.MaxLength = 3;
            this.tbPC2.Name = "tbPC2";
            this.tbPC2.Size = new System.Drawing.Size(192, 22);
            this.tbPC2.TabIndex = 72;
            this.tbPC2.Tag = "2";
            // 
            // lbPC2
            // 
            this.lbPC2.AutoSize = true;
            this.lbPC2.Font = new System.Drawing.Font("Calibri", 9F);
            this.lbPC2.ForeColor = System.Drawing.Color.Blue;
            this.lbPC2.Location = new System.Drawing.Point(21, 69);
            this.lbPC2.Name = "lbPC2";
            this.lbPC2.Size = new System.Drawing.Size(76, 14);
            this.lbPC2.TabIndex = 71;
            this.lbPC2.Text = "PC Device ID:";
            // 
            // tbPC1
            // 
            this.tbPC1.Font = new System.Drawing.Font("Calibri", 9F);
            this.tbPC1.Location = new System.Drawing.Point(138, 33);
            this.tbPC1.MaxLength = 3;
            this.tbPC1.Name = "tbPC1";
            this.tbPC1.Size = new System.Drawing.Size(192, 22);
            this.tbPC1.TabIndex = 70;
            this.tbPC1.Tag = "2";
            // 
            // lbPC1
            // 
            this.lbPC1.AutoSize = true;
            this.lbPC1.Font = new System.Drawing.Font("Calibri", 9F);
            this.lbPC1.ForeColor = System.Drawing.Color.Blue;
            this.lbPC1.Location = new System.Drawing.Point(21, 37);
            this.lbPC1.Name = "lbPC1";
            this.lbPC1.Size = new System.Drawing.Size(79, 14);
            this.lbPC1.TabIndex = 69;
            this.lbPC1.Text = "PC SubNet ID:";
            // 
            // tabNetwork
            // 
            this.tabNetwork.BackColor = System.Drawing.SystemColors.Control;
            this.tabNetwork.Controls.Add(this.tbPIP);
            this.tabNetwork.Controls.Add(this.label5);
            this.tabNetwork.Controls.Add(this.label4);
            this.tabNetwork.Controls.Add(this.cboType);
            this.tabNetwork.Controls.Add(this.label3);
            this.tabNetwork.Controls.Add(this.lvLists);
            this.tabNetwork.Controls.Add(this.rtbHistory);
            this.tabNetwork.Controls.Add(this.btnTest);
            this.tabNetwork.Controls.Add(this.btnSearch);
            this.tabNetwork.Controls.Add(this.tbPWD);
            this.tabNetwork.Controls.Add(this.lbPWD);
            this.tabNetwork.Controls.Add(this.tbUser);
            this.tabNetwork.Controls.Add(this.lbUser);
            this.tabNetwork.Controls.Add(this.tbPort);
            this.tabNetwork.Controls.Add(this.lbPort);
            this.tabNetwork.Controls.Add(this.lbPIP);
            this.tabNetwork.Controls.Add(this.tbName);
            this.tabNetwork.Controls.Add(this.lbname);
            this.tabNetwork.Controls.Add(this.tbGroup);
            this.tabNetwork.Controls.Add(this.lbGroup);
            this.tabNetwork.Controls.Add(this.CustomIP);
            this.tabNetwork.Controls.Add(this.tbUrl);
            this.tabNetwork.Controls.Add(this.cbIP);
            this.tabNetwork.Font = new System.Drawing.Font("Calibri", 9F);
            this.tabNetwork.Location = new System.Drawing.Point(4, 23);
            this.tabNetwork.Name = "tabNetwork";
            this.tabNetwork.Padding = new System.Windows.Forms.Padding(3);
            this.tabNetwork.Size = new System.Drawing.Size(597, 514);
            this.tabNetwork.TabIndex = 0;
            this.tabNetwork.Text = "Remote Access";
            // 
            // tbPIP
            // 
            this.tbPIP.iIPType = 1;
            this.tbPIP.IPAddressValue = "...";
            this.tbPIP.Location = new System.Drawing.Point(91, 300);
            this.tbPIP.Name = "tbPIP";
            this.tbPIP.Size = new System.Drawing.Size(165, 22);
            this.tbPIP.TabIndex = 87;
            // 
            // label5
            // 
            this.label5.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label5.Font = new System.Drawing.Font("Calibri", 9F);
            this.label5.ForeColor = System.Drawing.Color.Red;
            this.label5.Location = new System.Drawing.Point(3, 472);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(591, 39);
            this.label5.TabIndex = 84;
            this.label5.Text = "Hint: Local Network: could search all systems which has been in the same ip class" +
    "es.\r\n         P2P, Remote Control: have to work with your ip module. Ip module s" +
    "hould have same setup as below...";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Location = new System.Drawing.Point(297, 20);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(76, 14);
            this.label4.TabIndex = 82;
            this.label4.Text = "Server IP/Url:";
            // 
            // cboType
            // 
            this.cboType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboType.Font = new System.Drawing.Font("Calibri", 9F);
            this.cboType.Items.AddRange(new object[] {
            "Local Network",
            "P2P",
            "HDL Servers",
            "Custom Servers",
            "URL"});
            this.cboType.Location = new System.Drawing.Point(91, 17);
            this.cboType.Name = "cboType";
            this.cboType.Size = new System.Drawing.Size(192, 22);
            this.cboType.TabIndex = 81;
            this.cboType.SelectedIndexChanged += new System.EventHandler(this.cboType_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(8, 17);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(34, 14);
            this.label3.TabIndex = 80;
            this.label3.Text = "Type:";
            // 
            // lvLists
            // 
            this.lvLists.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.lvLists.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader6,
            this.columnHeader1,
            this.columnHeader5,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4});
            this.lvLists.Font = new System.Drawing.Font("Calibri", 9F);
            this.lvLists.FullRowSelect = true;
            this.lvLists.HideSelection = false;
            this.lvLists.LargeImageList = this.img;
            this.lvLists.Location = new System.Drawing.Point(91, 83);
            this.lvLists.MultiSelect = false;
            this.lvLists.Name = "lvLists";
            this.lvLists.Size = new System.Drawing.Size(489, 180);
            this.lvLists.SmallImageList = this.img;
            this.lvLists.StateImageList = this.img;
            this.lvLists.TabIndex = 79;
            this.lvLists.UseCompatibleStateImageBehavior = false;
            this.lvLists.View = System.Windows.Forms.View.Details;
            this.lvLists.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.lvLists_ItemSelectionChanged);
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "";
            this.columnHeader6.Width = 22;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "ID";
            this.columnHeader1.Width = 32;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Project Name ";
            this.columnHeader5.Width = 120;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "User";
            this.columnHeader2.Width = 90;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "IP";
            this.columnHeader3.Width = 90;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Port";
            // 
            // img
            // 
            this.img.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("img.ImageStream")));
            this.img.TransparentColor = System.Drawing.Color.Transparent;
            this.img.Images.SetKeyName(0, "黑.png");
            this.img.Images.SetKeyName(1, "绿.png");
            // 
            // rtbHistory
            // 
            this.rtbHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.rtbHistory.Font = new System.Drawing.Font("Calibri", 9F);
            this.rtbHistory.HideSelection = false;
            this.rtbHistory.Location = new System.Drawing.Point(91, 362);
            this.rtbHistory.Name = "rtbHistory";
            this.rtbHistory.Size = new System.Drawing.Size(488, 107);
            this.rtbHistory.TabIndex = 78;
            this.rtbHistory.Text = "";
            // 
            // btnTest
            // 
            this.btnTest.Font = new System.Drawing.Font("Calibri", 9F);
            this.btnTest.Location = new System.Drawing.Point(520, 273);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(60, 83);
            this.btnTest.TabIndex = 77;
            this.btnTest.Text = "Test";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnSearch.Image = global::HDL_Buspro_Setup_Tool.Properties.Resources.Search;
            this.btnSearch.Location = new System.Drawing.Point(552, 50);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(28, 25);
            this.btnSearch.TabIndex = 75;
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // tbPWD
            // 
            this.tbPWD.Font = new System.Drawing.Font("Calibri", 9F);
            this.tbPWD.Location = new System.Drawing.Point(349, 334);
            this.tbPWD.Name = "tbPWD";
            this.tbPWD.Size = new System.Drawing.Size(165, 22);
            this.tbPWD.TabIndex = 74;
            this.tbPWD.Tag = "2";
            this.tbPWD.TextChanged += new System.EventHandler(this.tbPWD_TextChanged);
            // 
            // lbPWD
            // 
            this.lbPWD.AutoSize = true;
            this.lbPWD.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbPWD.ForeColor = System.Drawing.Color.Black;
            this.lbPWD.Location = new System.Drawing.Point(270, 336);
            this.lbPWD.Name = "lbPWD";
            this.lbPWD.Size = new System.Drawing.Size(62, 14);
            this.lbPWD.TabIndex = 73;
            this.lbPWD.Text = "Password:";
            // 
            // tbUser
            // 
            this.tbUser.Enabled = false;
            this.tbUser.Font = new System.Drawing.Font("Calibri", 9F);
            this.tbUser.Location = new System.Drawing.Point(91, 334);
            this.tbUser.Name = "tbUser";
            this.tbUser.Size = new System.Drawing.Size(165, 22);
            this.tbUser.TabIndex = 72;
            this.tbUser.Tag = "2";
            // 
            // lbUser
            // 
            this.lbUser.AutoSize = true;
            this.lbUser.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbUser.ForeColor = System.Drawing.Color.Black;
            this.lbUser.Location = new System.Drawing.Point(8, 332);
            this.lbUser.Name = "lbUser";
            this.lbUser.Size = new System.Drawing.Size(70, 14);
            this.lbUser.TabIndex = 71;
            this.lbUser.Text = "User Name:";
            // 
            // tbPort
            // 
            this.tbPort.Font = new System.Drawing.Font("Calibri", 9F);
            this.tbPort.Location = new System.Drawing.Point(349, 303);
            this.tbPort.Name = "tbPort";
            this.tbPort.Size = new System.Drawing.Size(165, 22);
            this.tbPort.TabIndex = 70;
            this.tbPort.Tag = "2";
            this.tbPort.TextChanged += new System.EventHandler(this.tbPort_TextChanged);
            this.tbPort.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbPort_KeyPress);
            // 
            // lbPort
            // 
            this.lbPort.AutoSize = true;
            this.lbPort.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbPort.ForeColor = System.Drawing.Color.Black;
            this.lbPort.Location = new System.Drawing.Point(270, 304);
            this.lbPort.Name = "lbPort";
            this.lbPort.Size = new System.Drawing.Size(31, 14);
            this.lbPort.TabIndex = 69;
            this.lbPort.Text = "Port:";
            // 
            // lbPIP
            // 
            this.lbPIP.AutoSize = true;
            this.lbPIP.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbPIP.ForeColor = System.Drawing.Color.Black;
            this.lbPIP.Location = new System.Drawing.Point(8, 300);
            this.lbPIP.Name = "lbPIP";
            this.lbPIP.Size = new System.Drawing.Size(55, 14);
            this.lbPIP.TabIndex = 67;
            this.lbPIP.Text = "Client IP:";
            // 
            // tbName
            // 
            this.tbName.Enabled = false;
            this.tbName.Font = new System.Drawing.Font("Calibri", 9F);
            this.tbName.Location = new System.Drawing.Point(91, 273);
            this.tbName.Name = "tbName";
            this.tbName.Size = new System.Drawing.Size(423, 22);
            this.tbName.TabIndex = 66;
            this.tbName.Tag = "2";
            // 
            // lbname
            // 
            this.lbname.AutoSize = true;
            this.lbname.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbname.ForeColor = System.Drawing.Color.Black;
            this.lbname.Location = new System.Drawing.Point(11, 271);
            this.lbname.Name = "lbname";
            this.lbname.Size = new System.Drawing.Size(42, 14);
            this.lbname.TabIndex = 65;
            this.lbname.Text = "Name:";
            // 
            // tbGroup
            // 
            this.tbGroup.Font = new System.Drawing.Font("Calibri", 9F);
            this.tbGroup.Location = new System.Drawing.Point(91, 50);
            this.tbGroup.Name = "tbGroup";
            this.tbGroup.Size = new System.Drawing.Size(454, 22);
            this.tbGroup.TabIndex = 64;
            this.tbGroup.Tag = "2";
            this.tbGroup.Text = "LFH123";
            this.tbGroup.TextChanged += new System.EventHandler(this.tbGroup_TextChanged);
            // 
            // lbGroup
            // 
            this.lbGroup.AutoSize = true;
            this.lbGroup.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbGroup.ForeColor = System.Drawing.Color.Black;
            this.lbGroup.Location = new System.Drawing.Point(6, 50);
            this.lbGroup.Name = "lbGroup";
            this.lbGroup.Size = new System.Drawing.Size(73, 14);
            this.lbGroup.TabIndex = 63;
            this.lbGroup.Text = "Work Group:";
            // 
            // CustomIP
            // 
            this.CustomIP.iIPType = 1;
            this.CustomIP.IPAddressValue = "...";
            this.CustomIP.Location = new System.Drawing.Point(388, 17);
            this.CustomIP.Name = "CustomIP";
            this.CustomIP.Size = new System.Drawing.Size(191, 22);
            this.CustomIP.TabIndex = 86;
            // 
            // tbUrl
            // 
            this.tbUrl.Location = new System.Drawing.Point(388, 17);
            this.tbUrl.Name = "tbUrl";
            this.tbUrl.Size = new System.Drawing.Size(191, 22);
            this.tbUrl.TabIndex = 88;
            // 
            // cbIP
            // 
            this.cbIP.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbIP.FormattingEnabled = true;
            this.cbIP.Items.AddRange(new object[] {
            "115.29.251.24"});
            this.cbIP.Location = new System.Drawing.Point(388, 17);
            this.cbIP.Name = "cbIP";
            this.cbIP.Size = new System.Drawing.Size(191, 22);
            this.cbIP.TabIndex = 85;
            // 
            // btnApply
            // 
            this.btnApply.Location = new System.Drawing.Point(231, 543);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(108, 27);
            this.btnApply.TabIndex = 2;
            this.btnApply.Text = "Apply";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Visible = false;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(487, 543);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(108, 27);
            this.btnExit.TabIndex = 3;
            this.btnExit.Text = "Cancel";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(359, 543);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(108, 27);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // frmSetup
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(605, 577);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnApply);
            this.Controls.Add(this.tabSetups);
            this.Font = new System.Drawing.Font("Calibri", 9F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmSetup";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Options";
            this.Load += new System.EventHandler(this.frmSetup_Load);
            this.tabSetups.ResumeLayout(false);
            this.tabSystem.ResumeLayout(false);
            this.tabSystem.PerformLayout();
            this.tabNetwork.ResumeLayout(false);
            this.tabNetwork.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabSetups;
        private System.Windows.Forms.TabPage tabNetwork;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.TextBox tbName;
        private System.Windows.Forms.Label lbname;
        private System.Windows.Forms.TextBox tbGroup;
        private System.Windows.Forms.Label lbGroup;
        private System.Windows.Forms.TextBox tbPWD;
        private System.Windows.Forms.Label lbPWD;
        private System.Windows.Forms.TextBox tbUser;
        private System.Windows.Forms.Label lbUser;
        private System.Windows.Forms.TextBox tbPort;
        private System.Windows.Forms.Label lbPort;
        private System.Windows.Forms.Label lbPIP;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.RichTextBox rtbHistory;
        private System.Windows.Forms.ListView lvLists;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.TabPage tabSystem;
        private System.Windows.Forms.TextBox tbPC1;
        private System.Windows.Forms.Label lbPC1;
        private System.Windows.Forms.TextBox tbPC2;
        private System.Windows.Forms.Label lbPC2;
        private System.Windows.Forms.ComboBox cbIPs;
        private System.Windows.Forms.Label lbActive;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cboType;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.ComboBox cboEdit;
        private System.Windows.Forms.Label lbEdit;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cbIP;
        private System.Windows.Forms.ImageList img;
        private IPAddressNew CustomIP;
        private IPAddressNew tbPIP;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.TextBox tbUrl;
    }
}