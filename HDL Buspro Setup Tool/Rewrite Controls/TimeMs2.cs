using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;

namespace HDL_Buspro_Setup_Tool
{
    public class TimeMs2 : TextBox
    {
        private TextBox sencond = new TextBox();
        private TextBox Msecond = new TextBox();
        private TextBox Msecond2 = new TextBox();
        private Label label1 = new Label();

        public TimeMs2()
        {
            this.Controls.Add(sencond);
            this.Controls.Add(Msecond);
            this.Controls.Add(Msecond2);
            this.Controls.Add(label1);

            label1.Text = ".";
            label1.ForeColor = Color.Red;
            this.SizeChanged += new EventHandler(TimeText_SizeChanged);
            sencond.TextChanged += new EventHandler(sencond_TextChanged);
            Msecond.TextChanged += new EventHandler(Msecond_TextChanged);
            Msecond2.TextChanged += new EventHandler(Msecond2_TextChanged);
            sencond.KeyPress += sencond_KeyPress;
            Msecond.KeyPress += sencond_KeyPress;
            Msecond2.KeyPress += sencond_KeyPress;
            TimeText_SizeChanged(null, null);
        }

        void sencond_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar) || e.KeyChar == 8)
                e.Handled = false;
            else
                e.Handled = true;
        }

        void sencond_TextChanged(object sender, EventArgs e)
        {
            sencond.Text = HDLPF.IsNumStringMode(sencond.Text, 0, 2);
            if (sencond.Text == "2")
            {
                Msecond.Text = HDLPF.IsNumStringMode(Msecond2.Text, 0, 5);
            }
            sencond.SelectionStart = sencond.Text.Length;
            if (sencond.Text == "0")
            {
                sencond.Focus();
                sencond.SelectAll();
            }
            this.OnTextChanged(e);
        }

        void Msecond_TextChanged(object sender, EventArgs e)
        {
            if (sencond.Text == "2")
            {
                Msecond.Text = HDLPF.IsNumStringMode(Msecond.Text, 0, 5);
                if (Msecond.Text == "5")
                {
                    Msecond2.Text = HDLPF.IsNumStringMode(Msecond2.Text, 0, 5);
                }
            }
            else
                Msecond.Text = HDLPF.IsNumStringMode(Msecond.Text, 0, 9);
            Msecond.SelectionStart = Msecond.Text.Length;
            if (Msecond.Text == "0")
            {
                Msecond.Focus();
                Msecond.SelectAll();
            }
            this.OnTextChanged(e);
        }
        void Msecond2_TextChanged(object sender, EventArgs e)
        {
            if (sencond.Text == "2" && Msecond.Text == "5")
                Msecond2.Text = HDLPF.IsNumStringMode(Msecond2.Text, 0, 5);
            else
                Msecond2.Text = HDLPF.IsNumStringMode(Msecond2.Text, 0, 9);
            Msecond2.SelectionStart = Msecond2.Text.Length;
            if (Msecond2.Text == "0")
            {
                Msecond2.Focus();
                Msecond2.SelectAll();
            }
            this.OnTextChanged(e);
        }

        void TimeText_SizeChanged(object sender, EventArgs e)
        {
            label1.Width = 10;
            sencond.Width = (this.Width - 20) / 3;
            Msecond.Width = (this.Width - 20) / 3;
            Msecond2.Width = (this.Width - 20) / 3;

            label1.Location = new System.Drawing.Point(sencond.Width + sencond.Location.X, 0);
            label1.Height = this.Height - 2;

            Msecond.Location = new System.Drawing.Point(label1.Location.X + label1.Width, 0);
            Msecond.Height = this.Height - 2;

            Msecond2.Location = new System.Drawing.Point(Msecond.Location.X + Msecond.Width, 0);
            Msecond2.Height = this.Height - 2;
        }

        public override string Text
        {
            get
            {
                string strTmp = null;
                int s = 0, ms = 0, ms2 = 0;
                int.TryParse(sencond.Text.Trim(), out s);
                int.TryParse(Msecond.Text.Trim(), out ms);
                int.TryParse(Msecond2.Text.Trim(), out ms2);
                strTmp = (s * 100 + ms * 10 + ms2).ToString();
                return strTmp;
            }
            set
            {
                int time = 0;
                int intTmp = 0;
                int.TryParse(value.Trim(), out time);

                intTmp = time / 100;
                sencond.Text = intTmp.ToString();

                Msecond.Text = (time / 10 % 10).ToString();
                Msecond2.Text = (time % 10).ToString();

                base.Text =  sencond.Text + label1.Text + Msecond.Text + Msecond2.Text;
            }
        }
    }
}
  