using System;
using System.Collections.Generic;
using System.Text;
using System.Data.OleDb;
using System.Data;
using System.Xml;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace HDL_Buspro_Setup_Tool
{
    /// <summary>
    /// 执行用户信息和数据库之间的操作
    /// </summary>
    public class DataModule
    {
        /// <summary>
        /// 连接字符串，将数据文件MoneyNote.mdb放在程序目录下
        /// </summary>
        public const string ConString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=";

        /// <summary>

        /// 取所有表名

        /// </summary>

        /// <returns></returns>

        public static List<string> GetTableNameList(string strPath)
        {

            List<string> list = new List<string>();

            OleDbConnection Conn = new OleDbConnection(ConString + strPath);

            try
            {

                if (Conn.State == ConnectionState.Closed)

                    Conn.Open();

                DataTable dt = Conn.GetSchema("Tables");

                foreach (DataRow row in dt.Rows)
                {

                    if (row[3].ToString() == "TABLE")

                        list.Add(row[2].ToString());

                }

                return list;

            }

            catch (Exception e)

            { throw e; }

            finally { if (Conn.State == ConnectionState.Open) Conn.Close(); Conn.Dispose(); }

        }

        /// <summary>
        /// 数据是否存在数据库的简单操作
        /// </summary>
        /// <returns>成功返回true，失败返回false</returns>
        public static bool IsExitstInDatabase(string strsql)
        {
            OleDbCommand cmd = new OleDbCommand();
            //创建一个OleDbConnection对象

            OleDbConnection conn = new OleDbConnection(ConString + CsConst.mstrCurPath);
            try
            {
                //cmd属性赋值
                cmd.Connection = conn;
                cmd.CommandText = strsql;
                if (conn.State == ConnectionState.Closed) conn.Open();

                OleDbDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                if (reader.HasRows)
                {
                    conn.Close();
                    return true;
                }
                else
                {
                    conn.Close();
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                //关闭连接
                conn.Close();
            }
        }

        /// <summary>
        /// delete , update , insert ,add 数据库的简单操作
        /// </summary>
        /// <returns>成功返回true，失败返回false</returns>
        public static bool ExecuteSQLDatabase(string strsql)
        {
            OleDbConnection con = null;
            con = new OleDbConnection(ConString + CsConst.mstrDefaultPath);
            
            try
            {
                //执行删除命令
                string cmdTxt = strsql;
                OleDbCommand cmd = new OleDbCommand(cmdTxt, con);
                if (con.State == ConnectionState.Closed) con.Open();
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return false;
            }
            finally
            {
                //关闭连接
                con.Close();
            }
        }

        /// <summary>
        ///查找相关数据返回一条结果 数据库的简单操作
        /// </summary>
        /// <returns>成功返回true，失败返回false</returns>
        public static OleDbDataReader SearchAResultSQLDB(string strsql)
        {
            OleDbCommand cmd = new OleDbCommand();
            //创建一个OleDbConnection对象
            OleDbConnection conn = null;
            conn = new OleDbConnection(ConString + CsConst.mstrDefaultPath);
            try
            {
                //cmd属性赋值
                cmd.Connection = conn;
                cmd.CommandText = strsql;
                if (conn.State == ConnectionState.Closed) conn.Open();

                OleDbDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                // conn.Close();
                return reader;
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                //conn.Close();
            }
        }

        /// <summary>
        ///查找相关数据返回一条结果 数据库的简单操作
        /// </summary>
        /// <returns>成功返回true，失败返回false</returns>
        public static OleDbDataReader SearchAResultSQLDB(string strsql,string strPath)
        {
            OleDbCommand cmd = new OleDbCommand();
            //创建一个OleDbConnection对象
            OleDbConnection conn = null;
            conn = new OleDbConnection(ConString + strPath);
            try
            {
                //cmd属性赋值
                cmd.Connection = conn;
                cmd.CommandText = strsql;
                if (conn.State == ConnectionState.Closed) conn.Open();

                OleDbDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                // conn.Close();
                return reader;
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                //conn.Close();
            }
        }

        /// <summary>
        ///查找相关数据返回数据集 数据库的简单操作
        /// </summary>
        /// <returns>成功返回true，失败返回false</returns>
        public static DataSet SearchResultsSQLDB(string strsql)
        {
            OleDbCommand cmd = new OleDbCommand();
            //创建一个OleDbConnection对象

            OleDbConnection conn = new OleDbConnection(ConString + CsConst.mstrCurPath);
            try
            {
                //cmd属性赋值
                cmd.Connection = conn;
                cmd.CommandText = strsql;
                if (conn.State == ConnectionState.Closed)  conn.Open();

                OleDbDataAdapter da = new OleDbDataAdapter(cmd);
                DataSet ds = new DataSet();
                try
                {
                    //填充ds
                    da.Fill(ds);
                    // 清除cmd的参数集合 
                    cmd.Parameters.Clear();
                    //返回ds
                    conn.Close();
                    return ds;
                }
                catch
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
            finally
            {
                conn.Close();
            }
        }


        public static void WriteIni()
        {
            // wirte all to ini 
            string strsql = "select * from defDeviceType order by DeviceType";

            OleDbDataReader drPrj = DataModule.SearchAResultSQLDB(strsql);
            if (drPrj != null)
            {
                while (drPrj.Read())
                {
                    int intDevType = drPrj.GetInt32(0);
                    if (intDevType != 0)
                    {
                        CsConst.mstrINIDefault.IniWriteValue("DeviceType" + intDevType.ToString(), "Model", drPrj.GetValue(2).ToString());
                        if (CsConst.iLanguageId == 0)
                            CsConst.mstrINIDefault.IniWriteValue("DeviceType" + intDevType.ToString(), "Description", drPrj.GetValue(4).ToString());
                        else if (CsConst.iLanguageId == 1)
                        {
                            string str = drPrj.GetValue(3).ToString();
                            if (str.Contains("(")) str = str.Split('(')[0].ToString();
                            CsConst.mstrINIDefault.IniWriteValue("DeviceType" + intDevType.ToString(), "Description", str);
                        }
                        CsConst.mstrINIDefault.IniWriteValue("DeviceType" + intDevType.ToString(), "MaxValue", drPrj.GetInt32(5).ToString());
                        CsConst.mstrINIDefault.IniWriteValue("DeviceType" + intDevType.ToString(), "IsOnlist", drPrj.GetBoolean(7).ToString());
                        string strTmp =Convert.ToString(drPrj[8]);
                        string[] ArayTmp = strTmp.Split(';'); 
                        if (strTmp != null)
                        {
                            foreach (string strVersion in ArayTmp)
                                CsConst.mstrINIDefault.IniWriteValue("DeviceType" + intDevType.ToString(), "Version", strTmp);
                        }
                    }
                }
            }
        }


        public static void AddNewTable(string strsql)
        {
            string m_sConnectionString = DataModule.ConString + CsConst.mstrDefaultPath;
            OleDbConnection con = new OleDbConnection(m_sConnectionString);
            con.Open();
            OleDbCommand com = new OleDbCommand(strsql,con);
            com.ExecuteNonQuery();
        }


    }


}
