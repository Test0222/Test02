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
    public partial class FrmMultiRemoteSender : Form
    {
        private byte SubNetID;
        private byte DevID;
        private int mintIDIndex = -1;
        private int mintDeviceType = -1;
        private string mystrName;
        private SendIR oMultiRemoteSender = new SendIR();
        private SendIR tempSend = new SendIR();
        private int MyActivePage = 0; //按页面上传下载
        private bool isStopDownloadCodes = false;
        private BackgroundWorker MyBackGroup;
        private bool isRead = false;
        private TextBox txtRemark = new TextBox();
        public FrmMultiRemoteSender()
        {
            InitializeComponent();
        }
        public FrmMultiRemoteSender(SendIR ir, string strname, int intDIndex, int intDeviceType)
        {
            InitializeComponent();
            oMultiRemoteSender = ir;
            this.mystrName = strname;
            this.mintDeviceType = intDeviceType;
            this.mintIDIndex = intDIndex;

            string strDevName = strname.Split('\\')[0].ToString();
            HDLSysPF.DisplayDeviceNameModeDescription(strname, mintDeviceType, cboDevice, tbModel, tbDescription);
            SubNetID = Convert.ToByte(strDevName.Split('-')[0]);
            DevID = Convert.ToByte(strDevName.Split('-')[1]);
            System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = false;
            HDLSysPF.addIR(tvIR, tempSend, true);  // 添加已有的列表到窗体
            txtRemark.TextChanged += txtRemark_TextChanged;
            txtRemark.Visible = false;
            dgvIR.Controls.Add(txtRemark);
        }

        void txtRemark_TextChanged(object sender, EventArgs e)
        {
            if (dgvIR.CurrentRow.Index < 0) return;
            if (dgvIR.RowCount <= 0) return;
            int index = dgvIR.CurrentRow.Index;
            if (txtRemark.Text.Length > 0)
            {
                dgvIR[1, index].Value = txtRemark.Text;
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

        private void btnUpload_Click(object sender, EventArgs e)
        {
            try
            {
                txtRemark.Visible = false;
                isStopDownloadCodes = false;
                tsbProcess.Visible = true;
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
                tsbProcess.Value = 100;
                lblProgressValue.Text = "100%";
                System.Threading.Thread.Sleep(1000);
                tsbProcess.Visible = false;
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
                this.tsbProcess.Value = e.ProgressPercentage;
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
                    if (CsConst.mySends.AddBufToSndList(arayTmp, 0xD900, SubNetID, DevID, false, true, true, false) == true)
                    {
                        if (CsConst.myRevBuf[25] == 0xF8)
                        {
                            CsConst.myRevBuf = new byte[1200];
                            for (int j = 0; j < ListData.Length; j++)
                            {
                                arayTmp = new byte[16];
                                Array.Copy(arayCode, j * 16, arayTmp, 0, 16);
                                if (CsConst.mySends.AddBufToSndList(arayTmp, 0xD906, SubNetID, DevID, false, true, true, false) == true)
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
                            if (CsConst.mySends.AddBufToSndList(arayTmp, 0xD90E, SubNetID, DevID, false, true, true, false) == true)
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
                            if (CsConst.mySends.AddBufToSndList(arayTmp, 0xD90E, SubNetID, DevID, false, true, true, false) == true)
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

        private void btnTest_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            byte[] arayTmp = new byte[2];
            if (cbState.Visible)
            {
                if (oMultiRemoteSender.mbytTwo == 0)
                {
                    arayTmp[0] = oMultiRemoteSender.mbytOne;
                    if (cbState.SelectedIndex == 0) arayTmp[1] = 0;
                    else if (cbState.SelectedIndex == 1) arayTmp[1] = 255;
                }
                else
                {
                    if (Convert.ToByte(dgvIR[0, dgvIR.CurrentRow.Index].Value.ToString()) == oMultiRemoteSender.mbytTwo)
                    {
                        arayTmp[0] = oMultiRemoteSender.mbytTwo;
                    }
                    else if (Convert.ToByte(dgvIR[0, dgvIR.CurrentRow.Index].Value.ToString()) == oMultiRemoteSender.mbytOne)
                    {
                        arayTmp[0] = oMultiRemoteSender.mbytOne;
                    }
                    if (cbState.SelectedIndex == 0) arayTmp[1] = 0;
                    else if (cbState.SelectedIndex == 1) arayTmp[1] = 255;
                }
                if (CsConst.mySends.AddBufToSndList(arayTmp, 0xE01C, SubNetID, DevID, false, false, false,false) == true)
                {
                    CsConst.myRevBuf = new byte[1200];
                }
            }
            else
            {
                arayTmp[0] = Convert.ToByte(Convert.ToInt32(dgvIR[0, dgvIR.CurrentRow.Index].Value.ToString()) - 1);
                if (rb1.Checked) arayTmp[1] = 0;
                else if (rb2.Checked) arayTmp[1] = 1;
                else if (rb3.Checked) arayTmp[1] = 2;
                if (CsConst.mySends.AddBufToSndList(arayTmp, 0xD912, SubNetID, DevID, false, false, false, false) == true)
                {
                    CsConst.myRevBuf = new byte[1200];
                }
            }
            Cursor.Current = Cursors.Default;
        }

        private void tslRead_Click(object sender, EventArgs e)
        {
            try
            {
                txtRemark.Visible = false;
                byte bytTag = Convert.ToByte(((ToolStripButton)sender).Tag.ToString());
                bool blnShowMsg = (CsConst.MyEditMode != 1);
                if (HDLPF.UploadOrReadGoOn(this.Text, bytTag, blnShowMsg))
                {
                    Cursor.Current = Cursors.WaitCursor;

                    CsConst.MyUPload2DownLists = new List<byte[]>();

                    string strName = mystrName.Split('\\')[0].ToString();
                    byte bytSubID = byte.Parse(strName.Split('-')[0]);
                    byte bytDevID = byte.Parse(strName.Split('-')[1]);
                    byte[] ArayRelay = new byte[] { SubNetID, DevID, (byte)(mintDeviceType / 256), (byte)(mintDeviceType % 256), (byte)MyActivePage,
                                               (byte)(mintIDIndex / 256), (byte)(mintIDIndex % 256),
                                               Convert.ToByte(txtFrm.Text),Convert.ToByte(txtTo.Text)};
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
            switch (tabcontrol.SelectedIndex)
            {
                case 0: showBasicInfo(); break;
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
                if (oMultiRemoteSender != null)
                {
                    if (oMultiRemoteSender.IRCodes != null)
                    {
                        dgvIR.Rows.Clear();
                        for (int i = 0; i < oMultiRemoteSender.IRCodes.Count; i++)
                        {
                            UVCMD.IRCode ir = oMultiRemoteSender.IRCodes[i];
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
                    chbEnable.Checked = (oMultiRemoteSender.RemoteEnable == 1);
                }
                if (btnFree.Visible)
                    btnFree_Click(null, null);
            }
            catch
            {
            }
            isRead = false;
        }

        private void btnSure_Click(object sender, EventArgs e)
        {
            tslRead_Click(tslRead, null);
        }

        private void txtFrm_TextChanged(object sender, EventArgs e)
        {
            string str = txtFrm.Text;
            int num = Convert.ToInt32(txtTo.Text);
            txtFrm.Text = HDLPF.IsNumStringMode(str, 1, num);
            txtFrm.SelectionStart = txtFrm.Text.Length;
        }

        private void txtTo_TextChanged(object sender, EventArgs e)
        {
            string str = txtTo.Text;
            int num = Convert.ToInt32(txtFrm.Text);
            txtTo.Text = HDLPF.IsNumStringMode(str, num, 249);
            txtTo.SelectionStart = txtTo.Text.Length;
        }

        private void txtFrm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar) || e.KeyChar == 8)
                e.Handled = false;
            else
                e.Handled = true;
        }

        void InitialFormCtrlsTextOrItems()
        {
            cbState.Items.Clear();
            cbState.Items.Add(CsConst.Status[0]);
            cbState.Items.Add(CsConst.Status[1]);
            cbState.SelectedIndex = 0;
        }

        void SetCtrlsVisbleWithDifferentDeviceType()
        {
            toolStrip1.Visible = (CsConst.MyEditMode == 0);
            rb1.Checked = true;
            if (mintDeviceType == 304 || mintDeviceType == 306 || mintDeviceType == 313 || mintDeviceType == 319)
            {
                btnFree.Visible = true;
                lbFree.Visible = true;
            }
            else
            {
                btnFree.Visible = false;
                lbFree.Visible = false;
            }
        }

        private void FrmMultiRemoteSender_Load(object sender, EventArgs e)
        {
            InitialFormCtrlsTextOrItems();
        }

        private void FrmMultiRemoteSender_Shown(object sender, EventArgs e)
        {
            SetCtrlsVisbleWithDifferentDeviceType();
            if (CsConst.MyEditMode == 1) //在线模式
            {
                toolStrip1.Visible = false;
                MyActivePage = 1;
                tslRead_Click(tslRead, null);
            }
        }

        private void tabcontrol_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CsConst.MyEditMode == 1)
            {
                if (tabcontrol.SelectedIndex == 0)
                {
                    MyActivePage = 1;
                    tslRead_Click(tslRead, null);
                }
                else if (tabcontrol.SelectedIndex == 1)
                {
                    BtnReadK_Click(null, null);
                    btnReadDetect_Click(null, null);
                }
            }
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

        private void btnInitital_Click(object sender, EventArgs e)
        {
            txtRemark.Visible = false;
            Cursor.Current = Cursors.WaitCursor;
            DialogResult result = MessageBox.Show(CsConst.mstrINIDefault.IniReadValue("public", "99805", ""), "", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (result == DialogResult.OK)
            {
                byte[] arayTmp = new byte[0];
                CsConst.replySpanTimes = 15000;
                if (CsConst.mySends.AddBufToSndList(arayTmp, 0xD9E0, SubNetID, DevID, false, true, true, false) == true)
                {
                    if (CsConst.myRevBuf[25] == 0xF8)
                    {
                        MessageBox.Show(CsConst.mstrINIDefault.IniReadValue("public", "99806", ""));
                        dgvIR.Rows.Clear();
                        oMultiRemoteSender.IRCodes = new List<UVCMD.IRCode>();
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

        private void chbEnable_CheckedChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            Cursor.Current = Cursors.WaitCursor;
            byte[] arayTmp = new byte[2];
            arayTmp[0] = 255;
            if (chbEnable.Checked) arayTmp[1] = 255;
            else arayTmp[1] = 0;
            if (CsConst.mySends.AddBufToSndList(arayTmp, 0xE01C, SubNetID, DevID, false, true, true, false) == true)
            {
                oMultiRemoteSender.RemoteEnable = CsConst.myRevBuf[26];
                CsConst.myRevBuf = new byte[1200];
            }
            Cursor.Current = Cursors.Default;
        }

        private void dgvIR_SelectionChanged(object sender, EventArgs e)
        {
            lbState.Visible = false;
            cbState.Visible = false;
            lbPress.Visible = true;
            rb1.Visible = true;
            rb2.Visible = true;
            rb3.Visible = true;
            lbCurValue.Text = dgvIR[0, dgvIR.CurrentRow.Index].Value.ToString();
            int intTmp = Convert.ToByte(dgvIR[0, dgvIR.CurrentRow.Index].Value.ToString());
            if (oMultiRemoteSender.mbytTwo == 0)
            {
                if (intTmp == oMultiRemoteSender.mbytOne)
                {
                    cbState.Enabled = true;
                    lbState.Visible = true;
                    cbState.Visible = true;
                    lbPress.Visible = false;
                    rb1.Visible = false;
                    rb2.Visible = false;
                    rb3.Visible = false;
                }
            }
            else
            {
                if (intTmp == oMultiRemoteSender.mbytOne || intTmp == oMultiRemoteSender.mbytTwo)
                {
                    cbState.Enabled = false;
                    lbState.Visible = true;
                    cbState.Visible = true;
                    lbPress.Visible = false;
                    rb1.Visible = false;
                    rb2.Visible = false;
                    rb3.Visible = false;
                    if (intTmp == oMultiRemoteSender.mbytOne) cbState.SelectedIndex = 1;
                    else if (intTmp == oMultiRemoteSender.mbytTwo) cbState.SelectedIndex = 0;
                }
            }
        }

        private void btnFree_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            byte[] arayTmp = new byte[0];
            if (CsConst.mySends.AddBufToSndList(arayTmp, 0xD978, SubNetID, DevID, false, true, true, false) == true)
            {
                double temp = (double)(CsConst.myRevBuf[27] * 256 + CsConst.myRevBuf[28]) / (double)(CsConst.myRevBuf[25] * 256 + CsConst.myRevBuf[26]);
                lbFree.Text = string.Format("{0:P}", temp);
                CsConst.myRevBuf = new byte[1200];
            }
            Cursor.Current = Cursors.Default;
        }

        private void tmRead_Click(object sender, EventArgs e)
        {
            if (tabcontrol.SelectedIndex == 0)
                tslRead_Click(tslRead, null);
            else
            {
                BtnReadK_Click(null, null);
                btnReadDetect_Click(null, null);
            }
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
                if (CsConst.mySends.AddBufToSndList(arayTmp, 0xD904, SubNetID, DevID, false, true, true, false) == true)
                {
                    oMultiRemoteSender.IRCodes[e.RowIndex].Enable = 0;
                    oMultiRemoteSender.IRCodes[e.RowIndex].Codes = "";
                    oMultiRemoteSender.IRCodes[e.RowIndex].Remark1 = "";
                    dgvIR[1, e.RowIndex].Value = "";
                    dgvIR[2, e.RowIndex].Value = CsConst.WholeTextsList[1775].sDisplayName;
                    CsConst.myRevBuf = new byte[1200];
                }
            }
            Cursor.Current = Cursors.Default;
        }

        private void txtK1_TextChanged(object sender, EventArgs e)
        {
            if (txtK1.Text.Length > 0)
            {
                string str = txtK1.Text;
                int num = Convert.ToInt32(txtK1.Text);
                txtK1.Text = HDLPF.IsNumStringMode(str, 1, 255);
                txtK1.SelectionStart = txtK1.Text.Length;
            }
        }

        private void txtK2_TextChanged(object sender, EventArgs e)
        {
            if (txtK2.Text.Length > 0)
            {
                string str = txtK2.Text;
                int num = Convert.ToInt32(txtK2.Text);
                txtK2.Text = HDLPF.IsNumStringMode(str, 1, 255);
                txtK2.SelectionStart = txtK2.Text.Length;
            }
        }

        private void txtDTime_TextChanged(object sender, EventArgs e)
        {
            if (txtDTime.Text.Length > 0)
            {
                string str = txtDTime.Text;
                int num = Convert.ToInt32(txtDTime.Text);
                txtDTime.Text = HDLPF.IsNumStringMode(str, 0, 255);
                txtDTime.SelectionStart = txtDTime.Text.Length;
            }
        }

        private void txtDValue_TextChanged(object sender, EventArgs e)
        {
            if (txtDValue.Text.Length > 0)
            {
                string str = txtDValue.Text;
                int num = Convert.ToInt32(txtDValue.Text);
                txtDValue.Text = HDLPF.IsNumStringMode(str, 1, 255);
                txtDValue.SelectionStart = txtDValue.Text.Length;
            }
        }

        private void btnReadOn_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            byte[] ArayTmp = new byte[0];
            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xD974, SubNetID, DevID, false, true, true,false) == true)
            {
                lbDOnValue.Text = CsConst.myRevBuf[25].ToString();
            }
            lbDAValue.Text = ((Convert.ToInt32(lbDOnValue.Text) + Convert.ToInt32(lbDOffValue.Text)) / 2).ToString();
            CsConst.myRevBuf = new byte[1200];
            Cursor.Current = Cursors.Default;
        }

        private void btnReadDetect_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            byte[] ArayTmp = new byte[0];
            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xD970, SubNetID, DevID, false, true, true, false) == true)
            {
                txtDTime.Text = CsConst.myRevBuf[25].ToString();
                txtDValue.Text = CsConst.myRevBuf[26].ToString();
            }
            CsConst.myRevBuf = new byte[1200];
            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xD964, SubNetID, DevID, false, true, true, false) == true)
            {
                if (CsConst.myRevBuf[25] == 1) lbDSatusValue.Text = CsConst.Status[1];
                else if (CsConst.myRevBuf[25] == 0) lbDSatusValue.Text = CsConst.Status[0];
                CsConst.myRevBuf = new byte[1200];
            }
            Cursor.Current = Cursors.Default;
        }

        private void btnSaveDetect_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            byte[] arayTmp = new byte[2];
            arayTmp[0] = Convert.ToByte(txtDTime.Text);
            arayTmp[1] = Convert.ToByte(txtDValue.Text);
            if (CsConst.mySends.AddBufToSndList(arayTmp, 0xD972, SubNetID, DevID, false, true, true, false) == true)
            {
                CsConst.myRevBuf = new byte[1200];
            }
            Cursor.Current = Cursors.Default;
        }

        private void BtnReadK_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            byte[] ArayTmp = new byte[0];
            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xD962, SubNetID, DevID, false, true, true, false) == true)
            {
                if (CsConst.myRevBuf[26] == 0)
                {
                    rbK1.Checked = true;
                    txtK1.Text = CsConst.myRevBuf[25].ToString();
                    txtK2.Text = (CsConst.myRevBuf[25] + 1).ToString();
                }
                else
                {
                    rbK2.Checked = true;
                    txtK1.Text = CsConst.myRevBuf[25].ToString();
                    txtK2.Text = CsConst.myRevBuf[26].ToString();
                }
            }

            CsConst.myRevBuf = new byte[1200];
            Cursor.Current = Cursors.Default;
        }

        private void btnSaveK_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            byte[] arayTmp = new byte[2];
            arayTmp[0] = 0;
            arayTmp[1] = Convert.ToByte(txtK1.Text);
            if (CsConst.mySends.AddBufToSndList(arayTmp, 0xD960, SubNetID, DevID, false, true, true, false) == true)
            {
                oMultiRemoteSender.mbytOne = arayTmp[1];
                CsConst.myRevBuf = new byte[1200];
            }
            arayTmp[0] = 1;
            if (rbK2.Checked)
                arayTmp[1] = Convert.ToByte(txtK2.Text);
            else
                arayTmp[1] = 0;
            if (CsConst.mySends.AddBufToSndList(arayTmp, 0xD960, SubNetID, DevID, false, true, true, false) == true)
            {
                oMultiRemoteSender.mbytTwo = arayTmp[1];
                CsConst.myRevBuf = new byte[1200];
            }
            Cursor.Current = Cursors.Default;
        }

        private void btnReadOff_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            byte[] ArayTmp = new byte[0];
            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xD974, SubNetID, DevID, false, true, true, false) == true)
            {
                lbDOffValue.Text = CsConst.myRevBuf[25].ToString();
            }
            lbDAValue.Text = ((Convert.ToInt32(lbDOnValue.Text) + Convert.ToInt32(lbDOffValue.Text)) / 2).ToString();
            CsConst.myRevBuf = new byte[1200];
            Cursor.Current = Cursors.Default;
        }

        private void rbK1_CheckedChanged(object sender, EventArgs e)
        {
            if (rbK1.Checked)
            {
                lbK1.Text = CsConst.mstrINIDefault.IniReadValue("public", "99804", "");
                lbK2.Visible = false;
                txtK2.Visible = false;
            }
            else if (rbK2.Checked)
            {
                lbK1.Text = CsConst.mstrINIDefault.IniReadValue("public", "99803", "");
                lbK2.Visible = true;
                txtK2.Visible = true;
            }
        }

        private void dgvIR_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                txtRemark.Text = dgvIR[1, e.RowIndex].Value.ToString();
                if (CsConst.MyEditMode == 0)
                    addcontrols(1, e.RowIndex, txtRemark);
            }
        }

        private void dgvIR_Scroll(object sender, ScrollEventArgs e)
        {
            txtRemark.Visible = false;
        }

        private void addcontrols(int col, int row, Control con)
        {
            con.Show();
            con.Visible = true;
            Rectangle rect = dgvIR.GetCellDisplayRectangle(col, row, true);
            con.Size = rect.Size;
            con.Top = rect.Top;
            con.Left = rect.Left;
        }

        private void btnModify_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            byte[] arayRemark = new byte[20];
            string strRemark = "";
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
            if (CsConst.mySends.AddBufToSndList(araySendIR, 0xD90E, SubNetID, DevID, false, true, true, false) == true)
            {
                HDLUDP.TimeBetwnNext(araySendIR.Length);
                CsConst.myRevBuf = new byte[1200];
            }

            araySendIR[1] = 1;    //save the remark then
            for (int K = 0; K <= 9; K++) araySendIR[2 + K] = arayRemark[10 + K];

            if (CsConst.mySends.AddBufToSndList(araySendIR, 0xD90E, SubNetID, DevID, false, true, true, false) == true)
            {
                HDLUDP.TimeBetwnNext(araySendIR.Length);
                CsConst.myRevBuf = new byte[1200];
            }
            Cursor.Current = Cursors.Default;
        }

        private void txtKeyRemark_TextChanged(object sender, EventArgs e)
        {
            if (dgvIR.RowCount <= 0) return;
            if (dgvIR.CurrentRow.Index < 0) return;
            if (oMultiRemoteSender.IRCodes == null) return;
            if (oMultiRemoteSender.IRCodes.Count == 0) return;
            dgvIR[1, dgvIR.CurrentRow.Index].Value = "";
            txtRemark.Text = "";
            for (int j = 0; j < oMultiRemoteSender.IRCodes.Count; j++)
            {
                if (oMultiRemoteSender.IRCodes[j].KeyID == Convert.ToInt32(dgvIR[0, dgvIR.CurrentRow.Index].Value))
                {
                    oMultiRemoteSender.IRCodes[j].Remark1 = "";
                    break;
                }
            }
        }

        private void txtK1_Leave(object sender, EventArgs e)
        {
            if (txtK1.Text.Trim() == "")
            {
                txtK1.Text = "1";
            }
        }

        private void txtK2_Leave(object sender, EventArgs e)
        {
            if (txtK2.Text.Trim() == "") txtK2.Text = "1";
        }

        private void txtDTime_Leave(object sender, EventArgs e)
        {
            if (txtDTime.Text.Trim() == "") txtDTime.Text = "0";
        }

        private void txtDValue_Leave(object sender, EventArgs e)
        {
            if (txtDValue.Text.Trim() == "") txtDValue.Text = "1";
        }

        private void btnSave2_Click(object sender, EventArgs e)
        {
            btnSaveK_Click(null, null);
            btnSaveDetect_Click(null, null);
        }

        private void btnSaveAndClose2_Click(object sender, EventArgs e)
        {
            btnSave2_Click(null, null);
            this.Close();
        }
    }
}
