namespace HDL_Buspro_Setup_Tool
{
    partial class frmDimTemplates
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDimTemplates));
            this.gbDevA = new System.Windows.Forms.GroupBox();
            this.panel6 = new System.Windows.Forms.Panel();
            this.cboFunction = new System.Windows.Forms.ComboBox();
            this.gridValueTable = new System.Windows.Forms.DataGridView();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.tbTemplates = new System.Windows.Forms.TextBox();
            this.lbName = new System.Windows.Forms.Label();
            this.btnUpdateA = new System.Windows.Forms.Button();
            this.btnSure = new System.Windows.Forms.Button();
            this.lbTarget = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.tvTemplates = new System.Windows.Forms.TreeView();
            this.cm1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.Newtemplates = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.tbDelTemplate = new System.Windows.Forms.ToolStripMenuItem();
            this.label2 = new System.Windows.Forms.Label();
            this.clID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gbDevA.SuspendLayout();
            this.panel6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridValueTable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.panel3.SuspendLayout();
            this.cm1.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbDevA
            // 
            this.gbDevA.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.gbDevA.Controls.Add(this.panel6);
            this.gbDevA.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbDevA.Location = new System.Drawing.Point(213, 0);
            this.gbDevA.Name = "gbDevA";
            this.gbDevA.Size = new System.Drawing.Size(685, 407);
            this.gbDevA.TabIndex = 0;
            this.gbDevA.TabStop = false;
            this.gbDevA.Text = "Dimming Curve Setting";
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.cboFunction);
            this.panel6.Controls.Add(this.gridValueTable);
            this.panel6.Controls.Add(this.chart1);
            this.panel6.Controls.Add(this.tbTemplates);
            this.panel6.Controls.Add(this.lbName);
            this.panel6.Controls.Add(this.btnUpdateA);
            this.panel6.Controls.Add(this.btnSure);
            this.panel6.Controls.Add(this.lbTarget);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel6.Location = new System.Drawing.Point(3, 18);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(679, 386);
            this.panel6.TabIndex = 3;
            // 
            // cboFunction
            // 
            this.cboFunction.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboFunction.FormattingEnabled = true;
            this.cboFunction.Items.AddRange(new object[] {
            "y=x",
            "y=1.2x",
            "y=1.5x",
            "y=1.7x",
            "y=2.0x",
            "y=1/1.2x",
            "y=1/1.5x",
            "y=1/1.7x",
            "y=1/2.0x",
            "y=x^(1.2)",
            "y=x^(1.5)",
            "y=x^(1.7)",
            "y=x^(2.0)",
            "y=x^(1/1.2)",
            "y=x^(1/1.5)",
            "y=x^(1/1.7)",
            "y=x^(1/2.0)"});
            this.cboFunction.Location = new System.Drawing.Point(309, 7);
            this.cboFunction.Name = "cboFunction";
            this.cboFunction.Size = new System.Drawing.Size(162, 22);
            this.cboFunction.TabIndex = 150;
            this.cboFunction.SelectedIndexChanged += new System.EventHandler(this.cboFunction_SelectedIndexChanged);
            // 
            // gridValueTable
            // 
            this.gridValueTable.AllowUserToAddRows = false;
            this.gridValueTable.AllowUserToDeleteRows = false;
            this.gridValueTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridValueTable.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.clID,
            this.Column1});
            this.gridValueTable.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridValueTable.Location = new System.Drawing.Point(497, 44);
            this.gridValueTable.Name = "gridValueTable";
            this.gridValueTable.RowHeadersWidth = 10;
            this.gridValueTable.RowTemplate.Height = 23;
            this.gridValueTable.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridValueTable.Size = new System.Drawing.Size(172, 333);
            this.gridValueTable.TabIndex = 149;
            this.gridValueTable.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridValueTable_CellEndEdit);
            // 
            // chart1
            // 
            chartArea1.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea1);
            legend1.Alignment = System.Drawing.StringAlignment.Center;
            legend1.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Top;
            legend1.Name = "Legend1";
            this.chart1.Legends.Add(legend1);
            this.chart1.Location = new System.Drawing.Point(6, 44);
            this.chart1.Name = "chart1";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chart1.Series.Add(series1);
            this.chart1.Size = new System.Drawing.Size(501, 333);
            this.chart1.TabIndex = 148;
            this.chart1.Text = "chart1";
            // 
            // tbTemplates
            // 
            this.tbTemplates.Location = new System.Drawing.Point(79, 7);
            this.tbTemplates.Name = "tbTemplates";
            this.tbTemplates.Size = new System.Drawing.Size(162, 22);
            this.tbTemplates.TabIndex = 147;
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
            this.btnUpdateA.Location = new System.Drawing.Point(609, 5);
            this.btnUpdateA.Name = "btnUpdateA";
            this.btnUpdateA.Size = new System.Drawing.Size(60, 24);
            this.btnUpdateA.TabIndex = 141;
            this.btnUpdateA.UseVisualStyleBackColor = true;
            this.btnUpdateA.Click += new System.EventHandler(this.btnUpdateA_Click);
            // 
            // btnSure
            // 
            this.btnSure.Location = new System.Drawing.Point(514, 5);
            this.btnSure.Name = "btnSure";
            this.btnSure.Size = new System.Drawing.Size(60, 24);
            this.btnSure.TabIndex = 5;
            this.btnSure.Text = "+";
            this.btnSure.UseVisualStyleBackColor = true;
            this.btnSure.Click += new System.EventHandler(this.btnSure_Click);
            // 
            // lbTarget
            // 
            this.lbTarget.AutoSize = true;
            this.lbTarget.Location = new System.Drawing.Point(247, 10);
            this.lbTarget.Name = "lbTarget";
            this.lbTarget.Size = new System.Drawing.Size(56, 14);
            this.lbTarget.TabIndex = 0;
            this.lbTarget.Text = "Function:";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.tvTemplates);
            this.panel3.Controls.Add(this.label2);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(213, 407);
            this.panel3.TabIndex = 1;
            // 
            // tvTemplates
            // 
            this.tvTemplates.ContextMenuStrip = this.cm1;
            this.tvTemplates.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvTemplates.HideSelection = false;
            this.tvTemplates.Location = new System.Drawing.Point(0, 62);
            this.tvTemplates.Name = "tvTemplates";
            this.tvTemplates.Size = new System.Drawing.Size(213, 345);
            this.tvTemplates.TabIndex = 1;
            this.tvTemplates.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tvTemplates_MouseDown);
            // 
            // cm1
            // 
            this.cm1.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cm1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Newtemplates,
            this.toolStripMenuItem1,
            this.tbDelTemplate});
            this.cm1.Name = "contextMenuStrip1";
            this.cm1.Size = new System.Drawing.Size(138, 54);
            // 
            // Newtemplates
            // 
            this.Newtemplates.Name = "Newtemplates";
            this.Newtemplates.Size = new System.Drawing.Size(137, 22);
            this.Newtemplates.Text = "New Project";
            this.Newtemplates.Click += new System.EventHandler(this.Newtemplates_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(134, 6);
            // 
            // tbDelTemplate
            // 
            this.tbDelTemplate.Name = "tbDelTemplate";
            this.tbDelTemplate.Size = new System.Drawing.Size(137, 22);
            this.tbDelTemplate.Text = "Delete ";
            this.tbDelTemplate.Click += new System.EventHandler(this.tbDelTemplate_Click);
            // 
            // label2
            // 
            this.label2.Dock = System.Windows.Forms.DockStyle.Top;
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(213, 62);
            this.label2.TabIndex = 0;
            this.label2.Text = "Exised Templates";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // clID
            // 
            this.clID.HeaderText = "ID";
            this.clID.Name = "clID";
            this.clID.ReadOnly = true;
            this.clID.Width = 48;
            // 
            // Column1
            // 
            this.Column1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column1.HeaderText = "Output Value";
            this.Column1.MaxInputLength = 3;
            this.Column1.Name = "Column1";
            // 
            // frmDimTemplates
            // 
            this.AllowDrop = true;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(898, 407);
            this.Controls.Add(this.gbDevA);
            this.Controls.Add(this.panel3);
            this.Font = new System.Drawing.Font("Calibri", 9F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmDimTemplates";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Setup Button Commands";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmUpgrade_FormClosing);
            this.Load += new System.EventHandler(this.frmUpgrade_Load);
            this.Shown += new System.EventHandler(this.frmCmdSetup_Shown);
            this.gbDevA.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridValueTable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.panel3.ResumeLayout(false);
            this.cm1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbDevA;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Button btnSure;
        private System.Windows.Forms.Label lbTarget;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lbName;
        private System.Windows.Forms.ContextMenuStrip cm1;
        private System.Windows.Forms.ToolStripMenuItem tbDelTemplate;
        private System.Windows.Forms.Button btnUpdateA;
        private System.Windows.Forms.TextBox tbTemplates;
        private System.Windows.Forms.TreeView tvTemplates;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.DataGridView gridValueTable;
        private System.Windows.Forms.ToolStripMenuItem Newtemplates;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ComboBox cboFunction;
        private System.Windows.Forms.DataGridViewTextBoxColumn clID;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
    }
}