using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;


namespace HDL_Buspro_Setup_Tool
{
    public class TimeText : TextBox
    {
        public TextBox minute = new TextBox();
        public TextBox sencond = new TextBox();
        private Label label2 = new Label();

        public TimeText()
        {
            this.Controls.Add(minute);
            this.Controls.Add(sencond);
            this.Controls.Add(label2);

            label2.Text = ":";
            this.SizeChanged += new EventHandler(TimeText_SizeChanged);
            minute.TextChanged += new EventHandler(minute_TextChanged);
            sencond.TextChanged += new EventHandler(sencond_TextChanged);
            TimeText_SizeChanged(null, null);
        }

        public TimeText(string strSplit)
        {
            this.Controls.Add(minute);
            this.Controls.Add(sencond);
            this.Controls.Add(label2);

            label2.Text = strSplit;
            this.SizeChanged += new EventHandler(TimeText_SizeChanged);
            minute.Leave += minute_Leave;
            sencond.Leave += sencond_Leave;
            minute.TextChanged += minute_TextChanged;
            sencond.TextChanged += sencond_TextChanged;
            minute.KeyPress += minute_KeyPress;
            sencond.KeyPress += minute_KeyPress;
            TimeText_SizeChanged(null, null);
        }

        void sencond_Leave(object sender, EventArgs e)
        {
            int i = 0;
            if (label2.Text.Equals(":"))  //如果表示是分 : 秒
            {
                if (int.TryParse(sencond.Text, out i))
                {
                    if (i < 0 || i > 60)
                    {
                        sencond.Text = "0";
                    }
                }
                else
                {
                    sencond.Text = "0";
                }
            }
            else if (label2.Text.Equals("."))  //如果表示是秒 : 毫秒
            {
                if (int.TryParse(sencond.Text, out i))
                {
                    if (i < 0 || i > 9)
                    {
                        sencond.Text = "0";
                    }
                }
                else
                {
                    sencond.Text = "0";
                }
            }
            sencond.SelectionStart = sencond.Text.Length;
        }

        void minute_Leave(object sender, EventArgs e)
        {
            int i = 0;
            if (label2.Text.Equals(":"))  //如果表示是分 : 秒
            {
                if (int.TryParse(minute.Text, out i))
                {
                    if (i < 0 || i > 60)
                    {
                        minute.Text = "";
                    }
                }
                else
                {
                    minute.Text = "0";
                }
            }
            else if (label2.Text.Equals("."))  //如果表示是秒 : 毫秒
            {
                minute.Text = HDLPF.IsNumStringMode(minute.Text, 0, 25);
            }
            minute.SelectionStart = minute.Text.Length;
        }

        void minute_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar) || e.KeyChar == 8)
                e.Handled = false;
            else
                e.Handled = true;
        }


        [DefaultValue(typeof(string), ":"), Category("Extended attributes")]
        [Description("Spit Text")]

        public virtual string SplitText
        {
            set
            {
                label2.Text = value;
            }
        }

        void sencond_TextChanged(object sender, EventArgs e)
        {
            
            this.OnTextChanged(e);
        }

        void minute_TextChanged(object sender, EventArgs e)
        {
            this.OnTextChanged(e);
        }


        void TimeText_SizeChanged(object sender, EventArgs e)
        {
            label2.Width = 16;
            minute.Width = (this.Width - 16) / 2;
            sencond.Width = (this.Width - 16) / 2;

            label2.Location = new System.Drawing.Point(minute.Width + minute.Location.X - 2, 0);
            label2.Height = this.Height;
            sencond.Location = new System.Drawing.Point(label2.Width + label2.Location.X - 1, 0);

        }

        public override string Text
        {
            get
            {
                string strTmp = null;
                int m = 0, s = 0;
                string strM = "";
                string strS = "";
                strM = minute.Text;
                strS = sencond.Text;
                if (minute.Text == "" || !GlobalClass.IsNumeric(minute.Text)) strM = "0";
                if (sencond.Text == "" || !GlobalClass.IsNumeric(sencond.Text)) strS = "0";
                int.TryParse(strM.Trim(), out m);
                int.TryParse(strS.Trim(), out s);
                if (label2.Text.Equals(":"))
                {
                    strTmp = (m * 60 + s).ToString();
                }
                else if (label2.Text.Equals("."))
                {
                    strTmp = (m * 10 + s).ToString();
                }
                return strTmp;
            }
            set
            {
                int time = 0;
                int.TryParse(value.Trim(), out time);

                if (label2.Text.Equals(":"))
                {
                    time = time % 3600;
                    minute.Text = (time / 60).ToString(); ;
                    time = time % 60;
                    sencond.Text = time.ToString();
                    base.Text = minute.Text + label2.Text + sencond.Text;
                }
                else if (label2.Text.Equals("."))
                {
                    time = time % 256;
                    minute.Text = (time / 10).ToString(); ;
                    time = time % 10;
                    sencond.Text = time.ToString();
                    base.Text = minute.Text + label2.Text + sencond.Text;
                }
            }
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // TimeText
            // 
            this.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ResumeLayout(false);

        }
    }
}

