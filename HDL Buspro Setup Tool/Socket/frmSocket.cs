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
    public partial class frmSocket : Form
    {
        private string MyDevName = null;
        private byte SubNetID;
        private byte DeviceID;
        private int mywdDevicerType = 0;
        private int mintIDIndex = -1;
        private RFSocket oReceptacles;
        private int MyActivePage = 0; //按页面上传下载
        private bool isRead = false;
        public frmSocket()
        {
            InitializeComponent();
        }

        public frmSocket(RFSocket receptacles, string strName, int intDeviceType, int intDIndex)
        {
            InitializeComponent();
            this.oReceptacles = receptacles;
            this.mywdDevicerType = intDeviceType;
            this.MyDevName = strName;
            this.mintIDIndex = intDIndex;
            
            string strDevName = strName.Split('\\')[0].ToString();
            SubNetID = Convert.ToByte(strDevName.Split('-')[0]);
            DeviceID = Convert.ToByte(strDevName.Split('-')[1]);
            this.Text = MyDevName;
            tsl3.Text = MyDevName;
            HDLSysPF.DisplayDeviceNameModeDescription(strName, mywdDevicerType, cboDevice, tbModel, tbDescription);
        }

        private void FrmReceptacles_Load(object sender, EventArgs e)
        {
            InitialFormCtrlsTextOrItems();
        }

        void SetCtrlsVisbleWithDifferentDeviceType()
        {
            toolStrip1.Visible = (CsConst.MyEditMode == 0);
        }

        void InitialFormCtrlsTextOrItems()
        {
            cbAlarm1.Items.Clear();
            for (int i = 80; i <= 100; i++)
            {
                cbAlarm1.Items.Add(i.ToString() + "C");
            }
        }

        private void FrmReceptacles_Shown(object sender, EventArgs e)
        {
            SetCtrlsVisbleWithDifferentDeviceType();
            
            if (CsConst.MyEditMode == 1) //在线模式
            {
                MyActivePage = 1;
                tsbDown_Click(tsbDown, null);
            }
        }

        private void tsbDown_Click(object sender, EventArgs e)
        {
            try
            {
                byte bytTag = Convert.ToByte(((ToolStripButton)sender).Tag.ToString());
                bool blnShowMsg = (CsConst.MyEditMode != 1);
                if (HDLPF.UploadOrReadGoOn(this.Text, bytTag, blnShowMsg))
                {
                    Cursor.Current = Cursors.WaitCursor;

                    CsConst.MyUPload2DownLists = new List<byte[]>();

                    string strName = MyDevName.Split('\\')[0].ToString();
                    byte bytSubID = byte.Parse(strName.Split('-')[0]);
                    byte bytDevID = byte.Parse(strName.Split('-')[1]);
                    byte[] ArayRelay = new byte[] { SubNetID, DeviceID, (byte)(mywdDevicerType / 256), (byte)(mywdDevicerType % 256)
                        , (byte)MyActivePage,(byte)(mintIDIndex/256),(byte)(mintIDIndex%256)  };
                    CsConst.MyUPload2DownLists.Add(ArayRelay);
                    CsConst.MyUpload2Down = Convert.ToByte(((ToolStripButton)sender).Tag.ToString());
                    FrmDownloadShow Frm = new FrmDownloadShow();
                    Frm.FormClosing += new FormClosingEventHandler(UpdateDisplayInformationAccordingly);
                    Frm.ShowDialog();
                }
            }
            catch
            {
            }
        }

        void UpdateDisplayInformationAccordingly(object sender, FormClosingEventArgs e)
        {
            switch (tabControl1.SelectedIndex)
            {
                case 0: showBasicInfo(); break;
                case 1: showAdjust(); break;
            }
            this.TopMost = true;
            this.WindowState = FormWindowState.Normal;
            this.Activate();
            this.TopMost = false;
        }

        private void showBasicInfo()
        {
            try
            {
                isRead = true;
                if (oReceptacles == null) return;
                if (oReceptacles.MyBasicInfo == null) return;
                if (oReceptacles.MyBasicInfo.arayElectri[1] == 0)
                {
                    lb7.Text = CsConst.Status[0];
                    btnRelay.Text = CsConst.mstrINIDefault.IniReadValue("Public", "99649", "");
                    btnRelay.Tag = 100;
                }
                else if (oReceptacles.MyBasicInfo.arayElectri[1] == 100)
                {
                    lb7.Text = CsConst.Status[1];
                    btnRelay.Text = CsConst.mstrINIDefault.IniReadValue("Public", "99648", "");
                    btnRelay.Tag = 0;
                }
                lb8.Text = ((oReceptacles.MyBasicInfo.arayElectri[2] * 256 + oReceptacles.MyBasicInfo.arayElectri[3]) / 10).ToString() + "." +
                           ((oReceptacles.MyBasicInfo.arayElectri[2] * 256 + oReceptacles.MyBasicInfo.arayElectri[3]) % 10).ToString() + "V";
                string str = string.Format("{0:D3}", ((oReceptacles.MyBasicInfo.arayElectri[4] * 256 + oReceptacles.MyBasicInfo.arayElectri[5]) % 1000));
                lb9.Text = ((oReceptacles.MyBasicInfo.arayElectri[4] * 256 + oReceptacles.MyBasicInfo.arayElectri[5]) / 1000).ToString() + "." +
                           str + "A";
                lb10.Text = ((oReceptacles.MyBasicInfo.arayElectri[6] * 256 + oReceptacles.MyBasicInfo.arayElectri[7]) / 10).ToString() + "." +
                            ((oReceptacles.MyBasicInfo.arayElectri[6] * 256 + oReceptacles.MyBasicInfo.arayElectri[7]) % 10).ToString() + "W";
                lb11.Text = ((oReceptacles.MyBasicInfo.arayElectri[8] * 256 * 256 * 256 +
                              oReceptacles.MyBasicInfo.arayElectri[9] * 256 * 256 +
                              oReceptacles.MyBasicInfo.arayElectri[10] * 256 +
                              oReceptacles.MyBasicInfo.arayElectri[11]) / 10).ToString()+"."+
                              ((oReceptacles.MyBasicInfo.arayElectri[8] * 256 * 256 * 256 +
                              oReceptacles.MyBasicInfo.arayElectri[9] * 256 * 256 +
                              oReceptacles.MyBasicInfo.arayElectri[10] * 256 +
                              oReceptacles.MyBasicInfo.arayElectri[11]) % 10).ToString()+"kw.h";
                lb12.Text = oReceptacles.MyBasicInfo.arayElectri[12].ToString()+"%";

                if (80 <= oReceptacles.MyBasicInfo.arayAlarm[0] && oReceptacles.MyBasicInfo.arayAlarm[0] <= 100)
                    cbAlarm1.SelectedIndex = cbAlarm1.Items.IndexOf(oReceptacles.MyBasicInfo.arayAlarm[0].ToString() + "C");
                str = (oReceptacles.MyBasicInfo.arayAlarm[1] / 10).ToString() + "." + (oReceptacles.MyBasicInfo.arayAlarm[1] % 10).ToString()+"A";
                cbAlarm2.SelectedIndex = cbAlarm2.Items.IndexOf(str);
                
                if (0 <= oReceptacles.MyBasicInfo.arayAlarm[2] && oReceptacles.MyBasicInfo.arayAlarm[2] <= 100)
                    sbAlarm.Value = oReceptacles.MyBasicInfo.arayAlarm[2];
                if (oReceptacles.MyBasicInfo.arayAlarm[3] == 1)
                    lbAlarm2.Text = CsConst.mstrINIDefault.IniReadValue("Public", "99653", "");
                else
                    lbAlarm2.Text = CsConst.mstrINIDefault.IniReadValue("Public", "99763", "");

                if (oReceptacles.MyBasicInfo.arayAlarm[4] == 1)
                    lbAlarm4.Text = CsConst.mstrINIDefault.IniReadValue("Public", "99652", "");
                else
                    lbAlarm4.Text = CsConst.mstrINIDefault.IniReadValue("Public", "99763", "");

                lbAlarm11.Text = oReceptacles.MyBasicInfo.arayAlarm[5].ToString() + "C";
                lbAlarm13.Text = ((oReceptacles.MyBasicInfo.arayAlarm[6] * 256 + oReceptacles.MyBasicInfo.arayAlarm[7]) / 1000).ToString() + "." +
                                 ((oReceptacles.MyBasicInfo.arayAlarm[6] * 256 + oReceptacles.MyBasicInfo.arayAlarm[7]) % 1000).ToString() + "A";
            }
            catch
            {
            }
            isRead = false;
        }

        private void showAdjust()
        {
            try
            {
                isRead = true;
                if (oReceptacles == null) return;
                if (oReceptacles.MyBasicInfo == null) return;
                lbV3.Text = ((oReceptacles.MyBasicInfo.araAdjustV[0] * 256 + oReceptacles.MyBasicInfo.araAdjustV[1]) / 10).ToString() + "." +
                            ((oReceptacles.MyBasicInfo.araAdjustV[0] * 256 + oReceptacles.MyBasicInfo.araAdjustV[1]) % 10).ToString() + "V";
                lbE3.Text = ((oReceptacles.MyBasicInfo.araAdjustE[0] * 256 + oReceptacles.MyBasicInfo.araAdjustE[1]) / 1000).ToString() + "." +
                            ((oReceptacles.MyBasicInfo.araAdjustE[0] * 256 + oReceptacles.MyBasicInfo.araAdjustE[1]) % 1000).ToString() + "A";
                lbP3.Text = ((oReceptacles.MyBasicInfo.araAdjustP[0] * 256 + oReceptacles.MyBasicInfo.araAdjustP[1]) / 10).ToString() + "." +
                            ((oReceptacles.MyBasicInfo.araAdjustP[0] * 256 + oReceptacles.MyBasicInfo.araAdjustP[1]) % 10).ToString() + "W";
            }
            catch
            {
            }
            isRead = false;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            tsbDown_Click(tsbUpload, null);
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CsConst.MyEditMode == 1)
            {
                if (tabControl1.SelectedIndex == 1)
                {
                    if (!CsConst.isRightPasswork)
                    {
                        FrmPassWork frmTmp = new FrmPassWork();
                        DialogResult = DialogResult.OK;
                        if (frmTmp.ShowDialog() == DialogResult.Cancel)
                        {
                            tabControl1.SelectedIndex = 0;
                        }
                    }
                    if (!CsConst.isRightPasswork) return;
                }
                MyActivePage = tabControl1.SelectedIndex + 1;
                if (oReceptacles.MyRead2UpFlags[MyActivePage - 1] == false)
                {
                    tsbDown_Click(tsbDown, null);
                }
                else
                {
                    switch (tabControl1.SelectedIndex)
                    {
                        case 0: showBasicInfo(); break;
                        case 1: showAdjust(); break;
                    }
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                byte[] ArayTmp = new byte[0];
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1D80, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(mywdDevicerType)) == true)
                {
                    Array.Copy(CsConst.myRevBuf, 25, oReceptacles.MyBasicInfo.arayElectri, 0, 13);
                    HDLUDP.TimeBetwnNext(10);
                    CsConst.myRevBuf = new byte[1200];
                    showBasicInfo();
                }
            }
            catch
            {
            } 
            Cursor.Current = Cursors.Default;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                byte[] ArayTmp = new byte[0];
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1D90, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(mywdDevicerType)) == true)
                {
                    Array.Copy(CsConst.myRevBuf, 26, oReceptacles.MyBasicInfo.arayAlarm, 0, 8);
                    HDLUDP.TimeBetwnNext(10);
                    CsConst.myRevBuf = new byte[1200];
                    showBasicInfo();
                }
            }
            catch
            {
            }
            Cursor.Current = Cursors.Default;
        }

        private void btnV1_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                byte[] ArayTmp = new byte[0];
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1D84, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(mywdDevicerType)) == true)
                {
                    Array.Copy(CsConst.myRevBuf, 26, oReceptacles.MyBasicInfo.araAdjustV, 0, 2);
                    HDLUDP.TimeBetwnNext(10);
                    CsConst.myRevBuf = new byte[1200];
                    showAdjust();
                }
            }
            catch
            {
            }
            Cursor.Current = Cursors.Default;
        }

        private void btnV2_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                byte[] arayTmp = new byte[3];
                string str = txtV.Text;
                int intTmp = Convert.ToInt32(Convert.ToDecimal(str) * 10);
                arayTmp[0] = 1;
                arayTmp[1] = Convert.ToByte(intTmp / 256);
                arayTmp[2] = Convert.ToByte(intTmp % 256);
                if (CsConst.mySends.AddBufToSndList(arayTmp, 0x1D82, SubNetID, DeviceID, false, true, true, false) == true)
                {
                    if (CsConst.myRevBuf[25] == 0xF8)
                    {
                        Array.Copy(CsConst.myRevBuf, 27, oReceptacles.MyBasicInfo.araAdjustV, 0, 2);
                        lbV3.Text = ((CsConst.myRevBuf[27] * 256 + CsConst.myRevBuf[28]) / 10).ToString() + "." +
                                    ((CsConst.myRevBuf[27] * 256 + CsConst.myRevBuf[28]) % 10).ToString() + "(V)";
                        MessageBox.Show(CsConst.mstrINIDefault.IniReadValue("Public", "99650", ""),
                                                ""
                                                 , MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    }
                    else
                    {
                        MessageBox.Show(CsConst.mstrINIDefault.IniReadValue("Public", "99651", ""),
                                                ""
                                                 , MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    }
                    CsConst.myRevBuf = new byte[1200];
                }
            }
            catch
            {
            }
            Cursor.Current = Cursors.Default;
        }

        private void btnE1_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                byte[] ArayTmp = new byte[0];
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1D88, SubNetID, DeviceID, false, false, true, CsConst.minAllWirelessDeviceType.Contains(mywdDevicerType)) == true)
                {
                    Array.Copy(CsConst.myRevBuf, 26, oReceptacles.MyBasicInfo.araAdjustE, 0, 2);
                    HDLUDP.TimeBetwnNext(10);
                    CsConst.myRevBuf = new byte[1200];
                    showAdjust();
                }
            }
            catch
            {
            }
            Cursor.Current = Cursors.Default;
        }

        private void btnE2_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                byte[] arayTmp = new byte[3];
                string str = txtE.Text;
                int intTmp = Convert.ToInt32(Convert.ToDecimal(str) * 1000);
                arayTmp[0] = 1;
                arayTmp[1] = Convert.ToByte(intTmp / 256);
                arayTmp[2] = Convert.ToByte(intTmp % 256);
                if (CsConst.mySends.AddBufToSndList(arayTmp, 0x1D86, SubNetID, DeviceID, false, false, true, false) == true)
                {
                    if (CsConst.myRevBuf[25] == 0xF8)
                    {
                        Array.Copy(CsConst.myRevBuf, 27, oReceptacles.MyBasicInfo.araAdjustE, 0, 2);
                        lbE3.Text = ((CsConst.myRevBuf[27] * 256 + CsConst.myRevBuf[28]) / 1000).ToString() + "." +
                                    ((CsConst.myRevBuf[27] * 256 + CsConst.myRevBuf[28]) % 1000).ToString() + "(A)";
                        MessageBox.Show(CsConst.mstrINIDefault.IniReadValue("Public", "99650", ""),
                                               ""
                                                , MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    }
                    else
                    {
                        MessageBox.Show(CsConst.mstrINIDefault.IniReadValue("Public", "99651", ""),
                                               ""
                                                , MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    }
                    CsConst.myRevBuf = new byte[1200];
                }
            }
            catch
            {
            }
            Cursor.Current = Cursors.Default;
        }

        private void btnP1_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                byte[] ArayTmp = new byte[0];
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1D8C, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(mywdDevicerType)) == true)
                {
                    Array.Copy(CsConst.myRevBuf, 26, oReceptacles.MyBasicInfo.araAdjustP, 0, 2);
                    HDLUDP.TimeBetwnNext(10);
                    CsConst.myRevBuf = new byte[1200];
                    showAdjust();
                }
            }
            catch
            {
            }
            Cursor.Current = Cursors.Default;
        }

        private void btnP2_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                byte[] arayTmp = new byte[3];
                string str = txtP.Text;
                int intTmp = Convert.ToInt32(Convert.ToDecimal(str) * 10);
                arayTmp[0] = 1;
                arayTmp[1] = Convert.ToByte(intTmp / 256);
                arayTmp[2] = Convert.ToByte(intTmp % 256);
                if (CsConst.mySends.AddBufToSndList(arayTmp, 0x1D8A, SubNetID, DeviceID, false, false, true, false) == true)
                {
                    if (CsConst.myRevBuf[25] == 0xF8)
                    {
                        Array.Copy(CsConst.myRevBuf, 27, oReceptacles.MyBasicInfo.araAdjustP, 0, 2);
                        lbP3.Text = ((CsConst.myRevBuf[27] * 256 + CsConst.myRevBuf[28]) / 10).ToString() + "." +
                                    ((CsConst.myRevBuf[27] * 256 + CsConst.myRevBuf[28]) % 10).ToString() + "(W)";
                        MessageBox.Show(CsConst.mstrINIDefault.IniReadValue("Public", "99650", ""),
                                                 ""
                                                  , MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    }
                    else
                    {
                        MessageBox.Show(CsConst.mstrINIDefault.IniReadValue("Public", "99651", ""),
                                               ""
                                                , MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    }
                    CsConst.myRevBuf = new byte[1200];
                }
            }
            catch
            {
            }
            Cursor.Current = Cursors.Default;
        }

        private void sbAlarm_ValueChanged(object sender, EventArgs e)
        {
            lbAlarm8.Text = sbAlarm.Value.ToString() + "%";
        }

        private void txtV_KeyPress(object sender, KeyPressEventArgs e)
        {
            string str = ".1234567890";
            if (e.KeyChar != 8)
            {
                string strTxt = (sender as TextBox).Text;
                if (strTxt.Contains(".") && (e.KeyChar.ToString() == ".")) e.Handled = true;
                if (str.IndexOf(e.KeyChar.ToString()) < 0)
                {
                    e.Handled = true;
                }
            }
        }

        private void btnSaveAlarm_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                string str1 = cbAlarm1.Text.Trim();
                str1 = str1.Replace("C", "");
                string str = cbAlarm2.Text.Trim();
                str = str.Replace("A", "");
                byte[] arayTmp = new byte[4];
                arayTmp[0] = 1;
                arayTmp[1] = Convert.ToByte(str1);
                arayTmp[2] = Convert.ToByte(Convert.ToDecimal(str) * 10);
                arayTmp[3] = Convert.ToByte(sbAlarm.Value);
                if (arayTmp[2] > 140) arayTmp[2] = 140;
                if (arayTmp[2] < 50) arayTmp[2] = 50;
                if (CsConst.mySends.AddBufToSndList(arayTmp, 0x1D8E, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(mywdDevicerType)) == true)
                {
                    HDLUDP.TimeBetwnNext(10);
                    CsConst.myRevBuf = new byte[1200];
                    Array.Copy(arayTmp, 1, oReceptacles.MyBasicInfo.arayAlarm, 0, 3);
                }
            }
            catch
            {
            }
            Cursor.Current = Cursors.Default;
        }

        private void btnRelay_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            byte[] arayTmp = new byte[4];
            arayTmp[0] = 1;
            arayTmp[1] = Convert.ToByte(btnRelay.Tag);
            if (CsConst.mySends.AddBufToSndList(arayTmp, 0x0031, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(mywdDevicerType)) == true)
            {
                CsConst.myRevBuf = new byte[1200];
                button3_Click(null, null);
            }
            Cursor.Current = Cursors.Default;
        }

        private void txtV_Leave(object sender, EventArgs e)
        {
            string str = txtV.Text;
            int intTmp = Convert.ToInt32(Convert.ToDecimal(str) * 10);
            if (intTmp == 0)
            {
                txtV.Text = "220.0";
                txtV.SelectionStart = txtV.TextLength;
            }
        }

        private void txtE_Leave(object sender, EventArgs e)
        {
            string str = txtE.Text;
            int intTmp = Convert.ToInt32(Convert.ToDecimal(str) * 10);
            if (intTmp == 0)
            {
                txtE.Text = "4.54";
                txtE.SelectionStart = txtE.TextLength;
            }
        }

        private void txtP_Leave(object sender, EventArgs e)
        {
            string str = txtP.Text;
            int intTmp = Convert.ToInt32(Convert.ToDecimal(str) * 10);
            if (intTmp == 0)
            {
                txtP.Text = "1000.0";
                txtP.SelectionStart = txtP.TextLength;
            }
        }
    }
}
