using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Linq;
using System.Windows.Forms;

namespace HDL_Buspro_Setup_Tool
{
    public partial class NetworkInForm : UserControl
    {
        public Byte SubNetID;
        public Byte DeviceID;
        public int DeviceType = -1;
        public NetworkInformation TmpNetwork = new NetworkInformation();
        private bool isReadding = false;

        public NetworkInForm()
        {
            InitializeComponent();
        }

        void NetworkInfomation_SizeChanged(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            panel2.Width = this.Width / 3;
            panel2.Height = this.Height;
            panel2.Left = 0;

            btnRead.Width = this.Width / 4;
            btnRead.Left = txtSwRoutIP.Left;

            btnModify.Width = this.Width / 4;
            btnModify.Left = btnRead.Left + 8 + btnRead.Width;

            txtSwiIP.Width = this.Width / 2;
            txtSwiIP.Top = lbIP.Top + 2 + panel2.Top;
            txtSwiIP.Left = panel2.Width + 8;

            txtSwRoutIP.Width = this.Width / 2;
            txtSwRoutIP.Top = lbRouterIP.Top + 2 + panel2.Top;
            txtSwRoutIP.Left = panel2.Width + 8;

            MaskIP.Width = this.Width / 2;
            MaskIP.Top = lbMask.Top + 2 + panel2.Top;
            MaskIP.Left = panel2.Width + 8;

            panel1.Width = this.Width / 2;
            panel1.Top = lbMAC.Top + 2 + panel2.Top;
            panel1.Left = panel2.Width + 8;

            tbPort.Width = this.Width / 2;
            tbPort.Top = lbport.Top + 2 + panel2.Top;
            tbPort.Left = panel2.Width + 8;
        }

        public NetworkInForm(byte SubNetID, byte DevID, int DevType)
        {
            InitializeComponent();
            this.SizeChanged += NetworkInfomation_SizeChanged;
            this.SubNetID = SubNetID;
            this.DeviceID = DevID;
            this.DeviceType = DevType;

            SetCtrlsVisbleWithDifferentDeviceType();
        }

        public void DisplayNetworkInfomation()
        {
            isReadding = true;
            try
            {
                string strIP, strRouterIP, strport, strSubMark,StrDns,strDns1;

                strIP = TmpNetwork.ipAddress[0].ToString() + "." + TmpNetwork.ipAddress[1].ToString() + "."                        
                      + TmpNetwork.ipAddress[2].ToString() + "." + TmpNetwork.ipAddress[3].ToString();

                strRouterIP = TmpNetwork.routerIp[0].ToString() + "." + TmpNetwork.routerIp[1].ToString()+ "."
                            + TmpNetwork.routerIp[2].ToString() + "." + TmpNetwork.routerIp[3].ToString();

                this.txt4.Text = TmpNetwork.macAddress[3] .ToString("X2");
                this.txt5.Text = TmpNetwork.macAddress[4].ToString("X2");
                this.txt6.Text = TmpNetwork.macAddress[5].ToString("X2");

                strport = (TmpNetwork.port[0] * 256 + TmpNetwork.port[1]).ToString();

                strSubMark = TmpNetwork.ipGateway[0].ToString() + "." + TmpNetwork.ipGateway[1].ToString() + "." 
                           + TmpNetwork.ipGateway[2].ToString() + "." + TmpNetwork.ipGateway[3].ToString();

                StrDns = TmpNetwork.dnsOne[0].ToString() + "." + TmpNetwork.dnsOne[1].ToString() + "." 
                           + TmpNetwork.dnsOne[2].ToString() + "." + TmpNetwork.dnsOne[3].ToString();

                strDns1 = TmpNetwork.dnsTwo[0].ToString() + "." + TmpNetwork.dnsTwo[1].ToString() + "." 
                           + TmpNetwork.dnsTwo[2].ToString() + "." + TmpNetwork.dnsTwo[3].ToString();

                this.txtSwiIP.Text = strIP;
                this.txtSwRoutIP.Text = strRouterIP;
                MaskIP.Text = strSubMark;
                chbAuto.Checked = TmpNetwork.dhcp;

                if (TmpNetwork.dhcp)
                {
                    if (TmpNetwork.currentState ==1)
                        lbAuto.Text = CsConst.mstrINIDefault.IniReadValue("public", "99809", "");
                    else
                        lbAuto.Text = CsConst.mstrINIDefault.IniReadValue("public", "99808", "");
                }
                chbDns.Checked = TmpNetwork.dnsAuto;
                DNS.Text = StrDns;
                Dns1.Text = strDns1;
            }
            catch
            {
            }
            isReadding = false;
        }

        private void txt4_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (isReadding) return;
                string str = (sender as TextBox).Text;
                int intTmp = Convert.ToInt32(str,16);
                if (intTmp > 255)
                {
                    (sender as TextBox).Text = "1";
                }
                txtSwiIP_TextChanged(null, null);
            }
            catch
            {
            }
        }

        public void btnModify_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            btnModify.Enabled = false;
            try
            {
                TmpNetwork.ModifyNetworkInfomation(SubNetID,DeviceID);
            }
            catch
            {
            }
            Cursor.Current = Cursors.Default;
            btnModify.Enabled = true;
        }

        private void txtSwiIP_TextChanged(object sender, EventArgs e)
        {
            if (isReadding == true) return;
            try
            {
                String strIP, strRouterIP, strIPMac, strMaskIP;
                String DnsAddressOne, DnsAddressTwo;
                int Port = 6000;
                strIP = txtSwiIP.Text;
                String[] TmpStringArray = strIP.Split('.');
                Byte[] TmpByteArray = new Byte[4];
                Byte bytI = 0;
                foreach (String Tmp in TmpStringArray)
                {
                    if (Tmp != null && Tmp != "")
                    {
                        TmpByteArray[bytI] = Convert.ToByte(Tmp);
                        bytI++;
                    }
                }
                TmpNetwork.ipAddress = (Byte[])TmpByteArray.Clone();

                strRouterIP = txtSwRoutIP.Text;
                TmpStringArray = strRouterIP.Split('.');
                bytI = 0;
                foreach (String Tmp in TmpStringArray)
                {
                    if (Tmp != null && Tmp != "")
                    {
                        TmpByteArray[bytI] = Convert.ToByte(Tmp);
                        bytI++;
                    }
                }
                TmpNetwork.routerIp = TmpByteArray;

                strIPMac = txt1.Text + ":" + txt4.Text + ":" + txt5.Text + ":" + txt6.Text;
                TmpStringArray = strIPMac.Split(':');
                TmpByteArray = new Byte[6];
                bytI = 0;
                foreach (string Tmp in TmpStringArray)
                {
                    if (Tmp != null && Tmp != "")
                    {
                        TmpByteArray[bytI] = Convert.ToByte(Tmp,16);
                        bytI++;
                    }
                }
                TmpNetwork.macAddress = TmpByteArray;

                Port = Convert.ToInt32(tbPort.Text);
                TmpNetwork.port[0] = Convert.ToByte((Port / 256).ToString());
                TmpNetwork.port[1] = Convert.ToByte((Port % 256).ToString());

                strMaskIP = MaskIP.Text;
                TmpByteArray = new Byte[4];
                TmpStringArray = strMaskIP.Split('.');
                bytI = 0;
                foreach (String Tmp in TmpStringArray)
                {
                    if (Tmp != null && Tmp != "")
                    {
                        TmpByteArray[bytI] = Convert.ToByte(Tmp);
                        bytI++;
                    }
                }
                TmpNetwork.ipGateway = TmpByteArray;

                DnsAddressOne = DNS.Text;
                TmpByteArray = new Byte[4];
                TmpStringArray = DnsAddressOne.Split('.');
                bytI = 0;
                foreach (String Tmp in TmpStringArray)
                {
                    if (Tmp != null && Tmp != "")
                    {
                        TmpByteArray[bytI] = Convert.ToByte(Tmp);
                        bytI++;
                    }
                }
                TmpNetwork.dnsOne = TmpByteArray;

                DnsAddressTwo = Dns1.Text;
                TmpByteArray = new Byte[4];
                TmpStringArray = DnsAddressTwo.Split('.');
                bytI = 0;
                foreach (String Tmp in TmpStringArray)
                {
                    if (Tmp != null && Tmp != "")
                    {
                        TmpByteArray[bytI] = Convert.ToByte(Tmp);
                        bytI++;
                    }
                }
                TmpNetwork.dnsTwo = TmpByteArray;

            }
            catch
            {
            }
        }

        private void tbPort_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar) || e.KeyChar == 8 || (e.KeyChar >= 'a' && e.KeyChar <= 'f') || (e.KeyChar >= 'A' && e.KeyChar <= 'F'))
                e.Handled = false;
            else
                e.Handled = true;
        }

        public void btnRead_Click(object sender, EventArgs e)
        {
            isReadding = true;
            if (TmpNetwork == null) TmpNetwork = new NetworkInformation();
            TmpNetwork.readNetworkInfomation(SubNetID, DeviceID);
            DisplayNetworkInfomation();
        }

        private void NetworkInForm_Load(object sender, EventArgs e)
        {
            InitialFormCtrlsTextOrItems();
            if (CsConst.MyEditMode == 0) //工程师模式
            {
                DisplayNetworkInfomation();
            }
            else
            {
                btnRead_Click(btnRead, null);
            }
        }

        void InitialFormCtrlsTextOrItems()
        {
            if (CsConst.iLanguageId == 1)
            {
                lbRouterIP.Text = CsConst.mstrINIDefault.IniReadValue("NetworkInfomation", "00001", "");
                lbMask.Text = CsConst.mstrINIDefault.IniReadValue("NetworkInfomation", "00003", "");
                lbport.Text = CsConst.mstrINIDefault.IniReadValue("NetworkInfomation", "00004", "");
                btnModify.Text = CsConst.mstrINIDefault.IniReadValue("NetworkInfomation", "00008", "");
                btnRead.Text = CsConst.mstrINIDefault.IniReadValue("NetworkInfomation", "00007", "");
                chbAuto.Text = CsConst.mstrINIDefault.IniReadValue("NetworkInfomation", "00005", "");
                btnRead.ForeColor = Color.Black;
            }
            txtSwiIP.UserControlValueChanged += txtSwiIP_TextChanged;
            txtSwRoutIP.UserControlValueChanged += txtSwiIP_TextChanged;
            MaskIP.UserControlValueChanged += txtSwiIP_TextChanged;
            DNS.UserControlValueChanged += txtSwiIP_TextChanged;
            Dns1.UserControlValueChanged += txtSwiIP_TextChanged;
        }

        public void SetCtrlsVisbleWithDifferentDeviceType()
        {
            Boolean DisplayDHCP = !IPmoduleDeviceTypeList.IpModuleV1.Contains(DeviceType);
            Boolean TimeZoneUrl = IPmoduleDeviceTypeList.IpModuleV2DHCP.Contains(DeviceType);
            MaskIP.Enabled = (DeviceType != 854);
            chbAuto.Visible = DisplayDHCP;
            lbAuto.Visible = DisplayDHCP;

            chbDns.Visible = TimeZoneUrl;
            lbDns.Visible = TimeZoneUrl;
            lbDns1.Visible = TimeZoneUrl;
            DNS.Visible = TimeZoneUrl;
            Dns1.Visible = TimeZoneUrl;

            if (DNS.Visible)
            {
                this.Height = 321;
            }
            else
            {
                this.Height = 217;
            }
            btnModify.Visible = (DeviceType != 51966);
        }

        private void chbAuto_CheckedChanged(object sender, EventArgs e)
        {
            chbDns.Checked = chbAuto.Checked;
            DNS.Enabled = !(chbDns.Checked);
            Dns1.Enabled = !(chbDns.Checked);
            TmpNetwork.dhcp = chbDns.Checked;
        }

        private void chbDns_CheckedChanged(object sender, EventArgs e)
        {
            TmpNetwork.dnsAuto = chbAuto.Checked;
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
}

