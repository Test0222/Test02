using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HDL_Buspro_Setup_Tool
{
    public partial class CurtainForColorDLP : UserControl
    {
        private ColorDLP oColorDLP = null;
        private int CurtainID = 0;
        private object MyActiveObj;
        private string strName;
        private int PageID;
        private bool isReadingData = false;
        private byte SubnetID;
        private byte DeviceID;
        private int Index;
        public CurtainForColorDLP()
        {
            InitializeComponent();
        }
        public CurtainForColorDLP(object obj,int curtainid,string strname,int pageid,int index)
        {
            isReadingData = true;
            this.MyActiveObj = obj;
            this.CurtainID = curtainid;
            this.strName = strname;
            this.PageID = pageid;
            this.Index=index;
            string strDevName = strName.Split('\\')[0].ToString();
            SubnetID = Convert.ToByte(strDevName.Split('-')[0]);
            DeviceID = Convert.ToByte(strDevName.Split('-')[1]);
            InitializeComponent();
            oColorDLP = null;
            if (MyActiveObj is ColorDLP)
            {
                if (CsConst.myColorPanels != null)
                {
                    foreach (ColorDLP oTmp in CsConst.myColorPanels)
                    {
                        if (oTmp.DIndex == Index)
                        {
                            oColorDLP = oTmp;
                            break;
                        }
                    }
                }
            }

            cbMode.Items.Clear();
            cbMode.Items.Add(CsConst.WholeTextsList[1775].sDisplayName);
            cbMode.Items.Add(CsConst.mstrINIDefault.IniReadValue("CMDType", "00007", ""));
            cbSwitch.Items.Clear();
            cbSwitch.Items.Add(CsConst.mstrINIDefault.IniReadValue("public", "00036", ""));
            cbSwitch.Items.Add(CsConst.mstrINIDefault.IniReadValue("public", "00037", ""));
            cbSwitch.Items.Add(CsConst.mstrINIDefault.IniReadValue("public", "00038", ""));
            for (int i = 1; i <= 100; i++)
                cbSwitch.Items.Add(i.ToString() + "%");
            for (int i = 0; i < oColorDLP.MyCurtain.Count; i++)
            {
                if (oColorDLP.MyCurtain[i].PageID == Convert.ToByte(PageID) &&
                    oColorDLP.MyCurtain[i].KeyNo == Convert.ToByte(CurtainID))
                {
                    ColorDLP.Curtain temp = oColorDLP.MyCurtain[i];
                    txtRemark.Text = temp.Remark;
                    NumSub.Value = Convert.ToDecimal(SubnetID);
                    NumDev.Value = Convert.ToDecimal(DeviceID);
                    if (temp.Mode <= 1) cbMode.SelectedIndex = temp.Mode;
                    txtCurtaiNum.Text = temp.CurtainNo.ToString();
                    if (temp.CurtainSwitch <= 100)
                    {
                        if (temp.CurtainNo >= 17)
                        {
                            cbSwitch.SelectedIndex = temp.CurtainSwitch + 2;
                            txtCurtaiNum.Text = (temp.CurtainNo - 16).ToString();
                        }
                        else
                            cbSwitch.SelectedIndex = temp.CurtainSwitch;
                    }
                    break;
                }
            }
            this.groupBox1.Text = CsConst.mstrINIDefault.IniReadValue("public", "99882", "") + "-" + CurtainID.ToString();
            if (CsConst.iLanguageId == 1)
            {
                lbRemark.Text = CsConst.mstrINIDefault.IniReadValue("CurtainForColorDLP", "00000", "");
                lbSubnetID.Text = CsConst.mstrINIDefault.IniReadValue("CurtainForColorDLP", "00001", "");
                lbDeviceID.Text = CsConst.mstrINIDefault.IniReadValue("CurtainForColorDLP", "00002", "");
                lbMode.Text = CsConst.mstrINIDefault.IniReadValue("CurtainForColorDLP", "00003", "");
                lbCurtainNum.Text = CsConst.mstrINIDefault.IniReadValue("CurtainForColorDLP", "00004", "");
                lbSwitch.Text = CsConst.mstrINIDefault.IniReadValue("CurtainForColorDLP", "00005", "");
            }
            isReadingData = false;
        }

        private void cbMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isReadingData) return;
            setData();
        }

        private void txtRemark_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (isReadingData) return;
                for (int i = 0; i < oColorDLP.MyCurtain.Count; i++)
                {
                    if (oColorDLP.MyCurtain[i].PageID == Convert.ToByte(PageID) &&
                        oColorDLP.MyCurtain[i].KeyNo == Convert.ToByte(CurtainID))
                    {
                        string strRemark = txtRemark.Text;
                        oColorDLP.MyCurtain[i].Remark = strRemark;
                        break;
                    }
                }
            }
            catch
            {
            }
        }

        private void NumSub_ValueChanged(object sender, EventArgs e)
        {
            if (isReadingData) return;
            setData();
        }

        private void NumDev_ValueChanged(object sender, EventArgs e)
        {
            if (isReadingData) return;
            setData();
        }

        private void txtCurtaiNum_TextChanged(object sender, EventArgs e)
        {
            if (isReadingData) return;
            setData();
        }

        private void setData()
        {
            try
            {
                byte[] ArayTmp = new byte[12];
                ArayTmp[0] = Convert.ToByte(CurtainID);
                ArayTmp[1] = 1;
                if (cbMode.SelectedIndex == 0) ArayTmp[2] = 0;
                else if (cbMode.SelectedIndex == 1) ArayTmp[2] = 92;
                ArayTmp[3] = Convert.ToByte(NumSub.Value);
                ArayTmp[4] = Convert.ToByte(NumDev.Value);
                ArayTmp[5] = Convert.ToByte(txtCurtaiNum.Text);
                if (cbSwitch.SelectedIndex >= 0)
                {
                    if (cbSwitch.SelectedIndex > 2)
                    {
                        ArayTmp[6] = Convert.ToByte(cbSwitch.SelectedIndex - 2);
                        ArayTmp[5] = Convert.ToByte(ArayTmp[5] + 16);
                    }
                    else ArayTmp[6] = Convert.ToByte(cbSwitch.SelectedIndex);
                }
                for (int i = 0; i < oColorDLP.MyCurtain.Count; i++)
                {
                    if (oColorDLP.MyCurtain[i].PageID == Convert.ToByte(PageID) &&
                        oColorDLP.MyCurtain[i].KeyNo == Convert.ToByte(CurtainID))
                    {
                        oColorDLP.MyCurtain[i].Mode = ArayTmp[2];
                        oColorDLP.MyCurtain[i].SubnetID = ArayTmp[3];
                        oColorDLP.MyCurtain[i].DeviceID = ArayTmp[4];
                        oColorDLP.MyCurtain[i].CurtainNo = ArayTmp[5];
                        oColorDLP.MyCurtain[i].CurtainSwitch = ArayTmp[6];
                        break;
                    }
                }
            }
            catch
            {
            }
        }

        private void cbSwitch_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isReadingData) return;
            setData();
        }

        private void txtCurtaiNum_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar) || e.KeyChar == 8)
                e.Handled = false;
            else
                e.Handled = true;
        }

        private void btnSaveAC_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                btnSaveAC.Enabled = false;
                string strRemark = txtRemark.Text;
                byte[] ArayTmp = new byte[24];
                ArayTmp[0] = Convert.ToByte(CurtainID);
                byte[] arayTmpRemark = HDLUDP.StringToByte(strRemark);
                if (arayTmpRemark.Length > 20)
                {
                    Array.Copy(arayTmpRemark, 0, ArayTmp, 1, 20);
                }
                else
                {
                    Array.Copy(arayTmpRemark, 0, ArayTmp, 1, arayTmpRemark.Length);
                }
                for (int i = 0; i < oColorDLP.MyCurtain.Count; i++)
                {
                    if (oColorDLP.MyCurtain[i].PageID == Convert.ToByte(PageID) &&
                        oColorDLP.MyCurtain[i].KeyNo == Convert.ToByte(CurtainID))
                    {
                        ArayTmp[21] = oColorDLP.MyCurtain[i].CurtainType;
                        break;
                    }
                }
                ArayTmp[22] = Convert.ToByte(PageID);
                ArayTmp[23] = 12;
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xE006, SubnetID, DeviceID, false, true, true, false) == true)
                {
                    for (int i = 0; i < oColorDLP.MyCurtain.Count; i++)
                    {
                        if (oColorDLP.MyCurtain[i].PageID == Convert.ToByte(PageID) &&
                            oColorDLP.MyCurtain[i].KeyNo == Convert.ToByte(CurtainID))
                        {
                            oColorDLP.MyCurtain[i].Remark = strRemark;
                            break;
                        }
                    }
                    CsConst.myRevBuf = new byte[1200];
                }
                else
                {
                    btnSaveAC.Enabled = true;
                    Cursor.Current = Cursors.Default;
                    return;
                }
                ArayTmp = new byte[12];
                ArayTmp[0] = Convert.ToByte(CurtainID);
                ArayTmp[1] = 1;
                if (cbMode.SelectedIndex == 0) ArayTmp[2] = 0;
                else if (cbMode.SelectedIndex == 1) ArayTmp[2] = 92;
                ArayTmp[3] = Convert.ToByte(NumSub.Value);
                ArayTmp[4] = Convert.ToByte(NumDev.Value);
                ArayTmp[5] = Convert.ToByte(txtCurtaiNum.Text);
                if (cbSwitch.SelectedIndex >= 0)
                {
                    if (cbSwitch.SelectedIndex > 2)
                    {
                        ArayTmp[6] = Convert.ToByte(cbSwitch.SelectedIndex - 2);
                        ArayTmp[5] = Convert.ToByte(ArayTmp[5] + 16);
                    }
                    else ArayTmp[6] = Convert.ToByte(cbSwitch.SelectedIndex);
                }
                for (int i = 0; i < oColorDLP.MyCurtain.Count; i++)
                {
                    if (oColorDLP.MyCurtain[i].PageID == Convert.ToByte(PageID) &&
                        oColorDLP.MyCurtain[i].KeyNo == Convert.ToByte(CurtainID))
                    {
                        ArayTmp[9] = oColorDLP.MyCurtain[i].CurtainType;
                        break;
                    }
                }
                if (PageID == 5)
                    ArayTmp[10] = 0;
                else
                    ArayTmp[10] = Convert.ToByte(PageID);
                ArayTmp[11] = 12;
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xE002, SubnetID, DeviceID, false, true, true,false) == true)
                {
                    for (int i = 0; i < oColorDLP.MyCurtain.Count; i++)
                    {
                        if (oColorDLP.MyCurtain[i].PageID == Convert.ToByte(PageID) &&
                            oColorDLP.MyCurtain[i].KeyNo == Convert.ToByte(CurtainID))
                        {
                            oColorDLP.MyCurtain[i].Mode = ArayTmp[2];
                            oColorDLP.MyCurtain[i].SubnetID = ArayTmp[3];
                            oColorDLP.MyCurtain[i].DeviceID = ArayTmp[4];
                            oColorDLP.MyCurtain[i].CurtainNo = ArayTmp[5];
                            oColorDLP.MyCurtain[i].CurtainSwitch = ArayTmp[6];
                            break;
                        }
                    }
                    CsConst.myRevBuf = new byte[1200];
                }
                else
                {
                    btnSaveAC.Enabled = true;
                    Cursor.Current = Cursors.Default;
                    return;
                }

                ArayTmp = new byte[15];
                if (PageID < 5)
                {
                    ArayTmp[0] = 2;
                    ArayTmp[1] = Convert.ToByte(PageID);
                    for (int i = 3; i < 7; i++) ArayTmp[i] = 1;
                    ArayTmp[7] = 4;
                    ArayTmp[8] = 5;
                }
                else if (PageID == 5)
                {
                    ArayTmp[0] = 3;
                    ArayTmp[1] = 0;
                    for (int i = 3; i < 5; i++) ArayTmp[i] = 1;
                    ArayTmp[5] = 4;
                    ArayTmp[6] = 5;
                }
                ArayTmp[2] = 12;

                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x19A0, SubnetID, DeviceID, false, true, true,false) == true)
                {
                }
                btnSaveAC.Enabled = true;
                Cursor.Current = Cursors.Default;
            }
            catch
            {
                btnSaveAC.Enabled = true;
                Cursor.Current = Cursors.Default;
            }
        }
    }
}
