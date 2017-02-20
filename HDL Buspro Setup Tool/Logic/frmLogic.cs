using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;


namespace HDL_Buspro_Setup_Tool
{
    public partial class frmLogic : Form
    {
        private Logic myLogic = null;
        private string myDevName = null;
        private int mintIDIndex = -1;
        private int MyintDeviceType = -1;

        private int MySelectedLogic = -1;

        private TreeNode MyPasteNode = null;

        private Label[] MyTables = new Label[20]; //临时表格显示Logic Block

        private List<Byte[]> PublicPinTemplates = null; //将结构体里面的数据分解 里面存储为单个引脚的信息

        private int MyActivePage = 0; //按页面上传下载

        private byte SubNetID;
        private byte DeviceID;
        private bool isReading = false;


        public frmLogic()
        {
            InitializeComponent();
        }

        public frmLogic(Logic oLogic, string strName, int intDIndex, int intDeviceType)
        {
            InitializeComponent();
            this.myLogic = oLogic;
            this.myDevName = strName;
            this.mintIDIndex = intDIndex;
            this.MyintDeviceType = intDeviceType;
            string strDevName = strName.Split('\\')[0].ToString();
            this.Text = myDevName;
            tsName.Text = myDevName;

            HDLSysPF.DisplayDeviceNameModeDescription(strName, MyintDeviceType, cboDevice, tbModel, tbDescription);
        }

        void InitialFormCtrlsTextOrItems()
        {
            cbG34.Items.Clear();
            for (int i = 30; i < 121; i++)
            {
                cbG34.Items.Add(i.ToString());
            }
        }

        void SetCtrlsVisbleWithDifferentDeviceType()
        {
            if (myLogic == null) return;
            toolStrip1.Visible = (CsConst.MyEditMode == 0);
        }

        private void frmLogic_Shown(object sender, EventArgs e)
        {
            SetCtrlsVisbleWithDifferentDeviceType();
            if (CsConst.MyEditMode == 0)
            {
                UpdateDisplayInformationAccordingly(null,null);
            }
            else if (CsConst.MyEditMode == 1) //在线模式
            {
                MyActivePage = 1;
                if (myLogic.MyRead2UpFlags[0] == false)
                {
                    tbDown_Click(tbDown, null);
                }
                else
                {
                    UpdateDisplayInformationAccordingly(null, null);
                }
            }
        }

        void DisplayLogicBlocksInformation()
        {
            tvLogic.Nodes.Clear();
            if (myLogic == null) return;
            if (myLogic.MyDesign == null || myLogic.MyDesign.Length == 0)
            {
                for (int i = 1; i <= 20; i++)
                {
                    tvLogic.Nodes.Add(i.ToString(), CsConst.MyUnnamed + "(" + i.ToString() + ")", 5, 5);
                }
            }
            else
            {
                byte bytBlock = 1;
                foreach (Logic.LogicBlock oTmp in myLogic.MyDesign)
                {
                    int intImageIndex = 5;
                    byte bytTotalTB = myLogic.MyDesign[bytBlock - 1].TableIDs[0]; 
                    if (bytTotalTB != 0 && bytTotalTB <= 20) intImageIndex = 6;
                    TreeNode oNode = tvLogic.Nodes.Add(bytBlock.ToString(), myLogic.MyDesign[bytBlock - 1].Remark + "(" + bytBlock.ToString() + ")", intImageIndex, intImageIndex);

                    for (int i = 0; i < bytTotalTB; i++) 
                    { 
                        TreeNode oTmpKey = oNode.Nodes.Add(myLogic.MyDesign[bytBlock - 1].Remarks[i]);

                        if (myLogic.MyDesign[bytBlock - 1].ArmCmds[i] != null && myLogic.MyDesign[bytBlock - 1].ArmCmds[i].Count != 0)
                        {
                            for (int j = 0; j < myLogic.MyDesign[bytBlock - 1].ArmCmds[i].Count; j++)
                            {
                                oTmpKey.Nodes.Add(null, myLogic.MyDesign[bytBlock - 1].ArmCmds[i][j].Hint, 1, 1);
                            }
                        }
                    };
                    bytBlock++;
                }
            }
        }

        void DisplaySystemSetup()
        {
            if (myLogic == null) return;
            isReading = true;
            if (myLogic.LogicPos == null) myLogic.LogicPos = new byte[50];
            if (myLogic.DateTimeAry == null) myLogic.DateTimeAry = new byte[6];
            if (myLogic.LogicSummerTime == null) myLogic.LogicSummerTime = new byte[50];
            int wdYear = Convert.ToInt32(myLogic.DateTimeAry[0]) + 2000;
            byte bytMonth = myLogic.DateTimeAry[1];
            byte bytDay = myLogic.DateTimeAry[2];
            byte bytHour = myLogic.DateTimeAry[3];
            byte bytMinute = myLogic.DateTimeAry[4];
            byte bytSecond = myLogic.DateTimeAry[5];

            if (bytHour > 23) bytHour = 0;
            if (bytMinute > 59) bytMinute = 0;
            if (bytSecond > 59) bytSecond = 0;
            if (bytMonth > 12 || bytMonth == 0) bytMonth = 1;
            if (bytDay > 31 || bytDay ==0) bytDay = 1;

            DateTime d = new DateTime(wdYear, Convert.ToInt32(bytMonth), Convert.ToInt32(bytDay));
            DatePicker.Value = d;
            DatePicker_ValueChanged(null, null);
            numTime1.Value = Convert.ToDecimal(bytHour);
            numTime2.Value = Convert.ToDecimal(bytMinute);
            numTime3.Value = Convert.ToDecimal(bytSecond);

            chbTime.Checked = myLogic.isBroadcastTime;

            byte shortintLatitude = myLogic.LogicPos[0];
            byte shortintLatitudeMinute = myLogic.LogicPos[1];
            int smallintLongitude = myLogic.LogicPos[2] * 256 + myLogic.LogicPos[3];
            byte shortintLongitudeMinute = myLogic.LogicPos[4];
            byte shortintTimeZone = myLogic.LogicPos[5];
            byte shortintMinuteInTimeZone = myLogic.LogicPos[6];
            byte bytHourOfSunrise = myLogic.LogicPos[7];
            byte bytMinuteOfSunrise = myLogic.LogicPos[8];
            byte bytHourOfSundown = myLogic.LogicPos[9];
            byte bytMinuteOfSundown = myLogic.LogicPos[10];

            if (shortintLatitude > 0)
                cb11.SelectedIndex = 0;
            else
                cb11.SelectedIndex = 1;
            
            txt11.Text = Convert.ToString(Math.Abs(shortintLatitude));
            txt12.Text = Convert.ToString(Math.Abs(shortintLatitudeMinute));

            if ((myLogic.LogicPos[2] * 256 + myLogic.LogicPos[3]) < 32768)
            {
                cb21.SelectedIndex = 0;
                txt21.Text = Convert.ToString(Math.Abs(smallintLongitude));
            }
            else
            {
                cb21.SelectedIndex = 1;
                txt21.Text = Convert.ToString(Math.Abs((smallintLongitude - 65536) % 256));
            }
            txt22.Text = Convert.ToString(Math.Abs(shortintLongitudeMinute));

            string strTimeZone = string.Format("{0:F0}", float.Parse(Convert.ToString(shortintTimeZone)));
            strTimeZone = string.Format("{0:D2}", Convert.ToInt32(strTimeZone));
            if (shortintTimeZone > 0) strTimeZone = "+" + strTimeZone;
            cb31.SelectedIndex = cb31.Items.IndexOf(strTimeZone);
            txt31.Text=string.Format("{0:F0}", float.Parse(Convert.ToString(Math.Abs(shortintMinuteInTimeZone))));
            txt31.Text = string.Format("{0:D2}", Convert.ToInt32(txt31.Text));

            //lb41.Text = string.Format("{0:F0}", bytHourOfSunrise) + ":" + string.Format("{0:F0}", bytMinuteOfSunrise);
            //lb51.Text = string.Format("{0:F0}", bytHourOfSundown) + ":" + string.Format("{0:F0}", bytMinuteOfSundown);
            lb4.Visible = false;
            lb5.Visible = false;
            //lb41.Visible = false;
            lb51.Visible = false;

            if (0 <= ((myLogic.LogicPos[10] - 100) / 5) && ((myLogic.LogicPos[10] - 100) / 5) < cbG31.Items.Count)
                cbG31.SelectedIndex = (myLogic.LogicPos[10] - 100) / 5;
            if (0 <= ((myLogic.LogicPos[11] - 100) / 5) && ((myLogic.LogicPos[11] - 100) / 5) < cbG32.Items.Count)
                cbG32.SelectedIndex = (myLogic.LogicPos[11] - 100) / 5;
            if (0 <= ((myLogic.LogicPos[12] - 100) / 5) && ((myLogic.LogicPos[12] - 100) / 5) < cbG33.Items.Count)
                cbG33.SelectedIndex = (myLogic.LogicPos[12] - 100) / 5;
            cbG34.SelectedIndex = cbG34.Items.IndexOf(myLogic.LogicPos[13].ToString());

            string strTime = string.Format("{0:D2}", Convert.ToInt32(string.Format("{0:F0}", myLogic.LogicPos[21]))) + ":" +
                             string.Format("{0:D2}", Convert.ToInt32(string.Format("{0:F0}", myLogic.LogicPos[22])));
            string strTime1 = string.Format("{0:D2}", Convert.ToInt32(string.Format("{0:F0}", myLogic.LogicPos[23]))) + ":" +
                             string.Format("{0:D2}", Convert.ToInt32(string.Format("{0:F0}", myLogic.LogicPos[24])));
            string strTime2 = string.Format("{0:D2}", Convert.ToInt32(string.Format("{0:F0}", myLogic.LogicPos[25]))) + ":" +
                             string.Format("{0:D2}", Convert.ToInt32(string.Format("{0:F0}", myLogic.LogicPos[26])));
            string strTime3 = string.Format("{0:D2}", Convert.ToInt32(string.Format("{0:F0}", myLogic.LogicPos[27]))) + ":" +
                             string.Format("{0:D2}", Convert.ToInt32(string.Format("{0:F0}", myLogic.LogicPos[28])));
            string strTime4 = string.Format("{0:D2}", Convert.ToInt32(string.Format("{0:F0}", myLogic.LogicPos[29]))) + ":" +
                             string.Format("{0:D2}", Convert.ToInt32(string.Format("{0:F0}", myLogic.LogicPos[30])));
            string strTime5 = string.Format("{0:D2}", Convert.ToInt32(string.Format("{0:F0}", myLogic.LogicPos[31]))) + ":" +
                             string.Format("{0:D2}", Convert.ToInt32(string.Format("{0:F0}", myLogic.LogicPos[32])));

            if (myLogic.LogicPos[7] == 1)
            {
                chb31.Checked = true;
                lb41.Text = "";
                if (CsConst.iLanguageId == 1)
                {
                    lb41.Text = lb41.Text + "晨礼盛行拜:" + strTime + "  " + "晨礼主名拜:" + strTime1 + "\r\n";
                    lb41.Text = lb41.Text + "晌礼:      " + strTime2 + "  " + "哺礼:      " + strTime3 + "\r\n";
                    lb41.Text = lb41.Text + "昏礼:      " + strTime4 + "  " + "霄礼:      " + strTime5;
                }
                else
                {
                    lb41.Text = lb41.Text + "Fajr:   " + strTime + "  " + "Sunrise:" + strTime1 + "\r\n";
                    lb41.Text = lb41.Text + "Dhuhr: " + strTime2 + "  " + "Asr:    " + strTime3 + "\r\n";
                    lb41.Text = lb41.Text + "Maghrib:" + strTime4 + "  " + "Isha:   " + strTime5;
                }
                chb31_CheckedChanged(null, null);
            }
            else
            {
                lb4.Visible = true;
                lb5.Visible = true;
                lb51.Visible = true;
                lb41.Text = strTime1;
                lb51.Text = strTime4;
            }
            if (myLogic.LogicPos[8] == 1) rbG21.Checked = true;
            else rbG22.Checked = true;

            if (1 <= myLogic.LogicPos[14] && myLogic.LogicPos[14] <= 90)
                numG1.Value = Convert.ToDecimal(myLogic.LogicPos[14]);
            if (1 <= myLogic.LogicPos[15] && myLogic.LogicPos[15] <= 15)
                numG2.Value = Convert.ToDecimal(myLogic.LogicPos[15]);

            if (myLogic.LogicPos[9] == 1) rbG31.Checked = true;
            else if (myLogic.LogicPos[9] == 2) rbG32.Checked = true;
            else if (myLogic.LogicPos[9] == 3) rbG33.Checked = true;
            else if (myLogic.LogicPos[9] == 4) rbG34.Checked = true;
            else if (myLogic.LogicPos[9] == 5) rbG35.Checked = true;
            else if (myLogic.LogicPos[9] == 6) rbG36.Checked = true;
            else if (myLogic.LogicPos[9] == 7) rbG37.Checked = true;



            if (0 <=myLogic.LogicSummerTime[0] && myLogic.LogicSummerTime[0] <= 3) cbtype.SelectedIndex = myLogic.LogicSummerTime[0];
            if (myLogic.LogicSummerTime[0] == 1)
            {
                if (0 <= myLogic.LogicSummerTime[3] && myLogic.LogicSummerTime[3] <= 11)
                    cb1.SelectedIndex = myLogic.LogicSummerTime[3]-1;
                if (0 <= myLogic.LogicSummerTime[5] && myLogic.LogicSummerTime[5] <= 23)
                    cb4.SelectedIndex = myLogic.LogicSummerTime[5];
                if (0 <= myLogic.LogicSummerTime[6] && myLogic.LogicSummerTime[6] <= 4)
                    cb2.SelectedIndex = myLogic.LogicSummerTime[6];
                if (0 <= myLogic.LogicSummerTime[7] && myLogic.LogicSummerTime[7] <= 6)
                    cb3.SelectedIndex = myLogic.LogicSummerTime[7];
                if (myLogic.LogicSummerTime[8] == 1) chbEnd.Checked = true;
                else chbEnd.Checked = false;
                if (0 <= myLogic.LogicSummerTime[11] && myLogic.LogicSummerTime[11] <= 11)
                    cb5.SelectedIndex = myLogic.LogicSummerTime[11]-1;
                if (0 <= myLogic.LogicSummerTime[13] && myLogic.LogicSummerTime[13] <= 23)
                    cb8.SelectedIndex = myLogic.LogicSummerTime[13];
                if (0 <= myLogic.LogicSummerTime[14] && myLogic.LogicSummerTime[14] <= 4)
                    cb6.SelectedIndex = myLogic.LogicSummerTime[14];
                if (0 <= myLogic.LogicSummerTime[15] && myLogic.LogicSummerTime[15] <= 6)
                    cb7.SelectedIndex = myLogic.LogicSummerTime[15];
                
            }
            else if (myLogic.LogicSummerTime[0] == 2)
            {
                if (0 <= myLogic.LogicSummerTime[5] && myLogic.LogicSummerTime[5] <= 23)
                    cb4.SelectedIndex = myLogic.LogicSummerTime[5];
                if (myLogic.LogicSummerTime[8] == 1) chbEnd.Checked = true;
                else chbEnd.Checked = false;
                if (0 <= myLogic.LogicSummerTime[13] && myLogic.LogicSummerTime[13] <= 23)
                    cb8.SelectedIndex = myLogic.LogicSummerTime[13];
                wdYear = myLogic.LogicSummerTime[1] * 256 + myLogic.LogicSummerTime[2];
                bytMonth = myLogic.LogicSummerTime[3];
                bytDay = myLogic.LogicSummerTime[4];
                if (bytMonth > 12) bytMonth = 1;
                if (bytDay > 31) bytDay = 1;
                d = new DateTime(wdYear, Convert.ToInt32(bytMonth), Convert.ToInt32(bytDay));
                d1.Value = d;

                wdYear = myLogic.LogicSummerTime[9] * 256 + myLogic.LogicSummerTime[10];
                bytMonth = myLogic.LogicSummerTime[11];
                bytDay = myLogic.LogicSummerTime[12];
                if (bytMonth > 12) bytMonth = 1;
                if (bytDay > 31) bytDay = 1;
                d = new DateTime(wdYear, Convert.ToInt32(bytMonth), Convert.ToInt32(bytDay));
                d2.Value = d;
            }
            else if (myLogic.LogicSummerTime[0] == 3)
            {
                if (0 <= myLogic.LogicSummerTime[5] && myLogic.LogicSummerTime[5] <= 23)
                    cb4.SelectedIndex = myLogic.LogicSummerTime[5];
                if (myLogic.LogicSummerTime[8] == 1) chbEnd.Checked = true;
                else chbEnd.Checked = false;
                if (0 <= myLogic.LogicSummerTime[13] && myLogic.LogicSummerTime[13] <= 23)
                    cb8.SelectedIndex = myLogic.LogicSummerTime[13];

                wdYear = myLogic.LogicSummerTime[1] * 256 + myLogic.LogicSummerTime[2];
                bytMonth = myLogic.LogicSummerTime[3];
                bytDay = myLogic.LogicSummerTime[4];
                if (bytMonth > 12) bytMonth = 1;
                if (bytDay > 31) bytDay = 1;
                d = new DateTime(wdYear, Convert.ToInt32(bytMonth), Convert.ToInt32(bytDay));
                d3.SelectedSolarDate = d;

                wdYear = myLogic.LogicSummerTime[9] * 256 + myLogic.LogicSummerTime[10];
                bytMonth = myLogic.LogicSummerTime[11];
                bytDay = myLogic.LogicSummerTime[12];
                if (bytMonth > 12) bytMonth = 1;
                if (bytDay > 31) bytDay = 1;
                d = new DateTime(wdYear, Convert.ToInt32(bytMonth), Convert.ToInt32(bytDay));
                d4.SelectedSolarDate = d;
            }
            chb31_CheckedChanged(null, null);
            cbtype_SelectedIndexChanged(null, null);
            isReading = false;
        }

        private void tbDown_Click(object sender, EventArgs e)
        {
            byte bytTag = Convert.ToByte(((ToolStripButton)sender).Tag.ToString());
            bool blnShowMsg = (CsConst.MyEditMode != 1);
            try
            {
                if (HDLPF.UploadOrReadGoOn(this.Text, bytTag, blnShowMsg))
                {
                    Cursor.Current = Cursors.WaitCursor;
                    // ReadDownLoadThread();  //增加线程，使得当前窗体的任何操作不被限制

                    CsConst.MyUPload2DownLists = new List<byte[]>();

                    string strName = myDevName.Split('\\')[0].ToString();
                    byte bytSubID = byte.Parse(strName.Split('-')[0]);
                    byte bytDevID = byte.Parse(strName.Split('-')[1]);

                    byte[] ArayRelay = new byte[] {  bytSubID, bytDevID, (byte)(MyintDeviceType / 256), (byte)(MyintDeviceType % 256),
                         (byte)MyActivePage,(byte)(mintIDIndex/256),(byte)(mintIDIndex%256)};

                    CsConst.MyUPload2DownLists.Add(ArayRelay);
                    CsConst.MyUpload2Down = Convert.ToByte(((ToolStripButton)sender).Tag.ToString());
                    if (CsConst.MyUpload2Down == 1) setSummerTime();
                    FrmDownloadShow Frm = new FrmDownloadShow();
                    if (CsConst.MyUpload2Down == 0) Frm.FormClosing += new FormClosingEventHandler(UpdateDisplayInformationAccordingly);
                    Frm.ShowDialog();
                }
            }
            catch { }
        }

        void UpdateDisplayInformationAccordingly(object sender, FormClosingEventArgs e)
        {
            switch (tabLogic.SelectedIndex)
            {
                case 0:
                   PublicPinTemplates = HDLSysPF.CreatePublicPinTemplatesListByteArrayFromLogic(myLogic);
                   HDLSysPF.DisplayLogicTemplates(PublicPinTemplates, lvTemplates,true);
                    DisplayLogicBlocksInformation(); break;
                case 1: DisplaySystemSetup();break;
            }
        }


        private void tvLogic_MouseDown(object sender, MouseEventArgs e)
        {
            if (tvLogic.Nodes.Count == 0) return;
            TreeNode oNode = tvLogic.GetNodeAt(e.Location);
            if (oNode == null) return;
            MyPasteNode = oNode;

            if (oNode.Level == 1) oNode = oNode.Parent;
            else if (oNode.Level == 2) oNode = oNode.Parent.Parent;
            
            //显示是不是已选中过 否则直接显示 
            if (MySelectedLogic != oNode.Index)
            {
                MySelectedLogic = oNode.Index;
                ShowALogicBlockInformation();
            }
        }

        void ShowALogicBlockInformation()
        {
            //清空显示
            #region
            foreach (Label oTmp in MyTables)
            {
                if (oTmp != null && oTmp.Visible) oTmp.Visible = false;
            }
            #endregion
            if (MySelectedLogic == -1) return;
            if (myLogic == null) return;
            if (myLogic.MyDesign[MySelectedLogic].TableIDs == null) return;
            if (myLogic.MyDesign[MySelectedLogic].TableIDs[0] == 0) return;
            if (myLogic.MyDesign[MySelectedLogic].MyPins == null || myLogic.MyDesign[MySelectedLogic].MyPins.Count == 0) return;

            for (byte bytI = 1; bytI <= myLogic.MyDesign[MySelectedLogic].TableIDs[0]; bytI++)
            {
                byte bytTableID = myLogic.MyDesign[MySelectedLogic].TableIDs[bytI];
                //显示表 写引脚
                MyTables[bytTableID] = new Label();
                gbLogic.Controls.Add(MyTables[bytTableID]);
                MyTables[bytTableID].Size = new Size(197,165);

                if (bytI<=myLogic.MyDesign[MySelectedLogic].MyPins.Count &&  myLogic.MyDesign[MySelectedLogic].MyPins[bytI - 1] != null)
                {
                    byte[] oTmpPins = myLogic.MyDesign[MySelectedLogic].MyPins[bytI - 1];
                    DisplayOneTable(oTmpPins, bytTableID, bytI);
                }
            }
        }

        void DisplayOneTable(Byte[] TmpTableWithFourPinsInformation, Byte tableId, Byte saveIndex)
        {
            #region
            tableId =(Byte)(tableId - 1);
            string strPath = string.Empty;
            byte[] oTmpPins = TmpTableWithFourPinsInformation;
            if (oTmpPins[0] == 2 || oTmpPins[0] == 0)
            {
                oTmpPins[0] = 2;
                strPath = Application.StartupPath + @"\ICON\OR.bmp";
            }
            else if (oTmpPins[0] == 1)
            {
                strPath = Application.StartupPath + @"\ICON\AND.bmp";
            }
            else if (oTmpPins[0] == 3)
            {
                strPath = Application.StartupPath + @"\ICON\NAND.bmp";
            }

            string[] ArayHint = HDLSysPF.GetDescriptionsFromBuffer(oTmpPins);
            if (strPath == "") return;
            if (MyTables[tableId] == null)
            {
                //显示表 写引脚
                MyTables[tableId] = new Label();
                gbLogic.Controls.Add(MyTables[tableId]);
                MyTables[tableId].Size = new Size(197, 165);
            }
            MyTables[tableId].Tag = tableId;
            MyTables[tableId].Image = Image.FromFile(strPath);
            MyTables[tableId].Location = new Point(oTmpPins[2] * 256 + oTmpPins[3], oTmpPins[4] * 256 + oTmpPins[5]);

            Bitmap image = new Bitmap(MyTables[tableId].Width, MyTables[tableId].Height);
            MyTables[tableId].DrawToBitmap(image, new Rectangle(0, 0, MyTables[tableId].Width, MyTables[tableId].Height));

            Graphics g = Graphics.FromImage(image);
            g.DrawString("    " + (tableId + 1).ToString() + " " + myLogic.MyDesign[MySelectedLogic].Remarks[saveIndex], new Font("Calibri", 10f), Brushes.Black, new Point(0, 0));
            g.DrawString("Work after " + (oTmpPins[6] * 256 + oTmpPins[7]).ToString() + "s", new Font("Calibri", 10f), Brushes.Black, new Point(40, 6));
            for (byte bytJ = 0; bytJ < 4; bytJ++)
            {
                if (ArayHint[bytJ] != null && ArayHint[bytJ] != "")
                {
                    g.DrawString(ArayHint[bytJ], new Font("Calibri", 10f), Brushes.Black, new Point(24, 40 + bytJ * 24));
                }
            }
            MyTables[tableId].Visible = true;
            MyTables[tableId].MouseDoubleClick += new MouseEventHandler(frmLogic_MouseDoubleClick);
            MyTables[tableId].MouseDown += new MouseEventHandler(tmpLogicTable_MouseDown);
            MyTables[tableId].MouseMove += new MouseEventHandler(tmpLogicTable_MouseMove);
            MyTables[tableId].MouseUp += new MouseEventHandler(tmpLogicTable_Mouseup);
            MyTables[tableId].Image = image;
            #endregion
        }
        
        void frmLogic_MouseDoubleClick(object sender, MouseEventArgs e)
        {
           // frmLogicPin frmTmp = new frmLogicPin(myLogic, (byte)MySelectedLogic);
            //frmTmp.ShowDialog();

            //if (frmTmp.DialogResult == DialogResult.OK)
            //{
            //    //更新节点备注
            //    for (byte bytI = 0; bytI<myLogic.MyDesign[MySelectedLogic].TableIDs[0];bytI++)
            //    {
            //        tvLogic.Nodes[MySelectedLogic].Nodes[bytI].Text = myLogic.MyDesign[MySelectedLogic].Remarks[bytI];
            //    }
            //    ShowALogicBlockInformation();
            //}
            //myLogic.MyRead2UpFlags[1] = false;
        }

        private void frmLogic_Load(object sender, EventArgs e)
        {
            InitialFormCtrlsTextOrItems();
        }

        Point pt;
        Control c = null;
        private void tmpLogicTable_MouseDown(object sender, MouseEventArgs e)
        {
           // pt =new Point(Control.MousePosition.X,Control.MousePosition.Y);
            c = gbLogic;
        }


        private void tmpLogicTable_MouseMove(object sender, MouseEventArgs e)
        {
            Label Tmp = ((Label)sender);
            if (Tmp.Tag == null) return;
            Byte ButtonTag = Convert.ToByte(Tmp.Tag.ToString());
            if (MySelectedLogic == -1) return;
            try
            {
                int mx = 0;
                int my = 0;
                int px = 0;
                int py = 0;
                int x = 0;
                int y = 0;
                if (e.Button == MouseButtons.Left)
                {
                    x = Cursor.Position.X - this.Location.X - gbLogic.Location.X;
                    y = Cursor.Position.Y - this.Location.Y - gbLogic.Location.Y;
                    if (pt != new Point(0, 0))
                    {
                        px = Cursor.Position.X - pt.X;//取得鼠标相对移动的位置
                        py = Cursor.Position.Y - pt.Y;//pt是存储上次鼠标移动后的位置
                    }
                    mx = ((Label)sender).Location.X + px;//把鼠标移动大小复给label，让label也移动鼠标移动的大小
                    my = ((Label)sender).Location.Y + py;
                    if (mx <= 0) mx = 0;
                    if (my <= 0) my = 0;
                    ((Label)sender).Location = new Point(mx, my);

                    pt = Cursor.Position;//重新存储鼠标位置

                    if (panel1.Bounds.Contains(x, y))//判断鼠标是否移动到了panel1中
                    {
                        if (c.Equals(gbLogic)) return;//如果移动到了panel1中，并且上次移动也是在panel1中那么不做处理
                        mx = ((Label)sender).Location.X - panel1.Bounds.X;//下面这几句是当鼠标的位置切换容器时，把label1也切换控件，并且从新初始化label1的位置
                        my = ((Label)sender).Location.Y - panel1.Bounds.Y;
                        if (mx <= 0) mx = 0;
                        if (my <= 0) my = 0;
                        ((Label)sender).Location = new Point(mx, my);
                        c.Controls.Remove(((Label)sender));
                        gbLogic.Controls.Add(((Label)sender));
                        c = gbLogic;//从新记录容器
                    }
                   Byte[] oTmpPins = myLogic.MyDesign[MySelectedLogic].MyPins[ButtonTag];
                   oTmpPins[2] = Convert.ToByte(mx / 256);
                   oTmpPins[3] = Convert.ToByte(mx % 256);
                   oTmpPins[4] = Convert.ToByte(my / 256);
                   oTmpPins[5] =Convert.ToByte( my % 256);
                   myLogic.MyDesign[MySelectedLogic].MyPins[ButtonTag] = oTmpPins;
                }
            }
            catch { }
        }

        private void tmpLogicTable_Mouseup(object sender, MouseEventArgs e)
        {
            pt = new Point(0, 0);
        }

        private void frmLogic_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (CsConst.MyEditMode == 1)
            {
                if (myLogic.MyRead2UpFlags[1] == false)
                {
                    tbDown_Click(tbUpload, null);
                }
            }
            if (CsConst.calculationWorker != null && CsConst.calculationWorker.IsBusy)
            {
                CsConst.calculationWorker.Dispose();
                CsConst.calculationWorker = null;
            }
        }

        private void tmiCopy_Click(object sender, EventArgs e)
        {
            #region
            if (tvLogic.Nodes == null) return;
            if (myLogic == null) return;
            if (tvLogic.SelectedNode == null) return;
            if (tvLogic.SelectedNode.Level == 0) return;
            if (myLogic.MyDesign == null || myLogic.MyDesign.Length == 0) return;

            CsConst.MyPublicCtrls = new List<UVCMD.ControlTargets>();

            TreeNode oNode = tvLogic.SelectedNode;
            byte bytAll = 0;     //表示选择的目标个数
            if (oNode.Level == 2) // 表示单个选中的是目标
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
                UVCMD.ControlTargets oCtrls = (UVCMD.ControlTargets)myLogic.MyDesign[oNode.Parent.Parent.Index].ArmCmds[oNode.Parent.Index][oNode.Index].Clone();
                CsConst.MyPublicCtrls.Add(oCtrls);
            }
            else
            {
                for (byte bytI = 0; bytI < bytAll; bytI++)
                {
                    UVCMD.ControlTargets oCtrls = (UVCMD.ControlTargets)myLogic.MyDesign[oNode.Parent.Index].ArmCmds[oNode.Index][bytI].Clone();
                    CsConst.MyPublicCtrls.Add(oCtrls);
                }
            }
            #endregion
        }

        private void tmiPaste_Click(object sender, EventArgs e)
        {
            #region
            if (tvLogic.Nodes == null) return;
            if (MyPasteNode == null) return;
            if (CsConst.MyPublicCtrls == null) return;
            if (MyPasteNode.Level == 0) return;

            if (MyPasteNode.Level == 2) MyPasteNode = MyPasteNode.Parent;

            if (myLogic.MyDesign[MyPasteNode.Parent.Index].ArmCmds[MyPasteNode.Index] == null)
                myLogic.MyDesign[MyPasteNode.Parent.Index].ArmCmds[MyPasteNode.Index] = new List<UVCMD.ControlTargets>();

            for (int i = 0; i < CsConst.MyPublicCtrls.Count; i++)
            {
                MyPasteNode.Nodes.Add(null, CsConst.MyPublicCtrls[i].Hint, 1, 1);

                UVCMD.ControlTargets oCtrls = (UVCMD.ControlTargets)CsConst.MyPublicCtrls[i].Clone();
                myLogic.MyDesign[MyPasteNode.Parent.Index].ArmCmds[MyPasteNode.Index].Add(oCtrls);
            }
            #endregion
        }

        private void tmiDel_Click(object sender, EventArgs e)
        {
            #region
            if (tvLogic.Nodes == null) return;
            if (tvLogic.SelectedNode == null) return;
            if (tvLogic.SelectedNode.Level == 0) return;
            if (myLogic == null) return;
            if (myLogic.MyDesign == null) myLogic.MyDesign = new Logic.LogicBlock[12];

            TreeNode oNode = tvLogic.SelectedNode;

            if (oNode.Level == 1)
            {
                while (oNode.Nodes.Count > 0) oNode.Nodes.RemoveAt(0);

                myLogic.MyDesign[oNode.Parent.Index].ArmCmds[oNode.Index] = new List<UVCMD.ControlTargets>();
                return;
            }
            else
            {
                if (myLogic.MyDesign[oNode.Parent.Parent.Index].ArmCmds[oNode.Parent.Index] == null) return;
                myLogic.MyDesign[oNode.Parent.Parent.Index].ArmCmds[oNode.Parent.Index].RemoveAt(oNode.Index);
                oNode.Remove();
            }
            #endregion
        }

        private void addCommandsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (tvLogic.SelectedNode == null) return;
                if (MySelectedLogic == -1) return;
                Byte[] PageID = new Byte[4] { (Byte)MySelectedLogic ,0,0,12};
                PageID[0] =(Byte)MySelectedLogic;
                frmCmdSetup CmdSetup = new frmCmdSetup(myLogic, myDevName, MyintDeviceType, PageID);
                CmdSetup.ShowDialog();

                CmdSetup.FormClosing += new FormClosingEventHandler(frmTmp_FormClosing);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void frmTmp_FormClosing(object sender, FormClosingEventArgs e)
        {
            // throw new NotImplementedException();
            DisplayLogicBlocksInformation();
        }

        private void btnPC_Click(object sender, EventArgs e)
        {
            DateTime d1;
            d1 = DateTime.Now;
            numTime1.Value = Convert.ToDecimal(d1.Hour);
            numTime2.Value = Convert.ToDecimal(d1.Minute);
            numTime3.Value = Convert.ToDecimal(d1.Second);
            DatePicker.Text = d1.ToString();
        }

        private void DatePicker_ValueChanged(object sender, EventArgs e)
        {
            label5.Text = DatePicker.Value.DayOfWeek.ToString();
            List<string> DateStr =new List<string>(){"Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };
            if (CsConst.iLanguageId == 1)
            {
                string[] dateStr2 = new string[] {"日", "一","二","三","四","五","六"};
                label5.Text = "星期" + dateStr2[DateStr.IndexOf(DatePicker.Value.DayOfWeek.ToString())];
            }
            myLogic.DateTimeAry[6] = Convert.ToByte((DateStr.IndexOf(DatePicker.Value.DayOfWeek.ToString()) + 1));
            if (DatePicker.Value.Year >= 2000)
                myLogic.DateTimeAry[0] = Convert.ToByte((DatePicker.Value.Year - 2000));
            myLogic.DateTimeAry[1] = Convert.ToByte(DatePicker.Value.Month);
            myLogic.DateTimeAry[2] = Convert.ToByte(DatePicker.Value.Day);
        }

        private void btnRef_Click(object sender, EventArgs e)
        {
            byte[] arayTmp = new byte[0];
            if (CsConst.mySends.AddBufToSndList(arayTmp, 0xDA00, SubNetID, DeviceID, false, true, true, false))
            {
                byte bytYear = CsConst.myRevBuf[26];
                byte bytMonth = CsConst.myRevBuf[27];
                byte bytDay = CsConst.myRevBuf[28];
                byte bytHour = CsConst.myRevBuf[29];
                byte bytMinute = CsConst.myRevBuf[30];
                byte bytSecond = CsConst.myRevBuf[31];

                if (bytHour > 23) bytHour = 0;
                if (bytMinute > 59) bytMinute = 0;
                if (bytSecond > 59) bytSecond = 0;
                if (bytMonth > 12) bytMonth = 1;
                if (bytDay > 31) bytDay = 1;

                numTime1.Value = bytHour;
                numTime2.Value = bytMinute;
                numTime3.Value = bytSecond;

            }
        }

        private void btnLocation_Click(object sender, EventArgs e)
        {
            panel8.Visible = false;
            panel9.Visible = true;
            list1.Nodes.Clear();

            string strsqltmp = "";
            if (CsConst.mstrCurPath != null)
            {
                strsqltmp = CsConst.mstrCurPath;
                CsConst.mstrCurPath = null;
            }
            string strsql = "select distinct Country from defLogicAutoCity";
            OleDbDataReader dr = DataModule.SearchAResultSQLDB(strsql);
            if (dr != null)
            {
                while (dr.Read())
                {
                    list1.Nodes.Add(dr.GetValue(0).ToString());
                }
                dr.Close();
            }
            if (strsqltmp != "")
            {
                CsConst.mstrCurPath = strsqltmp;
            }
        }

        private void chb31_CheckedChanged(object sender, EventArgs e)
        {
            panel8.Visible = chb31.Checked;
            setGeographic();
        }

        private void btnselect_Click(object sender, EventArgs e)
        {
            lbcity.Text = "";
            panel9.Visible = false;
            chb31_CheckedChanged(null, null);
            if (list2.SelectedNode == null) return;
            lbcity.Text = list2.SelectedNode.Text;
            string str1 = list2.SelectedNode.Name.ToString();
            string str2 = list2.SelectedNode.ImageKey.ToString();
            string str3 = list2.SelectedNode.SelectedImageKey.ToString();

            float test = float.Parse(str1) * 60;
            string strLatitude = string.Format("{0:F0}", test);
            int shortintLatitude = Convert.ToInt32(strLatitude) / 60;
            int shortintLatitudeMinute = Convert.ToInt32(strLatitude) % 60;
            txt11.Text = Convert.ToString(Math.Abs(shortintLatitude));
            txt12.Text = Convert.ToString(Math.Abs(shortintLatitudeMinute));
            if (shortintLatitude > 0) cb11.SelectedIndex = 0;
            else cb11.SelectedIndex = 1;

            string strLongitude = string.Format("{0:F0}", float.Parse(str2) * 60);
            int smallintLongitude = Convert.ToInt32(strLongitude) / 60;
            int samllintLongitueMinute = Convert.ToInt32(strLongitude) % 60;
            txt21.Text = Convert.ToString(Math.Abs(smallintLongitude));
            txt22.Text = Convert.ToString(Math.Abs(samllintLongitueMinute));
            if (smallintLongitude > 0) cb21.SelectedIndex = 0;
            else cb21.SelectedIndex = 1;

            str3 = string.Format("{0:F0}", float.Parse(str3) * 10);
            int shortintTimeZone = Convert.ToInt32(str3) / 10;
            int shortintTimeZoneMinute = Convert.ToInt32(str3) % 10;
            if (shortintTimeZone >= 10)
                cb31.SelectedIndex = cb31.Items.IndexOf("+" + Convert.ToString(shortintTimeZone));
            else if (shortintTimeZone > 0 && shortintTimeZone < 10)
                cb31.SelectedIndex = cb31.Items.IndexOf("+0" + Convert.ToString(shortintTimeZone));
            else if (shortintTimeZone < 0 && (Math.Abs(shortintTimeZone) < 10))
                cb31.SelectedIndex = cb31.Items.IndexOf("-0" + Convert.ToString(shortintTimeZone));
            else if (shortintTimeZone == 0)
                cb31.SelectedIndex = cb31.Items.IndexOf("0" + Convert.ToString((Math.Abs(shortintTimeZone))));
            else
                cb31.SelectedIndex = cb31.Items.IndexOf(Convert.ToString(shortintTimeZone));
            txt31.Text =  string.Format("{0:F0}", Convert.ToInt32(Math.Abs(shortintTimeZoneMinute)*6));
            txt31.Text = string.Format("{0:D2}", Convert.ToInt32(txt31.Text));
        }

        private void list1_MouseDown(object sender, MouseEventArgs e)
        {
            if (list1.Nodes.Count == 0) return;
            TreeNode oNode = list1.GetNodeAt(e.Location);
            if (oNode == null) return;
            list2.Nodes.Clear();

            string strsqltmp = "";
            if (CsConst.mstrCurPath != null)
            {
                strsqltmp = CsConst.mstrCurPath;
                CsConst.mstrCurPath = null;
            }
            
            string strsql = string.Format("select * from defLogicAutoCity where Country='{0}'", oNode.Text);
            OleDbDataReader dr = DataModule.SearchAResultSQLDB(strsql);
            if (dr != null)
            {
                while (dr.Read())
                {
                    list2.Nodes.Add(dr.GetValue(3).ToString(), dr.GetValue(1).ToString(), dr.GetValue(4).ToString(),dr.GetValue(5).ToString());
                }
                dr.Close();
            }
            if (strsqltmp != "")
            {
                CsConst.mstrCurPath = strsqltmp;
            }
        }

        private void list2_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            btnselect_Click(null, null);
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            myLogic.SaveSecurityToDB();
            Cursor.Current = Cursors.Default;
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {

        }

        private void cbtype_SelectedIndexChanged(object sender, EventArgs e)
        {
            setSummerTime();
            if (cbtype.SelectedIndex == 0)
            {
                panel16.Visible = false;
            }
            else
            {
                panel16.Visible = true;
                if (cbtype.SelectedIndex == 1)
                {
                    lbMonth.Visible = true;
                    lbDay.Visible = true;
                    cb1.Visible = true;
                    cb2.Visible = true;
                    cb3.Visible = true;
                    cb5.Visible = true;
                    cb6.Visible = true;
                    cb7.Visible = true;
                    d1.Visible = false;
                    d2.Visible = false;
                    d3.Visible = false;
                    d4.Visible = false;
                }
                else if (cbtype.SelectedIndex == 2)
                {
                    lbMonth.Visible = false;
                    lbDay.Visible = false;
                    cb1.Visible = false;
                    cb2.Visible = false;
                    cb3.Visible = false;
                    cb5.Visible = false;
                    cb6.Visible = false;
                    cb7.Visible = false;

                    d1.Visible = true;
                    d2.Visible = true;
                    d3.Visible = false;
                    d4.Visible = false;
                }
                else if (cbtype.SelectedIndex == 3)
                {
                    lbMonth.Visible = false;
                    lbDay.Visible = false;
                    cb1.Visible = false;
                    cb2.Visible = false;
                    cb3.Visible = false;
                    cb5.Visible = false;
                    cb6.Visible = false;
                    cb7.Visible = false;

                    d1.Visible = false;
                    d2.Visible = false;
                    d3.Visible = true;
                    d4.Visible = true;
                }
            }
        }

        private void numTime1_ValueChanged(object sender, EventArgs e)
        {
            myLogic.DateTimeAry[3] = byte.Parse(numTime1.Value.ToString());
            myLogic.DateTimeAry[4] = byte.Parse(numTime2.Value.ToString());
            myLogic.DateTimeAry[5] = byte.Parse(numTime3.Value.ToString());
        }


        private void setGeographic()
        {
            if (isReading) return;
            int shortintLatitude;
            int shortintLatitudeMinute;
            int smallintLongitude;
            int shortintLongitueMinute;
            int shortintTimeZone,shortintTimeZoneMinute;

            shortintLatitude = Convert.ToInt32(cb11.Text + txt11.Text);
            shortintLatitudeMinute = Convert.ToInt32(cb11.Text + txt12.Text);
            smallintLongitude = Convert.ToInt32(cb21.Text + txt21.Text);
            shortintLongitueMinute = Convert.ToInt32(cb21.Text + txt22.Text);
            shortintLatitudeMinute = Convert.ToInt32(txt12.Text);
            shortintLongitueMinute = Convert.ToInt32(txt22.Text);

            shortintTimeZone = Convert.ToInt32(cb31.Text);
            if (shortintTimeZone < 0)
                shortintTimeZoneMinute = Convert.ToInt32("-" + txt31.Text);
            else
                shortintTimeZoneMinute = Convert.ToInt32(txt31.Text);

            byte[] ArayTmp = new byte[14];
            if (chb31.Checked) ArayTmp[0] = 1;
            if (rbG21.Checked) ArayTmp[1] = 1;
            else if (rbG22.Checked) ArayTmp[1] = 2;

            if (rbG31.Checked) ArayTmp[2] = 1;
            else if (rbG32.Checked) ArayTmp[2] = 2;
            else if (rbG33.Checked) ArayTmp[2] = 3;
            else if (rbG34.Checked) ArayTmp[2] = 4;
            else if (rbG35.Checked) ArayTmp[2] = 5;
            else if (rbG36.Checked) ArayTmp[2] = 6;
            else if (rbG37.Checked) ArayTmp[2] = 7;

            ArayTmp[3] = Convert.ToByte(100 + cbG31.SelectedIndex * 5);
            ArayTmp[4] = Convert.ToByte(100 + cbG32.SelectedIndex * 5);
            ArayTmp[5] = Convert.ToByte(100 + cbG33.SelectedIndex * 5);
            ArayTmp[6] = byte.Parse(cbG34.Text);
            ArayTmp[7] = byte.Parse(numG1.Value.ToString());
            ArayTmp[8] = byte.Parse(numG2.Value.ToString());

            myLogic.LogicPos[0] = Convert.ToByte(shortintLatitude);
            myLogic.LogicPos[1] = Convert.ToByte(shortintLatitudeMinute);
            myLogic.LogicPos[2] = 0;
            myLogic.LogicPos[3] = Convert.ToByte(smallintLongitude % 256);
            if (smallintLongitude < 0)
            {
                myLogic.LogicPos[2] = Convert.ToByte((smallintLongitude + 65536) / 256);
                myLogic.LogicPos[3] = Convert.ToByte((smallintLongitude + 65536) % 256);
            }
            myLogic.LogicPos[4] = Convert.ToByte(shortintLongitueMinute);
            myLogic.LogicPos[5] = Convert.ToByte(shortintTimeZone);
            myLogic.LogicPos[6] = Convert.ToByte(shortintTimeZoneMinute);
            for (int i = 0; i < 9; i++)
            {
                myLogic.LogicPos[7 + i] = ArayTmp[i];
            }
        }

        private void cb11_SelectedIndexChanged(object sender, EventArgs e)
        {
            setGeographic();
        }

        private void txt11_TextChanged(object sender, EventArgs e)
        {
            setGeographic();
        }

        private void numG1_ValueChanged(object sender, EventArgs e)
        {
            setGeographic();
        }

        private void rbG21_CheckedChanged(object sender, EventArgs e)
        {
            setGeographic();
        }

        private void setSummerTime()
        {
            if (isReading) return;
            if (cbtype.SelectedIndex < 0) return;
            myLogic.LogicSummerTime[0] = Convert.ToByte(cbtype.SelectedIndex);
            if (myLogic.LogicSummerTime[0] == 1)
            {
                myLogic.LogicSummerTime[1] = Convert.ToByte(DateTime.Now.Year / 256);
                myLogic.LogicSummerTime[2] = Convert.ToByte(DateTime.Now.Year % 256);
                myLogic.LogicSummerTime[3] = Convert.ToByte(cb1.SelectedIndex + 1);
                DateTime dt = new DateTime(DateTime.Now.Year, cb1.SelectedIndex + 1, 1);
                int num = dt.AddDays(1 - dt.Day).AddMonths(1).AddDays(-1).Day;
                List<string> DateStr =new List<string>(){ "Sunday","Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"  };
                int weekID = 0;
                for (int i = 1; i <= num; i++)
                {
                    DateTime dttemp = new DateTime(DateTime.Now.Year, cb1.SelectedIndex + 1, i);
                    if (cb3.SelectedIndex == DateStr.IndexOf(dttemp.DayOfWeek.ToString()))
                    {
                        weekID = weekID + 1;
                        myLogic.LogicSummerTime[4] = Convert.ToByte(i);
                    }
                    if (0 <= cb2.SelectedIndex && cb2.SelectedIndex <= 3)
                    {
                        if (cb2.SelectedIndex+1 == weekID)
                        {
                            break;
                        }
                    }
                }
                myLogic.LogicSummerTime[5] = Convert.ToByte(cb4.SelectedIndex);
                myLogic.LogicSummerTime[6] = Convert.ToByte(cb2.SelectedIndex);
                myLogic.LogicSummerTime[7] = Convert.ToByte(cb3.SelectedIndex);

                if (chbEnd.Checked) myLogic.LogicSummerTime[8] = 1;
                else myLogic.LogicSummerTime[8] = 0;
                myLogic.LogicSummerTime[9] = Convert.ToByte(DateTime.Now.Year / 256);
                myLogic.LogicSummerTime[10] = Convert.ToByte(DateTime.Now.Year % 256);
                myLogic.LogicSummerTime[11] = Convert.ToByte(cb5.SelectedIndex + 1);
                dt = new DateTime(DateTime.Now.Year, cb5.SelectedIndex + 1, 1);
                num = dt.AddDays(1 - dt.Day).AddMonths(1).AddDays(-1).Day;
                weekID = 0;
                for (int i = 1; i <= num; i++)
                {
                    DateTime dttemp = new DateTime(DateTime.Now.Year, cb5.SelectedIndex + 1, i);
                    if (cb7.SelectedIndex == DateStr.IndexOf(dttemp.DayOfWeek.ToString()))
                    {
                        weekID = weekID + 1;
                        myLogic.LogicSummerTime[12] = Convert.ToByte(i);
                    }
                    if (0 <= cb6.SelectedIndex && cb6.SelectedIndex <= 3)
                    {
                        if (cb6.SelectedIndex + 1 == weekID)
                        {
                            break;
                        }
                    }
                }
                myLogic.LogicSummerTime[13] = Convert.ToByte(cb8.SelectedIndex);
                myLogic.LogicSummerTime[14] = Convert.ToByte(cb6.SelectedIndex);
                myLogic.LogicSummerTime[15] = Convert.ToByte(cb7.SelectedIndex);
            }
            else if (myLogic.LogicSummerTime[0] == 2)
            {
                int year1 = 0, year2 = 0;
                int Month1 = 0, Month2 = 0;
                int Day1 = 0, Day2 = 0;
                year1 = d1.Value.Year;
                year2 = d2.Value.Year;
                Month1 = d1.Value.Month;
                Month2 = d2.Value.Month;
                Day1 = d1.Value.Day;
                Day2 = d2.Value.Day;

                myLogic.LogicSummerTime[1] = Convert.ToByte(year1 / 256);
                myLogic.LogicSummerTime[2] = Convert.ToByte(year1 % 256);
                myLogic.LogicSummerTime[3] = Convert.ToByte(Month1);
                myLogic.LogicSummerTime[4] = Convert.ToByte(Day1);
                myLogic.LogicSummerTime[5] = Convert.ToByte(cb4.SelectedIndex);
                if (chbEnd.Checked) myLogic.LogicSummerTime[8] = 1;
                else myLogic.LogicSummerTime[8] = 0;

                myLogic.LogicSummerTime[9] = Convert.ToByte(year2 / 256);
                myLogic.LogicSummerTime[10] = Convert.ToByte(year2 % 256);
                myLogic.LogicSummerTime[11] = Convert.ToByte(Month2);
                myLogic.LogicSummerTime[12] = Convert.ToByte(Day2);
                myLogic.LogicSummerTime[13] = Convert.ToByte(cb8.SelectedIndex);
            }
            else if (myLogic.LogicSummerTime[0] == 3)
            {
                int year1 = 0, year2 = 0;
                int Month1 = 0, Month2 = 0;
                int Day1 = 0, Day2 = 0;
                year1 = d3.SelectedSolarDate.Year;
                year2 = d4.SelectedSolarDate.Year;
                Month1 = d3.SelectedSolarDate.Month;
                Month2 = d4.SelectedSolarDate.Month;
                Day1 = d3.SelectedSolarDate.Day;
                Day2 = d4.SelectedSolarDate.Day;

                myLogic.LogicSummerTime[1] = Convert.ToByte(year1 / 256);
                myLogic.LogicSummerTime[2] = Convert.ToByte(year1 % 256);
                myLogic.LogicSummerTime[3] = Convert.ToByte(Month1);
                myLogic.LogicSummerTime[4] = Convert.ToByte(Day1);
                myLogic.LogicSummerTime[5] = Convert.ToByte(cb4.SelectedIndex);
                if (chbEnd.Checked) myLogic.LogicSummerTime[8] = 1;
                else myLogic.LogicSummerTime[8] = 0;

                myLogic.LogicSummerTime[9] = Convert.ToByte(year2 / 256);
                myLogic.LogicSummerTime[10] = Convert.ToByte(year2 % 256);
                myLogic.LogicSummerTime[11] = Convert.ToByte(Month2);
                myLogic.LogicSummerTime[12] = Convert.ToByte(Day2);
                myLogic.LogicSummerTime[13] = Convert.ToByte(cb8.SelectedIndex);
            }
        }

        private void d1_ValueChanged(object sender, EventArgs e)
        {
            setSummerTime();
        }

        private void d2_ValueChanged(object sender, EventArgs e)
        {
            setSummerTime();
        }

        private void tabLogic_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CsConst.MyEditMode == 1 && myLogic.MyRead2UpFlags[0] == false)
            {
                tbDown_Click(tbDown, null);
                myLogic.MyRead2UpFlags[tabLogic.SelectedIndex] = true;
            }
            else
            {
                switch (tabLogic.SelectedIndex)
                {
                    case 0: DisplayLogicBlocksInformation(); break;
                    case 1: DisplaySystemSetup(); break;
                }
            }
        }

        private void lvTemplates_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            frmLogicPin frmSet = new frmLogicPin(PublicPinTemplates);
            frmSet.FormClosing += frmSet_FormClosing;

            frmSet.ShowDialog(); 
        }

        void frmSet_FormClosing(object sender, FormClosingEventArgs e)
        {
            //throw new NotImplementedException();
            HDLSysPF.DisplayLogicTemplates(PublicPinTemplates, lvTemplates,true);
        }

        private void lvTemplates_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            
        }

        private void lvTemplates_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            if (isReading) return;

            Int32 selectPinSum = 0;
            Boolean canBeSelected = true;
            foreach (ListViewItem olv in lvTemplates.Items)
            {
                if (olv.Checked == true) selectPinSum++;
                if (selectPinSum > 4)
                {
                    canBeSelected = false;
                    break;
                }
            }
            if (canBeSelected == false) e.Item.Checked = !e.Item.Checked;
        }

        private void createTemplatesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lvTemplates_MouseDoubleClick(lvTemplates, null);
        }

        private void newOr_Click(object sender, EventArgs e)
        {
            if (MySelectedLogic == -1) return;

            ToolStripMenuItem Tmp = ((ToolStripMenuItem)sender);
            if (Tmp.Tag == null) return;
            Byte ButtonTag = Convert.ToByte(Tmp.Tag.ToString());
            // update the buffer and add a new table
            UpdateLogicTableInformationWhenAddANewOne(ButtonTag);
            // display a table
            Byte tableSumInCurrentLogic = myLogic.MyDesign[MySelectedLogic].TableIDs[0];
            Byte validtTableID = (Byte)(myLogic.MyDesign[MySelectedLogic].TableIDs[tableSumInCurrentLogic]);
            byte[] oTmpPins = myLogic.MyDesign[MySelectedLogic].MyPins[tableSumInCurrentLogic - 1];
            DisplayOneTable(oTmpPins, validtTableID,(Byte)(tableSumInCurrentLogic - 1));
  
        }

        void UpdateLogicTableInformationWhenAddANewOne(Byte relationOrAnd)
        {
            //获取未使用的ID 然后ID后开始后移为此腾位置 同时pin开始匹配 // 获取一个未使用的序列号
            #region
            List<byte> TmpAreaID = new List<byte>(); //取出所有已用的区域号
            for (int i = 0; i < myLogic.MyDesign[MySelectedLogic].TableIDs[0]; i++) TmpAreaID.Add(myLogic.MyDesign[MySelectedLogic].TableIDs[i + 1]);

            //查找位置，替换buffer
            byte bytAreaID = 1;
            while (TmpAreaID.Contains(bytAreaID))
            {
                bytAreaID++;
            }
            #endregion

            //更新缓存
            myLogic.MyDesign[MySelectedLogic].TableIDs[0] = (byte)(myLogic.MyDesign[MySelectedLogic].TableIDs[0] + 1);

            for (byte bytI = myLogic.MyDesign[MySelectedLogic].TableIDs[0]; bytI > bytAreaID; bytI--)
            {
                myLogic.MyDesign[MySelectedLogic].TableIDs[bytI] = myLogic.MyDesign[MySelectedLogic].TableIDs[bytI - 1];
            }
            myLogic.MyDesign[MySelectedLogic].TableIDs[bytAreaID] = bytAreaID;

            byte[] bytTmpPin = new byte[50];
            bytTmpPin[0] = relationOrAnd;
            bytTmpPin[2] = (byte)(((bytAreaID - 1) % 8 * 200 + 10) / 256);
            bytTmpPin[3] = (byte)(((bytAreaID - 1) % 8 * 200 + 10) % 256);

            bytTmpPin[4] = (byte)(((bytAreaID - 1) / 8 * 200 + 16) / 256);
            bytTmpPin[5] = (byte)(((bytAreaID - 1) / 8 * 200 + 16) % 256);
            Byte pinId = 0;
            foreach (ListViewItem olv in lvTemplates.Items)
            {
                if (olv.Checked == true)
                {
                    PublicPinTemplates[olv.Index].CopyTo(bytTmpPin,18 + pinId * 8);
                    pinId++;
                }
            }
            myLogic.MyDesign[MySelectedLogic].MyPins.Insert(bytAreaID - 1, bytTmpPin);
        }

        private void btnRef2_Click(object sender, EventArgs e)
        {
            isReading = true;
            tbDown_Click(tbDown, null);
        }

        private void btnSave2_Click(object sender, EventArgs e)
        {
            tbDown_Click(tbUpload, null);
        }

        private void btnSaveAndClose2_Click(object sender, EventArgs e)
        {
            btnSave2_Click(btnSave2, null);
            this.Close();
        }

        private void chbTime_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
