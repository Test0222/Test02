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
    public partial class UnivasalControl : UserControl
    {
        public UnivasalControl(string text)
        {
            InitializeComponent();
            this.text = text;
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

        public delegate void TextBoxChangedHandle(object sender, TextChangeEventArgs e);
        public event TextBoxChangedHandle UserControlValueChanged;
        private string text = "0/0";



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
            txt2.Text = str2;
            txt1_TextChanged(null, null);
            txt2_TextChanged(null, null);
        }

        private void txt2_TextChanged(object sender, EventArgs e)
        {
            if(txt2.Text.Length>=0)
            {
                txt2.Text = HDLPF.IsNumStringMode(txt2.Text, 0, 255);
                this.text = txt1.Text + "/" + txt2.Text;
                if (UserControlValueChanged != null)
                    UserControlValueChanged(this, new TextChangeEventArgs(this.text));
            }
        }

        private void txt1_TextChanged(object sender, EventArgs e)
        {
            if (txt1.Text.Length >= 0)
            {
                txt1.Text = HDLPF.IsNumStringMode(txt1.Text, 0, 255);
                this.text = txt1.Text + "/" + txt2.Text;
                if (UserControlValueChanged != null)
                    UserControlValueChanged(this, new TextChangeEventArgs(this.text));
            }
        }
    }
}
