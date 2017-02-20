using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;


    public partial class RGB : UserControl
    {
        private Color myColor = Color.Black;
 
        public RGB(Color oColor)
        {
            InitializeComponent();

            this._myColor = oColor;
            this.SizeChanged += new EventHandler(RGB_SizeChanged);
            RGB_SizeChanged(null, null);
        }

        void RGB_SizeChanged(object sender, EventArgs e)
        {
            Pcolor.Width = this.Width / 3 * 2;
            Pcolor.Height = this.Height;

            btnMore.Width = this.Width / 3;
            btnMore.Height = this.Height;

            Pcolor.Location = new System.Drawing.Point(1, 0);
            btnMore.Location = new System.Drawing.Point(Pcolor.Width + Pcolor.Location.X - 1, 0);
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

