using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Data.OleDb;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace HDL_Buspro_Setup_Tool
{
    public partial class frmMC : Form
    {
        [DllImport("user32.dll")]
        public static extern Boolean FlashWindow(IntPtr hwnd, Boolean bInvert);
        private byte SubNetID;
        private byte DevID;
        private int mintIDIndex = -1;
        private int mintDeviceType = -1;
        private string mystrName;
        private SendIR tempSend = new SendIR();  //用于存储公共的红外码
        private MMC mmc = null;
        public List<MMC.RS232Param> myTmpCMDLists = null;
        private int MyActivePage = 0; //按页面上传下载
        NetworkInForm networkinfo;
        private bool isReading = false;

        private TextBox txtSub = new TextBox();
        private TextBox txtDev = new TextBox();
        private ComboBox cbControlType = new ComboBox();
        private ComboBox cbbox1 = new ComboBox();
        private ComboBox cbbox2 = new ComboBox();
        private ComboBox cbbox3 = new ComboBox();
        private TextBox txtbox1 = new TextBox();
        private TextBox txtbox2 = new TextBox();
        private TextBox txtbox3 = new TextBox();
        private TimeText txtSeries = new TimeText(":");
        private ComboBox cbPanleControl = new ComboBox();
        private ComboBox cbAudioControl = new ComboBox();

        private ComboBox cb232RSNO = new ComboBox();
        private ComboBox cb232ControlMode = new ComboBox();
        private TextBox txt232Command = new TextBox();

        private TextBox txtLoad = new TextBox();
        private ComboBox cbLoad = new ComboBox();

        private ComboBox cbSeqType = new ComboBox();
        private ComboBox cbParam1 = new ComboBox();
        private TextBox txtParam1 = new TextBox();
        private ComboBox cbParam2 = new ComboBox();
        private ComboBox cbParam3 = new ComboBox();
        private ComboBox cbParam4 = new ComboBox();
        private TimeText txtTime = new TimeText(":");

        public frmMC()
        {
            InitializeComponent();
        }

        public frmMC(MMC oTmp, string strName, int intDIndex, int intDeviceType)
        {
            InitializeComponent();

            this.mmc = oTmp;
            this.mystrName = strName;
            this.mintDeviceType = intDeviceType;
            this.mintIDIndex = intDIndex;

            tbRemark.Text = strName.Split('\\')[1].ToString();
            string strDevName = strName.Split('\\')[0].ToString();
            NumSubID.Value = Convert.ToByte(strDevName.Split('-')[0]);
            NumDevID.Value = Convert.ToByte(strDevName.Split('-')[1]);
            SubNetID = Convert.ToByte(strDevName.Split('-')[0]);
            DevID = Convert.ToByte(strDevName.Split('-')[1]);
            this.Text = mystrName;
            tsl3.Text = mystrName;
            #region
            cbControlType.Items.Clear();
            HDLSysPF.AddControlTypeToControl(cbControlType, mintDeviceType);
            cbPanleControl.Items.Clear();
            HDLSysPF.getPanlControlType(cbPanleControl, mintDeviceType);
            cbAudioControl.Items.Clear();
            for (int i = 1; i <= 8; i++)
                cbAudioControl.Items.Add(CsConst.mstrINIDefault.IniReadValue("public", "0009" + i.ToString(), ""));

            cbControlType.DropDownStyle = ComboBoxStyle.DropDownList;
            cbPanleControl.DropDownStyle = ComboBoxStyle.DropDownList;
            cbAudioControl.DropDownStyle = ComboBoxStyle.DropDownList;
            cbbox1.DropDownStyle = ComboBoxStyle.DropDownList;
            cbbox2.DropDownStyle = ComboBoxStyle.DropDownList;
            cbbox3.DropDownStyle = ComboBoxStyle.DropDownList;

            txtSub.KeyPress += txtFrm_KeyPress;
            txtDev.KeyPress += txtFrm_KeyPress;
            txtbox1.KeyPress += txtFrm_KeyPress;
            txtbox2.KeyPress += txtFrm_KeyPress;
            txtbox3.KeyPress += txtFrm_KeyPress;

            setAllControlVisible(false);
            dgvTarget.Controls.Add(cbControlType);
            dgvTarget.Controls.Add(cbPanleControl);
            dgvTarget.Controls.Add(cbAudioControl);
            dgvTarget.Controls.Add(txtSub);
            dgvTarget.Controls.Add(txtDev);
            dgvTarget.Controls.Add(cbbox1);
            dgvTarget.Controls.Add(cbbox2);
            dgvTarget.Controls.Add(cbbox3);
            dgvTarget.Controls.Add(txtSeries);
            dgvTarget.Controls.Add(txtbox1);
            dgvTarget.Controls.Add(txtbox2);
            dgvTarget.Controls.Add(txtbox3);

            cbControlType.SelectedIndexChanged += cbControlType_SelectedIndexChanged;
            cbPanleControl.SelectedIndexChanged += cbPanleControl_SelectedIndexChanged;
            cbAudioControl.SelectedIndexChanged += cbAudioControl_SelectedIndexChanged;
            cbbox1.SelectedIndexChanged += cbbox1_SelectedIndexChanged;
            cbbox2.SelectedIndexChanged += cbbox2_SelectedIndexChanged;
            cbbox3.SelectedIndexChanged += cbbox3_SelectedIndexChanged;
            txtSub.TextChanged += txtSub_TextChanged;
            txtDev.TextChanged += txtDev_TextChanged;
            txtbox1.TextChanged += txtbox1_TextChanged;
            txtbox2.TextChanged += txtbox2_TextChanged;
            txtbox3.TextChanged += txtbox3_TextChanged;
            txtSeries.TextChanged += txtSeries_TextChanged;
            #endregion
            #region
            cb232RSNO.Items.Clear();
            cb232RSNO.DropDownStyle = ComboBoxStyle.DropDownList;
            for (int i = 1; i <= 4; i++) cb232RSNO.Items.Add(i.ToString());
            cb232ControlMode.Items.Clear();
            cb232ControlMode.DropDownStyle = ComboBoxStyle.DropDownList;
            cb232ControlMode.Items.Add(CsConst.WholeTextsList[1775].sDisplayName);
            cb232ControlMode.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99838", ""));
            cb232ControlMode.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99839", ""));
            cb232RSNO.SelectedIndexChanged += cb232RSNO_SelectedIndexChanged;
            cb232ControlMode.SelectedIndexChanged += cb232ControlMode_SelectedIndexChanged;
            txt232Command.TextChanged += txt232Command_TextChanged;
            dgvUVTargets.Controls.Add(cb232ControlMode);
            dgvUVTargets.Controls.Add(cb232RSNO);
            dgvUVTargets.Controls.Add(txt232Command);
            #endregion
            #region
            cbLoad.Items.Clear();
            for (int i = 127; i >= 0; i--) cbLoad.Items.Add(i.ToString());
            for (int i = 1; i < 129;i++ )cbLoad.Items.Add("-"+i.ToString());
            cbLoad.DropDownStyle = ComboBoxStyle.DropDownList;
            cbLoad.SelectedIndexChanged += cbLoad_SelectedIndexChanged;
            txtLoad.TextChanged += txtLoad_TextChanged;
            txtLoad.KeyPress += txtFrm_KeyPress;
            dgvLoad.Controls.Add(txtLoad);
            dgvLoad.Controls.Add(cbLoad);
            #endregion
            #region
            cbSeqType.Items.Clear();
            cbSeqType.Items.Add(CsConst.WholeTextsList[1775].sDisplayName);
            for (int i = 0; i < 4; i++) cbSeqType.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "0020" + i.ToString(), ""));
            cbSeqType.DropDownStyle = ComboBoxStyle.DropDownList;
            cbSeqType.SelectedIndexChanged += cbSeqType_SelectedIndexChanged;
            cbParam1.Items.Clear();
            for (int i = 1; i <= 8; i++)
                cbParam1.Items.Add(i.ToString());
            cbParam1.DropDownStyle = ComboBoxStyle.DropDownList;
            cbParam1.SelectedIndexChanged += cbParam1_SelectedIndexChanged;
            cbParam2.DropDownStyle = ComboBoxStyle.DropDownList;
            cbParam2.SelectedIndexChanged += cbParam2_SelectedIndexChanged;
            cbParam3.DropDownStyle = ComboBoxStyle.DropDownList;
            cbParam3.SelectedIndexChanged += cbParam3_SelectedIndexChanged;
            cbParam4.Items.Clear();
            cbParam4.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99832", ""));
            cbParam4.Items.Add(CsConst.Status[0]);
            cbParam4.Items.Add(CsConst.Status[1]);
            cbParam4.DropDownStyle = ComboBoxStyle.DropDownList;
            cbParam4.SelectedIndexChanged += cbParam4_SelectedIndexChanged;
            txtParam1.KeyPress += txtFrm_KeyPress;
            txtParam1.TextChanged += txtParam1_TextChanged;
            txtTime.TextChanged += txtTime_TextChanged;
            DgvSeries.Controls.Add(cbSeqType);
            DgvSeries.Controls.Add(cbParam1);
            DgvSeries.Controls.Add(cbParam2);
            DgvSeries.Controls.Add(cbParam3);
            DgvSeries.Controls.Add(cbParam4);
            DgvSeries.Controls.Add(txtParam1);
            DgvSeries.Controls.Add(txtTime);
            #endregion
            CtrlsVisisbleForForm(false);
            LoadCtrsVisible(false);
            SeqCtrsVisible(false);
        }

        void txtTime_TextChanged(object sender, EventArgs e)
        {
            if (DgvSeries.CurrentRow.Index < 0) return;
            if (DgvSeries.RowCount <= 0) return;
            int index = DgvSeries.CurrentRow.Index;
            string str = HDLPF.GetStringFromTime(Convert.ToInt32(txtTime.Text), ":");
            if (txtTime.Visible)
            {
                DgvSeries[6, index].Value = str;
                if (DgvSeries.SelectedRows == null || DgvSeries.SelectedRows.Count == 0) return;
                string strTmp = DgvSeries[6, index].Value.ToString();
                if (strTmp == null) strTmp = "N/A";
                if (txtTime.Focused)
                    ModifyMultilinesIfNeeds(strTmp, 6, DgvSeries);
            }
        }
        private void SeqCtrsVisible(bool TF)
        {
            cbSeqType.Visible = TF;
            cbParam1.Visible = TF;
            txtParam1.Visible = TF;
            cbParam2.Visible = TF;
            cbParam3.Visible = TF;
            cbParam4.Visible = TF;
            txtTime.Visible = TF;
        }

        void txtParam1_TextChanged(object sender, EventArgs e)
        {
            if (DgvSeries.CurrentRow.Index < 0) return;
            if (DgvSeries.RowCount <= 0) return;
            int index = DgvSeries.CurrentRow.Index;
            if (txtParam1.Text.Length > 0)
            {
                txtParam1.Text = HDLPF.IsNumStringMode(txtParam1.Text, 0, 255);
                txtParam1.SelectionStart = txtParam1.Text.Length;
                if (cbSeqType.SelectedIndex == 2)
                    DgvSeries[2, index].Value = txtParam1.Text + "(" + CsConst.mstrINIDefault.IniReadValue("Public", "99830", "") + ")";
                else if (cbSeqType.SelectedIndex == 3)
                    DgvSeries[2, index].Value = txtParam1.Text + "(" + CsConst.WholeTextsList[934].sDisplayName + ")";
                else if (cbSeqType.SelectedIndex == 4)
                    DgvSeries[2, index].Value = txtParam1.Text + "(" + CsConst.mstrINIDefault.IniReadValue("Public", "99831", "") + ")";
                if (txtParam1.Focused)
                    ModifyMultilinesIfNeeds(DgvSeries[2, index].Value.ToString(), 2, DgvSeries);
            }
        }

        void cbParam4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DgvSeries.CurrentRow.Index < 0) return;
            if (DgvSeries.RowCount <= 0) return;
            int index = DgvSeries.CurrentRow.Index;
            DgvSeries[5, index].Value = cbParam4.Text + "(" + CsConst.WholeTextsList[2506].sDisplayName + ")";
            if (cbParam4.Focused)
                ModifyMultilinesIfNeeds(DgvSeries[5, index].Value.ToString(), 5, DgvSeries);
        }

        void cbParam3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DgvSeries.CurrentRow.Index < 0) return;
            if (DgvSeries.RowCount <= 0) return;
            int index = DgvSeries.CurrentRow.Index;
            string str = cbParam3.Text;
            DgvSeries[4, index].Value = cbParam3.Text + "(" + CsConst.mstrINIDefault.IniReadValue("public", "99898", "") + ")";
            if (cbParam3.Focused)
                ModifyMultilinesIfNeeds(DgvSeries[4, index].Value.ToString(), 4, DgvSeries);
        }

        void cbParam2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DgvSeries.CurrentRow.Index < 0) return;
            if (DgvSeries.RowCount <= 0) return;
            int index = DgvSeries.CurrentRow.Index;
            if (cbSeqType.SelectedIndex == 1)
            {
                string str = DgvSeries[4, index].Value.ToString();
                int devid = mmc.IRCodes[Convert.ToInt32(cbParam2.Text.Split('-')[0]) - 1].DevID;
                cbParam3.Items.Clear();
                cbParam3.Items.AddRange(CsConst.mstrINIDefault.IniReadValuesInALlSectionStr("NewIRDevice" + devid.ToString()));
                if (cbParam3.Items.Contains(str)) cbParam3.Text = str;
                else cbParam3.SelectedIndex = 0;
                cbParam3_SelectedIndexChanged(null, null);
                DgvSeries[3, index].Value = cbParam2.Text;
            }
            else
            {
                DgvSeries[3, index].Value = cbParam2.Text + "(" + CsConst.WholeTextsList[2529].sDisplayName + ")";
            }
            if (cbParam2.Focused)
                ModifyMultilinesIfNeeds(DgvSeries[3, index].Value.ToString(), 3, DgvSeries);
        }

        void cbParam1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DgvSeries.CurrentRow.Index < 0) return;
            if (DgvSeries.RowCount <= 0) return;
            int index = DgvSeries.CurrentRow.Index;
            DgvSeries[2, index].Value = cbParam1.Text + "(" + CsConst.WholeTextsList[2506].sDisplayName + ")";
            if (cbParam1.Focused)
                ModifyMultilinesIfNeeds(DgvSeries[2, index].Value.ToString(), 2, DgvSeries);
        }

        void cbSeqType_SelectedIndexChanged(object sender, EventArgs e)
        {
            SeqCtrsVisible(false);
            cbSeqType.Visible = true;
            int index = DgvSeries.CurrentRow.Index;
            string str1 = DgvSeries[2, index].Value.ToString();
            string str2 = DgvSeries[3, index].Value.ToString();
            string str3 = DgvSeries[4, index].Value.ToString();
            string str4 = DgvSeries[5, index].Value.ToString();
            if (str1.Contains("(")) str1 = str1.Split('(')[0].ToString();
            if (str2.Contains("(")) str2 = str2.Split('(')[0].ToString();
            if (str3.Contains("(")) str3 = str3.Split('(')[0].ToString();
            if (str4.Contains("(")) str4 = str4.Split('(')[0].ToString();
            if (cbSeqType.SelectedIndex == 1)
            {
                addcontrols(2, index, cbParam1, DgvSeries);
                cbParam2.Items.Clear();
                if (mmc.IRCodes != null && mmc.IRCodes.Count == 24)
                {
                    for (int i = 0; i < mmc.IRCodes.Count; i++)
                        cbParam2.Items.Add((i + 1).ToString() + "-" + mmc.IRCodes[i].Remark);
                }
                else
                {
                    for (int i = 0; i < 24; i++)
                        cbParam2.Items.Add((i + 1).ToString() + "-");
                }
                addcontrols(3, index, cbParam2, DgvSeries);
                addcontrols(4, index, cbParam3, DgvSeries);
                addcontrols(5, index, cbParam4, DgvSeries);
            }
            else if (cbSeqType.SelectedIndex == 2)
            {
                txtParam1.Text = str1;
                addcontrols(2, index, txtParam1, DgvSeries);
                cbParam2.Items.Clear();
                cbParam2.Items.Add(CsConst.Status[0]);
                cbParam2.Items.Add(CsConst.Status[1]);
                addcontrols(3, index, cbParam2, DgvSeries);
                DgvSeries[4, index].Value = "N/A";
                DgvSeries[5, index].Value = "N/A";
            }
            else if (cbSeqType.SelectedIndex == 3)
            {
                txtParam1.Text = str1;
                addcontrols(2, index, txtParam1, DgvSeries);
                cbParam2.Items.Clear();
                cbParam2.Items.Add(CsConst.Status[0]);
                cbParam2.Items.Add(CsConst.Status[1]);
                addcontrols(3, index, cbParam2, DgvSeries);
                DgvSeries[4, index].Value = "N/A";
                DgvSeries[5, index].Value = "N/A";
            }
            else if (cbSeqType.SelectedIndex == 4)
            {
                txtParam1.Text = str1;
                addcontrols(2, index, txtParam1, DgvSeries);
                cbParam2.Items.Clear();
                cbParam2.Items.Add(CsConst.Status[0]);
                cbParam2.Items.Add(CsConst.Status[1]);
                addcontrols(3, index, cbParam2, DgvSeries);
                DgvSeries[4, index].Value = "N/A";
                DgvSeries[5, index].Value = "N/A";
            }
            DgvSeries[1, index].Value = cbSeqType.Text;
            string str = DgvSeries[6, index].Value.ToString();
            if (!str.Contains(":"))
                txtTime.Text = "0:0";
            else
                txtTime.Text = HDLPF.GetTimeFromString(str, ':');
            addcontrols(6, index, txtTime, DgvSeries);
            #region
            if (cbParam1.Visible && cbParam1.Items.Count > 0)
            {
                if (!cbParam1.Items.Contains(str1))
                    cbParam1.SelectedIndex = 0;
                else
                    cbParam1.Text = str1;
            }
            if (cbParam2.Visible && cbParam2.Items.Count > 0)
            {
                if (!cbParam2.Items.Contains(str2))
                    cbParam2.SelectedIndex = 0;
                else
                    cbParam2.Text = str2;
            }
            if (cbParam3.Visible && cbParam3.Items.Count > 0)
            {
                if (!cbParam3.Items.Contains(str3))
                    cbParam3.SelectedIndex = 0;
                else
                    cbParam3.Text = str3;
            }
            if (cbParam4.Visible && cbParam4.Items.Count > 0)
            {
                if (!cbParam4.Items.Contains(str4))
                    cbParam4.SelectedIndex = 0;
                else
                    cbParam4.Text = str4;
            }
            #endregion
            #region
            if (cbParam1.Visible) cbParam1_SelectedIndexChanged(null, null);
            if (cbParam2.Visible) cbParam2_SelectedIndexChanged(null, null);
            if (cbParam3.Visible) cbParam3_SelectedIndexChanged(null, null);
            if (cbParam4.Visible) cbParam4_SelectedIndexChanged(null, null);
            if (txtParam1.Visible) txtParam1_TextChanged(null, null);
            #endregion
            if (cbSeqType.Focused)
                ModifyMultilinesIfNeeds(DgvSeries[1, index].Value.ToString(), 1, DgvSeries);
        }

        private void LoadCtrsVisible(bool TF)
        {
            txtLoad.Visible = TF;
            cbLoad.Visible = TF;
        }

        void txtLoad_TextChanged(object sender, EventArgs e)
        {
            if (dgvLoad.CurrentRow.Index < 0) return;
            if (dgvLoad.RowCount <= 0) return;
            int index = dgvLoad.CurrentRow.Index;
            if (txtLoad.Text.Length > 0)
            {
                txtLoad.Text = HDLPF.IsNumStringMode(txtLoad.Text, 0, 65535);
                txtLoad.SelectionStart = txtLoad.Text.Length;
                dgvLoad[2, index].Value = txtLoad.Text;
                if (txtLoad.Focused)
                    ModifyMultilinesIfNeeds(dgvLoad[2, index].Value.ToString(), 2, dgvLoad);
            }
        }

        void cbLoad_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dgvLoad.CurrentRow.Index < 0) return;
            if (dgvLoad.RowCount <= 0) return;
            int index = dgvLoad.CurrentRow.Index;
            dgvLoad[3, index].Value = cbLoad.Text;
            if (cbLoad.Focused)
                ModifyMultilinesIfNeeds(dgvLoad[3, index].Value.ToString(), 3, dgvLoad);
        }

        void txt232Command_TextChanged(object sender, EventArgs e)
        {
            if (dgvUVTargets.CurrentRow.Index < 0) return;
            if (dgvUVTargets.RowCount <= 0) return;
            int index = dgvUVTargets.CurrentRow.Index;
            if (dgvUVTargets[2, index].Value.ToString() == CsConst.WholeTextsList[1775].sDisplayName)
            {
                dgvUVTargets[3, index].Value = txt232Command.Text;
                ModifyMultilinesIfNeeds(dgvUVTargets[3, index].Value.ToString(), 3, dgvUVTargets);
            }
            else if (dgvUVTargets[2, index].Value.ToString() == CsConst.mstrINIDefault.IniReadValue("Public", "99838", ""))
            {
                dgvUVTargets[3, index].Value = txt232Command.Text;
                ModifyMultilinesIfNeeds(dgvUVTargets[3, index].Value.ToString(), 3, dgvUVTargets);
            }
            else if (dgvUVTargets[2, index].Value.ToString() == CsConst.mstrINIDefault.IniReadValue("Public", "99839", ""))
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
                dgvUVTargets[3, index].Value = str;
                if (txt232Command.Focused)
                    ModifyMultilinesIfNeeds(dgvUVTargets[3, index].Value.ToString(), 3, dgvUVTargets);
            }
        }

        void cb232ControlMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dgvUVTargets.CurrentRow.Index < 0) return;
            if (dgvUVTargets.RowCount <= 0) return;
            int index = dgvUVTargets.CurrentRow.Index;
            dgvUVTargets[2, index].Value = cb232ControlMode.Text;
            if (cb232ControlMode.Focused)
                ModifyMultilinesIfNeeds(dgvUVTargets[2, index].Value.ToString(), 2, dgvUVTargets);
            if (cb232ControlMode.SelectedIndex == 2)
            {
                txt232Command.Text = dgvUVTargets[3, index].Value.ToString();
                txt232Command_TextChanged(null, null);
            }
        }

        void cb232RSNO_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dgvUVTargets.CurrentRow.Index < 0) return;
            if (dgvUVTargets.RowCount <= 0) return;
            int index = dgvUVTargets.CurrentRow.Index;
            dgvUVTargets[1, index].Value = cb232RSNO.Text;
            if (cb232RSNO.Focused)
                ModifyMultilinesIfNeeds(dgvUVTargets[1, index].Value.ToString(), 1, dgvUVTargets);
        }

        void CtrlsVisisbleForForm(bool TF)
        {
            cb232RSNO.Visible = TF;
            cb232ControlMode.Visible = TF;
            txt232Command.Visible = TF;
        }

        void txtbox3_TextChanged(object sender, EventArgs e)
        {
            if (dgvTarget.CurrentRow.Index < 0) return;
            if (dgvTarget.RowCount <= 0) return;
            int index = dgvTarget.CurrentRow.Index;
            string str = txtbox3.Text;
            if (str.Contains("(")) str = str.Split('(')[0].ToString();
            #region
            if (cbAudioControl.Visible)
            {
                if (cbAudioControl.SelectedIndex == 2)//列表/频道
                {
                    if (cbbox2.Visible && cbbox2.Items.Count >= 6)
                    {
                        if (txtbox3.Text.Length > 0)
                        {
                            if (cbbox2.SelectedIndex == 2)//列表号
                            {
                                txtbox3.Text = HDLPF.IsNumStringMode(str, 1, 255);
                                dgvTarget[6, index].Value = txtbox3.Text + "(" + CsConst.mstrINIDefault.IniReadValue("public", "00046", "") + ")";
                            }
                            else if (cbbox2.SelectedIndex == 5)//频道号
                            {
                                txtbox3.Text = HDLPF.IsNumStringMode(str, 1, 50);
                                dgvTarget[6, index].Value = txtbox3.Text + "(" + CsConst.mstrINIDefault.IniReadValue("public", "99873", "") + ")";
                            }
                            txtbox3.SelectionStart = txtbox3.Text.Length;
                        }
                    }
                }
                else if (cbAudioControl.SelectedIndex >= 5)
                {
                    if (txtbox3.Text.Length > 0)
                    {
                        txtbox3.Text = HDLPF.IsNumStringMode(str, 1, 999);
                        dgvTarget[6, index].Value = txtbox3.Text + "(" + CsConst.mstrINIDefault.IniReadValue("public", "00043", "") + ")";
                        txtbox3.SelectionStart = txtbox3.Text.Length;
                    }
                }
            }
            #endregion
            if (dgvTarget.SelectedRows == null || dgvTarget.SelectedRows.Count == 0) return;
            string strTmp = dgvTarget[6, index].Value.ToString();
            if (strTmp == null) strTmp = "N/A";
            if (txtbox3.Focused)
                ModifyMultilinesIfNeeds(strTmp, 6,dgvTarget);
        }

        void ModifyMultilinesIfNeeds(string strTmp, int ColumnIndex,DataGridView dgv)
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

        void txtSeries_TextChanged(object sender, EventArgs e)
        {
            if (dgvTarget.CurrentRow.Index < 0) return;
            if (dgvTarget.RowCount <= 0) return;
            int index = dgvTarget.CurrentRow.Index;
            string str = HDLPF.GetStringFromTime(Convert.ToInt32(txtSeries.Text), ":");
            if (txtSeries.Visible)
            {
                dgvTarget[6, index].Value = str + "(" + CsConst.WholeTextsList[2525].sDisplayName + ")";
                if (dgvTarget.SelectedRows == null || dgvTarget.SelectedRows.Count == 0) return;
                string strTmp = dgvTarget[6, index].Value.ToString();
                if (strTmp == null) strTmp = "N/A";
                if (txtSeries.Focused)
                    ModifyMultilinesIfNeeds(strTmp, 6,dgvTarget);
            }
        }

        void txtbox2_TextChanged(object sender, EventArgs e)
        {
            if (dgvTarget.CurrentRow.Index < 0) return;
            if (dgvTarget.RowCount <= 0) return;
            int index = dgvTarget.CurrentRow.Index;
            string str = txtbox2.Text;
            if (str.Contains("(")) str = str.Split('(')[0].ToString();
            #region
            if (txtbox2.Text.Length > 0)
            {
                if (cbControlType.Text == CsConst.myPublicControlType[1].ControlTypeName || //场景
                    cbControlType.Text == CsConst.myPublicControlType[10].ControlTypeName ||//广播场景
                    cbControlType.Text == CsConst.myPublicControlType[20].ControlTypeName)  //逻辑场景
                {
                    txtbox2.Text = HDLPF.IsNumStringMode(str, 0, 255);
                    dgvTarget[5, index].Value = txtbox2.Text + "(" + CsConst.WholeTextsList[2511].sDisplayName + ")";
                }
                else if (cbControlType.Text == CsConst.myPublicControlType[2].ControlTypeName)//序列
                {
                    txtbox2.Text = HDLPF.IsNumStringMode(str, 0, 99);
                    dgvTarget[5, index].Value = txtbox2.Text + "(" + CsConst.WholeTextsList[2512].sDisplayName+ ")";
                }
                else if (cbControlType.Text == CsConst.myPublicControlType[5].ControlTypeName || //单路调节
                         cbControlType.Text == CsConst.myPublicControlType[11].ControlTypeName) //广播回路
                {
                    txtbox2.Text = HDLPF.IsNumStringMode(str, 0, 100);
                    dgvTarget[5, index].Value = txtbox2.Text + "(" + CsConst.mstrINIDefault.IniReadValue("public", "00011", "") + ")";
                }
                else if (cbControlType.Text == CsConst.myPublicControlType[7].ControlTypeName)//GPRS控制
                {
                    txtbox2.Text = HDLPF.IsNumStringMode(str, 1, 99);
                    dgvTarget[5, index].Value = txtbox2.Text + "(" + CsConst.mstrINIDefault.IniReadValue("public", "99864", "") + ")";
                }
                else if (cbControlType.Text == CsConst.myPublicControlType[8].ControlTypeName)//面板控制
                {
                    if (cbPanleControl.Text == CsConst.myPublicControlType[3].ControlTypeName ||//背光亮度
                        cbPanleControl.Text == CsConst.myPublicControlType[4].ControlTypeName) //状态灯亮度
                    {
                        txtbox2.Text = HDLPF.IsNumStringMode(str, 0, 100);
                        dgvTarget[5, index].Value = txtbox2.Text + "(" + CsConst.WholeTextsList[2524].sDisplayName + ")";
                    }
                    else if (cbPanleControl.Text ==  CsConst.PanelControl[15] ||//空调升高温度 
                             cbPanleControl.Text ==  CsConst.PanelControl[16] ||//空调降低温度
                             cbPanleControl.Text == CsConst.PanelControl[23] ||//地热身高温度
                             cbPanleControl.Text == CsConst.PanelControl[24]) //地热降低温度
                    {
                        txtbox2.Text = HDLPF.IsNumStringMode(str, 1, 255);
                        dgvTarget[5, index].Value = txtbox2.Text;

                    }
                }
                else if (cbControlType.Text == CsConst.myPublicControlType[13].ControlTypeName)//音乐播放
                {
                    if (cbAudioControl.SelectedIndex == 5)//歌曲播放
                    {
                        txtbox2.Text = HDLPF.IsNumStringMode(str, 0, 255);
                        dgvTarget[5, index].Value = txtbox2.Text + "(" + CsConst.mstrINIDefault.IniReadValue("public", "00046", "") + ")";
                    }
                }
                else if (cbControlType.Text == CsConst.myPublicControlType[18].ControlTypeName)//红外控制
                {
                    txtbox2.Text = HDLPF.IsNumStringMode(str, 0, 255);
                    dgvTarget[5, index].Value = txtbox2.Text + "(" + CsConst.WholeTextsList[216].sDisplayName + ")";

                }
                else if (cbControlType.Text == CsConst.myPublicControlType[18].ControlTypeName)//逻辑灯调节
                {
                    txtbox2.Text = HDLPF.IsNumStringMode(str, 1, 4);
                    dgvTarget[5, index].Value = txtbox2.Text + "(" + CsConst.WholeTextsList[2439].sDisplayName + ")";
                }
                txtbox2.SelectionStart = txtbox2.Text.Length;
            }
            #endregion
            if (dgvTarget.SelectedRows == null || dgvTarget.SelectedRows.Count == 0) return;
            string strTmp = dgvTarget[5, index].Value.ToString();
            if (strTmp == null) strTmp = "N/A";
            if (txtbox2.Focused)
                ModifyMultilinesIfNeeds(strTmp, 5,dgvTarget);
        }

        void txtbox1_TextChanged(object sender, EventArgs e)
        {
            if (dgvTarget.CurrentRow.Index < 0) return;
            if (dgvTarget.RowCount <= 0) return;
            int index = dgvTarget.CurrentRow.Index;
            string str = txtbox1.Text;
            if (str.Contains("(")) str = str.Split('(')[0].ToString();
            #region
            if (txtbox1.Text.Length > 0)
            {
                if (cbControlType.Text == CsConst.myPublicControlType[1].ControlTypeName || //场景
                    cbControlType.Text == CsConst.myPublicControlType[2].ControlTypeName ||//序列
                    cbControlType.Text == CsConst.myPublicControlType[20].ControlTypeName)   //逻辑场景
                {
                    txtbox1.Text = HDLPF.IsNumStringMode(str, 1, 48);
                    dgvTarget[4, index].Value = txtbox1.Text + "(" + CsConst.WholeTextsList[2510].sDisplayName + ")";
                }
                else if (cbControlType.Text == CsConst.myPublicControlType[4].ControlTypeName)//通用开关
                {
                    txtbox1.Text = HDLPF.IsNumStringMode(str, 1, 255);
                    dgvTarget[4, index].Value = txtbox1.Text + "(" + CsConst.WholeTextsList[2513].sDisplayName + ")";
                }
                else if (cbControlType.Text == CsConst.myPublicControlType[5].ControlTypeName)//单路调节
                {
                    txtbox1.Text = HDLPF.IsNumStringMode(str, 1, 240);
                    dgvTarget[4, index].Value = txtbox1.Text + "(" + CsConst.WholeTextsList[934].sDisplayName + ")";
                }
                else if (cbControlType.Text == CsConst.myPublicControlType[6].ControlTypeName)//窗帘开关
                {
                    txtbox1.Text = HDLPF.IsNumStringMode(str, 1, 99);
                    dgvTarget[4, index].Value = txtbox1.Text + "(" + CsConst.mstrINIDefault.IniReadValue("public", "99844", "") + ")";
                }
                else if (cbControlType.Text == CsConst.myPublicControlType[12].ControlTypeName)//消防模块
                {
                    txtbox1.Text = HDLPF.IsNumStringMode(str, 1, 8);
                    dgvTarget[4, index].Value = txtbox1.Text + "(" + CsConst.WholeTextsList[2510].sDisplayName + ")";
                }
                else if (cbControlType.Text == CsConst.myPublicControlType[15].ControlTypeName)//连接页
                {
                    txtbox1.Text = HDLPF.IsNumStringMode(str, 1, 7);
                    dgvTarget[4, index].Value = txtbox1.Text + "(" + CsConst.WholeTextsList[2510].sDisplayName + ")";
                }
                else if (cbControlType.Text == CsConst.myPublicControlType[20].ControlTypeName)//逻辑灯调节
                {
                    txtbox1.Text = HDLPF.IsNumStringMode(str, 0, 255);
                    dgvTarget[4, index].Value = txtbox1.Text + "(" + CsConst.WholeTextsList[2440].sDisplayName + ")";
                }
                else if (cbControlType.Text == CsConst.myPublicControlType[18].ControlTypeName)//红外控制
                {
                    txtbox1.Text = HDLPF.IsNumStringMode(str, 0, 255);
                    dgvTarget[4, index].Value = txtbox1.Text + "(" + CsConst.mstrINIDefault.IniReadValue("public", "99921", "") + ")";
                }
                txtbox1.SelectionStart = txtbox1.Text.Length;
            }
            #endregion

            if (dgvTarget.SelectedRows == null || dgvTarget.SelectedRows.Count == 0) return;
            string strTmp = dgvTarget[4, index].Value.ToString();
            if (strTmp == null) strTmp = "N/A";
            if (txtbox1.Focused)
                ModifyMultilinesIfNeeds(strTmp, 4,dgvTarget);
        }

        void txtDev_TextChanged(object sender, EventArgs e)
        {
            if (dgvTarget.CurrentRow.Index < 0) return;
            if (dgvTarget.RowCount <= 0) return;
            int index = dgvTarget.CurrentRow.Index;
            if (txtDev.Text.Length > 0)
            {
                txtDev.Text = HDLPF.IsNumStringMode(txtDev.Text, 0, 254);
                txtDev.SelectionStart = txtDev.Text.Length;
                dgvTarget[2, index].Value = txtDev.Text;
                if (txtDev.Focused)
                    ModifyMultilinesIfNeeds(dgvTarget[2, index].Value.ToString(), 2, dgvTarget);
            }
        }

        void txtSub_TextChanged(object sender, EventArgs e)
        {
            if (dgvTarget.CurrentRow.Index < 0) return;
            if (dgvTarget.RowCount <= 0) return;
            int index = dgvTarget.CurrentRow.Index;
            if (txtSub.Text.Length > 0)
            {
                txtSub.Text = HDLPF.IsNumStringMode(txtSub.Text, 0, 254);
                txtSub.SelectionStart = txtSub.Text.Length;
                dgvTarget[1, index].Value = txtSub.Text;
                if (txtSub.Focused)
                    ModifyMultilinesIfNeeds(dgvTarget[1, index].Value.ToString(), 1, dgvTarget);
            }
        }

        void cbbox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dgvTarget.CurrentRow.Index < 0) return;
            if (dgvTarget.RowCount <= 0) return;
            int index = dgvTarget.CurrentRow.Index;
            string str = dgvTarget[6, index].Value.ToString();
            if (str.Contains("(")) str = str.Split('(')[0].ToString();
            if (cbPanleControl.Visible && cbPanleControl.Text == CsConst.myPublicControlType[22].ControlTypeName)
            {
                if (cbbox3.SelectedIndex == 8)
                    dgvTarget[6, index].Value = cbbox3.Text;
                else
                    dgvTarget[6, index].Value = cbbox3.Text.ToString() + "(" + CsConst.WholeTextsList[934].sDisplayName + ")";
            }
            else if (cbPanleControl.Visible &&
                    (cbPanleControl.Text == CsConst.myPublicControlType[25].ControlTypeName ||
                     cbPanleControl.Text == CsConst.myPublicControlType[26].ControlTypeName ||
                     cbPanleControl.Text == CsConst.myPublicControlType[27].ControlTypeName ||
                     cbPanleControl.Text == CsConst.myPublicControlType[28].ControlTypeName))
            {
                if (cbbox3.SelectedIndex == 8)
                    dgvTarget[6, index].Value = cbbox3.Text;
                else
                    dgvTarget[6, index].Value = cbbox3.Text.ToString() + "(" + CsConst.WholeTextsList[934].sDisplayName + ")";
            }
            else
                dgvTarget[6, index].Value = cbbox3.Text;
            #region
            if (dgvTarget.SelectedRows == null || dgvTarget.SelectedRows.Count == 0) return;
            string strTmp = dgvTarget[6, index].Value.ToString();
            if (strTmp == null) strTmp = "N/A";
            if (cbbox3.Focused)
                ModifyMultilinesIfNeeds(strTmp, 6,dgvTarget);
            #endregion
        }

        void cbbox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dgvTarget.CurrentRow.Index < 0) return;
            if (dgvTarget.RowCount <= 0) return;
            txtbox3.Visible = false;
            cbbox3.Visible = false;
            int index = dgvTarget.CurrentRow.Index;
            string str = dgvTarget[5, index].Value.ToString();
            if (str.Contains("(")) str = str.Split('(')[0].ToString();
            string str3 = dgvTarget[6, index].Value.ToString();
            if (str3.Contains("(")) str3 = str3.Split('(')[0].ToString();
            #region
            if (cbControlType.Text == CsConst.myPublicControlType[4].ControlTypeName || //通用开关
                cbControlType.Text == CsConst.myPublicControlType[6].ControlTypeName) //窗帘开关
            {
                dgvTarget[5, index].Value = cbbox2.Text + "(" + CsConst.WholeTextsList[2529].sDisplayName + ")";
                if (cbControlType.Text == CsConst.myPublicControlType[6].ControlTypeName)
                {
                    if (cbbox2.SelectedIndex > 2)
                    {
                        txtbox1_TextChanged(null, null);
                    }
                    else
                    {
                        addcontrols(4, index, txtbox1,dgvTarget);
                    }
                }
            }
            else if (cbControlType.Text == CsConst.myPublicControlType[8].ControlTypeName)//面板控制
            {
                if (cbPanleControl.Text == CsConst.myPublicControlType[1].ControlTypeName ||
                    cbPanleControl.Text == CsConst.myPublicControlType[2].ControlTypeName ||
                    cbPanleControl.Text == CsConst.myPublicControlType[5].ControlTypeName ||
                    cbPanleControl.Text == CsConst.myPublicControlType[7].ControlTypeName ||
                    cbPanleControl.Text == CsConst.myPublicControlType[8].ControlTypeName ||
                    cbPanleControl.Text ==  CsConst.PanelControl[12] ||
                    cbPanleControl.Text == CsConst.PanelControl[21])
                {
                    dgvTarget[5, index].Value = cbbox2.Text + "(" + CsConst.WholeTextsList[2529].sDisplayName + ")";
                }
                else if (cbPanleControl.Text == CsConst.PanelControl[17] ||
                         cbPanleControl.Text == CsConst.PanelControl[18] ||
                         cbPanleControl.Text == CsConst.PanelControl[19] ||
                         cbPanleControl.Text == CsConst.PanelControl[20])
                {
                    dgvTarget[5, index].Value = cbbox2.Text + "(" + CsConst.mstrINIDefault.IniReadValue("public", "99874", "") + ")";
                }
                else if (cbPanleControl.Text ==  CsConst.PanelControl[13] ||
                         cbPanleControl.Text == CsConst.myPublicControlType[22].ControlTypeName)
                {
                    dgvTarget[5, index].Value = cbbox2.Text + "(" + CsConst.mstrINIDefault.IniReadValue("public", "99876", "") + ")";
                }
                else if (cbPanleControl.Text ==  CsConst.PanelControl[14])
                {
                    dgvTarget[5, index].Value = cbbox2.Text + "(" + CsConst.mstrINIDefault.IniReadValue("public", "99875", "") + ")";
                }
                else if (cbPanleControl.Text == CsConst.myPublicControlType[6].ControlTypeName ||
                         cbPanleControl.Text == CsConst.myPublicControlType[9].ControlTypeName ||
                         cbPanleControl.Text ==  CsConst.PanelControl[10] ||
                         cbPanleControl.Text ==  CsConst.PanelControl[11])
                {
                    dgvTarget[5, index].Value = cbbox2.Text;
                }
                else if (cbPanleControl.Text == CsConst.myPublicControlType[25].ControlTypeName ||
                         cbPanleControl.Text == CsConst.myPublicControlType[26].ControlTypeName ||
                         cbPanleControl.Text == CsConst.myPublicControlType[27].ControlTypeName ||
                         cbPanleControl.Text == CsConst.myPublicControlType[28].ControlTypeName)
                {
                    dgvTarget[5, index].Value = cbbox2.Text + "(" + CsConst.mstrINIDefault.IniReadValue("public", "99874", "") + ")";
                }
                else if (cbPanleControl.Text == CsConst.PanelControl[29])
                {
                    dgvTarget[5, index].Value = cbbox2.Text;
                }
                else if (cbPanleControl.Text == CsConst.PanelControl[30])
                {
                    dgvTarget[5, index].Value = cbbox2.Text + "(" + CsConst.mstrINIDefault.IniReadValue("public", "99846", "") + ")";
                }
            }
            else if (cbControlType.Text == CsConst.myPublicControlType[12].ControlTypeName)//消防模块
            {
                dgvTarget[5, index].Value = cbbox2.Text;
            }
            else if (cbControlType.Text == CsConst.myPublicControlType[13].ControlTypeName)//音乐模块
            {
                if (cbAudioControl.SelectedIndex == 0 || cbAudioControl.SelectedIndex == 1 ||
                    cbAudioControl.SelectedIndex == 2 || cbAudioControl.SelectedIndex == 3 ||
                    cbAudioControl.SelectedIndex == 4 || cbAudioControl.SelectedIndex == 6 ||
                    cbAudioControl.SelectedIndex == 7)
                {
                    dgvTarget[5, index].Value = cbbox2.Text;
                }
            }
            else if (cbControlType.Text == CsConst.myPublicControlType[3].ControlTypeName)//时间开关
            {
                dgvTarget[5, index].Value = cbbox2.Text + "(" + CsConst.WholeTextsList[2529].sDisplayName + ")";
            }
            else if (cbControlType.Text == CsConst.myPublicControlType[15].ControlTypeName)//连接页
            {
                dgvTarget[5, index].Value = cbbox2.Text;
            }
            else if (cbControlType.Text == CsConst.myPublicControlType[21].ControlTypeName)//休眠模式
            {
                dgvTarget[5, index].Value = cbbox2.Text + "(" + CsConst.WholeTextsList[2437].sDisplayName + ")";
                txtbox3.Visible = true;
            }
            #endregion
            #region
            if (cbAudioControl.Visible)
            {
                if (cbAudioControl.SelectedIndex == 2)
                {
                    if (cbbox2.Visible && cbbox2.Items.Count >= 6)
                    {
                        if (cbbox2.SelectedIndex == 2 || cbbox2.SelectedIndex == 5)
                        {
                            txtbox3.Text = str3;
                            addcontrols(6, index, txtbox3,dgvTarget);
                        }
                    }
                }
                else if (cbAudioControl.SelectedIndex == 4)
                {
                    if (cbbox2.Visible && cbbox2.Items.Count > 0)
                    {
                        if (cbbox2.SelectedIndex == 0)
                        {
                            cbbox3.Items.Clear();
                            cbbox3.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00044", ""));
                            cbbox3.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00045", ""));
                            for (int i = 0; i < 80; i++)
                                cbbox3.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99872", "") + ":" + i.ToString());
                        }
                        else
                        {
                            cbbox3.Items.Clear();
                            cbbox3.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00044", ""));
                            cbbox3.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00045", ""));
                        }
                        addcontrols(6, index, cbbox3, dgvTarget);
                    }
                }
                else if (cbAudioControl.SelectedIndex == 6 || cbAudioControl.SelectedIndex == 7)//直接源播放 广播播放
                {
                    txtbox3.Visible = true;
                }
            }
            else if (cbPanleControl.Visible)
            {
                if ((cbPanleControl.Text == CsConst.myPublicControlType[6].ControlTypeName ||//面板页面锁 
                     cbPanleControl.Text == CsConst.myPublicControlType[9].ControlTypeName ||//按键锁
                     cbPanleControl.Text ==  CsConst.PanelControl[10] ||//控制按键状态
                     cbPanleControl.Text ==  CsConst.PanelControl[11] || //控制面板按键
                     cbPanleControl.Text == CsConst.PanelControl[21] ||//地热开关
                     cbPanleControl.Text == CsConst.myPublicControlType[22].ControlTypeName)) //地热模式
                {
                    cbbox3.Visible = true;
                }
            }
            #endregion
            #region
            if (cbbox3.Visible && cbbox3.Items.Count > 0)
            {
                if (!cbbox3.Items.Contains(str3))
                    cbbox3.SelectedIndex = 0;
                else
                    cbbox3.Text = str3;
            }
            #endregion
            #region
            if (txtbox3.Visible) txtbox3_TextChanged(null, null);
            if (cbbox3.Visible) cbbox3_SelectedIndexChanged(null, null);
            #endregion
            #region
            if (dgvTarget.SelectedRows == null || dgvTarget.SelectedRows.Count == 0) return;
            string strTmp = dgvTarget[5, index].Value.ToString();
            if (strTmp == null) strTmp = "N/A";
            if (cbbox2.Focused)
                ModifyMultilinesIfNeeds(strTmp, 5,dgvTarget);
            #endregion
        }

        void cbbox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dgvTarget.CurrentRow.Index < 0) return;
            if (dgvTarget.RowCount <= 0) return;
            int index = dgvTarget.CurrentRow.Index;
            string str = dgvTarget[4, index].Value.ToString();
            if (str.Contains("(")) str = str.Split('(')[0].ToString();
            if (cbControlType.SelectedIndex == 6)//GPRS控制
            {
                dgvTarget[4, index].Value = cbbox1.Text;
            }
            #region
            if (dgvTarget.SelectedRows == null || dgvTarget.SelectedRows.Count == 0) return;
            string strTmp = dgvTarget[4, index].Value.ToString();
            if (strTmp == null) strTmp = "N/A";
            if (cbbox1.Focused)
                ModifyMultilinesIfNeeds(strTmp, 4,dgvTarget);
            #endregion
        }


        void cbAudioControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbbox2.Visible = false;
            cbbox3.Visible = false;
            txtbox2.Visible = false;
            txtbox3.Visible = false;
            int index = dgvTarget.CurrentRow.Index;
            string str2 = dgvTarget[5, index].Value.ToString();
            string str3 = dgvTarget[6, index].Value.ToString();
            if (str2.Contains("(")) str2 = str2.Split('(')[0].ToString();
            if (str3.Contains("(")) str3 = str3.Split('(')[0].ToString();
            if (cbAudioControl.SelectedIndex == 0 || cbAudioControl.SelectedIndex == 1 ||//音源选择 播放模式
                cbAudioControl.SelectedIndex == 2 || cbAudioControl.SelectedIndex == 3 ||//列表/频道  播放控制
                cbAudioControl.SelectedIndex == 4)
            {
                #region
                cbbox2.Items.Clear();
                if (cbAudioControl.SelectedIndex == 0)//音源选择
                {
                    #region
                    for (int i = 1; i <= 4; i++)
                        cbbox2.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "0010" + i.ToString(), ""));
                    #endregion
                }
                else if (cbAudioControl.SelectedIndex == 1)
                {
                    #region
                    for (int i = 1; i <= 4; i++)
                        cbbox2.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "0011" + i.ToString(), ""));
                    #endregion
                }
                else if (cbAudioControl.SelectedIndex == 2)
                {
                    #region
                    for (int i = 1; i <= 6; i++)
                        cbbox2.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "0012" + i.ToString(), ""));
                    #endregion
                }
                else if (cbAudioControl.SelectedIndex == 3)
                {
                    #region
                    for (int i = 1; i <= 4; i++)
                        cbbox2.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "0013" + i.ToString(), ""));
                    #endregion
                }
                else if (cbAudioControl.SelectedIndex == 4)
                {
                    #region
                    for (int i = 1; i <= 3; i++)
                        cbbox2.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "0014" + i.ToString(), ""));
                    #endregion
                }

                addcontrols(5, index, cbbox2,dgvTarget);
                dgvTarget[6, index].Value = "N/A";
                #endregion
            }
            else if (cbAudioControl.SelectedIndex == 5)//歌曲播放
            {
                #region
                txtbox2.Text = str2;
                addcontrols(5, index, txtbox2, dgvTarget);

                txtbox3.Text = str3;
                addcontrols(6, index, txtbox3, dgvTarget);
                #endregion
            }
            else if (cbAudioControl.SelectedIndex == 6 || cbAudioControl.SelectedIndex == 7)//直接源播放 广播播放
            {
                #region
                cbbox2.Items.Clear();
                cbbox2.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00047", ""));
                for (int i = 1; i <= 48; i++)
                    cbbox2.Items.Add("SD:" + i.ToString());
                for (int i = 1; i <= 48; i++)
                    cbbox2.Items.Add("FTP:" + i.ToString());
                addcontrols(5, index, cbbox2, dgvTarget);
                txtbox3.Text = str3;
                addcontrols(6, index, txtbox3, dgvTarget);
                #endregion
            }
            dgvTarget[4, index].Value = cbAudioControl.Text;
            #region
            if (cbbox2.Visible && cbbox2.Items.Count > 0)
            {
                if (!cbbox2.Items.Contains(str2))
                    cbbox2.SelectedIndex = 0;
                else
                    cbbox2.Text = str2;
            }
            if (cbbox3.Visible && cbbox3.Items.Count > 0)
            {
                if (!cbbox3.Items.Contains(str3))
                    cbbox3.SelectedIndex = 0;
                else
                    cbbox3.Text = str3;
            }
            #endregion
            #region
            if (txtbox2.Visible) txtbox2_TextChanged(null, null);
            if (txtbox3.Visible) txtbox3_TextChanged(null, null);
            if (cbbox2.Visible) cbbox2_SelectedIndexChanged(null, null);
            if (cbbox3.Visible) cbbox3_SelectedIndexChanged(null, null);
            #endregion
            if (cbAudioControl.Focused)
                ModifyMultilinesIfNeeds(dgvTarget[4, index].Value.ToString(), 4,dgvTarget);
        }

        void cbPanleControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbbox2.Visible = false;
            cbbox3.Visible = false;
            txtbox2.Visible = false;
            txtbox3.Visible = false;
            int index = dgvTarget.CurrentRow.Index;
            string str2 = dgvTarget[5, index].Value.ToString();
            string str3 = dgvTarget[6, index].Value.ToString();
            if (str2.Contains("(")) str2 = str2.Split('(')[0].ToString();
            if (str3.Contains("(")) str3 = str3.Split('(')[0].ToString();
            if (cbPanleControl.Text == CsConst.myPublicControlType[0].ControlTypeName)//无效
            {
                #region
                dgvTarget[5, index].Value = "N/A";
                dgvTarget[6, index].Value = "N/A";
                #endregion
            }
            else if (cbPanleControl.Text == CsConst.myPublicControlType[1].ControlTypeName ||//红外接收使能 
                     cbPanleControl.Text == CsConst.myPublicControlType[2].ControlTypeName ||//背光开关
                     cbPanleControl.Text == CsConst.myPublicControlType[5].ControlTypeName ||//面板全锁
                     cbPanleControl.Text == CsConst.myPublicControlType[7].ControlTypeName ||//空调锁
                     cbPanleControl.Text == CsConst.myPublicControlType[8].ControlTypeName ||//配置页面锁 
                     cbPanleControl.Text ==  CsConst.PanelControl[12] ||//空调开关
                     cbPanleControl.Text ==  CsConst.PanelControl[13] ||//空调模式 
                     cbPanleControl.Text ==  CsConst.PanelControl[14])  //空调风速
            {
                #region
                cbbox2.Items.Clear();
                if (cbPanleControl.Text ==  CsConst.PanelControl[13])
                {
                    for (int i = 0; i < 5; i++)
                        cbbox2.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "0005" + i.ToString(), ""));
                }
                else if (cbPanleControl.Text ==  CsConst.PanelControl[14])
                {
                    for (int i = 0; i < 4; i++)
                        cbbox2.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "0006" + i.ToString(), ""));
                }
                else
                {
                    cbbox2.Items.Add(CsConst.Status[0]);
                    cbbox2.Items.Add(CsConst.Status[1]);
                }
                addcontrols(5, index, cbbox2, dgvTarget);
                dgvTarget[6, index].Value = "N/A";
                #endregion
            }
            else if (cbPanleControl.Text == CsConst.myPublicControlType[3].ControlTypeName ||//背光灯亮度
                     cbPanleControl.Text == CsConst.myPublicControlType[4].ControlTypeName ||//状态灯亮度
                     cbPanleControl.Text ==  CsConst.PanelControl[15] ||//空调升高温度 
                     cbPanleControl.Text ==  CsConst.PanelControl[16])//空调降低温度
            {
                #region
                txtbox2.Text = str2;
                addcontrols(5, index, txtbox2, dgvTarget);
                dgvTarget[6, index].Value = "N/A";
                #endregion
            }
            else if (cbPanleControl.Text == CsConst.myPublicControlType[6].ControlTypeName ||//面板页面锁
                     cbPanleControl.Text == CsConst.myPublicControlType[9].ControlTypeName ||//按键锁
                     cbPanleControl.Text ==  CsConst.PanelControl[10] ||//控制按键状态
                     cbPanleControl.Text ==  CsConst.PanelControl[11] ||//控制面板按键
                     cbPanleControl.Text == CsConst.PanelControl[21] ||//地热开关
                     cbPanleControl.Text == CsConst.myPublicControlType[22].ControlTypeName) //地热模式
            {
                #region
                cbbox2.Items.Clear();
                cbbox3.Items.Clear();
                if (cbPanleControl.Text == CsConst.myPublicControlType[6].ControlTypeName)//面板页面锁
                {
                    #region
                    cbbox2.Items.Add(CsConst.WholeTextsList[1775].sDisplayName);
                    for (int i = 1; i <= 7; i++)
                        cbbox2.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00048", "") + i.ToString());
                    cbbox2.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00049", ""));

                    cbbox3.Items.Add(CsConst.WholeTextsList[1775].sDisplayName);
                    cbbox3.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99866", ""));
                    #endregion
                }
                else if (cbPanleControl.Text == CsConst.myPublicControlType[9].ControlTypeName)//按键锁
                {
                    #region
                    cbbox2.Items.Add(CsConst.WholeTextsList[1775].sDisplayName);
                    for (int i = 1; i <= 56; i++)
                        cbbox2.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99867", "") + i.ToString());
                    cbbox2.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00049", ""));

                    cbbox3.Items.Add(CsConst.WholeTextsList[1775].sDisplayName);
                    cbbox3.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99866", ""));
                    #endregion
                }
                else if (cbPanleControl.Text ==  CsConst.PanelControl[10])//控制按键状态
                {
                    #region
                    cbbox2.Items.Add(CsConst.WholeTextsList[1775].sDisplayName);
                    for (int i = 1; i <= 32; i++)
                        cbbox2.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99867", "") + i.ToString());
                    cbbox2.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99868", ""));
                    cbbox2.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99869", ""));
                    cbbox2.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99870", ""));
                    cbbox2.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99871", ""));
                    cbbox2.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00049", ""));

                    cbbox3.Items.Add(CsConst.WholeTextsList[1775].sDisplayName);
                    cbbox3.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99866", ""));
                    #endregion
                }
                else if (cbPanleControl.Text ==  CsConst.PanelControl[11])//控制面板按键
                {
                    #region
                    cbbox2.Items.Add(CsConst.WholeTextsList[1775].sDisplayName);
                    for (int i = 1; i <= 32; i++)
                        cbbox2.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99867", "") + i.ToString());

                    cbbox3.Items.Add(CsConst.WholeTextsList[1775].sDisplayName);
                    cbbox3.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99866", ""));
                    #endregion
                }
                else if (cbPanleControl.Text == CsConst.PanelControl[21])//地热开关
                {
                    #region
                    cbbox2.Items.Add(CsConst.Status[0]);
                    cbbox2.Items.Add(CsConst.Status[1]);

                    for (int i = 1; i <= 8; i++)
                        cbbox3.Items.Add(i.ToString());
                    cbbox3.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00049", ""));
                    #endregion
                }
                else if (cbPanleControl.Text == CsConst.myPublicControlType[22].ControlTypeName)//地热模式
                {
                    #region
                    for (int i = 0; i <= 4; i++)
                        cbbox2.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "0007" + i.ToString(), ""));

                    for (int i = 1; i <= 8; i++)
                        cbbox3.Items.Add(i.ToString());
                    cbbox3.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00049", ""));
                    #endregion
                }
                addcontrols(5, index, cbbox2, dgvTarget);
                addcontrols(6, index, cbbox3, dgvTarget);
                #endregion
            }
            else if (cbPanleControl.Text == CsConst.PanelControl[17] ||//空调制冷温度
                     cbPanleControl.Text == CsConst.PanelControl[18] ||//空调制热温度
                     cbPanleControl.Text == CsConst.PanelControl[19] ||//空调自动温度 
                     cbPanleControl.Text == CsConst.PanelControl[20])//空调除湿温度 
            {
                #region
                cbbox2.Items.Clear();
                for (int i = 0; i < 31; i++)
                    cbbox2.Items.Add(i.ToString() + "C");
                for (int i = 32; i < 87; i++)
                    cbbox2.Items.Add(i.ToString() + "F");
                addcontrols(5, index, cbbox2, dgvTarget);
                dgvTarget[6, index].Value = "N/A";
                #endregion
            }
            else if (cbPanleControl.Text == CsConst.PanelControl[23] ||//地热身高温度
                     cbPanleControl.Text == CsConst.PanelControl[24]) //地热降低温度
            {
                #region
                txtbox2.Text = str2;
                addcontrols(5, index, txtbox2, dgvTarget);
                cbbox3.Items.Clear();
                for (int i = 1; i <= 8; i++)
                    cbbox3.Items.Add(i.ToString());
                cbbox3.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00049", ""));
                addcontrols(6, index, cbbox3, dgvTarget);
                #endregion
            }
            dgvTarget[4, index].Value = cbPanleControl.Text;
            #region
            if (cbbox2.Visible && cbbox2.Items.Count > 0)
            {
                if (!cbbox2.Items.Contains(str2))
                    cbbox2.SelectedIndex = 0;
                else
                    cbbox2.Text = str2;
            }
            if (cbbox3.Visible && cbbox3.Items.Count > 0)
            {
                if (!cbbox3.Items.Contains(str3))
                    cbbox3.SelectedIndex = 0;
                else
                    cbbox3.Text = str3;
            }
            #endregion
            #region
            if (txtbox2.Visible) txtbox2_TextChanged(null, null);
            if (txtbox3.Visible) txtbox3_TextChanged(null, null);
            if (cbbox2.Visible) cbbox2_SelectedIndexChanged(null, null);
            if (cbbox3.Visible) cbbox3_SelectedIndexChanged(null, null);
            #endregion
            if (cbPanleControl.Focused)
                ModifyMultilinesIfNeeds(dgvTarget[4, index].Value.ToString(), 4,dgvTarget);
        }

        void cbControlType_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbPanleControl.Visible = false;
            cbAudioControl.Visible = false;
            cbbox1.Visible = false;
            cbbox2.Visible = false;
            cbbox3.Visible = false;
            txtbox1.Visible = false;
            txtbox2.Visible = false;
            txtbox3.Visible = false;
            txtSeries.Visible = false;
            int index = dgvTarget.CurrentRow.Index;
            string str1 = dgvTarget[4, index].Value.ToString();
            string str2 = dgvTarget[5, index].Value.ToString();
            string str3 = dgvTarget[6, index].Value.ToString();
            if (str1.Contains('(')) str1 = str1.Split('(')[0].ToString();
            if (str2.Contains('(')) str2 = str2.Split('(')[0].ToString();
            if (str3.Contains('(')) str3 = str3.Split('(')[0].ToString();
            if (cbControlType.Text == CsConst.mstrINIDefault.IniReadValue("TargetType", "00000", ""))//无效
            {
                #region
                if (dgvTarget.SelectedRows.Count > 0)
                {
                    for (int i = 0; i < dgvTarget.SelectedRows.Count; i++)
                    {
                        dgvTarget.SelectedRows[i].Cells[3].Value = cbControlType.Items[0].ToString();
                        dgvTarget[4, index].Value = "N/A";
                        dgvTarget[5, index].Value = "N/A";
                        dgvTarget[6, index].Value = "N/A";
                    }
                }
                #endregion
            }
            else if (cbControlType.Text == CsConst.myPublicControlType[1].ControlTypeName ||//场景
                     cbControlType.Text == CsConst.myPublicControlType[2].ControlTypeName) //序列
            {
                #region
                txtbox1.Text = str1;
                addcontrols(4, index, txtbox1, dgvTarget);

                txtbox2.Text = str2;
                addcontrols(5, index, txtbox2, dgvTarget);
                dgvTarget[6, index].Value = "N/A";
                #endregion
            }
            else if (cbControlType.Text == CsConst.myPublicControlType[4].ControlTypeName ||//通用开关
                     cbControlType.Text == CsConst.myPublicControlType[6].ControlTypeName || //窗帘开关 
                     cbControlType.Text == CsConst.myPublicControlType[12].ControlTypeName) //消防模块
            {
                #region
                txtbox1.Text = str1;
                addcontrols(4, index, txtbox1, dgvTarget);

                cbbox2.Items.Clear();
                if (cbControlType.Text == CsConst.myPublicControlType[4].ControlTypeName)
                {
                    cbbox2.Items.Add(CsConst.Status[0]);
                    cbbox2.Items.Add(CsConst.Status[1]);
                }
                else if (cbControlType.Text == CsConst.myPublicControlType[6].ControlTypeName)
                {
                    cbbox2.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00036", ""));
                    cbbox2.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00037", ""));
                    cbbox2.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00038", ""));
                    for (int i = 1; i <= 100; i++)
                        cbbox2.Items.Add(i.ToString() + "%");
                }
                else if (cbControlType.Text == CsConst.myPublicControlType[12].ControlTypeName)
                {
                    for (int i = 0; i < 10; i++)
                        cbbox2.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "0008" + i.ToString(), ""));
                }
                addcontrols(5, index, cbbox2, dgvTarget);
                dgvTarget[6, index].Value = "N/A";
                #endregion
            }
            else if (cbControlType.Text == CsConst.myPublicControlType[5].ControlTypeName)//单路调节
            {
                #region
                txtbox1.Text = str1;
                addcontrols(4, index, txtbox1, dgvTarget);

                txtbox2.Text = str2;
                addcontrols(5, index, txtbox2, dgvTarget);


                if (!str3.Contains(":"))
                    txtSeries.Text = "0:0";
                else
                    txtSeries.Text = HDLPF.GetTimeFromString(str3, ':');
                addcontrols(6, index, txtSeries, dgvTarget);
                #endregion
            }
            else if (cbControlType.Text == CsConst.myPublicControlType[7].ControlTypeName)//GPRS控制
            {
                #region
                cbbox1.Items.Clear();
                cbbox1.Items.Add(CsConst.WholeTextsList[1775].sDisplayName);
                cbbox1.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99862", ""));
                cbbox1.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99863", ""));
                addcontrols(4, index, cbbox1, dgvTarget);

                txtbox2.Text = str2;
                addcontrols(5, index, txtbox2, dgvTarget);
                dgvTarget[6, index].Value = "N/A";
                #endregion
            }
            else if (cbControlType.Text == CsConst.myPublicControlType[8].ControlTypeName)//面板控制
            {
                #region
                addcontrols(4, index, cbPanleControl, dgvTarget);
                #endregion
            }
            else if (cbControlType.Text == CsConst.myPublicControlType[10].ControlTypeName || //广播场景
                     cbControlType.Text == CsConst.myPublicControlType[11].ControlTypeName) //广播回路
            {
                #region
                if (cbControlType.Text == CsConst.myPublicControlType[10].ControlTypeName)
                {
                    dgvTarget[4, index].Value = CsConst.WholeTextsList[2566].sDisplayName;
                    dgvTarget[6, index].Value = "N/A";
                }
                else if (cbControlType.Text == CsConst.myPublicControlType[11].ControlTypeName)
                {
                    dgvTarget[4, index].Value = CsConst.WholeTextsList[2567].sDisplayName;
                    if (!str3.Contains(":"))
                        txtSeries.Text = "0:0";
                    else
                        txtSeries.Text = HDLPF.GetTimeFromString(str3, ':');
                    addcontrols(6, index, txtSeries, dgvTarget);
                }
                txtbox2.Text = str2;
                addcontrols(5, index, txtbox2, dgvTarget);
                #endregion
            }
            else if (cbControlType.Text == CsConst.myPublicControlType[13].ControlTypeName)//音乐播放
            {
                #region
                addcontrols(4, index, cbAudioControl, dgvTarget);
                #endregion
            }
            dgvTarget[3, index].Value = cbControlType.Text;
            #region
            if (cbbox1.Visible && cbbox1.Items.Count > 0)
            {
                if (!cbbox1.Items.Contains(str1))
                    cbbox1.SelectedIndex = 0;
                else
                    cbbox1.Text = str1;
            }
            if (cbbox2.Visible && cbbox2.Items.Count > 0)
            {
                if (!cbbox2.Items.Contains(str2))
                    cbbox2.SelectedIndex = 0;
                else
                    cbbox2.Text = str2;
            }
            if (cbbox3.Visible && cbbox3.Items.Count > 0)
            {
                if (!cbbox3.Items.Contains(str3))
                    cbbox3.SelectedIndex = 0;
                else
                    cbbox3.Text = str3;
            }
            if (cbPanleControl.Visible && cbPanleControl.Items.Count > 0)
            {
                if (!cbPanleControl.Items.Contains(str1))
                    cbPanleControl.SelectedIndex = 0;
                else
                    cbPanleControl.Text = str1;
            }
            if (cbAudioControl.Visible && cbAudioControl.Items.Count > 0)
            {
                if (!cbAudioControl.Items.Contains(str1))
                    cbAudioControl.SelectedIndex = 0;
                else
                    cbAudioControl.Text = str1;
            }
            #endregion
            #region
            if (txtbox1.Visible) txtbox1_TextChanged(null, null);
            if (txtbox2.Visible) txtbox2_TextChanged(null, null);
            if (txtSeries.Visible) txtSeries_TextChanged(null, null);
            if (cbbox1.Visible) cbbox1_SelectedIndexChanged(null, null);
            if (cbbox2.Visible) cbbox2_SelectedIndexChanged(null, null);
            if (cbbox3.Visible) cbbox3_SelectedIndexChanged(null, null);
            if (cbPanleControl.Visible) cbPanleControl_SelectedIndexChanged(null, null);
            if (cbAudioControl.Visible) cbAudioControl_SelectedIndexChanged(null, null);
            #endregion
            if (cbControlType.Focused)
                ModifyMultilinesIfNeeds(dgvTarget[3, index].Value.ToString(), 3,dgvTarget);
        }

        private void setAllControlVisible(bool TF)
        {
            txtSub.Visible = TF;
            txtDev.Visible = TF;
            cbControlType.Visible = TF;
            cbbox1.Visible = TF;
            cbbox2.Visible = TF;
            cbbox3.Visible = TF;
            txtbox1.Visible = TF;
            txtbox2.Visible = TF;
            txtbox3.Visible = TF;
            txtSeries.Visible = TF;
            cbPanleControl.Visible = TF;
            cbAudioControl.Visible = TF;
        }

        private void frmMC_Load(object sender, EventArgs e)
        {

            this.Width = (Convert.ToInt32(SystemInformation.WorkingArea.Width * 0.9) > this.Width) ? Convert.ToInt32(SystemInformation.WorkingArea.Width * 0.9) : this.Width;
            this.Height = (Convert.ToInt32(SystemInformation.WorkingArea.Height * 0.9) > this.Height) ? Convert.ToInt32(SystemInformation.WorkingArea.Height * 0.9) : this.Height;
            this.Left = (SystemInformation.WorkingArea.Width - this.Width) / 2;
            this.Top = (SystemInformation.WorkingArea.Height - this.Height) / 2;
            toolStrip1.Visible = (CsConst.MyEditMode == 0);
            networkinfo = new NetworkInForm(SubNetID, DevID, mintDeviceType);
            groupBox5.Controls.Add(networkinfo);
            networkinfo.Dock = DockStyle.Fill;

            frmIRLibrary frm = new frmIRLibrary(this, mmc, mintDeviceType);
            frm.TopLevel = false;
            frm.SendToBack();
            panel6.Controls.Add(frm);
            frm.Dock = DockStyle.Fill;
            frm.Show();

            cbType.Items.Clear();
            cbType.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99836", ""));
            cbType.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99837", ""));
            cbType.SelectedIndex = 0;
            cbChannel.SelectedIndex = 0;

            HDLSysPF.setDataGridViewColumnsWidth(dgvCommand);
            HDLSysPF.setDataGridViewColumnsWidth(dgvTarget);
            HDLSysPF.setDataGridViewColumnsWidth(dgvUV);
            HDLSysPF.setDataGridViewColumnsWidth(dgvUVTargets);
            HDLSysPF.setDataGridViewColumnsWidth(DgvKey);
            HDLSysPF.setDataGridViewColumnsWidth(dgvLoad);
            HDLSysPF.setDataGridViewColumnsWidth(DgvSeries);
        }

        private void showRS232basiinfo(byte[] array)
        {
            showRSbasic(0);
            dry1.Text = mmc.strdry1;
            dry2.Text = mmc.strdry2;
            dry3.Text = mmc.strdry3;
            dry4.Text = mmc.strdry4;
            
            txtrelay1.Text = mmc.strrelay1;
            txtrelay2.Text = mmc.strrelay2;
        }

        private void tsbSave_Click(object sender, EventArgs e)
        {
            if (mmc == null) return;
            Cursor.Current = Cursors.WaitCursor;
            mmc.SaveMMCToDB();
            Cursor.Current = Cursors.Default;
        }

        private void tsbDefault_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            mmc.ReadDefaultInfo();
            Cursor.Current = Cursors.Default;
        }

        private void addRS2BUS(string strTmp,byte bytType)
        {
            MMC.RS2BUS temp = new MMC.RS2BUS();
            temp.BUSCMD = new List<UVCMD.ControlTargets>();
            temp.TmpRS232 = new MMC.RS232Param();

            temp.TmpRS232.RSCMD = strTmp;

            if (bytType == 0)
            {
            }
            else if (bytType == 1)
            {
            }
        }

        private void DisplayRS232BUSInfo(List<MMC.RS2BUS> array)
        {
            if (array == null) return;
            for (int i = 0; i < array.Count; i++)
            {
                MMC.RS2BUS temp = array[i];
                for (int j = 0; j < temp.BUSCMD.Count; j++)
                {
                    UVCMD.ControlTargets tmp = temp.BUSCMD[j];
                }
            }
        }
        private void dry1_TextChanged(object sender, EventArgs e)
        {
            mmc.strdry1 = dry1.Text.Trim();
        }

        private void dry2_TextChanged(object sender, EventArgs e)
        {
            mmc.strdry2 = dry2.Text.Trim();
        }

        private void dry3_TextChanged(object sender, EventArgs e)
        {
            mmc.strdry3 = dry3.Text.Trim();
        }

        private void dry4_TextChanged(object sender, EventArgs e)
        {
            mmc.strdry4 = dry4.Text.Trim();
        }

        private void txtrelay_TextChanged(object sender, EventArgs e)
        {
            mmc.strrelay1 = txtrelay1.Text.Trim();
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (dgvUVTargets.RowCount == 0) return;
            if (dgvUV.SelectedRows == null) return;
            if (dgvUV.CurrentRow == null) return;

            Cursor.Current = Cursors.WaitCursor;
            // 获取当前表格的目标类型
            myTmpCMDLists = new List<MMC.RS232Param>();
            GetCMDBufferFromGrid(dgvUVTargets);
            byte ID = Convert.ToByte(dgvUV[0, dgvUV.CurrentRow.Index].Value.ToString());
            mmc.myBUS2RS[ID - 1].RS232CMD = new List<MMC.RS232Param>();
            foreach (MMC.RS232Param oCMD in myTmpCMDLists)
            {
                mmc.myBUS2RS[ID - 1].RS232CMD.Add(oCMD);
            }
            
            Cursor.Current = Cursors.Default;
        }


        // 获取指令列表
        void GetCMDBufferFromGrid(DataGridView oDgv)
        {
            
        }

        private void showBUS2RStargets(List<MMC.BUS2RS> array)
        {
            if (array ==null ||  array.Count == 0) return;
            if (dgvUV.CurrentRow == null) return;

            for (int i = 0; i < array.Count; i++)
            {
                byte ID = Convert.ToByte(dgvUV[0, dgvUV.CurrentRow.Index].Value.ToString());
                MMC.BUS2RS temp = array[i];
                if (ID == temp.ID)
                {
                    dgvUVTargets.Rows.Clear();
                    if (temp.RS232CMD == null) return;
                    for (int j = 0; j < temp.RS232CMD.Count; j++)
                    {
                        MMC.RS232Param tmp = temp.RS232CMD[j];
                    }
                }
            }
        }

        private void txtNum_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar) || e.KeyChar==(Char)8)
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }


        private string getHexIRcodes(int DevID,string strCodes)
        {
            try
            {
                if (DevID != 6)
                {
                    strCodes = strCodes.Trim();
                    strCodes = strCodes.Substring(1, strCodes.Length - 4);
                    int intTmp = 0;
                    for (int i = 0; i < strCodes.Length; i++)
                    {
                        string strTmp = strCodes.Substring(i, 1);
                        if (strCodes == ",")
                        {
                            intTmp = intTmp + 1;
                        }
                    }
                    strCodes=strCodes.Replace("{", "");
                    strCodes=strCodes.Replace("}", "");
                    strCodes=strCodes.Replace(" ", "");
                    strCodes=strCodes.Replace("0x", "");

                }
                else
                {
                    strCodes = strCodes.Split('{')[2].ToString().Trim();
                    strCodes = strCodes.Substring(0, strCodes.Length - 3);
                    int intTmp = 0;
                    for (int i = 0; i < strCodes.Length; i++)
                    {
                        string strTmp = strCodes.Substring(i, 1);
                        if (strCodes == ",")
                        {
                            intTmp = intTmp + 1;
                        }
                    }
                    strCodes=strCodes.Replace("{", "");
                    strCodes=strCodes.Replace("}", "");
                    strCodes=strCodes.Replace(" ", "");
                    strCodes=strCodes.Replace("0x", "");

                }
                return strCodes;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return "";
            }
        }

        private void tvExisted_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (e.Action == TreeViewAction.ByMouse)
            {
                if (e.Node.Checked)
                {
                    //选中节点选中状态之后，选中父节点的选中状态
                    HDLPF.setChildNodeCheckedState(e.Node, true);
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

        void DisPlayIRcodesToForm()
        {
            try
            {
                isReading = true;
                if (mmc.IRCodes != null && mmc.IRCodes.Count > 0)
                {
                    DgvKey.Rows.Clear();
                    for (int i = 0; i < mmc.IRCodes.Count; i++)
                    {
                        MMC.NewIRCode ir = mmc.IRCodes[i];
                        string str = "N/A";
                        if (ir.DevID < 6)
                        {
                            str = CsConst.NewIRLibraryDeviceType[ir.DevID];
                        }
                        if (i < 4)
                        {
                            if (ir.DevID != 5) str = "N/A";
                        }
                        if (ir.IRLength == 0) str = "N/A";
                        object[] obj = new object[] { ir.KeyID, str, ir.Remark.ToString(), "Send" };
                        DgvKey.Rows.Add(obj);
                    }
                }
            }
            catch
            {
            }
            isReading = false;
        }

        private void addcontrols(int col, int row, Control con,DataGridView dgv)
        {
            con.Show();
            con.Visible = true;
            Rectangle rect = dgv.GetCellDisplayRectangle(col, row, true);
            con.Size = rect.Size;
            con.Top = rect.Top;
            con.Left = rect.Left;
        }

        private void frmMC_Shown(object sender, EventArgs e)
        {
            if (CsConst.MyEditMode == 0)
            {
                DisPlayIRcodesToForm();
            }
            else if (CsConst.MyEditMode == 1)
            {
                cbRS_SelectedIndexChanged(null, null);
                btnref2_Click(null, null);
            }
        }

        private void tslRead_Click(object sender, EventArgs e)
        {
            try
            {
                byte bytTag = Convert.ToByte(((ToolStripButton)sender).Tag.ToString());
                bool blnShowMsg = (CsConst.MyEditMode != 1);
                if (HDLPF.UploadOrReadGoOn(this.Text, bytTag, blnShowMsg))
                {
                    Cursor.Current = Cursors.WaitCursor;

                    NumSubID_ValueChanged(NumSubID, null);

                    CsConst.MyUPload2DownLists = new List<byte[]>();

                    string strName = mystrName.Split('\\')[0].ToString();
                    byte bytSubID = byte.Parse(strName.Split('-')[0]);
                    byte bytDevID = byte.Parse(strName.Split('-')[1]);

                    byte[] ArayRelay = new byte[] { bytSubID, bytDevID, (byte)(mintDeviceType / 256), (byte)(mintDeviceType % 256)
                        , (byte)MyActivePage,(byte)(mintIDIndex/256),(byte)(mintIDIndex%256) };

                    CsConst.MyUPload2DownLists.Add(ArayRelay);
                    CsConst.MyUpload2Down = Convert.ToByte(((ToolStripButton)sender).Tag.ToString());
                    FrmDownloadShow Frm = new FrmDownloadShow();
                    if (CsConst.MyUpload2Down == 0) Frm.FormClosing += new FormClosingEventHandler(UpdateDisplayInformationAccordingly);
                    Frm.ShowDialog();
                }
            }
            catch
            {
            }
        }

        void UpdateDisplayInformationAccordingly(object sender, FormClosingEventArgs e)
        {
            switch (tab.SelectedIndex)
            {
                case 2: DisPlayIRcodesToForm(); break;
                case 3: showCurrentofload(); break;
            }
            this.TopMost = true;
            this.WindowState = FormWindowState.Normal;
            this.Activate();
            this.TopMost = false;
        }

        private void showCurrentofload()
        {
            try
            {
                LoadCtrsVisible(false);
                if (mmc == null) return;
                if (mmc.Reserved == null) return;
                if (mmc.Reserved.Length < 50) return;
                dgvLoad.Rows.Clear();
                for (int i = 0; i < 5; i++)
                {
                    int Currentofload = mmc.Reserved[10 * i + 1] * 256 + mmc.Reserved[10 * i + 2];
                    int Compenvalue = mmc.Reserved[10 * i + 3] - 128;
                    object[] obj = new object[] { dgvLoad.RowCount + 1, mmc.Reserved[10 * i + 0].ToString(), Currentofload.ToString(),
                                             Compenvalue.ToString()};
                    dgvLoad.Rows.Add(obj);
                }
            }
            catch
            {
            }
        }

        private void NumSubID_ValueChanged(object sender, EventArgs e)
        {
            if (NumSubID.Value.ToString() == null) return;
            if (NumDevID.Value.ToString() == null) return;
            if (tbRemark.Text == null) tbRemark.Text = "";

            byte bytSubID = byte.Parse(NumSubID.Value.ToString());
            byte bytDevID = byte.Parse(NumDevID.Value.ToString());
            mystrName = bytSubID.ToString() + "-" + bytDevID.ToString() + @"\" + tbRemark.Text;
            SubNetID = bytSubID;
            DevID = bytDevID;

            mmc.strName = mystrName;
            tsl3.Text = mystrName;
            this.Text = mystrName;
        }

        private void rb1_CheckedChanged(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            isReading = true;
            DgvSeries.Rows.Clear();
            byte[] ArayTmp = new byte[1];
            byte ZoneID = Convert.ToByte((sender as RadioButton).Tag);
            ArayTmp[0] = 0x0B;
            if (CsConst.MyEditMode == 1)
            {
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1372, SubNetID, DevID, false, true, true,false) == true)
                {
                    chbEnable.Checked = (CsConst.myRevBuf[26 + ZoneID-1]==1);
                    CsConst.myRevBuf = new byte[1200];
                    cbSeq_SelectedIndexChanged(null, null);
                }
            }
            Cursor.Current = Cursors.Default;
            isReading = false;
        }

        private void chbEnable_CheckedChanged(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            byte[] ArayTmp = new byte[9];
            byte ZoneID = 1;
            ArayTmp[0] = 0x0B;
            for (int i = 0; i < 1; i++)
            {
                RadioButton temp = this.Controls.Find("rb" + (i + 1).ToString(), true)[0] as RadioButton;
                if (temp.Checked)
                {
                     ZoneID = Convert.ToByte(temp.Tag);
                     break;
                }
            }
            if (CsConst.MyEditMode == 1)
            {
                if (!isReading)
                {
                    if (chbEnable.Checked)
                        ArayTmp[ZoneID] = 1;
                    else
                        ArayTmp[ZoneID] = 0;
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1370, SubNetID, DevID, false, true, true, false) == true)
                    {
                        CsConst.myRevBuf = new byte[1200];
                    }
                }
            }
            Cursor.Current = Cursors.Default;
        }

        private void showSeq(int ZoneID,int SeqID)
        {
            if (mmc.mySeries == null) return;
            if (mmc.mySeries[ZoneID - 1].steps == null) return;
            if (mmc.mySeries.Count <= 0) return;
            if (mmc.mySeries[ZoneID-1].steps.Length == 0) return;

            for (int i = 0; i < 32; i++)
            {
                byte[] temp = new byte[10];
                Array.Copy(mmc.mySeries[ZoneID - 1].steps, (SeqID -1) * 320 + i * 10, temp, 0, 10);
                string strType="";
                string str1="",str2="",str3="",str4="";
                int intTime = temp[6] * 256 + temp[7];
                intTime = intTime % 3600;
                string strDelay = Convert.ToString(intTime / 60) + ":" + Convert.ToString(intTime % 60);
                if (temp[1] == 0)///IR
                {
                    str1 = temp[2].ToString();
                    str2 = temp[3].ToString();
                    str3 = temp[4].ToString();
                    if (temp[5] >= 2) temp[5] = 0;
                    str4 = cbbox2.Items[temp[5]].ToString(); ;
                }
                else if (temp[1] == 1)///串口
                {
                    str1 = temp[2].ToString();
                    if (temp[3] >= 2) temp[3] = 0;
                    str2 = cbbox1.Items[temp[3]].ToString();
                    str3 = str4 = "N/A";
                }
                else if (temp[1] == 2)//继电器
                {
                    str1 = temp[2].ToString();
                    if (temp[3] >= 2) temp[3] = 0;
                    str2 = cbbox1.Items[temp[3]].ToString();
                    str3 = str4 = "N/A";
                }
                else if (temp[1] == 3)//干接点
                {
                    str1 = temp[2].ToString();
                    if (temp[3] > 2) temp[3] = 0;
                    str2 = cbbox1.Items[temp[3]].ToString();
                    str3 = str4 = "N/A";
                }
                else//无效
                {
                    str1 = str2 = str3 = str4 = "N/A";
                }
                object[] obj = new object[] { DgvSeries.RowCount + 1, strType,str1,str2,str3,str4,strDelay};
                DgvSeries.Rows.Add(obj);
            }
        }

        private void tmUpload_Click(object sender, EventArgs e)
        {
            CsConst.myUploadDownManually = true;
            MyActivePage = tab.SelectedIndex + 1;

            tslRead_Click(tbUpload, null);
            Cursor.Current = Cursors.Default;
            CsConst.myUploadDownManually = false;
        }

        private void btnref1_Click(object sender, EventArgs e)
        {
            if (cbRS.SelectedIndex < 0 && cbRS.Items.Count > 0) cbRS.SelectedIndex = 0;
            if (cbRS.SelectedIndex < 0) return;
            byte[] ArayTmp = new byte[2];
            ArayTmp[0] = 0x01;
            ArayTmp[1] =Convert.ToByte(cbRS.SelectedIndex);
            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1372, SubNetID, DevID, false, true, true, false) == true)
            {
                if (cbRS.SelectedIndex == 0)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        mmc.ArayCom1[i] = CsConst.myRevBuf[27+i];
                    }
                }
                else if (cbRS.SelectedIndex == 1)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        mmc.ArayCom2[i] = CsConst.myRevBuf[27+i];
                    }
                }
                else if (cbRS.SelectedIndex == 2)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        mmc.ArayCom3[i] = CsConst.myRevBuf[27+i];
                    }
                }
                else
                {
                    for (int i = 0; i < 4; i++)
                    {
                        mmc.ArayCom4[i] = CsConst.myRevBuf[27+i];
                    }
                }
                CsConst.myRevBuf = new byte[1200];
                showRSbasic(cbRS.SelectedIndex);
            }
        }

        private void showRSbasic(int RSIndex)
        {
            cbb232BR.SelectedIndex = -1;
            cbb232SP.SelectedIndex = -1;
            cbb485BR.SelectedIndex = -1;
            cbb485SP.SelectedIndex = -1;
            if (RSIndex == 0)
            {
                if (mmc.ArayCom1[0] > 10 || mmc.ArayCom1[0] == 0) mmc.ArayCom1[0] = 5;
                cbb232BR.SelectedIndex = mmc.ArayCom1[0] - 1;
                if (mmc.ArayCom1[1] > 3) mmc.ArayCom1[1] = 0;
                cbb232SP.SelectedIndex = mmc.ArayCom1[1];
                if (mmc.ArayCom1[2] < 2)
                    cbb485BR.SelectedIndex = mmc.ArayCom1[2];
                if (mmc.ArayCom1[3] < 2)
                    cbb485SP.SelectedIndex = mmc.ArayCom1[3];
            }
            else if (RSIndex == 1)
            {
                if (mmc.ArayCom2[0] > 10 || mmc.ArayCom2[0] == 0) mmc.ArayCom2[0] = 5;
                    cbb232BR.SelectedIndex = mmc.ArayCom2[0] - 1;
                    if (mmc.ArayCom2[1] < 3) mmc.ArayCom2[1] = 0;
                    cbb232SP.SelectedIndex = mmc.ArayCom2[1];
                if (mmc.ArayCom2[2] < 2)
                    cbb485BR.SelectedIndex = mmc.ArayCom2[2];
                if (mmc.ArayCom2[3] < 2)
                    cbb485SP.SelectedIndex = mmc.ArayCom2[3];
            }
            else if (RSIndex == 2)
            {
                if (mmc.ArayCom3[0] > 10 || mmc.ArayCom3[0] == 0) mmc.ArayCom3[0] = 5;
                    cbb232BR.SelectedIndex = mmc.ArayCom3[0] - 1;
                    if (mmc.ArayCom3[1] < 3) mmc.ArayCom3[1] = 0;
                    cbb232SP.SelectedIndex = mmc.ArayCom3[1];
                if (mmc.ArayCom3[2] < 2)
                    cbb485BR.SelectedIndex = mmc.ArayCom3[2];
                if (mmc.ArayCom3[3] < 2)
                    cbb485SP.SelectedIndex = mmc.ArayCom3[3];
            }
            else
            {
                if (mmc.ArayCom4[0] > 10 || mmc.ArayCom4[0] == 0) mmc.ArayCom4[0] = 5;
                    cbb232BR.SelectedIndex = mmc.ArayCom4[0] - 1;
                if (mmc.ArayCom4[1] < 3) mmc.ArayCom4[1] = 0;
                    cbb232SP.SelectedIndex = mmc.ArayCom4[1];
                if (mmc.ArayCom4[2] < 2)
                    cbb485BR.SelectedIndex = mmc.ArayCom4[2];
                if (mmc.ArayCom4[3] < 2)
                    cbb485SP.SelectedIndex = mmc.ArayCom4[3];
            }
        }

        private void btnsave1_Click(object sender, EventArgs e)
        {
            if (cbRS.SelectedIndex < 0) return;
            byte[] ArayTmp = new byte[6];
            ArayTmp[0] = 0x01;
            ArayTmp[1] =Convert.ToByte(cbRS.SelectedIndex);
            if (cbb232BR.SelectedIndex >= 0) ArayTmp[2] = Convert.ToByte(cbb232BR.SelectedIndex + 1);
            if (cbb232SP.SelectedIndex >= 0) ArayTmp[3] = Convert.ToByte(cbb232SP.SelectedIndex);
            if (cbb485BR.SelectedIndex >= 0) ArayTmp[4] = Convert.ToByte(cbb485BR.SelectedIndex);
            if (cbb485SP.SelectedIndex >= 0) ArayTmp[5] = Convert.ToByte(cbb485SP.SelectedIndex);
            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1370, SubNetID, DevID, false, true, true, false) == true)
            {
                if (cbRS.SelectedIndex == 0)
                {
                    mmc.ArayCom1[0] = ArayTmp[2];
                    mmc.ArayCom1[1] = ArayTmp[3];
                    mmc.ArayCom1[2] = ArayTmp[4];
                    mmc.ArayCom1[3] = ArayTmp[5];
                }
                else if (cbRS.SelectedIndex == 1)
                {
                    mmc.ArayCom2[0] = ArayTmp[2];
                    mmc.ArayCom2[1] = ArayTmp[3];
                    mmc.ArayCom2[2] = ArayTmp[4];
                    mmc.ArayCom2[3] = ArayTmp[5];
                }
                else if (cbRS.SelectedIndex == 2)
                {
                    mmc.ArayCom3[0] = ArayTmp[2];
                    mmc.ArayCom3[1] = ArayTmp[3];
                    mmc.ArayCom3[2] = ArayTmp[4];
                    mmc.ArayCom3[3] = ArayTmp[5];
                }
                else
                {
                    mmc.ArayCom4[0] = ArayTmp[2];
                    mmc.ArayCom4[1] = ArayTmp[3];
                    mmc.ArayCom4[2] = ArayTmp[4];
                    mmc.ArayCom4[3] = ArayTmp[5];
                }
            }
        }

        private void cbRS_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CsConst.MyEditMode == 1)
            {
                btnref1_Click(null, null);
            }
        }

        private void btnref2_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            byte[] ArayTmp = new byte[2];
            ArayTmp[0] = 0x07;
            for (int i = 1; i <= 4; i++)
            {
                ArayTmp[1] = Convert.ToByte(i);
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1372, SubNetID, DevID, false, true, true, false) == true)
                {
                    byte[] arayRemark = new byte[20];
                    HDLSysPF.CopyRemarkBufferFrmMyRevBuffer(CsConst.myRevBuf, arayRemark, 27);
                    string strRemark = HDLPF.Byte2String(arayRemark);
                    TextBox temp = this.Controls.Find("dry" + (i).ToString(), true)[0] as TextBox;
                    temp.Text = strRemark;

                }
                else
                {
                    break;
                }
            }
            ArayTmp[0] = 0x08;
            for (int i = 1; i <= 1; i++)
            {
                ArayTmp[1] = Convert.ToByte(i);
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1372, SubNetID, DevID, false, true, true, false) == true)
                {
                    byte[] arayRemark = new byte[20];
                    for (int intI = 0; intI < 20; intI++) { arayRemark[intI] = CsConst.myRevBuf[27 + intI]; }
                    string strRemark = HDLPF.Byte2String(arayRemark);
                    TextBox temp = this.Controls.Find("txtrelay" + (i).ToString(), true)[0] as TextBox;
                    Text = strRemark;
                }
                else
                {
                    break;
                }
            }
            Cursor.Current = Cursors.Default;
        }

        private void btnsave2_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            byte[] ArayTmp = new byte[22];
            ArayTmp[0] = 0x07;
            for (int i = 1; i <= 4; i++)
            {
                ArayTmp[1] = Convert.ToByte(i);
                TextBox temp = this.Controls.Find("dry" + (i).ToString(), true)[0] as TextBox;
                string str = temp.Text;
                byte[] arayTmpRemark = HDLUDP.StringToByte(str);
                HDLSysPF.CopyRemarkBufferToSendBuffer(arayTmpRemark, ArayTmp, 2);
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1370, SubNetID, DevID, false, true, true, false) == true)
                {

                }
                else
                {
                    break;
                }
            }
            ArayTmp[0] = 0x08;
            for (int i = 1; i <= 2; i++)
            {
                ArayTmp[1] = Convert.ToByte(i);
                TextBox temp = this.Controls.Find("txtrelay" + (i).ToString(), true)[0] as TextBox;
                string str = Text;
                byte[] arayTmpRemark = HDLUDP.StringToByte(str);
                if (arayTmpRemark.Length > 20)
                {
                    Array.Copy(arayTmpRemark, 0, ArayTmp, 2, 20);
                }
                else
                {
                    Array.Copy(arayTmpRemark, 0, ArayTmp, 2, arayTmpRemark.Length);
                }
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1370, SubNetID, DevID, false, true, true, false) == true)
                {
                    
                }
                else
                {
                    break;
                }
            }
            Cursor.Current = Cursors.Default;
        }

        private void tmRead_Click(object sender, EventArgs e)
        {
            CsConst.myUploadDownManually = true;
            if (tab.SelectedIndex == 2 || tab.SelectedIndex == 3)
            {
                MyActivePage = tab.SelectedIndex + 1;
                tslRead_Click(tslRead, null);
            }
            else if (tab.SelectedIndex == 0)
            {
                btnSure_Click(null, null);
            }
            else if (tab.SelectedIndex == 1)
            {
                btnReadUV_Click(null, null);
            }
            else if (tab.SelectedIndex == 4)
            {
                for (int j = 1; j <= 8; j++)
                {
                    RadioButton temp = this.Controls.Find("rb" + j.ToString(), true)[0] as RadioButton;
                    if (temp.Checked)
                    {
                        rb1_CheckedChanged(temp, null);
                        break;
                    }
                }
            }
            CsConst.myUploadDownManually = false;
        }

        private void tab_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CsConst.MyEditMode == 0)
            {
               
            }
            else if (CsConst.MyEditMode == 1)
            {
                MyActivePage = tab.SelectedIndex + 1;
                if (tab.SelectedIndex == 2 || tab.SelectedIndex == 3)
                {
                    if (mmc.MyRead2UpFlags[MyActivePage - 1] == false)
                    {
                        tslRead_Click(tslRead, null);
                    }
                }
                else if (tab.SelectedIndex == 4)
                {
                    for (int j = 1; j <= 8; j++)
                    {
                        RadioButton temp = this.Controls.Find("rb" + j.ToString(), true)[0] as RadioButton;
                        if (temp.Checked)
                        {
                            rb1_CheckedChanged(temp, null);
                        }
                    }
                }
            }
        }

        private void txtrelay2_TextChanged(object sender, EventArgs e)
        {
            mmc.strrelay2 = txtrelay2.Text.Trim();
        }

        private void DgvKey_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            if (isReading) return;
            if (e.RowIndex == -1) return;
            if (e == null) return;
            if (mmc.IRCodes == null) return;

            mmc.IRCodes.RemoveAt(e.RowIndex);
        }

        private void dgvTargetBUSRS232_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            dgvUVTargets.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void DgvKey_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (DgvKey.RowCount == 0) return;
            if (e.ColumnIndex != 3) return;

            string strName = mystrName.Split('\\')[0].ToString();
            byte bytSubID = byte.Parse(strName.Split('-')[0]);
            byte bytDevID = byte.Parse(strName.Split('-')[1]);

            byte[] ArayTmp = new byte[3];
            ArayTmp[0] = 1;
            ArayTmp[1] = Convert.ToByte(mmc.IRCodes[e.RowIndex].KeyID);
            ArayTmp[2] = 1;
            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x0000, bytSubID, bytDevID, false, true, true, false) == true)
            {
                CsConst.myRevBuf = new byte[1200];
            }
        }

        private void btnTestSeq_Click(object sender, EventArgs e)
        {
            
        }

        private void dgvTargetBUSRS232_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control == true && e.KeyCode == Keys.V)
            {
                String strText = Clipboard.GetText();
                if (strText != null)
                {
                    // get first line 
                    byte CurrentPasteLine = 0;
                    if (dgvUVTargets.SelectedRows != null && dgvUVTargets.SelectedRows.Count !=0) 
                        CurrentPasteLine = Convert.ToByte(dgvUVTargets.SelectedRows[dgvUVTargets.SelectedRows.Count - 1].Index);

                    string[] ArayLines = strText.Split(new string[] {"\r\n"},StringSplitOptions.None);
                    if (ArayLines != null && ArayLines.Length != 0)
                    {
                        for (int i = 0; i < ArayLines.Length; i++)
                        {
                            string[] TmpArayString = ArayLines[i].Split('\t').ToArray();
                            if (TmpArayString != null && TmpArayString.Length != 0)
                            {
                                List<string> Tmp = TmpArayString.ToList();
                                Tmp.Insert(1, "100ms");
                                TmpArayString = Tmp.ToArray();
                                if (i + CurrentPasteLine >= dgvUVTargets.RowCount)
                                {
                                    TmpArayString[0] = (i + CurrentPasteLine + 1).ToString();
                                    dgvUVTargets.Rows.Add(TmpArayString);
                                }
                                else // in selection
                                {
                                    TmpArayString[0] = dgvUVTargets.Rows[i + CurrentPasteLine].Cells[0].Value.ToString();
                                    dgvUVTargets.Rows[i + CurrentPasteLine].SetValues(TmpArayString);
                                }
                            }
                        }
                    }
                }
            }
        }

        private void btnSure_Click(object sender, EventArgs e)
        {
            setAllControlVisible(false);
            Cursor.Current = Cursors.WaitCursor;
            dgvCommand.Rows.Clear();
            btnSure.Enabled = false;
            byte[] arayTmp = new byte[2];
            byte bytFrm = Convert.ToByte(Convert.ToInt32(txtFrm.Text));
            byte bytTo = Convert.ToByte(txtTo.Text);
            for (byte byt = bytFrm; byt <= bytTo; byt++)
            {
                arayTmp[0] = 0x04;
                arayTmp[1] = byt;
                //读取RS2BUS信息
                if (CsConst.mySends.AddBufToSndList(arayTmp, 0x1372, SubNetID, DevID, false, true, true, false) == true)
                {
                    int Length = CsConst.myRevBuf[27];
                    if (Length > 32) Length = 32;
                    int RSNO = 0;
                    if (CsConst.myRevBuf[28] <= 3) RSNO = CsConst.myRevBuf[28];
                    int type = CsConst.myRevBuf[29];
                    string strCommand="";
                    string strType = "";
                    string strRemark = "";
                    
                    if (type == 1)
                    {
                        byte[] arayCommand = new byte[Length];
                        Array.Copy(CsConst.myRevBuf, 30, arayCommand, 0, Length);
                        strCommand = HDLPF.Byte2String(arayCommand);
                        strType = CsConst.mstrINIDefault.IniReadValue("Public", "99838", "");
                    }
                    else if (type == 2)
                    {
                        for (int i = 0; i < Length; i++) strCommand = strCommand + GlobalClass.AddLeftZero(CsConst.myRevBuf[30 + i].ToString("X"), 2) + " ";
                        strCommand = strCommand.Trim();
                        strType = CsConst.mstrINIDefault.IniReadValue("Public", "99839", "");
                    }
                    else
                    {
                        strType = CsConst.WholeTextsList[1775].sDisplayName;
                    }
                    if (Length == 0) strType = CsConst.WholeTextsList[1775].sDisplayName;
                    arayTmp[0] = 0x05;
                    arayTmp[1] = byt;
                    CsConst.myRevBuf = new byte[1200];
                    if (CsConst.mySends.AddBufToSndList(arayTmp, 0x1372, SubNetID, DevID, false, true, true, false) == true)
                    {
                        byte[] arayRemark = new byte[20];
                        for (int intJ = 0; intJ < 20; intJ++) { arayRemark[intJ] = CsConst.myRevBuf[27 + intJ]; }
                        strRemark = HDLPF.Byte2String(arayRemark);
                        CsConst.myRevBuf = new byte[1200];
                        System.Threading.Thread.Sleep(1);
                    }
                    else break;
                    
                    object[] obj = new object[] { byt.ToString(), strRemark, (RSNO + 1).ToString(), strType, strCommand };
                    dgvCommand.Rows.Add(obj);
                }
                else break;
            }
            btnSure.Enabled = true;
            Cursor.Current = Cursors.Default;
            DataGridViewCellEventArgs even = new DataGridViewCellEventArgs(0, 0);
            dgvCommand_CellClick(null, even);
            setAllControlVisible(false);
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
            txtFrm.Text = HDLPF.IsNumStringMode(str, 1, num);
            txtFrm.SelectionStart = txtFrm.Text.Length;
        }

        private void txtTo_TextChanged(object sender, EventArgs e)
        {
                string str = txtTo.Text;
                int num = Convert.ToInt32(txtFrm.Text);
                txtTo.Text = HDLPF.IsNumStringMode(str, num, 200);
                txtTo.SelectionStart = txtTo.Text.Length;
        }

        private void dgvCommand_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                int id = Convert.ToInt32(dgvCommand[0, e.RowIndex].Value.ToString());
                readCommandTargets(id);
            }
        }

        private void readCommandTargets(int ID)
        {
            txtSub.Visible = false;
            txtDev.Visible = false;
            cbControlType.Visible = false;
            cbbox1.Visible = false;
            cbbox2.Visible = false;
            cbbox3.Visible = false;
            txtbox1.Visible = false;
            txtbox2.Visible = false;
            txtbox3.Visible = false;
            txtSeries.Visible = false;
            cbPanleControl.Visible = false;
            cbAudioControl.Visible = false;
            Cursor.Current = Cursors.WaitCursor;
            dgvTarget.Rows.Clear();
            byte[] ArayTmp = new byte[3];
            ArayTmp[0] = 0x06;
            ArayTmp[1] = Convert.ToByte(ID);
            for (byte i = 1; i <= 8; i++)
            {
                ArayTmp[2] = i;
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1372, SubNetID, DevID, false, true, true, false) == true)
                {
                    string strType = "";
                    strType = DryControlType.ConvertorKeyModeToPublicModeGroup(CsConst.myRevBuf[28]);
                    string strParam1 = "0", strParam2 = "0", strParam3 = "0", strParam4 = "0";
                    strParam1 = CsConst.myRevBuf[31].ToString();
                    strParam2 = CsConst.myRevBuf[32].ToString();
                    strParam3 = CsConst.myRevBuf[33].ToString();
                    strParam4 = CsConst.myRevBuf[34].ToString();
                    SetParams(ref strType, ref strParam1, ref strParam2, ref strParam3, strParam4);
                    object[] obj = new object[] { i.ToString(),CsConst.myRevBuf[29].ToString(),CsConst.myRevBuf[30].ToString(),strType
                                ,strParam1,strParam2,strParam3,strParam4};
                    dgvTarget.Rows.Add(obj);
                    CsConst.myRevBuf = new byte[1200];
                    System.Threading.Thread.Sleep(100);
                }
                else break;
            }
            Cursor.Current = Cursors.Default;
        }

        private void SetParams(ref string strType, ref string str1, ref string str2, ref string str3, string str4)
        {
            if (strType == CsConst.mstrINIDefault.IniReadValue("TargetType", "00000", ""))//无效
            {
                #region
                str1 = "N/A";
                str2 = "N/A";
                str3 = "N/A";
                #endregion
            }
            else if (strType == CsConst.myPublicControlType[1].ControlTypeName)//场景
            {
                #region
                if (str1 == "255")
                {
                    strType = CsConst.WholeTextsList[1777].sDisplayName;
                    str1 = CsConst.WholeTextsList[2566].sDisplayName;
                    str2 = str2 + "(" + CsConst.WholeTextsList[2511].sDisplayName + ")";
                    str3 = "N/A";
                }
                else
                {
                    str1 = str1 + "(" + CsConst.WholeTextsList[2510].sDisplayName + ")";
                    str2 = str2 + "(" + CsConst.WholeTextsList[2511].sDisplayName + ")";
                    str3 = "N/A";
                }
                #endregion
            }
            else if (strType == CsConst.myPublicControlType[2].ControlTypeName)//序列
            {
                #region
                str1 = str1 + "(" + CsConst.WholeTextsList[2510].sDisplayName + ")";
                str2 = str2 + "(" + CsConst.WholeTextsList[2512].sDisplayName+ ")";
                str3 = "N/A";
                #endregion
            }
            else if (strType == CsConst.myPublicControlType[4].ControlTypeName)//通用开关
            {
                #region
                str1 = str1 + "(" + CsConst.WholeTextsList[2513].sDisplayName + ")";
                if (str2 == "0") str2 = CsConst.Status[0] + "(" + CsConst.WholeTextsList[2529].sDisplayName + ")";
                else if (str2 == "255") str2 = CsConst.Status[1] + "(" + CsConst.WholeTextsList[2529].sDisplayName + ")";
                str3 = "N/A";
                #endregion
            }
            else if (strType == CsConst.myPublicControlType[5].ControlTypeName)//单路调节
            {
                #region
                if (str1 == "255")
                {
                    strType =  CsConst.myPublicControlType[11].ControlTypeName;
                    str1 = CsConst.WholeTextsList[2567].sDisplayName;
                    str2 = str2 + "(" + CsConst.WholeTextsList[2524].sDisplayName + ")";
                    int intTmp = Convert.ToInt32(str3) * 256 + Convert.ToInt32(str4);
                    str3 = HDLPF.GetStringFromTime(intTmp, ":") + "(" + CsConst.WholeTextsList[2525].sDisplayName + ")";
                }
                else
                {
                    str1 = str1 + "(" + CsConst.WholeTextsList[934].sDisplayName + ")";
                    str2 = str2 + "(" + CsConst.WholeTextsList[2524].sDisplayName + ")";
                    int intTmp = Convert.ToInt32(str3) * 256 + Convert.ToInt32(str4);
                    str3 = HDLPF.GetStringFromTime(intTmp, ":") + "(" + CsConst.WholeTextsList[2525].sDisplayName + ")";
                }
                #endregion
            }
            else if (strType == CsConst.myPublicControlType[10].ControlTypeName)//广播场景
            {
                #region
                str1 = CsConst.WholeTextsList[2566].sDisplayName;
                str2 = str2 + "(" + CsConst.WholeTextsList[2511].sDisplayName + ")";
                str3 = "N/A";
                #endregion
            }
            else if (strType == CsConst.myPublicControlType[11].ControlTypeName)//广播回路
            {
                #region
                str1 = CsConst.WholeTextsList[2567].sDisplayName;
                str2 = str2 + "(" + CsConst.WholeTextsList[2524].sDisplayName + ")";
                int intTmp = Convert.ToInt32(str3) * 256 + Convert.ToInt32(str4);
                str3 = HDLPF.GetStringFromTime(intTmp, ":") + "(" + CsConst.WholeTextsList[2525].sDisplayName + ")";
                #endregion
            }
            else if (strType == CsConst.myPublicControlType[6].ControlTypeName)//窗帘开关
            {
                #region
                if (Convert.ToInt32(str1) >= 17 && Convert.ToInt32(str1) <= 34)
                {
                    str2 = (Convert.ToInt32(str2)).ToString() + "%" + "(" + CsConst.WholeTextsList[2529].sDisplayName + ")";
                    str1 = (Convert.ToInt32(str1) - 16).ToString() + "(" + CsConst.mstrINIDefault.IniReadValue("public", "99844", "") + ")";
                }
                else
                {
                    if (str2 == "0") str2 = CsConst.mstrINIDefault.IniReadValue("public", "00036", "") + "(" + CsConst.WholeTextsList[2529].sDisplayName + ")";
                    else if (str2 == "1") str2 = CsConst.mstrINIDefault.IniReadValue("public", "00037", "") + "(" + CsConst.WholeTextsList[2529].sDisplayName + ")";
                    else if (str2 == "2") str2 = CsConst.mstrINIDefault.IniReadValue("public", "00038", "") + "(" + CsConst.WholeTextsList[2529].sDisplayName + ")";
                    else str2 = (Convert.ToInt32(str2)).ToString() + "%" + "(" + CsConst.WholeTextsList[2529].sDisplayName + ")";
                    str1 = str1 + "(" + CsConst.mstrINIDefault.IniReadValue("public", "99844", "") + ")";
                }
                str3 = "N/A";
                #endregion
            }
            else if (strType == CsConst.myPublicControlType[7].ControlTypeName)//GPRS
            {
                #region
                if (str1 == "1") str1 = CsConst.mstrINIDefault.IniReadValue("public", "99862", "");
                else if (str1 == "2") str1 = CsConst.mstrINIDefault.IniReadValue("public", "99863", "");
                else str1 = CsConst.WholeTextsList[1775].sDisplayName;
                str2 = str2 + "(" + CsConst.mstrINIDefault.IniReadValue("public", "99864", "") + ")";
                str3 = "N/A";
                #endregion
            }
            else if (strType == CsConst.myPublicControlType[8].ControlTypeName)//面板控制
            {
                #region
                str1 = HDLSysPF.InquirePanelControTypeStringFromDB(Convert.ToInt32(str1));
                if (str1 == CsConst.myPublicControlType[0].ControlTypeName)
                {
                    str2 = "N/A";
                    str3 = "N/A";
                }
                else if (str1 == CsConst.myPublicControlType[1].ControlTypeName ||
                         str1 == CsConst.myPublicControlType[2].ControlTypeName ||
                         str1 == CsConst.myPublicControlType[5].ControlTypeName ||
                         str1 == CsConst.myPublicControlType[7].ControlTypeName ||
                         str1 == CsConst.myPublicControlType[8].ControlTypeName ||
                         str1 ==  CsConst.PanelControl[12] ||
                         str1 == CsConst.PanelControl[21])
                {
                    if (str2 == "0") str2 = CsConst.Status[0] + "(" + CsConst.WholeTextsList[2529].sDisplayName + ")";
                    else if (str2 == "1") str2 = CsConst.Status[1] + "(" + CsConst.WholeTextsList[2529].sDisplayName + ")";
                    str3 = "N/A";
                }
                else if (str1 == CsConst.myPublicControlType[3].ControlTypeName ||
                         str1 == CsConst.myPublicControlType[4].ControlTypeName)
                {
                    str2 = str2 + "(" + CsConst.WholeTextsList[2524].sDisplayName + ")";
                    str3 = "N/A";
                }
                else if (str1 == CsConst.myPublicControlType[6].ControlTypeName ||
                         str1 == CsConst.myPublicControlType[9].ControlTypeName ||
                         str1 ==  CsConst.PanelControl[10] ||
                         str1 ==  CsConst.PanelControl[11])
                {
                    int intTmp = Convert.ToInt32(str2);
                    if (str1 == CsConst.myPublicControlType[6].ControlTypeName)
                    {
                        if (1 <= intTmp && intTmp <= 7) str2 = CsConst.mstrINIDefault.IniReadValue("public", "00048", "") + intTmp.ToString();
                        else if (intTmp == 255) str2 = CsConst.mstrINIDefault.IniReadValue("public", "00049", "");
                        else str2 = CsConst.WholeTextsList[1775].sDisplayName;
                    }
                    else if (str1 == CsConst.myPublicControlType[9].ControlTypeName)
                    {
                        if (intTmp == 255) str2 = CsConst.mstrINIDefault.IniReadValue("public", "00049", "");
                        else if (1 <= intTmp && intTmp <= 56) str2 = CsConst.mstrINIDefault.IniReadValue("public", "99867", "") + intTmp.ToString();
                        else str2 = CsConst.WholeTextsList[1775].sDisplayName;
                    }
                    else if (str1 ==  CsConst.PanelControl[10])
                    {
                        if (intTmp == 255) str2 = CsConst.mstrINIDefault.IniReadValue("public", "00049", "");
                        else if (1 <= intTmp && intTmp <= 32) str2 = CsConst.mstrINIDefault.IniReadValue("public", "99867", "") + intTmp.ToString();
                        else if (intTmp == 101) str2 = CsConst.mstrINIDefault.IniReadValue("public", "99868", "");
                        else if (intTmp == 102) str2 = CsConst.mstrINIDefault.IniReadValue("public", "99869", "");
                        else if (intTmp == 103) str2 = CsConst.mstrINIDefault.IniReadValue("public", "99870", "");
                        else if (intTmp == 104) str2 = CsConst.mstrINIDefault.IniReadValue("public", "99871", "");
                        else str2 = CsConst.mstrINIDefault.IniReadValue("public", "00049", "");
                    }
                    else if (str1 ==  CsConst.PanelControl[11])
                    {
                        if (1 <= intTmp && intTmp <= 32) str2 = CsConst.mstrINIDefault.IniReadValue("public", "99867", "") + intTmp.ToString();
                        else str2 = CsConst.WholeTextsList[1775].sDisplayName;
                    }
                    if (str3 == "1") str3 = CsConst.mstrINIDefault.IniReadValue("public", "00042", "");
                    else str3 = CsConst.WholeTextsList[1775].sDisplayName;
                }
                else if (str1 ==  CsConst.PanelControl[13])
                {
                    int intTmp = Convert.ToInt32(str2);
                    if (intTmp <= 4) str2 = CsConst.mstrINIDefault.IniReadValue("public", "0005" + intTmp.ToString(), "");
                    str3 = "N/A";
                }
                else if (str1 ==  CsConst.PanelControl[14])
                {
                    int intTmp = Convert.ToInt32(str2);
                    if (intTmp <= 3) str2 = CsConst.mstrINIDefault.IniReadValue("public", "0006" + intTmp.ToString(), "");
                    str3 = "N/A";
                }
                else if (str1 ==  CsConst.PanelControl[15] ||
                         str1 ==  CsConst.PanelControl[16])
                {
                    str3 = "N/A";
                }
                else if (str1 == CsConst.PanelControl[23] ||
                        str1 == CsConst.PanelControl[24])
                {
                    if (str3 == "255") str3 = CsConst.mstrINIDefault.IniReadValue("public", "00049", "");
                    else str3 = str3 + "(" + CsConst.WholeTextsList[934].sDisplayName + ")";
                }
                else if (str1 == CsConst.PanelControl[17] ||
                         str1 == CsConst.PanelControl[18] ||
                         str1 == CsConst.PanelControl[19] ||
                         str1 == CsConst.PanelControl[20])
                {
                    int intTmp = Convert.ToInt32(str2);
                    if (0 <= intTmp && intTmp <= 30) str2 = str2 + "C";
                    else if (32 <= intTmp && intTmp <= 86) str2 = str2 + "F";
                    str3 = "N/A";
                }
                else if (str1 == CsConst.myPublicControlType[22].ControlTypeName)
                {
                    int intTmp = Convert.ToInt32(str2);
                    if (intTmp <= 5) str2 = CsConst.mstrINIDefault.IniReadValue("public", "0007" + (intTmp - 1).ToString(), "");
                    if (str3 == "255") str3 = CsConst.mstrINIDefault.IniReadValue("public", "00049", "");
                    else str3 = str3 + "(" + CsConst.WholeTextsList[934].sDisplayName + ")";
                }
                else if (str1 == CsConst.myPublicControlType[25].ControlTypeName ||
                         str1 == CsConst.myPublicControlType[26].ControlTypeName ||
                         str1 == CsConst.myPublicControlType[27].ControlTypeName ||
                         str1 == CsConst.myPublicControlType[28].ControlTypeName)
                {
                    int intTmp = Convert.ToInt32(str2);
                    if (5 <= intTmp && intTmp <= 35) str2 = str2 + "C";
                    else if (41 <= intTmp && intTmp <= 95) str2 = str2 + "F";
                    intTmp = Convert.ToInt32(str3);
                    if (1 <= intTmp && intTmp <= 8) str2 = intTmp.ToString() + "(" + CsConst.WholeTextsList[934].sDisplayName + ")";
                    else if (intTmp == 255) str2 = CsConst.mstrINIDefault.IniReadValue("public", "00049", "");
                }
                else if (str1 == CsConst.PanelControl[29])
                {
                    int intTmp = Convert.ToInt32(str2);
                    if (1 <= intTmp && intTmp <= 7) str2 = CsConst.mstrINIDefault.IniReadValue("public", "00048", "") + str2;
                    str3 = "N/A";
                }
                else if (str1 == CsConst.PanelControl[30])
                {
                    int intTmp = Convert.ToInt32(str2);
                    if (1 <= intTmp && intTmp <= 32) str2 = str2 + "(" + CsConst.mstrINIDefault.IniReadValue("public", "99846", "") + ")";
                    str3 = "N/A";
                }

                str4 = "N/A";
                #endregion
            }
            else if (strType == CsConst.myPublicControlType[12].ControlTypeName)//消防模块
            {
                #region
                str1 = str1 + "(" + CsConst.WholeTextsList[2510].sDisplayName + ")";
                int intTmp = Convert.ToInt32(str2);
                if (1 <= intTmp && intTmp <= 10) str2 = CsConst.mstrINIDefault.IniReadValue("public", "0008" + (intTmp - 1).ToString(), "");
                str3 = "N/A";
                #endregion
            }
            else if (strType == CsConst.myPublicControlType[13].ControlTypeName)//音乐控制
            {
                #region
                int intTmp = Convert.ToInt32(str1);
                if (1 <= intTmp && intTmp <= 8) str1 = CsConst.mstrINIDefault.IniReadValue("public", "0009" + intTmp.ToString(), "");
                else str1 = CsConst.MusicControl[0];
                if (str1 == cbAudioControl.Items[0].ToString())
                {
                    intTmp = Convert.ToInt32(str2);
                    if (1 <= intTmp && intTmp <= 4) str2 = CsConst.mstrINIDefault.IniReadValue("public", "0010" + intTmp.ToString(), "");
                    str3 = "N/A";
                }
                else if (str1 == cbAudioControl.Items[1].ToString())
                {
                    intTmp = Convert.ToInt32(str2);
                    if (1 <= intTmp && intTmp <= 4) str2 = CsConst.mstrINIDefault.IniReadValue("public", "0011" + intTmp.ToString(), "");
                    str3 = "N/A";
                }
                else if (str1 == cbAudioControl.Items[2].ToString())
                {
                    intTmp = Convert.ToInt32(str2);
                    if (1 <= intTmp && intTmp <= 6) str2 = CsConst.mstrINIDefault.IniReadValue("public", "0012" + intTmp.ToString(), "");
                    if (intTmp == 3 || intTmp == 6)
                        str3 = str3 + "(" + CsConst.mstrINIDefault.IniReadValue("public", "00043", "") + ")";
                    else
                        str3 = "N/A";
                }
                else if (str1 == cbAudioControl.Items[3].ToString())
                {
                    intTmp = Convert.ToInt32(str2);
                    if (1 <= intTmp && intTmp <= 4) str2 = CsConst.mstrINIDefault.IniReadValue("public", "0013" + intTmp.ToString(), "");
                    str3 = "N/A";
                }
                else if (str1 == cbAudioControl.Items[4].ToString())
                {
                    intTmp = Convert.ToInt32(str2);
                    if (1 <= intTmp && intTmp <= 3) str2 = CsConst.mstrINIDefault.IniReadValue("public", "0014" + intTmp.ToString(), "");
                    if (intTmp == 1)
                    {
                        intTmp = Convert.ToInt32(str3);
                        if (intTmp >= 3)
                            str3 = CsConst.mstrINIDefault.IniReadValue("public", "99872", "") + ":" + (79 - (Convert.ToInt32(str4))).ToString();
                        else
                        {
                            if (intTmp == 1) str3 = CsConst.mstrINIDefault.IniReadValue("public", "00044", "");
                            else if (intTmp == 2) str3 = CsConst.mstrINIDefault.IniReadValue("public", "00045", "");
                        }
                    }
                    else
                    {
                        if (intTmp == 1) str3 = CsConst.mstrINIDefault.IniReadValue("public", "00044", "");
                        else if (intTmp == 2) str3 = CsConst.mstrINIDefault.IniReadValue("public", "00045", "");
                    }
                }
                else if (str1 == cbAudioControl.Items[5].ToString())
                {
                    str2 = str2 + "(" + CsConst.mstrINIDefault.IniReadValue("public", "00046", "") + ")";
                    str3 = (Convert.ToInt32(str3) * 256 + Convert.ToInt32(str4)).ToString() + "(" + CsConst.mstrINIDefault.IniReadValue("public", "00043", "") + ")";
                }
                else if (str1 == cbAudioControl.Items[6].ToString() || str1 == cbAudioControl.Items[7].ToString())
                {
                    intTmp = Convert.ToInt32(str2);
                    if (intTmp == 64)
                        str2 = CsConst.mstrINIDefault.IniReadValue("public", "00047", "");
                    else if (65 <= intTmp && intTmp <= 112)
                        str2 = "SD:" + (intTmp - 64).ToString();
                    else if (129 <= intTmp && intTmp <= 176)
                        str2 = "FTP:" + (intTmp - 128).ToString();
                    intTmp = Convert.ToInt32(str3) * 256 + Convert.ToInt32(str4);
                    str3 = intTmp.ToString() + "(" + CsConst.mstrINIDefault.IniReadValue("public", "00043", "") + ")";
                }
                #endregion
            }
        }

        private void dgvTarget_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                txtSub.Text = dgvTarget[1, e.RowIndex].Value.ToString();
                addcontrols(1, e.RowIndex, txtSub, dgvTarget);

                txtDev.Text = dgvTarget[2, e.RowIndex].Value.ToString();
                addcontrols(2, e.RowIndex, txtDev, dgvTarget);

                cbControlType.Text = dgvTarget[3, e.RowIndex].Value.ToString();
                addcontrols(3, e.RowIndex, cbControlType, dgvTarget);

                txtSub_TextChanged(txtSub, null);
                txtDev_TextChanged(txtDev, null);
                cbControlType_SelectedIndexChanged(cbControlType, null);
            }
        }

        private void btnSaveCommand_Click(object sender, EventArgs e)
        {
            try
            {
                if (CsConst.MyEditMode == 1)
                {
                    if (dgvTarget.RowCount == 0) return;
                    Cursor.Current = Cursors.WaitCursor;
                    btnSavePage1.Enabled = false;
                    setAllControlVisible(false);
                    for (int i = 0; i < dgvTarget.RowCount; i++)
                    {
                        byte[] arayTmp = new byte[10];
                        arayTmp[0] = 6;
                        arayTmp[1] = Convert.ToByte(dgvCommand[0, dgvCommand.CurrentRow.Index].Value.ToString());
                        arayTmp[2] = Convert.ToByte(i + 1);
                        arayTmp[4] = Convert.ToByte(dgvTarget[1, i].Value.ToString());
                        arayTmp[5] = Convert.ToByte(dgvTarget[2, i].Value.ToString());
                        arayTmp[3] = DryControlType.ConvertorKeyControlTypeToPublicModeGroup(dgvTarget[3, i].Value.ToString());
                        string str1 = dgvTarget[4, i].Value.ToString();
                        string str2 = dgvTarget[5, i].Value.ToString();
                        string str3 = dgvTarget[6, i].Value.ToString();
                        if (str1.Contains("(")) str1 = str1.Split('(')[0].ToString();
                        if (str2.Contains("(")) str2 = str2.Split('(')[0].ToString();
                        if (str3.Contains("(")) str3 = str3.Split('(')[0].ToString();
                        if (arayTmp[3] == 85 || arayTmp[3] == 86)//场景 序列
                        {
                            #region
                            if (HDLPF.IsRightNumStringMode(str1, 0, 255))
                                arayTmp[6] = Convert.ToByte(str1);
                            if (HDLPF.IsRightNumStringMode(str2, 0, 255))
                                arayTmp[7] = Convert.ToByte(str2);
                            #endregion
                        }
                        else if (arayTmp[3] == 88)//通用开关
                        {
                            #region
                            if (HDLPF.IsRightNumStringMode(str1, 0, 255))
                                arayTmp[6] = Convert.ToByte(str1);
                            if (str2 == CsConst.Status[0])
                                arayTmp[7] = 0;
                            else if (str2 == CsConst.Status[1])
                                arayTmp[7] = 255;
                            #endregion
                        }
                        else if (arayTmp[3] == 89)//单路调节
                        {
                            #region
                            if (HDLPF.IsRightNumStringMode(str1, 0, 255))
                                arayTmp[6] = Convert.ToByte(str1);
                            if (HDLPF.IsRightNumStringMode(str2, 0, 255))
                                arayTmp[7] = Convert.ToByte(str2);
                            if (str3.Contains(":"))
                            {
                                int intTmp = Convert.ToInt32(HDLPF.GetTimeFromString(str3, ':'));
                                arayTmp[8] = Convert.ToByte(intTmp / 256);
                                arayTmp[9] = Convert.ToByte(intTmp % 256);
                            }
                            #endregion
                        }
                        else if (arayTmp[3] == 100)//广播场景
                        {
                            #region
                            arayTmp[3] = 85;
                            arayTmp[6] = 255;
                            if (HDLPF.IsRightNumStringMode(str2, 0, 255))
                                arayTmp[7] = Convert.ToByte(str2);
                            #endregion
                        }
                        else if (arayTmp[3] == 101)//广播回路
                        {
                            #region
                            arayTmp[3] = 89;
                            arayTmp[6] = 255;
                            if (HDLPF.IsRightNumStringMode(str2, 0, 255))
                                arayTmp[7] = Convert.ToByte(str2);
                            if (str3.Contains(":"))
                            {
                                int intTmp = Convert.ToInt32(HDLPF.GetTimeFromString(str3, ':'));
                                arayTmp[8] = Convert.ToByte(intTmp / 256);
                                arayTmp[9] = Convert.ToByte(intTmp % 256);
                            }
                            #endregion
                        }
                        else if (arayTmp[3] == 92)//窗帘开关
                        {
                            #region
                            if (HDLPF.IsRightNumStringMode(str1, 0, 255))
                                arayTmp[6] = Convert.ToByte(str1);
                            if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00036", "")) arayTmp[7] = 0;
                            else if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00037", "")) arayTmp[7] = 1;
                            else if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00038", "")) arayTmp[7] = 2;
                            else
                            {
                                str2 = str2.Replace("%", "");
                                if (HDLPF.IsRightNumStringMode(str2, 0, 255))
                                    arayTmp[7] = Convert.ToByte(str2);
                                if (HDLPF.IsRightNumStringMode(str1, 0, 255))
                                    arayTmp[6] = Convert.ToByte(Convert.ToByte(str1) + 16);
                            }
                            #endregion
                        }
                        else if (arayTmp[3] == 94)//GPRS
                        {
                            #region
                            if (str1 == CsConst.mstrINIDefault.IniReadValue("public", "99862", "")) arayTmp[6] = 1;
                            else if (str1 == CsConst.mstrINIDefault.IniReadValue("public", "99863", "")) arayTmp[6] = 2;
                            else arayTmp[6] = 0;
                            if (HDLPF.IsRightNumStringMode(str2, 0, 255))
                                arayTmp[7] = Convert.ToByte(str2);
                            #endregion
                        }
                        else if (arayTmp[3] == 95)//面板控制
                        {
                            #region
                            arayTmp[6] = Convert.ToByte(HDLSysPF.getIDFromPanelControlTypeString(str1));
                            if (arayTmp[6] == 1 || arayTmp[6] == 11 ||
                                arayTmp[6] == 2 || arayTmp[6] == 12 ||
                                arayTmp[6] == 24 || arayTmp[6] == 3 ||
                                arayTmp[6] == 20)
                            {
                                if (str2 == CsConst.Status[0])
                                    arayTmp[7] = 0;
                                else if (str2 == CsConst.Status[1])
                                    arayTmp[7] = 1;
                            }
                            else if (arayTmp[6] == 13 || arayTmp[6] == 14)
                            {
                                if (HDLPF.IsRightNumStringMode(str2, 0, 255))
                                    arayTmp[7] = Convert.ToByte(str2);
                            }
                            else if (arayTmp[6] == 16 || arayTmp[6] == 15 ||
                                     arayTmp[6] == 17 || arayTmp[6] == 18)
                            {
                                if (arayTmp[6] == 16)
                                {
                                    if (str2.Contains(CsConst.mstrINIDefault.IniReadValue("public", "00048", "")))
                                    {
                                        str2 = str2.Replace(CsConst.mstrINIDefault.IniReadValue("public", "00048", ""), "");
                                        if (HDLPF.IsRightNumStringMode(str2, 0, 255))
                                            arayTmp[7] = Convert.ToByte(str2);
                                    }
                                    else if (str2 == CsConst.WholeTextsList[1775].sDisplayName)
                                        arayTmp[7] = 0;
                                    else if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00049", ""))
                                        arayTmp[7] = 255;
                                }
                                else if (arayTmp[6] == 15)
                                {
                                    if (str2.Contains(CsConst.mstrINIDefault.IniReadValue("public", "99867", "")))
                                    {
                                        str2 = str2.Replace(CsConst.mstrINIDefault.IniReadValue("public", "99867", ""), "");
                                        if (HDLPF.IsRightNumStringMode(str2, 0, 255))
                                            arayTmp[7] = Convert.ToByte(str2);
                                    }
                                    else if (str2 == CsConst.WholeTextsList[1775].sDisplayName)
                                        arayTmp[7] = 0;
                                    else if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00049", ""))
                                        arayTmp[7] = 255;
                                }
                                else if (arayTmp[6] == 17)
                                {
                                    if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00049", "")) arayTmp[7] = 255;
                                    else if (str2.Contains(CsConst.mstrINIDefault.IniReadValue("public", "99867", "")) &&
                                        !str2.Contains("["))
                                    {
                                        str2 = str2.Replace(CsConst.mstrINIDefault.IniReadValue("public", "99867", ""), "");
                                        if (HDLPF.IsRightNumStringMode(str2, 0, 255))
                                            arayTmp[7] = Convert.ToByte(str2);
                                    }
                                    else if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "99867", "")) arayTmp[7] = 101;
                                    else if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "99868", "")) arayTmp[7] = 102;
                                    else if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "99869", "")) arayTmp[7] = 103;
                                    else if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "99870", "")) arayTmp[7] = 104;
                                    else if (str2 == CsConst.WholeTextsList[1775].sDisplayName) arayTmp[7] = 0;
                                }
                                else if (arayTmp[6] == 18)
                                {
                                    if (str2.Contains(CsConst.mstrINIDefault.IniReadValue("public", "99867", "")))
                                    {
                                        str2 = str2.Replace(CsConst.mstrINIDefault.IniReadValue("public", "99867", ""), "");
                                        if (HDLPF.IsRightNumStringMode(str2, 0, 255))
                                            arayTmp[7] = Convert.ToByte(str2);
                                    }
                                    else if (str2 == CsConst.WholeTextsList[1775].sDisplayName) arayTmp[7] = 0;
                                }
                                if (str3 == CsConst.mstrINIDefault.IniReadValue("public", "00042", "")) arayTmp[8] = 1;
                                else if (str3 == CsConst.WholeTextsList[1775].sDisplayName) arayTmp[8] = 0;
                            }
                            else if (arayTmp[6] == 6)
                            {
                                if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00050", "")) arayTmp[7] = 0;
                                else if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00051", "")) arayTmp[7] = 1;
                                else if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00052", "")) arayTmp[7] = 2;
                                else if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00053", "")) arayTmp[7] = 3;
                                else if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00054", "")) arayTmp[7] = 4;
                            }
                            else if (arayTmp[6] == 5)
                            {
                                if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00060", "")) arayTmp[7] = 0;
                                else if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00061", "")) arayTmp[7] = 1;
                                else if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00062", "")) arayTmp[7] = 2;
                                else if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00063", "")) arayTmp[7] = 3;
                            }
                            else if (arayTmp[6] == 9 || arayTmp[6] == 10 ||
                                     arayTmp[6] == 22 || arayTmp[6] == 23)
                            {
                                if (HDLPF.IsRightNumStringMode(str2, 0, 255))
                                    arayTmp[7] = Convert.ToByte(str2);
                                if (arayTmp[6] == 22 || arayTmp[6] == 23)
                                {
                                    if (str3 == CsConst.mstrINIDefault.IniReadValue("public", "00049", ""))
                                    {
                                        arayTmp[8] = 255;
                                    }
                                    else
                                    {
                                        if (HDLPF.IsRightNumStringMode(str3, 0, 255))
                                            arayTmp[8] = Convert.ToByte(str3);
                                    }
                                }
                            }
                            else if (arayTmp[6] == 4 || arayTmp[6] == 7 ||
                                     arayTmp[6] == 8 || arayTmp[6] == 19)
                            {
                                if (str2.Contains("C")) str2 = str2.Replace("C", "");
                                if (str2.Contains("F")) str2 = str2.Replace("F", "");
                                if (HDLPF.IsRightNumStringMode(str2, 0, 255))
                                    arayTmp[7] = Convert.ToByte(str2);
                            }
                            else if (arayTmp[6] == 21)
                            {
                                if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00070", "")) arayTmp[7] = 1;
                                else if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00071", "")) arayTmp[7] = 2;
                                else if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00072", "")) arayTmp[7] = 3;
                                else if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00073", "")) arayTmp[7] = 4;
                                else if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00074", "")) arayTmp[7] = 5;

                                if (str3 == CsConst.mstrINIDefault.IniReadValue("public", "00049", ""))
                                {
                                    arayTmp[8] = 255;
                                }
                                else
                                {
                                    if (HDLPF.IsRightNumStringMode(str3, 0, 255))
                                        arayTmp[8] = Convert.ToByte(str3);
                                }
                            }
                            #endregion
                        }
                        else if (arayTmp[3] == 102)//消防模块
                        {
                            #region
                            if (HDLPF.IsRightNumStringMode(str1, 0, 255))
                                arayTmp[6] = Convert.ToByte(str1);
                            if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00080", "")) arayTmp[7] = 1;
                            else if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00081", "")) arayTmp[7] = 2;
                            else if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00082", "")) arayTmp[7] = 3;
                            else if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00083", "")) arayTmp[7] = 4;
                            else if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00084", "")) arayTmp[7] = 5;
                            else if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00085", "")) arayTmp[7] = 6;
                            else if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00086", "")) arayTmp[7] = 7;
                            else if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00087", "")) arayTmp[7] = 8;
                            else if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00088", "")) arayTmp[7] = 9;
                            else if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00089", "")) arayTmp[7] = 10;
                            #endregion
                        }
                        else if (arayTmp[3] == 103)//音乐控制
                        {
                            #region
                            if (str1 == cbAudioControl.Items[0].ToString())
                            {
                                arayTmp[6] = 1;
                                if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00101", "")) arayTmp[7] = 1;
                                else if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00102", "")) arayTmp[7] = 2;
                                else if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00103", "")) arayTmp[7] = 3;
                                else if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00104", "")) arayTmp[7] = 4;
                            }
                            else if (str1 == cbAudioControl.Items[1].ToString())
                            {
                                arayTmp[6] = 2;
                                if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00111", "")) arayTmp[7] = 1;
                                else if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00112", "")) arayTmp[7] = 2;
                                else if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00113", "")) arayTmp[7] = 3;
                                else if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00114", "")) arayTmp[7] = 4;
                            }
                            else if (str1 == cbAudioControl.Items[2].ToString())
                            {
                                arayTmp[6] = 3;
                                if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00121", "")) arayTmp[7] = 1;
                                else if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00122", "")) arayTmp[7] = 2;
                                else if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00123", "")) arayTmp[7] = 3;
                                else if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00124", "")) arayTmp[7] = 4;
                                else if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00123", "")) arayTmp[7] = 5;
                                else if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00124", "")) arayTmp[7] = 6;
                                if (arayTmp[7] == 3 || arayTmp[7] == 6)
                                {
                                    if (HDLPF.IsRightNumStringMode(str3, 0, 255))
                                        arayTmp[8] = Convert.ToByte(str3);
                                }
                            }
                            else if (str1 == cbAudioControl.Items[3].ToString())
                            {
                                arayTmp[6] = 4;
                                if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00131", "")) arayTmp[7] = 1;
                                else if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00132", "")) arayTmp[7] = 2;
                                else if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00133", "")) arayTmp[7] = 3;
                                else if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00134", "")) arayTmp[7] = 4;
                            }
                            else if (str1 == cbAudioControl.Items[4].ToString())
                            {
                                arayTmp[6] = 5;
                                if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00141", "")) arayTmp[7] = 1;
                                else if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00142", "")) arayTmp[7] = 2;
                                else if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00143", "")) arayTmp[7] = 3;
                                if (arayTmp[7] == 1)
                                {
                                    if (str3 == CsConst.mstrINIDefault.IniReadValue("public", "00044", ""))
                                        arayTmp[8] = 1;
                                    else if (str3 == CsConst.mstrINIDefault.IniReadValue("public", "00045", ""))
                                        arayTmp[8] = 2;
                                    else if (str3.Contains(CsConst.mstrINIDefault.IniReadValue("public", "99872", "")))
                                    {
                                        arayTmp[8] = 3;
                                        if (str3.Contains(":"))
                                        {
                                            str3 = str3.Split(':')[1].ToString();
                                            if (HDLPF.IsRightNumStringMode(str3, 0, 65535))
                                                arayTmp[9] = Convert.ToByte(79 - Convert.ToInt32(str3));
                                        }
                                    }
                                }
                                else if (arayTmp[6] == 2 || arayTmp[6] == 3)
                                {
                                    if (str3 == CsConst.mstrINIDefault.IniReadValue("public", "00044", ""))
                                        arayTmp[8] = 1;
                                    else if (str3 == CsConst.mstrINIDefault.IniReadValue("public", "00045", ""))
                                        arayTmp[8] = 2;
                                }
                            }
                            else if (str1 == cbAudioControl.Items[5].ToString())
                            {
                                arayTmp[6] = 6;
                                if (HDLPF.IsRightNumStringMode(str2, 0, 255))
                                    arayTmp[7] = Convert.ToByte(str2);
                                if (HDLPF.IsRightNumStringMode(str3, 0, 65535))
                                {
                                    int intTmp = Convert.ToInt32(str3);
                                    arayTmp[8] = Convert.ToByte(intTmp / 256);
                                    arayTmp[9] = Convert.ToByte(intTmp % 256);
                                }
                            }
                            else if (str1 == cbAudioControl.Items[6].ToString() || str1 == cbAudioControl.Items[7].ToString())
                            {
                                if (str1 == cbAudioControl.Items[6].ToString())
                                    arayTmp[6] = 7;
                                if (str1 == cbAudioControl.Items[7].ToString())
                                    arayTmp[6] = 8;

                                if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00047", ""))
                                    arayTmp[7] = 64;
                                else if (str2.Contains("SD"))
                                {
                                    if (str2.Contains(":"))
                                    {
                                        if (HDLPF.IsRightNumStringMode(str2.Split(':')[1].ToString(), 0, 65535))
                                            arayTmp[7] = Convert.ToByte(Convert.ToInt32(str2.Split(':')[1].ToString()) + 64);
                                    }
                                }
                                else if (str2.Contains("FTP"))
                                {
                                    if (str2.Contains(":"))
                                    {
                                        if (HDLPF.IsRightNumStringMode(str2.Split(':')[1].ToString(), 0, 65535))
                                            arayTmp[7] = Convert.ToByte(Convert.ToInt32(str2.Split(':')[1].ToString()) + 128);
                                    }
                                }
                                if (HDLPF.IsRightNumStringMode(str3, 0, 65535))
                                {
                                    int intTmp = Convert.ToInt32(str3);
                                    arayTmp[8] = Convert.ToByte(intTmp / 256);
                                    arayTmp[9] = Convert.ToByte(intTmp % 256);
                                }
                            }
                            #endregion
                        }
                        if (CsConst.mySends.AddBufToSndList(arayTmp, 0x1370, SubNetID, DevID, false, true, true, false) == false)
                        {
                            break;
                        }
                        else
                        {
                            System.Threading.Thread.Sleep(1);
                        }
                        CsConst.myRevBuf = new byte[1200];
                    }
                    
                }
            }
            catch
            {
            }
            btnSavePage1.Enabled = true;
            Cursor.Current = Cursors.Default;
        }

        private void dgvCommand_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            
        }

        

        private void btnReadUV_Click(object sender, EventArgs e)
        {
            CtrlsVisisbleForForm(false);
            Cursor.Current = Cursors.WaitCursor;
            btnReadUV.Enabled = false;
            byte[] arayTmp = new byte[2];
            byte bytFrm = Convert.ToByte(Convert.ToInt32(txtUV1.Text));
            byte bytTo = Convert.ToByte(txtUV2.Text);
            dgvUV.Rows.Clear();
            for (byte byt = bytFrm; byt <= bytTo; byt++)
            {
                string strType = "";
                string strRemark = "";
                arayTmp[0] = 0x03;
                arayTmp[1] = byt;
                if (CsConst.mySends.AddBufToSndList(arayTmp, 0x1372, SubNetID, DevID, false, true, true, false) == true)
                {
                    if (CsConst.myRevBuf[29] == 1) strType = CsConst.Status[1];
                    else if (CsConst.myRevBuf[29] == 0) strType = CsConst.Status[0];
                    if (CsConst.myRevBuf[27] != 1) strType = CsConst.WholeTextsList[1775].sDisplayName;
                    byte[] arayRemark = new byte[20];
                    Array.Copy(CsConst.myRevBuf, 30, arayRemark, 0, 20);
                    strRemark = HDLPF.Byte2String(arayRemark);
                    object[] obj = new object[] { byt.ToString(), strRemark, CsConst.myRevBuf[28].ToString(), strType };
                    dgvUV.Rows.Add(obj);
                    CsConst.myRevBuf = new byte[1200];
                    System.Threading.Thread.Sleep(1);
                }
            }
            DataGridViewCellEventArgs even = new DataGridViewCellEventArgs(0, 0);
            dgvUV_CellClick(null, even);
            btnReadUV.Enabled = true;
            Cursor.Current = Cursors.Default;
        }

        private void btnSaveUV_Click(object sender, EventArgs e)
        {
            try
            {
                CtrlsVisisbleForForm(false);
                Cursor.Current = Cursors.WaitCursor;
                for (int i = 0; i < dgvUVTargets.Rows.Count; i++)
                {
                    byte[] ArayTmp = new byte[38];
                    ArayTmp[0] = 2;
                    ArayTmp[1] = Convert.ToByte(dgvUV.CurrentRow.Index + 1);
                    ArayTmp[2] = Convert.ToByte(i + 1);
                    string strType = dgvUVTargets[2, i].Value.ToString();
                    string strCommand = dgvUVTargets[3, i].Value.ToString();
                    if (strType == CsConst.WholeTextsList[1775].sDisplayName)
                    {
                        ArayTmp[5] = 0;
                        byte[] arayCommand = new byte[32];
                        arayCommand = HDLUDP.StringToByte(strCommand);
                        if (arayCommand.Length <= 32)
                        {
                            Array.Copy(arayCommand, 0, ArayTmp, 6, arayCommand.Length);
                            ArayTmp[3] = Convert.ToByte(arayCommand.Length);
                        }
                        else
                        {
                            Array.Copy(arayCommand, 0, ArayTmp, 6, 32);
                            ArayTmp[3] = 32;
                        }
                    }
                    else if (strType == CsConst.mstrINIDefault.IniReadValue("Public", "99838", ""))
                    {
                        ArayTmp[5] = 1;
                        byte[] arayCommand = new byte[32];
                        arayCommand = HDLUDP.StringToByte(strCommand);
                        if (arayCommand.Length <= 32)
                        {
                            Array.Copy(arayCommand, 0, ArayTmp, 6, arayCommand.Length);
                            ArayTmp[3] = Convert.ToByte(arayCommand.Length);
                        }
                        else
                        {
                            Array.Copy(arayCommand, 0, ArayTmp, 6, 32);
                            ArayTmp[3] = 32;
                        }
                    }
                    else if (strType == CsConst.mstrINIDefault.IniReadValue("Public", "99839", ""))
                    {
                        if (strCommand != null && strCommand != "")
                        {
                            ArayTmp[5] = 2;
                            string[] strHex = strCommand.Split(' ');
                            byte[] arayCommand = new byte[32];
                            if (strHex.Length <= 32)
                            {
                                for (int j = 0; j < strHex.Length; j++)
                                    ArayTmp[6 + j] = Convert.ToByte(Convert.ToInt32(strHex[j], 16));
                                ArayTmp[3] = Convert.ToByte(strHex.Length);
                            }
                            else
                            {
                                for (int j = 0; j < 32; j++)
                                    ArayTmp[6 + j] = Convert.ToByte(Convert.ToInt32(strHex[j], 16));
                                ArayTmp[3] = 32;
                            }
                        }
                        else
                        {
                            ArayTmp[5] = 2;
                            ArayTmp[3] = 32;
                        }
                    }
                    ArayTmp[4] = Convert.ToByte(cb232RSNO.Items.IndexOf(dgvUVTargets[1, i].Value.ToString()));
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1370, SubNetID, DevID, false, true, true, false) == false)
                    {
                        break;
                    }
                    CsConst.myRevBuf = new byte[1200];
                    System.Threading.Thread.Sleep(1);
                }
            }
            catch
            {
            }
            Cursor.Current = Cursors.Default;
        }

        private void txtUV1_TextChanged(object sender, EventArgs e)
        {
            string str = txtUV1.Text;
            int num = Convert.ToInt32(txtUV2.Text);
            txtUV1.Text = HDLPF.IsNumStringMode(str, 1, num);
            txtUV1.SelectionStart = txtUV1.Text.Length;
        }

        private void txtUV2_TextChanged(object sender, EventArgs e)
        {
            string str = txtUV2.Text;
            int num = Convert.ToInt32(txtUV1.Text);
            txtUV2.Text = HDLPF.IsNumStringMode(str, num, 200);
            txtUV2.SelectionStart = txtUV2.Text.Length;
        }

        private void dgvUV_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                int id = Convert.ToInt32(dgvUV[0, e.RowIndex].Value.ToString());
                readUVTargets(id);
            }
        }

        private void dgvUV_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            
        }

        private void readUVTargets(int id)
        {
            Cursor.Current = Cursors.WaitCursor;
            byte[] arayTmp = new byte[3];
            arayTmp[0] = 2;
            arayTmp[1] = Convert.ToByte(id);
            dgvUVTargets.Rows.Clear();
            for (int i = 0; i < 8; i++)
            {
                arayTmp[2] = Convert.ToByte(i + 1);
                if (CsConst.mySends.AddBufToSndList(arayTmp, 0x1372, SubNetID, DevID, false, true, true, false) == true)
                {
                    int Length = CsConst.myRevBuf[28];
                    if (Length > 32) Length = 32;
                    int RSNO = 0;
                    if (CsConst.myRevBuf[29] <= 3) RSNO = CsConst.myRevBuf[29];
                    int type = CsConst.myRevBuf[30];
                    string strCommand = "";
                    string strType = "";
                    if (type == 1)
                    {
                        byte[] arayCommand = new byte[Length];
                        Array.Copy(CsConst.myRevBuf, 31, arayCommand, 0, Length);
                        strCommand = HDLPF.Byte2String(arayCommand);
                        strType = CsConst.mstrINIDefault.IniReadValue("Public", "99838", "");
                    }
                    else if (type == 2)
                    {
                        for (int j = 0; j < Length; j++) strCommand = strCommand + GlobalClass.AddLeftZero(CsConst.myRevBuf[31 + j].ToString("X"), 2) + " ";
                        strCommand = strCommand.Trim();
                        strType = CsConst.mstrINIDefault.IniReadValue("Public", "99839", "");
                    }
                    else
                    {
                        strType = CsConst.WholeTextsList[1775].sDisplayName;
                    }
                    if (Length == 0) strType = CsConst.WholeTextsList[1775].sDisplayName;
                    object[] obj = new object[] { (i + 1).ToString(), (RSNO + 1).ToString(), strType, strCommand };
                    dgvUVTargets.Rows.Add(obj);
                }
                else break;
            }
            Cursor.Current = Cursors.Default;
        }

        private void dgvUVTargets_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                cb232RSNO.Text = dgvUVTargets[1, e.RowIndex].Value.ToString();
                addcontrols(1, e.RowIndex, cb232RSNO, dgvUVTargets);

                cb232ControlMode.Text = dgvUVTargets[2, e.RowIndex].Value.ToString();
                addcontrols(2, e.RowIndex, cb232ControlMode, dgvUVTargets);

                txt232Command.Text = dgvUVTargets[3, e.RowIndex].Value.ToString();
                addcontrols(3, e.RowIndex, txt232Command, dgvUVTargets);
            }
        }

        private void btnReadOn_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            byte[] ArayTmp = new byte[3];
            ArayTmp[0] = 9;
            ArayTmp[1] = Convert.ToByte(cbChannel.SelectedIndex + 1);
            ArayTmp[2] = Convert.ToByte(cbType.SelectedIndex);
            int Currentofload = 0;
            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1372, SubNetID, DevID, false, true, true, false) == true)
            {
                Currentofload = CsConst.myRevBuf[28] * 256 + CsConst.myRevBuf[29];
                lbDOnValue.Text = Currentofload.ToString();
            }
            lbDAValue.Text = ((Convert.ToInt32(lbDOnValue.Text) + Convert.ToInt32(lbDOffValue.Text)) / 2).ToString();
            CsConst.myRevBuf = new byte[1200];
            Cursor.Current = Cursors.Default;
        }

        private void btnReadOff_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            byte[] ArayTmp = new byte[3];
            int Currentofload = 0;
            ArayTmp[0] = 9;
            ArayTmp[1] = Convert.ToByte(cbChannel.SelectedIndex + 1);
            ArayTmp[2] = Convert.ToByte(cbType.SelectedIndex);
            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1372, SubNetID, DevID, false, true, true, false) == true)
            {
                Currentofload = CsConst.myRevBuf[28] * 256 + CsConst.myRevBuf[29];
                lbDOffValue.Text = Currentofload.ToString();
            }
            lbDAValue.Text = ((Convert.ToInt32(lbDOnValue.Text) + Convert.ToInt32(lbDOffValue.Text)) / 2).ToString();
            CsConst.myRevBuf = new byte[1200];
            Cursor.Current = Cursors.Default;
        }

        private void btnSaveLoad_Click(object sender, EventArgs e)
        {
            if (mmc.Reserved == null) mmc.Reserved = new byte[50];
            if (mmc.Reserved.Length != 50) mmc.Reserved = new byte[50];
            for (int i = 0; i < 5; i++)
            {
                int intTmp = Convert.ToInt32(dgvLoad[2, i].Value.ToString());
                mmc.Reserved[i * 10 + 1] = Convert.ToByte(intTmp / 256);
                mmc.Reserved[i * 10 + 2] = Convert.ToByte(intTmp % 256);
                intTmp = Convert.ToInt32(dgvLoad[3, i].Value.ToString());
                mmc.Reserved[i * 10 + 3] = Convert.ToByte(intTmp + 128);
            }
            tmUpload_Click(null, null);
        }

        private void cbSeq_SelectedIndexChanged(object sender, EventArgs e)
        {
            SeqCtrsVisible(false);
            Cursor.Current = Cursors.WaitCursor;
            if (cbSeq.SelectedIndex < 0) cbSeq.SelectedIndex = 0;
            DgvSeries.Rows.Clear();
            if (mmc.IRCodes == null || mmc.IRCodes.Count != 24)
            {
                MyActivePage = 3;
                tslRead_Click(tslRead, null);
            }
            Cursor.Current = Cursors.WaitCursor;
            for (int i = 1; i <= 32; i++)
            {
                byte[] arayTmp = new byte[4];
                arayTmp[0] = 12;
                for (int j = 1; j <= 8; j++)
                {
                    RadioButton temp = this.Controls.Find("rb" + j.ToString(), true)[0] as RadioButton;
                    if (temp.Checked)
                    {
                        arayTmp[1] = Convert.ToByte(j);
                        break;
                    }
                }
                arayTmp[2] = Convert.ToByte(cbSeq.SelectedIndex + 1);
                arayTmp[3] = Convert.ToByte(i);
                if (CsConst.mySends.AddBufToSndList(arayTmp, 0x1372, SubNetID, DevID, false, true, true, false) == true)
                {
                    string strType = CsConst.WholeTextsList[1775].sDisplayName;
                    if (CsConst.myRevBuf[29] == 0) strType = CsConst.mstrINIDefault.IniReadValue("Public", "00200", "");
                    else if (CsConst.myRevBuf[29] == 1) strType = CsConst.mstrINIDefault.IniReadValue("Public", "00201", "");
                    else if (CsConst.myRevBuf[29] == 2) strType = CsConst.mstrINIDefault.IniReadValue("Public", "00202", "");
                    else if (CsConst.myRevBuf[29] == 3) strType = CsConst.mstrINIDefault.IniReadValue("Public", "00203", "");
                    string strParam1 = "N/A";
                    string strParam2 = "N/A";
                    string strParam3 = "N/A";
                    string strElectri = "N/A";
                    if (CsConst.myRevBuf[29] == 0)
                    {
                        strParam1 = CsConst.myRevBuf[30].ToString() + "(" + CsConst.WholeTextsList[2506].sDisplayName + ")";
                        if (1 <= CsConst.myRevBuf[31] && CsConst.myRevBuf[31] <= 24)
                        {
                            if (mmc.IRCodes.Count == 24)
                            {
                                strParam2 = CsConst.myRevBuf[31].ToString() + "-" + mmc.IRCodes[CsConst.myRevBuf[31] - 1].Remark;
                                int devid = mmc.IRCodes[CsConst.myRevBuf[31] - 1].DevID;
                                strParam3 = CsConst.mstrINIDefault.IniReadValue("NewIRDevice" + devid.ToString(), CsConst.myRevBuf[32].ToString("D5"), "");
                                if (strParam3 == "")
                                    strParam3 = CsConst.myRevBuf[32].ToString();
                            }
                            else
                            {
                                strParam2 = CsConst.myRevBuf[31].ToString() + "(" + CsConst.WholeTextsList[216].sDisplayName + ")";
                                strParam3 = CsConst.myRevBuf[32].ToString() + "(" + CsConst.WholeTextsList[2505].sDisplayName + ")";
                            }
                        }
                        else
                        {
                            strParam2 = CsConst.myRevBuf[31].ToString() + "(" + CsConst.WholeTextsList[216].sDisplayName + ")";
                            strParam3 = CsConst.myRevBuf[32].ToString() + "(" + CsConst.WholeTextsList[2505].sDisplayName + ")";
                        }
                        if (CsConst.myRevBuf[33] == 1) strElectri = CsConst.Status[0];
                        else if (CsConst.myRevBuf[33] == 2) strElectri = CsConst.Status[1];
                        else strElectri = CsConst.mstrINIDefault.IniReadValue("Public", "99832", "");
                    }
                    else if (CsConst.myRevBuf[29] == 1)
                    {
                        strParam1 = CsConst.myRevBuf[30].ToString() + "(" + CsConst.WholeTextsList[2513].sDisplayName + ")";
                        string str = CsConst.Status[0];
                        if (CsConst.myRevBuf[31] == 1) str = CsConst.Status[1];
                        strParam2 = str + "(" + CsConst.WholeTextsList[2529].sDisplayName + ")";
                    }
                    else if (CsConst.myRevBuf[29] == 2)
                    {
                        strParam1 = CsConst.myRevBuf[30].ToString() + "(" + CsConst.WholeTextsList[934].sDisplayName + ")";
                        string str = CsConst.Status[0];
                        if (CsConst.myRevBuf[31] == 1) str = CsConst.Status[1];
                        strParam2 = str + "(" + CsConst.WholeTextsList[2529].sDisplayName + ")";
                    }
                    else if (CsConst.myRevBuf[29] == 3)
                    {
                        strParam1 = CsConst.myRevBuf[30].ToString() + "(" + CsConst.mstrINIDefault.IniReadValue("Public", "99831", "") + ")";
                        string str = CsConst.Status[0];
                        if (CsConst.myRevBuf[31] == 1) str = CsConst.Status[1];
                        strParam2 = str + "(" + CsConst.WholeTextsList[2529].sDisplayName + ")";
                    }
                    int intDelay = CsConst.myRevBuf[35] * 256 + CsConst.myRevBuf[36];
                    string strDelay = HDLPF.GetStringFromTime(intDelay,":");
                    object[] obj = new object[] { i.ToString(), strType, strParam1, strParam2, strParam3, strElectri, strDelay };
                    DgvSeries.Rows.Add(obj);
                    CsConst.myRevBuf = new byte[1200];
                    System.Threading.Thread.Sleep(1);
                }
                else break;
            }
            Cursor.Current = Cursors.Default;
        }

        private void dgvLoad_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                txtLoad.Text = dgvLoad[2, e.RowIndex].Value.ToString();
                addcontrols(2, e.RowIndex, txtLoad, dgvLoad);

                cbLoad.Text = dgvLoad[3, e.RowIndex].Value.ToString();
                addcontrols(3, e.RowIndex, cbLoad, dgvLoad);
            }
        }

        private void DgvSeries_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            SeqCtrsVisible(false);
            if (e.RowIndex >= 0)
            {
                cbSeqType.SelectedIndex = cbSeqType.Items.IndexOf(DgvSeries[1, e.RowIndex].Value.ToString());
                addcontrols(1, e.RowIndex, cbSeqType, DgvSeries);

            }
            cbSeqType_SelectedIndexChanged(null, null);
        }

        private void btnSaveSeq_Click(object sender, EventArgs e)
        {
            SeqCtrsVisible(false);
            Cursor.Current = Cursors.WaitCursor;
            btnSave5.Enabled = false;
            try
            {
                for (int i = 0; i < DgvSeries.RowCount; i++)
                {
                    string str1 = DgvSeries[2, i].Value.ToString();
                    string str2 = DgvSeries[3, i].Value.ToString();
                    string str3 = DgvSeries[4, i].Value.ToString();
                    string str4 = DgvSeries[5, i].Value.ToString();
                    if (str1.Contains("(")) str1 = str1.Split('(')[0].ToString();
                    if (str2.Contains("(")) str2 = str2.Split('(')[0].ToString();
                    if (str3.Contains("(")) str3 = str3.Split('(')[0].ToString();
                    if (str4.Contains("(")) str4 = str4.Split('(')[0].ToString();
                    byte[] arayTmp = new byte[12];
                    arayTmp[0] = 12;
                    for (int j = 1; j <= 8; j++)
                    {
                        RadioButton temp = this.Controls.Find("rb" + j.ToString(), true)[0] as RadioButton;
                        if (temp.Checked)
                        {
                            arayTmp[1] = Convert.ToByte(j);
                            break;
                        }
                    }
                    arayTmp[2] = Convert.ToByte(cbSeq.SelectedIndex + 1);
                    arayTmp[3] = Convert.ToByte(i + 1);
                    if (DgvSeries[1, i].Value.ToString() == CsConst.WholeTextsList[1775].sDisplayName)
                    {
                        arayTmp[4] = 255;
                    }
                    else if (DgvSeries[1, i].Value.ToString() == CsConst.mstrINIDefault.IniReadValue("Public", "00200", ""))
                    {
                        arayTmp[4] = 0;
                        arayTmp[5] = Convert.ToByte(str1);
                        if (str2.Contains('-')) arayTmp[6] = Convert.ToByte(str2.Split('-')[0].ToString());
                        int devid = mmc.IRCodes[arayTmp[6] - 1].DevID;
                        cbParam3.Items.Clear();
                        cbParam3.Items.AddRange(CsConst.mstrINIDefault.IniReadValuesInALlSectionStr("NewIRDevice" + devid.ToString()));
                        if (str3 != "N/A")
                            arayTmp[7] = Convert.ToByte(cbParam3.Items.IndexOf(str3) + 1);
                        arayTmp[8] = Convert.ToByte(cbParam4.Items.IndexOf(str4));
                    }
                    else if (DgvSeries[1, i].Value.ToString() == CsConst.mstrINIDefault.IniReadValue("Public", "00201", ""))
                    {
                        arayTmp[4] = 1;
                        arayTmp[5] = Convert.ToByte(str1);
                        cbParam2.Items.Clear();
                        cbParam2.Items.Add(CsConst.Status[0]);
                        cbParam2.Items.Add(CsConst.Status[1]);
                        if (cbParam2.Items.IndexOf(str2) >= 0)
                            arayTmp[6] = Convert.ToByte(cbParam2.Items.IndexOf(str2));
                    }
                    else if (DgvSeries[1, i].Value.ToString() == CsConst.mstrINIDefault.IniReadValue("Public", "00202", ""))
                    {
                        arayTmp[4] = 2;
                        arayTmp[5] = Convert.ToByte(str1);
                        cbParam2.Items.Clear();
                        cbParam2.Items.Add(CsConst.Status[0]);
                        cbParam2.Items.Add(CsConst.Status[1]);
                        if (cbParam2.Items.IndexOf(str2) >= 0)
                            arayTmp[6] = Convert.ToByte(cbParam2.Items.IndexOf(str2));
                    }
                    else if (DgvSeries[1, i].Value.ToString() == CsConst.mstrINIDefault.IniReadValue("Public", "00203", ""))
                    {
                        arayTmp[4] = 3;
                        arayTmp[5] = Convert.ToByte(str1);
                        cbParam2.Items.Clear();
                        cbParam2.Items.Add(CsConst.Status[0]);
                        cbParam2.Items.Add(CsConst.Status[1]);
                        if (cbParam2.Items.IndexOf(str2) >= 0)
                            arayTmp[6] = Convert.ToByte(cbParam2.Items.IndexOf(str2));
                    }
                    string strTmp = DgvSeries[6, i].Value.ToString();
                    int intTmp = Convert.ToInt32(HDLPF.GetTimeFromString(strTmp, ':'));
                    arayTmp[10] = Convert.ToByte(intTmp / 256);
                    arayTmp[11] = Convert.ToByte(intTmp % 256);
                    if (CsConst.mySends.AddBufToSndList(arayTmp, 0x1370, SubNetID, DevID, false, true, true, false) == true)
                    {
                        CsConst.myRevBuf = new byte[1200];
                        System.Threading.Thread.Sleep(1);
                    }
                    System.Threading.Thread.Sleep(100);
                }
            }
            catch
            {
            }
            btnSave5.Enabled = true;
            Cursor.Current = Cursors.Default;
        }

        private void btnSaveAndClose1_Click(object sender, EventArgs e)
        {
            btnSaveCommand_Click(null, null);
            this.Close();
        }

        private void btnSaveAndClose2_Click(object sender, EventArgs e)
        {
            btnSaveUV_Click(null, null);
            this.Close();
        }

        private void btnSaveAndClose3_Click(object sender, EventArgs e)
        {
            tslRead_Click(tbUpload, null);
            this.Close();
        }

        private void btnSave3_Click(object sender, EventArgs e)
        {
            tslRead_Click(tbUpload, null);
        }

        private void DgvKey_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (mmc == null) return;
                if (mmc.IRCodes == null) return;
                if (mmc.IRCodes.Count < 24) return;
                if ((e.RowIndex == -1) || (e.ColumnIndex == -1)) return;
                if (DgvKey[e.ColumnIndex, e.RowIndex].Value == null) DgvKey[e.ColumnIndex, e.RowIndex].Value = "";

                for (int i = 0; i < DgvKey.SelectedRows.Count; i++)
                {
                    string strTmp = "";
                    switch (e.ColumnIndex)
                    {
                        case 2:
                            strTmp = DgvKey[2, DgvKey.SelectedRows[i].Index].Value.ToString();
                            DgvKey[2, DgvKey.SelectedRows[i].Index].Value = HDLPF.IsRightStringMode(strTmp);
                            mmc.IRCodes[DgvKey.SelectedRows[i].Index].Remark = DgvKey[2, DgvKey.SelectedRows[i].Index].Value.ToString();
                            break;
                    }
                    DgvKey.SelectedRows[i].Cells[e.ColumnIndex].Value = DgvKey[e.ColumnIndex, e.RowIndex].Value.ToString();
                }
                if (e.ColumnIndex == 2)
                {
                    string strTmp = DgvKey[2, e.RowIndex].Value.ToString();
                    DgvKey[2, e.RowIndex].Value = HDLPF.IsRightStringMode(strTmp);
                    mmc.IRCodes[e.RowIndex].Remark = DgvKey[2, e.RowIndex].Value.ToString();
                }
            }
            catch
            {
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            btnSaveLoad_Click(null, null);
            this.Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            btnSaveSeq_Click(null, null);
            this.Close();
        }

        private void tsmCopy_Click(object sender, EventArgs e)
        {
            if (dgvTarget.RowCount <= 0) return;
            if (dgvTarget.SelectedRows.Count <= 0) return;
            setAllControlVisible(false);
            CsConst.RowObj = new List<object[]>();
            for (int i = 0; i < dgvTarget.SelectedRows.Count; i++)
            {
                object[] obj = new object[] { dgvTarget.SelectedRows[i].Cells[0].Value.ToString(),
                                              dgvTarget.SelectedRows[i].Cells[1].Value.ToString(),
                                              dgvTarget.SelectedRows[i].Cells[2].Value.ToString(),
                                              dgvTarget.SelectedRows[i].Cells[3].Value.ToString(),
                                              dgvTarget.SelectedRows[i].Cells[4].Value.ToString(),
                                              dgvTarget.SelectedRows[i].Cells[5].Value.ToString(),
                                              dgvTarget.SelectedRows[i].Cells[6].Value.ToString(),
                                              dgvTarget.SelectedRows[i].Cells[7].Value.ToString()};
                CsConst.RowObj.Add(obj);
            }
        }

        private void tsmPaste_Click(object sender, EventArgs e)
        {
            if (CsConst.RowObj == null || CsConst.RowObj.Count <= 0) return;
            if (dgvTarget.RowCount <= 0) return;
            if (dgvTarget.SelectedRows.Count <= 0) return;
            setAllControlVisible(false);
            for (int i = 0; i < CsConst.RowObj.Count; i++)
            {
                if (dgvTarget.CurrentRow.Index + i < dgvTarget.RowCount)
                {
                    for (int j = 1; j <= 7; j++)
                        dgvTarget.Rows[dgvTarget.CurrentRow.Index + i].Cells[j].Value = CsConst.RowObj[i][j];
                }
                else
                {
                    if (dgvTarget.RowCount < 8)
                    {
                        int num = Convert.ToInt32(dgvTarget[0, dgvTarget.RowCount - 1].Value.ToString());
                        object[] obj = new object[] { (num + 1).ToString(), CsConst.RowObj[i][1], CsConst.RowObj[i][2], CsConst.RowObj[i][3], CsConst.RowObj[i][4],
                                                  CsConst.RowObj[i][5],CsConst.RowObj[i][6],CsConst.RowObj[i][7]};
                        dgvTarget.Rows.Add(obj);
                    }
                }
            }
            dgvTarget.Refresh();
        }

        private void delirall_Click(object sender, EventArgs e)
        {

        }

        private void delirselect_Click(object sender, EventArgs e)
        {

        }

        private void DgvKey_DoubleClick(object sender, EventArgs e)
        {
            if (DgvKey.RowCount == 0) return;
            if (e == null) return;
            if (DgvKey.CurrentCell.RowIndex == -1) return;
            if (mmc.IRCodes == null) return;

            mmc.IRCodes[DgvKey.CurrentCell.RowIndex].IRLength = 0;
            DgvKey[1, DgvKey.CurrentCell.RowIndex].Value = CsConst.mstrInvalid;
            DgvKey[2, DgvKey.CurrentCell.RowIndex].Value = "";
        }

    }
}
