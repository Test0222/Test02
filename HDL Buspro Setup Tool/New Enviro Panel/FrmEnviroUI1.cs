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
        private Image myCopyIcon = null;
        private Byte myCopyID = 0;
        private Byte myCurrentPageID = 1;
        private Byte myIconIDInReal = 3;
        private List<Byte> PublicAvaibleIconArray = null;
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
            InitialFormCtrlsTextOrItems();
        }

        void InitialFormCtrlsTextOrItems()
        {
            for (int i = 0; i < 12; i++)
            {
                (this.Controls.Find("panel" + (i + 3).ToString(), true)[0] as System.Windows.Forms.Panel).AllowDrop = true;
                (this.Controls.Find("panel" + (i + 3).ToString(), true)[0] as System.Windows.Forms.Panel).DragEnter += pnlM1_DragEnter;
                (this.Controls.Find("panel" + (i + 3).ToString(), true)[0] as System.Windows.Forms.Panel).MouseDown += PMain1_MouseDown;
                (this.Controls.Find("panel" + (i + 3).ToString(), true)[0] as System.Windows.Forms.Panel).DragDrop += pnlM1_DragDrop;
                (this.Controls.Find("panel" + (i + 3).ToString(), true)[0] as System.Windows.Forms.Panel).ContextMenuStrip = cms;
            }

            PublicAvaibleIconArray = new List<Byte>();
            for (Byte i = 1; i <= 65; i++)
            {
                PublicAvaibleIconArray.Add(i);
            }
        }

        private void PMain1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                System.Windows.Forms.Panel oTmp = ((System.Windows.Forms.Panel)sender);

                myCopyIcon = oTmp.BackgroundImage;
                myCopyID = Convert.ToByte(oTmp.Tag.ToString());
                myIconIDInReal = TmpEnviro.IconsInPage[myCurrentPageID - 1][myCopyID];
                oTmp.DoDragDrop(oTmp, DragDropEffects.Copy);
            }
        }

        private void pnlM1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(System.Windows.Forms.Panel)))
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
            System.Windows.Forms.Panel oTmp = ((System.Windows.Forms.Panel)sender);

            Image myCurrentIcon = oTmp.BackgroundImage;
            Byte myCurrentID = Convert.ToByte(oTmp.Tag.ToString());
            Byte IconIDInReal = TmpEnviro.IconsInPage[myCurrentPageID - 1][myCurrentID];
            //粘贴图片
            if (myCopyIcon != null)
            {
                Image img = myCopyIcon;
                (sender as System.Windows.Forms.Panel).BackgroundImage = myCopyIcon;
                TmpEnviro.IconsInPage[myCurrentPageID - 1][myCurrentID] = myIconIDInReal;
            }
            //要不要把当前的和之前的替换
            if (myCurrentIcon != null)
            {
                oTmp = (this.Controls.Find("panel" + (myCopyID + 3).ToString(), true)[0] as System.Windows.Forms.Panel);
                oTmp.BackgroundImage = myCurrentIcon;
                TmpEnviro.IconsInPage[myCurrentPageID - 1][myCopyID] = IconIDInReal;
            }
            //更新buffer
        }

        private void FrmEnviroUI_Shown(object sender, EventArgs e)
        {
            //移除已经占用的ID
            foreach (Byte[] Tmp in TmpEnviro.IconsInPage)
            {
                foreach (Byte TmpIcon in Tmp)
                {
                    if (TmpIcon != 0) PublicAvaibleIconArray.Remove(TmpIcon);
                }
            }
            if (cboPageList.SelectedIndex == -1) cboPageList.SelectedIndex = 0;            
        }

        void DisplayIconsToPagesFromStruct(Byte CurrentPageID)
        {
            if (TmpEnviro == null) return;
            if (TmpEnviro.TotalPages == 0) return;
            if (TmpEnviro.IconsInPage == null || TmpEnviro.IconsInPage.Count ==0) return;
            Byte PageID = 1;
            myCurrentPageID = CurrentPageID;

            FirstlearImageWhenPageChanges();
            
            if (myCurrentPageID > TmpEnviro.TotalPages) //新增页面
            {
                TmpEnviro.TotalPages++;
                Byte[] Tmp = new Byte[16];
                TmpEnviro.IconsInPage.Add(Tmp);
                 //大于当前页面，则初始化页面
            }
            else   //显示
            {
                foreach (Byte[] Tmp in TmpEnviro.IconsInPage)
                {
                    if (CurrentPageID == PageID)
                    {
                        if (Tmp == null || Tmp.Length == 0) return;
                        Byte IconID = 0;
                        foreach (Byte IconType in Tmp)
                        {
                            DisplayIconToForm(IconID, IconType);
                            IconID++;
                        }
                        break;
                    }
                    PageID++;
                }
            }
        }

        void FirstlearImageWhenPageChanges()
        {
            for (int i = 0; i < 12; i++)
            {
                (this.Controls.Find("panel" + (i + 3).ToString(), true)[0] as System.Windows.Forms.Panel).BackgroundImage = null;
            }
        }

        void DisplayIconToForm(Byte IconPostion, Byte IconType)
        {
            if (IconType == 0) return;
            if (IconPostion >= 12) return;
            System.Windows.Forms.Panel TmpPanel = new System.Windows.Forms.Panel();
            TmpPanel = this.Controls.Find("panel" + (IconPostion + 3).ToString(), true)[0] as System.Windows.Forms.Panel;
            if (TmpPanel != null)
            {
                Image TmpIcon = GetImageWithDifferentIconsType(IconType);
                if (TmpIcon !=null) TmpPanel.BackgroundImage = TmpIcon;
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

        private Int32 GetUnUsedIconsIDFromPublicIconArray(Byte IconType)
        {
            Int32 TmpIcon = -1;
            if (IconType == EnviroNewDeviceTypeList.SetupPage || IconType == EnviroNewDeviceTypeList.Standby)
            {
                if (PublicAvaibleIconArray.Contains(IconType)) return IconType;
                else return -1;
            }
            else if (IconType >= EnviroNewDeviceTypeList.ButtonStart && IconType <= EnviroNewDeviceTypeList.ButtonEnd)
            {
                for (int i =  EnviroNewDeviceTypeList.ButtonStart; i <= EnviroNewDeviceTypeList.ButtonEnd;i++)
                {
                    if (PublicAvaibleIconArray.Contains((Byte)i)) return i;
                }
            }
            else if (IconType >= EnviroNewDeviceTypeList.ACStart && IconType <= EnviroNewDeviceTypeList.ACEnd)
            {
                for (int i = EnviroNewDeviceTypeList.ACStart; i <= EnviroNewDeviceTypeList.ACEnd; i++)
                {
                    if (PublicAvaibleIconArray.Contains((Byte)i)) return i;
                }
            }
            else if (IconType >= EnviroNewDeviceTypeList.FHStart && IconType <= EnviroNewDeviceTypeList.FHEnd)
            {
                for (int i = EnviroNewDeviceTypeList.FHStart; i <= EnviroNewDeviceTypeList.FHEnd; i++)
                {
                    if (PublicAvaibleIconArray.Contains((Byte)i)) return i;
                }
            }
            else if (IconType >= EnviroNewDeviceTypeList.MusicStart && IconType <= EnviroNewDeviceTypeList.MusicEnd)
            {
                for (int i = EnviroNewDeviceTypeList.MusicStart; i <= EnviroNewDeviceTypeList.MusicEnd; i++)
                {
                    if (PublicAvaibleIconArray.Contains((Byte)i)) return i;
                }
            }
            return TmpIcon;
        }

        private void cboPageList_SelectedIndexChanged(object sender, EventArgs e)
        {
            Byte PageID = (Byte)(cboPageList.SelectedIndex + 1);
            myCurrentPageID = PageID;
            DisplayIconsToPagesFromStruct(PageID);
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (cboPageList.SelectedIndex < cboPageList.Items.Count - 1) cboPageList.SelectedIndex++;
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            if (cboPageList.SelectedIndex != 0) cboPageList.SelectedIndex--;
        }

        private void tsb1_Click(object sender, EventArgs e)
        {
            ToolStripButton Tmp = ((ToolStripButton)sender);
            if (Tmp.Tag == null) return;
            Byte ButtonTag = Convert.ToByte(Tmp.Tag.ToString());
            Byte PageID = 1;
            Byte HasIconsNumber = 0;
            foreach (Byte[] IconsInPage in TmpEnviro.IconsInPage)
            {
                if (PageID == myCurrentPageID)
                {
                    if (IconsInPage !=null) 
                    {
                        foreach(Byte IconID in IconsInPage)
                        {
                            if (IconID ==0)
                            {
                                break;
                            }
                            HasIconsNumber++;
                        }
                    }
                    DisplayIconToForm(HasIconsNumber, ButtonTag);
                }
                PageID++;
            }
           // 更新当前buffer
            Int32 TmpIcon = GetUnUsedIconsIDFromPublicIconArray(ButtonTag);
            if (TmpIcon != -1)
            {
                TmpEnviro.IconsInPage[myCurrentPageID - 1][HasIconsNumber] = (Byte)TmpIcon;
                PublicAvaibleIconArray.Remove((Byte)TmpIcon);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (TmpEnviro == null) return;
            if (TmpEnviro.IconsInPage == null || TmpEnviro.IconsInPage.Count ==0) return;
 
            //更新数据
            TmpEnviro.TotalPages = 0;
            for (int PageID = 0; PageID < TmpEnviro.IconsInPage.Count; PageID++)
            {
                Byte[] IconsList = TmpEnviro.IconsInPage[PageID];
                Boolean blnNeedNewPage = false;
                Byte j =0;
                while (IconsList[j]!=0)
                {
                    blnNeedNewPage = true;
                    break;
                }
                if (blnNeedNewPage == true) TmpEnviro.TotalPages++;
                else TmpEnviro.IconsInPage.Remove(IconsList);
            }
            TmpEnviro.ModifyColorPagesInformation(SubnetID, DeviceID, DeviceType);
        }

        private void panel3_DoubleClick(object sender, EventArgs e)
        {
            
            
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Panel oTmp = (System.Windows.Forms.Panel)cms.SourceControl;
            myCopyID = Convert.ToByte(oTmp.Tag.ToString());
            oTmp.BackgroundImage = null;
            TmpEnviro.IconsInPage[myCurrentPageID - 1][myCopyID] = 0;
        }
    }
}
