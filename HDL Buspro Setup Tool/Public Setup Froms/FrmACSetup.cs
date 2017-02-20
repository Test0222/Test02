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
    public partial class FrmACSetup : Form
    {
        private DLP oDLP;
        private string DeviceName;
        private int DeviceType;
        private byte SubNetID;
        private byte DeviceID;
        public FrmACSetup()
        {
            InitializeComponent();
        }

        public FrmACSetup(string strname, Object colordlp, int devicetype)
        {
            InitializeComponent();
            this.DeviceName = strname;
            this.oDLP = (DLP)colordlp;
            this.DeviceType = devicetype;

            string strDevName = strname.Split('\\')[0].ToString();
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
                if (oDLP == null) return;
                if (oDLP.DLPAC == null) return;
                ACSetting temp = oDLP.DLPAC;
                if (temp.bytAryWind.Length !=null && temp.bytAryWind.Length>0)
                {
                    for (int i = 0; i < temp.bytAryWind[0]; i++)
                    {
                        int TempModeID = temp.bytAryWind[i + 1]; 
                        chbListFAN.SetItemChecked(TempModeID, true);
                    }
                }

                if (temp.bytAryMode.Length != null && temp.bytAryMode.Length > 0)
                {
                    for (int i = 0; i < temp.bytAryMode[0]; i++)
                    {
                        int TempModeID = temp.bytAryMode[i];
                        chbListMode.SetItemChecked(TempModeID, true);
                    }
                }

                if (temp.bytSavePower == 1) chbPower.Checked = true;
                if (temp.bytWind == 1) chbWind.Checked = true;
                if (oDLP.bytTempType == 0)
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
                else if (oDLP.bytTempType == 1)
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
                sbL1.Value = temp.bytTempArea[0];
                sbH1.Value = temp.bytTempArea[1];
                sbL2.Value = temp.bytTempArea[2];
                sbH2.Value = temp.bytTempArea[3];
                sbL3.Value = temp.bytTempArea[4];
                sbH3.Value = temp.bytTempArea[5];
                sbL4.Value = temp.bytTempArea[6];
                sbH4.Value = temp.bytTempArea[7];
            }
            catch
            {
            }
        }

        private void sbL1_ValueChanged(object sender, EventArgs e)
        {
            if (oDLP.bytTempType == 0)
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
            else if (oDLP.bytTempType == 1)
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
                Byte[] arayTmp;
                if (tabControl1.SelectedIndex == 0)
                {
                    arayTmp = new Byte[chbListFAN.Items.Count + 1];
                    Byte ArraySaveID = 0;
                    // update fan + mode 
                    for (int i = 0; i < chbListFAN.Items.Count; i++)
                    {
                        if (chbListFAN.GetItemChecked(i) == true)
                        {
                            arayTmp[ArraySaveID + 1] = Convert.ToByte(i);
                            ArraySaveID++;
                        }
                    }
                    arayTmp[0] = ArraySaveID;
                    if (oDLP.DLPAC.bytAryWind == null) oDLP.DLPAC.bytAryWind = new byte[5];
                    oDLP.DLPAC.bytAryWind = arayTmp;

                    ArraySaveID = 0;
                    arayTmp = new Byte[chbListMode.Items.Count + 1];
                    for (int i = 0; i < chbListMode.Items.Count; i++)
                    {
                        if (chbListMode.GetItemChecked(i) == true)
                        {
                            arayTmp[ArraySaveID + 1] = Convert.ToByte(i);
                            ArraySaveID++;
                        }
                    }
                    arayTmp[0] = ArraySaveID;
                    if (oDLP.DLPAC.bytAryMode == null) oDLP.DLPAC.bytAryMode = new byte[6];
                    oDLP.DLPAC.bytAryMode = arayTmp;

                    oDLP.DLPAC.SaveHVACModeAndFan(SubNetID, DeviceID, DeviceType);

                    arayTmp = new byte[2];
                    if (chbPower.Checked) arayTmp[0] = 1;
                    if (chbWind.Checked) arayTmp[1] = 1;
                    if (CsConst.mySends.AddBufToSndList(arayTmp, 0x1911, SubNetID, DeviceID, false, true, true,CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                    {
                        oDLP.DLPAC.bytSavePower = arayTmp[0];
                        oDLP.DLPAC.bytWind = arayTmp[1];
                        CsConst.myRevBuf = new byte[1200];
                    }
                    HDLUDP.TimeBetwnNext(arayTmp.Length);
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
                    arayTmp[6] = oDLP.bytTempType;
                    arayTmp[7] = Convert.ToByte(sbL4.Value);
                    arayTmp[8] = Convert.ToByte(sbH4.Value);
                    arayTmp[9] = Convert.ToByte(oDLP.DLPFH.minTemp);
                    arayTmp[10] = Convert.ToByte(oDLP.DLPFH.maxTemp);
                    if (CsConst.mySends.AddBufToSndList(arayTmp, 0x1902, SubNetID, DeviceID, false, true, true,CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                    {
                        if (oDLP.DLPAC.bytTempArea == null) oDLP.DLPAC.bytTempArea = new byte[8];
                        Array.Copy(arayTmp, 0, oDLP.DLPAC.bytTempArea, 0, 6);
                        oDLP.DLPAC.bytTempArea[6] = arayTmp[7];
                        oDLP.DLPAC.bytTempArea[7] = arayTmp[8];
                        CsConst.myRevBuf = new byte[1200];
                    }
                    HDLUDP.TimeBetwnNext(arayTmp.Length);
                }
            }
            catch
            {
            }
            Cursor.Current = Cursors.Default;
        }
    }
}
