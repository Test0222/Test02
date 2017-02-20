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
    public partial class FrmEditClock : Form
    {
        private int SelectedKey = 1;
        private string strName;
        private byte SubnetID = 0;
        private byte DeviceID = 0;
        private int MyintDeviceType = 0;
        public FrmEditClock()
        {
            InitializeComponent();
        }

        public FrmEditClock(string strname,int devicetype,int selectedkey)
        {
            InitializeComponent();
            this.SelectedKey = selectedkey;
            this.MyintDeviceType = devicetype;
            this.strName = strname;
            string strDevName = strname.Split('\\')[0].ToString();
            SubnetID = Convert.ToByte(strDevName.Split('-')[0]);
            DeviceID = Convert.ToByte(strDevName.Split('-')[1]);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                byte[] arayTmp = new byte[11];
                arayTmp[0] = Convert.ToByte(SelectedKey);
                if (rb1.Checked)
                {
                    arayTmp[1] = 1;
                    arayTmp[4] = Convert.ToByte(numDate2.Value);
                    arayTmp[5] = Convert.ToByte(numDate1.Value);
                }
                else if (rb2.Checked)
                {
                    arayTmp[1] = 2;
                    for (int i = 0; i < 7; i++)
                    {
                        if (chbList.GetItemChecked(i)) arayTmp[4 + i] = 1;
                    }
                }
                else if (rb3.Checked)
                {
                    arayTmp[1] = 3;
                    arayTmp[4] = Convert.ToByte(numCustom.Value);
                }
                arayTmp[2] = Convert.ToByte(numTime1.Value);
                arayTmp[3] = Convert.ToByte(numTime2.Value);
                if (CsConst.mySends.AddBufToSndList(arayTmp, 0x1924, SubnetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(MyintDeviceType)) == true)
                {
                    
                }
                Cursor.Current = Cursors.Default;
            }
            catch
            {
            }
        }

        private void rb1_CheckedChanged(object sender, EventArgs e)
        {
            panel6.Visible = rb1.Checked;
            panel7.Visible = rb3.Checked;
            panel8.Visible = rb2.Checked;
        }

        private void FrmEditClock_Load(object sender, EventArgs e)
        {
            chbList.Items.Clear();
            chbList.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00230", ""));
            chbList.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00231", ""));
            chbList.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00232", ""));
            chbList.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00233", ""));
            chbList.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00234", ""));
            chbList.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00235", ""));
            chbList.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00236", ""));
            readClock();
        }

        private void readClock()
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                for (int i = 1; i < 7; i++) chbList.SetItemChecked(i, false);
                byte[] arayTmp = new byte[1];
                arayTmp[0] = Convert.ToByte(SelectedKey);
                if (CsConst.mySends.AddBufToSndList(arayTmp, 0x1922, SubnetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(MyintDeviceType)) == true)
                {
                    if (CsConst.myRevBuf[26] == 1)
                    {
                        rb1.Checked = true;
                        if (numDate1.Minimum <= CsConst.myRevBuf[30] &&
                        CsConst.myRevBuf[30] <= numDate1.Maximum) numDate1.Value = Convert.ToDecimal(CsConst.myRevBuf[30]);
                        if (numDate2.Minimum <= CsConst.myRevBuf[29] &&
                        CsConst.myRevBuf[29] <= numDate2.Maximum) numDate2.Value = Convert.ToDecimal(CsConst.myRevBuf[29]);
                    }
                    else if (CsConst.myRevBuf[26] == 2)
                    {
                        rb2.Checked = true;
                        for (int i = 0; i < 7; i++)
                        {
                            if (CsConst.myRevBuf[29 + i] == 1)
                                chbList.SetItemChecked(i, true);
                        }
                    }
                    else if (CsConst.myRevBuf[26] == 3)
                    {
                        rb3.Checked = true;
                        if (numCustom.Minimum <= CsConst.myRevBuf[29] &&
                        CsConst.myRevBuf[29] <= numCustom.Maximum) numCustom.Value = Convert.ToDecimal(CsConst.myRevBuf[29]);
                    }
                    if (numTime1.Minimum <= CsConst.myRevBuf[27] &&
                        CsConst.myRevBuf[27] <= numTime1.Maximum) numTime1.Value = Convert.ToDecimal(CsConst.myRevBuf[27]);
                    if (numTime2.Minimum <= CsConst.myRevBuf[28] &&
                        CsConst.myRevBuf[28] <= numTime2.Maximum) numTime2.Value = Convert.ToDecimal(CsConst.myRevBuf[28]);
                    rb1_CheckedChanged(null, null);
                }
                Cursor.Current = Cursors.Default;
                
            }
            catch
            {
            }
        }
    }
}
