using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data.OleDb;
using System.Windows.Forms;

namespace HDL_Buspro_Setup_Tool
{
    public class ControlTemplates
    {
        public int ID;
        public string Name;
        public byte bytType; //普通变量    全局变量  
        public byte bytGpID; // 互斥 分几组 组号
        public List<UVCMD.ControlTargets> GpCMD = null;
        public Boolean isSelected;

        public static void ShowGroupCommandsToListview(DataGridView oDG, byte bytType)
        {
            if (CsConst.myTemplates == null || CsConst.myTemplates.Count == 0) return;

            foreach (ControlTemplates oTmp in CsConst.myTemplates)
            {
                if (bytType == oTmp.bytType || bytType == 0)
                {
                    string strTmp = CsConst.mstrInvalid;
                    if (oTmp.bytGpID != 0) strTmp = oTmp.bytGpID.ToString();

                    object[] obj = { oTmp.ID, oTmp.Name, strTmp };
                    oDG.Rows.Add(obj);
                }
            }
        }

        public static void ShowGroupCommandsToTreetview(ListView oTv1)
        {
            if (CsConst.myTemplates == null || CsConst.myTemplates.Count == 0) return;
            oTv1.Items.Clear();
            foreach (ControlTemplates oTmp in CsConst.myTemplates)
            {
                if (oTmp != null)
                {
                    ListViewItem oLv = new ListViewItem();
                    oLv.Text = oTmp.ID.ToString();
                    oLv.SubItems.Add(oTmp.Name);
                    oTv1.Items.Add(oLv);
                }
            }
        }

        public static int GetNewIDForGroup()
        {
            // 添加到缓存
            if (CsConst.myTemplates == null)
            {
                CsConst.myTemplates = new List<ControlTemplates>();
                return 1;
            }

            List<int> oTmp = new List<int>(); //取出所有已用的场景号
            for (int i = 0; i < CsConst.myTemplates.Count; i++)
            {
                oTmp.Add(CsConst.myTemplates[i].ID);
            }
            //查找位置，替换buffer
            int intAreaID = 1;
            while (oTmp.Contains(intAreaID))
            {
                intAreaID++;
            }

            return intAreaID;
        }

        public static void ReadAllGroupCommandsFrmDatabase()
        {
            CsConst.myTemplates = new List<ControlTemplates>();

            ////read keys commands to buffer
            string str = string.Format("select * from dbGrpRemark order by GrpID");
            OleDbDataReader drKeyCmds = DataModule.SearchAResultSQLDB(str);

            if (drKeyCmds != null)
            {
                while (drKeyCmds.Read())
                {
                    ControlTemplates oTmpGrp = new ControlTemplates();
                    oTmpGrp.ID = drKeyCmds.GetInt16(0);
                    oTmpGrp.Name = drKeyCmds.GetString(1);
                    oTmpGrp.bytType = drKeyCmds.GetByte(2);
                    oTmpGrp.bytGpID = drKeyCmds.GetByte(3);
                    oTmpGrp.GpCMD = new List<UVCMD.ControlTargets>();

                    str = string.Format("select * from dbGrpCMD where GrpID = {0} order by objID", oTmpGrp.ID);

                    OleDbDataReader drKeyCmds1 = DataModule.SearchAResultSQLDB(str);

                    if (drKeyCmds1 != null)
                    {
                        while (drKeyCmds1.Read())
                        {
                            UVCMD.ControlTargets TmpCmd = new UVCMD.ControlTargets();
                            TmpCmd.ID = drKeyCmds1.GetByte(1);
                            TmpCmd.Type = drKeyCmds1.GetByte(2);
                            TmpCmd.SubnetID = drKeyCmds1.GetByte(3);
                            TmpCmd.DeviceID = drKeyCmds1.GetByte(4);
                            TmpCmd.Param1 = drKeyCmds1.GetByte(5);
                            TmpCmd.Param2 = drKeyCmds1.GetByte(6);
                            TmpCmd.Param3 = drKeyCmds1.GetByte(7);
                            TmpCmd.Param4 = drKeyCmds1.GetByte(8);

                            oTmpGrp.GpCMD.Add(TmpCmd);
                        }
                        drKeyCmds1.Close();
                    }
                    CsConst.myTemplates.Add(oTmpGrp);
                }
                drKeyCmds.Close();
            }
        }

        public static void SaveAllGroupCommandsFrmDatabase()
        {
            string str = string.Format("delete * from dbGrpRemark");
            DataModule.ExecuteSQLDatabase(str);

            str = string.Format("delete * from dbGrpCMD");
            DataModule.ExecuteSQLDatabase(str);

            if (CsConst.myTemplates == null) return;

            ////read keys commands to buffer
            foreach (ControlTemplates oTmp in CsConst.myTemplates)
            {
                if (oTmp.GpCMD != null)
                {
                    string strsql = string.Format("Insert into dbGrpRemark(GrpID,Remark,bytType,bytGpID) values ({0},'{1}',{2},{3})", oTmp.ID, oTmp.Name, oTmp.bytType, oTmp.bytGpID);
                    DataModule.ExecuteSQLDatabase(strsql);
                    for (int i = 0; i < oTmp.GpCMD.Count; i++)
                    {
                        UVCMD.ControlTargets TmpCmds = oTmp.GpCMD[i];
                        ///// insert into all commands to database
                        strsql = string.Format("Insert into dbGrpCMD(GrpID,objID,KeyFunType,SubNetID,DeviceID,FirstParameter,"
                                       + "SecondParameter,RunTimeMinute,RunTimeSecond,strHint,Default1) values ({0},{1},{2},{3},{4},{5},{6},{7},{8},'{9}',{10})",
                                       oTmp.ID, TmpCmds.ID, TmpCmds.Type, TmpCmds.SubnetID, TmpCmds.DeviceID, TmpCmds.Param1,
                                       TmpCmds.Param2, TmpCmds.Param3, TmpCmds.Param4, TmpCmds.Hint, oTmp.bytType);
                        DataModule.ExecuteSQLDatabase(strsql);
                    }
                }
            }
        }

        public static ControlTemplates AddNewTemplateToPublicGroup(String TemplateName, List<UVCMD.ControlTargets> TemplatesCMD)
        {
            if (CsConst.myTemplates == null) CsConst.myTemplates = new List<ControlTemplates>();

            //添加到结构体
            #region
            ControlTemplates oTmp = new ControlTemplates();
            oTmp.ID = GetNewIDForGroup();
            oTmp.Name = TemplateName;
            oTmp.bytType = 1;
            oTmp.bytGpID = 0;
            oTmp.GpCMD = new List<UVCMD.ControlTargets>();

            for (int i = 0; i < TemplatesCMD.Count; i++)
            {
                UVCMD.ControlTargets oCtrls = (UVCMD.ControlTargets)TemplatesCMD[i].Clone();
                oTmp.GpCMD.Add(oCtrls);
            }
            CsConst.myTemplates.Add(oTmp);
            #endregion
            return oTmp;
        }
    }

}
