namespace HDL_Buspro_Setup_Tool
{
    partial class FrmEnviroUI
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmEnviroUI));
            this.img = new System.Windows.Forms.ImageList(this.components);
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnSave = new System.Windows.Forms.Button();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsb1 = new System.Windows.Forms.ToolStripButton();
            this.tsb2 = new System.Windows.Forms.ToolStripButton();
            this.tsb3 = new System.Windows.Forms.ToolStripButton();
            this.tsb4 = new System.Windows.Forms.ToolStripButton();
            this.tsb5 = new System.Windows.Forms.ToolStripButton();
            this.tsb6 = new System.Windows.Forms.ToolStripButton();
            this.EnviroPage = new System.Windows.Forms.TableLayoutPanel();
            this.panel14 = new System.Windows.Forms.Panel();
            this.panel13 = new System.Windows.Forms.Panel();
            this.panel12 = new System.Windows.Forms.Panel();
            this.panel11 = new System.Windows.Forms.Panel();
            this.panel10 = new System.Windows.Forms.Panel();
            this.panel9 = new System.Windows.Forms.Panel();
            this.panel8 = new System.Windows.Forms.Panel();
            this.panel7 = new System.Windows.Forms.Panel();
            this.panel6 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lbHint = new System.Windows.Forms.Label();
            this.btnPrevious = new System.Windows.Forms.Button();
            this.cboPageList = new System.Windows.Forms.ComboBox();
            this.btnNext = new System.Windows.Forms.Button();
            this.cms = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel2.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.EnviroPage.SuspendLayout();
            this.panel1.SuspendLayout();
            this.cms.SuspendLayout();
            this.SuspendLayout();
            // 
            // img
            // 
            this.img.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("img.ImageStream")));
            this.img.TransparentColor = System.Drawing.Color.Transparent;
            this.img.Images.SetKeyName(0, "11.png");
            this.img.Images.SetKeyName(1, "9.png");
            this.img.Images.SetKeyName(2, "1.png");
            this.img.Images.SetKeyName(3, "2.png");
            this.img.Images.SetKeyName(4, "4.png");
            this.img.Images.SetKeyName(5, "5.png");
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnSave);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 478);
            this.panel2.Margin = new System.Windows.Forms.Padding(4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(325, 43);
            this.panel2.TabIndex = 2;
            // 
            // btnSave
            // 
            this.btnSave.Image = global::HDL_Buspro_Setup_Tool.Properties.Resources.Save;
            this.btnSave.Location = new System.Drawing.Point(220, 7);
            this.btnSave.Margin = new System.Windows.Forms.Padding(4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(99, 28);
            this.btnSave.TabIndex = 0;
            this.btnSave.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.AutoSize = false;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsb1,
            this.tsb2,
            this.tsb3,
            this.tsb4,
            this.tsb5,
            this.tsb6});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(325, 56);
            this.toolStrip1.TabIndex = 3;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tsb1
            // 
            this.tsb1.AutoSize = false;
            this.tsb1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsb1.Image = ((System.Drawing.Image)(resources.GetObject("tsb1.Image")));
            this.tsb1.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsb1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb1.Name = "tsb1";
            this.tsb1.Size = new System.Drawing.Size(46, 46);
            this.tsb1.Tag = "1";
            this.tsb1.Text = "toolStripButton1";
            this.tsb1.Click += new System.EventHandler(this.tsb1_Click);
            // 
            // tsb2
            // 
            this.tsb2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsb2.Image = ((System.Drawing.Image)(resources.GetObject("tsb2.Image")));
            this.tsb2.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsb2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb2.Name = "tsb2";
            this.tsb2.Size = new System.Drawing.Size(50, 53);
            this.tsb2.Tag = "2";
            this.tsb2.Text = "toolStripButton2";
            this.tsb2.Click += new System.EventHandler(this.tsb1_Click);
            // 
            // tsb3
            // 
            this.tsb3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsb3.Image = ((System.Drawing.Image)(resources.GetObject("tsb3.Image")));
            this.tsb3.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsb3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb3.Name = "tsb3";
            this.tsb3.Size = new System.Drawing.Size(50, 53);
            this.tsb3.Tag = "3";
            this.tsb3.Text = "toolStripButton3";
            this.tsb3.Click += new System.EventHandler(this.tsb1_Click);
            // 
            // tsb4
            // 
            this.tsb4.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsb4.Image = ((System.Drawing.Image)(resources.GetObject("tsb4.Image")));
            this.tsb4.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsb4.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb4.Name = "tsb4";
            this.tsb4.Size = new System.Drawing.Size(50, 53);
            this.tsb4.Tag = "39";
            this.tsb4.Text = "toolStripButton4";
            this.tsb4.Click += new System.EventHandler(this.tsb1_Click);
            // 
            // tsb5
            // 
            this.tsb5.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsb5.Image = ((System.Drawing.Image)(resources.GetObject("tsb5.Image")));
            this.tsb5.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsb5.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb5.Name = "tsb5";
            this.tsb5.RightToLeftAutoMirrorImage = true;
            this.tsb5.Size = new System.Drawing.Size(50, 53);
            this.tsb5.Tag = "48";
            this.tsb5.Text = "toolStripButton5";
            this.tsb5.Click += new System.EventHandler(this.tsb1_Click);
            // 
            // tsb6
            // 
            this.tsb6.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsb6.Image = ((System.Drawing.Image)(resources.GetObject("tsb6.Image")));
            this.tsb6.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsb6.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb6.Name = "tsb6";
            this.tsb6.Size = new System.Drawing.Size(50, 53);
            this.tsb6.Tag = "57";
            this.tsb6.Text = "toolStripButton6";
            this.tsb6.Click += new System.EventHandler(this.tsb1_Click);
            // 
            // EnviroPage
            // 
            this.EnviroPage.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.OutsetPartial;
            this.EnviroPage.ColumnCount = 3;
            this.EnviroPage.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 31.69231F));
            this.EnviroPage.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 34.76923F));
            this.EnviroPage.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.EnviroPage.Controls.Add(this.panel14, 2, 3);
            this.EnviroPage.Controls.Add(this.panel13, 1, 3);
            this.EnviroPage.Controls.Add(this.panel12, 0, 3);
            this.EnviroPage.Controls.Add(this.panel11, 2, 2);
            this.EnviroPage.Controls.Add(this.panel10, 1, 2);
            this.EnviroPage.Controls.Add(this.panel9, 0, 2);
            this.EnviroPage.Controls.Add(this.panel8, 2, 1);
            this.EnviroPage.Controls.Add(this.panel7, 1, 1);
            this.EnviroPage.Controls.Add(this.panel6, 0, 1);
            this.EnviroPage.Controls.Add(this.panel5, 2, 0);
            this.EnviroPage.Controls.Add(this.panel4, 1, 0);
            this.EnviroPage.Controls.Add(this.panel3, 0, 0);
            this.EnviroPage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.EnviroPage.Location = new System.Drawing.Point(0, 80);
            this.EnviroPage.Name = "EnviroPage";
            this.EnviroPage.RowCount = 4;
            this.EnviroPage.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.EnviroPage.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.EnviroPage.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.EnviroPage.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.EnviroPage.Size = new System.Drawing.Size(325, 398);
            this.EnviroPage.TabIndex = 0;
            // 
            // panel14
            // 
            this.panel14.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.panel14.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel14.Location = new System.Drawing.Point(220, 300);
            this.panel14.Name = "panel14";
            this.panel14.Size = new System.Drawing.Size(99, 92);
            this.panel14.TabIndex = 11;
            this.panel14.Tag = "11";
            // 
            // panel13
            // 
            this.panel13.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.panel13.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel13.Location = new System.Drawing.Point(108, 300);
            this.panel13.Name = "panel13";
            this.panel13.Size = new System.Drawing.Size(103, 92);
            this.panel13.TabIndex = 10;
            this.panel13.Tag = "10";
            // 
            // panel12
            // 
            this.panel12.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.panel12.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel12.Location = new System.Drawing.Point(6, 300);
            this.panel12.Name = "panel12";
            this.panel12.Size = new System.Drawing.Size(93, 92);
            this.panel12.TabIndex = 9;
            this.panel12.Tag = "9";
            // 
            // panel11
            // 
            this.panel11.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.panel11.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel11.Location = new System.Drawing.Point(220, 202);
            this.panel11.Name = "panel11";
            this.panel11.Size = new System.Drawing.Size(99, 89);
            this.panel11.TabIndex = 8;
            this.panel11.Tag = "8";
            // 
            // panel10
            // 
            this.panel10.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.panel10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel10.Location = new System.Drawing.Point(108, 202);
            this.panel10.Name = "panel10";
            this.panel10.Size = new System.Drawing.Size(103, 89);
            this.panel10.TabIndex = 7;
            this.panel10.Tag = "7";
            // 
            // panel9
            // 
            this.panel9.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.panel9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel9.Location = new System.Drawing.Point(6, 202);
            this.panel9.Name = "panel9";
            this.panel9.Size = new System.Drawing.Size(93, 89);
            this.panel9.TabIndex = 6;
            this.panel9.Tag = "6";
            // 
            // panel8
            // 
            this.panel8.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.panel8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel8.Location = new System.Drawing.Point(220, 104);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(99, 89);
            this.panel8.TabIndex = 5;
            this.panel8.Tag = "5";
            // 
            // panel7
            // 
            this.panel7.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel7.Location = new System.Drawing.Point(108, 104);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(103, 89);
            this.panel7.TabIndex = 4;
            this.panel7.Tag = "4";
            // 
            // panel6
            // 
            this.panel6.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel6.Location = new System.Drawing.Point(6, 104);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(93, 89);
            this.panel6.TabIndex = 3;
            this.panel6.Tag = "3";
            // 
            // panel5
            // 
            this.panel5.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(220, 6);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(99, 89);
            this.panel5.TabIndex = 2;
            this.panel5.Tag = "2";
            // 
            // panel4
            // 
            this.panel4.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(108, 6);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(103, 89);
            this.panel4.TabIndex = 1;
            this.panel4.Tag = "1";
            // 
            // panel3
            // 
            this.panel3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.panel3.Location = new System.Drawing.Point(6, 6);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(93, 89);
            this.panel3.TabIndex = 0;
            this.panel3.Tag = "0";
            this.panel3.DoubleClick += new System.EventHandler(this.panel3_DoubleClick);
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.lbHint);
            this.panel1.Controls.Add(this.btnPrevious);
            this.panel1.Controls.Add(this.cboPageList);
            this.panel1.Controls.Add(this.btnNext);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 56);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(325, 24);
            this.panel1.TabIndex = 1;
            // 
            // lbHint
            // 
            this.lbHint.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbHint.Location = new System.Drawing.Point(0, 0);
            this.lbHint.Name = "lbHint";
            this.lbHint.Size = new System.Drawing.Size(113, 22);
            this.lbHint.TabIndex = 0;
            this.lbHint.Text = "Total Pages:";
            this.lbHint.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnPrevious
            // 
            this.btnPrevious.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnPrevious.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnPrevious.Location = new System.Drawing.Point(113, 0);
            this.btnPrevious.Name = "btnPrevious";
            this.btnPrevious.Size = new System.Drawing.Size(53, 22);
            this.btnPrevious.TabIndex = 2;
            this.btnPrevious.Text = "<<";
            this.btnPrevious.UseVisualStyleBackColor = true;
            this.btnPrevious.Click += new System.EventHandler(this.btnPrevious_Click);
            // 
            // cboPageList
            // 
            this.cboPageList.Dock = System.Windows.Forms.DockStyle.Right;
            this.cboPageList.FormattingEnabled = true;
            this.cboPageList.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10"});
            this.cboPageList.Location = new System.Drawing.Point(166, 0);
            this.cboPageList.Name = "cboPageList";
            this.cboPageList.Size = new System.Drawing.Size(105, 22);
            this.cboPageList.TabIndex = 3;
            this.cboPageList.SelectedIndexChanged += new System.EventHandler(this.cboPageList_SelectedIndexChanged);
            // 
            // btnNext
            // 
            this.btnNext.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnNext.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnNext.Location = new System.Drawing.Point(271, 0);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(52, 22);
            this.btnNext.TabIndex = 1;
            this.btnNext.Text = ">>";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // cms
            // 
            this.cms.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deleteToolStripMenuItem});
            this.cms.Name = "cms";
            this.cms.Size = new System.Drawing.Size(153, 48);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.deleteToolStripMenuItem.Text = "Delete";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // FrmEnviroUI
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(325, 521);
            this.Controls.Add(this.EnviroPage);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.panel2);
            this.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FrmEnviroUI";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "UI";
            this.Load += new System.EventHandler(this.FrmColorDLPUI_Load);
            this.Shown += new System.EventHandler(this.FrmEnviroUI_Shown);
            this.panel2.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.EnviroPage.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.cms.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ImageList img;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsb1;
        private System.Windows.Forms.ToolStripButton tsb2;
        private System.Windows.Forms.ToolStripButton tsb3;
        private System.Windows.Forms.ToolStripButton tsb4;
        private System.Windows.Forms.TableLayoutPanel EnviroPage;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStripButton tsb5;
        private System.Windows.Forms.ToolStripButton tsb6;
        private System.Windows.Forms.Label lbHint;
        private System.Windows.Forms.Button btnPrevious;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.ComboBox cboPageList;
        private System.Windows.Forms.Panel panel14;
        private System.Windows.Forms.Panel panel13;
        private System.Windows.Forms.Panel panel12;
        private System.Windows.Forms.Panel panel11;
        private System.Windows.Forms.Panel panel10;
        private System.Windows.Forms.Panel panel9;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.ContextMenuStrip cms;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;

    }
}