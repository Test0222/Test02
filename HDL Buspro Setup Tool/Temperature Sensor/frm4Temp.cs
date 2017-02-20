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
    public partial class frm4Temp : Form
    {
        [DllImport("user32.dll")]
        public static extern Boolean FlashWindow(IntPtr hwnd, Boolean bInvert);

        private TempSensor oSensor = null;
        private string mstrName = null;
        private int myintDIndex = -1;
        private int myintDeviceTpye = -1;

        private int MyActivePage = 0; //按页面上传下载

        public frm4Temp()
        {
            InitializeComponent();
        }

        public frm4Temp(TempSensor ts, string strName, int intDIndex, int intDeviceType)
        {
            InitializeComponent();
           // Localization.ReadLanguage(this);
            this.oSensor = ts;
            mstrName = strName;
            myintDIndex = intDIndex;
            this.myintDeviceTpye = intDeviceType;
            string strDevName = strName.Split('\\')[0].ToString();

            HDLSysPF.DisplayDeviceNameModeDescription(strName, myintDeviceTpye, cboDevice, tbModel, tbDescription);
            tsl31.Text = strName;
            this.Text = strName;
        }

        private void frm4Temp_Load(object sender, EventArgs e)
        {
            //Public.Localization.SaveLanguage(((Control)sender), Public.Localization.LanguageType.English);

            InitialFormCtrlsTextOrItems();
        }

        void InitialFormCtrlsTextOrItems()
        {
            //Public.Localization.ReadLanguage(((Control)sender), Public.Localization.LanguageType.English);
            string[] strTmp = CsConst.mstrINIDefault.IniReadValuesInALlSectionStr("PublicTempType");
            clType.Items.Clear(); clType.Items.AddRange(strTmp);

            #region
            dgvTemp.Columns[0].Width = (int)(dgvTemp.Width * 0.09);
            dgvTemp.Columns[1].Width = (int)(dgvTemp.Width * 0.2);
            dgvTemp.Columns[2].Width = (int)(dgvTemp.Width * 0.1);
            dgvTemp.Columns[3].Width = (int)(dgvTemp.Width * 0.1);
            dgvTemp.Columns[4].Width = (int)(dgvTemp.Width * 0.1);
            dgvTemp.Columns[5].Width = (int)(dgvTemp.Width * 0.09);
            dgvTemp.Columns[6].Width = (int)(dgvTemp.Width * 0.2);
            dgvTemp.Columns[7].Width = (int)(dgvTemp.Width * 0.1);
            #endregion

            #region
            cl7.Items.Clear();
            if (CsConst.myOnlines != null || CsConst.myOnlines.Count !=0)
            {
                foreach (DevOnLine oDev in CsConst.myOnlines)
                {
                     if (oDev.DevName != mstrName) cl7.Items.Add(oDev.DevName);
                }
            }
            cl7.Items.Add("All above");
            #endregion
        }

        private void ShowTempSensor()
        {
            this.Text = mstrName;
            tsl31.Text = mstrName;
            dgvTemp.Rows.Clear();
            if (oSensor.Chans != null)
            {
                int i = 0;
                foreach (TempSensor.Chn ch in oSensor.Chans)
                {
                    i++;
                    if (ch.bytRValue == null) ch.bytRValue = new byte[6];
                    if (ch.byAdjustVal < 2 || ch.byAdjustVal > 18) ch.byAdjustVal = 10;

                    object[] obj = new object[] { i,clType.Items[ch.bytTheType],
                    ch.bytRValue[0]*256+ch.bytRValue[1],
                    ch.bytRValue[2]*256+ch.bytRValue[3],
                    ch.bytRValue[4]*256+ch.bytRValue[5],
                    ch.blnBroadTemp,cl7.Items[0],ch.byAdjustVal-10,};
                    dgvTemp.Rows.Add(obj);
                }
            }
        }

        private void DisplayCtrlEnableOrDisable(int intRowIndex)
        {
            int intType = clType.Items.IndexOf(dgvTemp[1,intRowIndex].Value.ToString()); // 当前选择的热敏电阻类型
            oSensor.Chans[intRowIndex].bytTheType = (byte)intType;

            dgvTemp[2, intRowIndex].ReadOnly = (intType != 3);
            dgvTemp[3, intRowIndex].ReadOnly = (intType != 3);
            dgvTemp[4, intRowIndex].ReadOnly = (intType != 3);
            dgvTemp[5, intRowIndex].ReadOnly = (intType == 0);
            dgvTemp[6, intRowIndex].ReadOnly = (intType == 0);
            dgvTemp[7, intRowIndex].ReadOnly = (intType == 0);

            switch (intType) 
            {
                case 1:
                    oSensor.Chans[intRowIndex].bytRValue = new byte[] { 29100 / 256, 29100 % 256, 18570 / 256, 18570 % 256, 10000 / 256, 10000 % 256 };
                    break;
                case 2:
                    oSensor.Chans[intRowIndex].bytRValue = new byte[] { 37942 / 256, 37942 % 256, 23364 / 256, 23364 % 256, 12000 / 256, 12000 % 256 };
                    break;
            }

            dgvTemp[2, intRowIndex].Value = oSensor.Chans[intRowIndex].bytRValue[0] * 256 + oSensor.Chans[intRowIndex].bytRValue[1];
            dgvTemp[3, intRowIndex].Value = oSensor.Chans[intRowIndex].bytRValue[2] * 256 + oSensor.Chans[intRowIndex].bytRValue[3];
            dgvTemp[4, intRowIndex].Value = oSensor.Chans[intRowIndex].bytRValue[4] * 256 + oSensor.Chans[intRowIndex].bytRValue[5];

            dgvTemp[6, intRowIndex].ReadOnly = (dgvTemp[5, intRowIndex].Value.ToString().ToLower() != "true");
            cl7.Frozen = dgvTemp[6, intRowIndex].ReadOnly;
        }

        private void UpdateDeatailsSettings(int intRowIndex)
        {
            oSensor.Chans[intRowIndex].bytRValue[0] = (byte)(Convert.ToInt32(dgvTemp[2, intRowIndex].Value.ToString()) / 256);
            oSensor.Chans[intRowIndex].bytRValue[1] = (byte)(Convert.ToInt32(dgvTemp[2, intRowIndex].Value.ToString()) % 256);
            oSensor.Chans[intRowIndex].bytRValue[2] = (byte)(Convert.ToInt32(dgvTemp[3, intRowIndex].Value.ToString()) / 256);
            oSensor.Chans[intRowIndex].bytRValue[3] = (byte)(Convert.ToInt32(dgvTemp[3, intRowIndex].Value.ToString()) % 256);
            oSensor.Chans[intRowIndex].bytRValue[4] = (byte)(Convert.ToInt32(dgvTemp[4, intRowIndex].Value.ToString()) / 256);
            oSensor.Chans[intRowIndex].bytRValue[5] = (byte)(Convert.ToInt32(dgvTemp[4, intRowIndex].Value.ToString()) % 256);

            oSensor.Chans[intRowIndex].blnBroadTemp = (dgvTemp[5, intRowIndex].Value.ToString().ToLower() == "true");

            if (cl7.Items.IndexOf(dgvTemp[6, intRowIndex].Value.ToString()) == cl7.Items.Count-1)
            {
                oSensor.Chans[intRowIndex].bytSubID = 255; ; //子网ID
                oSensor.Chans[intRowIndex].bytDevID = 255; ; //设备ID
            }
            else
            {
                string strDevName = dgvTemp[6, intRowIndex].Value.ToString().Split('\\')[0].ToString();

                oSensor.Chans[intRowIndex].bytSubID = Convert.ToByte(strDevName.Split('-')[0]); ; //子网ID
                oSensor.Chans[intRowIndex].bytDevID = Convert.ToByte(strDevName.Split('-')[1]); ; //设备ID
            }

            string strTmp = dgvTemp[7, intRowIndex].Value.ToString();
            dgvTemp[7, intRowIndex].Value = HDLPF.IsNumStringMode(strTmp, 0, 100);

            oSensor.Chans[intRowIndex].byAdjustVal = byte.Parse(dgvTemp[7, intRowIndex].Value.ToString());
        }

        private void dgvTemp_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dgvTemp_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == -1 || e.RowIndex == -1) return;
          
            if (e.Button == MouseButtons.Right)
            {
                if ((e.RowIndex >= 0) & ((sender as DataGridView).RowCount > 0))  //如果行数不为0
                {
                    // DgChns.CurrentCell = DgChns.Rows[e.RowIndex].Cells[e.ColumnIndex];
                    myintRowIndex = e.RowIndex;
                    myintColIndex = e.ColumnIndex;
                    cm4.Show(MousePosition.X, MousePosition.Y);
                }
            }
            DisplayCtrlEnableOrDisable(e.RowIndex);
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            if (oSensor == null) return;
            oSensor.ReadDefaultInfo(4);

            ShowTempSensor();
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            if (oSensor == null) return;
             oSensor.SaveInfoToDb();
        }


        void SetVisableForDownOrUpload(bool blnIsEnable)
        {
            tslRead.Enabled = blnIsEnable;
            toolStripLabel2.Enabled = blnIsEnable;

            if (blnIsEnable == true && tsbar.Value != 0)
            {
                tsbHint.Visible = true;
                tsbHint.Text = "";
            }
            else
            {
                tsbar.Value = 0;
                tsbl4.Text = "0";
                tsbHint.Visible = false;
            }

            if (tsbar.Value == 100) tsbHint.Text = "Fully Success!";
            tsbar.Visible = !blnIsEnable;
            tsbl4.Visible = !blnIsEnable;
        }


        private void frm4Temp_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (CsConst.MyEditMode == 1)
            {
                if (CsConst.myUploadMode == 0 || CsConst.myUploadMode == 1)
                {
                    if (oSensor.MyRead2UpFlags[1] == false)
                    {
                        MyActivePage = 0;
                        tslRead_Click(toolStripLabel2, null);
                    }
                }
            }
        }

        private void frm4Temp_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                FlashWindow(this.Handle, true);
            }
        }

        private void frm4Temp_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Dispose();
        }

        private void tmSame_Click(object sender, EventArgs e)
        {
            DataGridView oDG = dgvTemp;

            if (oDG.SelectedCells == null) return;
            if (oDG.SelectedRows == null) return;
            if (myintRowIndex == -1) return;
            if (myintColIndex == -1) return;
            string strTmp = oDG[myintColIndex, myintRowIndex].Value.ToString();

            int intTag = Convert.ToInt16((sender as ToolStripMenuItem).Tag.ToString());
            UpdateBufferWhenchanged(intTag, strTmp, oDG);
        }

        private int myintRowIndex = -1; // 当前行列
        private int myintColIndex = -1;
        void UpdateBufferWhenchanged(int intTag, string strTmp, DataGridView oDGV)
        {
            if (oDGV.SelectedCells == null) return;
            if (oDGV.SelectedRows == null) return;
            string strNo = HDLPF.GetNumFromString(strTmp);
            if (strNo == "") strNo = "0";

            switch (intTag)
            {
                case 0:
                    foreach (DataGridViewRow r in oDGV.SelectedRows)
                    {
                        if (r.Selected & r.Index != myintRowIndex)
                        {
                            oDGV[myintColIndex, r.Index].Value = strTmp;
                        }
                    }
                    break;
                case 1:
                    int intTmp = Convert.ToInt16(strNo);
                    foreach (DataGridViewRow r in oDGV.Rows)
                    {
                        if (r.Selected & r.Index != myintRowIndex)
                        {
                            intTmp++;
                            oDGV[myintColIndex, r.Index].Value = strTmp.Replace(strNo, intTmp.ToString());
                        }
                    }
                    break;
                case 2:
                    intTmp = Convert.ToInt16(strNo);
                    foreach (DataGridViewRow r in oDGV.Rows)
                    {
                        if (r.Selected & r.Index != myintRowIndex)
                        {
                            intTmp--;
                            if (intTmp <= 0) intTmp = 255;
                            oDGV[myintColIndex, r.Index].Value = strTmp.Replace(strNo, intTmp.ToString());
                        }
                    }
                    break;
            }
        }

        private void toolStripButton11_Click(object sender, EventArgs e)
        {
            if (oSensor == null) return;
        }

        private void tslRead_Click(object sender, EventArgs e)
        {
            byte bytTag = Convert.ToByte(((ToolStripButton)sender).Tag.ToString());
            bool blnShowMsg = (CsConst.MyEditMode != 1);
            if (HDLPF.UploadOrReadGoOn(this.Text, bytTag, blnShowMsg))
            {
                Cursor.Current = Cursors.WaitCursor;

                CsConst.MyUPload2DownLists = new List<byte[]>();

                string strName = mstrName.Split('\\')[0].ToString();
                byte bytSubID = byte.Parse(strName.Split('-')[0]);
                byte bytDevID = byte.Parse(strName.Split('-')[1]);

                byte[] ArayRelay = new byte[] { bytSubID, bytDevID, (byte)(myintDeviceTpye / 256), (byte)(myintDeviceTpye % 256), (byte)MyActivePage,
                                                (byte)(myintDIndex / 256), (byte)(myintDIndex % 256)};
                CsConst.MyUPload2DownLists.Add(ArayRelay);
                CsConst.MyUpload2Down = Convert.ToByte(((ToolStripButton)sender).Tag.ToString());
                FrmDownloadShow Frm = new FrmDownloadShow();
                Frm.FormClosing += new FormClosingEventHandler(UpdateDisplayInformationAccordingly);
                Frm.ShowDialog();
            }
        }

        void UpdateDisplayInformationAccordingly(object sender, FormClosingEventArgs e)
        {
            ShowTempSensor();
        }

        private void frm4Temp_Shown(object sender, EventArgs e)
        {
            if (CsConst.MyEditMode == 0)
            {
                ShowTempSensor();
            }
            else if (CsConst.MyEditMode == 1) //在线模式
            {
                if (CsConst.myDownLoadMode == 0 || CsConst.myDownLoadMode == 1)
                {
                    MyActivePage = 1;
                    if (oSensor.MyRead2UpFlags[0] == false)
                    {
                        tslRead_Click(tslRead, null);
                    }
                    else
                    {
                        ShowTempSensor();
                    }
                    oSensor.MyRead2UpFlags[1] = true;
                }
            }
        }

        private void dgvTemp_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == -1 || e.RowIndex == -1) return;
            //DisplayCtrlEnableOrDisable(e.RowIndex);

            if (dgvTemp.SelectedRows.Count == 0) return;

            for (int i = 0; i < dgvTemp.SelectedRows.Count; i++)
            {
                dgvTemp.SelectedRows[i].Cells[e.ColumnIndex].Value = dgvTemp[e.ColumnIndex, e.RowIndex].Value.ToString();
                UpdateDeatailsSettings(dgvTemp.SelectedRows[i].Index);
            }
            oSensor.MyRead2UpFlags[1] = false;
        }

        private void dgvTemp_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            dgvTemp.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }
    }
}
