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
    public partial class FrmBusToRS232 : Form
    {
        private int Type;
        private byte SubNetID;
        private byte DevID;
        private int MyintDeviceType;
        private string strRemark = "";
        private RS232 oRS232;
        private MHRCU oMHRCU;
        private object MyActiveObj;
        private TextBox txtRemark = new TextBox();
        private TextBox txtNO = new TextBox();
        private ComboBox cbType = new ComboBox();
        private ComboBox cbState = new ComboBox();
        private UnivasalControl uc1 = new UnivasalControl("0/0");
        private UnivasalControl uc2 = new UnivasalControl("0/0");
        private int MaxCount = 200;
        public FrmBusToRS232()
        {
            InitializeComponent();
        }

        public FrmBusToRS232(string strname, object obj, int devicetype, int maxcount,int type)
        {
            InitializeComponent();
            this.Type = type;
            this.MyActiveObj = obj;
            if (MyActiveObj is RS232)
            {
                if (CsConst.myRS232 != null)
                {
                    foreach (RS232 oTmp in CsConst.myRS232)
                    {
                        if (oTmp.DIndex == (MyActiveObj as RS232).DIndex)
                        {
                            oRS232 = oTmp;
                            break;
                        }
                    }
                }
            }
            this.MaxCount = maxcount;
            string strDevName = strname.Split('\\')[0].ToString();
            strRemark = strname.Split('\\')[1].ToString();
            SubNetID = Convert.ToByte(strDevName.Split('-')[0]);
            DevID = Convert.ToByte(strDevName.Split('-')[1]);
            this.MyintDeviceType = devicetype;

            cbType.Items.Clear();
            cbType.DropDownStyle = ComboBoxStyle.DropDownList;
            if (MyintDeviceType == 1041)
            {
                cbType.Items.Add(CsConst.mstrINIDefault.IniReadValue("TargetType", "00000", ""));
                cbType.Items.Add(CsConst.myPublicControlType[14].ControlTypeName);
            }
            else
            {
                cbType.Items.Add(CsConst.mstrINIDefault.IniReadValue("TargetType", "00000", ""));
                cbType.Items.Add(CsConst.myPublicControlType[4].ControlTypeName);
            }
            cbState.Items.Clear();
            cbState.DropDownStyle = ComboBoxStyle.DropDownList;
            cbState.Items.Add(CsConst.Status[0]);
            cbState.Items.Add(CsConst.Status[1]);
            cbType.SelectedIndexChanged += cbType_SelectedIndexChanged;
            txtNO.TextChanged += txtNO_TextChanged;
            cbState.SelectedIndexChanged += cbState_SelectedIndexChanged;
            txtRemark.TextChanged += txtRemark_TextChanged;
            uc1.UserControlValueChanged += uc1_UserControlValueChanged;
            uc2.UserControlValueChanged += uc2_UserControlValueChanged;
            dgvUV.Controls.Add(cbType);
            dgvUV.Controls.Add(txtRemark);
            dgvUV.Controls.Add(txtNO);
            dgvUV.Controls.Add(cbState);
            dgvUV.Controls.Add(uc1);
            dgvUV.Controls.Add(uc2);
            allVisible(false);
        }

        void uc2_UserControlValueChanged(object sender, UnivasalControl.TextChangeEventArgs e)
        {
            if (dgvUV.CurrentRow.Index < 0) return;
            if (dgvUV.RowCount <= 0) return;
            int index = dgvUV.CurrentRow.Index;
            string str = uc2.Text;
            if (uc2.Visible)
            {
                dgvUV[4, index].Value = str;
                string strTmp = dgvUV[4, index].Value.ToString();
                ModifyMultilinesIfNeeds(strTmp, 4,dgvUV);
            }
        }

        void uc1_UserControlValueChanged(object sender, UnivasalControl.TextChangeEventArgs e)
        {
            if (dgvUV.CurrentRow.Index < 0) return;
            if (dgvUV.RowCount <= 0) return;
            int index = dgvUV.CurrentRow.Index;
            string str = uc1.Text;
            if (uc1.Visible)
            {
                dgvUV[3, index].Value = str;
                string strTmp = dgvUV[3, index].Value.ToString();
                ModifyMultilinesIfNeeds(strTmp, 3,dgvUV);
            }
        }

        void cbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dgvUV.CurrentRow.Index < 0) return;
            if (dgvUV.RowCount <= 0) return;
            int index = dgvUV.CurrentRow.Index;
            dgvUV[2, index].Value = cbType.Text;
            ModifyMultilinesIfNeeds(dgvUV[2, index].Value.ToString(), 2, dgvUV);
        }

        private void allVisible(bool TF)
        {
            cbType.Visible = TF;
            cbState.Visible = TF;
            txtNO.Visible = TF;
            txtRemark.Visible = TF;
            uc1.Visible = TF;
            uc2.Visible = TF;
        }

        void txtNO_TextChanged(object sender, EventArgs e)
        {
            if (dgvUV.CurrentRow.Index < 0) return;
            if (dgvUV.RowCount <= 0) return;
            int index = dgvUV.CurrentRow.Index;
            dgvUV[3, index].Value = txtNO.Text + "(" +
                                       CsConst.WholeTextsList[2513].sDisplayName + ")";
            ModifyMultilinesIfNeeds(dgvUV[3, index].Value.ToString(), 3, dgvUV);
        }

        void cbState_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dgvUV.CurrentRow.Index < 0) return;
            if (dgvUV.RowCount <= 0) return;
            int index = dgvUV.CurrentRow.Index;
            dgvUV[4, index].Value = cbState.Text + "(" +
                                    CsConst.WholeTextsList[2529].sDisplayName + ")";
            ModifyMultilinesIfNeeds(dgvUV[4, index].Value.ToString(), 4, dgvUV);
        }

        void txtRemark_TextChanged(object sender, EventArgs e)
        {
            if (dgvUV.CurrentRow.Index < 0) return;
            if (dgvUV.RowCount <= 0) return;
            int index = dgvUV.CurrentRow.Index;
            dgvUV[1, index].Value = txtRemark.Text;
            ModifyMultilinesIfNeeds(dgvUV[1, index].Value.ToString(), 1, dgvUV);
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

        private void txtFrm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar) || e.KeyChar == 8)
                e.Handled = false;
            else
                e.Handled = true;
        }

        private void txtFrm_TextChanged(object sender, EventArgs e)
        {
            string str = txtFrm.Text;
            int num = Convert.ToInt32(txtTo.Text);
            if (HDLPF.IsRightNumStringMode(str, 1, num))
            {
                txtFrm.SelectionStart = txtFrm.Text.Length;
            }
            else
            {
                if (txtFrm.Text == null || txtFrm.Text == "")
                {
                    txtFrm.Text = "1";
                }
                else
                {
                    txtTo.Text = txtFrm.Text;
                    txtTo.Focus();
                    txtTo.SelectionStart = txtTo.Text.Length;
                }
            }
        }

        private void txtTo_TextChanged(object sender, EventArgs e)
        {
            string str = txtTo.Text;
            int num = Convert.ToInt32(txtFrm.Text);
            txtTo.Text = HDLPF.IsNumStringMode(str, num, MaxCount);
            txtTo.SelectionStart = txtTo.Text.Length;
        }

        private void btnSure_Click(object sender, EventArgs e)
        {
            if (oRS232 != null)
            {
                if (Type == 1)
                    oRS232.myBUS2RS = new List<RS232.BUS2RS>();
                else if (Type == 2)
                    oRS232.myBUS2485 = new List<RS232.BUS2RS>();
            }
            else if (oMHRCU != null)
            {
                if (Type == 1)
                    oMHRCU.myBUS2RS = new List<MHRCU.BUS2RS>();
                else if (Type == 2)
                    oMHRCU.myBUS2485 = new List<MHRCU.BUS2RS>();
            }
            allVisible(false);
            dgvUV.Rows.Clear();
            Cursor.Current = Cursors.WaitCursor;
            btnSure.Enabled = false;
            byte[] arayTmp = new byte[1];
            byte bytFrm = Convert.ToByte(Convert.ToInt32(txtFrm.Text));
            byte bytTo = Convert.ToByte(txtTo.Text);
            int CMD1 = 0xE420;
            int CMD2 = 0xE428;
            if (Type == 1)
            {
                CMD1 = 0xE420;
                CMD2 = 0xE428;
            }
            else if (Type == 2)
            {
                CMD1 = 0xDA61;
                CMD2 = 0xDA69;
            }
            for (byte byt = bytFrm; byt <= bytTo; byt++)
            {
                if (oRS232 != null)
                {
                    RS232.BUS2RS temp = new RS232.BUS2RS();
                    temp.ID = Convert.ToByte(byt);
                    arayTmp = new byte[1];
                    arayTmp[0] = Convert.ToByte(byt);
                    if (CsConst.mySends.AddBufToSndList(arayTmp, CMD1, SubNetID, DevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(MyintDeviceType)) == true)
                    {
                        temp.bytType = CsConst.myRevBuf[27];
                        temp.bytParam1 = CsConst.myRevBuf[28];
                        temp.bytParam2 = CsConst.myRevBuf[29];
                        temp.bytParam3 = CsConst.myRevBuf[30];
                        temp.bytParam4 = CsConst.myRevBuf[31];
                    }
                    else
                    {
                        Cursor.Current = Cursors.Default;
                        return;
                    }

                    HDLUDP.TimeBetwnNext(1);
                    if (CsConst.mySends.AddBufToSndList(arayTmp, CMD2, SubNetID, DevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(MyintDeviceType)) == true)
                    {
                        byte[] arayRemark = new byte[20];
                        for (int intI = 0; intI < 20; intI++) { arayRemark[intI] = CsConst.myRevBuf[27 + intI]; }
                        temp.Remark = HDLPF.Byte2String(arayRemark);

                    }
                    else
                    {
                        Cursor.Current = Cursors.Default;
                        return;
                    }

                    if (Type == 1)
                        oRS232.myBUS2RS.Add(temp);
                    else if (Type == 2)
                        oRS232.myBUS2485.Add(temp);
                }
                else if (oMHRCU != null)
                {
                    MHRCU.BUS2RS temp = new MHRCU.BUS2RS();
                    temp.ID = Convert.ToByte(byt);
                    arayTmp = new byte[1];
                    arayTmp[0] = Convert.ToByte(byt);
                    if (CsConst.mySends.AddBufToSndList(arayTmp, CMD1, SubNetID, DevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(MyintDeviceType)) == true)
                    {
                        temp.bytType = CsConst.myRevBuf[27];
                        temp.bytParam1 = CsConst.myRevBuf[28];
                        temp.bytParam2 = CsConst.myRevBuf[29];

                        System.Threading.Thread.Sleep(1);
                    }
                    else
                    {
                        Cursor.Current = Cursors.Default;
                        return;
                    }

                    if (CsConst.mySends.AddBufToSndList(arayTmp, CMD2, SubNetID, DevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(MyintDeviceType)) == true)
                    {
                        byte[] arayRemark = new byte[20];
                        for (int intI = 0; intI < 20; intI++) { arayRemark[intI] = CsConst.myRevBuf[27 + intI]; }
                        temp.Remark = HDLPF.Byte2String(arayRemark);

                        System.Threading.Thread.Sleep(1);
                    }
                    else
                    {
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                    if (Type == 1)
                        oMHRCU.myBUS2RS.Add(temp);
                    else if (Type == 2)
                        oMHRCU.myBUS2485.Add(temp);
                }
                HDLUDP.TimeBetwnNext(1);
            }
            btnSure.Enabled = true;
            Cursor.Current = Cursors.Default;
            getDataGridViewList();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                allVisible(false);
                Cursor.Current = Cursors.WaitCursor;
                btnSave.Enabled = false;
                int CMD1 = 0xE422;
                int CMD2 = 0xE42A;
                if (Type == 1)
                {
                    CMD1 = 0xE422;
                    CMD2 = 0xE42A;
                }
                else if (Type == 2)
                {
                    CMD1 = 0xDA63;
                    CMD2 = 0xDA6B;
                }
                for (int i = 0; i < dgvUV.Rows.Count; i++)
                {
                    byte[] arayTmp = new byte[4];
                    if (MyintDeviceType == 1041)
                    {
                        arayTmp = new byte[6];
                    }
                    arayTmp[0] = Convert.ToByte(dgvUV[0, i].Value.ToString());
                    if (dgvUV[1, i].Value == null) dgvUV[1, i].Value = "";
                    string strRemark = dgvUV[1, i].Value.ToString();
                    string strType = dgvUV[2, i].Value.ToString();
                    string str1 = dgvUV[3, i].Value.ToString();
                    string str2 = dgvUV[4, i].Value.ToString();
                    if (str1.Contains("(")) str1 = str1.Split('(')[0].ToString();
                    if (str2.Contains("(")) str2 = str2.Split('(')[0].ToString();
                    if (MyintDeviceType == 1041)
                    {
                        if (strType == CsConst.myPublicControlType[14].ControlTypeName)
                            arayTmp[1] = 0xBB;
                        arayTmp[2] = Convert.ToByte(str1.Split('/')[0].ToString());
                        arayTmp[3] = Convert.ToByte(str1.Split('/')[1].ToString());
                        arayTmp[4] = Convert.ToByte(str2.Split('/')[0].ToString());
                        arayTmp[5] = Convert.ToByte(str2.Split('/')[1].ToString());
                    }
                    else
                    {
                        if (strType == CsConst.myPublicControlType[4].ControlTypeName)
                            arayTmp[1] = 0x58;
                        arayTmp[2] = Convert.ToByte(str1);
                        if (str2 == CsConst.Status[0])
                            arayTmp[3] = 0;
                        else if (str2 == CsConst.Status[1])
                            arayTmp[3] = 255;
                    }
                    if (CsConst.mySends.AddBufToSndList(arayTmp, CMD1, SubNetID, DevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(MyintDeviceType)) == true)
                    {
                        if (oRS232 != null)
                        {
                            if (Type == 1)
                            {
                                oRS232.myBUS2RS[i].bytType = arayTmp[1];
                                oRS232.myBUS2RS[i].bytParam1 = arayTmp[2];
                                oRS232.myBUS2RS[i].bytParam2 = arayTmp[3];
                                if (MyintDeviceType == 1041)
                                {
                                    oRS232.myBUS2RS[i].bytParam3 = arayTmp[4];
                                    oRS232.myBUS2RS[i].bytParam4 = arayTmp[5];
                                }
                            }
                            else if (Type == 2)
                            {
                                oRS232.myBUS2485[i].bytType = arayTmp[1];
                                oRS232.myBUS2485[i].bytParam1 = arayTmp[2];
                                oRS232.myBUS2485[i].bytParam2 = arayTmp[3];
                            }
                        }
                        else if (oMHRCU != null)
                        {
                            if (Type == 1)
                            {
                                oMHRCU.myBUS2RS[i].bytType = arayTmp[1];
                                oMHRCU.myBUS2RS[i].bytParam1 = arayTmp[2];
                                oMHRCU.myBUS2RS[i].bytParam2 = arayTmp[3];
                            }
                            else if (Type == 2)
                            {
                                oMHRCU.myBUS2485[i].bytType = arayTmp[1];
                                oMHRCU.myBUS2485[i].bytParam1 = arayTmp[2];
                                oMHRCU.myBUS2485[i].bytParam2 = arayTmp[3];
                            }
                        }
                    }
                    else break;
                    HDLUDP.TimeBetwnNext(20);

                    arayTmp = new byte[21];
                    arayTmp[0] = Convert.ToByte(dgvUV[0, i].Value.ToString());
                    byte[] arayRemark = HDLUDP.StringToByte(strRemark);
                    if (arayRemark.Length <= 20)
                        arayRemark.CopyTo(arayTmp, 1);
                    else
                        Array.Copy(arayRemark, 0, arayTmp, 1, 20);
                    if (CsConst.mySends.AddBufToSndList(arayTmp, CMD2, SubNetID, DevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(MyintDeviceType)) == true)
                    {
                        if (oRS232 != null)
                        {
                            if (Type == 1)
                                oRS232.myBUS2RS[i].Remark = strRemark;
                            else if (Type == 2)
                                oRS232.myBUS2485[i].Remark = strRemark;
                        }
                        else if (oMHRCU != null)
                        {
                            if (Type == 1)
                                oMHRCU.myBUS2RS[i].Remark = strRemark;
                            else if (Type == 2)
                                oMHRCU.myBUS2485[i].Remark = strRemark;
                        }
                    }
                    else break;
                    HDLUDP.TimeBetwnNext(20);
                }
            }
            catch
            {
            }
            btnSave.Enabled = true;
            Cursor.Current = Cursors.Default;
        }

        private void dgvUV_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                txtRemark.Text = dgvUV[1, e.RowIndex].Value.ToString();
                addcontrols(1, e.RowIndex, txtRemark, dgvUV);

                cbType.Text = dgvUV[2, e.RowIndex].Value.ToString();
                addcontrols(2, e.RowIndex, cbType, dgvUV);

                string str1 = dgvUV[3, e.RowIndex].Value.ToString();
                string str2 = dgvUV[4, e.RowIndex].Value.ToString();
                if (str1.Contains("(")) str1 = str1.Split('(')[0].ToString();
                if (str2.Contains("(")) str2 = str2.Split('(')[0].ToString();

                if (MyintDeviceType == 1041)
                {
                    if (str1.Contains("/"))
                    {
                        string strtmp = str1;
                        str1 = HDLPF.IsNumStringMode(strtmp.Split('/')[0].ToString(), 0, 255) + "/" +
                               HDLPF.IsNumStringMode(strtmp.Split('/')[1].ToString(), 0, 255);
                        uc1.Text = str1;
                    }
                    else
                    {
                        uc1.Text = "0/0";
                    }
                    addcontrols(3, e.RowIndex, uc1, dgvUV);

                    if (str2.Contains("/"))
                    {
                        string strtmp = str2;
                        str2 = HDLPF.IsNumStringMode(strtmp.Split('/')[0].ToString(), 0, 255) + "/" +
                               HDLPF.IsNumStringMode(strtmp.Split('/')[1].ToString(), 0, 255);
                        uc2.Text = str2;
                    }
                    else
                    {
                        uc2.Text = "0/0";
                    }
                    addcontrols(4, e.RowIndex, uc2, dgvUV);
                }
                else
                {
                    txtNO.Text = str1;
                    addcontrols(3, e.RowIndex, txtNO, dgvUV);

                    cbState.Text = str2;
                    addcontrols(4, e.RowIndex, cbState, dgvUV);
                }
            }
            if (cbType.SelectedIndex < 0) cbType.SelectedIndex = 0;
            if (cbState.SelectedIndex < 0) cbState.SelectedIndex = 0;
            if (txtRemark.Visible) txtRemark_TextChanged(null, null);
            if (cbType.Visible) cbType_SelectedIndexChanged(null, null);
            if (txtNO.Visible) txtNO_TextChanged(null, null);
            if (cbState.Visible) cbState_SelectedIndexChanged(null, null);
            if (uc1.Visible) uc1_UserControlValueChanged(null, null);
            if (uc2.Visible) uc2_UserControlValueChanged(null, null);
        }

        private void addcontrols(int col, int row, Control con, DataGridView dgv)
        {
            con.Show();
            con.Visible = true;
            Rectangle rect = dgv.GetCellDisplayRectangle(col, row, true);
            con.Size = rect.Size;
            con.Top = rect.Top;
            con.Left = rect.Left;
        }

        private void FrmMCBUS_Load(object sender, EventArgs e)
        {
            lbSubValue.Text = SubNetID.ToString();
            lbDevValue.Text = DevID.ToString();
            lbRemarkValue.Text = strRemark;
            
            lbTarget.Text = lbTarget.Text.Split('-')[0].ToString() + "-" + MaxCount.ToString() + ")";
            if (MyintDeviceType == 1041)
            {
                cl31.HeaderText = CsConst.mstrINIDefault.IniReadValue("Public", "99546", "");
                BUSRS232Status.HeaderText = CsConst.mstrINIDefault.IniReadValue("Public", "99547", "");
            }
            if (Type == 1)
            {
                this.Text = CsConst.mstrINIDefault.IniReadValue("Public", "00712", "");
            }
            else if (Type == 2)
            {
                this.Text = CsConst.mstrINIDefault.IniReadValue("Public", "00716", "");
            }
            getDataGridViewList();
        }
        private void getDataGridViewList()
        {
            try
            {
                dgvUV.Rows.Clear();
                int Length = 0;
                if (oRS232 != null)
                {
                    if (Type == 1)
                        Length = oRS232.myBUS2RS.Count;
                    else if (Type == 2)
                        Length = oRS232.myBUS2485.Count;
                }
                else if (oMHRCU != null)
                {
                    if (Type == 1)
                        Length = oMHRCU.myBUS2RS.Count;
                    else if (Type == 2)
                        Length = oMHRCU.myBUS2485.Count;
                }
                for (int i = 0; i < Length; i++)
                {
                    if (oRS232 != null)
                    {
                        RS232.BUS2RS temp = null;
                        if (Type == 1)
                        {
                            temp = oRS232.myBUS2RS[i];
                        }
                        else if (Type == 2)
                        {
                            temp = oRS232.myBUS2485[i];
                        }
                        if (MyintDeviceType == 1041)
                        {
                            string strType = CsConst.mstrINIDefault.IniReadValue("TargetType", "00000", "");
                            if (temp.bytType == 0xBB) strType = CsConst.myPublicControlType[14].ControlTypeName;
                            string strParam1 = temp.bytParam1.ToString() + "/" + temp.bytParam2.ToString();
                            string strParam2 = temp.bytParam3.ToString() + "/" + temp.bytParam4.ToString();
                            object[] obj = new object[] { temp.ID.ToString(), temp.Remark.ToString(), strType, strParam1, strParam2 };
                            dgvUV.Rows.Add(obj);
                        }
                        else
                        {
                            string strType = CsConst.mstrINIDefault.IniReadValue("TargetType", "00000", "");
                            if (temp.bytType == 0x58) strType = CsConst.myPublicControlType[4].ControlTypeName;
                            string strParam1 = temp.bytParam1.ToString() + "(" +
                                               CsConst.WholeTextsList[2513].sDisplayName + ")";
                            string strParam2 = CsConst.Status[0] + "(" +
                                               CsConst.WholeTextsList[2529].sDisplayName + ")";
                            if (temp.bytParam2 == 255)
                                strParam2 = CsConst.Status[1] + "(" +
                                            CsConst.WholeTextsList[2529].sDisplayName + ")";
                            object[] obj = new object[] { temp.ID.ToString(), temp.Remark.ToString(), strType, strParam1, strParam2 };
                            dgvUV.Rows.Add(obj);
                        }
                    }
                    else if (oMHRCU != null)
                    {
                        MHRCU.BUS2RS temp = null;
                        if (Type == 1)
                        {
                            temp = oMHRCU.myBUS2RS[i];
                        }
                        else if (Type == 2)
                        {
                            temp = oMHRCU.myBUS2485[i];
                        }
                        string strType = CsConst.mstrINIDefault.IniReadValue("TargetType", "00000", "");
                        if (temp.bytType == 0x58) strType = CsConst.myPublicControlType[4].ControlTypeName;
                        string strParam1 = temp.bytParam1.ToString() + "(" +
                                           CsConst.WholeTextsList[2513].sDisplayName + ")";
                        string strParam2 = CsConst.Status[0] + "(" +
                                           CsConst.WholeTextsList[2529].sDisplayName + ")";
                        if (temp.bytParam2 == 255)
                            strParam2 = CsConst.Status[1] + "(" +
                                        CsConst.WholeTextsList[2529].sDisplayName + ")";
                        object[] obj = new object[] { temp.ID.ToString(), temp.Remark.ToString(), strType, strParam1, strParam2 };
                        dgvUV.Rows.Add(obj);
                    }
                }
            }
            catch
            {
            }
        }
    }
}
