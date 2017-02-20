using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace HDL_Buspro_Setup_Tool
{
    public partial class FrmAbout : Form
    {
        public FrmAbout()
        {
            InitializeComponent();
        }

        private void FrmAbout_Load(object sender, EventArgs e)
        {
            lbVersionValue.Text = CsConst.softwareverson;
            if (CsConst.iLanguageId == 1) this.Text = "关于";
            txtList.Text = "";
            string strFile = "";
            if (CsConst.iLanguageId == 0)
                strFile = Application.StartupPath + @"\ini\Readme-Eng.txt";
            else if (CsConst.iLanguageId == 1)
                strFile = Application.StartupPath + @"\ini\Readme-Chn.txt";
            
            try
            {
                StreamReader sr = new StreamReader(strFile, Encoding.Default);
                String line;
                while ((line = sr.ReadLine()) != null)
                {
                    txtList.AppendText(line.ToString()+"\r\n");
                }

            }
            catch
            {

            }
        }

        private void link1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(link1.Text);
            }
            catch 
            {
                
            }

        }

        private void link2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(link2.Text);
            }
            catch
            {

            }
        }
    }
}
