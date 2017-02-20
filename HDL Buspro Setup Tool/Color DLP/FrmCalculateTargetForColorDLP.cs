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
    public partial class FrmCalculateTargetForColorDLP : Form
    {
        private EnviroFH CurrentActiveFh;
        private string strName;
        private int DeviceType;
        private byte SubNetID;
        private byte DeviceID;
        private int SelectIndex;
        public FrmCalculateTargetForColorDLP()
        {
            InitializeComponent();
        }

        public FrmCalculateTargetForColorDLP(string strname, int devicetype, Object panel,int select)
        {
            InitializeComponent();
            this.strName = strname;
            if (panel is ColorDLP)
            {
                this.CurrentActiveFh =((ColorDLP)panel).MyHeat[select];
            }
            else if (panel is EnviroFH)
            {
                this.CurrentActiveFh = ((EnviroPanel)panel).MyHeat[select];
            }
            this.DeviceType = devicetype;
            string strDevName = strName.Split('\\')[0].ToString();
            SubNetID = Convert.ToByte(strDevName.Split('-')[0]);
            DeviceID = Convert.ToByte(strDevName.Split('-')[1]);
            SelectIndex = select;
        }

        private void FrmCalculateTarget_Load(object sender, EventArgs e)
        {
            for (int i = 1; i <= 8; i++)
            {
                ComboBox cb = this.Controls.Find("cb" + i.ToString(), true)[0] as ComboBox;
                cb.Items.Clear();
                cb.Items.Add(CsConst.WholeTextsList[1775].sDisplayName);
                cb.Items.Add(CsConst.mstrINIDefault.IniReadValue("public", "00042", ""));
            }
                lbRemarkValue.Text = strName.Split('\\')[1].ToString();
            lbSubValue.Text = SubNetID.ToString();
            lbDevValue.Text = DeviceID.ToString();
            cbHeat_SelectedIndexChanged(null, null);
            panel5.Visible = EnviroDLPPanelDeviceTypeList.HDLEnviroDLPPanelDeviceTypeList.Contains(DeviceType);
        }

        private void read()
        {
            if (cbHeat.SelectedIndex == -1) cbHeat.SelectedIndex = 0;
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                byte[] arayTmp = new byte[1];
                arayTmp[0] = Convert.ToByte(cbHeat.SelectedIndex);
                if (CsConst.mySends.AddBufToSndList(arayTmp, 0x1972, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                {
                    int intTmp = CsConst.myRevBuf[57];
                    string strTmp = GlobalClass.IntToBit(intTmp, 8);
                    for (int i = 1; i <= 8; i++)
                    {
                        ComboBox cb = this.Controls.Find("cb" + i.ToString(), true)[0] as ComboBox;
                        if (CsConst.myRevBuf[25 + (i - 1) * 4] <= 1)
                            cb.SelectedIndex = CsConst.myRevBuf[25 + (i - 1) * 4];
                        NumericUpDown tmp1 = this.Controls.Find("NumSub" + i.ToString(), true)[0] as NumericUpDown;
                        tmp1.Value = Convert.ToDecimal(CsConst.myRevBuf[26 + (i - 1) * 4]);
                        NumericUpDown tmp2 = this.Controls.Find("NumDev" + i.ToString(), true)[0] as NumericUpDown;
                        tmp2.Value = Convert.ToDecimal(CsConst.myRevBuf[27 + (i - 1) * 4]);
                        NumericUpDown tmp3 = this.Controls.Find("NumChn" + i.ToString(), true)[0] as NumericUpDown;
                        tmp3.Value = Convert.ToDecimal(CsConst.myRevBuf[28 + (i - 1) * 4]);
                        CheckBox chb = this.Controls.Find("chb" + i.ToString(), true)[0] as CheckBox;
                        if (strTmp.Substring((8 - i), 1) == "0") chb.Checked = true;
                        else chb.Checked = false;
                    }

                    CsConst.myRevBuf = new byte[1200];
                }
            }
            catch
            { }
            Cursor.Current = Cursors.Default;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                byte[] arayTmp = new byte[34];
                string strTmp = "";
                for (int i = 1; i <= 8; i++)
                {
                    ComboBox cb = this.Controls.Find("cb" + i.ToString(), true)[0] as ComboBox;
                    if (cb.SelectedIndex >= 0)
                        arayTmp[(i - 1) * 4]=Convert.ToByte(cb.SelectedIndex);
                    NumericUpDown tmp1 = this.Controls.Find("NumSub" + i.ToString(), true)[0] as NumericUpDown;
                    arayTmp[(i - 1) * 4 + 1] = Convert.ToByte(tmp1.Value);
                    NumericUpDown tmp2 = this.Controls.Find("NumDev" + i.ToString(), true)[0] as NumericUpDown;
                    arayTmp[(i - 1) * 4 + 2] = Convert.ToByte(tmp2.Value);
                    NumericUpDown tmp3 = this.Controls.Find("NumChn" + i.ToString(), true)[0] as NumericUpDown;
                    arayTmp[(i - 1) * 4 + 3] = Convert.ToByte(tmp3.Value);
                    CheckBox chb = this.Controls.Find("chb" + i.ToString(), true)[0] as CheckBox;
                    if (chb.Checked) strTmp = "0" + strTmp;
                    else strTmp = "1" + strTmp;
                }
                arayTmp[32] = Convert.ToByte(GlobalClass.BitToInt(strTmp));
                arayTmp[33] = Convert.ToByte(0);
                if (CsConst.mySends.AddBufToSndList(arayTmp, 0x1974, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                {
                    CsConst.myRevBuf = new byte[1200];
                }
            }
            catch
            {
            }
        }

        private void cbHeat_SelectedIndexChanged(object sender, EventArgs e)
        {
            read();
        }

        private void cb1_SelectedIndexChanged(object sender, EventArgs e)
        {
            NumSub1.Enabled = (cb1.SelectedIndex == 1);
            NumDev1.Enabled = (cb1.SelectedIndex == 1);
            NumChn1.Enabled = (cb1.SelectedIndex == 1);
            chb1.Enabled = (cb1.SelectedIndex == 1);
        }

        private void cb2_SelectedIndexChanged(object sender, EventArgs e)
        {
            NumSub2.Enabled = (cb2.SelectedIndex == 1);
            NumDev2.Enabled = (cb2.SelectedIndex == 1);
            NumChn2.Enabled = (cb2.SelectedIndex == 1);
            chb2.Enabled = (cb2.SelectedIndex == 1);
        }

        private void cb3_SelectedIndexChanged(object sender, EventArgs e)
        {
            NumSub3.Enabled = (cb3.SelectedIndex == 1);
            NumDev3.Enabled = (cb3.SelectedIndex == 1);
            NumChn3.Enabled = (cb3.SelectedIndex == 1);
            chb3.Enabled = (cb3.SelectedIndex == 1);
        }

        private void cb4_SelectedIndexChanged(object sender, EventArgs e)
        {
            NumSub4.Enabled = (cb4.SelectedIndex == 1);
            NumDev4.Enabled = (cb4.SelectedIndex == 1);
            NumChn4.Enabled = (cb4.SelectedIndex == 1);
            chb4.Enabled = (cb4.SelectedIndex == 1);
        }

        private void cb5_SelectedIndexChanged(object sender, EventArgs e)
        {
            NumSub5.Enabled = (cb5.SelectedIndex == 1);
            NumDev5.Enabled = (cb5.SelectedIndex == 1);
            NumChn5.Enabled = (cb5.SelectedIndex == 1);
            chb5.Enabled = (cb5.SelectedIndex == 1);
        }

        private void cb6_SelectedIndexChanged(object sender, EventArgs e)
        {
            NumSub6.Enabled = (cb6.SelectedIndex == 1);
            NumDev6.Enabled = (cb6.SelectedIndex == 1);
            NumChn6.Enabled = (cb6.SelectedIndex == 1);
            chb6.Enabled = (cb6.SelectedIndex == 1);
        }

        private void cb7_SelectedIndexChanged(object sender, EventArgs e)
        {
            NumSub7.Enabled = (cb7.SelectedIndex == 1);
            NumDev7.Enabled = (cb7.SelectedIndex == 1);
            NumChn7.Enabled = (cb7.SelectedIndex == 1);
            chb7.Enabled = (cb7.SelectedIndex == 1);
        }

        private void cb8_SelectedIndexChanged(object sender, EventArgs e)
        {
            NumSub8.Enabled = (cb8.SelectedIndex == 1);
            NumDev8.Enabled = (cb8.SelectedIndex == 1);
            NumChn8.Enabled = (cb8.SelectedIndex == 1);
            chb8.Enabled = (cb8.SelectedIndex == 1);
        }
    }
}
