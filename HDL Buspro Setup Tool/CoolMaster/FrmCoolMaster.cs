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
    public partial class FrmCoolMaster : Form
    {
        private CoolMaster MyCoolMaster;
        private string myDevName = null;
        private int mintIDIndex = -1;
        private int mywdDevicerType = -1;
        private int MyActivePage = 0; //按页面上传下载
        private byte SubNetID = 0;
        private byte DevID = 0;

        private ComboBox cbEnable = new ComboBox();
        private TextBox txtACNO = new TextBox();
        private ComboBox cbLine = new ComboBox();
        private TextBox txtIndoor = new TextBox();
        private bool isRead = false;
        NetworkInForm networkinfo;

        public FrmCoolMaster()
        {
            InitializeComponent();
        }

        public FrmCoolMaster(CoolMaster mycoolmaster, string strName, int intDIndex, int intDeviceType)
        {
            InitializeComponent();
            this.MyCoolMaster = mycoolmaster;
            this.myDevName = strName;
            this.mintIDIndex = intDIndex;
            this.mywdDevicerType = intDeviceType;
            string strDevName = strName.Split('\\')[0].ToString();

            HDLSysPF.DisplayDeviceNameModeDescription(strName, mywdDevicerType, cboDevice, tbModel, tbDescription);

            SubNetID = Convert.ToByte(strDevName.Split('-')[0]);
            DevID = Convert.ToByte(strDevName.Split('-')[1]);
            this.Text = strName;
            tsl3.Text = strName;
        }

        private void dgvAC_SelectionChanged(object sender, EventArgs e)
        {
            lbACNOValue.Text = dgvAC[1, dgvAC.CurrentRow.Index].Value.ToString();
            lbMasterValue.Text = dgvAC[4, dgvAC.CurrentRow.Index].Value.ToString();
        }

        private void FrmCoolMaster_Load(object sender, EventArgs e)
        {
            isRead = true;
            InitialFormCtrlsTextOrItems();
            isRead = false;
        }

        void InitialFormCtrlsTextOrItems()
        {
            cl2.Items.Clear();
            cl2.Items.Add(CsConst.WholeTextsList[1775].sDisplayName);
            cl2.Items.Add(CsConst.mstrINIDefault.IniReadValue("public", "00042", ""));
            toolStrip1.Visible = (CsConst.MyEditMode == 0);

            #region
            cbEnable.Items.Clear();
            cbEnable.Items.Add(CsConst.WholeTextsList[1775].sDisplayName);
            cbEnable.Items.Add(CsConst.mstrINIDefault.IniReadValue("public", "00042", ""));
            cbLine.Items.Clear();
            for (int i = 1; i <= 8; i++)
                cbLine.Items.Add(i.ToString());
            dgvAC.Controls.Add(cbEnable);
            dgvAC.Controls.Add(txtACNO);
            dgvAC.Controls.Add(cbLine);
            dgvAC.Controls.Add(txtIndoor);
            txtACNO.KeyPress += txtFrm_KeyPress;
            cbEnable.DropDownStyle = ComboBoxStyle.DropDownList;
            cbLine.DropDownStyle = ComboBoxStyle.DropDownList;
            txtACNO.KeyPress += txtFrm_KeyPress;
            txtIndoor.KeyPress += txtIndoor_KeyPress;
            txtACNO.TextChanged += txtACNO_TextChanged;
            cbEnable.SelectedIndexChanged += cbEnable_SelectedIndexChanged;
            cbLine.SelectedIndexChanged += cbLine_SelectedIndexChanged;
            txtIndoor.TextChanged += txtIndoor_TextChanged;
            setallvisible(false);
            #endregion
        }

        void SetCtrlsVisbleWithDifferentDeviceType()
        {
            if (MyCoolMaster == null) return;

            if (CoolMasterDeviceTypeList.CLModuleNormal232.Contains(mywdDevicerType)) // 232 version
            {
                tab.TabPages.Remove(tabNet);
            }
            else if (CoolMasterDeviceTypeList.CLModuleWithNet.Contains(mywdDevicerType)) // net version
            {
                MyCoolMaster = (CoolMasterNet)MyCoolMaster;
                tab.TabPages.Remove(tabLists);
                panel2.Controls.Clear();
                networkinfo = new NetworkInForm(SubNetID, DevID, mywdDevicerType);
                panel2.Controls.Add(networkinfo);
                networkinfo.Dock = DockStyle.Fill;
            }
            toolStrip1.Visible = (CsConst.MyEditMode == 0);
        }


        void txtIndoor_KeyPress(object sender, KeyPressEventArgs e)
        {
            string str = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            if (e.KeyChar != 8)
            {
                if (str.IndexOf(e.KeyChar.ToString()) < 0)
                {
                    e.Handled = true;
                }
                else if (txtIndoor.Text.Length >= 3 && txtIndoor.SelectedText.Length <= 0)
                {
                    e.Handled = true;
                }
            }
        }

        void ModifyMultilinesIfNeeds(string strTmp, int ColumnIndex, DataGridView dgv)
        {
            if (dgv.SelectedRows == null || dgv.SelectedRows.Count == 0) return;
            if (strTmp == null) strTmp = "";
            // change the value in selected more than one line
            if (dgv.SelectedRows.Count > 1)
            {
                for (int i = 0; i < dgv.SelectedRows.Count; i++)
                {
                    dgv.SelectedRows[i].Cells[ColumnIndex].Value = strTmp;
                }
            }
        }

        void txtIndoor_TextChanged(object sender, EventArgs e)
        {
            if (dgvAC.CurrentRow.Index < 0) return;
            if (dgvAC.RowCount <= 0) return;
            int index = dgvAC.CurrentRow.Index;
            dgvAC[4, index].Value = txtIndoor.Text;
            ModifyMultilinesIfNeeds(dgvAC[4, index].Value.ToString(), 4, dgvAC);
        }

        void cbLine_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dgvAC.CurrentRow.Index < 0) return;
            if (dgvAC.RowCount <= 0) return;
            int index = dgvAC.CurrentRow.Index;
            dgvAC[3, index].Value = cbLine.Text;
            ModifyMultilinesIfNeeds(dgvAC[3, index].Value.ToString(), 3, dgvAC);
        }

        void cbEnable_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dgvAC.CurrentRow.Index < 0) return;
            if (dgvAC.RowCount <= 0) return;
            int index = dgvAC.CurrentRow.Index;
            dgvAC[1, index].Value = cbEnable.Text;
            ModifyMultilinesIfNeeds(dgvAC[1, index].Value.ToString(), 1, dgvAC);
        }

        void txtACNO_TextChanged(object sender, EventArgs e)
        {
            if (dgvAC.CurrentRow.Index < 0) return;
            if (dgvAC.RowCount <= 0) return;
            int index = dgvAC.CurrentRow.Index;
            dgvAC[2, index].Value = txtACNO.Text;
            ModifyMultilinesIfNeeds(dgvAC[2, index].Value.ToString(), 2, dgvAC);
        }

        private void setallvisible(bool TF)
        {
            cbEnable.Visible = TF;
            txtACNO.Visible = TF;
            cbLine.Visible = TF;
            txtIndoor.Visible = TF;
        }

        private void btnSure_Click(object sender, EventArgs e)
        {
            tsbDown_Click(tsbDown, null);
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

                    byte[] ArayRelay = new byte[] { bytSubID, bytDevID, (byte)(mywdDevicerType / 256), (byte)(mywdDevicerType % 256), (byte)MyActivePage,
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

        private void showCoolMasterNetInfo()
        {
            try
            {
                isRead = true;
                Cursor.Current = Cursors.WaitCursor;
                chbEnable.Checked = ((CoolMasterNet)MyCoolMaster).Enable;
                txtIP.Text = ((CoolMasterNet)MyCoolMaster).strIP;
                txtPort.Text = ((CoolMasterNet)MyCoolMaster).Port.ToString();
                dgvAC.Rows.Clear();
                if (MyCoolMaster.myACSetting != null)
                {
                    for (int i = 0; i < MyCoolMaster.myACSetting.Count; i++)
                    {
                        ThirdPartAC temp = MyCoolMaster.myACSetting[i];
                        string strEnable = CsConst.WholeTextsList[1775].sDisplayName;
                        if (temp.Enable == 1) strEnable = CsConst.mstrINIDefault.IniReadValue("public", "00042", "");
                        byte[] arayTmp = new byte[1];
                        arayTmp[0] = temp.GroupID;
                        string strLine = System.Text.Encoding.Default.GetString(arayTmp);
                        string strIndoor = System.Text.Encoding.Default.GetString(temp.arayACinfo);
                        object[] obj = new object[] { temp.ID.ToString(), strEnable, temp.ACNO.ToString(), strLine, strIndoor };
                        dgvAC.Rows.Add(obj);
                    }
                }
            }
            catch
            {
            }
            Cursor.Current = Cursors.Default;
            isRead = false;
        }

        void UpdateDisplayInformationAccordingly(object sender, FormClosingEventArgs e)
        {
            if (tab.SelectedTab.Name == "tabList")
            {
                 showCoolMasterInfo();
            }
            else if (tab.SelectedTab.Name =="tabNet")
            {
                showCoolMasterNetInfo();
            }
        }

        private void showCoolMasterInfo()
        {
            try
            {
                isRead = true;
                dgvAC.Rows.Clear();
                if (MyCoolMaster.myACSetting != null)
                {
                    for (int i = 0; i < MyCoolMaster.myACSetting.Count; i++)
                    {
                        ThirdPartAC temp = MyCoolMaster.myACSetting[i];
                        string strEnable = CsConst.WholeTextsList[1775].sDisplayName;
                        if (temp.Enable == 1) strEnable = CsConst.mstrINIDefault.IniReadValue("public", "00042", "");
                        bool[] arayBL1 = new bool[] { false, false, false, false };
                        bool[] arayBL2 = new bool[] { false, false, false, false };
                        if (temp.arayACinfo[0] > 0)
                        {
                            for (int j = 0; j < temp.arayACinfo[0]; j++)
                            {
                                if (j < 4)
                                {
                                    if (1 <= temp.arayACinfo[1 + j] && temp.arayACinfo[1 + j] <= 4)
                                        arayBL1[temp.arayACinfo[1 + j] - 1] = true;
                                }
                            }
                        }
                        if (temp.arayACinfo[5] > 0)
                        {
                            for (int j = 0; j < temp.arayACinfo[5]; j++)
                            {
                                if (j < 4)
                                {
                                    if (1 <= temp.arayACinfo[6 + j] && temp.arayACinfo[6 + j] <= 4)
                                        arayBL2[temp.arayACinfo[6 + j] - 1] = true;
                                }
                            }
                        }
                        object[] obj = new object[] { temp.ID.ToString(), temp.ACNO.ToString(), strEnable ,temp.Remark,
                                   temp.CoolMasterAddress.ToString(),temp.GroupID.ToString(),
                                   arayBL1[0],arayBL1[1],arayBL1[2],arayBL1[3],arayBL2[0],arayBL2[1],arayBL2[2],arayBL2[3]};
                        dgvAC.Rows.Add(obj);
                    }
                }
            }
            catch
            {
            }
            isRead = false;
        }


        private void txtFrm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar) || e.KeyChar == 8)
                e.Handled = false;
            else
                e.Handled = true;
        }

        private void dgvAC_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (isRead) return;
            if (MyCoolMaster == null) return;
            if (MyCoolMaster.myACSetting == null) return;
            if ((e.RowIndex == -1) || (e.ColumnIndex == -1)) return;
            if (dgvAC[e.ColumnIndex, e.RowIndex].Value == null) dgvAC[e.ColumnIndex, e.RowIndex].Value = "";
            try
            {
                for (int i = 0; i < dgvAC.SelectedRows.Count; i++)
                {
                    dgvAC.SelectedRows[i].Cells[e.ColumnIndex].Value = dgvAC[e.ColumnIndex, e.RowIndex].Value.ToString();
                    string strTmp = "";
                    switch (e.ColumnIndex)
                    {
                        case 1:
                            strTmp = dgvAC[1, dgvAC.SelectedRows[i].Index].Value.ToString();
                            dgvAC[1, dgvAC.SelectedRows[i].Index].Value = HDLPF.IsNumStringMode(strTmp, 0, 255);
                            MyCoolMaster.myACSetting[dgvAC.SelectedRows[i].Index].ACNO = Convert.ToByte(dgvAC[1, dgvAC.SelectedRows[i].Index].Value.ToString());
                            break;
                        case 2:
                            MyCoolMaster.myACSetting[dgvAC.SelectedRows[i].Index].Enable = Convert.ToByte(cl2.Items.IndexOf(dgvAC[2, dgvAC.SelectedRows[i].Index].Value.ToString()));
                            break;
                        case 3:
                            strTmp = dgvAC[3, dgvAC.SelectedRows[i].Index].Value.ToString();
                            dgvAC[3, dgvAC.SelectedRows[i].Index].Value = HDLPF.IsRightStringMode(strTmp);
                            MyCoolMaster.myACSetting[dgvAC.SelectedRows[i].Index].Remark = dgvAC[3, dgvAC.SelectedRows[i].Index].Value.ToString();
                            break;
                        case 4:
                            strTmp = dgvAC[4, dgvAC.SelectedRows[i].Index].Value.ToString();
                            dgvAC[4, dgvAC.SelectedRows[i].Index].Value = HDLPF.IsNumStringMode(strTmp, 0, 255);
                            MyCoolMaster.myACSetting[dgvAC.SelectedRows[i].Index].CoolMasterAddress = Convert.ToByte(dgvAC[4, dgvAC.SelectedRows[i].Index].Value.ToString());
                            break;
                        case 5:
                            strTmp = dgvAC[5, dgvAC.SelectedRows[i].Index].Value.ToString();
                            dgvAC[5, dgvAC.SelectedRows[i].Index].Value = HDLPF.IsNumStringMode(strTmp, 0, 255);
                            MyCoolMaster.myACSetting[dgvAC.SelectedRows[i].Index].GroupID = Convert.ToByte(dgvAC[5, dgvAC.SelectedRows[i].Index].Value.ToString());
                            break;
                        case 6:
                        case 7:
                        case 8:
                        case 9:
                            if (MyCoolMaster.myACSetting[dgvAC.SelectedRows[i].Index].arayACinfo == null) MyCoolMaster.myACSetting[dgvAC.SelectedRows[i].Index].arayACinfo = new byte[10];
                            for (int j = 0; j < 5; j++)
                            {
                                MyCoolMaster.myACSetting[dgvAC.SelectedRows[i].Index].arayACinfo[j] = 0;
                            }
                            for (int j = 0; j < 4; j++)
                            {
                                if (dgvAC[6 + j, dgvAC.SelectedRows[i].Index].Value.ToString().ToLower() == "true")
                                {
                                    MyCoolMaster.myACSetting[dgvAC.SelectedRows[i].Index].arayACinfo[0] =
                                        Convert.ToByte(MyCoolMaster.myACSetting[dgvAC.SelectedRows[i].Index].arayACinfo[0] + 1);
                                }
                            }
                            int intTmp = 0;
                            for (int j = 0; j < 4; j++)
                            {
                                if (dgvAC[6 + j, dgvAC.SelectedRows[i].Index].Value.ToString().ToLower() == "true")
                                {
                                    MyCoolMaster.myACSetting[dgvAC.SelectedRows[i].Index].arayACinfo[1 + intTmp] = Convert.ToByte(1 + j);
                                    intTmp++;
                                }
                            }
                            break;
                        case 10:
                        case 11:
                        case 12:
                        case 13:
                            if (MyCoolMaster.myACSetting[dgvAC.SelectedRows[i].Index].arayACinfo == null) MyCoolMaster.myACSetting[dgvAC.SelectedRows[i].Index].arayACinfo = new byte[10];
                            for (int j = 5; j < 10; j++)
                            {
                                MyCoolMaster.myACSetting[dgvAC.SelectedRows[i].Index].arayACinfo[j] = 0;
                            }
                            for (int j = 0; j < 4; j++)
                            {
                                if (dgvAC[10 + j, dgvAC.SelectedRows[i].Index].Value.ToString().ToLower() == "true")
                                {
                                    MyCoolMaster.myACSetting[dgvAC.SelectedRows[i].Index].arayACinfo[5] =
                                        Convert.ToByte(MyCoolMaster.myACSetting[dgvAC.SelectedRows[i].Index].arayACinfo[5] + 1);
                                }
                            }
                            intTmp = 0;
                            for (int j = 0; j < 4; j++)
                            {
                                if (dgvAC[10 + j, dgvAC.SelectedRows[i].Index].Value.ToString().ToLower() == "true")
                                {
                                    MyCoolMaster.myACSetting[dgvAC.SelectedRows[i].Index].arayACinfo[6 + intTmp] = Convert.ToByte(1 + j);
                                    intTmp++;
                                }
                            }
                            break;
                    }
                }
                if (e.ColumnIndex == 1)
                {
                    string strTmp = dgvAC[1, e.RowIndex].Value.ToString();
                    MyCoolMaster.myACSetting[e.RowIndex].ACNO =Convert.ToByte(dgvAC[1, e.RowIndex].Value.ToString());
                }
                else if (e.ColumnIndex == 2)
                {
                    MyCoolMaster.myACSetting[e.RowIndex].Enable = Convert.ToByte(cl2.Items.IndexOf(dgvAC[2, e.RowIndex].Value.ToString()));
                }
                else if (e.RowIndex == 3)
                {
                    string strTmp = dgvAC[3, e.RowIndex].Value.ToString();
                    MyCoolMaster.myACSetting[e.RowIndex].Remark = dgvAC[3, e.RowIndex].Value.ToString();
                }
                else if (e.RowIndex == 4)
                {
                    string strTmp = dgvAC[4, e.RowIndex].Value.ToString();
                    MyCoolMaster.myACSetting[e.RowIndex].CoolMasterAddress = Convert.ToByte(dgvAC[4, e.RowIndex].Value.ToString());
                }
                else if (e.RowIndex == 5)
                {
                    string strTmp = dgvAC[5, e.RowIndex].Value.ToString();
                    MyCoolMaster.myACSetting[e.RowIndex].GroupID = Convert.ToByte(dgvAC[5, e.RowIndex].Value.ToString());
                }
            }
            catch
            {
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            tsbDown_Click(toolStripLabel2, null);
        }

        private void txtTo_TextChanged(object sender, EventArgs e)
        {
            string str = txtTo.Text;
            int num = Convert.ToInt32(txtFrm.Text);
            txtTo.Text = HDLPF.IsNumStringMode(str, num, 64);
            txtTo.SelectionStart = txtTo.Text.Length;
        }

        private void txtFrm_TextChanged(object sender, EventArgs e)
        {
            string str = txtFrm.Text;
            int num = Convert.ToInt32(txtTo.Text);
            txtFrm.Text = HDLPF.IsNumStringMode(str, 1, num);
            txtFrm.SelectionStart = txtFrm.Text.Length;
        }

        private void FrmCoolMaster_Shown(object sender, EventArgs e)
        {
            SetCtrlsVisbleWithDifferentDeviceType();
            if (CsConst.MyEditMode == 0)
            {

            }
            else if (CsConst.MyEditMode == 1) //在线模式
            {
                MyActivePage = 1;
                tsbDown_Click(tsbDown, null);
            }
        }

        private void dgvAC_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            dgvAC.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void btnSaveAndClose1_Click(object sender, EventArgs e)
        {
            btnSave_Click(null, null);
            this.Close();
        }
    }
}
