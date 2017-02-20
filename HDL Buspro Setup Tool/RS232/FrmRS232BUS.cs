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
    public partial class FrmRS232BUS : Form
    {
        private int Type;
        private byte SubNetID;
        private byte DevID;
        private int MyintDeviceType;
        private string strRemark = "";
        private RS232 oRS232;
        private MHRCU oMHRCU;
        private ComboBox cbEnable = new ComboBox();
        private ComboBox cb232ControlMode = new ComboBox();
        private TextBox txt232Remark = new TextBox();
        private TextBox txt232Command = new TextBox();
        private ComboBox cbEndChar = new ComboBox();

        private TextBox cbControlType = new TextBox();
        private TextBox txtPanelID = new TextBox();
        private ComboBox cbKeyType = new ComboBox();
        private int MaxCount = 200;
        private object MyActiveObj;
        public FrmRS232BUS()
        {
            InitializeComponent();
        }

        public FrmRS232BUS(string strname, object obj, int devicetype, int maxcount,int type)
        {
            InitializeComponent();
            this.Type = type;
            oRS232 = null;
            oMHRCU = null;
            MyActiveObj = obj;
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
            #region
            cbEndChar.Items.Clear();
            cbEndChar.DropDownStyle = ComboBoxStyle.DropDownList;
            cbEndChar.Items.Add("NONE");
            cbEndChar.Items.Add("<CR>");
            cbEndChar.Items.Add("<CR+LF>");
            cbKeyType.Items.Clear();
            cbKeyType.Items.Add(CsConst.mstrINIDefault.IniReadValue("keyMode", "00000", ""));
            cbKeyType.Items.Add(CsConst.mstrINIDefault.IniReadValue("keyMode", "00002", ""));
            cbKeyType.Items.Add(CsConst.mstrINIDefault.IniReadValue("keyMode", "00003", ""));
            cbKeyType.Items.Add(CsConst.mstrINIDefault.IniReadValue("keyMode", "00001", ""));
            cbKeyType.Items.Add(CsConst.mstrINIDefault.IniReadValue("keyMode", "00004", ""));
            cbKeyType.Items.Add(CsConst.mstrINIDefault.IniReadValue("keyMode", "00005", ""));
            cbKeyType.Items.Add(CsConst.mstrINIDefault.IniReadValue("keyMode", "00007", ""));
            
            cbKeyType.DropDownStyle = ComboBoxStyle.DropDownList;
            cbEnable.Items.Clear();
            cbEnable.DropDownStyle = ComboBoxStyle.DropDownList;
            cbEnable.Items.Add(CsConst.WholeTextsList[1775].sDisplayName);
            cbEnable.Items.Add(CsConst.mstrINIDefault.IniReadValue("public", "00042", ""));
            cb232ControlMode.Items.Clear();
            cb232ControlMode.DropDownStyle = ComboBoxStyle.DropDownList;
            cb232ControlMode.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99838", ""));
            cb232ControlMode.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99839", ""));
            cbEnable.SelectedIndexChanged += cbEnable_SelectedIndexChanged;
            cb232ControlMode.SelectedIndexChanged += cb232ControlMode_SelectedIndexChanged;
            txt232Command.TextChanged += txt232Command_TextChanged;
            txt232Remark.TextChanged += txt232Remark_TextChanged;
            cbEndChar.SelectedIndexChanged += cbEndChar_SelectedIndexChanged;
            cbControlType.TextChanged += cbControlType_SelectedIndexChanged;
            cbKeyType.SelectedIndexChanged += cbKeyType_SelectedIndexChanged;
            txtPanelID.KeyPress += txtFrm_KeyPress;
            txtPanelID.TextChanged += txtPanelID_TextChanged;
            dgvCommand.Controls.Add(cb232ControlMode);
            dgvCommand.Controls.Add(cbEnable);
            dgvCommand.Controls.Add(txt232Command);
            dgvCommand.Controls.Add(txt232Remark);
            dgvCommand.Controls.Add(cbEndChar);
            dgvCommand.Controls.Add(cbControlType);
            dgvCommand.Controls.Add(cbKeyType);
            dgvCommand.Controls.Add(txtPanelID);
            #endregion
            setAllControlVisible(false);
        }

        void txtPanelID_TextChanged(object sender, EventArgs e)
        {
            if (dgvCommand.CurrentRow.Index < 0) return;
            if (dgvCommand.RowCount <= 0) return;
            if (txtPanelID.Visible)
            {
                int index = dgvCommand.CurrentRow.Index;
                dgvCommand[7, index].Value = txtPanelID.Text;
                ModifyMultilinesIfNeeds(dgvCommand[7, index].Value.ToString(), 7, dgvCommand);
            }
        }

        void cbKeyType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dgvCommand.CurrentRow.Index < 0) return;
            if (dgvCommand.RowCount <= 0) return;
            if (cbKeyType.Visible)
            {
                int index = dgvCommand.CurrentRow.Index;
                dgvCommand[6, index].Value = cbKeyType.Text;
                ModifyMultilinesIfNeeds(dgvCommand[6, index].Value.ToString(), 6, dgvCommand);
            }
        }

        void cbControlType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dgvCommand.CurrentRow.Index < 0) return;
            if (dgvCommand.RowCount <= 0) return;
            if (cbControlType.Visible)
            {
                int index = dgvCommand.CurrentRow.Index;
                dgvCommand[8, index].Value = cbControlType.Text;
                ModifyMultilinesIfNeeds(dgvCommand[8, index].Value.ToString(), 8, dgvCommand);
            }
        }

        void cbEndChar_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dgvCommand.CurrentRow.Index < 0) return;
            if (dgvCommand.RowCount <= 0) return;
            int index = dgvCommand.CurrentRow.Index;
            dgvCommand[5, index].Value = cbEndChar.Text;
            ModifyMultilinesIfNeeds(dgvCommand[5, index].Value.ToString(), 5, dgvCommand);
        }

        void txt232Remark_TextChanged(object sender, EventArgs e)
        {
            if (dgvCommand.CurrentRow.Index < 0) return;
            if (dgvCommand.RowCount <= 0) return;
            int index = dgvCommand.CurrentRow.Index;
            dgvCommand[1, index].Value = txt232Remark.Text;
            ModifyMultilinesIfNeeds(dgvCommand[1, index].Value.ToString(), 1, dgvCommand);
        }

        void txt232Command_TextChanged(object sender, EventArgs e)
        {
            if (dgvCommand.CurrentRow.Index < 0) return;
            if (dgvCommand.RowCount <= 0) return;
            int index = dgvCommand.CurrentRow.Index;
            if (dgvCommand[3, index].Value.ToString() == CsConst.mstrINIDefault.IniReadValue("Public", "99838", ""))
            {
                dgvCommand[4, index].Value = txt232Command.Text;
                ModifyMultilinesIfNeeds(dgvCommand[4, index].Value.ToString(), 4, dgvCommand);
            }
            else if (dgvCommand[3, index].Value.ToString() == CsConst.mstrINIDefault.IniReadValue("Public", "99839", ""))
            {
                string[] strFiter = new string[] { " ","1","2","3","4","5","6","7","8","9","0",
                                                   "A","B","C","D","E","F","a","b","c","d","e","f"};
                string str = txt232Command.Text;
                for (int i = 0; i < str.Length; i++)
                {
                    if (!strFiter.Contains(str.Substring(i, 1)))
                    {
                        str = str.Substring(0, i).Trim();
                        break;
                    }
                }
                txt232Command.Text = str;
                if (txt232Command.Text.Length > 0) txt232Command.SelectionStart = txt232Command.Text.Length;
                dgvCommand[4, index].Value = str;
                ModifyMultilinesIfNeeds(dgvCommand[4, index].Value.ToString(), 4, dgvCommand);
            }
        }

        void cb232ControlMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbEndChar.Visible = false;
            if (dgvCommand.CurrentRow.Index < 0) return;
            if (dgvCommand.RowCount <= 0) return;
            int index = dgvCommand.CurrentRow.Index;
            dgvCommand[3, index].Value = cb232ControlMode.Text;
            ModifyMultilinesIfNeeds(dgvCommand[3, index].Value.ToString(), 3, dgvCommand);
            if (cb232ControlMode.SelectedIndex == 0)
            {
                cbEndChar.SelectedIndex = cbEndChar.Items.IndexOf(dgvCommand[5, index].Value.ToString());
                addcontrols(5, index, cbEndChar, dgvCommand);
                if (cbEndChar.SelectedIndex < 0) cbEndChar.SelectedIndex = 0;
                cbEndChar_SelectedIndexChanged(null, null);
            }
            else if (cb232ControlMode.SelectedIndex == 1)
            {
                txt232Command.Text = dgvCommand[4, index].Value.ToString();
                txt232Command_TextChanged(null, null);
                dgvCommand[5, index].Value = cbEndChar.Items[0].ToString();
            }
        }

        void cbEnable_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dgvCommand.CurrentRow.Index < 0) return;
            if (dgvCommand.RowCount <= 0) return;
            int index = dgvCommand.CurrentRow.Index;
            dgvCommand[2, index].Value = cbEnable.Text;
            ModifyMultilinesIfNeeds(dgvCommand[2, index].Value.ToString(), 2, dgvCommand);
        }

        private void txtFrm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar) || e.KeyChar == 8)
                e.Handled = false;
            else
                e.Handled = true;
        }

        private void dgvCommand_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                setAllControlVisible(false);
                if (e.RowIndex >= 0)
                {
                    txt232Remark.Text = dgvCommand[1, e.RowIndex].Value.ToString();
                    addcontrols(1, e.RowIndex, txt232Remark, dgvCommand);

                    cbEnable.SelectedIndex = cbEnable.Items.IndexOf(dgvCommand[2, e.RowIndex].Value.ToString());
                    addcontrols(2, e.RowIndex, cbEnable, dgvCommand);

                    if (MyintDeviceType == 1041)
                    {
                        cbKeyType.SelectedIndex = cbKeyType.Items.IndexOf(dgvCommand[6, e.RowIndex].Value.ToString());
                        addcontrols(6, e.RowIndex, cbKeyType, dgvCommand);
                        txtPanelID.Text = HDLPF.IsNumStringMode(dgvCommand[7, e.RowIndex].Value.ToString(), 0, 255);
                        addcontrols(7, e.RowIndex, txtPanelID, dgvCommand);
                        cbControlType.Text = HDLPF.IsNumStringMode(dgvCommand[8, e.RowIndex].Value.ToString(), 0, 255); ;
                        addcontrols(8, e.RowIndex, cbControlType, dgvCommand);
                    }
                    else
                    {
                        cb232ControlMode.SelectedIndex = cb232ControlMode.Items.IndexOf(dgvCommand[3, e.RowIndex].Value.ToString());
                        addcontrols(3, e.RowIndex, cb232ControlMode, dgvCommand);

                        txt232Command.Text = dgvCommand[4, e.RowIndex].Value.ToString();
                        addcontrols(4, e.RowIndex, txt232Command, dgvCommand);
                    }
                }
                if (cbEnable.SelectedIndex < 0) cbEnable.SelectedIndex = 0;
                if (cb232ControlMode.SelectedIndex < 0) cb232ControlMode.SelectedIndex = 0;
                if (cbKeyType.SelectedIndex < 0) cbKeyType.SelectedIndex = 0;
                if (txt232Remark.Visible) txt232Remark_TextChanged(null, null);
                if (cbEnable.Visible) cbEnable_SelectedIndexChanged(null, null);
                if (cb232ControlMode.Visible) cb232ControlMode_SelectedIndexChanged(null, null);
                if (txt232Remark.Visible) txt232Remark_TextChanged(null, null);
                if (cbControlType.Visible) cbControlType_SelectedIndexChanged(null, null);
                if (txtPanelID.Visible) txtPanelID_TextChanged(null, null);
                if (cbKeyType.Visible) cbKeyType_SelectedIndexChanged(null, null);
            }
            catch
            {
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

        private void setAllControlVisible(bool TF)
        {
            cb232ControlMode.Visible = TF;
            cbEnable.Visible = TF;
            txt232Command.Visible = TF;
            txt232Remark.Visible = TF;
            cbEndChar.Visible = TF;
            cbControlType.Visible = TF;
            cbKeyType.Visible = TF;
            txtPanelID.Visible = TF;
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
        private void FrmRS232BUS_Load(object sender, EventArgs e)
        {
            if (MyintDeviceType == 1041)
            {
                clA3.Visible = false;
                clA4.Visible = false;
                clA5.Visible = false;
                clA6.Visible = false;
                clA7.Visible = true;
                clA8.Visible = true;
                clA9.Visible = true;
            }
            lbSubValue.Text = SubNetID.ToString();
            lbDevValue.Text = DevID.ToString();
            lbRemarkValue.Text = strRemark;
            lbTarget.Text = lbTarget.Text.Split('-')[0].ToString() + "-" + MaxCount.ToString() + ")";

            if (Type == 1)
            {
                this.Text = CsConst.mstrINIDefault.IniReadValue("Public", "00710", "");
            }
            else if (Type == 2)
            {
                this.Text = CsConst.mstrINIDefault.IniReadValue("Public", "00714", "");
            }
            getDataGridViewList();

        }

        private void btnSure_Click(object sender, EventArgs e)
        {
            try
            {
                int wdMaxValue = int.Parse(CsConst.mstrINIDefault.IniReadValue("DeviceType" + MyintDeviceType.ToString(), "MaxValue", "33"));
                setAllControlVisible(false);
                if (oRS232 != null)
                {
                    if (Type == 1)
                        oRS232.myRS2BUS = new List<Rs232ToBus>();
                    else if (Type == 2)
                        oRS232.my4852BUS = new List<Rs232ToBus>();
                }
                else if (oMHRCU != null)
                {
                    wdMaxValue = 33;
                    if (Type == 1)
                        oMHRCU.myRS2BUS = new List<Rs232ToBus>();
                    else if (Type == 2)
                        oMHRCU.my4852BUS =new List<Rs232ToBus>();
                }
                Cursor.Current = Cursors.WaitCursor;
                dgvCommand.Rows.Clear();
                btnSure.Enabled = false;
                byte[] arayTmp = new byte[1];
                int bytFrm = Convert.ToInt32(txtFrm.Text);
                int bytTo = Convert.ToInt32(txtTo.Text);
                int CMD1 = 0xE410;
                int CMD2 = 0xE418;
                if (Type == 1)
                {
                    CMD1 = 0xE410;
                    CMD2 = 0xE418;
                }
                else if (Type == 2)
                {
                    CMD1 = 0xDA51;
                    CMD2 = 0xDA59;
                }
                for (int byt = bytFrm; byt <= bytTo; byt++)
                {
                    if (oRS232 != null)
                    {
                        Rs232ToBus temp = new Rs232ToBus();
                        temp.rs232Param = new Rs232Param(); // RS232.RS232Param();

                        if (MyintDeviceType == 1041)
                        {
                            arayTmp = new byte[2];
                            arayTmp[0] = Convert.ToByte(byt / 256);
                            arayTmp[1] = Convert.ToByte(byt % 256);
                        }
                        else
                        {
                            arayTmp[0] = Convert.ToByte(byt);
                        }
                        if (CsConst.mySends.AddBufToSndList(arayTmp, CMD1, SubNetID, DevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(MyintDeviceType)) == true)
                        {
                            temp.ID = byt;
                            if (MyintDeviceType == 1041)
                            {
                                temp.rs232Param.endFlag = CsConst.myRevBuf[28];
                                temp.rs232Param.type = CsConst.myRevBuf[29];
                                //temp.rs232Param.rsCmd = new byte[wdMaxValue + 1];
                                //Array.Copy(CsConst.myRevBuf, 30, temp.TmpRS232.RSCMD, 0, temp.TmpRS232.RSCMD.Length);
                            }
                            else
                            {
                                temp.rs232Param.endFlag = CsConst.myRevBuf[27];
                                temp.rs232Param.type = CsConst.myRevBuf[28];
                               // temp.rs232Param.rsCmd = new byte[wdMaxValue + 1];
                               // Array.Copy(CsConst.myRevBuf, 29, temp.TmpRS232.RSCMD, 0, temp.TmpRS232.RSCMD.Length);
                            }
                            HDLUDP.TimeBetwnNext(1);
                        }
                        else
                        {
                            Cursor.Current = Cursors.Default;
                            return;
                        }

                        if (CsConst.mySends.AddBufToSndList(arayTmp, CMD2, SubNetID, DevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(MyintDeviceType)) == true)
                        {

                            byte[] arayRemark = new byte[20];
                            if (MyintDeviceType == 1041)
                            {
                                for (int intI = 0; intI < 20; intI++) { arayRemark[intI] = CsConst.myRevBuf[28 + intI]; }
                            }
                            else
                            {
                                for (int intI = 0; intI < 20; intI++) { arayRemark[intI] = CsConst.myRevBuf[27 + intI]; }
                            }
                            temp.remark = HDLPF.Byte2String(arayRemark);
                            HDLUDP.TimeBetwnNext(1);
                        }
                        else
                        {
                            Cursor.Current = Cursors.Default;
                            return;
                        }

                        if (Type == 1)
                            oRS232.myRS2BUS.Add(temp);
                        else if (Type == 2)
                            oRS232.my4852BUS.Add(temp);
                    }
                    else if (oMHRCU != null)
                    {
                        Rs232ToBus temp = new Rs232ToBus();
                        temp.rs232Param = new Rs232Param();
                        arayTmp[0] = Convert.ToByte(byt);
                        if (CsConst.mySends.AddBufToSndList(arayTmp, CMD1, SubNetID, DevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(MyintDeviceType)) == true)
                        {
                            temp.ID = Convert.ToByte(byt);
                            temp.rs232Param.enable = CsConst.myRevBuf[27];
                            temp.rs232Param.type = CsConst.myRevBuf[28];
                            //temp.rs232Param.RSCMD = new byte[33 + 1];
                           // Array.Copy(CsConst.myRevBuf, 29, temp.TmpRS232.RSCMD, 0, temp.TmpRS232.RSCMD.Length);

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
                            temp.remark = HDLPF.Byte2String(arayRemark);

                            System.Threading.Thread.Sleep(1);
                        }
                        else
                        {
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                        if (Type == 1)
                            oMHRCU.myRS2BUS.Add(temp);
                        else if (Type == 2)
                            oMHRCU.my4852BUS.Add(temp);
                    }
                }
            }
            catch
            {
            }
            btnSure.Enabled = true;
            Cursor.Current = Cursors.Default;
            getDataGridViewList();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                int wdMaxValue = int.Parse(CsConst.mstrINIDefault.IniReadValue("DeviceType" + MyintDeviceType.ToString(), "MaxValue", "33"));
                if (oMHRCU != null) wdMaxValue = 33;
                btnSave.Enabled = false;
                Cursor.Current = Cursors.WaitCursor;
                int CMD1 = 0xE412;
                int CMD2 = 0xE41A;
                if (Type == 1)
                {
                    CMD1 = 0xE412;
                    CMD2 = 0xE41A;
                }
                else if (Type == 2)
                {
                    CMD1 = 0xDA53;
                    CMD2 = 0xDA5B;
                }
                for (int i = 0; i < dgvCommand.Rows.Count; i++)
                {
                    byte[] ArayTmp = new byte[wdMaxValue + 4];
                    string strEnable = dgvCommand[2, i].Value.ToString();
                    string strType = dgvCommand[3, i].Value.ToString();
                    string strCommand = dgvCommand[4, i].Value.ToString();
                    string strEnd = dgvCommand[5, i].Value.ToString();
                    string str1 = dgvCommand[6, i].Value.ToString();
                    string str2 = dgvCommand[7, i].Value.ToString();
                    string str3 = dgvCommand[8, i].Value.ToString();
                    string str4 = dgvCommand[9, i].Value.ToString();
                    
                    if (MyintDeviceType == 1041)
                    {
                        ArayTmp = new byte[wdMaxValue + 5];
                        int intID = Convert.ToInt32(dgvCommand[0, i].Value.ToString());
                        ArayTmp[0] = Convert.ToByte(intID / 256);
                        ArayTmp[1] = Convert.ToByte(intID % 256);
                        if (str1 == CsConst.mstrINIDefault.IniReadValue("keyMode", "00000", ""))
                            ArayTmp[2] = 0;
                        else if (str1 == CsConst.mstrINIDefault.IniReadValue("keyMode", "00002", ""))
                            ArayTmp[2] = 2;
                        else if (str1 == CsConst.mstrINIDefault.IniReadValue("keyMode", "00003", ""))
                            ArayTmp[2] = 3;
                        else if (str1 == CsConst.mstrINIDefault.IniReadValue("keyMode", "00001", ""))
                            ArayTmp[2] = 1;
                        else if (str1 == CsConst.mstrINIDefault.IniReadValue("keyMode", "00004", ""))
                            ArayTmp[2] = 4;
                        else if (str1 == CsConst.mstrINIDefault.IniReadValue("keyMode", "00005", ""))
                            ArayTmp[2] = 5;
                        else if (str1 == CsConst.mstrINIDefault.IniReadValue("keyMode", "00007", ""))
                            ArayTmp[2] = 7;
                        ArayTmp[3] = Convert.ToByte(HDLPF.IsNumStringMode(str2, 0, 255));
                        ArayTmp[4] = Convert.ToByte(HDLPF.IsNumStringMode(str3, 0, 255));
                        ArayTmp[ArayTmp.Length - 1] = 1;
                    }
                    else
                    {
                        ArayTmp[0] = Convert.ToByte(dgvCommand[0, i].Value.ToString());
                        ArayTmp[1] = Convert.ToByte(cbEnable.Items.IndexOf(dgvCommand[2, i].Value.ToString()));
                        if (strType == CsConst.mstrINIDefault.IniReadValue("Public", "99838", ""))
                        {
                            ArayTmp[2] = 0;
                            byte[] arayCommand = HDLUDP.StringToByte(strCommand);
                            if (strEnd == "NONE")
                            {
                                if (arayCommand.Length <= wdMaxValue)
                                {
                                    Array.Copy(arayCommand, 0, ArayTmp, 3, arayCommand.Length);
                                    ArayTmp[ArayTmp.Length - 1] = Convert.ToByte(arayCommand.Length);
                                }
                                else
                                {
                                    Array.Copy(arayCommand, 0, ArayTmp, 3, wdMaxValue);
                                    ArayTmp[ArayTmp.Length - 1] = Convert.ToByte(wdMaxValue);
                                }
                            }
                            else if (strEnd == "<CR>")
                            {
                                if (arayCommand.Length <= (wdMaxValue - 1))
                                {
                                    Array.Copy(arayCommand, 0, ArayTmp, 3, arayCommand.Length);
                                    ArayTmp[arayCommand.Length + 3] = 0X0D;
                                    ArayTmp[ArayTmp.Length - 1] = Convert.ToByte(arayCommand.Length + 1);
                                }
                                else
                                {
                                    Array.Copy(arayCommand, 0, ArayTmp, 3, (wdMaxValue - 1));
                                    ArayTmp[ArayTmp.Length - 2] = 0x0D;
                                    ArayTmp[ArayTmp.Length - 1] = Convert.ToByte(wdMaxValue); ;
                                }
                            }
                            else if (strEnd == "<CR+LF>")
                            {
                                if (arayCommand.Length <= (wdMaxValue - 2))
                                {
                                    Array.Copy(arayCommand, 0, ArayTmp, 3, arayCommand.Length);
                                    ArayTmp[arayCommand.Length + 3] = 0X0D;
                                    ArayTmp[arayCommand.Length + 4] = 0x0A;
                                    ArayTmp[ArayTmp.Length - 1] = Convert.ToByte(arayCommand.Length + 2);
                                }
                                else
                                {
                                    Array.Copy(arayCommand, 0, ArayTmp, 3, wdMaxValue - 2);
                                    ArayTmp[ArayTmp.Length - 3] = 0x0D;
                                    ArayTmp[ArayTmp.Length - 2] = 0x0A;
                                    ArayTmp[ArayTmp.Length - 1] = 33;
                                }
                            }
                        }
                        else if (strType == CsConst.mstrINIDefault.IniReadValue("Public", "99839", ""))
                        {
                            ArayTmp[2] = 1;
                            if (strCommand != null && strCommand != "")
                            {
                                string[] strHex = strCommand.Split(' ');
                                int NullCount = 0;
                                for (int j = 0; j < strHex.Length; j++)
                                {
                                    if (strHex[j] == null || strHex[j] == "") NullCount = NullCount + 1;
                                }

                                int intI = 0;
                                if ((strHex.Length - NullCount) <= 33)
                                {
                                    for (int j = 0; j < strHex.Length; j++)
                                    {
                                        if (strHex[j] != null && strHex[j] != "")
                                        {
                                            ArayTmp[3 + intI] = Convert.ToByte(Convert.ToInt32(strHex[j], 16));
                                            intI++;
                                        }
                                    }
                                    ArayTmp[ArayTmp.Length - 1] = Convert.ToByte(strHex.Length - NullCount);
                                }
                                else
                                {
                                    intI = 0;
                                    for (int j = 0; j < strHex.Length; j++)
                                    {
                                        if (intI < 33)
                                        {
                                            if (strHex[j] != null && strHex[j] != "")
                                            {
                                                ArayTmp[3 + intI] = Convert.ToByte(Convert.ToInt32(strHex[j], 16));
                                                intI++;
                                            }
                                        }
                                    }
                                    ArayTmp[ArayTmp.Length - 1] = 33;
                                }
                            }
                        }
                    }
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, CMD1, SubNetID, DevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(MyintDeviceType)) == true)
                    {
                        if (oRS232 != null)
                        {
                            if (MyintDeviceType == 1041)
                            {
                                if (Type == 1)
                                {
                                    oRS232.myRS2BUS[i].rs232Param.enable = ArayTmp[2];
                                    oRS232.myRS2BUS[i].rs232Param.type = ArayTmp[3];
                                   // Array.Copy(ArayTmp, 4, oRS232.myRS2BUS[i].rs232Param.rsCmd, 0, oRS232.myRS2BUS[i].rs232Param.rsCmd.Length);
                                }
                                else if (Type == 2)
                                {
                                    oRS232.my4852BUS[i].rs232Param.enable = ArayTmp[2];
                                    oRS232.my4852BUS[i].rs232Param.type = ArayTmp[3];
                                   // Array.Copy(ArayTmp, 4, oRS232.myRS2BUS[i].rs232Param.RSCMD, 0, oRS232.myRS2BUS[i].rs232Param.RSCMD.Length);
                                }
                            }
                            else
                            {
                                if (Type == 1)
                                {
                                    oRS232.myRS2BUS[i].rs232Param.enable = ArayTmp[1];
                                    oRS232.myRS2BUS[i].rs232Param.type = ArayTmp[2];
                                   // Array.Copy(ArayTmp, 3, oRS232.myRS2BUS[i].rs232Param.RSCMD, 0, oRS232.myRS2BUS[i].rs232Param.RSCMD.Length);
                                }
                                else if (Type == 2)
                                {
                                    oRS232.my4852BUS[i].rs232Param.enable = ArayTmp[1];
                                    oRS232.my4852BUS[i].rs232Param.type = ArayTmp[2];
                                   // Array.Copy(ArayTmp, 3, oRS232.myRS2BUS[i].TmpRS232.RSCMD, 0, oRS232.myRS2BUS[i].TmpRS232.RSCMD.Length);
                                }
                            }
                        }
                        else if (oMHRCU != null)
                        {
                            if (Type == 1)
                            {
                                oMHRCU.myRS2BUS[i].rs232Param.enable = ArayTmp[1];
                                oMHRCU.myRS2BUS[i].rs232Param.type = ArayTmp[2];
                                //Array.Copy(ArayTmp, 3, oMHRCU.myRS2BUS[i].rs232Param.rsCmd, 0, oMHRCU.myRS2BUS[i].TmpRS232.RSCMD.Length);
                            }
                            else if (Type == 2)
                            {
                                oMHRCU.my4852BUS[i].rs232Param.enable = ArayTmp[1];
                                oMHRCU.my4852BUS[i].rs232Param.type = ArayTmp[2];
                                //Array.Copy(ArayTmp, 3, oMHRCU.my4852BUS[i].TmpRS232.RSCMD, 0, oMHRCU.my4852BUS[i].TmpRS232.RSCMD.Length);
                            }
                        }
                        HDLUDP.TimeBetwnNext(20);
                    }
                    else break;

                    if (MyintDeviceType == 1041)
                    {
                        ArayTmp = new byte[22];
                        if (dgvCommand[1, i].Value == null) dgvCommand[1, i].Value = "";
                        string str = dgvCommand[0, i].Value.ToString();
                        int intID = Convert.ToInt32(str);
                        ArayTmp[0] = Convert.ToByte(intID / 256);
                        ArayTmp[1] = Convert.ToByte(intID % 256);
                        byte[] arayRemark = HDLUDP.StringToByte(dgvCommand[1, i].Value.ToString());
                        if (arayRemark.Length <= 20)
                            arayRemark.CopyTo(ArayTmp, 2);
                        else
                            Array.Copy(arayRemark, 0, ArayTmp, 2, 20);
                    }
                    else
                    {
                        ArayTmp = new byte[21];
                        if (dgvCommand[1, i].Value == null) dgvCommand[1, i].Value = "";
                        string str = dgvCommand[0, i].Value.ToString();
                        ArayTmp[0] = Convert.ToByte(str);
                        byte[] arayRemark = HDLUDP.StringToByte(dgvCommand[1, i].Value.ToString());
                        if (arayRemark.Length <= 20)
                            arayRemark.CopyTo(ArayTmp, 1);
                        else
                            Array.Copy(arayRemark, 0, ArayTmp, 1, 20);
                    }

                    if (CsConst.mySends.AddBufToSndList(ArayTmp, CMD2, SubNetID, DevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(MyintDeviceType)) == true)
                    {
                        if (oRS232 != null)
                        {
                            if (Type == 1)
                            {
                                oRS232.myRS2BUS[i].remark = dgvCommand[1, i].Value.ToString();
                            }
                            else if (Type == 2)
                            {
                                oRS232.my4852BUS[i].remark = dgvCommand[1, i].Value.ToString();
                            }
                        }
                        else if (oMHRCU != null)
                        {
                            if (Type == 1)
                                oMHRCU.myRS2BUS[i].remark = dgvCommand[1, i].Value.ToString();
                            else if (Type == 2)
                                oMHRCU.my4852BUS[i].remark = dgvCommand[1, i].Value.ToString();
                        }
                        HDLUDP.TimeBetwnNext(20);
                    }
                    else break;
                }
            }
            catch
            {
            }
            btnSave.Enabled = true;
            Cursor.Current = Cursors.Default;
        }

        private void txtFrm_Leave(object sender, EventArgs e)
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

        private void txtTo_Leave(object sender, EventArgs e)
        {
            string str = txtTo.Text;
            int num = Convert.ToInt32(txtFrm.Text);
            txtTo.Text = HDLPF.IsNumStringMode(str, num, MaxCount);
            txtTo.SelectionStart = txtTo.Text.Length;
        }

        private void getDataGridViewList()
        {
            try
            {
                dgvCommand.Rows.Clear();
                int wdMaxValue = int.Parse(CsConst.mstrINIDefault.IniReadValue("DeviceType" + MyintDeviceType.ToString(), "MaxValue", "33"));
                if (oMHRCU != null)
                {
                    wdMaxValue = 33;
                }
                int Length = 0;
                if (oRS232 != null)
                {
                    if (Type == 1)
                        Length = oRS232.myRS2BUS.Count;
                    else if (Type == 2)
                        Length = oRS232.my4852BUS.Count;
                }
                else if (oMHRCU != null)
                {
                    if (Type == 1)
                        Length = oMHRCU.myRS2BUS.Count;
                    else if (Type == 2)
                        Length = oMHRCU.my4852BUS.Count;
                }
                for (int i = 0; i < Length; i++)
                {
                    string str1 = "N/A";
                    string str2 = "N/A";
                    string str3 = "N/A";
                    string str4 = "N/A";
                    if (oRS232 != null)
                    {
                        Rs232ToBus temp = null;

                        if (Type == 1)
                        {
                            temp = oRS232.myRS2BUS[i];
                        }
                        else if (Type == 2)
                        {
                            temp = oRS232.my4852BUS[i];
                        }

                        string strEnable = CsConst.WholeTextsList[1775].sDisplayName;
                        if (temp.rs232Param.enable == 1) strEnable = CsConst.mstrINIDefault.IniReadValue("public", "00042", "");
                        string strType = CsConst.WholeTextsList[1775].sDisplayName;
                        string strCMD = "";
                        string strEnd = cbEndChar.Items[0].ToString();
                        if (MyintDeviceType == 1041)
                        {
                            strCMD = "N/A";
                            str1 = CsConst.mstrINIDefault.IniReadValue("keyMode", "00000", "");
                            if (temp.rs232Param.enable == 2)
                                str1 = CsConst.mstrINIDefault.IniReadValue("keyMode", "00002", "");
                            else if (temp.rs232Param.enable == 3)
                                str1 = CsConst.mstrINIDefault.IniReadValue("keyMode", "00003", "");
                            else if (temp.rs232Param.enable == 1)
                                str1 = CsConst.mstrINIDefault.IniReadValue("keyMode", "00001", "");
                            else if (temp.rs232Param.enable == 4)
                                str1 = CsConst.mstrINIDefault.IniReadValue("keyMode", "00004", "");
                            else if (temp.rs232Param.enable == 5)
                                str1 = CsConst.mstrINIDefault.IniReadValue("keyMode", "00005", "");
                            else if (temp.rs232Param.enable == 7)
                                str1 = CsConst.mstrINIDefault.IniReadValue("keyMode", "00007", "");
                            str2 = temp.rs232Param.type.ToString();
                            str3 = temp.rs232Param.rsCmd[0].ToString();
                            //if (oRS232.bytMode == 0)
                            //{
                            //    str4 = CsConst.mstrINIDefault.IniReadValue("public", "99551", "");
                            //    if (temp.TmpRS232.RSCMD[1] == 1)
                            //        str4 = CsConst.mstrINIDefault.IniReadValue("public", "99551", "");
                            //    else if (temp.TmpRS232.RSCMD[1] == 2)
                            //        str4 = CsConst.mstrINIDefault.IniReadValue("public", "99552", "");
                            //}
                            //else if (oRS232.bytMode == 1 || oRS232.bytMode == 2)
                            //{
                            //    str4 = "N/A";
                            //}
                        }
                        else
                        {
                            //if (temp.TmpRS232.bytType == 0)
                            //{
                            //    strType = CsConst.mstrINIDefault.IniReadValue("public", "99838", "");
                            //    int Count = temp.TmpRS232.RSCMD[temp.TmpRS232.RSCMD.Length - 1];
                            //    if (Count > wdMaxValue) Count = wdMaxValue;
                            //    byte[] arayTmp = new byte[Count];
                            //    Array.Copy(temp.TmpRS232.RSCMD, 0, arayTmp, 0, Count);
                            //    if (Count == 0)
                            //        strCMD = "";
                            //    else
                            //        strCMD = HDLPF.Byte2String(arayTmp);
                            //    if (arayTmp.Length > 2 && arayTmp[arayTmp.Length - 1] == 0x0A && arayTmp[arayTmp.Length - 2] == 0x0D) strEnd = cbEndChar.Items[2].ToString();
                            //    else if (arayTmp.Length > 1 && arayTmp[arayTmp.Length - 1] == 0x0D) strEnd = cbEndChar.Items[1].ToString();
                            //}
                            //else if (temp.TmpRS232.bytType == 1)
                            //{
                            //    strType = CsConst.mstrINIDefault.IniReadValue("public", "99839", "");
                            //    int Count = temp.TmpRS232.RSCMD[temp.TmpRS232.RSCMD.Length - 1];
                            //    if (Count > wdMaxValue) Count = wdMaxValue;
                            //    for (int j = 0; j < Count; j++)
                            //    {
                            //        strCMD = strCMD + GlobalClass.AddLeftZero(temp.rs232Param.rsCmd[j].ToString("X"), 2) + " ";
                            //    }
                            //}
                            strCMD = strCMD.Trim();
                        }

                        object[] obj = new object[] { temp.ID.ToString(), temp.remark.ToString(), strEnable, strType, strCMD, strEnd
                                                 ,str1,str2,str3,str4};
                        dgvCommand.Rows.Add(obj);
                    }
                    else if (oMHRCU != null)
                    {
                        Rs232ToBus temp = null;
                        if (Type == 1)
                        {
                            temp = oMHRCU.myRS2BUS[i];
                        }
                        else if (Type == 2)
                        {
                            temp = oMHRCU.my4852BUS[i];
                        }
                        string strEnable = CsConst.WholeTextsList[1775].sDisplayName;
                        if (temp.rs232Param.enable == 1) strEnable = CsConst.mstrINIDefault.IniReadValue("public", "00042", "");
                        string strType = CsConst.WholeTextsList[1775].sDisplayName;
                        string strCMD = "";
                        string strEnd = cbEndChar.Items[0].ToString();
                        if (temp.rs232Param.type == 0)
                        {
                            //strType = CsConst.mstrINIDefault.IniReadValue("public", "99838", "");
                            //int Count = temp.TmpRS232.RSCMD[temp.TmpRS232.RSCMD.Length - 1];
                            //if (Count > 33) Count = 33;
                            //byte[] arayTmp = new byte[Count];
                            //Array.Copy(temp.TmpRS232.RSCMD, 0, arayTmp, 0, Count);
                            //if (Count == 0)
                            //    strCMD = "";
                            //else
                            //    strCMD = HDLPF.Byte2String(arayTmp);
                            //strCMD = HDLPF.Byte2String(arayTmp);

                            //if (arayTmp.Length > 2 && arayTmp[arayTmp.Length - 1] == 0x0A && arayTmp[arayTmp.Length - 2] == 0x0D) strEnd = cbEndChar.Items[2].ToString();
                            //else if (arayTmp.Length > 1 && arayTmp[arayTmp.Length - 1] == 0x0D) strEnd = cbEndChar.Items[1].ToString();

                        }
                        else if (temp.rs232Param.type == 1)
                        {
                            //strType = CsConst.mstrINIDefault.IniReadValue("public", "99839", "");
                            //int Count = temp.TmpRS232.RSCMD[temp.TmpRS232.RSCMD.Length - 1];
                            //if (Count > 33) Count = 33;
                            //for (int j = 0; j < Count; j++)
                            //{
                            //    strCMD = strCMD + GlobalClass.AddLeftZero(temp.rs232Param.rsCmd.RSCMD[j].ToString("X"), 2) + " ";
                            //}
                        }
                        strCMD = strCMD.Trim();
                        object[] obj = new object[] { temp.ID.ToString(), temp.remark.ToString(), strEnable, strType, strCMD, strEnd };
                        dgvCommand.Rows.Add(obj);
                    }
                }
            }
            catch
            {
            }
        }


        private void dgvCommand_Scroll(object sender, ScrollEventArgs e)
        {
            setAllControlVisible(false);
        }
    }
}
