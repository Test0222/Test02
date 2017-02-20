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
    public partial class FrmEnviroUI : Form
    {
        private string strName;
        private int DeviceType;
        private EnviroPanel TmpEnviro;
        private byte SubnetID = 0;
        private byte DeviceID = 0;
        private BackgroundWorker MyBackGroup;
        private System.Windows.Forms.Button myPnlTemp;
        private System.Windows.Forms.ToolStripButton myTslButton;
        private int DoDragIndex = 0;
        private int PageIDForcms = 1;
        private System.Windows.Forms.Button cmsPnlTemp;
        private Button cmsBtnTemp;
        private List<byte> bytObject = new List<byte>();
        public FrmEnviroUI()
        {
            InitializeComponent();
        }

        public FrmEnviroUI(string strname, int devicetype, EnviroPanel colordlp)
        {
            InitializeComponent();
            this.TmpEnviro = colordlp;
            this.DeviceType = devicetype;
            this.strName = strname;
            string strDevName = strname.Split('\\')[0].ToString();
            SubnetID = Convert.ToByte(strDevName.Split('-')[0]);
            DeviceID = Convert.ToByte(strDevName.Split('-')[1]);
        }

        private void FrmColorDLPUI_Load(object sender, EventArgs e)
        {
            try
            {
                CsConst.isStopDealImageBackground = false;
                //ReadRemarkByBackGroup();
                byte byt = 0;
                byte SetupCount = 0;
                byte TimerCount = 0;
                byte LightCount = 0;
                byte ACCount = 0;
                byte HeatCount = 0;
                byte MusicCount = 0;
                for (byte i = 0; i < TmpEnviro.TotalPages; i++)
                {
                    byt = 0;
                    foreach (Byte bTmpIconId in TmpEnviro.IconsInPage[i].arrIconLists)
                    {
                        byt++;
                        System.Windows.Forms.Button pnl1 = this.Controls.Find("pnlP" + (i + 1).ToString() + "I" + byt.ToString(), true)[0] as System.Windows.Forms.Button;
                        pnl1.Image = GetImageWithDifferentIconsType(bTmpIconId);
                        pnl1.Text = GetCaptionWithDifferentIconsType(bTmpIconId);
                        pnl1.Tag = bTmpIconId;
                    }
                  /*  for (int j = 0; j < TmpEnviro.IconsInPage.Count; j++)
                    {
                        if (TmpEnviro.IconsInPage[j].PageID == (i + 1))
                        {
                            
                            if (TmpEnviro.IconsInPage[j].IconID == 1)
                            {
                                SetupCount++;
                                pnl1.Image = img.Images[0];
                                pnl1.Text = "Setup";
                                pnl1.Tag = TmpEnviro.IconsInPage[j].IconID;
                            }
                            else if (TmpEnviro.IconsInPage[j].IconID == 2)
                            {
                                TimerCount++;
                                pnl1.Image = img.Images[1];
                                pnl1.Text = "Timer";
                                pnl1.Tag = TmpEnviro.IconsInPage[j].IconID;
                            }
                            else if (3 <= TmpEnviro.IconsInPage[j].IconID && TmpEnviro.IconsInPage[j].IconID <= 38)
                            {
                                LightCount++;
                                pnl1.Image = img.Images[2];
                                pnl1.Text = "Light" + (TmpEnviro.IconsInPage[j].IconID - 2).ToString();
                                pnl1.Tag = TmpEnviro.IconsInPage[j].IconID;
                            }
                            else if (39 <= TmpEnviro.IconsInPage[j].IconID && TmpEnviro.IconsInPage[j].IconID <= 47)
                            {
                                ACCount++;
                                pnl1.Image = img.Images[3];
                                pnl1.Text = "AC" + (TmpEnviro.IconsInPage[j].IconID - 38).ToString();
                                pnl1.Tag = TmpEnviro.IconsInPage[j].IconID;
                            }
                            else if (48 <= TmpEnviro.IconsInPage[j].IconID && TmpEnviro.IconsInPage[j].IconID <= 56)
                            {
                                HeatCount++;
                                pnl1.Image = img.Images[4];
                                pnl1.Text = "Heat" + (TmpEnviro.IconsInPage[j].IconID - 47).ToString();
                                pnl1.Tag = TmpEnviro.IconsInPage[j].IconID;
                            }
                            else if (57 <= TmpEnviro.IconsInPage[j].IconID && TmpEnviro.IconsInPage[j].IconID <= 65)
                            {
                                MusicCount++;
                                pnl1.Image = img.Images[5];
                                pnl1.Text = "Music" + (TmpEnviro.IconsInPage[j].IconID - 56).ToString();
                                pnl1.Tag = TmpEnviro.IconsInPage[j].IconID;
                            }
                            if (byt >= 12) break;
                        }
                    }*/
                }
                 lb1.Text = "Pages:" + GetPageCount().ToString() + "/10";
                 lb2.Text = "Setup:" + SetupCount.ToString() + "/1";
                 lb3.Text = "Timer:" + TimerCount.ToString() + "/1";
                 lb4.Text = "Light:" + LightCount.ToString() + "/36";
                 lb5.Text = "AC:" + ACCount.ToString() + "/9";
                 lb6.Text = "Heat:" + HeatCount.ToString() + "/9";
                 lb7.Text = "Music:" + MusicCount.ToString() + "/9";
            }
            catch
            {
            }
        }

        private Image GetImageWithDifferentIconsType(Byte IconType)
        {
            Image TmpIcon = null;
            if (IconType == EnviroNewDeviceTypeList.SetupPage)
            {
                TmpIcon = img.Images[0];
            }
            else if (IconType == EnviroNewDeviceTypeList.Standby)
            {
                TmpIcon = img.Images[1];
            }
            else if (IconType >= EnviroNewDeviceTypeList.ButtonStart && IconType <= EnviroNewDeviceTypeList.ButtonEnd)
            {
                TmpIcon = img.Images[2];
            }
            else if (IconType >= EnviroNewDeviceTypeList.ACStart && IconType <= EnviroNewDeviceTypeList.ACEnd)
            {
                TmpIcon = img.Images[3];
            }
            else if (IconType >= EnviroNewDeviceTypeList.FHStart && IconType <= EnviroNewDeviceTypeList.FHEnd)
            {
                TmpIcon = img.Images[4];
            }
            else if (IconType >= EnviroNewDeviceTypeList.MusicStart && IconType <= EnviroNewDeviceTypeList.MusicEnd)
            {
                TmpIcon = img.Images[5];
            }
            return TmpIcon;
        }

        private String GetCaptionWithDifferentIconsType(Byte IconType)
        {
            String TmpIcon = "";
            try
            {
                if (IconType == EnviroNewDeviceTypeList.SetupPage)
                {
                    TmpIcon = "Setup";
                }
                else if (IconType == EnviroNewDeviceTypeList.Standby)
                {
                    TmpIcon = "Timer";
                }
                else if (IconType >= EnviroNewDeviceTypeList.ButtonStart && IconType <= EnviroNewDeviceTypeList.ButtonEnd)
                {
                    TmpIcon = "Light";
                }
                else if (IconType >= EnviroNewDeviceTypeList.ACStart && IconType <= EnviroNewDeviceTypeList.ACEnd)
                {
                    TmpIcon = "AC";
                }
                else if (IconType >= EnviroNewDeviceTypeList.FHStart && IconType <= EnviroNewDeviceTypeList.FHEnd)
                {
                    TmpIcon = "Heat";
                }
                else if (IconType >= EnviroNewDeviceTypeList.MusicStart && IconType <= EnviroNewDeviceTypeList.MusicEnd)
                {
                    TmpIcon = "Music";
                }
            }
            catch { }
            return TmpIcon;
        }

        private void pnlP1I1_MouseHover(object sender, EventArgs e)
        {
            try
            {
                byte Tag = Convert.ToByte((sender as System.Windows.Forms.Button).Tag);
                for (int i = 0; i < TmpEnviro.IconsInPage.Count; i++)
                {
                    foreach (Byte bTmpIcon in TmpEnviro.IconsInPage[i].arrIconLists)
                    {
                        if (bTmpIcon == Tag)
                        {
                            toolTip1.Show(TmpEnviro.IconsInPage[i].Remark, (sender as System.Windows.Forms.Button));
                            break;
                        }
                    }
                }
            }
            catch
            {
            }
        }

        private void pnlP1I1_MouseLeave(object sender, EventArgs e)
        {
            try
            {
                toolTip1.Show("", (sender as System.Windows.Forms.Button));
            }
            catch
            {
            }
        }

        private void pnlP1I1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(System.Windows.Forms.Button)) || 
                e.Data.GetDataPresent(typeof(System.Windows.Forms.ToolStripButton)))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void pnlP1I1_DragDrop(object sender, DragEventArgs e)
        {
            try
            {
                if (DoDragIndex == 0)
                {
                    if (myPnlTemp == null) return;
                    int Tag1 = Convert.ToByte(myPnlTemp.Tag);
                    int Tag2 = Convert.ToByte((sender as System.Windows.Forms.Button).Tag);
                    (sender as System.Windows.Forms.Button).Tag = Tag1;
                    myPnlTemp.Tag = Tag2;
                    string strTxt = myPnlTemp.Text;
                    Image imgTmp = myPnlTemp.Image;
                    myPnlTemp.Text = (sender as System.Windows.Forms.Button).Text;
                    myPnlTemp.Image = (sender as System.Windows.Forms.Button).Image;
                    (sender as System.Windows.Forms.Button).Image = imgTmp;
                    (sender as System.Windows.Forms.Button).Text = strTxt;
                }
                else if (DoDragIndex == 1)
                {
                    if (myTslButton == null) return;
                    int Tag1 = Convert.ToByte(myTslButton.Tag);
                    int Tag2 = Convert.ToByte((sender as System.Windows.Forms.Button).Tag);
                    if (Tag1 == 1)
                    {
                        bool isHad = false;
                        for (int i = 1; i <= 10; i++)
                        {
                            if (isHad) break;
                            for (int j = 1; j <= 12; j++)
                            {
                                System.Windows.Forms.Button pnl = this.Controls.Find("pnlP" + i.ToString() + "I" + j.ToString(), true)[0] as System.Windows.Forms.Button;
                                if (Convert.ToByte(pnl.Tag) == 1)
                                {
                                    isHad = true;
                                    break;
                                }
                            }
                        }
                        if (isHad)
                        {
                            MessageBox.Show(CsConst.mstrINIDefault.IniReadValue("Public", "99563", "")
                                , "", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                        }
                        else
                        {
                            (sender as System.Windows.Forms.Button).Image = img.Images[0];
                            (sender as System.Windows.Forms.Button).Tag = 1;
                            (sender as System.Windows.Forms.Button).Text = "Setup";
                        }
                    }
                    else if (Tag1 == 2)
                    {
                        bool isHad = false;
                        for (int i = 1; i <= 10; i++)
                        {
                            if (isHad) break;
                            for (int j = 1; j <= 12; j++)
                            {
                                System.Windows.Forms.Button pnl = this.Controls.Find("pnlP" + i.ToString() + "I" + j.ToString(), true)[0] as System.Windows.Forms.Button;
                                if (Convert.ToByte(pnl.Tag) == 2)
                                {
                                    isHad = true;
                                    break;
                                }
                            }
                        }
                        if (isHad)
                        {
                            MessageBox.Show(CsConst.mstrINIDefault.IniReadValue("Public", "99562", "")
                                , "", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                        }
                        else
                        {
                            (sender as System.Windows.Forms.Button).Image = img.Images[1];
                            (sender as System.Windows.Forms.Button).Tag = 2;
                            (sender as System.Windows.Forms.Button).Text = "Timer";
                        }
                    }
                    else if (Tag1 == 3)
                    {
                        bool isMax = false;
                        int bytTmp = 0;
                        List<byte> LightList = new List<byte>();
                        for (int i = 1; i <= 10; i++)
                        {
                            if (isMax) break;
                            for (int j = 1; j <= 12; j++)
                            {
                                System.Windows.Forms.Button pnl = this.Controls.Find("pnlP" + i.ToString() + "I" + j.ToString(), true)[0] as System.Windows.Forms.Button;
                                if (3 <= Convert.ToByte(pnl.Tag) && Convert.ToByte(pnl.Tag) <= 38)
                                {
                                    bytTmp++;
                                    LightList.Add(Convert.ToByte(pnl.Tag));
                                    if (bytTmp >= 36)
                                    {
                                        isMax = true;
                                        break;
                                    }
                                }
                            }
                        }
                        if (isMax)
                        {
                            MessageBox.Show(CsConst.mstrINIDefault.IniReadValue("Public", "99561", "")
                                , "", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                        }
                        else
                        {
                            for (byte i = 3; i <= 38; i++)
                            {
                                if (!LightList.Contains(i) && (Convert.ToInt32((sender as System.Windows.Forms.Button).Tag) > 38 ||
                                    Convert.ToInt32((sender as System.Windows.Forms.Button).Tag) < 3))
                                {
                                    (sender as System.Windows.Forms.Button).Image = img.Images[2];
                                    (sender as System.Windows.Forms.Button).Tag = i;
                                    (sender as System.Windows.Forms.Button).Text = "Light" + (i - 2).ToString();
                                    break;
                                }
                            }
                        }
                    }
                    else if (Tag1 == 39)
                    {
                        bool isMax = false;
                        int bytTmp = 0;
                        List<byte> ACList = new List<byte>();
                        for (int i = 1; i <= 10; i++)
                        {
                            if (isMax) break;
                            for (int j = 1; j <= 12; j++)
                            {
                                System.Windows.Forms.Button pnl = this.Controls.Find("pnlP" + i.ToString() + "I" + j.ToString(), true)[0] as System.Windows.Forms.Button;
                                if (39 <= Convert.ToByte(pnl.Tag) && Convert.ToByte(pnl.Tag) <= 47)
                                {
                                    bytTmp++;
                                    ACList.Add(Convert.ToByte(pnl.Tag));
                                    if (bytTmp >= 9)
                                    {
                                        isMax = true;
                                        break;
                                    }
                                }
                            }
                        }
                        if (isMax)
                        {
                            MessageBox.Show(CsConst.mstrINIDefault.IniReadValue("Public", "99560", "")
                                , "", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                        }
                        else
                        {
                            for (byte i = 39; i <= 47; i++)
                            {
                                if (!ACList.Contains(i) && (Convert.ToInt32((sender as System.Windows.Forms.Button).Tag) > 47 ||
                                    Convert.ToInt32((sender as System.Windows.Forms.Button).Tag) < 39))
                                {
                                    (sender as System.Windows.Forms.Button).Image = img.Images[3];
                                    (sender as System.Windows.Forms.Button).Tag = i;
                                    (sender as System.Windows.Forms.Button).Text = "AC" + (i - 38).ToString();
                                    break;
                                }
                            }
                        }
                    }
                    else if (Tag1 == 48)
                    {
                        bool isMax = false;
                        int bytTmp = 0;
                        List<byte> FHList = new List<byte>();
                        for (int i = 1; i <= 10; i++)
                        {
                            if (isMax) break;
                            for (int j = 1; j <= 12; j++)
                            {
                                System.Windows.Forms.Button pnl = this.Controls.Find("pnlP" + i.ToString() + "I" + j.ToString(), true)[0] as System.Windows.Forms.Button;
                                if (48 <= Convert.ToByte(pnl.Tag) && Convert.ToByte(pnl.Tag) <= 56)
                                {
                                    bytTmp++;
                                    FHList.Add(Convert.ToByte(pnl.Tag));
                                    if (bytTmp >= 9)
                                    {
                                        isMax = true;
                                        break;
                                    }
                                }
                            }
                        }
                        if (isMax)
                        {
                            MessageBox.Show(CsConst.mstrINIDefault.IniReadValue("Public", "99559", "")
                                , "", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                        }
                        else
                        {
                            for (byte i = 48; i <= 56; i++)
                            {
                                if (!FHList.Contains(i) && (Convert.ToInt32((sender as System.Windows.Forms.Button).Tag) > 56 ||
                                    Convert.ToInt32((sender as System.Windows.Forms.Button).Tag) < 48))
                                {
                                    (sender as System.Windows.Forms.Button).Image = img.Images[4];
                                    (sender as System.Windows.Forms.Button).Tag = i;
                                    (sender as System.Windows.Forms.Button).Text = "Heat" + (i - 47).ToString();
                                    break;
                                }
                            }
                        }
                    }
                    else if (Tag1 == 57)
                    {
                        bool isMax = false;
                        int bytTmp = 0;
                        List<byte> MusicList = new List<byte>();
                        for (int i = 1; i <= 10; i++)
                        {
                            if (isMax) break;
                            for (int j = 1; j <= 12; j++)
                            {
                                System.Windows.Forms.Button pnl = this.Controls.Find("pnlP" + i.ToString() + "I" + j.ToString(), true)[0] as System.Windows.Forms.Button;
                                if (57 <= Convert.ToByte(pnl.Tag) && Convert.ToByte(pnl.Tag) <= 65)
                                {
                                    bytTmp++;
                                    MusicList.Add(Convert.ToByte(pnl.Tag));
                                    if (bytTmp >= 9)
                                    {
                                        isMax = true;
                                        break;
                                    }
                                }
                            }
                        }
                        if (isMax)
                        {
                            MessageBox.Show(CsConst.mstrINIDefault.IniReadValue("Public", "99558", "")
                                , "", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                        }
                        else
                        {
                            for (byte i = 57; i <= 65; i++)
                            {
                                if (!MusicList.Contains(i) && (Convert.ToInt32((sender as System.Windows.Forms.Button).Tag) > 65 ||
                                    Convert.ToInt32((sender as System.Windows.Forms.Button).Tag) < 57))
                                {
                                    (sender as System.Windows.Forms.Button).Image = img.Images[5];
                                    (sender as System.Windows.Forms.Button).Tag = i;
                                    (sender as System.Windows.Forms.Button).Text = "Music" + (i - 56).ToString();
                                    break;
                                }
                            }
                        }
                    }
                }
                lb1.Text = "Pages:" + GetPageCount().ToString() + "/10";
                lb2.Text = "Setup:" + GetSetupCount().ToString() + "/1";
                lb3.Text = "Timer:" + GetTimerCount().ToString() + "/1";
                lb4.Text = "Light:" + GetLightCount().ToString() + "/36";
                lb5.Text = "AC:" +  GetACCount().ToString() + "/9";
                lb6.Text = "Heat:" + GetFHCount().ToString() + "/9";
                lb7.Text = "Music:" + GetMusicCount().ToString() + "/9";
            }
            catch
            {
            }
        }

        private List<byte> getLightList()
        {
            List<byte> LightList = new List<byte>();
            bool isMax = false;
            int bytTmp = 0;
            for (int i = 1; i <= 10; i++)
            {
                if (isMax) break;
                for (int j = 1; j <= 12; j++)
                {
                    System.Windows.Forms.Button pnl = this.Controls.Find("pnlP" + i.ToString() + "I" + j.ToString(), true)[0] as System.Windows.Forms.Button;
                    if (3 <= Convert.ToByte(pnl.Tag) && Convert.ToByte(pnl.Tag) <= 38)
                    {
                        bytTmp++;
                        LightList.Add(Convert.ToByte(pnl.Tag));
                        if (bytTmp >= 36)
                        {
                            isMax = true;
                            break;
                        }
                    }
                }
            }
            return LightList;
        }


        private List<byte> getACList()
        {
            bool isMax = false;
            int bytTmp = 0;
            List<byte> ACList = new List<byte>();
            for (int i = 1; i <= 10; i++)
            {
                if (isMax) break;
                for (int j = 1; j <= 12; j++)
                {
                    System.Windows.Forms.Button pnl = this.Controls.Find("pnlP" + i.ToString() + "I" + j.ToString(), true)[0] as System.Windows.Forms.Button;
                    if (39 <= Convert.ToByte(pnl.Tag) && Convert.ToByte(pnl.Tag) <= 47)
                    {
                        bytTmp++;
                        ACList.Add(Convert.ToByte(pnl.Tag));
                        if (bytTmp >= 9)
                        {
                            isMax = true;
                            break;
                        }
                    }
                }
            }
            return ACList;
        }

        private List<byte> getFHList()
        {
            bool isMax = false;
            int bytTmp = 0;
            List<byte> FHList = new List<byte>();
            for (int i = 1; i <= 10; i++)
            {
                if (isMax) break;
                for (int j = 1; j <= 12; j++)
                {
                    System.Windows.Forms.Button pnl = this.Controls.Find("pnlP" + i.ToString() + "I" + j.ToString(), true)[0] as System.Windows.Forms.Button;
                    if (48 <= Convert.ToByte(pnl.Tag) && Convert.ToByte(pnl.Tag) <= 56)
                    {
                        bytTmp++;
                        FHList.Add(Convert.ToByte(pnl.Tag));
                        if (bytTmp >= 9)
                        {
                            isMax = true;
                            break;
                        }
                    }
                }
            }
            return FHList;
        }

        private List<byte> getMusicList()
        {
            bool isMax = false;
            int bytTmp = 0;
            List<byte> MusicList = new List<byte>();
            for (int i = 1; i <= 10; i++)
            {
                if (isMax) break;
                for (int j = 1; j <= 12; j++)
                {
                    System.Windows.Forms.Button pnl = this.Controls.Find("pnlP" + i.ToString() + "I" + j.ToString(), true)[0] as System.Windows.Forms.Button;
                    if (57 <= Convert.ToByte(pnl.Tag) && Convert.ToByte(pnl.Tag) <= 65)
                    {
                        bytTmp++;
                        MusicList.Add(Convert.ToByte(pnl.Tag));
                        if (bytTmp >= 9)
                        {
                            isMax = true;
                            break;
                        }
                    }
                }
            }
            return MusicList;
        }

        private void ReadRemarkByBackGroup()
        {
            MyBackGroup = new BackgroundWorker();
            MyBackGroup.DoWork += new DoWorkEventHandler(calculationWorker_DoWork);
            MyBackGroup.ProgressChanged += new ProgressChangedEventHandler(calculationWorker_ProgressChanged);
            MyBackGroup.WorkerReportsProgress = true;
            MyBackGroup.RunWorkerCompleted += new RunWorkerCompletedEventHandler(calculationWorker_RunWorkerCompleted);
            MyBackGroup.RunWorkerAsync();
            MyBackGroup.WorkerSupportsCancellation = true;
        }

        void calculationWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {

            }
            catch
            {
            }
        }

        void calculationWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                //#region
                //for (int i = 0; i < TmpEnviro.IconsInPage.Count; i++)
                //{
                //    if (TmpEnviro.MyKeys != null && TmpEnviro.MyKeys.Count > 0)
                //    {
                //        for (int j = 0; j < TmpEnviro.MyKeys.Count; j++)
                //        {
                //            if (TmpEnviro.IconsInPage[i].IconID == TmpEnviro.MyKeys[j].IconID)
                //            {
                //                TmpEnviro.IconsInPage[i].Remark = TmpEnviro.MyKeys[j].Remark;
                //            }
                //        }
                //    }

                //    if (TmpEnviro.MyAC != null && TmpEnviro.MyAC.Count > 0)
                //    {
                //        for (int j = 0; j < TmpEnviro.MyAC.Count; j++)
                //        {
                //            if (TmpEnviro.IconsInPage[i].IconID == TmpEnviro.MyAC[j].IconID)
                //            {
                //                TmpEnviro.IconsInPage[i].Remark = TmpEnviro.MyAC[j].Remark;
                //            }
                //        }
                //    }

                //    if (TmpEnviro.MyHeat != null && TmpEnviro.MyHeat.Count > 0)
                //    {
                //        for (int j = 0; j < TmpEnviro.MyHeat.Count; j++)
                //        {
                //            if (TmpEnviro.IconsInPage[i].IconID == TmpEnviro.MyHeat[j].IconID)
                //            {
                //                TmpEnviro.IconsInPage[i].Remark = TmpEnviro.MyHeat[j].Remark;
                //            }
                //        }
                //    }

                //    if (TmpEnviro.MyMusic != null && TmpEnviro.MyMusic.Count > 0)
                //    {
                //        for (int j = 0; j < TmpEnviro.MyMusic.Count; j++)
                //        {
                //            if (TmpEnviro.IconsInPage[i].IconID == TmpEnviro.MyMusic[j].IconID)
                //            {
                //                TmpEnviro.IconsInPage[i].Remark = TmpEnviro.MyMusic[j].Remark;
                //            }
                //        }
                //    }
                //}

                //for (int i = 0; i < TmpEnviro.IconsInPage.Count; i++)
                //{
                //    if (CsConst.isStopDealImageBackground) break;

                //    if (3 <= TmpEnviro.IconsInPage[i].IconID && TmpEnviro.IconsInPage[i].IconID <= 65)
                //    {
                //        if (3 <= TmpEnviro.IconsInPage[i].IconID && TmpEnviro.IconsInPage[i].IconID <= 38)
                //        {
                //            HDLButton tmp = new HDLButton();
                //            tmp.ID = Convert.ToByte(TmpEnviro.IconsInPage[i].IconID - 2);
                //            tmp.ReadButtonRemark(SubnetID, DeviceID, DeviceType, 0);
                //            TmpEnviro.IconsInPage[i].Remark = tmp.Remark;
                //        }
                //        else if (39 <= TmpEnviro.IconsInPage[i].IconID && TmpEnviro.IconsInPage[i].IconID <= 47)
                //        {
                //            EnviroAc tmp = new EnviroAc();
                //            tmp.ID = Convert.ToByte(TmpEnviro.IconsInPage[i].IconID - 39);
                //            tmp.ReadButtonRemark(SubnetID, DeviceID, DeviceType, false);
                //            TmpEnviro.IconsInPage[i].Remark = tmp.Remark;
                //        }
                //        else if (48 <= TmpEnviro.IconsInPage[i].IconID && TmpEnviro.IconsInPage[i].IconID <= 56)
                //        {
                //            EnviroFH tmp = new EnviroFH();
                //            tmp.ID = Convert.ToByte(TmpEnviro.IconsInPage[i].IconID - 48);
                //            tmp.ReadButtonRemark(SubnetID, DeviceID, DeviceType, false);
                //            TmpEnviro.IconsInPage[i].Remark = tmp.Remark;
                //        }
                //        else if (57 <= TmpEnviro.IconsInPage[i].IconID && TmpEnviro.IconsInPage[i].IconID <= 65)
                //        {
                //            EnviroMusic tmp = new EnviroMusic();
                //            tmp.ID = Convert.ToByte(TmpEnviro.IconsInPage[i].IconID - 57);
                //            tmp.ReadButtonRemark(SubnetID, DeviceID, DeviceType, false);
                //            TmpEnviro.IconsInPage[i].Remark = tmp.Remark;
                //        }
                //    }
                //}
                //#endregion
            }
            catch
            {
            }
        }

        void calculationWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            try
            {

            }
            catch
            {
            }
        }

        private void tsb1_Click(object sender, EventArgs e)
        {

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                TmpEnviro.TotalPages = Convert.ToByte(GetPageCount());
                TmpEnviro.IconsInPage = new List<EnviroPanel.IconInfo>();
                byte bytPage = 1;
                for (Byte i = 1; i <= 10; i++)
                {
                    bool isEnpty = true; ;
                    for (byte j = 1; j <= 12; j++)
                    {
                        System.Windows.Forms.Button pnl = this.Controls.Find("pnlP" + i.ToString() + "I" + j.ToString(), true)[0] as System.Windows.Forms.Button;
                        if (Convert.ToByte(pnl.Tag) != 0)
                        {
                            isEnpty = false;
                        }
                    }
                    if (!isEnpty)
                    {
                        EnviroPanel.IconInfo temp = new EnviroPanel.IconInfo();
                        temp.PageID = bytPage;
                        temp.Remark = "";
                        temp.arrIconLists = new Byte[12];
                        for (byte j = 1; j <= 12; j++)
                        {
                            System.Windows.Forms.Button pnl = this.Controls.Find("pnlP" + i.ToString() + "I" + j.ToString(), true)[0] as System.Windows.Forms.Button;
                            temp.arrIconLists[j - 1] = Convert.ToByte(pnl.Tag);                            
                        }
                        TmpEnviro.IconsInPage.Add(temp);
                        bytPage++;
                    }
                }
                TmpEnviro.ModifyColorPagesInformation(SubnetID, DeviceID, DeviceType);
            }
            catch
            {
            }
            Cursor.Current = Cursors.Default;
        }


        private void btnP1_Click(object sender, EventArgs e)
        {
            cmsBtn.Show((sender as Button), new Point((sender as Button).Left + ((sender as Button).Width - cms.Width) / 2, (sender as Button).Top - 2*cms.Height));
        }

        private void pnlP1I1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                DoDragIndex = 0;
                if (e.Clicks > 1)
                {
                    pnlP1I1_MouseDoubleClick(sender, e);
                }
                else
                {
                    System.Windows.Forms.Button Tmp = ((System.Windows.Forms.Button)sender);
                    if (Tmp == null) return;
                    myPnlTemp = Tmp;
                    ((System.Windows.Forms.Button)sender).DoDragDrop(Tmp, DragDropEffects.Copy);
                }
            }
        }

        private void FrmEnviroUI_FormClosing(object sender, FormClosingEventArgs e)
        {
            CsConst.isStopDealImageBackground = true;
        }

        private void tsb1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                DoDragIndex = 1;
                System.Windows.Forms.ToolStripButton Tmp = ((System.Windows.Forms.ToolStripButton)sender);
                if (Tmp == null) return;
                myTslButton = Tmp;
                DoDragDrop(Tmp, DragDropEffects.Copy);
            }
        }

        private void pnlP1I1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int Tag = Convert.ToByte((sender as System.Windows.Forms.Button).Tag);
            if (Tag != 0)
            {
                if (Tag == 1)
                {
                    lb2.Text = "Setup:" + "0/1";
                }
                else if (Tag == 2)
                {
                    lb3.Text = "Timer:" +  "0/1";
                }
                else if (3 <= Tag && Tag <= 38)
                {
                    lb4.Text = "Light:" + GetLightCount().ToString() + "/36";
                }
                else if (39 <= Tag && Tag <= 47)
                {
                    lb5.Text = "AC:" + GetACCount().ToString() + "/9";
                }
                else if (48 <= Tag && Tag <= 56)
                {
                    lb6.Text = "Heat:" + GetFHCount().ToString() + "/9";
                }
                else if (57 <= Tag && Tag <= 65)
                {
                    lb7.Text = "Music:" + GetMusicCount().ToString() + "/9";
                }
                (sender as System.Windows.Forms.Button).Tag = 0;
                (sender as System.Windows.Forms.Button).Image = null;
                (sender as System.Windows.Forms.Button).Text = "";
            }
        }

        private int GetPageCount()
        {
            int PageCount = 0;
            for (int i = 1; i <= 10; i++)
            {
                for (int j = 1; j <= 12; j++)
                {
                    System.Windows.Forms.Button pnl = this.Controls.Find("pnlP" + i.ToString() + "I" + j.ToString(), true)[0] as System.Windows.Forms.Button;
                    if (Convert.ToByte(pnl.Tag) != 0)
                    {
                        PageCount++;
                        break;
                    }
                }
            }
            return PageCount;
        }

        private int GetLightCount()
        {
            int LightCount = 0;
            for (int i = 1; i <= 10; i++)
            {
                if (LightCount >= 36) break;
                for (int j = 1; j <= 12; j++)
                {
                    System.Windows.Forms.Button pnl = this.Controls.Find("pnlP" + i.ToString() + "I" + j.ToString(), true)[0] as System.Windows.Forms.Button;
                    int Tag = Convert.ToByte(pnl.Tag);
                    if (3 <= Tag && Tag <= 38)
                    {
                        LightCount++;
                        if (LightCount >= 36) break;
                    }
                }
            }
            return LightCount;
        }

        private int GetACCount()
        {
            int ACCount = 0;
            for (int i = 1; i <= 10; i++)
            {
                if (ACCount >= 9) break;
                for (int j = 1; j <= 12; j++)
                {
                    System.Windows.Forms.Button pnl = this.Controls.Find("pnlP" + i.ToString() + "I" + j.ToString(), true)[0] as System.Windows.Forms.Button;
                    int Tag = Convert.ToByte(pnl.Tag);
                    if (39 <= Tag && Tag <= 47)
                    {
                        ACCount++;
                        if (ACCount >= 9) break;
                    }
                }
            }
            return ACCount;
        }

        private int GetFHCount()
        {
            int FHCount = 0;
            for (int i = 1; i <= 10; i++)
            {
                if (FHCount >= 9) break;
                for (int j = 1; j <= 12; j++)
                {
                    System.Windows.Forms.Button pnl = this.Controls.Find("pnlP" + i.ToString() + "I" + j.ToString(), true)[0] as System.Windows.Forms.Button;
                    int Tag = Convert.ToByte(pnl.Tag);
                    if (48 <= Tag && Tag <= 56)
                    {
                        FHCount++;
                        if (FHCount >= 9) break;
                    }
                }
            }
            return FHCount;
        }

        private int GetMusicCount()
        {
            int MusicCount = 0;
            for (int i = 1; i <= 10; i++)
            {
                if (MusicCount >= 9) break;
                for (int j = 1; j <= 12; j++)
                {
                    System.Windows.Forms.Button pnl = this.Controls.Find("pnlP" + i.ToString() + "I" + j.ToString(), true)[0] as System.Windows.Forms.Button;
                    int Tag = Convert.ToByte(pnl.Tag);
                    if (57 <= Tag && Tag <= 65)
                    {
                        MusicCount++;
                        if (MusicCount >= 9) break;
                    }
                }
            }
            return MusicCount;
        }

        private int GetSetupCount()
        {
            int SetupCount = 0;
            for (int i = 1; i <= 10; i++)
            {
                if (SetupCount >= 1) break;
                for (int j = 1; j <= 12; j++)
                {
                    System.Windows.Forms.Button pnl = this.Controls.Find("pnlP" + i.ToString() + "I" + j.ToString(), true)[0] as System.Windows.Forms.Button;
                    if (Convert.ToByte(pnl.Tag) == 1)
                    {
                        SetupCount++;
                        break;
                    }
                }
            }
            return SetupCount;
        }

        private int GetTimerCount()
        {
            int TimerCount = 0;
            for (int i = 1; i <= 10; i++)
            {
                if (TimerCount >= 1) break;
                for (int j = 1; j <= 12; j++)
                {
                    System.Windows.Forms.Button pnl = this.Controls.Find("pnlP" + i.ToString() + "I" + j.ToString(), true)[0] as System.Windows.Forms.Button;
                    if (Convert.ToByte(pnl.Tag) == 2)
                    {
                        TimerCount++;
                        break;
                    }
                }
            }
            return TimerCount;
        }

        private void delSel_Click(object sender, EventArgs e)
        {
            int Tag = Convert.ToByte(cmsPnlTemp.Tag);
            if (Tag != 0)
            {
                if (GetPageCount() == 1)
                {
                    bool isLastIcon = false;
                    
                    for (int i = 1; i <= 12; i++)
                    {

                    }
                    if (isLastIcon)
                    {
                        MessageBox.Show(CsConst.mstrINIDefault.IniReadValue("Public", "99557", "")
                                       , "", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                        return;
                    }
                }
                cmsPnlTemp.Tag = 0;
                cmsPnlTemp.Image = null;
                cmsPnlTemp.Text = "";
                if (Tag == 1)
                {
                    lb2.Text = "Setup:" + "0/1";
                }
                else if (Tag == 2)
                {
                    lb3.Text = "Timer:" + "0/1";
                }
                else if (3 <= Tag && Tag <= 38)
                {
                    lb4.Text = "Light:" + GetLightCount().ToString() + "/36";
                }
                else if (39 <= Tag && Tag <= 47)
                {
                    lb5.Text = "AC:" + GetACCount().ToString() + "/9";
                }
                else if (48 <= Tag && Tag <= 56)
                {
                    lb6.Text = "Heat:" + GetFHCount().ToString() + "/9";
                }
                else if (57 <= Tag && Tag <= 65)
                {
                    lb7.Text = "Music:" + GetMusicCount().ToString() + "/9";
                }
            }
        }

        private void cms_Opening(object sender, CancelEventArgs e)
        {
            cmsPnlTemp = ((sender as ContextMenuStrip).SourceControl) as System.Windows.Forms.Button;
        }

        private void tsmI1_Click(object sender, EventArgs e)
        {
            if (GetPageCount() == 1)
            {
                MessageBox.Show(CsConst.mstrINIDefault.IniReadValue("Public", "99557", "")
                               , "", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            }
            else
            {
                int Tag = Convert.ToByte(cmsBtnTemp.Tag);
                for (int i = 1; i <= 12; i++)
                {
                    System.Windows.Forms.Button pnl = this.Controls.Find("pnlP" + Tag.ToString() + "I" + i.ToString(), true)[0] as System.Windows.Forms.Button;
                    pnl.Tag = 0;
                    pnl.Image = null;
                    pnl.Text = "";
                }
            }
            
        }

        private void tsmI2_Click(object sender, EventArgs e)
        {
            try
            {
                bytObject = new List<byte>();
                int Tag = Convert.ToByte(cmsBtnTemp.Tag);
                for (int i = 1; i <= 12; i++)
                {
                    System.Windows.Forms.Button pnl = this.Controls.Find("pnlP" + Tag.ToString() + "I" + i.ToString(), true)[0] as System.Windows.Forms.Button;
                    bytObject.Add(Convert.ToByte(pnl.Tag));
                }
            }
            catch
            {
            }
        }

        private void tsmI3_Click(object sender, EventArgs e)
        {
            try
            {
                if (bytObject.Count != 12) return;
                int Tag = Convert.ToByte(cmsBtnTemp.Tag);
                for (int i = 1; i <= 12; i++)
                {
                    System.Windows.Forms.Button pnl = this.Controls.Find("pnlP" + Tag.ToString() + "I" + i.ToString(), true)[0] as System.Windows.Forms.Button;
                    if (bytObject[i - 1] == 1)
                    {
                        if (GetSetupCount() != 0)
                        {
                            pnl.Tag = 0;
                            pnl.Image = null;
                            pnl.Text = "";
                        }
                        else
                        {
                            pnl.Tag = 1;
                            pnl.Image = img.Images[0];
                            pnl.Text = "Setup";
                        }
                    }
                    else if (bytObject[i - 1] == 2)
                    {
                        if (GetTimerCount() != 0)
                        {
                            pnl.Tag = 0;
                            pnl.Image = null;
                            pnl.Text = "";
                        }
                        else
                        {
                            pnl.Tag = 2;
                            pnl.Image = img.Images[1];
                            pnl.Text = "Timer";
                        }
                    }
                    else if (3 <= bytObject[i - 1] && bytObject[i - 1] <= 38)
                    {
                        if (GetLightCount() >= 36)
                        {
                            pnl.Tag = 0;
                            pnl.Image = null;
                            pnl.Text = "";
                        }
                        else
                        {
                            List<byte> lightList=getLightList();
                            for (byte j = 3; j <= 38; j++)
                            {
                                if (!lightList.Contains(j))
                                {
                                    pnl.Tag = j;
                                    pnl.Text = "Light" + (j - 2).ToString();
                                    break;
                                }
                            }
                            pnl.Image = img.Images[2];
                        }
                    }
                    else if (39 <= bytObject[i - 1] && bytObject[i - 1] <= 47)
                    {
                        if (GetACCount() >= 9)
                        {
                            pnl.Tag = 0;
                            pnl.Image = null;
                            pnl.Text = "";
                        }
                        else
                        {
                            List<byte> ACList = getACList();
                            for (byte j = 39; j <= 47; j++)
                            {
                                if (!ACList.Contains(j))
                                {
                                    pnl.Tag = j;
                                    pnl.Text = "AC" + (j - 38).ToString();
                                    break;
                                }
                            }
                            pnl.Image = img.Images[3];
                        }
                    }
                    else if (48 <= bytObject[i - 1] && bytObject[i - 1] <= 56)
                    {
                        if (GetFHCount() >= 9)
                        {
                            pnl.Tag = 0;
                            pnl.Image = null;
                            pnl.Text = "";
                        }
                        else
                        {
                            List<byte> FHlist = getFHList();
                            for (byte j = 48; j <= 56; j++)
                            {
                                if (!FHlist.Contains(j))
                                {
                                    pnl.Tag = j;
                                    pnl.Text = "Heat" + (j - 47).ToString();
                                    break;
                                }
                            }
                            pnl.Image = img.Images[4];
                        }
                    }
                    else if (57 <= bytObject[i - 1] && bytObject[i - 1] <= 65)
                    {
                        if (GetMusicCount() >= 9)
                        {
                            pnl.Tag = 0;
                            pnl.Image = null;
                            pnl.Text = "";
                        }
                        else
                        {
                            List<byte> Musiclist = getMusicList();
                            for (byte j = 57; j <= 65; j++)
                            {
                                if (!Musiclist.Contains(j))
                                {
                                    pnl.Tag = j;
                                    pnl.Text = "Music" + (j - 56).ToString();
                                    break;
                                }
                            }
                            pnl.Image = img.Images[5];
                        }
                    }
                }
                lb1.Text = "Pages:" + GetPageCount().ToString() + "/10";
                lb2.Text = "Setup:" + GetSetupCount().ToString() + "/1";
                lb3.Text = "Timer:" + GetTimerCount().ToString() + "/1";
                lb4.Text = "Light:" + GetLightCount().ToString() + "/36";
                lb5.Text = "AC:" + GetACCount().ToString() + "/9";
                lb6.Text = "Heat:" + GetFHCount().ToString() + "/9";
                lb7.Text = "Music:" + GetMusicCount().ToString() + "/9";
            }
            catch
            {
            }
        }

        private void cmsBtn_Opening(object sender, CancelEventArgs e)
        {
            cmsBtnTemp = ((sender as ContextMenuStrip).SourceControl) as System.Windows.Forms.Button;
        }
       
    }
}
