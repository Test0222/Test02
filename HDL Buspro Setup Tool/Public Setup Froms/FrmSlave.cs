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
    public partial class FrmSlave : Form
    {
        private Panel oDLP;
        private string strName;
        private int DeviceType;
        private byte SubNetID;
        private byte DeviceID;
        public FrmSlave()
        {
            InitializeComponent();
        }

        public FrmSlave(string strname,DLP panel,int devicetype)
        {
            InitializeComponent();
            this.strName = strname;
            this.oDLP = panel;
            this.DeviceType = devicetype;
            string strDevName = strName.Split('\\')[0].ToString();
            SubNetID = Convert.ToByte(strDevName.Split('-')[0]);
            DeviceID = Convert.ToByte(strDevName.Split('-')[1]);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (tabControl1.SelectedIndex == 0)
                {
                    byte[] arayTmp = new byte[4];
                    for (int i = 0; i < dgvSlave.Rows.Count; i++)
                    {
                        arayTmp[0] = Convert.ToByte(i + 1);
                        arayTmp[1] = Convert.ToByte(clS2.Items.IndexOf(dgvSlave[1, i].Value.ToString()));
                        arayTmp[2] = Convert.ToByte(HDLPF.IsNumStringMode(dgvSlave[2, i].Value.ToString(), 0, 255));
                        arayTmp[3] = Convert.ToByte(HDLPF.IsNumStringMode(dgvSlave[3, i].Value.ToString(), 0, 255));
                        if (CsConst.mySends.AddBufToSndList(arayTmp, 0xE0EA, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                        {
                            
                            HDLUDP.TimeBetwnNext(20);
                        }
                        else break;
                        
                    }
                }
                else if (tabControl1.SelectedIndex == 1)
                {
                    byte[] arayTmp = new byte[17];
                    if (chbEnable.Checked) arayTmp[0] = 1;
                    for (int i = 0; i < dgvSyn.Rows.Count; i++)
                    {
                        arayTmp[1 + 2 * i] = Convert.ToByte(HDLPF.IsNumStringMode(dgvSyn[1, i].Value.ToString(), 0, 255));
                        arayTmp[2 + 2 * i] = Convert.ToByte(HDLPF.IsNumStringMode(dgvSyn[2, i].Value.ToString(), 0, 255));
                        if (CsConst.mySends.AddBufToSndList(arayTmp, 0x1962, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                        {
                            
                        }
                        else break;
                    }
                }
            }
            catch
            {
            }
            Cursor.Current = Cursors.Default;
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            if (tabControl1.SelectedIndex == 0)
            {
                byte[] arayTmp = new byte[1];
                dgvSlave.Rows.Clear();
                for (int i = 1; i <= 8; i++)
                {
                    arayTmp[0] = Convert.ToByte(i);
                    if (CsConst.mySends.AddBufToSndList(arayTmp, 0xE0E8, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                    {
                        string strEnable = CsConst.mstrINIDefault.IniReadValue("Public", "99816", "");
                        if (CsConst.myRevBuf[27] == 1) strEnable = CsConst.mstrINIDefault.IniReadValue("Public", "99817", "");
                        object[] obj = new object[] { i.ToString(), strEnable, CsConst.myRevBuf[28].ToString(), CsConst.myRevBuf[29].ToString() };
                        dgvSlave.Rows.Add(obj);

                        HDLUDP.TimeBetwnNext(20);
                    }
                    else
                    {
                        Cursor.Current = Cursors.Default;
                        break;
                    }
                }
            }
            else if (tabControl1.SelectedIndex == 1)
            {
                dgvSyn.Rows.Clear();
                byte[] arayTmp = new byte[0];
                if (CsConst.mySends.AddBufToSndList(arayTmp, 0x1960, SubNetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                {
                    chbEnable.Checked = (CsConst.myRevBuf[25] == 1);
                    for (int i = 0; i < 8; i++)
                    {
                        object[] obj = new object[] { (i + 1).ToString(), CsConst.myRevBuf[26 + 2 * i], CsConst.myRevBuf[26 + 2 * i + 1] };
                        dgvSyn.Rows.Add(obj);
                    }
                    
                }
            }
            Cursor.Current = Cursors.Default;
        }

        private void FrmSlave_Load(object sender, EventArgs e)
        {
            clS2.Items.Clear();
            clS2.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99816", ""));
            clS2.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "99817", ""));
            tabControl1_SelectedIndexChanged(null, null);

        }

        private void dgvSlave_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if ((e.RowIndex == -1) || (e.ColumnIndex == -1)) return;
            if (dgvSlave[e.ColumnIndex, e.RowIndex].Value == null) dgvSlave[e.ColumnIndex, e.RowIndex].Value = "";

            for (int i = 0; i < dgvSlave.SelectedRows.Count; i++)
            {
                string strTmp = "";
                switch (e.ColumnIndex)
                {
                    case 2:
                        strTmp = dgvSlave[2, dgvSlave.SelectedRows[i].Index].Value.ToString();
                        dgvSlave[2, dgvSlave.SelectedRows[i].Index].Value = HDLPF.IsNumStringMode(strTmp, 0, 255);
                        break;
                    case 3:
                        strTmp = dgvSlave[3, dgvSlave.SelectedRows[i].Index].Value.ToString();
                        dgvSlave[3, dgvSlave.SelectedRows[i].Index].Value = HDLPF.IsNumStringMode(strTmp, 0, 255);
                        break;
                }
                dgvSlave.SelectedRows[i].Cells[e.ColumnIndex].Value = dgvSlave[e.ColumnIndex, e.RowIndex].Value.ToString();
            }
            if (e.ColumnIndex == 2)
            {
                string strTmp = dgvSlave[2, e.RowIndex].Value.ToString();
                dgvSlave[2, e.RowIndex].Value = HDLPF.IsNumStringMode(strTmp, 0, 255);
            }
            else if (e.ColumnIndex == 3)
            {
                string strTmp = dgvSlave[3, e.RowIndex].Value.ToString();
                dgvSlave[3, e.RowIndex].Value = HDLPF.IsNumStringMode(strTmp, 0, 255);
            }
        }

        private void dgvSlave_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            dgvSlave.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void dgvSyn_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if ((e.RowIndex == -1) || (e.ColumnIndex == -1)) return;
            if (dgvSlave[e.ColumnIndex, e.RowIndex].Value == null) dgvSlave[e.ColumnIndex, e.RowIndex].Value = "";

            for (int i = 0; i < dgvSlave.SelectedRows.Count; i++)
            {
                string strTmp = "";
                switch (e.ColumnIndex)
                {
                    case 1:
                        strTmp = dgvSlave[2, dgvSlave.SelectedRows[i].Index].Value.ToString();
                        dgvSlave[2, dgvSlave.SelectedRows[i].Index].Value = HDLPF.IsNumStringMode(strTmp, 0, 255);
                        break;
                    case 2:
                        strTmp = dgvSlave[3, dgvSlave.SelectedRows[i].Index].Value.ToString();
                        dgvSlave[3, dgvSlave.SelectedRows[i].Index].Value = HDLPF.IsNumStringMode(strTmp, 0, 255);
                        break;
                }
                dgvSlave.SelectedRows[i].Cells[e.ColumnIndex].Value = dgvSlave[e.ColumnIndex, e.RowIndex].Value.ToString();
            }
            if (e.ColumnIndex == 1)
            {
                string strTmp = dgvSlave[2, e.RowIndex].Value.ToString();
                dgvSlave[2, e.RowIndex].Value = HDLPF.IsNumStringMode(strTmp, 0, 255);
            }
            else if (e.ColumnIndex == 2)
            {
                string strTmp = dgvSlave[3, e.RowIndex].Value.ToString();
                dgvSlave[3, e.RowIndex].Value = HDLPF.IsNumStringMode(strTmp, 0, 255);
            }
        }

        private void dgvSyn_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            dgvSyn.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }
    }
}
