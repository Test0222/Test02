namespace HDL_Buspro_Setup_Tool
{
    partial class FrmBackup
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmBackup));
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnUpdateA = new System.Windows.Forms.Button();
            this.chbOption = new System.Windows.Forms.CheckBox();
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
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgOnline)).BeginInit();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnUpdateA);
            this.panel2.Controls.Add(this.chbOption);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 356);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(910, 39);
            this.panel2.TabIndex = 1;
            // 
            // btnUpdateA
            // 
            this.btnUpdateA.Location = new System.Drawing.Point(748, 7);
            this.btnUpdateA.Name = "btnUpdateA";
            this.btnUpdateA.Size = new System.Drawing.Size(150, 28);
            this.btnUpdateA.TabIndex = 144;
            this.btnUpdateA.Text = "Backups";
            this.btnUpdateA.UseVisualStyleBackColor = true;
            this.btnUpdateA.Click += new System.EventHandler(this.btnUpdateA_Click);
            // 
            // chbOption
            // 
            this.chbOption.AutoSize = true;
            this.chbOption.Location = new System.Drawing.Point(21, 12);
            this.chbOption.Name = "chbOption";
            this.chbOption.Size = new System.Drawing.Size(114, 18);
            this.chbOption.TabIndex = 143;
            this.chbOption.Text = "Select All/ None";
            this.chbOption.UseVisualStyleBackColor = true;
            this.chbOption.CheckedChanged += new System.EventHandler(this.chbOption_CheckedChanged);
            // 
            // dgOnline
            // 
            this.dgOnline.AllowDrop = true;
            this.dgOnline.AllowUserToAddRows = false;
            this.dgOnline.AllowUserToDeleteRows = false;
            this.dgOnline.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Calibri", 9F);
            this.dgOnline.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgOnline.BackgroundColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.AliceBlue;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgOnline.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
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
            this.dgOnline.Location = new System.Drawing.Point(0, 0);
            this.dgOnline.MultiSelect = false;
            this.dgOnline.Name = "dgOnline";
            this.dgOnline.RowHeadersVisible = false;
            this.dgOnline.RowHeadersWidth = 10;
            dataGridViewCellStyle12.Font = new System.Drawing.Font("Calibri", 9F);
            this.dgOnline.RowsDefaultCellStyle = dataGridViewCellStyle12;
            this.dgOnline.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Calibri", 9F);
            this.dgOnline.RowTemplate.Height = 23;
            this.dgOnline.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgOnline.Size = new System.Drawing.Size(910, 356);
            this.dgOnline.TabIndex = 49;
            this.dgOnline.CurrentCellDirtyStateChanged += new System.EventHandler(this.dgOnline_CurrentCellDirtyStateChanged);
            // 
            // clSelect
            // 
            this.clSelect.HeaderText = "";
            this.clSelect.Name = "clSelect";
            this.clSelect.Width = 32;
            // 
            // clEnable
            // 
            this.clEnable.HeaderText = "Session Visibility";
            this.clEnable.Name = "clEnable";
            this.clEnable.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.clEnable.Visible = false;
            // 
            // Column4
            // 
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Column4.DefaultCellStyle = dataGridViewCellStyle3;
            this.Column4.HeaderText = "ID";
            this.Column4.Name = "Column4";
            this.Column4.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column4.Width = 48;
            // 
            // clSubID
            // 
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Calibri", 9F);
            this.clSubID.DefaultCellStyle = dataGridViewCellStyle4;
            this.clSubID.HeaderText = "SubNet ID";
            this.clSubID.Name = "clSubID";
            // 
            // clDevID
            // 
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Calibri", 9F);
            this.clDevID.DefaultCellStyle = dataGridViewCellStyle5;
            this.clDevID.HeaderText = "Device ID";
            this.clDevID.Name = "clDevID";
            // 
            // clName
            // 
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Calibri", 9F);
            this.clName.DefaultCellStyle = dataGridViewCellStyle6;
            this.clName.HeaderText = "Model";
            this.clName.Name = "clName";
            this.clName.Width = 160;
            // 
            // Remark
            // 
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Calibri", 9F);
            this.Remark.DefaultCellStyle = dataGridViewCellStyle7;
            this.Remark.HeaderText = "Name";
            this.Remark.Name = "Remark";
            this.Remark.Width = 200;
            // 
            // clDesp
            // 
            this.clDesp.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Calibri", 9F);
            this.clDesp.DefaultCellStyle = dataGridViewCellStyle8;
            this.clDesp.HeaderText = "Description";
            this.clDesp.Name = "clDesp";
            this.clDesp.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Column14
            // 
            dataGridViewCellStyle9.Font = new System.Drawing.Font("Calibri", 9F);
            this.Column14.DefaultCellStyle = dataGridViewCellStyle9;
            this.Column14.HeaderText = "Version";
            this.Column14.Name = "Column14";
            this.Column14.Visible = false;
            this.Column14.Width = 80;
            // 
            // IndexID
            // 
            dataGridViewCellStyle10.Font = new System.Drawing.Font("Calibri", 9F);
            this.IndexID.DefaultCellStyle = dataGridViewCellStyle10;
            this.IndexID.HeaderText = "ID";
            this.IndexID.Name = "IndexID";
            this.IndexID.Visible = false;
            // 
            // cl10
            // 
            dataGridViewCellStyle11.Font = new System.Drawing.Font("Calibri", 9F);
            this.cl10.DefaultCellStyle = dataGridViewCellStyle11;
            this.cl10.HeaderText = "Backup status";
            this.cl10.Name = "cl10";
            this.cl10.Visible = false;
            this.cl10.Width = 150;
            // 
            // FrmBackup
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(910, 395);
            this.Controls.Add(this.dgOnline);
            this.Controls.Add(this.panel2);
            this.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmBackup";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Data Backups";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmRestrore_FormClosing);
            this.Load += new System.EventHandler(this.FrmRestrore_Load);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgOnline)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnUpdateA;
        private System.Windows.Forms.CheckBox chbOption;
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
    }
}