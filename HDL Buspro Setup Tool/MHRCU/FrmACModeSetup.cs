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
    public partial class FrmACModeSetup : Form
    {
        private string myDevName = null;
        private byte SubNetID;
        private byte DevID;
        private int MyintDeviceType;
        private MHRCU oMHRCU = new MHRCU();
        private string strTest = "C";
        private ComboBox cb1 = new ComboBox();
        private ComboBox cb2 = new ComboBox();
        private ComboBox cb3 = new ComboBox();
        private TimeText time1 = new TimeText(".");
        private TimeText time2 = new TimeText(".");
        private TimeText time3 = new TimeText(".");
        public FrmACModeSetup()
        {
            InitializeComponent();
        }

        public FrmACModeSetup(MHRCU myMHRCU, string strName, int intDeviceType)
        {
            InitializeComponent();
            this.myDevName = strName;
            string strDevName = strName.Split('\\')[0].ToString();
            this.MyintDeviceType = intDeviceType;
            this.oMHRCU = myMHRCU;
            lbRemarkValue.Text = strName.Split('\\')[1].ToString();
            lbSubValue.Text = strDevName.Split('-')[0];
            lbDevValue.Text = strDevName.Split('-')[1];
            SubNetID = Convert.ToByte(strDevName.Split('-')[0]);
            DevID = Convert.ToByte(strDevName.Split('-')[1]);
            cb1 = new ComboBox();
            cb1.DropDownStyle = ComboBoxStyle.DropDownList;
            cb1.Items.Clear();
            cb1.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99655", ""));
            cb1.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99656", ""));
            cb1.SelectedIndexChanged += cb1_SelectedIndexChanged;
            cb2 = new ComboBox();
            cb2.DropDownStyle = ComboBoxStyle.DropDownList;
            cb2.Items.Clear();
            cb2.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99655", ""));
            cb2.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99656", ""));
            cb2.SelectedIndexChanged += cb2_SelectedIndexChanged;
            cb3 = new ComboBox();
            cb3.DropDownStyle = ComboBoxStyle.DropDownList;
            cb3.Items.Clear();
            cb3.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99655", ""));
            cb3.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99656", ""));
            cb3.SelectedIndexChanged += cb3_SelectedIndexChanged;
            time1 = new TimeText(".");
            time1.TextChanged += time1_TextChanged;
            time2 = new TimeText(".");
            time2.TextChanged += time2_TextChanged;
            time3 = new TimeText(".");
            time3.TextChanged += time3_TextChanged;
            dgvMode.Controls.Add(cb1);
            dgvMode.Controls.Add(cb2);
            dgvMode.Controls.Add(cb3);
            dgvMode.Controls.Add(time1);
            dgvMode.Controls.Add(time2);
            dgvMode.Controls.Add(time3);
            setVisible(false);
        }

        void time3_TextChanged(object sender, EventArgs e)
        {
            if (dgvMode.CurrentRow.Index < 0) return;
            if (dgvMode.RowCount <= 0) return;
            int index = dgvMode.CurrentRow.Index;
            dgvMode[6, index].Value = HDLPF.GetStringFromTime(Convert.ToInt32(time3.Text), ".");
        }

        void time2_TextChanged(object sender, EventArgs e)
        {
            if (dgvMode.CurrentRow.Index < 0) return;
            if (dgvMode.RowCount <= 0) return;
            int index = dgvMode.CurrentRow.Index;
            dgvMode[4, index].Value = HDLPF.GetStringFromTime(Convert.ToInt32(time2.Text), ".");
        }

        void time1_TextChanged(object sender, EventArgs e)
        {
            if (dgvMode.CurrentRow.Index < 0) return;
            if (dgvMode.RowCount <= 0) return;
            int index = dgvMode.CurrentRow.Index;
            dgvMode[2, index].Value = HDLPF.GetStringFromTime(Convert.ToInt32(time1.Text), ".");
        }

        void cb3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dgvMode.CurrentRow.Index < 0) return;
            if (dgvMode.RowCount <= 0) return;
            int index = dgvMode.CurrentRow.Index;
            dgvMode[5, index].Value = cb1.Text;
        }

        void cb2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dgvMode.CurrentRow.Index < 0) return;
            if (dgvMode.RowCount <= 0) return;
            int index = dgvMode.CurrentRow.Index;
            dgvMode[3, index].Value = cb1.Text;
        }

        void cb1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dgvMode.CurrentRow.Index < 0) return;
            if (dgvMode.RowCount <= 0) return;
            int index = dgvMode.CurrentRow.Index;
            dgvMode[1, index].Value = cb1.Text;
        }

        private void FrmACModeSetup_Load(object sender, EventArgs e)
        {
            lbSubValue.Text = myDevName.Split('\\')[0].ToString().Split('-')[0].ToString();
            lbDevValue.Text = myDevName.Split('\\')[0].ToString().Split('-')[1].ToString();
            cbACPower.Items.Clear();
            cbACPower.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00038", ""));
            cbACPower.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00037", ""));
            cbMode.Items.Clear();
            for (int i = 0; i < 5; i++)
                cbMode.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "0005" + i.ToString(), ""));
            cbFanSpeed.Items.Clear();
            for (int i = 0; i < 4; i++)
                cbFanSpeed.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "0006" + i.ToString(), ""));
            cbTime1.Items.Clear();
            for (int i = 1; i <= 120; i++)
                cbTime1.Items.Add(i.ToString());
            cbTime2.Items.Clear();
            for (int i = 0; i <= 10; i++)
                cbTime2.Items.Add(i.ToString());
            for (int i = 1; i <= 3; i++)
            {
                ComboBox temp1 = this.Controls.Find("cbMode" + i.ToString(), true)[0] as ComboBox;
                temp1.Items.Clear();
                temp1.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99659", ""));
                temp1.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99660", ""));
                ComboBox temp2 = this.Controls.Find("cbRestore" + i.ToString(), true)[0] as ComboBox;
                temp2.Items.Clear();
                temp2.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99657", ""));
                temp2.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99658", ""));
            }
            btnRefAC_Click(null, null);
            showMODEInfo();
        }

        private void showMODEInfo()
        {
            try
            {
                if (oMHRCU.myHVAC.arayMode[0] == 0) rb1.Checked = true;
                else if (oMHRCU.myHVAC.arayMode[0] == 1) rb2.Checked = true;
                if (oMHRCU.myHVAC.arayMode[1] == 1) chbEnable.Checked = true;
                else chbEnable.Checked = false;
                cbTime1.SelectedIndex = oMHRCU.myHVAC.arayMode[2] - 1;
                cbTime2.SelectedIndex = oMHRCU.myHVAC.arayMode[3];

                cbMode1.SelectedIndex = oMHRCU.myHVAC.arayRelay[0];
                cbMode2.SelectedIndex = oMHRCU.myHVAC.arayRelay[1];
                cbMode3.SelectedIndex = oMHRCU.myHVAC.arayRelay[2];
                cbRestore1.SelectedIndex = oMHRCU.myHVAC.arayRelay[3];
                cbRestore2.SelectedIndex = oMHRCU.myHVAC.arayRelay[4];
                cbRestore3.SelectedIndex = oMHRCU.myHVAC.arayRelay[5];

                dgvMode.Rows.Clear();
                for (int i = 0; i < 5; i++)
                {
                    string strMode = "";
                    string strRelay1=CsConst.mstrINIDefault.IniReadValue("Public", "99655", "");
                    string strRelay2=CsConst.mstrINIDefault.IniReadValue("Public", "99655", "");
                    string strRelay3=CsConst.mstrINIDefault.IniReadValue("Public", "99655", "");
                    string strDelay1="0.0";
                     string strDelay2="0.0";
                     string strDelay3="0.0";
                    if (i == 0) strMode = CsConst.mstrINIDefault.IniReadValue("Public", "00050", "");
                    else if (i == 1) strMode = CsConst.mstrINIDefault.IniReadValue("Public", "00051", "");
                    else if (i == 2) strMode = CsConst.mstrINIDefault.IniReadValue("Public", "00054", "");
                    else if (i == 3) strMode = CsConst.mstrINIDefault.IniReadValue("Public", "00052", "");
                    else if (i == 4) strMode = CsConst.mstrINIDefault.IniReadValue("Public", "00055", "");
                    if (oMHRCU.myHVAC.araComplex[i * 7 + 1] == 0) strRelay1 = CsConst.mstrINIDefault.IniReadValue("Public", "99655", "");
                    else if (oMHRCU.myHVAC.araComplex[i * 7 + 1] == 1) strRelay1 = CsConst.mstrINIDefault.IniReadValue("Public", "99656", "");
                    strDelay1 = (oMHRCU.myHVAC.araComplex[i * 7 + 2] / 10).ToString() + "." + (oMHRCU.myHVAC.araComplex[i * 7 + 2] % 10).ToString();
                    if (oMHRCU.myHVAC.araComplex[i * 7 + 3] == 0) strRelay2 = CsConst.mstrINIDefault.IniReadValue("Public", "99655", "");
                    else if (oMHRCU.myHVAC.araComplex[i * 7 + 3] == 1) strRelay2 = CsConst.mstrINIDefault.IniReadValue("Public", "99656", "");
                    strDelay2 = (oMHRCU.myHVAC.araComplex[i * 5 + 4] / 10).ToString() + "." + (oMHRCU.myHVAC.araComplex[i * 7 + 4] % 10).ToString();
                    if (oMHRCU.myHVAC.araComplex[i * 7 + 5] == 0) strRelay3 = CsConst.mstrINIDefault.IniReadValue("Public", "99655", "");
                    else if (oMHRCU.myHVAC.araComplex[i * 7 + 5] == 1) strRelay3 = CsConst.mstrINIDefault.IniReadValue("Public", "99656", "");
                    strDelay3 = (oMHRCU.myHVAC.araComplex[i * 7 + 6] / 10).ToString() + "." + (oMHRCU.myHVAC.araComplex[i * 7 + 6] % 10).ToString();
                    object[] obj = new object[] { strMode, strRelay1, strDelay1, strRelay2, strDelay2, strRelay3, strDelay3 };
                    dgvMode.Rows.Add(obj);
                }
                rb2_CheckedChanged(null, null);
            }
            catch
            {
            }
        }

        private void btnRefAC_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (cbACNO.SelectedIndex < 0) cbACNO.SelectedIndex = 0;
                byte[] arayTmp = new byte[1];
                arayTmp[0] = Convert.ToByte(cbACNO.SelectedIndex + 1);
                if (CsConst.mySends.AddBufToSndList(arayTmp, 0x1938, SubNetID, DevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(MyintDeviceType)) == true)
                {
                    if (CsConst.myRevBuf[26] == 0)
                    {
                        sbCoolTemp.Minimum = 0;
                        sbHeatTemp.Minimum = 0;
                        sbAutoTemp.Minimum = 0;
                        sbDryTemp.Minimum = 0;
                        sbCoolTemp.Maximum = 30;
                        sbHeatTemp.Maximum = 30;
                        sbAutoTemp.Maximum = 30;
                        sbDryTemp.Maximum = 30;
                        strTest = "C";
                    }
                    else if (CsConst.myRevBuf[26] == 1)
                    {
                        sbCoolTemp.Minimum = 32;
                        sbHeatTemp.Minimum = 32;
                        sbAutoTemp.Minimum = 32;
                        sbDryTemp.Minimum = 32;
                        sbCoolTemp.Maximum = 86;
                        sbHeatTemp.Maximum = 86;
                        sbAutoTemp.Maximum = 86;
                        sbDryTemp.Maximum = 86;
                        strTest = "F";
                    }
                    lbCurTempValue.Text = CsConst.myRevBuf[27].ToString() + strTest;
                    sbCoolTemp.Value = CsConst.myRevBuf[28];
                    sbHeatTemp.Value = CsConst.myRevBuf[29];
                    sbAutoTemp.Value = CsConst.myRevBuf[30];
                    sbDryTemp.Value = CsConst.myRevBuf[31];
                    string strRunnig="";
                    string strTmp = GlobalClass.IntToBit(Convert.ToInt32(CsConst.myRevBuf[32]), 8);
                    string str1 = strTmp.Substring(0, 4);
                    string str2 = strTmp.Substring(4, 4);
                    int intTmp = GlobalClass.BitToInt(str1);
                    if (intTmp == 0) strRunnig = CsConst.mstrINIDefault.IniReadValue("Public", "00050", "");
                    else if (intTmp == 1) strRunnig = CsConst.mstrINIDefault.IniReadValue("Public", "00051", "");
                    else if (intTmp == 2) strRunnig = CsConst.mstrINIDefault.IniReadValue("Public", "00052", "");
                    else if (intTmp == 3) strRunnig = CsConst.mstrINIDefault.IniReadValue("Public", "00053", "");
                    else if (intTmp == 4) strRunnig = CsConst.mstrINIDefault.IniReadValue("Public", "00054", "");
                    intTmp = GlobalClass.BitToInt(str2);
                    if (intTmp == 0) strRunnig = strRunnig + "," + CsConst.mstrINIDefault.IniReadValue("Public", "00060", "");
                    else if (intTmp == 1) strRunnig = strRunnig + "," + CsConst.mstrINIDefault.IniReadValue("Public", "00061", "");
                    else if (intTmp == 2) strRunnig = strRunnig + "," + CsConst.mstrINIDefault.IniReadValue("Public", "00062", "");
                    else if (intTmp == 3) strRunnig = strRunnig + "," + CsConst.mstrINIDefault.IniReadValue("Public", "00063", "");
                    lbRunningValue.Text = strRunnig;
                    cbACPower.SelectedIndex = CsConst.myRevBuf[33];
                    cbMode.SelectedIndex = CsConst.myRevBuf[34];
                    cbFanSpeed.SelectedIndex = CsConst.myRevBuf[35];
                    
                }
            }
            catch
            {
            }
            Cursor.Current = Cursors.Default;
        }

        private void btnSaveAC_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            byte[] arayTmp = new byte[13];
            arayTmp[0] = Convert.ToByte(cbACNO.SelectedIndex + 1);
            if (strTest == "C") arayTmp[1] = 0;
            else if (strTest == "F") arayTmp[1] = 1;
            arayTmp[3] = Convert.ToByte(sbCoolTemp.Value);
            arayTmp[4] = Convert.ToByte(sbHeatTemp.Value);
            arayTmp[5] = Convert.ToByte(sbAutoTemp.Value);
            arayTmp[6] = Convert.ToByte(sbDryTemp.Value);
            arayTmp[8] = Convert.ToByte(cbACPower.SelectedIndex);
            arayTmp[9] = Convert.ToByte(cbMode.SelectedIndex);
            arayTmp[10] = Convert.ToByte(cbFanSpeed.SelectedIndex);
            if (CsConst.mySends.AddBufToSndList(arayTmp, 0x193A, SubNetID, DevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(MyintDeviceType)) == true)
            {
                
            }
            Cursor.Current = Cursors.Default;
        }

        private void btnRefMode_Click(object sender, EventArgs e)
        {
            setVisible(false);
            Cursor.Current = Cursors.WaitCursor;
            byte[] ArayTmp = new byte[0];

            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1C9A, SubNetID, DevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(MyintDeviceType)) == true)
            {
                Array.Copy(CsConst.myRevBuf, 25, oMHRCU.myHVAC.arayMode, 0, 4);
                HDLUDP.TimeBetwnNext(1);
                
            }
            else
            {
                Cursor.Current = Cursors.Default;
                return;
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy) CsConst.calculationWorker.ReportProgress(98);

            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1C9E, SubNetID, DevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(MyintDeviceType)) == true)
            {
                Array.Copy(CsConst.myRevBuf, 25, oMHRCU.myHVAC.arayRelay, 0, 6);
                HDLUDP.TimeBetwnNext(1);
                
            }
            else
            {
                Cursor.Current = Cursors.Default;
                return;
            }
            ArayTmp = new byte[1];
            for (byte i = 0; i < 5; i++)
            {
                ArayTmp[0] = i;
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1CA2, SubNetID, DevID, false, true, true,CsConst.minAllWirelessDeviceType.Contains(MyintDeviceType)) == true)
                {
                    Array.Copy(CsConst.myRevBuf, 25, oMHRCU.myHVAC.araComplex, i * 7, 7);
                    HDLUDP.TimeBetwnNext(1);
                    
                }
                else
                {
                    Cursor.Current = Cursors.Default;
                    return;
                }
            }
            showMODEInfo();
            Cursor.Current = Cursors.Default;
        }

        private void btnSaveMode_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                byte[] arayTmp = new byte[4];
                if (rb1.Checked) arayTmp[0] = 0;
                else if (rb2.Checked) arayTmp[0] = 1;
                if (chbEnable.Checked) arayTmp[1] = 1;
                arayTmp[2] = Convert.ToByte(cbTime1.SelectedIndex + 1);
                arayTmp[3] = Convert.ToByte(cbTime2.SelectedIndex);
                if (CsConst.mySends.AddBufToSndList(arayTmp, 0x1C98, SubNetID, DevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(MyintDeviceType)) == true)
                {
                    Array.Copy(arayTmp, 0, oMHRCU.myHVAC.arayMode, 0, 4);
                    
                    HDLUDP.TimeBetwnNext(20);
                }
                else
                {
                    Cursor.Current = Cursors.Default;
                    return;
                }

                arayTmp = new byte[6];
                arayTmp[0] = Convert.ToByte(cbMode1.SelectedIndex);
                arayTmp[1] = Convert.ToByte(cbMode2.SelectedIndex);
                arayTmp[2] = Convert.ToByte(cbMode3.SelectedIndex);
                arayTmp[3] = Convert.ToByte(cbRestore1.SelectedIndex);
                arayTmp[4] = Convert.ToByte(cbRestore2.SelectedIndex);
                arayTmp[5] = Convert.ToByte(cbRestore3.SelectedIndex);
                if (CsConst.mySends.AddBufToSndList(arayTmp, 0x1C9C, SubNetID, DevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(MyintDeviceType)) == true)
                {
                    Array.Copy(arayTmp, 0, oMHRCU.myHVAC.arayRelay, 0, 6);
                    
                    HDLUDP.TimeBetwnNext(20);
                }
                else
                {
                    Cursor.Current = Cursors.Default;
                    return;
                }

                for (int i = 0; i < dgvMode.Rows.Count; i++)
                {
                    string strRelay1 = dgvMode[1, i].Value.ToString();
                    string strDelay1 = dgvMode[2, i].Value.ToString();
                    string strRelay2 = dgvMode[3, i].Value.ToString();
                    string strDelay2 = dgvMode[4, i].Value.ToString();
                    string strRelay3 = dgvMode[5, i].Value.ToString();
                    string strDelay3 = dgvMode[6, i].Value.ToString();
                    oMHRCU.myHVAC.araComplex[i * 7 + 1] = Convert.ToByte(cb1.Items.IndexOf(strRelay1));
                    oMHRCU.myHVAC.araComplex[i * 7 + 2] = Convert.ToByte(HDLPF.GetTimeFromString(strDelay1, '.'));
                    oMHRCU.myHVAC.araComplex[i * 7 + 3] = Convert.ToByte(cb2.Items.IndexOf(strRelay2));
                    oMHRCU.myHVAC.araComplex[i * 7 + 4] = Convert.ToByte(HDLPF.GetTimeFromString(strDelay2, '.'));
                    oMHRCU.myHVAC.araComplex[i * 7 + 5] = Convert.ToByte(cb3.Items.IndexOf(strRelay3));
                    oMHRCU.myHVAC.araComplex[i * 7 + 6] = Convert.ToByte(HDLPF.GetTimeFromString(strDelay3, '.'));
                    arayTmp = new byte[7];
                    Array.Copy(oMHRCU.myHVAC.araComplex, i * 7, arayTmp, 0, 7);
                    if (CsConst.mySends.AddBufToSndList(arayTmp, 0x1CA0, SubNetID, DevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(MyintDeviceType)) == true)
                    {
                        HDLUDP.TimeBetwnNext(10);
                        
                    }
                    else
                    {
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                }
            }
            catch
            {
            }
            Cursor.Current = Cursors.Default;
        }

        private void sbCoolTemp_ValueChanged(object sender, EventArgs e)
        {
            lbCoolTempValue.Text = sbCoolTemp.Value.ToString() + strTest;
            lbHeatTempValue.Text = sbHeatTemp.Value.ToString() + strTest;
            lbAutoTempValue.Text = sbAutoTemp.Value.ToString() + strTest;
            lbDryTempValue.Text = sbDryTemp.Value.ToString() + strTest;
        }

        private void dgvMode_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            setVisible(false);
            if (e.RowIndex >= 0)
            {
                string strRelay1 = dgvMode[1, e.RowIndex].Value.ToString();
                string strDelay1 = dgvMode[2, e.RowIndex].Value.ToString();
                string strRelay2 = dgvMode[3, e.RowIndex].Value.ToString();
                string strDelay2 = dgvMode[4, e.RowIndex].Value.ToString();
                string strRelay3 = dgvMode[5, e.RowIndex].Value.ToString();
                string strDelay3 = dgvMode[6, e.RowIndex].Value.ToString();
                cb1.SelectedIndex = cb1.Items.IndexOf(cb1.Items.IndexOf(strRelay1));
                cb1.Text = strRelay1;
                addcontrols(1, e.RowIndex, cb1);
                time1.Text = HDLPF.GetTimeFromString(strDelay1, '.');
                addcontrols(2, e.RowIndex, time1);
                cb2.SelectedIndex = cb2.Items.IndexOf(cb2.Items.IndexOf(strRelay2));
                cb2.Text = strRelay2;
                addcontrols(3, e.RowIndex, cb2);
                time2.Text = HDLPF.GetTimeFromString(strDelay2, '.');
                addcontrols(4, e.RowIndex, time2);
                cb3.SelectedIndex = cb3.Items.IndexOf(cb3.Items.IndexOf(strRelay3));
                cb3.Text = strRelay3;
                addcontrols(5, e.RowIndex, cb3);
                time3.Text = HDLPF.GetTimeFromString(strDelay3, '.');
                addcontrols(6, e.RowIndex, time3);
            }
        }

        private void addcontrols(int col, int row, Control con)
        {
            con.Show();
            con.Visible = true;
            Rectangle rect = dgvMode.GetCellDisplayRectangle(col, row, true);
            con.Size = rect.Size;
            con.Top = rect.Top;
            con.Left = rect.Left;
        }

        private void setVisible(bool TF)
        {
            cb1.Visible = TF;
            cb2.Visible = TF;
            cb3.Visible = TF;
            time1.Visible = TF;
            time2.Visible = TF;
            time3.Visible = TF;
        }

        private void rb2_CheckedChanged(object sender, EventArgs e)
        {
            if (rb1.Checked) panel6.Enabled = false;
            else if (rb2.Checked) panel6.Enabled = true;
        }
    }
}
