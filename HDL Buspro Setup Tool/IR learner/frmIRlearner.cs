using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Data.OleDb;
using System.Windows.Forms;

namespace HDL_Buspro_Setup_Tool
{
    public partial class frmIRlearner : Form
    {
        UsbHidFind myUsb = new UsbHidFind();
        Thread readThread;//读设备线程
        string[] vendorID = new string[128];
        string[] productID = new string[128];
        string[] devicePath = new string[128];
        string[] outputID = new string[128];
        string[] outputLength = new string[128];

        int mintNowStatus = -1;
        int mintNeedSum = -1;
        int mintSendLine = -1;
        int mintAllLines = -1;

        byte[] writeValueBuffer; //存放要发送的数据的缓存

       // private SendIR tempSend = new SendIR();  //用于存储公共的红外码

        ////private static int desphAddress = 0;
        ////private static int desgrAddress = 0;

        public frmIRlearner()
        {
            InitializeComponent();
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            cboDevicePath.Items.Clear();

            //btnRead.Enabled = false;
            //btnTest.Enabled = false;
            string hidGuid = "";
            int deviceCount = 0;

            bool result = myUsb.FindHidDevice(ref hidGuid, ref vendorID, ref productID, ref devicePath, ref outputID, ref outputLength, ref deviceCount);
            if (result)
            {
                for (int i = 0; i <= deviceCount; i++)
                {
                    if (vendorID[i].ToUpper() == "1FC9" && productID[i].ToString() == "3")
                    {
                        cboDevicePath.Items.Add("HDL-IR-Learner-" + (i + 1).ToString());
                    }
                }
                if (cboDevicePath.Items != null && cboDevicePath.Items.Count !=0) cboDevicePath.SelectedIndex =0;
            }
            else
            {
                //  lstShowFindInfo.Items.Add("没有发现连接的HidUSB设备");
            }
        }

        private void btnReady_Click(object sender, EventArgs e)
        {
            if (readThread.ThreadState == ThreadState.Unstarted)
            {
                readThread.Start();
            }
            writeValueBuffer = new byte[3];
            writeValueBuffer[0] = 1;
            writeValueBuffer[1] = 0xF2;
            writeValueBuffer[2] = 00;
            mintNowStatus = 0;

            WritetxtData();
        }

        private void WritetxtData()
        {
            if (writeValueBuffer == null) return;
            byte[] nowWriteData = null;

            string strTmp = cboDevicePath.Text;
            if (strTmp == null || strTmp == "") return;
            int intID = int.Parse(strTmp.Substring(15,1).ToString());
            if (myUsb.WriteHidFile(devicePath[intID-1], 64, writeValueBuffer, ref nowWriteData))
            {
               
            }
        }

        private void ReadData()
        {
            try
            {
                while (true)
                {
                    byte[] readValue = new byte[64];
                    int readLength = 0;

                    string strTmp = cboDevicePath.Text;
                    if (strTmp == null || strTmp == "") break;
                    int intID = int.Parse(strTmp.Substring(15,1).ToString());

                    if (myUsb.ReadHidFile(devicePath[intID - 1], ref readValue, ref readLength))
                    {
                        switch (mintNowStatus)
                        {
                            case 0:
                                if (readValue[1] == 0x84 && readValue[2] == 0x01)
                                {
                                    PicStatus.Image = HDL_Buspro_Setup_Tool.Properties.Resources.OK;
                                    btnRead.Enabled = true;
                                }
                                else
                                    PicStatus.Image = HDL_Buspro_Setup_Tool.Properties.Resources.OK;
                                break;  //进入准备状态

                            case 1:
                                if (readValue[1] == 0x84 && readValue[2] == 0x01)
                                {
                                    mintSendLine = rtbText.Lines.Length - 1;

                                    writeValueBuffer = new byte[64];
                                    writeValueBuffer[0] = 1;
                                    writeValueBuffer[1] = 0xF2;
                                    writeValueBuffer[2] = 05;

                                    mintNowStatus = 5;

                                    string strIR = rtbText.Lines[0].ToString();
                                    string[] ArayStr = strIR.Split(' ');

                                    for (int i = 0; i < 15; i++)
                                    {
                                        if (ArayStr[i] != "")
                                        {
                                            writeValueBuffer[3 + i] = Convert.ToByte(ArayStr[i + 1], 16);
                                        }
                                    }
                                    WritetxtData();
                                }
                                break;
                            case 5:
                                if (readValue[1] == 0x84 && readValue[2] == 0x01)
                                {
                                    writeValueBuffer = new byte[64];
                                    writeValueBuffer[0] = 1;
                                    writeValueBuffer[1] = 0xF2;
                                    writeValueBuffer[2] = 05;

                                    mintNowStatus = 5;
                                    if (mintSendLine != 0)
                                    {
                                        string strIR = rtbText.Lines[mintAllLines - mintSendLine].ToString();
                                        string[] ArayStr = strIR.Split(' ');

                                        for (int i = 0; i < 15; i++)
                                        {
                                            if (ArayStr[i] != "")
                                            {
                                                writeValueBuffer[3 + i] = Convert.ToByte(ArayStr[i + 1], 16);
                                            }
                                        }
                                        WritetxtData();
                                        mintSendLine--;
                                    }
                                    else
                                    {
                                        Cursor.Current = Cursors.Default;
                                    }
                                }
                                break;
                            case 2:
                                if (readValue[1] == 0x84 && readValue[2] == 0x01)
                                    tbStatus.Text = "Success in Sending";
                                else
                                    tbStatus.Text = "Failure in Sending";
                                break;  //进入准备状态

                            case 3:
                                strTmp = "(1):";
                                for (int intI = 0; intI < 16; intI++) strTmp = strTmp + readValue[intI + 1].ToString("X2") + " ";
                                rtxCodes.Text = strTmp + '\n';

                                mintNeedSum = (readValue[3] * 256 + readValue[4]) - 1;

                                writeValueBuffer = new byte[3];
                                writeValueBuffer[0] = 1;
                                writeValueBuffer[1] = 0xF2;
                                writeValueBuffer[2] = 04;

                                mintNowStatus = 4;

                                WritetxtData();
                                break;

                            case 4:
                                mintNeedSum = mintNeedSum - 14;
                                strTmp = "(" + (readValue[2] + 1).ToString() + "):";
                                for (int intI = 0; intI < 16; intI++) strTmp = strTmp + readValue[intI + 1].ToString("X2") + " ";
                                rtxCodes.Text += strTmp + '\n';

                                if (mintNeedSum > 0)
                                {
                                    writeValueBuffer = new byte[3];
                                    writeValueBuffer[0] = 1;
                                    writeValueBuffer[1] = 0xF2;
                                    writeValueBuffer[2] = 04;

                                    mintNowStatus = 4;

                                    WritetxtData();
                                    PicStatus.Image = HDL_Buspro_Setup_Tool.Properties.Resources.OK;
                                }
                                btnTest.Enabled = true;
                                break;
                        }
                    }
                }
            }
            catch
            {
            }
        }

        private void frmIRlearner_Load(object sender, EventArgs e)
        {          
            #region
            if (tvExisted.Nodes != null && tvExisted.Nodes.Count != 0)
            {
                cboDevice.Items.Clear();
                foreach (TreeNode oNode in tvExisted.Nodes)
                {
                    cboDevice.Items.Add(oNode.Text.ToString());
                }
            }
            #endregion

            //使得线程可以访问Form控件
            System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = false;
            readThread = new Thread(ReadData);
            this.tvExisted.DrawNode += new DrawTreeNodeEventHandler(treeView_DrawNode);
            this.tvEdit.DrawNode += new DrawTreeNodeEventHandler(treeView_DrawNode);
            rtxCodes_TextChanged(null, null);
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

        private void btnRead_Click(object sender, EventArgs e)
        {
            if (readThread.ThreadState == ThreadState.Unstarted)
            {
                readThread.Start();
            }
            writeValueBuffer = new byte[3];
            writeValueBuffer[0] = 1;
            writeValueBuffer[1] = 0xF2;
            writeValueBuffer[2] = 03;

            mintNowStatus = 3;

            WritetxtData();
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            if (readThread.ThreadState == ThreadState.Unstarted)
            {
                readThread.Start();
            }
            writeValueBuffer = new byte[3];
            writeValueBuffer[0] = 1;
            writeValueBuffer[1] = 0xF2;
            writeValueBuffer[2] = 02;

            mintNowStatus = 2;

            WritetxtData();
        }

        private void tvExisted_MouseDown(object sender, MouseEventArgs e)
        {
            TreeNode oNode = tvExisted.GetNodeAt(e.X, e.Y);
            if (oNode == null) return;
            if (oNode.Text == null || oNode.Text == "") return;
            int intLevel = oNode.Level; // get Room or function ID
            if (intLevel == 1)
            {
                txtGrAddress.Text = oNode.Text;
                oNode = oNode.Parent;
            }

            cboDevice.SelectedIndex = cboDevice.Items.IndexOf(oNode.Text);
        }

        private void frmIRlearner_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Dispose();
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            if (tvEdit.SelectedNode == null) return;
            TreeNode oTmp = tvEdit.SelectedNode;
            if (oTmp.Level == 0 && (oTmp.Nodes == null || oTmp.Nodes.Count == 0))  // 如果是设备 而且本身没有任何红外码 可以直接移除
            {
                oTmp.Remove();
                // 删除当前全部类
                string strsql = string.Format("Delete from tblRemoteDevice where ID = {0}", oTmp.Name);
                DataModule.ExecuteSQLDatabase(strsql);
            }
            else if (oTmp.Level == 1)  // 如果设备中的其中一个红外码 可以直接删除
            {
                string strDevice = oTmp.Parent.Text;
                string strOldRemark = oTmp.Text;

                string strInsert = string.Format("Delete from tblRemoteCode where ID = {0} and RemoteDeviceID = {1} and Remark = '{2}'", oTmp.Name, oTmp.Parent.Name, strOldRemark);
                DataModule.ExecuteSQLDatabase(strInsert);

                oTmp.Remove();
            }
        }

        private void btnDelAll_Click(object sender, EventArgs e)
        {
            if (tvEdit.SelectedNode == null) return;
            TreeNode oTmp = tvEdit.SelectedNode;
            if (oTmp.Level != 0) return;

            if (oTmp.Level == 0)  // 如果是设备 而且本身没有任何红外码 可以直接移除并移除其全部子节点
            {

                string strsql = string.Format("Delete from tblRemoteDevice where ID = {0}", oTmp.Name);
                DataModule.ExecuteSQLDatabase(strsql);

                //更新表格
                string strInsert = string.Format("Delete from tblRemoteCode where RemoteDeviceID = {0}", oTmp.Name);
                DataModule.ExecuteSQLDatabase(strInsert);
                oTmp.Remove();
            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            // 检测是不是重复备注 如果重复提示替换 默认为新增
            if (cboDevice.Text == null || cboDevice.Text == "") return;
            if (txtGrAddress.Text == null || cboDevice.Text == "") return;
            if (rtxCodes.Text == null || rtxCodes.Lines.Length ==0 ) return;
            int IRLegth = 0;
            string str0 = rtxCodes.Lines[0].ToString().Split(':')[1].Trim();
            string[] strlist0 = str0.Split(' ');
            IRLegth = Convert.ToInt32(strlist0[2], 16) * 256 + Convert.ToInt32(strlist0[3], 16);
            int strLength = 0;
            strLength = (rtxCodes.Lines.Length * 16) - (2 * rtxCodes.Lines.Length);
            if (strLength < IRLegth)
            {
                if (MessageBox.Show(CsConst.mstrINIDefault.IniReadValue("public", "99583", ""), "", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.OK)
                {
                    return;
                }
                else return;
            }
            Cursor.Current = Cursors.WaitCursor;
            string strDevice = cboDevice.Text;
            string strRemark = txtGrAddress.Text;
            bool blnIsExistedDev = false;
            TreeNode oNode = null; //临时存放节点 用于取其ID

            // 判断是否存在当前设备类型 是否存在当前红外码
            UVCMD.IRCode TmpIR = null;
            int intIRID = 1;
            List<int> TmpIRIDs = new List<int>(); //取出所有已用的红外码号
            #region
            if (tvExisted.Nodes != null && tvExisted.Nodes.Count != 0)
            {
                for (int intI = 0; intI < tvExisted.Nodes.Count; intI++)
                { 
                    blnIsExistedDev = strDevice.Equals(tvExisted.Nodes[intI].Text.ToString());

                    if (blnIsExistedDev == true)
                    {
                        oNode = tvExisted.Nodes[intI];
                        if (tvExisted.Nodes[intI].Nodes != null && tvExisted.Nodes[intI].Nodes.Count != 0)
                        {
                            foreach (TreeNode NodeTmp in tvExisted.Nodes[intI].Nodes)
                            {
                                TmpIRIDs.Add(Convert.ToInt32(NodeTmp.Name));
                                if (strRemark.Equals(NodeTmp.Text.ToString()))
                                {
                                    break;
                                }
                            }
                        }
                        break;
                    }
                }
            }

            #endregion

            //新红外码

            #region
            if (TmpIRIDs != null)
            {
                //查找位置，替换buffer
                while (TmpIRIDs.Contains(intIRID))
                {
                    intIRID++;
                }
            }
            #endregion

            TmpIR = new UVCMD.IRCode();
            TmpIR.KeyID = intIRID;
            TmpIR.Remark1 = strDevice;
            TmpIR.Remark2 = strRemark;
            TmpIR.IRLength = rtxCodes.Lines.Length - 1;
            TmpIR.Codes = "";
            for (int intI = 0; intI < rtxCodes.Lines.Length; intI++)
            {
                if (rtxCodes.Lines[intI] != "" && rtxCodes.Lines[intI] != null) TmpIR.Codes += rtxCodes.Lines[intI].ToString().Split(':')[1].Trim() + ";";
            }
            if (TmpIR.Codes.Length > 0)
            {
                TmpIR.Codes = TmpIR.Codes.Substring(0, TmpIR.Codes.Length - 1);
            }

            Cursor.Current = Cursors.Default;
        }

        private void tvEdit_MouseDown(object sender, MouseEventArgs e)
        {
            TreeNode oNode = tvEdit.GetNodeAt(e.X, e.Y);
            if (oNode == null) return;
            if (oNode.Text == null || oNode.Text == "") return;
            tbRemark1.Text = oNode.Text;
            btnDelAll.Enabled = (oNode.Level == 0);
           
        }

        private void btnReName_Click(object sender, EventArgs e)
        {
            if (tvEdit.SelectedNode == null) return;
            if (tvEdit.SelectedNode.Level == 0) return;
            if (tbRemark1.Text == null || tbRemark1.Text == "") return;

            string strDevice = tvEdit.SelectedNode.Parent.Text;
            string strOldRemark = tvEdit.SelectedNode.Text;
            string strNewRemark = tbRemark1.Text;

            //更新到数据库

            string strsql = string.Format("update tblRemoteCode set Remark='{0}' where ID={1} and RemoteDeviceID = {2} and Remark='{3}'", strNewRemark,
                                           tvEdit.SelectedNode.Name,tvEdit.SelectedNode.Parent.Name, strOldRemark);
            DataModule.ExecuteSQLDatabase(strsql);
        }

        private void Upload_Click(object sender, EventArgs e)
        {
            if (readThread.ThreadState == ThreadState.Unstarted)
            {
                readThread.Start();
            }
            // 检测是不是重复备注 如果重复提示替换 默认为新增
            if (cboDevicePath.SelectedIndex == -1) return;
            if (rtbText.Text == null || rtbText.Lines.Length == 0) return;

            Cursor.Current = Cursors.WaitCursor;
            writeValueBuffer = new byte[3];
            writeValueBuffer[0] = 1;
            writeValueBuffer[1] = 0xF2;
            writeValueBuffer[2] = 01;

            mintNowStatus = 1;
            mintAllLines = rtbText.Lines.Length - 1;
            WritetxtData();
        }

        private void tabIR_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnBacks_Click(object sender, EventArgs e)
        {
            string strFileName = HDLPF.SaveFileDialog("IR Database");
            if (strFileName == null) return; 
        }

        private void btnRestore_Click(object sender, EventArgs e)
        {
            // open file 
            string strFileName = HDLPF.OpenFileDialog("mdb files (*.mdb)|*.mdb","IR Database");
            if (strFileName == null) return;

            string strPrj = "select * from tblRemoteDevice order by ID";

            List<int> ArayID = new List<int>();
            OleDbCommand cmd = new OleDbCommand();
            //创建一个OleDbConnection对象

            #region
            OleDbConnection conn = null;
            conn = new OleDbConnection(DataModule.ConString + strFileName);
            try
            {
                //cmd属性赋值
                cmd.Connection = conn;
                cmd.CommandText = strPrj;
                if (conn.State == ConnectionState.Closed) conn.Open();

                OleDbDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                #region
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        string strRemark = reader.GetString(1);
                        string strsql = string.Format("select * from tblRemoteDevice where Remark= '{0}'",strRemark);
                        if (DataModule.IsExitstInDatabase(strsql) == false)
                        {
                            ArayID.Add(reader.GetInt16(0));
                            strsql = string.Format("Insert into tblRemoteDevice(ID,Remark) values({0},'{1}')", reader.GetInt16(0), strRemark);
                            DataModule.ExecuteSQLDatabase(strsql);
                        }
                    }
                }
                #endregion
            }
            catch (Exception)
            {
                conn.Close();
            }
            finally
            {
                 conn.Close();
            }
            #endregion

            #region
            for (int intI = 0; intI < ArayID.Count; intI++)
            {
                strPrj = "select * from tblRemoteCode where RemoteDeviceID=" + ArayID[intI] + " order by ID";
                try
                {
                    //cmd属性赋值
                    cmd.Connection = conn;
                    cmd.CommandText = strPrj;
                    if (conn.State == ConnectionState.Closed) conn.Open();

                    OleDbDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                    #region
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            string strSql = string.Format("insert into tblRemoteCode(ID,RemoteDeviceID,Remark,Code,QtyPack) values ({0},{1},'{2}','{3}',{4})",
                                    reader.GetInt16(0), reader.GetInt16(1), reader.GetString(2), reader.GetString(3), reader.GetInt16(4));
                            DataModule.ExecuteSQLDatabase(strSql);

                        }
                    }
                    #endregion
                }
                catch (Exception)
                {
                    conn.Close();
                }
                finally
                {
                    conn.Close();
                }
            }
            #endregion
        }

        private void groupBox4_Enter(object sender, EventArgs e)
        {

        }

        private void cboDevicePath_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void rtxCodes_TextChanged(object sender, EventArgs e)
        {
            if (rtxCodes.Text.Trim() == "") btnNew.Enabled = false;
            else btnNew.Enabled = true;
        }
    }
}
