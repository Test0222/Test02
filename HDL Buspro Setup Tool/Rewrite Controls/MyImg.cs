using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace HDL_Buspro_Setup_Tool
{
    public class MyImg : PictureBox
    {
        private PictureBox Pic = new PictureBox();
        string FileName = string.Empty;

        public MyImg()
        {
            this.Controls.Add(Pic);
            this.Pic.AllowDrop = true;
            this.Pic.MouseDoubleClick += new MouseEventHandler(Pic_MouseDoubleClick);
            this.Pic.DragDrop += new DragEventHandler(DspList_DragDrop);
            this.Pic.DragEnter += new DragEventHandler(DspList_DragEnter);
            this.Pic.MouseDown += new MouseEventHandler(Pic_MouseDown);
        }

        public virtual PictureBox IMG
        {
            get
            {
                return Pic;
            }
        }

        public virtual Image oImg
        {
            get
            {
                return Pic.Image;
            }
            set
            {
                this.Pic.Image = value;
            }
        }

        void DspList_DragDrop(object sender, DragEventArgs e)
        {
            #region
            string[] tmp = e.Data.GetData(DataFormats.FileDrop, false) as string[];

            if (tmp != null)
            {
                FileName = tmp[0];
                try
                {
                    DisplayDspIcons(FileName, (sender as PictureBox));
                }
                catch (Exception)
                {
                    MessageBox.Show("文件格式不对");
                }
            }
            else
            {
                PictureBox Pic = sender as PictureBox;
                Pic.Image = e.Data.GetData(DataFormats.Bitmap) as Bitmap;
            }
            #endregion
        }

        void DspList_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                if (e.Data.GetDataPresent(DataFormats.FileDrop))
                    e.Effect = DragDropEffects.All;
                else
                    e.Effect = DragDropEffects.None;
            }
            //检查拖动的数据是否适合于目标空间类型；若不适合，则拒绝放置。
            else if (e.Data.GetDataPresent(DataFormats.Bitmap))
            {
                //检查Ctrl键是否被按住
                if (e.KeyState == (int)Keys.ControlKey)
                {
                    //若Ctrl键被按住，则设置拖放类型为拷贝
                    e.Effect = DragDropEffects.Copy;
                }
                else
                {
                    //若Ctrl键没有被按住，则设置拖放类型为移动
                    e.Effect = DragDropEffects.Move;
                }
            }
            else
            {
                //若被拖动的数据不适合于目标控件，则设置拖放类型为拒绝放置
                e.Effect = DragDropEffects.None;
            }
        }

        public static void DisplayDspIcons(string strPath, PictureBox oImg)
        {
            oImg.Image = Image.FromFile(strPath);
            FileStream fs = new FileStream(strPath, FileMode.Open, FileAccess.Read);
            oImg.Image = Image.FromStream(fs);
            fs.Dispose();
        }

        private void Pic_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            string MyPath = HDLPF.OpenFileDialog("bmp files (*.bmp)|*.bmp", "Device Picture");
            if (MyPath == null) return;
            DisplayDspIcons(MyPath, (sender as PictureBox));
        }

        void Pic_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                PictureBox picTemp = sender as PictureBox;                //判断事件源对象是否正显示了图片   
                if (picTemp.Image != null)
                {
                    //开始拖放操作，并传递要拖动的数据以及拖放的类型（是移动操作还是拷贝操作）                    
                    picTemp.DoDragDrop(picTemp.Image, DragDropEffects.Move | DragDropEffects.Copy);
                }
            }
        }

        private void InitializeComponent()
        {
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }
    }
}
