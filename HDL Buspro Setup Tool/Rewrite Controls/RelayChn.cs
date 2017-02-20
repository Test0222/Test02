using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;


    public partial class RelayChn : UserControl
    {
        public RelayChn(int ChnID)
        {
            InitializeComponent();
            this.RelayID = ChnID;

            this.SizeChanged += new EventHandler(TimeText_SizeChanged);
            
            Ron.CheckedChanged += new EventHandler(Ron_CheckedChanged);
            Roff.CheckedChanged += new EventHandler(Ron_CheckedChanged);
            TimeText_SizeChanged(null, null);
        }

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

        void TimeText_SizeChanged(object sender, EventArgs e)
        {
            Ron.Width = this.Width / 2;
            Ron.Height = this.Height;
            Roff.Width = this.Width / 2;
            Roff.Height = this.Height;
            Roff.Location = new System.Drawing.Point(1, 0);
            Ron.Location = new System.Drawing.Point(Roff.Width + Roff.Location.X - 1, 0);
        }

        private int RelayID;
        [DefaultValue(typeof(int), "0"), Category("Extended attributes")]
        [Description("Relay ID")]
        public virtual int _RelayID
        {
            get
            {
                int intTmp = 0;
                if (Ron.Checked == true)  intTmp = 1;
                return intTmp;
            }
            set
            {
                if (value == 0)
                {
                    this.Roff.Checked  = true;
                    this.Ron.Checked = false;
                }
                else
                {
                    this.Roff.Checked = false;
                    this.Ron.Checked = true;
                }
            }
        }

        private void Ron_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Tag == null) return;

            byte bytTag = Convert.ToByte(((RadioButton)sender).Tag.ToString());

            if (TextChanged != null)
                TextChanged(sender, new EventArgs());
            //this.OnTextChanged(e);
        }

        private void Roff_CheckedChanged(object sender, EventArgs e)
        {

        }
    }

