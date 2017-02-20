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
    public partial class frmSecurity : Form
    {
        private Security mySecurity = null;
        private string mystrName = string.Empty;
        private int mintIDIndex = -1;
        private int mywdDevicerType = 0;

        private TreeNode MyPasteNode = null;

        private int MyActivePage = 0; //按页面上传下载

        public frmSecurity()
        {
            InitializeComponent();
        }

        public frmSecurity(Security oTmp, string strName, int intIndex, int intDeviceType)
        {
            InitializeComponent();

            this.mySecurity = oTmp;
            this.mystrName = strName;
            this.mintIDIndex = intIndex;
            this.mywdDevicerType = intDeviceType;

            string strDevName = strName.Split('\\')[0].ToString();

            HDLSysPF.DisplayDeviceNameModeDescription(strName, mywdDevicerType, cboDevice, tbModel, tbDescription);
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

                string strName = mystrName.Split('\\')[0].ToString();
                byte bytSubID = byte.Parse(strName.Split('-')[0]);
                byte bytDevID = byte.Parse(strName.Split('-')[1]);

                byte[] ArayRelay = new byte[] { bytSubID, bytDevID, (byte)(mywdDevicerType / 256), (byte)(mywdDevicerType % 256)
                    , (byte)MyActivePage,(byte)(mintIDIndex/256),(byte)(mintIDIndex%256)  };
                CsConst.MyUPload2DownLists.Add(ArayRelay);
                CsConst.MyUpload2Down = Convert.ToByte(((ToolStripButton)sender).Tag.ToString());
                FrmDownloadShow Frm = new FrmDownloadShow();
                Frm.FormClosing += new FormClosingEventHandler(UpdateDisplayInformationAccordingly);
                Frm.ShowDialog();
            }
        }

        void UpdateDisplayInformationAccordingly(object sender, FormClosingEventArgs e)
        {
            switch (tabControl1.SelectedIndex)
            {
                case 0: 
                case 1:
                    DisplayGetReadythings(); break;
                case 2: DisplayArmCMD(); break;
                case 3: DisplayAutoArmDisarm(); break;
                case 4: DisplayAlarmSetups(); break;
                case 5: DisplayVacationsSetup(); break;
            }
        }

        void DisplayArmCMD()
        {
            if (mySecurity == null) return;
            if (mySecurity.myArms8 == null) return;
            if (mySecurity.myArms8[0] == null || mySecurity.myArms8[0].ArmCmds == null) return;
            if (mySecurity.myArms8[0].ArmCmds.Length == 0) return;

            foreach (TreeNode Temp in tvArm1.Nodes)
            {
                Temp.Nodes.Clear();
            }

            for (int i = 0; i < mySecurity.myArms8[0].ArmCmds.Length; i++)
            {
                if (mySecurity.myArms8[0].ArmCmds[i] != null && mySecurity.myArms8[0].ArmCmds[i].Count != 0)
                {
                    foreach (UVCMD.ControlTargets oTmp in mySecurity.myArms8[0].ArmCmds[i])
                    {
                    }
                }
            }

            if (mySecurity.myReadyThings8[0] == null || mySecurity.myReadyThings8[0].ArmModules == null) return;
            if (mySecurity.myReadyThings8[0].ArmModules.Length == 0) return;

            lvArmModule.Items.Clear();
            for (byte bytI = 0; bytI < mySecurity.myReadyThings8[0].ArmModules[0]; bytI++)
            {
                ListViewItem tmp = new ListViewItem();
                tmp.SubItems.Add((lvArmModule.Items.Count + 1).ToString());
                byte bytSubID = mySecurity.myReadyThings8[0].ArmModules[1 + bytI * 10];
                byte bytDevID = mySecurity.myReadyThings8[0].ArmModules[2 + bytI * 10];

                lvArmModule.Items.Add(tmp);
            }
        }

        void DisplayGetReadythings()
        {
            if (mySecurity == null) return;
            if (mySecurity.myReadyThings8 == null) return;
            if (mySecurity.myReadyThings8[0] == null || mySecurity.myReadyThings8[0].LeaveCmds == null) return;
            if (mySecurity.myReadyThings8[0].LeaveCmds.Length == 0) return;

            foreach (TreeNode Temp in tvArm.Nodes)
            {
                Temp.Nodes.Clear();
            }

            for (int i = 0; i < mySecurity.myReadyThings8[0].LeaveCmds.Length; i++)
            {
                if (mySecurity.myReadyThings8[0].LeaveCmds[i] != null && mySecurity.myReadyThings8[0].LeaveCmds.Length != 0)
                {
                    foreach (UVCMD.ControlTargets oTmp in mySecurity.myReadyThings8[0].LeaveCmds[i])
                    {
                    }
                }
            }

            if (mySecurity.myReadyThings8[0].bytExitDelay < 10 || mySecurity.myReadyThings8[0].bytExitDelay > 180)
            {
                numExit.Value = 10;
            }
            else
            {
                numExit.Value = mySecurity.myReadyThings8[0].bytExitDelay;
            }
            chbMore.Checked = (mySecurity.myReadyThings8[0].bytMore == 1);

            tvBypass.Nodes.Clear();
            for (byte bytI = 0; bytI < mySecurity.myReadyThings8[0].DetectDoors[0]; bytI++)
            {
                byte bytSubID = mySecurity.myReadyThings8[0].DetectDoors[2 + bytI * 10];
                byte bytDevID = mySecurity.myReadyThings8[0].DetectDoors[3 + bytI * 10];
            }
        }

        void DisplayAutoArmDisarm()
        {
            if (mySecurity == null) return;
            if (mySecurity.myArms8[0] == null) return;
            if (mySecurity.myArms8[0].AutoArmDisarm == null) return;



            for (int i = 0; i <=5; i++)
            {
                (this.Controls.Find("chbAA" + (i + 1).ToString(), true)[0] as CheckBox).Checked = (mySecurity.myArms8[0].AutoArmDisarm[0 + i * 10] == 1);
                (this.Controls.Find("dtTime" + (i + 1).ToString(), true)[0] as DateTimePicker).Text = mySecurity.myArms8[0].AutoArmDisarm[1 + i * 10] + ":"
                                                                                              + mySecurity.myArms8[0].AutoArmDisarm[2 + i * 10] + ":"
                                                                                              + mySecurity.myArms8[0].AutoArmDisarm[3 + i * 10];
                if (i < 3)
                {
                    for (int j = 1; j <= 4; j++)
                    {
                        (this.Controls.Find("rbtnA" + (i + 1).ToString() + j.ToString(), true)[0] as RadioButton).Checked = (mySecurity.myArms8[0].AutoArmDisarm[4 + i * 10] == j - 1);
                    }
                }
                else
                {
                    for (int j = 1; j <= 4; j++)
                    {
                        (this.Controls.Find("chbA" + (i + 1).ToString(), true)[0] as CheckedListBox).SetItemChecked(j - 1,(mySecurity.myArms8[0].AutoArmDisarm[4 + i * 10] & (1 << (j -1))) == (1 << (j-1)));
                    }
                }
            }
        }

        void DisplayAlarmSetups()
        {
            if (mySecurity == null) return;
            if (mySecurity.myAlarms8 == null) return;
            if (mySecurity.myAlarms8[0] == null) return;
            if (mySecurity.myAlarms8[0].AlarmCmds == null || mySecurity.myAlarms8[0].AlarmCmds.Length == 0) return;

            foreach (TreeNode Temp in tvAlarm.Nodes)
            {
                Temp.Nodes.Clear();
            }

            if (Convert.ToByte(numEntry.Minimum) <= mySecurity.myAlarms8[0].bytEnterDelay && mySecurity.myAlarms8[0].bytEnterDelay <= Convert.ToByte(numEntry.Maximum))
                numEntry.Value = mySecurity.myAlarms8[0].bytEnterDelay;
            if (mySecurity.myAlarms8[0].bytSirnDelay >= 10 && mySecurity.myAlarms8[0].bytSirnDelay <= 180)
            {
                chbSiren.Checked = true;
                numSiren.Value = mySecurity.myAlarms8[0].bytSirnDelay;
            }
            if (Convert.ToByte(numReset.Minimum) <= mySecurity.myAlarms8[0].bytReset && mySecurity.myAlarms8[0].bytReset <= Convert.ToByte(numReset.Maximum))
                numReset.Value = mySecurity.myAlarms8[0].bytReset;

            for (int i = 0; i < mySecurity.myAlarms8[0].AlarmCmds.Length; i++)
            {
                if (mySecurity.myAlarms8[0].AlarmCmds[i] != null && mySecurity.myAlarms8[0].AlarmCmds[i].Count != 0)
                {
                    foreach (UVCMD.ControlTargets oTmp in mySecurity.myAlarms8[0].AlarmCmds[i])
                    {
                        
                    }
                }
            }
        }

        void DisplayVacationsSetup()
        {
            if (mySecurity == null) return;
            if (mySecurity.myVacs8 == null) return;
            if (mySecurity.myVacs8[0] == null) return;
            if (mySecurity.myVacs8[0].VacationCmds == null || mySecurity.myVacs8[0].VacationCmds.Length == 0) return;

            foreach (TreeNode Temp in tvVac.Nodes)
            {
                Temp.Nodes.Clear();
            }

            for (int i = 0; i < mySecurity.myVacs8[0].VacationCmds.Length; i++)
            {
                if (mySecurity.myVacs8[0].VacationCmds[i] != null && mySecurity.myVacs8[0].VacationCmds[i].Count != 0)
                {
                    foreach (UVCMD.ControlTargets oTmp in mySecurity.myVacs8[0].VacationCmds[i])
                    {
                    }
                }
            }
        }

        private void frmSecurity_Load(object sender, EventArgs e)
        {
            
        }

        private void frmSecurity_Shown(object sender, EventArgs e)
        {
            if (CsConst.MyEditMode == 0)
            {
                UpdateDisplayInformationAccordingly(sender, null);
            }
            else if (CsConst.MyEditMode == 1) //在线模式
            {
                if (mySecurity.MyRead2UpFlags[0] == false)
                {
                    MyActivePage = 1;
                    tsbDown_Click(tsbDown, null);

                }
                else
                {
                    DisplayGetReadythings();
                }
            }
        }

        private void btnApplyB_Click(object sender, EventArgs e)
        {
            if (tvBypass.SelectedNode == null) return;
            TreeNode oNode = tvBypass.SelectedNode;
            if (oNode.Level == 1) oNode = oNode.Parent;
            
            if (mySecurity.myReadyThings8[0].DetectDoors[0] == 0) return;

            byte bytID = (byte)oNode.Index;
            //取后面五个 依次表示报警状态 当前状态 信息设置
            byte[] AraySetups = new byte[5];

            if ( rbtnO.Checked == true) { AraySetups[0] = 0;}
            else { AraySetups[0] = 1;}

            if (cboType.SelectedIndex == 0)
            {
                #region
                AraySetups[4] = 0;
                if (radioButton15.Checked)
                {
                    AraySetups[2] = 1;
                    AraySetups[3] = 0;
                }
                else if (radioButton16.Checked)
                {
                    AraySetups[2] = 1 << 1;
                    AraySetups[3] = 0;
                }
                else if (radioButton17.Checked)
                {
                    AraySetups[2] = 1 << 2;
                    AraySetups[3] = 0;
                }
                else if (radioButton18.Checked)  //低位4 bit
                {
                    AraySetups[2] = 1 << 3;
                    AraySetups[3] = 0;
                }
                else if (radioButton19.Checked)
                {
                    AraySetups[2] = 0;
                    AraySetups[3] = 1 << 1;
                }
                else if (radioButton20.Checked)
                {
                    AraySetups[2] = 0;
                    AraySetups[3] = 1 << 2;
                }
                else if (radioButton21.Checked)
                {
                    AraySetups[2] = 0;
                    AraySetups[3] = 1 << 3;
                }
                #endregion
            }
            else if (cboType.SelectedIndex == 1) 
            {
                AraySetups[2] = 0;
                AraySetups[3] = 0;
                for (byte byti = 0; byti <= 4; byti++)
                {
                    if (chbTypes.GetItemChecked(byti)) AraySetups[3] =(byte)(AraySetups[3] | (1 << byti));
                }
            }

            Array.Copy(AraySetups, 0, mySecurity.myReadyThings8[0].DetectDoors, bytID * 20 + 6, 5);
        }

        private void cboType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboType.SelectedIndex == -1) return;
            chbTypes.Visible = (cboType.SelectedIndex == 1);
            if (chbTypes.Visible) chbTypes.BringToFront();
        }

        private void numExit_ValueChanged(object sender, EventArgs e)
        {
            if (mySecurity == null) return;
            if (mySecurity.myReadyThings8[0] == null) return;
            mySecurity.myReadyThings8[0].bytExitDelay = (byte)numExit.Value;
        }

        private void chbMore_CheckedChanged(object sender, EventArgs e)
        {
            if (mySecurity == null) return;
            if (mySecurity.myReadyThings8[0] == null) return;
            if (chbMore.Checked) mySecurity.myReadyThings8[0].bytMore = 1;
            else mySecurity.myReadyThings8[0].bytMore = 0;
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CsConst.MyEditMode == 0)
            {
                switch (tabControl1.SelectedIndex)
                {
                    case 0: DisplayGetReadythings(); break;
                    case 1: DisplayArmCMD(); break;
                    case 2: DisplayAutoArmDisarm(); break;
                    case 3: DisplayAlarmSetups(); break;
                    case 4: DisplayVacationsSetup(); break;
                }
            }
            else if (CsConst.MyEditMode == 1)
            {
                MyActivePage = tabControl1.SelectedIndex + 1;
                if (mySecurity.MyRead2UpFlags[MyActivePage - 1] == false)
                {
                    tsbDown_Click(tsbDown, null);
                }
                else
                {
                    switch (tabControl1.SelectedIndex)
                    {
                        case 0: DisplayGetReadythings(); break;
                        case 1: DisplayArmCMD(); break;
                        case 2: DisplayAutoArmDisarm(); break;
                        case 3: DisplayAlarmSetups(); break;
                        case 4: DisplayVacationsSetup(); break;
                    }
                }
            }
        }

        private void tvVac_MouseDown(object sender, MouseEventArgs e)
        {
            TreeNode oNode = tvVac.GetNodeAt(e.Location);
            if (oNode == null) return;

            if (oNode.Level != 0) oNode = oNode.Parent;
            if (mySecurity.myVacs8[0] == null) return;
            if (mySecurity.myVacs8[0].SetupTime.Length == 0) return;

            byte bytID = (byte)oNode.Index;

            byte[] AraySetups = new byte[10];
            Array.Copy(mySecurity.myVacs8[0].SetupTime, bytID * 10, AraySetups, 0, 10);

            chbVac.Checked = (AraySetups[0] == 0);
            dtAction.Text = AraySetups[1].ToString() + ":"
                          + AraySetups[2].ToString() + ":"
                          + AraySetups[3].ToString();

            dtStop.Text = AraySetups[4].ToString() + ":"
                          + AraySetups[5].ToString() + ":"
                          + AraySetups[6].ToString();
        }

        private void chbVac_CheckedChanged(object sender, EventArgs e)
        {
            dtAction.Enabled = chbVac.Checked;
            dtStop.Enabled = chbVac.Checked;
        }

        private void lvArmModule_KeyDown(object sender, KeyEventArgs e)
        {
            if (lvArmModule.SelectedItems == null) return;
            if (lvArmModule.SelectedItems.Count == 0) return;
            if (e.KeyData == Keys.Delete) //按下delete键
            {
                //删除当前按键 缓存更新
                byte bytIndex = Convert.ToByte(lvArmModule.SelectedItems[0].Index.ToString());
                byte bytTotal = mySecurity.myReadyThings8[0].ArmModules[0];

                if (bytIndex == bytTotal)
                {
                    bytTotal--;
                    mySecurity.myReadyThings8[0].ArmModules[0] = bytTotal;
                }
                else if (bytIndex < bytTotal)
                {
                    for (byte bytI = bytIndex; bytI < bytTotal; bytI++)
                    {
                        Array.Copy(mySecurity.myReadyThings8[0].ArmModules, (bytIndex + 1) * 30 + 1, mySecurity.myReadyThings8[0].ArmModules, bytIndex * 30 + 1, 30);
                        bytTotal--;
                        mySecurity.myReadyThings8[0].ArmModules[0] = bytTotal;
                    }
                }
                lvArmModule.SelectedItems.Clear();
            }


        }

        private void chbBT_CheckedChanged(object sender, EventArgs e)
        {
            if (tvArm1.SelectedNode == null) return;
            TreeNode oNode = tvArm1.SelectedNode;
            if (oNode.Level != 0) oNode = oNode.Parent;

            if (chbBT.Checked) mySecurity.myArms8[0].bytFlag[oNode.Index] = 1;
            else mySecurity.myArms8[0].bytFlag[oNode.Index] = 0;
        }

        private void chbAA1_CheckedChanged(object sender, EventArgs e)
        {
            byte bytTag = Convert.ToByte(((CheckBox)sender).Tag.ToString());

            if (((CheckBox)sender).Checked)
            {
                mySecurity.myArms8[0].AutoArmDisarm[0 + bytTag * 10] = 1;
            }
            else
            {
                mySecurity.myArms8[0].AutoArmDisarm[0 + bytTag * 10] = 0;
            }
        }

        private void dtTime1_ValueChanged(object sender, EventArgs e)
        {
            byte bytTag = Convert.ToByte(((DateTimePicker)sender).Tag.ToString());

            string strTime = ((DateTimePicker)sender).Text;

            string[] ArayStr = strTime.Split(':');
            for (byte bytI = 0; bytI < ArayStr.Length; bytI++)
            {
                mySecurity.myArms8[0].AutoArmDisarm[1 + bytI + bytTag * 10] = Convert.ToByte(ArayStr[bytI]);
            }
        }

        private void rbtnA11_CheckedChanged(object sender, EventArgs e)
        {
            byte bytTag = Convert.ToByte(((RadioButton)sender).Tag.ToString());

            if (((RadioButton)sender).Checked)
            {
                mySecurity.myArms8[0].AutoArmDisarm[3] = bytTag;
            }
        }

        private void rbtnA21_CheckedChanged(object sender, EventArgs e)
        {
            byte bytTag = Convert.ToByte(((RadioButton)sender).Tag.ToString());

            if (((RadioButton)sender).Checked)
            {
                mySecurity.myArms8[0].AutoArmDisarm[13] = bytTag;
            }
        }

        private void rbtnA31_CheckedChanged(object sender, EventArgs e)
        {
            byte bytTag = Convert.ToByte(((RadioButton)sender).Tag.ToString());

            if (((RadioButton)sender).Checked)
            {
                mySecurity.myArms8[0].AutoArmDisarm[23] = bytTag;
            }
        }

        private void chbA4_SelectedIndexChanged(object sender, EventArgs e)
        {
            byte bytTag = Convert.ToByte(((CheckedListBox)sender).Tag.ToString());

            mySecurity.myArms8[0].AutoArmDisarm[3 + bytTag * 10] = 0;

            for (byte bytI = 0; bytI < ((CheckedListBox)sender).Items.Count; bytI++)
            {
                if (((CheckedListBox)sender).GetItemChecked(bytI))
                {
                    mySecurity.myArms8[0].AutoArmDisarm[3 + bytTag * 10] =(byte)( mySecurity.myArms8[0].AutoArmDisarm[3 + bytTag * 10] | (1 << bytI));
                }
            }
        }

        private void numEntry_ValueChanged(object sender, EventArgs e)
        {

        }

        private void btnApplyV_Click(object sender, EventArgs e)
        {
            if (tvVac.SelectedNode == null) return;

            TreeNode oNode = tvVac.SelectedNode;
            if (oNode.Level != 0) oNode = oNode.Parent;
            if (mySecurity.myVacs8[0].SetupTime.Length == 0) return;

            byte bytID = (byte)oNode.Index;

            byte[] AraySetups = new byte[10];
            
            if (chbVac.Checked) AraySetups[0] = 1;
            string strTime = dtAction.Text;
            string[] ArayStr = strTime.Split(':');
            AraySetups[1] = Convert.ToByte(ArayStr[0]);
            AraySetups[2] = Convert.ToByte(ArayStr[1]);
            AraySetups[3] = Convert.ToByte(ArayStr[2]);

            strTime = dtStop.Text;
            ArayStr = strTime.Split(':');
            AraySetups[4] = Convert.ToByte(ArayStr[0]);
            AraySetups[5] = Convert.ToByte(ArayStr[1]);
            AraySetups[6] = Convert.ToByte(ArayStr[2]);

            Array.Copy(AraySetups, 0, mySecurity.myVacs8[0].SetupTime, bytID * 10, 10);
        }

        private void tmiCopy_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 0) // 第一个页面
            {
                #region
                if (tvArm.Nodes == null) return;
                if (mySecurity == null) return;
                if (tvArm.SelectedNode == null) return;
                if (tvArm.SelectedNode.Level == 2) return;
                if (tvArm.SelectedNode.Level == 1 && tvArm.SelectedNode.Index == 0) return;
                if (mySecurity.myReadyThings8[0].LeaveCmds == null) return;
                
                CsConst.MyPublicCtrls = new List<UVCMD.ControlTargets>();

                TreeNode oNode = tvArm.SelectedNode;
                byte bytAll = 0;     //表示选择的目标个数
                if (oNode.Level == 1) // 表示单个选中的是目标
                {
                    bytAll = 1;
                }
                else                                     //表示总共选中的目标
                {
                    bytAll = Convert.ToByte((oNode.Nodes.Count).ToString());
                }

                //2.  根据分类进行复制
                if (bytAll == 1)
                {
                    UVCMD.ControlTargets oCtrls = (UVCMD.ControlTargets)mySecurity.myReadyThings8[0].LeaveCmds[oNode.Parent.Index][oNode.Index].Clone();
                    CsConst.MyPublicCtrls.Add(oCtrls);
                }
                else
                {
                    string strTmp = oNode.Name;
                    byte bytKeyID = byte.Parse(strTmp.Split('-')[0].ToString());
                    byte bytStatus = byte.Parse(strTmp.Split('-')[1].ToString());

                    for (byte bytI = 0; bytI < bytAll; bytI++)
                    {
                        UVCMD.ControlTargets oCtrls = (UVCMD.ControlTargets)mySecurity.myReadyThings8[0].LeaveCmds[oNode.Index][bytI].Clone();
                        CsConst.MyPublicCtrls.Add(oCtrls);
                    }
                }
                #endregion
            }
            else if (tabControl1.SelectedIndex == 1) //第二个页面
            {
                #region
                if (tvBypass.Nodes == null) return;
                if (mySecurity == null) return;
                if (tvBypass.SelectedNode == null) return;
                if (tvBypass.SelectedNode.Level == 2) return;
                if (tvBypass.SelectedNode.Level == 1 && tvBypass.SelectedNode.Index == 0) return;
                if (mySecurity.myReadyThings8[0].DoorWarns == null) return;

                CsConst.MyPublicCtrls = new List<UVCMD.ControlTargets>();

                TreeNode oNode = tvBypass.SelectedNode;
                byte bytAll = 0;     //表示选择的目标个数
                if (oNode.Level == 1) // 表示单个选中的是目标
                {
                    bytAll = 1;
                }
                else                                     //表示总共选中的目标
                {
                    bytAll = Convert.ToByte((oNode.Nodes.Count).ToString());
                }

                //2.  根据分类进行复制
                if (bytAll == 1)
                {
                    UVCMD.ControlTargets oCtrls = (UVCMD.ControlTargets)mySecurity.myReadyThings8[0].DoorWarns[oNode.Parent.Index][oNode.Index].Clone();
                    CsConst.MyPublicCtrls.Add(oCtrls);
                }
                else
                {
                    string strTmp = oNode.Name;
                    byte bytKeyID = byte.Parse(strTmp.Split('-')[0].ToString());
                    byte bytStatus = byte.Parse(strTmp.Split('-')[1].ToString());

                    for (byte bytI = 0; bytI < bytAll; bytI++)
                    {
                        UVCMD.ControlTargets oCtrls = (UVCMD.ControlTargets)mySecurity.myReadyThings8[0].DoorWarns[oNode.Index][bytI].Clone();
                        CsConst.MyPublicCtrls.Add(oCtrls);
                    }
                }
                #endregion
            }
            else if (tabControl1.SelectedIndex == 2) // 第三个页面
            {
                #region
                if (tvArm1.Nodes == null) return;
                if (mySecurity == null) return;
                if (tvArm1.SelectedNode == null) return;
                if (tvArm1.SelectedNode.Level == 2) return;
                if (tvArm1.SelectedNode.Level == 1 && tvArm1.SelectedNode.Index == 0) return;
                if (mySecurity.myArms8[0].ArmCmds == null) return;

                CsConst.MyPublicCtrls = new List<UVCMD.ControlTargets>();

                TreeNode oNode = tvArm1.SelectedNode;
                byte bytAll = 0;     //表示选择的目标个数
                if (oNode.Level == 1) // 表示单个选中的是目标
                {
                    bytAll = 1;
                }
                else                                     //表示总共选中的目标
                {
                    bytAll = Convert.ToByte((oNode.Nodes.Count).ToString());
                }

                //2.  根据分类进行复制
                if (bytAll == 1)
                {
                    UVCMD.ControlTargets oCtrls = (UVCMD.ControlTargets)mySecurity.myArms8[0].ArmCmds[oNode.Parent.Index][oNode.Index].Clone();
                    CsConst.MyPublicCtrls.Add(oCtrls);
                }
                else
                {
                    string strTmp = oNode.Name;
                    byte bytKeyID = byte.Parse(strTmp.Split('-')[0].ToString());
                    byte bytStatus = byte.Parse(strTmp.Split('-')[1].ToString());

                    for (byte bytI = 0; bytI < bytAll; bytI++)
                    {
                        UVCMD.ControlTargets oCtrls = (UVCMD.ControlTargets)mySecurity.myArms8[0].ArmCmds[oNode.Index][bytI].Clone();
                        CsConst.MyPublicCtrls.Add(oCtrls);
                    }
                }
                #endregion
            }
            else if (tabControl1.SelectedIndex == 4) // 第五个页面
            {
                #region
                if (tvAlarm.Nodes == null) return;
                if (mySecurity == null) return;
                if (tvAlarm.SelectedNode == null) return;
                if (tvAlarm.SelectedNode.Level == 2) return;
                if (tvAlarm.SelectedNode.Level == 1 && tvAlarm.SelectedNode.Index == 0) return;
                if (mySecurity.myAlarms8[0].AlarmCmds == null) return;

                CsConst.MyPublicCtrls = new List<UVCMD.ControlTargets>();

                TreeNode oNode = tvAlarm.SelectedNode;
                byte bytAll = 0;     //表示选择的目标个数
                if (oNode.Level == 1) // 表示单个选中的是目标
                {
                    bytAll = 1;
                }
                else                                     //表示总共选中的目标
                {
                    bytAll = Convert.ToByte((oNode.Nodes.Count).ToString());
                }

                //2.  根据分类进行复制
                if (bytAll == 1)
                {
                    UVCMD.ControlTargets oCtrls = (UVCMD.ControlTargets)mySecurity.myAlarms8[0].AlarmCmds[oNode.Parent.Index][oNode.Index].Clone();
                    CsConst.MyPublicCtrls.Add(oCtrls);
                }
                else
                {
                    string strTmp = oNode.Name;
                    byte bytKeyID = byte.Parse(strTmp.Split('-')[0].ToString());
                    byte bytStatus = byte.Parse(strTmp.Split('-')[1].ToString());

                    for (byte bytI = 0; bytI < bytAll; bytI++)
                    {
                        UVCMD.ControlTargets oCtrls = (UVCMD.ControlTargets)mySecurity.myAlarms8[0].AlarmCmds[oNode.Index][bytI].Clone();
                        CsConst.MyPublicCtrls.Add(oCtrls);
                    }
                }
                #endregion
            }
            else if (tabControl1.SelectedIndex == 5) // 第六个页面
            {
                #region
                if (tvVac.Nodes == null) return;
                if (mySecurity == null) return;
                if (tvVac.SelectedNode == null) return;
                if (tvVac.SelectedNode.Level == 2) return;
                if (tvVac.SelectedNode.Level == 1 && tvVac.SelectedNode.Index == 0) return;
                if (mySecurity.myVacs8[0].VacationCmds == null) return;

                CsConst.MyPublicCtrls = new List<UVCMD.ControlTargets>();

                TreeNode oNode = tvVac.SelectedNode;
                byte bytAll = 0;     //表示选择的目标个数
                if (oNode.Level == 1) // 表示单个选中的是目标
                {
                    bytAll = 1;
                }
                else                                     //表示总共选中的目标
                {
                    bytAll = Convert.ToByte((oNode.Nodes.Count).ToString());
                }

                //2.  根据分类进行复制
                if (bytAll == 1)
                {
                    UVCMD.ControlTargets oCtrls = (UVCMD.ControlTargets)mySecurity.myVacs8[0].VacationCmds[oNode.Parent.Index][oNode.Index].Clone();
                    CsConst.MyPublicCtrls.Add(oCtrls);
                }
                else
                {
                    string strTmp = oNode.Name;
                    byte bytKeyID = byte.Parse(strTmp.Split('-')[0].ToString());
                    byte bytStatus = byte.Parse(strTmp.Split('-')[1].ToString());

                    for (byte bytI = 0; bytI < bytAll; bytI++)
                    {
                        UVCMD.ControlTargets oCtrls = (UVCMD.ControlTargets)mySecurity.myVacs8[0].VacationCmds[oNode.Index][bytI].Clone();
                        CsConst.MyPublicCtrls.Add(oCtrls);
                    }
                }
                #endregion

            }
        }

        private void tmiPaste_Click(object sender, EventArgs e)
        {
            
        }

        private void tmiDel_Click(object sender, EventArgs e)
        {
            
        }

        private void addCommandsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        void frmTmp_FormClosing(object sender, FormClosingEventArgs e)
        {
           
        }

        private void btnAddB_Click(object sender, EventArgs e)
        {

        }

        private void btnAlarm_Click(object sender, EventArgs e)
        {

        }

        private void tvArm_MouseDown(object sender, MouseEventArgs e)
        {
            MyPasteNode = ((TreeView)sender).GetNodeAt(e.Location);
        }

        private void frmSecurity_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            
        }

        private void tvBypass_MouseDown(object sender, MouseEventArgs e)
        {
            if (tvBypass.SelectedNode == null) return;
            TreeNode oNode = tvBypass.SelectedNode;

            if (oNode.Level == 1) oNode = oNode.Parent;
            if (mySecurity.myReadyThings8[0].DetectDoors[0] == 0) return;

            byte bytID = (byte)oNode.Index;
            //取后面五个 依次表示报警状态 当前状态 信息设置
            byte[] AraySetups = new byte[5];
            Array.Copy(mySecurity.myReadyThings8[0].DetectDoors, bytID * 20 + 6, AraySetups, 0, 5);

            if (AraySetups[0] == 0) rbtnO.Checked = true;
            else rbtnC.Checked = true;

            panel6.Visible = true;

            if (AraySetups[3] > 31 || AraySetups[2] > 0)
            {
                cboType.SelectedIndex = 0;
                chbTypes.Visible = false;
                for (byte byti = 0; byti <= 7; byti++)
                {
                    if ((AraySetups[2] << byti) == (1 << byti))
                    {
                        (this.Controls.Find("radioButton" + (15 + byti).ToString(), true)[0] as RadioButton).Checked = true;
                        break;
                    }
                }
            }
            else
            {
                cboType.SelectedIndex = 1;
                chbTypes.Visible = true;
                for (byte byti = 0; byti <= 4; byti++)
                {
                    bool blnIsChecked = ((AraySetups[3] & (1 << byti)) == (1 << byti));
                    chbTypes.SetItemChecked(byti, blnIsChecked);
                }
            }                         
        }

    }
}
