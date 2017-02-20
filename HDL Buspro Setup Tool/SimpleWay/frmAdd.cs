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
    public partial class frmAdd : Form
    {
        public frmAdd()
        {
            InitializeComponent();
        }

        public frmAdd(int intEditMode, string strName, string strDisplay)
        {
            InitializeComponent();
            this.Text = strName;
            tbRmName.Text = strDisplay;
            //分类处理不同情况的数据
            if (intEditMode == 0) // 新增楼层和房子备注
            {
                ndSum.Enabled = true;
                btnOK.Text = "OK";
            }
            else if (intEditMode == 1)  // 新增房间备注
            {
                ndSum.Enabled = true;
                btnOK.Text = "OK";
            }
            else if (intEditMode == 2) // 修改楼层和房子备注
            {
                ndSum.Enabled = false;
                btnOK.Text = "Modify";
            }
            else if (intEditMode == 3) //修改房间备注
            {
                ndSum.Enabled = false;
                btnOK.Text = "OK";
            }
            else if (intEditMode == 4 || intEditMode == 5) //新增全局变量  或者普通变量
            {
                ndSum.Enabled = false;
                btnOK.Text = "OK";
            }
            else if (intEditMode == 6 || intEditMode == 7) // 修改全局变量备注 或者普通变量
            {
                ndSum.Enabled = false;
                btnOK.Text = "Modify";
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (tbRmName.Text == null || tbRmName.Text == "") return;
            int intSum = Convert.ToInt16(ndSum.Value.ToString());

            CsConst.MyTmpName = new List<string>();

            if (intSum == 1)
            {
                CsConst.MyTmpName.Add(tbRmName.Text);
            }
            else
            {
                for (int intI = 0; intI < intSum; intI++)
                {
                    CsConst.MyTmpName.Add(tbRmName.Text + (intI + 1).ToString());
                }
            }
            this.Close();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            CsConst.MyTmpName = new List<string>();
            this.Close();
        }

        private void frmAdd_Load(object sender, EventArgs e)
        {

        }
    }
}
