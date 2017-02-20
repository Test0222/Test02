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
    public partial class SingleChn : UserControl
    {
        public delegate void TextChangedEventHandler(object sender, EventArgs e);
        public event TextChangedEventHandler TextChanged;//ColorChange会在属性窗口显示  

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


        public virtual string Text
        {
            get
            {
                return txtValue.Text;
            }
            set
            {
                txtValue.Text = value;
                SetControlValues();
            }
        }

        public SingleChn()
        {
            InitializeComponent();
            trackBar.ValueChanged += new EventHandler(trackBar2_ValueChanged);
            txtValue.TextChanged += new EventHandler(textBox2_TextChanged);
            txtValue.KeyPress += new KeyPressEventHandler(textBox2_KeyPress);
            SetControlValues();
        }

        void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar) || e.KeyChar == 8)
                e.Handled = false;
            else
                e.Handled = true;
        }        

        private void SetControlValues()
        {
            if (Text == "" || Text == null) Text = "0";
            trackBar.Value = Convert.ToInt32(Text);
            txtValue.Text = Text;
        }

        private void trackBar2_ValueChanged(object sender, EventArgs e)
        {
            txtValue.Text = trackBar.Value.ToString();
            Text = trackBar.Value.ToString();
        }

        public void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (txtValue.Text.Length > 0)
            {
                txtValue.Text = HDLPF.IsNumStringMode(txtValue.Text, 0, 100);
                txtValue.SelectionStart = txtValue.Text.Length;
                trackBar.Value = Convert.ToInt32(txtValue.Text);
                Text = txtValue.Text;
            }
            if (TextChanged != null)
                TextChanged(sender, new EventArgs());
        }

        public void trackBar_Scroll(object sender, ScrollEventArgs e)
        {

        }


    }
}

