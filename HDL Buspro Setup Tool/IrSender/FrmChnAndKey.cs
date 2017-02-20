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
    public partial class FrmChnAndKey : Form
    {
        private int DeviceType;
        private int IRID;
        private int IRDevice;
        private byte SubNetID;
        private byte DevID;
        public FrmChnAndKey()
        {
            InitializeComponent();
        }
        public FrmChnAndKey(int irid,int devicetype,int irdevice,byte subnetid,byte deviceid)
        {
            InitializeComponent();
            DeviceType = devicetype;
            IRID = irid;
            SubNetID = subnetid;
            DevID = deviceid;
            IRDevice = irdevice;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            CsConst.ChannelIDForNewIR = cbChannel.SelectedIndex;
            CsConst.KeyIDForNewIR = cbKey.SelectedIndex;
            if (panel9.Visible)
            {
                CsConst.arayACParamForNewIR = new byte[6];
                CsConst.arayACParamForNewIR[0] = Convert.ToByte(IRDevice);
                CsConst.arayACParamForNewIR[1] = Convert.ToByte(cbPower.SelectedIndex);
                CsConst.arayACParamForNewIR[2] = Convert.ToByte(cbMode.SelectedIndex);
                CsConst.arayACParamForNewIR[3] = Convert.ToByte(cbFan.SelectedIndex);
                CsConst.arayACParamForNewIR[4] = Convert.ToByte(sbTemp.Value);
                CsConst.arayACParamForNewIR[5] = Convert.ToByte(cbWind.SelectedIndex);
            }
            //DialogResult = DialogResult.OK;

            byte[] arayTmp = new byte[4];
            if (DeviceType == 729)
            {
                arayTmp = new byte[4];
                int int1 = 0;
                int int2 = CsConst.ChannelIDForNewIR + 1;
                string str1 = GlobalClass.IntToBit(int1, 2);
                string str2 = GlobalClass.IntToBit(int2, 6);
                string str = str1 + str2;
                arayTmp[0] = 1;
                arayTmp[1] = Convert.ToByte(GlobalClass.BitToInt(str));
                arayTmp[2] = Convert.ToByte(IRDevice);
                arayTmp[3] = Convert.ToByte(CsConst.KeyIDForNewIR + 1);
                if (CsConst.mySends.AddBufToSndList(arayTmp, 0xdb90, SubNetID, DevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                {
                    
                }
            }
            else if (DeviceType == 1301 || DeviceType == 1300 || DeviceType == 6100 || DeviceType == 908)
            {
                if (IRID == 5)
                {
                    arayTmp = new byte[13];
                    arayTmp[0] = Convert.ToByte(IRDevice);
                    arayTmp[3] = Convert.ToByte(sbTemp.Value);
                    arayTmp[4] = Convert.ToByte(sbTemp.Value);
                    arayTmp[5] = Convert.ToByte(sbTemp.Value);
                    arayTmp[6] = Convert.ToByte(sbTemp.Value);
                    arayTmp[7] = Convert.ToByte(sbTemp.Value);
                    Array.Copy(CsConst.arayACParamForNewIR, 1, arayTmp, 8, 5);
                    if (CsConst.mySends.AddBufToSndList(arayTmp, 0x193A, SubNetID, DevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                    {
                        
                    }
                }
                else
                {
                    arayTmp = new byte[4];
                    int int1 = 0;
                    int int2 = CsConst.ChannelIDForNewIR + 1;
                    string str1 = GlobalClass.IntToBit(int1, 2);
                    string str2 = GlobalClass.IntToBit(int2, 6);
                    string str = str1 + str2;
                    arayTmp[0] = Convert.ToByte(GlobalClass.BitToInt(str));
                    arayTmp[1] = Convert.ToByte(IRDevice);
                    if (arayTmp[1] > 4)
                        arayTmp[1] = Convert.ToByte(arayTmp[1] - 4);
                    arayTmp[2] = Convert.ToByte(CsConst.KeyIDForNewIR + 1);
                    if (CsConst.mySends.AddBufToSndList(arayTmp, 0xdb90, SubNetID, DevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                    {
                        
                    }
                }
            }
            else
            {
                arayTmp = new byte[4];
                int int1 = 0;
                int int2 = CsConst.ChannelIDForNewIR + 1;
                string str1 = GlobalClass.IntToBit(int1, 2);
                string str2 = GlobalClass.IntToBit(int2, 6);
                string str = str1 + str2;
                arayTmp[0] = Convert.ToByte(GlobalClass.BitToInt(str));
                arayTmp[1] = Convert.ToByte(IRDevice);
                arayTmp[2] = Convert.ToByte(CsConst.KeyIDForNewIR + 1);
                if (CsConst.mySends.AddBufToSndList(arayTmp, 0xdb90, SubNetID, DevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                {
                    
                }

            }
            Cursor.Current = Cursors.Default;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void sbTemp_ValueChanged(object sender, EventArgs e)
        {
            TempValue.Text = sbTemp.Value.ToString() + "C";
        }

        private void FrmChnAndKey_Load(object sender, EventArgs e)
        {
            if (DeviceType == 6100)
            {
                cbChannel.Items.Clear();
                for (int i = 1; i <= 3; i++)
                    cbChannel.Items.Add(i.ToString());
            }
            else if (DeviceType == 908)
            {
                cbChannel.Items.Clear();
                for (int i = 1; i <= 8; i++)
                    cbChannel.Items.Add(i.ToString());
            }
            if (cbChannel.Items.Count > 0)
                cbChannel.SelectedIndex = 0;
            cbKey.Items.Clear();
            cbKey.Items.AddRange(CsConst.mstrINIDefault.IniReadValuesInALlSectionStr("NewIRDevice" + IRID.ToString()));
            if (cbKey.Items.Count > 0)
                cbKey.SelectedIndex = 0;

            cbPower.Items.Clear();
            cbPower.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00038", ""));
            cbPower.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00037", ""));
            cbPower.SelectedIndex = 1;
            cbMode.Items.Clear();
            for (int i = 0; i < 5; i++)
            {
                cbMode.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "0005" + i.ToString(), ""));
            }
            cbMode.SelectedIndex = 0;
            cbFan.Items.Clear();
            for (int i = 0; i < 4; i++)
            {
                cbFan.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "0006" + i.ToString(), ""));
            }
            cbFan.SelectedIndex = 0;
            cbWind.Items.Clear();
            cbWind.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00038", ""));
            cbWind.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00037", ""));
            cbWind.SelectedIndex = 0;
            sbTemp_ValueChanged(null, null);
            panel9.Visible = false;
            if (IRID == 5 && DeviceType == 1301 && IRDevice <= 4)
            {
                panel9.Visible = true;
            }

            if (IRID == 5 && DeviceType == 1300 && IRDevice <= 4)
            {
                panel9.Visible = true;
            }

            if (IRID == 5 && DeviceType == 6100 && IRDevice <= 3)
            {
                panel9.Visible = true;
            }

            if (IRID == 5 && DeviceType == 908 && IRDevice <= 8)
            {
                panel9.Visible = true;
            }
        }
    }
}
