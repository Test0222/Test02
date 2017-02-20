using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace HDL_Buspro_Setup_Tool
{
    public partial class FrmMHRCU : Form
    {
        [DllImport("user32.dll")]
        public static extern Boolean FlashWindow(IntPtr hwnd, Boolean bInvert);

        private string myDevName = null;
        private int mintIDIndex = -1;
        private byte SubNetID;
        private byte DevID;
        private int MyintDeviceType;
        NetworkInForm networkinfo;
        Connetion conn;
        private MHRCU oMHRCU = new MHRCU();
        private int MyActivePage = 0; //按页面上传下载
        private Point Position = new Point(0, 0);
        private TimeText txtONDelay = new TimeText(".");
        private TimeText txtCurtainOpenDelay = new TimeText(":");
        private TimeText txtCurtainCloseDelay = new TimeText(":");
        private TimeText txtCurtainRuntime = new TimeText(":");
        private bool isChangeScene = false;
        private bool MyBlnReading = false;
        private bool isRead = false;
        private int SlectedLogicIndex = 0;
        public FrmMHRCU()
        {
            InitializeComponent();
        }
        public FrmMHRCU(MHRCU myMHRCU, string strName, int intDIndex, int intDeviceType)
        {
            InitializeComponent();
            this.myDevName = strName;
            this.mintIDIndex = intDIndex;
            string strDevName = strName.Split('\\')[0].ToString();
            this.MyintDeviceType = intDeviceType;
            this.oMHRCU = myMHRCU;

            HDLSysPF.DisplayDeviceNameModeDescription(strName, MyintDeviceType, cboDevice, tbModel, tbDescription);

            SubNetID = Convert.ToByte(strDevName.Split('-')[0]);
            DevID = Convert.ToByte(strDevName.Split('-')[1]);
            tsl3.Text = strName;
        }

        void InitialFormCtrlsTextOrItems()
        {
            clL3.Items.Clear();
            clL3.Items.Add(CsConst.WholeTextsList[1775].sDisplayName);
            clL3.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00042", ""));
            CC2.Items.Clear();
            CC2.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99816", ""));
            CC2.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99817", ""));

            cbDelay2.Items.Clear();
            cbDelay3.Items.Clear();
            cbDelay4.Items.Clear();
            for (int i = 1; i <= 10; i++)
            {
                cbDelay2.Items.Add(i.ToString());
                cbDelay3.Items.Add(i.ToString());
                cbDelay4.Items.Add(i.ToString());
            }
            cbHeat.Items.Clear();
            cbHeat.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00390", ""));
            cbHeat.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00391", ""));
            cbHeat.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00392", ""));
            cbInterval.Items.Clear();
            for (int i = 3; i <= 60; i++)
                cbInterval.Items.Add(i.ToString() + "  S");
            cbOper.Items.Clear();
            cbOper.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99761", ""));
            cbOper.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99762", ""));

            cbDry1.Items.Clear();
            cbDry2.Items.Clear();
            cbDry1.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99616", ""));
            cbDry1.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99617", ""));
            cbDry2.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99616", ""));
            cbDry2.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99617", ""));
            cbUV1.Items.Clear();
            cbUV2.Items.Clear();
            cbUV1.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99614", ""));
            cbUV1.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99615", ""));
            cbUV2.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99614", ""));
            cbUV2.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99615", ""));
            cbLogicStatu.Items.Clear();
            cbLogicStatu.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99856", ""));
            cbLogicStatu.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99857", ""));

            chbList1.Items.Clear();
            chbList2.Items.Clear();
            for (int i = 0; i < 7; i++)
            {
                chbList1.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "0062" + i.ToString(), ""));
                chbList2.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "0062" + i.ToString(), ""));
            }
            HDLSysPF.setRS232ModeByDB(MyintDeviceType, cbMode);

            HDLSysPF.LoadDrynModeWithDifferentDeviceType(clK3, MyintDeviceType);
        }

        void SetCtrlsVisbleWithDifferentDeviceType()
        {
            toolStrip1.Visible = (CsConst.MyEditMode == 0);
            btn485.Visible = (MyintDeviceType == 3504);
            if (MyintDeviceType == 3503 || MyintDeviceType == 3504)
            {
                grp1.Visible = true;
                grp2.Visible = true;
                btnRefMode_Click(null, null);
            }
            if (MyintDeviceType != 3505)
            {
                tabControl.TabPages.Remove(tabHVAC);
            }
            else
            {
                tabControl.TabPages.Remove(tabNew);
            }
            if (MyintDeviceType != 3503)
            {
                if (MyintDeviceType != 3504)
                {
                    tabControl.TabPages.Remove(tabPage1);
                    tabControl.TabPages.Remove(tabPage2);
                    tabControl.TabPages.Remove(tabPage6);
                }
            }
            if (MyintDeviceType != 3504)
            {
                tabControl.TabPages.Remove(tabLogic);
                tabControl.TabPages.Remove(tabBUS485);
                tabControl.TabPages.Remove(tab485BUS);
            }
        }

        private void FrmMHRCU_Load(object sender, EventArgs e)
        {
            InitialFormCtrlsTextOrItems();
            
            loadNetWorkAndConn();

            this.tvZone.DrawNode += new DrawTreeNodeEventHandler(treeView_DrawNode);
        }

        private void treeView_DrawNode(object sender, DrawTreeNodeEventArgs e)
        {
           try
            {
                if ((e.State & TreeNodeStates.Selected) != 0)
                {

                    e.Graphics.FillRectangle(Brushes.Blue, e.Bounds.X - 5, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
                    Font nodeFont = e.Node.NodeFont;
                    if (nodeFont == null) nodeFont = ((TreeView)sender).Font;

                    // Draw the node text.
                    e.Graphics.DrawString(e.Node.Text, nodeFont, Brushes.White,
                        Rectangle.Inflate(new Rectangle(e.Bounds.X, e.Bounds.Y, e.Bounds.Width + 10, e.Bounds.Height), 5, -6));
                }
                else
                {
                    e.DrawDefault = false;
                    e.Graphics.DrawString(e.Node.Text, ((TreeView)sender).Font, Brushes.Black,
                        Rectangle.Inflate(new Rectangle(e.Bounds.X, e.Bounds.Y, e.Bounds.Width + 10, e.Bounds.Height), 5, -6));
                }

                /*if ((e.State & TreeNodeStates.Focused) != 0)
                {
                    using (Pen focusPen = new Pen(Color.Black))
                    {
                        focusPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
                        e.Graphics.DrawRectangle(focusPen, e.Bounds.X - 5, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
                    }
                }*/
            }
            catch
            {
            }
        }

        private void loadNetWorkAndConn()
        {
            groupBox1.Controls.Clear();
            networkinfo = new NetworkInForm(SubNetID, DevID, MyintDeviceType);
            groupBox1.Controls.Add(networkinfo);
            networkinfo.Dock = DockStyle.Fill;

            if (conn == null)
            {
                conn = new Connetion(SubNetID, DevID, MyintDeviceType);
                tabConn.Controls.Add(conn);
                conn.Dock = DockStyle.Fill;
            }
            conn.Dock = DockStyle.Fill;
            conn.BringToFront();
        }

        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CsConst.MyEditMode == 1)
            {
                if (tabControl.SelectedIndex == 0)
                {
                    loadNetWorkAndConn();
                    return;
                }
                if (tabControl.SelectedTab.Name == "tabZone") MyActivePage = 2;
                else if (tabControl.SelectedTab.Name == "tabChn") MyActivePage = 3;
                else if (tabControl.SelectedTab.Name == "tabSence") MyActivePage = 4;
                else if (tabControl.SelectedTab.Name == "tabCurtain") MyActivePage = 5;
                else if (tabControl.SelectedTab.Name == "tabDry") MyActivePage = 6;
                else if (tabControl.SelectedTab.Name == "tabSec") MyActivePage = 7;
                else if (tabControl.SelectedTab.Name == "tabNew") MyActivePage = 8;
                else if (tabControl.SelectedTab.Name == "tabHVAC") MyActivePage = 9;
                else if (tabControl.SelectedTab.Name == "tabPage1") MyActivePage = 10;
                else if (tabControl.SelectedTab.Name == "tabPage2") MyActivePage = 11;
                else if (tabControl.SelectedTab.Name == "tabPage6") MyActivePage = 12;
                else if (tabControl.SelectedTab.Name == "tabLogic") MyActivePage = 13;
                else if (tabControl.SelectedTab.Name == "tab485BUS") MyActivePage = 14;
                else if (tabControl.SelectedTab.Name == "tabBUS485") MyActivePage = 15;
                if ((oMHRCU.MyRead2UpFlags[MyActivePage - 1] == false) || (tabControl.SelectedTab.Name == "tabSence") ||
                    (tabControl.SelectedTab.Name == "tabNew"))
                {
                    tsbDown_Click(tsbDown, null);
                }
                else
                {
                    UpdateDisplayInformationAccordingly(null, null);
                }
            }
            Cursor.Current = Cursors.Default;
        }

        private void DisplayLogicInfo()
        {
            try
            {
                isRead = true;
                dgvLogic.Rows.Clear();
                for (int i = 0; i < oMHRCU.myLogic.Count; i++)
                {
                    MHRCU.RCULogic temp = oMHRCU.myLogic[i];
                    string strEnable = clL3.Items[0].ToString();
                    if (temp.Enable == 1) strEnable= clL3.Items[1].ToString();
                    object[] obj = new object[] { temp.ID.ToString(),strEnable, temp.Remark };
                    dgvLogic.Rows.Add(obj);
                }
            }
            catch
            {
            }
            for (int i = 0; i < dgvLogic.Rows.Count; i++)
            {
                dgvLogic.Rows[i].Selected = false;
            }
            isRead = false;
            btnReadSysTime_Click(null, null);
            if (dgvLogic.Rows.Count == 0) return;
            dgvLogic.Rows[SlectedLogicIndex].Selected = true;
            this.dgvLogic.CurrentCell = this.dgvLogic.Rows[SlectedLogicIndex].Cells[0];
            dgvLogic_CellClick(dgvLogic, new DataGridViewCellEventArgs(0, SlectedLogicIndex));
        }

        private void DisplayFilterInfo()
        {
            try
            {
                isRead = true;
                dgvFilter.Rows.Clear();
                if (oMHRCU == null) return;
                if (oMHRCU.myFilter == null) return;
                for (int i = 0; i < oMHRCU.myFilter.Count; i++)
                {
                    object[] obj = new object[] { (oMHRCU.myFilter[i][0]+1).ToString(),(oMHRCU.myFilter[i][1]==1),
                    oMHRCU.myFilter[i][2].ToString(),oMHRCU.myFilter[i][3].ToString()};
                    dgvFilter.Rows.Add(obj);
                }
            }
            catch
            {
            }
            isRead = false;
        }

        private void showHVACFunction()
        {
            try
            {
                if (oMHRCU == null) return;
                if (oMHRCU.myHVAC == null) return;
                isRead = true;
                chbTest.Checked = (oMHRCU.myHVAC.arayTest[0] == 1);
                for (int i = 1; i <= 6; i++)
                {
                    Button tmp = this.Controls.Find("btnTest" + i.ToString(), true)[0] as Button;
                    tmp.ImageIndex = 0;
                    tmp.Tag = 0;
                }
                if ((oMHRCU.myHVAC.arayTest[1] & 1) > 0)
                {
                    btnTest1.ImageIndex = 1;
                    btnTest1.Tag = 1;
                }
                if ((oMHRCU.myHVAC.arayTest[1] & (1 << 1)) > 0)
                {
                    btnTest2.ImageIndex = 1;
                    btnTest2.Tag = 1;
                }
                if ((oMHRCU.myHVAC.arayTest[1] & (1 << 2)) > 0)
                {
                    btnTest3.ImageIndex = 1;
                    btnTest3.Tag = 1;
                }

                if (oMHRCU.myHVAC.arayTest[2] == 1)
                {
                    btnTest4.ImageIndex = 1;
                    btnTest4.Tag = 1;
                }
                else if (oMHRCU.myHVAC.arayTest[2] == 2)
                {
                    btnTest5.ImageIndex = 1;
                    btnTest5.Tag = 1;
                }
                else if (oMHRCU.myHVAC.arayTest[2] == 3)
                {
                    btnTest6.ImageIndex = 1;
                    btnTest6.Tag = 1;
                }
                chbTest_CheckedChanged(null, null);

                cbDelay3.SelectedIndex = oMHRCU.myHVAC.arayTime[0] - 1;
                cbDelay4.SelectedIndex = oMHRCU.myHVAC.arayTime[1] - 1;

                if ((oMHRCU.myHVAC.arayTime[2] & 0x80) != 0x80)
                {
                    rbM.Checked = true;
                    rbM_CheckedChanged(null, null);
                    cbDelay1.SelectedIndex = oMHRCU.myHVAC.arayTime[2] - 1;
                }
                else
                {
                    rbS.Checked = true;
                    rbM_CheckedChanged(null, null);
                    cbDelay1.SelectedIndex = oMHRCU.myHVAC.arayTime[2] - 128 - 3;
                }
                cbDelay2.SelectedIndex = oMHRCU.myHVAC.arayTime[3] - 1;
                rbM_CheckedChanged(null, null);

                chbHost.Checked = (oMHRCU.myHVAC.arayHost[1] == 1);
                checkBox1_CheckedChanged(null, null);
                if (cbHost.SelectedIndex < 0) cbHost.SelectedIndex = 0;
                cbHost_SelectedIndexChanged(null, null);

                if (oMHRCU.myHVAC.arayProtect[0] <= 1)
                    cbOper.SelectedIndex = oMHRCU.myHVAC.arayProtect[0];
                if (5 <= oMHRCU.myHVAC.arayProtect[2] && oMHRCU.myHVAC.arayProtect[2] <= 15)
                    sbTempAdjust.Value = oMHRCU.myHVAC.arayProtect[2];
                sbTempAdjust_ValueChanged(null, null);
                chbSensor1.Checked = ((oMHRCU.myHVAC.arayProtect[3] & 1) == 1);
                chbSensor2.Checked = ((oMHRCU.myHVAC.arayProtect[3] & (1 << 1)) > 0);
                chbSensor3.Checked = ((oMHRCU.myHVAC.arayProtect[3] & (1 << 2)) > 0);
                chbSensor4.Checked = ((oMHRCU.myHVAC.arayProtect[3] & (1 << 3)) > 0);
                txtSubS1.Text = oMHRCU.myHVAC.arayProtect[4].ToString();
                txtDevS1.Text = oMHRCU.myHVAC.arayProtect[5].ToString();
                if (1 <= oMHRCU.myHVAC.arayProtect[6] && oMHRCU.myHVAC.arayProtect[6] <= 10)
                    cbChnS1.SelectedIndex = oMHRCU.myHVAC.arayProtect[6] - 1;
                else
                    cbChnS1.SelectedIndex = 0;
                txtSubS2.Text = oMHRCU.myHVAC.arayProtect[7].ToString();
                txtDevS2.Text = oMHRCU.myHVAC.arayProtect[8].ToString();
                if (1 <= oMHRCU.myHVAC.arayProtect[9] && oMHRCU.myHVAC.arayProtect[9] <= 10)
                    cbChnS2.SelectedIndex = oMHRCU.myHVAC.arayProtect[9] - 1;
                else
                    cbChnS2.SelectedIndex = 0;
                txtSubS3.Text = oMHRCU.myHVAC.arayProtect[10].ToString();
                txtDevS3.Text = oMHRCU.myHVAC.arayProtect[11].ToString();
                if (1 <= oMHRCU.myHVAC.arayProtect[12] && oMHRCU.myHVAC.arayProtect[12] <= 10)
                    cbChnS3.SelectedIndex = oMHRCU.myHVAC.arayProtect[12] - 1;
                else
                    cbChnS3.SelectedIndex = 0;
                txtSubS4.Text = oMHRCU.myHVAC.arayProtect[13].ToString();
                txtDevS4.Text = oMHRCU.myHVAC.arayProtect[14].ToString();
                if (1 <= oMHRCU.myHVAC.arayProtect[15] && oMHRCU.myHVAC.arayProtect[15] <= 10)
                    cbChnS4.SelectedIndex = oMHRCU.myHVAC.arayProtect[15] - 1;
                else
                    cbChnS4.SelectedIndex = 0;
                if (oMHRCU.myHVAC.arayProtect[16] == 0)
                    lbTempUnitValue.Text = "C";
                else if (oMHRCU.myHVAC.arayProtect[16] == 1)
                    lbTempUnitValue.Text = "F";
                lbCurentTempValue.Text = oMHRCU.myHVAC.arayProtect[17].ToString() + lbTempUnitValue.Text;

                chbHeat.Checked = (oMHRCU.myHVAC.arayProtect[23] == 1);
                chbHeat_CheckedChanged(null, null);
                if (oMHRCU.myHVAC.arayProtect[24] < cbHeat.Items.Count)
                    cbHeat.SelectedIndex = oMHRCU.myHVAC.arayProtect[24];
                txtHeatSub.Text = oMHRCU.myHVAC.arayProtect[26].ToString();
                txtHeatDev.Text = oMHRCU.myHVAC.arayProtect[27].ToString();
                txtHeatChn.Text = oMHRCU.myHVAC.arayProtect[28].ToString();
                txtHeat2.Text = oMHRCU.myHVAC.arayProtect[29].ToString();
                txtHeat1.Text = oMHRCU.myHVAC.arayProtect[30].ToString();
                if (oMHRCU.myHVAC.arayProtect[33] == 0)
                    lbHeat2.Text = "C";
                else if (oMHRCU.myHVAC.arayProtect[33] == 1)
                    lbHeat2.Text = "F";
                if (oMHRCU.myHVAC.arayProtect[34] > 0x80) 
                    lbHeat4.Text = (0x80 - oMHRCU.myHVAC.arayProtect[34]).ToString();
                else
                    lbHeat4.Text = oMHRCU.myHVAC.arayProtect[34].ToString();
                if (oMHRCU.myHVAC.arayProtect[35] == 1) lbHeat4.Text = CsConst.mstrINIDefault.IniReadValue("Public", "99764", "");
                else if (oMHRCU.myHVAC.arayProtect[35] == 0) lbHeat4.Text = CsConst.mstrINIDefault.IniReadValue("Public", "99763", "");

                if (3 <= oMHRCU.myHVAC.Interval && oMHRCU.myHVAC.Interval <= 60) cbInterval.SelectedIndex = oMHRCU.myHVAC.Interval - 3;
                else cbInterval.SelectedIndex = 0;
            }
            catch
            {
            }
            isRead = false;
            cbHost_SelectedIndexChanged(null, null);
        }

        private void showNewFunction()
        {
            pnStair.Controls.Clear();
            RelayExclusion temp = new RelayExclusion(this.MyintDeviceType, oMHRCU, myDevName);
            temp.Dock = DockStyle.Fill;
            pnStair.Controls.Add(temp);
        }

        private void tsbDown_Click(object sender, EventArgs e)
        {
            byte bytTag = Convert.ToByte(((ToolStripButton)sender).Tag.ToString());
            bool blnShowMsg = (CsConst.MyEditMode != 1);
            if (CsConst.MyEditMode == 1)
            {
                Cursor.Current = Cursors.WaitCursor;
                if (bytTag == 0)
                {
                    if (tabControl.SelectedIndex == 0)
                    {
                        loadNetWorkAndConn();
                        return;
                    }
                }
                else if (bytTag == 1)
                {
                    if (tabControl.SelectedIndex == 0)
                    {
                        conn.btnModify_Click(null, null);
                        return;
                    }
                }
                Cursor.Current = Cursors.Default;
            }
            if (HDLPF.UploadOrReadGoOn(this.Text, bytTag, blnShowMsg))
            {
                Cursor.Current = Cursors.WaitCursor;

                CsConst.MyUPload2DownLists = new List<byte[]>();

                string strName = myDevName.Split('\\')[0].ToString();
                byte bytSubID = byte.Parse(strName.Split('-')[0]);
                byte bytDevID = byte.Parse(strName.Split('-')[1]);
                int num1 = 1;
                int num2 = 1;
                switch (tabControl.SelectedTab.Name)
                {
                    case "tabPage1": num1 = Convert.ToInt32(txt232ToBusFrm.Text);
                        num2 = Convert.ToInt32(txt232ToBusTo.Text); break;
                    case "tabPage2":num1 = Convert.ToInt32(txtBusFrm.Text);
                        num2 = Convert.ToInt32(txtBusTo.Text); break;

                    case "tabLogic": if (dgvLogic.RowCount > 0 && dgvLogic.CurrentRow.Index >= 0) num1 = Convert.ToInt32(dgvLogic[0, SlectedLogicIndex].Value.ToString());
                                     if (dgvLogic.RowCount > 0 && dgvLogic.CurrentRow.Index>=0)num2 = Convert.ToInt32(dgvLogic[0, SlectedLogicIndex].Value.ToString());break;
                }
                byte[] ArayRelay = new byte[] { SubNetID, DevID, (byte)(MyintDeviceType / 256), 
                                                (byte)(MyintDeviceType % 256), (byte)MyActivePage ,
                                                (byte)(mintIDIndex / 256), (byte)(mintIDIndex % 256),
                                                Convert.ToByte(num1),Convert.ToByte(num2)};
                CsConst.MyUPload2DownLists.Add(ArayRelay);
                CsConst.MyUpload2Down = Convert.ToByte(((ToolStripButton)sender).Tag.ToString());
                FrmDownloadShow Frm = new FrmDownloadShow();
                Frm.FormClosing += new FormClosingEventHandler(UpdateDisplayInformationAccordingly);
                Frm.ShowDialog();
            }
        }
        void UpdateDisplayInformationAccordingly(object sender, FormClosingEventArgs e)
        {
            switch (tabControl.SelectedTab.Name)
            {
                case "tabZone": showZoneInfo(); break;
                case "tabChn": showChnInfo(); break;
                case "tabSence": showSenceInfo(); break;
                case "tabCurtain": showCurtainInfo(); break;
                case "tabDry": ShowKeysInformationPanel(); break;
                case "tabSec": DisplaySecuritySetup(); break;
                case "tabNew": showNewFunction(); break;
                case "tabHVAC": showHVACFunction(); break;
                case "tabPage1": DisplayRS232BUSInfo(); break;
                case "tabPage2": DisplayBUSRS232Info(); break;
                case "tab485BUS": DisplayRS485BUSInfo(); break;
                case "tabBUS485": DisplayBUSRS485Info(); break;
                case "tabPage6": DisplayFilterInfo(); break;
                case "tabLogic": DisplayLogicInfo(); break;
            }
            this.TopMost = true;
            this.WindowState = FormWindowState.Normal;
            this.Activate();
            this.TopMost = false;
        }

        private void DisplayBUSRS232Info()
        {
            try
            {
                isRead = true;
                dgvBUSto232.Rows.Clear();
                if (oMHRCU == null) return;
                if (oMHRCU.myBUS2RS == null) return;
                for (int i = 0; i < oMHRCU.myBUS2RS.Count; i++)
                {
                    MHRCU.BUS2RS temp = oMHRCU.myBUS2RS[i];
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
                    dgvBUSto232.Rows.Add(obj);
                }
                if (dgvBUSto232.RowCount > 0)
                    dgvBUSto232_CellClick(dgvBUSto232, new DataGridViewCellEventArgs(0, 0));
            }
            catch
            {
            }
            isRead = false;
        }

        private void DisplayRS232BUSInfo()
        {
            try
            {
                isRead = true;
                dgv232ToBus.Rows.Clear();
                if (oMHRCU == null) return;
                if (oMHRCU.myRS2BUS == null) return;
                for (int i = 0; i < oMHRCU.myRS2BUS.Count; i++)
                {
                    Rs232ToBus temp = oMHRCU.myRS2BUS[i];
                    string strEnable = CsConst.WholeTextsList[1775].sDisplayName;
                    if (temp.rs232Param.enable == 1) strEnable = CsConst.mstrINIDefault.IniReadValue("public", "00042", "");
                    string strType = CsConst.WholeTextsList[1775].sDisplayName;
                    string strCMD = "";
                    string strEnd = "NONE";
                    if (temp.rs232Param.type == 0)
                    {
                        strType = CsConst.mstrINIDefault.IniReadValue("public", "99838", "");
                        //int Count = temp.rs232Param.RSCMD[temp.TmpRS232.RSCMD.Length - 1];
                        //if (Count > 33) Count = 33;
                        //byte[] arayTmp = new byte[Count];
                        //Array.Copy(temp.rs232Param.RSCMD, 0, arayTmp, 0, Count);
                        //if (Count == 0)
                        //    strCMD = "";
                        //else
                        //    strCMD = HDLPF.Byte2String(arayTmp);
                        //if (arayTmp.Length > 2 && arayTmp[arayTmp.Length - 1] == 0x0A && arayTmp[arayTmp.Length - 2] == 0x0D) strEnd = "<CR+LF>";
                        //else if (arayTmp.Length > 1 && arayTmp[arayTmp.Length - 1] == 0x0D) strEnd = "<CR>";
                    }
                    else if (temp.rs232Param.type == 1)
                    {
                        //strType = CsConst.mstrINIDefault.IniReadValue("public", "99839", "");
                        //int Count = temp.TmpRS232.RSCMD[temp.TmpRS232.RSCMD.Length - 1];
                        //if (Count > 33) Count = 33;
                        //for (int j = 0; j < Count; j++)
                        //{
                        //    strCMD = strCMD + GlobalClass.AddLeftZero(temp.rs232Param.rsCmd[j].ToString("X"), 2) + " ";
                        //}
                    }
                    strCMD = strCMD.Trim();
                    object[] obj = new object[] { temp.ID.ToString(), temp.remark.ToString(), strEnable, strType, strCMD, strEnd };
                    dgv232ToBus.Rows.Add(obj);
                }
                if (dgv232ToBus.RowCount > 0)
                    dgv232ToBus_CellClick(dgv232ToBus, new DataGridViewCellEventArgs(0, 0));
            }
            catch
            {
            }
            isRead = false;
        }


        private void DisplaySecuritySetup()
        {
            dgvSec.Rows.Clear();
            if (oMHRCU.myKeySeu != null && oMHRCU.myKeySeu.Count != 0)
            {
                for (int i = 0; i < oMHRCU.myKeySeu.Count; i++)
                {
                    object[] obj = null;
                    UVCMD.SecurityInfo reader = oMHRCU.myKeySeu[i];
                    obj = new object[]{dgvSec.RowCount + 1, reader.strRemark, (reader.bytTerms==1),
                                    reader.strRemark,reader.bytSubID.ToString(), reader.bytDevID.ToString(),
                                    reader.bytArea.ToString(), false};
                    if (obj != null)
                        dgvSec.Rows.Add(obj);
                }
            }
        }



        void ShowKeysInformationPanel()
        {
            try
            {
                if (oMHRCU == null) return;
                if (oMHRCU.MSKeys == null) return;
                dgvKey.Rows.Clear();
                Byte ButtonID = 1;
                for (int i = 0; i < oMHRCU.MSKeys.Count; i++)
                {
                    MS04Key TempKey = oMHRCU.MSKeys[i];
                    String DryModeTmp = DryMode.ConvertorKeyModeToPublicModeGroup(TempKey.Mode);
                    int TmpMode = clK3.Items.IndexOf(DryModeTmp);

                    if (TempKey.Mode == 0 || TmpMode == -1)
                    {
                        String OnDelay = HDLPF.GetStringFromTime(TempKey.ONOFFDelay[0], ":");
                        String OffDelay = HDLPF.GetStringFromTime(TempKey.ONOFFDelay[1], ":");
                        object[] obj = new object[] { ButtonID, TempKey.Remark, DryModeTmp, OnDelay, OffDelay, false }; //strDimming, strSaveDimmingValue, strLED,strMutex,strDouble
                        dgvKey.Rows.Add(obj);
                    }
                    else
                    {
                        object[] obj = new object[] { ButtonID, TempKey.Remark, DryModeTmp, CsConst.mstrInvalid, CsConst.mstrInvalid, false }; //strDimming, strSaveDimmingValue, strLED,strMutex,strDouble
                        int RowID = dgvKey.Rows.Add(obj);
                        dgvKey[3, RowID].ReadOnly = true;
                        dgvKey[4, RowID].ReadOnly = true;
                    }
                    cbEnable.SetItemChecked(i, (TempKey.bytEnable == 1));
                    cbLock.SetItemChecked(i, (TempKey.bytReflag == 1));
                    ButtonID++;
                }
                sbMin.Value = oMHRCU.dimmerLow;
                sbMin_ValueChanged(sbMin, null);
            }
            catch
            {
            }
            MyBlnReading = false;
        }

        private void showCurtainInfo()
        {
            try
            {
                dgvCurtain.Rows.Clear();
                txtCurtainOpenDelay.Visible = false;
                txtCurtainCloseDelay.Visible = false;
                txtCurtainRuntime.Visible = false;
                if (oMHRCU == null) return;
                if (oMHRCU.Curtains == null) return;
                if (oMHRCU.Curtains.Count > 0)
                {
                    for (int i = 0; i < oMHRCU.Curtains.Count; i++)
                    {
                        MHRCU.Curtain temp = oMHRCU.Curtains[i];
                        string strEnable = CC2.Items[0].ToString();
                        if (temp.Enable) strEnable = CC2.Items[1].ToString();
                        object[] obj = new object[] { (i+1).ToString(), strEnable,HDLPF.GetStringFromTime(temp.StartDelay,":"),
                                     HDLPF.GetStringFromTime(temp.CloseDelay,":"),HDLPF.GetStringFromTime(temp.Runtime,":"),i.ToString()};
                        dgvCurtain.Rows.Add(obj);

                    }
                }
            }
            catch
            {
            }
        }

        private void showZoneInfo()
        {
            try
            {
                tvZone.Nodes.Clear();
                dgvZoneChn1.Rows.Clear();
                if (oMHRCU == null) return;
                if (oMHRCU.Areas == null) return;
                if (oMHRCU.Areas.Count > 0)
                {
                    foreach (MHRCU.Area oArea in oMHRCU.Areas)
                    {
                        TreeNode OND = tvZone.Nodes.Add(oArea.ID.ToString(), (tvZone.Nodes.Count + 1).ToString() + "-" + oArea.Remark, 1, 1);

                        if (oMHRCU.ChnList != null)
                        {
                            for (int intI = 0; intI < oMHRCU.ChnList.Count; intI++)
                            {
                                if (oMHRCU.ChnList[intI].intBelongs == (oArea.ID))
                                {
                                    string strRelay = "Relay";
                                    if (CsConst.iLanguageId == 1) strRelay = "继电器";
                                    string strDimmer = "Dimmer";
                                    if (CsConst.iLanguageId == 1) strDimmer = "调光";
                                    if (MyintDeviceType == 3501 || MyintDeviceType == 3502)
                                    {
                                        if (intI < 17 || intI == 21)
                                        {
                                            OND.Nodes.Add(null, (intI + 1).ToString() + "-" + strRelay + "-" + oMHRCU.ChnList[intI].Remark, 0, 0);
                                        }
                                        if (17 <= intI && intI != 21)
                                        {
                                            OND.Nodes.Add(null, (intI + 1).ToString() + "-" + strDimmer + "-" + oMHRCU.ChnList[intI].Remark, 0, 0);
                                        }
                                    }
                                    else if (HotelMixModuleDeviceType.RcuHas16RelaysIn22Chns.Contains(MyintDeviceType))
                                    {
                                        if (intI < 16)
                                        {
                                            OND.Nodes.Add(null, (intI + 1).ToString() + "-" + strRelay + "-" + oMHRCU.ChnList[intI].Remark, 0, 0);
                                        }
                                        if (16 <= intI)
                                        {
                                            OND.Nodes.Add(null, (intI + 1).ToString() + "-" + strDimmer + "-" + oMHRCU.ChnList[intI].Remark, 0, 0);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                if (oMHRCU.ChnList != null && oMHRCU.ChnList.Count > 0)
                {
                    for (int i = 0; i < oMHRCU.ChnList.Count; i++)
                    {
                        MHRCU.Channel chnTmp = oMHRCU.ChnList[i];
                        if (chnTmp.intBelongs == 0)
                        {
                            string strRelay = "Relay";
                            if (CsConst.iLanguageId == 1) strRelay = "继电器";
                            string strDimmer = "Dimmer";
                            if (CsConst.iLanguageId == 1) strDimmer = "调光";
                            string strChn = "";
                            if (MyintDeviceType == 3503 || MyintDeviceType == 3504)
                            {
                                if (i < 16)
                                {
                                    strChn = (i + 1).ToString() + "-" + strRelay;
                                    object[] obj = new object[] { strChn, chnTmp.Remark, false };
                                    dgvZoneChn1.Rows.Add(obj);
                                }
                                if (16 <= i)
                                {
                                    strChn = (i + 1).ToString() + "-" + strDimmer;
                                    object[] obj = new object[] { strChn, chnTmp.Remark, false };
                                    dgvZoneChn1.Rows.Add(obj);
                                }
                            }
                            else if (HotelMixModuleDeviceType.RcuHas16RelaysIn22Chns.Contains(MyintDeviceType))
                            {
                                if (i < 17 || i == 21)
                                {
                                    strChn = (i + 1).ToString() + "-" + strRelay;
                                    object[] obj = new object[] { strChn, chnTmp.Remark, false };
                                    dgvZoneChn1.Rows.Add(obj);
                                }
                                if (17 <= i && i != 21)
                                {
                                    strChn = (i + 1).ToString() + "-" + strDimmer;
                                    object[] obj = new object[] { strChn, chnTmp.Remark, false };
                                    dgvZoneChn1.Rows.Add(obj);
                                }
                            }
                        }
                    }
                }
                if (tvZone.Nodes.Count > 0) tvZone.SelectedNode = tvZone.Nodes[0];//选中
            }
            catch
            {
            }
        }

        private void showChnInfo()
        {
            try
            {
                DgChns.Rows.Clear();
                string strTmp = CsConst.mstrINIDefault.IniReadValue("Public", "00031", "");
                clChn3.Items.Clear(); clChn3.Items.AddRange(CsConst.LoadType);
                
                if (oMHRCU.ChnList != null)
                {
                    string strRelay = CsConst.mstrINIDefault.IniReadValue("Public", "99601", "");
                    string strDimmer = CsConst.mstrINIDefault.IniReadValue("Public", "99600", "");
                    string strChn = "";
                    for (int i = 0; i < oMHRCU.ChnList.Count; i++)
                    {
                        MHRCU.Channel chnTmp = oMHRCU.ChnList[i];
                        if (MyintDeviceType == 3501 || MyintDeviceType == 3502)
                        {
                            if (i < 17 || i == 21)
                            {
                                strChn = (i + 1).ToString() + "-" + strRelay;
                                object[] obj = new object[] { strChn, chnTmp.Remark, clChn3.Items[chnTmp.LoadType],
                                                    "N/A","N/A","N/A",HDLPF.GetStringFromTime(chnTmp.PowerOnDelay,"."), 
                                                    chnTmp.ProtectDealy,false};
                                DgChns.Rows.Add(obj);
                            }
                            if (17 <= i && i <= 20)
                            {
                                strChn = (i + 1).ToString() + "-" + strDimmer;
                                object[] obj = new object[] { strChn, chnTmp.Remark, clChn3.Items[chnTmp.LoadType],
                                                    chnTmp.MinValue.ToString(),chnTmp.MaxValue.ToString(),chnTmp.MaxLevel.ToString(),
                                                    "N/A","N/A",false};
                                DgChns.Rows.Add(obj);
                            }
                        }
                        else if (HotelMixModuleDeviceType.RcuHas16RelaysIn22Chns.Contains(MyintDeviceType))
                        {
                            if (i < 16)
                            {
                                strChn = (i + 1).ToString() + "-" + strRelay;
                                object[] obj = new object[] { strChn, chnTmp.Remark, clChn3.Items[chnTmp.LoadType],
                                                    "N/A","N/A","N/A",HDLPF.GetStringFromTime(chnTmp.PowerOnDelay,"."), 
                                                    chnTmp.ProtectDealy,false};
                                DgChns.Rows.Add(obj);
                            }
                            if (16 <= i)
                            {
                                strChn = (i + 1).ToString() + "-" + strDimmer;
                                object[] obj = new object[] { strChn, chnTmp.Remark, clChn3.Items[chnTmp.LoadType],
                                                    chnTmp.MinValue.ToString(),chnTmp.MaxValue.ToString(),chnTmp.MaxLevel.ToString(),
                                                    "N/A","N/A",false};
                                DgChns.Rows.Add(obj);
                            }
                        }
                    }
                }
            }
            catch
            {
            }
        }

        private void showSenceInfo()
        {
            try
            {
                if (oMHRCU == null) return;
                if (oMHRCU.Areas == null) return;
                cbSence.Items.Clear();
                panel11.Controls.Clear();
                if (oMHRCU.Areas.Count > 0)
                {

                    for (int i = 0; i < oMHRCU.Areas.Count; i++)
                    {
                        MHRCU.Area temp = oMHRCU.Areas[i];
                        cbSence.Items.Add((i + 1).ToString() + "-" + temp.Remark);
                    }
                    cbSence.SelectedIndex = 0;
                    cbSence_SelectedIndexChanged(null, null);
                }
            }
            catch
            {
            }
        }

        private void FrmMHRCU_Shown(object sender, EventArgs e)
        {
            SetCtrlsVisbleWithDifferentDeviceType();
            if (CsConst.MyEditMode == 0)
            {
                
            }
            else if (CsConst.MyEditMode == 1) //在线模式
            {
                tsbDown_Click(tsbDown, null);
                btnRefreshStatus_Click(null, null);
            }
        }

        private void tvZone_MouseDown(object sender, MouseEventArgs e)
        {
            TreeNode oNode = tvZone.GetNodeAt(e.X, e.Y);
            if (oNode == null) return;
            MyBlnReading = true;
            if (oNode.Level == 1) oNode = oNode.Parent;
            txtZoneRemark.Text = oNode.Text.Split('-')[1].ToString();
            MyBlnReading = false;
        }

        private void btnAddZone_Click(object sender, EventArgs e)
        {
            if (tvZone.Nodes.Count >= 22) return;
            if (dgvZoneChn1.RowCount <= 0) return;
            int ID = 1;
            int max = 1;
            int min = 1;
            if (oMHRCU.Areas.Count > 0)
            {
                foreach (MHRCU.Area a in oMHRCU.Areas)
                {
                    if (a.ID >= max)
                        max = a.ID;
                    if (a.ID < min && a.ID != 0)
                        min = a.ID;
                }
                if (min == max)
                {
                    if (min == 1)
                    {
                        ID = 2;
                    }
                    else
                    {
                        ID = 1;
                    }
                }
                else if(max > min)
                {
                    for (int i = min; i <= max; i++)
                    {
                        bool isSure = true;
                        for (int j = 0; j < oMHRCU.Areas.Count; j++)
                        {
                            if (oMHRCU.Areas[j].ID == i)
                            {
                                isSure = false;
                                break;
                            }
                        }
                        if (isSure)
                        {
                            ID = i;
                            break;
                        }
                    }
                    if (ID <= max)
                    {
                        ID = max + 1;
                    }
                }
            }
            else
            {
                ID = 1;
            }
            

            MHRCU.Area temp = new MHRCU.Area();
            temp.ID = Convert.ToByte(ID);
            temp.Remark = "";
            temp.Scen = new List<MHRCU.Scene>();
            for (int j = 0; j <= 12; j++)
            {
                MHRCU.Scene oSce = new MHRCU.Scene();
                oSce.ID = (byte)j;
                oSce.Remark = "Scene" + j.ToString();
                oSce.light = new byte[oMHRCU.ChnList.Count].ToList();
                oSce.Time = 0;
                temp.Scen.Add(oSce);
            }
            temp.bytDefaultSce = 0;
            oMHRCU.Areas.Add(temp);
            tvZone.Nodes.Add(temp.ID.ToString(), (tvZone.Nodes.Count+1).ToString()+ "-" + temp.Remark, 1, 1);
            lb22.Text = tvZone.Nodes.Count.ToString();
        }

        private void dgvZoneChn1_MouseDown(object sender, MouseEventArgs e)
        {
            if (tvZone.SelectedNode == null) return;

        }

        private void dgvZoneChn1_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            lb11.Text = dgvZoneChn1.RowCount.ToString();
        }

        private void dgvZoneChn1_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            lb11.Text = dgvZoneChn1.RowCount.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (tvZone.Nodes == null) return;
            if (oMHRCU == null) return;
            if (oMHRCU.Areas == null) return;
            if (tvZone.SelectedNode == null) return;
            TreeNode node = tvZone.SelectedNode;
            if (node.Level == 0)
            {
                int intAreaID = tvZone.SelectedNode.Index;
                for (int i = intAreaID + 1; i < oMHRCU.Areas.Count; i++)
                {
                    for (int j = 0; j < oMHRCU.ChnList.Count; j++)
                    {
                        if (oMHRCU.ChnList[j].intBelongs == oMHRCU.Areas[i].ID)
                        {
                            oMHRCU.ChnList[j].intBelongs = Convert.ToByte(oMHRCU.Areas[i].ID - 1);
                        }
                    }
                    oMHRCU.Areas[i].ID = Convert.ToByte(oMHRCU.Areas[i].ID - 1);
                }
                oMHRCU.Areas.RemoveAt(intAreaID);

                foreach (TreeNode Tmp in tvZone.SelectedNode.Nodes)
                {
                    DeleteNodeInAreaForm(Tmp.Text);
                }
                tvZone.SelectedNode.Remove();

                for (int i = 0; i < tvZone.Nodes.Count; i++)
                {
                    string str = tvZone.Nodes[i].Text.Split('-')[1];
                    tvZone.Nodes[i].Text = (i + 1).ToString() +"-"+ str;
                }
                lb22.Text = tvZone.Nodes.Count.ToString();
            }
        }
        void DeleteNodeInAreaForm(string strName)
        {
            int intIndex = int.Parse(strName.Split('-')[0].ToString());
            oMHRCU.ChnList[intIndex - 1].intBelongs = 0;
            resetChnsWaittingAllocation();
        }

        private void resetChnsWaittingAllocation()
        {
            dgvZoneChn1.Rows.Clear();
            for (int i = 0; i < oMHRCU.ChnList.Count; i++)
            {
                MHRCU.Channel chnTmp = oMHRCU.ChnList[i];
                if (chnTmp.intBelongs == 0)
                {
                    string strRelay = "Relay";
                    if (CsConst.iLanguageId == 1) strRelay = "继电器";
                    string strDimmer = "Dimmer";
                    if (CsConst.iLanguageId == 1) strDimmer = "调光";
                    string strChn = "";
                    if (MyintDeviceType == 3501 || MyintDeviceType == 3502)
                    {
                        if (i < 17 || i == 21)
                        {
                            strChn = (i + 1).ToString() + "-" + strRelay;
                            object[] obj = new object[] { strChn, chnTmp.Remark, false };
                            dgvZoneChn1.Rows.Add(obj);
                        }
                        if (17 <= i && i <= 20)
                        {
                            strChn = (i + 1).ToString() + "-" + strDimmer;
                            object[] obj = new object[] { strChn, chnTmp.Remark, false };
                            dgvZoneChn1.Rows.Add(obj);
                        }
                    }
                    else if (MyintDeviceType == 3503 || MyintDeviceType == 3504)
                    {
                        if (i < 16)
                        {
                            strChn = (i + 1).ToString() + "-" + strRelay;
                            object[] obj = new object[] { strChn, chnTmp.Remark, false };
                            dgvZoneChn1.Rows.Add(obj);
                        }
                        if (16 <= i && i <= 19)
                        {
                            strChn = (i + 1).ToString() + "-" + strDimmer;
                            object[] obj = new object[] { strChn, chnTmp.Remark, false };
                            dgvZoneChn1.Rows.Add(obj);
                        }
                    }
                }
            }
        }

        private void tvZone_ItemDrag(object sender, ItemDragEventArgs e)
        {
            DoDragDrop(e.Item, DragDropEffects.Move);
        }

        private void tvZone_DragDrop(object sender, DragEventArgs e)
        {
            TreeNode myNode = null;
            if (e.Data.GetDataPresent(typeof(TreeNode)))
            {
                myNode = (TreeNode)(e.Data.GetData(typeof(TreeNode)));
            }
            else
            {
                MessageBox.Show("error");
            }

            Position.X = e.X;
            Position.Y = e.Y;
            Position = ((TreeView)sender).PointToClient(Position);
            TreeNode DropNode = ((TreeView)sender).GetNodeAt(Position);
            // 1.目标节点不是空。2.目标节点不是被拖拽接点的字节点。3.目标节点不是被拖拽节点本身
            if (DropNode != null && DropNode.Parent != myNode && DropNode != myNode && DropNode.Level != myNode.Level)
            {
                TreeNode DragNode = myNode;
                // 将被拖拽节点从原来位置删除。
                myNode.Remove();
                // 在目标节点下增加被拖拽节点
                DropNode.Nodes.Add(DragNode);
                int intLoadID = int.Parse(DragNode.Text.Split('-')[0].ToString());
                oMHRCU.ChnList[intLoadID - 1].intBelongs = byte.Parse(DropNode.Name.ToString());
            }
            if (DropNode == null) return;
            // 如果目标节点不存在，即拖拽的位置不存在节点，那么就将被拖拽节点放在根节点之
        }

        private void tvZone_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(TreeNode)))
            {
                e.Effect = DragDropEffects.Move;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void txtZoneRemark_TextChanged(object sender, EventArgs e)
        {
            if (tvZone.Nodes == null) return;
            if (oMHRCU == null) return;
            if (oMHRCU.Areas == null) return;
            if (tvZone.SelectedNode == null) return;
            if (MyBlnReading) return;
            TreeNode node = tvZone.SelectedNode;
            if (node.Level == 0)
            {
                oMHRCU.Areas[node.Index].Remark = txtZoneRemark.Text.Trim();
                node.Text = (node.Index + 1).ToString() + "-" + txtZoneRemark.Text.Trim();
            }
            else if(node.Level==1)
            {
                node=node.Parent;
                oMHRCU.Areas[node.Index].Remark = txtZoneRemark.Text.Trim();
                node.Text = (node.Index + 1).ToString() + "-" + txtZoneRemark.Text.Trim();
            }
        }

        private void DgChns_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (oMHRCU == null) return;
                if (oMHRCU.ChnList == null) return;
                if ((e.RowIndex == -1) || (e.ColumnIndex == -1)) return;
                if (DgChns[e.ColumnIndex, e.RowIndex].Value == null) DgChns[e.ColumnIndex, e.RowIndex].Value = "";

                for (int i = 0; i < DgChns.SelectedRows.Count; i++)
                {
                    DgChns.SelectedRows[i].Cells[e.ColumnIndex].Value = DgChns[e.ColumnIndex, e.RowIndex].Value.ToString();

                    string strTmp = "";
                    switch (e.ColumnIndex)
                    {
                        case 1:
                            strTmp = DgChns[1, DgChns.SelectedRows[i].Index].Value.ToString();
                            DgChns[1, DgChns.SelectedRows[i].Index].Value = HDLPF.IsRightStringMode(strTmp);

                            oMHRCU.ChnList[DgChns.SelectedRows[i].Index].Remark = DgChns[1, DgChns.SelectedRows[i].Index].Value.ToString();
                            break;
                        case 2:
                            oMHRCU.ChnList[DgChns.SelectedRows[i].Index].LoadType = clChn3.Items.IndexOf(DgChns[2, DgChns.SelectedRows[i].Index].Value.ToString());
                            break;
                        case 3:
                            strTmp = DgChns[3, DgChns.SelectedRows[i].Index].Value.ToString();
                            //DgChns[3, DgChns.SelectedRows[i].Index].Value = HDLPF.IsNumStringMode(strTmp, 0, 100);
                            oMHRCU.ChnList[DgChns.SelectedRows[i].Index].MinValue = int.Parse(HDLPF.IsNumStringMode(strTmp, 0, 100)/*DgChns[3, DgChns.SelectedRows[i].Index].Value.ToString()*/);
                            break;
                        case 4:
                            strTmp = DgChns[4, DgChns.SelectedRows[i].Index].Value.ToString();
                            //DgChns[4, DgChns.SelectedRows[i].Index].Value = HDLPF.IsNumStringMode(strTmp, 0, 100);
                            oMHRCU.ChnList[DgChns.SelectedRows[i].Index].MaxValue = int.Parse(HDLPF.IsNumStringMode(strTmp, 0, 100)/*DgChns[4, DgChns.SelectedRows[i].Index].Value.ToString()*/);
                            break;
                        case 5:
                            strTmp = DgChns[5, DgChns.SelectedRows[i].Index].Value.ToString();
                            //DgChns[5, DgChns.SelectedRows[i].Index].Value = HDLPF.IsNumStringMode(strTmp, 0, 100);
                            oMHRCU.ChnList[DgChns.SelectedRows[i].Index].MaxLevel = int.Parse(HDLPF.IsNumStringMode(strTmp, 0, 100)/*DgChns[5, DgChns.SelectedRows[i].Index].Value.ToString()*/);
                            break;
                        case 6:
                            DgChns[6, DgChns.SelectedRows[i].Index].Value = HDLPF.GetStringFromTime(int.Parse(txtONDelay.Text.ToString()), ".");
                            oMHRCU.ChnList[DgChns.SelectedRows[i].Index].PowerOnDelay = int.Parse(HDLPF.GetTimeFromString(DgChns[6, DgChns.SelectedRows[i].Index].Value.ToString(), '.'));
                            break;
                        case 7:
                            strTmp = DgChns[7, DgChns.SelectedRows[i].Index].Value.ToString();
                            DgChns[7, DgChns.SelectedRows[i].Index].Value = HDLPF.IsNumStringMode(strTmp, 0, 60);
                            oMHRCU.ChnList[DgChns.SelectedRows[i].Index].ProtectDealy = byte.Parse((HDLPF.IsNumStringMode(DgChns[7, DgChns.SelectedRows[i].Index].Value.ToString(), 0, 60)));
                            break;
                        case 8:
                            string strName = myDevName.Split('\\')[0].ToString();
                            byte bytSubID = byte.Parse(strName.Split('-')[0]);
                            byte bytDevID = byte.Parse(strName.Split('-')[1]);

                            byte[] bytTmp = new byte[4];
                            bytTmp[0] = (byte)(DgChns.SelectedRows[i].Index + 1);
                            bytTmp[2] = 0;
                            bytTmp[3] = 0;

                            if (DgChns[8, DgChns.SelectedRows[i].Index].Value.ToString().ToLower() == "true")
                                bytTmp[1] = 100;
                            else bytTmp[1] = 0;

                            Cursor.Current = Cursors.WaitCursor;
                            if (CsConst.mySends.AddBufToSndList(bytTmp, 0x0031, bytSubID, bytDevID, false, true, true,false) == true)
                            {
                                
                                HDLUDP.TimeBetwnNext(20);
                            }

                            Cursor.Current = Cursors.Default;
                            break;
                    }
                }

                if (e.ColumnIndex == 1)
                {
                    string strTmp = DgChns[1, e.RowIndex].Value.ToString();
                    DgChns[1, e.RowIndex].Value = HDLPF.IsRightStringMode(strTmp);
                    oMHRCU.ChnList[e.RowIndex].Remark = DgChns[1, e.RowIndex].Value.ToString();
                }
                else if (e.ColumnIndex == 2)
                {
                    oMHRCU.ChnList[e.RowIndex].LoadType = clChn3.Items.IndexOf(DgChns[2, e.RowIndex].Value.ToString());
                }
                else if (e.ColumnIndex == 3)
                {
                    string strTmp = DgChns[3, e.RowIndex].Value.ToString();
                    oMHRCU.ChnList[e.RowIndex].MinValue = int.Parse(HDLPF.IsNumStringMode(strTmp, 0, 100));
                }
                else if (e.ColumnIndex == 4)
                {
                    string strTmp = DgChns[4, e.RowIndex].Value.ToString();
                    oMHRCU.ChnList[e.RowIndex].MaxValue = int.Parse(HDLPF.IsNumStringMode(strTmp, 0, 100));
                }
                else if (e.ColumnIndex == 5)
                {
                    string strTmp = DgChns[5, e.RowIndex].Value.ToString();
                    oMHRCU.ChnList[e.RowIndex].MaxLevel = int.Parse(HDLPF.IsNumStringMode(strTmp, 0, 100));
                }
                else if (e.ColumnIndex == 6)
                {
                    DgChns[6, e.RowIndex].Value = HDLPF.GetStringFromTime(int.Parse(txtONDelay.Text.ToString()), ".");
                    oMHRCU.ChnList[e.RowIndex].PowerOnDelay = int.Parse(HDLPF.GetTimeFromString(DgChns[6, e.RowIndex].Value.ToString(), '.'));
                }
                else if (e.ColumnIndex == 7)
                {
                    string strTmp = DgChns[7, e.RowIndex].Value.ToString();
                    DgChns[7, e.RowIndex].Value = HDLPF.IsNumStringMode(strTmp, 0, 60);
                    oMHRCU.ChnList[e.RowIndex].ProtectDealy = byte.Parse((HDLPF.IsNumStringMode(DgChns[7, e.RowIndex].Value.ToString(), 0, 60)));
                }
            }
            catch
            {
            }
        }



        private void DgChns_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (oMHRCU == null) return;
            if (oMHRCU.ChnList == null) return;
            if ((e.RowIndex == -1) || (e.ColumnIndex == -1)) return;
            if (txtONDelay.Visible == true) DgChns[6, e.RowIndex].Value = HDLPF.GetStringFromTime(int.Parse(txtONDelay.Text.ToString()), ".");
        }

        private void DgChns_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            DgChns.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void DgChns_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (e.ColumnIndex == 6)
            {
                DgChns.Controls.Add(txtONDelay);
                string strTmp = HDLPF.GetTimeFromString(DgChns[6, e.RowIndex].Value.ToString(), '.');
                txtONDelay.Text = strTmp;
                txtONDelay.Show();
                txtONDelay.Visible = true;
                Rectangle rect = DgChns.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true);
                txtONDelay.Size = rect.Size;
                txtONDelay.Top = rect.Top;
                txtONDelay.Left = rect.Left;
                txtONDelay.TextChanged += new EventHandler(txtONDelay_TextChanged);
            }
            else
            {
                txtONDelay.Visible = false;
            }
        }
        void txtONDelay_TextChanged(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            if (oMHRCU == null) return;
            if (oMHRCU.ChnList == null) return;

            if ((DgChns.CurrentCell.RowIndex == -1) || (DgChns.CurrentCell.ColumnIndex == -1)) return;
            if (txtONDelay.Visible)
            {
                DgChns[6, DgChns.CurrentCell.RowIndex].Value = HDLPF.GetStringFromTime(int.Parse(txtONDelay.Text.ToString()), ".");
                // DgMode_CellValueChanged(DgMode, e);
            }
        }

        private void BtnZoneRemark_Click(object sender, EventArgs e)
        {
            if (tvZone.Nodes == null) return;
            if (oMHRCU == null) return;
            if (oMHRCU.Areas == null) return;
            if (tvZone.SelectedNode == null) return;
            TreeNode node = tvZone.SelectedNode;
            if (node.Level == 0)
            {
                node.Text =  txtZoneRemark.Text.Trim();
                oMHRCU.Areas[node.Index].Remark = txtZoneRemark.Text.Trim();
            }
        }

        private void txtScene_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar) || e.KeyChar == (char)8)
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void txtScene_TextChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            if (cbSence.Items.Count < 0 || cbSence.SelectedIndex < 0) return;
            string str = txtScene.Text;
            if (GlobalClass.IsNumeric(str))
            {
                MHRCU.Area temp = oMHRCU.Areas[cbSence.SelectedIndex];
                str = HDLPF.IsNumStringMode(str, 0, 254);
                temp.bytDefaultSce = byte.Parse(str);
            }
        }

        private void rb1_CheckedChanged(object sender, EventArgs e)
        {
            if (cbSence.Items.Count < 0 || cbSence.SelectedIndex < 0) return;
            if (rb1.Checked)
            {
                MHRCU.Area temp = oMHRCU.Areas[cbSence.SelectedIndex];
                txtScene.Visible = false;
                temp.bytDefaultSce = 255;
            }
            else if (rb2.Checked) txtScene.Visible = true;
        }

        private void cbSence_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (oMHRCU == null) return;
            if (oMHRCU.Areas == null) return;
            isRead = true;
            MHRCU.Area temp = oMHRCU.Areas[cbSence.SelectedIndex];
            if (temp.bytDefaultSce == 255)
            {
                rb1.Checked = true;
            }
            else
            {
                rb2.Checked = true;
                txtScene.Text = temp.bytDefaultSce.ToString();
            }
            cbChooseScene.Items.Clear();
            for (int i = 0; i < temp.Scen.Count; i++)
            {
                cbChooseScene.Items.Add((i).ToString() + "-" + temp.Scen[i].Remark.ToString());
            }
            if (cbChooseScene.Items.Count > 0)
            {
                cbChooseScene.SelectedIndex = 0;
                cbChooseScene_SelectedIndexChanged(null, null);
            }
            isRead = false;
        }

        private void FrmMHRCU_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void cbChooseScene_SelectedIndexChanged(object sender, EventArgs e)
        {
            isChangeScene = true;
            txtSceneRemark.Text = cbChooseScene.Text.Split('-')[1].ToString();
            MHRCU.Scene temp = oMHRCU.Areas[cbSence.SelectedIndex].Scen[cbChooseScene.SelectedIndex];
            SceneRuntime.Text = temp.Time.ToString();
            List<ChnSenceInfomation> Scene = new List<ChnSenceInfomation>();
            panel11.Controls.Clear();
            for (int i = 0; i < oMHRCU.ChnList.Count; i++)
            {
                if (oMHRCU.ChnList[i].intBelongs == cbSence.SelectedIndex+1)
                {
                    ChnSenceInfomation tmp = new ChnSenceInfomation();
                    tmp.ID = Convert.ToByte(oMHRCU.ChnList[i].ID);
                    tmp.light = oMHRCU.Areas[cbSence.SelectedIndex].Scen[cbChooseScene.SelectedIndex].light[i];
                    tmp.Remark = oMHRCU.ChnList[i].Remark;
                    if (MyintDeviceType == 3501 || MyintDeviceType == 3502)
                    {
                        if (0 <= i && i <= 16) tmp.type = 2;
                        if (17 <= i && i <= 20) tmp.type = 1;
                        if (i == 21) tmp.type = 2;
                    }
                    else if (MyintDeviceType == 3503 || MyintDeviceType == 3504)
                    {
                        if (0 <= i && i <= 15) tmp.type = 2;
                        if (16 <= i && i <= 19) tmp.type = 1;
                    }
                    if (oMHRCU.Curtains.Count == 4)
                    {
                        if (0 <= i && i <= 1)
                        {
                            if (oMHRCU.Curtains[0].Enable) tmp.isCurtainChannel = true;
                        }
                        else if (2 <= i && i <= 3)
                        {
                            if (oMHRCU.Curtains[1].Enable) tmp.isCurtainChannel = true;
                        }
                        else if (4 <= i && i <= 5)
                        {
                            if (oMHRCU.Curtains[2].Enable) tmp.isCurtainChannel = true;
                        }
                        else if (6 <= i && i <= 7)
                        {
                            if (oMHRCU.Curtains[3].Enable) tmp.isCurtainChannel = true;
                        }
                    }
                    if (cbChooseScene.SelectedIndex == 0) tmp.light = 0;
                    Scene.Add(tmp); 
                }
            }
            SenceDesign sencedesign = new SenceDesign(panel11, Scene, MyintDeviceType,cbSence.SelectedIndex,cbChooseScene.SelectedIndex,oMHRCU,myDevName);
            isChangeScene = false;                                
        }

        private void txtSceneRemark_TextChanged(object sender, EventArgs e)
        {
            if (cbSence.Items.Count < 0 || cbSence.SelectedIndex < 0) return;
            cbChooseScene.Items[cbChooseScene.SelectedIndex] = (cbChooseScene.SelectedIndex).ToString() + "-" + txtSceneRemark.Text;
            oMHRCU.Areas[cbSence.SelectedIndex].Scen[cbChooseScene.SelectedIndex].Remark = txtSceneRemark.Text.Trim();
        }

        private void SceneRuntime_TextChanged(object sender, EventArgs e)
        {
            if (cbSence.Items.Count < 0 || cbSence.SelectedIndex < 0) return;
            if (isChangeScene) return;
            string str = "";
            str = SceneRuntime.Text;
            oMHRCU.Areas[cbSence.SelectedIndex].Scen[cbChooseScene.SelectedIndex].Time = Convert.ToInt32(str);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (dgvCurtain.RowCount >= 4) return;
            if (oMHRCU.Curtains.Count ==0)
            {
                for (int intI = 0; intI < 4; intI++)
                {
                    MHRCU.Curtain temp = new MHRCU.Curtain();
                    temp.ID = intI + 1;
                    oMHRCU.Curtains.Add(temp);
                }
            }
            dgvCurtain.Rows.Clear();
            for (int i = 0; i < 4; i++)
            {
                if (!oMHRCU.Curtains[i].Enable)
                {
                    oMHRCU.Curtains[i].Enable = true;
                    break;
                }
            }
            for (int i = 0; i < 4; i++)
            {
                if (oMHRCU.Curtains[i].Enable)
                {
                    object[] obj = new object[] { (i + 1).ToString(), HDLPF.GetStringFromTime(oMHRCU.Curtains[i].StartDelay,":"),
                          HDLPF.GetStringFromTime(oMHRCU.Curtains[i].CloseDelay,":"),HDLPF.GetStringFromTime(oMHRCU.Curtains[i].Runtime,":"), i.ToString() };
                    dgvCurtain.Rows.Add(obj);
                }
            }
        }

        private void dgvCurtain_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            txtCurtainOpenDelay.Visible = false;
            txtCurtainCloseDelay.Visible = false;
            txtCurtainRuntime.Visible = false;
            if (e.ColumnIndex == 2)
            {
                dgvCurtain.Controls.Add(txtCurtainOpenDelay);
                string strTmp = HDLPF.GetTimeFromString(dgvCurtain[2, e.RowIndex].Value.ToString(), ':');
                txtCurtainOpenDelay.Text = strTmp;
                txtCurtainOpenDelay.Show();
                txtCurtainOpenDelay.Visible = true;
                Rectangle rect = dgvCurtain.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true);
                txtCurtainOpenDelay.Size = rect.Size;
                txtCurtainOpenDelay.Top = rect.Top;
                txtCurtainOpenDelay.Left = rect.Left;
                txtCurtainOpenDelay.TextChanged += new EventHandler(txtCurtainOpenDelay_TextChangedForCurtain);
            }
            else if (e.ColumnIndex == 3)
            {
                dgvCurtain.Controls.Add(txtCurtainCloseDelay);
                string strTmp = HDLPF.GetTimeFromString(dgvCurtain[3, e.RowIndex].Value.ToString(), ':');
                txtCurtainCloseDelay.Text = strTmp;
                txtCurtainCloseDelay.Show();
                txtCurtainCloseDelay.Visible = true;
                Rectangle rect = dgvCurtain.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true);
                txtCurtainCloseDelay.Size = rect.Size;
                txtCurtainCloseDelay.Top = rect.Top;
                txtCurtainCloseDelay.Left = rect.Left;
                txtCurtainCloseDelay.TextChanged += new EventHandler(txtCurtainCloseDelay_TextChangedForCurtain);
            }
            else if(e.ColumnIndex==4)
            {
                dgvCurtain.Controls.Add(txtCurtainRuntime);
                string strTmp = HDLPF.GetTimeFromString(dgvCurtain[4, e.RowIndex].Value.ToString(), ':');
                txtCurtainRuntime.Text = strTmp;
                txtCurtainRuntime.Show();
                txtCurtainRuntime.Visible = true;
                Rectangle rect = dgvCurtain.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true);
                txtCurtainRuntime.Size = rect.Size;
                txtCurtainRuntime.Top = rect.Top;
                txtCurtainRuntime.Left = rect.Left;
                txtCurtainRuntime.TextChanged += new EventHandler(txtCurtainRuntime_TextChangedForCurtain);
            }
        }

        void txtCurtainOpenDelay_TextChangedForCurtain(object sender, EventArgs e)
        {
            if (oMHRCU == null) return;
            if (oMHRCU.Curtains == null) return;

            if ((dgvCurtain.CurrentCell.RowIndex == -1) || (dgvCurtain.CurrentCell.ColumnIndex == -1)) return;
            if (txtCurtainOpenDelay.Visible)
            {
                dgvCurtain[2, dgvCurtain.CurrentCell.RowIndex].Value = HDLPF.GetStringFromTime(int.Parse(txtCurtainOpenDelay.Text.ToString()), ":");
            }
        }
        void txtCurtainCloseDelay_TextChangedForCurtain(object sender, EventArgs e)
        {
            if (oMHRCU == null) return;
            if (oMHRCU.Curtains == null) return;

            if ((dgvCurtain.CurrentCell.RowIndex == -1) || (dgvCurtain.CurrentCell.ColumnIndex == -1)) return;
            if (txtCurtainCloseDelay.Visible)
            {
                dgvCurtain[3, dgvCurtain.CurrentCell.RowIndex].Value = HDLPF.GetStringFromTime(int.Parse(txtCurtainCloseDelay.Text.ToString()), ":");
            }
        }
        void txtCurtainRuntime_TextChangedForCurtain(object sender, EventArgs e)
        {
            if (oMHRCU == null) return;
            if (oMHRCU.Curtains == null) return;

            if ((dgvCurtain.CurrentCell.RowIndex == -1) || (dgvCurtain.CurrentCell.ColumnIndex == -1)) return;
            if (txtCurtainRuntime.Visible)
            {
                dgvCurtain[4,dgvCurtain.CurrentCell.RowIndex].Value = HDLPF.GetStringFromTime(int.Parse(txtCurtainRuntime.Text.ToString()), ":");
            }
        }

        private void dgvCurtain_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (oMHRCU == null) return;
            if (oMHRCU.Curtains == null) return;
            if ((e.RowIndex == -1) || (e.ColumnIndex == -1)) return;
            if (txtCurtainOpenDelay.Visible == true) dgvCurtain[2, e.RowIndex].Value = HDLPF.GetStringFromTime(int.Parse(txtCurtainOpenDelay.Text.ToString()), ":");
            if (txtCurtainCloseDelay.Visible == true) dgvCurtain[3, e.RowIndex].Value = HDLPF.GetStringFromTime(int.Parse(txtCurtainCloseDelay.Text.ToString()), ":");
            if (txtCurtainRuntime.Visible == true) dgvCurtain[4, e.RowIndex].Value = HDLPF.GetStringFromTime(int.Parse(txtCurtainRuntime.Text.ToString()), ":");
        }

        private void dgvCurtain_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            dgvCurtain.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void dgvCurtain_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (oMHRCU == null) return;
                if (oMHRCU.Curtains == null) return;
                if ((e.RowIndex == -1) || (e.ColumnIndex == -1)) return;
                if (dgvCurtain[e.ColumnIndex, e.RowIndex].Value == null) dgvCurtain[e.ColumnIndex, e.RowIndex].Value = "";

                for (int i = 0; i < dgvCurtain.SelectedRows.Count; i++)
                {
                    dgvCurtain.SelectedRows[i].Cells[e.ColumnIndex].Value = dgvCurtain[e.ColumnIndex, e.RowIndex].Value.ToString();
                    switch (e.ColumnIndex)
                    {
                        case 1:
                            if (CC2.Items.IndexOf(dgvCurtain[1, dgvCurtain.SelectedRows[i].Index].Value.ToString()) == 1)
                                oMHRCU.Curtains[dgvCurtain.SelectedRows[i].Index].Enable = true;
                            else
                                oMHRCU.Curtains[dgvCurtain.SelectedRows[i].Index].Enable = false;
                            break;
                        case 2:
                            dgvCurtain[2, dgvCurtain.SelectedRows[i].Index].Value = HDLPF.GetStringFromTime(int.Parse(txtCurtainOpenDelay.Text.ToString()), ":");
                            oMHRCU.Curtains[dgvCurtain.SelectedRows[i].Index].StartDelay = int.Parse(HDLPF.GetTimeFromString(dgvCurtain[2, dgvCurtain.SelectedRows[i].Index].Value.ToString(), ':'));
                            break;
                        case 3:
                            dgvCurtain[3, dgvCurtain.SelectedRows[i].Index].Value = HDLPF.GetStringFromTime(int.Parse(txtCurtainCloseDelay.Text.ToString()), ":");
                            oMHRCU.Curtains[dgvCurtain.SelectedRows[i].Index].CloseDelay = int.Parse(HDLPF.GetTimeFromString(dgvCurtain[3, dgvCurtain.SelectedRows[i].Index].Value.ToString(), ':'));
                            break;
                        case 4:
                            dgvCurtain[4, dgvCurtain.SelectedRows[i].Index].Value = HDLPF.GetStringFromTime(int.Parse(txtCurtainRuntime.Text.ToString()), ":");
                            oMHRCU.Curtains[dgvCurtain.SelectedRows[i].Index].Runtime = int.Parse(HDLPF.GetTimeFromString(dgvCurtain[4, dgvCurtain.SelectedRows[i].Index].Value.ToString(), ':'));
                            break;
                    }
                }
            }
            catch
            {
            }
        }

        private void dgvSec_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (oMHRCU == null) return;
                if (oMHRCU.myKeySeu == null) return;
                if (e.RowIndex == -1) return;
                if (e.ColumnIndex == -1) return;
                if (dgvSec[e.ColumnIndex, e.RowIndex].Value == null) dgvSec[e.ColumnIndex, e.RowIndex].Value = "";
                for (int i = 0; i < dgvSec.SelectedRows.Count; i++)
                {
                    UVCMD.SecurityInfo tempfire = oMHRCU.myKeySeu[dgvSec.SelectedRows[i].Index];
                    dgvSec.SelectedRows[i].Cells[e.ColumnIndex].Value = dgvSec[e.ColumnIndex, e.RowIndex].Value.ToString();
                    switch (e.ColumnIndex)
                    {
                        case 2:
                            tempfire.bytTerms = Convert.ToByte(dgvSec[2, dgvSec.SelectedRows[i].Index].Value.ToString().ToLower() == "true");
                            break;
                        case 3:
                            string strTmp = dgvSec[3, dgvSec.SelectedRows[i].Index].Value.ToString();
                            tempfire.strRemark = strTmp;
                            break;
                        case 4:
                            strTmp = dgvSec[4, dgvSec.SelectedRows[i].Index].Value.ToString();
                            dgvSec[4, dgvSec.SelectedRows[i].Index].Value = HDLPF.IsNumStringMode(strTmp, 0, 254);
                            tempfire.bytSubID = byte.Parse(dgvSec[4, dgvSec.SelectedRows[i].Index].Value.ToString());
                            break;
                        case 5:
                            strTmp = dgvSec[5, dgvSec.SelectedRows[i].Index].Value.ToString();
                            dgvSec[5, dgvSec.SelectedRows[i].Index].Value = HDLPF.IsNumStringMode(strTmp, 0, 254);
                            tempfire.bytDevID = byte.Parse(dgvSec[5, dgvSec.SelectedRows[i].Index].Value.ToString());
                            break;
                        case 6:
                            strTmp = dgvSec[6, dgvSec.SelectedRows[i].Index].Value.ToString();
                            dgvSec[6, dgvSec.SelectedRows[i].Index].Value = HDLPF.IsNumStringMode(strTmp, 0, 254);
                            tempfire.bytArea = byte.Parse(dgvSec[6, dgvSec.SelectedRows[i].Index].Value.ToString());
                            break;
                        case 7:
                            if (oMHRCU.MSKeys[dgvSec.SelectedRows[i].Index].AvoidCut == null)
                                oMHRCU.MSKeys[dgvSec.SelectedRows[i].Index].AvoidCut = new byte[10];
                            oMHRCU.MSKeys[dgvSec.SelectedRows[i].Index].AvoidCut[0] = Convert.ToByte(dgvSec[2, dgvSec.SelectedRows[i].Index].Value.ToString().ToLower() == "true");
                            break;
                    }
                }
                if (e.ColumnIndex == 2)
                {
                    if (dgvSec[2, e.RowIndex].Value.ToString().ToLower() == "true")
                        oMHRCU.myKeySeu[e.RowIndex].bytTerms = 1;
                    else
                        oMHRCU.myKeySeu[e.RowIndex].bytTerms = 0;
                }
                if (e.ColumnIndex == 3)
                {
                    string strTmp = dgvSec[3, e.RowIndex].Value.ToString();
                    oMHRCU.myKeySeu[e.RowIndex].strRemark = strTmp;
                }
                if (e.ColumnIndex == 4)
                {
                    string strTmp = dgvSec[4, e.RowIndex].Value.ToString();
                    dgvSec[4, e.RowIndex].Value = HDLPF.IsNumStringMode(strTmp, 0, 254);
                    oMHRCU.myKeySeu[e.RowIndex].bytSubID = byte.Parse(dgvSec[4, e.RowIndex].Value.ToString());
                }
                if (e.ColumnIndex == 5)
                {
                    string strTmp = dgvSec[5, e.RowIndex].Value.ToString();
                    dgvSec[5, e.RowIndex].Value = HDLPF.IsNumStringMode(strTmp, 0, 254);
                    oMHRCU.myKeySeu[e.RowIndex].bytDevID = byte.Parse(dgvSec[5, e.RowIndex].Value.ToString());
                }
                if (e.ColumnIndex == 6)
                {
                    string strTmp = dgvSec[6, e.RowIndex].Value.ToString();
                    dgvSec[6, e.RowIndex].Value = HDLPF.IsNumStringMode(strTmp, 0, 254);
                    oMHRCU.myKeySeu[e.RowIndex].bytArea = byte.Parse(dgvSec[6, e.RowIndex].Value.ToString());
                }
            }
            catch
            {
            }
        }

        private void dgvSec_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (oMHRCU == null) return;
            if (oMHRCU.myKeySeu == null) return;
            if (e.RowIndex == -1) return;
            if (e.ColumnIndex == -1) return;

            dgvSec[7, e.RowIndex].ReadOnly = (dgvSec[2, e.RowIndex].Value.ToString().ToLower() == "false");

           /* MyBlnReading = true;
            numSON.Value = oMHRCU.MSKeys[e.RowIndex].AvoidCut[1];
            numSOFF.Value = oMHRCU.MSKeys[e.RowIndex].AvoidCut[2];
            MyBlnReading = false;*/
        }

        private void uploadCurrentPageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tsbDown_Click(tsbUpload, null);
        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tabControl.SelectedIndex == 0)
            {
                loadNetWorkAndConn();
            }
            else
            {
                tsbDown_Click(tsbDown, null);
            }
        }

        private void dgvZoneChn1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if ((e.RowIndex == -1) || (e.ColumnIndex == -1)) return;
            for (int i = 0; i < dgvZoneChn1.SelectedRows.Count; i++)
            {
                dgvZoneChn1.SelectedRows[i].Cells[e.ColumnIndex].Value = dgvZoneChn1[e.ColumnIndex, e.RowIndex].Value.ToString();
                switch (e.ColumnIndex)
                {
                    case 2:
                        string strName = myDevName.Split('\\')[0].ToString();
                        byte bytSubID = byte.Parse(strName.Split('-')[0]);
                        byte bytDevID = byte.Parse(strName.Split('-')[1]);

                        byte[] bytTmp = new byte[4];
                        bytTmp[0] = (byte)(dgvZoneChn1.SelectedRows[i].Index + 1);
                        bytTmp[2] = 0;
                        bytTmp[3] = 0;

                        if (dgvZoneChn1[2, dgvZoneChn1.SelectedRows[i].Index].Value.ToString().ToLower() == "true")
                            bytTmp[1] = 100;
                        else bytTmp[1] = 0;

                        Cursor.Current = Cursors.WaitCursor;
                        if (CsConst.mySends.AddBufToSndList(bytTmp, 0x0031, bytSubID, bytDevID, false, true, true,false) == true)
                        {
                            
                            HDLUDP.TimeBetwnNext(20);
                        }

                        Cursor.Current = Cursors.Default;
                        break;
                }
            }
        }

        private void dgvZoneChn1_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            dgvZoneChn1.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void btn1_Click(object sender, EventArgs e)
        {
            if (tvZone.SelectedNode != null && tvZone.SelectedNode.Level == 0)
            {
                foreach (DataGridViewRow Tmp in dgvZoneChn1.SelectedRows)
                {
                    if (Tmp.Selected)  // 确定被选中，添加至区域列表
                    {
                        int intLoadID = int.Parse(Tmp.Cells[0].Value.ToString().Split('-')[0].ToString());
                        oMHRCU.ChnList[intLoadID - 1].intBelongs = Convert.ToByte(tvZone.SelectedNode.Name.ToString());
                        tvZone.SelectedNode.Nodes.Insert(0, null, Tmp.Cells[0].Value.ToString() + "-" + oMHRCU.ChnList[intLoadID - 1].Remark, 0, 0);
                        dgvZoneChn1.Rows.Remove(Tmp);
                    }
                }
                SortNodesByText(tvZone.SelectedNode);
            }
            else if (tvZone.SelectedNode != null && tvZone.SelectedNode.Level == 1)
            {
                foreach (DataGridViewRow Tmp in dgvZoneChn1.SelectedRows)
                {
                    if (Tmp.Selected)  // 确定被选中，添加至区域列表
                    {
                        int intLoadID = int.Parse(Tmp.Cells[0].Value.ToString());
                        oMHRCU.ChnList[intLoadID - 1].intBelongs = Convert.ToByte(tvZone.SelectedNode.Parent.Name.ToString());
                        tvZone.SelectedNode.Parent.Nodes.Insert(0, null, Tmp.Cells[0].Value.ToString() + "-" + oMHRCU.ChnList[intLoadID - 1].Remark, 0, 0);
                        dgvZoneChn1.Rows.Remove(Tmp);
                    }
                }
                SortNodesByText(tvZone.SelectedNode.Parent);
            }
            lb11.Text = dgvZoneChn1.Rows.Count.ToString();
        }

        private void SortNodesByText(TreeNode node)
        {
            TreeNodeCollection NodeCollection = node.Nodes;
            TreeNode[] NodeAry = new TreeNode[NodeCollection.Count];
            int[,] arayChannelIDAndIndex = new int[2, NodeCollection.Count];
            for (int i = 0; i < NodeCollection.Count; i++)
            {
                arayChannelIDAndIndex[0, i] = Convert.ToInt32(NodeCollection[i].Text.Split('-')[0].ToString());
                arayChannelIDAndIndex[1, i] = i;
                NodeAry[i] = new TreeNode();
                NodeAry[i] = NodeCollection[i];
            }
            for (int i = 0; i < arayChannelIDAndIndex.Length / 2; i++)
            {
                int temp1 = 0;
                int temp2 = 0;
                temp1 = arayChannelIDAndIndex[0, i];
                temp2 = arayChannelIDAndIndex[1, i];
                for (int j = i + 1; j < arayChannelIDAndIndex.Length / 2; j++)
                {
                    int temp3 = arayChannelIDAndIndex[0, j];
                    int temp4 = arayChannelIDAndIndex[1, j];
                    if (arayChannelIDAndIndex[0, j] < arayChannelIDAndIndex[0, i])
                    {
                        arayChannelIDAndIndex[0, i] = temp3;
                        arayChannelIDAndIndex[1, i] = temp4;
                        arayChannelIDAndIndex[0, j] = temp1;
                        arayChannelIDAndIndex[1, j] = temp2;
                    }
                }
            }
            node.Nodes.Clear();
            for (int i = 0; i < arayChannelIDAndIndex.Length / 2; i++)
            {
                node.Nodes.Add(NodeAry[arayChannelIDAndIndex[1, i]]);
            }
        }

        private void btn2_Click(object sender, EventArgs e)
        {
            if (tvZone.Nodes == null) return;
            if (oMHRCU == null) return;
            if (oMHRCU.Areas == null) return;
            if (tvZone.SelectedNode == null) return;
            TreeNode node = tvZone.SelectedNode;
            if (node.Level == 1)
            {
                DeleteNodeInAreaForm(node.Text);
                tvZone.SelectedNode.Remove();
            }
            else if (node.Level == 0)
            {
                foreach (TreeNode Tmp in tvZone.SelectedNode.Nodes)
                {
                    DeleteNodeInAreaForm(Tmp.Text);
                }
                tvZone.SelectedNode.Nodes.Clear();
            }
            lb11.Text = dgvZoneChn1.Rows.Count.ToString();
        }

        private void btn3_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void chbSceneOutput_CheckedChanged(object sender, EventArgs e)
        {
            oMHRCU.isOutput = chbSceneOutput.Checked;
            if (!chbSceneOutput.Checked)
            {
                byte[] bytTmp = new byte[2];
                byte bytAreaID = Convert.ToByte(cbSence.Text.Split('-')[0].ToString());
                byte bytSceID = Convert.ToByte(cbChooseScene.SelectedIndex);
                bytTmp[0] = bytAreaID;
                bytTmp[1] = bytSceID;
                CsConst.mySends.AddBufToSndList(bytTmp, 0xF076, SubNetID, DevID, false, false, false, false);
            }
        }

        private void SceneOutput_Tick(object sender, EventArgs e)
        {

            if (oMHRCU == null) return;
            if (oMHRCU.Areas == null) return;
            if (cbSence.Items.Count <= 0) return;


            byte bytAreaID = Convert.ToByte(cbSence.Text.Split('-')[0].ToString());
            byte bytSceID = Convert.ToByte(cbChooseScene.SelectedIndex);

            string strName = myDevName.Split('\\')[0].ToString();
            byte bytSubID = byte.Parse(strName.Split('-')[0]);
            byte bytDevID = byte.Parse(strName.Split('-')[1]);

            if (oMHRCU.Areas.Count < bytAreaID) return;
            if (oMHRCU.Areas[bytAreaID - 1].Scen.Count < bytSceID) return;
            byte[] bytTmp = new byte[2 + oMHRCU.Areas[bytAreaID-1].Scen[bytSceID].light.Count];
            bytTmp[0] = bytAreaID;
            bytTmp[1] = bytSceID;
            if (cbChooseScene.SelectedIndex != 0)
                oMHRCU.Areas[bytAreaID - 1].Scen[bytSceID].light.CopyTo(bytTmp, 2);
            CsConst.mySends.AddBufToSndList(bytTmp, 0xF074, bytSubID, bytDevID, false, false, false, false);
        }

        private void panel5_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel11_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel10_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnSaveCurtain_Click(object sender, EventArgs e)
        {
            tsbDown_Click(tsbUpload, null);
        }

        private void btnZone_Click(object sender, EventArgs e)
        {
            tsbDown_Click(tsbUpload, null);
        }

        private void btnSaveDry_Click(object sender, EventArgs e)
        {
            tsbDown_Click(tsbUpload, null);
        }

        private void btnChn_Click(object sender, EventArgs e)
        {
            tsbDown_Click(tsbUpload, null);
        }

        private void btnSaveScenes_Click(object sender, EventArgs e)
        {
            tsbDown_Click(tsbUpload, null);
        }

        private void btnSvaeSec_Click(object sender, EventArgs e)
        {
            try
            {
                if (tabControl.SelectedTab.Name == "tabLogic")
                {

                    rbOr_CheckedChanged(null, null);
                    chb1_CheckedChanged(null, null);
                    DateS1_ValueChanged(null, null);
                    Time1Num1_ValueChanged(null, null);
                    DateS2_ValueChanged(null, null);
                    Time2Num1_ValueChanged(null, null);
                    truetime_TextChanged(null, null);
                    cbDryNum1_SelectedIndexChanged(null, null);
                    cbDryNum2_SelectedIndexChanged(null, null);
                    cbDry1_SelectedIndexChanged(null, null);
                    cbDry2_SelectedIndexChanged(null, null);
                    oMHRCU.myLogic[SlectedLogicIndex].UV1 = byte.Parse(txtUVID1.Text);
                    oMHRCU.myLogic[SlectedLogicIndex].UV2 = byte.Parse(txtUVID2.Text);
                    cbUV1_SelectedIndexChanged(null, null);
                    cbUV2_SelectedIndexChanged(null, null);
                    cbLogicNum_SelectedIndexChanged(null, null);
                    cbLogicStatu_SelectedIndexChanged(null, null);

                }
            }
            catch
            {
            }
            tsbDown_Click(tsbUpload, null);
        }

        private void btnSaveOther_Click(object sender, EventArgs e)
        {
            tsbDown_Click(tsbUpload, null);
        }

        private void dgvSec_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            dgvSec.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void chbBroadcast_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        private void btnRefreshStatus_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            byte[] arayTmp = new byte[0];
            if (CsConst.mySends.AddBufToSndList(arayTmp, 0x1806, SubNetID, DevID, false, true, true, false) == true)
            {
                if (CsConst.myRevBuf[27] == 1) chbBroadcast.Checked = true;
                else chbBroadcast.Checked = false;
                if (CsConst.myRevBuf[26] == 0) radioButtonTrailing.Checked = true;
                else if (CsConst.myRevBuf[26] == 1) radioButtonLeading.Checked = true;
                
            }
            Cursor.Current = Cursors.Default;
        }

        private void btnSaveStatus_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            byte[] arayTmp = new byte[2];
            if (radioButtonTrailing.Checked) arayTmp[0] = 0;
            else if (radioButtonLeading.Checked) arayTmp[0] = 1;
            if (chbBroadcast.Checked) arayTmp[1] = 1;
            if (CsConst.mySends.AddBufToSndList(arayTmp, 0x1808, SubNetID, DevID, false, true, true, false) == true)
            {
                CsConst.myRevBuf=new byte[1200];
            }
            Cursor.Current = Cursors.Default;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            grpHost.Enabled = chbHost.Checked;
            if (isRead) return;
            if (oMHRCU == null) return;
            if (oMHRCU.myHVAC == null) return;
            if (chbHost.Checked)
                oMHRCU.myHVAC.arayHost[1] = 1;
            else
                oMHRCU.myHVAC.arayHost[1] = 0;
        }

        private void checkBox1_CheckedChanged_1(object sender, EventArgs e)
        {
            if (isRead) return;
            if (oMHRCU == null) return;
            if (oMHRCU.myHVAC == null) return;
            if (cbHost.SelectedIndex < 0) return;
            if (chbSlave.Checked)
                oMHRCU.myHVAC.arayHost[2] = Convert.ToByte(oMHRCU.myHVAC.arayHost[2] | (1 << cbHost.SelectedIndex));
            else
                oMHRCU.myHVAC.arayHost[2] = Convert.ToByte(oMHRCU.myHVAC.arayHost[2] & (~((1 << cbHost.SelectedIndex) & 0xFF)));
        }

        private void rbM_CheckedChanged(object sender, EventArgs e)
        {
            cbDelay1.Items.Clear();
            if (rbM.Checked)
            {
                lbU1.Text = "(M)";
                for (int i = 1; i <= 10; i++)
                    cbDelay1.Items.Add(i.ToString());
            }
            else if (rbS.Checked)
            {
                lbU1.Text = "(S)";
                for (int i = 3; i <= 127; i++)
                    cbDelay1.Items.Add(i.ToString());
            }
            if (cbDelay1.SelectedIndex < 0) cbDelay1.SelectedIndex = 0;
            if (isRead) return;
            if (oMHRCU == null) return;
            if (oMHRCU.myHVAC == null) return;
            if (rbM.Checked)
            {
                oMHRCU.myHVAC.arayTime[2] = Convert.ToByte(cbDelay1.SelectedIndex + 1);
            }
            else
            {
                oMHRCU.myHVAC.arayTime[2] = Convert.ToByte((cbDelay1.SelectedIndex + 3) | (1 << 7));
            }
        }

        private void btnRefTest_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            byte[] ArayTmp = new byte[0];
            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1C96, SubNetID, DevID, false, true, true, false) == true)
            {
                if (oMHRCU == null) return;
                if (oMHRCU.myHVAC == null) return;
                Array.Copy(CsConst.myRevBuf, 25, oMHRCU.myHVAC.arayTest, 0, 3);
                showHVACFunction();
            }
            Cursor.Current = Cursors.Default;
        }

        private void btnSaveTest_Click(object sender, EventArgs e)
        {
            if (oMHRCU == null) return;
            if (oMHRCU.myHVAC == null) return;
            Cursor.Current = Cursors.WaitCursor;
            CsConst.mySends.AddBufToSndList(oMHRCU.myHVAC.arayTest, 0x1C94, SubNetID, DevID, false, true, true, false);
            Cursor.Current = Cursors.Default;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (oMHRCU == null) return;
            if (oMHRCU.myHVAC == null) return;
            Cursor.Current = Cursors.WaitCursor;
            CsConst.mySends.AddBufToSndList(oMHRCU.myHVAC.arayTime, 0xE3F6, SubNetID, DevID, false, true, true, false);
            Cursor.Current = Cursors.Default;
        }

        private void btnRefHost_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            byte[] ArayTmp = new byte[0];
            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1C46, SubNetID, DevID, false, true, true, false) == true)
            {
                if (oMHRCU == null) return;
                if (oMHRCU.myHVAC == null) return;
                Array.Copy(CsConst.myRevBuf, 25, oMHRCU.myHVAC.arayHost, 0, 19);
                showHVACFunction();
            }
            Cursor.Current = Cursors.Default;
        }

        private void btnSaveHost_Click(object sender, EventArgs e)
        {
            if (oMHRCU == null) return;
            if (oMHRCU.myHVAC == null) return;
            Cursor.Current = Cursors.WaitCursor;
            CsConst.mySends.AddBufToSndList(oMHRCU.myHVAC.arayHost, 0x1C44, SubNetID, DevID, false, true, true, false);
            Cursor.Current = Cursors.Default;
        }

        private void btnSaveHeat_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            byte[] arayTmp = new byte[31];
            Array.Copy(oMHRCU.myHVAC.arayProtect, 0, arayTmp, 0, 16);
            Array.Copy(oMHRCU.myHVAC.arayProtect, 23, arayTmp, 21, 10);
            arayTmp[1] = 1;
            if (arayTmp[6] > 10 || arayTmp[6] < 1) arayTmp[6] = 1;
            if (arayTmp[9] > 10 || arayTmp[9] < 1) arayTmp[9] = 1;
            if (arayTmp[12] > 10 || arayTmp[12] < 1) arayTmp[12] = 1;
            if (arayTmp[15] > 10 || arayTmp[15] < 1) arayTmp[15] = 1;
            arayTmp[17] = 1;
            arayTmp[18] = 1;
            arayTmp[19] = 1;
            arayTmp[20] = 1;
            arayTmp[22] = 1;
            arayTmp[23] = 1;
            if (CsConst.mySends.AddBufToSndList(arayTmp, 0x1C4C, SubNetID, DevID, false, true, true, false) == true)
            {
                
                HDLUDP.TimeBetwnNext(20);
            }
            else
            {
                Cursor.Current = Cursors.Default;
                return;
            }

            arayTmp = new byte[1];
            arayTmp[0] = oMHRCU.myHVAC.Interval;
            CsConst.mySends.AddBufToSndList(arayTmp, 0x1C90, SubNetID, DevID, false, true, true, false);
            Cursor.Current = Cursors.Default;
        }

        private void btnAC_Click(object sender, EventArgs e)
        {
            FrmACModeSetup frmTmp = new FrmACModeSetup(oMHRCU, myDevName, MyintDeviceType);
            frmTmp.ShowDialog();
        }

        private void chbTest_CheckedChanged(object sender, EventArgs e)
        {
            grotestmode.Enabled = chbTest.Checked;
            groupBox6.Enabled = chbTest.Checked;
            if (isRead) return;
            if (oMHRCU == null) return;
            if (oMHRCU.myHVAC == null) return;
            if (chbTest.Checked)
                oMHRCU.myHVAC.arayTest[0] = 1;
            else
                oMHRCU.myHVAC.arayTest[0] = 0;
        }

        private void chbHeat_CheckedChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            if (oMHRCU == null) return;
            if (oMHRCU.myHVAC == null) return;
            if (chbHeat.Checked)
                oMHRCU.myHVAC.arayProtect[23] = 1;
            else
                oMHRCU.myHVAC.arayProtect[23] = 0;
        }

        private void sbTempAdjust_ValueChanged(object sender, EventArgs e)
        {
            lbTempAdjustValue.Text = (sbTempAdjust.Value - 10).ToString() + lbTempUnitValue.Text;
            if (isRead) return;
            if (oMHRCU == null) return;
            if (oMHRCU.myHVAC == null) return;
            oMHRCU.myHVAC.arayProtect[2] = Convert.ToByte(sbTempAdjust.Value);
        }

        private void cbHost_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            if (oMHRCU == null) return;
            if (oMHRCU.myHVAC == null) return;
            if (cbHost.SelectedIndex < 0) return;
            chbSlave.Checked = ((oMHRCU.myHVAC.arayHost[2] & (1 << (cbHost.SelectedIndex))) > 0);
            txtSlaveSub.Text = oMHRCU.myHVAC.arayHost[(cbHost.SelectedIndex + 2) * 2 - 1].ToString();
            txtSlaveDev.Text = oMHRCU.myHVAC.arayHost[(cbHost.SelectedIndex + 2) * 2].ToString();
        }

        private void btnTest1_Click(object sender, EventArgs e)
        {
            int Tag = Convert.ToInt32((sender as Button).Tag);
            if (Tag == 0)
            {
                (sender as Button).Tag = 1;
                (sender as Button).ImageIndex = 1;
            }
            else
            {
                (sender as Button).Tag = 0;
                (sender as Button).ImageIndex = 0;
            }
            if (isRead) return;
            if (oMHRCU == null) return;
            if (oMHRCU.myHVAC == null) return;
            oMHRCU.myHVAC.arayTest[1] = 0;
            if (Convert.ToInt32(btnTest1.Tag) == 1) oMHRCU.myHVAC.arayTest[1] = Convert.ToByte((oMHRCU.myHVAC.arayTest[1] | 1));
            if (Convert.ToInt32(btnTest2.Tag) == 1) oMHRCU.myHVAC.arayTest[1] = Convert.ToByte((oMHRCU.myHVAC.arayTest[1] | (1 << 1)));
            if (Convert.ToInt32(btnTest3.Tag) == 1) oMHRCU.myHVAC.arayTest[1] = Convert.ToByte((oMHRCU.myHVAC.arayTest[1] | (1 << 2)));
        }

        private void btnTest4_Click(object sender, EventArgs e)
        {
            int Tag = Convert.ToInt32((sender as Button).Tag);
            if (Tag == 0)
            {
                for (int i = 4; i <= 6; i++)
                {
                    Button tmp = this.Controls.Find("btnTest" + i.ToString(), true)[0] as Button;
                    tmp.ImageIndex = 0;
                    tmp.Tag = 0;
                }
                (sender as Button).Tag = 1;
                (sender as Button).ImageIndex = 1;
            }
            else
            {
                (sender as Button).Tag = 0;
                (sender as Button).ImageIndex = 0;
            }
            if (isRead) return;
            if (oMHRCU == null) return;
            if (oMHRCU.myHVAC == null) return;
            if (Convert.ToInt32(btnTest4.Tag) == 1) oMHRCU.myHVAC.arayTest[2] = 1;
            else if (Convert.ToInt32(btnTest5.Tag) == 1) oMHRCU.myHVAC.arayTest[2] = 2;
            else if (Convert.ToInt32(btnTest6.Tag) == 1) oMHRCU.myHVAC.arayTest[2] = 3;
        }

        private void cbDelay1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            if (oMHRCU == null) return;
            if (oMHRCU.myHVAC == null) return;
            if (rbM.Checked)
            {
                oMHRCU.myHVAC.arayTime[2] = Convert.ToByte(cbDelay1.SelectedIndex + 1);
            }
            else
            {
                oMHRCU.myHVAC.arayTime[2] = Convert.ToByte((cbDelay1.SelectedIndex + 3) | (1 << 7));
            }
        }

        private void cbDelay3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            if (oMHRCU == null) return;
            if (oMHRCU.myHVAC == null) return;
            oMHRCU.myHVAC.arayTime[0] = Convert.ToByte(cbDelay3.SelectedIndex + 1);
        }

        private void cbDelay4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            if (oMHRCU == null) return;
            if (oMHRCU.myHVAC == null) return;
            oMHRCU.myHVAC.arayTime[1] = Convert.ToByte(cbDelay4.SelectedIndex + 1);
        }

        private void cbDelay2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            if (oMHRCU == null) return;
            if (oMHRCU.myHVAC == null) return;
            oMHRCU.myHVAC.arayTime[3] = Convert.ToByte(cbDelay2.SelectedIndex + 1);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            byte[] ArayTmp = new byte[0];
            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1C4E, SubNetID, DevID, false, true, true, false) == true)
            {
                if (oMHRCU == null) return;
                if (oMHRCU.myHVAC == null) return;
                Array.Copy(CsConst.myRevBuf, 25, oMHRCU.myHVAC.arayProtect, 0, 36);
                
                HDLUDP.TimeBetwnNext(1);
            }
            else
            {
                Cursor.Current = Cursors.Default;
                return;
            }

            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x1C92, SubNetID, DevID, false, true, true, false) == true)
            {
                oMHRCU.myHVAC.Interval = CsConst.myRevBuf[25];
                
            }
            else
            {
                Cursor.Current = Cursors.Default;
                return;
            }
            showHVACFunction();
            Cursor.Current = Cursors.Default;
        }

        private void txtSlaveSub_TextChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            if (oMHRCU == null) return;
            if (oMHRCU.myHVAC == null) return;
            if (cbHost.SelectedIndex < 0) return;
            string str = txtSlaveSub.Text;
            txtSlaveSub.Text = HDLPF.IsNumStringMode(str, 0, 255);
            oMHRCU.myHVAC.arayHost[(cbHost.SelectedIndex + 2) * 2 - 1] = Convert.ToByte(txtSlaveSub.Text);
        }

        private void txtSlaveDev_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar) || e.KeyChar == (char)8)
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void txtSlaveDev_Leave(object sender, EventArgs e)
        {
            if (isRead) return;
            if (oMHRCU == null) return;
            if (oMHRCU.myHVAC == null) return;
            if (cbHost.SelectedIndex < 0) return;
            string str = txtSlaveDev.Text;
            txtSlaveDev.Text = HDLPF.IsNumStringMode(str, 0, 255);
            oMHRCU.myHVAC.arayHost[(cbHost.SelectedIndex + 2) *2] = Convert.ToByte(txtSlaveSub.Text);
        }

        private void cbHeat_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            if (oMHRCU == null) return;
            if (oMHRCU.myHVAC == null) return;
            oMHRCU.myHVAC.arayProtect[24] = Convert.ToByte(cbHeat.SelectedIndex);
        }

        private void cbOper_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            if (oMHRCU == null) return;
            if (oMHRCU.myHVAC == null) return;
            oMHRCU.myHVAC.arayProtect[0] = Convert.ToByte(cbOper.SelectedIndex);
        }

        private void chbSensor1_CheckedChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            if (oMHRCU == null) return;
            if (oMHRCU.myHVAC == null) return;
            oMHRCU.myHVAC.arayProtect[3] = 0;
            if (chbSensor1.Checked)
                oMHRCU.myHVAC.arayProtect[3] = 1;
            else
                oMHRCU.myHVAC.arayProtect[3] = 0;

            if (chbSensor2.Checked)
                oMHRCU.myHVAC.arayProtect[3] = Convert.ToByte(oMHRCU.myHVAC.arayProtect[3] | (1 << 1));
            if (chbSensor3.Checked)
                oMHRCU.myHVAC.arayProtect[3] = Convert.ToByte(oMHRCU.myHVAC.arayProtect[3] | (1 << 2));
            if (chbSensor4.Checked)
                oMHRCU.myHVAC.arayProtect[3] = Convert.ToByte(oMHRCU.myHVAC.arayProtect[3] | (1 << 3));
        }

        private void txtSubS1_Leave(object sender, EventArgs e)
        {
            if (isRead) return;
            if (oMHRCU == null) return;
            if (oMHRCU.myHVAC == null) return;
            string str = (sender as TextBox).Text;
            (sender as TextBox).Text = HDLPF.IsNumStringMode(str, 0, 255);
            for (int i = 1; i <= 4; i++)
            {
                TextBox temp1 = this.Controls.Find("txtSubS" + i.ToString(), true)[0] as TextBox;
                TextBox temp2 = this.Controls.Find("txtDevS" + i.ToString(), true)[0] as TextBox;
                oMHRCU.myHVAC.arayProtect[4 + (i - 1) * 3] = Convert.ToByte(temp1.Text);
                oMHRCU.myHVAC.arayProtect[5 + (i - 1) * 3] = Convert.ToByte(temp2.Text);
            }
        }

        private void cbChnS1_TextChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            if (oMHRCU == null) return;
            if (oMHRCU.myHVAC == null) return;
            for (int i = 1; i <= 4; i++)
            {
                ComboBox temp3 = this.Controls.Find("cbChnS" + i.ToString(), true)[0] as ComboBox;
                oMHRCU.myHVAC.arayProtect[6 + (i - 1) * 3] = Convert.ToByte(temp3.SelectedIndex);
            }
        }

        private void cbInterval_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            if (oMHRCU == null) return;
            if (oMHRCU.myHVAC == null) return;
            oMHRCU.myHVAC.Interval = Convert.ToByte(cbInterval.SelectedIndex + 3);
        }

        private void txtHeatSub_Leave(object sender, EventArgs e)
        {
            if (isRead) return;
            if (oMHRCU == null) return;
            if (oMHRCU.myHVAC == null) return;
            string str = txtHeatSub.Text;
            txtHeatSub.Text = HDLPF.IsNumStringMode(str, 0, 255);
            oMHRCU.myHVAC.arayProtect[26] = Convert.ToByte(txtHeatSub.Text);
        }

        private void txtHeatDev_Leave(object sender, EventArgs e)
        {
            if (isRead) return;
            if (oMHRCU == null) return;
            if (oMHRCU.myHVAC == null) return;
            string str = txtHeatDev.Text;
            txtHeatDev.Text = HDLPF.IsNumStringMode(str, 0, 255);
            oMHRCU.myHVAC.arayProtect[27] = Convert.ToByte(txtHeatDev.Text);
        }

        private void txtHeatChn_Leave(object sender, EventArgs e)
        {
            if (isRead) return;
            if (oMHRCU == null) return;
            if (oMHRCU.myHVAC == null) return;
            string str = txtHeatChn.Text;
            txtHeatChn.Text = HDLPF.IsNumStringMode(str, 0, 255);
            oMHRCU.myHVAC.arayProtect[28] = Convert.ToByte(txtHeatChn.Text);
        }

        private void txtHeat1_Leave(object sender, EventArgs e)
        {
            if (isRead) return;
            if (oMHRCU == null) return;
            if (oMHRCU.myHVAC == null) return;
            string str = txtHeat1.Text;
            txtHeat1.Text = HDLPF.IsNumStringMode(str, 0, 100);
            oMHRCU.myHVAC.arayProtect[30] = Convert.ToByte(txtHeat1.Text);
        }

        private void txtHeat2_Leave(object sender, EventArgs e)
        {
            if (isRead) return;
            if (oMHRCU == null) return;
            if (oMHRCU.myHVAC == null) return;
            string str = txtHeat2.Text;
            txtHeat2.Text = HDLPF.IsNumStringMode(str, 0, 100);
            oMHRCU.myHVAC.arayProtect[29] = Convert.ToByte(txtHeat2.Text);
        }

        private void btnACSetup_Click(object sender, EventArgs e)
        {
            FrmACSetupForRCU frmtemp = new FrmACSetupForRCU(myDevName, oMHRCU, MyintDeviceType);
            frmtemp.ShowDialog();
        }

        private void grpAC_Enter(object sender, EventArgs e)
        {

        }

        private void btnSaveAndClose2_Click(object sender, EventArgs e)
        {
            btnZone_Click(null, null);
            this.Close();
        }

        private void btnSaveAndClose3_Click(object sender, EventArgs e)
        {
            btnChn_Click(null, null);
            this.Close();
        }

        private void btnSaveAndClose4_Click(object sender, EventArgs e)
        {
            btnSaveScenes_Click(null, null);
            this.Close();
        }

        private void btnSaveAndClose5_Click(object sender, EventArgs e)
        {
            btnSaveCurtain_Click(null, null);
            this.Close();
        }

        private void btnSaveAndClose6_Click(object sender, EventArgs e)
        {
            btnSaveDry_Click(null, null);
            this.Close();
        }

        private void btnSaveAndClose7_Click(object sender, EventArgs e)
        {
            btnSvaeSec_Click(null, null);
            this.Close();
        }

        private void btnSaveAndClose8_Click(object sender, EventArgs e)
        {
            btnSaveOther_Click(null, null);
            this.Close();
        }

        private void btnSave9_Click(object sender, EventArgs e)
        {
            btnSaveTest_Click(null, null);
            btnSaveHost_Click(null, null);
            btnSaveHeat_Click(null, null);
        }

        private void btnSaveAndClose9_Click(object sender, EventArgs e)
        {
            btnSave9_Click(null, null);
            this.Close();
        }

        private void txt232ToBusFrm_Leave(object sender, EventArgs e)
        {
            string str = txt232ToBusFrm.Text;
            int num = Convert.ToInt32(txt232ToBusTo.Text);
            txt232ToBusFrm.Text = HDLPF.IsNumStringMode(str, 1, num);
            txt232ToBusFrm.SelectionStart = txt232ToBusFrm.Text.Length;
        }

        private void txt232ToBusTo_Leave(object sender, EventArgs e)
        {
            string str = txt232ToBusTo.Text;
            int num = Convert.ToInt32(txt232ToBusFrm.Text);
            txt232ToBusTo.Text = HDLPF.IsNumStringMode(str, num, 49);
            txt232ToBusTo.SelectionStart = txt232ToBusTo.Text.Length;
        }

        private void dgv232ToBus_MouseDown(object sender, MouseEventArgs e)
        {
            
        }


        private void readRS232BUSTargets(int SelectIndex)
        {
            Cursor.Current = Cursors.WaitCursor;
            dgv232ToBusTarget.Rows.Clear();
            for (int i = 1; i <= 5; i++)
            {

                byte[] ArayTmp = new byte[2];
                ArayTmp[0] = Convert.ToByte(SelectIndex);
                ArayTmp[1] = Convert.ToByte(i);


                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xE414, SubNetID, DevID, false, true, true, false) == true)
                {
                    string strType = "";
                    strType = ButtonControlType.ConvertorKeyModeToPublicModeGroup(CsConst.myRevBuf[28]);
                    string strParam1 = "0", strParam2 = "0", strParam3 = "0", strParam4 = "0";
                    strParam1 = CsConst.myRevBuf[31].ToString();
                    strParam2 = CsConst.myRevBuf[32].ToString();
                    strParam3 = CsConst.myRevBuf[33].ToString();
                    strParam4 = CsConst.myRevBuf[34].ToString();
                    SetParams(ref strType, ref strParam1, ref strParam2, ref strParam3, strParam4);
                    object[] obj = new object[] { i.ToString(),CsConst.myRevBuf[29].ToString(),CsConst.myRevBuf[30].ToString(),strType
                                ,strParam1,strParam2,strParam3};
                    dgv232ToBusTarget.Rows.Add(obj);
                    
                    HDLUDP.TimeBetwnNext(1);
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
                str1 = str1 + "(" + CsConst.mstrINIDefault.IniReadValue("public", "99844", "") + ")";
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

        private void dgv232ToBusTarget_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            Byte[] PageID = new Byte[4] { 200, 1, 0, 0 };

            if (dgv232ToBusTarget.SelectedRows != null && dgv232ToBusTarget.SelectedRows.Count > 0)
            {
                PageID[0] = (Byte)dgv232ToBusTarget.SelectedRows[0].Index;
            }

            frmCmdSetup CmdSetup = new frmCmdSetup(oMHRCU, myDevName, MyintDeviceType, PageID);
            CmdSetup.ShowDialog();
        }

        void FrmRS232BUSTargetsForMHRCU_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (tabControl.SelectedTab.Name == "tabPage1")
            {
                if (dgv232ToBus.RowCount > 0)
                    dgv232ToBus_CellClick(dgv232ToBus, new DataGridViewCellEventArgs(0, dgv232ToBus.CurrentRow.Index));
            }
            else if (tabControl.SelectedTab.Name == "tab485BUS")
            {
                if (dgv485Bus.RowCount > 0)
                    dgv485Bus_CellClick(dgv232ToBus, new DataGridViewCellEventArgs(0, dgv485Bus.CurrentRow.Index));
            }
        }

        private void dgv232ToBus_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int id = Convert.ToInt32(dgv232ToBus[0, e.RowIndex].Value.ToString());
            readRS232BUSTargets(id);
        }

        private void dgv232ToBus_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            FrmRS232BUSForMHRCU frmTmp = new FrmRS232BUSForMHRCU(myDevName, oMHRCU, MyintDeviceType, 49, 1);
            frmTmp.FormClosed += frmTmp_FormClosed;
            frmTmp.ShowDialog();
        }

        void frmTmp_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (tabControl.SelectedTab.Name == "tabPage1")
                DisplayRS232BUSInfo();
            else if (tabControl.SelectedTab.Name == "tab485BUS")
                DisplayRS485BUSInfo();
        }

        private void txtBusFrm_Leave(object sender, EventArgs e)
        {
            string str = txtBusFrm.Text;
            int num = Convert.ToInt32(txtBusTo.Text);
            txtBusFrm.Text = HDLPF.IsNumStringMode(str, 1, num);
            txtBusFrm.SelectionStart = txtBusFrm.Text.Length;
        }

        private void txtBusTo_Leave(object sender, EventArgs e)
        {
            string str = txtBusTo.Text;
            int num = Convert.ToInt32(txtBusFrm.Text);
            txtBusTo.Text = HDLPF.IsNumStringMode(str, num, 49);
            txtBusTo.SelectionStart = txtBusTo.Text.Length;
        }

        private void dgvBUSto232_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int id = Convert.ToInt32(dgvBUSto232[0, e.RowIndex].Value.ToString());
            readBusRS232Targets(id);
        }

        private void readBusRS232Targets(int SelectIndex)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                dgvBusTarget.Rows.Clear();
                int wdMaxValue = 33;
                for (int i = 1; i <= 6; i++)
                {

                    byte[] ArayTmp = new byte[2];
                    ArayTmp[0] = Convert.ToByte(SelectIndex);
                    ArayTmp[1] = Convert.ToByte(i);

                    if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xE424, SubNetID, DevID, false, true, true, false) == true)
                    {
                        string strTime = CsConst.WholeTextsList[1775].sDisplayName;
                        if (1 <= CsConst.myRevBuf[28] && CsConst.myRevBuf[28] <= 8) strTime = CsConst.strAryRS232Time[CsConst.myRevBuf[28] - 1];
                        string strType = CsConst.WholeTextsList[1775].sDisplayName;
                        string strCMD = "";
                        string strEnd = "NONE";
                        byte[] arayCMD = new byte[wdMaxValue + 1];
                        Array.Copy(CsConst.myRevBuf, 30, arayCMD, 0, arayCMD.Length);
                        if (CsConst.myRevBuf[29] == 0)
                        {
                            strType = CsConst.mstrINIDefault.IniReadValue("public", "99838", "");
                            int Count = arayCMD[arayCMD.Length - 1];
                            if (Count > wdMaxValue) Count = wdMaxValue;
                            byte[] arayTmp = new byte[Count];
                            if (Count == 0)
                                strCMD = "";
                            else
                            {
                                Array.Copy(arayCMD, 0, arayTmp, 0, Count);
                                strCMD = HDLPF.Byte2String(arayTmp);
                            }
                            if (arayTmp.Length > 2 && arayTmp[arayTmp.Length - 1] == 0x0A && arayTmp[arayTmp.Length - 2] == 0x0D) strEnd = "<CR+LF>";
                            else if (arayTmp.Length > 1 && arayTmp[arayTmp.Length - 1] == 0x0D) strEnd = "<CR>";
                        }
                        else if (CsConst.myRevBuf[29] == 1)
                        {
                            strType = CsConst.mstrINIDefault.IniReadValue("public", "99839", "");
                            int Count = arayCMD[arayCMD.Length - 1];
                            if (Count > wdMaxValue) Count = wdMaxValue;
                            for (int j = 0; j < Count; j++)
                            {
                                strCMD = strCMD + GlobalClass.AddLeftZero(arayCMD[j].ToString("X"), 2) + " ";
                            }
                        }
                        object[] obj = new object[] { i.ToString(), strTime, strType, strCMD, strEnd };
                        dgvBusTarget.Rows.Add(obj);
                        
                        HDLUDP.TimeBetwnNext(1);
                    }
                    else break;
                }
            }
            catch
            {
            }
            Cursor.Current = Cursors.Default;
        }

        private void btnClose10_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dgvBUSto232_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int MaxCount = 49;
            FrmBusToRS232ForMHRCU frmTmp = new FrmBusToRS232ForMHRCU(myDevName, oMHRCU, MyintDeviceType, MaxCount,1);
            frmTmp.FormClosed += FrmBusToRS232_FormClosed;
            frmTmp.ShowDialog();
        }
        void FrmBusToRS232_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (tabControl.SelectedTab.Name == "tabPage2")
                DisplayBUSRS232Info();
            else if (tabControl.SelectedTab.Name == "tabBUS485")
                DisplayBUSRS485Info();
        }

        private void dgvBusTarget_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int MaxCount = 10;
            FrmBusRS232TargetForMHRCU frmTmp = new FrmBusRS232TargetForMHRCU(myDevName, oMHRCU, MyintDeviceType, MaxCount,
                Convert.ToInt32(dgvBUSto232[0, dgvBUSto232.CurrentRow.Index].Value.ToString()),1);
            frmTmp.FormClosed += dgvBUSto232_FormClosed;
            frmTmp.ShowDialog();
        }

        void dgvBUSto232_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (tabControl.SelectedTab.Name == "tabPage2")
            {
                if (dgvBUSto232.RowCount > 0)
                    dgvBUSto232_CellClick(dgvBUSto232, new DataGridViewCellEventArgs(0, dgvBUSto232.CurrentRow.Index));
            }
            else if (tabControl.SelectedTab.Name == "tabBUS485")
            {
                if (dgvBus485.RowCount > 0)
                    dgvBus485_CellClick(dgvBus485, new DataGridViewCellEventArgs(0, dgvBus485.CurrentRow.Index));
            }
                    
            
        }
        private void dgvFilter_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            dgvFilter.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void dgvFilter_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (oMHRCU == null) return;
                if (oMHRCU.myFilter == null) return;
                if ((e.RowIndex == -1) || (e.ColumnIndex == -1)) return;
                if (dgvFilter.SelectedRows.Count == 0) return;
                if (isRead) return;
                if (dgvFilter[e.ColumnIndex, e.RowIndex].Value == null) dgvFilter[e.ColumnIndex, e.RowIndex].Value = "";
                for (int i = 0; i < dgvFilter.SelectedRows.Count; i++)
                {
                    string strTmp = "";
                    switch (e.ColumnIndex)
                    {
                        case 1:
                            if (dgvFilter[1, dgvFilter.SelectedRows[i].Index].Value.ToString().ToLower() == "true")
                                oMHRCU.myFilter[dgvFilter.SelectedRows[i].Index][1] = 1;
                            else
                                oMHRCU.myFilter[dgvFilter.SelectedRows[i].Index][1] = 0;
                            break;
                        case 2:
                            strTmp = dgvFilter[2, dgvFilter.SelectedRows[i].Index].Value.ToString();
                            oMHRCU.myFilter[dgvFilter.SelectedRows[i].Index][2] = byte.Parse(HDLPF.IsNumStringMode(strTmp, 0, 255));
                            break;
                        case 3:
                            strTmp = dgvFilter[3, dgvFilter.SelectedRows[i].Index].Value.ToString();
                            oMHRCU.myFilter[dgvFilter.SelectedRows[i].Index][3] = byte.Parse(HDLPF.IsNumStringMode(strTmp, 0, 255));
                            break;
                    }
                    dgvFilter.SelectedRows[i].Cells[e.ColumnIndex].Value = dgvFilter[e.ColumnIndex, e.RowIndex].Value.ToString();
                }
                if (e.ColumnIndex == 1)
                {
                    if (dgvFilter[1, e.RowIndex].Value.ToString().ToLower() == "true")
                        oMHRCU.myFilter[e.RowIndex][1] = 1;
                    else
                        oMHRCU.myFilter[e.RowIndex][1] = 0;
                }
                if (e.ColumnIndex == 2)
                {
                    string strTmp = dgvFilter[2, e.RowIndex].Value.ToString();
                    oMHRCU.myFilter[e.RowIndex][2] = byte.Parse(HDLPF.IsNumStringMode(strTmp, 0, 255));
                }
                if (e.ColumnIndex == 3)
                {
                    string strTmp = dgvFilter[3, e.RowIndex].Value.ToString();
                    oMHRCU.myFilter[e.RowIndex][3] = byte.Parse(HDLPF.IsNumStringMode(strTmp, 0, 255));
                }
            }
            catch
            {
            }
        }

        private void btnPort_Click(object sender, EventArgs e)
        {
            FrmBoundRate frmtmp = new FrmBoundRate(SubNetID, DevID, oMHRCU, false);
            frmtmp.ShowDialog();
        }

        private void btnSaveMode_Click(object sender, EventArgs e)
        {
            btnSaveMode.Enabled = false;
            Cursor.Current = Cursors.WaitCursor;
            byte[] arayTmp = new byte[1];
            switch (cbMode.SelectedIndex)
            {
                case 0: arayTmp[0] = 0; break;
                case 1: arayTmp[0] = 3; break;
                case 2: arayTmp[0] = 1; break;
                case 3: arayTmp[0] = 2; break;
                case 4: arayTmp[0] = 4; break;
            }
            if (CsConst.mySends.AddBufToSndList(arayTmp, 0xE42E, SubNetID, DevID, false, true, true, false) == true)
            {
                
            }
            Cursor.Current = Cursors.Default;
            btnSaveMode.Enabled = true;
        }

        private void btnRefMode_Click(object sender, EventArgs e)
        {
            btnRefMode.Enabled = false;
            Cursor.Current = Cursors.WaitCursor;
            byte[] arayTmp = new byte[0];
            if (CsConst.mySends.AddBufToSndList(arayTmp, 0xE42C, SubNetID, DevID, false, true, true, false) == true)
            {
                if (CsConst.myRevBuf[26] == 1) cbMode.SelectedIndex = 2;
                else if (CsConst.myRevBuf[26] == 2) cbMode.SelectedIndex = 3;
                else if (CsConst.myRevBuf[26] == 3) cbMode.SelectedIndex = 1;
                else if (CsConst.myRevBuf[26] == 4) cbMode.SelectedIndex = 4;
                else cbMode.SelectedIndex = 0;
            }
            Cursor.Current = Cursors.Default;
            btnRefMode.Enabled = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Byte[] PageID = new Byte[4]{200,3,0,0};
            if (dgvKey.SelectedRows != null && dgvKey.SelectedRows.Count > 0)
            {
                PageID[0] = (Byte)dgvKey.SelectedRows[0].Index;
            }
            frmCmdSetup CmdSetup = new frmCmdSetup(oMHRCU, myDevName, MyintDeviceType, PageID);
            CmdSetup.ShowDialog();
        }

        private void DgChns_SizeChanged(object sender, EventArgs e)
        {
            HDLSysPF.setDataGridViewColumnsWidth(DgChns);
        }

        private void dgvCurtain_SizeChanged(object sender, EventArgs e)
        {
            HDLSysPF.setDataGridViewColumnsWidth(dgvCurtain);
        }

        private void dgvSec_SizeChanged(object sender, EventArgs e)
        {
            HDLSysPF.setDataGridViewColumnsWidth(dgvSec);
        }

        private void dgv232ToBus_SizeChanged(object sender, EventArgs e)
        {
            HDLSysPF.setDataGridViewColumnsWidth(dgv232ToBus);
        }

        private void dgv232ToBusTarget_SizeChanged(object sender, EventArgs e)
        {
            HDLSysPF.setDataGridViewColumnsWidth(dgv232ToBusTarget);
        }

        private void dgvBUSto232_SizeChanged(object sender, EventArgs e)
        {
            HDLSysPF.setDataGridViewColumnsWidth(dgvBUSto232);
        }

        private void dgvBusTarget_SizeChanged(object sender, EventArgs e)
        {
            HDLSysPF.setDataGridViewColumnsWidth(dgvBusTarget);
        }

        private void dgvFilter_SizeChanged(object sender, EventArgs e)
        {
            HDLSysPF.setDataGridViewColumnsWidth(dgvFilter);
        }

        private void DatePicker_ValueChanged(object sender, EventArgs e)
        {
            List<string> DateStr = new List<string>() { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };
            if (isRead) return;
            if (oMHRCU == null) return;
            if (oMHRCU.arayTime == null) oMHRCU.arayTime = new byte[20];
            oMHRCU.arayTime[0] = Convert.ToByte(DatePicker.Value.Year - 2000);
            oMHRCU.arayTime[1] = Convert.ToByte(DatePicker.Value.Month);
            oMHRCU.arayTime[2] = Convert.ToByte(DatePicker.Value.Day);
            oMHRCU.arayTime[6] = Convert.ToByte(DateStr.IndexOf(DatePicker.Value.DayOfWeek.ToString()));
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

        private void btnReadSysTime_Click(object sender, EventArgs e)
        {
            try
            {
                isRead = true;
                Cursor.Current = Cursors.WaitCursor;
                byte[] ArayTmp = new byte[0];
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xDA00, SubNetID, DevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(MyintDeviceType)) == true)
                {
                    Array.Copy(CsConst.myRevBuf, 26, oMHRCU.arayTime, 0, 6);
                    
                    int wdYear = Convert.ToInt32(oMHRCU.arayTime[0]) + 2000;
                    byte bytMonth = oMHRCU.arayTime[1];
                    byte bytDay = oMHRCU.arayTime[2];
                    byte bytHour = oMHRCU.arayTime[3];
                    byte bytMinute = oMHRCU.arayTime[4];
                    byte bytSecond = oMHRCU.arayTime[5];

                    if (bytHour > 23) bytHour = 0;
                    if (bytMinute > 59) bytMinute = 0;
                    if (bytSecond > 59) bytSecond = 0;
                    if (bytMonth > 12 || bytMonth < 1) bytMonth = 1;
                    if (bytDay > 31 || bytDay<1) bytDay = 1;

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
            isRead = false;
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
                    ArayTmp[i] = oMHRCU.arayTime[i];
                }
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0xDA02, SubNetID, DevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(MyintDeviceType)) == true)
                {
                    
                }
            }
            catch
            {
            }
            Cursor.Current = Cursors.Default;
        }

        private void numTime1_ValueChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            if (oMHRCU == null) return;
            if (oMHRCU.arayTime == null) oMHRCU.arayTime = new byte[20];
            oMHRCU.arayTime[3] = Convert.ToByte(numTime1.Value);
            oMHRCU.arayTime[4] = Convert.ToByte(numTime2.Value);
            oMHRCU.arayTime[5] = Convert.ToByte(numTime3.Value);
        }

        private void chbBrocast_CheckedChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            Cursor.Current = Cursors.WaitCursor;
            byte[] arayTmp = new byte[1];
            if (chbBrocast.Checked) arayTmp[0] = 1;
            if (CsConst.mySends.AddBufToSndList(arayTmp, 0xDA42, SubNetID, DevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(MyintDeviceType)) == true)
            {
                oMHRCU.arayTime[7] = arayTmp[0];
                
            }
            Cursor.Current = Cursors.Default;
        }

        private void dgvLogic_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            SlectedLogicIndex = e.RowIndex;
            try
            {
                byte[] arayTmp = new byte[1];
                arayTmp[0] = Convert.ToByte(dgvLogic[0, e.RowIndex].Value.ToString());
                if (CsConst.mySends.AddBufToSndList(arayTmp, 0x1704, SubNetID, DevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(MyintDeviceType)) == true)
                {
                    oMHRCU.myLogic[e.RowIndex].Relation = CsConst.myRevBuf[27];
                    oMHRCU.myLogic[e.RowIndex].ConditionEnable = CsConst.myRevBuf[28] * 256 + CsConst.myRevBuf[29];
                    if (oMHRCU.myLogic[e.RowIndex].ConditionEnable == 65535) oMHRCU.myLogic[e.RowIndex].ConditionEnable = 0;
                    oMHRCU.myLogic[e.RowIndex].TrueTime = CsConst.myRevBuf[30] * 256 + CsConst.myRevBuf[31];
                    oMHRCU.myLogic[e.RowIndex].FalseTime = CsConst.myRevBuf[32] * 256 + CsConst.myRevBuf[33];
                    oMHRCU.myLogic[e.RowIndex].arayTime1 = new byte[11];
                    Array.Copy(CsConst.myRevBuf, 34, oMHRCU.myLogic[e.RowIndex].arayTime1, 0, 11);
                    oMHRCU.myLogic[e.RowIndex].arayTime2 = new byte[11];
                    Array.Copy(CsConst.myRevBuf, 45, oMHRCU.myLogic[e.RowIndex].arayTime2, 0, 11);
                    oMHRCU.myLogic[e.RowIndex].DryNum1 = CsConst.myRevBuf[56];
                    oMHRCU.myLogic[e.RowIndex].Dry1 = CsConst.myRevBuf[57];
                    oMHRCU.myLogic[e.RowIndex].DryNum2 = CsConst.myRevBuf[58];
                    oMHRCU.myLogic[e.RowIndex].Dry2 = CsConst.myRevBuf[59];
                    oMHRCU.myLogic[e.RowIndex].UV1 = Convert.ToByte(HDLPF.IsNumStringMode(CsConst.myRevBuf[60].ToString(), 201, 248));
                    oMHRCU.myLogic[e.RowIndex].UVCondition1 = CsConst.myRevBuf[61];
                    oMHRCU.myLogic[e.RowIndex].UV2 = Convert.ToByte(HDLPF.IsNumStringMode(CsConst.myRevBuf[62].ToString(), 201, 248));
                    oMHRCU.myLogic[e.RowIndex].UVCondition2 = CsConst.myRevBuf[63];
                    oMHRCU.myLogic[e.RowIndex].LogicNO = CsConst.myRevBuf[64];
                    oMHRCU.myLogic[e.RowIndex].LogicState = CsConst.myRevBuf[65];
                    
                    HDLUDP.TimeBetwnNext(1);
                    showCurrentLogicInfo(e.RowIndex);
                }
                else
                {
                    Cursor.Current = Cursors.Default;
                    return;
                }
            }
            catch
            {
            }
        }

        private void showCurrentLogicInfo(int Index)
        {
            try
            {
                isRead = true;
                MHRCU.RCULogic temp = oMHRCU.myLogic[Index];
                if (temp.Relation == 0) rbOr.Checked = true;
                else if (temp.Relation == 1) rbAnd.Checked = true;
                chb1.Checked = ((((temp.ConditionEnable & (1 << 0)) > 0) ? 1 : 0) == 1);
                chb2.Checked = ((((temp.ConditionEnable & (1 << 1)) > 0) ? 1 : 0) == 1);
                chbDry1.Checked = ((((temp.ConditionEnable & (1 << 2)) > 0) ? 1 : 0) == 1);
                chbDry2.Checked = ((((temp.ConditionEnable & (1 << 3)) > 0) ? 1 : 0) == 1);
                checkboxUV1.Checked = ((((temp.ConditionEnable & (1 << 4)) > 0) ? 1 : 0) == 1);
                checkboxUV2.Checked = ((((temp.ConditionEnable & (1 << 5)) > 0) ? 1 : 0) == 1);
                CheckBoxLogic.Checked = ((((temp.ConditionEnable & (1 << 6)) > 0) ? 1 : 0) == 1);
                truetime.Text = temp.TrueTime.ToString();
                falsetime.Text = temp.FalseTime.ToString();
                int wdYear = Convert.ToInt32(temp.arayTime1[0]) + 2000;
                if (wdYear > 2099) wdYear = 2000;
                byte bytMonth = temp.arayTime1[1];
                byte bytDay = temp.arayTime1[2];
                if (bytMonth > 12) bytMonth = 1;
                if (bytDay > 31) bytDay = 1;
                DateTime d = new DateTime(wdYear, Convert.ToInt32(bytMonth), Convert.ToInt32(bytDay));
                DateS1.Value = d;
                wdYear = Convert.ToInt32(temp.arayTime1[3]) + 2000;
                if (wdYear > 2099) wdYear = DateS1.Value.Year + 1;
                bytMonth = temp.arayTime1[4];
                bytDay = temp.arayTime1[5];
                if (bytMonth > 12) bytMonth = 1;
                if (bytDay > 31) bytDay = 1;
                d = new DateTime(wdYear, Convert.ToInt32(bytMonth), Convert.ToInt32(bytDay));
                DateE1.Value = d;
                byte week = temp.arayTime1[6];
                for (int i = 0; i < 7; i++)
                    chbList1.SetItemChecked(i, (HDLSysPF.GetBit(week, i) == 1));
                if (temp.arayTime1[7] < Time1Num1.Maximum)
                    Time1Num1.Value = temp.arayTime1[7];
                if (temp.arayTime1[8] < Time1Num2.Maximum)
                    Time1Num2.Value = temp.arayTime1[8];
                if (temp.arayTime1[9] < Time1Num4.Maximum)
                    Time1Num4.Value = temp.arayTime1[9];
                if (temp.arayTime1[10] < Time1Num5.Maximum)
                    Time1Num5.Value = temp.arayTime1[10];

                wdYear = Convert.ToInt32(temp.arayTime2[0]) + 2000;
                if (wdYear > 2099) wdYear = 2000;
                bytMonth = temp.arayTime2[1];
                bytDay = temp.arayTime2[2];
                if (bytMonth > 12) bytMonth = 1;
                if (bytDay > 31) bytDay = 1;
                d = new DateTime(wdYear, Convert.ToInt32(bytMonth), Convert.ToInt32(bytDay));
                DateS2.Value = d;
                wdYear = Convert.ToInt32(temp.arayTime2[3]) + 2000;
                if (wdYear > 2099) wdYear = DateS1.Value.Year + 1;
                bytMonth = temp.arayTime2[4];
                bytDay = temp.arayTime2[5];
                if (bytMonth > 12) bytMonth = 1;
                if (bytDay > 31) bytDay = 1;
                d = new DateTime(wdYear, Convert.ToInt32(bytMonth), Convert.ToInt32(bytDay));
                DateE2.Value = d;
                week = temp.arayTime2[6];
                for (int i = 0; i < 7; i++)
                    chbList2.SetItemChecked(i, (HDLSysPF.GetBit(week, i) == 1));
                if (temp.arayTime2[7] < Time2Num1.Maximum)
                    Time2Num1.Value = temp.arayTime2[7];
                if (temp.arayTime2[8] < Time2Num2.Maximum)
                    Time2Num2.Value = temp.arayTime2[8];
                if (temp.arayTime2[9] < Time2Num4.Maximum)
                    Time2Num4.Value = temp.arayTime2[9];
                if (temp.arayTime2[10] < Time2Num5.Maximum)
                    Time2Num5.Value = temp.arayTime2[10];
                cbDryNum1.SelectedIndex = cbDryNum1.Items.IndexOf(temp.DryNum1.ToString());
                if (cbDryNum1.SelectedIndex < 0) cbDryNum1.SelectedIndex = 0;
                if (temp.Dry1 < 2)
                    cbDry1.Text = cbDry1.Items[temp.Dry1].ToString();
                else if (temp.Dry1 == 255)
                    cbDry1.SelectedIndex = 1;
                if (cbDry1.SelectedIndex < 0) cbDry1.SelectedIndex = 0;
                cbDryNum2.SelectedIndex = cbDryNum2.Items.IndexOf(temp.DryNum2.ToString());
                if (cbDryNum2.SelectedIndex < 0) cbDryNum2.SelectedIndex = 0;
                if (temp.Dry2 < 2)
                    cbDry2.Text = cbDry2.Items[temp.Dry2].ToString();
                else if (temp.Dry2 == 255)
                    cbDry2.SelectedIndex = 1;
                if (cbDry2.SelectedIndex < 0) cbDry2.SelectedIndex = 0;
                if (1 <= temp.LogicNO && temp.LogicNO <= 24)
                    cbLogicNum.SelectedIndex = temp.LogicNO - 1;
                if (cbLogicNum.SelectedIndex < 0) cbLogicNum.SelectedIndex = 0;
                if (temp.LogicState < 2)
                    cbLogicStatu.SelectedIndex = temp.LogicState;
                if (cbLogicStatu.SelectedIndex < 0) cbLogicStatu.SelectedIndex = 0;

                txtUVID1.Text = temp.UV1.ToString();
                if (temp.UVCondition1 < 2)
                    cbUV1.Text = cbUV1.Items[temp.UVCondition1].ToString();
                else if (temp.UVCondition1 == 255)
                    cbUV1.SelectedIndex = 1;
                if (cbUV1.SelectedIndex < 0) cbUV1.SelectedIndex = 0;
                txtUVID2.Text = temp.UV2.ToString();
                if (txtUVID2.Text == txtUVID1.Text)
                {
                    if (Convert.ToInt32(txtUVID1.Text) < 248) txtUVID2.Text = (Convert.ToInt32(txtUVID1.Text) + 1).ToString();
                    else txtUVID2.Text = "247";
                }
                if (temp.UVCondition2 < 2)
                    cbUV2.Text = cbUV2.Items[temp.UVCondition2].ToString();
                else if (temp.UVCondition2 == 255)
                    cbUV2.SelectedIndex = 1;
                if (cbUV2.SelectedIndex < 0) cbUV2.SelectedIndex = 0;
            }
            catch
            {
            }
            isRead = false;
            txtUVID1_TextChanged(null, null);
            txtUVID2_TextChanged(null, null);
        }

        private void dgvLogic_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (oMHRCU == null) return;
                if (oMHRCU.myLogic == null) return;
                if ((e.RowIndex == -1) || (e.ColumnIndex == -1)) return;
                if (dgvLogic.SelectedRows.Count == 0) return;
                if (isRead) return;
                if (dgvLogic[e.ColumnIndex, e.RowIndex].Value == null) dgvLogic[e.ColumnIndex, e.RowIndex].Value = "";
                for (int i = 0; i < dgvLogic.SelectedRows.Count; i++)
                {
                    string strTmp = "";
                    switch (e.ColumnIndex)
                    {
                        case 2:
                            strTmp = dgvLogic[2, dgvLogic.SelectedRows[i].Index].Value.ToString();
                            dgvLogic[2, dgvLogic.SelectedRows[i].Index].Value = HDLPF.IsRightStringMode(strTmp);
                            oMHRCU.myLogic[dgvLogic.SelectedRows[i].Index].Remark = dgvLogic[2, dgvLogic.SelectedRows[i].Index].Value.ToString();
                            break;
                        case 1:
                            oMHRCU.myLogic[dgvLogic.SelectedRows[i].Index].Enable = Convert.ToByte(clL3.Items.IndexOf(dgvLogic[1, dgvLogic.SelectedRows[i].Index].Value.ToString()));
                            break;
                    }
                    dgvLogic.SelectedRows[i].Cells[e.ColumnIndex].Value = dgvLogic[e.ColumnIndex, e.RowIndex].Value.ToString();
                }
                if (e.ColumnIndex == 2)
                {
                    string strTmp = dgvLogic[2, e.RowIndex].Value.ToString();
                    dgvLogic[2, e.RowIndex].Value = HDLPF.IsRightStringMode(strTmp);
                    oMHRCU.myLogic[e.RowIndex].Remark = dgvLogic[2, e.RowIndex].Value.ToString();
                }
                if (e.ColumnIndex == 1)
                {
                    oMHRCU.myLogic[e.RowIndex].Enable = Convert.ToByte(clL3.Items.IndexOf(dgvLogic[1, e.RowIndex].Value.ToString()));
                }
            }
            catch
            {
            }
        }

        private void btnTure_Click(object sender, EventArgs e)
        {
           
        }

        private void btnFalse_Click(object sender, EventArgs e)
        {
           
        }

        private void dgvLogic_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            dgvLogic.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void rbOr_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (isRead) return;
                if (oMHRCU == null) return;
                if (dgvLogic.CurrentRow.Index < 0) return;
                if (rbOr.Checked)
                    oMHRCU.myLogic[SlectedLogicIndex].Relation = 0;
                else if(rbAnd.Checked)
                    oMHRCU.myLogic[SlectedLogicIndex].Relation = 1;
            }
            catch
            {
            }
        }

        private void chb1_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                panel64.Enabled = chb1.Checked;
                panel65.Enabled = chb2.Checked;
                cbDry1.Enabled = chbDry1.Checked;
                cbDry2.Enabled = chbDry2.Checked;
                txtUVID1.Enabled = checkboxUV1.Checked;
                txtUVRemark1.Enabled = checkboxUV1.Checked;
                cbUV1.Enabled = checkboxUV1.Checked;
                checkAuto1.Enabled = checkboxUV1.Checked;
                txtUVAuto1.Enabled = checkboxUV1.Checked;
                txtUVID2.Enabled = checkboxUV2.Checked;
                txtUVRemark2.Enabled = checkboxUV2.Checked;
                cbUV2.Enabled = checkboxUV2.Checked;
                checkAuto2.Enabled = checkboxUV2.Checked;
                txtUVAuto2.Enabled = checkboxUV2.Checked;
                cbLogicNum.Enabled = CheckBoxLogic.Checked;
                cbLogicStatu.Enabled = CheckBoxLogic.Checked;
                if (isRead) return;
                if (oMHRCU == null) return;
                if (dgvLogic.CurrentRow.Index < 0) return;
                
                if (chb1.Checked)
                    oMHRCU.myLogic[SlectedLogicIndex].ConditionEnable = (oMHRCU.myLogic[SlectedLogicIndex].ConditionEnable | (1 << 0));
                else
                    oMHRCU.myLogic[SlectedLogicIndex].ConditionEnable =  (oMHRCU.myLogic[SlectedLogicIndex].ConditionEnable & (0xFFFF - (1 << 0))); 

                if(chb2.Checked)
                    oMHRCU.myLogic[SlectedLogicIndex].ConditionEnable = (oMHRCU.myLogic[SlectedLogicIndex].ConditionEnable | (1 << 1));
                else
                    oMHRCU.myLogic[SlectedLogicIndex].ConditionEnable = (oMHRCU.myLogic[SlectedLogicIndex].ConditionEnable & (0xFFFF - (1 << 1)));

                if (chbDry1.Checked)
                    oMHRCU.myLogic[SlectedLogicIndex].ConditionEnable = (oMHRCU.myLogic[SlectedLogicIndex].ConditionEnable | (1 << 2));
                else
                    oMHRCU.myLogic[SlectedLogicIndex].ConditionEnable = (oMHRCU.myLogic[SlectedLogicIndex].ConditionEnable & (0xFFFF - (1 << 2)));

                if (chbDry2.Checked)
                    oMHRCU.myLogic[SlectedLogicIndex].ConditionEnable = (oMHRCU.myLogic[SlectedLogicIndex].ConditionEnable | (1 << 3));
                else
                    oMHRCU.myLogic[SlectedLogicIndex].ConditionEnable = (oMHRCU.myLogic[SlectedLogicIndex].ConditionEnable & (0xFFFF - (1 << 3)));

                if (checkboxUV1.Checked)
                    oMHRCU.myLogic[SlectedLogicIndex].ConditionEnable = (oMHRCU.myLogic[SlectedLogicIndex].ConditionEnable | (1 << 4));
                else
                    oMHRCU.myLogic[SlectedLogicIndex].ConditionEnable = (oMHRCU.myLogic[SlectedLogicIndex].ConditionEnable & (0xFFFF - (1 << 4)));

                if (checkboxUV2.Checked)
                    oMHRCU.myLogic[SlectedLogicIndex].ConditionEnable = (oMHRCU.myLogic[SlectedLogicIndex].ConditionEnable | (1 << 5));
                else
                    oMHRCU.myLogic[SlectedLogicIndex].ConditionEnable = (oMHRCU.myLogic[SlectedLogicIndex].ConditionEnable & (0xFFFF - (1 << 5)));

                if (CheckBoxLogic.Checked)
                    oMHRCU.myLogic[SlectedLogicIndex].ConditionEnable = (oMHRCU.myLogic[SlectedLogicIndex].ConditionEnable | (1 << 6));
                else
                    oMHRCU.myLogic[SlectedLogicIndex].ConditionEnable = (oMHRCU.myLogic[SlectedLogicIndex].ConditionEnable & (0xFFFF - (1 << 6)));

                for (int i = 7; i < 16;i++ )
                    oMHRCU.myLogic[SlectedLogicIndex].ConditionEnable = (oMHRCU.myLogic[SlectedLogicIndex].ConditionEnable & (0xFFFF - (1 << i)));

            }
            catch
            {
            }
        }

        private void DateS1_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (isRead) return;
                if (oMHRCU == null) return;
                if (dgvLogic.CurrentRow.Index < 0) return;
                if (oMHRCU.myLogic[SlectedLogicIndex].arayTime1 == null) oMHRCU.myLogic[SlectedLogicIndex].arayTime1 = new byte[10];
                oMHRCU.myLogic[SlectedLogicIndex].arayTime1[0] = Convert.ToByte(DateS1.Value.Year - 2000);
                if (oMHRCU.myLogic[SlectedLogicIndex].arayTime1[0] > 99) oMHRCU.myLogic[SlectedLogicIndex].arayTime1[0] = 99;
                oMHRCU.myLogic[SlectedLogicIndex].arayTime1[1] = Convert.ToByte(DateS1.Value.Month);
                oMHRCU.myLogic[SlectedLogicIndex].arayTime1[2] = Convert.ToByte(DateS1.Value.Day);
                oMHRCU.myLogic[SlectedLogicIndex].arayTime1[3] = Convert.ToByte(DateE1.Value.Year - 2000);
                if (oMHRCU.myLogic[SlectedLogicIndex].arayTime1[3] > 99) oMHRCU.myLogic[SlectedLogicIndex].arayTime1[3] = 99;
                oMHRCU.myLogic[SlectedLogicIndex].arayTime1[4] = Convert.ToByte(DateE1.Value.Month);
                oMHRCU.myLogic[SlectedLogicIndex].arayTime1[5] = Convert.ToByte(DateE1.Value.Day);

            }
            catch
            {
            }
        }

        private void chbList1_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            try
            {
                if (isRead) return;
                if (oMHRCU == null) return;
                if (dgvLogic.CurrentRow.Index < 0) return;
                if (oMHRCU.myLogic[SlectedLogicIndex].arayTime1 == null) oMHRCU.myLogic[SlectedLogicIndex].arayTime1 = new byte[10];
                oMHRCU.myLogic[SlectedLogicIndex].arayTime1[6] = HDLSysPF.ClearBit(oMHRCU.myLogic[SlectedLogicIndex].arayTime1[6], 7);
                if (e.NewValue == CheckState.Checked)
                    oMHRCU.myLogic[SlectedLogicIndex].arayTime1[6] = HDLSysPF.SetBit(oMHRCU.myLogic[SlectedLogicIndex].arayTime1[6], e.Index);
                else if(e.NewValue==CheckState.Unchecked)
                    oMHRCU.myLogic[SlectedLogicIndex].arayTime1[6] = HDLSysPF.ClearBit(oMHRCU.myLogic[SlectedLogicIndex].arayTime1[6], e.Index);
            }
            catch
            {
            }
        }

        private void Time1Num1_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (isRead) return;
                if (oMHRCU == null) return;
                if (dgvLogic.CurrentRow.Index < 0) return;
                if (oMHRCU.myLogic[SlectedLogicIndex].arayTime1 == null) oMHRCU.myLogic[SlectedLogicIndex].arayTime1 = new byte[10];
                oMHRCU.myLogic[SlectedLogicIndex].arayTime1[7] = Convert.ToByte(Time1Num1.Value);
                oMHRCU.myLogic[SlectedLogicIndex].arayTime1[8] = Convert.ToByte(Time1Num2.Value);
                oMHRCU.myLogic[SlectedLogicIndex].arayTime1[9] = Convert.ToByte(Time1Num4.Value);
                oMHRCU.myLogic[SlectedLogicIndex].arayTime1[10] = Convert.ToByte(Time1Num5.Value);
            }
            catch
            {
            }
        }

        private void DateS2_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (isRead) return;
                if (oMHRCU == null) return;
                if (dgvLogic.CurrentRow.Index < 0) return;
                if (oMHRCU.myLogic[SlectedLogicIndex].arayTime2 == null) oMHRCU.myLogic[SlectedLogicIndex].arayTime2 = new byte[10];
                oMHRCU.myLogic[SlectedLogicIndex].arayTime2[0] = Convert.ToByte(DateS2.Value.Year - 2000);
                if (oMHRCU.myLogic[SlectedLogicIndex].arayTime2[0] > 99) oMHRCU.myLogic[SlectedLogicIndex].arayTime2[0] = 99;
                oMHRCU.myLogic[SlectedLogicIndex].arayTime2[1] = Convert.ToByte(DateS2.Value.Month);
                oMHRCU.myLogic[SlectedLogicIndex].arayTime2[2] = Convert.ToByte(DateS2.Value.Day);
                oMHRCU.myLogic[SlectedLogicIndex].arayTime2[3] = Convert.ToByte(DateE2.Value.Year - 2000);
                if (oMHRCU.myLogic[SlectedLogicIndex].arayTime2[3] > 99) oMHRCU.myLogic[SlectedLogicIndex].arayTime2[3] = 99;
                oMHRCU.myLogic[SlectedLogicIndex].arayTime2[4] = Convert.ToByte(DateE2.Value.Month);
                oMHRCU.myLogic[SlectedLogicIndex].arayTime2[5] = Convert.ToByte(DateE2.Value.Day);
            }
            catch
            {
            }
        }

        private void chbList2_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            try
            {
                if (isRead) return;
                if (oMHRCU == null) return;
                if (dgvLogic.CurrentRow.Index < 0) return;
                if (oMHRCU.myLogic[SlectedLogicIndex].arayTime2 == null) oMHRCU.myLogic[SlectedLogicIndex].arayTime2 = new byte[10];
                oMHRCU.myLogic[SlectedLogicIndex].arayTime2[6] = HDLSysPF.ClearBit(oMHRCU.myLogic[SlectedLogicIndex].arayTime2[6], 7);
                if (e.NewValue == CheckState.Checked)
                    oMHRCU.myLogic[SlectedLogicIndex].arayTime2[6] = HDLSysPF.SetBit(oMHRCU.myLogic[SlectedLogicIndex].arayTime2[6], e.Index);
                else if (e.NewValue == CheckState.Unchecked)
                    oMHRCU.myLogic[SlectedLogicIndex].arayTime2[6] = HDLSysPF.ClearBit(oMHRCU.myLogic[SlectedLogicIndex].arayTime2[6], e.Index);
            }
            catch
            {
            }
        }

        private void Time2Num1_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (isRead) return;
                if (oMHRCU == null) return;
                if (dgvLogic.CurrentRow.Index < 0) return;
                if (oMHRCU.myLogic[SlectedLogicIndex].arayTime2 == null) oMHRCU.myLogic[SlectedLogicIndex].arayTime2 = new byte[10];
                oMHRCU.myLogic[SlectedLogicIndex].arayTime2[7] = Convert.ToByte(Time2Num1.Value);
                oMHRCU.myLogic[SlectedLogicIndex].arayTime2[8] = Convert.ToByte(Time2Num2.Value);
                oMHRCU.myLogic[SlectedLogicIndex].arayTime2[9] = Convert.ToByte(Time2Num4.Value);
                oMHRCU.myLogic[SlectedLogicIndex].arayTime2[10] = Convert.ToByte(Time2Num5.Value);
            }
            catch
            {
            }
        }

        private void truetime_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (isRead) return;
                if (oMHRCU == null) return;
                if (dgvLogic.CurrentRow.Index < 0) return;
                oMHRCU.myLogic[SlectedLogicIndex].TrueTime = Convert.ToInt32(truetime.Text);
                oMHRCU.myLogic[SlectedLogicIndex].FalseTime = Convert.ToInt32(falsetime.Text);
            }
            catch
            {
            }
        }

        private void btnBus485_Click(object sender, EventArgs e)
        {
            tsbDown_Click(tsbDown, null);
        }

        private void btnRef5_Click(object sender, EventArgs e)
        {
            tsbDown_Click(tsbDown, null);
        }

        private void btnClose5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnRef4_Click(object sender, EventArgs e)
        {
            tsbDown_Click(tsbDown, null);
        }

        private void btnClose4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn485Bus_Click(object sender, EventArgs e)
        {
            tsbDown_Click(tsbDown, null);
        }

        private void readRS232BUSTargets(int SelectIndex, int Type)
        {
            Cursor.Current = Cursors.WaitCursor;
            int CMD = 0xE414;
            if (Type == 1)
            {
                dgv232ToBusTarget.Rows.Clear();
                CMD = 0xE414;
            }
            else if (Type == 2)
            {
                dgv484BusTargets.Rows.Clear();
                CMD = 0xDA55;
            }
            for (int i = 1; i <= 6; i++)
            {
                byte[] ArayTmp = new byte[2];
                ArayTmp[0] = Convert.ToByte(SelectIndex);
                ArayTmp[1] = Convert.ToByte(i);
                if (CsConst.mySends.AddBufToSndList(ArayTmp, CMD, SubNetID, DevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(MyintDeviceType)) == true)
                {
                    string strType = "";
                    strType = ButtonControlType.ConvertorKeyModeToPublicModeGroup(CsConst.myRevBuf[28]);
                    string strParam1 = "0", strParam2 = "0", strParam3 = "0", strParam4 = "0";
                    strParam1 = CsConst.myRevBuf[31].ToString();
                    strParam2 = CsConst.myRevBuf[32].ToString();
                    strParam3 = CsConst.myRevBuf[33].ToString();
                    strParam4 = CsConst.myRevBuf[34].ToString();
                    SetParams(ref strType, ref strParam1, ref strParam2, ref strParam3, strParam4);
                    object[] obj = new object[] { i.ToString(),CsConst.myRevBuf[29].ToString(),CsConst.myRevBuf[30].ToString(),strType
                                ,strParam1,strParam2,strParam3};
                    if (Type == 1)
                    {
                        dgv232ToBusTarget.Rows.Add(obj);
                    }
                    else if (Type == 2)
                    {
                        dgv484BusTargets.Rows.Add(obj);
                    }
                    
                    HDLUDP.TimeBetwnNext(1);
                }
                else break;
            }
            Cursor.Current = Cursors.Default;
        }


        private void dgv485Bus_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int id = Convert.ToInt32(dgv485Bus[0, e.RowIndex].Value.ToString());
            readRS232BUSTargets(id, 2);
        }

        private void dgv485Bus_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int MaxCount = 49;
            FrmRS232BUSForMHRCU frmTmp = new FrmRS232BUSForMHRCU(myDevName, oMHRCU, MyintDeviceType, MaxCount, 2);
            frmTmp.FormClosed += frmTmp_FormClosed;
            frmTmp.ShowDialog();
        }

        private void dgv484BusTargets_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            Byte[] PageID = new Byte[4] { 200, 2, 0, 0 };
            if (dgvKey.SelectedRows != null && dgvKey.SelectedRows.Count > 0)
            {
                PageID[0] = (Byte)dgvKey.SelectedRows[0].Index;
            }
            frmCmdSetup CmdSetup = new frmCmdSetup(oMHRCU, myDevName, MyintDeviceType, PageID);
            CmdSetup.ShowDialog();
        }

        private void DisplayRS485BUSInfo()
        {
            try
            {
                int wdMaxValue = 33;
                isRead = true;
                dgv485Bus.Rows.Clear();
                if (oMHRCU == null) return;
                if (oMHRCU.my4852BUS == null) return;
                for (int i = 0; i < oMHRCU.my4852BUS.Count; i++)
                {
                    Rs232ToBus temp = oMHRCU.my4852BUS[i];
                    string strEnable = CsConst.WholeTextsList[1775].sDisplayName;
                    if (temp.rs232Param.enable == 1) strEnable = CsConst.mstrINIDefault.IniReadValue("public", "00042", "");
                    string strType = CsConst.WholeTextsList[1775].sDisplayName;
                    string strCMD = "";
                    string strEnd = "NONE";
                    if (temp.rs232Param.type == 0)
                    {
                        //strType = CsConst.mstrINIDefault.IniReadValue("public", "99838", "");
                        //int Count = temp.TmpRS232.RSCMD[temp.TmpRS232.RSCMD.Length - 1];
                        //if (Count > wdMaxValue) Count = wdMaxValue;
                        //byte[] arayTmp = new byte[Count];
                        //Array.Copy(temp.TmpRS232.RSCMD, 0, arayTmp, 0, Count);
                        //if (Count == 0)
                        //    strCMD = "";
                        //else
                        //    strCMD = HDLPF.Byte2String(arayTmp);
                        //if (arayTmp.Length > 2 && arayTmp[arayTmp.Length - 1] == 0x0A && arayTmp[arayTmp.Length - 2] == 0x0D) strEnd = "<CR+LF>";
                        //else if (arayTmp.Length > 1 && arayTmp[arayTmp.Length - 1] == 0x0D) strEnd = "<CR>";
                    }
                    else if (temp.rs232Param.type == 1)
                    {
                        //strType = CsConst.mstrINIDefault.IniReadValue("public", "99839", "");
                        //int Count = temp.TmpRS232.RSCMD[temp.TmpRS232.RSCMD.Length - 1];
                        //if (Count > wdMaxValue) Count = wdMaxValue;
                        //for (int j = 0; j < Count; j++)
                        //{
                        //    strCMD = strCMD + GlobalClass.AddLeftZero(temp.TmpRS232.RSCMD[j].ToString("X"), 2) + " ";
                        //}
                    }
                    strCMD = strCMD.Trim();
                    object[] obj = new object[] { temp.ID.ToString(), temp.remark.ToString(), strEnable, strType, strCMD, strEnd };
                    dgv485Bus.Rows.Add(obj);
                }
                if (dgv485Bus.RowCount > 0)
                    dgv485Bus_CellClick(dgv485Bus, new DataGridViewCellEventArgs(0, 0));
            }
            catch
            {
            }
            isRead = false;
        }

        private void DisplayBUSRS485Info()
        {
            try
            {
                isRead = true;
                dgvBus485.Rows.Clear();
                if (oMHRCU == null) return;
                if (oMHRCU.myBUS2485 == null) return;
                for (int i = 0; i < oMHRCU.myBUS2485.Count; i++)
                {
                    MHRCU.BUS2RS temp = oMHRCU.myBUS2485[i];
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
                    dgvBus485.Rows.Add(obj);
                }
                if (dgvBus485.RowCount > 0)
                    dgvBus485_CellClick(dgvBus485, new DataGridViewCellEventArgs(0, 0));
            }
            catch
            {
            }
            isRead = false;
        }

        private void dgvBus485_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int id = Convert.ToInt32(dgvBus485[0, e.RowIndex].Value.ToString());
            readBusRS232Targets(id, 2);
        }

        private void dgvBus485_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int MaxCount = 49;
            FrmBusToRS232ForMHRCU frmTmp = new FrmBusToRS232ForMHRCU(myDevName, oMHRCU, MyintDeviceType, MaxCount, 2);
            frmTmp.FormClosed += FrmBusToRS232_FormClosed;
            frmTmp.ShowDialog();
        }

        private void dgvBus485Targets_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int MaxCount = 10;
            FrmBusRS232TargetForMHRCU frmTmp = new FrmBusRS232TargetForMHRCU(myDevName, oMHRCU, MyintDeviceType, MaxCount,
                Convert.ToInt32(dgvBus485Targets[0, dgvBus485Targets.CurrentRow.Index].Value.ToString()),2);
            frmTmp.FormClosed += dgvBUSto232_FormClosed;
            frmTmp.ShowDialog();
        }

        private void readBusRS232Targets(int SelectIndex, int Type)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                int CMD = 0xE424;
                if (Type == 1)
                {
                    dgvBusTarget.Rows.Clear();
                    CMD = 0xE424;
                }
                else if (Type == 2)
                {
                    dgvBus485Targets.Rows.Clear();
                    CMD = 0xDA65;
                }
                int wdMaxValue = 33;
                for (int i = 1; i <= 6; i++)
                {

                    byte[] ArayTmp = new byte[2];
                    ArayTmp[0] = Convert.ToByte(SelectIndex);
                    ArayTmp[1] = Convert.ToByte(i);

                    if (CsConst.mySends.AddBufToSndList(ArayTmp, CMD, SubNetID, DevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(MyintDeviceType)) == true)
                    {
                        string strTime = CsConst.WholeTextsList[1775].sDisplayName;
                        if (1 <= CsConst.myRevBuf[28] && CsConst.myRevBuf[28] <= 8) strTime = CsConst.strAryRS232Time[CsConst.myRevBuf[28] - 1];
                        string strType = CsConst.WholeTextsList[1775].sDisplayName;
                        string strCMD = "";
                        string strEnd = "NONE";
                        byte[] arayCMD = new byte[wdMaxValue + 1];
                        Array.Copy(CsConst.myRevBuf, 30, arayCMD, 0, arayCMD.Length);
                        if (CsConst.myRevBuf[29] == 0)
                        {
                            strType = CsConst.mstrINIDefault.IniReadValue("public", "99838", "");
                            int Count = arayCMD[arayCMD.Length - 1];
                            if (Count > wdMaxValue) Count = wdMaxValue;
                            byte[] arayTmp = new byte[Count];
                            if (Count == 0) strCMD = "";
                            else
                            {
                                Array.Copy(arayCMD, 0, arayTmp, 0, Count);
                                strCMD = HDLPF.Byte2String(arayTmp);
                            }
                            if (arayTmp.Length > 2 && arayTmp[arayTmp.Length - 1] == 0x0A && arayTmp[arayTmp.Length - 2] == 0x0D) strEnd = "<CR+LF>";
                            else if (arayTmp.Length > 1 && arayTmp[arayTmp.Length - 1] == 0x0D) strEnd = "<CR>";
                        }
                        else if (CsConst.myRevBuf[29] == 1)
                        {
                            strType = CsConst.mstrINIDefault.IniReadValue("public", "99839", "");
                            int Count = arayCMD[arayCMD.Length - 1];
                            if (Count > wdMaxValue) Count = wdMaxValue;
                            for (int j = 0; j < Count; j++)
                            {
                                strCMD = strCMD + GlobalClass.AddLeftZero(arayCMD[j].ToString("X"), 2) + " ";
                            }
                        }
                        object[] obj = new object[] { i.ToString(), strTime, strType, strCMD,strEnd };
                        if (Type == 1)
                        {
                            dgvBusTarget.Rows.Add(obj);
                        }
                        else if (Type == 2)
                        {
                            dgvBus485Targets.Rows.Add(obj);
                        }
                        
                        HDLUDP.TimeBetwnNext(1);
                    }
                    else break;
                }
            }
            catch
            {
            }
            Cursor.Current = Cursors.Default;
        }

        private void btn485_Click(object sender, EventArgs e)
        {
            FrmBoundRate frmtmp = new FrmBoundRate(SubNetID, DevID, oMHRCU, true);
            frmtmp.ShowDialog();
        }

        private void groupBox16_Enter(object sender, EventArgs e)
        {

        }

        private void lbLogicTime2_Click(object sender, EventArgs e)
        {

        }

        private void txtUVID1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (isRead) return;
                if (dgvLogic.RowCount <= 0) return;
                if (dgvLogic.CurrentRow.Index < 0) return;
                if (oMHRCU == null) return;
                if (oMHRCU.myLogic == null) return;
                string str = txtUVID1.Text.Trim();
                if (str == "") return;
                if (str.Length >= 3)
                {
                    txtUVID1.Text = HDLPF.IsNumStringMode(str, 201, 248);
                    if (checkboxUV2.Checked && txtUVID1.Text == txtUVID2.Text)
                    {
                        if (txtUVID2.Text == "201") txtUVID1.Text = "202";
                        else txtUVID1.Text = (Convert.ToInt32(txtUVID2.Text) - 1).ToString();
                    }
                    txtUVID1.SelectionStart = txtUVID1.Text.Length;
                    oMHRCU.myLogic[SlectedLogicIndex].UV1 = byte.Parse(txtUVID1.Text);
                    if (CsConst.MyEditMode == 1)
                    {
                        Cursor.Current = Cursors.WaitCursor;
                        isRead = true;
                        byte[] arayTmp = new byte[1];
                        arayTmp[0] = Convert.ToByte(txtUVID1.Text);
                        if ((CsConst.mySends.AddBufToSndList(arayTmp, 0x160A, SubNetID, DevID, false, false, true, false) == true))
                        {
                            byte[] arayRemark = new byte[20];
                            Array.Copy(CsConst.myRevBuf, 27, arayRemark, 0, arayRemark.Length);
                            txtUVRemark1.Text = HDLPF.Byte2String(arayRemark);
                            checkAuto1.Checked = (CsConst.myRevBuf[47] == 1);
                            if (CsConst.myRevBuf[48] * 256 + CsConst.myRevBuf[49] > 3600)
                                txtUVAuto1.Text = "1";
                            else
                                txtUVAuto1.Text = (CsConst.myRevBuf[48] * 256 + CsConst.myRevBuf[49]).ToString();
                            
                        }
                        isRead = false;
                       
                    }
                }
            }
            catch
            {
            }
            Cursor.Current = Cursors.Default;
        }

        private void txtUVID2_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (isRead) return;
                if (dgvLogic.RowCount <= 0) return;
                if (dgvLogic.CurrentRow.Index < 0) return;
                if (oMHRCU == null) return;
                if (oMHRCU.myLogic == null) return;
                string str = txtUVID2.Text.Trim();
                if (str == "") return;
                if (str.Length >= 3)
                {
                    txtUVID2.Text = HDLPF.IsNumStringMode(str, 201, 248);
                    if (checkboxUV1.Checked && txtUVID1.Text == txtUVID2.Text)
                    {
                        if (txtUVID1.Text == "248") txtUVID2.Text = "247";
                        else txtUVID2.Text = (Convert.ToInt32(txtUVID1.Text) + 1).ToString();
                    }
                    txtUVID2.SelectionStart = txtUVID2.Text.Length;
                    oMHRCU.myLogic[SlectedLogicIndex].UV2 = byte.Parse(txtUVID2.Text);
                    if (CsConst.MyEditMode == 1)
                    {
                        Cursor.Current = Cursors.WaitCursor;
                        isRead = true;
                        byte[] arayTmp = new byte[1];
                        arayTmp[0] = byte.Parse(txtUVID2.Text);
                        if ((CsConst.mySends.AddBufToSndList(arayTmp, 0x160A, SubNetID, DevID, false, false, true, false) == true))
                        {
                            byte[] arayRemark = new byte[20];
                            Array.Copy(CsConst.myRevBuf, 27, arayRemark, 0, arayRemark.Length);
                            txtUVRemark2.Text = HDLPF.Byte2String(arayRemark);
              
                            checkAuto2.Checked = (CsConst.myRevBuf[47] == 1);
                            if (CsConst.myRevBuf[48] * 256 + CsConst.myRevBuf[49] > 3600)
                                txtUVAuto2.Text = "1";
                            else
                                txtUVAuto2.Text = (CsConst.myRevBuf[48] * 256 + CsConst.myRevBuf[49]).ToString();
                            
                        }
                        isRead = false;
                    }
                }
            }
            catch
            {
            }
            Cursor.Current = Cursors.Default;
        }

        private void txtUVRemark1_Leave(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            txtUVAuto1.Text = HDLPF.IsNumStringMode(txtUVAuto1.Text, 1, 3600);
            byte[] arayTmp = new byte[24];
            arayTmp[0] = Convert.ToByte(txtUVID1.Text);
            byte[] arayRemark = HDLUDP.StringToByte(txtUVRemark1.Text);
            if (arayRemark.Length <= 20)
                Array.Copy(arayRemark, 0, arayTmp, 1, arayRemark.Length);
            else
                Array.Copy(arayRemark, 0, arayTmp, 1, 20);
            if (checkAuto1.Checked)
                arayTmp[21] = 1;
            int num = Convert.ToInt32(txtUVAuto1.Text);
            arayTmp[22] = Convert.ToByte(num / 256);
            arayTmp[23] = Convert.ToByte(num % 256);
            if ((CsConst.mySends.AddBufToSndList(arayTmp, 0x160C, SubNetID, DevID, false, false, true, false) == true))
            {
                
            }
            Cursor.Current = Cursors.Default;
        }

        private void txtUVRemark2_Leave(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            txtUVAuto2.Text = HDLPF.IsNumStringMode(txtUVAuto2.Text, 1, 3600);
            byte[] arayTmp = new byte[24];
            arayTmp[0] = Convert.ToByte(txtUVID2.Text);
            byte[] arayRemark = HDLUDP.StringToByte(txtUVRemark2.Text);
            if (arayRemark.Length <= 20)
                Array.Copy(arayRemark, 0, arayTmp, 1, arayRemark.Length);
            else
                Array.Copy(arayRemark, 0, arayTmp, 1, 20);
            if (checkAuto2.Checked)
                arayTmp[21] = 1;
            int num = Convert.ToInt32(txtUVAuto2.Text);
            arayTmp[22] = Convert.ToByte(num / 256);
            arayTmp[23] = Convert.ToByte(num % 256);
            if ((CsConst.mySends.AddBufToSndList(arayTmp, 0x160C, SubNetID, DevID, false, false, true, false) == true))
            {
                
            }
            Cursor.Current = Cursors.Default;
        }

        private void cbDry1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (isRead) return;
                if (dgvLogic.RowCount <= 0) return;
                if (dgvLogic.CurrentRow.Index < 0) return;
                if (oMHRCU == null) return;
                if (oMHRCU.myLogic == null) return;
                oMHRCU.myLogic[SlectedLogicIndex].Dry1 = Convert.ToByte(cbDry1.SelectedIndex);
            }
            catch
            {
            }
        }

        private void cbDry2_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (isRead) return;
                if (dgvLogic.RowCount <= 0) return;
                if (dgvLogic.CurrentRow.Index < 0) return;
                if (oMHRCU == null) return;
                if (oMHRCU.myLogic == null) return;
                oMHRCU.myLogic[SlectedLogicIndex].Dry2 = Convert.ToByte(cbDry2.SelectedIndex);
            }
            catch
            {
            }
        }

        private void cbUV1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (isRead) return;
                if (dgvLogic.RowCount <= 0) return;
                if (dgvLogic.CurrentRow.Index < 0) return;
                if (oMHRCU == null) return;
                if (oMHRCU.myLogic == null) return;
                oMHRCU.myLogic[SlectedLogicIndex].UVCondition1 = Convert.ToByte(cbUV1.SelectedIndex);
            }
            catch
            {
            }
        }

        private void cbUV2_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (isRead) return;
                if (dgvLogic.RowCount <= 0) return;
                if (dgvLogic.CurrentRow.Index < 0) return;
                if (oMHRCU == null) return;
                if (oMHRCU.myLogic == null) return;
                oMHRCU.myLogic[SlectedLogicIndex].UVCondition2 = Convert.ToByte(cbUV2.SelectedIndex);
            }
            catch
            {
            }
        }

        private void cbLogicNum_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (isRead) return;
                if (dgvLogic.RowCount <= 0) return;
                if (dgvLogic.CurrentRow.Index < 0) return;
                if (oMHRCU == null) return;
                if (oMHRCU.myLogic == null) return;
                oMHRCU.myLogic[SlectedLogicIndex].LogicNO = Convert.ToByte(cbLogicNum.SelectedIndex + 1);
            }
            catch
            {
            }
        }

        private void cbLogicStatu_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (isRead) return;
                if (dgvLogic.RowCount <= 0) return;
                if (dgvLogic.CurrentRow.Index < 0) return;
                if (oMHRCU == null) return;
                if (oMHRCU.myLogic == null) return;
                oMHRCU.myLogic[SlectedLogicIndex].LogicState = Convert.ToByte(cbLogicStatu.SelectedIndex);
            }
            catch
            {
            }
        }

        private void cbDryNum2_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (isRead) return;
                if (dgvLogic.RowCount <= 0) return;
                if (dgvLogic.CurrentRow.Index < 0) return;
                if (oMHRCU == null) return;
                if (oMHRCU.myLogic == null) return;
                oMHRCU.myLogic[SlectedLogicIndex].DryNum2 = Convert.ToByte(cbDryNum2.SelectedIndex + 1);
            }
            catch
            {
            }
        }

        private void cbDryNum1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (isRead) return;
                if (dgvLogic.RowCount <= 0) return;
                if (dgvLogic.CurrentRow.Index < 0) return;
                if (oMHRCU == null) return;
                if (oMHRCU.myLogic == null) return;
                oMHRCU.myLogic[SlectedLogicIndex].DryNum1 = Convert.ToByte(cbDryNum1.SelectedIndex + 1);
            }
            catch
            {
            }
        }

        private void sbAdjust_ValueChanged(object sender, EventArgs e)
        {

        }

        private void chbBroadCast_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void txtBroadSub_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnCMD_Click(object sender, EventArgs e)
        {
            Byte[] PageID = new Byte[4];
            if (dgvKey.SelectedRows != null && dgvKey.SelectedRows.Count > 0)
            {
                PageID[0] = (Byte)dgvKey.SelectedRows[0].Index;
            }
            frmCmdSetup CmdSetup = new frmCmdSetup(oMHRCU, myDevName, MyintDeviceType, PageID);
            CmdSetup.ShowDialog();
        }

        private void btnMode_Click(object sender, EventArgs e)
        {
            frmButtonSetup ButtonConfigration = new frmButtonSetup(oMHRCU, myDevName, null);
            ButtonConfigration.ShowDialog();
        }

        private void sbMin_ValueChanged(object sender, EventArgs e)
        {

        }

        private void cbEnable_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (MyBlnReading) return;
            if (oMHRCU == null) return;
            if (oMHRCU.MSKeys == null || oMHRCU.MSKeys.Count == 0) return;

            oMHRCU.MSKeys[e.Index].bytEnable = Convert.ToByte(e.NewValue);
        }

        private void cbLock_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (MyBlnReading) return;
            if (oMHRCU == null) return;
            if (oMHRCU.MSKeys == null || oMHRCU.MSKeys.Count == 0) return;

            oMHRCU.MSKeys[e.Index].bytReflag = Convert.ToByte(e.NewValue);
        }

        private void sbMin_Scroll(object sender, ScrollEventArgs e)
        {

        }

        private void sbMin_ValueChanged_1(object sender, EventArgs e)
        {
            lbv4.Text = sbMin.Value.ToString();
            if (MyBlnReading) return;
            if (oMHRCU == null) return;
            oMHRCU.dimmerLow = byte.Parse(sbMin.Value.ToString());
        }

        private void dgvKey_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (oMHRCU == null) return;
                if (oMHRCU.MSKeys == null) return;
                if ((e.RowIndex == -1) || (e.ColumnIndex == -1)) return;
                if (dgvKey[e.ColumnIndex, e.RowIndex].Value == null) return;
                if (dgvKey.SelectedRows.Count == 0) return;

                if (dgvKey[e.ColumnIndex, e.RowIndex].Value == null) dgvKey[e.ColumnIndex, e.RowIndex].Value = "";
                String Remark = dgvKey[e.ColumnIndex, e.RowIndex].Value.ToString();

                for (int i = 0; i < dgvKey.SelectedRows.Count; i++)
                {
                    dgvKey.SelectedRows[i].Cells[e.ColumnIndex].Value = Remark;

                    string strTmp = "";
                    int RowID = dgvKey.SelectedRows[i].Index;
                    switch (e.ColumnIndex + 1)
                    {
                        case 2:
                            strTmp = dgvKey[e.ColumnIndex, RowID].Value.ToString();
                            oMHRCU.MSKeys[RowID].Remark = dgvKey[e.ColumnIndex, RowID].Value.ToString();
                            break;
                        case 3:
                            oMHRCU.MSKeys[RowID].Mode = DryMode.ConvertorKeyModesToPublicModeGroup(dgvKey[e.ColumnIndex, RowID].Value.ToString());
                            dgvKey[3, RowID].ReadOnly = (oMHRCU.MSKeys[RowID].Mode != 0);
                            dgvKey[4, RowID].ReadOnly = (oMHRCU.MSKeys[RowID].Mode != 0);
                            if (oMHRCU.MSKeys[RowID].Mode == 0)
                            {
                                String OnDelay = HDLPF.GetStringFromTime(oMHRCU.MSKeys[RowID].ONOFFDelay[0], ":");
                                String OffDelay = HDLPF.GetStringFromTime(oMHRCU.MSKeys[RowID].ONOFFDelay[1], ":");
                                dgvKey[3, RowID].Value = OnDelay;
                                dgvKey[4, RowID].Value = OffDelay;
                            }
                            else
                            {
                                dgvKey[3, e.RowIndex].Value = CsConst.mstrInvalid;
                                dgvKey[4, e.RowIndex].Value = CsConst.mstrInvalid;
                            }
                            break;
                        case 4:
                            int DelayTime = HDLPF.GetTimeIntegerFromString(dgvKey[e.ColumnIndex, RowID].Value.ToString(), ':');
                            oMHRCU.MSKeys[RowID].ONOFFDelay[0] = DelayTime;
                            break;

                        case 5:
                            DelayTime = HDLPF.GetTimeIntegerFromString(dgvKey[e.ColumnIndex, RowID].Value.ToString(), ':');
                            oMHRCU.MSKeys[RowID].ONOFFDelay[1] = DelayTime;
                            break;
                        case 6:
                            byte[] bytTmp = new byte[4];
                            bytTmp[0] = (byte)(dgvKey.SelectedRows[i].Index + 1);
                            bytTmp[2] = 0;
                            bytTmp[3] = 0;


                            if (dgvKey[e.ColumnIndex, dgvKey.SelectedRows[i].Index].Value.ToString().ToLower() == "true")
                                bytTmp[1] = Convert.ToByte(dgvKey[5, dgvKey.SelectedRows[i].Index].Value.ToString());
                            else
                                bytTmp[1] = 0;

                            Cursor.Current = Cursors.WaitCursor;
                            if (CsConst.mySends.AddBufToSndList(bytTmp, 0x0031, SubNetID, DevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(MyintDeviceType)) == false)
                            {
                                //DgChns.BeginInvoke(new SetvalueHandle(Setvalue), e.RowIndex);
                            }

                            Cursor.Current = Cursors.Default;
                            break;
                    }
                }
            }
            catch
            {
            }
        }

        private void dgvKey_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            txtScene.Visible = false;
            if (dgvKey.SelectedRows == null) return;
            if (dgvKey.Rows == null) return;
            if (e.RowIndex >= 0)
            {
                Byte Mode = DryMode.ConvertorKeyModesToPublicModeGroup(dgvKey[2, e.RowIndex].Value.ToString());
                dgvKey[3, e.RowIndex].ReadOnly = (Mode != 0);
                dgvKey[4, e.RowIndex].ReadOnly = (Mode != 0);
                if (dgvKey[3, e.RowIndex].ReadOnly == true)
                {
                    dgvKey[3, e.RowIndex].Value = CsConst.mstrInvalid;
                    dgvKey[4, e.RowIndex].Value = CsConst.mstrInvalid;
                }

                if (e.ColumnIndex == 3 || e.ColumnIndex == 4)
                {
                    if (dgvKey[e.ColumnIndex, e.RowIndex].Value.ToString() == CsConst.mstrInvalid) return;
                    txtScene.Text = HDLPF.GetTimeFromString(dgvKey[e.ColumnIndex, e.RowIndex].Value.ToString(), ':');
                    HDLSysPF.addcontrols(e.ColumnIndex, e.RowIndex, txtScene, dgvKey);
                }
            }
        }

        private void dgvKey_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            dgvKey.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void btnSaveAndClose12_Click(object sender, EventArgs e)
        {
            btnSvaeSec_Click(btnSave12, null);
            this.Close();
        }

    }
}
