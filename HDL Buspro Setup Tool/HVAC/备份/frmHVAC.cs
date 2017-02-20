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
    public partial class frmHVAC : Form
    {
        [DllImport("user32.dll")]
        public static extern Boolean FlashWindow(IntPtr hwnd, Boolean bInvert);

        private HVAC myHVAC = null;
        private int myintDIndex = -1;
        private string myDevName = null;
        private bool myIsLoading = false;
        private int mywdDeviceType = -1;
        private int MyActivePage = 0; //按页面上传下载
        
        public frmHVAC()
        {
            InitializeComponent();
        }


        public frmHVAC(HVAC oHVAC, string strName, int intDIndex, int intDeviceType)
        {
            InitializeComponent();

            this.myHVAC = oHVAC;
            this.myDevName = strName;
            this.myintDIndex = intDIndex;
            this.mywdDeviceType = intDeviceType;

            string strDevName = strName.Split('\\')[0].ToString();

            HDLSysPF.DisplayDeviceNameModeDescription(strName, mywdDeviceType, cboDevice, tbModel, tbDescription);
            this.Text = strName;
            tsl31.Text = strName;
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            if (myHVAC == null) return;
            Cursor.Current = Cursors.WaitCursor;
            myHVAC.SaveInfoToDb();

            //  LoadBasicInformationToForm();
            Cursor.Current = Cursors.Default;
        }


        private void DisplayHVACInformationToForm()
        {
            if (myHVAC == null) return;

            NumONDelay.Value = myHVAC.bytFanONDelay;
            NumOFFDelay.Value = myHVAC.bytFanOFFDelay;
            NumHigh.Value = myHVAC.bytHigh;
            NumMid.Value = myHVAC.bytMid;
            NumLow.Value = myHVAC.bytLow;

            NumONDelay1.Value = myHVAC.bytFanONDelay;
            NumOFFDelay1.Value = myHVAC.bytFanOFFDelay;
            NumHigh1.Value = myHVAC.bytHigh;
            NumMid1.Value = myHVAC.bytMid;
            NumLow1.Value = myHVAC.bytLow;

            if (myHVAC.bytModeONDelay > 127) myHVAC.bytModeONDelay =byte.Parse((myHVAC.bytModeONDelay - 127).ToString());
            NumON.Value = myHVAC.bytModeONDelay;
            NumOFF.Value = myHVAC.bytModeOFFDelay;

            if (tabControl1.Contains(tabBasic))
            {
                #region
                for (int intI = 0; intI < 3; intI++)
                {
                    ComboBox ocbo = this.Controls.Find("cbMode" + (intI + 1).ToString(), true)[0] as ComboBox;
                    ocbo.SelectedIndex = myHVAC.bytSteps[intI * 5];

                    NumericUpDown oNum = this.Controls.Find("Num" + (intI + 1).ToString() + "1", true)[0] as NumericUpDown;
                    oNum.Value = myHVAC.bytSteps[intI * 5 + 1];
                    oNum = this.Controls.Find("Num" + (intI + 1).ToString() + "2", true)[0] as NumericUpDown;
                    oNum.Value = myHVAC.bytSteps[intI * 5 + 2];

                    oNum = this.Controls.Find("Num" + (intI + 1).ToString() + "3", true)[0] as NumericUpDown;
                    oNum.Value = myHVAC.bytSteps[intI * 5 + 3];

                    oNum = this.Controls.Find("Num" + (intI + 1).ToString() + "4", true)[0] as NumericUpDown;
                    oNum.Value = myHVAC.bytSteps[intI * 5 + 4];
                }
                #endregion
            }
            //新的显示编辑方式
            if (tabControl1.Contains(tabNew))
            {
                #region
                cboWMode.SelectedIndex = myHVAC.bytSteps[140]; // 
                cboNMode.SelectedIndex = myHVAC.bytSteps[00]; // 表示简单控制 或者完全控制

                if (myHVAC.bytSteps[141] == 0)
                    rbIN.Checked = true;
                else
                {
                    rbBus.Checked = true;
                    for (int i = 0; i < 4; i++)
                    {
                        if ((myHVAC.bytSteps[143] & (1 << i)) == (1 << i))
                        {
                            ListViewItem tmp = new ListViewItem();
                            tmp.Checked = true;
                            tmp.SubItems.Add(myHVAC.bytSteps[144 + i * 3] + "-" + myHVAC.bytSteps[145 + i * 3] );
                            tmp.SubItems.Add(myHVAC.bytSteps[146 + i * 3].ToString());
                            lvSensor.Items.Add(tmp);
                        }
                    }
                }

                if (myHVAC.bytSteps[142] > 15 || myHVAC.bytSteps[142] < 5) myHVAC.bytSteps[142] = 10;
                tbGive.Value = myHVAC.bytSteps[142];
                tbGive_Scroll(tbGive, null);

                if (myHVAC.bytSteps[160] > 160 || myHVAC.bytSteps[160] < 3) myHVAC.bytSteps[160] = 3;
                NumRead.Value = myHVAC.bytSteps[160];

                cboNMode_SelectedIndexChanged(cboNMode, null);

                chbCom.Checked = (myHVAC.bytSteps[1] == 1);
                chbCom_CheckedChanged(chbCom, null);

                if (ComON.Maximum < myHVAC.bytSteps[2] || ComON.Minimum > myHVAC.bytSteps[2])
                {
                    myHVAC.bytSteps[2] = 1;
                }
                ComON.Value = myHVAC.bytSteps[2];
                if (ComOFF.Maximum < myHVAC.bytSteps[3] || ComOFF.Minimum > myHVAC.bytSteps[3])
                {
                    myHVAC.bytSteps[3] = 0;
                }
                ComOFF.Value = myHVAC.bytSteps[3];

                lvHVAC.Items.Clear();
                if (myHVAC.bytSteps[180] == 1 && myHVAC.bytSteps[181] == 1)
                {
                    for (int i = 0; i < 8; i++)
                    {
                        if ((myHVAC.bytSteps[182] & (1 << i)) == (1 << i))
                        {
                            ListViewItem tmp = new ListViewItem();
                            tmp.Checked = true;
                            tmp.SubItems.Add(myHVAC.bytSteps[183 + i * 2] + "-" + myHVAC.bytSteps[184 + i * 2]);
                            tmp.SubItems.Add("1");
                            lvHVAC.Items.Add(tmp);
                        }
                    }
                }
                #endregion
            }
        }

        private void frmHVAC_Load(object sender, EventArgs e)
        {
            InitialFormCtrlsTextOrItems();
        }


        void InitialFormCtrlsTextOrItems()
        {
            //myIsLoading = true;
            cbMode1.Items.Clear();
            cbMode2.Items.Clear();
            cbMode3.Items.Clear();
            cbMode1.Items.Add(CsConst.mstrINIDefault.IniReadValue("PublicControlMode", "00000", ""));
            cbMode1.Items.Add(CsConst.mstrINIDefault.IniReadValue("PublicACMode", "00000", ""));
            cbMode1.Items.Add(CsConst.mstrINIDefault.IniReadValue("PublicACMode", "00001", ""));
            cbMode1.Items.Add(CsConst.mstrINIDefault.IniReadValue("PublicACMode", "00004", ""));

            cbMode2.Items.Add(CsConst.mstrINIDefault.IniReadValue("PublicControlMode", "00000", ""));
            cbMode2.Items.Add(CsConst.mstrINIDefault.IniReadValue("PublicACMode", "00000", ""));
            cbMode2.Items.Add(CsConst.mstrINIDefault.IniReadValue("PublicACMode", "00001", ""));
            cbMode2.Items.Add(CsConst.mstrINIDefault.IniReadValue("PublicACMode", "00004", ""));

            cbMode3.Items.Add(CsConst.mstrINIDefault.IniReadValue("PublicControlMode", "00000", ""));
            cbMode3.Items.Add(CsConst.mstrINIDefault.IniReadValue("PublicACMode", "00000", ""));
            cbMode3.Items.Add(CsConst.mstrINIDefault.IniReadValue("PublicACMode", "00001", ""));
            cbMode3.Items.Add(CsConst.mstrINIDefault.IniReadValue("PublicACMode", "00004", ""));
        }

        void SetCtrlsVisbleWithDifferentDeviceType()
        {
            //HDLSysPF.addIR(tvExisted, tempSend, true);
            lbNMode.Visible = HVACModuleDeviceTypeList.HVACG2ComplexRelayDeviceTypeLists.Contains(mywdDeviceType);
            cboNMode.Visible = HVACModuleDeviceTypeList.HVACG2ComplexRelayDeviceTypeLists.Contains(mywdDeviceType);

            if (HVACModuleDeviceTypeList.HVACG1NormalRelayDeviceTypeLists.Contains(mywdDeviceType)) // generation 1
            {
                tabControl1.TabPages.Remove(tabNew);
            }
            else if (HVACModuleDeviceTypeList.HVACG2ComplexRelayDeviceTypeLists.Contains(mywdDeviceType)) // 复杂模式继电器
            {
                tabControl1.TabPages.Remove(tabBasic);
            }
        }

        private void UpdateBufferWhenchanged(int intType)
        {
            if (myHVAC == null) return;
            if (myIsLoading == true) return;

            switch (intType)
            {
                case 1: myHVAC.bytFanONDelay =Convert.ToByte(NumONDelay.Value); break;
                case 2: myHVAC.bytFanOFFDelay = Convert.ToByte(NumOFFDelay.Value); break;
                case 3: myHVAC.bytHigh = Convert.ToByte(NumHigh.Value); break;
                case 4: myHVAC.bytMid = Convert.ToByte(NumMid.Value); break;
                case 5: myHVAC.bytLow = Convert.ToByte(NumLow.Value); break;
                case 6: myHVAC.bytModeONDelay = Convert.ToByte(NumON.Value); break;
                case 7: myHVAC.bytModeOFFDelay = Convert.ToByte(NumOFF.Value); break;

                case 8: if (myHVAC.bytSteps == null) myHVAC.bytSteps = new byte[15];
                        myHVAC.bytSteps[0] = Convert.ToByte(cbMode1.SelectedIndex);
                        myHVAC.bytSteps[1] = Convert.ToByte(Num11.Value);
                        myHVAC.bytSteps[2] = Convert.ToByte(Num12.Value);
                        myHVAC.bytSteps[3] = Convert.ToByte(Num13.Value);
                        myHVAC.bytSteps[4] = Convert.ToByte(Num14.Value);
                        break;

                case 9: if (myHVAC.bytSteps == null) myHVAC.bytSteps = new byte[15];
                        myHVAC.bytSteps[5] = Convert.ToByte(cbMode2.SelectedIndex);
                        myHVAC.bytSteps[6] = Convert.ToByte(Num21.Value);
                        myHVAC.bytSteps[7] = Convert.ToByte(Num22.Value);
                        myHVAC.bytSteps[8] = Convert.ToByte(Num23.Value);
                        myHVAC.bytSteps[9] = Convert.ToByte(Num24.Value);
                        break;

                case 10: if (myHVAC.bytSteps == null) myHVAC.bytSteps = new byte[15];
                        myHVAC.bytSteps[10] = Convert.ToByte(cbMode3.SelectedIndex);
                        myHVAC.bytSteps[11] = Convert.ToByte(Num31.Value);
                        myHVAC.bytSteps[12] = Convert.ToByte(Num32.Value);
                        myHVAC.bytSteps[13] = Convert.ToByte(Num33.Value);
                        myHVAC.bytSteps[14] = Convert.ToByte(Num34.Value);
                        break;
                case 11: myHVAC.bytFanONDelay = Convert.ToByte(NumONDelay1.Value);
                        NumONDelay.Value = myHVAC.bytFanONDelay;break;

                case 12: myHVAC.bytFanOFFDelay = Convert.ToByte(NumOFFDelay1.Value);
                        NumOFFDelay.Value = myHVAC.bytFanOFFDelay; break;

                case 13: myHVAC.bytHigh = Convert.ToByte(NumHigh1.Value); break;
                case 14: myHVAC.bytMid = Convert.ToByte(NumMid1.Value); break;
                case 15: myHVAC.bytLow = Convert.ToByte(NumLow1.Value); break;
                case 16: myHVAC.bytSteps[2] = Convert.ToByte(ComON.Value); break;
                case 17: myHVAC.bytSteps[3] = Convert.ToByte(ComOFF.Value); break;
            }
            myHVAC.MyRead2UpFlags[1] = false;
        }

        private void NumONDelay_ValueChanged(object sender, EventArgs e)
        {
            int intTag = Convert.ToInt16((sender as NumericUpDown).Tag.ToString()) ;
            UpdateBufferWhenchanged(intTag);
        }

        private void NumOFFDelay_ValueChanged(object sender, EventArgs e)
        {
            int intTag = Convert.ToInt16((sender as NumericUpDown).Tag.ToString());
            UpdateBufferWhenchanged(intTag);
        }

        private void cbMode1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int intTag = Convert.ToInt16((sender as ComboBox).Tag.ToString());
            UpdateBufferWhenchanged(intTag);
        }

        private void frmHVAC_SizeChanged(object sender, EventArgs e)
        {
        }

        private void frmHVAC_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Dispose();
        }

        private void toolStripButton11_Click(object sender, EventArgs e)
        {
            if (myHVAC == null) return;
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
                byte bytDevID = byte.Parse(strName.Split('-')[1]);

                byte[] ArayRelay = new byte[] { bytSubID, bytDevID, (byte)(mywdDeviceType / 256), (byte)(mywdDeviceType % 256), 
                    (byte)MyActivePage,(byte)(myintDIndex/256),(byte)(myintDIndex%256) };
                CsConst.MyUPload2DownLists.Add(ArayRelay);
                CsConst.MyUpload2Down = Convert.ToByte(((ToolStripButton)sender).Tag.ToString());
                FrmDownloadShow Frm = new FrmDownloadShow();
                if (CsConst.MyUpload2Down == 0) Frm.FormClosing += new FormClosingEventHandler(UpdateDisplayInformationAccordingly);
                Frm.ShowDialog();
            }
        }

        void UpdateDisplayInformationAccordingly(object sender, FormClosingEventArgs e)
        {
            DisplayHVACInformationToForm();
        }


        private void btnTest_Click(object sender, EventArgs e)
        {
            if (myHVAC == null) return;

            string strName = myDevName.Split('\\')[0].ToString();
            byte bytSubID = byte.Parse(strName.Split('-')[0]);
            byte bytDevID = byte.Parse(strName.Split('-')[1]);

            byte[] bytTmp = new byte[3];
            bytTmp[0] = 1;
            if (rbOFF.Checked)  bytTmp[0] = 0;

            if (rb1.Checked == true) bytTmp[1] = 1;
            else if (rb2.Checked == true) bytTmp[1] = 2;
            else if (rb3.Checked == true) bytTmp[1] = 4;

            if (rbHigh.Checked == true) bytTmp[2] = 1;
            else if (rbMid.Checked == true) bytTmp[2] = 2;
            else if (rbLow.Checked == true) bytTmp[2] = 4;

            if (CsConst.mySends.AddBufToSndList(bytTmp, 0x1C94, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(mywdDeviceType)) == false) return;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            if (myHVAC == null) return;

            string strName = myDevName.Split('\\')[0].ToString();
            byte bytSubID = byte.Parse(strName.Split('-')[0]);
            byte bytDevID = byte.Parse(strName.Split('-')[1]);

            byte[] bytTmp = null;
            if (CsConst.mySends.AddBufToSndList(bytTmp, 0x1C94, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(mywdDeviceType)) == true)
            {
                rbOFF.Checked = (CsConst.myRevBuf[25] == 0);

                rb1.Checked = (CsConst.myRevBuf[26] == 1);
                rb2.Checked  = (CsConst.myRevBuf[26] == 2);
                rb3.Checked = (CsConst.myRevBuf[26] ==4);

                rbHigh.Checked  = (CsConst.myRevBuf[27] == 1);
                rbMid.Checked  = (CsConst.myRevBuf[27] == 2);
                rbLow.Checked = (CsConst.myRevBuf[27] == 4);
            }
        }

        private void frmHVAC_Shown(object sender, EventArgs e)
        {
            SetCtrlsVisbleWithDifferentDeviceType();
            if (CsConst.MyEditMode == 0)
            {
                DisplayHVACInformationToForm();
                myIsLoading = false;
            }
            else if (CsConst.MyEditMode == 1)
            {
                MyActivePage = 1;

                if (myHVAC.MyRead2UpFlags[0] == false)
                {
                    tslRead_Click(tslRead, null);
                    myHVAC.MyRead2UpFlags[0] = true;
                    myHVAC.MyRead2UpFlags[1] = true;
                }
                else
                {
                    DisplayHVACInformationToForm();
                    myIsLoading = false;
                }

            }
        }


        void UpdateSensorsInformation()
        {
            if (myHVAC == null) return;
            if (myHVAC.bytSteps == null || myHVAC.bytSteps.Length == 0) return;
            if (lvSensor.Items.Count == 0) return;

            bool blnIsIN = false;
            bool blnOUT = false;

            foreach (DeviceInfo Tmp in CsConst.MyTmpSensors)
            {
                if (Tmp.StrName == myHVAC.DeviceName) blnIsIN = true;
                if (Tmp.StrName != myHVAC.DeviceName) blnOUT = true;

                if (blnIsIN && blnOUT) break;
            }

            if (blnIsIN == true)
            {
                rbIN.Checked = (blnIsIN && blnOUT == false);
                myHVAC.bytSteps[141] = 0;
                foreach (DeviceInfo Tmp in CsConst.MyTmpSensors)
                {
                    myHVAC.bytSteps[143] = Tmp.Other;
                    break;
                }
            }

            if (blnOUT == true)
            {
                rbBus.Checked = true;
                myHVAC.bytSteps[141] = 1;
                int i = 0;
                foreach (DeviceInfo Tmp in CsConst.MyTmpSensors)
                {
                    myHVAC.bytSteps[143] =Convert.ToByte(myHVAC.bytSteps[143] | Convert.ToByte(1 << i));
                    myHVAC.bytSteps[144 + i * 3] = Tmp.SubnetID;
                    myHVAC.bytSteps[145 + i * 3] = Tmp.DeviceID;
                    myHVAC.bytSteps[146 + i * 3] = Tmp.Other;
                    i++;
                }
            }
            if (rbIN.Checked) rbIN_CheckedChanged(rbIN, null);
            else if (rbBus.Checked) rbIN_CheckedChanged(rbBus, null);
        }

        void UpdateHVACsInformation()
        {
            if (myHVAC == null) return;
            if (myHVAC.bytSteps == null || myHVAC.bytSteps.Length == 0) myHVAC.bytSteps = new byte[200];
            if (lvHVAC.Items.Count == 0) return;
            if (CsConst.MyTmpSensors == null || CsConst.MyTmpSensors.Count == 0)
                myHVAC.bytSteps[180] = 0;
            else 
                myHVAC.bytSteps[180] = 1;

            int i = 0;
            myHVAC.bytSteps[181] = 1;
            myHVAC.bytSteps[182] = 0;
            foreach (DeviceInfo Tmp in CsConst.MyTmpSensors)
            {
                myHVAC.bytSteps[182] += Convert.ToByte(1 << i);
                myHVAC.bytSteps[183 + i * 2] = Tmp.SubnetID;
                myHVAC.bytSteps[184 + i * 2] = Tmp.DeviceID;
                i++;
            }
        }

        private void tbGive_Scroll(object sender, EventArgs e)
        {
            myHVAC.bytSteps[142] = Convert.ToByte(tbGive.Value);
            lbGive.Text = (tbGive.Value -10).ToString();
        }

        private void rbIN_CheckedChanged(object sender, EventArgs e)
        {
            //rbBus.Enabled = !(rbIN.Checked);
            NumRead.Enabled = rbBus.Enabled;
            lvSensor.Enabled = rbBus.Enabled;
            if (myHVAC.bytSteps == null || myHVAC.bytSteps.Length < 140) myHVAC.bytSteps = new byte[200];
            if (rbIN.Checked) myHVAC.bytSteps[141] = 0;
            else if (rbBus.Checked) myHVAC.bytSteps[141] = 1;
            myHVAC.MyRead2UpFlags[1] = false;
        }

        private void cboWMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            myHVAC.bytSteps[140] = (byte)cboWMode.SelectedIndex;
            myHVAC.MyRead2UpFlags[1] = false;
        }

        private void cboNMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            myHVAC.bytSteps[0] = (byte)cboNMode.SelectedIndex;

            Boolean bIsMaster = (cboNMode.SelectedIndex == 1);
            gbSensor.Enabled = bIsMaster;
            gbHVAC.Enabled = bIsMaster;
            myHVAC.MyRead2UpFlags[1] = false;
        }

        private void NumRead_ValueChanged(object sender, EventArgs e)
        {
            myHVAC.bytSteps[160] = (byte)NumRead.Value;
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lvSensor.SelectedItems == null) return;
            if (lvSensor.SelectedItems.Count == 0) return;


            while (lvSensor.SelectedItems != null && lvSensor.SelectedItems.Count != 0)
            {
                myHVAC.bytSteps[143] = Convert.ToByte(myHVAC.bytSteps[143] - (1 << lvSensor.SelectedItems[0].Index));
                myHVAC.bytSteps[144 + lvSensor.SelectedItems[0].Index * 3] = 0;
                myHVAC.bytSteps[145 + lvSensor.SelectedItems[0].Index * 3] = 0;
                lvSensor.Items.Remove(lvSensor.SelectedItems[0]);
            }
        }

        private void pasteNewAddToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CsConst.MyTmpSensors == null) return;
            byte bytTag = Convert.ToByte(((ToolStripMenuItem)sender).Tag.ToString());
            if (bytTag == 1) lvSensor.Items.Clear();
            foreach (DeviceInfo oTmp in CsConst.MyTmpSensors)
            {
                ListViewItem tmp = new ListViewItem();
                tmp.Checked = true;
                tmp.SubItems.Add(oTmp.StrName);
                tmp.SubItems.Add(oTmp.Other.ToString());
                lvSensor.Items.Add(tmp);
            }
            UpdateSensorsInformation();
        }

        private void chbCom_CheckedChanged(object sender, EventArgs e)
        {
            if (chbCom.Checked)
                myHVAC.bytSteps[1] = 1;
            else
                myHVAC.bytSteps[1] = 0;
            ComON.Enabled = chbCom.Checked;
            ComOFF.Enabled = chbCom.Checked;
        }

        private void ComON_ValueChanged(object sender, EventArgs e)
        {
            int intTag = Convert.ToInt16((sender as NumericUpDown).Tag.ToString());
            UpdateBufferWhenchanged(intTag);
        }

        private void ComOFF_ValueChanged(object sender, EventArgs e)
        {
            int intTag = Convert.ToInt16((sender as NumericUpDown).Tag.ToString());
            UpdateBufferWhenchanged(intTag);
        }

        private void btnHVAC_Click(object sender, EventArgs e)
        {
            Boolean isOpen = HDLSysPF.IsAlreadyOpenedForm("frmTSensor");

            if (isOpen == false)
            {
                frmTSensor frmUpdate = new frmTSensor(myHVAC, 4, CsConst.HvacGroup, CsConst.HvacGroup, myDevName);
                frmUpdate.ShowDialog();
                if (frmUpdate.DialogResult == System.Windows.Forms.DialogResult.OK)
                {
                    PasteH_Click(PasteRH, null);
                }
            }
        }

        private void DeleteH_Click(object sender, EventArgs e)
        {
            if (lvHVAC.SelectedItems == null) return;
            if (lvHVAC.SelectedItems.Count == 0) return;

            while (lvHVAC.SelectedItems != null && lvHVAC.SelectedItems.Count != 0)
            {
                myHVAC.bytSteps[181 + lvHVAC.SelectedItems[0].Index * 3] = 0;
                lvHVAC.Items.Remove(lvHVAC.SelectedItems[0]);
            }
        }

        private void PasteH_Click(object sender, EventArgs e)
        {            
            if (CsConst.MyTmpSensors == null) return;
            byte bytTag = Convert.ToByte(((ToolStripMenuItem)sender).Tag.ToString());
            if (bytTag == 1) lvHVAC.Items.Clear();
            foreach (DeviceInfo oTmp in CsConst.MyTmpSensors)
            {
                ListViewItem tmp = new ListViewItem();
                tmp.Checked = true;
                tmp.SubItems.Add(oTmp.StrName);
                tmp.SubItems.Add(oTmp.Other.ToString());
                lvHVAC.Items.Add(tmp);
            }
            UpdateHVACsInformation();
        }

        private void frmHVAC_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void btnRef4_Click(object sender, EventArgs e)
        {
            tslRead_Click(tslRead, null);
        }

        private void btnSave4_Click(object sender, EventArgs e)
        {
            tslRead_Click(toolStripLabel2, null);
        }

        private void btnSaveAndClose4_Click(object sender, EventArgs e)
        {
            tslRead_Click(toolStripLabel2, null);
            this.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Boolean isOpen = HDLSysPF.IsAlreadyOpenedForm("frmTSensor");

            if (isOpen == false)
            {
                frmTSensor frmUpdate = new frmTSensor(myHVAC, 4, CsConst.HvacGroup, CsConst.TemperatureGroup, myDevName);
                frmUpdate.ShowDialog();
                if (frmUpdate.DialogResult == System.Windows.Forms.DialogResult.OK)
                {
                    pasteNewAddToolStripMenuItem_Click(pasteNewAddToolStripMenuItem, null);
                }
            }
        }

        private void btnComplex_Click(object sender, EventArgs e)
        {
            frmHvacRelay frmUpdate = new frmHvacRelay(myHVAC, myDevName, 2);
            frmUpdate.ShowDialog();
        }
    }
}
