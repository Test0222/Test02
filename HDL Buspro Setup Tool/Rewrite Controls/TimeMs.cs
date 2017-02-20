using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;

namespace HDL_Buspro_Setup_Tool
{
    public class TimeMs : TextBox
    {
        private TextBox minute = new TextBox();
        private TextBox sencond = new TextBox();
        private TextBox Msecond = new TextBox();
        private Label label2 = new Label();
        private Label label1 = new Label();

        public TimeMs()
        {
            this.Controls.Add(minute);
            this.Controls.Add(sencond);
            this.Controls.Add(Msecond);
            this.Controls.Add(label2);
            this.Controls.Add(label1);

            label2.Text = ":";
            label1.Text = ".";
            label2.ForeColor = Color.Red;
            label1.ForeColor = Color.Red;
            this.SizeChanged += new EventHandler(TimeText_SizeChanged);
            minute.TextChanged += new EventHandler(minute_TextChanged);
            sencond.TextChanged += new EventHandler(sencond_TextChanged);
            Msecond.TextChanged += new EventHandler(Msecond_TextChanged);
            minute.Leave += minute_Leave;
            sencond.Leave += sencond_Leave;
            Msecond.Leave += Msecond_Leave;
            minute.KeyPress += minute_KeyPress;
            sencond.KeyPress += minute_KeyPress;
            Msecond.KeyPress += minute_KeyPress;
            TimeText_SizeChanged(null, null);
        }

        void Msecond_Leave(object sender, EventArgs e)
        {
            Msecond.Text = HDLPF.IsNumStringMode(Msecond.Text, 0, 9);
        }

        void sencond_Leave(object sender, EventArgs e)
        {
            sencond.Text = HDLPF.IsNumStringMode(sencond.Text, 0, 59);
        }

        void minute_Leave(object sender, EventArgs e)
        {
            minute.Text = HDLPF.IsNumStringMode(minute.Text, 0, 60);
        }

        void minute_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar) || e.KeyChar == 8)
                e.Handled = false;
            else
                e.Handled = true;
        }

        void sencond_TextChanged(object sender, EventArgs e)
        {
            this.OnTextChanged(e);
        }

        void minute_TextChanged(object sender, EventArgs e)
        {
            if (minute.Text == "60")
            {
                sencond.Text = "0";
                Msecond.Text = "0";
            }
            this.OnTextChanged(e);
        }

        void Msecond_TextChanged(object sender, EventArgs e)
        {
            this.OnTextChanged(e);
        }

        void TimeText_SizeChanged(object sender, EventArgs e)
        {
            label2.Width = 10;
            label1.Width = 10;
            minute.Width = (this.Width - 22) / 3;
            sencond.Width = (this.Width - 22) / 3;
            Msecond.Width = (this.Width - 22) / 3;

            label2.Location = new System.Drawing.Point(minute.Width + minute.Location.X, 0);
            label2.Height = this.Height - 2;

            sencond.Location = new System.Drawing.Point(label2.Width + label2.Location.X, 0);
            sencond.Height = this.Height - 2;

            label1.Location = new System.Drawing.Point(sencond.Width + sencond.Location.X , 0);
            label1.Height = this.Height - 2;

            Msecond.Location = new System.Drawing.Point(label1.Location.X + label1.Width , 0);
            Msecond.Height = this.Height - 2;
        }

        public override string Text
        {
            get
            {
                string strTmp = null;
                int m = 0, s = 0, ms = 0;
                int.TryParse(minute.Text.Trim(), out m);
                int.TryParse(sencond.Text.Trim(), out s);
                int.TryParse(Msecond.Text.Trim(), out ms);
                strTmp = ((m * 60 + s) * 10 + ms).ToString();
                return strTmp;
            }
            set
            {
                int time = 0;
                int intTmp = 0;
                int.TryParse(value.Trim(), out time);

                intTmp = time % 36000;
                minute.Text = (time / 600).ToString();

                intTmp = time / 10 % 60;
                sencond.Text = intTmp.ToString();

                Msecond.Text = (time % 10).ToString();

                base.Text = minute.Text + label2.Text + sencond.Text + label1.Text + Msecond.Text;
            }
        }
    }
}

