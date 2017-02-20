using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HDL_Buspro_Setup_Tool
{
    public partial class FrmEditPanlePicture : Form
    {
        private int SelectedText = 0;
        private int ColorSelectedText = 0;
        private Single[] MyMoveLeft = new float[] { 0, 0, 0, 0 };
        private Single[] MyMoveTop = new float[] { 0, 0, 0, 0 };
        private String[] myString = new string[4];
        private Font[] MyFont = new Font[] { new Font("Arial", 12f), new Font("Arial", 12f), new Font("Arial", 12f), new Font("Arial", 12f) };
        private Single[] MyColorMoveLeft = new float[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        private Single[] MyColorMoveTop = new float[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        private String[] myColorString = new string[12];
        private Font[] MyColorFont = new Font[] { new Font("Arial", 12f), new Font("Arial", 12f), new Font("Arial", 12f), new Font("Arial", 12f),
                                             new Font("Arial", 12f), new Font("Arial", 12f), new Font("Arial", 12f), new Font("Arial", 12f),
                                             new Font("Arial", 12f), new Font("Arial", 12f), new Font("Arial", 12f), new Font("Arial", 12f)};
        private PictureBox myPicTemp = null;
        private byte[] arayPosition = new byte[7] { 1, 2, 3, 4, 5, 6, 7 };
        private bool isRead = false;
        FrmProcess frmProcess;
        public FrmEditPanlePicture()
        {
            InitializeComponent();
        }

        private void FrmEditPanlePicture_Load(object sender, EventArgs e)
        {
            isRead = true;
            tabControl1.TabPages.Remove(tabHeat);
            cbType.SelectedIndex = 0;
            if (CsConst.myOnlines != null && CsConst.myOnlines.Count > 0)
            {
                dgvDevice.Rows.Clear();
                for (int i = 0; i < CsConst.myOnlines.Count; i++)
                {
                    if (CsConst.mintDLPDeviceType.Contains(CsConst.myOnlines[i].DeviceType) ||
                        EnviroNewDeviceTypeList.EnviroNewPanelDeviceTypeList.Contains(CsConst.myOnlines[i].DeviceType) ||
                        EnviroNewDeviceTypeList.EnviroNewPanelDeviceTypeList.Contains(CsConst.myOnlines[i].DeviceType) ||
                        (CsConst.myOnlines[i].DeviceType == 5003) ||
                        (CsConst.myOnlines[i].DeviceType == 5064))
                    {
                        object[] obj = new object[] { dgvDevice.RowCount+1,CsConst.myOnlines[i].bytSub.ToString(),
                    CsConst.myOnlines[i].bytDev.ToString(),CsConst.myOnlines[i].DevName.Split('\\')[1].ToString(),
                    CsConst.mstrINIDefault.IniReadValue("DeviceType" + CsConst.myOnlines[i].DeviceType.ToString(), "Description", ""),
                    "Read","Upload",CsConst.myOnlines[i].DeviceType.ToString(),CsConst.myOnlines[i].intDIndex.ToString()};
                        dgvDevice.Rows.Add(obj);
                    }
                }
            }
            isRead = false;
        }

        private void SearNode()
        {

            UDPReceive.ClearQueueData();
            if (CsConst.myOnlines == null) CsConst.myOnlines = new List<DevOnLine>();
            CsConst.calculationWorker = new BackgroundWorker();
            CsConst.calculationWorker.DoWork += new DoWorkEventHandler(calculationWorker_DoWork);
            CsConst.calculationWorker.ProgressChanged += new ProgressChangedEventHandler(calculationWorker_ProgressChanged);
            CsConst.calculationWorker.WorkerReportsProgress = true;
            CsConst.calculationWorker.WorkerSupportsCancellation = true;
            CsConst.calculationWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(calculationWorker_RunWorkerCompleted);
            CsConst.calculationWorker.RunWorkerAsync();
            frmProcess = new FrmProcess();
            frmProcess.ShowDialog();
        }

        void calculationWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                #region
                dgvDevice.Rows.Clear();
                if (CsConst.myOnlines != null && CsConst.myOnlines.Count > 0)
                {
                    for (int i = 0; i < CsConst.myOnlines.Count; i++)
                    {
                        if (CsConst.mintDLPDeviceType.Contains(CsConst.myOnlines[i].DeviceType)
                         || EnviroDLPPanelDeviceTypeList.HDLEnviroDLPPanelDeviceTypeList.Contains(CsConst.myOnlines[i].DeviceType)
                         || EnviroNewDeviceTypeList.EnviroNewPanelDeviceTypeList.Contains(CsConst.myOnlines[i].DeviceType)
                         || CsConst.myOnlines[i].DeviceType == 5003 ||
                            CsConst.myOnlines[i].DeviceType == 5064)
                        {
                            bool isAdd = true;
                            if (CsConst.isKamanli)
                            {
                                if (!CsConst.KeManLiDeviceTypeList.Contains(CsConst.myOnlines[i].DeviceType))
                                {
                                    isAdd = false;
                                }
                            }
                            if (isAdd)
                            {
                                object[] obj = new object[] { dgvDevice.RowCount+1,CsConst.myOnlines[i].bytSub.ToString(),
                    CsConst.myOnlines[i].bytDev.ToString(),CsConst.myOnlines[i].DevName.Split('\\')[1].ToString(),
                    CsConst.mstrINIDefault.IniReadValue("DeviceType" + CsConst.myOnlines[i].DeviceType.ToString(), "Description", ""),
                    "Read","Upload",CsConst.myOnlines[i].DeviceType.ToString(),CsConst.myOnlines[i].intDIndex.ToString()};
                                dgvDevice.Rows.Add(obj);
                            }
                        }
                    }
                }
                frmProcess.Close();
                #endregion
            }
            catch
            {
            }
        }

        void calculationWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            try
            {

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
                DateTime d1, d2;
                int SendTime = 0;
                ReSend:
                int intCMD = 0x000E;
                CsConst.mySends.AddBufToSndList(null, intCMD, 255, 255, false, false, false, false);
                d1 = DateTime.Now;
                d2 = DateTime.Now;
                while (SendTime < 3)
                {
                    d2 = DateTime.Now;
                    if (HDLSysPF.Compare(d2, d1) > 1000)
                    {
                        SendTime = SendTime + 1;
                        goto ReSend;
                    }

                    int index = 1;
                    if (CsConst.myOnlines.Count > 0)
                    {
                        for (int i = 0; i < CsConst.myOnlines.Count; i++)
                        {
                            int intTmp1 = CsConst.myOnlines[i].intDIndex;
                            if (intTmp1 > index) index = intTmp1;
                        }
                    }
                    if (UDPReceive.receiveQueue.Count > 0)
                    {
                        byte[] readData = UDPReceive.receiveQueue.Dequeue();
                        if (readData[21] == 0x00 && readData[22] == 0x0F && readData.Length >= 45)
                        {
                            DevOnLine temp = new DevOnLine();
                            byte[] arayRemark = new byte[20];

                            for (int intI = 0; intI < 20; intI++)
                            {
                                arayRemark[intI] = readData[25 + intI];
                            }
                            string strRemark = HDLPF.Byte2String(arayRemark);
                            temp.bytSub = readData[17];
                            temp.bytDev = readData[18];
                            temp.DevName = temp.bytSub.ToString() + "-" + temp.bytDev.ToString() + "\\" + strRemark.ToString();
                            temp.DeviceType = readData[19] * 256 + readData[20];
                            temp.strVersion = "Unread";

                            if (CsConst.myOnlines.Count > 0)
                            {
                                bool isAdd = true;
                                foreach (DevOnLine tmp in CsConst.myOnlines)
                                {
                                    if (temp.DevName == tmp.DevName && temp.bytSub == tmp.bytSub &&
                                        temp.bytDev == tmp.bytDev && temp.DeviceType == tmp.DeviceType)
                                    {
                                        isAdd = false;
                                        break;
                                    }
                                }
                                if (isAdd)
                                {
                                    index = index + 1;
                                    temp.intDIndex = index;
                                    CsConst.myOnlines.Add(temp);
                                    HDLSysPF.AddItsDefaultSettings(temp.DeviceType, temp.intDIndex, temp.DevName);
                                }
                            }
                            else
                            {
                                temp.intDIndex = 1;
                                CsConst.myOnlines.Add(temp);
                                HDLSysPF.AddItsDefaultSettings(temp.DeviceType, temp.intDIndex, temp.DevName);
                            }
                        }

                    }
                }
                #endregion
            }
            catch
            {
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            SearNode();
        }

        private void cbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            chb1.Checked = true;
            chb2.Checked = true;
            chb3.Checked = true;
            chb4.Checked = true;
            chb5.Checked = true;
            txtDIY1.Text = "";
            txtDIY2.Text = "";
            txtDIY3.Text = "";
            txtDIY4.Text = "";
            for (int i = 1; i <= 4; i++)
            {
                System.Windows.Forms.Panel temp = this.Controls.Find("pnlDIY" + i.ToString(), true)[0] as System.Windows.Forms.Panel;
                TextBox temp2 = this.Controls.Find("txtDIY" + i.ToString(), true)[0] as TextBox;
                PictureBox temp3 = this.Controls.Find("picDIYTmp" + i.ToString(), true)[0] as PictureBox;
                PictureBox temp4 = this.Controls.Find("picDIY" + i.ToString(), true)[0] as PictureBox;
                if (cbType.SelectedIndex == 0)
                {
                    #region
                    temp.Width = 40;
                    temp.Height = 32;
                    temp2.Width = 40;
                    temp2.Height = 32;
                    temp3.Width = 40;
                    temp3.Height = 32;
                    if (i == 1)
                    {
                        temp.Top = 43;
                        temp2.Top = 43;
                        temp3.Top = 43;
                    }
                    else
                    {
                        temp.Top = 43 + 32 * (i - 1);
                        temp2.Top = 43 + 32 * (i - 1);
                        temp3.Top = 43 + 32 * (i - 1);
                    }
                    temp3.Image = Image.FromFile(Application.StartupPath + @"\Icon\DLP\Empty1.bmp");
                    temp4.Image = Image.FromFile(Application.StartupPath + @"\Icon\DLP\Empty1.bmp");
                    #endregion
                }
                else if (cbType.SelectedIndex == 1)
                {
                    #region
                    temp.Width = 80;
                    temp.Height = 32;
                    temp2.Width = 80;
                    temp2.Height = 32;
                    temp3.Width = 80;
                    temp3.Height = 32;
                    if (i == 1)
                    {
                        temp.Top = 43;
                        temp2.Top = 43;
                        temp3.Top = 43;
                    }
                    else
                    {
                        temp.Top = 43 + 32 * (i - 1);
                        temp2.Top = 43 + 32 * (i - 1);
                        temp3.Top = 43 + 32 * (i - 1);
                    }
                    temp3.Image = Image.FromFile(Application.StartupPath + @"\Icon\DLP\Empty2.bmp");
                    temp4.Image = Image.FromFile(Application.StartupPath + @"\Icon\DLP\Empty2.bmp");
                    #endregion
                }
                else if (cbType.SelectedIndex == 2)
                {
                    #region
                    temp.Width = 40;
                    temp.Height = 48;
                    temp2.Width = 40;
                    temp2.Height = 48;
                    temp3.Width = 40;
                    temp3.Height = 48;
                    if (i == 1)
                    {
                        temp.Top = 43;
                        temp2.Top = 43;
                        temp3.Top = 43;
                    }
                    else
                    {
                        temp.Top = 43 + 48 * (i - 1);
                        temp2.Top = 43 + 48 * (i - 1);
                        temp3.Top = 43 + 48 * (i - 1);
                    }
                    temp3.Image = Image.FromFile(Application.StartupPath + @"\Icon\DLP\Empty3.bmp");
                    temp4.Image = Image.FromFile(Application.StartupPath + @"\Icon\DLP\Empty3.bmp");
                    #endregion
                }
                else if (cbType.SelectedIndex == 3)
                {
                    #region
                    temp.Width = 80;
                    temp.Height = 48;
                    temp2.Width = 80;
                    temp2.Height = 48;
                    temp3.Width = 80;
                    temp3.Height = 48;
                    if (i == 1)
                    {
                        temp.Top = 43;
                        temp2.Top = 43;
                        temp3.Top = 43;
                    }
                    else
                    {
                        temp.Top = 43 + 48 * (i - 1);
                        temp2.Top = 43 + 48 * (i - 1);
                        temp3.Top = 43 + 48 * (i - 1);
                    }
                    temp3.Image = Image.FromFile(Application.StartupPath + @"\Icon\DLP\Empty4.bmp");
                    temp4.Image = Image.FromFile(Application.StartupPath + @"\Icon\DLP\Empty4.bmp");
                    #endregion
                }
                
            }
        }

        private void dgvDevice_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                pnlColor.Visible = false;
                CL6.Visible = true;
                if (dgvDevice.CurrentRow.Index < 0) return;

                for (int i = 0; i < tabBtn.Controls.Count; i++)
                {
                    if (tabBtn.Controls[i] is System.Windows.Forms.Panel)
                    {
                        (tabBtn.Controls[i] as System.Windows.Forms.Panel).Visible = false;
                    }
                }
                tsl3.Text = dgvDevice[1, dgvDevice.CurrentRow.Index].Value.ToString() + "-" + dgvDevice[2, dgvDevice.CurrentRow.Index].Value.ToString()
                          + "/" + dgvDevice[3, dgvDevice.CurrentRow.Index].Value.ToString();
             
                int SelDeviceType = Convert.ToInt32(dgvDevice[7, dgvDevice.CurrentRow.Index].Value.ToString());
                int DIndex = Convert.ToInt32(dgvDevice[8, dgvDevice.CurrentRow.Index].Value.ToString());
                if (CsConst.mintDLPDeviceType.Contains(SelDeviceType) || SelDeviceType == 5003||SelDeviceType==5064)
                {
                    #region
                    pnlColor.Visible = false;
                    pnlDLP.Visible = true;
                    if (SelDeviceType == 5003)
                    {
                        #region
                        CL6.Visible = false;
                        panel2.Visible = false;
                        if (!tabControl1.TabPages.Contains(tabHeat))
                        {
                            tabControl1.TabPages.Add(tabHeat);
                        }
                        if (tabControl1.TabPages.Contains(tabBtn))
                        {
                            tabControl1.TabPages.Remove(tabBtn);
                        }
                        if (tabControl1.TabPages.Contains(tabIcon))
                        {
                            tabControl1.TabPages.Remove(tabIcon);
                        }
                        #endregion
                    }
                    else if (SelDeviceType == 5064)
                    {
                        #region
                        CL6.Visible = false;
                        panel2.Visible = false;
                        if (!tabControl1.TabPages.Contains(tabIcon))
                        {
                            tabControl1.TabPages.Add(tabIcon);
                        }
                        if (tabControl1.TabPages.Contains(tabBtn))
                        {
                            tabControl1.TabPages.Remove(tabBtn);
                        }
                        if (tabControl1.TabPages.Contains(tabHeat))
                        {
                            tabControl1.TabPages.Remove(tabHeat);
                        }
                        #endregion
                    }
                    else
                    {
                        #region
                        CL6.Visible = true;
                        panel2.Visible = true;
                        if (!tabControl1.TabPages.Contains(tabBtn))
                        {
                            tabControl1.TabPages.Add(tabBtn);
                        }
                        if (tabControl1.TabPages.Contains(tabHeat))
                        {
                            tabControl1.TabPages.Remove(tabHeat);
                        }
                        if (tabControl1.TabPages.Contains(tabIcon))
                        {
                            tabControl1.TabPages.Remove(tabIcon);
                        }
                        #endregion
                    }
                    if (SelDeviceType != 5003 && SelDeviceType!=5064)
                    {
                        #region
                        Panel pnl = null;
                        for (int i = 0; i < CsConst.myPanels.Count; i++)
                        {
                            if (CsConst.myPanels[i].DIndex == DIndex)
                            {
                                pnl = CsConst.myPanels[i];
                                break;
                            }
                        }
                        if (pnl.IconOFF == null)
                        {
                            if (CsConst.mintNewDLPFHSetupDeviceType.Contains(SelDeviceType))
                            {
                                if (CsConst.mintMPTLDeviceType.Contains(SelDeviceType))
                                {
                                    if (pnl.IconOFF == null && pnl.IconON == null)
                                    {
                                        pnl.IconOFF = new byte[7680 + 1 + 32];
                                        pnl.IconON = new byte[7680 + 1 + 32];
                                    }
                                }
                                else
                                {
                                    if (pnl.IconOFF == null && pnl.IconON == null)
                                    {
                                        pnl.IconOFF = new byte[5120 + 1 + 32];
                                        pnl.IconON = new byte[5120 + 1 + 32];
                                    }
                                }
                            }
                            else
                            {
                                if (pnl.IconOFF == null && pnl.IconON == null)
                                {
                                    pnl.IconOFF = new byte[5120 + 1 + 32];
                                    pnl.IconON = new byte[5120 + 1 + 32];
                                }
                            }
                            addDaulfIcon(SelDeviceType);
                        }
                        else
                        {
                            if (pnl.IconON[pnl.IconON.Length - 1 - 32] == 0)
                            {
                                addDaulfIcon(SelDeviceType);
                            }
                            else
                            {
                                addDaulfIcon(SelDeviceType);
                                showDownloadPanelIcons(DIndex, SelDeviceType);
                            }
                        }
                        #endregion
                    }
                    #endregion
                }
                else if (EnviroDLPPanelDeviceTypeList.HDLEnviroDLPPanelDeviceTypeList.Contains(SelDeviceType))
                {
                    #region
                    CL6.Visible = false;
                    pnlColor.Visible = true;
                    pnlDLP.Visible = false;
                    isRead = true;
                    cboMenu.Items.Clear();
                    for (int i = 0; i < 9; i++)
                        cboMenu.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "0052" + i.ToString(), ""));
                    cboMenu.SelectedIndex = 0;
                    cbIcon.Items.Clear();
                    for (int i = 0; i < 20; i++)
                        cbIcon.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "005" + (i + 30).ToString(), ""));
                    cbIcon.SelectedIndex = 0;
                    isRead = false;
                    cbIcon_SelectedIndexChanged(null, null);
                    #endregion
                }
                else if (EnviroNewDeviceTypeList.EnviroNewPanelDeviceTypeList.Contains(SelDeviceType))
                {
                    #region
                    CL6.Visible = false;
                    pnlColor.Visible = true;
                    pnlDLP.Visible = false;
                    isRead = true;
                    cboMenu.Items.Clear();
                    for (int i = 0; i < 4; i++)
                        cboMenu.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "0068" + i.ToString(), ""));
                    cboMenu.SelectedIndex = 0;
                    cbIcon.Items.Clear();
                    for (int i = 0; i < 10; i++)
                        cbIcon.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "0069" + (i ).ToString(), ""));
                    cbIcon.SelectedIndex = 0;
                    isRead = false;
                    cbIcon_SelectedIndexChanged(null, null);
                    #endregion
                }
            }
            catch
            {
            }
        }

        private void addDaulfIcon(int deivcetype)
        {
            if (CsConst.mintNewDLPFHSetupDeviceType.Contains(deivcetype))
            {
                #region
                for (int i = 1; i <= 4; i++)
                {
                    for (int j = 1; j <= 8; j++)
                    {
                        PictureBox temp1 = this.Controls.Find("pag" + i.ToString() + "OFF" + j.ToString(), true)[0] as PictureBox;
                        PictureBox temp2 = this.Controls.Find("pag" + i.ToString() + "ON" + j.ToString(), true)[0] as PictureBox;
                        System.Windows.Forms.Panel temp3 = this.Controls.Find("pnl" + i.ToString() + "OFF" + j.ToString(), true)[0] as System.Windows.Forms.Panel;
                        System.Windows.Forms.Panel temp4 = this.Controls.Find("pnl" + i.ToString() + "ON" + j.ToString(), true)[0] as System.Windows.Forms.Panel;
                        temp3.Width = 40;
                        temp4.Width = 40;
                        temp3.Visible = true;
                        temp4.Visible = true;
                        if (CsConst.mintMPTLDeviceType.Contains(deivcetype))
                        {
                            temp3.Height = 48;
                            temp4.Height = 48;
                            temp1.Image = Image.FromFile(Application.StartupPath + @"\Icon\DLP\PIC3.bmp");
                            temp2.Image = Image.FromFile(Application.StartupPath + @"\Icon\DLP\PIC3.bmp");
                            temp1.ErrorImage = Image.FromFile(Application.StartupPath + @"\Icon\DLP\PIC3.bmp");
                            temp2.ErrorImage = Image.FromFile(Application.StartupPath + @"\Icon\DLP\PIC3.bmp");
                            temp1.InitialImage = Image.FromFile(Application.StartupPath + @"\Icon\DLP\PIC3.bmp");
                            temp2.InitialImage = Image.FromFile(Application.StartupPath + @"\Icon\DLP\PIC3.bmp");
                            temp1.ErrorImage.Tag = 0;
                            temp2.ErrorImage.Tag = 1;
                            temp1.InitialImage.Tag = j;
                            temp2.InitialImage.Tag = j;
                        }
                        else
                        {
                            temp3.Height = 32;
                            temp4.Height = 32;
                            temp1.Image = Image.FromFile(Application.StartupPath + @"\Icon\DLP\PIC1.bmp");
                            temp2.Image = Image.FromFile(Application.StartupPath + @"\Icon\DLP\PIC1.bmp");
                            temp1.ErrorImage = Image.FromFile(Application.StartupPath + @"\Icon\DLP\PIC1.bmp");
                            temp2.ErrorImage = Image.FromFile(Application.StartupPath + @"\Icon\DLP\PIC1.bmp");
                            temp1.InitialImage = Image.FromFile(Application.StartupPath + @"\Icon\DLP\PIC1.bmp");
                            temp2.InitialImage = Image.FromFile(Application.StartupPath + @"\Icon\DLP\PIC1.bmp");
                            temp1.ErrorImage.Tag = 0;
                            temp2.ErrorImage.Tag = 1;
                            temp1.InitialImage.Tag = j;
                            temp2.InitialImage.Tag = j;
                        }
                        temp1.Image.Tag = i;
                        temp2.Image.Tag = i;
                        if (j >= 5)
                        {
                            temp3.Top = 25 + (j - 5) * temp3.Height;
                            temp4.Top = 25 + (j - 5) * temp4.Height;
                        }
                        else
                        {
                            temp3.Top = 25 + (j - 1) * temp3.Height;
                            temp4.Top = 25 + (j - 1) * temp4.Height;
                        }
                    }
                }
                #endregion
            }
            else
            {
                #region
                for (int i = 1; i <= 4; i++)
                {
                    for (int j = 1; j <= 4; j++)
                    {
                        PictureBox temp1 = this.Controls.Find("pag" + i.ToString() + "OFF" + j.ToString(), true)[0] as PictureBox;
                        PictureBox temp2 = this.Controls.Find("pag" + i.ToString() + "ON" + j.ToString(), true)[0] as PictureBox;
                        System.Windows.Forms.Panel temp3 = this.Controls.Find("pnl" + i.ToString() + "OFF" + j.ToString(), true)[0] as System.Windows.Forms.Panel;
                        System.Windows.Forms.Panel temp4 = this.Controls.Find("pnl" + i.ToString() + "ON" + j.ToString(), true)[0] as System.Windows.Forms.Panel;
                        temp3.Visible = true;
                        temp4.Visible = true;
                        temp1.Image = Image.FromFile(Application.StartupPath + @"\Icon\DLP\PIC2.bmp");
                        temp2.Image = Image.FromFile(Application.StartupPath + @"\Icon\DLP\PIC2.bmp");
                        temp1.ErrorImage = Image.FromFile(Application.StartupPath + @"\Icon\DLP\PIC2.bmp");
                        temp2.ErrorImage = Image.FromFile(Application.StartupPath + @"\Icon\DLP\PIC2.bmp");
                        temp1.InitialImage = Image.FromFile(Application.StartupPath + @"\Icon\DLP\PIC2.bmp");
                        temp2.InitialImage = Image.FromFile(Application.StartupPath + @"\Icon\DLP\PIC2.bmp");
                        temp3.Width = 80;
                        temp3.Height = 32;
                        temp4.Width = 80;
                        temp4.Height = 32;
                        temp1.Image.Tag = i;
                        temp2.Image.Tag = i;
                        temp1.ErrorImage.Tag = 0;
                        temp2.ErrorImage.Tag = 1;
                        temp1.InitialImage.Tag = 0;
                        temp2.InitialImage.Tag = 0;
                        temp3.Top = 25 + (j - 1) * temp3.Height;
                        temp4.Top = 25 + (j - 1) * temp4.Height;
                    }
                }
                #endregion
            }
        }

        

        void temp_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                string MyPath = HDLPF.OpenFileDialog("BMP Image|*.bmp|JPEG Image|*.jpg|PNG Image|*.png", "Add Icons");
                if (MyPath == null || MyPath == "") return;
                Image img = Image.FromFile(MyPath);
                if (img.Width % ((PictureBox)sender).Width != 0 || img.Height % ((PictureBox)sender).Height != 0)
                {
                    MessageBox.Show(CsConst.mstrINIDefault.IniReadValue("Public", "99811", "") + ((PictureBox)sender).Width.ToString() + "*" + ((PictureBox)sender).Height.ToString());
                    return;
                }
                int DIndex = Convert.ToInt32(dgvDevice[8, dgvDevice.CurrentRow.Index].Value.ToString());
                int DeviceType = Convert.ToInt32(dgvDevice[7, dgvDevice.CurrentRow.Index].Value.ToString());

                Panel pnl = null;
                for (int i = 0; i < CsConst.myPanels.Count; i++)
                {
                    if (CsConst.myPanels[i].DIndex == DIndex)
                    {
                        pnl = CsConst.myPanels[i];
                        break;
                    }
                }

                #region
                int Tag = Convert.ToInt32(((PictureBox)sender).Tag);
                int Page = Convert.ToInt32(((PictureBox)sender).Image.Tag);
                int Type = Convert.ToByte(((PictureBox)sender).ErrorImage.Tag);
                int Index = Convert.ToByte(((PictureBox)sender).InitialImage.Tag);
                if (Tag == 1)
                {
                    #region
                    if (Index == 0)
                    {
                        #region
                        int imgHeight = img.Height;
                        int numH = imgHeight / ((PictureBox)sender).Height;

                        for (int i = 0; i < numH; i++)
                        {
                            if (i <= 3)
                            {
                                PictureBox temp = this.Controls.Find("pag" + Page.ToString() + "OFF" + (i + 1).ToString(), true)[0] as PictureBox;
                                if (Type == 1)
                                    temp = this.Controls.Find("pag" + Page.ToString() + "ON" + (i + 1).ToString(), true)[0] as PictureBox;
                                System.Drawing.Bitmap partImg = new System.Drawing.Bitmap(temp.Width, temp.Height);
                                System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(partImg);
                                System.Drawing.Rectangle destRect = new System.Drawing.Rectangle(new System.Drawing.Point(0, 0), new System.Drawing.Size(temp.Width, temp.Height));//目标位置
                                System.Drawing.Rectangle origRect = new System.Drawing.Rectangle(new System.Drawing.Point(0, temp.Height * i), new System.Drawing.Size(temp.Width, temp.Height));//原图位置（默认从原图中截取的图片大小等于目标图片的大小）
                                graphics.DrawImage(img, destRect, origRect, System.Drawing.GraphicsUnit.Pixel);
                                temp.Image = partImg;
                                temp.Image.Tag = Page;
                                int num = 2;
                                if (i == 3) num = 8;
                                else if (i == 2) num = 6;
                                else if (i == 1) num = 4;
                                else num = 2;
                                for (int j = 1; j <= num; j++)
                                {
                                    if (Type == 1)
                                        pnl.IconON[(pnl.IconON.Length - 1 - 32) + (Page - 1) * 8 + j] = 1;
                                    else
                                        pnl.IconOFF[(pnl.IconOFF.Length - 1 - 32) + (Page - 1) * 8 + j] = 1;
                                }
                            }
                        }
                        #endregion
                    }
                    else
                    {
                        int imgHeight = img.Height;
                        int numH = imgHeight / ((PictureBox)sender).Height;
                        int imgWidth = img.Width;
                        int numW = imgWidth / ((PictureBox)sender).Width;
                        if (numW == 1)
                        {
                            #region
                            for (int i = 0; i < numH; i++)
                            {
                                if (i <= 3)
                                {
                                    PictureBox temp = this.Controls.Find("pag" + Page.ToString() + "OFF" + (i + 1).ToString(), true)[0] as PictureBox;
                                    if (Type == 1)
                                        temp = this.Controls.Find("pag" + Page.ToString() + "ON" + (i + 1).ToString(), true)[0] as PictureBox;
                                    if (Index >= 5)
                                    {
                                        temp = this.Controls.Find("pag" + Page.ToString() + "OFF" + (i + 5).ToString(), true)[0] as PictureBox;
                                        if (Type == 1)
                                            temp = this.Controls.Find("pag" + Page.ToString() + "ON" + (i + 5).ToString(), true)[0] as PictureBox;
                                    }
                                    System.Drawing.Bitmap partImg = new System.Drawing.Bitmap(temp.Width, temp.Height);
                                    System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(partImg);
                                    System.Drawing.Rectangle destRect = new System.Drawing.Rectangle(new System.Drawing.Point(0, 0), new System.Drawing.Size(temp.Width, temp.Height));//目标位置
                                    System.Drawing.Rectangle origRect = new System.Drawing.Rectangle(new System.Drawing.Point(0, temp.Height * i), new System.Drawing.Size(temp.Width, temp.Height));//原图位置（默认从原图中截取的图片大小等于目标图片的大小）
                                    graphics.DrawImage(img, destRect, origRect, System.Drawing.GraphicsUnit.Pixel);
                                    temp.Image = partImg;
                                    temp.Image.Tag = Page;
                                    if (i == 3)
                                    {
                                        if (Type == 1)
                                        {
                                            if (Index >= 5)
                                                pnl.IconON[(pnl.IconON.Length - 1 - 32) + (Page - 1) * 8 + 8] = 1;
                                            else
                                                pnl.IconON[(pnl.IconON.Length - 1 - 32) + (Page - 1) * 8 + 7] = 1;
                                        }
                                        else
                                        {
                                            if (Index >= 5)
                                                pnl.IconOFF[(pnl.IconOFF.Length - 1 - 32) + (Page - 1) * 8 + 8] = 1;
                                            else
                                                pnl.IconOFF[(pnl.IconOFF.Length - 1 - 32) + (Page - 1) * 8 + 7] = 1;
                                        }
                                    }
                                    else if (i == 2)
                                    {
                                        if (Type == 1)
                                        {
                                            if (Index >= 5)
                                                pnl.IconON[(pnl.IconON.Length - 1 - 32) + (Page - 1) * 8 + 6] = 1;
                                            else
                                                pnl.IconON[(pnl.IconON.Length - 1 - 32) + (Page - 1) * 8 + 5] = 1;
                                        }
                                        else
                                        {
                                            if (Index >= 5)
                                                pnl.IconOFF[(pnl.IconOFF.Length - 1 - 32) + (Page - 1) * 8 + 6] = 1;
                                            else
                                                pnl.IconOFF[(pnl.IconOFF.Length - 1 - 32) + (Page - 1) * 8 + 5] = 1;
                                        }
                                    }
                                    else if (i == 1)
                                    {
                                        if (Type == 1)
                                        {
                                            if (Index >= 5)
                                                pnl.IconON[(pnl.IconON.Length - 1 - 32) + (Page - 1) * 8 + 4] = 1;
                                            else
                                                pnl.IconON[(pnl.IconON.Length - 1 - 32) + (Page - 1) * 8 + 3] = 1;
                                        }
                                        else
                                        {
                                            if (Index >= 5)
                                                pnl.IconOFF[(pnl.IconOFF.Length - 1 - 32) + (Page - 1) * 8 + 4] = 1;
                                            else
                                                pnl.IconOFF[(pnl.IconOFF.Length - 1 - 32) + (Page - 1) * 8 + 3] = 1;
                                        }
                                    }
                                    else
                                    {
                                        if (Type == 1)
                                        {
                                            if (Index >= 5)
                                                pnl.IconON[(pnl.IconON.Length - 1 - 32) + (Page - 1) * 8 + 2] = 1;
                                            else
                                                pnl.IconON[(pnl.IconON.Length - 1 - 32) + (Page - 1) * 8 + 1] = 1;
                                        }
                                        else
                                        {
                                            if (Index >= 5)
                                                pnl.IconOFF[(pnl.IconOFF.Length - 1 - 32) + (Page - 1) * 8 + 2] = 1;
                                            else
                                                pnl.IconOFF[(pnl.IconOFF.Length - 1 - 32) + (Page - 1) * 8 + 1] = 1;
                                        }
                                    }
                                }
                            }
                            #endregion
                        }
                        else
                        {
                            #region
                            for (int i = 0; i < numH; i++)
                            {
                                if (i <= 3)
                                {
                                    PictureBox temp1 = this.Controls.Find("pag" + Page.ToString() + "OFF" + (i + 1).ToString(), true)[0] as PictureBox;
                                    if (Type == 1)
                                        temp1 = this.Controls.Find("pag" + Page.ToString() + "ON" + (i + 1).ToString(), true)[0] as PictureBox;
                                    PictureBox temp2 = this.Controls.Find("pag" + Page.ToString() + "OFF" + (i + 5).ToString(), true)[0] as PictureBox;
                                    if (Type == 1)
                                        temp2 = this.Controls.Find("pag" + Page.ToString() + "ON" + (i + 5).ToString(), true)[0] as PictureBox;
                                    System.Drawing.Bitmap partImg = new System.Drawing.Bitmap(temp1.Width, temp1.Height);
                                    System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(partImg);
                                    System.Drawing.Rectangle destRect = new System.Drawing.Rectangle(new System.Drawing.Point(0, 0), new System.Drawing.Size(temp1.Width, temp1.Height));//目标位置
                                    System.Drawing.Rectangle origRect = new System.Drawing.Rectangle(new System.Drawing.Point(0, temp1.Height * i), new System.Drawing.Size(temp1.Width, temp1.Height));//原图位置（默认从原图中截取的图片大小等于目标图片的大小）
                                    graphics.DrawImage(img, destRect, origRect, System.Drawing.GraphicsUnit.Pixel);
                                    temp1.Image = partImg;
                                    temp1.Image.Tag = Page;
                                    partImg = new System.Drawing.Bitmap(temp2.Width, temp2.Height);
                                    graphics = System.Drawing.Graphics.FromImage(partImg);
                                    destRect = new System.Drawing.Rectangle(new System.Drawing.Point(0, 0), new System.Drawing.Size(temp2.Width, temp2.Height));//目标位置
                                    origRect = new System.Drawing.Rectangle(new System.Drawing.Point(temp2.Width, temp2.Height * i), new System.Drawing.Size(temp2.Width, temp2.Height));//原图位置（默认从原图中截取的图片大小等于目标图片的大小）
                                    graphics.DrawImage(img, destRect, origRect, System.Drawing.GraphicsUnit.Pixel);
                                    temp2.Image = partImg;
                                    temp2.Image.Tag = Page;
                                    if (i == 3)
                                    {
                                        if (Type == 1)
                                        {
                                            pnl.IconON[(pnl.IconON.Length - 1 - 32) + (Page - 1) * 8 + 8] = 1;
                                            pnl.IconON[(pnl.IconON.Length - 1 - 32) + (Page - 1) * 8 + 7] = 1;
                                        }
                                        else
                                        {
                                            pnl.IconOFF[(pnl.IconOFF.Length - 1 - 32) + (Page - 1) * 8 + 8] = 1;
                                            pnl.IconOFF[(pnl.IconOFF.Length - 1 - 32) + (Page - 1) * 8 + 7] = 1;
                                        }
                                    }
                                    else if (i == 2)
                                    {
                                        if (Type == 1)
                                        {
                                            pnl.IconON[(pnl.IconON.Length - 1 - 32) + (Page - 1) * 8 + 6] = 1;
                                            pnl.IconON[(pnl.IconON.Length - 1 - 32) + (Page - 1) * 8 + 5] = 1;
                                        }
                                        else
                                        {
                                            pnl.IconOFF[(pnl.IconOFF.Length - 1 - 32) + (Page - 1) * 8 + 6] = 1;
                                            pnl.IconOFF[(pnl.IconOFF.Length - 1 - 32) + (Page - 1) * 8 + 5] = 1;
                                        }
                                    }
                                    else if (i == 1)
                                    {
                                        if (Type == 1)
                                        {
                                            pnl.IconON[(pnl.IconON.Length - 1 - 32) + (Page - 1) * 8 + 4] = 1;
                                            pnl.IconON[(pnl.IconON.Length - 1 - 32) + (Page - 1) * 8 + 3] = 1;
                                        }
                                        else
                                        {
                                            pnl.IconOFF[(pnl.IconOFF.Length - 1 - 32) + (Page - 1) * 8 + 4] = 1;
                                            pnl.IconOFF[(pnl.IconOFF.Length - 1 - 32) + (Page - 1) * 8 + 3] = 1;
                                        }
                                    }
                                    else
                                    {
                                        if (Type == 1)
                                        {
                                            pnl.IconON[(pnl.IconON.Length - 1 - 32) + (Page - 1) * 8 + 2] = 1;
                                            pnl.IconON[(pnl.IconON.Length - 1 - 32) + (Page - 1) * 8 + 1] = 1;
                                        }
                                        else
                                        {
                                            pnl.IconOFF[(pnl.IconOFF.Length - 1 - 32) + (Page - 1) * 8 + 2] = 1;
                                            pnl.IconOFF[(pnl.IconOFF.Length - 1 - 32) + (Page - 1) * 8 + 1] = 1;
                                        }
                                    }
                                }
                            }
                            #endregion
                        }
                    }
                    #endregion
                }
                else if (Tag == 2)
                {
                    #region
                    if (Index == 0)
                    {
                        
                        int imgHeight = img.Height;
                        int num = imgHeight / ((PictureBox)sender).Height;
                        if (num >= 4)
                        {
                            #region
                            for (int i = 0; i < 4; i++)
                            {
                                PictureBox temp = this.Controls.Find("pag" + Page.ToString() + "OFF" + (i + 1).ToString(), true)[0] as PictureBox;
                                if (Type == 1)
                                    temp = this.Controls.Find("pag" + Page.ToString() + "ON" + (i + 1).ToString(), true)[0] as PictureBox;
                                System.Drawing.Bitmap partImg = new System.Drawing.Bitmap(temp.Width, temp.Height);
                                System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(partImg);
                                System.Drawing.Rectangle destRect = new System.Drawing.Rectangle(new System.Drawing.Point(0, 0), new System.Drawing.Size(temp.Width, temp.Height));//目标位置
                                System.Drawing.Rectangle origRect = new System.Drawing.Rectangle(new System.Drawing.Point(0, temp.Height * i), new System.Drawing.Size(temp.Width, temp.Height));//原图位置（默认从原图中截取的图片大小等于目标图片的大小）
                                graphics.DrawImage(img, destRect, origRect, System.Drawing.GraphicsUnit.Pixel);
                                temp.Image = partImg;
                                temp.Image.Tag = Page;
                                int Countnum = 2;
                                if (i == 3) Countnum = 8;
                                else if (i == 2) Countnum = 6;
                                else if (i == 1) Countnum = 4;
                                else Countnum = 2;
                                for (int j = 1; j <= Countnum; j++)
                                {
                                    if (Type == 1)
                                        pnl.IconON[(pnl.IconON.Length - 1 - 32) + (Page - 1) * 8 + j] = 1;
                                    else
                                        pnl.IconOFF[(pnl.IconOFF.Length - 1 - 32) + (Page - 1) * 8 + j] = 1;
                                }
                            }
                        #endregion
                        }
                        else
                        {
                            #region
                            for (int i = 0; i < num; i++)
                            {
                                if (i <= 2)
                                {
                                    PictureBox temp = this.Controls.Find("pag" + Page.ToString() + "OFF" + (i + 2).ToString(), true)[0] as PictureBox;
                                    if (Type == 1)
                                        temp = this.Controls.Find("pag" + Page.ToString() + "ON" + (i + 2).ToString(), true)[0] as PictureBox;
                                    System.Drawing.Bitmap partImg = new System.Drawing.Bitmap(temp.Width, temp.Height);
                                    System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(partImg);
                                    System.Drawing.Rectangle destRect = new System.Drawing.Rectangle(new System.Drawing.Point(0, 0), new System.Drawing.Size(temp.Width, temp.Height));//目标位置
                                    System.Drawing.Rectangle origRect = new System.Drawing.Rectangle(new System.Drawing.Point(0, temp.Height * i), new System.Drawing.Size(temp.Width, temp.Height));//原图位置（默认从原图中截取的图片大小等于目标图片的大小）
                                    graphics.DrawImage(img, destRect, origRect, System.Drawing.GraphicsUnit.Pixel);
                                    temp.Image = partImg;
                                    temp.Image.Tag = Page;
                                    int Countnum = 2;
                                    if (i == 2) Countnum = 8;
                                    else if (i == 1) Countnum = 6;
                                    else Countnum = 4;
                                    for (int j = 3; j <= Countnum; j++)
                                    {
                                        if (Type == 1)
                                            pnl.IconON[(pnl.IconON.Length - 1 - 32) + (Page - 1) * 8 + j] = 1;
                                        else
                                            pnl.IconOFF[(pnl.IconOFF.Length - 1 - 32) + (Page - 1) * 8 + j] = 1;
                                    }
                                }
                            }
                            #endregion
                        }
                    }
                    else
                    {
                        int imgHeight = img.Height;
                        int numH = imgHeight / ((PictureBox)sender).Height;
                        int imgWidth = img.Width;
                        int numW = imgWidth / ((PictureBox)sender).Width;
                        if (numW == 1)
                        {
                            if (numH >= 4)
                            {
                                #region
                                for (int i = 0; i < 4; i++)
                                {
                                    PictureBox temp = this.Controls.Find("pag" + Page.ToString() + "OFF" + (i + 1).ToString(), true)[0] as PictureBox;
                                    if (Type == 1)
                                        temp = this.Controls.Find("pag" + Page.ToString() + "ON" + (i + 1).ToString(), true)[0] as PictureBox;
                                    if (Index >= 5)
                                    {
                                        temp = this.Controls.Find("pag" + Page.ToString() + "OFF" + (i + 5).ToString(), true)[0] as PictureBox;
                                        if (Type == 1)
                                            temp = this.Controls.Find("pag" + Page.ToString() + "ON" + (i + 5).ToString(), true)[0] as PictureBox;
                                    }
                                    System.Drawing.Bitmap partImg = new System.Drawing.Bitmap(temp.Width, temp.Height);
                                    System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(partImg);
                                    System.Drawing.Rectangle destRect = new System.Drawing.Rectangle(new System.Drawing.Point(0, 0), new System.Drawing.Size(temp.Width, temp.Height));//目标位置
                                    System.Drawing.Rectangle origRect = new System.Drawing.Rectangle(new System.Drawing.Point(0, temp.Height * i), new System.Drawing.Size(temp.Width, temp.Height));//原图位置（默认从原图中截取的图片大小等于目标图片的大小）
                                    graphics.DrawImage(img, destRect, origRect, System.Drawing.GraphicsUnit.Pixel);
                                    temp.Image = partImg;
                                    temp.Image.Tag = Page;
                                    if (i == 3)
                                    {
                                        if (Type == 1)
                                        {
                                            if (Index >= 5)
                                                pnl.IconON[(pnl.IconON.Length - 1 - 32) + (Page - 1) * 8 + 8] = 1;
                                            else
                                                pnl.IconON[(pnl.IconON.Length - 1 - 32) + (Page - 1) * 8 + 7] = 1;
                                        }
                                        else
                                        {
                                            if (Index >= 5)
                                                pnl.IconOFF[(pnl.IconOFF.Length - 1 - 32) + (Page - 1) * 8 + 8] = 1;
                                            else
                                                pnl.IconOFF[(pnl.IconOFF.Length - 1 - 32) + (Page - 1) * 8 + 7] = 1;
                                        }
                                    }
                                    else if (i == 2)
                                    {
                                        if (Type == 1)
                                        {
                                            if (Index >= 5)
                                                pnl.IconON[(pnl.IconON.Length - 1 - 32) + (Page - 1) * 8 + 6] = 1;
                                            else
                                                pnl.IconON[(pnl.IconON.Length - 1 - 32) + (Page - 1) * 8 + 5] = 1;
                                        }
                                        else
                                        {
                                            if (Index >= 5)
                                                pnl.IconOFF[(pnl.IconOFF.Length - 1 - 32) + (Page - 1) * 8 + 6] = 1;
                                            else
                                                pnl.IconOFF[(pnl.IconOFF.Length - 1 - 32) + (Page - 1) * 8 + 5] = 1;
                                        }
                                    }
                                    else if (i == 1)
                                    {
                                        if (Type == 1)
                                        {
                                            if (Index >= 5)
                                                pnl.IconON[(pnl.IconON.Length - 1 - 32) + (Page - 1) * 8 + 4] = 1;
                                            else
                                                pnl.IconON[(pnl.IconON.Length - 1 - 32) + (Page - 1) * 8 + 3] = 1;
                                        }
                                        else
                                        {
                                            if (Index >= 5)
                                                pnl.IconOFF[(pnl.IconOFF.Length - 1 - 32) + (Page - 1) * 8 + 4] = 1;
                                            else
                                                pnl.IconOFF[(pnl.IconOFF.Length - 1 - 32) + (Page - 1) * 8 + 3] = 1;
                                        }
                                    }
                                    else
                                    {
                                        if (Type == 1)
                                        {
                                            if (Index >= 5)
                                                pnl.IconON[(pnl.IconON.Length - 1 - 32) + (Page - 1) * 8 + 2] = 1;
                                            else
                                                pnl.IconON[(pnl.IconON.Length - 1 - 32) + (Page - 1) * 8 + 1] = 1;
                                        }
                                        else
                                        {
                                            if (Index >= 5)
                                                pnl.IconOFF[(pnl.IconOFF.Length - 1 - 32) + (Page - 1) * 8 + 2] = 1;
                                            else
                                                pnl.IconOFF[(pnl.IconOFF.Length - 1 - 32) + (Page - 1) * 8 + 1] = 1;
                                        }
                                    }
                                }
                            #endregion
                            }
                            else
                            {
                                #region
                                for (int i = 0; i < numH; i++)
                                {
                                    if (i <= 2)
                                    {
                                        PictureBox temp = this.Controls.Find("pag" + Page.ToString() + "OFF" + (i + Index).ToString(), true)[0] as PictureBox;
                                        if (Type == 1)
                                            temp = this.Controls.Find("pag" + Page.ToString() + "ON" + (i + Index).ToString(), true)[0] as PictureBox;
                                        System.Drawing.Bitmap partImg = new System.Drawing.Bitmap(temp.Width, temp.Height);
                                        System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(partImg);
                                        System.Drawing.Rectangle destRect = new System.Drawing.Rectangle(new System.Drawing.Point(0, 0), new System.Drawing.Size(temp.Width, temp.Height));//目标位置
                                        System.Drawing.Rectangle origRect = new System.Drawing.Rectangle(new System.Drawing.Point(0, temp.Height * i), new System.Drawing.Size(temp.Width, temp.Height));//原图位置（默认从原图中截取的图片大小等于目标图片的大小）
                                        graphics.DrawImage(img, destRect, origRect, System.Drawing.GraphicsUnit.Pixel);
                                        temp.Image = partImg;
                                        temp.Image.Tag = Page;
                                        if (i == 2)
                                        {
                                            if (Type == 1)
                                            {
                                                if (Index >= 5)
                                                    pnl.IconON[(pnl.IconON.Length - 1 - 32) + (Page - 1) * 8 + 8] = 1;
                                                else
                                                    pnl.IconON[(pnl.IconON.Length - 1 - 32) + (Page - 1) * 8 + 7] = 1;
                                            }
                                            else
                                            {
                                                if (Index >= 5)
                                                    pnl.IconOFF[(pnl.IconOFF.Length - 1 - 32) + (Page - 1) * 8 + 8] = 1;
                                                else
                                                    pnl.IconOFF[(pnl.IconOFF.Length - 1 - 32) + (Page - 1) * 8 + 7] = 1;
                                            }
                                        }
                                        else if (i == 1)
                                        {
                                            if (Type == 1)
                                            {
                                                if (Index >= 5)
                                                    pnl.IconON[(pnl.IconON.Length - 1 - 32) + (Page - 1) * 8 + 6] = 1;
                                                else
                                                    pnl.IconON[(pnl.IconON.Length - 1 - 32) + (Page - 1) * 8 + 5] = 1;
                                            }
                                            else
                                            {
                                                if (Index >= 5)
                                                    pnl.IconOFF[(pnl.IconOFF.Length - 1 - 32) + (Page - 1) * 8 + 6] = 1;
                                                else
                                                    pnl.IconOFF[(pnl.IconOFF.Length - 1 - 32) + (Page - 1) * 8 + 5] = 1;
                                            }
                                        }
                                        else
                                        {
                                            if (Type == 1)
                                            {
                                                if (Index >= 5)
                                                    pnl.IconON[(pnl.IconON.Length - 1 - 32) + (Page - 1) * 8 + 4] = 1;
                                                else
                                                    pnl.IconON[(pnl.IconON.Length - 1 - 32) + (Page - 1) * 8 + 3] = 1;
                                            }
                                            else
                                            {
                                                if (Index >= 5)
                                                    pnl.IconOFF[(pnl.IconOFF.Length - 1 - 32) + (Page - 1) * 8 + 4] = 1;
                                                else
                                                    pnl.IconOFF[(pnl.IconOFF.Length - 1 - 32) + (Page - 1) * 8 + 3] = 1;
                                            }
                                        }
                                    }
                                }
                                #endregion
                            }
                        }
                        else
                        {
                            if (numH >= 4)
                            {
                                #region
                                for (int i = 0; i < 4; i++)
                                {
                                    PictureBox temp1 = this.Controls.Find("pag" + Page.ToString() + "OFF" + (i + 1).ToString(), true)[0] as PictureBox;
                                    if (Type == 1)
                                        temp1 = this.Controls.Find("pag" + Page.ToString() + "ON" + (i + 1).ToString(), true)[0] as PictureBox;
                                    PictureBox temp2 = this.Controls.Find("pag" + Page.ToString() + "OFF" + (i + 5).ToString(), true)[0] as PictureBox;
                                    if (Type == 1)
                                        temp2 = this.Controls.Find("pag" + Page.ToString() + "ON" + (i + 5).ToString(), true)[0] as PictureBox;
                                    System.Drawing.Bitmap partImg = new System.Drawing.Bitmap(temp1.Width, temp1.Height);
                                    System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(partImg);
                                    System.Drawing.Rectangle destRect = new System.Drawing.Rectangle(new System.Drawing.Point(0, 0), new System.Drawing.Size(temp1.Width, temp1.Height));//目标位置
                                    System.Drawing.Rectangle origRect = new System.Drawing.Rectangle(new System.Drawing.Point(0, temp1.Height * i), new System.Drawing.Size(temp1.Width, temp1.Height));//原图位置（默认从原图中截取的图片大小等于目标图片的大小）
                                    graphics.DrawImage(img, destRect, origRect, System.Drawing.GraphicsUnit.Pixel);
                                    temp1.Image = partImg;
                                    temp1.Image.Tag = Page;
                                    partImg = new System.Drawing.Bitmap(temp2.Width, temp2.Height);
                                    graphics = System.Drawing.Graphics.FromImage(partImg);
                                    destRect = new System.Drawing.Rectangle(new System.Drawing.Point(0, 0), new System.Drawing.Size(temp2.Width, temp2.Height));//目标位置
                                    origRect = new System.Drawing.Rectangle(new System.Drawing.Point(temp2.Width, temp2.Height * i), new System.Drawing.Size(temp2.Width, temp2.Height));//原图位置（默认从原图中截取的图片大小等于目标图片的大小）
                                    graphics.DrawImage(img, destRect, origRect, System.Drawing.GraphicsUnit.Pixel);
                                    temp2.Image = partImg;
                                    temp2.Image.Tag = Page;
                                    if (i == 3)
                                    {
                                        if (Type == 1)
                                        {
                                            pnl.IconON[(pnl.IconON.Length - 1 - 32) + (Page - 1) * 8 + 8] = 1;
                                            pnl.IconON[(pnl.IconON.Length - 1 - 32) + (Page - 1) * 8 + 7] = 1;
                                        }
                                        else
                                        {
                                            pnl.IconOFF[(pnl.IconOFF.Length - 1 - 32) + (Page - 1) * 8 + 8] = 1;
                                            pnl.IconOFF[(pnl.IconOFF.Length - 1 - 32) + (Page - 1) * 8 + 7] = 1;
                                        }
                                    }
                                    else if (i == 2)
                                    {
                                        if (Type == 1)
                                        {
                                            pnl.IconON[(pnl.IconON.Length - 1 - 32) + (Page - 1) * 8 + 6] = 1;
                                            pnl.IconON[(pnl.IconON.Length - 1 - 32) + (Page - 1) * 8 + 5] = 1;
                                        }
                                        else
                                        {
                                            pnl.IconOFF[(pnl.IconOFF.Length - 1 - 32) + (Page - 1) * 8 + 6] = 1;
                                            pnl.IconOFF[(pnl.IconOFF.Length - 1 - 32) + (Page - 1) * 8 + 5] = 1;
                                        }
                                    }
                                    else if (i == 1)
                                    {
                                        if (Type == 1)
                                        {
                                            pnl.IconON[(pnl.IconON.Length - 1 - 32) + (Page - 1) * 8 + 4] = 1;
                                            pnl.IconON[(pnl.IconON.Length - 1 - 32) + (Page - 1) * 8 + 3] = 1;
                                        }
                                        else
                                        {
                                            pnl.IconOFF[(pnl.IconOFF.Length - 1 - 32) + (Page - 1) * 8 + 4] = 1;
                                            pnl.IconOFF[(pnl.IconOFF.Length - 1 - 32) + (Page - 1) * 8 + 3] = 1;
                                        }
                                    }
                                    else
                                    {
                                        if (Type == 1)
                                        {
                                            pnl.IconON[(pnl.IconON.Length - 1 - 32) + (Page - 1) * 8 + 2] = 1;
                                            pnl.IconON[(pnl.IconON.Length - 1 - 32) + (Page - 1) * 8 + 1] = 1;
                                        }
                                        else
                                        {
                                            pnl.IconOFF[(pnl.IconOFF.Length - 1 - 32) + (Page - 1) * 8 + 2] = 1;
                                            pnl.IconOFF[(pnl.IconOFF.Length - 1 - 32) + (Page - 1) * 8 + 1] = 1;
                                        }
                                    }
                                }
                                #endregion
                            }
                            else
                            {
                                #region
                                for (int i = 0; i < numH; i++)
                                {
                                    if (i <= 2)
                                    {
                                        PictureBox temp1 = this.Controls.Find("pag" + Page.ToString() + "OFF" + (i + 2).ToString(), true)[0] as PictureBox;
                                        if (Type == 1)
                                            temp1 = this.Controls.Find("pag" + Page.ToString() + "ON" + (i + 2).ToString(), true)[0] as PictureBox;
                                        PictureBox temp2 = this.Controls.Find("pag" + Page.ToString() + "OFF" + (i + 6).ToString(), true)[0] as PictureBox;
                                        if (Type == 1)
                                            temp2 = this.Controls.Find("pag" + Page.ToString() + "ON" + (i + 6).ToString(), true)[0] as PictureBox;
                                        System.Drawing.Bitmap partImg = new System.Drawing.Bitmap(temp1.Width, temp1.Height);
                                        System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(partImg);
                                        System.Drawing.Rectangle destRect = new System.Drawing.Rectangle(new System.Drawing.Point(0, 0), new System.Drawing.Size(temp1.Width, temp1.Height));//目标位置
                                        System.Drawing.Rectangle origRect = new System.Drawing.Rectangle(new System.Drawing.Point(0, temp1.Height * i), new System.Drawing.Size(temp1.Width, temp1.Height));//原图位置（默认从原图中截取的图片大小等于目标图片的大小）
                                        graphics.DrawImage(img, destRect, origRect, System.Drawing.GraphicsUnit.Pixel);
                                        temp1.Image = partImg;
                                        temp1.Image.Tag = Page;
                                        partImg = new System.Drawing.Bitmap(temp2.Width, temp2.Height);
                                        graphics = System.Drawing.Graphics.FromImage(partImg);
                                        destRect = new System.Drawing.Rectangle(new System.Drawing.Point(0, 0), new System.Drawing.Size(temp2.Width, temp2.Height));//目标位置
                                        origRect = new System.Drawing.Rectangle(new System.Drawing.Point(temp2.Width, temp2.Height * i), new System.Drawing.Size(temp2.Width, temp2.Height));//原图位置（默认从原图中截取的图片大小等于目标图片的大小）
                                        graphics.DrawImage(img, destRect, origRect, System.Drawing.GraphicsUnit.Pixel);
                                        temp2.Image = partImg;
                                        temp2.Image.Tag = Page;
                                        if (i == 2)
                                        {
                                            if (Type == 1)
                                            {
                                                pnl.IconON[(pnl.IconON.Length - 1 - 32) + (Page - 1) * 8 + 8] = 1;
                                                pnl.IconON[(pnl.IconON.Length - 1 - 32) + (Page - 1) * 8 + 7] = 1;
                                            }
                                            else
                                            {
                                                pnl.IconOFF[(pnl.IconOFF.Length - 1 - 32) + (Page - 1) * 8 + 8] = 1;
                                                pnl.IconOFF[(pnl.IconOFF.Length - 1 - 32) + (Page - 1) * 8 + 7] = 1;
                                            }
                                        }
                                        else if (i == 1)
                                        {
                                            if (Type == 1)
                                            {
                                                pnl.IconON[(pnl.IconON.Length - 1 - 32) + (Page - 1) * 8 + 6] = 1;
                                                pnl.IconON[(pnl.IconON.Length - 1 - 32) + (Page - 1) * 8 + 5] = 1;
                                            }
                                            else
                                            {
                                                pnl.IconOFF[(pnl.IconOFF.Length - 1 - 32) + (Page - 1) * 8 + 6] = 1;
                                                pnl.IconOFF[(pnl.IconOFF.Length - 1 - 32) + (Page - 1) * 8 + 5] = 1;
                                            }
                                        }
                                        else
                                        {
                                            if (Type == 1)
                                            {
                                                pnl.IconON[(pnl.IconON.Length - 1 - 32) + (Page - 1) * 8 + 4] = 1;
                                                pnl.IconON[(pnl.IconON.Length - 1 - 32) + (Page - 1) * 8 + 3] = 1;
                                            }
                                            else
                                            {
                                                pnl.IconOFF[(pnl.IconOFF.Length - 1 - 32) + (Page - 1) * 8 + 4] = 1;
                                                pnl.IconOFF[(pnl.IconOFF.Length - 1 - 32) + (Page - 1) * 8 + 3] = 1;
                                            }
                                        }
                                    }
                                }
                                #endregion
                            }
                        }
                    }
                    #endregion
                }
                else if (Tag == 3)
                {
                    #region
                    if (Index == 0)
                    {
                        int imgHeight = img.Height;
                        int num = imgHeight / ((PictureBox)sender).Height;
                        if (num >= 4)
                        {
                            #region
                            for (int i = 0; i < 4; i++)
                            {
                                PictureBox temp = this.Controls.Find("pag" + Page.ToString() + "OFF" + (i + 1).ToString(), true)[0] as PictureBox;
                                if (Type == 1)
                                    temp = this.Controls.Find("pag" + Page.ToString() + "ON" + (i + 1).ToString(), true)[0] as PictureBox;
                                System.Drawing.Bitmap partImg = new System.Drawing.Bitmap(temp.Width, temp.Height);
                                System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(partImg);
                                System.Drawing.Rectangle destRect = new System.Drawing.Rectangle(new System.Drawing.Point(0, 0), new System.Drawing.Size(temp.Width, temp.Height));//目标位置
                                System.Drawing.Rectangle origRect = new System.Drawing.Rectangle(new System.Drawing.Point(0, temp.Height * i), new System.Drawing.Size(temp.Width, temp.Height));//原图位置（默认从原图中截取的图片大小等于目标图片的大小）
                                graphics.DrawImage(img, destRect, origRect, System.Drawing.GraphicsUnit.Pixel);
                                temp.Image = partImg;
                                temp.Image.Tag = Page;
                                int Countnum = 2;
                                if (i == 3) Countnum = 8;
                                else if (i == 2) Countnum = 6;
                                else if (i == 1) Countnum = 4;
                                else Countnum = 2;
                                for (int j = 1; j <= Countnum; j++)
                                {
                                    if (Type == 1)
                                        pnl.IconON[(pnl.IconON.Length - 1 - 32) + (Page - 1) * 8 + j] = 1;
                                    else
                                        pnl.IconOFF[(pnl.IconOFF.Length - 1 - 32) + (Page - 1) * 8 + j] = 1;
                                }
                            }
                            #endregion
                        }
                        else
                        {
                            #region
                            for (int i = 0; i < num; i++)
                            {
                                if (i <= 1)
                                {
                                    PictureBox temp = this.Controls.Find("pag" + Page.ToString() + "OFF" + (i + 3).ToString(), true)[0] as PictureBox;
                                    if (Type == 1)
                                        temp = this.Controls.Find("pag" + Page.ToString() + "ON" + (i + 3).ToString(), true)[0] as PictureBox;
                                    System.Drawing.Bitmap partImg = new System.Drawing.Bitmap(temp.Width, temp.Height);
                                    System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(partImg);
                                    System.Drawing.Rectangle destRect = new System.Drawing.Rectangle(new System.Drawing.Point(0, 0), new System.Drawing.Size(temp.Width, temp.Height));//目标位置
                                    System.Drawing.Rectangle origRect = new System.Drawing.Rectangle(new System.Drawing.Point(0, temp.Height * i), new System.Drawing.Size(temp.Width, temp.Height));//原图位置（默认从原图中截取的图片大小等于目标图片的大小）
                                    graphics.DrawImage(img, destRect, origRect, System.Drawing.GraphicsUnit.Pixel);
                                    temp.Image = partImg;
                                    temp.Image.Tag = Page;
                                    int Countnum = 2;
                                    if (i == 1) Countnum = 8;
                                    else Countnum = 6;
                                    for (int j = 5; j <= Countnum; j++)
                                    {
                                        if (Type == 1)
                                            pnl.IconON[(pnl.IconON.Length - 1 - 32) + (Page - 1) * 8 + j] = 1;
                                        else
                                            pnl.IconOFF[(pnl.IconOFF.Length - 1 - 32) + (Page - 1) * 8 + j] = 1;
                                    }
                                }
                            }
                            #endregion
                        }
                    }
                    else
                    {
                        int imgHeight = img.Height;
                        int numH = imgHeight / ((PictureBox)sender).Height;
                        int imgWidth = img.Width;
                        int numW = imgWidth / ((PictureBox)sender).Width;
                        if (numW == 1)
                        {
                            if (numH >= 4)
                            {
                                #region
                                for (int i = 0; i < 4; i++)
                                {
                                    PictureBox temp = this.Controls.Find("pag" + Page.ToString() + "OFF" + (i + 1).ToString(), true)[0] as PictureBox;
                                    if (Type == 1)
                                        temp = this.Controls.Find("pag" + Page.ToString() + "ON" + (i + 1).ToString(), true)[0] as PictureBox;
                                    if (Index >= 5)
                                    {
                                        temp = this.Controls.Find("pag" + Page.ToString() + "OFF" + (i + 5).ToString(), true)[0] as PictureBox;
                                        if (Type == 1)
                                            temp = this.Controls.Find("pag" + Page.ToString() + "ON" + (i + 5).ToString(), true)[0] as PictureBox;
                                    }
                                    System.Drawing.Bitmap partImg = new System.Drawing.Bitmap(temp.Width, temp.Height);
                                    System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(partImg);
                                    System.Drawing.Rectangle destRect = new System.Drawing.Rectangle(new System.Drawing.Point(0, 0), new System.Drawing.Size(temp.Width, temp.Height));//目标位置
                                    System.Drawing.Rectangle origRect = new System.Drawing.Rectangle(new System.Drawing.Point(0, temp.Height * i), new System.Drawing.Size(temp.Width, temp.Height));//原图位置（默认从原图中截取的图片大小等于目标图片的大小）
                                    graphics.DrawImage(img, destRect, origRect, System.Drawing.GraphicsUnit.Pixel);
                                    temp.Image = partImg;
                                    temp.Image.Tag = Page;
                                    if (i == 3)
                                    {
                                        if (Type == 1)
                                        {
                                            if (Index >= 5)
                                                pnl.IconON[(pnl.IconON.Length - 1 - 32) + (Page - 1) * 8 + 8] = 1;
                                            else
                                                pnl.IconON[(pnl.IconON.Length - 1 - 32) + (Page - 1) * 8 + 7] = 1;
                                        }
                                        else
                                        {
                                            if (Index >= 5)
                                                pnl.IconOFF[(pnl.IconOFF.Length - 1 - 32) + (Page - 1) * 8 + 8] = 1;
                                            else
                                                pnl.IconOFF[(pnl.IconOFF.Length - 1 - 32) + (Page - 1) * 8 + 7] = 1;
                                        }
                                    }
                                    else if (i == 2)
                                    {
                                        if (Type == 1)
                                        {
                                            if (Index >= 5)
                                                pnl.IconON[(pnl.IconON.Length - 1 - 32) + (Page - 1) * 8 + 6] = 1;
                                            else
                                                pnl.IconON[(pnl.IconON.Length - 1 - 32) + (Page - 1) * 8 + 5] = 1;
                                        }
                                        else
                                        {
                                            if (Index >= 5)
                                                pnl.IconOFF[(pnl.IconOFF.Length - 1 - 32) + (Page - 1) * 8 + 6] = 1;
                                            else
                                                pnl.IconOFF[(pnl.IconOFF.Length - 1 - 32) + (Page - 1) * 8 + 5] = 1;
                                        }
                                    }
                                    else if (i == 1)
                                    {
                                        if (Type == 1)
                                        {
                                            if (Index >= 5)
                                                pnl.IconON[(pnl.IconON.Length - 1 - 32) + (Page - 1) * 8 + 4] = 1;
                                            else
                                                pnl.IconON[(pnl.IconON.Length - 1 - 32) + (Page - 1) * 8 + 3] = 1;
                                        }
                                        else
                                        {
                                            if (Index >= 5)
                                                pnl.IconOFF[(pnl.IconOFF.Length - 1 - 32) + (Page - 1) * 8 + 4] = 1;
                                            else
                                                pnl.IconOFF[(pnl.IconOFF.Length - 1 - 32) + (Page - 1) * 8 + 3] = 1;
                                        }
                                    }
                                    else
                                    {
                                        if (Type == 1)
                                        {
                                            if (Index >= 5)
                                                pnl.IconON[(pnl.IconON.Length - 1 - 32) + (Page - 1) * 8 + 2] = 1;
                                            else
                                                pnl.IconON[(pnl.IconON.Length - 1 - 32) + (Page - 1) * 8 + 1] = 1;
                                        }
                                        else
                                        {
                                            if (Index >= 5)
                                                pnl.IconOFF[(pnl.IconOFF.Length - 1 - 32) + (Page - 1) * 8 + 2] = 1;
                                            else
                                                pnl.IconOFF[(pnl.IconOFF.Length - 1 - 32) + (Page - 1) * 8 + 1] = 1;
                                        }
                                    }
                                }
                                #endregion
                            }
                            else
                            {
                                #region
                                for (int i = 0; i < numH; i++)
                                {
                                    if (i <= 1)
                                    {
                                        PictureBox temp = this.Controls.Find("pag" + Page.ToString() + "OFF" + (i + Index).ToString(), true)[0] as PictureBox;
                                        if (Type == 1)
                                            temp = this.Controls.Find("pag" + Page.ToString() + "ON" + (i + Index).ToString(), true)[0] as PictureBox;
                                        System.Drawing.Bitmap partImg = new System.Drawing.Bitmap(temp.Width, temp.Height);
                                        System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(partImg);
                                        System.Drawing.Rectangle destRect = new System.Drawing.Rectangle(new System.Drawing.Point(0, 0), new System.Drawing.Size(temp.Width, temp.Height));//目标位置
                                        System.Drawing.Rectangle origRect = new System.Drawing.Rectangle(new System.Drawing.Point(0, temp.Height * i), new System.Drawing.Size(temp.Width, temp.Height));//原图位置（默认从原图中截取的图片大小等于目标图片的大小）
                                        graphics.DrawImage(img, destRect, origRect, System.Drawing.GraphicsUnit.Pixel);
                                        temp.Image = partImg;
                                        temp.Image.Tag = Page;
                                        if (i == 1)
                                        {
                                            if (Type == 1)
                                            {
                                                if (Index >= 5)
                                                    pnl.IconON[(pnl.IconON.Length - 1 - 32) + (Page - 1) * 8 + 8] = 1;
                                                else
                                                    pnl.IconON[(pnl.IconON.Length - 1 - 32) + (Page - 1) * 8 + 7] = 1;
                                            }
                                            else
                                            {
                                                if (Index >= 5)
                                                    pnl.IconOFF[(pnl.IconOFF.Length - 1 - 32) + (Page - 1) * 8 + 8] = 1;
                                                else
                                                    pnl.IconOFF[(pnl.IconOFF.Length - 1 - 32) + (Page - 1) * 8 + 7] = 1;
                                            }
                                        }
                                        else
                                        {
                                            if (Type == 1)
                                            {
                                                if (Index >= 5)
                                                    pnl.IconON[(pnl.IconON.Length - 1 - 32) + (Page - 1) * 8 + 6] = 1;
                                                else
                                                    pnl.IconON[(pnl.IconON.Length - 1 - 32) + (Page - 1) * 8 + 5] = 1;
                                            }
                                            else
                                            {
                                                if (Index >= 5)
                                                    pnl.IconOFF[(pnl.IconOFF.Length - 1 - 32) + (Page - 1) * 8 + 6] = 1;
                                                else
                                                    pnl.IconOFF[(pnl.IconOFF.Length - 1 - 32) + (Page - 1) * 8 + 5] = 1;
                                            }
                                        }
                                    }
                                }
                                #endregion
                            }
                        }
                        else
                        {
                            if (numH >= 4)
                            {
                                #region
                                for (int i = 0; i < 4; i++)
                                {
                                    PictureBox temp1 = this.Controls.Find("pag" + Page.ToString() + "OFF" + (i + 1).ToString(), true)[0] as PictureBox;
                                    if (Type == 1)
                                        temp1 = this.Controls.Find("pag" + Page.ToString() + "ON" + (i + 1).ToString(), true)[0] as PictureBox;
                                    PictureBox temp2 = this.Controls.Find("pag" + Page.ToString() + "OFF" + (i + 5).ToString(), true)[0] as PictureBox;
                                    if (Type == 1)
                                        temp2 = this.Controls.Find("pag" + Page.ToString() + "ON" + (i + 5).ToString(), true)[0] as PictureBox;
                                    System.Drawing.Bitmap partImg = new System.Drawing.Bitmap(temp1.Width, temp1.Height);
                                    System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(partImg);
                                    System.Drawing.Rectangle destRect = new System.Drawing.Rectangle(new System.Drawing.Point(0, 0), new System.Drawing.Size(temp1.Width, temp1.Height));//目标位置
                                    System.Drawing.Rectangle origRect = new System.Drawing.Rectangle(new System.Drawing.Point(0, temp1.Height * i), new System.Drawing.Size(temp1.Width, temp1.Height));//原图位置（默认从原图中截取的图片大小等于目标图片的大小）
                                    graphics.DrawImage(img, destRect, origRect, System.Drawing.GraphicsUnit.Pixel);
                                    temp1.Image = partImg;
                                    temp1.Image.Tag = Page;
                                    partImg = new System.Drawing.Bitmap(temp2.Width, temp2.Height);
                                    graphics = System.Drawing.Graphics.FromImage(partImg);
                                    destRect = new System.Drawing.Rectangle(new System.Drawing.Point(0, 0), new System.Drawing.Size(temp2.Width, temp2.Height));//目标位置
                                    origRect = new System.Drawing.Rectangle(new System.Drawing.Point(temp2.Width, temp2.Height * i), new System.Drawing.Size(temp2.Width, temp2.Height));//原图位置（默认从原图中截取的图片大小等于目标图片的大小）
                                    graphics.DrawImage(img, destRect, origRect, System.Drawing.GraphicsUnit.Pixel);
                                    temp2.Image = partImg;
                                    temp2.Image.Tag = Page;
                                    if (i == 3)
                                    {
                                        if (Type == 1)
                                        {
                                            pnl.IconON[(pnl.IconON.Length - 1 - 32) + (Page - 1) * 8 + 8] = 1;
                                            pnl.IconON[(pnl.IconON.Length - 1 - 32) + (Page - 1) * 8 + 7] = 1;
                                        }
                                        else
                                        {
                                            pnl.IconOFF[(pnl.IconOFF.Length - 1 - 32) + (Page - 1) * 8 + 8] = 1;
                                            pnl.IconOFF[(pnl.IconOFF.Length - 1 - 32) + (Page - 1) * 8 + 7] = 1;
                                        }
                                    }
                                    else if (i == 2)
                                    {
                                        if (Type == 1)
                                        {
                                            pnl.IconON[(pnl.IconON.Length - 1 - 32) + (Page - 1) * 8 + 6] = 1;
                                            pnl.IconON[(pnl.IconON.Length - 1 - 32) + (Page - 1) * 8 + 5] = 1;
                                        }
                                        else
                                        {
                                            pnl.IconOFF[(pnl.IconOFF.Length - 1 - 32) + (Page - 1) * 8 + 6] = 1;
                                            pnl.IconOFF[(pnl.IconOFF.Length - 1 - 32) + (Page - 1) * 8 + 5] = 1;
                                        }
                                    }
                                    else if (i == 1)
                                    {
                                        if (Type == 1)
                                        {
                                            pnl.IconON[(pnl.IconON.Length - 1 - 32) + (Page - 1) * 8 + 4] = 1;
                                            pnl.IconON[(pnl.IconON.Length - 1 - 32) + (Page - 1) * 8 + 3] = 1;
                                        }
                                        else
                                        {
                                            pnl.IconOFF[(pnl.IconOFF.Length - 1 - 32) + (Page - 1) * 8 + 4] = 1;
                                            pnl.IconOFF[(pnl.IconOFF.Length - 1 - 32) + (Page - 1) * 8 + 3] = 1;
                                        }
                                    }
                                    else
                                    {
                                        if (Type == 1)
                                        {
                                            pnl.IconON[(pnl.IconON.Length - 1 - 32) + (Page - 1) * 8 + 2] = 1;
                                            pnl.IconON[(pnl.IconON.Length - 1 - 32) + (Page - 1) * 8 + 1] = 1;
                                        }
                                        else
                                        {
                                            pnl.IconOFF[(pnl.IconOFF.Length - 1 - 32) + (Page - 1) * 8 + 2] = 1;
                                            pnl.IconOFF[(pnl.IconOFF.Length - 1 - 32) + (Page - 1) * 8 + 1] = 1;
                                        }
                                    }
                                }
                                #endregion
                            }
                            else
                            {
                                #region
                                for (int i = 0; i < numH; i++)
                                {
                                    if (i <= 1)
                                    {
                                        PictureBox temp1 = this.Controls.Find("pag" + Page.ToString() + "OFF" + (i + 3).ToString(), true)[0] as PictureBox;
                                        if (Type == 1)
                                            temp1 = this.Controls.Find("pag" + Page.ToString() + "ON" + (i + 3).ToString(), true)[0] as PictureBox;
                                        PictureBox temp2 = this.Controls.Find("pag" + Page.ToString() + "OFF" + (i + 7).ToString(), true)[0] as PictureBox;
                                        if (Type == 1)
                                            temp2 = this.Controls.Find("pag" + Page.ToString() + "ON" + (i + 7).ToString(), true)[0] as PictureBox;
                                        System.Drawing.Bitmap partImg = new System.Drawing.Bitmap(temp1.Width, temp1.Height);
                                        System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(partImg);
                                        System.Drawing.Rectangle destRect = new System.Drawing.Rectangle(new System.Drawing.Point(0, 0), new System.Drawing.Size(temp1.Width, temp1.Height));//目标位置
                                        System.Drawing.Rectangle origRect = new System.Drawing.Rectangle(new System.Drawing.Point(0, temp1.Height * i), new System.Drawing.Size(temp1.Width, temp1.Height));//原图位置（默认从原图中截取的图片大小等于目标图片的大小）
                                        graphics.DrawImage(img, destRect, origRect, System.Drawing.GraphicsUnit.Pixel);
                                        temp1.Image = partImg;
                                        temp1.Image.Tag = Page;
                                        partImg = new System.Drawing.Bitmap(temp2.Width, temp2.Height);
                                        graphics = System.Drawing.Graphics.FromImage(partImg);
                                        destRect = new System.Drawing.Rectangle(new System.Drawing.Point(0, 0), new System.Drawing.Size(temp2.Width, temp2.Height));//目标位置
                                        origRect = new System.Drawing.Rectangle(new System.Drawing.Point(temp2.Width, temp2.Height * i), new System.Drawing.Size(temp2.Width, temp2.Height));//原图位置（默认从原图中截取的图片大小等于目标图片的大小）
                                        graphics.DrawImage(img, destRect, origRect, System.Drawing.GraphicsUnit.Pixel);
                                        temp2.Image = partImg;
                                        temp2.Image.Tag = Page;
                                        if (i == 1)
                                        {
                                            if (Type == 1)
                                            {
                                                pnl.IconON[(pnl.IconON.Length - 1 - 32) + (Page - 1) * 8 + 8] = 1;
                                                pnl.IconON[(pnl.IconON.Length - 1 - 32) + (Page - 1) * 8 + 7] = 1;
                                            }
                                            else
                                            {
                                                pnl.IconOFF[(pnl.IconOFF.Length - 1 - 32) + (Page - 1) * 8 + 8] = 1;
                                                pnl.IconOFF[(pnl.IconOFF.Length - 1 - 32) + (Page - 1) * 8 + 7] = 1;
                                            }
                                        }
                                        else
                                        {
                                            if (Type == 1)
                                            {
                                                pnl.IconON[(pnl.IconON.Length - 1 - 32) + (Page - 1) * 8 + 6] = 1;
                                                pnl.IconON[(pnl.IconON.Length - 1 - 32) + (Page - 1) * 8 + 5] = 1;
                                            }
                                            else
                                            {
                                                pnl.IconOFF[(pnl.IconOFF.Length - 1 - 32) + (Page - 1) * 8 + 6] = 1;
                                                pnl.IconOFF[(pnl.IconOFF.Length - 1 - 32) + (Page - 1) * 8 + 5] = 1;
                                            }
                                        }
                                    }
                                }
                                #endregion
                            }
                        }
                    }
                    #endregion
                }
                else if (Tag == 4)
                {
                    #region
                    if (Index == 0)
                    {
                        int imgHeight = img.Height;
                        int num = imgHeight / ((PictureBox)sender).Height;
                        if (num >= 4)
                        {
                            #region
                            for (int i = 0; i < 4; i++)
                            {
                                PictureBox temp = this.Controls.Find("pag" + Page.ToString() + "OFF" + (i + 1).ToString(), true)[0] as PictureBox;
                                if (Type == 1)
                                    temp = this.Controls.Find("pag" + Page.ToString() + "ON" + (i + 1).ToString(), true)[0] as PictureBox;
                                System.Drawing.Bitmap partImg = new System.Drawing.Bitmap(temp.Width, temp.Height);
                                System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(partImg);
                                System.Drawing.Rectangle destRect = new System.Drawing.Rectangle(new System.Drawing.Point(0, 0), new System.Drawing.Size(temp.Width, temp.Height));//目标位置
                                System.Drawing.Rectangle origRect = new System.Drawing.Rectangle(new System.Drawing.Point(0, temp.Height * i), new System.Drawing.Size(temp.Width, temp.Height));//原图位置（默认从原图中截取的图片大小等于目标图片的大小）
                                graphics.DrawImage(img, destRect, origRect, System.Drawing.GraphicsUnit.Pixel);
                                temp.Image = partImg;
                                temp.Image.Tag = Page;
                                int Countnum = 2;
                                if (i == 3) Countnum = 8;
                                else if (i == 2) Countnum = 6;
                                else if (i == 1) Countnum = 4;
                                else Countnum = 2;
                                for (int j = 1; j <= Countnum; j++)
                                {
                                    if (Type == 1)
                                        pnl.IconON[(pnl.IconON.Length - 1 - 32) + (Page - 1) * 8 + j] = 1;
                                    else
                                        pnl.IconOFF[(pnl.IconOFF.Length - 1 - 32) + (Page - 1) * 8 + j] = 1;
                                }
                            }
                            #endregion
                        }
                        else
                        {
                            #region
                            PictureBox temp = this.Controls.Find("pag" + Page.ToString() + "OFF4", true)[0] as PictureBox;
                            if (Type == 1)
                                temp = this.Controls.Find("pag" + Page.ToString() + "ON4", true)[0] as PictureBox;
                            System.Drawing.Bitmap partImg = new System.Drawing.Bitmap(temp.Width, temp.Height);
                            System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(partImg);
                            System.Drawing.Rectangle destRect = new System.Drawing.Rectangle(new System.Drawing.Point(0, 0), new System.Drawing.Size(temp.Width, temp.Height));//目标位置
                            System.Drawing.Rectangle origRect = new System.Drawing.Rectangle(new System.Drawing.Point(0, 0), new System.Drawing.Size(temp.Width, temp.Height));//原图位置（默认从原图中截取的图片大小等于目标图片的大小）
                            graphics.DrawImage(img, destRect, origRect, System.Drawing.GraphicsUnit.Pixel);
                            temp.Image = partImg;
                            temp.Image.Tag = Page;
                            if (Type == 1)
                            {
                                pnl.IconON[(pnl.IconON.Length - 1 - 32) + (Page - 1) * 8 + 7] = 1;
                                pnl.IconON[(pnl.IconON.Length - 1 - 32) + (Page - 1) * 8 + 8] = 1;
                            }
                            else
                            {
                                pnl.IconOFF[(pnl.IconOFF.Length - 1 - 32) + (Page - 1) * 8 + 7] = 1;
                                pnl.IconOFF[(pnl.IconOFF.Length - 1 - 32) + (Page - 1) * 8 + 8] = 1;
                            }
                            #endregion
                        }
                    }
                    else
                    {
                        int imgHeight = img.Height;
                        int numH = imgHeight / ((PictureBox)sender).Height;
                        int imgWidth = img.Width;
                        int numW = imgWidth / ((PictureBox)sender).Width;
                        if (numW == 1)
                        {
                            if (numH >= 4)
                            {
                                #region
                                for (int i = 0; i < 4; i++)
                                {
                                    PictureBox temp = this.Controls.Find("pag" + Page.ToString() + "OFF" + (i+1).ToString(), true)[0] as PictureBox;
                                    if (Type == 1)
                                        temp = this.Controls.Find("pag" + Page.ToString() + "ON" + (i+1).ToString(), true)[0] as PictureBox;
                                    if (Index >= 5)
                                    {
                                        temp = this.Controls.Find("pag" + Page.ToString() + "OFF" + (i + 5).ToString(), true)[0] as PictureBox;
                                        if (Type == 1)
                                            temp = this.Controls.Find("pag" + Page.ToString() + "ON" + (i + 5).ToString(), true)[0] as PictureBox;
                                    }
                                    System.Drawing.Bitmap partImg = new System.Drawing.Bitmap(temp.Width, temp.Height);
                                    System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(partImg);
                                    System.Drawing.Rectangle destRect = new System.Drawing.Rectangle(new System.Drawing.Point(0, 0), new System.Drawing.Size(temp.Width, temp.Height));//目标位置
                                    System.Drawing.Rectangle origRect = new System.Drawing.Rectangle(new System.Drawing.Point(0, temp.Height * i), new System.Drawing.Size(temp.Width, temp.Height));//原图位置（默认从原图中截取的图片大小等于目标图片的大小）
                                    graphics.DrawImage(img, destRect, origRect, System.Drawing.GraphicsUnit.Pixel);
                                    temp.Image = partImg;
                                    temp.Image.Tag = Page;
                                    if (i == 3)
                                    {
                                        if (Type == 1)
                                        {
                                            if (Index >= 5)
                                                pnl.IconON[(pnl.IconON.Length - 1 - 32) + (Page - 1) * 8 + 8] = 1;
                                            else
                                                pnl.IconON[(pnl.IconON.Length - 1 - 32) + (Page - 1) * 8 + 7] = 1;
                                        }
                                        else
                                        {
                                            if (Index >= 5)
                                                pnl.IconOFF[(pnl.IconOFF.Length - 1 - 32) + (Page - 1) * 8 + 8] = 1;
                                            else
                                                pnl.IconOFF[(pnl.IconOFF.Length - 1 - 32) + (Page - 1) * 8 + 7] = 1;
                                        }
                                    }
                                    else if (i == 2)
                                    {
                                        if (Type == 1)
                                        {
                                            if (Index >= 5)
                                                pnl.IconON[(pnl.IconON.Length - 1 - 32) + (Page - 1) * 8 + 6] = 1;
                                            else
                                                pnl.IconON[(pnl.IconON.Length - 1 - 32) + (Page - 1) * 8 + 5] = 1;
                                        }
                                        else
                                        {
                                            if (Index >= 5)
                                                pnl.IconOFF[(pnl.IconOFF.Length - 1 - 32) + (Page - 1) * 8 + 6] = 1;
                                            else
                                                pnl.IconOFF[(pnl.IconOFF.Length - 1 - 32) + (Page - 1) * 8 + 5] = 1;
                                        }
                                    }
                                    else if (i == 1)
                                    {
                                        if (Type == 1)
                                        {
                                            if (Index >= 5)
                                                pnl.IconON[(pnl.IconON.Length - 1 - 32) + (Page - 1) * 8 + 4] = 1;
                                            else
                                                pnl.IconON[(pnl.IconON.Length - 1 - 32) + (Page - 1) * 8 + 3] = 1;
                                        }
                                        else
                                        {
                                            if (Index >= 5)
                                                pnl.IconOFF[(pnl.IconOFF.Length - 1 - 32) + (Page - 1) * 8 + 4] = 1;
                                            else
                                                pnl.IconOFF[(pnl.IconOFF.Length - 1 - 32) + (Page - 1) * 8 + 3] = 1;
                                        }
                                    }
                                    else
                                    {
                                        if (Type == 1)
                                        {
                                            if (Index >= 5)
                                                pnl.IconON[(pnl.IconON.Length - 1 - 32) + (Page - 1) * 8 + 2] = 1;
                                            else
                                                pnl.IconON[(pnl.IconON.Length - 1 - 32) + (Page - 1) * 8 + 1] = 1;
                                        }
                                        else
                                        {
                                            if (Index >= 5)
                                                pnl.IconOFF[(pnl.IconOFF.Length - 1 - 32) + (Page - 1) * 8 + 2] = 1;
                                            else
                                                pnl.IconOFF[(pnl.IconOFF.Length - 1 - 32) + (Page - 1) * 8 + 1] = 1;
                                        }
                                    }
                                }
                                #endregion
                            }
                            else
                            {
                                #region
                                PictureBox temp = this.Controls.Find("pag" + Page.ToString() + "OFF" + Index.ToString(), true)[0] as PictureBox;
                                if (Type == 1)
                                    temp = this.Controls.Find("pag" + Page.ToString() + "ON" + Index.ToString(), true)[0] as PictureBox;
                                System.Drawing.Bitmap partImg = new System.Drawing.Bitmap(temp.Width, temp.Height);
                                System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(partImg);
                                System.Drawing.Rectangle destRect = new System.Drawing.Rectangle(new System.Drawing.Point(0, 0), new System.Drawing.Size(temp.Width, temp.Height));//目标位置
                                System.Drawing.Rectangle origRect = new System.Drawing.Rectangle(new System.Drawing.Point(0, 0), new System.Drawing.Size(temp.Width, temp.Height));//原图位置（默认从原图中截取的图片大小等于目标图片的大小）
                                graphics.DrawImage(img, destRect, origRect, System.Drawing.GraphicsUnit.Pixel);
                                temp.Image = partImg;
                                temp.Image.Tag = Page;
                                if (Type == 1)
                                {
                                    if (Index >= 5)
                                        pnl.IconON[(pnl.IconON.Length - 1 - 32) + (Page - 1) * 8 + 8] = 1;
                                    else
                                        pnl.IconON[(pnl.IconON.Length - 1 - 32) + (Page - 1) * 8 + 7] = 1;
                                }
                                else
                                {
                                    if (Index >= 5)
                                        pnl.IconOFF[(pnl.IconOFF.Length - 1 - 32) + (Page - 1) * 8 + 8] = 1;
                                    else
                                        pnl.IconOFF[(pnl.IconOFF.Length - 1 - 32) + (Page - 1) * 8 + 7] = 1;
                                }
                                #endregion
                            }
                        }
                        else
                        {
                            if (numH >= 4)
                            {
                                #region
                                for (int i = 0; i < 4; i++)
                                {
                                    PictureBox temp1 = this.Controls.Find("pag" + Page.ToString() + "OFF" + (i + 1).ToString(), true)[0] as PictureBox;
                                    if (Type == 1)
                                        temp1 = this.Controls.Find("pag" + Page.ToString() + "ON" + (i + 1).ToString(), true)[0] as PictureBox;
                                    PictureBox temp2 = this.Controls.Find("pag" + Page.ToString() + "OFF" + (i + 5).ToString(), true)[0] as PictureBox;
                                    if (Type == 1)
                                        temp2 = this.Controls.Find("pag" + Page.ToString() + "ON" + (i + 5).ToString(), true)[0] as PictureBox;
                                    System.Drawing.Bitmap partImg = new System.Drawing.Bitmap(temp1.Width, temp1.Height);
                                    System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(partImg);
                                    System.Drawing.Rectangle destRect = new System.Drawing.Rectangle(new System.Drawing.Point(0, 0), new System.Drawing.Size(temp1.Width, temp1.Height));//目标位置
                                    System.Drawing.Rectangle origRect = new System.Drawing.Rectangle(new System.Drawing.Point(0, temp1.Height * i), new System.Drawing.Size(temp1.Width, temp1.Height));//原图位置（默认从原图中截取的图片大小等于目标图片的大小）
                                    graphics.DrawImage(img, destRect, origRect, System.Drawing.GraphicsUnit.Pixel);
                                    temp1.Image = partImg;
                                    temp1.Image.Tag = Page;
                                    partImg = new System.Drawing.Bitmap(temp2.Width, temp2.Height);
                                    graphics = System.Drawing.Graphics.FromImage(partImg);
                                    destRect = new System.Drawing.Rectangle(new System.Drawing.Point(0, 0), new System.Drawing.Size(temp2.Width, temp2.Height));//目标位置
                                    origRect = new System.Drawing.Rectangle(new System.Drawing.Point(temp2.Width, temp2.Height * i), new System.Drawing.Size(temp2.Width, temp2.Height));//原图位置（默认从原图中截取的图片大小等于目标图片的大小）
                                    graphics.DrawImage(img, destRect, origRect, System.Drawing.GraphicsUnit.Pixel);
                                    temp2.Image = partImg;
                                    temp2.Image.Tag = Page;
                                    if (i == 3)
                                    {
                                        if (Type == 1)
                                        {
                                            pnl.IconON[(pnl.IconON.Length - 1 - 32) + (Page - 1) * 8 + 8] = 1;
                                            pnl.IconON[(pnl.IconON.Length - 1 - 32) + (Page - 1) * 8 + 7] = 1;
                                        }
                                        else
                                        {
                                            pnl.IconOFF[(pnl.IconOFF.Length - 1 - 32) + (Page - 1) * 8 + 8] = 1;
                                            pnl.IconOFF[(pnl.IconOFF.Length - 1 - 32) + (Page - 1) * 8 + 7] = 1;
                                        }
                                    }
                                    else if (i == 2)
                                    {
                                        if (Type == 1)
                                        {
                                            pnl.IconON[(pnl.IconON.Length - 1 - 32) + (Page - 1) * 8 + 6] = 1;
                                            pnl.IconON[(pnl.IconON.Length - 1 - 32) + (Page - 1) * 8 + 5] = 1;
                                        }
                                        else
                                        {
                                            pnl.IconOFF[(pnl.IconOFF.Length - 1 - 32) + (Page - 1) * 8 + 6] = 1;
                                            pnl.IconOFF[(pnl.IconOFF.Length - 1 - 32) + (Page - 1) * 8 + 5] = 1;
                                        }
                                    }
                                    else if (i == 1)
                                    {
                                        if (Type == 1)
                                        {
                                            pnl.IconON[(pnl.IconON.Length - 1 - 32) + (Page - 1) * 8 + 4] = 1;
                                            pnl.IconON[(pnl.IconON.Length - 1 - 32) + (Page - 1) * 8 + 3] = 1;
                                        }
                                        else
                                        {
                                            pnl.IconOFF[(pnl.IconOFF.Length - 1 - 32) + (Page - 1) * 8 + 4] = 1;
                                            pnl.IconOFF[(pnl.IconOFF.Length - 1 - 32) + (Page - 1) * 8 + 3] = 1;
                                        }
                                    }
                                    else
                                    {
                                        if (Type == 1)
                                        {
                                            pnl.IconON[(pnl.IconON.Length - 1 - 32) + (Page - 1) * 8 + 2] = 1;
                                            pnl.IconON[(pnl.IconON.Length - 1 - 32) + (Page - 1) * 8 + 1] = 1;
                                        }
                                        else
                                        {
                                            pnl.IconOFF[(pnl.IconOFF.Length - 1 - 32) + (Page - 1) * 8 + 2] = 1;
                                            pnl.IconOFF[(pnl.IconOFF.Length - 1 - 32) + (Page - 1) * 8 + 1] = 1;
                                        }
                                    }
                                }
                                #endregion
                            }
                            else
                            {
                                #region
                                PictureBox temp1 = this.Controls.Find("pag" + Page.ToString() + "OFF4", true)[0] as PictureBox;
                                if (Type == 1)
                                    temp1 = this.Controls.Find("pag" + Page.ToString() + "ON4", true)[0] as PictureBox;
                                PictureBox temp2 = this.Controls.Find("pag" + Page.ToString() + "OFF8", true)[0] as PictureBox;
                                if (Type == 1)
                                    temp2 = this.Controls.Find("pag" + Page.ToString() + "ON8", true)[0] as PictureBox;
                                System.Drawing.Bitmap partImg = new System.Drawing.Bitmap(temp1.Width, temp1.Height);
                                System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(partImg);
                                System.Drawing.Rectangle destRect = new System.Drawing.Rectangle(new System.Drawing.Point(0, 0), new System.Drawing.Size(temp1.Width, temp1.Height));//目标位置
                                System.Drawing.Rectangle origRect = new System.Drawing.Rectangle(new System.Drawing.Point(0, 0), new System.Drawing.Size(temp1.Width, temp1.Height));//原图位置（默认从原图中截取的图片大小等于目标图片的大小）
                                graphics.DrawImage(img, destRect, origRect, System.Drawing.GraphicsUnit.Pixel);
                                temp1.Image = partImg;
                                temp1.Image.Tag = Page;
                                partImg = new System.Drawing.Bitmap(temp2.Width, temp2.Height);
                                graphics = System.Drawing.Graphics.FromImage(partImg);
                                destRect = new System.Drawing.Rectangle(new System.Drawing.Point(temp2.Width, 0), new System.Drawing.Size(temp2.Width, temp2.Height));//目标位置
                                origRect = new System.Drawing.Rectangle(new System.Drawing.Point(temp2.Width, 0), new System.Drawing.Size(temp2.Width, temp2.Height));//原图位置（默认从原图中截取的图片大小等于目标图片的大小）
                                graphics.DrawImage(img, destRect, origRect, System.Drawing.GraphicsUnit.Pixel);
                                temp2.Image = partImg;
                                temp2.Image.Tag = Page;
                                if (Type == 1)
                                {
                                    pnl.IconON[(pnl.IconON.Length - 1 - 32) + (Page - 1) * 8 + 8] = 1;
                                    pnl.IconON[(pnl.IconON.Length - 1 - 32) + (Page - 1) * 8 + 7] = 1;
                                }
                                else
                                {
                                    pnl.IconOFF[(pnl.IconOFF.Length - 1 - 32) + (Page - 1) * 8 + 8] = 1;
                                    pnl.IconOFF[(pnl.IconOFF.Length - 1 - 32) + (Page - 1) * 8 + 7] = 1;
                                }
                                #endregion
                            }
                        }
                    }
                    #endregion
                }
                #endregion

                #region
                if (pnl != null)
                {
                    if (Type == 0)
                    {
                        if (pnl.IconOFF != null)
                        {
                            if (Index < 5)
                            {
                                if (Tag == 1)
                                    pnl.IconOFF[(pnl.IconOFF.Length - 1 - 32) + (Page - 1) * 8 + 1] = 1;
                                else if (Tag == 2)
                                    pnl.IconOFF[(pnl.IconOFF.Length - 1 - 32) + (Page - 1) * 8 + 3] = 1;
                                else if (Tag == 3)
                                    pnl.IconOFF[(pnl.IconOFF.Length - 1 - 32) + (Page - 1) * 8 + 5] = 1;
                                else if (Tag == 4)
                                    pnl.IconOFF[(pnl.IconOFF.Length - 1 - 32) + (Page - 1) * 8 + 7] = 1;
                            }
                            else
                            {
                                if (Tag == 1)
                                    pnl.IconOFF[(pnl.IconOFF.Length - 1 - 32) + (Page - 1) * 8 + 2] = 1;
                                else if (Tag == 2)
                                    pnl.IconOFF[(pnl.IconOFF.Length - 1 - 32) + (Page - 1) * 8 + 4] = 1;
                                else if (Tag == 3)
                                    pnl.IconOFF[(pnl.IconOFF.Length - 1 - 32) + (Page - 1) * 8 + 6] = 1;
                                else if (Tag == 4)
                                    pnl.IconOFF[(pnl.IconOFF.Length - 1 - 32) + (Page - 1) * 8 + 8] = 1;
                            }
                        }
                    }
                    else if (Type == 1)
                    {
                        if (pnl.IconON != null)
                        {
                            if (Index < 5)
                            {
                                if (Tag == 1)
                                    pnl.IconON[(pnl.IconON.Length - 1 - 32) + (Page - 1) * 8 + 1] = 1;
                                else if (Tag == 2)
                                    pnl.IconON[(pnl.IconON.Length - 1 - 32) + (Page - 1) * 8 + 3] = 1;
                                else if (Tag == 3)
                                    pnl.IconON[(pnl.IconON.Length - 1 - 32) + (Page - 1) * 8 + 5] = 1;
                                else if (Tag == 4)
                                    pnl.IconON[(pnl.IconON.Length - 1 - 32) + (Page - 1) * 8 + 7] = 1;
                            }
                            else
                            {
                                if (Tag == 1)
                                    pnl.IconON[(pnl.IconON.Length - 1 - 32) + (Page - 1) * 8 + 2] = 1;
                                else if (Tag == 2)
                                    pnl.IconON[(pnl.IconON.Length - 1 - 32) + (Page - 1) * 8 + 4] = 1;
                                else if (Tag == 3)
                                    pnl.IconON[(pnl.IconON.Length - 1 - 32) + (Page - 1) * 8 + 6] = 1;
                                else if (Tag == 4)
                                    pnl.IconON[(pnl.IconON.Length - 1 - 32) + (Page - 1) * 8 + 8] = 1;
                            }
                        }
                    }
                }
                #endregion
            }
            catch
            {
            }
        }

        private void btnDefaultBorder_Click(object sender, EventArgs e)
        {
            try
            {
                for (int i = 1; i <= 4; i++)
                {
                    PictureBox temp = this.Controls.Find("picDIY" + i.ToString(), true)[0] as PictureBox;
                    Graphics g = temp.CreateGraphics();
                    Rectangle rect = new Rectangle(temp.ClientRectangle.X, temp.ClientRectangle.Location.Y,
                                                     temp.ClientRectangle.X + temp.ClientRectangle.Width - 1,
                                                     temp.ClientRectangle.Y + temp.ClientRectangle.Height - 1);
                    System.Drawing.Pen p = new System.Drawing.Pen(System.Drawing.Color.Black);
                    g.DrawRectangle(p, rect);

                    PictureBox temp2 = this.Controls.Find("picDIYTmp" + i.ToString(), true)[0] as PictureBox;
                    g = temp2.CreateGraphics();
                    rect = new Rectangle(temp2.ClientRectangle.X, temp2.ClientRectangle.Location.Y,
                                                     temp2.ClientRectangle.X + temp2.ClientRectangle.Width - 1,
                                                     temp2.ClientRectangle.Y + temp2.ClientRectangle.Height - 1);
                    p = new System.Drawing.Pen(System.Drawing.Color.Black);
                    g.DrawRectangle(p, rect);
                }
            }
            catch
            {
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            for (int i = 1; i <= 4; i++)
            {
                PictureBox temp = this.Controls.Find("picDIY" + i.ToString(), true)[0] as PictureBox;
                if (cbType.SelectedIndex == 0) temp.Image = Image.FromFile(Application.StartupPath + @"\Icon\DLP\Empty1.bmp");
                if (cbType.SelectedIndex == 1) temp.Image = Image.FromFile(Application.StartupPath + @"\Icon\DLP\Empty2.bmp");
                if (cbType.SelectedIndex == 2) temp.Image = Image.FromFile(Application.StartupPath + @"\Icon\DLP\Empty3.bmp");
                if (cbType.SelectedIndex == 3) temp.Image = Image.FromFile(Application.StartupPath + @"\Icon\DLP\Empty4.bmp");

                PictureBox temp2 = this.Controls.Find("picDIYTmp"+i.ToString(), true)[0] as PictureBox;
                if (cbType.SelectedIndex == 0) temp2.Image = Image.FromFile(Application.StartupPath + @"\Icon\DLP\Empty1.bmp");
                if (cbType.SelectedIndex == 1) temp2.Image = Image.FromFile(Application.StartupPath + @"\Icon\DLP\Empty2.bmp");
                if (cbType.SelectedIndex == 2) temp2.Image = Image.FromFile(Application.StartupPath + @"\Icon\DLP\Empty3.bmp");
                if (cbType.SelectedIndex == 3) temp2.Image = Image.FromFile(Application.StartupPath + @"\Icon\DLP\Empty4.bmp");
            }
        }

        private void dgvDevice_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                bool isSetColorDLPUploadType = false;
                Cursor.Current = Cursors.WaitCursor;
                int DIndex = Convert.ToInt32(dgvDevice[8, e.RowIndex].Value.ToString());
                int DeviceType= Convert.ToInt32(dgvDevice[7, e.RowIndex].Value.ToString());
                CsConst.panelWaitUpload = new List<DevOnLine>();
                for (int i = 0; i < CsConst.myOnlines.Count; i++)
                {
                    if (CsConst.myOnlines[i].intDIndex == DIndex)
                    {
                        CsConst.panelWaitUpload.Add(CsConst.myOnlines[i]);
                        break;
                    }
                }

                for (int i = 0; i < CsConst.panelWaitUpload.Count; i++)
                {
                    CsConst.isStopDealImageBackground = false;
                    if (DLPPanelDeviceTypeList.HDLDLPPanelDeviceTypeList.Contains(CsConst.panelWaitUpload[i].DeviceType) ||
                        CsConst.panelWaitUpload[i].DeviceType == 5003 || 
                        CsConst.panelWaitUpload[i].DeviceType == 5064)
                    {
                        if (e.RowIndex >= 0 && CsConst.myPanels.Count > 0)
                        {
                            if (e.ColumnIndex == 5)
                            {
                                #region
                                FrmPage frmTmp = new FrmPage();
                                frmTmp.StartPosition = FormStartPosition.CenterParent;
                                DialogResult = DialogResult.Cancel;
                                if (frmTmp.ShowDialog() == DialogResult.OK)
                                {
                                    CsConst.MyUPload2DownLists = new List<byte[]>();

                                    byte[] ArayRelay = new byte[] { 
                                        CsConst.panelWaitUpload[i].bytSub, 
                                        CsConst.panelWaitUpload[i].bytDev, 
                                        (byte)(CsConst.panelWaitUpload[i].DeviceType / 256), 
                                        (byte)(CsConst.panelWaitUpload[i].DeviceType % 256), 0,
                                        (byte)(CsConst.panelWaitUpload[i].intDIndex / 256), 
                                        (byte)(CsConst.panelWaitUpload[i].intDIndex % 256)};
                                    CsConst.MyUPload2DownLists.Add(ArayRelay);
                                    CsConst.MyUpload2Down = 2;
                                    FrmDownloadShow Frm = new FrmDownloadShow();
                                    Frm.ShowDialog();
                                    showDownloadPanelIcons(CsConst.panelWaitUpload[i].intDIndex, CsConst.panelWaitUpload[i].DeviceType);
                                }
                                #endregion
                            }
                            else if (e.ColumnIndex == 6)
                            {
                                #region
                                CsConst.MyUPload2DownLists = new List<byte[]>();
                                if (CsConst.panelWaitUpload.Count > 1)
                                {
                                    if (i == 0)
                                    {
                                        savePanelIcons(CsConst.panelWaitUpload[i].intDIndex, CsConst.panelWaitUpload[i].DeviceType);
                                    }
                                    else
                                    {
                                        Panel pnlTmp = null;
                                        for (int k = 0; k < CsConst.myPanels.Count; k++)
                                        {
                                            if (CsConst.myPanels[k].DIndex == CsConst.panelWaitUpload[0].intDIndex)
                                            {
                                                pnlTmp = CsConst.myPanels[k];
                                                break;
                                            }
                                        }
                                        for (int j = 1; j < CsConst.panelWaitUpload.Count; j++)
                                        {
                                            Panel pnl = null;
                                            for (int k = 0; k < CsConst.myPanels.Count; k++)
                                            {
                                                if (CsConst.myPanels[k].DIndex == CsConst.panelWaitUpload[j].intDIndex)
                                                {
                                                    pnl = CsConst.myPanels[i];
                                                    pnl.IconOFF = pnlTmp.IconOFF;
                                                    pnl.IconON = pnlTmp.IconON;
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    savePanelIcons(CsConst.panelWaitUpload[i].intDIndex, CsConst.panelWaitUpload[i].DeviceType);
                                }
                                byte[] ArayRelay = new byte[] {
                                        CsConst.panelWaitUpload[i].bytSub, 
                                        CsConst.panelWaitUpload[i].bytDev, 
                                        (byte)(CsConst.panelWaitUpload[i].DeviceType / 256), 
                                        (byte)(CsConst.panelWaitUpload[i].DeviceType % 256), 0,
                                        (byte)(CsConst.panelWaitUpload[i].intDIndex / 256), 
                                        (byte)(CsConst.panelWaitUpload[i].intDIndex % 256)};
                                CsConst.MyUPload2DownLists.Add(ArayRelay);
                                CsConst.MyUpload2Down = 3;
                                FrmDownloadShow Frm = new FrmDownloadShow();
                                Frm.ShowDialog();
                                #endregion
                            }
                        }
                    }
                    else if (EnviroDLPPanelDeviceTypeList.HDLEnviroDLPPanelDeviceTypeList.Contains(CsConst.panelWaitUpload[i].DeviceType))
                    {
                        #region
                        if (e.RowIndex >= 0 && CsConst.myColorPanels.Count > 0)
                        {
                            if (e.ColumnIndex == 5)
                            {

                            }
                            else if (e.ColumnIndex == 6)
                            {
                                #region
                                FrmColorDLPImageType frmTmp = new FrmColorDLPImageType(1);
                                frmTmp.StartPosition = FormStartPosition.CenterParent;
                                DialogResult = DialogResult.Cancel;
                                if (isSetColorDLPUploadType || frmTmp.ShowDialog() == DialogResult.OK)
                                {
                                    isSetColorDLPUploadType = true;
                                    CsConst.MyUPload2DownLists = new List<byte[]>();
                                    saveColorDLPIcons(CsConst.panelWaitUpload[i].intDIndex, CsConst.panelWaitUpload[i].DeviceType);
                                    byte[] arayTmp = new byte[3];
                                    if (CsConst.UploadColorDLPType == 1 || CsConst.UploadColorDLPType == 0)
                                    {
                                        int StartAddress = cboMenu.SelectedIndex * 65280;
                                        if (cboMenu.SelectedIndex == 3) StartAddress = 5 * 65280;
                                        else if (cboMenu.SelectedIndex == 4) StartAddress = 391680;
                                        else if (cboMenu.SelectedIndex == 5) StartAddress = 65280 + 391680;
                                        else if (cboMenu.SelectedIndex == 6) StartAddress = 2 * 65280 + 391680;
                                        else if (cboMenu.SelectedIndex == 7) StartAddress = 4 * 65280 + 391680;
                                        else if (cboMenu.SelectedIndex == 8) StartAddress = 5 * 65280 + 391680;
                                        arayTmp[0] = Convert.ToByte(StartAddress / 256 / 256 % 256);
                                        arayTmp[1] = Convert.ToByte(StartAddress / 256 % 256);
                                        arayTmp[2] = Convert.ToByte(StartAddress % 256);
                                    }
                                    else if (CsConst.UploadColorDLPType == 2 || CsConst.UploadColorDLPType == 0)
                                    {
                                        arayTmp[0] = Convert.ToByte(cbIcon.SelectedIndex);
                                    }
                                    else if (CsConst.UploadColorDLPType == 3 || CsConst.UploadColorDLPType == 0)
                                    {
                                        arayTmp[0] = Convert.ToByte(cbIcon.SelectedIndex);
                                    }
                                    byte[] ArayRelay = new byte[] { 
                                        CsConst.panelWaitUpload[i].bytSub, 
                                        CsConst.panelWaitUpload[i].bytDev, 
                                        (byte)(CsConst.panelWaitUpload[i].DeviceType / 256), 
                                        (byte)(CsConst.panelWaitUpload[i].DeviceType % 256), (byte)CsConst.UploadColorDLPType,
                                        (byte)(CsConst.panelWaitUpload[i].intDIndex / 256), 
                                        (byte)(CsConst.panelWaitUpload[i].intDIndex % 256),
                                        arayTmp[0],arayTmp[1],arayTmp[2]};
                                    CsConst.MyUPload2DownLists.Add(ArayRelay);
                                    CsConst.MyUpload2Down = 3;
                                    FrmDownloadShow Frm = new FrmDownloadShow();
                                    Frm.ShowDialog();
                                }
                                #endregion
                            }
                        }
                        #endregion
                    }
                    else if (EnviroNewDeviceTypeList.EnviroNewPanelDeviceTypeList.Contains(CsConst.panelWaitUpload[i].DeviceType))
                    {
                        #region
                        if (e.ColumnIndex == 5)
                        {

                        }
                        else if (e.ColumnIndex == 6)
                        {
                            FrmColorDLPImageType frmTmp = new FrmColorDLPImageType(1);
                            frmTmp.StartPosition = FormStartPosition.CenterParent;
                            DialogResult = DialogResult.Cancel;
                            if (isSetColorDLPUploadType || frmTmp.ShowDialog() == DialogResult.OK)
                            {
                                isSetColorDLPUploadType = true;
                                CsConst.MyUPload2DownLists = new List<byte[]>();
                                saveColorDLPIcons(CsConst.panelWaitUpload[i].intDIndex, CsConst.panelWaitUpload[i].DeviceType);
                                byte[] arayTmp = new byte[3];
                                if (CsConst.UploadColorDLPType == 1 || CsConst.UploadColorDLPType == 0)
                                {
                                    arayTmp[0] = Convert.ToByte(cboMenu.SelectedIndex);
                                }
                                else if (CsConst.UploadColorDLPType == 2 || CsConst.UploadColorDLPType == 0)
                                {
                                    arayTmp[0] = Convert.ToByte(cbIcon.SelectedIndex + 1);
                                }
                                else if (CsConst.UploadColorDLPType == 3 || CsConst.UploadColorDLPType == 0)
                                {
                                    arayTmp[0] = Convert.ToByte(cbIcon.SelectedIndex + 1);
                                }
                                byte[] ArayRelay = new byte[] { 
                                        CsConst.panelWaitUpload[i].bytSub, 
                                        CsConst.panelWaitUpload[i].bytDev, 
                                        (byte)(CsConst.panelWaitUpload[i].DeviceType / 256), 
                                        (byte)(CsConst.panelWaitUpload[i].DeviceType % 256),(byte)CsConst.UploadColorDLPType,
                                        (byte)(CsConst.panelWaitUpload[i].intDIndex / 256), 
                                        (byte)(CsConst.panelWaitUpload[i].intDIndex % 256),
                                        arayTmp[0],arayTmp[1],arayTmp[2]};
                                CsConst.MyUPload2DownLists.Add(ArayRelay);
                                CsConst.MyUpload2Down = 3;
                                FrmDownloadShow Frm = new FrmDownloadShow();
                                Frm.ShowDialog();
                            }
                        }
                        #endregion
                    }
                }
            }
            catch
            {
                
            }
            Cursor.Current = Cursors.Default;
        }

        private void saveColorDLPIcons(int dindex, int devicetype)
        {
            try
            {
                byte SubnetID = Convert.ToByte(dgvDevice[1, dgvDevice.CurrentRow.Index].Value.ToString());
                byte DeviceID=Convert.ToByte(dgvDevice[2, dgvDevice.CurrentRow.Index].Value.ToString());
                if (EnviroDLPPanelDeviceTypeList.HDLEnviroDLPPanelDeviceTypeList.Contains(devicetype))
                {
                    #region
                    ColorDLP pnl = null;
                    for (int i = 0; i < CsConst.myColorPanels.Count; i++)
                    {
                        if (CsConst.myColorPanels[i].DIndex == dindex)
                        {
                            pnl = CsConst.myColorPanels[i];
                            break;
                        }
                    }
                    if (pnl != null)
                    {

                        if (pnl.arayPIC == null) pnl.arayPIC = new List<byte[]>();
                        if (CsConst.UploadColorDLPType == 0 || CsConst.UploadColorDLPType == 1)//背景图片
                        {
                            #region
                            int StartAddress = cboMenu.SelectedIndex * 65280;
                            if (cboMenu.SelectedIndex == 3) StartAddress = 5 * 65280;
                            else if (cboMenu.SelectedIndex == 4) StartAddress = 391680;
                            else if (cboMenu.SelectedIndex == 5) StartAddress = 65280 + 391680;
                            else if (cboMenu.SelectedIndex == 6) StartAddress = 2 * 65280 + 391680;
                            else if (cboMenu.SelectedIndex == 7) StartAddress = 4 * 65280 + 391680;
                            else if (cboMenu.SelectedIndex == 8) StartAddress = 5 * 65280 + 391680;
                            for (int i = 0; i < pnl.arayPIC.Count; i++)
                            {
                                int AddressTemp = pnl.arayPIC[i][0] * 256 * 256 + pnl.arayPIC[i][1] * 256 + pnl.arayPIC[i][2];
                                if (AddressTemp == StartAddress)
                                {
                                    pnl.arayPIC.RemoveAt(i);
                                    break;
                                }
                            }
                            byte[] source = HDLPF.BackgroundColorfulImageToByte((Bitmap)PicMenu.Image, 480, 272);
                            byte[] arayTmp = new byte[source.Length + 3];
                            arayTmp[0] = Convert.ToByte(StartAddress / 256 / 256 % 256);
                            arayTmp[1] = Convert.ToByte(StartAddress / 256 % 256);
                            arayTmp[2] = Convert.ToByte(StartAddress % 256);
                            Array.Copy(source, 0, arayTmp, 3, source.Length);
                            pnl.arayPIC.Add(arayTmp);
                            #endregion
                        }
                        if (CsConst.UploadColorDLPType == 0 || CsConst.UploadColorDLPType == 2)//图标
                        {
                            #region
                            if (cbIcon.SelectedIndex == 0)//主界面图标
                            {
                                #region
                                byte[] arayTmp = new byte[1];
                                arayTmp[0] = 0;

                                if (CsConst.mySends.AddBufToSndList(arayTmp, 0x19B2, SubnetID, DeviceID, false, false, true, false) == true)
                                {
                                    arayPosition = new byte[7] { 1, 2, 3, 4, 5, 6, 7 };
                                    Array.Copy(CsConst.myRevBuf, 26, arayPosition, 0, 7);
                                }
                                else
                                {
                                    Cursor.Current = Cursors.Default;
                                    return;
                                }

                                for (int i = 0; i < 11; i++)
                                {
                                    PictureBox temp = this.Controls.Find("picICon" + i.ToString(), true)[0] as PictureBox;
                                    if (temp.Image != null)
                                    {
                                        int StartAddress = 1305600 + 4232;
                                        int Tag = Convert.ToInt32(temp.Tag);

                                        if (i < 7)
                                        {
                                            if (arayPosition[i] == 0) continue;
                                            StartAddress = 1305600 + 4232 * (arayPosition[i] - 1);
                                        }
                                        else
                                            StartAddress = 1305600 + 4232 * Tag;
                                        for (int j = 0; j < pnl.arayPIC.Count; j++)
                                        {
                                            int AddressTemp = pnl.arayPIC[j][0] * 256 * 256 + pnl.arayPIC[j][1] * 256 + pnl.arayPIC[j][2];
                                            if (AddressTemp == StartAddress)
                                            {
                                                pnl.arayPIC.RemoveAt(j);
                                                break;
                                            }
                                        }

                                        byte[] source = HDLPF.ColorfulImageToByte((Bitmap)temp.Image, 46, 46);
                                        arayTmp = new byte[source.Length + 4];
                                        arayTmp[0] = Convert.ToByte(StartAddress / 256 / 256 % 256);
                                        arayTmp[1] = Convert.ToByte(StartAddress / 256 % 256);
                                        arayTmp[2] = Convert.ToByte(StartAddress % 256);

                                        Array.Copy(source, 0, arayTmp, 4, source.Length);
                                        pnl.arayPIC.Add(arayTmp);
                                    }
                                }
                                #endregion
                            }
                            else if (cbIcon.SelectedIndex == 1)//灯光菜单图标
                            {
                                #region
                                for (int i = 0; i < 8; i++)
                                {
                                    PictureBox temp = this.Controls.Find("picICon" + i.ToString(), true)[0] as PictureBox;
                                    if (temp.Image != null)
                                    {
                                        int Tag = Convert.ToInt32(temp.Tag);
                                        int StartAddress = 1305600 + 4232 * (Tag + 11);
                                        for (int j = 0; j < pnl.arayPIC.Count; j++)
                                        {
                                            int AddressTemp = pnl.arayPIC[j][0] * 256 * 256 + pnl.arayPIC[j][1] * 256 + pnl.arayPIC[j][2];
                                            if (AddressTemp == StartAddress)
                                            {
                                                pnl.arayPIC.RemoveAt(j);
                                                break;
                                            }
                                        }
                                        byte[] source = HDLPF.ColorfulImageToByte((Bitmap)temp.Image, 46, 46);
                                        byte[] arayTmp = new byte[source.Length + 4];
                                        arayTmp[0] = Convert.ToByte(StartAddress / 256 / 256 % 256);
                                        arayTmp[1] = Convert.ToByte(StartAddress / 256 % 256);
                                        arayTmp[2] = Convert.ToByte(StartAddress % 256);
                                        Array.Copy(source, 0, arayTmp, 4, source.Length);
                                        pnl.arayPIC.Add(arayTmp);
                                    }
                                }
                                #endregion
                            }
                            else if (cbIcon.SelectedIndex == 2 || cbIcon.SelectedIndex == 3 || cbIcon.SelectedIndex == 4 ||
                                     cbIcon.SelectedIndex == 5 || cbIcon.SelectedIndex == 6 || cbIcon.SelectedIndex == 7)//灯光1-6页图标
                            {
                                #region
                                for (int i = 0; i < 12; i++)
                                {
                                    PictureBox temp = this.Controls.Find("picICon" + i.ToString(), true)[0] as PictureBox;
                                    if (temp.Image != null)
                                    {
                                        int Tag = Convert.ToInt32(temp.Tag);
                                        int StartAddress = 1305600 + 4232 * (Tag + 19 + 12 * (cbIcon.SelectedIndex - 2));
                                        for (int j = 0; j < pnl.arayPIC.Count; j++)
                                        {
                                            int AddressTemp = pnl.arayPIC[j][0] * 256 * 256 + pnl.arayPIC[j][1] * 256 + pnl.arayPIC[j][2];
                                            if (AddressTemp == StartAddress)
                                            {
                                                pnl.arayPIC.RemoveAt(j);
                                                break;
                                            }
                                        }
                                        byte[] source = HDLPF.ColorfulImageToByte((Bitmap)temp.Image, 46, 46);
                                        byte[] arayTmp = new byte[source.Length + 4];
                                        arayTmp[0] = Convert.ToByte(StartAddress / 256 / 256 % 256);
                                        arayTmp[1] = Convert.ToByte(StartAddress / 256 % 256);
                                        arayTmp[2] = Convert.ToByte(StartAddress % 256);
                                        Array.Copy(source, 0, arayTmp, 4, source.Length);
                                        pnl.arayPIC.Add(arayTmp);
                                    }
                                }
                                #endregion
                            }
                            else if (cbIcon.SelectedIndex == 8)//AC菜单图标
                            {
                                #region
                                for (int i = 0; i < 11; i++)
                                {
                                    PictureBox temp = this.Controls.Find("picICon" + i.ToString(), true)[0] as PictureBox;
                                    if (temp.Visible && temp.Image != null)
                                    {
                                        int Tag = Convert.ToInt32(temp.Tag);
                                        int StartAddress = 1305600 + 4232 * (Tag + 91);
                                        for (int j = 0; j < pnl.arayPIC.Count; j++)
                                        {
                                            int AddressTemp = pnl.arayPIC[j][0] * 256 * 256 + pnl.arayPIC[j][1] * 256 + pnl.arayPIC[j][2];
                                            if (AddressTemp == StartAddress)
                                            {
                                                pnl.arayPIC.RemoveAt(j);
                                                break;
                                            }
                                        }
                                        byte[] source = HDLPF.ColorfulImageToByte((Bitmap)temp.Image, 46, 46);
                                        byte[] arayTmp = new byte[source.Length + 4];
                                        arayTmp[0] = Convert.ToByte(StartAddress / 256 / 256 % 256);
                                        arayTmp[1] = Convert.ToByte(StartAddress / 256 % 256);
                                        arayTmp[2] = Convert.ToByte(StartAddress % 256);
                                        Array.Copy(source, 0, arayTmp, 4, source.Length);
                                        pnl.arayPIC.Add(arayTmp);
                                    }
                                }
                                #endregion
                            }
                            else if (cbIcon.SelectedIndex == 9)//窗帘菜单图标
                            {
                                #region
                                for (int i = 0; i < 8; i++)
                                {
                                    PictureBox temp = this.Controls.Find("picICon" + i.ToString(), true)[0] as PictureBox;
                                    if (temp.Image != null)
                                    {
                                        int Tag = Convert.ToInt32(temp.Tag);
                                        int StartAddress = 1305600 + 4232 * (Tag + 110);
                                        for (int j = 0; j < pnl.arayPIC.Count; j++)
                                        {
                                            int AddressTemp = pnl.arayPIC[j][0] * 256 * 256 + pnl.arayPIC[j][1] * 256 + pnl.arayPIC[j][2];
                                            if (AddressTemp == StartAddress)
                                            {
                                                pnl.arayPIC.RemoveAt(j);
                                                break;
                                            }
                                        }
                                        byte[] source = HDLPF.ColorfulImageToByte((Bitmap)temp.Image, 46, 46);
                                        byte[] arayTmp = new byte[source.Length + 4];
                                        arayTmp[0] = Convert.ToByte(StartAddress / 256 / 256 % 256);
                                        arayTmp[1] = Convert.ToByte(StartAddress / 256 % 256);
                                        arayTmp[2] = Convert.ToByte(StartAddress % 256);
                                        Array.Copy(source, 0, arayTmp, 4, source.Length);
                                        pnl.arayPIC.Add(arayTmp);
                                    }
                                }
                                #endregion
                            }
                            else if (cbIcon.SelectedIndex == 10)//地热菜单图标
                            {
                                #region
                                for (int i = 0; i < 11; i++)
                                {
                                    PictureBox temp = this.Controls.Find("picICon" + i.ToString(), true)[0] as PictureBox;
                                    if (temp.Visible && temp.Image != null)
                                    {
                                        int Tag = Convert.ToInt32(temp.Tag);
                                        int StartAddress = 1305600 + 4232 * (Tag + 121);
                                        for (int j = 0; j < pnl.arayPIC.Count; j++)
                                        {
                                            int AddressTemp = pnl.arayPIC[j][0] * 256 * 256 + pnl.arayPIC[j][1] * 256 + pnl.arayPIC[j][2];
                                            if (AddressTemp == StartAddress)
                                            {
                                                pnl.arayPIC.RemoveAt(j);
                                                break;
                                            }
                                        }
                                        byte[] source = HDLPF.ColorfulImageToByte((Bitmap)temp.Image, 46, 46);
                                        byte[] arayTmp = new byte[source.Length + 4];
                                        arayTmp[0] = Convert.ToByte(StartAddress / 256 / 256 % 256);
                                        arayTmp[1] = Convert.ToByte(StartAddress / 256 % 256);
                                        arayTmp[2] = Convert.ToByte(StartAddress % 256);
                                        Array.Copy(source, 0, arayTmp, 4, source.Length);
                                        pnl.arayPIC.Add(arayTmp);
                                    }
                                }
                                #endregion
                            }
                            else if (cbIcon.SelectedIndex == 11)//音乐图标
                            {
                                #region
                                for (int i = 0; i < 12; i++)
                                {
                                    PictureBox temp = this.Controls.Find("picICon" + i.ToString(), true)[0] as PictureBox;
                                    if (temp.Visible && temp.Image != null)
                                    {
                                        int Tag = Convert.ToInt32(temp.Tag);
                                        int StartAddress = 1305600 + 4232 * (Tag + 137);
                                        for (int j = 0; j < pnl.arayPIC.Count; j++)
                                        {
                                            int AddressTemp = pnl.arayPIC[j][0] * 256 * 256 + pnl.arayPIC[j][1] * 256 + pnl.arayPIC[j][2];
                                            if (AddressTemp == StartAddress)
                                            {
                                                pnl.arayPIC.RemoveAt(j);
                                                break;
                                            }
                                        }
                                        byte[] source = HDLPF.ColorfulImageToByte((Bitmap)temp.Image, 46, 46);
                                        byte[] arayTmp = new byte[source.Length + 4];
                                        arayTmp[0] = Convert.ToByte(StartAddress / 256 / 256 % 256);
                                        arayTmp[1] = Convert.ToByte(StartAddress / 256 % 256);
                                        arayTmp[2] = Convert.ToByte(StartAddress % 256);
                                        Array.Copy(source, 0, arayTmp, 4, source.Length);
                                        pnl.arayPIC.Add(arayTmp);
                                    }
                                }
                                #endregion
                            }
                            else if (cbIcon.SelectedIndex == 12 || cbIcon.SelectedIndex == 13)//场景图标
                            {
                                #region
                                for (int i = 0; i < 12; i++)
                                {
                                    PictureBox temp = this.Controls.Find("picICon" + i.ToString(), true)[0] as PictureBox;
                                    if (temp.Image != null)
                                    {
                                        int Tag = Convert.ToInt32(temp.Tag);
                                        int StartAddress = 1305600 + 4232 * (Tag + 153 + 12 * (cbIcon.SelectedIndex - 12));
                                        for (int j = 0; j < pnl.arayPIC.Count; j++)
                                        {
                                            int AddressTemp = pnl.arayPIC[j][0] * 256 * 256 + pnl.arayPIC[j][1] * 256 + pnl.arayPIC[j][2];
                                            if (AddressTemp == StartAddress)
                                            {
                                                pnl.arayPIC.RemoveAt(j);
                                                break;
                                            }
                                        }
                                        byte[] source = HDLPF.ColorfulImageToByte((Bitmap)temp.Image, 46, 46);
                                        byte[] arayTmp = new byte[source.Length + 4];
                                        arayTmp[0] = Convert.ToByte(StartAddress / 256 / 256 % 256);
                                        arayTmp[1] = Convert.ToByte(StartAddress / 256 % 256);
                                        arayTmp[2] = Convert.ToByte(StartAddress % 256);
                                        Array.Copy(source, 0, arayTmp, 4, source.Length);
                                        pnl.arayPIC.Add(arayTmp);
                                    }
                                }
                                #endregion
                            }
                            #endregion
                        }
                        if (CsConst.UploadColorDLPType == 0 || CsConst.UploadColorDLPType == 3)//文字图片
                        {
                            #region
                            if (cbIcon.SelectedIndex == 0)//主界面文字
                            {
                                #region
                                byte[] arayTmp = new byte[1];
                                arayTmp[0] = 0;

                                if (CsConst.mySends.AddBufToSndList(arayTmp, 0x19B2, SubnetID, DeviceID, false, false, true, false) == true)
                                {
                                    arayPosition = new byte[7] { 1, 2, 3, 4, 5, 6, 7 };
                                    Array.Copy(CsConst.myRevBuf, 26, arayPosition, 0, 7);
                                }
                                else
                                {
                                    Cursor.Current = Cursors.Default;
                                    return;
                                }



                                for (int i = 0; i < 11; i++)
                                {
                                    PictureBox temp = this.Controls.Find("picTxt" + i.ToString(), true)[0] as PictureBox;
                                    if (temp.Image != null)
                                    {
                                        int StartAddress = 3075016 + 3840;
                                        int Tag = Convert.ToInt32(temp.Tag);

                                        if (i < 7)
                                        {
                                            if (arayPosition[i] == 0) continue;
                                            StartAddress = 3075016 + 3840 * (arayPosition[i] - 1);
                                        }
                                        else
                                            StartAddress = 3075016 + 3840 * Tag;

                                        for (int j = 0; j < pnl.arayPIC.Count; j++)
                                        {
                                            int AddressTemp = pnl.arayPIC[j][0] * 256 * 256 + pnl.arayPIC[j][1] * 256 + pnl.arayPIC[j][2];
                                            if (AddressTemp == StartAddress)
                                            {
                                                pnl.arayPIC.RemoveAt(j);
                                                break;
                                            }
                                        }

                                        byte[] source = HDLPF.ColorfulImageToByte((Bitmap)temp.Image, 24, 80);
                                        arayTmp = new byte[source.Length + 4];
                                        arayTmp[0] = Convert.ToByte(StartAddress / 256 / 256 % 256);
                                        arayTmp[1] = Convert.ToByte(StartAddress / 256 % 256);
                                        arayTmp[2] = Convert.ToByte(StartAddress % 256);

                                        Array.Copy(source, 0, arayTmp, 4, source.Length);
                                        pnl.arayPIC.Add(arayTmp);
                                    }
                                }
                                #endregion
                            }
                            else if (cbIcon.SelectedIndex == 1)//灯光菜单文字
                            {
                                #region
                                for (int i = 0; i < 8; i++)
                                {
                                    PictureBox temp = this.Controls.Find("picTxt" + i.ToString(), true)[0] as PictureBox;
                                    if (temp.Image != null)
                                    {
                                        int Tag = Convert.ToInt32(temp.Tag);
                                        int StartAddress = 3075016 + 3840 * (12 + Tag);
                                        for (int j = 0; j < pnl.arayPIC.Count; j++)
                                        {
                                            int AddressTemp = pnl.arayPIC[j][0] * 256 * 256 + pnl.arayPIC[j][1] * 256 + pnl.arayPIC[j][2];
                                            if (AddressTemp == StartAddress)
                                            {
                                                pnl.arayPIC.RemoveAt(j);
                                                break;
                                            }
                                        }

                                        byte[] source = HDLPF.ColorfulImageToByte((Bitmap)temp.Image, 24, 80);
                                        byte[] arayTmp = new byte[source.Length + 4];
                                        arayTmp[0] = Convert.ToByte(StartAddress / 256 / 256 % 256);
                                        arayTmp[1] = Convert.ToByte(StartAddress / 256 % 256);
                                        arayTmp[2] = Convert.ToByte(StartAddress % 256);

                                        Array.Copy(source, 0, arayTmp, 4, source.Length);
                                        pnl.arayPIC.Add(arayTmp);
                                    }
                                }
                                #endregion
                            }
                            else if (cbIcon.SelectedIndex == 2 || cbIcon.SelectedIndex == 3 || cbIcon.SelectedIndex == 4 ||
                                     cbIcon.SelectedIndex == 5 || cbIcon.SelectedIndex == 6 || cbIcon.SelectedIndex == 7)//灯光1-6页文字
                            {
                                #region
                                for (int i = 0; i < 12; i++)
                                {
                                    PictureBox temp = this.Controls.Find("picTxt" + i.ToString(), true)[0] as PictureBox;
                                    if (temp.Image != null)
                                    {
                                        int Tag = Convert.ToInt32(temp.Tag);
                                        int StartAddress = 3075016 + 3840 * (Tag + 20 + 12 * (cbIcon.SelectedIndex - 2));
                                        for (int j = 0; j < pnl.arayPIC.Count; j++)
                                        {
                                            int AddressTemp = pnl.arayPIC[j][0] * 256 * 256 + pnl.arayPIC[j][1] * 256 + pnl.arayPIC[j][2];
                                            if (AddressTemp == StartAddress)
                                            {
                                                pnl.arayPIC.RemoveAt(j);
                                                break;
                                            }
                                        }

                                        byte[] source = HDLPF.ColorfulImageToByte((Bitmap)temp.Image, 24, 80);
                                        byte[] arayTmp = new byte[source.Length + 4];
                                        arayTmp[0] = Convert.ToByte(StartAddress / 256 / 256 % 256);
                                        arayTmp[1] = Convert.ToByte(StartAddress / 256 % 256);
                                        arayTmp[2] = Convert.ToByte(StartAddress % 256);

                                        Array.Copy(source, 0, arayTmp, 4, source.Length);
                                        pnl.arayPIC.Add(arayTmp);
                                    }
                                }
                                #endregion
                            }
                            else if (cbIcon.SelectedIndex == 8)//AC菜单文字
                            {
                                #region
                                for (int i = 0; i < 12; i++)
                                {
                                    PictureBox temp = this.Controls.Find("picTxt" + i.ToString(), true)[0] as PictureBox;
                                    if (temp.Visible && temp.Image != null)
                                    {
                                        int Tag = Convert.ToInt32(temp.Tag);
                                        int StartAddress = 3075016 + 3840 * (Tag + 92);
                                        for (int j = 0; j < pnl.arayPIC.Count; j++)
                                        {
                                            int AddressTemp = pnl.arayPIC[j][0] * 256 * 256 + pnl.arayPIC[j][1] * 256 + pnl.arayPIC[j][2];
                                            if (AddressTemp == StartAddress)
                                            {
                                                pnl.arayPIC.RemoveAt(j);
                                                break;
                                            }
                                        }
                                        byte[] source = HDLPF.ColorfulImageToByte((Bitmap)temp.Image, 24, 80);
                                        byte[] arayTmp = new byte[source.Length + 4];
                                        arayTmp[0] = Convert.ToByte(StartAddress / 256 / 256 % 256);
                                        arayTmp[1] = Convert.ToByte(StartAddress / 256 % 256);
                                        arayTmp[2] = Convert.ToByte(StartAddress % 256);
                                        Array.Copy(source, 0, arayTmp, 4, source.Length);
                                        pnl.arayPIC.Add(arayTmp);
                                    }
                                }
                                #endregion
                            }
                            else if (cbIcon.SelectedIndex == 9)//窗帘菜单文字
                            {
                                #region
                                for (int i = 0; i < 8; i++)
                                {
                                    PictureBox temp = this.Controls.Find("picTxt" + i.ToString(), true)[0] as PictureBox;
                                    if (temp.Image != null)
                                    {
                                        int Tag = Convert.ToInt32(temp.Tag);
                                        int StartAddress = 3075016 + 3840 * (Tag + 107);
                                        for (int j = 0; j < pnl.arayPIC.Count; j++)
                                        {
                                            int AddressTemp = pnl.arayPIC[j][0] * 256 * 256 + pnl.arayPIC[j][1] * 256 + pnl.arayPIC[j][2];
                                            if (AddressTemp == StartAddress)
                                            {
                                                pnl.arayPIC.RemoveAt(j);
                                                break;
                                            }
                                        }
                                        byte[] source = HDLPF.ColorfulImageToByte((Bitmap)temp.Image, 24, 80);
                                        byte[] arayTmp = new byte[source.Length + 4];
                                        arayTmp[0] = Convert.ToByte(StartAddress / 256 / 256 % 256);
                                        arayTmp[1] = Convert.ToByte(StartAddress / 256 % 256);
                                        arayTmp[2] = Convert.ToByte(StartAddress % 256);
                                        Array.Copy(source, 0, arayTmp, 4, source.Length);
                                        pnl.arayPIC.Add(arayTmp);
                                    }
                                }
                                #endregion
                            }
                            else if (cbIcon.SelectedIndex == 10)//地热菜单文字
                            {
                                #region
                                for (int i = 0; i < 12; i++)
                                {
                                    PictureBox temp = this.Controls.Find("picTxt" + i.ToString(), true)[0] as PictureBox;
                                    if (temp.Visible && temp.Image != null)
                                    {
                                        int Tag = Convert.ToInt32(temp.Tag);
                                        int StartAddress = 3075016 + 3840 * (Tag + 137);
                                        for (int j = 0; j < pnl.arayPIC.Count; j++)
                                        {
                                            int AddressTemp = pnl.arayPIC[j][0] * 256 * 256 + pnl.arayPIC[j][1] * 256 + pnl.arayPIC[j][2];
                                            if (AddressTemp == StartAddress)
                                            {
                                                pnl.arayPIC.RemoveAt(j);
                                                break;
                                            }
                                        }
                                        byte[] source = HDLPF.ColorfulImageToByte((Bitmap)temp.Image, 24, 80);
                                        byte[] arayTmp = new byte[source.Length + 4];
                                        arayTmp[0] = Convert.ToByte(StartAddress / 256 / 256 % 256);
                                        arayTmp[1] = Convert.ToByte(StartAddress / 256 % 256);
                                        arayTmp[2] = Convert.ToByte(StartAddress % 256);
                                        Array.Copy(source, 0, arayTmp, 4, source.Length);
                                        pnl.arayPIC.Add(arayTmp);
                                    }
                                }
                                #endregion
                            }
                            else if (cbIcon.SelectedIndex == 11)//音乐文字
                            {
                                #region
                                for (int i = 0; i < 12; i++)
                                {
                                    PictureBox temp = this.Controls.Find("picTxt" + i.ToString(), true)[0] as PictureBox;
                                    if (temp.Visible && temp.Image != null)
                                    {
                                        int Tag = Convert.ToInt32(temp.Tag);
                                        int StartAddress = 3075016 + 3840 * (Tag + 148);
                                        for (int j = 0; j < pnl.arayPIC.Count; j++)
                                        {
                                            int AddressTemp = pnl.arayPIC[j][0] * 256 * 256 + pnl.arayPIC[j][1] * 256 + pnl.arayPIC[j][2];
                                            if (AddressTemp == StartAddress)
                                            {
                                                pnl.arayPIC.RemoveAt(j);
                                                break;
                                            }
                                        }
                                        byte[] source = HDLPF.ColorfulImageToByte((Bitmap)temp.Image, 24, 80);
                                        byte[] arayTmp = new byte[source.Length + 4];
                                        arayTmp[0] = Convert.ToByte(StartAddress / 256 / 256 % 256);
                                        arayTmp[1] = Convert.ToByte(StartAddress / 256 % 256);
                                        arayTmp[2] = Convert.ToByte(StartAddress % 256);
                                        Array.Copy(source, 0, arayTmp, 4, source.Length);
                                        pnl.arayPIC.Add(arayTmp);
                                    }
                                }
                                #endregion
                            }
                            else if (cbIcon.SelectedIndex == 12 || cbIcon.SelectedIndex == 13)//场景文字
                            {
                                #region
                                for (int i = 0; i < 12; i++)
                                {
                                    PictureBox temp = this.Controls.Find("picTxt" + i.ToString(), true)[0] as PictureBox;
                                    if (temp.Image != null)
                                    {
                                        int Tag = Convert.ToInt32(temp.Tag);
                                        int StartAddress = 3075016 + 3840 * (Tag + 159 + 12 * (cbIcon.SelectedIndex - 12));
                                        for (int j = 0; j < pnl.arayPIC.Count; j++)
                                        {
                                            int AddressTemp = pnl.arayPIC[j][0] * 256 * 256 + pnl.arayPIC[j][1] * 256 + pnl.arayPIC[j][2];
                                            if (AddressTemp == StartAddress)
                                            {
                                                pnl.arayPIC.RemoveAt(j);
                                                break;
                                            }
                                        }
                                        byte[] source = HDLPF.ColorfulImageToByte((Bitmap)temp.Image, 24, 80);
                                        byte[] arayTmp = new byte[source.Length + 4];
                                        arayTmp[0] = Convert.ToByte(StartAddress / 256 / 256 % 256);
                                        arayTmp[1] = Convert.ToByte(StartAddress / 256 % 256);
                                        arayTmp[2] = Convert.ToByte(StartAddress % 256);
                                        Array.Copy(source, 0, arayTmp, 4, source.Length);
                                        pnl.arayPIC.Add(arayTmp);
                                    }
                                }
                                #endregion
                            }
                            else if (cbIcon.SelectedIndex == 14 || cbIcon.SelectedIndex == 15 || cbIcon.SelectedIndex == 16 ||
                                     cbIcon.SelectedIndex == 17 || cbIcon.SelectedIndex == 18)//窗帘文字
                            {
                                #region
                                for (int i = 0; i < 4; i++)
                                {
                                    PictureBox temp = this.Controls.Find("picTxt" + i.ToString(), true)[0] as PictureBox;
                                    if (temp.Image != null)
                                    {
                                        int Tag = Convert.ToInt32(temp.Tag);
                                        int StartAddress = 3516616 + 3840 * (Tag + 4 * (cbIcon.SelectedIndex - 14));
                                        for (int j = 0; j < pnl.arayPIC.Count; j++)
                                        {
                                            int AddressTemp = pnl.arayPIC[j][0] * 256 * 256 + pnl.arayPIC[j][1] * 256 + pnl.arayPIC[j][2];
                                            if (AddressTemp == StartAddress)
                                            {
                                                pnl.arayPIC.RemoveAt(j);
                                                break;
                                            }
                                        }
                                        byte[] source = HDLPF.ColorfulImageToByte((Bitmap)temp.Image, 24, 80);
                                        byte[] arayTmp = new byte[source.Length + 4];
                                        arayTmp[0] = Convert.ToByte(StartAddress / 256 / 256 % 256);
                                        arayTmp[1] = Convert.ToByte(StartAddress / 256 % 256);
                                        arayTmp[2] = Convert.ToByte(StartAddress % 256);
                                        Array.Copy(source, 0, arayTmp, 4, source.Length);
                                        pnl.arayPIC.Add(arayTmp);
                                    }
                                }
                                #endregion
                            }
                            else if (cbIcon.SelectedIndex == 19)
                            {
                                #region
                                for (int i = 0; i < 2; i++)
                                {
                                    PictureBox temp = this.Controls.Find("picTxt" + i.ToString(), true)[0] as PictureBox;
                                    if (temp.Image != null)
                                    {
                                        int Tag = Convert.ToInt32(temp.Tag);
                                        int StartAddress = 3593416 + 3840 * Tag;
                                        for (int j = 0; j < pnl.arayPIC.Count; j++)
                                        {
                                            int AddressTemp = pnl.arayPIC[j][0] * 256 * 256 + pnl.arayPIC[j][1] * 256 + pnl.arayPIC[j][2];
                                            if (AddressTemp == StartAddress)
                                            {
                                                pnl.arayPIC.RemoveAt(j);
                                                break;
                                            }
                                        }
                                        byte[] source = HDLPF.ColorfulImageToByte((Bitmap)temp.Image, 24, 80);
                                        byte[] arayTmp = new byte[source.Length + 4];
                                        arayTmp[0] = Convert.ToByte(StartAddress / 256 / 256 % 256);
                                        arayTmp[1] = Convert.ToByte(StartAddress / 256 % 256);
                                        arayTmp[2] = Convert.ToByte(StartAddress % 256);
                                        Array.Copy(source, 0, arayTmp, 4, source.Length);
                                        pnl.arayPIC.Add(arayTmp);
                                    }
                                }
                                #endregion
                            }
                            #endregion
                        }
                    }
                    #endregion
                }
                else if (EnviroNewDeviceTypeList.EnviroNewPanelDeviceTypeList.Contains(devicetype))
                {
                    #region
                    EnviroPanel pnl = null;
                    for (int i = 0; i < CsConst.myEnviroPanels.Count; i++)
                    {
                        if (CsConst.myEnviroPanels[i].DIndex == dindex)
                        {
                            pnl = CsConst.myEnviroPanels[i];
                            break;
                        }
                    }
                    if (pnl != null)
                    {
                        if (pnl.arayPIC == null) pnl.arayPIC = new List<byte[]>();
                        if (CsConst.UploadColorDLPType == 0 || CsConst.UploadColorDLPType == 1)//背景图片
                        {
                            #region
                            int StartAddress = 567652 + cboMenu.SelectedIndex * 65280;
                            for (int i = 0; i < pnl.arayPIC.Count; i++)
                            {
                                int AddressTemp = pnl.arayPIC[i][0] * 256 * 256 + pnl.arayPIC[i][1] * 256 + pnl.arayPIC[i][2];
                                if (AddressTemp == StartAddress)
                                {
                                    pnl.arayPIC.RemoveAt(i);
                                    break;
                                }
                            }
                            byte[] source = HDLPF.BackgroundColorfulImageToByte((Bitmap)PicMenu.Image, 480, 272);
                            byte[] arayTmp = new byte[source.Length + 3];
                            arayTmp[0] = Convert.ToByte(StartAddress / 256 / 256 % 256);
                            arayTmp[1] = Convert.ToByte(StartAddress / 256 % 256);
                            arayTmp[2] = Convert.ToByte(StartAddress % 256);
                            Array.Copy(source, 0, arayTmp, 3, source.Length);
                            pnl.arayPIC.Add(arayTmp);
                            #endregion
                        }
                        if (CsConst.UploadColorDLPType == 0 || CsConst.UploadColorDLPType == 2 || CsConst.UploadColorDLPType == 3)
                        {
                            if (pnl.ReadColorPagesInformation(SubnetID, DeviceID, devicetype) == false)
                            {
                                Cursor.Current = Cursors.Default;
                                return;
                            }
                        }
                        if (CsConst.UploadColorDLPType == 0 || CsConst.UploadColorDLPType == 2)//图标
                        {
                            #region
                            byte PageID = Convert.ToByte(cbIcon.SelectedIndex + 1);
                            byte[] IConIDAry = new byte[12];
                            byte byt = 0;
                            for (int i = 0; i < pnl.IconsInPage.Count; i++)
                            {
                                IConIDAry = pnl.IconsInPage[PageID].arrIconLists;
                            }
                            for (int i = 0; i < IConIDAry.Length; i++)
                            {
                                if (IConIDAry[i] == 1 || (3 <= IConIDAry[i] && IConIDAry[i] <= 65))
                                {
                                    PictureBox temp = this.Controls.Find("picICon" + i.ToString(), true)[0] as PictureBox;
                                    if (temp.Image != null)
                                    {
                                        byte IconID = IConIDAry[i];
                                        int StartAddress = 0;
                                        if (IconID == 1)
                                        {
                                            StartAddress = 1416292;
                                        }
                                        else if (3 <= IconID && IconID <= 38)
                                        {
                                            StartAddress = 1416292 + 4232 * 9 + (IconID - 3) * 4232;
                                        }
                                        else if (39 <= IconID && IconID <= 47)
                                        {
                                            StartAddress = 1416292 + 4232 * 9 + 4232 * 36 + (IconID - 39) * 4232;
                                        }
                                        else if (48 <= IconID && IconID <= 56)
                                        {
                                            StartAddress = 1416292 + 4232 * 9 + 4232 * 36 + 4232 * 9 + 4232 * 8 + (IconID - 48) * 4232; 
                                        }
                                        else if (57 <= IconID && IconID <= 65)
                                        {
                                            StartAddress = 1416292 + 4232 * 9 + 4232 * 36 + 4232 * 9 + 4232 * 8 + 4232 * 9 + 4232 * 10 + (IconID - 57) * 4232; 
                                        }
                                        for (int j = 0; j < pnl.arayPIC.Count; j++)
                                        {
                                            int AddressTemp = pnl.arayPIC[j][0] * 256 * 256 + pnl.arayPIC[j][1] * 256 + pnl.arayPIC[j][2];
                                            if (AddressTemp == StartAddress)
                                            {
                                                pnl.arayPIC.RemoveAt(j);
                                                break;
                                            }
                                        }
                                        byte[] source = HDLPF.ColorfulImageToByte((Bitmap)temp.Image, 46, 46);
                                        byte[] arayTmp = new byte[source.Length + 4];
                                        arayTmp[0] = Convert.ToByte(StartAddress / 256 / 256 % 256);
                                        arayTmp[1] = Convert.ToByte(StartAddress / 256 % 256);
                                        arayTmp[2] = Convert.ToByte(StartAddress % 256);
                                        Array.Copy(source, 0, arayTmp, 4, source.Length);
                                        pnl.arayPIC.Add(arayTmp);
                                    }
                                }
                            }
                            #endregion
                        }
                        if (CsConst.UploadColorDLPType == 0 || CsConst.UploadColorDLPType == 3)//文字图片
                        {
                            #region
                            byte PageID = Convert.ToByte(cbIcon.SelectedIndex + 1);
                            byte[] IConIDAry = new byte[12];
                            byte byt = 0;
                            for (int i = 0; i < pnl.IconsInPage.Count; i++)
                            {
                                IConIDAry = pnl.IconsInPage[i].arrIconLists;
                            }
                            for (int i = 0; i < IConIDAry.Length; i++)
                            {
                                if (IConIDAry[i] == 1 || IConIDAry[i] == 2 || (3 <= IConIDAry[i] && IConIDAry[i] <= 65))
                                {
                                    PictureBox temp = this.Controls.Find("picTxt" + i.ToString(), true)[0] as PictureBox;
                                    if (temp.Image != null)
                                    {
                                        byte IconID = IConIDAry[i];
                                        int StartAddress = 0;
                                        if (IconID == 1)
                                        {
                                            StartAddress = 2259124;
                                        }
                                        if (IconID == 2)
                                        {
                                            StartAddress = 2259124 + 3840 * 9;
                                        }
                                        else if (3 <= IconID && IconID <= 38)
                                        {
                                            StartAddress = 2259124 + 3840 * 10 + (IconID - 3) * 3840;
                                        }
                                        else if (39 <= IconID && IconID <= 47)
                                        {
                                            StartAddress = 2259124 + 3840 * 10 + 3840 * 36 + (IconID - 39) * 3840;
                                        }
                                        else if (48 <= IconID && IconID <= 56)
                                        {
                                            StartAddress = 2259124 + 3840 * 10 + 3840 * 36 + 3840 * 9 + (IconID - 48) * 3840;
                                        }
                                        else if (57 <= IconID && IconID <= 65)
                                        {
                                            StartAddress = 2259124 + 3840 * 10 + 3840 * 36 + 3840 * 9 + 3840 * 9 + (IconID - 57) * 3840;
                                        }
                                        for (int j = 0; j < pnl.arayPIC.Count; j++)
                                        {
                                            int AddressTemp = pnl.arayPIC[j][0] * 256 * 256 + pnl.arayPIC[j][1] * 256 + pnl.arayPIC[j][2];
                                            if (AddressTemp == StartAddress)
                                            {
                                                pnl.arayPIC.RemoveAt(j);
                                                break;
                                            }
                                        }
                                        byte[] source = HDLPF.ColorfulImageToByte((Bitmap)temp.Image, 24, 80);
                                        byte[] arayTmp = new byte[source.Length + 4];
                                        arayTmp[0] = Convert.ToByte(StartAddress / 256 / 256 % 256);
                                        arayTmp[1] = Convert.ToByte(StartAddress / 256 % 256);
                                        arayTmp[2] = Convert.ToByte(StartAddress % 256);
                                        Array.Copy(source, 0, arayTmp, 4, source.Length);
                                        pnl.arayPIC.Add(arayTmp);
                                    }
                                }
                            }
                            #endregion
                        }
                    }
                    #endregion
                }
            }
            catch
            {
            }
        }

        private void savePanelIcons(int dindex, int devicetype)
        {
            try
            {
                Panel pnl = null;

                for (int i = 0; i < CsConst.myPanels.Count; i++)
                {
                    if (CsConst.myPanels[i].DIndex == dindex)
                    {
                        pnl = CsConst.myPanels[i];
                        break;
                    }
                }
                if (pnl != null)
                {
                    if (CsConst.mintNewDLPFHSetupDeviceType.Contains(devicetype))
                    {
                        #region
                        PictureBox temp = new PictureBox();
                        if (CsConst.mintMPTLDeviceType.Contains(devicetype))
                        {
                            temp.Width = 80;
                            temp.Height = 192;
                            if (pnl.IconOFF == null)
                            {
                                pnl.IconOFF = new byte[7680 + 1 + 32];
                                pnl.IconON = new byte[7680 + 1 + 32];
                                pnl.IconOFF[7680] = 1;
                                pnl.IconON[7680] = 1;
                            }
                        }
                        else
                        {
                            temp.Width = 80;
                            temp.Height = 128;
                            if (pnl.IconOFF == null)
                            {
                                pnl.IconOFF = new byte[5120 + 1 + 32];
                                pnl.IconON = new byte[5120 + 1 + 32];
                                pnl.IconOFF[5120] = 1;
                                pnl.IconON[5120] = 1;
                            }
                        }
                        for (int i = 0; i < 4; i++)
                        {
                            System.Drawing.Bitmap partImg = new System.Drawing.Bitmap(temp.Width, temp.Height);
                            for (int j = 0; j < 8; j++)
                            {
                                PictureBox temp1 = this.Controls.Find("pag" + (i + 1).ToString() + "OFF" + (j + 1).ToString(), true)[0] as PictureBox;

                                System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(partImg);
                                System.Drawing.Rectangle destRect;
                                if (j < 4)
                                    destRect = new System.Drawing.Rectangle(new System.Drawing.Point(0, j * temp1.Height), new System.Drawing.Size(temp1.Width, temp1.Height));
                                else
                                    destRect = new System.Drawing.Rectangle(new System.Drawing.Point(temp1.Width, (j-4) * temp1.Height), new System.Drawing.Size(temp1.Width, temp1.Height));
                                System.Drawing.Rectangle origRect = new System.Drawing.Rectangle(new System.Drawing.Point(0, 0), new System.Drawing.Size(temp1.Width, temp1.Height));//原图位置（默认从原图中截取的图片大小等于目标图片的大小）
                                graphics.DrawImage(temp1.Image, destRect, origRect, System.Drawing.GraphicsUnit.Pixel);
                            }
                            temp.Image = partImg;
                            byte[] arayTmp = null;
                            if (CsConst.mintMPTLDeviceType.Contains(devicetype))
                            {
                                arayTmp=HDLPF.ImageToByte(partImg, temp.Width, temp.Height, 2);
                            }
                            else
                            {
                                arayTmp=HDLPF.ImageToByte(partImg, temp.Width, temp.Height, 1);
                            }
                            Array.Copy(arayTmp, 0, pnl.IconOFF, i * arayTmp.Length, arayTmp.Length);

                            for (int j = 0; j < 8; j++)
                            {
                                PictureBox temp2 = this.Controls.Find("pag" + (i + 1).ToString() + "ON" + (j + 1).ToString(), true)[0] as PictureBox;
                                System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(partImg);
                                System.Drawing.Rectangle destRect;
                                if (j < 4)
                                    destRect = new System.Drawing.Rectangle(new System.Drawing.Point(0, j * temp2.Height), new System.Drawing.Size(temp2.Width, temp2.Height));
                                else
                                    destRect = new System.Drawing.Rectangle(new System.Drawing.Point(temp2.Width, (j - 4) * temp2.Height), new System.Drawing.Size(temp2.Width, temp2.Height));
                                System.Drawing.Rectangle origRect = new System.Drawing.Rectangle(new System.Drawing.Point(0, 0), new System.Drawing.Size(temp2.Width, temp2.Height));//原图位置（默认从原图中截取的图片大小等于目标图片的大小）
                                graphics.DrawImage(temp2.Image, destRect, origRect, System.Drawing.GraphicsUnit.Pixel);
                            }
                            temp.Image = partImg;
                            
                            if (CsConst.mintMPTLDeviceType.Contains(devicetype))
                            {
                                arayTmp = HDLPF.ImageToByte(partImg, temp.Width, temp.Height, 2);
                            }
                            else
                            {
                                arayTmp = HDLPF.ImageToByte(partImg, temp.Width, temp.Height, 1);
                            }
                            Array.Copy(arayTmp, 0, pnl.IconON, i * arayTmp.Length, arayTmp.Length);
                        }
                        #endregion
                    }
                    else if (CsConst.mintDLPDeviceType.Contains(devicetype) && !CsConst.mintNewDLPFHSetupDeviceType.Contains(devicetype))
                    {
                        #region
                        PictureBox temp = new PictureBox();
                        temp.Width = 80;
                        temp.Height = 128;
                        if (pnl.IconOFF == null)
                        {
                            pnl.IconOFF = new byte[5120 + 1 + 32];
                            pnl.IconON = new byte[5120 + 1 + 32];
                            pnl.IconOFF[5120] = 1;
                            pnl.IconON[5120] = 1;
                        }
                        for (int i = 0; i < 4; i++)
                        {
                            System.Drawing.Bitmap partImg = new System.Drawing.Bitmap(temp.Width, temp.Height);
                            for (int j = 0; j < 4; j++)
                            {
                                PictureBox temp1 = this.Controls.Find("pag" + (i + 1).ToString() + "OFF" + (j + 1).ToString(), true)[0] as PictureBox;
                                System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(partImg);
                                System.Drawing.Rectangle destRect = new System.Drawing.Rectangle(new System.Drawing.Point(0, j * temp1.Height), new System.Drawing.Size(temp1.Width, temp1.Height));
                                System.Drawing.Rectangle origRect = new System.Drawing.Rectangle(new System.Drawing.Point(0, 0), new System.Drawing.Size(temp1.Width, temp1.Height));//原图位置（默认从原图中截取的图片大小等于目标图片的大小）
                                graphics.DrawImage(temp1.Image, destRect, origRect, System.Drawing.GraphicsUnit.Pixel);
                            }
                            temp.Image = partImg;
                            byte[] arayTmp = HDLPF.ImageToByte(partImg, temp.Width, temp.Height, 0);
                            Array.Copy(arayTmp, 0, pnl.IconOFF, i * arayTmp.Length, arayTmp.Length);

                            for (int j = 0; j < 4; j++)
                            {
                                PictureBox temp2 = this.Controls.Find("pag" + (i + 1).ToString() + "ON" + (j + 1).ToString(), true)[0] as PictureBox;
                                System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(partImg);
                                System.Drawing.Rectangle destRect = new System.Drawing.Rectangle(new System.Drawing.Point(0, j * temp2.Height), new System.Drawing.Size(temp2.Width, temp2.Height));
                                System.Drawing.Rectangle origRect = new System.Drawing.Rectangle(new System.Drawing.Point(0, 0), new System.Drawing.Size(temp2.Width, temp2.Height));//原图位置（默认从原图中截取的图片大小等于目标图片的大小）
                                graphics.DrawImage(temp2.Image, destRect, origRect, System.Drawing.GraphicsUnit.Pixel);
                            }
                            temp.Image = partImg;
                            arayTmp = HDLPF.ImageToByte(partImg, temp.Width, temp.Height, 0);
                            Array.Copy(arayTmp, 0, pnl.IconON, i * arayTmp.Length, arayTmp.Length);
                        }
                        #endregion
                    }
                    else if (devicetype == 5003)
                    {
                        #region
                        int Count = 288 + 288 + 288 + 288 * 5 + 52 * 12 + 288 * 2 + 288 * 2 + 72 + 72 + 72 + 288 + 288 * 30 + 72 * 8 + 288 * 4 + 288 + 288 + 288 * 3 + 288 * 3;
                        pnl.IconOFF = new byte[Count];
                        for (int i = 1; i <= 3; i++)
                        {
                            PictureBox temp = this.Controls.Find("picH" + i.ToString(), true)[0] as PictureBox;
                            System.Drawing.Bitmap partImg = new System.Drawing.Bitmap(temp.Width, temp.Height);
                            System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(partImg);
                            System.Drawing.Rectangle destRect = new System.Drawing.Rectangle(new System.Drawing.Point(0, 0), new System.Drawing.Size(temp.Width, temp.Height));
                            System.Drawing.Rectangle origRect = new System.Drawing.Rectangle(new System.Drawing.Point(0, 0), new System.Drawing.Size(temp.Width, temp.Height));//原图位置（默认从原图中截取的图片大小等于目标图片的大小）
                            graphics.DrawImage(temp.Image, destRect, origRect, System.Drawing.GraphicsUnit.Pixel);
                            byte[] arayTmp = HDLPF.ImageToByte(partImg, temp.Width, temp.Height, 3);
                            Array.Copy(arayTmp, 0, pnl.IconOFF, (i - 1) * arayTmp.Length, arayTmp.Length);
                        }
                        for (int i = 6; i <= 10; i++)
                        {
                            PictureBox temp = this.Controls.Find("picH" + i.ToString(), true)[0] as PictureBox;
                            System.Drawing.Bitmap partImg = new System.Drawing.Bitmap(temp.Width, temp.Height);
                            System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(partImg);
                            System.Drawing.Rectangle destRect = new System.Drawing.Rectangle(new System.Drawing.Point(0, 0), new System.Drawing.Size(temp.Width, temp.Height));
                            System.Drawing.Rectangle origRect = new System.Drawing.Rectangle(new System.Drawing.Point(0, 0), new System.Drawing.Size(temp.Width, temp.Height));//原图位置（默认从原图中截取的图片大小等于目标图片的大小）
                            graphics.DrawImage(temp.Image, destRect, origRect, System.Drawing.GraphicsUnit.Pixel);
                            byte[] arayTmp = HDLPF.ImageToByte(partImg, temp.Width, temp.Height, 3);
                            Array.Copy(arayTmp, 0, pnl.IconOFF, (i - 6) * arayTmp.Length + 864, arayTmp.Length);
                        }
                        
                        for (int i = 11; i <= 20; i++)
                        {
                            PictureBox temp = this.Controls.Find("picH" + i.ToString(), true)[0] as PictureBox;
                            System.Drawing.Bitmap partImg = new System.Drawing.Bitmap(temp.Width, temp.Height);
                            System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(partImg);
                            System.Drawing.Rectangle destRect = new System.Drawing.Rectangle(new System.Drawing.Point(0, 0), new System.Drawing.Size(temp.Width, temp.Height));
                            System.Drawing.Rectangle origRect = new System.Drawing.Rectangle(new System.Drawing.Point(0, 0), new System.Drawing.Size(temp.Width, temp.Height));//原图位置（默认从原图中截取的图片大小等于目标图片的大小）
                            graphics.DrawImage(temp.Image, destRect, origRect, System.Drawing.GraphicsUnit.Pixel);
                            byte[] arayTmp = HDLPF.ImageToByte(partImg, temp.Width, temp.Height, 3);
                            Array.Copy(arayTmp, 0, pnl.IconOFF, (i - 11) * arayTmp.Length + 2304, arayTmp.Length);
                        }
                        for (int i = 75; i <= 76; i++)
                        {
                            PictureBox temp = this.Controls.Find("picH" + i.ToString(), true)[0] as PictureBox;
                            System.Drawing.Bitmap partImg = new System.Drawing.Bitmap(temp.Width, temp.Height);
                            System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(partImg);
                            System.Drawing.Rectangle destRect = new System.Drawing.Rectangle(new System.Drawing.Point(0, 0), new System.Drawing.Size(temp.Width, temp.Height));
                            System.Drawing.Rectangle origRect = new System.Drawing.Rectangle(new System.Drawing.Point(0, 0), new System.Drawing.Size(temp.Width, temp.Height));//原图位置（默认从原图中截取的图片大小等于目标图片的大小）
                            graphics.DrawImage(temp.Image, destRect, origRect, System.Drawing.GraphicsUnit.Pixel);
                            byte[] arayTmp = HDLPF.ImageToByte(partImg, temp.Width, temp.Height, 3);
                            Array.Copy(arayTmp, 0, pnl.IconOFF, (i - 75) * arayTmp.Length + 2824, arayTmp.Length);
                        }
                        for (int i = 21; i <= 22; i++)
                        {
                            PictureBox temp = this.Controls.Find("picH" + i.ToString(), true)[0] as PictureBox;
                            System.Drawing.Bitmap partImg = new System.Drawing.Bitmap(temp.Width, temp.Height);
                            System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(partImg);
                            System.Drawing.Rectangle destRect = new System.Drawing.Rectangle(new System.Drawing.Point(0, 0), new System.Drawing.Size(temp.Width, temp.Height));
                            System.Drawing.Rectangle origRect = new System.Drawing.Rectangle(new System.Drawing.Point(0, 0), new System.Drawing.Size(temp.Width, temp.Height));//原图位置（默认从原图中截取的图片大小等于目标图片的大小）
                            graphics.DrawImage(temp.Image, destRect, origRect, System.Drawing.GraphicsUnit.Pixel);
                            byte[] arayTmp = HDLPF.ImageToByte(partImg, temp.Width, temp.Height, 3);
                            Array.Copy(arayTmp, 0, pnl.IconOFF, (i - 21) * arayTmp.Length + 2928, arayTmp.Length);
                        }
                        for (int i = 26; i <= 27; i++)
                        {
                            PictureBox temp = this.Controls.Find("picH" + i.ToString(), true)[0] as PictureBox;
                            System.Drawing.Bitmap partImg = new System.Drawing.Bitmap(temp.Width, temp.Height);
                            System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(partImg);
                            System.Drawing.Rectangle destRect = new System.Drawing.Rectangle(new System.Drawing.Point(0, 0), new System.Drawing.Size(temp.Width, temp.Height));
                            System.Drawing.Rectangle origRect = new System.Drawing.Rectangle(new System.Drawing.Point(0, 0), new System.Drawing.Size(temp.Width, temp.Height));//原图位置（默认从原图中截取的图片大小等于目标图片的大小）
                            graphics.DrawImage(temp.Image, destRect, origRect, System.Drawing.GraphicsUnit.Pixel);
                            byte[] arayTmp = HDLPF.ImageToByte(partImg, temp.Width, temp.Height, 3);
                            Array.Copy(arayTmp, 0, pnl.IconOFF, (i - 26) * arayTmp.Length + 3504, arayTmp.Length);
                        }
                        for (int i = 29; i <= 31; i++)
                        {
                            PictureBox temp = this.Controls.Find("picH" + i.ToString(), true)[0] as PictureBox;
                            System.Drawing.Bitmap partImg = new System.Drawing.Bitmap(temp.Width, temp.Height);
                            System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(partImg);
                            System.Drawing.Rectangle destRect = new System.Drawing.Rectangle(new System.Drawing.Point(0, 0), new System.Drawing.Size(temp.Width, temp.Height));
                            System.Drawing.Rectangle origRect = new System.Drawing.Rectangle(new System.Drawing.Point(0, 0), new System.Drawing.Size(temp.Width, temp.Height));//原图位置（默认从原图中截取的图片大小等于目标图片的大小）
                            graphics.DrawImage(temp.Image, destRect, origRect, System.Drawing.GraphicsUnit.Pixel);
                            byte[] arayTmp = HDLPF.ImageToByte(partImg, temp.Width, temp.Height, 3);
                            Array.Copy(arayTmp, 0, pnl.IconOFF, (i - 29) * arayTmp.Length + 4080, arayTmp.Length);
                        }
                        for (int i = 28; i <= 28; i++)
                        {
                            PictureBox temp = this.Controls.Find("picH" + i.ToString(), true)[0] as PictureBox;
                            System.Drawing.Bitmap partImg = new System.Drawing.Bitmap(temp.Width, temp.Height);
                            System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(partImg);
                            System.Drawing.Rectangle destRect = new System.Drawing.Rectangle(new System.Drawing.Point(0, 0), new System.Drawing.Size(temp.Width, temp.Height));
                            System.Drawing.Rectangle origRect = new System.Drawing.Rectangle(new System.Drawing.Point(0, 0), new System.Drawing.Size(temp.Width, temp.Height));//原图位置（默认从原图中截取的图片大小等于目标图片的大小）
                            graphics.DrawImage(temp.Image, destRect, origRect, System.Drawing.GraphicsUnit.Pixel);
                            byte[] arayTmp = HDLPF.ImageToByte(partImg, temp.Width, temp.Height, 3);
                            Array.Copy(arayTmp, 0, pnl.IconOFF, (i - 28) * arayTmp.Length + 4296, arayTmp.Length);
                        }
                        for (int i = 44; i <= 73; i++)
                        {
                            PictureBox temp = this.Controls.Find("picH" + i.ToString(), true)[0] as PictureBox;
                            System.Drawing.Bitmap partImg = new System.Drawing.Bitmap(temp.Width, temp.Height);
                            System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(partImg);
                            System.Drawing.Rectangle destRect = new System.Drawing.Rectangle(new System.Drawing.Point(0, 0), new System.Drawing.Size(temp.Width, temp.Height));
                            System.Drawing.Rectangle origRect = new System.Drawing.Rectangle(new System.Drawing.Point(0, 0), new System.Drawing.Size(temp.Width, temp.Height));//原图位置（默认从原图中截取的图片大小等于目标图片的大小）
                            graphics.DrawImage(temp.Image, destRect, origRect, System.Drawing.GraphicsUnit.Pixel);
                            byte[] arayTmp = HDLPF.ImageToByte(partImg, temp.Width, temp.Height, 3);
                            Array.Copy(arayTmp, 0, pnl.IconOFF, (i - 44) * arayTmp.Length + 4584, arayTmp.Length);
                        }
                        for (int i = 36; i <= 43; i++)
                        {
                            PictureBox temp = this.Controls.Find("picH" + i.ToString(), true)[0] as PictureBox;
                            System.Drawing.Bitmap partImg = new System.Drawing.Bitmap(temp.Width, temp.Height);
                            System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(partImg);
                            System.Drawing.Rectangle destRect = new System.Drawing.Rectangle(new System.Drawing.Point(0, 0), new System.Drawing.Size(temp.Width, temp.Height));
                            System.Drawing.Rectangle origRect = new System.Drawing.Rectangle(new System.Drawing.Point(0, 0), new System.Drawing.Size(temp.Width, temp.Height));//原图位置（默认从原图中截取的图片大小等于目标图片的大小）
                            graphics.DrawImage(temp.Image, destRect, origRect, System.Drawing.GraphicsUnit.Pixel);
                            byte[] arayTmp = HDLPF.ImageToByte(partImg, temp.Width, temp.Height, 3);
                            Array.Copy(arayTmp, 0, pnl.IconOFF, (i - 36) * arayTmp.Length + 13224, arayTmp.Length);
                        }
                        for (int i = 32; i <= 35; i++)
                        {
                            PictureBox temp = this.Controls.Find("picH" + i.ToString(), true)[0] as PictureBox;
                            System.Drawing.Bitmap partImg = new System.Drawing.Bitmap(temp.Width, temp.Height);
                            System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(partImg);
                            System.Drawing.Rectangle destRect = new System.Drawing.Rectangle(new System.Drawing.Point(0, 0), new System.Drawing.Size(temp.Width, temp.Height));
                            System.Drawing.Rectangle origRect = new System.Drawing.Rectangle(new System.Drawing.Point(0, 0), new System.Drawing.Size(temp.Width, temp.Height));//原图位置（默认从原图中截取的图片大小等于目标图片的大小）
                            graphics.DrawImage(temp.Image, destRect, origRect, System.Drawing.GraphicsUnit.Pixel);
                            byte[] arayTmp = HDLPF.ImageToByte(partImg, temp.Width, temp.Height, 3);
                            Array.Copy(arayTmp, 0, pnl.IconOFF, (i - 32) * arayTmp.Length + 13800, arayTmp.Length);
                        }
                        for (int i = 4; i <= 4; i++)
                        {
                            PictureBox temp = this.Controls.Find("picH" + i.ToString(), true)[0] as PictureBox;
                            System.Drawing.Bitmap partImg = new System.Drawing.Bitmap(temp.Width, temp.Height);
                            System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(partImg);
                            System.Drawing.Rectangle destRect = new System.Drawing.Rectangle(new System.Drawing.Point(0, 0), new System.Drawing.Size(temp.Width, temp.Height));
                            System.Drawing.Rectangle origRect = new System.Drawing.Rectangle(new System.Drawing.Point(0, 0), new System.Drawing.Size(temp.Width, temp.Height));//原图位置（默认从原图中截取的图片大小等于目标图片的大小）
                            graphics.DrawImage(temp.Image, destRect, origRect, System.Drawing.GraphicsUnit.Pixel);
                            byte[] arayTmp = HDLPF.ImageToByte(partImg, temp.Width, temp.Height, 3);
                            Array.Copy(arayTmp, 0, pnl.IconOFF, (i - 4) * arayTmp.Length + 14952, arayTmp.Length);
                        }
                        for (int i = 5; i <= 5; i++)
                        {
                            PictureBox temp = this.Controls.Find("picH" + i.ToString(), true)[0] as PictureBox;
                            System.Drawing.Bitmap partImg = new System.Drawing.Bitmap(temp.Width, temp.Height);
                            System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(partImg);
                            System.Drawing.Rectangle destRect = new System.Drawing.Rectangle(new System.Drawing.Point(0, 0), new System.Drawing.Size(temp.Width, temp.Height));
                            System.Drawing.Rectangle origRect = new System.Drawing.Rectangle(new System.Drawing.Point(0, 0), new System.Drawing.Size(temp.Width, temp.Height));//原图位置（默认从原图中截取的图片大小等于目标图片的大小）
                            graphics.DrawImage(temp.Image, destRect, origRect, System.Drawing.GraphicsUnit.Pixel);
                            byte[] arayTmp = HDLPF.ImageToByte(partImg, temp.Width, temp.Height, 3);
                            Array.Copy(arayTmp, 0, pnl.IconOFF, (i - 5) * arayTmp.Length + 15240, arayTmp.Length);
                        }
                        for (int i = 23; i <= 25; i++)
                        {
                            PictureBox temp = this.Controls.Find("picH" + i.ToString(), true)[0] as PictureBox;
                            System.Drawing.Bitmap partImg = new System.Drawing.Bitmap(temp.Width, temp.Height);
                            System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(partImg);
                            System.Drawing.Rectangle destRect = new System.Drawing.Rectangle(new System.Drawing.Point(0, 0), new System.Drawing.Size(temp.Width, temp.Height));
                            System.Drawing.Rectangle origRect = new System.Drawing.Rectangle(new System.Drawing.Point(0, 0), new System.Drawing.Size(temp.Width, temp.Height));//原图位置（默认从原图中截取的图片大小等于目标图片的大小）
                            graphics.DrawImage(temp.Image, destRect, origRect, System.Drawing.GraphicsUnit.Pixel);
                            byte[] arayTmp = HDLPF.ImageToByte(partImg, temp.Width, temp.Height, 3);
                            Array.Copy(arayTmp, 0, pnl.IconOFF, (i - 23) * arayTmp.Length + 15528, arayTmp.Length);
                        }
                        for (int i = 77; i <= 79; i++)
                        {
                            PictureBox temp = this.Controls.Find("picH" + i.ToString(), true)[0] as PictureBox;
                            System.Drawing.Bitmap partImg = new System.Drawing.Bitmap(temp.Width, temp.Height);
                            System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(partImg);
                            System.Drawing.Rectangle destRect = new System.Drawing.Rectangle(new System.Drawing.Point(0, 0), new System.Drawing.Size(temp.Width, temp.Height));
                            System.Drawing.Rectangle origRect = new System.Drawing.Rectangle(new System.Drawing.Point(0, 0), new System.Drawing.Size(temp.Width, temp.Height));//原图位置（默认从原图中截取的图片大小等于目标图片的大小）
                            graphics.DrawImage(temp.Image, destRect, origRect, System.Drawing.GraphicsUnit.Pixel);
                            byte[] arayTmp = HDLPF.ImageToByte(partImg, temp.Width, temp.Height, 3);
                            Array.Copy(arayTmp, 0, pnl.IconOFF, (i - 77) * arayTmp.Length + 16392, arayTmp.Length);
                        }
                        #endregion
                    }
                    else if (devicetype == 5064)
                    {
                        #region
                        int Count = 512 + 512 + 512 + 128 * 12 + 108 * 16 + 512 * 5 + 38 * 4 + 38 * 5 + 128 * 2 + 64 * 12 + 1024 + 14 * 12 + 1024;
                        pnl.IconOFF = new byte[Count];
                        for (int i = 1; i <= 3; i++)
                        {
                            PictureBox temp = this.Controls.Find("picO" + i.ToString(), true)[0] as PictureBox;
                            System.Drawing.Bitmap partImg = new System.Drawing.Bitmap(temp.Width, temp.Height);
                            System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(partImg);
                            System.Drawing.Rectangle destRect = new System.Drawing.Rectangle(new System.Drawing.Point(0, 0), new System.Drawing.Size(temp.Width, temp.Height));
                            System.Drawing.Rectangle origRect = new System.Drawing.Rectangle(new System.Drawing.Point(0, 0), new System.Drawing.Size(temp.Width, temp.Height));//原图位置（默认从原图中截取的图片大小等于目标图片的大小）
                            graphics.DrawImage(temp.Image, destRect, origRect, System.Drawing.GraphicsUnit.Pixel);
                            byte[] arayTmp = HDLPF.ImageToByte(partImg, temp.Width, temp.Height, 3);
                            Array.Copy(arayTmp, 0, pnl.IconOFF, (i - 1) * arayTmp.Length, arayTmp.Length);
                        }//1536

                        for (int i = 4; i <= 15; i++)
                        {
                            PictureBox temp = this.Controls.Find("picO" + i.ToString(), true)[0] as PictureBox;
                            System.Drawing.Bitmap partImg = new System.Drawing.Bitmap(temp.Width, temp.Height);
                            System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(partImg);
                            System.Drawing.Rectangle destRect = new System.Drawing.Rectangle(new System.Drawing.Point(0, 0), new System.Drawing.Size(temp.Width, temp.Height));
                            System.Drawing.Rectangle origRect = new System.Drawing.Rectangle(new System.Drawing.Point(0, 0), new System.Drawing.Size(temp.Width, temp.Height));//原图位置（默认从原图中截取的图片大小等于目标图片的大小）
                            graphics.DrawImage(temp.Image, destRect, origRect, System.Drawing.GraphicsUnit.Pixel);
                            byte[] arayTmp = HDLPF.ImageToByte(partImg, temp.Width, temp.Height, 3);
                            Array.Copy(arayTmp, 0, pnl.IconOFF, (i - 4) * arayTmp.Length + 1536, arayTmp.Length);
                        }//3072

                        for (int i = 16; i <= 31; i++)
                        {
                            PictureBox temp = this.Controls.Find("picO" + i.ToString(), true)[0] as PictureBox;
                            System.Drawing.Bitmap partImg = new System.Drawing.Bitmap(temp.Width, temp.Height);
                            System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(partImg);
                            System.Drawing.Rectangle destRect = new System.Drawing.Rectangle(new System.Drawing.Point(0, 0), new System.Drawing.Size(temp.Width, temp.Height));
                            System.Drawing.Rectangle origRect = new System.Drawing.Rectangle(new System.Drawing.Point(0, 0), new System.Drawing.Size(temp.Width, temp.Height));//原图位置（默认从原图中截取的图片大小等于目标图片的大小）
                            graphics.DrawImage(temp.Image, destRect, origRect, System.Drawing.GraphicsUnit.Pixel);
                            byte[] arayTmp = HDLPF.ImageToByte(partImg, temp.Width, temp.Height, 3);
                            Array.Copy(arayTmp, 0, pnl.IconOFF, (i - 16) * arayTmp.Length + 3072, arayTmp.Length);
                        }//4800

                        for (int i = 32; i <= 36; i++)
                        {
                            PictureBox temp = this.Controls.Find("picO" + i.ToString(), true)[0] as PictureBox;
                            System.Drawing.Bitmap partImg = new System.Drawing.Bitmap(temp.Width, temp.Height);
                            System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(partImg);
                            System.Drawing.Rectangle destRect = new System.Drawing.Rectangle(new System.Drawing.Point(0, 0), new System.Drawing.Size(temp.Width, temp.Height));
                            System.Drawing.Rectangle origRect = new System.Drawing.Rectangle(new System.Drawing.Point(0, 0), new System.Drawing.Size(temp.Width, temp.Height));//原图位置（默认从原图中截取的图片大小等于目标图片的大小）
                            graphics.DrawImage(temp.Image, destRect, origRect, System.Drawing.GraphicsUnit.Pixel);
                            byte[] arayTmp = HDLPF.ImageToByte(partImg, temp.Width, temp.Height, 3);
                            Array.Copy(arayTmp, 0, pnl.IconOFF, (i - 32) * arayTmp.Length + 4800, arayTmp.Length);
                        }//7360

                        for (int i = 37; i <= 45; i++)
                        {
                            PictureBox temp = this.Controls.Find("picO" + i.ToString(), true)[0] as PictureBox;
                            System.Drawing.Bitmap partImg = new System.Drawing.Bitmap(temp.Width, temp.Height);
                            System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(partImg);
                            System.Drawing.Rectangle destRect = new System.Drawing.Rectangle(new System.Drawing.Point(0, 0), new System.Drawing.Size(temp.Width, temp.Height));
                            System.Drawing.Rectangle origRect = new System.Drawing.Rectangle(new System.Drawing.Point(0, 0), new System.Drawing.Size(temp.Width, temp.Height));//原图位置（默认从原图中截取的图片大小等于目标图片的大小）
                            graphics.DrawImage(temp.Image, destRect, origRect, System.Drawing.GraphicsUnit.Pixel);
                            byte[] arayTmp = HDLPF.ImageToByte(partImg, temp.Width, temp.Height, 3);
                            Array.Copy(arayTmp, 0, pnl.IconOFF, (i - 37) * arayTmp.Length + 7360, arayTmp.Length);
                        }//7702

                        for (int i = 46; i <= 47; i++)
                        {
                            PictureBox temp = this.Controls.Find("picO" + i.ToString(), true)[0] as PictureBox;
                            System.Drawing.Bitmap partImg = new System.Drawing.Bitmap(temp.Width, temp.Height);
                            System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(partImg);
                            System.Drawing.Rectangle destRect = new System.Drawing.Rectangle(new System.Drawing.Point(0, 0), new System.Drawing.Size(temp.Width, temp.Height));
                            System.Drawing.Rectangle origRect = new System.Drawing.Rectangle(new System.Drawing.Point(0, 0), new System.Drawing.Size(temp.Width, temp.Height));//原图位置（默认从原图中截取的图片大小等于目标图片的大小）
                            graphics.DrawImage(temp.Image, destRect, origRect, System.Drawing.GraphicsUnit.Pixel);
                            byte[] arayTmp = HDLPF.ImageToByte(partImg, temp.Width, temp.Height, 3);
                            Array.Copy(arayTmp, 0, pnl.IconOFF, (i - 46) * arayTmp.Length + 7702, arayTmp.Length);
                        }//7958

                        for (int i = 48; i <= 59; i++)
                        {
                            PictureBox temp = this.Controls.Find("picO" + i.ToString(), true)[0] as PictureBox;
                            System.Drawing.Bitmap partImg = new System.Drawing.Bitmap(temp.Width, temp.Height);
                            System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(partImg);
                            System.Drawing.Rectangle destRect = new System.Drawing.Rectangle(new System.Drawing.Point(0, 0), new System.Drawing.Size(temp.Width, temp.Height));
                            System.Drawing.Rectangle origRect = new System.Drawing.Rectangle(new System.Drawing.Point(0, 0), new System.Drawing.Size(temp.Width, temp.Height));//原图位置（默认从原图中截取的图片大小等于目标图片的大小）
                            graphics.DrawImage(temp.Image, destRect, origRect, System.Drawing.GraphicsUnit.Pixel);
                            byte[] arayTmp = HDLPF.ImageToByte(partImg, temp.Width, temp.Height, 3);
                            Array.Copy(arayTmp, 0, pnl.IconOFF, (i - 48) * arayTmp.Length + 7958, arayTmp.Length);
                        }//8726

                        for (int i = 72; i <= 72; i++)
                        {
                            PictureBox temp = this.Controls.Find("picO" + i.ToString(), true)[0] as PictureBox;
                            System.Drawing.Bitmap partImg = new System.Drawing.Bitmap(temp.Width, temp.Height);
                            System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(partImg);
                            System.Drawing.Rectangle destRect = new System.Drawing.Rectangle(new System.Drawing.Point(0, 0), new System.Drawing.Size(temp.Width, temp.Height));
                            System.Drawing.Rectangle origRect = new System.Drawing.Rectangle(new System.Drawing.Point(0, 0), new System.Drawing.Size(temp.Width, temp.Height));//原图位置（默认从原图中截取的图片大小等于目标图片的大小）
                            graphics.DrawImage(temp.Image, destRect, origRect, System.Drawing.GraphicsUnit.Pixel);
                            byte[] arayTmp = HDLPF.ImageToByte(partImg, temp.Width, temp.Height, 3);
                            Array.Copy(arayTmp, 0, pnl.IconOFF, (i - 72) * arayTmp.Length + 8726, arayTmp.Length);
                        }//9750

                        for (int i = 60; i <= 71; i++)
                        {
                            PictureBox temp = this.Controls.Find("picO" + i.ToString(), true)[0] as PictureBox;
                            System.Drawing.Bitmap partImg = new System.Drawing.Bitmap(temp.Width, temp.Height);
                            System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(partImg);
                            System.Drawing.Rectangle destRect = new System.Drawing.Rectangle(new System.Drawing.Point(0, 0), new System.Drawing.Size(temp.Width, temp.Height));
                            System.Drawing.Rectangle origRect = new System.Drawing.Rectangle(new System.Drawing.Point(0, 0), new System.Drawing.Size(temp.Width, temp.Height));//原图位置（默认从原图中截取的图片大小等于目标图片的大小）
                            graphics.DrawImage(temp.Image, destRect, origRect, System.Drawing.GraphicsUnit.Pixel);
                            byte[] arayTmp = HDLPF.ImageToByte(partImg, temp.Width, temp.Height, 3);
                            Array.Copy(arayTmp, 0, pnl.IconOFF, (i - 60) * arayTmp.Length + 9750, arayTmp.Length);
                        }//9918
                        for (int i = 73; i <= 73; i++)
                        {
                            PictureBox temp = this.Controls.Find("picO" + i.ToString(), true)[0] as PictureBox;
                            System.Drawing.Bitmap partImg = new System.Drawing.Bitmap(temp.Width, temp.Height);
                            System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(partImg);
                            System.Drawing.Rectangle destRect = new System.Drawing.Rectangle(new System.Drawing.Point(0, 0), new System.Drawing.Size(temp.Width, temp.Height));
                            System.Drawing.Rectangle origRect = new System.Drawing.Rectangle(new System.Drawing.Point(0, 0), new System.Drawing.Size(temp.Width, temp.Height));//原图位置（默认从原图中截取的图片大小等于目标图片的大小）
                            graphics.DrawImage(temp.Image, destRect, origRect, System.Drawing.GraphicsUnit.Pixel);
                            byte[] arayTmp = HDLPF.ImageToByte(partImg, temp.Width, temp.Height, 3);
                            Array.Copy(arayTmp, 0, pnl.IconOFF, (i - 73) * arayTmp.Length + 9918, arayTmp.Length);
                        }//1024
                        #endregion
                    }
                }
            }
            catch
            {
            }
        }

        private void showDownloadPanelIcons(int dindex,int devicetype)
        {
            try
            {
                Panel pnl = null;
                for (int i = 0; i < CsConst.myPanels.Count; i++)
                {
                    if (CsConst.myPanels[i].DIndex == dindex)
                    {
                        pnl = CsConst.myPanels[i];
                        break;
                    }
                }

                if (CsConst.mintNewDLPFHSetupDeviceType.Contains(devicetype))
                {
                    #region
                    int num1 = 80;
                    int num2 = 128;
                    int num3 = 1280;
                    if (CsConst.mintMPTLDeviceType.Contains(devicetype))
                    {
                        num2 = 192;
                        num3 = 1920;
                    }

                    for (int i = 0; i < 4; i++)
                    {
                        if ((i + 1) == CsConst.DownloadPictruePageID || CsConst.DownloadPictruePageID == 255)
                        {
                            byte[] arayTmp1 = new byte[num3];
                            byte[] arayTmp2 = new byte[num3];
                            Array.Copy(pnl.IconOFF, i * num3, arayTmp1, 0, num3);
                            Image img1 = HDLPF.ByteToImage(arayTmp1, num1, num2, devicetype);
                            Array.Copy(pnl.IconON, i * num3, arayTmp2, 0, num3);
                            Image img2 = HDLPF.ByteToImage(arayTmp2, num1, num2, devicetype);

                            for (int j = 0; j < 4; j++)
                            {
                                PictureBox temp1 = this.Controls.Find("pag" + (i + 1).ToString() + "OFF" + (j + 1).ToString(), true)[0] as PictureBox;
                                PictureBox temp2 = this.Controls.Find("pag" + (i + 1).ToString() + "OFF" + (j + 5).ToString(), true)[0] as PictureBox;
                                PictureBox temp3 = this.Controls.Find("pag" + (i + 1).ToString() + "ON" + (j + 1).ToString(), true)[0] as PictureBox;
                                PictureBox temp4 = this.Controls.Find("pag" + (i + 1).ToString() + "ON" + (j + 5).ToString(), true)[0] as PictureBox;

                                System.Drawing.Bitmap partImg = new System.Drawing.Bitmap(temp1.Width, temp1.Height);
                                System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(partImg);
                                System.Drawing.Rectangle destRect = new System.Drawing.Rectangle(new System.Drawing.Point(0, 0), new System.Drawing.Size(temp1.Width, temp1.Height));//目标位置
                                System.Drawing.Rectangle origRect = new System.Drawing.Rectangle(new System.Drawing.Point(0, temp1.Height * j), new System.Drawing.Size(temp1.Width, temp1.Height));//原图位置（默认从原图中截取的图片大小等于目标图片的大小）
                                graphics.DrawImage(img1, destRect, origRect, System.Drawing.GraphicsUnit.Pixel);
                                temp1.Image = partImg;
                                temp1.Image.Tag = i + 1;

                                partImg = new System.Drawing.Bitmap(temp2.Width, temp2.Height);
                                graphics = System.Drawing.Graphics.FromImage(partImg);
                                destRect = new System.Drawing.Rectangle(new System.Drawing.Point(0, 0), new System.Drawing.Size(temp2.Width, temp2.Height));//目标位置
                                origRect = new System.Drawing.Rectangle(new System.Drawing.Point(temp2.Width, temp2.Height * j), new System.Drawing.Size(temp2.Width, temp2.Height));//原图位置（默认从原图中截取的图片大小等于目标图片的大小）
                                graphics.DrawImage(img1, destRect, origRect, System.Drawing.GraphicsUnit.Pixel);
                                temp2.Image = partImg;
                                temp2.Image.Tag = i + 1;

                                partImg = new System.Drawing.Bitmap(temp3.Width, temp3.Height);
                                graphics = System.Drawing.Graphics.FromImage(partImg);
                                destRect = new System.Drawing.Rectangle(new System.Drawing.Point(0, 0), new System.Drawing.Size(temp3.Width, temp3.Height));//目标位置
                                origRect = new System.Drawing.Rectangle(new System.Drawing.Point(0, temp3.Height * j), new System.Drawing.Size(temp3.Width, temp3.Height));//原图位置（默认从原图中截取的图片大小等于目标图片的大小）
                                graphics.DrawImage(img2, destRect, origRect, System.Drawing.GraphicsUnit.Pixel);
                                temp3.Image = partImg;
                                temp3.Image.Tag = i + 1;

                                partImg = new System.Drawing.Bitmap(temp4.Width, temp4.Height);
                                graphics = System.Drawing.Graphics.FromImage(partImg);
                                destRect = new System.Drawing.Rectangle(new System.Drawing.Point(0, 0), new System.Drawing.Size(temp4.Width, temp4.Height));//目标位置
                                origRect = new System.Drawing.Rectangle(new System.Drawing.Point(temp4.Width, temp4.Height * j), new System.Drawing.Size(temp4.Width, temp4.Height));//原图位置（默认从原图中截取的图片大小等于目标图片的大小）
                                graphics.DrawImage(img2, destRect, origRect, System.Drawing.GraphicsUnit.Pixel);
                                temp4.Image = partImg;
                                temp4.Image.Tag = i + 1;

                            }
                        }
                    }
                    #endregion
                }
                else
                {
                    #region
                    for (int i = 0; i < 4; i++)
                    {
                        if ((i + 1) == CsConst.DownloadPictruePageID || CsConst.DownloadPictruePageID==255)
                        {
                            byte[] arayTmp1 = new byte[1280];
                            byte[] arayTmp2 = new byte[1280];
                            Array.Copy(pnl.IconOFF, i * 1280, arayTmp1, 0, 1280);
                            Array.Copy(pnl.IconON, i * 1280, arayTmp2, 0, 1280);
                            Image img1 = HDLPF.ByteToImage(arayTmp1, 80, 128, devicetype);
                            Image img2 = HDLPF.ByteToImage(arayTmp2, 80, 128, devicetype);

                            for (int j = 0; j < 4; j++)
                            {
                                PictureBox temp1 = this.Controls.Find("pag" + (i + 1).ToString() + "OFF" + (j + 1).ToString(), true)[0] as PictureBox;
                                PictureBox temp2 = this.Controls.Find("pag" + (i + 1).ToString() + "ON" + (j + 1).ToString(), true)[0] as PictureBox;
                                System.Drawing.Bitmap partImg = new System.Drawing.Bitmap(temp1.Width, temp1.Height);
                                System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(partImg);
                                System.Drawing.Rectangle destRect = new System.Drawing.Rectangle(new System.Drawing.Point(0, 0), new System.Drawing.Size(temp1.Width, temp1.Height));//目标位置
                                System.Drawing.Rectangle origRect = new System.Drawing.Rectangle(new System.Drawing.Point(0, temp1.Height * j), new System.Drawing.Size(temp1.Width, temp1.Height));//原图位置（默认从原图中截取的图片大小等于目标图片的大小）
                                graphics.DrawImage(img1, destRect, origRect, System.Drawing.GraphicsUnit.Pixel);
                                temp1.Image = partImg;
                                temp1.Image.Tag = i + 1;

                                partImg = new System.Drawing.Bitmap(temp2.Width, temp2.Height);
                                graphics = System.Drawing.Graphics.FromImage(partImg);
                                destRect = new System.Drawing.Rectangle(new System.Drawing.Point(0, 0), new System.Drawing.Size(temp2.Width, temp2.Height));//目标位置
                                origRect = new System.Drawing.Rectangle(new System.Drawing.Point(0, temp2.Height * j), new System.Drawing.Size(temp2.Width, temp2.Height));//原图位置（默认从原图中截取的图片大小等于目标图片的大小）
                                graphics.DrawImage(img2, destRect, origRect, System.Drawing.GraphicsUnit.Pixel);
                                temp2.Image = partImg;
                                temp2.Image.Tag = i + 1;
                            }
                        }
                    }
                    #endregion
                }
            }
            catch
            {
            }
        }

        private void btnClearBorder_Click(object sender, EventArgs e)
        {
            try
            {
                for (int i = 1; i <= 4; i++)
                {
                    PictureBox temp = this.Controls.Find("picDIYTmp" + i.ToString(), true)[0] as PictureBox;
                    Graphics g = temp.CreateGraphics();
                    Rectangle rect = new Rectangle(temp.ClientRectangle.X, temp.ClientRectangle.Location.Y,
                                                   temp.ClientRectangle.X + temp.ClientRectangle.Width - 1,
                                                   temp.ClientRectangle.Y + temp.ClientRectangle.Height - 1);
                    System.Drawing.Pen p = new System.Drawing.Pen(System.Drawing.Color.White);
                    g.DrawRectangle(p, rect);

                    PictureBox temp2 = this.Controls.Find("picDIY" + i.ToString(), true)[0] as PictureBox;
                    g = temp2.CreateGraphics();
                    rect = new Rectangle(temp2.ClientRectangle.X, temp2.ClientRectangle.Location.Y,
                                                   temp2.ClientRectangle.X + temp2.ClientRectangle.Width - 1,
                                                   temp2.ClientRectangle.Y + temp2.ClientRectangle.Height - 1);
                    p = new System.Drawing.Pen(System.Drawing.Color.White);
                    g.DrawRectangle(p, rect);
                }
            }
            catch
            {
            }
        }

        private void chb1_CheckedChanged(object sender, EventArgs e)
        {
            if (chb1.Checked)
            {
                Graphics g = picDIY1.CreateGraphics();
                g.DrawLine(new System.Drawing.Pen(System.Drawing.Color.Black), 1, 0, picDIY1.Width - 2, 0);

                g = picDIYTmp1.CreateGraphics();
                g.DrawLine(new System.Drawing.Pen(System.Drawing.Color.Black), 1, 0, picDIYTmp1.Width - 2, 0);
            }
            else
            {
                Graphics g = picDIY1.CreateGraphics();
                g.DrawLine(new System.Drawing.Pen(System.Drawing.Color.White), 1, 0, picDIY1.Width - 2, 0);

                g = picDIYTmp1.CreateGraphics();
                g.DrawLine(new System.Drawing.Pen(System.Drawing.Color.White), 1, 0, picDIYTmp1.Width - 2, 0);
            }
        }

        private void chb2_CheckedChanged(object sender, EventArgs e)
        {
            if (chb2.Checked)
            {
                Graphics g1 = picDIY1.CreateGraphics();
                g1.DrawLine(new System.Drawing.Pen(System.Drawing.Color.Black), 1, picDIY1.Height - 1, picDIY1.Width - 2, picDIY1.Height - 1);
                Graphics g2 = picDIY2.CreateGraphics();
                g2.DrawLine(new System.Drawing.Pen(System.Drawing.Color.Black), 1, 0, picDIY2.Width - 2, 0);

                g1 = picDIYTmp1.CreateGraphics();
                g1.DrawLine(new System.Drawing.Pen(System.Drawing.Color.Black), 1, picDIYTmp1.Height - 1, picDIYTmp1.Width - 2, picDIYTmp1.Height - 1);
                g2 = picDIYTmp2.CreateGraphics();
                g2.DrawLine(new System.Drawing.Pen(System.Drawing.Color.Black), 1, 0, picDIYTmp2.Width - 2, 0);
            }
            else
            {
                Graphics g1 = picDIY1.CreateGraphics();
                g1.DrawLine(new System.Drawing.Pen(System.Drawing.Color.White), 1, picDIY1.Height - 1, picDIY1.Width - 2, picDIY1.Height - 1);
                Graphics g2 = picDIY2.CreateGraphics();
                g2.DrawLine(new System.Drawing.Pen(System.Drawing.Color.White), 1, 0, picDIY2.Width - 2, 0);

                g1 = picDIYTmp1.CreateGraphics();
                g1.DrawLine(new System.Drawing.Pen(System.Drawing.Color.White), 1, picDIYTmp1.Height - 1, picDIYTmp1.Width - 2, picDIYTmp1.Height - 1);
                g2 = picDIYTmp2.CreateGraphics();
                g2.DrawLine(new System.Drawing.Pen(System.Drawing.Color.White), 1, 0, picDIYTmp2.Width - 2, 0);
            }
        }

        private void chb3_CheckedChanged(object sender, EventArgs e)
        {
            if (chb3.Checked)
            {
                Graphics g1 = picDIY2.CreateGraphics();
                g1.DrawLine(new System.Drawing.Pen(System.Drawing.Color.Black), 1, picDIY2.Height - 1, picDIY2.Width - 2, picDIY2.Height - 1);
                Graphics g2 = picDIY3.CreateGraphics();
                g2.DrawLine(new System.Drawing.Pen(System.Drawing.Color.Black), 1, 0, picDIY3.Width - 2, 0);

                g1 = picDIYTmp2.CreateGraphics();
                g1.DrawLine(new System.Drawing.Pen(System.Drawing.Color.Black), 1, picDIYTmp2.Height - 1, picDIYTmp2.Width - 2, picDIYTmp2.Height - 1);
                g2 = picDIYTmp3.CreateGraphics();
                g2.DrawLine(new System.Drawing.Pen(System.Drawing.Color.Black), 1, 0, picDIYTmp3.Width - 2, 0);
            }
            else
            {
                Graphics g1 = picDIY2.CreateGraphics();
                g1.DrawLine(new System.Drawing.Pen(System.Drawing.Color.White), 1, picDIY2.Height - 1, picDIY2.Width - 2, picDIY2.Height - 1);
                Graphics g2 = picDIY3.CreateGraphics();
                g2.DrawLine(new System.Drawing.Pen(System.Drawing.Color.White), 1, 0, picDIY3.Width - 2, 0);

                g1 = picDIYTmp2.CreateGraphics();
                g1.DrawLine(new System.Drawing.Pen(System.Drawing.Color.White), 1, picDIYTmp2.Height - 1, picDIYTmp2.Width - 2, picDIYTmp2.Height - 1);
                g2 = picDIYTmp3.CreateGraphics();
                g2.DrawLine(new System.Drawing.Pen(System.Drawing.Color.White), 1, 0, picDIYTmp3.Width - 2, 0);
            }
        }

        private void chb4_CheckedChanged(object sender, EventArgs e)
        {
            if (chb4.Checked)
            {
                Graphics g1 = picDIY3.CreateGraphics();
                g1.DrawLine(new System.Drawing.Pen(System.Drawing.Color.Black), 1, picDIY3.Height - 1, picDIY3.Width - 2, picDIY3.Height - 1);
                Graphics g2 = picDIY4.CreateGraphics();
                g2.DrawLine(new System.Drawing.Pen(System.Drawing.Color.Black), 1, 0, picDIY4.Width - 2, 0);

                g1 = picDIYTmp3.CreateGraphics();
                g1.DrawLine(new System.Drawing.Pen(System.Drawing.Color.Black), 1, picDIYTmp3.Height - 1, picDIYTmp3.Width - 2, picDIYTmp3.Height - 1);
                g2 = picDIYTmp4.CreateGraphics();
                g2.DrawLine(new System.Drawing.Pen(System.Drawing.Color.Black), 1, 0, picDIYTmp4.Width - 2, 0);
            }
            else
            {
                Graphics g1 = picDIY3.CreateGraphics();
                g1.DrawLine(new System.Drawing.Pen(System.Drawing.Color.White), 1, picDIY3.Height - 1, picDIY3.Width - 2, picDIY3.Height - 1);
                Graphics g2 = picDIY4.CreateGraphics();
                g2.DrawLine(new System.Drawing.Pen(System.Drawing.Color.White), 1, 0, picDIY4.Width - 2, 0);

                g1 = picDIYTmp3.CreateGraphics();
                g1.DrawLine(new System.Drawing.Pen(System.Drawing.Color.White), 1, picDIYTmp3.Height - 1, picDIYTmp3.Width - 2, picDIYTmp3.Height - 1);
                g2 = picDIYTmp4.CreateGraphics();
                g2.DrawLine(new System.Drawing.Pen(System.Drawing.Color.White), 1, 0, picDIYTmp4.Width - 2, 0);
            }
        }

        private void chb5_CheckedChanged(object sender, EventArgs e)
        {
            if (chb5.Checked)
            {
                Graphics g = picDIY4.CreateGraphics();
                g.DrawLine(new System.Drawing.Pen(System.Drawing.Color.Black), 1, picDIY4.Height - 1, picDIY4.Width - 2, picDIY4.Height - 1);

                g = picDIYTmp4.CreateGraphics();
                g.DrawLine(new System.Drawing.Pen(System.Drawing.Color.Black), 1, picDIYTmp4.Height - 1, picDIYTmp4.Width - 2, picDIYTmp4.Height - 1);
            }
            else
            {
                Graphics g = picDIY4.CreateGraphics();
                g.DrawLine(new System.Drawing.Pen(System.Drawing.Color.White), 1, picDIY4.Height - 1, picDIY4.Width - 2, picDIY4.Height - 1);

                g = picDIYTmp4.CreateGraphics();
                g.DrawLine(new System.Drawing.Pen(System.Drawing.Color.White), 1, picDIYTmp4.Height - 1, picDIYTmp4.Width - 2, picDIYTmp4.Height - 1);
            }
        }

        private void btnFont_Click(object sender, EventArgs e)
        {
            if (SelectedText == 0) SelectedText = 1;
            if (Fonts.ShowDialog() == DialogResult.OK)
            {
                MyFont[SelectedText - 1] = Fonts.Font;
                WriteStringToIcons();
            }
        }

        private void btnClearText_Click(object sender, EventArgs e)
        {
            txtDIY1.Text = "";
            txtDIY2.Text = "";
            txtDIY3.Text = "";
            txtDIY4.Text = "";
            myString = new string[4];
            WriteStringToIcons();
        }

        private void WriteStringToIcons()
        {
            if (SelectedText == 1)
            {
                #region
                if (myString[SelectedText - 1] == null) myString[SelectedText - 1] = "";
                string str = myString[SelectedText - 1];
                Font font = MyFont[SelectedText - 1];
                Bitmap image = new Bitmap(picDIY1.Width, picDIY1.Height);
                picDIYTmp1.DrawToBitmap(image, new Rectangle(0, 0, picDIY1.Width, picDIY1.Height));
                Graphics g = Graphics.FromImage(image);
                System.Drawing.Brush brush = System.Drawing.Brushes.Black;
                PointF point = new PointF(6 + MyMoveLeft[SelectedText - 1], 8 + MyMoveTop[SelectedText-1]);
                System.Drawing.StringFormat sf = new System.Drawing.StringFormat();
                sf.FormatFlags = StringFormatFlags.DisplayFormatControl;
                g.DrawString(str.ToString(), font, brush, point, sf);
                picDIY1.Image = image;
                #endregion
            }
            else if (SelectedText == 2)
            {
                #region
                if (myString[SelectedText - 1] == null) myString[SelectedText - 1] = "";
                string str = myString[SelectedText - 1];
                Font font = MyFont[SelectedText - 1];
                Bitmap image = new Bitmap(picDIY2.Width, picDIY2.Height);
                picDIYTmp2.DrawToBitmap(image, new Rectangle(0, 0, picDIY2.Width, picDIY2.Height));
                Graphics g = Graphics.FromImage(image);
                System.Drawing.Brush brush = System.Drawing.Brushes.Black;
                PointF point = new PointF(6 + MyMoveLeft[SelectedText - 1], 8 + MyMoveTop[SelectedText - 1]);
                System.Drawing.StringFormat sf = new System.Drawing.StringFormat();
                sf.FormatFlags = StringFormatFlags.DisplayFormatControl;
                g.DrawString(str.ToString(), font, brush, point, sf);
                picDIY2.Image = image;
                #endregion
            }
            else if (SelectedText == 3)
            {
                #region
                if (myString[SelectedText - 1] == null) myString[SelectedText - 1] = "";
                string str = myString[SelectedText - 1];
                Font font = MyFont[SelectedText - 1];
                Bitmap image = new Bitmap(picDIY3.Width, picDIY3.Height);
                picDIYTmp3.DrawToBitmap(image, new Rectangle(0, 0, picDIY3.Width, picDIY3.Height));
                Graphics g = Graphics.FromImage(image);
                System.Drawing.Brush brush = System.Drawing.Brushes.Black;
                PointF point = new PointF(6 + MyMoveLeft[SelectedText - 1], 8 + MyMoveTop[SelectedText - 1]);
                System.Drawing.StringFormat sf = new System.Drawing.StringFormat();
                sf.FormatFlags = StringFormatFlags.DisplayFormatControl;
                g.DrawString(str.ToString(), font, brush, point, sf);
                picDIY3.Image = image;
                #endregion
            }
            else if (SelectedText == 4)
            {
                #region
                if (myString[SelectedText - 1] == null) myString[SelectedText - 1] = "";
                string str = myString[SelectedText - 1];
                Font font = MyFont[SelectedText - 1];
                Bitmap image = new Bitmap(picDIY4.Width, picDIY4.Height);
                picDIYTmp4.DrawToBitmap(image, new Rectangle(0, 0, picDIY4.Width, picDIY4.Height));
                Graphics g = Graphics.FromImage(image);
                System.Drawing.Brush brush = System.Drawing.Brushes.Black;
                PointF point = new PointF(6 + MyMoveLeft[SelectedText - 1], 8 + MyMoveTop[SelectedText - 1]);
                System.Drawing.StringFormat sf = new System.Drawing.StringFormat();
                sf.FormatFlags = StringFormatFlags.DisplayFormatControl;
                g.DrawString(str.ToString(), font, brush, point, sf);
                picDIY4.Image = image;
                #endregion
            }
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            if (SelectedText == 0) return;
            MyMoveTop[SelectedText - 1] += 1;
            WriteStringToIcons();
        }

        private void btnTop_Click(object sender, EventArgs e)
        {
            if (SelectedText == 0) return;
            MyMoveTop[SelectedText - 1] -= 1;
            WriteStringToIcons();
        }

        private void btnLeft_Click(object sender, EventArgs e)
        {
            if (SelectedText == 0) return;
            MyMoveLeft[SelectedText - 1] -= 1;
            WriteStringToIcons();
        }

        private void btnRight_Click(object sender, EventArgs e)
        {
            if (SelectedText == 0) return;
            MyMoveLeft[SelectedText - 1] += 1;
            WriteStringToIcons();
        }

        private void txtDIY1_Click(object sender, EventArgs e)
        {
            SelectedText = 1;
        }

        private void txtDIY2_Click(object sender, EventArgs e)
        {
            SelectedText = 2;
        }

        private void txtDIY3_Click(object sender, EventArgs e)
        {
            SelectedText = 3;
        }

        private void txtDIY4_Click(object sender, EventArgs e)
        {
            SelectedText = 4;
        }

        private void txtDIY1_TextChanged(object sender, EventArgs e)
        {
            if (picDIYTmp1.Width == 40 && picDIYTmp1.Height == 32)
                picDIYTmp1.Image = Image.FromFile(Application.StartupPath + @"\Icon\DLP\Empty1.bmp");
            else if (picDIYTmp1.Width == 80 && picDIYTmp1.Height == 32)
                picDIYTmp1.Image = Image.FromFile(Application.StartupPath + @"\Icon\DLP\Empty2.bmp");
            else if (picDIYTmp1.Width == 40 && picDIYTmp1.Height == 48)
                picDIYTmp1.Image = Image.FromFile(Application.StartupPath + @"\Icon\DLP\Empty3.bmp");
            else if (picDIYTmp1.Width == 80 && picDIYTmp1.Height == 48)
                picDIYTmp1.Image = Image.FromFile(Application.StartupPath + @"\Icon\DLP\Empty4.bmp");
            SelectedText = 1;
            if (myString == null) myString = new string[4];
            myString[SelectedText - 1] = txtDIY1.Text;
            WriteStringToIcons();
        }

        private void txtDIY2_TextChanged(object sender, EventArgs e)
        {
            if (picDIYTmp2.Width == 40 && picDIYTmp2.Height == 32)
                picDIYTmp2.Image = Image.FromFile(Application.StartupPath + @"\Icon\DLP\Empty1.bmp");
            else if (picDIYTmp2.Width == 80 && picDIYTmp2.Height == 32)
                picDIYTmp2.Image = Image.FromFile(Application.StartupPath + @"\Icon\DLP\Empty2.bmp");
            else if (picDIYTmp2.Width == 40 && picDIYTmp2.Height == 48)
                picDIYTmp2.Image = Image.FromFile(Application.StartupPath + @"\Icon\DLP\Empty3.bmp");
            else if (picDIYTmp2.Width == 80 && picDIYTmp2.Height == 48)
                picDIYTmp2.Image = Image.FromFile(Application.StartupPath + @"\Icon\DLP\Empty4.bmp");
            SelectedText = 2;
            if (myString == null) myString = new string[4];
            myString[SelectedText - 1] = txtDIY2.Text;
            WriteStringToIcons();
        }

        private void txtDIY3_TextChanged(object sender, EventArgs e)
        {
            if (picDIYTmp3.Width == 40 && picDIYTmp3.Height == 32)
                picDIYTmp3.Image = Image.FromFile(Application.StartupPath + @"\Icon\DLP\Empty1.bmp");
            else if (picDIYTmp3.Width == 80 && picDIYTmp3.Height == 32)
                picDIYTmp3.Image = Image.FromFile(Application.StartupPath + @"\Icon\DLP\Empty2.bmp");
            else if (picDIYTmp3.Width == 40 && picDIYTmp3.Height == 48)
                picDIYTmp3.Image = Image.FromFile(Application.StartupPath + @"\Icon\DLP\Empty3.bmp");
            else if (picDIYTmp3.Width == 80 && picDIYTmp3.Height == 48)
                picDIYTmp3.Image = Image.FromFile(Application.StartupPath + @"\Icon\DLP\Empty4.bmp");
            SelectedText = 3;
            if (myString == null) myString = new string[4];
            myString[SelectedText - 1] = txtDIY3.Text;
            WriteStringToIcons();
        }

        private void txtDIY4_TextChanged(object sender, EventArgs e)
        {
            if (picDIYTmp4.Width == 40 && picDIYTmp4.Height == 32)
                picDIYTmp4.Image = Image.FromFile(Application.StartupPath + @"\Icon\DLP\Empty1.bmp");
            else if (picDIYTmp4.Width == 80 && picDIYTmp4.Height == 32)
                picDIYTmp4.Image = Image.FromFile(Application.StartupPath + @"\Icon\DLP\Empty2.bmp");
            else if (picDIYTmp4.Width == 40 && picDIYTmp4.Height == 48)
                picDIYTmp4.Image = Image.FromFile(Application.StartupPath + @"\Icon\DLP\Empty3.bmp");
            else if (picDIYTmp4.Width == 80 && picDIYTmp4.Height == 48)
                picDIYTmp4.Image = Image.FromFile(Application.StartupPath + @"\Icon\DLP\Empty4.bmp");
            SelectedText = 4;
            if (myString == null) myString = new string[4];
            myString[SelectedText - 1] = txtDIY4.Text;
            WriteStringToIcons();
        }

        private void numValue_ValueChanged(object sender, EventArgs e)
        {
            if (SelectedText == 0) SelectedText = 1;
            MyFont[SelectedText - 1] = new Font(MyFont[SelectedText - 1].Name, (float)numValue.Value);
            WriteStringToIcons();
        }

        private void picDIY1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Point Tmp = e.Location;
            string MyPath = HDLPF.OpenFileDialog("BMP Image|*.bmp|JPEG Image|*.jpg|PNG Image|*.png", "Add Icons");
            if (MyPath == null || MyPath == "") return;
            DrawIconsAccordinglyPs(sender, 1, Tmp.X, Tmp.Y, MyPath);
        }

        private void DrawIconsAccordinglyPs(object sender, byte bytType, int intX, int intY, string FileName)
        {
            if (sender == null) return;
            Image img = Image.FromFile(FileName);
            if (img.Width % ((PictureBox)sender).Width != 0 || img.Height % ((PictureBox)sender).Height != 0)
            {
                MessageBox.Show(CsConst.mstrINIDefault.IniReadValue("Public", "99811", "") + ((PictureBox)sender).Width.ToString() + "*" + ((PictureBox)sender).Height.ToString());
                return;
            }

            int Tag = Convert.ToInt32(((PictureBox)sender).Tag);
            if (Tag == 1)
            {
                #region
                int imgHeight = img.Height;
                int num = imgHeight / ((PictureBox)sender).Height;

                for (int i = 0; i < num; i++)
                {
                    if (i <= 3)
                    {
                        PictureBox temp = this.Controls.Find("picDIY" + (Tag + i).ToString(), true)[0] as PictureBox;
                        PictureBox temp2 = this.Controls.Find("picDIYTmp" + (Tag + i).ToString(), true)[0] as PictureBox;
                        System.Drawing.Bitmap partImg = new System.Drawing.Bitmap(temp.Width, temp.Height);
                        System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(partImg);
                        System.Drawing.Rectangle destRect = new System.Drawing.Rectangle(new System.Drawing.Point(0, 0), new System.Drawing.Size(temp.Width, temp.Height));//目标位置
                        System.Drawing.Rectangle origRect = new System.Drawing.Rectangle(new System.Drawing.Point(0, temp.Height * i), new System.Drawing.Size(temp.Width, temp.Height));//原图位置（默认从原图中截取的图片大小等于目标图片的大小）
                        graphics.DrawImage(img, destRect, origRect, System.Drawing.GraphicsUnit.Pixel);
                        temp.Image = partImg;
                        temp2.Image = partImg;
                    }
                }
                #endregion
            }
            else if (Tag == 2)
            {
                #region
                int imgHeight = img.Height;
                int num = imgHeight / ((PictureBox)sender).Height;
                if (num >= 4)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        PictureBox temp = this.Controls.Find("picDIY" + (i + 1).ToString(), true)[0] as PictureBox;
                        PictureBox temp2 = this.Controls.Find("picDIYTmp" + (i + 1).ToString(), true)[0] as PictureBox;
                        System.Drawing.Bitmap partImg = new System.Drawing.Bitmap(temp.Width, temp.Height);
                        System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(partImg);
                        System.Drawing.Rectangle destRect = new System.Drawing.Rectangle(new System.Drawing.Point(0, 0), new System.Drawing.Size(temp.Width, temp.Height));//目标位置
                        System.Drawing.Rectangle origRect = new System.Drawing.Rectangle(new System.Drawing.Point(0, temp.Height * i), new System.Drawing.Size(temp.Width, temp.Height));//原图位置（默认从原图中截取的图片大小等于目标图片的大小）
                        graphics.DrawImage(img, destRect, origRect, System.Drawing.GraphicsUnit.Pixel);
                        temp.Image = partImg;
                        temp2.Image = partImg;
                    }
                }
                else
                {
                    for (int i = 0; i < num; i++)
                    {
                        if (num <= 2)
                        {
                            PictureBox temp = this.Controls.Find("picDIY" + (Tag + i).ToString(), true)[0] as PictureBox;
                            PictureBox temp2 = this.Controls.Find("picDIYTmp" + (Tag + i).ToString(), true)[0] as PictureBox;
                            System.Drawing.Bitmap partImg = new System.Drawing.Bitmap(temp.Width, temp.Height);
                            System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(partImg);
                            System.Drawing.Rectangle destRect = new System.Drawing.Rectangle(new System.Drawing.Point(0, 0), new System.Drawing.Size(temp.Width, temp.Height));//目标位置
                            System.Drawing.Rectangle origRect = new System.Drawing.Rectangle(new System.Drawing.Point(0, temp.Height * i), new System.Drawing.Size(temp.Width, temp.Height));//原图位置（默认从原图中截取的图片大小等于目标图片的大小）
                            graphics.DrawImage(img, destRect, origRect, System.Drawing.GraphicsUnit.Pixel);
                            temp.Image = partImg;
                            temp2.Image = partImg;
                        }
                    }
                }
                #endregion
            }
            else if (Tag == 3)
            {
                #region
                int imgHeight = img.Height;
                int num = imgHeight / ((PictureBox)sender).Height;
                if (num >= 4)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        PictureBox temp = this.Controls.Find("picDIY" + (i + 1).ToString(), true)[0] as PictureBox;
                        PictureBox temp2 = this.Controls.Find("picDIYTmp" + (i + 1).ToString(), true)[0] as PictureBox;
                        System.Drawing.Bitmap partImg = new System.Drawing.Bitmap(temp.Width, temp.Height);
                        System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(partImg);
                        System.Drawing.Rectangle destRect = new System.Drawing.Rectangle(new System.Drawing.Point(0, 0), new System.Drawing.Size(temp.Width, temp.Height));//目标位置
                        System.Drawing.Rectangle origRect = new System.Drawing.Rectangle(new System.Drawing.Point(0, temp.Height * i), new System.Drawing.Size(temp.Width, temp.Height));//原图位置（默认从原图中截取的图片大小等于目标图片的大小）
                        graphics.DrawImage(img, destRect, origRect, System.Drawing.GraphicsUnit.Pixel);
                        temp.Image = partImg;
                        temp2.Image = partImg;
                    }
                }
                else
                {
                    for (int i = 0; i < num; i++)
                    {
                        if (num <= 1)
                        {
                            PictureBox temp = this.Controls.Find("picDIY" + (Tag + i).ToString(), true)[0] as PictureBox;
                            PictureBox temp2 = this.Controls.Find("picDIYTmp" + (Tag + i).ToString(), true)[0] as PictureBox;
                            System.Drawing.Bitmap partImg = new System.Drawing.Bitmap(temp.Width, temp.Height);
                            System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(partImg);
                            System.Drawing.Rectangle destRect = new System.Drawing.Rectangle(new System.Drawing.Point(0, 0), new System.Drawing.Size(temp.Width, temp.Height));//目标位置
                            System.Drawing.Rectangle origRect = new System.Drawing.Rectangle(new System.Drawing.Point(0, temp.Height * i), new System.Drawing.Size(temp.Width, temp.Height));//原图位置（默认从原图中截取的图片大小等于目标图片的大小）
                            graphics.DrawImage(img, destRect, origRect, System.Drawing.GraphicsUnit.Pixel);
                            temp.Image = partImg;
                            temp2.Image = partImg;
                        }
                    }
                }
                #endregion
            }
            else if (Tag == 4)
            {
                #region
                int imgHeight = img.Height;
                int num = imgHeight / ((PictureBox)sender).Height;
                if (num >= 4)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        PictureBox temp = this.Controls.Find("picDIY" + (i + 1).ToString(), true)[0] as PictureBox;
                        PictureBox temp2 = this.Controls.Find("picDIYTmp" + (i + 1).ToString(), true)[0] as PictureBox;
                        System.Drawing.Bitmap partImg = new System.Drawing.Bitmap(temp.Width, temp.Height);
                        System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(partImg);
                        System.Drawing.Rectangle destRect = new System.Drawing.Rectangle(new System.Drawing.Point(0, 0), new System.Drawing.Size(temp.Width, temp.Height));//目标位置
                        System.Drawing.Rectangle origRect = new System.Drawing.Rectangle(new System.Drawing.Point(0, temp.Height * i), new System.Drawing.Size(temp.Width, temp.Height));//原图位置（默认从原图中截取的图片大小等于目标图片的大小）
                        graphics.DrawImage(img, destRect, origRect, System.Drawing.GraphicsUnit.Pixel);
                        temp.Image = partImg;
                        temp2.Image = partImg;
                    }
                }
                else
                {
                    System.Drawing.Bitmap partImg = new System.Drawing.Bitmap(picDIY4.Width, picDIY4.Height);
                    System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(partImg);
                    System.Drawing.Rectangle destRect = new System.Drawing.Rectangle(new System.Drawing.Point(0, 0), new System.Drawing.Size(picDIY4.Width, picDIY4.Height));//目标位置
                    System.Drawing.Rectangle origRect = new System.Drawing.Rectangle(new System.Drawing.Point(0, 0), new System.Drawing.Size(picDIY4.Width, picDIY4.Height));//原图位置（默认从原图中截取的图片大小等于目标图片的大小）
                    graphics.DrawImage(img, destRect, origRect, System.Drawing.GraphicsUnit.Pixel);
                    picDIY4.Image = partImg;
                    picDIYTmp4.Image = partImg;
                }
                #endregion
            }
        }

        private void picDIY1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (e.Clicks > 1)
                {
                    picDIY1_MouseDoubleClick(sender, e);
                }
                else
                {
                    PictureBox Tmp = ((PictureBox)sender);
                    if (Tmp == null) return;
                    myPicTemp = Tmp;
                    ((PictureBox)sender).DoDragDrop(Tmp, DragDropEffects.Copy);

                }
            }
        }

        private void pnlDIY2_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(PictureBox)))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void pnlDIY2_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
        {
            if (myPicTemp != null && myPicTemp.Image!=null)
            {
                PictureBox temp = this.Controls.Find("picDIY" + (sender as System.Windows.Forms.Panel).Tag.ToString(), true)[0] as PictureBox;
                int PicHeight = myPicTemp.Image.Height;
                int PicWidth = myPicTemp.Image.Width;
                if (PicWidth % temp.Width != 0 || PicHeight % temp.Height != 0)
                {
                    MessageBox.Show(CsConst.mstrINIDefault.IniReadValue("Public", "99811", "") + temp.Width.ToString() + "*" + temp.Height.ToString());
                    return;
                }
                int numW = PicWidth / temp.Width;
                int numH = PicHeight / temp.Height;
                if (numW == 1)
                {
                    temp.Image = myPicTemp.Image;
                    string strtmp = temp.Name.ToString();
                    strtmp = strtmp.Replace("DIY", "DIYTmp");
                    PictureBox temp2 = this.Controls.Find(strtmp, true)[0] as PictureBox;
                    temp2.Image = myPicTemp.Image;
                }
                else
                {
                    System.Drawing.Bitmap partImg = new System.Drawing.Bitmap(temp.Width, temp.Height);
                    System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(partImg);
                    System.Drawing.Rectangle destRect = new System.Drawing.Rectangle(new System.Drawing.Point(0, 0), new System.Drawing.Size(temp.Width, temp.Height));//目标位置
                    System.Drawing.Rectangle origRect = new System.Drawing.Rectangle(new System.Drawing.Point(0, 0), new System.Drawing.Size(temp.Width, temp.Height));//原图位置（默认从原图中截取的图片大小等于目标图片的大小）
                    graphics.DrawImage(myPicTemp.Image, destRect, origRect, System.Drawing.GraphicsUnit.Pixel);
                    temp.Image = partImg;
                    string strtmp = temp.Name.ToString();
                    strtmp = strtmp.Replace("DIY", "DIYTmp");
                    PictureBox temp2 = this.Controls.Find(strtmp, true)[0] as PictureBox;
                    temp2.Image = myPicTemp.Image;
                }
                e.Effect = DragDropEffects.Copy;
            }
        }


        private void pag1OFF1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (e.Clicks > 1)
                {
                    temp_MouseDoubleClick(sender, e);
                }
                else
                {
                    PictureBox Tmp = ((PictureBox)sender);
                    if (Tmp == null) return;
                    myPicTemp = Tmp;
                    ((PictureBox)sender).DoDragDrop(Tmp, DragDropEffects.Copy);

                }
            }
        }

        private void pnl1OFF1_DragDrop(object sender, DragEventArgs e)
        {
            try
            {
                if (myPicTemp != null && myPicTemp.Image != null)
                {
                    string pnlName = (sender as System.Windows.Forms.Panel).Name.ToString();
                    int Tag = Convert.ToInt32((sender as System.Windows.Forms.Panel).Tag.ToString());
                    int Page = Convert.ToInt32((sender as System.Windows.Forms.Panel).Name.Substring(3, 1).ToString());
                    pnlName = pnlName.Replace("pnl", "pag");
                    PictureBox temp = this.Controls.Find(pnlName, true)[0] as PictureBox;
                    int PicHeight = myPicTemp.Image.Height;
                    int PicWidth = myPicTemp.Image.Width;
                    if (PicWidth % temp.Width != 0 || PicHeight % temp.Height != 0)
                    {
                        MessageBox.Show(CsConst.mstrINIDefault.IniReadValue("Public", "99811", "") + temp.Width.ToString() + "*" + temp.Height.ToString());
                        return;
                    }
                    int DIndex = Convert.ToInt32(dgvDevice[8, dgvDevice.CurrentRow.Index].Value.ToString());
                    int DeviceType = Convert.ToInt32(dgvDevice[7, dgvDevice.CurrentRow.Index].Value.ToString());
                    Panel pnl = null;
                    for (int i = 0; i < CsConst.myPanels.Count; i++)
                    {
                        if (CsConst.myPanels[i].DIndex == DIndex)
                        {
                            pnl = CsConst.myPanels[i];
                            break;
                        }
                    }
                    int numW = PicWidth / temp.Width;
                    int numH = PicHeight / temp.Height;
                    if (numW == 1)
                    {
                        #region
                        temp.Image = myPicTemp.Image;
                        string strTmp = pnlName;
                        if (strTmp.Contains("OFF"))
                        {
                            int intTmp = Convert.ToInt32(strTmp.Substring(7, 1));
                            #region
                            if (intTmp < 5)
                            {
                                if (Tag == 1)
                                {
                                    pnl.IconOFF[(pnl.IconOFF.Length - 1 - 32) + (Page - 1) * 8 + 1] = 1;
                                }
                                else if (Tag == 2)
                                {
                                    pnl.IconOFF[(pnl.IconOFF.Length - 1 - 32) + (Page - 1) * 8 + 3] = 1;
                                }
                                else if (Tag == 3)
                                {
                                    pnl.IconOFF[(pnl.IconOFF.Length - 1 - 32) + (Page - 1) * 8 + 5] = 1;
                                }
                                else if (Tag == 4)
                                {
                                    pnl.IconOFF[(pnl.IconOFF.Length - 1 - 32) + (Page - 1) * 8 + 7] = 1;
                                }
                            }
                            else
                            {
                                if (Tag == 1)
                                {
                                    pnl.IconOFF[(pnl.IconOFF.Length - 1 - 32) + (Page - 1) * 8 + 2] = 1;
                                }
                                else if (Tag == 2)
                                {
                                    pnl.IconOFF[(pnl.IconOFF.Length - 1 - 32) + (Page - 1) * 8 + 4] = 1;
                                }
                                else if (Tag == 3)
                                {
                                    pnl.IconOFF[(pnl.IconOFF.Length - 1 - 32) + (Page - 1) * 8 + 6] = 1;
                                }
                                else if (Tag == 4)
                                {
                                    pnl.IconOFF[(pnl.IconOFF.Length - 1 - 32) + (Page - 1) * 8 + 8] = 1;
                                }
                            }
                            #endregion
                        }
                        else
                        {
                            #region
                            int intTmp = Convert.ToInt32(strTmp.Substring(6, 1));
                            if (intTmp < 5)
                            {
                                if (Tag == 1)
                                {
                                    pnl.IconON[(pnl.IconON.Length - 1 - 32) + (Page - 1) * 8 + 1] = 1;
                                }
                                else if (Tag == 2)
                                {
                                    pnl.IconON[(pnl.IconON.Length - 1 - 32) + (Page - 1) * 8 + 3] = 1;
                                }
                                else if (Tag == 3)
                                {
                                    pnl.IconON[(pnl.IconON.Length - 1 - 32) + (Page - 1) * 8 + 5] = 1;
                                }
                                else if (Tag == 4)
                                {
                                    pnl.IconON[(pnl.IconON.Length - 1 - 32) + (Page - 1) * 8 + 7] = 1;
                                }
                            }
                            else
                            {
                                if (Tag == 1)
                                {
                                    pnl.IconON[(pnl.IconON.Length - 1 - 32) + (Page - 1) * 8 + 2] = 1;
                                }
                                else if (Tag == 2)
                                {
                                    pnl.IconON[(pnl.IconON.Length - 1 - 32) + (Page - 1) * 8 + 4] = 1;
                                }
                                else if (Tag == 3)
                                {
                                    pnl.IconON[(pnl.IconON.Length - 1 - 32) + (Page - 1) * 8 + 6] = 1;
                                }
                                else if (Tag == 4)
                                {
                                    pnl.IconON[(pnl.IconON.Length - 1 - 32) + (Page - 1) * 8 + 8] = 1;
                                }
                            }
                            #endregion
                        }
                        #endregion
                    }
                    else
                    {
                        #region
                        string strTmp = pnlName;
                        PictureBox temp1 = this.Controls.Find(strTmp, true)[0] as PictureBox;
                        System.Drawing.Bitmap partImg = new System.Drawing.Bitmap(temp1.Width, temp1.Height);
                        System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(partImg);
                        System.Drawing.Rectangle destRect = new System.Drawing.Rectangle(new System.Drawing.Point(0, 0), new System.Drawing.Size(temp1.Width, temp1.Height));//目标位置
                        System.Drawing.Rectangle origRect = new System.Drawing.Rectangle(new System.Drawing.Point(0, 0), new System.Drawing.Size(temp1.Width, temp1.Height));//原图位置（默认从原图中截取的图片大小等于目标图片的大小）
                        graphics.DrawImage(myPicTemp.Image, destRect, origRect, System.Drawing.GraphicsUnit.Pixel);
                        temp1.Image = partImg;
                        int intTmp = 0;
                        if (strTmp.Contains("OFF"))
                        {
                            intTmp = Convert.ToInt32(strTmp.Substring(7, 1));
                            if (intTmp < 5)
                            {
                                #region
                                strTmp = strTmp.Substring(0, 7) + (intTmp + 4).ToString();
                                PictureBox temp2 = this.Controls.Find(strTmp, true)[0] as PictureBox;
                                partImg = new System.Drawing.Bitmap(temp2.Width, temp2.Height);
                                graphics = System.Drawing.Graphics.FromImage(partImg);
                                destRect = new System.Drawing.Rectangle(new System.Drawing.Point(0, 0), new System.Drawing.Size(temp2.Width, temp2.Height));//目标位置
                                origRect = new System.Drawing.Rectangle(new System.Drawing.Point(temp2.Width, 0), new System.Drawing.Size(temp2.Width, temp2.Height));//原图位置（默认从原图中截取的图片大小等于目标图片的大小）
                                graphics.DrawImage(myPicTemp.Image, destRect, origRect, System.Drawing.GraphicsUnit.Pixel);
                                temp2.Image = partImg;
                                if (Tag == 1)
                                {
                                    pnl.IconOFF[(pnl.IconOFF.Length - 1 - 32) + (Page - 1) * 8 + 1] = 1;
                                    pnl.IconOFF[(pnl.IconOFF.Length - 1 - 32) + (Page - 1) * 8 + 2] = 1;
                                }
                                else if (Tag == 2)
                                {
                                    pnl.IconOFF[(pnl.IconOFF.Length - 1 - 32) + (Page - 1) * 8 + 3] = 1;
                                    pnl.IconOFF[(pnl.IconOFF.Length - 1 - 32) + (Page - 1) * 8 + 4] = 1;
                                }
                                else if (Tag == 3)
                                {
                                    pnl.IconOFF[(pnl.IconOFF.Length - 1 - 32) + (Page - 1) * 8 + 5] = 1;
                                    pnl.IconOFF[(pnl.IconOFF.Length - 1 - 32) + (Page - 1) * 8 + 6] = 1;
                                }
                                else if (Tag == 4)
                                {
                                    pnl.IconOFF[(pnl.IconOFF.Length - 1 - 32) + (Page - 1) * 8 + 7] = 1;
                                    pnl.IconOFF[(pnl.IconOFF.Length - 1 - 32) + (Page - 1) * 8 + 8] = 1;
                                }
                                #endregion
                            }
                        }
                        else
                        {
                            intTmp = Convert.ToInt32(strTmp.Substring(6, 1));
                            if (intTmp < 5)
                            {
                                #region
                                strTmp = strTmp.Substring(0, 6) + (intTmp + 4).ToString();
                                PictureBox temp2 = this.Controls.Find(strTmp, true)[0] as PictureBox;
                                partImg = new System.Drawing.Bitmap(temp2.Width, temp2.Height);
                                graphics = System.Drawing.Graphics.FromImage(partImg);
                                destRect = new System.Drawing.Rectangle(new System.Drawing.Point(0, 0), new System.Drawing.Size(temp2.Width, temp2.Height));//目标位置
                                origRect = new System.Drawing.Rectangle(new System.Drawing.Point(temp2.Width, 0), new System.Drawing.Size(temp2.Width, temp2.Height));//原图位置（默认从原图中截取的图片大小等于目标图片的大小）
                                graphics.DrawImage(myPicTemp.Image, destRect, origRect, System.Drawing.GraphicsUnit.Pixel);
                                temp2.Image = partImg;
                                if (Tag == 1)
                                {
                                    pnl.IconON[(pnl.IconON.Length - 1 - 32) + (Page - 1) * 8 + 1] = 1;
                                    pnl.IconON[(pnl.IconON.Length - 1 - 32) + (Page - 1) * 8 + 2] = 1;
                                }
                                else if (Tag == 2)
                                {
                                    pnl.IconON[(pnl.IconON.Length - 1 - 32) + (Page - 1) * 8 + 3] = 1;
                                    pnl.IconON[(pnl.IconON.Length - 1 - 32) + (Page - 1) * 8 + 4] = 1;
                                }
                                else if (Tag == 3)
                                {
                                    pnl.IconON[(pnl.IconON.Length - 1 - 32) + (Page - 1) * 8 + 5] = 1;
                                    pnl.IconON[(pnl.IconON.Length - 1 - 32) + (Page - 1) * 8 + 6] = 1;
                                }
                                else if (Tag == 4)
                                {
                                    pnl.IconON[(pnl.IconON.Length - 1 - 32) + (Page - 1) * 8 + 7] = 1;
                                    pnl.IconON[(pnl.IconON.Length - 1 - 32) + (Page - 1) * 8 + 8] = 1;
                                }
                                #endregion
                            }
                        }
                        #endregion
                    }
                    e.Effect = DragDropEffects.Copy;
                }
            }
            catch
            {
            }
        }

        private void pnl1OFF1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(PictureBox)))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                Bitmap obmp = new Bitmap(picDIY1.Width, picDIY1.Height);
                Graphics g = Graphics.FromImage(obmp);
                g.DrawRectangle(new System.Drawing.Pen(System.Drawing.Color.Black), 0, 0, picDIY1.Width, picDIY1.Height);
                for (int intI = 1; intI <= 4; intI++)
                {
                    if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        string strFileName = saveFileDialog1.FileName.ToString();
                        (this.Controls.Find("picDIY" + intI.ToString(), true)[0] as PictureBox).Image.Save(strFileName);
                    }
                }
            }
            catch
            { }
        }

        private void btnClear1_Click(object sender, EventArgs e)
        {
            try
            {
                int Tag = Convert.ToByte((sender as Button).Tag);
                int Type = 0;
                int Page = 1;
                int SelDeviceType = Convert.ToInt32(dgvDevice[7, dgvDevice.CurrentRow.Index].Value.ToString());
                string strImagic = "";
                int DIndex = Convert.ToInt32(dgvDevice[8, dgvDevice.CurrentRow.Index].Value.ToString());
                Panel pnl = null;
                for (int i = 0; i < CsConst.myPanels.Count; i++)
                {
                    if (CsConst.myPanels[i].DIndex == DIndex)
                    {
                        pnl = CsConst.myPanels[i];
                        break;
                    }
                }
                if (Tag == 1)
                {
                    Page = 1;
                    Type = 0;
                }
                else if (Tag == 2)
                {
                    Page = 1;
                    Type = 1;
                }
                else if (Tag == 3)
                {
                    Page = 2;
                    Type = 0;
                }
                else if (Tag == 4)
                {
                    Page = 2;
                    Type = 1;
                }
                else if (Tag == 5)
                {
                    Page = 3;
                    Type = 0;
                }
                else if (Tag == 6)
                {
                    Page = 3;
                    Type = 1;
                }
                else if (Tag == 7)
                {
                    Page = 4;
                    Type = 0;
                }
                else if (Tag == 8)
                {
                    Page = 4;
                    Type = 1;
                }
                if (CsConst.mintNewDLPFHSetupDeviceType.Contains(SelDeviceType))
                {
                    strImagic = Application.StartupPath + @"\Icon\DLP\PIC1.bmp";
                    if (CsConst.mintMPTLDeviceType.Contains(SelDeviceType))
                    {
                        strImagic = Application.StartupPath + @"\Icon\DLP\PIC3.bmp";
                    }
                }
                else
                {
                    strImagic = Application.StartupPath + @"\Icon\DLP\PIC2.bmp";
                }

                if (Type == 0)
                {
                    for (int i = 1; i <= 8; i++)
                    {
                        PictureBox temp = this.Controls.Find("pag" + Page.ToString() + "OFF" + i.ToString(), true)[0] as PictureBox;
                        temp.Image = Image.FromFile(strImagic);
                        temp.Image.Tag = Page;
                        pnl.IconOFF[(pnl.IconOFF.Length - 1 - 32) + (Page - 1) * 8 + i] = 0;
                    }
                }
                else if (Type == 1)
                {
                    for (int i = 1; i <= 8; i++)
                    {
                        PictureBox temp = this.Controls.Find("pag" + Page.ToString() + "ON" + i.ToString(), true)[0] as PictureBox;
                        temp.Image = Image.FromFile(strImagic);
                        temp.Image.Tag = Page;
                        pnl.IconON[(pnl.IconON.Length - 1 - 32) + (Page - 1) * 8 + i] = 0;
                    }
                }
            }
            catch
            {
            }
        }

        private void cboMenu_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        void DisplayIconsAccordinglyPages(int PageNo)
        {
            #region
            switch (PageNo)
            {
                case 0: // main menu
                    picIcon.Image = HDL_Buspro_Setup_Tool.Properties.Resources.Menu3;
                    break;
                case 1: // light menu
                    picIcon.Image = HDL_Buspro_Setup_Tool.Properties.Resources.Menu3;
                    break;
                case 2: // light pages 1
                case 3: // light pages 2              
                case 4: // light pages 3
                case 5: // light pages 4
                case 6: // light pages 5
                case 7: // light pages 6
                    picIcon.Image = HDL_Buspro_Setup_Tool.Properties.Resources.Menu3;
                    break;
                case 8:  //ac menu
                    picIcon.Image = HDL_Buspro_Setup_Tool.Properties.Resources.Menu3;
                    break;
                case 9:  // curtain
                    picIcon.Image = HDL_Buspro_Setup_Tool.Properties.Resources.Menu3;
                    break;
                case 10: // heat menu
                    picIcon.Image = HDL_Buspro_Setup_Tool.Properties.Resources.Menu3;
                    break;
                case 11: // music
                    picIcon.Image = HDL_Buspro_Setup_Tool.Properties.Resources.Menu3;
                    break;
                case 12:
                case 13:
                    picIcon.Image = HDL_Buspro_Setup_Tool.Properties.Resources.Menu3;
                    break;
                case 14:  // 窗帘
                case 15:
                case 16:
                case 17:
                case 18:
                    picIcon.Image = HDL_Buspro_Setup_Tool.Properties.Resources.Menu3;
                    break;
                case 19:
                    picIcon.Image = HDL_Buspro_Setup_Tool.Properties.Resources.Menu4;
                    break;
            }
            #endregion
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (cboMenu.SelectedIndex != cboMenu.Items.Count - 1)
            {
                cboMenu.SelectedIndex = cboMenu.SelectedIndex + 1;
            }
            else if (cboMenu.SelectedIndex == cboMenu.Items.Count - 1)
            {
                cboMenu.SelectedIndex = 0;
            }
        }

        private void btnPre_Click(object sender, EventArgs e)
        {
            if (cboMenu.SelectedIndex != 0)
            {
                cboMenu.SelectedIndex = cboMenu.SelectedIndex - 1;
            }
            else if (cboMenu.SelectedIndex == 0)
            {
                cboMenu.SelectedIndex = cboMenu.Items.Count - 1;
            }
        }

        private void PicMenu_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                string MyPath = HDLPF.OpenFileDialog("PNG Image|*.png", "Add Icons");
                if (MyPath == null || MyPath == "") return;
                Image img = Image.FromFile(MyPath);
                if (img.Width != ((PictureBox)sender).Width  || img.Height != ((PictureBox)sender).Height)
                {
                    MessageBox.Show(CsConst.mstrINIDefault.IniReadValue("Public", "99811", "") + ((PictureBox)sender).Width.ToString() + "*" + ((PictureBox)sender).Height.ToString());
                    return;
                }
                (sender as PictureBox).Image = img;
                if ((sender as PictureBox).Name.Contains("picTxt"))
                {
                    (sender as PictureBox).InitialImage = null;
                    PictureBox temp = this.Controls.Find("picTxtTmp" + (sender as PictureBox).Tag.ToString(), true)[0] as PictureBox;
                    temp.Image = img;
                }
            }
            catch
            {
            }
        }

        private void cbIcon_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (isRead) return;
                if (cbIcon.SelectedIndex == -1) return;
                if (dgvDevice.RowCount <= 0) return;
                int SelDeviceType = Convert.ToInt32(dgvDevice[7, dgvDevice.CurrentRow.Index].Value.ToString());
                if (EnviroDLPPanelDeviceTypeList.HDLEnviroDLPPanelDeviceTypeList.Contains(SelDeviceType))
                {
                    #region
                    picICon11.Visible = false;
                    DisplayIconsAccordinglyPages(cbIcon.SelectedIndex);
                    int Everyline = 3;
                    int Distance = 90;
                    if (cbIcon.SelectedIndex == 0)
                    {
                        #region
                        for (int i = 0; i < 11; i++)
                        {
                            PictureBox temp = this.Controls.Find("picICon" + i.ToString(), true)[0] as PictureBox;
                            int X = ((i) % Everyline) * Distance + 70;
                            int Y = (int)((i) / Everyline) * 120 + 55;
                            temp.Location = new Point(X, Y);
                            temp.Visible = true;
                        }
                        picICon7.Visible = false;
                        picICon8.Visible = false;
                        picICon9.Visible = false;

                        for (int i = 0; i < 11; i++)
                        {
                            PictureBox temp = this.Controls.Find("picTxt" + i.ToString(), true)[0] as PictureBox;
                            int X = ((i) % Everyline) * Distance + 53;
                            int Y = (int)((i) / Everyline) * 120 + 115;
                            temp.Location = new Point(X, Y);
                            temp.Visible = true;
                        }
                        picTxt7.Visible = false;
                        picTxt8.Visible = false;
                        picTxt9.Visible = false;
                        #endregion
                    }
                    else if (cbIcon.SelectedIndex == 1 || cbIcon.SelectedIndex == 9)
                    {
                        #region
                        Everyline = 2;
                        Distance = 140;
                        for (int i = 0; i < 8; i++)
                        {
                            PictureBox temp = this.Controls.Find("picICon" + i.ToString(), true)[0] as PictureBox;
                            int X = ((i) % Everyline) * Distance + 90;
                            int Y = (int)((i) / Everyline) * 100 + 40 + ((int)((i) / Everyline) * 5);
                            temp.Location = new Point(X, Y);
                            temp.Visible = true;
                        }
                        for (int i = 8; i < 11; i++)
                        {
                            PictureBox temp = this.Controls.Find("picICon" + i.ToString(), true)[0] as PictureBox;
                            temp.Visible = false;
                        }
                        #endregion
                    }
                    else if (cbIcon.SelectedIndex == 14 || cbIcon.SelectedIndex == 15 || cbIcon.SelectedIndex == 16 ||
                             cbIcon.SelectedIndex == 17 || cbIcon.SelectedIndex == 18 || cbIcon.SelectedIndex == 19)
                    {
                        #region
                        for (int i = 0; i < 12; i++)
                        {
                            PictureBox temp = this.Controls.Find("picICon" + i.ToString(), true)[0] as PictureBox;
                            temp.Visible = false;
                        }
                        #endregion
                    }
                    else
                    {
                        #region
                        Everyline = 3;
                        Distance = 90;
                        for (int i = 0; i < 11; i++)
                        {
                            PictureBox temp = this.Controls.Find("picICon" + i.ToString(), true)[0] as PictureBox;
                            int X = ((i) % Everyline) * Distance + 70;
                            int Y = (int)((i) / Everyline) * 100 + 40 + ((int)((i) / Everyline) * 6);
                            temp.Location = new Point(X, Y);
                            temp.Visible = true;
                        }

                        if (cbIcon.SelectedIndex == 2 || cbIcon.SelectedIndex == 3 || cbIcon.SelectedIndex == 4 ||
                            cbIcon.SelectedIndex == 5 || cbIcon.SelectedIndex == 6 || cbIcon.SelectedIndex == 7)
                        {
                            picICon10.Left = picICon7.Left;
                            picICon11.Left = picICon8.Left;
                            picICon11.Top = picICon10.Top;
                            picICon11.Visible = true;
                        }
                        else if (cbIcon.SelectedIndex == 12 || cbIcon.SelectedIndex == 13)
                        {
                            picICon10.Left = picICon7.Left;
                            picICon11.Left = picICon8.Left;
                            picICon11.Top = picICon10.Top;
                            picICon11.Visible = true;
                        }
                        else
                        {
                            picICon10.Visible = false;
                            picICon11.Left = picICon8.Left;
                            picICon11.Top = picICon10.Top;
                            picICon11.Visible = true;
                        }
                        #endregion
                    }
                    cbText_SelectedIndexChanged(cbIcon, null);
                    #endregion
                }
                else if (EnviroNewDeviceTypeList.EnviroNewPanelDeviceTypeList.Contains(SelDeviceType))
                {
                    #region
                    int Everyline = 3;
                    int Distance = 90;
                    picIcon.Image = HDL_Buspro_Setup_Tool.Properties.Resources.Menu3;
                    for (int i = 0; i < 12; i++)
                    {
                        PictureBox temp = this.Controls.Find("picICon" + i.ToString(), true)[0] as PictureBox;
                        int X = ((i) % Everyline) * Distance + 70;
                        int Y = (int)((i) / Everyline) * 110 + 50 + ((int)((i) / Everyline) * 10);
                        temp.Location = new Point(X, Y);
                        temp.Visible = true;
                    }
                    #endregion
                }
            }
            catch
            {
            }
        }

        private void cbText_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((sender as ComboBox).SelectedIndex == -1) return;
             int SelDeviceType = Convert.ToInt32(dgvDevice[7, dgvDevice.CurrentRow.Index].Value.ToString());
             if (EnviroDLPPanelDeviceTypeList.HDLEnviroDLPPanelDeviceTypeList.Contains(SelDeviceType))
             {
                 #region
                 picTxt11.Visible = false;
                 int Everyline = 3;
                 int Distance = 90;
                 if (cbIcon.SelectedIndex == 0)
                 {

                 }
                 else if (cbIcon.SelectedIndex == 1 || cbIcon.SelectedIndex == 9)
                 {
                     #region
                     Everyline = 2;
                     Distance = 138;
                     for (int i = 0; i < 8; i++)
                     {
                         PictureBox temp = this.Controls.Find("picTxt" + i.ToString(), true)[0] as PictureBox;
                         int X = ((i) % Everyline) * Distance + 75;
                         int Y = (Byte)((i) / Everyline) * 113 + 100 - ((Byte)((i) / Everyline) * 6);
                         temp.Location = new Point(X, Y);
                         temp.Visible = true;
                     }
                     for (int i = 8; i < 11; i++)
                     {
                         PictureBox temp = this.Controls.Find("picTxt" + i.ToString(), true)[0] as PictureBox;
                         temp.Visible = false;
                     }
                     #endregion
                 }
                 else if (cbIcon.SelectedIndex == 14 || cbIcon.SelectedIndex == 15 || cbIcon.SelectedIndex == 16 ||
                          cbIcon.SelectedIndex == 17 || cbIcon.SelectedIndex == 18)
                 {
                     #region
                     Distance = 138;
                     for (int i = 0; i < 4; i++)
                     {
                         PictureBox temp = this.Controls.Find("picTxt" + i.ToString(), true)[0] as PictureBox;
                         temp.Left = 145;
                         temp.Top = 33 + i * 90 - (i * 6) + 5;
                         temp.Visible = true;
                     }

                     for (int i = 4; i < 12; i++)
                     {
                         PictureBox temp = this.Controls.Find("picTxt" + i.ToString(), true)[0] as PictureBox;
                         temp.Visible = false;
                     }
                     #endregion
                 }
                 else if (cbIcon.SelectedIndex == 19)
                 {
                     #region
                     picTxt0.Top = 50;
                     picTxt0.Left = 65;
                     picTxt1.Top = picTxt0.Top;
                     picTxt1.Left = 225;
                     for (int i = 2; i < 12; i++)
                     {
                         PictureBox temp = this.Controls.Find("picTxt" + i.ToString(), true)[0] as PictureBox;
                         temp.Visible = false;
                     }
                     #endregion
                 }
                 else
                 {
                     #region
                     Everyline = 3;
                     Distance = 92;
                     for (int i = 0; i < 11; i++)
                     {
                         PictureBox temp = this.Controls.Find("picTxt" + i.ToString(), true)[0] as PictureBox;
                         int X = ((i) % Everyline) * Distance + 50;
                         int Y = (Byte)((i) / Everyline) * 110 + 105 - ((Byte)((i) / Everyline) * 3) - 5;
                         temp.Location = new Point(X, Y);
                         temp.Visible = true;
                     }

                     if (cbIcon.SelectedIndex == 2 || cbIcon.SelectedIndex == 3 || cbIcon.SelectedIndex == 4 ||
                         cbIcon.SelectedIndex == 5 || cbIcon.SelectedIndex == 6 || cbIcon.SelectedIndex == 7)
                     {
                         picTxt10.Left = picTxt7.Left;
                         picTxt11.Left = picTxt8.Left;
                         picTxt11.Top = picTxt10.Top;
                         picTxt11.Visible = true;
                     }
                     else if (cbIcon.SelectedIndex == 12 || cbIcon.SelectedIndex == 13)
                     {
                         picTxt10.Left = picTxt7.Left;
                         picTxt11.Left = picTxt8.Left;
                         picTxt11.Top = picTxt10.Top;
                         picTxt11.Visible = true;
                     }
                     else
                     {
                         picTxt10.Visible = false;
                         picTxt11.Left = picTxt8.Left;
                         picTxt11.Top = picTxt10.Top;
                         picTxt11.Visible = true;
                     }
                     #endregion
                 }
                 #endregion
             }
             else if (EnviroNewDeviceTypeList.EnviroNewPanelDeviceTypeList.Contains(SelDeviceType))
             {
                 #region
                 int Everyline = 3;
                 int Distance = 90;
                 picIcon.Image = HDL_Buspro_Setup_Tool.Properties.Resources.Menu3;
                 for (int i = 0; i < 12; i++)
                 {
                     PictureBox temp = this.Controls.Find("picTxt" + i.ToString(), true)[0] as PictureBox;
                     int X = ((i) % Everyline) * Distance + 50;
                     int Y = (int)((i) / Everyline) * 110 + 105 + ((int)((i) / Everyline) * 10);
                     temp.Location = new Point(X, Y);
                     temp.Visible = true;
                 }
                 #endregion
             }
        }

        private void btnClearColor1_Click(object sender, EventArgs e)
        {
            try
            {
                int DIndex = Convert.ToInt32(dgvDevice[8, dgvDevice.CurrentRow.Index].Value.ToString());
                int DeviceType = Convert.ToInt32(dgvDevice[7, dgvDevice.CurrentRow.Index].Value.ToString());
                if (EnviroDLPPanelDeviceTypeList.HDLEnviroDLPPanelDeviceTypeList.Contains(DeviceType))
                {
                    #region
                    if (dgvDevice.CurrentRow.Index >= 0)
                    {
                        
                        int Tag = Convert.ToInt32((sender as Button).Tag);
                        PictureBox temp1 = this.Controls.Find("picICon" + Tag.ToString(), true)[0] as PictureBox;
                        PictureBox temp2 = this.Controls.Find("picTxt" + Tag.ToString(), true)[0] as PictureBox;
                        temp1.Image = null;
                        temp2.Image = null;
                        temp1.InitialImage = null;
                        temp2.InitialImage = null;
                    }
                    #endregion
                }
                else if (EnviroNewDeviceTypeList.EnviroNewPanelDeviceTypeList.Contains(DeviceType))
                {
                    #region
                    if (dgvDevice.CurrentRow.Index >= 0)
                    {

                        int Tag = Convert.ToInt32((sender as Button).Tag);
                        PictureBox temp1 = this.Controls.Find("picICon" + Tag.ToString(), true)[0] as PictureBox;
                        PictureBox temp2 = this.Controls.Find("picTxt" + Tag.ToString(), true)[0] as PictureBox;
                        temp1.Image = null;
                        temp2.Image = null;
                        temp1.InitialImage = null;
                        temp2.InitialImage = null;
                    }
                    #endregion
                }
            }
            catch
            {
            }
        }

        private void txtColor0_TextChanged(object sender, EventArgs e)
        {
            ColorSelectedText = Convert.ToInt32((sender as TextBox).Tag);
            int Tag = Convert.ToInt32((sender as TextBox).Tag);
            if (myColorString == null) myColorString = new string[12];
            myColorString[ColorSelectedText] = (sender as TextBox).Text;
            WriteStringToIconsForColor();
        }

        private void txtColor0_Click(object sender, EventArgs e)
        {
            ColorSelectedText = Convert.ToInt32((sender as TextBox).Tag);
        }

        private void btnFontColor_Click(object sender, EventArgs e)
        {
            if (Fonts.ShowDialog() == DialogResult.OK)
            {
                MyColorFont[ColorSelectedText] = Fonts.Font;
                WriteStringToIconsForColor();
            }
        }

        private void WriteStringToIconsForColor()
        {
            try
            {
                PictureBox picTemp = this.Controls.Find("picTxt" + ColorSelectedText.ToString(), true)[0] as PictureBox;
                if (myColorString[ColorSelectedText] == null) myColorString[ColorSelectedText] = "";
                string str = myColorString[ColorSelectedText];
                Font font = MyColorFont[ColorSelectedText];
                Image bit = new Bitmap(80, 24); 
                if (picTemp.Image != null)
                {
                    PictureBox picTmp = this.Controls.Find("picTxtTmp" + ColorSelectedText.ToString(), true)[0] as PictureBox;
                    picTmp.DrawToBitmap((Bitmap)bit, new Rectangle(0, 0, 80, 24));
                }
                else
                {
                    picTemp.Image = new Bitmap(80, 24);
                    picTemp.InitialImage = new Bitmap(80, 24);
                    picTemp.InitialImage.Tag = 255;
                    PictureBox picTmp = this.Controls.Find("picTxtTmp" + ColorSelectedText.ToString(), true)[0] as PictureBox;
                    picTmp.Image = picTemp.Image;
                    picTmp.DrawToBitmap((Bitmap)bit, new Rectangle(0, 0, 80, 24));
                }
                Graphics g = Graphics.FromImage(bit);
                g.Clear(Color.Blue);
                PointF point = new PointF(6 + MyColorMoveLeft[ColorSelectedText], 6 + MyColorMoveTop[ColorSelectedText]);
                System.Drawing.StringFormat sf = new System.Drawing.StringFormat();
                sf.FormatFlags = StringFormatFlags.DisplayFormatControl;
                Brush brush = new SolidBrush(myColor.BackColor);
                g.DrawString(str.ToString(), font, brush, point, sf);
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                picTemp.Image = bit;
                
                Bitmap image = new Bitmap(80, 24);
                picTemp.DrawToBitmap(image, new Rectangle(0, 0, 80, 24));
                image.MakeTransparent(Color.Blue);
                unsafe
                {
                    for (int i = 0; i < image.Height; i++)
                    {
                        for (int j = 0; j < image.Width; j++)
                        {
                            // write the logic implementation here
                            Color CurrentColor = image.GetPixel(j, i);
                            if (CurrentColor.A == 0)
                            {

                            }
                            else
                            {
                                image.SetPixel(j, i, myColor.BackColor);
                            }
                        }
                    }
                }
                //image.Save(Application.StartupPath + @"\DIY\ColorDLP-Text\" + ColorSelectedText.ToString() + ".png", ImageFormat.Png);
                picTemp.Image = image;
                if (picTemp.InitialImage != null)
                {
                    if (str == "" && Convert.ToInt32(picTemp.InitialImage.Tag) == 255)
                    {
                        picTemp.Image = null;
                    }
                }
            }
            catch
            {

            }
        }

        /// <summary>

        /// 无论摄像头拍摄图像的大小，均以picturebox为标准进行缩小 、放大

        /// </summary>

        /// <param name="bmp"></param>

        /// <param name="pictureBox1"></param>

        /// <returns></returns>

        public Bitmap imgSuitablePicturebox(Bitmap bmp, System.Windows.Forms.PictureBox pictureBox1)
        {

            Bitmap tmpbmp = null;

            Rectangle oldrct, newrct;

            bmp = new Bitmap(pictureBox1.Image);

            oldrct = new Rectangle(0, 0, bmp.Width, bmp.Height);

            tmpbmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);

            oldrct = new Rectangle(0, 0, bmp.Width, bmp.Height);

            Graphics g = Graphics.FromImage(tmpbmp);

            newrct = new Rectangle(0, 0, tmpbmp.Width, tmpbmp.Height);

            g.DrawImage(bmp, newrct, oldrct, GraphicsUnit.Pixel);

            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

            g.Dispose();


            return tmpbmp;

        }


        /// <summary>   
        /// 无损压缩图片   
        /// </summary>   
        /// <param name="sFile">原图片</param>   
        /// <param name="dFile">压缩后保存位置</param>   
        /// <param name="dHeight">高度</param>   
        /// <param name="dWidth"></param>   
        /// <param name="flag">压缩质量(数字越小压缩率越高) 1-100</param>   
        /// <returns></returns>   
        public static bool GetPicThumbnail(string sFile, string dFile, int dHeight, int dWidth, int flag)
        {

            System.Drawing.Image iSource = System.Drawing.Image.FromFile(sFile);
            ImageFormat tFormat = iSource.RawFormat;
            int sW = 0, sH = 0;
            //按比例缩放   
            Size tem_size = new Size(iSource.Width, iSource.Height);

            if (tem_size.Width > dHeight || tem_size.Width > dWidth) //将**改成c#中的或者操作符号   
            {
                if ((tem_size.Width * dHeight) > (tem_size.Height * dWidth))
                {
                    sW = dWidth;
                    sH = (dWidth * tem_size.Height) / tem_size.Width;
                }
                else
                {
                    sH = dHeight;
                    sW = (tem_size.Width * dHeight) / tem_size.Height;
                }
            }
            else
            {
                sW = tem_size.Width;
                sH = tem_size.Height;
            }
            Bitmap ob = new Bitmap(dWidth, dHeight);
            Graphics g = Graphics.FromImage(ob);
            g.Clear(Color.WhiteSmoke);
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.DrawImage(iSource, new Rectangle((dWidth - sW) / 2, (dHeight - sH) / 2, sW, sH), 0, 0, iSource.Width, iSource.Height, GraphicsUnit.Pixel);
            g.Dispose();
            //以下代码为保存图片时，设置压缩质量   
            EncoderParameters ep = new EncoderParameters();
            long[] qy = new long[1];
            qy[0] = flag;//设置压缩的比例1-100   
            EncoderParameter eParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, qy);
            ep.Param[0] = eParam;
            try
            {
                ImageCodecInfo[] arrayICI = ImageCodecInfo.GetImageEncoders();
                ImageCodecInfo jpegICIinfo = null;
                for (int x = 0; x < arrayICI.Length; x++)
                {
                    if (arrayICI[x].FormatDescription.Equals("JPEG"))
                    {
                        jpegICIinfo = arrayICI[x];
                        break;
                    }
                }
                if (jpegICIinfo != null)
                {
                    ob.Save(dFile, jpegICIinfo, ep);//dFile是压缩后的新路径   
                }
                else
                {
                    ob.Save(dFile, tFormat);
                }
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                iSource.Dispose();
                ob.Dispose();
            }

        }  


        private static ImageCodecInfo GetEncoderInfo(String mimeType)
        {
            int j;
            ImageCodecInfo[] encoders;
            encoders = ImageCodecInfo.GetImageEncoders();
            for (j = 0; j < encoders.Length; ++j)
            {
                if (encoders[j].MimeType == mimeType)
                    return encoders[j];
            }
            return null;
        }


        private void btnClearColor_Click(object sender, EventArgs e)
        {
            myColorString = new string[12];
            for (int i = 0; i < 12; i++)
            {
                ColorSelectedText = i;
                TextBox temp = this.Controls.Find("txtColor" + ColorSelectedText.ToString(), true)[0] as TextBox;
                temp.Text = "";
                WriteStringToIconsForColor();
            }
        }

        private void btnUpColor_Click(object sender, EventArgs e)
        {
            MyColorMoveTop[ColorSelectedText] -= 1;
            WriteStringToIconsForColor();
        }

        private void btnLeftColor_Click(object sender, EventArgs e)
        {
            MyColorMoveLeft[ColorSelectedText] -= 1;
            WriteStringToIconsForColor();
        }

        private void btnDownColor_Click(object sender, EventArgs e)
        {
            MyColorMoveTop[ColorSelectedText] += 1;
            WriteStringToIconsForColor();
        }

        private void btnRightColor_Click(object sender, EventArgs e)
        {
            MyColorMoveLeft[ColorSelectedText] += 1;
            WriteStringToIconsForColor();
        }

        private void btnColor_Click(object sender, EventArgs e)
        {
            if (myColorDialog.ShowDialog() == DialogResult.OK)
            {
                myColor.BackColor = myColorDialog.Color;
            }
            WriteStringToIconsForColor();
        }

        private void picH1_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                string MyPath = HDLPF.OpenFileDialog("BMP Image|*.bmp|JPEG Image|*.jpg|PNG Image|*.png", "Add Icons");
                if (MyPath == null || MyPath == "") return;
                Image img = Image.FromFile(MyPath);
                if (img.Width != ((PictureBox)sender).Width || img.Height != ((PictureBox)sender).Height )
                {
                    MessageBox.Show(CsConst.mstrINIDefault.IniReadValue("Public", "99811", "") + ((PictureBox)sender).Width.ToString() + "*" + ((PictureBox)sender).Height.ToString());
                    return;
                }
                ((PictureBox)sender).Image = img;
            }
            catch
            {
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            for (int intI = 1; intI <= 4; intI++)
            {
                Image oImg = (this.Controls.Find("picDIY" + intI.ToString(), true)[0] as PictureBox).Image;
                (this.Controls.Find("picDIY" + intI.ToString(), true)[0] as PictureBox).Image = HDLSysPF.ConvertToInvert(oImg);

                oImg = (this.Controls.Find("picDIYTmp" + intI.ToString(), true)[0] as PictureBox).Image;
                (this.Controls.Find("picDIYTmp" + intI.ToString(), true)[0] as PictureBox).Image = HDLSysPF.ConvertToInvert(oImg);
            }
        }

        private void picDIYTmp1_Click(object sender, EventArgs e)
        {

        }

        private void btnPNG_Click(object sender, EventArgs e)
        {
            if (DialogResult.OK == openFileDialog1.ShowDialog())
            {
                string filePath = openFileDialog1.FileName;
                if (System.IO.File.Exists(filePath))
                {
                    SaveAsPng(filePath);
                }
            }
        }

        private void SaveAsPng(string filePath)
        {
            string newFilePath = filePath;
            try
            {
                Image bit = Image.FromFile(newFilePath);
                Graphics g = Graphics.FromImage(bit);
                g.Clear(Color.White);

                Bitmap image = (Bitmap)bit;
                image.MakeTransparent(Color.White);
                unsafe
                {
                    for (int i = 0; i < image.Height; i++)
                    {
                        for (int j = 0; j < image.Width; j++)
                        {
                            // write the logic implementation here
                            Color CurrentColor = image.GetPixel(j, i);
                            if (CurrentColor.A == 0)
                            {

                            }
                            else
                            {
                                
                            }
                        }
                    }
                }
                image.Save(newFilePath + ".png", ImageFormat.Png);
            }
            catch
            {

            }
        }

    }
}
