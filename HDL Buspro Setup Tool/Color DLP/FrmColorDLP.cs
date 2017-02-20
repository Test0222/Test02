using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace HDL_Buspro_Setup_Tool
{
    public partial class FrmColorDLP : Form
    {
        [DllImport("user32.dll")]
        public static extern Boolean FlashWindow(IntPtr hwnd, Boolean bInvert);
        private ColorDLP myDLP = null;
        private string myDevName = null;
        private int mintIDIndex = -1;
        private int MyintDeviceType = -1;
        private int MyActivePage = 0; //按页面上传下载
        private byte SubNetID;
        private byte DevID;
        private bool isReadingData = false;
        private ComboBox cbKeyType = new ComboBox();
        private ComboBox cbKeyDim = new ComboBox();
        private ComboBox cbKeySaveDim = new ComboBox();
        private ComboBox cbKeyMutex = new ComboBox();
        private ComboBox cbScenesType = new ComboBox();
        private ComboBox cbScenesDim = new ComboBox();
        private ComboBox cbScenesSaveDim = new ComboBox();
        private ComboBox cbScenesMutex = new ComboBox();

        public FrmColorDLP()
        {
            InitializeComponent();
        }

        public FrmColorDLP(ColorDLP myColorDLP, string strName, int intDeviceType, int intDIndex)
        {
            InitializeComponent();
            this.myDevName = strName;
            this.mintIDIndex = intDIndex;
            string strDevName = strName.Split('\\')[0].ToString();
            this.MyintDeviceType = intDeviceType;
            this.myDLP = myColorDLP;

            HDLSysPF.DisplayDeviceNameModeDescription(strName, MyintDeviceType, cboDevice, tbModel, tbDescription);
            SubNetID = Convert.ToByte(strDevName.Split('-')[0]);
            DevID = Convert.ToByte(strDevName.Split('-')[1]);
            tsl3.Text = strName;
            dgvLightKeys.Controls.Add(cbKeyType);
            dgvLightKeys.Controls.Add(cbKeyDim);
            dgvLightKeys.Controls.Add(cbKeySaveDim);
            dgvLightKeys.Controls.Add(cbKeyMutex);
            cbKeyType.DropDownStyle = ComboBoxStyle.DropDownList;
            cbKeyDim.DropDownStyle = ComboBoxStyle.DropDownList;
            cbKeySaveDim.DropDownStyle = ComboBoxStyle.DropDownList;
            cbKeyMutex.DropDownStyle = ComboBoxStyle.DropDownList;
            cbKeyType.SelectedIndexChanged += cbKeyType_SelectedIndexChanged;
            cbKeyDim.SelectedIndexChanged += cbKeyDim_SelectedIndexChanged;
            cbKeySaveDim.SelectedIndexChanged += cbKeySaveDim_SelectedIndexChanged;
            cbKeyMutex.SelectedIndexChanged += cbKeyMutex_SelectedIndexChanged;

            dgvScenesKey.Controls.Add(cbScenesType);
            dgvScenesKey.Controls.Add(cbScenesDim);
            dgvScenesKey.Controls.Add(cbScenesSaveDim);
            dgvScenesKey.Controls.Add(cbScenesMutex);
            cbScenesType.DropDownStyle = ComboBoxStyle.DropDownList;
            cbScenesDim.DropDownStyle = ComboBoxStyle.DropDownList;
            cbScenesSaveDim.DropDownStyle = ComboBoxStyle.DropDownList;
            cbScenesMutex.DropDownStyle = ComboBoxStyle.DropDownList;
            cbScenesType.SelectedIndexChanged += cbScenesType_SelectedIndexChanged;
            cbScenesDim.SelectedIndexChanged += cbScenesDim_SelectedIndexChanged;
            cbScenesSaveDim.SelectedIndexChanged += cbScenesSaveDim_SelectedIndexChanged;
            cbScenesMutex.SelectedIndexChanged += cbScenesMutex_SelectedIndexChanged;
            setAllVisible(false);
        }

        private void setAllVisible(bool TF)
        {
            cbKeyType.Visible = TF;
            cbKeyDim.Visible = TF;
            cbKeySaveDim.Visible = TF;
            cbKeyMutex.Visible = TF;
            cbScenesType.Visible = TF;
            cbScenesDim.Visible = TF;
            cbScenesSaveDim.Visible = TF;
            cbScenesMutex.Visible = TF;
        }

        void cbScenesMutex_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dgvScenesKey.CurrentRow.Index < 0) return;
            if (dgvScenesKey.RowCount <= 0) return;
            if (myDLP == null) return;
            if (myDLP.myScenes == null) return;
            int index = dgvScenesKey.CurrentRow.Index;
            if (cbScenesMutex.Visible)
            {
                dgvScenesKey[5, index].Value = cbScenesMutex.Text;
                myDLP.myScenes[cbScenes.SelectedIndex * 12 + index].bytMutex = Convert.ToByte(cbScenesMutex.SelectedIndex);
            }
        }

        void cbScenesSaveDim_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dgvScenesKey.CurrentRow.Index < 0) return;
            if (dgvScenesKey.RowCount <= 0) return;
            if (myDLP == null) return;
            if (myDLP.myScenes == null) return;
            int index = dgvScenesKey.CurrentRow.Index;
            if (cbScenesSaveDim.Visible)
            {
                dgvScenesKey[4, index].Value = cbScenesSaveDim.Text;
                if (dgvScenesKey[4, index].Value.ToString() == CsConst.mstrINIDefault.IniReadValue("Public", "99880", ""))
                    myDLP.myScenes[cbScenes.SelectedIndex * 12 + index].SaveDimmer = 1;
                else myDLP.myScenes[cbScenes.SelectedIndex * 12 + index].SaveDimmer = 0;
            }
        }

        void cbScenesDim_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dgvScenesKey.CurrentRow.Index < 0) return;
            if (dgvScenesKey.RowCount <= 0) return;
            if (myDLP == null) return;
            if (myDLP.myScenes == null) return;
            int index = dgvScenesKey.CurrentRow.Index;
            if (cbScenesDim.Visible)
            {
                dgvScenesKey[3, index].Value = cbScenesDim.Text;
                if (dgvScenesKey[3, index].Value.ToString() == CsConst.mstrINIDefault.IniReadValue("Public", "99878", ""))
                    myDLP.myScenes[cbScenes.SelectedIndex * 12 + index].IsDimmer = 1;
                else myDLP.myScenes[cbScenes.SelectedIndex * 12 + index].IsDimmer = 0;
            }
        }

        void cbScenesType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dgvScenesKey.CurrentRow.Index < 0) return;
            if (dgvScenesKey.RowCount <= 0) return;
            if (myDLP == null) return;
            if (myDLP.myScenes == null) return;
            int index = dgvScenesKey.CurrentRow.Index;
            if (cbScenesType.Visible)
            {
                dgvScenesKey[2, index].Value = cbScenesType.Text;
                if (dgvScenesKey[2, index].Value.ToString() == CsConst.mstrINIDefault.IniReadValue("keyMode", "00001", ""))
                    myDLP.myScenes[cbScenes.SelectedIndex * 12 + index].Mode = 1;
                else if (dgvScenesKey[2, index].Value.ToString() == CsConst.mstrINIDefault.IniReadValue("keyMode", "00002", ""))
                    myDLP.myScenes[cbScenes.SelectedIndex * 12 + index].Mode = 2;
                else if (dgvScenesKey[2, index].Value.ToString() == CsConst.mstrINIDefault.IniReadValue("keyMode", "00003", ""))
                    myDLP.myScenes[cbScenes.SelectedIndex * 12 + index].Mode = 3;
                else if (dgvScenesKey[2, index].Value.ToString() == CsConst.mstrINIDefault.IniReadValue("keyMode", "00004", ""))
                    myDLP.myScenes[cbScenes.SelectedIndex * 12 + index].Mode = 4;
                else if (dgvScenesKey[2, index].Value.ToString() == CsConst.mstrINIDefault.IniReadValue("keyMode", "00005", ""))
                    myDLP.myScenes[cbScenes.SelectedIndex * 12 + index].Mode = 5;
                else if (dgvScenesKey[2, index].Value.ToString() == CsConst.mstrINIDefault.IniReadValue("keyMode", "00007", ""))
                    myDLP.myScenes[cbScenes.SelectedIndex * 12 + index].Mode = 7;
                else myDLP.myScenes[cbScenes.SelectedIndex * 12 + index].Mode = 0;
            }
        }

        void cbKeyMutex_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dgvLightKeys.CurrentRow.Index < 0) return;
            if (dgvLightKeys.RowCount <= 0) return;
            if (myDLP == null) return;
            if (myDLP.MyKeys == null) return;
            int index = dgvLightKeys.CurrentRow.Index;
            if (cbKeyMutex.Visible)
            {
                dgvLightKeys[5, index].Value = cbKeyMutex.Text;
                myDLP.MyKeys[cbLight.SelectedIndex * 11 + index].bytMutex = Convert.ToByte(cbKeyMutex.SelectedIndex);
            }
        }

        void cbKeySaveDim_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dgvLightKeys.CurrentRow.Index < 0) return;
            if (dgvLightKeys.RowCount <= 0) return;
            if (myDLP == null) return;
            if (myDLP.MyKeys == null) return;
            int index = dgvLightKeys.CurrentRow.Index;
            if (cbKeySaveDim.Visible)
            {
                dgvLightKeys[4, index].Value = cbKeySaveDim.Text;
                if (dgvLightKeys[4, index].Value.ToString() == CsConst.mstrINIDefault.IniReadValue("Public", "99880", ""))
                    myDLP.MyKeys[cbLight.SelectedIndex * 11 + index].SaveDimmer = 1;
                else myDLP.MyKeys[cbLight.SelectedIndex * 11 + index].SaveDimmer = 0;
            }
        }

        void cbKeyDim_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dgvLightKeys.CurrentRow.Index < 0) return;
            if (dgvLightKeys.RowCount <= 0) return;
            if (myDLP == null) return;
            if (myDLP.MyKeys == null) return;
            int index = dgvLightKeys.CurrentRow.Index;
            if (cbKeyDim.Visible)
            {
                dgvLightKeys[3, index].Value = cbKeyDim.Text;
                if (dgvLightKeys[3, index].Value.ToString() == CsConst.mstrINIDefault.IniReadValue("Public", "99878", ""))
                    myDLP.MyKeys[cbLight.SelectedIndex * 11 + index].IsDimmer = 1;
                else myDLP.MyKeys[cbLight.SelectedIndex * 11 + index].IsDimmer = 0;
            }
        }

        void cbKeyType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dgvLightKeys.CurrentRow.Index < 0) return;
            if (dgvLightKeys.RowCount <= 0) return;
            if (myDLP == null) return;
            if (myDLP.MyKeys == null) return;
            int index = dgvLightKeys.CurrentRow.Index;
            if (cbKeyType.Visible)
            {
                dgvLightKeys[2, index].Value = cbKeyType.Text;
                if (dgvLightKeys[2, index].Value.ToString() == CsConst.mstrINIDefault.IniReadValue("keyMode", "00001", ""))
                    myDLP.MyKeys[cbLight.SelectedIndex * 11 + index].Mode = 1;
                else if (dgvLightKeys[2, index].Value.ToString() == CsConst.mstrINIDefault.IniReadValue("keyMode", "00002", ""))
                    myDLP.MyKeys[cbLight.SelectedIndex * 11 + index].Mode = 2;
                else if (dgvLightKeys[2, index].Value.ToString() == CsConst.mstrINIDefault.IniReadValue("keyMode", "00003", ""))
                    myDLP.MyKeys[cbLight.SelectedIndex * 11 + index].Mode = 3;
                else if (dgvLightKeys[2, index].Value.ToString() == CsConst.mstrINIDefault.IniReadValue("keyMode", "00004", ""))
                    myDLP.MyKeys[cbLight.SelectedIndex * 11 + index].Mode = 4;
                else if (dgvLightKeys[2, index].Value.ToString() == CsConst.mstrINIDefault.IniReadValue("keyMode", "00005", ""))
                    myDLP.MyKeys[cbLight.SelectedIndex * 11 + index].Mode = 5;
                else if (dgvLightKeys[2, index].Value.ToString() == CsConst.mstrINIDefault.IniReadValue("keyMode", "00007", ""))
                    myDLP.MyKeys[cbLight.SelectedIndex * 11 + index].Mode = 7;
                else myDLP.MyKeys[cbLight.SelectedIndex * 11 + index].Mode = 0;
            }   
        }

        private void FrmColorDLP_Load(object sender, EventArgs e)
        {
            for (int i = 1; i <= 2; i++)
            {
                ComboBox temp = this.Controls.Find("cbPM" + i.ToString(), true)[0] as ComboBox;
                temp.Items.Clear();
                temp.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00038", ""));
                temp.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99891", ""));
                temp.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99892", ""));
            }
            if (MyintDeviceType != 179)
                tabControl.TabPages.Remove(tabSensor);
            addComboboxItem();
            if (MyintDeviceType != 181)
            {
                btnACAdv.Visible = false;
            }
        }

        private void addComboboxItem()
        {
            chbDisplay.Items.Clear();
            chbDisplay.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99874", ""));
            chbDisplay.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99800", ""));

            cbFanSpeed.Items.Clear();
            for (int i = 0; i < 4; i++)
            {
                cbFanSpeed.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "0006" + i.ToString(), ""));
            }
            cbPowerOnState.Items.Clear();
            cbPowerOnState.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00038", ""));
            cbPowerOnState.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99883", ""));
            cbACType.Items.Clear();
            cbACType.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99884", ""));
            cbACType.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99885", ""));
            cbMode.Items.Clear();
            for (int i = 0; i < 5; i++)
            {
                cbMode.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "0005" + i.ToString(), ""));
            }
            cbHeatType.Items.Clear();
            cbHeatType.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99886", ""));
            cbHeatType.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99887", ""));
            cbHeatType.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99888", ""));
            cbControl.Items.Clear();
            cbControl.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99889", ""));
            cbControl.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99890", ""));
            for (int i = 1; i <= 4; i++)
            {
                ComboBox temp = this.Controls.Find("cbHeatSensor" + i.ToString(), true)[0] as ComboBox;
                temp.Items.Clear();
                temp.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00038", ""));
                temp.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99891", ""));
                temp.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99892", ""));
            }
            cbHeatSpeed.Items.Clear();
            for (int i = 0; i <= 5; i++)
            {
                cbHeatSpeed.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "0015" + i.ToString(), ""));
            }
            cbMinPWM.Items.Clear();
            cbMaxPWM.Items.Clear();
            for (int i = 0; i <= 100; i++)
            {
                cbMinPWM.Items.Add(i.ToString() + "%");
                cbMaxPWM.Items.Add(i.ToString() + "%");
            }
            cbCurrentMode.Items.Clear();
            for (int i = 0; i <= 4; i++)
            {
                cbCurrentMode.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "0007" + i.ToString(), ""));
            }

            cbKeyDim.Items.Clear();
            cbKeyDim.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99877", ""));
            cbKeyDim.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99878", ""));
            cbKeySaveDim.Items.Clear();
            cbKeySaveDim.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99879", ""));
            cbKeySaveDim.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99880", ""));
            cbKeyMutex.Items.Clear();
            cbKeyMutex.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99881", ""));
            for (int i = 1; i <= 20; i++)
                cbKeyMutex.Items.Add(i.ToString());

            cbScenesDim.Items.Clear();
            cbScenesDim.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99877", ""));
            cbScenesDim.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99878", ""));
            cbScenesSaveDim.Items.Clear();
            cbScenesSaveDim.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99879", ""));
            cbScenesSaveDim.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99880", ""));
            cbScenesMutex.Items.Clear();
            cbScenesMutex.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99881", ""));
            for (int i = 1; i <= 20; i++)
                cbScenesMutex.Items.Add(i.ToString());
            cbScenesType.Items.Clear();
            cbScenesType.Items.Add(CsConst.mstrINIDefault.IniReadValue("keyMode", "00000", ""));
            cbScenesType.Items.Add(CsConst.mstrINIDefault.IniReadValue("keyMode", "00004", ""));
            cbScenesType.Items.Add(CsConst.mstrINIDefault.IniReadValue("keyMode", "00005", ""));
            cbScenesType.Items.Add(CsConst.mstrINIDefault.IniReadValue("keyMode", "00007", ""));
            chbDisplay.Visible = (MyintDeviceType == 177 || MyintDeviceType == 179 || MyintDeviceType == 181);
            lbDisplay.Visible = (MyintDeviceType == 177 || MyintDeviceType == 179 || MyintDeviceType == 181);
            
            cbPage.Items.Clear();
            cbPage.Items.Add(CsConst.mstrINIDefault.IniReadValue("ColorDLP", "00000", ""));//主界面
            cbPage.Items.Add("--------------------------------------------------------");
            cbPage.Items.Add(CsConst.mstrINIDefault.IniReadValue("ColorDLP", "00001", ""));//待机界面
            cbPage.Items.Add("--------------------------------------------------------");
            for (int i = 2; i <= 8; i++)
                cbPage.Items.Add(CsConst.mstrINIDefault.IniReadValue("ColorDLP", "0000" + i.ToString(), ""));//灯光
            cbPage.Items.Add("--------------------------------------------------------");
            for (int i = 9; i <= 10; i++)
                cbPage.Items.Add(CsConst.mstrINIDefault.IniReadValue("ColorDLP", "000" + GlobalClass.AddLeftZero(i.ToString(), 2), ""));//窗帘
            cbPage.Items.Add("--------------------------------------------------------");
            for (int i = 11; i <= 17; i++)
                cbPage.Items.Add(CsConst.mstrINIDefault.IniReadValue("ColorDLP", "000" + i.ToString(), ""));
            cbPage.Items.Add("--------------------------------------------------------");
            for (int i = 18; i <= 27; i++)
                cbPage.Items.Add(CsConst.mstrINIDefault.IniReadValue("ColorDLP", "000" + i.ToString(), ""));
            cbPage.Items.Add("--------------------------------------------------------");
            for (int i = 28; i <= 37; i++)
                cbPage.Items.Add(CsConst.mstrINIDefault.IniReadValue("ColorDLP", "000" + i.ToString(), ""));
            cbPage.Items.Add("--------------------------------------------------------");
            for (int i = 38; i <= 47; i++)
                cbPage.Items.Add(CsConst.mstrINIDefault.IniReadValue("ColorDLP", "000" + i.ToString(), ""));
            cbPage.Items.Add("--------------------------------------------------------");
            cbPage.Items.Add(CsConst.mstrINIDefault.IniReadValue("ColorDLP", "00048", ""));
            cbPage.SelectedIndex = 0;
        }

        private void tbDown_Click(object sender, EventArgs e)
        {
            try
            {
                byte bytTag = Convert.ToByte(((ToolStripButton)sender).Tag.ToString());
                bool blnShowMsg = (CsConst.MyEditMode != 1);
                if (HDLPF.UploadOrReadGoOn(this.Text, bytTag, blnShowMsg))
                {
                    Cursor.Current = Cursors.WaitCursor;

                    CsConst.MyUPload2DownLists = new List<byte[]>();

                    string strName = myDevName.Split('\\')[0].ToString();
                    byte bytSubID = byte.Parse(strName.Split('-')[0]);
                    byte bytDevID = byte.Parse(strName.Split('-')[1]);
                    byte[] ArayRelay = new byte[] { SubNetID, DevID, (byte)(MyintDeviceType / 256), (byte)(MyintDeviceType % 256), 
                        (byte)MyActivePage,(byte)(mintIDIndex / 256), (byte)(mintIDIndex % 256) };
                    CsConst.MyUPload2DownLists.Add(ArayRelay);
                    CsConst.MyUpload2Down = Convert.ToByte(((ToolStripButton)sender).Tag.ToString());
                    FrmDownloadShow Frm = new FrmDownloadShow();
                    if (CsConst.MyUpload2Down ==0) Frm.FormClosing += new FormClosingEventHandler(UpdateDisplayInformationAccordingly);
                    Frm.ShowDialog();
                }
            }
            catch
            {
            }
        }

        void UpdateDisplayInformationAccordingly(object sender, FormClosingEventArgs e)
        {
            switch (tabControl.SelectedTab.Name)
            {
                case "tabBasic": showBasic(); break;
                case "tabLight": showKeyInfo(); break;
                case "tabAC": showACInfo(); break;
                case "tabCurtain": showCurtainInfo(); break;
                case "tabHeat": showHeatingInfo(); break;
                case "tabMusic": showMusicInfo(); break;
                case "tabScene": showScenesInfo(); break;
                case "tabSensor": showPMInfo(); break;
            }
        }

        private void showPMInfo()
        {
            try
            {
                isReadingData = true;
                if (cbPM.SelectedIndex < 0) cbPM.SelectedIndex = 0;
                int AreaID = cbPM.SelectedIndex;
                if (myDLP == null) return;
                if (myDLP.mySensor == null) return;
                ColorDLP.Sensor temp = myDLP.mySensor[AreaID];
                for (int i = 1; i <= 2; i++)
                {
                    ComboBox tmp1 = this.Controls.Find("cbPM" + i.ToString(), true)[0] as ComboBox;
                    if (temp.arayPM[(i - 1) * 5 + 1] < tmp1.Items.Count)
                        tmp1.SelectedIndex = temp.arayPM[(i - 1) * 5 + 1];
                    else
                        tmp1.SelectedIndex = 0;
                }

                for (int i = 1; i <= 2; i++)
                {
                    NumericUpDown tmp1 = this.Controls.Find("numSubPM" + i.ToString(), true)[0] as NumericUpDown;
                    NumericUpDown tmp2 = this.Controls.Find("numDevPM" + i.ToString(), true)[0] as NumericUpDown;
                    NumericUpDown tmp3 = this.Controls.Find("numChnPM" + i.ToString(), true)[0] as NumericUpDown;
                    ComboBox tmp4 = this.Controls.Find("cbTypePM" + i.ToString(), true)[0] as ComboBox;
                    tmp1.Value = Convert.ToDecimal(temp.arayPM[(i - 1) * 5 + 2]);
                    tmp2.Value = Convert.ToDecimal(temp.arayPM[(i - 1) * 5 + 3]);
                    tmp3.Value = Convert.ToDecimal(temp.arayPM[(i - 1) * 5 + 4]);
                    int num = temp.arayPM[(i - 1) * 5 + 5];
                    if (i == 1) tmp4.SelectedIndex = 0;
                    else if (i == 2) tmp4.SelectedIndex = 1;
                    /*string str = GlobalClass.AddLeftZero(Convert.ToString(num, 2), 8);
                    str = str.Substring(2, 6);
                    num = Convert.ToInt32(str, 2);
                    if (num == 9) tmp4.SelectedIndex = 0;
                    else if (num == 19) tmp4.SelectedIndex = 1;
                    else tmp4.SelectedIndex = 0;*/
                }
                
            }
            catch
            {
            }
            isReadingData = false;
        }

        private void showBasic()
        {
            try
            {
                isReadingData = true;
                if (myDLP.BasicInfo[0] == 100) rb1.Checked = true;
                else if (10 <= myDLP.BasicInfo[0] && myDLP.BasicInfo[0] <= 99)
                {
                    rb2.Checked = true;
                    txt1.Text = myDLP.BasicInfo[0].ToString();
                }
                if (myDLP.BasicInfo[1] == 1) chb2.Checked = true;
                else chb2.Checked = false;
                if (0 <= myDLP.BasicInfo[2] && myDLP.BasicInfo[2] <= 100)
                    sb1.Value = myDLP.BasicInfo[2];
                if (myDLP.BasicInfo[3] == 1) chb1.Checked = true;
                else chb1.Checked = false;
                sb2.Value = myDLP.BasicInfo[4];
                sb3.Value = myDLP.BasicInfo[6];
                num1.Value = myDLP.BasicInfo[5] / 10;
                num2.Value = myDLP.BasicInfo[5] % 10;
                cbTemp.SelectedIndex = myDLP.TemperatureType;
                cbACTempType_SelectedIndexChanged(null, null);
                cbHeatTempType_SelectedIndexChanged(null, null);
                if (myDLP.BasicInfo[7] == 0) rbPage1.Checked = true;
                else
                {
                    rbPage2.Checked = true;
                    switch (myDLP.BasicInfo[7])
                    {
                        case 1: cbPage.SelectedIndex = 0; break;//主界面
                        case 2: cbPage.SelectedIndex = 2; break;//待机界面
                        case 10: cbPage.SelectedIndex = 4; break;//灯光主界面
                        case 11:                                 
                        case 12:
                        case 13:
                        case 14:
                        case 15:
                        case 16:
                            cbPage.SelectedIndex = myDLP.BasicInfo[7] - 6; break;//灯光分页面
                        case 21: cbPage.SelectedIndex = 12; break;//场景页一
                        case 22: cbPage.SelectedIndex = 13; break;//场景页二
                        case 30: cbPage.SelectedIndex = 15; break;//窗帘主界面
                        case 31: 
                        case 32:
                        case 33:
                        case 34:
                        case 35:
                        case 36:
                            cbPage.SelectedIndex = myDLP.BasicInfo[7] - 15;break;//窗帘分页面
                        case 40: cbPage.SelectedIndex = 23; break;//空调主界面
                        case 41:
                        case 42:
                        case 43:
                        case 44:
                        case 45:
                        case 46:
                        case 47:
                        case 48:
                        case 49:
                            cbPage.SelectedIndex = myDLP.BasicInfo[7] - 17; break;//空调分页面
                        case 50: cbPage.SelectedIndex = 34; break;//地热主界面
                        case 51:
                        case 52:
                        case 53:
                        case 54:
                        case 55:
                        case 56:
                        case 57:
                        case 58:
                        case 59:
                            cbPage.SelectedIndex = myDLP.BasicInfo[7] - 16; break;//地热分页面
                        case 60: cbPage.SelectedIndex = 45; break;//音乐主界面
                        case 61:
                        case 62:
                        case 63:
                        case 64:
                        case 65:
                        case 66:
                        case 67:
                        case 68:
                        case 69:
                            cbPage.SelectedIndex = myDLP.BasicInfo[7] - 15; break;//音乐分页面
                        case 70: cbPage.SelectedIndex = 56; break;//传感器主界面
                    }
                }
                if (myDLP.isHasSensorSensitivity)
                {
                    lbSensitivity.Visible = true;
                    cbSensitivity.Visible = true;
                    if (myDLP.BasicInfo[9] <= 100)
                        cbSensitivity.Value = myDLP.BasicInfo[9];
                }
                else
                {
                    lbSensitivity.Visible = false;
                    cbSensitivity.Visible = false;
                }

                if (myDLP.BasicInfo[13] == 0) rbTime1.Checked = true;
                else if (myDLP.BasicInfo[13] == 1) rbTime2.Checked = true;
                if (myDLP.BasicInfo[14] == 0) rbTimeDate1.Checked = true;
                else if (myDLP.BasicInfo[14] == 1) rbTimeDate2.Checked = true;
                else if (myDLP.BasicInfo[14] == 2) rbTimeDate3.Checked = true;
                else if (myDLP.BasicInfo[14] == 3) rbTimeDate4.Checked = true;
                if (myDLP.BasicInfo[15] == 1) chbDisplay.SetItemChecked(0, true);
                else chbDisplay.SetItemChecked(0, false);
                if (myDLP.BasicInfo[16] == 1) chbDisplay.SetItemChecked(1, true);
                else chbDisplay.SetItemChecked(1, false);
            }
            catch
            {
            }
            isReadingData = false;
            rbPage1_CheckedChanged(null, null);
        }

        private void showKeyInfo()
        {
            try
            {
                isReadingData = true;
                setAllVisible(false);
                if (myDLP == null) return;
                if (myDLP.MyKeys == null) return;
                isReadingData = true;
                if (cbLight.Items.Count != 0 && cbLight.SelectedIndex < 0) cbLight.SelectedIndex = 0;
                dgvLightKeys.Rows.Clear();
                for (int i = 0; i < myDLP.MyKeys.Count; i++)
                {
                    HDLButton temp = myDLP.MyKeys[i];
                    if (temp.PageID == cbLight.SelectedIndex)
                    {
                        string strMode = "";
                        string strDimming = "";
                        string strSaveDimmingValue = "";
                        string strMutex = "";
                        if (1 <= temp.Mode && temp.Mode <= 7) strMode = CsConst.mstrINIDefault.IniReadValue("keyMode", "0000" + temp.Mode.ToString(), "");
                        else strMode = CsConst.mstrINIDefault.IniReadValue("keyMode", "00000", "");
                        if (temp.IsDimmer ==1) strDimming = CsConst.mstrINIDefault.IniReadValue("Public", "99878", "");
                        else strDimming = CsConst.mstrINIDefault.IniReadValue("Public", "99877", "");
                        if (temp.SaveDimmer ==1) strSaveDimmingValue = CsConst.mstrINIDefault.IniReadValue("Public", "99880", "");
                        else strSaveDimmingValue = CsConst.mstrINIDefault.IniReadValue("Public", "99879", "");
                        if (10 <= temp.ID && temp.ID <= 12)
                        {
                            if (1 <= temp.bytMutex && temp.bytMutex <= 20) strMutex = temp.bytMutex.ToString();
                            else strMutex = CsConst.mstrINIDefault.IniReadValue("Public", "99881", "");
                        }
                        else
                        {
                            strMutex = "N/A";
                        }
                        object[] obj = new object[] { temp.ID.ToString(), temp.Remark.ToString(), strMode, strDimming, strSaveDimmingValue, strMutex };
                        dgvLightKeys.Rows.Add(obj);
                    }
                }
            }
            catch
            {
            }
            isReadingData = false;
        }

        private void showACInfo()
        {
            try
            {
                isReadingData = true;
                if (myDLP == null) return;
                if (myDLP.MyAC == null) return;
                if (cbAC.Items.Count != 0 && cbAC.SelectedIndex < 0) cbAC.SelectedIndex = 0;
                for (int i = 0; i < myDLP.MyAC.Count; i++)
                {
                    EnviroAc temp = myDLP.MyAC[i];
                    if (temp.ID == cbAC.SelectedIndex)
                    {
                        if (temp.Enable == 1) chbEnable.Checked = true;
                        else chbEnable.Checked = false;
                        chbFunction.Checked = Convert.ToBoolean(temp.OperationEnable);
                        if (temp.PowerOnRestoreState <= 1) cbPowerOnState.SelectedIndex = temp.PowerOnRestoreState;
                        NumHVACSub.Value = Convert.ToDecimal(temp.DesSubnetID);
                        NumHVACDev.Value = Convert.ToDecimal(temp.DesDevID);
                        if (temp.ControlWays <= 1) cbACType.SelectedIndex = temp.ControlWays;
                        NumAC.Value = Convert.ToDecimal(temp.ACNum);
                        if (chbEnable.Checked) ReadACWorkingState();
                        break;
                    }
                }
                if (myDLP.BroadCastAry[0] == 1) chbBroadCast.Checked = true;
                txtBroadSub.Text = myDLP.BroadCastAry[1].ToString();
                txtBroadDev.Text = myDLP.BroadCastAry[2].ToString();
                sbBroadcast.Value = myDLP.BroadCastAry[3];
            }
            catch
            {
            }
            isReadingData = false;
        }

        private void showCurtainInfo()
        {
            try
            {
                if (myDLP == null) return;
                if (myDLP.MyCurtain == null) return;
                isReadingData = true;
                PanelCurtain.Controls.Clear();
                if (cbCurtain.Items.Count != 0 && cbCurtain.SelectedIndex < 0) cbCurtain.SelectedIndex = 0;
                int intTmp = 1;
                for (int i = 0; i < myDLP.MyCurtain.Count; i++)
                {
                    if (myDLP.MyCurtain[i].PageID == cbCurtain.SelectedIndex)
                    {
                        CurtainForColorDLP temp = new CurtainForColorDLP(myDLP, myDLP.MyCurtain[i].KeyNo, myDevName, myDLP.MyCurtain[i].PageID,mintIDIndex);
                        if (intTmp <= 2) temp.Top = 5;
                        else temp.Top = 10 + temp.Height;
                        if (intTmp == 1 || intTmp == 3) temp.Left = 20;
                        else temp.Left = 40 + temp.Width;
                        intTmp = intTmp + 1;
                        PanelCurtain.Controls.Add(temp);
                    }
                }
            }
            catch
            {
            }
            isReadingData = false;
        }

        private void showHeatingInfo()
        {
            try
            {
                if (myDLP == null) return;
                if (myDLP.MyHeat == null) return;
                isReadingData = true;
                if (cbHeat.Items.Count != 0 && cbHeat.SelectedIndex < 0) cbHeat.SelectedIndex = 0;
                for (int i = 0; i < myDLP.MyHeat.Count; i++)
                {
                    EnviroFH temp = myDLP.MyHeat[i];
                    if (temp.ID == cbHeat.SelectedIndex)
                    {

                        if (temp.HeatEnable == 1) chbHeating.Checked = true;
                        else chbHeating.Checked = false;
                        if (temp.HeatType <= 2) cbHeatType.SelectedIndex = temp.HeatType;
                        else cbHeatType.SelectedIndex = 0;
                        if (temp.ControlMode <= 1) cbControl.SelectedIndex = temp.ControlMode;
                        else cbControl.SelectedIndex = 0;
                        cbHeatTempType_SelectedIndexChanged(null, null);
                        cbHeatType_SelectedIndexChanged(null, null);
                        cbControl_SelectedIndexChanged(null, null);

                        if (temp.SourceTemp == 0) rbHeat1.Checked = true;
                        else if (temp.SourceTemp == 1) rbHeat2.Checked = true;
                        else if (temp.SourceTemp == 2) rbHeat3.Checked = true;
                        if (temp.OutDoorParam[0] <= 2) cbHeatSensor1.SelectedIndex = temp.OutDoorParam[0];
                        else cbHeatSensor1.SelectedIndex = 0;
                        NumSub1.Value = Convert.ToDecimal(temp.OutDoorParam[1]);
                        NumDev1.Value = Convert.ToDecimal(temp.OutDoorParam[2]);
                        NumChn1.Value = Convert.ToDecimal(temp.OutDoorParam[3]);
                        if (temp.OutDoorParam[4] <= 2) cbHeatSensor2.SelectedIndex = temp.OutDoorParam[4];
                        else cbHeatSensor2.SelectedIndex = 0;
                        NumSub2.Value = Convert.ToDecimal(temp.OutDoorParam[5]);
                        NumDev2.Value = Convert.ToDecimal(temp.OutDoorParam[6]);
                        NumChn2.Value = Convert.ToDecimal(temp.OutDoorParam[7]);
                        if (temp.SourceParam[0] <= 2) cbHeatSensor3.SelectedIndex = temp.SourceParam[0];
                        else cbHeatSensor3.SelectedIndex = 0;
                        NumSub3.Value = Convert.ToDecimal(temp.SourceParam[1]);
                        NumDev3.Value = Convert.ToDecimal(temp.SourceParam[2]);
                        NumChn3.Value = Convert.ToDecimal(temp.SourceParam[3]);
                        if (temp.SourceParam[4] <= 2) cbHeatSensor4.SelectedIndex = temp.SourceParam[4];
                        else cbHeatSensor4.SelectedIndex = 0;
                        NumSub4.Value = Convert.ToDecimal(temp.SourceParam[5]);
                        NumDev4.Value = Convert.ToDecimal(temp.SourceParam[6]);
                        NumChn4.Value = Convert.ToDecimal(temp.SourceParam[7]);
                        if (myDLP.TemperatureType == 0)
                            cbMaxTemp.SelectedIndex = cbMaxTemp.Items.IndexOf(temp.ProtectTemperature.ToString() + "C");
                        else if (myDLP.TemperatureType == 1)
                            cbMaxTemp.SelectedIndex = cbMaxTemp.Items.IndexOf(temp.ProtectTemperature.ToString() + "F");
                        if (cbMaxTemp.SelectedIndex < 0) cbMaxTemp.SelectedIndex = 0;
                        if (temp.PIDEnable == 1) chbHeat2.Checked = true;
                        else chbHeat2.Checked = false;

                        if (temp.Switch == 1) chbHeat1.Checked = true;
                        else chbHeat1.Checked = false;
                        if (temp.SysEnable[0] == 1)
                            chbHeat3.Checked = true;
                        else
                            chbHeat3.Checked = false;

                        if (temp.SysEnable[1] == 1)
                            chbHeat4.Checked = true;
                        else
                            chbHeat4.Checked = false;

                        if (temp.SysEnable[2] == 1)
                            chbHeat5.Checked = true;
                        else
                            chbHeat5.Checked = false;

                        if (temp.SysEnable[3] == 1)
                            chbHeat6.Checked = true;
                        else
                            chbHeat6.Checked = false;
                        if (temp.OutputType <= 1) cbOutput.SelectedIndex = temp.OutputType;
                        else cbOutput.SelectedIndex = 0;
                        if (temp.Cycle < cbCycle.Items.Count) cbCycle.SelectedIndex = temp.Cycle;
                        else cbCycle.SelectedIndex = 0;
                        if (temp.minPWM <= 100) cbMinPWM.SelectedIndex = temp.minPWM;
                        else cbMinPWM.SelectedIndex = 0;
                        if (temp.maxPWM <= 100) cbMaxPWM.SelectedIndex = temp.maxPWM;
                        else cbMaxPWM.SelectedIndex = 0;
                        if (temp.Speed <= 4) cbHeatSpeed.SelectedIndex = temp.Speed;
                        else cbHeatSpeed.SelectedIndex = 0;
                        if (temp.TimeAry[0] <= 23) numStart1.Value = Convert.ToDecimal(temp.TimeAry[0]);
                        if (temp.TimeAry[1] <= 59) numStart2.Value = Convert.ToDecimal(temp.TimeAry[1]);
                        if (temp.TimeAry[2] <= 23) numEnd1.Value = Convert.ToDecimal(temp.TimeAry[2]);
                        if (temp.TimeAry[3] <= 59) numEnd2.Value = Convert.ToDecimal(temp.TimeAry[3]);
                        for (int j = 0; j < 5; j++)
                            chbListHeatMode.SetItemChecked(j, temp.ModeAry[j] == 1);
                        if (1 <= temp.CompenValue && temp.CompenValue <= 5) sbModeChaneover.Value = temp.CompenValue;
                        if (myDLP.TemperatureType == 0)
                            cbHeatMinTemp.SelectedIndex = cbHeatMinTemp.Items.IndexOf(temp.minTemp.ToString() + "C");
                        else if (myDLP.TemperatureType == 1)
                            cbHeatMinTemp.SelectedIndex = cbHeatMinTemp.Items.IndexOf(temp.minTemp.ToString() + "F");
                        if (myDLP.TemperatureType == 0)
                            cbHeatMaxTemp.SelectedIndex = cbHeatMaxTemp.Items.IndexOf(temp.maxTemp.ToString() + "C");
                        else if (myDLP.TemperatureType == 1)
                            cbHeatMaxTemp.SelectedIndex = cbHeatMaxTemp.Items.IndexOf(temp.maxTemp.ToString() + "F");
                        if (cbHeatMinTemp.SelectedIndex < 0) cbHeatMinTemp.SelectedIndex = 0;
                        if (cbHeatMaxTemp.SelectedIndex < 0) cbHeatMaxTemp.SelectedIndex = 0;
                        if (temp.WorkingSwitch == 1) chbHeatSwitch.Checked = true;
                        else chbHeatSwitch.Checked = false;
                        if (myDLP.TemperatureType == 0)
                        {
                            if (HDLSysPF.GetBit(temp.CurrentTemp, 7) == 1)
                                lbHeatCurrentTempValue.Text = "-" + ((temp.CurrentTemp & (byte.MaxValue - (1 << 7)))).ToString() + "C";
                            else
                                lbHeatCurrentTempValue.Text = temp.CurrentTemp.ToString() + "C";
                        }
                        else if (myDLP.TemperatureType == 1)
                        {
                            if (HDLSysPF.GetBit(temp.CurrentTemp, 7) == 1)
                                lbHeatCurrentTempValue.Text = "-" + ((temp.CurrentTemp & (byte.MaxValue - (1 << 7)))).ToString() + "F";
                            else
                                lbHeatCurrentTempValue.Text = temp.CurrentTemp.ToString() + "F";
                        }

                        if (1 <= temp.WorkingTempMode && temp.WorkingTempMode <= 5) cbCurrentMode.SelectedIndex = temp.WorkingTempMode - 1;
                        else cbCurrentMode.SelectedIndex = 0;
                        if (sbHeatTemp1.Minimum <= temp.ModeTemp[0] && temp.ModeTemp[0] <= sbHeatTemp1.Maximum) sbHeatTemp1.Value = Convert.ToInt32(temp.ModeTemp[0]);
                        if (sbHeatTemp2.Minimum <= temp.ModeTemp[1] && temp.ModeTemp[1] <= sbHeatTemp2.Maximum) sbHeatTemp2.Value = Convert.ToInt32(temp.ModeTemp[1]);
                        if (sbHeatTemp3.Minimum <= temp.ModeTemp[2] && temp.ModeTemp[2] <= sbHeatTemp3.Maximum) sbHeatTemp3.Value = Convert.ToInt32(temp.ModeTemp[2]);
                        if (sbHeatTemp4.Minimum <= temp.ModeTemp[3] && temp.ModeTemp[3] <= sbHeatTemp4.Maximum) sbHeatTemp4.Value = Convert.ToInt32(temp.ModeTemp[3]);
                        break;
                    }
                }
            }
            catch
            {
            }
            panel23.Visible = (cbHeat.SelectedIndex == 0);
            isReadingData = false;
        }

        private void showMusicInfo()
        {
            try
            {
                if (myDLP == null) return;
                if (myDLP.MyMusic == null) return;
                isReadingData = true;
                for (int i = 0; i < myDLP.MyMusic.Count; i++)
                {
                    EnviroMusic temp = myDLP.MyMusic[i];
                    if (temp.ID == 0)
                    {
                        if (temp.Enable == 1) rbMusic1.Checked = true;
                        else rbMusic2.Checked = false;
                        if (temp.Type <= 1) cbMusicMode.SelectedIndex = temp.Type;
                        if (1 <= temp.CurrentZoneID && temp.CurrentZoneID <= 24) cbMusicZone.SelectedIndex = temp.CurrentZoneID - 1;
                        txtMusicSub.Text = temp.aryNetDevID[cbMusicZone.SelectedIndex * 2].ToString();
                        txtMusicDev.Text = temp.aryNetDevID[cbMusicZone.SelectedIndex * 2 + 1].ToString();
                        break;
                    }
                }
            }
            catch
            {
            }
            isReadingData = false;
        }

        private void showScenesInfo()
        {
            try
            {
                setAllVisible(false);
                if (myDLP == null) return;
                if (myDLP.myScenes == null) return;
                isReadingData = true;
                if (cbScenes.Items.Count != 0 && cbScenes.SelectedIndex < 0) cbScenes.SelectedIndex = 0;
                dgvScenesKey.Rows.Clear();
                for (int i = 0; i < myDLP.myScenes.Count; i++)
                {
                    HDLButton temp = myDLP.myScenes[i];
                    if (temp.PageID == cbScenes.SelectedIndex)
                    {
                        string strMode = "";
                        string strDimming = "";
                        string strSaveDimmingValue = "";
                        string strMutex = "";
                        if (temp.Mode == 4 || temp.Mode == 5 || temp.Mode == 7) strMode = CsConst.mstrINIDefault.IniReadValue("keyMode", "0000" + temp.Mode.ToString(), "");
                        else strMode = CsConst.mstrINIDefault.IniReadValue("keyMode", "00000", "");
                        if (temp.IsDimmer==1) strDimming = CsConst.mstrINIDefault.IniReadValue("Public", "99878", "");
                        else strDimming = CsConst.mstrINIDefault.IniReadValue("Public", "99877", "");
                        if (temp.SaveDimmer==1) strSaveDimmingValue = CsConst.mstrINIDefault.IniReadValue("Public", "99880", "");
                        else strSaveDimmingValue = CsConst.mstrINIDefault.IniReadValue("Public", "99880", "");
                        if (1 <= temp.bytMutex && temp.bytMutex <= 20) strMutex = temp.bytMutex.ToString();
                        else strMutex = CsConst.mstrINIDefault.IniReadValue("Public", "99881", "");
                        object[] obj = new object[] { temp.ID.ToString(), temp.Remark.ToString(), strMode, strDimming, strSaveDimmingValue, strMutex };
                        dgvScenesKey.Rows.Add(obj);
                    }
                }
            }
            catch
            {
            }
            isReadingData = false;
        }

        private void cbLight_SelectedIndexChanged(object sender, EventArgs e)
        {
            showKeyInfo();
        }

        private void cbAC_SelectedIndexChanged(object sender, EventArgs e)
        {
            showACInfo();
        }

        private void sbCoolTemp_ValueChanged(object sender, EventArgs e)
        {
            if (myDLP.TemperatureType == 0)
                lbCoolTempValue.Text = sbCoolTemp.Value.ToString() + "C";
            else
                lbCoolTempValue.Text = sbCoolTemp.Value.ToString() + "F";
        }

        private void sbHeatTemp_ValueChanged(object sender, EventArgs e)
        {
            if (myDLP.TemperatureType == 0)
                lbHeatTempValue.Text = sbHeatTemp.Value.ToString() + "C";
            else
                lbHeatTempValue.Text = sbHeatTemp.Value.ToString() + "F";
        }

        private void sbAutoTemp_ValueChanged(object sender, EventArgs e)
        {
            if (myDLP.TemperatureType == 0)
                lbAutoTempValue.Text = sbAutoTemp.Value.ToString() + "C";
            else
                lbAutoTempValue.Text = sbAutoTemp.Value.ToString() + "F";
        }

        private void sbDryTemp_ValueChanged(object sender, EventArgs e)
        {
            if (myDLP.TemperatureType == 0)
                lbDryTempValue.Text = sbDryTemp.Value.ToString() + "C";
            else
                lbDryTempValue.Text = sbDryTemp.Value.ToString() + "F";
        }

        private void btnACSetup_Click(object sender, EventArgs e)
        {
            FrmColorDLPACSetup ACSetup = new FrmColorDLPACSetup(myDevName, myDLP, MyintDeviceType, cbAC.SelectedIndex);
            ACSetup.ShowDialog();
            if (ACSetup.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                //UpdateTemperatureToForm();
                //UpdateWindAndModesToForm();
            }
        }

        private void FrmColorDLP_Shown(object sender, EventArgs e)
        {
            if (CsConst.MyEditMode == 0)
            {

            }
            else if (CsConst.MyEditMode == 1) //在线模式
            {
                MyActivePage = 1;
                tbDown_Click(tbDown, null);
            }
        }

        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CsConst.MyEditMode == 1)
            {
                isReadingData = true;
                if (tabControl.SelectedTab.Name == "tabBasic") MyActivePage = 1;
                else if (tabControl.SelectedTab.Name == "tabLight") MyActivePage = 2;
                else if (tabControl.SelectedTab.Name == "tabAC") MyActivePage = 3;
                else if (tabControl.SelectedTab.Name == "tabCurtain") MyActivePage = 4;
                else if (tabControl.SelectedTab.Name == "tabHeat") MyActivePage = 5;
                else if (tabControl.SelectedTab.Name == "tabMusic") MyActivePage = 6;
                else if (tabControl.SelectedTab.Name == "tabScene") MyActivePage = 7;
                else if (tabControl.SelectedTab.Name == "tabSensor") MyActivePage = 8;
                
                MyActivePage = tabControl.SelectedIndex + 1;
                if (myDLP.MyRead2UpFlags[MyActivePage-1] == false)
                {
                    tbDown_Click(tbDown, null);
                }
                else
                {
                    UpdateDisplayInformationAccordingly(null,null);
                }
                isReadingData = false;
            }
            Cursor.Current = Cursors.Default;
        }

        private void chbEnable_CheckedChanged(object sender, EventArgs e)
        {
            grpACBasic.Enabled = chbEnable.Enabled;
            grpsection.Visible = chbEnable.Checked;
            if (isReadingData) return;
            btnSaveAC_Click(null, null);
        }

        private void ReadACWorkingState()
        {
            Cursor.Current = Cursors.WaitCursor;
            byte[] ArayTmp = new byte[1];
            ArayTmp[0] = Convert.ToByte(cbAC.SelectedIndex);
            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x19AE, SubNetID, DevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(MyintDeviceType)) == true)
            {
                string strTemp = "";
                string strMode = "";
                string strSpeed = "";
                string strWind = "";
                if (CsConst.myRevBuf[34] == 0)
                {
                    sbCoolTemp.Minimum = 0;
                    sbHeatTemp.Minimum = 0;
                    sbAutoTemp.Minimum = 0;
                    sbDryTemp.Minimum = 0;
                    sbCoolTemp.Maximum = 30;
                    sbHeatTemp.Maximum = 30;
                    sbAutoTemp.Maximum = 30;
                    sbDryTemp.Maximum = 30;
                    if (HDLSysPF.GetBit(CsConst.myRevBuf[38], 7) == 1)
                        strTemp = "-" + ((CsConst.myRevBuf[38] & (byte.MaxValue - (1 << 7)))).ToString() + "C";
                    else
                        strTemp = CsConst.myRevBuf[38].ToString() + "C";
                    lbACTempValue.Text = strTemp;
                }
                else
                {
                    sbCoolTemp.Minimum = 32;
                    sbHeatTemp.Minimum = 32;
                    sbAutoTemp.Minimum = 32;
                    sbDryTemp.Minimum = 32;
                    sbCoolTemp.Maximum = 86;
                    sbHeatTemp.Maximum = 86;
                    sbAutoTemp.Maximum = 86;
                    sbDryTemp.Maximum = 86;
                    if (HDLSysPF.GetBit(CsConst.myRevBuf[38], 7) == 1)
                        strTemp = "-" + ((CsConst.myRevBuf[38] & (byte.MaxValue - (1 << 7)))).ToString() + "F";
                    else
                        strTemp = CsConst.myRevBuf[38].ToString() + "F";
                    lbACTempValue.Text = strTemp;
                }
                if (CsConst.myRevBuf[34] <= 1) myDLP.TemperatureType = CsConst.myRevBuf[34];
                cbPowerOn.Checked = (CsConst.myRevBuf[26] == 1);
                if (sbCoolTemp.Minimum <= CsConst.myRevBuf[27] && CsConst.myRevBuf[27] <= sbCoolTemp.Maximum)
                    sbCoolTemp.Value = Convert.ToInt32(CsConst.myRevBuf[27]);
                if (sbHeatTemp.Minimum <= CsConst.myRevBuf[28] && CsConst.myRevBuf[28] <= sbHeatTemp.Maximum)
                    sbHeatTemp.Value = Convert.ToInt32(CsConst.myRevBuf[28]);
                if (sbAutoTemp.Minimum <= CsConst.myRevBuf[29] && CsConst.myRevBuf[29] <= sbAutoTemp.Maximum)
                    sbAutoTemp.Value = Convert.ToInt32(CsConst.myRevBuf[29]);
                if (sbDryTemp.Minimum <= CsConst.myRevBuf[30] && CsConst.myRevBuf[30] <= sbDryTemp.Maximum)
                    sbDryTemp.Value = Convert.ToInt32(CsConst.myRevBuf[30]);
                if (CsConst.myRevBuf[31] < cbMode.Items.Count) cbMode.SelectedIndex = CsConst.myRevBuf[31];
                if (CsConst.myRevBuf[32] < cbFanSpeed.Items.Count) cbFanSpeed.SelectedIndex = CsConst.myRevBuf[32];
                if (CsConst.myRevBuf[33] == 1) chbWind.Checked = true;
                else chbWind.Checked = false;
                
                strMode = CsConst.mstrINIDefault.IniReadValue("Public", "0005" + CsConst.myRevBuf[35].ToString(), "");
                strSpeed = CsConst.mstrINIDefault.IniReadValue("Public", "0006" + CsConst.myRevBuf[36].ToString(), "");
                if (CsConst.myRevBuf[37] == 1) strWind = CsConst.mstrINIDefault.IniReadValue("Public", "99893", "");
                lbACStateValue.Text = strMode + "," + strSpeed + "," + strWind;
                if (myDLP == null) return;
                if (myDLP.MyAC == null) return;
                if (myDLP.MyAC.Count <= cbAC.SelectedIndex) return;
                EnviroAc temp = myDLP.MyAC[cbAC.SelectedIndex];
                temp.ACSwitch = CsConst.myRevBuf[26];
                temp.CoolingTemp = CsConst.myRevBuf[27];
                temp.HeatingTemp = CsConst.myRevBuf[28];
                temp.AutoTemp = CsConst.myRevBuf[29];
                temp.DryTemp = CsConst.myRevBuf[30];
                temp.ACMode = CsConst.myRevBuf[31];
                temp.ACWind = CsConst.myRevBuf[32];
                temp.ACFanEnable = CsConst.myRevBuf[33];
                temp.WorkingMode = CsConst.myRevBuf[35];
                temp.WorkingWind = CsConst.myRevBuf[36];
                temp.WorkingFan = CsConst.myRevBuf[37];
                temp.EnviromentTemp = CsConst.myRevBuf[38];
                CsConst.myRevBuf = new byte[1200];
            }
            Cursor.Current = Cursors.Default;

        }

        private void rbHeat1_CheckedChanged(object sender, EventArgs e)
        {
            panel16.Enabled = !rbHeat1.Checked;
            if (isReadingData) return;
        }

        private void SetHeatValue()
        {
            if (isReadingData) return;
            if (myDLP == null) return;
            if (myDLP.MyHeat == null) return;
            if (myDLP.MyHeat.Count <= 0) return;
            EnviroFH temp = myDLP.MyHeat[0];
            if (cbHeatType.SelectedIndex >= 0) temp.ControlMode = Convert.ToByte(cbHeatType.SelectedIndex);
        }

        private void cbHeatTempType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (myDLP.TemperatureType == 0)
            {
                cbHeatMinTemp.Items.Clear();
                cbHeatMaxTemp.Items.Clear();
                cbMaxTemp.Items.Clear();
                for (int i = 5; i <= 99; i++)
                {
                    cbHeatMinTemp.Items.Add(i.ToString() + "C");
                    cbHeatMaxTemp.Items.Add(i.ToString() + "C");

                }
                for (int i = 5; i <= 80; i++)
                {
                    cbMaxTemp.Items.Add("<=" + i.ToString() + "C");
                }

                cbHeatMinTemp.SelectedIndex = 0;
                cbHeatMaxTemp.SelectedIndex = 0;
                cbMaxTemp.SelectedIndex = 0;
                for (int i = 1; i <= 4; i++)
                {
                    HScrollBar temp = this.Controls.Find("sbHeatTemp" + i.ToString(), true)[0] as HScrollBar;
                    temp.Minimum = 5;
                    temp.Maximum = 90;
                    temp.Value = 5;
                }
            }
            else if (myDLP.TemperatureType == 1)
            {
                cbHeatMinTemp.Items.Clear();
                cbHeatMaxTemp.Items.Clear();
                cbMaxTemp.Items.Clear();
                for (int i = 41; i <= 210; i++)
                {
                    cbHeatMinTemp.Items.Add(i.ToString() + "F");
                    cbHeatMaxTemp.Items.Add(i.ToString() + "F");
                }
                for (int i = 41; i <= 176; i++)
                {
                    cbMaxTemp.Items.Add("<=" + i.ToString() + "F");
                }
                cbHeatMinTemp.SelectedIndex = 0;
                cbHeatMaxTemp.SelectedIndex = 0;
                cbMaxTemp.SelectedIndex = 0;
                for (int i = 1; i <= 4; i++)
                {
                    HScrollBar temp = this.Controls.Find("sbHeatTemp" + i.ToString(), true)[0] as HScrollBar;
                    temp.Minimum = 41;
                    temp.Maximum = 95;
                    temp.Value = 41;
                }
            }
            sbHeatTemp1_ValueChanged(null, null);
            sbHeatTemp2_ValueChanged(null, null);
            sbHeatTemp3_ValueChanged(null, null);
            sbHeatTemp4_ValueChanged(null, null);
        }

        private void cbHeatSensor3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbHeatSensor3.SelectedIndex == 0)
            {
                NumSub3.Enabled = false;
                NumDev3.Enabled = false;
                NumChn3.Enabled = false;
                cbMaxTemp.Enabled = false;
            }
            else
            {
                NumSub3.Enabled = true;
                NumDev3.Enabled = true;
                NumChn3.Enabled = true;
                cbMaxTemp.Enabled = true;
            }
        }

        private void cbHeatSensor1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbHeatSensor1.SelectedIndex == 0)
            {
                NumSub1.Enabled = false;
                NumDev1.Enabled = false;
                NumChn1.Enabled = false;
            }
            else
            {
                NumSub1.Enabled = true;
                NumDev1.Enabled = true;
                NumChn1.Enabled = true;
            }
        }

        private void cbHeatSensor2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbHeatSensor2.SelectedIndex == 0)
            {
                NumSub2.Enabled = false;
                NumDev2.Enabled = false;
                NumChn2.Enabled = false;
            }
            else
            {
                NumSub2.Enabled = true;
                NumDev2.Enabled = true;
                NumChn2.Enabled = true;
            }
        }

        private void cbHeatSensor4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbHeatSensor4.SelectedIndex == 0)
            {
                NumSub4.Enabled = false;
                NumDev4.Enabled = false;
                NumChn4.Enabled = false;
            }
            else
            {
                NumSub4.Enabled = true;
                NumDev4.Enabled = true;
                NumChn4.Enabled = true;
            }
        }

        private void sbHeatTemp1_ValueChanged(object sender, EventArgs e)
        {
            if (myDLP.TemperatureType == 0)
            {
                lbHeatTempValue1.Text = sbHeatTemp1.Value.ToString() + "C";
            }
            else
            {
                lbHeatTempValue1.Text = sbHeatTemp1.Value.ToString() + "F";
            }
        }

        private void sbHeatTemp2_ValueChanged(object sender, EventArgs e)
        {
            if (myDLP.TemperatureType == 0)
            {
                lbHeatTempValue2.Text = sbHeatTemp2.Value.ToString() + "C";
            }
            else
            {
                lbHeatTempValue2.Text = sbHeatTemp2.Value.ToString() + "F";
            }
        }

        private void sbHeatTemp3_ValueChanged(object sender, EventArgs e)
        {
            if (myDLP.TemperatureType == 0)
            {
                lbHeatTempValue3.Text = sbHeatTemp3.Value.ToString() + "C";
            }
            else
            {
                lbHeatTempValue3.Text = sbHeatTemp3.Value.ToString() + "F";
            }
        }

        private void sbHeatTemp4_ValueChanged(object sender, EventArgs e)
        {
            if (myDLP.TemperatureType == 0)
            {
                lbHeatTempValue4.Text = sbHeatTemp4.Value.ToString() + "C";
            }
            else
            {
                lbHeatTempValue4.Text = sbHeatTemp4.Value.ToString() + "F";
            }
        }

        private void sbModeChaneover_ValueChanged(object sender, EventArgs e)
        {
            lbModeChangeOverValue.Text = sbModeChaneover.Value.ToString() + "C";
        }

        private void cbHeatType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbHeatType.SelectedIndex == 2) btnAdvance.Enabled = false;
            else btnAdvance.Enabled = true;
        }

        private void cbControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbControl.SelectedIndex == 0)
            {
                grpHeatTemp.Enabled = false;
                grpWorking.Enabled = false;
                btnHeatTarget.Enabled = false;
            }
            else if (cbControl.SelectedIndex == 1)
            {
                grpHeatTemp.Enabled = true;
                grpWorking.Enabled = true;
                btnHeatTarget.Enabled = true;
            }
        }

        private void txtDesSub_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar) || e.KeyChar == 8)
                e.Handled = false;
            else
                e.Handled = true;
        }

        private void cbMusic_SelectedIndexChanged(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            showMusicInfo();
            ReadMusicUVTargets();
            Cursor.Current = Cursors.Default;
        }
        private void ReadMusicUVTargets()
        {
            if (myDLP == null) return;
            if (myDLP.MyMusic == null) return;
            if (myDLP.MyMusic.Count <= 0) return;
            EnviroMusic temp = myDLP.MyMusic[0];
           
            dgvMusic.Rows.Clear();
            for (int i = 0; i < temp.Targets.Length; i++)
            {
                string str = "";
                string strType = "";
                string str1 = "N/A";
                string str2 = "N/A";
                string str3 = "N/A";
                switch (i)
                {
                    #region
                    case 0: str = CsConst.mstrINIDefault.IniReadValue("Public", "00160", ""); break;
                    case 1: str = CsConst.mstrINIDefault.IniReadValue("Public", "00161", ""); break;
                    case 2: str = CsConst.mstrINIDefault.IniReadValue("Public", "00162", ""); break;
                    case 3: str = CsConst.mstrINIDefault.IniReadValue("Public", "00163", ""); break;
                    case 4: str = CsConst.mstrINIDefault.IniReadValue("Public", "00164", ""); break;
                    case 5: str = CsConst.mstrINIDefault.IniReadValue("Public", "00165", ""); break;
                    case 6: str = CsConst.mstrINIDefault.IniReadValue("Public", "00166", ""); break;
                    case 7: str = CsConst.mstrINIDefault.IniReadValue("Public", "00167", ""); break;
                    case 8: str = CsConst.mstrINIDefault.IniReadValue("Public", "00168", ""); break;
                    case 9: str = CsConst.mstrINIDefault.IniReadValue("Public", "00168", ""); break;
                    case 10: str = CsConst.mstrINIDefault.IniReadValue("Public", "00170", ""); break;
                    case 11: str = CsConst.mstrINIDefault.IniReadValue("Public", "00171", ""); break;
                    case 12: str = CsConst.mstrINIDefault.IniReadValue("Public", "00172", ""); break;
                    case 13: str = CsConst.mstrINIDefault.IniReadValue("Public", "00173", ""); break;
                    #endregion
                }
                UVCMD.ControlTargets tmp = temp.Targets[i];
                #region
                if (tmp.Type == 85)//场景
                {
                    strType = CsConst.mstrINIDefault.IniReadValue("CMDType", "00000", "");
                    str1 = tmp.Param1.ToString() + "(" + CsConst.WholeTextsList[2510].sDisplayName + ")";
                    str2 = tmp.Param2.ToString() + "(" + CsConst.WholeTextsList[2511].sDisplayName + ")";
                }
                else if (tmp.Type == 88)//通用开关
                {
                    strType = CsConst.mstrINIDefault.IniReadValue("CMDType", "00002", "");
                    str1 = tmp.Param1.ToString() + "(" + CsConst.WholeTextsList[2513].sDisplayName + ")";
                    if (tmp.Param2.ToString() == "0") str2 = CsConst.Status[0] + "(" + CsConst.WholeTextsList[2529].sDisplayName + ")";
                    else if (tmp.Param2.ToString() == "255") str2 = CsConst.Status[1] + "(" + CsConst.WholeTextsList[2529].sDisplayName + ")"; 
                }
                else if (tmp.Type == 89)//单路调节
                {
                    strType = CsConst.mstrINIDefault.IniReadValue("CMDType", "00004", "");
                    str1 = tmp.Param1.ToString() + "(" + CsConst.WholeTextsList[934].sDisplayName + ")";
                    str2 = tmp.Param2.ToString() + "(" + CsConst.WholeTextsList[2524].sDisplayName + ")";
                    int intTmp = tmp.Param3 * 256 + tmp.Param4;
                    str3 = HDLPF.GetStringFromTime(intTmp, ":") + "(" + CsConst.WholeTextsList[2525].sDisplayName + ")";
                }
                else strType = CsConst.mstrINIDefault.IniReadValue("CMDType", "00003", "");
                #endregion
                object[] obj = new object[] {(i+1).ToString()+str,tmp.SubnetID.ToString(),tmp.DeviceID.ToString(),
                                            strType,str1,str2,str3};
                dgvMusic.Rows.Add(obj);
            }
        }

        private void cbCurtain_SelectedIndexChanged(object sender, EventArgs e)
        {
            showCurtainInfo();
        }

        private void btnrefence_Click(object sender, EventArgs e)
        {
            ReadACWorkingState();
        }

        private void cbScenes_SelectedIndexChanged(object sender, EventArgs e)
        {
            showScenesInfo();
        }

        private void cbHeat_SelectedIndexChanged(object sender, EventArgs e)
        {
            showHeatingInfo();
        }

        private void cbACTempType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (myDLP.TemperatureType == 0)
            {
                sbCoolTemp.Minimum = 0;
                sbHeatTemp.Minimum = 0;
                sbAutoTemp.Minimum = 0;
                sbDryTemp.Minimum = 0;
                sbCoolTemp.Maximum = 32;
                sbHeatTemp.Maximum = 32;
                sbAutoTemp.Maximum = 32;
                sbDryTemp.Maximum = 32;
            }
            else if (myDLP.TemperatureType == 1)
            {
                sbCoolTemp.Minimum = 32;
                sbHeatTemp.Minimum = 32;
                sbAutoTemp.Minimum = 32;
                sbDryTemp.Minimum = 32;
                sbCoolTemp.Maximum = 86;
                sbHeatTemp.Maximum = 86;
                sbAutoTemp.Maximum = 86;
                sbDryTemp.Maximum = 86;
            }
            sbCoolTemp_ValueChanged(null, null);
            sbHeatTemp1_ValueChanged(null, null);
            sbAutoTemp_ValueChanged(null, null);
            sbDryTemp_ValueChanged(null, null);
        }

        private void btnKeyTarget_Click(object sender, EventArgs e)
        {
            if (dgvLightKeys.RowCount <= 0) return;
            int num = 0;
            if (dgvLightKeys.CurrentRow.Index > 0) num = dgvLightKeys.CurrentRow.Index;
            Form form = null;
            bool isOpen = true;
            foreach (Form frm in Application.OpenForms)
            {
                if (frm.Name == "FrmColorDLPTargets")
                {
                    if (((frm as FrmColorDLPTargets).DIndex == mintIDIndex) && ((frm as FrmColorDLPTargets).KeyType == 0))
                    {
                        form = frm;
                        form.TopMost = true;
                        form.WindowState = FormWindowState.Normal;
                        form.Activate();
                        form.TopMost = false;
                        isOpen = false;
                        break;
                    }
                }
            }
            if (isOpen)
            {
                FrmColorDLPTargets frmTarget = new FrmColorDLPTargets(myDevName, myDLP, cbLight.SelectedIndex, num, 0, MyintDeviceType);
                frmTarget.Show();
            }
        }

        private void btnScenesTargets_Click(object sender, EventArgs e)
        {
            if (dgvScenesKey.RowCount <= 0) return;
            int num = 0;
            if (dgvScenesKey.CurrentRow.Index > 0) num = dgvScenesKey.CurrentRow.Index;
            Form form = null;
            bool isOpen = true;
            foreach (Form frm in Application.OpenForms)
            {
                if (frm.Name == "FrmColorDLPTargets")
                {
                    if (((frm as FrmColorDLPTargets).DIndex == mintIDIndex) && ((frm as FrmColorDLPTargets).KeyType == 1))
                    {
                        form = frm;
                        form.TopMost = true;
                        form.WindowState = FormWindowState.Normal;
                        form.Activate();
                        form.TopMost = false;
                        isOpen = false;
                        break;
                    }
                }
            }
            if (isOpen)
            {
                FrmColorDLPTargets frmTarget = new FrmColorDLPTargets(myDevName, myDLP, cbScenes.SelectedIndex, num, 1, MyintDeviceType);
                frmTarget.Show();
            }
        }

        private void dgvLightKeys_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            cbKeyType.Visible = false;
            cbKeySaveDim.Visible = false;
            cbKeyMutex.Visible = false;
            cbKeyDim.Visible = false;
            int index = e.RowIndex;
            if (e.RowIndex >= 0)
            {
                string strMode = dgvLightKeys[2, e.RowIndex].Value.ToString();
                string strDim = dgvLightKeys[3, e.RowIndex].Value.ToString();
                string strSaveDim = dgvLightKeys[4, e.RowIndex].Value.ToString();
                string strMutex = dgvLightKeys[5, e.RowIndex].Value.ToString();

                addcontrols(2, index, cbKeyType, dgvLightKeys);
                addcontrols(3, index, cbKeyDim, dgvLightKeys);
                addcontrols(4, index, cbKeySaveDim, dgvLightKeys);
                if (e.RowIndex >= 9)
                {
                    cbKeyType.Items.Clear();
                    cbKeyType.Items.Add(CsConst.mstrINIDefault.IniReadValue("keyMode", "00000", ""));
                    cbKeyType.Items.Add(CsConst.mstrINIDefault.IniReadValue("keyMode", "00004", ""));
                    cbKeyType.Items.Add(CsConst.mstrINIDefault.IniReadValue("keyMode", "00005", ""));
                    cbKeyType.Items.Add(CsConst.mstrINIDefault.IniReadValue("keyMode", "00007", ""));
                    addcontrols(5, index, cbKeyMutex, dgvLightKeys);
                }
                else
                {
                    cbKeyType.Items.Clear();
                    cbKeyType.Items.Add(CsConst.mstrINIDefault.IniReadValue("keyMode", "00000", ""));
                    cbKeyType.Items.Add(CsConst.mstrINIDefault.IniReadValue("keyMode", "00001", ""));
                    cbKeyType.Items.Add(CsConst.mstrINIDefault.IniReadValue("keyMode", "00002", ""));
                    cbKeyType.Items.Add(CsConst.mstrINIDefault.IniReadValue("keyMode", "00003", ""));
                }
                if (cbKeyType.Visible && cbKeyType.Items.Count > 0)
                {
                    if (!cbKeyType.Items.Contains(strMode))
                        cbKeyType.SelectedIndex = 0;
                    else
                        cbKeyType.Text = strMode;
                }
                if (cbKeyDim.Visible && cbKeyDim.Items.Count > 0)
                {
                    if (!cbKeyDim.Items.Contains(strDim))
                        cbKeyDim.SelectedIndex = 0;
                    else
                        cbKeyDim.Text = strDim;
                }
                if (cbKeySaveDim.Visible && cbKeySaveDim.Items.Count > 0)
                {
                    if (!cbKeySaveDim.Items.Contains(strSaveDim))
                        cbKeySaveDim.SelectedIndex = 0;
                    else
                        cbKeySaveDim.Text = strSaveDim;
                }
                if (cbKeyMutex.Visible && cbKeyMutex.Items.Count > 0)
                {
                    if (!cbKeyMutex.Items.Contains(strMutex))
                        cbKeyMutex.SelectedIndex = 0;
                    else
                        cbKeyMutex.Text = strMutex;
                }
            }
        }

        private void addcontrols(int col, int row, Control con,DataGridView dgv)
        {
            con.Show();
            con.Visible = true;
            Rectangle rect = dgv.GetCellDisplayRectangle(col, row, true);
            con.Size = rect.Size;
            con.Top = rect.Top;
            con.Left = rect.Left;
        }

        private void saveKeySettingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            setAllVisible(false);
            if (dgvLightKeys.Rows.Count <= 0) return;
            if (myDLP == null) return;
            if (myDLP.MyKeys == null) return;
            Cursor.Current = Cursors.WaitCursor;
            byte[] ArayTmp = new byte[24];
            for (int i = 0; i < dgvLightKeys.Rows.Count; i++)
            {
                ArayTmp = new byte[24];
                string strRemark = dgvLightKeys[1, i].Value.ToString();
                ArayTmp[0] = Convert.ToByte(dgvLightKeys[0, i].Value.ToString());
                byte[] arayTmpRemark = HDLUDP.StringToByte(strRemark);
                if (arayTmpRemark.Length > 20)
                {
                    Array.Copy(arayTmpRemark, 0, ArayTmp, 1, 20);
                }
                else
                {
                    Array.Copy(arayTmpRemark, 0, ArayTmp, 1, arayTmpRemark.Length);
                }
                ArayTmp[21] = 0;
                ArayTmp[22] = Convert.ToByte(cbLight.SelectedIndex);
                ArayTmp[23] = 12;
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xE006, SubNetID, DevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(MyintDeviceType)) == false)
                {
                    CsConst.myRevBuf = new byte[1200];
                    break;
                }
            }
            ArayTmp = new byte[15];
            ArayTmp[0] = 0;
            ArayTmp[1] = Convert.ToByte(cbLight.SelectedIndex);
            ArayTmp[2] = 12;
            for (int j = 0; j < myDLP.MyKeys.Count; j++)
            {
                if (myDLP.MyKeys[j].PageID == Convert.ToByte(cbLight.SelectedIndex))
                {
                    ArayTmp[myDLP.MyKeys[j].ID + 2] = myDLP.MyKeys[j].Mode;
                }
            }

            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x19A0, SubNetID, DevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(MyintDeviceType)) == true)
            {
                CsConst.myRevBuf = new byte[1200];
            }
            else return;

            ArayTmp = new byte[15];
            ArayTmp[0] = 0;
            ArayTmp[1] = Convert.ToByte(cbLight.SelectedIndex);
            ArayTmp[2] = 12;
            for (int j = 0; j < myDLP.MyKeys.Count; j++)
            {
                if (myDLP.MyKeys[j].PageID == Convert.ToByte(cbLight.SelectedIndex))
                {
                    byte dim = myDLP.MyKeys[j].IsDimmer;
                    byte savedim = myDLP.MyKeys[j].SaveDimmer;
                    ArayTmp[myDLP.MyKeys[j].ID + 2] = Convert.ToByte(((dim << 4) & 0xF0) | (savedim & 0x0F));
                }
            }
            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x19A4, SubNetID, DevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(MyintDeviceType)) == true)
            {
                CsConst.myRevBuf = new byte[1200];
            }
            else return;

            ArayTmp = new byte[15];
            ArayTmp[0] = 0;
            ArayTmp[1] = Convert.ToByte(cbLight.SelectedIndex);
            ArayTmp[2] = 12;
            for (int j = 0; j < myDLP.MyKeys.Count; j++)
            {
                if (myDLP.MyKeys[j].PageID == Convert.ToByte(cbLight.SelectedIndex))
                {
                    if (10 <= myDLP.MyKeys[j].ID && myDLP.MyKeys[j].ID <= 11)
                    {
                        ArayTmp[myDLP.MyKeys[j].ID + 2] = myDLP.MyKeys[j].bytMutex;
                    }
                }
            }
            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x19A8, SubNetID, DevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(MyintDeviceType)) == true)
            {
                CsConst.myRevBuf = new byte[1200];
            }
            Cursor.Current = Cursors.Default;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MyActivePage = 2;

        }

        private void btnAdvance_Click(object sender, EventArgs e)
        {
            frmCmdSetup frmTemp = new frmCmdSetup(myDLP,myDevName, MyintDeviceType,null);
            frmTemp.ShowDialog();
        }

        private void saveScenesSettingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            setAllVisible(false);
            if (dgvScenesKey.Rows.Count <= 0) return;
            if (myDLP == null) return;
            if (myDLP.myScenes == null) return;
            Cursor.Current = Cursors.WaitCursor;
            byte[] ArayTmp = new byte[24];
            for (int i = 0; i < dgvScenesKey.Rows.Count; i++)
            {
                ArayTmp = new byte[24];
                string strRemark = dgvScenesKey[1, i].Value.ToString();
                ArayTmp[0] = Convert.ToByte(dgvScenesKey[0, i].Value.ToString());
                byte[] arayTmpRemark = HDLUDP.StringToByte(strRemark);
                if (arayTmpRemark.Length > 20)
                {
                    Array.Copy(arayTmpRemark, 0, ArayTmp, 1, 20);
                }
                else
                {
                    Array.Copy(arayTmpRemark, 0, ArayTmp, 1, arayTmpRemark.Length);
                }
                ArayTmp[21] = 0;
                ArayTmp[22] = Convert.ToByte(cbScenes.SelectedIndex);
                ArayTmp[23] = 12;
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xE006, SubNetID, DevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(MyintDeviceType)) == false)
                {
                    CsConst.myRevBuf = new byte[1200];
                    break;
                }
            }
            ArayTmp = new byte[15];
            ArayTmp[0] = 1;
            ArayTmp[1] = Convert.ToByte(cbScenes.SelectedIndex);
            ArayTmp[2] = 12;
            for (int j = 0; j < myDLP.myScenes.Count; j++)
            {
                if (myDLP.myScenes[j].PageID == Convert.ToByte(cbScenes.SelectedIndex))
                {
                    ArayTmp[myDLP.myScenes[j].ID + 2] = myDLP.myScenes[j].Mode;
                }
            }
            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x19A0, SubNetID, DevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(MyintDeviceType)) == true)
            {
                CsConst.myRevBuf = new byte[1200];
            }
            else return;

            ArayTmp = new byte[15];
            ArayTmp[0] = 1;
            ArayTmp[1] = Convert.ToByte(cbScenes.SelectedIndex);
            ArayTmp[2] = 12;
            for (int j = 0; j < myDLP.myScenes.Count; j++)
            {
                if (myDLP.myScenes[j].PageID == Convert.ToByte(cbScenes.SelectedIndex))
                {
                    byte dim = myDLP.myScenes[j].IsDimmer;
                    byte savedim = myDLP.myScenes[j].SaveDimmer;
                    ArayTmp[myDLP.myScenes[j].ID + 2] = Convert.ToByte(((dim << 4) & 0xF0) | (savedim & 0x0F));
                }
            }
            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x19A4, SubNetID, DevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(MyintDeviceType)) == true)
            {
                CsConst.myRevBuf = new byte[1200];
            }
            else return;

            ArayTmp = new byte[15];
            ArayTmp[0] = 1;
            ArayTmp[1] = Convert.ToByte(cbScenes.SelectedIndex);
            ArayTmp[2] = 12;
            for (int j = 0; j < myDLP.myScenes.Count; j++)
            {
                if (myDLP.myScenes[j].PageID == Convert.ToByte(cbScenes.SelectedIndex))
                {
                    ArayTmp[myDLP.myScenes[j].ID + 2] = myDLP.myScenes[j].bytMutex;
                }
            }
            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x19A8, SubNetID, DevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(MyintDeviceType)) == false)
            {
                CsConst.myRevBuf = new byte[1200];
            }

            Cursor.Current = Cursors.Default;
        }

        private void tmRead_Click(object sender, EventArgs e)
        {
            tbDown_Click(tbDown, null);
        }

        private void btnSaveTemp_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            byte[] ArayTmp = new byte[10];
            ArayTmp[0] = Convert.ToByte(cbAC.SelectedIndex);
            if (cbPowerOn.Checked == true)
                ArayTmp[1] = 1;
            else
                ArayTmp[1] = 0;
            ArayTmp[2] = Convert.ToByte(sbCoolTemp.Value);
            ArayTmp[3] = Convert.ToByte(sbHeatTemp.Value);
            ArayTmp[4] = Convert.ToByte(sbAutoTemp.Value);
            ArayTmp[5] = Convert.ToByte(sbDryTemp.Value);
            ArayTmp[6] = Convert.ToByte(cbMode.SelectedIndex);
            ArayTmp[7] = Convert.ToByte(cbFanSpeed.SelectedIndex);
            if (chbWind.Checked) ArayTmp[8] = 1;
            ArayTmp[9] = myDLP.TemperatureType;
            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x19B0, SubNetID, DevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(MyintDeviceType)) == true)
            {
                if (myDLP == null) return;
                if (myDLP.MyAC == null) return;
                if (myDLP.MyAC.Count <= cbAC.SelectedIndex) return;
                myDLP.MyAC[cbAC.SelectedIndex].ACSwitch = ArayTmp[1];
                myDLP.MyAC[cbAC.SelectedIndex].CoolingTemp = ArayTmp[2];
                myDLP.MyAC[cbAC.SelectedIndex].HeatingTemp = ArayTmp[3];
                myDLP.MyAC[cbAC.SelectedIndex].AutoTemp = ArayTmp[4];
                myDLP.MyAC[cbAC.SelectedIndex].DryTemp = ArayTmp[5];
                myDLP.MyAC[cbAC.SelectedIndex].ACMode = ArayTmp[6];
                myDLP.MyAC[cbAC.SelectedIndex].ACWind = ArayTmp[7];
                myDLP.MyAC[cbAC.SelectedIndex].ACFanEnable = ArayTmp[8];
            }
            Cursor.Current = Cursors.Default;
        }

        private void btnSaveAC_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            byte[] ArayTmp = new byte[42];
            ArayTmp[0] = Convert.ToByte(cbAC.SelectedIndex);
            if (chbEnable.Checked) ArayTmp[1] = 1;
            ArayTmp[2] = Convert.ToByte(NumHVACSub.Value);
            ArayTmp[3] = Convert.ToByte(NumHVACDev.Value);
            ArayTmp[4] = Convert.ToByte(NumAC.Value);
            ArayTmp[5] = Convert.ToByte(cbACType.SelectedIndex);
            if (chbFunction.Checked) ArayTmp[6] = 1;
            ArayTmp[7] = Convert.ToByte(cbPowerOnState.SelectedIndex);
            ArayTmp[11] = myDLP.MyAC[cbAC.SelectedIndex].FanEnable;
            ArayTmp[12] = myDLP.MyAC[cbAC.SelectedIndex].FanEnergySaveEnable;
            for (int i = 0; i < 11; i++)
                ArayTmp[13 + i] = myDLP.MyAC[cbAC.SelectedIndex].FanParam[i];
            for (int i = 0; i < 8; i++)
                ArayTmp[24 + i] = myDLP.MyAC[cbAC.SelectedIndex].CoolParam[i];
            for (int i = 0; i < 9; i++)
                ArayTmp[32 + i] = myDLP.MyAC[cbAC.SelectedIndex].OutDoorParam[i];
            ArayTmp[41] = myDLP.TemperatureType;
            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x19AC, SubNetID, DevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(MyintDeviceType)) == true)
            {
                if (myDLP == null) return;
                if (myDLP.MyAC == null) return;
                if (myDLP.MyAC.Count <= cbAC.SelectedIndex) return;
                myDLP.MyAC[cbAC.SelectedIndex].Enable = ArayTmp[1];
                myDLP.MyAC[cbAC.SelectedIndex].DesSubnetID = ArayTmp[2];
                myDLP.MyAC[cbAC.SelectedIndex].DesDevID = ArayTmp[3];
                myDLP.MyAC[cbAC.SelectedIndex].ACNum = ArayTmp[4];
                myDLP.MyAC[cbAC.SelectedIndex].ControlWays = ArayTmp[5];
                myDLP.MyAC[cbAC.SelectedIndex].OperationEnable = ArayTmp[6];
                myDLP.MyAC[cbAC.SelectedIndex].PowerOnRestoreState = ArayTmp[7];

                CsConst.myRevBuf = new byte[1200];
                if (chbEnable.Checked)
                {
                    ReadACWorkingState();
                }
            }
            Cursor.Current = Cursors.Default;
        }

        private void rbMusic1_CheckedChanged(object sender, EventArgs e)
        {
            if (rbMusic1.Checked)
                panel29.Enabled = true;
            else if (rbMusic2.Checked)
                panel29.Enabled = false;
        }

        private void btnSaveHeating_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                byte[] arayTmp = new byte[44];
                if (chbHeating.Checked) arayTmp[0] = 1;
                if (rbHeat1.Checked) arayTmp[1] = 0;
                else if (rbHeat2.Checked) arayTmp[1] = 1;
                else if (rbHeat3.Checked) arayTmp[1] = 2;
                arayTmp[2] = Convert.ToByte(cbHeatSensor1.SelectedIndex);
                arayTmp[3] = Convert.ToByte(NumSub1.Value);
                arayTmp[4] = Convert.ToByte(NumDev1.Value);
                arayTmp[5] = Convert.ToByte(NumChn1.Value);
                arayTmp[6] = Convert.ToByte(cbHeatSensor2.SelectedIndex);
                arayTmp[7] = Convert.ToByte(NumSub2.Value);
                arayTmp[8] = Convert.ToByte(NumDev2.Value);
                arayTmp[9] = Convert.ToByte(NumChn2.Value);
                arayTmp[10] = Convert.ToByte(cbHeatSensor3.SelectedIndex);
                arayTmp[11] = Convert.ToByte(NumSub3.Value);
                arayTmp[12] = Convert.ToByte(NumDev3.Value);
                arayTmp[13] = Convert.ToByte(NumChn3.Value);
                arayTmp[14] = Convert.ToByte(cbHeatSensor4.SelectedIndex);
                arayTmp[15] = Convert.ToByte(NumSub4.Value);
                arayTmp[16] = Convert.ToByte(NumDev4.Value);
                arayTmp[17] = Convert.ToByte(NumChn4.Value);
                if (chbHeat2.Checked) arayTmp[18] = 1;
                arayTmp[19] = Convert.ToByte(cbOutput.SelectedIndex);
                arayTmp[20] = Convert.ToByte(cbMinPWM.SelectedIndex);
                arayTmp[21] = Convert.ToByte(cbMaxPWM.SelectedIndex);
                arayTmp[22] = Convert.ToByte(cbHeatSpeed.SelectedIndex);
                arayTmp[23] = Convert.ToByte(cbCycle.SelectedIndex);
                string str1 = "000";
                string str2 = "0";
                string str3 = "0";
                string str4 = "0";
                string str5 = "0";
                string str6 = "0";
                if (chbListHeatMode.GetItemChecked(0) == true) str2 = "1";
                if (chbListHeatMode.GetItemChecked(1) == true) str3 = "1";
                if (chbListHeatMode.GetItemChecked(2) == true) str4 = "1";
                if (chbListHeatMode.GetItemChecked(3) == true) str5 = "1";
                if (chbListHeatMode.GetItemChecked(4) == true) str6 = "1";
                string str = str1 + str2 + str3 + str4 + str5 + str6;
                arayTmp[24] = Convert.ToByte(GlobalClass.BitToInt(str));
                if (chbHeat1.Checked) arayTmp[25] = 1;
                arayTmp[26] = Convert.ToByte(numStart1.Value);
                arayTmp[27] = Convert.ToByte(numStart2.Value);
                arayTmp[28] = Convert.ToByte(numEnd1.Value);
                arayTmp[29] = Convert.ToByte(numEnd2.Value);
                str = cbMaxTemp.Text;
                str = str.Replace("<=", "");
                str = str.Replace("C", "");
                str = str.Replace("F", "");
                arayTmp[30] = Convert.ToByte(str);
                arayTmp[32] = Convert.ToByte(cbControl.SelectedIndex);
                arayTmp[33] = Convert.ToByte(cbHeatType.SelectedIndex);
                if (chbHeat3.Checked) arayTmp[34] = 1;
                str1 = "00000";
                str2 = "0";
                str3 = "0";
                str4 = "0";
                if (chbHeat4.Checked) str2 = "1";
                if (chbHeat5.Checked) str3 = "1";
                if (chbHeat6.Checked) str4 = "1";
                str = str1 + str2 + str3 + str4;
                arayTmp[35] = Convert.ToByte(GlobalClass.BitToInt(str));
                arayTmp[36] = Convert.ToByte(sbModeChaneover.Value);
                //arayTmp[37] = Convert.ToByte(txtDesSub.Text);
                //arayTmp[38] = Convert.ToByte(txtDesDev.Text);
                //arayTmp[39] = Convert.ToByte(cbDesChn.SelectedIndex + 1);
                str = cbHeatMinTemp.Text;
                str = str.Replace("C", "");
                str = str.Replace("F", "");
                arayTmp[40] = Convert.ToByte(str);
                str = cbHeatMaxTemp.Text;
                str = str.Replace("C", "");
                str = str.Replace("F", "");
                arayTmp[41] = Convert.ToByte(str);
                arayTmp[42] = myDLP.TemperatureType;
                arayTmp[43] = Convert.ToByte(0);
                if (CsConst.mySends.AddBufToSndList(arayTmp, 0x1942, SubNetID, DevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(MyintDeviceType)) == true)
                {
                    if (myDLP == null) return;
                    if (myDLP.MyHeat == null) return;
                    if (myDLP.MyHeat.Count <= 0) return;
                    myDLP.MyHeat[0].HeatEnable = arayTmp[0];
                    myDLP.MyHeat[0].SourceTemp = arayTmp[0];
                    myDLP.MyHeat[0].OutDoorParam = new byte[20];
                    Array.Copy(arayTmp, 2, myDLP.MyHeat[0].OutDoorParam, 0, 8);
                    myDLP.MyHeat[0].SourceParam = new byte[20];
                    Array.Copy(arayTmp, 10, myDLP.MyHeat[0].SourceParam, 0, 8);
                    myDLP.MyHeat[0].PIDEnable = arayTmp[18];
                    myDLP.MyHeat[0].OutputType = arayTmp[19];
                    myDLP.MyHeat[0].minPWM = arayTmp[20];
                    myDLP.MyHeat[0].maxPWM = arayTmp[21];
                    myDLP.MyHeat[0].Speed = arayTmp[22];
                    myDLP.MyHeat[0].Cycle = arayTmp[23];
                    myDLP.MyHeat[0].ModeAry = new byte[5];
                    if (chbListHeatMode.GetItemChecked(0))
                        myDLP.MyHeat[0].ModeAry[0] = 1;
                    if (chbListHeatMode.GetItemChecked(1))
                        myDLP.MyHeat[0].ModeAry[1] = 1;
                    if (chbListHeatMode.GetItemChecked(2))
                        myDLP.MyHeat[0].ModeAry[2] = 1;
                    if (chbListHeatMode.GetItemChecked(3))
                        myDLP.MyHeat[0].ModeAry[3] = 1;
                    if (chbListHeatMode.GetItemChecked(4))
                        myDLP.MyHeat[0].ModeAry[4] = 1;
                    myDLP.MyHeat[0].Switch = arayTmp[25];
                    myDLP.MyHeat[0].TimeAry = new byte[4];
                    Array.Copy(arayTmp, 26, myDLP.MyHeat[0].TimeAry, 0, 4);
                    myDLP.MyHeat[0].ProtectTemperature = arayTmp[30];
                    myDLP.MyHeat[0].ControlMode = arayTmp[32];
                    myDLP.MyHeat[0].HeatType = arayTmp[33];
                    myDLP.MyHeat[0].SysEnable = new byte[4];
                    if (chbHeat6.Checked)
                        myDLP.MyHeat[0].ModeAry[0] = 1;
                    if (chbHeat5.Checked)
                        myDLP.MyHeat[0].ModeAry[1] = 1;
                    if (chbHeat4.Checked)
                        myDLP.MyHeat[0].ModeAry[2] = 1;
                    if (chbHeat3.Checked)
                        myDLP.MyHeat[0].ModeAry[3] = 1;
                    myDLP.MyHeat[0].CompenValue = arayTmp[36];
                    myDLP.MyHeat[0].DesSubnetID = arayTmp[37];
                    myDLP.MyHeat[0].DesDeviceID = arayTmp[38];
                    myDLP.MyHeat[0].Channel = arayTmp[39];
                    myDLP.MyHeat[0].minTemp = arayTmp[40];
                    myDLP.MyHeat[0].maxTemp = arayTmp[41];
                    CsConst.myRevBuf = new byte[1200];
                }
                else return;
                arayTmp = new byte[8];
                arayTmp[0] = myDLP.TemperatureType;
                if (chbHeatSwitch.Checked) arayTmp[1] = 1;
                arayTmp[2] = Convert.ToByte(cbCurrentMode.SelectedIndex + 1);
                arayTmp[3] = Convert.ToByte(sbHeatTemp1.Value);
                arayTmp[4] = Convert.ToByte(sbHeatTemp2.Value);
                arayTmp[5] = Convert.ToByte(sbHeatTemp3.Value);
                arayTmp[6] = Convert.ToByte(sbHeatTemp4.Value);
                arayTmp[7] = Convert.ToByte(0);
                if (CsConst.mySends.AddBufToSndList(arayTmp, 0x1946, SubNetID, DevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(MyintDeviceType)) == true)
                {
                    myDLP.MyHeat[0].WorkingSwitch = arayTmp[1];
                    myDLP.MyHeat[0].WorkingTempMode = arayTmp[2];
                    myDLP.MyHeat[0].ModeTemp = new byte[4];
                    Array.Copy(arayTmp, 3, myDLP.MyHeat[0].ModeTemp, 0, 4);
                    CsConst.myRevBuf = new byte[1200];
                }
                Cursor.Current = Cursors.Default;
            }
            catch
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnSaveMusic_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            byte[] arayTmp = new byte[4];
            if (rbMusic1.Checked) arayTmp[0] = 1;
            arayTmp[1] = Convert.ToByte(cbMusicZone.SelectedIndex + 1);
            arayTmp[2] = Convert.ToByte(cbMusicMode.SelectedIndex);
            arayTmp[3] = Convert.ToByte(0);
            if (CsConst.mySends.AddBufToSndList(arayTmp, 0x1932, SubNetID, DevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(MyintDeviceType)) == true)
            {
                if (myDLP == null) return;
                if (myDLP.MyMusic == null) return;
                if (myDLP.MyMusic.Count <= 0) return;
                myDLP.MyMusic[0].Enable = arayTmp[0];
                myDLP.MyMusic[0].CurrentZoneID = arayTmp[1];
                myDLP.MyMusic[0].Type = arayTmp[2];
                CsConst.myRevBuf = new byte[1200];
            }
            else return;
            arayTmp = new byte[26];
            arayTmp[0] = 0;
            arayTmp[25] = Convert.ToByte(0);
            Array.Copy(myDLP.MyMusic[0].aryNetDevID, 0, arayTmp, 1, 24);
            if (CsConst.mySends.AddBufToSndList(arayTmp, 0x1936, SubNetID, DevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(MyintDeviceType)) == true)
            {
                CsConst.myRevBuf = new byte[1200];
            }
            arayTmp = new byte[26];
            arayTmp[0] = 1;
            arayTmp[25] = Convert.ToByte(0);
            Array.Copy(myDLP.MyMusic[0].aryNetDevID, 24, arayTmp, 1, 24);
            if (CsConst.mySends.AddBufToSndList(arayTmp, 0x1936, SubNetID, DevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(MyintDeviceType)) == true)
            {
                CsConst.myRevBuf = new byte[1200];
            }
            Cursor.Current = Cursors.Default;
        }

        private void dgvScenesKey_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            cbScenesDim.Visible = false;
            cbScenesSaveDim.Visible = false;
            cbScenesMutex.Visible = false;
            cbScenesDim.Visible = false;
            int index = e.RowIndex;
            if (e.RowIndex >= 0)
            {
                string strMode = dgvScenesKey[2, e.RowIndex].Value.ToString();
                string strDim = dgvScenesKey[3, e.RowIndex].Value.ToString();
                string strSaveDim = dgvScenesKey[4, e.RowIndex].Value.ToString();
                string strMutex = dgvScenesKey[5, e.RowIndex].Value.ToString();

                addcontrols(2, index, cbScenesType, dgvScenesKey);
                addcontrols(3, index, cbScenesDim, dgvScenesKey);
                addcontrols(4, index, cbScenesSaveDim, dgvScenesKey);
                addcontrols(5, index, cbScenesMutex, dgvScenesKey);
                if (cbScenesType.Visible && cbScenesType.Items.Count > 0)
                {
                    if (!cbScenesType.Items.Contains(strMode))
                        cbScenesType.SelectedIndex = 0;
                    else
                        cbScenesType.Text = strMode;
                }
                if (cbScenesDim.Visible && cbScenesDim.Items.Count > 0)
                {
                    if (!cbScenesDim.Items.Contains(strDim))
                        cbScenesDim.SelectedIndex = 0;
                    else
                        cbScenesDim.Text = strDim;
                }
                if (cbScenesSaveDim.Visible && cbScenesSaveDim.Items.Count > 0)
                {
                    if (!cbScenesSaveDim.Items.Contains(strSaveDim))
                        cbScenesSaveDim.SelectedIndex = 0;
                    else
                        cbScenesSaveDim.Text = strSaveDim;
                }
                if (cbScenesMutex.Visible && cbScenesMutex.Items.Count > 0)
                {
                    if (!cbScenesMutex.Items.Contains(strMutex))
                        cbScenesMutex.SelectedIndex = 0;
                    else
                        cbScenesMutex.Text = strMutex;
                }
            }
        }

        private void cbMusicZone_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (myDLP == null) return;
            if (myDLP.MyMusic == null) return;
            if (myDLP.MyMusic.Count <= 0) return;
            if (isReadingData) return;
            myDLP.MyMusic[0].CurrentZoneID = Convert.ToByte(cbMusicZone.SelectedIndex + 1);
            txtMusicSub.Text = myDLP.MyMusic[0].aryNetDevID[cbMusicZone.SelectedIndex * 2].ToString();
            txtMusicDev.Text = myDLP.MyMusic[0].aryNetDevID[cbMusicZone.SelectedIndex * 2 + 1].ToString();
        }

        private void txtMusicSub_TextAlignChanged(object sender, EventArgs e)
        {

        }

        private void txtMusicDev_TextChanged(object sender, EventArgs e)
        {
            
        }


        private void btnKeyRead_Click(object sender, EventArgs e)
        {
            if (myDLP == null) return;
            if (myDLP.MyKeys == null) return;
            if (myDLP.MyKeys.Count < cbLight.Items.Count * 11) tbDown_Click(tbDown, null);
            setAllVisible(false);
            Cursor.Current = Cursors.WaitCursor;
            byte[] ArayTmp = new byte[3];
            ArayTmp[0] = 0;
            ArayTmp[1] = Convert.ToByte(cbLight.SelectedIndex);
            ArayTmp[2] = 12;
            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x199E, SubNetID, DevID, false, true, true,CsConst.minAllWirelessDeviceType.Contains(MyintDeviceType)) == true)
            {
                for (byte j = 0; j < 11; j++)
                {
                    HDLButton temp = myDLP.MyKeys[cbLight.SelectedIndex * 11 + j];
                    temp.PageID = Convert.ToByte(cbLight.SelectedIndex);
                    temp.ID = Convert.ToByte(j + 1);
                    temp.KeyTargets = new List<UVCMD.ControlTargets>();
                    temp.Mode = CsConst.myRevBuf[28 + j];
                    temp.Remark = "";
                    
                }
                CsConst.myRevBuf = new byte[1200];

                for (byte j = 1; j <= 11; j++)//12个按键
                {
                    ArayTmp = new byte[4];
                    ArayTmp[0] = j;
                    ArayTmp[1] = 0;
                    ArayTmp[2] = Convert.ToByte(cbLight.SelectedIndex);
                    ArayTmp[3] = 12;
                    //读取按键备注
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xE004, SubNetID, DevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(MyintDeviceType)) == true)
                    {
                        byte[] arayRemark = new byte[20];
                        Array.Copy(CsConst.myRevBuf, 26, arayRemark, 0, 20);
                        myDLP.MyKeys[(cbLight.SelectedIndex * 11) + (j - 1)].Remark = HDLPF.Byte2String(arayRemark);
                        CsConst.myRevBuf = new byte[1200];
                    }
                    else return;
                }
                //读取按键调光使能和保存
                ArayTmp = new byte[3];
                ArayTmp[0] = 0;
                ArayTmp[1] = Convert.ToByte(cbLight.SelectedIndex);
                ArayTmp[2] = 12;
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x19A2, SubNetID, DevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(MyintDeviceType)) == true)
                {
                    for (byte j = 0; j < 11; j++)
                    {
                        byte bytTmp = CsConst.myRevBuf[28 + j];
                        string str = GlobalClass.IntToBit(Convert.ToInt32(bytTmp), 8);
                        if (str.Substring(3, 1) == "1") myDLP.MyKeys[(cbLight.SelectedIndex * 11) + j].IsDimmer = 1;
                        else myDLP.MyKeys[(cbLight.SelectedIndex * 11) + j].IsDimmer = 0;
                        if (str.Substring(7, 1) == "1") myDLP.MyKeys[(cbLight.SelectedIndex * 11) + j].SaveDimmer = 1;
                        else myDLP.MyKeys[(cbLight.SelectedIndex * 11) + j].SaveDimmer = 0;
                    }
                    CsConst.myRevBuf = new byte[1200];
                }
                else return;

                //读取按键互斥
                ArayTmp = new byte[3];
                ArayTmp[0] = 0;
                ArayTmp[1] = Convert.ToByte(cbLight.SelectedIndex);
                ArayTmp[2] = 12;
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x19A6, SubNetID, DevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(MyintDeviceType)) == true)
                {
                    for (byte j = 0; j < 11; j++)
                    {
                        myDLP.MyKeys[(cbLight.SelectedIndex * 11) + j].bytMutex = CsConst.myRevBuf[28 + j];
                    }
                    CsConst.myRevBuf = new byte[1200];
                }
                else return;
                Cursor.Current = Cursors.Default;
            }
            showKeyInfo();
        }

        private void btnScenesRead_Click(object sender, EventArgs e)
        {
            if (myDLP == null) return;
            if (myDLP.myScenes == null) return;
            if (myDLP.myScenes.Count < cbScenes.Items.Count * 12) tbDown_Click(tbDown, null);
            Cursor.Current = Cursors.WaitCursor;
            setAllVisible(false);
            byte[] ArayTmp = new byte[3];
            ArayTmp[0] = 1;
            ArayTmp[1] = Convert.ToByte(cbScenes.SelectedIndex);
            ArayTmp[2] = 12;
            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x199E, SubNetID, DevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(MyintDeviceType)) == true)
            {
                for (byte j = 0; j < 12; j++)
                {
                    HDLButton temp = myDLP.myScenes[cbScenes.SelectedIndex * 12 + j];
                    temp.PageID = Convert.ToByte(cbScenes.SelectedIndex);
                    temp.ID = Convert.ToByte(j + 1);
                    temp.KeyTargets = new List<UVCMD.ControlTargets>();
                    temp.Mode = CsConst.myRevBuf[28 + j];
                    temp.Remark = "";
                }
                CsConst.myRevBuf = new byte[1200];

                for (byte j = 1; j <= 12; j++)//12个按键
                {
                    ArayTmp = new byte[4];
                    ArayTmp[0] = j;
                    ArayTmp[1] = 1;
                    ArayTmp[2] = Convert.ToByte(cbScenes.SelectedIndex);
                    ArayTmp[3] = 12;
                    //读取按键备注
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xE004, SubNetID, DevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(MyintDeviceType)) == true)
                    {
                        byte[] arayRemark = new byte[20];
                        Array.Copy(CsConst.myRevBuf, 26, arayRemark, 0, 20);
                        myDLP.myScenes[(cbScenes.SelectedIndex * 12) + (j - 1)].Remark = HDLPF.Byte2String(arayRemark);
                        CsConst.myRevBuf = new byte[1200];
                    }
                    else return;
                }
                //读取按键调光使能和保存
                ArayTmp = new byte[3];
                ArayTmp[0] = 1;
                ArayTmp[1] = Convert.ToByte(cbScenes.SelectedIndex);
                ArayTmp[2] = 12;
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x19A2, SubNetID, DevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(MyintDeviceType)) == true)
                {
                    for (byte j = 0; j < 12; j++)
                    {
                        byte bytTmp = CsConst.myRevBuf[28 + j];
                        string str = GlobalClass.IntToBit(Convert.ToInt32(bytTmp), 8);
                        if (str.Substring(3, 1) == "1") myDLP.myScenes[(cbScenes.SelectedIndex * 12) + j].IsDimmer = 1;
                        else myDLP.myScenes[(cbScenes.SelectedIndex * 12) + j].IsDimmer = 0;
                        if (str.Substring(7, 1) == "1") myDLP.myScenes[(cbScenes.SelectedIndex * 12) + j].SaveDimmer = 1;
                        else myDLP.myScenes[(cbScenes.SelectedIndex * 12) + j].SaveDimmer = 0;
                    }
                    CsConst.myRevBuf = new byte[1200];
                }
                else return;

                //读取按键互斥
                ArayTmp = new byte[3];
                ArayTmp[0] = 1;
                ArayTmp[1] = Convert.ToByte(cbScenes.SelectedIndex);
                ArayTmp[2] = 12;
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x19A6, SubNetID, DevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(MyintDeviceType)) == true)
                {
                    for (byte j = 0; j < 12; j++)
                    {
                        myDLP.myScenes[(cbScenes.SelectedIndex * 12) + j].bytMutex = CsConst.myRevBuf[28 + j];
                    }
                    CsConst.myRevBuf = new byte[1200];
                }
                else return;
            }
            showScenesInfo();
            Cursor.Current = Cursors.Default;
        }

        private void dgvMusic_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            frmCmdSetup frmTemp = new frmCmdSetup(myDLP,myDevName, MyintDeviceType, null);
            frmTemp.ShowDialog();
        }

        private void rb1_CheckedChanged(object sender, EventArgs e)
        {
            txt1.Visible = rb2.Checked;
            lbStandby.Visible = rb2.Checked;
            sb3.Visible = rb2.Checked;
            lbv3.Visible = rb2.Checked;
            lbS.Visible = rb2.Checked;
            if (isReadingData) return;
            if (myDLP == null) return;
            if (myDLP.BasicInfo == null) myDLP.BasicInfo = new byte[20];
            if (rb1.Checked) myDLP.BasicInfo[0] = 100;
            else if (rb2.Checked) myDLP.BasicInfo[0] = Convert.ToByte(txt1.Text);
        }

        private void sb1_ValueChanged(object sender, EventArgs e)
        {
            lb3.Text = (sb1.Value).ToString() + "%";
            if (isReadingData) return;
            if (myDLP == null) return;
            if (myDLP.BasicInfo == null) myDLP.BasicInfo = new byte[20];
            myDLP.BasicInfo[2] = Convert.ToByte(sb1.Value);
        }

        private void sb2_ValueChanged(object sender, EventArgs e)
        {
            lb5.Text = sb2.Value.ToString();
            if (isReadingData) return;
            if (myDLP == null) return;
            if (myDLP.BasicInfo == null) myDLP.BasicInfo = new byte[20];
            myDLP.BasicInfo[4] = Convert.ToByte(sb2.Value);
        }

        private void txt1_TextChanged(object sender, EventArgs e)
        {
            if (txt1.Text.Length >= 2)
            {
                string str = txt1.Text;
                txt1.Text = HDLPF.IsNumStringMode(str, 10, 99);
                txt1.SelectionStart = txt1.Text.Length;
                if (isReadingData) return;
                if (myDLP == null) return;
                if (myDLP.BasicInfo == null) myDLP.BasicInfo = new byte[20];
                myDLP.BasicInfo[0] = Convert.ToByte(txt1.Text);
            }
        }

        private void num2_ValueChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt32(num1.Value) == 0)
            {
                if (Convert.ToInt32(num2.Value) < 3) num2.Value = 3;
            }
            if (isReadingData) return;
            if (myDLP == null) return;
            if (myDLP.BasicInfo == null) myDLP.BasicInfo = new byte[20];
            myDLP.BasicInfo[5] = Convert.ToByte(Convert.ToInt32(num1.Value) * 10 + Convert.ToInt32(num2.Value));
        }

        private void num1_ValueChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt32(num1.Value) == 25)
            {
                num2.Value = 0;
            }
            if (isReadingData) return;
            if (myDLP == null) return;
            if (myDLP.BasicInfo == null) myDLP.BasicInfo = new byte[20];
            myDLP.BasicInfo[5] = Convert.ToByte(Convert.ToInt32(num1.Value) * 10 + Convert.ToInt32(num2.Value));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            tbDown_Click(tbUpload, null);
        }

        private void sbBroadcast_ValueChanged(object sender, EventArgs e)
        {
            if (myDLP.TemperatureType == 0)
                lbAdjustValue.Text = (sbBroadcast.Value - 10).ToString() + "C";
            else if(myDLP.TemperatureType == 1)
                lbAdjustValue.Text = (sbBroadcast.Value - 10).ToString() + "F";
        }

        private void btnBroadcast_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            byte[] ArayTmp = new byte[4];
            if (chbBroadCast.Checked)
                ArayTmp[0] = 1;
            ArayTmp[1] = Convert.ToByte(txtBroadSub.Text);
            ArayTmp[2] = Convert.ToByte(txtBroadDev.Text);
            ArayTmp[3] = Convert.ToByte(sbBroadcast.Value);
            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xE0FA, SubNetID, DevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(MyintDeviceType)) == true)
            {
                for (int i = 0; i < 4;i++ )
                    myDLP.BroadCastAry[i] = ArayTmp[i];
                CsConst.myRevBuf = new byte[1200];   
            }
            Cursor.Current = Cursors.Default;
        }

        private void chb2_CheckedChanged(object sender, EventArgs e)
        {
            if (isReadingData) return;
            if (myDLP == null) return;
            if (myDLP.BasicInfo == null) myDLP.BasicInfo = new byte[20];
            if (chb2.Checked)
                myDLP.BasicInfo[1] = 1;
            else
                myDLP.BasicInfo[1] = 0;
        }

        private void chb1_CheckedChanged(object sender, EventArgs e)
        {
            if (isReadingData) return;
            if (myDLP == null) return;
            if (myDLP.BasicInfo == null) myDLP.BasicInfo = new byte[20];
            if (chb2.Checked)
                myDLP.BasicInfo[3] = 1;
            else
                myDLP.BasicInfo[3] = 0;
        }

        private void cbTemp_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isReadingData) return;
            if (myDLP == null) return;
            myDLP.TemperatureType = Convert.ToByte(cbTemp.SelectedIndex);
        }

        private void rbTime1_CheckedChanged(object sender, EventArgs e)
        {
            if (isReadingData) return;
            if (myDLP == null) return;
            if (myDLP.BasicInfo == null) myDLP.BasicInfo = new byte[20];
            if (rbTime1.Checked) myDLP.BasicInfo[13] = 0;
            else myDLP.BasicInfo[13] = 1;
        }

        private void rbTimeDate1_CheckedChanged(object sender, EventArgs e)
        {
            if (isReadingData) return;
            if (myDLP == null) return;
            if (myDLP.BasicInfo == null) myDLP.BasicInfo = new byte[20];
            myDLP.BasicInfo[14] = Convert.ToByte(Convert.ToInt32((sender as RadioButton).Tag));
        }

        private void txtBroadSub_TextChanged(object sender, EventArgs e)
        {
            if (txtBroadSub.Text.Length > 0)
            {
                string str = txtBroadSub.Text;
                txtBroadSub.Text = HDLPF.IsNumStringMode(str, 0, 255);
                txtBroadSub.SelectionStart = txtBroadSub.Text.Length;
            }
        }

        private void txtBroadDev_TextChanged(object sender, EventArgs e)
        {
            if (txtBroadDev.Text.Length > 0)
            {
                string str = txtBroadDev.Text;
                txtBroadDev.Text = HDLPF.IsNumStringMode(str, 0, 255);
                txtBroadDev.SelectionStart = txtBroadDev.Text.Length;
            }
        }

        private void btnUI_Click(object sender, EventArgs e)
        {
            FrmColorDLPUI frmtemp = new FrmColorDLPUI(myDevName, MyintDeviceType, myDLP);
            frmtemp.ShowDialog();
        }

        private void chbHeating_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void chbDisplay_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (isReadingData) return;
            if (myDLP == null) return;
            if (myDLP.BasicInfo == null) myDLP.BasicInfo=new byte[20];
            if (e.Index == 0)
                myDLP.BasicInfo[15] = Convert.ToByte(e.NewValue);
            if (e.Index == 1)
                myDLP.BasicInfo[16] = Convert.ToByte(e.NewValue);
        }


        private void txtMusicSub_Leave(object sender, EventArgs e)
        {
            string str = txtMusicSub.Text;
            txtMusicSub.Text = HDLPF.IsNumStringMode(str, 0, 255);
            txtMusicSub.SelectionStart = txtMusicSub.Text.Length;
            if (isReadingData) return;
            if (myDLP == null) return;
            if (myDLP.MyMusic == null) return;
            if (myDLP.MyMusic.Count <= 0) return;
            myDLP.MyMusic[0].aryNetDevID[cbMusicZone.SelectedIndex * 2] = Convert.ToByte(txtMusicSub.Text);
        }

        private void txtMusicDev_Leave(object sender, EventArgs e)
        {
            string str = txtMusicDev.Text;
            txtMusicDev.Text = HDLPF.IsNumStringMode(str, 0, 255);
            txtMusicDev.SelectionStart = txtMusicDev.Text.Length;
            if (isReadingData) return;
            if (myDLP == null) return;
            if (myDLP.MyMusic == null) return;
            if (myDLP.MyMusic.Count <= 0) return;
            myDLP.MyMusic[0].aryNetDevID[cbMusicZone.SelectedIndex * 2 + 1] = Convert.ToByte(txtMusicDev.Text);
        }

        private void sb3_ValueChanged(object sender, EventArgs e)
        {
            lbv3.Text = (sb3.Value * 10).ToString() + "%";
            if (isReadingData) return;
            if (myDLP == null) return;
            if (myDLP.BasicInfo == null) myDLP.BasicInfo = new byte[20];
            myDLP.BasicInfo[6] = Convert.ToByte(sb3.Value);
        }

        private void btnHeatTarget_Click(object sender, EventArgs e)
        {
            FrmCalculateTargetForColorDLP frmTmp = new FrmCalculateTargetForColorDLP(myDevName, MyintDeviceType, myDLP, 0);
            frmTmp.ShowDialog();
        }

        private void btnSaveSensor_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                byte[] arayTmp = new byte[14];
                arayTmp[0] = 2;
                arayTmp[1] = 0;
                arayTmp[2] = Convert.ToByte(cbPM.SelectedIndex);
                arayTmp[3] = 2;
                arayTmp[4] = Convert.ToByte(cbPM1.SelectedIndex);
                arayTmp[5] = Convert.ToByte(numSubPM1.Value);
                arayTmp[6] = Convert.ToByte(numDevPM1.Value);
                arayTmp[7] = Convert.ToByte(numChnPM1.Value);
                if (cbTypePM1.SelectedIndex == 0)
                    arayTmp[8] = 9;
                else
                    arayTmp[8] = 19;
                arayTmp[9] = Convert.ToByte(cbPM2.SelectedIndex);
                arayTmp[10] = Convert.ToByte(numSubPM2.Value);
                arayTmp[11] = Convert.ToByte(numDevPM2.Value);
                arayTmp[12] = Convert.ToByte(numChnPM2.Value);
                if (cbTypePM2.SelectedIndex == 0)
                    arayTmp[13] = 9;
                else
                    arayTmp[13] = 19;
                if (CsConst.mySends.AddBufToSndList(arayTmp, 0xE4FE, SubNetID, DevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(MyintDeviceType)) == true)
                {
                    CsConst.myRevBuf = new byte[1200];
                    if (myDLP == null) return;
                    if (myDLP.mySensor == null) return;
                    byte[] arayValue = new byte[11];
                    Array.Copy(arayTmp, 3, arayValue, 0, 11);
                    myDLP.mySensor[cbPM.SelectedIndex].arayPM = arayValue;
                }
                
            }
            catch
            {
            }
            Cursor.Current = Cursors.Default;
        }

        private void cbPM1_SelectedIndexChanged(object sender, EventArgs e)
        {
            numSubPM1.Enabled = (cbPM1.SelectedIndex != 0);
            numDevPM1.Enabled = (cbPM1.SelectedIndex != 0);
            numChnPM1.Enabled = (cbPM1.SelectedIndex != 0);
        }

        private void cbPM2_SelectedIndexChanged(object sender, EventArgs e)
        {
            numSubPM2.Enabled = (cbPM1.SelectedIndex != 0);
            numDevPM2.Enabled = (cbPM1.SelectedIndex != 0);
            numChnPM2.Enabled = (cbPM1.SelectedIndex != 0);

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isReadingData) return;
            showPMInfo();
        }

        private void btnSaveAndClose1_Click(object sender, EventArgs e)
        {
            button1_Click(null, null);
            this.Close();
        }

        private void sb2_Scroll(object sender, ScrollEventArgs e)
        {

        }

        private void btnSaveAndClose2_Click(object sender, EventArgs e)
        {
            saveKeySettingToolStripMenuItem_Click(null, null);
            this.Close();
        }

        private void btnSave3_Click(object sender, EventArgs e)
        {
            if (tabControl.SelectedTab.Name == "tabBasic" || tabControl.SelectedTab.Name == "tabCurtain")
            {
                tbDown_Click(tbUpload, null);
            }
            else if (tabControl.SelectedTab.Name == "tabLight")
            {
                saveKeySettingToolStripMenuItem_Click(null, null);
            }
            else if (tabControl.SelectedTab.Name == "tabAC")
            {
                btnSaveAC_Click(null, null);
                btnSaveTemp_Click(null, null);
            }
            else if (tabControl.SelectedTab.Name == "tabHeat")
            {
                btnSaveHeating_Click(null, null);
            }
            else if (tabControl.SelectedTab.Name == "tabMusic")
            {
                btnSaveMusic_Click(null, null);
            }
            else if (tabControl.SelectedTab.Name == "tabScene")
            {
                saveScenesSettingToolStripMenuItem_Click(null, null);
            }
            else if (tabControl.SelectedTab.Name == "tabSensor")
            {
                btnSaveSensor_Click(null, null);
            }
        }


        private void btnSaveAndClose3_Click(object sender, EventArgs e)
        {
            btnSave3_Click(null, null);
            this.Close();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            btnSaveHeating_Click(null, null);
            this.Close();
        }

        private void btnSaveAndClose6_Click(object sender, EventArgs e)
        {
            btnSaveMusic_Click(null, null);
            this.Close();
        }

        private void btnSaveAndClose7_Click(object sender, EventArgs e)
        {
            saveScenesSettingToolStripMenuItem_Click(null, null);
            this.Close();
        }

        private void button2_Click_2(object sender, EventArgs e)
        {
            btnSaveSensor_Click(null, null);
            this.Close();
        }

        private void btnSave4_Click(object sender, EventArgs e)
        {
            tbDown_Click(tbUpload, null);
        }

        private void btnSaveAndClose4_Click(object sender, EventArgs e)
        {
            btnSave4_Click(null, null);
            this.Close();
        }

        private void toolStripButton10_Click(object sender, EventArgs e)
        {

        }

        private void cbMusicMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (myDLP == null) return;
            if (myDLP.MyMusic == null) return;
            if (myDLP.MyMusic.Count <= 0) return;
            if (isReadingData) return;
            myDLP.MyMusic[0].Type = Convert.ToByte(cbMusicMode.SelectedIndex);
        }

        private void rbPage1_CheckedChanged(object sender, EventArgs e)
        {
            lbPage.Visible = rbPage2.Checked;
            cbPage.Visible = rbPage2.Checked;
            lbDelay.Visible = rbPage2.Checked;
            tbDelay.Visible = rbPage2.Checked;
            ls2.Visible = rbPage2.Checked;
            if (myDLP == null) return;
            if (myDLP.MyMusic == null) return;
            if (myDLP.BasicInfo == null) myDLP.BasicInfo = new byte[20];
            if (rbPage1.Checked) myDLP.BasicInfo[7] = 0;
            else if (rbPage2.Checked)
            {
                cbPage_SelectedIndexChanged(null, null);
            }
        }

        private void cbPage_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbPage.SelectedText.Contains("---")) cbPage.SelectedIndex = cbPage.SelectedIndex - 1;
            if (cbPage.Text.Contains("---")) cbPage.SelectedIndex = cbPage.SelectedIndex - 1;
            if (myDLP == null) return;
            if (myDLP.MyMusic == null) return;
            if (myDLP.BasicInfo == null) myDLP.BasicInfo = new byte[20];
            switch (cbPage.SelectedIndex)
            {
                case 0: myDLP.BasicInfo[7] = 1; break;//主界面
                case 2: myDLP.BasicInfo[7] = 2; break;//待机界面
                case 4: myDLP.BasicInfo[7] = 10; break;//灯光主界面
                case 5:
                case 6:
                case 7:
                case 8:
                case 9:
                case 10:
                    myDLP.BasicInfo[7] = myDLP.BasicInfo[7] = Convert.ToByte(cbPage.SelectedIndex + 6); break;//灯光分页面
                case 12: myDLP.BasicInfo[7] = 21; break;//场景页一
                case 13: myDLP.BasicInfo[7] = 22; break;//场景页二
                case 15: myDLP.BasicInfo[7] = 30; break;//窗帘主界面
                case 16:
                case 17:
                case 18:
                case 19:
                case 20:
                case 21:
                    myDLP.BasicInfo[7] = Convert.ToByte(cbPage.SelectedIndex + 15); break;//窗帘分页面
                case 23: myDLP.BasicInfo[7] = 40; break;//空调主界面
                case 24:
                case 25:
                case 26:
                case 27:
                case 28:
                case 29:
                case 30:
                case 31:
                case 32:
                    myDLP.BasicInfo[7] = Convert.ToByte(cbPage.SelectedIndex + 17); break;//空调分页面
                case 34: myDLP.BasicInfo[7] = 50; break;//地热主界面
                case 35:
                case 36:
                case 37:
                case 38:
                case 39:
                case 40:
                case 41:
                case 42:
                case 43:
                    myDLP.BasicInfo[7] = Convert.ToByte(cbPage.SelectedIndex + 16); break;//地热分页面
                case 45: myDLP.BasicInfo[7] = 60; break;//音乐主界面
                case 46:
                case 47:
                case 48:
                case 49:
                case 50:
                case 51:
                case 52:
                case 53:
                case 54:
                    myDLP.BasicInfo[7] = Convert.ToByte(cbPage.SelectedIndex + 15); break;//音乐分页面
                case 56: myDLP.BasicInfo[7] = 70; break;//传感器主界面

            }
        }

        private void tbDelay_ValueChanged(object sender, EventArgs e)
        {
            if (myDLP == null) return;
            if (myDLP.MyMusic == null) return;
            if (myDLP.BasicInfo == null) myDLP.BasicInfo = new byte[20];
            myDLP.BasicInfo[8] = Convert.ToByte(tbDelay.Value);
        }

        private void cbSensitivity_SelectedIndexChanged(object sender, EventArgs e)
        {
            lb9.Text = cbSensitivity.Value.ToString() + "%";
            if (myDLP == null) return;
            if (myDLP.MyMusic == null) return;
            if (myDLP.BasicInfo == null) myDLP.BasicInfo = new byte[20];
            myDLP.BasicInfo[9] = Convert.ToByte(cbSensitivity.Value);
        }

        private void toolStripButton11_Click(object sender, EventArgs e)
        {

        }

        private void btnACAdv_Click(object sender, EventArgs e)
        {
            frmCmdSetup frmTemp = new frmCmdSetup(myDLP,myDevName, MyintDeviceType,null);
            frmTemp.ShowDialog();
        }

        private void grpsection_Enter(object sender, EventArgs e)
        {

        }

        private void btnCMD_Click(object sender, EventArgs e)
        {
            if (cbLight.SelectedIndex == -1) return;
            try
            {
                Byte[] PageID = new Byte[4];
                if (dgvLightKeys.SelectedRows != null && dgvLightKeys.SelectedRows.Count > 0)
                {
                    PageID[0] = (Byte)dgvLightKeys.SelectedRows[0].Index;
                }
                PageID[1] = Convert.ToByte(cbLight.SelectedIndex);
                frmCmdSetup CmdSetup = new frmCmdSetup(myDLP, myDevName, MyintDeviceType, PageID);
                CmdSetup.ShowDialog();
            }
            catch { }
        }

    }
}
