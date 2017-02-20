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
    public partial class frmRemark : Form
    {
        private byte bytSubID = 0;
        private byte bytDevID = 0;

        public frmRemark()
        {
            InitializeComponent();
        }

        public frmRemark(byte bytSubID, byte bytDevID , string strRemark)
        {
            InitializeComponent();
            this.bytSubID = bytSubID;
            this.bytDevID = bytDevID;
            tbRmName.Text = strRemark;
            tbNew.Text = strRemark;
            tbNew.Focus();
            CsConst.MyTmpName = new List<string>();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            CsConst.MyTmpName = new List<string>();
            this.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            ((Button)sender).Enabled = false;

            byte[] ArayTmp = new byte[20];
            string strTmp = tbNew.Text;
            if (strTmp == null || strTmp == string.Empty) strTmp = "";

            if (CsConst.MyEditMode == 0) // project mode
            {                
                CsConst.MyTmpName = new List<string>();
                CsConst.MyTmpName.Add(strTmp);
            }
            else if (CsConst.MyEditMode == 1) // online mode
            {
                #region
                byte[] arayTmpRemark = HDLUDP.StringToByte(strTmp);
                if (arayTmpRemark.Length <= 20)
                {
                    Array.Copy(arayTmpRemark, 0, ArayTmp, 0, arayTmpRemark.Length);
                }
                else
                {
                    Array.Copy(arayTmpRemark, 0, ArayTmp, 0, 20);
                }

                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x0010, bytSubID, bytDevID, false, true, true,false) == true) //修改备注
                {
                    CsConst.MyTmpName = new List<string>();
                    CsConst.MyTmpName.Add(strTmp);
                }
                else
                {
                    CsConst.MyTmpName = new List<string>();
                    Cursor.Current = Cursors.Default;
                    return;
                }
                #endregion
            }
            ((Button)sender).Enabled = true;
            Cursor.Current = Cursors.Default;
            DialogResult = DialogResult.OK;
            this.Close();
        }

        private void frmRemark_Load(object sender, EventArgs e)
        {
            
        }
    }
}
