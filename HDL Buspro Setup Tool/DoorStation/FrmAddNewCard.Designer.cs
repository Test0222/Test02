namespace HDL_Buspro_Setup_Tool
{
    partial class FrmAddNewCard
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmAddNewCard));
            this.grpBasic = new System.Windows.Forms.GroupBox();
            this.lbRemarkValue = new System.Windows.Forms.Label();
            this.lbRemark = new System.Windows.Forms.Label();
            this.lbDevValue = new System.Windows.Forms.Label();
            this.lbDev = new System.Windows.Forms.Label();
            this.lbSubValue = new System.Windows.Forms.Label();
            this.lbSub = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnPC = new System.Windows.Forms.Button();
            this.btnModify = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.grpCard = new System.Windows.Forms.GroupBox();
            this.txtRemark = new System.Windows.Forms.TextBox();
            this.lbCardRemark = new System.Windows.Forms.Label();
            this.txtPhone = new System.Windows.Forms.TextBox();
            this.lbPhone = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.lbName = new System.Windows.Forms.Label();
            this.lbTimeHint = new System.Windows.Forms.Label();
            this.numTime2 = new System.Windows.Forms.NumericUpDown();
            this.lbTime1 = new System.Windows.Forms.Label();
            this.numTime1 = new System.Windows.Forms.NumericUpDown();
            this.lbTime = new System.Windows.Forms.Label();
            this.TimePicker = new System.Windows.Forms.DateTimePicker();
            this.lbDate = new System.Windows.Forms.Label();
            this.txtRoom = new System.Windows.Forms.TextBox();
            this.lbRoom = new System.Windows.Forms.Label();
            this.txtUnit = new System.Windows.Forms.TextBox();
            this.lbUnit = new System.Windows.Forms.Label();
            this.txtBuilding = new System.Windows.Forms.TextBox();
            this.lbBuilding = new System.Windows.Forms.Label();
            this.cbType = new System.Windows.Forms.ComboBox();
            this.lbType = new System.Windows.Forms.Label();
            this.lbUIDDV = new System.Windows.Forms.Label();
            this.lbUIDD = new System.Windows.Forms.Label();
            this.lbUIDLV = new System.Windows.Forms.Label();
            this.lbUIDL = new System.Windows.Forms.Label();
            this.grpBasic.SuspendLayout();
            this.panel2.SuspendLayout();
            this.grpCard.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numTime2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTime1)).BeginInit();
            this.SuspendLayout();
            // 
            // grpBasic
            // 
            this.grpBasic.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.grpBasic.Controls.Add(this.lbRemarkValue);
            this.grpBasic.Controls.Add(this.lbRemark);
            this.grpBasic.Controls.Add(this.lbDevValue);
            this.grpBasic.Controls.Add(this.lbDev);
            this.grpBasic.Controls.Add(this.lbSubValue);
            this.grpBasic.Controls.Add(this.lbSub);
            this.grpBasic.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpBasic.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpBasic.Location = new System.Drawing.Point(0, 0);
            this.grpBasic.Margin = new System.Windows.Forms.Padding(4);
            this.grpBasic.Name = "grpBasic";
            this.grpBasic.Padding = new System.Windows.Forms.Padding(4);
            this.grpBasic.Size = new System.Drawing.Size(657, 82);
            this.grpBasic.TabIndex = 1;
            this.grpBasic.TabStop = false;
            this.grpBasic.Text = "Basic information";
            // 
            // lbRemarkValue
            // 
            this.lbRemarkValue.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbRemarkValue.Location = new System.Drawing.Point(117, 51);
            this.lbRemarkValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbRemarkValue.Name = "lbRemarkValue";
            this.lbRemarkValue.Size = new System.Drawing.Size(329, 19);
            this.lbRemarkValue.TabIndex = 5;
            this.lbRemarkValue.Text = "      ";
            // 
            // lbRemark
            // 
            this.lbRemark.AutoSize = true;
            this.lbRemark.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbRemark.Location = new System.Drawing.Point(15, 51);
            this.lbRemark.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbRemark.Name = "lbRemark";
            this.lbRemark.Size = new System.Drawing.Size(51, 14);
            this.lbRemark.TabIndex = 4;
            this.lbRemark.Text = "Remark:";
            // 
            // lbDevValue
            // 
            this.lbDevValue.AutoSize = true;
            this.lbDevValue.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbDevValue.Location = new System.Drawing.Point(443, 22);
            this.lbDevValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbDevValue.Name = "lbDevValue";
            this.lbDevValue.Size = new System.Drawing.Size(13, 14);
            this.lbDevValue.TabIndex = 3;
            this.lbDevValue.Text = "0";
            // 
            // lbDev
            // 
            this.lbDev.AutoSize = true;
            this.lbDev.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbDev.Location = new System.Drawing.Point(311, 22);
            this.lbDev.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbDev.Name = "lbDev";
            this.lbDev.Size = new System.Drawing.Size(61, 14);
            this.lbDev.TabIndex = 2;
            this.lbDev.Text = "Device ID:";
            // 
            // lbSubValue
            // 
            this.lbSubValue.AutoSize = true;
            this.lbSubValue.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbSubValue.Location = new System.Drawing.Point(117, 22);
            this.lbSubValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbSubValue.Name = "lbSubValue";
            this.lbSubValue.Size = new System.Drawing.Size(13, 14);
            this.lbSubValue.TabIndex = 1;
            this.lbSubValue.Text = "0";
            // 
            // lbSub
            // 
            this.lbSub.AutoSize = true;
            this.lbSub.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbSub.Location = new System.Drawing.Point(15, 22);
            this.lbSub.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbSub.Name = "lbSub";
            this.lbSub.Size = new System.Drawing.Size(63, 14);
            this.lbSub.TabIndex = 0;
            this.lbSub.Text = "Subnet ID:";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnPC);
            this.panel2.Controls.Add(this.btnModify);
            this.panel2.Controls.Add(this.btnClose);
            this.panel2.Controls.Add(this.btnAdd);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 333);
            this.panel2.Margin = new System.Windows.Forms.Padding(4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(657, 56);
            this.panel2.TabIndex = 1;
            // 
            // btnPC
            // 
            this.btnPC.Location = new System.Drawing.Point(54, 11);
            this.btnPC.Margin = new System.Windows.Forms.Padding(4);
            this.btnPC.Name = "btnPC";
            this.btnPC.Size = new System.Drawing.Size(140, 32);
            this.btnPC.TabIndex = 26;
            this.btnPC.Text = "PC time";
            this.btnPC.UseVisualStyleBackColor = true;
            this.btnPC.Click += new System.EventHandler(this.btnPC_Click);
            // 
            // btnModify
            // 
            this.btnModify.Image = global::HDL_Buspro_Setup_Tool.Properties.Resources.Save;
            this.btnModify.Location = new System.Drawing.Point(194, 11);
            this.btnModify.Margin = new System.Windows.Forms.Padding(4);
            this.btnModify.Name = "btnModify";
            this.btnModify.Size = new System.Drawing.Size(140, 32);
            this.btnModify.TabIndex = 2;
            this.btnModify.Text = "Modify card";
            this.btnModify.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnModify.UseVisualStyleBackColor = true;
            this.btnModify.Click += new System.EventHandler(this.btnModify_Click);
            // 
            // btnClose
            // 
            this.btnClose.Image = global::HDL_Buspro_Setup_Tool.Properties.Resources.Save;
            this.btnClose.Location = new System.Drawing.Point(474, 11);
            this.btnClose.Margin = new System.Windows.Forms.Padding(4);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(140, 32);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "Save && Close";
            this.btnClose.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Image = ((System.Drawing.Image)(resources.GetObject("btnAdd.Image")));
            this.btnAdd.Location = new System.Drawing.Point(334, 11);
            this.btnAdd.Margin = new System.Windows.Forms.Padding(4);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(140, 32);
            this.btnAdd.TabIndex = 0;
            this.btnAdd.Text = "Add Card";
            this.btnAdd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // grpCard
            // 
            this.grpCard.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.grpCard.Controls.Add(this.txtRemark);
            this.grpCard.Controls.Add(this.lbCardRemark);
            this.grpCard.Controls.Add(this.txtPhone);
            this.grpCard.Controls.Add(this.lbPhone);
            this.grpCard.Controls.Add(this.txtName);
            this.grpCard.Controls.Add(this.lbName);
            this.grpCard.Controls.Add(this.lbTimeHint);
            this.grpCard.Controls.Add(this.numTime2);
            this.grpCard.Controls.Add(this.lbTime1);
            this.grpCard.Controls.Add(this.numTime1);
            this.grpCard.Controls.Add(this.lbTime);
            this.grpCard.Controls.Add(this.TimePicker);
            this.grpCard.Controls.Add(this.lbDate);
            this.grpCard.Controls.Add(this.txtRoom);
            this.grpCard.Controls.Add(this.lbRoom);
            this.grpCard.Controls.Add(this.txtUnit);
            this.grpCard.Controls.Add(this.lbUnit);
            this.grpCard.Controls.Add(this.txtBuilding);
            this.grpCard.Controls.Add(this.lbBuilding);
            this.grpCard.Controls.Add(this.cbType);
            this.grpCard.Controls.Add(this.lbType);
            this.grpCard.Controls.Add(this.lbUIDDV);
            this.grpCard.Controls.Add(this.lbUIDD);
            this.grpCard.Controls.Add(this.lbUIDLV);
            this.grpCard.Controls.Add(this.lbUIDL);
            this.grpCard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpCard.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpCard.Location = new System.Drawing.Point(0, 82);
            this.grpCard.Margin = new System.Windows.Forms.Padding(4);
            this.grpCard.Name = "grpCard";
            this.grpCard.Padding = new System.Windows.Forms.Padding(4);
            this.grpCard.Size = new System.Drawing.Size(657, 251);
            this.grpCard.TabIndex = 0;
            this.grpCard.TabStop = false;
            this.grpCard.Text = "Card infomation";
            // 
            // txtRemark
            // 
            this.txtRemark.Font = new System.Drawing.Font("Calibri", 12F);
            this.txtRemark.Location = new System.Drawing.Point(117, 216);
            this.txtRemark.Margin = new System.Windows.Forms.Padding(4);
            this.txtRemark.Name = "txtRemark";
            this.txtRemark.Size = new System.Drawing.Size(461, 27);
            this.txtRemark.TabIndex = 25;
            // 
            // lbCardRemark
            // 
            this.lbCardRemark.AutoSize = true;
            this.lbCardRemark.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbCardRemark.Location = new System.Drawing.Point(16, 216);
            this.lbCardRemark.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbCardRemark.Name = "lbCardRemark";
            this.lbCardRemark.Size = new System.Drawing.Size(51, 14);
            this.lbCardRemark.TabIndex = 24;
            this.lbCardRemark.Text = "Remark:";
            // 
            // txtPhone
            // 
            this.txtPhone.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPhone.Location = new System.Drawing.Point(117, 141);
            this.txtPhone.Margin = new System.Windows.Forms.Padding(4);
            this.txtPhone.MaxLength = 11;
            this.txtPhone.Name = "txtPhone";
            this.txtPhone.Size = new System.Drawing.Size(132, 22);
            this.txtPhone.TabIndex = 23;
            this.txtPhone.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtPhone_KeyPress);
            // 
            // lbPhone
            // 
            this.lbPhone.AutoSize = true;
            this.lbPhone.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbPhone.Location = new System.Drawing.Point(16, 144);
            this.lbPhone.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbPhone.Name = "lbPhone";
            this.lbPhone.Size = new System.Drawing.Size(44, 14);
            this.lbPhone.TabIndex = 22;
            this.lbPhone.Text = "Phone:";
            // 
            // txtName
            // 
            this.txtName.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtName.Location = new System.Drawing.Point(120, 104);
            this.txtName.Margin = new System.Windows.Forms.Padding(4);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(132, 22);
            this.txtName.TabIndex = 21;
            // 
            // lbName
            // 
            this.lbName.AutoSize = true;
            this.lbName.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbName.Location = new System.Drawing.Point(16, 107);
            this.lbName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbName.Name = "lbName";
            this.lbName.Size = new System.Drawing.Size(59, 14);
            this.lbName.TabIndex = 20;
            this.lbName.Text = "Contacts :";
            // 
            // lbTimeHint
            // 
            this.lbTimeHint.AutoSize = true;
            this.lbTimeHint.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbTimeHint.Location = new System.Drawing.Point(592, 177);
            this.lbTimeHint.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbTimeHint.Name = "lbTimeHint";
            this.lbTimeHint.Size = new System.Drawing.Size(54, 14);
            this.lbTimeHint.TabIndex = 19;
            this.lbTimeHint.Text = "(HH:MM)";
            // 
            // numTime2
            // 
            this.numTime2.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numTime2.Location = new System.Drawing.Point(526, 169);
            this.numTime2.Margin = new System.Windows.Forms.Padding(4);
            this.numTime2.Maximum = new decimal(new int[] {
            59,
            0,
            0,
            0});
            this.numTime2.Name = "numTime2";
            this.numTime2.Size = new System.Drawing.Size(52, 22);
            this.numTime2.TabIndex = 18;
            // 
            // lbTime1
            // 
            this.lbTime1.AutoSize = true;
            this.lbTime1.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbTime1.Location = new System.Drawing.Point(508, 173);
            this.lbTime1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbTime1.Name = "lbTime1";
            this.lbTime1.Size = new System.Drawing.Size(10, 14);
            this.lbTime1.TabIndex = 16;
            this.lbTime1.Text = ":";
            // 
            // numTime1
            // 
            this.numTime1.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numTime1.Location = new System.Drawing.Point(446, 171);
            this.numTime1.Margin = new System.Windows.Forms.Padding(4);
            this.numTime1.Maximum = new decimal(new int[] {
            23,
            0,
            0,
            0});
            this.numTime1.Name = "numTime1";
            this.numTime1.Size = new System.Drawing.Size(52, 22);
            this.numTime1.TabIndex = 15;
            // 
            // lbTime
            // 
            this.lbTime.AutoSize = true;
            this.lbTime.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbTime.Location = new System.Drawing.Point(311, 183);
            this.lbTime.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbTime.Name = "lbTime";
            this.lbTime.Size = new System.Drawing.Size(37, 14);
            this.lbTime.TabIndex = 14;
            this.lbTime.Text = "Time:";
            // 
            // TimePicker
            // 
            this.TimePicker.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TimePicker.Location = new System.Drawing.Point(120, 177);
            this.TimePicker.Margin = new System.Windows.Forms.Padding(4);
            this.TimePicker.Name = "TimePicker";
            this.TimePicker.Size = new System.Drawing.Size(132, 22);
            this.TimePicker.TabIndex = 13;
            // 
            // lbDate
            // 
            this.lbDate.AutoSize = true;
            this.lbDate.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbDate.Location = new System.Drawing.Point(16, 183);
            this.lbDate.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbDate.Name = "lbDate";
            this.lbDate.Size = new System.Drawing.Size(66, 14);
            this.lbDate.TabIndex = 12;
            this.lbDate.Text = "Valid date:";
            // 
            // txtRoom
            // 
            this.txtRoom.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRoom.Location = new System.Drawing.Point(446, 141);
            this.txtRoom.Margin = new System.Windows.Forms.Padding(4);
            this.txtRoom.MaxLength = 4;
            this.txtRoom.Name = "txtRoom";
            this.txtRoom.Size = new System.Drawing.Size(132, 22);
            this.txtRoom.TabIndex = 11;
            // 
            // lbRoom
            // 
            this.lbRoom.AutoSize = true;
            this.lbRoom.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbRoom.Location = new System.Drawing.Point(311, 144);
            this.lbRoom.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbRoom.Name = "lbRoom";
            this.lbRoom.Size = new System.Drawing.Size(86, 14);
            this.lbRoom.TabIndex = 10;
            this.lbRoom.Text = "Room number:";
            // 
            // txtUnit
            // 
            this.txtUnit.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUnit.Location = new System.Drawing.Point(446, 104);
            this.txtUnit.Margin = new System.Windows.Forms.Padding(4);
            this.txtUnit.MaxLength = 4;
            this.txtUnit.Name = "txtUnit";
            this.txtUnit.Size = new System.Drawing.Size(132, 22);
            this.txtUnit.TabIndex = 9;
            // 
            // lbUnit
            // 
            this.lbUnit.AutoSize = true;
            this.lbUnit.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbUnit.Location = new System.Drawing.Point(311, 107);
            this.lbUnit.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbUnit.Name = "lbUnit";
            this.lbUnit.Size = new System.Drawing.Size(78, 14);
            this.lbUnit.TabIndex = 8;
            this.lbUnit.Text = "Unit number:";
            // 
            // txtBuilding
            // 
            this.txtBuilding.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBuilding.Location = new System.Drawing.Point(446, 67);
            this.txtBuilding.Margin = new System.Windows.Forms.Padding(4);
            this.txtBuilding.MaxLength = 4;
            this.txtBuilding.Name = "txtBuilding";
            this.txtBuilding.Size = new System.Drawing.Size(132, 22);
            this.txtBuilding.TabIndex = 7;
            this.txtBuilding.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtBuilding_KeyPress);
            // 
            // lbBuilding
            // 
            this.lbBuilding.AutoSize = true;
            this.lbBuilding.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbBuilding.Location = new System.Drawing.Point(311, 70);
            this.lbBuilding.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbBuilding.Name = "lbBuilding";
            this.lbBuilding.Size = new System.Drawing.Size(101, 14);
            this.lbBuilding.TabIndex = 6;
            this.lbBuilding.Text = "Building number:";
            // 
            // cbType
            // 
            this.cbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbType.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbType.FormattingEnabled = true;
            this.cbType.Location = new System.Drawing.Point(446, 29);
            this.cbType.Margin = new System.Windows.Forms.Padding(4);
            this.cbType.Name = "cbType";
            this.cbType.Size = new System.Drawing.Size(132, 22);
            this.cbType.TabIndex = 5;
            // 
            // lbType
            // 
            this.lbType.AutoSize = true;
            this.lbType.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbType.Location = new System.Drawing.Point(311, 33);
            this.lbType.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbType.Name = "lbType";
            this.lbType.Size = new System.Drawing.Size(60, 14);
            this.lbType.TabIndex = 4;
            this.lbType.Text = "Card type:";
            // 
            // lbUIDDV
            // 
            this.lbUIDDV.AutoSize = true;
            this.lbUIDDV.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbUIDDV.Location = new System.Drawing.Point(117, 70);
            this.lbUIDDV.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbUIDDV.Name = "lbUIDDV";
            this.lbUIDDV.Size = new System.Drawing.Size(13, 14);
            this.lbUIDDV.TabIndex = 3;
            this.lbUIDDV.Text = "0";
            // 
            // lbUIDD
            // 
            this.lbUIDD.AutoSize = true;
            this.lbUIDD.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbUIDD.Location = new System.Drawing.Point(16, 70);
            this.lbUIDD.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbUIDD.Name = "lbUIDD";
            this.lbUIDD.Size = new System.Drawing.Size(93, 14);
            this.lbUIDD.TabIndex = 2;
            this.lbUIDD.Text = "UID infomation:";
            // 
            // lbUIDLV
            // 
            this.lbUIDLV.AutoSize = true;
            this.lbUIDLV.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbUIDLV.Location = new System.Drawing.Point(117, 33);
            this.lbUIDLV.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbUIDLV.Name = "lbUIDLV";
            this.lbUIDLV.Size = new System.Drawing.Size(13, 14);
            this.lbUIDLV.TabIndex = 1;
            this.lbUIDLV.Text = "0";
            // 
            // lbUIDL
            // 
            this.lbUIDL.AutoSize = true;
            this.lbUIDL.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbUIDL.Location = new System.Drawing.Point(16, 33);
            this.lbUIDL.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbUIDL.Name = "lbUIDL";
            this.lbUIDL.Size = new System.Drawing.Size(68, 14);
            this.lbUIDL.TabIndex = 0;
            this.lbUIDL.Text = "UID length:";
            // 
            // FrmAddNewCard
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(657, 389);
            this.Controls.Add(this.grpCard);
            this.Controls.Add(this.grpBasic);
            this.Controls.Add(this.panel2);
            this.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmAddNewCard";
            this.Text = "FrmAddNewCard";
            this.Load += new System.EventHandler(this.FrmAddNewCard_Load);
            this.grpBasic.ResumeLayout(false);
            this.grpBasic.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.grpCard.ResumeLayout(false);
            this.grpCard.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numTime2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTime1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpBasic;
        private System.Windows.Forms.Label lbRemarkValue;
        private System.Windows.Forms.Label lbRemark;
        private System.Windows.Forms.Label lbDevValue;
        private System.Windows.Forms.Label lbDev;
        private System.Windows.Forms.Label lbSubValue;
        private System.Windows.Forms.Label lbSub;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.GroupBox grpCard;
        private System.Windows.Forms.DateTimePicker TimePicker;
        private System.Windows.Forms.Label lbDate;
        private System.Windows.Forms.TextBox txtRoom;
        private System.Windows.Forms.Label lbRoom;
        private System.Windows.Forms.TextBox txtUnit;
        private System.Windows.Forms.Label lbUnit;
        private System.Windows.Forms.TextBox txtBuilding;
        private System.Windows.Forms.Label lbBuilding;
        private System.Windows.Forms.ComboBox cbType;
        private System.Windows.Forms.Label lbType;
        private System.Windows.Forms.Label lbUIDDV;
        private System.Windows.Forms.Label lbUIDD;
        private System.Windows.Forms.Label lbUIDLV;
        private System.Windows.Forms.Label lbUIDL;
        private System.Windows.Forms.TextBox txtRemark;
        private System.Windows.Forms.Label lbCardRemark;
        private System.Windows.Forms.TextBox txtPhone;
        private System.Windows.Forms.Label lbPhone;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label lbName;
        private System.Windows.Forms.Label lbTimeHint;
        private System.Windows.Forms.NumericUpDown numTime2;
        private System.Windows.Forms.Label lbTime1;
        private System.Windows.Forms.NumericUpDown numTime1;
        private System.Windows.Forms.Label lbTime;
        private System.Windows.Forms.Button btnModify;
        private System.Windows.Forms.Button btnPC;
    }
}