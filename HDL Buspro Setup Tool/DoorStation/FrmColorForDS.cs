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
    public partial class FrmColorForDS : Form
    {
        private NewDS oNewDS;
        private DS oDS;
        private int Index;
        private object MyActiveObj;
        public FrmColorForDS()
        {
            InitializeComponent();
        }
        public FrmColorForDS(int tag,object obj,int index)
        {
            InitializeComponent();
            MyActiveObj = obj;
            oNewDS = null;
            oDS = null;
            if (MyActiveObj is DS)
            {
                if (CsConst.myDS != null)
                {
                    foreach (DS oTmp in CsConst.myDS)
                    {
                        if (oTmp.DIndex == (MyActiveObj as DS).DIndex)
                        {
                            oDS = oTmp;
                            break;
                        }
                    }
                }
            }
            else if (MyActiveObj is NewDS)
            {
                if (CsConst.myNewDS != null)
                {
                    foreach (NewDS oTmp in CsConst.myNewDS)
                    {
                        if (oTmp.DIndex == (MyActiveObj as NewDS).DIndex)
                        {
                            oNewDS = oTmp;
                            break;
                        }
                    }
                }
            }
            Tag = tag;
            Index = index;
        }


        private void FrmColorForDS_Load(object sender, EventArgs e)
        {
            if (CsConst.iLanguageId == 1)
            {
                btnOK.Text = "确定";
                btnCancle.Text = "取消";
            }
            System.Windows.Forms.Panel temp = this.Controls.Find("pnlC" + Tag.ToString(), true)[0] as System.Windows.Forms.Panel;
            PIC.Left = temp.Left + 20;
        }

        private void btnCancle_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void panel3_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Panel temp = (sender as System.Windows.Forms.Panel);
            PIC.Left = temp.Left + 20;
            Tag = Convert.ToInt32(temp.Tag);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (oNewDS != null)
                oNewDS.arayBasic[Index] = Convert.ToByte(Tag);
            else if(oDS!=null)
                oDS.arayBasic[Index] = Convert.ToByte(Tag);
            DialogResult = DialogResult.OK;
            this.Close();
        }

        private void pnlC1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            btnOK_Click(sender, null);
        }
    }
}
