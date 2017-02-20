namespace HDL_Buspro_Setup_Tool
{
    partial class frmManualFunctionLists
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lbHint = new System.Windows.Forms.Label();
            this.btnBuildList = new System.Windows.Forms.Button();
            this.btnReadRemark = new System.Windows.Forms.Button();
            this.btnSaveFunctionList = new System.Windows.Forms.Button();
            this.btnAddScene = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnRebuild = new System.Windows.Forms.Button();
            this.tabLight = new System.Windows.Forms.TabPage();
            this.tabSim = new System.Windows.Forms.TabControl();
            this.dgOnline = new System.Windows.Forms.DataGridView();
            this.clSelect = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.clEnable = new System.Windows.Forms.DataGridViewImageColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clSubID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clDevID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Remark = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clDesp = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column14 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IndexID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cl10 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LightLists = new RowMergeView();
            this.Selected1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.smallType1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DeviceName1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Chn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.statusStrip1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.tabLight.SuspendLayout();
            this.tabSim.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgOnline)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LightLists)).BeginInit();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripStatusLabel2});
            this.statusStrip1.Location = new System.Drawing.Point(0, 456);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(979, 22);
            this.statusStrip1.TabIndex = 88;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(93, 17);
            this.toolStripStatusLabel1.Text = "Current Status:";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(11, 17);
            this.toolStripStatusLabel2.Text = "|";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.lbHint);
            this.panel2.Controls.Add(this.btnBuildList);
            this.panel2.Controls.Add(this.btnReadRemark);
            this.panel2.Controls.Add(this.btnSaveFunctionList);
            this.panel2.Controls.Add(this.btnAddScene);
            this.panel2.Controls.Add(this.btnExport);
            this.panel2.Controls.Add(this.btnRebuild);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 419);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(979, 37);
            this.panel2.TabIndex = 14;
            // 
            // lbHint
            // 
            this.lbHint.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbHint.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbHint.ForeColor = System.Drawing.Color.Blue;
            this.lbHint.Location = new System.Drawing.Point(0, 0);
            this.lbHint.Name = "lbHint";
            this.lbHint.Size = new System.Drawing.Size(259, 37);
            this.lbHint.TabIndex = 15;
            this.lbHint.Text = "Hint: Could save function lists to a file, then could use the list without device" +
    "s.";
            this.lbHint.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnBuildList
            // 
            this.btnBuildList.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnBuildList.Location = new System.Drawing.Point(259, 0);
            this.btnBuildList.Name = "btnBuildList";
            this.btnBuildList.Size = new System.Drawing.Size(120, 37);
            this.btnBuildList.TabIndex = 19;
            this.btnBuildList.Text = "Refresh All Devices";
            this.btnBuildList.UseVisualStyleBackColor = true;
            this.btnBuildList.Click += new System.EventHandler(this.btnBuildList_Click);
            // 
            // btnReadRemark
            // 
            this.btnReadRemark.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnReadRemark.Location = new System.Drawing.Point(379, 0);
            this.btnReadRemark.Name = "btnReadRemark";
            this.btnReadRemark.Size = new System.Drawing.Size(120, 37);
            this.btnReadRemark.TabIndex = 16;
            this.btnReadRemark.Text = "Refresh Remark";
            this.btnReadRemark.UseVisualStyleBackColor = true;
            this.btnReadRemark.Click += new System.EventHandler(this.btnReadRemark_Click);
            // 
            // btnSaveFunctionList
            // 
            this.btnSaveFunctionList.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnSaveFunctionList.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnSaveFunctionList.Location = new System.Drawing.Point(499, 0);
            this.btnSaveFunctionList.Name = "btnSaveFunctionList";
            this.btnSaveFunctionList.Size = new System.Drawing.Size(120, 37);
            this.btnSaveFunctionList.TabIndex = 14;
            this.btnSaveFunctionList.Text = "Add";
            this.btnSaveFunctionList.UseVisualStyleBackColor = true;
            this.btnSaveFunctionList.Click += new System.EventHandler(this.btnSaveFunctionList_Click);
            // 
            // btnAddScene
            // 
            this.btnAddScene.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnAddScene.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnAddScene.Location = new System.Drawing.Point(619, 0);
            this.btnAddScene.Name = "btnAddScene";
            this.btnAddScene.Size = new System.Drawing.Size(120, 37);
            this.btnAddScene.TabIndex = 20;
            this.btnAddScene.Text = "Add Scene";
            this.btnAddScene.UseVisualStyleBackColor = true;
            this.btnAddScene.Click += new System.EventHandler(this.btnAddScene_Click);
            // 
            // btnExport
            // 
            this.btnExport.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnExport.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnExport.Location = new System.Drawing.Point(739, 0);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(120, 37);
            this.btnExport.TabIndex = 18;
            this.btnExport.Text = "Export";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btnRebuild
            // 
            this.btnRebuild.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnRebuild.Location = new System.Drawing.Point(859, 0);
            this.btnRebuild.Name = "btnRebuild";
            this.btnRebuild.Size = new System.Drawing.Size(120, 37);
            this.btnRebuild.TabIndex = 17;
            this.btnRebuild.Text = "Save Whole Lists";
            this.btnRebuild.UseVisualStyleBackColor = true;
            this.btnRebuild.Click += new System.EventHandler(this.btnRebuild_Click);
            // 
            // tabLight
            // 
            this.tabLight.Controls.Add(this.LightLists);
            this.tabLight.Location = new System.Drawing.Point(4, 23);
            this.tabLight.Name = "tabLight";
            this.tabLight.Padding = new System.Windows.Forms.Padding(3);
            this.tabLight.Size = new System.Drawing.Size(392, 392);
            this.tabLight.TabIndex = 1;
            this.tabLight.Tag = "1";
            this.tabLight.Text = "Functions List";
            this.tabLight.UseVisualStyleBackColor = true;
            // 
            // tabSim
            // 
            this.tabSim.Controls.Add(this.tabLight);
            this.tabSim.Dock = System.Windows.Forms.DockStyle.Right;
            this.tabSim.Location = new System.Drawing.Point(579, 0);
            this.tabSim.Name = "tabSim";
            this.tabSim.SelectedIndex = 0;
            this.tabSim.Size = new System.Drawing.Size(400, 419);
            this.tabSim.TabIndex = 90;
            this.tabSim.SelectedIndexChanged += new System.EventHandler(this.tabSim_SelectedIndexChanged);
            // 
            // dgOnline
            // 
            this.dgOnline.AllowDrop = true;
            this.dgOnline.AllowUserToAddRows = false;
            this.dgOnline.AllowUserToDeleteRows = false;
            this.dgOnline.AllowUserToResizeRows = false;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Calibri", 9F);
            this.dgOnline.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle2;
            this.dgOnline.BackgroundColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.AliceBlue;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgOnline.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgOnline.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.clSelect,
            this.clEnable,
            this.Column4,
            this.clSubID,
            this.clDevID,
            this.clName,
            this.Remark,
            this.clDesp,
            this.Column14,
            this.IndexID,
            this.cl10});
            this.dgOnline.Cursor = System.Windows.Forms.Cursors.Default;
            this.dgOnline.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgOnline.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgOnline.EnableHeadersVisualStyles = false;
            this.dgOnline.Location = new System.Drawing.Point(0, 0);
            this.dgOnline.MultiSelect = false;
            this.dgOnline.Name = "dgOnline";
            this.dgOnline.ReadOnly = true;
            this.dgOnline.RowHeadersVisible = false;
            this.dgOnline.RowHeadersWidth = 10;
            dataGridViewCellStyle13.Font = new System.Drawing.Font("Calibri", 9F);
            this.dgOnline.RowsDefaultCellStyle = dataGridViewCellStyle13;
            this.dgOnline.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Calibri", 9F);
            this.dgOnline.RowTemplate.Height = 23;
            this.dgOnline.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgOnline.Size = new System.Drawing.Size(579, 419);
            this.dgOnline.TabIndex = 91;
            this.dgOnline.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgOnline_CellClick);
            // 
            // clSelect
            // 
            this.clSelect.HeaderText = "";
            this.clSelect.Name = "clSelect";
            this.clSelect.ReadOnly = true;
            this.clSelect.Visible = false;
            this.clSelect.Width = 32;
            // 
            // clEnable
            // 
            this.clEnable.HeaderText = "Session Visibility";
            this.clEnable.Name = "clEnable";
            this.clEnable.ReadOnly = true;
            this.clEnable.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.clEnable.Visible = false;
            // 
            // Column4
            // 
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Column4.DefaultCellStyle = dataGridViewCellStyle4;
            this.Column4.HeaderText = "Index";
            this.Column4.Name = "Column4";
            this.Column4.ReadOnly = true;
            this.Column4.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column4.Width = 48;
            // 
            // clSubID
            // 
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Calibri", 9F);
            this.clSubID.DefaultCellStyle = dataGridViewCellStyle5;
            this.clSubID.HeaderText = "SubNet ID";
            this.clSubID.Name = "clSubID";
            this.clSubID.ReadOnly = true;
            this.clSubID.Width = 80;
            // 
            // clDevID
            // 
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Calibri", 9F);
            this.clDevID.DefaultCellStyle = dataGridViewCellStyle6;
            this.clDevID.HeaderText = "Device ID";
            this.clDevID.Name = "clDevID";
            this.clDevID.ReadOnly = true;
            this.clDevID.Width = 80;
            // 
            // clName
            // 
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Calibri", 9F);
            this.clName.DefaultCellStyle = dataGridViewCellStyle7;
            this.clName.HeaderText = "Model";
            this.clName.Name = "clName";
            this.clName.ReadOnly = true;
            this.clName.Width = 140;
            // 
            // Remark
            // 
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Calibri", 9F);
            this.Remark.DefaultCellStyle = dataGridViewCellStyle8;
            this.Remark.HeaderText = "Name";
            this.Remark.Name = "Remark";
            this.Remark.ReadOnly = true;
            this.Remark.Width = 140;
            // 
            // clDesp
            // 
            this.clDesp.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("Calibri", 9F);
            this.clDesp.DefaultCellStyle = dataGridViewCellStyle9;
            this.clDesp.HeaderText = "Description";
            this.clDesp.Name = "clDesp";
            this.clDesp.ReadOnly = true;
            this.clDesp.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Column14
            // 
            dataGridViewCellStyle10.Font = new System.Drawing.Font("Calibri", 9F);
            this.Column14.DefaultCellStyle = dataGridViewCellStyle10;
            this.Column14.HeaderText = "Version";
            this.Column14.Name = "Column14";
            this.Column14.ReadOnly = true;
            this.Column14.Visible = false;
            this.Column14.Width = 200;
            // 
            // IndexID
            // 
            dataGridViewCellStyle11.Font = new System.Drawing.Font("Calibri", 9F);
            this.IndexID.DefaultCellStyle = dataGridViewCellStyle11;
            this.IndexID.HeaderText = "ID";
            this.IndexID.Name = "IndexID";
            this.IndexID.ReadOnly = true;
            this.IndexID.Visible = false;
            // 
            // cl10
            // 
            dataGridViewCellStyle12.Font = new System.Drawing.Font("Calibri", 9F);
            this.cl10.DefaultCellStyle = dataGridViewCellStyle12;
            this.cl10.HeaderText = "Backup status";
            this.cl10.Name = "cl10";
            this.cl10.ReadOnly = true;
            this.cl10.Visible = false;
            this.cl10.Width = 150;
            // 
            // LightLists
            // 
            this.LightLists.AllowDrop = true;
            this.LightLists.AllowUserToAddRows = false;
            this.LightLists.AllowUserToDeleteRows = false;
            this.LightLists.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.LightLists.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.LightLists.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.LightLists.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Selected1,
            this.smallType1,
            this.DeviceName1,
            this.Chn,
            this.dataGridViewTextBoxColumn4});
            this.LightLists.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LightLists.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.LightLists.Location = new System.Drawing.Point(3, 3);
            this.LightLists.MergeColumnHeaderBackColor = System.Drawing.SystemColors.Control;
            this.LightLists.MergeColumnNames = null;
            this.LightLists.Name = "LightLists";
            this.LightLists.RowHeadersWidth = 10;
            this.LightLists.RowTemplate.Height = 23;
            this.LightLists.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.LightLists.Size = new System.Drawing.Size(386, 386);
            this.LightLists.TabIndex = 13;
            this.LightLists.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.LightLists_CellEndEdit);
            this.LightLists.CurrentCellDirtyStateChanged += new System.EventHandler(this.LightLists_CurrentCellDirtyStateChanged);
            this.LightLists.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.LightLists_MouseDoubleClick);
            // 
            // Selected1
            // 
            this.Selected1.HeaderText = "";
            this.Selected1.Name = "Selected1";
            this.Selected1.Width = 48;
            // 
            // smallType1
            // 
            this.smallType1.HeaderText = "Small Type";
            this.smallType1.Name = "smallType1";
            this.smallType1.ReadOnly = true;
            // 
            // DeviceName1
            // 
            this.DeviceName1.HeaderText = "Device Name";
            this.DeviceName1.Name = "DeviceName1";
            this.DeviceName1.ReadOnly = true;
            this.DeviceName1.Visible = false;
            this.DeviceName1.Width = 200;
            // 
            // Chn
            // 
            this.Chn.HeaderText = "Chn.";
            this.Chn.Name = "Chn";
            this.Chn.ReadOnly = true;
            this.Chn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Chn.Width = 64;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn4.DataPropertyName = "3";
            this.dataGridViewTextBoxColumn4.HeaderText = "Remark";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // frmManualFunctionLists
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(979, 478);
            this.Controls.Add(this.dgOnline);
            this.Controls.Add(this.tabSim);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.statusStrip1);
            this.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "frmManualFunctionLists";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "HDL System Functions List";
            this.Load += new System.EventHandler(this.frmEMain_Load);
            this.Shown += new System.EventHandler(this.frmFunctionLists_Shown);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.tabLight.ResumeLayout(false);
            this.tabSim.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgOnline)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LightLists)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnSaveFunctionList;
        private System.Windows.Forms.Label lbHint;
        private System.Windows.Forms.Button btnReadRemark;
        private System.Windows.Forms.Button btnRebuild;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.TabPage tabLight;
        private RowMergeView LightLists;
        private System.Windows.Forms.TabControl tabSim;
        private System.Windows.Forms.DataGridView dgOnline;
        private System.Windows.Forms.DataGridViewCheckBoxColumn clSelect;
        private System.Windows.Forms.DataGridViewImageColumn clEnable;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn clSubID;
        private System.Windows.Forms.DataGridViewTextBoxColumn clDevID;
        private System.Windows.Forms.DataGridViewTextBoxColumn clName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Remark;
        private System.Windows.Forms.DataGridViewTextBoxColumn clDesp;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column14;
        private System.Windows.Forms.DataGridViewTextBoxColumn IndexID;
        private System.Windows.Forms.DataGridViewTextBoxColumn cl10;
        private System.Windows.Forms.Button btnBuildList;
        private System.Windows.Forms.Button btnAddScene;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Selected1;
        private System.Windows.Forms.DataGridViewTextBoxColumn smallType1;
        private System.Windows.Forms.DataGridViewTextBoxColumn DeviceName1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Chn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;

    }
}