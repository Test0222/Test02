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
    public partial class BackNetID : UserControl
    {
        public BackNetID(string text)
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
        private string text = "";

        private void txtID_TextChanged(object sender, EventArgs e)
        {
            string str = txtID.Text;
            if (!GlobalClass.IsNumeric(str))
            {
                str = "0";
            }
            else
            {
                if (str.Length > 10) str = "0";
                if ((Convert.ToInt32(str.Substring(0, 1)) > 1) && str.Length >= 10) str = "0";
                int intTmp = Convert.ToInt32(str);
                if (intTmp > 0x7FFFFFFF)
                {
                    str = "0";
                }
            }
            txtID.Text = str;
            this.text = cbID.Text + ":" + txtID.Text;
            if (UserControlValueChanged != null)
                UserControlValueChanged(this, new TextChangeEventArgs(this.text));
        }

        private void cbID_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.text = cbID.Text + ":" + txtID.Text;
            if (UserControlValueChanged != null)
                UserControlValueChanged(this, new TextChangeEventArgs(this.text));
        }

        private void SetControlValues()
        {
            string str1 = text.Split(':')[0];
            string str2 = text.Split(':')[1];
            cbID.SelectedIndex = cbID.Items.IndexOf(str1);
            txtID.Text = str2;
            if (cbID.SelectedIndex < 0) cbID.SelectedIndex = 0;
            if (txtID.Text.Length <= 0) txtID.Text = "0";
        }
    }
}
