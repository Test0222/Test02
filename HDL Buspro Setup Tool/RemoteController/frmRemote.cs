using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.Threading;
using System.Windows.Forms;
using System.Collections;
using System.IO;

namespace HDL_Buspro_Setup_Tool
{
    public partial class frmRemote : Form
    {
        UsbHidFind myUsb = new UsbHidFind();
        //Thread readThread = null;//读设备线程
        private BackgroundWorker MyBackGroup;
        string[] vendorID = new string[128];
        string[] productID = new string[128];
        string[] devicePath = new string[128];
        string[] outputID = new string[128];
        string[] outputLength = new string[128];

        int mintNowSending = -1;
        int myintUSB = -1;
        byte[] writeValueBuffer; //存放要发送的数据的缓存

        public frmRemote()
        {
            InitializeComponent();
           // LoadControlsText.DisplayTextToFormWhenFirstShow(this);
        }


        private void NstDefault_Click(object sender, EventArgs e)
        {
            for (int intI = 1; intI < 253; intI++)
            {
                string strPath = "";
                if (intI >= 1 && intI <= 56)
                {
                    if (intI % 8 == 0)
                        strPath = System.Windows.Forms.Application.StartupPath + @"\Icon\DLP\" + ((intI - 1) / 8 + 1).ToString() + "-8.bmp";
                    else
                        strPath = System.Windows.Forms.Application.StartupPath + @"\Icon\DLP\" + ((intI - 1) / 8 + 1).ToString() + "-" + (intI % 8).ToString() + ".bmp";
                }
                else if (intI >= 57 && intI <= 66)
                    strPath = System.Windows.Forms.Application.StartupPath + @"\Icon\Single\1.bmp";
                else if (intI >= 67 && intI <= 72)
                    strPath = System.Windows.Forms.Application.StartupPath + @"\Icon\Single\2.bmp";
                else if (intI >= 73 && intI <= 82)
                    strPath = System.Windows.Forms.Application.StartupPath + @"\Icon\Double\1.bmp";
                else if (intI >= 83 && intI <= 92)
                    strPath = System.Windows.Forms.Application.StartupPath + @"\Icon\Touch\1.bmp";
                else if (intI >= 93 && intI <= 188)
                    strPath = System.Windows.Forms.Application.StartupPath + @"\Icon\DLP\1.bmp";
                else
                    strPath = System.Windows.Forms.Application.StartupPath + @"\Icon\library\" + (intI - 188).ToString() + ".bmp";

                MyImg oImg = (this.Controls.Find("Img" + intI, true)[0] as MyImg);
                if (oImg != null) oImg.oImg= Image.FromFile(strPath);
            }
        }

        private void rtLines_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void tcbo1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void TsbLeft_Click(object sender, EventArgs e)
        {
            
        }

        private void TsbFont_Click(object sender, EventArgs e)
        {
        }
            

        private void toolStripButton19_Click(object sender, EventArgs e)
        {
            
        }

        private void toolStripButton20_Click(object sender, EventArgs e)
        {
            
        }

        private void NstSave_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            string str = string.Format("delete * from dbRemote where DIndex = 1");
            DataModule.ExecuteSQLDatabase(str);

            // DLP type
            string strsql = "";
            OleDbConnection conn = new OleDbConnection("");
            OleDbCommand cmd = new OleDbCommand(strsql, conn);
            byte[] ArayByt = new byte[0];
            MyImg oImg = null;
            #region
            for (int intI = 0; intI < 7; intI++)
            {
                strsql = "Insert into dbRemote(DIndex,PageID,Pic1,Pic2,Pic3,Pic4,Pic5,Pic6,Pic7,Pic8)"
                           + " values(@DIndex,@PageID,@Pic1,@Pic2,@Pic3,@Pic4,@Pic5,@Pic6,@Pic7,@Pic8)";

                //创建一个OleDbConnection对象
                conn = new OleDbConnection(DataModule.ConString + CsConst.mstrDefaultPath);
                conn.Open();

                cmd = new OleDbCommand(strsql, conn);
                ((OleDbParameter)cmd.Parameters.Add("@DIndex", OleDbType.Integer)).Value = 1;
                ((OleDbParameter)cmd.Parameters.Add("@PageID", OleDbType.Char)).Value = intI + 1;

                for (int intJ = 0; intJ < 8; intJ++)
                {
                    oImg = (this.Controls.Find("Img" + (intI * 8 + intJ + 1).ToString(), true)[0] as MyImg);
                    if (oImg != null) ArayByt = HDLPF.Pic2ByteBuf(oImg.oImg);

                    ((OleDbParameter)cmd.Parameters.Add("@Pic" + (intJ + 1), OleDbType.Binary)).Value = ArayByt;
                }
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (OleDbException exp)
                {
                    MessageBox.Show(exp.ToString());
                }
                conn.Close();
            }
            #endregion
            //single panel  1 2 3 
            #region
            strsql = "Insert into dbRemote(DIndex,PageID,Pic1) values(@DIndex,@PageID,@Pic1)";

            //创建一个OleDbConnection对象
            conn = new OleDbConnection(DataModule.ConString + CsConst.mstrDefaultPath);
            conn.Open();

            cmd = new OleDbCommand(strsql, conn);
            ((OleDbParameter)cmd.Parameters.Add("@DIndex", OleDbType.Integer)).Value = 1;
            ((OleDbParameter)cmd.Parameters.Add("@PageID", OleDbType.Char)).Value = 8;

            oImg = (this.Controls.Find("Img57", true)[0] as MyImg);
            if (oImg != null) ArayByt = HDLPF.Pic2ByteBuf(oImg.oImg);

            ((OleDbParameter)cmd.Parameters.Add("@Pic1", OleDbType.Binary)).Value = ArayByt;
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (OleDbException exp)
            {
                MessageBox.Show(exp.ToString());
            }
            conn.Close();

            strsql = "Insert into dbRemote(DIndex,PageID,Pic1,Pic2) values(@DIndex,@PageID,@Pic1,@Pic2)";
            //创建一个OleDbConnection对象
            conn = new OleDbConnection(DataModule.ConString + CsConst.mstrDefaultPath);
            conn.Open();

            cmd = new OleDbCommand(strsql, conn);
            ((OleDbParameter)cmd.Parameters.Add("@DIndex", OleDbType.Integer)).Value = 1;
            ((OleDbParameter)cmd.Parameters.Add("@PageID", OleDbType.Char)).Value = 9;

            oImg = (this.Controls.Find("Img58", true)[0] as MyImg);
            if (oImg != null) ArayByt = HDLPF.Pic2ByteBuf(oImg.oImg);

            ((OleDbParameter)cmd.Parameters.Add("@Pic1", OleDbType.Binary)).Value = ArayByt;

            oImg = (this.Controls.Find("Img59", true)[0] as MyImg);
            if (oImg != null) ArayByt = HDLPF.Pic2ByteBuf(oImg.oImg);

            ((OleDbParameter)cmd.Parameters.Add("@Pic2", OleDbType.Binary)).Value = ArayByt;
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (OleDbException exp)
            {
                MessageBox.Show(exp.ToString());
            }
            conn.Close();

            strsql = "Insert into dbRemote(DIndex,PageID,Pic1,Pic2,Pic3) values(@DIndex,@PageID,@Pic1,@Pic2,@Pic3)";
            //创建一个OleDbConnection对象
            conn = new OleDbConnection(DataModule.ConString + CsConst.mstrDefaultPath);
            conn.Open();

            cmd = new OleDbCommand(strsql, conn);
            ((OleDbParameter)cmd.Parameters.Add("@DIndex", OleDbType.Integer)).Value = 1;
            ((OleDbParameter)cmd.Parameters.Add("@PageID", OleDbType.Char)).Value = 10;

            oImg = (this.Controls.Find("Img60", true)[0] as MyImg);
            if (oImg != null) ArayByt = HDLPF.Pic2ByteBuf(oImg.oImg);

            ((OleDbParameter)cmd.Parameters.Add("@Pic1", OleDbType.Binary)).Value = ArayByt;

            oImg = (this.Controls.Find("Img61", true)[0] as MyImg);
            if (oImg != null) ArayByt = HDLPF.Pic2ByteBuf(oImg.oImg);

            ((OleDbParameter)cmd.Parameters.Add("@Pic2", OleDbType.Binary)).Value = ArayByt;

            oImg = (this.Controls.Find("Img62", true)[0] as MyImg);
            if (oImg != null) ArayByt = HDLPF.Pic2ByteBuf(oImg.oImg);

            ((OleDbParameter)cmd.Parameters.Add("@Pic3", OleDbType.Binary)).Value = ArayByt;
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (OleDbException exp)
            {
                MessageBox.Show(exp.ToString());
            }
            conn.Close();
            #endregion
            //single panel  4 6 
            #region
            strsql = "Insert into dbRemote(DIndex,PageID,Pic1,Pic2,Pic3,Pic4) values(@DIndex,@PageID,@Pic1,@Pic2,@Pic3,@Pic4)";

            //创建一个OleDbConnection对象
            conn = new OleDbConnection(DataModule.ConString + CsConst.mstrDefaultPath);
            conn.Open();

            cmd = new OleDbCommand(strsql, conn);
            ((OleDbParameter)cmd.Parameters.Add("@DIndex", OleDbType.Integer)).Value = 1;
            ((OleDbParameter)cmd.Parameters.Add("@PageID", OleDbType.Char)).Value = 11;

            oImg = (this.Controls.Find("Img63", true)[0] as MyImg);
            if (oImg != null) ArayByt = HDLPF.Pic2ByteBuf(oImg.oImg);

            ((OleDbParameter)cmd.Parameters.Add("@Pic1", OleDbType.Binary)).Value = ArayByt;

            oImg = (this.Controls.Find("Img64", true)[0] as MyImg);
            if (oImg != null) ArayByt = HDLPF.Pic2ByteBuf(oImg.oImg);

            ((OleDbParameter)cmd.Parameters.Add("@Pic2", OleDbType.Binary)).Value = ArayByt;

            oImg = (this.Controls.Find("Img65", true)[0] as MyImg);
            if (oImg != null) ArayByt = HDLPF.Pic2ByteBuf(oImg.oImg);

            ((OleDbParameter)cmd.Parameters.Add("@Pic3", OleDbType.Binary)).Value = ArayByt;

            oImg = (this.Controls.Find("Img66", true)[0] as MyImg);
            if (oImg != null) ArayByt = HDLPF.Pic2ByteBuf(oImg.oImg);
            ((OleDbParameter)cmd.Parameters.Add("@Pic4", OleDbType.Binary)).Value = ArayByt;

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (OleDbException exp)
            {
                MessageBox.Show(exp.ToString());
            }
            conn.Close();

            strsql = "Insert into dbRemote(DIndex,PageID,Pic1,Pic2,Pic3,Pic4,Pic5,Pic6) values(@DIndex,@PageID,@Pic1,@Pic2,@Pic3,@Pic4,@Pic5,@Pic6)";
            //创建一个OleDbConnection对象
            conn = new OleDbConnection(DataModule.ConString + CsConst.mstrDefaultPath);
            conn.Open();

            cmd = new OleDbCommand(strsql, conn);
            ((OleDbParameter)cmd.Parameters.Add("@DIndex", OleDbType.Integer)).Value = 1;
            ((OleDbParameter)cmd.Parameters.Add("@PageID", OleDbType.Char)).Value = 12;

            oImg = (this.Controls.Find("Img67", true)[0] as MyImg);
            if (oImg != null) ArayByt = HDLPF.Pic2ByteBuf(oImg.oImg);

            ((OleDbParameter)cmd.Parameters.Add("@Pic1", OleDbType.Binary)).Value = ArayByt;

            oImg = (this.Controls.Find("Img68", true)[0] as MyImg);
            if (oImg != null) ArayByt = HDLPF.Pic2ByteBuf(oImg.oImg);

            ((OleDbParameter)cmd.Parameters.Add("@Pic2", OleDbType.Binary)).Value = ArayByt;

            oImg = (this.Controls.Find("Img69", true)[0] as MyImg);
            if (oImg != null) ArayByt = HDLPF.Pic2ByteBuf(oImg.oImg);

            ((OleDbParameter)cmd.Parameters.Add("@Pic3", OleDbType.Binary)).Value = ArayByt;

            oImg = (this.Controls.Find("Img70", true)[0] as MyImg);
            if (oImg != null) ArayByt = HDLPF.Pic2ByteBuf(oImg.oImg);
            ((OleDbParameter)cmd.Parameters.Add("@Pic4", OleDbType.Binary)).Value = ArayByt;

            oImg = (this.Controls.Find("Img71", true)[0] as MyImg);
            if (oImg != null) ArayByt = HDLPF.Pic2ByteBuf(oImg.oImg);

            ((OleDbParameter)cmd.Parameters.Add("@Pic5", OleDbType.Binary)).Value = ArayByt;

            oImg = (this.Controls.Find("Img72", true)[0] as MyImg);
            if (oImg != null) ArayByt = HDLPF.Pic2ByteBuf(oImg.oImg);
            ((OleDbParameter)cmd.Parameters.Add("@Pic6", OleDbType.Binary)).Value = ArayByt;
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (OleDbException exp)
            {
                MessageBox.Show(exp.ToString());
            }
            conn.Close();
            #endregion

            //double panel  1 2 3  4
            #region
            strsql = "Insert into dbRemote(DIndex,PageID,Pic1) values(@DIndex,@PageID,@Pic1)";

            //创建一个OleDbConnection对象
            conn = new OleDbConnection(DataModule.ConString + CsConst.mstrDefaultPath);
            conn.Open();

            cmd = new OleDbCommand(strsql, conn);
            ((OleDbParameter)cmd.Parameters.Add("@DIndex", OleDbType.Integer)).Value = 1;
            ((OleDbParameter)cmd.Parameters.Add("@PageID", OleDbType.Char)).Value = 13;

            oImg = (this.Controls.Find("Img73", true)[0] as MyImg);
            if (oImg != null) ArayByt = HDLPF.Pic2ByteBuf(oImg.oImg);

            ((OleDbParameter)cmd.Parameters.Add("@Pic1", OleDbType.Binary)).Value = ArayByt;
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (OleDbException exp)
            {
                MessageBox.Show(exp.ToString());
            }
            conn.Close();

            strsql = "Insert into dbRemote(DIndex,PageID,Pic1,Pic2) values(@DIndex,@PageID,@Pic1,@Pic2)";
            //创建一个OleDbConnection对象
            conn = new OleDbConnection(DataModule.ConString + CsConst.mstrDefaultPath);
            conn.Open();

            cmd = new OleDbCommand(strsql, conn);
            ((OleDbParameter)cmd.Parameters.Add("@DIndex", OleDbType.Integer)).Value = 1;
            ((OleDbParameter)cmd.Parameters.Add("@PageID", OleDbType.Char)).Value = 14;

            oImg = (this.Controls.Find("Img74", true)[0] as MyImg);
            if (oImg != null) ArayByt = HDLPF.Pic2ByteBuf(oImg.oImg);

            ((OleDbParameter)cmd.Parameters.Add("@Pic1", OleDbType.Binary)).Value = ArayByt;

            oImg = (this.Controls.Find("Img75", true)[0] as MyImg);
            if (oImg != null) ArayByt = HDLPF.Pic2ByteBuf(oImg.oImg);

            ((OleDbParameter)cmd.Parameters.Add("@Pic2", OleDbType.Binary)).Value = ArayByt;
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (OleDbException exp)
            {
                MessageBox.Show(exp.ToString());
            }
            conn.Close();

            strsql = "Insert into dbRemote(DIndex,PageID,Pic1,Pic2,Pic3) values(@DIndex,@PageID,@Pic1,@Pic2,@Pic3)";
            //创建一个OleDbConnection对象
            conn = new OleDbConnection(DataModule.ConString + CsConst.mstrDefaultPath);
            conn.Open();

            cmd = new OleDbCommand(strsql, conn);
            ((OleDbParameter)cmd.Parameters.Add("@DIndex", OleDbType.Integer)).Value = 1;
            ((OleDbParameter)cmd.Parameters.Add("@PageID", OleDbType.Char)).Value = 15;

            oImg = (this.Controls.Find("Img76", true)[0] as MyImg);
            if (oImg != null) ArayByt = HDLPF.Pic2ByteBuf(oImg.oImg);

            ((OleDbParameter)cmd.Parameters.Add("@Pic1", OleDbType.Binary)).Value = ArayByt;

            oImg = (this.Controls.Find("Img77", true)[0] as MyImg);
            if (oImg != null) ArayByt = HDLPF.Pic2ByteBuf(oImg.oImg);

            ((OleDbParameter)cmd.Parameters.Add("@Pic2", OleDbType.Binary)).Value = ArayByt;

            oImg = (this.Controls.Find("Img78", true)[0] as MyImg);
            if (oImg != null) ArayByt = HDLPF.Pic2ByteBuf(oImg.oImg);

            ((OleDbParameter)cmd.Parameters.Add("@Pic3", OleDbType.Binary)).Value = ArayByt;
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (OleDbException exp)
            {
                MessageBox.Show(exp.ToString());
            }
            conn.Close();

            strsql = "Insert into dbRemote(DIndex,PageID,Pic1,Pic2,Pic3,Pic4) values(@DIndex,@PageID,@Pic1,@Pic2,@Pic3,@Pic4)";

            //创建一个OleDbConnection对象
            conn = new OleDbConnection(DataModule.ConString + CsConst.mstrDefaultPath);
            conn.Open();

            cmd = new OleDbCommand(strsql, conn);
            ((OleDbParameter)cmd.Parameters.Add("@DIndex", OleDbType.Integer)).Value = 1;
            ((OleDbParameter)cmd.Parameters.Add("@PageID", OleDbType.Char)).Value = 16;

            oImg = (this.Controls.Find("Img79", true)[0] as MyImg);
            if (oImg != null) ArayByt = HDLPF.Pic2ByteBuf(oImg.oImg);

            ((OleDbParameter)cmd.Parameters.Add("@Pic1", OleDbType.Binary)).Value = ArayByt;

            oImg = (this.Controls.Find("Img80", true)[0] as MyImg);
            if (oImg != null) ArayByt = HDLPF.Pic2ByteBuf(oImg.oImg);

            ((OleDbParameter)cmd.Parameters.Add("@Pic2", OleDbType.Binary)).Value = ArayByt;

            oImg = (this.Controls.Find("Img81", true)[0] as MyImg);
            if (oImg != null) ArayByt = HDLPF.Pic2ByteBuf(oImg.oImg);

            ((OleDbParameter)cmd.Parameters.Add("@Pic3", OleDbType.Binary)).Value = ArayByt;

            oImg = (this.Controls.Find("Img82", true)[0] as MyImg);
            if (oImg != null) ArayByt = HDLPF.Pic2ByteBuf(oImg.oImg);
            ((OleDbParameter)cmd.Parameters.Add("@Pic4", OleDbType.Binary)).Value = ArayByt;

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (OleDbException exp)
            {
                MessageBox.Show(exp.ToString());
            }
            conn.Close();
            #endregion

            //touch panel  1 2 3  4
            #region
            strsql = "Insert into dbRemote(DIndex,PageID,Pic1) values(@DIndex,@PageID,@Pic1)";

            //创建一个OleDbConnection对象
            conn = new OleDbConnection(DataModule.ConString + CsConst.mstrDefaultPath);
            conn.Open();

            cmd = new OleDbCommand(strsql, conn);
            ((OleDbParameter)cmd.Parameters.Add("@DIndex", OleDbType.Integer)).Value = 1;
            ((OleDbParameter)cmd.Parameters.Add("@PageID", OleDbType.Char)).Value = 17;

            oImg = (this.Controls.Find("Img83", true)[0] as MyImg);
            if (oImg != null) ArayByt = HDLPF.Pic2ByteBuf(oImg.oImg);

            ((OleDbParameter)cmd.Parameters.Add("@Pic1", OleDbType.Binary)).Value = ArayByt;
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (OleDbException exp)
            {
                MessageBox.Show(exp.ToString());
            }
            conn.Close();

            strsql = "Insert into dbRemote(DIndex,PageID,Pic1,Pic2) values(@DIndex,@PageID,@Pic1,@Pic2)";
            //创建一个OleDbConnection对象
            conn = new OleDbConnection(DataModule.ConString + CsConst.mstrDefaultPath);
            conn.Open();

            cmd = new OleDbCommand(strsql, conn);
            ((OleDbParameter)cmd.Parameters.Add("@DIndex", OleDbType.Integer)).Value = 1;
            ((OleDbParameter)cmd.Parameters.Add("@PageID", OleDbType.Char)).Value = 18;

            oImg = (this.Controls.Find("Img84", true)[0] as MyImg);
            if (oImg != null) ArayByt = HDLPF.Pic2ByteBuf(oImg.oImg);

            ((OleDbParameter)cmd.Parameters.Add("@Pic1", OleDbType.Binary)).Value = ArayByt;

            oImg = (this.Controls.Find("Img85", true)[0] as MyImg);
            if (oImg != null) ArayByt = HDLPF.Pic2ByteBuf(oImg.oImg);

            ((OleDbParameter)cmd.Parameters.Add("@Pic2", OleDbType.Binary)).Value = ArayByt;
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (OleDbException exp)
            {
                MessageBox.Show(exp.ToString());
            }
            conn.Close();

            strsql = "Insert into dbRemote(DIndex,PageID,Pic1,Pic2,Pic3) values(@DIndex,@PageID,@Pic1,@Pic2,@Pic3)";
            //创建一个OleDbConnection对象
            conn = new OleDbConnection(DataModule.ConString + CsConst.mstrDefaultPath);
            conn.Open();

            cmd = new OleDbCommand(strsql, conn);
            ((OleDbParameter)cmd.Parameters.Add("@DIndex", OleDbType.Integer)).Value = 1;
            ((OleDbParameter)cmd.Parameters.Add("@PageID", OleDbType.Char)).Value = 19;

            oImg = (this.Controls.Find("Img86", true)[0] as MyImg);
            if (oImg != null) ArayByt = HDLPF.Pic2ByteBuf(oImg.oImg);

            ((OleDbParameter)cmd.Parameters.Add("@Pic1", OleDbType.Binary)).Value = ArayByt;

            oImg = (this.Controls.Find("Img87", true)[0] as MyImg);
            if (oImg != null) ArayByt = HDLPF.Pic2ByteBuf(oImg.oImg);

            ((OleDbParameter)cmd.Parameters.Add("@Pic2", OleDbType.Binary)).Value = ArayByt;

            oImg = (this.Controls.Find("Img88", true)[0] as MyImg);
            if (oImg != null) ArayByt = HDLPF.Pic2ByteBuf(oImg.oImg);

            ((OleDbParameter)cmd.Parameters.Add("@Pic3", OleDbType.Binary)).Value = ArayByt;
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (OleDbException exp)
            {
                MessageBox.Show(exp.ToString());
            }
            conn.Close();

            strsql = "Insert into dbRemote(DIndex,PageID,Pic1,Pic2,Pic3,Pic4) values(@DIndex,@PageID,@Pic1,@Pic2,@Pic3,@Pic4)";

            //创建一个OleDbConnection对象
            conn = new OleDbConnection(DataModule.ConString + CsConst.mstrDefaultPath);
            conn.Open();

            cmd = new OleDbCommand(strsql, conn);
            ((OleDbParameter)cmd.Parameters.Add("@DIndex", OleDbType.Integer)).Value = 1;
            ((OleDbParameter)cmd.Parameters.Add("@PageID", OleDbType.Char)).Value = 20;

            oImg = (this.Controls.Find("Img79", true)[0] as MyImg);
            if (oImg != null) ArayByt = HDLPF.Pic2ByteBuf(oImg.oImg);

            ((OleDbParameter)cmd.Parameters.Add("@Pic1", OleDbType.Binary)).Value = ArayByt;

            oImg = (this.Controls.Find("Img80", true)[0] as MyImg);
            if (oImg != null) ArayByt = HDLPF.Pic2ByteBuf(oImg.oImg);

            ((OleDbParameter)cmd.Parameters.Add("@Pic2", OleDbType.Binary)).Value = ArayByt;

            oImg = (this.Controls.Find("Img81", true)[0] as MyImg);
            if (oImg != null) ArayByt = HDLPF.Pic2ByteBuf(oImg.oImg);

            ((OleDbParameter)cmd.Parameters.Add("@Pic3", OleDbType.Binary)).Value = ArayByt;

            oImg = (this.Controls.Find("Img82", true)[0] as MyImg);
            if (oImg != null) ArayByt = HDLPF.Pic2ByteBuf(oImg.oImg);
            ((OleDbParameter)cmd.Parameters.Add("@Pic4", OleDbType.Binary)).Value = ArayByt;

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (OleDbException exp)
            {
                MessageBox.Show(exp.ToString());
            }
            conn.Close();
            #endregion

            // 红外学习页面 + wifi 模块 + 素材库
            #region
            for (int intI = 0; intI < 20; intI++)
            {
                strsql = "Insert into dbRemote(DIndex,PageID,Pic1,Pic2,Pic3,Pic4,Pic5,Pic6,Pic7,Pic8)"
                           + " values(@DIndex,@PageID,@Pic1,@Pic2,@Pic3,@Pic4,@Pic5,@Pic6,@Pic7,@Pic8)";

                //创建一个OleDbConnection对象
                conn = new OleDbConnection(DataModule.ConString + CsConst.mstrDefaultPath);
                conn.Open();

                cmd = new OleDbCommand(strsql, conn);
                ((OleDbParameter)cmd.Parameters.Add("@DIndex", OleDbType.Integer)).Value = 1;
                ((OleDbParameter)cmd.Parameters.Add("@PageID", OleDbType.Char)).Value = intI + 21;

                for (int intJ = 0; intJ < 8; intJ++)
                {
                    oImg = (this.Controls.Find("Img" + ( 93 + intI * 8 + intJ).ToString(), true)[0] as MyImg);
                    if (oImg != null) ArayByt = HDLPF.Pic2ByteBuf(oImg.oImg);

                    ((OleDbParameter)cmd.Parameters.Add("@Pic" + (intJ + 1), OleDbType.Binary)).Value = ArayByt;
                }
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (OleDbException exp)
                {
                    MessageBox.Show(exp.ToString());
                }
                conn.Close();
            }
            #endregion
            Cursor.Current = Cursors.Default;
        }

        private void Upload_Click(object sender, EventArgs e)
        {
            if (cboDevicePath.SelectedIndex < 0) return;
            Cursor.Current = Cursors.WaitCursor;
            if (MyBackGroup != null && MyBackGroup.IsBusy) MyBackGroup.CancelAsync();
            MyBackGroup = new BackgroundWorker();
            MyBackGroup.DoWork += new DoWorkEventHandler(calculationWorker_DoWork);
            MyBackGroup.ProgressChanged += new ProgressChangedEventHandler(calculationWorker_ProgressChanged);
            MyBackGroup.WorkerReportsProgress = true;
            MyBackGroup.RunWorkerCompleted += new RunWorkerCompletedEventHandler(calculationWorker_RunWorkerCompleted);
            MyBackGroup.RunWorkerAsync();
            MyBackGroup.WorkerSupportsCancellation = true;
            DateTime t1, t2;
            //DLP

            #region
            for (int intI = 1; intI <= 56; intI++)
            {
                MyImg oImg = (this.Controls.Find("Img" + intI.ToString(), true)[0] as MyImg);
                if (oImg != null)
                {
                    byte[] ArayTmp = HDLPF.RemoteIconToByte(oImg.oImg, oImg.Width, oImg.Height);

                    writeValueBuffer = new byte[64];
                    writeValueBuffer[0] = 1;
                    writeValueBuffer[1] = 0xA1;  // write thing
                    writeValueBuffer[2] = 0x1;   // DLP
                    writeValueBuffer[3] = (byte)((intI - 1) / 8 + 1);   // page no

                    if (intI % 8 == 0)
                        writeValueBuffer[4] = 8;
                    else
                        writeValueBuffer[4] = (byte)(intI % 8);  // button no

                    for (int intJ = 0; intJ < ArayTmp.Length / 58 + 1; intJ++)
                    {
                        writeValueBuffer[5] = (byte)(intJ + 1);

                        if (ArayTmp.Length > intJ * 58 + 58)
                        {
                            Array.Copy(ArayTmp, intJ * 58, writeValueBuffer, 6, 58);
                        }
                        else
                        {
                            Array.Copy(ArayTmp, intJ * 58, writeValueBuffer, 6, ArayTmp.Length % 58);
                        }

                        mintNowSending = writeValueBuffer[4];

                        WritetxtData();

                        t1 = DateTime.Now;
                        while (mintNowSending != -1)
                        {
                            t2 = DateTime.Now;
                            int TimeBetw = HDLSysPF.Compare(t2, t1);
                            if (TimeBetw >= CsConst.replySpanTimes)
                            {
                                break;
                            }
                            HDLUDP.TimeBetwnNext(20);
                        }
                    }
                }
            }
            #endregion

            //single 

            #region
            for (int intI = 57; intI <= 72; intI++)
            {
                MyImg oImg = (this.Controls.Find("Img" + intI.ToString(), true)[0] as MyImg);
                if (oImg != null)
                {
                    byte[] ArayTmp = HDLPF.RemoteIconToByte(oImg.oImg, oImg.Width, oImg.Height);

                    writeValueBuffer = new byte[64];
                    writeValueBuffer[0] = 1;
                    writeValueBuffer[1] = 0xA1;  // write thing
                    writeValueBuffer[2] = 0x2;   // single
                    writeValueBuffer[3] = byte.Parse(oImg.Tag.ToString());   // page no

                    switch (intI)
                    {
                        case 57:
                        case 58:
                        case 60:
                        case 63:
                        case 67:
                            writeValueBuffer[4] = 1; break;
                        case 59:
                        case 61:
                        case 64:
                        case 68:
                            writeValueBuffer[4] = 2; break;
                        case 62:
                        case 65:
                        case 69:
                            writeValueBuffer[4] = 3; break;
                        case 66:
                        case 70:
                            writeValueBuffer[4] = 4; break;
                        case 71:
                            writeValueBuffer[4] = 5; break;
                        case 72:
                            writeValueBuffer[4] = 6; break;
                    }
                    writeValueBuffer[4] = (byte)(intI % 8);  // button no

                    for (int intJ = 0; intJ < ArayTmp.Length / 58 + 1; intJ++)
                    {
                        writeValueBuffer[5] = (byte)(intJ + 1);

                        if (ArayTmp.Length > intJ * 58 + 58)
                        {
                            Array.Copy(ArayTmp, intJ * 58, writeValueBuffer, 6, 58);
                        }
                        else
                        {
                            Array.Copy(ArayTmp, intJ * 58, writeValueBuffer, 6, ArayTmp.Length % 58);
                        }

                        mintNowSending = writeValueBuffer[5];
                        WritetxtData();
                        t1 = DateTime.Now;
                        while (mintNowSending != -1)
                        {
                            t2 = DateTime.Now;
                            int TimeBetw = HDLSysPF.Compare(t2, t1);
                            if (TimeBetw >= CsConst.replySpanTimes)
                            {
                                break;
                            }
                            HDLUDP.TimeBetwnNext(20);
                        }
                    }
                }
            }
            #endregion

            // double

            #region
            for (int intI = 73; intI <= 82; intI++)
            {
                MyImg oImg = (this.Controls.Find("Img" + intI.ToString(), true)[0] as MyImg);
                if (oImg != null)
                {
                    byte[] ArayTmp = HDLPF.RemoteIconToByte(oImg.oImg, oImg.Width, oImg.Height);

                    writeValueBuffer = new byte[64];
                    writeValueBuffer[0] = 1;
                    writeValueBuffer[1] = 0xA1;  // write thing
                    writeValueBuffer[2] = 0x3;   // single
                    writeValueBuffer[3] = byte.Parse(oImg.Tag.ToString());   // page no

                    switch (intI)
                    {
                        case 73:
                        case 74:
                        case 76:
                        case 79:
                            writeValueBuffer[4] = 1; break;
                        case 75:
                        case 77:
                        case 80:
                            writeValueBuffer[4] = 2; break;
                        case 78:
                        case 81:
                            writeValueBuffer[4] = 3; break;
                        case 82:
                            writeValueBuffer[4] = 4; break;
                    }
                    writeValueBuffer[4] = (byte)(intI % 8);  // button no

                    for (int intJ = 0; intJ < ArayTmp.Length / 58 + 1; intJ++)
                    {
                        writeValueBuffer[5] = (byte)(intJ + 1);

                        if (ArayTmp.Length > intJ * 58 + 58)
                        {
                            Array.Copy(ArayTmp, intJ * 58, writeValueBuffer, 6, 58);
                        }
                        else
                        {
                            Array.Copy(ArayTmp, intJ * 58, writeValueBuffer, 6, ArayTmp.Length % 58);
                        }
                        mintNowSending = writeValueBuffer[5];
                        WritetxtData();

                        t1 = DateTime.Now;
                        while (mintNowSending != -1)
                        {
                            t2 = DateTime.Now;
                            int TimeBetw = HDLSysPF.Compare(t2, t1);
                            if (TimeBetw >= CsConst.replySpanTimes)
                            {
                                break;
                            }
                            HDLUDP.TimeBetwnNext(20);
                        }
                    }
                }
            }
            #endregion

            // touch

            #region
            for (int intI = 83; intI <= 92; intI++)
            {
                MyImg oImg = (this.Controls.Find("Img" + intI.ToString(), true)[0] as MyImg);
                if (oImg != null)
                {
                    byte[] ArayTmp = HDLPF.RemoteIconToByte(oImg.oImg, oImg.Width, oImg.Height);

                    writeValueBuffer = new byte[64];
                    writeValueBuffer[0] = 1;
                    writeValueBuffer[1] = 0xA1;  // write thing
                    writeValueBuffer[2] = 0x4;   // single
                    writeValueBuffer[3] = byte.Parse(oImg.Tag.ToString());   // page no

                    switch (intI)
                    {
                        case 83:
                        case 84:
                        case 86:
                        case 89:
                            writeValueBuffer[4] = 1; break;
                        case 85:
                        case 87:
                        case 90:
                            writeValueBuffer[4] = 2; break;
                        case 88:
                        case 91:
                            writeValueBuffer[4] = 3; break;
                        case 92:
                            writeValueBuffer[4] = 4; break;
                    }
                    writeValueBuffer[4] = (byte)(intI % 8);  // button no

                    for (int intJ = 0; intJ < ArayTmp.Length / 58 + 1; intJ++)
                    {
                        writeValueBuffer[5] = (byte)(intJ + 1);

                        if (ArayTmp.Length > intJ * 58 + 58)
                        {
                            Array.Copy(ArayTmp, intJ * 58, writeValueBuffer, 6, 58);
                        }
                        else
                        {
                            Array.Copy(ArayTmp, intJ * 58, writeValueBuffer, 6, ArayTmp.Length % 58);
                        }
                        mintNowSending = writeValueBuffer[5];

                        WritetxtData();
                        t1 = DateTime.Now;
                        while (mintNowSending != -1)
                        {
                            t2 = DateTime.Now;
                            int TimeBetw = HDLSysPF.Compare(t2, t1);
                            if (TimeBetw >= CsConst.replySpanTimes)
                            {
                                break;
                            }
                            HDLUDP.TimeBetwnNext(20);
                        }
                    }
                }
            }
            #endregion

            //ir learner page

            #region
            for (int intI = 93; intI <= 140; intI++)
            {
                MyImg oImg = (this.Controls.Find("Img" + intI.ToString(), true)[0] as MyImg);
                if (oImg != null)
                {
                    byte[] ArayTmp = HDLPF.RemoteIconToByte(oImg.oImg, oImg.oImg.Width, oImg.oImg.Height);

                    writeValueBuffer = new byte[64];
                    writeValueBuffer[0] = 1;
                    writeValueBuffer[1] = 0xA1;  // write thing
                    writeValueBuffer[2] = 0x5;   // DLP
                    if (intI > 140) writeValueBuffer[2] = 0x6;   // DLP
                    writeValueBuffer[3] = (byte)((intI - 93) / 8 + 1);   // page no

                    if (intI % 8 == 0)
                        writeValueBuffer[4] = 8;
                    else
                        writeValueBuffer[4] = (byte)(intI % 8);  // button no

                    for (int intJ = 0; intJ < ArayTmp.Length / 58 + 1; intJ++)
                    {
                        writeValueBuffer[5] = (byte)(intJ + 1);

                        if (ArayTmp.Length > intJ * 58 + 58)
                        {
                            Array.Copy(ArayTmp, intJ * 58, writeValueBuffer, 6, 58);
                        }
                        else
                        {
                            Array.Copy(ArayTmp, intJ * 58, writeValueBuffer, 6, ArayTmp.Length % 58);
                        }

                        mintNowSending = writeValueBuffer[4];

                        WritetxtData();
                        t1 = DateTime.Now;
                        while (mintNowSending != -1)
                        {
                            t2 = DateTime.Now;
                            int TimeBetw = HDLSysPF.Compare(t2, t1);
                            if (TimeBetw >= CsConst.replySpanTimes)
                            {
                                break;
                            }
                            HDLUDP.TimeBetwnNext(20);
                        }
                    }
                }

            #endregion
            }

            #region
            for (int intI = 141; intI <= 188; intI++)
            {
                MyImg oImg = (this.Controls.Find("Img" + intI.ToString(), true)[0] as MyImg);
                if (oImg != null)
                {
                    byte[] ArayTmp = HDLPF.RemoteIconToByte(oImg.oImg, oImg.oImg.Width, oImg.oImg.Height);

                    writeValueBuffer = new byte[64];
                    writeValueBuffer[0] = 1;
                    writeValueBuffer[1] = 0xA1;  // write thing
                    writeValueBuffer[2] = 0x6;   // DLP
                    writeValueBuffer[3] = (byte)((intI - 141) / 8 + 1);   // page no

                    if (intI % 8 == 0)
                        writeValueBuffer[4] = 8;
                    else
                        writeValueBuffer[4] = (byte)(intI % 8);  // button no

                    for (int intJ = 0; intJ < ArayTmp.Length / 58 + 1; intJ++)
                    {
                        writeValueBuffer[5] = (byte)(intJ + 1);

                        if (ArayTmp.Length > intJ * 58 + 58)
                        {
                            Array.Copy(ArayTmp, intJ * 58, writeValueBuffer, 6, 58);
                        }
                        else
                        {
                            Array.Copy(ArayTmp, intJ * 58, writeValueBuffer, 6, ArayTmp.Length % 58);
                        }

                        mintNowSending = writeValueBuffer[4];

                        WritetxtData();

                        t1 = DateTime.Now;
                        while (mintNowSending != -1)
                        {
                            t2 = DateTime.Now;
                            int TimeBetw = HDLSysPF.Compare(t2, t1);
                            if (TimeBetw >= CsConst.replySpanTimes)
                            {
                                break;
                            }
                            HDLUDP.TimeBetwnNext(20);
                        }
                    }
                }

            #endregion
            }
            Cursor.Current = Cursors.Default;
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

        void calculationWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //throw new NotImplementedException();
            try
            {

            }
            catch
            {
            }
        }

        void calculationWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                //将sleep和无限循环放在等待异步的外面
                ReadData();
                //Thread.Sleep(1);
            }
        }

        private void WritetxtData()
        {
            if (writeValueBuffer == null) return;
            CsConst.isWriteDataToUSB = true;
            byte[] nowWriteData = null;
            //System.Threading.Thread.Sleep(1000);
            if (myUsb.WriteHidFile(devicePath[myintUSB], 64, writeValueBuffer, ref nowWriteData))
            {

            }

        }

        private void ReadData()
        {
            try
            {
                byte[] readValue = new byte[64];
                int readLength = 0;
                if (myintUSB != -1)
                {
                    if (!CsConst.isWriteDataToUSB)
                    {
                        if (myUsb.ReadHidFile(devicePath[myintUSB], ref readValue, ref readLength))
                        {
                            if (readValue[4] == mintNowSending)
                            {
                                mintNowSending = -1;
                            }
                        }
                    }
                }
            }
            catch
            {
            }
        }

        private void openToolStripButton_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            string strPrj = "select * from dbRemote where DIndex = 1 order by PageID";
            OleDbDataReader drPrj =DataModule.SearchAResultSQLDB(strPrj);
            if (drPrj != null)
            {
                while (drPrj.Read())
                {
                    byte bytPageID = drPrj.GetByte(1);

                    if (bytPageID >= 1 && bytPageID <= 7) // DLP 
                    {
                        for (int intJ = 1; intJ < 9; intJ++)
                        {
                            MyImg oImg = (this.Controls.Find("Img" + ((bytPageID - 1) * 8 + intJ).ToString(), true)[0] as MyImg);
                            if (oImg != null) oImg.oImg = HDLPF.BytArayToPic((byte[])drPrj[1 + intJ]);
                        }
                    }
                    else if (bytPageID == 8) // single 1
                    {
                        Img57.oImg = HDLPF.BytArayToPic((byte[])drPrj[2]);
                    }
                    else if (bytPageID == 9) // single 2
                    {
                        Img58.oImg = HDLPF.BytArayToPic((byte[])drPrj[2]);
                        Img59.oImg = HDLPF.BytArayToPic((byte[])drPrj[3]);
                    }
                    else if (bytPageID == 10) // single 3
                    {
                        Img60.oImg = HDLPF.BytArayToPic((byte[])drPrj[2]);
                        Img61.oImg = HDLPF.BytArayToPic((byte[])drPrj[3]);
                        Img62.oImg = HDLPF.BytArayToPic((byte[])drPrj[4]);
                    }
                    else if (bytPageID == 11) // single 4
                    {
                        Img63.oImg = HDLPF.BytArayToPic((byte[])drPrj[2]);
                        Img64.oImg = HDLPF.BytArayToPic((byte[])drPrj[3]);
                        Img65.oImg = HDLPF.BytArayToPic((byte[])drPrj[4]);
                        Img66.oImg = HDLPF.BytArayToPic((byte[])drPrj[5]);
                    }
                    else if (bytPageID == 12) // single 6
                    {
                        Img67.oImg = HDLPF.BytArayToPic((byte[])drPrj[2]);
                        Img68.oImg = HDLPF.BytArayToPic((byte[])drPrj[3]);
                        Img69.oImg = HDLPF.BytArayToPic((byte[])drPrj[4]);
                        Img70.oImg = HDLPF.BytArayToPic((byte[])drPrj[5]);
                        Img71.oImg = HDLPF.BytArayToPic((byte[])drPrj[6]);
                        Img72.oImg = HDLPF.BytArayToPic((byte[])drPrj[7]);
                    }
                    else if (bytPageID == 13) // double 1
                    {
                        Img73.oImg = HDLPF.BytArayToPic((byte[])drPrj[2]);
                    }
                    else if (bytPageID == 14) // double 2
                    {
                        Img74.oImg = HDLPF.BytArayToPic((byte[])drPrj[2]);
                        Img75.oImg = HDLPF.BytArayToPic((byte[])drPrj[3]);
                    }
                    else if (bytPageID == 15) // double 3
                    {
                        Img76.oImg = HDLPF.BytArayToPic((byte[])drPrj[2]);
                        Img77.oImg = HDLPF.BytArayToPic((byte[])drPrj[3]);
                        Img78.oImg = HDLPF.BytArayToPic((byte[])drPrj[4]);
                    }
                    else if (bytPageID == 16) // double 4
                    {
                        Img79.oImg = HDLPF.BytArayToPic((byte[])drPrj[2]);
                        Img80.oImg = HDLPF.BytArayToPic((byte[])drPrj[3]);
                        Img81.oImg = HDLPF.BytArayToPic((byte[])drPrj[4]);
                        Img82.oImg = HDLPF.BytArayToPic((byte[])drPrj[5]);
                    }
                    else if (bytPageID == 17) // touch 1
                    {
                        Img83.oImg = HDLPF.BytArayToPic((byte[])drPrj[2]);
                    }
                    else if (bytPageID == 18) // touch 2
                    {
                        Img84.oImg = HDLPF.BytArayToPic((byte[])drPrj[2]);
                        Img85.oImg = HDLPF.BytArayToPic((byte[])drPrj[3]);
                    }
                    else if (bytPageID == 19) // touch 3
                    {
                        Img86.oImg = HDLPF.BytArayToPic((byte[])drPrj[2]);
                        Img87.oImg = HDLPF.BytArayToPic((byte[])drPrj[3]);
                        Img88.oImg = HDLPF.BytArayToPic((byte[])drPrj[4]);
                    }
                    else if (bytPageID == 20) // touch 4
                    {
                        Img89.oImg = HDLPF.BytArayToPic((byte[])drPrj[2]);
                        Img90.oImg = HDLPF.BytArayToPic((byte[])drPrj[3]);
                        Img91.oImg = HDLPF.BytArayToPic((byte[])drPrj[4]);
                        Img92.oImg = HDLPF.BytArayToPic((byte[])drPrj[5]);
                    }
                    else  //  红外学习 + wifi + 素材库
                    {
                        for (int intJ = 1; intJ < 9; intJ++)
                        {
                            MyImg oImg = (this.Controls.Find("Img" + (92 + (bytPageID - 21) * 8 + intJ).ToString(), true)[0] as MyImg);
                            if (oImg != null) oImg.oImg = HDLPF.BytArayToPic((byte[])drPrj[1 + intJ]);
                        }
                    }

                }
                drPrj.Close();
            }
            Cursor.Current = Cursors.Default;
        }

        private void frmRemote_Load(object sender, EventArgs e)
        {
            string strPath  = Application.StartupPath + @"\DIY\";
            tbPage.SelectedIndex = 0;
            System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = false;
            NstDefault_Click(null,null);
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

        private void button1_Click(object sender, EventArgs e)
        {
            cboDevicePath.Items.Clear();

            string hidGuid = "";
            int deviceCount = 0;

            bool result = myUsb.FindHidDevice(ref hidGuid, ref vendorID, ref productID, ref devicePath, ref outputID, ref outputLength, ref deviceCount);
            if (result)
            {
                for (int i = 0; i <= deviceCount; i++)
                {
                    if (vendorID[i].ToUpper() == "1FC9" && productID[i].ToString() == "3")
                    {
                        cboDevicePath.Items.Add(i.ToString() + "-" + vendorID[i].ToUpper() + "-" + productID[i].ToString());
                    }
                }
                if (cboDevicePath.Items != null & cboDevicePath.Items.Count !=0) cboDevicePath.SelectedIndex = 0;
            }
            else
            {
                //  lstShowFindInfo.Items.Add("没有发现连接的HidUSB设备");
            }
        }

        private void tbFixed_Click(object sender, EventArgs e)
        {
            if (cboDevicePath.SelectedIndex == -1) return;
            Cursor.Current = Cursors.WaitCursor;

            if (MyBackGroup != null && MyBackGroup.IsBusy) MyBackGroup.CancelAsync();
            MyBackGroup = new BackgroundWorker();
            MyBackGroup.DoWork += new DoWorkEventHandler(calculationWorker_DoWork);
            MyBackGroup.ProgressChanged += new ProgressChangedEventHandler(calculationWorker_ProgressChanged);
            MyBackGroup.WorkerReportsProgress = true;
            MyBackGroup.RunWorkerCompleted += new RunWorkerCompletedEventHandler(calculationWorker_RunWorkerCompleted);
            MyBackGroup.RunWorkerAsync();
            MyBackGroup.WorkerSupportsCancellation = true;
            DateTime t1, t2;

            switch (tbPage.SelectedIndex + 1)
            {
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                case 6:
                case 7:
                #region
                    for (int intI = 1 + (tbPage.SelectedIndex) * 8; intI <= (tbPage.SelectedIndex + 1) * 8; intI++)
                    {
                        MyImg oImg = (this.Controls.Find("Img" + intI.ToString(), true)[0] as MyImg);
                        if (oImg != null)
                        {
                            byte[] ArayTmp = HDLPF.RemoteIconToByte(oImg.oImg, oImg.oImg.Width, oImg.oImg.Height);

                            writeValueBuffer = new byte[64];
                            writeValueBuffer[0] = 1;
                            writeValueBuffer[1] = 0xA1;  // write thing
                            writeValueBuffer[2] = 0x1;   // DLP
                            writeValueBuffer[3] = (byte)(tbPage.SelectedIndex + 1);   // page no

                            if (intI % 8 == 0)
                                writeValueBuffer[4] = 8;
                            else
                                writeValueBuffer[4] = (byte)(intI % 8);  // button no

                            for (int intJ = 0; intJ < ArayTmp.Length / 58 + 1; intJ++)
                            {
                                writeValueBuffer[5] = (byte)(intJ + 1);

                                if (ArayTmp.Length > intJ * 58 + 58)
                                {
                                    Array.Copy(ArayTmp, intJ * 58, writeValueBuffer, 6, 58);
                                }
                                else
                                {
                                    Array.Copy(ArayTmp, intJ * 58, writeValueBuffer, 6, ArayTmp.Length % 58);
                                }

                                mintNowSending =writeValueBuffer[4];

                                WritetxtData();
                                
                                t1 = DateTime.Now;
                                while (mintNowSending != -1 )
                                {
                                    t2 = DateTime.Now;
                                    int TimeBetw =HDLSysPF.Compare(t2, t1);
                                    if (TimeBetw >= CsConst.replySpanTimes)
                                    {
                                        break;
                                    }
                                    //HDLUDP.TimeBetwnNext(1);
                                }
                            }
                        }
                    }
            #endregion
                    break;
                case 8:
                case 9:
                case 10:
                case 11:
                case 12:
                    #region
                    byte bytStart = 0;
                    byte bytEnd = 0;
                    if (tbPage.SelectedIndex + 1 == 8)
                    {
                        bytStart = 57;
                        bytEnd = 57;
                    }
                    else if (tbPage.SelectedIndex + 1 == 9)
                    {
                        bytStart = 58;
                        bytEnd = 59;
                    }
                    else if (tbPage.SelectedIndex + 1 == 10)
                    {
                        bytStart = 60;
                        bytEnd = 62;
                    }
                    else if (tbPage.SelectedIndex + 1 == 11)
                    {
                        bytStart = 63;
                        bytEnd = 66;
                    }
                    else if (tbPage.SelectedIndex + 1 == 12)
                    {
                        bytStart = 67;
                        bytEnd = 72;
                    }

                    for (int intI = bytStart; intI <= bytEnd; intI++)
                    {
                        MyImg oImg = (this.Controls.Find("Img" + intI.ToString(), true)[0] as MyImg);
                        if (oImg != null)
                        {
                            byte[] ArayTmp = HDLPF.RemoteIconToByte(oImg.oImg, oImg.oImg.Width, oImg.oImg.Height);

                            writeValueBuffer = new byte[64];
                            writeValueBuffer[0] = 1;
                            writeValueBuffer[1] = 0xA1;  // write thing
                            writeValueBuffer[2] = 0x2;   // single
                            writeValueBuffer[3] = byte.Parse(oImg.Tag.ToString());   // page no

                            switch (intI)
                            {
                                case 57:
                                case 58:
                                case 60:
                                case 63:
                                case 67:
                                    writeValueBuffer[4] = 1; break;
                                case 59:
                                case 61:
                                case 64:
                                case 68:
                                    writeValueBuffer[4] = 2; break;
                                case 62:
                                case 65:
                                case 69:
                                    writeValueBuffer[4] = 3; break;
                                case 66:
                                case 70:
                                    writeValueBuffer[4] = 4; break;
                                case 71:
                                    writeValueBuffer[4] = 5; break;
                                case 72:
                                    writeValueBuffer[4] = 6; break;
                            }
                           // writeValueBuffer[4] = (byte)(intI % 8);  // button no

                            for (int intJ = 0; intJ < ArayTmp.Length / 58 + 1; intJ++)
                            {
                                writeValueBuffer[5] = (byte)(intJ + 1);

                                if (ArayTmp.Length > intJ * 58 + 58)
                                {
                                    Array.Copy(ArayTmp, intJ * 58, writeValueBuffer, 6, 58);
                                }
                                else
                                {
                                    Array.Copy(ArayTmp, intJ * 58, writeValueBuffer, 6, ArayTmp.Length % 58);
                                }

                                mintNowSending = writeValueBuffer[4];

                                WritetxtData();

                                t1 = DateTime.Now;
                                while (mintNowSending != -1)
                                {
                                    t2 = DateTime.Now;
                                    int TimeBetw = HDLSysPF.Compare(t2, t1);
                                    if (TimeBetw >= CsConst.replySpanTimes)
                                    {
                                        break;
                                    }
                                    HDLUDP.TimeBetwnNext(20);
                                }
                            }
                        }
                    }
                    #endregion
                    break;
                case 13:
                case 14:
                case 15:
                case 16:
                    #region
                    bytStart = 0;
                    bytEnd = 0;
                    if (tbPage.SelectedIndex + 1 == 13)
                    {
                        bytStart = 73;
                        bytEnd = 73;
                    }
                    else if (tbPage.SelectedIndex + 1 == 14)
                    {
                        bytStart = 74;
                        bytEnd = 75;
                    }
                    else if (tbPage.SelectedIndex + 1 == 15)
                    {
                        bytStart = 76;
                        bytEnd = 78;
                    }
                    else if (tbPage.SelectedIndex + 1 == 16)
                    {
                        bytStart = 79;
                        bytEnd = 82;
                    }

                    for (int intI = bytStart; intI <= bytEnd; intI++)
                    {
                        MyImg oImg = (this.Controls.Find("Img" + intI.ToString(), true)[0] as MyImg);
                        if (oImg != null)
                        {
                            byte[] ArayTmp = HDLPF.RemoteIconToByte(oImg.oImg, oImg.oImg.Width, oImg.oImg.Height);

                            writeValueBuffer = new byte[64];
                            writeValueBuffer[0] = 1;
                            writeValueBuffer[1] = 0xA1;  // write thing
                            writeValueBuffer[2] = 0x3;   // single
                            writeValueBuffer[3] = byte.Parse(oImg.Tag.ToString());   // page no

                            switch (intI)
                            {
                                case 73:
                                case 74:
                                case 76:
                                case 79:
                                    writeValueBuffer[4] = 1; break;
                                case 75:
                                case 77:
                                case 80:
                                    writeValueBuffer[4] = 2; break;
                                case 78:
                                case 81:
                                    writeValueBuffer[4] = 3; break;
                                case 82:
                                    writeValueBuffer[4] = 4; break;
                            }

                            for (int intJ = 0; intJ < ArayTmp.Length / 58 + 1; intJ++)
                            {
                                writeValueBuffer[5] = (byte)(intJ + 1);

                                if (ArayTmp.Length > intJ * 58 + 58)
                                {
                                    Array.Copy(ArayTmp, intJ * 58, writeValueBuffer, 6, 58);
                                }
                                else
                                {
                                    Array.Copy(ArayTmp, intJ * 58, writeValueBuffer, 6, ArayTmp.Length % 58);
                                }
                                mintNowSending = writeValueBuffer[4];

                                WritetxtData();

                                t1 = DateTime.Now;
                                while (mintNowSending != -1)
                                {
                                    t2 = DateTime.Now;
                                    int TimeBetw = HDLSysPF.Compare(t2, t1);
                                    if (TimeBetw >= CsConst.replySpanTimes)
                                    {
                                        break;
                                    }
                                    HDLUDP.TimeBetwnNext(20);
                                }
                            }
                        }
                    }
                    #endregion
                    break;
                case 17:
                case 18:
                case 19:
                case 20:
                    #region
                    bytStart = 0;
                    bytEnd = 0;
                    if (tbPage.SelectedIndex + 1 == 17)
                    {
                        bytStart = 83;
                        bytEnd = 83;
                    }
                    else if (tbPage.SelectedIndex + 1 == 18)
                    {
                        bytStart = 84;
                        bytEnd = 85;
                    }
                    else if (tbPage.SelectedIndex + 1 == 19)
                    {
                        bytStart = 86;
                        bytEnd = 88;
                    }
                    else if (tbPage.SelectedIndex + 1 == 20)
                    {
                        bytStart = 89;
                        bytEnd = 92;
                    }
                    for (int intI = bytStart; intI <= bytEnd; intI++)
                    {
                        MyImg oImg = (this.Controls.Find("Img" + intI.ToString(), true)[0] as MyImg);
                        if (oImg != null)
                        {
                            byte[] ArayTmp = HDLPF.RemoteIconToByte(oImg.oImg, oImg.oImg.Width, oImg.oImg.Height);

                            writeValueBuffer = new byte[64];
                            writeValueBuffer[0] = 1;
                            writeValueBuffer[1] = 0xA1;  // write thing
                            writeValueBuffer[2] = 0x4;   // single
                            writeValueBuffer[3] = byte.Parse(oImg.Tag.ToString());   // page no

                            switch (intI)
                            {
                                case 83:
                                case 84:
                                case 86:
                                case 89:
                                    writeValueBuffer[4] = 1; break;
                                case 85:
                                case 87:
                                case 90:
                                    writeValueBuffer[4] = 2; break;
                                case 88:
                                case 91:
                                    writeValueBuffer[4] = 3; break;
                                case 92:
                                    writeValueBuffer[4] = 4; break;
                            }

                            for (int intJ = 0; intJ < ArayTmp.Length / 58 + 1; intJ++)
                            {
                                writeValueBuffer[5] = (byte)(intJ + 1);

                                if (ArayTmp.Length > intJ * 58 + 58)
                                {
                                    Array.Copy(ArayTmp, intJ * 58, writeValueBuffer, 6, 58);
                                }
                                else
                                {
                                    Array.Copy(ArayTmp, intJ * 58, writeValueBuffer, 6, ArayTmp.Length % 58);
                                }
                                mintNowSending = writeValueBuffer[4];

                                WritetxtData();

                                t1 = DateTime.Now;
                                while (mintNowSending != -1)
                                {
                                    t2 = DateTime.Now;
                                    int TimeBetw = HDLSysPF.Compare(t2, t1);
                                    if (TimeBetw >= CsConst.replySpanTimes)
                                    {
                                        break;
                                    }
                                    HDLUDP.TimeBetwnNext(20);
                                }
                            }
                        }
                    }
                    #endregion
                    break;
                case 21:
                case 22:
                case 23:
                case 24:
                case 25:
                case 26:
                    //ir learner page
                    #region
                    for (int intI = 93 + (tbPage.SelectedIndex - 20) * 8; intI < 93 + (tbPage.SelectedIndex - 19) * 8; intI++)
                    {
                        MyImg oImg = (this.Controls.Find("Img" + intI.ToString(), true)[0] as MyImg);
                        if (oImg != null)
                        {
                            byte[] ArayTmp = HDLPF.RemoteIconToByte(oImg.oImg, oImg.oImg.Width, oImg.oImg.Height);

                            writeValueBuffer = new byte[64];
                            writeValueBuffer[0] = 1;
                            writeValueBuffer[1] = 0xA1;  // write thing
                            writeValueBuffer[2] = 0x5;   // DLP
                            writeValueBuffer[3] = (byte)((intI - 93) / 8 + 1);   // page no

                            if ((intI - 92) % 8 == 0)
                                writeValueBuffer[4] = 8;
                            else
                                writeValueBuffer[4] = (byte)((intI -92) % 8);  // button no

                            for (int intJ = 0; intJ < ArayTmp.Length / 58 + 1; intJ++)
                            {
                                writeValueBuffer[5] = (byte)(intJ + 1);

                                if (ArayTmp.Length > intJ * 58 + 58)
                                {
                                    Array.Copy(ArayTmp, intJ * 58, writeValueBuffer, 6, 58);
                                }
                                else
                                {
                                    Array.Copy(ArayTmp, intJ * 58, writeValueBuffer, 6, ArayTmp.Length % 58);
                                }

                                mintNowSending = writeValueBuffer[4];

                                WritetxtData();

                                t1 = DateTime.Now;
                                while (mintNowSending != -1)
                                {
                                    t2 = DateTime.Now;
                                    int TimeBetw = HDLSysPF.Compare(t2, t1);
                                    if (TimeBetw >= CsConst.replySpanTimes)
                                    {
                                        break;
                                    }
                                    HDLUDP.TimeBetwnNext(20);
                                }
                            }
                        }
                    }
                    #endregion
                    break;
                case 27:
                case 28:
                case 29:
                case 30:
                case 31:
                case 32:
                    #region
                    for (int intI = 141 + (tbPage.SelectedIndex - 26) * 8; intI < 141 + (tbPage.SelectedIndex - 25) * 8; intI++)
                    {
                        MyImg oImg = (this.Controls.Find("Img" + intI.ToString(), true)[0] as MyImg);
                        if (oImg != null)
                        {
                            byte[] ArayTmp = HDLPF.RemoteIconToByte(oImg.oImg, oImg.oImg.Width, oImg.oImg.Height);

                            writeValueBuffer = new byte[64];
                            writeValueBuffer[0] = 1;
                            writeValueBuffer[1] = 0xA1;  // write thing
                            writeValueBuffer[2] = 0x6;   // DLP
                            writeValueBuffer[3] = (byte)((intI - 141) / 8 + 1);   // page no

                            if ((intI - 140) % 8 == 0)
                                writeValueBuffer[4] = 8;
                            else
                                writeValueBuffer[4] = (byte)((intI - 140) % 8);  // button no

                            for (int intJ = 0; intJ < ArayTmp.Length / 58 + 1; intJ++)
                            {
                                writeValueBuffer[5] = (byte)(intJ + 1);

                                if (ArayTmp.Length > intJ * 58 + 58)
                                {
                                    Array.Copy(ArayTmp, intJ * 58, writeValueBuffer, 6, 58);
                                }
                                else
                                {
                                    Array.Copy(ArayTmp, intJ * 58, writeValueBuffer, 6, ArayTmp.Length % 58);
                                }

                                mintNowSending = writeValueBuffer[4];

                                WritetxtData();

                                t1 = DateTime.Now;
                                while (mintNowSending != -1)
                                {
                                    t2 = DateTime.Now;
                                    int TimeBetw = HDLSysPF.Compare(t2, t1);
                                    if (TimeBetw >= CsConst.replySpanTimes)
                                    {
                                        break;
                                    }
                                    HDLUDP.TimeBetwnNext(20);
                                }
                            }
                        }
                    }
                    #endregion
                    break;
                default:
                    #region
                    for (int intI = 189 + (tbPage.SelectedIndex - 32) * 8; intI < 189 + (tbPage.SelectedIndex - 31) * 8; intI++)
                    {
                        MyImg oImg = (this.Controls.Find("Img" + intI.ToString(), true)[0] as MyImg);
                        if (oImg != null)
                        {
                            byte[] ArayTmp = HDLPF.RemoteIconToByte(oImg.oImg, oImg.oImg.Width, oImg.oImg.Height);

                            writeValueBuffer = new byte[64];
                            writeValueBuffer[0] = 1;
                            writeValueBuffer[1] = 0xA1;  // write thing
                            writeValueBuffer[2] = 0x7;   // DLP
                            writeValueBuffer[3] = (byte)((intI - 189) / 8 + 1);   // page no

                            if ((intI - 188) % 8 == 0)
                                writeValueBuffer[4] = 8;
                            else
                                writeValueBuffer[4] = (byte)((intI - 188) % 8);  // button no

                            for (int intJ = 0; intJ < ArayTmp.Length / 58 + 1; intJ++)
                            {
                                writeValueBuffer[5] = (byte)(intJ + 1);

                                if (ArayTmp.Length > intJ * 58 + 58)
                                {
                                    Array.Copy(ArayTmp, intJ * 58, writeValueBuffer, 6, 58);
                                }
                                else
                                {
                                    Array.Copy(ArayTmp, intJ * 58, writeValueBuffer, 6, ArayTmp.Length % 58);
                                }

                                mintNowSending = writeValueBuffer[4];

                                WritetxtData();

                                t1 = DateTime.Now;
                                while (mintNowSending != -1)
                                {
                                    t2 = DateTime.Now;
                                    int TimeBetw = HDLSysPF.Compare(t2, t1);
                                    if (TimeBetw >= CsConst.replySpanTimes)
                                    {
                                        break;
                                    }
                                    HDLUDP.TimeBetwnNext(20);
                                }
                            }
                        }
                    }
                    #endregion
                    break;

            }
            Cursor.Current = Cursors.Default;
        }

        private void cboDevicePath_SelectedIndexChanged(object sender, EventArgs e)
        {
            myintUSB = Convert.ToInt32(cboDevicePath.Text.Split('-')[0].ToString());
        }


        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            byte bytTag = byte.Parse(((ToolStripButton)sender).Tag.ToString());
            if (bytTag == 0)
            {
                ((ToolStripButton)sender).Tag = 1;
            }
            else
            {
                ((ToolStripButton)sender).Tag = 0;
            }
        }

        private void tslDIY_Click(object sender, EventArgs e)
        {
            Form form = null;
            bool isOpen = true;
            foreach (Form frm in Application.OpenForms)
            {
                if (frm.Name == "frmDIY")
                {
                    form = frm;
                    form.TopMost = true;
                    this.WindowState = FormWindowState.Normal;
                    form.Activate();
                    form.TopMost = false;
                    isOpen = false;
                    break;
                }
            }
            if (isOpen)
            {
                //frmDIY frmNew = new frmDIY();
                //frmNew.Show();
            }
        }

        private void frmRemote_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MyBackGroup != null && MyBackGroup.IsBusy) MyBackGroup.CancelAsync();
        }
    }
}
