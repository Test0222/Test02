using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HDL_Buspro_Setup_Tool
{
    public partial class FrmCombinationWays : Form
    {
        private byte SubnetID = 0;
        private byte DeviceID = 0;
        private int MyintDeviceType;
        private string strRemark = "";
        private int PageID = 0;
        private DLP oPanel;
        public FrmCombinationWays()
        {
            InitializeComponent();
        }

        public FrmCombinationWays(string strname, int devicetype, int selectedPageid,DLP panel)
        {
            InitializeComponent();
            string strDevName = strname.Split('\\')[0].ToString();
            strRemark = strname.Split('\\')[1].ToString();
            SubnetID = Convert.ToByte(strDevName.Split('-')[0]);
            DeviceID = Convert.ToByte(strDevName.Split('-')[1]);
            this.MyintDeviceType = devicetype;
            this.PageID = selectedPageid;
            this.oPanel = panel;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            byte[] arayTmp = new byte[2];
            arayTmp[0] = Convert.ToByte(cbPage.SelectedIndex);
            for (int i = 1; i <= 8; i++)
            {
                System.Windows.Forms.Panel temp = this.Controls.Find("pnl" + i.ToString(), true)[0] as System.Windows.Forms.Panel;
                if (temp.BackColor == Color.Red)
                {
                    arayTmp[1] = Convert.ToByte(i - 1);
                    break;
                }
            }
            if (CsConst.mySends.AddBufToSndList(arayTmp, 0xE112, SubnetID, DeviceID, false, false, true, CsConst.minAllWirelessDeviceType.Contains(MyintDeviceType)) == true)
            {
                
            }
            Cursor.Current = Cursors.Default;
        }

        private void FrmCombinationWays_Load(object sender, EventArgs e)
        {
            lbSubValue.Text = SubnetID.ToString();
            lbDevValue.Text = DeviceID.ToString();
            lbRemarkValue.Text = strRemark;
            cbPage.SelectedIndex = PageID;
            if (cbPage.SelectedIndex < 0) cbPage.SelectedIndex = 0;
            cbPage_SelectedIndexChanged(null, null);
        }

        private void pic1_Click(object sender, EventArgs e)
        {
            for (int i = 1; i <= 8; i++)
            {
                System.Windows.Forms.Panel temp = this.Controls.Find("pnl" + i.ToString(), true)[0] as System.Windows.Forms.Panel;
                temp.BackColor = temp.Parent.BackColor;
            }
            int Tag = Convert.ToInt32((sender as System.Windows.Forms.PictureBox).Tag);
            System.Windows.Forms.Panel tmp = this.Controls.Find("pnl" + (Tag + 1).ToString(), true)[0] as System.Windows.Forms.Panel;
            tmp.BackColor = Color.Red;
        }

        private void cbPage_SelectedIndexChanged(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            for (int i = 1; i <= 8; i++)
            {
                System.Windows.Forms.Panel temp = this.Controls.Find("pnl" + i.ToString(), true)[0] as System.Windows.Forms.Panel;
                temp.BackColor = temp.Parent.BackColor;
            }
            byte[] arayTmp = new byte[1];
            arayTmp[0] = Convert.ToByte(cbPage.SelectedIndex);
            if (CsConst.mySends.AddBufToSndList(arayTmp, 0xE110, SubnetID, DeviceID, false, false, false, CsConst.minAllWirelessDeviceType.Contains(MyintDeviceType)) == true)
            {
                int Way = CsConst.myRevBuf[27];
                System.Windows.Forms.Panel temp = this.Controls.Find("pnl" + (Way + 1).ToString(), true)[0] as System.Windows.Forms.Panel;
                temp.BackColor = Color.Red;
                
            }
            Cursor.Current = Cursors.Default;
        }
    }
}
