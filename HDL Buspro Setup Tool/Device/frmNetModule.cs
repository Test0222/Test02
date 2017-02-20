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
    public partial class frmNetModule : Form
    {
        private Byte SubNetID;
        private Byte DeviceID;
        private String myDeviceName;
        private int mywdDeviceType = -1;

        private NetworkInformation tmpNetworkInformation = null;

        public frmNetModule()
        {
            InitializeComponent();
        }

        public frmNetModule(String DeviceName, int DeviceType)  // 0  温度传感器   1 干接点
        {
            InitializeComponent();
            myDeviceName = DeviceName;
            mywdDeviceType = DeviceType;

            string strMainRemark = DeviceName.Split('\\')[0].Trim();

            //保存basic informations
            SubNetID = byte.Parse(strMainRemark.Split('-')[0].ToString());
            DeviceID = byte.Parse(strMainRemark.Split('-')[1].ToString());
        }

        private void frmTSensor_Shown(object sender, EventArgs e)
        {
            if (CsConst.MyEditMode == 0) //离线模式
            {
            }
            else if (CsConst.MyEditMode == 1) // 只有工程师模式
            {
                DisplayDeviceInformationToListview();
            }
        }

        void DisplayDeviceInformationToListview()
        {
            try
            {
                networkPublic.SubNetID = SubNetID;
                networkPublic.DeviceID = DeviceID;
                networkPublic.DeviceType = mywdDeviceType;
                networkPublic.SetCtrlsVisbleWithDifferentDeviceType();
                networkPublic.btnRead_Click(null, null);

                ListViewItem oLv = new ListViewItem();
                oLv.Text = (lvSensors.Items.Count + 1).ToString();
                oLv.SubItems.Add(myDeviceName);
                lvSensors.Items.Add(oLv);
            }
            catch { }
        }

        private void lvSensors_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void lvSensors_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
          if (lvSensors.Items.Count == 0) return;
            if (lvSensors.SelectedItems.Count == 0) return;
            e.Item.Checked = e.IsSelected;
            for (int i = 0; i < lvSensors.Items.Count; i++)
            {
                lvSensors.Items[i].Checked = (lvSensors.SelectedItems.Contains(lvSensors.Items[i]));
            }
        }

        private void lvSensors_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvSensors.Items == null || lvSensors.Items.Count == 0) return;
            if (lvSensors.SelectedItems == null || lvSensors.SelectedItems.Count == 0) return;

            try
            {
 
            }
            catch
            { }
        }
    }
}
