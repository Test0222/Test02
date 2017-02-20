using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;


    public partial class LogicChannel : UserControl
    {
        public LogicChannel(string strName, int ChnID)
        {
            InitializeComponent();
            this.channelID = ChnID;
            if (strName != null)
            {
                label2.Text = strName;
            }
            trackBar2.ValueChanged +=new EventHandler(trackBar2_ValueChanged);
            textBox2.TextChanged+=new EventHandler(textBox2_TextChanged);
            textBox2.KeyPress += new KeyPressEventHandler(textBox2_KeyPress);
        }

        void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)8)
            {
                e.Handled = true;
            }
            //throw new NotImplementedException();
        }

        private int channelID;
        [DefaultValue(typeof(int), "0"), Category("Extended attributes")]
        [Description("Channel ID")]
        public virtual int ChannelID
        {
            set
            {
                channelID = value;
            }
        }


        [DefaultValue(typeof(string), ""), Category("Extended attributes")]
        [Description("Channel Remark")]
        public virtual string Remark
        {
            get
            {
                return label2.Text;
            }
            set
            {
                label2.Text = value;
            }
        }

        [DefaultValue(typeof(int), "0"), Category("Extended attributes")]
        [Description("Channel Light")]
        public virtual int Light
        {
            get
            {
                return -trackBar2.Value;
            }
            set
            {
                if (value > 255 || value < 0)
                {
                    this.trackBar2.Value = -255;
                }
                else
                {
                    trackBar2.Value = -value;
                }
            }
        }


        [Description("Channel Light Change event"), Category("Extended Event")]
        public event EventHandler LightValueChanged;

        private void trackBar2_ValueChanged(object sender, EventArgs e)
        {
            textBox2.Text =(trackBar2.Value * (-1)).ToString();
            if (LightValueChanged != null) LightValueChanged(this, e);
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            string str = textBox2.Text;
            if (str == "") textBox2.Text = "0";
            if (str == "") return;
            int num = Convert.ToInt32(str);
            if (num > 255)
                textBox2.Text = "255";
            
            trackBar2.Value = Convert.ToInt32(textBox2.Text) * (-1);
        }

        private void trackBar2_Scroll(object sender, ScrollEventArgs e)
        {

        }

        private void textBox2_TextChanged_1(object sender, EventArgs e)
        {

        }
    }

