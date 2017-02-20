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
    public partial class IRControl : UserControl
    {
        public IRControl(string str)
        {
            InitializeComponent();
            this.text = str;
            cb1.Items.Clear();
            cb1.Items.Add(CsConst.Status[0]);
            cb1.Items.Add(CsConst.Status[1]);
            SetControlValues();
        }

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

        public new string Text
        {
            get
            {
                return text;
            }
            set
            {
                text = value;
                SetControlValues();
            }
        }

        private void txt1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar) || e.KeyChar == 8)
                e.Handled = false;
            else
                e.Handled = true;
        }

        private void SetControlValues()
        {
            string str1 = text.Split('/')[0];
            string str2 = text.Split('/')[1];
            txt1.Text = str1;
            cb1.SelectedIndex = cb1.Items.IndexOf(str2);
            if (cb1.Items.Count > 0 && cb1.SelectedIndex < 0) cb1.SelectedIndex = 0;
            txt1_TextChanged(null, null);
        }

        public delegate void TextBoxChangedHandle(object sender, TextChangeEventArgs e);
        public event TextBoxChangedHandle UserControlValueChanged;
        private string text = "0/Off";

        private void cb1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.text = txt1.Text + "/" + cb1.Text;
            if (UserControlValueChanged != null)
                UserControlValueChanged(this, new TextChangeEventArgs(this.text));
        }

        private void txt1_TextChanged(object sender, EventArgs e)
        {
            if (txt1.Text.Length >= 0)
            {
                txt1.Text = HDLPF.IsNumStringMode(txt1.Text, 0, 100);
                this.text = txt1.Text + "/" + cb1.Text;
                if (UserControlValueChanged != null)
                    UserControlValueChanged(this, new TextChangeEventArgs(this.text));
            }
        }
    }
}
