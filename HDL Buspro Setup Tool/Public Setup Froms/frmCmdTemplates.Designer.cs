namespace HDL_Buspro_Setup_Tool
{
    partial class frmCmdTemplates
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCmdTemplates));
            this.gbDevA = new System.Windows.Forms.GroupBox();
            this.panel6 = new System.Windows.Forms.Panel();
            this.tbTemplates = new System.Windows.Forms.TextBox();
            this.btnSavetemplate = new System.Windows.Forms.Button();
            this.lbName = new System.Windows.Forms.Label();
            this.btnUpdateA = new System.Windows.Forms.Button();
            this.btnSure = new System.Windows.Forms.Button();
            this.txtFrm = new System.Windows.Forms.TextBox();
            this.lbTarget = new System.Windows.Forms.Label();
            this.dgvListA = new System.Windows.Forms.DataGridView();
            this.cl1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cl2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cl3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cl4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cl5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cl6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cl7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cl8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.copy = new System.Windows.Forms.ToolStripMenuItem();
            this.paste = new System.Windows.Forms.ToolStripMenuItem();
            this.panel3 = new System.Windows.Forms.Panel();
            this.LvTemplates = new System.Windows.Forms.ListView();
            this.TemplateID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tbDelTemplate = new System.Windows.Forms.ToolStripMenuItem();
            this.label2 = new System.Windows.Forms.Label();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.addToExportListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gbDevA.SuspendLayout();
            this.panel6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvListA)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.contextMenuStrip2.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbDevA
            // 
            this.gbDevA.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.gbDevA.Controls.Add(this.panel6);
            this.gbDevA.Controls.Add(this.dgvListA);
            this.gbDevA.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbDevA.Location = new System.Drawing.Point(171, 0);
            this.gbDevA.Name = "gbDevA";
            this.gbDevA.Size = new System.Drawing.Size(708, 360);
            this.gbDevA.TabIndex = 0;
            this.gbDevA.TabStop = false;
            this.gbDevA.Text = "Commands Setting";
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.tbTemplates);
            this.panel6.Controls.Add(this.btnSavetemplate);
            this.panel6.Controls.Add(this.lbName);
            this.panel6.Controls.Add(this.btnUpdateA);
            this.panel6.Controls.Add(this.btnSure);
            this.panel6.Controls.Add(this.txtFrm);
            this.panel6.Controls.Add(this.lbTarget);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel6.Location = new System.Drawing.Point(3, 18);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(702, 39);
            this.panel6.TabIndex = 3;
            // 
            // tbTemplates
            // 
            this.tbTemplates.Location = new System.Drawing.Point(79, 7);
            this.tbTemplates.Name = "tbTemplates";
            this.tbTemplates.Size = new System.Drawing.Size(149, 22);
            this.tbTemplates.TabIndex = 147;
            // 
            // btnSavetemplate
            // 
            this.btnSavetemplate.Location = new System.Drawing.Point(589, 7);
            this.btnSavetemplate.Name = "btnSavetemplate";
            this.btnSavetemplate.Size = new System.Drawing.Size(80, 24);
            this.btnSavetemplate.TabIndex = 142;
            this.btnSavetemplate.Text = "New templates";
            this.btnSavetemplate.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSavetemplate.UseVisualStyleBackColor = true;
            this.btnSavetemplate.Click += new System.EventHandler(this.btnSavetemplate_Click);
            // 
            // lbName
            // 
            this.lbName.Location = new System.Drawing.Point(3, 11);
            this.lbName.Name = "lbName";
            this.lbName.Size = new System.Drawing.Size(82, 14);
            this.lbName.TabIndex = 146;
            this.lbName.Text = "Templates:";
            // 
            // btnUpdateA
            // 
            this.btnUpdateA.Image = global::HDL_Buspro_Setup_Tool.Properties.Resources.Save;
            this.btnUpdateA.Location = new System.Drawing.Point(486, 7);
            this.btnUpdateA.Name = "btnUpdateA";
            this.btnUpdateA.Size = new System.Drawing.Size(80, 24);
            this.btnUpdateA.TabIndex = 141;
            this.btnUpdateA.UseVisualStyleBackColor = true;
            this.btnUpdateA.Click += new System.EventHandler(this.btnUpdateA_Click);
            // 
            // btnSure
            // 
            this.btnSure.Location = new System.Drawing.Point(383, 7);
            this.btnSure.Name = "btnSure";
            this.btnSure.Size = new System.Drawing.Size(80, 24);
            this.btnSure.TabIndex = 5;
            this.btnSure.Text = "+";
            this.btnSure.UseVisualStyleBackColor = true;
            this.btnSure.Click += new System.EventHandler(this.btnSure_Click);
            // 
            // txtFrm
            // 
            this.txtFrm.Location = new System.Drawing.Point(295, 8);
            this.txtFrm.Name = "txtFrm";
            this.txtFrm.Size = new System.Drawing.Size(75, 22);
            this.txtFrm.TabIndex = 3;
            this.txtFrm.Text = "1";
            // 
            // lbTarget
            // 
            this.lbTarget.AutoSize = true;
            this.lbTarget.Location = new System.Drawing.Point(258, 11);
            this.lbTarget.Name = "lbTarget";
            this.lbTarget.Size = new System.Drawing.Size(31, 14);
            this.lbTarget.TabIndex = 0;
            this.lbTarget.Text = "Add:";
            // 
            // dgvListA
            // 
            this.dgvListA.AllowDrop = true;
            this.dgvListA.AllowUserToAddRows = false;
            this.dgvListA.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.AliceBlue;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Calibri", 9F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvListA.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvListA.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvListA.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.cl1,
            this.cl2,
            this.cl3,
            this.cl4,
            this.cl5,
            this.cl6,
            this.cl7,
            this.cl8});
            this.dgvListA.ContextMenuStrip = this.contextMenuStrip1;
            this.dgvListA.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dgvListA.EnableHeadersVisualStyles = false;
            this.dgvListA.Location = new System.Drawing.Point(3, 57);
            this.dgvListA.Name = "dgvListA";
            this.dgvListA.ReadOnly = true;
            this.dgvListA.RowHeadersVisible = false;
            this.dgvListA.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvListA.Size = new System.Drawing.Size(702, 300);
            this.dgvListA.TabIndex = 1;
            this.dgvListA.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvListA_CellClick);
            this.dgvListA.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvListA_CellMouseDown);
            this.dgvListA.DragDrop += new System.Windows.Forms.DragEventHandler(this.dgvListA_DragDrop);
            this.dgvListA.DragEnter += new System.Windows.Forms.DragEventHandler(this.dgvListA_DragEnter);
            // 
            // cl1
            // 
            this.cl1.HeaderText = "ID";
            this.cl1.Name = "cl1";
            this.cl1.ReadOnly = true;
            this.cl1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.cl1.Width = 32;
            // 
            // cl2
            // 
            this.cl2.HeaderText = "Subnet ID";
            this.cl2.Name = "cl2";
            this.cl2.ReadOnly = true;
            this.cl2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.cl2.Width = 80;
            // 
            // cl3
            // 
            this.cl3.HeaderText = "Device ID";
            this.cl3.Name = "cl3";
            this.cl3.ReadOnly = true;
            this.cl3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.cl3.Width = 80;
            // 
            // cl4
            // 
            this.cl4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.cl4.HeaderText = "Type";
            this.cl4.Name = "cl4";
            this.cl4.ReadOnly = true;
            this.cl4.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // cl5
            // 
            this.cl5.HeaderText = "Param1";
            this.cl5.Name = "cl5";
            this.cl5.ReadOnly = true;
            this.cl5.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // cl6
            // 
            this.cl6.HeaderText = "Param2";
            this.cl6.Name = "cl6";
            this.cl6.ReadOnly = true;
            this.cl6.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // cl7
            // 
            this.cl7.HeaderText = "Param3";
            this.cl7.Name = "cl7";
            this.cl7.ReadOnly = true;
            this.cl7.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // cl8
            // 
            this.cl8.HeaderText = "Param4";
            this.cl8.Name = "cl8";
            this.cl8.ReadOnly = true;
            this.cl8.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.cl8.Width = 60;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copy,
            this.paste});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(143, 48);
            // 
            // copy
            // 
            this.copy.Name = "copy";
            this.copy.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.copy.Size = new System.Drawing.Size(142, 22);
            this.copy.Text = "Copy";
            this.copy.Click += new System.EventHandler(this.copy_Click);
            // 
            // paste
            // 
            this.paste.Name = "paste";
            this.paste.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
            this.paste.Size = new System.Drawing.Size(142, 22);
            this.paste.Text = "Paste";
            this.paste.Click += new System.EventHandler(this.paste_Click);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.LvTemplates);
            this.panel3.Controls.Add(this.label2);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(171, 360);
            this.panel3.TabIndex = 1;
            // 
            // LvTemplates
            // 
            this.LvTemplates.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.TemplateID,
            this.clName});
            this.LvTemplates.ContextMenuStrip = this.contextMenuStrip2;
            this.LvTemplates.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LvTemplates.FullRowSelect = true;
            this.LvTemplates.HideSelection = false;
            this.LvTemplates.Location = new System.Drawing.Point(0, 25);
            this.LvTemplates.Name = "LvTemplates";
            this.LvTemplates.Size = new System.Drawing.Size(171, 335);
            this.LvTemplates.TabIndex = 1;
            this.LvTemplates.UseCompatibleStateImageBehavior = false;
            this.LvTemplates.View = System.Windows.Forms.View.Details;
            this.LvTemplates.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.LvTemplates_ItemChecked);
            this.LvTemplates.MouseClick += new System.Windows.Forms.MouseEventHandler(this.LvTemplates_MouseClick);
            // 
            // TemplateID
            // 
            this.TemplateID.Text = "ID";
            this.TemplateID.Width = 32;
            // 
            // clName
            // 
            this.clName.Text = "Name";
            this.clName.Width = 160;
            // 
            // contextMenuStrip2
            // 
            this.contextMenuStrip2.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.contextMenuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tbDelTemplate,
            this.toolStripMenuItem1,
            this.addToExportListToolStripMenuItem});
            this.contextMenuStrip2.Name = "contextMenuStrip1";
            this.contextMenuStrip2.Size = new System.Drawing.Size(169, 54);
            // 
            // tbDelTemplate
            // 
            this.tbDelTemplate.Name = "tbDelTemplate";
            this.tbDelTemplate.Size = new System.Drawing.Size(168, 22);
            this.tbDelTemplate.Text = "Delete template";
            this.tbDelTemplate.Click += new System.EventHandler(this.tbDelTemplate_Click);
            // 
            // label2
            // 
            this.label2.Dock = System.Windows.Forms.DockStyle.Top;
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(171, 25);
            this.label2.TabIndex = 0;
            this.label2.Text = "Exised Templates";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(165, 6);
            this.toolStripMenuItem1.Visible = false;
            // 
            // addToExportListToolStripMenuItem
            // 
            this.addToExportListToolStripMenuItem.Name = "addToExportListToolStripMenuItem";
            this.addToExportListToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.addToExportListToolStripMenuItem.Text = "Add to export List";
            this.addToExportListToolStripMenuItem.Visible = false;
            this.addToExportListToolStripMenuItem.Click += new System.EventHandler(this.addToExportListToolStripMenuItem_Click);
            // 
            // frmCmdTemplates
            // 
            this.AllowDrop = true;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(879, 360);
            this.Controls.Add(this.gbDevA);
            this.Controls.Add(this.panel3);
            this.Font = new System.Drawing.Font("Calibri", 9F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmCmdTemplates";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Setup Button Commands";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmUpgrade_FormClosing);
            this.Load += new System.EventHandler(this.frmUpgrade_Load);
            this.Shown += new System.EventHandler(this.frmCmdSetup_Shown);
            this.gbDevA.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvListA)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.contextMenuStrip2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbDevA;
        private System.Windows.Forms.DataGridView dgvListA;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Button btnSure;
        private System.Windows.Forms.TextBox txtFrm;
        private System.Windows.Forms.Label lbTarget;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnSavetemplate;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem copy;
        private System.Windows.Forms.ToolStripMenuItem paste;
        private System.Windows.Forms.Label lbName;
        private System.Windows.Forms.ListView LvTemplates;
        private System.Windows.Forms.ColumnHeader TemplateID;
        private System.Windows.Forms.ColumnHeader clName;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip2;
        private System.Windows.Forms.ToolStripMenuItem tbDelTemplate;
        private System.Windows.Forms.Button btnUpdateA;
        private System.Windows.Forms.TextBox tbTemplates;
        private System.Windows.Forms.DataGridViewTextBoxColumn cl1;
        private System.Windows.Forms.DataGridViewTextBoxColumn cl2;
        private System.Windows.Forms.DataGridViewTextBoxColumn cl3;
        private System.Windows.Forms.DataGridViewTextBoxColumn cl4;
        private System.Windows.Forms.DataGridViewTextBoxColumn cl5;
        private System.Windows.Forms.DataGridViewTextBoxColumn cl6;
        private System.Windows.Forms.DataGridViewTextBoxColumn cl7;
        private System.Windows.Forms.DataGridViewTextBoxColumn cl8;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem addToExportListToolStripMenuItem;
    }
}