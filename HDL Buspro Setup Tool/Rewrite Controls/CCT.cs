using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;


    public partial class CCT : UserControl
    {
        private string channelID;
        public CCT(string ChnID,int bytPercent)
        {
            InitializeComponent();
            this.channelID = ChnID;
            this.textBox1.Text = ChnID.Trim();
            this.Light = bytPercent;
           // trackBar1.Value = bytPercent;

            this.SizeChanged += new EventHandler(CCT_SizeChanged);
            trackBar1.ValueChanged += new EventHandler(trackBar1_ValueChanged);
            textBox2.TextChanged+=new EventHandler(textBox2_TextChanged);
            textBox2.KeyPress += new KeyPressEventHandler(textBox2_KeyPress);
        }

        void CCT_SizeChanged(object sender, EventArgs e)
        {
            label1.Width = this.Width /12;
            trackBar1.Width = this.Width / 2;
            label2.Width = this.Width /12;
            textBox2.Width = this.Width / 6;
            textBox1.Width = this.Width / 6;

            label1.Location = new System.Drawing.Point(0, 0);
            trackBar1.Location = new System.Drawing.Point(label1.Width + label1.Location.X, 0);
            label2.Location = new System.Drawing.Point(trackBar1.Width + trackBar1.Location.X, 0);
            textBox2.Location = new System.Drawing.Point(label2.Width + label2.Location.X, 0);
            textBox1.Location = new System.Drawing.Point(textBox2.Width + textBox2.Location.X, 0);
        }

        void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)8)
            {
                e.Handled = true;
            }
            //throw new NotImplementedException();
        }

        [DefaultValue(typeof(int), "0"), Category("Extended attributes")]
        [Description("Channel Light")]
        public virtual int Light
        {
            get
            {
                return trackBar1.Value;
            }
            set
            {
                if (value > 100 || value < 0)
                {
                    this.trackBar1.Value = 0;
                }
                else
                {
                    trackBar1.Value = value;
                    textBox2.Text = trackBar1.Value.ToString();
                }
            }
        }

        [DefaultValue(typeof(string), "2700 K"), Category("Extended attributes")]
        [Description("Kelvin Scale")]
        public virtual string _channelID
        {
            get
            {
                return textBox1.Text.ToString();
            }
            set
            {
                textBox1.Text = value;
            }
        }


        [Description("Channel Light Change event"), Category("Extended Event")]
        public event EventHandler LightValueChanged;

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            textBox2.Text = trackBar1.Value.ToString();
            if (LightValueChanged != null) LightValueChanged(this, e);
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            string str = textBox2.Text;
            if (str == "") textBox2.Text = "0";
            if (str == "") return;
            int num = Convert.ToInt32(str);
            if (num > 100)
                textBox2.Text = "100";

            trackBar1.Value = Convert.ToInt32(textBox2.Text);
        }
    }

