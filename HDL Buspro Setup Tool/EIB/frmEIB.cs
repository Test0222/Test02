using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace HDL_Buspro_Setup_Tool
{
    public partial class frmEIB : Form
    {
        private EIBConverter myEIB = null;
        private string myDevName = null;
        private int mintIDIndex = -1;
        private int SelectRowIndex = 0;
        private int MyintDeviceType = -1;
        private TextBox txtbox1 = new TextBox();
        private TextBox txtbox2 = new TextBox();
        private ComboBox cbType = new ComboBox();
        private TimeText timetxt = new TimeText(":");
        private bool isSencond = false;
        private bool isselect = true;

        public frmEIB()
        {
            InitializeComponent();
        }

        public frmEIB(EIBConverter myrs232, string strName, int intDIndex, int wdDevivceType)
        {
            InitializeComponent();
            this.myEIB = myrs232;
            this.myDevName = strName;
            this.mintIDIndex = intDIndex;
            this.Text = strName;
            this.MyintDeviceType = wdDevivceType;
            tsl3.Text = strName;

            string strDevName = strName.Split('\\')[0].ToString();

            HDLSysPF.DisplayDeviceNameModeDescription(strName, MyintDeviceType, cboDevice, tbModel, tbDescription);

            tbAddress.Text = myEIB.Address;

            cbType.Items.Clear();
            cbType.SelectedIndexChanged += new EventHandler(cbType_SelectedIndexChanged);
            txtbox1.TextChanged += new EventHandler(txtbox1_TextChanged);
            txtbox2.TextChanged += new EventHandler(txtbox2_TextChanged);
            txtbox1.KeyPress += new KeyPressEventHandler(txtbox1_KeyPress);
            txtbox2.KeyPress += new KeyPressEventHandler(txtbox1_KeyPress);
            timetxt.TextChanged += new EventHandler(timetxt_TextChanged);
            cbType.Items.AddRange(CsConst.EIBConverTors);
            clType.Items.AddRange(CsConst.EIBConverTors);
            cbType.DropDownStyle = ComboBoxStyle.DropDownList;
            DgvList.Controls.Add(cbType);
            DgvList.Controls.Add(txtbox1);
            DgvList.Controls.Add(txtbox2);
            DgvList.Controls.Add(timetxt);
            allvisible(false);
        }

        private void toolStripButton11_Click(object sender, EventArgs e)
        {
            // if (oDMX == null) return;

            // frmProperty frm = new frmProperty(myStrName, mintDeviceType);
            // frm.ShowDialog();
        }

        private void frmEIB_Load(object sender, EventArgs e)
        {
           

        }

        private void ShowConverterListsToForm()
        {
            if (myEIB.otherInfo == null) return;
            DgvList.Rows.Clear();
            byte bytID = 1;
            foreach (EIBConverter.OtherInfo temp in myEIB.otherInfo)
            {
                int num = Convert.ToInt32(temp.Param3) * 256 + Convert.ToInt32(temp.Param4);
                System.Diagnostics.Debug.WriteLine(HDLPF.GetStringFromTime(num, ":"));
            }
            foreach (EIBConverter.OtherInfo oTmp in myEIB.otherInfo)
            {
                int num = Convert.ToInt32(oTmp.Param3) * 256 + Convert.ToInt32(oTmp.Param4);
                object[] obj = { bytID, oTmp.GroupAddress,cbType.Items[oTmp.ControlType].ToString(),"",oTmp.strDevName.Split('/')[0].ToString(),
                                 oTmp.strDevName.Split('/')[1].ToString(), oTmp.Param1,oTmp.Param2 ,HDLPF.GetStringFromTime(num, ":"),clFlag.Items[oTmp.Type].ToString()};
                DgvList.Rows.Add(obj);
                bytID++;
            }
        }

        private void tsbDown_Click(object sender, EventArgs e)
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

                byte[] ArayRelay = new byte[] { bytSubID, bytDevID, (byte)(MyintDeviceType / 256), (byte)(MyintDeviceType % 256)
                    , (byte)0 ,(byte)(mintIDIndex / 256), (byte)(mintIDIndex % 256),};
                CsConst.MyUPload2DownLists.Add(ArayRelay);
                CsConst.MyUpload2Down = Convert.ToByte(((ToolStripButton)sender).Tag.ToString());
                FrmDownloadShow Frm = new FrmDownloadShow();
                Frm.FormClosing += new FormClosingEventHandler(UpdateDisplayInformationAccordingly);
                Frm.ShowDialog();
            }
        }

        void UpdateDisplayInformationAccordingly(object sender, FormClosingEventArgs e)
        {
            ShowConverterListsToForm();
            Cursor.Current = Cursors.Default;
        }

        void SetVisableForDownOrUpload(bool blnIsEnable)
        {
            tsbDown.Enabled = blnIsEnable;
            toolStripLabel2.Enabled = blnIsEnable;

            if (blnIsEnable == true && tsbar.Value != 0)
            {
                tsbHint.Visible = true;
                tsbHint.Text = "Fully Success!";
            }
            else
            {
                tsbar.Value = 0;
                tsbHint.Visible = false;
            }
            tsbar.Visible = !blnIsEnable;
            tsbl4.Visible = !blnIsEnable;
        }

        private void toolStripLabel2_Click(object sender, EventArgs e)
        {

        }

        private void tbAddress_TextChanged(object sender, EventArgs e)
        {
            if (tbAddress.Text == null || tbAddress.Text == "") return;

            string strTmp = tbAddress.Text;
            if (!strTmp.Contains('/')) return;

            myEIB.Address = strTmp;
        }

        private void btnAutoAdd_Click(object sender, EventArgs e)
        {
            if (DgvList.RowCount >= 254) return;
            if (txtNum.Text == null || txtNum.Text == "") return;
            int Num = Convert.ToInt32(txtNum.Text);
            if (Num > 254) Num = 254;
            if (myEIB.otherInfo == null) myEIB.otherInfo = new List<EIBConverter.OtherInfo>();
            if (Num > 0)
            {
                for (int i = 0; i < Num; i++)
                {
                    EIBConverter.OtherInfo oTmp = new EIBConverter.OtherInfo();
                    oTmp.GroupAddress = "0/0/0";
                    oTmp.ControlType = 0;
                    oTmp.strDevName = "0/0";
                    oTmp.Param1 = 1;
                    oTmp.Param2 = 1;
                    oTmp.Param3 = 1;
                    oTmp.Param4 = 1;
                    oTmp.Type = 0;
                    myEIB.otherInfo.Add(oTmp);
                    object[] obj = new object[] {DgvList.RowCount + 1, oTmp.GroupAddress,cbType.Items[0].ToString(),"",oTmp.strDevName.Split('/')[0].ToString(),
                                 oTmp.strDevName.Split('/')[1].ToString(), oTmp.Param1,oTmp.Param2 ,oTmp.Param3,clFlag.Items[0].ToString() };
                    DgvList.Rows.Add(obj);
                }
            }
            myEIB.MyRead2UpFlags[1] = false;
        }

        private void tsbDefault_Click(object sender, EventArgs e)
        {
            if (myEIB == null) return;
            myEIB.ReadDefaultInfo();
            ShowConverterListsToForm();
        }

        private void tsbSave_Click(object sender, EventArgs e)
        {
            if (myEIB == null) return;
            Cursor.Current = Cursors.WaitCursor;
            myEIB.SaveDevieceInfoToDB(mintIDIndex);
            foreach (EIBConverter.OtherInfo temp in myEIB.otherInfo)
            {
                int num = Convert.ToInt32(temp.Param3) * 256 + Convert.ToInt32(temp.Param4);
                System.Diagnostics.Debug.WriteLine(HDLPF.GetStringFromTime(num, ":"));
            }
            Cursor.Current = Cursors.Default;
        }

        private void DgvList_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            //更新缓存 函数补全
            if (DgvList.SelectedRows == null || DgvList.SelectedRows.Count == 0) return;
            if (DgvList.RowCount > 0)
            {
                int intLastSelect = SelectRowIndex;
                for (int i = 0; i < DgvList.SelectedRows.Count; i++)
                {
                    DgvList.SelectedRows[i].Cells[e.ColumnIndex].Value = DgvList[e.ColumnIndex, e.RowIndex].Value.ToString();

                    EIBConverter.OtherInfo temp = myEIB.otherInfo[DgvList.SelectedRows[i].Index];
                    temp.ControlType = Convert.ToByte(cbType.Items.IndexOf(DgvList[2, DgvList.SelectedRows[i].Index].Value.ToString()));
                    string str1 = DgvList[6, DgvList.SelectedRows[i].Index].Value.ToString();
                    string str2 = DgvList[7, DgvList.SelectedRows[i].Index].Value.ToString();
                    string str3 = DgvList[8, DgvList.SelectedRows[i].Index].Value.ToString();
                    if (str1.Contains("("))
                        str1 = str1.Split('(')[0].ToString();
                    else
                        str1 = IsNumeric(str1).ToString();
                    if (str2.Contains("("))
                        str2 = str2.Split('(')[0].ToString();
                    else
                        str2 = IsNumeric(str2).ToString();

                    if (temp.ControlType == 0 || temp.ControlType == 2 || temp.ControlType == 3 || temp.ControlType == 9 || temp.ControlType == 10
                     || temp.ControlType == 11 || temp.ControlType == 13)
                    {
                        temp.Param1 = Convert.ToByte(int.Parse(str1));
                    }
                    if (temp.ControlType == 1)
                    {
                        temp.Param1 = Convert.ToByte(int.Parse(str1));
                        temp.Param2 = Convert.ToByte(int.Parse(str2));
                    }
                    if (temp.ControlType == 4 || temp.ControlType == 5)
                    {
                        temp.Param1 = Convert.ToByte(int.Parse(str1));
                        temp.Param2 = Convert.ToByte(int.Parse(str2));
                        if (!str3.Contains(":"))
                        {
                            str3 = "0:0";
                        }
                        string time = HDLPF.GetTimeFromString(str3, ':');
                        temp.Param3 = Convert.ToByte(int.Parse(time) / 256);
                        temp.Param4 = Convert.ToByte(int.Parse(time) % 256);
                    }
                    if (temp.ControlType == 6)
                    {
                        temp.Param1 = 255;
                    }
                    if (temp.ControlType == 7)
                    {
                        temp.Param1 = Convert.ToByte(int.Parse(str1));
                        temp.Param2 = Convert.ToByte(int.Parse(str2));
                        if (!str3.Contains(":"))
                        {
                            str3 = "0:0";
                        }
                        string time = HDLPF.GetTimeFromString(str3, ':');
                        temp.Param3 = Convert.ToByte(int.Parse(time) / 256);
                        temp.Param4 = Convert.ToByte(int.Parse(time) % 256);
                    }
                    if (temp.ControlType == 8)
                    {
                        temp.Param1 = 255;
                        temp.Param2 = Convert.ToByte(int.Parse(str2));
                        if (!str3.Contains(":"))
                        {
                            str3 = "0:0";
                        }
                        string time = HDLPF.GetTimeFromString(str3, ':');
                        temp.Param3 = Convert.ToByte(int.Parse(time) / 256);
                        temp.Param4 = Convert.ToByte(int.Parse(time) % 256);
                    }
                }
            }
            myEIB.MyRead2UpFlags[1] = false;
        }

        private void addtxtbox(int num, TextBox txtbox)
        {
            txtbox.Visible = true;
            Rectangle rect2 = DgvList.GetCellDisplayRectangle(num, SelectRowIndex, true);
            txtbox.Size = rect2.Size;
            txtbox.Top = rect2.Top;
            txtbox.Left = rect2.Left;
            string str = DgvList[num, SelectRowIndex].Value.ToString();
            if (str.Contains("("))
                txtbox.Text = str.Split('(')[0].ToString();
            else
                txtbox.Text = IsNumeric(str).ToString();
            txtbox1_TextChanged(null, null);
            if (isSencond)
                txtbox2_TextChanged(null, null);
            isSencond = false;
        }

        private void DgvList_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            //if (DgvList[e.ColumnIndex, e.RowIndex].Value.ToString() == CsConst.mstrInvalid) return;
            if (DgvList.RowCount == 0) return;
            if (e.Button == MouseButtons.Left)
            {
                if (e.RowIndex >= 0)
                {
                    SelectRowIndex = e.RowIndex;
                    addTypebox(2, cbType);
                }
            }
        }

        public static int IsNumeric(string str)
        {
            int i;
            if (str != null && Regex.IsMatch(str, @"^\d+$"))
                i = int.Parse(str);
            else
                i = 1;
            return i;
        }

        private void cbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            allvisible(false);
            cbType.Visible = true;
            DgvList[2, SelectRowIndex].Value = cbType.Text;
            if (((ComboBox)sender).Text == "Scene(1 byte)" || ((ComboBox)sender).Text == "Sequence(1 byte)" || ((ComboBox)sender).Text == "Universal switch(1 bit)" ||
                ((ComboBox)sender).Text == "Curtain switch on(1 bit)" || ((ComboBox)sender).Text == "Curtain switch off(1 bit)" || ((ComboBox)sender).Text == "Message(1 byte)" ||
                ((ComboBox)sender).Text == "Absolute Dimming(1 byte)")
            {
                addtxtbox(6, txtbox1);
                DgvList[7, SelectRowIndex].Value = "N/A";
                DgvList[8, SelectRowIndex].Value = "N/A";
            }
            else if (((ComboBox)sender).Text == "Scene Dimmer(4 bit)")
            {
                addtxtbox(6, txtbox1);
                isSencond = true;
                addtxtbox(7, txtbox2);
                DgvList[8, SelectRowIndex].Value = "N/A";
            }
            else if (((ComboBox)sender).Text == "Single channel switch(1 bit)" || ((ComboBox)sender).Text == "Single channel Dimmer(4 bit)")
            {
                addtxtbox(6, txtbox1);
                isSencond = true;
                addtxtbox(7, txtbox2);
                addtimetxt(8);
            }
            else if (((ComboBox)sender).Text == "Broadcast scene(1 byte)")
            {
                DgvList[6, SelectRowIndex].Value = "All Areas";
                DgvList[7, SelectRowIndex].Value = "N/A";
                DgvList[8, SelectRowIndex].Value = "N/A";
            }
            else if (((ComboBox)sender).Text == "Broadcast Channels Switch(1 bit)")
            {
                addtxtbox(6, txtbox1);
                isSencond = true;
                addtxtbox(7, txtbox2);
                addtimetxt(8);
            }
            else if (((ComboBox)sender).Text == "Broadcast Channels Dimmer(4 bit)")
            {
                DgvList[6, SelectRowIndex].Value = "All Areas";
                isSencond = true;
                addtxtbox(7, txtbox2);
                addtimetxt(8);
            }
            else if (((ComboBox)sender).Text == "String Conversion(14 byte)" || ((ComboBox)sender).Text == "Current Temperature(2 byte)")
            {
                DgvList[6, SelectRowIndex].Value = "N/A";
                DgvList[7, SelectRowIndex].Value = "N/A";
                DgvList[8, SelectRowIndex].Value = "N/A";
            }
        }

        private void addTypebox(int num, ComboBox cb)
        {
            Rectangle rect = DgvList.GetCellDisplayRectangle(num, SelectRowIndex, true);
            cb.Visible = true;
            cb.Size = rect.Size;
            cb.Top = rect.Top;
            cb.Left = rect.Left;
            cb.Text = DgvList[num, SelectRowIndex].Value.ToString();
        }

        private void addtimetxt(int num)
        {
            timetxt.Visible = true;
            Rectangle rect2 = DgvList.GetCellDisplayRectangle(num, SelectRowIndex, true);
            timetxt.Size = rect2.Size;
            timetxt.Top = rect2.Top;
            timetxt.Left = rect2.Left;
            string str = DgvList[num, SelectRowIndex].Value.ToString();
            if (str.Contains(":"))
                timetxt.Text = HDLPF.GetTimeFromString(str, ':');
            else
                timetxt.Text = "0:0";
            timetxt_TextChanged(null, null);
        }

        private void allvisible(bool TF)
        {
            cbType.Visible = TF;
            txtbox1.Visible = TF;
            txtbox2.Visible = TF;
            timetxt.Visible = TF;
        }

        private void txtbox1_TextChanged(object sender, EventArgs e)
        {
            string str = DgvList[2, SelectRowIndex].Value.ToString();
            if (txtbox1.Text == "")
                return;
            txtbox1.Text = IsNumeric(txtbox1.Text).ToString();
            if (str == "Scene(1 byte)" || str == "Scene Dimmer(4 bit)")
                settxtvalue(txtbox1, 6, 48, 1, "Area No.");
            if (str == "Sequence(1 byte)")
                settxtvalue(txtbox1, 6, 255, 1, "Area No.");
            if (str == "Universal switch(1 bit)")
                settxtvalue(txtbox1, 6, 255, 1, "Switch No.");
            if (str == "Single channel switch(1 bit)" || str == "Single channel Dimmer(4 bit)")
                settxtvalue(txtbox1, 6, 240, 1, "Chn No.");
            if (str == "Curtain switch off(1 bit)" || str == "Curtain switch on(1 bit)")
                settxtvalue(txtbox1, 6, 255, 1, "Curtain No.");
            if (str == "Message(1 byte)")
                settxtvalue(txtbox1, 6, 20, 1, "Seq No.");
            if (str == "Absolute Dimming(1 byte)")
                settxtvalue(txtbox1, 6, 240, 1, "");
            if (str == "Single channel switch(1 bit)")
                settxtvalue(txtbox1, 6, 255, 1, "Chn No.");
        }

        private void txtbox2_TextChanged(object sender, EventArgs e)
        {
            string str = DgvList[2, SelectRowIndex].Value.ToString();
            if (txtbox2.Text == "")
                return;
            txtbox2.Text = IsNumeric(txtbox2.Text).ToString();
            if (str == "Scene Dimmer(4 bit)")
                settxtvalue(txtbox2, 7, 99, 0, "Sce No.");
            if (str == "Single channel switch(1 bit)" || str == "Single channel Dimmer(4 bit)" || str == "Broadcast Channels Switch(1 bit)" ||
                str == "Broadcast Channels Dimmer(4 bit)")
            {
                settxtvalue(txtbox2, 7, 100, 0, "Level");
            }
        }

        private void timetxt_TextChanged(object sender, EventArgs e)
        {
            string str = DgvList[2, SelectRowIndex].Value.ToString();
            if (str == "Single channel switch(1 bit)" || str == "Single channel Dimmer(4 bit)" || str == "Broadcast Channels Switch(1 bit)" ||
                str == "Broadcast Channels Dimmer(4 bit)")
            {
                DgvList[8, SelectRowIndex].Value = HDLPF.GetStringFromTime(int.Parse(timetxt.Text.ToString()), ":");
            }
        }

        private void settxtvalue(TextBox txtbox, int num, int num1, int num2, string str1)
        {
            if (int.Parse(txtbox.Text) > num1 || int.Parse(txtbox.Text) < num2)
                txtbox.Text = "1";
            if (str1 != "")
                DgvList[num, SelectRowIndex].Value = txtbox.Text + "(" + str1 + ")";
            else
                DgvList[num, SelectRowIndex].Value = txtbox.Text;
        }

        private void DgvList_SelectionChanged(object sender, EventArgs e)
        {
            if (isselect)
            {
                if (DgvList.CurrentRow.Index == SelectRowIndex)
                {
                    addTypebox(2, cbType);
                    cbType_SelectedIndexChanged((object)cbType, null);
                }
            }
        }

        private void txtbox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            string str = "abcdefghijklmnopqrstuwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            if (str.Contains(e.KeyChar.ToString()))
                e.Handled = true;
        }

        private void frmEIB_Shown(object sender, EventArgs e)
        {
            if (CsConst.MyEditMode == 0)
            {
                 ShowConverterListsToForm();
            }
            else if (CsConst.MyEditMode == 1) //在线模式
            {
                if (myEIB.MyRead2UpFlags[0] == false)
                {
                    tsbDown_Click(tsbDown, null);
                }
                else
                {
                    ShowConverterListsToForm();
                }
            }
        }

        private void DgvList_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            DgvList.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void frmEIB_FormClosing(object sender, FormClosingEventArgs e)
        {

        }
    }
}
