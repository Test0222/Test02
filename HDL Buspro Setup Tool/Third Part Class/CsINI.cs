using System;
using System.Runtime.InteropServices;
using System.Text;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Reflection;
using System.Collections;

namespace HDL_Buspro_Setup_Tool
{
    /**/
    /// <summary>
    /// INI文件的操作类
    /// </summary>
    public class IniFile
    {
        public string Path;

        public IniFile(string path)
        {
            Path = path;
        }

        //    声明读写INI文件的API函数#region 声明读写INI文件的API函数 
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string defVal, StringBuilder retVal, int size, string filePath);

        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string defVal, Byte[] retVal, int size, string filePath);

        /**/
        /// <summary>
        /// 写INI文件
        /// </summary>
        /// <param name="section">段落</param>
        /// <param name="key">键</param>
        /// <param name="iValue">值</param>
        public void IniWriteValue(string section, string key, string iValue)
        {
            WritePrivateProfileString(section, key, iValue, Path);
        }
        //languageText
        #region
        public static void writevalue(string section, string key, string value)
        {
            WritePrivateProfileString(section, key, value, Application.StartupPath + @"\ini\languageText.ini");
        }
        public static void intwritevalue(string section, string[] Keys)
        {
            if (Keys == null) return;
            int intI = 0;
            foreach (string strTmp in Keys)
            {
                if (strTmp != "")
                {
                    WritePrivateProfileString(section, String.Format("{0:X5} ", intI), strTmp, Application.StartupPath + @"\ini\languageText.ini");
                }
                intI++;
            }
        }

        public static string readvalue(string section, string key)
        {
            StringBuilder temp = new StringBuilder(2550);

            int i = GetPrivateProfileString(section, key, "", temp, 2550, Application.StartupPath + @"\ini\languageText.ini");
            return temp.ToString();
        }

        public static ArrayList ReadKeys(string sectionName)
        {

            byte[] buffer = new byte[5120];
            int rel = GetPrivateProfileString(sectionName, null, "", buffer, buffer.GetUpperBound(0), Application.StartupPath + @"\ini\languageText.ini");

            int iCnt, iPos;
            ArrayList arrayList = new ArrayList();
            string tmp;
            if (rel > 0)
            {
                iCnt = 0; iPos = 0;
                for (iCnt = 0; iCnt < rel; iCnt++)
                {
                    if (buffer[iCnt] == 0x00)
                    {
                        tmp = System.Text.ASCIIEncoding.Default.GetString(buffer, iPos, iCnt - iPos).Trim();
                        iPos = iCnt + 1;
                        if (tmp != "")
                            arrayList.Add(tmp);
                    }
                }
            }
            return arrayList;
        }


        #endregion
        /// <summary>
        /// 往节点写入信息  
        /// </summary>
        public void IniWriteValues(string section, string[] Keys)
        {
            if (Keys == null) return;
            int intI = 0;
            foreach (string strTmp in Keys)
            {
                if (strTmp != "")
                {
                    WritePrivateProfileString(section, String.Format("{0:X5} ", intI), strTmp, Path);
                }
                intI++;
            }
        }


        /**/
        /// <summary>
        /// 读取INI文件
        /// </summary>
        /// <param name="section">段落</param>
        /// <param name="key">键</param>
        /// <returns>返回的键值</returns>
        public string IniReadValue(string section, string key, string strDefault)
        {
            StringBuilder temp = new StringBuilder(2550);

            int i = GetPrivateProfileString(section, key, "", temp, 2550, Path);
            if (temp.ToString() == null || temp.ToString() == "")
            {
                return strDefault;
            }
            else
            {
                return temp.ToString();
            }
        }

        /**/
        /// <summary>
        /// 读取INI文件
        /// </summary>
        /// <param name="Section">段，格式[]</param>
        /// <param name="Key">键</param>
        /// <returns>返回byte类型的section组或键值组</returns>
        public byte[] IniReadValues(string section, string key)
        {
            byte[] temp = new byte[255];

            int i = GetPrivateProfileString(section, key, "", temp, 255, Path);
            return temp;

        }

        /// <summary>
        /// 读取INI文件
        /// </summary>
        /// <param name="Section">段，格式[]</param>
        /// <param name="Key">键</param>
        /// <returns>返回string类型的section组或键值组</returns>
        public string[] IniReadValuesStr(string section, string key)
        {
            byte[] temp = new byte[20480];

            int i = GetPrivateProfileString(section, key, "", temp, 20480, Path);

            ASCIIEncoding ascii = new ASCIIEncoding();

            string strTmp = ascii.GetString(temp);
            string[] sectionList = strTmp.Split(new char[1] { '\0' });

            return sectionList;
        }

        /// <summary>
        /// 读取INI文件
        /// </summary>
        /// <param name="Section">段，格式[]</param>
        /// <param name="Key">键</param>
        /// <param name="value">值</param>
        /// <returns>返回string类型的value组或键值组</returns>
        /// 
        public string[] IniReadValuesInALlSectionStr(string section)
        {
            string[] sectionList = IniReadValuesStr(section, null);

            string strTmp = null;

            foreach (string str in sectionList)
            {
                if (str != "")
                {
                    strTmp += IniReadValue(section, str, "") + '\0';
                }
                else
                {
                    break;
                }
            }

            if (strTmp != null)
            {
                strTmp = strTmp.Substring(0, strTmp.Length - 1);
                string[] values = strTmp.Split(new char[1] { '\0' });
                return values;
            }
            else return null;
        }

        public string[] IniReadAllSections()
        {
            string[] sectionList = IniReadValuesStr(null, null);
            List<string> strs = new List<string>();
            foreach (string str in sectionList)
            {
                if (str != "" && str != null)
                {
                    strs.Add(str);
                }
            }
            return strs.ToArray();
        }

        /*
         * 下面看一下具体实例化IniFile类的操作：

         //path为ini文件的物理路径

         IniFile ini = new IniFile(path);

         //读取ini文件的所有段落名

         byte[] allSection = ini.IniReadValues(null, null);

         

         通过如下方式转换byte[]类型为string[]数组类型

         string[] sectionList;

         ASCIIEncoding ascii = new ASCIIEncoding();

         //获取自定义设置section中的所有key，byte[]类型

         sectionByte = ini.IniReadValues("personal", null);

         //编码所有key的string类型

         sections = ascii.GetString(sectionByte);

         //获取key的数组

         sectionList = sections.Split(new char[1]{'\0'});

         

         //读取ini文件personal段落的所有键名，返回byte[]类型

         byte[] sectionByte = ini.IniReadValues("personal", null);

         

         //读取ini文件evideo段落的MODEL键值

         model = ini.IniReadValue("evideo", "MODEL");

        
         //将值eth0写入ini文件evideo段落的DEVICE键

         ini.IniWriteValue("evideo", "DEVICE", "eth0");

         即：

         [evideo]
         DEVICE = eth0         

         //删除ini文件下personal段落下的所有键

         ini.IniWriteValue("personal", null, null);

         

         //删除ini文件下所有段落

         ini.IniWriteValue(null, null, null);



         */
    }
}
