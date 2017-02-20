using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;


    public partial class ConstantLux : UserControl
    {
        private bool blnIsFirstTime = true;
        public ConstantLux()
        {
            InitializeComponent();
        }

        public ConstantLux(byte bytEnable,int intLux,byte bytKp, byte bytKi,byte bytCycle,byte bytLow)
        {
            InitializeComponent();

            this.chbEnable.Checked = (bytEnable == 1);
            this.NumLux.Value = intLux;
            this.NumKp.Text = (bytKp / 100).ToString("0.00");
            this.NumKi.Text = (bytKi / 100).ToString("0.00");
            this.cboCycle.Text = bytCycle.ToString();
            this.sbLow.Value = bytLow;

            SetVisibleAccordingly();
        }

        [DefaultValue(typeof(int), "0"), Category("Extended attributes")]
        [Description("Enable Constant Function")]
        public virtual byte bytEnable
        {
            get
            {
                 if (chbEnable.Checked) return 1;
                 else return 0;
            }
            set
            {
                chbEnable.Checked = (value == 1);
            }
        }


        [DefaultValue(typeof(int), "0"), Category("Extended attributes")]
        [Description("intLux")]
        public virtual int intLux
        {
            get
            {
                return (int)NumLux.Value;
            }
            set
            {
                if (value > 5000 || value < 0)
                {
                    this.NumLux.Value = 0;
                }
                else
                {
                    NumLux.Value = value;
                }
            }
        }


        [DefaultValue(typeof(int), "0"), Category("Extended attributes")]
        [Description("bytKp")]
        public virtual byte bytKp
        {
            get
            {
                if (NumKp.SelectedIndex == -1) return 0;
                else return (byte)NumKp.SelectedIndex;
            }
            set
            {
                if (value > NumKp.Items.Count - 1) value = 0;
                NumKp.SelectedIndex = value;
            }
        }

        [DefaultValue(typeof(int), "0"), Category("Extended attributes")]
        [Description("bytKi")]
        public virtual byte bytKi
        {
            get
            {
                if (NumKi.SelectedIndex == -1) return 0;
                else return (byte)NumKi.SelectedIndex;
            }
            set
            {
                if (value > NumKi.Items.Count - 1) value = 0;
                NumKi.SelectedIndex = value;
            }
        }

        [DefaultValue(typeof(int), "0"), Category("Extended attributes")]
        [Description("bytCycle")]
        public virtual byte bytCycle
        {
            get
            {
                if (cboCycle.SelectedIndex == -1) return 0;
                else return (byte) cboCycle.SelectedIndex;
            }
            set
            {
                if (value > cboCycle.Items.Count - 1) value = 0;
                cboCycle.SelectedIndex = value;
            }
        }

        [DefaultValue(typeof(int), "0"), Category("Extended attributes")]
        [Description("bytLow")]
        public virtual byte bytLow
        {
            get
            {
                return (byte)sbLow.Value;
            }
            set
            {
                if (value > 100 || value < 0) value = 0;
                sbLow.Value = value;
                sbLow_Scroll(sbLow, null);
            }
        }


        void SetVisibleAccordingly()
        {
            NumLux.Enabled = chbEnable.Checked;
            NumKp.Enabled = chbEnable.Checked;
            NumKi.Enabled = chbEnable.Checked;
            sbLow.Enabled = chbEnable.Checked;
            cboCycle.Enabled = chbEnable.Checked;
        }


        [DefaultValue(typeof(byte), "0"), Category("Extended attributes")]
        [Description("Enable Constant Function")]

        private void chbEnable_CheckedChanged(object sender, EventArgs e)
        {
            SetVisibleAccordingly();
            if (blnIsFirstTime == false)
            {
                if (this.chbEnable.Checked) bytEnable = 1;
                else bytEnable = 0;
            }
            else blnIsFirstTime = false;
        }

        private void sbLow_Scroll(object sender, ScrollEventArgs e)
        {
            lbLowValue.Text = sbLow.Value.ToString();
        }

        private void ConstantLux_Load(object sender, EventArgs e)
        {
            for (byte bytI = 1; bytI <= 50; bytI++) this.cboCycle.Items.Add((float)bytI / 10);
            for (byte bytI = 1; bytI <= 100; bytI++) this.NumKp.Items.Add((float)bytI / 100);
            for (byte bytI = 1; bytI <= 100; bytI++) this.NumKi.Items.Add((float)bytI / 100);
        }

        private void NumLux_ValueChanged(object sender, EventArgs e)
        {
            //if (blnIsFirstTime == false)
            //{
            //    intLux = (int)this.NumLux.Value;
            //}
        }

        private void cboCycle_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (blnIsFirstTime == false)
            //{
            //    bytCycle = (byte)(this.cboCycle.SelectedIndex + 1);
            //}
        }

        private void NumKp_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (blnIsFirstTime == false)
            //{
            //    bytKp = (byte)(this.NumKp.SelectedIndex + 1);
            //}
        }

        private void NumKi_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (blnIsFirstTime == false)
            //{
            //    bytKi = (byte)(this.NumKi.SelectedIndex + 1);
            //}
        }

        private void sbLow_ValueChanged(object sender, EventArgs e)
        {

        }
    }

