namespace HDL_Buspro_Setup_Tool
{
    partial class CurtainForColorDLP
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CurtainForColorDLP));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnSaveAC = new System.Windows.Forms.Button();
            this.cbSwitch = new System.Windows.Forms.ComboBox();
            this.lbSwitch = new System.Windows.Forms.Label();
            this.txtCurtaiNum = new System.Windows.Forms.TextBox();
            this.lbCurtainNum = new System.Windows.Forms.Label();
            this.cbMode = new System.Windows.Forms.ComboBox();
            this.lbMode = new System.Windows.Forms.Label();
            this.NumDev = new System.Windows.Forms.NumericUpDown();
            this.NumSub = new System.Windows.Forms.NumericUpDown();
            this.lbDeviceID = new System.Windows.Forms.Label();
            this.lbSubnetID = new System.Windows.Forms.Label();
            this.txtRemark = new System.Windows.Forms.TextBox();
            this.lbRemark = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumDev)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumSub)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.groupBox1.Controls.Add(this.btnSaveAC);
            this.groupBox1.Controls.Add(this.cbSwitch);
            this.groupBox1.Controls.Add(this.lbSwitch);
            this.groupBox1.Controls.Add(this.txtCurtaiNum);
            this.groupBox1.Controls.Add(this.lbCurtainNum);
            this.groupBox1.Controls.Add(this.cbMode);
            this.groupBox1.Controls.Add(this.lbMode);
            this.groupBox1.Controls.Add(this.NumDev);
            this.groupBox1.Controls.Add(this.NumSub);
            this.groupBox1.Controls.Add(this.lbDeviceID);
            this.groupBox1.Controls.Add(this.lbSubnetID);
            this.groupBox1.Controls.Add(this.txtRemark);
            this.groupBox1.Controls.Add(this.lbRemark);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(275, 210);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // btnSaveAC
            // 
            this.btnSaveAC.Image = ((System.Drawing.Image)(resources.GetObject("btnSaveAC.Image")));
            this.btnSaveAC.Location = new System.Drawing.Point(235, 176);
            this.btnSaveAC.Name = "btnSaveAC";
            this.btnSaveAC.Size = new System.Drawing.Size(31, 25);
            this.btnSaveAC.TabIndex = 102;
            this.btnSaveAC.UseVisualStyleBackColor = true;
            this.btnSaveAC.Click += new System.EventHandler(this.btnSaveAC_Click);
            // 
            // cbSwitch
            // 
            this.cbSwitch.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSwitch.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbSwitch.FormattingEnabled = true;
            this.cbSwitch.Location = new System.Drawing.Point(103, 174);
            this.cbSwitch.Name = "cbSwitch";
            this.cbSwitch.Size = new System.Drawing.Size(114, 22);
            this.cbSwitch.TabIndex = 11;
            this.cbSwitch.SelectedIndexChanged += new System.EventHandler(this.cbSwitch_SelectedIndexChanged);
            // 
            // lbSwitch
            // 
            this.lbSwitch.AutoSize = true;
            this.lbSwitch.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbSwitch.Location = new System.Drawing.Point(8, 177);
            this.lbSwitch.Name = "lbSwitch";
            this.lbSwitch.Size = new System.Drawing.Size(44, 14);
            this.lbSwitch.TabIndex = 10;
            this.lbSwitch.Text = "Status:";
            // 
            // txtCurtaiNum
            // 
            this.txtCurtaiNum.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCurtaiNum.Location = new System.Drawing.Point(103, 143);
            this.txtCurtaiNum.Name = "txtCurtaiNum";
            this.txtCurtaiNum.Size = new System.Drawing.Size(117, 22);
            this.txtCurtaiNum.TabIndex = 9;
            this.txtCurtaiNum.TextChanged += new System.EventHandler(this.txtCurtaiNum_TextChanged);
            this.txtCurtaiNum.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtCurtaiNum_KeyPress);
            // 
            // lbCurtainNum
            // 
            this.lbCurtainNum.AutoSize = true;
            this.lbCurtainNum.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbCurtainNum.Location = new System.Drawing.Point(8, 147);
            this.lbCurtainNum.Name = "lbCurtainNum";
            this.lbCurtainNum.Size = new System.Drawing.Size(70, 14);
            this.lbCurtainNum.TabIndex = 8;
            this.lbCurtainNum.Text = "Curtain No.:";
            // 
            // cbMode
            // 
            this.cbMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbMode.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbMode.FormattingEnabled = true;
            this.cbMode.Location = new System.Drawing.Point(103, 113);
            this.cbMode.Name = "cbMode";
            this.cbMode.Size = new System.Drawing.Size(117, 22);
            this.cbMode.TabIndex = 7;
            this.cbMode.SelectedIndexChanged += new System.EventHandler(this.cbMode_SelectedIndexChanged);
            // 
            // lbMode
            // 
            this.lbMode.AutoSize = true;
            this.lbMode.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbMode.Location = new System.Drawing.Point(8, 117);
            this.lbMode.Name = "lbMode";
            this.lbMode.Size = new System.Drawing.Size(41, 14);
            this.lbMode.TabIndex = 6;
            this.lbMode.Text = "Mode:";
            // 
            // NumDev
            // 
            this.NumDev.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NumDev.Location = new System.Drawing.Point(103, 84);
            this.NumDev.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.NumDev.Name = "NumDev";
            this.NumDev.Size = new System.Drawing.Size(117, 22);
            this.NumDev.TabIndex = 5;
            this.NumDev.ValueChanged += new System.EventHandler(this.NumDev_ValueChanged);
            // 
            // NumSub
            // 
            this.NumSub.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NumSub.Location = new System.Drawing.Point(103, 54);
            this.NumSub.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.NumSub.Name = "NumSub";
            this.NumSub.Size = new System.Drawing.Size(117, 22);
            this.NumSub.TabIndex = 4;
            this.NumSub.ValueChanged += new System.EventHandler(this.NumSub_ValueChanged);
            // 
            // lbDeviceID
            // 
            this.lbDeviceID.AutoSize = true;
            this.lbDeviceID.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbDeviceID.Location = new System.Drawing.Point(8, 87);
            this.lbDeviceID.Name = "lbDeviceID";
            this.lbDeviceID.Size = new System.Drawing.Size(61, 14);
            this.lbDeviceID.TabIndex = 3;
            this.lbDeviceID.Text = "Device ID:";
            // 
            // lbSubnetID
            // 
            this.lbSubnetID.AutoSize = true;
            this.lbSubnetID.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbSubnetID.Location = new System.Drawing.Point(8, 57);
            this.lbSubnetID.Name = "lbSubnetID";
            this.lbSubnetID.Size = new System.Drawing.Size(63, 14);
            this.lbSubnetID.TabIndex = 2;
            this.lbSubnetID.Text = "Subnet ID:";
            // 
            // txtRemark
            // 
            this.txtRemark.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRemark.Location = new System.Drawing.Point(103, 24);
            this.txtRemark.Name = "txtRemark";
            this.txtRemark.Size = new System.Drawing.Size(117, 22);
            this.txtRemark.TabIndex = 1;
            this.txtRemark.TextChanged += new System.EventHandler(this.txtRemark_TextChanged);
            // 
            // lbRemark
            // 
            this.lbRemark.AutoSize = true;
            this.lbRemark.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbRemark.Location = new System.Drawing.Point(8, 27);
            this.lbRemark.Name = "lbRemark";
            this.lbRemark.Size = new System.Drawing.Size(42, 14);
            this.lbRemark.TabIndex = 0;
            this.lbRemark.Text = "Name:";
            // 
            // CurtainForColorDLP
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Arial", 10F);
            this.Name = "CurtainForColorDLP";
            this.Size = new System.Drawing.Size(275, 210);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumDev)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumSub)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox  groupBox1;
        private System.Windows.Forms.ComboBox cbSwitch;
        private System.Windows.Forms.Label lbSwitch;
        private System.Windows.Forms.TextBox txtCurtaiNum;
        private System.Windows.Forms.Label lbCurtainNum;
        private System.Windows.Forms.ComboBox cbMode;
        private System.Windows.Forms.Label lbMode;
        private System.Windows.Forms.NumericUpDown NumDev;
        private System.Windows.Forms.NumericUpDown NumSub;
        private System.Windows.Forms.Label lbDeviceID;
        private System.Windows.Forms.Label lbSubnetID;
        private System.Windows.Forms.TextBox txtRemark;
        private System.Windows.Forms.Label lbRemark;
        private System.Windows.Forms.Button btnSaveAC;
    }
}
