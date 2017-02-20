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
    public partial class FrmRaw : Form
    {
        string strPath = "";
        public FrmRaw()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnCreat_Click(object sender, EventArgs e)
        {
            if (strPath == null || strPath == "") return;
            if (Pic.Image == null) return;
            Cursor.Current = Cursors.WaitCursor;
            Byte[] data = new byte[0];
            if (Pic.Image.Width == 272 && Pic.Image.Height == 480)
            {
                data = HDLPF.BackgroundColorfulImageToByte((Bitmap)Pic.Image, Pic.Image.Height, Pic.Image.Width);
            }
            else 
            {
                data = HDLPF.ColorfulImageToByte((Bitmap)Pic.Image, Pic.Image.Height, Pic.Image.Width);
            }
            System.IO.File.WriteAllBytes(strPath, data);
            Cursor.Current = Cursors.Default;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                this.strPath = saveFileDialog1.FileName;
                lbPath.Text = saveFileDialog1.FileName;
            }
        }

        private void Pic_DoubleClick(object sender, EventArgs e)
        {
            string MyPath = HDLPF.OpenFileDialog("PNG Image|*.png", "Add Icons");
            if (MyPath == null || MyPath == "") return;
            Image img = Image.FromFile(MyPath);
            (sender as PictureBox).Image = img;

            if (strPath == null || strPath == "") return;
            if (Pic.Image == null) return;
            Cursor.Current = Cursors.WaitCursor;
            Byte[] data = new byte[0];
            if (Pic.Image.Width == 272 && Pic.Image.Height == 480)
            {
                data = HDLPF.BackgroundColorfulImageToByte((Bitmap)Pic.Image, Pic.Image.Height, Pic.Image.Width);
            }
            else
            {
                data = HDLPF.ColorfulImageToByte((Bitmap)Pic.Image, Pic.Image.Height, Pic.Image.Width);
            }
            System.IO.File.WriteAllBytes(strPath, data);
            Cursor.Current = Cursors.Default;
        }
    }
}
