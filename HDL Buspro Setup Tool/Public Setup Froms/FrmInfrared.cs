using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;

namespace HDL_Buspro_Setup_Tool
{
    public partial class FrmInfrared : Form
    {
        public int DIndex = 0;
        private string strName;
        private int DeviceType;
        private DLP oPanel;
        private EnviroPanel oEnviroPanel;
        private byte SubnetID = 0;
        private byte DeviceID = 0;
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
        private object MyActiveObj;
        private byte SelACNO;
        public FrmInfrared()
        {
            InitializeComponent();
        }

        public FrmInfrared(string strname, int devicetype, object obj,byte selacno)
        {
            InitializeComponent();
            this.MyActiveObj = obj;
            this.DeviceType = devicetype;
            this.strName = strname;
            this.SelACNO = selacno;
            #region
            oPanel = null;
            oEnviroPanel = null;
            if (MyActiveObj is Panel)
            {
                if (CsConst.myPanels != null)
                {
                    foreach (Panel oTmp in CsConst.myPanels)
                    {
                        if (oTmp.DIndex == (MyActiveObj as DLP).DIndex)
                        {
                            oPanel =(DLP)oTmp;
                            this.DIndex = oPanel.DIndex;
                            break;
                        }
                    }
                }
            }
            else if (MyActiveObj is EnviroPanel)
            {
                if (CsConst.myEnviroPanels != null)
                {
                    foreach (EnviroPanel oTmp in CsConst.myEnviroPanels)
                    {
                        if (oTmp.DIndex == (MyActiveObj as EnviroPanel).DIndex)
                        {
                            oEnviroPanel = oTmp;
                            this.DIndex = oEnviroPanel.DIndex;
                            break;
                        }
                    }
                }
            }
            #endregion
            string strDevName = strname.Split('\\')[0].ToString();
            SubnetID = Convert.ToByte(strDevName.Split('-')[0]);
            DeviceID = Convert.ToByte(strDevName.Split('-')[1]);
            #region

            addUVControlType();
            cbPanleControl.Items.Clear();
            HDLSysPF.getPanlControlType(cbPanleControl, DeviceType);
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
        }

        private void addUVControlType()
        {
            OleDbDataReader dr = null;
            try
            {
                string str = "";
                cbControlType.Items.Clear();
                string sql = "select * from defKeyFunType ";
                sql = sql + "where KeyFunType = 0 or KeyFunType = 85 or KeyFunType = 88 or KeyFunType = 89";
                dr = DataModule.SearchAResultSQLDB(sql);
                if (dr != null)
                {
                    while (dr.Read())
                    {
                        if (CsConst.iLanguageId == 1) str = dr.GetValue(1).ToString();
                        else if (CsConst.iLanguageId == 0) str = dr.GetValue(2).ToString();
                        cbControlType.Items.Add(str);
                    }
                    dr.Close();
                }
            }
            catch
            {
                dr.Close();
            }
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

        private void txtFrm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar) || e.KeyChar == 8)
                e.Handled = false;
            else
                e.Handled = true;
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
                                txtbox3.Text = HDLPF.IsNumStringMode(str, 1, 24);
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
            if (dgvTarget.SelectedRows.Count > 1)
            {
                for (int i = 0; i < dgvTarget.SelectedRows.Count; i++)
                {
                    if (dgvTarget.SelectedRows[i].Cells[3].Value.ToString() == dgvTarget[3, index].Value.ToString() &&
                        dgvTarget.SelectedRows[i].Cells[4].Value.ToString() == dgvTarget[4, index].Value.ToString() &&
                        dgvTarget.SelectedRows[i].Cells[5].Value.ToString() == dgvTarget[5, index].Value.ToString())
                        dgvTarget.SelectedRows[i].Cells[6].Value = strTmp;
                }
            }
        }

        void ModifyMultilinesIfNeeds(string strTmp, int ColumnIndex)
        {
            if (dgvTarget.SelectedRows == null || dgvTarget.SelectedRows.Count == 0) return;
            if (strTmp == null) strTmp = "";
            // change the value in selected more than one line
            if (dgvTarget.SelectedRows.Count > 1)
            {
                for (int i = 0; i < dgvTarget.SelectedRows.Count; i++)
                {
                    dgvTarget.SelectedRows[i].Cells[ColumnIndex].Value = strTmp;
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
                dgvTarget[6, index].Value = str + "(" + CsConst.WholeTextsList[2524].sDisplayName + ")";
                if (dgvTarget.SelectedRows == null || dgvTarget.SelectedRows.Count == 0) return;
                string strTmp = dgvTarget[6, index].Value.ToString();
                if (strTmp == null) strTmp = "N/A";
                if (dgvTarget.SelectedRows.Count > 1)
                {
                    for (int i = 0; i < dgvTarget.SelectedRows.Count; i++)
                    {
                        if (dgvTarget.SelectedRows[i].Cells[3].Value.ToString() == cbControlType.Items[4].ToString() ||
                            dgvTarget.SelectedRows[i].Cells[3].Value.ToString() == cbControlType.Items[9].ToString())
                            dgvTarget.SelectedRows[i].Cells[6].Value = strTmp;
                    }
                }
            }
        }

        void txtbox2_TextChanged(object sender, EventArgs e)
        {
            if (dgvTarget.CurrentRow.Index < 0) return;
            if (dgvTarget.RowCount <= 0) return;
            int index = dgvTarget.CurrentRow.Index;
            string str = txtbox2.Text;
            if (str.Contains("(")) str = str.Split('(')[0].ToString();

            int ID = ButtonControlType.ConvertorKeyControlTypeToPublicModeGroup(cbControlType.Text);
            #region
            if (txtbox2.Text.Length > 0)
            {
                if (ID == 85 || ID == 100)//场景  广播场景
                {
                    txtbox2.Text = HDLPF.IsNumStringMode(str, 0, 255);
                    dgvTarget[5, index].Value = txtbox2.Text + "(" + CsConst.WholeTextsList[2511].sDisplayName + ")";
                }
                else if (ID == 86)//序列
                {
                    txtbox2.Text = HDLPF.IsNumStringMode(str, 0, 99);
                    dgvTarget[5, index].Value = txtbox2.Text + "(" + CsConst.WholeTextsList[2512].sDisplayName+ ")";
                }
                else if (ID == 89 || ID == 101)//单路调节 广播回路
                {
                    txtbox2.Text = HDLPF.IsNumStringMode(str, 0, 100);
                    dgvTarget[5, index].Value = txtbox2.Text + "(" + CsConst.mstrINIDefault.IniReadValue("public", "00011", "") + ")";
                }
                else if (ID == 94)//GPRS控制
                {
                    txtbox2.Text = HDLPF.IsNumStringMode(str, 1, 24);
                    dgvTarget[5, index].Value = txtbox2.Text + "(" + CsConst.mstrINIDefault.IniReadValue("public", "99864", "") + ")";
                }
                else if (ID == 95)//面板控制
                {
                    if (cbPanleControl.SelectedIndex == 3 || cbPanleControl.SelectedIndex == 4)//背光亮度 状态灯亮度
                    {
                        txtbox2.Text = HDLPF.IsNumStringMode(str, 0, 100);
                        dgvTarget[5, index].Value = txtbox2.Text + "(" + CsConst.WholeTextsList[2524].sDisplayName + ")";
                    }
                    else if (cbPanleControl.SelectedIndex == 15 || cbPanleControl.SelectedIndex == 16 ||//空调升高温度 空调降低温度
                        cbPanleControl.SelectedIndex == 23 || cbPanleControl.SelectedIndex == 24)//地热身高温度 地热降低温度
                    {
                        txtbox2.Text = HDLPF.IsNumStringMode(str, 1, 255);
                        dgvTarget[5, index].Value = txtbox2.Text;

                    }
                }
                else if (ID == 103)//音乐播放
                {
                    if (cbAudioControl.SelectedIndex == 5)//歌曲播放
                    {
                        txtbox2.Text = HDLPF.IsNumStringMode(str, 0, 255);
                        dgvTarget[5, index].Value = txtbox2.Text + "(" + CsConst.mstrINIDefault.IniReadValue("public", "00046", "") + ")";
                    }
                }
                txtbox2.SelectionStart = txtbox2.Text.Length;
            }
            #endregion
            if (dgvTarget.SelectedRows == null || dgvTarget.SelectedRows.Count == 0) return;
            string strTmp = dgvTarget[5, index].Value.ToString();
            if (strTmp == null) strTmp = "N/A";
            if (dgvTarget.SelectedRows.Count > 1)
            {
                for (int i = 0; i < dgvTarget.SelectedRows.Count; i++)
                {
                    if (dgvTarget.SelectedRows[i].Cells[3].Value.ToString() == dgvTarget[3, index].Value.ToString() &&
                        dgvTarget.SelectedRows[i].Cells[4].Value.ToString() == dgvTarget[4, index].Value.ToString())
                        dgvTarget.SelectedRows[i].Cells[5].Value = strTmp;
                }
            }
        }

        void txtbox1_TextChanged(object sender, EventArgs e)
        {
            if (dgvTarget.CurrentRow.Index < 0) return;
            if (dgvTarget.RowCount <= 0) return;
            int index = dgvTarget.CurrentRow.Index;
            string str = txtbox1.Text;
            if (str.Contains("(")) str = str.Split('(')[0].ToString();
            int ID = ButtonControlType.ConvertorKeyControlTypeToPublicModeGroup(cbControlType.Text);
            #region
            if (txtbox1.Text.Length > 0)
            {
                if (ID == 85 || ID == 86)//场景 序列
                {
                    txtbox1.Text = HDLPF.IsNumStringMode(str, 1, 48);
                    dgvTarget[4, index].Value = txtbox1.Text + "(" + CsConst.WholeTextsList[2510].sDisplayName + ")";
                }
                else if (ID == 88)//通用开关
                {
                    txtbox1.Text = HDLPF.IsNumStringMode(str, 1, 255);
                    dgvTarget[4, index].Value = txtbox1.Text + "(" + CsConst.WholeTextsList[2513].sDisplayName + ")";
                }
                else if (ID == 89)//单路调节
                {
                    txtbox1.Text = HDLPF.IsNumStringMode(str, 1, 240);
                    dgvTarget[4, index].Value = txtbox1.Text + "(" + CsConst.WholeTextsList[934].sDisplayName + ")";
                }
                else if (ID == 92)//窗帘开关
                {
                    txtbox1.Text = HDLPF.IsNumStringMode(str, 1, 99);
                    dgvTarget[4, index].Value = txtbox1.Text + "(" + CsConst.mstrINIDefault.IniReadValue("public", "99844", "") + ")";
                }
                else if (ID == 102)//消防模块
                {
                    txtbox1.Text = HDLPF.IsNumStringMode(str, 1, 8);
                    dgvTarget[4, index].Value = txtbox1.Text + "(" + CsConst.WholeTextsList[2510].sDisplayName + ")";
                }
                txtbox1.SelectionStart = txtbox1.Text.Length;
            }
            #endregion

            if (dgvTarget.SelectedRows == null || dgvTarget.SelectedRows.Count == 0) return;
            string strTmp = dgvTarget[4, index].Value.ToString();
            if (strTmp == null) strTmp = "N/A";
            if (dgvTarget.SelectedRows.Count > 1)
            {
                for (int i = 0; i < dgvTarget.SelectedRows.Count; i++)
                {
                    if (dgvTarget.SelectedRows[i].Cells[3].Value.ToString() == dgvTarget[3, index].Value.ToString())
                        dgvTarget.SelectedRows[i].Cells[4].Value = strTmp;
                }
            }
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
                ModifyMultilinesIfNeeds(dgvTarget[2, index].Value.ToString(), 2);
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
                ModifyMultilinesIfNeeds(dgvTarget[1, index].Value.ToString(), 1);
            }
        }

        void cbbox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dgvTarget.CurrentRow.Index < 0) return;
            if (dgvTarget.RowCount <= 0) return;
            int index = dgvTarget.CurrentRow.Index;
            string str = dgvTarget[6, index].Value.ToString();
            if (str.Contains("(")) str = str.Split('(')[0].ToString();
            if (cbPanleControl.Visible && cbPanleControl.SelectedIndex == 22)
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
            if (dgvTarget.SelectedRows.Count > 1)
            {
                for (int i = 0; i < dgvTarget.SelectedRows.Count; i++)
                {
                    if (dgvTarget.SelectedRows[i].Cells[3].Value.ToString() == dgvTarget[3, index].Value.ToString() &&
                        dgvTarget.SelectedRows[i].Cells[4].Value.ToString() == dgvTarget[4, index].Value.ToString() &&
                        dgvTarget.SelectedRows[i].Cells[5].Value.ToString() == dgvTarget[5, index].Value.ToString())
                        dgvTarget.SelectedRows[i].Cells[6].Value = strTmp;
                }
            }
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
            int ID = ButtonControlType.ConvertorKeyControlTypeToPublicModeGroup(cbControlType.Text);
            #region
            if (ID == 88 || ID == 92)//通用开关 窗帘开关
            {
                dgvTarget[5, index].Value = cbbox2.Text + "(" + CsConst.WholeTextsList[2529].sDisplayName + ")";
                if (cbControlType.SelectedIndex == 5)
                {
                    if (cbbox2.SelectedIndex > 2)
                    {
                        txtbox1.Text = "17";
                        txtbox1_TextChanged(null, null);
                        txtbox1.Visible = false;
                    }
                    else
                    {
                        addcontrols(4, index, txtbox1);
                    }
                }
            }
            else if (ID == 95)//面板控制
            {
                if (cbPanleControl.SelectedIndex == 1 || cbPanleControl.SelectedIndex == 2 ||
                    cbPanleControl.SelectedIndex == 5 || cbPanleControl.SelectedIndex == 7 ||
                    cbPanleControl.SelectedIndex == 8 || cbPanleControl.SelectedIndex == 12 ||
                    cbPanleControl.SelectedIndex == 21)
                {
                    dgvTarget[5, index].Value = cbbox2.Text + "(" + CsConst.WholeTextsList[2529].sDisplayName + ")";
                }
                else if (cbPanleControl.SelectedIndex == 17 || cbPanleControl.SelectedIndex == 18 ||
                         cbPanleControl.SelectedIndex == 19 || cbPanleControl.SelectedIndex == 20)
                {
                    dgvTarget[5, index].Value = cbbox2.Text + "(" + CsConst.mstrINIDefault.IniReadValue("public", "99874", "") + ")";
                }
                else if (cbPanleControl.SelectedIndex == 13 || cbPanleControl.SelectedIndex == 22)
                {
                    dgvTarget[5, index].Value = cbbox2.Text + "(" + CsConst.mstrINIDefault.IniReadValue("public", "99876", "") + ")";
                }
                else if (cbPanleControl.SelectedIndex == 14)
                {
                    dgvTarget[5, index].Value = cbbox2.Text + "(" + CsConst.mstrINIDefault.IniReadValue("public", "99875", "") + ")";
                }
                else if (cbPanleControl.SelectedIndex == 6 || cbPanleControl.SelectedIndex == 9 ||
                         cbPanleControl.SelectedIndex == 10 || cbPanleControl.SelectedIndex == 11)
                {
                    dgvTarget[5, index].Value = cbbox2.Text;
                }
            }
            else if (ID == 102)//消防模块
            {
                dgvTarget[5, index].Value = cbbox2.Text;
            }
            else if (ID == 103)//音乐模块
            {
                if (cbAudioControl.SelectedIndex == 0 || cbAudioControl.SelectedIndex == 1 ||
                    cbAudioControl.SelectedIndex == 2 || cbAudioControl.SelectedIndex == 3 ||
                    cbAudioControl.SelectedIndex == 4 || cbAudioControl.SelectedIndex == 6 ||
                    cbAudioControl.SelectedIndex == 7)
                {
                    dgvTarget[5, index].Value = cbbox2.Text;
                }
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
                            addcontrols(6, index, txtbox3);
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
                        addcontrols(6, index, cbbox3);
                    }
                }
                else if (cbAudioControl.SelectedIndex == 6 || cbAudioControl.SelectedIndex == 7)//直接源播放 广播播放
                {
                    txtbox3.Visible = true;
                }
            }
            else if (cbPanleControl.Visible)
            {
                if ((cbPanleControl.SelectedIndex == 6 || cbPanleControl.SelectedIndex == 9 ||//面板页面锁  按键锁
                     cbPanleControl.SelectedIndex == 10 || cbPanleControl.SelectedIndex == 11 ||//控制按键状态 控制面板按键
                     cbPanleControl.SelectedIndex == 21 || cbPanleControl.SelectedIndex == 22))//地热开关 地热模式
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
            if (dgvTarget.SelectedRows.Count > 1)
            {
                for (int i = 0; i < dgvTarget.SelectedRows.Count; i++)
                {
                    if (dgvTarget.SelectedRows[i].Cells[3].Value.ToString() == dgvTarget[3, index].Value.ToString() &&
                        dgvTarget.SelectedRows[i].Cells[4].Value.ToString() == dgvTarget[4, index].Value.ToString())
                        dgvTarget.SelectedRows[i].Cells[5].Value = strTmp;
                }
            }
            #endregion
        }

        void cbbox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dgvTarget.CurrentRow.Index < 0) return;
            if (dgvTarget.RowCount <= 0) return;
            int index = dgvTarget.CurrentRow.Index;
            string str = dgvTarget[4, index].Value.ToString();
            if (str.Contains("(")) str = str.Split('(')[0].ToString();
            int ID = ButtonControlType.ConvertorKeyControlTypeToPublicModeGroup(cbControlType.Text);
            if (ID == 94)//GPRS控制
            {
                dgvTarget[4, index].Value = cbbox1.Text;
            }
            #region
            if (dgvTarget.SelectedRows == null || dgvTarget.SelectedRows.Count == 0) return;
            string strTmp = dgvTarget[4, index].Value.ToString();
            if (strTmp == null) strTmp = "N/A";
            if (dgvTarget.SelectedRows.Count > 1)
            {
                for (int i = 0; i < dgvTarget.SelectedRows.Count; i++)
                {
                    if (dgvTarget.SelectedRows[i].Cells[3].Value.ToString() == dgvTarget[3, index].Value.ToString())
                        dgvTarget.SelectedRows[i].Cells[4].Value = strTmp;
                }
            }
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

                addcontrols(5, index, cbbox2);
                dgvTarget[6, index].Value = "N/A";
                #endregion
            }
            else if (cbAudioControl.SelectedIndex == 5)//歌曲播放
            {
                #region
                txtbox2.Text = str2;
                addcontrols(5, index, txtbox2);

                txtbox3.Text = str3;
                addcontrols(6, index, txtbox3);
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
                addcontrols(5, index, cbbox2);
                txtbox3.Text = str3;
                addcontrols(6, index, txtbox3);
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
            if (cbPanleControl.SelectedIndex == 0)//无效
            {
                #region
                dgvTarget[5, index].Value = "N/A";
                dgvTarget[6, index].Value = "N/A";
                #endregion
            }
            else if (cbPanleControl.SelectedIndex == 1 || cbPanleControl.SelectedIndex == 2 ||//红外接收使能 背光开关
                     cbPanleControl.SelectedIndex == 5 || cbPanleControl.SelectedIndex == 7 ||//面板全锁  空调锁
                     cbPanleControl.SelectedIndex == 8 || cbPanleControl.SelectedIndex == 12 ||//配置页面锁 空调开关
                     cbPanleControl.SelectedIndex == 13 || cbPanleControl.SelectedIndex == 14)//空调模式  空调风速
            {
                #region
                cbbox2.Items.Clear();
                if (cbPanleControl.SelectedIndex == 13)
                {
                    for (int i = 0; i < 5; i++)
                        cbbox2.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "0005" + i.ToString(), ""));
                }
                else if (cbPanleControl.SelectedIndex == 14)
                {
                    for (int i = 0; i < 4; i++)
                        cbbox2.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "0006" + i.ToString(), ""));
                }
                else
                {
                    cbbox2.Items.Add(CsConst.Status[0]);
                    cbbox2.Items.Add(CsConst.Status[1]);
                }
                addcontrols(5, index, cbbox2);
                dgvTarget[6, index].Value = "N/A";
                #endregion
            }
            else if (cbPanleControl.SelectedIndex == 3 || cbPanleControl.SelectedIndex == 4 ||//背光灯亮度，状态灯亮度
                     cbPanleControl.SelectedIndex == 15 || cbPanleControl.SelectedIndex == 16)//空调升高温度 空调降低温度
            {
                #region
                txtbox2.Text = str2;
                addcontrols(5, index, txtbox2);
                dgvTarget[6, index].Value = "N/A";
                #endregion
            }
            else if (cbPanleControl.SelectedIndex == 6 || cbPanleControl.SelectedIndex == 9 ||//面板页面锁  按键锁
                     cbPanleControl.SelectedIndex == 10 || cbPanleControl.SelectedIndex == 11 ||//控制按键状态 控制面板按键
                     cbPanleControl.SelectedIndex == 21 || cbPanleControl.SelectedIndex == 22)//地热开关 地热模式
            {
                #region
                cbbox2.Items.Clear();
                cbbox3.Items.Clear();
                if (cbPanleControl.SelectedIndex == 6)//面板页面锁
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
                else if (cbPanleControl.SelectedIndex == 9)//按键锁
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
                else if (cbPanleControl.SelectedIndex == 10)//控制按键状态
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
                else if (cbPanleControl.SelectedIndex == 11)//控制面板按键
                {
                    #region
                    cbbox2.Items.Add(CsConst.WholeTextsList[1775].sDisplayName);
                    for (int i = 1; i <= 32; i++)
                        cbbox2.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99867", "") + i.ToString());

                    cbbox3.Items.Add(CsConst.WholeTextsList[1775].sDisplayName);
                    cbbox3.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99866", ""));
                    #endregion
                }
                else if (cbPanleControl.SelectedIndex == 21)//地热开关
                {
                    #region
                    cbbox2.Items.Add(CsConst.Status[0]);
                    cbbox2.Items.Add(CsConst.Status[1]);

                    for (int i = 1; i <= 8; i++)
                        cbbox3.Items.Add(i.ToString());
                    cbbox3.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00049", ""));
                    #endregion
                }
                else if (cbPanleControl.SelectedIndex == 22)//地热模式
                {
                    #region
                    for (int i = 0; i <= 4; i++)
                        cbbox2.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "0007" + i.ToString(), ""));

                    for (int i = 1; i <= 8; i++)
                        cbbox3.Items.Add(i.ToString());
                    cbbox3.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00049", ""));
                    #endregion
                }
                addcontrols(5, index, cbbox2);
                addcontrols(6, index, cbbox3);
                #endregion
            }
            else if (cbPanleControl.SelectedIndex == 17 || cbPanleControl.SelectedIndex == 18 ||//空调制冷温度 空调制热温度
                     cbPanleControl.SelectedIndex == 19 || cbPanleControl.SelectedIndex == 20)//空调自动温度 //空调除湿温度 
            {
                #region
                cbbox2.Items.Clear();
                for (int i = 0; i < 31; i++)
                    cbbox2.Items.Add(i.ToString() + "C");
                for (int i = 32; i < 87; i++)
                    cbbox2.Items.Add(i.ToString() + "F");
                addcontrols(5, index, cbbox2);
                dgvTarget[6, index].Value = "N/A";
                #endregion
            }
            else if (cbPanleControl.SelectedIndex == 23 || cbPanleControl.SelectedIndex == 24)//地热身高温度 地热降低温度
            {
                #region
                txtbox2.Text = str2;
                addcontrols(5, index, txtbox2);
                cbbox3.Items.Clear();
                for (int i = 1; i <= 8; i++)
                    cbbox3.Items.Add(i.ToString());
                cbbox3.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00049", ""));
                addcontrols(6, index, cbbox3);
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
            string strType = cbControlType.Text;
            string str1 = dgvTarget[4, index].Value.ToString();
            string str2 = dgvTarget[5, index].Value.ToString();
            string str3 = dgvTarget[6, index].Value.ToString();
            if (str1.Contains('(')) str1 = str1.Split('(')[0].ToString();
            if (str2.Contains('(')) str2 = str2.Split('(')[0].ToString();
            if (str3.Contains('(')) str3 = str3.Split('(')[0].ToString();
            int ID = ButtonControlType.ConvertorKeyControlTypeToPublicModeGroup(strType);
            if (ID == 0)//无效
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
            else if (ID == 85 || ID == 86)//场景 序列
            {
                #region
                txtbox1.Text = str1;
                addcontrols(4, index, txtbox1);

                txtbox2.Text = str2;
                addcontrols(5, index, txtbox2);
                dgvTarget[6, index].Value = "N/A";
                #endregion
            }
            else if (ID == 88 || ID == 92 ||//通用开关 窗帘开关 
                 ID == 102)                                        //消防模块
            {
                #region
                txtbox1.Text = str1;
                addcontrols(4, index, txtbox1);

                cbbox2.Items.Clear();
                if (ID == 88)
                {
                    cbbox2.Items.Add(CsConst.Status[0]);
                    cbbox2.Items.Add(CsConst.Status[1]);
                }
                else if (ID == 92)
                {
                    cbbox2.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00036", ""));
                    cbbox2.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00037", ""));
                    cbbox2.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00038", ""));
                    for (int i = 1; i <= 100; i++)
                        cbbox2.Items.Add(i.ToString() + " %");
                }
                else if (ID == 102)
                {
                    for (int i = 0; i < 10; i++)
                        cbbox2.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "0008" + i.ToString(), ""));
                }
                addcontrols(5, index, cbbox2);
                dgvTarget[6, index].Value = "N/A";
                #endregion
            }
            else if (ID == 89)//单路调节
            {
                #region
                txtbox1.Text = str1;
                addcontrols(4, index, txtbox1);

                txtbox2.Text = str2;
                addcontrols(5, index, txtbox2);


                if (!str3.Contains(":"))
                    txtSeries.Text = "0:0";
                else
                    txtSeries.Text = HDLPF.GetTimeFromString(str3, ':');
                addcontrols(6, index, txtSeries);
                #endregion
            }
            else if (ID == 94)//GPRS控制
            {
                #region
                cbbox1.Items.Clear();
                cbbox1.Items.Add(CsConst.WholeTextsList[1775].sDisplayName);
                cbbox1.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99862", ""));
                cbbox1.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99863", ""));
                addcontrols(4, index, cbbox1);

                txtbox2.Text = str2;
                addcontrols(5, index, txtbox2);
                dgvTarget[6, index].Value = "N/A";
                #endregion
            }
            else if (ID == 95)//面板控制
            {
                #region
                addcontrols(4, index, cbPanleControl);
                #endregion
            }
            else if (ID == 100 || ID == 101)//广播场景 广播回路
            {
                #region
                if (ID == 100)
                {
                    dgvTarget[4, index].Value = CsConst.WholeTextsList[2566].sDisplayName;
                    dgvTarget[6, index].Value = "N/A";
                }
                else if (ID == 101)
                {
                    dgvTarget[4, index].Value = CsConst.WholeTextsList[2567].sDisplayName;
                    if (!str3.Contains(":"))
                        txtSeries.Text = "0:0";
                    else
                        txtSeries.Text = HDLPF.GetTimeFromString(str3, ':');
                    addcontrols(6, index, txtSeries);
                }
                txtbox2.Text = str2;
                addcontrols(5, index, txtbox2);
                #endregion
            }
            else if (ID == 103)//音乐播放
            {
                #region
                addcontrols(4, index, cbAudioControl);
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
            ModifyMultilinesIfNeeds(dgvTarget[3, index].Value.ToString(), 3);
        }

        private void FrmInfrared_Load(object sender, EventArgs e)
        {
            cbType.Items.Clear();
            cbType.Items.AddRange(CsConst.mstrINIDefault.IniReadValuesInALlSectionStr("IRControlType"));
            if (cbType.Items.Count > 0 && cbType.SelectedIndex < 0)
                cbType.SelectedIndex = 0;
            cbType_SelectedIndexChanged(null, null);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (dgvTarget.RowCount == 0) return;
            Cursor.Current = Cursors.WaitCursor;
            btnSave.Enabled = false;
            setAllControlVisible(false);
            int intTmp = 0;
            if (cbType.SelectedIndex == 0)
            {
                intTmp = 0;
            }
            else if (1 <= cbType.SelectedIndex && cbType.SelectedIndex <= 8)
            {
                intTmp = 1 + ((cbType.SelectedIndex - 1) * 55);
            }
            else if (9 <= cbType.SelectedIndex && cbType.SelectedIndex <= 12)
            {
                intTmp = 432 + cbType.SelectedIndex;
            }
            else if (13 <= cbType.SelectedIndex && cbType.SelectedIndex <= 25)
            {
                intTmp = 445 + ((cbType.SelectedIndex - 13) * 55);
            }
            else if (26 <= cbType.SelectedIndex && cbType.SelectedIndex <= 29)
            {
                intTmp = 1134 + cbType.SelectedIndex;
            }
            else if (30 <= cbType.SelectedIndex && cbType.SelectedIndex <= 34)
            {
                intTmp = 1164 + ((cbType.SelectedIndex - 30) * 55);
            }
            else
            {
                intTmp = 1404 + cbType.SelectedIndex;
            }
            for (int i = 0; i < dgvTarget.RowCount; i++)
            {
                byte[] arayTmp = new byte[9];
                if (oEnviroPanel != null)
                    arayTmp = new byte[10];
        
                arayTmp[0] = Convert.ToByte(intTmp / 256);
                arayTmp[1] = Convert.ToByte(intTmp % 256);
                if (oEnviroPanel != null)
                    arayTmp[9] = Convert.ToByte(SelACNO - 1);
                arayTmp[3] = Convert.ToByte(dgvTarget[1, i].Value.ToString());
                arayTmp[4] = Convert.ToByte(dgvTarget[2, i].Value.ToString());
                arayTmp[2] = ButtonControlType.ConvertorKeyControlTypeToPublicModeGroup(dgvTarget[3, i].Value.ToString());

                string str1 = dgvTarget[4, i].Value.ToString();
                string str2 = dgvTarget[5, i].Value.ToString();
                string str3 = dgvTarget[6, i].Value.ToString();
                if (str1.Contains("(")) str1 = str1.Split('(')[0].ToString();
                if (str2.Contains("(")) str2 = str2.Split('(')[0].ToString();
                if (str3.Contains("(")) str3 = str3.Split('(')[0].ToString();
                if (arayTmp[2] == 85 )//场景 序列
                {
                    #region
                    arayTmp[5] = Convert.ToByte(str1);
                    arayTmp[6] = Convert.ToByte(str2);
                    #endregion
                }
                else if (arayTmp[2] == 88)//通用开关
                {
                    #region
                    arayTmp[5] = Convert.ToByte(str1);
                    if (str2 == CsConst.Status[0])
                        arayTmp[6] = 0;
                    else if (str2 == CsConst.Status[1])
                        arayTmp[6] = 255;
                    #endregion
                }
                else if (arayTmp[2] == 89)//单路调节
                {
                    #region
                    arayTmp[5] = Convert.ToByte(str1);
                    arayTmp[6] = Convert.ToByte(str2);
                    int intTime = Convert.ToInt32(HDLPF.GetTimeFromString(str3, ':'));
                    arayTmp[7] = Convert.ToByte(intTime / 256);
                    arayTmp[8] = Convert.ToByte(intTime % 256);
                    #endregion
                }
                intTmp = intTmp + 1;
                if (CsConst.mySends.AddBufToSndList(arayTmp, 0xE0F6, SubnetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                {
                    
                    HDLUDP.TimeBetwnNext(20);
                }
                else break;
            }
            btnSave.Enabled = true;
            Cursor.Current = Cursors.Default;
        }

        private void dgvTarget_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                txtSub.Text = dgvTarget[1, e.RowIndex].Value.ToString();
                addcontrols(1, e.RowIndex, txtSub);

                txtDev.Text = dgvTarget[2, e.RowIndex].Value.ToString();
                addcontrols(2, e.RowIndex, txtDev);

                cbControlType.Text = dgvTarget[3, e.RowIndex].Value.ToString();
                addcontrols(3, e.RowIndex, cbControlType);

                txtSub_TextChanged(txtSub, null);
                txtDev_TextChanged(txtDev, null);
                cbControlType_SelectedIndexChanged(cbControlType, null);
            }
        }

        private void addcontrols(int col, int row, Control con)
        {
            con.Show();
            con.Visible = true;
            Rectangle rect = dgvTarget.GetCellDisplayRectangle(col, row, true);
            con.Size = rect.Size;
            con.Top = rect.Top;
            con.Left = rect.Left;
        }

        private void SetParams(ref string strType, ref string str1, ref string str2, ref string str3, string str4)
        {
            int ID = ButtonControlType.ConvertorKeyControlTypeToPublicModeGroup(strType);
            if (ID == 0)//无效
            {
                #region
                str1 = "N/A";
                str2 = "N/A";
                str3 = "N/A";
                #endregion
            }
            else if (ID == 85)//场景
            {
                #region
                str1 = str1 + "(" + CsConst.WholeTextsList[2510].sDisplayName + ")";
                str2 = str2 + "(" + CsConst.WholeTextsList[2511].sDisplayName + ")";
                str3 = "N/A";
                #endregion
            }
            else if (ID == 86)//序列
            {
                #region
                str1 = str1 + "(" + CsConst.WholeTextsList[2510].sDisplayName + ")";
                str2 = str2 + "(" + CsConst.WholeTextsList[2512].sDisplayName+ ")";
                str3 = "N/A";
                #endregion
            }
            else if (ID == 88)//通用开关
            {
                #region
                str1 = str1 + "(" + CsConst.WholeTextsList[2513].sDisplayName + ")";
                if (str2 == "0") str2 = CsConst.Status[0] + "(" + CsConst.WholeTextsList[2529].sDisplayName + ")";
                else if (str2 == "255") str2 = CsConst.Status[1] + "(" + CsConst.WholeTextsList[2529].sDisplayName + ")";
                str3 = "N/A";
                #endregion
            }
            else if (ID == 89)//单路调节
            {
                #region
                str1 = str1 + "(" + CsConst.WholeTextsList[934].sDisplayName + ")";
                str2 = str2 + "(" + CsConst.WholeTextsList[2524].sDisplayName + ")";
                int intTmp = Convert.ToInt32(str3) * 256 + Convert.ToInt32(str4);
                str3 = HDLPF.GetStringFromTime(intTmp, ":") + "(" + CsConst.WholeTextsList[2525].sDisplayName + ")";
                #endregion
            }
            else if (ID == 100)//广播场景
            {
                #region
                str1 = CsConst.WholeTextsList[2566].sDisplayName;
                str2 = str2 + "(" + CsConst.WholeTextsList[2511].sDisplayName + ")";
                str3 = "N/A";
                #endregion
            }
            else if (ID == 101)//广播回路
            {
                #region
                str1 = CsConst.WholeTextsList[2567].sDisplayName;
                str2 = str2 + "(" + CsConst.WholeTextsList[2524].sDisplayName + ")";
                int intTmp = Convert.ToInt32(str3) * 256 + Convert.ToInt32(str4);
                str3 = HDLPF.GetStringFromTime(intTmp, ":") + "(" + CsConst.WholeTextsList[2525].sDisplayName + ")";
                #endregion
            }
            else if (ID == 92)//窗帘开关
            {
                #region
                str1 = str1 + "(" + CsConst.mstrINIDefault.IniReadValue("public", "99844", "") + ")";
                if (str1 == "17")
                {
                    str2 = (Convert.ToInt32(str2)).ToString() + "%" + "(" + CsConst.WholeTextsList[2529].sDisplayName + ")";
                }
                else
                {
                    if (str2 == "0") str2 = CsConst.mstrINIDefault.IniReadValue("public", "00036", "") + "(" + CsConst.WholeTextsList[2529].sDisplayName + ")";
                    else if (str2 == "1") str2 = CsConst.mstrINIDefault.IniReadValue("public", "00037", "") + "(" + CsConst.WholeTextsList[2529].sDisplayName + ")";
                    else if (str2 == "2") str2 = CsConst.mstrINIDefault.IniReadValue("public", "00038", "") + "(" + CsConst.WholeTextsList[2529].sDisplayName + ")";
                    else str2 = (Convert.ToInt32(str2)).ToString() + "%" + "(" + CsConst.WholeTextsList[2529].sDisplayName + ")";
                }
                str3 = "N/A";
                #endregion
            }
            else if (ID == 94)//GPRS
            {
                #region
                if (str1 == "1") str1 = CsConst.mstrINIDefault.IniReadValue("public", "99862", "");
                else if (str1 == "2") str1 = CsConst.mstrINIDefault.IniReadValue("public", "99863", "");
                else str1 = CsConst.WholeTextsList[1775].sDisplayName;
                str2 = str2 + "(" + CsConst.mstrINIDefault.IniReadValue("public", "99864", "") + ")";
                str3 = "N/A";
                #endregion
            }
            else if (ID == 95)//面板控制
            {
                #region
                str1 = HDLSysPF.InquirePanelControTypeStringFromDB(Convert.ToInt32(str1));
                if (str1 == cbPanleControl.Items[0].ToString())
                {
                    str2 = "N/A";
                    str3 = "N/A";
                }
                else if (str1 == cbPanleControl.Items[1].ToString() || str1 == cbPanleControl.Items[2].ToString() ||
                         str1 == cbPanleControl.Items[5].ToString() || str1 == cbPanleControl.Items[7].ToString() ||
                         str1 == cbPanleControl.Items[8].ToString() || str1 == cbPanleControl.Items[12].ToString() ||
                         str1 == cbPanleControl.Items[21].ToString())
                {
                    if (str2 == "0") str2 = CsConst.Status[0] + "(" + CsConst.WholeTextsList[2529].sDisplayName + ")";
                    else if (str2 == "1") str2 = CsConst.Status[1] + "(" + CsConst.WholeTextsList[2529].sDisplayName + ")";
                    str3 = "N/A";
                }
                else if (str1 == cbPanleControl.Items[3].ToString() || str1 == cbPanleControl.Items[4].ToString())
                {
                    str2 = str2 + "(" + CsConst.WholeTextsList[2524].sDisplayName + ")";
                    str3 = "N/A";
                }
                else if (str1 == cbPanleControl.Items[6].ToString() || str1 == cbPanleControl.Items[9].ToString() ||
                         str1 == cbPanleControl.Items[10].ToString() || str1 == cbPanleControl.Items[11].ToString())
                {
                    int intTmp = Convert.ToInt32(str2);
                    if (str1 == cbPanleControl.Items[6].ToString())
                    {
                        if (1 <= intTmp && intTmp <= 7) str2 = CsConst.mstrINIDefault.IniReadValue("public", "00048", "") + intTmp.ToString();
                        else if (intTmp == 255) str2 = CsConst.mstrINIDefault.IniReadValue("public", "00049", "");
                        else str2 = CsConst.WholeTextsList[1775].sDisplayName;
                    }
                    else if (str1 == cbPanleControl.Items[9].ToString())
                    {
                        if (intTmp == 255) str2 = CsConst.mstrINIDefault.IniReadValue("public", "00049", "");
                        else if (1 <= intTmp && intTmp <= 56) str2 = CsConst.mstrINIDefault.IniReadValue("public", "99867", "") + intTmp.ToString();
                        else str2 = CsConst.WholeTextsList[1775].sDisplayName;
                    }
                    else if (str1 == cbPanleControl.Items[10].ToString())
                    {
                        if (intTmp == 255) str2 = CsConst.mstrINIDefault.IniReadValue("public", "00049", "");
                        else if (1 <= intTmp && intTmp <= 32) str2 = CsConst.mstrINIDefault.IniReadValue("public", "99867", "") + intTmp.ToString();
                        else if (intTmp == 101) str2 = CsConst.mstrINIDefault.IniReadValue("public", "99867", "");
                        else if (intTmp == 102) str2 = CsConst.mstrINIDefault.IniReadValue("public", "99868", "");
                        else if (intTmp == 103) str2 = CsConst.mstrINIDefault.IniReadValue("public", "99869", "");
                        else if (intTmp == 104) str2 = CsConst.mstrINIDefault.IniReadValue("public", "99870", "");
                        else str2 = CsConst.WholeTextsList[1775].sDisplayName;
                    }
                    else if (str1 == cbPanleControl.Items[11].ToString())
                    {
                        if (1 <= intTmp && intTmp <= 32) str2 = CsConst.mstrINIDefault.IniReadValue("public", "99867", "") + intTmp.ToString();
                        else str2 = CsConst.WholeTextsList[1775].sDisplayName;
                    }
                    if (str3 == "1") str3 = CsConst.mstrINIDefault.IniReadValue("public", "00042", "");
                    else str3 = CsConst.WholeTextsList[1775].sDisplayName;
                }
                else if (str1 == cbPanleControl.Items[13].ToString())
                {
                    int intTmp = Convert.ToInt32(str2);
                    if (intTmp <= 4) str2 = CsConst.mstrINIDefault.IniReadValue("public", "0005" + intTmp.ToString(), "");
                    str3 = "N/A";
                }
                else if (str1 == cbPanleControl.Items[14].ToString())
                {
                    int intTmp = Convert.ToInt32(str2);
                    if (intTmp <= 3) str2 = CsConst.mstrINIDefault.IniReadValue("public", "0006" + intTmp.ToString(), "");
                    str3 = "N/A";
                }
                else if (str1 == cbPanleControl.Items[15].ToString() || str1 == cbPanleControl.Items[16].ToString() ||
                    str1 == cbPanleControl.Items[23].ToString() || str1 == cbPanleControl.Items[24].ToString())
                {
                    str3 = "N/A";
                }
                else if (str1 == cbPanleControl.Items[17].ToString() || str1 == cbPanleControl.Items[18].ToString() ||
                         str1 == cbPanleControl.Items[19].ToString() || str1 == cbPanleControl.Items[20].ToString())
                {
                    int intTmp = Convert.ToInt32(str2);
                    if (0 <= intTmp && intTmp <= 30) str2 = str2 + "C";
                    else if (32 <= intTmp && intTmp <= 86) str2 = str2 + "F";
                    str3 = "N/A";
                }
                else if (str1 == cbPanleControl.Items[22].ToString())
                {
                    int intTmp = Convert.ToInt32(str2);
                    if (intTmp <= 5) str2 = CsConst.mstrINIDefault.IniReadValue("public", "0007" + (intTmp - 1).ToString(), "");
                    if (str3 == "255") str3 = CsConst.mstrINIDefault.IniReadValue("public", "00049", "");
                    else str3 = str3 + "(" + CsConst.WholeTextsList[934].sDisplayName + ")";
                }
                #endregion
            }
            else if (ID == 102)//消防模块
            {
                #region
                str1 = str1 + "(" + CsConst.WholeTextsList[2510].sDisplayName + ")";
                int intTmp = Convert.ToInt32(str2);
                if (1 <= intTmp && intTmp <= 10) str2 = CsConst.mstrINIDefault.IniReadValue("public", "0008" + (intTmp - 1).ToString(), "");
                str3 = "N/A";
                #endregion
            }
            else if (ID == 103)//音乐控制
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

        private void cbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                setAllControlVisible(false);
                dgvTarget.Rows.Clear();
                int intFrm = 0;
                int intTo = 0;
                if (cbType.SelectedIndex == 0)
                {
                    intFrm = 0;
                    intTo = 0;
                }
                else if (1 <= cbType.SelectedIndex && cbType.SelectedIndex <= 8)
                {
                    if (oPanel != null)
                    {
                        if (oPanel.bytTempType == 0)
                        {
                            intFrm = 1 + ((cbType.SelectedIndex - 1) * 55);
                            intTo = 31 + ((cbType.SelectedIndex - 1) * 55);
                        }
                        else if (oPanel.bytTempType == 1)
                        {
                            intFrm = 1 + ((cbType.SelectedIndex - 1) * 55);
                            intTo = 55 + ((cbType.SelectedIndex - 1) * 55);
                        }
                    }
                    else if (oEnviroPanel != null)
                    {
                        if (oEnviroPanel.TemperatureType == 0)
                        {
                            intFrm = 1 + ((cbType.SelectedIndex - 1) * 55);
                            intTo = 31 + ((cbType.SelectedIndex - 1) * 55);
                        }
                        else if (oEnviroPanel.TemperatureType == 1)
                        {
                            intFrm = 1 + ((cbType.SelectedIndex - 1) * 55);
                            intTo = 55 + ((cbType.SelectedIndex - 1) * 55);
                        }
                    }
                }
                else if (9 <= cbType.SelectedIndex && cbType.SelectedIndex <= 12)
                {
                    intFrm = 432 + cbType.SelectedIndex;
                    intTo = 432 + cbType.SelectedIndex;
                }
                else if (13 <= cbType.SelectedIndex && cbType.SelectedIndex <= 25)
                {
                    if (oPanel != null)
                    {
                        if (oPanel.bytTempType == 0)
                        {
                            intFrm = 445 + ((cbType.SelectedIndex - 13) * 55);
                            intTo = 475 + ((cbType.SelectedIndex - 13) * 55);
                        }
                        else if (oPanel.bytTempType == 1)
                        {
                            intFrm = 445 + ((cbType.SelectedIndex - 13) * 55);
                            intTo = 499 + ((cbType.SelectedIndex - 13) * 55);
                        }
                    }
                    else if (oEnviroPanel != null)
                    {
                        if (oEnviroPanel.TemperatureType == 0)
                        {
                            intFrm = 445 + ((cbType.SelectedIndex - 13) * 55);
                            intTo = 475 + ((cbType.SelectedIndex - 13) * 55);
                        }
                        else if (oEnviroPanel.TemperatureType == 1)
                        {
                            intFrm = 445 + ((cbType.SelectedIndex - 13) * 55);
                            intTo = 499 + ((cbType.SelectedIndex - 13) * 55);
                        }
                    }
                }
                else if (26 <= cbType.SelectedIndex && cbType.SelectedIndex <= 29)
                {
                    intFrm = 1134 + cbType.SelectedIndex;
                    intTo = 1134 + cbType.SelectedIndex;
                }
                else if (30 <= cbType.SelectedIndex && cbType.SelectedIndex <= 34)
                {
                    if (oPanel != null)
                    {
                        if (oPanel.bytTempType == 0)
                        {
                            intFrm = 1164 + ((cbType.SelectedIndex - 30) * 55);
                            intTo = 1194 + ((cbType.SelectedIndex - 30) * 55);
                        }
                        else if (oPanel.bytTempType == 1)
                        {
                            intFrm = 1164 + ((cbType.SelectedIndex - 30) * 55);
                            intTo = 1218 + ((cbType.SelectedIndex - 30) * 55);
                        }
                    }
                    else if (oEnviroPanel != null)
                    {
                        if (oEnviroPanel.TemperatureType == 0)
                        {
                            intFrm = 1164 + ((cbType.SelectedIndex - 30) * 55);
                            intTo = 1194 + ((cbType.SelectedIndex - 30) * 55);
                        }
                        else if (oEnviroPanel.TemperatureType == 1)
                        {
                            intFrm = 1164 + ((cbType.SelectedIndex - 30) * 55);
                            intTo = 1218 + ((cbType.SelectedIndex - 30) * 55);
                        }
                    }
                }
                else
                {
                    intFrm = 1404 + cbType.SelectedIndex;
                    intTo = 1404 + cbType.SelectedIndex;
                }
                int intTmp = 0;
                string strTempType = "C";
                if (oPanel != null)
                {
                    if (oPanel.bytTempType == 0)
                    {
                        intTmp = 0;
                        strTempType = "C";
                    }
                    else if (oPanel.bytTempType == 1)
                    {
                        intTmp = 32;
                        strTempType = "F";
                    }
                }
                else if (oEnviroPanel != null)
                {
                    if (oEnviroPanel.TemperatureType == 0)
                    {
                        intTmp = 0;
                        strTempType = "C";
                    }
                    else if (oEnviroPanel.TemperatureType == 1)
                    {
                        intTmp = 32;
                        strTempType = "F";
                    }
                }
                for (int i = intFrm; i <= intTo; i++)
                {
                    byte[] arayTmp = new byte[2];
                    if (oPanel != null)
                    {
                        arayTmp = new byte[2];
                        arayTmp[0] = Convert.ToByte(i / 256);
                        arayTmp[1] = Convert.ToByte(i % 256);
                    }
                    else if (oEnviroPanel != null)
                    {
                        arayTmp = new byte[3];
                        arayTmp[0] = Convert.ToByte(i / 256);
                        arayTmp[1] = Convert.ToByte(i % 256);
                        arayTmp[2] = Convert.ToByte(SelACNO - 1);
                    }
                    if (CsConst.mySends.AddBufToSndList(arayTmp, 0xE0F4, SubnetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                    {
                        if (intFrm == intTo)
                        {
                            intTmp = 1;
                            strTempType = "";
                        }
                        string strType = ButtonControlType.ConvertorKeyModeToPublicModeGroup(CsConst.myRevBuf[28]);
                        string strParam1 = "0", strParam2 = "0", strParam3 = "0", strParam4 = "0";
                        strParam1 = CsConst.myRevBuf[31].ToString();
                        strParam2 = CsConst.myRevBuf[32].ToString();
                        strParam3 = CsConst.myRevBuf[33].ToString();
                        strParam4 = CsConst.myRevBuf[34].ToString();
                        SetParams(ref strType, ref strParam1, ref strParam2, ref strParam3, strParam4);
                        object[] obj = new object[] { cbType.Text + "-" + intTmp.ToString() + strTempType,CsConst.myRevBuf[29].ToString(),
                                   CsConst.myRevBuf[30].ToString(),strType, strParam1,strParam2,strParam3,strParam4};
                        dgvTarget.Rows.Add(obj);
                        
                        intTmp = intTmp + 1;
                    }
                    else break;
                }
            }
            catch
            {
            }
            Cursor.Current = Cursors.Default;
        }
    }
}
