namespace HDL_Buspro_Setup_Tool
{
    partial class UserForNewDoor
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserForNewDoor));
            this.lb = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.PIC = new System.Windows.Forms.PictureBox();
            this.picSelect = new System.Windows.Forms.PictureBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PIC)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picSelect)).BeginInit();
            this.SuspendLayout();
            // 
            // lb
            // 
            this.lb.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lb.Location = new System.Drawing.Point(0, 80);
            this.lb.Name = "lb";
            this.lb.Size = new System.Drawing.Size(130, 30);
            this.lb.TabIndex = 1;
            this.lb.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.lb.MouseClick += new System.Windows.Forms.MouseEventHandler(this.UserForNewDoor_MouseClick);
            this.lb.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.UserForNewDoor_MouseDoubleClick);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.PIC);
            this.panel1.Controls.Add(this.picSelect);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(130, 80);
            this.panel1.TabIndex = 2;
            // 
            // PIC
            // 
            this.PIC.Image = ((System.Drawing.Image)(resources.GetObject("PIC.Image")));
            this.PIC.Location = new System.Drawing.Point(30, 7);
            this.PIC.Name = "PIC";
            this.PIC.Size = new System.Drawing.Size(64, 64);
            this.PIC.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.PIC.TabIndex = 0;
            this.PIC.TabStop = false;
            this.PIC.MouseClick += new System.Windows.Forms.MouseEventHandler(this.UserForNewDoor_MouseClick);
            this.PIC.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.UserForNewDoor_MouseDoubleClick);
            this.PIC.MouseLeave += new System.EventHandler(this.PIC_MouseLeave);
            this.PIC.MouseHover += new System.EventHandler(this.PIC_MouseHover);
            // 
            // picSelect
            // 
            this.picSelect.Image = ((System.Drawing.Image)(resources.GetObject("picSelect.Image")));
            this.picSelect.Location = new System.Drawing.Point(23, 0);
            this.picSelect.Name = "picSelect";
            this.picSelect.Size = new System.Drawing.Size(80, 80);
            this.picSelect.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picSelect.TabIndex = 1;
            this.picSelect.TabStop = false;
            this.picSelect.MouseClick += new System.Windows.Forms.MouseEventHandler(this.UserForNewDoor_MouseClick);
            this.picSelect.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.UserForNewDoor_MouseDoubleClick);
            // 
            // UserForNewDoor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.lb);
            this.Name = "UserForNewDoor";
            this.Size = new System.Drawing.Size(130, 110);
            this.Load += new System.EventHandler(this.UserForNewDoor_Load);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.UserForNewDoor_MouseClick);
            this.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.UserForNewDoor_MouseDoubleClick);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PIC)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picSelect)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lb;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox PIC;
        private System.Windows.Forms.PictureBox picSelect;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}

