namespace HDL_Buspro_Setup_Tool
{
    partial class frmButtonSetup
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmButtonSetup));
            this.gbDevA = new System.Windows.Forms.GroupBox();
            this.cboController = new System.Windows.Forms.ComboBox();
            this.lbController = new System.Windows.Forms.Label();
            this.gbListA = new System.Windows.Forms.GroupBox();
            this.dgvListA = new System.Windows.Forms.DataGridView();
            this.panel2 = new System.Windows.Forms.Panel();
            this.Next = new System.Windows.Forms.Button();
            this.Pre = new System.Windows.Forms.Button();
            this.lbPage = new System.Windows.Forms.Label();
            this.cboPages = new System.Windows.Forms.ComboBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.RefreshDisplay = new System.Windows.Forms.Button();
            this.btnUpdateA = new System.Windows.Forms.Button();
            this.cboDevA = new System.Windows.Forms.ComboBox();
            this.lbDevA = new System.Windows.Forms.Label();
            this.clID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clRemark = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Mode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clLed = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.ClDim = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.ClDimValue = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.clMutux = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.clLink = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.DryDim = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.gbDevA.SuspendLayout();
            this.gbListA.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvListA)).BeginInit();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbDevA
            // 
            this.gbDevA.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.gbDevA.Controls.Add(this.cboController);
            this.gbDevA.Controls.Add(this.lbController);
            this.gbDevA.Controls.Add(this.gbListA);
            this.gbDevA.Controls.Add(this.cboDevA);
            this.gbDevA.Controls.Add(this.lbDevA);
            this.gbDevA.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbDevA.Location = new System.Drawing.Point(0, 0);
            this.gbDevA.Name = "gbDevA";
            this.gbDevA.Size = new System.Drawing.Size(709, 521);
            this.gbDevA.TabIndex = 0;
            this.gbDevA.TabStop = false;
            this.gbDevA.Text = "Select Device";
            // 
            // cboController
            // 
            this.cboController.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboController.FormattingEnabled = true;
            this.cboController.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4"});
            this.cboController.Location = new System.Drawing.Point(434, 23);
            this.cboController.Name = "cboController";
            this.cboController.Size = new System.Drawing.Size(74, 22);
            this.cboController.TabIndex = 136;
            this.cboController.SelectedIndexChanged += new System.EventHandler(this.cboController_SelectedIndexChanged);
            // 
            // lbController
            // 
            this.lbController.AutoSize = true;
            this.lbController.Font = new System.Drawing.Font("Calibri", 9F);
            this.lbController.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lbController.Location = new System.Drawing.Point(355, 29);
            this.lbController.Name = "lbController";
            this.lbController.Size = new System.Drawing.Size(73, 14);
            this.lbController.TabIndex = 134;
            this.lbController.Text = "Controller 1:";
            // 
            // gbListA
            // 
            this.gbListA.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.gbListA.Controls.Add(this.dgvListA);
            this.gbListA.Controls.Add(this.panel2);
            this.gbListA.Controls.Add(this.panel1);
            this.gbListA.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.gbListA.Location = new System.Drawing.Point(3, 54);
            this.gbListA.Name = "gbListA";
            this.gbListA.Size = new System.Drawing.Size(703, 464);
            this.gbListA.TabIndex = 127;
            this.gbListA.TabStop = false;
            this.gbListA.Text = "Button Setup";
            // 
            // dgvListA
            // 
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
            this.clID,
            this.clRemark,
            this.Mode,
            this.clLed,
            this.ClDim,
            this.ClDimValue,
            this.clMutux,
            this.clLink,
            this.DryDim});
            this.dgvListA.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvListA.EnableHeadersVisualStyles = false;
            this.dgvListA.Location = new System.Drawing.Point(3, 50);
            this.dgvListA.Name = "dgvListA";
            this.dgvListA.RowHeadersVisible = false;
            this.dgvListA.RowHeadersWidth = 10;
            this.dgvListA.RowTemplate.Height = 23;
            this.dgvListA.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvListA.Size = new System.Drawing.Size(697, 371);
            this.dgvListA.TabIndex = 139;
            this.dgvListA.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dgvListA_CellBeginEdit);
            this.dgvListA.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvListA_CellValueChanged);
            this.dgvListA.CurrentCellDirtyStateChanged += new System.EventHandler(this.dgvListA_CurrentCellDirtyStateChanged);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.Next);
            this.panel2.Controls.Add(this.Pre);
            this.panel2.Controls.Add(this.lbPage);
            this.panel2.Controls.Add(this.cboPages);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(3, 18);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(697, 32);
            this.panel2.TabIndex = 144;
            // 
            // Next
            // 
            this.Next.Location = new System.Drawing.Point(314, 5);
            this.Next.Name = "Next";
            this.Next.Size = new System.Drawing.Size(20, 23);
            this.Next.TabIndex = 139;
            this.Next.Text = ">";
            this.Next.UseVisualStyleBackColor = true;
            this.Next.Click += new System.EventHandler(this.Next_Click);
            // 
            // Pre
            // 
            this.Pre.Location = new System.Drawing.Point(120, 5);
            this.Pre.Name = "Pre";
            this.Pre.Size = new System.Drawing.Size(20, 23);
            this.Pre.TabIndex = 138;
            this.Pre.Text = "<";
            this.Pre.UseVisualStyleBackColor = true;
            this.Pre.Click += new System.EventHandler(this.Pre_Click);
            // 
            // lbPage
            // 
            this.lbPage.AutoSize = true;
            this.lbPage.Font = new System.Drawing.Font("Calibri", 9F);
            this.lbPage.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lbPage.Location = new System.Drawing.Point(16, 9);
            this.lbPage.Name = "lbPage";
            this.lbPage.Size = new System.Drawing.Size(36, 14);
            this.lbPage.TabIndex = 135;
            this.lbPage.Text = "Page:";
            // 
            // cboPages
            // 
            this.cboPages.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboPages.FormattingEnabled = true;
            this.cboPages.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6"});
            this.cboPages.Location = new System.Drawing.Point(146, 5);
            this.cboPages.Name = "cboPages";
            this.cboPages.Size = new System.Drawing.Size(162, 22);
            this.cboPages.TabIndex = 137;
            this.cboPages.SelectedIndexChanged += new System.EventHandler(this.cboPages_SelectedIndexChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.RefreshDisplay);
            this.panel1.Controls.Add(this.btnUpdateA);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(3, 421);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(697, 40);
            this.panel1.TabIndex = 143;
            // 
            // RefreshDisplay
            // 
            this.RefreshDisplay.Image = global::HDL_Buspro_Setup_Tool.Properties.Resources.Refresh;
            this.RefreshDisplay.Location = new System.Drawing.Point(339, 6);
            this.RefreshDisplay.Name = "RefreshDisplay";
            this.RefreshDisplay.Size = new System.Drawing.Size(120, 24);
            this.RefreshDisplay.TabIndex = 143;
            this.RefreshDisplay.UseVisualStyleBackColor = true;
            this.RefreshDisplay.Click += new System.EventHandler(this.RefreshDisplay_Click);
            // 
            // btnUpdateA
            // 
            this.btnUpdateA.Image = global::HDL_Buspro_Setup_Tool.Properties.Resources.Save;
            this.btnUpdateA.Location = new System.Drawing.Point(494, 6);
            this.btnUpdateA.Name = "btnUpdateA";
            this.btnUpdateA.Size = new System.Drawing.Size(120, 24);
            this.btnUpdateA.TabIndex = 141;
            this.btnUpdateA.UseVisualStyleBackColor = true;
            this.btnUpdateA.Click += new System.EventHandler(this.btnUpdateA_Click);
            // 
            // cboDevA
            // 
            this.cboDevA.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDevA.FormattingEnabled = true;
            this.cboDevA.Location = new System.Drawing.Point(126, 25);
            this.cboDevA.Name = "cboDevA";
            this.cboDevA.Size = new System.Drawing.Size(214, 22);
            this.cboDevA.TabIndex = 125;
            this.cboDevA.SelectedIndexChanged += new System.EventHandler(this.cboDevA_SelectedIndexChanged);
            // 
            // lbDevA
            // 
            this.lbDevA.AutoSize = true;
            this.lbDevA.ForeColor = System.Drawing.Color.Black;
            this.lbDevA.Location = new System.Drawing.Point(8, 26);
            this.lbDevA.Name = "lbDevA";
            this.lbDevA.Size = new System.Drawing.Size(82, 14);
            this.lbDevA.TabIndex = 124;
            this.lbDevA.Text = "Select Device:";
            // 
            // clID
            // 
            this.clID.HeaderText = "ID";
            this.clID.Name = "clID";
            this.clID.ReadOnly = true;
            this.clID.Width = 36;
            // 
            // clRemark
            // 
            this.clRemark.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.clRemark.HeaderText = "Remark";
            this.clRemark.Name = "clRemark";
            this.clRemark.ReadOnly = true;
            // 
            // Mode
            // 
            this.Mode.HeaderText = "Mode";
            this.Mode.Name = "Mode";
            this.Mode.ReadOnly = true;
            // 
            // clLed
            // 
            this.clLed.HeaderText = "Status LED";
            this.clLed.Name = "clLed";
            this.clLed.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.clLed.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // ClDim
            // 
            this.ClDim.HeaderText = "Dimable";
            this.ClDim.Name = "ClDim";
            this.ClDim.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ClDim.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // ClDimValue
            // 
            this.ClDimValue.HeaderText = "Save Dim Value";
            this.ClDimValue.Name = "ClDimValue";
            this.ClDimValue.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ClDimValue.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // clMutux
            // 
            this.clMutux.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.ComboBox;
            this.clMutux.HeaderText = "Exclusive";
            this.clMutux.Items.AddRange(new object[] {
            "NO",
            "YES"});
            this.clMutux.Name = "clMutux";
            this.clMutux.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.clMutux.Width = 80;
            // 
            // clLink
            // 
            this.clLink.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.ComboBox;
            this.clLink.HeaderText = "Linkage";
            this.clLink.Items.AddRange(new object[] {
            "N/A",
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8"});
            this.clLink.Name = "clLink";
            this.clLink.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.clLink.Width = 80;
            // 
            // DryDim
            // 
            this.DryDim.HeaderText = "Dimable";
            this.DryDim.Items.AddRange(new object[] {
            "Switch mode",
            "Dimmable two-way",
            "Dimmable increase",
            "Dimmable reduced"});
            this.DryDim.Name = "DryDim";
            this.DryDim.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.DryDim.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.DryDim.Width = 160;
            // 
            // frmButtonSetup
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(709, 521);
            this.Controls.Add(this.gbDevA);
            this.Font = new System.Drawing.Font("Calibri", 9F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmButtonSetup";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Button detail information";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmUpgrade_FormClosing);
            this.Load += new System.EventHandler(this.frmUpgrade_Load);
            this.Shown += new System.EventHandler(this.frmButtonSetup_Shown);
            this.gbDevA.ResumeLayout(false);
            this.gbDevA.PerformLayout();
            this.gbListA.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvListA)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbDevA;
        private System.Windows.Forms.ComboBox cboDevA;
        private System.Windows.Forms.Label lbDevA;
        private System.Windows.Forms.GroupBox gbListA;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.DataGridView dgvListA;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnUpdateA;
        private System.Windows.Forms.Button RefreshDisplay;
        private System.Windows.Forms.ComboBox cboController;
        private System.Windows.Forms.Label lbController;
        private System.Windows.Forms.Button Next;
        private System.Windows.Forms.Button Pre;
        private System.Windows.Forms.Label lbPage;
        private System.Windows.Forms.ComboBox cboPages;
        private System.Windows.Forms.DataGridViewTextBoxColumn clID;
        private System.Windows.Forms.DataGridViewTextBoxColumn clRemark;
        private System.Windows.Forms.DataGridViewTextBoxColumn Mode;
        private System.Windows.Forms.DataGridViewCheckBoxColumn clLed;
        private System.Windows.Forms.DataGridViewCheckBoxColumn ClDim;
        private System.Windows.Forms.DataGridViewCheckBoxColumn ClDimValue;
        private System.Windows.Forms.DataGridViewComboBoxColumn clMutux;
        private System.Windows.Forms.DataGridViewComboBoxColumn clLink;
        private System.Windows.Forms.DataGridViewComboBoxColumn DryDim;
    }
}