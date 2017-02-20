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
    public partial class FrmBoundRate : Form
    {
        private byte SubNetID;
        private byte DeviceID;
        private MHRCU sTmpMhrcu;
        private RS232 sTmpRs232;
        private bool is485Port;
        public FrmBoundRate()
        {
            InitializeComponent();
        }
        public FrmBoundRate(byte subnetid, byte deviceid, Object rs232, bool is485)
        {
            this.SubNetID = subnetid;
            this.DeviceID = deviceid;

            if (rs232 is MHRCU) sTmpMhrcu = (MHRCU)rs232;
            else if (rs232 is RS232) sTmpRs232 = (RS232)rs232;
            
            this.is485Port = is485;
            InitializeComponent();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (cbRate.SelectedIndex == -1) return;
            if (cbBit.SelectedIndex == -1) return;
            try
            {
                btnSave.Enabled = false;
                Cursor.Current = Cursors.WaitCursor;
                byte[] arayTmp = new byte[2];
                arayTmp[0] = Convert.ToByte(cbRate.SelectedIndex);
                arayTmp[1] = Convert.ToByte(cbBit.SelectedIndex + 1);
                if (!is485Port)
                {
                    if (CsConst.mySends.AddBufToSndList(arayTmp, 0xE41E, SubNetID, DeviceID, false, true, true, false) == true)
                    {
                        sTmpRs232.byt232Paut = arayTmp[0];
                        sTmpRs232.byt232Paity = arayTmp[1];
                    }
                }
                else
                {
                    if (CsConst.mySends.AddBufToSndList(arayTmp, 0xDA5F, SubNetID, DeviceID, false, true, true, false) == true)
                    {

                    }
                }
            }
            catch { }
            Cursor.Current = Cursors.Default;
            btnSave.Enabled = true;
        }

        private void FrmBoundRate_Load(object sender, EventArgs e)
        {
            cbBit.Items.Clear();
            for (int i = 0; i < 6; i++)
                cbBit.Items.Add(CsConst.mstrINIDefault.IniReadValue("public", "0019" + i.ToString(), ""));
            btnRead_Click(null, null);
        }

        private void btnRead_Click(object sender, EventArgs e)
        {
            try
            {
                btnRead.Enabled = false;
                Cursor.Current = Cursors.WaitCursor;
                byte[] arayTmp = new byte[0];
                if (!is485Port)
                {
                    if (CsConst.mySends.AddBufToSndList(arayTmp, 0xE41C, SubNetID, DeviceID, false, true, true, false) == true)
                    {
                        cbRate.SelectedIndex = CsConst.myRevBuf[26];
                        cbBit.SelectedIndex = CsConst.myRevBuf[27] - 1;
                        
                    }
                }
                else
                {
                    if (CsConst.mySends.AddBufToSndList(arayTmp, 0xDA5D, SubNetID, DeviceID, false, true, true, false) == true)
                    {
                        cbRate.SelectedIndex = CsConst.myRevBuf[26];
                        cbBit.SelectedIndex = CsConst.myRevBuf[27] - 1;
                        
                    }
                }
            }
            catch
            {
            }
            Cursor.Current = Cursors.Default;
            btnRead.Enabled = true;
        }
    }
}
