using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Windows.Forms;
using System.Drawing;
using System.Data.OleDb;
using System.Drawing.Imaging;
using System.Management;
using System.Runtime.InteropServices;
using System.Net.NetworkInformation;

namespace HDL_Buspro_Setup_Tool
{
   public  class HDLPF
    {
       [DllImport("ntdll.dll")]
       public static extern int RtlCompareMemory(IntPtr Destination, IntPtr Source, int Length);

       public static Boolean IsHasSameBufferArrayInList(Byte[] BufferArray, List<Byte[]> TmpListBuffer)
       {
           Boolean IsHasSameData = false;
           foreach (Byte[] Tmp in TmpListBuffer)
           {
               int id = RtlCompareMemory(Marshal.UnsafeAddrOfPinnedArrayElement(Tmp, 0),
                   Marshal.UnsafeAddrOfPinnedArrayElement(BufferArray, 0), sizeof(int) * Tmp.Length);
               if (id >= Tmp.Length)
               {
                   IsHasSameData = true;
                   break;
               }
           }
           return IsHasSameData;
       }

       public static UVCMD.DeviceAllIRInfo[] selectIRIndex()
       {
           int TestID = 0;
           UVCMD.DeviceAllIRInfo[] devAllIRInfo = new UVCMD.DeviceAllIRInfo[6];
           try
           {
               for (int i = 1; i <= 6; i++)
               {
                   string sql = string.Format("select * from dbNewIBrand where ID={0} order by DIndex", i);
                   OleDbDataReader dr = DataModule.SearchAResultSQLDB(sql);
                   UVCMD.DeviceAllIRInfo temp = new UVCMD.DeviceAllIRInfo();
                   temp.ID = i;
                   string strDev = CsConst.NewIRLibraryDeviceType[i - 1];
                   temp.DevRemark = strDev;
                   if (dr == null) return null; ;

                   temp.brand = new List<UVCMD.brandInfo>();
                   if (dr.HasRows)
                   {
                       while (dr.Read())
                       {
                           UVCMD.brandInfo tmp = new UVCMD.brandInfo();
                           tmp.brandID = dr.GetInt32(1);
                           TestID = tmp.brandID;
                           tmp.brandRemark = dr.GetValue(6).ToString();
                           if (CsConst.iLanguageId == 1)
                               tmp.brandRemark = dr.GetValue(2).ToString();
                           //tmp.IRInfo = new List<UVCMD.IRCode>();
                           string str = dr.GetValue(5).ToString();
                           string str1 = str.Split(',')[0];
                           string str2 = str.Split('}')[0];
                           str2 = str2.Substring(str1.Length + 1, str2.Length - str1.Length - 1);
                           str2 = str2.Trim();
                           str1 = str1.Trim();
                           if (str1.Length > 1)
                               str1 = str.Substring(1, str1.Length - 1);
                           temp.brandCount = Convert.ToInt32(str1);
                           if (temp.brandCount > 1)
                           {
                               int intTmp = 0;
                               for (int j = 0; j < str2.Length; j++)
                               {
                                   string strTmp = str2.Substring(j, 1);
                                   if (strTmp == ",")
                                   {
                                       intTmp = intTmp + 1;
                                   }
                               }
                               str2.Replace(" ", "");
                               tmp.IRIndexIntAry = new int[intTmp + 1];
                               tmp.IRcount = intTmp + 1;
                               for (int j = 0; j <= intTmp; j++)
                               {
                                   tmp.IRIndexIntAry[j] = Convert.ToInt32(str2.Split(',')[j]);
                               }
                           }
                           else
                           {
                               tmp.IRcount = 1;
                               tmp.IRIndexIntAry = new int[1];
                               tmp.IRIndexIntAry[0] = Convert.ToInt32(str2);
                           }

                           /*for (int k = 0; k < tmp.IRcount; k++)
                           {
                               UVCMD.IRCode irtemp = new UVCMD.IRCode();
                               sql = string.Format("select * from dbNewICode where ID={0} and DIndex={1}", i, tmp.IRIndexIntAry[k]);
                               OleDbDataReader drtmp = Public.DataModule.SearchAResultSQLDB(sql);
                               irtemp.KeyID = tmp.IRIndexIntAry[k];
                               irtemp.Remark = "";
                               if (drtmp == null) return false;
                               if (drtmp.HasRows)
                               {
                                   while (drtmp.Read())
                                   {
                                       irtemp.Codes = drtmp.GetValue(2).ToString();
                                       irtemp.IRLength = irtemp.Codes.Length;
                                       tmp.IRInfo.Add(irtemp);
                                   }
                                   drtmp.Close();
                               }
                           }*/
                           temp.brand.Add(tmp);
                       }
                       dr.Close();
                   }
                   temp.brandCount = temp.brand.Count;
                   devAllIRInfo[i - 1] = temp;
               }
           }
           catch
           {
               return null;
           }
           return devAllIRInfo;
       }

        /// <summary>
        /// read skin file
        /// </summary>
        /// <returns></returns>
        public static int ReadSkin()
        {
            System.IO.StreamReader sr = new StreamReader(Application.StartupPath + @"\Skin\Default.txt");
            int skinType = 0;
            int.TryParse(sr.ReadLine(), out skinType);
            sr.Close();
            return skinType;
        }


        // <summary>
        ///  保存对话框
        /// </summary>
        /// <returns></returns>
        public static string SaveFileDialogAccordingly(string strCaption, string strFilter)
        {
            //string localFilePath, fileNameExt, newFileName, FilePath;   
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            //设置文件类型   
            saveFileDialog1.Filter = strFilter;

            //设置默认文件类型显示顺序   
            saveFileDialog1.FilterIndex = 2;

            //保存对话框是否记忆上次打开的目录   
            saveFileDialog1.RestoreDirectory = true;

            //保存对话框的名称
            saveFileDialog1.Title = strCaption;

            string localFilePath = null;
            //点了保存按钮进入   
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //获得文件路径   
                localFilePath = saveFileDialog1.FileName.ToString();

                //获取文件名，不带路径   
                string fileNameExt = localFilePath.Substring(localFilePath.LastIndexOf("\\") + 1);

                //获取文件路径，不带文件名   
                string FilePath = localFilePath.Substring(0, localFilePath.LastIndexOf("\\"));

                // System.IO.FileStream fs = (System.IO.FileStream)saveFileDialog1.OpenFile();//输出文件   //fs输出带文字或图片的文件，就看需求了   
                System.IO.File.Copy(CsConst.mstrDefaultPath, localFilePath, true);
            }
            return localFilePath;
        }


        /// <summary>
        /// write skin file
        /// </summary>
        /// <returns></returns>
        public static void WriteSkin(int intTag)
        {
            System.IO.StreamWriter sr = new StreamWriter(Application.StartupPath + @"\Skin\Default.txt");
            sr.WriteLine(intTag.ToString());
            sr.Close();
        }

        /// <summary>
        /// read ip 
        /// </summary>
        /// <returns></returns>
        public static string GetLocalIP()
        {
            string strIP = "";
            CsConst.mstrActiveIPs = new List<string>();
            IPAddress[] arrIPAddresses = Dns.GetHostAddresses(Dns.GetHostName());

            foreach (IPAddress ip in arrIPAddresses)
            {
                if (ip.AddressFamily.Equals(AddressFamily.InterNetwork))
                {
                    CsConst.mstrActiveIPs.Add(ip.ToString());
                }
            }
            strIP = CsConst.mstrINIDefault.IniReadValue("ProgramMode", "IP", "");
            if (strIP == null || strIP == "" || !CsConst.mstrActiveIPs.Contains(strIP))
            {
                if (CsConst.mstrActiveIPs.Count != 0)
                    strIP = CsConst.mstrActiveIPs[0];
            }
            return strIP;
        }



        /// <summary>
        ///  保存对话框
        /// </summary>
        /// <returns></returns>
        public static string SaveFileDialog(string strCaption)
        {
            //string localFilePath, fileNameExt, newFileName, FilePath;   
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            //设置文件类型   
            saveFileDialog1.Filter = "mdb files (*.mdb)|*.mdb";

            //设置默认文件类型显示顺序   
            saveFileDialog1.FilterIndex = 2;

            //保存对话框是否记忆上次打开的目录   
            saveFileDialog1.RestoreDirectory = true;

            //保存对话框的名称
            saveFileDialog1.Title = strCaption;

            string localFilePath = null;
            //点了保存按钮进入   
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //获得文件路径   
                localFilePath = saveFileDialog1.FileName.ToString();

                //获取文件名，不带路径   
                string fileNameExt = localFilePath.Substring(localFilePath.LastIndexOf("\\") + 1);

                //获取文件路径，不带文件名   
                string FilePath = localFilePath.Substring(0, localFilePath.LastIndexOf("\\"));

                // System.IO.FileStream fs = (System.IO.FileStream)saveFileDialog1.OpenFile();//输出文件   //fs输出带文字或图片的文件，就看需求了   
                System.IO.File.Copy(CsConst.mstrDefaultPath, localFilePath, true);
            }
            return localFilePath;
        }

        /// <summary>
        ///  打开对话框
        /// </summary>
        /// <param name="strFilter"></param>
        /// <returns></returns>
        public static string OpenFileDialog(string strFilter, string strCaption)
        {
            //string localFilePath, fileNameExt, newFileName, FilePath;   
            OpenFileDialog OpenFileDialog1 = new OpenFileDialog();

            //设置文件类型   
            OpenFileDialog1.Filter = strFilter; //"mdb files (*.mdb)|*.mdb";

            //设置默认文件类型显示顺序   
            OpenFileDialog1.FilterIndex = 2;

            //保存对话框是否记忆上次打开的目录   
            OpenFileDialog1.RestoreDirectory = true;

            //保存对话框的名称
            OpenFileDialog1.Title = "Project Name";

            string localFilePath = null;
            //点了保存按钮进入   
            if (OpenFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //获得文件路径   
                localFilePath = OpenFileDialog1.FileName.ToString();

                //获取文件名，不带路径   
                string fileNameExt = localFilePath.Substring(localFilePath.LastIndexOf("\\") + 1);

                //获取文件路径，不带文件名   
                string FilePath = localFilePath.Substring(0, localFilePath.LastIndexOf("\\"));

                //System.IO.FileStream fs = (System.IO.FileStream)OpenFileDialog1.OpenFile();//输出文件   //fs输出带文字或图片的文件，就看需求了   

            }
            return localFilePath;
        }


        public static byte[] BackgroundColorfulImageToByte(Bitmap pic, int width, int height)
        {
            if (pic == null) return null;
            Bitmap bitmap = new Bitmap((Image)pic.Clone());
            bitmap.RotateFlip(RotateFlipType.Rotate270FlipNone); // 旋转90°

            //2015 12 25 ruby
            int intEachFileLength = width * height / 2;
            Byte[] buffer = new Byte[5];// ConvertorBackgroundToBufferFirstStep(bitmap);
            //调用DLL
            Byte[] CompressedBuffer = new Byte[intEachFileLength];
            /*
            unsafe
            {
                fixed (Byte* SourcePiexls = &buffer[0])
                {
                    fixed (Byte* DestinationPiexls = &CompressedBuffer[0])
                    {
                        //CompressImage(SourcePiexls, width, height, DestinationPiexls, 1);
                        CompressImage(
                    }
                }
            }
             * */
            //CompressImage(ref buffer[0], width, height, ref CompressedBuffer[0], 1);
            //Squish.Squish.CompressImage(buffer, width, height, ref CompressedBuffer, SquishFlags.kDxt1);

            // LCD屏特殊处理 以宽度为一个单位处理
            Byte[] TmpCompressedBufferB0B1 = new Byte[intEachFileLength / 2];
            Byte[] FinallyCompressedBufferB0B1 = new Byte[intEachFileLength / 2];
            Byte[] finalyOutputBufferC0C1B0B1 = new Byte[intEachFileLength];

            for (int i = 0; i < intEachFileLength / 8; i++)
            {
                // c0  C1
                Array.Copy(CompressedBuffer, i * 8, finalyOutputBufferC0C1B0B1, i * 2, 2);
                Array.Copy(CompressedBuffer, i * 8 + 2, finalyOutputBufferC0C1B0B1, i * 2 + intEachFileLength / 4, 2);

                // 后面直接拷贝
                Array.Copy(CompressedBuffer, i * 8 + 4, TmpCompressedBufferB0B1, i * 4, 4);
            }

            FinallyCompressedBufferB0B1 = FT80ICColorReorder(TmpCompressedBufferB0B1);

            Array.Copy(FinallyCompressedBufferB0B1, 0, finalyOutputBufferC0C1B0B1, intEachFileLength / 2, intEachFileLength / 2);
            System.IO.File.WriteAllBytes(@"c:\test.raw", finalyOutputBufferC0C1B0B1);
            return finalyOutputBufferC0C1B0B1;
        }

        public static byte[] Pic2ByteBuf(Image oImg)
        {
            if (oImg == null) return null;
            Bitmap Tmp = (Bitmap)oImg.Clone();

            MemoryStream ms = new MemoryStream();
            if (oImg == null) return new byte[ms.Length];
            oImg.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            byte[] BPicture = new byte[ms.Length];
            BPicture = ms.GetBuffer();
            return BPicture;
        }

        /// <summary>
        /// read pic content to byte array
        /// </summary>
        /// <param name="strPath"></param>
        /// <returns></returns>
        public static byte[] PicToByteBuf(string strPath)
        {
            if (strPath == null) return null;
            byte[] source = null;
            if (!strPath.Equals("") && File.Exists(strPath))
            {
                FileStream fs = new FileStream(strPath, FileMode.Open, FileAccess.Read);//创建文件流

                source = new byte[fs.Length];

                fs.Read(source, 0, source.Length);
                fs.Flush();
                fs.Close();
            }
            return source;
        }

        /// <summary>
        /// converter byte array to pic
        /// </summary>
        /// <param name="bytaray"></param>
        /// <returns></returns>
        public static Image BytArayToPic(byte[] bytaray)
        {
            if (bytaray == null) return null;
            Image oimg = null;
            if (bytaray.Length > 0)
            {
                MemoryStream stream = new MemoryStream(bytaray, true);
                stream.Write(bytaray, 0, bytaray.Length);
                if (stream.Length != 1) oimg = new Bitmap(stream);
                stream.Close();
            }
            return oimg;
        }

        public static byte StringToByte(string InString)
        {
            byte intTmp = Convert.ToByte(InString, 16);

            return intTmp;
        }


        public static string GetStringFromTime(int time, string strSplit)
        {
            string strTmp = "0" + strSplit + "0";
            try
            {
                if (strSplit == ":")
                {
                    time = time % 3601;
                    strTmp = (time / 60).ToString() + strSplit + (time % 60).ToString();
                }
                else if (strSplit == ".")
                {
                    time = time % 3601;
                    strTmp = (time / 10).ToString() + strSplit + (time % 10).ToString();
                }
            }
            catch
            {
            }
            return strTmp;
        }

        public static string GetTimeFromString(string strTmp, char strSplit)
        {
            string strResult = "0";
            try
            {
                if (strTmp == CsConst.mstrInvalid) return "0";
                string[] ArayByt = strTmp.Split(strSplit);

                if (strSplit == ':')
                {
                    strResult = (byte.Parse(ArayByt[0]) * 60 + byte.Parse(ArayByt[1])).ToString();
                }
                else if (strSplit == '.')
                {
                    strResult = (byte.Parse(ArayByt[0]) * 10 + byte.Parse(ArayByt[1])).ToString();
                }
            }
            catch
            {
            }
            return strResult;
        }

        public static int GetTimeIntegerFromString(string strTmp, char strSplit)
        {
            int TimeValue = 0;
            string strResult = "0";
            try
            {
                if (strTmp == CsConst.mstrInvalid) return TimeValue;
                string[] ArayByt = strTmp.Split(strSplit);

                if (strSplit == ':')
                {
                    strResult = (byte.Parse(ArayByt[0]) * 60 + byte.Parse(ArayByt[1])).ToString();
                }
                else if (strSplit == '.')
                {
                    strResult = (byte.Parse(ArayByt[0]) * 10 + byte.Parse(ArayByt[1])).ToString();
                }
                TimeValue = Convert.ToInt16(strResult);
            }
            catch
            {
            }
            return TimeValue;
        }

        /// <summary>
        /// 获取时间字符串0.1s
        /// </summary>
        /// <param name="InString"></param>
        /// <returns></returns>
        public static string GetStringFrmTimeMs(int time)
        {
            string strTmp = null;

            time = time % 36001;
            strTmp = (time / 600).ToString() + ":" + (time / 10 % 60).ToString() + "." + (time % 10).ToString();

            return strTmp;
        }

        /// <summary>
        /// 获取时间字符串0.01s
        /// </summary>
        /// <param name="InString"></param>
        /// <returns></returns>
        public static string GetStringFrmTimeMs2(int time)
        {
            string strTmp = null;

            time = time % 60001;
            strTmp = (time / 100).ToString() + "." + (time / 10 % 10).ToString() + (time % 10).ToString();

            return strTmp;
        }

        public static string GetTimeFrmStringMs(string strTmp)
        {
            string strResult = null;
            string[] ArayByt = strTmp.Split(':');

            strResult = ((byte.Parse(ArayByt[0]) * 60 + byte.Parse(ArayByt[1].Split('.')[0])) * 10 + byte.Parse(ArayByt[1].Split('.')[1])).ToString();
            return strResult;
        }

        /// <summary>
        /// 获取时间数值0.01s
        /// </summary>
        /// <param name="InString"></param>
        /// <returns></returns>
        public static string GetTimeFrmStringMs2(string strTmp)
        {
            string strResult = null;
            string[] ArayByt = strTmp.Split('.');

            strResult = (byte.Parse(ArayByt[0]) * 100 + (byte.Parse(ArayByt[1]))).ToString();
            return strResult;
        }

        public static string IsRightStringMode(string strTmp)
        {
            string strResult = strTmp;
            if (strTmp != null && strTmp.Length > 20)
            {
                strResult = strTmp.Substring(0, 20);
            }
            else if (strTmp == null)
            {
                strResult = "";
            }
            return strResult.ToString();
        }

        public static string IsNumStringMode(string strTmp, int intStart, int intEnd)
        {
            if (strTmp == null || strTmp == CsConst.mstrInvalid || strTmp == "")  return intStart.ToString();
            string strResult = strTmp.ToString();

            if (strTmp != null)
            {
                try
                {
                    Convert.ToInt32(strTmp);
                }
                catch (Exception /*ex*/)
                {
                    strTmp = intStart.ToString();
                }

                if (Convert.ToInt32(strTmp) < intStart || Convert.ToInt32(strTmp) > intEnd)
                {
                    strResult = intStart.ToString();
                }
                else
                {
                    strResult = strTmp.ToString();
                }
            }
            else
            {
                strResult = intStart.ToString();
            }

            return strResult;
        }

        public static bool IsRightNumStringMode(string strTmp, int intStart, int intEnd)
        {
            if (strTmp == null) return false;
            if (strTmp.Contains(" ")) return false;
            if (strTmp.Contains(",")) return false;
            bool Result = true;
            try
            {
                if (strTmp != null)
                {
                    try
                    {
                        Convert.ToInt16(strTmp);
                    }
                    catch
                    {
                        strTmp = intStart.ToString();
                        return false;
                    }

                    if (Convert.ToInt16(strTmp) < intStart || Convert.ToInt16(strTmp) > intEnd)
                    {
                        Result = false;
                    }
                    else
                    {
                        Result = true;
                    }
                }
                else
                {
                    Result = false;
                }
            }
            catch 
            {
                return Result;
            }
            return Result;
        }

        public static void TraverseTree(TreeNode oNode)
        {
            if (oNode.Nodes.Count != 0)
            {
                foreach (TreeNode childNode in oNode.Nodes)
                {
                    childNode.Checked = oNode.Checked;
                }
                oNode.ExpandAll();
            }

        }

        public static string Byte2String(byte[] bytTmp)
        {
            string strOut = null;
            if (bytTmp == null || bytTmp.Length == 0) return "";
            try
            {
                if (bytTmp[0] == 0 && bytTmp[1] == 0) return "";
                if (bytTmp[0] == 255 && bytTmp[1] == 255 && bytTmp[2] == 255) return "";

                for (int i = 0; i < bytTmp.Length; i++)
                {
                    if (bytTmp[i] == 0) // 255 在俄罗斯里面表示特殊字符
                    {
                        // 后面全部变成0
                        for (int j = i; j < bytTmp.Length; j++) { bytTmp[j] = 0; }
                        break;
                    }
                }
                strOut = System.Text.ASCIIEncoding.Default.GetString(bytTmp);
                strOut = strOut.TrimStart('\0');
                strOut = strOut.TrimEnd('\0');
            }
            catch { }
            return strOut.Trim();
        }

        public static string Byte22String(byte[] bytTmp, bool blnVerse)
        {
            string strOut = null;
            try
            {
                if (blnVerse)
                {
                    for (int intI = 0; intI < bytTmp.Length / 2; intI++)
                    {
                        byte bytVale = bytTmp[intI * 2];
                        bytTmp[intI * 2] = bytTmp[intI * 2 + 1];
                        bytTmp[intI * 2 + 1] = bytVale;
                    }
                }
                UnicodeEncoding encoding = new UnicodeEncoding();
                strOut = encoding.GetString(bytTmp);

                //strOut = System.Text.Encoding.ASCII.GetString(bytTmp).ToString();
                strOut = strOut.Replace("\0", "");
                strOut = strOut.TrimEnd('\0');
            }
            catch { }
            return strOut.Trim();
        }

        public static byte[] GetPiexFromBitmap(byte[] bytImage)
        {
            if (bytImage == null) return null;
            Bitmap myBitmap = (Bitmap)HDLPF.BytArayToPic(bytImage);  //创建图像 
            PixelFormat Tmp = myBitmap.PixelFormat;
            BitmapData data = myBitmap.LockBits(new Rectangle(0, 0, myBitmap.Width, myBitmap.Height), ImageLockMode.ReadWrite, Tmp);
            //循环处理 

            byte[] bytTmp = new byte[data.Width * data.Height];
            /*
            unsafe
            {
                byte* ptr = (byte*)(data.Scan0);
                for (int i = 0; i < data.Height; i++)
                {
                    string strTmp = (i + 1).ToString() + ":";
                    for (int j = 0; j < data.Width; j++)
                    {
                        bytTmp[i * 80 + j] = *ptr;

                        if (bytTmp[i * 80 + j] == 255)
                        {
                            bytTmp[i * 80 + j] = 0;
                        }
                        else
                        {
                            bytTmp[i * 80 + j] = 1;
                        }

                        if (Tmp == PixelFormat.Format1bppIndexed)
                        { ptr++; }
                        else
                        { ptr += 3; }
                    }
                    ptr += data.Stride - data.Width * 3;
                }
            }
             * */
            return bytTmp;
        }

        /// <summary>
        /// 将读出数据转换为组合数据
        /// </summary>
        /// <param name="ArayImage"></param>
        /// <returns></returns>
        public static byte[] SetPiex2BINBuffer(byte[] ArayImage)
        {
            if (ArayImage == null) return null;

            byte[] ArayResult = new byte[ArayImage.Length / 8];
            for (int i = 0; i < ArayResult.Length; i++)
            {
                int bytResult = ArayImage[i * 8]
                              + (ArayImage[i * 8 + 1] << 1)
                              + (ArayImage[i * 8 + 2] << 2)
                              + (ArayImage[i * 8 + 3] << 3)
                              + (ArayImage[i * 8 + 4] << 4)
                              + (ArayImage[i * 8 + 5] << 5)
                              + (ArayImage[i * 8 + 6] << 6)
                              + (ArayImage[i * 8 + 7] << 7);
                ArayResult[i] = (byte)bytResult;
            }
            return ArayResult;
        }

        /// <summary>
        /// DLP Remote 上传图片
        /// </summary>
        /// <param name="pic"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static byte[] ImageToByte(Image pic, int width, int height)
        {
            if (pic == null) return null;
            Bitmap bitmap = new Bitmap(pic);
            byte[] buffer = new byte[width * height];
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    byte va = 0;
                    if (bitmap.GetPixel(j, i).R != 255)//白
                    {
                        va = 1;
                    }
                    else
                    {
                        va = 0;
                    }
                    buffer[i * width + j] = va;
                }
            }
            byte[] buf = new byte[buffer.Length / 8];

            for (int i = 0; i < buf.Length; i++)
            {
                int bytResult = buffer[i * 8]
                              + (buffer[i * 8 + 1] << 1)
                              + (buffer[i * 8 + 2] << 2)
                              + (buffer[i * 8 + 3] << 3)
                              + (buffer[i * 8 + 4] << 4)
                              + (buffer[i * 8 + 5] << 5)
                              + (buffer[i * 8 + 6] << 6)
                              + (buffer[i * 8 + 7] << 7);
                buf[i] = (byte)bytResult;
            }

            //for (int i = 0; i < buf.Length; i++)
            //{
            //    buf[i] = Convert.ToByte(buffer.Substring(i * 8, 8), 2);
            //}
            return buf;
        }

        /// <summary>
        /// 从读取数据中转化为图片
        /// </summary>
        /// <param name="ArayTmp"></param>
        /// <returns></returns>
        public static Image ByteToImage(byte[] bytImage, int width, int height)
        {
            if (bytImage == null) return null;
            Bitmap myBitmap = new Bitmap(width, height, PixelFormat.Format32bppPArgb);  //创建图像 

            //循环处理 
            //unsafe
            {
                #region
                for (int intI = 0; intI < height; intI++)
                {
                    for (int j = 0; j < width / 8; j++)
                    {
                        byte bytTmp = bytImage[intI * width / 8 + j];

                        for (int i = 0; i < 8; i++)
                        {
                            if ((bytTmp & (1 << i)) == (1 << i))
                            {
                                myBitmap.SetPixel(j * 8 + i, intI, Color.Black);
                            }
                            else
                            {
                                myBitmap.SetPixel(j * 8 + i, intI, Color.White);
                            }
                        }
                    }
                }
                #endregion
            }
            return myBitmap;
        }


        public static Bitmap ByteToImage(byte[] bytImage, int width, int height, int intDeviceType)
        {
            if (bytImage == null) return null;
            Bitmap myBitmap = new Bitmap(width, height, PixelFormat.Format32bppPArgb);  //创建图像 

            //循环处理 
            unsafe
            {
                if (CsConst.mintDLPDeviceType.Contains(intDeviceType) && !CsConst.mintNewDLPFHSetupDeviceType.Contains(intDeviceType))
                {
                    #region
                    int intTmp = 0;
                    for (int intI = 0; intI < height; intI++)
                    {
                        for (int j = 0; j < width / 8; j++)
                        {
                            byte bytTmp = bytImage[intTmp];

                            for (int i = 0; i < 8; i++)
                            {
                                if ((bytTmp & (1 << i)) == (1 << i))
                                {
                                    myBitmap.SetPixel(j * 8 + i, intI, Color.Black);
                                }
                                else
                                {
                                    myBitmap.SetPixel(j * 8 + i, intI, Color.White);
                                }
                            }
                            intTmp++;
                        }
                    }
                    #endregion
                }
                else if (CsConst.mintNewDLPFHSetupDeviceType.Contains(intDeviceType) && !CsConst.mintMPTLDeviceType.Contains(intDeviceType))
                {
                    #region
                    int intTmp = 0;
                    for (int intID = 0; intID < 4; intID++) //循环写左边像素点
                    {
                        for (int j = 0; j < height / 4; j++)
                        {
                            for (int i = 0; i < width / 16; i++)
                            {
                                byte bytTmp = bytImage[intTmp];

                                for (int inti = 0; inti < 8; inti++)
                                {
                                    if ((bytTmp & (1 << inti)) == (1 << inti))
                                    {
                                        myBitmap.SetPixel(i * 8 + inti, j + intID * 32, Color.Black);
                                    }
                                    else
                                    {
                                        myBitmap.SetPixel(i * 8 + inti, j + intID * 32, Color.White);
                                    }
                                }
                                intTmp++;
                            }
                        }

                        for (int j = 0; j < height / 4; j++)
                        {
                            for (int i = width / 16; i < width / 8; i++) // 右边
                            {
                                byte bytTmp = bytImage[intTmp];

                                for (int inti = 0; inti < 8; inti++)
                                {
                                    if ((bytTmp & (1 << inti)) == (1 << inti))
                                    {
                                        myBitmap.SetPixel(i * 8 + inti, j + intID * 32, Color.Black);
                                    }
                                    else
                                    {
                                        myBitmap.SetPixel(i * 8 + inti, j + intID * 32, Color.White);
                                    }
                                }
                                intTmp++;
                            }
                        }
                    }
                    #endregion
                }
                else if (CsConst.mintMPTLDeviceType.Contains(intDeviceType))
                {
                    #region
                    int intTmp = 0;
                    for (int intID = 0; intID < 4; intID++) //循环写左边像素点
                    {
                        for (int j = 0; j < height / 4; j++)
                        {
                            for (int i = 0; i < width / 16; i++)
                            {
                                byte bytTmp = bytImage[intTmp];

                                for (int inti = 0; inti < 8; inti++)
                                {
                                    if ((bytTmp & (1 << inti)) == (1 << inti))
                                    {
                                        myBitmap.SetPixel(i * 8 + inti, j + intID * 48, Color.Black);
                                    }
                                    else
                                    {
                                        myBitmap.SetPixel(i * 8 + inti, j + intID * 48, Color.White);
                                    }
                                }
                                intTmp++;
                            }
                        }

                        for (int j = 0; j < height / 4; j++)
                        {
                            for (int i = width / 16; i < width / 8; i++) // 右边
                            {
                                byte bytTmp = bytImage[intTmp];

                                for (int inti = 0; inti < 8; inti++)
                                {
                                    if ((bytTmp & (1 << inti)) == (1 << inti))
                                    {
                                        myBitmap.SetPixel(i * 8 + inti, j + intID * 48, Color.Black);
                                    }
                                    else
                                    {
                                        myBitmap.SetPixel(i * 8 + inti, j + intID * 48, Color.White);
                                    }
                                }
                                intTmp++;
                            }
                        }
                    }
                    #endregion
                }
            }
            return myBitmap;
        }

        /// <summary>
        /// DLP Remote 上传图片
        /// </summary>
        /// <param name="pic"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static byte[] ImageToByte(Bitmap pic, int width, int height, byte bytType)
        {
            if (pic == null) return null;
            Bitmap bitmap = new Bitmap((Image)pic.Clone());
            byte[] buffer = new byte[width * height];

            if (bytType == 0) //三代面板
            {
                #region
                for (int i = 0; i < height; i++)
                {
                    for (int j = 0; j < width; j++)
                    {
                        byte va = 0;
                        Color oTmp = bitmap.GetPixel(j, i);
                        // if (bitmap.GetPixel(j, i).R > 120 || ((bitmap.GetPixel(j, i).R == 0) && (bitmap.GetPixel(j, i).G > 125)))//白
                        //   int luma = (int)(bitmap.GetPixel(i, j + intID * 32).R * 0.299 + bitmap.GetPixel(i, j + intID * 32).G * 0.59 + bitmap.GetPixel(i, j + intID * 32).B * 0.11);
                        if (oTmp.R == oTmp.G && oTmp.G == oTmp.B && oTmp.R <= 220)
                        {
                            va = 1;
                        }
                        else
                        {
                            if (bitmap.GetPixel(j, i).R > 120 || ((bitmap.GetPixel(j, i).R == 0) && (bitmap.GetPixel(j, i).G > 125)))//白
                            {
                                va = 0;
                            }
                            else
                            {
                                va = 1;
                            }
                        }
                        buffer[i * width + j] = va;
                    }
                }
                #endregion
            }
            else if (bytType == 1) //四代面板
            {
                #region
                int intTotal = 0;
                for (int intID = 0; intID < 4; intID++) //循环写左边像素点
                {
                    for (int j = 0; j < height / 4; j++)
                    {
                        for (int i = 0; i < width / 2; i++)
                        {
                            byte va = 0;
                            Color oTmp = bitmap.GetPixel(i, j + intID * 32);
                            // if (bitmap.GetPixel(j, i).R > 120 || ((bitmap.GetPixel(j, i).R == 0) && (bitmap.GetPixel(j, i).G > 125)))//白
                            //   int luma = (int)(bitmap.GetPixel(i, j + intID * 32).R * 0.299 + bitmap.GetPixel(i, j + intID * 32).G * 0.59 + bitmap.GetPixel(i, j + intID * 32).B * 0.11);
                            if (oTmp.R == oTmp.G && oTmp.G == oTmp.B && oTmp.R <= 220)
                            {
                                va = 1;
                            }
                            else
                            {
                                if (oTmp.R >= 120 || (oTmp.R == 0) && (oTmp.G > 125))//白
                                {
                                    // if (oTmp.R == oTmp.G && oTmp.G == oTmp.B) va = 1;
                                    va = 0;
                                }
                                else
                                {
                                    va = 1;
                                }
                            }
                            buffer[intTotal] = va;
                            intTotal++;
                        }
                    }

                    for (int j = 0; j < height / 4; j++)
                    {
                        for (int i = width / 2; i < width; i++) // 右边
                        {
                            byte va = 0;
                            //if (bitmap.GetPixel(i, j + intID * 32).R >= 100)//白
                            Color oTmp = bitmap.GetPixel(i, j + intID * 32);
                            // if (bitmap.GetPixel(j, i).R > 120 || ((bitmap.GetPixel(j, i).R == 0) && (bitmap.GetPixel(j, i).G > 125)))//白
                            //   int luma = (int)(bitmap.GetPixel(i, j + intID * 32).R * 0.299 + bitmap.GetPixel(i, j + intID * 32).G * 0.59 + bitmap.GetPixel(i, j + intID * 32).B * 0.11);
                            if (oTmp.R == oTmp.G && oTmp.G == oTmp.B && oTmp.R <= 220)
                            {
                                va = 1;
                            }
                            else
                            {
                                if ((bitmap.GetPixel(i, j + intID * 32).R >= 120 || (bitmap.GetPixel(i, j + intID * 32).R == 0) && (bitmap.GetPixel(i, j + intID * 32).G > 125)))//白
                                {
                                    va = 0;
                                }
                                else
                                {
                                    va = 1;
                                }
                            }
                            buffer[intTotal] = va;
                            intTotal++;
                        }
                    }
                }
                #endregion
            }
            else if (bytType == 2) //触摸面板
            {
                #region
                int intTotal = 0;
                for (int intID = 0; intID < 4; intID++) //循环写左边像素点
                {
                    for (int j = 0; j < height / 4; j++)
                    {
                        for (int i = 0; i < width / 2; i++)
                        {
                            byte va = 0;
                            Color oTmp = bitmap.GetPixel(i, j + intID * 48);
                            // if (bitmap.GetPixel(j, i).R > 120 || ((bitmap.GetPixel(j, i).R == 0) && (bitmap.GetPixel(j, i).G > 125)))//白
                            //   int luma = (int)(bitmap.GetPixel(i, j + intID * 32).R * 0.299 + bitmap.GetPixel(i, j + intID * 32).G * 0.59 + bitmap.GetPixel(i, j + intID * 32).B * 0.11);
                            if (oTmp.R == oTmp.G && oTmp.G == oTmp.B && oTmp.R <= 220)
                            {
                                va = 1;
                            }
                            else
                            {
                                //if (bitmap.GetPixel(i, j + intID * 48).R >= 100)//白
                                if ((bitmap.GetPixel(i, j + intID * 48).R >= 120 || (bitmap.GetPixel(i, j + intID * 48).R == 0) && (bitmap.GetPixel(i, j + intID * 48).G > 125)))//白
                                {
                                    va = 0;
                                }
                                else
                                {
                                    va = 1;
                                }
                            }
                            buffer[intTotal] = va;
                            intTotal++;
                        }
                    }

                    for (int j = 0; j < height / 4; j++)
                    {
                        for (int i = width / 2; i < width; i++) // 右边
                        {
                            byte va = 0;
                            Color oTmp = bitmap.GetPixel(i, j + intID * 48);
                            // if (bitmap.GetPixel(j, i).R > 120 || ((bitmap.GetPixel(j, i).R == 0) && (bitmap.GetPixel(j, i).G > 125)))//白
                            //   int luma = (int)(bitmap.GetPixel(i, j + intID * 32).R * 0.299 + bitmap.GetPixel(i, j + intID * 32).G * 0.59 + bitmap.GetPixel(i, j + intID * 32).B * 0.11);
                            if (oTmp.R == oTmp.G && oTmp.G == oTmp.B && oTmp.R <= 220)
                            {
                                va = 1;
                            }
                            else
                            {
                                //if (bitmap.GetPixel(i, j + intID * 48).R >= 100)//白
                                if ((bitmap.GetPixel(i, j + intID * 48).R >= 120 || (bitmap.GetPixel(i, j + intID * 48).R == 0) && (bitmap.GetPixel(i, j + intID * 48).G > 125)))//白
                                {
                                    va = 0;
                                }
                                else
                                {
                                    va = 1;
                                }
                            }
                            buffer[intTotal] = va;
                            intTotal++;
                        }
                    }
                }
                #endregion
            }
            byte[] buf = new byte[buffer.Length / 8];

            for (int i = 0; i < buf.Length; i++)
            {
                int bytResult = buffer[i * 8]
                              + (buffer[i * 8 + 1] << 1)
                              + (buffer[i * 8 + 2] << 2)
                              + (buffer[i * 8 + 3] << 3)
                              + (buffer[i * 8 + 4] << 4)
                              + (buffer[i * 8 + 5] << 5)
                              + (buffer[i * 8 + 6] << 6)
                              + (buffer[i * 8 + 7] << 7);
                buf[i] = (byte)bytResult;
            }
            return buf;
        }

        public static void GetRightIPAndPort()
        {
            HDLUDP.ConstPort = 6000;
            if (CsConst.myintProxy == 0)
            {
                string subnetMask = "255.255.255.0";

                #region
                Boolean blnGetMask = NetworkInterface.GetIsNetworkAvailable();
                //获取所有网络接口放在adapters中。
                if (blnGetMask != false)
                {
                    NetworkInterface[] adapters = System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces();
                    foreach (System.Net.NetworkInformation.NetworkInterface adapter in adapters)
                    {
                        //未启用的网络接口不要
                        if (adapter.OperationalStatus != System.Net.NetworkInformation.OperationalStatus.Up)
                        {
                            continue;
                        }

                        //不是以太网和无线网的网络接口不要
                        if (adapter.NetworkInterfaceType != System.Net.NetworkInformation.NetworkInterfaceType.Ethernet &&
                            adapter.NetworkInterfaceType != System.Net.NetworkInformation.NetworkInterfaceType.Wireless80211 &&
                            adapter.NetworkInterfaceType != System.Net.NetworkInformation.NetworkInterfaceType.Ppp)
                        {
                            continue;
                        }

                        //虚拟机的网络接口不要
                        if (adapter.Name.IndexOf("VMware") != -1 || adapter.Name.IndexOf("Virtual") != -1)
                        {
                            continue;
                        }

                        //获取IP地址和Mask地址
                        System.Net.NetworkInformation.IPInterfaceProperties ipif = adapter.GetIPProperties();
                        System.Net.NetworkInformation.UnicastIPAddressInformationCollection ipifCollection = ipif.UnicastAddresses;

                        foreach (System.Net.NetworkInformation.UnicastIPAddressInformation ipInformation in ipifCollection)
                        {
                            if (ipInformation.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                            {
                                if (ipInformation.Address.ToString() == CsConst.myLocalIP)
                                {
                                    subnetMask = ipInformation.IPv4Mask.ToString();//子网掩码
                                    blnGetMask = true;
                                    break;
                                }
                            }
                        }
                        if (blnGetMask == true) break;
                    }
                }
                #endregion

                byte[] ip = IPAddress.Parse(CsConst.myLocalIP).GetAddressBytes();
                byte[] sub = IPAddress.Parse(subnetMask).GetAddressBytes();

                // 广播地址=子网按位求反 再 或IP地址 
                for (int i = 0; i < ip.Length; i++)
                {
                    ip[i] = (byte)((~sub[i]) | ip[i]);
                }
                CsConst.myDestIP = new IPAddress(ip).ToString();
            }
        }

        public static string GetNumFromString(string strTmp)
        {
            if (strTmp == null) return "0";
            StringBuilder sb = new StringBuilder();
            foreach (char var in strTmp.ToCharArray())
            {
                bool flag = Char.IsNumber(var);
                if (flag == true) // 发现不是数字 跳出
                {
                    sb.Append(flag ? var.ToString() : string.Empty);
                }
                else
                {
                    if (sb != null) sb = new StringBuilder();
                }
            }

            return sb.ToString();
        }

        // 读入欲转换的图片并转成为 WritableBitmap www.it165.net 03.
        public static System.Drawing.Image ConvertToInvert(System.Drawing.Image img)
        {
            if (img == null) return null;
            System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(img);
            for (int y = 0; y < bitmap.Height; y++)
            {
                for (int x = 0; x < bitmap.Width; x++)
                {
                    // 取得每一个 pixel 09.            
                    var pixel = bitmap.GetPixel(x, y);
                    // 负片效果 将其反转 12.            
                    System.Drawing.Color newColor = System.Drawing.Color.FromArgb(pixel.A, 255 - pixel.R, 255 - pixel.G, 255 - pixel.B);
                    bitmap.SetPixel(x, y, newColor);
                }
            }
            return bitmap;
        }

        /// <summary>
        /// Remote 上传图片
        /// </summary>
        /// <param name="pic"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static byte[] RemoteIconToByte(Image pic, int width, int height)
        {
            if (pic == null) return null;
            Bitmap bitmap = new Bitmap(pic);

            byte bytTime = (byte)(height / 8);
            if (height % 8 != 0) bytTime = (byte)(bytTime + 1);

            byte[] buffer = new byte[bytTime * width];


            for (int i = 0; i < bytTime; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    byte va = 0;
                    string strTmp = "";
                    for (int k = 0; k < 8; k++)
                    {
                        if (i * 8 + k >= height) break;
                        if (bitmap.GetPixel(j, i * 8 + k).R != 255)//白
                        {
                            va = 1;
                            // bytResult = (byte)(bytResult | (byte)(va << k));
                        }
                        else
                        {
                            va = 0;
                        }
                        strTmp = va.ToString() + strTmp;
                    }
                    buffer[i * width + j] = Convert.ToByte(strTmp, 2);
                }
            }
            return buffer;
        }

        public static void UpdateBufferWhenchanged(int intTag, string strTmp, DataGridView oDGV, int myintRowIndex, int myintColIndex)
        {
            if (oDGV.SelectedCells == null) return;
            if (oDGV.SelectedRows == null) return;
            string strNo = HDLPF.GetNumFromString(strTmp);
            if (strNo == "") strNo = "0";

            switch (intTag)
            {
                case 0:
                    foreach (DataGridViewRow r in oDGV.SelectedRows)
                    {
                        if (r.Selected & r.Index != myintRowIndex)
                        {
                            oDGV[myintColIndex, r.Index].Value = strTmp;
                        }
                    }
                    break;
                case 1:
                    int intTmp = Convert.ToInt16(strNo);
                    foreach (DataGridViewRow r in oDGV.Rows)
                    {
                        if (r.Selected & r.Index != myintRowIndex)
                        {
                            intTmp++;
                            oDGV[myintColIndex, r.Index].Value = strTmp.Replace(strNo, intTmp.ToString());
                        }
                    }
                    break;
                case 2:
                    intTmp = Convert.ToInt16(strNo);
                    foreach (DataGridViewRow r in oDGV.Rows)
                    {
                        if (r.Selected & r.Index != myintRowIndex)
                        {
                            intTmp--;
                            if (intTmp <= 0) intTmp = 255;
                            oDGV[myintColIndex, r.Index].Value = strTmp.Replace(strNo, intTmp.ToString());
                        }
                    }
                    break;
            }
        }


        // 删除所选节点以及子节点		删除所选节点以及子节点
        #region
        public void DelTreeNodes(TreeNode oNode)
        {
            foreach (TreeNode tn in oNode.Nodes)
            {
                if (oNode.Nodes != null)  //定位哪个类型
                {
                    for (int intI = 0; intI < oNode.Nodes.Count; intI++)   //定位哪个红外码
                    {
                        if (oNode.Nodes[intI].StateImageIndex == 3)
                        {
                            //定位所有的红外码
                            oNode.Nodes.RemoveAt(intI);
                            DelTreeNodes(oNode);
                        }
                    }
                }
            }
        }
        #endregion


        private void ShowFileName(ListView ListViewControl, string folderPath, ImageList imageList1)
        {
            string[] FileName1 = Directory.GetFiles(folderPath);

            if (FileName1 == null)
            {
                ListViewControl.Clear();
                imageList1.Images.Clear();
            }
            else
            {
                imageList1.Images.Clear();
                ListViewControl.Clear();

                int NumberOfFiles = FileName1.Length;

                ListViewControl.View = View.LargeIcon;
                ListViewControl.LargeImageList = imageList1;
                ListViewControl.BeginUpdate();
                for (int j = 0; j < NumberOfFiles; j++)
                {
                    //DealWithImage si = new DealWithImage(FileName1[j]);
                    //Image Bitmap = si.GetReducedImage(0.2);
                    //Size BitmapSize = Bitmap.Size;

                    //imageList1.Images.Add(Bitmap);
                    //imageList1.ImageSize = BitmapSize;

                    //ListViewItem LVItem = new ListViewItem(Path.GetFileName(FileName1[j]));
                    //LVItem.ImageIndex = j;
                    //LVItem.Tag = FileName1[j];
                    //LVItem.SubItems.Add("文件");
                    //LVItem.SubItems.Add("");
                    //ListViewControl.Items.Add(LVItem);
                    //Cint = NumberOfFiles;
                    //this.toolStripStatusLabel1.Text = Cint + "个对象    " + Seleint + "个对象被选中";
                }

                ListViewControl.EndUpdate();

            }

        }

        public static void AddFolderIntern(string folderPath, ListView oLv, ImageList oImg)
        {
            // not using AllDirectories
            oLv.Controls.Clear();
            oImg.Images.Clear();
            string[] files = Directory.GetFiles(folderPath);
            int intI = 0;
            foreach (string file in files)
            {
                Image img = null;

                try
                {
                    img = Image.FromFile(file);
                    //img = img.GetThumbnailImage(80, 32, null, IntPtr.Zero);
                    // System.Drawing.Graphics.DrawImage(img, 80, 32);
                }
                catch
                {
                    // do nothing
                }

                if (img != null)
                {

                    oImg.Images.Add(img);
                    oLv.Items.Add(file.Substring(file.LastIndexOf(@"\") + 1), intI);
                    img.Dispose();
                    intI++;
                }
            }
        }

        public static void setParentNodeCheckedState(TreeNode currNode, bool state)
        {
            TreeNode parentNode = currNode.Parent;
            parentNode.Checked = state;
            if (currNode.Parent.Parent != null)
            {
                setParentNodeCheckedState(currNode.Parent, state);
            }
        }

        public static void setChildNodeCheckedState(TreeNode currNode, bool state)
        {
            TreeNodeCollection nodes = currNode.Nodes;
            if (nodes.Count > 0)
                foreach (TreeNode tn in nodes)
                {
                    tn.Checked = state;
                    setChildNodeCheckedState(tn, state);
                }
        }

        public static bool UploadOrReadGoOn(string strHint, byte bytType, bool blnShowMsg) // 0 下载  1 上传
        {
            bool blnResult = false;
            if (blnShowMsg)
            {
                string strTmp = CsConst.mstrINIDefault.IniReadValue("Read2Down", bytType.ToString("d5"), "");
                if (MessageBox.Show(strTmp, strHint, MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.OK)
                {
                    blnResult = true;
                }
            }
            else
            {
                blnResult = true;
            }
            return blnResult;
        }




        public static string GetPYString(string str)
        {
            string tempStr = "";
            if (str == null || str.Length == 0) return "";
            char c = str[0]; //当前在text里的中文            
            {
                if ((int)c >= 33 && (int)c <= 126)
                {
                    tempStr += c.ToString();
                }
                else
                {
                    tempStr += GetPYChar(c.ToString());
                }
            }
            return tempStr;
        }

        public static string GetPYChar(string c)
        {
            byte[] array = new byte[2];
            array = System.Text.Encoding.Default.GetBytes(c);
            int i = (short)(array[0] - '\0') * 256 + ((short)(array[1] - '\0'));
            if (i < 0xB0A1) return "*";
            if (i < 0xB0C5) return "A";
            if (i < 0xB2C1) return "B";
            if (i < 0xB4EE) return "C";
            if (i < 0xB6EA) return "D";
            if (i < 0xB7A2) return "E";
            if (i < 0xB8C1) return "F";
            if (i < 0xB9FE) return "G";
            if (i < 0xBBF7) return "H";
            if (i < 0xBFA6) return "G";
            if (i < 0xC0AC) return "K";
            if (i < 0xC2E8) return "L";
            if (i < 0xC4C3) return "M";
            if (i < 0xC5B6) return "N";
            if (i < 0xC5BE) return "O";
            if (i < 0xC6DA) return "P";
            if (i < 0xC8BB) return "Q";
            if (i < 0xC8F6) return "R";
            if (i < 0xCBFA) return "S";
            if (i < 0xCDDA) return "T";
            if (i < 0xCEF4) return "W";
            if (i < 0xD1B9 || i == 0xF6CE) return "X";
            if (i < 0xD4D1 || i == 0xDDBA) return "Y";
            if (i < 0xD7FA) return "Z";
            return "*";
        }


        public static Byte[] FT80ICColorReorder(Byte[] colorOrderLists)
        {
            //特殊版本，只可用于此型号屏的处理
            if (colorOrderLists == null) return null;
            int totalLength = colorOrderLists.Length;
            Byte[] resultFT80ColorLists = new Byte[totalLength];

            Byte[] TmpLines = new Byte[480];

            for (int packetOneByone = 0; packetOneByone < colorOrderLists.Length / 480; packetOneByone++)
            {
                Array.Copy(colorOrderLists, packetOneByone * 480, TmpLines, 0, 480);

                for (int i = 0; i < 4; i++)  //文件中的换行
                {
                    for (int k = 0; k <= 3; k++) //换行
                    {
                        // 每64个包的数据的第4个的第一个byte处理
                        #region
                        int intTmp = 8;
                        if (i == 3) intTmp = 6;
                        for (int j = 0; j < intTmp; j++) //FIRST LINE  奇数位
                        {
                            string tmpColorList = TmpLines[i * 128 + 4 + j * 16 + k].ToString("X2")
                                                + TmpLines[i * 128 + 0 + j * 16 + k].ToString("X2");
                            Byte[] Tmp = ReverseString(tmpColorList);
                            Array.Copy(Tmp, 0, resultFT80ColorLists, packetOneByone * 240 + i * 16 + k * 60 + j * 2, 1);
                            Array.Copy(Tmp, 1, resultFT80ColorLists, packetOneByone * 240 + i * 16 + k * 60 + j * 2 + totalLength / 2, 1);
                        }

                        for (int j = 0; j < intTmp; j++) //FIVE LINES LINE  偶数位
                        {
                            string tmpColorList = TmpLines[i * 128 + 12 + j * 16 + k].ToString("X2")
                                               + TmpLines[i * 128 + 8 + j * 16 + k].ToString("X2");
                            Byte[] Tmp = ReverseString(tmpColorList);
                            Array.Copy(Tmp, 0, resultFT80ColorLists, packetOneByone * 240 + i * 16 + k * 60 + j * 2 + 1, 1);
                            Array.Copy(Tmp, 1, resultFT80ColorLists, packetOneByone * 240 + i * 16 + k * 60 + j * 2 + 1 + totalLength / 2, 1);
                        }
                        #endregion
                    }
                }
            }
            return resultFT80ColorLists;
        }

        public static Byte[] ReverseString(string text)  // 0 or 1
        {
            string strTmp = Convert.ToString(Convert.ToInt64(text, 16), 2);
            Byte[] resultOrder = new Byte[8];

            //补 0 
            while (strTmp.Length != 16)
            {
                strTmp = "0" + strTmp;
            }
            //rtb1.Text += strTmp.ToString() + "\r\n";
            //倒置+ 转换
            string newString = "";
            for (int i = 0; i < 8; i++)
            {
                string oldString = strTmp.Substring(i * 2, 2); ;

                if (oldString == "01") oldString = "11";
                else if (oldString == "10") oldString = "01";
                else if (oldString == "11") oldString = "10";

                newString = oldString + newString;
            }
            // rtb1.Text += newString.ToString() + "\r\n";
            //取数
            string b1WordNumber = "";
            string b0WordNumber = "";
            Char[] tmpCharLost = newString.ToCharArray();
            for (int i = 0; i < 8; i++)
            {
                b0WordNumber += tmpCharLost[i * 2 + 1].ToString();
                b1WordNumber += tmpCharLost[i * 2].ToString();
            }
            resultOrder[0] = (Byte)(Convert.ToInt32(b0WordNumber, 2));
            resultOrder[1] = (Byte)(Convert.ToInt32(b1WordNumber, 2));
            return resultOrder;
            // MessageBox.Show(strTmp);
        }

        /// <summary>
        /// DLP Remote Colorful panels 上传图片
        /// </summary>
        /// <param name="pic"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static byte[] ColorfulImageToByte(Bitmap pic, int width, int height)
        {
            if (pic == null) return null;
            Bitmap bitmap = new Bitmap((Image)pic.Clone());
            bitmap.RotateFlip(RotateFlipType.Rotate270FlipNone); // 旋转90°
            byte[] buffer = new byte[width * height * 2];

            #region
            int intTotal = 0;
            for (int j = 0; j < height; j++)//循环写左边像素点
            {
                for (int i = 0; i < width; i++)
                {
                    System.Drawing.Color oTmp = bitmap.GetPixel(i, j);

                    Byte A = Convert.ToByte(oTmp.A & 0xF0);
                    Byte R = Convert.ToByte(Convert.ToByte(oTmp.R & 0xF0) >> 4);
                    Byte G = Convert.ToByte(oTmp.G & 0xF0);
                    Byte B = Convert.ToByte(Convert.ToByte(oTmp.B & 0xF0) >> 4);

                    buffer[intTotal] = (Byte)(G + B);
                    intTotal++;
                    buffer[intTotal] = (Byte)(A + R);
                    intTotal++;
                }
            }
            #endregion

            return buffer;
        }


        public static Byte GetIndexFromBufferLists(String[] arrStringList, String sTmpName)
        {
            Byte bResult = 0;
            try
            {
                Byte bTmp = 0;
                foreach (String sTmp in arrStringList)
                {
                    if (sTmpName == sTmp)
                    {
                        bResult = bTmp;
                        return bResult;
                        break;
                    }
                    bTmp++;
                }
            }
            catch
            {
                return bResult;
            }
            return bResult;
        }

    }
}
