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
    public partial class FrmACSetupForRCU : Form
    {
        private MHRCU oMHRCU;
        private string strName;
        private int DeviceType;
        private byte SubNetID;
        private byte DeviceID;
        private byte bytTempType;
        private byte[] araTemperature = new byte[11];
        public FrmACSetupForRCU()
        {
            InitializeComponent();
        }

        public FrmACSetupForRCU(string strname, MHRCU mhrcu, int devicetype)
        {
            InitializeComponent();
            this.strName = strname;
            this.oMHRCU = mhrcu;
            this.DeviceType = devicetype;
            this.Text = strName;
            string strDevName = strName.Split('\\')[0].ToString();
            SubNetID = Convert.ToByte(strDevName.Split('-')[0]);
            DeviceID = Convert.ToByte(strDevName.Split('-')[1]);
        }

        private void FrmACSetup_Load(object sender, EventArgs e)
        {
            setValue();
        }

        private void setValue()
        {
            try
            {
                if (tabControl1.SelectedIndex == 0)
                {
                    byte[] ArayTmp = new byte[0];

                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xE120, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                    {
                        if (CsConst.myRevBuf[25] <= 1)
                            cbTemp.SelectedIndex = CsConst.myRevBuf[25];
                        
                        HDLUDP.TimeBetwnNext(1);
                    }
                    else
                    {
                        Cursor.Current = Cursors.Default;
                        return;
                    }

                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x190F, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)  // read power saving or wind
                    {
                        if (CsConst.myRevBuf[25] == 1) chbPower.Checked = true;
                        else chbPower.Checked = false;
                        if (CsConst.myRevBuf[26] == 1) chbWind.Checked = true;
                        else chbWind.Checked = false;
                        
                        HDLUDP.TimeBetwnNext(1);
                    }
                    else
                    {
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                    byte[] bytAryWind = new byte[4];
                    byte[] bytAryMode = new byte[5];
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xE124, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)  // read mode
                    {
                        for (byte bytI = 0; bytI < 4; bytI++) { bytAryWind[bytI] = CsConst.myRevBuf[26 + bytI]; }

                        for (byte bytI = 0; bytI < 5; bytI++) {bytAryMode[bytI] = CsConst.myRevBuf[31 + bytI]; }
                        
                        HDLUDP.TimeBetwnNext(1);
                    }
                    else
                    {
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                    if (bytAryWind[0] == 0) chbListFAN.SetItemChecked(0, true);
                    else chbListFAN.SetItemChecked(0, false);
                    if (bytAryWind[1] == 1) chbListFAN.SetItemChecked(1, true);
                    else chbListFAN.SetItemChecked(1, false);
                    if (bytAryWind[2] == 2) chbListFAN.SetItemChecked(2, true);
                    else chbListFAN.SetItemChecked(2, false);
                    if (bytAryWind[3] == 3) chbListFAN.SetItemChecked(3, true);
                    else chbListFAN.SetItemChecked(3, false);
                    if (bytAryMode[0] == 0) chbListMode.SetItemChecked(0, true);
                    else chbListMode.SetItemChecked(0, false);
                    if (bytAryMode[1] == 1) chbListMode.SetItemChecked(1, true);
                    else chbListMode.SetItemChecked(1, false);
                    if (bytAryMode[2] == 2) chbListMode.SetItemChecked(2, true);
                    else chbListMode.SetItemChecked(2, false);
                    if (bytAryMode[3] == 3) chbListMode.SetItemChecked(3, true);
                    else chbListMode.SetItemChecked(3, false);
                    if (bytAryMode[4] == 4) chbListMode.SetItemChecked(4, true);
                    else chbListMode.SetItemChecked(4, false);
                }
                else if (tabControl1.SelectedIndex == 1)
                {
                    byte[] ArayTmp = new byte[0];
                    araTemperature = new byte[11];
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1900, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                    {
                        Array.Copy(CsConst.myRevBuf, 25, araTemperature, 0, 11);
                        
                        HDLUDP.TimeBetwnNext(1);
                    }
                    else
                    {
                        Cursor.Current = Cursors.Default;
                        return;
                    }

                    if (araTemperature[6] == 0)
                    {
                        for (int i = 1; i <= 4; i++)
                        {
                            HScrollBar tmp1 = this.Controls.Find("sbL" + i.ToString(), true)[0] as HScrollBar;
                            HScrollBar tmp2 = this.Controls.Find("sbH" + i.ToString(), true)[0] as HScrollBar;
                            tmp1.Minimum = 0;
                            tmp1.Maximum = 30;
                            tmp2.Minimum = 0;
                            tmp2.Maximum = 30;
                        }
                    }
                    else if (araTemperature[6] == 1)
                    {
                        for (int i = 1; i <= 4; i++)
                        {
                            HScrollBar tmp1 = this.Controls.Find("sbL" + i.ToString(), true)[0] as HScrollBar;
                            HScrollBar tmp2 = this.Controls.Find("sbH" + i.ToString(), true)[0] as HScrollBar;
                            tmp1.Minimum = 32;
                            tmp1.Maximum = 86;
                            tmp2.Minimum = 32;
                            tmp2.Maximum = 86;
                        }
                    }
                    sbL1.Value = araTemperature[0];
                    sbH1.Value = araTemperature[1];
                    sbL2.Value = araTemperature[2];
                    sbH2.Value = araTemperature[3];
                    sbL3.Value = araTemperature[4];
                    sbH3.Value = araTemperature[5];
                    sbL4.Value = araTemperature[7];
                    sbH4.Value = araTemperature[8];
                }
            }
            catch
            {
            }
        }

        private void sbL1_ValueChanged(object sender, EventArgs e)
        {
            if (bytTempType == 0)
            {
                lbLValue1.Text = sbL1.Value.ToString() + "C";
                lbHValue1.Text = sbH1.Value.ToString() + "C";
                lbLValue2.Text = sbL2.Value.ToString() + "C";
                lbHValue2.Text = sbH2.Value.ToString() + "C";
                lbLValue3.Text = sbL3.Value.ToString() + "C";
                lbHValue3.Text = sbH3.Value.ToString() + "C";
                lbLValue4.Text = sbL4.Value.ToString() + "C";
                lbHValue4.Text = sbH4.Value.ToString() + "C";
            }
            else if (bytTempType == 1)
            {
                lbLValue1.Text = sbL1.Value.ToString() + "F";
                lbHValue1.Text = sbH1.Value.ToString() + "F";
                lbLValue2.Text = sbL2.Value.ToString() + "F";
                lbHValue2.Text = sbH2.Value.ToString() + "F";
                lbLValue3.Text = sbL3.Value.ToString() + "F";
                lbHValue3.Text = sbH3.Value.ToString() + "F";
                lbLValue4.Text = sbL4.Value.ToString() + "F";
                lbHValue4.Text = sbH4.Value.ToString() + "F";
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                byte[] arayTmp = new byte[1];
                if (tabControl1.SelectedIndex == 0)
                {
                    arayTmp[0] = Convert.ToByte(cbTemp.SelectedIndex);
                    if (CsConst.mySends.AddBufToSndList(arayTmp, 0xE122, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                    {
                        
                    }
                    else
                    {
                        Cursor.Current = Cursors.Default;
                        return;
                    }

                    arayTmp = new byte[11];
                    byte byt = 0;
                    byte bytIndex = 1;
                    for (int i = 0; i < chbListFAN.Items.Count; i++)
                    {
                        if (chbListFAN.GetItemChecked(i) == true)
                        {
                            byt = Convert.ToByte(byt + 1);
                            arayTmp[bytIndex] = Convert.ToByte(i);
                            bytIndex++;
                        }
                    }
                    arayTmp[0] = byt;
                    byt = 0;
                    bytIndex = 6;
                    for (int i = 0; i < chbListMode.Items.Count; i++)
                    {
                        if (chbListMode.GetItemChecked(i) == true)
                        {
                            byt = Convert.ToByte(byt + 1);
                            arayTmp[bytIndex] = Convert.ToByte(i);
                            bytIndex++;
                        }
                    }
                    arayTmp[5] = byt;
                    if (CsConst.mySends.AddBufToSndList(arayTmp, 0xE126, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                    {
                        
                    }
                    else
                    {
                        Cursor.Current = Cursors.WaitCursor;
                        return;
                    }
                    HDLUDP.TimeBetwnNext(20);

                    arayTmp = new byte[2];
                    if (chbPower.Checked) arayTmp[0] = 1;
                    if (chbWind.Checked) arayTmp[1] = 1;
                    if (CsConst.mySends.AddBufToSndList(arayTmp, 0x1911, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                    {
                        
                    }
                    else
                    {
                        Cursor.Current = Cursors.WaitCursor;
                        return;
                    }
                    HDLUDP.TimeBetwnNext(20);
                }
                else if (tabControl1.SelectedIndex == 1)
                {
                    arayTmp = new byte[11];
                    arayTmp[0] = Convert.ToByte(sbL1.Value);
                    arayTmp[1] = Convert.ToByte(sbH1.Value);
                    arayTmp[2] = Convert.ToByte(sbL2.Value);
                    arayTmp[3] = Convert.ToByte(sbH2.Value);
                    arayTmp[4] = Convert.ToByte(sbL3.Value);
                    arayTmp[5] = Convert.ToByte(sbH3.Value);
                    arayTmp[6] = araTemperature[6];
                    arayTmp[7] = Convert.ToByte(sbL4.Value);
                    arayTmp[8] = Convert.ToByte(sbH4.Value);
                    arayTmp[9] = araTemperature[9];
                    arayTmp[10] = araTemperature[10];
                    if (CsConst.mySends.AddBufToSndList(arayTmp, 0x1902, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                    {
                        
                    }
                }
                
            }
            catch
            {
            }
            Cursor.Current = Cursors.Default;
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            setValue();
        }
    }
}
