namespace HDL_Buspro_Setup_Tool
{
    partial class frmCmdSetup
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCmdSetup));
            this.gbDevA = new System.Windows.Forms.GroupBox();
            this.lbPage = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
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
            this.panel6 = new System.Windows.Forms.Panel();
            this.btnSavetemplate = new System.Windows.Forms.Button();
            this.btnUpdateA = new System.Windows.Forms.Button();
            this.btnSure = new System.Windows.Forms.Button();
            this.txtTo = new System.Windows.Forms.TextBox();
            this.txtFrm = new System.Windows.Forms.TextBox();
            this.lbTo = new System.Windows.Forms.Label();
            this.lbFrm = new System.Windows.Forms.Label();
            this.lbTarget = new System.Windows.Forms.Label();
            this.cboPages = new System.Windows.Forms.ComboBox();
            this.rbLongPressON = new System.Windows.Forms.RadioButton();
            this.rbShortOff = new System.Windows.Forms.RadioButton();
            this.cbKey = new System.Windows.Forms.ComboBox();
            this.lbKey = new System.Windows.Forms.Label();
            this.cboDevA = new System.Windows.Forms.ComboBox();
            this.lbDevA = new System.Windows.Forms.Label();
            this.lbRemote = new System.Windows.Forms.Label();
            this.cboRemote = new System.Windows.Forms.ComboBox();
            this.gbDevA.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvListA)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.panel6.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbDevA
            // 
            this.gbDevA.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.gbDevA.Controls.Add(this.lbPage);
            this.gbDevA.Controls.Add(this.panel2);
            this.gbDevA.Controls.Add(this.cboPages);
            this.gbDevA.Controls.Add(this.rbLongPressON);
            this.gbDevA.Controls.Add(this.rbShortOff);
            this.gbDevA.Controls.Add(this.cbKey);
            this.gbDevA.Controls.Add(this.lbKey);
            this.gbDevA.Controls.Add(this.cboDevA);
            this.gbDevA.Controls.Add(this.lbDevA);
            this.gbDevA.Controls.Add(this.lbRemote);
            this.gbDevA.Controls.Add(this.cboRemote);
            this.gbDevA.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbDevA.Location = new System.Drawing.Point(0, 0);
            this.gbDevA.Name = "gbDevA";
            this.gbDevA.Size = new System.Drawing.Size(836, 521);
            this.gbDevA.TabIndex = 0;
            this.gbDevA.TabStop = false;
            this.gbDevA.Text = "Select Device";
            // 
            // lbPage
            // 
            this.lbPage.AutoSize = true;
            this.lbPage.Location = new System.Drawing.Point(416, 60);
            this.lbPage.Name = "lbPage";
            this.lbPage.Size = new System.Drawing.Size(51, 14);
            this.lbPage.TabIndex = 146;
            this.lbPage.Text = "Page ID:";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.dgvListA);
            this.panel2.Controls.Add(this.panel6);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(3, 84);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(830, 434);
            this.panel2.TabIndex = 144;
            // 
            // dgvListA
            // 
            this.dgvListA.AllowDrop = true;
            this.dgvListA.AllowUserToAddRows = false;
            this.dgvListA.AllowUserToDeleteRows = false;
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
            this.dgvListA.Location = new System.Drawing.Point(0, 37);
            this.dgvListA.Name = "dgvListA";
            this.dgvListA.ReadOnly = true;
            this.dgvListA.RowHeadersVisible = false;
            this.dgvListA.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvListA.Size = new System.Drawing.Size(830, 397);
            this.dgvListA.TabIndex = 0;
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
            this.cl5.Width = 120;
            // 
            // cl6
            // 
            this.cl6.HeaderText = "Param2";
            this.cl6.Name = "cl6";
            this.cl6.ReadOnly = true;
            this.cl6.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.cl6.Width = 120;
            // 
            // cl7
            // 
            this.cl7.HeaderText = "Param3";
            this.cl7.Name = "cl7";
            this.cl7.ReadOnly = true;
            this.cl7.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.cl7.Width = 120;
            // 
            // cl8
            // 
            this.cl8.HeaderText = "Param4";
            this.cl8.Name = "cl8";
            this.cl8.ReadOnly = true;
            this.cl8.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copy,
            this.paste});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(153, 70);
            // 
            // copy
            // 
            this.copy.Name = "copy";
            this.copy.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.copy.Size = new System.Drawing.Size(152, 22);
            this.copy.Text = "Copy";
            this.copy.Click += new System.EventHandler(this.copy_Click);
            // 
            // paste
            // 
            this.paste.Name = "paste";
            this.paste.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
            this.paste.Size = new System.Drawing.Size(152, 22);
            this.paste.Text = "Paste";
            this.paste.Click += new System.EventHandler(this.paste_Click);
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.btnSavetemplate);
            this.panel6.Controls.Add(this.btnUpdateA);
            this.panel6.Controls.Add(this.btnSure);
            this.panel6.Controls.Add(this.txtTo);
            this.panel6.Controls.Add(this.txtFrm);
            this.panel6.Controls.Add(this.lbTo);
            this.panel6.Controls.Add(this.lbFrm);
            this.panel6.Controls.Add(this.lbTarget);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel6.Location = new System.Drawing.Point(0, 0);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(830, 434);
            this.panel6.TabIndex = 3;
            // 
            // btnSavetemplate
            // 
            this.btnSavetemplate.Location = new System.Drawing.Point(703, 6);
            this.btnSavetemplate.Name = "btnSavetemplate";
            this.btnSavetemplate.Size = new System.Drawing.Size(100, 24);
            this.btnSavetemplate.TabIndex = 4;
            this.btnSavetemplate.Text = "Save Templates";
            this.btnSavetemplate.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSavetemplate.UseVisualStyleBackColor = true;
            this.btnSavetemplate.Click += new System.EventHandler(this.btnSavetemplate_Click);
            // 
            // btnUpdateA
            // 
            this.btnUpdateA.Image = global::HDL_Buspro_Setup_Tool.Properties.Resources.Save;
            this.btnUpdateA.Location = new System.Drawing.Point(600, 6);
            this.btnUpdateA.Name = "btnUpdateA";
            this.btnUpdateA.Size = new System.Drawing.Size(100, 24);
            this.btnUpdateA.TabIndex = 1;
            this.btnUpdateA.UseVisualStyleBackColor = true;
            this.btnUpdateA.Click += new System.EventHandler(this.btnUpdateA_Click);
            // 
            // btnSure
            // 
            this.btnSure.Image = global::HDL_Buspro_Setup_Tool.Properties.Resources.Refresh;
            this.btnSure.Location = new System.Drawing.Point(497, 6);
            this.btnSure.Name = "btnSure";
            this.btnSure.Size = new System.Drawing.Size(100, 24);
            this.btnSure.TabIndex = 2;
            this.btnSure.UseVisualStyleBackColor = true;
            this.btnSure.Click += new System.EventHandler(this.btnSure_Click);
            // 
            // txtTo
            // 
            this.txtTo.Location = new System.Drawing.Point(416, 8);
            this.txtTo.Name = "txtTo";
            this.txtTo.Size = new System.Drawing.Size(75, 22);
            this.txtTo.TabIndex = 4;
            this.txtTo.Text = "1";
            // 
            // txtFrm
            // 
            this.txtFrm.Location = new System.Drawing.Point(266, 9);
            this.txtFrm.Name = "txtFrm";
            this.txtFrm.Size = new System.Drawing.Size(75, 22);
            this.txtFrm.TabIndex = 0;
            this.txtFrm.Text = "1";
            // 
            // lbTo
            // 
            this.lbTo.AutoSize = true;
            this.lbTo.Location = new System.Drawing.Point(364, 12);
            this.lbTo.Name = "lbTo";
            this.lbTo.Size = new System.Drawing.Size(19, 14);
            this.lbTo.TabIndex = 1;
            this.lbTo.Text = "To";
            // 
            // lbFrm
            // 
            this.lbFrm.AutoSize = true;
            this.lbFrm.Location = new System.Drawing.Point(164, 11);
            this.lbFrm.Name = "lbFrm";
            this.lbFrm.Size = new System.Drawing.Size(34, 14);
            this.lbFrm.TabIndex = 3;
            this.lbFrm.Text = "From";
            // 
            // lbTarget
            // 
            this.lbTarget.AutoSize = true;
            this.lbTarget.Location = new System.Drawing.Point(9, 11);
            this.lbTarget.Name = "lbTarget";
            this.lbTarget.Size = new System.Drawing.Size(121, 14);
            this.lbTarget.TabIndex = 1;
            this.lbTarget.Text = "Input Target Number:";
            // 
            // cboPages
            // 
            this.cboPages.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboPages.FormattingEnabled = true;
            this.cboPages.Items.AddRange(new object[] {
            "Page 1",
            "Page 2",
            "Page 3",
            "Page 4"});
            this.cboPages.Location = new System.Drawing.Point(501, 56);
            this.cboPages.Name = "cboPages";
            this.cboPages.Size = new System.Drawing.Size(99, 22);
            this.cboPages.TabIndex = 2;
            this.cboPages.SelectedIndexChanged += new System.EventHandler(this.cboPage_SelectedIndexChanged);
            // 
            // rbLongPressON
            // 
            this.rbLongPressON.AutoSize = true;
            this.rbLongPressON.Location = new System.Drawing.Point(603, 58);
            this.rbLongPressON.Name = "rbLongPressON";
            this.rbLongPressON.Size = new System.Drawing.Size(82, 18);
            this.rbLongPressON.TabIndex = 3;
            this.rbLongPressON.TabStop = true;
            this.rbLongPressON.Text = "Long Press";
            this.rbLongPressON.UseVisualStyleBackColor = true;
            this.rbLongPressON.CheckedChanged += new System.EventHandler(this.rbShort_CheckedChanged);
            // 
            // rbShortOff
            // 
            this.rbShortOff.AutoSize = true;
            this.rbShortOff.Location = new System.Drawing.Point(705, 58);
            this.rbShortOff.Name = "rbShortOff";
            this.rbShortOff.Size = new System.Drawing.Size(85, 18);
            this.rbShortOff.TabIndex = 4;
            this.rbShortOff.TabStop = true;
            this.rbShortOff.Text = "Short Press";
            this.rbShortOff.UseVisualStyleBackColor = true;
            this.rbShortOff.CheckedChanged += new System.EventHandler(this.rbShort_CheckedChanged);
            // 
            // cbKey
            // 
            this.cbKey.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbKey.FormattingEnabled = true;
            this.cbKey.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4"});
            this.cbKey.Location = new System.Drawing.Point(105, 56);
            this.cbKey.Name = "cbKey";
            this.cbKey.Size = new System.Drawing.Size(309, 22);
            this.cbKey.TabIndex = 1;
            this.cbKey.SelectedIndexChanged += new System.EventHandler(this.cbKey_SelectedIndexChanged);
            // 
            // lbKey
            // 
            this.lbKey.AutoSize = true;
            this.lbKey.Location = new System.Drawing.Point(12, 60);
            this.lbKey.Name = "lbKey";
            this.lbKey.Size = new System.Drawing.Size(46, 14);
            this.lbKey.TabIndex = 128;
            this.lbKey.Text = "Button:";
            // 
            // cboDevA
            // 
            this.cboDevA.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDevA.FormattingEnabled = true;
            this.cboDevA.Location = new System.Drawing.Point(105, 25);
            this.cboDevA.Name = "cboDevA";
            this.cboDevA.Size = new System.Drawing.Size(685, 22);
            this.cboDevA.TabIndex = 0;
            this.cboDevA.SelectedIndexChanged += new System.EventHandler(this.cboDevA_SelectedIndexChanged);
            // 
            // lbDevA
            // 
            this.lbDevA.AutoSize = true;
            this.lbDevA.ForeColor = System.Drawing.Color.Black;
            this.lbDevA.Location = new System.Drawing.Point(12, 28);
            this.lbDevA.Name = "lbDevA";
            this.lbDevA.Size = new System.Drawing.Size(82, 14);
            this.lbDevA.TabIndex = 124;
            this.lbDevA.Text = "Select Device:";
            // 
            // lbRemote
            // 
            this.lbRemote.AutoSize = true;
            this.lbRemote.Location = new System.Drawing.Point(606, 60);
            this.lbRemote.Name = "lbRemote";
            this.lbRemote.Size = new System.Drawing.Size(79, 14);
            this.lbRemote.TabIndex = 145;
            this.lbRemote.Text = "Controller ID:";
            // 
            // cboRemote
            // 
            this.cboRemote.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboRemote.FormattingEnabled = true;
            this.cboRemote.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4"});
            this.cboRemote.Location = new System.Drawing.Point(691, 56);
            this.cboRemote.Name = "cboRemote";
            this.cboRemote.Size = new System.Drawing.Size(99, 22);
            this.cboRemote.TabIndex = 135;
            this.cboRemote.SelectedIndexChanged += new System.EventHandler(this.cboRemote_SelectedIndexChanged);
            // 
            // frmCmdSetup
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(836, 521);
            this.Controls.Add(this.gbDevA);
            this.Font = new System.Drawing.Font("Calibri", 9F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmCmdSetup";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Setup Button Commands";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmUpgrade_FormClosing);
            this.Load += new System.EventHandler(this.frmUpgrade_Load);
            this.Shown += new System.EventHandler(this.frmCmdSetup_Shown);
            this.gbDevA.ResumeLayout(false);
            this.gbDevA.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvListA)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbDevA;
        private System.Windows.Forms.ComboBox cboDevA;
        private System.Windows.Forms.Label lbDevA;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnUpdateA;
        private System.Windows.Forms.ComboBox cboPages;
        private System.Windows.Forms.RadioButton rbLongPressON;
        private System.Windows.Forms.RadioButton rbShortOff;
        private System.Windows.Forms.ComboBox cbKey;
        private System.Windows.Forms.Label lbKey;
        private System.Windows.Forms.DataGridView dgvListA;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Button btnSure;
        private System.Windows.Forms.TextBox txtTo;
        private System.Windows.Forms.TextBox txtFrm;
        private System.Windows.Forms.Label lbTo;
        private System.Windows.Forms.Label lbFrm;
        private System.Windows.Forms.Label lbTarget;
        private System.Windows.Forms.ComboBox cboRemote;
        private System.Windows.Forms.Button btnSavetemplate;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem copy;
        private System.Windows.Forms.ToolStripMenuItem paste;
        private System.Windows.Forms.DataGridViewTextBoxColumn cl1;
        private System.Windows.Forms.DataGridViewTextBoxColumn cl2;
        private System.Windows.Forms.DataGridViewTextBoxColumn cl3;
        private System.Windows.Forms.DataGridViewTextBoxColumn cl4;
        private System.Windows.Forms.DataGridViewTextBoxColumn cl5;
        private System.Windows.Forms.DataGridViewTextBoxColumn cl6;
        private System.Windows.Forms.DataGridViewTextBoxColumn cl7;
        private System.Windows.Forms.DataGridViewTextBoxColumn cl8;
        private System.Windows.Forms.Label lbRemote;
        private System.Windows.Forms.Label lbPage;
    }
}