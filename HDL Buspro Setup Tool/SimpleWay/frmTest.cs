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
    public partial class frmTest : Form
    {
        private List<Byte[]> NeedSendOutBuffer = null;
        public frmTest()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (rbAddress.Text == null || rbAddress.Text == "") return;
            NeedSendOutBuffer = new List<Byte[]>();
            try
            {
                String[] arrDeviceLists = rbAddress.Lines;
                Byte bLineId = 0;
                foreach (String sTmp in arrDeviceLists)
                {
                    if (sTmp != null && sTmp != "")
                    {
                        String[] sAddressList = sTmp.Split(' ');
                        Byte[] bAddressList = new Byte[3];
                        Byte bTmpIndex = 0;
                        foreach (String sTmpAddress in sAddressList)
                        {
                            if (sTmpAddress != "")
                            {
                                bAddressList[bTmpIndex] = Convert.ToByte(sTmpAddress);
                            }
                            bTmpIndex++;
                        }
                        Byte[] arrTmpSendBuffer = new Byte[]{0x24,01,01,00,01,0xE5,0x49,01,0xFE,01,03,
                                                               00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,
                                                               01,01,02,0,0};
                        switch (bLineId)
                        {
                            case 0: arrTmpSendBuffer[1] = bAddressList[0];
                                    arrTmpSendBuffer[2] = bAddressList[1];
                                    arrTmpSendBuffer[31] = 1;
                                    arrTmpSendBuffer[32] = 0;
                                    arrTmpSendBuffer[33] = bAddressList[2];break;

                            case 1: arrTmpSendBuffer[1] = bAddressList[0];
                                    arrTmpSendBuffer[2] = bAddressList[1];
                                    arrTmpSendBuffer[31] = 1;
                                    arrTmpSendBuffer[32] = 1;
                                    arrTmpSendBuffer[33] = bAddressList[2]; break;

                            case 2: arrTmpSendBuffer[1] = bAddressList[0];
                                    arrTmpSendBuffer[2] = bAddressList[1];
                                    arrTmpSendBuffer[31] = 2;
                                    arrTmpSendBuffer[32] = 2;
                                    arrTmpSendBuffer[33] = bAddressList[2]; break;

                            case 3: arrTmpSendBuffer[1] = bAddressList[0];
                                    arrTmpSendBuffer[2] = bAddressList[1];
                                    arrTmpSendBuffer[31] = 4;
                                    arrTmpSendBuffer[32] = 1;
                                    arrTmpSendBuffer[33] = bAddressList[2]; break;

                            case 4: arrTmpSendBuffer[1] = bAddressList[0];
                                    arrTmpSendBuffer[2] = bAddressList[1];
                                    arrTmpSendBuffer[31] = 7;
                                    arrTmpSendBuffer[32] = 0;
                                    arrTmpSendBuffer[33] = bAddressList[2]; break;
                        }
                        int iTmpPackCrc = HDLUDP.Pack_crc(arrTmpSendBuffer, 34);
                        Byte[] PacketHead = new Byte[16 + 34 + 2];
                        PacketHead[0] = byte.Parse(CsConst.myLocalIP.Split('.')[0].ToString());
                        PacketHead[1] = byte.Parse(CsConst.myLocalIP.Split('.')[1].ToString());
                        PacketHead[2] = byte.Parse(CsConst.myLocalIP.Split('.')[2].ToString());
                        PacketHead[3] = byte.Parse(CsConst.myLocalIP.Split('.')[3].ToString());
                        byte[] Signal = System.Text.ASCIIEncoding.Default.GetBytes("HDLMIRACLE");
                        Array.Copy(Signal, 0, PacketHead, 4, 10);

                        PacketHead[14] = 0xAA;
                        PacketHead[15] = 0xAA;

                        arrTmpSendBuffer.CopyTo(PacketHead, 16);
                       // PacketHead[50] = (Byte)(iTmpPackCrc / 256);
                       // PacketHead[51] = (Byte)(iTmpPackCrc % 256);
                        NeedSendOutBuffer.Add(PacketHead);
                    }
                    bLineId++;
                }
                CsConst.bStartSimpleTesting = true;
                timer1.Enabled = true;
            }          
            catch
            { }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                while (CsConst.bStartSimpleTesting)
                {
                    #region
                    DateTime d1, d2;

                    d1 = DateTime.Now;
                    d2 = DateTime.Now;
                    while (UDPReceive.receiveQueue.Count > 0 || HDLSysPF.Compare(d2, d1) < 20000)
                    {
                        if (NeedSendOutBuffer.Count == 0)
                        {
                            timer1.Enabled = false;
                            CsConst.bStartSimpleTesting = false;
                        }
                        d2 = DateTime.Now;
                        if (UDPReceive.receiveQueue.Count > 0)
                        {
                            byte[] readData = UDPReceive.receiveQueue.Dequeue();
                            if (readData[21] * 256 + readData[22] == 0xE548)
                            {
                                if (readData[16] == 0x0D)
                                {
                                    foreach (Byte[] ArayTmp in NeedSendOutBuffer)
                                    {
                                        CsConst.mySends.SendBufToRemote(ArayTmp, CsConst.myDestIP);
                                        HDLUDP.TimeBetwnNext(10);
                                        CsConst.bStartSimpleTesting = false;
                                    }
                                }
                                else
                                {
                                    for (int I = 0; I < (readData[16] - 0x0B - 2) / 2; I++)
                                    {
                                        for (int j = 0; j < NeedSendOutBuffer.Count; j++)
                                        {
                                            if (readData[17] == NeedSendOutBuffer[j][0] && readData[18] == NeedSendOutBuffer[j][1])
                                            {
                                                NeedSendOutBuffer.RemoveAt(j);
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    #endregion
                    
                }
            }
           catch
           {}
           
        }

        private void frmTest_Load(object sender, EventArgs e)
        {

        }
    }
}
