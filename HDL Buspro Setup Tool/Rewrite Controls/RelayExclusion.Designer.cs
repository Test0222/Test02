namespace HDL_Buspro_Setup_Tool
{
    partial class RelayExclusion
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RelayExclusion));
            this.panel1 = new System.Windows.Forms.Panel();
            this.DgvChns = new System.Windows.Forms.DataGridView();
            this.cl1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cl2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cl3 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.cl4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cl5 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.chbBoxList = new System.Windows.Forms.CheckedListBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.lbHint = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DgvChns)).BeginInit();
            this.panel2.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.DgvChns);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(587, 295);
            this.panel1.TabIndex = 0;
            // 
            // DgvChns
            // 
            this.DgvChns.AllowUserToAddRows = false;
            this.DgvChns.AllowUserToDeleteRows = false;
            this.DgvChns.AllowUserToResizeRows = false;
            this.DgvChns.BackgroundColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.AliceBlue;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.DgvChns.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.DgvChns.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DgvChns.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.cl1,
            this.cl2,
            this.cl3,
            this.cl4,
            this.cl5});
            this.DgvChns.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DgvChns.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.DgvChns.EnableHeadersVisualStyles = false;
            this.DgvChns.Location = new System.Drawing.Point(0, 0);
            this.DgvChns.Name = "DgvChns";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.DgvChns.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.DgvChns.RowHeadersVisible = false;
            this.DgvChns.RowHeadersWidth = 10;
            this.DgvChns.RowTemplate.Height = 23;
            this.DgvChns.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.DgvChns.Size = new System.Drawing.Size(587, 295);
            this.DgvChns.TabIndex = 10;
            this.DgvChns.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.DgChns_CellValueChanged);
            this.DgvChns.CurrentCellDirtyStateChanged += new System.EventHandler(this.DgChns_CurrentCellDirtyStateChanged);
            this.DgvChns.SizeChanged += new System.EventHandler(this.DgvChns_SizeChanged);
            // 
            // cl1
            // 
            this.cl1.HeaderText = "Chn No.";
            this.cl1.Name = "cl1";
            this.cl1.ReadOnly = true;
            // 
            // cl2
            // 
            dataGridViewCellStyle2.NullValue = "\"\"";
            this.cl2.DefaultCellStyle = dataGridViewCellStyle2;
            this.cl2.HeaderText = "Name";
            this.cl2.MaxInputLength = 20;
            this.cl2.Name = "cl2";
            this.cl2.Width = 180;
            // 
            // cl3
            // 
            this.cl3.HeaderText = "Stair light";
            this.cl3.Name = "cl3";
            this.cl3.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.cl3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // cl4
            // 
            this.cl4.HeaderText = "Auto Close(S)";
            this.cl4.Name = "cl4";
            this.cl4.Width = 120;
            // 
            // cl5
            // 
            this.cl5.HeaderText = "Test";
            this.cl5.Name = "cl5";
            this.cl5.Width = 60;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.panel4);
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 295);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(587, 176);
            this.panel2.TabIndex = 1;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.chbBoxList);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(0, 55);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(587, 121);
            this.panel4.TabIndex = 12;
            // 
            // chbBoxList
            // 
            this.chbBoxList.ColumnWidth = 300;
            this.chbBoxList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chbBoxList.FormattingEnabled = true;
            this.chbBoxList.Location = new System.Drawing.Point(0, 0);
            this.chbBoxList.MultiColumn = true;
            this.chbBoxList.Name = "chbBoxList";
            this.chbBoxList.Size = new System.Drawing.Size(587, 121);
            this.chbBoxList.TabIndex = 0;
            this.chbBoxList.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.chbBoxList_ItemCheck);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.lbHint);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(587, 55);
            this.panel3.TabIndex = 11;
            // 
            // lbHint
            // 
            this.lbHint.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbHint.ForeColor = System.Drawing.Color.Blue;
            this.lbHint.Location = new System.Drawing.Point(0, 0);
            this.lbHint.Name = "lbHint";
            this.lbHint.Size = new System.Drawing.Size(587, 55);
            this.lbHint.TabIndex = 0;
            this.lbHint.Text = resources.GetString("lbHint.Text");
            // 
            // RelayExclusion
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "RelayExclusion";
            this.Size = new System.Drawing.Size(587, 471);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.DgvChns)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.DataGridView DgvChns;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.CheckedListBox chbBoxList;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label lbHint;
        private System.Windows.Forms.DataGridViewTextBoxColumn cl1;
        private System.Windows.Forms.DataGridViewTextBoxColumn cl2;
        private System.Windows.Forms.DataGridViewCheckBoxColumn cl3;
        private System.Windows.Forms.DataGridViewTextBoxColumn cl4;
        private System.Windows.Forms.DataGridViewCheckBoxColumn cl5;
    }
}
