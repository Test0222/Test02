using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;


    public partial class RGBW : UserControl
    {
        private Color myColor = Color.Black;

        public RGBW(Color oColor,int ChnID)
        {
            InitializeComponent();

            this._myColor = oColor;
            this.Light = ChnID;

            this.SizeChanged += new EventHandler(RGBW_SizeChanged);
            RGBW_SizeChanged(null, null);

            trackBar2.ValueChanged += new EventHandler(trackBar2_ValueChanged);
            textBox2.TextChanged += new EventHandler(textBox2_TextChanged);
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


        [DefaultValue(typeof(int), "0"), Category("Extended attributes")]
        [Description("Channel Light")]
        public virtual int Light
        {
            get
            {
                return trackBar2.Value;
            }
            set
            {
                if (value > 255 || value < 0)
                {
                    this.trackBar2.Value = 0;
                }
                else
                {
                    trackBar2.Value = value;
                    textBox2.Text = value.ToString();
                }
            }
        }


        [Description("Channel Light Change event"), Category("Extended Event")]
        public event EventHandler LightValueChanged;

        private void trackBar2_ValueChanged(object sender, EventArgs e)
        {
            textBox2.Text = trackBar2.Value.ToString();
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

            trackBar2.Value = Convert.ToInt32(textBox2.Text);
        }

        void RGBW_SizeChanged(object sender, EventArgs e)
        {
            Pcolor.Width = this.Width / 8;
            Pcolor.Height = this.Height;

            trackBar2.Width = this.Width / 8 * 5;
            trackBar2.Height = this.Height;

            textBox2.Width = this.Width / 8;
            textBox2.Height = this.Height;

            btnMore.Width = this.Width / 8;
            btnMore.Height = this.Height;

            Pcolor.Location = new System.Drawing.Point(1, 0);
            trackBar2.Location = new Point(Pcolor.Width + Pcolor.Location.X - 1,0);
            textBox2.Location = new Point(trackBar2.Width + trackBar2.Location.X - 1, 0);
            btnMore.Location = new System.Drawing.Point(textBox2.Width + textBox2.Location.X - 1, 0);
        }

        [DefaultValue(typeof(int), "0"), Category("Extended attributes")]
        [Description("Relay ID")]
        public virtual Color _myColor
        {
            get
            {
                Color oTmp = Color.Black;
                oTmp = Pcolor.BackColor;

                return oTmp;
            }
            set
            {
                Pcolor.BackColor = value;
            }
        }


        private void btnMore_Click(object sender, EventArgs e)
        {
            ColorDialog MyDialog = new ColorDialog();
            MyDialog.AllowFullOpen = true;
            MyDialog.ShowHelp = true;
            MyDialog.Color = myColor;
            MyDialog.SolidColorOnly = true;


            if (MyDialog.ShowDialog() == DialogResult.OK)
            {
                myColor = MyDialog.Color;
            }
            Pcolor.BackColor = myColor;
            //this.OnTextChanged(e);
        }

        private void Pcolor_Paint(object sender, PaintEventArgs e)
        {

        }
    }

