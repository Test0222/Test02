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
    public partial class frmModifAddress : Form
    {
        private byte bytSubID = 0;
        private byte bytDevID = 0;
        private int MyintDeviceType = 0;

        public frmModifAddress()
        {
            InitializeComponent();
        }

        public frmModifAddress(byte bytSubID, byte bytDevID,int intDeviceType)
        {
            InitializeComponent();
            this.bytSubID = bytSubID;
            this.bytDevID = bytDevID;
            this.MyintDeviceType = intDeviceType;

            tbDev.Text = bytDevID.ToString();
            tbsub.Text = bytSubID.ToString();

            if (IPmoduleDeviceTypeList.HDLIPModuleDeviceTypeLists.Contains(MyintDeviceType))
            {
                numDev.Minimum = 0;
                numDev.Value = 0;
                numDev.Enabled = false;
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            CsConst.MyTmpName = new List<string>();
            this.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (tbMAC.Text == null || tbMAC.Text == "")
            {
                MessageBox.Show(CsConst.mstrINIDefault.IniReadValue("Public", "99631", ""), "",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                return;
            }
            Cursor.Current = Cursors.WaitCursor;

            byte[] ArayTmp = new byte[10];
            ArayTmp[8] = Convert.ToByte(numSub.Value.ToString());
            ArayTmp[9] = Convert.ToByte(numDev.Value.ToString());

            if (CsConst.MyEditMode == 1) 
            {
                if (tbMAC.Text == null || tbMAC.Text == "") return;
                string[] ArayStr = tbMAC.Text.Split('.');
                for (int intI = 0; intI < ArayStr.Length; intI++) ArayTmp[intI] = Convert.ToByte(ArayStr[intI], 16);
                #region
                // 是不是特殊版本的设备
                if (CsConst.SpecailAddress.Contains(MyintDeviceType)) // 要不要加修改地址加密界面
                {
                    if (HDLSysPF.SpecialModifyAddress(MyintDeviceType, 1, bytSubID, bytDevID, ArayTmp) == true)
                    {
                        byte[] arayTmp = new byte[10];
                        ArayTmp = new byte[8];
                        for (int intI = 0; intI < ArayStr.Length; intI++) ArayTmp[intI] = Convert.ToByte(ArayStr[intI], 16);
                        Array.Copy(ArayTmp, 0, arayTmp, 0, 8);
                        arayTmp[8] = Convert.ToByte(numSub.Value);
                        arayTmp[9] = Convert.ToByte(numDev.Value);
                        CsConst.ModifyDeviceAddressSubNetID = arayTmp[8];
                        CsConst.ModifyDeviceAddressDeviceID = arayTmp[9];
                        if (CsConst.mySends.AddBufToSndList(arayTmp, 0xF005, bytSubID, bytDevID, false, true, true,false)) //修改地址
                        {
                            ((Button)sender).Enabled = true;
                            CsConst.MyTmpName = new List<string>();
                            CsConst.MyTmpName.Add(arayTmp[8].ToString());
                            CsConst.MyTmpName.Add(arayTmp[9].ToString());
                            CsConst.myRevBuf = new byte[1200];
                            DialogResult = DialogResult.OK;
                            this.Close();
                        }
                    }
                    else
                    {
                        MessageBox.Show(CsConst.mstrINIDefault.IniReadValue("Public", "99632", ""), ""
                                                      , MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                        Cursor.Current = Cursors.Default;
                        return;

                    }
                }
                else
                {
                    byte[] arayTmp = new byte[10];
                    ArayTmp = new byte[8];
                    for (int intI = 0; intI < ArayStr.Length; intI++) ArayTmp[intI] = Convert.ToByte(ArayStr[intI], 16);
                    Array.Copy(ArayTmp, 0, arayTmp, 0, 8);
                    arayTmp[8] = Convert.ToByte(numSub.Value);
                    arayTmp[9] = Convert.ToByte(numDev.Value);
                    CsConst.ModifyDeviceAddressSubNetID = arayTmp[8];
                    CsConst.ModifyDeviceAddressDeviceID = arayTmp[9];
                    if (CsConst.mySends.AddBufToSndList(arayTmp, 0xF005, bytSubID, bytDevID, false, false, true, false)) //修改地址
                    {
                        ((Button)sender).Enabled = true;
                        CsConst.MyTmpName = new List<string>();
                        CsConst.MyTmpName.Add(arayTmp[8].ToString());
                        CsConst.MyTmpName.Add(arayTmp[9].ToString());
                        CsConst.myRevBuf = new byte[1200];
                        DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show(CsConst.mstrINIDefault.IniReadValue("Public", "99632", ""), ""
                                                      , MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                }
                #endregion
            }
            Cursor.Current = Cursors.Default;
        }

        private void frmRemark_Load(object sender, EventArgs e)
        {
            button1_Click(button1, null);
        }

        private void btnRead_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            ((Button)sender).Enabled = false;

            byte[] ArayTmp = null;
            UDPReceive.ClearQueueData();
            bool isRead = false;
            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xE5F5, 255, 255, false, false, true, false) == true)
            {
                System.Threading.Thread.Sleep(100);
                for (int i = 0; i < UDPReceive.receiveQueue.Count; i++)
                {
                    byte[] readData = UDPReceive.receiveQueue.ToArray()[i];
                    if (readData[21] == 0xE5 && readData[22] == 0xF6)
                    {
                        tbsub.Text = readData[25].ToString();
                        tbDev.Text = readData[26].ToString();
                        MyintDeviceType = readData[19] * 256 + readData[20];
                        btnModify1.Enabled = true;
                        isRead = true;
                        break;
                    }
                }
            }
            ((Button)sender).Enabled = true;
            Cursor.Current = Cursors.Default;
            if (!isRead)
            {
                MessageBox.Show(CsConst.mstrINIDefault.IniReadValue("Public", "99633", ""), ""
                                                        , MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
            }
        }

        private void btnModify1_Click(object sender, EventArgs e)
        {
            if (numSub.Value == 255) return;
            if (numDev.Value == 255) return;
            byte[] ArayTmp = new byte[2];
            
            Cursor.Current = Cursors.WaitCursor;
            
            ArayTmp[0] = (byte)numSub.Value;
            ArayTmp[1] = (byte)numDev.Value;
            bool isModify = false;
            if (CsConst.SpecailAddress.Contains(MyintDeviceType)) // 要不要加修改地址加密界面
            {
                if (HDLSysPF.SpecialModifyAddress(MyintDeviceType, 0, 255, 255, ArayTmp) == true)
                {
                    UDPReceive.ClearQueueData();
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xE5F7, 255, 255, false, false, true, false) == true)
                    {
                        System.Threading.Thread.Sleep(100);
                        for (int i = 0; i < UDPReceive.receiveQueue.Count; i++)
                        {
                            byte[] readData = UDPReceive.receiveQueue.ToArray()[i];
                            if (readData[21] == 0xE5 && readData[22] == 0xF8)
                            {
                                CsConst.MyTmpName = new List<string>();
                                CsConst.MyTmpName.Add(ArayTmp[0].ToString());
                                CsConst.MyTmpName.Add(ArayTmp[1].ToString());
                                DialogResult = DialogResult.OK;
                                Cursor.Current = Cursors.Default;
                                isModify = true;
                                this.Close();
                                break;
                            }
                        }
                    }
                    if (isModify)
                        this.Close();
                    else
                        MessageBox.Show(CsConst.mstrINIDefault.IniReadValue("Public", "99632", ""), ""
                                                        , MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                }
                else
                {
                    MessageBox.Show(CsConst.mstrINIDefault.IniReadValue("Public", "99632", ""), ""
                                                        , MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                }
            }
            else
            {
                UDPReceive.ClearQueueData();
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xE5F7, 255, 255, false, false, true, false) == true)
                {
                    System.Threading.Thread.Sleep(100);
                    for (int i = 0; i < UDPReceive.receiveQueue.Count; i++)
                    {
                        byte[] readData = UDPReceive.receiveQueue.ToArray()[i];
                        if (readData[21] == 0xE5 && readData[22] == 0xF8)
                        {
                            CsConst.MyTmpName = new List<string>();
                            CsConst.MyTmpName.Add(ArayTmp[0].ToString());
                            CsConst.MyTmpName.Add(ArayTmp[1].ToString());
                            DialogResult = DialogResult.OK;
                            Cursor.Current = Cursors.Default;
                            isModify = true;
                            break;
                        }
                    }
                }
                if (isModify)
                    this.Close();
                else
                    MessageBox.Show(CsConst.mstrINIDefault.IniReadValue("Public", "99632", ""), ""
                                                            , MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
            }

            Cursor.Current = Cursors.Default;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ((Button)sender).Enabled = false;
            Cursor.Current = Cursors.WaitCursor;
            tbMAC.Text = "";
            if (MyintDeviceType != -1 && MyintDeviceType !=0)
            {
                String sMacInformation = HdlUdpPublicFunctions.ReadDeviceMacInformation(bytSubID, bytDevID,false);
                if (sMacInformation == "") sMacInformation = HdlUdpPublicFunctions.ReadDeviceMacInformationViaBroadcast(bytSubID, bytDevID, MyintDeviceType);
                if (sMacInformation != "")  //读取MAC备注
                {
                    gbMAC.Visible = true;
                    gbManual.Visible = false;
                    tbMAC.Text = sMacInformation;
                }
                else
                {
                    gbMAC.Visible = false;
                    gbManual.Visible = true;
                    tbsub.Text = "0";
                    tbDev.Text = "0";
                }
            }
            else
            {
                gbMAC.Visible = false;
                gbManual.Visible = true;
                tbsub.Text = "0";
                tbDev.Text = "0";
            }
            Cursor.Current = Cursors.Default;
            ((Button)sender).Enabled = true;
        }

        private void frmModifAddress_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (CsConst.MyEditMode == 0)
            {
                CsConst.MyTmpName = new List<string>();
                CsConst.MyTmpName.Add(numSub.Value.ToString());
                CsConst.MyTmpName.Add(numDev.Value.ToString());

                DialogResult = DialogResult.OK;
                Cursor.Current = Cursors.Default;
            }
        }

        private void tbsub_TextChanged(object sender, EventArgs e)
        {
            //this.bytSubID = byte.Parse(tbsub.Text);
            //this.bytDevID = byte.Parse(tbDev.Text);
        }

        private void numSub_ValueChanged(object sender, EventArgs e)
        {
           
        }
    }
}
