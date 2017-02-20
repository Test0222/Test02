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
    public partial class FrmColorDLPUI : Form
    {
        private string strName;
        private int DeviceType;
        private ColorDLP oColorDLP;
        private byte SubnetID = 0;
        private byte DeviceID = 0;
        private PictureBox myPanel = null;
        public FrmColorDLPUI()
        {
            InitializeComponent();
        }
        public FrmColorDLPUI(string strname, int devicetype, ColorDLP colordlp)
        {
            InitializeComponent();
            this.oColorDLP = colordlp;
            this.DeviceType = devicetype;
            this.strName = strname;
            string strDevName = strname.Split('\\')[0].ToString();
            SubnetID = Convert.ToByte(strDevName.Split('-')[0]);
            DeviceID = Convert.ToByte(strDevName.Split('-')[1]);
            
        }

        private void FrmColorDLPUI_Load(object sender, EventArgs e)
        {
            pnlM7.Visible = (DeviceType == 179);
            for (int i = 0; i < 7; i++)
            {
                PictureBox temp1 = this.Controls.Find("PMain" + (i + 1).ToString(), true)[0] as PictureBox;
                System.Windows.Forms.Panel tmpPnl = this.Controls.Find("pnlM" + (i + 1).ToString(), true)[0] as System.Windows.Forms.Panel;
                temp1.Image.Tag = 0;
            }
            for (int i = 0; i < 6; i++)
            {
                PictureBox temp2 = this.Controls.Find("PL" + (i + 1).ToString(), true)[0] as PictureBox;
                PictureBox temp3 = this.Controls.Find("PC" + (i + 1).ToString(), true)[0] as PictureBox;
                temp2.Image.Tag = 0;
                temp3.Image.Tag = 0;
            }
            for (int i = 0; i < 2; i++)
            {
                PictureBox temp2 = this.Controls.Find("PS" + (i + 1).ToString(), true)[0] as PictureBox;
                temp2.Image.Tag = 0;
            }
            for (int i = 0; i < 9; i++)
            {
                PictureBox temp1 = this.Controls.Find("PA" + (i + 1).ToString(), true)[0] as PictureBox;
                PictureBox temp2 = this.Controls.Find("PH" + (i + 1).ToString(), true)[0] as PictureBox;
                PictureBox temp3 = this.Controls.Find("PM" + (i + 1).ToString(), true)[0] as PictureBox;
                temp1.Image.Tag = 0;
                temp2.Image.Tag = 0;
                temp3.Image.Tag = 0;
            }
            tabControl1_SelectedIndexChanged(null, null);
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            byte[] arayTmp = new byte[1];
            if (tabControl1.SelectedIndex == 1)
                arayTmp[0] = 1;
            else if (tabControl1.SelectedIndex == 2)
                arayTmp[0] = 2;
            else if (tabControl1.SelectedIndex == 3)
                arayTmp[0] = 3;
            else if (tabControl1.SelectedIndex == 4)
                arayTmp[0] = 4;
            else if (tabControl1.SelectedIndex == 5)
                arayTmp[0] = 5;
            else if (tabControl1.SelectedIndex == 6)
                arayTmp[0] = 6;
            else if (tabControl1.SelectedIndex == 7)
                arayTmp[0] = 7;
            if (CsConst.mySends.AddBufToSndList(arayTmp, 0x19B2, SubnetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == false)
            {
                Cursor.Current = Cursors.Default;
                return;
            }
            if (tabControl1.SelectedIndex == 0)
            {
                List<byte> listTmp = new List<byte>();
                int intTmp = 0;
                for (int i = 0; i < 7; i++)
                {
                    if (CsConst.myRevBuf[26 + i] != 0)
                    {
                        listTmp.Add(CsConst.myRevBuf[26 + i]);
                    }
                }
                List<byte> arayByt = new List<byte>() { 1, 2, 3, 4, 5, 6, 7 };
                if (listTmp.Count < 7)
                {
                    foreach(byte tmp in listTmp)
                        arayByt.Remove(tmp);
                }
                for (int i = 0; i < 7; i++)
                {
                    PictureBox temp = this.Controls.Find("PMain" + (i + 1).ToString(), true)[0] as PictureBox;
                    if (CsConst.myRevBuf[26 + i] != 0)
                    {
                        if (i != 6)
                        {
                            temp.Image = img.Images[CsConst.myRevBuf[26 + i]];
                            temp.Image.Tag = (CsConst.myRevBuf[26 + i]);
                            Tag = (CsConst.myRevBuf[26 + i]);
                        }
                        else
                        {
                            temp.Image = img.Images[9];
                            temp.Image.Tag = (CsConst.myRevBuf[26 + i]);
                            Tag = (CsConst.myRevBuf[26 + i]);
                        }
                    }
                    else
                    {
                        temp.Image = img.Images[0];
                        temp.Image.Tag = 0;
                        Tag = arayByt[intTmp];
                        intTmp = intTmp + 1;
                    }
                }
                for (int i = 1; i <= 7; i++)
                {
                    PictureBox tmp = this.Controls.Find("PMain" + i.ToString(), true)[0] as PictureBox;
                    Label lbTmp = this.Controls.Find("Main" + i.ToString(), true)[0] as Label;
                    if (Convert.ToInt32(tmp.Tag) == 1)
                        lbTmp.Text = "LIGHT";
                    else if (Convert.ToInt32(tmp.Tag) == 2)
                        lbTmp.Text = "AC";
                    else if (Convert.ToInt32(tmp.Tag) == 3)
                        lbTmp.Text = "CURTAIN";
                    else if (Convert.ToInt32(tmp.Tag) == 4)
                        lbTmp.Text = "HEAT";
                    else if (Convert.ToInt32(tmp.Tag) == 5)
                        lbTmp.Text = "MUSIC";
                    else if (Convert.ToInt32(tmp.Tag) == 6)
                        lbTmp.Text = "SCENES";
                    else if (Convert.ToInt32(tmp.Tag) == 7)
                        lbTmp.Text = "SENSOR";
                }
            }
            else if (tabControl1.SelectedIndex == 1)
            {
                for (int i = 0; i < 6; i++)
                {
                    PictureBox temp = this.Controls.Find("PL" + (i + 1).ToString(), true)[0] as PictureBox;
                    if (CsConst.myRevBuf[26 + i] == (i + 1))
                    {
                        temp.Image = img.Images[7];
                        temp.Image.Tag = (i + 1);
                    }
                    else
                    {
                        temp.Image = img.Images[0];
                        temp.Image.Tag = 0;
                    }
                }
            }
            else if (tabControl1.SelectedIndex == 2)
            {
                for (int i = 0; i < 9; i++)
                {
                    PictureBox temp = this.Controls.Find("PA" + (i + 1).ToString(), true)[0] as PictureBox;
                    if (CsConst.myRevBuf[26 + i] == (i + 1))
                    {
                        temp.Image = img.Images[2];
                        temp.Image.Tag = (i + 1);
                    }
                    else
                    {
                        temp.Image = img.Images[0];
                        temp.Image.Tag = 0;
                    }
                }
            }
            else if (tabControl1.SelectedIndex == 3)
            {
                for (int i = 0; i < 6; i++)
                {
                    PictureBox temp = this.Controls.Find("PC" + (i + 1).ToString(), true)[0] as PictureBox;
                    if (CsConst.myRevBuf[26 + i] == (i + 1))
                    {
                        temp.Image = img.Images[8];
                        temp.Image.Tag = (i + 1);
                    }
                    else
                    {
                        temp.Image = img.Images[0];
                        temp.Image.Tag = 0;
                    }
                }
            }
            else if (tabControl1.SelectedIndex == 4)
            {
                for (int i = 0; i < 9; i++)
                {
                    PictureBox temp = this.Controls.Find("PH" + (i + 1).ToString(), true)[0] as PictureBox;
                    if (CsConst.myRevBuf[26 + i] == (i + 1))
                    {
                        temp.Image = img.Images[4];
                        temp.Image.Tag = (i + 1);
                    }
                    else
                    {
                        temp.Image = img.Images[0];
                        temp.Image.Tag = 0;
                    }
                }
            }
            else if (tabControl1.SelectedIndex == 5)
            {
                for (int i = 0; i < 9; i++)
                {
                    PictureBox temp = this.Controls.Find("PM" + (i + 1).ToString(), true)[0] as PictureBox;
                    if (CsConst.myRevBuf[26 + i] == (i + 1))
                    {
                        temp.Image = img.Images[5];
                        temp.Image.Tag = (i + 1);
                    }
                    else
                    {
                        temp.Image = img.Images[0];
                        temp.Image.Tag = 0;
                    }
                }
            }
            else if (tabControl1.SelectedIndex == 6)
            {
                for (int i = 0; i < 2; i++)
                {
                    PictureBox temp = this.Controls.Find("PS" + (i + 1).ToString(), true)[0] as PictureBox;
                    if (CsConst.myRevBuf[26 + i] == (i + 1))
                    {
                        temp.Image = img.Images[6];
                        temp.Image.Tag = (i + 1);
                    }
                    else
                    {
                        temp.Image = img.Images[0];
                        temp.Image.Tag = 0;
                    }
                }
            }
            else if (tabControl1.SelectedIndex == 7)
            {
                for (int i = 0; i < 4; i++)
                {
                    PictureBox temp = this.Controls.Find("PR" + (i + 1).ToString(), true)[0] as PictureBox;
                    if (CsConst.myRevBuf[26 + i] == (i + 1))
                    {
                        if (Name == "PR1")
                            temp.Image = img.Images[10];
                        else if (Name == "PR2")
                            temp.Image = img.Images[11];
                        else if (Name == "PR3")
                            temp.Image = img.Images[12];
                        else if (Name == "PR4")
                            temp.Image = img.Images[13];
                        temp.Image.Tag = (i + 1);
                    }
                    else
                    {
                        temp.Image = img.Images[0];
                        temp.Image.Tag = 0;
                    }
                }
            }
            CsConst.myRevBuf=new byte[1200];
            Cursor.Current = Cursors.Default;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            byte[] arayTmp = new byte[17];
            for (byte i = 0; i < 17; i++) arayTmp[i] = i;
            if (tabControl1.SelectedIndex == 0)
            {
                arayTmp[0] = 0;
                arayTmp[8] = 0;
                arayTmp[9] = 0;
                arayTmp[10] = 0;
                for (int i = 0; i < 7; i++)
                {
                    PictureBox temp = this.Controls.Find("PMain" + (i + 1).ToString(), true)[0] as PictureBox;
                    arayTmp[i + 1] = Convert.ToByte(temp.Image.Tag);
                }
                if (DeviceType != 179)
                {
                    arayTmp[7] = 0;
                }
            }
            else if (tabControl1.SelectedIndex == 1)
            {
                arayTmp[0] = 1;
                for (int i = 0; i < 6; i++)
                {
                    PictureBox temp = this.Controls.Find("PL" + (i + 1).ToString(), true)[0] as PictureBox;
                    arayTmp[i + 1] = Convert.ToByte(temp.Image.Tag);
                }
            }
            else if (tabControl1.SelectedIndex == 2)
            {
                arayTmp[0] = 2;
                for (int i = 0; i < 9; i++)
                {
                    PictureBox temp = this.Controls.Find("PA" + (i + 1).ToString(), true)[0] as PictureBox;
                    arayTmp[i + 1] = Convert.ToByte(temp.Image.Tag);
                }
            }
            else if (tabControl1.SelectedIndex == 3)
            {
                arayTmp[0] = 3;
                for (int i = 0; i < 6; i++)
                {
                    PictureBox temp = this.Controls.Find("PC" + (i + 1).ToString(), true)[0] as PictureBox;
                    arayTmp[i + 1] = Convert.ToByte(temp.Image.Tag);
                }
            }
            else if (tabControl1.SelectedIndex == 4)
            {
                arayTmp[0] = 4;
                for (int i = 0; i < 9; i++)
                {
                    PictureBox temp = this.Controls.Find("PH" + (i + 1).ToString(), true)[0] as PictureBox;
                    arayTmp[i + 1] = Convert.ToByte(temp.Image.Tag);
                }
            }
            else if (tabControl1.SelectedIndex == 5)
            {
                arayTmp[0] = 5;
                for (int i = 0; i < 9; i++)
                {
                    PictureBox temp = this.Controls.Find("PM" + (i + 1).ToString(), true)[0] as PictureBox;
                    arayTmp[i + 1] = Convert.ToByte(temp.Image.Tag);
                }
            }
            else if (tabControl1.SelectedIndex == 6)
            {
                arayTmp[0] = 6;
                for (int i = 0; i < 2; i++)
                {
                    PictureBox temp = this.Controls.Find("PS" + (i + 1).ToString(), true)[0] as PictureBox;
                    arayTmp[i + 1] = Convert.ToByte(temp.Image.Tag);
                }
            }
            else if (tabControl1.SelectedIndex == 7)
            {
                arayTmp[0] = 7;
                for (int i = 0; i < 4; i++)
                {
                    PictureBox temp = this.Controls.Find("PR" + (i + 1).ToString(), true)[0] as PictureBox;
                    arayTmp[i + 1] = Convert.ToByte(temp.Image.Tag);
                }
            }
            if (CsConst.mySends.AddBufToSndList(arayTmp, 0x19B4, SubnetID, DeviceID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == false)
            {
                Cursor.Current = Cursors.Default;
                return;
            }
            Cursor.Current = Cursors.Default;
        }

        private void PL1_Click(object sender, EventArgs e)
        {
            PictureBox temp = sender as PictureBox;
            if (Convert.ToInt32(temp.Image.Tag) == 0)
            {
                temp.Image = img.Images[7];
                temp.Image.Tag = Convert.ToInt32(Tag);
            }
            else
            {
                temp.Image = img.Images[0];
                temp.Image.Tag = 0;
            }
        }

        private void PMain1_Click(object sender, EventArgs e)
        {
            PictureBox temp = sender as PictureBox;
            if (Convert.ToInt32(temp.Image.Tag) == 0)
            {
                if (Convert.ToInt32(Tag) == 1)
                    temp.Image = img.Images[1];
                else if (Convert.ToInt32(Tag) == 2)
                    temp.Image = img.Images[2];
                else if (Convert.ToInt32(Tag) == 3)
                    temp.Image = img.Images[3];
                else if (Convert.ToInt32(Tag) == 4)
                    temp.Image = img.Images[4];
                else if (Convert.ToInt32(Tag) == 5)
                    temp.Image = img.Images[5];
                else if (Convert.ToInt32(Tag) == 6)
                    temp.Image = img.Images[6];
                else if (Convert.ToInt32(Tag) == 7)
                    temp.Image = img.Images[9];
                temp.Image.Tag = Convert.ToInt32(Tag);
            }
            else
            {
                Tag = temp.Image.Tag;
                temp.Image = img.Images[0];
                temp.Image.Tag = 0;
            }
        }

        private void PS1_Click(object sender, EventArgs e)
        {
            PictureBox temp = sender as PictureBox;
            if (Convert.ToInt32(temp.Image.Tag) == 0)
            {
                temp.Image = img.Images[6];
                temp.Image.Tag = Convert.ToInt32(Tag);
            }
            else
            {
                temp.Image = img.Images[0];
                temp.Image.Tag = 0;
            }
        }

        private void PM1_Click(object sender, EventArgs e)
        {
            PictureBox temp = sender as PictureBox;
            if (Convert.ToInt32(temp.Image.Tag) == 0)
            {
                if (Convert.ToInt32(Tag) == 10)
                    temp.Image = img.Images[7];
                else if (Convert.ToInt32(Tag) == 12)
                    temp.Image = img.Images[8];
                else
                    temp.Image = img.Images[5];
                temp.Image.Tag = Convert.ToInt32(Tag);
            }
            else
            {
                temp.Image = img.Images[0];
                temp.Image.Tag = 0;
            }
        }

        private void PH1_Click(object sender, EventArgs e)
        {
            PictureBox temp = sender as PictureBox;
            if (Convert.ToInt32(temp.Image.Tag) == 0)
            {
                if (Convert.ToInt32(Tag) == 10)
                    temp.Image = img.Images[7];
                else if (Convert.ToInt32(Tag) == 12)
                    temp.Image = img.Images[8];
                else
                    temp.Image = img.Images[4];
                temp.Image.Tag = Convert.ToInt32(Tag);
            }
            else
            {
                temp.Image = img.Images[0];
                temp.Image.Tag = 0;
            }
        }

        private void PA1_Click(object sender, EventArgs e)
        {
            PictureBox temp = sender as PictureBox;
            if (Convert.ToInt32(temp.Image.Tag) == 0)
            {
                if (Convert.ToInt32(Tag) == 10)
                    temp.Image = img.Images[7];
                else if (Convert.ToInt32(Tag) == 12)
                    temp.Image = img.Images[8];
                else
                    temp.Image = img.Images[2];
                temp.Image.Tag = Convert.ToInt32(Tag);
            }
            else
            {
                temp.Image = img.Images[0];
                temp.Image.Tag = 0;
            }
        }

        private void PC1_Click(object sender, EventArgs e)
        {
            PictureBox temp = sender as PictureBox;
            if (Convert.ToInt32(temp.Image.Tag) == 0)
            {
                temp.Image = img.Images[8];
                temp.Image.Tag = Convert.ToInt32(Tag);
            }
            else
            {
                temp.Image = img.Images[0];
                temp.Image.Tag = 0;
            }
        }

        private void PMain1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (e.Clicks >= 2)
                {
                    PMain1_Click(sender, null);
                    return;
                }
                PictureBox Tmp = ((PictureBox)sender);
                if (Tmp == null) return;
                myPanel = Tmp;
                ((PictureBox)sender).DoDragDrop(Tmp, DragDropEffects.Copy);
            }
        }

        private void pnlM1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(PictureBox)))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void pnlM1_DragDrop(object sender, DragEventArgs e)
        {
            if (myPanel != null && myPanel.Image != null)
            {
                int Tag1 = Convert.ToInt32(myPanel.Tag);
                int Tage2 = Convert.ToInt32(myPanel.Image.Tag);
                Image img = myPanel.Image;
                PictureBox temp2 = this.Controls.Find("PMain" + (sender as System.Windows.Forms.Panel).Tag.ToString(), true)[0] as PictureBox;
                myPanel.Image = temp2.Image;
                myPanel.Image.Tag = temp2.Image.Tag;
                myPanel.Tag = temp2.Tag;

                temp2.Image = img;
                temp2.Image.Tag = Tage2;
                temp2.Tag = Tag1;


                for (int i = 1; i <= 7; i++)
                {
                    PictureBox tmp = this.Controls.Find("PMain" + i.ToString(), true)[0] as PictureBox;
                    Label lbTmp = this.Controls.Find("Main" + i.ToString(), true)[0] as Label;
                    if (Convert.ToInt32(tmp.Tag) == 1)
                        lbTmp.Text = "LIGHT";
                    else if (Convert.ToInt32(tmp.Tag) == 2)
                        lbTmp.Text = "AC";
                    else if (Convert.ToInt32(tmp.Tag) == 3)
                    {
                        lbTmp.Text = "CURTAIN";
                        lbTmp.Left = lbTmp.Left - 5;
                    }
                    else if (Convert.ToInt32(tmp.Tag) == 4)
                        lbTmp.Text = "HEAT";
                    else if (Convert.ToInt32(tmp.Tag) == 5)
                        lbTmp.Text = "MUSIC";
                    else if (Convert.ToInt32(tmp.Tag) == 6)
                        lbTmp.Text = "SCENES";
                    else if (Convert.ToInt32(tmp.Tag) == 7)
                        lbTmp.Text = "SENSOR";
                }
            }
        }

        private void PR1_Click(object sender, EventArgs e)
        {
            PictureBox temp = sender as PictureBox;
            if (Convert.ToInt32(temp.Image.Tag) == 0)
            {
                if (Name == "PR1")
                    temp.Image = img.Images[10];
                else if (Name == "PR2")
                    temp.Image = img.Images[11];
                else if (Name == "PR3")
                    temp.Image = img.Images[12];
                else if (Name == "PR4")
                    temp.Image = img.Images[13];
                temp.Image.Tag = Convert.ToInt32(Tag);
                
            }
            else
            {
                temp.Image = img.Images[0];
                temp.Image.Tag = 0;
            }
        }
    }
}
