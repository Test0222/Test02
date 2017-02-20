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
    public partial class frmCurtain : Form
    {
        [DllImport("user32.dll")]
        public static extern Boolean FlashWindow(IntPtr hwnd, Boolean bInvert);

        private Curtain oCurtain = null;
        private string MyDevName = null;
        private int myintDIndex = -1;
        private int mywdDeviceType = -1;
        private byte SubNetID;
        private byte DevID;
        TimeText txtSeries1 = new TimeText(":");
        TimeMs txtTimeMs = new TimeMs();
        private bool isRead = false;
        private byte[] arayTest = new byte[4];
        public frmCurtain()
        {
            InitializeComponent();
        }

        public frmCurtain(Curtain oCur,string strName, int intDIndex, int wdDeviceType)
        {
            InitializeComponent();

            MyDevName = strName;
            this.oCurtain = oCur;
            this.myintDIndex = intDIndex;
            string strDevName = strName.Split('\\')[0].ToString();
            this.mywdDeviceType = wdDeviceType;

            HDLSysPF.DisplayDeviceNameModeDescription(strName, mywdDeviceType, cboDevice, tbModel, tbDescription);

            SubNetID = Convert.ToByte(strDevName.Split('-')[0]);
            DevID = Convert.ToByte(strDevName.Split('-')[1]);
            tsl3.Text = strName;

        }

        private void frmCurtain_Load(object sender, EventArgs e)
        {
            InitialFormCtrlsTextOrItems();        
        }


        void SetCtrlsVisbleWithDifferentDeviceType()
        {
            if (oCurtain == null) return;
            toolStrip1.Visible = (CsConst.MyEditMode == 0);
            if (CurtainDeviceType.NormalCurtainG1DeviceType.Contains(mywdDeviceType)) // 第一代窗帘模块
            {
                tabControl1.TabPages.Remove(tabRoll);
                tabControl1.TabPages.Remove(tabMotor);
                tabControl1.TabPages.Remove(tabTest);
                grpTime.Visible = false;
                panel1.Visible = false;
            }
            else if (CurtainDeviceType.CurtainG2DeviceType.Contains(mywdDeviceType)) // 是不是第二代窗帘模块
            {
                tabControl1.TabPages.Remove(tabRoll);
                tabControl1.TabPages.Remove(tabMotor);
                tabControl1.TabPages.Remove(tabTest);
                grpTime.Visible = true;
            }
            else if (CurtainDeviceType.RollerCurtainDeviceType.Contains(mywdDeviceType)) // 是不是roll电机
            {
                tabControl1.TabPages.Remove(tabBasic);
                tabControl1.TabPages.Remove(tabMotor);
                grpTime.Visible = false;
            }
            else if (CurtainDeviceType.NormalMotorCurtainDeviceType.Contains(mywdDeviceType)) // 是不是开合帘
            {
                tabControl1.TabPages.Remove(tabBasic);
                tabControl1.TabPages.Remove(tabRoll);
                grpTime.Visible = false;
            }
            cl5.Visible = (mywdDeviceType != 707);
        }

        void InitialFormCtrlsTextOrItems()
        {
            arayTest = new byte[] { 1, 1, 1, 1 };
            cbTest.Items.Clear();
            cbTest.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00036", ""));
            cbTest.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00037", ""));
            cbTest.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00038", ""));
            for (int i = 1; i <= 100; i++)
                cbTest.Items.Add(i.ToString() + " %");
            cbTest.SelectedIndex = 0;
        }

        void LoadBasicInformationToForm()
        {
            if (oCurtain == null) return;
            if (oCurtain.Curtains == null) return;
            try
            {
                if (tabControl1.SelectedTab.Name == "tabBasic")
                {
                    isRead = true;
                    int i = 1;
                    #region
                    lbRead.Rows.Clear();
                    foreach (BasicCurtain ch in oCurtain.Curtains)
                    {
                        string strTmp = null;
                        if (i % 2 !=0)
                        {
                            strTmp = CsConst.mstrINIDefault.IniReadValue("CurtainFlags", "00000", "");
                        }
                        else
                        {
                            strTmp = CsConst.mstrINIDefault.IniReadValue("CurtainFlags", "00001", "");
                        }

                        object[] boj = null;

                        if (mywdDeviceType == 706 || mywdDeviceType == 707 || CurtainDeviceType.CurtainG2DeviceType.Contains(mywdDeviceType))
                        {
                            boj = new object[] { (i + 1) / 2 + "---" + strTmp, ch.remark,HDLPF.GetStringFrmTimeMs(ch.runTime),
                                                              HDLPF.GetStringFrmTimeMs(ch.onDelay),
                                                              HDLPF.GetStringFrmTimeMs(ch.offDelay),strTmp };
                        }
                        else
                        {
                            boj = new object[] { (i + 1) / 2 + "---" + strTmp, ch.remark,HDLPF.GetStringFromTime(ch.runTime,":"),
                                                              HDLPF.GetStringFromTime(ch.onDelay,":"),
                                                              HDLPF.GetStringFromTime(ch.offDelay,":"),strTmp };
                        }
                        lbRead.Rows.Add(boj);
                        i++;
                    }
                    if (grpTime.Visible)
                    {
                        Time1.Text = oCurtain.intJogTime.ToString();
                        Time2.Text = oCurtain.intJogTime1.ToString();
                        Time3.Text = oCurtain.intJogTime2.ToString();
                        Time4.Text = oCurtain.intJogTime3.ToString();
                    }
                    #endregion
                }
                else if (tabControl1.SelectedTab.Name == "tabRoll")
                {
                    #region
                    if (oCurtain.bytInvert == 0) rb1.Checked = true;
                    else rb2.Checked = true;
                    #endregion
                }
                else if (tabControl1.SelectedTab.Name == "tabMotor")
                {
                    #region
                    if (oCurtain.bytInvert == 0) radioButton2.Checked = true;
                    else radioButton1.Checked = true;

                    chbRestore.Checked = (oCurtain.bytAutoMeasure == 0);


                    if (oCurtain.bytDragMode == 0) rb31.Checked = true;
                    else if (oCurtain.bytDragMode == 1) rb32.Checked = true;
                    else rb33.Checked = true;

                    if (oCurtain.intDragLong < 5) oCurtain.intDragLong = 5;
                    if (oCurtain.intDragShort < 5) oCurtain.intDragShort = 5;
                    if (oCurtain.intDragSafe < 5) oCurtain.intDragSafe = 5;

                    numlength1.Value = oCurtain.intDragLong;
                    numlength2.Value = oCurtain.intDragShort;
                    numlength3.Value = oCurtain.intDragSafe;

                    #endregion
                }
            }
            catch
            {
            }
            isRead = false;
        }

        private void DgCurtain_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            txtSeries1.Visible = false;
            txtTimeMs.Visible = false;
            if (mywdDeviceType == 706 || mywdDeviceType == 707 || mywdDeviceType == 713|| mywdDeviceType ==719)
            {
                if (e.ColumnIndex != 1 && e.ColumnIndex != 0 && e.ColumnIndex != 5)
                {
                    lbRead.Controls.Add(txtTimeMs);
                    txtTimeMs.Text = HDLPF.GetTimeFrmStringMs(lbRead[e.ColumnIndex, e.RowIndex].Value.ToString());
                    txtTimeMs.Show();
                    txtTimeMs.Visible = true;
                    Rectangle rect = lbRead.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true);
                    txtTimeMs.Size = rect.Size;
                    txtTimeMs.Top = rect.Top;
                    txtTimeMs.Left = rect.Left;
                    txtTimeMs.TextChanged += txtTimeMs_TextChanged;
                }
            }
            else
            {
                if (e.ColumnIndex != 1 && e.ColumnIndex != 0 && e.ColumnIndex != 5)
                {
                    lbRead.Controls.Add(txtSeries1);
                    txtSeries1.Text = HDLPF.GetTimeFromString(lbRead[e.ColumnIndex, e.RowIndex].Value.ToString(), ':');
                    txtSeries1.Show();
                    txtSeries1.Visible = true;
                    Rectangle rect = lbRead.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true);
                    txtSeries1.Size = rect.Size;
                    txtSeries1.Top = rect.Top;
                    txtSeries1.Left = rect.Left;
                    txtSeries1.TextChanged += new EventHandler(txtScene_TextChanged);
                }
            }
        }

        void txtTimeMs_TextChanged(object sender, EventArgs e)
        {
            if ((lbRead.CurrentCell.RowIndex == -1) || (lbRead.CurrentCell.ColumnIndex == -1)) return;
            string str = HDLPF.GetStringFrmTimeMs(int.Parse(txtTimeMs.Text.ToString()));
            lbRead[lbRead.CurrentCell.ColumnIndex, lbRead.CurrentCell.RowIndex].Value = str;

        }

        void txtScene_TextChanged(object sender, EventArgs e)
        {
            if ((lbRead.CurrentCell.RowIndex == -1) || (lbRead.CurrentCell.ColumnIndex == -1)) return;
            lbRead[lbRead.CurrentCell.ColumnIndex, lbRead.CurrentCell.RowIndex].Value = HDLPF.GetStringFromTime(int.Parse(txtSeries1.Text.ToString()), ":");

        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            if (oCurtain == null) return;
            Cursor.Current = Cursors.WaitCursor;
            oCurtain.SaveCurtainToDB();
            Cursor.Current = Cursors.Default;
        }

        private void frmCurtain_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                FlashWindow(this.Handle, true);
            }
        }

        private void DgCurtain_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            lbRead.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        delegate void SetvalueHandle(int rowIndex);

        private void Setvalue(int rowIndex)
        {
            lbRead.CellValueChanged -= new DataGridViewCellEventHandler(DgCurtain_CellValueChanged);

            System.Threading.Thread.Sleep(50);
            List<int> values = new List<int>();

            for (int i = 0; i < lbRead.Rows.Count; i++)
            {
                if (lbRead[5, i].Value.ToString().ToLower() == "true")
                {
                    if (i != rowIndex)
                        values.Add(i);
                }
            }
            lbRead.Rows.Clear();
            LoadBasicInformationToForm();
            for (int i = 0; i < lbRead.Rows.Count; i++)
            {
                if (values.Contains(i))
                {
                    lbRead[5, i].Value = true;
                }
            }
            lbRead[0, 0].Selected = false;
            lbRead[5, rowIndex].Selected = true;

            lbRead.CellValueChanged += new DataGridViewCellEventHandler(DgCurtain_CellValueChanged);
        }

        private void DgCurtain_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1) return;
            if (e.ColumnIndex == -1) return;
            if (lbRead.RowCount == 0) return;
            if (lbRead[e.ColumnIndex, e.RowIndex].Value == null) lbRead[e.ColumnIndex, e.RowIndex].Value = "";
            try
            {
                int intIndex = e.RowIndex;  //窗帘号下标

                for (int i = 0; i < lbRead.SelectedRows.Count; i++)
                {
                    //lbRead.SelectedRows[i].Cells[e.ColumnIndex].Value = lbRead[e.ColumnIndex, e.RowIndex].Value.ToString();

                    if (e.ColumnIndex == 1)   //Name
                    {
                        if (lbRead[1, e.RowIndex].Value == null)
                        {
                            lbRead[1, e.RowIndex].Value = "";
                        }
                        string strTmp = lbRead[1, e.RowIndex].Value.ToString();
                        oCurtain.Curtains[intIndex].remark = strTmp;
                    }
                    else
                    {
                        if (mywdDeviceType == 706 || mywdDeviceType == 707 || mywdDeviceType == 713 || mywdDeviceType == 719)
                        {
                        }
                        else
                        {
                            if (e.ColumnIndex != 5)
                            {
                                lbRead[e.ColumnIndex, e.RowIndex].Value = HDLPF.GetStringFromTime(int.Parse(txtSeries1.Text.ToString()), ":");
                            }
                        }
                        string strTmp = lbRead[e.ColumnIndex, e.RowIndex].Value.ToString();
                        switch (e.ColumnIndex)
                        {
                            case 2:
                                if (mywdDeviceType == 706 || mywdDeviceType == 707 || mywdDeviceType == 713 || mywdDeviceType == 719)
                                {
                                    oCurtain.Curtains[intIndex].runTime = int.Parse(HDLPF.GetTimeFrmStringMs(strTmp));
                                }
                                else
                                {
                                    oCurtain.Curtains[intIndex].runTime = int.Parse(HDLPF.GetTimeFromString(strTmp, ':'));
                                }
                                break;
                            case 3:
                                if (mywdDeviceType == 706 || mywdDeviceType == 707 || mywdDeviceType == 713 || mywdDeviceType == 719)
                                {
                                    oCurtain.Curtains[intIndex].onDelay = int.Parse(HDLPF.GetTimeFrmStringMs(strTmp));
                                }
                                else
                                {
                                    oCurtain.Curtains[intIndex].onDelay = int.Parse(HDLPF.GetTimeFromString(strTmp, ':'));
                                }
                                break;
                            case 4:
                                if (mywdDeviceType == 706 || mywdDeviceType == 707 || mywdDeviceType == 713 || mywdDeviceType == 719)
                                {
                                    oCurtain.Curtains[intIndex].offDelay = int.Parse(HDLPF.GetTimeFrmStringMs(strTmp));
                                }
                                else
                                {
                                    oCurtain.Curtains[intIndex].offDelay = int.Parse(HDLPF.GetTimeFromString(strTmp, ':'));
                                }
                                break;
                        }
                    }

                    lbRead.SelectedRows[i].Cells[e.ColumnIndex].Value = lbRead[e.ColumnIndex, e.RowIndex].Value.ToString();
                }
            }
            catch
            {
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
                    // SetVisableForDownOrUpload(false);
                    // ReadDownLoadThread();  //增加线程，使得当前窗体的任何操作不被限制

                    CsConst.MyUPload2DownLists = new List<byte[]>();

                    string strName = MyDevName.Split('\\')[0].ToString();
                    byte bytSubID = byte.Parse(strName.Split('-')[0]);
                    byte bytDevID = byte.Parse(strName.Split('-')[1]);

                    byte[] ArayRelay = new byte[] { bytSubID, bytDevID, (byte)(mywdDeviceType / 256), (byte)(mywdDeviceType % 256), 
                        (byte)0 ,(byte)(myintDIndex / 256), (byte)(myintDIndex % 256),};
                    CsConst.MyUPload2DownLists.Add(ArayRelay);
                    CsConst.MyUpload2Down = Convert.ToByte(((ToolStripButton)sender).Tag.ToString());
                    FrmDownloadShow Frm = new FrmDownloadShow();
                    if (bytTag ==0) Frm.FormClosing += new FormClosingEventHandler(UpdateDisplayInformationAccordingly);
                    Frm.ShowDialog();
                }
            }
            catch
            {
            }
        }

        void UpdateDisplayInformationAccordingly(object sender, FormClosingEventArgs e)
        {
            LoadBasicInformationToForm();
            this.BringToFront();
        }


        private void toolStripButton11_Click(object sender, EventArgs e)
        {
            if (oCurtain == null) return;

        }

        private void frmCurtain_Shown(object sender, EventArgs e)
        {
            SetCtrlsVisbleWithDifferentDeviceType();
            if (CsConst.MyEditMode == 0)
            {
                LoadBasicInformationToForm();
            }
            else if (CsConst.MyEditMode == 1)
            {
                if (oCurtain.MyRead2UpFlags[0] == false)
                {
                    tslRead_Click(tslRead, null);
                }
                else
                {
                    LoadBasicInformationToForm();
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            tslRead_Click(toolStripLabel2, null);
        }

        private void btnSaveTime_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                byte[] arayTmp = new byte[3];
                arayTmp[0] = 1;
                arayTmp[1] = byte.Parse((oCurtain.intJogTime / 256).ToString());
                arayTmp[2] = byte.Parse((oCurtain.intJogTime % 256).ToString());
                if (CsConst.mySends.AddBufToSndList(arayTmp, 0x1C74, SubNetID, DevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(mywdDeviceType)) == false) return;
                HDLUDP.TimeBetwnNext(3);

                //if new curtain, add jog time to it
                arayTmp[0] = 2;
                arayTmp[1] = byte.Parse((oCurtain.intJogTime1 / 256).ToString());
                arayTmp[2] = byte.Parse((oCurtain.intJogTime1 % 256).ToString());
                if (CsConst.mySends.AddBufToSndList(arayTmp, 0x1C74, SubNetID, DevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(mywdDeviceType)) == false) return;
                HDLUDP.TimeBetwnNext(20);
                Cursor.Current = Cursors.Default;

                arayTmp[0] = 3;
                arayTmp[1] = byte.Parse((oCurtain.intJogTime2 / 256).ToString());
                arayTmp[2] = byte.Parse((oCurtain.intJogTime2 % 256).ToString());
                if (CsConst.mySends.AddBufToSndList(arayTmp, 0x1C74, SubNetID, DevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(mywdDeviceType)) == false) return;
                HDLUDP.TimeBetwnNext(20);
                Cursor.Current = Cursors.Default;

                arayTmp[0] = 4;
                arayTmp[1] = byte.Parse((oCurtain.intJogTime3 / 256).ToString());
                arayTmp[2] = byte.Parse((oCurtain.intJogTime3 % 256).ToString());
                if (CsConst.mySends.AddBufToSndList(arayTmp, 0x1C74, SubNetID, DevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(mywdDeviceType)) == false) return;
                HDLUDP.TimeBetwnNext(20);
                Cursor.Current = Cursors.Default;
            }
            catch
            {
            }
            Cursor.Current = Cursors.Default;
        }

        private void Time1_TextChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            if (oCurtain == null) return;
            oCurtain.intJogTime = int.Parse(Time1.Text);
        }

        private void Time2_TextChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            if (oCurtain == null) return;
            oCurtain.intJogTime1 = int.Parse(Time2.Text);
        }

        private void lbRead_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 5)
            {
                string strName = MyDevName.Split('\\')[0].ToString();
                byte bytSubID = byte.Parse(strName.Split('-')[0]);
                byte bytDevID = byte.Parse(strName.Split('-')[1]);

                byte[] bytTmp = new byte[2];

                bytTmp[0] =(Byte)(e.RowIndex / 2 + 1);
                bytTmp[1] = (Byte)(e.RowIndex % 2 + 1);
                
                Cursor.Current = Cursors.WaitCursor;
                if (CsConst.mySends.AddBufToSndList(bytTmp, 0xE3E0, bytSubID, bytDevID, false, false, false, CsConst.minAllWirelessDeviceType.Contains(mywdDeviceType)) == false)
                {
                    CsConst.myRevBuf = new byte[1200];
                }
                Cursor.Current = Cursors.Default;
            }
        }

        private void tmRead_Click(object sender, EventArgs e)
        {
            tslRead_Click(tslRead, null);
        }

        private void tmUpload_Click(object sender, EventArgs e)
        {
            tslRead_Click(toolStripLabel2, null);
        }

        private void btnSaveAndClose1_Click(object sender, EventArgs e)
        {
            btnSave_Click(null, null);
            this.Close();
        }

        private void timeMs2_TextChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            if (oCurtain == null) return;
            oCurtain.intJogTime2 = int.Parse(Time3.Text);
        }

        private void timeMs1_TextChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            if (oCurtain == null) return;
            oCurtain.intJogTime3 = int.Parse(Time4.Text);
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            bool isSureOpen = false;
            byte[] arayTmp = new byte[1];
            arayTmp[0] = 3;
            if (CsConst.mySends.AddBufToSndList(arayTmp, 0x1F0C, SubNetID, DevID, false, false, false, CsConst.minAllWirelessDeviceType.Contains(mywdDeviceType)) == true)
            {
                if (CsConst.myRevBuf[26] == 3) isSureOpen = true;
                if (isSureOpen)
                {
                    btnOpen.Enabled = false;
                }
            }
            else
            {
                Cursor.Current = Cursors.Default;
                return;
            }
            CsConst.myRevBuf = new byte[1200];
            Cursor.Current = Cursors.Default;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(CsConst.mstrINIDefault.IniReadValue("Public", "99785", ""),
                                                CsConst.mstrINIDefault.IniReadValue("Public", "99784", "")
                                                , MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
            if (result == DialogResult.OK)
            {
                Cursor.Current = Cursors.WaitCursor;
                byte[] arayTmp = new byte[2];
                arayTmp[0] = 17;
                arayTmp[1] = 238;
                if (CsConst.mySends.AddBufToSndList(arayTmp, 0xE3E0, SubNetID, DevID, false, false, false, CsConst.minAllWirelessDeviceType.Contains(mywdDeviceType)) == false)
                {
                    Cursor.Current = Cursors.Default;
                    return;
                }
                CsConst.myRevBuf = new byte[1200];
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnrefence_Click(object sender, EventArgs e)
        {
            byte[] arayTmp = new byte[1];
            arayTmp[0] = 17;
            if (CsConst.mySends.AddBufToSndList(arayTmp, 0xE3E2, SubNetID, DevID, false, false, true, CsConst.minAllWirelessDeviceType.Contains(mywdDeviceType)) == false)
            {
                Cursor.Current = Cursors.Default;
                return;
            }
            if (CsConst.myRevBuf[26] <= 100)
                lbPercent.Text = CsConst.myRevBuf[26].ToString() + "%";
            else if (CsConst.myRevBuf[26] == 0xDD)
                lbPercent.Text = CsConst.mstrINIDefault.IniReadValue("Public", "99905", "");
            else if (CsConst.myRevBuf[26] == 0xEE)
                lbPercent.Text = CsConst.mstrINIDefault.IniReadValue("Public", "99906", "");
            CsConst.myRevBuf = new byte[1200];

            if (CsConst.mySends.AddBufToSndList(arayTmp, 0x1F08, SubNetID, DevID, false, false, true, CsConst.minAllWirelessDeviceType.Contains(mywdDeviceType)) == false)
            {
                Cursor.Current = Cursors.Default;
                return;
            }
            lbLength.Text = (CsConst.myRevBuf[26] * 256 + CsConst.myRevBuf[27]).ToString() + "(CM)";
            CsConst.myRevBuf = new byte[1200];
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            byte[] arayTmp = new byte[2];
            if (cbTest.SelectedIndex > 2)
            {
                arayTmp[0] = 17;
                arayTmp[1] = Convert.ToByte(cbTest.SelectedIndex - 2);
            }
            else
            {
                arayTmp[0] = 1;
                arayTmp[1] = Convert.ToByte(cbTest.SelectedIndex);
            }
            if (CsConst.mySends.AddBufToSndList(arayTmp, 0xE3E0, SubNetID, DevID, false, false, false, CsConst.minAllWirelessDeviceType.Contains(mywdDeviceType)) == false)
            {
                Cursor.Current = Cursors.Default;
                return;
            }
            //lbPercent.Text = CsConst.myRevBuf[26].ToString() + "%";
            CsConst.myRevBuf = new byte[1200];
            Cursor.Current = Cursors.Default;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            btnClose.Enabled = false;
            byte[] arayTmp = new byte[2];
            arayTmp[0] = 1;
            arayTmp[1] = 2;
            if (CsConst.mySends.AddBufToSndList(arayTmp, 0xE3E0, SubNetID, DevID, false, false, false, CsConst.minAllWirelessDeviceType.Contains(mywdDeviceType)) == true)
            {
            }
            btnClose.Enabled = true;
            CsConst.myRevBuf = new byte[1200];
            Cursor.Current = Cursors.Default;
        }

        private void rb1_CheckedChanged(object sender, EventArgs e)
        {
            if (oCurtain == null) return;
            byte intTag = byte.Parse((sender as RadioButton).Tag.ToString());
            oCurtain.bytInvert = intTag;
        }

        private void chbAuto_CheckedChanged(object sender, EventArgs e)
        {
            CsConst.isAutoRefreshCurtainPercent = chbAuto.Checked;
            if (chbAuto.Checked) UDPReceive.ClearQueueDataForCurtain();
            timer1.Enabled = chbAuto.Checked;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                if (UDPReceive.receiveQueueForCurtain.Count > 0)
                {
                    byte[] arayTmp = UDPReceive.receiveQueueForCurtain.Dequeue();
                    if (arayTmp[21] == 0xE3 && arayTmp[22] == 0xE1 &&
                        arayTmp[17] == SubNetID && arayTmp[18] == DevID)
                    {
                        if (arayTmp[25] == 0x11)
                        {
                            if (arayTmp[26] <= 100)
                            {
                                lbPercent.Text = arayTmp[26].ToString() + "%";
                            }
                            else if (arayTmp[26] == 0xDD)
                                lbPercent.Text = CsConst.mstrINIDefault.IniReadValue("Public", "99905", "");
                            else if (arayTmp[26] == 0xEE)
                                lbPercent.Text = CsConst.mstrINIDefault.IniReadValue("Public", "99906", "");
                            else if (arayTmp[26] == 0xFA)
                            {
                                System.Diagnostics.Debug.WriteLine(arayTmp[21].ToString("X") + " " + arayTmp[22].ToString("X"));
                            }
                        }
                        else if (arayTmp[25] == 0x01)
                        {
                            UDPReceive.ClearQueueDataForCurtain();
                        }
                    }
                }
            }
            catch
            {
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void numlength2_ValueChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            if (oCurtain == null) return;

            int intTag = int.Parse((sender as NumericUpDown).Tag.ToString());
            int intTmp = int.Parse((sender as NumericUpDown).Value.ToString());
            switch (intTag)
            {
                case 0: oCurtain.intDragLong = intTmp; break;
                case 1: oCurtain.intDragShort = intTmp; break;
                case 2: oCurtain.intDragSafe = intTmp; break;
            }
        }

        private void rb31_CheckedChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            if (oCurtain == null) return;

            byte intTag = byte.Parse((sender as RadioButton).Tag.ToString());
            oCurtain.bytDragMode = intTag;
        }

        private void chbRestore_CheckedChanged(object sender, EventArgs e)
        {
            if (isRead) return;
            if (chbRestore.Checked) oCurtain.bytAutoMeasure = 0;
            else oCurtain.bytAutoMeasure = 1;
        }

    }
}
