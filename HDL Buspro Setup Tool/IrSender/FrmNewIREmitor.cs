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
    public partial class FrmNewIREmitor : Form
    {
        public FrmNewIREmitor()
        {
            InitializeComponent();
        }
        private byte SubNetID;
        private byte DevID;
        private int mintIDIndex = -1;
        private int mintDeviceType = -1;
        private string mystrName;
        private SendIR tempSend = new SendIR();  //用于存储公共的红外码
        private NewIR myir = null;
        private int MyActivePage = 0; //按页面上传下载
        frmIRLibrary frm;
        private bool isReadding = false;
        private ComboBox cbDev;
        private ComboBox cbState;
        private ComboBox cbChannel;
        private TextBox StepDelay;
        private ComboBox cbEditType;

        private ComboBox cbKeyNo;
        private TimeText ReSendDelay;
        private TextBox txtReSendTimes;

        private ComboBox cbACMode;
        private ComboBox cbACFAN;
        private ComboBox cbWind;
        private ComboBox cbManualWind;
        private TextBox txtTemp;

        private ComboBox cbUVDev;
        private ComboBox cbUVState;
        private ComboBox cbUVChannel;
        private ComboBox cbUVEditType;

        private ComboBox cbUVKeyNo;
        private TimeText UVReSendDelay;
        private TextBox txtUVReSendTimes;

        private ComboBox cbUVACMode;
        private ComboBox cbUVACFAN;
        private ComboBox cbUVWind;
        private ComboBox cbUVManualWind;
        private TextBox txtUVTemp;

        private BackgroundWorker MyBackGroup;
        FrmProcess frmProcessTmp;
        private bool isStopDownloadCodes = false;
        public FrmNewIREmitor(NewIR ir, string strname, int intDIndex, int intDeviceType)
        {
            InitializeComponent();
            this.myir = ir;
            this.mystrName = strname;
            this.mintDeviceType = intDeviceType;
            this.mintIDIndex = intDIndex;

            string strDevName = strname.Split('\\')[0].ToString();
            SubNetID = Convert.ToByte(strDevName.Split('-')[0]);
            DevID = Convert.ToByte(strDevName.Split('-')[1]);
            this.Text = strname;
            tsbl4.Text = strname;
            HDLSysPF.DisplayDeviceNameModeDescription(strDevName, mintDeviceType, cboDevice, tbModel, tbDescription);
        }

        void cbUVEditType_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbUVState.Visible = false;
            cbUVACMode.Visible = false;
            cbUVACFAN.Visible = false;
            cbUVWind.Visible = false;
            cbUVManualWind.Visible = false;
            txtUVTemp.Visible = false;
            cbUVKeyNo.Visible = false;
            txtUVReSendTimes.Visible = false;
            UVReSendDelay.Visible = false;
            if (dgvUV.CurrentRow.Index < 0) return;
            if (dgvUV.RowCount <= 0) return;
            int index = dgvUV.CurrentRow.Index;
            string str = cbUVEditType.Text;
            dgvUV[3, index].Value = str;
            string str1 = dgvUV[4, index].Value.ToString();
            string str2 = dgvUV[5, index].Value.ToString();
            string str3 = dgvUV[6, index].Value.ToString();
            string str4 = dgvUV[7, index].Value.ToString();
            string str5 = dgvUV[8, index].Value.ToString();
            string str6 = dgvUV[9, index].Value.ToString();
            if (str1.Contains('(')) str1 = str1.Split('(')[0].ToString();
            if (str2.Contains('(')) str2 = str2.Split('(')[0].ToString();
            if (str3.Contains('(')) str3 = str3.Split('(')[0].ToString();
            if (str4.Contains('(')) str4 = str4.Split('(')[0].ToString();
            if (str5.Contains('(')) str5 = str5.Split('(')[0].ToString();
            if (str6.Contains('(')) str6 = str6.Split('(')[0].ToString();
            if (cbUVEditType.SelectedIndex == 0)
            {
                addcontrols(4, index, cbUVState, dgvUV);
                addcontrols(5, index, cbUVACMode, dgvUV);
                addcontrols(6, index, cbUVACFAN, dgvUV);
                addcontrols(7, index, cbUVWind, dgvUV);
                addcontrols(8, index, cbUVManualWind, dgvUV);
                addcontrols(9, index, txtUVTemp, dgvUV);
                if (cbUVState.Visible && cbUVState.Items.Count > 0)
                {
                    if (!cbUVState.Items.Contains(str1))
                        cbUVState.SelectedIndex = 0;
                    else
                        cbUVState.Text = str1;
                }
            }
            else if (cbUVEditType.SelectedIndex == 1)
            {
                for (int i = 0; i < dgvUV.SelectedRows.Count; i++)
                {
                    dgvUV.SelectedRows[i].Cells[8].Value = "N/A";
                    dgvUV.SelectedRows[i].Cells[9].Value = "N/A";
                }
                int devid = myir.IRCodes[Convert.ToInt32(cbUVDev.Text.Split('-')[0]) + 3].DevID;
                if(mintDeviceType==6100)
                    devid = myir.IRCodes[Convert.ToInt32(cbUVDev.Text.Split('-')[0]) + 2].DevID;
                cbUVKeyNo.Items.Clear();
                cbUVKeyNo.Items.AddRange(CsConst.mstrINIDefault.IniReadValuesInALlSectionStr("NewIRDevice" + devid.ToString()));
                addcontrols(4, index, cbUVKeyNo, dgvUV);
                addcontrols(5, index, txtUVReSendTimes, dgvUV);
                addcontrols(6, index, UVReSendDelay, dgvUV);
                addcontrols(7, index, cbUVState, dgvUV);
                if (cbUVState.Visible && cbUVState.Items.Count > 0)
                {
                    if (!cbUVState.Items.Contains(str4))
                        cbUVState.SelectedIndex = 0;
                    else
                        cbUVState.Text = str4;
                }
            }

            if (cbUVACMode.Visible && cbUVACMode.Items.Count > 0)
            {
                if (!cbUVACMode.Items.Contains(str2))
                    cbUVACMode.SelectedIndex = 0;
                else
                    cbUVACMode.Text = str2;
            }
            if (cbUVACFAN.Visible && cbUVACFAN.Items.Count > 0)
            {
                if (!cbUVACFAN.Items.Contains(str3))
                    cbUVACFAN.SelectedIndex = 0;
                else
                    cbUVACFAN.Text = str3;
            }
            if (cbUVWind.Visible && cbUVWind.Items.Count > 0)
            {
                if (!cbUVWind.Items.Contains(str4))
                    cbUVWind.SelectedIndex = 0;
                else
                    cbUVWind.Text = str4;
            }
            if (cbUVManualWind.Visible && cbUVManualWind.Items.Count > 0)
            {
                if (!cbUVManualWind.Items.Contains(str5))
                    cbUVManualWind.SelectedIndex = 0;
                else
                    cbUVManualWind.Text = str5;
            }
            if (cbUVKeyNo.Visible && cbUVKeyNo.Items.Count > 0)
            {
                if (!cbUVKeyNo.Items.Contains(str1))
                    cbUVKeyNo.SelectedIndex = 0;
                else
                    cbUVKeyNo.Text = str1;
            }
            if (UVReSendDelay.Visible)
            {
                if (str3.Contains("."))
                    UVReSendDelay.Text = HDLPF.GetTimeFromString(str3, '.');
                else
                    UVReSendDelay.Text = "1";
            }
            if (txtUVReSendTimes.Visible) txtUVReSendTimes.Text = str2;
            if (txtUVTemp.Visible) txtUVTemp.Text = str6;

            if (cbUVACMode.Visible) cbUVACMode_SelectedIndexChanged(null, null);
            if (cbUVACFAN.Visible) cbUVACFAN_SelectedIndexChanged(null, null);
            if (cbUVWind.Visible) cbUVWind_SelectedIndexChanged(null, null);
            if (txtUVTemp.Visible) txtUVTemp_TextChanged(null, null);
            if (cbUVManualWind.Visible) cbUVManualWind_SelectedIndexChanged(null, null);
            if (cbUVKeyNo.Visible) cbUVKeyNo_SelectedIndexChanged(null, null);
            if (txtUVReSendTimes.Visible) txtUVReSendTimes_TextChanged(null, null);
            if (UVReSendDelay.Visible) UVReSendDelay_TextChanged(null, null);
            if (cbUVState.Visible) cbUVState_SelectedIndexChanged(null, null);
        }

        void cbEditType_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbState.Visible = false;
            cbACMode.Visible = false;
            cbACFAN.Visible = false;
            cbWind.Visible = false;
            cbManualWind.Visible = false;
            txtTemp.Visible = false;
            cbKeyNo.Visible = false;
            txtReSendTimes.Visible = false;
            ReSendDelay.Visible = false;
            if (dgvSequence.CurrentRow.Index < 0) return;
            if (dgvSequence.RowCount <= 0) return;
            int index = dgvSequence.CurrentRow.Index;
            string str = cbEditType.Text;
            dgvSequence[3, index].Value = str;
            string str1 = dgvSequence[5, index].Value.ToString();
            string str2 = dgvSequence[6, index].Value.ToString();
            string str3 = dgvSequence[7, index].Value.ToString();
            string str4 = dgvSequence[8, index].Value.ToString();
            string str5 = dgvSequence[9, index].Value.ToString();
            string str6 = dgvSequence[10, index].Value.ToString();
            if (str1.Contains('(')) str1 = str1.Split('(')[0].ToString();
            if (str2.Contains('(')) str2 = str2.Split('(')[0].ToString();
            if (str3.Contains('(')) str3 = str3.Split('(')[0].ToString();
            if (str4.Contains('(')) str4 = str4.Split('(')[0].ToString();
            if (str5.Contains('(')) str5 = str5.Split('(')[0].ToString();
            if (str6.Contains('(')) str6 = str6.Split('(')[0].ToString();
            if (cbEditType.SelectedIndex == 0)
            {
                addcontrols(5, index, cbState, dgvSequence);
                addcontrols(6, index, cbACMode, dgvSequence);
                addcontrols(7, index, cbACFAN, dgvSequence);
                addcontrols(8, index, cbWind, dgvSequence);
                addcontrols(9, index, cbManualWind, dgvSequence);
                addcontrols(10, index, txtTemp, dgvSequence);
                if (cbState.Visible && cbState.Items.Count > 0)
                {
                    if (!cbState.Items.Contains(str1))
                        cbState.SelectedIndex = 0;
                    else
                        cbState.Text = str1;
                }
            }
            else if (cbEditType.SelectedIndex == 1)
            {
                for (int i = 0; i < dgvSequence.SelectedRows.Count; i++)
                {
                    dgvSequence.SelectedRows[i].Cells[9].Value = "N/A";
                    dgvSequence.SelectedRows[i].Cells[10].Value = "N/A";
                }
                int devid = myir.IRCodes[Convert.ToInt32(cbDev.Text.Split('-')[0]) + 3].DevID;
                if (mintDeviceType == 6100) devid = myir.IRCodes[Convert.ToInt32(cbDev.Text.Split('-')[0]) + 2].DevID;
                cbKeyNo.Items.Clear();
                cbKeyNo.Items.AddRange(CsConst.mstrINIDefault.IniReadValuesInALlSectionStr("NewIRDevice" + devid.ToString()));
                addcontrols(5, index, cbKeyNo, dgvSequence);
                addcontrols(6, index, txtReSendTimes, dgvSequence);
                addcontrols(7, index, ReSendDelay, dgvSequence);
                addcontrols(8, index, cbState, dgvSequence);
                if (cbState.Visible && cbState.Items.Count > 0)
                {
                    if (!cbState.Items.Contains(str4))
                        cbState.SelectedIndex = 0;
                    else
                        cbState.Text = str4;
                }
            }

            if (cbACMode.Visible && cbACMode.Items.Count > 0)
            {
                if (!cbACMode.Items.Contains(str2))
                    cbACMode.SelectedIndex = 0;
                else
                    cbACMode.Text = str2;
            }
            if (cbACFAN.Visible && cbACFAN.Items.Count > 0)
            {
                if (!cbACFAN.Items.Contains(str3))
                    cbACFAN.SelectedIndex = 0;
                else
                    cbACFAN.Text = str3;
            }
            if (cbWind.Visible && cbWind.Items.Count > 0)
            {
                if (!cbWind.Items.Contains(str4))
                    cbWind.SelectedIndex = 0;
                else
                    cbWind.Text = str4;
            }
            if (cbManualWind.Visible && cbManualWind.Items.Count > 0)
            {
                if (!cbManualWind.Items.Contains(str5))
                    cbManualWind.SelectedIndex = 0;
                else
                    cbManualWind.Text = str5;
            }
            if (cbKeyNo.Visible && cbKeyNo.Items.Count > 0)
            {
                if (!cbKeyNo.Items.Contains(str1))
                    cbKeyNo.SelectedIndex = 0;
                else
                    cbKeyNo.Text = str1;
            }

            if (txtReSendTimes.Visible) txtReSendTimes.Text = str2;
            if (ReSendDelay.Visible)
            {
                if (str3.Contains("."))
                    ReSendDelay.Text = HDLPF.GetTimeFromString(str3, '.');
                else
                    ReSendDelay.Text = "1";
            }
            if (txtTemp.Visible) txtTemp.Text = str6;

            if (cbACMode.Visible) cbACMode_SelectedIndexChanged(null, null);
            if (cbACFAN.Visible) cbACFAN_SelectedIndexChanged(null, null);
            if (cbWind.Visible) cbWind_SelectedIndexChanged(null, null);
            if (txtTemp.Visible) txtTemp_TextChanged(null, null);
            if (cbManualWind.Visible) cbManualWind_SelectedIndexChanged(null, null);
            if (cbKeyNo.Visible) cbKeyNo_SelectedIndexChanged(null, null);
            if (txtReSendTimes.Visible) txtReSendTimes_TextChanged(null, null);
            if (ReSendDelay.Visible) ReSendDelay_TextChanged(null, null);
            if (cbState.Visible) cbState_SelectedIndexChanged(null, null);
        }

        void txtUVTemp_TextChanged(object sender, EventArgs e)
        {
            if (dgvUV.CurrentRow.Index < 0) return;
            if (dgvUV.RowCount <= 0) return;
            if (txtUVTemp.Text.Length > 0)
            {
                int index = dgvUV.CurrentRow.Index;
                string str = txtUVTemp.Text;
                txtUVTemp.Text = HDLPF.IsNumStringMode(str, 0, 255);
                txtUVTemp.SelectionStart = txtUVTemp.Text.Length;
                dgvUV[9, index].Value = str + "(" + CsConst.mstrINIDefault.IniReadValue("public", "99874", "") + ")";
                if (dgvUV.SelectedRows.Count > 1)
                {
                    for (int i = 0; i < dgvUV.SelectedRows.Count; i++)
                    {
                        for (int j = 1; j < dgvUV.SelectedRows[i].Cells.Count; j++)
                        {
                            dgvUV.SelectedRows[i].Cells[j].Value = dgvUV[j, index].Value.ToString();
                        }
                    }
                }
            }
        }

        void cbUVManualWind_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dgvUV.CurrentRow.Index < 0) return;
            if (dgvUV.RowCount <= 0) return;
            int index = dgvUV.CurrentRow.Index;
            string str = cbUVManualWind.Text;
            dgvUV[8, index].Value = str;
            if (dgvUV.SelectedRows.Count > 1)
            {
                for (int i = 0; i < dgvUV.SelectedRows.Count; i++)
                {
                    for (int j = 1; j < dgvUV.SelectedRows[i].Cells.Count; j++)
                    {
                        dgvUV.SelectedRows[i].Cells[j].Value = dgvUV[j, index].Value.ToString();
                    }
                }
            }
        }

        void cbUVState_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dgvUV.CurrentRow.Index < 0) return;
            if (dgvUV.RowCount <= 0) return;
            int index = dgvUV.CurrentRow.Index;
            string str = cbUVState.Text;
            int devIndex = 0;
            if (cbUVDev.SelectedIndex < (cbUVDev.Items.Count - 1))
            {
                if (mintDeviceType == 6100)
                {
                    if (cbUVDev.SelectedIndex <= 3) devIndex = 1;
                    else devIndex = 2;
                }
                else
                {
                    if (cbUVDev.SelectedIndex <= 4) devIndex = 1;
                    else devIndex = 2;
                }
            }
            else if (cbUVDev.SelectedIndex == (cbUVDev.Items.Count - 1))
            {
                devIndex = 2;
            }
            if (devIndex == 1)
            {
                if (cbUVEditType.SelectedIndex == 0)
                {
                    dgvUV[4, index].Value = str + "(" + CsConst.mstrINIDefault.IniReadValue("public", "99620", "") + ")";
                    if (dgvUV.SelectedRows.Count > 1)
                    {
                        for (int i = 0; i < dgvUV.SelectedRows.Count; i++)
                        {
                            for (int j = 1; j < dgvUV.SelectedRows[i].Cells.Count; j++)
                            {
                                dgvUV.SelectedRows[i].Cells[j].Value = dgvUV[j, index].Value.ToString();
                            }
                        }
                    }
                }
                else if (cbUVEditType.SelectedIndex == 1)
                {
                    dgvUV[7, index].Value = str + "(" + CsConst.mstrINIDefault.IniReadValue("public", "99621", "") + ")";
                    if (dgvUV.SelectedRows.Count > 1)
                    {
                        for (int i = 0; i < dgvUV.SelectedRows.Count; i++)
                        {
                            for (int j = 1; j < dgvUV.SelectedRows[i].Cells.Count; j++)
                            {
                                dgvUV.SelectedRows[i].Cells[j].Value = dgvUV[j, index].Value.ToString();
                            }
                        }
                    }
                }
            }
            else if (devIndex == 2)
            {
                dgvUV[7, index].Value = str + "(" + CsConst.mstrINIDefault.IniReadValue("public", "99621", "") + ")";
                if (dgvUV.SelectedRows.Count > 1)
                {
                    for (int i = 0; i < dgvUV.SelectedRows.Count; i++)
                    {
                        for (int j = 1; j < dgvUV.SelectedRows[i].Cells.Count; j++)
                        {
                            dgvUV.SelectedRows[i].Cells[j].Value = dgvUV[j, index].Value.ToString();
                        }
                    }
                }
            }
        }

        void UVReSendDelay_TextChanged(object sender, EventArgs e)
        {
            if (dgvUV.CurrentRow.Index < 0) return;
            if (dgvUV.RowCount <= 0) return;
            int index = dgvUV.CurrentRow.Index;
            string str = HDLPF.GetStringFromTime(Convert.ToInt32(UVReSendDelay.Text), ".");
            dgvUV[6, index].Value = str + "(" + CsConst.mstrINIDefault.IniReadValue("public", "99900", "") + ")";
            if (dgvUV.SelectedRows.Count > 1)
            {
                for (int i = 0; i < dgvUV.SelectedRows.Count; i++)
                {
                    for (int j = 1; j < dgvUV.SelectedRows[i].Cells.Count; j++)
                    {
                        dgvUV.SelectedRows[i].Cells[j].Value = dgvUV[j, index].Value.ToString();
                    }
                }
            }
        }

        void cbUVWind_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dgvUV.CurrentRow.Index < 0) return;
            if (dgvUV.RowCount <= 0) return;
            int index = dgvUV.CurrentRow.Index;
            string str = cbUVWind.Text;
            dgvUV[7, index].Value = str;
            if (dgvUV.SelectedRows.Count > 1)
            {
                for (int i = 0; i < dgvUV.SelectedRows.Count; i++)
                {
                    for (int j = 1; j < dgvUV.SelectedRows[i].Cells.Count; j++)
                    {
                        dgvUV.SelectedRows[i].Cells[j].Value = dgvUV[j, index].Value.ToString();
                    }
                }
            }
        }

        void cbUVACFAN_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dgvUV.CurrentRow.Index < 0) return;
            if (dgvUV.RowCount <= 0) return;
            int index = dgvUV.CurrentRow.Index;
            string str = cbUVACFAN.Text;
            dgvUV[6, index].Value = str + "(" + CsConst.mstrINIDefault.IniReadValue("public", "99875", "") + ")";
            if (dgvUV.SelectedRows.Count > 1)
            {
                for (int i = 0; i < dgvUV.SelectedRows.Count; i++)
                {
                    for (int j = 1; j < dgvUV.SelectedRows[i].Cells.Count; j++)
                    {
                        dgvUV.SelectedRows[i].Cells[j].Value = dgvUV[j, index].Value.ToString();
                    }
                }
            }
        }

        void cbUVACMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dgvUV.CurrentRow.Index < 0) return;
            if (dgvUV.RowCount <= 0) return;
            int index = dgvUV.CurrentRow.Index;
            string str = cbUVACMode.Text;
            dgvUV[5, index].Value = str + "(" + CsConst.mstrINIDefault.IniReadValue("public", "99876", "") + ")";
            if (dgvUV.SelectedRows.Count > 1)
            {
                for (int i = 0; i < dgvUV.SelectedRows.Count; i++)
                {
                    for (int j = 1; j < dgvUV.SelectedRows[i].Cells.Count; j++)
                    {
                        dgvUV.SelectedRows[i].Cells[j].Value = dgvUV[j, index].Value.ToString();
                    }
                }
            }
        }

        void txtUVReSendTimes_TextChanged(object sender, EventArgs e)
        {
            if (dgvUV.CurrentRow.Index < 0) return;
            if (dgvUV.RowCount <= 0) return;
            if (txtUVReSendTimes.Text.Length > 0)
            {
                int index = dgvUV.CurrentRow.Index;
                string str = txtUVReSendTimes.Text;
                txtUVReSendTimes.Text = HDLPF.IsNumStringMode(str, 0, 65535);
                txtUVReSendTimes.SelectionStart = txtUVReSendTimes.Text.Length;
                dgvUV[5, index].Value = str + "(" + CsConst.mstrINIDefault.IniReadValue("public", "99899", "") + ")";
                if (dgvUV.SelectedRows.Count > 1)
                {
                    for (int i = 0; i < dgvUV.SelectedRows.Count; i++)
                    {
                        for (int j = 1; j < dgvUV.SelectedRows[i].Cells.Count; j++)
                        {
                            dgvUV.SelectedRows[i].Cells[j].Value = dgvUV[j, index].Value.ToString();
                        }
                    }
                }
            }
        }

        void cbUVKeyNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dgvUV.CurrentRow.Index < 0) return;
            if (dgvUV.RowCount <= 0) return;
            int index = dgvUV.CurrentRow.Index;
            string str = cbUVKeyNo.Text;
            dgvUV[4, index].Value = cbUVKeyNo.Text + "(" + CsConst.mstrINIDefault.IniReadValue("public", "99898", "") + ")";
            if (dgvUV.SelectedRows.Count > 1)
            {
                for (int i = 0; i < dgvUV.SelectedRows.Count; i++)
                {
                    for (int j = 1; j < dgvUV.SelectedRows[i].Cells.Count; j++)
                    {
                        dgvUV.SelectedRows[i].Cells[j].Value = dgvUV[j, index].Value.ToString();
                    }
                }
            }
        }

        void cbUVDev_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dgvUV.CurrentRow.Index < 0) return;
            if (dgvUV.RowCount <= 0) return;
            try
            {
                isReadding = true;
                cbUVChannel.Visible = false;
                cbUVState.Visible = false;
                cbUVKeyNo.Visible = false;
                txtUVReSendTimes.Visible = false;
                UVReSendDelay.Visible = false;
                cbUVACMode.Visible = false;
                cbUVACFAN.Visible = false;
                cbUVWind.Visible = false;
                txtUVTemp.Visible = false;
                cbUVManualWind.Visible = false;
                cbUVEditType.Visible = false;
                int index = dgvUV.CurrentRow.Index;
                string str = cbUVDev.Text;
                string strChannel = dgvUV[2, index].Value.ToString();
                string strStatus = dgvUV[3, index].Value.ToString();
                string str1 = dgvUV[4, index].Value.ToString();
                string str2 = dgvUV[5, index].Value.ToString();
                string str3 = dgvUV[6, index].Value.ToString();
                string str4 = dgvUV[7, index].Value.ToString();
                string str5 = dgvUV[8, index].Value.ToString();
                string str6 = dgvUV[9, index].Value.ToString();
                if (str1.Contains('(')) str1 = str1.Split('(')[0].ToString();
                if (str2.Contains('(')) str2 = str2.Split('(')[0].ToString();
                if (str3.Contains('(')) str3 = str3.Split('(')[0].ToString();
                if (str4.Contains('(')) str4 = str4.Split('(')[0].ToString();
                if (str5.Contains('(')) str5 = str5.Split('(')[0].ToString();
                if (str6.Contains('(')) str6 = str6.Split('(')[0].ToString();
                if (cbUVDev.SelectedIndex == 0)
                {
                    for (int i = 2; i <= 9; i++)
                    {
                        for (int j = 0; j < dgvUV.SelectedRows.Count; j++)
                        {
                            dgvUV.SelectedRows[j].Cells[i].Value = "N/A";
                        }
                    }
                    dgvUV[1, index].Value = str;
                    ModifyMultilinesIfNeeds(dgvUV[1, index].Value.ToString(), 1, dgvUV);
                }
                else if (cbUVDev.SelectedIndex < cbUVDev.Items.Count - 1)
                {
                    int devIndex = 0;
                    if (mintDeviceType == 6100)
                    {
                        if (cbUVDev.SelectedIndex <= 3) devIndex = 1;
                        else devIndex = 2;
                    }
                    else
                    {
                        if (cbUVDev.SelectedIndex <= 4) devIndex = 1;
                        else devIndex = 2;
                    }
                    if (devIndex != 0)
                    {
                        addcontrols(2, index, cbUVChannel, dgvUV);
                        dgvUV[1, index].Value = str;
                        ModifyMultilinesIfNeeds(dgvUV[1, index].Value.ToString(), 1, dgvUV);
                    }
                    if (devIndex == 2)
                    {
                        for (int i = 0; i < dgvUV.SelectedRows.Count; i++)
                        {
                            dgvUV.SelectedRows[i].Cells[3].Value = cbUVEditType.Items[1].ToString();
                            dgvUV.SelectedRows[i].Cells[8].Value = "N/A";
                            dgvUV.SelectedRows[i].Cells[9].Value = "N/A";
                        }
                        int devid = myir.IRCodes[Convert.ToInt32(cbUVDev.Text.Split('-')[0]) + 3].DevID;
                        if(mintDeviceType==6100)
                            devid = myir.IRCodes[Convert.ToInt32(cbUVDev.Text.Split('-')[0]) + 2].DevID;
                        cbUVKeyNo.Items.Clear();
                        cbUVKeyNo.Items.AddRange(CsConst.mstrINIDefault.IniReadValuesInALlSectionStr("NewIRDevice" + devid.ToString()));
                        addcontrols(4, index, cbUVKeyNo, dgvUV);
                        addcontrols(5, index, txtUVReSendTimes, dgvUV);
                        addcontrols(6, index, UVReSendDelay, dgvUV);
                        addcontrols(7, index, cbUVState, dgvUV);
                        if (cbUVState.Visible && cbUVState.Items.Count > 0)
                        {
                            if (!cbUVState.Items.Contains(str4))
                                cbUVState.SelectedIndex = 0;
                            else
                                cbUVState.Text = str4;
                        }
                    }
                    else if (devIndex == 1)
                    {
                        cbUVEditType.SelectedIndex = 0;
                        addcontrols(3, index, cbUVEditType, dgvUV);
                    }
                }
                else if (cbUVDev.SelectedIndex == cbUVDev.Items.Count - 1)
                {
                    for (int i = 0; i < dgvUV.SelectedRows.Count; i++)
                    {
                        dgvUV.SelectedRows[i].Cells[3].Value = cbUVEditType.Items[1].ToString();
                        dgvUV.SelectedRows[i].Cells[8].Value = "N/A";
                        dgvUV.SelectedRows[i].Cells[9].Value = "N/A";
                    }
                    addcontrols(2, index, cbUVChannel, dgvUV);
                    dgvUV[1, index].Value = str;
                    ModifyMultilinesIfNeeds(dgvUV[1, index].Value.ToString(), 1, dgvUV);

                    cbUVKeyNo.Items.Clear();
                    for (int i = 1; i <= 100; i++)
                        cbUVKeyNo.Items.Add(i.ToString());
                    addcontrols(4, index, cbUVKeyNo, dgvUV);
                    addcontrols(5, index, txtUVReSendTimes, dgvUV);
                    addcontrols(6, index, UVReSendDelay, dgvUV);
                    addcontrols(7, index, cbUVState, dgvUV);
                    if (cbUVState.Visible && cbUVState.Items.Count > 0)
                    {
                        if (!cbUVState.Items.Contains(str4))
                            cbUVState.SelectedIndex = 0;
                        else
                            cbUVState.Text = str4;
                    }
                }

                /*if (cbUVEditType.Visible && cbUVEditType.Items.Count > 0)
                {
                    if (!cbUVEditType.Items.Contains(strStatus))
                        cbUVEditType.SelectedIndex = 0;
                    else
                        cbUVEditType.Text = strStatus;
                }*/
                if (cbUVChannel.Visible && cbUVChannel.Items.Count > 0)
                {
                    if (!cbUVChannel.Items.Contains(strChannel))
                        cbUVChannel.SelectedIndex = 0;
                    else
                        cbUVChannel.Text = strChannel;
                }
                if (cbUVKeyNo.Visible && cbUVKeyNo.Items.Count > 0)
                {
                    if (!cbUVKeyNo.Items.Contains(str1))
                        cbUVKeyNo.SelectedIndex = 0;
                    else
                        cbUVKeyNo.Text = str1;
                }
                if (txtUVReSendTimes.Visible) txtUVReSendTimes.Text = str2;
                if (UVReSendDelay.Visible)
                {
                    if (str4.Contains("."))
                        UVReSendDelay.Text = HDLPF.GetTimeFromString(str3, '.');
                    else
                        UVReSendDelay.Text = "1";
                }


                if (cbUVEditType.Visible) cbUVEditType_SelectedIndexChanged(null, null);
                if (cbUVChannel.Visible) cbUVChannel_SelectedIndexChanged(null, null);
                if (cbUVState.Visible) cbUVState_SelectedIndexChanged(null, null);
                if (cbUVKeyNo.Visible) cbUVKeyNo_SelectedIndexChanged(null, null);
                if (txtUVReSendTimes.Visible) txtUVReSendTimes_TextChanged(null, null);
                if (UVReSendDelay.Visible) UVReSendDelay_TextChanged(null, null);
            }
            catch
            {
            }
            isReadding = false;
        }

        void cbUVChannel_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dgvUV.CurrentRow.Index < 0) return;
            if (dgvUV.RowCount <= 0) return;
            int index = dgvUV.CurrentRow.Index;
            string str = cbUVChannel.Text;
            dgvUV[2, index].Value = str;
            if (dgvUV.SelectedRows.Count > 1)
            {
                for (int i = 0; i < dgvUV.SelectedRows.Count; i++)
                {
                    for (int j = 1; j < dgvUV.SelectedRows[i].Cells.Count; j++)
                    {
                        dgvUV.SelectedRows[i].Cells[j].Value = dgvUV[j, index].Value.ToString();
                    }
                }
            }
        }

        void cbManualWind_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dgvSequence.CurrentRow.Index < 0) return;
            if (dgvSequence.RowCount <= 0) return;
            int index = dgvSequence.CurrentRow.Index;
            string str = cbManualWind.Text;
            dgvSequence[9, index].Value = str;
            if (dgvSequence.SelectedRows.Count > 1)
            {
                for (int i = 0; i < dgvSequence.SelectedRows.Count; i++)
                {
                    for (int j = 1; j < dgvSequence.SelectedRows[i].Cells.Count; j++)
                    {
                        dgvSequence.SelectedRows[i].Cells[j].Value = dgvSequence[j, index].Value.ToString();
                    }
                }
            }
        }

        void cbKeyNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dgvSequence.CurrentRow.Index < 0) return;
            if (dgvSequence.RowCount <= 0) return;
            int index = dgvSequence.CurrentRow.Index;
            string str = cbKeyNo.Text;
            dgvSequence[5, index].Value = cbKeyNo.Text + "(" + CsConst.mstrINIDefault.IniReadValue("public", "99898", "") + ")";
            if (dgvSequence.SelectedRows.Count > 1)
            {
                for (int i = 0; i < dgvSequence.SelectedRows.Count; i++)
                {
                    for (int j = 1; j < dgvSequence.SelectedRows[i].Cells.Count; j++)
                    {
                        dgvSequence.SelectedRows[i].Cells[j].Value = dgvSequence[j, index].Value.ToString();
                    }
                }
            }
        }


        void cbACMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dgvSequence.CurrentRow.Index < 0) return;
            if (dgvSequence.RowCount <= 0) return;
            int index = dgvSequence.CurrentRow.Index;
            string str = cbACMode.Text;
            dgvSequence[6, index].Value = str + "(" + CsConst.mstrINIDefault.IniReadValue("public", "99876", "") + ")";
            if (dgvSequence.SelectedRows.Count > 1)
            {
                for (int i = 0; i < dgvSequence.SelectedRows.Count; i++)
                {
                    for (int j = 1; j < dgvSequence.SelectedRows[i].Cells.Count; j++)
                    {
                        dgvSequence.SelectedRows[i].Cells[j].Value = dgvSequence[j, index].Value.ToString();
                    }
                }
            }
        }

        void cbACFAN_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dgvSequence.CurrentRow.Index < 0) return;
            if (dgvSequence.RowCount <= 0) return;
            int index = dgvSequence.CurrentRow.Index;
            string str = cbACFAN.Text;
            dgvSequence[7, index].Value = str + "(" + CsConst.mstrINIDefault.IniReadValue("public", "99875", "") + ")";
            if (dgvSequence.SelectedRows.Count > 1)
            {
                for (int i = 0; i < dgvSequence.SelectedRows.Count; i++)
                {
                    for (int j = 1; j < dgvSequence.SelectedRows[i].Cells.Count; j++)
                    {
                        dgvSequence.SelectedRows[i].Cells[j].Value = dgvSequence[j, index].Value.ToString();
                    }
                }
            }
        }

        void txtTemp_TextChanged(object sender, EventArgs e)
        {
            if (dgvSequence.CurrentRow.Index < 0) return;
            if (dgvSequence.RowCount <= 0) return;
            if (txtTemp.Text.Length > 0)
            {
                int index = dgvSequence.CurrentRow.Index;
                string str = txtTemp.Text;
                txtTemp.Text = HDLPF.IsNumStringMode(str, 0, 255);
                txtTemp.SelectionStart = txtTemp.Text.Length;
                dgvSequence[10, index].Value = str + "(" + CsConst.mstrINIDefault.IniReadValue("public", "99874", "") + ")";
                if (dgvSequence.SelectedRows.Count > 1)
                {
                    for (int i = 0; i < dgvSequence.SelectedRows.Count; i++)
                    {
                        for (int j = 1; j < dgvSequence.SelectedRows[i].Cells.Count; j++)
                        {
                            dgvSequence.SelectedRows[i].Cells[j].Value = dgvSequence[j, index].Value.ToString();
                        }
                    }
                }
            }
        }

        void cbWind_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dgvSequence.CurrentRow.Index < 0) return;
            if (dgvSequence.RowCount <= 0) return;
            int index = dgvSequence.CurrentRow.Index;
            string str = cbWind.Text;
            dgvSequence[8, index].Value = str;
            if (dgvSequence.SelectedRows.Count > 1)
            {
                for (int i = 0; i < dgvSequence.SelectedRows.Count; i++)
                {
                    for (int j = 1; j < dgvSequence.SelectedRows[i].Cells.Count; j++)
                    {
                        dgvSequence.SelectedRows[i].Cells[j].Value = dgvSequence[j, index].Value.ToString();
                    }
                }
            }
        }

        void txtReSendTimes_TextChanged(object sender, EventArgs e)
        {
            if (dgvSequence.CurrentRow.Index < 0) return;
            if (dgvSequence.RowCount <= 0) return;
            if (txtReSendTimes.Text.Length > 0)
            {
                int index = dgvSequence.CurrentRow.Index;
                string str = txtReSendTimes.Text;
                txtReSendTimes.Text = HDLPF.IsNumStringMode(str, 0, 65535);
                txtReSendTimes.SelectionStart = txtReSendTimes.Text.Length;
                dgvSequence[6, index].Value = str + "(" + CsConst.mstrINIDefault.IniReadValue("public", "99899", "") + ")";
                if (dgvSequence.SelectedRows.Count > 1)
                {
                    for (int i = 0; i < dgvSequence.SelectedRows.Count; i++)
                    {
                        for (int j = 1; j < dgvSequence.SelectedRows[i].Cells.Count; j++)
                        {
                            dgvSequence.SelectedRows[i].Cells[j].Value = dgvSequence[j, index].Value.ToString();
                        }
                    }
                }
            }
        }

        void StepDelay_TextChanged(object sender, EventArgs e)
        {
            if (dgvSequence.CurrentRow.Index < 0) return;
            if (dgvSequence.RowCount <= 0) return;
            if (StepDelay.Text.Length > 0)
            {
                int index = dgvSequence.CurrentRow.Index;
                string str = StepDelay.Text;
                StepDelay.Text = HDLPF.IsNumStringMode(str, 0, 255);
                StepDelay.SelectionStart = StepDelay.Text.Length;
                dgvSequence[4, index].Value = StepDelay.Text;
                if (dgvSequence.SelectedRows.Count > 1)
                {
                    for (int i = 0; i < dgvSequence.SelectedRows.Count; i++)
                    {
                        for (int j = 1; j < dgvSequence.SelectedRows[i].Cells.Count; j++)
                        {
                            dgvSequence.SelectedRows[i].Cells[j].Value = dgvSequence[j, index].Value.ToString();
                        }
                    }
                }
            }
        }

        void cbChannel_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dgvSequence.CurrentRow.Index < 0) return;
            if (dgvSequence.RowCount <= 0) return;
            int index = dgvSequence.CurrentRow.Index;
            string str = cbChannel.Text;
            dgvSequence[2, index].Value = str;
            if (dgvSequence.SelectedRows.Count > 1)
            {
                for (int i = 0; i < dgvSequence.SelectedRows.Count; i++)
                {
                    for (int j = 1; j < dgvSequence.SelectedRows[i].Cells.Count; j++)
                    {
                        dgvSequence.SelectedRows[i].Cells[j].Value = dgvSequence[j, index].Value.ToString();
                    }
                }
            }
        }

        void cbState_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dgvSequence.CurrentRow.Index < 0) return;
            if (dgvSequence.RowCount <= 0) return;
            int index = dgvSequence.CurrentRow.Index;
            string str = cbState.Text;
            int devIndex = 0;
            if (cbDev.SelectedIndex < (cbDev.Items.Count - 1))
            {
                if (mintDeviceType == 6100)
                {
                    if (cbDev.SelectedIndex <= 3) devIndex = 1;
                    else devIndex = 2;
                }
                else
                {
                    if (cbDev.SelectedIndex <= 4) devIndex = 1;
                    else devIndex = 2;
                }
            }
            else if (cbDev.SelectedIndex == cbDev.Items.Count - 1)
            {
                devIndex = 2;
            }
            if (devIndex == 1)
            {
                if (cbEditType.SelectedIndex == 0)
                {
                    dgvSequence[5, index].Value = str + "(" + CsConst.mstrINIDefault.IniReadValue("public", "99620", "") + ")";
                    if (dgvSequence.SelectedRows.Count > 1)
                    {
                        for (int i = 0; i < dgvSequence.SelectedRows.Count; i++)
                        {
                            for (int j = 1; j < dgvSequence.SelectedRows[i].Cells.Count; j++)
                            {
                                dgvSequence.SelectedRows[i].Cells[j].Value = dgvSequence[j, index].Value.ToString();
                            }
                        }
                    }
                }
                else if (cbEditType.SelectedIndex == 1)
                {
                    dgvSequence[8, index].Value = str + "(" + CsConst.mstrINIDefault.IniReadValue("public", "99621", "") + ")";
                    if (dgvSequence.SelectedRows.Count > 1)
                    {
                        for (int i = 0; i < dgvSequence.SelectedRows.Count; i++)
                        {
                            for (int j = 1; j < dgvSequence.SelectedRows[i].Cells.Count; j++)
                            {
                                dgvSequence.SelectedRows[i].Cells[j].Value = dgvSequence[j, index].Value.ToString();
                            }
                        }
                    }
                }
            }
            else if (devIndex == 2)
            {
                dgvSequence[8, index].Value = str + "(" + CsConst.mstrINIDefault.IniReadValue("public", "99621", "") + ")";
                if (dgvSequence.SelectedRows.Count > 1)
                {
                    for (int i = 0; i < dgvSequence.SelectedRows.Count; i++)
                    {
                        for (int j = 1; j < dgvSequence.SelectedRows[i].Cells.Count; j++)
                        {
                            dgvSequence.SelectedRows[i].Cells[j].Value = dgvSequence[j, index].Value.ToString();
                        }
                    }
                }
            }
        }

        void ReSendDelay_TextChanged(object sender, EventArgs e)
        {
            if (dgvSequence.CurrentRow.Index < 0) return;
            if (dgvSequence.RowCount <= 0) return;
            int index = dgvSequence.CurrentRow.Index;
            string str = HDLPF.GetStringFromTime(Convert.ToInt32(ReSendDelay.Text), ".");
            dgvSequence[7, index].Value = str + "(" + CsConst.mstrINIDefault.IniReadValue("public", "99900", "") + ")";
            if (dgvSequence.SelectedRows.Count > 1)
            {
                for (int i = 0; i < dgvSequence.SelectedRows.Count; i++)
                {
                    for (int j = 1; j < dgvSequence.SelectedRows[i].Cells.Count; j++)
                    {
                        dgvSequence.SelectedRows[i].Cells[j].Value = dgvSequence[j, index].Value.ToString();
                    }
                }
            }
        }

        void cbDev_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dgvSequence.CurrentRow.Index < 0) return;
            if (dgvSequence.RowCount <= 0) return;
            try
            {
                isReadding = true;
                cbEditType.Visible = false;
                cbChannel.Visible = false;
                cbState.Visible = false;
                StepDelay.Visible = false;
                cbKeyNo.Visible = false;
                txtReSendTimes.Visible = false;
                ReSendDelay.Visible = false;
                cbACMode.Visible = false;
                cbACFAN.Visible = false;
                cbWind.Visible = false;
                txtTemp.Visible = false;
                cbManualWind.Visible = false;
                int index = dgvSequence.CurrentRow.Index;
                string str = cbDev.Text;
                string strChannel = dgvSequence[2, index].Value.ToString();
                string strStatus = dgvSequence[3, index].Value.ToString();
                string strStepDelay = dgvSequence[4, index].Value.ToString();
                string str1 = dgvSequence[5, index].Value.ToString();
                string str2 = dgvSequence[6, index].Value.ToString();
                string str3 = dgvSequence[7, index].Value.ToString();
                string str4 = dgvSequence[8, index].Value.ToString();
                string str5 = dgvSequence[9, index].Value.ToString();
                string str6 = dgvSequence[10, index].Value.ToString();
                if (str1.Contains('(')) str1 = str1.Split('(')[0].ToString();
                if (str2.Contains('(')) str2 = str2.Split('(')[0].ToString();
                if (str3.Contains('(')) str3 = str3.Split('(')[0].ToString();
                if (str4.Contains('(')) str4 = str4.Split('(')[0].ToString();
                if (str5.Contains('(')) str5 = str5.Split('(')[0].ToString();
                if (str6.Contains('(')) str6 = str6.Split('(')[0].ToString();
                if (cbDev.SelectedIndex == 0)
                {
                    for (int i = 2; i <= 10; i++)
                    {
                        for (int j = 0; j < dgvSequence.SelectedRows.Count; j++)
                        {
                            dgvSequence.SelectedRows[j].Cells[i].Value = "N/A";
                        }
                    }
                    dgvSequence[1, index].Value = str;
                    ModifyMultilinesIfNeeds(dgvSequence[1, index].Value.ToString(), 1, dgvSequence);
                }
                else if (cbDev.SelectedIndex < cbDev.Items.Count - 1)
                {
                    int devIndex = 0;
                    if (mintDeviceType == 6100)
                    {
                        if (cbDev.SelectedIndex <= 3) devIndex = 1;
                        else devIndex = 2;
                    }
                    else
                    {
                        if (cbDev.SelectedIndex <= 4) devIndex = 1;
                        else devIndex = 2;
                    }
                    if (devIndex != 0)
                    {
                        addcontrols(2, index, cbChannel, dgvSequence);
                        addcontrols(4, index, StepDelay, dgvSequence);
                        dgvSequence[1, index].Value = str;
                        ModifyMultilinesIfNeeds(dgvSequence[1, index].Value.ToString(), 1, dgvSequence);
                    }
                    if (devIndex == 2)
                    {
                        for (int i = 0; i < dgvSequence.SelectedRows.Count; i++)
                        {
                            dgvSequence.SelectedRows[i].Cells[3].Value = cbEditType.Items[1].ToString();
                            dgvSequence.SelectedRows[i].Cells[8].Value = "N/A";
                            dgvSequence.SelectedRows[i].Cells[9].Value = "N/A";
                        }
                        int devid = myir.IRCodes[Convert.ToInt32(cbDev.Text.Split('-')[0]) + 3].DevID;
                        if (mintDeviceType == 6100) devid = myir.IRCodes[Convert.ToInt32(cbDev.Text.Split('-')[0]) + 2].DevID;
                        cbKeyNo.Items.Clear();
                        cbKeyNo.Items.AddRange(CsConst.mstrINIDefault.IniReadValuesInALlSectionStr("NewIRDevice" + devid.ToString()));
                        addcontrols(5, index, cbKeyNo, dgvSequence);
                        addcontrols(6, index, txtReSendTimes, dgvSequence);
                        addcontrols(7, index, ReSendDelay, dgvSequence);
                        addcontrols(8, index, cbState, dgvSequence);
                        if (cbState.Visible && cbState.Items.Count > 0)
                        {
                            if (!cbState.Items.Contains(str4))
                                cbState.SelectedIndex = 0;
                            else
                                cbState.Text = str4;
                        }
                    }
                    else if (devIndex == 1)
                    {
                        cbEditType.SelectedIndex = 0;
                        addcontrols(3, index, cbEditType, dgvSequence);
                    }
                }
                else if (cbDev.SelectedIndex == cbDev.Items.Count - 1)
                {
                    for (int i = 0; i < dgvSequence.SelectedRows.Count; i++)
                    {
                        dgvSequence.SelectedRows[i].Cells[3].Value = cbEditType.Items[1].ToString();
                        dgvSequence.SelectedRows[i].Cells[8].Value = "N/A";
                        dgvSequence.SelectedRows[i].Cells[9].Value = "N/A";
                    }

                    addcontrols(2, index, cbChannel, dgvSequence);
                    addcontrols(4, index, StepDelay, dgvSequence);
                    dgvSequence[1, index].Value = str;
                    ModifyMultilinesIfNeeds(dgvSequence[1, index].Value.ToString(), 1, dgvSequence);

                    cbKeyNo.Items.Clear();
                    for (int i = 1; i <= 100; i++)
                        cbKeyNo.Items.Add(i.ToString());
                    
                    addcontrols(5, index, cbKeyNo, dgvSequence);
                    addcontrols(6, index, txtReSendTimes, dgvSequence);
                    addcontrols(7, index, ReSendDelay, dgvSequence);
                    addcontrols(8, index, cbState, dgvSequence);
                    if (cbState.Visible && cbState.Items.Count > 0)
                    {
                        if (!cbState.Items.Contains(str4))
                            cbState.SelectedIndex = 0;
                        else
                            cbState.Text = str4;
                    }
                }

                if (cbKeyNo.Visible && cbKeyNo.Items.Count > 0)
                {
                    if (!cbKeyNo.Items.Contains(str1))
                        cbKeyNo.SelectedIndex = 0;
                    else
                        cbKeyNo.Text = str1;
                }

                /*if (cbEditType.Visible && cbEditType.Items.Count > 0)
                {
                    if (!cbEditType.Items.Contains(strStatus))
                        cbEditType.SelectedIndex = 0;
                    else
                        cbEditType.Text = strStatus;
                }*/
                if (cbChannel.Visible && cbChannel.Items.Count > 0)
                {
                    if (!cbChannel.Items.Contains(strChannel))
                        cbChannel.SelectedIndex = 0;
                    else
                        cbChannel.Text = strChannel;
                }
                if (StepDelay.Visible) StepDelay.Text = strStepDelay;
                if (txtReSendTimes.Visible) txtReSendTimes.Text = str2;
                if (ReSendDelay.Visible)
                {
                    if (str3.Contains("."))
                        ReSendDelay.Text = HDLPF.GetTimeFromString(str3, '.');
                    else
                        ReSendDelay.Text = "1";
                }
                
                if (cbChannel.Visible) cbChannel_SelectedIndexChanged(null, null);
                if (StepDelay.Visible) StepDelay_TextChanged(null, null);
                if (cbEditType.Visible) cbEditType_SelectedIndexChanged(null, null);
                if (cbKeyNo.Visible) cbKeyNo_SelectedIndexChanged(null, null);
                if (txtReSendTimes.Visible) txtReSendTimes_TextChanged(null, null);
                if (ReSendDelay.Visible) ReSendDelay_TextChanged(null, null);
                if (cbState.Visible) cbState_SelectedIndexChanged(null, null);
            }
            catch
            {
            }
            isReadding = false;
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

        private void setConVisible(bool TF)
        {
            cbEditType.Visible = TF;
            cbManualWind.Visible = TF;
            cbDev.Visible = TF;
            cbWind.Visible = TF;
            cbKeyNo.Visible = TF;
            ReSendDelay.Visible = TF;
            txtReSendTimes.Visible = TF;
            cbChannel.Visible = TF;
            cbState.Visible = TF;
            StepDelay.Visible = TF;
            cbACMode.Visible = TF;
            cbACFAN.Visible = TF;
            cbWind.Visible = TF;
            txtTemp.Visible = TF;

            cbUVEditType.Visible = TF;
            cbUVManualWind.Visible = TF;
            cbUVDev.Visible = TF;
            cbUVWind.Visible = TF;
            cbUVKeyNo.Visible = TF;
            UVReSendDelay.Visible = TF;
            txtUVReSendTimes.Visible = TF;
            cbUVChannel.Visible = TF;
            cbUVState.Visible = TF;
            cbUVACMode.Visible = TF;
            cbUVACFAN.Visible = TF;
            cbUVWind.Visible = TF;
            txtUVTemp.Visible = TF;
        }

        void InitialFormCtrlsTextOrItems()
        {
            frm = new frmIRLibrary(this, myir, mintDeviceType);
            frm.TopLevel = false;
            frm.SendToBack();
            panel3.Controls.Add(frm);
            frm.Dock = DockStyle.Fill;
            frm.Show();
            if (cbSeq.SelectedIndex < 0) cbSeq.SelectedIndex = 0;
            int wdMaxValue = int.Parse(CsConst.mstrINIDefault.IniReadValue("DeviceType" + mintDeviceType.ToString(), "MaxValue", "0"));
            cbChn.Items.Clear();
            for (int i = 1; i <= wdMaxValue; i++)
                cbChn.Items.Add(i.ToString());
            cbChn.SelectedIndex = 0;
            cbSeq.Items.Clear();
            for (int i = 1; i <= 8; i++)
            {
                cbSeq.Items.Add(i.ToString() + "(" + CsConst.mstrINIDefault.IniReadValue("Public", "99582", "") + "-" + (200 + i).ToString() + ")");
            }

            cbType.Items.Clear();
            cbType.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00410", ""));
            cbType.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00411", ""));
            #region
            cbEditType = new ComboBox();
            cbEditType.DropDownStyle = ComboBoxStyle.DropDownList;
            cbEditType.SelectedIndexChanged += cbEditType_SelectedIndexChanged;
            cbDev = new ComboBox();
            cbDev.DropDownStyle = ComboBoxStyle.DropDownList;
            cbDev.SelectedIndexChanged += cbDev_SelectedIndexChanged;
            cbState = new ComboBox();
            cbState.DropDownStyle = ComboBoxStyle.DropDownList;
            cbEditType.Items.Clear();
            cbEditType.Items.Add(CsConst.mstrINIDefault.IniReadValue("public", "99622", ""));
            cbEditType.Items.Add(CsConst.mstrINIDefault.IniReadValue("public", "99623", ""));
            cbState.Items.Clear();
            cbState.Items.Add(CsConst.Status[0]);
            cbState.Items.Add(CsConst.Status[1]);
            cbState.SelectedIndexChanged += cbState_SelectedIndexChanged;
            cbChannel = new ComboBox();
            cbChannel.DropDownStyle = ComboBoxStyle.DropDownList;
            cbChannel.SelectedIndexChanged += cbChannel_SelectedIndexChanged;
            cbChannel.Items.Clear();
            for (int i = 1; i <= 4; i++)
            {
                cbChannel.Items.Add(i.ToString());
            }
            StepDelay = new TextBox();
            StepDelay.TextChanged += StepDelay_TextChanged;
            StepDelay.KeyPress += txtFrm_KeyPress;

            cbKeyNo = new ComboBox();
            cbKeyNo.DropDownStyle = ComboBoxStyle.DropDownList;
            cbKeyNo.SelectedIndexChanged += cbKeyNo_SelectedIndexChanged;
            ReSendDelay = new TimeText();
            ReSendDelay.TextChanged += ReSendDelay_TextChanged;
            txtReSendTimes = new TextBox();
            txtReSendTimes.TextChanged += txtReSendTimes_TextChanged;
            txtReSendTimes.KeyPress += txtFrm_KeyPress;


            cbACMode = new ComboBox();
            cbACMode.DropDownStyle = ComboBoxStyle.DropDownList;
            cbACMode.Items.Clear();
            for (int i = 0; i < 5; i++)
                cbACMode.Items.Add(CsConst.mstrINIDefault.IniReadValue("public", "0005" + i.ToString(), ""));
            cbACMode.SelectedIndexChanged += cbACMode_SelectedIndexChanged;

            cbACFAN = new ComboBox();
            cbACFAN.DropDownStyle = ComboBoxStyle.DropDownList;
            cbACFAN.Items.Clear();
            for (int i = 0; i < 4; i++)
                cbACFAN.Items.Add(CsConst.mstrINIDefault.IniReadValue("public", "0006" + i.ToString(), ""));
            cbACFAN.SelectedIndexChanged += cbACFAN_SelectedIndexChanged;
            cbWind = new ComboBox();
            cbWind.DropDownStyle = ComboBoxStyle.DropDownList;
            cbWind.Items.Clear();
            cbWind.Items.Add(CsConst.mstrINIDefault.IniReadValue("public", "99902", ""));
            cbWind.Items.Add(CsConst.mstrINIDefault.IniReadValue("public", "99896", ""));
            cbWind.SelectedIndexChanged += cbWind_SelectedIndexChanged;
            cbManualWind = new ComboBox();
            cbManualWind.DropDownStyle = ComboBoxStyle.DropDownList;
            cbManualWind.Items.Clear();
            for (int i = 1; i <= 3; i++)
                cbManualWind.Items.Add(CsConst.mstrINIDefault.IniReadValue("public", "99897", "") + " " + i.ToString());
            cbManualWind.SelectedIndexChanged += cbManualWind_SelectedIndexChanged;
            txtTemp = new TextBox();
            txtTemp.KeyPress += txtFrm_KeyPress;
            txtTemp.TextChanged += txtTemp_TextChanged;


            dgvSequence.Controls.Add(cbDev);
            dgvSequence.Controls.Add(cbChannel);
            dgvSequence.Controls.Add(cbState);
            dgvSequence.Controls.Add(StepDelay);
            dgvSequence.Controls.Add(cbKeyNo);
            dgvSequence.Controls.Add(ReSendDelay);
            dgvSequence.Controls.Add(txtReSendTimes);
            dgvSequence.Controls.Add(cbACMode);
            dgvSequence.Controls.Add(cbACFAN);
            dgvSequence.Controls.Add(cbWind);
            dgvSequence.Controls.Add(cbManualWind);
            dgvSequence.Controls.Add(txtTemp);
            dgvSequence.Controls.Add(cbEditType);
            #endregion

            #region
            cbUVEditType = new ComboBox();
            cbUVEditType.DropDownStyle = ComboBoxStyle.DropDownList;
            cbUVEditType.SelectedIndexChanged += cbUVEditType_SelectedIndexChanged;
            cbUVDev = new ComboBox();
            cbUVDev.DropDownStyle = ComboBoxStyle.DropDownList;
            cbUVDev.SelectedIndexChanged += cbUVDev_SelectedIndexChanged;
            cbUVState = new ComboBox();
            cbUVState.DropDownStyle = ComboBoxStyle.DropDownList;
            cbUVEditType.Items.Clear();
            cbUVEditType.Items.Add(CsConst.mstrINIDefault.IniReadValue("public", "99622", ""));
            cbUVEditType.Items.Add(CsConst.mstrINIDefault.IniReadValue("public", "99623", ""));
            cbUVState.Items.Clear();
            cbUVState.Items.Add(CsConst.Status[0]);
            cbUVState.Items.Add(CsConst.Status[1]);
            cbUVState.SelectedIndexChanged += cbUVState_SelectedIndexChanged;
            cbUVChannel = new ComboBox();
            cbUVChannel.DropDownStyle = ComboBoxStyle.DropDownList;
            cbUVChannel.SelectedIndexChanged += cbUVChannel_SelectedIndexChanged;
            cbUVChannel.Items.Clear();
            for (int i = 1; i <= 4; i++)
            {
                cbUVChannel.Items.Add(i.ToString());
            }

            cbUVKeyNo = new ComboBox();
            cbUVKeyNo.DropDownStyle = ComboBoxStyle.DropDownList;
            cbUVKeyNo.SelectedIndexChanged += cbUVKeyNo_SelectedIndexChanged;
            UVReSendDelay = new TimeText();
            UVReSendDelay.TextChanged += UVReSendDelay_TextChanged;
            txtUVReSendTimes = new TextBox();
            txtUVReSendTimes.TextChanged += txtUVReSendTimes_TextChanged;
            txtUVReSendTimes.KeyPress += txtFrm_KeyPress;


            cbUVACMode = new ComboBox();
            cbUVACMode.DropDownStyle = ComboBoxStyle.DropDownList;
            cbUVACMode.Items.Clear();
            for (int i = 0; i < 5; i++)
                cbUVACMode.Items.Add(CsConst.mstrINIDefault.IniReadValue("public", "0005" + i.ToString(), ""));
            cbUVACMode.SelectedIndexChanged += cbUVACMode_SelectedIndexChanged;

            cbUVACFAN = new ComboBox();
            cbUVACFAN.DropDownStyle = ComboBoxStyle.DropDownList;
            cbUVACFAN.Items.Clear();
            for (int i = 0; i < 4; i++)
                cbUVACFAN.Items.Add(CsConst.mstrINIDefault.IniReadValue("public", "0006" + i.ToString(), ""));
            cbUVACFAN.SelectedIndexChanged += cbUVACFAN_SelectedIndexChanged;
            cbUVWind = new ComboBox();
            cbUVWind.DropDownStyle = ComboBoxStyle.DropDownList;
            cbUVWind.Items.Clear();
            cbUVWind.Items.Add(CsConst.mstrINIDefault.IniReadValue("public", "99902", ""));
            cbUVWind.Items.Add(CsConst.mstrINIDefault.IniReadValue("public", "99896", ""));
            cbUVWind.SelectedIndexChanged += cbUVWind_SelectedIndexChanged;
            cbUVManualWind = new ComboBox();
            cbUVManualWind.DropDownStyle = ComboBoxStyle.DropDownList;
            cbUVManualWind.Items.Clear();
            for (int i = 1; i <= 3; i++)
                cbUVManualWind.Items.Add(CsConst.mstrINIDefault.IniReadValue("public", "99897", "") + " " + i.ToString());
            cbUVManualWind.SelectedIndexChanged += cbUVManualWind_SelectedIndexChanged;
            txtUVTemp = new TextBox();
            txtUVTemp.KeyPress += txtFrm_KeyPress;
            txtUVTemp.TextChanged += txtUVTemp_TextChanged;

            dgvUV.Controls.Add(cbUVDev);
            dgvUV.Controls.Add(cbUVChannel);
            dgvUV.Controls.Add(cbUVState);
            dgvUV.Controls.Add(cbUVKeyNo);
            dgvUV.Controls.Add(UVReSendDelay);
            dgvUV.Controls.Add(txtUVReSendTimes);
            dgvUV.Controls.Add(cbUVACMode);
            dgvUV.Controls.Add(cbUVACFAN);
            dgvUV.Controls.Add(cbUVWind);
            dgvUV.Controls.Add(cbUVManualWind);
            dgvUV.Controls.Add(txtUVTemp);
            dgvUV.Controls.Add(cbUVEditType);
            #endregion
            HDLSysPF.addIR(tvIR, tempSend, true);  // 添加已有的列表到窗体
        }

        void SetCtrlsVisbleWithDifferentDeviceType()
        {
            setConVisible(false);
            panel9.Visible = (mintDeviceType == 6100);
            groupBox7.Visible = false;
            groupBox8.Visible = false;
            groupBox4.Visible = false;
            btnInitital.Visible = (mintDeviceType != 6100);
            if (mintDeviceType == 729)
            {
                groupBox7.Visible = true;
                button2_Click_1(null, null);
            }
            else if (mintDeviceType == 1300)
            {
                groupBox7.Visible = true;
                button2_Click_1(null, null);
            }
            else if (mintDeviceType == 6100)
            {
                if (lbUV.Text.Contains("-"))
                    lbUV.Text = lbUV.Text.Split('-')[0].ToString() + "-100)";
                if (lbNum.Text.Contains("-"))
                    lbNum.Text = lbNum.Text.Split('-')[0].ToString() + "-16)";
            }
            else if (mintDeviceType == 1301)
            {
                groupBox8.Visible = true;
                btnRemark_Click(null, null);
            }

            if (mintDeviceType == 729)
            {
                tabcontrol.TabPages.Remove(tabSeq);
                tabcontrol.TabPages.Remove(tabUV);
                tabcontrol.TabPages.Remove(tabDetect);
                tabcontrol.TabPages.Remove(tabCodes);
            }
            if (mintDeviceType == 6100)
            {
                groupBox7.Visible = true;
                groupBox4.Visible = true;
                tabcontrol.TabPages.Remove(tabDetect);
            }
            if (mintDeviceType != 6100)
            {
                if (mintDeviceType != 1301)
                {
                    tabcontrol.TabPages.Remove(tabDetect);
                    tabcontrol.TabPages.Remove(tabCodes);
                }
                tabcontrol.TabPages.Remove(tabTime);

            }
        }

        private void FrmNewIREmitor_Load(object sender, EventArgs e)
        {
            isReadding = true;
            InitialFormCtrlsTextOrItems();
            isReadding = false;
        }

        private void FrmNewIREmitor_Shown(object sender, EventArgs e)
        {
            SetCtrlsVisbleWithDifferentDeviceType();
            if (CsConst.MyEditMode == 1) //在线模式
            {
                toolStrip1.Visible = false;
                MyActivePage = 1;
                tslRead_Click(tslRead, null);
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
    
                    CsConst.MyUPload2DownLists = new List<byte[]>();

                    string strName = mystrName.Split('\\')[0].ToString();
                    byte bytSubID = byte.Parse(strName.Split('-')[0]);
                    byte bytDevID = byte.Parse(strName.Split('-')[1]);
                    if (tabcontrol.SelectedTab.Name == "tabKeys") MyActivePage = 1;
                    else if (tabcontrol.SelectedTab.Name == "tabSeq") MyActivePage = 2;
                    else if (tabcontrol.SelectedTab.Name == "tabUV") MyActivePage = 3;
                    else if (tabcontrol.SelectedTab.Name == "tabDetect") MyActivePage = 4;
                    else if (tabcontrol.SelectedTab.Name == "tabcontrol") MyActivePage = 5;
                    else if (tabcontrol.SelectedTab.Name == "tabTime") MyActivePage = 6;
                    byte[] ArayRelay = new byte[] { SubNetID, DevID, (byte)(mintDeviceType / 256), (byte)(mintDeviceType % 256)
                        , (byte)MyActivePage,(byte)(mintIDIndex/256),(byte)(mintIDIndex%256)  };
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
            switch (tabcontrol.SelectedTab.Name)
            {
                case "tabKeys": showBasicInfo(); break;
                case "tabTime": showTimeInfo(); break;
            }
            Cursor.Current = Cursors.Default;
            this.TopMost = true;
            this.WindowState = FormWindowState.Normal;
            this.Activate();
            this.BringToFront();
            this.TopMost = false;
        }

        private void showTimeInfo()
        {
            try
            {
                if (myir == null) return;
                isReadding = true;
                
                dgvClock.Rows.Clear();
                object[] obj = new object[16];
                for (int i = 0; i < 16; i++)
                {
                    if (myir.Clocks[i].Enable == 1) obj[i] = imageList2.Images[1];
                    else obj[i] = imageList2.Images[0];
                }
                dgvClock.Rows.Add(obj);
                DatePicker_ValueChanged(null, null);
                if (cbScene.SelectedIndex < 0) cbScene.SelectedIndex = 0;
            }
            catch
            {
            }
            isReadding = false;
            cbScene_SelectedIndexChanged(null, null);
            dgvClock_CellClick(dgvClock, new DataGridViewCellEventArgs(0, 0));
        }

        private void showBasicInfo()
        {
            try
            {
                isReadding = true;
                if (myir.IRCodes != null && myir.IRCodes.Count > 0)
                {
                    dgvIR.Rows.Clear();
                    for (int i = 0; i < myir.IRCodes.Count; i++)
                    {
                        NewIR.NewIRCode ir = myir.IRCodes[i];
                        string str = "N/A";
                        if (ir.DevID < 6)
                        {
                            str = CsConst.NewIRLibraryDeviceType[ir.DevID];
                        }
                        if (ir.IRLength == 0) str = "N/A";
                        object[] obj = new object[] { ir.KeyID, str, ir.Remark.ToString(), "Send" };
                        dgvIR.Rows.Add(obj);
                    }
                }
                if (myir.arayBrocast != null && mintDeviceType == 6100)
                {
                    chbTemp.Checked = (myir.arayBrocast[0] == 1);
                    txtTemp1.Text = myir.arayBrocast[1].ToString();
                    txtTemp2.Text = myir.arayBrocast[2].ToString();
                    sbAdjust.Value = myir.arayBrocast[3];
                    lbCurrentValue.Text = myir.arayBrocast[4].ToString() + "C";


                    int wdYear = Convert.ToInt32(myir.arayTime[0]) + 2000;
                    byte bytMonth = myir.arayTime[1];
                    byte bytDay = myir.arayTime[2];
                    byte bytHour = myir.arayTime[3];
                    byte bytMinute = myir.arayTime[4];
                    byte bytSecond = myir.arayTime[5];

                    if (bytHour > 23) bytHour = 0;
                    if (bytMinute > 59) bytMinute = 0;
                    if (bytSecond > 59) bytSecond = 0;
                    if (bytMonth > 12) bytMonth = 1;
                    if (bytDay > 31) bytDay = 1;

                    DateTime d = new DateTime(wdYear, Convert.ToInt32(bytMonth), Convert.ToInt32(bytDay));
                    DatePicker.Value = d;
                    DatePicker_ValueChanged(null, null);
                    numTime1.Value = Convert.ToDecimal(bytHour);
                    numTime2.Value = Convert.ToDecimal(bytMinute);
                    numTime3.Value = Convert.ToDecimal(bytSecond);
                }
            }
            catch
            {
            }
            isReadding = false;
            sbAdjust_ValueChanged(null, null);
        }


        private void tmRead_Click(object sender, EventArgs e)
        {
            if (tabcontrol.SelectedTab.Name == "tabKeys")
                tslRead_Click(tslRead, null);
            else if (tabcontrol.SelectedTab.Name == "tabSeq")
                cbSeq_SelectedIndexChanged(cbSeq, null);
            else if (tabcontrol.SelectedTab.Name == "tabUV")
                btnReadUV_Click(btnReadUV, null);
            else if (tabcontrol.SelectedTab.Name == "tabDetect")
                btnReadDetect_Click(null, null);
        }

        private void dgvIR_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (myir == null) return;
                if (myir.IRCodes == null) return;
                if ((e.RowIndex == -1) || (e.ColumnIndex == -1)) return;
                if (dgvIR[e.ColumnIndex, e.RowIndex].Value == null) dgvIR[e.ColumnIndex, e.RowIndex].Value = "";

                for (int i = 0; i < dgvIR.SelectedRows.Count; i++)
                {
                    string strTmp = "";
                    switch (e.ColumnIndex)
                    {
                        case 2:
                            strTmp = dgvIR[2, dgvIR.SelectedRows[i].Index].Value.ToString();
                            dgvIR[2, dgvIR.SelectedRows[i].Index].Value = HDLPF.IsRightStringMode(strTmp);
                            myir.IRCodes[dgvIR.SelectedRows[i].Index].Remark = dgvIR[2, dgvIR.SelectedRows[i].Index].Value.ToString();
                            break;
                    }
                    dgvIR.SelectedRows[i].Cells[e.ColumnIndex].Value = dgvIR[e.ColumnIndex, e.RowIndex].Value.ToString();
                }
                if (e.ColumnIndex == 2)
                {
                    string strTmp = dgvIR[2, e.RowIndex].Value.ToString();
                    dgvIR[2, e.RowIndex].Value = HDLPF.IsRightStringMode(strTmp);
                    myir.IRCodes[e.RowIndex].Remark = dgvIR[2, e.RowIndex].Value.ToString();
                }
            }
            catch
            {
            }
        }

        private void dgvIR_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            if (e.ColumnIndex == 3)
            {
                if (dgvIR[1, e.RowIndex].Value.ToString() == "N/A") return;
                int IRDeviceID = Convert.ToInt32(dgvIR[0, e.RowIndex].Value.ToString());
                string strDev = dgvIR[1, e.RowIndex].Value.ToString();
                int IRDevID=0;

                IRDevID = HDLPF.GetIndexFromBufferLists(CsConst.NewIRLibraryDeviceType, strDev);

                FrmChnAndKey frmTmp = new FrmChnAndKey(IRDevID, mintDeviceType, IRDeviceID, SubNetID, DevID);
                frmTmp.ShowDialog(); 
                /*DialogResult = DialogResult.Cancel;
                if (frmTmp.ShowDialog() == DialogResult.OK)
                {
                    byte[] arayTmp = new byte[4];
                    if (mintDeviceType == 729)
                    {
                        arayTmp = new byte[4];
                        int int1 = 0;
                        int int2 = CsConst.ChannelIDForNewIR + 1;
                        string str1 = GlobalClass.IntToBit(int1, 2);
                        string str2 = GlobalClass.IntToBit(int2, 6);
                        string str = str1 + str2;
                        arayTmp[0] = 1;
                        arayTmp[1] = Convert.ToByte(GlobalClass.BitToInt(str));
                        arayTmp[2] = Convert.ToByte(dgvIR.CurrentRow.Index + 1);
                        arayTmp[3] = Convert.ToByte(CsConst.KeyIDForNewIR + 1);
                        if (CsConst.mySends.AddBufToSndList(arayTmp, 0xdb90, SubNetID, DevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(mintDeviceType)) == true)
                        {
                            
                        }
                    }
                    else if (mintDeviceType == 1301 || mintDeviceType == 1300 || mintDeviceType==6100)
                    {
                        if (IRDeviceID <= 4 && IRDevID == 5)
                        {
                            arayTmp = new byte[13];
                            arayTmp[0] = Convert.ToByte(IRDeviceID);
                            Array.Copy(CsConst.arayACParamForNewIR, 1, arayTmp, 8, 5);
                            if (CsConst.mySends.AddBufToSndList(arayTmp, 0x193A, SubNetID, DevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(mintDeviceType)) == true)
                            {
                                
                            }
                        }
                        else
                        {
                            arayTmp = new byte[4];
                            int int1 = 0;
                            int int2 = CsConst.ChannelIDForNewIR + 1;
                            string str1 = GlobalClass.IntToBit(int1, 2);
                            string str2 = GlobalClass.IntToBit(int2, 6);
                            string str = str1 + str2;
                            arayTmp[0] = Convert.ToByte(GlobalClass.BitToInt(str));
                            arayTmp[1] = Convert.ToByte(dgvIR.CurrentRow.Index + 1);
                            if (arayTmp[1] > 4)
                                arayTmp[1] = Convert.ToByte(arayTmp[1] - 4);
                            arayTmp[2] = Convert.ToByte(CsConst.KeyIDForNewIR + 1);
                            if (CsConst.mySends.AddBufToSndList(arayTmp, 0xdb90, SubNetID, DevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(mintDeviceType)) == true)
                            {
                                
                            }
                        }
                    }
                    else
                    {
                        arayTmp = new byte[4];
                        int int1 = 0;
                        int int2 = CsConst.ChannelIDForNewIR + 1;
                        string str1 = GlobalClass.IntToBit(int1, 2);
                        string str2 = GlobalClass.IntToBit(int2, 6);
                        string str = str1 + str2;
                        arayTmp[0] = Convert.ToByte(GlobalClass.BitToInt(str));
                        arayTmp[1] = Convert.ToByte(dgvIR.CurrentRow.Index + 1);
                        arayTmp[2] = Convert.ToByte(CsConst.KeyIDForNewIR + 1);
                        if (CsConst.mySends.AddBufToSndList(arayTmp, 0xdb90, SubNetID, DevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(mintDeviceType)) == true)
                        {
                            
                        }
                    }
                    
                   
                }*/
            }
            Cursor.Current = Cursors.Default;
        }

        private void btnSure_Click(object sender, EventArgs e)
        {
            if (CsConst.MyEditMode == 1)
            {
                try
                {
                    if (myir == null) return;
                    if (myir.IRCodes == null) return;
                    //if (myir.IRCodes.Count != 28) return;
                    setConVisible(false);
                    dgvSequence.Rows.Clear();
                    Cursor.Current = Cursors.WaitCursor;
                    byte[] arayTmp = new byte[2];
                    byte bytFrm = Convert.ToByte(txtFrm.Text);
                    byte bytTo = Convert.ToByte(txtTo.Text);
                    arayTmp[0] = Convert.ToByte(cbSeq.SelectedIndex + 1);
                    for (byte i = bytFrm; i <= bytTo; i++)
                    {
                        arayTmp[1] = i;
                        if (CsConst.mySends.AddBufToSndList(arayTmp, 0xDB80, SubNetID, DevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(mintDeviceType)) == true)
                        {
                            string strDev = CsConst.WholeTextsList[1775].sDisplayName;
                            string strState = "N/A";
                            string strChannel = "N/A";
                            string strTime = "N/A";
                            string str1 = "N/A";
                            string str2 = "N/A";
                            string str3 = "N/A";
                            string str4 = "N/A";
                            string str5 = "N/A";
                            string strEditType = "N/A";
                            int DeviceType = 0;
                            if (1 <= CsConst.myRevBuf[29] && CsConst.myRevBuf[29] <= (myir.IRCodes.Count+1) && 
                                (CsConst.myRevBuf[28] == 1 || CsConst.myRevBuf[28] == 2))
                                DeviceType = Convert.ToInt32(CsConst.myRevBuf[28]);
                            if (DeviceType != 0)
                                strChannel = CsConst.myRevBuf[27].ToString();
                            if (1 <= CsConst.myRevBuf[29] && CsConst.myRevBuf[29] <= (myir.IRCodes.Count) && DeviceType != 0)
                            {
                                strDev = CsConst.myRevBuf[29].ToString();
                                if (DeviceType == 1)
                                {
                                    strDev = strDev + "-" + myir.IRCodes[CsConst.myRevBuf[29] - 1].Remark;
                                }
                                else if (DeviceType == 2)
                                {
                                    if (mintDeviceType == 6100)
                                        strDev = strDev + "-" + myir.IRCodes[CsConst.myRevBuf[29] + 2].Remark;
                                    else
                                        strDev = strDev + "-" + myir.IRCodes[CsConst.myRevBuf[29] + 3].Remark;
                                }

                            }
                            else if (CsConst.myRevBuf[29] == (myir.IRCodes.Count + 1) && DeviceType != 0)
                            {
                                strDev = cbDev.Items[cbDev.Items.Count - 1].ToString();
                            }

                            if (DeviceType == 1)
                            {
                                strEditType = CsConst.mstrINIDefault.IniReadValue("public", "99622", "");
                                if (CsConst.myRevBuf[30] <= 1)
                                    strState = cbState.Items[CsConst.myRevBuf[30]].ToString()
                                             + "(" + CsConst.mstrINIDefault.IniReadValue("public", "99620", "") + ")";
                                strTime = CsConst.myRevBuf[36].ToString();
                                if (0 <= CsConst.myRevBuf[31] && CsConst.myRevBuf[31] <= 4)
                                    str1 = CsConst.mstrINIDefault.IniReadValue("public", "0005" + (CsConst.myRevBuf[31]).ToString(), "")
                                         + "(" + CsConst.mstrINIDefault.IniReadValue("public", "99876", "") + ")";
                                if (0 <= CsConst.myRevBuf[32] && CsConst.myRevBuf[32] <= 3)
                                    str2 = CsConst.mstrINIDefault.IniReadValue("public", "0006" + (CsConst.myRevBuf[32]).ToString(), "")
                                         + "(" + CsConst.mstrINIDefault.IniReadValue("public", "99875", "") + ")";
                                if (CsConst.myRevBuf[33] == 1)
                                    str3 = CsConst.mstrINIDefault.IniReadValue("public", "99896", "");  
                                else if (CsConst.myRevBuf[33] == 0)
                                    str3 = CsConst.mstrINIDefault.IniReadValue("public", "99902", "");
                                str4 = CsConst.mstrINIDefault.IniReadValue("public", "99897", "") + " " + (CsConst.myRevBuf[34]+1).ToString();
                                str5 = CsConst.myRevBuf[35].ToString() + "(" + CsConst.mstrINIDefault.IniReadValue("public", "99874", "") + ")";
                                object[] obj = new object[] { i.ToString(), strDev, strChannel, strEditType, strTime, strState,str1, str2, str3, str4, str5 };
                                dgvSequence.Rows.Add(obj);
                            }
                            else if (DeviceType == 2)
                            {
                                strEditType = CsConst.mstrINIDefault.IniReadValue("public", "99623", "");
                                if (CsConst.myRevBuf[31] <= 1)
                                    strState = cbState.Items[CsConst.myRevBuf[31]].ToString()
                                             + "(" + CsConst.mstrINIDefault.IniReadValue("public", "99621", "") + ")";
                                strTime = CsConst.myRevBuf[36].ToString();

                                if (CsConst.myRevBuf[29] <= (myir.IRCodes.Count))
                                {
                                    int devid = myir.IRCodes[CsConst.myRevBuf[29] + 3].DevID;
                                    if (mintDeviceType == 6100) devid = myir.IRCodes[CsConst.myRevBuf[29] + 2].DevID;
                                    string strTmp = CsConst.mstrINIDefault.IniReadValue("NewIRDevice" + devid.ToString(), CsConst.myRevBuf[30].ToString("D5"), "");
                                    if (strTmp != "")
                                        str1 = CsConst.mstrINIDefault.IniReadValue("NewIRDevice" + devid.ToString(), CsConst.myRevBuf[30].ToString("D5"), CsConst.myRevBuf[30].ToString())
                                             + "(" + CsConst.mstrINIDefault.IniReadValue("public", "99898", "") + ")";
                                }
                                else if (CsConst.myRevBuf[29] == (myir.IRCodes.Count + 1))
                                {
                                    str1 = CsConst.myRevBuf[30].ToString() + "(" + CsConst.mstrINIDefault.IniReadValue("public", "99898", "") + ")";
                                }
                                str2 = (CsConst.myRevBuf[32] * 256 + CsConst.myRevBuf[33]).ToString() + "(" + CsConst.mstrINIDefault.IniReadValue("public", "99899", "") + ")";
                                str3 = HDLPF.GetStringFromTime(Convert.ToInt32(CsConst.myRevBuf[34]), ".") +
                                       "(" + CsConst.mstrINIDefault.IniReadValue("public", "99900", "") + ")";
                                object[] obj = new object[] { i.ToString(), strDev, strChannel, strEditType, strTime, str1, str2, str3, strState, str4, str5 };
                                dgvSequence.Rows.Add(obj);
                            }
                            else
                            {
                                
                                object[] obj = new object[] { i.ToString(), strDev, strChannel, strEditType, strTime, str1, str2, str3, strState, str4, str5 };
                                dgvSequence.Rows.Add(obj);
                            }
                            
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                catch
                {
                }
                Cursor.Current = Cursors.Default;
            }
        }

        private void txtFrm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar) || e.KeyChar == 8)
                e.Handled = false;
            else
                e.Handled = true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (CsConst.MyEditMode == 1)
            {
                setConVisible(false);
                chbEnable_CheckedChanged(null, null);
                Cursor.Current = Cursors.WaitCursor;
                for (int i = 0; i < dgvSequence.Rows.Count; i++)
                {
                    byte[] arayTmp = new byte[12];
                    string str1 = dgvSequence[5, i].Value.ToString();
                    string str2 = dgvSequence[6, i].Value.ToString();
                    string str3 = dgvSequence[7, i].Value.ToString();
                    string str4 = dgvSequence[8, i].Value.ToString();
                    string str5 = dgvSequence[9, i].Value.ToString();
                    string str6 = dgvSequence[10, i].Value.ToString();
                    if (str1.Contains("(")) str1 = str1.Split('(')[0].ToString();
                    if (str2.Contains("(")) str2 = str2.Split('(')[0].ToString();
                    if (str3.Contains("(")) str3 = str3.Split('(')[0].ToString();
                    if (str4.Contains("(")) str4 = str4.Split('(')[0].ToString();
                    if (str5.Contains("(")) str5 = str5.Split('(')[0].ToString();
                    if (str6.Contains("(")) str6 = str6.Split('(')[0].ToString();
                    arayTmp[0] = Convert.ToByte(cbSeq.SelectedIndex + 1);
                    arayTmp[1] = Convert.ToByte(dgvSequence[0, i].Value.ToString());
                    for (int j = 2; j < 12; j++)
                        arayTmp[j] = 1;
                    if (dgvSequence[1, i].Value.ToString() == CsConst.WholeTextsList[1775].sDisplayName)
                        arayTmp[3] = 0;
                    else
                    {
                        arayTmp[2] = Convert.ToByte(dgvSequence[2, i].Value.ToString());
                        arayTmp[4] = Convert.ToByte(dgvSequence[1, i].Value.ToString().Split('-')[0].ToString());

                        if (dgvSequence[3, i].Value.ToString() == cbEditType.Items[0].ToString()) arayTmp[3] = 1;
                        else arayTmp[3] = 2;
                        if (arayTmp[3] == 1)
                        {

                            if (dgvSequence[5, i].Value.ToString() != "N/A")
                                arayTmp[5] = Convert.ToByte(cbState.Items.IndexOf(str1));
                            if (dgvSequence[6, i].Value.ToString() != "N/A")
                                arayTmp[6] = Convert.ToByte(cbACMode.Items.IndexOf(str2));
                            if (dgvSequence[7, i].Value.ToString() != "N/A")
                                arayTmp[7] = Convert.ToByte(cbACFAN.Items.IndexOf(str3));
                            if (dgvSequence[8, i].Value.ToString() == CsConst.mstrINIDefault.IniReadValue("public", "99896", ""))
                                arayTmp[8] = 1;
                            else if (str4 == CsConst.mstrINIDefault.IniReadValue("public", "99902", ""))
                                arayTmp[8] = 0;
                            if (cbManualWind.Items.Contains(str5))
                                arayTmp[9] = Convert.ToByte(cbManualWind.Items.IndexOf(str5) + 1);
                            arayTmp[10] = Convert.ToByte(str6);
                            arayTmp[11] = Convert.ToByte(dgvSequence[4, i].Value.ToString());
                        }
                        else
                        {
                            int DeviceID = Convert.ToInt32(dgvSequence[1, i].Value.ToString().Split('-')[0]);
                            if (DeviceID <= (cbDev.Items.Count-2))
                            {
                                DeviceID = Convert.ToInt32(dgvSequence[1, i].Value.ToString().Split('-')[0]) + 3;
                                if (mintDeviceType == 6100) DeviceID = Convert.ToInt32(dgvSequence[1, i].Value.ToString().Split('-')[0]) + 2;
                                int devid = myir.IRCodes[DeviceID].DevID;
                                cbKeyNo.Items.Clear();
                                cbKeyNo.Items.AddRange(CsConst.mstrINIDefault.IniReadValuesInALlSectionStr("NewIRDevice" + devid.ToString()));
                                if (dgvSequence[5, i].Value.ToString() != "N/A")
                                    arayTmp[5] = Convert.ToByte(cbKeyNo.Items.IndexOf(str1) + 1);
                                arayTmp[6] = Convert.ToByte(cbState.Items.IndexOf(str4));
                                int intTmp = Convert.ToInt32(str2);
                                arayTmp[7] = Convert.ToByte(intTmp / 256);
                                arayTmp[8] = Convert.ToByte(intTmp % 256);
                                if (str3.Contains("."))
                                    arayTmp[9] = Convert.ToByte(HDLPF.GetTimeFromString(str3, '.'));
                                arayTmp[11] = Convert.ToByte(dgvSequence[4, i].Value.ToString());
                            }
                            else if (DeviceID == (cbDev.Items.Count - 1))
                            {
                                if (dgvSequence[5, i].Value.ToString() != "N/A")
                                    arayTmp[5] = Convert.ToByte(str1);
                                arayTmp[6] = Convert.ToByte(cbState.Items.IndexOf(str4));
                                int intTmp = Convert.ToInt32(str2);
                                arayTmp[7] = Convert.ToByte(intTmp / 256);
                                arayTmp[8] = Convert.ToByte(intTmp % 256);
                                if (str3.Contains("."))
                                    arayTmp[9] = Convert.ToByte(HDLPF.GetTimeFromString(str3, '.'));
                                arayTmp[11] = Convert.ToByte(dgvSequence[4, i].Value.ToString());
                            }
                        }
                    }
                    if (CsConst.mySends.AddBufToSndList(arayTmp, 0xDB82, SubNetID, DevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(mintDeviceType)) == true)
                    {
                        
                        System.Threading.Thread.Sleep(200);
                    }
                    else
                    {
                        break;
                    }
                }
                Cursor.Current = Cursors.Default;
            }
        }

        private void tabcontrol_SelectedIndexChanged(object sender, EventArgs e)
        {
            setConVisible(false);
            if (CsConst.MyEditMode == 1)
            {
                if (tabcontrol.SelectedIndex == 0)
                {
                    if (myir.MyRead2UpFlags[0] == false)
                    {
                        tslRead_Click(tslRead, null);
                    }
                }
                else if (tabcontrol.SelectedTab.Name == "tabSeq")
                {
                    cbDev.Items.Clear();
                    cbDev.Items.Add(CsConst.WholeTextsList[1775].sDisplayName);
                    checkIRCodesCount();
                    if (mintDeviceType == 6100)
                    {
                        for (int i = 1; i <= 10; i++)
                        {
                            if (i <= 3)
                                cbDev.Items.Add(i.ToString() + "-" + myir.IRCodes[i - 1].Remark);
                            else
                                cbDev.Items.Add((i - 3).ToString() + "-" + myir.IRCodes[i - 1].Remark);
                        }
                        cbDev.Items.Add("11" + "-" + CsConst.mstrINIDefault.IniReadValue("public", "99802", ""));
                    }
                    else
                    {
                        for (int i = 1; i <= 28; i++)
                        {
                            if (i <= 4)
                                cbDev.Items.Add(i.ToString() + "-" + myir.IRCodes[i - 1].Remark);
                            else
                                cbDev.Items.Add((i - 4).ToString() + "-" + myir.IRCodes[i - 1].Remark);
                        }
                        if (mintDeviceType != 1300)
                            cbDev.Items.Add("29" + "-" + CsConst.mstrINIDefault.IniReadValue("public", "99802", ""));
                    }
                }
                else if (tabcontrol.SelectedTab.Name == "tabUV")
                {
                    cbUVDev.Items.Clear();
                    cbUVDev.Items.Add(CsConst.WholeTextsList[1775].sDisplayName);
                    checkIRCodesCount();
                    if (mintDeviceType == 6100)
                    {
                        for (int i = 1; i <= 10; i++)
                        {
                            if (i <= 3)
                                cbUVDev.Items.Add(i.ToString() + "-" + myir.IRCodes[i - 1].Remark);
                            else
                                cbUVDev.Items.Add((i - 3).ToString() + "-" + myir.IRCodes[i - 1].Remark);
                        }
                        cbUVDev.Items.Add("11" + "-" + CsConst.mstrINIDefault.IniReadValue("public", "99802", ""));
                    }
                    else
                    {
                        for (int i = 1; i <= 28; i++)
                        {
                            if (i <= 4)
                                cbUVDev.Items.Add(i.ToString() + "-" + myir.IRCodes[i - 1].Remark);
                            else
                                cbUVDev.Items.Add((i - 4).ToString() + "-" + myir.IRCodes[i - 1].Remark);
                        }
                        if (mintDeviceType != 1300)
                            cbUVDev.Items.Add("29" + "-" + CsConst.mstrINIDefault.IniReadValue("public", "99802", ""));
                    }
                }
                else if (tabcontrol.SelectedTab.Name == "tabDetect")
                {
                    cbDDevice.Items.Clear();
                    checkIRCodesCount();
                    for (int i = 1; i <= 24; i++)
                        cbDDevice.Items.Add(i.ToString() + "-" + myir.IRCodes[i + 3].Remark);
                    cbDDevice.Items.Add("29" + "-" + CsConst.mstrINIDefault.IniReadValue("public", "99802", ""));
                    btnReadDetect_Click(null, null);
                }
                else if (tabcontrol.SelectedTab.Name == "tabTime")
                {
                    if (myir.MyRead2UpFlags[5] == false)
                    {
                        tslRead_Click(tslRead, null);
                    }
                }
            }
        }

        private void checkIRCodesCount()
        {
            recheck:
            if (mintDeviceType == 729 || mintDeviceType == 1300)
            {
                if (myir == null) return;
                if (myir.IRCodes == null) return;
                if (myir.IRCodes.Count != 28)
                {
                    MyActivePage = 1;
                    tslRead_Click(tslRead, null);
                    System.Threading.Thread.Sleep(1000);
                    if (myir.IRCodes.Count != 28)
                        goto recheck;
                }
            }
            else if (mintDeviceType == 6100)
            {
                if (myir == null) return;
                if (myir.IRCodes == null) return;
                if (myir.IRCodes.Count != 10)
                {
                    MyActivePage = 1;
                    tslRead_Click(tslRead, null);
                    System.Threading.Thread.Sleep(1000);
                    if (myir.IRCodes.Count != 10)
                        goto recheck;
                }
            }
        }

        private void cbSeq_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isReadding) return;
            if (CsConst.MyEditMode == 1)
            {
                Cursor.Current = Cursors.WaitCursor;
                byte[] arayTmp = new byte[2];
                arayTmp[0] = Convert.ToByte(cbSeq.SelectedIndex + 1);
                arayTmp[1] = 0;
                if (CsConst.mySends.AddBufToSndList(arayTmp, 0xDB80, SubNetID, DevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(mintDeviceType)) == true)
                {
                    isReadding = true;
                    byte[] arayRemark = new byte[20];
                    Array.Copy(CsConst.myRevBuf, 27, arayRemark, 0, 20);
                    txtRemark.Text = HDLPF.Byte2String(arayRemark);
                    if (CsConst.myRevBuf[47] == 1) chbEnable.Checked = true;
                    else chbEnable.Checked = false;
                    
                    btnSure_Click(null, null);
                }
                Cursor.Current = Cursors.Default;
                isReadding = false;
            }
        }

        private void chbEnable_CheckedChanged(object sender, EventArgs e)
        {
            if (isReadding) return;
            if (CsConst.MyEditMode == 1)
            {
                string str = txtRemark.Text;
                Cursor.Current = Cursors.WaitCursor;
                byte[] arayTmp = new byte[23];
                arayTmp[0] = Convert.ToByte(cbSeq.SelectedIndex + 1);
                arayTmp[1] = 0;
                byte[] arayRemark = new byte[20];
                arayRemark = HDLUDP.StringToByte(str);
                if (arayRemark.Length > 20)
                    Array.Copy(arayRemark, 0, arayTmp, 2, 20);
                else
                    Array.Copy(arayRemark, 0, arayTmp, 2, arayRemark.Length);
                if (chbEnable.Checked) arayTmp[22] = 1;
                if (CsConst.mySends.AddBufToSndList(arayTmp, 0xDB82, SubNetID, DevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(mintDeviceType)) == true)
                {
                    
                }
                Cursor.Current = Cursors.Default;
            }
        }

        private void txtRemark_Leave(object sender, EventArgs e)
        {
            
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

        private void dgvSequence_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            setConVisible(false);
            if (e.RowIndex >= 0)
            {
                cbDev.SelectedIndex = cbDev.Items.IndexOf(dgvSequence[1, e.RowIndex].Value.ToString());
                addcontrols(1, e.RowIndex, cbDev, dgvSequence);
            }
            cbDev_SelectedIndexChanged(null, null);
        }

        private void dgvSequence_Scroll(object sender, ScrollEventArgs e)
        {
            setConVisible(false);
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
            if (mintDeviceType == 6100)
                txtTo.Text = HDLPF.IsNumStringMode(str, num, 16);
            else
                txtTo.Text = HDLPF.IsNumStringMode(str, num, 20);
            txtTo.SelectionStart = txtTo.Text.Length;
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
            if (mintDeviceType == 6100)
                txtUV2.Text = HDLPF.IsNumStringMode(str, num, 100);
            else
                txtUV2.Text = HDLPF.IsNumStringMode(str, num, 99);
            txtUV2.SelectionStart = txtUV2.Text.Length;
        }

        private void btnReadUV_Click(object sender, EventArgs e)
        {
            if (CsConst.MyEditMode == 1)
            {
                if (myir == null) return;
                if (myir.IRCodes == null) return;
                //if (myir.IRCodes.Count != 28) return;
                setConVisible(false);
                dgvUV.Rows.Clear();
                Cursor.Current = Cursors.WaitCursor;
                byte[] arayTmp = new byte[2];
                byte bytFrm = Convert.ToByte(txtUV1.Text);
                byte bytTo = Convert.ToByte(txtUV2.Text);
                for (int i = bytFrm; i <= bytTo; i++)
                {
                    arayTmp[0] = (byte)i;
                    arayTmp[1] = 1;
                    if (CsConst.mySends.AddBufToSndList(arayTmp, 0xDB96, SubNetID, DevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(mintDeviceType)) == true)
                    {
                        string strON = "-" + CsConst.Status[1];
                        string strDev = CsConst.WholeTextsList[1775].sDisplayName;
                        string strState = "N/A";
                        string strChannel = "N/A";
                        string str1 = "N/A";
                        string str2 = "N/A";
                        string str3 = "N/A";
                        string str4 = "N/A";
                        string str5 = "N/A";
                        string strEditType = "N/A";
                        int DeviceType = 0;
                        if (1 <= CsConst.myRevBuf[29] && CsConst.myRevBuf[29] <= (myir.IRCodes.Count+1) &&
                            (CsConst.myRevBuf[28] == 1 || CsConst.myRevBuf[28] == 2))
                            DeviceType = Convert.ToInt32(CsConst.myRevBuf[28]);
                        if (DeviceType != 0)
                            strChannel = CsConst.myRevBuf[27].ToString();
                        if (1 <= CsConst.myRevBuf[29] && CsConst.myRevBuf[29] <= myir.IRCodes.Count && DeviceType != 0)
                        {
                            strDev = CsConst.myRevBuf[29].ToString();
                            if (DeviceType == 1)
                                strDev = strDev + "-" + myir.IRCodes[CsConst.myRevBuf[29] - 1].Remark;
                            else if (DeviceType == 2)
                            {
                                if (mintDeviceType == 6100)
                                    strDev = strDev + "-" + myir.IRCodes[CsConst.myRevBuf[29] + 2].Remark;
                                else
                                    strDev = strDev + "-" + myir.IRCodes[CsConst.myRevBuf[29] + 3].Remark;
                            }
                        }
                        else if (CsConst.myRevBuf[29] <= (myir.IRCodes.Count+1) && DeviceType != 0)
                        {
                            strDev = cbUVDev.Items[cbUVDev.Items.Count - 1].ToString();
                        }
                        if (DeviceType == 1)
                        {
                            strEditType = CsConst.mstrINIDefault.IniReadValue("public", "99622", "");
                            if (CsConst.myRevBuf[30] <= 1)
                                strState = cbState.Items[CsConst.myRevBuf[30]].ToString() + "(" + CsConst.mstrINIDefault.IniReadValue("public", "99620", "") + ")";
                            if (0 <= CsConst.myRevBuf[31] && CsConst.myRevBuf[31] <= 4)
                                str1 = CsConst.mstrINIDefault.IniReadValue("public", "0005" + (CsConst.myRevBuf[31]).ToString(), "")
                                     + "(" + CsConst.mstrINIDefault.IniReadValue("public", "99876", "") + ")";
                            if (0 <= CsConst.myRevBuf[32] && CsConst.myRevBuf[32] <= 3)
                                str2 = CsConst.mstrINIDefault.IniReadValue("public", "0006" + (CsConst.myRevBuf[32]).ToString(), "")
                                     + "(" + CsConst.mstrINIDefault.IniReadValue("public", "99875", "") + ")";
                            if (CsConst.myRevBuf[33] == 1)
                                str3 = CsConst.mstrINIDefault.IniReadValue("public", "99896", "");
                            else if (CsConst.myRevBuf[33] == 0)
                                str3 = CsConst.mstrINIDefault.IniReadValue("public", "99902", "");
                            str4 = CsConst.mstrINIDefault.IniReadValue("public", "99897", "") + " " + CsConst.myRevBuf[34].ToString();
                            str5 = CsConst.myRevBuf[35].ToString() + "(" + CsConst.mstrINIDefault.IniReadValue("public", "99874", "") + ")";
                        }
                        else if (DeviceType == 2)
                        {
                            strEditType = CsConst.mstrINIDefault.IniReadValue("public", "99623", "");
                            if (CsConst.myRevBuf[29] <= (myir.IRCodes.Count))
                            {
                                int devid = myir.IRCodes[CsConst.myRevBuf[29] + 3].DevID;
                                if (mintDeviceType == 6100) devid = myir.IRCodes[CsConst.myRevBuf[29] + 2].DevID;
                                string strTmp = CsConst.mstrINIDefault.IniReadValue("NewIRDevice" + devid.ToString(), CsConst.myRevBuf[30].ToString("D5"), "");
                                if (strTmp != "")
                                    strState = CsConst.mstrINIDefault.IniReadValue("NewIRDevice" + devid.ToString(), CsConst.myRevBuf[30].ToString("D5"), "")
                                         + "(" + CsConst.mstrINIDefault.IniReadValue("public", "99898", "") + ")";
                            }
                            else if (CsConst.myRevBuf[29] == (myir.IRCodes.Count + 1))
                            {
                                strState = CsConst.myRevBuf[30].ToString() + "(" + CsConst.mstrINIDefault.IniReadValue("public", "99898", "") + ")";
                            }
                            str1 = (CsConst.myRevBuf[32] * 256 + CsConst.myRevBuf[33]).ToString() + "(" + CsConst.mstrINIDefault.IniReadValue("public", "99899", "") + ")";
                            str2 = HDLPF.GetStringFromTime(Convert.ToInt32(CsConst.myRevBuf[34]), ".") +
                                   "(" + CsConst.mstrINIDefault.IniReadValue("public", "99900", "") + ")";
                            if (CsConst.myRevBuf[31] <= 1)
                                str3 = cbState.Items[CsConst.myRevBuf[31]].ToString() 
                                   +"(" + CsConst.mstrINIDefault.IniReadValue("public", "99621", "") + ")";
                        }

                        object[] obj = new object[] { i.ToString() + strON, strDev, strChannel,strEditType, strState, str1, str2, str3, str4, str5 };
                        dgvUV.Rows.Add(obj);
                        
                    }
                    else
                    {
                        break;
                    }

                    arayTmp[0] = (byte)i;
                    arayTmp[1] = 0;
                    if (CsConst.mySends.AddBufToSndList(arayTmp, 0xDB96, SubNetID, DevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(mintDeviceType)) == true)
                    {
                        string strOFF = "-" + CsConst.Status[0];
                        string strDev = CsConst.WholeTextsList[1775].sDisplayName;
                        string strState = "N/A";
                        string strChannel = "N/A";
                        string str1 = "N/A";
                        string str2 = "N/A";
                        string str3 = "N/A";
                        string str4 = "N/A";
                        string str5 = "N/A";
                        string strEditType = "N/A";
                        int DeviceType = 0;
                        if (1 <= CsConst.myRevBuf[29] && CsConst.myRevBuf[29] <= (myir.IRCodes.Count + 1) &&
                            (CsConst.myRevBuf[28] == 1 || CsConst.myRevBuf[28] == 2))
                            DeviceType = Convert.ToInt32(CsConst.myRevBuf[28]);
                        if (DeviceType != 0)
                            strChannel = CsConst.myRevBuf[27].ToString();
                        if (1 <= CsConst.myRevBuf[29] && CsConst.myRevBuf[29] <= myir.IRCodes.Count && DeviceType != 0)
                        {
                            strDev = CsConst.myRevBuf[29].ToString();
                            if (DeviceType == 1)
                                strDev = strDev + "-" + myir.IRCodes[CsConst.myRevBuf[29] - 1].Remark;
                            else if (DeviceType == 2)
                            {
                                if (mintDeviceType == 6100)
                                    strDev = strDev + "-" + myir.IRCodes[CsConst.myRevBuf[29] + 2].Remark;
                                else
                                    strDev = strDev + "-" + myir.IRCodes[CsConst.myRevBuf[29] + 3].Remark;
                            }
                        }
                        else if (CsConst.myRevBuf[29] <= (myir.IRCodes.Count + 1) && DeviceType != 0)
                        {
                            strDev = cbUVDev.Items[cbUVDev.Items.Count - 1].ToString();
                        }

                        if (DeviceType == 1)
                        {
                            strEditType = CsConst.mstrINIDefault.IniReadValue("public", "99622", "");
                            if (CsConst.myRevBuf[30] <= 1)
                                strState = cbState.Items[CsConst.myRevBuf[30]].ToString() + "(" + CsConst.mstrINIDefault.IniReadValue("public", "99620", "") + ")";
                            if (0 <= CsConst.myRevBuf[31] && CsConst.myRevBuf[31] <= 4)
                                str1 = CsConst.mstrINIDefault.IniReadValue("public", "0005" + (CsConst.myRevBuf[31]).ToString(), "")
                                     + "(" + CsConst.mstrINIDefault.IniReadValue("public", "99876", "") + ")";
                            if (0 <= CsConst.myRevBuf[32] && CsConst.myRevBuf[32] <= 3)
                                str2 = CsConst.mstrINIDefault.IniReadValue("public", "0006" + (CsConst.myRevBuf[32]).ToString(), "")
                                     + "(" + CsConst.mstrINIDefault.IniReadValue("public", "99875", "") + ")";
                            if (CsConst.myRevBuf[33] == 1)
                                str3 = CsConst.mstrINIDefault.IniReadValue("public", "99896", "");
                            else if (CsConst.myRevBuf[33] == 0)
                                str3 = CsConst.mstrINIDefault.IniReadValue("public", "99902", "");
                            str4 = CsConst.mstrINIDefault.IniReadValue("public", "99897", "") + " " + CsConst.myRevBuf[34].ToString();
                            str5 = CsConst.myRevBuf[35].ToString() + "(" + CsConst.mstrINIDefault.IniReadValue("public", "99874", "") + ")";
                        }
                        else if (DeviceType == 2)
                        {
                            strEditType = CsConst.mstrINIDefault.IniReadValue("public", "99623", "");
                            if (CsConst.myRevBuf[29] <= (myir.IRCodes.Count))
                            {
                                int devid = myir.IRCodes[CsConst.myRevBuf[29] + 3].DevID;
                                if (mintDeviceType == 6100) devid = myir.IRCodes[CsConst.myRevBuf[29] + 2].DevID;
                                string strTmp = CsConst.mstrINIDefault.IniReadValue("NewIRDevice" + devid.ToString(), CsConst.myRevBuf[30].ToString("D5"), "");
                                if (strTmp != "")
                                    strState = CsConst.mstrINIDefault.IniReadValue("NewIRDevice" + devid.ToString(), CsConst.myRevBuf[30].ToString("D5"), "")
                                         + "(" + CsConst.mstrINIDefault.IniReadValue("public", "99898", "") + ")";
                            }
                            else if (CsConst.myRevBuf[29] == (myir.IRCodes.Count + 1))
                            {
                                strState = CsConst.myRevBuf[30].ToString() + "(" + CsConst.mstrINIDefault.IniReadValue("public", "99898", "") + ")";
                            }
                            str1 = (CsConst.myRevBuf[32] * 256 + CsConst.myRevBuf[33]).ToString() + "(" + CsConst.mstrINIDefault.IniReadValue("public", "99899", "") + ")";
                            str2 = HDLPF.GetStringFromTime(Convert.ToInt32(CsConst.myRevBuf[34]), ".") +
                                   "(" + CsConst.mstrINIDefault.IniReadValue("public", "99900", "") + ")";
                            if (CsConst.myRevBuf[31] <= 1)
                                str3 = cbState.Items[CsConst.myRevBuf[31]].ToString();
                        }


                        object[] obj = new object[] { i.ToString() + strOFF, strDev, strChannel,strEditType, strState, str1, str2, str3, str4, str5 };
                        dgvUV.Rows.Add(obj);
                        
                    }
                    else
                    {
                        break;
                    }
                }
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnSaveUV_Click(object sender, EventArgs e)
        {
            if (CsConst.MyEditMode == 1)
            {
                setConVisible(false);
                Cursor.Current = Cursors.WaitCursor;
                for (int i = 0; i < dgvUV.Rows.Count; i++)
                {
                    byte[] arayTmp = new byte[11];
                    string str1 = dgvUV[4, i].Value.ToString();
                    string str2 = dgvUV[5, i].Value.ToString();
                    string str3 = dgvUV[6, i].Value.ToString();
                    string str4 = dgvUV[7, i].Value.ToString();
                    string str5 = dgvUV[8, i].Value.ToString();
                    string str6 = dgvUV[9, i].Value.ToString();
                    if (str1.Contains("(")) str1 = str1.Split('(')[0].ToString();
                    if (str2.Contains("(")) str2 = str2.Split('(')[0].ToString();
                    if (str3.Contains("(")) str3 = str3.Split('(')[0].ToString();
                    if (str4.Contains("(")) str4 = str4.Split('(')[0].ToString();
                    if (str5.Contains("(")) str5 = str5.Split('(')[0].ToString();
                    if (str6.Contains("(")) str6 = str6.Split('(')[0].ToString();
                    arayTmp[0] = Convert.ToByte(dgvUV[0, i].Value.ToString().Split('-')[0].ToString());
                    if (dgvUV[0, i].Value.ToString().Split('-')[1] == CsConst.Status[1])
                        arayTmp[1] = 1;
                    else if (dgvUV[0, i].Value.ToString().Split('-')[1] == CsConst.Status[0])
                        arayTmp[1] = 0;
                    for (int j = 2; j < 11; j++)
                        arayTmp[j] = 1;
                    if (dgvUV[1, i].Value.ToString() == CsConst.WholeTextsList[1775].sDisplayName)
                        arayTmp[3] = 0;
                    else
                    {
                        arayTmp[2] = Convert.ToByte(dgvUV[2, i].Value.ToString());
                        arayTmp[4] = Convert.ToByte(dgvUV[1, i].Value.ToString().Split('-')[0].ToString());
                        if (dgvUV[3, i].Value.ToString() == cbUVEditType.Items[0].ToString()) arayTmp[3] = 1;
                        else arayTmp[3] = 2;
                        if (arayTmp[3] == 1)
                        {
                            if (str1 != "N/A")
                                arayTmp[5] = Convert.ToByte(cbUVState.Items.IndexOf(str1));
                            if (str2 != "N/A")
                                arayTmp[6] = Convert.ToByte(cbUVACMode.Items.IndexOf(str2));
                            if (str3 != "N/A")
                                arayTmp[7] = Convert.ToByte(cbUVACFAN.Items.IndexOf(str3));
                            if (str4 == CsConst.mstrINIDefault.IniReadValue("public", "99896", ""))
                                arayTmp[8] = 1;
                            else if (str4 == CsConst.mstrINIDefault.IniReadValue("public", "99902", ""))
                                arayTmp[8] = 0;
                            if (cbUVManualWind.Items.Contains(str5))
                                arayTmp[9] = Convert.ToByte(cbUVManualWind.Items.IndexOf(str5) + 1);
                            arayTmp[10] = Convert.ToByte(str6);
                        }
                        else if (arayTmp[3] == 2)
                        {
                            int DeviceID = Convert.ToInt32(dgvUV[1, i].Value.ToString().Split('-')[0]);
                            if (DeviceID <= (cbUVDev.Items.Count - 2))
                            {
                                DeviceID = Convert.ToInt32(dgvUV[1, i].Value.ToString().Split('-')[0]) + 3;
                                if (mintDeviceType == 6100) DeviceID = Convert.ToInt32(dgvUV[1, i].Value.ToString().Split('-')[0]) + 2;
                                int devid = myir.IRCodes[DeviceID].DevID;
                                if (devid > 6) continue;
                                cbUVKeyNo.Items.Clear();
                                cbUVKeyNo.Items.AddRange(CsConst.mstrINIDefault.IniReadValuesInALlSectionStr("NewIRDevice" + devid.ToString()));
                                if (str1 != "N/A")
                                    arayTmp[5] = Convert.ToByte(cbUVKeyNo.Items.IndexOf(str1) + 1);
                            }
                            else if (DeviceID <= (cbUVDev.Items.Count - 1))
                            {
                                arayTmp[5] = Convert.ToByte(str1.Split('(')[0].ToString());
                            }
                            arayTmp[6] = Convert.ToByte(cbUVState.Items.IndexOf(str4));
                            int intTmp = Convert.ToInt32(str2);
                            arayTmp[7] = Convert.ToByte(intTmp / 256);
                            arayTmp[8] = Convert.ToByte(intTmp % 256);
                            if (str3.Contains("."))
                                arayTmp[9] = Convert.ToByte(HDLPF.GetTimeFromString(str3, '.'));
                        }
                    }
                    if (CsConst.mySends.AddBufToSndList(arayTmp, 0xDB94, SubNetID, DevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(mintDeviceType)) == true)
                    {
                        
                        System.Threading.Thread.Sleep(100);
                    }
                    else
                    {
                        break;
                    }
                }
                Cursor.Current = Cursors.Default;
            }
        }

        private void dgvUV_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            setConVisible(false);
            if (e.RowIndex >= 0)
            {
                cbUVDev.SelectedIndex = cbUVDev.Items.IndexOf(dgvUV[1, e.RowIndex].Value.ToString());
                addcontrols(1, e.RowIndex, cbUVDev, dgvUV);
            }
            cbUVDev_SelectedIndexChanged(null, null);
        }

        private void btnReadDetect_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            byte[] ArayTmp = new byte[0];
            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xD970, SubNetID, DevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(mintDeviceType)) == true)
            {
                txtDTime.Text = CsConst.myRevBuf[25].ToString();
                txtDValue.Text = CsConst.myRevBuf[26].ToString();
                if (CsConst.myRevBuf[27] <= 28) cbDDevice.SelectedIndex = (CsConst.myRevBuf[27] - 1);
                cbDDevice_SelectedIndexChanged(null, null);
                if (cbDKey.Items.Count > 0 && CsConst.myRevBuf[28] < cbDKey.Items.Count)
                    cbDKey.SelectedIndex = (CsConst.myRevBuf[28] - 1);
                if (CsConst.myRevBuf[29] == 1) lbDSatusValue.Text = CsConst.Status[1];
                else if (CsConst.myRevBuf[29] == 0) lbDSatusValue.Text = CsConst.Status[0];
            }
            ArayTmp = new byte[1];
            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xDB9A, SubNetID, DevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(mintDeviceType)) == true)
            {
                if (CsConst.myRevBuf[26] == 1) chbLoad.Checked = true;
                else chbLoad.Checked = false;
                txtLoad1.Text = CsConst.myRevBuf[27].ToString();
                txtLoad2.Text = CsConst.myRevBuf[28].ToString();
                txtLoad3.Text = CsConst.myRevBuf[29].ToString();
            }
            
            Cursor.Current = Cursors.Default;
        }

        private void btnSaveDetect_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            byte[] arayTmp = new byte[4];
            arayTmp[0] = Convert.ToByte(txtDTime.Text);
            arayTmp[1] = Convert.ToByte(txtDValue.Text);
            arayTmp[2] = Convert.ToByte(cbDDevice.SelectedIndex + 1);
            if (cbDKey.SelectedIndex >= 0)
                arayTmp[3] = Convert.ToByte(cbDKey.SelectedIndex + 1);
            if (CsConst.mySends.AddBufToSndList(arayTmp, 0xD972, SubNetID, DevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(mintDeviceType)) == true)
            {
                CsConst.myRevBuf=new byte[1200];
                HDLUDP.TimeBetwnNext(20);
            }
            else
            {
                Cursor.Current = Cursors.Default;
                return;
            }
            arayTmp = new byte[5];
            arayTmp[0] = 1;
            if(chbLoad.Checked) arayTmp[1] = 1;
            arayTmp[2] = Convert.ToByte(txtLoad1.Text);
            arayTmp[3] = Convert.ToByte(txtLoad2.Text);
            arayTmp[4] = Convert.ToByte(txtLoad3.Text);
            if (CsConst.mySends.AddBufToSndList(arayTmp, 0xDB9A, SubNetID, DevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(mintDeviceType)) == true)
            {
                
            }
            Cursor.Current = Cursors.Default;
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

        private void cbDDevice_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbDDevice.SelectedIndex >= 0 && cbDDevice.Text != "")
            {
                if (cbDDevice.SelectedIndex < (cbDDevice.Items.Count - 1))
                {
                    int devid = myir.IRCodes[Convert.ToInt32(cbDDevice.Text.Split('-')[0]) + 3].DevID;
                    cbDKey.Items.Clear();
                    if (devid < 6)
                    {
                        cbDKey.Items.AddRange(CsConst.mstrINIDefault.IniReadValuesInALlSectionStr("NewIRDevice" + devid.ToString()));
                        if (cbDKey.Items.Count > 0 && cbDKey.SelectedIndex < 0) cbDKey.SelectedIndex = 0;
                    }
                }
                else if (cbDDevice.SelectedIndex == (cbDDevice.Items.Count - 1))
                {
                    cbDKey.Items.Clear();
                    for (int i = 1; i <= 100; i++)
                    {
                        cbDKey.Items.Add(i.ToString());
                    }
                }
            }
        }

        private void btnReadOn_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            byte[] ArayTmp = new byte[0];
            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xD974, SubNetID, DevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(mintDeviceType)) == true)
            {
                lbDOnValue.Text = CsConst.myRevBuf[25].ToString();
            }
            lbDAValue.Text = ((Convert.ToInt32(lbDOnValue.Text) + Convert.ToInt32(lbDOffValue.Text)) / 2).ToString();
            
            Cursor.Current = Cursors.Default;
        }

        private void btnReadOff_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            byte[] ArayTmp = new byte[0];
            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xD974, SubNetID, DevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(mintDeviceType)) == true)
            {
                lbDOffValue.Text = CsConst.myRevBuf[25].ToString();
            }
            lbDAValue.Text = ((Convert.ToInt32(lbDOnValue.Text) + Convert.ToInt32(lbDOffValue.Text)) / 2).ToString();
            
            Cursor.Current = Cursors.Default;
        }

        private void dgvIR_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            dgvIR.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void btnPre_Click(object sender, EventArgs e)
        {
            if (cbSeq.SelectedIndex == 0)
            {
                cbSeq.SelectedIndex = 7;
            }
            else
            {
                cbSeq.SelectedIndex = cbSeq.SelectedIndex - 1;
            }
            cbSeq_SelectedIndexChanged(null, null);
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (cbSeq.SelectedIndex == 7)
            {
                cbSeq.SelectedIndex = 0;
            }
            else
            {
                cbSeq.SelectedIndex = cbSeq.SelectedIndex + 1;
            }
            cbSeq_SelectedIndexChanged(null, null);
        }

        private void txtDTime_Leave(object sender, EventArgs e)
        {
            if (txtDTime.Text.Trim() == "")
            {
                txtDTime.Text = "0";
            }
        }

        private void txtDValue_Leave(object sender, EventArgs e)
        {
            if (txtDValue.Text.Trim() == "") txtDValue.Text = "1";
        }


        private void btnSaveSeqRemark_Click(object sender, EventArgs e)
        {
            if (CsConst.MyEditMode == 1)
            {
                string str = txtRemark.Text;
                Cursor.Current = Cursors.WaitCursor;
                byte[] arayTmp = new byte[23];
                arayTmp[0] = Convert.ToByte(cbSeq.SelectedIndex + 1);
                arayTmp[1] = 0;
                byte[] arayRemark = new byte[20];
                arayRemark = HDLUDP.StringToByte(str);
                if (arayRemark.Length > 20)
                    Array.Copy(arayRemark, 0, arayTmp, 2, 20);
                else
                    Array.Copy(arayRemark, 0, arayTmp, 2, arayRemark.Length);
                if (chbEnable.Checked) arayTmp[22] = 1;
                if (CsConst.mySends.AddBufToSndList(arayTmp, 0xDB82, SubNetID, DevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(mintDeviceType)) == true)
                {
                    
                }
                Cursor.Current = Cursors.Default;
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

        private void btnUpload_Click(object sender, EventArgs e)
        {
            try
            {
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
                int RowIndex = dgvKey.CurrentRow.Index;
                int RowCount = dgvKey.RowCount;
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
                int FirstIndex = dgvKey.CurrentRow.Index;
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
                    byte[] arayTmp = new byte[4];
                    if (mintDeviceType == 6100)
                        arayTmp[0] = 11;
                    else
                        arayTmp[0] = 29;
                    arayTmp[1] = Convert.ToByte(Convert.ToInt32(dgvKey[0, dgvKey.CurrentRow.Index].Value.ToString()));
                    arayTmp[2] = arayCode[2];
                    arayTmp[3] = arayCode[3];
                    if (CsConst.mySends.AddBufToSndList(arayTmp, 0xDB6E, SubNetID, DevID, false, true, true,CsConst.minAllWirelessDeviceType.Contains(mintDeviceType)) == true)
                    {
                        if (CsConst.myRevBuf[27] == 0xF8)
                        {
                            
                            for (int j = 0; j < ListData.Length; j++)
                            {
                                arayTmp = new byte[18];
                                if (mintDeviceType == 6100)
                                    arayTmp[0] = 11;
                                else
                                    arayTmp[0] = 29;
                                arayTmp[1] = Convert.ToByte(Convert.ToInt32(dgvKey[0, dgvKey.CurrentRow.Index].Value.ToString()));
                                Array.Copy(arayCode, j * 16, arayTmp, 2, 16);
                                if (CsConst.mySends.AddBufToSndList(arayTmp, 0xDB70, SubNetID, DevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(mintDeviceType)) == true)
                                {
                                    
                                }
                                else
                                {
                                    Cursor.Current = Cursors.Default;
                                    return;
                                }
                                HDLUDP.TimeBetwnNext(20);
                            }
                            arayTmp = new byte[22];
                            if (mintDeviceType == 6100)
                                arayTmp[0] = 11;
                            else
                                arayTmp[0] = 29;
                            arayTmp[1] = Convert.ToByte(Convert.ToInt32(dgvKey[0, dgvKey.CurrentRow.Index].Value.ToString()));
                            string strRemark = listRemark[CodeIndex];
                            byte[] arayTmpRemark = HDLUDP.StringToByte(strRemark);
                            if (arayTmpRemark.Length >= 20)
                                Array.Copy(arayTmpRemark, 0, arayTmp, 2, 20);
                            else 
                                Array.Copy(arayTmpRemark, 0, arayTmp, 2, arayTmpRemark.Length);
                            if (CsConst.mySends.AddBufToSndList(arayTmp, 0xDB74, SubNetID, DevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(mintDeviceType)) == true)
                            {
                                
                            }
                            else
                            {
                                Cursor.Current = Cursors.Default;
                                return;
                            }
                            dgvKey.Rows[FirstIndex + CodeIndex].Cells[1].Value = strRemark;
                            dgvKey.Rows[FirstIndex + CodeIndex].Cells[2].Value = CsConst.mstrINIDefault.IniReadValue("public", "00042", "");
                            CodeIndex = CodeIndex + 1;
                            dgvKey.Rows[FirstIndex + CodeIndex].Selected = true;
                            this.dgvKey.CurrentCell = this.dgvKey.Rows[FirstIndex + CodeIndex].Cells[0];
                            if (isStopDownloadCodes) break;
                            if (CodeIndex >= listCode.Count) break;
                        }
                    }
                    else break;
                    if (MyBackGroup != null && MyBackGroup.IsBusy) MyBackGroup.ReportProgress(i * 90 / RowCount);
                }
                #endregion
            }
            catch
            {
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

        private void button1_Click_1(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            byte[] ArayTmp = new byte[3];
            if (mintDeviceType == 6100)
                ArayTmp[0] = 11;
            else
                ArayTmp[0] = 29;
            ArayTmp[1] = 0;
            ArayTmp[2] = 100;
            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xdb86, SubNetID, DevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(mintDeviceType)) == true)
            {
                myir.IRKeys = new List<UVCMD.IRCode>();
                dgvKey.Rows.Clear();
                byte[] arayEnable = new byte[13];
                Array.Copy(CsConst.myRevBuf, 28, arayEnable, 0, 13);
                string strEnable = "";

                for (int i = 12; i >= 0; i--)
                {
                    string strTmp = GlobalClass.AddLeftZero(Convert.ToString(arayEnable[i], 2), 8);
                    for (int j = 7; j >= 0; j--)
                    {
                        string str = strTmp.Substring(j, 1);
                        strEnable = strEnable + str;
                    }
                }

                int intFrm = Convert.ToInt32(txtKeyFrm.Text);
                int intTo = Convert.ToInt32(txtKeyTo.Text);

                for (int i = intFrm; i <= intTo; i++)
                {
                    string strRemark = "";
                    string strValid = CsConst.WholeTextsList[1775].sDisplayName;
                    if (strEnable.Substring(i - 1, 1) == "1")
                    {
                        strValid = CsConst.mstrINIDefault.IniReadValue("public", "00042", "");
                        ArayTmp = new byte[2];
                        if (mintDeviceType == 6100)
                            ArayTmp[0] = 11;
                        else
                            ArayTmp[0] = 29;
                        ArayTmp[1] = (byte)i;
                        if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xdB72, SubNetID, DevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(mintDeviceType)) == true)
                        {
                            byte[] arayRemark = new byte[20];
                            for (int intI = 0; intI < 20; intI++) { arayRemark[intI] = CsConst.myRevBuf[29 + intI]; }
                            strRemark =  HDLPF.Byte2String(arayRemark);
                            
                        }
                        else
                        {
                            Cursor.Current = Cursors.Default;
                            break;
                        }
                    }
                    object[] obj = new object[] { i.ToString(), strRemark, strValid, 
                        CsConst.mstrINIDefault.IniReadValue("public", "99810", "") };
                    dgvKey.Rows.Add(obj);
                }
            }
            
            Cursor.Current = Cursors.Default;
        }

        private void dgvKey_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                Cursor.Current = Cursors.WaitCursor;
                if (e.ColumnIndex == 3)
                {
                    if (dgvKey[2, e.RowIndex].Value.ToString() == CsConst.WholeTextsList[1775].sDisplayName) return;
                    int Key = Convert.ToInt32(dgvKey[0, e.RowIndex].Value.ToString());
                    byte[] arayTmp = new byte[2];
                    arayTmp[0] = 11;
                    arayTmp[1] = Convert.ToByte(Key);
                    if (CsConst.mySends.AddBufToSndList(arayTmp, 0xDB7A, SubNetID, DevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(mintDeviceType)) == true)
                    {
                        dgvKey[1, e.RowIndex].Value = "";
                        dgvKey[2, e.RowIndex].Value = CsConst.WholeTextsList[1775].sDisplayName;
                        
                    }
                }
                Cursor.Current = Cursors.Default;
            }
        }

        private void txtKeyFrm_Leave(object sender, EventArgs e)
        {
            string str = txtKeyFrm.Text;
            int num = Convert.ToInt32(txtKeyTo.Text);
            if (HDLPF.IsRightNumStringMode(str, 1, num))
            {
                txtKeyFrm.SelectionStart = txtKeyFrm.Text.Length;
            }
            else
            {
                if (txtKeyFrm.Text == null || txtKeyFrm.Text == "")
                {
                    txtKeyFrm.Text = "1";
                }
                else
                {
                    txtKeyTo.Text = txtKeyFrm.Text;
                    txtKeyTo.Focus();
                    txtKeyTo.SelectionStart = txtKeyTo.Text.Length;
                }
            }
        }

        private void txtKeyTo_Leave(object sender, EventArgs e)
        {
            string str = txtKeyTo.Text;
            int num = Convert.ToInt32(txtKeyFrm.Text);
            txtKeyTo.Text = HDLPF.IsNumStringMode(str, num, 100);
            txtKeyTo.SelectionStart = txtTo.Text.Length;
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

        private void btnTest_Click(object sender, EventArgs e)
        {
            byte[] arayTmp = new byte[4];
            int int1 = 0;
            int int2 = cbChn.SelectedIndex + 1;
            string str1 = GlobalClass.IntToBit(int1, 2);
            string str2 = GlobalClass.IntToBit(int2, 6);
            string str = str1 + str2;
            arayTmp[0] = Convert.ToByte(GlobalClass.BitToInt(str));
            if (mintDeviceType == 6100)
                arayTmp[1] = 11;
            else if (mintDeviceType == 1301 || mintDeviceType == 1300)
                arayTmp[1] = 31;
            else
                arayTmp[1] = 29;
            arayTmp[2] = Convert.ToByte(Convert.ToByte(lbCurSelValue.Text));
            if (CsConst.mySends.AddBufToSndList(arayTmp, 0xdb90, SubNetID, DevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(mintDeviceType)) == true)
            {
                
            }
        }

        private void dgvKey_SelectionChanged(object sender, EventArgs e)
        {
            lbCurSelValue.Text = dgvKey[0, dgvKey.CurrentRow.Index].Value.ToString();
        }

        private void btnInitital_Click(object sender, EventArgs e)
        {

            try
            {
                DialogResult result = MessageBox.Show(CsConst.mstrINIDefault.IniReadValue("public", "99805", ""), "", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                if (result == DialogResult.OK)
                {
                    byte[] arayTmp = new byte[1];
                    arayTmp[0] = 25;
                    if (CsConst.mySends.AddBufToSndList(arayTmp, 0xd9E0, SubNetID, DevID, false, false, false, CsConst.minAllWirelessDeviceType.Contains(mintDeviceType)) == true)
                    {
                        
                    }
                    txtRemark.Visible = false;
                    CsConst.isIniAllKeySucess = false;
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
                if (CsConst.isIniAllKeySucess)
                {
                    MessageBox.Show(CsConst.mstrINIDefault.IniReadValue("public", "99806", ""));
                    dgvIR.Rows.Clear();
                }
                else
                {
                    MessageBox.Show(CsConst.mstrINIDefault.IniReadValue("public", "99807", ""));
                }
                
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
                DateTime t1 = DateTime.Now;
                DateTime t2 = DateTime.Now;
                while (!CsConst.isIniAllKeySucess)
                {
                    t2 = DateTime.Now;
                    if (HDLSysPF.Compare(t2, t1) > 180000)
                    {
                        break;
                    }
                }

            }
            catch
            {
            }
        }


        private void button2_Click(object sender, EventArgs e)
        {
            btnSaveUV_Click(null, null);
            this.Close();
        }

        private void btnSaveAndClose3_Click_1(object sender, EventArgs e)
        {
            btnSave_Click(null, null);
            this.Close();
        }

        private void btnSaveAndClose5_Click(object sender, EventArgs e)
        {
            btnSaveDetect_Click(null, null);
            this.Close();
        }

        private void btnSave2_Click(object sender, EventArgs e)
        {
            if (dgvKey.RowCount<=0) return;
            Cursor.Current = Cursors.WaitCursor;
            for (int i = 0; i < dgvKey.Rows.Count; i++)
            {
                byte[] arayRemark = new byte[22];
                if (dgvKey[1, i].Value == null) dgvKey[1, i].Value = "";
                string strRemark = dgvKey[1,i].Value.ToString();
                byte[] arayTmp = HDLUDP.StringToByte(strRemark);
                if (arayTmp.Length > 20)
                {
                    Array.Copy(arayTmp, 0, arayRemark, 2, 20);
                }
                else
                {
                    Array.Copy(arayTmp, 0, arayRemark, 2, arayTmp.Length);
                }
                if (mintDeviceType == 6100)
                    arayRemark[0] = 11;
                else
                    arayRemark[0] = 29;
                arayRemark[1] = Convert.ToByte(Convert.ToInt32(dgvKey[0, i].Value.ToString()));
                if (CsConst.mySends.AddBufToSndList(arayRemark, 0xDB74, SubNetID, DevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(mintDeviceType)) == true)
                {
                    HDLUDP.TimeBetwnNext(20);
                    
                }
                else
                {
                    break;
                }
            }
            Cursor.Current = Cursors.Default;
        }

        private void btnSaveAndClose2_Click(object sender, EventArgs e)
        {
            btnSave2_Click(null, null);
            this.Close();
        }

        private void btnPC_Click(object sender, EventArgs e)
        {
            DateTime d1;
            d1 = DateTime.Now;
            numTime1.Value = Convert.ToDecimal(d1.Hour);
            numTime2.Value = Convert.ToDecimal(d1.Minute);
            numTime3.Value = Convert.ToDecimal(d1.Second);
            DatePicker.Text = d1.ToString();
        }

        private void DatePicker_ValueChanged(object sender, EventArgs e)
        {
            DayOfWeek Week = DatePicker.Value.DayOfWeek;
            label5.Text = CsConst.mstrINIDefault.IniReadValue("Public", "0075" + Week.GetHashCode().ToString(), "");
            if (isReadding) return;
            if (myir == null) return;
            if (myir.arayTime == null) myir.arayTime = new byte[7];
            myir.arayTime[0] = Convert.ToByte(DatePicker.Value.Year - 2000);
            myir.arayTime[1] = Convert.ToByte(DatePicker.Value.Month);
            myir.arayTime[2] = Convert.ToByte(DatePicker.Value.Day);
            myir.arayTime[6] = Convert.ToByte(Week);
        }

        private void cbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cbType.SelectedIndex == 0)
                {
                    pnlWeek.Visible = true;
                    pnlDate.Visible = false;
                }
                else if (cbType.SelectedIndex == 1)
                {
                    pnlWeek.Visible = false;
                    pnlDate.Visible = true;
                }
                if (isReadding) return;
                if (myir == null) return;
                if (myir.Clocks == null) return;
                myir.Clocks[dgvClock.CurrentCell.ColumnIndex].Type = Convert.ToByte(cbType.SelectedIndex);
            }
            catch
            {
            }
        }

        private void dgvClock_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                isReadding = true;
                if (myir == null) return;
                if (myir.Clocks == null) return;
                NewIR.Clock temp = myir.Clocks[e.ColumnIndex];
                if (temp.Enable == 1)
                {
                    picEnable.Image = imageList2.Images[1];
                    picEnable.Image.Tag = 1;
                }
                else
                {
                    picEnable.Image = imageList2.Images[0];
                    picEnable.Image.Tag = 0;
                }
                if (temp.Type <= 1)
                    cbType.SelectedIndex = temp.Type;
                if (cbType.SelectedIndex < 0) cbType.SelectedIndex = 0;
                cbType_SelectedIndexChanged(null, null);
                if (temp.Type == 0)
                {
                    if (numC1.Minimum <= temp.arayParam[2] && temp.arayParam[2] <= numC1.Maximum)
                        numC1.Value = temp.arayParam[2];
                    else
                        temp.arayParam[2] = Convert.ToByte(numC1.Value);
                    if (numC2.Minimum <= temp.arayParam[3] && temp.arayParam[3] <= numC2.Maximum)
                        numC2.Value = temp.arayParam[3];
                    else
                        temp.arayParam[3] = Convert.ToByte(numC2.Value);
                    for (int i = 0; i < 7; i++)
                    {
                        if (HDLSysPF.GetBit(temp.arayParam[4], i) == 1)
                            chbList.SetItemChecked(i, true);
                        else
                            chbList.SetItemChecked(i, false);
                    }
                }
                else if (temp.Type == 1)
                {
                    if (numM.Minimum <= temp.arayParam[0] && temp.arayParam[0] <= numM.Maximum)
                        numM.Value = temp.arayParam[0];
                    else
                        temp.arayParam[0] = Convert.ToByte(numM.Value);
                    if (numD.Minimum <= temp.arayParam[1] && temp.arayParam[1] <= numD.Maximum)
                        numD.Value = temp.arayParam[1];
                    else
                        temp.arayParam[1] = Convert.ToByte(numD.Value);
                    if (numH.Minimum <= temp.arayParam[2] && temp.arayParam[2] <= numH.Maximum)
                        numH.Value = temp.arayParam[2];
                    else
                        temp.arayParam[2] = Convert.ToByte(numH.Value);
                    if (numS.Minimum <= temp.arayParam[3] && temp.arayParam[3] <= numS.Maximum)
                        numS.Value = temp.arayParam[3];
                    else
                        temp.arayParam[3] = Convert.ToByte(numS.Value);
                }
                if (1 <= temp.SceneID && temp.SceneID <= 16)
                    cbTrigger.SelectedIndex = temp.SceneID - 1;
                if (cbTrigger.SelectedIndex < 0) cbTrigger.SelectedIndex = 0;
            }
            catch
            {
            }
            isReadding = false;
        }

        private void picEnable_Click(object sender, EventArgs e)
        {
            try
            {
                int Tag = Convert.ToInt32(picEnable.Image.Tag);
                if (Tag == 1)
                {
                    picEnable.Image = imageList2.Images[0];
                    picEnable.Image.Tag = 0;
                    dgvClock[dgvClock.CurrentCell.ColumnIndex, 0].Value = imageList2.Images[0];
                }
                else
                {
                    picEnable.Image = imageList2.Images[1];
                    picEnable.Image.Tag = 1;
                    dgvClock[dgvClock.CurrentCell.ColumnIndex, 0].Value = imageList2.Images[1];
                }
                if (isReadding) return;
                if (myir == null) return;
                if (myir.Clocks == null) return;
                myir.Clocks[dgvClock.CurrentCell.ColumnIndex].Enable = Convert.ToByte(picEnable.Image.Tag);
            }
            catch
            {
            }
        }

        private void cbScene_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (isReadding) return;
                dgvTarget.Rows.Clear();
                for (int i = 0; i < 6; i++)
                {
                    byte[] ArayTmp = new byte[2];
                    ArayTmp[0] = Convert.ToByte(cbScene.SelectedIndex + 1);
                    ArayTmp[1] = Convert.ToByte(i + 1);
                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1402, SubNetID, DevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(mintDeviceType)) == true)
                    {
                        string strType = "";
                        strType = DryMode.ConvertorKeyModeToPublicModeGroup(CsConst.myRevBuf[27]);
                        string strParam1 = "0", strParam2 = "0", strParam3 = "0", strParam4 = "0";
                        strParam1 = CsConst.myRevBuf[30].ToString();
                        strParam2 = CsConst.myRevBuf[31].ToString();
                        strParam3 = CsConst.myRevBuf[32].ToString();
                        strParam4 = CsConst.myRevBuf[33].ToString();
                        SetParams(ref strType, ref strParam1, ref strParam2, ref strParam3, strParam4);
                        object[] obj = new object[] { (i+1).ToString(),CsConst.myRevBuf[28].ToString(),CsConst.myRevBuf[29].ToString(),strType
                                ,strParam1,strParam2,strParam3};
                        dgvTarget.Rows.Add(obj);
                        
                    }
                    else break;
                }
            }
            catch
            {
            }
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
                if (str1 == CsConst.myPublicControlType[0].ControlTypeName)//无效
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
                if (str1 == CsConst.MusicControl[0])
                {
                    intTmp = Convert.ToInt32(str2);
                    if (1 <= intTmp && intTmp <= 4) str2 = CsConst.mstrINIDefault.IniReadValue("public", "0010" + intTmp.ToString(), "");
                    str3 = "N/A";
                }
                else if (str1 == CsConst.mstrINIDefault.IniReadValue("public", "00092", ""))
                {
                    intTmp = Convert.ToInt32(str2);
                    if (1 <= intTmp && intTmp <= 4) str2 = CsConst.mstrINIDefault.IniReadValue("public", "0011" + intTmp.ToString(), "");
                    str3 = "N/A";
                }
                else if (str1 == CsConst.mstrINIDefault.IniReadValue("public", "00093", ""))
                {
                    intTmp = Convert.ToInt32(str2);
                    if (1 <= intTmp && intTmp <= 6) str2 = CsConst.mstrINIDefault.IniReadValue("public", "0012" + intTmp.ToString(), "");
                    if (intTmp == 3 || intTmp == 6)
                        str3 = str3 + "(" + CsConst.mstrINIDefault.IniReadValue("public", "00043", "") + ")";
                    else
                        str3 = "N/A";
                }
                else if (str1 == CsConst.mstrINIDefault.IniReadValue("public", "00094", ""))
                {
                    intTmp = Convert.ToInt32(str2);
                    if (1 <= intTmp && intTmp <= 4) str2 = CsConst.mstrINIDefault.IniReadValue("public", "0013" + intTmp.ToString(), "");
                    str3 = "N/A";
                }
                else if (str1 == CsConst.mstrINIDefault.IniReadValue("public", "00095", ""))
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
                else if (str1 == CsConst.mstrINIDefault.IniReadValue("public", "00096", ""))
                {
                    str2 = str2 + "(" + CsConst.mstrINIDefault.IniReadValue("public", "00046", "") + ")";
                    str3 = (Convert.ToInt32(str3) * 256 + Convert.ToInt32(str4)).ToString() + "(" + CsConst.mstrINIDefault.IniReadValue("public", "00043", "") + ")";
                }
                else if (str1 == CsConst.mstrINIDefault.IniReadValue("public", "00097", "") ||
                         str1 == CsConst.mstrINIDefault.IniReadValue("public", "00098", ""))
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

        private void btnRef6_Click(object sender, EventArgs e)
        {
            tslRead_Click(tslRead, null);
        }

        private void numTime1_ValueChanged(object sender, EventArgs e)
        {
            if (isReadding) return;
            if (myir == null) return;
            if (myir.arayTime == null) myir.arayTime = new byte[7];
            myir.arayTime[3] = Convert.ToByte(numTime1.Value);
            myir.arayTime[4] = Convert.ToByte(numTime2.Value);
            myir.arayTime[5] = Convert.ToByte(numTime3.Value);
        }

        private void numC1_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (isReadding) return;
                if (myir == null) return;
                if (myir.Clocks == null) return;
                myir.Clocks[dgvClock.CurrentCell.ColumnIndex].arayParam[2] = Convert.ToByte(numC1.Value);
                myir.Clocks[dgvClock.CurrentCell.ColumnIndex].arayParam[3] = Convert.ToByte(numC2.Value);
            }
            catch
            {
            }
        }

        private void chbList_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            try
            {
                if (isReadding) return;
                if (myir == null) return;
                if (myir.Clocks == null) return;
                if (e.NewValue == CheckState.Checked)
                {
                    myir.Clocks[dgvClock.CurrentCell.ColumnIndex].arayParam[4] = HDLSysPF.SetBit(myir.Clocks[dgvClock.CurrentCell.ColumnIndex].arayParam[4], e.Index);
                }
                else if (e.NewValue == CheckState.Unchecked)
                {
                    myir.Clocks[dgvClock.CurrentCell.ColumnIndex].arayParam[4] = HDLSysPF.ClearBit(myir.Clocks[dgvClock.CurrentCell.ColumnIndex].arayParam[4], e.Index);
                }
            }
            catch
            {
            }
        }

        private void numM_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (isReadding) return;
                if (myir == null) return;
                if (myir.Clocks == null) return;
                myir.Clocks[dgvClock.CurrentCell.ColumnIndex].arayParam[0] = Convert.ToByte(numM.Value);
                myir.Clocks[dgvClock.CurrentCell.ColumnIndex].arayParam[1] = Convert.ToByte(numD.Value);
                myir.Clocks[dgvClock.CurrentCell.ColumnIndex].arayParam[2] = Convert.ToByte(numH.Value);
                myir.Clocks[dgvClock.CurrentCell.ColumnIndex].arayParam[3] = Convert.ToByte(numS.Value);
            }
            catch
            {
            }
        }

        private void cbTrigger_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (isReadding) return;
                if (myir == null) return;
                if (myir.Clocks == null) return;
                myir.Clocks[dgvClock.CurrentCell.ColumnIndex].SceneID = Convert.ToByte(cbTrigger.SelectedIndex + 1);
            }
            catch
            {
            }
        }

        private void dgvTarget_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            Form form = null;
            bool isOpen = true;
        }

        void TargetsFormClosed(object sender, FormClosedEventArgs e)
        {
            cbScene_SelectedIndexChanged(null, null);
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (CsConst.mySends.AddBufToSndList(null, 0xE0F8, SubNetID, DevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(mintDeviceType)) == true)
                {
                    Array.Copy(CsConst.myRevBuf, 25, myir.arayBrocast, 0, 5);
                    
                    chbTemp.Checked = (myir.arayBrocast[0] == 1);
                    txtTemp1.Text = myir.arayBrocast[1].ToString();
                    txtTemp2.Text = myir.arayBrocast[2].ToString();
                    sbAdjust.Value = myir.arayBrocast[3];
                    if (HDLSysPF.GetBit(myir.arayBrocast[4], 7) == 1)
                        lbCurrentValue.Text = "-" + ((myir.arayBrocast[4] & (byte.MaxValue - (1 << 7)))).ToString() + "C";
                    else
                        lbCurrentValue.Text = myir.arayBrocast[4].ToString() + "C";
                }
            }
            catch
            {
            }
            Cursor.Current = Cursors.Default;
        }

        private void btnSaveBroadcast_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                byte[] ArayTmp = new byte[4];
                for (int i = 0; i < 4; i++)
                {
                    ArayTmp[i] = myir.arayBrocast[i];
                }
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xE0FA, SubNetID, DevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(mintDeviceType)) == true)
                {
                    
                }
            }
            catch
            {
            }
            Cursor.Current = Cursors.Default;
        }

        private void btnSaveAndClose6_Click(object sender, EventArgs e)
        {
            tslRead_Click(toolStripLabel2, null);
            this.Close();
        }

        private void btnSave6_Click(object sender, EventArgs e)
        {
            tslRead_Click(toolStripLabel2, null);
        }

        private void sbAdjust_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                lbAdjust.Text = (sbAdjust.Value - 10).ToString() + "C";
                if (isReadding) return;
                if (myir.arayBrocast == null) return;
                myir.arayBrocast[3] = Convert.ToByte(sbAdjust.Value);
            }
            catch
            {
            }
        }

        private void chbTemp_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (isReadding) return;
                if (chbTemp.Checked)
                    myir.arayBrocast[0] = 1;
                else
                    myir.arayBrocast[0] = 0;
            }
            catch
            {
            }
        }

        private void txtTemp1_Leave(object sender, EventArgs e)
        {
            try
            {
                if (txtTemp1.Text.Length > 0)
                {
                    string str = txtTemp1.Text;
                    txtTemp1.Text = HDLPF.IsNumStringMode(str, 0, 255);
                    txtTemp1.SelectionStart = txtTemp1.Text.Length;
                }
                if (isReadding) return;
                myir.arayBrocast[1] = Convert.ToByte(txtTemp1.Text);
            }
            catch
            {
            }
        }

        private void txtTemp2_Leave(object sender, EventArgs e)
        {
            try
            {
                if (txtTemp2.Text.Length > 0)
                {
                    string str = txtTemp2.Text;
                    txtTemp2.Text = HDLPF.IsNumStringMode(str, 0, 255);
                    txtTemp2.SelectionStart = txtTemp2.Text.Length;
                }
                myir.arayBrocast[2] = Convert.ToByte(txtTemp2.Text);
            }
            catch
            {
            }
        }

        private void btnReadSysTime_Click(object sender, EventArgs e)
        {
            try
            {
                isReadding = true;
                Cursor.Current = Cursors.WaitCursor;
                byte[] ArayTmp = new byte[0];
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xDA00, SubNetID, DevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(mintDeviceType)) == true)
                {
                    Array.Copy(CsConst.myRevBuf, 26,myir.arayTime, 0, 6);
                    
                    int wdYear = Convert.ToInt32(myir.arayTime[0]) + 2000;
                    byte bytMonth = myir.arayTime[1];
                    byte bytDay = myir.arayTime[2];
                    byte bytHour = myir.arayTime[3];
                    byte bytMinute = myir.arayTime[4];
                    byte bytSecond = myir.arayTime[5];

                    if (bytHour > 23) bytHour = 0;
                    if (bytMinute > 59) bytMinute = 0;
                    if (bytSecond > 59) bytSecond = 0;
                    if (bytMonth > 12 || bytMonth < 1) bytMonth = 1;
                    if (bytDay > 31 || bytDay < 1) bytDay = 1;

                    DateTime d = new DateTime(wdYear, Convert.ToInt32(bytMonth), Convert.ToInt32(bytDay));
                    DatePicker.Value = d;
                    DatePicker_ValueChanged(null, null);
                    numTime1.Value = Convert.ToDecimal(bytHour);
                    numTime2.Value = Convert.ToDecimal(bytMinute);
                    numTime3.Value = Convert.ToDecimal(bytSecond);
                }
            }
            catch
            {
            }
            isReadding = false;
            Cursor.Current = Cursors.Default;
        }

        private void btnSaveSysTime_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                byte[] ArayTmp = new byte[7];
                for (int i = 0; i < 7; i++)
                {
                    ArayTmp[i] = myir.arayTime[i];
                }
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xDA02, SubNetID, DevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(mintDeviceType)) == true)
                {
                    
                }
            }
            catch
            {
            }
            Cursor.Current = Cursors.Default;
        }

        private void btnSaveAndClose1_Click(object sender, EventArgs e)
        {
            tslRead_Click(toolStripLabel2, null);
            this.Close();
        }

        private void btnSavePage1_Click(object sender, EventArgs e)
        {
            tslRead_Click(toolStripLabel2, null);
        }

        private void cbDKey_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void dgvIR_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (mintDeviceType == 1301 || mintDeviceType == 1300)
            {
                if (e.RowIndex >= 0 && e.RowIndex < 4)
                {
                    dgvIR[e.ColumnIndex, e.RowIndex].Style.BackColor = Color.Red;
                }
                else if (e.RowIndex >= 4)
                {
                    dgvIR[e.ColumnIndex, e.RowIndex].Style.BackColor = Color.Yellow;
                }
            }
            else if (mintDeviceType == 6100)
            {
                if (e.RowIndex >= 0 && e.RowIndex < 3)
                {
                    dgvIR[e.ColumnIndex, e.RowIndex].Style.BackColor = Color.Red;
                }
                else if (e.RowIndex >= 3)
                {
                    dgvIR[e.ColumnIndex, e.RowIndex].Style.BackColor = Color.Yellow;
                }
            }
        }

        private void btnRemark_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            for (byte i = 1; i <= 4; i++)
            {
                byte[] arayTmp = new byte[2];
                arayTmp[0] = 1;
                arayTmp[1] = i;
                if (CsConst.mySends.AddBufToSndList(arayTmp, 0xE504, SubNetID, DevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(mintDeviceType)) == true)
                {
                    byte[] arayRemark = new byte[20];
                    for (int intI = 0; intI < 20; intI++) { arayRemark[intI] = CsConst.myRevBuf[27 + intI]; }
                    string strRemark = HDLPF.Byte2String(arayRemark);
                    if (i == 1) txtC1.Text = strRemark;
                    else if (i == 2) txtC2.Text = strRemark;
                    else if (i == 3) txtC3.Text = strRemark;
                    else if (i == 4) txtC4.Text = strRemark;
                    
                    HDLUDP.TimeBetwnNext(1);
                }
                else
                {
                }
            }
            Cursor.Current = Cursors.Default;
        }

        private void btnSaveRemark_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            for (byte i = 1; i <= 4; i++)
            {
                byte[] arayTmp = new byte[22];
                arayTmp[0] = 2;
                arayTmp[1] = i;
                string strMainRemark = "";
                if (i == 1) strMainRemark = txtC1.Text;
                else if (i == 2) strMainRemark = txtC2.Text;
                else if (i == 3) strMainRemark = txtC3.Text;
                else if (i == 4) strMainRemark = txtC4.Text;
                byte[] arayTmpRemark = HDLUDP.StringToByte(strMainRemark);
                if (arayTmpRemark.Length > 20)
                {
                    Array.Copy(arayTmpRemark, 0, arayTmp, 2, 20);
                }
                else
                {
                    Array.Copy(arayTmpRemark, 0, arayTmp, 2, arayTmpRemark.Length);
                }
                if (CsConst.mySends.AddBufToSndList(arayTmp, 0xE504, SubNetID, DevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(mintDeviceType)) == true)
                {
                    
                    HDLUDP.TimeBetwnNext(20);
                }
                else break;
            }
            Cursor.Current = Cursors.Default;
        }

        private void txtLoad1_Leave(object sender, EventArgs e)
        {
            string str = txtLoad1.Text;
            txtLoad1.Text = HDLPF.IsNumStringMode(str, 0, 255);
            txtLoad1.SelectionStart = txtLoad1.Text.Length;
        }

        private void txtLoad2_Leave(object sender, EventArgs e)
        {
            string str = txtLoad2.Text;
            txtLoad2.Text = HDLPF.IsNumStringMode(str, 0, 255);
            txtLoad2.SelectionStart = txtLoad2.Text.Length;
        }

        private void txtLoad3_Leave(object sender, EventArgs e)
        {
            string str = txtLoad3.Text;
            txtLoad3.Text = HDLPF.IsNumStringMode(str, 0, 255);
            txtLoad3.SelectionStart = txtLoad3.Text.Length;
        }

        private void dgvTarget_SizeChanged(object sender, EventArgs e)
        {

        }

        private void dgvIR_SizeChanged(object sender, EventArgs e)
        {

        }

        private void btnHint_Click(object sender, EventArgs e)
        {
            Form frmtmp = new Form();
            frmtmp.FormBorderStyle = FormBorderStyle.FixedSingle;
            frmtmp.MinimizeBox = false;
            frmtmp.MaximizeBox = false;
            
            PictureBox pic=new PictureBox();
            pic.Image = HDL_Buspro_Setup_Tool.Properties.Resources.ir_副本_副本;
            frmtmp.Width = 520;
            frmtmp.Height = 540;
            frmtmp.StartPosition = FormStartPosition.CenterScreen;
            pic.Dock = DockStyle.Fill;
            frmtmp.Controls.Add(pic);
            frmtmp.ShowDialog();
        }

        private void groupBox7_Enter(object sender, EventArgs e)
        {

        }

    }
}
