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
    public partial class frmCamera : Form
    {
        private object Myobj; // 父类窗体
        private int MyLimit = -1;
        private int SensorType = -1; // 0 : camera
        public frmCamera()
        {
            InitializeComponent();
        }

        public frmCamera(object obj, int intLimit, byte bytType)  // 0  温度传感器   1 干接点
        {
            InitializeComponent();
            Myobj = obj;
            MyLimit = intLimit;
            this.SensorType = bytType;

            if (bytType == 0)
            {
                this.Text = "Avaible Cameras on site";
            }
            else if (bytType == 1)
            {
                this.Text = "Dry Contacts on system";
            }
        }

        private void frmTSensor_Shown(object sender, EventArgs e)
        {
            if (CsConst.MyEditMode == 0) //离线模式
            {
                //#region
                //if (CsConst.mintFHID == -1 || CsConst.mintCurRmID == -1) return;
                //if (CsConst.myWorking == null) return;
                //if (CsConst.myWorking.PrjInfors[CsConst.mintFHID] == null) return;
                //if (CsConst.myWorking.PrjInfors[CsConst.mintFHID].PrjAreas[CsConst.mintCurRmID].DevicesList == null) return;
                //if (CsConst.myWorking.PrjInfors[CsConst.mintFHID].PrjAreas[CsConst.mintCurRmID].DevicesList.Count == 0) return;

                //foreach (DevInfosList oTmp in CsConst.myWorking.PrjInfors[CsConst.mintFHID].PrjAreas[CsConst.mintCurRmID].DevicesList)
                //{
                //    int wdDeviceType = oTmp.DeviceType;
                //    int intI = -1;
                //    if (CsConst.mintDLPDeviceType.Contains(wdDeviceType) || CsConst.mint8in1DeviceType.Contains(wdDeviceType) || CsConst.min12in1DeviceType.Contains(wdDeviceType))
                //    {
                //        intI = 1;
                //    }
                //    else if (CsConst.mintTsensorDeviceType.Contains(wdDeviceType))
                //    {
                //        intI = 4;
                //    }

                //    if (intI != -1)
                //    {
                //        for (int i = 1; i <= intI; i++)
                //        {
                //            ListViewItem tmp = new ListViewItem();
                //            tmp.SubItems.Add(oTmp.DevName);
                //            tmp.SubItems.Add(i.ToString());
                //            lvSensors.Items.Add(tmp);
                //        }
                //    }
                //}
                //#endregion
            }
            else if (CsConst.MyEditMode == 1) // 只有工程师模式
            {
                #region
                if (SensorType == 0) //在线模式
                {
                    if (CsConst.myCameras == null || CsConst.myCameras.Count ==0) return;

                    foreach (Camera oTmp in CsConst.myCameras)
                    {
                        if (!(oTmp is Nvr))
                        {
                            ListViewItem tmp = new ListViewItem();
                            tmp.Checked = false;
                            tmp.Text = (lvSensors.Items.Count + 1).ToString();
                            tmp.SubItems.Add(oTmp.Devname);
                            tmp.SubItems.Add(oTmp.networkInformation.ipAddress.ToString());
                            tmp.SubItems.Add("80");

                            lvSensors.Items.Add(tmp);
                        }

                    }
                }
                #endregion
            }
        }

        private void lvSensors_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            if (lvSensors.Items == null || lvSensors.Items.Count == 0) return;
            int intLimit = 0;
            foreach (ListViewItem oTmp in lvSensors.Items)
            {
                if (oTmp.Checked) intLimit++;
            }
            
            if (intLimit > MyLimit)
            {
                 MessageBox.Show("Could not add more than " + MyLimit.ToString() + "Temperature sensors!" );
            }
            e.Item.Checked = false;
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lvSensors.Items.Count == 0) return;
            if (lvSensors.SelectedItems.Count == 0) return;

            //CsConst.MyTmpSensors = new List<DeviceInfo>();
            //for (int i = 0; i < lvSensors.Items.Count; i++)
            //{
            //    if (lvSensors.Items[i].Checked)
            //    {
            //        DeviceInfo oTmp = new DeviceInfo(lvSensors.Items[i].SubItems[1].Text);

            //        oTmp.Other = Convert.ToByte(lvSensors.Items[i].SubItems[2].Text);
            //        CsConst.MyTmpSensors.Add(oTmp);
            //    }
            //}
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

        private void frmTSensor_Load(object sender, EventArgs e)
        {

        }
    }
}
