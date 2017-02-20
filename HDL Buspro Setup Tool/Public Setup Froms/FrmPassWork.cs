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
    public partial class FrmPassWork : Form
    {
        public FrmPassWork()
        {
            InitializeComponent();
            if (CsConst.iLanguageId == 1)
            {
                lbPassword.Text = "请输入密码:";
                btnOK.Text = "确定";
                btnCencel.Text = "取消";
            }
        }

        private void btnCencel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            string str = txtPassword.Text;
            if (str == "85521566")
            {
                DialogResult = DialogResult.OK;
                CsConst.isRightPasswork = true;
                this.Close();
            }
            else
            {
                MessageBox.Show(CsConst.mstrINIDefault.IniReadValue("Public", "99637", ""), ""
                                , MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            }
        }
    }
}
