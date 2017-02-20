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
    public partial class FrmHeatTargets : Form
    {
        private Panel oDLP;
        private string strName;
        private int DeviceType;
        private byte SubNetID;
        private byte DeviceID;
        public FrmHeatTargets()
        {
            InitializeComponent();
        }

        public FrmHeatTargets(string strname,int devicetype,Panel panel)
        {
            InitializeComponent();
            this.strName = strname;
            this.oDLP = panel;
            this.DeviceType = devicetype;
            string strDevName = strName.Split('\\')[0].ToString();
            SubNetID = Convert.ToByte(strDevName.Split('-')[0]);
            DeviceID = Convert.ToByte(strDevName.Split('-')[1]);
        }

        private void FrmHeatTargets_Load(object sender, EventArgs e)
        {
            lbRemarkValue.Text = strName.Split('\\')[1].ToString();
            lbSubValue.Text = SubNetID.ToString();
            lbDevValue.Text = DeviceID.ToString();
            read();
            chb1_CheckedChanged(null, null);
            chb2_CheckedChanged(null, null);
            chb3_CheckedChanged(null, null);
            chb4_CheckedChanged(null, null);
            chb5_CheckedChanged(null, null);
            chb6_CheckedChanged(null, null);
            chb7_CheckedChanged(null, null);
            chb8_CheckedChanged(null, null);
        }

        private void read()
        {
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                byte[] arayTmp = new byte[0];
                if (CsConst.mySends.AddBufToSndList(arayTmp, 0x1976, SubNetID, DeviceID, false, true, true,CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                {
                    for (int i = 1; i <= 8; i++)
                    {
                        CheckBox chb = this.Controls.Find("chb" + i.ToString(), true)[0] as CheckBox;
                        chb.Checked = (CsConst.myRevBuf[26 + (i - 1) * 4] == 1);
                        NumericUpDown tmp1 = this.Controls.Find("NumSub" + i.ToString(), true)[0] as NumericUpDown;
                        tmp1.Value = Convert.ToDecimal(CsConst.myRevBuf[27 + (i - 1) * 4]);
                        NumericUpDown tmp2 = this.Controls.Find("NumDev" + i.ToString(), true)[0] as NumericUpDown;
                        tmp2.Value = Convert.ToDecimal(CsConst.myRevBuf[28 + (i - 1) * 4]);
                        ComboBox tmp3 = this.Controls.Find("cbChn" + i.ToString(), true)[0] as ComboBox;
                        if (CsConst.myRevBuf[29 + (i - 1) * 4] <= tmp3.Items.Count) tmp3.SelectedIndex = CsConst.myRevBuf[29 + (i - 1) * 4] - 1;
                    }
                    cbTarget.Items.Clear();
                    for (int i = 1; i <= 8; i++)
                    {
                        CheckBox chb = this.Controls.Find("chb" + i.ToString(), true)[0] as CheckBox;
                        if (chb.Checked)
                        {
                            cbTarget.Items.Add(i.ToString());
                        }
                    }
                    if (cbTarget.Items.Count > 0) cbTarget.SelectedIndex = cbTarget.Items.IndexOf((CsConst.myRevBuf[25] + 1).ToString());
                    if (cbTarget.Items.Count > 0 && cbTarget.SelectedIndex < 0) cbTarget.SelectedIndex = 0;
                    
                }
            }
            catch
            { }
            Cursor.Current = Cursors.Default;
        }

        private void chb1_CheckedChanged(object sender, EventArgs e)
        {
            NumSub1.Enabled = chb1.Checked;
            NumDev1.Enabled = chb1.Checked;
            cbChn1.Enabled = chb1.Checked;
            if (chb1.Checked)
            {
                string strtmp = cbTarget.Text;
                cbTarget.Items.Clear();
                for (int i = 1; i <= 8; i++)
                {
                    CheckBox chb = this.Controls.Find("chb" + i.ToString(), true)[0] as CheckBox;
                    if (chb.Checked)
                    {
                        cbTarget.Items.Add(i.ToString());
                    }
                }
                cbTarget.SelectedIndex = cbTarget.Items.IndexOf(strtmp);
                if (cbTarget.SelectedIndex < 0) cbTarget.SelectedIndex = 0;
            }
            else
            {
                if (cbTarget.Items.Contains("1"))
                    cbTarget.Items.Remove("1");
            }
            bool isEmty = true;
            for (int i = 1; i <= 8; i++)
            {
                CheckBox chb = this.Controls.Find("chb" + i.ToString(), true)[0] as CheckBox;
                if (chb.Checked)
                {
                    isEmty = false;
                }
            }
            if (isEmty)
            {
                chb1.Checked = true;
                cbTarget.Items.Clear();
                cbTarget.Items.Add("1");
                cbTarget.SelectedIndex = 0;
            }
        }

        private void chb2_CheckedChanged(object sender, EventArgs e)
        {
            NumSub2.Enabled = chb2.Checked;
            NumDev2.Enabled = chb2.Checked;
            cbChn2.Enabled = chb2.Checked;
            if (chb1.Checked)
            {
                string strtmp = cbTarget.Text;
                cbTarget.Items.Clear();
                for (int i = 1; i <= 8; i++)
                {
                    CheckBox chb = this.Controls.Find("chb" + i.ToString(), true)[0] as CheckBox;
                    if (chb.Checked)
                    {
                        cbTarget.Items.Add(i.ToString());
                    }
                }
                cbTarget.SelectedIndex = cbTarget.Items.IndexOf(strtmp);
                if (cbTarget.SelectedIndex < 0) cbTarget.SelectedIndex = 0;
            }
            else
            {
                if (cbTarget.Items.Contains("2"))
                    cbTarget.Items.Remove("2");
            }
            bool isEmty = true;
            for (int i = 1; i <= 8; i++)
            {
                CheckBox chb = this.Controls.Find("chb" + i.ToString(), true)[0] as CheckBox;
                if (chb.Checked)
                {
                    isEmty = false;
                }
            }
            if (isEmty)
            {
                chb1.Checked = true;
                cbTarget.Items.Clear();
                cbTarget.Items.Add("1");
                cbTarget.SelectedIndex = 0;
            }
        }

        private void chb3_CheckedChanged(object sender, EventArgs e)
        {
            NumSub3.Enabled = chb3.Checked;
            NumDev3.Enabled = chb3.Checked;
            cbChn3.Enabled = chb3.Checked;
            if (chb1.Checked)
            {
                string strtmp = cbTarget.Text;
                cbTarget.Items.Clear();
                for (int i = 1; i <= 8; i++)
                {
                    CheckBox chb = this.Controls.Find("chb" + i.ToString(), true)[0] as CheckBox;
                    if (chb.Checked)
                    {
                        cbTarget.Items.Add(i.ToString());
                    }
                }
                cbTarget.SelectedIndex = cbTarget.Items.IndexOf(strtmp);
                if (cbTarget.SelectedIndex < 0) cbTarget.SelectedIndex = 0;
            }
            else
            {
                if (cbTarget.Items.Contains("3"))
                    cbTarget.Items.Remove("3");
            }
            bool isEmty = true;
            for (int i = 1; i <= 8; i++)
            {
                CheckBox chb = this.Controls.Find("chb" + i.ToString(), true)[0] as CheckBox;
                if (chb.Checked)
                {
                    isEmty = false;
                }
            }
            if (isEmty)
            {
                chb1.Checked = true;
                cbTarget.Items.Clear();
                cbTarget.Items.Add("1");
                cbTarget.SelectedIndex = 0;
            }
        }

        private void chb4_CheckedChanged(object sender, EventArgs e)
        {
            NumSub4.Enabled = chb4.Checked;
            NumDev4.Enabled = chb4.Checked;
            cbChn4.Enabled = chb4.Checked;
            if (chb1.Checked)
            {
                string strtmp = cbTarget.Text;
                cbTarget.Items.Clear();
                for (int i = 1; i <= 8; i++)
                {
                    CheckBox chb = this.Controls.Find("chb" + i.ToString(), true)[0] as CheckBox;
                    if (chb.Checked)
                    {
                        cbTarget.Items.Add(i.ToString());
                    }
                }
                cbTarget.SelectedIndex = cbTarget.Items.IndexOf(strtmp);
                if (cbTarget.SelectedIndex < 0) cbTarget.SelectedIndex = 0;
            }
            else
            {
                if (cbTarget.Items.Contains("4"))
                    cbTarget.Items.Remove("4");
            }
            bool isEmty = true;
            for (int i = 1; i <= 8; i++)
            {
                CheckBox chb = this.Controls.Find("chb" + i.ToString(), true)[0] as CheckBox;
                if (chb.Checked)
                {
                    isEmty = false;
                }
            }
            if (isEmty)
            {
                chb1.Checked = true;
                cbTarget.Items.Clear();
                cbTarget.Items.Add("1");
                cbTarget.SelectedIndex = 0;
            }
        }

        private void chb5_CheckedChanged(object sender, EventArgs e)
        {
            NumSub5.Enabled = chb5.Checked;
            NumDev5.Enabled = chb5.Checked;
            cbChn5.Enabled = chb5.Checked;
            if (chb1.Checked)
            {
                string strtmp = cbTarget.Text;
                cbTarget.Items.Clear();
                for (int i = 1; i <= 8; i++)
                {
                    CheckBox chb = this.Controls.Find("chb" + i.ToString(), true)[0] as CheckBox;
                    if (chb.Checked)
                    {
                        cbTarget.Items.Add(i.ToString());
                    }
                }
                cbTarget.SelectedIndex = cbTarget.Items.IndexOf(strtmp);
                if (cbTarget.SelectedIndex < 0) cbTarget.SelectedIndex = 0;
            }
            else
            {
                if (cbTarget.Items.Contains("5"))
                    cbTarget.Items.Remove("5");
            }
            bool isEmty = true;
            for (int i = 1; i <= 8; i++)
            {
                CheckBox chb = this.Controls.Find("chb" + i.ToString(), true)[0] as CheckBox;
                if (chb.Checked)
                {
                    isEmty = false;
                }
            }
            if (isEmty)
            {
                chb1.Checked = true;
                cbTarget.Items.Clear();
                cbTarget.Items.Add("1");
                cbTarget.SelectedIndex = 0;
            }
        }

        private void chb6_CheckedChanged(object sender, EventArgs e)
        {
            NumSub6.Enabled = chb6.Checked;
            NumDev6.Enabled = chb6.Checked;
            cbChn6.Enabled = chb6.Checked;
            if (chb1.Checked)
            {
                string strtmp = cbTarget.Text;
                cbTarget.Items.Clear();
                for (int i = 1; i <= 8; i++)
                {
                    CheckBox chb = this.Controls.Find("chb" + i.ToString(), true)[0] as CheckBox;
                    if (chb.Checked)
                    {
                        cbTarget.Items.Add(i.ToString());
                    }
                }
                cbTarget.SelectedIndex = cbTarget.Items.IndexOf(strtmp);
                if (cbTarget.SelectedIndex < 0) cbTarget.SelectedIndex = 0;
            }
            else
            {
                if (cbTarget.Items.Contains("6"))
                    cbTarget.Items.Remove("6");
            }
            bool isEmty = true;
            for (int i = 1; i <= 8; i++)
            {
                CheckBox chb = this.Controls.Find("chb" + i.ToString(), true)[0] as CheckBox;
                if (chb.Checked)
                {
                    isEmty = false;
                }
            }
            if (isEmty)
            {
                chb1.Checked = true;
                cbTarget.Items.Clear();
                cbTarget.Items.Add("1");
                cbTarget.SelectedIndex = 0;
            }
        }

        private void chb7_CheckedChanged(object sender, EventArgs e)
        {
            NumSub7.Enabled = chb7.Checked;
            NumDev7.Enabled = chb7.Checked;
            cbChn7.Enabled = chb7.Checked;
            if (chb1.Checked)
            {
                string strtmp = cbTarget.Text;
                cbTarget.Items.Clear();
                for (int i = 1; i <= 8; i++)
                {
                    CheckBox chb = this.Controls.Find("chb" + i.ToString(), true)[0] as CheckBox;
                    if (chb.Checked)
                    {
                        cbTarget.Items.Add(i.ToString());
                    }
                }
                cbTarget.SelectedIndex = cbTarget.Items.IndexOf(strtmp);
                if (cbTarget.SelectedIndex < 0) cbTarget.SelectedIndex = 0;
            }
            else
            {
                if (cbTarget.Items.Contains("7"))
                    cbTarget.Items.Remove("7");
            }
            bool isEmty = true;
            for (int i = 1; i <= 8; i++)
            {
                CheckBox chb = this.Controls.Find("chb" + i.ToString(), true)[0] as CheckBox;
                if (chb.Checked)
                {
                    isEmty = false;
                }
            }
            if (isEmty)
            {
                chb1.Checked = true;
                cbTarget.Items.Clear();
                cbTarget.Items.Add("1");
                cbTarget.SelectedIndex = 0;
            }
        }

        private void chb8_CheckedChanged(object sender, EventArgs e)
        {
            NumSub8.Enabled = chb8.Checked;
            NumDev8.Enabled = chb8.Checked;
            cbChn8.Enabled = chb8.Checked;
            if (chb1.Checked)
            {
                string strtmp = cbTarget.Text;
                cbTarget.Items.Clear();
                for (int i = 1; i <= 8; i++)
                {
                    CheckBox chb = this.Controls.Find("chb" + i.ToString(), true)[0] as CheckBox;
                    if (chb.Checked)
                    {
                        cbTarget.Items.Add(i.ToString());
                    }
                }
                cbTarget.SelectedIndex = cbTarget.Items.IndexOf(strtmp);
                if (cbTarget.SelectedIndex < 0) cbTarget.SelectedIndex = 0;
            }
            else
            {
                if (cbTarget.Items.Contains("8"))
                    cbTarget.Items.Remove("8");
            }
            bool isEmty = true;
            for (int i = 1; i <= 8; i++)
            {
                CheckBox chb = this.Controls.Find("chb" + i.ToString(), true)[0] as CheckBox;
                if (chb.Checked)
                {
                    isEmty = false;
                }
            }
            if (isEmty)
            {
                chb1.Checked = true;
                cbTarget.Items.Clear();
                cbTarget.Items.Add("1");
                cbTarget.SelectedIndex = 0;
            }
        }

        private void cbTarget_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                byte[] arayTmp = new byte[33];
                arayTmp[0] = Convert.ToByte(Convert.ToInt32(cbTarget.Text)-1);
                for (int i = 1; i <= 8; i++)
                {
                    CheckBox chb = this.Controls.Find("chb" + i.ToString(), true)[0] as CheckBox;
                    if (chb.Checked)
                        arayTmp[1 + (i - 1) * 4] = 1;
                    NumericUpDown tmp1 = this.Controls.Find("NumSub" + i.ToString(), true)[0] as NumericUpDown;
                    arayTmp[2 + (i - 1) * 4] = Convert.ToByte(tmp1.Value);
                    NumericUpDown tmp2 = this.Controls.Find("NumDev" + i.ToString(), true)[0] as NumericUpDown;
                    arayTmp[3 + (i - 1) * 4] = Convert.ToByte(tmp2.Value);
                    ComboBox tmp3 = this.Controls.Find("cbChn" + i.ToString(), true)[0] as ComboBox;
                    if (tmp3.SelectedIndex >= 0)
                        arayTmp[4 + (i - 1) * 4] = Convert.ToByte(tmp3.SelectedIndex + 1);
                    else
                        arayTmp[4 + (i - 1) * 4] = 1;
                }
                if (CsConst.mySends.AddBufToSndList(arayTmp, 0x1978, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                {
                    
                }
            }
            catch
            {
            }
        }

    }
}
