using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Data.OleDb;

namespace HDL_Buspro_Setup_Tool
{
    public partial class frm12in1 : Form
    {
        [DllImport("user32.dll")]
        public static extern Boolean FlashWindow(IntPtr hwnd, Boolean bInvert);
        private Sensor_12in1 mySensor = null;
        private string myDevName = null;
        private int mintIDIndex = -1;
        private int mywdDevicerType = -1;
        private int MyActivePage = 0; //按页面上传下载
        private bool isRead = false;
        private BackgroundWorker MyBackGroup;
        private bool isStopDownloadCodes = false;
        private SendIR tempSend = new SendIR();
        private byte SubNetID;
        private byte DeviceID;
        private int mySelectedLNo = 0; //用于存储当前选中的逻辑块
        public frm12in1()
        {
            InitializeComponent();
        }

        public frm12in1(Sensor_12in1 mysensor, string strName, int intDIndex, int intDeviceType)
        {
            InitializeComponent();
            this.mySensor = mysensor;
            this.myDevName = strName;
            this.mintIDIndex = intDIndex;
            string strDevName = strName.Split('\\')[0].ToString();
            this.mywdDevicerType = intDeviceType;

            cboDevice.Text = strName;
            SubNetID = Convert.ToByte(strDevName.Split('-')[0]);
            DeviceID = Convert.ToByte(strDevName.Split('-')[1]);
            System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = false;

            HDLSysPF.DisplayDeviceNameModeDescription(strName, mywdDevicerType, cboDevice, tbModel, tbDescription);
            HDLSysPF.addIR(tvIR, tempSend, true);  // 添加已有的列表到窗体
            HDLSysPF.LoadButtonModeWithDifferentDeviceType(clK3, mywdDevicerType);
        }

        void InitialFormCtrlsTextOrItems()
        {
            checklistSensor.Items.Clear();
            checklistSensor.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00610", ""));
            checklistSensor.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00611", ""));
            checklistSensor.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00612", ""));
            checklistSensor.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00613", ""));
            checklistSensor.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00614", ""));
            checklistSensor.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00615", ""));
            checklistSensor.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00616", ""));
            checklistSensor.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00617", ""));
            checklistSensor.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00618", ""));

            cbParam1.Items.Clear();
            cbParam2.Items.Clear();
            for (int i = 1; i < 100; i++)
            {
                cbParam1.Items.Add((i / 100).ToString() + "." + string.Format("{0:D2}", (i % 100)));
                cbParam2.Items.Add((i / 100).ToString() + "." + string.Format("{0:D2}", (i % 100)));
            }
            cbParam1.Items.Add("1");
            cbParam2.Items.Add("1");
            cbCycle.Items.Clear();
            for (int i = 1; i <= 50; i++)
            {
                cbCycle.Items.Add((i / 10).ToString() + "." + (i % 10));
            }
            cbTemp1.Items.Clear();
            cbTemp2.Items.Clear();
            for (int i = 0; i < 80; i++)
            {
                cbTemp1.Items.Add((i - 20).ToString());
                cbTemp2.Items.Add((i - 20).ToString());
            }

            checklistLED.Items.Clear();
            checklistLED.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00600", ""));
            checklistLED.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00601", ""));

            cbIRSensor.Items.Clear();
            cbIRSensor.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99852", ""));
            cbIRSensor.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99853", ""));
            cbUL.Items.Clear();
            cbUL.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99854", ""));
            cbUL.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99855", ""));
            cbDry1.Items.Clear();
            cbDry2.Items.Clear();
            cbDry1.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99616", ""));
            cbDry1.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99617", ""));
            cbDry2.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99616", ""));
            cbDry2.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99617", ""));
            cbUV1.Items.Clear();
            cbUV2.Items.Clear();
            cbUV1.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99614", ""));
            cbUV1.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99615", ""));
            cbUV2.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99614", ""));
            cbUV2.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99615", ""));
            cbLogicStatu.Items.Clear();
            cbLogicStatu.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99856", ""));
            cbLogicStatu.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99857", ""));
            this.tvLogic.DrawNode += new DrawTreeNodeEventHandler(treeView_DrawNode);
            rb1.Checked = true;
        }


        private void frm12in1_Load(object sender, EventArgs e)
        {
            InitialFormCtrlsTextOrItems();
        }

        void SetCtrlsVisbleWithDifferentDeviceType()
        {
            toolStrip1.Visible = (CsConst.MyEditMode == 0);
        }

        private void treeView_DrawNode(object sender, DrawTreeNodeEventArgs e)
        {
            try
            {
                if ((e.State & TreeNodeStates.Selected) != 0)
                {
                    e.Graphics.FillRectangle(Brushes.Blue, e.Bounds.X - 5, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
                    Font nodeFont = e.Node.NodeFont;
                    if (nodeFont == null) nodeFont = ((TreeView)sender).Font;

                    // Draw the node text.
                    e.Graphics.DrawString(e.Node.Text, nodeFont, Brushes.White,
                        Rectangle.Inflate(new Rectangle(e.Bounds.X, e.Bounds.Y, e.Bounds.Width + 10, e.Bounds.Height), 5, -6));
                }
                else
                {
                    e.DrawDefault = false;
                    e.Graphics.DrawString(e.Node.Text, ((TreeView)sender).Font, Brushes.Black,
                        Rectangle.Inflate(new Rectangle(e.Bounds.X, e.Bounds.Y, e.Bounds.Width + 10, e.Bounds.Height), 5, -6));
                }
            }
            catch
            {
            }
        }

        private void tslRead_Click(object sender, EventArgs e)
        {
            byte bytTag = Convert.ToByte(((ToolStripButton)sender).Tag.ToString());
            bool blnShowMsg = (CsConst.MyEditMode != 1);
            if (HDLPF.UploadOrReadGoOn(this.Text, bytTag, blnShowMsg))
            {
                Cursor.Current = Cursors.WaitCursor;
                // SetVisableForDownOrUpload(false);
                // ReadDownLoadThread();  //增加线程，使得当前窗体的任何操作不被限制

                CsConst.MyUPload2DownLists = new List<byte[]>();

                string strName = myDevName.Split('\\')[0].ToString();
                byte bytSubID = byte.Parse(strName.Split('-')[0]);
                byte bytDeviceID = byte.Parse(strName.Split('-')[1]);
                int num1 = 0;
                int num2 = 0;
                if (tab12in1.SelectedTab.Name == "tabKeys")
                {
                    num1 = Convert.ToInt32(txtFrm.Text);
                    num2 = Convert.ToInt32(txtTo.Text);
                }
                else if (tab12in1.SelectedTab.Name == "tabPage2")
                {
                    num1 = cbPage.SelectedIndex * 8 + 1;
                    num2 = cbPage.SelectedIndex * 8 + 8;
                }
                byte[] ArayRelay = new byte[] { bytSubID, bytDeviceID, (byte)(mywdDevicerType / 256), (byte)(mywdDevicerType % 256), 
                    (byte)MyActivePage,(byte)(mintIDIndex / 256), (byte)(mintIDIndex % 256),(byte)num1,(byte)num2 };

                CsConst.MyUPload2DownLists.Add(ArayRelay);
                CsConst.MyUpload2Down = Convert.ToByte(((ToolStripButton)sender).Tag.ToString());
                FrmDownloadShow Frm = new FrmDownloadShow();
                Frm.FormClosing += new FormClosingEventHandler(UpdateDisplayInformationAccordingly);
                Frm.ShowDialog();
            }
        }


        void UpdateDisplayInformationAccordingly(object sender, FormClosingEventArgs e)
        {
            if (tab12in1.SelectedTab.Name == "tabKeys") showIRSenderInfo();
            else if (tab12in1.SelectedTab.Name == "tabPage2") showIRReceiveInfo();
            else if (tab12in1.SelectedTab.Name == "tabPage1") showSensorInfo();
            else if (tab12in1.SelectedTab.Name == "tabPage4") showLogicInfo();
            else if (tab12in1.SelectedTab.Name == "tabPage3") showSecurityInfo();
            else if (tab12in1.SelectedTab.Name == "tabPage5") showRelayInfo();
            else if (tab12in1.SelectedTab.Name == "tabPage6") showSimulateInfo();
            this.TopMost = true;
            this.WindowState = FormWindowState.Normal;
            this.Activate();
            this.TopMost = false;
        }

        private void frm12in1_Shown(object sender, EventArgs e)
        {
            SetCtrlsVisbleWithDifferentDeviceType();
            if (CsConst.MyEditMode == 1) //在线模式
            {
                MyActivePage = 1;
                tslRead_Click(tslRead, null);
                btnRefStatus_Click(null, null);
                btnBroadcast1_Click(null, null);
            }
        }

        private void tab12in1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tab12in1.SelectedTab.Name == "tabKeys") MyActivePage = 1;
            else if (tab12in1.SelectedTab.Name == "tabPage2")
            {
                isRead = true;
                if (cbPage.SelectedIndex < 0) cbPage.SelectedIndex = 0;
                MyActivePage = 2;
            }
            else if (tab12in1.SelectedTab.Name == "tabPage1") MyActivePage = 3;
            else if (tab12in1.SelectedTab.Name == "tabPage4") MyActivePage = 4;
            else if (tab12in1.SelectedTab.Name == "tabPage3") MyActivePage = 5;
            else if (tab12in1.SelectedTab.Name == "tabPage5") MyActivePage = 6;
            else if (tab12in1.SelectedTab.Name == "tabPage6") MyActivePage = 7;
            if (CsConst.MyEditMode == 1)
            {
                if (tab12in1.SelectedTab.Name != "tabKeys")
                {
                    if (mySensor.MyRead2UpFlags[MyActivePage - 1] == false)
                    {
                        tslRead_Click(tslRead, null);
                    }
                    else
                    {
                        UpdateDisplayInformationAccordingly(null,null);

                    }
                }
            }
        }

        private void showIRSenderInfo()
        {
            try
            {
                isRead = true;
                if (mySensor != null)
                {
                    if (mySensor.IRCodes != null)
                    {
                        dgvIR.Rows.Clear();
                        for (int i = 0; i < mySensor.IRCodes.Count; i++)
                        {
                            UVCMD.IRCode ir = mySensor.IRCodes[i];
                            string str = CsConst.WholeTextsList[1775].sDisplayName;
                            if (ir.Enable == 1) str = CsConst.mstrINIDefault.IniReadValue("public", "00042", "");
                            object[] obj = new object[] { ir.KeyID.ToString(), ir.Remark1.ToString(), str ,
                                   CsConst.mstrINIDefault.IniReadValue("public", "99810", "")};
                            dgvIR.Rows.Add(obj);
                        }
                        if (dgvIR.RowCount > 0)
                        {
                            dgvIR.Rows[0].Selected = true;
                        }
                    }
                }
                btnFree_Click(null, null);
            }
            catch
            {
            }
            isRead = false;
        }

        private void showIRReceiveInfo()
        {
            try
            {
                isRead = true;
                if (mySensor == null) return;
                if (mySensor.IrReceiver == null) return;
                dgvKey.Rows.Clear();
                for (int i = 0; i < mySensor.IrReceiver.Count; i++)
                {
                    Sensor_12in1.IRReceive temp = mySensor.IrReceiver[i];
                    string strMode = ButtonMode.ConvertorKeyModeToPublicModeGroup(temp.IRBtnModel);
                    if (!clK3.Items.Contains(strMode)) strMode = clK3.Items[0].ToString();
                    object[] obj = new object[] { dgvKey.RowCount + 1, temp.IRBtnRemark, strMode };
                    dgvKey.Rows.Add(obj);
                }
            }
            catch
            {
            }
            isRead = false;
        }

        private void showSensorInfo()
        {
            try
            {
                isRead = true;
                for (int i = 0; i < 2; i++)
                {
                    if (mySensor.ONLEDs[i] == 1) checklistLED.SetItemChecked(i, true);
                    else checklistLED.SetItemChecked(i, false);
                }
                for (int i = 0; i < 9; i++)
                {
                    if (mySensor.EnableSensors[i] == 1) checklistSensor.SetItemChecked(i, true);
                    else checklistSensor.SetItemChecked(i, false);
                }
                if (mySensor.ParamSensors[0] <= 128)
                    sbSensitivity4.Value = mySensor.ParamSensors[0];
                else
                    sbSensitivity4.Value = (mySensor.ParamSensors[0] - 128);
                if (mySensor.ParamSensors[1] <= 128)
                    sbSensitivity3.Value = mySensor.ParamSensors[1];
                else
                    sbSensitivity3.Value = (mySensor.ParamSensors[1] - 128);
                if (mySensor.ParamSensors[2] >= sbSensitivity1.Minimum && mySensor.ParamSensors[2] <= sbSensitivity1.Maximum)
                    sbSensitivity1.Value = mySensor.ParamSensors[2];
                if (mySensor.ParamSensors[3] >= sbSensitivity2.Minimum && mySensor.ParamSensors[3] <= sbSensitivity2.Maximum)
                    sbSensitivity2.Value = mySensor.ParamSensors[3];
                chbEnable.Checked = (mySensor.EnableBroads[0] == 1);
                txtLux.Text = (mySensor.EnableBroads[1] * 256 + mySensor.EnableBroads[2]).ToString();
                string str1=((mySensor.EnableBroads[3] * 256 + mySensor.EnableBroads[4]) / 100).ToString() + "."+
                            string.Format("{0:D2}", ((mySensor.EnableBroads[3] * 256 + mySensor.EnableBroads[4]) % 100));
                if (str1 == "1.00") str1 = "1";
                string str2 = ((mySensor.EnableBroads[5] * 256 + mySensor.EnableBroads[6]) / 100).ToString() + "." +
                            string.Format("{0:D2}", ((mySensor.EnableBroads[5] * 256 + mySensor.EnableBroads[6]) % 100));
                if (str2 == "1.00") str2 = "1";
                string str3 = (mySensor.EnableBroads[7] / 10).ToString() + "." + (mySensor.EnableBroads[7] % 10).ToString();
                cbParam1.SelectedIndex = cbParam1.Items.IndexOf(str1);
                cbParam2.SelectedIndex = cbParam2.Items.IndexOf(str2);
                cbCycle.SelectedIndex = cbCycle.Items.IndexOf(str3);
                if (mySensor.EnableBroads[8] <= sbLimit.Maximum)
                    sbLimit.Value = mySensor.EnableBroads[8];
                if (cbParam1.SelectedIndex < 0) cbParam1.SelectedIndex = 0;
                if (cbParam2.SelectedIndex < 0) cbParam2.SelectedIndex = 0;
                if (cbCycle.SelectedIndex < 0) cbCycle.SelectedIndex = 0;
            }
            catch
            {
            }
            isRead = false;
            sbSensitivity1_ValueChanged(null, null);
            sbSensitivity2_ValueChanged(null, null);
            sbSensitivity3_ValueChanged(null, null);
            sbSensitivity4_ValueChanged(null, null);
            sbLimit_ValueChanged(null, null);
        }

        private void showLogicInfo()
        {
            try
            {
                isRead = true;
                HDLSysPF.ShowSensorsKeysTargetsInformation(tvLogic, mySensor, 0); // 显示逻辑信息
                tvLogic.SelectedNode = tvLogic.Nodes[0];
                tvLogic_MouseDown(tvLogic, new MouseEventArgs(MouseButtons.Left, 1, 0, 0, 0));
                
            }
            catch
            {
            }
            isRead = false;
        }

        private void showSecurityInfo()
        {
            try
            {
                isRead = true;
                dgvSec.Rows.Clear();
                if (mySensor.fireset != null && mySensor.fireset.Count != 0)
                {
                    for (int i = 0; i < mySensor.fireset.Count; i++)
                    {
                        UVCMD.SecurityInfo reader = mySensor.fireset[i];
                        bool enable = false;
                        if (i < 2)
                        {
                            if (reader.bytTerms == 1) enable = true;
                        }
                        else
                        {
                            if (reader.bytTerms == 2) enable = true;
                        }
                        string strHint = CsConst.WholeTextsList[51].sDisplayName;
                        switch (i)
                        {
                            case 1: strHint = CsConst.WholeTextsList[50].sDisplayName; break;
                            case 2: strHint = CsConst.WholeTextsList[89].sDisplayName; break;
                            case 3: strHint = CsConst.WholeTextsList[91].sDisplayName; break;
                        }
                        object[] obj = new object[]{dgvSec.RowCount+1,strHint, enable,
                        reader.strRemark,reader.bytSubID.ToString(),reader.bytDevID.ToString(), reader.bytArea.ToString()};
                        dgvSec.Rows.Add(obj);
                    }
                }
                
            }
            catch
            {
            }
            isRead = false;
        }

        private void showRelayInfo()
        {
            try
            {
                isRead = true;
                if (mySensor == null) return;
                if (mySensor.MyRelays == null) return;
                txtRelay1.Text = mySensor.MyRelays[0].RelayName.ToString();
                if (mySensor.MyRelays[0].Setup[1] == 1) rbRelay2.Checked = true;
                else rbRelay1.Checked = true;
                numOpen1.Value = Convert.ToDecimal(mySensor.MyRelays[0].Setup[2] / 10);
                numOpen2.Value = Convert.ToDecimal(mySensor.MyRelays[0].Setup[2] % 10);
                if (mySensor.MyRelays[0].Setup[3] < 60) numProtect1.Value = Convert.ToDecimal(mySensor.MyRelays[0].Setup[3]);
                int num1 = mySensor.MyRelays[0].Setup[4] * 256 + mySensor.MyRelays[0].Setup[5];
                int num2 = mySensor.MyRelays[0].Setup[6] * 256 + mySensor.MyRelays[0].Setup[7];
                if (num1 > 3600) num1 = 3600;
                if (num2 > 3600) num2 = 3600;
                numON1.Value = Convert.ToDecimal(num1 / 60);
                numON2.Value = Convert.ToDecimal(num1 % 60);
                numOFF1.Value = Convert.ToDecimal(num2 / 60);
                numOFF2.Value = Convert.ToDecimal(num2 % 60);
                if (mySensor.MyRelays[0].Setup[8] == 100) lbRelayStatusValue1.Text = CsConst.Status[1];
                else lbRelayStatusValue1.Text = CsConst.Status[0];

                txtRelay2.Text = mySensor.MyRelays[0].RelayName.ToString();
                if (mySensor.MyRelays[1].Setup[1] == 1) rbRelay4.Checked = true;
                else rbRelay3.Checked = true;
                numOpen3.Value = Convert.ToDecimal(mySensor.MyRelays[1].Setup[2] / 10);
                numOpen4.Value = Convert.ToDecimal(mySensor.MyRelays[1].Setup[2] % 10);
                if (mySensor.MyRelays[1].Setup[3] < 60) numProtect2.Value = Convert.ToDecimal(mySensor.MyRelays[1].Setup[3]);
                num1 = mySensor.MyRelays[1].Setup[4] * 256 + mySensor.MyRelays[1].Setup[5];
                num2 = mySensor.MyRelays[1].Setup[6] * 256 + mySensor.MyRelays[1].Setup[7];
                if (num1 > 3600) num1 = 3600;
                if (num2 > 3600) num2 = 3600;
                numON3.Value = Convert.ToDecimal(num1 / 60);
                numON4.Value = Convert.ToDecimal(num1 % 60);
                numOFF3.Value = Convert.ToDecimal(num2 / 60);
                numOFF4.Value = Convert.ToDecimal(num2 % 60);
                if (mySensor.MyRelays[1].Setup[8] == 100) lbRelayStatusValue2.Text = CsConst.Status[1];
                else lbRelayStatusValue2.Text = CsConst.Status[0];
            }
            catch
            {
            }
            isRead = false;
        }

        private void showSimulateInfo()
        {
            try
            {
                isRead = true;
                if (mySensor.SimulateEnable[0] == 1) lbState.Text = CsConst.mstrINIDefault.IniReadValue("Public", "99619", "");
                else lbState.Text = CsConst.mstrINIDefault.IniReadValue("Public", "99618", "");

                if (mySensor.SimulateEnable[1] == 1) chb0.Checked = true;
                else chb0.Checked = false;
                if (mySensor.SimulateEnable[2] == 1) chb1.Checked = true;
                else chb1.Checked = false;
                if (mySensor.SimulateEnable[3] == 1) chb2.Checked = true;
                else chb2.Checked = false;
                if (mySensor.SimulateEnable[4] == 1) chb3.Checked = true;
                else chb3.Checked = false;
                if (mySensor.ParamSimulate[0] <= sb0.Maximum)
                    sb0.Value = mySensor.ParamSimulate[0];
                if ((mySensor.ParamSimulate[1] * 256 + mySensor.ParamSimulate[2]) < sb1.Value)
                sb1.Value = mySensor.ParamSimulate[1] * 256 + mySensor.ParamSimulate[2];
                if (mySensor.ParamSimulate[3] <= sb2.Maximum)
                    sb2.Value = mySensor.ParamSimulate[3];
                if (mySensor.ParamSimulate[4] <= sb3.Maximum)
                    sb3.Value = mySensor.ParamSimulate[4];
            }
            catch
            {
            }
            isRead = false;
            sb0_ValueChanged(null, null);
            sb1_ValueChanged(null, null);
            sb2_ValueChanged(null, null);
            sb3_ValueChanged(null, null);
        }

        private void txtFrm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar) || e.KeyChar == 8)
                e.Handled = false;
            else
                e.Handled = true;
        }

        private void tmRead_Click(object sender, EventArgs e)
        {
            if (isRead) return;
            tslRead_Click(tslRead, null);
        }

        private void btnLearner_Click(object sender, EventArgs e)
        {
            frmIRlearner frmTmp = new frmIRlearner();
            frmTmp.FormClosed += frmTmp_FormClosed;
            frmTmp.ShowDialog();
        }

        void frmTmp_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                HDLSysPF.addIR(tvIR, tempSend, true);  // 添加已有的列表到窗体
            }
            catch
            {
            }
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            try
            {
                isStopDownloadCodes = false;
                tsbar.Visible = true;
                lblProgressValue.Visible = true;
                MyBackGroup = new BackgroundWorker();
                MyBackGroup.DoWork += new DoWorkEventHandler(calculationWorker_DoWork);
                MyBackGroup.ProgressChanged += new ProgressChangedEventHandler(calculationWorker_ProgressChanged);
                MyBackGroup.WorkerReportsProgress = true;
                MyBackGroup.RunWorkerCompleted += new RunWorkerCompletedEventHandler(calculationWorker_RunWorkerCompleted);
                MyBackGroup.RunWorkerAsync();
                MyBackGroup.WorkerSupportsCancellation = true;
            }
            catch
            {
            }
            Cursor.Current = Cursors.Default;
        }

        void calculationWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                tsbar.Value = 100;
                lblProgressValue.Text = "100%";
                System.Threading.Thread.Sleep(1000);
                tsbar.Visible = false;
                lblProgressValue.Visible = false;
            }
            catch
            {
            }
        }

        void calculationWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //throw new NotImplementedException();
            try
            {
                this.tsbar.Value = e.ProgressPercentage;
                this.lblProgressValue.Text = e.ProgressPercentage.ToString() + "%";
            }
            catch
            {
            }
        }

        void calculationWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                #region
                if (tempSend.IRCodes == null) return;
                Cursor.Current = Cursors.WaitCursor;
                List<string> listCode = new List<string>();
                List<string> listRemark = new List<string>();
                int RowIndex = dgvIR.CurrentRow.Index;
                int RowCount = dgvIR.RowCount;
                for (int i = 0; i < tvIR.Nodes.Count; i++)
                {
                    if (tvIR.Nodes[i].Checked)
                    {
                        for (int j = 0; j < tvIR.Nodes[i].Nodes.Count; j++)
                        {
                            int DeviceID = Convert.ToInt32(tvIR.Nodes[i].Name);
                            int KeyID = Convert.ToInt32(tvIR.Nodes[i].Nodes[j].Name);
                            string strCodes = "";
                            for (int k = 0; k < tempSend.IRCodes.Count; k++)
                            {
                                if (tempSend.IRCodes[k].KeyID == KeyID && tempSend.IRCodes[k].IRLoation == DeviceID)
                                {
                                    strCodes = tempSend.IRCodes[k].Codes;
                                    break;
                                }
                            }
                            if (strCodes != "")
                            {
                                listCode.Add(strCodes);
                                listRemark.Add(tvIR.Nodes[i].Text.ToString() + "-" + tvIR.Nodes[i].Nodes[j].Text.ToString());
                            }
                        }
                    }
                    else
                    {
                        for (int j = 0; j < tvIR.Nodes[i].Nodes.Count; j++)
                        {
                            if (tvIR.Nodes[i].Nodes[j].Checked)
                            {
                                int DeviceID = Convert.ToInt32(tvIR.Nodes[i].Name);
                                int KeyID = Convert.ToInt32(tvIR.Nodes[i].Nodes[j].Name);
                                string strCodes = "";
                                for (int k = 0; k < tempSend.IRCodes.Count; k++)
                                {
                                    if (tempSend.IRCodes[k].KeyID == KeyID && tempSend.IRCodes[k].IRLoation == DeviceID)
                                    {
                                        strCodes = tempSend.IRCodes[k].Codes;
                                        break;
                                    }
                                }
                                if (strCodes != "")
                                {
                                    listCode.Add(strCodes);
                                    listRemark.Add(tvIR.Nodes[i].Text.ToString() + "-" + tvIR.Nodes[i].Nodes[j].Text.ToString());
                                }
                            }
                        }
                    }
                }
                int CodeIndex = 0;
                int FirstIndex = dgvIR.CurrentRow.Index;
                if (MyBackGroup != null && MyBackGroup.IsBusy) MyBackGroup.ReportProgress(0);
                for (int i = RowIndex; i < RowCount; i++)
                {
                    string strCodes = listCode[CodeIndex];
                    string[] ListData = new string[0];
                    if (strCodes.Contains(";"))
                        ListData = strCodes.Split(';');
                    else
                    {
                        ListData = new string[1];
                        ListData[0] = strCodes;
                    }
                    byte[] arayCode = new byte[ListData.Length * 16];
                    for (int j = 0; j < ListData.Length; j++)
                    {
                        byte[] arayCodeTmp = GlobalClass.HexToByte(ListData[j]);
                        Array.Copy(arayCodeTmp, 0, arayCode, 16 * j, 16);
                    }
                    byte[] arayTmp = new byte[3];
                    arayTmp[0] = Convert.ToByte(Convert.ToInt32(dgvIR[0, dgvIR.CurrentRow.Index].Value.ToString()) - 1);
                    arayTmp[1] = arayCode[2];
                    arayTmp[2] = arayCode[3];
                    if (CsConst.mySends.AddBufToSndList(arayTmp, 0x1692, SubNetID, DeviceID, false, true, true, false) == true)
                    {
                        if (CsConst.myRevBuf[25] == 0xF8)
                        {
                            CsConst.myRevBuf = new byte[1200];
                            for (int j = 0; j < ListData.Length; j++)
                            {
                                arayTmp = new byte[16];
                                Array.Copy(arayCode, j * 16, arayTmp, 0, 16);
                                if (CsConst.mySends.AddBufToSndList(arayTmp, 0x1694, SubNetID, DeviceID, false, true, true, false) == true)
                                {
                                    CsConst.myRevBuf = new byte[1200];
                                }
                                else return;
                                HDLUDP.TimeBetwnNext(20);
                            }
                            arayTmp = new byte[12];
                            arayTmp[0] = Convert.ToByte(Convert.ToInt32(dgvIR[0, dgvIR.CurrentRow.Index].Value.ToString()) - 1);
                            arayTmp[1] = 0;
                            string strRemark = listRemark[CodeIndex];
                            byte[] arayTmpRemark = HDLUDP.StringToByte(strRemark);
                            if (arayTmpRemark.Length >= 10)
                                Array.Copy(arayTmpRemark, 0, arayTmp, 2, 10);
                            else
                                Array.Copy(arayTmpRemark, 0, arayTmp, 2, arayTmpRemark.Length);
                            if (CsConst.mySends.AddBufToSndList(arayTmp, 0x1690, SubNetID, DeviceID, false, true, true, false) == true)
                            {
                                CsConst.myRevBuf = new byte[1200];
                            }
                            else return;
                            HDLUDP.TimeBetwnNext(arayTmp.Length);
                            arayTmp = new byte[12];
                            arayTmp[0] = Convert.ToByte(Convert.ToInt32(dgvIR[0, dgvIR.CurrentRow.Index].Value.ToString()) - 1);
                            arayTmp[1] = 1;
                            if (arayTmpRemark.Length >= 20)
                                Array.Copy(arayTmpRemark, 10, arayTmp, 2, 10);
                            else if (arayTmpRemark.Length > 10 && arayTmpRemark.Length < 20)
                                Array.Copy(arayTmpRemark, 10, arayTmp, 2, arayTmpRemark.Length - 10);
                            if (CsConst.mySends.AddBufToSndList(arayTmp, 0x1690, SubNetID, DeviceID, false, true, true, false) == true)
                            {
                                CsConst.myRevBuf = new byte[1200];
                            }
                            else return;
                            dgvIR.Rows[FirstIndex + CodeIndex].Cells[1].Value = strRemark;
                            dgvIR.Rows[FirstIndex + CodeIndex].Cells[2].Value = CsConst.mstrINIDefault.IniReadValue("public", "00042", "");
                            CodeIndex = CodeIndex + 1;
                            dgvIR.Rows[FirstIndex + CodeIndex].Selected = true;
                            this.dgvIR.CurrentCell = this.dgvIR.Rows[FirstIndex + CodeIndex].Cells[0];
                            if (isStopDownloadCodes) break;
                            if (CodeIndex >= listCode.Count) break;
                        }
                    }
                    else break;
                    if (MyBackGroup != null && MyBackGroup.IsBusy) MyBackGroup.ReportProgress(i * 90 / RowCount);
                }
                if (btnFree.Visible)
                    btnFree_Click(null, null);
                #endregion
            }
            catch
            {
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            try
            {
                isStopDownloadCodes = true;
                if (MyBackGroup != null && MyBackGroup.IsBusy) MyBackGroup.CancelAsync();
            }
            catch
            {
            }
        }

        private void btnFree_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            byte[] arayTmp = new byte[0];
            if (CsConst.mySends.AddBufToSndList(arayTmp, 0x1696, SubNetID, DeviceID, false, true, true, false) == true)
            {
                double temp = (double)(CsConst.myRevBuf[27] * 256 + CsConst.myRevBuf[28]) / (double)(CsConst.myRevBuf[25] * 256 + CsConst.myRevBuf[26]);
                lbFree.Text = string.Format("{0:P}", temp);
                CsConst.myRevBuf = new byte[1200];
            }
            Cursor.Current = Cursors.Default;
        }

        private void btnInitital_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            DialogResult result = MessageBox.Show(CsConst.mstrINIDefault.IniReadValue("public", "99805", ""), "", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (result == DialogResult.OK)
            {
                byte[] arayTmp = new byte[0];
                CsConst.replySpanTimes = 15000;
                if (CsConst.mySends.AddBufToSndList(arayTmp, 0x1698, SubNetID, DeviceID, false, true, true, false) == true)
                {
                    if (CsConst.myRevBuf[25] == 0xF8)
                    {
                        MessageBox.Show(CsConst.mstrINIDefault.IniReadValue("public", "99806", ""));
                        dgvIR.Rows.Clear();
                        lbFree.Text = "100%";
                    }
                    else if (CsConst.myRevBuf[25] == 0xF5)
                    {
                        MessageBox.Show(CsConst.mstrINIDefault.IniReadValue("public", "99807", ""));
                    }
                    CsConst.myRevBuf = new byte[1200];
                }
            }
            Cursor.Current = Cursors.Default;
            CsConst.replySpanTimes = 2000;
        }

        private void dgvIR_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            if (e.ColumnIndex == 3)
            {
                if (dgvIR[2, e.RowIndex].Value.ToString() == CsConst.WholeTextsList[1775].sDisplayName) return;
                byte Key = Convert.ToByte(Convert.ToInt32(dgvIR[0, e.RowIndex].Value.ToString()) - 1);
                byte[] arayTmp = new byte[1];
                arayTmp[0] = Key;
                if (CsConst.mySends.AddBufToSndList(arayTmp, 0x169A, SubNetID, DeviceID, false, true, true, false) == true)
                {
                    mySensor.IRCodes[e.RowIndex].Enable = 0;
                    mySensor.IRCodes[e.RowIndex].Codes = "";
                    mySensor.IRCodes[e.RowIndex].Remark1 = "";
                    dgvIR[1, e.RowIndex].Value = "";
                    dgvIR[2, e.RowIndex].Value = CsConst.WholeTextsList[1775].sDisplayName;
                    CsConst.myRevBuf = new byte[1200];
                }
            }
            Cursor.Current = Cursors.Default;
        }

        private void dgvIR_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            dgvIR.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void sbSensitivity4_ValueChanged(object sender, EventArgs e)
        {
            lbSensitivity4.Text = sbSensitivity4.Value.ToString() + "C";
            if (isRead) return;
            if (sbSensitivity4.Value >= 0)
                mySensor.ParamSensors[0] = Convert.ToByte(sbSensitivity4.Value);
            else
                mySensor.ParamSensors[0] = Convert.ToByte(128 + Math.Abs(sbSensitivity4.Value));
        }

        private void sbSensitivity3_ValueChanged(object sender, EventArgs e)
        {
            lbSensitivity3.Text = sbSensitivity3.Value.ToString() + "Lux";
            if (isRead) return;
            if (sbSensitivity3.Value >= 0)
                mySensor.ParamSensors[1] = Convert.ToByte(sbSensitivity3.Value);
            else
                mySensor.ParamSensors[1] = Convert.ToByte(128 + Math.Abs(sbSensitivity3.Value));
        }

        private void sbSensitivity1_ValueChanged(object sender, EventArgs e)
        {
            lbSensitivity1.Text = sbSensitivity1.Value.ToString() + "%";
            if (isRead) return;
            mySensor.ParamSensors[2] = Convert.ToByte(sbSensitivity1.Value);
        }

        private void sbSensitivity2_ValueChanged(object sender, EventArgs e)
        {
            lbSensitivity2.Text = sbSensitivity2.Value.ToString() + "%";
            if (isRead) return;
            mySensor.ParamSensors[3] = Convert.ToByte(sbSensitivity2.Value);
        }

        private void sbLimit_ValueChanged(object sender, EventArgs e)
        {
            lbLimitValue.Text = sbLimit.Value.ToString() + "%";
            if (isRead) return;
            mySensor.EnableBroads[8] = Convert.ToByte(sbLimit.Value);
        }

        private void dgvIR_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (mySensor == null) return;
            if (mySensor.IRCodes == null) return;
            if ((e.RowIndex == -1) || (e.ColumnIndex == -1)) return;
            if (dgvIR.SelectedRows.Count == 0) return;
            if (isRead) return;
            if (dgvIR[e.ColumnIndex, e.RowIndex].Value == null) dgvIR[e.ColumnIndex, e.RowIndex].Value = "";
            for (int i = 0; i < dgvIR.SelectedRows.Count; i++)
            {
                string strTmp = "";
                switch (e.ColumnIndex)
                {
                    case 1:
                        strTmp = dgvIR[1, dgvIR.SelectedRows[i].Index].Value.ToString();
                        mySensor.IRCodes[dgvIR.SelectedRows[i].Index].Remark1 = strTmp;
                        ModifyIrOneLineRemark(strTmp);
                        break;
                }
                dgvIR.SelectedRows[i].Cells[e.ColumnIndex].Value = dgvIR[e.ColumnIndex, e.RowIndex].Value.ToString();
            }
        }

        void ModifyIrOneLineRemark(String strRemark)
        {
            Cursor.Current = Cursors.WaitCursor;
            try
            {                
                byte[] arayRemark = new byte[20];
                byte[] arayTmp2 = HDLUDP.StringToByte(strRemark);
                if (arayTmp2.Length > 20)
                {
                    Array.Copy(arayTmp2, 0, arayRemark, 0, 20);
                }
                else
                {
                    Array.Copy(arayTmp2, 0, arayRemark, 0, arayTmp2.Length);
                }
                byte[] araySendIR = new byte[12];
                araySendIR[0] = Convert.ToByte(Convert.ToInt32(dgvIR[0, dgvIR.CurrentRow.Index].Value.ToString()) - 1);
                araySendIR[1] = 0;
                for (int K = 0; K <= 9; K++) araySendIR[2 + K] = arayRemark[K];
                if (CsConst.mySends.AddBufToSndList(araySendIR, 0x1690, SubNetID, DeviceID, false, true, true, false) == true)
                {
                    HDLUDP.TimeBetwnNext(araySendIR.Length);
                    CsConst.myRevBuf = new byte[1200];
                }

                araySendIR[1] = 1;    //save the remark then
                for (int K = 0; K <= 9; K++) araySendIR[2 + K] = arayRemark[10 + K];

                if (CsConst.mySends.AddBufToSndList(araySendIR, 0x1690, SubNetID, DeviceID, false, true, true, false) == true)
                {
                    HDLUDP.TimeBetwnNext(araySendIR.Length);
                    CsConst.myRevBuf = new byte[1200];
                }
            }
            catch
            { }
            Cursor.Current = Cursors.Default;
        }

        private void rbRelay1_CheckedChanged(object sender, EventArgs e)
        {
            if (rbRelay1.Checked)
            {
                panel27.Visible = true;
                panel28.Visible = false;
            }
            else if (rbRelay2.Checked)
            {
                panel27.Visible = false;
                panel28.Visible = true;
            }
            if (isRead) return;
            if (mySensor == null) return;
            if (mySensor.MyRelays == null) return;
            if (rbRelay2.Checked) mySensor.MyRelays[0].Setup[1] = 1;
            else mySensor.MyRelays[0].Setup[1] = 0;
        }

        private void rbRelay3_CheckedChanged(object sender, EventArgs e)
        {
            if (rbRelay3.Checked)
            {
                panel29.Visible = true;
                panel30.Visible = false;
            }
            else if (rbRelay4.Checked)
            {
                panel29.Visible = false;
                panel30.Visible = true;
            }
            if (isRead) return;
            if (mySensor == null) return;
            if (mySensor.MyRelays == null) return;
            if (isRead) return;
            if (mySensor == null) return;
            if (mySensor.MyRelays == null) return;
            if (rbRelay4.Checked) mySensor.MyRelays[1].Setup[1] = 1;
            else mySensor.MyRelays[1].Setup[1] = 0;
        }

        private void numOpen1_ValueChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt32(numOpen1.Value) == 25) numOpen2.Value = 0;
            if (Convert.ToInt32(numOpen3.Value) == 25) numOpen4.Value = 0;
            if (isRead) return;
            if (mySensor == null) return;
            if (mySensor.MyRelays == null) return;
            int num1 = Convert.ToInt32(numOpen1.Value) * 10 + Convert.ToInt32(numOpen2.Value);
            mySensor.MyRelays[0].Setup[2] = Convert.ToByte(num1);
            int num2 = Convert.ToInt32(numOpen3.Value) * 10 + Convert.ToInt32(numOpen4.Value);
            mySensor.MyRelays[1].Setup[2] = Convert.ToByte(num2);
        }

        private void numON1_ValueChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt32(numON1.Value) == 60) numON2.Value = 0;
            if (Convert.ToInt32(numOFF1.Value) == 60) numOFF2.Value = 0;
            if (Convert.ToInt32(numON3.Value) == 60) numON4.Value = 0;
            if (Convert.ToInt32(numOFF3.Value) == 60) numOFF4.Value = 0;
            if (isRead) return;
            if (mySensor == null) return;
            if (mySensor.MyRelays == null) return;
            int num1 = Convert.ToInt32(numON1.Value) * 60 + Convert.ToInt32(numON2.Value);
            mySensor.MyRelays[0].Setup[4] = Convert.ToByte(num1 / 256);
            mySensor.MyRelays[0].Setup[5] = Convert.ToByte(num1 % 256);
            int num2 = Convert.ToInt32(numOFF1.Value) * 60 + Convert.ToInt32(numOFF2.Value);
            mySensor.MyRelays[0].Setup[6] = Convert.ToByte(num2 / 256);
            mySensor.MyRelays[0].Setup[7] = Convert.ToByte(num2 % 256);

            int num3 = Convert.ToInt32(numON3.Value) * 60 + Convert.ToInt32(numON4.Value);
            mySensor.MyRelays[1].Setup[4] = Convert.ToByte(num3 / 256);
            mySensor.MyRelays[1].Setup[5] = Convert.ToByte(num3 % 256);
            int num4 = Convert.ToInt32(numOFF3.Value) * 60 + Convert.ToInt32(numOFF4.Value);
            mySensor.MyRelays[1].Setup[6] = Convert.ToByte(num4 / 256);
            mySensor.MyRelays[1].Setup[7] = Convert.ToByte(num4 % 256);
        }

        private void sb0_ValueChanged(object sender, EventArgs e)
        {
            lb0.Text = (sb0.Value - 20).ToString() + "C";
        }

        private void sb1_ValueChanged(object sender, EventArgs e)
        {
            lb1.Text = sb1.Value.ToString() + "Lux";
            if (isRead) return;
            mySensor.ParamSimulate[1] = Convert.ToByte(sb1.Value / 256);
            mySensor.ParamSimulate[2] = Convert.ToByte(sb1.Value % 256);
        }

        private void sb2_ValueChanged(object sender, EventArgs e)
        {
            if (sb2.Value == 1) lb2.Text = CsConst.mstrINIDefault.IniReadValue("Public", "99849", "");
            else lb2.Text = CsConst.mstrINIDefault.IniReadValue("Public", "99848", "");
            if (isRead) return;
            mySensor.ParamSimulate[3] = Convert.ToByte(sb2.Value);
        }

        private void sb3_ValueChanged(object sender, EventArgs e)
        {
            if (sb3.Value == 1) lb3.Text = CsConst.mstrINIDefault.IniReadValue("Public", "99851", "");
            else lb3.Text = CsConst.mstrINIDefault.IniReadValue("Public", "99850", "");
            if (isRead) return;
            mySensor.ParamSimulate[4] = Convert.ToByte(sb3.Value);
        }

        private void timerSensor_Tick(object sender, EventArgs e)
        {
            if (isRead) return;
            byte[] ArayTmp = new byte[0];
            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1645, SubNetID, DeviceID, false, false, true,false) == true)
            {
                lbTempValue.Text = (CsConst.myRevBuf[26] - 20).ToString() + "C";
                lbStatus6.Text = CsConst.myRevBuf[27] * 256 + CsConst.myRevBuf[28] + "Lux";
                if (CsConst.myRevBuf[29] == 1) lbStatus4.Text = CsConst.mstrINIDefault.IniReadValue("Public", "99849", "");
                else lbStatus4.Text = CsConst.mstrINIDefault.IniReadValue("Public", "99848", "");
                if (CsConst.myRevBuf[30] == 1) lbStatus5.Text = CsConst.mstrINIDefault.IniReadValue("Public", "99851", "");
                else lbStatus5.Text = CsConst.mstrINIDefault.IniReadValue("Public", "99850", "");
                if (CsConst.myRevBuf[31] == 1) lbStatus7.Text = CsConst.mstrINIDefault.IniReadValue("Public", "99655", "");
                else lbStatus7.Text = CsConst.mstrINIDefault.IniReadValue("Public", "99605", "");
                if (CsConst.myRevBuf[32] == 1) lbStatus8.Text = CsConst.mstrINIDefault.IniReadValue("Public", "99655", "");
                else lbStatus8.Text = CsConst.mstrINIDefault.IniReadValue("Public", "99605", "");
            }
        }

        private void txtUVID1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (isRead) return;
                if (tvLogic.SelectedNode == null) return;
                if (mySensor.logic[mySelectedLNo].UV1 == null) mySensor.logic[mySelectedLNo].UV1 = new Sensor_12in1.UVSet();
                string str = txtUVID1.Text.Trim();
                if (str == "") return;
                if (str.Length >= 3)
                {
                    txtUVID1.Text = HDLPF.IsNumStringMode(str, 201, 248);
                    if (checkboxUV2.Checked && txtUVID1.Text == txtUVID2.Text)
                    {
                        if (txtUVID2.Text == "201") txtUVID1.Text = "202";
                        else txtUVID1.Text = (Convert.ToInt32(txtUVID2.Text) - 1).ToString();
                    }
                    txtUVID1.SelectionStart = txtUVID1.Text.Length;
                    mySensor.logic[mySelectedLNo].UV1.UvNum = byte.Parse(txtUVID1.Text);
                    if (CsConst.MyEditMode == 1)
                    {
                        Cursor.Current = Cursors.WaitCursor;
                        isRead = true;
                        byte[] arayTmp = new byte[1];
                        arayTmp[0] = Convert.ToByte(txtUVID1.Text);
                        if ((CsConst.mySends.AddBufToSndList(arayTmp, 0x164C, SubNetID, DeviceID, false, false, true, false) == true))
                        {
                            byte[] arayRemark = new byte[20];
                            Array.Copy(CsConst.myRevBuf, 27, arayRemark, 0, arayRemark.Length);
                            mySensor.logic[mySelectedLNo].UV1.UvRemark = HDLPF.Byte2String(arayRemark);
                            txtUVRemark1.Text = mySensor.logic[mySelectedLNo].UV1.UvRemark;
                            mySensor.logic[mySelectedLNo].UV1.AutoOff = (CsConst.myRevBuf[47] == 1);
                            checkAuto1.Checked = mySensor.logic[mySelectedLNo].UV1.AutoOff;
                            if (CsConst.myRevBuf[48] * 256 + CsConst.myRevBuf[49] > 3600)
                                mySensor.logic[mySelectedLNo].UV1.OffTime = 1;
                            else
                                mySensor.logic[mySelectedLNo].UV1.OffTime = CsConst.myRevBuf[48] * 256 + CsConst.myRevBuf[49];
                            txtUVAuto1.Text = mySensor.logic[mySelectedLNo].UV1.OffTime.ToString();
                            CsConst.myRevBuf = new byte[1200];
                        }
                        isRead = false;
                        if ((CsConst.mySends.AddBufToSndList(arayTmp, 0xE018, SubNetID, DeviceID, false, false, true, false) == true))
                        {
                            lbUV1.Visible = true;
                            if (CsConst.myRevBuf[26] == 0) lbUV1.Text = CsConst.mstrINIDefault.IniReadValue("Public", "99859", "") + txtUVID1.Text + " " +
                                                                        CsConst.Status[0];
                            else lbUV1.Text = CsConst.mstrINIDefault.IniReadValue("Public", "99859", "") + txtUVID1.Text + " " +
                                                                        CsConst.Status[0];
                            CsConst.myRevBuf = new byte[1200];
                        }
                        else
                        {
                            lbUV1.Visible = false;
                        }
                    }
                }
            }
            catch
            {
            }
            Cursor.Current = Cursors.Default;
        }

        private void tvLogic_MouseDown(object sender, MouseEventArgs e)
        {
            TreeNode oNode = tvLogic.GetNodeAt(e.Location);
            if (oNode == null) return;
            if (oNode.Level != 0)
            {
                mySelectedLNo = oNode.Parent.Index;
            }
            else
            {
                mySelectedLNo = oNode.Index;
            }
            if (mySelectedLNo == -1) return;
            DisplaySomeLogicInformation();
        }

        private void DisplaySomeLogicInformation()
        {
            if (mySelectedLNo == -1) return;
            if (mySensor == null) return;
            if (mySensor.logic == null) return;
            isRead = true;
            try
            {
                chbTemp.Enabled = (mySensor.EnableSensors[0] == 1);
                checkboxBright.Enabled = (mySensor.EnableSensors[1] == 1);
                chbIR.Enabled = (mySensor.EnableSensors[2] == 1);
                chbUL.Enabled = (mySensor.EnableSensors[3] == 1);
                chbDry1.Enabled = (mySensor.EnableSensors[4] == 1);
                chbDry2.Enabled = (mySensor.EnableSensors[5] == 1);
                checkboxUV1.Enabled = (mySensor.EnableSensors[6] == 1);
                checkboxUV2.Enabled = (mySensor.EnableSensors[7] == 1);
                CheckBoxLogic.Enabled = (mySensor.EnableSensors[8] == 1);
                Sensor_12in1.Logic oLogic = mySensor.logic[mySelectedLNo];
                chbLogicE.Checked = (oLogic.Enabled == 1);
                SetVisibleForLogic(chbLogicE.Checked);
                tbRemarkL.Text = oLogic.Remarklogic;//逻辑备注

                chbTemp.Checked = Convert.ToBoolean(oLogic.EnableSensors[0]);
                checkboxBright.Checked = Convert.ToBoolean(oLogic.EnableSensors[1]);
                chbIR.Checked = Convert.ToBoolean(oLogic.EnableSensors[2]);
                chbUL.Checked = Convert.ToBoolean(oLogic.EnableSensors[3]);
                chbDry1.Checked = Convert.ToBoolean(oLogic.EnableSensors[4]);
                chbDry2.Checked = Convert.ToBoolean(oLogic.EnableSensors[5]);
                checkboxUV1.Checked = Convert.ToBoolean(oLogic.EnableSensors[6]);
                checkboxUV2.Checked = Convert.ToBoolean(oLogic.EnableSensors[7]);
                CheckBoxLogic.Checked = Convert.ToBoolean(oLogic.EnableSensors[8]);

                if (oLogic.bytRelation == 0) rbOr.Checked = true;
                else if (oLogic.bytRelation == 1) rbAnd.Checked = true;
                cbTemp1.SelectedIndex = cbTemp1.Items.IndexOf((oLogic.Paramters[0] - 20).ToString());
                cbTemp2.SelectedIndex = cbTemp1.Items.IndexOf((oLogic.Paramters[1] - 20).ToString());
                if (cbTemp1.SelectedIndex < 0) cbTemp1.SelectedIndex = 0;
                if (cbTemp2.SelectedIndex < 0) cbTemp2.SelectedIndex = cbTemp1.SelectedIndex;

                if ((0 <= Convert.ToDecimal(oLogic.Paramters[2] * 256 + oLogic.Paramters[3])) && (Convert.ToDecimal(oLogic.Paramters[2] * 256 + oLogic.Paramters[3]) <= 5000))
                    NumBr1.Value = Convert.ToDecimal(oLogic.Paramters[2] * 256 + oLogic.Paramters[3]);
                if ((0 <= Convert.ToDecimal(oLogic.Paramters[4] * 256 + oLogic.Paramters[5])) && (Convert.ToDecimal(oLogic.Paramters[4] * 256 + oLogic.Paramters[5]) <= 5000))
                    NumBr2.Value = Convert.ToDecimal(oLogic.Paramters[4] * 256 + oLogic.Paramters[5]);
                if (oLogic.Paramters[6] < 2)
                    cbIRSensor.Text = cbIRSensor.Items[oLogic.Paramters[6]].ToString();
                if (cbIRSensor.SelectedIndex < 0) cbIRSensor.SelectedIndex = 0;
                if (oLogic.Paramters[7] < 2)
                    cbUL.Text = cbUL.Items[oLogic.Paramters[7]].ToString();
                if (cbUL.SelectedIndex < 0) cbUL.SelectedIndex = 0;
                if (oLogic.Paramters[8] < 2)
                    cbDry1.Text = cbDry1.Items[oLogic.Paramters[8]].ToString();
                if (cbDry1.SelectedIndex < 0) cbDry1.SelectedIndex = 0;
                if (oLogic.Paramters[9] < 2)
                    cbDry2.Text = cbDry2.Items[oLogic.Paramters[9]].ToString();
                if (cbDry2.SelectedIndex < 0) cbDry2.SelectedIndex = 0;
                if (1 <= oLogic.Paramters[10] && oLogic.Paramters[10] <= 24)
                    cbLogicNum.SelectedIndex = oLogic.Paramters[10] - 1;
                if (cbLogicNum.SelectedIndex < 0) cbLogicNum.SelectedIndex = 0;
                if (oLogic.Paramters[11] < 2)
                    cbLogicStatu.SelectedIndex = oLogic.Paramters[11];
                if (cbLogicStatu.SelectedIndex < 0) cbLogicStatu.SelectedIndex = 0;
                if (oLogic.UV1 != null)
                {
                    txtUVID1.Text = oLogic.UV1.UvNum.ToString();
                    if (oLogic.UV1.UvRemark == null) oLogic.UV1.UvRemark = "";
                    txtUVRemark1.Text = oLogic.UV1.UvRemark.ToString();
                    if (oLogic.UV1.UVCondition < 2)
                        cbUV1.Text = cbUV1.Items[oLogic.UV1.UVCondition].ToString();
                    checkAuto1.Checked = oLogic.UV1.AutoOff;
                    txtUVAuto1.Text = oLogic.UV1.OffTime.ToString();
                    lbUV1.Visible = true;
                    if (oLogic.UV1.state == 0) lbUV1.Text = CsConst.mstrINIDefault.IniReadValue("Public", "99859", "") + txtUVID1.Text + " " +
                                                                CsConst.Status[0];
                    else lbUV1.Text = CsConst.mstrINIDefault.IniReadValue("Public", "99859", "") + txtUVID1.Text + " " +
                                                                CsConst.Status[0];
                }

                if (oLogic.UV2 != null)
                {
                    txtUVID2.Text = oLogic.UV2.UvNum.ToString();
                    if (oLogic.UV2.UvRemark == null) oLogic.UV2.UvRemark = "";
                    txtUVRemark2.Text = oLogic.UV2.UvRemark.ToString();
                    if (oLogic.UV2.UVCondition < 2)
                        cbUV2.Text = cbUV2.Items[oLogic.UV2.UVCondition].ToString();
                    checkAuto2.Checked = oLogic.UV2.AutoOff;
                    txtUVAuto2.Text = oLogic.UV2.OffTime.ToString();
                    lbUV2.Visible = true;
                    if (oLogic.UV1.state == 0) lbUV2.Text = CsConst.mstrINIDefault.IniReadValue("Public", "99859", "") + txtUVID2.Text + " " +
                                                                CsConst.Status[0];
                    else lbUV2.Text = CsConst.mstrINIDefault.IniReadValue("Public", "99859", "") + txtUVID2.Text + " " +
                                                                CsConst.Status[0];
                }
                truetime.Text = oLogic.DelayTimeT.ToString();
                falsetime.Text = oLogic.DelayTimeF.ToString();
            }
            catch
            {
            }
            isRead = false;
            txtUVID1_TextChanged(null, null);
            txtUVID2_TextChanged(null, null);
            if (chbLogicE.Checked)
            {
                chbTemp_CheckedChanged(chbTemp, null);
                chbTemp_CheckedChanged(checkboxBright, null);
                chbTemp_CheckedChanged(chbIR, null);
                chbTemp_CheckedChanged(chbUL, null);
                chbTemp_CheckedChanged(chbDry1, null);
                chbTemp_CheckedChanged(chbDry2, null);
                chbTemp_CheckedChanged(checkboxUV1, null);
                chbTemp_CheckedChanged(checkboxUV2, null);
                chbTemp_CheckedChanged(CheckBoxLogic, null);
            }
        }

        void SetVisibleForLogic(bool blnIsEnalbe)
        {
            tbRemarkL.Enabled = blnIsEnalbe;
            truetime.Enabled = blnIsEnalbe;
            falsetime.Enabled = blnIsEnalbe;
            p1.Enabled = blnIsEnalbe;
            p2.Enabled = blnIsEnalbe;
            p3.Enabled = blnIsEnalbe;
            p4.Enabled = blnIsEnalbe;
            p5.Enabled = blnIsEnalbe;
            p6.Enabled = blnIsEnalbe;
            p8.Enabled = blnIsEnalbe;
            p7.Enabled = blnIsEnalbe;
            p9.Enabled = blnIsEnalbe;
            rbAnd.Enabled = blnIsEnalbe;
            rbOr.Enabled = blnIsEnalbe;
        }

        private void chbLogicE_CheckedChanged(object sender, EventArgs e)
        {
            SetVisibleForLogic(chbLogicE.Checked);
            if (isRead) return;
            if (tvLogic.SelectedNode == null) return;
            mySensor.logic[mySelectedLNo].Enabled = Convert.ToByte(chbLogicE.Checked);
            if (chbLogicE.Checked)
            {
                if (mySensor.logic[mySelectedLNo].UV1 == null) mySensor.logic[mySelectedLNo].UV1 = new Sensor_12in1.UVSet();
                if (mySensor.logic[mySelectedLNo].UV2 == null) mySensor.logic[mySelectedLNo].UV2 = new Sensor_12in1.UVSet();
                tvLogic.SelectedNode.SelectedImageIndex = 6;
                tvLogic.SelectedNode.ImageIndex = 6;
            }
            else
            {
                tvLogic.SelectedNode.SelectedImageIndex = 5;
                tvLogic.SelectedNode.ImageIndex = 5;
            }
        }

        private void chbTemp_CheckedChanged(object sender, EventArgs e)
        {
            int Tag = Convert.ToInt32((sender as CheckBox).Tag);
            if (Tag == 0)
            {
                cbTemp1.Enabled = ((sender as CheckBox).Checked && (sender as CheckBox).Enabled);
                cbTemp2.Enabled = ((sender as CheckBox).Checked && (sender as CheckBox).Enabled);
                if (isRead) return;
                if (tvLogic.SelectedNode == null) return;
                mySensor.logic[mySelectedLNo].EnableSensors[0] = Convert.ToByte((sender as CheckBox).Checked);
            }
            else if (Tag == 1)
            {
                NumBr1.Enabled = ((sender as CheckBox).Checked && (sender as CheckBox).Enabled);
                NumBr2.Enabled = ((sender as CheckBox).Checked && (sender as CheckBox).Enabled);
                if (isRead) return;
                if (tvLogic.SelectedNode == null) return;
                mySensor.logic[mySelectedLNo].EnableSensors[1] = Convert.ToByte((sender as CheckBox).Checked);
            }
            else if (Tag == 2)
            {
                cbIRSensor.Enabled = ((sender as CheckBox).Checked && (sender as CheckBox).Enabled);
                if (isRead) return;
                if (tvLogic.SelectedNode == null) return;
                mySensor.logic[mySelectedLNo].EnableSensors[2] = Convert.ToByte((sender as CheckBox).Checked);
            }
            else if (Tag == 3)
            {
                cbUL.Enabled = ((sender as CheckBox).Checked && (sender as CheckBox).Enabled);
                if (isRead) return;
                if (tvLogic.SelectedNode == null) return;
                mySensor.logic[mySelectedLNo].EnableSensors[3] = Convert.ToByte((sender as CheckBox).Checked);
            }
            else if (Tag == 4)
            {
                cbDry1.Enabled = ((sender as CheckBox).Checked && (sender as CheckBox).Enabled);
                if (isRead) return;
                if (tvLogic.SelectedNode == null) return;
                mySensor.logic[mySelectedLNo].EnableSensors[4] = Convert.ToByte((sender as CheckBox).Checked);
            }
            else if (Tag == 5)
            {
                cbDry2.Enabled = ((sender as CheckBox).Checked && (sender as CheckBox).Enabled);
                if (isRead) return;
                if (tvLogic.SelectedNode == null) return;
                mySensor.logic[mySelectedLNo].EnableSensors[5] = Convert.ToByte((sender as CheckBox).Checked);
            }
            else if (Tag == 6)
            {
                cbUV1.Enabled = ((sender as CheckBox).Checked && (sender as CheckBox).Enabled);
                txtUVID1.Enabled = ((sender as CheckBox).Checked && (sender as CheckBox).Enabled);
                txtUVRemark1.Enabled = ((sender as CheckBox).Checked && (sender as CheckBox).Enabled);
                checkAuto1.Enabled = ((sender as CheckBox).Checked && (sender as CheckBox).Enabled);
                txtUVAuto1.Enabled = ((sender as CheckBox).Checked && (sender as CheckBox).Enabled);
                if (isRead) return;
                if (tvLogic.SelectedNode == null) return;
                mySensor.logic[mySelectedLNo].EnableSensors[6] = Convert.ToByte((sender as CheckBox).Checked);
            }
            else if (Tag == 7)
            {
                cbUV2.Enabled = ((sender as CheckBox).Checked && (sender as CheckBox).Enabled);
                txtUVID2.Enabled = ((sender as CheckBox).Checked && (sender as CheckBox).Enabled);
                txtUVRemark2.Enabled = ((sender as CheckBox).Checked && (sender as CheckBox).Enabled);
                checkAuto2.Enabled = ((sender as CheckBox).Checked && (sender as CheckBox).Enabled);
                txtUVAuto2.Enabled = ((sender as CheckBox).Checked && (sender as CheckBox).Enabled);
                if (isRead) return;
                if (tvLogic.SelectedNode == null) return;
                mySensor.logic[mySelectedLNo].EnableSensors[7] = Convert.ToByte((sender as CheckBox).Checked);
            }
            else if (Tag == 8)
            {
                cbLogicNum.Enabled = ((sender as CheckBox).Checked && (sender as CheckBox).Enabled);
                cbLogicStatu.Enabled = ((sender as CheckBox).Checked && (sender as CheckBox).Enabled);
                if (isRead) return;
                if (tvLogic.SelectedNode == null) return;
                mySensor.logic[mySelectedLNo].EnableSensors[8] = Convert.ToByte((sender as CheckBox).Checked);
            }
        }

        private void tbRemarkL_TextChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            if (tvLogic.SelectedNode == null) return;
            mySensor.logic[mySelectedLNo].Remarklogic = tbRemarkL.Text;
            TreeNode node = tvLogic.SelectedNode;
            if (node.Level == 0)
            {
                node.Text = (node.Index + 1).ToString() + "-" + tbRemarkL.Text.Trim();
            }
        }

        private void NumBr1_ValueChanged(object sender, EventArgs e)
        {
            int num1 = Convert.ToInt32(NumBr1.Value);
            int num2 = Convert.ToInt32(NumBr2.Value);
            if (num2 < num1) NumBr2.Value = NumBr1.Value;
            if (isRead) return;
            if (tvLogic.SelectedNode == null) return;
            num1 = Convert.ToInt32(NumBr1.Value);
            mySensor.logic[mySelectedLNo].Paramters[2] = Convert.ToByte(num1 / 256);
            mySensor.logic[mySelectedLNo].Paramters[3] = Convert.ToByte(num1 % 256);
            num2 = Convert.ToInt32(NumBr2.Value);
            mySensor.logic[mySelectedLNo].Paramters[4] = Convert.ToByte(num2 / 256);
            mySensor.logic[mySelectedLNo].Paramters[5] = Convert.ToByte(num2 % 256);
        }

        private void cbTemp1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbTemp1.SelectedIndex >= 0 && cbTemp2.SelectedIndex >= 0)
            {
                int num1 = Convert.ToInt32(cbTemp1.Text);
                int num2 = Convert.ToInt32(cbTemp2.Text);
                if (num2 < num1) cbTemp2.SelectedIndex = cbTemp1.SelectedIndex;
            }
            if (isRead) return;
            if (tvLogic.SelectedNode == null) return;
            int Tmp1 = Convert.ToInt32(cbTemp1.Text);
            int Tmp2 = Convert.ToInt32(cbTemp2.Text);
            mySensor.logic[mySelectedLNo].Paramters[0] = Convert.ToByte(Tmp1 + 20);
            mySensor.logic[mySelectedLNo].Paramters[1] = Convert.ToByte(Tmp2 + 20);
        }

        private void cbIRSensor_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            if (tvLogic.SelectedNode == null) return;
            mySensor.logic[mySelectedLNo].Paramters[6] = Convert.ToByte(cbIRSensor.SelectedIndex);
        }

        private void cbUL_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            if (tvLogic.SelectedNode == null) return;
            mySensor.logic[mySelectedLNo].Paramters[7] = Convert.ToByte(cbUL.SelectedIndex);
        }

        private void cbDry1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            if (tvLogic.SelectedNode == null) return;
            mySensor.logic[mySelectedLNo].Paramters[8] = Convert.ToByte(cbDry1.SelectedIndex);
        }

        private void cbDry2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            if (tvLogic.SelectedNode == null) return;
            mySensor.logic[mySelectedLNo].Paramters[9] = Convert.ToByte(cbDry2.SelectedIndex);
        }

        private void cbLogicNum_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            if (tvLogic.SelectedNode == null) return;
            mySensor.logic[mySelectedLNo].Paramters[10] = Convert.ToByte(cbLogicNum.SelectedIndex + 1);
        }

        private void cbLogicStatu_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            if (tvLogic.SelectedNode == null) return;
            mySensor.logic[mySelectedLNo].Paramters[11] = Convert.ToByte(cbLogicStatu.SelectedIndex);
        }

        private void txtUVID2_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (isRead) return;
                if (tvLogic.SelectedNode == null) return;
                if (mySensor.logic[mySelectedLNo].UV2 == null) mySensor.logic[mySelectedLNo].UV2 = new Sensor_12in1.UVSet();
                string str = txtUVID2.Text.Trim();
                if (str == "") return;
                if (str.Length >= 3)
                {
                    txtUVID2.Text = HDLPF.IsNumStringMode(str, 201, 248);
                    if (checkboxUV1.Checked && txtUVID1.Text == txtUVID2.Text)
                    {
                        if (txtUVID1.Text == "248") txtUVID2.Text = "247";
                        else txtUVID2.Text = (Convert.ToInt32(txtUVID1.Text) + 1).ToString();
                    }
                    txtUVID2.SelectionStart = txtUVID2.Text.Length;
                    mySensor.logic[mySelectedLNo].UV2.UvNum = byte.Parse(txtUVID2.Text);
                    if (CsConst.MyEditMode == 1)
                    {
                        Cursor.Current = Cursors.WaitCursor;
                        isRead = true;
                        byte[] arayTmp = new byte[1];
                        arayTmp[0] = byte.Parse(txtUVID2.Text);
                        if ((CsConst.mySends.AddBufToSndList(arayTmp, 0x164C, SubNetID, DeviceID, false, false, true, false) == true))
                        {
                            byte[] arayRemark = new byte[20];
                            Array.Copy(CsConst.myRevBuf, 27, arayRemark, 0, arayRemark.Length);
                            mySensor.logic[mySelectedLNo].UV2.UvRemark = HDLPF.Byte2String(arayRemark);
                            txtUVRemark2.Text = mySensor.logic[mySelectedLNo].UV2.UvRemark;
                            mySensor.logic[mySelectedLNo].UV2.AutoOff = (CsConst.myRevBuf[47] == 1);
                            checkAuto2.Checked = mySensor.logic[mySelectedLNo].UV2.AutoOff;
                            if (CsConst.myRevBuf[48] * 256 + CsConst.myRevBuf[49] > 3600)
                                mySensor.logic[mySelectedLNo].UV2.OffTime = 1;
                            else
                                mySensor.logic[mySelectedLNo].UV2.OffTime = CsConst.myRevBuf[48] * 256 + CsConst.myRevBuf[49];
                            txtUVAuto2.Text = mySensor.logic[mySelectedLNo].UV2.OffTime.ToString();
                            CsConst.myRevBuf = new byte[1200];
                        }
                        isRead = false;
                        if ((CsConst.mySends.AddBufToSndList(arayTmp, 0xE018, SubNetID, DeviceID, false, false, true, CsConst.minAllWirelessDeviceType.Contains(mywdDevicerType)) == true))
                        {
                            lbUV2.Visible = true;
                            if (CsConst.myRevBuf[26] == 0) lbUV2.Text = CsConst.mstrINIDefault.IniReadValue("Public", "99859", "") + txtUVID2.Text + " " +
                                                                        CsConst.Status[0];
                            else lbUV2.Text = CsConst.mstrINIDefault.IniReadValue("Public", "99859", "") + txtUVID2.Text + " " +
                                                                        CsConst.Status[0];
                            CsConst.myRevBuf = new byte[1200];
                        }
                        else
                        {
                            lbUV2.Visible = false;
                        }
                    }
                }
            }
            catch
            {
            }
            Cursor.Current = Cursors.Default;
        }

        private void txtUVRemark1_TextChanged(object sender, EventArgs e)
        {
            if (isRead == true) return;
            if (tvLogic.SelectedNode == null) return;
            mySensor.logic[mySelectedLNo].UV1.UvRemark = txtUVRemark1.Text;
            mySensor.logic[mySelectedLNo].UV1.AutoOff = checkAuto1.Checked;
            if (txtUVAuto1.Text.Length > 0)
                mySensor.logic[mySelectedLNo].UV1.OffTime = Convert.ToInt32(txtUVAuto1.Text);
        }

        private void txtUVRemark2_TextChanged(object sender, EventArgs e)
        {
            if (isRead == true) return;
            if (tvLogic.SelectedNode == null) return;
            mySensor.logic[mySelectedLNo].UV2.UvRemark = txtUVRemark2.Text;
            mySensor.logic[mySelectedLNo].UV2.AutoOff = checkAuto2.Checked;
            if (txtUVAuto2.Text.Length > 0)
                mySensor.logic[mySelectedLNo].UV2.OffTime = Convert.ToInt32(txtUVAuto2.Text);
        }

        private void cbUV1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            if (tvLogic.SelectedNode == null) return;
            if (cbUV1.SelectedIndex >= 0)
                mySensor.logic[mySelectedLNo].UV1.UVCondition = Convert.ToByte(cbUV1.SelectedIndex);
            if (cbUV2.SelectedIndex >= 0)
                mySensor.logic[mySelectedLNo].UV2.UVCondition = Convert.ToByte(cbUV2.SelectedIndex);
        }

        private void truetime_TextChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            if (tvLogic.SelectedNode == null) return;
            mySensor.logic[mySelectedLNo].DelayTimeT = Convert.ToInt32(truetime.Text);
            mySensor.logic[mySelectedLNo].DelayTimeF = Convert.ToInt32(falsetime.Text);
        }

        private void dgvSec_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            dgvSec.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void dgvSec_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (mySensor == null) return;
                if (mySensor.fireset == null) return;
                if (e.RowIndex == -1) return;
                if (e.ColumnIndex == -1) return;
                if (dgvSec[e.ColumnIndex, e.RowIndex].Value == null) dgvSec[e.ColumnIndex, e.RowIndex].Value = "";
                for (int i = 0; i < dgvSec.SelectedRows.Count; i++)
                {
                    UVCMD.SecurityInfo tempfire = mySensor.fireset[dgvSec.SelectedRows[i].Index];
                    dgvSec.SelectedRows[i].Cells[e.ColumnIndex].Value = dgvSec[e.ColumnIndex, e.RowIndex].Value.ToString();
                    switch (e.ColumnIndex)
                    {
                        case 2:
                            if (dgvSec[2, dgvSec.SelectedRows[i].Index].Value.ToString().ToLower() == "true")
                            {
                                if (dgvSec.SelectedRows[i].Index < 2)
                                    tempfire.bytTerms = 1;
                                else
                                    tempfire.bytTerms = 2;
                            }
                            else
                            {
                                tempfire.bytTerms = 0;
                            }
                            break;

                        case 3:
                            if (dgvSec[3, dgvSec.SelectedRows[i].Index].Value == null)
                            {
                                dgvSec[3, dgvSec.SelectedRows[i].Index].Value = "";
                            }
                            string strTmp = dgvSec[3, dgvSec.SelectedRows[i].Index].Value.ToString();
                            tempfire.strRemark = strTmp;

                            break;

                        case 4:
                            strTmp = dgvSec[4, dgvSec.SelectedRows[i].Index].Value.ToString();
                            dgvSec[4, dgvSec.SelectedRows[i].Index].Value = HDLPF.IsNumStringMode(strTmp, 0, 254);
                            tempfire.bytSubID = byte.Parse(dgvSec[4, dgvSec.SelectedRows[i].Index].Value.ToString());
                            break;

                        case 5:
                            strTmp = dgvSec[5, dgvSec.SelectedRows[i].Index].Value.ToString();
                            dgvSec[5, dgvSec.SelectedRows[i].Index].Value = HDLPF.IsNumStringMode(strTmp, 0, 254);
                            tempfire.bytDevID = byte.Parse(dgvSec[5, dgvSec.SelectedRows[i].Index].Value.ToString());
                            break;

                        case 6:
                            strTmp = dgvSec[6, dgvSec.SelectedRows[i].Index].Value.ToString();
                            dgvSec[6, dgvSec.SelectedRows[i].Index].Value = HDLPF.IsNumStringMode(strTmp, 0, 254);
                            tempfire.bytArea = byte.Parse(dgvSec[6, dgvSec.SelectedRows[i].Index].Value.ToString());
                            break;
                    }
                }
                if (e.ColumnIndex == 2)
                {
                    if (dgvSec[2, e.RowIndex].Value.ToString().ToLower() == "true")
                    {
                        if (e.RowIndex < 2)
                            mySensor.fireset[e.RowIndex].bytTerms = 1;
                        else
                            mySensor.fireset[e.RowIndex].bytTerms = 2;
                    }
                    else
                    {
                        mySensor.fireset[e.RowIndex].bytTerms = 0;
                    }
                }
                if (e.ColumnIndex == 3)
                {
                    string strTmp = dgvSec[3, e.RowIndex].Value.ToString();
                    mySensor.fireset[e.RowIndex].strRemark = strTmp;
                }
                if (e.ColumnIndex == 4)
                {
                    string strTmp = dgvSec[4, e.RowIndex].Value.ToString();
                    dgvSec[4, e.RowIndex].Value = HDLPF.IsNumStringMode(strTmp, 0, 254);
                    mySensor.fireset[e.RowIndex].bytSubID = byte.Parse(dgvSec[4, e.RowIndex].Value.ToString());
                }
                if (e.ColumnIndex == 5)
                {
                    string strTmp = dgvSec[5, e.RowIndex].Value.ToString();
                    dgvSec[5, e.RowIndex].Value = HDLPF.IsNumStringMode(strTmp, 0, 254);
                    mySensor.fireset[e.RowIndex].bytDevID = byte.Parse(dgvSec[5, e.RowIndex].Value.ToString());
                }
                if (e.ColumnIndex == 6)
                {
                    string strTmp = dgvSec[6, e.RowIndex].Value.ToString();
                    dgvSec[6, e.RowIndex].Value = HDLPF.IsNumStringMode(strTmp, 0, 254);
                    mySensor.fireset[e.RowIndex].bytArea = byte.Parse(dgvSec[6, e.RowIndex].Value.ToString());
                }
            }
            catch
            {
            }
        }

        private void chbUpdata_CheckedChanged(object sender, EventArgs e)
        {
            timerSensor.Enabled = chbUpdata.Checked;
        }

        private void btnSure_Click(object sender, EventArgs e)
        {
            tslRead_Click(tslRead, null);
        }

        private void btnRefStatus_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            byte[] ArayTmp = new byte[0];
            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1645, SubNetID, DeviceID, false, false, true, false) == true)
            {
                lbTempValue.Text = (CsConst.myRevBuf[26] - 20).ToString() + "C";
                lbStatus6.Text = CsConst.myRevBuf[27] * 256 + CsConst.myRevBuf[28] + "Lux";
                if (CsConst.myRevBuf[29] == 1) lbStatus4.Text = CsConst.mstrINIDefault.IniReadValue("Public", "99849", "");
                else lbStatus4.Text = CsConst.mstrINIDefault.IniReadValue("Public", "99848", "");
                if (CsConst.myRevBuf[30] == 1) lbStatus5.Text = CsConst.mstrINIDefault.IniReadValue("Public", "99851", "");
                else lbStatus5.Text = CsConst.mstrINIDefault.IniReadValue("Public", "99850", "");
                if (CsConst.myRevBuf[31] == 1) lbStatus7.Text = CsConst.mstrINIDefault.IniReadValue("Public", "99605", "");
                else lbStatus7.Text = CsConst.mstrINIDefault.IniReadValue("Public", "99655", "");
                if (CsConst.myRevBuf[32] == 1) lbStatus8.Text = CsConst.mstrINIDefault.IniReadValue("Public", "99605", "");
                else lbStatus8.Text = CsConst.mstrINIDefault.IniReadValue("Public", "99655", "");
            }
            Cursor.Current = Cursors.Default;
        }

        private void btnBroadcast1_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            byte[] ArayTmp = new byte[0];
            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x16B3, SubNetID, DeviceID, false, false, true, false) == true)
            {
                chbBroadcast.Checked = (CsConst.myRevBuf[26] == 1);
                CsConst.myRevBuf = new byte[1200];
            }
            Cursor.Current = Cursors.Default;
        }

        private void btnBroadcast2_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            byte[] ArayTmp = new byte[1];
            if (chbBroadcast.Checked) ArayTmp[0] = 1;
            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x16B5, SubNetID, DeviceID, false, false, true, false) == true)
            {
                CsConst.myRevBuf = new byte[1200];
            }
            Cursor.Current = Cursors.Default;
        }

        private void btnTargets_Click(object sender, EventArgs e)
        {
            Byte[] PageID = new Byte[4];
            if (dgvKey.SelectedRows != null && dgvKey.SelectedRows.Count > 0)
            {
                PageID[0] = (Byte)dgvKey.SelectedRows[0].Index;
            }
            PageID[1] = Convert.ToByte(cbPage.SelectedIndex);
            PageID[2] = 1;
            frmCmdSetup CmdSetup = new frmCmdSetup(mySensor, myDevName, mywdDevicerType, PageID);
            CmdSetup.ShowDialog();
        }

        private void tvIR_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (e.Action == TreeViewAction.ByMouse)
            {
                if (e.Node.Checked)
                {
                    //选中节点选中状态之后，选中父节点的选中状态
                    HDLPF.setChildNodeCheckedState(e.Node, true);
                    if (e.Node.Parent != null)
                    {
                        bool isAllTrue = true;
                        for (int i = 0; i < e.Node.Parent.Nodes.Count; i++)
                        {
                            if (e.Node.Parent.Nodes[i].Checked == false)
                            {
                                isAllTrue = false;
                                break;
                            }
                        }
                        if (isAllTrue) HDLPF.setParentNodeCheckedState(e.Node, true);
                    }
                }
                else
                {
                    //取消节点之后，取消节点的所有子节点
                    HDLPF.setChildNodeCheckedState(e.Node, false);
                    //如果节点存在父节点，取消父节点的选中状态
                    if (e.Node.Parent != null)
                    {
                        HDLPF.setParentNodeCheckedState(e.Node, false);
                    }
                }
            }
        }

        private void btnSave1_Click(object sender, EventArgs e)
        {
            if (tab12in1.SelectedTab.Name == "tabPage4")
            {
                
                chbLogicE_CheckedChanged(null, null);
                chbTemp_CheckedChanged(chbTemp, null);
                cbTemp1_SelectedIndexChanged(null, null);
                chbTemp_CheckedChanged(checkboxBright, null);
                NumBr1_ValueChanged(null, null);
                chbTemp_CheckedChanged(chbIR, null);
                cbIRSensor_SelectedIndexChanged(null, null);
                chbTemp_CheckedChanged(chbUL, null);
                cbUL_SelectedIndexChanged(null, null);
                chbTemp_CheckedChanged(chbDry1, null);
                cbDry1_SelectedIndexChanged(null, null);
                chbTemp_CheckedChanged(chbDry2, null);
                cbDry2_SelectedIndexChanged(null, null);
                chbTemp_CheckedChanged(checkboxUV1, null);
                chbTemp_CheckedChanged(checkboxUV2, null);
                cbUV1_SelectedIndexChanged(null, null);
                chbTemp_CheckedChanged(CheckBoxLogic, null);
                cbLogicNum_SelectedIndexChanged(null, null);
                cbLogicStatu_SelectedIndexChanged(null, null);
                truetime_TextChanged(null, null);
                rbOr_CheckedChanged(null, null);
            }
            tslRead_Click(tbUpload, null);
        }

        private void btnSaveAndClose1_Click(object sender, EventArgs e)
        {
            tslRead_Click(tbUpload, null);
            this.Close();
        }

        private void dgvKey_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (mySensor == null) return;
                if (mySensor.IrReceiver == null) return;
                if ((e.RowIndex == -1) || (e.ColumnIndex == -1)) return;
                if (dgvKey.SelectedRows.Count == 0) return;
                if (isRead) return;
                if (dgvKey[e.ColumnIndex, e.RowIndex].Value == null) dgvKey[e.ColumnIndex, e.RowIndex].Value = "";
                for (int i = 0; i < dgvKey.SelectedRows.Count; i++)
                {
                    dgvKey.SelectedRows[i].Cells[e.ColumnIndex].Value = dgvKey[e.ColumnIndex, e.RowIndex].Value.ToString();
                    string strTmp = "";
                    switch (e.ColumnIndex)
                    {
                        case 1:
                            strTmp = dgvKey[1, dgvKey.SelectedRows[i].Index].Value.ToString();
                            mySensor.IrReceiver[dgvKey.SelectedRows[i].Index].IRBtnRemark = strTmp;
                            break;
                        case 2:
                            strTmp = dgvKey[2, dgvKey.SelectedRows[i].Index].Value.ToString();
                            int Mode = Convert.ToByte(clK3.Items.IndexOf(strTmp));
                            if (Mode == 0) mySensor.IrReceiver[dgvKey.SelectedRows[i].Index].IRBtnModel = 0;
                            else if (Mode == 1) mySensor.IrReceiver[dgvKey.SelectedRows[i].Index].IRBtnModel = 2;
                            else if (Mode == 2) mySensor.IrReceiver[dgvKey.SelectedRows[i].Index].IRBtnModel = 3;
                            else if (Mode == 3) mySensor.IrReceiver[dgvKey.SelectedRows[i].Index].IRBtnModel = 1;
                            else if (Mode == 4) mySensor.IrReceiver[dgvKey.SelectedRows[i].Index].IRBtnModel = 4;
                            else if (Mode == 5) mySensor.IrReceiver[dgvKey.SelectedRows[i].Index].IRBtnModel = 5;
                            else if (Mode == 6) mySensor.IrReceiver[dgvKey.SelectedRows[i].Index].IRBtnModel = 7;
                            break;
                    }
                }
                if (e.ColumnIndex == 1)
                {
                    string strTmp = dgvKey[1, e.RowIndex].Value.ToString();
                    mySensor.IrReceiver[e.RowIndex].IRBtnRemark = dgvKey[1, e.RowIndex].Value.ToString();
                }
                else if (e.ColumnIndex == 2)
                {
                    string strTmp = dgvKey[2, e.RowIndex].Value.ToString();
                    int Mode = Convert.ToByte(clK3.Items.IndexOf(strTmp));
                    if (Mode == 0) mySensor.IrReceiver[e.RowIndex].IRBtnModel = 0;
                    else if (Mode == 1) mySensor.IrReceiver[e.RowIndex].IRBtnModel = 2;
                    else if (Mode == 2) mySensor.IrReceiver[e.RowIndex].IRBtnModel = 3;
                    else if (Mode == 3) mySensor.IrReceiver[e.RowIndex].IRBtnModel = 1;
                    else if (Mode == 4) mySensor.IrReceiver[e.RowIndex].IRBtnModel = 4;
                    else if (Mode == 5) mySensor.IrReceiver[e.RowIndex].IRBtnModel = 5;
                    else if (Mode == 6) mySensor.IrReceiver[e.RowIndex].IRBtnModel = 7;
                }
            }
            catch
            {
            }
        }

        private void dgvKey_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            dgvKey.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void checklistLED_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (isRead) return;
            mySensor.ONLEDs[e.Index] = Convert.ToByte(e.NewValue);
        }

        private void checklistSensor_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (isRead) return;
            mySensor.EnableSensors[e.Index] = Convert.ToByte(e.NewValue);
        }

        private void chbEnable_CheckedChanged(object sender, EventArgs e)
        {
            lbLux.Enabled = chbEnable.Checked;
            txtLux.Enabled = chbEnable.Checked;
            lbCycle.Enabled = chbEnable.Checked;
            cbCycle.Enabled = chbEnable.Checked;
            panel19.Enabled = chbEnable.Checked;
            if (isRead) return;
            if(chbEnable.Checked)
                mySensor.EnableBroads[0] = 1;
            else
                mySensor.EnableBroads[0] = 0;
        }

        private void txtLux_TextChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            if (txtLux.Text.Length > 0)
            {
                int num = Convert.ToInt32(txtLux.Text);
                mySensor.EnableBroads[1] = Convert.ToByte(num / 256);
                mySensor.EnableBroads[2] = Convert.ToByte(num % 256);
            }
        }

        private void txtLux_Leave(object sender, EventArgs e)
        {
            string str = txtLux.Text;
            txtFrm.Text = HDLPF.IsNumStringMode(str, 0, 5000);
            txtFrm.SelectionStart = txtTo.Text.Length;
        }

        private void txtFrm_Leave(object sender, EventArgs e)
        {
            string str = txtTo.Text;
            int num = Convert.ToInt32(txtFrm.Text);
            txtFrm.Text = HDLPF.IsNumStringMode(str, 1, num);
            txtFrm.SelectionStart = txtTo.Text.Length;
        }

        private void txtTo_Leave(object sender, EventArgs e)
        {
            string str = txtTo.Text;
            int num = Convert.ToInt32(txtFrm.Text);
            txtTo.Text = HDLPF.IsNumStringMode(str, num, 240);
            txtTo.SelectionStart = txtTo.Text.Length;
        }

        private void cbCycle_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            string str = cbCycle.Text;
            int num = Convert.ToInt32(Convert.ToSingle(str) * 10);
            mySensor.EnableBroads[7] = Convert.ToByte(num);
        }

        private void cbParam1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            string str = cbParam1.Text;
            int num = Convert.ToInt32(Convert.ToSingle(str) * 100);
            mySensor.EnableBroads[3] = Convert.ToByte(num / 256);
            mySensor.EnableBroads[4] = Convert.ToByte(num % 256);
        }

        private void cbParam2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            string str = cbParam2.Text;
            int num = Convert.ToInt32(Convert.ToSingle(str) * 100);
            mySensor.EnableBroads[5] = Convert.ToByte(num / 256);
            mySensor.EnableBroads[6] = Convert.ToByte(num % 256);
        }

        private void txtUVRemark2_Leave(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            txtUVAuto2.Text = HDLPF.IsNumStringMode(txtUVAuto2.Text, 1, 3600);
            byte[] arayTmp = new byte[24];
            arayTmp[0] = Convert.ToByte(txtUVID2.Text);
            byte[] arayRemark = HDLUDP.StringToByte(txtUVRemark2.Text);
            if (arayRemark.Length <= 20)
                Array.Copy(arayRemark, 0, arayTmp, 1, arayRemark.Length);
            else
                Array.Copy(arayRemark, 0, arayTmp, 1, 20);
            if (checkAuto2.Checked)
                arayTmp[21] = 1;
            int num = Convert.ToInt32(txtUVAuto2.Text);
            arayTmp[22] = Convert.ToByte(num / 256);
            arayTmp[23] = Convert.ToByte(num % 256);
            if ((CsConst.mySends.AddBufToSndList(arayTmp, 0x164E, SubNetID, DeviceID, false, false, true, false) == true))
            {
                CsConst.myRevBuf = new byte[1200];
            }
            Cursor.Current = Cursors.Default;
        }

        private void txtUVRemark1_Leave(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            txtUVAuto1.Text = HDLPF.IsNumStringMode(txtUVAuto1.Text, 1, 3600);
            byte[] arayTmp = new byte[24];
            arayTmp[0] = Convert.ToByte(txtUVID1.Text);
            byte[] arayRemark = HDLUDP.StringToByte(txtUVRemark1.Text);
            if (arayRemark.Length <= 20)
                Array.Copy(arayRemark, 0, arayTmp, 1, arayRemark.Length);
            else
                Array.Copy(arayRemark, 0, arayTmp, 1, 20);
            if (checkAuto1.Checked)
                arayTmp[21] = 1;
            int num = Convert.ToInt32(txtUVAuto1.Text);
            arayTmp[22] = Convert.ToByte(num / 256);
            arayTmp[23] = Convert.ToByte(num % 256);
            if ((CsConst.mySends.AddBufToSndList(arayTmp, 0x164E, SubNetID, DeviceID, false, false, true, false) == true))
            {
                CsConst.myRevBuf = new byte[1200];
            }
            Cursor.Current = Cursors.Default;
        }

        private void rbOr_CheckedChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            if (tvLogic.SelectedNode == null) return;
            if (rbOr.Checked)
                mySensor.logic[mySelectedLNo].bytRelation = 0;
            else if(rbAnd.Checked)
                mySensor.logic[mySelectedLNo].bytRelation = 1;
        }

        private void txtRelay1_TextChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            if (mySensor == null) return;
            if (mySensor.MyRelays == null) return;
            mySensor.MyRelays[0].RelayName = txtRelay1.Text;
        }

        private void numProtect1_ValueChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            if (mySensor == null) return;
            if (mySensor.MyRelays == null) return;
            mySensor.MyRelays[0].Setup[3] = Convert.ToByte(numProtect1.Value); 
        }

        private void txtRelay2_TextChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            if (mySensor == null) return;
            if (mySensor.MyRelays == null) return;
            mySensor.MyRelays[1].RelayName = txtRelay2.Text;
        }

        private void numProtect2_ValueChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            if (mySensor == null) return;
            if (mySensor.MyRelays == null) return;
            mySensor.MyRelays[1].Setup[3] = Convert.ToByte(numProtect2.Value); 
        }

        private void btn1_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            (sender as Button).Enabled = false;
            int tag = Convert.ToInt32((sender as Button).Tag);
            byte[] ArayTmp = new byte[16];

            if (tag == 1)
            {
                ArayTmp[0] = 1;
                if (chb0.Checked) ArayTmp[1] = 1;
                if (chb1.Checked) ArayTmp[2] = 1;
                if (chb2.Checked) ArayTmp[3] = 1;
                if (chb3.Checked) ArayTmp[4] = 1;
                ArayTmp[5] = Convert.ToByte(sb0.Value + 20);
                ArayTmp[6] = Convert.ToByte(sb1.Value / 256);
                ArayTmp[7] = Convert.ToByte(sb1.Value % 256);
                ArayTmp[8] = Convert.ToByte(sb2.Value);
                ArayTmp[9] = Convert.ToByte(sb3.Value);

            }
            else if (tag == 0)
            {
                ArayTmp[10] = 20;
                ArayTmp[11] = 2;
                ArayTmp[12] = 2;
            }
            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1664, SubNetID, DeviceID, false, true, true, false) == true)
            {
                if (tag == 1)
                {
                    lbState.Text = CsConst.mstrINIDefault.IniReadValue("public", "99619", "");
                }
                else
                {
                    lbState.Text = CsConst.mstrINIDefault.IniReadValue("public", "99618", "");
                    chb0.Checked = false;
                    chb1.Checked = false;
                    chb2.Checked = false;
                    chb3.Checked = false;
                    sb0.Value = 0;
                    sb1.Value = 0;
                    sb2.Value = 0;
                    sb3.Value = 0;
                }
            }
            Cursor.Current = Cursors.Default;
            (sender as Button).Enabled = true;
        }

        private void btnTure_Click(object sender, EventArgs e)
        {
            Byte[] PageID = new Byte[4];
            if (tvLogic.SelectedNode != null && tvLogic.Nodes.Count > 0)
            {
                PageID[0] = (Byte)tvLogic.SelectedNode.Index;
            }
            frmCmdSetup CmdSetup = new frmCmdSetup(mySensor, myDevName, mywdDevicerType, PageID);
            CmdSetup.ShowDialog();
        }

        private void btnFalse_Click(object sender, EventArgs e)
        {
            
        }

        private void dgvKey_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            btnTargets_Click(null, null);
        }

    }
}
