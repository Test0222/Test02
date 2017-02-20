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
    public partial class FrmCalibrationLux : Form
    {
        private byte SubNetID;
        private byte DeviceID;
        public FrmCalibrationLux()
        {
            InitializeComponent();
        }

        public FrmCalibrationLux(String strname,int DeviceType)
        {
            InitializeComponent();
            string strDevName = strname.Split('\\')[0].ToString();
            SubNetID = Convert.ToByte(strDevName.Split('-')[0]);
            DeviceID = Convert.ToByte(strDevName.Split('-')[1]);
        }

        private void pic2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pic1_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            byte[] arayTmp = new byte[2];
            int intLux = Convert.ToInt32(txtLux.Text);
            arayTmp[0] = Convert.ToByte(intLux / 256);
            arayTmp[1] = Convert.ToByte(intLux % 256);
            if (CsConst.mySends.AddBufToSndList(arayTmp, 0x16B1, SubNetID, DeviceID, false, false, true,true) == true)
            {
                if (CsConst.myRevBuf[25] == 0xF8)
                {
                    MessageBox.Show(CsConst.mstrINIDefault.IniReadValue("Public", "99608", ""),
                                                     ""
                                                     , MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                    
                }
                else if (CsConst.myRevBuf[25] == 0xF5)
                {
                   MessageBox.Show(CsConst.mstrINIDefault.IniReadValue("Public", "99607", ""),
                                                ""
                                                , MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                    
                }
                
            }
            else
            {
                MessageBox.Show(CsConst.mstrINIDefault.IniReadValue("Public", "99607", ""),
                                                 ""
                                                 , MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
            }
            Cursor.Current = Cursors.Default;
        }

        private void txtLux_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar) || e.KeyChar == 8)
                e.Handled = false;
            else
                e.Handled = true;
        }

        private void txtLux_Leave(object sender, EventArgs e)
        {
            string str = txtLux.Text;
            txtLux.Text = HDLPF.IsNumStringMode(str, 0, 100);
            txtLux.SelectionStart = txtLux.Text.Length;
        }
    }
}
