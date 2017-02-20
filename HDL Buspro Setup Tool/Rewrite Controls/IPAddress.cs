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
    public partial class IPAddressNew : UserControl
    {
        public delegate void TextBoxChangedHandle(object sender, TextChangeEventArgs e);
        public event TextBoxChangedHandle UserControlValueChanged;
        private string text = "0.0.0.0";

        public IPAddressNew()
        {
            InitializeComponent();

            this.SizeChanged += new EventHandler(IPAddress_SizeChanged);
            IPAddress_SizeChanged(null, null);
        }

        int inputIndex = 3;

        public class TextChangeEventArgs : EventArgs
        {
            private string message;
            public TextChangeEventArgs(string message)
            {
                this.message = message;

            }
            public string Message
            {
                get { return message; }
            }
        }

        void IPAddress_SizeChanged(object sender, EventArgs e)
        {
            ttxIp1.Width = this.Width / 5;
            ttxIp1.Height = this.Height;
            ttxIp1.Left = 0;

            ttxIp2.Width = this.Width / 5;
            ttxIp2.Height = this.Height;
            ttxIp2.Left = lbl1.Left+ lbl1.Width;

            ttxIp3.Width = this.Width / 5;
            ttxIp3.Height = this.Height;
            ttxIp3.Left = lbl2.Left + lbl2.Width;

            ttxIp4.Width = this.Width / 5;
            ttxIp4.Height = this.Height;
            ttxIp4.Left = lbl3.Left + lbl3.Width;

            lbl1.Width = this.Width / 15;
            lbl1.Height = this.Height;
            lbl1.Left = ttxIp1.Width;

            lbl2.Width = this.Width / 15;
            lbl2.Height = this.Height;
            lbl2.Left = ttxIp2.Left + ttxIp2.Width;

            lbl3.Width = this.Width / 15;
            lbl3.Height = this.Height;
            lbl3.Left = ttxIp3.Left + ttxIp3.Width;
        }

        #region [属性]
        /// <summary>
        /// IP类别：1为IP地址；2为子网掩码；3为网关
        /// </summary>
        private int _iIPType = 1;
        private string _IPAddressValue = "";
        [Description("IP地址类别：1为IP地址；2为子网掩码；3为网关；DNS设为1"), Category("IP地址类别")]
        public int iIPType
        {
            set
            {
                _iIPType = value;
            }
            get
            {
                return _iIPType;
            }
        }
        [Description("IP地址"), Category("文本")]
        [Browsable(true)]
        public string IPAddressValue
        {
            set
            {
                _IPAddressValue = value;
                string[] ipMessage = _IPAddressValue.Split('.');
                ttxIp1.Text = ipMessage[0];
                ttxIp2.Text = ipMessage[1];
                ttxIp3.Text = ipMessage[2];
                ttxIp4.Text = ipMessage[3];
            }
            get
            {
                if (ttxIp1.Text != "0")
                {
                    ttxIp1.Text = ttxIp1.Text.TrimStart('0');//不出错,还是显示之前输入的ttxIp1.text的内容
                }
                else
                {
                    ttxIp1.Text = "0";
                }
                if (ttxIp2.Text != "0")
                {
                    ttxIp2.Text = ttxIp2.Text.TrimStart('0');//不出错,还是显示之前输入的ttxIp1.text的内容
                }
                else
                {
                    ttxIp2.Text = "0";
                }
                if (ttxIp3.Text != "0")
                {
                    ttxIp3.Text = ttxIp3.Text.TrimStart('0');//不出错,还是显示之前输入的ttxIp1.text的内容
                }
                else
                {
                    ttxIp3.Text = "0";
                }
                if (ttxIp4.Text != "0")
                {
                    ttxIp4.Text = ttxIp4.Text.TrimStart('0');//不出错,还是显示之前输入的ttxIp1.text的内容
                }
                else
                {
                    ttxIp4.Text = "0";
                }
                _IPAddressValue = ttxIp1.Text + "." + ttxIp2.Text + "." + ttxIp3.Text + "." + ttxIp4.Text;
                return _IPAddressValue;
            }
        }
        #endregion

        #region [方法]

        #region [TextChange方法]
        /// <summary>
        /// 验证IP输入
        /// </summary>
        /// <param name="strIPInput">录入的IP地址信息</param>
        /// <param name="iIpStart">用于判断的起始地址</param>
        /// <param name="iIpEnd">用于判断的结束地址</param>
        /// <returns></returns>
        private string ValidateIPTextChange(string strIPInput, int iIpStart, int iIpEnd)
        {
            if (strIPInput == null || strIPInput == "") return "0";
            string strReturn = "0";
            int iIPS = 0;
            try
            {
                iIPS = Convert.ToInt32(strIPInput);
                if (iIPS < iIpStart || iIPS > iIpEnd)
                {
                    MessageBox.Show(iIPS + "不是一个有效项目。请指定一个介于" + iIpStart + "到" + iIpEnd + "之间的数值！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    if (iIPS < iIpStart)
                    {
                        strReturn = iIpStart.ToString();
                    }
                    if (iIPS > iIpEnd)
                    {
                        strReturn = iIpEnd.ToString();
                    }
                }
            }
            catch
            {
               // MessageBox.Show("请输入正整数！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                strReturn = iIpStart.ToString();
            }
            return strReturn;
        }
        #endregion

        #endregion

        #region [事件]

        #region [键盘TextChange事件]
        private void ttxIp1_TextChanged(object sender, EventArgs e)
        {
            string strGet = "0";
            switch (iIPType)
            {
                case 1:
                    //IP地址
                    strGet = ValidateIPTextChange(ttxIp1.Text.Trim(), 0, 255);
                    break;
                case 2:
                    //子网掩码
                    strGet = ValidateIPTextChange(ttxIp1.Text.Trim(), 0, 255);
                    break;
                case 3:
                    //网关
                    strGet = ValidateIPTextChange(ttxIp1.Text.Trim(), 0, 255);
                    break;
                default:
                    strGet = ValidateIPTextChange(ttxIp1.Text.Trim(), 0, 255);
                    break;
            }
            if (strGet == "0")
            {
                //不出错,还是显示之前输入的ttxIp1.text的内容                
            }
            else
            {
                ttxIp1.Text = strGet;//出错了就显示try，catch中付的值
                //ttxIp1.Focus();
            }
             if (UserControlValueChanged != null)
                UserControlValueChanged(this, new TextChangeEventArgs(this.text));
        }

        private void ttxIp2_TextChanged(object sender, EventArgs e)
        {
            string strGet = "0";
            switch (iIPType)
            {
                case 1:
                    strGet = ValidateIPTextChange(ttxIp2.Text.Trim(), 0, 255);
                    break;
                case 2:
                    strGet = ValidateIPTextChange(ttxIp2.Text.Trim(), 0, 255);
                    break;
                case 3:
                    strGet = ValidateIPTextChange(ttxIp2.Text.Trim(), 0, 255);
                    break;
                default:
                    strGet = ValidateIPTextChange(ttxIp2.Text.Trim(), 0, 255);
                    break;
            }
            if (strGet == "0")
            {
                //不出错
            }
            else
            {
                ttxIp2.Text = strGet;
                //ttxIp2.Focus();
            }
             if (UserControlValueChanged != null)
                UserControlValueChanged(this, new TextChangeEventArgs(this.text));
        }

        private void ttxIp3_TextChanged(object sender, EventArgs e)
        {
            string strGet = "0";
            switch (iIPType)
            {
                case 1:
                    strGet = ValidateIPTextChange(ttxIp3.Text.Trim(), 0, 255);
                    break;
                case 2:
                    strGet = ValidateIPTextChange(ttxIp3.Text.Trim(), 0, 255);
                    break;
                case 3:
                    strGet = ValidateIPTextChange(ttxIp3.Text.Trim(), 0, 255);
                    break;
                default:
                    strGet = ValidateIPTextChange(ttxIp3.Text.Trim(), 0, 255);
                    break;
            }
            if (strGet == "0")
            {
                //不出错
            }
            else
            {
                ttxIp3.Text = strGet;
                //ttxIp3.Focus();
            }
             if (UserControlValueChanged != null)
                UserControlValueChanged(this, new TextChangeEventArgs(this.text));
        }

        private void ttxIp4_TextChanged(object sender, EventArgs e)
        {
            string strGet = "0";
            switch (iIPType)
            {
                case 1:
                    //IP地址
                    strGet = ValidateIPTextChange(ttxIp4.Text.Trim(), 0, 255);
                    break;
                case 2:
                    strGet = ValidateIPTextChange(ttxIp4.Text.Trim(), 0, 255);
                    break;
                case 3:
                    strGet = ValidateIPTextChange(ttxIp4.Text.Trim(), 0, 255);
                    break;
                default:
                    strGet = ValidateIPTextChange(ttxIp4.Text.Trim(), 0, 255);
                    break;
            }
            if (strGet == "0")
            {
                //不出错
            }
            else
            {
                ttxIp4.Text = strGet;
                //ttxIp4.Focus();
            }
             if (UserControlValueChanged != null)
                UserControlValueChanged(this, new TextChangeEventArgs(this.text));
        }
        #endregion

        #region [键盘KeyPress事件]
        private void ttxIp1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar < '0' || e.KeyChar > '9') && e.KeyChar != 8 && e.KeyChar != 46)
            {
                //除0--9数字键、退格键(keychar为8)、句号键(46)),其他全禁用
                e.Handled = true;
            }
            else if (e.KeyChar == 46 || e.KeyChar == 8)
            {
                e.Handled = true;
                string strGet = "0";
                switch (iIPType)
                {
                    case 1:
                        //IP地址
                        strGet = ValidateIPTextChange(ttxIp3.Text.Trim(), 1, 223);
                        break;
                    case 2:
                        //子网掩码
                        strGet = ValidateIPTextChange(ttxIp3.Text.Trim(), 0, 255);
                        break;
                    case 3:
                        //网关
                        strGet = ValidateIPTextChange(ttxIp3.Text.Trim(), 1, 223);
                        break;
                    default:
                        strGet = ValidateIPTextChange(ttxIp3.Text.Trim(), 1, 223);
                        break;
                }
                if (strGet != "")
                {
                    if (e.KeyChar == 46)
                        ttxIp2.Focus();
                }
                else
                {
                    ttxIp1.Text = strGet;
                    ttxIp1.Focus();
                }
            }
        }

        private void ttxIp2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar < '0' || e.KeyChar > '9') && e.KeyChar != 8 && e.KeyChar != 46)
            {
                e.Handled = true;
            }
            else if (e.KeyChar == 46 || e.KeyChar ==8)
            {
                e.Handled = true;

                string strGet = "0";
                switch (iIPType)
                {
                    case 1:
                        //IP地址
                        strGet = ValidateIPTextChange(ttxIp3.Text.Trim(), 1, 223);
                        break;
                    case 2:
                        //子网掩码
                        strGet = ValidateIPTextChange(ttxIp3.Text.Trim(), 0, 255);
                        break;
                    case 3:
                        //网关
                        strGet = ValidateIPTextChange(ttxIp3.Text.Trim(), 1, 223);
                        break;
                    default:
                        strGet = ValidateIPTextChange(ttxIp3.Text.Trim(), 1, 223);
                        break;
                }
                if (strGet != "")
                {
                    if (e.KeyChar == 8)
                      ttxIp1.Focus(); 
                    else 
                      ttxIp3.Focus();
                }
                else 
                {
                    ttxIp2.Text = strGet;
                    ttxIp1.Focus();
                }
            }
        }

        private void ttxIp3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar < '0' || e.KeyChar > '9') && e.KeyChar != 8 && e.KeyChar != 46)
            {
                e.Handled = true;
            }
            else if (e.KeyChar == 46 || e.KeyChar == 8)
            {
                e.Handled = true;

                string strGet = "0";
                switch (iIPType)
                {
                    case 1:
                        //IP地址
                        strGet = ValidateIPTextChange(ttxIp3.Text.Trim(), 1, 223);
                        break;
                    case 2:
                        //子网掩码
                        strGet = ValidateIPTextChange(ttxIp3.Text.Trim(), 0, 255);
                        break;
                    case 3:
                        //网关
                        strGet = ValidateIPTextChange(ttxIp3.Text.Trim(), 1, 223);
                        break;
                    default:
                        strGet = ValidateIPTextChange(ttxIp3.Text.Trim(), 1, 223);
                        break;
                }
                if (strGet == "0")
                {
                    if (e.KeyChar == 8)
                      ttxIp3.Focus(); 
                    else 
                      ttxIp4.Focus();
                }
                else
                {
                    ttxIp3.Text = strGet;
                    ttxIp3.Focus();
                }
            }
        }

        private void ttxIp4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar < '0' || e.KeyChar > '9') && e.KeyChar != 8 && e.KeyChar != 46 && e.KeyChar != 8)
            {
                e.Handled = true;
            }
            else if (e.KeyChar == 46 ||e.KeyChar ==8)
            {
                e.Handled = true;

                string strGet = "0";
                switch (iIPType)
                {
                    case 1:
                        //IP地址
                        strGet = ValidateIPTextChange(ttxIp4.Text.Trim(), 1, 223);
                        break;
                    case 2:
                        //子网掩码
                        strGet = ValidateIPTextChange(ttxIp4.Text.Trim(), 0, 255);
                        break;
                    case 3:
                        //网关
                        strGet = ValidateIPTextChange(ttxIp4.Text.Trim(), 1, 223);
                        break;
                    default:
                        strGet = ValidateIPTextChange(ttxIp4.Text.Trim(), 1, 223);
                        break;
                }
                if (strGet == "0")
                {
                    if (e.KeyChar == 8) ttxIp3.Focus();
                }
                else
                {
                    ttxIp4.Text = strGet;
                    ttxIp4.Focus();
                }
            }
        }
        #endregion

        #endregion

        private void ttxIp_Enter(object sender, EventArgs e)
        {
            switch (((TextBox)sender).Name)
            {
                case "ttxIp4":
                    inputIndex = 3;
                    break;
                case "ttxIp3":
                    inputIndex = 2;
                    break;
                case "ttxIp2":
                    inputIndex = 1;
                    break;
                case "ttxIp1":
                    inputIndex = 0;
                    break;
                default:
                    inputIndex = 3;
                    break;
            }
        }

        public new string Text
        {
            get
            {
                string str1 = ttxIp1.Text;
                string str2 = ttxIp2.Text;
                string str3 = ttxIp3.Text;
                string str4 = ttxIp4.Text;
                str1 = HDLPF.IsNumStringMode(str1, 0, 255);
                str2 = HDLPF.IsNumStringMode(str2, 0, 255);
                str3 = HDLPF.IsNumStringMode(str3, 0, 255);
                str4 = HDLPF.IsNumStringMode(str4, 0, 255);
                return str1 + "." + str2 + "." + str3 + "." + str4;
            }
            set
            {
                text = value;
                string[] arayIP = text.Split('.');
                ttxIp1.Text = arayIP[0].ToString();
                ttxIp2.Text = arayIP[1].ToString();
                ttxIp3.Text = arayIP[2].ToString();
                ttxIp4.Text = arayIP[3].ToString();
            }
        }
  
    }
}
