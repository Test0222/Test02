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
    public partial class FrmSignal : Form
    {
        private int SendCount = 0;
        private byte bytSubID;
        private byte bytDevID;
        FrmProcess frmTmp;

        public FrmSignal()
        {
            InitializeComponent();
        }

        private void FrmSingnal_Load(object sender, EventArgs e)
        {
            if (CsConst.myOnlines != null && CsConst.myOnlines.Count > 0)
            {
                dgvDevice.Rows.Clear();
                for (int i = 0; i < CsConst.myOnlines.Count; i++)
                {
                    if (CsConst.minAllWirelessDeviceType.Contains(CsConst.myOnlines[i].DeviceType))
                    {
                        object[] obj = new object[] { dgvDevice.RowCount+1,CsConst.myOnlines[i].bytSub.ToString(),
                    CsConst.myOnlines[i].bytDev.ToString(),CsConst.myOnlines[i].DevName.Split('\\')[1].ToString(),
                    CsConst.mstrINIDefault.IniReadValue("DeviceType" + CsConst.myOnlines[i].DeviceType.ToString(), "Description", ""),
                    CsConst.mstrINIDefault.IniReadValue("DeviceType" + CsConst.myOnlines[i].DeviceType.ToString(), "Model", ""),
                    CsConst.myOnlines[i].DeviceType.ToString(),CsConst.myOnlines[i].intDIndex.ToString()};
                        dgvDevice.Rows.Add(obj);
                    }
                }
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                #region
                Cursor.Current = Cursors.WaitCursor;
                CsConst.FastSearch = true;
                CsConst.AddedDevicesIndexGroup = new List<int>();
                CsConst.mbytCurUsingSubNetID = 255;
                CsConst.mbytCurUsingDevNetID = 255;
                UDPReceive.ClearQueueData();
                if (CsConst.myOnlines == null) CsConst.myOnlines = new List<DevOnLine>();
                DateTime d1, d2;
                int intCMD = 0x000E;
                CsConst.mySends.AddBufToSndList(null, intCMD, 255, 255, false, false, false, false);
                d1 = DateTime.Now;
                d2 = DateTime.Now;

                if (UDPReceive.receiveQueue.Count > 0)
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
                    for (int i = 0; i < UDPReceive.receiveQueue.Count; i++)
                    {
                        byte[] readData = UDPReceive.receiveQueue.ToArray()[i];
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
                Cursor.Current = Cursors.Default;
            
                dgvDevice.Rows.Clear();
                if (CsConst.myOnlines != null && CsConst.myOnlines.Count > 0)
                {
                    for (int i = 0; i < CsConst.myOnlines.Count; i++)
                    {
                        if (CsConst.minAllWirelessDeviceType.Contains(CsConst.myOnlines[i].DeviceType))
                        {
                            object[] obj = new object[] { dgvDevice.RowCount+1,CsConst.myOnlines[i].bytSub.ToString(),
                            CsConst.myOnlines[i].bytDev.ToString(),CsConst.myOnlines[i].DevName.Split('\\')[1].ToString(),
                            CsConst.mstrINIDefault.IniReadValue("DeviceType" + CsConst.myOnlines[i].DeviceType.ToString(), "Description", ""),
                            CsConst.myOnlines[i].DeviceType.ToString(),CsConst.myOnlines[i].intDIndex.ToString(),
                            CsConst.mstrINIDefault.IniReadValue("DeviceType" + CsConst.myOnlines[i].DeviceType.ToString(), "Model", "")
                            };
                            dgvDevice.Rows.Add(obj);
                        }
                    }
                }
            #endregion
            }
            catch
            {
            }
            CsConst.FastSearch = false;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            byte[] ArayTmp = new byte[0];
            byte bytSubID = Convert.ToByte(numSub.Value);
            byte bytDevID = Convert.ToByte(numDev.Value);
            if (CsConst.mySends.AddBufToSndList(ArayTmp, 0x000E, bytSubID, bytDevID, false, true, true,false) == true)
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
                string strModel = CsConst.mstrINIDefault.IniReadValue("DeviceType" + temp.DeviceType.ToString(), "Model", "");
                object[] obj = {dgvDevice.RowCount+1, temp.bytSub.ToString(), temp.bytDev.ToString(),strRemark,
                                CsConst.mstrINIDefault.IniReadValue("DeviceType" + temp.DeviceType.ToString(),  "Description", ""),
                                strModel,temp.DeviceType.ToString(),index.ToString() };
                dgvDevice.Rows.Add(obj);
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
                CsConst.myRevBuf = new byte[1200];
            }
            Cursor.Current = Cursors.Default;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            timer1.Interval = Convert.ToInt32(txtTime.Text);
            timer1.Enabled = !timer1.Enabled;
            panel1.Enabled = !timer1.Enabled;
            NumCount.Enabled = !timer1.Enabled;
            txtTime.Enabled = !timer1.Enabled;
            btnStart.Enabled = !timer1.Enabled;
            btnExport.Enabled = !timer1.Enabled;
            dgvCommands.Text = "";
            UDPReceive.receiveQueueForSingnal.Clear();
            SendCount = 0;
            frmTmp = new FrmProcess();
            frmTmp.ShowDialog();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    string strFileName = saveFileDialog1.FileName.ToString();
                    if (!File.Exists(strFileName))
                    {
                        FileStream fs1 = new FileStream(strFileName, FileMode.Create, FileAccess.Write);//创建写入文件                 
                        StreamWriter sw = new StreamWriter(fs1);
                        for (int i = 0; i < dgvCommands.Lines.Length; i++)
                        {
                            sw.WriteLine(dgvCommands.Lines[i].ToString());//开始写入值      
                        }

                        sw.Close();
                        fs1.Close();
                    }
                    else
                    {
                        FileStream fs = new FileStream(strFileName, FileMode.Open, FileAccess.Write);
                        StreamWriter sr = new StreamWriter(fs);
                        for (int i = 0; i < dgvCommands.Lines.Length; i++)
                        {
                            sr.WriteLine(dgvCommands.Lines[i].ToString());//开始写入值      
                        }
                        sr.Close();
                        fs.Close();
                    }
                }
            }
            catch
            {
            }
            Cursor.Current = Cursors.Default;
        }

        private void txtTime_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar) || e.KeyChar == 8)
                e.Handled = false;
            else
                e.Handled = true;
        }

        private void txtTime_Leave(object sender, EventArgs e)
        {
            try
            {
                string str = txtTime.Text;
                txtTime.Text = HDLPF.IsNumStringMode(str, 1, 3600000);
                txtTime.SelectionStart = txtTime.Text.Length;
            }
            catch
            {
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                int count = Convert.ToInt32(NumCount.Value);
                if (SendCount < count)
                {
                    byte[] ArayTmp = new byte[2];
                    ArayTmp[0] = bytSubID;
                    CsConst.mySends.AddBufToSndList(ArayTmp, 0x1D50, bytSubID, bytDevID, false, false, false, false);
                    SendCount = SendCount + 1;
                }
                else
                {
                    timer1.Enabled = false;
                    panel1.Enabled = !timer1.Enabled;
                    
                    NumCount.Enabled = !timer1.Enabled;
                    txtTime.Enabled = !timer1.Enabled;
                    btnStart.Enabled = !timer1.Enabled;
                    btnExport.Enabled = !timer1.Enabled;
                    showResult();
                }
            }
            catch
            {
            }
        }

        private void dgvDevice_SelectionChanged(object sender, EventArgs e)
        {
            bytSubID = Convert.ToByte(dgvDevice[1, dgvDevice.CurrentRow.Index].Value.ToString());
            bytDevID = Convert.ToByte(dgvDevice[2, dgvDevice.CurrentRow.Index].Value.ToString());
        }

        private void showResult()
        {
            try
            {
                System.Threading.Thread.Sleep(2000);
                lbSendCount.Text = SendCount.ToString();
                lbReceiveCount.Text = UDPReceive.receiveQueueForSingnal.Count.ToString();
                lbLost.Text = (SendCount - UDPReceive.receiveQueueForSingnal.Count).ToString();
                int Singnal = 0;
                dgvCommands.Text = "";
                for (int i = 0; i < UDPReceive.receiveQueueForSingnal.Count; i++)
                {
                    byte[] Tmp = UDPReceive.receiveQueueForSingnal.ToArray()[i];
                    if (Tmp[17] == bytSubID && Tmp[18] == bytDevID)
                    {
                        string str1 = "";//长度
                        string str2 = "";//源子网ID设备ID
                        string str3 = "";//设备类型
                        string str4 = "";//操作码
                        string str5 = "";//目标子网ID设备ID
                        string str6 = "";//数据1
                        string str7 = "";//数据2
                        string str8 = "";//数据3
                        string str9 = "";//数据4
                        string str10 = "";//数据5
                        string strCRC = "";
                        str1 = Tmp[16].ToString("X2") + " ";
                        str2 = Tmp[17].ToString("X2") + " " + Tmp[18].ToString("X2") + " ";
                        str3 = Tmp[19].ToString("X2") + " " + Tmp[20].ToString("X2") + " ";
                        str4 = Tmp[21].ToString("X2") + " " + Tmp[22].ToString("X2") + " ";
                        str5 = Tmp[23].ToString("X2") + " " + Tmp[24].ToString("X2") + " ";

                        int DataCount = Tmp[16] - 11 - 6;
                        str6 = Tmp[25].ToString("X2") + " ";
                        str7 = Tmp[26].ToString("X2") + " " + Tmp[27].ToString("X2") + " ";
                        str8 = Tmp[28].ToString("X2") + " " + Tmp[29].ToString("X2") + " ";
                        str9 = Tmp[30].ToString("X2") + " ";
                        for (int j = 0; j < DataCount; j++)
                            str10 = str10 + Tmp[31 + j].ToString("X2") + " ";
                        strCRC = Tmp[25 + (Tmp[16] - 11)].ToString("X2") + " " + Tmp[25 + (Tmp[16] - 11) + 1].ToString("X2");

                        string strCMD = Tmp[21].ToString("X2") + Tmp[22].ToString("X2");
                        dgvCommands.AppendText((dgvCommands.Lines.Length + 1).ToString("D5") + "  ");
                        ShowText(str1, dgvCommands.TextLength, str1.Length, Color.Pink);
                        ShowText(str2, dgvCommands.TextLength, str2.Length, Color.Green);
                        ShowText(str3, dgvCommands.TextLength, str3.Length, Color.Blue);
                        ShowText(str4, dgvCommands.TextLength, str4.Length, Color.DarkRed);
                        ShowText(str5, dgvCommands.TextLength, str5.Length, Color.Green);
                        ShowText(str6, dgvCommands.TextLength, str6.Length, Color.Red);
                        ShowText(str7, dgvCommands.TextLength, str7.Length, Color.DarkBlue);
                        ShowText(str8, dgvCommands.TextLength, str8.Length, Color.DarkGreen);
                        ShowText(str9, dgvCommands.TextLength, str9.Length, Color.DarkOrange);
                        ShowText(str10, dgvCommands.TextLength, str10.Length, Color.Red);
                        ShowText(strCRC, dgvCommands.TextLength, strCRC.Length, Color.Black);
                        dgvCommands.AppendText("\r\n");
                        Singnal = Tmp[30] + Singnal;
                    }
                }
                lbSingal.Text = (Singnal / UDPReceive.receiveQueueForSingnal.Count).ToString();
            }
            catch
            {
            }
            frmTmp.Close();
        }

        private void ShowText(string sText, int nStart, int nLength, Color color)
        {
            dgvCommands.AppendText(sText);
            dgvCommands.Select(nStart, nLength);    // 需要修改颜色的部分             
            dgvCommands.SelectionColor = color;     // 颜色                    
        }
    }
}
