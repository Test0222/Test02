namespace HDL_Buspro_Setup_Tool
{
    partial class ServerMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ServerMain));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label8 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.txtIp = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnSendMsg = new System.Windows.Forms.Button();
            this.btnConnect = new System.Windows.Forms.Button();
            this.txtMsgSend = new System.Windows.Forms.RichTextBox();
            this.GridView1 = new System.Windows.Forms.DataGridView();
            this.GridView2 = new System.Windows.Forms.DataGridView();
            this.imgList = new System.Windows.Forms.ImageList(this.components);
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsbEngineer = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel4 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsbTotal = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsbHint = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsbServer = new System.Windows.Forms.ToolStripStatusLabel();
            this.AutoConnect = new System.Windows.Forms.Timer(this.components);
            this.cmAuto = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.Exit = new System.Windows.Forms.ToolStripMenuItem();
            this.Hide = new System.Windows.Forms.ToolStripMenuItem();
            this.ShowIcon = new System.Windows.Forms.ToolStripMenuItem();
            this.notifyicon = new System.Windows.Forms.NotifyIcon(this.components);
            this.tabControl1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GridView2)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.cmAuto.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1048, 550);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.panel1);
            this.tabPage2.Location = new System.Drawing.Point(4, 23);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1040, 523);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Histtory";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.txtName);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Controls.Add(this.txtPort);
            this.panel1.Controls.Add(this.txtIp);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.btnSendMsg);
            this.panel1.Controls.Add(this.btnConnect);
            this.panel1.Controls.Add(this.txtMsgSend);
            this.panel1.Controls.Add(this.GridView1);
            this.panel1.Controls.Add(this.GridView2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1034, 517);
            this.panel1.TabIndex = 0;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(3, 465);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(34, 14);
            this.label8.TabIndex = 29;
            this.label8.Text = "Path:";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(923, 394);
            this.txtName.Name = "txtName";
            this.txtName.ReadOnly = true;
            this.txtName.Size = new System.Drawing.Size(100, 22);
            this.txtName.TabIndex = 16;
            this.txtName.Text = "Sally";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(881, 399);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(42, 14);
            this.label3.TabIndex = 15;
            this.label3.Text = "Name:";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(883, 14);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(140, 281);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 14;
            this.pictureBox1.TabStop = false;
            // 
            // txtPort
            // 
            this.txtPort.Enabled = false;
            this.txtPort.Location = new System.Drawing.Point(923, 356);
            this.txtPort.Name = "txtPort";
            this.txtPort.ReadOnly = true;
            this.txtPort.Size = new System.Drawing.Size(100, 22);
            this.txtPort.TabIndex = 7;
            this.txtPort.Text = "20000";
            // 
            // txtIp
            // 
            this.txtIp.Location = new System.Drawing.Point(923, 317);
            this.txtIp.Name = "txtIp";
            this.txtIp.Size = new System.Drawing.Size(100, 22);
            this.txtIp.TabIndex = 6;
            this.txtIp.Text = "192.168.10.96";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(881, 359);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(36, 14);
            this.label2.TabIndex = 5;
            this.label2.Text = "PORT:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(881, 320);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(20, 14);
            this.label1.TabIndex = 4;
            this.label1.Text = "IP:";
            // 
            // btnSendMsg
            // 
            this.btnSendMsg.Location = new System.Drawing.Point(883, 449);
            this.btnSendMsg.Name = "btnSendMsg";
            this.btnSendMsg.Size = new System.Drawing.Size(140, 48);
            this.btnSendMsg.TabIndex = 12;
            this.btnSendMsg.Text = "Send";
            this.btnSendMsg.UseVisualStyleBackColor = true;
            this.btnSendMsg.Click += new System.EventHandler(this.btnSendMsg_Click);
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(883, 449);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(132, 48);
            this.btnConnect.TabIndex = 10;
            this.btnConnect.Text = "连接服务器";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Visible = false;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // txtMsgSend
            // 
            this.txtMsgSend.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtMsgSend.Location = new System.Drawing.Point(44, 457);
            this.txtMsgSend.Name = "txtMsgSend";
            this.txtMsgSend.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.txtMsgSend.Size = new System.Drawing.Size(823, 31);
            this.txtMsgSend.TabIndex = 1;
            this.txtMsgSend.Text = "";
            // 
            // GridView1
            // 
            this.GridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.GridView1.Location = new System.Drawing.Point(8, 8);
            this.GridView1.Name = "GridView1";
            this.GridView1.ReadOnly = true;
            this.GridView1.RowTemplate.Height = 23;
            this.GridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.GridView1.Size = new System.Drawing.Size(867, 443);
            this.GridView1.TabIndex = 32;
            // 
            // GridView2
            // 
            this.GridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.GridView2.Location = new System.Drawing.Point(8, 8);
            this.GridView2.Name = "GridView2";
            this.GridView2.ReadOnly = true;
            this.GridView2.RowTemplate.Height = 23;
            this.GridView2.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.GridView2.Size = new System.Drawing.Size(867, 443);
            this.GridView2.TabIndex = 33;
            // 
            // imgList
            // 
            this.imgList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgList.ImageStream")));
            this.imgList.TransparentColor = System.Drawing.Color.Transparent;
            this.imgList.Images.SetKeyName(0, "Red1.png");
            this.imgList.Images.SetKeyName(1, "绿1 - 副本.png");
            this.imgList.Images.SetKeyName(2, "MAIL01A.ICO");
            this.imgList.Images.SetKeyName(3, "MAIL01B.ICO");
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripStatusLabel2,
            this.tsbEngineer,
            this.toolStripStatusLabel3,
            this.toolStripStatusLabel4,
            this.tsbTotal,
            this.tsbHint,
            this.tsbServer});
            this.statusStrip1.Location = new System.Drawing.Point(0, 550);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1048, 26);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(95, 21);
            this.toolStripStatusLabel1.Text = "Current Status：";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(13, 21);
            this.toolStripStatusLabel2.Text = "|";
            // 
            // tsbEngineer
            // 
            this.tsbEngineer.AutoSize = false;
            this.tsbEngineer.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbEngineer.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsbEngineer.Name = "tsbEngineer";
            this.tsbEngineer.Size = new System.Drawing.Size(140, 21);
            // 
            // toolStripStatusLabel3
            // 
            this.toolStripStatusLabel3.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripStatusLabel3.Name = "toolStripStatusLabel3";
            this.toolStripStatusLabel3.Size = new System.Drawing.Size(13, 21);
            this.toolStripStatusLabel3.Text = "|";
            // 
            // toolStripStatusLabel4
            // 
            this.toolStripStatusLabel4.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripStatusLabel4.Name = "toolStripStatusLabel4";
            this.toolStripStatusLabel4.Size = new System.Drawing.Size(76, 21);
            this.toolStripStatusLabel4.Text = "Firmwares：";
            // 
            // tsbTotal
            // 
            this.tsbTotal.AutoSize = false;
            this.tsbTotal.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsbTotal.Name = "tsbTotal";
            this.tsbTotal.Size = new System.Drawing.Size(42, 21);
            // 
            // tsbHint
            // 
            this.tsbHint.AutoSize = false;
            this.tsbHint.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsbHint.ForeColor = System.Drawing.Color.Red;
            this.tsbHint.Name = "tsbHint";
            this.tsbHint.Size = new System.Drawing.Size(454, 21);
            this.tsbHint.Spring = true;
            // 
            // tsbServer
            // 
            this.tsbServer.AutoSize = false;
            this.tsbServer.Name = "tsbServer";
            this.tsbServer.Size = new System.Drawing.Size(200, 21);
            // 
            // AutoConnect
            // 
            this.AutoConnect.Interval = 1000000;
            this.AutoConnect.Tick += new System.EventHandler(this.AutoConnect_Tick);
            // 
            // cmAuto
            // 
            this.cmAuto.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Exit,
            this.Hide,
            this.ShowIcon});
            this.cmAuto.Name = "cmAuto";
            this.cmAuto.Size = new System.Drawing.Size(108, 70);
            // 
            // Exit
            // 
            this.Exit.Name = "Exit";
            this.Exit.Size = new System.Drawing.Size(107, 22);
            this.Exit.Text = "Exit";
            this.Exit.Click += new System.EventHandler(this.Exit_Click);
            // 
            // Hide
            // 
            this.Hide.Name = "Hide";
            this.Hide.Size = new System.Drawing.Size(107, 22);
            this.Hide.Text = "Hide";
            this.Hide.Click += new System.EventHandler(this.Hide_Click);
            // 
            // ShowIcon
            // 
            this.ShowIcon.Name = "ShowIcon";
            this.ShowIcon.Size = new System.Drawing.Size(107, 22);
            this.ShowIcon.Text = "Show";
            this.ShowIcon.Click += new System.EventHandler(this.ShowIcon_Click);
            // 
            // notifyicon
            // 
            this.notifyicon.ContextMenuStrip = this.cmAuto;
            this.notifyicon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyicon.Icon")));
            this.notifyicon.Text = "notifyIcon1";
            this.notifyicon.Visible = true;
            // 
            // ServerMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(1048, 576);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.statusStrip1);
            this.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "ServerMain";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Auto Upgrade Software & Firmwares";
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            this.Load += new System.EventHandler(this.ServerMain_Load);
            this.SizeChanged += new System.EventHandler(this.ServerMain_SizeChanged);
            this.tabControl1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GridView2)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.cmAuto.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RichTextBox txtMsgSend;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.TextBox txtIp;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnSendMsg;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripStatusLabel tsbEngineer;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel3;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel4;
        private System.Windows.Forms.ToolStripStatusLabel tsbTotal;
        private System.Windows.Forms.ToolStripStatusLabel tsbHint;
        private System.Windows.Forms.ToolStripStatusLabel tsbServer;
        private System.Windows.Forms.ImageList imgList;
        private System.Windows.Forms.Timer AutoConnect;
        private System.Windows.Forms.DataGridView GridView1;
        private System.Windows.Forms.ContextMenuStrip cmAuto;
        private System.Windows.Forms.ToolStripMenuItem Exit;
        private System.Windows.Forms.ToolStripMenuItem Hide;
        private System.Windows.Forms.ToolStripMenuItem ShowIcon;
        private System.Windows.Forms.NotifyIcon notifyicon;
        private System.Windows.Forms.DataGridView GridView2;
    }
}

