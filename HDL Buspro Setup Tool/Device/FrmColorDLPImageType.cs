using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HDL_Buspro_Setup_Tool
{
    public partial class FrmColorDLPImageType : Form
    {
        private int Type;
        public FrmColorDLPImageType()
        {
            InitializeComponent();
        }

        public FrmColorDLPImageType(int type)
        {
            InitializeComponent();
            this.Type = type;
        }

        private void FrmColorDLPImageType_Load(object sender, EventArgs e)
        {
            if (Type == 0)
            {
                for (int i = 0; i < 6; i++)
                {
                    ToolStripMenuItem temp = toolStrip1.Items.Find("tsmB" + i.ToString(), true)[0] as ToolStripMenuItem;
                    Visible = true;
                }
                for (int i = 0; i < 14; i++)
                {
                    ToolStripMenuItem temp = toolStrip1.Items.Find("tsmI" + i.ToString(), true)[0] as ToolStripMenuItem;
                    Visible = true;
                }
                for (int i = 0; i < 14; i++)
                {
                    ToolStripMenuItem temp = toolStrip1.Items.Find("tsmL" + i.ToString(), true)[0] as ToolStripMenuItem;
                    Visible = true;
                }
            }
            else if (Type == 1)
            {
                for (int i = 0; i < 6; i++)
                {
                    ToolStripMenuItem temp = toolStrip1.Items.Find("tsmB" + i.ToString(), true)[0] as ToolStripMenuItem;
                    Visible = false;
                }
                for (int i = 0; i < 14; i++)
                {
                    ToolStripMenuItem temp = toolStrip1.Items.Find("tsmI" + i.ToString(), true)[0] as ToolStripMenuItem;
                    Visible = false;
                }
                for (int i = 0; i < 14; i++)
                {
                    ToolStripMenuItem temp = toolStrip1.Items.Find("tsmL" + i.ToString(), true)[0] as ToolStripMenuItem;
                    Visible = false;
                }
            }
        }

        private void tsbM1_Click(object sender, EventArgs e)
        {
            if (Type == 0)
            {

            }
            else if (Type == 1)
            {
                int Tage = Convert.ToInt32((sender as ToolStripDropDownButton).Tag);
                CsConst.UploadColorDLPType = Tage;
                DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void pic2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
