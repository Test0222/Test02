using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace HDL_Buspro_Setup_Tool
{
    public partial class FrmBackup : Form
    {
        string strPath = "";
        public FrmBackup()
        {
            InitializeComponent();
            LoadControlsText.DisplayTextToFormWhenFirstShow(this);
        }

        public FrmBackup(string strpath)
        {
            InitializeComponent();
            this.strPath = strpath;
            CsConst.mstrCurPath = strPath;
        }

        private void txtFrm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar) || e.KeyChar == 8)
                e.Handled = false;
            else
                e.Handled = true;
        }

        private void chbOption_CheckedChanged(object sender, EventArgs e)
        {
            if (dgOnline.Rows.Count == 0) return;

            for (int intI = 0; intI < dgOnline.Rows.Count; intI++)
            {
                dgOnline[0, intI].Value = chbOption.Checked;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgOnline.Rows.Count == 0) return;
            List<int> selctList = new List<int>();
            for (int i = dgOnline.RowCount - 1; i >= 0; i--)
            {
                if (dgOnline[0, i].Value.ToString().ToLower() == "true")
                {
                    selctList.Add(i);
                }
            }

            foreach (int i in selctList)
            {
                dgOnline.Rows.RemoveAt(i);
            }
        }

        private void btnUpdateA_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (dgOnline.RowCount <= 0) return;
                if (CsConst.myOnlines.Count <= 0) return;
                for (int i = 0; i < dgOnline.RowCount; i++)
                {
                    if (dgOnline[0, i].Value.ToString().ToUpper() == "TRUE")
                    {
                        break;
                    }
                }
                string strFileName = HDLPF.SaveFileDialogAccordingly("Project Name", "dat files (*.dat)|*.dat");
                if (strFileName == null) return;
                CsConst.isRestore = true;
                cl10.Visible = true;
                this.Enabled = false;
                CsConst.myBackupLists = new List<HdlDeviceBackupAndRestore>();
                for (int i = 0; i < dgOnline.RowCount; i++)
                {
                    if (dgOnline[0, i].Value.ToString().ToUpper() == "TRUE")
                    {
                        CsConst.isBackUpSucceed = false;
                        
                        dgOnline[10, i].Value = CsConst.WholeTextsList[2577].sDisplayName;
                        int intTag = 0;
                        intTag = Convert.ToInt32(dgOnline[9, i].Value.ToString());
                        for (int j = 0; j < CsConst.myOnlines.Count; j++)
                        {
                            if (intTag == CsConst.myOnlines[j].intDIndex)
                            {
                                intTag = j;
                                break;
                            }
                        }
                        
                        int wdDeviceType = CsConst.myOnlines[intTag].DeviceType;
                        int dIndex = CsConst.myOnlines[intTag].intDIndex;
                        string strName = CsConst.myOnlines[intTag].DevName;
                        byte SubNetID = CsConst.myOnlines[intTag].bytSub;
                        byte DevID = CsConst.myOnlines[intTag].bytDev;

                        CsConst.MyUPload2DownLists = new List<byte[]>();
                        byte[] ArayRelay = new byte[] { SubNetID, DevID, (byte)(wdDeviceType / 256), (byte)(wdDeviceType % 256), 0,
                        (byte)(dIndex/256),(byte)(dIndex%256)};
                        CsConst.MyUPload2DownLists.Add(ArayRelay);
                        CsConst.MyUpload2Down = 0;

                        FrmDownloadShow Frm = new FrmDownloadShow();
                        Frm.ShowDialog();
                        Cursor.Current = Cursors.WaitCursor;
                        if (CsConst.isBackUpSucceed)
                        {
                            dgOnline[10, i].Value = CsConst.WholeTextsList[2576].sDisplayName;
                        }
                        else
                        {
                            dgOnline[10, i].Value = CsConst.WholeTextsList[2578].sDisplayName;
                        }
                        Cursor.Current = Cursors.Default;
                        dgOnline[0, i].Value = false;
                    }
                }
                if (CsConst.myBackupLists != null && CsConst.myBackupLists.Count != 0)
                {
                    FileStream fs = new FileStream(strFileName, FileMode.Create);
                    BinaryFormatter bf = new BinaryFormatter();
                    bf.Serialize(fs, CsConst.myBackupLists);
                    fs.Close();

                }    
            }
            catch ( Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            Cursor.Current = Cursors.Default;
            this.Enabled = true;
            CsConst.isRestore = false;
        }

        private void FrmRestrore_Load(object sender, EventArgs e)
        {
            try
            {
                HDLSysPF.PasteCMDToPublicBufferWaitPasteOrCopyToGrid(dgOnline);
            }
            catch
            {
            }
            HDLSysPF.setDataGridViewColumnsWidth(dgOnline);
        }

        private void addcontrols(int col, int row, Control con)
        {
            con.Visible = true;
            con.Show();
            Rectangle rect = dgOnline.GetCellDisplayRectangle(col, row, true);
            con.Size = rect.Size;
            con.Top = rect.Top;
            con.Left = rect.Left;
        }

        private void FrmRestrore_FormClosing(object sender, FormClosingEventArgs e)
        {
        }

        private void btnModify_Click(object sender, EventArgs e)
        {
            string strFileName = HDLPF.OpenFileDialog("mdb files (*.mdb)|*.mdb", "mdb Files");
            if (strFileName == null) return;
            this.strPath = strFileName;
            CsConst.mstrCurPath = strPath;
           // lbPathValue.Text = strFileName;
            dgOnline.Rows.Clear();
            try
            {

                string strsql = "select * from dbBasicInfo";
                OleDbDataReader dr = DataModule.SearchAResultSQLDB(strsql, strPath);

                if (dr != null)
                {
                    while (dr.Read())
                    {
                        string strname = dr.GetValue(1).ToString();
                        int DeviceType = Convert.ToInt32(dr.GetValue(2).ToString());
                        object[] obj = new object[] { false,
                                                      strname.Split('\\')[0].ToString().Split('-')[0].ToString(),
                                                      strname.Split('\\')[0].ToString().Split('-')[1].ToString(),
                                                      strname.Split('\\')[1].ToString(),
                                                      CsConst.mstrINIDefault.IniReadValue("DeviceType" + DeviceType.ToString(), "Description", ""),
                                                      CsConst.mstrINIDefault.IniReadValue("DeviceType" + DeviceType.ToString(), "Model", ""),
                                                      CsConst.mstrINIDefault.IniReadValue("Public", "99933", ""),
                                                      dr.GetValue(0).ToString(),DeviceType.ToString()};
                        dgOnline.Rows.Add(obj);
                    }
                    dr.Close();
                }
            }
            catch
            {
            }
            
        }

        private void dgOnline_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            dgOnline.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

    }
}
