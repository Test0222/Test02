using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HDL_Buspro_Setup_Tool
{
    public partial class NewIRKeyID : UserControl
    {
        public delegate void TextBoxChangedHandle(object sender, TextChangeEventArgs e);
        public event TextBoxChangedHandle UserControlValueChanged;
        private string ID = "1";
        public NewIRKeyID(string id)
        {
            InitializeComponent();
            this.ID = id;
            if (CsConst.iLanguageId == 6)
            {
                for (int i = 1; i <= 6; i++)
                {
                    ToolStripMenuItem temp1 = toolStrip1.Items.Find("tsm" + i.ToString(), true)[0] as ToolStripMenuItem;
                    temp1.Text = CsConst.NewIRLibraryDeviceType[i - 1].ToString();
                }
                for (int i = 1; i <= 22; i++)
                {
                    ToolStripMenuItem temp1 = toolStrip1.Items.Find("tsm1K" + i.ToString(), true)[0] as ToolStripMenuItem;
                    temp1.Text = CsConst.mstrINIDefault.IniReadValue("NewIRDevice0", i.ToString("D5"), "");
                    ToolStripMenuItem temp2 = toolStrip1.Items.Find("tsm2K" + i.ToString(), true)[0] as ToolStripMenuItem;
                    temp2.Text = CsConst.mstrINIDefault.IniReadValue("NewIRDevice1", i.ToString("D5"), "");
                }
                for (int i = 1; i <= 23; i++)
                {
                    ToolStripMenuItem temp1 = toolStrip1.Items.Find("tsm3K" + i.ToString(), true)[0] as ToolStripMenuItem;
                    temp1.Text = CsConst.mstrINIDefault.IniReadValue("NewIRDevice2", i.ToString("D5"), "");
                }

                for (int i = 1; i <= 19; i++)
                {
                    ToolStripMenuItem temp1 = toolStrip1.Items.Find("tsm4K" + i.ToString(), true)[0] as ToolStripMenuItem;
                    temp1.Text = CsConst.mstrINIDefault.IniReadValue("NewIRDevice3", i.ToString("D5"), "");
                }

                for (int i = 1; i <= 25; i++)
                {
                    ToolStripMenuItem temp1 = toolStrip1.Items.Find("tsm5K" + i.ToString(), true)[0] as ToolStripMenuItem;
                    temp1.Text = CsConst.mstrINIDefault.IniReadValue("NewIRDevice4", i.ToString("D5"), "");
                }

                for (int i = 1; i <= 7; i++)
                {
                    ToolStripMenuItem temp1 = toolStrip1.Items.Find("tsm6K" + i.ToString(), true)[0] as ToolStripMenuItem;
                    temp1.Text = CsConst.mstrINIDefault.IniReadValue("NewIRDevice5", i.ToString("D5"), "");
                }
            }
        }

        public new string Text
        {
            get
            {
                return ID;
            }
            set
            {
                ID = value;
                SetControlValues();
            }
        }

        private void SetControlValues()
        {
            tsbID.Text = ID.ToString();
        }

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

        private void tsm1K1_Click(object sender, EventArgs e)
        {
            tsbID.Text=Convert.ToString((sender as ToolStripMenuItem).Tag);
            this.ID = Convert.ToString(tsbID.Text);
            if (UserControlValueChanged != null)
                UserControlValueChanged(this, new TextChangeEventArgs(this.ID));
        }
    }
}
