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
    public partial class FrmReceiverTargets : Form
    {
        public int DIndex = 0;
        private Sensor_12in1 oSensor = null;
        private MultiSensor oSensor_8in1 = null;
        private int SelectedPage = 0;
        private int SelectedKey = 1;
        private string strName;
        private byte SubnetID = 0;
        private byte DeviceID = 0;
        private int MyintDeviceType = 0;
        private TextBox txtSub = new TextBox();
        private TextBox txtDev = new TextBox();
        private ComboBox cbControlType = new ComboBox();
        private ComboBox cbbox1 = new ComboBox();
        private ComboBox cbbox2 = new ComboBox();
        private ComboBox cbbox3 = new ComboBox();
        private TextBox txtbox1 = new TextBox();
        private TextBox txtbox2 = new TextBox();
        private TextBox txtbox3 = new TextBox();
        private TextBox txtbox4 = new TextBox();
        private TimeText txtSeries = new TimeText(":");
        private ComboBox cbPanleControl = new ComboBox();
        private ComboBox cbAudioControl = new ComboBox();
        private UnivasalControl uc1 = new UnivasalControl("0/0");
        private UnivasalControl uc2 = new UnivasalControl("0/0");
        private NewIRKeyID ir = new NewIRKeyID("1");
        private byte[] arayMode = new byte[40];
        public FrmReceiverTargets()
        {
            InitializeComponent();
        }

        public FrmReceiverTargets(string strname, object osensor, int selectedpage, int selectedkey, int devicetype)
        {
            InitializeComponent();
            this.oSensor = null;
            this.oSensor_8in1 = null;
            if (osensor is Sensor_12in1)
            {
                if (CsConst.mysensor_12in1 != null)
                {
                    foreach (Sensor_12in1 oTmp in CsConst.mysensor_12in1)
                    {
                        if (oTmp.DIndex == (osensor as Sensor_12in1).DIndex)
                        {
                            oSensor = oTmp;
                            DIndex = oSensor.DIndex;
                            break;
                        }
                    }
                }
            }
            else if (osensor is MultiSensor)
            {
                if (CsConst.mysensor_8in1 != null)
                {
                    foreach (MultiSensor oTmp in CsConst.mysensor_8in1)
                    {
                        if (oTmp.DIndex == (osensor as MultiSensor).DIndex)
                        {
                            oSensor_8in1 = oTmp;
                            DIndex = oSensor.DIndex;
                            break;
                        }
                    }
                }
            }
            this.SelectedKey = selectedkey;
            this.SelectedPage = selectedpage;
            this.MyintDeviceType = devicetype;
            this.strName = strname;
            string strDevName = strname.Split('\\')[0].ToString();
            SubnetID = Convert.ToByte(strDevName.Split('-')[0]);
            DeviceID = Convert.ToByte(strDevName.Split('-')[1]);
            int MaxValue = int.Parse(CsConst.mstrINIDefault.IniReadValue("DeviceType" + MyintDeviceType.ToString(), "MaxValue", "8"));
            arayMode = new byte[40];

            #region
            cbControlType.Items.Clear();
            HDLSysPF.AddControlTypeToControl(cbControlType, MyintDeviceType);
            cbPanleControl.Items.Clear();
            HDLSysPF.getPanlControlType(cbPanleControl, MyintDeviceType);
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
            txtbox4.KeyPress += txtFrm_KeyPress;

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
            dgvTarget.Controls.Add(txtbox4);
            dgvTarget.Controls.Add(uc1);
            dgvTarget.Controls.Add(uc2);
            dgvTarget.Controls.Add(ir);

            ir.UserControlValueChanged += ir_UserControlValueChanged;
            uc1.UserControlValueChanged += uc1_UserControlValueChanged;
            uc2.UserControlValueChanged += uc2_UserControlValueChanged;
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
            txtbox4.TextChanged += txtbox4_TextChanged;
            txtSeries.TextChanged += txtSeries_TextChanged;
            #endregion
        }

        void txtbox4_TextChanged(object sender, EventArgs e)
        {
            if (dgvTarget.CurrentRow.Index < 0) return;
            if (dgvTarget.RowCount <= 0) return;
            int index = dgvTarget.CurrentRow.Index;
            string str = txtbox4.Text;
            if (str.Contains("(")) str = str.Split('(')[0].ToString();
            if (txtbox4.TextLength > 0)
            {
                if (cbControlType.Text == CsConst.myPublicControlType[19].ControlTypeName)//逻辑灯调节
                {
                    txtbox4.Text = HDLPF.IsNumStringMode(str, 0, 255);
                    dgvTarget[7, index].Value = txtbox4.Text + "(" + CsConst.WholeTextsList[2438].sDisplayName + ")";
                }
                if (cbControlType.Text == CsConst.myPublicControlType[21].ControlTypeName)//休眠模式
                {
                    txtbox4.Text = HDLPF.IsNumStringMode(str, 0, 100);
                    dgvTarget[7, index].Value = txtbox4.Text + "(" + CsConst.WholeTextsList[2436].sDisplayName + ")";
                }
                txtbox4.SelectionStart = txtbox4.Text.Length;
            }
            if (dgvTarget.SelectedRows == null || dgvTarget.SelectedRows.Count == 0) return;
            string strTmp = dgvTarget[7, index].Value.ToString();
            if (strTmp == null) strTmp = "N/A";
            if (txtbox4.Focused)
                ModifyMultilinesIfNeeds(strTmp, 7);
        }

        void ir_UserControlValueChanged(object sender, NewIRKeyID.TextChangeEventArgs e)
        {
            if (dgvTarget.CurrentRow.Index < 0) return;
            if (dgvTarget.RowCount <= 0) return;
            int index = dgvTarget.CurrentRow.Index;
            string str = ir.Text;
            if (ir.Visible)
            {
                dgvTarget[6, index].Value = str + "(" + CsConst.WholeTextsList[2505].sDisplayName + ")";
                if (dgvTarget.SelectedRows == null || dgvTarget.SelectedRows.Count == 0) return;
                string strTmp = dgvTarget[6, index].Value.ToString();
                if (strTmp == null) strTmp = "N/A";
                if (ir.Focused)
                    ModifyMultilinesIfNeeds(strTmp, 6);
            }
        }

        void uc2_UserControlValueChanged(object sender, UnivasalControl.TextChangeEventArgs e)
        {
            if (dgvTarget.CurrentRow.Index < 0) return;
            if (dgvTarget.RowCount <= 0) return;
            int index = dgvTarget.CurrentRow.Index;
            string str = uc2.Text;
            if (uc2.Visible)
            {
                dgvTarget[5, index].Value = str;
                if (dgvTarget.SelectedRows == null || dgvTarget.SelectedRows.Count == 0) return;
                string strTmp = dgvTarget[5, index].Value.ToString() + "(" + CsConst.WholeTextsList[2496].sDisplayName + ")";
                if (strTmp == null) strTmp = "N/A";
                if (uc2.Focused)
                    ModifyMultilinesIfNeeds(strTmp, 5);
            }
        }

        void uc1_UserControlValueChanged(object sender, UnivasalControl.TextChangeEventArgs e)
        {
            if (dgvTarget.CurrentRow.Index < 0) return;
            if (dgvTarget.RowCount <= 0) return;
            int index = dgvTarget.CurrentRow.Index;
            string str = uc1.Text;
            if (uc1.Visible)
            {
                dgvTarget[4, index].Value = str;
                if (dgvTarget.SelectedRows == null || dgvTarget.SelectedRows.Count == 0) return;
                string strTmp = dgvTarget[4, index].Value.ToString();
                if (strTmp == null) strTmp = "N/A";
                if (uc1.Focused)
                    ModifyMultilinesIfNeeds(strTmp, 4);
            }
        }


        void txtbox3_TextChanged(object sender, EventArgs e)
        {
            txtbox3.Visible = true;
            if (dgvTarget.CurrentRow.Index < 0) return;
            if (dgvTarget.RowCount <= 0) return;
            int index = dgvTarget.CurrentRow.Index;
            string str = txtbox3.Text;
            if (str.Contains("(")) str = str.Split('(')[0].ToString();
            #region
            if (cbControlType.Text == CsConst.myPublicControlType[18].ControlTypeName)//红外控制
            {
                if (cbbox1.SelectedIndex == 2)
                {
                    txtbox3.Text = HDLPF.IsNumStringMode(str, 0, 255);
                    dgvTarget[6, index].Value = txtbox3.Text + "(" + CsConst.mstrINIDefault.IniReadValue("public", "99819", "") + ")";
                    txtbox3.SelectionStart = txtbox3.Text.Length;
                }
            }
            else if (cbControlType.Text == CsConst.myPublicControlType[19].ControlTypeName)//逻辑灯调节
            {
                txtbox3.Text = HDLPF.IsNumStringMode(str, 0, 100);
                dgvTarget[6, index].Value = txtbox3.Text + "(" + CsConst.mstrINIDefault.IniReadValue("public", "00011", "") + ")";
                txtbox3.SelectionStart = txtbox3.Text.Length;
            }
            else if (cbControlType.Text == CsConst.myPublicControlType[21].ControlTypeName)//休眠模式
            {
                txtbox3.Text = HDLPF.IsNumStringMode(str, 0, 100);
                dgvTarget[6, index].Value = txtbox3.Text + "(" + CsConst.WholeTextsList[2435].sDisplayName + ")";
                txtbox3.SelectionStart = txtbox3.Text.Length;
            }
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
                ModifyMultilinesIfNeeds(strTmp, 6);
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
                dgvTarget[6, index].Value = str + "(" + CsConst.WholeTextsList[2525].sDisplayName + ")";
                if (dgvTarget.SelectedRows == null || dgvTarget.SelectedRows.Count == 0) return;
                string strTmp = dgvTarget[6, index].Value.ToString();
                if (strTmp == null) strTmp = "N/A";
                if (txtSeries.Focused)
                    ModifyMultilinesIfNeeds(strTmp, 6);
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
                    cbControlType.Text == CsConst.myPublicControlType[10].ControlTypeName ||  //广播场景
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
                ModifyMultilinesIfNeeds(strTmp, 5);
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
                    cbControlType.Text == CsConst.myPublicControlType[2].ControlTypeName||//序列
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
                ModifyMultilinesIfNeeds(strTmp, 4);
        }

        void txtDev_TextChanged(object sender, EventArgs e)
        {
            if (dgvTarget.CurrentRow.Index < 0) return;
            if (dgvTarget.RowCount <= 0) return;
            int index = dgvTarget.CurrentRow.Index;
            if (txtDev.Text.Length > 0)
            {
                txtDev.Text = HDLPF.IsNumStringMode(txtDev.Text, 0, 255);
                txtDev.SelectionStart = txtDev.Text.Length;
                dgvTarget[2, index].Value = txtDev.Text;
                if (txtDev.Focused)
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
                txtSub.Text = HDLPF.IsNumStringMode(txtSub.Text, 0, 255);
                txtSub.SelectionStart = txtSub.Text.Length;
                dgvTarget[1, index].Value = txtSub.Text;
                if (txtSub.Focused)
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
                ModifyMultilinesIfNeeds(strTmp, 6);
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
                        addcontrols(4, index, txtbox1);
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
                ModifyMultilinesIfNeeds(strTmp, 5);
            #endregion
        }

        void cbbox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ir.Visible = false;
            txtbox2.Visible = false;
            cbbox2.Visible = false;
            txtbox3.Visible = false;
            txtbox4.Visible = false;
            if (dgvTarget.CurrentRow.Index < 0) return;
            if (dgvTarget.RowCount <= 0) return;
            int index = dgvTarget.CurrentRow.Index;
            string str = dgvTarget[4, index].Value.ToString();
            string str2 = dgvTarget[5, index].Value.ToString();
            string str3 = dgvTarget[6, index].Value.ToString();
            string str4 = dgvTarget[7, index].Value.ToString();
            if (str.Contains("(")) str = str.Split('(')[0].ToString();
            if (str2.Contains("(")) str2 = str2.Split('(')[0].ToString();
            if (str3.Contains("(")) str3 = str3.Split('(')[0].ToString();
            if (str4.Contains("(")) str4 = str4.Split('(')[0].ToString();
            #region
            if (cbControlType.Text == CsConst.myPublicControlType[7].ControlTypeName)//GPRS控制
            {
                dgvTarget[4, index].Value = cbbox1.Text;
                txtbox2.Text = str2;
                addcontrols(5, index, txtbox2);
            }
            else if (cbControlType.Text == CsConst.myPublicControlType[21].ControlTypeName)//休眠模式
            {
                dgvTarget[4, index].Value = cbbox1.Text;
                if (cbbox1.SelectedIndex == 0)
                {
                    cbbox2.Items.Clear();
                    for (int i = 1; i <= 32; i++)
                        cbbox2.Items.Add(i.ToString());
                    cbbox2.Items.Add(CsConst.WholeTextsList[2434].sDisplayName);
                    addcontrols(5, index, cbbox2);
                    txtbox3.Text = str3;
                    addcontrols(6, index, txtbox3);

                    txtbox4.Text = str4;
                    addcontrols(7, index, txtbox4);
                }
                else
                {
                    dgvTarget[5, index].Value = "N/A";
                    dgvTarget[6, index].Value = "N/A";
                    dgvTarget[7, index].Value = "N/A";
                }
            }
            #endregion
            #region
            if (cbbox2.Visible && cbbox2.Items.Count > 0)
            {
                if (!cbbox2.Items.Contains(str2))
                    cbbox2.SelectedIndex = 0;
                else
                    cbbox2.Text = str2;
            }
            #endregion
            #region
            if (cbbox2.Visible) cbbox2_SelectedIndexChanged(null, null);
            if (ir.Visible) ir_UserControlValueChanged(null, null);
            if (txtbox3.Visible) txtbox3_TextChanged(null, null);
            if (txtbox4.Visible) txtbox4_TextChanged(null, null);
            #endregion
            #region
            if (dgvTarget.SelectedRows == null || dgvTarget.SelectedRows.Count == 0) return;
            string strTmp = dgvTarget[4, index].Value.ToString();
            if (strTmp == null) strTmp = "N/A";
            if (cbbox1.Focused)
                ModifyMultilinesIfNeeds(strTmp, 4);
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
            if (cbAudioControl.Focused)
                ModifyMultilinesIfNeeds(dgvTarget[4, index].Value.ToString(), 4);
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
                addcontrols(5, index, cbbox2);
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
                addcontrols(5, index, txtbox2);
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
                addcontrols(5, index, cbbox2);
                addcontrols(6, index, cbbox3);
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
                addcontrols(5, index, cbbox2);
                dgvTarget[6, index].Value = "N/A";
                #endregion
            }
            else if (cbPanleControl.Text == CsConst.PanelControl[23] ||//地热身高温度
                     cbPanleControl.Text == CsConst.PanelControl[24]) //地热降低温度
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
            else if (cbPanleControl.Text == CsConst.myPublicControlType[25].ControlTypeName ||//地热普通温度
                     cbPanleControl.Text == CsConst.myPublicControlType[26].ControlTypeName ||//地热白天温度
                     cbPanleControl.Text == CsConst.myPublicControlType[27].ControlTypeName ||//地热夜晚温度 
                     cbPanleControl.Text == CsConst.myPublicControlType[28].ControlTypeName)  //地热离开温度
            {
                #region
                cbbox2.Items.Clear();
                for (int i = 5; i <= 35; i++)
                    cbbox2.Items.Add(i.ToString() + "C");
                for (int i = 41; i <= 95; i++)
                    cbbox2.Items.Add(i.ToString() + "F");
                addcontrols(5, index, cbbox2);
                cbbox3.Items.Clear();
                for (int i = 1; i <= 8; i++)
                    cbbox3.Items.Add(i.ToString());
                cbbox3.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00049", ""));
                #endregion
            }
            else if (cbPanleControl.Text == CsConst.PanelControl[29])//选择页
            {
                #region
                cbbox2.Items.Clear();
                for (int i = 1; i <= 7; i++)
                    cbbox2.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00048", "") + i.ToString());
                #endregion
            }
            else if (cbPanleControl.Text == CsConst.PanelControl[30])//感应目标键
            {
                #region
                cbbox2.Items.Clear();
                cbbox2.Items.Add(CsConst.WholeTextsList[1775].sDisplayName);
                for (int i = 1; i <= 32; i++)
                    cbbox2.Items.Add(i.ToString());
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
            if(cbPanleControl.Focused)
                ModifyMultilinesIfNeeds(dgvTarget[4, index].Value.ToString(), 4);
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
            txtbox4.Visible = false;
            txtSeries.Visible = false;
            uc1.Visible = false;
            uc2.Visible = false;
            ir.Visible = false;
            int index = dgvTarget.CurrentRow.Index;
            string str1 = dgvTarget[4, index].Value.ToString();
            string str2 = dgvTarget[5, index].Value.ToString();
            string str3 = dgvTarget[6, index].Value.ToString();
            string str4 = dgvTarget[7, index].Value.ToString();
            if (str1.Contains('(')) str1 = str1.Split('(')[0].ToString();
            if (str2.Contains('(')) str2 = str2.Split('(')[0].ToString();
            if (str3.Contains('(')) str3 = str3.Split('(')[0].ToString();
            if (str4.Contains('(')) str4 = str4.Split('(')[0].ToString();
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
                addcontrols(4, index, txtbox1);

                txtbox2.Text = str2;
                addcontrols(5, index, txtbox2);
                dgvTarget[6, index].Value = "N/A";
                #endregion
            }
            else if (cbControlType.Text == CsConst.myPublicControlType[4].ControlTypeName ||//通用开关
                     cbControlType.Text == CsConst.myPublicControlType[6].ControlTypeName || //窗帘开关 
                     cbControlType.Text == CsConst.myPublicControlType[12].ControlTypeName) //消防模块
            {
                #region
                txtbox1.Text = str1;
                addcontrols(4, index, txtbox1);

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
                addcontrols(5, index, cbbox2);
                dgvTarget[6, index].Value = "N/A";
                #endregion
            }
            else if (cbControlType.Text == CsConst.myPublicControlType[5].ControlTypeName)//单路调节
            {
                #region
                txtbox1.Text = str1;
                addcontrols(4, index, txtbox1);

                txtbox2.Text = str2;
                addcontrols(5, index, txtbox2);


                if (!str3.Contains(":"))
                    txtSeries.Text = "0:0";
                else
                {
                    if (HDLPF.IsRightNumStringMode(str3.Split(':')[0].ToString(), 0, 255) &&
                        HDLPF.IsRightNumStringMode(str3.Split(':')[1].ToString(), 0, 255))
                        txtSeries.Text = HDLPF.GetTimeFromString(str3, ':');
                    else
                        txtSeries.Text = "0:0";
                }
                addcontrols(6, index, txtSeries);
                #endregion
            }
            else if (cbControlType.Text == CsConst.myPublicControlType[7].ControlTypeName)//GPRS控制
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
            else if (cbControlType.Text == CsConst.myPublicControlType[8].ControlTypeName)//面板控制
            {
                #region
                addcontrols(4, index, cbPanleControl);
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
                    {
                        if (HDLPF.IsRightNumStringMode(str3.Split(':')[0].ToString(), 0, 255) &&
                        HDLPF.IsRightNumStringMode(str3.Split(':')[1].ToString(), 0, 255))
                            txtSeries.Text = HDLPF.GetTimeFromString(str3, ':');
                        else
                            txtSeries.Text = "0:0";
                    }
                    addcontrols(6, index, txtSeries);
                }
                txtbox2.Text = str2;
                addcontrols(5, index, txtbox2);
                #endregion
            }
            else if (cbControlType.Text == CsConst.myPublicControlType[13].ControlTypeName)//音乐播放
            {
                #region
                addcontrols(4, index, cbAudioControl);
                #endregion
            }
            else if (cbControlType.Text == CsConst.myPublicControlType[3].ControlTypeName)//时间开关
            {
                #region
                txtbox1.Text = str1;
                addcontrols(4, index, txtbox1);

                cbbox2.Items.Clear();
                cbbox2.Items.Add(CsConst.Status[0]);
                cbbox2.Items.Add(CsConst.Status[1]);
                addcontrols(5, index, cbbox2);
                dgvTarget[6, index].Value = "N/A";
                #endregion
            }
            else if (cbControlType.Text == CsConst.myPublicControlType[14].ControlTypeName)//通用控制
            {
                #region
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
                addcontrols(4, index, uc1);

                if (str2.Contains("/"))
                {
                    string strtmp = str2;
                    str2 = HDLPF.IsNumStringMode(strtmp.Split('/')[0].ToString(), 0, 255) + "/" +
                           HDLPF.IsNumStringMode(strtmp.Split('/')[1].ToString(), 0, 255);
                    uc2.Text = str1;
                }
                else
                {
                    uc2.Text = "0/0";
                }
                addcontrols(5, index, uc2);
                dgvTarget[6, index].Value = "N/A";
                #endregion
            }
            else if (cbControlType.Text == CsConst.myPublicControlType[15].ControlTypeName)//连接页
            {
                #region
                txtbox1.Text = str1;
                addcontrols(4, index, txtbox1);

                cbbox2.Items.Clear();
                cbbox2.Items.Add(CsConst.WholeTextsList[1775].sDisplayName);
                cbbox2.Items.Add(CsConst.WholeTextsList[2497].sDisplayName);
                cbbox2.Items.Add(CsConst.WholeTextsList[2498].sDisplayName);
                addcontrols(5, index, cbbox2);
                dgvTarget[6, index].Value = "N/A";
                #endregion
            }
            else if (cbControlType.Text == CsConst.myPublicControlType[18].ControlTypeName)//红外控制
            {
                #region
                txtbox1.Text = str1;
                addcontrols(4, index, txtbox1);
                txtbox2.Text = str2;
                addcontrols(5, index, txtbox2);
                ir.Text =  HDLPF.IsNumStringMode(str3, 0, 255);
                addcontrols(6, index, ir);
                dgvTarget[7, index].Value = "N/A";
                #endregion
            }
            else if (cbControlType.Text == CsConst.myPublicControlType[19].ControlTypeName)//逻辑灯调节
            {
                #region
                txtbox1.Text = str1;
                addcontrols(4, index, txtbox1);

                txtbox2.Text = str2;
                addcontrols(5, index, txtbox2);

                txtbox3.Text = str3;
                addcontrols(6, index, txtbox3);

                txtbox4.Text = str4;
                addcontrols(7, index, txtbox4);
                #endregion
            }
            else if (cbControlType.Text == CsConst.myPublicControlType[20].ControlTypeName)//逻辑场景
            {
                #region
                txtbox1.Text = str1;
                addcontrols(4, index, txtbox1);

                txtbox2.Text = str2;
                addcontrols(5, index, txtbox2);
                dgvTarget[6, index].Value = "N/A";
                dgvTarget[7, index].Value = "N/A";
                #endregion
            }
            else if (cbControlType.Text == CsConst.myPublicControlType[21].ControlTypeName)//休眠模式
            {
                #region
                cbbox1.Items.Clear();
                cbbox1.Items.Add(CsConst.WholeTextsList[757].sDisplayName);
                cbbox1.Items.Add(CsConst.WholeTextsList[1051].sDisplayName);
                addcontrols(4, index, cbbox1);
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
            if (uc1.Visible) uc1_UserControlValueChanged(null, null);
            if (uc2.Visible) uc2_UserControlValueChanged(null, null);
            if (ir.Visible) ir_UserControlValueChanged(null, null);
            #endregion
            if (cbControlType.Focused)
                ModifyMultilinesIfNeeds(dgvTarget[3, index].Value.ToString(), 3);
        }

        private void FrmColorDLPTargets_Load(object sender, EventArgs e)
        {
            cbPage.SelectedIndex = SelectedPage;
            cbKey.SelectedIndex = SelectedKey;
            lbRemarkValue.Text = strName.Split('\\')[1].ToString();
            lbSubValue.Text = SubnetID.ToString();
            lbDevValue.Text = DeviceID.ToString();
            if (CsConst.mySends.AddBufToSndList(null, 0x1672, SubnetID, DeviceID, false, true, true, false) == true)
            {
                for (int intJ = 0; intJ < arayMode.Length; intJ++)
                {
                    arayMode[intJ] = CsConst.myRevBuf[26 + intJ];
                }
                
            }
            cbPage_SelectedIndexChanged(null, null);
        }

        private void txtFrm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar) || e.KeyChar == 8)
                e.Handled = false;
            else
                e.Handled = true;
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
            txtbox4.Visible = TF;
            txtSeries.Visible = TF;
            cbPanleControl.Visible = TF;
            cbAudioControl.Visible = TF;
            uc1.Visible = TF;
            uc2.Visible = TF;
            ir.Visible = TF;
        }

        private void btnSure_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            dgvTarget.Rows.Clear();
            setAllControlVisible(false);
            if (cbPage.SelectedIndex < 0) cbPage.SelectedIndex = 0;
            if (cbKey.SelectedIndex < 0) cbKey.SelectedIndex = 0;
            byte bytFrm = Convert.ToByte(Convert.ToInt32(txtFrm.Text));
            byte bytTo = Convert.ToByte(txtTo.Text);
            for (byte byt = bytFrm; byt <= bytTo; byt++)
            {
                byte[] arayTmp = new byte[2];
                arayTmp[0] = Convert.ToByte(cbPage.SelectedIndex * cbKey.Items.Count + cbKey.SelectedIndex + 1);
                arayTmp[1] = byt;
                if (CsConst.mySends.AddBufToSndList(arayTmp, 0x1676, SubnetID, DeviceID, false, true, true, false) == true)
                {
                    string strType = "";
                   // strType = HDLSysPF.InquireKeyFunTypeStringFromDB(Convert.ToByte(GetIDAccordinglyRightControlType(CsConst.myRevBuf[28])));
                    string strParam1 = "0", strParam2 = "0", strParam3 = "0", strParam4 = "0";
                    strParam1 = CsConst.myRevBuf[31].ToString();
                    strParam2 = CsConst.myRevBuf[32].ToString();
                    strParam3 = CsConst.myRevBuf[33].ToString();
                    strParam4 = CsConst.myRevBuf[34].ToString();
                    SetParams(ref strType, ref strParam1, ref strParam2, ref strParam3,ref strParam4);
                    object[] obj = new object[] { byt.ToString(),CsConst.myRevBuf[29].ToString(),CsConst.myRevBuf[30].ToString(),strType
                                ,strParam1,strParam2,strParam3,strParam4};
                    dgvTarget.Rows.Add(obj);
                    
                    HDLUDP.TimeBetwnNext(1);
                }
                else
                {
                    break;
                }
            }
            Cursor.Current = Cursors.Default;
        }

        private string GetIDAccordinglyRightControlType(byte bytType)
        {
            string bytResult = "0";
            switch (bytType)
            {
                case 0: bytResult = "85"; break;//场景
                case 1: bytResult = "86"; break;//序列
                case 2: bytResult = "88"; break;//通用开关
                case 3: bytResult = "0"; break;//无效
                case 4: bytResult = "89"; break;//单路调节 广播回路
                case 5: bytResult = "100"; break;//广播场景
                case 7: bytResult = "92"; break;//窗帘开关
                case 13: bytResult = "94"; break;//GPRS控制
                case 14: bytResult = "95"; break;//面板控制
                case 17: bytResult = "102"; break;//消防模块
                case 18: bytResult = "103"; break;//音乐播放
            }
            return bytResult;
        }

        private byte GetValueAfterChangeType(byte value)
        {
            byte Value = 0;
            switch (value)
            {
                case 85: Value = 0; break;//场景
                case 86: Value = 1; break;//序列
                case 88: Value = 2; break;//通用开关
                case 0: Value = 3; break;//无效
                case 89: Value = 4; break;//单路调节 广播回路
                case 100: Value = 5; break;//广播场景
                case 92: Value = 7; break;//窗帘开关
                case 94: Value = 13; break;//GPRS控制
                case 95: Value = 14; break;//面板控制
                case 102: Value = 17; break;//消防模块
                case 103: Value = 18; break;//音乐播放
            }
            return Value;
        }

        private void SetParams(ref string strType, ref string str1, ref string str2, ref string str3, ref string str4)
        {
            if (strType == CsConst.mstrINIDefault.IniReadValue("TargetType", "00000", ""))//无效
            {
                #region
                str1 = "N/A";
                str2 = "N/A";
                str3 = "N/A";
                str4 = "N/A";
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
                str4 = "N/A";
                #endregion
            }
            else if (strType == CsConst.myPublicControlType[2].ControlTypeName)//序列
            {
                #region
                str1 = str1 + "(" + CsConst.WholeTextsList[2510].sDisplayName + ")";
                str2 = str2 + "(" + CsConst.WholeTextsList[2512].sDisplayName+ ")";
                str3 = "N/A";
                str4 = "N/A";
                #endregion
            }
            else if (strType == CsConst.myPublicControlType[4].ControlTypeName)//通用开关
            {
                #region
                str1 = str1 + "(" + CsConst.WholeTextsList[2513].sDisplayName + ")";
                if (str2 == "0") str2 = CsConst.Status[0] + "(" + CsConst.WholeTextsList[2529].sDisplayName + ")";
                else if (str2 == "255") str2 = CsConst.Status[1] + "(" + CsConst.WholeTextsList[2529].sDisplayName + ")";
                str3 = "N/A";
                str4 = "N/A";
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
                str4 = "N/A";
                #endregion
            }
            else if (strType == CsConst.myPublicControlType[10].ControlTypeName)//广播场景
            {
                #region
                str1 = CsConst.WholeTextsList[2566].sDisplayName;
                str2 = str2 + "(" + CsConst.WholeTextsList[2511].sDisplayName + ")";
                str3 = "N/A";
                str4 = "N/A";
                #endregion
            }
            else if (strType == CsConst.myPublicControlType[11].ControlTypeName)//广播回路
            {
                #region
                str1 = CsConst.WholeTextsList[2567].sDisplayName;
                str2 = str2 + "(" + CsConst.WholeTextsList[2524].sDisplayName + ")";
                int intTmp = Convert.ToInt32(str3) * 256 + Convert.ToInt32(str4);
                str3 = HDLPF.GetStringFromTime(intTmp, ":") + "(" + CsConst.WholeTextsList[2525].sDisplayName + ")";
                str4 = "N/A";
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
                str4 = "N/A";
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
                str4 = "N/A";
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
                str4 = "N/A";
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
                str4 = "N/A";
                #endregion
            }
            else if (strType == CsConst.myPublicControlType[3].ControlTypeName)//时间开关
            {
                #region
                str1 = str1 + "(" + CsConst.WholeTextsList[2513].sDisplayName + ")";
                if (str2 == "0") str2 = CsConst.Status[0] + "(" + CsConst.WholeTextsList[2529].sDisplayName + ")";
                else if (str2 == "255") str2 = CsConst.Status[1] + "(" + CsConst.WholeTextsList[2529].sDisplayName + ")";
                str3 = "N/A";
                str4 = "N/A";
                #endregion
            }
            else if (strType == CsConst.myPublicControlType[14].ControlTypeName)//通用控制
            {
                #region
                str1 = str1 + "/" + str2;
                str2 = str3 + "/" + str4;
                str3 = "N/A";
                str4 = "N/A";
                #endregion
            }
            else if (strType == CsConst.myPublicControlType[15].ControlTypeName)//连接页
            {
                #region
                str1 = str1 + "(" + CsConst.WholeTextsList[2499].sDisplayName + ")";
                if (str2 == "0")
                    str2 = CsConst.WholeTextsList[2497].sDisplayName;
                else if (str2 == "1")
                    str2 = CsConst.WholeTextsList[2498].sDisplayName;
                else
                    str2 = CsConst.WholeTextsList[1775].sDisplayName;
                str3 = "N/A";
                str4 = "N/A";
                #endregion
            }
            else if (strType == CsConst.myPublicControlType[18].ControlTypeName)//红外控制
            {
                #region
                str1 = str1 + "(" + CsConst.WholeTextsList[2506].sDisplayName + ")";
                str2 = str2 + "(" + CsConst.WholeTextsList[216].sDisplayName + ")";
                str3 = str3 + "(" + CsConst.WholeTextsList[2505].sDisplayName + ")";
                str4 = "N/A";
                #endregion
            }
            else if (strType == CsConst.myPublicControlType[19].ControlTypeName)//逻辑灯调节
            {
                #region
                string strTmp = str2;
                str1 = str1 + "(" + CsConst.WholeTextsList[2440].sDisplayName + ")";
                str2 = str3 + "(" + CsConst.WholeTextsList[2439].sDisplayName + ")";
                str3 = strTmp + "(" + CsConst.WholeTextsList[2524].sDisplayName + ")";
                str4 = str4 + "(" + CsConst.WholeTextsList[2438].sDisplayName + ")";
                #endregion
            }
            else if (strType == CsConst.myPublicControlType[20].ControlTypeName)//逻辑场景
            {
                #region
                str1 = str1 + "(" + CsConst.WholeTextsList[2510].sDisplayName + ")";
                str2 = str2 + "(" + CsConst.WholeTextsList[2511].sDisplayName + ")";
                str3 = "N/A";
                str4 = "N/A";
                #endregion
            }
            else if (strType == CsConst.myPublicControlType[21].ControlTypeName)//休眠模式
            {
                #region
                if (str1 == "1") str1 = CsConst.WholeTextsList[757].sDisplayName;
                else str1 = CsConst.WholeTextsList[1051].sDisplayName;
                if (str1 == CsConst.WholeTextsList[757].sDisplayName)
                {
                    if (str2 == "255")
                        str2 = CsConst.WholeTextsList[2434].sDisplayName;
                    else
                        str2 = str2 + "(" + CsConst.WholeTextsList[2437].sDisplayName + ")";
                    str3 = str3 + "(" + CsConst.WholeTextsList[2435].sDisplayName + ")";
                    str4 = str4 + "(" + CsConst.WholeTextsList[2436].sDisplayName + ")";
                }
                else
                {
                    str2 = "N/A";
                    str3 = "N/A";
                    str4 = "N/A";
                }
                #endregion
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (dgvTarget.RowCount == 0) return;
            Cursor.Current = Cursors.WaitCursor;
            btnSave.Enabled = false;
            setAllControlVisible(false);
            for (int i = 0; i < dgvTarget.RowCount; i++)
            {
                byte[] arayTmp = new byte[9];
                arayTmp[0] = Convert.ToByte(cbPage.SelectedIndex * cbKey.Items.Count + cbKey.SelectedIndex + 1);
                arayTmp[1] = Convert.ToByte(dgvTarget[0, i].Value.ToString());
                arayTmp[3] = Convert.ToByte(dgvTarget[1, i].Value.ToString());
                arayTmp[4] = Convert.ToByte(dgvTarget[2, i].Value.ToString());
               // arayTmp[2] = Convert.ToByte(HDLSysPF.getIDFromControlTypeString(dgvTarget[3, i].Value.ToString()));

                string str1 = dgvTarget[4, i].Value.ToString();
                string str2 = dgvTarget[5, i].Value.ToString();
                string str3 = dgvTarget[6, i].Value.ToString();
                string str4 = dgvTarget[7, i].Value.ToString();
                if (str1.Contains("(")) str1 = str1.Split('(')[0].ToString();
                if (str2.Contains("(")) str2 = str2.Split('(')[0].ToString();
                if (str3.Contains("(")) str3 = str3.Split('(')[0].ToString();
                if (str4.Contains("(")) str4 = str4.Split('(')[0].ToString();
                if (arayTmp[2] == 85 || arayTmp[2] == 86)//场景 序列
                {
                    #region
                    if (HDLPF.IsRightNumStringMode(str1, 0, 255))
                        arayTmp[5] = Convert.ToByte(str1);
                    if (HDLPF.IsRightNumStringMode(str2, 0, 255))
                        arayTmp[6] = Convert.ToByte(str2);
                    #endregion
                }
                else if (arayTmp[2] == 88)//通用开关
                {
                    #region
                    if (HDLPF.IsRightNumStringMode(str1, 0, 255))
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
                    if (HDLPF.IsRightNumStringMode(str1, 0, 255))
                        arayTmp[5] = Convert.ToByte(str1);
                    if (HDLPF.IsRightNumStringMode(str2, 0, 255))
                        arayTmp[6] = Convert.ToByte(str2);
                    if (str3.Contains(":"))
                    {
                        int intTmp = Convert.ToInt32(HDLPF.GetTimeFromString(str3, ':'));
                        arayTmp[7] = Convert.ToByte(intTmp / 256);
                        arayTmp[8] = Convert.ToByte(intTmp % 256);
                    }
                    #endregion
                }
                else if (arayTmp[2] == 100)//广播场景
                {
                    #region
                    arayTmp[2] = 85;
                    arayTmp[5] = 255;
                    if (HDLPF.IsRightNumStringMode(str2, 0, 255))
                        arayTmp[6] = Convert.ToByte(str2);
                    #endregion
                }
                else if (arayTmp[2] == 101)//广播回路
                {
                    #region
                    arayTmp[2] = 89;
                    arayTmp[5] = 255;
                    if (HDLPF.IsRightNumStringMode(str2, 0, 255))
                        arayTmp[6] = Convert.ToByte(str2);
                    if (str3.Contains(":"))
                    {
                        int intTmp = Convert.ToInt32(HDLPF.GetTimeFromString(str3, ':'));
                        arayTmp[7] = Convert.ToByte(intTmp / 256);
                        arayTmp[8] = Convert.ToByte(intTmp % 256);
                    }
                    #endregion
                }
                else if (arayTmp[2] == 92)//窗帘开关
                {
                    #region
                    if (HDLPF.IsRightNumStringMode(str1, 0, 255))
                        arayTmp[5] = Convert.ToByte(str1);
                    if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00036", "")) arayTmp[6] = 0;
                    else if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00037", "")) arayTmp[6] = 1;
                    else if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00038", "")) arayTmp[6] = 2;
                    else
                    {
                        str2 = str2.Replace("%", "");
                        if (HDLPF.IsRightNumStringMode(str2, 0, 255))
                            arayTmp[6] = Convert.ToByte(str2);
                        if (HDLPF.IsRightNumStringMode(str1, 0, 255))
                            arayTmp[5] = Convert.ToByte(Convert.ToByte(str1) + 16);
                    }
                    #endregion
                }
                else if (arayTmp[2] == 94)//GPRS
                {
                    #region
                    if (str1 == CsConst.mstrINIDefault.IniReadValue("public", "99862", "")) arayTmp[5] = 1;
                    else if (str1 == CsConst.mstrINIDefault.IniReadValue("public", "99863", "")) arayTmp[5] = 2;
                    else arayTmp[5] = 0;
                    if (HDLPF.IsRightNumStringMode(str2, 0, 255))
                        arayTmp[6] = Convert.ToByte(str2);
                    #endregion
                }
                else if (arayTmp[2] == 95)//面板控制
                {
                    #region
                    arayTmp[5] = Convert.ToByte(HDLSysPF.getIDFromPanelControlTypeString(str1));
                    if (arayTmp[5] == 1 || arayTmp[5] == 11 ||
                        arayTmp[5] == 2 || arayTmp[5] == 12 ||
                        arayTmp[5] == 24 || arayTmp[5] == 3 ||
                        arayTmp[5] == 20)
                    {
                        if (str2 == CsConst.Status[0])
                            arayTmp[6] = 0;
                        else if (str2 == CsConst.Status[1])
                            arayTmp[6] = 1;
                    }
                    else if (arayTmp[5] == 13 || arayTmp[5] == 14)
                    {
                        if (HDLPF.IsRightNumStringMode(str2, 0, 255))
                            arayTmp[6] = Convert.ToByte(str2);
                    }
                    else if (arayTmp[5] == 16 || arayTmp[5] == 15 ||
                             arayTmp[5] == 17 || arayTmp[5] == 18)
                    {
                        if (arayTmp[5] == 16)
                        {
                            if (str2.Contains(CsConst.mstrINIDefault.IniReadValue("public", "00048", "")))
                            {
                                str2 = str2.Replace(CsConst.mstrINIDefault.IniReadValue("public", "00048", ""), "");
                                if (HDLPF.IsRightNumStringMode(str2, 0, 255))
                                    arayTmp[6] = Convert.ToByte(str2);
                            }
                            else if (str2 == CsConst.WholeTextsList[1775].sDisplayName)
                                arayTmp[6] = 0;
                            else if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00049", ""))
                                arayTmp[6] = 255;
                        }
                        else if (arayTmp[5] == 15)
                        {
                            if (str2.Contains(CsConst.mstrINIDefault.IniReadValue("public", "99867", "")))
                            {
                                str2 = str2.Replace(CsConst.mstrINIDefault.IniReadValue("public", "99867", ""), "");
                                if (HDLPF.IsRightNumStringMode(str2, 0, 255))
                                    arayTmp[6] = Convert.ToByte(str2);
                            }
                            else if (str2 == CsConst.WholeTextsList[1775].sDisplayName)
                                arayTmp[6] = 0;
                            else if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00049", ""))
                                arayTmp[6] = 255;
                        }
                        else if (arayTmp[5] == 17)
                        {
                            if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00049", "")) arayTmp[6] = 255;
                            else if (str2.Contains(CsConst.mstrINIDefault.IniReadValue("public", "99867", ""))
                                && !str2.Contains("["))
                            {
                                str2 = str2.Replace(CsConst.mstrINIDefault.IniReadValue("public", "99867", ""), "");
                                if (HDLPF.IsRightNumStringMode(str2, 0, 255))
                                    arayTmp[6] = Convert.ToByte(str2);
                            }
                            else if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "99868", "")) arayTmp[6] = 101;
                            else if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "99869", "")) arayTmp[6] = 102;
                            else if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "99870", "")) arayTmp[6] = 103;
                            else if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "99871", "")) arayTmp[6] = 104;
                            else if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00049", "")) arayTmp[6] = 255;
                        }
                        else if (arayTmp[5] == 18)
                        {
                            if (str2.Contains(CsConst.mstrINIDefault.IniReadValue("public", "99867", "")))
                            {
                                str2 = str2.Replace(CsConst.mstrINIDefault.IniReadValue("public", "99867", ""), "");
                                if (HDLPF.IsRightNumStringMode(str2, 0, 255))
                                    arayTmp[6] = Convert.ToByte(str2);
                            }
                            else if (str2 == CsConst.WholeTextsList[1775].sDisplayName) arayTmp[6] = 0;
                        }
                        if (str3 == CsConst.mstrINIDefault.IniReadValue("public", "00042", "")) arayTmp[7] = 1;
                        else if (str3 == CsConst.WholeTextsList[1775].sDisplayName) arayTmp[7] = 0;
                    }
                    else if (arayTmp[5] == 6)
                    {
                        if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00050", "")) arayTmp[6] = 0;
                        else if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00051", "")) arayTmp[6] = 1;
                        else if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00052", "")) arayTmp[6] = 2;
                        else if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00053", "")) arayTmp[6] = 3;
                        else if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00054", "")) arayTmp[6] = 4;
                    }
                    else if (arayTmp[5] == 5)
                    {
                        if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00060", "")) arayTmp[6] = 0;
                        else if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00061", "")) arayTmp[6] = 1;
                        else if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00062", "")) arayTmp[6] = 2;
                        else if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00063", "")) arayTmp[6] = 3;
                    }
                    else if (arayTmp[5] == 9 || arayTmp[5] == 10 ||
                             arayTmp[5] == 22 || arayTmp[5] == 23)
                    {
                        if (HDLPF.IsRightNumStringMode(str2, 0, 255))
                            arayTmp[6] = Convert.ToByte(str2);
                        if (arayTmp[5] == 22 || arayTmp[5] == 23)
                        {
                            if (str3 == CsConst.mstrINIDefault.IniReadValue("public", "00049", ""))
                            {
                                arayTmp[7] = 255;
                            }
                            else
                            {
                                if (HDLPF.IsRightNumStringMode(str3, 0, 255))
                                    arayTmp[7] = Convert.ToByte(str3);
                            }
                        }
                    }
                    else if (arayTmp[5] == 4 || arayTmp[5] == 7 ||
                             arayTmp[5] == 8 || arayTmp[5] == 19)
                    {
                        if (str2.Contains("C")) str2 = str2.Replace("C", "");
                        if (str2.Contains("F")) str2 = str2.Replace("F", "");
                        if (HDLPF.IsRightNumStringMode(str2, 0, 255))
                            arayTmp[6] = Convert.ToByte(str2);
                    }
                    else if (arayTmp[5] == 21)
                    {
                        if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00070", "")) arayTmp[6] = 1;
                        else if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00071", "")) arayTmp[6] = 2;
                        else if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00072", "")) arayTmp[6] = 3;
                        else if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00073", "")) arayTmp[6] = 4;
                        else if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00074", "")) arayTmp[6] = 5;

                        if (str3 == CsConst.mstrINIDefault.IniReadValue("public", "00049", ""))
                        {
                            arayTmp[7] = 255;
                        }
                        else
                        {
                            if (HDLPF.IsRightNumStringMode(str3, 0, 255))
                                arayTmp[7] = Convert.ToByte(str3);
                        }
                    }
                    else if (arayTmp[5] == 25 || arayTmp[5] == 26 ||
                             arayTmp[5] == 27 || arayTmp[5] == 28)
                    {
                        str2 = str2.Replace("C", "");
                        str2 = str2.Replace("F", "");
                        if (HDLPF.IsRightNumStringMode(str2, 0, 255))
                            arayTmp[6] = Convert.ToByte(str2);
                        if (str3 == CsConst.mstrINIDefault.IniReadValue("public", "00049", ""))
                        {
                            arayTmp[7] = 255;
                        }
                        else
                        {
                            if (HDLPF.IsRightNumStringMode(str3, 0, 255))
                                arayTmp[7] = Convert.ToByte(str3);
                        }
                    }
                    else if (arayTmp[5] == 29)
                    {
                        str2 = str2.Replace(CsConst.mstrINIDefault.IniReadValue("public", "00048", ""), "");
                        if (HDLPF.IsRightNumStringMode(str2, 0, 255))
                            arayTmp[6] = Convert.ToByte(str2);
                    }
                    else if (arayTmp[5] == 30)
                    {
                        if (HDLPF.IsRightNumStringMode(str2, 0, 255))
                            arayTmp[6] = Convert.ToByte(str2);
                    }
                    #endregion
                }
                else if (arayTmp[2] == 102)//消防模块
                {
                    #region
                    if (HDLPF.IsRightNumStringMode(str1, 0, 255))
                        arayTmp[5] = Convert.ToByte(str1);
                    if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00080", "")) arayTmp[6] = 1;
                    else if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00081", "")) arayTmp[6] = 2;
                    else if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00082", "")) arayTmp[6] = 3;
                    else if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00083", "")) arayTmp[6] = 4;
                    else if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00084", "")) arayTmp[6] = 5;
                    else if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00085", "")) arayTmp[6] = 6;
                    else if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00086", "")) arayTmp[6] = 7;
                    else if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00087", "")) arayTmp[6] = 8;
                    else if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00088", "")) arayTmp[6] = 9;
                    else if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00089", "")) arayTmp[6] = 10;
                    #endregion
                }
                else if (arayTmp[2] == 103)//音乐控制
                {
                    #region
                    if (str1 == cbAudioControl.Items[0].ToString())
                    {
                        arayTmp[5] = 1;
                        if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00101", "")) arayTmp[6] = 1;
                        else if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00102", "")) arayTmp[6] = 2;
                        else if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00103", "")) arayTmp[6] = 3;
                        else if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00104", "")) arayTmp[6] = 4;
                    }
                    else if (str1 == cbAudioControl.Items[1].ToString())
                    {
                        arayTmp[5] = 2;
                        if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00111", "")) arayTmp[6] = 1;
                        else if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00112", "")) arayTmp[6] = 2;
                        else if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00113", "")) arayTmp[6] = 3;
                        else if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00114", "")) arayTmp[6] = 4;
                    }
                    else if (str1 == cbAudioControl.Items[2].ToString())
                    {
                        arayTmp[5] = 3;
                        if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00121", "")) arayTmp[6] = 1;
                        else if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00122", "")) arayTmp[6] = 2;
                        else if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00123", "")) arayTmp[6] = 3;
                        else if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00124", "")) arayTmp[6] = 4;
                        else if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00125", "")) arayTmp[6] = 5;
                        else if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00126", "")) arayTmp[6] = 6;
                        if (arayTmp[6] == 3 || arayTmp[6] == 6)
                        {
                            if (HDLPF.IsRightNumStringMode(str3, 0, 255))
                                arayTmp[7] = Convert.ToByte(str3);
                        }
                    }
                    else if (str1 == cbAudioControl.Items[3].ToString())
                    {
                        arayTmp[5] = 4;
                        if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00131", "")) arayTmp[6] = 1;
                        else if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00132", "")) arayTmp[6] = 2;
                        else if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00133", "")) arayTmp[6] = 3;
                        else if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00134", "")) arayTmp[6] = 4;
                    }
                    else if (str1 == cbAudioControl.Items[4].ToString())
                    {
                        arayTmp[5] = 5;
                        if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00141", "")) arayTmp[6] = 1;
                        else if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00142", "")) arayTmp[6] = 2;
                        else if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00143", "")) arayTmp[6] = 3;
                        if (arayTmp[6] == 1)
                        {
                            if (str3 == CsConst.mstrINIDefault.IniReadValue("public", "00044", ""))
                                arayTmp[7] = 1;
                            else if (str3 == CsConst.mstrINIDefault.IniReadValue("public", "00045", ""))
                                arayTmp[7] = 2;
                            else if (str3.Contains(CsConst.mstrINIDefault.IniReadValue("public", "99872", "")))
                            {
                                arayTmp[7] = 3;
                                if (str3.Contains(":"))
                                {
                                    str3 = str3.Split(':')[1].ToString();
                                    if (HDLPF.IsRightNumStringMode(str3, 0, 255))
                                        arayTmp[8] = Convert.ToByte(79 - Convert.ToInt32(str3));
                                }
                            }
                        }
                        else if (arayTmp[6] == 2 || arayTmp[6] == 3)
                        {
                            if (str3 == CsConst.mstrINIDefault.IniReadValue("public", "00044", ""))
                                arayTmp[7] = 1;
                            else if (str3 == CsConst.mstrINIDefault.IniReadValue("public", "00045", ""))
                                arayTmp[7] = 2;
                        }
                    }
                    else if (str1 == cbAudioControl.Items[5].ToString())
                    {
                        arayTmp[5] = 6;
                        if (HDLPF.IsRightNumStringMode(str2, 0, 255))
                            arayTmp[6] = Convert.ToByte(str2);
                        if (HDLPF.IsRightNumStringMode(str3, 0, 65535))
                        {
                            int intTmp = Convert.ToInt32(str3);
                            arayTmp[7] = Convert.ToByte(intTmp / 256);
                            arayTmp[8] = Convert.ToByte(intTmp % 256);
                        }
                    }
                    else if (str1 == cbAudioControl.Items[6].ToString() || str1 == cbAudioControl.Items[7].ToString())
                    {
                        if (str1 == cbAudioControl.Items[6].ToString())
                            arayTmp[5] = 7;
                        if (str1 == cbAudioControl.Items[7].ToString())
                            arayTmp[5] = 8;

                        if (str2 == CsConst.mstrINIDefault.IniReadValue("public", "00047", ""))
                            arayTmp[6] = 64;
                        else if (str2.Contains("SD"))
                        {
                            if (str2.Contains(":"))
                            {
                                if (HDLPF.IsRightNumStringMode(str2.Split(':')[1].ToString(), 0, 65535))
                                    arayTmp[6] = Convert.ToByte(Convert.ToInt32(str2.Split(':')[1].ToString()) + 64);
                            }
                        }
                        else if (str2.Contains("FTP"))
                        {
                            if (str2.Contains(":"))
                            {
                                if (HDLPF.IsRightNumStringMode(str2.Split(':')[1].ToString(), 0, 65535))
                                    arayTmp[6] = Convert.ToByte(Convert.ToInt32(str2.Split(':')[1].ToString()) + 128);
                            }
                        }
                        if (HDLPF.IsRightNumStringMode(str3, 0, 65535))
                        {
                            int intTmp = Convert.ToInt32(str3);
                            arayTmp[7] = Convert.ToByte(intTmp / 256);
                            arayTmp[8] = Convert.ToByte(intTmp % 256);
                        }
                    }
                    #endregion
                }
                else if (arayTmp[2] == 87)//时间开关
                {
                    #region
                    if (HDLPF.IsRightNumStringMode(str1, 0, 255))
                        arayTmp[5] = Convert.ToByte(str1);
                    if (str2 == CsConst.Status[0])
                        arayTmp[6] = 0;
                    else if (str2 == CsConst.Status[1])
                        arayTmp[6] = 255;
                    #endregion
                }
                else if (arayTmp[2] == 104)//通用控制
                {
                    #region
                    arayTmp[5] = Convert.ToByte(str1.Split('/')[0].ToString());
                    arayTmp[6] = Convert.ToByte(str1.Split('/')[1].ToString());
                    arayTmp[7] = Convert.ToByte(str2.Split('/')[0].ToString());
                    arayTmp[8] = Convert.ToByte(str2.Split('/')[1].ToString());
                    #endregion
                }
                else if (arayTmp[2] == 105)//连接页
                {
                    #region
                    if (HDLPF.IsRightNumStringMode(str1, 0, 255))
                        arayTmp[5] = Convert.ToByte(str1);
                    if (str2 == CsConst.WholeTextsList[2497].sDisplayName)
                        arayTmp[6] = 0;
                    else if (str2 == CsConst.WholeTextsList[2498].sDisplayName)
                        arayTmp[6] = 1;
                    else if (str2 == CsConst.WholeTextsList[1775].sDisplayName)
                        arayTmp[6] = 255;
                    #endregion
                }
                else if (arayTmp[2] == 108)//红外控制
                {
                    #region
                    if (str1 == CsConst.mstrINIDefault.IniReadValue("public", "00221", ""))
                    {
                        arayTmp[5] = 1;
                        if (HDLPF.IsRightNumStringMode(str2, 0, 65535))
                        {
                            int intTmp = Convert.ToInt32(str2);
                            arayTmp[7] = Convert.ToByte(intTmp / 256);
                            arayTmp[8] = Convert.ToByte(intTmp % 256);
                        }
                        if (str3.Contains("/"))
                        {
                            string strtemp1 = str3.Split('/')[0].ToString();
                            string strtemp2 = str3.Split('/')[1].ToString();
                            if (HDLPF.IsRightNumStringMode(strtemp1, 0, 255))
                            {
                                string strtemp = GlobalClass.IntToBit(Convert.ToInt32(strtemp1), 6);
                                if (strtemp2 == CsConst.Status[0])
                                    strtemp = "00" + strtemp;
                                else if (strtemp2 == CsConst.Status[1])
                                    strtemp = "01" + strtemp;
                                arayTmp[6] = Convert.ToByte(GlobalClass.BitToInt(strtemp));
                            }
                        }
                    }
                    else if (str1 == CsConst.mstrINIDefault.IniReadValue("public", "00222", ""))
                    {
                        arayTmp[5] = 2;
                        if (HDLPF.IsRightNumStringMode(str2, 0, 255))
                            arayTmp[6] = Convert.ToByte(str2);
                    }
                    else if (str1 == CsConst.mstrINIDefault.IniReadValue("public", "00223", ""))
                    {
                        arayTmp[5] = 3;
                        if (HDLPF.IsRightNumStringMode(str2, 0, 255))
                            arayTmp[6] = Convert.ToByte(str2);
                        if (HDLPF.IsRightNumStringMode(str3, 0, 255))
                            arayTmp[7] = Convert.ToByte(str3);
                    }
                    #endregion
                }
                arayTmp[2] = GetValueAfterChangeType(arayTmp[2]);
                if (CsConst.mySends.AddBufToSndList(arayTmp, 0x1678, SubnetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(MyintDeviceType)) == true)
                {
                    
                    HDLUDP.TimeBetwnNext(20);
                }
                else break;
               
            }
            btnSave.Enabled = true;
            Cursor.Current = Cursors.Default;
        }

        private void cbKey_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                txtFrm.Enabled = true;
                txtTo.Enabled = true;

                if (oSensor != null)
                {
                    Sensor_12in1.IRReceive temp = new Sensor_12in1.IRReceive();
                    bool isExist = false;
                    for (int i = 0; i < oSensor.IrReceiver.Count; i++)
                    {
                        if ((cbPage.SelectedIndex * cbKey.Items.Count + cbKey.SelectedIndex + 1) == oSensor.IrReceiver[i].BtnNum)
                        {
                            temp = oSensor.IrReceiver[i];
                            isExist = true;
                            break;
                        }
                    }
                    if (!isExist)
                    {
                        byte[] ArayTmp = new byte[1];
                        ArayTmp[0] = (byte)(cbPage.SelectedIndex * cbKey.Items.Count + cbKey.SelectedIndex + 1);
                        if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x167A, SubnetID, DeviceID, false, true, true, false) == true)
                        {
                            byte[] arayRemark = new byte[20];
                            for (int intJ = 0; intJ < 20; intJ++) { arayRemark[intJ] = CsConst.myRevBuf[26 + intJ]; }
                            temp.IRBtnRemark = HDLPF.Byte2String(arayRemark);
                            
                        }
                        else
                        {
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                        temp.IRBtnModel = arayMode[cbPage.SelectedIndex * cbKey.Items.Count + cbKey.SelectedIndex];
                    }
                    lbKeyTypeValue.Text = CsConst.mstrINIDefault.IniReadValue("keyMode", "000" + temp.IRBtnModel.ToString("D2"), "");
                    if (lbKeyTypeValue.Text == "") CsConst.mstrINIDefault.IniReadValue("keyMode", "00000", "");
                    lbKeyRemarkValue.Text = temp.IRBtnRemark.ToString();
                    if (temp.IRBtnModel == 0 || temp.IRBtnModel == 1 || temp.IRBtnModel == 2 ||
                        temp.IRBtnModel == 3)
                    {
                        txtFrm.Enabled = false;
                        txtFrm.Text = "1";
                        txtTo.Enabled = false;
                        txtTo.Text = "1";
                        lbTarget.Text = lbTarget.Text.Split('-')[0].ToString() + "-1)";
                    }
                    else
                    {
                        lbTarget.Text = lbTarget.Text.Split('-')[0].ToString() + "-20)";
                        txtFrm.Enabled = true;
                        txtTo.Enabled = true;
                    }
                }
                else if (oSensor_8in1 != null)
                {
                    MultiSensor.IRReceive temp = new MultiSensor.IRReceive();
                    bool isExist = false;
                    for (int i = 0; i < oSensor_8in1.IrReceiver.Count; i++)
                    {
                        if ((cbPage.SelectedIndex * cbKey.Items.Count + cbKey.SelectedIndex + 1) == oSensor_8in1.IrReceiver[i].BtnNum)
                        {
                            temp = oSensor_8in1.IrReceiver[i];
                            isExist = true;
                            break;
                        }
                    }
                    if (!isExist)
                    {
                        byte[] ArayTmp = new byte[1];
                        ArayTmp[0] = (byte)(cbPage.SelectedIndex * cbKey.Items.Count + cbKey.SelectedIndex + 1);
                        if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x167A, SubnetID, DeviceID, false, true, true, false) == true)
                        {
                            byte[] arayRemark = new byte[20];
                            for (int intJ = 0; intJ < 20; intJ++) { arayRemark[intJ] = CsConst.myRevBuf[26 + intJ]; }
                            temp.IRBtnRemark = HDLPF.Byte2String(arayRemark);
                            
                        }
                        else
                        {
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                        temp.IRBtnModel = arayMode[cbPage.SelectedIndex * cbKey.Items.Count + cbKey.SelectedIndex];
                    }
                    lbKeyTypeValue.Text = CsConst.mstrINIDefault.IniReadValue("keyMode", "000" + temp.IRBtnModel.ToString("D2"), "");
                    if (lbKeyTypeValue.Text == "") CsConst.mstrINIDefault.IniReadValue("keyMode", "00000", "");
                    lbKeyRemarkValue.Text = temp.IRBtnRemark.ToString();
                    if (temp.IRBtnModel == 0 || temp.IRBtnModel == 1 || temp.IRBtnModel == 2 ||
                        temp.IRBtnModel == 3)
                    {
                        txtFrm.Enabled = false;
                        txtFrm.Text = "1";
                        txtTo.Enabled = false;
                        txtTo.Text = "1";
                        lbTarget.Text = lbTarget.Text.Split('-')[0].ToString() + "-1)";
                    }
                    else
                    {
                        lbTarget.Text = lbTarget.Text.Split('-')[0].ToString() + "-20)";
                        txtFrm.Enabled = true;
                        txtTo.Enabled = true;
                    }
                }
            }
            catch
            {
            }
            btnSure_Click(null, null);
        }

        private void txtFrm_TextChanged(object sender, EventArgs e)
        {
            if (txtFrm.Text.Length > 0)
            {
                string str = txtFrm.Text;
                int num = Convert.ToInt32(txtTo.Text);
                txtFrm.Text = HDLPF.IsNumStringMode(str, 1, num);
                txtFrm.SelectionStart = txtFrm.Text.Length;
            }
        }

        private void txtTo_TextChanged(object sender, EventArgs e)
        {
            if (txtTo.Text.Length > 0)
            {
                if (oSensor != null)
                {
                    Sensor_12in1.IRReceive temp = new Sensor_12in1.IRReceive();
                    bool isExist = false;
                    for (int i = 0; i < oSensor.IrReceiver.Count; i++)
                    {
                        if ((cbPage.SelectedIndex * cbKey.Items.Count + cbKey.SelectedIndex + 1) == oSensor.IrReceiver[i].BtnNum)
                        {
                            temp = oSensor.IrReceiver[i];
                            isExist = true;
                            break;
                        }
                    }
                    if (!isExist)
                    {
                        temp.IRBtnModel = arayMode[cbPage.SelectedIndex * cbKey.Items.Count + cbKey.SelectedIndex];
                    }

                    string str = txtTo.Text;
                    int num = Convert.ToInt32(txtFrm.Text);
                    if (temp.IRBtnModel == 4 || temp.IRBtnModel == 5 || temp.IRBtnModel == 7)
                        txtTo.Text = HDLPF.IsNumStringMode(str, num, 20);
                    txtTo.SelectionStart = txtTo.Text.Length;
                }
                else if (oSensor_8in1 != null)
                {
                    MultiSensor.IRReceive temp = new MultiSensor.IRReceive();
                    bool isExist = false;
                    for (int i = 0; i < oSensor_8in1.IrReceiver.Count; i++)
                    {
                        if ((cbPage.SelectedIndex * cbKey.Items.Count + cbKey.SelectedIndex + 1) == oSensor_8in1.IrReceiver[i].BtnNum)
                        {
                            temp = oSensor_8in1.IrReceiver[i];
                            isExist = true;
                            break;
                        }
                    }
                    if (!isExist)
                    {
                        temp.IRBtnModel = arayMode[cbPage.SelectedIndex * cbKey.Items.Count + cbKey.SelectedIndex];
                    }

                    string str = txtTo.Text;
                    int num = Convert.ToInt32(txtFrm.Text);
                    if (temp.IRBtnModel == 4 || temp.IRBtnModel == 5 || temp.IRBtnModel == 7)
                        txtTo.Text = HDLPF.IsNumStringMode(str, num, 20);
                    txtTo.SelectionStart = txtTo.Text.Length;
                }
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

        private void cbPage_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbKey_SelectedIndexChanged(null, null);
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

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
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
                    if (txtFrm.Enabled && txtTo.Enabled)
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

        private void rb1_CheckedChanged(object sender, EventArgs e)
        {
            cbKey_SelectedIndexChanged(null, null);
        }

        private void btnClock_Click(object sender, EventArgs e)
        {
            FrmEditClock frmTemp = new FrmEditClock(strName, MyintDeviceType, cbPage.SelectedIndex * 8 + cbKey.SelectedIndex + 1);
            frmTemp.ShowDialog();
        }

        private void rbL1_CheckedChanged(object sender, EventArgs e)
        {
            cbKey_SelectedIndexChanged(null, null);
        }

        private void rbShort_CheckedChanged(object sender, EventArgs e)
        {
            cbKey_SelectedIndexChanged(null, null);
        }
    }
}
