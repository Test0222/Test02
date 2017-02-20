using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;


    public partial class CurtainChn : UserControl
    {
        public CurtainChn(int ChnID)
        {
            InitializeComponent();
            this.CurtainID = ChnID;

            this.SizeChanged += new EventHandler(TimeText_SizeChanged);
            
            Rstop.CheckedChanged += new EventHandler(Ron_CheckedChanged);
            Rclose.CheckedChanged += new EventHandler(Ron_CheckedChanged);
            Ropen.CheckedChanged += new EventHandler(Ron_CheckedChanged);
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
            Rstop.Width = this.Width / 3;
            Rstop.Height = this.Height;
            Rclose.Width = this.Width / 3;
            Rclose.Height = this.Height;
            Ropen.Width = this.Width / 3;
            Ropen.Height = this.Height;

            Rclose.Location = new System.Drawing.Point(1, 0);
            Rstop.Location = new System.Drawing.Point(Rclose.Width + Rclose.Location.X - 1, 0);
            Ropen.Location = new System.Drawing.Point(Rstop.Width + Rstop.Location.X - 1, 0);
        }

        private int CurtainID;
        [DefaultValue(typeof(int), "0"), Category("Extended attributes")]
        [Description("Relay ID")]
        public virtual int _CurtainID
        {
            get
            {
                int intTmp = 0;
                if (Rstop.Checked == true) intTmp = 0;
                else if (Ropen.Checked == true) intTmp = 1;
                else if (Rclose.Checked == true) intTmp = 2;
                return intTmp;
            }
            set
            {
                if (CurtainID == 0)
                {
                    this.Rclose.Checked  = false;
                    this.Ropen.Checked = false;
                    this.Rstop.Checked = true;
                }
                else if (CurtainID == 1)
                {
                    this.Rclose.Checked = false;
                    this.Rstop.Checked = false;
                    this.Ropen.Checked = true;
                }
                else
                {
                    this.Rclose.Checked = true;
                    this.Rstop.Checked = false;
                    this.Ropen.Checked = false;
                }
            }
        }

        private void Ron_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Tag == null) return;

            byte bytTag = Convert.ToByte(((RadioButton)sender).Tag.ToString());

            if (TextChanged != null)
                TextChanged(sender, new EventArgs());
        }
    }

