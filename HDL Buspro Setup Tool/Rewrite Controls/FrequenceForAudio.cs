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
    public partial class FrequenceForAudio : UserControl
    {
        public delegate void TextBoxChangedHandle(object sender, TextChangeEventArgs e);
        public event TextBoxChangedHandle UserControlValueChanged;
        private string text = "";
        private bool isSet = false;
        public FrequenceForAudio(string txt)
        {
            InitializeComponent();
            this.text = txt;
            SetControlValues();
        }

        private void FrequenceForAudio_Load(object sender, EventArgs e)
        {

        }

        private void num1_ValueChanged(object sender, EventArgs e)
        {
            if (isSet) return;
            
            if (Convert.ToInt32(num1.Value) == 6000)
            {
                num2.Value = 0;
            }
            this.text = num1.Value.ToString() + "." + num2.Value.ToString();
            if (UserControlValueChanged != null)
                UserControlValueChanged(this, new TextChangeEventArgs(this.text));
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

        private void SetControlValues()
        {
            isSet = true;
            num1.Value = Convert.ToInt32(text.Split('.')[0].ToString());
            num2.Value = Convert.ToInt32(text.Split('.')[1].ToString());
            num1.Refresh();
            num2.Refresh();
            isSet = false;
        }

        private void num2_ValueChanged(object sender, EventArgs e)
        {
            if (isSet) return;
            this.text = num1.Value.ToString() + "." + num2.Value.ToString();
            if (UserControlValueChanged != null)
                UserControlValueChanged(this, new TextChangeEventArgs(this.text));
        }
    }
}
