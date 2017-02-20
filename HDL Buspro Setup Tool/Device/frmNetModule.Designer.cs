namespace HDL_Buspro_Setup_Tool
{
    partial class frmNetModule
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.lvSensors = new System.Windows.Forms.ListView();
            this.clID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clDevice = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clIP = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clRoute = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clMaSk = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clIPMAC = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.cm1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.networkPublic = new HDL_Buspro_Setup_Tool.NetworkInForm();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.cm1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(670, 443);
            this.tabControl1.TabIndex = 102;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.lvSensors);
            this.tabPage1.Location = new System.Drawing.Point(4, 23);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(662, 416);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Camera Available";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // lvSensors
            // 
            this.lvSensors.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.clID,
            this.clDevice,
            this.clIP,
            this.clRoute,
            this.clMaSk,
            this.clIPMAC});
            this.lvSensors.ContextMenuStrip = this.cm1;
            this.lvSensors.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvSensors.FullRowSelect = true;
            this.lvSensors.HideSelection = false;
            this.lvSensors.Location = new System.Drawing.Point(3, 3);
            this.lvSensors.Name = "lvSensors";
            this.lvSensors.Size = new System.Drawing.Size(656, 410);
            this.lvSensors.TabIndex = 102;
            this.lvSensors.UseCompatibleStateImageBehavior = false;
            this.lvSensors.View = System.Windows.Forms.View.Details;
            this.lvSensors.SelectedIndexChanged += new System.EventHandler(this.lvSensors_SelectedIndexChanged);
            // 
            // clID
            // 
            this.clID.Text = "ID";
            this.clID.Width = 48;
            // 
            // clDevice
            // 
            this.clDevice.Text = "Device";
            this.clDevice.Width = 200;
            // 
            // clIP
            // 
            this.clIP.Text = "IP";
            this.clIP.Width = 100;
            // 
            // clRoute
            // 
            this.clRoute.DisplayIndex = 4;
            this.clRoute.Text = "Route IP";
            this.clRoute.Width = 100;
            // 
            // clMaSk
            // 
            this.clMaSk.DisplayIndex = 5;
            this.clMaSk.Text = "Mask IP";
            this.clMaSk.Width = 100;
            // 
            // clIPMAC
            // 
            this.clIPMAC.DisplayIndex = 3;
            this.clIPMAC.Text = "IP MAC";
            this.clIPMAC.Width = 100;
            // 
            // cm1
            // 
            this.cm1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyToolStripMenuItem});
            this.cm1.Name = "cm1";
            this.cm1.Size = new System.Drawing.Size(138, 26);
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
            this.copyToolStripMenuItem.Text = "Copy";
            this.copyToolStripMenuItem.Click += new System.EventHandler(this.copyToolStripMenuItem_Click);
            // 
            // networkPublic
            // 
            this.networkPublic.Dock = System.Windows.Forms.DockStyle.Right;
            this.networkPublic.Font = new System.Drawing.Font("Calibri", 9F);
            this.networkPublic.Location = new System.Drawing.Point(670, 0);
            this.networkPublic.Name = "networkPublic";
            this.networkPublic.Size = new System.Drawing.Size(237, 443);
            this.networkPublic.TabIndex = 103;
            // 
            // frmNetModule
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(907, 443);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.networkPublic);
            this.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "frmNetModule";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Current RJ45 modules list";
            this.Shown += new System.EventHandler(this.frmTSensor_Shown);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.cm1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.ContextMenuStrip cm1;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.ListView lvSensors;
        private System.Windows.Forms.ColumnHeader clID;
        private System.Windows.Forms.ColumnHeader clDevice;
        private System.Windows.Forms.ColumnHeader clIP;
        private System.Windows.Forms.ColumnHeader clRoute;
        private System.Windows.Forms.ColumnHeader clIPMAC;
        private System.Windows.Forms.ColumnHeader clMaSk;
        private NetworkInForm networkPublic;

    }
}