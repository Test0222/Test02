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
    public partial class frmHAI : Form
    {
        private HAI myHAI = null;
        private string myDevName = null;
        private int mintIDIndex = -1;
        private int mywdDeviceType = -1;
        private byte mySelectedID = 0;

        public frmHAI()
        {
            InitializeComponent();
        }

        public frmHAI(HAI myhai, string strName, int intDIndex, int wdDeviceType)
        {
            InitializeComponent();
            this.myHAI = myhai;
            this.myDevName = strName;
            this.mintIDIndex = intDIndex;
            this.mywdDeviceType = wdDeviceType;

            HDLSysPF.DisplayDeviceNameModeDescription(strName, mywdDeviceType, cboDevice, tbModel, tbDescription);

            this.Text = strName;
            tsl3.Text = strName;
        }

        private void tbDefault_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            myHAI.ReadDefaultInfo();
            Cursor.Current = Cursors.Default;
        }

        private void frmHAI_Load(object sender, EventArgs e)
        {
            InitialFormCtrlsTextOrItems();
        }

        void InitialFormCtrlsTextOrItems()
        {
        }

        void SetCtrlsVisbleWithDifferentDeviceType()
        {
            toolStrip1.Visible = (CsConst.MyEditMode == 0);
        }

        void DisplayHAIInformationToForm()
        {
            tvUnit.Nodes.Clear();
            tvScenes.Nodes.Clear();
            tvButtons.Nodes.Clear();

        }

        private void frmHAI_SizeChanged(object sender, EventArgs e)
        {
        }

        private void tbSave_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            myHAI.saveHaiInfoToDB(mintIDIndex);
            Cursor.Current = Cursors.Default;
        }

        private void tsbDown_Click(object sender, EventArgs e)
        {
            byte bytTag = Convert.ToByte(((ToolStripButton)sender).Tag.ToString());
            bool blnShowMsg = (CsConst.MyEditMode != 1);
            if (HDLPF.UploadOrReadGoOn(this.Text, bytTag, blnShowMsg))
            {
                Cursor.Current = Cursors.WaitCursor;
                // SetVisableForDownOrUpload(false);
                // ReadDownLoadThread();  //增加线程，使得当前窗体的任何操作不被限制

                CsConst.MyUPload2DownLists = new List<byte[]>();

                string strName = myDevName.Split('\\')[0].ToString();
                byte bytSubID = byte.Parse(strName.Split('-')[0]);
                byte bytDevID = byte.Parse(strName.Split('-')[1]);

                byte[] ArayRelay = new byte[] { bytSubID, bytDevID, (byte)(mywdDeviceType / 256), (byte)(mywdDeviceType % 256), 
                    (byte)0 ,(byte)(mintIDIndex/256),(byte)(mintIDIndex%256)};
                CsConst.MyUPload2DownLists.Add(ArayRelay);
                CsConst.MyUpload2Down = Convert.ToByte(((ToolStripButton)sender).Tag.ToString());
                FrmDownloadShow Frm = new FrmDownloadShow();
                if (CsConst.MyUpload2Down == 0) Frm.FormClosing += new FormClosingEventHandler(UpdateDisplayInformationAccordingly);
                Frm.ShowDialog();
            }
        }

        void UpdateDisplayInformationAccordingly(object sender, FormClosingEventArgs e)
        {
            DisplayHAIInformationToForm();
        }


        void SetVisableForDownOrUpload(bool blnIsEnable)
        {
            tsbDown.Enabled = blnIsEnable;
            tbUpload.Enabled = blnIsEnable;

            if (blnIsEnable == true)
            {
                tsbHint.Visible = true;
            }
            else
            {
                tsbar.Value = 0;
                tsbHint.Visible = false;
            }

            if (tsbar.Value == 100)
            {
                tsbHint.Text = "Fully Success!";
            }
            tsbar.Visible = !blnIsEnable;
            tsbl4.Visible = !blnIsEnable;
        }


        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (myHAI == null) return;

            byte bytTag = byte.Parse(((Button)sender).Tag.ToString());

            switch (bytTag)
            {
                case 1:
                    #region
                    if (tvUnit.Nodes.Count >= 192) return;
                    if (txtNum.Text == null || txtNum.Text == "") return;
                    int Num = Convert.ToInt32(txtNum.Text);
                    if (Num > 192) Num = 192;
                    if (myHAI.units == null) myHAI.units = new List<HAI.Unit>();

                    if (Num > 0)
                    {
                        for (int i = 1; i <= Num; i++)
                        {
                            // 获取一个未使用的序列号
                            #region
                            List<byte> TmpAreaID = new List<byte>(); //取出所有已用的区域号
                            for (int j = 0; j < myHAI.units.Count; j++) TmpAreaID.Add(myHAI.units[j].bytID);

                            //查找位置，替换buffer
                            byte bytAreaID = 1;
                            while (TmpAreaID.Contains(bytAreaID))
                            {
                                bytAreaID++;
                            }
                            #endregion

                            HAI.Unit temp = new HAI.Unit();
                            temp.bytID = bytAreaID;
                            temp.Command = "^A" + String.Format("{0:D3} ", bytAreaID);
                            temp.oUnit = new UVCMD.ControlTargets();

                            TreeNode node = tvUnit.Nodes.Add(temp.Command);
                            myHAI.units.Add(temp);
                        }
                    }
                    #endregion
                    break;
                case 2:
                    #region
                    if (tvScenes.Nodes.Count >= 254) return;
                    if (txtNum1.Text == null || txtNum1.Text == "") return;
                    Num = Convert.ToInt32(txtNum1.Text);
                    if (Num > 254) Num = 254;
                    if (myHAI.scen == null) myHAI.scen = new List<HAI.Scene>();

                    if (Num > 0)
                    {
                        for (int i = 1; i <= Num; i++)
                        {
                            // 获取一个未使用的序列号
                            #region
                            List<byte> TmpAreaID = new List<byte>(); //取出所有已用的区域号
                            for (int j = 0; j < myHAI.scen.Count; j++) TmpAreaID.Add(myHAI.scen[j].bytID);

                            //查找位置，替换buffer
                            byte bytAreaID = 1;
                            while (TmpAreaID.Contains(bytAreaID))
                            {
                                bytAreaID++;
                            }
                            #endregion

                            HAI.Scene temp = new HAI.Scene();
                            temp.bytID = bytAreaID;
                            temp.Command = "^C" + String.Format("{0:D3} ", bytAreaID);
                            temp.oUnit = new UVCMD.ControlTargets();

                            TreeNode node = tvScenes.Nodes.Add(temp.Command);
                            myHAI.scen.Add(temp);
                        }
                    }
                    #endregion
                    break;
                case 3:
                    #region
                    if (tvButtons.Nodes.Count >= 192) return;
                    if (txtNum2.Text == null || txtNum2.Text == "") return;
                    Num = Convert.ToInt32(txtNum1.Text);
                    if (Num > 192) Num = 192;
                    if (myHAI.buttonstatus == null) myHAI.buttonstatus = new List<HAI.ButtonStatus>();

                    if (Num > 0)
                    {
                        for (int i = 1; i <= Num; i++)
                        {
                            // 获取一个未使用的序列号
                            #region
                            List<byte> TmpAreaID = new List<byte>(); //取出所有已用的区域号
                            for (int j = 0; j < myHAI.buttonstatus.Count; j++) TmpAreaID.Add(myHAI.buttonstatus[j].bytID);

                            //查找位置，替换buffer
                            byte bytAreaID = 1;
                            while (TmpAreaID.Contains(bytAreaID))
                            {
                                bytAreaID++;
                            }
                            #endregion

                            HAI.ButtonStatus temp = new HAI.ButtonStatus();
                            temp.bytID = bytAreaID;
                            temp.Command = String.Format("{0:D3} ", bytAreaID) + " Button Tab/Press<-->OFF/ON";
                            temp.oUnit = new UVCMD.ControlTargets();

                            TreeNode node = tvButtons.Nodes.Add(temp.Command);
                            myHAI.buttonstatus.Add(temp);
                        }
                    }
                    #endregion
                    break;
            }
            myHAI.MyRead2UpFlags[1] = false;
        }

        private void tvUnit_MouseDown(object sender, MouseEventArgs e)
        {
            mySelectedID = byte.Parse(((TreeView)sender).Tag.ToString());

        }

        private void CmsCopy_Opening(object sender, CancelEventArgs e)
        {

        }


        void frmTmp_FormClosing(object sender, FormClosingEventArgs e)
        {
            // throw new NotImplementedException();
            DisplayHAIInformationToForm();
        }

        private void addCommandsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void frmHAI_Shown(object sender, EventArgs e)
        {
            SetCtrlsVisbleWithDifferentDeviceType();
            if (CsConst.MyEditMode == 0)
            {
                DisplayHAIInformationToForm();
            }
            else if (CsConst.MyEditMode == 1)
            {
                if (myHAI.MyRead2UpFlags[0] == false)
                {
                    tsbDown_Click(tsbDown, null);
                    myHAI.MyRead2UpFlags[0] = true;
                }
                else
                {
                    DisplayHAIInformationToForm();
                }
            }
        }

        private void frmHAI_FormClosing(object sender, FormClosingEventArgs e)
        {

        }
    }
}
