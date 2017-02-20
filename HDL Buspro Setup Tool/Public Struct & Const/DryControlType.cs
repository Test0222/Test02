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
    /// <summary>
    /// 公共Load按键模式
    /// </summary>
    public class DryControlType
    {
        public Byte ControlTypeSaveID;  // 保存用的ID
        public String ControlTypeName; //中文

        public static void LoadButtonCoontrolTypeFromDatabaseToPublicClass()
        {
            CsConst.myDryGroupControlType = new List<DryControlType>();

            string strsql = string.Format("select * from defGroupMemberType order by ID");
            OleDbDataReader dr = DataModule.SearchAResultSQLDB(strsql, CsConst.mstrDefaultPath);
            
            if (dr != null)
            {
                while (dr.Read())
                {
                    DryControlType TmpButtonMode = new DryControlType();
                    TmpButtonMode.ControlTypeSaveID =(Byte) dr.GetInt32(0);
                    if (CsConst.iLanguageId == 1)
                    {
                        TmpButtonMode.ControlTypeName = dr.GetString(1);
                    }
                    else
                    {
                        TmpButtonMode.ControlTypeName = dr.GetString(2);
                    }
                    CsConst.myDryGroupControlType.Add(TmpButtonMode);
                }
                dr.Close();
            }
        }

        /// <summary>
        /// 将字符串转换成可以保存的ID记录
        /// </summary>
        /// <param name="bytKeyMode"></param>
        /// <param name="DeviceType"></param>
        /// <returns></returns>      
        public static byte ConvertorKeyControlTypeToPublicModeGroup(String TmpButtonMode)
        {
            byte SaveID = 0;
            if (CsConst.myDryGroupControlType == null) return SaveID;

            foreach (DryControlType oTmp in CsConst.myDryGroupControlType)
            {
                if (oTmp.ControlTypeName == TmpButtonMode)
                {
                    SaveID = oTmp.ControlTypeSaveID;
                    break;
                }
            }
            return SaveID;
        }

        /// <summary>
        /// 转换按键模式以便于保存
        /// </summary>
        /// <param name="bytKeyMode"></param>
        /// <param name="DeviceType"></param>
        /// <returns></returns>      
        public static String ConvertorKeyModeToPublicModeGroup(Byte TmpButtonMode)
        {
            if (CsConst.myDryGroupControlType == null) return "";
            string ButtonMode = CsConst.myDryGroupControlType[0].ControlTypeName;

            foreach (DryControlType oTmp in CsConst.myDryGroupControlType)
            {
                if (oTmp.ControlTypeSaveID == TmpButtonMode)
                {
                    ButtonMode = oTmp.ControlTypeName;
                    break;
                }
            }
            return ButtonMode;
        }
    }
}
