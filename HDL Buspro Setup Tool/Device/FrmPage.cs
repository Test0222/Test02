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
    public partial class FrmPage : Form
    {
        public FrmPage()
        {
            InitializeComponent();
            rb1.Checked = true;
        }

        private void pic1_Click(object sender, EventArgs e)
        {
            if (rb1.Checked) CsConst.DownloadPictruePageID = 1;
            if (rb2.Checked) CsConst.DownloadPictruePageID = 2;
            if (rb3.Checked) CsConst.DownloadPictruePageID = 3;
            if (rb4.Checked) CsConst.DownloadPictruePageID = 4;
            if (rb5.Checked) CsConst.DownloadPictruePageID = 255;
            DialogResult = DialogResult.OK;
            this.Close();
        }

        private void pic2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
