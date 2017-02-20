using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.OleDb;


[Serializable]
public class BugList
{
    public String BugInformation;//描述
    public int Status;  //状态 0 ： 未解决； 1： 解决
    public int ReadOrUnRead; // 0 : 未读；1： 已读 
    public String Finder; // 提交人
    public String Time; // 提交时间
}

[Serializable]
public class VersionInformation
{
    public String sModel;
    public String sVersion;//产品
    public String sProduction;//产品
    public String sFirmwareName; //升级文件名称
    public String sSerialId; // 升级的编号
    public String Description;  //版本号
    public Byte bProcess; // 进度条
    public List<BugList> currentBugLists;//详细描述//获取到的时间闭
}

class PublicMethods
{
    public static DataTable LoadDataFromExcel(string Path)
    {
        DataTable dt = new DataTable();
        string strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Path + "; Extended Properties=Excel 8.0;";

        OleDbConnection conn = new OleDbConnection(strConn);
        conn.Open(); string strExcel = "";
        OleDbDataAdapter myCommand = null;

        strExcel = "select * from [sheet1$]";
        myCommand = new OleDbDataAdapter(strExcel, strConn);
        dt = new DataTable();
        myCommand.Fill(dt);
        return dt;
    }

    public static List<VersionInformation> LoadDataFromExcelToStruct(string Path)
    {
        List<VersionInformation> ReadResults = new List<VersionInformation>();

        return ReadResults;
    }
}
