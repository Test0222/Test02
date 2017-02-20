namespace HDL_Buspro_Setup_Tool
{
    partial class frmLogicPin
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
            this.btnRefresh = new System.Windows.Forms.Button();
            this.lvSensors = new System.Windows.Forms.ListView();
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lvID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lvDecription = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.cm1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addNewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tp3 = new System.Windows.Forms.TableLayoutPanel();
            this.RW2 = new System.Windows.Forms.RadioButton();
            this.RW1 = new System.Windows.Forms.RadioButton();
            this.cboW1 = new System.Windows.Forms.ComboBox();
            this.cboW2 = new System.Windows.Forms.ComboBox();
            this.cboW21 = new System.Windows.Forms.ComboBox();
            this.tp1 = new System.Windows.Forms.TableLayoutPanel();
            this.numY4 = new System.Windows.Forms.DateTimePicker();
            this.rbY4 = new System.Windows.Forms.RadioButton();
            this.numY3 = new System.Windows.Forms.NumericUpDown();
            this.rbY3 = new System.Windows.Forms.RadioButton();
            this.rbY2 = new System.Windows.Forms.RadioButton();
            this.rbY1 = new System.Windows.Forms.RadioButton();
            this.numY1 = new System.Windows.Forms.NumericUpDown();
            this.numY2 = new System.Windows.Forms.DateTimePicker();
            this.numY41 = new System.Windows.Forms.DateTimePicker();
            this.numY31 = new System.Windows.Forms.NumericUpDown();
            this.tp2 = new System.Windows.Forms.TableLayoutPanel();
            this.rbD3 = new System.Windows.Forms.RadioButton();
            this.rbD2 = new System.Windows.Forms.RadioButton();
            this.rbD1 = new System.Windows.Forms.RadioButton();
            this.tp4 = new System.Windows.Forms.TableLayoutPanel();
            this.numT2 = new HDL_Buspro_Setup_Tool.TimeText();
            this.numT21 = new HDL_Buspro_Setup_Tool.TimeText();
            this.numT1 = new System.Windows.Forms.ComboBox();
            this.numT11 = new HDL_Buspro_Setup_Tool.TimeText();
            this.rbT1 = new System.Windows.Forms.RadioButton();
            this.rbT2 = new System.Windows.Forms.RadioButton();
            this.cboType = new System.Windows.Forms.ComboBox();
            this.lbRule = new System.Windows.Forms.Label();
            this.chbAuto = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.tp5 = new System.Windows.Forms.TableLayoutPanel();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cboUV1 = new System.Windows.Forms.ComboBox();
            this.numUV1 = new System.Windows.Forms.NumericUpDown();
            this.tp6 = new System.Windows.Forms.TableLayoutPanel();
            this.label7 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cboDev1 = new System.Windows.Forms.ComboBox();
            this.cboDev2 = new System.Windows.Forms.NumericUpDown();
            this.cboDev3 = new System.Windows.Forms.ComboBox();
            this.cboDev31 = new System.Windows.Forms.ComboBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.cm1.SuspendLayout();
            this.tp3.SuspendLayout();
            this.tp1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numY3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numY1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numY31)).BeginInit();
            this.tp2.SuspendLayout();
            this.tp4.SuspendLayout();
            this.tp5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numUV1)).BeginInit();
            this.tp6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cboDev2)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnRefresh
            // 
            this.btnRefresh.Image = global::HDL_Buspro_Setup_Tool.Properties.Resources.Refresh;
            this.btnRefresh.Location = new System.Drawing.Point(205, 333);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(80, 26);
            this.btnRefresh.TabIndex = 20;
            this.btnRefresh.Tag = "0";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // lvSensors
            // 
            this.lvSensors.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader3,
            this.lvID,
            this.lvDecription});
            this.lvSensors.ContextMenuStrip = this.cm1;
            this.lvSensors.FullRowSelect = true;
            this.lvSensors.HideSelection = false;
            this.lvSensors.Location = new System.Drawing.Point(12, 13);
            this.lvSensors.Name = "lvSensors";
            this.lvSensors.Size = new System.Drawing.Size(258, 388);
            this.lvSensors.TabIndex = 103;
            this.lvSensors.UseCompatibleStateImageBehavior = false;
            this.lvSensors.View = System.Windows.Forms.View.Details;
            this.lvSensors.SelectedIndexChanged += new System.EventHandler(this.lvSensors_SelectedIndexChanged);
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "";
            this.columnHeader3.Width = 10;
            // 
            // lvID
            // 
            this.lvID.Text = "ID";
            this.lvID.Width = 36;
            // 
            // lvDecription
            // 
            this.lvDecription.Text = "Description";
            this.lvDecription.Width = 200;
            // 
            // cm1
            // 
            this.cm1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addNewToolStripMenuItem,
            this.toolStripMenuItem1,
            this.deleteToolStripMenuItem});
            this.cm1.Name = "cm1";
            this.cm1.Size = new System.Drawing.Size(141, 54);
            // 
            // addNewToolStripMenuItem
            // 
            this.addNewToolStripMenuItem.Name = "addNewToolStripMenuItem";
            this.addNewToolStripMenuItem.Size = new System.Drawing.Size(140, 22);
            this.addNewToolStripMenuItem.Text = "Add New";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(137, 6);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Delete;
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(140, 22);
            this.deleteToolStripMenuItem.Text = "Delete";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // tp3
            // 
            this.tp3.ColumnCount = 3;
            this.tp3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 61.97183F));
            this.tp3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 126F));
            this.tp3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 129F));
            this.tp3.Controls.Add(this.RW2, 0, 1);
            this.tp3.Controls.Add(this.RW1, 0, 0);
            this.tp3.Controls.Add(this.cboW1, 1, 0);
            this.tp3.Controls.Add(this.cboW2, 1, 1);
            this.tp3.Controls.Add(this.cboW21, 2, 1);
            this.tp3.Location = new System.Drawing.Point(13, 69);
            this.tp3.Name = "tp3";
            this.tp3.RowCount = 2;
            this.tp3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 24.81752F));
            this.tp3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tp3.Size = new System.Drawing.Size(355, 55);
            this.tp3.TabIndex = 30;
            this.tp3.Visible = false;
            // 
            // RW2
            // 
            this.RW2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RW2.Location = new System.Drawing.Point(3, 31);
            this.RW2.Name = "RW2";
            this.RW2.Size = new System.Drawing.Size(94, 21);
            this.RW2.TabIndex = 6;
            this.RW2.TabStop = true;
            this.RW2.Tag = "1";
            this.RW2.Text = "WeekDays";
            this.RW2.UseVisualStyleBackColor = true;
            this.RW2.CheckedChanged += new System.EventHandler(this.RW1_CheckedChanged);
            // 
            // RW1
            // 
            this.RW1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RW1.Location = new System.Drawing.Point(3, 3);
            this.RW1.Name = "RW1";
            this.RW1.Size = new System.Drawing.Size(94, 22);
            this.RW1.TabIndex = 5;
            this.RW1.TabStop = true;
            this.RW1.Tag = "0";
            this.RW1.Text = "WeekDay";
            this.RW1.UseVisualStyleBackColor = true;
            this.RW1.CheckedChanged += new System.EventHandler(this.RW1_CheckedChanged);
            // 
            // cboW1
            // 
            this.cboW1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboW1.Enabled = false;
            this.cboW1.FormattingEnabled = true;
            this.cboW1.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4"});
            this.cboW1.Location = new System.Drawing.Point(103, 3);
            this.cboW1.Name = "cboW1";
            this.cboW1.Size = new System.Drawing.Size(120, 22);
            this.cboW1.TabIndex = 29;
            // 
            // cboW2
            // 
            this.cboW2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboW2.Enabled = false;
            this.cboW2.FormattingEnabled = true;
            this.cboW2.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4"});
            this.cboW2.Location = new System.Drawing.Point(103, 31);
            this.cboW2.Name = "cboW2";
            this.cboW2.Size = new System.Drawing.Size(120, 22);
            this.cboW2.TabIndex = 31;
            // 
            // cboW21
            // 
            this.cboW21.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboW21.Enabled = false;
            this.cboW21.FormattingEnabled = true;
            this.cboW21.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4"});
            this.cboW21.Location = new System.Drawing.Point(229, 31);
            this.cboW21.Name = "cboW21";
            this.cboW21.Size = new System.Drawing.Size(123, 22);
            this.cboW21.TabIndex = 30;
            // 
            // tp1
            // 
            this.tp1.ColumnCount = 3;
            this.tp1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 27.27273F));
            this.tp1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 37.61194F));
            this.tp1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35.22388F));
            this.tp1.Controls.Add(this.numY4, 1, 3);
            this.tp1.Controls.Add(this.rbY4, 0, 3);
            this.tp1.Controls.Add(this.numY3, 1, 2);
            this.tp1.Controls.Add(this.rbY3, 0, 2);
            this.tp1.Controls.Add(this.rbY2, 0, 1);
            this.tp1.Controls.Add(this.rbY1, 0, 0);
            this.tp1.Controls.Add(this.numY1, 1, 0);
            this.tp1.Controls.Add(this.numY2, 1, 1);
            this.tp1.Controls.Add(this.numY41, 2, 3);
            this.tp1.Controls.Add(this.numY31, 2, 2);
            this.tp1.Location = new System.Drawing.Point(13, 43);
            this.tp1.Name = "tp1";
            this.tp1.RowCount = 4;
            this.tp1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 24.81752F));
            this.tp1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 24.81752F));
            this.tp1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.18248F));
            this.tp1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.18248F));
            this.tp1.Size = new System.Drawing.Size(355, 105);
            this.tp1.TabIndex = 22;
            this.tp1.Visible = false;
            // 
            // numY4
            // 
            this.numY4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.numY4.Enabled = false;
            this.numY4.Location = new System.Drawing.Point(99, 81);
            this.numY4.Name = "numY4";
            this.numY4.Size = new System.Drawing.Size(127, 22);
            this.numY4.TabIndex = 7;
            // 
            // rbY4
            // 
            this.rbY4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rbY4.Location = new System.Drawing.Point(3, 81);
            this.rbY4.Name = "rbY4";
            this.rbY4.Size = new System.Drawing.Size(90, 21);
            this.rbY4.TabIndex = 6;
            this.rbY4.TabStop = true;
            this.rbY4.Tag = "3";
            this.rbY4.Text = "From Date to";
            this.rbY4.UseVisualStyleBackColor = true;
            this.rbY4.CheckedChanged += new System.EventHandler(this.rbY1_CheckedChanged);
            // 
            // numY3
            // 
            this.numY3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.numY3.Enabled = false;
            this.numY3.Location = new System.Drawing.Point(99, 55);
            this.numY3.Maximum = new decimal(new int[] {
            2500,
            0,
            0,
            0});
            this.numY3.Minimum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.numY3.Name = "numY3";
            this.numY3.Size = new System.Drawing.Size(127, 22);
            this.numY3.TabIndex = 5;
            this.numY3.Value = new decimal(new int[] {
            2014,
            0,
            0,
            0});
            // 
            // rbY3
            // 
            this.rbY3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rbY3.Location = new System.Drawing.Point(3, 55);
            this.rbY3.Name = "rbY3";
            this.rbY3.Size = new System.Drawing.Size(90, 20);
            this.rbY3.TabIndex = 4;
            this.rbY3.TabStop = true;
            this.rbY3.Tag = "2";
            this.rbY3.Text = "From Year to";
            this.rbY3.UseVisualStyleBackColor = true;
            this.rbY3.CheckedChanged += new System.EventHandler(this.rbY1_CheckedChanged);
            // 
            // rbY2
            // 
            this.rbY2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rbY2.Location = new System.Drawing.Point(3, 29);
            this.rbY2.Name = "rbY2";
            this.rbY2.Size = new System.Drawing.Size(90, 20);
            this.rbY2.TabIndex = 2;
            this.rbY2.TabStop = true;
            this.rbY2.Tag = "1";
            this.rbY2.Text = "Fix Date";
            this.rbY2.UseVisualStyleBackColor = true;
            this.rbY2.CheckedChanged += new System.EventHandler(this.rbY1_CheckedChanged);
            // 
            // rbY1
            // 
            this.rbY1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rbY1.Location = new System.Drawing.Point(3, 3);
            this.rbY1.Name = "rbY1";
            this.rbY1.Size = new System.Drawing.Size(90, 20);
            this.rbY1.TabIndex = 0;
            this.rbY1.TabStop = true;
            this.rbY1.Tag = "0";
            this.rbY1.Text = "Fix Year";
            this.rbY1.UseVisualStyleBackColor = true;
            this.rbY1.CheckedChanged += new System.EventHandler(this.rbY1_CheckedChanged);
            // 
            // numY1
            // 
            this.numY1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.numY1.Enabled = false;
            this.numY1.Location = new System.Drawing.Point(99, 3);
            this.numY1.Maximum = new decimal(new int[] {
            2500,
            0,
            0,
            0});
            this.numY1.Minimum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.numY1.Name = "numY1";
            this.numY1.Size = new System.Drawing.Size(127, 22);
            this.numY1.TabIndex = 1;
            this.numY1.Value = new decimal(new int[] {
            2014,
            0,
            0,
            0});
            // 
            // numY2
            // 
            this.numY2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.numY2.Enabled = false;
            this.numY2.Location = new System.Drawing.Point(99, 29);
            this.numY2.Name = "numY2";
            this.numY2.Size = new System.Drawing.Size(127, 22);
            this.numY2.TabIndex = 3;
            // 
            // numY41
            // 
            this.numY41.Dock = System.Windows.Forms.DockStyle.Fill;
            this.numY41.Enabled = false;
            this.numY41.Location = new System.Drawing.Point(232, 81);
            this.numY41.Name = "numY41";
            this.numY41.Size = new System.Drawing.Size(120, 22);
            this.numY41.TabIndex = 8;
            // 
            // numY31
            // 
            this.numY31.Dock = System.Windows.Forms.DockStyle.Fill;
            this.numY31.Enabled = false;
            this.numY31.Location = new System.Drawing.Point(232, 55);
            this.numY31.Maximum = new decimal(new int[] {
            2500,
            0,
            0,
            0});
            this.numY31.Minimum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.numY31.Name = "numY31";
            this.numY31.Size = new System.Drawing.Size(120, 22);
            this.numY31.TabIndex = 9;
            this.numY31.Value = new decimal(new int[] {
            2014,
            0,
            0,
            0});
            // 
            // tp2
            // 
            this.tp2.ColumnCount = 3;
            this.tp2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 27.27273F));
            this.tp2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 37.61194F));
            this.tp2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35.22388F));
            this.tp2.Controls.Add(this.rbD3, 0, 2);
            this.tp2.Controls.Add(this.rbD2, 0, 1);
            this.tp2.Controls.Add(this.rbD1, 0, 0);
            this.tp2.Location = new System.Drawing.Point(13, 48);
            this.tp2.Name = "tp2";
            this.tp2.RowCount = 3;
            this.tp2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 24.81752F));
            this.tp2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 24.81752F));
            this.tp2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.18248F));
            this.tp2.Size = new System.Drawing.Size(355, 98);
            this.tp2.TabIndex = 26;
            this.tp2.Visible = false;
            // 
            // rbD3
            // 
            this.rbD3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rbD3.Location = new System.Drawing.Point(3, 67);
            this.rbD3.Name = "rbD3";
            this.rbD3.Size = new System.Drawing.Size(90, 28);
            this.rbD3.TabIndex = 4;
            this.rbD3.TabStop = true;
            this.rbD3.Tag = "2";
            this.rbD3.Text = "Every Month";
            this.rbD3.UseVisualStyleBackColor = true;
            this.rbD3.CheckedChanged += new System.EventHandler(this.rbD1_CheckedChanged);
            // 
            // rbD2
            // 
            this.rbD2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rbD2.Location = new System.Drawing.Point(3, 35);
            this.rbD2.Name = "rbD2";
            this.rbD2.Size = new System.Drawing.Size(90, 26);
            this.rbD2.TabIndex = 2;
            this.rbD2.TabStop = true;
            this.rbD2.Tag = "1";
            this.rbD2.Text = "From Date to";
            this.rbD2.UseVisualStyleBackColor = true;
            this.rbD2.CheckedChanged += new System.EventHandler(this.rbD1_CheckedChanged);
            // 
            // rbD1
            // 
            this.rbD1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rbD1.Location = new System.Drawing.Point(3, 3);
            this.rbD1.Name = "rbD1";
            this.rbD1.Size = new System.Drawing.Size(90, 26);
            this.rbD1.TabIndex = 0;
            this.rbD1.TabStop = true;
            this.rbD1.Tag = "0";
            this.rbD1.Text = "Fix Day";
            this.rbD1.UseVisualStyleBackColor = true;
            this.rbD1.CheckedChanged += new System.EventHandler(this.rbD1_CheckedChanged);
            // 
            // tp4
            // 
            this.tp4.ColumnCount = 3;
            this.tp4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 27.27273F));
            this.tp4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 37.61194F));
            this.tp4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35.22388F));
            this.tp4.Controls.Add(this.numT2, 1, 1);
            this.tp4.Controls.Add(this.numT21, 2, 1);
            this.tp4.Controls.Add(this.numT1, 1, 0);
            this.tp4.Controls.Add(this.numT11, 2, 0);
            this.tp4.Controls.Add(this.rbT1, 0, 0);
            this.tp4.Controls.Add(this.rbT2, 0, 1);
            this.tp4.Location = new System.Drawing.Point(13, 63);
            this.tp4.Name = "tp4";
            this.tp4.RowCount = 2;
            this.tp4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 24.81752F));
            this.tp4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.18248F));
            this.tp4.Size = new System.Drawing.Size(355, 66);
            this.tp4.TabIndex = 31;
            this.tp4.Visible = false;
            // 
            // numT2
            // 
            this.numT2.Enabled = false;
            this.numT2.Location = new System.Drawing.Point(99, 35);
            this.numT2.Name = "numT2";
            this.numT2.Size = new System.Drawing.Size(125, 22);
            this.numT2.TabIndex = 6;
            this.numT2.Text = "0";
            // 
            // numT21
            // 
            this.numT21.Enabled = false;
            this.numT21.Location = new System.Drawing.Point(232, 35);
            this.numT21.Name = "numT21";
            this.numT21.Size = new System.Drawing.Size(120, 22);
            this.numT21.TabIndex = 7;
            this.numT21.Text = "0";
            // 
            // numT1
            // 
            this.numT1.Enabled = false;
            this.numT1.FormattingEnabled = true;
            this.numT1.Items.AddRange(new object[] {
            "Time ",
            "Sunrise",
            "Sunset",
            "Before Sunrise",
            "After Sunrise",
            "Before Sunset",
            "After Sunset"});
            this.numT1.Location = new System.Drawing.Point(99, 3);
            this.numT1.Name = "numT1";
            this.numT1.Size = new System.Drawing.Size(127, 22);
            this.numT1.TabIndex = 10;
            // 
            // numT11
            // 
            this.numT11.Enabled = false;
            this.numT11.Location = new System.Drawing.Point(232, 3);
            this.numT11.Name = "numT11";
            this.numT11.Size = new System.Drawing.Size(120, 22);
            this.numT11.TabIndex = 8;
            this.numT11.Text = "0";
            this.numT11.TextChanged += new System.EventHandler(this.numT11_TextChanged);
            // 
            // rbT1
            // 
            this.rbT1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rbT1.Location = new System.Drawing.Point(3, 3);
            this.rbT1.Name = "rbT1";
            this.rbT1.Size = new System.Drawing.Size(90, 26);
            this.rbT1.TabIndex = 0;
            this.rbT1.TabStop = true;
            this.rbT1.Tag = "0";
            this.rbT1.Text = "Time";
            this.rbT1.UseVisualStyleBackColor = true;
            this.rbT1.CheckedChanged += new System.EventHandler(this.rbT1_CheckedChanged);
            // 
            // rbT2
            // 
            this.rbT2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rbT2.Location = new System.Drawing.Point(3, 35);
            this.rbT2.Name = "rbT2";
            this.rbT2.Size = new System.Drawing.Size(90, 28);
            this.rbT2.TabIndex = 4;
            this.rbT2.TabStop = true;
            this.rbT2.Tag = "1";
            this.rbT2.Text = "From Time to";
            this.rbT2.UseVisualStyleBackColor = true;
            this.rbT2.CheckedChanged += new System.EventHandler(this.rbT1_CheckedChanged);
            // 
            // cboType
            // 
            this.cboType.FormattingEnabled = true;
            this.cboType.Items.AddRange(new object[] {
            "Year Type",
            "Date Type",
            "Week Type",
            "Time Type",
            "UV Switch",
            "Exterior input value",
            "Device scene status",
            "Device sequence status",
            "Exterior universal status",
            "Device channel status",
            "Device curtain status",
            "Panel status",
            "Security Setting",
            "Invalid"});
            this.cboType.Location = new System.Drawing.Point(111, 21);
            this.cboType.Name = "cboType";
            this.cboType.Size = new System.Drawing.Size(127, 22);
            this.cboType.TabIndex = 9;
            this.cboType.SelectedIndexChanged += new System.EventHandler(this.cboType_SelectedIndexChanged);
            // 
            // lbRule
            // 
            this.lbRule.Location = new System.Drawing.Point(13, 20);
            this.lbRule.Name = "lbRule";
            this.lbRule.Size = new System.Drawing.Size(60, 20);
            this.lbRule.TabIndex = 8;
            this.lbRule.Text = "Rule:";
            this.lbRule.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // chbAuto
            // 
            this.chbAuto.Location = new System.Drawing.Point(244, 19);
            this.chbAuto.Name = "chbAuto";
            this.chbAuto.Size = new System.Drawing.Size(145, 29);
            this.chbAuto.TabIndex = 16;
            this.chbAuto.Text = "Action Every Time";
            this.chbAuto.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.ForeColor = System.Drawing.Color.Blue;
            this.label6.Location = new System.Drawing.Point(10, 180);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(358, 35);
            this.label6.TabIndex = 42;
            this.label6.Text = "If you can not find the device from the list, could try to fill the address in th" +
    "is format \"Subnet id\" + \"-\" + \"Device id\", for example 1-2.";
            // 
            // tp5
            // 
            this.tp5.ColumnCount = 3;
            this.tp5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 27.27273F));
            this.tp5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 37.61194F));
            this.tp5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35.22388F));
            this.tp5.Controls.Add(this.label3, 0, 1);
            this.tp5.Controls.Add(this.label1, 0, 0);
            this.tp5.Controls.Add(this.cboUV1, 1, 1);
            this.tp5.Controls.Add(this.numUV1, 1, 0);
            this.tp5.Location = new System.Drawing.Point(13, 72);
            this.tp5.Name = "tp5";
            this.tp5.RowCount = 2;
            this.tp5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 48F));
            this.tp5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 52F));
            this.tp5.Size = new System.Drawing.Size(355, 49);
            this.tp5.TabIndex = 32;
            this.tp5.Visible = false;
            // 
            // label3
            // 
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Location = new System.Drawing.Point(3, 23);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(90, 26);
            this.label3.TabIndex = 14;
            this.label3.Text = "Status:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(90, 23);
            this.label1.TabIndex = 11;
            this.label1.Text = "UV Switch:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cboUV1
            // 
            this.cboUV1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboUV1.FormattingEnabled = true;
            this.cboUV1.Location = new System.Drawing.Point(99, 26);
            this.cboUV1.Name = "cboUV1";
            this.cboUV1.Size = new System.Drawing.Size(127, 22);
            this.cboUV1.TabIndex = 10;
            // 
            // numUV1
            // 
            this.numUV1.Location = new System.Drawing.Point(99, 3);
            this.numUV1.Maximum = new decimal(new int[] {
            254,
            0,
            0,
            0});
            this.numUV1.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numUV1.Name = "numUV1";
            this.numUV1.Size = new System.Drawing.Size(127, 22);
            this.numUV1.TabIndex = 12;
            this.numUV1.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // tp6
            // 
            this.tp6.ColumnCount = 3;
            this.tp6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 27.27273F));
            this.tp6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 37.61194F));
            this.tp6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35.22388F));
            this.tp6.Controls.Add(this.label7, 0, 2);
            this.tp6.Controls.Add(this.label5, 0, 1);
            this.tp6.Controls.Add(this.label2, 0, 0);
            this.tp6.Controls.Add(this.cboDev1, 1, 0);
            this.tp6.Controls.Add(this.cboDev2, 1, 1);
            this.tp6.Controls.Add(this.cboDev3, 1, 2);
            this.tp6.Controls.Add(this.cboDev31, 2, 2);
            this.tp6.Location = new System.Drawing.Point(13, 50);
            this.tp6.Name = "tp6";
            this.tp6.RowCount = 3;
            this.tp6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 24.81752F));
            this.tp6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 24.81752F));
            this.tp6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.18248F));
            this.tp6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 21F));
            this.tp6.Size = new System.Drawing.Size(355, 91);
            this.tp6.TabIndex = 33;
            this.tp6.Visible = false;
            // 
            // label7
            // 
            this.label7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label7.Location = new System.Drawing.Point(3, 60);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(90, 31);
            this.label7.TabIndex = 16;
            this.label7.Text = "Value/Scene:";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label5
            // 
            this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label5.Location = new System.Drawing.Point(3, 30);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(90, 30);
            this.label5.TabIndex = 13;
            this.label5.Text = "Channel/Area:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Location = new System.Drawing.Point(3, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(90, 30);
            this.label2.TabIndex = 10;
            this.label2.Text = "Device:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cboDev1
            // 
            this.cboDev1.FormattingEnabled = true;
            this.cboDev1.Location = new System.Drawing.Point(99, 3);
            this.cboDev1.Name = "cboDev1";
            this.cboDev1.Size = new System.Drawing.Size(127, 22);
            this.cboDev1.TabIndex = 11;
            // 
            // cboDev2
            // 
            this.cboDev2.Location = new System.Drawing.Point(99, 33);
            this.cboDev2.Maximum = new decimal(new int[] {
            254,
            0,
            0,
            0});
            this.cboDev2.Name = "cboDev2";
            this.cboDev2.Size = new System.Drawing.Size(127, 22);
            this.cboDev2.TabIndex = 17;
            // 
            // cboDev3
            // 
            this.cboDev3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDev3.FormattingEnabled = true;
            this.cboDev3.Items.AddRange(new object[] {
            "OFF",
            "ON"});
            this.cboDev3.Location = new System.Drawing.Point(99, 63);
            this.cboDev3.Name = "cboDev3";
            this.cboDev3.Size = new System.Drawing.Size(127, 22);
            this.cboDev3.TabIndex = 18;
            // 
            // cboDev31
            // 
            this.cboDev31.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDev31.FormattingEnabled = true;
            this.cboDev31.Location = new System.Drawing.Point(232, 63);
            this.cboDev31.Name = "cboDev31";
            this.cboDev31.Size = new System.Drawing.Size(120, 22);
            this.cboDev31.TabIndex = 19;
            // 
            // panel2
            // 
            this.panel2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.btnOK);
            this.panel2.Controls.Add(this.tp6);
            this.panel2.Controls.Add(this.tp5);
            this.panel2.Controls.Add(this.label6);
            this.panel2.Controls.Add(this.btnRefresh);
            this.panel2.Controls.Add(this.chbAuto);
            this.panel2.Controls.Add(this.lbRule);
            this.panel2.Controls.Add(this.cboType);
            this.panel2.Controls.Add(this.tp4);
            this.panel2.Controls.Add(this.tp2);
            this.panel2.Controls.Add(this.tp1);
            this.panel2.Controls.Add(this.tp3);
            this.panel2.Enabled = false;
            this.panel2.Location = new System.Drawing.Point(276, 13);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(386, 387);
            this.panel2.TabIndex = 33;
            // 
            // btnOK
            // 
            this.btnOK.BackgroundImage = global::HDL_Buspro_Setup_Tool.Properties.Resources.Save;
            this.btnOK.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnOK.Location = new System.Drawing.Point(291, 333);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(80, 26);
            this.btnOK.TabIndex = 43;
            this.btnOK.Tag = "1";
            this.btnOK.Text = "                      ";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // frmLogicPin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(674, 409);
            this.Controls.Add(this.lvSensors);
            this.Controls.Add(this.panel2);
            this.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "frmLogicPin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Logic Rules creator";
            this.Load += new System.EventHandler(this.frmLogicPin_Load);
            this.cm1.ResumeLayout(false);
            this.tp3.ResumeLayout(false);
            this.tp1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numY3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numY1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numY31)).EndInit();
            this.tp2.ResumeLayout(false);
            this.tp4.ResumeLayout(false);
            this.tp4.PerformLayout();
            this.tp5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numUV1)).EndInit();
            this.tp6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cboDev2)).EndInit();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnRefresh;
        private DateAndTime dateMD1;
        private DateAndTime dateMD2;
        private DateAndTime dateMD21;
        private DateAndTime dateMD3;
        private DateAndTime dateMD31;
        private System.Windows.Forms.ListView lvSensors;
        private System.Windows.Forms.ColumnHeader lvID;
        private System.Windows.Forms.ColumnHeader lvDecription;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ContextMenuStrip cm1;
        private System.Windows.Forms.ToolStripMenuItem addNewToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.TableLayoutPanel tp3;
        private System.Windows.Forms.RadioButton RW2;
        private System.Windows.Forms.RadioButton RW1;
        private System.Windows.Forms.ComboBox cboW1;
        private System.Windows.Forms.ComboBox cboW2;
        private System.Windows.Forms.ComboBox cboW21;
        private System.Windows.Forms.TableLayoutPanel tp1;
        private System.Windows.Forms.DateTimePicker numY4;
        private System.Windows.Forms.RadioButton rbY4;
        private System.Windows.Forms.NumericUpDown numY3;
        private System.Windows.Forms.RadioButton rbY3;
        private System.Windows.Forms.RadioButton rbY2;
        private System.Windows.Forms.RadioButton rbY1;
        private System.Windows.Forms.NumericUpDown numY1;
        private System.Windows.Forms.DateTimePicker numY2;
        private System.Windows.Forms.DateTimePicker numY41;
        private System.Windows.Forms.NumericUpDown numY31;
        private System.Windows.Forms.TableLayoutPanel tp2;
        private System.Windows.Forms.RadioButton rbD3;
        private System.Windows.Forms.RadioButton rbD2;
        private System.Windows.Forms.RadioButton rbD1;
        private System.Windows.Forms.TableLayoutPanel tp4;
        private TimeText numT2;
        private TimeText numT21;
        private System.Windows.Forms.ComboBox numT1;
        private TimeText numT11;
        private System.Windows.Forms.RadioButton rbT1;
        private System.Windows.Forms.RadioButton rbT2;
        private System.Windows.Forms.ComboBox cboType;
        private System.Windows.Forms.Label lbRule;
        private System.Windows.Forms.CheckBox chbAuto;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TableLayoutPanel tp5;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboUV1;
        private System.Windows.Forms.NumericUpDown numUV1;
        private System.Windows.Forms.TableLayoutPanel tp6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cboDev1;
        private System.Windows.Forms.NumericUpDown cboDev2;
        private System.Windows.Forms.ComboBox cboDev3;
        private System.Windows.Forms.ComboBox cboDev31;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnOK;

    }
}