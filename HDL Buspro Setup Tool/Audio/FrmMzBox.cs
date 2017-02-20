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
    public partial class FrmMzBox : Form
    {
        private string myDevName = null;
        private int mintIDIndex = -1;
        private byte SubNetID;
        private byte DeviceID;
        private int MyintDeviceType;
        private MzBox oMzBox = new MzBox();
        private int MyActivePage = 0; //按页面上传下载
        private bool isRead = false;
        FrmProcess frmProcessTmp;
        private int ProcessValue = 0;
        private SendIR tempSend1 = new SendIR();  //用于存储公共的红外码
        private SendIR tempSend2 = new SendIR();  //用于存储公共的红外码
        private BackgroundWorker MyBackGroup;
        private bool isStopDownloadCodes = false;
        private Byte SDorFTP = 0;
        private Byte PlaySongSource = 1;

        private int ReadFileIndex = 0;
        List<string> strFileList = new List<string>();
        private int LengReadFileStrLength = 0;

        private TreeNode SelectNode;
        NetworkInForm networkinfo;
        FrequenceForAudio Frequence = new FrequenceForAudio("0.0");

        private int DataGridViewSelectIndexOfKNX = 0;
        public FrmMzBox()
        {
            InitializeComponent();
        }

        public FrmMzBox(MzBox mzbox, string strName, int intdevicetype, int intdIndex)
        {
            InitializeComponent();
            this.myDevName = strName;
            this.mintIDIndex = intdIndex;
            string strDevName = strName.Split('\\')[0].ToString();
            this.MyintDeviceType = intdevicetype;
            this.oMzBox = mzbox;

            HDLSysPF.DisplayDeviceNameModeDescription(strName, MyintDeviceType, cboDevice, tbModel, tbDescription);

            SubNetID = Convert.ToByte(strDevName.Split('-')[0]);
            DeviceID = Convert.ToByte(strDevName.Split('-')[1]);
            tsl3.Text = strName;
        }

        void SetCtrlsVisbleWithDifferentDeviceType()
        {
            toolStrip1.Visible = (CsConst.MyEditMode == 0);
            if (AudioDeviceTypeList.DinRailAudioDeviceType.Contains(MyintDeviceType)) //丁导轨系列
            {
                checklist.Items.RemoveAt(3);
                checklist.Items.RemoveAt(4);
                chb1.Visible = false;
                chb2.Visible = false;
                chb3.Visible = false;
                tabControl.TabPages.Remove(tabPDIF);
                tabControl.TabPages.Remove(tabIn);
            }
        }

        void InitialFormCtrlsTextOrItems()
        {
            clFM4.Items.Clear();
            clFM4.Items.Add(CsConst.WholeTextsList[1775].sDisplayName);
            clFM4.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00042", ""));
            HDLSysPF.addIR(tvIN, tempSend1, true);
            HDLSysPF.addIR(tvPDIF, tempSend2, true);
            panel26.Controls.Clear();
            dgvFM.Controls.Add(Frequence);
            Frequence.UserControlValueChanged += Frequence_UserControlValueChanged;
            Frequence.Visible = false;
            networkinfo = new NetworkInForm(SubNetID, DeviceID, MyintDeviceType);
            panel26.Controls.Add(networkinfo);
            networkinfo.Dock = DockStyle.Fill;
        }

        void Frequence_UserControlValueChanged(object sender, FrequenceForAudio.TextChangeEventArgs e)
        {
            int index = dgvFM.CurrentRow.Index;
            dgvFM[1, index].Value = Frequence.Text;
            HDLSysPF.ModifyMultilinesIfNeeds(dgvFM[1, index].Value.ToString(), 1, dgvFM);
        }

        private void FrmMzBox_Load(object sender, EventArgs e)
        {
            InitialFormCtrlsTextOrItems();
        }

        void RefreshActivePageIndex()
        {
            if (tabControl.SelectedTab.Name == "tabBasic") MyActivePage = 1;
            else if (tabControl.SelectedTab.Name == "tabFTP") MyActivePage = 2;
            else if (tabControl.SelectedTab.Name == "tabFM") MyActivePage = 3;
            else if (tabControl.SelectedTab.Name == "tabIn") MyActivePage = 4;
            else if (tabControl.SelectedTab.Name == "tabPDIF") MyActivePage = 5;
            else if (tabControl.SelectedTab.Name == "tabKNX") MyActivePage = 6;
        }

        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshActivePageIndex();
            if (tabControl.SelectedTab.Name == "tabTest")
            {
            }
            else
            {
                if (oMzBox.MyRead2UpFlags[MyActivePage - 1] == false || tabControl.SelectedTab.Name == "tabIn"
                    || tabControl.SelectedTab.Name == "tabPDIF")
                {
                    tsbDown_Click(tsbDown, null);
                }
                else
                {
                    UpdateDisplayInformationAccordingly(null, null);
                }
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

                    string strName = myDevName.Split('\\')[0].ToString();
                    byte bytSubID = byte.Parse(strName.Split('-')[0]);
                    byte bytDevID = byte.Parse(strName.Split('-')[1]);
                    RefreshActivePageIndex();
                    byte[] ArayRelay = new byte[] { SubNetID, DeviceID, (byte)(MyintDeviceType / 256), (byte)(MyintDeviceType % 256)
                        , (byte)MyActivePage,(byte)(mintIDIndex/256),(byte)(mintIDIndex%256) };
                    CsConst.MyUPload2DownLists.Add(ArayRelay);
                    CsConst.MyUpload2Down = Convert.ToByte(((ToolStripButton)sender).Tag.ToString());
                    FrmDownloadShow Frm = new FrmDownloadShow();
                    if (bytTag ==0) Frm.FormClosing += new FormClosingEventHandler(UpdateDisplayInformationAccordingly);
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
                case "tabBasic": showBasicInfo(); break;
                case "tabFTP": showFTPInfom(); break;
                case "tabFM": showFMInfo(); break;
                case "tabIn":showAudioInInfo();break;
                case "tabPDIF": showPDIFInfo(); break;
                case "tabKNX": showKNXInfo(); break;
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
                if (oMzBox == null) return;
                isRead = true;
                for (int i = 0; i < checklist.Items.Count; i++)
                {
                    if (oMzBox.arayEnable[i] == 1) checklist.SetItemChecked(i, true);
                    else checklist.SetItemChecked(i, false);
                }

                if (AudioDeviceTypeList.DinRailAudioDeviceType.Contains(MyintDeviceType)) //丁导轨系列
                {
                    checklist.SetItemChecked(3, (oMzBox.arayEnable[4] == 1));
                }
                for (int i = 0; i < 3; i++)
                {
                    CheckBox temp = this.Controls.Find("chb" + (i + 1).ToString(), true)[0] as CheckBox;
                    if (oMzBox.arayOther[i] == 1) temp.Checked = true;
                    else temp.Checked = false;
                }
                if (oMzBox.arayPA[0] == 1) chb4.Checked = true;
                else chb4.Checked = false;
                pnlPA.Enabled = chb4.Checked;
                if (oMzBox.arayPA[1] == 0) rb1.Checked = true;
                else if (oMzBox.arayPA[1] == 1) rb2.Checked = true;
                else if (oMzBox.arayPA[1] == 2) rb3.Checked = true;
                if (oMzBox.arayPA[2] == 1) chb5.Checked = true;
                else chb5.Checked = false;
                txt1.Text = oMzBox.arayPA[3].ToString();
                txt2.Text = oMzBox.arayPA[4].ToString();
                txt3.Text = oMzBox.arayPA[5].ToString();
                txt4.Text = oMzBox.arayPA[6].ToString();
            }
            catch
            {
            }
            isRead = false;
        }

        private void showFTPInfom()
        {
            try
            {
                if (oMzBox == null) return;
                if (oMzBox.myFTP == null) return;
                isRead = true;
                SeverIP.Text = oMzBox.myFTP.IP.ToString();
                txtName.Text = oMzBox.myFTP.strName;
                txtPassword.Text = oMzBox.myFTP.strPassword;
                if (oMzBox.myFTP.Encode < 2)
                    cbEncode.SelectedIndex = oMzBox.myFTP.Encode;
                else
                    cbEncode.SelectedIndex = 0;
                txtPath.Text = oMzBox.myFTP.strPath;
            }
            catch
            {
            }
            isRead = false;
        }

        private void showFMInfo()
        {
            try
            {
                dgvFM.Rows.Clear();
                if (oMzBox == null) return;
                if (oMzBox.myFM == null) return;
                isRead = true;
                for (int i = 0; i < oMzBox.myFM.Count; i++)
                {
                    string strFrequence = (oMzBox.myFM[i].Frequence / 10).ToString() + "." + (oMzBox.myFM[i].Frequence % 10).ToString();
                    string strEnable = clFM4.Items[0].ToString();
                    if (oMzBox.myFM[i].Enable < 2) strEnable = clFM4.Items[oMzBox.myFM[i].Enable].ToString();
                    object[] obj = new object[] { dgvFM.RowCount + 1, strFrequence, oMzBox.myFM[i].strRemark, strEnable };
                    dgvFM.Rows.Add(obj);
                }
            }
            catch
            {
            }
            isRead = false;
        }

        private void showAudioInInfo()
        {
            try
            {
                dgvIN.Rows.Clear();
                if (oMzBox == null) return;
                if (oMzBox.myAudioIN == null) return;
                isRead = true;
                if (oMzBox.AudioINCarrier == 0) rbIN1.Checked = true;
                else if (oMzBox.AudioINCarrier == 1) rbIN2.Checked = true;
                for (int i = 0; i < oMzBox.myAudioIN.Count; i++)
                {
                    string strHint = (i + 1).ToString();
                    if (i == 0) strHint = strHint + "-(ON)";
                    else if (i == 1) strHint = strHint + "-(OFF)";
                    else if (i == 2) strHint = strHint + "-(PlAY)";
                    else if (i == 3) strHint = strHint + "-(PAUSE)";
                    else if (i == 4) strHint = strHint + "-(STOP)";
                    else if (i == 5) strHint = strHint + "-(NEXT)";
                    else if (i == 6) strHint = strHint + "-(PREV)";
                    string strValid = CsConst.WholeTextsList[1775].sDisplayName;
                    if (oMzBox.myAudioIN[i].Enable == 1) strValid = CsConst.mstrINIDefault.IniReadValue("Public", "00042", "");
                    string strLen = oMzBox.myAudioIN[i].strCode.Trim().Replace(" ", "");
                    strLen = (strLen.Length / 2).ToString();
                    object[] obj = new object[] { strHint, strValid, strLen };
                    dgvIN.Rows.Add(obj);
                }
            }
            catch
            {
            }
            isRead = false;
        }

        private void showPDIFInfo()
        {
            try
            {
                dgvPDIF.Rows.Clear();
                if (oMzBox == null) return;
                if (oMzBox.myPDIF == null) return;
                isRead = true;
                if (oMzBox.PDIFCarrier == 0) rbPDIF1.Checked = true;
                else if (oMzBox.PDIFCarrier == 1) rbPDIF2.Checked = true;
                for (int i = 0; i < oMzBox.myPDIF.Count; i++)
                {
                    string strHint = (i + 1).ToString();
                    if (i == 0) strHint = strHint + "-(ON)";
                    else if (i == 1) strHint = strHint + "-(OFF)";
                    else if (i == 2) strHint = strHint + "-(PlAY)";
                    else if (i == 3) strHint = strHint + "-(PAUSE)";
                    else if (i == 4) strHint = strHint + "-(STOP)";
                    else if (i == 5) strHint = strHint + "-(NEXT)";
                    else if (i == 6) strHint = strHint + "-(PREV)";
                    string strValid = CsConst.WholeTextsList[1775].sDisplayName;
                    if (oMzBox.myPDIF[i].Enable == 1) strValid = CsConst.mstrINIDefault.IniReadValue("Public", "00042", "");
                    string strLen = oMzBox.myPDIF[i].strCode.Trim().Replace(" ", "");
                    strLen = (strLen.Length / 2).ToString();
                    object[] obj = new object[] { strHint, strValid, strLen };
                    dgvPDIF.Rows.Add(obj);
                }
            }
            catch
            {
            }
            isRead = false;
        }

        private void showKNXInfo()
        {
            try
            {
                isRead = true;

                if (Convert.ToInt32(oMzBox.PhysicalAddress.Split('.')[0].ToString()) < num1.Maximum)
                    num1.Value = Convert.ToInt32(oMzBox.PhysicalAddress.Split('.')[0].ToString());
                if (Convert.ToInt32(oMzBox.PhysicalAddress.Split('.')[1].ToString()) < num2.Maximum)
                    num2.Value = Convert.ToInt32(oMzBox.PhysicalAddress.Split('.')[1].ToString());
                if (Convert.ToInt32(oMzBox.PhysicalAddress.Split('.')[2].ToString()) <= num3.Maximum)
                    num3.Value = Convert.ToInt32(oMzBox.PhysicalAddress.Split('.')[2].ToString());

                dgvKNX.Rows.Clear();
                List<string[]> strAry = oMzBox.ReadKNXTargets();
                for (int i = 0; i < strAry.Count; i++)
                {
                    object[] obj = new object[] { strAry[i][0].ToString(), strAry[i][1].ToString(), 
                                                    strAry[i][3].ToString(), strAry[i][4].ToString()};
                    dgvKNX.Rows.Add(obj);
                }
                if (dgvKNX.Rows == null || dgvKNX.Rows.Count == 0) return;
                dgvKNX.Rows[DataGridViewSelectIndexOfKNX].Selected = true;
                dgvKNX.CurrentCell = dgvKNX.Rows[DataGridViewSelectIndexOfKNX].Cells[0];
                dgvKNX_CellClick(dgvKNX, new DataGridViewCellEventArgs(0, DataGridViewSelectIndexOfKNX));
            }
            catch
            {
            }
            isRead = false;
        }

        private void chb4_CheckedChanged(object sender, EventArgs e)
        {
            pnlPA.Enabled = chb4.Checked;
            if (isRead) return;
            if (oMzBox == null) return;
            if (chb4.Checked) oMzBox.arayPA[0] = 1;
            else oMzBox.arayPA[0] = 0;

        }

        private void txt1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar) || e.KeyChar == 8)
                e.Handled = false;
            else
                e.Handled = true;
        }

        private void txt1_Leave(object sender, EventArgs e)
        {
            string str = (sender as TextBox).Text;
            (sender as TextBox).Text = HDLPF.IsNumStringMode(str, 0, 255);
            (sender as TextBox).SelectionStart = (sender as TextBox).Text.Length;
            if (isRead) return;
            if (oMzBox == null) return;
            oMzBox.arayPA[3] = Convert.ToByte(txt1.Text);
            oMzBox.arayPA[4] = Convert.ToByte(txt2.Text);
            oMzBox.arayPA[5] = Convert.ToByte(txt3.Text);
            oMzBox.arayPA[6] = Convert.ToByte(txt4.Text);
        }

        private void checklist_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (isRead) return;
            if (oMzBox == null) return;
            if (e.NewValue == CheckState.Checked) oMzBox.arayEnable[e.Index] = 1;
            else oMzBox.arayEnable[e.Index] = 0;

            if (AudioDeviceTypeList.DinRailAudioDeviceType.Contains(MyintDeviceType) && e.Index == 3)
            {
                oMzBox.arayEnable[e.Index + 1] = oMzBox.arayEnable[e.Index];
            }
        }

        private void chb1_CheckedChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            if (oMzBox == null) return;
            if (chb1.Checked)
                oMzBox.arayOther[0] = 1;
            else
                oMzBox.arayOther[0] = 0;
            if (chb2.Checked)
                oMzBox.arayOther[1] = 1;
            else
                oMzBox.arayOther[1] = 0;
            if (chb3.Checked)
                oMzBox.arayOther[2] = 1;
            else
                oMzBox.arayOther[2] = 0;
        }

        private void chb5_CheckedChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            if (oMzBox == null) return;
            if (chb5.Checked) oMzBox.arayPA[2] = 1;
            else oMzBox.arayPA[2] = 0;
        }

        private void rb1_CheckedChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            if (oMzBox == null) return;
            if (rb1.Checked) oMzBox.arayPA[1] = 0;
            else if (rb2.Checked) oMzBox.arayPA[1] = 1;
            else if (rb3.Checked) oMzBox.arayPA[1] = 2;
        }

        private void btnFomatting_Click(object sender, EventArgs e)
        {
            byte[] arayTmp = new byte[1]{1};
            UDPReceive.receiveQueue = new Queue<byte[]>();
            UDPReceive.ClearQueueData();
            ProcessValue = 0;
            CsConst.mySends.AddBufToSndList(arayTmp, 0x1384, SubNetID, DeviceID, false, false, false, false);
            timer1.Enabled = true;
            frmProcessTmp = new FrmProcess();
            frmProcessTmp.ShowDialog();
        }

        private void SeverIP_UserControlValueChanged(object sender, IPAddressNew.TextChangeEventArgs e)
        {
            if (isRead) return;
            if (oMzBox == null) return;
            if (oMzBox.myFTP == null) return;
            oMzBox.myFTP.IP = SeverIP.Text;
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            if (oMzBox == null) return;
            if (oMzBox.myFTP == null) return;
            oMzBox.myFTP.strName = txtName.Text;
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            if (oMzBox == null) return;
            if (oMzBox.myFTP == null) return;
            oMzBox.myFTP.strPassword = txtPassword.Text;
        }

        private void cbEncode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            if (oMzBox == null) return;
            if (oMzBox.myFTP == null) return;
            oMzBox.myFTP.Encode = Convert.ToByte(cbEncode.SelectedIndex);
        }

        private void txtPath_TextChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            if (oMzBox == null) return;
            if (oMzBox.myFTP == null) return;
            oMzBox.myFTP.strPath = txtPath.Text;
        }

        private void btnRefreshPage1_Click(object sender, EventArgs e)
        {
            tsbDown_Click(tsbDown, null);
        }

        private void btnSavePage1_Click(object sender, EventArgs e)
        {
            tsbDown_Click(tsbUpload, null);
        }

        private void btnSaveAndClose1_Click(object sender, EventArgs e)
        {
            tsbDown_Click(tsbUpload, null);
            this.Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            ProcessValue = ProcessValue + 1;
            if (ProcessValue >= 100)
            {
                frmProcessTmp.Close();
                timer1.Enabled = false;
                MessageBox.Show(CsConst.mstrINIDefault.IniReadValue("Public", "99910", ""), "", MessageBoxButtons.OK, 
                    MessageBoxIcon.Warning,MessageBoxDefaultButton.Button1);               
            }
            else
            {
                if (UDPReceive.receiveQueue.Count > 0)
                {
                    byte[] arayTmp = UDPReceive.receiveQueue.Dequeue();
                    if (arayTmp[17] == SubNetID && arayTmp[18] == DeviceID &&
                       arayTmp[21] == 0x13 && arayTmp[22] == 0x85)
                    {
                        if (arayTmp[25] == 2)
                        {
                            MessageBox.Show(CsConst.mstrINIDefault.IniReadValue("Public", "99909", ""), "", MessageBoxButtons.OK,
                                            MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                            frmProcessTmp.Close();
                            timer1.Enabled = false;
                        }
                    }
                }
            }
        }

        private void dgvFM_SizeChanged(object sender, EventArgs e)
        {
            HDLSysPF.setDataGridViewColumnsWidth(dgvFM);
        }

        private void dgvFM_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            dgvFM.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void dgvFM_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (isRead) return;
                if (oMzBox == null) return;
                if (oMzBox.myFM == null) return;
                if ((e.RowIndex == -1) || (e.ColumnIndex == -1)) return;
                if (dgvFM[e.ColumnIndex, e.RowIndex].Value == null) dgvFM[e.ColumnIndex, e.RowIndex].Value = "";
                for (int i = 0; i < dgvFM.SelectedRows.Count; i++)
                {
                    dgvFM.SelectedRows[i].Cells[e.ColumnIndex].Value = dgvFM[e.ColumnIndex, e.RowIndex].Value.ToString();

                    string strTmp = "";
                    switch (e.ColumnIndex)
                    {
                        case 1:
                            strTmp = dgvFM[1, dgvFM.SelectedRows[i].Index].Value.ToString();
                            oMzBox.myFM[dgvFM.SelectedRows[i].Index].Frequence = Convert.ToInt32(HDLPF.GetTimeFromString(strTmp, '.'));
                            break;
                        case 2:
                            strTmp = dgvFM[2, dgvFM.SelectedRows[i].Index].Value.ToString();
                            dgvFM[2, dgvFM.SelectedRows[i].Index].Value = HDLPF.IsRightStringMode(strTmp);
                            oMzBox.myFM[dgvFM.SelectedRows[i].Index].strRemark = dgvFM[2, dgvFM.SelectedRows[i].Index].Value.ToString();
                            break;
                        case 3:
                            oMzBox.myFM[dgvFM.SelectedRows[i].Index].Enable = Convert.ToByte(clFM4.Items.IndexOf(dgvFM[3, dgvFM.SelectedRows[i].Index].Value.ToString()));
                            break;
                    }
                }
                if (e.ColumnIndex == 1)
                {
                    string strTmp = dgvFM[1, e.RowIndex].Value.ToString();
                    oMzBox.myFM[e.RowIndex].Frequence = Convert.ToInt32(HDLPF.GetTimeFromString(strTmp, '.'));
                }
                if (e.ColumnIndex == 2)
                {
                    oMzBox.myFM[e.RowIndex].strRemark = dgvFM[2, e.RowIndex].Value.ToString();
                }
                if (e.ColumnIndex == 3)
                {
                    oMzBox.myFM[e.RowIndex].Enable = Convert.ToByte(clFM4.Items.IndexOf(dgvFM[3, e.RowIndex].Value.ToString()));
                }
            }
            catch
            {
            }
        }

        private void rbIN1_CheckedChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            if (oMzBox == null) return;
            Cursor.Current = Cursors.WaitCursor;
            byte[] arayTmp = new byte[3];
            arayTmp[0] = 5;
            arayTmp[1] = 1;
            if (rbIN1.Checked) arayTmp[2] = 0;
            else if (rbIN2.Checked) arayTmp[2] = 1;
            if (CsConst.mySends.AddBufToSndList(arayTmp, 0x1360, SubNetID, DeviceID, false, true, true, false) == false)
            {
                oMzBox.AudioINCarrier = arayTmp[2];
                CsConst.myRevBuf = new byte[1200];
            }
            else
            {
                Cursor.Current = Cursors.Default;
                return;
            }
            Cursor.Current = Cursors.Default;
        }

        private void tvIN_AfterCheck(object sender, TreeViewEventArgs e)
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

        private void btnLearnerIN_Click(object sender, EventArgs e)
        {
            frmIRlearner frmTmp = new frmIRlearner();
            
            frmTmp.FormClosed += frmTmp1_FormClosed;
            frmTmp.ShowDialog();
        }

        void frmTmp1_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                HDLSysPF.addIR(tvIN, tempSend1, true);  // 添加已有的列表到窗体
            }
            catch
            {
            }
        }

        private void btnUploadIN_Click(object sender, EventArgs e)
        {
            try
            {
                isStopDownloadCodes = false;
                MyBackGroup = new BackgroundWorker();
                MyBackGroup.DoWork += new DoWorkEventHandler(calculationWorker_DoWork);
                MyBackGroup.ProgressChanged += new ProgressChangedEventHandler(calculationWorker_ProgressChanged);
                MyBackGroup.WorkerReportsProgress = true;
                MyBackGroup.RunWorkerCompleted += new RunWorkerCompletedEventHandler(calculationWorker_RunWorkerCompleted);
                MyBackGroup.RunWorkerAsync();
                MyBackGroup.WorkerSupportsCancellation = true;
                frmProcessTmp = new FrmProcess();
                frmProcessTmp.ShowDialog();
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
                frmProcessTmp.Close();
            }
            catch
            {
            }
        }

        void calculationWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            try
            {
               
            }
            catch
            {
            }
        }

        void calculationWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                List<string> listCode = new List<string>();
                int RowIndex = dgvIN.CurrentRow.Index;
                int RowCount = dgvIN.RowCount;
                Boolean bIsSelectedWholeGroup = false;
                for (int i = 0; i < tvIN.Nodes.Count; i++)
                {
                    bIsSelectedWholeGroup = tvIN.Nodes[i].Checked;

                    for (int j = 0; j < tvIN.Nodes[i].Nodes.Count; j++)
                    {
                        if (tvIN.Nodes[i].Nodes[j].Checked || bIsSelectedWholeGroup == true)
                        {
                            int DeviceID = Convert.ToInt32(tvIN.Nodes[i].Name);
                            int KeyID = Convert.ToInt32(tvIN.Nodes[i].Nodes[j].Name);
                            string strCodes = "";
                            for (int k = 0; k < tempSend1.IRCodes.Count; k++)
                            {
                                if (tempSend1.IRCodes[k].KeyID == KeyID && tempSend1.IRCodes[k].IRLoation == DeviceID)
                                {
                                    strCodes = tempSend1.IRCodes[k].Codes;
                                    break;
                                }
                            }
                            if (strCodes != "")
                            {
                                listCode.Add(strCodes);
                            }
                        }
                    }
                }
                int CodeIndex = 0;
                int FirstIndex = dgvIN.CurrentRow.Index;
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
                    byte[] arayCode = new byte[ListData.Length * 14];
                    for (int j = 0; j < ListData.Length; j++)
                    {
                        byte[] arayCodeTmp = GlobalClass.HexToByte(ListData[j]);
                        Array.Copy(arayCodeTmp, 2, arayCode, 14 * j, 14);
                    }

                    dgvIN.Rows[FirstIndex + CodeIndex].Cells[1].Value = CsConst.mstrINIDefault.IniReadValue("public", "00042", "");
                    dgvIN.Rows[FirstIndex + CodeIndex].Cells[2].Value = (arayCode[0] * 256 + arayCode[1]).ToString();

                    oMzBox.myAudioIN[FirstIndex + CodeIndex].Length = arayCode[0] * 256 + arayCode[1];
                    strCodes = "";
                    foreach (Byte bTmp in arayCode)
                    {
                        strCodes += bTmp.ToString("X2") + " ";
                    }
                    if (oMzBox.myAudioIN[FirstIndex + CodeIndex].Length > 0)
                    {
                        oMzBox.myAudioIN[FirstIndex + CodeIndex].Enable = 1;
                        oMzBox.myAudioIN[FirstIndex + CodeIndex].strCode = strCodes.Trim();
                    }

                    CodeIndex = CodeIndex + 1;
                    dgvIN.Rows[FirstIndex + CodeIndex].Selected = true;
                    this.dgvIN.CurrentCell = this.dgvIN.Rows[FirstIndex + CodeIndex].Cells[0];
                    if (isStopDownloadCodes) break;
                    if (CodeIndex >= listCode.Count) break;
                }
            }
            catch
            {
            }
        }

        private void btnStopIN_Click(object sender, EventArgs e)
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

        private void rbPDIF1_CheckedChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            if (oMzBox == null) return;
            Cursor.Current = Cursors.WaitCursor;
            byte[] arayTmp = new byte[3];
            arayTmp[0] = 0x15;
            arayTmp[1] = 1;
            if (rbPDIF1.Checked) arayTmp[2] = 0;
            else if (rbPDIF2.Checked) arayTmp[2] = 1;
            if (CsConst.mySends.AddBufToSndList(arayTmp, 0x1360, SubNetID, DeviceID, false, true, true, false) == false)
            {
                oMzBox.PDIFCarrier = arayTmp[2];
                CsConst.myRevBuf = new byte[1200];
            }
            else
            {
                Cursor.Current = Cursors.Default;
                return;
            }
            Cursor.Current = Cursors.Default;
        }

        private void btnLearnerPDIF_Click(object sender, EventArgs e)
        {
            frmIRlearner frmTmp = new frmIRlearner();
            
            frmTmp.FormClosed += frmTmp2_FormClosed;
            frmTmp.ShowDialog();
        }


        void frmTmp2_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                HDLSysPF.addIR(tvPDIF, tempSend2, true);  // 添加已有的列表到窗体
            }
            catch
            {
            }
        }

        private void btnUploadPDIF_Click(object sender, EventArgs e)
        {
            try
            {
                isStopDownloadCodes = false;
                MyBackGroup = new BackgroundWorker();
                MyBackGroup.DoWork += new DoWorkEventHandler(calculationWorker_DoWork2);
                MyBackGroup.ProgressChanged += new ProgressChangedEventHandler(calculationWorker_ProgressChanged2);
                MyBackGroup.WorkerReportsProgress = true;
                MyBackGroup.RunWorkerCompleted += new RunWorkerCompletedEventHandler(calculationWorker_RunWorkerCompleted2);
                MyBackGroup.RunWorkerAsync();
                MyBackGroup.WorkerSupportsCancellation = true;
                frmProcessTmp = new FrmProcess();
                frmProcessTmp.ShowDialog();
            }
            catch
            {
            }
            Cursor.Current = Cursors.Default;
        }

        void calculationWorker_RunWorkerCompleted2(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                frmProcessTmp.Close();
            }
            catch
            {
            }
        }

        void calculationWorker_ProgressChanged2(object sender, ProgressChangedEventArgs e)
        {
            try
            {

            }
            catch
            {
            }
        }

        void calculationWorker_DoWork2(object sender, DoWorkEventArgs e)
        {
            try
            {
                List<string> listCode = new List<string>();
                int RowIndex = dgvPDIF.CurrentRow.Index;
                int RowCount = dgvPDIF.RowCount;
                Boolean bIsSelectedGroup = false;
                for (int i = 0; i < tvPDIF.Nodes.Count; i++)
                {
                    bIsSelectedGroup = tvPDIF.Nodes[i].Checked;

                    for (int j = 0; j < tvPDIF.Nodes[i].Nodes.Count; j++)
                    {
                        if (bIsSelectedGroup || tvPDIF.Nodes[i].Nodes[j].Checked)
                        {
                            int DeviceID = Convert.ToInt32(tvPDIF.Nodes[i].Name);
                            int KeyID = Convert.ToInt32(tvPDIF.Nodes[i].Nodes[j].Name);
                            string strCodes = "";
                            for (int k = 0; k < tempSend2.IRCodes.Count; k++)
                            {
                                if (tempSend2.IRCodes[k].KeyID == KeyID && tempSend2.IRCodes[k].IRLoation == DeviceID)
                                {
                                    strCodes = tempSend2.IRCodes[k].Codes;
                                    break;
                                }
                            }
                            if (strCodes != "")
                            {
                                listCode.Add(strCodes);
                            }
                        }
                    }
                }
                int CodeIndex = 0;
                int FirstIndex = dgvPDIF.CurrentRow.Index;
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
                    byte[] arayCode = new byte[ListData.Length * 14];
                    for (int j = 0; j < ListData.Length; j++)
                    {
                        byte[] arayCodeTmp = GlobalClass.HexToByte(ListData[j]);
                        Array.Copy(arayCodeTmp, 2, arayCode, 14 * j, 14);
                    }

                    dgvPDIF.Rows[FirstIndex + CodeIndex].Cells[1].Value = CsConst.mstrINIDefault.IniReadValue("public", "00042", "");
                    dgvPDIF.Rows[FirstIndex + CodeIndex].Cells[2].Value = (arayCode[0] * 256 + arayCode[1]).ToString();

                    oMzBox.myPDIF[FirstIndex + CodeIndex].Length = arayCode[0] * 256 + arayCode[1];

                    strCodes = "";
                    foreach (Byte bTmp in arayCode)
                    {
                        strCodes += bTmp.ToString("X2") + " ";
                    }
                    if (oMzBox.myPDIF[FirstIndex + CodeIndex].Length > 0)
                    {
                        oMzBox.myPDIF[FirstIndex + CodeIndex].Enable = 1;
                        oMzBox.myPDIF[FirstIndex + CodeIndex].strCode = strCodes.Trim();
                    }
                    CodeIndex = CodeIndex + 1;
                    dgvPDIF.Rows[FirstIndex + CodeIndex].Selected = true;
                    this.dgvPDIF.CurrentCell = this.dgvPDIF.Rows[FirstIndex + CodeIndex].Cells[0];
                    if (isStopDownloadCodes) break;
                    if (CodeIndex >= listCode.Count) break;
                }
            }
            catch
            {
            }
        }


        private void btnStopPDIF_Click(object sender, EventArgs e)
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

        private void dgvKNX_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewSelectIndexOfKNX = e.RowIndex;
            if (isRead) return;
            if (e.RowIndex < 0) return;
            dgvAddress.Rows.Clear();
            int LenUnite=(oMzBox.arayUniteInEIB.Length/4);
            for (int i = 0; i < LenUnite; i++)
            {
                int intObjectID = oMzBox.arayUniteInEIB[i, 2] * 256 + oMzBox.arayUniteInEIB[i, 3];
                int GroupAddressID = oMzBox.arayUniteInEIB[i, 0] * 256 + oMzBox.arayUniteInEIB[i, 1];
                if(intObjectID<98 && intObjectID==e.RowIndex)
                {
                    int LenGroupAddress = (oMzBox.arayGroupAddressInEIB.Length / 2);
                    for (int j = 0; j < LenGroupAddress; j++)
                    {
                        if (GroupAddressID >= 1 && GroupAddressID <= LenGroupAddress)
                        {
                            string strTmp = GlobalClass.AddLeftZero(Convert.ToString(oMzBox.arayGroupAddressInEIB[GroupAddressID - 1, 0], 2), 8);
                            strTmp = strTmp.Substring(1, 7);
                            string str1 = strTmp.Substring(0, 4);
                            string str2 = strTmp.Substring(4, 3);
                            int int1 = Convert.ToInt32(str1, 2);
                            int int2 = Convert.ToInt32(str2, 2);
                            string str3 = Convert.ToString(oMzBox.arayGroupAddressInEIB[GroupAddressID - 1, 1]);
                            string strGroup = int1.ToString() + "/" + int2.ToString() + "/" + str3;
                            bool isAdd = true;
                            for (int k = 0; k < dgvAddress.RowCount; k++)
                            {
                                if (dgvAddress[1, k].Value.ToString() == strGroup)
                                {
                                    isAdd = false;
                                    break;
                                }
                            }
                            if (isAdd)
                            {
                                if (strGroup != "0/0/0")
                                {
                                    object[] obj = new object[] { dgvAddress.RowCount + 1, strGroup, 
                                        CsConst.mstrINIDefault.IniReadValue("public", "99810", "") };
                                    dgvAddress.Rows.Add(obj);
                                }
                            }
                        }
                    }
                }
            }
        }

        private void num1_ValueChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            if (oMzBox == null) return;
            oMzBox.PhysicalAddress = (Convert.ToInt32(num1.Value)).ToString() + "." + (Convert.ToInt32(num2.Value)).ToString() + "." + (Convert.ToInt32(num3.Value)).ToString();
        }

        private void dgvAddress_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == 2)
                {
                    int ObjectID = dgvKNX.CurrentRow.Index;
                    string strTmp = dgvAddress[1, e.RowIndex].Value.ToString();
                    List<object[]> objlist = new List<object[]>();
                    for (int i = 0; i < dgvAddress.Rows.Count; i++)
                    {
                        object[] obj = new object[] { dgvAddress[0, i].Value.ToString() ,dgvAddress[1,i].Value.ToString(),
                                                      dgvAddress[2,i].Value.ToString()};
                        objlist.Add(obj);
                    }
                    objlist.RemoveAt(e.RowIndex);
                    dgvAddress.Rows.Clear();
                    for (int i = 0; i < objlist.Count; i++)
                    {
                        Object[] obj = new object[] { (i + 1).ToString(), objlist[i][1].ToString(), objlist[i][2].ToString() };
                        dgvAddress.Rows.Add(obj);
                    }

                    for (int i = 0; i < oMzBox.arayUniteInEIB.Length / 4; i++)
                    {
                        string strUniteAddress = GlobalClass.AddLeftZero(Convert.ToString(oMzBox.arayUniteInEIB[i, 0], 2), 8).Substring(1, 7);
                        string str1 = Convert.ToInt32(strUniteAddress.Substring(0, 4), 2).ToString();
                        string str2 = Convert.ToInt32(strUniteAddress.Substring(4, 3), 2).ToString();
                        string str3 = Convert.ToString(oMzBox.arayUniteInEIB[i, 1]);
                        strUniteAddress = str1 + "/" + str2 + "/" + str3;
                        int ObjectIDTmp = oMzBox.arayUniteInEIB[i, 2] * 256 + oMzBox.arayUniteInEIB[i, 3];
                        if (strUniteAddress == strTmp && ObjectID == ObjectIDTmp)
                        {
                            oMzBox.arayUniteInEIB[i, 0] = 0;
                            oMzBox.arayUniteInEIB[i, 1] = 0;
                            oMzBox.arayUniteInEIB[i, 2] = 255;
                            oMzBox.arayUniteInEIB[i, 3] = 255;
                        }
                    }
                }
            }
            catch
            {
            }
        }

        private void btnKNX2_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current=Cursors.WaitCursor;
                btnKNX1_Click(null, null);
                for (int i = 1; i < 99; i++)
                {
                    numA3.Value = i;
                    dgvKNX.Rows[i - 1].Selected = true;
                    dgvKNX.CurrentCell = dgvKNX.Rows[i - 1].Cells[0];
                    dgvKNX_CellClick(dgvKNX, new DataGridViewCellEventArgs(0, i - 1));
                    btnKNX3_Click(null, null);
                }
            }
            catch
            {
            }
            Cursor.Current = Cursors.Default;
        }

        private void btnKNX1_Click(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < oMzBox.arayGroupAddressInEIB.Length / 2; i++)
                {
                    oMzBox.arayGroupAddressInEIB[i, 0] = 0;
                    oMzBox.arayGroupAddressInEIB[i, 1] = 0;
                }
                for (int i = 0; i < oMzBox.arayGroupAddressInEIB.Length / 4; i++)
                {
                    oMzBox.arayUniteInEIB[i, 0] = 0;
                    oMzBox.arayUniteInEIB[i, 1] = 0;
                    oMzBox.arayUniteInEIB[i, 2] = 255;
                    oMzBox.arayUniteInEIB[i, 3] = 255;
                }
            }
            catch
            {
            }
        }

        private void btnKNX3_Click(object sender, EventArgs e)
        {
            try
            {
                string strGroupAddress = numA1.Value.ToString() + "/" + numA2.Value.ToString() + "/" + numA3.Value.ToString();
                if (strGroupAddress == "0/0/0")
                {
                    MessageBox.Show(CsConst.mstrINIDefault.IniReadValue("Public", "99567", "")
                        , "", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                    return;
                }
                for (int i = 0; i < dgvAddress.RowCount; i++)
                {
                    if (strGroupAddress == dgvAddress[1, i].Value.ToString())
                    {
                        MessageBox.Show(CsConst.mstrINIDefault.IniReadValue("Public", "99566", "")
                      , "", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                        return;
                    }
                }
                object[] obj = new object[] { dgvAddress.RowCount + 1, strGroupAddress, CsConst.mstrINIDefault.IniReadValue("public", "99810", "") };
                dgvAddress.Rows.Add(obj);
                string str1 = strGroupAddress.Split('/')[0].ToString();
                string str2 = strGroupAddress.Split('/')[1].ToString();
                string str3 = strGroupAddress.Split('/')[2].ToString();
                int int1 = Convert.ToInt32(str1);
                int int2 = Convert.ToInt32(str2);
                int int3 = Convert.ToInt32(str3);
                str1 = GlobalClass.AddLeftZero(Convert.ToString(int1, 2), 4);
                str2 = GlobalClass.AddLeftZero(Convert.ToString(int2, 2), 3);
                string strTmp = "0" + str1 + str2;
                byte byt1 = Convert.ToByte(strTmp, 2);
                byte byt2 = Convert.ToByte(str3);
                int IntTmp = 0;
                for (int i = 0; i < oMzBox.arayGroupAddressInEIB.Length / 2; i++)
                {
                    if (!(byt1 == oMzBox.arayGroupAddressInEIB[i, 0] && byt2 == oMzBox.arayGroupAddressInEIB[i, 1]))
                    {
                        if (oMzBox.arayGroupAddressInEIB[i, 0] == 255 && oMzBox.arayGroupAddressInEIB[i, 1] == 255)
                        {
                            oMzBox.arayGroupAddressInEIB[i, 0] = byt1;
                            oMzBox.arayGroupAddressInEIB[i, 1] = byt2;
                            IntTmp = i;
                            break;
                        }
                    }
                    else
                    {
                        IntTmp = i;
                        break;
                    }
                }
                int ObjectID=dgvKNX.CurrentRow.Index;
                for (int i = 0; i < oMzBox.arayUniteInEIB.Length / 4; i++)
                {
                    if (byt1 == oMzBox.arayUniteInEIB[i, 0] && byt2 == oMzBox.arayUniteInEIB[i, 1] &&
                        ObjectID == (oMzBox.arayUniteInEIB[i, 2] * 256 + oMzBox.arayUniteInEIB[i, 3]))
                    {
                        oMzBox.arayUniteInEIB[i, 0] = Convert.ToByte((IntTmp + 1) / 256);
                        oMzBox.arayUniteInEIB[i, 1] = Convert.ToByte((IntTmp + 1) % 256);
                        oMzBox.arayUniteInEIB[i, 2] = Convert.ToByte(ObjectID / 256);
                        oMzBox.arayUniteInEIB[i, 3] = Convert.ToByte(ObjectID % 256);
                        break;
                    }
                    else
                    {
                        if (oMzBox.arayUniteInEIB[i, 0] == 0 && oMzBox.arayUniteInEIB[i, 1] == 0)
                        {
                            oMzBox.arayUniteInEIB[i, 0] = Convert.ToByte((IntTmp + 1) / 256);
                            oMzBox.arayUniteInEIB[i, 1] = Convert.ToByte((IntTmp + 1) % 256);
                            oMzBox.arayUniteInEIB[i, 2] = Convert.ToByte(ObjectID / 256);
                            oMzBox.arayUniteInEIB[i, 3] = Convert.ToByte(ObjectID % 256);
                            break;
                        }
                        if ((oMzBox.arayUniteInEIB[i, 2] * 256 + oMzBox.arayUniteInEIB[i, 3]) > 97)
                        {
                            oMzBox.arayUniteInEIB[i, 0] = Convert.ToByte((IntTmp + 1) / 256);
                            oMzBox.arayUniteInEIB[i, 1] = Convert.ToByte((IntTmp + 1) % 256);
                            oMzBox.arayUniteInEIB[i, 2] = Convert.ToByte(ObjectID / 256);
                            oMzBox.arayUniteInEIB[i, 3] = Convert.ToByte(ObjectID % 256);
                            break;
                        }
                    }
                }
            }
            catch
            {
            }
        }

        private void btnKNX4_Click(object sender, EventArgs e)
        {
            try
            {
                int ObjectID = dgvKNX.CurrentRow.Index;
                for (int i = 0; i < oMzBox.arayUniteInEIB.Length / 4; i++)
                {
                    int objectIDTmp = oMzBox.arayUniteInEIB[i, 2] * 256 + oMzBox.arayUniteInEIB[i, 3];
                    if (ObjectID == objectIDTmp)
                    {
                        oMzBox.arayUniteInEIB[i, 0] = 0;
                        oMzBox.arayUniteInEIB[i, 1] = 0;
                        oMzBox.arayUniteInEIB[i, 2] = 255;
                        oMzBox.arayUniteInEIB[i, 3] = 255;
                    }
                }
            }
            catch
            {
            }
        }

        private void rbTets1_CheckedChanged(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            byte bytTag = 0;
            if (rbTets1.Checked) bytTag = 6;
            else if (rbTets2.Checked) bytTag = 5;
            else if (rbTets3.Checked) bytTag = 2;
            else if (rbTets4.Checked) bytTag = 8;
            else if (rbTets5.Checked) bytTag = 1;
            else if (rbTets6.Checked) bytTag = 9;
            string strTmp = "*Z1SRC" + bytTag.ToString();
            byte[] arayTmp = new byte[strTmp.Length + 1];
            byte[] arayRemark = HDLUDP.StringToByte(strTmp);
            Array.Copy(arayRemark, 0, arayTmp, 0, arayRemark.Length);
            arayTmp[arayTmp.Length - 1] = 13;
            if (CsConst.mySends.AddBufToSndList(arayTmp, 0x192E, SubNetID, DeviceID, false, true, true, false) == true)
            {
                CsConst.myRevBuf = new byte[1200];
            }
            else
            {
                Cursor.Current = Cursors.Default;
                return;
            }
            Cursor.Current = Cursors.Default;
        }

        private void btnSpace_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            byte[] arayTmp = new byte[1];
            arayTmp[0] = 11;
            if (CsConst.mySends.AddBufToSndList(arayTmp, 0x1364, SubNetID, DeviceID, true, true, true, false) == true)
            {
                string str1 = GlobalClass.AddLeftZero(CsConst.myRevBuf[28].ToString("X"), 2) +
                              GlobalClass.AddLeftZero(CsConst.myRevBuf[29].ToString("X"), 2) +
                              GlobalClass.AddLeftZero(CsConst.myRevBuf[30].ToString("X"), 2) +
                              GlobalClass.AddLeftZero(CsConst.myRevBuf[31].ToString("X"), 2);

                string str2 = GlobalClass.AddLeftZero(CsConst.myRevBuf[33].ToString("X"), 2) +
                              GlobalClass.AddLeftZero(CsConst.myRevBuf[34].ToString("X"), 2) +
                              GlobalClass.AddLeftZero(CsConst.myRevBuf[35].ToString("X"), 2) +
                              GlobalClass.AddLeftZero(CsConst.myRevBuf[36].ToString("X"), 2);
                UInt64 num1 = Convert.ToUInt64(str1, 16);
                UInt64 num2 = Convert.ToUInt64(str2, 16);
                lbSDSpace.Text = CsConst.mstrINIDefault.IniReadValue("Public", "99564", "") + " " + ((num2 / 1024)).ToString() + "(M)" + "  " +
                               CsConst.mstrINIDefault.IniReadValue("Public", "99565", "") + " " + ((num1 / 1024)).ToString() + "(M)";

            }
            else
            {
                Cursor.Current = Cursors.Default;
                return;
            }
            Cursor.Current = Cursors.Default;
        }

        private void btnSD_Click(object sender, EventArgs e)
        {
            tvList.Visible = false;
            tvSD.Visible = true;
            btnList.Dock = DockStyle.Bottom;
            tableLayoutPanel2.Visible = false;
            tvSD.Dock = DockStyle.Fill;
            try
            {
                dgvSong.Rows.Clear();
                ReadFileIndex = 0;
                AudioDeviceTypeList.isEndReceiveFile = false;
                strFileList = new List<string>();
                UDPReceive.receiveQueueForAudio = new Queue<byte[]>();
                byte[] arayTmp = new byte[]{12,0x2F,0X0D};
                CsConst.mySends.AddBufToSndList(arayTmp, 0x1364, SubNetID, DeviceID, true, false, false, false);
                MyBackGroup = new BackgroundWorker();
                MyBackGroup.DoWork += new DoWorkEventHandler(calculationWorker_DoWork4);
                MyBackGroup.ProgressChanged += new ProgressChangedEventHandler(calculationWorker_ProgressChanged4);
                MyBackGroup.WorkerReportsProgress = true;
                MyBackGroup.RunWorkerCompleted += new RunWorkerCompletedEventHandler(calculationWorker_RunWorkerCompleted4);
                MyBackGroup.RunWorkerAsync();
                MyBackGroup.WorkerSupportsCancellation = true;
                frmProcessTmp = new FrmProcess();
                frmProcessTmp.ShowDialog();
            }
            catch
            {
            }
            Cursor.Current = Cursors.Default;
        }

        void calculationWorker_RunWorkerCompleted4(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                if (ReadFileIndex == 0)
                {
                    tvSD.Nodes.Clear();
                    for (int i = 0; i < strFileList.Count; i++)
                    {
                        if (strFileList[i].Contains(".mp3") || strFileList[i].Contains(".MP3"))
                        {
                            tvSD.Nodes.Add("1", strFileList[i], 1, 1);
                        }
                        else
                        {
                            tvSD.Nodes.Add("0", strFileList[i], 0, 0);
                        }
                    }
                }
                else if (ReadFileIndex == 1)
                {
                    SelectNode.Nodes.Clear();
                    for (int i = 0; i < strFileList.Count; i++)
                    {
                        if (strFileList[i].Contains(".mp3") || strFileList[i].Contains(".MP3"))
                        {
                            SelectNode.Nodes.Add("1", strFileList[i], 1, 1);
                        }
                        else
                        {
                            SelectNode.Nodes.Add("0", strFileList[i], 0, 0);
                        }
                    }
                    if (SelectNode.Nodes.Count > 0)
                    {
                        SelectNode.Expand();
                    }
                }
                frmProcessTmp.Close();
            }
            catch
            {
            }
        }

        void calculationWorker_ProgressChanged4(object sender, ProgressChangedEventArgs e)
        {
            try
            {

            }
            catch
            {
            }
        }

        void calculationWorker_DoWork4(object sender, DoWorkEventArgs e)
        {
            try
            {
                DateTime d1 = DateTime.Now;
                DateTime d2 = DateTime.Now;
                while (!AudioDeviceTypeList.isEndReceiveFile)
                {
                    d2 = DateTime.Now;
                    if (HDLSysPF.Compare(d2, d1) > 10000)
                    {
                        break;
                    }
                }
                if (UDPReceive.receiveQueueForAudio.Count > 0)
                {
                    List<byte> bytAry = new List<byte>();
                    for (int i = 0; i < UDPReceive.receiveQueueForAudio.Count; i++)
                    {
                        byte[] arayTmp = UDPReceive.receiveQueueForAudio.ToArray()[i];
                        if (i == 0)
                        {
                            if (ReadFileIndex == 0)
                            {
                                Byte Test = 13;
                                int NameBeginPosition = Array.IndexOf(arayTmp, Test);
                                for (int j = NameBeginPosition + 1; j < arayTmp.Length; j++)
                                {
                                    bytAry.Add(arayTmp[j]);
                                }
                            }
                            else if (ReadFileIndex == 1)
                            {
                                for (int j = LengReadFileStrLength; j < arayTmp.Length; j++)
                                {
                                    bytAry.Add(arayTmp[j]);
                                }
                            }
                        }
                        else
                        {
                            if (ReadFileIndex == 1)
                            {
                                for (int j = LengReadFileStrLength; j < arayTmp.Length; j++)
                                {
                                    bytAry.Add(arayTmp[j]);
                                }
                            }
                            else
                            {
                                for (int j = 3; j < arayTmp.Length; j++)
                                {
                                    bytAry.Add(arayTmp[j]);
                                }
                            }
                        }
                    }
                    int index1 = 0;
                    int index2 = 0;
                    strFileList = new List<string>();

                    for (int i = 0; i < bytAry.Count - 1; i++)
                    {
                        if (bytAry[i] == 0x0D && bytAry[i + 1] == 0x0A)
                        {
                            index2 = i;
                            if (index1 != index2 && index2 > index1)
                            {
                                byte[] arayTmp = new byte[index2 - index1];
                                for (int j = 0; j < arayTmp.Length; j++)
                                {
                                    arayTmp[j] = bytAry[index1 + j];
                                }
                                string strTmp = HDLPF.Byte22String(arayTmp, true);
                                strFileList.Add(strTmp);
                                index1 = index2 + 2;
                            }
                        }
                    }

                }
            }
            catch
            {
            }
        }

        private void btnList_Click(object sender, EventArgs e)
        {
            tvSD.Visible = false;
            btnList.Dock = DockStyle.Top;
            tableLayoutPanel2.Visible = true;
            tableLayoutPanel2.BringToFront();
            tableLayoutPanel2.Dock = DockStyle.Top;
            tvList.Visible = true;
            tvList.Dock = DockStyle.Fill;
            tvList.BringToFront();
        }

        private void btnRef1_Click(object sender, EventArgs e)
        {
            try
            {

                dgvSong.Rows.Clear();
                ReadFileIndex = 2;
                UDPReceive.receiveQueueForAudio = new Queue<byte[]>();
                strFileList = new List<string>();
                byte[] arayTmp = new byte[7];
                arayTmp[0] = 13;
                if (Convert.ToInt32((sender as Button).Tag) == 0)
                {
                    arayTmp[1] = 0;
                    SDorFTP = 0;
                    PlaySongSource = 1;
                }
                else if (Convert.ToInt32((sender as Button).Tag) == 1)
                {
                    arayTmp[1] = 1;
                    SDorFTP = 1;
                    PlaySongSource = 3;
                }
                arayTmp[3] = 1;
                CsConst.mySends.AddBufToSndList(arayTmp, 0x1364, SubNetID, DeviceID, true, false, false, false);
                MyBackGroup = new BackgroundWorker();
                MyBackGroup.DoWork += new DoWorkEventHandler(calculationWorker_DoWork3);
                MyBackGroup.ProgressChanged += new ProgressChangedEventHandler(calculationWorker_ProgressChanged3);
                MyBackGroup.WorkerReportsProgress = true;
                MyBackGroup.RunWorkerCompleted += new RunWorkerCompletedEventHandler(calculationWorker_RunWorkerCompleted3);
                MyBackGroup.RunWorkerAsync();
                MyBackGroup.WorkerSupportsCancellation = true;
                frmProcessTmp = new FrmProcess();
                frmProcessTmp.ShowDialog();
            }
            catch
            {
            }
            Cursor.Current = Cursors.Default;
        }

        void calculationWorker_RunWorkerCompleted3(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                if (ReadFileIndex == 2)
                {
                    tvList.Nodes.Clear();
                    for (int i = 0; i < strFileList.Count; i++)
                    {
                        tvList.Nodes.Add("0", strFileList[i], 0, 0);
                    }
                }
                else if (ReadFileIndex == 3)
                {
                    SelectNode.Nodes.Clear();
                    for (int i = 0; i < strFileList.Count; i++)
                    {
                        SelectNode.Nodes.Add("2", strFileList[i], 2, 2);
                    }
                    if (SelectNode.Nodes.Count > 0)
                    {
                        SelectNode.Expand();
                    }
                }
                else if (ReadFileIndex == 4)
                {
                    dgvSong.Rows.Clear();
                    for (int i = 0; i < strFileList.Count; i++)
                    {
                        object[] obj = new object[] { dgvSong.RowCount + 1, strFileList[i] };
                        dgvSong.Rows.Add(obj);
                    }
                }
                frmProcessTmp.Close();
            }
            catch
            {
            }
        }

        void calculationWorker_ProgressChanged3(object sender, ProgressChangedEventArgs e)
        {
            try
            {

            }
            catch
            {
            }
        }

        void calculationWorker_DoWork3(object sender, DoWorkEventArgs e)
        {
            try
            {
                DateTime d1 = DateTime.Now;
                DateTime d2 = DateTime.Now;
                while (HDLSysPF.Compare(d2, d1) < 2000)
                {
                    d2 = DateTime.Now;
                }
                if (UDPReceive.receiveQueueForAudio.Count > 0)
                {
                    List<byte> bytAry = new List<byte>();

                    while (UDPReceive.receiveQueueForAudio.Count > 0)
                    {
                        byte[] ArayTmp = UDPReceive.receiveQueueForAudio.Dequeue();
                        for (int j = 0; j < ArayTmp.Length; j++)
                        {
                            bytAry.Add(ArayTmp[j]);
                        }
                    }

                    int index1 = 4;
                    int index2 = 0;
                    strFileList = new List<string>();

                    for (int i = 0; i < bytAry.Count - 1; i++)
                    {
                        if (bytAry[i] == 0x0D && bytAry[i + 1] == 0x0A)
                        {
                            index2 = i;
                            if (index1 != index2 && index2 > index1)
                            {
                                byte[] arayTmp = new byte[index2 - index1];
                                for (int j = 0; j < arayTmp.Length; j++)
                                {
                                    arayTmp[j] = bytAry[index1 + j];
                                }
                                Byte[] arrHead = new Byte[4];
                                Array.Copy(bytAry.ToArray(), index1 - 4, arrHead, 0, 4);
                                String sHead = HDLPF.Byte2String(arrHead);
                                string strTmp = HDLPF.Byte22String(arayTmp, true);
                                strFileList.Add(sHead + strTmp);
                                index1 = index2 + 2 + 4;
                            }
                        }
                    }
                }
            }
            catch
            {
            }
        }

        private void btnRef3_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            string strTmp = "*S"+"1"+"UPDATELIST";
            byte[] arayTmp = new byte[strTmp.Length + 1];
            byte[] arayRemark = HDLUDP.StringToByte(strTmp);
            Array.Copy(arayRemark, 0, arayTmp, 0, arayRemark.Length);
            arayTmp[arayTmp.Length - 1] = 13;
            if (CsConst.mySends.AddBufToSndList(arayTmp, 0x192E, SubNetID, DeviceID, false, true, true, false) == true)
            {
                CsConst.myRevBuf = new byte[1200];
            }
            else
            {
                Cursor.Current = Cursors.Default;
                return;
            }
            Cursor.Current = Cursors.Default;
        }

        private void btnRef4_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            string strTmp = "*S" + "2" + "UPDATELIST";
            byte[] arayTmp = new byte[strTmp.Length + 1];
            byte[] arayRemark = HDLUDP.StringToByte(strTmp);
            Array.Copy(arayRemark, 0, arayTmp, 0, arayRemark.Length);
            arayTmp[arayTmp.Length - 1] = 13;
            if (CsConst.mySends.AddBufToSndList(arayTmp, 0x192E, SubNetID, DeviceID, false, true, true, false) == true)
            {
                CsConst.myRevBuf = new byte[1200];
            }
            else
            {
                Cursor.Current = Cursors.Default;
                return;
            }
            Cursor.Current = Cursors.Default;
        }

        private void btnNextList_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            byte bytTag = 0;
            if (rbTets1.Checked) bytTag = 6;
            else if (rbTets2.Checked) bytTag = 5;
            else if (rbTets3.Checked) bytTag = 2;
            else if (rbTets4.Checked) bytTag = 8;
            else if (rbTets5.Checked) bytTag = 1;
            else if (rbTets6.Checked) bytTag = 9;
            string strTmp = "*S" + bytTag.ToString() + "NEXTLIST";
            byte[] arayTmp = new byte[strTmp.Length + 1];
            byte[] arayRemark = HDLUDP.StringToByte(strTmp);
            Array.Copy(arayRemark, 0, arayTmp, 0, arayRemark.Length);
            arayTmp[arayTmp.Length - 1] = 13;
            if (CsConst.mySends.AddBufToSndList(arayTmp, 0x192E, SubNetID, DeviceID, false, true, true, false) == true)
            {
                CsConst.myRevBuf = new byte[1200];
            }
            else
            {
                Cursor.Current = Cursors.Default;
                return;
            }
            Cursor.Current = Cursors.Default;
        }

        private void btnPreList_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            byte bytTag = 0;
            if (rbTets1.Checked) bytTag = 6;
            else if (rbTets2.Checked) bytTag = 5;
            else if (rbTets3.Checked) bytTag = 2;
            else if (rbTets4.Checked) bytTag = 8;
            else if (rbTets5.Checked) bytTag = 1;
            else if (rbTets6.Checked) bytTag = 9;
            string strTmp = "*S" + bytTag.ToString() + "PREVLIST";
            byte[] arayTmp = new byte[strTmp.Length + 1];
            byte[] arayRemark = HDLUDP.StringToByte(strTmp);
            Array.Copy(arayRemark, 0, arayTmp, 0, arayRemark.Length);
            arayTmp[arayTmp.Length - 1] = 13;
            if (CsConst.mySends.AddBufToSndList(arayTmp, 0x192E, SubNetID, DeviceID, false, true, true, false) == true)
            {
                CsConst.myRevBuf = new byte[1200];
            }
            else
            {
                Cursor.Current = Cursors.Default;
                return;
            }
            Cursor.Current = Cursors.Default;
        }

        private void btnNextSong_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            byte bytTag = 0;
            if (rbTets1.Checked) bytTag = 6;
            else if (rbTets2.Checked) bytTag = 5;
            else if (rbTets3.Checked) bytTag = 2;
            else if (rbTets4.Checked) bytTag = 8;
            else if (rbTets5.Checked) bytTag = 1;
            else if (rbTets6.Checked) bytTag = 9;
            string strTmp = "*S" + bytTag.ToString() + "NEXT";
            byte[] arayTmp = new byte[strTmp.Length + 1];
            byte[] arayRemark = HDLUDP.StringToByte(strTmp);
            Array.Copy(arayRemark, 0, arayTmp, 0, arayRemark.Length);
            arayTmp[arayTmp.Length - 1] = 13;
            if (CsConst.mySends.AddBufToSndList(arayTmp, 0x192E, SubNetID, DeviceID, false, true, true, false) == true)
            {
                CsConst.myRevBuf = new byte[1200];
            }
            else
            {
                Cursor.Current = Cursors.Default;
                return;
            }
            Cursor.Current = Cursors.Default;
        }

        private void btnPreSong_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            byte bytTag = 0;
            if (rbTets1.Checked) bytTag = 6;
            else if (rbTets2.Checked) bytTag = 5;
            else if (rbTets3.Checked) bytTag = 2;
            else if (rbTets4.Checked) bytTag = 8;
            else if (rbTets5.Checked) bytTag = 1;
            else if (rbTets6.Checked) bytTag = 9;
            string strTmp = "*S" + bytTag.ToString() + "PREV";
            byte[] arayTmp = new byte[strTmp.Length + 1];
            byte[] arayRemark = HDLUDP.StringToByte(strTmp);
            Array.Copy(arayRemark, 0, arayTmp, 0, arayRemark.Length);
            arayTmp[arayTmp.Length - 1] = 13;
            if (CsConst.mySends.AddBufToSndList(arayTmp, 0x192E, SubNetID, DeviceID, false, true, true, false) == true)
            {
                CsConst.myRevBuf = new byte[1200];
            }
            else
            {
                Cursor.Current = Cursors.Default;
                return;
            }
            Cursor.Current = Cursors.Default;
        }

        private void btnVol1_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            string strTmp = "*Z1VOL+";
            byte[] arayTmp = new byte[strTmp.Length + 1];
            byte[] arayRemark = HDLUDP.StringToByte(strTmp);
            Array.Copy(arayRemark, 0, arayTmp, 0, arayRemark.Length);
            arayTmp[arayTmp.Length - 1] = 13;
            if (CsConst.mySends.AddBufToSndList(arayTmp, 0x192E, SubNetID, DeviceID, false, true, true, false) == true)
            {
                CsConst.myRevBuf = new byte[1200];
            }
            else
            {
                Cursor.Current = Cursors.Default;
                return;
            }
            Cursor.Current = Cursors.Default;
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            byte bytTag = 0;
            if (rbTets1.Checked) bytTag = 6;
            else if (rbTets2.Checked) bytTag = 5;
            else if (rbTets3.Checked) bytTag = 2;
            else if (rbTets4.Checked) bytTag = 8;
            else if (rbTets5.Checked) bytTag = 1;
            else if (rbTets6.Checked) bytTag = 9;
            string strTmp = "*S" + bytTag.ToString() + "PLAYPAUSE";
            byte[] arayTmp = new byte[strTmp.Length + 1];
            byte[] arayRemark = HDLUDP.StringToByte(strTmp);
            Array.Copy(arayRemark, 0, arayTmp, 0, arayRemark.Length);
            arayTmp[arayTmp.Length - 1] = 13;
            if (CsConst.mySends.AddBufToSndList(arayTmp, 0x192E, SubNetID, DeviceID, false, true, true, false) == true)
            {
                CsConst.myRevBuf = new byte[1200];
            }
            else
            {
                Cursor.Current = Cursors.Default;
                return;
            }
            Cursor.Current = Cursors.Default;
        }

        private void btnVol2_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            string strTmp = "*Z1VOL-";
            byte[] arayTmp = new byte[strTmp.Length + 1];
            byte[] arayRemark = HDLUDP.StringToByte(strTmp);
            Array.Copy(arayRemark, 0, arayTmp, 0, arayRemark.Length);
            arayTmp[arayTmp.Length - 1] = 13;
            if (CsConst.mySends.AddBufToSndList(arayTmp, 0x192E, SubNetID, DeviceID, false, true, true, false) == true)
            {
                CsConst.myRevBuf = new byte[1200];
            }
            else
            {
                Cursor.Current = Cursors.Default;
                return;
            }
            Cursor.Current = Cursors.Default;
        }

        private void dgvSong_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {

        }

        private void FrmMzBox_Shown(object sender, EventArgs e)
        {
            SetCtrlsVisbleWithDifferentDeviceType();

            if (CsConst.MyEditMode == 1)
            {
                MyActivePage = 1;
                tsbDown_Click(tsbDown, null);
            }
        }

        private void dgvFM_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            Frequence.Visible = false;
            if (e.RowIndex >= 0)
            {
                string str = dgvFM[1, e.RowIndex].Value.ToString();
                Frequence.Text = str;
                HDLSysPF.addcontrols(1, e.RowIndex, Frequence, dgvFM);
            }
        }

        private void tvList_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            try
            {
                ReadFileIndex = 3;
                TreeNode node = tvList.GetNodeAt(e.Location);
                if (node.ImageIndex == 0 && node.Level == 0)
                {
                    dgvSong.Rows.Clear();
                    SelectNode = node;
                    strFileList = new List<string>();
                    UDPReceive.receiveQueueForAudio = new Queue<byte[]>();
                    byte[] arayTmp = new byte[7];
                    arayTmp[0] = 13;
                    arayTmp[1] = SDorFTP;
                    arayTmp[2] = 1;
                    arayTmp[3] = Convert.ToByte(node.Index + 1);
                    arayTmp[4] = 1;
                    CsConst.mySends.AddBufToSndList(arayTmp, 0x1364, SubNetID, DeviceID, true, false, false, false);
                    MyBackGroup = new BackgroundWorker();
                    MyBackGroup.DoWork += new DoWorkEventHandler(calculationWorker_DoWork3);
                    MyBackGroup.ProgressChanged += new ProgressChangedEventHandler(calculationWorker_ProgressChanged3);
                    MyBackGroup.WorkerReportsProgress = true;
                    MyBackGroup.RunWorkerCompleted += new RunWorkerCompletedEventHandler(calculationWorker_RunWorkerCompleted3);
                    MyBackGroup.RunWorkerAsync();
                    MyBackGroup.WorkerSupportsCancellation = true;
                    frmProcessTmp = new FrmProcess();
                    frmProcessTmp.ShowDialog();
                }
            }
            catch
            {
            }
        }

        private void tvList_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            try
            {
                ReadFileIndex = 4;
                TreeNode node = tvList.GetNodeAt(e.Location);
                if (node.ImageIndex == 2 && node.Level == 1)
                {
                    SelectNode = node;
                    dgvSong.Rows.Clear();
                    strFileList = new List<string>();
                    UDPReceive.receiveQueueForAudio = new Queue<byte[]>();
                    byte[] arayTmp = new byte[7];
                    arayTmp[0] = 13;
                    arayTmp[1] = SDorFTP;
                    arayTmp[2] = 2;
                    arayTmp[3] = Convert.ToByte(node.Parent.Index + 1);
                    arayTmp[4] = Convert.ToByte(node.Index + 1);
                    arayTmp[5] = 0;
                    arayTmp[6] = 1;
                    CsConst.mySends.AddBufToSndList(arayTmp, 0x1364, SubNetID, DeviceID, true, false, false, false);
                    MyBackGroup = new BackgroundWorker();
                    MyBackGroup.DoWork += new DoWorkEventHandler(calculationWorker_DoWork3);
                    MyBackGroup.ProgressChanged += new ProgressChangedEventHandler(calculationWorker_ProgressChanged3);
                    MyBackGroup.WorkerReportsProgress = true;
                    MyBackGroup.RunWorkerCompleted += new RunWorkerCompletedEventHandler(calculationWorker_RunWorkerCompleted3);
                    MyBackGroup.RunWorkerAsync();
                    MyBackGroup.WorkerSupportsCancellation = true;
                    frmProcessTmp = new FrmProcess();
                    frmProcessTmp.ShowDialog();
                }
            }
            catch
            {

            }
        }

        private void tvSD_MouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            try
            {
                TreeNode node = tvSD.GetNodeAt(e.Location);
                if (node.ImageIndex == 0)
                {
                    ReadFileIndex = 1;
                    SelectNode = node;
                    AudioDeviceTypeList.isEndReceiveFile = false;
                    strFileList = new List<string>();
                    UDPReceive.receiveQueueForAudio = new Queue<byte[]>();
                    TreeNode nodetmp = null;

                    string str = "";
                    if (node.Level == 0)
                    {
                        str = node.Text;
                    }
                    else
                    {
                        nodetmp = node;
                        str = node.Text;
                        while (nodetmp.Parent != null)
                        {
                            str = "/" + nodetmp.Parent.Text + "/" + str;
                            nodetmp = nodetmp.Parent;
                        }
                    }
                    byte[] arayRemark = HDLUDP.StringToByte(str);
                    byte[] arayTmp = new byte[arayRemark.Length + 4];
                    LengReadFileStrLength = arayRemark.Length + 3;
                    arayTmp[0] = Convert.ToByte((arayTmp.Length - 2) / 256);
                    arayTmp[1] = Convert.ToByte((arayTmp.Length - 2) % 256);
                    arayTmp[2] = 12;
                    Array.Copy(arayRemark, 0, arayTmp, 3, arayRemark.Length);
                    arayTmp[arayTmp.Length - 1] = 13;
                    CsConst.mySends.AddBufToSndList(arayTmp, 0x1364, SubNetID, DeviceID, true, false, false, false);
                    MyBackGroup = new BackgroundWorker();
                    MyBackGroup.DoWork += new DoWorkEventHandler(calculationWorker_DoWork4);
                    MyBackGroup.ProgressChanged += new ProgressChangedEventHandler(calculationWorker_ProgressChanged4);
                    MyBackGroup.WorkerReportsProgress = true;
                    MyBackGroup.RunWorkerCompleted += new RunWorkerCompletedEventHandler(calculationWorker_RunWorkerCompleted4);
                    MyBackGroup.RunWorkerAsync();
                    MyBackGroup.WorkerSupportsCancellation = true;
                    frmProcessTmp = new FrmProcess();
                    frmProcessTmp.ShowDialog();
                }
            }
            catch
            {
            }
        }

        private void tvSD_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            try
            {
                TreeNode node = tvSD.GetNodeAt(e.Location);
                if (node.ImageIndex == 0)
                {
                    ReadFileIndex = 1;
                    SelectNode = node;
                    AudioDeviceTypeList.isEndReceiveFile = false; 
                    strFileList = new List<string>();
                    UDPReceive.receiveQueueForAudio = new Queue<byte[]>();
                    TreeNode nodetmp = null;

                    string str = "";
                    if (node.Level == 0)
                    {
                        str = "/" + node.Text ;
                    }
                    else
                    {
                        nodetmp = node;
                        str = node.Text;
                        while (nodetmp.Parent != null)
                        {
                            str = "/" + nodetmp.Parent.Text + "/" + str;
                            nodetmp = nodetmp.Parent;
                        }
                    }
                    byte[] arayRemark = HDLUDP.StringToByte(str);
                    byte[] arayTmp = new byte[arayRemark.Length + 2];
                    LengReadFileStrLength = arayRemark.Length + 3;
                    arayTmp[0] = 12;
                    Array.Copy(arayRemark, 0, arayTmp, 1, arayRemark.Length);
                    arayTmp[arayTmp.Length - 1] = 13;
                    CsConst.mySends.AddBufToSndList(arayTmp, 0x1364, SubNetID, DeviceID, true, false, false, false);
                    MyBackGroup = new BackgroundWorker();
                    MyBackGroup.DoWork += new DoWorkEventHandler(calculationWorker_DoWork4);
                    MyBackGroup.ProgressChanged += new ProgressChangedEventHandler(calculationWorker_ProgressChanged4);
                    MyBackGroup.WorkerReportsProgress = true;
                    MyBackGroup.RunWorkerCompleted += new RunWorkerCompletedEventHandler(calculationWorker_RunWorkerCompleted4);
                    MyBackGroup.RunWorkerAsync();
                    MyBackGroup.WorkerSupportsCancellation = true;
                    frmProcessTmp = new FrmProcess();
                    frmProcessTmp.ShowDialog();
                }
            }
            catch
            {
            }
        }

        private void dgvKNX_SelectionChanged(object sender, EventArgs e)
        {
            DataGridViewSelectIndexOfKNX = dgvKNX.CurrentRow.Index;
            dgvKNX_CellClick(dgvKNX, new DataGridViewCellEventArgs(0, DataGridViewSelectIndexOfKNX));
        }
    }
}
