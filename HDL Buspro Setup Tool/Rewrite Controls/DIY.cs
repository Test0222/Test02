using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace HDL_Buspro_Setup_Tool
{
    public partial class DIY : UserControl
    {
        private byte MyFixedLine = 1; // 255 表示全部，其他表示行
        private String[] myString = new String[4];
      //  public int iconHeight;
      //  public int iconWidth;
        private string FileName = "";

        private Single[] MyMoveLeft = new float[] { 0, 0, 0, 0 };
        private Single[] MyMoveTop = new float[] { 0, 0, 0, 0 };
        private Font[] MyFont = new Font[] { new Font("Calibri", 9f), new Font("Calibri", 9f), new Font("Calibri", 9f), new Font("Calibri", 9f) };
        private byte[] MyBlank2No = new byte[] { 0, 0, 0, 0 };  //一次空白 一次加框

        public DIY()
        {
            InitializeComponent();
            lbF1.AllowDrop = true;
            lbF1.DragDrop += new DragEventHandler(lbF1_DragDrop);
            lbF1.DragEnter += new DragEventHandler(lbF1_DragEnter);
            lbF1.MouseDown += new MouseEventHandler(lbF1_MouseDown);
        }

        //public new int IconHeight
        //{
        //    get
        //    {
        //        iconHeight = lbF1.Height;
        //        return iconHeight;
        //    }
        //    set
        //    {
        //        iconHeight = value;
        //        lbF1.Height = iconHeight;
        //        lbF2.Height = iconHeight;
        //    }
        //}

        //public new int IconWidth
        //{
        //    get
        //    {
        //        iconWidth = lbF1.Width;
        //        return iconWidth;
        //    }
        //    set
        //    {
        //        iconWidth = value;
        //        lbF1.Width = iconWidth;
        //        lbF2.Width = iconWidth;
        //    }
        //}        

        private void txt1_TextChanged(object sender, EventArgs e)
        {
            myString = new string[4];

            for (int i = 0; i < 4; i++)
            {
                TextBox temp = (TextBox)this.Controls.Find("txt" + Convert.ToString(i + 1), true)[0];
                myString[i] = temp.Text;
            }
            WriteStringToIcons();
        }

        private void WriteStringToIcons()
        {
            Bitmap image = new Bitmap(lbF1.Width, lbF1.Height);
            lbF2.DrawToBitmap(image, new Rectangle(0, 0, lbF1.Width, lbF1.Height));
            Graphics g = Graphics.FromImage(image);

            for (int i = 0; i < 4; i++)
            {
                if (i < myString.Length)
                {
                    if (myString[i] == null) myString[i] = "";
                    string str = myString[i];
                    Font font = MyFont[i]; //字是什么样子的？

                    Brush brush = Brushes.Black; //用红色涂上我的字吧；

                    PointF point = new PointF(6 + MyMoveLeft[i], 8 + MyMoveTop[i] + i * lbF1.Height / 4); //从什么地方开始写字捏？//横着写还是竖着写呢？
                    //PointF point = new PointF(0,0); //从什么地方开始写字捏？//横着写还是竖着写呢？

                    System.Drawing.StringFormat sf = new System.Drawing.StringFormat(); //还是竖着写吧

                    sf.FormatFlags = StringFormatFlags.NoClip; //开始写咯

                    #region
                    if (chbFrame.Checked)
                    {
                        g.DrawString(str.ToString(), font, brush, point, sf);
                        DrawRoundRectangle(g, Pens.Black, new Rectangle(0, lbF1.Height / 4 * i, lbF1.Width - 1, lbF1.Height / 4), 3); 
                    }
                    g.DrawString(str.ToString(), font, brush, point, sf);
                    #endregion
  
                    lbF1.Image = image;//.MakeTransparent(Color.White);
                    
                }
            }
        }

        private void rbtnL1_Click(object sender, EventArgs e)
        {
            byte bytTag = Convert.ToByte(((Button)sender).Tag.ToString());

            if (MyBlank2No[bytTag] == 0) // 空白页面
            {
                Bitmap image = new Bitmap(lbF1.Width, lbF1.Height);

                lbF1.DrawToBitmap(image, new Rectangle(0, 0, lbF1.Width, lbF1.Height));

                Graphics g = Graphics.FromImage(image);

                g.FillRectangle(Brushes.White, 0, bytTag * lbF1.Height / 4, lbF1.Width, lbF1.Height / 4);
                lbF1.Image = image;
            }
            else if (MyBlank2No[bytTag] == 1) // 加边框
            {
                string strFileName = string.Empty;
                strFileName = Application.StartupPath + @"\DIY\G4D.bmp";
               
                Bitmap image = new Bitmap(lbF1.Width, lbF1.Height);
                lbF1.DrawToBitmap(image, new Rectangle(0, 0, lbF1.Width, lbF1.Height));
                Graphics g = Graphics.FromImage(image);
                g.DrawImage(Image.FromFile(strFileName), 0, lbF1.Height / 4 * bytTag);
                lbF1.Image = image;
            }
            lbF2.Image = lbF1.Image;
            MyBlank2No[bytTag] = (byte)((MyBlank2No[bytTag] + 1) % 2);//取反
        }

        private void chbInvert_CheckedChanged(object sender, EventArgs e)
        {
            lbF1.Image = HDLPF.ConvertToInvert(lbF1.Image);
        }

        private void chbOne1_CheckedChanged(object sender, EventArgs e)
        {
            byte bytTag = Convert.ToByte(((CheckBox)sender).Tag.ToString());

            Bitmap image = new Bitmap(lbF1.Width, lbF1.Height);
            lbF1.DrawToBitmap(image, new Rectangle(0, 0, lbF1.Width, lbF1.Height));
            Graphics g = Graphics.FromImage(image);
            g.DrawRectangle(new Pen(Color.Black), 0, 0, lbF1.Width - 1, lbF1.Height - 1);
            lbF1.Image = image;
            txt1_TextChanged(null, null);
        }

        private void btnTop_Click(object sender, EventArgs e)
        {
            if (MyFixedLine == 255) MyFixedLine = 1;
            for (int i = 0; i < 4; i++)
            {
                MyMoveTop[i] -= 1;
            }
            WriteStringToIcons();
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            if (MyFixedLine == 255) MyFixedLine = 1;
            for (int i = 0; i < 4; i++)
            {
                MyMoveTop[i] += 1;
            }
            WriteStringToIcons();
        }

        private void btnLeft_Click(object sender, EventArgs e)
        {
            if (MyFixedLine == 255) MyFixedLine = 1;
            for (int i = 0; i < 4; i++)
            {
                MyMoveLeft[i] -= 1;
            }
            WriteStringToIcons();
        }

        private void btnRight_Click(object sender, EventArgs e)
        {
            if (MyFixedLine == 255) MyFixedLine = 1;
            for (int i = 0; i < 4; i++)
            {
                MyMoveLeft[i] += 1;
            }
            WriteStringToIcons();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (MyFixedLine == 255) MyFixedLine = 1;
            for (int i = 0; i < 4; i++)
            {
                MyFont[i] = new Font(MyFont[MyFixedLine - 1].Name, (float)numericUpDown1.Value, MyFont[MyFixedLine - 1].Style);
            }
            WriteStringToIcons();
        }

        private void lbF1_DragDrop(object sender, DragEventArgs e)
        {
            string[] tmp = e.Data.GetData(DataFormats.FileDrop, false) as string[];

            if (tmp != null)
            {
                FileName = tmp[0];
                try
                {
                    Point Tmp = ((Label)sender).PointToClient(new Point(e.X, e.Y));

                    DrawIconsAccordinglyPs(sender, 1, Tmp.X, Tmp.Y, FileName);
                }
                catch (Exception)
                {
                    MessageBox.Show("文件格式不对");
                }
            }
            else
            {
                // Pic = sender as PictureBox;
                ((Label)sender).Image = e.Data.GetData(DataFormats.Bitmap) as Bitmap;
                if (((Label)sender) == lbF1) lbF2.Image = lbF1.Image;
            }
        }

        private void lbF1_DragEnter(object sender, DragEventArgs e)
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

        private void lbF1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (e.Clicks > 1)
                {
                    lbF1_MouseDoubleClick(sender, e);
                }
                else
                {
                    Label picTemp = sender as Label;                //判断事件源对象是否正显示了图片   
                    if (picTemp.Image != null)
                    {
                        //开始拖放操作，并传递要拖动的数据以及拖放的类型（是移动操作还是拷贝操作）                    
                        picTemp.DoDragDrop(picTemp.Image, DragDropEffects.Move | DragDropEffects.Copy);
                    }
                }
            }
        }

        private void lbF1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Point Tmp = e.Location;
            string MyPath = HDLPF.OpenFileDialog("bmp files (*.bmp)|*.bmp","Add Icons");
            if (MyPath == null) return;

            DrawIconsAccordinglyPs(sender, 1, Tmp.X, Tmp.Y, MyPath);
            lbF2.Image = lbF1.Image;
        }

        private void DrawIconsAccordinglyPs(object sender, byte bytType, int intX, int intY, string FileName)
        {
            if (sender == null) return;
            Bitmap image = new Bitmap(((Label)sender).Width, ((Label)sender).Height);

            ((Label)sender).DrawToBitmap(image, new Rectangle(0, 0, ((Label)sender).Width, ((Label)sender).Height));

            Graphics g = Graphics.FromImage(image);

            if (bytType == 0) //写字
            {
                string str = "Baidu"; //写什么字？

                Font font = new Font("宋体", 30f); //字是什么样子的？

                Brush brush = Brushes.Red; //用红色涂上我的字吧；

                PointF point = new PointF(10f, 10f); //从什么地方开始写字捏？  //横着写还是竖着写呢？

                System.Drawing.StringFormat sf = new System.Drawing.StringFormat(); //还是竖着写吧

                sf.FormatFlags = StringFormatFlags.DirectionVertical;  //开始写咯

                g.DrawString(str, font, brush, point, sf); //写好了，我要把我的作品收藏起来
            }
            else if (bytType == 1) //画图
            {
                Image Tmp = Image.FromFile(FileName);
                if (intY <= ((Label)sender).Height / 4)
                    g.DrawImage(Tmp, 0, 0, Tmp.Width, Tmp.Height);

                else if (intY > ((Label)sender).Height / 4 && intY <= ((Label)sender).Height / 2)
                    g.DrawImage(Image.FromFile(FileName), 0, ((Label)sender).Height / 4, Tmp.Width, Tmp.Height);

                else if (intY <= ((Label)sender).Height * 0.75 && intY > ((Label)sender).Height / 2)
                    g.DrawImage(Image.FromFile(FileName), 0, ((Label)sender).Height / 2, Tmp.Width, Tmp.Height);
                else
                    g.DrawImage(Image.FromFile(FileName), 0, ((Label)sender).Height * 3 / 4, Tmp.Width, Tmp.Height);
            }
            ((Label)sender).Image = image;
        }

        private void chbFrame_CheckedChanged(object sender, EventArgs e)
        {
            lbF2.Image = null;
        }
        
        public static void DrawRoundRectangle(Graphics g, Pen pen, Rectangle rect, int cornerRadius) 
        { 
            using (GraphicsPath path = CreateRoundedRectanglePath(rect, cornerRadius)) 
            { 
                g.DrawPath(pen, path); 
            } 
        }

        public static void FillRoundRectangle(Graphics g, Brush brush, Rectangle rect, int cornerRadius) 
        { 
            using (GraphicsPath path = CreateRoundedRectanglePath(rect, cornerRadius)) 
            {
                g.FillPath(brush, path); 
            } 
        }
        
        internal static GraphicsPath CreateRoundedRectanglePath(Rectangle rect, int cornerRadius) 
        { 
            GraphicsPath roundedRect = new GraphicsPath(); 
            roundedRect.AddArc(rect.X, rect.Y, cornerRadius * 2, cornerRadius * 2, 180, 90); 
            roundedRect.AddLine(rect.X + cornerRadius, rect.Y, rect.Right - cornerRadius * 2, rect.Y); 
            roundedRect.AddArc(rect.X + rect.Width - cornerRadius * 2, rect.Y, cornerRadius * 2, cornerRadius * 2, 270, 90); 
            roundedRect.AddLine(rect.Right, rect.Y + cornerRadius * 2, rect.Right, rect.Y + rect.Height - cornerRadius * 2); 
            roundedRect.AddArc(rect.X + rect.Width - cornerRadius * 2, rect.Y + rect.Height - cornerRadius * 2, cornerRadius * 2, cornerRadius * 2, 0, 90); 
            roundedRect.AddLine(rect.Right - cornerRadius * 2, rect.Bottom, rect.X + cornerRadius * 2, rect.Bottom); 
            roundedRect.AddArc(rect.X, rect.Bottom - cornerRadius * 2, cornerRadius * 2, cornerRadius * 2, 90, 90); 
            roundedRect.AddLine(rect.X, rect.Bottom - cornerRadius * 2, rect.X, rect.Y + cornerRadius * 2); 
            roundedRect.CloseFigure(); return roundedRect;
        }

        private void lbF1_Click(object sender, EventArgs e)
        {

        }

        private void btnFont_Click(object sender, EventArgs e)
        {
            FontDialog Fonts = new FontDialog();
            Fonts.Font = MyFont[MyFixedLine - 1];

            if (Fonts.ShowDialog() == DialogResult.OK)
            {
                for (int i = 0; i < 4; i++)
                {
                    MyFont[i] = new Font(Fonts.Font.Name, Fonts.Font.Size, Fonts.Font.Style);
                }
                // MyFont[MyFixedLine - 1] = Fonts.Font.Size;
                numericUpDown1.Value = Convert.ToDecimal(Fonts.Font.Size);
                WriteStringToIcons();
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txt1.Text = "";
            txt2.Text = "";
            txt3.Text = "";
            txt4.Text = "";
        }

        private void ClearIcons_Click(object sender, EventArgs e)
        {
            lbF1.Image = null;
            lbF2.Image = null;
        }

        private void numericUpDown1_ValueChanged_1(object sender, EventArgs e)
        {

        }
    }
}
