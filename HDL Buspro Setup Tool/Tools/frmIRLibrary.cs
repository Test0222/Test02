using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HDL_Buspro_Setup_Tool
{
    public partial class frmIRLibrary : Form
    {
        private int MyRemID = -1;
        private int MyintBrand = -1;
        private int MyCodeLine = -1; // 红外码在第几行
        private static string MyCodes = string.Empty;
        private Form MyCtrl; // 父类窗体设备名称
        private object MyObject; // 哪个设备的结构体
        private byte bytSubID;
        private byte bytDevID;
        private bool isTestOk = false;
        private int MyDeviceType = 0;
        private int CurrentSelectDevice = 0;
        public frmIRLibrary()
        {
            InitializeComponent();
        }

        public frmIRLibrary(Form strDevName,object TmpObject,int devicetype)
        {
            InitializeComponent();
            MyCtrl = strDevName;
            this.MyObject = TmpObject;
            this.MyDeviceType = devicetype;
            if (MyObject is MMC)   //是不是多媒体播放器类型
            {
                string strMain = ((MMC)this.MyObject).strName.Split('\\')[0].Trim();
                bytSubID = byte.Parse(strMain.Split('-')[0].ToString());
                bytDevID = byte.Parse(strMain.Split('-')[1].ToString());
                cbChannel.Items.Clear();
                for (int i = 0; i < 8; i++) cbChannel.Items.Add((i + 1).ToString());
            }
            else if (MyObject is NewIR)//是不是新型红外发射
            {
                string strMain = ((NewIR)this.MyObject).strName.Split('\\')[0].Trim();
                bytSubID = byte.Parse(strMain.Split('-')[0].ToString());
                bytDevID = byte.Parse(strMain.Split('-')[1].ToString());
                int wdMaxValue = int.Parse(CsConst.mstrINIDefault.IniReadValue("DeviceType" + MyDeviceType.ToString(), "MaxValue", "0"));
                cbChannel.Items.Clear();
                for (int i = 1; i <= wdMaxValue; i++)
                    cbChannel.Items.Add(i.ToString());
            }
        }

        private void frmIRLibrary_Load(object sender, EventArgs e)
        {
            cbChannel.SelectedIndex = 0;
            cbPower.Items.Clear();
            cbPower.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00038", ""));
            cbPower.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00037", ""));
            cbPower.SelectedIndex = 1;
            cbMode.Items.Clear();
            for (int i = 0; i < 5; i++)
            {
                cbMode.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "0005" + i.ToString(), ""));
            }
            cbMode.SelectedIndex = 0;
            cbFan.Items.Clear();
            for (int i = 0; i < 4; i++)
            {
                cbFan.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "0006" + i.ToString(), ""));
            }
            cbFan.SelectedIndex = 0;
            cbWind.Items.Clear();
            cbWind.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00038", ""));
            cbWind.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "00037", ""));
            cbWind.SelectedIndex = 0;
            sbTemp_ValueChanged(null, null);
        }

        private int iOld = -1;
        private void listView1_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            try
            {
                if ((sender as ListView).SelectedIndices.Count > 0) //若有选中项 
                {
                    if (iOld == -1)
                    {
                        (sender as ListView).Items[e.ItemIndex].BackColor = Color.FromArgb(255, 0, 0); //设置选中项的背景颜色 
                        iOld = e.ItemIndex; //设置当前选中项索引 
                    }
                    else
                    {
                        if ((sender as ListView).SelectedIndices[0] != iOld)
                        {
                            (sender as ListView).Items[e.ItemIndex].BackColor = Color.FromArgb(255, 0, 0); //设置选中项的背景颜色 
                            (sender as ListView).Items[iOld].BackColor = Color.FromArgb(255, 255, 255); //恢复默认背景色 
                            iOld = e.ItemIndex; //设置当前选中项索引 
                        }
                    }
                }
                else //若无选中项 
                {
                    (sender as ListView).Items[iOld].BackColor = Color.FromArgb(255, 255, 255); //恢复默认背景色 
                    iOld = -1; //设置当前处于无选中项状态 
                }
            }
            catch
            {
            }
        } 

        private void frmIRLibrary_Shown(object sender, EventArgs e)
        {
            CsConst.MyLibrary = new UVCMD.DeviceAllIRInfo[6];
            CsConst.MyLibrary = HDLPF.selectIRIndex();
        }

        private void label2_Click(object sender, EventArgs e)
        {
            try
            {
                panel4.Visible = false;
                lvModel.Items.Clear();
                if (CsConst.MyLibrary == null || CsConst.MyLibrary.Length == 0) return;
                byte bytTag = Convert.ToByte(((Label)sender).Tag.ToString());
                CurrentSelectDevice = bytTag;
                if (bytTag == 5) panel9.Visible = true;
                else panel9.Visible = false;
                if (CsConst.MyLibrary[bytTag] == null || CsConst.MyLibrary[bytTag].brand.Count == 0) return;
                cbKey.Items.Clear();
                cbKey.Items.AddRange(CsConst.mstrINIDefault.IniReadValuesInALlSectionStr("NewIRDevice" + bytTag.ToString()));
                cbKey.SelectedIndex = 0;
                for (int i = 0; i < 25; i++)
                {
                    Button temp = this.Controls.Find("btnKey" + (i + 1).ToString(), true)[0] as Button;
                    Visible = false;
                }
                for (int i = 0; i < cbKey.Items.Count; i++)
                {
                    Button temp = this.Controls.Find("btnKey" + (i + 1).ToString(), true)[0] as Button;
                    Visible = true;
                    Text = cbKey.Items[i].ToString();
                }
                label2.BackColor = Color.Empty;
                label3.BackColor = Color.Empty;
                label4.BackColor = Color.Empty;
                label5.BackColor = Color.Empty;
                label6.BackColor = Color.Empty;
                label7.BackColor = Color.Empty;
                ((Label)sender).BackColor = Color.Red;
                MyRemID = bytTag;

                lbS1.BackColor = Color.Empty;
                lbS2.BackColor = Color.Empty;
                lbS3.BackColor = Color.Empty;
                lbS4.BackColor = Color.Empty;

                if (bytTag > 5) return;
                lvBrands.Items.Clear();
                DisplayAllBrandsToListview(bytTag);
                tsbName.Text = ((Label)sender).Text;
            }
            catch
            {
            }
        }

        void DisplayAllBrandsToListview(byte bytTag)
        {
            try
            {
                for (int i = 65; i <= 90; i++)
                {
                    string strTmp = Convert.ToChar(i).ToString();
                    ListViewGroup tmp = new ListViewGroup();
                    tmp.Name = strTmp;
                    tmp.Header = strTmp;

                    lvBrands.Groups.Add(tmp);
                }

                foreach (UVCMD.brandInfo Tmp in CsConst.MyLibrary[bytTag].brand)
                {
                    string strBrand = Tmp.brandRemark.Trim().ToUpper();
                    if (strBrand != "")
                    {
                        string strHead = HDLPF.GetPYString(strBrand);
                        int intIndex = Convert.ToChar(strHead) - 65;
                        if (strHead != "*" && intIndex >= 0 && intIndex < 26)
                        {
                            ListViewItem oItem = new ListViewItem(strBrand, lvBrands.Groups[intIndex]);
                            oItem.Tag = Tmp.brandID;
                            lvBrands.Items.Add(oItem);
                        }
                    }
                }
            }
            catch
            {
            }
        }

        void DisplayAllModelsToListview(int bytTag)
        {
            try
            {
                lvModel.Items.Clear();
                for (int i = 65; i <= 90; i++)
                {
                    string strTmp = Convert.ToChar(i).ToString();
                    ListViewGroup tmp = new ListViewGroup();
                    tmp.Name = strTmp;
                    tmp.Header = strTmp;

                    lvModel.Groups.Add(tmp);
                }

                for (int i = 0; i < CsConst.MyLibrary[MyRemID].brand[MyintBrand].IRcount; i++)
                {
                    string strBrand = tsbName.Text + "--" + (i + 1).ToString("D3");
                    if (strBrand != "")
                    {
                        ListViewItem oItem = new ListViewItem(strBrand, lvModel.Groups[0]);
                        oItem.Tag = CsConst.MyLibrary[MyRemID].brand[MyintBrand].IRIndexIntAry[i];
                        lvModel.Items.Add(oItem);
                    }
                }
            }
            catch
            {
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
        }

        private void lvBrands_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                panel4.Visible = false;
                if (lvBrands.SelectedItems == null || lvBrands.SelectedItems.Count == 0) return;
                if (lvBrands.SelectedItems[0].Tag == null) return;
                MyintBrand = Convert.ToInt16(lvBrands.SelectedItems[0].Tag.ToString()) - 1;

                //1)选择品牌
                lbS3.BackColor = Color.Empty;
                lbS4.BackColor = Color.Empty;
                lbS1.BackColor = Color.Cyan;
                lbS2.BackColor = Color.Brown;

                //显示型号
                DisplayAllModelsToListview(MyintBrand);
                string str = "Current Brand:";
                if (CsConst.iLanguageId == 1) str = "当前所选品牌:";
                lbModel.Text = str + lvBrands.SelectedItems[0].SubItems[0].Text;
            }
            catch
            {
            }
        }

        private void lvModel_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (lvModel.SelectedItems == null || lvModel.SelectedItems.Count == 0) return;
                int intTag = Convert.ToInt16(lvModel.SelectedItems[0].Tag.ToString());
                MyCodeLine = intTag;
                //1)选择型号
                lbS4.BackColor = Color.Empty;
                lbS1.BackColor = Color.Cyan;
                lbS2.BackColor = Color.Cyan;
                lbS3.BackColor = Color.Brown;

                //读取红外码备用
                string sql = string.Format("select * from dbNewICode where ID={0} and DIndex={1}", MyRemID + 1, intTag + 1);
                OleDbDataReader drtmp = DataModule.SearchAResultSQLDB(sql);

                if (drtmp != null && drtmp.HasRows)
                {
                    while (drtmp.Read())
                    {
                        MyCodes = drtmp.GetValue(2).ToString();
                    }
                    drtmp.Close();
                }

                if (MyObject is NewIR)
                {
                    panel4.Visible = true;
                }
                else if (MyObject is MMC)
                {
                    panel4.Visible = true;
                }
            }
            catch
            {
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            try
            {
                if (lvModel.Items.Count == 0) return;
                if (lvModel.SelectedItems.Count == 0)
                {
                    lvModel.Items[0].Selected = true;
                    lvModel.EnsureVisible(0);
                }
                else
                {
                    if (lvModel.SelectedItems[0].Index < lvModel.Items.Count - 1)
                    {
                        int intIndex = lvModel.SelectedItems[0].Index + 1;
                        lvModel.Items[intIndex].Selected = true;
                        lvModel.EnsureVisible(intIndex);
                    }
                }
                if (MyObject is MMC)
                {
                    if (MyRemID == 0 || MyRemID == 1 || MyRemID == 2 || MyRemID == 5)
                        button2_Click(btnKey1, null);
                    else if (MyRemID == 3 || MyRemID == 4)
                        button2_Click(btnKey6, null);
                }
                else if (MyObject is NewIR)
                {
                    if (MyDeviceType == 1301 || MyDeviceType == 1300 || MyDeviceType == 6100)
                    {
                        if (MyRemID == 0 || MyRemID == 1 || MyRemID == 2)
                            button2_Click(btnKey1, null);
                        else if (MyRemID == 3 || MyRemID == 4)
                            button2_Click(btnKey6, null);
                        else if (MyRemID == 5)
                        {
                            button2_Click(btnNext, null);
                        }
                    }
                    else
                    {
                        if (MyRemID == 0 || MyRemID == 1 || MyRemID == 2 || MyRemID == 5)
                            button2_Click(btnKey1, null);
                        else if (MyRemID == 3 || MyRemID == 4)
                            button2_Click(btnKey6, null);
                    }
                }
            }
            catch
            {
            }
        }

        private void btnPre_Click(object sender, EventArgs e)
        {
            try
            {
                if (lvModel == null || lvModel.Items.Count == 0) return;
                if (lvModel.SelectedItems == null || lvModel.SelectedItems.Count == 0)
                {
                    lvModel.Items[0].Selected = true;
                    lvModel.EnsureVisible(0);
                }
                else
                {
                    if (lvModel.SelectedItems[0].Index >= 1)
                    {
                        int intIndex = lvModel.SelectedItems[0].Index - 1;
                        lvModel.Items[intIndex].Selected = true;
                        lvModel.EnsureVisible(intIndex);
                    }
                }
                if (MyObject is MMC)
                {
                    if (MyRemID == 0 || MyRemID == 1 || MyRemID == 2 || MyRemID == 5)
                        button2_Click(btnKey1, null);
                    else if (MyRemID == 3 || MyRemID == 4)
                        button2_Click(btnKey6, null);
                }
                else if (MyObject is NewIR)
                {
                    if (MyDeviceType == 1301 || MyDeviceType == 1300 || MyDeviceType == 6100)
                    {
                        if (MyRemID == 0 || MyRemID == 1 || MyRemID == 2)
                            button2_Click(btnKey1, null);
                        else if (MyRemID == 3 || MyRemID == 4)
                            button2_Click(btnKey6, null);
                        else if (MyRemID == 5)
                        {
                            button2_Click(btnPre, null);
                        }
                    }
                    else
                    {
                        if (MyRemID == 0 || MyRemID == 1 || MyRemID == 2 || MyRemID == 5)
                            button2_Click(btnKey1, null);
                        else if (MyRemID == 3 || MyRemID == 4)
                            button2_Click(btnKey6, null);
                    }
                }
            }
            catch
            {
            }
        }

        private string getHexIRcodes(int DevID, string strCodes)
        {
            try
            {
                strCodes = strCodes.Trim();
                strCodes = strCodes.Split('}')[0].ToString().Trim();
                int intTmp = 0;
                for (int i = 0; i < strCodes.Length; i++)
                {
                    string strTmp = strCodes.Substring(i, 1);
                    if (strCodes == ",")
                    {
                        intTmp = intTmp + 1;
                    }
                }
                strCodes = strCodes.Replace("{", "");
                strCodes = strCodes.Replace("}", "");
                strCodes = strCodes.Replace(" ", "");
                strCodes = strCodes.Replace("0x", "");
                return strCodes;
            }
            catch
            {
                //MessageBox.Show(ex.ToString());
                return "";
            }
        }

        private void btnON_Click(object sender, EventArgs e)
        {
            isTestOk = false;
            if (MyCtrl == null) return;
            if (MyCodes == null) return;
            if (lvBrands.SelectedItems == null) return;
            if (lvModel.SelectedItems == null) return;
            if (lvBrands.SelectedItems == null) return;
            if (lvModel.SelectedItems == null) return;
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                string strCode = getHexIRcodes(MyRemID, MyCodes);
                //上传红外码
                string[] ArayCode = strCode.Split(',');

                if (MyObject is MMC)
                {
                    #region
                    byte[] ArayUpload = new byte[26];
                    ArayUpload[0] = Convert.ToByte((MyCtrl.Controls.Find("DgvKey", true)[0] as DataGridView).CurrentRow.Index + 1);
                    ArayUpload[1] = 0;
                    ArayUpload[2] = (byte)(MyRemID);
                    ArayUpload[3] = (byte)(MyCodeLine / 256);
                    ArayUpload[4] = (byte)(MyCodeLine % 256);
                    ArayUpload[5] = (byte)ArayCode.Length;
                    byte[] arayTmpRemark = HDLUDP.StringToByte(lvBrands.SelectedItems[0].Text + "-" + lvModel.SelectedItems[0].Text);
                    HDLSysPF.CopyRemarkBufferToSendBuffer(arayTmpRemark, ArayUpload, 6);

                    if (CsConst.mySends.AddBufToSndList(ArayUpload, 0x137A, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(MyDeviceType)) == true) //请求空间
                    {
                        if (ArayCode.Length > 62)
                        {
                            int Count = ArayCode.Length / 62;
                            if (ArayCode.Length % 62 != 0) Count = Count + 1;
                            for (int i = 0; i < Count; i++)
                            {
                                if (ArayCode.Length % 62 != 0)
                                {
                                    if (i == (Count - 1))
                                    {
                                        ArayUpload = new byte[2 + ArayCode.Length % 62];
                                        ArayUpload[0] = Convert.ToByte((MyCtrl.Controls.Find("DgvKey", true)[0] as DataGridView).CurrentRow.Index + 1);
                                        ArayUpload[1] = Convert.ToByte(i + 1);
                                        for (int j = 0; j < ArayCode.Length % 62; j++) ArayUpload[2 + j] = HDLPF.StringToByte(ArayCode[i * 62 + j].ToString());
                                    }
                                    else
                                    {
                                        ArayUpload = new byte[2 + 62];
                                        ArayUpload[0] = Convert.ToByte((MyCtrl.Controls.Find("DgvKey", true)[0] as DataGridView).CurrentRow.Index + 1);
                                        ArayUpload[1] = Convert.ToByte(i + 1);
                                        for (int j = 0; j < 62; j++) ArayUpload[2 + j] = HDLPF.StringToByte(ArayCode[i * 62 + j].ToString());
                                    }
                                }
                                else
                                {
                                    ArayUpload = new byte[2 + 62];
                                    ArayUpload[0] = Convert.ToByte((MyCtrl.Controls.Find("DgvKey", true)[0] as DataGridView).CurrentRow.Index + 1);
                                    ArayUpload[1] = Convert.ToByte(i + 1);
                                    for (int j = 0; j < 62; j++) ArayUpload[2 + j] = HDLPF.StringToByte(ArayCode[i * 62 + j].ToString());
                                }
                                if (CsConst.mySends.AddBufToSndList(ArayUpload, 0x137A, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(MyDeviceType)) == true)
                                {
                                    CsConst.myRevBuf = new byte[1200];
                                }
                                else
                                {
                                    break;
                                }
                            }
                            isTestOk = true;
                        }
                        else
                        {
                            ArayUpload = new byte[2 + ArayCode.Length];
                            ArayUpload[0] = Convert.ToByte((MyCtrl.Controls.Find("DgvKey", true)[0] as DataGridView).CurrentRow.Index + 1);
                            ArayUpload[1] = 1;
                            for (int i = 0; i < ArayCode.Length; i++) ArayUpload[2 + i] = HDLPF.StringToByte(ArayCode[i].ToString());
                            // 上传试红外码
                            if (CsConst.mySends.AddBufToSndList(ArayUpload, 0x137A, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(MyDeviceType)) == true)
                            {
                                isTestOk = true;
                                CsConst.myRevBuf = new byte[1200];
                            }
                        }
                    }
                    #endregion
                }
                else if (MyObject is NewIR)
                {
                    if (MyDeviceType == 729 || MyDeviceType == 1300 || MyDeviceType == 1301)
                    {
                        if ((MyCtrl.Controls.Find("dgvIR", true)[0] as DataGridView).CurrentRow.Index < 4)
                        {
                            if (MyRemID != 5)
                            {
                                string str = CsConst.mstrINIDefault.IniReadValue("Public", "99858", "");
                                MessageBox.Show(str);
                                return;
                            }
                        }
                        else
                        {
                            if (MyRemID == 5)
                            {
                                string str = CsConst.mstrINIDefault.IniReadValue("Public", "99858", "");
                                MessageBox.Show(str);
                                return;
                            }
                        }
                    }
                    else if (MyDeviceType == 6100)
                    {
                        if ((MyCtrl.Controls.Find("dgvIR", true)[0] as DataGridView).CurrentRow.Index < 3)
                        {
                            if (MyRemID != 5)
                            {
                                string str = CsConst.mstrINIDefault.IniReadValue("Public", "99858", "");
                                MessageBox.Show(str);
                                return;
                            }
                        }
                        else
                        {
                            if (MyRemID == 5)
                            {
                                string str = CsConst.mstrINIDefault.IniReadValue("Public", "99858", "");
                                MessageBox.Show(str);
                                return;
                            }
                        }
                    }
                    byte[] ArayUpload = new byte[26];
                    if (MyDeviceType == 1301 || MyDeviceType == 1300 || MyDeviceType == 6100)
                        ArayUpload = new byte[27];
                    ArayUpload[0] = Convert.ToByte((MyCtrl.Controls.Find("dgvIR", true)[0] as DataGridView).CurrentRow.Index + 1);
                    ArayUpload[1] = 0;
                    ArayUpload[2] = (byte)(MyRemID);
                    ArayUpload[3] = (byte)(MyCodeLine / 256);
                    ArayUpload[4] = (byte)(MyCodeLine % 256);
                    ArayUpload[5] = (byte)ArayCode.Length;
                    
                    byte[] arayTmpRemark = HDLUDP.StringToByte(lvBrands.SelectedItems[0].Text + "-" + lvModel.SelectedItems[0].Text);
                    if (arayTmpRemark.Length > 20)
                    {
                        Array.Copy(arayTmpRemark, 0, ArayUpload, 6, 20);
                    }
                    else
                    {
                        Array.Copy(arayTmpRemark, 0, ArayUpload, 6, arayTmpRemark.Length);
                    }
                    if (MyDeviceType == 1301 || MyDeviceType == 1300)
                    {
                        if (MyRemID == 5)
                            ArayUpload[26] = 1;
                        else
                            ArayUpload[26] = 0;
                        if (MyRemID != 5)
                        {
                            ArayUpload[0] = Convert.ToByte(ArayUpload[0] - 4);
                        }
                    }
                    else if (MyDeviceType == 6100)
                    {
                        if (MyRemID == 5)
                            ArayUpload[26] = 1;
                        else
                            ArayUpload[26] = 0;
                        if (MyRemID != 5)
                        {
                            ArayUpload[0] = Convert.ToByte(ArayUpload[0] - 3);
                        }
                    }
                    if (CsConst.mySends.AddBufToSndList(ArayUpload, 0x137A, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(MyDeviceType)) == true) //请求空间
                    {
                        if (ArayCode.Length > 62)
                        {
                            int Count = ArayCode.Length / 62;
                            if (ArayCode.Length % 62 != 0) Count = Count + 1;
                            for (int i = 0; i < Count; i++)
                            {
                                if (ArayCode.Length % 62 != 0)
                                {
                                    if (i == (Count - 1))
                                    {
                                        ArayUpload = new byte[2 + ArayCode.Length % 62];
                                        ArayUpload[0] = Convert.ToByte((MyCtrl.Controls.Find("dgvIR", true)[0] as DataGridView).CurrentRow.Index + 1);
                                        if (MyDeviceType == 1301 || MyDeviceType==1300)
                                        {
                                            if (MyRemID != 5)
                                            {
                                                ArayUpload[0] = Convert.ToByte(ArayUpload[0] - 4);
                                            }
                                        }
                                        else if (MyDeviceType == 6100)
                                        {
                                            if (MyRemID != 5)
                                            {
                                                ArayUpload[0] = Convert.ToByte(ArayUpload[0] - 3);
                                            }
                                        }
                                        ArayUpload[1] = Convert.ToByte(i + 1);
                                        for (int j = 0; j < ArayCode.Length % 62; j++) ArayUpload[2 + j] = HDLPF.StringToByte(ArayCode[i * 62 + j].ToString());
                                    }
                                    else
                                    {
                                        ArayUpload = new byte[2 + 62];
                                        ArayUpload[0] = Convert.ToByte((MyCtrl.Controls.Find("dgvIR", true)[0] as DataGridView).CurrentRow.Index + 1);
                                        if (MyDeviceType == 1301 || MyDeviceType == 1300)
                                        {
                                            if (MyRemID != 5)
                                            {
                                                ArayUpload[0] = Convert.ToByte(ArayUpload[0] - 4);
                                            }
                                        }
                                        else if (MyDeviceType == 6100)
                                        {
                                            if (MyRemID != 5)
                                            {
                                                ArayUpload[0] = Convert.ToByte(ArayUpload[0] - 3);
                                            }
                                        }
                                        ArayUpload[1] = Convert.ToByte(i + 1);
                                        for (int j = 0; j < 62; j++) ArayUpload[2 + j] = HDLPF.StringToByte(ArayCode[i * 62 + j].ToString());
                                    }
                                }
                                else
                                {
                                    ArayUpload = new byte[2 + 62];
                                    ArayUpload[0] = Convert.ToByte((MyCtrl.Controls.Find("dgvIR", true)[0] as DataGridView).CurrentRow.Index + 1);
                                    if (MyDeviceType == 1301 || MyDeviceType == 1300)
                                    {
                                        if (MyRemID != 5)
                                        {
                                            ArayUpload[0] = Convert.ToByte(ArayUpload[0] - 4);
                                        }
                                    }
                                    else if (MyDeviceType == 6100)
                                    {
                                        if (MyRemID != 5)
                                        {
                                            ArayUpload[0] = Convert.ToByte(ArayUpload[0] - 3);
                                        }
                                    }
                                    ArayUpload[1] = Convert.ToByte(i + 1);
                                    for (int j = 0; j < 62; j++) ArayUpload[2 + j] = HDLPF.StringToByte(ArayCode[i * 62 + j].ToString());
                                }
                                if (CsConst.mySends.AddBufToSndList(ArayUpload, 0x137A, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(MyDeviceType)) == true)
                                {
                                    CsConst.myRevBuf = new byte[1200];
                                }
                                else
                                {
                                    break;
                                }
                            }
                            isTestOk = true;
                        }
                        else
                        {
                            ArayUpload = new byte[2 + ArayCode.Length];
                            ArayUpload[0] = Convert.ToByte((MyCtrl.Controls.Find("dgvIR", true)[0] as DataGridView).CurrentRow.Index + 1);
                            if (MyDeviceType == 1301 || MyDeviceType == 1300)
                            {
                                if (MyRemID != 5)
                                {
                                    ArayUpload[0] = Convert.ToByte(ArayUpload[0] - 4);
                                }
                            }
                            else if (MyDeviceType == 6100)
                            {
                                if (MyRemID != 5)
                                {
                                    ArayUpload[0] = Convert.ToByte(ArayUpload[0] - 3);
                                }
                            }
                            ArayUpload[1] = 1;
                            for (int i = 0; i < ArayCode.Length; i++) ArayUpload[2 + i] = HDLPF.StringToByte(ArayCode[i].ToString());
                            // 上传试红外码
                            if (CsConst.mySends.AddBufToSndList(ArayUpload, 0x137A, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(MyDeviceType)) == true)
                            {
                                CsConst.myRevBuf = new byte[1200];
                                isTestOk = true;
                            }
                        }
                    }
                }
                
            }
            catch
            {
            }
            btnOK_Click(null, null);
            Cursor.Current = Cursors.Default;
        }

        private void label15_Click(object sender, EventArgs e)
        {

        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                if (MyObject == null) return;
                // DialogResult = DialogResult.None;
                if (MyObject is MMC)   //是不是多媒体播放器类型
                {
                    if (isTestOk)
                    {
                        if (((MMC)MyObject).IRCodes == null) return;
                        string strCode = getHexIRcodes(MyRemID, MyCodes);
                        string[] Tmp = strCode.Split(',');
                        MMC.NewIRCode oTmp = ((MMC)MyObject).IRCodes[(MyCtrl.Controls.Find("DgvKey", true)[0] as DataGridView).CurrentRow.Index];
                        string str = CsConst.NewIRLibraryDeviceType[MyRemID];
                        (MyCtrl.Controls.Find("DgvKey", true)[0] as DataGridView)[1, (MyCtrl.Controls.Find("DgvKey", true)[0] as DataGridView).CurrentRow.Index].Value = str;
                        (MyCtrl.Controls.Find("DgvKey", true)[0] as DataGridView)[2, (MyCtrl.Controls.Find("DgvKey", true)[0] as DataGridView).CurrentRow.Index].Value = lvBrands.SelectedItems[0].Text + "-" + lvModel.SelectedItems[0].Text;
                        oTmp.Remark = lvBrands.SelectedItems[0].Text + "-" + lvModel.SelectedItems[0].Text;
                        oTmp.Codes = strCode;
                        oTmp.IRIndex = MyCodeLine;
                        oTmp.IRLength = Tmp.Length;
                        oTmp.DevID = (byte)MyRemID;
                    }
                }
                else if (MyObject is NewIR)//新型红外发射模块
                {
                    if (isTestOk)
                    {
                        if (((NewIR)MyObject).IRCodes == null) return;
                        string strCode = getHexIRcodes(MyRemID, MyCodes);
                        string[] Tmp = strCode.Split(',');
                        NewIR.NewIRCode oTmp = ((NewIR)MyObject).IRCodes[(MyCtrl.Controls.Find("dgvIR", true)[0] as DataGridView).CurrentRow.Index];
                        string str = CsConst.NewIRLibraryDeviceType[MyRemID];
                        (MyCtrl.Controls.Find("dgvIR", true)[0] as DataGridView)[1, (MyCtrl.Controls.Find("dgvIR", true)[0] as DataGridView).CurrentRow.Index].Value = str;
                        (MyCtrl.Controls.Find("dgvIR", true)[0] as DataGridView)[2, (MyCtrl.Controls.Find("dgvIR", true)[0] as DataGridView).CurrentRow.Index].Value = lvBrands.SelectedItems[0].Text + "-" + lvModel.SelectedItems[0].Text;
                        oTmp.Remark = lvBrands.SelectedItems[0].Text + "-" + lvModel.SelectedItems[0].Text;
                        oTmp.Codes = strCode;
                        oTmp.IRIndex = MyCodeLine;
                        oTmp.IRLength = Tmp.Length;
                        oTmp.DevID = (byte)MyRemID;
                    }
                }
            }
            catch
            {
            }
        }

        void frmIRLibrary_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void btnAuto_Click(object sender, EventArgs e)
        {
            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                panel4.Visible = false;
                string str = textBox1.Text;
                if (str == null || str == "") return;
                bool isEmpty = false;
                for (int i = 0; i < lvBrands.Items.Count; i++)
                {
                    if (lvBrands.Items[i].Text.Contains(str))
                    {
                        lvBrands.Items[i].Selected = true;
                        lvBrands.EnsureVisible(i);
                        isEmpty = false;
                        break;
                    }
                    else
                    {
                        isEmpty = true;
                    }
                }
                if (isEmpty)
                {
                    for (int i = 0; i < lvBrands.Items.Count; i++)
                    {
                        if (lvBrands.Items[i].Text.Contains(str.Substring(0, 1)))
                        {
                            lvBrands.Items[i].Selected = true;
                            lvBrands.EnsureVisible(i);
                            break;
                        }
                    }
                }
            }
            catch
            {
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (MyObject is MMC)
                {
                    #region
                    string strCode = getHexIRcodes(MyRemID, MyCodes);
                    //上传红外码
                    string[] ArayCode = strCode.Split(',');
                    byte[] ArayUpload = new byte[26];
                    ArayUpload[0] = 25;
                    ArayUpload[1] = 0;
                    ArayUpload[2] = (byte)(MyRemID);
                    ArayUpload[3] = (byte)(MyCodeLine / 256);
                    ArayUpload[4] = (byte)(MyCodeLine % 256);
                    ArayUpload[5] = (byte)ArayCode.Length;
                    byte[] arayTmpRemark = HDLUDP.StringToByte(lvBrands.SelectedItems[0].Text + "-" + lvModel.SelectedItems[0].Text);
                    if (arayTmpRemark.Length > 20)
                        Array.Copy(arayTmpRemark, 0, ArayUpload, 6, 20);
                    else
                        Array.Copy(arayTmpRemark, 0, ArayUpload, 6, arayTmpRemark.Length);

                    if (CsConst.mySends.AddBufToSndList(ArayUpload, 0x137A, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(MyDeviceType)) == true) //请求空间
                    {
                        if (ArayCode.Length > 62)
                        {
                            int Count = ArayCode.Length / 62;
                            if (ArayCode.Length % 62 != 0) Count = Count + 1;
                            for (int i = 0; i < Count; i++)
                            {
                                if (ArayCode.Length % 62 != 0)
                                {
                                    if (i == (Count - 1))
                                    {
                                        ArayUpload = new byte[2 + ArayCode.Length % 62];
                                        ArayUpload[0] = 25;
                                        ArayUpload[1] = Convert.ToByte(i + 1);
                                        for (int j = 0; j < ArayCode.Length % 62; j++) ArayUpload[2 + j] = HDLPF.StringToByte(ArayCode[i * 62 + j].ToString());
                                    }
                                    else
                                    {
                                        ArayUpload = new byte[2 + 62];
                                        ArayUpload[0] = 25;
                                        ArayUpload[1] = Convert.ToByte(i + 1);
                                        for (int j = 0; j < 62; j++) ArayUpload[2 + j] = HDLPF.StringToByte(ArayCode[i * 62 + j].ToString());
                                    }
                                }
                                else
                                {
                                    ArayUpload = new byte[2 + 62];
                                    ArayUpload[0] = 25;
                                    ArayUpload[1] = Convert.ToByte(i + 1);
                                    for (int j = 0; j < 62; j++) ArayUpload[2 + j] = HDLPF.StringToByte(ArayCode[i * 62 + j].ToString());
                                }
                                if (CsConst.mySends.AddBufToSndList(ArayUpload, 0x137A, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(MyDeviceType)) == true)
                                {
                                    CsConst.myRevBuf = new byte[1200];
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                        else
                        {
                            ArayUpload = new byte[2 + ArayCode.Length];
                            ArayUpload[0] = 25;
                            ArayUpload[1] = 1;
                            for (int i = 0; i < ArayCode.Length; i++) ArayUpload[2 + i] = HDLPF.StringToByte(ArayCode[i].ToString());
                            // 上传试红外码
                            if (CsConst.mySends.AddBufToSndList(ArayUpload, 0x137A, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(MyDeviceType)) == true)
                            {
                                CsConst.myRevBuf = new byte[1200];
                            }
                        }
                    }
                    byte[] arayTmp = new byte[3];
                    arayTmp[0] = Convert.ToByte(cbChannel.SelectedIndex + 1);
                    arayTmp[1] = 25;
                    arayTmp[2] = Convert.ToByte(Convert.ToInt32((sender as Button).Tag));
                    if (CsConst.mySends.AddBufToSndList(arayTmp, 0x0000, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(MyDeviceType)) == true)
                    {
                        CsConst.myRevBuf = new byte[1200];
                    }
                    #endregion
                }
                else if (MyObject is NewIR)
                {
                    #region
                    string strCode = getHexIRcodes(MyRemID, MyCodes);
                    //上传红外码
                    string[] ArayCode = strCode.Split(',');
                    byte[] ArayUpload = new byte[26];
                    ArayUpload[0] = 29;
                    if (MyDeviceType == 6100) ArayUpload[0] = 12;
                    else if (MyDeviceType == 1300) ArayUpload[0] = 30;
                    else if (MyDeviceType == 1301) ArayUpload[0] = 30;
                    ArayUpload[1] = 0;
                    ArayUpload[2] = (byte)(MyRemID);
                    ArayUpload[3] = (byte)(MyCodeLine / 256);
                    ArayUpload[4] = (byte)(MyCodeLine % 256);
                    ArayUpload[5] = (byte)ArayCode.Length;
                    byte[] arayTmpRemark = HDLUDP.StringToByte(lvBrands.SelectedItems[0].Text + "-" + lvModel.SelectedItems[0].Text);
                    if (arayTmpRemark.Length > 20)
                    {
                        Array.Copy(arayTmpRemark, 0, ArayUpload, 6, 20);
                    }
                    else
                    {
                        Array.Copy(arayTmpRemark, 0, ArayUpload, 6, arayTmpRemark.Length);
                    }

                    if (CsConst.mySends.AddBufToSndList(ArayUpload, 0x137A, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(MyDeviceType)) == true) //请求空间
                    {
                        if (ArayCode.Length > 62)
                        {
                            int Count = ArayCode.Length / 62;
                            if (ArayCode.Length % 62 != 0) Count = Count + 1;
                            for (int i = 0; i < Count; i++)
                            {
                                if (ArayCode.Length % 62 != 0)
                                {
                                    if (i == (Count - 1))
                                    {
                                        ArayUpload = new byte[2 + ArayCode.Length % 62];
                                        ArayUpload[0] = 29;
                                        if (MyDeviceType == 6100) ArayUpload[0] = 12;
                                        else if (MyDeviceType == 1300) ArayUpload[0] = 30;
                                        else if (MyDeviceType == 1301) ArayUpload[0] = 30;
                                        ArayUpload[1] = Convert.ToByte(i + 1);
                                        for (int j = 0; j < ArayCode.Length % 62; j++) ArayUpload[2 + j] = HDLPF.StringToByte(ArayCode[i * 62 + j].ToString());
                                    }
                                    else
                                    {
                                        ArayUpload = new byte[2 + 62];
                                        ArayUpload[0] = 29;
                                        if (MyDeviceType == 6100) ArayUpload[0] = 12;
                                        else if (MyDeviceType == 1300) ArayUpload[0] = 30;
                                        else if (MyDeviceType == 1301) ArayUpload[0] = 30;
                                        ArayUpload[1] = Convert.ToByte(i + 1);
                                        for (int j = 0; j < 62; j++) ArayUpload[2 + j] = HDLPF.StringToByte(ArayCode[i * 62 + j].ToString());
                                    }
                                }
                                else
                                {
                                    ArayUpload = new byte[2 + 62];
                                    ArayUpload[0] = 29;
                                    if (MyDeviceType == 6100) ArayUpload[0] = 12;
                                    else if (MyDeviceType == 1300) ArayUpload[0] = 30;
                                    else if (MyDeviceType == 1301) ArayUpload[0] = 30;
                                    ArayUpload[1] = Convert.ToByte(i + 1);
                                    for (int j = 0; j < 62; j++) ArayUpload[2 + j] = HDLPF.StringToByte(ArayCode[i * 62 + j].ToString());
                                }
                                if (CsConst.mySends.AddBufToSndList(ArayUpload, 0x137A, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(MyDeviceType)) == true)
                                {
                                    CsConst.myRevBuf = new byte[1200];
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                        else
                        {
                            ArayUpload = new byte[2 + ArayCode.Length];
                            ArayUpload[0] = 29;
                            if (MyDeviceType == 6100) ArayUpload[0] = 12;
                            else if (MyDeviceType == 1300) ArayUpload[0] = 30;
                            else if (MyDeviceType == 1301) ArayUpload[0] = 30;
                            ArayUpload[1] = 1;
                            for (int i = 0; i < ArayCode.Length; i++) ArayUpload[2 + i] = HDLPF.StringToByte(ArayCode[i].ToString());
                            // 上传试红外码
                            if (CsConst.mySends.AddBufToSndList(ArayUpload, 0x137A, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(MyDeviceType)) == true)
                            {
                                CsConst.myRevBuf = new byte[1200];
                            }
                        }
                    }
                    Cursor.Current = Cursors.WaitCursor;
                    byte[] arayTmp = new byte[4];
                    if (MyDeviceType == 729)
                    {
                        arayTmp = new byte[4];
                        arayTmp[0] = 1;
                        arayTmp[1] = Convert.ToByte(cbChannel.SelectedIndex + 1);
                        arayTmp[2] = 29;
                        arayTmp[3] = Convert.ToByte(Convert.ToInt32((sender as Button).Tag));
                        if (CsConst.mySends.AddBufToSndList(arayTmp, 0xdb90, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(MyDeviceType)) == true)
                        {
                            CsConst.myRevBuf = new byte[1200];
                        }
                    }
                    else if (MyDeviceType == 1301 || MyDeviceType == 1300 || MyDeviceType == 6100)
                    {
                        if (CurrentSelectDevice == 5)
                        {
                            arayTmp = new byte[13];
                            arayTmp[0] = Convert.ToByte(128 + (cbChannel.SelectedIndex + 1));
                            arayTmp[8] = Convert.ToByte(cbPower.SelectedIndex);
                            arayTmp[9] = Convert.ToByte(cbMode.SelectedIndex);
                            arayTmp[10] = Convert.ToByte(cbFan.SelectedIndex);
                            arayTmp[11] = Convert.ToByte(sbTemp.Value);
                            arayTmp[12] = Convert.ToByte(cbWind.SelectedIndex);
                            if (CsConst.mySends.AddBufToSndList(arayTmp, 0x193A, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(MyDeviceType)) == true)
                            {
                                CsConst.myRevBuf = new byte[1200];
                            }
                        }
                        else
                        {
                            arayTmp = new byte[4];
                            arayTmp[0] = Convert.ToByte(cbChannel.SelectedIndex + 1);
                            int int1 = 0;
                            int int2 = cbChannel.SelectedIndex + 1;
                            string str1 = GlobalClass.IntToBit(int1, 2);
                            string str2 = GlobalClass.IntToBit(int2, 6);
                            string str = str1 + str2;
                            arayTmp[1] = 30;
                            arayTmp[2] = Convert.ToByte(Convert.ToInt32((sender as Button).Tag));
                            arayTmp[3] = 0;
                            if (CsConst.mySends.AddBufToSndList(arayTmp, 0xdb90, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(MyDeviceType)) == true)
                            {
                                CsConst.myRevBuf = new byte[1200];
                            }
                        }
                    }
                    
                    #endregion
                }
                Cursor.Current = Cursors.Default;
            }
            catch
            {
            }
            Cursor.Current = Cursors.Default;
        }

        private void lvBrands_MouseClick(object sender, MouseEventArgs e)
        {
            panel4.Visible = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            panel4.Visible = false;
        }

        private void txtKey_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar) || e.KeyChar == 8)
                e.Handled = false;
            else
                e.Handled = true;
        }

        private void txtKey_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void lvBrands_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            for (int i = 0; i < lvBrands.Items.Count; i++)
            {
                (sender as ListView).Items[i].BackColor = Color.FromArgb(255, 255, 255); //设置选中项的背景颜色 
            }
            (sender as ListView).Items[e.ItemIndex].BackColor = Color.FromArgb(255, 0, 0); //设置选中项的背景颜色 
        }

        private void cbChannel_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void cbKey_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void lbChannel_Click(object sender, EventArgs e)
        {

        }

        private void lbS1_Click(object sender, EventArgs e)
        {

        }

        private void sbTemp_ValueChanged(object sender, EventArgs e)
        {
            TempValue.Text = sbTemp.Value.ToString() + "C";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            panel4.Visible = false;
        }

        private void btnAuto_Click_1(object sender, EventArgs e)
        {
            int Tag = Convert.ToInt32(btnAutoTest.Tag);
            if (Tag == 1)
            {
                timer1.Interval = Convert.ToInt32(nunAuto.Value) * 1000;
                timer1.Enabled = true;
                btnAutoTest.Text = CsConst.mstrINIDefault.IniReadValue("Public", "99580", "");
                btnAutoTest.Tag = 0;
            }
            else if (Tag == 0)
            {
                timer1.Interval = Convert.ToInt32(nunAuto.Value) * 1000;
                timer1.Enabled = false;
                btnAutoTest.Text = CsConst.mstrINIDefault.IniReadValue("Public", "99581", "");
                btnAutoTest.Tag = 1;
            }

        }

        private void timer1_Tick_1(object sender, EventArgs e)
        {
            btnNext_Click(btnNext, null);
        }

    }
}
