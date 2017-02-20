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
    public partial class frmTSensor : Form
    {
        private object Myobj; // 父类窗体
        private int MyLimit = -1;
        private Byte deviceType;
        private Byte senSorType;

        private String myDeviceName;


        public frmTSensor()
        {
            InitializeComponent();
        }

        public frmTSensor(object obj, int intLimit, Byte bDeviceType, Byte bSensorType, String TmpDeviceName)  //  //0 : 温度; 1 干结点； 2：HVAC 3: 按键 干结点 ; 255 ： 全部
        {
            InitializeComponent();
            Myobj = obj;
            MyLimit = intLimit;
            this.myDeviceName = TmpDeviceName;
            this.deviceType = bDeviceType;
            this.senSorType = bSensorType;

            if (senSorType == CsConst.TemperatureGroup)
            {
                this.Text = "Temperature sensors on system";
            }
            else if (senSorType == 1)
            {
                this.Text = "Dry Contacts on system";
            }
        }

        void InitialFormCtrlsTextOrItems()
        {
            HDLSysPF.LoadDeviceListsWithDifferentSensorType(cboDevice, deviceType);
        }

        private void frmTSensor_Shown(object sender, EventArgs e)
        {
            if (cboDevice.Items == null || cboDevice.Items.Count == 0) return;
            if (cboDevice.SelectedIndex == -1) cboDevice.SelectedIndex = cboDevice.Items.IndexOf(myDeviceName);
            DisplayAllTSensorInformationToListview();
        }

        void DisplayAllTSensorInformationToListview()
        {
            if (senSorType == CsConst.TemperatureGroup)
            {
                #region
                foreach (DevOnLine oTmp in CsConst.myOnlines)
                {
                    if (oTmp.DevName == cboDevice.Text) continue;
                    int wdDeviceType = oTmp.DeviceType;
                    int intI = -1;
                    if (CsConst.mintDLPDeviceType.Contains(wdDeviceType) || Eightin1DeviceTypeList.HDL8in1DeviceType.Contains(wdDeviceType) 
                     || Twelvein1DeviceTypeList.HDL12in1DeviceType.Contains(wdDeviceType)) 
                    {
                        intI = 1;
                    }
                    else if (T4SensorDeviceTypeList.HDLTsensorDeviceType.Contains(wdDeviceType))
                    {
                        intI = 4;
                    }

                    if (intI != -1)
                    {
                        for (int i = 1; i <= intI; i++)
                        {
                            ListViewItem tmp = new ListViewItem();
                            tmp.SubItems.Add(oTmp.DevName);
                            tmp.SubItems.Add(i.ToString());
                            lvSensors.Items.Add(tmp);
                        }
                    }
                }
                #endregion
            }
            else if (senSorType == CsConst.HvacGroup)
            {
                #region
                foreach (DevOnLine oTmp in CsConst.myOnlines)
                {
                    if (oTmp.DevName == cboDevice.Text) continue;
                    int wdDeviceType = oTmp.DeviceType;
                    int intI = -1;

                    if (HVACModuleDeviceTypeList.HDLHVACModuleDeviceTypeLists.Contains(wdDeviceType))
                    {
                        intI = 1;
                    }

                    if (intI != -1)
                    {
                        for (int i = 1; i <= intI; i++)
                        {
                            ListViewItem tmp = new ListViewItem();
                            tmp.SubItems.Add(oTmp.DevName);
                            tmp.SubItems.Add(i.ToString());
                            lvSensors.Items.Add(tmp);
                        }
                    }
                }
                #endregion
            }
            lbHint.Text = this.Text + " : " + lvSensors.Items.Count.ToString(); 
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
            InitialFormCtrlsTextOrItems();
        }

        private void cboDevice_SelectedIndexChanged(object sender, EventArgs e)
        {
            CsConst.MyTmpSensors = new List<DeviceInfo>();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (cboDevice.Text == null || cboDevice.Text == "") return;
                GetSensorsFromList();

                SaveToDeviceAccordinglyClassType();
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }
            catch { }
        }

        void GetSensorsFromList()
        {
            if (lvSensors.Items.Count == 0) return;

            CsConst.MyTmpSensors = new List<DeviceInfo>();
            for (int i = 0; i < lvSensors.Items.Count; i++)
            {
                if (lvSensors.Items[i].Checked)
                {
                    DeviceInfo oTmp = new DeviceInfo(lvSensors.Items[i].SubItems[1].Text);

                    oTmp.Other = Convert.ToByte(lvSensors.Items[i].SubItems[2].Text);
                    CsConst.MyTmpSensors.Add(oTmp);
                }
            }
        }

        void SaveToDeviceAccordinglyClassType()
        {
            if (Myobj == null) return;
            if (CsConst.MyTmpSensors == null || CsConst.MyTmpSensors.Count == 0) return;

            if (Myobj is DLP)
            {
                DLP TmpDLP = (DLP)Myobj;
                #region
                int i=0;
                foreach (DeviceInfo oTmp in CsConst.MyTmpSensors)
                {
                    TmpDLP.DLPAC.TempSensors[i * 4 + 0] = 1;
                    TmpDLP.DLPAC.TempSensors[i * 4 + 1] = oTmp.SubnetID;
                    TmpDLP.DLPAC.TempSensors[i * 4 + 2] = oTmp.DeviceID;
                    TmpDLP.DLPAC.TempSensors[i * 4 + 3] = oTmp.Other;
                    i++;
                }
                String TmpDevName = TmpDLP.DeviceName.Split('\\')[0].Trim();

                byte SubNetID = byte.Parse(TmpDevName.Split('-')[0].ToString());
                byte DeviceID = byte.Parse(TmpDevName.Split('-')[1].ToString());

                TmpDLP.DLPAC.SaveTemperatureSensor(SubNetID, DeviceID, 172);
                #endregion
            }
        }

        private void BtnSaveExit_Click(object sender, EventArgs e)
        {
            btnSave_Click(btnSave, null);
        }
    }
}
