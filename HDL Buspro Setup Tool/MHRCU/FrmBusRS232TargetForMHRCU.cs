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
    public partial class FrmBusRS232TargetForMHRCU : Form
    {
        public int Type;
        private byte SubNetID;
        private byte DevID;
        private int MyintDeviceType;
        private string strRemark = "";
        private MHRCU MyRS232;
        private ComboBox cb232ControlMode = new ComboBox();
        private TextBox txt232Command = new TextBox();
        private ComboBox cbEndChar = new ComboBox();
        private ComboBox cbTime = new ComboBox();
        private int MaxCount = 200;
        private int CommandID = 1;
        public FrmBusRS232TargetForMHRCU()
        {
            InitializeComponent();
        }

        public FrmBusRS232TargetForMHRCU(string strname, MHRCU rs232, int devicetype, int maxcount, int commandid,int type)
        {
            InitializeComponent();
            this.Type = type;
            this.MyRS232 = rs232;
            this.MaxCount = maxcount;
            this.CommandID = commandid;
            string strDevName = strname.Split('\\')[0].ToString();
            strRemark = strname.Split('\\')[1].ToString();
            SubNetID = Convert.ToByte(strDevName.Split('-')[0]);
            DevID = Convert.ToByte(strDevName.Split('-')[1]);
            this.MyintDeviceType = devicetype;
            #region
            cbTime.Items.Clear();
            cbTime.DropDownStyle = ComboBoxStyle.DropDownList;
            cbTime.Items.Add(CsConst.WholeTextsList[1775].sDisplayName);
            cbTime.Items.AddRange(CsConst.strAryRS232Time);
            cbEndChar.Items.Clear();
            cbEndChar.DropDownStyle = ComboBoxStyle.DropDownList;
            cbEndChar.Items.Add("NONE");
            cbEndChar.Items.Add("<CR>");
            cbEndChar.Items.Add("<CR+LF>");
            cb232ControlMode.Items.Clear();
            cb232ControlMode.DropDownStyle = ComboBoxStyle.DropDownList;
            cb232ControlMode.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99838", ""));
            cb232ControlMode.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99839", ""));
            cbTime.SelectedIndexChanged += cbTime_SelectedIndexChanged;
            cb232ControlMode.SelectedIndexChanged += cb232ControlMode_SelectedIndexChanged;
            txt232Command.TextChanged += txt232Command_TextChanged;
            
            cbEndChar.SelectedIndexChanged += cbEndChar_SelectedIndexChanged;
            dgvCommand.Controls.Add(cb232ControlMode);
            dgvCommand.Controls.Add(txt232Command);
            dgvCommand.Controls.Add(cbEndChar);
            dgvCommand.Controls.Add(cbTime);
            #endregion
            setAllControlVisible(false);
        }

        void cbTime_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dgvCommand.CurrentRow.Index < 0) return;
            if (dgvCommand.RowCount <= 0) return;
            int index = dgvCommand.CurrentRow.Index;
            dgvCommand[1, index].Value = cbTime.Text;
            ModifyMultilinesIfNeeds(dgvCommand[1, index].Value.ToString(), 1, dgvCommand);
        }

        void cbEndChar_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dgvCommand.CurrentRow.Index < 0) return;
            if (dgvCommand.RowCount <= 0) return;
            int index = dgvCommand.CurrentRow.Index;
            dgvCommand[4, index].Value = cbEndChar.Text;
            ModifyMultilinesIfNeeds(dgvCommand[4, index].Value.ToString(), 4, dgvCommand);
        }


        void txt232Command_TextChanged(object sender, EventArgs e)
        {
            if (dgvCommand.CurrentRow.Index < 0) return;
            if (dgvCommand.RowCount <= 0) return;
            int index = dgvCommand.CurrentRow.Index;
            if (dgvCommand[2, index].Value.ToString() == CsConst.mstrINIDefault.IniReadValue("Public", "99838", ""))
            {
                dgvCommand[3, index].Value = txt232Command.Text;
                ModifyMultilinesIfNeeds(dgvCommand[3, index].Value.ToString(), 3, dgvCommand);
            }
            else if (dgvCommand[2, index].Value.ToString() == CsConst.mstrINIDefault.IniReadValue("Public", "99839", ""))
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
                dgvCommand[3, index].Value = str;
                ModifyMultilinesIfNeeds(dgvCommand[3, index].Value.ToString(), 3, dgvCommand);
            }
        }

        void cb232ControlMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbEndChar.Visible = false;
            if (dgvCommand.CurrentRow.Index < 0) return;
            if (dgvCommand.RowCount <= 0) return;
            int index = dgvCommand.CurrentRow.Index;
            dgvCommand[2, index].Value = cb232ControlMode.Text;
            ModifyMultilinesIfNeeds(dgvCommand[2, index].Value.ToString(), 2, dgvCommand);
            if (cb232ControlMode.SelectedIndex == 0)
            {
                cbEndChar.SelectedIndex = cbEndChar.Items.IndexOf(dgvCommand[4, index].Value.ToString());
                addcontrols(4, index, cbEndChar, dgvCommand);
                if (cbEndChar.SelectedIndex < 0) cbEndChar.SelectedIndex = 0;
                cbEndChar_SelectedIndexChanged(null, null);
            }
            else if (cb232ControlMode.SelectedIndex == 1)
            {
                txt232Command.Text = dgvCommand[3, index].Value.ToString();
                txt232Command_TextChanged(null, null);
                dgvCommand[4, index].Value = cbEndChar.Items[0].ToString();
            }
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
                    cbTime.SelectedIndex = cbTime.Items.IndexOf(dgvCommand[1, e.RowIndex].Value.ToString());
                    addcontrols(1, e.RowIndex, cbTime, dgvCommand);

                    cb232ControlMode.SelectedIndex = cb232ControlMode.Items.IndexOf(dgvCommand[2, e.RowIndex].Value.ToString());
                    addcontrols(2, e.RowIndex, cb232ControlMode, dgvCommand);

                    txt232Command.Text = dgvCommand[3, e.RowIndex].Value.ToString();
                    addcontrols(3, e.RowIndex, txt232Command, dgvCommand);
                }
                if (cb232ControlMode.SelectedIndex < 0) cb232ControlMode.SelectedIndex = 0;
                if(cbTime.SelectedIndex<0)cbTime.SelectedIndex=0;
                if (cb232ControlMode.Visible) cb232ControlMode_SelectedIndexChanged(null, null);
                if(cbTime.Visible)cbTime_SelectedIndexChanged(null,null);
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
            txt232Command.Visible = TF;
            cbEndChar.Visible = TF;
            cbTime.Visible = TF;
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
            lbSubValue.Text = SubNetID.ToString();
            lbDevValue.Text = DevID.ToString();
            lbRemarkValue.Text = strRemark;
            lbTarget.Text = lbTarget.Text.Split('-')[0].ToString() + "-" + MaxCount.ToString() + ")";
            btnSure_Click(null, null);
        }

        private void btnSure_Click(object sender, EventArgs e)
        {
            try
            {
                int wdMaxValue = 33;
                setAllControlVisible(false);
                Cursor.Current = Cursors.WaitCursor;
                dgvCommand.Rows.Clear();
                btnSure.Enabled = false;
                byte bytFrm = Convert.ToByte(Convert.ToInt32(txtFrm.Text));
                byte bytTo = Convert.ToByte(txtTo.Text);
                int CMD = 0xE424;
                if (Type == 1)
                {
                    CMD = 0xE424;
                }
                else if (Type == 2)
                {
                    CMD = 0xDA65;
                }
                for (byte byt = bytFrm; byt <= bytTo; byt++)
                {
                    byte[] ArayTmp = new byte[2];
                    ArayTmp[0] = Convert.ToByte(CommandID);
                    ArayTmp[1] = Convert.ToByte(byt);

                    if (CsConst.mySends.AddBufToSndList(ArayTmp, CMD, SubNetID, DevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(MyintDeviceType)) == true)
                    {
                        string strTime = CsConst.WholeTextsList[1775].sDisplayName;
                        if (1 <= CsConst.myRevBuf[28] && CsConst.myRevBuf[28] <= 8) strTime = CsConst.strAryRS232Time[CsConst.myRevBuf[28] - 1];
                        string strType = CsConst.WholeTextsList[1775].sDisplayName;
                        string strCMD = "";
                        string strEnd = cbEndChar.Items[0].ToString();
                        byte[] arayCMD = new byte[wdMaxValue + 1];
                        Array.Copy(CsConst.myRevBuf, 30, arayCMD, 0, arayCMD.Length);
                        if (CsConst.myRevBuf[29] == 0)
                        {
                            strType = CsConst.mstrINIDefault.IniReadValue("public", "99838", "");
                            int Count = arayCMD[arayCMD.Length - 1];
                            byte[] arayTmp = new byte[Count];
                            Array.Copy(arayCMD, 0, arayTmp, 0, Count);
                            strCMD = HDLPF.Byte2String(arayTmp);
                            if (arayTmp.Length > 2 && arayTmp[arayTmp.Length - 1] == 0x0A && arayTmp[arayTmp.Length - 2] == 0x0D) strEnd = cbEndChar.Items[2].ToString();
                            else if (arayTmp.Length > 1 && arayTmp[arayTmp.Length - 1] == 0x0D) strEnd = cbEndChar.Items[1].ToString();


                        }
                        else if (CsConst.myRevBuf[29] == 1)
                        {
                            strType = CsConst.mstrINIDefault.IniReadValue("public", "99839", "");
                            int Count = arayCMD[arayCMD.Length - 1];
                            for (int j = 0; j < Count; j++)
                            {
                                strCMD = strCMD + GlobalClass.AddLeftZero(arayCMD[j].ToString("X"), 2) + " ";
                            }
                        }
                        object[] obj = new object[] { byt.ToString(), strTime, strType, strCMD, strEnd };
                        dgvCommand.Rows.Add(obj);
                        
                        HDLUDP.TimeBetwnNext(1);
                    }
                    else break;
                }
            }
            catch
            {
            }
            btnSure.Enabled = true;
            Cursor.Current = Cursors.Default;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                int wdMaxValue = 33;
                btnSave.Enabled = false;
                Cursor.Current = Cursors.WaitCursor;
                int CMD = 0xE426;
                if (Type == 1)
                {
                    CMD = 0xE426;
                }
                else if (Type == 2)
                {
                    CMD = 0xDA67;
                }
                for (int i = 0; i < dgvCommand.Rows.Count; i++)
                {
                    byte[] ArayTmp = new byte[wdMaxValue + 5];
                    ArayTmp[0] = Convert.ToByte(CommandID);
                    ArayTmp[1] = Convert.ToByte(dgvCommand[0, i].Value.ToString());
                    string strTime = dgvCommand[1, i].Value.ToString();
                    string strType = dgvCommand[2, i].Value.ToString();
                    string strCommand = dgvCommand[3, i].Value.ToString();
                    string strEnd = dgvCommand[4, i].Value.ToString();
                    ArayTmp[2] = Convert.ToByte(cbTime.Items.IndexOf(strTime));
                    if (strType == CsConst.mstrINIDefault.IniReadValue("Public", "99838", ""))
                    {
                        ArayTmp[3] = 0;
                        byte[] arayCommand = HDLUDP.StringToByte(strCommand);
                        if (strEnd == "NONE")
                        {
                            if (arayCommand.Length <= wdMaxValue)
                            {
                                Array.Copy(arayCommand, 0, ArayTmp, 4, arayCommand.Length);
                                ArayTmp[ArayTmp.Length - 1] = Convert.ToByte(arayCommand.Length);
                            }
                            else
                            {
                                Array.Copy(arayCommand, 0, ArayTmp, 4, wdMaxValue);
                                ArayTmp[ArayTmp.Length - 1] = Convert.ToByte(wdMaxValue);
                            }
                        }
                        else if (strEnd == "<CR>")
                        {
                            if (arayCommand.Length <= (wdMaxValue - 1))
                            {
                                Array.Copy(arayCommand, 0, ArayTmp, 4, arayCommand.Length);
                                ArayTmp[arayCommand.Length + 4] = 0X0D;
                                ArayTmp[ArayTmp.Length - 1] = Convert.ToByte(arayCommand.Length + 1);
                            }
                            else
                            {
                                Array.Copy(arayCommand, 0, ArayTmp, 4, wdMaxValue - 1);
                                ArayTmp[ArayTmp.Length - 2] = 0x0D;
                                ArayTmp[ArayTmp.Length - 1] = Convert.ToByte(wdMaxValue);
                            }
                        }
                        else if (strEnd == "<CR+LF>")
                        {
                            if (arayCommand.Length <= (wdMaxValue - 2))
                            {
                                Array.Copy(arayCommand, 0, ArayTmp, 4, arayCommand.Length);
                                ArayTmp[arayCommand.Length + 4] = 0X0D;
                                ArayTmp[arayCommand.Length + 5] = 0x0A;
                                ArayTmp[ArayTmp.Length - 1] = Convert.ToByte(arayCommand.Length + 2);
                            }
                            else
                            {
                                Array.Copy(arayCommand, 0, ArayTmp, 4, wdMaxValue - 2);
                                ArayTmp[ArayTmp.Length - 3] = 0x0D;
                                ArayTmp[ArayTmp.Length - 2] = 0x0A;
                                ArayTmp[ArayTmp.Length - 1] = Convert.ToByte(wdMaxValue);
                            }
                        }
                    }
                    else if (strType == CsConst.mstrINIDefault.IniReadValue("Public", "99839", ""))
                    {
                        ArayTmp[3] = 1;
                        if (strCommand != null && strCommand != "")
                        {
                            string[] strHex = strCommand.Split(' ');
                            int NullCount = 0;
                            for (int j = 0; j < strHex.Length; j++)
                            {
                                if (strHex[j] == null || strHex[j] == "") NullCount = NullCount + 1;
                            }

                            int intI = 0;
                            if ((strHex.Length - NullCount) <= wdMaxValue)
                            {
                                for (int j = 0; j < strHex.Length; j++)
                                {
                                    if (strHex[j] != null && strHex[j] != "")
                                    {
                                        ArayTmp[4 + intI] = Convert.ToByte(Convert.ToInt32(strHex[j], 16));
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
                                    if (intI < wdMaxValue)
                                    {
                                        if (strHex[j] != null && strHex[j] != "")
                                        {
                                            ArayTmp[4 + intI] = Convert.ToByte(Convert.ToInt32(strHex[j], 16));
                                            intI++;
                                        }
                                    }
                                }
                                ArayTmp[ArayTmp.Length - 1] = Convert.ToByte(wdMaxValue);
                            }
                        }
                    }

                    if (CsConst.mySends.AddBufToSndList(ArayTmp, CMD, SubNetID, DevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(MyintDeviceType)) == true)
                    {
                        
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
            txtFrm.Text = HDLPF.IsNumStringMode(str, 1, num);
            txtFrm.SelectionStart = txtFrm.Text.Length;
        }

        private void txtTo_Leave(object sender, EventArgs e)
        {
            string str = txtTo.Text;
            int num = Convert.ToInt32(txtFrm.Text);
            txtTo.Text = HDLPF.IsNumStringMode(str, num, MaxCount);
            txtTo.SelectionStart = txtTo.Text.Length;
        }

    }
}
