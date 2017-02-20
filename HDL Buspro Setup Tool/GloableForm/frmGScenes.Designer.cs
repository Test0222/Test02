namespace HDL_Buspro_Setup_Tool
{
    partial class frmGScenes
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmGScenes));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tbnew = new System.Windows.Forms.ToolStripButton();
            this.tbSave = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton7 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton9 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton10 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.tslRead = new System.Windows.Forms.ToolStripButton();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton11 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.Dg1 = new RowMergeView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Dg1)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tbnew,
            this.tbSave,
            this.toolStripButton7,
            this.toolStripSeparator4,
            this.toolStripButton9,
            this.toolStripButton10,
            this.toolStripSeparator5,
            this.tslRead,
            this.toolStripLabel2,
            this.toolStripButton11,
            this.toolStripSeparator1});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(855, 25);
            this.toolStrip1.TabIndex = 81;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tbnew
            // 
            this.tbnew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbnew.Image = global::HDL_Buspro_Setup_Tool.Properties.Resources.新建;
            this.tbnew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbnew.Name = "tbnew";
            this.tbnew.Size = new System.Drawing.Size(23, 22);
            this.tbnew.Text = "&New";
            this.tbnew.ToolTipText = "Load Default";
            // 
            // tbSave
            // 
            this.tbSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbSave.Image = global::HDL_Buspro_Setup_Tool.Properties.Resources.保存;
            this.tbSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbSave.Name = "tbSave";
            this.tbSave.Size = new System.Drawing.Size(23, 22);
            this.tbSave.Text = "&Save";
            this.tbSave.ToolTipText = "Save ";
            // 
            // toolStripButton7
            // 
            this.toolStripButton7.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton7.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton7.Image")));
            this.toolStripButton7.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton7.Name = "toolStripButton7";
            this.toolStripButton7.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton7.Text = "&Print";
            this.toolStripButton7.Visible = false;
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButton9
            // 
            this.toolStripButton9.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton9.Image = global::HDL_Buspro_Setup_Tool.Properties.Resources.复制;
            this.toolStripButton9.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton9.Name = "toolStripButton9";
            this.toolStripButton9.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton9.Text = "&Copy";
            // 
            // toolStripButton10
            // 
            this.toolStripButton10.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton10.Image = global::HDL_Buspro_Setup_Tool.Properties.Resources.粘贴;
            this.toolStripButton10.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton10.Name = "toolStripButton10";
            this.toolStripButton10.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton10.Text = "&Paste";
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 25);
            // 
            // tslRead
            // 
            this.tslRead.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tslRead.Font = new System.Drawing.Font("Times New Roman", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tslRead.Image = global::HDL_Buspro_Setup_Tool.Properties.Resources.下载;
            this.tslRead.ImageTransparentColor = System.Drawing.Color.Silver;
            this.tslRead.Name = "tslRead";
            this.tslRead.Size = new System.Drawing.Size(23, 22);
            this.tslRead.Tag = "0";
            this.tslRead.Text = "Read";
            this.tslRead.ToolTipText = "Read";
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripLabel2.Image = global::HDL_Buspro_Setup_Tool.Properties.Resources.上传;
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(23, 22);
            this.toolStripLabel2.Tag = "1";
            this.toolStripLabel2.Text = "toolStripLabel1";
            this.toolStripLabel2.ToolTipText = "Upload";
            // 
            // toolStripButton11
            // 
            this.toolStripButton11.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton11.Image = global::HDL_Buspro_Setup_Tool.Properties.Resources.帮助;
            this.toolStripButton11.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton11.Name = "toolStripButton11";
            this.toolStripButton11.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton11.Text = "He&lp";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // Dg1
            // 
            this.Dg1.AllowUserToAddRows = false;
            this.Dg1.AllowUserToDeleteRows = false;
            this.Dg1.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllHeaders;
            this.Dg1.ColumnHeadersHeight = 48;
            this.Dg1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1});
            this.Dg1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Dg1.Location = new System.Drawing.Point(0, 25);
            this.Dg1.MergeColumnHeaderBackColor = System.Drawing.SystemColors.Control;
            this.Dg1.MergeColumnNames = ((System.Collections.Generic.List<string>)(resources.GetObject("Dg1.MergeColumnNames")));
            this.Dg1.Name = "Dg1";
            this.Dg1.RowHeadersWidth = 24;
            this.Dg1.RowTemplate.Height = 23;
            this.Dg1.Size = new System.Drawing.Size(855, 391);
            this.Dg1.TabIndex = 82;
            this.Dg1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.Dg1_CellContentClick);
            // 
            // Column1
            // 
            this.Column1.HeaderText = "Column1";
            this.Column1.Name = "Column1";
            // 
            // frmGScenes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(855, 416);
            this.Controls.Add(this.Dg1);
            this.Controls.Add(this.toolStrip1);
            this.Name = "frmGScenes";
            this.Text = "frmGScenes";
            this.Load += new System.EventHandler(this.frmGScenes_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Dg1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tbnew;
        private System.Windows.Forms.ToolStripButton tbSave;
        private System.Windows.Forms.ToolStripButton toolStripButton7;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripButton toolStripButton9;
        private System.Windows.Forms.ToolStripButton toolStripButton10;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripButton tslRead;
        private System.Windows.Forms.ToolStripButton toolStripLabel2;
        private System.Windows.Forms.ToolStripButton toolStripButton11;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private RowMergeView Dg1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
    }
}