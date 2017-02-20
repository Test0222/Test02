using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HDL_Buspro_Setup_Tool
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            IniFile iniFile = new IniFile(Application.StartupPath + @"\ini\Default.ini");
            String sIsAutoUpgradeSoftware = iniFile.IniReadValue("AutoUpgrade", "IsAuto", "");

            //if (sIsAutoUpgradeSoftware == "true")
            //{
            //    ServerMain Tmp = new ServerMain();
            //    Tmp.Show();

            //    Tmp.FormClosing += new FormClosingEventHandler(UpdateDisplayInformationAccordingly);
            //}
            Application.Run(new frmMain());
        }


       static void UpdateDisplayInformationAccordingly(object sender, FormClosingEventArgs e)
        {
            try
            {
                IniFile iniFile = new IniFile(Application.StartupPath + @"\ini\Default.ini");

                String sCurrentSoftwareVersion = iniFile.IniReadValue("AutoUpgrade", "Software", "");

                if (String.Compare(sCurrentSoftwareVersion, CsConst.softwareverson) > 0)
                {
                    if (MessageBox.Show("There is a newer version avaible on network, are you going to install it now?", "", MessageBoxButtons.OKCancel,
                        MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.OK)
                    {

                    }
                }
            }
            catch
            { }
        }
    }
}
