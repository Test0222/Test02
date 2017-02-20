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
    public partial class FrmColorDLPACSetup : Form
    {
        private EnviroAc CurrentAc;
        private Byte TemperatureType;
        private Object myActivePanel;
        private string strName;
        private int DeviceType;
        private byte SubNetID;
        private byte DeviceID;
        private int SelectIndex;
        public FrmColorDLPACSetup()
        {
            InitializeComponent();
        }
        public FrmColorDLPACSetup(string strname, Object colordlp, int devicetype, int selectindex)
        {
            this.strName = strname;
            this.myActivePanel = colordlp;
            this.DeviceType = devicetype;
            this.Text = strName;
            this.SelectIndex=selectindex;
            string strDevName = strName.Split('\\')[0].ToString();
            SubNetID = Convert.ToByte(strDevName.Split('-')[0]);
            DeviceID = Convert.ToByte(strDevName.Split('-')[1]);
            InitializeComponent();
        }

        void FirstAssignToPublicClassFromDifferentDeviceType()
        {
            if (myActivePanel == null) return;

            if (EnviroDLPPanelDeviceTypeList.HDLEnviroDLPPanelDeviceTypeList.Contains(DeviceType)) // 旧版彩屏面板
            {
                ColorDLP oColorDLP = (ColorDLP)myActivePanel;
                if (oColorDLP.MyAC == null) return;
                if (oColorDLP.MyAC.Count <= cbAC.SelectedIndex) return;
                CurrentAc = oColorDLP.MyAC[cbAC.SelectedIndex];
                TemperatureType = oColorDLP.TemperatureType;
            }
            else if (EnviroNewDeviceTypeList.EnviroNewPanelDeviceTypeList.Contains(DeviceType)) // 新版彩屏面板
            {
                EnviroPanel Tmp = (EnviroPanel)myActivePanel;
                if (Tmp == null) return;
                if (Tmp.MyAC == null || Tmp.MyAC.Count == 0) return;
                CurrentAc = Tmp.MyAC[cbAC.SelectedIndex];
                TemperatureType = Tmp.TemperatureType;
            }
        }

        private void setValue()
        {
            try
            {
                FirstAssignToPublicClassFromDifferentDeviceType();
                if (CurrentAc.FanEnable == 1) chbWind.Checked = true;
                if (CurrentAc.FanEnergySaveEnable == 1) chbPower.Checked = true;
                if (CurrentAc.FanParam[1] == 0) chbListFAN.SetItemChecked(0, true);
                else chbListFAN.SetItemChecked(0, false);
                if (CurrentAc.FanParam[2] == 1) chbListFAN.SetItemChecked(1, true);
                else chbListFAN.SetItemChecked(1, false);
                if (CurrentAc.FanParam[3] == 2) chbListFAN.SetItemChecked(2, true);
                else chbListFAN.SetItemChecked(2, false);
                if (CurrentAc.FanParam[4] == 3) chbListFAN.SetItemChecked(3, true);
                else chbListFAN.SetItemChecked(3, false);
                if (CurrentAc.FanParam[6] == 0) chbListMode.SetItemChecked(0, true);
                else chbListMode.SetItemChecked(0, false);
                if (CurrentAc.FanParam[7] == 1) chbListMode.SetItemChecked(1, true);
                else chbListMode.SetItemChecked(1, false);
                if (CurrentAc.FanParam[8] == 2) chbListMode.SetItemChecked(2, true);
                else chbListMode.SetItemChecked(2, false);
                if (CurrentAc.FanParam[9] == 3) chbListMode.SetItemChecked(3, true);
                else chbListMode.SetItemChecked(3, false);
                if (CurrentAc.FanParam[10] == 4) chbListMode.SetItemChecked(4, true);
                else chbListMode.SetItemChecked(4, false);
                if (TemperatureType == 0)
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
                else if (TemperatureType == 1)
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
                sbL1.Value = CurrentAc.CoolParam[0];
                sbH1.Value = CurrentAc.CoolParam[1];
                sbL2.Value = CurrentAc.CoolParam[2];
                sbH2.Value = CurrentAc.CoolParam[3];
                sbL3.Value = CurrentAc.CoolParam[4];
                sbH3.Value = CurrentAc.CoolParam[5];
                sbL4.Value = CurrentAc.CoolParam[6];
                sbH4.Value = CurrentAc.CoolParam[7];
                if (CurrentAc.OutDoorParam[0] == 0) rb1.Checked = true;
                else if (CurrentAc.OutDoorParam[0] == 1) rb2.Checked = true;
                else if (CurrentAc.OutDoorParam[0] == 2) rb3.Checked = true;
                cbHeatSensor1.SelectedIndex = CurrentAc.OutDoorParam[1];
                NumSub1.Value = Convert.ToDecimal(CurrentAc.OutDoorParam[2]);
                NumDev1.Value = Convert.ToDecimal(CurrentAc.OutDoorParam[3]);
                NumChn1.Value = Convert.ToDecimal(CurrentAc.OutDoorParam[4]);
                cbHeatSensor2.SelectedIndex = CurrentAc.OutDoorParam[5];
                NumSub2.Value = Convert.ToDecimal(CurrentAc.OutDoorParam[6]);
                NumDev2.Value = Convert.ToDecimal(CurrentAc.OutDoorParam[7]);
                NumChn2.Value = Convert.ToDecimal(CurrentAc.OutDoorParam[8]);
            }
            catch
            {
            }
        }

        private void saveSetup()
        {
            try
            {
                FirstAssignToPublicClassFromDifferentDeviceType();

                Cursor.Current = Cursors.WaitCursor;
                byte[] ArayTmp = new byte[42];
                ArayTmp[0] = Convert.ToByte(cbAC.SelectedIndex);
                ArayTmp[1] = CurrentAc.Enable;
                ArayTmp[2] = CurrentAc.DesSubnetID;
                ArayTmp[3] = CurrentAc.DesDevID;
                ArayTmp[4] = CurrentAc.ACNum;
                ArayTmp[5] = CurrentAc.ControlWays;
                ArayTmp[6] = CurrentAc.OperationEnable;
                ArayTmp[7] = CurrentAc.PowerOnRestoreState;
                if (chbWind.Checked) ArayTmp[11] = 1;
                if (chbPower.Checked) ArayTmp[12] = 1;
                byte byt = 0;
                for (int i = 0; i < chbListFAN.Items.Count; i++)
                {
                    if (chbListFAN.GetItemChecked(i) == true)
                    {
                        byt = Convert.ToByte(byt + 1);
                        ArayTmp[14 + i] = Convert.ToByte(i);
                    }
                }
                ArayTmp[13] = byt;
                byt = 0;
                for (int i = 0; i < chbListMode.Items.Count; i++)
                {
                    if (chbListMode.GetItemChecked(i) == true)
                    {
                        byt = Convert.ToByte(byt + 1);
                        ArayTmp[19 + i] = Convert.ToByte(i);
                    }
                }
                ArayTmp[18] = byt;
                ArayTmp[24] = Convert.ToByte(sbL1.Value);
                ArayTmp[25] = Convert.ToByte(sbH1.Value);
                ArayTmp[26] = Convert.ToByte(sbL2.Value);
                ArayTmp[27] = Convert.ToByte(sbH2.Value);
                ArayTmp[28] = Convert.ToByte(sbL3.Value);
                ArayTmp[29] = Convert.ToByte(sbH3.Value);
                ArayTmp[30] = Convert.ToByte(sbL4.Value);
                ArayTmp[31] = Convert.ToByte(sbH4.Value);
                if (rb1.Checked) ArayTmp[32] = 0;
                else if (rb2.Checked) ArayTmp[32] = 1;
                else if (rb3.Checked) ArayTmp[32] = 2;
                ArayTmp[33] = Convert.ToByte(cbHeatSensor1.SelectedIndex);
                ArayTmp[34] = Convert.ToByte(NumSub1.Value);
                ArayTmp[35] = Convert.ToByte(NumDev1.Value);
                ArayTmp[36] = Convert.ToByte(NumChn1.Value);
                ArayTmp[37] = Convert.ToByte(cbHeatSensor2.SelectedIndex);
                ArayTmp[38] = Convert.ToByte(NumSub1.Value);
                ArayTmp[39] = Convert.ToByte(NumDev1.Value);
                ArayTmp[40] = Convert.ToByte(NumChn1.Value);
                ArayTmp[41] = TemperatureType;
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x19AC, SubNetID, DeviceID, false, true, true,false) == true)
                {
                    CurrentAc.FanEnable = ArayTmp[11];
                    CurrentAc.FanEnergySaveEnable = ArayTmp[12];
                    CurrentAc.FanParam = new byte[20];
                    Array.Copy(ArayTmp, 13, CurrentAc.FanParam, 0, 11);
                    CurrentAc.CoolParam = new byte[20];
                    Array.Copy(ArayTmp, 24, CurrentAc.CoolParam, 0, 8);
                    CurrentAc.OutDoorParam = new byte[20];
                    Array.Copy(ArayTmp, 32, CurrentAc.OutDoorParam, 0, 9);
                    CsConst.myRevBuf = new byte[1200];
                }
            }
            catch
            {
            }
            Cursor.Current = Cursors.Default;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            setValue();
            Cursor.Current = Cursors.Default;
        }

        private void sbL1_ValueChanged(object sender, EventArgs e)
        {
            if (TemperatureType == 0)
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
            else if (TemperatureType == 1)
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

        private void FrmColorDLPACSetup_Load(object sender, EventArgs e)
        {
            InitialFormCtrlsTextOrItems();            
        }

        void InitialFormCtrlsTextOrItems()
        {
            for (int i = 1; i <= 2; i++)
            {
                ComboBox temp = this.Controls.Find("cbHeatSensor" + i.ToString(), true)[0] as ComboBox;
                temp.Items.Clear();
                temp.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00038", ""));
                temp.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99891", ""));
                temp.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99892", ""));
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            saveSetup();
        }

        private void rb1_CheckedChanged(object sender, EventArgs e)
        {
            if (rb1.Checked)
                panel16.Enabled = false;
            else
                panel16.Enabled = true;
        }

        private void FrmColorDLPACSetup_Shown(object sender, EventArgs e)
        {
            cbAC.SelectedIndex = SelectIndex;
            if (cbAC.Items.Count > 0 && cbAC.SelectedIndex < 0) cbAC.SelectedIndex = 0;
            comboBox1_SelectedIndexChanged(null, null);
        }
    }
}
