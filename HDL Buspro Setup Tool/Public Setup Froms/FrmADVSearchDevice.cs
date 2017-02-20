using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace HDL_Buspro_Setup_Tool
{
    public partial class FrmADVSearchDevice : Form
    {
        private delegate void FlushClient1(object[] tmp); //代理
        private delegate void FlushClient2(object[] tmp); //代理
        private byte SubnetID;
        private byte DeviceID1;
        private byte DeviceID2;
        private DataGridView DGV;
        private bool isLoad = false;
        private bool isAddDeviceToMain = false;
        private bool isAddToList = false;
        private bool isStopSearch = false;

        private static Byte currentSelectDeviceSubnetId = 0;
        private static Byte currentSelectDeviceDeviceId = 0;

        public FrmADVSearchDevice()
        {
            InitializeComponent();
        }

        public FrmADVSearchDevice(byte sub,byte dev1,byte dev2)
        {
            InitializeComponent();
            SubnetID = sub;
            DeviceID1 = dev1;
            DeviceID2 = dev2;
        }

        private void FrmADVSearchDevice_Load(object sender, EventArgs e)
        {
            try
            {
                setValue();
                Form form = null;
                foreach (Form frm in Application.OpenForms)
                {
                    if (frm.Name == "frmMain")
                    {
                        form = frm;
                        break;
                    }
                }
                if (form != null)
                {
                    DGV = form.Controls.Find("dgOnline", true)[0] as DataGridView;
                }
                
            }
            catch
            {
            }
        }

        private void setValue()
        {
            isLoad = true;
            IniFile iniFile = new IniFile(Application.StartupPath + @"\ini\Default.ini");
            txtASub.Text = iniFile.IniReadValue("Search", "ADV1", "");
            txtADev1.Text = iniFile.IniReadValue("Search", "ADV2", "");
            txtADev2.Text = iniFile.IniReadValue("Search", "ADV3", "");
            txtMSub.Text = iniFile.IniReadValue("Search", "ADV4", "");
            txtMDev.Text = iniFile.IniReadValue("Search", "ADV5", "");
            isLoad = false;
        }

        void calculationWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.Default;
                dgvResult.Enabled = true;
                btnStop_Click(null, null);
            }
            catch
            {
            }
        }

        void calculationWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            try
            {
                this.Process.Value = e.ProgressPercentage;
            }
            catch
            {
            }
        }

        void calculationWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                UDPReceive.receiveQueue.Clear();
                dgvResult.Rows.Clear();
                if (CsConst.myOnlines == null) CsConst.myOnlines = new List<DevOnLine>();
                //CsConst.mySends.AddBufToSndList(null, 0x000E, 255, 255, false, false, false, false);
                lbCurrentSubValue.Text = txtASub.Text;
                if (txtASub.Text == "255" || (txtADev1.Text == "255" && txtADev2.Text == "255"))
                {
                    Process.Visible = false;
                    lbCurrentDevValue.Text = "255";
                    CsConst.FastSearch = true;
                    Byte RepeatSendTimes = 1;

                    Random R1 = new Random();
                    Random R2 = new Random();
                    List<Byte> SearchDeviceAddressesList = new List<byte>();
                    R1.Next();
                    Byte[] RandomWhenSearch = new Byte[2];
                    R1.NextBytes(RandomWhenSearch);

                    Byte[] DataSendBuffer = new Byte[2 + SearchDeviceAddressesList.Count];
                    RandomWhenSearch.CopyTo(DataSendBuffer, 0);
                RepeatSearch:
                    if (SearchDeviceAddressesList != null && SearchDeviceAddressesList.Count > 0)
                    {
                        DataSendBuffer = new Byte[2 + SearchDeviceAddressesList.Count];
                        RandomWhenSearch.CopyTo(DataSendBuffer, 0);
                        SearchDeviceAddressesList.CopyTo(DataSendBuffer, 2);
                        SearchDeviceAddressesList = new List<byte>();
                    }
                    CsConst.mySends.AddBufToSndList(DataSendBuffer, 0x000E, 255, 255, false, false, false, false);
                    // 广播搜索方式汇集
                    #region
                    DateTime d1, d2;
                    d1 = DateTime.Now;
                    d2 = DateTime.Now;
                    while (UDPReceive.receiveQueue.Count > 0 || HDLSysPF.Compare(d2, d1) < 2000)
                    {
                        if (isStopSearch) break;
                        d2 = DateTime.Now;
                        if (UDPReceive.receiveQueue.Count > 0)
                        {
                            byte[] readData = UDPReceive.receiveQueue.Dequeue();
                            
                            if (readData[21] == 0x00 && readData[22] == 0x0F && readData.Length >= 45)
                            {
                                int index = 1;
                                while (CsConst.AddedDevicesIndexGroup.Contains(index))
                                {
                                    index++;
                                }

                                DevOnLine temp = new DevOnLine();
                                byte[] arayRemark = new byte[20];

                                HDLSysPF.CopyRemarkBufferFrmMyRevBuffer(readData, arayRemark, 25);
                                string strRemark = HDLPF.Byte2String(arayRemark);
                                temp.bytSub = readData[17];
                                temp.bytDev = readData[18];
                                temp.DevName = temp.bytSub.ToString() + "-" + temp.bytDev.ToString() + "\\" + strRemark.ToString();
                                temp.DeviceType = readData[19] * 256 + readData[20];
                                temp.strVersion = "Unread";
                                temp.intDIndex = index;

                                String[] strTmp = DeviceTypeList.GetDisplayInformationFromPublicModeGroup(temp.DeviceType);
                                String deviceDescription = strTmp[1];
                                String strModel = strTmp[0];

                                if (temp.strVersion == null) temp.strVersion = "";
                                string strVersion = temp.strVersion;
                                if (strVersion == "") strVersion = "Unread";

                                object[] obj1 = new object[] { true, temp.bytSub, temp.bytDev, temp.DevName.Split('\\')[1].ToString(), 
                                   deviceDescription,strModel,temp.DeviceType};
                                object[] obj2 = { false, HDL_Buspro_Setup_Tool.Properties.Resources.OK,  DGV.RowCount+1, temp.bytSub, temp.bytDev, strModel,temp.DevName.Split('\\')[1].ToString(), 
                                   deviceDescription,strVersion, temp.intDIndex.ToString(),"",temp.DeviceType};
                                Boolean isAddDeviceToMain = true;
                                // 是不是增加到在线设备列表和主窗体
                                #region
                                if (CsConst.myOnlines.Count > 0)
                                {
                                    foreach (DevOnLine tmp in CsConst.myOnlines)
                                    {
                                        if (temp.DevName == tmp.DevName && temp.bytSub == tmp.bytSub && temp.bytDev == tmp.bytDev && temp.DeviceType == tmp.DeviceType)
                                        {
                                            isAddDeviceToMain = false;
                                            break;
                                        }
                                    }
                                }

                                if (isAddDeviceToMain)
                                {
                                    CsConst.myOnlines.Add(temp);
                                    HDLSysPF.AddItsDefaultSettings(temp.DeviceType, temp.intDIndex, temp.DevName);
                                    CsConst.AddedDevicesIndexGroup.Add(index);
                                    if (this.DGV.InvokeRequired)//等待异步
                                    {
                                        FlushClient2 fc = new FlushClient2(ThreadFunction2);
                                        this.DGV.Invoke(fc, new object[] { obj2});
                                    }
                                    else
                                    {
                                        DGV.Rows.Add(obj2);
                                    }
                                }
                                #endregion
                                //添加到当前窗体
                                #region
                                isAddToList = true;
                                if (dgvResult.Rows.Count > 0)
                                {
                                    for (int i = 0; i < dgvResult.Rows.Count; i++)
                                    {
                                        if (temp.DevName == dgvResult[1, i].Value.ToString() + "-" + dgvResult[2, i].Value.ToString() +
                                            "\\" + dgvResult[3, i].Value.ToString())
                                        {
                                            isAddToList = false;
                                            break;
                                        }
                                    }
                                    if (isAddToList)
                                    {
                                        if (this.dgvResult.InvokeRequired)//等待异步
                                        {
                                            FlushClient1 fc = new FlushClient1(ThreadFunction1);
                                            this.dgvResult.Invoke(fc, new object[] { obj1 });
                                        }
                                        else
                                        {
                                            dgvResult.Rows.Add(obj1);
                                        }
                                    }
                                }
                                else
                                {
                                    if (this.dgvResult.InvokeRequired)//等待异步
                                    {
                                        FlushClient1 fc = new FlushClient1(ThreadFunction1);
                                        this.dgvResult.Invoke(fc, new object[] { obj1 });
                                    }
                                    else
                                    {
                                        dgvResult.Rows.Add(obj1);
                                    }
                                }
                                if (isAddToList)
                                {
                                    SearchDeviceAddressesList.Add(temp.bytSub);
                                    SearchDeviceAddressesList.Add(temp.bytDev);
                                }                                
                                #endregion
                            }
                        }
                    }
                    #endregion

                    RepeatSendTimes++;
                    if (RepeatSendTimes <= 4) goto RepeatSearch;
                }
                else
                {
                    Process.Visible = true;
                    SubnetID = Convert.ToByte(txtASub.Text);
                    DeviceID1 = Convert.ToByte(txtADev1.Text);
                    DeviceID2 = Convert.ToByte(txtADev2.Text);
                    int intTmp = 0;
                    for (byte byt = DeviceID1; byt <= DeviceID2; byt++)
                    {
                        if (isStopSearch) break;
                        byte[] ArayTmp = new byte[0];
                        byte bytSubID = Convert.ToByte(SubnetID);
                        byte bytDevID = Convert.ToByte(byt);
                        if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x000E, bytSubID, bytDevID, false, false, true, false) == true)
                        {
                            int index = 1;
                            if (CsConst.myOnlines.Count > 0)
                            {
                                for (int i = 0; i < CsConst.myOnlines.Count; i++)
                                {
                                    int intTmp1 = CsConst.myOnlines[i].intDIndex;
                                    if (intTmp1 > index) index = intTmp1;
                                }
                            }
                            byte[] arayRemark = new byte[20];
                            for (int intI = 0; intI < 20; intI++) { arayRemark[intI] = CsConst.myRevBuf[25 + intI]; }
                            string strRemark = HDLPF.Byte2String(arayRemark);
                            DevOnLine temp = new DevOnLine();
                            temp.bytSub = CsConst.myRevBuf[17];
                            temp.bytDev = CsConst.myRevBuf[18];
                            temp.DevName = temp.bytSub.ToString() + "-" + temp.bytDev.ToString() + "\\" + strRemark.ToString();
                            temp.DeviceType = CsConst.myRevBuf[19] * 256 + CsConst.myRevBuf[20];
                            temp.strVersion = "Unread";

                            String[] strArayTmp = DeviceTypeList.GetDisplayInformationFromPublicModeGroup(temp.DeviceType);
                            String deviceDescription = strArayTmp[1];
                            String strModel = strArayTmp[0];

                            string strTmp = string.Format("{0,-3}{1,-1}{2,-3}{3,-1}{4,-20}", temp.bytSub.ToString(), "-", temp.bytDev.ToString(), "-",
                                            strModel);
                            if (temp.strVersion == null) temp.strVersion = "";
                            string strVersion = temp.strVersion;
                            if (strVersion == "") strVersion = "Unread";
                            isAddDeviceToMain = true;
                            if (CsConst.myOnlines.Count > 0)
                            {
                                foreach (DevOnLine tmp in CsConst.myOnlines)
                                {
                                    if (temp.DevName == tmp.DevName && temp.bytSub == tmp.bytSub &&
                                        temp.bytDev == tmp.bytDev && temp.DeviceType == tmp.DeviceType)
                                    {
                                        isAddDeviceToMain = false;
                                        break;
                                    }
                                }
                                if (isAddDeviceToMain)
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
                            object[] obj1 = new object[] { true, temp.bytSub, temp.bytDev, temp.DevName.Split('\\')[1].ToString(), 
                                   deviceDescription,strModel};
                            object[] obj2 = new object[] {   false,  DGV.RowCount+1, temp.DevName.Split('\\')[1].ToString(), 
                                   deviceDescription,strVersion, temp.bytSub, temp.bytDev, strModel,
                                   index,""};
                            if (DGV.InvokeRequired)//等待异步
                            {
                                FlushClient2 fc = new FlushClient2(ThreadFunction2);
                                this.DGV.Invoke(fc, new object[] { obj2 });
                            }
                            else
                            {
                                if (isAddDeviceToMain)DGV.Rows.Add(obj2);
                            }
                            isAddToList = true;
                            if (dgvResult.Rows.Count > 0)
                            {
                                for (int i = 0; i < dgvResult.Rows.Count; i++)
                                {
                                    if (temp.DevName == dgvResult[1, i].Value.ToString() + "-" + dgvResult[2, i].Value.ToString() +
                                        "\\" + dgvResult[3, i].Value.ToString())
                                    {
                                        isAddToList = false;
                                        break;
                                    }
                                }
                                if (isAddToList)
                                {
                                    if (this.dgvResult.InvokeRequired)//等待异步
                                    {
                                        FlushClient1 fc = new FlushClient1(ThreadFunction1);
                                        this.dgvResult.Invoke(fc, new object[] { obj1 });
                                    }
                                    else
                                    {
                                        dgvResult.Rows.Add(obj1);
                                    }
                                }
                            }
                            else
                            {
                                if (this.dgvResult.InvokeRequired)//等待异步
                                {
                                    FlushClient1 fc = new FlushClient1(ThreadFunction1);
                                    this.dgvResult.Invoke(fc, new object[] { obj1 });
                                }
                                else
                                {
                                    dgvResult.Rows.Add(obj1);
                                }
                            }
                            //CsConst.myRevBuf = new byte[1200];
                            //System.Threading.Thread.Sleep(10);
                        }

                        if (byt + 1 <= DeviceID2)
                            lbCurrentDevValue.Text = (byt + 1).ToString();

                        if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy)
                            if (intTmp * 100 / (DeviceID2 - DeviceID1) <= 100)
                                CsConst.calculationWorker.ReportProgress(intTmp * 100 / (DeviceID2 - DeviceID1));
                        intTmp++;
                    }
                }
            }
            catch
            {
            }
        }

        private void ThreadFunction1(object[] obj)
        {
            if (isAddToList) dgvResult.Rows.Add(obj);
        }

        private void ThreadFunction2(object[] obj)
        {
            if (isAddDeviceToMain) DGV.Rows.Add(obj);
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            Process.Value = 0;
            isStopSearch = true;
            CsConst.FastSearch = false;
        }

        private void txtASub_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar) || e.KeyChar == 8)
                e.Handled = false;
            else
                e.Handled = true;
        }

        private void btnADV_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                isStopSearch = false;
                dgvResult.Enabled = false;
                CsConst.calculationWorker = new BackgroundWorker();
                CsConst.calculationWorker.DoWork += new DoWorkEventHandler(calculationWorker_DoWork);
                CsConst.calculationWorker.ProgressChanged += new ProgressChangedEventHandler(calculationWorker_ProgressChanged);
                CsConst.calculationWorker.WorkerReportsProgress = true;
                CsConst.calculationWorker.WorkerSupportsCancellation = true;
                CsConst.calculationWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(calculationWorker_RunWorkerCompleted);
                CsConst.calculationWorker.RunWorkerAsync();
            }
            catch
            {
            }
            Cursor.Current = Cursors.Default;
        }

        private void btnManual_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                byte[] ArayTmp = new byte[0];
                byte bytSubID = Convert.ToByte(txtMSub.Text);
                byte bytDevID = Convert.ToByte(txtMDev.Text);
                
                if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x000E, bytSubID, bytDevID, false, true, true, false) == true)
                {
                    int index = 1;
                    if (CsConst.myOnlines != null && CsConst.myOnlines.Count > 0)
                    {
                        for (int i = 0; i < CsConst.myOnlines.Count; i++)
                        {
                            int intTmp1 = CsConst.myOnlines[i].intDIndex;
                            if (intTmp1 > index) index = intTmp1;
                        }
                    }
                    byte[] arayRemark = new byte[20];
                    Array.Copy(CsConst.myRevBuf, 25, arayRemark, 0, 20);
                    string strRemark = HDLPF.Byte2String(arayRemark);
                    DevOnLine temp = new DevOnLine();
                    temp.bytSub = CsConst.myRevBuf[17];
                    temp.bytDev = CsConst.myRevBuf[18];
                    temp.DevName = temp.bytSub.ToString() + "-" + temp.bytDev.ToString() + "\\" + strRemark.ToString();
                    temp.DeviceType = CsConst.myRevBuf[19] * 256 + CsConst.myRevBuf[20];

                    Image oimg1 = HDL_Buspro_Setup_Tool.Properties.Resources.OK;

                    String[] strTmp = DeviceTypeList.GetDisplayInformationFromPublicModeGroup(temp.DeviceType);
                    String deviceModel = strTmp[0];
                    String deviceDescription = strTmp[1];

                    if (temp.strVersion == null) temp.strVersion = "";
                    string strVersion = temp.strVersion;
                    if (strVersion == "") strVersion = "Unread";

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
                            object[] obj = new object[] {   false, oimg1, DGV.RowCount+1,temp.bytSub, temp.bytDev, deviceModel, temp.DevName.Split('\\')[1].ToString(), 
                                   deviceDescription, strVersion,  index,""};
                            DGV.Rows.Add(obj);
                        }
                        else
                        {
                            bool isAddRow = true;
                            for (int i = 0; i < DGV.Rows.Count; i++)
                            {
                                if (DGV[9, i].Value.ToString() == index.ToString())
                                {
                                    isAddRow = false;
                                    break;
                                }
                            }
                            object[] obj;
                            if (isAddRow)
                            {
                                obj = new object[] {   false, oimg1, DGV.RowCount+1, temp.bytSub, temp.bytDev, deviceModel,temp.DevName.Split('\\')[1].ToString(), 
                                   deviceDescription, strVersion, index,""};
                                DGV.Rows.Add(obj);
                            }
                            obj = new object[] {true, temp.bytSub, temp.bytDev, temp.DevName.Split('\\')[1].ToString(), 
                                   deviceModel,deviceDescription};
                            dgvResult.Rows.Add(obj);

                        }
                    }
                    else
                    {
                        temp.intDIndex = 1;
                        if (CsConst.myOnlines == null) CsConst.myOnlines = new List<DevOnLine>();
                        CsConst.myOnlines.Add(temp);
                        HDLSysPF.AddItsDefaultSettings(temp.DeviceType, temp.intDIndex, temp.DevName);
                        object[] obj = new object[]{   false, oimg1, DGV.RowCount+1, temp.bytSub, temp.bytDev, deviceModel, temp.DevName.Split('\\')[1].ToString(), 
                                   deviceDescription,strVersion, index,""};
                        DGV.Rows.Add(obj);
                        obj = new object[] { true, temp.bytSub, temp.bytDev, temp.DevName.Split('\\')[1].ToString(), 
                                  deviceDescription,deviceModel};
                        dgvResult.Rows.Add(obj);
                    }
                    CsConst.myRevBuf = new byte[1200];
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            Cursor.Current = Cursors.Default;
        }

        private void txtMDev_TextChanged(object sender, EventArgs e)
        {
            if (isLoad) return;
            IniFile iniFile = new IniFile(Application.StartupPath + @"\ini\Default.ini");
            iniFile.IniWriteValue("Search", "ADV5", txtMDev.Text.ToString());
        }

        private void txtMSub_TextChanged(object sender, EventArgs e)
        {
            if (isLoad) return;
            IniFile iniFile = new IniFile(Application.StartupPath + @"\ini\Default.ini");
            iniFile.IniWriteValue("Search", "ADV4", txtMSub.Text.ToString());
        }

        private void txtADev2_TextChanged(object sender, EventArgs e)
        {
            if (isLoad) return;
            IniFile iniFile = new IniFile(Application.StartupPath + @"\ini\Default.ini");
            iniFile.IniWriteValue("Search", "ADV3", txtADev2.Text.ToString());
        }

        private void txtADev1_TextChanged(object sender, EventArgs e)
        {
            if (isLoad) return;
            IniFile iniFile = new IniFile(Application.StartupPath + @"\ini\Default.ini");
            iniFile.IniWriteValue("Search", "ADV2", txtADev1.Text.ToString());
        }

        private void txtASub_TextChanged(object sender, EventArgs e)
        {
            if (isLoad) return;
            IniFile iniFile = new IniFile(Application.StartupPath + @"\ini\Default.ini");
            iniFile.IniWriteValue("Search", "ADV1", txtASub.Text.ToString());
        }

        private void txtASub_Leave(object sender, EventArgs e)
        {

        }

        private void txtADev1_Leave(object sender, EventArgs e)
        {

        }

        private void txtADev2_Leave(object sender, EventArgs e)
        {

        }

        private void txtMSub_Leave(object sender, EventArgs e)
        {

        }

        private void txtMDev_Leave(object sender, EventArgs e)
        {

        }

        private void FrmADVSearchDevice_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy)
            {
                CsConst.calculationWorker.ReportProgress(100);
            }
            this.Dispose();
        }

        private void dgvResult_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                if ((sender as DataGridView).SelectedCells == null) return;
                if (e.RowIndex < 0) return;
                Cursor.Current = Cursors.WaitCursor;
                int rowIndex = dgvResult.SelectedRows[0].Index;
                if (e.ColumnIndex == 1 || e.ColumnIndex == 2)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    currentSelectDeviceSubnetId = Byte.Parse(dgvResult.SelectedCells[1].Value.ToString());
                    currentSelectDeviceDeviceId = Byte.Parse(dgvResult.SelectedCells[2].Value.ToString());
                    int wdDeviceType =Int32.Parse(dgvResult.SelectedCells[6].Value.ToString());
                    CsConst.MyTmpName = new List<string>();
                    frmModifAddress frmNew = new frmModifAddress(currentSelectDeviceSubnetId, currentSelectDeviceDeviceId, wdDeviceType);
                    if (frmNew.ShowDialog() == DialogResult.OK)
                    {
                        dgvResult.SelectedCells[1].Value = CsConst.MyTmpName[0];
                        dgvResult.SelectedCells[2].Value = CsConst.MyTmpName[1];
                    }
                }
            }
            catch
            { }
        }
    }
}
