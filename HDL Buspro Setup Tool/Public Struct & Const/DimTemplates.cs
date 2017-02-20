using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data.OleDb;
using System.Windows.Forms;
using System.Runtime.Serialization.Formatters.Binary;

namespace HDL_Buspro_Setup_Tool
{
    [Serializable]
    public class DimTemplate
    {
        public int iID;
        public string sName;
        public Int16[] arrChannelLevel = new Int16[100]; // 每个值对应的level
    }

    [Serializable]
    public class DimTemplates
    {
        public String sProjectName;
        public List<DimTemplate> arrDimProfiles = null;

        public static int GetNewIDForGroup(String sProjectName)
        {
            // 添加到缓存
            if (CsConst.myDimTemplates == null)
            {
                CsConst.myDimTemplates = new List<DimTemplates>();
                return 1;
            }

            DimTemplates dimSelectedCurve = new DimTemplates();
            foreach (DimTemplates oTmp in CsConst.myDimTemplates)
            {
                if (oTmp.sProjectName == sProjectName)
                {
                    dimSelectedCurve = oTmp; break;
                }
            }

            if (dimSelectedCurve.arrDimProfiles == null || dimSelectedCurve.arrDimProfiles.Count == 0) return 1;
            if (dimSelectedCurve.arrDimProfiles.Count == 4) return -1;
            //查找位置，替换buffer
            int intAreaID = 1;
            try
            {
                List<int> oTmpIdList = new List<int>(); //取出所有已用的场景号
                for (int i = 0; i < dimSelectedCurve.arrDimProfiles.Count; i++)
                {
                    oTmpIdList.Add(dimSelectedCurve.arrDimProfiles[i].iID);
                }
                
                while (oTmpIdList.Contains(intAreaID))
                {
                    intAreaID++;
                }
            }
            catch
            { }

            return intAreaID;
        }

        public static void ShowGroupCommandsToTreetview(TreeView oTv1)
        {
            if (CsConst.myDimTemplates == null || CsConst.myDimTemplates.Count == 0) return;
            oTv1.Nodes.Clear();
            try
            {
                foreach (DimTemplates oTmp in CsConst.myDimTemplates)
                {
                    if (oTmp != null)
                    {
                        TreeNode oNode = oTv1.Nodes.Add(oTmp.sProjectName);

                        foreach (DimTemplate tmpDimProfile in oTmp.arrDimProfiles)
                        {
                            oNode.Nodes.Add(tmpDimProfile.sName);
                        }
                    }
                }
            }
            catch { }
        }

        public static void ReadAllGroupCommandsFrmDatabase()
        {
            CsConst.myDimTemplates = new List<DimTemplates>();
            try
            {
                ////read keys commands to buffer
                String sDatPath = CsConst.mstrDefaultPath = Application.StartupPath + @"\Dat\DimTemplates.dat";
                if (File.Exists(sDatPath) == true)
                {
                    FileStream fs = new FileStream(sDatPath, FileMode.Open);
                    BinaryFormatter bf = new BinaryFormatter();
                    CsConst.myDimTemplates = bf.Deserialize(fs) as List<DimTemplates>;
                    fs.Close();
                }
            }
            catch { }
        }

        public static void SaveAllGroupCommandsFrmDatabase()
        {
            try
            {
                String sDatPath = CsConst.mstrDefaultPath = Application.StartupPath + @"\Dat\DimTemplates.dat";

                if (CsConst.myDimTemplates != null && CsConst.myDimTemplates.Count != 0)
                {
                    FileStream fs = new FileStream(sDatPath, FileMode.Create);
                    BinaryFormatter bf = new BinaryFormatter();
                    if (CsConst.myDimTemplates != null) bf.Serialize(fs, CsConst.myDimTemplates);
                    fs.Close();
                }
            }
            catch
            { }
        }

        public static DimTemplates AddNewTemplateToPublicGroup(String TemplateName, List<DimTemplates> TemplatesCMD)
        {
            DimTemplates oTmp = new DimTemplates();
            try
            {
                if (CsConst.myDimTemplates == null) CsConst.myDimTemplates = new List<DimTemplates>();

                //添加到结构体
                #region
                oTmp.sProjectName = CsConst.MyUnnamed;
                oTmp.arrDimProfiles = new List<DimTemplate>();

                DimTemplate dimTmp = new DimTemplate();
                dimTmp.iID = 1;
                dimTmp.sName = "Dim Cuver " + dimTmp.iID.ToString();
                dimTmp.arrChannelLevel = new Int16[100];
                for (Byte i = 0; i < 100; i++)
                {
                    dimTmp.arrChannelLevel[i] = i;
                }
                oTmp.arrDimProfiles.Add(dimTmp);
                #endregion

                CsConst.myDimTemplates.Add(oTmp);
            }
            catch { }
            return oTmp;
        }
    }

}
